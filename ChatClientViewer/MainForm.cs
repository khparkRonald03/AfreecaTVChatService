using System;
using System.Windows.Forms;

namespace ChatClientViewer
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            //webBrowser1.DocumentText = HtmlFormat.BjHtml;
            //webBrowser2.DocumentText = HtmlFormat.KingHtml;
            //webBrowser3.DocumentText = HtmlFormat.BigFanHtml;
            //webBrowser4.DocumentText = HtmlFormat.ChatHtml;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            (new ShowHelper()).ShowDialog();
        }
    }
}
