namespace TwitchClient.Panels.ChatBot
{
    partial class ChatBotCredentialsPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.nicknameLabel = new System.Windows.Forms.Label();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.ircServerLabel = new System.Windows.Forms.Label();
            this.nicknameTextBox = new System.Windows.Forms.TextBox();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.ircServerTextBox = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // nicknameLabel
            // 
            this.nicknameLabel.AutoSize = true;
            this.nicknameLabel.Location = new System.Drawing.Point(3, 9);
            this.nicknameLabel.Name = "nicknameLabel";
            this.nicknameLabel.Size = new System.Drawing.Size(55, 13);
            this.nicknameLabel.TabIndex = 0;
            this.nicknameLabel.Text = "Nickname";
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(3, 35);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(53, 13);
            this.passwordLabel.TabIndex = 1;
            this.passwordLabel.Text = "Password";
            // 
            // ircServerLabel
            // 
            this.ircServerLabel.AutoSize = true;
            this.ircServerLabel.Location = new System.Drawing.Point(3, 61);
            this.ircServerLabel.Name = "ircServerLabel";
            this.ircServerLabel.Size = new System.Drawing.Size(59, 13);
            this.ircServerLabel.TabIndex = 2;
            this.ircServerLabel.Text = "IRC Server";
            // 
            // nicknameTextBox
            // 
            this.nicknameTextBox.Location = new System.Drawing.Point(80, 6);
            this.nicknameTextBox.Name = "nicknameTextBox";
            this.nicknameTextBox.Size = new System.Drawing.Size(212, 20);
            this.nicknameTextBox.TabIndex = 3;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(80, 32);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(212, 20);
            this.passwordTextBox.TabIndex = 4;
            // 
            // ircServerTextBox
            // 
            this.ircServerTextBox.Location = new System.Drawing.Point(80, 58);
            this.ircServerTextBox.Name = "ircServerTextBox";
            this.ircServerTextBox.Size = new System.Drawing.Size(212, 20);
            this.ircServerTextBox.TabIndex = 5;
            this.ircServerTextBox.Text = "irc.twitch.tv/6667";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(217, 84);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 6;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            // 
            // ChatBotCredentialsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.ircServerTextBox);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.nicknameTextBox);
            this.Controls.Add(this.ircServerLabel);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.nicknameLabel);
            this.Name = "ChatBotCredentialsPanel";
            this.Size = new System.Drawing.Size(297, 112);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label nicknameLabel;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Label ircServerLabel;
        private System.Windows.Forms.TextBox nicknameTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.TextBox ircServerTextBox;
        public System.Windows.Forms.Button connectButton;
    }
}
