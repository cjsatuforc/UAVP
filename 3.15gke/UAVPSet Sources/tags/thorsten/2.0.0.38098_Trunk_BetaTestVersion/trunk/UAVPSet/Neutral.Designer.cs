namespace UAVPSet
{
    partial class Neutral
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
            this.neutralLabel = new System.Windows.Forms.Label();
            this.ueberButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // neutralLabel
            // 
            this.neutralLabel.AutoSize = true;
            this.neutralLabel.Location = new System.Drawing.Point(25, 27);
            this.neutralLabel.Name = "neutralLabel";
            this.neutralLabel.Size = new System.Drawing.Size(41, 13);
            this.neutralLabel.TabIndex = 0;
            this.neutralLabel.Text = "Neutral";
            // 
            // ueberButton
            // 
            this.ueberButton.Location = new System.Drawing.Point(28, 57);
            this.ueberButton.Name = "ueberButton";
            this.ueberButton.Size = new System.Drawing.Size(109, 23);
            this.ueberButton.TabIndex = 1;
            this.ueberButton.Text = "Übernehme Werte";
            this.ueberButton.UseVisualStyleBackColor = true;
            this.ueberButton.Click += new System.EventHandler(this.ueberButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(143, 57);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // Neutral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(248, 106);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.ueberButton);
            this.Controls.Add(this.neutralLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Neutral";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Neutral";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label neutralLabel;
        private System.Windows.Forms.Button ueberButton;
        private System.Windows.Forms.Button cancelButton;

    }
}