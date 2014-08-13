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
    public partial class CheckConnection : Form
    {
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

        public CheckConnection()
        {
            InitializeComponent();
            IsConnectedUser = false;
            IsConnectedITAssets = false;
            try
            {
                SqlConnection objcon = new SqlConnection("Server=Divya-PC;Database=UserDB;user id=sa;password=viperx");
                objcon.Open();
                // add check for Open Connection
                if (objcon.State == ConnectionState.Open)
                {
                    label2.Text = "Connected";
                    IsConnectedUser = true;
                }
                else
                {
                    label2.Text = "Not Connected";
                }
                objcon.Close();

                objcon = new SqlConnection("Data Source=DIVYA-PC;Initial Catalog=db_ITAssets;Persist Security Info=True;User ID=sa;password=viperx");
                objcon.Open();
                if (objcon.State == ConnectionState.Open)
                {
                    label4.Text = "Connected";
                    IsConnectedITAssets = true;
                }
                else
                {
                    label4.Text = "Not Connected";
                }

 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }
    }
}