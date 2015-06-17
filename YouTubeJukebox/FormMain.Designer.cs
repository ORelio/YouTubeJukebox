namespace YouTubeJukebox
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.MediaPlayerGroupBox = new System.Windows.Forms.GroupBox();
            this.MediaPlayerIcon = new System.Windows.Forms.PictureBox();
            this.MediaPlayerBrowseButton = new System.Windows.Forms.Button();
            this.MediaPlayerTextBox = new System.Windows.Forms.TextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.LoadingAnim = new System.Windows.Forms.PictureBox();
            this.LoadingProgress = new System.Windows.Forms.Label();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.YoutubeTabPage = new System.Windows.Forms.TabPage();
            this.YouTubeChannelReadRandom = new System.Windows.Forms.CheckBox();
            this.YouTubeChannelReadReverse = new System.Windows.Forms.CheckBox();
            this.YoutubeChannelImage = new System.Windows.Forms.PictureBox();
            this.YoutubeChannelNameHistory = new System.Windows.Forms.ComboBox();
            this.PlayerTabPage = new System.Windows.Forms.TabPage();
            this.UpgradeMediaPlayerGroupBox = new System.Windows.Forms.GroupBox();
            this.buttonUpgradeVlcScript = new System.Windows.Forms.Button();
            this.AboutTabPage = new System.Windows.Forms.TabPage();
            this.aboutWebsite = new System.Windows.Forms.LinkLabel();
            this.aboutDescription = new System.Windows.Forms.Label();
            this.aboutSubtitle = new System.Windows.Forms.Label();
            this.aboutTitle = new System.Windows.Forms.Label();
            this.MediaPlayerGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MediaPlayerIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingAnim)).BeginInit();
            this.mainTabControl.SuspendLayout();
            this.YoutubeTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.YoutubeChannelImage)).BeginInit();
            this.PlayerTabPage.SuspendLayout();
            this.UpgradeMediaPlayerGroupBox.SuspendLayout();
            this.AboutTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // MediaPlayerGroupBox
            // 
            this.MediaPlayerGroupBox.Controls.Add(this.MediaPlayerIcon);
            this.MediaPlayerGroupBox.Controls.Add(this.MediaPlayerBrowseButton);
            this.MediaPlayerGroupBox.Controls.Add(this.MediaPlayerTextBox);
            this.MediaPlayerGroupBox.Location = new System.Drawing.Point(3, 6);
            this.MediaPlayerGroupBox.Name = "MediaPlayerGroupBox";
            this.MediaPlayerGroupBox.Size = new System.Drawing.Size(316, 55);
            this.MediaPlayerGroupBox.TabIndex = 0;
            this.MediaPlayerGroupBox.TabStop = false;
            this.MediaPlayerGroupBox.Text = "Media Player";
            // 
            // MediaPlayerIcon
            // 
            this.MediaPlayerIcon.Location = new System.Drawing.Point(9, 16);
            this.MediaPlayerIcon.Name = "MediaPlayerIcon";
            this.MediaPlayerIcon.Size = new System.Drawing.Size(32, 32);
            this.MediaPlayerIcon.TabIndex = 2;
            this.MediaPlayerIcon.TabStop = false;
            // 
            // MediaPlayerBrowseButton
            // 
            this.MediaPlayerBrowseButton.Location = new System.Drawing.Point(283, 20);
            this.MediaPlayerBrowseButton.Name = "MediaPlayerBrowseButton";
            this.MediaPlayerBrowseButton.Size = new System.Drawing.Size(27, 23);
            this.MediaPlayerBrowseButton.TabIndex = 1;
            this.MediaPlayerBrowseButton.Text = "...";
            this.MediaPlayerBrowseButton.UseVisualStyleBackColor = true;
            this.MediaPlayerBrowseButton.Click += new System.EventHandler(this.MediaPlayerBrowseButton_Click);
            // 
            // MediaPlayerTextBox
            // 
            this.MediaPlayerTextBox.Location = new System.Drawing.Point(48, 22);
            this.MediaPlayerTextBox.Name = "MediaPlayerTextBox";
            this.MediaPlayerTextBox.ReadOnly = true;
            this.MediaPlayerTextBox.Size = new System.Drawing.Size(229, 20);
            this.MediaPlayerTextBox.TabIndex = 0;
            // 
            // buttonOK
            // 
            this.buttonOK.Enabled = false;
            this.buttonOK.Location = new System.Drawing.Point(176, 159);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "Play";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(257, 159);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 3;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // LoadingAnim
            // 
            this.LoadingAnim.Image = global::YouTubeJukebox.Properties.Resources.Loading;
            this.LoadingAnim.Location = new System.Drawing.Point(21, 162);
            this.LoadingAnim.Name = "LoadingAnim";
            this.LoadingAnim.Size = new System.Drawing.Size(16, 16);
            this.LoadingAnim.TabIndex = 7;
            this.LoadingAnim.TabStop = false;
            this.LoadingAnim.Visible = false;
            // 
            // LoadingProgress
            // 
            this.LoadingProgress.AutoSize = true;
            this.LoadingProgress.Location = new System.Drawing.Point(43, 164);
            this.LoadingProgress.Name = "LoadingProgress";
            this.LoadingProgress.Size = new System.Drawing.Size(54, 13);
            this.LoadingProgress.TabIndex = 8;
            this.LoadingProgress.Text = "Loading...";
            this.LoadingProgress.Visible = false;
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.YoutubeTabPage);
            this.mainTabControl.Controls.Add(this.PlayerTabPage);
            this.mainTabControl.Controls.Add(this.AboutTabPage);
            this.mainTabControl.Location = new System.Drawing.Point(6, 8);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(333, 145);
            this.mainTabControl.TabIndex = 9;
            // 
            // YoutubeTabPage
            // 
            this.YoutubeTabPage.Controls.Add(this.YouTubeChannelReadRandom);
            this.YoutubeTabPage.Controls.Add(this.YouTubeChannelReadReverse);
            this.YoutubeTabPage.Controls.Add(this.YoutubeChannelImage);
            this.YoutubeTabPage.Controls.Add(this.YoutubeChannelNameHistory);
            this.YoutubeTabPage.Location = new System.Drawing.Point(4, 22);
            this.YoutubeTabPage.Name = "YoutubeTabPage";
            this.YoutubeTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.YoutubeTabPage.Size = new System.Drawing.Size(325, 119);
            this.YoutubeTabPage.TabIndex = 0;
            this.YoutubeTabPage.Text = "YouTube";
            this.YoutubeTabPage.UseVisualStyleBackColor = true;
            // 
            // YouTubeChannelReadRandom
            // 
            this.YouTubeChannelReadRandom.AutoSize = true;
            this.YouTubeChannelReadRandom.Location = new System.Drawing.Point(128, 67);
            this.YouTubeChannelReadRandom.Name = "YouTubeChannelReadRandom";
            this.YouTubeChannelReadRandom.Size = new System.Drawing.Size(93, 17);
            this.YouTubeChannelReadRandom.TabIndex = 10;
            this.YouTubeChannelReadRandom.Text = "Shuffle videos";
            this.YouTubeChannelReadRandom.UseVisualStyleBackColor = true;
            // 
            // YouTubeChannelReadReverse
            // 
            this.YouTubeChannelReadReverse.AutoSize = true;
            this.YouTubeChannelReadReverse.Location = new System.Drawing.Point(128, 44);
            this.YouTubeChannelReadReverse.Name = "YouTubeChannelReadReverse";
            this.YouTubeChannelReadReverse.Size = new System.Drawing.Size(93, 17);
            this.YouTubeChannelReadReverse.TabIndex = 9;
            this.YouTubeChannelReadReverse.Text = "Reverse order";
            this.YouTubeChannelReadReverse.UseVisualStyleBackColor = true;
            // 
            // YoutubeChannelImage
            // 
            this.YoutubeChannelImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.YoutubeChannelImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.YoutubeChannelImage.Location = new System.Drawing.Point(10, 12);
            this.YoutubeChannelImage.Name = "YoutubeChannelImage";
            this.YoutubeChannelImage.Size = new System.Drawing.Size(96, 96);
            this.YoutubeChannelImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.YoutubeChannelImage.TabIndex = 8;
            this.YoutubeChannelImage.TabStop = false;
            this.YoutubeChannelImage.Click += new System.EventHandler(this.YoutubeChannelImage_Click);
            // 
            // YoutubeChannelNameHistory
            // 
            this.YoutubeChannelNameHistory.FormattingEnabled = true;
            this.YoutubeChannelNameHistory.Location = new System.Drawing.Point(128, 12);
            this.YoutubeChannelNameHistory.Name = "YoutubeChannelNameHistory";
            this.YoutubeChannelNameHistory.Size = new System.Drawing.Size(164, 21);
            this.YoutubeChannelNameHistory.TabIndex = 6;
            this.YoutubeChannelNameHistory.SelectedIndexChanged += new System.EventHandler(this.YoutubeChannelNameChanged);
            this.YoutubeChannelNameHistory.TextChanged += new System.EventHandler(this.YoutubeChannelNameChanged);
            // 
            // PlayerTabPage
            // 
            this.PlayerTabPage.Controls.Add(this.UpgradeMediaPlayerGroupBox);
            this.PlayerTabPage.Controls.Add(this.MediaPlayerGroupBox);
            this.PlayerTabPage.Location = new System.Drawing.Point(4, 22);
            this.PlayerTabPage.Name = "PlayerTabPage";
            this.PlayerTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.PlayerTabPage.Size = new System.Drawing.Size(325, 119);
            this.PlayerTabPage.TabIndex = 1;
            this.PlayerTabPage.Text = "Player";
            this.PlayerTabPage.UseVisualStyleBackColor = true;
            // 
            // UpgradeMediaPlayerGroupBox
            // 
            this.UpgradeMediaPlayerGroupBox.Controls.Add(this.buttonUpgradeVlcScript);
            this.UpgradeMediaPlayerGroupBox.Location = new System.Drawing.Point(3, 67);
            this.UpgradeMediaPlayerGroupBox.Name = "UpgradeMediaPlayerGroupBox";
            this.UpgradeMediaPlayerGroupBox.Size = new System.Drawing.Size(316, 45);
            this.UpgradeMediaPlayerGroupBox.TabIndex = 2;
            this.UpgradeMediaPlayerGroupBox.TabStop = false;
            this.UpgradeMediaPlayerGroupBox.Text = "Playback Issues?";
            // 
            // buttonUpgradeVlcScript
            // 
            this.buttonUpgradeVlcScript.Location = new System.Drawing.Point(48, 16);
            this.buttonUpgradeVlcScript.Name = "buttonUpgradeVlcScript";
            this.buttonUpgradeVlcScript.Size = new System.Drawing.Size(229, 23);
            this.buttonUpgradeVlcScript.TabIndex = 1;
            this.buttonUpgradeVlcScript.Text = "Auto-Upgrade VLC YouTube Script";
            this.buttonUpgradeVlcScript.UseVisualStyleBackColor = true;
            this.buttonUpgradeVlcScript.Click += new System.EventHandler(this.buttonUpgradeVlcScript_Click);
            // 
            // AboutTabPage
            // 
            this.AboutTabPage.Controls.Add(this.aboutWebsite);
            this.AboutTabPage.Controls.Add(this.aboutDescription);
            this.AboutTabPage.Controls.Add(this.aboutSubtitle);
            this.AboutTabPage.Controls.Add(this.aboutTitle);
            this.AboutTabPage.Location = new System.Drawing.Point(4, 22);
            this.AboutTabPage.Name = "AboutTabPage";
            this.AboutTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.AboutTabPage.Size = new System.Drawing.Size(325, 119);
            this.AboutTabPage.TabIndex = 2;
            this.AboutTabPage.Text = "About...";
            this.AboutTabPage.UseVisualStyleBackColor = true;
            // 
            // aboutWebsite
            // 
            this.aboutWebsite.AutoSize = true;
            this.aboutWebsite.Location = new System.Drawing.Point(112, 95);
            this.aboutWebsite.Name = "aboutWebsite";
            this.aboutWebsite.Size = new System.Drawing.Size(109, 13);
            this.aboutWebsite.TabIndex = 3;
            this.aboutWebsite.TabStop = true;
            this.aboutWebsite.Text = "Visit my GitHub profile";
            this.aboutWebsite.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.aboutWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.aboutWebsite_LinkClicked);
            // 
            // aboutDescription
            // 
            this.aboutDescription.AutoSize = true;
            this.aboutDescription.Location = new System.Drawing.Point(73, 59);
            this.aboutDescription.Name = "aboutDescription";
            this.aboutDescription.Size = new System.Drawing.Size(182, 26);
            this.aboutDescription.TabIndex = 2;
            this.aboutDescription.Text = "Easy playback of YouTube Channels\r\nInspired by youtube.nestharion.de";
            this.aboutDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // aboutSubtitle
            // 
            this.aboutSubtitle.AutoSize = true;
            this.aboutSubtitle.Location = new System.Drawing.Point(109, 36);
            this.aboutSubtitle.Name = "aboutSubtitle";
            this.aboutSubtitle.Size = new System.Drawing.Size(115, 13);
            this.aboutSubtitle.TabIndex = 1;
            this.aboutSubtitle.Text = "Version 1.0 - by ORelio";
            // 
            // aboutTitle
            // 
            this.aboutTitle.AutoSize = true;
            this.aboutTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aboutTitle.Location = new System.Drawing.Point(62, 11);
            this.aboutTitle.Name = "aboutTitle";
            this.aboutTitle.Size = new System.Drawing.Size(202, 25);
            this.aboutTitle.TabIndex = 0;
            this.aboutTitle.Text = "YouTube Jukebox";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 190);
            this.Controls.Add(this.mainTabControl);
            this.Controls.Add(this.LoadingProgress);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.LoadingAnim);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "YouTube Jukebox";
            this.MediaPlayerGroupBox.ResumeLayout(false);
            this.MediaPlayerGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MediaPlayerIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingAnim)).EndInit();
            this.mainTabControl.ResumeLayout(false);
            this.YoutubeTabPage.ResumeLayout(false);
            this.YoutubeTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.YoutubeChannelImage)).EndInit();
            this.PlayerTabPage.ResumeLayout(false);
            this.UpgradeMediaPlayerGroupBox.ResumeLayout(false);
            this.AboutTabPage.ResumeLayout(false);
            this.AboutTabPage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox MediaPlayerGroupBox;
        private System.Windows.Forms.Button MediaPlayerBrowseButton;
        private System.Windows.Forms.TextBox MediaPlayerTextBox;
        private System.Windows.Forms.PictureBox MediaPlayerIcon;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.PictureBox LoadingAnim;
        private System.Windows.Forms.Label LoadingProgress;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage YoutubeTabPage;
        private System.Windows.Forms.TabPage PlayerTabPage;
        private System.Windows.Forms.CheckBox YouTubeChannelReadRandom;
        private System.Windows.Forms.CheckBox YouTubeChannelReadReverse;
        private System.Windows.Forms.PictureBox YoutubeChannelImage;
        private System.Windows.Forms.ComboBox YoutubeChannelNameHistory;
        private System.Windows.Forms.TabPage AboutTabPage;
        private System.Windows.Forms.Button buttonUpgradeVlcScript;
        private System.Windows.Forms.GroupBox UpgradeMediaPlayerGroupBox;
        private System.Windows.Forms.Label aboutSubtitle;
        private System.Windows.Forms.Label aboutTitle;
        private System.Windows.Forms.LinkLabel aboutWebsite;
        private System.Windows.Forms.Label aboutDescription;
    }
}

