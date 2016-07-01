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
    public partial class SecureCodeProtector : Form
    {
        private string _code = "FJDSKFLDJ@!32";
        public Form mainForm { get; set; }
        public SecureCodeProtector(FunctionProtected functionProtected)
        {
            InitializeComponent();
            Init(functionProtected);
        }
        public void Init(FunctionProtected functionProtected)
        {
            Form frm=null;
            switch (functionProtected)
            {
                case FunctionProtected.PriorityManager:
                    frm = new Manage_package_priority();
                    break;
                case FunctionProtected.ManagePackage:
                    frm = new ManagePackage();
                    break;
            }
            textBox1.TextChanged += (sender, e) => {
                var code = NexGenClient.Service.Get_secure_code(_code);
                if (code != 0)
                {
                    if (textBox1.Text == code + "")
                    {
                        this.Close();
                        //var frm = new Manage_package_priority();
                        frm.MdiParent = mainForm;
                        frm.Show();
                    }
                }
                
            };
        }
    }
    public enum FunctionProtected
    {
        PriorityManager=1,
        ManagePackage=2
    }
}
