using SecuritySystemBusinessLogic.BusinessLogics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SecuritySystemView
{
    public partial class FormMails : Form
    {
        private readonly MailLogic _mailLogic;

        public FormMails(MailLogic mailLogic)
        {
            InitializeComponent();
            _mailLogic = mailLogic;
        }

        private void FormMails_Load(object sender, EventArgs e)
        {
            var list = _mailLogic.Read(null);
            if (list != null)
            {
                dataGridViewMails.DataSource = list;
                dataGridViewMails.Columns[0].Visible = false;
                dataGridViewMails.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridViewMails.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridViewMails.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }
    }
}
