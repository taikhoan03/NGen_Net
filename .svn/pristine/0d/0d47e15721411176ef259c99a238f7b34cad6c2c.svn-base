using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DataObject
{
    public class Download_File
    {
        #region "Serialize"


        private static XmlSerializer serializer = new XmlSerializer(typeof(Download_File));
        private static XmlSerializer serializerList = new XmlSerializer(typeof(List<Download_File>));
        public static List<Download_File> mapListObject(string xml)
        {
            if (String.IsNullOrEmpty(xml)) return null;

            object result;
            using (System.IO.TextReader reader = new System.IO.StringReader(xml))
            {
                result = serializerList.Deserialize(reader);
            }
            return (List<Download_File>)result;
        }

        public static Download_File mapObject(string xml)
        {
            if (String.IsNullOrEmpty(xml)) return null;

            object result;
            using (System.IO.TextReader reader = new System.IO.StringReader(xml))
            {
                result = serializer.Deserialize(reader);
            }
            return (Download_File)result;
        }

        public static string ToXMLListString(List<Download_File> list)
        {
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(stringWriter);
            serializerList.Serialize(writer, list);
            return stringWriter.ToString();
        }

        public static string ToXMLString(Download_File downloadfile)
        {
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(stringWriter);
            serializer.Serialize(writer, downloadfile);
            return stringWriter.ToString();
        }

        public string ToXMLString()
        {
            var entity = this;
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(stringWriter);
            serializer.Serialize(writer, entity);
            return stringWriter.ToString();
        }
        #endregion

        public int ID { get; set; }
        public string File_name { get; set; }
        public string File_Path { get; set; }
        public int County_ID { get; set; }
        public string Description { get; set; }
        public DateTime Download_date { get; set; }
        public DateTime Create_date { get; set; }
        public DateTime Modified_date { get; set; }
        public string First_doc_num { get; set; }
        public string Last_doc_num { get; set; }
        public DateTime First_record_date { get; set; }
        public DateTime Last_record_date { get; set; }
        public string Film_tracker_ID { get; set; }
        public int Month_of_county { get; set; }
        public int Year_of_county { get; set; }
        public int Batch_num { get; set; }
        public string State_code { get; set; }
        public int Priority { get; set; }

        public bool AlreadyImported { get; set; }
        public bool Set_Name_As_Doc_ID { get; set; }
    }
}
