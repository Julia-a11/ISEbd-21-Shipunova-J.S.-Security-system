using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.BusinessLogics;
using SecuritySystemBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;

namespace SecuritySystemView
{
    public partial class FormReplenishmentStoreHouse : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int ComponentId
        {
            get { return Convert.ToInt32(comboBoxComponent.SelectedValue); }
            set { comboBoxComponent.SelectedValue = value; }
        }

        public int StoreHouse
        {
            get { return Convert.ToInt32(comboBoxComponent.SelectedValue); }
            set { comboBoxComponent.SelectedValue = value; }
        }

        public int Count
        {
            get { return Convert.ToInt32(textBoxCount.Text); }
            set
            {
                textBoxCount.Text = value.ToString();
            }
        }

        private readonly StoreHouseLogic storeHouseLogic;

        public FormReplenishmentStoreHouse(ComponentLogic logicComponent, StoreHouseLogic logicStoreHouse )
        {
            InitializeComponent();
            storeHouseLogic = logicStoreHouse;

            List<ComponentViewModel> listComponents = logicComponent.Read(null);
            if (listComponents != null)
            {
                comboBoxComponent.DisplayMember = "ComponentName";
                comboBoxComponent.ValueMember = "Id";
                comboBoxComponent.DataSource = listComponents;
                comboBoxComponent.SelectedItem = null;
            }

            List<StoreHouseViewModel> listStoreHouses = logicStoreHouse.Read(null);
            if (listStoreHouses != null)
            {
                comboBoxStoreHouse.DisplayMember = "StoreHouseName";
                comboBoxStoreHouse.ValueMember = "Id";
                comboBoxStoreHouse.DataSource = listStoreHouses;
                comboBoxStoreHouse.SelectedItem = null;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxComponent.SelectedValue == null)
            {
                MessageBox.Show("Выберите компонент", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            if (comboBoxStoreHouse.SelectedValue == null)
            {
                MessageBox.Show("Выберите склад", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }

            storeHouseLogic.Replenishment(new StoreHouseReplenishmentBindingModel
            {
                ComponentId = Convert.ToInt32(comboBoxComponent.SelectedValue),
                StoreHouseId = Convert.ToInt32(comboBoxStoreHouse.SelectedValue),
                Count = Convert.ToInt32(textBoxCount.Text)
            });

            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}