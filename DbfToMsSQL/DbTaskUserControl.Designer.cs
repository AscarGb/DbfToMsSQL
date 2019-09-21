namespace BdfToMsSQL
{
    partial class DbTaskUserControl
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

                Readers?.ForEach(a => a?.Dispose());
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
            this.IntegratedSecurityCheckBox = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.TableFieldsListBox = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TablesListbox = new System.Windows.Forms.ListBox();
            this.UserNameTextBox = new System.Windows.Forms.TextBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.DbNameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ConnectToSqlButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SQLServerAdressTextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.EncodingComboBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.CreateSQLTableButton = new System.Windows.Forms.Button();
            this.RemoveTaskButton = new System.Windows.Forms.Button();
            this.ValidationLabel = new System.Windows.Forms.Label();
            this.DbfFilesListBox = new System.Windows.Forms.ListBox();
            this.DbfCountLabel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.BbfFieldsLabel = new System.Windows.Forms.ListBox();
            this.OpenDbfButton = new System.Windows.Forms.Button();
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
            this.groupBox1.Controls.Add(this.IntegratedSecurityCheckBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.TableFieldsListBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.TablesListbox);
            this.groupBox1.Controls.Add(this.UserNameTextBox);
            this.groupBox1.Controls.Add(this.PasswordTextBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.DbNameTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.ConnectToSqlButton);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.SQLServerAdressTextBox);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(557, 307);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MS SQL Connection parameters";
            // 
            // IntegratedSecurityCheckBox
            // 
            this.IntegratedSecurityCheckBox.AutoSize = true;
            this.IntegratedSecurityCheckBox.Location = new System.Drawing.Point(362, 117);
            this.IntegratedSecurityCheckBox.Name = "IntegratedSecurityCheckBox";
            this.IntegratedSecurityCheckBox.Size = new System.Drawing.Size(115, 17);
            this.IntegratedSecurityCheckBox.TabIndex = 25;
            this.IntegratedSecurityCheckBox.Text = "Integrated Security";
            this.IntegratedSecurityCheckBox.UseVisualStyleBackColor = true;
            this.IntegratedSecurityCheckBox.CheckedChanged += new System.EventHandler(this.IntegratedSecurityCheckBox_CheckedChanged);
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
            // TableFieldsListBox
            // 
            this.TableFieldsListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TableFieldsListBox.FormattingEnabled = true;
            this.TableFieldsListBox.Items.AddRange(new object[] {
            " "});
            this.TableFieldsListBox.Location = new System.Drawing.Point(164, 220);
            this.TableFieldsListBox.Name = "TableFieldsListBox";
            this.TableFieldsListBox.Size = new System.Drawing.Size(313, 67);
            this.TableFieldsListBox.TabIndex = 22;
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
            // TablesListbox
            // 
            this.TablesListbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TablesListbox.FormattingEnabled = true;
            this.TablesListbox.Items.AddRange(new object[] {
            " "});
            this.TablesListbox.Location = new System.Drawing.Point(164, 142);
            this.TablesListbox.Name = "TablesListbox";
            this.TablesListbox.Size = new System.Drawing.Size(313, 67);
            this.TablesListbox.TabIndex = 20;
            this.TablesListbox.SelectedIndexChanged += new System.EventHandler(this.TablesListbox_SelectedIndexChanged);
            // 
            // UserNameTextBox
            // 
            this.UserNameTextBox.Location = new System.Drawing.Point(164, 83);
            this.UserNameTextBox.Name = "UserNameTextBox";
            this.UserNameTextBox.Size = new System.Drawing.Size(113, 20);
            this.UserNameTextBox.TabIndex = 19;
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Location = new System.Drawing.Point(364, 83);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.PasswordChar = '*';
            this.PasswordTextBox.Size = new System.Drawing.Size(113, 20);
            this.PasswordTextBox.TabIndex = 18;
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
            // DbNameTextBox
            // 
            this.DbNameTextBox.Location = new System.Drawing.Point(164, 57);
            this.DbNameTextBox.Name = "DbNameTextBox";
            this.DbNameTextBox.Size = new System.Drawing.Size(313, 20);
            this.DbNameTextBox.TabIndex = 15;
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
            // ConnectToSqlButton
            // 
            this.ConnectToSqlButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ConnectToSqlButton.Location = new System.Drawing.Point(164, 113);
            this.ConnectToSqlButton.Name = "ConnectToSqlButton";
            this.ConnectToSqlButton.Size = new System.Drawing.Size(85, 23);
            this.ConnectToSqlButton.TabIndex = 13;
            this.ConnectToSqlButton.Text = "Connect";
            this.ConnectToSqlButton.UseVisualStyleBackColor = true;
            this.ConnectToSqlButton.Click += new System.EventHandler(this.ConnectToSqlButton_Click);
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
            // SQLServerAdressTextBox
            // 
            this.SQLServerAdressTextBox.Location = new System.Drawing.Point(164, 31);
            this.SQLServerAdressTextBox.Name = "SQLServerAdressTextBox";
            this.SQLServerAdressTextBox.Size = new System.Drawing.Size(313, 20);
            this.SQLServerAdressTextBox.TabIndex = 11;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.EncodingComboBox);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.CreateSQLTableButton);
            this.groupBox2.Controls.Add(this.RemoveTaskButton);
            this.groupBox2.Controls.Add(this.ValidationLabel);
            this.groupBox2.Controls.Add(this.DbfFilesListBox);
            this.groupBox2.Controls.Add(this.DbfCountLabel);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.BbfFieldsLabel);
            this.groupBox2.Controls.Add(this.OpenDbfButton);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(553, 307);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DBF parameters";
            // 
            // EncodingComboBox
            // 
            this.EncodingComboBox.FormattingEnabled = true;
            this.EncodingComboBox.Items.AddRange(new object[] {
            "1251",
            "866"});
            this.EncodingComboBox.Location = new System.Drawing.Point(76, 217);
            this.EncodingComboBox.Name = "EncodingComboBox";
            this.EncodingComboBox.Size = new System.Drawing.Size(121, 21);
            this.EncodingComboBox.TabIndex = 32;
            this.EncodingComboBox.Text = "1251";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 220);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 31;
            this.label9.Text = "Encoding";
            // 
            // CreateSQLTableButton
            // 
            this.CreateSQLTableButton.Enabled = false;
            this.CreateSQLTableButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CreateSQLTableButton.Location = new System.Drawing.Point(21, 157);
            this.CreateSQLTableButton.Name = "CreateSQLTableButton";
            this.CreateSQLTableButton.Size = new System.Drawing.Size(111, 23);
            this.CreateSQLTableButton.TabIndex = 30;
            this.CreateSQLTableButton.Text = "Create SQL table";
            this.CreateSQLTableButton.UseVisualStyleBackColor = true;
            this.CreateSQLTableButton.Click += new System.EventHandler(this.CreateSQLTableButton_Click);
            // 
            // RemoveTaskButton
            // 
            this.RemoveTaskButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveTaskButton.Location = new System.Drawing.Point(347, 266);
            this.RemoveTaskButton.Name = "RemoveTaskButton";
            this.RemoveTaskButton.Size = new System.Drawing.Size(120, 23);
            this.RemoveTaskButton.TabIndex = 29;
            this.RemoveTaskButton.Text = "Remove this task";
            this.RemoveTaskButton.UseVisualStyleBackColor = true;
            this.RemoveTaskButton.Click += new System.EventHandler(this.RemoveTaskButton_Click);
            // 
            // ValidationLabel
            // 
            this.ValidationLabel.AutoSize = true;
            this.ValidationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ValidationLabel.Location = new System.Drawing.Point(17, 264);
            this.ValidationLabel.Name = "ValidationLabel";
            this.ValidationLabel.Size = new System.Drawing.Size(45, 22);
            this.ValidationLabel.TabIndex = 28;
            this.ValidationLabel.Text = "   __";
            // 
            // DbfFilesListBox
            // 
            this.DbfFilesListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DbfFilesListBox.FormattingEnabled = true;
            this.DbfFilesListBox.Items.AddRange(new object[] {
            " "});
            this.DbfFilesListBox.Location = new System.Drawing.Point(169, 24);
            this.DbfFilesListBox.Name = "DbfFilesListBox";
            this.DbfFilesListBox.Size = new System.Drawing.Size(298, 67);
            this.DbfFilesListBox.TabIndex = 27;
            this.DbfFilesListBox.SelectedIndexChanged += new System.EventHandler(this.DbfFilesListBox_SelectedIndexChanged);
            // 
            // DbfCountLabel
            // 
            this.DbfCountLabel.AutoSize = true;
            this.DbfCountLabel.Location = new System.Drawing.Point(88, 64);
            this.DbfCountLabel.Name = "DbfCountLabel";
            this.DbfCountLabel.Size = new System.Drawing.Size(13, 13);
            this.DbfCountLabel.TabIndex = 24;
            this.DbfCountLabel.Text = "0";
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
            // BbfFieldsLabel
            // 
            this.BbfFieldsLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BbfFieldsLabel.FormattingEnabled = true;
            this.BbfFieldsLabel.Items.AddRange(new object[] {
            " "});
            this.BbfFieldsLabel.Location = new System.Drawing.Point(169, 113);
            this.BbfFieldsLabel.Name = "BbfFieldsLabel";
            this.BbfFieldsLabel.Size = new System.Drawing.Size(298, 67);
            this.BbfFieldsLabel.TabIndex = 21;
            // 
            // OpenDbfButton
            // 
            this.OpenDbfButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OpenDbfButton.Location = new System.Drawing.Point(21, 24);
            this.OpenDbfButton.Name = "OpenDbfButton";
            this.OpenDbfButton.Size = new System.Drawing.Size(111, 23);
            this.OpenDbfButton.TabIndex = 1;
            this.OpenDbfButton.Text = "Open DBF files";
            this.OpenDbfButton.UseVisualStyleBackColor = true;
            this.OpenDbfButton.Click += new System.EventHandler(this.BtnOpenDbf_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Multiselect = true;
            // 
            // DbTaskUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "DbTaskUserControl";
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
        private System.Windows.Forms.TextBox UserNameTextBox;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ConnectToSqlButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SQLServerAdressTextBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label ValidationLabel;
        private System.Windows.Forms.ListBox DbfFilesListBox;
        private System.Windows.Forms.Label DbfCountLabel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListBox BbfFieldsLabel;
        private System.Windows.Forms.Button OpenDbfButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button RemoveTaskButton;
        public System.Windows.Forms.ListBox TableFieldsListBox;
        public System.Windows.Forms.ListBox TablesListbox;
        private System.Windows.Forms.Button CreateSQLTableButton;
        public System.Windows.Forms.TextBox DbNameTextBox;
        private System.Windows.Forms.ComboBox EncodingComboBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox IntegratedSecurityCheckBox;        
    }
}
