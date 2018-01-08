using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaceHand.Common.Util;
using System.Security.Cryptography;
using System.Threading;

namespace SexyMonitor
{
    public class SexyFilter
    {

        private static string CLOUD_url = System.Configuration.ConfigurationManager.AppSettings["CLOUD_url"];
        private static string CLOUD_appid = System.Configuration.ConfigurationManager.AppSettings["CLOUD_Appid"];
        private static string CLOUD_bucket = System.Configuration.ConfigurationManager.AppSettings["CLOUD_bucket"];
        private static string CLOUD_secretID = System.Configuration.ConfigurationManager.AppSettings["CLOUD_secretID"];
        private static string CLOUD_secretkey = System.Configuration.ConfigurationManager.AppSettings["CLOUD_secretkey"];

        public static Task Process(IEnumerable<FileTaskInfo> tasks)
        {

            return Task.Factory.StartNew(() => {

                if (tasks == null || tasks.Count() == 0)
                    return;

                try
                {

                    var urls = new List<string>();
                    var taskIds = new List<long>();
                    foreach (var task in tasks)
                    {
                        if (String.IsNullOrEmpty(task.FileFullPath) || task.IsProcessed)
                            continue;

                        if (!System.IO.File.Exists(task.FileFullPath))
                        {
                            //文件已不存在，删除任务
                            FileTask.RemoveFileTask(task.Id);
                            continue;
                        }

                        var url = replaceFileUrl(task.FileFullPath);
                        TaskIdMaps.Set(url, task.Id);//将ID映射记录下来

                        urls.Add(url);
                        taskIds.Add(task.Id);

                    }

                    //再次确认哪些URL要处理
                    if (urls.Count == 0)
                        return;

                    var reqdata = new RequestBody() { appid = CLOUD_appid, bucket = CLOUD_bucket, url_list = urls };
                    var str_reqdata = Newtonsoft.Json.JsonConvert.SerializeObject(reqdata);
                    var byt_reqdata = Encoding.UTF8.GetBytes(str_reqdata);
                    var sign_reqdata = GetSign();

                    var client = (HttpWebRequest)WebRequest.Create(CLOUD_url);
                    client.Headers.Add("Authorization", sign_reqdata);
                    client.ContentType = "application/json";
                    client.ContentLength = byt_reqdata.Length;
                    client.Timeout = 20000;//20s
                    client.Method = "POST";
                    //写数据到请求
                    using (var reqStream = client.GetRequestStream())
                    {
                        reqStream.Write(byt_reqdata, 0, byt_reqdata.Length);
                    }

                    //发送请求前记录请求日志
                    if (LogState.Enabled)
                    {
                        string log = String.Format("{0} {1} {2}\n{3}", DateTime.Now, tasks.Count(), sign_reqdata, str_reqdata);
                        FaceHand.Common.Core.WxLogProvider.Write(log, "SexyFilter_Process");
                    }

                    //发送请求
                    try
                    {
                        HttpWebResponse resp = client.GetResponse() as HttpWebResponse;
                        using (var respStream = resp.GetResponseStream())
                        {
                            var sr = new System.IO.StreamReader(respStream, Encoding.UTF8);
                            var resp_data = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseBody>(sr.ReadToEnd());

                            //处理结果
                            TaskResult.Process(resp_data.result_list);

                        }

                    }
                    catch (WebException ex)
                    {

                        HttpWebResponse resp = ex.Response as HttpWebResponse;

                        var buf = new StringBuilder();
                        buf.AppendLine("Service Interface Call Exception " + ((int)resp.StatusCode).ToString() + " " + resp.StatusDescription);
                        buf.AppendLine("Url:" + CLOUD_url + " Sign:" + sign_reqdata);
                        

                        //在次检测文件的有效性，有问题的直接删除任务
                        foreach (var task in tasks)
                        {
                            if (!System.IO.File.Exists(task.FileFullPath))
                                FileTask.RemoveFileTask(task.Id);

                            buf.AppendLine(String.Format("{0} {1}", task.Id, task.FileFullPath));
                        }

                        //再次更新为可处理的状态
                        FileTaskDAL.GetInstance().UpdateTaskProcessState4(taskIds);

                        if (LogState.Enabled)
                            FaceHand.Common.Core.WxLogProvider.Write(buf.ToString(), "SexyFilter_Process");

                    }

                }
                catch (Exception ex)
                {
                    FaceHand.Common.Util.SystemLoger.Current.Write(ex);
                }

            });

        }

        public static void Process(string md5)
        {
            Process(new List<FileTaskInfo>() { FileTask.GetFileTask(md5) });
        }

        public static void Process(int id)
        {
            Process(new List<FileTaskInfo>() { FileTask.GetFileTask(id) });
        }

        private static string replaceFileUrl(string fullPath)
        {

            if (string.IsNullOrEmpty(fullPath))
                return String.Empty;

            fullPath = fullPath.Replace("\\", "/");

            var tmp = GetUrlReplacePattern();
            for (int i = 0; i < tmp.Length; i++)
            {
                fullPath = fullPath.Replace(tmp[i], tmp[i + 1]);
                i++;
            }
            return fullPath;

        }

        private static string[] _urlReplacePattern;
        private static string[] GetUrlReplacePattern()
        {

            if (_urlReplacePattern == null)
            {

                var urlReplacePattern = System.Configuration.ConfigurationManager.AppSettings["UrlReplacePattern"];
                if (String.IsNullOrEmpty(urlReplacePattern))
                    throw new Exception("UrlReplacePattern未配置");

                urlReplacePattern = urlReplacePattern.Replace("\\", "/");

                var tmp = urlReplacePattern.Split(new char[] { '|', ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (tmp.Length % 2 != 0)
                    throw new Exception("UrlReplacePattern配置不正确");

                _urlReplacePattern = tmp;

            }

            return _urlReplacePattern;

        }

        private static string _signstring = string.Empty;
        private static DateTime? _lastAccessTime = null;
        private static int _expiredMinutes = 60;
        private static string GetSign()
        {

            if (!_lastAccessTime.HasValue || (DateTime.Now - _lastAccessTime.Value).TotalMinutes >= _expiredMinutes - 5)
            {

                var current = DateTime.Now;
                var expired = current.AddMinutes(_expiredMinutes);

                var str = String.Format("a={0}&b={1}&k={2}&t={3}&e={4}",
                    CLOUD_appid, CLOUD_bucket, CLOUD_secretID, current.AsUnixTimestamp(), expired.AsUnixTimestamp());

                _signstring = hash_hmac(str, CLOUD_secretkey);
                _lastAccessTime = current;

            }

            return _signstring;

        }

        private static string hash_hmac(string orgstr, string secretKey)
        {

            var enc = Encoding.UTF8;
            HMACSHA1 hmac = new HMACSHA1(enc.GetBytes(secretKey));
            hmac.Initialize();

            byte[] buffer = enc.GetBytes(orgstr);
            byte[] entmp = hmac.ComputeHash(buffer);
            byte[] retmp = entmp.Concat<byte>(buffer).ToArray();

            return Convert.ToBase64String(retmp);

        }

    }


    #region 接口数据结构

    class RequestBody
    {
        public string appid { get; set; }
        public string bucket { get; set; }
        public List<string> url_list { get; set; }
    }
    class ResponseBody
    {
        public List<ResponseResultItem> result_list { get; set; }
    }
    class ResponseResultItem
    {
        public int code { get; set; }
        public string message { get; set; }
        public string url { get; set; }
        public ResponseResultItemData data { get; set; }
    }
    class ResponseResultItemData
    {
        public int result { get; set; }
        public int forbid_status { get; set; }
        public double confidence { get; set; }
        public double hot_score { get; set; }
        public double normal_score { get; set; }
        public double porn_score { get; set; }
    }

    #endregion



}
