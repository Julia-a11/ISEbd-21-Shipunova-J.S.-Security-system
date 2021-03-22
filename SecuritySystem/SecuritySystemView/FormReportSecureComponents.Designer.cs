
namespace SecuritySystemView
{
    partial class FormReportSecureComponents
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
            this.buttonSaveToExcel = new System.Windows.Forms.Button();
            this.dataGridViewReportSecureComponents = new System.Windows.Forms.DataGridView();
            this.Component = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Secure = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReportSecureComponents)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSaveToExcel
            // 
            this.buttonSaveToExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonSaveToExcel.Location = new System.Drawing.Point(12, 12);
            this.buttonSaveToExcel.Name = "buttonSaveToExcel";
            this.buttonSaveToExcel.Size = new System.Drawing.Size(139, 27);
            this.buttonSaveToExcel.TabIndex = 0;
            this.buttonSaveToExcel.Text = "Сохранить в эксель";
            this.buttonSaveToExcel.UseVisualStyleBackColor = true;
            this.buttonSaveToExcel.Click += new System.EventHandler(this.buttonSaveToExcel_Click);
            // 
            // dataGridViewReportSecureComponents
            // 
            this.dataGridViewReportSecureComponents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewReportSecureComponents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Component,
            this.Secure,
            this.Count});
            this.dataGridViewReportSecureComponents.Location = new System.Drawing.Point(6, 55);
            this.dataGridViewReportSecureComponents.Name = "dataGridViewReportSecureComponents";
            this.dataGridViewReportSecureComponents.Size = new System.Drawing.Size(515, 412);
            this.dataGridViewReportSecureComponents.TabIndex = 1;
            // 
            // Component
            // 
            this.Component.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Component.HeaderText = "Компонент";
            this.Component.Name = "Component";
            // 
            // Secure
            // 
            this.Secure.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Secure.HeaderText = "Комплектация";
            this.Secure.Name = "Secure";
            // 
            // Count
            // 
            this.Count.HeaderText = "Количество";
            this.Count.Name = "Count";
            // 
            // FormReportSecureComponents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 479);
            this.Controls.Add(this.dataGridViewReportSecureComponents);
            this.Controls.Add(this.buttonSaveToExcel);
            this.Name = "FormReportSecureComponents";
            this.Text = "Компоненты по изделиям";
            this.Load += new System.EventHandler(this.FormReportSecureComponents_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewReportSecureComponents)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSaveToExcel;
        private System.Windows.Forms.DataGridView dataGridViewReportSecureComponents;
        private System.Windows.Forms.DataGridViewTextBoxColumn Component;
        private System.Windows.Forms.DataGridViewTextBoxColumn Secure;
        private System.Windows.Forms.DataGridViewTextBoxColumn Count;
    }
}