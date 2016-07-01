using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
namespace NexGen
{
    public class Download_Doc_Data
    {
        public static void StartDownload(string param,bool waitProcessComplete=false)
        {
            Process process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = "cmd.exe";
            //process.StartInfo.Arguments = "/c D:\\Projects\\Clore\\_new\\CacheImage\\CacheImage\\CacheImage\\bin\\Debug\\CacheImage.exe " + param;
            process.StartInfo.Arguments = "/c doc_downloader\\CacheImage.exe " + param;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            process.StartInfo.UseShellExecute = false;

            process.StartInfo.CreateNoWindow = true;

            process.StartInfo.LoadUserProfile = true;

            process.StartInfo.RedirectStandardError = true;

            process.StartInfo.RedirectStandardInput = true;

            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            if (waitProcessComplete)
                process.WaitForExit();// Waits here for the process to exit.
        }
    }
}
