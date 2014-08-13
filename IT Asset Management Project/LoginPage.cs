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
    public partial class LoginPage : Form
    {
        public bool IsAuthenticated
        {
            get;
            set;
        }
        public string UserName
        {
            get;
            set;

        }
        public int RoleID
        {
            get;
            set;

        }
        public LoginPage()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void LoginPage_Load(object sender, EventArgs e)
        {
            CheckConnection();
            if(IsConnectedITAssets== true && IsConnectedUser==true)
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        public bool IsConnectedUser
        {
            set;
            get;



        }
        public bool IsConnectedITAssets
        {
            set;
            get;

        }

        public void CheckConnection()
        {
            IsConnectedUser = false;
            IsConnectedITAssets = false;
            try
            {
                SqlConnection objcon = new SqlConnection("Server=Divya-PC;Database=UserDB;user id=sa;password=viperx");
                objcon.Open();
                // add check for Open Connection
                if (objcon.State == ConnectionState.Open)
                {
                    label3.Text = "Connected";
                    IsConnectedUser = true;
                }
                else
                {
                    label3.Text = "Not Connected";
                }
                objcon.Close();

                objcon = new SqlConnection("Data Source=DIVYA-PC;Initial Catalog=db_ITAssets;Persist Security Info=True;User ID=sa;password=viperx");
                objcon.Open();
                if (objcon.State == ConnectionState.Open)
                {
                    label5.Text = "Connected";
                    IsConnectedITAssets = true;
                }
                else
                {
                    label5.Text = "Not Connected";
                
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            if (txtUserID.Text.Length > 0 && txtPassword.Text.Length > 0 )
            {
                SqlConnection objcon = new SqlConnection("Server=Divya-PC;Database=UserDB;user id=sa;password=viperx");
                SqlCommand objCmd = new SqlCommand("Select username,roleid from tblUser where username=@username and password=@password");
                objCmd.Parameters.Add("@username", SqlDbType.VarChar, 50).Value = txtUserID.Text;
                objCmd.Parameters.Add("@password", SqlDbType.VarChar, 50).Value = txtPassword.Text;
                objCmd.CommandType = CommandType.Text;
                objCmd.Connection = objcon;
                SqlDataReader objReader;
                try
                {
                    objcon.Open();
                    objReader = objCmd.ExecuteReader();
                   
                    if (objReader.Read())
                    {
                        IsAuthenticated = true;
                        UserName = objReader[0].ToString();
                        RoleID = int.Parse(objReader[1].ToString());

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
                    if (IsAuthenticated)
                        MessageBox.Show("Welcome "+UserName.ToUpper());

               }

                this.Close();


            }
            else
            {
                MessageBox.Show("Please Enter Login Details");

            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }

        private void txtPassword_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}