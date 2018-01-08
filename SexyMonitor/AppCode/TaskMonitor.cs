using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SexyMonitor
{
    public class TaskMonitor
    {

        private TaskMonitor() { }
        private static readonly TaskMonitor _instance = new TaskMonitor();

        public static TaskMonitor GetInstance()
        {
            return _instance;
        }

        public void Run()
        {

            var str_MonitorInterval = System.Configuration.ConfigurationManager.AppSettings["MonitorInterval"];
            var int_MonitorInterval = String.IsNullOrEmpty(str_MonitorInterval) ? 30 : Convert.ToInt32(str_MonitorInterval);

            ThreadPool.QueueUserWorkItem((object state) =>
            {

                while (true)
                {

                    try
                    {
                        //腾讯云限制最大只能20个
                        var tasks = FileTask.GetFileTaskWaitPorcessing(20);
                        if (tasks != null && tasks.Count() > 0)
                        {

                            SexyFilter.Process(tasks);

                            //更新任务状态，防止重复查询
                            var taskIds = from item in tasks select item.Id;
                            FileTask.UpdateTaskProcessState(taskIds);

                            //记录日志
                            if (LogState.Enabled)
                            {
                                string log = String.Format("{0} {1} {2}", DateTime.Now, tasks.Count(), String.Join(",", taskIds));
                                FaceHand.Common.Core.WxLogProvider.Write(log, "TaskMonitor_Run");
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        FaceHand.Common.Util.SystemLoger.Current.Write(ex);
                    }

                    //定时检测频率
                    Thread.Sleep(int_MonitorInterval * 1000);

                }

            });



        }

    }

}
