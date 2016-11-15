using System.Collections.Generic;
using System.ServiceModel;

namespace NexGenService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IsvManage_Download_File" in both code and config file together.
    [ServiceContract]
    public interface IsvManage_Download_File
    {
        [OperationContract]
        bool Insert(string downloadfile, string[] FileList,string listDocInfos, string username,string csv_file);

        //[OperationContract]
        //string Select(List<int> ID);

        //[OperationContract]
        //string Selectall();

        [OperationContract]
        string SelectImportedByFolderName(string listFolderName);

        [OperationContract]
        string verifyListDocumentInserted(string listFileName);

        [OperationContract]
        string verifyListDocumentInserted_withPath(string listFileName,string docpath);
        [OperationContract]
        string getPackages();
        [OperationContract]
        string Get_Assigned_Packages(string username, int packageid);
        [OperationContract]
        long addAssignPackage(string xmlPackageAssign);

        [OperationContract]
        void removeAssignPackage(int packageid);
        [OperationContract]
        void updateAssignPackage(string xmlPackageAssign);
    }
}
