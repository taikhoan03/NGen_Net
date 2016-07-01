using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataObject.MergedObject
{
    public class DocumentExport:Document
    {
        public int Sr_no { get; set; }
        public string Item_type { get; set; }
        public int Qty { get; set; }
        public string Rin { get; set; }
        public string Rsd { get; set; }
        public string Upc { get; set; }
        public string Rin_upc_rsd_description { get; set; }
        public double Price { get; set; }
        public double Per_Item { get; set; }
        public string Rejection_reason { get; set; }
        public string Worker_id { get; set; }
        public string Item_number { get; set; }
    }
}
