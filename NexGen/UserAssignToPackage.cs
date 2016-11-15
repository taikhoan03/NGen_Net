using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataObject;
using Libs;
namespace NexGen
{
    public partial class UserAssignToPackage : Form
    {
        List<User> Users { get; set; }
        List<PackageImport> Packages { get; set; }
        BindingList<PackageAssignment> ResultList = new BindingList<PackageAssignment>();
        NexGenService.CommonServiceClient svCommon = new NexGenService.CommonServiceClient();
        svDownloadFile.IsvManage_Download_FileClient svPackage = new svDownloadFile.IsvManage_Download_FileClient();

        public UserAssignToPackage()
        {
            InitializeComponent();
            initData();
        }
        private void initData (){
            
            Users = svCommon.getListUser().XMLStringToListObject<User>();
            Users.Insert(0, new User() { });
            Packages = svPackage.getPackages().XMLStringToListObject<PackageImport>();
            Packages = Packages.OrderByDescending(c => c.Create_date).ToList();
            Packages.Insert(0, new PackageImport() { Id=0});

            cboxUsers.DisplayMember = "Username";
            cboxUsers.ValueMember = "Username";
            cboxUsers.DataSource = Users;

            cboxPackage.DisplayMember = "Name";
            cboxPackage.ValueMember = "Id";
            cboxPackage.DataSource = Packages;
            cboxPackage.SelectedIndex = 0;

            var assigned_packages = svPackage.Get_Assigned_Packages(null, 0).XMLStringToListObject<PackageAssignment>() ;
            ResultList = new BindingList<PackageAssignment>(assigned_packages);
            dataGridView1.DataSource = ResultList;
            if (dataGridView1.Rows.Count > 0)
            {
                //dataGridView1.DataSource = dt;
                //dataGridViewProjects.Columns["Title"].Width = 300;
                dataGridView1.Columns["created_date"].Visible = false;
                dataGridView1.Columns["id"].ReadOnly=true;
            }

            cboxUsers.SelectedIndexChanged += new EventHandler(Search);
            cboxPackage.SelectedIndexChanged += new EventHandler(Search);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboxUsers.SelectedValue.ToString()) || string.IsNullOrEmpty(cboxPackage.SelectedValue.ToString()))
                return;
            int packageid = 0;
            var item = cboxPackage.SelectedItem;
            if (item != null)
                int.TryParse(cboxPackage.SelectedValue.ToString(), out packageid);
            string username = cboxUsers.SelectedValue.ToString();

            var filter = ResultList.ToList();
            if(!string.IsNullOrEmpty(username))
                filter=filter.Where(c => c.username == username).ToList();
            //if(packageid>0)
            //    filter = filter.Where(c => c.packageid == packageid).ToList();
            int priority = 0;
            if(filter.Count()>0)
                priority = filter.Max(c => c.priority) + 1;

            var new_item = new PackageAssignment();
            new_item.is_enabled = true;
            new_item.created_date = DateTime.Now;
            new_item.packageid = packageid;
            new_item.priority = priority;
            new_item.username = username;

            //add
            var id = svPackage.addAssignPackage(new_item.XmlSerialize());
            new_item.id = id;
            ResultList.Insert(0, new_item);
            search();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            
            var item = (PackageAssignment)dataGridView1.SelectedRows[0].DataBoundItem;
            svPackage.removeAssignPackage(int.Parse(item.id.ToString()));
            ResultList.Remove(item);
            search();
        }

        private void cboxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboxPackage_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void Search(object sender, EventArgs e)
        {
            search();
        }
        private void search()
        {
            int packageid = 0;
            var Tmp = ResultList;
            var item = cboxPackage.SelectedItem;
            if (item != null)
            {
                int.TryParse(cboxPackage.SelectedValue.ToString(), out packageid);

            }
            if (packageid > 0)
            {
                Tmp = new BindingList<PackageAssignment>(Tmp.Where(c => c.packageid == packageid).ToList());
            }

            var user = cboxUsers.SelectedItem;
            var username = string.Empty;
            if (user != null)
            {
                if (!string.IsNullOrEmpty(((DataObject.User)user).Username))
                {
                    username = cboxUsers.SelectedValue.ToString();
                    Tmp = new BindingList<PackageAssignment>(Tmp.Where(c => c.username == username).ToList());
                }

            }
            var assigned_packages = svPackage.Get_Assigned_Packages(username, packageid).XMLStringToListObject<PackageAssignment>();

            //var Tmp = ResultList.Where(c => c.username == username && c.packageid == packageid);
            dataGridView1.DataSource = Tmp;
        }
        PackageAssignment itemEdit;
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            
            
            svPackage.updateAssignPackage(itemEdit.XmlSerialize());
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            itemEdit = (PackageAssignment)dataGridView1.SelectedRows[0].DataBoundItem;
        }

        private void cboxPackage_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            int packageid = 0;
            var item = cboxPackage.SelectedItem;
            if (item != null)
            {
                int.TryParse(cboxPackage.SelectedValue.ToString(), out packageid);

            }
            if (packageid > 0)
            {
                lblPackageid.Text = cboxPackage.SelectedValue.ToString();
            }
            
        }
    }
}
