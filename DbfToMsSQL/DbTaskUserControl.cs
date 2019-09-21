using BdfToMsSQL.Loader;
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
        private bool _isIntegratedSecutiry
        {
            get => IntegratedSecurityCheckBox.Checked;
            set => UserNameTextBox.Enabled =
                        PasswordTextBox.Enabled = !value;
        }
        public string ConnestionString
        {
            get
            {
                if (_isIntegratedSecutiry)
                {
                    return $"Integrated Security=true;Server={SQLServerAdressTextBox.Text};Database={DbNameTextBox.Text};";
                }
                else
                {
                    return
                        $"Server={SQLServerAdressTextBox.Text};Database={DbNameTextBox.Text};User Id={UserNameTextBox.Text};Password={PasswordTextBox.Text};";
                }
            }
        }
        public List<DBFReader> Readers { get; set; }
        public string SelectedDbfTable { get; set; } = "";

        private bool _sqlConnect = false;
        private bool _dbfOpen = false;
        private string last_open_dir = "D:\\";

        private Dictionary<int, string> FieldIndex = new Dictionary<int, string>();
        private ListBox _listBoxValidateErrors;
        private MainForm _form;

        public bool SqlConnected
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

        private async void ConnectToSqlButton_Click(object sender, EventArgs e)
        {
            ConnectToSqlButton.Enabled = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnestionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("", connection)
                    {
                        CommandType = CommandType.Text,
                        CommandText = $"use [{DbNameTextBox.Text}] SELECT sobjects.name FROM sysobjects sobjects WHERE sobjects.xtype = 'U' order by name"
                    })
                    {
                        await connection.OpenAsync();

                        using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            TablesListbox.Items.Clear();

                            while (await reader.ReadAsync())
                            {
                                string tmpReader = reader[0].ToString();

                                TablesListbox.Items.Add(tmpReader);
                            }

                            SqlConnected = true;
                            Logger.WriteMessage("Now select SQL table");
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                SqlConnected = false;
                Logger.WriteError(exc);
            }
            finally
            {
                ConnectToSqlButton.Enabled = true;
            }
        }

        private void Invoke(Action p)
        {
            Invoke(new MethodInvoker(p));
        }

        private async void TablesListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            TablesListbox.Enabled = false;

            try
            {
                if (string.IsNullOrEmpty(ConnestionString.Trim()))
                {
                    return;
                }

                string item = TablesListbox.SelectedItem.ToString();

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
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                        {

                            TableFieldsListBox.Items.Clear();

                            FieldIndex.Clear();
                            int i = 0;
                            while (await reader.ReadAsync())
                            {
                                TableFieldsListBox.Items.Add(reader[0].ToString());

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

            openFileDialog1.InitialDirectory = last_open_dir;
            openFileDialog1.Filter = "dbf (*.dbf)|*.dbf";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            OpenDbfButton.Enabled = false;

            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    bool IsErrors = false;

                    DbfFilesListBox.Items.Clear();

                    string[] fileList = openFileDialog1.FileNames;

                    fileList.ToList().ForEach(f =>
                    {
                        string fileName = Path.GetFileName(f);

                        DBFReader bulkReader = new DBFReader(f, FieldIndex, int.Parse(EncodingComboBox.Text), fileName);
                        Readers.Add(bulkReader);

                        bulkReader.OnLoad += BulkReader_OnLoad;

                        last_open_dir = Path.GetDirectoryName(f);

                        DbfFilesListBox.Items.Add($"{fileName} ( Fields count:{bulkReader.Fields.Count()} Rows count: {bulkReader.RowsCount} )");

                        try
                        {
                            if (SqlConnected && SqlSelectedTab)
                            {
                                List<string> dbfFields = bulkReader.Fields.Select(a => a.Name).ToList();
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

                    if (SqlConnected && SqlSelectedTab)
                    {
                        if (!IsErrors)
                        {

                            ValidationLabel.Text = "Validation passed.";
                            ValidationLabel.ForeColor = Color.Green;
                        }
                        else
                        {
                            ValidationLabel.Text = "Field do not coincide!";
                            ValidationLabel.ForeColor = Color.Red;
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
                OpenDbfButton.Enabled = true;
            }
        }

        private void BulkReader_OnLoad(int total)
        {
           _form.TotalRows += total;

            Logger.Clear();
            Logger.WriteMessage($"Loading...  {(_form.TotalRows / (float)_form.RowsCnt * 100).ToString("f2") }%");
        }

        private void ShowDbfTableFields()
        {
            if (Readers.Count() > 0)
            {
                BbfFieldsLabel.Items.Clear();
                RowsCnt = Readers.Sum(a => a.RowsCount);
                DbfCountLabel.Text = RowsCnt.ToString();

                DBFReader r = Readers
                    .FirstOrDefault(a => StringComparer.Ordinal.Equals(a.TableName, SelectedDbfTable.Split('(').First().Trim()))
                    ?? Readers[0];

                foreach (DbfToMsSQL.Loader.Field f in r.Fields)
                {
                    BbfFieldsLabel.Items.Add(f.Name);
                }
            }
        }

        public async Task StartImport()
        {
            if (Readers != null)
            {
                await Task.WhenAll(
                    Readers.Select(async r =>
                    {
                        _form.CurrentFileLabel.Text = r.FileName;

                        try
                        {
                            using (r)
                            {
                                using (SqlBulkCopy loader = new SqlBulkCopy(ConnestionString, SqlBulkCopyOptions.Default))
                                {
                                    loader.DestinationTableName = TablesListbox.SelectedItem.ToString();
                                    loader.BulkCopyTimeout = int.MaxValue;
                                    await loader.WriteToServerAsync(r);
                                }
                            }
                        }
                        catch (Exception exc)
                        {
                            Logger.WriteError(exc);
                        }
                    }));
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
                _form.TotalRowsLabel.Text = _form.RowsCnt.ToString();

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
                List<string> DbfFields = r.Fields.Select(a => a.Name).ToList();
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

        private void IntegratedSecurityCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _isIntegratedSecutiry = IntegratedSecurityCheckBox.Checked;
        }
    }
}