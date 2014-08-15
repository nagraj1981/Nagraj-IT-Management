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
using Microsoft.Win32;
using System.IO;
using iTuner;
namespace ClientSideProgram
{
    public partial class CSPMain : Form
    {
        private static readonly string CR = Environment.NewLine;

        private UsbManager manager;
        
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

        public string Last_Usb;
        private void DoStateChanged(UsbStateChangedEventArgs e)
        {
           if(e.State.ToString()=="Inserted")
           {
               Commit_Usb_Data(e.Disk.ToString()+" "+"Inserted");
               MessageBox.Show(e.State + " " + e.Disk.ToString());
               Last_Usb = e.Disk.ToString();

           }
           else
           {
             
               MessageBox.Show(e.State + " " + Last_Usb);
               Last_Usb = " ";
           }
            //MessageBox.Show(e.State + " " + e.Disk.ToString());
        }
        public CSPMain()
        {
            InitializeComponent();
            manager = new UsbManager();
            UsbDiskCollection disks = manager.GetAvailableDisks();

//            textBox.AppendText(CR);
  //          textBox.AppendText("Available USB disks" + CR);

    //        foreach (UsbDisk disk in disks)
      //      {
        //        textBox.AppendText(disk.ToString() + CR);
          //  }

         //   textBox.AppendText(CR);

            manager.StateChanged += new UsbStateChangedEventHandler(DoStateChanged);
            sendtonotify();
            timecount = 0;
            MinutesTimer = 29;
            SecondsTimer = 0;
            Seconds.Start();
                       
            
            CheckConnection();
            label7.Text = "29";
            
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
        public int notifyStatus 
        {set;
            get;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            notifyStatus = 0;    
            this.Hide();
                groupBox2.Enabled = false;
                groupBox3.Enabled = false;
                btnApplyRegistryDisable.Enabled = false;
                btnChangeSettings.Enabled = false;
                notifyIcon2.Visible = true;
                showToolStripMenuItem.Enabled = false;
                txtPasswordField.Focus();
                Reg_getStatus();
                Usb_getStatus();

                if (File.Exists(Program.strPwdFilePath += "\\usbpolicy"))
                {
                    inputPasswordPannel.Visible = true;
                    createPasswordPannel.Visible = false;
                }
                else
                {
                    inputPasswordPannel.Visible = false;
                    createPasswordPannel.Visible = true;
                    groupBox2.Enabled = true;
                    groupBox3.Enabled = true;
                }

                if (notifyStatus == 1)
                {
                    notifyIcon1.Text = "ITAMS - Status : Full Access.";
                }
                else if (notifyStatus == 2)
                {
                    notifyIcon1.Text = "ITAMS - Status : Read Only.";
                }
                else if (notifyStatus == 3)
                {
                    notifyIcon1.Text = "ITAMS - Status : Disabled.";
                }

                label3.Text = "Program Version : " + Application.ProductVersion;
}

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

         
        private void button1_Click(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipText = "IT Asset Managemnt App Minimized To Tray";
            notifyIcon1.ShowBalloonTip(20000);
            notifyIcon1.Visible = true;
            this.Hide();
        }
        public void Commit_Usb_Data(string Device_Name)
        {
           SqlConnection objcon = new SqlConnection("Data Source=DIVYA-PC;Initial Catalog=db_ITAssets;Persist Security Info=True;User ID=sa;password=viperx");
            SqlCommand objCmd = new SqlCommand();
            objCmd.CommandText = "Insert into UsbDetails values(@Machine_ID,@Device_Name,@Time_Stamp)";
            objCmd.Parameters.Add("@Device_Name", SqlDbType.VarChar,50).Value = Device_Name;
            
            objCmd.Parameters.Add("@Machine_ID", SqlDbType.Int).Value = C_Machine_ID;
            objCmd.Parameters.Add("@Time_Stamp", SqlDbType.DateTime).Value = DateTime.Now;




            objCmd.Connection = objcon;
            objCmd.CommandType = CommandType.Text;

            try
            {
                objcon.Open();
        
                objCmd.ExecuteNonQuery();

                objCmd.Dispose();
                objcon.Close();
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
        public string MACAddress
        {
            set;
            get;
        }
        public string C_Machine_ID
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
                }
            objCmd.Parameters.Add("@MACAddres", SqlDbType.VarChar, 50).Value = MACAddress;
            
            objCmd.CommandType = CommandType.Text;
            objCmd.Connection = objcon;
            SqlDataReader objReader;
            CheckConnection();
            ConnectionBallon();
            try
            {
                objcon.Open();
               
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

             
                objCmd.ExecuteNonQuery();

                objCmd.Dispose();
                objcon.Close();
                
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

        private void label7_Click_1(object sender, EventArgs e)
        {

        }


        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            groupBox1.Enabled = true;
            txtPasswordField.Text = "";
            txtPasswordField.Enabled = true;
            btnCheckPassword.Enabled = true;
            btnChangeSettings.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            showToolStripMenuItem.Enabled = false;
            hideToolStripMenuItem.Enabled = true;
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            groupBox1.Enabled = true;
            txtPasswordField.Text = "";
            txtPasswordField.Enabled = true;
            btnCheckPassword.Enabled = true;
            btnChangeSettings.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            showToolStripMenuItem.Enabled = true;
            hideToolStripMenuItem.Enabled = false;
        }

      

        private void btnExitApp_Click(object sender, EventArgs e)
        {
        
        }

        private void CSPMain_Unload(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CSPMain_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(900);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = true;
            }
        }



        private void exitApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnExitApp_Click(null, null);
        }

    

        private void cbDisableRegistry_CheckedChanged(object sender, EventArgs e)
        {
            btnApplyRegistryDisable.Enabled = true;
        }

        private void notifyIcon2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            contextMenuStrip1.Show();
        }

        public void Check_passwordStatus()
        {
            Program.strPwdFilePath += "\\usbpolicy";
            if (File.Exists(Program.strPwdFilePath))
            {
                try
                {
                    StreamReader fsPwdFile =
                        new StreamReader(new FileStream(Program.strPwdFilePath, FileMode.Open, FileAccess.Read));
                    string pwd = fsPwdFile.ReadToEnd();

                    if (String.IsNullOrEmpty(pwd) == false)
                    {
                        //Create_newPassword();
                        Program.isPwdEnabled = true;
                    }
                    else
                        Program.isPwdEnabled = false;
                }
                catch { }

            }
        }

        public void Usb_getStatus()
        {
            RegistryKey key;
            try
            {
                key = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\StorageDevicePolicies");

                if (System.Convert.ToInt16(key.GetValue("WriteProtect", null)) == 1)
                {
                    radiobtnReadonlyAccess.Checked = true;
                    notifyStatus = 2;
                }
                else
                {
                    radioBtnFullAccess.Checked = true;
                    notifyStatus = 1;
                }
            }
            catch (NullReferenceException)
            {
                key = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control", true);
                key.CreateSubKey("StorageDevicePolicies");
                key.Close();
            }
            catch (Exception) { }

            try
            {
                key = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\UsbStor");

                if (System.Convert.ToInt16(key.GetValue("Start", null)) == 4)
                {
                    radioBtnDisabledAccess.Checked = true;
                    notifyStatus = 3;
                    return;
                }
            }
            catch (NullReferenceException)
            {
                key = Registry.LocalMachine.OpenSubKey
                    ("SYSTEM\\CurrentControlSet\\Services\\Usbstor", true);
                key.CreateSubKey("USBSTOR");

                key = Registry.LocalMachine.OpenSubKey
                    ("SYSTEM\\CurrentControlSet\\Services\\UsbStor", true);

                key.SetValue("Type", 1, RegistryValueKind.DWord);
                key.SetValue("Start", 3, RegistryValueKind.DWord);
                key.SetValue("ImagePath", "system32\\drivers\\usbstor.sys", RegistryValueKind.DWord);
                key.SetValue("ErrorControl", 1, RegistryValueKind.DWord);
                key.SetValue("DisplayName", "USB Mass Storage Driver", RegistryValueKind.DWord);
                key.Close();
            }
            catch (Exception) { }
        }

        public void Reg_getStatus()
        {

            RegistryKey key;
            try
            {
                key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
                if (System.Convert.ToInt16(key.GetValue("DisableRegistryTools", null)) == 1)
                    cbDisableRegistry.Checked = true;
                else
                    cbDisableRegistry.Checked = false;
                key.Close();

            }
            catch (NullReferenceException)
            {
                key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies", true);
                key.CreateSubKey("System");
            };
        }

        public void Reg_DisableAccess()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey
                ("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);
            key.SetValue("DisableRegistryTools", 1, RegistryValueKind.DWord);
            key.Close();
        }

        public void Reg_EnableAccess()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey
            ("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);
            key.SetValue("DisableRegistryTools", 0, RegistryValueKind.DWord);
            key.Close();
        }

        public void Usb_disableAllStorageDevices()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey
                ("SYSTEM\\CurrentControlSet\\Services\\UsbStor", true);

            if (key != null)
            {
                key.SetValue("Start", 4, RegistryValueKind.DWord);
            }
            key.Close();
        }

        public void Usb_enabledAllStorageDevices()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey
                ("SYSTEM\\CurrentControlSet\\Services\\UsbStor", true);

            if (key != null)
            {
                key.SetValue("Start", 3, RegistryValueKind.DWord);
            }
            key.Close();
        }

        public void Usb_enableWriteProtectDevices()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey
                ("SYSTEM\\CurrentControlSet\\Control\\StorageDevicePolicies", true);

            if (key == null)
            {
                Registry.LocalMachine.CreateSubKey
                    ("SYSTEM\\CurrentControlSet\\Control\\StorageDevicePolicies", RegistryKeyPermissionCheck.ReadWriteSubTree);
                key = Registry.LocalMachine.OpenSubKey
                    ("SYSTEM\\CurrentControlSet\\Control\\StorageDevicePolicies", true);
                key.SetValue("WriteProtect", 1, RegistryValueKind.DWord);
            }
            else if (key.GetValue("WriteProtect") != (object)(1))
            {
                key.SetValue("WriteProtect", 1, RegistryValueKind.DWord);
            }
        }

        public void Usb_disableWriteProtect()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey
                ("SYSTEM\\CurrentControlSet\\Control\\StorageDevicePolicies", true);
            if (key != null)
                key.SetValue("WriteProtect", 0, RegistryValueKind.DWord);
            key.Close();
        }


        


        private void btnRefresh_Click(object sender, EventArgs e)
        {
          
        }

        private void btnGotoWeb_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.google.co.in");
        }

        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            Usb_getStatus();
            Reg_getStatus();
        }

        private void btnApplySettings_Click_1(object sender, EventArgs e)
        {
            DialogResult resultApplySettings = MessageBox.Show("Apply Settings ?", "Confirm Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultApplySettings == DialogResult.Yes)
            {
                if (radioBtnDisabledAccess.Checked == true)
                {
                    Usb_disableAllStorageDevices();
                    notifyIcon1.Text = "ITAMS - Status : Disabled.";
                    notifyIcon1.BalloonTipText = "Access to the USB External Storage device(s) on this computer is DISABLED.";
                    notifyIcon1.BalloonTipTitle = "ITAMS - Access Denied";
                    notifyIcon1.ShowBalloonTip(1000);
                }
                else if (radiobtnReadonlyAccess.Checked == true)
                {
                    Usb_enabledAllStorageDevices();
                    Usb_enableWriteProtectDevices();
                    notifyIcon1.Text = "ITAMS - Status : Read Only.";
                    notifyIcon1.BalloonTipText = "Access to the USB External Storage device(s) on this computer is READ ONLY.";
                    notifyIcon1.BalloonTipTitle = "ITAMS - Access READ Only";
                    notifyIcon1.ShowBalloonTip(1000);
                }
                else if (radioBtnFullAccess.Checked == true)
                {
                    Usb_enabledAllStorageDevices();
                    Usb_disableWriteProtect();
                    notifyIcon1.Text = "ITAMS - Status : Full Access.";
                    notifyIcon1.BalloonTipText = "Access to the external storage device to this computer is ENABLED.";
                    notifyIcon1.BalloonTipTitle = "ITAMS - Access Granted";
                    notifyIcon1.ShowBalloonTip(1000);
                }
                else
                {
                    Usb_getStatus();
                }

            }
        }

        private void btnCheckPassword_Click_1(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtPasswordField.Text))
            {
                DialogResult emptyPasswordField = MessageBox.Show("No password, Try Again !", "No Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (emptyPasswordField == DialogResult.OK)
                {
                    txtPasswordField.Text = "";
                    txtPasswordField.Focus();
                }
            }
            else
                if (Md5_Hash.ConfirmPwd(txtPasswordField.Text))
                {
                    txtPasswordField.Enabled = false;
                    btnCheckPassword.Enabled = false;
                    btnChangeSettings.Enabled = true;
                    groupBox2.Enabled = true;
                    groupBox3.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Wrong Password, Try Again!", "Wrong Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPasswordField.Focus();
                    return;
                }
        }

        private void toolTip_MainWindow_Popup(object sender, PopupEventArgs e)
        {

        }

        private void btnCreatePasswrd_Click_1(object sender, EventArgs e)
        {
            frmCreatePassword frmChangePassword1 = new frmCreatePassword();
            frmChangePassword1.ShowDialog();
        }

        private void btnChangeSettings_Click_1(object sender, EventArgs e)
        {
            var frmChangePassword = new frmChangePassword();
            frmChangePassword.ShowDialog();
        }

        private void radioBtnFullAccess_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnApplyRegistryDisable_Click_1(object sender, EventArgs e)
        {
            if (cbDisableRegistry.Checked == true)
            {
                DialogResult registryAccessSwitch = MessageBox.Show("You are going to disabled access to Windows Registry, Confirm ?", "Registry Access Disable", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (registryAccessSwitch == DialogResult.Yes)
                {
                    Reg_DisableAccess();
                    btnApplyRegistryDisable.Enabled = false;
                }
                else
                {
                    cbDisableRegistry.Checked = false;
                }
            }
            else if (cbDisableRegistry.Checked == false)
            {
                DialogResult registryAccessSwitch = MessageBox.Show("You are going to enabled access to Windows Registry, Confirm ?", "Registry Access Enable", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (registryAccessSwitch == DialogResult.Yes)
                {
                    Reg_EnableAccess();
                    btnApplyRegistryDisable.Enabled = false;
                }
                else
                {
                    cbDisableRegistry.Checked = true;
                }
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void txtPasswordField_TextChanged_1(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtPasswordField.Text))
            {
                txtPasswordField.Text = "Enter your password";
                txtPasswordField.PasswordChar = Char.MinValue;
            }

            if (txtPasswordField.PasswordChar.Equals(Char.MinValue) && txtPasswordField.Text.Equals("Enter your password").Equals(false))
            {
                txtPasswordField.Text = txtPasswordField.Text[0].ToString();
                txtPasswordField.SelectionStart = 1;
                //txtPasswordField.PasswordChar = "*";

            }
        }

        private void btnHideApp_Click_1(object sender, EventArgs e)
        {
            this.Visible = false;
            groupBox1.Enabled = true;
            txtPasswordField.Text = "";
            txtPasswordField.Enabled = true;
            btnCheckPassword.Enabled = true;
            btnChangeSettings.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            showToolStripMenuItem.Enabled = true;
            hideToolStripMenuItem.Enabled = false;
        }

        private void btnExitApp_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
    

}
