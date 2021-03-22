using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.BusinessLogics;
using System;
using System.Windows.Forms;
using Unity;

namespace SecuritySystemView
{
    public partial class FormStoreHouses : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly StoreHouseLogic storeHouseLogic;

        public FormStoreHouses(StoreHouseLogic storeHouseLogic)
        {
            InitializeComponent();
            this.storeHouseLogic = storeHouseLogic;
        }

        private void FormStoreHouses_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var list = storeHouseLogic.Read(null);
                if (list != null)
                {
                    dataGridViewStoreHouses.DataSource = list;
                    dataGridViewStoreHouses.Columns[0].Visible = false;
                    dataGridViewStoreHouses.Columns[1].AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.Fill;
                    dataGridViewStoreHouses.Columns[4].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormStoreHouse>();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadData();
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridViewStoreHouses.SelectedRows.Count == 1)
            {
                var form = Container.Resolve<FormStoreHouse>();
                form.Id = Convert.ToInt32(dataGridViewStoreHouses.SelectedRows[0].Cells[0].Value);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewStoreHouses.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo,
               MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id =
                   Convert.ToInt32(dataGridViewStoreHouses.SelectedRows[0].Cells[0].Value);
                    try
                    {
                        storeHouseLogic.Delete(new StoreHouseBindingModel { Id = id });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}