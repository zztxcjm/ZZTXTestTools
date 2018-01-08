using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CombatResource
{
    class Program
    {
        public static int Main(string[] args)
        {
            if (args == null || args.Length == 0)
                return 1;


            var configFile = args[0];
            return ProcessConfig(configFile);

        }

        private static string _currentFolder = System.Environment.CurrentDirectory;
        private static string _contextFolder = _currentFolder;

        private static int ProcessConfig(string configFile)
        {

            if (string.IsNullOrEmpty(configFile))
                return 2;

            //验证一下文件是否存在
            if (configFile.IndexOf(":") < 0)
            {
                configFile = Path.Combine(_currentFolder, configFile);
            }
            else
            {
                _currentFolder = configFile.Substring(0, configFile.LastIndexOf("\\"));
            }
            if (!File.Exists(configFile))
            {
                return 3;
            }


            //处理配置
            var lines = File.ReadAllLines(configFile);
            if (lines.Length > 0)
            {
                foreach (string line in lines)
                {
                    if (!String.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
                    {
                        try
                        {
                            ProcessConfigLine(line);

                            Console.WriteLine(line);
                            Console.WriteLine("OK");

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(line);
                            Console.WriteLine("=================================================================");
                            Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
                            Console.WriteLine("=================================================================");
                        }
                    }
                }
            }


            return 0;

        }

        private static int ProcessConfigLine(string line)
        {

            //#去到一个路径
            //GO resources/js

            //#最简单的文件合并
            //COMBAT a.js b.js c.js -> d.js
            //COMBAT x/a.js y/b.js z/c.js -> d.js

            //#通配规则
            //COMBAT *.js->all.js
            //COMBAT *.css->all.css
            //COMBAT *.*->all.js
            //COMBAT_BY_FOLDER *.js->{FolderName}.js
            //COMBAT_BY_FOLDER *.css->{FolderName}.js
            //COMBAT_BY_FOLDER *.*->{FolderName}.js

            if (line.StartsWith("GO "))
            {

                line = line.Replace("GO ", String.Empty);

                var _currentFolder_cpy = _currentFolder;
                var tmp = line.Split('/');
                foreach (string item in tmp)
                {
                    if (item == "..")
                    {
                        _currentFolder_cpy = _currentFolder_cpy.Substring(0, _currentFolder_cpy.LastIndexOf("\\"));
                    }
                    else
                    {
                        break;
                    }
                }

                _contextFolder = Path.Combine(_currentFolder_cpy, line.Replace("../", String.Empty).Replace("/", "\\"));

                return 0;

            }
            else if (line.StartsWith("COMBAT "))
            {  
                return ProcessCombat(line.Replace("COMBAT ", String.Empty));
            }
            else if (line.StartsWith("COMBAT_BY_FOLDER "))
            {
                return ProcessCombatByFolder(line.Replace("COMBAT_BY_FOLDER ", String.Empty));
            }

            return 0;

        }

        private static int ProcessCombat(string line)
        {

            var i = line.IndexOf("->");
            var src = line.Substring(0, i);
            var tar = line.Substring(i + 2);

            if (String.IsNullOrWhiteSpace(src)
                || String.IsNullOrWhiteSpace(tar))
                return 0;

            src = src.Trim();
            tar = tar.Trim();

            //处理源地址
            var src_tmp = src.Split(new char[] { ' ', '+' }, StringSplitOptions.RemoveEmptyEntries);
            if (src_tmp.Length == 0)
            {
                return 0;
            }

            List<string> srcList = new List<string>();
            foreach (var oldsrc in src_tmp)
            {
                if (oldsrc.IndexOf("*") >= 0)
                {
                    var files = Directory.GetFiles(_contextFolder, oldsrc, SearchOption.TopDirectoryOnly);
                    srcList.AddRange(files);
                }
                else
                {
                    srcList.Add(Path.Combine(_contextFolder, oldsrc.Replace("/", "\\")));
                }
            }

            //处理目标地址
            var newtar = Path.Combine(_contextFolder, tar.Replace("/", "\\"));
            if (File.Exists(newtar))
                File.Delete(newtar);

            //写文件
            if (srcList.Count > 0)
            {
                foreach (string item in srcList.Distinct())
                {
                    if (item != newtar)
                    {
                        if (File.Exists(item))
                        {
                            File.AppendAllText(newtar, Environment.NewLine + "/*" + item + "*/" + Environment.NewLine + File.ReadAllText(item));
                        }
                    }
                }

            }

            return 0;

        }

        private static int ProcessCombatByFolder(string line)
        {

            //看当前文件夹中是否有目录
            var dirs = Directory.GetDirectories(_contextFolder, "*", SearchOption.TopDirectoryOnly);
            if (dirs.Count() == 0)
                return 0;

            //处理目标和模式
            var i = line.IndexOf("->");
            var pattern = line.Substring(0, i);
            var tarFile = line.Substring(i + 2);

            if (String.IsNullOrWhiteSpace(pattern)
                || String.IsNullOrWhiteSpace(tarFile))
                return 0;

            pattern = pattern.Trim();
            tarFile = tarFile.Trim();


            foreach (var d in dirs)
            {

                //处理目标文件
                var dName = d.Substring(d.LastIndexOf("\\") + 1);
                var fName = tarFile.Replace("{FolderName}", dName);
                var destFile = Path.Combine(d, fName);
                if (File.Exists(destFile))
                {
                    File.Delete(destFile);
                }

                //处理源文件
                var files = Directory.GetFiles(d, pattern, SearchOption.TopDirectoryOnly);
                foreach (var f in files)
                {
                    if (f != destFile)
                    {
                        if (File.Exists(f))
                        {
                            File.AppendAllText(destFile, Environment.NewLine + "/*" + f + "*/" + Environment.NewLine + File.ReadAllText(f));
                        }
                    }
                }

            }


            return 0;


        }

    }

}
