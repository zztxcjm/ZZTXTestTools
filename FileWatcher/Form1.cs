using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileWatcher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private IDictionary<string, FileSystemWatcher> _watcher = new Dictionary<string, FileSystemWatcher>();
        private List<string> _changelist = new List<string>();

        private FileSystemWatcher GetWatcher(string folder)
        {

            if (String.IsNullOrEmpty(folder))
            {
                return null;
            }

            FileSystemWatcher o;
            if (_watcher.TryGetValue(folder, out o))
            {
                return o;
            }
            else
            {
                var re = new FileSystemWatcher();
                re.Path = folder;
                re.IncludeSubdirectories = true;

                re.Changed += fileSystemWatcher1_CCDEvent;
                re.Created += fileSystemWatcher1_CCDEvent;
                re.Deleted += fileSystemWatcher1_CCDEvent;

                _watcher.Add(folder, re);

                return re;

            }

        }

        private void fileSystemWatcher1_CCDEvent(object sender, System.IO.FileSystemEventArgs e)
        {

            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    CreateFile(e.FullPath);
                });
            }
            else if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    DeleteFile(e.FullPath);
                });
            }
            else if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                if (!_changelist.Contains(e.FullPath))
                {
                    _changelist.Add(e.FullPath);

                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        ChangeFile(e.FullPath);
                        _changelist.Remove(e.FullPath);
                    });

                }
            }

        }

        private void CreateFile(string fullpath)
        {
            this.listBox1.Items.Add("创建文件" + fullpath);
        }
        private void DeleteFile(string fullpath)
        {
            this.listBox1.Items.Add("删除文件" + fullpath);
        }
        private void ChangeFile(string fullpath)
        {
            this.listBox1.Items.Add("更改文件" + fullpath);
        }

        private void StartWatcher(IEnumerable<string> folders)
        {

            if (folders == null)
                return;

            foreach (var f in folders)
            {
                var w = GetWatcher(f);
                if (w != null)
                {
                    w.EnableRaisingEvents = true;
                }
            }

        }

        private void StopWatcher()
        {
            foreach (FileSystemWatcher w in _watcher.Values)
            {
                w.EnableRaisingEvents = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            //启动监视   
            var folder = System.Configuration.ConfigurationManager.AppSettings["WatchFolder"];
            if (String.IsNullOrEmpty(folder))
            {
                listBox1.Items.Clear();
                listBox1.Items.Add("配置项“WatchFolder”为空，检视未启动。");
                return;
            }

            var folderlist = folder.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (folderlist.Length == 0)
            {
                listBox1.Items.Clear();
                listBox1.Items.Add("配置项“WatchFolder”为空，检视未启动。");
                return;
            }

            StartWatcher(folderlist);

            listBox1.Items.Clear();
            listBox1.Items.Add("检视已启动");
            foreach (var f in folderlist)
            {
                listBox1.Items.Add("正在检视目录" + f);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {

            //停止检视
            StopWatcher();

            listBox1.Items.Clear();
            listBox1.Items.Add("检视已停止");

        }
    }
}
