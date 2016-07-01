using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DataObject.Util
{
    public class Encrypt
    {
        private static Byte[] encodedBytes;
        private static MD5 md5 = new MD5CryptoServiceProvider();
        private static Byte[] originalBytes;
        public static string EncodePassword(string originalPassword)
        {
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);

            //Convert encoded bytes back to a 'readable' string
            string Result = String.Join("", encodedBytes.Select(p => p.ToString("x2")).ToArray());
            return Result;
        }
    }
    public class Tools
    {
        private static readonly Object locker = new Object();
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static System.Collections.Generic.List<String> readCSVLines(string path)
        {
            lock (locker)
            {
                System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read,System.IO.FileShare.ReadWrite);  
 
                System.IO.StreamReader sr = new System.IO.StreamReader(fs);  
 
                System.Collections.Generic.List<String> lst = new System.Collections.Generic.List<string>();

                while (!sr.EndOfStream)
                    lst.Add(sr.ReadLine());
                sr.Close();
                //sr.Dispose();
                return lst;
            }
        }
        
    }
}