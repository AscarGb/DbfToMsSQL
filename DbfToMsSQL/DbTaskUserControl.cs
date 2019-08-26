using DbfToMsSQL;
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
        public int RowsCnt { get; set; } = 0;
        public int TotalRows { get; set; } = 0;
        public bool SqlSelectedTab { get; set; } = false;
        public string ConnestionString { get; set; } = "";
        public List<DBFReader> Readers { get; set; }
        public string SelectedDbfTable { get; set; } = "";

        private bool _sqlConnect = false;
        private bool _dbfOpen = false;
        private string last_open_dir = "D:\\";

        private delegate string MethodInvokerString();
        private delegate string[] MethodInvokerStringArray();

        private Dictionary<int, string> FieldIndex = new Dictionary<int, string>();
        private ListBox _listBoxValidateErrors;
        private MainForm _form;

        public bool SqlConnect
        {
            get => _sqlConnect;
            set
            {
                _sqlConnect = value;
                Invoke(() =>
                {
                    CreateSQLTableButton.Enabled = _dbfOpen && _sqlConnect;
                });
            }
        }

        public bool DbfOpen
        {
            get => _dbfOpen;
            set
            {
                _dbfOpen = value;
                Invoke(() =>
                {
                    CreateSQLTableButton.Enabled = _dbfOpen && _sqlConnect;
                });
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
                                Logger.WriteMessage("Now select SQL table");
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    SqlConnect = false;
                    Logger.WriteError(exc);
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

                    string item = (string)Invoke(new MethodInvokerString(() => { return TablesListbox.SelectedItem.ToString(); }));

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
                                       WHERE t.type = 'U' and T.name = '{item}'"
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
                    Logger.WriteError(exc);
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
                        string fileName = Path.GetFileName(f);

                        DBFReader bulkReader = new DBFReader(f, FieldIndex, _form, int.Parse(EncodingComboBox.Text), fileName);
                        Readers.Add(bulkReader);

                        last_open_dir = Path.GetDirectoryName(f);
                        Invoke(() =>
                        {
                            DbfFilesListBox.Items.Add($"{fileName} ( Fields count:{bulkReader.FieldName.Count()} Rows count: {bulkReader.RowsCount} )");
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
                                        Logger.WriteMessage($"dbf table {f} missing field: {item.ToString()}");
                                        IsErrors = true;
                                    }
                                }
                                dbfFields.ForEach(item =>
                                {
                                    if (!sqlFields.Contains(item))
                                    {
                                        Logger.WriteMessage("SQL Table missing field: " + item);
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
                            Logger.WriteError(exc);
                        }

                    });

                    RecalculateRowCount();

                    ShowDbfTableFields();

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
                Logger.WriteError(exc);
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

        private void ShowDbfTableFields()
        {
            if (Readers.Count() > 0)
            {
                Invoke(() =>
                {
                    BbfFieldsLabel.Items.Clear();
                    RowsCnt = Readers.Sum(a => a.RowsCount);
                    DbfCountLabel.Text = RowsCnt.ToString();

                    DBFReader r = Readers
                        .FirstOrDefault(a => StringComparer.Ordinal.Equals(a.TableName, SelectedDbfTable.Split('(').First().Trim()))
                        ?? Readers[0];

                    for (int i = 0; i < r.FieldCount; i++)
                    {
                        string fieldName = r.FieldName[i];

                        BbfFieldsLabel.Items.Add(fieldName);
                    }
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
                        Logger.WriteError(exc);
                    }
                });
            }
            else
            {
                Logger.WriteMessage("No connection");
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
                Logger.WriteError(exc);
            }
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
                        Logger.WriteMessage($"dbf table {r.FileName} missing field: {item.ToString()}");
                        isErrors = true;
                    }
                }

                DbfFields.ForEach(item =>
                {
                    if (!SqlFields.Contains(item))
                    {
                        Logger.WriteMessage("SQL Table missing field: " + item);
                        isErrors = true;
                    }
                });
            });

            if (!isErrors)
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

        private void DbfFilesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedDbfTable = DbfFilesListBox.Text;
            ShowDbfTableFields();
        }
    }
}