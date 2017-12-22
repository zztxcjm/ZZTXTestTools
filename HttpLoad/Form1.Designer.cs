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
            this.btOpenChart = new System.Windows.Forms.Button();
            this.txtHttpHeader = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lstConcurrentMethod = new System.Windows.Forms.ComboBox();
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
            this.txtResult.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(744, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "人数(必填)";
            // 
            // txtThreadCount
            // 
            this.txtThreadCount.Location = new System.Drawing.Point(746, 37);
            this.txtThreadCount.Name = "txtThreadCount";
            this.txtThreadCount.Size = new System.Drawing.Size(117, 21);
            this.txtThreadCount.TabIndex = 3;
            this.txtThreadCount.Text = "100";
            // 
            // btStart
            // 
            this.btStart.Location = new System.Drawing.Point(24, 536);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(75, 23);
            this.btStart.TabIndex = 2;
            this.btStart.Text = "开始";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // txtTimeout
            // 
            this.txtTimeout.Location = new System.Drawing.Point(746, 101);
            this.txtTimeout.Name = "txtTimeout";
            this.txtTimeout.Size = new System.Drawing.Size(117, 21);
            this.txtTimeout.TabIndex = 4;
            this.txtTimeout.Text = "30";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(744, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "超时秒数(必填)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(744, 140);
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
            this.lstHttpMethod.Location = new System.Drawing.Point(746, 162);
            this.lstHttpMethod.Name = "lstHttpMethod";
            this.lstHttpMethod.Size = new System.Drawing.Size(117, 20);
            this.lstHttpMethod.TabIndex = 5;
            // 
            // btOpenExcel
            // 
            this.btOpenExcel.Location = new System.Drawing.Point(502, 536);
            this.btOpenExcel.Name = "btOpenExcel";
            this.btOpenExcel.Size = new System.Drawing.Size(111, 23);
            this.btOpenExcel.TabIndex = 2;
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
            // btOpenChart
            // 
            this.btOpenChart.Location = new System.Drawing.Point(619, 536);
            this.btOpenChart.Name = "btOpenChart";
            this.btOpenChart.Size = new System.Drawing.Size(79, 23);
            this.btOpenChart.TabIndex = 2;
            this.btOpenChart.Text = "查看图表";
            this.btOpenChart.UseVisualStyleBackColor = true;
            this.btOpenChart.Click += new System.EventHandler(this.btOpenChart_Click);
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
            this.label8.Location = new System.Drawing.Point(744, 195);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 1;
            this.label8.Text = "并发方式";
            // 
            // lstConcurrentMethod
            // 
            this.lstConcurrentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstConcurrentMethod.FormattingEnabled = true;
            this.lstConcurrentMethod.Location = new System.Drawing.Point(746, 218);
            this.lstConcurrentMethod.Name = "lstConcurrentMethod";
            this.lstConcurrentMethod.Size = new System.Drawing.Size(117, 20);
            this.lstConcurrentMethod.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 592);
            this.Controls.Add(this.lstConcurrentMethod);
            this.Controls.Add(this.lstHttpMethod);
            this.Controls.Add(this.btOpenChart);
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
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
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
        private System.Windows.Forms.Button btOpenChart;
        private System.Windows.Forms.TextBox txtHttpHeader;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox lstConcurrentMethod;
    }
}

