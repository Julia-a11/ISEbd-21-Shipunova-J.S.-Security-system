using Microsoft.Reporting.WinForms;
using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.BusinessLogics;
using SecuritySystemBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Reflection;
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
                MethodInfo method = logic.GetType().GetMethod("GetOrdersInfo");
                List<ReportOrderByDateViewModel> dataSource = (List<ReportOrderByDateViewModel>)method.Invoke(logic, null);

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
                        MethodInfo method = logic.GetType().GetMethod("SaveOrdersInfoToPdfFile");
                        method.Invoke(logic, new object[]
                        { 
                            new ReportBindingModel
                            {
                                FileName = dialog.FileName
                            }
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
