﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NexGen
{
    public partial class FormApp : Form
    {
        public FormApp()
        {
            InitializeComponent();
            this.IsMdiContainer = true;
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void importPackageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new frmImport();
            frm.MdiParent = this;
            frm.Show();
        }

        private void keyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new From_Keyer();
            if (!frm.IsDisposed)
            {
                frm.MdiParent = this;
                //set property record List
                //frm.set_RecordList_MDI(this);
                frm.Show();
            }
        }

        private void FormApp_FormClosed(object sender, FormClosedEventArgs e)
        {
             NexGenService.CommonServiceClient svClient = new NexGenService.CommonServiceClient();
            svClient.UnLock(DataObject.User.Get_authenticated_user().Username);
            Application.Exit();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void FormApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            //NexGenService.CommonServiceClient svClient = new NexGenService.CommonServiceClient();
            //    svClient.UnLock(DataObject.User.Get_authenticated_user().Username);
            //Application.Exit();
            //System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new Export();
            if (!frm.IsDisposed)
            {
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void packagePriorityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new SecureCodeProtector(FunctionProtected.PriorityManager);
            frm.frmToShow = new Manage_package_priority();
            if (!frm.IsDisposed)
            {
                frm.mainForm = this;
                frm.ShowDialog();
            }
            
        }

        private void managePackageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new SecureCodeProtector(FunctionProtected.ManagePackage);
            frm.frmToShow = new ManagePackage();
            if (!frm.IsDisposed)
            {
                frm.mainForm = this;
                frm.ShowDialog();
            }
        }

        private void packageAssignmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new SecureCodeProtector(FunctionProtected.ManagePackage);
            frm.frmToShow = new UserAssignToPackage();
            if (!frm.IsDisposed)
            {
                frm.mainForm = this;
                frm.ShowDialog();
            }
        }
    }
}