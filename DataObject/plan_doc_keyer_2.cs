using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataObject
{
    public class plan_doc_keyer_2
    {
        public long id { get; set; }
        public long docid { get; set; }
        public string doctype { get; set; }
        public DateTime create_date { get; set; }
        public string lockedby { get; set; }
        public DateTime lockeddate { get; set; }
        public bool lockedfinish { get; set; }
    }
}
