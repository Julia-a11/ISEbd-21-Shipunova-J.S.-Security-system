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
    public partial class FormReportSecureComponents : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly ReportLogic logic;

        public FormReportSecureComponents(ReportLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void FormReportSecureComponents_Load(object sender, EventArgs e)
        {
            try
            {
                MethodInfo method = logic.GetType().GetMethod("GetSecureComponent");
                List<ReportSecureComponentViewModel> secureComponents = (List<ReportSecureComponentViewModel>)method.Invoke(logic, null);
                if (secureComponents != null)
                {
                    dataGridViewReportSecureComponents.Rows.Clear();
                    foreach (var secureComponent in secureComponents)
                    {
                        dataGridViewReportSecureComponents.Rows.Add(new object[]
                        {
                            secureComponent.SecureName, "", ""
                        });
                        foreach (var listElem in secureComponent.Components)
                        {
                            dataGridViewReportSecureComponents.Rows.Add(new object[]
                            {
                                "", listElem.Item1, listElem.Item2
                            });
                        }
                        dataGridViewReportSecureComponents.Rows.Add(new object[]
                        {
                            "Итого", "", secureComponent.TotalCount
                        });
                        dataGridViewReportSecureComponents.Rows.Add(new object[] { });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        MethodInfo method = logic.GetType().GetMethod("SaveSecureComponentToExcelFile");
                        method.Invoke(logic, new object[]
                        {
                            new ReportBindingModel
                        {
                            FileName = dialog.FileName
                        }
                        });
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}