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

        private Percent percent;


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

            this.panel2.Location = this.panel1.Location;

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

        private void lstConcurrentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConcurrentMethod _m = (lstConcurrentMethod.SelectedItem as ConcurrentMethodOption).Method;
            if (_m == ConcurrentMethod.Meanwhile)
            {
                this.panel1.Visible = false;
                this.panel2.Visible = false;
            }
            else if (_m == ConcurrentMethod.Order)
            {
                this.panel1.Visible = false;
                this.panel2.Visible = true;
            }
            else if (_m == ConcurrentMethod.Random)
            {
                this.panel1.Visible = true;
                this.panel2.Visible = false;
            }

        }

        private void btStart_Click(object sender, EventArgs e)
        {

            var param = new TParam();

            //并发方式
            param.ConcurrentMethod = (this.lstConcurrentMethod.SelectedItem as ConcurrentMethodOption).Method;

            //请求方式
            param.Httpmethod = lstHttpMethod.Text;

            //postData
            if (param.Httpmethod == "POST")
            {
                param.Postdata = this.txtPostData.Text;
            }

            //url
            if (String.IsNullOrEmpty(this.txtUrl.Text))
            {
                MessageBox.Show("URL不能为空");
                return;
            }
            else
            {
                param.Url = ProcessUrl(this.txtUrl.Text);
            }

            //超时
            if (!String.IsNullOrEmpty(this.txtTimeout.Text))
            {
                if (!int.TryParse(this.txtTimeout.Text, out param.TimeOut))
                {
                    this.txtTimeout.Text = param.TimeOut.ToString();
                }
            }
            else
            {
                this.txtTimeout.Text = param.TimeOut.ToString();
            }

            //随机时间范围
            if (!String.IsNullOrEmpty(this.txtRandomTimeRange.Text))
            {
                if (!int.TryParse(this.txtRandomTimeRange.Text, out param.RandomTimeRange))
                {
                    this.txtRandomTimeRange.Text = param.RandomTimeRange.ToString();
                }
            }
            else
            {
                this.txtRandomTimeRange.Text = param.RandomTimeRange.ToString();
            }

            //递增时间间隔
            if (!String.IsNullOrEmpty(this.txtOrderInterval.Text))
            {
                if (!int.TryParse(this.txtOrderInterval.Text, out param.OrderInterval))
                {
                    this.txtOrderInterval.Text = param.OrderInterval.ToString();
                }
            }
            else
            {
                this.txtOrderInterval.Text = param.OrderInterval.ToString();
            }

            //间隔递增量
            if (!String.IsNullOrEmpty(this.txtOrderIncrease.Text))
            {
                if (!int.TryParse(this.txtOrderIncrease.Text, out param.OrderIncrease))
                {
                    this.txtOrderIncrease.Text = param.OrderIncrease.ToString();
                }
            }
            else
            {
                this.txtOrderIncrease.Text = param.OrderIncrease.ToString();
            }

            //线程数
            int threadCount = 100;
            if (!String.IsNullOrEmpty(this.txtThreadCount.Text))
            {
                if (!int.TryParse(this.txtThreadCount.Text, out threadCount))
                {
                    this.txtThreadCount.Text = threadCount.ToString();
                }
            }
            else
            {
                this.txtThreadCount.Text = threadCount.ToString();
            }

            //Http Header
            param.Httpheader = new Dictionary<string, string>();
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
                        param.Httpheader[k] = v;
                    }
                }
            }

            //ui state
            this.btStart.Enabled = false;
            this.btStart.Text = "测试中...";
            this.txtResult.Text = "测试中...";
            this.btOpenExcel.Enabled = false;

            //start
            StartTest(param, threadCount);

            //ui state
            this.btStart.Enabled = true;
            this.btStart.Text = "开始";
            this.btOpenExcel.Enabled = true;

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

            if (param.Sleep > 0)
            {
                System.Threading.Thread.Sleep(param.Sleep);
            }

            //==

            var request = (HttpWebRequest)HttpWebRequest.Create(param.Url);
            request.Method = param.Httpmethod;
            request.Accept = "*/*";
            request.UserAgent = param.Agent;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = param.TimeOut * 1000;

            //可能会覆盖前面的设置
            if (param.Httpheader != null && param.Httpheader.Count > 0)
            {
                foreach (var k in param.Httpheader.Keys)
                {
                    request.Headers.Add(k, param.Httpheader[k]);
                }
            }

            //处理POSTDATA
            if (param.Httpmethod == "POST")
            {
                if (!String.IsNullOrEmpty(param.Postdata))
                {
                    var bytes = System.Text.Encoding.Default.GetBytes(param.Postdata);
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
                    param.Responsetime[param.Index] = t;
                    param.Resultcache[param.Index] = String.Format(
                        "人员{0},{1}ms,{2},{3},成功({4})", param.Index + 1, t, d1.ToString(), d2.ToString(), (int)wr.StatusCode);
                }
            }
            catch (Exception ex)
            {
                DateTime d2 = DateTime.Now;
                var t = (d2 - d1).TotalMilliseconds;
                param.Responsetime[param.Index] = t;
                param.Resultcache[param.Index] = String.Format(
                    "人员{0},{1}ms,{2},{3},失败({4})", param.Index + 1, t, d1.ToString(), d2.ToString(), ex.Message);
            }
            //==

            param.PercentData.UpdateCompleted();

        }

        private async void StartTest(TParam param, int threadCount)
        {

            var _result = new string[threadCount];
            var _responsetime = new double[threadCount];
            param.Responsetime = _responsetime;
            param.Resultcache = _result;
            param.PercentData = percent = new Percent() { Total = threadCount };

            int step = 1;

            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

            var tasks = new List<Task>();
            for (int i = 0; i < threadCount; i++)
            {
                TParam p = param.Copy();
                p.Index = i;

                if (param.ConcurrentMethod == ConcurrentMethod.Order)
                {
                    int stepMax = step * param.OrderIncrease;
                    if (i < stepMax)
                    {
                        p.Sleep = ran.Next((step - 1) * param.OrderInterval, param.OrderInterval * step * 1000);
                    }
                    if (i >= stepMax)
                    {
                        step += 1;//区间升位
                    }
                }
                else if (param.ConcurrentMethod == ConcurrentMethod.Random)
                {
                    p.Sleep = ran.Next(0, param.RandomTimeRange * 1000);
                }

                tasks.Add(Task.Factory.StartNew(TaskProcess, p));

            }

            this.timer1.Enabled = true;

            var d1 = DateTime.Now;

            await Task.WhenAll(tasks);

            //所有请求已完成===================================

            var d2 = DateTime.Now;

            var buf = new StringBuilder();
            buf.AppendLine("MaxTime,MinTime,AvgTime,Total");
            buf.AppendLine(String.Format("{0}ms,{1}ms,{2}ms,{3}ms", _responsetime.Max(), _responsetime.Min(), _responsetime.Average(), (d2 - d1).TotalMilliseconds));
            buf.AppendLine("人员,耗时,开始时间,结束时间,结果");
            buf.AppendLine(string.Join("\r\n", _result));
            this.txtResult.Text = buf.ToString();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (percent != null)
            {
                if (!this.lbPercent.Visible)
                {
                    this.lbPercent.Visible = true;
                }

                if (percent.Completed == percent.Total)
                {
                    this.timer1.Enabled = false;
                    this.lbPercent.Text = "100%";
                    this.lbPercent.Update();
                }
                else
                {
                    this.lbPercent.Text = (Math.Max(0, percent.Completed / percent.Total)) + "%";
                    this.lbPercent.Update();
                }
            }
        }
    }
}
