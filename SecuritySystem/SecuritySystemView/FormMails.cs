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

        private readonly int stringsCountOnPage = 7;

        public FormMails(MailLogic mailLogic)
        {
            InitializeComponent();
            _mailLogic = mailLogic;
        }

        private void FormMails_Load(object sender, EventArgs e)
        {
            LoadData();
            textBoxPage.Text = pageNumber.ToString();
        }

        private void LoadData()
        {
            try
            {
                Program.ConfigGrid(_mailLogic.Read(new MessageInfoBindingModel
                {
                    PageNumber = pageNumber,
                    StringsCountOnPage = stringsCountOnPage
                }), dataGridViewMails);
                textBoxPage.Text = pageNumber.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            if (pageNumber > 1)
            {
                pageNumber--;
            }

            LoadData();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            int stringsCountOnPage = _mailLogic.Read(new MessageInfoBindingModel
            {
                PageNumber = pageNumber + 1
            }).Count;

            if (stringsCountOnPage != 0)
            {
                pageNumber++;
                LoadData();
            }
        }

        private void textBoxPage_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBoxPage.Text != "")
                {
                    int pageNumberValue = Convert.ToInt32(textBoxPage.Text);

                    if (pageNumberValue < 1)
                    {
                        throw new Exception();
                    }

                    int stringsCountOnPage = _mailLogic.Read(new MessageInfoBindingModel
                    {
                        PageNumber = pageNumberValue
                    }).Count;

                    if (stringsCountOnPage == 0)
                    {
                        throw new Exception();
                    }

                    pageNumber = pageNumberValue;
                    LoadData();
                }
            }
            catch (Exception)
            {
                textBoxPage.Text = pageNumber.ToString();
            }
        }
    }
}
