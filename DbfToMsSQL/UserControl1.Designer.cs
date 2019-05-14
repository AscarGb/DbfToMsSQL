namespace BdfToMsSQL
{
    partial class UserControl1
    {
        /// <summary> 
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.listBox_swl_table_fields = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.listbox_Tables = new System.Windows.Forms.ListBox();
            this.tb_user_name = new System.Windows.Forms.TextBox();
            this.tb_psw = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_db_name = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_connect_to_sql = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_SQLServerAdress = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnCreateTable = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label_valid = new System.Windows.Forms.Label();
            this.listBox_files = new System.Windows.Forms.ListBox();
            this.lbl_dbf_count = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.listBox_dbf_fields = new System.Windows.Forms.ListBox();
            this.btn_open_dbf = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(1114, 307);
            this.splitContainer1.SplitterDistance = 557;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.listBox_swl_table_fields);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.listbox_Tables);
            this.groupBox1.Controls.Add(this.tb_user_name);
            this.groupBox1.Controls.Add(this.tb_psw);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tb_db_name);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btn_connect_to_sql);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tb_SQLServerAdress);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(557, 307);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MS SQL Connection parameters";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(97, 220);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Table fileds";
            // 
            // listBox_swl_table_fields
            // 
            this.listBox_swl_table_fields.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox_swl_table_fields.FormattingEnabled = true;
            this.listBox_swl_table_fields.Items.AddRange(new object[] {
            " "});
            this.listBox_swl_table_fields.Location = new System.Drawing.Point(164, 220);
            this.listBox_swl_table_fields.Name = "listBox_swl_table_fields";
            this.listBox_swl_table_fields.Size = new System.Drawing.Size(313, 67);
            this.listBox_swl_table_fields.TabIndex = 22;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(94, 142);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "User Tables";
            // 
            // listbox_Tables
            // 
            this.listbox_Tables.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listbox_Tables.FormattingEnabled = true;
            this.listbox_Tables.Items.AddRange(new object[] {
            " "});
            this.listbox_Tables.Location = new System.Drawing.Point(164, 142);
            this.listbox_Tables.Name = "listbox_Tables";
            this.listbox_Tables.Size = new System.Drawing.Size(313, 67);
            this.listbox_Tables.TabIndex = 20;
            this.listbox_Tables.SelectedIndexChanged += new System.EventHandler(this.listbox_Tables_SelectedIndexChanged);
            // 
            // tb_user_name
            // 
            this.tb_user_name.Location = new System.Drawing.Point(164, 83);
            this.tb_user_name.Name = "tb_user_name";
            this.tb_user_name.Size = new System.Drawing.Size(113, 20);
            this.tb_user_name.TabIndex = 19;
            this.tb_user_name.Text = "sa";
            // 
            // tb_psw
            // 
            this.tb_psw.Location = new System.Drawing.Point(364, 83);
            this.tb_psw.Name = "tb_psw";
            this.tb_psw.PasswordChar = '*';
            this.tb_psw.Size = new System.Drawing.Size(113, 20);
            this.tb_psw.TabIndex = 18;
            this.tb_psw.Text = "123123";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(305, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(100, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "User name";
            // 
            // tb_db_name
            // 
            this.tb_db_name.Location = new System.Drawing.Point(164, 57);
            this.tb_db_name.Name = "tb_db_name";
            this.tb_db_name.Size = new System.Drawing.Size(313, 20);
            this.tb_db_name.TabIndex = 15;
            this.tb_db_name.Text = "test";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(76, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Database name";
            // 
            // btn_connect_to_sql
            // 
            this.btn_connect_to_sql.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_connect_to_sql.Location = new System.Drawing.Point(164, 113);
            this.btn_connect_to_sql.Name = "btn_connect_to_sql";
            this.btn_connect_to_sql.Size = new System.Drawing.Size(85, 23);
            this.btn_connect_to_sql.TabIndex = 13;
            this.btn_connect_to_sql.Text = "Connect";
            this.btn_connect_to_sql.UseVisualStyleBackColor = true;
            this.btn_connect_to_sql.Click += new System.EventHandler(this.btn_connect_to_sql_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "MS SQL Server location";
            // 
            // tb_SQLServerAdress
            // 
            this.tb_SQLServerAdress.Location = new System.Drawing.Point(164, 31);
            this.tb_SQLServerAdress.Name = "tb_SQLServerAdress";
            this.tb_SQLServerAdress.Size = new System.Drawing.Size(313, 20);
            this.tb_SQLServerAdress.TabIndex = 11;
            this.tb_SQLServerAdress.Text = "localhost";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnCreateTable);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.label_valid);
            this.groupBox2.Controls.Add(this.listBox_files);
            this.groupBox2.Controls.Add(this.lbl_dbf_count);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.listBox_dbf_fields);
            this.groupBox2.Controls.Add(this.btn_open_dbf);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(553, 307);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DBF parameters";
            // 
            // btnCreateTable
            // 
            this.btnCreateTable.Enabled = false;
            this.btnCreateTable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateTable.Location = new System.Drawing.Point(21, 157);
            this.btnCreateTable.Name = "btnCreateTable";
            this.btnCreateTable.Size = new System.Drawing.Size(111, 23);
            this.btnCreateTable.TabIndex = 30;
            this.btnCreateTable.Text = "Create SQL table";
            this.btnCreateTable.UseVisualStyleBackColor = true;
            this.btnCreateTable.Click += new System.EventHandler(this.btnCreateTable_Click);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(347, 266);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 23);
            this.button1.TabIndex = 29;
            this.button1.Text = "Remove this task";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label_valid
            // 
            this.label_valid.AutoSize = true;
            this.label_valid.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label_valid.Location = new System.Drawing.Point(145, 238);
            this.label_valid.Name = "label_valid";
            this.label_valid.Size = new System.Drawing.Size(45, 22);
            this.label_valid.TabIndex = 28;
            this.label_valid.Text = "   __";
            // 
            // listBox_files
            // 
            this.listBox_files.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox_files.FormattingEnabled = true;
            this.listBox_files.Items.AddRange(new object[] {
            " "});
            this.listBox_files.Location = new System.Drawing.Point(169, 24);
            this.listBox_files.Name = "listBox_files";
            this.listBox_files.Size = new System.Drawing.Size(298, 67);
            this.listBox_files.TabIndex = 27;
            // 
            // lbl_dbf_count
            // 
            this.lbl_dbf_count.AutoSize = true;
            this.lbl_dbf_count.Location = new System.Drawing.Point(88, 64);
            this.lbl_dbf_count.Name = "lbl_dbf_count";
            this.lbl_dbf_count.Size = new System.Drawing.Size(13, 13);
            this.lbl_dbf_count.TabIndex = 24;
            this.lbl_dbf_count.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Rows count";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(98, 113);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "DBF fileds";
            // 
            // listBox_dbf_fields
            // 
            this.listBox_dbf_fields.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox_dbf_fields.FormattingEnabled = true;
            this.listBox_dbf_fields.Items.AddRange(new object[] {
            " "});
            this.listBox_dbf_fields.Location = new System.Drawing.Point(169, 113);
            this.listBox_dbf_fields.Name = "listBox_dbf_fields";
            this.listBox_dbf_fields.Size = new System.Drawing.Size(298, 67);
            this.listBox_dbf_fields.TabIndex = 21;
            // 
            // btn_open_dbf
            // 
            this.btn_open_dbf.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_open_dbf.Location = new System.Drawing.Point(21, 24);
            this.btn_open_dbf.Name = "btn_open_dbf";
            this.btn_open_dbf.Size = new System.Drawing.Size(111, 23);
            this.btn_open_dbf.TabIndex = 1;
            this.btn_open_dbf.Text = "Open DBF files";
            this.btn_open_dbf.UseVisualStyleBackColor = true;
            this.btn_open_dbf.Click += new System.EventHandler(this.btn_open_dbf_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Multiselect = true;
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(1114, 307);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_user_name;
        private System.Windows.Forms.TextBox tb_psw;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_connect_to_sql;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_SQLServerAdress;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label_valid;
        private System.Windows.Forms.ListBox listBox_files;
        private System.Windows.Forms.Label lbl_dbf_count;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListBox listBox_dbf_fields;
        private System.Windows.Forms.Button btn_open_dbf;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.ListBox listBox_swl_table_fields;
        public System.Windows.Forms.ListBox listbox_Tables;
        private System.Windows.Forms.Button btnCreateTable;
        public System.Windows.Forms.TextBox tb_db_name;

    }
}
