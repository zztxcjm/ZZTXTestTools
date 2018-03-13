using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReplace
{
    class Program
    {
        static void Main(string[] args)
        {

            //FileCopy replace srcFile targetPath needConfirm 
            //FileCopy copy srcFile targetPath needConfirm 
            //FileCopy copy srcPath targetPath needConfirm 


            if (args == null)
                return;
            if (args.Length == 0)
                return;

            if (args.Length == 1)
            {
                Console.WriteLine("请输入要替换的目标路径");
                Console.ReadLine();
            }
            if (args.Length == 2)
            {
                StartReplaceFile(args[0], args[1]);
                Console.ReadLine();
            }
            if (args.Length == 3)
            {
                StartReplaceFile(args[0], args[1], args[2]);
                Console.ReadLine();
            }

        }

        private static void StartReplaceFile(string srcfile, string targetpath, string confirm = "auto")
        {

            srcfile = srcfile.Replace("/", "\\");
            if (srcfile.IndexOf(":") == -1)
            {
                srcfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, srcfile);
            }

            if (!File.Exists(srcfile))
            {
                Console.WriteLine($"原文件{srcfile}不存在");
                return;
            }

            if(!Directory.Exists(targetpath))
            {
                Console.WriteLine($"目标路径{targetpath}不存在");
                return;
            }

            var srcFileinfo = new FileInfo(srcfile);
            var targetFiles = FindTargetFile(targetpath, srcFileinfo.Name);
            if (targetFiles!=null && targetFiles.Count > 0)
            {

                Console.Write($"共找到{targetFiles.Count}个需要替换的文件。");

                confirm = confirm.ToLower();
                if (confirm != "auto" && confirm != "no")
                {
                    if (!YesorNo())
                    {
                        Console.WriteLine($"无需替换已跳过{srcfile}");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("");
                }

                int success = 0;
                int failed = 0;
                foreach (string f in targetFiles)
                {
                    bool isOK = false;
                    int count = 0;
                    while (!isOK && count < 20)
                    {
                        try
                        {
                            File.Copy(srcfile, f, true);
                            isOK = true;
                            success++;
                        }
                        catch
                        {
                            isOK = false;
                            count++;
                            failed++;
                            System.Threading.Thread.Sleep(1000);
                        }

                    }
                    Console.Write($"替换文件{f}");
                    Console.WriteLine((isOK ? "成功" : "失败"));
                }
                Console.WriteLine($"成功替换{success}个,失败{failed}个");


            }
            else
            {
                Console.WriteLine($"无需替换已跳过{srcfile}");
            }

        }

        private static bool YesorNo()
        {
            Console.Write("请确认是否替换[yes] or [no]？ ");
            var cmd = Console.ReadLine();
            if (cmd.Length > 0)
            {
                if (cmd.ToLower() == "yes")
                    return true;
                if (cmd.ToLower() == "no")
                    return false;

                return YesorNo();

            }
            else
            {
                return YesorNo();
            }
        }

        private static List<string> FindTargetFile(string targetpath,string fileName)
        {
            var files = Directory.GetFiles(targetpath, fileName, SearchOption.AllDirectories);
            if (files == null || files.Length == 0)
                return null;
            else
                return files.ToList();
        }
    }
}
