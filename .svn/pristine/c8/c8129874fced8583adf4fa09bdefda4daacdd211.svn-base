using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Services.Client;
using Libs;
namespace NexGen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string str_sv=NexGenClient.Service.Login("1", "1");
            var user = str_sv.XMLStringToObject<DataObject.User>();
            //this.Close();
            
        }
    }
}
