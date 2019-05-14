using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using DbfToMsSQL;

namespace BdfToMsSQL
{
    public partial class UserControl1 : UserControl
    {
        public bool _SQL_CONNECT = false;
        public bool SQL_CONNECT
        {
            get { return _SQL_CONNECT; }
            set
            {
                _SQL_CONNECT = value;
                btnCreateTable.Enabled = _DBF_OPEN && _SQL_CONNECT;
            }
        }

        public bool SQL_SELECTED_TAB = false;

        bool _DBF_OPEN = false;
        public bool DBF_OPEN
        {
            get { return _DBF_OPEN; }
            set
            {
                _DBF_OPEN = value;
                btnCreateTable.Enabled = _DBF_OPEN && _SQL_CONNECT;
            }
        }

        Form1 form;
        ListBox listBox_validate_errors;
        private delegate string GetSelectedTableDel();
        private delegate void FormMethod();
        public int RowsCnt = 0;
        public int TotalRows = 0;
        public List<DBFBulkReader> reader;
        Dictionary<int, string> FieldIndex = new Dictionary<int, string>();
        public string connestionString = "";
        string last_open_dir = "D:\\";

        public UserControl1(Form1 form)
        {
            this.form = form;
            InitializeComponent();
            this.listBox_validate_errors = form.listBox_validate_errors;
        }

        private void btn_connect_to_sql_Click(object sender, EventArgs e)
        {
            try
            {
                connestionString = "Server=" + tb_SQLServerAdress.Text
                    + ";Database=" + tb_db_name.Text
                    + ";User Id=" + tb_user_name.Text
                    + ";Password=" + tb_psw.Text + ";";
                SqlConnection connection = new SqlConnection(connestionString);
                SqlCommand sqlCommand = new SqlCommand("", connection);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = @"use [" + tb_db_name.Text + "] SELECT sobjects.name FROM sysobjects sobjects WHERE sobjects.xtype = 'U' order by name";
                connection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                listbox_Tables.Items.Clear();
                while (reader.Read())
                {
                    listbox_Tables.Items.Add(reader[0].ToString());
                }
                reader.Close();
                connection.Close();
                SQL_CONNECT = true;
                ConsoleLog("Now select SQL table");
            }
            catch (Exception exc)
            {
                SQL_CONNECT = false;
                ConsoleLog(exc);
            }
        }

        private void listbox_Tables_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (connestionString.Trim() == "") return;

                SqlConnection connection = new SqlConnection(connestionString);
                SqlCommand sqlCommand = new SqlCommand("", connection);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = @"use [" + tb_db_name.Text + @"]
                                       select c.name
                                       FROM SYS .OBJECTS AS T
                                       JOIN SYS .COLUMNS AS C
                                       ON T. OBJECT_ID=C .OBJECT_ID
                                       WHERE t.type = 'U' and T.name = '" + listbox_Tables.SelectedItem.ToString() + @"'";
                connection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                listBox_swl_table_fields.Items.Clear();
                FieldIndex.Clear();
                int i = 0;
                while (reader.Read())
                {
                    listBox_swl_table_fields.Items.Add(reader[0].ToString());
                    FieldIndex.Add(i, reader[0].ToString());
                    i++;
                }
                reader.Close();
                connection.Close();
                SQL_SELECTED_TAB = true;
                if (DBF_OPEN)
                {
                    CheckFields();
                }
            }
            catch (Exception exc)
            {
                ConsoleLog(exc);
                SQL_SELECTED_TAB = false;
            }
        }

        private void btn_open_dbf_Click(object sender, EventArgs e)
        {
            if (reader != null && reader.Count() > 0)
                reader.ForEach(r =>
                {
                    try
                    {
                        r.Dispose();
                    }
                    catch { }
                });
            reader = new List<DBFBulkReader>();

            openFileDialog1.InitialDirectory = last_open_dir;
            openFileDialog1.Filter = "dbf (*.dbf)|*.dbf";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    bool IsErrors = false;
                    listBox_files.Items.Clear();
                    openFileDialog1.FileNames.ToList().ForEach(f =>
                    {
                        DBFBulkReader bulkReader = new DBFBulkReader(f, FieldIndex, form);
                        reader.Add(bulkReader);

                        last_open_dir = Path.GetDirectoryName(f);
                        listBox_files.Items.Add(Path.GetFileName(f) + " ( Fields count:" + bulkReader.FieldName.Count() + " Rows count: " + bulkReader.RowsCount + " )");

                        try
                        {
                            if (SQL_CONNECT && SQL_SELECTED_TAB)
                            {
                                List<string> DBF_Fields = bulkReader.FieldName.ToList();
                                List<string> SQL_Fields = new List<string>();

                                foreach (var item in listBox_swl_table_fields.Items)
                                {
                                    SQL_Fields.Add(item.ToString());
                                    if (!DBF_Fields.Contains(item.ToString()))
                                    {
                                        ConsoleLog("dbf table " + f + " missing field: " + item.ToString());
                                        IsErrors = true;
                                    }
                                }
                                DBF_Fields.ForEach(item =>
                                {
                                    if (!SQL_Fields.Contains(item))
                                    {
                                        ConsoleLog("SQL Table missing field: " + item);
                                        IsErrors = true;
                                    }
                                });
                            }
                            else
                            {

                            }

                        }
                        catch (Exception exc)
                        {
                            ConsoleLog(exc);
                        }
                    });

                    RecalculateRowCount();

                    if (reader.Count() > 0)
                    {
                        listBox_dbf_fields.Items.Clear();
                        RowsCnt = reader.Sum(a => a.RowsCount);
                        lbl_dbf_count.Text = RowsCnt.ToString();
                        for (int i = 0; i < reader[0].FieldCount; i++)
                        {
                            listBox_dbf_fields.Items.Add(reader[0].FieldName[i]);
                        }
                    }

                    if (SQL_CONNECT && SQL_SELECTED_TAB)
                        if (!IsErrors)
                        {
                            //  ConsoleLog("Validation passed");
                            label_valid.Text = "Validation passed.";
                            label_valid.ForeColor = Color.Green;
                        }
                        else
                        {
                            label_valid.Text = "Field do not coincide!";
                            label_valid.ForeColor = Color.Red;
                        }

                }
                DBF_OPEN = true;
            }
            catch (Exception exc)
            {
                ConsoleLog(exc);
                DBF_OPEN = false;
            }
        }

        public void start_import()
        {
            if (reader != null)
            {
                this.reader.ForEach(r =>
                {
                    form.Invoke(new FormMethod(() => { form.lblCurrentFile.Text = r.FileName; }));
                    try
                    {
                        using (var loader = new SqlBulkCopy(connestionString, SqlBulkCopyOptions.Default))
                        {
                            loader.DestinationTableName = Invoke(new GetSelectedTableDel(GetSelectedTable)).ToString();
                            loader.BulkCopyTimeout = 9999;
                            loader.WriteToServer(r);
                            r.Dispose();
                        }
                    }
                    catch (Exception exc)
                    {
                        ConsoleLog(exc);
                    }
                });
            }
            else
            {
                ConsoleLog("No connection");
            }
        }

        public string GetSelectedTable()
        {
            return listbox_Tables.SelectedItem.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form.Tasks.Remove(this);
            RecalculateRowCount();
            form.panel1.Controls.Remove(this);
            if (reader != null)
            {
                this.reader.ForEach(a => a.Dispose());
            }
            this.Dispose();
        }

        void RecalculateRowCount()
        {
            form.RowsCnt = 0;
            form.Tasks.ForEach(t =>
            {
                if (t.reader != null)
                {
                    form.RowsCnt += t.reader.Sum(a => a.RowsCount);
                }
            });
            form.lblTotalRows.Text = form.RowsCnt.ToString();
        }
        void ConsoleLog(Exception exc)
        {
            form.Invoke(new FormMethod(() => { listBox_validate_errors.Items.Add("Error: " + exc.Message + "\r\n " + exc.InnerException); }));
        }
        void ConsoleLog(string message)
        {
            form.Invoke(new FormMethod(() => { listBox_validate_errors.Items.Add(message); }));
        }
        bool CheckFields()
        {
            if (reader == null) return false;

            bool is_errors = false;
            reader.ForEach(r =>
            {
                List<string> DBF_Fields = r.FieldName.ToList();
                List<string> SQL_Fields = new List<string>();
                foreach (var item in listBox_swl_table_fields.Items)
                {
                    SQL_Fields.Add(item.ToString());
                    if (!DBF_Fields.Contains(item.ToString()))
                    {
                        ConsoleLog("dbf table " + r.FileName + " missing field: " + item.ToString());
                        is_errors = true;
                    }
                }
                DBF_Fields.ForEach(item =>
                {
                    if (!SQL_Fields.Contains(item))
                    {
                        ConsoleLog("SQL Table missing field: " + item);
                        is_errors = true;
                    }
                });
            });

            if (!is_errors)
            {
                //    ConsoleLog("Validation passed");
                label_valid.Text = "Validation passed.";
                label_valid.ForeColor = Color.Green;
            }
            else
            {
                label_valid.Text = "Field do not coincide!";
                label_valid.ForeColor = Color.Red;
            }

            return is_errors;
        }

        private void btnCreateTable_Click(object sender, EventArgs e)
        {
            CreateTable t = new CreateTable(this);
            t.StartPosition = FormStartPosition.CenterParent;
            t.ShowDialog();
        }
    }
}
