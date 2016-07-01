using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace DataObject.Util
{
    public static class MyFile
    {
        public static byte[] convertStreamToByteArray(Stream input)
        {
            //byte[] buffer = new byte[16 * 1024];
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    int read;
            //    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            //    {
            //        ms.Write(buffer, 0, read);
            //    }
            //    return ms.ToArray();
            //}

            using (var memoryStream = new MemoryStream())
            {
                input.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
