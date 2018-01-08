using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using FaceHand.Common.DBMaper;

namespace SexyMonitor
{
    class FileTask
    {

        private static int _MonitorImageWidth = 0;
        private static int _MonitorImageHeight = 0;

        static FileTask()
        {
            string str_MonitorImageWidth = System.Configuration.ConfigurationManager.AppSettings["MonitorImageWidth"];
            string str_MonitorImageHeight = System.Configuration.ConfigurationManager.AppSettings["MonitorImageHeight"];
            if (!int.TryParse(str_MonitorImageWidth, out _MonitorImageWidth))
                _MonitorImageWidth = 250;
            if (!int.TryParse(str_MonitorImageHeight, out _MonitorImageHeight))
                _MonitorImageHeight = 250;

        }

        public static void CreateTask(string filePath)
        {
            if (String.IsNullOrEmpty(filePath))
                return;

            //临时文件不提交
            if (filePath.IndexOf(".tmp.") >= 0)
                return;

            //文件已不存在
            var fi = new System.IO.FileInfo(filePath);
            if (!fi.Exists)
                return;

            //分辨率大于250*250才提交
            if (!ResolutionRatioTest(filePath))
                return;

            //提交任务
            var md5 = GetMD5HashFromFile(filePath);
            if (!String.IsNullOrEmpty(md5))
            {
                if (!FileTaskDAL.GetInstance().IsExistFileTask(md5))
                {
                    FileTaskDAL.GetInstance().Insert(new FileTaskInfo()
                    {
                        Md5 = md5,
                        FileFullPath = filePath,
                        IsProcessed = false,
                        Confidence = 0,
                        Hot_score = 0,
                        Porn_score = 0,
                        Normal_score = 0,
                        Result = AppraiseResult.Unknow,
                        FileUploadTime = fi.CreationTime,
                        CorpId = GetCorpIdByFilePath(filePath)
                    });
                }
            }

        }

        public static void CreateTaskAsync(string filePath)
        {
            if (String.IsNullOrEmpty(filePath))
                return;

            Task.Factory.StartNew(() => {

                try
                {
                    FileTask.CreateTask(filePath);
                }
                catch (Exception ex)
                {
                    FaceHand.Common.Util.SystemLoger.Current.Write(ex);
                }

            });

        }

        public static void UpdateCorpId()
        {

            Task.Factory.StartNew(() => {

                while (true)
                {
                    try
                    {
                        var tasks = FileTaskDAL.GetInstance()
                                                .SelectFileTaskWhereCorpIdIsNull(50).ToList<FileTaskInfo>();

                        if (tasks.Count() == 0)
                        {
                            Console.WriteLine("UpdateCorpId Done");
                            return;
                        }
                        else
                        {
                            var buf = new StringBuilder();
                            foreach (var t in tasks)
                            {
                                var corpid = GetCorpIdByFilePath(t.FileFullPath);
                                if (corpid.HasValue)
                                    buf.AppendLine("update sm_fileinfo set CorpId=" + corpid.Value + " where Id=" + t.Id + ";");
                                else
                                    buf.AppendLine("update sm_fileinfo set CorpId=-1 where Id=" + t.Id + ";");
                            }

                            FileTaskDAL.GetInstance().DbInstance
                                .ExecuteNonQuery(System.Data.CommandType.Text, buf.ToString());

                            Console.WriteLine("UpdateCorpId Run");

                        }
                    }
                    catch (Exception ex)
                    {
                        FaceHand.Common.Util.SystemLoger.Current.Write(ex);
                    }

                    System.Threading.Thread.Sleep(2000);

                }

            });

        }
        public static void RemoveFileTask(string md5)
        {
            if (!String.IsNullOrEmpty(md5))
                FileTaskDAL.GetInstance().Delete(md5);
        }
        public static void RemoveFileTask(long id)
        {
            FileTaskDAL.GetInstance().Delete(id);
        }
        public static FileTaskInfo GetFileTask(string md5)
        {

            if (String.IsNullOrEmpty(md5))
                return null;

            return FileTaskDAL.GetInstance().SelectFileTask(md5).Fill<FileTaskInfo>();

        }
        public static FileTaskInfo GetFileTask(long id)
        {
            return FileTaskDAL.GetInstance().SelectFileTask(id).Fill<FileTaskInfo>();
        }
        public static IEnumerable<FileTaskInfo> GetFileTaskWaitPorcessing(int num)
        {
            return FileTaskDAL.GetInstance().SelectFileTaskWaitPorcessing(num).ToList<FileTaskInfo>();
        }
        public static void UpdateTaskProcessState(IEnumerable<long> taskIds)
        {
            FileTaskDAL.GetInstance().UpdateTaskProcessState(taskIds);
        }
        public static string GetMD5HashFromFile(string fileFullPath)
        {

            FileStream file = null;

            try
            {
                if (File.Exists(fileFullPath))
                {

                    while (!canReadFile(fileFullPath))
                    {
                        System.Threading.Thread.Sleep(500);
                    }

                    file = new FileStream(fileFullPath, FileMode.Open);

                    var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                    byte[] retVal = md5.ComputeHash(file);

                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < retVal.Length; i++)
                    {
                        sb.Append(retVal[i].ToString("x2"));
                    }
                    return sb.ToString();
                }

            }
            catch (Exception ex)
            {
                FaceHand.Common.Util.SystemLoger.Current.Write(ex);
            }
            finally
            {
                if (file != null)
                    file.Close();
            }

            return null;

        }

        private static bool canReadFile(string path)
        {
            var canRead = false;
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
                {
                    canRead = true;
                }
            }
            catch (System.IO.IOException)
            {
                canRead = false;
            }

            return canRead;
        }

        private static Regex _matchCorpId = new Regex("\\\\(\\d{5})\\\\", RegexOptions.IgnoreCase);

        private static long? GetCorpIdByFilePath(string filePath)
        {

            if (String.IsNullOrEmpty(filePath))
                return null;

            var m = _matchCorpId.Match(filePath);
            if (m.Success)
                return Convert.ToInt64(m.Groups[1].Value);
            else
                return null;

        }

        private static object _bmpLock = new object();

        private static bool ResolutionRatioTest(string filePath)
        {

            lock (_bmpLock)
            {

                System.Drawing.Image bmp = null;

                try
                {
                    bmp = System.Drawing.Bitmap.FromFile(filePath);
                    return bmp.Width > _MonitorImageWidth && bmp.Height > _MonitorImageHeight;
                }
                catch (Exception ex)
                {
                    FaceHand.Common.Util.SystemLoger.Current.Write(ex);
                    return false;
                }
                finally
                {
                    if (bmp != null)
                        bmp.Dispose();

                }

            }

         }

     }

}
