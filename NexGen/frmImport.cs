//using Clore_WinForm.svManageFileTransfer;

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DataObject;
using Libs;
using Limilabs.FTP.Client;
using log4net;

namespace NexGen
{
    public partial class frmImport : Form
    {
        public static readonly ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //IsvManage_DocumentClient svDocument = new IsvManage_DocumentClient();
        svDownloadFile.IsvManage_Download_FileClient svDownloadFile = new svDownloadFile.IsvManage_Download_FileClient();
        //IsvManage_File_TransferClient svFileTransfer = new IsvManage_File_TransferClient();
        DataObject.Util.ApplicationCacheData appCache = DataObject.Util.ApplicationCacheData.Init();
        string username = User.Get_authenticated_user().Username;

        struct FileHandler
        {
            public string MergeFrom;
            public string MergeTo;
        }

        //IsvManage_CountyClient svManageCounty = new IsvManage_CountyClient();

        //List<County> County_List = new List<County>();
        //List<string> State_List = new List<string>();
        public frmImport()
        {
            InitializeComponent();
            cboxDocType.Populate();
            //County_List = County.mapListObject(Common.serviceCounty.ListAllCounty());
            //State_List = County_List.Select(p => p.State_code).Distinct().ToList();
            dgFiletoImport.AutoGenerateColumns = false;

            DataGridViewComboBoxColumn c = (DataGridViewComboBoxColumn)dgFiletoImport.Columns["ColCountyID"];
            //c.DataSource = County_List;
            //c.DisplayMember = "County_name";
            //c.ValueMember = "ID";

            DataGridViewComboBoxColumn s = (DataGridViewComboBoxColumn)dgFiletoImport.Columns["ColStateCode"];
            //s.DataSource = State_List;

            txtImportURL.Text = appCache.LastImportedDir;
        }
        
        private void uploadFile(string foldername, string filename, string localFilePath)
        {
            try
            {
                using (Ftp client = new Ftp())
                {
                    //client.Mode = FtpMode.Active;
                    client.Connect("192.168.101.253");    // or ConnectSSL for SSL
                    client.Login("detoolv2", "UrgRu1");
                    client.CreateAllFolders(foldername);
                    //string currentfolder=client.GetCurrentFolder();
                    client.ChangeFolder(foldername);
                    client.Upload(filename, localFilePath);

                    client.Close();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + "\n " + ex.StackTrace);
            }
        }

        private void UpdatedgFiletoImport(DataObject.PackageImport F)
        {
            string Filename = F.Name;
            List<string> FileInfo = new List<string>(Filename.Split('_'));
            //County _cty = new County();
            //var test = County_List.Where(p => p.State_code == "CA" );
            //foreach(var item in test)
            //{
            //    Console.WriteLine(item.State_code + ":" + item.County_name);
            //}
            //List<County> L = new List<County>(from l in County_List
            //                                  where ((l.State_code == FileInfo[0]) && (l.County_name.ToUpper() == FileInfo[1].ToUpper()))
            //                                  //where ((l.State_code == FileInfo[0]) && (Filename.Replace(FileInfo[0]+"_",string.Empty).IndexOf(l.County_name.ToUpper())==0 ))
            //                                  select l);
            
            //if (L.Count == 0) throw new Exception("County " + FileInfo[1] + " in state " + FileInfo[0] + " does not exist.");
            //else _cty = L[0];

            ///////////////
            
            //F.County_ID = _cty.ID;
            //F.State_code = _cty.State_code;
            //F.Month_of_county = DateTime.ParseExact(FileInfo[2], "MMddyyyy", null).Month;
            //F.Year_of_county = DateTime.ParseExact(FileInfo[2], "MMddyyyy", null).Year;
            //F.Batch_num = int.Parse(FileInfo[3]);
            //F.Download_date = DateTime.Now;
            F.Create_date = DateTime.Now;
            //F.Modified_date = DateTime.Now;
        }

        /// <summary>
        /// This is the new way (copy files using web service)
        /// </summary>
        /// <param name="FileURL"></param>
        /// <param name="Decompressto"></param>
        /// 
        private static frmImportProccessing proccessing = new frmImportProccessing();
        public static System.Threading.ManualResetEvent ar1 = new System.Threading.ManualResetEvent(true);

        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (proccessing == null)
                proccessing = new frmImportProccessing();

            //this.BeginInvoke(new Action(() => (new frmImportProccessing()).ShowDialog()));
            //for (var i = 0; i <= 15;i++ )
            //{
            //    System.Threading.Thread.Sleep(1000);
            //    proccessing.proccess("file " + i, i, 15);
            //}
            try
            {
                if (string.IsNullOrEmpty(txtCSVFile.Text))
                {
                    MessageBox.Show("Please input vsc file");
                    return;
                }
                if (!File.Exists(txtCSVFile.Text))
                {
                    MessageBox.Show("File vsc not found");
                    return;
                }
            }
            catch (Exception)
            {
            }
            try
            {
                BtnImport.Enabled = false;
                BtnDelete.Enabled = false;
                frmImportProccessing.fails = 0;
                ar1.Reset();
                DataObject.PackageImport downloadfile = (DataObject.PackageImport)dgFiletoImport.CurrentRow.DataBoundItem;
                string F_URL = txtImportURL.Text + "\\" + downloadfile.Name;

                string Fileto = DateTime.Now.ToString("yyyyMMdd") + "/" + Path.GetFileName(F_URL);
                
                #region add more info to package
                
                //+ add more info to package
                downloadfile.File_path = Fileto;
                
                //record date
                try
                {
                    if (!cboxDocType.choseOneValid())
                    {
                        MessageBox.Show("Please chose Doc Type");
                        BtnImport.Enabled = true;
                        BtnDelete.Enabled = true;
                        return;
                    }
                    
                    downloadfile.Doctype = cboxDocType.SelectedItem + string.Empty;
                    downloadfile.Film_tracker_id = int.Parse(txtFilmTrackerID.Text) + string.Empty;
                    //DateTime _firstRecordDate = DateTime.ParseExact(txtFirstRecordDate.Text, "MMddyyyy", null);
                    //DateTime _lastRecordDate = DateTime.ParseExact(txtFirstRecordDate.Text, "MMddyyyy", null);
                    //downloadfile.First_record_date = _firstRecordDate;
                    //downloadfile.Last_record_date = _lastRecordDate;
                    //+ check docNum là kiểu số (viết tắt)
                    //downloadfile.First_doc_num = int.Parse(txtFirstDocNum.Text).ToString();
                    //downloadfile.Last_doc_num = int.Parse(txtLastDocNum.Text).ToString();
                }
                catch (Exception)
                {
                    //Common.ToastMessage("Data input is not correct!!!\n Doc number must be number\nDate format (MMddyyyy)");
                    MessageBox.Show("Data input is not correct!!!");
                    BtnImport.Invoke((Action)delegate { BtnImport.Enabled = true; });
                    BtnDelete.Invoke((Action)delegate { BtnDelete.Enabled = true; });
                    return;
                }
                downloadfile.Film_tracker_id = txtFilmTrackerID.Text;
                
                #endregion add more info to package
                
                //+ hiển thị form tiến độ xữ lý
                proccessing.Show();
                List<string> Filelist = ListDocInPackage.Where(p => p.IsImported != true).Select(p => p.Name).ToList();//FilesInFolder(F_URL); //
                List<System.Threading.Thread> tListThread = new List<System.Threading.Thread>();
                if (Filelist.Count == 0)
                {
                    BtnImport.Enabled = true;
                    BtnDelete.Enabled = true;
                    return;
                }
                var tMain = new System.Threading.Thread(() =>
                {
                    for (var i = 0; i < Filelist.Count; i++)
                    {
                        string filename = Filelist[i];
                        
                        frmImportProccessing.name = filename;
                        frmImportProccessing.index = i;
                        frmImportProccessing.total = Filelist.Count - 1;
                        System.Threading.Thread t = new System.Threading.Thread(() =>
                        {
                            proccessing.updateProccess();
                            //uploadFile();
                            string foldername = "Resource/IMG/" + DateTime.Now.ToString("yyyyMMdd") + "/" + Path.GetFileName(F_URL);
                            //string fileto = foldername + "/" + filename;
                            string localPath = F_URL + "\\" + filename;
                        
                            //+ upload to IMG server
                            uploadFile(foldername, filename, localPath);
                        });
                        t.SetApartmentState(System.Threading.ApartmentState.STA);
                        t.Start();
                    
                        t.Join();
                    }
                    //ar1.Set();
                    //+ Save data to DB
                    svDownloadFile.Insert(downloadfile.XmlSerialize(), Filelist.ToArray(), docinfos.XmlSerialize(), username, txtCSVFile.Text);
                    BtnImport.Invoke((Action)delegate { BtnImport.Enabled = true; });
                    BtnDelete.Invoke((Action)delegate { BtnDelete.Enabled = true; });
                    frmImportProccessing.OCRText = "Waiting for OCR";
                    proccessing.updateProccess();
                    runOCR(DateTime.Now.ToString("yyyyMMdd") + "/" + Path.GetFileName(F_URL), 0);
                    frmImportProccessing.OCRText = "OCR Done";
                    proccessing.updateProccess();
                    MessageBox.Show("Import succesfully!");
                });
            
                tMain.SetApartmentState(System.Threading.ApartmentState.STA);
                tMain.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            LoaddgFiletoImport();
        }
            
        private List<string> FilesInFolder(string folder)
        {
            var FileList = new List<string>();
            FileInfo[] Files = new DirectoryInfo(folder).GetFiles("*.TIF");
            foreach (FileInfo F in Files)
            {
                FileList.Add(F.Name);
            }
            return FileList;
        }
        
        #region Get info from ws

        string ServerURL = "192.168.101.253";//Common.GetServerURL();
        string IMGRoot = "Resource/IMG";//Common.GetIMGRoot();
        string OCRRoot = "Resource/OCR";//Common.GetOCRRoot();
        string OCRServerAddress = "http://192.168.101.169/ocr_service/";//Common.GetOCRServerAddress();
            
        #endregion
            
        private void runOCR(string Fileto, int countyID)
        {
            string IMGURL = "\\\\" + ServerURL + @"\" + IMGRoot + @"\" + Fileto + "\\";
            string OCRURL = "\\\\" + ServerURL + @"\" + OCRRoot + @"\" + Fileto + "\\";
            IMGURL = IMGURL.Replace('/', '\\');
            OCRURL = OCRURL.Replace('/', '\\');
        
            string parameter = "input=" + IMGURL +
                               "&output=" + OCRURL +
                               "&timelimit=" + 1000 +
                               "&countyid=" + countyID;
            PostMessageToURL(OCRServerAddress + "convert/", parameter);
        }
                
        public static string PostMessageToURL(string url, string parameters)
        {
            string HtmlResult = "";
            try
            {
                using (WebClientEx wc = new WebClientEx())
                {
                    wc.Headers[System.Net.HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    wc.Timeout = 20 * 60 * 1000;
                    HtmlResult = wc.UploadString(url, "POST", parameters);
                }
            }
            catch (Exception ex)
            {
                HtmlResult = "error";
                MessageBox.Show(ex.Message);
            }
            return HtmlResult;
        }
        
        private void btnLoadData_Click(object sender, EventArgs e)
        {
            LoaddgFiletoImport();
        }
                
        private void LoaddgFiletoImport()
        {
            try
            {
                List<string> ListFile = GetList(txtImportURL.Text).ToList();
                List<DataObject.PackageImport> ListDownloadFile = new List<DataObject.PackageImport>();
                var listImported = svDownloadFile.SelectImportedByFolderName(ListFile.XmlSerialize()).XMLStringToListObject<string>();
                        
                foreach (string str in ListFile)
                {
                    DataObject.PackageImport F = new DataObject.PackageImport();
                    foreach (var package in listImported)
                    {
                        if (str == package)
                        {
                            F.AlreadyImported = true;
                        }
                    }
                    F.Name = str;
                    F.Create_by = username;
                    F.Doctype = "";
                    UpdatedgFiletoImport(F);
                    ListDownloadFile.Add(F);
                }
                
                dgFiletoImport.DataSource = ListDownloadFile;
                dgFiletoImport.Columns["ColFileName"].DataPropertyName = "Name";
                    
                appCache.set("LastImportedDir", txtImportURL.Text);
                    
                if (dgFiletoImport.Rows.Count > 0)
                {
                    //var font = dgFiletoImport.Font.OriginalFontName;
                    var strikeoutFont = new Font(dgFiletoImport.Font.OriginalFontName, 10, FontStyle.Strikeout | FontStyle.Italic);
                    for (var i = 0; i < dgFiletoImport.Rows.Count; i++)
                    {
                        var packageItem = (DataObject.PackageImport)dgFiletoImport.Rows[i].DataBoundItem;
                        if (packageItem.AlreadyImported)
                        {
                            for (var cellindex = 0; cellindex < dgFiletoImport.Rows[i].Cells.Count; cellindex++)
                            {
                                dgFiletoImport.Rows[i].Cells[cellindex].Style.Font = strikeoutFont;
                                dgFiletoImport.Rows[i].Cells[cellindex].Style.ForeColor = Color.GhostWhite;
                                dgFiletoImport.Rows[i].Cells[cellindex].Style.BackColor = Color.Gray;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
            
        List<DocumentInPackage> ListDocInPackage = new List<DocumentInPackage>();
        List<DataObject.DocInfo> docinfos = new List<DocInfo>();

        private void btnLoadIndex_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCSVFile.Text))
                {
                    MessageBox.Show("Please input vsc file");
                    return;
                }
                if (!File.Exists(txtCSVFile.Text))
                {
                    MessageBox.Show("File vsc not found");
                    return;
                }
            }
            catch (Exception)
            {
            }
            try
            {
                try
                {
                    var lines = File.ReadAllLines(txtCSVFile.Text);  
                    docinfos = new List<DocInfo>();
                    foreach (var line in lines.Skip(1))
                    {
                        var columns = line.Split(new string[] { "," }, StringSplitOptions.None);
                        DocInfo docinfo = new DocInfo();
                        docinfo.Transaction_type = columns[0];
                        docinfo.Receipt_id = columns[1];
                        docinfo.Banner_id = columns[2];
                        double total = 0;
                        double.TryParse(columns[3], out total);
                        //if (total == 0)
                        //    total = -1;
                        docinfo.Total = total;
                        docinfo.Transaction_date = columns[4];
                        docinfo.Transaction_time = columns[5];
                        docinfo.Image_1 = columns[14];
                        docinfo.Image_2 = columns[15];
                        docinfo.Image_3 = columns[16];
                        docinfo.Image_4 = columns[17];
                        docinfo.Image_5 = columns[18];
                        docinfo.Image_6 = columns[19];
                        docinfo.Image_7 = columns[20];
                        docinfo.Image_8 = columns[21];
                        docinfo.Image_9 = columns[22];
                        docinfos.Add(docinfo);
                    }
                    //test
                    foreach (var item in docinfos)
                    {
                        var debug = 0;
                        if(item.Receipt_id=="65601969")
                        {
                            debug = 1;
                        }
                    }
                }
                catch (Exception exReadCSV)
                {
                    MessageBox.Show("Error while readling csv data");
                    return;
                }
                
                var currentPackageItem = (DataObject.PackageImport)dgFiletoImport.CurrentRow.DataBoundItem;
                //if (currentPackageItem.AlreadyImported)
                //{
                //    MessageBox.Show("This Package Item is already IMPORTED, Import again may cause duplicate Item, Please Notify me if you need to do did even if cause system errors");
                //    return;
                //}
                //dgIndex.Rows.Clear();
                string TriggerDir = txtImportURL.Text + @"\" + dgFiletoImport.CurrentRow.Cells["ColFileName"].Value.ToString();
                
                List<string> ListtxttFileBeforeIndex = new List<string>();
                List<string> ListtxttFileAfterIndex = new List<string>();
                    
                List<string> Listtxtindex = Directory.GetFiles(TriggerDir, "*.txt").ToList();
                if (Listtxtindex.Count > 0)
                {
                    string txtindex = Directory.GetFiles(TriggerDir, "*.txt")[0];
                    DataTable tmp = ReadtxtDocument(txtindex, '\t');
                    foreach (DataRow r in tmp.Rows)
                    {
                        ListtxttFileBeforeIndex.Add(r[4].ToString());
                        ListtxttFileAfterIndex.Add(r[0].ToString());
                    }
                }
                
                List<FileInfo> Files = new DirectoryInfo(TriggerDir).GetFiles("*.*").OrderBy(p => p.Name).ToList();
                
                if (Files.Count == 0)
                    throw new Exception("No TIF file to index");
                
                ListDocInPackage = new List<DocumentInPackage>();
                                                         
                var _downloadFile = (DataObject.PackageImport)dgFiletoImport.CurrentRow.DataBoundItem;
                string F_URL = txtImportURL.Text + "\\" + _downloadFile.Name;
                    
                string Fileto = DateTime.Now.ToString("yyyyMMdd") + "/" + Path.GetFileName(F_URL);
                var ListDocImported = svDownloadFile.verifyListDocumentInserted_withPath(Files.Select(p => p.Name).ToList().XmlSerialize(),
                                                         Fileto).XMLStringToListObject<string>();
                foreach (FileInfo F in Files)
                {
                    string Fname = F.Name;
                    string Iname = "";
                    int tmpIndex = ListtxttFileBeforeIndex.IndexOf(Fname);
                    if (tmpIndex > -1)
                        Iname = ListtxttFileAfterIndex[tmpIndex];
                    
                    //dgIndex.Rows.Add(Fname, Iname);
                    ListDocInPackage.Add(new DocumentInPackage
                    {
                        Name = F.Name,
                        IsImported = ListDocImported.Any(p => p == F.Name)
                    });
                }
                var orderedFiles = ListDocInPackage.OrderBy(p => p.IsImported);
                dgIndex.DataSource = orderedFiles.ToList();
                dgIndex.Columns["ClmBeforeIndex"].DataPropertyName = "Name";
                dgIndex.Columns["ClmAfterIndex"].DataPropertyName = "Index";
                        
                if (dgIndex.Rows.Count > 0)
                {
                    var strikeoutFont = new Font(dgIndex.Font.OriginalFontName, 10, FontStyle.Strikeout | FontStyle.Italic);
                    for (var i = 0; i < dgIndex.Rows.Count; i++)
                    {
                        var packageItem = (DocumentInPackage)dgIndex.Rows[i].DataBoundItem;
                        if (packageItem.IsImported)
                        {
                            for (var cellindex = 0; cellindex < dgIndex.Rows[i].Cells.Count; cellindex++)
                            {
                                dgIndex.Rows[i].Cells[cellindex].Style.Font = strikeoutFont;
                                dgIndex.Rows[i].Cells[cellindex].Style.ForeColor = Color.GhostWhite;
                                dgIndex.Rows[i].Cells[cellindex].Style.BackColor = Color.Gray;
                            }
                        }
                    }
                }
                //update: add more info to package
                        
                try
                {
                    List<string> Filelist = ListDocInPackage.Select(p => p.Name).ToList();
                    if (Filelist.FirstOrDefault() != null)
                    {
                        var name = Filelist.FirstOrDefault();
                        int startIndex = 5;
                        int length = name.IndexOf("-") - startIndex;
                        if (length <= 0)
                            length = name.IndexOf("@") - startIndex;
                        if (length <= 0)
                            length = name.IndexOf(".") - startIndex;
                        //currentPackageItem.First_doc_num = int.Parse(name.Substring(startIndex, length)).ToString();
                        //txtFirstDocNum.Text = currentPackageItem.First_doc_num;
                        //last
                        name = Filelist.LastOrDefault();
                        length = name.IndexOf("-") - startIndex;
                        if (length <= 0)
                            length = name.IndexOf("@") - startIndex;
                        if (length <= 0)
                            length = name.IndexOf(".") - startIndex;
                        //currentPackageItem.Last_doc_num = int.Parse(name.Substring(startIndex, length)).ToString();
                        //txtLastDocNum.Text = currentPackageItem.Last_doc_num;
                    }
                }
                catch (Exception)
                {
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
                
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string TriggerDir = txtImportURL.Text + @"\" + dgFiletoImport.CurrentRow.Cells["ColFileName"].Value.ToString();
                Directory.Delete(TriggerDir, true);
                btnLoadData.PerformClick();
            }
            catch
            {
                MessageBox.Show("Cannot delete at this time...");
            }
        }
                
        public string[] GetList(string ImportURL)
        {
            string[] List = Directory.GetDirectories(ImportURL);
            for (int i = 0; i < List.Length; i++)
            {
                List[i] = Path.GetFileName(List[i]);
            }
            return List;
        }
                
        private void btnRunIndex_Click(object sender, EventArgs e)
        {
            try
            {
                //List<string> ListMergeFrom = new List<string>();
                //string MergeTo = "";
                List<FileHandler> ListFhandler = new List<FileHandler>();
                foreach (DataGridViewRow DGR in dgIndex.Rows)
                {
                    FileHandler FH = new FileHandler();
                    FH.MergeFrom = DGR.Cells[0].Value.ToString();
                    FH.MergeTo = DGR.Cells[1].Value.ToString();
                    ListFhandler.Add(FH);
                    //if (DGR.Cells[2].Value != MergeTo)
                    //{
                    //    if (ListMergeFrom.Count > 0) { }
                    //}
                }
                foreach (string Mergeto in ListFhandler.Where(u => u.MergeTo != "").Select(p => p.MergeTo).Distinct().ToList())
                {
                    List<string> ListMergeFrom = ListFhandler.Where(p => p.MergeTo == Mergeto).Select(u => u.MergeFrom).Distinct().ToList();
                    if (ListMergeFrom.Count > 0)
                    {
                        string TriggerDir = txtImportURL.Text + @"\" + dgFiletoImport.CurrentRow.Cells["ColFileName"].Value.ToString();
                        
                        for (int i = 0; i < ListMergeFrom.Count; i++)
                        {
                            ListMergeFrom[i] = TriggerDir + @"\" + ListMergeFrom[i];
                        }
                        //TiffManager TM2 = new TiffManager();
                        //bool Join = TM2.JoinTiffImages(ListMergeFrom.ToArray(), TriggerDir + @"\" + Mergeto + ".TIF");
                        //Delete file after Merging
                        //if (Join == true)
                        //    foreach (string MergeFrom in ListMergeFrom)
                        //        File.Delete(MergeFrom);
                    }
                }
                btnLoadIndex.PerformClick();
                MessageBox.Show("File indexed successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
            
        [DllImport("advapi32.DLL")]
        private static extern int LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        public class DocumentInPackage
        {
            public string Name { get; set; }
        
            public string Index { get; set; }
            
            public bool IsImported { get; set; }
        }
                
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtImportURL.Text = folderBrowserDialog1.SelectedPath;
            }
        }
            
        public static DataTable ReadtxtDocument(string TXTFileName, char Splitter)
        {
            StreamReader oStreamReader = new StreamReader(TXTFileName);
            DataTable oDataTable = null;
            int RowCount = 0;
            List<string> oStreamDataValues = new List<string>();
                    
            while (!oStreamReader.EndOfStream)
            {
                String oStreamRowData = oStreamReader.ReadLine().Trim().ToString();
                if (oStreamRowData.Length > 0)
                {
                    string[] tmp = oStreamRowData.Split(Splitter);
                    oStreamDataValues = new List<string>(tmp);

                    //Bcoz the first row contains column names, we will poluate the column name by reading the first row and RowCount-0 will be true only once
                    if (RowCount == 0)
                    {
                        RowCount = 1;
                        oDataTable = new DataTable();
                        
                        foreach (string csvcolumn in oStreamDataValues)
                        {
                            DataColumn oDataColumn = new DataColumn();
                            oDataColumn.DefaultValue = string.Empty;
                            oDataTable.Columns.Add(oDataColumn);
                        }
                    }
                    DataRow oDataRow = oDataTable.NewRow();
                    for (int i = 0; i < oDataTable.Columns.Count; i++)
                    {
                        oDataRow[i] = oStreamDataValues[i] == null ? string.Empty : oStreamDataValues[i].ToString();
                    }
                    oDataTable.Rows.Add(oDataRow);
                }
            }
            oStreamReader.Close();
            oStreamReader.Dispose();
            return oDataTable;
        }
    }
}