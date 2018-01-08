using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SexyMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            //ExecModule(new string[] { "hisdata" });
            ExecModule(args);
        }

        static void ExecModule(string[] args)
        {
            string cmd = "all";
            string[] cmdArr;

            if (args != null && args.Length > 0)
            {
                cmd = args[0].ToLower();
                cmdArr = cmd.Split(new char[] { '|', ',', ';' });
            }
            else
            {
                cmdArr = new string[] { };
            }

            //启动新文件监视器，将监视文件的变化，新增，修改，删除等
            if (cmd == "all" || cmdArr.Contains<string>("nf"))
            {
                Console.WriteLine("Load NewFileProcessor");
                NewFileProcessor.GetInstance().Run();
            }

            //启动任务监视器
            if (cmd == "all" || cmdArr.Contains<string>("tm"))
            {
                Console.WriteLine("Load TaskMonitor");
                TaskMonitor.GetInstance().Run();
            }

            //将历史文件转化问处理任务
            if (cmdArr.Contains<string>("hisdata"))
            {
                Console.WriteLine("Load HistoryFileProcessor");
                HistoryFileProcessor.GetInstance().Run();
            }

            //讲CorpId为空的数据更新为有数据
            if (cmdArr.Contains<string>("upcorpid"))
            {
                FileTask.UpdateCorpId();
            }

            //Console.WriteLine("Run...");
            Console.ReadLine();

        }


    }
}
