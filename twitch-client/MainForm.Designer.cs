namespace TwitchClient
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.masterStatusStrip = new System.Windows.Forms.StatusStrip();
            this.mainStatsLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.masterMenuStrip = new System.Windows.Forms.MenuStrip();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.broadcastingTabPage = new System.Windows.Forms.TabPage();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.followersRichTextBox = new System.Windows.Forms.RichTextBox();
            this.followersUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.watchStreamPanel = new TwitchClient.Panels.WatchStreamPanel();
            this.updateBroadcastingInfoPanel = new TwitchClient.Panels.UpdateBroadcastingInfoPanel();
            this.broadcastingInfoPanel = new TwitchClient.Panels.BroadcastingInfoPanel();
            this.masterStatusStrip.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.broadcastingTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // masterStatusStrip
            // 
            this.masterStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainStatsLabel});
            this.masterStatusStrip.Location = new System.Drawing.Point(0, 423);
            this.masterStatusStrip.Name = "masterStatusStrip";
            this.masterStatusStrip.Size = new System.Drawing.Size(534, 22);
            this.masterStatusStrip.TabIndex = 0;
            this.masterStatusStrip.Text = "statusStrip1";
            // 
            // mainStatsLabel
            // 
            this.mainStatsLabel.Name = "mainStatsLabel";
            this.mainStatsLabel.Size = new System.Drawing.Size(73, 17);
            this.mainStatsLabel.Text = "Main Stats: ?";
            // 
            // masterMenuStrip
            // 
            this.masterMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.masterMenuStrip.Name = "masterMenuStrip";
            this.masterMenuStrip.Size = new System.Drawing.Size(534, 24);
            this.masterMenuStrip.TabIndex = 1;
            this.masterMenuStrip.Text = "menuStrip1";
            this.masterMenuStrip.Resize += new System.EventHandler(this.masterMenuStrip_Resize);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.broadcastingTabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(534, 399);
            this.tabControl.TabIndex = 2;
            // 
            // broadcastingTabPage
            // 
            this.broadcastingTabPage.Controls.Add(this.followersRichTextBox);
            this.broadcastingTabPage.Controls.Add(this.watchStreamPanel);
            this.broadcastingTabPage.Controls.Add(this.updateBroadcastingInfoPanel);
            this.broadcastingTabPage.Controls.Add(this.broadcastingInfoPanel);
            this.broadcastingTabPage.Location = new System.Drawing.Point(4, 22);
            this.broadcastingTabPage.Name = "broadcastingTabPage";
            this.broadcastingTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.broadcastingTabPage.Size = new System.Drawing.Size(526, 373);
            this.broadcastingTabPage.TabIndex = 0;
            this.broadcastingTabPage.Text = "Broadcasting";
            this.broadcastingTabPage.UseVisualStyleBackColor = true;
            // 
            // updateTimer
            // 
            this.updateTimer.Interval = 2500;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // followersRichTextBox
            // 
            this.followersRichTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.followersRichTextBox.Location = new System.Drawing.Point(346, 3);
            this.followersRichTextBox.Name = "followersRichTextBox";
            this.followersRichTextBox.ReadOnly = true;
            this.followersRichTextBox.Size = new System.Drawing.Size(177, 147);
            this.followersRichTextBox.TabIndex = 3;
            this.followersRichTextBox.Text = "";
            // 
            // followersUpdateTimer
            // 
            this.followersUpdateTimer.Interval = 5000;
            this.followersUpdateTimer.Tick += new System.EventHandler(this.followersUpdateTimer_Tick);
            // 
            // watchStreamPanel
            // 
            this.watchStreamPanel.Location = new System.Drawing.Point(8, 156);
            this.watchStreamPanel.Name = "watchStreamPanel";
            this.watchStreamPanel.Size = new System.Drawing.Size(512, 211);
            this.watchStreamPanel.TabIndex = 2;
            // 
            // updateBroadcastingInfoPanel
            // 
            this.updateBroadcastingInfoPanel.Location = new System.Drawing.Point(3, 69);
            this.updateBroadcastingInfoPanel.Name = "updateBroadcastingInfoPanel";
            this.updateBroadcastingInfoPanel.Size = new System.Drawing.Size(337, 81);
            this.updateBroadcastingInfoPanel.TabIndex = 1;
            // 
            // broadcastingInfoPanel
            // 
            this.broadcastingInfoPanel.Location = new System.Drawing.Point(3, 3);
            this.broadcastingInfoPanel.Name = "broadcastingInfoPanel";
            this.broadcastingInfoPanel.Size = new System.Drawing.Size(300, 60);
            this.broadcastingInfoPanel.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 445);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.masterStatusStrip);
            this.Controls.Add(this.masterMenuStrip);
            this.MainMenuStrip = this.masterMenuStrip;
            this.Name = "MainForm";
            this.Text = "twitch-client";
            this.masterStatusStrip.ResumeLayout(false);
            this.masterStatusStrip.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.broadcastingTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip masterStatusStrip;
        private System.Windows.Forms.MenuStrip masterMenuStrip;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage broadcastingTabPage;
        private Panels.UpdateBroadcastingInfoPanel updateBroadcastingInfoPanel;
        private Panels.BroadcastingInfoPanel broadcastingInfoPanel;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.ToolStripStatusLabel mainStatsLabel;
        private Panels.WatchStreamPanel watchStreamPanel;
        private System.Windows.Forms.RichTextBox followersRichTextBox;
        private System.Windows.Forms.Timer followersUpdateTimer;
    }
}

