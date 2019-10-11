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
                Dock = DockStyle.Fill
            };          

            Tasks.Add(dbTaskUserControl);

            var page = new TabPage($"Task {(TaskTabControl.TabCount + 1).ToString()}");

            page.Controls.Add(dbTaskUserControl);

            TaskTabControl.TabPages.Add(page);
        }

        private async void StartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TotalRows = 0;
            bool is_errors = false;

            if (!is_errors)
            {
                TaskTabControl.Enabled =
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
                    TaskTabControl.Enabled =
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
