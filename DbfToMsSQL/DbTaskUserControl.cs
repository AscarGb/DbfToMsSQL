using BdfToMsSQL.Loader;
using DbfToMsSQL;
using DbfToMsSQL.Loader;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
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
        public List<DBFInfo> Readers { get; } = new List<DBFInfo>();
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
                    using (SqlCommand sqlCommand = new SqlCommand(
                        $@"use [{DbNameTextBox.Text}] SELECT sobjects.name, c.name FROM sysobjects sobjects 
                           join SYS .OBJECTS AS T
                           ON t.type = 'U' and T.name = sobjects.name
                           JOIN SYS .COLUMNS AS C
                           ON T. OBJECT_ID=C .OBJECT_ID
                           WHERE sobjects.xtype = 'U' order by sobjects.name", connection))
                    {
                        await connection.OpenAsync();

                        using (SqlDataReader reader = await sqlCommand.ExecuteReaderAsync())
                        {
                            DbTablesTreeView.Nodes.Clear();

                            List<TableField> tbList = new List<TableField>();

                            while (await reader.ReadAsync())
                            {
                                tbList.Add(new TableField
                                {
                                    Table = reader[0].ToString(),
                                    Field = reader[1].ToString()
                                });
                            }

                            foreach (IGrouping<string, TableField> tbg in tbList.GroupBy(a => a.Table))
                            {
                                TreeNode tnode = DbTablesTreeView.Nodes.Add(tbg.Key);

                                foreach (TableField f in tbg)
                                {
                                    tnode.Nodes.Add(f.Field);
                                }
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
                Invoke(() =>
                {
                    ConnectToSqlButton.Enabled = true;
                });
            }
        }

        private void Invoke(Action p)
        {
            Invoke(new MethodInvoker(p));
        }



        private void BtnOpenDbf_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = last_open_dir;
            openFileDialog1.Filter = "dbf (*.dbf)|*.dbf";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            OpenDbfButton.Enabled = false;

            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string[] fileList = openFileDialog1.FileNames;

                    DbfFilesTreeView.Nodes.Clear();

                    fileList.ToList().ForEach(f =>
                    {
                        string fileName = Path.GetFileNameWithoutExtension(f);

                        DBFInfo dBFInfo = new DBFInfo(f);

                        Readers.Add(dBFInfo);

                        last_open_dir = Path.GetDirectoryName(f);

                        TreeNode node = DbfFilesTreeView.Nodes.Add($"{fileName} ( Fields count:{dBFInfo.Fields.Count()} Rows count: {dBFInfo.RowsCount} )");

                        foreach (Field field in dBFInfo.Fields)
                        {
                            node.Nodes.Add(field.Name);
                        }
                    });

                    RecalculateRowCount();
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

        public async Task StartImport()
        {
            if (Readers != null)
            {
                if (AutoCreateCheckBox.Checked)
                {
                    StringBuilder s = new StringBuilder();

                    string dbName = DbNameTextBox.Text;

                    Readers.ForEach(r =>
                    {
                        s.Append(SQLHelper.CreateTableSQL(r, dbName));
                    });

                    await SQLHelper.CreateTable(s.ToString(), ConnestionString);
                }

                using (SqlConnection connection = new SqlConnection(ConnestionString))
                {
                    await connection.OpenAsync();

                    foreach (DBFInfo r in Readers)
                    {
                        _form.CurrentFileLabel.Text = r.TableName;

                        try
                        {
                            using (DBFReader reader = new DBFReader(r.FileName, int.Parse(EncodingComboBox.Text), r.TableName))
                            {
                                reader.OnLoad += BulkReader_OnLoad;

                                using (SqlBulkCopy loader = new SqlBulkCopy(connection))
                                {
                                    loader.DestinationTableName = r.TableName;
                                    loader.BulkCopyTimeout = int.MaxValue;
                                    await loader.WriteToServerAsync(reader);
                                }
                            }
                        }
                        catch (Exception exc)
                        {
                            Logger.WriteError(exc);
                        }
                    };
                }

            }
            else
            {
                Logger.WriteMessage("No connection");
            }
        }

        private void RemoveTaskButton_Click(object sender, EventArgs e)
        {
            _form.Tasks.Remove(this);
            RecalculateRowCount();
            _form.TaskTabControl.TabPages.Remove(_form.TaskTabControl.SelectedTab);
        }

        private void RecalculateRowCount()
        {
            _form.RowsCnt = 0;
            _form.TotalRows = 0;
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

        private void CreateSQLTableButton_Click(object sender, EventArgs e)
        {
            CreateTable t = new CreateTable(this)
            {
                StartPosition = FormStartPosition.CenterParent
            };
            t.ShowDialog();
        }

        private void IntegratedSecurityCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _isIntegratedSecutiry = IntegratedSecurityCheckBox.Checked;
        }
    }
}