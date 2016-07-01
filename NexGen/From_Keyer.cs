using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Libs;
using NexGen.Common;
using DataObject.Util;

namespace NexGen
{
    public partial class From_Keyer : Form, IMessageFilter
    {
        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static RecordList _list_form = new RecordList();
        private DataObject.User user = DataObject.User.Get_authenticated_user();
        NexGen.Common.OCRDocument DocORC = new NexGen.Common.OCRDocument();//old style
        private DataObject.Document _doc;
        private string _doc_image_path;
        private System.Xml.XmlDocument _ocr_data;
        private DataObject.OCR.CoordinateList OCRData;
        private int old_width,old_height;
        //private records=
        private Image _img,_img2;
        private bool addRuler;
        private System.Diagnostics.Stopwatch watch;// = Stopwatch.StartNew();
        private Timer timer;
        private string[]imgs;
        private List<Control> buttonImgs;
        public DataObject.DocCounter Counter;
        private int btnImageIndex = 0;

        public void set_RecordList_MDI(Form mdiparent)
        {
            _list_form.MdiParent = mdiparent;
        }

        public From_Keyer()
        {
            InitializeComponent();
            addRuler = chkAddRuler.Checked;
            lblMessage.Text = string.Empty;
            this.KeyPreview = true;
            cboxTransactionType.Populate();
            
            _list_form.caller = this;
            
            //this.MdiChildren = _list_form;
            _list_form.populate();
            cboxRemarks.Populate();
            if (_list_form != null && !_list_form.IsDisposed)
            {
                _list_form.set_data(new List<DataObject.Record_base>());
                _list_form.bind_grid();
            }
            pboxImage.MouseWheel += new MouseEventHandler(panel1_MouseWheel);
            panel1.MouseWheel += new MouseEventHandler(panel1_MouseWheel);
            //set pbox event
            pboxImage.MouseDown += new MouseEventHandler(pboxImage_MouseDown);
            pboxImage.MouseMove += new MouseEventHandler(pboxImage_MouseMove);
            pboxImage.MouseUp += new MouseEventHandler(pboxImage_MouseUp);
            pboxImage.Paint += new PaintEventHandler(pboxImage_Paint);
            
            //update record function selected from ListRecord form
            txtTransactionTime.CharacterCasing = CharacterCasing.Upper;
            _list_form.mnuUpdate.Click += new EventHandler(grid_update_menu_click);
            txtTransactionTime.Enter += (sender,e) =>
            {
                focus_control = (Control)sender;
            };
            txtTransactionTime.Leave += (sender,e) =>
            {
                focus_control = null;
                try
                {
                    string time_input = txtTransactionTime.Text;
                    //input: 1234 -> 12:34
                    if (time_input.Trim().Length == 4 && time_input.IndexOf(":") < 0)
                    {
                        int hour = int.Parse(time_input.Substring(0, 2));
                        int minutes = int.Parse(time_input.Substring(2, 2));
                        //int seconds = int.Parse(time_input.Substring(4, 2));
                        TimeSpan ts = new TimeSpan(hour,minutes,0);
                        txtTransactionTime.Text = string.Format("{0:hh\\:mm}", ts);
                    }
                    //input: 1234a -> 12:34 AM
                    //input: 1234p -> 12:34 PM
                    if (time_input.Trim().Length == 5 && time_input.IndexOf(":") < 0)
                    {
                        int hour = int.Parse(time_input.Substring(0, 2));
                        int minutes = int.Parse(time_input.Substring(2, 2));
                        var am_or_pm = time_input.Substring(4, 1);
                        TimeSpan ts = new TimeSpan(hour,minutes,0);
                        if (am_or_pm.ToLower() == "a")
                        {
                            txtTransactionTime.Text = string.Format("{0:hh\\:mm}", ts) + " AM";
                        }
                        else if (am_or_pm.ToLower() == "p")
                        {
                            txtTransactionTime.Text = string.Format("{0:hh\\:mm}", ts) + " PM";
                        }
                    }
                    //input: 123456 -> 12:34:56
                    if (time_input.Trim().Length == 6 && time_input.IndexOf(":") < 0)
                    {
                        int hour = int.Parse(time_input.Substring(0, 2));
                        int minutes = int.Parse(time_input.Substring(2, 2));
                        int seconds = int.Parse(time_input.Substring(4, 2));
                        TimeSpan ts = new TimeSpan(hour,minutes,seconds);
                        txtTransactionTime.Text = string.Format("{0:hh\\:mm\\:ss}", ts);
                    }
                    //input: 123456a -> 12:34:56 AM
                    //input: 123456p -> 12:34:56 PM
                    if (time_input.Trim().Length == 7 && time_input.IndexOf(":") < 0)
                    {
                        int hour = int.Parse(time_input.Substring(0, 2));
                        int minutes = int.Parse(time_input.Substring(2, 2));
                        int seconds = int.Parse(time_input.Substring(4, 2));
                        TimeSpan ts = new TimeSpan(hour,minutes,seconds);
                        var am_or_pm = time_input.Substring(6, 1);
                        if (am_or_pm.ToLower() == "a")
                        {
                            txtTransactionTime.Text = string.Format("{0:hh\\:mm\\:ss}", ts) + " AM";
                        }
                        else if (am_or_pm.ToLower() == "p")
                        {
                            txtTransactionTime.Text = string.Format("{0:hh\\:mm\\:ss}", ts) + " PM";
                        }
                    }

                    //VALIDATE
                    txtTransactionTime.Text = txtTransactionTime.Text.Trim();
                    txtTransactionTime.BackColor = Color.White;
                    if (!check_format_transaction_time.IsMatch(txtTransactionTime.Text))
                        txtTransactionTime.BackColor = Color.SandyBrown;
                }
                catch (Exception)
                {
                }
            };
            txtTransactionTime.Focus();
            Counter = new DataObject.DocCounter();
            Counter.Load();
            Update_counter_label();
            LoadDoc();
            //set width recordList
            //Form myForm;
            //_list_form.Width = 1900 - this.Width;
            //_list_form.Height = 800;
        }

        public void Update_counter_label(int num_of_rec_virtual=0)
        {
            var list_records = _list_form.get_data<DataObject.Record_base>();
            //update Counter
            if (num_of_rec_virtual > 0)
            {
                lblCounter.Text = string.Format("Docs: [{0}], Records: [{1}], Discard: [{2}]", Counter.Num_of_doc, num_of_rec_virtual, Counter.Num_of_doc_discard);
            }
            else
                lblCounter.Text = string.Format("Docs: [{0}], Records: [{1}], Discard: [{2}]", Counter.Num_of_doc, Counter.Num_of_records, Counter.Num_of_doc_discard);
        }
        public static string current_package_type = string.Empty;
        private void LoadDoc()
        {
            //get doc for keyer
            var str_sv = NexGenClient.Service.Get_doc_for_keyer_with_agent(user.Username,"");
            
            _doc = str_sv.XMLStringToObject<DataObject.Document>();
            
            if (_doc == null)
            {
                MessageBox.Show("No more doc");
                this.Close();
                return;
            }
            if (!string.IsNullOrEmpty(_doc.Reset_by))
            {
                string comment = "Comment: " + _doc.Reset_comment;
                MessageBox.Show("Doc Reset by: "+_doc.Reset_by+"\r\n"+comment);
            }
            //hien thi thong bao neu package type thay doi
            if(string.IsNullOrEmpty(current_package_type)|| current_package_type != _doc.Doctype)
            {
                MessageBox.Show("You are working on "+_doc.Doctype+" package");
            }
            current_package_type = _doc.Doctype;
            lblPackageName.Text = "Package: "+_doc.Doc_path.Substring(9);
            txtTransactionDate.Text = _doc.Transaction_date;
            lblTotalToCheck.Text = string.Format("[{0}]", _doc.Total);
            txtTotalToCheck.Text = _doc.Total + string.Empty;
            _doc_image_path = _doc.Doc_path + "/" + _doc.Doc_name;
            var extention = "." + System.IO.Path.GetExtension(_doc.Doc_name);
            imgs = new string[]
            {
                _doc.Doc_name.Replace(extention, string.Empty),
                _doc.Image_2,
                _doc.Image_3,
                _doc.Image_4,
                _doc.Image_5,
                _doc.Image_6,
                _doc.Image_7,
                _doc.Image_8,
                _doc.Image_9,
            };
            imgs = imgs.Where(p => !string.IsNullOrEmpty(p)).Select(p => p + extention).ToArray();
            int i = 0;
            panelButtonImg.Controls.Clear();
            buttonImgs = new List<Control>();
            foreach (var img in imgs)
            {
                download_doc(_doc.Doc_path + "/" + img);
                i++;
                var button = new Button();
                button.Text = i + string.Empty;
                button.Width = 20;
                button.Left = 25 * i;
                button.Name = _doc.Doc_path + "/" + img;
                button.Click += new EventHandler(imageButton_click);
                buttonImgs.Add(button);
                panelButtonImg.Controls.Add(button);
            }
            
            //download image 2 neu co
            //string image2_name = _doc_image_path.Replace("_1.", "_2.");
            //download_doc(image2_name);
            //if (!System.IO.File.Exists("cache\\IMG\\" + image2_name))
            //{
            //    btnImg2.Visible = false;
            //}
            //else
            //    btnImg2.Visible = true;
            _ocr_data = GetOCRDoc(_doc_image_path);
            try
            {
                OCRData = _ocr_data.XmlSerialize().XMLStringToObject<DataObject.OCR.CoordinateList>();
            }
            catch (Exception)
            {
            }
            //log OCR can not parse
            if (OCRData == null)
                logger.Error("OCR parse errors");
            //add data for OCR old way, noisy
            if (_ocr_data != null)
            {
                DocORC.Doc = _ocr_data;
                if (DocORC.Doc.ChildNodes.Count > 0)
                    DocORC.Root = DocORC.Doc.ChildNodes[1];
            }
            
            var orginal_img = Image.FromFile("cache\\IMG\\" + _doc_image_path);
            old_width = orginal_img.Width;
            old_height = orginal_img.Height;
            _img = ImageTool.LowQuality(orginal_img);
            pboxImage.Image = _img;
            pboxImage.Size = _img.Size;
            
            //
            select_TransactionType(_doc.Doctype);
            
            btnUpdate.Enabled = false;
            watch = System.Diagnostics.Stopwatch.StartNew();
            watch.Start();
            timer = new Timer();
            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Tick += (sender, e) =>
            {
                lblTimer.Text = "Keying time: " + string.Format("{0:mm\\:ss}", watch.Elapsed);
            };
            timer.Start();
            lblImageName.Text = _doc.Doc_name.Replace(System.IO.Path.GetExtension(_doc.Doc_name), string.Empty);
            txtTransactionTime.Focus();
            //var tmpdoc_not_saved = (new TmpDoc()).load();
            //if(tmpdoc_not_saved!=null)
            //{
            //    if(tmpdoc_not_saved.Docid==_doc.Id)
            //    {
            //        var dialog = MessageBox.Show("Recover your doc has not been saved?", "Recover your doc has not been saved?", MessageBoxButtons.YesNo);
            //        if(dialog==System.Windows.Forms.DialogResult.Yes)
            //        {
            //            txtTransactionTime.Text = tmpdoc_not_saved.Time;
            //            //_list_form.set_data(tmpdoc_not_saved.records);
            //            _update_label_info();
            //        }
            //    }else
            //    {
            //        //tmpdoc_not_saved.records = null;
            //        tmpdoc_not_saved.save();//save blank data
            //    }
            //}
        }

        private void imageButton_click(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in buttonImgs)
                {
                    item.ForeColor = Color.Black;
                    //item.Font.bo
                }
            }
            catch (Exception)
            {
            }
            var control = (Control)sender;
            btnImageIndex = buttonImgs.IndexOf(control);
            control.ForeColor = Color.BurlyWood;
            string image_path2 = control.Name;
            if (!System.IO.File.Exists("cache\\IMG\\" + image_path2))
            {
                return;
            }
            var orginal_img = Image.FromFile("cache\\IMG\\" + image_path2);
            old_width = orginal_img.Width;
            old_height = orginal_img.Height;
            _img = ImageTool.LowQuality(orginal_img);
            pboxImage.Image = _img;
            
            _ocr_data = GetOCRDoc(image_path2);
            try
            {
                OCRData = _ocr_data.XmlSerialize().XMLStringToObject<DataObject.OCR.CoordinateList>();
                //log OCR can not parse
                if (OCRData == null)
                    logger.Error("OCR parse errors");
                if (_ocr_data != null)
                {
                    DocORC.Doc = _ocr_data;
                    if (DocORC.Doc.ChildNodes.Count > 0)
                        DocORC.Root = DocORC.Doc.ChildNodes[1];
                }
            }
            catch (Exception)
            {
            }
        }

        private void download_doc(string path)
        {
            try
            {
                Downloader.Run(new string[] { path });
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + "\n" + ex.StackTrace);
            }
            //Download_Doc_Data.StartDownload(path, true);
        }

        private DataObject.Record_base _editting_record;

        private void grid_update_menu_click(object obj, EventArgs e)
        {
            var record = _list_form.Get_selected_record();
            if (record == null)
                return;
            _editting_record = record;
            var usercontrol = this.panel3.Controls[0];
            if (usercontrol.GetType() == typeof(UserConctrol.doc_rin_input))
            {
                var i_usercontrol = (UserConctrol.doc_rin_input)usercontrol;
                
                i_usercontrol.Load_record_to_editting_form((DataObject.Record_RIN)record);
            }
            else if (usercontrol.GetType() == typeof(UserConctrol.doc_rsd_input))
            {
                var i_usercontrol = (UserConctrol.doc_rsd_input)usercontrol;
                
                i_usercontrol.Load_record_to_editting_form((DataObject.Record_RSD)record);
            }
            else if (usercontrol.GetType() == typeof(UserConctrol.doc_upc_input))
            {
                var i_usercontrol = (UserConctrol.doc_upc_input)usercontrol;
                
                i_usercontrol.Load_record_to_editting_form((DataObject.Record_UPC)record);
            }
            btnUpdate.Enabled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var usercontrol = this.panel3.Controls[0];
            if (usercontrol.GetType() == typeof(UserConctrol.doc_rin_input))
            {
                var i_usercontrol = (UserConctrol.doc_rin_input)usercontrol;
                i_usercontrol._TransactionType = cboxTransactionType.SelectedItem + string.Empty;
                i_usercontrol._TransactionDate = txtTransactionDate.Text;
                var new_record = i_usercontrol.GetRecord();
                if (new_record == null)
                {
                    //Common.ToastMessage.Show("Data input not valid");
                    lblMessage.Text = "Data input not valid";
                    return;
                }
                new_record.Sr_No = _editting_record.Sr_No;
                _list_form.UpdateRecord(new_record);
            }
            if (usercontrol.GetType() == typeof(UserConctrol.doc_rsd_input))
            {
                var i_usercontrol = (UserConctrol.doc_rsd_input)usercontrol;
                i_usercontrol._TransactionType = cboxTransactionType.SelectedItem + string.Empty;
                i_usercontrol._TransactionDate = txtTransactionDate.Text;
                var new_record = i_usercontrol.GetRecord();
                if (new_record == null)
                {
                    //Common.ToastMessage.Show("Data input not valid");
                    lblMessage.Text = "Data input not valid";
                    return;
                }
                new_record.Sr_No = _editting_record.Sr_No;
                _list_form.UpdateRecord(new_record);
            }
            if (usercontrol.GetType() == typeof(UserConctrol.doc_upc_input))
            {
                var i_usercontrol = (UserConctrol.doc_upc_input)usercontrol;
                i_usercontrol._TransactionType = cboxTransactionType.SelectedItem + string.Empty;
                i_usercontrol._TransactionDate = txtTransactionDate.Text;
                var new_record = i_usercontrol.GetRecord();
                if (new_record == null)
                {
                    //Common.ToastMessage.Show("Data input not valid");
                    lblMessage.Text = "Data input not valid";
                    return;
                }
                new_record.Sr_No = _editting_record.Sr_No;
                _list_form.UpdateRecord(new_record);
            }
            _list_form.bind_grid();
            btnUpdate.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <param name="imagepath">20150327/KS_Shawnee_05122014_003/OCR_Project_test.tif</param>
        private System.Xml.XmlDocument GetOCRDoc(string imagepath)
        {
            //+ load Image and OCR
            try
            {
                System.Xml.XmlDocument xmldoc = null;
                var extention = System.IO.Path.GetExtension(imagepath); 
                var newSentence = imagepath.Replace(extention, ".xml");
                try
                {
                    xmldoc = new System.Xml.XmlDocument();
                
                    //var regex = new System.Text.RegularExpressions.Regex(".TIF", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                
                    xmldoc.Load("cache\\OCR\\" + newSentence);
                    return xmldoc;
                    //xmldoc.Load("cache\\OCR\\" + filepath.Replace(".TIF", ".xml"));
                }
                catch (Exception ex)
                {
                    xmldoc = null;
                    logger.Error("OCR Error:" + "cache\\OCR\\" + newSentence);
                    logger.Error(ex.Message);
                    logger.Error(ex.StackTrace);
                    Common.ToastMessage.Show("OCR Error");
                }
            }
            catch (Exception)
            {
            }
            
            return null;
        }

        private void select_TransactionType(string type)
        {
            cboxTransactionType.SelectedItem = type;
        }

        #region Events
        
        private void cboxTransactionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.panel3.Controls.Clear();
            if (cboxTransactionType.SelectedItem + string.Empty == DataObject.EVRData.Transaction_type_RIN)
            {
                UserConctrol.doc_rin_input _doc_rin_input = new UserConctrol.doc_rin_input();
                
                this.panel3.Controls.Add(_doc_rin_input);
            }
            else if (cboxTransactionType.SelectedItem + string.Empty == DataObject.EVRData.Transaction_type_RSD)
            {
                var _doc_input = new UserConctrol.doc_rsd_input();
                
                this.panel3.Controls.Add(_doc_input);
            }
            else if (cboxTransactionType.SelectedItem + string.Empty == DataObject.EVRData.Transaction_type_UPC)
            {
                var _doc_input = new UserConctrol.doc_upc_input();
                
                this.panel3.Controls.Add(_doc_input);
            }
            else if (cboxTransactionType.SelectedItem + string.Empty == DataObject.EVRData.Transaction_type_RIN_RSD)
            {
                var _doc_input = new UserConctrol.doc_rin_rsd_input();

                this.panel3.Controls.Add(_doc_input);
            }
            else if (cboxTransactionType.SelectedItem + string.Empty == DataObject.EVRData.Transaction_type_UPC_RSD)
            {
                var _doc_input = new UserConctrol.doc_upc_rsd_input();

                this.panel3.Controls.Add(_doc_input);
            }
        }
        
        #endregion Events
        
        private void btnShowList_Click(object sender, EventArgs e)
        {
            _list_form.Show();
            _list_form.Activate();
        }

        public void _update_label_info()
        {
            var records = _list_form.get_data<DataObject.Record_base>();
            var total_record = records.Count;
            lblRecordInfo.Text = string.Format("Record: {0}/{1}", total_record, total_record);
            lblMessage.Text = string.Empty;
            try
            {
                //var records = _list_form.get_data<DataObject.Record_base>();
                var cal_total = records.Sum(p => p.Price);
                lblTotal.Text = cal_total + string.Empty;
                //lblTotal.BackColor = Color.Red;
                //if(string.Format("[{0}]",lblTotal.Text)==lblTotalToCheck.Text)
                //{
                //    lblTotal.BackColor = Color.Blue;
                //}
                var sum_item = records.Where(p => p.Type == DataObject.EVRData.Item_type_Item).Sum(p => p.Qty);
                var sum_void = records.Where(p => p.Type == DataObject.EVRData.Item_type_Void).Sum(p => p.Qty);
                lblCount.Text = string.Format("Count: {0}", sum_item - sum_void);
                try
                {
                    lblTax.Text = string.Format("Tax: {0}", Math.Round(_doc.Total - cal_total, 2));
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
                lblTotal.Text = "Data error, can not calculate [Total] result";
            }
        }

        DataObject.Record_base record_copy;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var usercontrol = this.panel3.Controls[0];
            if (usercontrol.GetType() == typeof(UserConctrol.doc_rin_input))
            {
                var i_usercontrol = (UserConctrol.doc_rin_input)usercontrol;
                i_usercontrol._TransactionType = cboxTransactionType.SelectedItem + string.Empty;
                i_usercontrol._TransactionDate = txtTransactionDate.Text;
                var new_record = i_usercontrol.GetRecord();
                
                if (new_record == null)
                {
                    //ToastMessage.Show("Data is not valid to create new record");                    
                    lblMessage.Text = "Data is not valid to create new record";
                    return;
                }
                if (active_copy_paste)
                {
                    if (new_record == null)
                    {
                        record_copy = _list_form.get_data<DataObject.Record_RIN>().LastOrDefault();
                    }
                    else
                    {
                        record_copy = new_record;
                    }
                    active_copy_paste = false;
                }
                _list_form.add_record(new_record);
            }
            else if (usercontrol.GetType() == typeof(UserConctrol.doc_rsd_input))
            {
                var i_usercontrol = (UserConctrol.doc_rsd_input)usercontrol;
                i_usercontrol._TransactionType = cboxTransactionType.SelectedItem + string.Empty;
                i_usercontrol._TransactionDate = txtTransactionDate.Text;
                var new_record = i_usercontrol.GetRecord();
                if (new_record == null)
                {
                    //ToastMessage.Show("Data is not valid to create new record");
                    lblMessage.Text = "Data is not valid to create new record";
                    return;
                }
                if (active_copy_paste)
                {
                    if (new_record == null)
                    {
                        record_copy = _list_form.get_data<DataObject.Record_RIN>().LastOrDefault();
                    }
                    else
                    {
                        record_copy = new_record;
                    }
                    active_copy_paste = false;
                }
                _list_form.add_record(new_record);
            }
            else if (usercontrol.GetType() == typeof(UserConctrol.doc_upc_input))
            {
                var i_usercontrol = (UserConctrol.doc_upc_input)usercontrol;
                i_usercontrol._TransactionType = cboxTransactionType.SelectedItem + string.Empty;
                i_usercontrol._TransactionDate = txtTransactionDate.Text;
                var new_record = i_usercontrol.GetRecord();
                if (new_record == null)
                {
                    //ToastMessage.Show("Data is not valid to create new record");
                    lblMessage.Text = "Data is not valid to create new record";
                    return;
                }
                if (active_copy_paste)
                {
                    if (new_record == null)
                    {
                        record_copy = _list_form.get_data<DataObject.Record_RIN>().LastOrDefault();
                    }
                    else
                    {
                        record_copy = new_record;
                    }
                    active_copy_paste = false;
                }
                _list_form.add_record(new_record);
            }
            else if (usercontrol.GetType() == typeof(UserConctrol.doc_rin_rsd_input))
            {
                var i_usercontrol = (UserConctrol.doc_rin_rsd_input)usercontrol;
                i_usercontrol._TransactionType = cboxTransactionType.SelectedItem + string.Empty;
                i_usercontrol._TransactionDate = txtTransactionDate.Text;
                var new_record = i_usercontrol.GetRecord();
                if (new_record == null)
                {
                    //ToastMessage.Show("Data is not valid to create new record");
                    lblMessage.Text = "Data is not valid to create new record";
                    return;
                }
                if (active_copy_paste)
                {
                    if (new_record == null)
                    {
                        record_copy = _list_form.get_data<DataObject.Record_RIN_RSD>().LastOrDefault();
                    }
                    else
                    {
                        record_copy = new_record;
                    }
                    active_copy_paste = false;
                }
                _list_form.add_record(new_record);
                
            }
            else if (usercontrol.GetType() == typeof(UserConctrol.doc_upc_rsd_input))
            {
                var i_usercontrol = (UserConctrol.doc_upc_rsd_input)usercontrol;
                i_usercontrol._TransactionType = cboxTransactionType.SelectedItem + string.Empty;
                i_usercontrol._TransactionDate = txtTransactionDate.Text;
                var new_record = i_usercontrol.GetRecord();
                if (new_record == null)
                {
                    //ToastMessage.Show("Data is not valid to create new record");
                    lblMessage.Text = "Data is not valid to create new record";
                    return;
                }
                if (active_copy_paste)
                {
                    if (new_record == null)
                    {
                        record_copy = _list_form.get_data<DataObject.Record_UPC_RSD>().LastOrDefault();
                    }
                    else
                    {
                        record_copy = new_record;
                    }
                    active_copy_paste = false;
                }
                _list_form.add_record(new_record);
            }
            _update_label_info();
            //txtTransactionTime.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var list_records = _list_form.get_data<DataObject.Record_base>();
            if (list_records == null || list_records.Count == 0)
            {
                btnAdd.PerformClick();//MessageBox.Show("No Record to save");
                //return;
            }
            list_records = _list_form.get_data<DataObject.Record_base>();
            if (list_records == null || list_records.Count == 0)
            {
                return;
            }
            DialogResult config = MessageBox.Show(string.Format("Save {0} records", list_records.Count), "Confirm", MessageBoxButtons.OKCancel);
            if (config != System.Windows.Forms.DialogResult.OK)
                return;
            list_records.ForEach(item =>
            {
                item.Docid = _doc.Id;
                item.Transaction_Date = DateTime.ParseExact(txtTransactionDate.Text, "MM/dd/yyyy", null);
                if (item.GetType() == typeof(DataObject.Record_RIN))
                {
                    var obj = (DataObject.Record_RIN)item;
                    obj.Item_number = obj.Item_number.ToUpper().TrimAllWhiteSpace();
                }
                else if (item.GetType() == typeof(DataObject.Record_RSD))
                {
                    var obj = (DataObject.Record_RSD)item;
                    obj.Item_description = obj.Item_description.ToUpper().TrimAllWhiteSpace();
                }
                else if(item.GetType() == typeof(DataObject.Record_UPC))
                {
                    var obj = (DataObject.Record_UPC)item;
                    obj.Upc_code = obj.Upc_code.ToUpper().TrimAllWhiteSpace();
                }
                else if (item.GetType() == typeof(DataObject.Record_RIN_RSD))
                {
                    var obj = (DataObject.Record_RIN_RSD)item;
                    obj.Item_number = obj.Item_number.ToUpper().TrimAllWhiteSpace();
                    obj.Description = obj.Description.ToUpper().TrimAllWhiteSpace();
                }
                else if (item.GetType() == typeof(DataObject.Record_UPC_RSD))
                {
                    var obj = (DataObject.Record_UPC_RSD)item;
                    obj.Upc_code = obj.Upc_code.ToUpper().TrimAllWhiteSpace();
                    obj.Description = obj.Description.ToUpper().TrimAllWhiteSpace();
                }
            });
            //Tinh Total cho first_rec
            //var total_price = list_records.Sum(p => p.Price);
            //list_records[0].Total = total_price + string.Empty;
            //_list_form.set_data(list_records);
            //_list_form.bind_grid();
            var rs = new DataObject.QueryResult();//str_sv.XMLStringToObject<DataObject.QueryResult>();
            string str_sv = string.Empty;
            double new_total = 0;
            try
            {
                new_total = double.Parse(txtTotalToCheck.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Value input for [Total] field is not valid");
                return;
            }
            // var data_save_to_disk = new TmpDoc();
            //        data_save_to_disk.Time = txtTransactionTime.Text;
            //        data_save_to_disk.Docid = _doc.Id;
            //        //data_save_to_disk.records = _list_form.get_data<DataObject.Record_base>();
            //var str = data_save_to_disk.XmlSerialize();
            //data_save_to_disk.save();
            //return;
            if (list_records != null)
            {
                long keying_time = watch.ElapsedMilliseconds;
                watch.Stop();
                timer.Stop();
                var type = list_records[0].GetType();
                if (type == typeof(DataObject.Record_RIN))
                {
                    str_sv = NexGenClient.Service.Insert_records_with_comment((_list_form.get_data<DataObject.Record_RIN>()).XmlSerialize(), DataObject.EVRData.Transaction_type_RIN, _doc.Id, watch.ElapsedMilliseconds, user.Username, txtTransactionDate.Text, txtTransactionTime.Text, new_total,txtComment.Text);
                }
                else if (type == typeof(DataObject.Record_RSD))
                {
                    str_sv = NexGenClient.Service.Insert_records_with_comment((_list_form.get_data<DataObject.Record_RSD>()).XmlSerialize(), DataObject.EVRData.Transaction_type_RSD, _doc.Id, watch.ElapsedMilliseconds, user.Username, txtTransactionDate.Text, txtTransactionTime.Text, new_total, txtComment.Text);
                }
                else if (type == typeof(DataObject.Record_UPC))
                {
                    str_sv = NexGenClient.Service.Insert_records_with_comment((_list_form.get_data<DataObject.Record_UPC>()).XmlSerialize(), DataObject.EVRData.Transaction_type_UPC, _doc.Id, watch.ElapsedMilliseconds, user.Username, txtTransactionDate.Text, txtTransactionTime.Text, new_total, txtComment.Text);
                }
                else if (type == typeof(DataObject.Record_RIN_RSD))
                {
                    str_sv = NexGenClient.Service.Insert_records_with_comment((_list_form.get_data<DataObject.Record_RIN_RSD>()).XmlSerialize(), DataObject.EVRData.Transaction_type_RIN_RSD, _doc.Id, watch.ElapsedMilliseconds, user.Username, txtTransactionDate.Text, txtTransactionTime.Text, new_total, txtComment.Text);
                }
                else if (type == typeof(DataObject.Record_UPC_RSD))
                {
                    str_sv = NexGenClient.Service.Insert_records_with_comment((_list_form.get_data<DataObject.Record_UPC_RSD>()).XmlSerialize(), DataObject.EVRData.Transaction_type_UPC_RSD, _doc.Id, watch.ElapsedMilliseconds, user.Username, txtTransactionDate.Text, txtTransactionTime.Text, new_total, txtComment.Text);
                }
                _list_form.set_data(new List<DataObject.Record_base>());
                //LoadDoc();
                rs = str_sv.XMLStringToObject<DataObject.QueryResult>();
                if (rs == null)
                {
                    LoadDoc();
                    //update Counter
                    Counter.Num_of_doc++;
                    Counter.Num_of_records += list_records.Count;
                    Update_counter_label();
                    Counter.Save();
                }
                else
                {
                    MessageBox.Show("Save Fail: " + rs.Message);
                    //var data_save_to_disk = new TmpDoc();
                    //data_save_to_disk.Time = txtTransactionTime.Text;
                    //data_save_to_disk.Docid = _doc.Id;
                    //data_save_to_disk.records = _list_form.get_data<DataObject.Record_base>();
                    //LoadDoc();
                }
                
                _update_label_info();
                if (!TopMost)
                    _list_form.Hide();
                //clear input form
                var usercontrol = this.panel3.Controls[0];
                if (usercontrol.GetType() == typeof(UserConctrol.doc_rin_input))
                {
                    var i_usercontrol = (UserConctrol.doc_rin_input)usercontrol;
                    i_usercontrol.resetForm();
                }
                else if (usercontrol.GetType() == typeof(UserConctrol.doc_rsd_input))
                {
                }
                else if (usercontrol.GetType() == typeof(UserConctrol.doc_upc_input))
                {
                }
                txtComment.Text = string.Empty;
                txtTransactionTime.Text = string.Empty;
                txtTransactionTime.Focus();

            }
            //NexGenClient.Service.Insert_records((_list_form.get_data<DataObject.Record_base>()).XmlSerialize(),cboxTransactionType.SelectedItem+string.Empty);
        }

        #region mouse scrolling events
         
        /************************************
         * IMessageFilter implementation
         * *********************************/
        private const int WM_MOUSEWHEEL = 0x20a;
        
        //private Control Moving_on_picture
        // P/Invoke declarations
        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(Point pt);
        
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
        
        //[DllImport("user32.dll")]
        //public static extern IntPtr GetParent(IntPtr hWnd);
        ///****************************************
        // * MouseWheelManagedForm specific methods
        // * **************************************/
        public void ManagedMouseWheelStart()
        {
            Application.AddMessageFilter(this);
        }
        
        public void ManagedMouseWheelStop()
        {
            Application.RemoveMessageFilter(this);
        }
        
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_VSCROLL = 0x115;
        private bool picture_focus_scrolling = false;
        
        private void pboxImage_MouseEnter(object sender, EventArgs e)
        {
            picture_focus_scrolling = true;
            ManagedMouseWheelStart();
        }
        
        private void pboxImage_MouseLeave(object sender, EventArgs e)
        {
            picture_focus_scrolling = false;
            ManagedMouseWheelStop();
        }
        
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_MOUSEWHEEL && m.Msg != WM_KEYDOWN)
            {
                Point pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);
                IntPtr hWnd = WindowFromPoint(pos);
                
                if (!picture_focus_scrolling)
                {
                    return false;
                }
                if (hWnd != IntPtr.Zero && hWnd != m.HWnd && Control.FromHandle(hWnd) != null)
                {
                    if (picture_focus_scrolling)
                    {
                        hWnd = panel1.Handle;
                    }
                    //+ đang hold control thì ko send nữa
                    //if (ModifierKeys.HasFlag(Keys.Control))
                    //{
                    //    //HK_Zoom(Math.Sign(e.Delta) > 0);
                    //    return false;
                    //}
                    SendMessage(hWnd, m.Msg, m.WParam, m.LParam);
                    //CtrlIsHolding = false;
                    return true;
                }
                //}
            }
            return false;
        }
        
        #endregion mouse scrolling events
        
        #region mouse drawing
        
        private System.Drawing.Graphics g;
        Brush brush = new SolidBrush(Color.FromArgb(65, 137, 240, 205));
        Pen pen = new Pen(Color.FromArgb(100, 137, 240, 205), 4);
        Pen pen2 = new Pen(Color.FromArgb(255, 137, 100, 255), 4);
        Pen pen3 = new Pen(Color.FromArgb(255, 137, 100, 255), 2);
        Pen penClicked = new Pen(Color.FromArgb(100, 248, 213, 57), 4);
        Pen penCrop = new Pen(Color.FromArgb(100, 248, 213, 57), 4);
        Point PDown = new Point();
        Point PUp = new Point();
        DataObject.OCR.Entity ClickedEntity;
            
        private Rectangle SelectRectangle()
        {
            Rectangle Result = new Rectangle();
            if (PUp.X < PDown.X)
            {
                Result.X = PUp.X;
                Result.Width = PDown.X - PUp.X;
            }
            else
            {
                Result.X = PDown.X;
                Result.Width = PUp.X - PDown.X;
            }
            if (PUp.Y < PDown.Y)
            {
                Result.Y = PUp.Y;
                Result.Height = PDown.Y - PUp.Y;
            }
            else
            {
                Result.Y = PDown.Y;
                Result.Height = PUp.Y - PDown.Y;
            }
            return Result;
        }
            
        private void pboxImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PUp = new Point(e.X, e.Y);
                
                #region test crop image
                
                //pboxImage.Refresh();
                //int cropWidth = e.X - PDown.X;
                //int cropHeight = e.Y - PDown.Y;
                //pboxImage.CreateGraphics().DrawRectangle(penCrop, e.X, e.Y, cropWidth, cropHeight);
            
                #endregion
            
                pboxImage.Invalidate();
            }
            else if (e.Button == MouseButtons.None)
            {
                try
                {
                    double percent = pboxImage.Size.Width * 1.0 / old_width;
                    var ocr = OCRData;
                    var entity = ocr.GetEntity(1, Convert.ToInt32(e.X / percent), Convert.ToInt32(e.Y / percent));
                    if (entity == null)
                    {
                        pboxImage.Invalidate();
                        return;
                    }
                    else
                    {
                        direct_draw(null, entity);
                    }
                }
                catch (Exception)
                {
                }
            }
        }
            
        private void pboxImage_MouseDown(object sender, MouseEventArgs e)
        {
            PDown = new Point(e.X, e.Y);
            PUp = new Point(e.X, e.Y);
            try
            {
                pboxImage.Invalidate();
                double percent = pboxImage.Size.Width * 1.0 / old_width;
                var ocr = OCRData;
                var entity = ocr.GetEntity(1, Convert.ToInt32(e.X / percent), Convert.ToInt32(e.Y / percent));
                if (entity == null)
                {
                    pboxImage.Invalidate();
                    return;
                }
                
                //draw_highlight_with_clear(entity);
                direct_draw(null, entity);
                ClickedEntity = new DataObject.OCR.Entity()
                {
                    Top = entity.Top,
                    Left = entity.Left,
                    Width = entity.Width,
                    Height = entity.Height,
                    String = entity.String
                };
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
        }

        Control focus_control;
        Regex check_format_transaction_time = new Regex("^[0-9]{1,2}[:]{1}[0-9]{1,2}(:[0-9]{1,2})?(\\s(AM|PM))?$");

        private delegate void HighlightDelegate(Control c);

        static System.Threading.Thread thread;

        private void HighlightControl(Control control)
        {
            if (thread != null && thread.IsAlive)
            {
                control.BackColor = Color.White;
                thread.Abort();
            }
                
            //if(thread==null)
            thread = new System.Threading.Thread(() => delay_change_highlight(control));
            //if (thread.IsAlive)
            //{
            //    //thread.Abort();
                
            //    thread = new Thread(() => delay_change_text(control));
            //}
            thread.Start();
        }

        private void delay_change_highlight(Control control)
        {
            System.Threading.Thread.Sleep(1500);
            if (control.InvokeRequired)
            {
                control.Invoke(new MethodInvoker(delegate ()
                {
                    control.BackColor = Color.White;
                }));
            }
            
        }

        private void pboxImage_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (addRuler)
                    draw_highlight_with_clear(null, new Point(e.X,e.Y));
                double percent = pboxImage.Size.Width * 1.0 / old_width;
                if (OCRData == null)
                {
                    return;
                }
                var entity = OCRData.GetEntity(1, Convert.ToInt32(e.X / percent), Convert.ToInt32(e.Y / percent));
                try
                {
                    draw_highlight_with_clear(entity, new Point(e.X,e.Y));
                }
                catch (Exception)
                {
                }
                
                //var new_rectangle = new Rectangle(Convert.ToInt32(e.X), Convert.ToInt32(e.Y),
                //    Convert.ToInt32(e.X)-Convert.ToInt32(PDown.X),
                //    Convert.ToInt32(e.Y)-Convert.ToInt32(PDown.Y));
                //var new_rectangle = new Rectangle(Convert.ToInt32(e.X / percent), Convert.ToInt32(e.Y / percent),
                //    Convert.ToInt32(e.X / percent)-Convert.ToInt32(PDown.X / percent),
                //    Convert.ToInt32(e.Y / percent)-Convert.ToInt32(PDown.Y / percent));
                string strResult = Common.OCR_old.OCRDrag(pboxImage, SelectRectangle(), DocORC, 0, (double)pboxImage.Width / old_width);
                
                if (entity == null && string.IsNullOrEmpty(strResult))
                    return;
                var ConfidenceRateArr = entity.ConfidenceRateList.Split(new char[] { '-' }).Where(p => !string.IsNullOrEmpty(p)).Select(p => int.Parse(p));
                foreach (var ConfidenceRate in ConfidenceRateArr)
                {
                    if (ConfidenceRate < 100)
                    {
                        var control = get_focused_control_for_OCR();
                        ;//ucKeyinfo.Controls.Find(ucKeyinfo.strControlName, true);
                        control.BackColor = Color.PaleVioletRed;
                        HighlightDelegate d = new HighlightDelegate(HighlightControl);
                        d(control);
                        break;
                    }
                }
                if (focus_control != null)
                {
                    if (focus_control.Name == "txtTransactionTime")
                    {
                    }
                    else
                        focus_control = get_focused_control_for_OCR();
                }
                else
                {
                    focus_control = get_focused_control_for_OCR();
                }
                
                if (focus_control != null)
                {
                    if (focus_control.GetType() == typeof(TextBox) ||
                        focus_control.GetType() == typeof(ComboBox))
                    {
                        //focus_control.Text = entity.String;
                        focus_control.Focus();
                        if (focus_control.Name == "txtItemDescription" || focus_control.Name == "txt_rsd_description")
                        {
                            focus_control.Text = string.Join(" ", (new string[2] { focus_control.Text.Trim(), strResult.Trim() }).Where(p => !string.IsNullOrEmpty(p))) + " ";
                            ((TextBox)focus_control).Select(focus_control.Text.Length, 0);
                        }
                        // update 2015-11-4
                        // remove ky tu ko phai~ so doi voi UPC va RIN
                        else if (focus_control.Name == "txtUPCCode"|| focus_control.Name == "txtItemNumber")
                        {
                            
                            focus_control.Text = Regex.Replace(strResult.Trim(), @"[^\d]", "");
                            SendKeys.Send("{TAB}");
                        }
                        else if (focus_control.Name == "txtTransactionTime")
                        {
                            focus_control.Text = strResult.Trim().Replace(".", ":").Replace(";", ":");
                            SendKeys.Send("{TAB}");
                        }
                        else
                        {
                            focus_control.Text = string.Join("", (new string[2] { focus_control.Text, strResult.Trim() }).Where(p => !string.IsNullOrEmpty(p)));
                            if (focus_control.Name == "txtPrice")
                            {
                                //rule tiền
                                focus_control.Text = focus_control.Text.Replace("$", string.Empty);
                                
                                Regex regex_character_remove = new Regex("[a-zA-Z]");
                                focus_control.Text = regex_character_remove.Replace(focus_control.Text, string.Empty);
                                focus_control.Text = Regex.Replace(focus_control.Text, "[^0-9\\.\\s]", string.Empty).Replace(" ", ".");
                            }
                            else if (focus_control.Name == "txtQty")
                            {
                                //rule tiền
                                focus_control.Text = focus_control.Text.Replace("$", string.Empty);
                                
                                Regex regex_character_remove = new Regex("[a-zA-Z]");
                                //focus_control.Text = regex_character_remove.Replace(focus_control.Text, string.Empty);
                                focus_control.Text = Regex.Replace(focus_control.Text, "[^0-9]", string.Empty);
                            }
                            if (focus_control.Name != "cboxItemType")
                                SendKeys.Send("{TAB}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
        
            PUp = new Point();
            PDown = new Point();
            //pboxImage.Invalidate();
        }

        //Regex number = new Regex("^[^0]*$");

        private Control get_focused_control_for_OCR()
        {
            var usercontrol = this.panel3.Controls[0];
            if (usercontrol.GetType() == typeof(UserConctrol.doc_rin_input))
            {
                var i_usercontrol = (UserConctrol.doc_rin_input)usercontrol;
                return i_usercontrol.Get_focused_control();
            }
            else if (usercontrol.GetType() == typeof(UserConctrol.doc_rsd_input))
            {
                var i_usercontrol = (UserConctrol.doc_rsd_input)usercontrol;
                return i_usercontrol.Get_focused_control();
            }
            else if (usercontrol.GetType() == typeof(UserConctrol.doc_upc_input))
            {
                var i_usercontrol = (UserConctrol.doc_upc_input)usercontrol;
                return i_usercontrol.Get_focused_control();
            }
            else if (usercontrol.GetType() == typeof(UserConctrol.doc_rin_rsd_input))
            {
                var i_usercontrol = (UserConctrol.doc_rin_rsd_input)usercontrol;
                return i_usercontrol.Get_focused_control();
            }
            else if (usercontrol.GetType() == typeof(UserConctrol.doc_upc_rsd_input))
            {
                var i_usercontrol = (UserConctrol.doc_upc_rsd_input)usercontrol;
                return i_usercontrol.Get_focused_control();
            }
            return null;
        }

        private void pboxImage_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                int transparency = 100;
                int red = 175;
                int green = 175;
                int blue = 255;
                SolidBrush B = new SolidBrush(Color.FromArgb(transparency, red, green, blue));
                e.Graphics.FillRectangle(B, SelectRectangle());
                    
                if (ClickedEntity != null)
                {
                    //draw_highlight_with_clear(ClickedEntity);
                    direct_draw(null, ClickedEntity);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
        }

        private static Bitmap tempBitmap;
        private static Bitmap originalBmp = null;

        private void getBitMapSearch(Image image, DataObject.OCR.Entity entityToDraw)
        {
            try
            {
                // The original bitmap with the wrong pixel format. 
                // You can check the pixel format with originalBmp.PixelFormat
                if (originalBmp != (Bitmap)image)
                {
                    originalBmp = (Bitmap)image;
                }
                //originalBmp.SelectActiveFrame(FrameDimension.Page, nPage - 1);
                
                //originalBmp.Size = new Size(panel1.Width - 10, pboxImage.Image.Height * panel1.Width / pboxImage.Image.Width);
                // Create a blank bitmap with the same dimensions
                if (tempBitmap != null)
                {
                    tempBitmap.Dispose();
                    tempBitmap = null;
                }
                tempBitmap = new Bitmap(originalBmp.Width, originalBmp.Height);
                //Bitmap tempBitmap = new Bitmap(old_width, old_height);
                // From this bitmap, the graphics can be obtained, because it has the right PixelFormat
                Graphics g = Graphics.FromImage(tempBitmap);
                // Draw the original bitmap onto the graphics of the new bitmap
                //var tmpPercent = pboxImage.Image.Size.Width *1.0/ old_width;
                //entityToDraw.Left = Convert.ToInt32( entityToDraw.Left*tmpPercent);
                //entityToDraw.Top = Convert.ToInt32( entityToDraw.Top*tmpPercent);
                //entityToDraw.Width = Convert.ToInt32( entityToDraw.Width*tmpPercent);
                //entityToDraw.Height = Convert.ToInt32( entityToDraw.Height*tmpPercent);
                double percent = pboxImage.Image.Size.Width * 1.0 / old_width;//(pboxImage.Size.Width * 1.0 / pboxImage.Image.Size.Width) * tmpPercent;
                g.DrawImage(originalBmp, 0, 0, originalBmp.Width, originalBmp.Height);
                if (entityToDraw != null)
                {
                    Rectangle rect = new Rectangle(
                        (Convert.ToInt32(entityToDraw.Left * percent)) - 2,
                        (Convert.ToInt32(entityToDraw.Top * percent)) - 4 ,
                        Convert.ToInt32(entityToDraw.Width * percent) + 8 ,
                        Convert.ToInt32(entityToDraw.Height * percent) + 6);
                
                    //g.FillRectangle(brush, rect);
                    g.DrawRectangle(pen2, rect);
                }
                if (addRuler)
                {
                    Rectangle rect = new Rectangle(
                        0,
                        (Convert.ToInt32(entityToDraw.Top * percent)) - 4 ,
                        _img.Width ,
                        1);
                
                    //g.FillRectangle(brush, rect);
                    g.DrawRectangle(pen3, rect);
                }
                string drawString = entityToDraw.String;
                System.Drawing.Font drawFont = new System.Drawing.Font("Arial", Convert.ToInt32(24));
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                int x = Convert.ToInt32(percent * entityToDraw.Left) - 2;
                int y = Convert.ToInt32(percent * entityToDraw.Top) - 4 - 50;
                System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
                
                Size textSize = TextRenderer.MeasureText(drawString, drawFont);
                
                var mypen = new SolidBrush(Color.FromArgb(100, 0, 0, 0));
                g.FillRectangle(mypen, x, y, textSize.Width, textSize.Height);
                g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
                
                drawFont.Dispose();
                drawBrush.Dispose();
                mypen.Dispose();
                drawFormat.Dispose();
                g.Flush();
                g.Dispose();
            }
            catch (Exception ex)
            {
                //log.Error(ex.Message);
                //log.Error(ex.StackTrace);
            }
        }

        private void getBitMapSearch(Image image, DataObject.OCR.Entity entityToDraw, Point point)
        {
            try
            {
                // The original bitmap with the wrong pixel format. 
                // You can check the pixel format with originalBmp.PixelFormat
                if (originalBmp != (Bitmap)image)
                {
                    originalBmp = (Bitmap)image;
                }
                //originalBmp.SelectActiveFrame(FrameDimension.Page, nPage - 1);
                
                //originalBmp.Size = new Size(panel1.Width - 10, pboxImage.Image.Height * panel1.Width / pboxImage.Image.Width);
                // Create a blank bitmap with the same dimensions
                if (tempBitmap != null)
                {
                    tempBitmap.Dispose();
                    tempBitmap = null;
                }
                tempBitmap = new Bitmap(originalBmp.Width, originalBmp.Height);
                //Bitmap tempBitmap = new Bitmap(old_width, old_height);
                // From this bitmap, the graphics can be obtained, because it has the right PixelFormat
                Graphics g = Graphics.FromImage(tempBitmap);
                // Draw the original bitmap onto the graphics of the new bitmap
                //var tmpPercent = pboxImage.Image.Size.Width *1.0/ old_width;
                //entityToDraw.Left = Convert.ToInt32( entityToDraw.Left*tmpPercent);
                //entityToDraw.Top = Convert.ToInt32( entityToDraw.Top*tmpPercent);
                //entityToDraw.Width = Convert.ToInt32( entityToDraw.Width*tmpPercent);
                //entityToDraw.Height = Convert.ToInt32( entityToDraw.Height*tmpPercent);
                double percent = pboxImage.Image.Size.Width * 1.0 / old_width;//(pboxImage.Size.Width * 1.0 / pboxImage.Image.Size.Width) * tmpPercent;
                g.DrawImage(originalBmp, 0, 0, originalBmp.Width, originalBmp.Height);
                if (entityToDraw != null)
                {
                    Rectangle rect = new Rectangle(
                        (Convert.ToInt32(entityToDraw.Left * percent)) - 2,
                        (Convert.ToInt32(entityToDraw.Top * percent)) - 4 ,
                        Convert.ToInt32(entityToDraw.Width * percent) + 8 ,
                        Convert.ToInt32(entityToDraw.Height * percent) + 6);
                
                    //g.FillRectangle(brush, rect);
                    g.DrawRectangle(pen2, rect);
                }
                if (addRuler)
                {
                    double percent2 = pboxImage.Width * 1.0 / _img.Width;
                    //Rectangle rect = new Rectangle(
                    //    0,
                    //    Convert.ToInt32(point.Y*percent2),
                    //    _img.Width ,
                    //    1);
                
                    //g.FillRectangle(brush, rect);
                    //g.DrawRectangle(pen3, rect);
                    var calY = Convert.ToInt32(point.Y / percent2);
                  
                    g.DrawLine(pen3, new Point() { X = 0, Y = calY },
                        new Point() { X = _img.Width, Y = calY });
                }
                try
                {
                    string drawString = entityToDraw.String;
                    System.Drawing.Font drawFont = new System.Drawing.Font("Arial", Convert.ToInt32(24));
                    System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                    int x = Convert.ToInt32(percent * entityToDraw.Left) - 2;
                    int y = Convert.ToInt32(percent * entityToDraw.Top) - 4 - 50;
                    System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
                
                    Size textSize = TextRenderer.MeasureText(drawString, drawFont);
                
                    var mypen = new SolidBrush(Color.FromArgb(100, 0, 0, 0));
                    g.FillRectangle(mypen, x, y, textSize.Width, textSize.Height);
                    g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);

                    drawFont.Dispose();
                    drawBrush.Dispose();
                    mypen.Dispose();
                    drawFormat.Dispose();
                }
                catch (Exception)
                {
                }
                
                g.Flush();
                g.Dispose();
            }
            catch (Exception ex)
            {
                //log.Error(ex.Message);
                //log.Error(ex.StackTrace);
            }
        }

        private void chkAddRuler_CheckedChanged(object sender, EventArgs e)
        {
            addRuler = chkAddRuler.Checked;
        }

        public void draw_highlight_with_clear(DataObject.OCR.Entity entity)
        {
            try
            {
                //double percent = pboxImage.Size.Width * 1.0 / old_width;
                //    //var ocr = OCRData;
                //    entity = OCRData.GetEntity(nPage, Convert.ToInt32(entity.Left / percent), Convert.ToInt32(entity.Top / percent));
                if (ClickedEntity == null)
                {
                    ClickedEntity = entity;
                }
                //reset to original image
                //double percent = pboxImage.Size.Width * 1.0 / old_width;
                //    //var ocr = OCRData;
                //    entity = OCRData.GetEntity(nPage, Convert.ToInt32(entity.Left / percent), Convert.ToInt32(entity.Top / percent));
                pboxImage.Image = _img;
                getBitMapSearch(pboxImage.Image, entity);
            
                pboxImage.Image = tempBitmap;
            }
            catch (Exception ex)
            {
                //log.Error(ex.Message);
                //log.Error(ex.StackTrace);
            }
        }

        public void draw_highlight_with_clear(DataObject.OCR.Entity entity, Point point)
        {
            try
            {
                //double percent = pboxImage.Size.Width * 1.0 / old_width;
                //    //var ocr = OCRData;
                //    entity = OCRData.GetEntity(nPage, Convert.ToInt32(entity.Left / percent), Convert.ToInt32(entity.Top / percent));
                if (ClickedEntity == null)
                {
                    ClickedEntity = entity;
                }
                //reset to original image
                //double percent = pboxImage.Size.Width * 1.0 / old_width;
                //    //var ocr = OCRData;
                //    entity = OCRData.GetEntity(nPage, Convert.ToInt32(entity.Left / percent), Convert.ToInt32(entity.Top / percent));
                pboxImage.Image = _img;
                getBitMapSearch(pboxImage.Image, entity, point);
            
                pboxImage.Image = tempBitmap;
            }
            catch (Exception ex)
            {
                //log.Error(ex.Message);
                //log.Error(ex.StackTrace);
            }
        }
        
        private void direct_draw(System.Drawing.Graphics g, DataObject.OCR.Entity entityToDraw)
        {
            try
            {
                if (entityToDraw == null)
                    return;
                if (g == null)
                    g = pboxImage.CreateGraphics();
                
                // Draw the original bitmap onto the graphics of the new bitmap
                double percent = pboxImage.Size.Width * 1.0 / old_width;
                if (entityToDraw != null)
                {
                    //pboxImage.Refresh();
                    Rectangle rect = new Rectangle(
                        (Convert.ToInt32(percent * entityToDraw.Left)) - 2,
                        (Convert.ToInt32(percent * entityToDraw.Top)) - 4 ,
                        Convert.ToInt32(percent * entityToDraw.Width) + 8 ,
                        Convert.ToInt32(percent * entityToDraw.Height) + 6);
                
                    g.DrawRectangle(pen, rect);
                }
                string drawString = entityToDraw.String;
                System.Drawing.Font drawFont = new System.Drawing.Font("Arial", Convert.ToInt32(18));
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                System.Drawing.SolidBrush drawBrushWrong = new SolidBrush(Color.FromArgb(150, 243, 255, 135));;//new System.Drawing.SolidBrush(System.Drawing.Color.Red);
                int x = Convert.ToInt32(percent * entityToDraw.Left) - 2;
                int y = Convert.ToInt32(percent * entityToDraw.Top) - 4 - 35;
                System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
                
                Size textSize = TextRenderer.MeasureText(drawString, drawFont);
                
                var mypen = new SolidBrush(Color.FromArgb(150, 0, 0, 0));
                var mypen_wrong = new SolidBrush(Color.FromArgb(150, 255, 80, 0));
                var entity = entityToDraw;
                g.FillRectangle(mypen, x, y, textSize.Width+Convert.ToInt32((textSize.Width*0.1)), textSize.Height);
                var new_string_position = x;
                if (entity != null)
                {
                    var ConfidenceRateArr = entity.ConfidenceRateList.Split(new char[] { '-' }).Where(p => !string.IsNullOrEmpty(p)).Select(p => int.Parse(p)).ToArray();
                    if (ConfidenceRateArr == null)
                        return;
                    for (int i = 0; i < ConfidenceRateArr.Length; i++)
                    {
                        var newCharStr = entity.String[i] + string.Empty;
                        var charSize = TextRenderer.MeasureText(newCharStr, drawFont);
                        if (ConfidenceRateArr[i] < 100)
                        {
                            //mypen = new SolidBrush(Color.FromArgb(150, 255, 80, 0));
                            //break;
                            g.DrawString(newCharStr, drawFont, drawBrushWrong, new_string_position, y, drawFormat);
                            new_string_position += charSize.Width-12;
                        }else
                        {
                            g.DrawString(newCharStr, drawFont, drawBrush, new_string_position, y, drawFormat);
                            new_string_position += charSize.Width-12;
                        }
                    }
                    //foreach (var ConfidenceRate in ConfidenceRateArr)
                    //{
                    //    var textSize = TextRenderer.MeasureText(drawString, drawFont);
                    //    if (ConfidenceRate < 100)
                    //    {
                    //        mypen = new SolidBrush(Color.FromArgb(150, 255, 80, 0));
                    //        break;
                    //    }
                    //}
                }
                
                
                //g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
                //drawFont.Dispose();
                //drawBrush.Dispose();
                //mypen.Dispose();
                //drawFormat.Dispose();
                //g.Flush();
                //g.Dispose();
            }
            catch (Exception)
            {
                var a = 1;
            }
        }
        
        #endregion mouse drawing
        
        #region hot key
            
        int zoom_size = 0;
                
        public void HK_Zoom_image(int size)
        {
            try
            {
                //pboxImage.Size = new Size(pboxImage.Image.Width, pboxImage.Image.Height);
                int w = pboxImage.Image.Width * (100 + size) / 100;
                //int h = w * IMG.Height / IMG.Width;
                var h = pboxImage.Image.Height * (100 + size) / 100;
                pboxImage.Size = new Size(w, h);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
        }

        private void panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            HK_Zoom(Math.Sign(e.Delta) > 0);
            base.OnMouseWheel(e);
        }

        public void HK_Zoom(bool isUp)
        {
            try
            {
                if (!ModifierKeys.HasFlag(Keys.Control))
                    return;
                if (isUp)
                {
                    zoom_size = zoom_size + 10;
                    HK_Zoom_image(zoom_size);
                }
                else
                {
                    if (zoom_size > -80)
                    {
                        zoom_size = zoom_size - 10;
                        HK_Zoom_image(zoom_size);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
        }

        private bool active_copy_paste = false;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //if (keyData == (Keys.Control | Keys.D1))
            //{
            //    btnImg1.PerformClick();
            //    return true;
            //}
            //else if (keyData == (Keys.Control | Keys.D2))
            //{
            //    btnImg2.PerformClick();
            //    return true;
            //}
            if (keyData == (Keys.F1))
            {
                try
                {
                    --btnImageIndex;
                    if (btnImageIndex <= 0)
                        btnImageIndex = 0;
                    ((Button)buttonImgs[btnImageIndex]).PerformClick();
                }
                catch (Exception)
                {
                }
                return true;
            }
            else if (keyData == (Keys.F2))
            {
                try
                {
                    ++btnImageIndex;
                    if (btnImageIndex >= buttonImgs.Count)
                        btnImageIndex = buttonImgs.Count - 1;
                    ((Button)buttonImgs[btnImageIndex]).PerformClick();
                }
                catch (Exception)
                {
                }
                return true;
            }
            else if (keyData == (Keys.F3))
            {
                try
                {
                    ((Button)buttonImgs[0]).PerformClick();
                }
                catch (Exception)
                {
                }
                return true;
            }
            else if (keyData == (Keys.F4))
            {
                try
                {
                    ((Button)buttonImgs[buttonImgs.Count - 1]).PerformClick();
                }
                catch (Exception)
                {
                }
                return true;
            }
            if (keyData == (Keys.Control | Keys.C))
            {
                btnAdd.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.S))
            {
                btnSave.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.K))
            {
                active_copy_paste = true;
                btnAdd.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.P))
            {
                if (record_copy != null)
                    _list_form.add_record(record_copy);
                _update_label_info();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.H))
            {
                try
                {
                    _list_form.Activate();
                    _list_form.Show();
                }
                catch (Exception)
                {
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        
        #endregion hot key
        
        #region discard
        
        private void btnDiscard_Click(object sender, EventArgs e)
        {
            panelDiscard.Visible = true;
        }
        
        private void btnCancelDiscard_Click(object sender, EventArgs e)
        {
            //txtDescriptionDiscard.Text = string.Empty;
            //txtDocNumdiscard.Text = string.Empty;
            panelDiscard.Visible = false;
        }
        
        private void btnOKDiscard_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboxRemarks.Text))
            {
                MessageBox.Show("Please input docnum and description to discard");
            }
            DialogResult config = MessageBox.Show("Discard this DOC???", "Confirm", MessageBoxButtons.OKCancel);
            if (config != System.Windows.Forms.DialogResult.OK)
                return;
            string str_sv = NexGenClient.Service.discard_doc_keyer(_doc.Id, string.Empty, cboxRemarks.Text, user.Username);
            var rs = str_sv.XMLStringToObject<DataObject.QueryResult>();
            if (rs == null)
            {
                _list_form.set_data(new List<DataObject.Record_base>());
                _update_label_info();
                //clear input form
                var usercontrol = this.panel3.Controls[0];
                if (usercontrol.GetType() == typeof(UserConctrol.doc_rin_input))
                {
                    var i_usercontrol = (UserConctrol.doc_rin_input)usercontrol;
                    i_usercontrol.resetForm();
                }
                else if (usercontrol.GetType() == typeof(UserConctrol.doc_rsd_input))
                {
                    var i_usercontrol = (UserConctrol.doc_rsd_input)usercontrol;
                    i_usercontrol.resetForm();
                }
                else if (usercontrol.GetType() == typeof(UserConctrol.doc_upc_input))
                {
                    var i_usercontrol = (UserConctrol.doc_upc_input)usercontrol;
                    i_usercontrol.resetForm();
                }
                LoadDoc();
                //update Counter
                Counter.Num_of_doc_discard++;
                Update_counter_label();
                Counter.Save();

                panelDiscard.Visible = false;
                txtTransactionTime.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("Fail to discard: " + rs.Message);
            }
        }
        
        #endregion discard

        private void chkTopList_CheckedChanged(object sender, EventArgs e)
        {
            _list_form.TopMost = chkTopList.Checked;
        }

        private void rotateRight_Click(object sender, EventArgs e)
        {
            pboxImage.Image = RotateImage(pboxImage.Image);
            focus_control = get_focused_control_for_OCR();
            if (focus_control != null)
            {
                
                    focus_control.Focus();
            }
        }

        public Image RotateImage(Image img)
        {
            var bmp = new Bitmap(img);

            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                gfx.Clear(Color.White);
                gfx.DrawImage(img, 0, 0, img.Width, img.Height);
            }

            bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            return bmp;
        }
        
        #region image next/previous click
        
        //private void btnImg1_Click(object sender, EventArgs e)
        //{
        //    var orginal_img = Image.FromFile("cache\\IMG\\" + _doc_image_path);
        //    old_width = orginal_img.Width;
        //    old_height = orginal_img.Height;
        //    _img = ImageTool.LowQuality(orginal_img);
        //    pboxImage.Image = _img;
        //    _ocr_data = GetOCRDoc(_doc_image_path);
        //    OCRData = _ocr_data.XmlSerialize().XMLStringToObject<DataObject.OCR.CoordinateList>();
        //    //log OCR can not parse
        //    if (OCRData == null)
        //        logger.Error("OCR parse errors");
        //}
        //private void btnImg2_Click(object sender, EventArgs e)
        //{
        //    string image_path2 = _doc_image_path.Replace("_1.", "_2.");
        //    if (!System.IO.File.Exists("cache\\IMG\\" + image_path2))
        //    {
        //        return;
        //    }
        //    var orginal_img = Image.FromFile("cache\\IMG\\" + image_path2);
        //    old_width = orginal_img.Width;
        //    old_height = orginal_img.Height;
        //    _img = ImageTool.LowQuality(orginal_img);
        //    pboxImage.Image = _img;
        //    _ocr_data = GetOCRDoc(image_path2);
        //    OCRData = _ocr_data.XmlSerialize().XMLStringToObject<DataObject.OCR.CoordinateList>();
        //    //log OCR can not parse
        //    if (OCRData == null)
        //        logger.Error("OCR parse errors");
        //}

        #endregion image next/previous click
    }
}