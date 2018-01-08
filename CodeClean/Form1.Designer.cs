namespace CodeClean
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.chkDeleteWebConfig = new System.Windows.Forms.CheckBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkDeleteClientConfig = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(760, 21);
            this.textBox1.TabIndex = 0;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(12, 39);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(760, 280);
            this.listBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(602, 494);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(170, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "开始清理";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkDeleteWebConfig
            // 
            this.chkDeleteWebConfig.AutoSize = true;
            this.chkDeleteWebConfig.Location = new System.Drawing.Point(14, 494);
            this.chkDeleteWebConfig.Name = "chkDeleteWebConfig";
            this.chkDeleteWebConfig.Size = new System.Drawing.Size(132, 16);
            this.chkDeleteWebConfig.TabIndex = 3;
            this.chkDeleteWebConfig.Text = "是否删除Web.config";
            this.chkDeleteWebConfig.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 382);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(758, 106);
            this.textBox2.TabIndex = 4;
            this.textBox2.Text = "SwfUpload,ueditor,ligerUI,ligerUI123,datepicker,highcharts,AppPack,AppProxy,Servi" +
    "ces\\\\WebApi\\\\Views,Services\\\\WebApi\\\\Areas,Services\\IMServices\\Cert,Services\\IMS" +
    "ervices\\WebSocket,Web\\MobileApi\\Areas";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 358);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(257, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "要补充删除的目录名字，多个之间用“，”分割";
            // 
            // chkDeleteClientConfig
            // 
            this.chkDeleteClientConfig.AutoSize = true;
            this.chkDeleteClientConfig.Location = new System.Drawing.Point(152, 494);
            this.chkDeleteClientConfig.Name = "chkDeleteClientConfig";
            this.chkDeleteClientConfig.Size = new System.Drawing.Size(156, 16);
            this.chkDeleteClientConfig.TabIndex = 3;
            this.chkDeleteClientConfig.Text = "是否删除clients.config";
            this.chkDeleteClientConfig.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.chkDeleteClientConfig);
            this.Controls.Add(this.chkDeleteWebConfig);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "代码清理工具";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chkDeleteWebConfig;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkDeleteClientConfig;
    }
}

