namespace UAVP.UAVPSet
{
    partial class ViewParameter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewParameter));
            this.set1Label = new System.Windows.Forms.Label();
            this.set2Label = new System.Windows.Forms.Label();
            this.set1TextBox = new System.Windows.Forms.TextBox();
            this.set2TextBox = new System.Windows.Forms.TextBox();
            this.set1Button = new System.Windows.Forms.Button();
            this.set2Button = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // set1Label
            // 
            resources.ApplyResources(this.set1Label, "set1Label");
            this.set1Label.Name = "set1Label";
            // 
            // set2Label
            // 
            resources.ApplyResources(this.set2Label, "set2Label");
            this.set2Label.Name = "set2Label";
            // 
            // set1TextBox
            // 
            resources.ApplyResources(this.set1TextBox, "set1TextBox");
            this.set1TextBox.Name = "set1TextBox";
            // 
            // set2TextBox
            // 
            resources.ApplyResources(this.set2TextBox, "set2TextBox");
            this.set2TextBox.Name = "set2TextBox";
            // 
            // set1Button
            // 
            resources.ApplyResources(this.set1Button, "set1Button");
            this.set1Button.Name = "set1Button";
            this.set1Button.UseVisualStyleBackColor = true;
            this.set1Button.Click += new System.EventHandler(this.set1Button_Click);
            // 
            // set2Button
            // 
            resources.ApplyResources(this.set2Button, "set2Button");
            this.set2Button.Name = "set2Button";
            this.set2Button.UseVisualStyleBackColor = true;
            this.set2Button.Click += new System.EventHandler(this.set2Button_Click);
            // 
            // closeButton
            // 
            resources.ApplyResources(this.closeButton, "closeButton");
            this.closeButton.Name = "closeButton";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Name = "label1";
            // 
            // ViewParameter
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.set2Button);
            this.Controls.Add(this.set1Button);
            this.Controls.Add(this.set2TextBox);
            this.Controls.Add(this.set1TextBox);
            this.Controls.Add(this.set2Label);
            this.Controls.Add(this.set1Label);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ViewParameter";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label set1Label;
        private System.Windows.Forms.Label set2Label;
        private System.Windows.Forms.TextBox set1TextBox;
        private System.Windows.Forms.TextBox set2TextBox;
        private System.Windows.Forms.Button set1Button;
        private System.Windows.Forms.Button set2Button;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label label1;
    }
}