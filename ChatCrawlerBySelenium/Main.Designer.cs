namespace ChatCrawlerBySelenium
{
    partial class Main
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.LblText1 = new System.Windows.Forms.Label();
            this.LblText2 = new System.Windows.Forms.Label();
            this.LblText3 = new System.Windows.Forms.Label();
            this.ChatTxtBox1 = new System.Windows.Forms.TextBox();
            this.ChatTxtBox2 = new System.Windows.Forms.TextBox();
            this.ChatTxtBox3 = new System.Windows.Forms.TextBox();
            this.LblErr = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LblText1
            // 
            this.LblText1.AutoSize = true;
            this.LblText1.Location = new System.Drawing.Point(26, 87);
            this.LblText1.Name = "LblText1";
            this.LblText1.Size = new System.Drawing.Size(51, 12);
            this.LblText1.TabIndex = 0;
            this.LblText1.Text = "- 주소 : ";
            // 
            // LblText2
            // 
            this.LblText2.AutoSize = true;
            this.LblText2.Location = new System.Drawing.Point(314, 87);
            this.LblText2.Name = "LblText2";
            this.LblText2.Size = new System.Drawing.Size(51, 12);
            this.LblText2.TabIndex = 0;
            this.LblText2.Text = "- 주소 : ";
            // 
            // LblText3
            // 
            this.LblText3.AutoSize = true;
            this.LblText3.Location = new System.Drawing.Point(608, 87);
            this.LblText3.Name = "LblText3";
            this.LblText3.Size = new System.Drawing.Size(51, 12);
            this.LblText3.TabIndex = 0;
            this.LblText3.Text = "- 주소 : ";
            // 
            // ChatTxtBox1
            // 
            this.ChatTxtBox1.Location = new System.Drawing.Point(28, 113);
            this.ChatTxtBox1.Multiline = true;
            this.ChatTxtBox1.Name = "ChatTxtBox1";
            this.ChatTxtBox1.ReadOnly = true;
            this.ChatTxtBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ChatTxtBox1.Size = new System.Drawing.Size(273, 255);
            this.ChatTxtBox1.TabIndex = 1;
            // 
            // ChatTxtBox2
            // 
            this.ChatTxtBox2.Location = new System.Drawing.Point(316, 113);
            this.ChatTxtBox2.Multiline = true;
            this.ChatTxtBox2.Name = "ChatTxtBox2";
            this.ChatTxtBox2.ReadOnly = true;
            this.ChatTxtBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ChatTxtBox2.Size = new System.Drawing.Size(273, 255);
            this.ChatTxtBox2.TabIndex = 1;
            // 
            // ChatTxtBox3
            // 
            this.ChatTxtBox3.Location = new System.Drawing.Point(610, 113);
            this.ChatTxtBox3.Multiline = true;
            this.ChatTxtBox3.Name = "ChatTxtBox3";
            this.ChatTxtBox3.ReadOnly = true;
            this.ChatTxtBox3.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ChatTxtBox3.Size = new System.Drawing.Size(273, 255);
            this.ChatTxtBox3.TabIndex = 1;
            // 
            // LblErr
            // 
            this.LblErr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblErr.AutoSize = true;
            this.LblErr.ForeColor = System.Drawing.Color.Red;
            this.LblErr.Location = new System.Drawing.Point(425, 32);
            this.LblErr.Name = "LblErr";
            this.LblErr.Size = new System.Drawing.Size(59, 12);
            this.LblErr.TabIndex = 2;
            this.LblErr.Text = "- errLog :";
            this.LblErr.Visible = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 386);
            this.Controls.Add(this.LblErr);
            this.Controls.Add(this.ChatTxtBox3);
            this.Controls.Add(this.ChatTxtBox2);
            this.Controls.Add(this.ChatTxtBox1);
            this.Controls.Add(this.LblText3);
            this.Controls.Add(this.LblText2);
            this.Controls.Add(this.LblText1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Main";
            this.Text = "그룹 번호 : ";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblText1;
        private System.Windows.Forms.Label LblText2;
        private System.Windows.Forms.Label LblText3;
        private System.Windows.Forms.TextBox ChatTxtBox1;
        private System.Windows.Forms.TextBox ChatTxtBox2;
        private System.Windows.Forms.TextBox ChatTxtBox3;
        private System.Windows.Forms.Label LblErr;
    }
}

