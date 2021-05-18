using System;
using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.BusinessLogics;
using System.Windows.Forms;
using Unity;
using System.Reflection;

namespace SecuritySystemView
{
    public partial class FormMain : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly OrderLogic _orderLogic;

        private readonly ReportLogic _reportLogic;

        private readonly WorkModeling _workModeling;

        private readonly BackUpAbstractLogic _backUpAbstractLogic;

        public FormMain(OrderLogic orderLogic, ReportLogic reportLogic, WorkModeling workModeling, BackUpAbstractLogic backUpAbstractLogic)
        {
            InitializeComponent();
            _orderLogic = orderLogic;
            _reportLogic = reportLogic;
            _workModeling = workModeling;
            _backUpAbstractLogic = backUpAbstractLogic;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                Program.ConfigGrid(_orderLogic.Read(null), dataGridViewOrders);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }

        private void компонентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormComponents>();
            form.ShowDialog();
        }

        private void комплектацииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormSecures>();
            form.ShowDialog();
        }

        private void buttonCreateOrder_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormCreateOrder>();
            form.ShowDialog();
            LoadData();
        }

        private void buttonOrderPayed_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrders.SelectedRows.Count == 1)
            {
                int id = Convert.ToInt32(dataGridViewOrders.SelectedRows[0].Cells[0].Value);
                try
                {
                    _orderLogic.PayOrder(new ChangeStatusBindingModel { OrderId = id });
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                }
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void списокКомплектовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog { Filter = "docx|*.docx" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    MethodInfo method = _reportLogic.GetType().GetMethod("SaveSecuresToWordFile");
                    method.Invoke(_reportLogic, new object[]
                        {
                            new ReportBindingModel
                            {
                                FileName = dialog.FileName
                            }
                        });
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK,
                   MessageBoxIcon.Information);
                }
            }
        }

        private void компонентыПоКомплектациямToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormReportSecureComponents>();
            form.ShowDialog();
        }

        private void списокЗаказовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormReportOrders>();
            form.ShowDialog();
        }

        private void складыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormStoreHouses>();
            form.ShowDialog();
        }

        private void пополнениеСкладаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormReplenishmentStoreHouse>();
            form.ShowDialog();
        }

        private void списокКомпонентовВСкладахToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormReportStoreHouseComponents>();
            form.ShowDialog();
        }

        private void списокИнформацииОЗаказахToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormReportOrdersByDate>();
            form.ShowDialog();
        }

        private void списокСкладовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog { Filter = "docx|*.docx" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    MethodInfo method = _reportLogic.GetType().GetMethod("SaveStoreHousesToWordFile");
                    method.Invoke(_reportLogic, new object[] { new ReportBindingModel
                    {
                        FileName = dialog.FileName
                    }});
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK,
                   MessageBoxIcon.Information);
                }
            }
        }

        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormClients>();
            form.ShowDialog();
        }

        private void исполнителиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormImplementers>();
            form.ShowDialog();
        }

        private void запускРаботToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _workModeling.DoWork();
            LoadData();
        }

        private void почтаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormMails>();
            form.ShowDialog();
        }

        private void создатьБекапToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_backUpAbstractLogic != null)
                {
                    var fbd = new FolderBrowserDialog();
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        _backUpAbstractLogic.CreateArchive(fbd.SelectedPath);
                        MessageBox.Show("Бекап создан", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
