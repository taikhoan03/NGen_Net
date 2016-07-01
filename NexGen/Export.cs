﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Libs;

namespace NexGen
{
    public partial class Export : Form
    {
        private List<DataObject.MergedObject.DocumentExport> doc_for_export = new List<DataObject.MergedObject.DocumentExport>();

        public Export()
        {
            InitializeComponent();
            string str_sv = NexGenClient.Service.Get_packages();
            var packages = str_sv.XMLStringToListObject<DataObject.PackageImport>();
            if (packages == null)
            {
                return;
            }
            if (packages.Count <= 0)
                return;
            cboxPackages.DisplayMember = "Name";
            cboxPackages.ValueMember = "Id";
            cboxPackages.DataSource = packages;
            dgExport.AutoGenerateColumns = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var package = (DataObject.PackageImport)cboxPackages.SelectedItem;
            doc_for_export = new List<DataObject.MergedObject.DocumentExport>();

            string str_sv = NexGenClient.Service.search_doc_for_export(package.Id);
            doc_for_export = str_sv.XMLStringToListObject<DataObject.MergedObject.DocumentExport>();
            //var aa = doc_for_export.GroupBy(p => p.Doc_name).Count();
            
            dgExport.DataSource = doc_for_export;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var package = (DataObject.PackageImport)cboxPackages.SelectedItem;
            

            //if(package==null)
            var csv_name = package.CSV_name;
            string name = csv_name.Replace(System.IO.Path.GetExtension(csv_name),string.Empty)+".xlsx";
            string filename = Open_Save_Dialog(name, "Excel");
            if (string.IsNullOrEmpty(filename))
                return;
            // find duplicated docs
            var group_doc_id = doc_for_export.GroupBy(p => p.Receipt_id);
            foreach (var group_item in group_doc_id)
            {
                var group_username = group_item.GroupBy(p => p.Worker_id);
                if (group_username.Count() > 1)
                {
                    MessageBox.Show("found duplicates");
                    var group_username_exclude_first = group_username.Skip(1);
                    foreach (var item_to_remove in group_username_exclude_first)
                    {
                        doc_for_export.RemoveAll(p => p.Receipt_id == group_item.Key && p.Worker_id == item_to_remove.Key);
                    }
                }
            }
            //cleanup
            foreach (var rec in doc_for_export)
            {
                if(rec.Sr_no!=1)
                {
                    rec.Transaction_type = null;
                    rec.Receipt_id = null;
                    rec.Banner_id = null;
                    rec.Total = -1234599878.09998;
                    rec.Transaction_date = null;
                    rec.Transaction_time = null;
                    
                    rec.Image_1 = null;
                    rec.Image_2 = null;
                    rec.Image_3 = null;
                    rec.Image_4 = null;
                    rec.Image_5 = null;
                    rec.Image_6 = null;
                    rec.Image_7 = null;
                    rec.Image_8 = null;
                    rec.Image_9 = null;

                    rec.Rejection_reason = null;
                }else
                {
                    if (rec.Transaction_type == DataObject.EVRData.Transaction_type_RIN_RSD)
                        rec.Transaction_type = "RIN/RSD";
                    else if (rec.Transaction_type == DataObject.EVRData.Transaction_type_UPC_RSD)
                        rec.Transaction_type = "UPC/RSD";
                    rec.Image_1 = rec.Doc_name.Replace(System.IO.Path.GetExtension(rec.Doc_name), string.Empty).Replace(".", string.Empty);
                }
                //rec.Item_type=rec.
                rec.Worker_id = null;
                rec.Rin = rec.Item_number;
                //rec.item
                
            }
            //var docs_group = doc_for_export.GroupBy(p => p.Doc_name);
            //foreach (var docid in docs_group)
            //{
            //    var sub_list_record = doc_for_export.Where(p => p.Id == docid.Key);
            //    var first_record = sub_list_record.Where(p => p.Sr_no == 1).FirstOrDefault();
            //    //if(first_record!=null)
            //    //{
            //    //    first_record.Total = sub_list_record.Sum(p => p.Price);
            //    //}
            //}
            //doc_for_export = doc_for_export.OrderBy(p => p.Image_1).ThenBy(p => p.Sr_no).ToList();
            DataObject.Document.ToExcel<DataObject.MergedObject.DocumentExport>(filename, doc_for_export);
        }
        private string Open_Save_Dialog(string filename, string fileType)
        {
            // Prompt the user to enter a path/filename to save an example Excel file to
            //saveFileDialog1.FileName = "Exported.xlsx";
            saveFileDialog1.FileName = filename;
            if (fileType.ToLower() == "excel")
                saveFileDialog1.Filter = "Excel 2007 files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            else if (fileType.ToLower() == "zip")
                saveFileDialog1.Filter = "Zip files (*.zip)|*.zip|All files (*.*)|*.*";
            else if (fileType.ToLower() == "rpt")
                saveFileDialog1.Filter = "Text files (*.RPT)|*.RPT";
            else if (fileType.ToLower() == "txt")
                saveFileDialog1.Filter = "Text files (*.txt)|*.txt";
            else
                return null;
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.OverwritePrompt = false;
            
            //  If the user hit Cancel, then abort!
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return null;
            
            return saveFileDialog1.FileName;
        }
    }
}