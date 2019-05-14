using BdfToMsSQL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbfToMsSQL
{
    public partial class Form1 : Form
    {
        public DateTime Expired;
        public bool IS_EXPIRED = false;
        bool _IS_ACTIVATED = false;
        public bool IS_ACTIVATED
        {
            get { return _IS_ACTIVATED; }
            set
            {
                _IS_ACTIVATED = value;                
            }
        }

        bool _IS_ERROR = false;
        public bool IS_ERROR
        {
            get { return _IS_ERROR; }
            set
            {
                startToolStripMenuItem.Enabled = !value;
                _IS_ERROR = value;
            }
        }

        public List<UserControl1> Tasks = new List<UserControl1>();
        public int RowsCnt = 0;
        public int TotalRows = 0;
        public void SetStatus(int TotalRows)
        {
            this.TotalRows += TotalRows;
            listBox_validate_errors.Items.Clear();
            listBox_validate_errors.Items.Add("Loading...  " + (((float)this.TotalRows / (float)RowsCnt) * 100).ToString("f2") + "%");
        }
        public Form1()
        {
            InitializeComponent();            
        }

        private void addTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserControl1 c = new UserControl1(this);
            Tasks.Add(c);
            c.Dock = DockStyle.Top;
            this.panel1.Controls.Add(c);
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TotalRows = 0;
            bool is_errors = false;
            Tasks.ForEach(t =>
            {
                t.reader.ForEach(r =>
                {
                    List<string> DBF_Fields = r.FieldName.ToList();
                    List<string> SQL_Fields = new List<string>();
                    foreach (var item in t.listBox_swl_table_fields.Items)
                    {
                        SQL_Fields.Add(item.ToString());
                        if (!DBF_Fields.Contains(item.ToString()))
                        {
                            is_errors = true;
                        }
                    }
                    DBF_Fields.ForEach(item =>
                    {
                        if (!SQL_Fields.Contains(item))
                        {
                            is_errors = true;
                        }
                    });
                });
            });

            if (!is_errors)
            {

                this.panel1.Enabled =
                addTaskToolStripMenuItem.Enabled =
                startToolStripMenuItem.Enabled = false;

                new Thread(_ =>
                {
                    Tasks.ForEach(t =>
                    {
                        t.start_import();
                    });

                    this.Invoke(new FormMethod(() =>
                    {
                        this.panel1.Enabled =
                        addTaskToolStripMenuItem.Enabled =
                            startToolStripMenuItem.Enabled = true;
                    }));

                }).Start();
            }
            else
            {
                listBox_validate_errors.Items.Add("I can not start, there is an error.");
            }
        }
        private delegate void FormMethod();

        private void activateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnterKey w = new EnterKey(this,IS_EXPIRED);
            w.StartPosition = FormStartPosition.CenterParent;           
            w.ShowDialog();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {                              
        }
    }

}
