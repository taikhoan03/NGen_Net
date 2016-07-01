using System;

namespace DataObject
{
    public class PackageImport:ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set ; }
        public string File_path { get; set ; }
        public string Create_by { get; set; }
        public DateTime Create_date { get; set; }
        public string Doctype { get; set; }
        public string Film_tracker_id { get; set; }
        public int Priority { get; set; }
        public bool AlreadyImported { get; set; }
        public string CSV_name { get; set; }
        public bool Edited { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
