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
        public List<DbTaskUserControl> Tasks { get; set; } = new List<DbTaskUserControl>();
        public int RowsCnt { get; set; } = 0;
        public int TotalRows { get; set; } = 0;
        public void SetStatus(int totalRows)
        {
            TotalRows += totalRows;

            Logger.Clear();
            Logger.WriteMessage($"Loading...  {(TotalRows / (float)RowsCnt * 100).ToString("f2") }%");
        }
        public MainForm()
        {
            InitializeComponent();

            Logger.Form = this;
            Logger.ListBox = InfoListBox;
        }

        private void AddTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DbTaskUserControl dbTaskUserControl = new DbTaskUserControl(this)
            {
                Dock = DockStyle.Top
            };

            Tasks.Add(dbTaskUserControl);
            MainPanel.Controls.Add(dbTaskUserControl);

            MainPanel.ScrollControlIntoView(dbTaskUserControl);
        }

        private async void StartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TotalRows = 0;
            bool is_errors = false;
            Tasks.ForEach(t =>
            {
                t.Readers?.ForEach(r =>
                {
                    List<string> dbfFields = r.Fields.Select(a => a.Name).ToList();
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
                MainPanel.Enabled =
                AddTaskToolStripMenuItem.Enabled =
                StartToolStripMenuItem.Enabled = false;

                try
                {
                    await Task.WhenAll(Tasks.Select(t => t.StartImport()));
                }
                catch (Exception exc)
                {
                    Logger.WriteError(exc);
                }
                finally
                {
                    MainPanel.Enabled =
                    AddTaskToolStripMenuItem.Enabled =
                        StartToolStripMenuItem.Enabled = true;
                }
            }
            else
            {
                Logger.WriteMessage("Can't start, there is an errors.");
            }
        }

        internal void Invoke(Action p)
        {
            Invoke(new MethodInvoker(p));
        }
    }

}
