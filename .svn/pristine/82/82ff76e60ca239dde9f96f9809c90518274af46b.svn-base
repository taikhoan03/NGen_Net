using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Libs;

namespace DataObject
{
    public class DocCounter
    {
        public string Date { get; set; }

        public int Num_of_doc_discard { get; set; }

        public int Num_of_doc { get; set; }

        public int Num_of_records { get; set; }

        private const string prefix_name = "-counter";

        public DocCounter()
        {
        }

        public void Load()
        {
            DataObject.User user = DataObject.User.Get_authenticated_user();
            string filename = user.Username + prefix_name;
            if (File.Exists(filename))
            {
                try
                {
                    var doc_counter = File.ReadAllText(filename).XMLStringToObject<DocCounter>();
                    if (doc_counter.Date == DateTime.Now.ToShortDateString())
                    {
                        this.Date = DateTime.Now.ToShortDateString();
                        this.Num_of_doc = doc_counter.Num_of_doc;
                        this.Num_of_doc_discard = doc_counter.Num_of_doc_discard;
                        this.Num_of_records = doc_counter.Num_of_records;
                        return;
                    }

                    
                }
                catch (Exception ex)
                {
                }
            }
            this.Date = DateTime.Now.ToShortDateString();
            this.Num_of_doc = 0;
            this.Num_of_doc_discard = 0;
            this.Num_of_records = 0;
        }

        public void Save()
        {
            DataObject.User user = DataObject.User.Get_authenticated_user();
            string filename = user.Username + prefix_name;
            File.WriteAllText(filename, this.XmlSerialize());
        }
    }
}