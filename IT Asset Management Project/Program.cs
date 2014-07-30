using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IT_Asset_Management_Project
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AdminForm());
            LoginPage objLog = new LoginPage();
            // objLog.ShowDialog();

            if(objLog.IsAuthenticated)
            {
                switch(objLog.RoleID)
                {    case 1:
   
                    AdminForm objAdmin = new AdminForm();
                        objAdmin.UserName = objLog.UserName.ToString();
                        objAdmin.ShowDialog();
                        
                    break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                }
            MessageBox.Show("Exiting");
                Application.Exit();

        }
    }
    }
}
