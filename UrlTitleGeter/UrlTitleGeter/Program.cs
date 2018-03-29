using System;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Practices.EnterpriseLibrary.Data;
using FaceHand.Common.Util;
using FaceHand.Common.DBMaper;
using FaceHand.Common;
using FaceHand.Common.ShortUrl;


namespace UrlTitleGeter
{
    class Program
    {
        static void Main(string[] args)
        {
            //var title = String.Empty;
            //var tryAgain = false;
            //var httpCode = -1;
            //TryGetTitle("https://weidian.com/item.html?itemID=2258301029&wfr=qr&spider_token=98a6&spider=seller.zx-shopdetail.5.4", out title, out tryAgain, out httpCode);

            //Console.WriteLine(tryAgain);
            //Console.WriteLine(title);

            //return;

            Console.WriteLine("开始处理监视");

            while (true)
            {

                try
                {
                    StartTask();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    SystemLoger.Current.Write(ex);
                }

                var interval_str = System.Configuration.ConfigurationManager.AppSettings["Interval"];
                var interval_int = 30;

                if (!String.IsNullOrEmpty(interval_str))
                {
                    try
                    {
                        interval_int = Convert.ToInt32(interval_str);
                    }
                    catch
                    {
                    }
                }

                System.Threading.Thread.Sleep(interval_int * 1000);

            }

        }

        private static void StartTask()
        {

            var pagesize_str = System.Configuration.ConfigurationManager.AppSettings["PageSize"];
            var pagesize = 50;
            if (!String.IsNullOrEmpty(pagesize_str))
            {
                try
                {
                    pagesize = Convert.ToInt32(pagesize_str);
                }
                catch
                {
                }
            }


            var sql = "select * from common_shorturl where IsUpdateTitle=0 and TryCount<=5 limit 0," + Math.Max(10, pagesize);
            var db = FaceHand.Common.CustomDb.CustomMySqlFactory.CreateDefaultDatabase();
            var dt = db.ExecuteDataSet(CommandType.Text, sql).Tables[0];

            int successCount = 0;
            int failedCount = 0;

            if (dt.Rows.Count > 0)
            {
                using (var mSql = new SampleBatchSql())
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        try
                        {
                            var url = row.Fill<UrlInfo>();
                            if (String.IsNullOrEmpty(url.Url))
                                continue;

                            var urlIsChange = false;

                            //兼容一个错误的URL格式
                            if (url.Url.StartsWith("http://https://"))
                            {
                                url.Url = url.Url.Replace("http://https://", "https://");
                                urlIsChange = true;
                            }
                            else if (url.Url.StartsWith("https://http://"))
                            {
                                url.Url = url.Url.Replace("https://http://", "http://");
                                urlIsChange = true;
                            }
                            else if (url.Url.StartsWith("http://http://"))
                            {
                                url.Url = url.Url.Replace("http://http://", "http://");
                                urlIsChange = true;
                            }
                            else if (url.Url.StartsWith("https://https://"))
                            {
                                url.Url = url.Url.Replace("https://https://", "https://");
                                urlIsChange = true;
                            }

                            Console.WriteLine($"开始处理{url.Url}");

                            var tryAgain = false;
                            var title = String.Empty;
                            var sql2 = String.Empty;
                            var httpCode = -1;

                            if (TryGetTitle(url.Url, out title, out tryAgain, out httpCode))
                            {
                                if (!String.IsNullOrEmpty(title))
                                {
                                    sql2 = $"update common_shorturl set $changeurl$Title='{title.Replace("'", "\\'")}',IsUpdateTitle=1,TryCount=TryCount+1,HttpCode={httpCode} where ID={url.ID};";
                                }
                                else
                                {
                                    sql2 = $"update common_shorturl set $changeurl$IsUpdateTitle=1,TryCount=TryCount+1,HttpCode={httpCode} where ID = {url.ID};";
                                }
                            }
                            else
                            {
                                if (tryAgain)
                                {
                                    sql2 = $"update common_shorturl set $changeurl$TryCount = TryCount + 1,HttpCode={httpCode} where ID = {url.ID};";
                                }
                                else
                                {
                                    sql2 = $"update common_shorturl set $changeurl$IsUpdateTitle=1,TryCount=TryCount+1,HttpCode={httpCode} where ID = {url.ID};";
                                }
                            }

                            if (sql2.Length > 0)
                            {
                                if (urlIsChange)
                                {
                                    mSql.AppendSql(sql2.Replace("$changeurl$", "Url='" + url.Url.Replace("'", "\\'") + "',"));
                                }
                                else
                                {
                                    mSql.AppendSql(sql2.Replace("$changeurl$", String.Empty));
                                }
                            }

                            successCount++;

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            SystemLoger.Current.Write(ex);
                            failedCount++;
                        }
                    }

                    mSql.Commit(db);

                }

            }

            var total = successCount + failedCount;
            if (total > 0)
            {
                Console.WriteLine($"成功{successCount}个,失败{failedCount}个,共{total}个");
            }

        }

        private static bool TryGetTitle(string url, out string title, out bool tryAgain, out int httpcode)
        {

            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "http://" + url;
            }

            tryAgain = false;
            //if (!TrySpecialUrl(url, out title))
            //{
            //    return TryWebGet(url, out title, out tryAgain);
            //}
            return TryWebGet(url, out title, out tryAgain, out httpcode);

        }

        private static bool TrySpecialUrl(string url, out string title)
        {

            var tmpUrl = url.ToLower();

            var id = string.Empty;
            if (tmpUrl.Contains("id=") && tmpUrl.Contains("?"))
            {
                string[] sArray = url.Split('?');
                var tmp = sArray[1];
                if (tmp.Contains("&"))
                {
                    string[] mArray = tmp.Split('&');
                    foreach (var a in mArray)
                    {
                        if (a.Contains("id=") && !a.Contains("corpid="))
                            id = a.Replace("id=", "");
                        if (a.Contains("Id=") && !a.Contains("corpId="))
                            id = a.Replace("Id=", "");
                    }
                }
                else
                {
                    if (!tmp.Contains("corpid"))
                    {
                        id = tmp.Replace("id=", "");
                        id = tmp.Replace("Id=", "");
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                title = String.Empty;
                return false;
            }

            var re = false;
            var name = string.Empty;
            if (tmpUrl.Contains("mall"))
            {
                using (DataContext context = new DataContext(ConstDefined.Connect_Mall))
                {
                    // 商城标题
                    name = ShortUrl.MallName(Convert.ToInt64(id));
                    if (string.IsNullOrWhiteSpace(name))
                        name = "互动商城";

                    re = true;
                }
            }
            else
            {
                using (DataContext context = new DataContext(ConstDefined.Connect_App))
                {
                    if (tmpUrl.Contains("vote"))
                    {
                        // 投票
                        var idstr = id.Base64Decode(string.Empty);
                        var idArr = idstr.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                        if (idArr.Length != 3)
                            throw new FaceHand.Common.Exceptions.BusinessException("参数错误。");

                        var voteId = Convert.ToInt64(idArr[2]);
                        name = ShortUrl.VoteName(voteId);
                        if (string.IsNullOrWhiteSpace(name))
                            name = "微投票";

                        re = true;
                    }
                    else if (tmpUrl.Contains("books"))
                    {
                        // 微学习
                        name = ShortUrl.BookName(Convert.ToInt64(id));
                        if (string.IsNullOrWhiteSpace(name))
                            name = "微学习";

                        re = true;
                    }
                    else if (tmpUrl.Contains("signup"))
                    {
                        // 微报名
                        name = ShortUrl.SignUpName(Convert.ToInt64(id));
                        if (string.IsNullOrWhiteSpace(name))
                            name = "微报名";

                        re = true;
                    }
                    else if (tmpUrl.Contains("survey"))
                    {
                        // 微问卷
                        name = ShortUrl.SurveyName(id);
                        if (string.IsNullOrWhiteSpace(name))
                            name = "问卷调查";

                        re = true;
                    }
                    else if (tmpUrl.Contains("sharework"))
                    {
                        // 公文包
                        name = ShortUrl.WorkFileName(id);
                        if (string.IsNullOrWhiteSpace(name))
                            name = "公文包";

                        re = true;
                    }
                    else if (tmpUrl.Contains("journal"))
                    {
                        // 微刊
                        name = ShortUrl.JournalName(Convert.ToInt64(id));
                        if (string.IsNullOrWhiteSpace(name))
                            name = "微刊";

                        re = true;
                    }
                }
            }

            title = name;

            return re;

        }

        //<title></title>
        private static System.Text.RegularExpressions.Regex title_regex =
            FaceHand.Common.Util.RegExt.CreateFromCache("<title>([^<]+)</title>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        //<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        private static System.Text.RegularExpressions.Regex charset_regex =
            FaceHand.Common.Util.RegExt.CreateFromCache("charset=\"{0,1}([^\\s\">]+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        //<meta http-equiv="Content-Language" content="utf-8" />
        private static System.Text.RegularExpressions.Regex content_language_regex =
            FaceHand.Common.Util.RegExt.CreateFromCache("content=[\"'](.+)[\"']", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        private static System.Text.RegularExpressions.Regex comment_regex =
            FaceHand.Common.Util.RegExt.CreateFromCache("<!--[^-]*-->", System.Text.RegularExpressions.RegexOptions.IgnoreCase);


        private static bool TryWebGet(string url, out string title, out bool tryAgain, out int httpcode)
        {

            tryAgain = false;
            httpcode = 200;

            try
            {

                var wq = (HttpWebRequest)HttpWebRequest.Create(url);
                wq.Method = "GET";
                wq.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.119 Safari/537.36";
                wq.ContentType = "text/html;";
                wq.Timeout = 30000;

                using (var resp = (HttpWebResponse)wq.GetResponse())
                {

                    if (resp.ContentType.Contains("text/html"))
                    {

                        //请求成功
                        if (resp.StatusCode == HttpStatusCode.OK)
                        {
                            //编码明确
                            if (!String.IsNullOrEmpty(resp.ContentEncoding))
                            {
                                //获取标题
                                using (var st = resp.GetResponseStream())
                                {
                                    using (StreamReader rs = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding(resp.ContentEncoding)))
                                    {
                                        while (!rs.EndOfStream)
                                        {
                                            var line = rs.ReadLine();
                                            if (line == null)
                                                break;

                                            var re = title_regex.Match(line);
                                            if (re.Success)
                                            {
                                                title = re.Groups[1].Value;
                                                return true;
                                            }

                                        }
                                    }
                                }

                            }
                            else//编码不明确
                            {

                                byte[] title_bytes = null;
                                string c_encode = string.Empty;

                                var bytes = new List<byte>();
                                using (var rs = resp.GetResponseStream())
                                {
                                    while (rs.CanRead)
                                    {
                                        int d = rs.ReadByte();
                                        if (d == -1)
                                        {
                                            break;
                                        }

                                        if (d == '\r' || d == '\n')
                                        {
                                            if (bytes.Count != 0)
                                            {
                                                var line_bytes = bytes.ToArray();
                                                var line_string = Encoding.UTF8.GetString(line_bytes);
                                                //去掉里面的注释，防止被匹配干扰
                                                line_string = comment_regex.Replace(line_string, String.Empty);

                                                if (line_string.IndexOf("<title>", StringComparison.CurrentCultureIgnoreCase) != -1)
                                                {
                                                    //将title行的bytes数据放入缓存
                                                    title_bytes = line_bytes;

                                                    if (!String.IsNullOrEmpty(c_encode))
                                                        break;

                                                }
                                                if (String.IsNullOrEmpty(c_encode) && line_string.IndexOf("charset=") != -1)
                                                {
                                                    var re = charset_regex.Match(line_string);
                                                    if (re.Success)
                                                    {
                                                        c_encode = re.Groups[1].Value;
                                                        if (title_bytes != null && title_bytes.Length > 0)
                                                            break;
                                                    }
                                                }
                                                if (String.IsNullOrEmpty(c_encode) && line_string.IndexOf("Content-Language") != -1)
                                                {
                                                    var re = charset_regex.Match(line_string);
                                                    if (re.Success)
                                                    {
                                                        c_encode = re.Groups[1].Value;
                                                        if (title_bytes != null && title_bytes.Length > 0)
                                                            break;
                                                    }
                                                }
                                                if (line_string.IndexOf("</head>", StringComparison.CurrentCultureIgnoreCase) != -1)
                                                {
                                                    break;
                                                }
                                                //处理完一行换新行
                                                bytes = new List<byte>();
                                            }
                                        }
                                        else
                                        {
                                            bytes.Add((byte)d);
                                        }
                                    }
                                }

                                if (title_bytes != null && title_bytes.Length > 0)
                                {

                                    string title_str = !String.IsNullOrEmpty(c_encode)
                                        ? Encoding.GetEncoding(c_encode).GetString(title_bytes)
                                        : Encoding.UTF8.GetString(title_bytes);

                                    var re = title_regex.Match(title_str);
                                    if (re.Success)
                                    {

                                        title = re.Groups[1].Value;
                                        if (title != null && title.Length > 0)
                                            title = title.Trim();

                                        if (title.IsMessyCode())
                                        {
                                            title = String.Empty;
                                        }

                                        return true;

                                    }

                                }

                            }

                        }
                        //重定向到新地址
                        else if (resp.StatusCode == HttpStatusCode.Found)
                        {
                            var redirect = resp.GetResponseHeader("Location");
                            if (!String.IsNullOrEmpty(redirect))
                            {
                                return TryWebGet(redirect, out title, out tryAgain, out httpcode);
                            }
                        }
                        else
                        {
                            httpcode = (int)resp.StatusCode;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                SystemLoger.Current.Write(ex);
            }

            title = String.Empty;
            return false;

        }

    }

    public class UrlInfo
    {
        public long ID { get; set; }
        public string ShortUrlCode { get; set; }
        public string HashCode { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public bool IsUpdateTitle { get; set; }
        public int TryCount { get; set; }
    }

}
