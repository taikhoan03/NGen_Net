using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading;
using Libs;
using System.Threading;
namespace NexGenService
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class svUploadFile : IsvUploadFile
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private FtpWebRequest CreateFTPRequest(string ServerURL)
        {
            try
            {
                string URL = ServerURL;
                string UserName = ConfigurationManager.AppSettings["UserName"];
                string Password = ConfigurationManager.AppSettings["Password"];

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(URL));

                //Set proxy to null. Under current configuration if this option is not set then the proxy that is used will get an html response from the web content gateway (firewall monitoring system)
                request.Proxy = null;

                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;
                request.Timeout = 3000;
                request.ReadWriteTimeout = 3000;

                request.Credentials = new NetworkCredential(UserName, Password);
                return request;
            }
            catch
            { return null; }
        }

        

        //private static Logger Logger = LogManager.GetCurrentClassLogger();
        public RemoteFileInfo DownloadFile(DownloadRequest request)
        {
            RemoteFileInfo result = new RemoteFileInfo();
            try
            {
                string ServerURL = ConfigurationManager.AppSettings["serverURL"];
                string Folder = "";
                switch (request.FolderType)
                {
                    case EnumType.FolderTypes.IMG:
                        Folder = ConfigurationManager.AppSettings["IMGURL"];
                        request.FileName = request.FileName.Replace(".xml", ".tif");
                        break;
                    case EnumType.FolderTypes.OCR:
                        Folder = ConfigurationManager.AppSettings["OCRURL"];
                        request.FileName = request.FileName.Replace(".tif", ".xml");
                        break;
                    default:
                        break;
                }
                
                string IMGURL = "ftp://" + ConfigurationManager.AppSettings["serverURL"] + "/" + Folder + "/" + request.FolderName + "/" + request.FileName;

                FtpWebRequest ftpRequest = CreateFTPRequest(IMGURL);
                ftpRequest.Timeout = 180000;
                ftpRequest.UsePassive = true;
                ftpRequest.UseBinary = false;
                ftpRequest.KeepAlive = false;
                ftpRequest.Proxy = null;
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                using (FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse())
                {
                    Stream responseStream = response.GetResponseStream();

                    result.FileName = request.FileName;
                    result.FileByteStream = responseStream;

                    //response.Close();
                    OperationContext context = OperationContext.Current;
                }

                
                //context.OperationCompleted += (sender, args) => response.Dispose();
            }
            catch (Exception ex)
            {
                //Logger.ErrorException("Download File", ex);
            }
            finally
            {
            }

            return result;
        }

        private Boolean CheckFileNotExist(string strFilePath)
        {
            Boolean bResult = false;
            FtpWebRequest ftpRequest = CreateFTPRequest(strFilePath);
            ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;
            try
            {
                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                response.Close();
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode ==
                    FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    //Does not exist
                    bResult = true;
                }
                else
                {
                    bResult = false;
                }
                response.Close();
            }
            return bResult;
        }

        private Boolean CreateDirectory(string strFolderPath)
        {
            Boolean bResult = true;
            FtpWebRequest ftpRequest = CreateFTPRequest(strFolderPath);
            ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                response.Close();
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                bResult = false;
                response.Close();
            }
            return bResult;
        }

        private Boolean CheckFolderNotExist(string strFolderPath)
        {
            Boolean bResult = false;
            FtpWebRequest ftpRequest = CreateFTPRequest(strFolderPath);
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
            try
            {
                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                response.Close();
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    bResult = true;
                }
                else
                {
                    bResult = false;
                }
                response.Close();
            }
            return bResult;
        }
        public void UploadFile(RemoteFileInfo request)
        {
            var a = 1;
            string ServerURL = ConfigurationManager.AppSettings["serverURL"];
            string _IMGURL = ConfigurationManager.AppSettings["IMGURL"];
            string _OCRURL = ConfigurationManager.AppSettings["OCRURL"];
            var fileContentAsByteArray = DataObject.Util.MyFile.convertStreamToByteArray(request.FileByteStream);
            
            var t_main_uploadThread = new Thread(() => {
                Stream targetStream = null;
                Stream sourceStream = null;
                try
                {

                    string Folder = "";
                    switch (request.FolderType)
                    {
                        case EnumType.FolderTypes.IMG:
                            Folder = _IMGURL;
                            break;
                        case EnumType.FolderTypes.OCR:
                            Folder = _OCRURL;
                            break;
                        default:
                            break;
                    }
                    string URL = "ftp://" + ConfigurationManager.AppSettings["serverURL"] + "/" + Folder + "/" + request.FolderName + "/";
                    string IMGURL = URL + request.FileName;

                    if (CheckFolderNotExist(URL))
                    {
                        string[] lstFolder = request.FolderName.Trim('/').Replace("//", "/").Split('/');
                        string strURI_temp = "ftp://" + ConfigurationManager.AppSettings["serverURL"] + "/" + Folder + "/";
                        foreach (string str in lstFolder)
                        {
                            strURI_temp = strURI_temp + str + "/";
                            if (CheckFolderNotExist(strURI_temp))
                            {
                                CreateDirectory(strURI_temp);
                            }
                        }
                    }


                    // instance a memory stream and pass the byte array to its constructor
                    FtpWebRequest ftpRequest = CreateFTPRequest(IMGURL);
                    ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                    ftpRequest.ContentLength = request.Length;
                    ftpRequest.Timeout = 1000000 * 90;
                    // Copy the contents of the file to the request stream.

                    //Stream sourceStream =null;//request.FileByteStream;
                    sourceStream = new MemoryStream(fileContentAsByteArray);
                    using (targetStream = ftpRequest.GetRequestStream())
                    {
                        const int bufferLen = 65000;
                        byte[] buffer = new byte[bufferLen];
                        int count = 0;
                        while ((count = sourceStream.Read(buffer, 0, bufferLen)) > 0)
                        {
                            targetStream.Write(buffer, 0, count);
                        }
                        //while ((count = sourceStream.Read(buffer, 0, bufferLen)) > 0)
                        //{
                        //    targetStream.Write(buffer, 0, count);
                        //}
                        targetStream.Close();
                        sourceStream.Close();
                    }
                    FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                    response.Close();

                }
                catch (IOException IOEx)
                {
                    log.Debug(IOEx);
                    //throw ex;
                }
                catch (Exception ex)
                {
                    log.Error("Upload file " + request.FileName + " fail.", ex);
                    //throw ex;
                }
                finally
                {
                    if (targetStream != null)
                    {
                        targetStream.Close();
                        targetStream.Dispose();
                        //targetStream.Close();
                    }
                    if (sourceStream != null)
                    {
                        sourceStream.Close();
                        sourceStream.Dispose();
                        //targetStream.Close();
                    }
                }
            });
            t_main_uploadThread.SetApartmentState(System.Threading.ApartmentState.STA);
            t_main_uploadThread.Start();
            t_main_uploadThread.Join();
        }
        #region Backup
        //public void UploadFile(RemoteFileInfo request)
        //{
        //    try
        //    {
        //        string ServerURL = ConfigurationManager.AppSettings["serverURL"];
        //        string Folder = "";
        //        switch (request.FolderType)
        //        {
        //            case EnumType.FolderTypes.IMG:
        //                Folder = ConfigurationManager.AppSettings["IMGURL"];
        //                break;
        //            case EnumType.FolderTypes.OCR:
        //                Folder = ConfigurationManager.AppSettings["OCRURL"];
        //                break;
        //            default:
        //                break;
        //        }
        //        string URL = "ftp://" + ConfigurationManager.AppSettings["serverURL"] + "/" + Folder + "/" + request.FolderName + "/";
        //        string IMGURL = URL + request.FileName;

        //        if (CheckFolderNotExist(URL))
        //        {
        //            string[] lstFolder = request.FolderName.Trim('/').Replace("//", "/").Split('/');
        //            string strURI_temp = "ftp://" + ConfigurationManager.AppSettings["serverURL"] + "/" + Folder + "/";
        //            foreach (string str in lstFolder)
        //            {
        //                strURI_temp = strURI_temp + str + "/";
        //                if (CheckFolderNotExist(strURI_temp))
        //                {
        //                    CreateDirectory(strURI_temp);
        //                }
        //            }
        //        }
        //        /*
        //        FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
        //        ftp.Method = WebRequestMethods.Ftp.MakeDirectory;
        //        FtpWebResponse CreateForderResponse = (FtpWebResponse)ftp.GetResponse();
        //        if (CreateForderResponse.StatusCode == FtpStatusCode.PathnameCreated)
        //        {
        //        }
        //         */
        //        //if (!CheckFileNotExist(IMGURL))
        //        //{
        //        //    DeleteFile(IMGURL);
        //        //}

        //        // instance a memory stream and pass the byte array to its constructor
        //        FtpWebRequest ftpRequest = CreateFTPRequest(IMGURL);
        //        ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
        //        ftpRequest.ContentLength = request.Length;
        //        ftpRequest.Timeout = 1000000*90;
        //        // Copy the contents of the file to the request stream.
        //        Stream targetStream = null;
        //        Stream sourceStream = request.FileByteStream;
        //        using (targetStream = ftpRequest.GetRequestStream())
        //        {
        //            //read from the input stream in 6K chunks
        //            //and save to output stream
        //            const int bufferLen = 65000;
        //            byte[] buffer = new byte[bufferLen];
        //            int count = 0;
        //            while ((count = sourceStream.Read(buffer, 0, bufferLen)) > 0)
        //            {
        //                targetStream.Write(buffer, 0, count);
        //            }
        //            targetStream.Close();
        //            sourceStream.Close();
        //        }
        //        FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
        //        //Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);
        //        response.Close();
        //        //OperationContext context = OperationContext.Current;
        //        //context.OperationCompleted += (sender, args) => response.Dispose();
        //        //return response.StatusDescription;
            
        //        //ManualResetEvent waitObject;

        //        //FtpState state = new FtpState();

        //        //// Store the request in the object that we pass into the 
        //        //// asynchronous operations.
        //        //state.Request = ftpRequest;
        //        //state.FileName = request.FileName;

        //        //// Get the event to wait on.
        //        //waitObject = state.OperationComplete;

        //        //// Asynchronously get the stream for the file contents.
        //        //ftpRequest.BeginGetRequestStream(
        //        //    new AsyncCallback(EndGetStreamCallback),
        //        //    state
        //        //);

        //        //// Block the current thread until all operations are complete.
        //        //waitObject.WaitOne();

        //        //// The operations either completed or threw an exception. 
        //        //if (state.OperationException != null)
        //        //{
        //        //    throw state.OperationException;
        //        //}
        //        //else
        //        //{
        //        //    Console.WriteLine("The operation completed - {0}", state.StatusDescription);
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        throw ex;
        //    }
        //}
        #endregion

        //private void EndGetStreamCallback(IAsyncResult ar)
        //{
        //    FtpState state = (FtpState)ar.AsyncState;

        //    Stream requestStream = null;
        //    // End the asynchronous call to get the request stream. 
        //    try
        //    {
        //        requestStream = state.Request.EndGetRequestStream(ar);
        //        // Copy the file contents to the request stream. 
        //        const int bufferLength = 2048;
        //        byte[] buffer = new byte[bufferLength];
        //        int count = 0;
        //        int readBytes = 0;
        //        FileStream stream = File.OpenRead(state.FileName);
        //        do
        //        {
        //            readBytes = stream.Read(buffer, 0, bufferLength);
        //            requestStream.Write(buffer, 0, readBytes);
        //            count += readBytes;
        //        }
        //        while (readBytes != 0);
        //        Console.WriteLine("Writing {0} bytes to the stream.", count);
        //        // IMPORTANT: Close the request stream before sending the request.
        //        requestStream.Close();
        //        // Asynchronously get the response to the upload request.
        //        state.Request.BeginGetResponse(
        //            new AsyncCallback(EndGetResponseCallback),
        //            state
        //        );
        //    }
        //    // Return exceptions to the main application thread. 
        //    catch (Exception e)
        //    {fdsfef
        //        Console.WriteLine("Could not get the request stream.");
        //        state.OperationException = e;
        //        state.OperationComplete.Set();
        //        return;
        //    }

        //}
         
        //// The EndGetResponseCallback method   
        //// completes a call to BeginGetResponse. 
        //private void EndGetResponseCallback(IAsyncResult ar)
        //{
        //    FtpState state = (FtpState)ar.AsyncState;
        //    FtpWebResponse response = null;
        //    try
        //    {
        //        response = (FtpWebResponse)state.Request.EndGetResponse(ar);
        //        response.Close();
        //        state.StatusDescription = response.StatusDescription;
        //        // Signal the main application thread that  
        //        // the operation is complete.
        //        state.OperationComplete.Set();
        //    }
        //    // Return exceptions to the main application thread. 
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Error getting response.");
        //        state.OperationException = e;
        //        state.OperationComplete.Set();
        //    }
        //}

        //private bool RefreshDirectory(string DirectoryName)
        //{
        //    try
        //    {
        //        DirectoryName = DirectoryName.Replace('\\', '/');
        //        string[] folderlist = DirectoryName.Split('/');
        //        string IMGURL = "ftp://" + ConfigurationManager.AppSettings["serverURL"] + "/" + ConfigurationManager.AppSettings["IMGURL"];
        //        foreach (string folder in folderlist)
        //        {
        //            IMGURL += "/" + folder;
        //            FtpWebRequest request = CreateFTPRequest(IMGURL);
        //            request.Method = WebRequestMethods.Ftp.MakeDirectory;

        //            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
        //        }
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
    }
}
