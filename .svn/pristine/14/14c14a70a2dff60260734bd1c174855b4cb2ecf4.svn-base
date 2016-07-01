using System;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using NexGen;
using log4net;
using DataObject;
using DataObject.Util;
using Libs;
namespace NexGen
{
    public partial class frmLogin : Form
    {
        private Thread loginThread = null;
       // private static Object lockObject = new Object();
        public static readonly ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ApplicationCacheData AppCache = ApplicationCacheData.Init();
        public frmLogin()
        {
            InitializeComponent();
            this.Text = "NexGen Client DEMO - Build version: " + System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToShortDateString();
            customInit();
            //this.OnKeyUp+=new EventHandler(checkCaplock);
            lblLoginMessageResult.Text = frmLogin.IsKeyLocked(Keys.CapsLock) ? "Notice: CAPLOCK is ON." : String.Empty;

            //load last username login
            //added by cuong.ha 2014-06-14
            if (AppCache != null && !string.IsNullOrEmpty(AppCache.UsernameLogin))
            {
                txtUser.Text = AppCache.UsernameLogin;
                txtPass.Focus();
                txtPass.Select();
            }
            
        }


        private void customInit()
        {
            lblValiddateUsername.Text = string.Empty;
            lblValidatePassword.Text = string.Empty;
            lblLoginMessageResult.Text = string.Empty;
            txtUser.Select();
            txtUser.KeyUp += new KeyEventHandler(eventLogin);
            txtPass.KeyUp += new KeyEventHandler(eventLogin);
            //btnLogin.Click += (sender, e) => { submitLogin(); };
        }



        private void eventLogin(object sender, KeyEventArgs e)
        {
            lblLoginMessageResult.Text = frmLogin.IsKeyLocked(Keys.CapsLock) ? "Notice: CAPLOCK is ON." : String.Empty;

            if (e.KeyCode.Equals(Keys.Enter))
                submitLogin();
        }

        private void submitLogin()
        {
            if (!validate()) return;
            lblLoginMessageResult.Text = "Login Authenticating, please wait...";
            submitToWebservice();
            //login
            //this.loginThread = new Thread(new ThreadStart(_loginThread));
            //loginThread.Start();
        }

        //private void _loginThread()
        //{
        //    LoginDelegate loginDelegate = new LoginDelegate(submitToWebservice);
        //    this.Invoke(loginDelegate);
        //}
        /// <summary>
        /// Finds the MAC address of the NIC with maximum speed.
        /// </summary>
        /// <returns>The MAC address.</returns>
        //private string GetMacAddress()
        //{
        //    const int MIN_MAC_ADDR_LENGTH = 12;
        //    string macAddress = string.Empty;
        //    long maxSpeed = -1;

        //    foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
        //    {
        //        //logger.Error(
        //        //  "Found MAC Address: " + nic.GetPhysicalAddress() +
        //        //" Type: " + nic.NetworkInterfaceType);

        //        string tempMac = nic.GetPhysicalAddress().ToString();
        //        if (nic.Speed > maxSpeed &&
        //            !string.IsNullOrEmpty(tempMac) &&
        //            tempMac.Length >= MIN_MAC_ADDR_LENGTH)
        //        {
        //            //logger.Error("New Max Speed = " + nic.Speed + ", MAC: " + tempMac);
        //            maxSpeed = nic.Speed;
        //            macAddress = tempMac;
        //        }
        //    }

        //    return macAddress;
        //}

        public string GetIP()   //get IP address
        {
            IPHostEntry ipHost = Dns.Resolve(Dns.GetHostName());
            var s = ipHost.AddressList[0].ToString();
            return s.ToString();
        }

        private void submitToWebservice()
        {
            //lock (lockObject)
            //{
                try
                {
                    //string strMACAddress = GetMacAddress();

                    //string strIP = GetIP();

                    string str_sv_login = NexGenClient.Service.Login(txtUser.Text, txtPass.Text);
                    var auUser = str_sv_login.XMLStringToObject<User>();
                logger.Info(auUser);
                    if (auUser == null)
                    {
                        this.lblLoginMessageResult.Text = "Login Fail";
                        return;
                    }
                    User.Set_authenticated_user(auUser);
                    
                    AppCache.set("UsernameLogin", txtUser.Text);
                    var nextFrom = new FormApp();
                    nextFrom.Show();
                    this.Hide();
                    
                }
                catch (Exception ex)
                {
                   // MessageBox.Show(ex.ToString());
                    this.lblLoginMessageResult.Text = "ERROR: " + ex.Message;
                }
            //}
        }

        public delegate void LoginDelegate();

        private bool validate()
        {
            bool isvalid = !txtUser.Text.Equals(string.Empty);
            lblValiddateUsername.Text = isvalid ? String.Empty : "Please enter Username";

            isvalid = !txtPass.Text.Equals(String.Empty);
            lblValidatePassword.Text = isvalid ? String.Empty : "Please enter Password";

            return isvalid;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            
            Application.ExitThread();
            
            Application.Exit();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            submitLogin();
        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.ExitThread();
            
            Application.Exit();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}