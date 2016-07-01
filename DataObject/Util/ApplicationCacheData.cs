using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libs;
using System.IO;
using System.Reflection;
namespace DataObject.Util
{
    [Serializable]
    public class ApplicationCacheData
    {
        private static readonly string cacheFile = System.IO.Directory.GetCurrentDirectory()+"\\userdata.txt";
        private static string _LastImportedDir { get; set; }
        public string LastImportedDir { get { return _LastImportedDir; } set { _LastImportedDir = value; } }
        private static string _UsernameLogin { get; set; }
        public string UsernameLogin { get { return _UsernameLogin; } set { _UsernameLogin = value; } }        
        

        public static ApplicationCacheData Init()
        {
            
            if (File.Exists(cacheFile))
            {
                var strdata = File.ReadAllText(cacheFile);
                var rs = strdata.XMLStringToObject<ApplicationCacheData>();
                if(rs!=null)
                    return rs;
            }
            return new ApplicationCacheData();
        }
        public void set(string propertyName, string value)
        {
            
            PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static );
            foreach (var property in properties)
            {
                if (property.Name == "_" + propertyName)
                {
                    property.SetValue(this, Convert.ChangeType(value, property.PropertyType), null);
                }
            }
            File.WriteAllText(cacheFile, this.XmlSerialize());
            
        }
    }
}
