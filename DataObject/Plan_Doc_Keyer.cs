using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataObject
{
    public class Plan_Doc_Keyer
    {
        public int Id { get; set; }
        public int Docid { get; set; }
        public string Doctype { get; set; }
        public DateTime Create_date { get; set; }
        public string Lockedby { get; set ; }
        public DateTime lockeddate { get; set; }
        public bool lockedfinish { get; set; }
        //public bool is_deleted { get; set; }

    }
}
