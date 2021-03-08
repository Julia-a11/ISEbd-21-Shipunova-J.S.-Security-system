using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.BusinessLogics;
using SecuritySystemBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Unity;

namespace SecuritySystemView
{
    public partial class FormStoreHouse : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly StoreHouseLogic logic;

        private int? id;

        private Dictionary<int, (string, int)> storeHouseComponents;

        public FormStoreHouse(StoreHouseLogic storeHouselogic)
        {
            InitializeComponent();
            this.logic = storeHouselogic;
        }

        private void FormStoreHouse_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    StoreHouseViewModel view = logic.Read(new StoreHouseBindingModel
                    {
                        Id = id.Value
                    })?[0];
                    if (view != null)
                    {
                        textBoxName.Text = view.StoreHouseName;
                        textBoxFCS.Text = view.ResponsiblePersonFCS;
                        storeHouseComponents = view.StoreHouseComponents;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                }
            }
            else
            {
                storeHouseComponents = new Dictionary<int, (string, int)>();
            }
        }

        private void LoadData()
        {
            try
            {
                if (storeHouseComponents != null)
                {
                    dataGridView.Rows.Clear();
                    foreach (var storeHouseComponent in storeHouseComponents)
                    {
                        dataGridView.Rows.Add(new object[] { storeHouseComponent.Key, storeHouseComponent.Value.Item1, storeHouseComponent.Value.Item2 });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }

        
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxFCS.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            try
            {
                logic.CreateOrUpdate(new StoreHouseBindingModel
                {
                    Id = id,
                    StoreHouseName = textBoxName.Text,
                    ResponsiblePersonFCS = textBoxFCS.Text,
                    StoreHouseComponents = storeHouseComponents
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}