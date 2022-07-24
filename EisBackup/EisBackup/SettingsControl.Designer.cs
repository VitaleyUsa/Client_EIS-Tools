namespace EisBackup
{
    partial class SettingsControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.btSaveSet = new System.Windows.Forms.Button();
            this.tbLogin = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbDatabase = new System.Windows.Forms.TextBox();
            this.tbServer = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 19);
            this.label1.TabIndex = 47;
            this.label1.Text = "Настройки MySQL";
            // 
            // btSaveSet
            // 
            this.btSaveSet.Location = new System.Drawing.Point(212, 271);
            this.btSaveSet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btSaveSet.Name = "btSaveSet";
            this.btSaveSet.Size = new System.Drawing.Size(118, 33);
            this.btSaveSet.TabIndex = 37;
            this.btSaveSet.Text = "Сохранить";
            this.btSaveSet.UseVisualStyleBackColor = true;
            this.btSaveSet.Click += new System.EventHandler(this.BtSaveSet_Click);
            // 
            // tbLogin
            // 
            this.tbLogin.Location = new System.Drawing.Point(24, 122);
            this.tbLogin.Name = "tbLogin";
            this.tbLogin.ReadOnly = true;
            this.tbLogin.Size = new System.Drawing.Size(302, 20);
            this.tbLogin.TabIndex = 5;
            this.tbLogin.Text = "root";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(24, 158);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.ReadOnly = true;
            this.tbPassword.Size = new System.Drawing.Size(302, 20);
            this.tbPassword.TabIndex = 6;
            this.tbPassword.Text = "haha_no_pass_for_u :p";
            this.tbPassword.UseSystemPasswordChar = true;
            // 
            // tbDatabase
            // 
            this.tbDatabase.Location = new System.Drawing.Point(24, 193);
            this.tbDatabase.Name = "tbDatabase";
            this.tbDatabase.Size = new System.Drawing.Size(302, 20);
            this.tbDatabase.TabIndex = 7;
            // 
            // tbServer
            // 
            this.tbServer.Location = new System.Drawing.Point(24, 229);
            this.tbServer.Name = "tbServer";
            this.tbServer.Size = new System.Drawing.Size(302, 20);
            this.tbServer.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(20, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(291, 31);
            this.label3.TabIndex = 49;
            this.label3.Text = "Заполните данные для подключения к серверу MySQL и нажмите кнопку \"Сохранить\"";
            // 
            // SettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbServer);
            this.Controls.Add(this.tbDatabase);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbLogin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btSaveSet);
            this.Name = "SettingsControl";
            this.Size = new System.Drawing.Size(341, 321);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btSaveSet;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox tbLogin;
        public System.Windows.Forms.TextBox tbPassword;
        public System.Windows.Forms.TextBox tbDatabase;
        public System.Windows.Forms.TextBox tbServer;
    }
}
