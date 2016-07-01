using Libs;
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
    public partial class Manage_package_priority : Form
    {
        public Manage_package_priority()
        {
            InitializeComponent();
            string str_sv = NexGenClient.Service.Get_packages_lastest(50);
            var packages = str_sv.XMLStringToListObject<DataObject.PackageImport>();
            if (packages == null)
            {
                return;
            }
            if (packages.Count <= 0)
                return;

            foreach (var item in packages)
            {
                create_records(item);
                x = 10;
                y += 30;
            }
        }
        int x = 10, y = 10;
        public void create_records(DataObject.PackageImport package)
        {
            Panel p1 = new Panel();
            p1.Height = 30;
            p1.AutoSize = true;

            
            Label lbl = new Label();
            lbl.Top = y;
            lbl.Left = x;
            lbl.Text = package.Name;
            lbl.AutoSize = true;
            lbl.Name = "label";
            p1.Controls.Add(lbl);
            x = 300;

            TextBox txtPriority = new TextBox();
            txtPriority.Left = x;
            txtPriority.Top = y;
            txtPriority.Text = package.Priority+string.Empty;
            txtPriority.Name = "priority";
            txtPriority.TextChanged += (sender, e) => {
                var btn = (TextBox)sender;
                btn.BackColor = System.Drawing.Color.OrangeRed;
            };
            txtPriority.Enter += (sender, e) => {
                var btn = (TextBox)sender;
                var allLabel = btn.Parent.Parent.Controls.Find("label", true);
                foreach (var item in allLabel)
                {
                    item.BackColor = System.Drawing.Color.White;
                }
                var focusLabel = (Label)btn.Parent.Controls.Find("label", false)[0];
                focusLabel.BackColor = System.Drawing.Color.OrangeRed;

                foreach (Control itemAll in btn.Parent.Parent.Controls)
                {
                    foreach (Control control in itemAll.Controls)
                    {
                        if (control.GetType() == typeof(Button))
                        {
                            control.Visible = false;
                        }
                    }
                }
                foreach (Control control in btn.Parent.Controls)
                {
                    if (control.GetType() == typeof(Button)) {
                        control.Visible = true;
                    }
                }
                
            };
            p1.Controls.Add(txtPriority);

            x += 200;
            Button btnSave = new Button();
            btnSave.Left = x;
            btnSave.Top = y;
            btnSave.Name = package.Id+string.Empty;
            btnSave.Text = "Save";
            btnSave.Visible = false;
            btnSave.Click += (sender, e) => {
                var btn = (Button)sender;
                var packageid = int.Parse(btn.Name);
                var priorityControl = (TextBox)btn.Parent.Controls.Find("priority", false)[0];
                var priorityValue = int.Parse(priorityControl.Text);
                NexGenClient.Service.Update_package_priority(packageid, priorityValue);
                txtPriority.BackColor = System.Drawing.Color.White;
            };
            p1.Controls.Add(btnSave);

            panel1.Controls.Add(p1);
            
        }
    }
}
