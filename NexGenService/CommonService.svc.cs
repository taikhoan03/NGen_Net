using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Libs;
using log4net;

namespace NexGenService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CommonService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CommonService.svc or CommonService.svc.cs at the Solution Explorer and start debugging.
    public class CommonService : ICommonService
    {
        public static readonly ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void DoWork()
        {
        }
        static CommonService()
        {
            log4net.Config.XmlConfigurator.Configure();
            logger.Info("service start");
            InitConnection();
        }
        private static void InitConnection()
        {
            try
            {
                if (XpoDefault.Session == null || !XpoDefault.Session.IsConnected) 
                    NexGenFlow.NexGen.ConnectionHelper.Connect(AutoCreateOption.DatabaseAndSchema);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw new Exception("Cannot connect to database, please verify connection");
            }
        }
        #region login
        public string Login(string username, string password)
        {
            //InitConnection();
            return (new NexGenFlow.Manage.User()).login(username, password).XmlSerialize();
        }
        public string getListUser()
        {
            return (new NexGenFlow.Manage.User()).GetUsers().XmlSerialize();
        }
        #endregion login
        public string Get_doc_for_keyer(string username)
        {
            return (new NexGenFlow.Manage.UserAction()).Get_doc_for_keyer(username).XmlSerialize();
        }
        public string Get_doc_for_keyer_with_agent(string username, string agent)
        {
            return (new NexGenFlow.Manage.UserAction()).Get_doc_for_keyer(username, agent).XmlSerialize();
        }
        public void UnLock(string username)
        {
            var plan = new NexGenFlow.Manage.Plan_doc_keyer();
            plan.UnLock(username);
        }
        #region insert records
        public string Insert_records(string xml_record,string transaction_type,long documentid,long keying_time,string username,string transaction_date, string transaction_time,double total_update){
            if(transaction_type==DataObject.EVRData.Transaction_type_RIN)
            {
                var records = xml_record.XMLStringToListObject<DataObject.Record_RIN>();
                return (new NexGenFlow.Manage.Record_rin()).Insert(records,documentid,keying_time,username, transaction_date.Trim(),  transaction_time.Trim(), total_update).XmlSerialize();

            }
            else if(transaction_type==DataObject.EVRData.Transaction_type_RSD)
            {
                var records = xml_record.XMLStringToListObject<DataObject.Record_RSD>();
                return (new NexGenFlow.Manage.Record_rsd()).Insert(records,documentid,keying_time,username, transaction_date.Trim(),  transaction_time.Trim(), total_update).XmlSerialize();

            }
            else if (transaction_type==DataObject.EVRData.Transaction_type_UPC)
            {
                var records = xml_record.XMLStringToListObject<DataObject.Record_UPC>();
                return (new NexGenFlow.Manage.Record_upc()).Insert(records,documentid,keying_time,username, transaction_date.Trim(),  transaction_time.Trim(), total_update).XmlSerialize();

            }
            else if (transaction_type == DataObject.EVRData.Transaction_type_RIN_RSD)
            {
                var records = xml_record.XMLStringToListObject<DataObject.Record_RIN_RSD>();
                return (new NexGenFlow.Manage.Record_rin_rsd()).Insert(records, documentid, keying_time, username, transaction_date.Trim(), transaction_time.Trim(), total_update).XmlSerialize();

            }
            else if (transaction_type == DataObject.EVRData.Transaction_type_UPC_RSD)
            {
                var records = xml_record.XMLStringToListObject<DataObject.Record_UPC_RSD>();
                return (new NexGenFlow.Manage.Record_upc_rsd()).Insert(records, documentid, keying_time, username, transaction_date.Trim(), transaction_time.Trim(), total_update).XmlSerialize();

            }
            return null;
        }
        public string Insert_records_with_comment(string xml_record, string transaction_type, long documentid, long keying_time, string username, string transaction_date, string transaction_time, double total_update,string comment)
        {
            if (!string.IsNullOrEmpty(comment))
            {
                (new NexGenFlow.Manage.Plan_doc_keyer()).Update_comment(documentid, comment);
            }

            if (transaction_type == DataObject.EVRData.Transaction_type_RIN)
            {
                var records = xml_record.XMLStringToListObject<DataObject.Record_RIN>();
                return (new NexGenFlow.Manage.Record_rin()).Insert(records, documentid, keying_time, username, transaction_date.Trim(), transaction_time.Trim(), total_update).XmlSerialize();

            }
            else if (transaction_type == DataObject.EVRData.Transaction_type_RSD)
            {
                var records = xml_record.XMLStringToListObject<DataObject.Record_RSD>();
                return (new NexGenFlow.Manage.Record_rsd()).Insert(records, documentid, keying_time, username, transaction_date.Trim(), transaction_time.Trim(), total_update).XmlSerialize();

            }
            else if (transaction_type == DataObject.EVRData.Transaction_type_UPC)
            {
                var records = xml_record.XMLStringToListObject<DataObject.Record_UPC>();
                return (new NexGenFlow.Manage.Record_upc()).Insert(records, documentid, keying_time, username, transaction_date.Trim(), transaction_time.Trim(), total_update).XmlSerialize();

            }
            else if (transaction_type == DataObject.EVRData.Transaction_type_RIN_RSD)
            {
                var records = xml_record.XMLStringToListObject<DataObject.Record_RIN_RSD>();
                return (new NexGenFlow.Manage.Record_rin_rsd()).Insert(records, documentid, keying_time, username, transaction_date.Trim(), transaction_time.Trim(), total_update).XmlSerialize();

            }
            else if (transaction_type == DataObject.EVRData.Transaction_type_UPC_RSD)
            {
                var records = xml_record.XMLStringToListObject<DataObject.Record_UPC_RSD>();
                return (new NexGenFlow.Manage.Record_upc_rsd()).Insert(records, documentid, keying_time, username, transaction_date.Trim(), transaction_time.Trim(), total_update).XmlSerialize();

            }
            
            return null;
        }
        public string discard_doc_keyer(long docid,string docnum,string description,string username_discard)
        {
            return (new NexGenFlow.Manage.Plan_doc_keyer()).Discard_doc(docid, description, username_discard).XmlSerialize();
        }
        public string search_doc_for_export(int packageid)
        {
            return (new NexGenFlow.Manage.Document()).Get_keyed_doc_by_package_export(packageid).XmlSerialize();
        }
        public string Get_packages()
        {
            return (new NexGenFlow.Manage.PackageImport()).Get_packages().XmlSerialize();
        }
        #endregion insert records
        public string Get_packages_lastest(int limit)
        {
            return (new NexGenFlow.Manage.PackageImport()).Get_packages_lastest(limit).XmlSerialize();
        }
        public void Update_package_priority(int packageid, int priority)
        {
            (new NexGenFlow.Manage.PackageImport()).Update_package_priority(packageid, priority);
        }
        public void Update_package_file_path(int packageid, string path)
        {
            (new NexGenFlow.Manage.PackageImport()).change_package_path(packageid, path);
        }
        public void remove_package(int packageid)
        {
            (new NexGenFlow.Manage.PackageImport()).remove_package(packageid);
        }
        public int Get_secure_code(string code)
        {
            if (code == "FJDSKFLDJ@!32")
                return 44568;
            return 0;
        }
    }
}
