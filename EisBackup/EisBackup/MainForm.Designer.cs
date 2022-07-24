namespace EisBackup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btSettings = new System.Windows.Forms.Button();
            this.btExport = new System.Windows.Forms.Button();
            this.btImport = new System.Windows.Forms.Button();
            this.btTasker = new System.Windows.Forms.Button();
            this.settingsControl1 = new EisBackup.SettingsControl();
            this.exportControl1 = new EisBackup.ExportControl();
            this.importControl1 = new EisBackup.ImportControl();
            this.taskerControl1 = new EisBackup.TaskerControl();
            this.btAbout = new System.Windows.Forms.Button();
            this.aboutControl1 = new EisBackup.AboutControl();
            this.SuspendLayout();
            // 
            // btSettings
            // 
            this.btSettings.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btSettings.FlatAppearance.BorderSize = 0;
            this.btSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSettings.Location = new System.Drawing.Point(21, 71);
            this.btSettings.Name = "btSettings";
            this.btSettings.Size = new System.Drawing.Size(159, 23);
            this.btSettings.TabIndex = 1;
            this.btSettings.Text = "Настройки";
            this.btSettings.UseVisualStyleBackColor = false;
            this.btSettings.Click += new System.EventHandler(this.BtSettings_Click);
            // 
            // btExport
            // 
            this.btExport.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btExport.FlatAppearance.BorderSize = 0;
            this.btExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btExport.Location = new System.Drawing.Point(21, 100);
            this.btExport.Name = "btExport";
            this.btExport.Size = new System.Drawing.Size(159, 23);
            this.btExport.TabIndex = 2;
            this.btExport.Text = "Резервирование";
            this.btExport.UseVisualStyleBackColor = false;
            this.btExport.Click += new System.EventHandler(this.btExport_Click);
            // 
            // btImport
            // 
            this.btImport.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btImport.FlatAppearance.BorderSize = 0;
            this.btImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btImport.Location = new System.Drawing.Point(21, 129);
            this.btImport.Name = "btImport";
            this.btImport.Size = new System.Drawing.Size(159, 23);
            this.btImport.TabIndex = 3;
            this.btImport.Text = "Восстановление";
            this.btImport.UseVisualStyleBackColor = false;
            this.btImport.Click += new System.EventHandler(this.BtImport_Click);
            // 
            // btTasker
            // 
            this.btTasker.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btTasker.FlatAppearance.BorderSize = 0;
            this.btTasker.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btTasker.Location = new System.Drawing.Point(21, 158);
            this.btTasker.Name = "btTasker";
            this.btTasker.Size = new System.Drawing.Size(159, 23);
            this.btTasker.TabIndex = 4;
            this.btTasker.Text = "Планировщик";
            this.btTasker.UseVisualStyleBackColor = false;
            this.btTasker.Click += new System.EventHandler(this.BtTasker_Click);
            // 
            // settingsControl1
            // 
            this.settingsControl1.Location = new System.Drawing.Point(186, 7);
            this.settingsControl1.Name = "settingsControl1";
            this.settingsControl1.Size = new System.Drawing.Size(341, 327);
            this.settingsControl1.TabIndex = 4;
            // 
            // exportControl1
            // 
            this.exportControl1.Location = new System.Drawing.Point(186, 8);
            this.exportControl1.Name = "exportControl1";
            this.exportControl1.Size = new System.Drawing.Size(341, 331);
            this.exportControl1.TabIndex = 5;
            // 
            // importControl1
            // 
            this.importControl1.Location = new System.Drawing.Point(185, 8);
            this.importControl1.Name = "importControl1";
            this.importControl1.Size = new System.Drawing.Size(342, 327);
            this.importControl1.TabIndex = 6;
            // 
            // taskerControl1
            // 
            this.taskerControl1.Location = new System.Drawing.Point(185, 8);
            this.taskerControl1.Name = "taskerControl1";
            this.taskerControl1.Size = new System.Drawing.Size(338, 310);
            this.taskerControl1.TabIndex = 7;
            // 
            // btAbout
            // 
            this.btAbout.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btAbout.FlatAppearance.BorderSize = 0;
            this.btAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btAbout.Location = new System.Drawing.Point(21, 187);
            this.btAbout.Name = "btAbout";
            this.btAbout.Size = new System.Drawing.Size(159, 23);
            this.btAbout.TabIndex = 4;
            this.btAbout.Text = "О программе";
            this.btAbout.UseVisualStyleBackColor = false;
            this.btAbout.Click += new System.EventHandler(this.BtAbout_Click);
            // 
            // aboutControl1
            // 
            this.aboutControl1.Location = new System.Drawing.Point(186, 7);
            this.aboutControl1.Name = "aboutControl1";
            this.aboutControl1.Size = new System.Drawing.Size(374, 311);
            this.aboutControl1.TabIndex = 8;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 346);
            this.Controls.Add(this.btAbout);
            this.Controls.Add(this.btTasker);
            this.Controls.Add(this.btImport);
            this.Controls.Add(this.btExport);
            this.Controls.Add(this.btSettings);
            this.Controls.Add(this.settingsControl1);
            this.Controls.Add(this.exportControl1);
            this.Controls.Add(this.importControl1);
            this.Controls.Add(this.taskerControl1);
            this.Controls.Add(this.aboutControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Резервное копирование ЕИС Енот";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private SettingsControl settingsControl1;
        private ExportControl exportControl1;
        private ImportControl importControl1;
        public System.Windows.Forms.Button btSettings;
        public System.Windows.Forms.Button btExport;
        public System.Windows.Forms.Button btImport;
        public System.Windows.Forms.Button btTasker;
        private TaskerControl taskerControl1;
        public System.Windows.Forms.Button btAbout;
        private AboutControl aboutControl1;
    }
}

