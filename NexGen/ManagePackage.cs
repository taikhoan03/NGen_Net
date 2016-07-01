using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Libs;
using Limilabs.FTP.Client;

namespace NexGen
{
    public partial class ManagePackage : Form
    {
        Dictionary<int, DataObject.PackageImport> old_packages = null;
        BindingList<DataObject.PackageImport> packages;
        public ManagePackage()
        {
            InitializeComponent();
            old_packages = null;
            string str_sv = NexGenClient.Service.Get_packages_lastest(15000);
            
            packages = new BindingList<DataObject.PackageImport>(str_sv.XMLStringToListObject<DataObject.PackageImport>().OrderByDescending(p => p.Id).ToList());
            if (packages == null)
            {
                return;
            }
            old_packages = new Dictionary<int, DataObject.PackageImport>();
            foreach (var package in packages)
            {
                old_packages.Add(package.Id, (DataObject.PackageImport)package.Clone());
            }
            packages.AllowRemove = true;
            if (packages.Count <= 0)
                return;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();
            var column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Id";
            column.HeaderText = "Id";
            column.ReadOnly = true;
            dataGridView1.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Width = 300;
            column.DataPropertyName = "File_path";
            column.HeaderText = "File_path";
            dataGridView1.Columns.Add(column);

            var column_btn_save = new DataGridViewButtonColumn();
            column_btn_save.Name = "Save";
            column_btn_save.Text = "Save";
            column_btn_save.HeaderText = "Save";
            column_btn_save.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(column_btn_save);

            var column_btn_remove = new DataGridViewButtonColumn();
            column_btn_remove.Name = "Delete";
            column_btn_remove.Text = "Delete";
            column_btn_remove.HeaderText = "Delete";
            column_btn_remove.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(column_btn_remove);


            dataGridView1.DataSource = packages;
            dataGridView1.CellContentClick += new DataGridViewCellEventHandler(dataGridView1_CellContentClick);
            //foreach (var item in packages)
            //{
            //    //create_records(item);
            //    //x = 10;
            //    //y += 30;
            //}
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                var package = (DataObject.PackageImport)senderGrid.Rows[e.RowIndex].DataBoundItem;
                //TODO - Button Clicked - Execute Code Here
                if (e.ColumnIndex == senderGrid.Columns["Save"].Index)
                {
                    var confirm = MessageBox.Show("Save??", "Save??", MessageBoxButtons.OKCancel);
                    if (confirm==DialogResult.OK)
                    {
                        var old_package_path = old_packages[package.Id].File_path;
                        // rename on ftp server
                        try
                        {
                            using (Ftp client = new Ftp())
                            {
                                //client.Mode = FtpMode.Active;
                                client.Connect("192.168.101.253");    // or ConnectSSL for SSL
                                client.Login("detoolv2", "UrgRu1");
                                var part_old_path = old_package_path.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                                var part_new_path= package.File_path.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

                                if (part_old_path.Length != part_new_path.Length)
                                    throw new Exception("Path to update may not valid");
                                if(part_old_path[0]!=part_new_path[0])
                                    throw new Exception("First part of this part (20150602) should not editable");


                                client.ChangeFolder("/");
                                var img_folder = "Resource/IMG/";
                                var ocr_folder= "Resource/OCR/";
                                client.ChangeFolder(img_folder);
                                for(var i=0;i<part_old_path.Length;i++)
                                {
                                    if(part_old_path[i]!= part_new_path[i])
                                        client.Rename(part_old_path[i], part_new_path[i]);
                                    client.ChangeFolder(part_new_path[i]);
                                }
                                client.ChangeFolder("/");
                                client.ChangeFolder(ocr_folder);
                                for (var i = 0; i < part_old_path.Length; i++)
                                {
                                    if (part_old_path[i] != part_new_path[i])
                                        client.Rename(part_old_path[i], part_new_path[i]);
                                    client.ChangeFolder(part_new_path[i]);
                                }
                                //client.Rename(ocr_folder + old_package_path, ocr_folder + package.File_path);
                                //client.CreateAllFolders(foldername);
                                ////string currentfolder=client.GetCurrentFolder();
                                //client.ChangeFolder(foldername);
                                //client.Upload(filename, localFilePath);

                                client.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message + "\n " + ex.StackTrace);
                            throw;
                        }

                        NexGenClient.Service.Update_package_file_path(package.Id, package.File_path);
                        MessageBox.Show("Saved");
                    }
                }else if (e.ColumnIndex == senderGrid.Columns["Delete"].Index)
                {
                    var confirm = new YesConfirm(string.Format("You are wanted to delete package '{0}'.\r\nTo make sure you really need to do that, input [YES] into the textbox below",package.Name));
                    confirm.ShowDialog();
                    if (confirm.ConfirmResult)
                    {
                        NexGenClient.Service.remove_package(package.Id);
                        packages.Remove(package);
                        packages.ResetBindings();
                        MessageBox.Show("OK, deleted");
                    }
                }

                //MessageBox.Show(e.RowIndex+"");
            }
        }
    }
}
