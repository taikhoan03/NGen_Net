using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Spring.Context;
using Spring.Context.Support;
using log4net;
using System.Text.RegularExpressions;

namespace NexGen
{
    public class Downloader
    {
        public static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private static IApplicationContext _context;
        //public static IApplicationContext GetContext()
        //{
        //    if (_context == null)
        //    {
        //        try
        //        {
        //            _context = new XmlApplicationContext("cache-config.xml");
        //        }
        //        catch (Exception e)
        //        {
        //            string error = e.Message;
        //        }
        //    }
        //    return _context;
        //}
        public static void Run(string[] args)
        {
            //test
            //var b = Limilabs.FTP.Licensing.LicenseHelper.GetLicensePath();
            //var a=Limilabs.FTP.Licensing.LicenseHelper.GetLicenseStatus();
            if (args.Count() == 0)
            {
                log.Error("This is my error", new Exception("Please input parameter"));
                //throw new Exception("Please input parameter");
                return;
            }
            Console.WriteLine("Download Param: " + args[0]);
            var listFile = args[0].Split(new char[] { ';' });
            Console.WriteLine("List files:");
            //var context = (Downloader)Clore_WinForm.ApplicationContextFactory.GetContext().GetObject("ChangeInputValueToShortWrite");;
            var ftp = (FTPHelper)ApplicationContextFactory.GetContext().GetObject("FTPHelper");//context.GetObject("FTPHelper");
            var path = get_dir();
            //+ download files (user documents)
            Console.WriteLine("Downloading...");
            var arr_file_download = new string[listFile.Count() * 2];
            var arr_file_output = new string[listFile.Count() * 2];
            int i = 0;
            try
            {
                foreach (var file in listFile)
                {
                    Console.WriteLine("Image: " + "Resource/IMG/" + file);
                    arr_file_download[i] = "Resource/IMG/" + file;
                    arr_file_output[i] = path + "/IMG/" + file;
                    i++;

                    var fileExtention = Path.GetExtension(file);
                    var regex = new Regex(fileExtention, RegexOptions.IgnoreCase);
                    //var newSentence = regex.Replace( sentence, "horse" );
                    var xml_file_name = regex.Replace(file, ".xml");
                    arr_file_download[i] = "Resource/OCR/" + xml_file_name;
                    arr_file_output[i] = path + "/OCR/" + xml_file_name;
                    i++;
                    //try
                    //{
                    //    ftp.Download("Resource/IMG/" + file, path + "/IMG/" + file);
                    //    //log.Info("Image: " + "Resource/IMG/" + file + " success");
                    //    Console.WriteLine("Success");
                    //}
                    //catch (Exception ex)
                    //{
                    //    log.Error("Image: " + "Resource/IMG/" + file + " Fail");
                    //    log.Error(ex.Message);
                    //    log.Error(ex.StackTrace);
                    //}
                    ////+ download ocr file
                    //var fileExtention = Path.GetExtension(file);
                    //var regex = new Regex( fileExtention, RegexOptions.IgnoreCase );
                    ////var newSentence = regex.Replace( sentence, "horse" );
                    //var xml_file_name = regex.Replace(file, ".xml");
                    ////var newSentence = regex.Replace( sentence, "horse" );
                    ////var xml_file_name = file.Replace(".TIF", ".xml");
                    //Console.WriteLine("OCR: " + "Resource/IMG/" + xml_file_name);
                    //try
                    //{
                    //    ftp.Download("Resource/OCR/" + xml_file_name, path + "/OCR/" + xml_file_name);
                    //    //log.Info("Image: " + "Resource/IMG/" + file + " success");
                    //    Console.WriteLine("Success");
                    //}
                    //catch (Exception ex) {
                    //    log.Error("OCR: " + "Resource/OCR/" + file + " Fail");
                    //    log.Error(ex.Message);
                    //    log.Error(ex.StackTrace);
                    //}
                }
                ftp.Download(arr_file_download, arr_file_output);
            }
            catch (Exception ex)
            {
                //log.Error("OCR: " + "Resource/OCR/" + file + " Fail");
                log.Error(ex.Message);
                log.Error(ex.StackTrace);
            }
            
            
        }

        private static string app_path = Directory.GetCurrentDirectory();

        private static string get_dir()
        {
            string dir_name = "cache";
            string path = app_path + "/" + dir_name;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}
