﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NexGen.svDownloadFile {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="svDownloadFile.IsvManage_Download_File")]
    public interface IsvManage_Download_File {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IsvManage_Download_File/Insert", ReplyAction="http://tempuri.org/IsvManage_Download_File/InsertResponse")]
        bool Insert(string downloadfile, string[] FileList, string listDocInfos, string username, string csv_file);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IsvManage_Download_File/SelectImportedByFolderName", ReplyAction="http://tempuri.org/IsvManage_Download_File/SelectImportedByFolderNameResponse")]
        string SelectImportedByFolderName(string listFolderName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IsvManage_Download_File/verifyListDocumentInserted", ReplyAction="http://tempuri.org/IsvManage_Download_File/verifyListDocumentInsertedResponse")]
        string verifyListDocumentInserted(string listFileName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IsvManage_Download_File/verifyListDocumentInserted_withPath", ReplyAction="http://tempuri.org/IsvManage_Download_File/verifyListDocumentInserted_withPathRes" +
            "ponse")]
        string verifyListDocumentInserted_withPath(string listFileName, string docpath);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IsvManage_Download_File/getPackages", ReplyAction="http://tempuri.org/IsvManage_Download_File/getPackagesResponse")]
        string getPackages();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IsvManage_Download_File/Get_Assigned_Packages", ReplyAction="http://tempuri.org/IsvManage_Download_File/Get_Assigned_PackagesResponse")]
        string Get_Assigned_Packages(string username, int packageid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IsvManage_Download_File/addAssignPackage", ReplyAction="http://tempuri.org/IsvManage_Download_File/addAssignPackageResponse")]
        long addAssignPackage(string xmlPackageAssign);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IsvManage_Download_File/removeAssignPackage", ReplyAction="http://tempuri.org/IsvManage_Download_File/removeAssignPackageResponse")]
        void removeAssignPackage(int packageid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IsvManage_Download_File/updateAssignPackage", ReplyAction="http://tempuri.org/IsvManage_Download_File/updateAssignPackageResponse")]
        void updateAssignPackage(string xmlPackageAssign);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IsvManage_Download_FileChannel : NexGen.svDownloadFile.IsvManage_Download_File, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class IsvManage_Download_FileClient : System.ServiceModel.ClientBase<NexGen.svDownloadFile.IsvManage_Download_File>, NexGen.svDownloadFile.IsvManage_Download_File {
        
        public IsvManage_Download_FileClient() {
        }
        
        public IsvManage_Download_FileClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public IsvManage_Download_FileClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IsvManage_Download_FileClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public IsvManage_Download_FileClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool Insert(string downloadfile, string[] FileList, string listDocInfos, string username, string csv_file) {
            return base.Channel.Insert(downloadfile, FileList, listDocInfos, username, csv_file);
        }
        
        public string SelectImportedByFolderName(string listFolderName) {
            return base.Channel.SelectImportedByFolderName(listFolderName);
        }
        
        public string verifyListDocumentInserted(string listFileName) {
            return base.Channel.verifyListDocumentInserted(listFileName);
        }
        
        public string verifyListDocumentInserted_withPath(string listFileName, string docpath) {
            return base.Channel.verifyListDocumentInserted_withPath(listFileName, docpath);
        }
        
        public string getPackages() {
            return base.Channel.getPackages();
        }
        
        public string Get_Assigned_Packages(string username, int packageid) {
            return base.Channel.Get_Assigned_Packages(username, packageid);
        }
        
        public long addAssignPackage(string xmlPackageAssign) {
            return base.Channel.addAssignPackage(xmlPackageAssign);
        }
        
        public void removeAssignPackage(int packageid) {
            base.Channel.removeAssignPackage(packageid);
        }
        
        public void updateAssignPackage(string xmlPackageAssign) {
            base.Channel.updateAssignPackage(xmlPackageAssign);
        }
    }
}
