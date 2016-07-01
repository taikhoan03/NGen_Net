using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using log4net;
using System.Net.FtpClient;

namespace NexGen
{
    class FTPHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string Host;
        private string Username = "detoolv2";
        private string Password = "UrgRu1";
        private string PathToSave;

        public void Download(string path, string outputFile)
        {
            try
            {
                //match file name ".xyz"
                outputFile = outputFile.Replace("/", "\\");
                //+ kiểm tra folder đã tồn tại chưa, tạo nếu chưa có
                var regex = new Regex(@"[^\\]*$");
                var folder_path = regex.Replace(outputFile, string.Empty);
                if (!Directory.Exists(folder_path))
                {
                    Directory.CreateDirectory(folder_path);
                    log.Info("Create Folder: " + folder_path);
                }
                //+ kiểm tra file đã có chưa, nếu có rồi thì ko download nữa
                if (File.Exists(outputFile))
                {
                    log.Info("File download requested is existed -> ignore (" + outputFile + ")");
                    return; 
                }
                using (var client = new FtpClient())
                {
                    client.Host = Host;
                    client.Port = 21;
                    client.Credentials = new System.Net.NetworkCredential(Username, Password);
                    client.DataConnectionType = FtpDataConnectionType.AutoPassive;
                    client.EncryptionMode = FtpEncryptionMode.None;
                    client.DataConnectionEncryption = true;
                    client.Connect();
                    using (var ftpStream = client.OpenRead(path))
                    {
                        using (var fileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
                        {
                            ftpStream.CopyTo(fileStream);
                        }
                    }
                    
                    log.Info("Download file: " + outputFile + " success");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw ex;
            }
        }

        public void Download(string[] path, string[] output_arr)
        {
            try
            {
                using (var client = new FtpClient())
                {
                    client.Host = Host;
                    client.Port = 21;
                    client.Credentials = new System.Net.NetworkCredential(Username, Password);
                    client.DataConnectionType = FtpDataConnectionType.AutoPassive;
                    client.EncryptionMode = FtpEncryptionMode.None;
                    client.DataConnectionEncryption = true;
                    client.Connect();
                    for (int i = 0; i < path.Length; i++)
                    {
                        //match file name ".xyz"
                        string outputFile = output_arr[i].Replace("/", "\\");
                        //+ kiểm tra folder đã tồn tại chưa, tạo nếu chưa có
                        var regex = new Regex(@"[^\\]*$");
                        var folder_path = regex.Replace(outputFile, string.Empty);
                        if (!Directory.Exists(folder_path))
                        {
                            Directory.CreateDirectory(folder_path);
                            log.Info("Create Folder: " + folder_path);
                        }
                        //+ kiểm tra file đã có chưa, nếu có rồi thì ko download nữa
                        if (File.Exists(outputFile))
                        {
                            log.Info("File download requested is existed -> ignore (" + outputFile + ")");
                            return; 
                        }
                    
                        using (var ftpStream = client.OpenRead(path[i]))
                        {
                            using (var fileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
                            {
                                ftpStream.CopyTo(fileStream);
                            }
                        }
                    
                        log.Info("Download file: " + outputFile + " success");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}