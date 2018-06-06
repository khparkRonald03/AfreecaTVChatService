namespace ChatClientViewer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.ITContainer = new iTalk.iTalk_ThemeContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.iTalk_ControlBox1 = new iTalk.iTalk_ControlBox();
            this.BtnReStart = new iTalk.iTalk_Button_1();
            this.ITContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "bul_king.gif");
            this.imageList.Images.SetKeyName(1, "bul_red_heart.gif");
            this.imageList.Images.SetKeyName(2, "bul_yellow_heart.gif");
            this.imageList.Images.SetKeyName(3, "bul_gray_heart.gif");
            this.imageList.Images.SetKeyName(4, "bul_s_on.gif");
            // 
            // ITContainer
            // 
            this.ITContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.ITContainer.Controls.Add(this.splitContainer1);
            this.ITContainer.Controls.Add(this.BtnReStart);
            this.ITContainer.Controls.Add(this.iTalk_ControlBox1);
            this.ITContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ITContainer.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.ITContainer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(142)))), ((int)(((byte)(142)))));
            this.ITContainer.Location = new System.Drawing.Point(0, 0);
            this.ITContainer.Name = "ITContainer";
            this.ITContainer.Padding = new System.Windows.Forms.Padding(3, 28, 3, 28);
            this.ITContainer.Sizable = true;
            this.ITContainer.Size = new System.Drawing.Size(340, 877);
            this.ITContainer.SmartBounds = false;
            this.ITContainer.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.ITContainer.TabIndex = 0;
            this.ITContainer.Text = "ABJ v1.1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 28);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Size = new System.Drawing.Size(334, 790);
            this.splitContainer1.SplitterDistance = 361;
            this.splitContainer1.TabIndex = 6;
            // 
            // iTalk_ControlBox1
            // 
            this.iTalk_ControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iTalk_ControlBox1.BackColor = System.Drawing.Color.Transparent;
            this.iTalk_ControlBox1.Location = new System.Drawing.Point(259, -1);
            this.iTalk_ControlBox1.Name = "iTalk_ControlBox1";
            this.iTalk_ControlBox1.Size = new System.Drawing.Size(77, 19);
            this.iTalk_ControlBox1.TabIndex = 0;
            this.iTalk_ControlBox1.Text = "iTalk_ControlBox1";
            // 
            // BtnReStart
            // 
            this.BtnReStart.BackColor = System.Drawing.Color.Transparent;
            this.BtnReStart.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BtnReStart.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.BtnReStart.Image = null;
            this.BtnReStart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnReStart.Location = new System.Drawing.Point(3, 818);
            this.BtnReStart.Name = "BtnReStart";
            this.BtnReStart.Size = new System.Drawing.Size(334, 31);
            this.BtnReStart.TabIndex = 7;
            this.BtnReStart.Text = "재시작";
            this.BtnReStart.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 877);
            this.Controls.Add(this.ITContainer);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(126, 39);
            this.Name = "Main";
            this.Text = "ABJ v1.1";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.ITContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private iTalk.iTalk_ThemeContainer ITContainer;
        private System.Windows.Forms.ImageList imageList;
        private iTalk.iTalk_ControlBox iTalk_ControlBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private iTalk.iTalk_Button_1 BtnReStart;
    }
}

