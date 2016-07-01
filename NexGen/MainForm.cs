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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            From_Keyer frm = new From_Keyer();
            frm.Show();
        }
    }
}
