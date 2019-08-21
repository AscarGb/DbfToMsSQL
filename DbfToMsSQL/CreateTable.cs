using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BdfToMsSQL
{
    public partial class CreateTable : Form
    {
        private string listItem = "";
        private DbTaskUserControl control;
        private StringBuilder s = new StringBuilder();
        public CreateTable(DbTaskUserControl control)
        {
            this.control = control;
            InitializeComponent();
            s.Append($"use {control.DbNameTextBox.Text + Environment.NewLine}CREATE TABLE [dbo].[{Path.GetFileNameWithoutExtension(control.Readers[0].FileName)}] ({Environment.NewLine}");

            listItem = Path.GetFileNameWithoutExtension(control.Readers[0].FileName);

            DBFReader r = control.Readers[0];

            for (int i = 0; i < r.FieldCount; i++)
            {
                s.Append($"\t   [{r.FieldName[i]}] ");
                string sqlType = "";
                switch (r.FieldType[i])
                {
                    case "L": sqlType = "bit"; break;
                    case "D": sqlType = "date"; break;
                    case "N":
                        {
                            if (r.FieldDigs[i] == 0)
                            {
                                sqlType = "int";
                            }
                            else
                            {
                                sqlType = $"numeric({(int)r.FieldSize[i]},{(int)r.FieldDigs[i]})";
                            }

                            break;
                        }
                    case "F": sqlType = "double"; break;
                    default: sqlType = $"nvarchar({(int)r.FieldSize[i]})"; break;
                }
                s.Append($"\t    { sqlType } null{ (i < r.FieldCount - 1 ? "," : "")}{ Environment.NewLine}");
            }

            s.Append(")");
            SQLTableTextBox.Text = s.ToString();
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(control.ConnestionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("", connection)
                    {
                        CommandType = CommandType.Text,
                        CommandText = SQLTableTextBox.Text
                    })
                    {
                        connection.Open();
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            StatusLabel.Text = "Complete";
                        }
                    }
                }

                try
                {
                    using (SqlConnection connection = new SqlConnection(control.ConnestionString))
                    {
                        using (SqlCommand sqlCommand = new SqlCommand("", connection)
                        {
                            CommandType = CommandType.Text,
                            CommandText = $"use [{control.DbNameTextBox.Text}] SELECT sobjects.name FROM sysobjects sobjects WHERE sobjects.xtype = 'U' order by name"
                        })
                        {
                            connection.Open();
                            using (SqlDataReader reader = sqlCommand.ExecuteReader())
                            {
                                control.TablesListbox.Items.Clear();
                                while (reader.Read())
                                {
                                    control.TablesListbox.Items.Add(reader[0].ToString());
                                }
                                control.SqlConnect = true;
                                control.TablesListbox.SelectedItem = listItem;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    control.SqlConnect = false;
                }

            }
            catch (Exception exc)
            {
                StatusLabel.Text = exc.Message;
            }
        }
    }
}
