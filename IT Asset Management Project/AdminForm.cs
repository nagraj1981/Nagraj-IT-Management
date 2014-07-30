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
            Int16 i = 0;
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
            // Network Adapters
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapter");
            bool IsPhysical;
            foreach (ManagementObject queryObj in searcher.Get())
            {
                IsPhysical=bool.Parse(queryObj["PhysicalAdapter"].ToString());
                if (IsPhysical)
                {
                   listBox2.Items.Add(queryObj["Description"].ToString());
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
            CurrentConfig();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
                MessageBox.Show("For New Entry  Into Database Pl Specify Unique ID");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddRemoveUserAdmin objAddRemove = new AddRemoveUserAdmin();
            objAddRemove.ShowDialog();

        }

                
    }
    
}

