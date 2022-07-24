namespace EisBackup
{
    partial class ImportControl
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.btCancel = new System.Windows.Forms.Button();
            this.btImport = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lb2import = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(20, 187);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(302, 23);
            this.progressBar1.TabIndex = 25;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 157);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Скопировано байт:";
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(88, 271);
            this.btCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(118, 33);
            this.btCancel.TabIndex = 22;
            this.btCancel.Text = "Отмена импорта";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btImport
            // 
            this.btImport.Location = new System.Drawing.Point(212, 271);
            this.btImport.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btImport.Name = "btImport";
            this.btImport.Size = new System.Drawing.Size(118, 33);
            this.btImport.TabIndex = 21;
            this.btImport.Text = "Начать импорт";
            this.btImport.UseVisualStyleBackColor = true;
            this.btImport.Click += new System.EventHandler(this.btImport_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(220, 19);
            this.label1.TabIndex = 21;
            this.label1.Text = "Импорт базы данных ЕИС";
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(129, 157);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(63, 13);
            this.lbStatus.TabIndex = 28;
            this.lbStatus.Text = "0 из 0 байт";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(20, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(291, 31);
            this.label3.TabIndex = 29;
            this.label3.Text = "После нажатия кнопки \"Начать импорт\" будет предложено выбрать файл для восстановл" +
    "ения.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "Файл для импорта:";
            // 
            // lb2import
            // 
            this.lb2import.Location = new System.Drawing.Point(31, 114);
            this.lb2import.Name = "lb2import";
            this.lb2import.Size = new System.Drawing.Size(280, 32);
            this.lb2import.TabIndex = 30;
            this.lb2import.Text = "Не выбран";
            // 
            // ImportControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lb2import);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btImport);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.progressBar1);
            this.Name = "ImportControl";
            this.Size = new System.Drawing.Size(342, 326);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btImport;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label lb2import;
    }
}
