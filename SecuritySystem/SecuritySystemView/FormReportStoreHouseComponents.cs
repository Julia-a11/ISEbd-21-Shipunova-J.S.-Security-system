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
    public partial class FormReportStoreHouseComponents : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly ReportLogic logic;

        public FormReportStoreHouseComponents(ReportLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void FormReportStoreHouseComponents_Load(object sender, EventArgs e)
        {
            try
            {
                MethodInfo method = logic.GetType().GetMethod("GetSecureComponent");
                List<ReportStoreHouseComponentsViewModel> dict = (List<ReportStoreHouseComponentsViewModel>)method.Invoke(logic, null);
                if (dict != null)
                {
                    dataGridViewReportStoreHouseComponents.Rows.Clear();
                    foreach (var elem in dict)
                    {
                        dataGridViewReportStoreHouseComponents.Rows.Add(new object[]
                        {
                            elem.StoreHouseName, "", ""
                        });
                        foreach (var listElem in elem.Components)
                        {
                            dataGridViewReportStoreHouseComponents.Rows.Add(new object[]
                            {
                                "", listElem.Item1, listElem.Item2
                            });
                        }
                        dataGridViewReportStoreHouseComponents.Rows.Add(new object[]
                        {
                            "Итого", "", elem.TotalCount
                        });
                        dataGridViewReportStoreHouseComponents.Rows.Add(new object[] { });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
MessageBoxIcon.Error);
            }
        }

        private void buttonSaveToExcel_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog { Filter = "xlsx|*.xlsx" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        MethodInfo method = logic.GetType().GetMethod("SaveStoreHouseComponentsToExcel");
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
