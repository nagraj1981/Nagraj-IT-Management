using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;

using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Management;
using System.Management.Instrumentation;

namespace ClientSideProgram
{
    public partial class CSPMain : Form
    {
        
        public bool IsConnectedUser
        {
            set;
            get;
        }
        public bool IsDomain
        {
            set;
            get;
        }
        
         public int timecount
         {
             set;
             get;
         }
         public int MinutesTimer
         {
             set;
             get;
         }
         public int SecondsTimer
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
                objcon.Close();
 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                
            }
            
        }
        
    
        public CSPMain()
        {
            timecount = 0;
            MinutesTimer = 29;
            SecondsTimer = 0;
            label7.Text ="29";
            Seconds.Start();
            sendtonotify();
            CheckConnection();
            ConnectionBallon();
            if (IsConnectedITAssets == true && IsConnectedUser == true)
            {
                RefreshData();
                        }
        }
        public void ConnectionBallon()
        {
            if (IsConnectedITAssets == true && IsConnectedUser == true)
            {
                notifyIcon1.BalloonTipText = "Connection Established - Ready To Upload Data";
                notifyIcon1.ShowBalloonTip(6000);
              
            }
            else
            {
                notifyIcon1.BalloonTipText = "No Connection Check Lan Connection";
                notifyIcon1.ShowBalloonTip(6000);

            }

        }
        public void sendtonotify()
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                // notifyIcon1.ShowBalloonTip(50);
                this.Hide();

            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.Focus();
        }

         
        private void button1_Click(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipText = "IT Asset Managemnt App Minimized To Tray";
            notifyIcon1.ShowBalloonTip(20000);
            notifyIcon1.Visible = true;
            this.Hide();
        }
        public string MACAddress
        {
            set;
            get;
        }
        //Refresh Data Class
        public void RefreshData()
        {


            SqlConnection objcon = new SqlConnection("Data Source=DIVYA-PC;Initial Catalog=db_ITAssets;Persist Security Info=True;User ID=sa;password=viperx");
            SqlCommand objCmd = new SqlCommand("Select Machine_ID,MACAddress from Current_Config_Data where MACAddress=@MACAddres");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapterConfiguration");

            foreach (ManagementObject queryObj in searcher.Get())
                if (bool.Parse(queryObj["IPEnabled"].ToString()) == true)
                {
                    MACAddress = (queryObj["MACAddress"].ToString());
                    objCmd.Parameters.Add("@MACAddres", SqlDbType.VarChar, 50).Value = (queryObj["MACAddress"].ToString());
                }
                    objCmd.CommandType = CommandType.Text;
            objCmd.Connection = objcon;
            SqlDataReader objReader;
            CheckConnection();
            ConnectionBallon();
            try
            {
                objcon.Open();
                string C_Machine_ID;
                objReader = objCmd.ExecuteReader();

                if (objReader.Read())
                {
                    if (objReader[1].ToString()==MACAddress)
                    {
                        C_Machine_ID = objReader[0].ToString();
                        WriteCurrentConfig(C_Machine_ID);
                    }

                }
                else
                {
                    MessageBox.Show("Machine ID Does Not Exist .. Contact Administrator");
                    // WriteNewConfig(MACAddress);
                    Add_New_Machine FormAddNew = new Add_New_Machine();
                    FormAddNew.Show();

                }

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
        
    public void WriteCurrentConfig(string MachinID)
        {
            SqlConnection objcon = new SqlConnection("Data Source=DIVYA-PC;Initial Catalog=db_ITAssets;Persist Security Info=True;User ID=sa;password=viperx");
            SqlCommand objCmd = new SqlCommand();
            objCmd.CommandText = "UPDATE Current_Config_Data SET Computer_Name=@ComputerName,Model=@Model,Bios_Version=@Bios,Physical_RAM=@PhyRAM,Domain=@Domain,Workgroup=@Workgroup,Processor=@Processor,Core=@Core,Logical_Cores=@Logical_Cores,Max_Speed=@MaxSpeed,OS_Serial_No=@OSSerial,OS_Name=@OSName,OS_Version=@OSVer,OS_Install_Date=@OSInstallDate,OS_Activation_Status=@OSActivationStatus,MACAddress=@MACAddress,Last_Refresh=@lastrefresh Where Machine_ID=@MAC";
            objCmd.Parameters.Add("@MAC", SqlDbType.VarChar, 50).Value = MachinID;
            objCmd.Parameters.Add("@lastrefresh", SqlDbType.DateTime).Value = DateTime.Now;  
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapterConfiguration");

            foreach (ManagementObject queryObj in searcher.Get())
                if (bool.Parse(queryObj["IPEnabled"].ToString()) == true)
                // int.Parse(queryObj["IPConnectionMetric"].ToString())>1 ) // 
                {

                    objCmd.Parameters.Add("@MACAddress", SqlDbType.VarChar, 50).Value = (queryObj["MACAddress"].ToString());
                }


            // Processor Info
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");

            foreach (ManagementObject queryObj in searcher.Get())
            {

                objCmd.Parameters.Add("@Processor", SqlDbType.VarChar, 50).Value = queryObj["Name"].ToString();
                objCmd.Parameters.Add("@Core", SqlDbType.Int).Value = int.Parse(queryObj["NumberOfCores"].ToString());
                objCmd.Parameters.Add("@Logical_Cores", SqlDbType.Int).Value = int.Parse(queryObj["NumberOfLogicalProcessors"].ToString());
                objCmd.Parameters.Add("@MaxSpeed", SqlDbType.VarChar, 50).Value = (double.Parse(queryObj["MaxClockSpeed"].ToString()) / 1000).ToString() + " GHz";

            }
            // Account Info
            /*   
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_UserAccount");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    listBox4.Items.Add(queryObj["Name"].ToString());
                    listBox5.Items.Add(queryObj["Status"].ToString());
                    listBox6.Items.Add(queryObj["PasswordRequired"].ToString());

                }*/

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
                    objCmd.Parameters.Add("@OSActivationStatus", SqlDbType.VarChar, 50).Value = "Activated";
                else
                    objCmd.Parameters.Add("@OSActivationStatus", SqlDbType.VarChar, 50).Value = "Not Activated";
            }


            // ComputerSystem
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_ComputerSystem");

            foreach (ManagementObject queryObj in searcher.Get())
            {


                objCmd.Parameters.Add("@ComputerName", SqlDbType.VarChar, 50).Value = queryObj["Name"].ToString();
                objCmd.Parameters.Add("@Model", SqlDbType.VarChar, 50).Value = queryObj["Model"].ToString();
                objCmd.Parameters.Add("@Bios", SqlDbType.VarChar, 50).Value = "sdfsf";
                objCmd.Parameters.Add("@PhyRAM", SqlDbType.VarChar, 50).Value = queryObj["TotalPhysicalMemory"].ToString();

                IsDomain = bool.Parse(queryObj["PartOfDomain"].ToString());
                if (!IsDomain)
                {
                    objCmd.Parameters.Add("@Domain", SqlDbType.VarChar, 50).Value = "Not Part Of Domain";
                    objCmd.Parameters.Add("@Workgroup", SqlDbType.VarChar, 50).Value = queryObj["Workgroup"].ToString();

                }
                else
                {
                    objCmd.Parameters.Add("@Domain", SqlDbType.VarChar, 50).Value = queryObj["Domain"].ToString();
                    objCmd.Parameters.Add("@Workgroup", SqlDbType.VarChar, 50).Value = "Not Part Of Workgroup";

                }
            }

            // Operating System
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");


            foreach (ManagementObject queryObj in searcher.Get())
            {

                objCmd.Parameters.Add("@OSSerial", SqlDbType.VarChar, 50).Value = queryObj["SerialNumber"].ToString();
                objCmd.Parameters.Add("@OSName", SqlDbType.VarChar, 50).Value = queryObj["Caption"].ToString();
                objCmd.Parameters.Add("@OSVer", SqlDbType.VarChar, 50).Value = queryObj["Version"].ToString();
                objCmd.Parameters.Add("@OSInstallDate", SqlDbType.VarChar, 50).Value = queryObj["InstallDate"].ToString();

            }
         
          
            
            objCmd.Connection = objcon;
            objCmd.CommandType = CommandType.Text;

            try
            {
                objcon.Open();
                // add check for Open Connection

                MessageBox.Show(objcon.State.ToString());
                objCmd.ExecuteNonQuery();

                objCmd.Dispose();
                objcon.Close();
                MessageBox.Show("Created Successfully");
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
        /*
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
                IsPhysical = bool.Parse(queryObj["PhysicalAdapter"].ToString());

                if (IsPhysical)
                {
                    int NetConStat = int.Parse(queryObj["NetConnectionStatus"].ToString());

                    if (NetConStat == 7 | NetConStat == 2)
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
        


            // CDROM Drives
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_CDROMDrive");

            foreach (ManagementObject queryObj in searcher.Get())
            {

                listBox3.Items.Add(queryObj["Caption"].ToString() + "   Drive : " + queryObj["Drive"].ToString());

            }

        
        */
            
        }
     

        private void Seconds_Tick(object sender, EventArgs e)
        {
            label8.Text = (60 - SecondsTimer).ToString();
            SecondsTimer++;
            if (SecondsTimer == 60)
            {
                SecondsTimer = 0;
                MinutesTimer--;
                label7.Text = (MinutesTimer).ToString();
            }
            if (MinutesTimer == 0)
            {
                if (IsConnectedITAssets == true && IsConnectedUser == true)
                {
                    notifyIcon1.BalloonTipText = "Connection Established - Refreshing Data";
                    notifyIcon1.ShowBalloonTip(20000);
                    RefreshData();
                }
                else
                {
                    notifyIcon1.BalloonTipText = "No Connection - Pl Ensure Network Connectivity";
                    notifyIcon1.ShowBalloonTip(20000);

                } 
                MinutesTimer = 29;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            RefreshData();
            notifyIcon1.BalloonTipText = "Refreshed Data";
            notifyIcon1.ShowBalloonTip(20000);
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

    }
    

}
