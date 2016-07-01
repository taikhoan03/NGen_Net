using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Data;
using NexGenFlow.Manage;
using DataObject;
using Libs;
namespace NexGenService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "svManage_download_file" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select svManage_download_file.svc or svManage_download_file.svc.cs at the Solution Explorer and start debugging.
    //[System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Required)]
    [AspNetCompatibilityRequirements(
        RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class svManage_Download_File : IsvManage_Download_File
    {

        NexGenFlow.Manage.PackageImport manage_download_file = new NexGenFlow.Manage.PackageImport();

        public bool Insert(string downloadfile, string[] FileList,string listDocInfos, string username,string csv_file)
        {
            var docinfos = listDocInfos.XMLStringToListObject<DataObject.DocInfo>();
            //foreach (var item in docinfos)
            //        {
            //            var debug = 0;
            //            if(item.Receipt_id=="65601969")
            //            {
            //                debug = 1;
            //            }
            //        }
            return manage_download_file.Insert(downloadfile.XMLStringToObject<DataObject.PackageImport>(), FileList,docinfos, username,csv_file);
        }

        //public string Select(List<int> ID)
        //{
        //    return Download_File.ToXMLListString(manage_download_file.Select(ID));
        //}

        //public string Selectall()
        //{
        //    return Download_File.ToXMLListString(manage_download_file.Select());
        //}
        public string SelectImportedByFolderName(string listFolderName)
        {
            var listFolders=listFolderName.XMLStringToListObject<string>();
            var result=NexGenFlow.Manage.PackageImport.SelectNamePackageIsImported(listFolders);
            return result.XmlSerialize();
        }
        public string verifyListDocumentInserted(string listFileName)
        {
            var ListDocument = listFileName.XMLStringToListObject<string>();
            var result = (new NexGenFlow.Manage.Document()).verifyListDocumentInserted(ListDocument);
            return result.XmlSerialize();
        }
        public string verifyListDocumentInserted_withPath(string listFileName,string docpath)
        {
            var ListDocument = listFileName.XMLStringToListObject<string>();
            var result = (new NexGenFlow.Manage.Document()).verifyListDocumentInserted(ListDocument,docpath);
            return result.XmlSerialize();
        }
    }
}
