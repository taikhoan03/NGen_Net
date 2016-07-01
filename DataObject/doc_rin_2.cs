using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataObject
{
    public class doc_rin_2
    {
        public long id { get; set; }
        public string transcription_type { get; set; }
        public double total { get; set; }
        public DateTime transaction_date { get; set; }
        public string sr_no { get; set; }
        public string type { get; set; }
        public int qty { get; set; }
        public string item_number { get; set; }
        public double price { get; set; }
        public double per_item { get; set; }
        public string remarks { get; set; }
        public string receipt_images { get; set; }
        public string image_2 { get; set; }
        public string role_author { get; set; }
        public long docid { get; set; }
        public DateTime created_date { get; set; }
        public string item_type { get; set; }
    }
}
