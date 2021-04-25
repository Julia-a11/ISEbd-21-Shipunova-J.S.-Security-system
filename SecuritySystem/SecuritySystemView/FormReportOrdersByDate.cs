using Microsoft.Reporting.WinForms;
using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.BusinessLogics;
using System;
using System.Windows.Forms;
using Unity;

namespace SecuritySystemView
{
    public partial class FormReportOrdersByDate : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly ReportLogic logic;

        public FormReportOrdersByDate(ReportLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            try
            {
                var dataSource = logic.GetOrdersInfo();
               
                ReportDataSource source = new ReportDataSource("DataSetOrders", dataSource);
                reportViewerOrders.LocalReport.DataSources.Add(source);
                reportViewerOrders.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog { Filter = "pdf|*.pdf" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        logic.SaveOrdersInfoToPdfFile(new ReportBindingModel
                        {
                            FileName = dialog.FileName
                        });
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
