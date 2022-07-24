namespace EisBackup
{
    partial class ExportControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.lbRowInAllTable = new System.Windows.Forms.Label();
            this.lbRowInCurTable = new System.Windows.Forms.Label();
            this.lbTableCount = new System.Windows.Forms.Label();
            this.pbRowInAllTable = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.pbRowInCurTable = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.pbTable = new System.Windows.Forms.ProgressBar();
            this.lbCurrentTableName = new System.Windows.Forms.Label();
            this.btCancel = new System.Windows.Forms.Button();
            this.btExport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(225, 19);
            this.label1.TabIndex = 35;
            this.label1.Text = "Экспорт базы данных ЕИС";
            // 
            // lbRowInAllTable
            // 
            this.lbRowInAllTable.AutoSize = true;
            this.lbRowInAllTable.Location = new System.Drawing.Point(24, 248);
            this.lbRowInAllTable.Name = "lbRowInAllTable";
            this.lbRowInAllTable.Size = new System.Drawing.Size(30, 13);
            this.lbRowInAllTable.TabIndex = 34;
            this.lbRowInAllTable.Text = "1 / 1";
            // 
            // lbRowInCurTable
            // 
            this.lbRowInCurTable.AutoSize = true;
            this.lbRowInCurTable.Location = new System.Drawing.Point(24, 172);
            this.lbRowInCurTable.Name = "lbRowInCurTable";
            this.lbRowInCurTable.Size = new System.Drawing.Size(30, 13);
            this.lbRowInCurTable.TabIndex = 33;
            this.lbRowInCurTable.Text = "1 / 1";
            // 
            // lbTableCount
            // 
            this.lbTableCount.AutoSize = true;
            this.lbTableCount.Location = new System.Drawing.Point(24, 98);
            this.lbTableCount.Name = "lbTableCount";
            this.lbTableCount.Size = new System.Drawing.Size(30, 13);
            this.lbTableCount.TabIndex = 32;
            this.lbTableCount.Text = "1 / 1";
            // 
            // pbRowInAllTable
            // 
            this.pbRowInAllTable.Location = new System.Drawing.Point(24, 222);
            this.pbRowInAllTable.Name = "pbRowInAllTable";
            this.pbRowInAllTable.Size = new System.Drawing.Size(306, 23);
            this.pbRowInAllTable.TabIndex = 31;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 203);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "Строк во всех таблицах";
            // 
            // pbRowInCurTable
            // 
            this.pbRowInCurTable.Location = new System.Drawing.Point(24, 146);
            this.pbRowInCurTable.Name = "pbRowInCurTable";
            this.pbRowInCurTable.Size = new System.Drawing.Size(306, 23);
            this.pbRowInCurTable.TabIndex = 29;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "Строк в текущей таблице";
            // 
            // pbTable
            // 
            this.pbTable.Location = new System.Drawing.Point(24, 72);
            this.pbTable.Name = "pbTable";
            this.pbTable.Size = new System.Drawing.Size(306, 23);
            this.pbTable.TabIndex = 27;
            // 
            // lbCurrentTableName
            // 
            this.lbCurrentTableName.AutoSize = true;
            this.lbCurrentTableName.Location = new System.Drawing.Point(24, 53);
            this.lbCurrentTableName.Name = "lbCurrentTableName";
            this.lbCurrentTableName.Size = new System.Drawing.Size(96, 13);
            this.lbCurrentTableName.TabIndex = 26;
            this.lbCurrentTableName.Text = "Текущая таблица";
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(88, 271);
            this.btCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(118, 33);
            this.btCancel.TabIndex = 25;
            this.btCancel.Text = "Отменить экспорт";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btExport
            // 
            this.btExport.Location = new System.Drawing.Point(212, 271);
            this.btExport.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btExport.Name = "btExport";
            this.btExport.Size = new System.Drawing.Size(118, 33);
            this.btExport.TabIndex = 24;
            this.btExport.Text = "Начать экспорт";
            this.btExport.UseVisualStyleBackColor = true;
            this.btExport.Click += new System.EventHandler(this.btExport_Click);
            // 
            // ExportControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbRowInAllTable);
            this.Controls.Add(this.lbRowInCurTable);
            this.Controls.Add(this.lbTableCount);
            this.Controls.Add(this.pbRowInAllTable);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pbRowInCurTable);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pbTable);
            this.Controls.Add(this.lbCurrentTableName);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btExport);
            this.Name = "ExportControl";
            this.Size = new System.Drawing.Size(363, 325);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbRowInAllTable;
        private System.Windows.Forms.Label lbRowInCurTable;
        private System.Windows.Forms.Label lbTableCount;
        private System.Windows.Forms.ProgressBar pbRowInAllTable;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar pbRowInCurTable;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar pbTable;
        private System.Windows.Forms.Label lbCurrentTableName;
        private System.Windows.Forms.Button btCancel;
        public System.Windows.Forms.Button btExport;
    }
}
