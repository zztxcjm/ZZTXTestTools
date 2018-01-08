using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SexyMonitor
{
    class FileProcessor : IDisposable
    {
        private FileIndex _fi = null;
        private string[] _fileExtArrays = null;
        private string _currentDirName = null;

        public FileProcessor(string dirName)
        {

            if (String.IsNullOrEmpty(dirName))
                throw new Exception("待处理文件目录为空");
            if(!System.IO.Directory.Exists(dirName))
                throw new Exception("目录不存在("+ dirName + ")");


            var _fileExts = System.Configuration.ConfigurationManager.AppSettings["MonitorFileExt"];
            if (String.IsNullOrEmpty(_fileExts))
                _fileExts = ".jpg;.png;.gif;.bmp";

            _fileExtArrays = _fileExts.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            _currentDirName = dirName;
            _fi = new FileIndex(dirName);


        }

        private string GetFileExtName(string fn)
        {
            var tmp = fn.Split('.');
            return tmp[tmp.Length - 1];
        }

        private bool ValidateFileExtName(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                return false;

            foreach(var ext in _fileExtArrays)
            {
                if (fileName.EndsWith(ext))
                    return true;
            }

            return false;

        }

        public Task Run()
        {
            return Task.Factory.StartNew(RunThread);
        }

        private void RunThread()
        {
            //加载所有文件
            var files = System.IO.Directory.EnumerateFiles(_currentDirName);
            var fileReplacePattern = !_currentDirName.EndsWith("\\") ? _currentDirName + "\\" : _currentDirName;
            foreach (var fileFullPath in files)
            {
                try
                {
                    if (ValidateFileExtName(fileFullPath))
                    {

                        var shortFileName = fileFullPath.Replace(fileReplacePattern, String.Empty);

                        if (!_fi.Exist(shortFileName))
                        {
                            //创建任务
                            FileTask.CreateTask(fileFullPath);
                            Console.WriteLine("CHF "+ fileFullPath);
                            //Console.WriteLine("FileTask.CreateTask("+ fileFullPath + ")");

                            //添加到索引表示文件已处理过
                            _fi.AddFile(shortFileName);

                        }
                        else
                        {
                            Console.WriteLine("SHF "+ fileFullPath);
                        }

                    }
                }
                catch (Exception ex)
                {
                    FaceHand.Common.Util.SystemLoger.Current.Write(ex);
                }

                System.Threading.Thread.Sleep(100);

            }

            //是放当前对象内容的资源
            this.Close();

            //处理子目录
            ProcessChildDir();

        }

        private void ProcessChildDir()
        {
            var threadList = new List<Task>();
            var childDirs = System.IO.Directory.EnumerateDirectories(_currentDirName);
            foreach (var childDir in childDirs)
            {
                if (threadList.Count < 4)
                {
                    threadList.Add(new FileProcessor(childDir).Run());
                }
                else
                {
                    //等待子线程执行完毕
                    Task.WaitAll(threadList.ToArray());

                    //执行完成后重新初始化任务列表，继续下4个任务
                    threadList = new List<Task>();

                }


            }

        }

        public void Close()
        {
            if (_fi != null)
                _fi.Close();

            _fileExtArrays = null;

        }

        public void Dispose()
        {
            Close();
        }
    }

}
