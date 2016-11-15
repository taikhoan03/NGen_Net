using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DataObject;
using System.Configuration;
using log4net;
using DevExpress.Data.Filtering;
using Libs;

namespace NexGenFlow.Manage
{
    public class PackageImport
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool Insert(DataObject.PackageImport downloadfile, string[] FileList, List<DataObject.DocInfo> docinfos, string Username, string csv_file)
        {
            Session workingSession = new Session();
            try
            {
                if (downloadfile == null)
                    return false;

                workingSession.BeginTransaction();

                #region begin transaction
                CriteriaOperator criteria_limit_and_get_max_priority = CriteriaOperator.Parse("created_date >=?", DateTime.Now.AddMonths(-1));
                XPCollection cl_limit_and_get_max_priority = new XPCollection(workingSession, typeof(NexGen.package), criteria_limit_and_get_max_priority);
                var _list_limit_and_get_max_priority = new List<NexGen.package>();
                foreach (NexGen.package item in cl_limit_and_get_max_priority)
                {
                    _list_limit_and_get_max_priority.Add(item);
                }
                var max_priority = _list_limit_and_get_max_priority.Max(p => p.priority);

                CriteriaOperator criteria = CriteriaOperator.Parse(string.Format("name = '{0}'", downloadfile.Name));
                XPCollection cl = new XPCollection(workingSession, typeof(NexGen.package), criteria);
                var _list = new List<NexGen.package>();
                foreach (NexGen.package item in cl)
                {
                    _list.Add(item);
                }
                //edited by cuong.ha 2014-06-14
                //only insert if not existed yet
                int downloadFileID = 0;
                if (_list.Count() == 0)
                {
                    var DF = new NexGen.package(new Session());

                    //DF.batch_num=downloadfile.b
                    DF.name = downloadfile.Name;
                    DF.created_by = downloadfile.Create_by;
                    DF.created_date = DateTime.Now;
                    DF.doctype = downloadfile.Doctype;
                    DF.file_path = downloadfile.File_path;
                    DF.film_tracker_id = downloadfile.Film_tracker_id;
                    DF.csv_name = Path.GetFileName(csv_file);

                    if (max_priority > 0)
                        DF.priority = max_priority + 1;
                    else
                        DF.priority = downloadfile.Priority;
                    DF.Save();
                    downloadFileID = DF.id;
                    log.Info("Import new Package: ID=" + downloadFileID);
                }
                else
                    downloadFileID = _list.FirstOrDefault().id;//this File is existed in DB, get it value

                downloadfile.Id = downloadFileID;
                //var date = DateTime.Now;
                var DocsInPackageInserted = (new Document()).SelectfromFile(downloadFileID).Select(p => p.Doc_name);
                var filterNotInsertedDoc = FileList.Where(p => !DocsInPackageInserted.Any(docInserted => docInserted == Path.GetFileName(p)));
                var listDocBulkInsert = new List<DataObject.Document>();
                foreach (string File in filterNotInsertedDoc)
                {
                    //loai bo file hình thứ 2 cùng tên trở đi
                    if (File.IndexOf("_2.") > 0 ||
                        File.IndexOf("_3.") > 0 ||
                        File.IndexOf("_4.") > 0 ||
                        File.IndexOf("_5.") > 0 ||
                        File.IndexOf("_6.") > 0 ||
                        File.IndexOf("_7.") > 0 ||
                        File.IndexOf("_8.") > 0 ||
                        File.IndexOf("_9.") > 0 || File.ToLower() == "thumb.db")
                        continue;
                    var simplefilename = File.Replace(Path.GetExtension(File), string.Empty).Replace(".", string.Empty);
                    var docinfo = docinfos.Where(p => p.Image_1 == simplefilename).FirstOrDefault();

                    if (docinfo != null)
                    {
                        var doc = CreateDocument(downloadfile, File, docinfo, Username);
                        if (doc == null)
                        {
                            log.Error("Import Fail at: " + doc.XmlSerialize());
                            workingSession.RollbackTransaction();
                            return false;

                        }
                        listDocBulkInsert.Add(doc);
                        //log.Info("Insert Document in Package: filename" + File);
                    } else
                    {
                        log.Info("This doc is null: " + docinfo.XmlSerialize());
                    }

                }
                //foreach (var item in listDocBulkInsert)
                //    {
                //        var debug = 0;
                //        if(item.Receipt_id=="65601969")
                //        {
                //            debug = 1;
                //        }
                //    }
                (new NexGenFlow.Manage.Document()).Insert(listDocBulkInsert);
                //copy Document to Plan_Doc

                //int rs = workingSession.ExecuteNonQuery(string.Format("INSERT INTO \"{0}\"(docid, doctype, created_date) Select id, doctype, now() from \"{2}\" d where (select 1 from \"{1}\" pl where pl.docid=d.id) is null", TableName.CONS_plan_doc_keyer, TableName.CONS_plan_doc_keyer, TableName.CONS_document));
                workingSession.CommitTransaction();
                //int rs = (new Session()).ExecuteNonQuery("INSERT INTO \"plan_doc_keyer\"(docid, doctype, created_date) Select id, doctype, now() from \"document\" d where (select 1 from \"plan_doc_keyer\" pl where pl.docid=d.id) is null");
                //update 2015-06-03
                int rs = (new Session()).ExecuteNonQuery("INSERT INTO \"plan_doc_keyer\"(docid, doctype, created_date) Select d.id, d.doctype, now() from \"document\" d left join \"plan_doc_keyer\" pl on pl.docid=d.id where pl.docid is null");
                if (rs <= 0)
                {
                    throw new Exception(string.Format("No record moved from [{0}] to [{1}]", TableName.CONS_document, TableName.CONS_plan_doc_keyer));
                }
                //var a = (DateTime.Now - date).Milliseconds;

                return true;

                #endregion begin transaction
            }
            catch (Exception ex)
            {
                log.Error(ex);
                workingSession.RollbackTransaction();
                return false;
            }
        }

        private void InsertDocument(DataObject.PackageImport downloadfile, string File, string username, Session session)
        {
            DataObject.Document D = new DataObject.Document();
            D.Packageid = downloadfile.Id;
            D.Create_date = downloadfile.Create_date;
            D.Doc_name = Path.GetFileName(File);
            D.Doc_path = downloadfile.File_path;
            D.username = username;
            D.Create_date = DateTime.Now;
            D.Doctype = downloadfile.Doctype;

            (new Document()).Insert(D, session);
        }
        private DataObject.Document CreateDocument(DataObject.PackageImport downloadfile, string File, DocInfo docinfo, string username)
        {
            try
            {
                DataObject.Document D = new DataObject.Document();
                D.Packageid = downloadfile.Id;
                //D.Create_date = downloadfile.Create_date;
                D.Doc_name = Path.GetFileName(File);
                D.Doc_path = downloadfile.File_path;
                D.username = username;
                D.Create_date = DateTime.Now;
                D.Modified_date = DateTime.Now;
                D.Doctype = downloadfile.Doctype;

                D.Transaction_type = docinfo.Transaction_type;
                D.Receipt_id = docinfo.Receipt_id;
                D.Banner_id = docinfo.Banner_id;
                D.Total = docinfo.Total;
                D.Transaction_date = docinfo.Transaction_date;
                D.Transaction_time = docinfo.Transaction_time;
                D.Image_1 = docinfo.Image_1;
                D.Image_2 = docinfo.Image_2;
                D.Image_3 = docinfo.Image_3;
                D.Image_4 = docinfo.Image_4;
                D.Image_5 = docinfo.Image_5;
                D.Image_6 = docinfo.Image_6;
                D.Image_7 = docinfo.Image_7;
                D.Image_8 = docinfo.Image_8;
                D.Image_9 = docinfo.Image_9;

                return D;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.Error(ex.StackTrace);
                log.Error("DownloadFile:" + downloadfile.XmlSerialize());
                log.Error("DocInfo:" + docinfo.XmlSerialize());
                throw;
            }

        }
        private void InsertDocument(DataObject.PackageImport downloadfile, string File, DocInfo docinfo, string username)
        {
            try
            {
                DataObject.Document D = new DataObject.Document();
                D.Packageid = downloadfile.Id;
                //D.Create_date = downloadfile.Create_date;
                D.Doc_name = Path.GetFileName(File);
                D.Doc_path = downloadfile.File_path;
                D.username = username;
                D.Create_date = DateTime.Now;
                D.Modified_date = DateTime.Now;
                D.Doctype = downloadfile.Doctype;

                D.Transaction_type = docinfo.Transaction_type;
                D.Receipt_id = docinfo.Receipt_id;
                D.Banner_id = docinfo.Banner_id;
                D.Total = docinfo.Total;
                D.Transaction_date = docinfo.Transaction_date;
                D.Transaction_time = docinfo.Transaction_time;
                D.Image_1 = docinfo.Image_1;
                D.Image_2 = docinfo.Image_2;
                D.Image_3 = docinfo.Image_3;
                D.Image_4 = docinfo.Image_4;
                D.Image_5 = docinfo.Image_5;
                D.Image_6 = docinfo.Image_6;
                D.Image_7 = docinfo.Image_7;
                D.Image_8 = docinfo.Image_8;
                D.Image_9 = docinfo.Image_9;

                Manage.Document.Insert(D);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.Error(ex.StackTrace);
                log.Error("DownloadFile:" + downloadfile.XmlSerialize());
                log.Error("DocInfo:" + docinfo.XmlSerialize());
                throw;
            }
        }

        //public List<Download_File> Select(List<int> ID)
        //{
        //    XPCollection cl = new XPCollection(new Session(), typeof(Clore_DB.Download_File));
        //    var _list = cl.Cast<Clore_DB.Download_File>();
        //    return _list.Where(p => ID.Contains(p.ID)).Select(p => new Crunch_DataObject.Download_File
        //    {
        //        ID = p.ID,
        //        File_name = p.file_name,
        //        File_Path = p.file_path,
        //        County_ID = p.county_ID,
        //        Description = p.description,
        //        Download_date = p.download_date,
        //        Create_date = p.created_date,
        //        Modified_date = p.modified_date,
        //        First_doc_num = p.first_doc_num,
        //        Last_doc_num = p.last_doc_num,
        //        First_record_date = p.first_record_date,
        //        Last_record_date = p.last_record_date,
        //        Film_tracker_ID = p.film_tracker_ID,
        //        Month_of_county = p.month_of_county,
        //        Year_of_county = p.year_of_county,
        //        Batch_num = p.batch_num,
        //        State_code = p.state_code,
        //        Priority = p.priority,
        //    }).ToList();
        //}

        //public List<Download_File> Select()
        //{
        //    XPCollection cl = new XPCollection(new Session(), typeof(Clore_DB.Download_File));
        //    var _list = cl.Cast<Clore_DB.Download_File>();
        //    return _list.Select(p => new Crunch_DataObject.Download_File
        //    {
        //        ID = p.ID,
        //        File_name = p.file_name,
        //        File_Path = p.file_path,
        //        County_ID = p.county_ID,
        //        Description = p.description,
        //        Download_date = p.download_date,
        //        Create_date = p.created_date,
        //        Modified_date = p.modified_date,
        //        First_doc_num = p.first_doc_num,
        //        Last_doc_num = p.last_doc_num,
        //        First_record_date = p.first_record_date,
        //        Last_record_date = p.last_record_date,
        //        Film_tracker_ID = p.film_tracker_ID,
        //        Month_of_county = p.month_of_county,
        //        Year_of_county = p.year_of_county,
        //        Batch_num = p.batch_num,
        //        State_code = p.state_code,
        //        Priority = p.priority,
        //    }).ToList();
        //}

        //public static Download_File Select(int ID)
        //{
        //    XPCollection cl = new XPCollection(new Session(), typeof(Clore_DB.Download_File));
        //    var _list = cl.Cast<Clore_DB.Download_File>();
        //    return _list.Where(p => p.ID == ID).Select(p => new Crunch_DataObject.Download_File
        //    {
        //        ID = p.ID,
        //        File_name = p.file_name,
        //        File_Path = p.file_path,
        //        County_ID = p.county_ID,
        //        Description = p.description,
        //        Download_date = p.download_date,
        //        Create_date = p.created_date,
        //        Modified_date = p.modified_date,
        //        First_doc_num = p.first_doc_num,
        //        Last_doc_num = p.last_doc_num,
        //        First_record_date = p.first_record_date,
        //        Last_record_date = p.last_record_date,
        //        Film_tracker_ID = p.film_tracker_ID,
        //        Month_of_county = p.month_of_county,
        //        Year_of_county = p.year_of_county,
        //        Batch_num = p.batch_num,
        //        State_code = p.state_code,
        //        Priority = p.priority,
        //    }).FirstOrDefault();
        //}
        public List<DataObject.PackageImport> Get_packages()
        {
            XPCollection cl = new XPCollection(new Session(), typeof(NexGen.package));

            var packages = new List<NexGen.package>();
            foreach (NexGen.package item in cl)
            {
                packages.Add(item);
            }
            List<DataObject.PackageImport> packages_rs = new List<DataObject.PackageImport>();
            foreach (var item in packages)
            {
                DataObject.PackageImport package = new DataObject.PackageImport();
                package.Create_by = item.created_by;
                package.Create_date = item.created_date;
                package.Doctype = item.doctype;
                package.File_path = item.file_path;
                package.Film_tracker_id = item.film_tracker_id;
                package.Id = item.id;
                package.Name = item.name;
                package.Priority = item.priority;
                package.CSV_name = item.csv_name;
                packages_rs.Add(package);
            }
            if (packages_rs.Count > 0)
                return packages_rs;
            return null;
        }
        public List<DataObject.PackageAssignment> Get_Assigned_Packages(string username, int packageid)
        {
            
            Session workingSession = new Session();
            //XPCollection cl = new XPCollection(new Session(), typeof(Clore_DB.Document));
            
            var condition = new List<string>();
            if (!string.IsNullOrEmpty(username))
            {
                var str = string.Format("username='{0}'", username);
                condition.Add(str);
            }
            if (packageid>0)
            {
                var str = string.Format("packageid='{0}'", packageid);
                condition.Add(str);
            }
            condition.Add("is_enabled=TRUE");
            var strCondition = " " + string.Join(" AND ", condition) + " ";


            //strCondition = strCondition.Replace("OR ;", string.Empty);
            CriteriaOperator criteria = CriteriaOperator.Parse(strCondition);
            XPCollection cl = new XPCollection(workingSession, typeof(NexGen.package_assignment), criteria);

            var _list = new List<DataObject.PackageAssignment>();
            foreach (NexGen.package_assignment item in cl)
            {
                var assign_item = new DataObject.PackageAssignment();
                assign_item.id = item.id;
                assign_item.packageid = item.packageid;
                assign_item.priority = item.priority;
                assign_item.username = item.username;
                assign_item.created_date = item.created_date;
                assign_item.is_enabled = item.is_enabled;
                _list.Add(assign_item);
            }
            //var a = _list.Select(p => p.file_name).ToList();
            return _list;
        }
        public long addAssignPackage(DataObject.PackageAssignment ap)
        {
            var item = new NexGen.package_assignment(new Session());
            item.created_date = DateTime.Now;
            item.packageid = ap.packageid;
            item.priority = ap.priority;
            item.username = ap.username;
            item.is_enabled = ap.is_enabled;
            item.Save();
            return item.id;
        }
        public void updateAssignPackage(DataObject.PackageAssignment ap)
        {
            Session workingSession = new Session();



            //strCondition = strCondition.Replace("OR ;", string.Empty);
            CriteriaOperator criteria = CriteriaOperator.Parse("id=?", ap.id);
            XPCollection cl = new XPCollection(workingSession, typeof(NexGen.package_assignment), criteria);
            cl.TopReturnedObjects = 1;

            var item = (NexGen.package_assignment)cl[0];
            item.created_date = DateTime.Now;
            item.is_enabled = true;
            item.packageid = ap.packageid;
            item.priority = ap.priority;
            item.username = ap.username;
            item.is_enabled = ap.is_enabled;
            item.Save();
        }
        public void removeAssignPackage(int id)
        {
            Session workingSession = new Session();
            


            //strCondition = strCondition.Replace("OR ;", string.Empty);
            CriteriaOperator criteria = CriteriaOperator.Parse("id=?", id);
            XPCollection cl = new XPCollection(workingSession, typeof(NexGen.package_assignment), criteria);
            cl.TopReturnedObjects = 1;
            workingSession.Delete(cl);
            //foreach (NexGen.package_assignment item in cl)
            //{
            //    item.Delete();
            //    //item.Save();
            //}
        }
        public void Update_package_priority(int packageid,int priority)
        {
            CriteriaOperator criteria = CriteriaOperator.Parse("id = ?", packageid);

            XPCollection cl = new XPCollection(new Session(), typeof(NexGen.package),criteria);
            cl.TopReturnedObjects = 1;
            if (cl.Count > 0)
            {
                var package = (NexGen.package)cl[0];
                package.priority = priority;
                package.Save();
            }
            
        }
        public List<DataObject.PackageImport> Get_packages_lastest(int limit=50)
        {
                CriteriaOperator criteria = CriteriaOperator.Parse("priority < 9999999");
            XPCollection cl = new XPCollection(new Session(), typeof(NexGen.package), criteria);
            cl.Sorting.Add(new SortProperty("priority", DevExpress.Xpo.DB.SortingDirection.Descending));
            cl.TopReturnedObjects = limit;
            var packages = new List<NexGen.package>();
            foreach (NexGen.package item in cl)
            {
                packages.Add(item);
            }
            List<DataObject.PackageImport> packages_rs = new List<DataObject.PackageImport>();
            foreach (var item in packages)
            {
                DataObject.PackageImport package = new DataObject.PackageImport();
                package.Create_by = item.created_by;
                package.Create_date = item.created_date;
                package.Doctype = item.doctype;
                package.File_path = item.file_path;
                package.Film_tracker_id = item.film_tracker_id;
                package.Id = item.id;
                package.Name = item.name;
                package.Priority = item.priority;
                package.CSV_name = item.csv_name;
                packages_rs.Add(package);
            }
            if (packages_rs.Count > 0)
                return packages_rs;
            return null;
        }
        public static List<string> SelectNamePackageIsImported(List<string> packageNames)
        {
            try
            {
                Session workingSession = new Session();
                //XPCollection cl = new XPCollection(new Session(), typeof(Clore_DB.Document));
                string strCondition = string.Empty;
                foreach (var name in packageNames)
                {
                    strCondition += string.Format("name = '{0}' OR ", name);
                }
                strCondition += ";";
                strCondition = strCondition.Replace("OR ;", string.Empty);
                CriteriaOperator criteria = CriteriaOperator.Parse(strCondition);
                XPCollection cl = new XPCollection(workingSession, typeof(NexGen.package), criteria);

                var _list = new List<NexGen.package>();
                foreach (NexGen.package item in cl)
                {
                    _list.Add(item);
                }
                //var a = _list.Select(p => p.file_name).ToList();
                return _list.Select(p => p.name).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return null;
        }
        public void remove_package(int packageid)
        {
            Session workingSession = new Session();
            workingSession.BeginTransaction();
            try
            {
                int rs = (new Session()).ExecuteNonQuery(string.Format("delete from plan_doc_keyer where docid in (select id from document where packageid={0});delete from document where packageid = {0};            delete from package where id = {0}; ", packageid));
                workingSession.CommitTransaction();
            }
            catch (Exception ex)
            {
                workingSession.RollbackTransaction();
                log.Error(ex.Message + "\r\n" + ex.StackTrace);
            }
            

            //if (rs <= 0)
            //{
            //    throw new Exception(string.Format("No record moved from [{0}] to [{1}]", TableName.CONS_document, TableName.CONS_plan_doc_keyer));
            //}
        }
        public void change_package_path(int packageid,string path)
        {
            Session workingSession = new Session();
            workingSession.BeginTransaction();
            try
            {
                CriteriaOperator crit = CriteriaOperator.Parse("id=?", packageid);
                XPCollection cl_package = new XPCollection(workingSession, typeof(NexGen.package), crit);
                cl_package.TopReturnedObjects = 1;
                var package = (NexGen.package)cl_package[0];
                package.file_path = path;
                package.Save();

                //update document belong to this package
                using (UnitOfWork uow = new UnitOfWork())
                {
                    try
                    {
                        var xpCollection = new XPCollection<NexGen.document>(uow, CriteriaOperator.Parse("packageid=?", packageid));
                        foreach (NexGen.document doc in xpCollection)
                        {
                            doc.doc_path = path;
                        }
                        uow.CommitChanges();
                    }
                    catch (Exception aa)
                    {
                        uow.RollbackTransaction();
                        throw;
                    }
                    
                }

                //var crit_doc = CriteriaOperator.Parse("packageid=?", packageid);
                //XPCollection cl_docs = new XPCollection(workingSession, typeof(NexGen.document), crit);
                //foreach (NexGen.document doc in cl_docs)
                //{
                //    doc.doc_path = path;
                //}

                workingSession.CommitTransaction();
            }
            catch (Exception ex)
            {
                workingSession.RollbackTransaction();
                log.Error(ex.Message + "\r\n" + ex.StackTrace);
            }
        }
    }
}