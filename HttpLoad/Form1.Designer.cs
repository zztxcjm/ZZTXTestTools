namespace HttpLoad
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtThreadCount = new System.Windows.Forms.TextBox();
            this.btStart = new System.Windows.Forms.Button();
            this.txtTimeout = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lstHttpMethod = new System.Windows.Forms.ComboBox();
            this.btOpenExcel = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPostData = new System.Windows.Forms.TextBox();
            this.txtHttpHeader = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lstConcurrentMethod = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbRandomTimeRange = new System.Windows.Forms.Label();
            this.txtRandomTimeRange = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbOrderIncrease = new System.Windows.Forms.Label();
            this.lbOrderInterval = new System.Windows.Forms.Label();
            this.txtOrderIncrease = new System.Windows.Forms.TextBox();
            this.txtOrderInterval = new System.Windows.Forms.TextBox();
            this.lbPercent = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(24, 37);
            this.txtUrl.Multiline = true;
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtUrl.Size = new System.Drawing.Size(671, 50);
            this.txtUrl.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "URL(必填)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 362);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "测试结果";
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(24, 380);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResult.Size = new System.Drawing.Size(671, 139);
            this.txtResult.TabIndex = 100;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(724, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "人数";
            // 
            // txtThreadCount
            // 
            this.txtThreadCount.Location = new System.Drawing.Point(726, 37);
            this.txtThreadCount.Name = "txtThreadCount";
            this.txtThreadCount.Size = new System.Drawing.Size(135, 21);
            this.txtThreadCount.TabIndex = 3;
            this.txtThreadCount.Text = "100";
            // 
            // btStart
            // 
            this.btStart.Location = new System.Drawing.Point(24, 536);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(75, 23);
            this.btStart.TabIndex = 10;
            this.btStart.Text = "开始";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // txtTimeout
            // 
            this.txtTimeout.Location = new System.Drawing.Point(726, 101);
            this.txtTimeout.Name = "txtTimeout";
            this.txtTimeout.Size = new System.Drawing.Size(135, 21);
            this.txtTimeout.TabIndex = 4;
            this.txtTimeout.Text = "30";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(724, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "请求超时(秒)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(724, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "请求方式";
            // 
            // lstHttpMethod
            // 
            this.lstHttpMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstHttpMethod.FormattingEnabled = true;
            this.lstHttpMethod.Items.AddRange(new object[] {
            "GET",
            "POST"});
            this.lstHttpMethod.Location = new System.Drawing.Point(726, 162);
            this.lstHttpMethod.Name = "lstHttpMethod";
            this.lstHttpMethod.Size = new System.Drawing.Size(135, 20);
            this.lstHttpMethod.TabIndex = 5;
            // 
            // btOpenExcel
            // 
            this.btOpenExcel.Enabled = false;
            this.btOpenExcel.Location = new System.Drawing.Point(584, 536);
            this.btOpenExcel.Name = "btOpenExcel";
            this.btOpenExcel.Size = new System.Drawing.Size(111, 23);
            this.btOpenExcel.TabIndex = 11;
            this.btOpenExcel.Text = "在Excel中查看结果";
            this.btOpenExcel.UseVisualStyleBackColor = true;
            this.btOpenExcel.Click += new System.EventHandler(this.btOpenExcel_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 101);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "PostData";
            // 
            // txtPostData
            // 
            this.txtPostData.Location = new System.Drawing.Point(24, 119);
            this.txtPostData.Multiline = true;
            this.txtPostData.Name = "txtPostData";
            this.txtPostData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPostData.Size = new System.Drawing.Size(671, 100);
            this.txtPostData.TabIndex = 1;
            // 
            // txtHttpHeader
            // 
            this.txtHttpHeader.Location = new System.Drawing.Point(24, 250);
            this.txtHttpHeader.Multiline = true;
            this.txtHttpHeader.Name = "txtHttpHeader";
            this.txtHttpHeader.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtHttpHeader.Size = new System.Drawing.Size(671, 100);
            this.txtHttpHeader.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 232);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "HttpHeadr";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(724, 195);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 1;
            this.label8.Text = "并发方式";
            // 
            // lstConcurrentMethod
            // 
            this.lstConcurrentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstConcurrentMethod.FormattingEnabled = true;
            this.lstConcurrentMethod.Location = new System.Drawing.Point(726, 218);
            this.lstConcurrentMethod.Name = "lstConcurrentMethod";
            this.lstConcurrentMethod.Size = new System.Drawing.Size(135, 20);
            this.lstConcurrentMethod.TabIndex = 6;
            this.lstConcurrentMethod.SelectedIndexChanged += new System.EventHandler(this.lstConcurrentMethod_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbRandomTimeRange);
            this.panel1.Controls.Add(this.txtRandomTimeRange);
            this.panel1.Location = new System.Drawing.Point(726, 250);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(146, 52);
            this.panel1.TabIndex = 101;
            this.panel1.Visible = false;
            // 
            // lbRandomTimeRange
            // 
            this.lbRandomTimeRange.AutoSize = true;
            this.lbRandomTimeRange.Location = new System.Drawing.Point(-2, 0);
            this.lbRandomTimeRange.Name = "lbRandomTimeRange";
            this.lbRandomTimeRange.Size = new System.Drawing.Size(137, 12);
            this.lbRandomTimeRange.TabIndex = 102;
            this.lbRandomTimeRange.Text = "在时间范围内随机（秒）";
            // 
            // txtRandomTimeRange
            // 
            this.txtRandomTimeRange.Location = new System.Drawing.Point(0, 24);
            this.txtRandomTimeRange.Name = "txtRandomTimeRange";
            this.txtRandomTimeRange.Size = new System.Drawing.Size(135, 21);
            this.txtRandomTimeRange.TabIndex = 103;
            this.txtRandomTimeRange.Text = "30";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbOrderIncrease);
            this.panel2.Controls.Add(this.lbOrderInterval);
            this.panel2.Controls.Add(this.txtOrderIncrease);
            this.panel2.Controls.Add(this.txtOrderInterval);
            this.panel2.Location = new System.Drawing.Point(726, 308);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(146, 113);
            this.panel2.TabIndex = 102;
            this.panel2.Visible = false;
            // 
            // lbOrderIncrease
            // 
            this.lbOrderIncrease.AutoSize = true;
            this.lbOrderIncrease.Location = new System.Drawing.Point(-2, 56);
            this.lbOrderIncrease.Name = "lbOrderIncrease";
            this.lbOrderIncrease.Size = new System.Drawing.Size(65, 12);
            this.lbOrderIncrease.TabIndex = 10;
            this.lbOrderIncrease.Text = "增长请求数";
            // 
            // lbOrderInterval
            // 
            this.lbOrderInterval.AutoSize = true;
            this.lbOrderInterval.Location = new System.Drawing.Point(-2, 0);
            this.lbOrderInterval.Name = "lbOrderInterval";
            this.lbOrderInterval.Size = new System.Drawing.Size(113, 12);
            this.lbOrderInterval.TabIndex = 11;
            this.lbOrderInterval.Text = "间隔时间递增（秒）";
            // 
            // txtOrderIncrease
            // 
            this.txtOrderIncrease.Location = new System.Drawing.Point(0, 80);
            this.txtOrderIncrease.Name = "txtOrderIncrease";
            this.txtOrderIncrease.Size = new System.Drawing.Size(135, 21);
            this.txtOrderIncrease.TabIndex = 13;
            this.txtOrderIncrease.Text = "20";
            // 
            // txtOrderInterval
            // 
            this.txtOrderInterval.Location = new System.Drawing.Point(0, 24);
            this.txtOrderInterval.Name = "txtOrderInterval";
            this.txtOrderInterval.Size = new System.Drawing.Size(135, 21);
            this.txtOrderInterval.TabIndex = 12;
            this.txtOrderInterval.Text = "5";
            // 
            // lbPercent
            // 
            this.lbPercent.AutoSize = true;
            this.lbPercent.Location = new System.Drawing.Point(105, 541);
            this.lbPercent.Name = "lbPercent";
            this.lbPercent.Size = new System.Drawing.Size(17, 12);
            this.lbPercent.TabIndex = 103;
            this.lbPercent.Text = "0%";
            this.lbPercent.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 592);
            this.Controls.Add(this.lbPercent);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lstConcurrentMethod);
            this.Controls.Add(this.lstHttpMethod);
            this.Controls.Add(this.btOpenExcel);
            this.Controls.Add(this.btStart);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.txtTimeout);
            this.Controls.Add(this.txtThreadCount);
            this.Controls.Add(this.txtHttpHeader);
            this.Controls.Add(this.txtPostData);
            this.Controls.Add(this.txtUrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "HttpLoad";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtThreadCount;
        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.TextBox txtTimeout;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox lstHttpMethod;
        private System.Windows.Forms.Button btOpenExcel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPostData;
        private System.Windows.Forms.TextBox txtHttpHeader;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox lstConcurrentMethod;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbRandomTimeRange;
        private System.Windows.Forms.TextBox txtRandomTimeRange;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lbOrderIncrease;
        private System.Windows.Forms.Label lbOrderInterval;
        private System.Windows.Forms.TextBox txtOrderIncrease;
        private System.Windows.Forms.TextBox txtOrderInterval;
        private System.Windows.Forms.Label lbPercent;
        private System.Windows.Forms.Timer timer1;
    }
}

