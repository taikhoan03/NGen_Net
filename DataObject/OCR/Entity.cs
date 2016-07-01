using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libs;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
namespace DataObject.OCR{
    [Serializable]
    public class CoordinateList
    {
        //[XmlArray("Pages")]
        //[XmlArrayItem("Page")]
        [XmlElement("Page")]
        public List<Page> Pages { get; set; }

        public Page GetPage(int page_num)
        {
            return Pages.FirstOrDefault(p => p.Nb == page_num);
        }
        public Entity GetEntity(int page_num, int x, int y)
        {
            var page = GetPage(page_num);
            if (page == null) return null;
            foreach(var line in page.Lines)
            {
                foreach(var entity in line.Entities)
                {
                    if (entity.Left < x && (entity.Left + entity.Width) > x &&
                        entity.Top < y && (entity.Top + entity.Height) > y)
                        return entity;
                }
            }
            return null;
        }
        public Entity GetEntity(int pageindex, string content, int x=0, int y=0)
        {
            var page = GetPage(pageindex);
            if (page == null) return null;
            foreach(var line in page.Lines)
            {
                foreach(var entity in line.Entities)
                {
                    if (entity.String==content)
                        if (entity.Left < x && (entity.Left + entity.Width) > x &&
                        entity.Top < y && (entity.Top + entity.Height) > y)
                            return entity;
                }
            }
            return null;
        }
        private List<Entity> listEntities;

        private void flatEntities()
        {
            if (listEntities == null)
                listEntities = new List<Entity>();
            foreach(var page in this.Pages)
            {
                foreach(var line in page.Lines)
                {
                    foreach(var entity in line.Entities)
                    {
                        listEntities.Add(entity);
                    }
                }
            }
        }
        public List<Entity> Get_List_Entity_Flatted()
        {
            if (listEntities == null)
                flatEntities();
            return listEntities;
        }
    }
    //public class Pages : List<Page> { }

    [Serializable]
    public class Page{
        //[XmlArray("Lines")]
        //[XmlArrayItem("Line")]
        [XmlElement("Line")]
        public List<Line> Lines { get; set; }
        [XmlAttribute("nb")]
        public int Nb{get;set;}
    }
    //public class Lines : List<Line> { }
    [Serializable]
    public class Line
    {
        //[XmlArray("Entities")]
        //[XmlArrayItem("Entity")]
        [XmlElement("Entity")]
        public List<Entity> Entities { get; set; }
        [XmlAttribute("nb")]
        public int Nb{get;set;}
    }

    //public class Entities : List<Entity> { }
    [Serializable]
    public class Entity:ICloneable
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public string String { get; set; }
        private string _coordinate;
        public string Coordinate { 
            get {
                
                return _coordinate;
            }
            set {
                _coordinate = value;
                try
                {
                    var attr = this._coordinate.Split(new char[] { '-' });
                    try
                    {
                        this.Left = int.Parse(attr[0]);
                        this.Top = int.Parse(attr[1]);
                        this.Width = int.Parse(attr[2]);
                        this.Height = int.Parse(attr[3]);
                    }
                    catch (Exception) { }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    throw ex;
                }
                
            }
        }
        public string ConfidenceRateList { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

    }
}
