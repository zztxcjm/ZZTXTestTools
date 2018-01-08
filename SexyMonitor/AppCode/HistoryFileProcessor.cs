using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SexyMonitor
{
    public class HistoryFileProcessor
    {

        private HistoryFileProcessor() { }
        private static readonly HistoryFileProcessor _instance = new HistoryFileProcessor();

        public static HistoryFileProcessor GetInstance()
        {
            return _instance;
        }

        public void Run()
        {


            //往数据库添加任务时，如果指纹已存在则不重复添加，非图片类型不添加，分辨率小于250*250的不添加

            //查找文件流程
            //从开始目录走，进入目录，首先查找目录下的索引文件，如果索引文件不存在就处理当前目录下的所有文件,并自动创建索引文件
            //如果索引文件存在就加载索引文件，处理文件时跳过已在索引中存在的文件，如在索引文件中不存在则处理并追加到索引文件末尾
            //处理历史文件时优先处理一个限定时间之前的（2月17号之前的）

            //当前目录下的文件处理完成后开始逐级处理子文件夹中的文件，处理逻辑同上


            var _monitorDirs = System.Configuration.ConfigurationManager.AppSettings["MonitorDirs"];
            if (String.IsNullOrEmpty(_monitorDirs))
                return;

            var rootFolders = _monitorDirs.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            try
            {
                foreach (var rf in rootFolders)
                {
                    new FileProcessor(rf).Run().Wait();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("HistoryFileProcessor Error." + ex.Message + ex.StackTrace);
            }

        }

    }
}
