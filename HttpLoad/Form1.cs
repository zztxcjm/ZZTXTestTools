using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HttpLoad
{

    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.txtUrl.Focus();
            this.lstHttpMethod.SelectedIndex = 0;

            this.lstConcurrentMethod.Items.Add(new ConcurrentMethodOption(ConcurrentMethod.Meanwhile));
            this.lstConcurrentMethod.Items.Add(new ConcurrentMethodOption(ConcurrentMethod.Order));
            this.lstConcurrentMethod.Items.Add(new ConcurrentMethodOption(ConcurrentMethod.Random));
            this.lstConcurrentMethod.SelectedIndex = 0;

        }

        private void btOpenExcel_Click(object sender, EventArgs e)
        {
            if (this.txtResult.Text.Length > 0)
            {
                File.WriteAllText("result.csv", this.txtResult.Text);
                System.Diagnostics.Process.Start("result.csv");
            }

        }

        private void btOpenChart_Click(object sender, EventArgs e)
        {

        }

        private void btStart_Click(object sender, EventArgs e)
        {

            //url
            if (String.IsNullOrEmpty(this.txtUrl.Text))
            {
                MessageBox.Show("URL不能为空");
                return;
            }

            //线程数
            if (String.IsNullOrEmpty(this.txtThreadCount.Text))
            {
                MessageBox.Show("人数不能为空");
                return;
            }
            int threadCount = 0;
            if (!int.TryParse(this.txtThreadCount.Text, out threadCount))
            {
                MessageBox.Show("人数必须为数字");
                return;
            }

            //超时
            if (String.IsNullOrEmpty(this.txtTimeout.Text))
            {
                MessageBox.Show("超时秒数不能为空");
                return;
            }
            int timeOut = 0;
            if (!int.TryParse(this.txtTimeout.Text, out timeOut))
            {
                MessageBox.Show("超时秒数必须为数字");
                return;
            }


            //请求方式
            string httpmethod = lstHttpMethod.Text;
            if (String.IsNullOrEmpty(httpmethod))
            {
                httpmethod = "GET";
            }

            //Http Header
            Dictionary<string, string> httpheader = new Dictionary<string, string>();
            if (!String.IsNullOrEmpty(this.txtHttpHeader.Text))
            {
                string[] lines = this.txtHttpHeader.Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var l in lines)
                {
                    string[] lines2 = l.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (lines2.Length == 2)
                    {
                        var k = lines2[0].Trim();
                        var v = lines2[1].Trim();
                        httpheader[k] = v;
                    }
                }
            }

            //ui state
            this.btStart.Enabled = false;
            this.btStart.Text = "测试中...";
            this.txtResult.Text = "测试中...";

            //并发方式
            ConcurrentMethod concurrentMethod = (this.lstConcurrentMethod.SelectedItem as ConcurrentMethodOption).Method;

            //start
            StartTest(this.txtUrl.Text, threadCount, timeOut, httpmethod, txtPostData.Text, httpheader, concurrentMethod);

            //ui state
            this.btStart.Enabled = true;
            this.btStart.Text = "开始";

        }

        private string ProcessUrl(string url)
        {

            if (String.IsNullOrEmpty(url))
                return String.Empty;

            var url1 = url.ToLower();
            if (!url1.StartsWith("http://") && !url1.StartsWith("https://"))
            {
                return "http://" + url.Replace("\r", "").Replace("\n", "");
            }
            else
            {
                return url.Replace("\r", "").Replace("\n", "");
            }

        }

        private void TaskProcess(object state)
        {
            TParam param = state as TParam;

            if(param.concurrentMethod==ConcurrentMethod.Random)
            {
                System.Threading.Thread.Sleep(new Random().Next(1, 60));
            }

            //==

            var request = (HttpWebRequest)HttpWebRequest.Create(param.url);
            request.Method = param.httpmethod;
            request.Accept = "*/*";
            request.UserAgent = param.agent;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = param.timeOut * 1000;

            //可能会覆盖前面的设置
            if (param.httpheader != null && param.httpheader.Count > 0)
            {
                foreach (var k in param.httpheader.Keys)
                {
                    request.Headers.Add(k, param.httpheader[k]);
                }
            }

            if (param.httpmethod == "POST")
            {
                if (!String.IsNullOrEmpty(param.postdata))
                {
                    var bytes = System.Text.Encoding.Default.GetBytes(param.postdata);
                    request.ContentLength = bytes.Length;

                    using (var reqStream = request.GetRequestStream())
                    {
                        reqStream.Write(bytes, 0, bytes.Length);
                    }
                }
                else
                {
                    request.ContentLength = 0;
                }
            }

            DateTime d1 = DateTime.Now;

            //获取请求
            try
            {
                using (HttpWebResponse wr = (HttpWebResponse)request.GetResponse())
                {
                    DateTime d2 = DateTime.Now;
                    var t = (d2 - d1).TotalMilliseconds;
                    param.responsetime[param.index] = t;
                    param.resultcache[param.index] = String.Format(
                        "人员{0},{1}ms,成功({2})", param.index, t, (int)wr.StatusCode);
                }
            }
            catch (Exception ex)
            {
                DateTime d2 = DateTime.Now;
                var t = (d2 - d1).TotalMilliseconds;
                param.responsetime[param.index] = t;
                param.resultcache[param.index] = String.Format(
                    "人员{0},{1}ms,失败({2})", param.index, t, ex.Message);
            }
            //==

        }

        private async void StartTest(string url, 
            int threadCount, 
            int timeOut, 
            string httpmethod, 
            string postdata, 
            Dictionary<string, string> httpheader, 
            ConcurrentMethod concurrentMethod)
        {

            var _result = new string[threadCount];
            var _responsetime = new double[threadCount];
            url = ProcessUrl(url);

            var tasks = new List<Task>();
            for (int i = 0; i < threadCount; i++)
            {
                tasks.Add(Task.Factory.StartNew(TaskProcess, new TParam()
                {
                    index = i,
                    url = url,
                    timeOut = timeOut,
                    httpmethod = httpmethod,
                    postdata = postdata,
                    httpheader = httpheader,
                    concurrentMethod = concurrentMethod,
                    resultcache = _result,
                    responsetime = _responsetime
                }));
            }

            var d1 = DateTime.Now;

            await Task.WhenAll(tasks);

            //所有请求已完成===================================

            var d2 = DateTime.Now;

            var buf = new StringBuilder();
            buf.AppendLine("MaxTime,MinTime,AvgTime,Total");
            buf.AppendLine(String.Format("{0}ms,{1}ms,{2}ms,{3}ms", _responsetime.Max(), _responsetime.Min(), _responsetime.Average(), (d2 - d1).TotalMilliseconds));
            buf.AppendLine("人员,时间,结果");
            buf.AppendLine(string.Join("\r\n", _result));
            this.txtResult.Text = buf.ToString();
        }

    }
}
