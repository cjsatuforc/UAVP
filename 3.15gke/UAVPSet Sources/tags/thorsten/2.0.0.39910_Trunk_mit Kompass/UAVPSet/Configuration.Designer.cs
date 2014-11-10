namespace UAVPSet
{
    partial class Configuration
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Configuration));
            this.configurationTabControl = new System.Windows.Forms.TabControl();
            this.tabPageCOM = new System.Windows.Forms.TabPage();
            this.timeOutMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.timeLabel = new System.Windows.Forms.Label();
            this.flussLabel = new System.Windows.Forms.Label();
            this.flussComboBox = new System.Windows.Forms.ComboBox();
            this.stopBitsLabel = new System.Windows.Forms.Label();
            this.stopBitsComboBox = new System.Windows.Forms.ComboBox();
            this.paritätLabel = new System.Windows.Forms.Label();
            this.paritaetComboBox = new System.Windows.Forms.ComboBox();
            this.bitsSLabel = new System.Windows.Forms.Label();
            this.bitsSComboBox = new System.Windows.Forms.ComboBox();
            this.comPortLabel = new System.Windows.Forms.Label();
            this.comPortComboBox = new System.Windows.Forms.ComboBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.loglevelComboBox = new System.Windows.Forms.ComboBox();
            this.loglevelLabel = new System.Windows.Forms.Label();
            this.divTabPage = new System.Windows.Forms.TabPage();
            this.writeSleepMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.writeSleepLabel = new System.Windows.Forms.Label();
            this.splashCheckBox = new System.Windows.Forms.CheckBox();
            this.configurationTabControl.SuspendLayout();
            this.tabPageCOM.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.divTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // configurationTabControl
            // 
            this.configurationTabControl.Controls.Add(this.tabPageCOM);
            this.configurationTabControl.Controls.Add(this.tabPage1);
            this.configurationTabControl.Controls.Add(this.divTabPage);
            resources.ApplyResources(this.configurationTabControl, "configurationTabControl");
            this.configurationTabControl.Name = "configurationTabControl";
            this.configurationTabControl.SelectedIndex = 0;
            // 
            // tabPageCOM
            // 
            this.tabPageCOM.Controls.Add(this.timeOutMaskedTextBox);
            this.tabPageCOM.Controls.Add(this.timeLabel);
            this.tabPageCOM.Controls.Add(this.flussLabel);
            this.tabPageCOM.Controls.Add(this.flussComboBox);
            this.tabPageCOM.Controls.Add(this.stopBitsLabel);
            this.tabPageCOM.Controls.Add(this.stopBitsComboBox);
            this.tabPageCOM.Controls.Add(this.paritätLabel);
            this.tabPageCOM.Controls.Add(this.paritaetComboBox);
            this.tabPageCOM.Controls.Add(this.bitsSLabel);
            this.tabPageCOM.Controls.Add(this.bitsSComboBox);
            this.tabPageCOM.Controls.Add(this.comPortLabel);
            this.tabPageCOM.Controls.Add(this.comPortComboBox);
            resources.ApplyResources(this.tabPageCOM, "tabPageCOM");
            this.tabPageCOM.Name = "tabPageCOM";
            this.tabPageCOM.UseVisualStyleBackColor = true;
            // 
            // timeOutMaskedTextBox
            // 
            resources.ApplyResources(this.timeOutMaskedTextBox, "timeOutMaskedTextBox");
            this.timeOutMaskedTextBox.Name = "timeOutMaskedTextBox";
            this.timeOutMaskedTextBox.TextChanged += new System.EventHandler(this.timeOutMaskedTextBox_TextChanged);
            // 
            // timeLabel
            // 
            resources.ApplyResources(this.timeLabel, "timeLabel");
            this.timeLabel.Name = "timeLabel";
            // 
            // flussLabel
            // 
            resources.ApplyResources(this.flussLabel, "flussLabel");
            this.flussLabel.Name = "flussLabel";
            // 
            // flussComboBox
            // 
            resources.ApplyResources(this.flussComboBox, "flussComboBox");
            this.flussComboBox.FormattingEnabled = true;
            this.flussComboBox.Items.AddRange(new object[] {
            resources.GetString("flussComboBox.Items"),
            resources.GetString("flussComboBox.Items1"),
            resources.GetString("flussComboBox.Items2")});
            this.flussComboBox.Name = "flussComboBox";
            // 
            // stopBitsLabel
            // 
            resources.ApplyResources(this.stopBitsLabel, "stopBitsLabel");
            this.stopBitsLabel.Name = "stopBitsLabel";
            // 
            // stopBitsComboBox
            // 
            resources.ApplyResources(this.stopBitsComboBox, "stopBitsComboBox");
            this.stopBitsComboBox.FormattingEnabled = true;
            this.stopBitsComboBox.Items.AddRange(new object[] {
            resources.GetString("stopBitsComboBox.Items"),
            resources.GetString("stopBitsComboBox.Items1"),
            resources.GetString("stopBitsComboBox.Items2")});
            this.stopBitsComboBox.Name = "stopBitsComboBox";
            // 
            // paritätLabel
            // 
            resources.ApplyResources(this.paritätLabel, "paritätLabel");
            this.paritätLabel.Name = "paritätLabel";
            // 
            // paritaetComboBox
            // 
            resources.ApplyResources(this.paritaetComboBox, "paritaetComboBox");
            this.paritaetComboBox.FormattingEnabled = true;
            this.paritaetComboBox.Items.AddRange(new object[] {
            resources.GetString("paritaetComboBox.Items"),
            resources.GetString("paritaetComboBox.Items1"),
            resources.GetString("paritaetComboBox.Items2"),
            resources.GetString("paritaetComboBox.Items3"),
            resources.GetString("paritaetComboBox.Items4")});
            this.paritaetComboBox.Name = "paritaetComboBox";
            // 
            // bitsSLabel
            // 
            resources.ApplyResources(this.bitsSLabel, "bitsSLabel");
            this.bitsSLabel.Name = "bitsSLabel";
            // 
            // bitsSComboBox
            // 
            resources.ApplyResources(this.bitsSComboBox, "bitsSComboBox");
            this.bitsSComboBox.FormattingEnabled = true;
            this.bitsSComboBox.Items.AddRange(new object[] {
            resources.GetString("bitsSComboBox.Items"),
            resources.GetString("bitsSComboBox.Items1"),
            resources.GetString("bitsSComboBox.Items2"),
            resources.GetString("bitsSComboBox.Items3"),
            resources.GetString("bitsSComboBox.Items4"),
            resources.GetString("bitsSComboBox.Items5"),
            resources.GetString("bitsSComboBox.Items6"),
            resources.GetString("bitsSComboBox.Items7"),
            resources.GetString("bitsSComboBox.Items8"),
            resources.GetString("bitsSComboBox.Items9"),
            resources.GetString("bitsSComboBox.Items10"),
            resources.GetString("bitsSComboBox.Items11"),
            resources.GetString("bitsSComboBox.Items12")});
            this.bitsSComboBox.Name = "bitsSComboBox";
            // 
            // comPortLabel
            // 
            resources.ApplyResources(this.comPortLabel, "comPortLabel");
            this.comPortLabel.Name = "comPortLabel";
            // 
            // comPortComboBox
            // 
            this.comPortComboBox.FormattingEnabled = true;
            this.comPortComboBox.Items.AddRange(new object[] {
            resources.GetString("comPortComboBox.Items"),
            resources.GetString("comPortComboBox.Items1"),
            resources.GetString("comPortComboBox.Items2"),
            resources.GetString("comPortComboBox.Items3"),
            resources.GetString("comPortComboBox.Items4"),
            resources.GetString("comPortComboBox.Items5"),
            resources.GetString("comPortComboBox.Items6"),
            resources.GetString("comPortComboBox.Items7"),
            resources.GetString("comPortComboBox.Items8")});
            resources.ApplyResources(this.comPortComboBox, "comPortComboBox");
            this.comPortComboBox.Name = "comPortComboBox";
            this.comPortComboBox.SelectedIndexChanged += new System.EventHandler(this.comPortComboBox_SelectedIndexChanged_1);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.loglevelComboBox);
            this.tabPage1.Controls.Add(this.loglevelLabel);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // loglevelComboBox
            // 
            this.loglevelComboBox.FormattingEnabled = true;
            this.loglevelComboBox.Items.AddRange(new object[] {
            resources.GetString("loglevelComboBox.Items"),
            resources.GetString("loglevelComboBox.Items1")});
            resources.ApplyResources(this.loglevelComboBox, "loglevelComboBox");
            this.loglevelComboBox.Name = "loglevelComboBox";
            this.loglevelComboBox.SelectedIndexChanged += new System.EventHandler(this.loglevelComboBox_SelectedIndexChanged);
            // 
            // loglevelLabel
            // 
            resources.ApplyResources(this.loglevelLabel, "loglevelLabel");
            this.loglevelLabel.Name = "loglevelLabel";
            // 
            // divTabPage
            // 
            this.divTabPage.Controls.Add(this.writeSleepMaskedTextBox);
            this.divTabPage.Controls.Add(this.writeSleepLabel);
            this.divTabPage.Controls.Add(this.splashCheckBox);
            resources.ApplyResources(this.divTabPage, "divTabPage");
            this.divTabPage.Name = "divTabPage";
            this.divTabPage.UseVisualStyleBackColor = true;
            // 
            // writeSleepMaskedTextBox
            // 
            this.writeSleepMaskedTextBox.Culture = new System.Globalization.CultureInfo("");
            resources.ApplyResources(this.writeSleepMaskedTextBox, "writeSleepMaskedTextBox");
            this.writeSleepMaskedTextBox.Name = "writeSleepMaskedTextBox";
            this.writeSleepMaskedTextBox.ValidatingType = typeof(int);
            this.writeSleepMaskedTextBox.TextChanged += new System.EventHandler(this.writeSleepMaskedTextBox_TextChanged);
            // 
            // writeSleepLabel
            // 
            resources.ApplyResources(this.writeSleepLabel, "writeSleepLabel");
            this.writeSleepLabel.Name = "writeSleepLabel";
            // 
            // splashCheckBox
            // 
            resources.ApplyResources(this.splashCheckBox, "splashCheckBox");
            this.splashCheckBox.Checked = true;
            this.splashCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.splashCheckBox.Name = "splashCheckBox";
            this.splashCheckBox.UseVisualStyleBackColor = true;
            this.splashCheckBox.CheckedChanged += new System.EventHandler(this.splashCheckBox_CheckedChanged);
            // 
            // Configuration
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.configurationTabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Configuration";
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Configuration_FormClosing);
            this.configurationTabControl.ResumeLayout(false);
            this.tabPageCOM.ResumeLayout(false);
            this.tabPageCOM.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.divTabPage.ResumeLayout(false);
            this.divTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPageCOM;
        private System.Windows.Forms.ComboBox comPortComboBox;
        private System.Windows.Forms.Label comPortLabel;
        private System.Windows.Forms.Label bitsSLabel;
        private System.Windows.Forms.ComboBox bitsSComboBox;
        private System.Windows.Forms.Label paritätLabel;
        private System.Windows.Forms.ComboBox paritaetComboBox;
        private System.Windows.Forms.Label flussLabel;
        private System.Windows.Forms.ComboBox flussComboBox;
        private System.Windows.Forms.Label stopBitsLabel;
        private System.Windows.Forms.ComboBox stopBitsComboBox;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ComboBox loglevelComboBox;
        private System.Windows.Forms.Label loglevelLabel;
        public System.Windows.Forms.TabControl configurationTabControl;
        private System.Windows.Forms.TabPage divTabPage;
        private System.Windows.Forms.CheckBox splashCheckBox;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.MaskedTextBox writeSleepMaskedTextBox;
        private System.Windows.Forms.Label writeSleepLabel;
        private System.Windows.Forms.MaskedTextBox timeOutMaskedTextBox;
    }
}