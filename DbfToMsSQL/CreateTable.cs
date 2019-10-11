using System;
using System.Text;
using System.Windows.Forms;

namespace BdfToMsSQL
{
    public partial class CreateTable : Form
    {       
        private DbTaskUserControl control;
        private StringBuilder s = new StringBuilder();
        public CreateTable(DbTaskUserControl control)
        {
            this.control = control;
            InitializeComponent();

            string dbName = control.DbNameTextBox.Text;

            control.Readers.ForEach(r =>
            {
                s.Append(SQLHelper.CreateTableSQL(r, dbName));
            });

            SQLTableTextBox.Text = s.ToString();
        }

        private async void RunButton_Click(object sender, EventArgs e)
        {
            try
            {
                await SQLHelper.CreateTable(SQLTableTextBox.Text, control.ConnestionString);

                StatusLabel.Text = "Complete";
            }
            catch (Exception exc)
            {
                StatusLabel.Text = exc.Message;
            }
        }
    }
}
