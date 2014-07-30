using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace IT_Asset_Management_Project
{
    public partial class AddRemoveUserAdmin : Form
    {
        public AddRemoveUserAdmin()
        {
            InitializeComponent();
        }

        private void AddRemoveUserAdmin_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'userDBDataSet.tblRoles' table. You can move, or remove it, as needed.
            this.tblRolesTableAdapter.Fill(this.userDBDataSet.tblRoles);
            // TODO: This line of code loads data into the 'userDBDataSet.tblUser' table. You can move, or remove it, as needed.
            this.tblUserTableAdapter.Fill(this.userDBDataSet.tblUser);

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void writetodatabase(string UserName,String Password,int RoleID)
        {

            SqlConnection objcon = new SqlConnection("Server=Divya-PC;Database=UserDB;user id=sa;password=viperx");
            SqlCommand objCmd = new SqlCommand();
            objCmd.CommandText="Insert into tblUser(username,roleid,password)  values(@username,@roleid,@password)";
            objCmd.Parameters.Add("@username", SqlDbType.VarChar, 50).Value = UserName;
            objCmd.Parameters.Add("@password", SqlDbType.VarChar, 50).Value = Password;
            objCmd.Parameters.Add("@roleid", SqlDbType.Int).Value = RoleID+1;
            objCmd.Connection = objcon;
            objCmd.CommandType = CommandType.Text;
            
            try
            {
                objcon.Open();
                objCmd.ExecuteNonQuery();

                objCmd.Dispose();
                objcon.Close();
                MessageBox.Show("User Created Successfully");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (objcon.State == ConnectionState.Open)
                    objcon.Close();
             
            }
            
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if(!textBox1.Text.Equals(textBox2.Text))
            {
                MessageBox.Show("Passwords Dont Match !");
                textBox1.ResetText();
                textBox2.ResetText();
            }

            SqlConnection objcon = new SqlConnection("Server=Divya-PC;Database=UserDB;user id=sa;password=viperx");
            SqlCommand objCmd = new SqlCommand("Select username from tblUser where username=@username");
            objCmd.Parameters.Add("@username", SqlDbType.VarChar, 50).Value = comboBox1.Text;
            objCmd.CommandType = CommandType.Text;
            objCmd.Connection = objcon;
            SqlDataReader objReader;
            try
            {
                objcon.Open();
                objReader = objCmd.ExecuteReader();

                if (objReader.Read())
                {
                    if (objReader[0].ToString().Equals(comboBox1.Text))
                    {
                        MessageBox.Show("User ALready Exists");
                    }
                    else
                    {

                        writetodatabase(comboBox1.Text, textBox1.Text, listBox1.SelectedIndex);

                    }
                }

                objReader.Close();

            }

            catch (Exception ex)
            {

            }
            finally
            {
                if (objcon.State == ConnectionState.Open)
                    objcon.Close();

            }
    

        }
    }
}
