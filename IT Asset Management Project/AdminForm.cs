using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Runtime.InteropServices;
using System.Data.SqlClient;


namespace IT_Asset_Management_Project
{
    public partial class AdminForm : Form
    {
        public String UserName
        {
            set;
            get;
        }
        public bool IsDomain
        {
            set;
            get;
        }
    
        public void CurrentConfig()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listBox5.Items.Clear();
            listBox6.Items.Clear();
      
         
            // Processor Info
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                label50.Text = queryObj["Name"].ToString();
                label51.Text = queryObj["NumberOfCores"].ToString();
                label59.Text = queryObj["NumberOfLogicalProcessors"].ToString();
                label52.Text =  (double.Parse(queryObj["MaxClockSpeed"].ToString())/1000).ToString()+" GHz";
            
                
            }
            // Account Info
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_UserAccount");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                listBox4.Items.Add(queryObj["Name"].ToString());
                listBox5.Items.Add(queryObj["Status"].ToString());
                listBox6.Items.Add(queryObj["PasswordRequired"].ToString());
    
            }

            // GenuineStatus
            string ComputerName = "localhost";
            ManagementScope Scope;
            Scope = new ManagementScope(String.Format("\\\\{0}\\root\\CIMV2", ComputerName), null);

            Scope.Connect();
            ObjectQuery Query = new ObjectQuery("SELECT * FROM SoftwareLicensingProduct Where PartialProductKey <> null AND LicenseIsAddon=False");
            ManagementObjectSearcher Searcher = new ManagementObjectSearcher(Scope, Query);
          
            foreach (ManagementObject WmiObject in Searcher.Get())
            {
                if (WmiObject["LicenseStatus"].ToString() == "1")
                    label62.Text = "Activated";
                else
                    label62.Text = "Not Activated";
            }
            
            
            // ComputerSystem
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_ComputerSystem");
            
            foreach (ManagementObject queryObj in searcher.Get())
            {
                label45.Text = queryObj["Name"].ToString();
                label47.Text = queryObj["Manufacturer"].ToString();
                label46.Text = queryObj["Model"].ToString();
                label49.Text = queryObj["TotalPhysicalMemory"].ToString();
                
                IsDomain =  bool.Parse(queryObj["PartOfDomain"].ToString());
              if (!IsDomain)
              {
                  label60.Text="Not Part Of Domain";
                  label61.Text = queryObj["Workgroup"].ToString();
              }
              else
              {
                  label60.Text=queryObj["Domain"].ToString();
                  label61.Text="Not Part Of Workgroup";
              }
            }

            // Operating System
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                label48.Text = queryObj["Caption"].ToString()+queryObj["Version"].ToString();
                label55.Text = queryObj["Caption"].ToString();
                label54.Text = queryObj["SerialNumber"].ToString();
                label56.Text = queryObj["Version"].ToString();
                label57.Text = queryObj["InstallDate"].ToString();

            }

            // Disk Drives
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                
                comboBox1.Items.Add(queryObj["Model"].ToString());

            }
            comboBox1.SelectedIndex = 0;
            
            // Network Adapters
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapter");
            bool IsPhysical;
            
            foreach (ManagementObject queryObj in searcher.Get())
            {
                IsPhysical=bool.Parse(queryObj["PhysicalAdapter"].ToString());
                
                if (IsPhysical )
                {
                    int NetConStat=int.Parse(queryObj["NetConnectionStatus"].ToString()) ;
                    
                    if (NetConStat== 7 | NetConStat==2)
                   {
                       if (int.Parse(queryObj["NetConnectionStatus"].ToString()).Equals(2))
                       {
                          label84.Text = (queryObj["MACAddress"].ToString());
                          label83.Text = queryObj["Description"].ToString();
                       }
                           listBox2.Items.Add(queryObj["Description"].ToString());
                }
                }
            
                
            }
            // Establishing Currrent Adapter
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapterConfiguration");
            
            foreach (ManagementObject queryObj in searcher.Get())
            {
            
                 
                if (bool.Parse(queryObj["IPEnabled"].ToString())==true)
                // int.Parse(queryObj["IPConnectionMetric"].ToString())>1 ) // 
                {
                   
                            label85.Text = (queryObj["MACAddress"].ToString());
                            label86.Text = queryObj["Description"].ToString();
                }
                   
            }


            

            // CDROM Drives
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_CDROMDrive");
            
            foreach (ManagementObject queryObj in searcher.Get())
            {

                    listBox3.Items.Add(queryObj["Caption"].ToString()+"   Drive : "+queryObj["Drive"].ToString());
                
            }

    
        }
        public AdminForm()
        {
            
            InitializeComponent();
            
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'db_ITAssetsDataSet.tbl_Faculty' table. You can move, or remove it, as needed.
            this.tbl_FacultyTableAdapter.Fill(this.db_ITAssetsDataSet.tbl_Faculty);

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void listView4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label57_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem  = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            MessageBox.Show(selectedItem);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive");

            foreach (ManagementObject queryObj in searcher.Get())
            {

                if (selectedItem == queryObj["Model"].ToString())
                {
                   listBox1.Items.Add("Serial No" );
                    listBox1.Items.Add( queryObj["SerialNumber"].ToString());
                    listBox1.Items.Add(" ");
                   listBox1.Items.Add("Size :" );
                   listBox1.Items.Add(((double.Parse(queryObj["Size"].ToString())) / 1000000000).ToString() + " GB");
                   listBox1.Items.Add( " ");
                    listBox1.Items.Add("No Of Partitions : ");
                    listBox1.Items.Add(queryObj["Partitions"].ToString());

                }
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

       

        private void button3_Click(object sender, EventArgs e)
        {
        
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label59_Click(object sender, EventArgs e)
        {

        }

        private void label45_Click(object sender, EventArgs e)
        {

        }

        private void getCurrentConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentConfig();
        }

        private void addRemoveUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddRemoveUserAdmin objAddRemove = new AddRemoveUserAdmin();
            objAddRemove.ShowDialog();

        }

        private void storeNewInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewMAchineData objnewdata = new NewMAchineData();
            objnewdata.ShowDialog();
        }

        private void label41_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

            SqlConnection objcon = new SqlConnection("Data Source=DIVYA-PC;Initial Catalog=db_ITAssets;Persist Security Info=True;User ID=sa;password=viperx");

            SqlCommand objCmd = new SqlCommand("Select dept_name from tbl_Department where faculty_id=@faculty_id");
            objCmd.Parameters.Add("@faculty_id", SqlDbType.Int).Value = (comboBox3.SelectedIndex) + 1;
            objCmd.Connection = objcon;
            objCmd.CommandType = CommandType.Text;


            objCmd.Connection = objcon;
            SqlDataReader objReader;
            comboBox2.Items.Clear();
            try
            {
                objcon.Open();
                MessageBox.Show(objcon.State.ToString());

                objReader = objCmd.ExecuteReader();

                //      if (objReader.Read())
                //     {
                while (objReader.Read())
                {
                    comboBox2.Items.Add(objReader[0].ToString());
                }
                //      }
                objReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                if (objcon.State == ConnectionState.Open)
                    objcon.Close();


            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void groupBox12_Enter(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void backToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CurrentConfig();
        }

        private void label86_Click(object sender, EventArgs e)
        {

        }

        private void label85_Click(object sender, EventArgs e)
        {

        }

                
    }
    
}

