using DbfToMsSQL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BdfToMsSQL
{
    public partial class EnterKey : Form
    {
        Form1 Parent;
        bool IS_EXPIRED = false;
        public EnterKey(Form1 Parent,bool IS_EXPIRED)
        {
            this.IS_EXPIRED = IS_EXPIRED;
            this.Parent = Parent;
            InitializeComponent();

            if (IS_EXPIRED) {
                this.FormClosing += (a, b) => { Application.Exit(); };
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            this.Close();
        }
    }
}
