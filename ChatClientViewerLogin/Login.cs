using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClientViewerLogin
{
    public partial class Login : Form
    {
        const string IdText = "ID";

        const string PwText = "Password";

        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            (new ShowHelper()).ShowDialog();

            var idSave = GetConfigData("CbIdSave");
            if (idSave == true.ToString())
            {
                CbIdSave.Checked = true;
                var id = GetConfigData("TxtId");
                if (!string.IsNullOrEmpty(id))
                    TxtId.Text = id;
                else
                    TxtId.Text = IdText;
            }
            else
            {
                TxtId.Text = IdText;
            }
            
            TxtPw.Text = PwText;
            TxtId.Focus();
        }

        private void TxtId_Enter(object sender, EventArgs e)
        {
            TxtId.ImeMode = ImeMode.Alpha;

            if (TxtId.Text == IdText)
            {
                TxtId.Text = string.Empty;
            }

            TxtId.ImeMode = ImeMode.Alpha;
        }

        private void TxtId_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtId.Text))
            {
                TxtId.Text = IdText;
            }
        }

        private void TxtPw_Enter(object sender, EventArgs e)
        {
            if (TxtPw.Text == PwText)
            {
                TxtPw.Text = string.Empty;
            }
        }

        private void TxtPw_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtPw.Text))
            {
                TxtPw.Text = PwText;
            }
        }

        private void BtnLogin_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
                LoginProc();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (CbIdSave.Checked)
            {
                SetConfigData("CbIdSave", CbIdSave.Checked.ToString());
                SetConfigData("TxtId", TxtId.Text);
            }
            LoginProc();
        }

        private void LoginProc()
        {
            if (TxtId.Text == IdText)
            {
                MessageBoxEx.Show("아이디를 입력하여주십시오.", "아이디 입력", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (TxtPw.Text == PwText)
            {
                MessageBoxEx.Show("비밀번호를 입력하여주십시오.", "비밀번호 입력", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string id = TxtId.Text;
            string pw = TxtPw.Text;

            if (TxtId.Text == IdText)
                id = string.Empty;

            if (TxtPw.Text == PwText)
                pw = string.Empty;

            DoLogin(id, pw);
        }

        private void DoLogin(string id, string pw)
        {
            string argu = $"{id} {pw} {ChkDoNotLogin.Checked.ToString()}";

            using (Process ps = new Process())
            {
                ps.StartInfo.FileName = "ChatClientViewer.exe";
                ps.StartInfo.Arguments = argu;
                ps.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                ps.Start();
            }

            this.Close();
        }

        #region App.config 수정 / 가져오기

        /// <summary>
        /// App.config 파일 설정 데이터 가져오기
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetConfigData(string key)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                string value = ConfigurationManager.AppSettings[key];
                return value;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// App.config 파일 설정 데이터 수정
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool SetConfigData(string key, string value)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                config.AppSettings.Settings[key].Value = value;

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(key);

                return true;
            }
            catch
            {
                return false;
            }
        }


        #endregion

        private void Login_Shown(object sender, EventArgs e)
        {
            TxtId.ImeMode = ImeMode.Alpha;
        }
    }
}
