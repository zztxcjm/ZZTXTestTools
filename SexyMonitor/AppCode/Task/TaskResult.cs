using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SexyMonitor
{
    class TaskResult
    {

        public static Dictionary<int, string> _errorCode = new Dictionary<int, string>();

        static TaskResult()
        {
            _errorCode.Add(3, "错误的请求");
            _errorCode.Add(4, "签名为空");
            _errorCode.Add(5, "签名串错误");
            _errorCode.Add(6, "appid/bucket/url不匹配");
            _errorCode.Add(7, "签名编码失败（内部错误）");
            _errorCode.Add(8, "签名解码失败（内部错误）");
            _errorCode.Add(9, "签名过期");
            _errorCode.Add(10, "appid不存在");
            _errorCode.Add(11, "secretid不存在");
            _errorCode.Add(12, "appid不匹配");
            _errorCode.Add(13, "重放攻击");
            _errorCode.Add(14, "签名失败");
            _errorCode.Add(15, "操作太频繁，触发频控");
            _errorCode.Add(16, "内部错误");
            _errorCode.Add(17, "未知错误");
            _errorCode.Add(200, "内部打包失败");
            _errorCode.Add(201, "内部解包失败");
            _errorCode.Add(202, "内部链接失败");
            _errorCode.Add(203, "内部处理超时");
            _errorCode.Add(-1300, "图片为空");
            _errorCode.Add(-1308, "url图片下载失败");
            _errorCode.Add(-1400, "非法的图片格式");
            _errorCode.Add(-1403, "图片下载失败");
            _errorCode.Add(-1404, "图片无法识别");
            _errorCode.Add(-1505, "url格式不对");
            _errorCode.Add(-1506, "图片下载超时");
            _errorCode.Add(-1507, "无法访问url对应的图片服务器");
            _errorCode.Add(-5062, "url对应的图片已被标注为不良图片，无法访问（专指存储于腾讯云的图片）");
        }

        private static string getMd5FromResultUrl(string url)
        {
            var filePath = replaceFileUrl(url);
            var md5 = FileTask.GetMD5HashFromFile(filePath);
            return md5;
        }        

        private static string replaceFileUrl(string url)
        {

            var tmp = GetUrlReplacePattern();
            for (int i = 0; i < tmp.Length; i++)
            {
                url = url.Replace(tmp[i + 1], tmp[i]);
                i++;
            }
            url = url.Replace("/", "\\");
            return url;

        }

        private static string[] _urlReplacePattern;
        private static string[] GetUrlReplacePattern()
        {

            if (_urlReplacePattern == null)
            {

                var urlReplacePattern = System.Configuration.ConfigurationManager.AppSettings["UrlReplacePattern"];
                if (String.IsNullOrEmpty(urlReplacePattern))
                    throw new Exception("UrlReplacePattern未配置");

                var tmp = urlReplacePattern.Split(new char[] { '|', ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (tmp.Length % 2 != 0)
                    throw new Exception("UrlReplacePattern配置不正确");

                _urlReplacePattern = tmp;
            }

            return _urlReplacePattern;

        }

        public static void Process(IEnumerable<ResponseResultItem> resp)
        {

            if (resp == null || resp.Count() == 0)
                return;

            Task.Factory.StartNew(() => {

                List<string> exceptionMd5s = new List<string>();
                List<long> exceptionTaskids = new List<long>();

                var buf = new StringBuilder();
                buf.AppendLine("处理结果"+ resp.Count());

                //处理相应
                foreach (var item in resp)
                {
                    long? taskId = null;
                    string taskMd5 = null;

                    try
                    {

                        taskId = TaskIdMaps.GetId(item.url);

                        if (!taskId.HasValue)
                            taskMd5 = getMd5FromResultUrl(item.url);

                        if (item.code == 0)//识别成功
                        {
                            if (taskId.HasValue)
                            {
                                FileTaskDAL.GetInstance().UpdateTaskProcessState3(taskId.Value, true, false, 0, "success",
                                     (AppraiseResult)item.data.result, item.data.confidence, item.data.hot_score, item.data.normal_score, item.data.porn_score);

                                buf.AppendLine("task:"+ taskId.Value+" code:"+ item.code);

                            }
                            else
                            {
                                FileTaskDAL.GetInstance().UpdateTaskProcessState3(taskMd5, true, false, 0, "success",
                                     (AppraiseResult)item.data.result, item.data.confidence, item.data.hot_score, item.data.normal_score, item.data.porn_score);

                                buf.AppendLine("task:" + taskMd5 + " code:" + item.code);

                            }
                        }
                        else//识别错误
                        {
                            if(taskId.HasValue)
                            {
                                //错误有定义
                                if (_errorCode.ContainsKey(item.code))
                                {
                                    FileTaskDAL.GetInstance().UpdateTaskProcessState2(taskId.Value, false, false, item.code, _errorCode[item.code]);
                                }
                                else//错误未定义
                                {
                                    FileTaskDAL.GetInstance().UpdateTaskProcessState2(taskId.Value, false, false, -1, "不能识别的错误");
                                }

                                buf.AppendLine("task:" + taskId.Value + " code:" + item.code);


                            }
                            else
                            {
                                //错误有定义
                                if (_errorCode.ContainsKey(item.code))
                                {
                                    FileTaskDAL.GetInstance().UpdateTaskProcessState2(taskMd5, false, false, item.code, _errorCode[item.code]);
                                }
                                else//错误未定义
                                {
                                    FileTaskDAL.GetInstance().UpdateTaskProcessState2(taskMd5, false, false, -1, "不能识别的错误");
                                }

                                buf.AppendLine("task:" + taskMd5 + " code:" + item.code);

                            }
                        }

                    }
                    catch (Exception ex)
                    {

                        //将产生异常的ID记录下来

                        if (taskId.HasValue)
                        {
                            exceptionTaskids.Add(taskId.Value);
                            buf.AppendLine("task exception:" + taskId.Value + " code:" + item.code);
                        }
                        if (!String.IsNullOrEmpty(taskMd5))
                        {
                            exceptionMd5s.Add(taskMd5);
                            buf.AppendLine("task exception:" + taskMd5 + " code:" + item.code);
                        }
                        
                        FaceHand.Common.Util.SystemLoger.Current.Write(ex);

                    }
                    finally
                    {
                        try
                        {
                            //使用完成后从缓存中移除ID映射
                            TaskIdMaps.RemoveId(item.url);
                        }
                        catch (Exception ex)
                        {
                            FaceHand.Common.Util.SystemLoger.Current.Write(ex);
                        }
                    }

                }

                if (LogState.Enabled)
                {
                    FaceHand.Common.Core.WxLogProvider.Write(buf.ToString(), "TaskResult_Process");
                }

                //更新异常任务的状态
                try
                {
                    if (exceptionMd5s != null && exceptionMd5s.Count() > 0)
                        FileTaskDAL.GetInstance().UpdateTaskProcessState4(exceptionMd5s);
                    if (exceptionTaskids != null && exceptionTaskids.Count() > 0)
                        FileTaskDAL.GetInstance().UpdateTaskProcessState4(exceptionTaskids);
                }
                catch (Exception ex)
                {
                    FaceHand.Common.Util.SystemLoger.Current.Write(ex);
                }

            });


        }
    }
}
