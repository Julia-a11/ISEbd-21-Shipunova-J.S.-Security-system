
namespace SecuritySystemView
{
    partial class FormReportStoreHouseComponents
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
            this.dataGridViewReportStoreHouseComponents = new System.Windows.Forms.DataGridView();
            this.buttonSaveToExcel = new System.Windows.Forms.Button();
            this.StoreHouseColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComponentColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CountColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReportStoreHouseComponents)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewReportStoreHouseComponents
            // 
            this.dataGridViewReportStoreHouseComponents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewReportStoreHouseComponents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StoreHouseColumn,
            this.ComponentColumn,
            this.CountColumn});
            this.dataGridViewReportStoreHouseComponents.Location = new System.Drawing.Point(12, 55);
            this.dataGridViewReportStoreHouseComponents.Name = "dataGridViewReportStoreHouseComponents";
            this.dataGridViewReportStoreHouseComponents.Size = new System.Drawing.Size(507, 389);
            this.dataGridViewReportStoreHouseComponents.TabIndex = 3;
            // 
            // buttonSaveToExcel
            // 
            this.buttonSaveToExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonSaveToExcel.Location = new System.Drawing.Point(12, 12);
            this.buttonSaveToExcel.Name = "buttonSaveToExcel";
            this.buttonSaveToExcel.Size = new System.Drawing.Size(139, 27);
            this.buttonSaveToExcel.TabIndex = 2;
            this.buttonSaveToExcel.Text = "Сохранить в эксель";
            this.buttonSaveToExcel.UseVisualStyleBackColor = true;
            this.buttonSaveToExcel.Click += new System.EventHandler(this.buttonSaveToExcel_Click);
            // 
            // StoreHouseColumn
            // 
            this.StoreHouseColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.StoreHouseColumn.HeaderText = "Склад";
            this.StoreHouseColumn.Name = "StoreHouseColumn";
            // 
            // ComponentColumn
            // 
            this.ComponentColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ComponentColumn.HeaderText = "Компонент";
            this.ComponentColumn.Name = "ComponentColumn";
            // 
            // CountColumn
            // 
            this.CountColumn.HeaderText = "Количество";
            this.CountColumn.Name = "CountColumn";
            // 
            // FormReportStoreHouseComponents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 458);
            this.Controls.Add(this.dataGridViewReportStoreHouseComponents);
            this.Controls.Add(this.buttonSaveToExcel);
            this.Name = "FormReportStoreHouseComponents";
            this.Text = "Отчёт по складам";
            this.Load += new System.EventHandler(this.FormReportStoreHouseComponents_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReportStoreHouseComponents)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewReportStoreHouseComponents;
        private System.Windows.Forms.Button buttonSaveToExcel;
        private System.Windows.Forms.DataGridViewTextBoxColumn StoreHouseColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComponentColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CountColumn;
    }
}