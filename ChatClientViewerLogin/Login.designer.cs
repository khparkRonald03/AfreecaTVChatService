namespace ChatClientViewerLogin
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.iTalk_ThemeContainer1 = new iTalk.iTalk_ThemeContainer();
            this.CbIdSave = new iTalk.iTalk_CheckBox();
            this.TxtPw = new iTalk.iTalk_TextBox_Big();
            this.TxtId = new iTalk.iTalk_TextBox_Big();
            this.BtnLogin = new iTalk.iTalk_Button_2();
            this.iTalk_ControlBox1 = new iTalk.iTalk_ControlBox();
            this.iTalk_ThemeContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // iTalk_ThemeContainer1
            // 
            this.iTalk_ThemeContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.iTalk_ThemeContainer1.Controls.Add(this.CbIdSave);
            this.iTalk_ThemeContainer1.Controls.Add(this.TxtPw);
            this.iTalk_ThemeContainer1.Controls.Add(this.TxtId);
            this.iTalk_ThemeContainer1.Controls.Add(this.BtnLogin);
            this.iTalk_ThemeContainer1.Controls.Add(this.iTalk_ControlBox1);
            this.iTalk_ThemeContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iTalk_ThemeContainer1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.iTalk_ThemeContainer1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(142)))), ((int)(((byte)(142)))));
            this.iTalk_ThemeContainer1.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.iTalk_ThemeContainer1.Location = new System.Drawing.Point(0, 0);
            this.iTalk_ThemeContainer1.Name = "iTalk_ThemeContainer1";
            this.iTalk_ThemeContainer1.Padding = new System.Windows.Forms.Padding(3, 28, 3, 28);
            this.iTalk_ThemeContainer1.Sizable = true;
            this.iTalk_ThemeContainer1.Size = new System.Drawing.Size(590, 331);
            this.iTalk_ThemeContainer1.SmartBounds = false;
            this.iTalk_ThemeContainer1.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.iTalk_ThemeContainer1.TabIndex = 0;
            this.iTalk_ThemeContainer1.Text = "ABJ v1.1";
            // 
            // CbIdSave
            // 
            this.CbIdSave.BackColor = System.Drawing.Color.Transparent;
            this.CbIdSave.Checked = false;
            this.CbIdSave.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.CbIdSave.Location = new System.Drawing.Point(391, 247);
            this.CbIdSave.Name = "CbIdSave";
            this.CbIdSave.Size = new System.Drawing.Size(98, 15);
            this.CbIdSave.TabIndex = 4;
            this.CbIdSave.Text = "아이디 저장";
            // 
            // TxtPw
            // 
            this.TxtPw.BackColor = System.Drawing.Color.Transparent;
            this.TxtPw.Font = new System.Drawing.Font("Tahoma", 11F);
            this.TxtPw.ForeColor = System.Drawing.Color.DimGray;
            this.TxtPw.Image = null;
            this.TxtPw.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.TxtPw.Location = new System.Drawing.Point(102, 127);
            this.TxtPw.MaxLength = 32767;
            this.TxtPw.Multiline = false;
            this.TxtPw.Name = "TxtPw";
            this.TxtPw.ReadOnly = false;
            this.TxtPw.Size = new System.Drawing.Size(387, 41);
            this.TxtPw.TabIndex = 2;
            this.TxtPw.Text = "Password";
            this.TxtPw.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.TxtPw.UseSystemPasswordChar = true;
            this.TxtPw.Enter += new System.EventHandler(this.TxtPw_Enter);
            this.TxtPw.Leave += new System.EventHandler(this.TxtPw_Leave);
            // 
            // TxtId
            // 
            this.TxtId.BackColor = System.Drawing.Color.Transparent;
            this.TxtId.Font = new System.Drawing.Font("Tahoma", 11F);
            this.TxtId.ForeColor = System.Drawing.Color.DimGray;
            this.TxtId.Image = null;
            this.TxtId.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.TxtId.Location = new System.Drawing.Point(102, 76);
            this.TxtId.MaxLength = 32767;
            this.TxtId.Multiline = false;
            this.TxtId.Name = "TxtId";
            this.TxtId.ReadOnly = false;
            this.TxtId.Size = new System.Drawing.Size(387, 41);
            this.TxtId.TabIndex = 1;
            this.TxtId.Text = "ID";
            this.TxtId.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.TxtId.UseSystemPasswordChar = false;
            this.TxtId.Enter += new System.EventHandler(this.TxtId_Enter);
            this.TxtId.Leave += new System.EventHandler(this.TxtId_Leave);
            // 
            // BtnLogin
            // 
            this.BtnLogin.BackColor = System.Drawing.Color.Transparent;
            this.BtnLogin.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.BtnLogin.ForeColor = System.Drawing.Color.White;
            this.BtnLogin.Image = null;
            this.BtnLogin.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnLogin.Location = new System.Drawing.Point(102, 185);
            this.BtnLogin.Name = "BtnLogin";
            this.BtnLogin.Size = new System.Drawing.Size(387, 52);
            this.BtnLogin.TabIndex = 3;
            this.BtnLogin.Text = "로그인";
            this.BtnLogin.TextAlignment = System.Drawing.StringAlignment.Center;
            this.BtnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            this.BtnLogin.KeyUp += new System.Windows.Forms.KeyEventHandler(this.BtnLogin_KeyUp);
            // 
            // iTalk_ControlBox1
            // 
            this.iTalk_ControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iTalk_ControlBox1.BackColor = System.Drawing.Color.Transparent;
            this.iTalk_ControlBox1.Location = new System.Drawing.Point(509, -1);
            this.iTalk_ControlBox1.Name = "iTalk_ControlBox1";
            this.iTalk_ControlBox1.Size = new System.Drawing.Size(77, 19);
            this.iTalk_ControlBox1.TabIndex = 0;
            this.iTalk_ControlBox1.Text = "iTalk_ControlBox1";
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 331);
            this.Controls.Add(this.iTalk_ThemeContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.MaximumSize = new System.Drawing.Size(590, 331);
            this.MinimumSize = new System.Drawing.Size(590, 331);
            this.Name = "Login";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ABJ v1.1";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.Load += new System.EventHandler(this.Login_Load);
            this.Shown += new System.EventHandler(this.Login_Shown);
            this.iTalk_ThemeContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private iTalk.iTalk_ThemeContainer iTalk_ThemeContainer1;
        private iTalk.iTalk_ControlBox iTalk_ControlBox1;
        private iTalk.iTalk_TextBox_Big TxtPw;
        private iTalk.iTalk_TextBox_Big TxtId;
        private iTalk.iTalk_Button_2 BtnLogin;
        private iTalk.iTalk_CheckBox CbIdSave;
    }
}