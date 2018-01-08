using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RenameTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var files = System.IO.Directory.GetFiles(this.textBox1.Text, "*.min.*", SearchOption.AllDirectories);
            foreach (var f in files)
            {
                var oldf = f.Replace(".min.", ".");
                File.Delete(oldf);
                File.Move(f, oldf);

                this.listBox1.Items.Add(f);
                this.listBox1.Update();

            }

            MessageBox.Show("处理完成");

        }
    }
}
