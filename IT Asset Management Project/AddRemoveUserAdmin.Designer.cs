namespace IT_Asset_Management_Project
{
    partial class AddRemoveUserAdmin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.userDBDataSet = new IT_Asset_Management_Project.UserDBDataSet();
            this.tblUserBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tblUserTableAdapter = new IT_Asset_Management_Project.UserDBDataSetTableAdapters.tblUserTableAdapter();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.tblRolesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tblRolesTableAdapter = new IT_Asset_Management_Project.UserDBDataSetTableAdapters.tblRolesTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.userDBDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblUserBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblRolesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.comboBox1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.tblUserBindingSource, "username", true));
            this.comboBox1.DataSource = this.tblUserBindingSource;
            this.comboBox1.DisplayMember = "username";
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(52, 60);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(248, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // userDBDataSet
            // 
            this.userDBDataSet.DataSetName = "UserDBDataSet";
            this.userDBDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tblUserBindingSource
            // 
            this.tblUserBindingSource.DataMember = "tblUser";
            this.tblUserBindingSource.DataSource = this.userDBDataSet;
            // 
            // tblUserTableAdapter
            // 
            this.tblUserTableAdapter.ClearBeforeFill = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(52, 97);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(248, 20);
            this.textBox1.TabIndex = 1;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(52, 123);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(248, 20);
            this.textBox2.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(52, 262);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(248, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Add New User";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedIndex", this.tblRolesBindingSource, "RoleName", true));
            this.listBox1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.tblRolesBindingSource, "RoleName", true));
            this.listBox1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.tblRolesBindingSource, "RoleName", true));
            this.listBox1.DataSource = this.tblRolesBindingSource;
            this.listBox1.DisplayMember = "RoleName";
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(205, 149);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(95, 69);
            this.listBox1.TabIndex = 4;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // tblRolesBindingSource
            // 
            this.tblRolesBindingSource.DataMember = "tblRoles";
            this.tblRolesBindingSource.DataSource = this.userDBDataSet;
            // 
            // tblRolesTableAdapter
            // 
            this.tblRolesTableAdapter.ClearBeforeFill = true;
            // 
            // AddRemoveUserAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 426);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.comboBox1);
            this.Name = "AddRemoveUserAdmin";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.AddRemoveUserAdmin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.userDBDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblUserBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblRolesBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private UserDBDataSet userDBDataSet;
        private System.Windows.Forms.BindingSource tblUserBindingSource;
        private UserDBDataSetTableAdapters.tblUserTableAdapter tblUserTableAdapter;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.BindingSource tblRolesBindingSource;
        private UserDBDataSetTableAdapters.tblRolesTableAdapter tblRolesTableAdapter;
    }
}