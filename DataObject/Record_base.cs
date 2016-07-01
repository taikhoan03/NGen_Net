using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace DataObject
{
    [Serializable]
    public class Record_base : ICloneable, INotifyPropertyChanged 
    {
    //    var ser = new XmlSerializer(typeof(Record_base),
    //new[] { typeof(Record_RIN), typeof(Record_RSD), typeof(Record_UPC) });
    //    public string ToXMLString()
    //    {
    //        var entity = this;
    //        System.IO.StringWriter stringWriter = new System.IO.StringWriter();
    //        System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(stringWriter);
    //        ser.Serialize(writer, entity);
    //        return stringWriter.ToString();
    //    }
        public long Id { get; set; }

        public long Docid { get; set; }

        public string Transaction_Type { get; set; }

        public string Total { get; set; }

        [XmlIgnore]
        public DateTime Transaction_Date { get; set; }

        public int Sr_No { get; set; }

        public string Type { get; set; }

        private int _qty;

        public int Qty
        {
            get
            {
                return _qty;
            }
            set
            {
                _qty = value;
                this.NotifyPropertyChanged("Qty");
            }
        }
        private double _price;

        public double Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
                this.NotifyPropertyChanged("Price");
            }
}
        private double _per_Item;

        public double Per_Item
        {
            get
            {
                return _per_Item;
            }
            set
            {
                _per_Item = value;
                this.NotifyPropertyChanged("Per_Item");
            }
}

        public string Remarks { get; set; }

        public string Role_author { get; set; }

        public string Item_type { get; set; }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        
        public object Clone()
        {
            // TODO: Implement this method
            return this.MemberwiseClone();
            //throw new NotImplementedException();
        }
    }
}