using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NexGen
{
    public partial class YesConfirm : Form
    {
        public bool ConfirmResult = false;
        public YesConfirm(string text)
        {
            InitializeComponent();
            label1.Text = text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "YES")
            {
                ConfirmResult = true;
                this.Close();
                this.Dispose();
            }
        }
    }
}
