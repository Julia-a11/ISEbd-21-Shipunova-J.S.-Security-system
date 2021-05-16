using SecuritySystemBusinessLogic.BindingModels;
using SecuritySystemBusinessLogic.BusinessLogics;
using System;
using System.Windows.Forms;

namespace SecuritySystemView
{
    public partial class FormMails : Form
    {
        private readonly MailLogic _mailLogic;

        private int pageNumber = 1;

        public FormMails(MailLogic mailLogic)
        {
            InitializeComponent();
            _mailLogic = mailLogic;
        }

        private void FormMails_Load(object sender, EventArgs e)
        {
            try
            {
                Program.ConfigGrid(_mailLogic.Read(null), dataGridViewMails);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
