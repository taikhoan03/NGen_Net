using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataObject
{
    public class PackageAssignment
    {
        public long id { get; set; }
        public int packageid { get; set; }
        public string username { get; set; }
        public DateTime created_date { get; set; }
        public bool is_enabled { get; set; }
        public int priority { get; set; }
    }
}
