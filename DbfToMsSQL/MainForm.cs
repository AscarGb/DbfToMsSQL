using BdfToMsSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbfToMsSQL
{
    public partial class MainForm : Form
    {
        private delegate void MethodInvoker();
        public List<DbTaskUserControl> Tasks = new List<DbTaskUserControl>();
        public int RowsCnt = 0;
        public int TotalRows = 0;
        public void SetStatus(int totalRows)
        {
            TotalRows += totalRows;
            InfoListBox.Items.Clear();
            InfoListBox.Items.Add($"Loading...  {(TotalRows / (float)RowsCnt * 100).ToString("f2") }%");
        }
        public MainForm()
        {
            InitializeComponent();
        }

        private void AddTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DbTaskUserControl dbTaskUserControl = new DbTaskUserControl(this);
            Tasks.Add(dbTaskUserControl);
            dbTaskUserControl.Dock = DockStyle.Top;
            MainPanel.Controls.Add(dbTaskUserControl);
        }

        private void StartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TotalRows = 0;
            bool is_errors = false;
            Tasks.ForEach(t =>
            {
                t.Readers?.ForEach(r =>
                {
                    List<string> dbfFields = r.FieldName.ToList();
                    List<string> sqlFields = new List<string>();

                    foreach (object item in t.TableFieldsListBox.Items)
                    {
                        sqlFields.Add(item.ToString());
                        if (!dbfFields.Contains(item.ToString()))
                        {
                            is_errors = true;
                        }
                    }
                    dbfFields.ForEach(item =>
                    {
                        if (!sqlFields.Contains(item))
                        {
                            is_errors = true;
                        }
                    });
                });
            });

            if (!is_errors)
            {
                Task.Run(() =>
                {
                    Invoke(() =>
                    {
                        MainPanel.Enabled =
                        AddTaskToolStripMenuItem.Enabled =
                        StartToolStripMenuItem.Enabled = false;
                    });

                    try
                    {
                        Tasks.ForEach(task =>
                        {
                            task.StartImport();
                        });

                    }
                    catch (Exception exc)
                    {
                        Invoke(() =>
                        {
                            InfoListBox.Items.Add($"Error: {exc.Message }\r\n {exc.InnerException?.ToString() ?? ""}");
                        });
                    }
                    finally
                    {
                        Invoke(() =>
                        {
                            MainPanel.Enabled =
                            AddTaskToolStripMenuItem.Enabled =
                                StartToolStripMenuItem.Enabled = true;
                        });
                    }

                });
            }
            else
            {
                InfoListBox.Items.Add("Can't start, there is an errors.");
            }
        }

        internal void Invoke(Action p)
        {
            Invoke(new MethodInvoker(p));
        }
    }

}
