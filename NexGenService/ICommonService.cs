﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace NexGenService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICommonService" in both code and config file together.
    [ServiceContract]
    public interface ICommonService
    {
        [OperationContract]
        void DoWork();
        #region user
        [OperationContract]
        string Login(string username, string password);

        [OperationContract]
        void UnLock(string username);
        #endregion user
        [OperationContract]
        string Get_doc_for_keyer(string username);
        [OperationContract]
        string Get_doc_for_keyer_with_agent(string username, string agent);

        #region add records
        [OperationContract]
        string Insert_records(string xml_record,string transaction_type,long documentid,long keying_time,string username,string transaction_date, string transaction_time,double total_update);

        [OperationContract]
        string Insert_records_with_comment(string xml_record, string transaction_type, long documentid, long keying_time, string username, string transaction_date, string transaction_time, double total_update,string comment);

        [OperationContract]
        string discard_doc_keyer(long docid,string docnum,string description,string username_discard);

        [OperationContract]
        string search_doc_for_export(int packageid);

        [OperationContract]
        string Get_packages();
        #endregion add records

        [OperationContract]
        string Get_packages_lastest(int limit);
        [OperationContract]
        void Update_package_priority(int packageid, int priority);

        [OperationContract]
        void Update_package_file_path(int packageid, string path);

        [OperationContract]
        void remove_package(int packageid);
        [OperationContract]
        int Get_secure_code(string code);
    }
}
