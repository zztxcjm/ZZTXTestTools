using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SexyMonitor
{
    public class NewFileProcessor
    {
        private NewFileProcessor() { }
        private static readonly NewFileProcessor _instance = new NewFileProcessor();

        public static NewFileProcessor GetInstance()
        {
            return _instance;
        }

        #region FileExt

        private string _FileExt = System.Configuration.ConfigurationManager.AppSettings["MonitorFileExt"];
        private string _getFileExt(string filePath)
        {
            if (String.IsNullOrEmpty(filePath))
                return String.Empty;
            var tmp = filePath.Trim().Split('.');
            if (tmp.Length == 0)
                return String.Empty;

            return tmp[tmp.Length - 1];

        }
        private bool _fileExtIsOk(string filePath)
        {
            var ext = _getFileExt(filePath);
            return !String.IsNullOrEmpty(ext) && _FileExt.IndexOf(ext) >= 0;
        }
        #endregion

        public void Run()
        {

            var _monitorDirs = System.Configuration.ConfigurationManager.AppSettings["MonitorDirs"];
            if (String.IsNullOrEmpty(_monitorDirs))
                return;

            try
            {
                var dirs = _monitorDirs.Trim().Split(new char[] { ';', '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var d in dirs)
                {

                    var fw = new System.IO.FileSystemWatcher(d);
                    fw.IncludeSubdirectories = true;
                    fw.Created += Fw_Created;
                    //fw.Deleted += Fw_Deleted; //删除文件时无法关联删除任务，因为无法获取MD5指纹
                    fw.EnableRaisingEvents = true;
                }
            }
            catch (Exception ex)
            {
                FaceHand.Common.Util.SystemLoger.Current.Write(ex);
            }


        }

        public void Stop()
        {
        }

        private void Fw_Created(object sender, System.IO.FileSystemEventArgs e)
        {
            if (_fileExtIsOk(e.FullPath))
            {

                //休眠的目的是为了让刚上传的文件不产生争用
                System.Threading.Thread.Sleep(3000);

                //交文件处理模块转为任务
                FileTask.CreateTaskAsync(e.FullPath);

            }
        }

    }
}
