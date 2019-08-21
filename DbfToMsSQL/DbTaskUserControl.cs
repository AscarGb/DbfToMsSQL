﻿using DbfToMsSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BdfToMsSQL
{
    public partial class DbTaskUserControl : UserControl
    {
        public int RowsCnt = 0;
        public int TotalRows = 0;
        public bool SqlSelectedTab = false;
        public string ConnestionString = "";
        public List<DBFReader> Readers;

        private bool _sqlConnect = false;
        private bool _dbfOpen = false;
        private string last_open_dir = "D:\\";
        private delegate string MethodInvokerString();
        private delegate string[] MethodInvokerStringArray();

        private delegate void MethodInvoker();
        private Dictionary<int, string> FieldIndex = new Dictionary<int, string>();
        private ListBox _listBoxValidateErrors;
        private MainForm _form;

        public bool SqlConnect
        {
            get => _sqlConnect;
            set
            {
                _sqlConnect = value;
                CreateSQLTableButton.Enabled = _dbfOpen && _sqlConnect;
            }
        }

        public bool DbfOpen
        {
            get => _dbfOpen;
            set
            {
                _dbfOpen = value;
                CreateSQLTableButton.Enabled = _dbfOpen && _sqlConnect;
            }
        }

        public DbTaskUserControl(MainForm form)
        {
            _form = form;
            InitializeComponent();
            _listBoxValidateErrors = form.InfoListBox;
        }

        private void ConnectToSqlButton_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                Invoke(() =>
                {
                    ConnectToSqlButton.Enabled = false;
                });

                try
                {
                    ConnestionString = $"Server={SQLServerAdressTextBox.Text};Database={DbNameTextBox.Text};User Id={UserNameTextBox.Text};Password={PasswordTextBox.Text};";

                    using (SqlConnection connection = new SqlConnection(ConnestionString))
                    {
                        using (SqlCommand sqlCommand = new SqlCommand("", connection)
                        {
                            CommandType = CommandType.Text,
                            CommandText = $"use [{DbNameTextBox.Text}] SELECT sobjects.name FROM sysobjects sobjects WHERE sobjects.xtype = 'U' order by name"
                        })
                        {
                            connection.Open();
                            using (SqlDataReader reader = sqlCommand.ExecuteReader())
                            {
                                Invoke(() =>
                                {
                                    TablesListbox.Items.Clear();
                                });

                                while (reader.Read())
                                {
                                    string tmpReader = reader[0].ToString();
                                    Invoke(() =>
                                    {
                                        TablesListbox.Items.Add(tmpReader);
                                    });
                                }

                                SqlConnect = true;
                                WriteMessage("Now select SQL table");
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    SqlConnect = false;
                    WriteError(exc);
                }
                finally
                {
                    Invoke(() =>
                    {
                        ConnectToSqlButton.Enabled = true;
                    });
                }
            });
        }

        private void Invoke(Action p)
        {
            Invoke(new MethodInvoker(p));
        }

        private void TablesListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                Invoke(() =>
                {
                    TablesListbox.Enabled = false;
                });

                try
                {
                    if (string.IsNullOrEmpty(ConnestionString.Trim()))
                    {
                        return;
                    }

                    using (SqlConnection connection = new SqlConnection(ConnestionString))
                    {
                        using (SqlCommand sqlCommand = new SqlCommand("", connection)
                        {
                            CommandType = CommandType.Text,
                            CommandText = $@"use [{DbNameTextBox.Text}]
                                       select c.name
                                       FROM SYS .OBJECTS AS T
                                       JOIN SYS .COLUMNS AS C
                                       ON T. OBJECT_ID=C .OBJECT_ID
                                       WHERE t.type = 'U' and T.name = '{TablesListbox.SelectedItem.ToString()}'"
                        })
                        {
                            connection.Open();
                            using (SqlDataReader reader = sqlCommand.ExecuteReader())
                            {
                                Invoke(() =>
                                {
                                    TableFieldsListBox.Items.Clear();
                                });
                                FieldIndex.Clear();
                                int i = 0;
                                while (reader.Read())
                                {
                                    Invoke(() =>
                                    {
                                        TableFieldsListBox.Items.Add(reader[0].ToString());
                                    });

                                    FieldIndex.Add(i, reader[0].ToString());
                                    i++;
                                }

                                SqlSelectedTab = true;
                                if (DbfOpen)
                                {
                                    CheckFields();
                                }
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    WriteError(exc);
                    SqlSelectedTab = false;
                }
                finally
                {
                    Invoke(() =>
                    {
                        TablesListbox.Enabled = true;
                    });
                }
            });
        }

        private void BtnOpenDbf_Click(object sender, EventArgs e)
        {
            if (Readers != null && Readers.Count() > 0)
            {
                Readers.ForEach(r =>
                {
                    try
                    {
                        r?.Dispose();
                    }
                    catch { }
                });
            }

            Readers = new List<DBFReader>();

            Invoke(() =>
            {
                openFileDialog1.InitialDirectory = last_open_dir;
                openFileDialog1.Filter = "dbf (*.dbf)|*.dbf";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;
                OpenDbfButton.Enabled = false;
            });

            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    bool IsErrors = false;
                    Invoke(() =>
                    {
                        DbfFilesListBox.Items.Clear();
                    });

                    string[] fileList = (string[])Invoke(new MethodInvokerStringArray(() => { return openFileDialog1.FileNames; }));

                    fileList.ToList().ForEach(f =>
                    {
                        DBFReader bulkReader = new DBFReader(f, FieldIndex, _form, int.Parse(EncodingComboBox.Text));
                        Readers.Add(bulkReader);

                        last_open_dir = Path.GetDirectoryName(f);
                        Invoke(() =>
                        {
                            DbfFilesListBox.Items.Add($"{Path.GetFileName(f)} ( Fields count:{bulkReader.FieldName.Count()} Rows count: {bulkReader.RowsCount} )");
                        });

                        try
                        {
                            if (SqlConnect && SqlSelectedTab)
                            {
                                List<string> dbfFields = bulkReader.FieldName.ToList();
                                List<string> sqlFields = new List<string>();

                                foreach (object item in TableFieldsListBox.Items)
                                {
                                    sqlFields.Add(item.ToString());
                                    if (!dbfFields.Contains(item.ToString()))
                                    {
                                        WriteMessage($"dbf table {f} missing field: {item.ToString()}");
                                        IsErrors = true;
                                    }
                                }
                                dbfFields.ForEach(item =>
                                {
                                    if (!sqlFields.Contains(item))
                                    {
                                        WriteMessage("SQL Table missing field: " + item);
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
                            WriteError(exc);
                        }

                    });

                    RecalculateRowCount();

                    if (Readers.Count() > 0)
                    {
                        Invoke(() =>
                        {
                            BbfFieldsLabel.Items.Clear();
                        });

                        RowsCnt = Readers.Sum(a => a.RowsCount);

                        Invoke(() =>
                        {
                            DbfCountLabel.Text = RowsCnt.ToString();
                        });

                        for (int i = 0; i < Readers[0].FieldCount; i++)
                        {
                            string fieldName = Readers[0].FieldName[i];
                            Invoke(() =>
                            {
                                BbfFieldsLabel.Items.Add(fieldName);
                            });
                        }
                    }

                    if (SqlConnect && SqlSelectedTab)
                    {
                        if (!IsErrors)
                        {
                            Invoke(() =>
                            {
                                ValidationLabel.Text = "Validation passed.";
                                ValidationLabel.ForeColor = Color.Green;
                            });
                        }
                        else
                        {
                            Invoke(() =>
                            {
                                ValidationLabel.Text = "Field do not coincide!";
                                ValidationLabel.ForeColor = Color.Red;
                            });
                        }
                    }
                }
                DbfOpen = true;
            }
            catch (Exception exc)
            {
                WriteError(exc);
                DbfOpen = false;
            }
            finally
            {
                Invoke(() =>
                {
                    OpenDbfButton.Enabled = true;
                });
            }
        }

        public void StartImport()
        {
            if (Readers != null)
            {
                Readers.ForEach(r =>
                {
                    _form.Invoke(() => { _form.CurrentFileLabel.Text = r.FileName; });
                    try
                    {
                        using (r)
                        {
                            using (SqlBulkCopy loader = new SqlBulkCopy(ConnestionString, SqlBulkCopyOptions.Default))
                            {
                                loader.DestinationTableName = Invoke(new MethodInvokerString(GetSelectedTable)).ToString();
                                loader.BulkCopyTimeout = int.MaxValue;
                                loader.WriteToServer(r);
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        WriteError(exc);
                    }
                });
            }
            else
            {
                WriteMessage("No connection");
            }
        }

        public string GetSelectedTable()
        {
            return TablesListbox.SelectedItem.ToString();
        }

        private void RemoveTaskButton_Click(object sender, EventArgs e)
        {
            _form.Tasks.Remove(this);
            RecalculateRowCount();
            _form.MainPanel.Controls.Remove(this);

            Readers?.ForEach(a => a?.Dispose());
        }

        private void RecalculateRowCount()
        {
            _form.RowsCnt = 0;
            _form.Tasks.ForEach(t =>
            {
                if (t.Readers != null)
                {
                    _form.RowsCnt += t.Readers.Sum(a => a.RowsCount);
                }
            });

            try
            {
                Invoke(() =>
                {
                    _form.TotalRowsLabel.Text = _form.RowsCnt.ToString();
                });
            }
            catch (Exception exc)
            {
                WriteError(exc);
            }
        }

        private void WriteError(Exception exc)
        {
            _form.Invoke(new MethodInvoker(() => { _listBoxValidateErrors.Items.Add($"Error: {exc.Message }\r\n {exc.InnerException?.ToString() ?? ""}"); }));
        }



        private void WriteMessage(string message)
        {
            _form.Invoke(new MethodInvoker(() => { _listBoxValidateErrors.Items.Add(message); }));
        }

        private bool CheckFields()
        {
            if (Readers == null)
            {
                return false;
            }

            bool isErrors = false;
            Readers.ForEach(r =>
            {
                List<string> DbfFields = r.FieldName.ToList();
                List<string> SqlFields = new List<string>();

                foreach (object item in TableFieldsListBox.Items)
                {
                    SqlFields.Add(item.ToString());
                    if (!DbfFields.Contains(item.ToString()))
                    {
                        WriteMessage($"dbf table {r.FileName} missing field: {item.ToString()}");
                        isErrors = true;
                    }
                }

                DbfFields.ForEach(item =>
                {
                    if (!SqlFields.Contains(item))
                    {
                        WriteMessage("SQL Table missing field: " + item);
                        isErrors = true;
                    }
                });
            });

            if (!isErrors)
            {
                ValidationLabel.Text = "Validation passed.";
                ValidationLabel.ForeColor = Color.Green;
            }
            else
            {
                ValidationLabel.Text = "Field do not coincide!";
                ValidationLabel.ForeColor = Color.Red;
            }

            return isErrors;
        }

        private void CreateSQLTableButton_Click(object sender, EventArgs e)
        {
            CreateTable t = new CreateTable(this)
            {
                StartPosition = FormStartPosition.CenterParent
            };
            t.ShowDialog();
        }
    }
}