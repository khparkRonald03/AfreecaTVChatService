using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatCrawlerManager_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var textArray = textBox1.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            string argu = string.Empty;
            for (int Idx = 0, group = 1; Idx < textArray.Length; Idx++, group++)
            {
                if (group <= 3)
                {
                    if (argu == string.Empty)
                        argu = "0";

                    argu += $" {textArray[Idx]}";
                    if (group == 3)
                    {
                        group = 0;
                        using (Process ps = new Process())
                        {
                            ps.StartInfo.FileName = "ChatCrawlerBySelenium.exe";
                            ps.StartInfo.Arguments = argu;
                            ps.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                            ps.Start();
                        }
                        Thread.Sleep(3000);
                        argu = string.Empty;
                    }
                    else if (Idx + 1 == textArray.Length && !string.IsNullOrEmpty(argu))
                    {
                        using (Process ps = new Process())
                        {
                            ps.StartInfo.FileName = "ChatCrawlerBySelenium.exe";
                            ps.StartInfo.Arguments = argu;
                            ps.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                            ps.Start();
                        }
                        Thread.Sleep(3000);
                    }
                }
                
            }
        }
    }
}
