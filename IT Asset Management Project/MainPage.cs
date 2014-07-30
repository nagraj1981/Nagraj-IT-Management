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


namespace IT_Asset_Management_Project
{
    public partial class MainPage : Form
    {
        public void CurrentConfig()
        {
            ManagementObjectSearcher searcher =new ManagementObjectSearcher("root\\CIMV2","SELECT * FROM Win32_Processor");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                label1.Text =queryObj["Name"].ToString();
            }
        }
        public MainPage()
        {
            InitializeComponent();
        }

        private void MainPage_Load(object sender, EventArgs e)
        {
            CurrentConfig();
        }
    }
}
