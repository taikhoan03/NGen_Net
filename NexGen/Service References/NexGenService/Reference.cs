﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NexGen.NexGenService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="NexGenService.ICommonService")]
    public interface ICommonService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommonService/DoWork", ReplyAction="http://tempuri.org/ICommonService/DoWorkResponse")]
        void DoWork();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommonService/Login", ReplyAction="http://tempuri.org/ICommonService/LoginResponse")]
        string Login(string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommonService/UnLock", ReplyAction="http://tempuri.org/ICommonService/UnLockResponse")]
        void UnLock(string username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommonService/Get_doc_for_keyer", ReplyAction="http://tempuri.org/ICommonService/Get_doc_for_keyerResponse")]
        string Get_doc_for_keyer(string username);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommonService/Get_doc_for_keyer_with_agent", ReplyAction="http://tempuri.org/ICommonService/Get_doc_for_keyer_with_agentResponse")]
        string Get_doc_for_keyer_with_agent(string username, string agent);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommonService/Insert_records", ReplyAction="http://tempuri.org/ICommonService/Insert_recordsResponse")]
        string Insert_records(string xml_record, string transaction_type, long documentid, long keying_time, string username, string transaction_date, string transaction_time, double total_update);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommonService/Insert_records_with_comment", ReplyAction="http://tempuri.org/ICommonService/Insert_records_with_commentResponse")]
        string Insert_records_with_comment(string xml_record, string transaction_type, long documentid, long keying_time, string username, string transaction_date, string transaction_time, double total_update, string comment);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommonService/discard_doc_keyer", ReplyAction="http://tempuri.org/ICommonService/discard_doc_keyerResponse")]
        string discard_doc_keyer(long docid, string docnum, string description, string username_discard);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommonService/search_doc_for_export", ReplyAction="http://tempuri.org/ICommonService/search_doc_for_exportResponse")]
        string search_doc_for_export(int packageid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommonService/Get_packages", ReplyAction="http://tempuri.org/ICommonService/Get_packagesResponse")]
        string Get_packages();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommonService/Get_packages_lastest", ReplyAction="http://tempuri.org/ICommonService/Get_packages_lastestResponse")]
        string Get_packages_lastest(int limit);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommonService/Update_package_priority", ReplyAction="http://tempuri.org/ICommonService/Update_package_priorityResponse")]
        void Update_package_priority(int packageid, int priority);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommonService/Update_package_file_path", ReplyAction="http://tempuri.org/ICommonService/Update_package_file_pathResponse")]
        void Update_package_file_path(int packageid, string path);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommonService/remove_package", ReplyAction="http://tempuri.org/ICommonService/remove_packageResponse")]
        void remove_package(int packageid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommonService/Get_secure_code", ReplyAction="http://tempuri.org/ICommonService/Get_secure_codeResponse")]
        int Get_secure_code(string code);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICommonServiceChannel : NexGen.NexGenService.ICommonService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CommonServiceClient : System.ServiceModel.ClientBase<NexGen.NexGenService.ICommonService>, NexGen.NexGenService.ICommonService {
        
        public CommonServiceClient() {
        }
        
        public CommonServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CommonServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CommonServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CommonServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void DoWork() {
            base.Channel.DoWork();
        }
        
        public string Login(string username, string password) {
            return base.Channel.Login(username, password);
        }
        
        public void UnLock(string username) {
            base.Channel.UnLock(username);
        }
        
        public string Get_doc_for_keyer(string username) {
            return base.Channel.Get_doc_for_keyer(username);
        }
        
        public string Get_doc_for_keyer_with_agent(string username, string agent) {
            return base.Channel.Get_doc_for_keyer_with_agent(username, agent);
        }
        
        public string Insert_records(string xml_record, string transaction_type, long documentid, long keying_time, string username, string transaction_date, string transaction_time, double total_update) {
            return base.Channel.Insert_records(xml_record, transaction_type, documentid, keying_time, username, transaction_date, transaction_time, total_update);
        }
        
        public string Insert_records_with_comment(string xml_record, string transaction_type, long documentid, long keying_time, string username, string transaction_date, string transaction_time, double total_update, string comment) {
            return base.Channel.Insert_records_with_comment(xml_record, transaction_type, documentid, keying_time, username, transaction_date, transaction_time, total_update, comment);
        }
        
        public string discard_doc_keyer(long docid, string docnum, string description, string username_discard) {
            return base.Channel.discard_doc_keyer(docid, docnum, description, username_discard);
        }
        
        public string search_doc_for_export(int packageid) {
            return base.Channel.search_doc_for_export(packageid);
        }
        
        public string Get_packages() {
            return base.Channel.Get_packages();
        }
        
        public string Get_packages_lastest(int limit) {
            return base.Channel.Get_packages_lastest(limit);
        }
        
        public void Update_package_priority(int packageid, int priority) {
            base.Channel.Update_package_priority(packageid, priority);
        }
        
        public void Update_package_file_path(int packageid, string path) {
            base.Channel.Update_package_file_path(packageid, path);
        }
        
        public void remove_package(int packageid) {
            base.Channel.remove_package(packageid);
        }
        
        public int Get_secure_code(string code) {
            return base.Channel.Get_secure_code(code);
        }
    }
}
