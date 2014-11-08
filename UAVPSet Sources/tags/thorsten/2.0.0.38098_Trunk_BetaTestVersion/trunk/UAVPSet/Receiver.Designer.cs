namespace UAVPSet
{
    partial class Receiver
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
            this.gasTrackBar = new System.Windows.Forms.TrackBar();
            this.rollTrackBar = new System.Windows.Forms.TrackBar();
            this.nickTrackBar = new System.Windows.Forms.TrackBar();
            this.gierTrackBar = new System.Windows.Forms.TrackBar();
            this.ch5TrackBar = new System.Windows.Forms.TrackBar();
            this.gasLabel = new System.Windows.Forms.Label();
            this.rollLabel = new System.Windows.Forms.Label();
            this.nickLabel = new System.Windows.Forms.Label();
            this.gierLabel = new System.Windows.Forms.Label();
            this.ch5Label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.okButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gasTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rollTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nickTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gierTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ch5TrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // gasTrackBar
            // 
            this.gasTrackBar.Enabled = false;
            this.gasTrackBar.LargeChange = 10;
            this.gasTrackBar.Location = new System.Drawing.Point(8, 32);
            this.gasTrackBar.Maximum = 200;
            this.gasTrackBar.Name = "gasTrackBar";
            this.gasTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.gasTrackBar.Size = new System.Drawing.Size(45, 104);
            this.gasTrackBar.TabIndex = 1;
            this.gasTrackBar.TickFrequency = 10;
            // 
            // rollTrackBar
            // 
            this.rollTrackBar.Enabled = false;
            this.rollTrackBar.LargeChange = 10;
            this.rollTrackBar.Location = new System.Drawing.Point(59, 32);
            this.rollTrackBar.Maximum = 100;
            this.rollTrackBar.Minimum = -100;
            this.rollTrackBar.Name = "rollTrackBar";
            this.rollTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.rollTrackBar.Size = new System.Drawing.Size(45, 104);
            this.rollTrackBar.TabIndex = 2;
            this.rollTrackBar.TickFrequency = 10;
            // 
            // nickTrackBar
            // 
            this.nickTrackBar.Enabled = false;
            this.nickTrackBar.LargeChange = 10;
            this.nickTrackBar.Location = new System.Drawing.Point(110, 32);
            this.nickTrackBar.Maximum = 100;
            this.nickTrackBar.Minimum = -100;
            this.nickTrackBar.Name = "nickTrackBar";
            this.nickTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.nickTrackBar.Size = new System.Drawing.Size(45, 104);
            this.nickTrackBar.TabIndex = 3;
            this.nickTrackBar.TickFrequency = 10;
            // 
            // gierTrackBar
            // 
            this.gierTrackBar.Enabled = false;
            this.gierTrackBar.LargeChange = 10;
            this.gierTrackBar.Location = new System.Drawing.Point(161, 32);
            this.gierTrackBar.Maximum = 100;
            this.gierTrackBar.Minimum = -100;
            this.gierTrackBar.Name = "gierTrackBar";
            this.gierTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.gierTrackBar.Size = new System.Drawing.Size(45, 104);
            this.gierTrackBar.TabIndex = 4;
            this.gierTrackBar.TickFrequency = 10;
            // 
            // ch5TrackBar
            // 
            this.ch5TrackBar.Enabled = false;
            this.ch5TrackBar.LargeChange = 10;
            this.ch5TrackBar.Location = new System.Drawing.Point(212, 32);
            this.ch5TrackBar.Maximum = 200;
            this.ch5TrackBar.Name = "ch5TrackBar";
            this.ch5TrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.ch5TrackBar.Size = new System.Drawing.Size(45, 104);
            this.ch5TrackBar.TabIndex = 5;
            this.ch5TrackBar.TickFrequency = 10;
            // 
            // gasLabel
            // 
            this.gasLabel.AutoSize = true;
            this.gasLabel.Location = new System.Drawing.Point(8, 139);
            this.gasLabel.Name = "gasLabel";
            this.gasLabel.Size = new System.Drawing.Size(13, 13);
            this.gasLabel.TabIndex = 6;
            this.gasLabel.Text = "0";
            this.gasLabel.TextChanged += new System.EventHandler(this.gasLabel_TextChanged);
            // 
            // rollLabel
            // 
            this.rollLabel.AutoSize = true;
            this.rollLabel.Location = new System.Drawing.Point(56, 139);
            this.rollLabel.Name = "rollLabel";
            this.rollLabel.Size = new System.Drawing.Size(13, 13);
            this.rollLabel.TabIndex = 7;
            this.rollLabel.Text = "0";
            this.rollLabel.TextChanged += new System.EventHandler(this.rollLabel_TextChanged);
            // 
            // nickLabel
            // 
            this.nickLabel.AutoSize = true;
            this.nickLabel.Location = new System.Drawing.Point(107, 139);
            this.nickLabel.Name = "nickLabel";
            this.nickLabel.Size = new System.Drawing.Size(13, 13);
            this.nickLabel.TabIndex = 8;
            this.nickLabel.Text = "0";
            this.nickLabel.TextChanged += new System.EventHandler(this.nickLabel_TextChanged);
            // 
            // gierLabel
            // 
            this.gierLabel.AutoSize = true;
            this.gierLabel.Location = new System.Drawing.Point(158, 139);
            this.gierLabel.Name = "gierLabel";
            this.gierLabel.Size = new System.Drawing.Size(13, 13);
            this.gierLabel.TabIndex = 9;
            this.gierLabel.Text = "0";
            this.gierLabel.TextChanged += new System.EventHandler(this.gierLabel_TextChanged);
            // 
            // ch5Label
            // 
            this.ch5Label.AutoSize = true;
            this.ch5Label.Location = new System.Drawing.Point(209, 139);
            this.ch5Label.Name = "ch5Label";
            this.ch5Label.Size = new System.Drawing.Size(13, 13);
            this.ch5Label.TabIndex = 10;
            this.ch5Label.Text = "0";
            this.ch5Label.TextChanged += new System.EventHandler(this.ch5Label_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Gas";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(56, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Roll";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(107, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Nick";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(158, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Gier";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(209, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Ch 5";
            // 
            // timer1
            // 
            this.timer1.Interval = 300;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(96, 164);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 16;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // Receiver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 199);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ch5Label);
            this.Controls.Add(this.gierLabel);
            this.Controls.Add(this.nickLabel);
            this.Controls.Add(this.rollLabel);
            this.Controls.Add(this.gasLabel);
            this.Controls.Add(this.ch5TrackBar);
            this.Controls.Add(this.gierTrackBar);
            this.Controls.Add(this.nickTrackBar);
            this.Controls.Add(this.rollTrackBar);
            this.Controls.Add(this.gasTrackBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Receiver";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Receiver";
            this.VisibleChanged += new System.EventHandler(this.Receiver_VisibleChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Receiver_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.gasTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rollTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nickTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gierTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ch5TrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar gasTrackBar;
        private System.Windows.Forms.TrackBar rollTrackBar;
        private System.Windows.Forms.TrackBar nickTrackBar;
        private System.Windows.Forms.TrackBar gierTrackBar;
        private System.Windows.Forms.TrackBar ch5TrackBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label gasLabel;
        public System.Windows.Forms.Label rollLabel;
        public System.Windows.Forms.Label nickLabel;
        public System.Windows.Forms.Label gierLabel;
        public System.Windows.Forms.Label ch5Label;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button okButton;

    }
}