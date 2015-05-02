namespace TwitchClient.Panels
{
    partial class UpdateBroadcastingInfoPanel
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
            this.gameTextBox = new System.Windows.Forms.TextBox();
            this.titleTextBox = new System.Windows.Forms.TextBox();
            this.gameLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.updateButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // gameTextBox
            // 
            this.gameTextBox.Location = new System.Drawing.Point(49, 3);
            this.gameTextBox.Name = "gameTextBox";
            this.gameTextBox.Size = new System.Drawing.Size(285, 20);
            this.gameTextBox.TabIndex = 0;
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new System.Drawing.Point(49, 29);
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new System.Drawing.Size(285, 20);
            this.titleTextBox.TabIndex = 1;
            // 
            // gameLabel
            // 
            this.gameLabel.AutoSize = true;
            this.gameLabel.Location = new System.Drawing.Point(3, 6);
            this.gameLabel.Name = "gameLabel";
            this.gameLabel.Size = new System.Drawing.Size(35, 13);
            this.gameLabel.TabIndex = 2;
            this.gameLabel.Text = "Game";
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(3, 32);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(27, 13);
            this.titleLabel.TabIndex = 3;
            this.titleLabel.Text = "Title";
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(259, 55);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(75, 23);
            this.updateButton.TabIndex = 4;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            // 
            // UpdateBroadcastingInfoPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.gameLabel);
            this.Controls.Add(this.titleTextBox);
            this.Controls.Add(this.gameTextBox);
            this.Name = "UpdateBroadcastingInfoPanel";
            this.Size = new System.Drawing.Size(337, 81);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox gameTextBox;
        private System.Windows.Forms.TextBox titleTextBox;
        private System.Windows.Forms.Label gameLabel;
        private System.Windows.Forms.Label titleLabel;
        public System.Windows.Forms.Button updateButton;
    }
}
