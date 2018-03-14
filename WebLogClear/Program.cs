using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLogClear
{
    class Program
    {

        static int MAX_DAYS = 7;
        static bool AUTO_OVER = false;


        static void Main(string[] args)
        {

            if (args != null && args.Length > 0)
            {
                if(args[0].ToLower()=="auto")
                {
                    AUTO_OVER = true;
                }
            }

            try
            {
                Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            if (!AUTO_OVER)
            {
                Console.Read();
            }
        }

        private static void Start()
        {
            var target = System.Configuration.ConfigurationManager.AppSettings["Target"];
            var str_MaxDays = System.Configuration.ConfigurationManager.AppSettings["MaxDays"];
            if (!String.IsNullOrEmpty(str_MaxDays))
            {
                try
                {
                    MAX_DAYS = Convert.ToInt32(str_MaxDays);
                }
                catch (Exception)
                {
                    MAX_DAYS = 7;
                }
            }

            if (!String.IsNullOrEmpty(target))
            {
                //进行目标路径处理
                var arrTarget = target.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (arrTarget.Length > 0)
                {
                    foreach (string item in arrTarget)
                    {

                        var path = item;
                        if (path.EndsWith("*"))
                        {

                            var path2 = path.Substring(0, path.LastIndexOf("\\"));

                            var path3 = path.Replace(path2, string.Empty).Replace("*", "\\");
                            string[] path3_arr = null;
                            if (path3.IndexOf(",") != -1)
                            {
                                path3_arr = path3.Replace("\\", "").Replace("*", "").ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            }
                            else
                            {
                                path3_arr = new string[] { path3.Replace("\\", "").Replace("*", "").ToLower() };
                            }

                            try
                            {
                                ExecDelete(path2, path3_arr, "*.*");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                Console.WriteLine(ex.StackTrace);
                            }

                        }
                        else
                        {
                            try
                            {
                                ExecDelete(path, null, "*.*");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                Console.WriteLine(ex.StackTrace);
                            }
                        }
                    }

                }

            }
        }

        private static void ExecDelete(string root, string[] subpath, string filepattern)
        {
            //Console.Write(root);
            //Console.Write(",");
            //if (subpath != null)
            //{
            //    Console.Write(string.Join(",", subpath));
            //}
            //else
            //{
            //    Console.Write("null");
            //}
            //Console.Write(",");
            //Console.WriteLine(filepattern);

            if (Directory.Exists(root))
            {

                Console.WriteLine("开始处理路径"+ root);

                if (subpath == null || subpath.Length == 0)
                {
                    //直接在root下按filepattern模式进行匹配
                    var files = Directory.EnumerateFiles(root, filepattern, SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        DeleteFile(file);
                    }
                }
                else
                {
                    //先找目录，在目录下进行模式匹配
                    var dirs = Directory.EnumerateDirectories(root, "*", SearchOption.AllDirectories);
                    foreach (var d in dirs)
                    {
                        var folder = d.Substring(d.LastIndexOf('\\') + 1);
                        if (subpath.Contains(folder.ToLower()))
                        {
                            var files = Directory.EnumerateFiles(d, filepattern, SearchOption.AllDirectories);
                            foreach (var file in files)
                            {
                                DeleteFile(file);
                            }
                        }
                    }
                }

                Console.WriteLine("完成");

            }

        }

        private static void DeleteFile(string file)
        {
            Console.Write("Delete File " + file);
            Console.Write("  ");
            try
            {
                var fi = new FileInfo(file);
                if (fi.Exists && (DateTime.Now - fi.CreationTime).TotalDays >= MAX_DAYS)
                {
                    fi.Delete();
                    Console.WriteLine("OK");
                }
                else
                {
                    Console.WriteLine("Skiped");
                }

            }
            catch (Exception ex)
            {
                Console.Write("Failed  ");
                Console.WriteLine(ex.Message);
            }

        }

    }
}
