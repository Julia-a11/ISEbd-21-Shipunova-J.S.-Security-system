using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.BusinessLogics;
using SecuritySystemBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;


namespace SecuritySystemView
{
    public partial class FormSecure : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
       
        public int Id { set { id = value; } }
        
        private readonly SecureLogic logic;
       
        private int? id;
        
        private Dictionary<int, (string, int)> secureComponents;
       
        public FormSecure(SecureLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void FormSecure_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    SecureViewModel view = logic.Read(new SecureBindingModel
                    {
                        Id = id.Value
                    })?[0];
                    if (view != null)
                    {
                        textBoxName.Text = view.SecureName;
                        textBoxPrice.Text = view.Price.ToString();
                        secureComponents = view.SecureComponents;
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
                secureComponents = new Dictionary<int, (string, int)>();
            }
        }

        private void LoadData()
        {
            try
            {
                if (secureComponents != null)
                {
                    dataGridViewComponents.Rows.Clear();
                    foreach (var pc in secureComponents)
                    {
                        dataGridViewComponents.Rows.Add(new object[] { pc.Key, pc.Value.Item1, pc.Value.Item2 });
                    }
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
            var form = Container.Resolve<FormSecureComponent>();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (secureComponents.ContainsKey(form.Id))
                {
                    secureComponents[form.Id] = (form.ComponentName, form.Count);
                }
                else
                {
                    secureComponents.Add(form.Id, (form.ComponentName, form.Count));
                }
                LoadData();
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridViewComponents.SelectedRows.Count == 1)
            {
                var form = Container.Resolve<FormSecureComponent>();
                int id = Convert.ToInt32(dataGridViewComponents.SelectedRows[0].Cells[0].Value);
                form.Id = id;
                form.Count = secureComponents[id].Item2;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    secureComponents[form.Id] = (form.ComponentName, form.Count);
                    LoadData();
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewComponents.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo,
               MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        secureComponents.Remove(Convert.ToInt32(dataGridViewComponents.SelectedRows[0].Cells[0].Value));
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            if (secureComponents == null || secureComponents.Count == 0)
            {
                MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            try
            {
                logic.CreateOrUpdate(new SecureBindingModel
                {
                    Id = id,
                    SecureName = textBoxName.Text,
                    Price = Convert.ToDecimal(textBoxPrice.Text),
                    SecureComponents = secureComponents
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