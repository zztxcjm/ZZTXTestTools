using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Web.Administration;

namespace WSPreLoad
{
    class Program
    {
        static void Main(string[] args)
        {

            var appList = GetWebAppList();

            Console.WriteLine($"共找到{appList.Count}个地址");

            var tasks = new List<Task>();
            foreach (var item in appList)
            {
                tasks.Add(Task.Factory.StartNew(p => {

                    var webapp = p as WebApp;
                    if (webapp != null)
                    {
                        try
                        {

                            var startDate = DateTime.Now;
                            var httpcode = 0;
                            var resp = SendRequest(webapp.Url, out httpcode);
                            var endDate = DateTime.Now;

                            Console.WriteLine($"{webapp.Url}，code:{httpcode},time:{(endDate - startDate).TotalMilliseconds}ms");

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{webapp.Url}，exception:{ex.Message}");
                        }
                    }

                }, item));
            }

            Task.WhenAll(tasks).ContinueWith(t => {

                Console.WriteLine("处理完成");

            }).Wait();

            //自动任务时自动关闭
            if (args == null || args.Length == 0)
            {
                Console.ReadLine();
            }

        }

        private static List<WebApp> GetWebAppList()
        {
            var re = new List<WebApp>();

            ServerManager iisManager = new ServerManager();
            foreach (var site in iisManager.Sites)
            {
                foreach (var bind in site.Bindings)
                {
                    if (bind.Protocol == "http" || bind.Protocol == "https")
                    {
                        var host = String.IsNullOrEmpty(bind.Host)
                            ? "127.0.0.1"
                            : bind.Host;

                        var url = $"{bind.Protocol}://{host}" + (bind.EndPoint.Port != 80 && bind.EndPoint.Port != 443 ? ":" + bind.EndPoint.Port : "");
                        re.Add(new WebApp() { SiteName = site.Name, Url = ProcessUrl(url) });

                        //WebAppChildPattern(site.Name, url, re);

                        foreach (var app in site.Applications)
                        {
                            if (app.Path != "/")
                            {
                                var cur = new WebApp() { SiteName = site.Name, Url = ProcessUrl(url + app.Path) };
                                re.Add(cur);
                                //WebAppChildPattern(site.Name, cur.Url, re);
                            }
                        }
                    }
                }
            }

            return re;

        }

        private static string ProcessUrl(string url)
        {

            if (String.IsNullOrEmpty(url))
                return null;

            var val = System.Configuration.ConfigurationManager.AppSettings[url];
            if (!String.IsNullOrEmpty(val))
            {
                return url.TrimEnd('/') + '/' + val.TrimStart('/');
            }

            var defaultRequestUrI = System.Configuration.ConfigurationManager.AppSettings["DefaultRequestUrI"];
            if (!String.IsNullOrEmpty(defaultRequestUrI))
            {
                return url.TrimEnd('/') + '/' + defaultRequestUrI.TrimStart('/');
            }

            return url;


        }

        private static void WebAppChildPattern(string siteName, string url, List<WebApp> lst)
        {

            if (lst == null)
                return;

            if (String.IsNullOrEmpty(url))
                return;

            var val = System.Configuration.ConfigurationManager.AppSettings[url];
            if (!String.IsNullOrEmpty(val))
            {
                var arr = val.Split(new char[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in arr)
                {
                    lst.Add(new WebApp() { SiteName = siteName, Url = url.TrimEnd('/') + "/" + p.TrimStart('/') });
                }
            }

        }

        private static string SendRequest(string url, out int code)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Accept = "*/*";
            request.Method = "POST";
            request.UserAgent = "ZZTX WSPreLoad";
            request.ContentType = "text/html";
            request.ContentLength = 0;

            var WebRequestTimeOut = System.Configuration.ConfigurationManager.AppSettings["WebRequestTimeOut"];
            if (String.IsNullOrEmpty(WebRequestTimeOut))
                request.Timeout = 100 * 1000;
            else
            {
                int WebRequestTimeOut_int;
                if (int.TryParse(WebRequestTimeOut, out WebRequestTimeOut_int))
                {
                    request.Timeout = WebRequestTimeOut_int * 1000;
                }
                else
                {
                    request.Timeout = 100 * 1000;
                }
            }

            //获取请求
            using (HttpWebResponse wr = (HttpWebResponse)request.GetResponse())
            {
                code = (int)wr.StatusCode;

                if (wr.StatusCode == HttpStatusCode.OK)
                {
                    using (var st = wr.GetResponseStream())
                    {
                        using (StreamReader rs = new StreamReader(st, System.Text.Encoding.UTF8))
                        {
                            return rs.ReadToEnd();
                        }
                    }
                }
            }

            return String.Empty;

        }

    }

    class WebApp
    {
        public string Url;
        public string SiteName;

        public override string ToString()
        {
            return Url;
        }

    }
}
