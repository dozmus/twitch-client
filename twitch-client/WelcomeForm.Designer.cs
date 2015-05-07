namespace TwitchClient
{
    partial class WelcomeForm
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
            this.doneButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.twitchUsernameLabel = new System.Windows.Forms.Label();
            this.connectTwitchButton = new System.Windows.Forms.Button();
            this.twitchUsernameTextBox = new System.Windows.Forms.TextBox();
            this.lowRamModeCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // doneButton
            // 
            this.doneButton.Location = new System.Drawing.Point(197, 87);
            this.doneButton.Name = "doneButton";
            this.doneButton.Size = new System.Drawing.Size(75, 23);
            this.doneButton.TabIndex = 3;
            this.doneButton.Text = "&Done";
            this.doneButton.UseVisualStyleBackColor = true;
            this.doneButton.Click += new System.EventHandler(this.doneButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(12, 72);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(63, 13);
            this.statusLabel.TabIndex = 0;
            this.statusLabel.Text = "Status: N/A";
            // 
            // twitchUsernameLabel
            // 
            this.twitchUsernameLabel.AutoSize = true;
            this.twitchUsernameLabel.Location = new System.Drawing.Point(12, 15);
            this.twitchUsernameLabel.Name = "twitchUsernameLabel";
            this.twitchUsernameLabel.Size = new System.Drawing.Size(90, 13);
            this.twitchUsernameLabel.TabIndex = 0;
            this.twitchUsernameLabel.Text = "Twitch Username";
            // 
            // connectTwitchButton
            // 
            this.connectTwitchButton.Location = new System.Drawing.Point(15, 38);
            this.connectTwitchButton.Name = "connectTwitchButton";
            this.connectTwitchButton.Size = new System.Drawing.Size(257, 23);
            this.connectTwitchButton.TabIndex = 1;
            this.connectTwitchButton.Text = "Connect with Twitch";
            this.connectTwitchButton.UseVisualStyleBackColor = true;
            this.connectTwitchButton.Click += new System.EventHandler(this.connectTwitchButton_Click);
            // 
            // twitchUsernameTextBox
            // 
            this.twitchUsernameTextBox.Location = new System.Drawing.Point(117, 12);
            this.twitchUsernameTextBox.Name = "twitchUsernameTextBox";
            this.twitchUsernameTextBox.Size = new System.Drawing.Size(155, 20);
            this.twitchUsernameTextBox.TabIndex = 0;
            // 
            // lowRamModeCheckBox
            // 
            this.lowRamModeCheckBox.AutoSize = true;
            this.lowRamModeCheckBox.Location = new System.Drawing.Point(15, 91);
            this.lowRamModeCheckBox.Name = "lowRamModeCheckBox";
            this.lowRamModeCheckBox.Size = new System.Drawing.Size(103, 17);
            this.lowRamModeCheckBox.TabIndex = 2;
            this.lowRamModeCheckBox.Text = "Low RAM Mode";
            this.lowRamModeCheckBox.UseVisualStyleBackColor = true;
            // 
            // WelcomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 122);
            this.Controls.Add(this.lowRamModeCheckBox);
            this.Controls.Add(this.twitchUsernameTextBox);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.twitchUsernameLabel);
            this.Controls.Add(this.connectTwitchButton);
            this.Controls.Add(this.doneButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "WelcomeForm";
            this.Text = "Welcome";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WelcomeForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button doneButton;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label twitchUsernameLabel;
        private System.Windows.Forms.Button connectTwitchButton;
        private System.Windows.Forms.TextBox twitchUsernameTextBox;
        public System.Windows.Forms.CheckBox lowRamModeCheckBox;
    }
}