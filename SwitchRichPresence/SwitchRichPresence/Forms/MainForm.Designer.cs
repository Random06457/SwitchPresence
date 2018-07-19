namespace SwitchRichPresence
{
    partial class MainForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pictureBox_icon = new System.Windows.Forms.PictureBox();
            this.textBox_ip = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_connect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label_game = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.utilsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportIconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBox_showUser = new System.Windows.Forms.CheckBox();
            this.checkBox_showTime = new System.Windows.Forms.CheckBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_icon)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox_icon
            // 
            this.pictureBox_icon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_icon.Location = new System.Drawing.Point(12, 126);
            this.pictureBox_icon.Name = "pictureBox_icon";
            this.pictureBox_icon.Size = new System.Drawing.Size(100, 100);
            this.pictureBox_icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_icon.TabIndex = 0;
            this.pictureBox_icon.TabStop = false;
            // 
            // textBox_ip
            // 
            this.textBox_ip.Location = new System.Drawing.Point(88, 28);
            this.textBox_ip.Name = "textBox_ip";
            this.textBox_ip.Size = new System.Drawing.Size(118, 20);
            this.textBox_ip.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(65, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "IP :";
            // 
            // button_connect
            // 
            this.button_connect.Location = new System.Drawing.Point(98, 56);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(75, 23);
            this.button_connect.TabIndex = 3;
            this.button_connect.Text = "Connect";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(118, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Playing :";
            // 
            // label_game
            // 
            this.label_game.AutoSize = true;
            this.label_game.Location = new System.Drawing.Point(118, 143);
            this.label_game.Name = "label_game";
            this.label_game.Size = new System.Drawing.Size(16, 13);
            this.label_game.TabIndex = 5;
            this.label_game.Text = "...";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.utilsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(270, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // utilsToolStripMenuItem
            // 
            this.utilsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportIconsToolStripMenuItem});
            this.utilsToolStripMenuItem.Name = "utilsToolStripMenuItem";
            this.utilsToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.utilsToolStripMenuItem.Text = "Utils";
            this.utilsToolStripMenuItem.Visible = false;
            // 
            // exportIconsToolStripMenuItem
            // 
            this.exportIconsToolStripMenuItem.Name = "exportIconsToolStripMenuItem";
            this.exportIconsToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.exportIconsToolStripMenuItem.Text = "Export icons";
            this.exportIconsToolStripMenuItem.Click += new System.EventHandler(this.exportIconsToolStripMenuItem_Click);
            // 
            // checkBox_showUser
            // 
            this.checkBox_showUser.AutoSize = true;
            this.checkBox_showUser.Checked = true;
            this.checkBox_showUser.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_showUser.Location = new System.Drawing.Point(12, 88);
            this.checkBox_showUser.Name = "checkBox_showUser";
            this.checkBox_showUser.Size = new System.Drawing.Size(119, 17);
            this.checkBox_showUser.TabIndex = 7;
            this.checkBox_showUser.Text = "Show selected user";
            this.checkBox_showUser.UseVisualStyleBackColor = true;
            this.checkBox_showUser.CheckedChanged += new System.EventHandler(this.checkBox_showUser_CheckedChanged);
            // 
            // checkBox_showTime
            // 
            this.checkBox_showTime.AutoSize = true;
            this.checkBox_showTime.Checked = true;
            this.checkBox_showTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_showTime.Location = new System.Drawing.Point(12, 104);
            this.checkBox_showTime.Name = "checkBox_showTime";
            this.checkBox_showTime.Size = new System.Drawing.Size(78, 17);
            this.checkBox_showTime.TabIndex = 8;
            this.checkBox_showTime.Text = "Show timer";
            this.checkBox_showTime.UseVisualStyleBackColor = true;
            this.checkBox_showTime.CheckedChanged += new System.EventHandler(this.checkBox_showTime_CheckedChanged);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(231, 222);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(35, 13);
            this.linkLabel1.TabIndex = 9;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "About";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(270, 242);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.checkBox_showTime);
            this.Controls.Add(this.checkBox_showUser);
            this.Controls.Add(this.label_game);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_connect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_ip);
            this.Controls.Add(this.pictureBox_icon);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Sys-Discord Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_icon)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_icon;
        private System.Windows.Forms.TextBox textBox_ip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_game;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem utilsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportIconsToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBox_showUser;
        private System.Windows.Forms.CheckBox checkBox_showTime;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}

