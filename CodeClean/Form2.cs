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
using System.Text.RegularExpressions;

namespace CodeClean
{
    public partial class Form2 : Form
    {

        const string _myselfName = "myself.config";
        const string _cleanConfigExtName = ".xconfig";

        private string myselfConfigFilePath = String.Empty;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            //读配置
            myselfConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _myselfName);
            if (File.Exists(myselfConfigFilePath))
            {
                var content = File.ReadAllText(myselfConfigFilePath);
                if (!String.IsNullOrEmpty(myselfConfigFilePath))
                    this.textBox1.Text = content;
            }

            //加载清理配置
            var fs = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*" + _cleanConfigExtName, SearchOption.AllDirectories);
            if (fs != null && fs.Length > 0)
            {
                foreach (var item in fs)
                {
                    this.comboBox1.Items.Add(item.Substring(item.LastIndexOf("\\") + 1));
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            var rootPath = this.textBox1.Text;
            var configName = this.comboBox1.SelectedItem == null ? String.Empty : this.comboBox1.SelectedItem.ToString();

            #region 验证前置条件

            if (String.IsNullOrEmpty(rootPath))
            {
                MessageBox.Show("代码文件根目录不能为空");
                return;
            }

            if (String.IsNullOrEmpty(configName))
            {
                MessageBox.Show("请选择清理配置");
                return;
            }

            var configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configName);
            if (!File.Exists(configFile))
            {
                MessageBox.Show("无效的配置文件，请确定配置文件是否存在");
                return;
            }

            #endregion

            this.listBox1.Items.Clear();
            this.listBox1.Update();

            appendMsg("处理开始...");

            this.button2.Enabled = false;

            ExecClean(rootPath, configFile);

            this.button2.Enabled = true;

            appendMsg("处理已完成");

        }

        private void ExecClean(string rootPath, string configFile)
        {
            var configItems = File.ReadAllLines(configFile);
            if (configItems != null && configItems.Length > 0)
            {
                foreach (var item in configItems)
                {
                    if (!String.IsNullOrEmpty(item) && !item.StartsWith("#"))
                    {

                        ProcessCmdLine(rootPath, item);

                    }
                }
            }
        }

        private void ProcessCmdLine(string rootPath, string item)
        {

            if (String.IsNullOrEmpty(item))
                return;

            if (item.StartsWith("/D"))//删除整个目录
            {
                var tmpArr = item.Split(' ');
                if (tmpArr.Length == 2)
                {
                    DeleteFolder(rootPath, tmpArr[1]);
                }
                else if (tmpArr.Length == 3)
                {
                    DeleteFolder(rootPath, tmpArr[1], tmpArr[2]);
                }

            }
            else if (item.StartsWith("/F"))//删除指定文件
            {
                var tmpArr = item.Split(' ');
                if (tmpArr.Length == 2)
                {
                    DeleteFile(rootPath, tmpArr[1]);
                }
                else if (tmpArr.Length == 3)
                {
                    DeleteFile(rootPath, tmpArr[1], tmpArr[2]);
                }
            }
            else if (item.StartsWith("/M"))//重新拷贝文件
            {
            }

        }

        private void CopyDirectory(string srcDir, string tgtDir)
        {
            DirectoryInfo source = new DirectoryInfo(srcDir);
            DirectoryInfo target = new DirectoryInfo(tgtDir);

            if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("父目录不能拷贝到子目录！");
            }

            if (!source.Exists)
            {
                return;
            }

            if (!target.Exists)
            {
                target.Create();
            }

            FileInfo[] files = source.GetFiles();

            for (int i = 0; i < files.Length; i++)
            {
                File.Copy(files[i].FullName, target.FullName + @"\" + files[i].Name, true);
            }

            DirectoryInfo[] dirs = source.GetDirectories();

            for (int j = 0; j < dirs.Length; j++)
            {
                CopyDirectory(dirs[j].FullName, target.FullName + @"\" + dirs[j].Name);
            }
        }

        private Regex _rep = new Regex("\\\\{2,}", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        private void DeleteFile(string rootfolder, string fileName, string extArg = null)
        {
            string[] d;

            try
            {
                d = Directory.GetFiles(rootfolder, fileName, SearchOption.AllDirectories);
            }
            catch (Exception)
            {
                d = null;
            }


            if (d != null && d.Length > 0)
            {

                var removeF = new List<string>();
                if (!String.IsNullOrEmpty(extArg))
                {
                    if (extArg.StartsWith("!"))
                    {
                        var tmp = extArg.Replace("!", "");
                        var tmp2 = tmp.Split('|');
                        foreach (var i in tmp2)
                        {
                            var tmp3 = i.Contains('*') ? i : "\\" + i + "\\" + fileName;
                            tmp3 = _rep.Replace(tmp3.Replace("/", "\\").Replace("*", ""), "\\");
                            removeF.Add(tmp3);
                        }
                    }
                }

                foreach (string item in d)
                {
                    if (File.Exists(item))
                    {

                        var rst = from it in removeF where item.ToLower().IndexOf(it.ToLower()) != -1 select item;
                        if (rst.Count() > 0)
                        {
                            appendMsg("忽略文件" + item);
                            continue;//排除此路径的处理
                        }


                        try
                        {
                            appendMsg("删除文件" + item);
                            File.Delete(item);
                        }
                        catch (Exception ex)
                        {
                            appendMsg("删除文件“" + item + "”错误。" + ex.Message);
                        }
                    }
                }
            }

        }

        private void DeleteFolder(string root, string folder, string extArg = null)
        {

            if (!root.EndsWith("\\"))
                root = root + "\\";

            //多级目录
            if (folder.IndexOf('\\') != -1)
            {
                root += folder.Substring(0, folder.LastIndexOf("\\"));
                folder = folder.Substring(folder.LastIndexOf("\\") + 1);
            }

            string[] d;

            try
            {
                d = Directory.GetDirectories(root, folder, SearchOption.AllDirectories);
            }
            catch (Exception)
            {
                d = null;
            }

            if (d != null && d.Length > 0)
            {

                var removeF = new List<string>();
                if (!String.IsNullOrEmpty(extArg))
                {
                    if (extArg.StartsWith("!"))
                    {
                        var tmp = extArg.Replace("!", "");
                        var tmp2 = tmp.Split('|');
                        foreach (var i in tmp2)
                        {
                            var tmp3 = i.Contains('*') ? i : "\\" + i + "\\" + folder;
                            tmp3 = _rep.Replace(i.Replace("/", "\\").Replace("*", ""), "\\");
                            removeF.Add(tmp3);
                        }
                    }
                }

                foreach (string item in d)
                {
                    if (Directory.Exists(item))
                    {

                        var rst = from it in removeF where item.ToLower().IndexOf(it.ToLower()) != -1 select item;
                        if (rst.Count() > 0)
                        {
                            appendMsg("忽略目录" + item);
                            continue;//排除此路径的处理
                        }


                        try
                        {
                            appendMsg("删除目录" + item);
                            Directory.Delete(item, true);
                        }
                        catch (Exception ex)
                        {
                            appendMsg("删除目录“" + item + "”错误。" + ex.Message);
                        }

                    }
                }

            }


        }

        private void appendMsg(string m)
        {
            if (!String.IsNullOrEmpty(m))
            {
                this.listBox1.Items.Add(m);
                this.listBox1.Update();

                this.listBox1.SelectedIndex = this.listBox1.Items.Count - 1;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var re = this.folderBrowserDialog1.ShowDialog();
            if (re == DialogResult.OK)
            {
                var path = this.folderBrowserDialog1.SelectedPath;
                if (!String.IsNullOrEmpty(path))
                {
                    this.textBox1.Text = path;
                    File.WriteAllText(myselfConfigFilePath, path);
                }
            }
        }


    }
}
