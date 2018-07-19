namespace SwitchRichPresence
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.linkLabel_twitter = new System.Windows.Forms.LinkLabel();
            this.linkLabel_github = new System.Windows.Forms.LinkLabel();
            this.linkLabel_youtube = new System.Windows.Forms.LinkLabel();
            this.textBox_discord = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(123, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Created by Random0666";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(15, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(98, 181);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 23);
            this.button_ok.TabIndex = 2;
            this.button_ok.Text = "Ok";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // linkLabel_twitter
            // 
            this.linkLabel_twitter.AutoSize = true;
            this.linkLabel_twitter.Location = new System.Drawing.Point(124, 46);
            this.linkLabel_twitter.Name = "linkLabel_twitter";
            this.linkLabel_twitter.Size = new System.Drawing.Size(39, 13);
            this.linkLabel_twitter.TabIndex = 3;
            this.linkLabel_twitter.TabStop = true;
            this.linkLabel_twitter.Text = "Twitter";
            this.linkLabel_twitter.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_twitter_LinkClicked);
            // 
            // linkLabel_github
            // 
            this.linkLabel_github.AutoSize = true;
            this.linkLabel_github.Location = new System.Drawing.Point(124, 61);
            this.linkLabel_github.Name = "linkLabel_github";
            this.linkLabel_github.Size = new System.Drawing.Size(38, 13);
            this.linkLabel_github.TabIndex = 4;
            this.linkLabel_github.TabStop = true;
            this.linkLabel_github.Text = "Github";
            this.linkLabel_github.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_github_LinkClicked);
            // 
            // linkLabel_youtube
            // 
            this.linkLabel_youtube.AutoSize = true;
            this.linkLabel_youtube.Location = new System.Drawing.Point(124, 77);
            this.linkLabel_youtube.Name = "linkLabel_youtube";
            this.linkLabel_youtube.Size = new System.Drawing.Size(47, 13);
            this.linkLabel_youtube.TabIndex = 5;
            this.linkLabel_youtube.TabStop = true;
            this.linkLabel_youtube.Text = "Youtube";
            this.linkLabel_youtube.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_youtube_LinkClicked);
            // 
            // textBox_discord
            // 
            this.textBox_discord.Location = new System.Drawing.Point(14, 145);
            this.textBox_discord.Name = "textBox_discord";
            this.textBox_discord.ReadOnly = true;
            this.textBox_discord.Size = new System.Drawing.Size(79, 20);
            this.textBox_discord.TabIndex = 6;
            this.textBox_discord.Text = "random#6457";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 26);
            this.label2.TabIndex = 7;
            this.label2.Text = "If you have any question/problem, please\r\ncontact me on discord : ";
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 210);
            this.Controls.Add(this.textBox_discord);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.linkLabel_youtube);
            this.Controls.Add(this.linkLabel_github);
            this.Controls.Add(this.linkLabel_twitter);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.LinkLabel linkLabel_twitter;
        private System.Windows.Forms.LinkLabel linkLabel_github;
        private System.Windows.Forms.LinkLabel linkLabel_youtube;
        private System.Windows.Forms.TextBox textBox_discord;
        private System.Windows.Forms.Label label2;
    }
}