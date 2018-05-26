using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClientViewerLogin
{
    public partial class ShowHelper : Form
    {
        public ShowHelper()
        {
            InitializeComponent();
        }

        private void ShowHelper_Shown(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
