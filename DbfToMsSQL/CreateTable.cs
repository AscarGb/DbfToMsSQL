using BdfToMsSQL.Loader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
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

            DBFReader r = control.Readers
                .FirstOrDefault(a => StringComparer.Ordinal.Equals(a.TableName, control.SelectedDbfTable.Split('(').First().Trim()))
                ?? control.Readers[0];

            s.Append($"use {control.DbNameTextBox.Text + Environment.NewLine}CREATE TABLE [dbo].[{Path.GetFileNameWithoutExtension(r.FileName)}] ({Environment.NewLine}");

            listItem = Path.GetFileNameWithoutExtension(r.FileName);

            IEnumerable<string> fields = r.Fields.Select(field =>
            {

                StringBuilder fieldsBuilder = new StringBuilder();

                fieldsBuilder.Append($"\t   [{field.Name}] ");
                string sqlType = "";
                switch (field.Type)
                {
                    case 'L': sqlType = "bit"; break;
                    case 'D': sqlType = "date"; break;
                    case 'N':
                        {
                            if (field.Digits == 0)
                            {
                                sqlType = "int";
                            }
                            else
                            {
                                sqlType = $"numeric({(int)field.Size},{(int)field.Digits})";
                            }

                            break;
                        }
                    case 'F': sqlType = "double"; break;
                    default: sqlType = $"nvarchar({(int)field.Size})"; break;
                }
                fieldsBuilder.Append($"\t    { sqlType } null");

                return fieldsBuilder.ToString();
            });

            s.Append(string.Join("," + Environment.NewLine, fields));

            s.Append(")");
            SQLTableTextBox.Text = s.ToString();
        }

        private async void RunButton_Click(object sender, EventArgs e)
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
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
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
                            await connection.OpenAsync();
                            using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                            {
                                control.TablesListbox.Items.Clear();
                                while (await reader.ReadAsync())
                                {
                                    control.TablesListbox.Items.Add(reader[0].ToString());
                                }
                                control.SqlConnected = true;
                                control.TablesListbox.SelectedItem = listItem;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    control.SqlConnected = false;
                }

            }
            catch (Exception exc)
            {
                StatusLabel.Text = exc.Message;
            }
        }
    }
}
