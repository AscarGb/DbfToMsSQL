using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BdfToMsSQL
{
    public partial class CreateTable : Form
    {
        string listItem = "";
        UserControl1 control;
        string s = "";
        public CreateTable(UserControl1 control)
        {
            this.control = control;
            InitializeComponent();
            s = "use " + control.tb_db_name.Text + Environment.NewLine + "CREATE TABLE [dbo].[" + Path.GetFileNameWithoutExtension(control.reader[0].FileName) + "] ("+Environment.NewLine;

            listItem = Path.GetFileNameWithoutExtension(control.reader[0].FileName);

            var r = control.reader[0];

            for (int i = 0; i < r.FieldCount; i++) {
                s += "\t   [" + r.FieldName[i] + "] ";
                string sqlType = "";
                switch (r.FieldType[i])
                {
                    case "L": sqlType = "bit"; break;
                    case "D": sqlType="date"; break;
                    case "N":
                        {
                            if (r.FieldDigs[i] == 0)
                                sqlType="int";
                            else
                                sqlType = "numeric("+((int)r.FieldSize[i])+","+((int)r.FieldDigs[i])+")";
                            break;
                        }
                    case "F": sqlType = "double"; break;
                    default: sqlType = "nvarchar(" + ((int)r.FieldSize[i]) + ")"; break;
                }
                s += "\t    " + sqlType + " null" + (i < r.FieldCount-1?",":"") + Environment.NewLine;
            }
            
            s += ")";
            richTextBox1.Text = s;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                {
                    SqlConnection connection = new SqlConnection(control.connestionString);
                    SqlCommand sqlCommand = new SqlCommand("", connection);
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = richTextBox1.Text;
                    connection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    connection.Close();
                    reader.Close();
                    label1.Text = "Complete";
                }

                try
                {
                    SqlConnection connection = new SqlConnection(control.connestionString);
                    SqlCommand sqlCommand = new SqlCommand("", connection);
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.CommandText = @"use [" + control.tb_db_name.Text + "] SELECT sobjects.name FROM sysobjects sobjects WHERE sobjects.xtype = 'U' order by name";
                    connection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    control.listbox_Tables.Items.Clear();
                    while (reader.Read())
                    {
                        control.listbox_Tables.Items.Add(reader[0].ToString());
                    }
                    reader.Close();
                    connection.Close();
                    control.SQL_CONNECT = true;
                    control.listbox_Tables.SelectedItem = listItem;
                }
                catch (Exception exc)
                {
                    control.SQL_CONNECT = false;                    
                }

            }
            catch (Exception exc) { label1.Text = exc.Message; }
        }
    }
}
