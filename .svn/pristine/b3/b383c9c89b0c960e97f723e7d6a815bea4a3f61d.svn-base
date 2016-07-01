using System;
using System.Windows.Forms;

namespace NexGen
{
    public partial class frmMessage : Form
    {
        
        private int startPosX;
        private int startPosY;
        private Timer timer, timer_Close;
        /// <summary>
        /// The Time this Dialog wait before hide
        /// </summary>
        private int timeShow = 3;
        private string str_currentText = String.Empty;
        public frmMessage()
        {
            InitializeComponent();

            startPosX = Screen.PrimaryScreen.WorkingArea.Width - Width;
            startPosY = Screen.PrimaryScreen.WorkingArea.Height;
            SetDesktopLocation(startPosX, startPosY);

            // We want our window to be the top most
            TopMost = true;
            // Pop doesn't need to be shown in task bar
            ShowInTaskbar = false;
            // Create and run timer for animation
            timer = new Timer();
            timer.Interval = 25;
            timer.Tick += timer_Tick;

            timer_Close = new Timer();
            timer_Close.Interval = 3 * 1000;
            timer_Close.Tick += timer_Close_Tick;
        }

        public void Toast(String msg, int TimeShow)
        {
            timeShow = TimeShow;
            str_currentText = msg;
            lblMessage.Text = str_currentText;
            startPosX = Screen.PrimaryScreen.WorkingArea.Width - Width;
            startPosY = Screen.PrimaryScreen.WorkingArea.Height;
            SetDesktopLocation(startPosX, startPosY);

            timer.Stop();
            timer_Close.Stop();
            timer_Close.Interval = timeShow * 1000;
            timer.Start();
        }

        protected override void OnLoad(EventArgs e)
        {
            // Move window out of screen

            base.OnLoad(e);
            // Begin animation
        }

        private void timer_Close_Tick(object sender, EventArgs e)
        {
            timer_Close.Interval = 25;
            //Lift window by 5 pixels
            startPosY += 5;
            //If window is fully visible stop the timer
            if (startPosY > Screen.PrimaryScreen.WorkingArea.Height + Height)
            {
                timer_Close.Stop();
            }
            else
                SetDesktopLocation(startPosX, startPosY);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //Lift window by 5 pixels
            startPosY -= 5;
            //If window is fully visible stop the timer
            if (startPosY < Screen.PrimaryScreen.WorkingArea.Height - Height)
            {
                timer.Stop();
                timer_Close.Start();
            }
            else
                SetDesktopLocation(startPosX, startPosY);
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            Toast(str_currentText, 7);
        }
    }
}