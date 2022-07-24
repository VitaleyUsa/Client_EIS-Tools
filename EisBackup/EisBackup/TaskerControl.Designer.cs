namespace EisBackup
{
    partial class TaskerControl
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btSaveSet = new System.Windows.Forms.Button();
            this.btnDeleteSet = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(21, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 18);
            this.label2.TabIndex = 51;
            this.label2.Text = "Текущее состояние:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 19);
            this.label1.TabIndex = 50;
            this.label1.Text = "Планировщик";
            // 
            // btSaveSet
            // 
            this.btSaveSet.Location = new System.Drawing.Point(212, 271);
            this.btSaveSet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btSaveSet.Name = "btSaveSet";
            this.btSaveSet.Size = new System.Drawing.Size(118, 33);
            this.btSaveSet.TabIndex = 55;
            this.btSaveSet.Text = "Создать";
            this.btSaveSet.UseVisualStyleBackColor = true;
            this.btSaveSet.Click += new System.EventHandler(this.BtSaveSet_Click);
            // 
            // btnDeleteSet
            // 
            this.btnDeleteSet.Location = new System.Drawing.Point(88, 271);
            this.btnDeleteSet.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDeleteSet.Name = "btnDeleteSet";
            this.btnDeleteSet.Size = new System.Drawing.Size(118, 33);
            this.btnDeleteSet.TabIndex = 55;
            this.btnDeleteSet.Text = "Отключить";
            this.btnDeleteSet.UseVisualStyleBackColor = true;
            this.btnDeleteSet.Click += new System.EventHandler(this.BtnDeleteSet_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(39, 126);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(287, 19);
            this.lblStatus.TabIndex = 51;
            this.lblStatus.Text = "Задача не запланирована";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(20, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(291, 31);
            this.label4.TabIndex = 56;
            this.label4.Text = "Вы можете запланировать задачу для резервного копирования баз данных ЕИС \"Енот\".";
            // 
            // TaskerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnDeleteSet);
            this.Controls.Add(this.btSaveSet);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "TaskerControl";
            this.Size = new System.Drawing.Size(338, 327);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btSaveSet;
        private System.Windows.Forms.Button btnDeleteSet;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label4;
    }
}
