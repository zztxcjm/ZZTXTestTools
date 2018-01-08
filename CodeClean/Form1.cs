using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeClean
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (String.IsNullOrEmpty(this.textBox1.Text))
            {
                MessageBox.Show("请输入要清理的代码文件目录");
                return;
            }

            var rootfolder = this.textBox1.Text;
            if (!Directory.Exists(rootfolder))
            {
                MessageBox.Show("目录不存在，请重新输入");
                return;
            }

            this.button1.Enabled = false;
            this.button1.Update();

            ExecClean(rootfolder);

            this.button1.Enabled = true;

        }

        private void ExecClean(string rootfolder)
        {


            if (!rootfolder.EndsWith("\\"))
            {
                rootfolder += "\\";
            }

            DeleteFolder(rootfolder, "Controllers");
            DeleteFolder(rootfolder, "Models");
            DeleteFolder(rootfolder, "obj");
            DeleteFolder(rootfolder, "Properties");
            DeleteFolder(rootfolder, "AppCode");
            DeleteFolder(rootfolder, "App_Data");
            DeleteFolder(rootfolder, "App_Start");
            DeleteFolder(rootfolder, "Log");
            DeleteFolder(rootfolder, "TempFile");
            DeleteFolder(rootfolder, "packages");


            DeleteFile(rootfolder, "packages.config");
            DeleteFile(rootfolder, "Web.Debug.config");
            DeleteFile(rootfolder, "Web.Release.config");
            DeleteFile(rootfolder, "*.vspscc");
            DeleteFile(rootfolder, "*.user");
            DeleteFile(rootfolder, "*.csproj");
            DeleteFile(rootfolder, "*.suo");
            DeleteFile(rootfolder, "*.sln");
            DeleteFile(rootfolder, "*.vssscc");
            DeleteFile(rootfolder, "*.cs");
            DeleteFile(rootfolder, "*.xml");
            DeleteFile(rootfolder, "*.pdb");

            if (chkDeleteWebConfig.Checked)
            {
                DeleteFile(rootfolder, "web.config");
            }
            if (chkDeleteClientConfig.Checked)
            {
                DeleteFile(rootfolder, "clients.config");
            }

            //要补充删除的目录
            if (!String.IsNullOrEmpty(this.textBox2.Text))
            {
                var d = this.textBox2.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (d != null && d.Length > 0)
                {
                    foreach (string item in d)
                    {

                        if (item.IndexOf("\\") != -1)
                        {
                            var name = item.Substring(item.LastIndexOf("\\") + 1);
                            var path = item.Substring(0, item.LastIndexOf("\\")).TrimStart('\\');
                            DeleteFolder(rootfolder + path, name);
                        }
                        else
                        {
                            DeleteFolder(rootfolder, item);
                        }

                    }
                }
            }

        }

        private void DeleteFile(string rootfolder, string fileName, bool onlyRoot = false)
        {

            if (String.IsNullOrEmpty(fileName))
                return;

            var d = Directory.GetFiles(rootfolder, fileName, onlyRoot ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
            if (d != null && d.Length > 0)
            {
                foreach (string item in d)
                {
                    try
                    {
                        showMsg("正在删除文件" + item);
                        File.Delete(item);
                    }
                    catch (Exception ex)
                    {
                        showMsg("删除文件“" + item + "”错误。" + ex.Message);
                    }
                }
            }

        }

        private void DeleteFolder(string root, string folder)
        {

            if (String.IsNullOrEmpty(folder))
                return;

            var d = Directory.GetDirectories(root, folder, SearchOption.AllDirectories);
            if (d != null && d.Length > 0)
            {
                foreach (string item in d)
                {
                    try
                    {
                        showMsg("正在删除目录" + item);
                        Directory.Delete(item, true);
                    }
                    catch (Exception ex)
                    {
                        showMsg("删除目录“" + item + "”错误。" + ex.Message);
                    }
                }
            }

        }

        private void showMsg(string m)
        {
            if (!String.IsNullOrEmpty(m))
            {
                this.listBox1.Items.Add(m);
                this.listBox1.Update();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //for (int i = 0; i < 100; i++)
            //{
            //    showMsg(i.ToString());
            //}

            //var s = "x\\y\\z";
            //var s1 = s.Substring(0, s.LastIndexOf("\\"));
            //var s2 = s.Substring(s.LastIndexOf("\\")+1);

            //MessageBox.Show(s1);
            //MessageBox.Show(s2);


        }

    }
}
