using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataObject
{
    public class Record_UPC:Record_base
    {
        public string Receipt_Id { get; set; }
        public string Banner_id { get; set; }
        public string Upc_code { get; set; }
    }
}
