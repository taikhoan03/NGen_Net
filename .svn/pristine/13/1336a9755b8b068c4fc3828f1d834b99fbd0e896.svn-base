using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NexGen.Common
{
    public class ToastMessage
    {
        private static frmMessage msg = new frmMessage();
        public static void Show(string message, int TimeShow = 4)
        {
            if (msg.InvokeRequired)
            {
                msg.Invoke((Action)delegate
                {
                    msg.Show();
                    msg.Toast(message, TimeShow);
                });
            }
            else
            {
                msg.Show();
                msg.Toast(message, TimeShow);
            }
        }
    }
}
