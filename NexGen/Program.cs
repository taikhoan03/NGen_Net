using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DataObject.Util;
namespace NexGen
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
            //Application.Run(new Form1());
            //System.Text.RegularExpressions.Regex regex_character_remove = new System.Text.RegularExpressions.Regex("[a-zA-Z]");
            //string a = regex_character_remove.Replace("1.50F", string.Empty);
            //Application.ApplicationExit += new EventHandler(unlock_doc);
            //AppDomain.CurrentDomain.ProcessExit += new EventHandler(unlock_doc);
            //Regex number = new Regex("^[^0]*$");
            //var a=Regex.Replace("1 .23 fds fd", "[^0-9\\.]", string.Empty);
            //var a = " hello work   ".TrimAllWhiteSpace(); 
            // a = Regex.Replace(a, @"\s+", " ");
            var a = " hello work   ".TrimAllWhiteSpace();
            Application.ApplicationExit += (sender, e) =>
            {
                NexGenService.CommonServiceClient svClient = new NexGenService.CommonServiceClient();
                svClient.UnLock(DataObject.User.Get_authenticated_user().Username);
            };
            AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
            {
                NexGenService.CommonServiceClient svClient = new NexGenService.CommonServiceClient();
                svClient.UnLock(DataObject.User.Get_authenticated_user().Username);
            };
            //Regex check_format_transaction_time = new Regex("^[0-9]+[:]{1}[0-9]+(:[0-9]+)?(\\s(AM|PM))?$");
            //var match = check_format_transaction_time.IsMatch("1324");
            //var b= Regex.Replace("123A123BC", @"[^\d]", "");
            log4net.Config.XmlConfigurator.Configure(); 
                //Application.Run(new UserAssignToPackage());
            Application.Run(new frmLogin());
            //Application.Run(new SecureCodeProtector(FunctionProtected.PriorityManager));
            //Application.Run(new frmLogin());
            //Application.Run(new From_Keyer());
        }
        //private static void unlock_doc(object sender,EventArgs e)
        //{
        //    NexGenService.CommonServiceClient svClient = new NexGenService.CommonServiceClient();
        //        svClient.UnLock(DataObject.User.Get_authenticated_user().Username);
        //}
    }
}