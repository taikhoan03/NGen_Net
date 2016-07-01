using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using System.IO;

namespace NexGen.Common
{
    public class OCRDocument
    {
        string _FileName;
        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }
        
        string _FilePath;
        public string FilePath
        {
            get { return _FilePath; }
            set { _FilePath = value; }
        }

        int _ImgWidth;
        public int ImgWidth
        {
            get { return _ImgWidth; }
            set { _ImgWidth = value; }
        }

        XmlDocument _Doc;
        public XmlDocument Doc
        {
            get { return _Doc; }
            set { _Doc = value; }
        }

        XmlNode _Root;
        public XmlNode Root
        {
            get { return _Root; }
            set { _Root = value; }
        }

        int _Page;
        public int Page
        {
            get { return _Page; }
            set { _Page = value; }
        }

        public OCRDocument()
        {
            _Doc = new XmlDocument();
            _FileName = "";
            _FilePath = "";
            _ImgWidth = 0;
        }

        public void Load()
        {
            if (_FilePath == "") _FilePath = "D:/";
            string Path = _FilePath + "\\" + _FileName;
            if (File.Exists(Path))
            {
                _Doc.Load(Path);

                if (_Doc.ChildNodes.Count > 0)
                {
                    _Root = _Doc.ChildNodes[1];
                    _Page = Root.ChildNodes.Count;
                }
            }
            else
            {
                _Doc = new XmlDocument();
                _Page = 0;
                _Root = null;
            }
        }

        public void LoadFromStream(Stream frmStream)
        {
            if (frmStream != null)
            {
                _Doc.Load(frmStream);

                if (_Doc.ChildNodes.Count > 0)
                {
                    _Root = _Doc.ChildNodes[1];
                    _Page = Root.ChildNodes.Count;
                }
            }
            else
            {
                _Doc = new XmlDocument();
                _Page = 0;
                _Root = null;
            }
        }

        public void Save()
        {
            string SFileName = "Result_" + FileName;
            string Path = _FilePath + SFileName;
            _Doc.Save(Path);
        }

        public List<OCRObject> LoadOCRObject()
        {
            List<OCRObject> _OCRList = new List<OCRObject>();
            if (Root.ChildNodes.Count > 0)
                //foreach (XmlNode N in Root.ChildNodes) //Search Page node in XML
                for (int J = 0; J < Root.ChildNodes.Count; J++)
                {
                    XmlNode CurrentPage = Root.ChildNodes[J];
                    if (CurrentPage.ChildNodes.Count == 0) continue;
                    double AvgBold = BoldFactor(CurrentPage);
                    OCRObject tmp = new OCRObject();
                    tmp.Page = J;
                    for (int j = 0; j < CurrentPage.ChildNodes.Count; j++)
                    {
                        XmlNode L = CurrentPage.ChildNodes[j];
                        tmp.Line = j;
                        if (L.ChildNodes.Count == 0) continue;
                        tmp.MarginLeft = GetBeginX(L.ChildNodes[0]);
                        for (int i = 0; i < L.ChildNodes.Count; i++)
                        {
                            XmlNode E = L.ChildNodes[i];
                            string Str = (E.ChildNodes[0].FirstChild).InnerText;
                            Str = RemoveTrash(Str);

                            if (Str != "")
                            {
                                tmp.Text += Str + " ";
                                tmp.Width += GetWidth(E);
                                tmp.Height = getHeight(L.ChildNodes[0]);
                                if (IsBold(E, AvgBold) == false) tmp.IsBold = false;
                                if (IsCapital(E) == false) tmp.IsCapital = false;
                            }

                            if (i < L.ChildNodes.Count - 1)
                            {
                                int Distance = GetBeginX(L.ChildNodes[i + 1]) - GetEndX(E);
                                if (Distance > (tmp.Height * 2)) //Nếu khoảng cách Entity sau và Entity hiện tại quá lớn
                                {
                                    if (tmp.Text.Length > 1)
                                    {
                                        tmp.Text = tmp.Text.Remove(tmp.Text.Length - 1);
                                        tmp.MarrginRight = Distance;
                                        _OCRList.Add(tmp);
                                    }
                                    tmp = new OCRObject();
                                    tmp.MarginLeft = Distance;
                                    tmp.Height = getHeight(Root.ChildNodes[0]);
                                }
                            }
                            if (i == L.ChildNodes.Count - 1)
                            {
                                if (tmp.Text.Length > 1)
                                {
                                    tmp.Text = tmp.Text.Remove(tmp.Text.Length - 1);
                                    tmp.MarrginRight = _ImgWidth - GetEndX(L.ChildNodes[i]);
                                    _OCRList.Add(tmp);
                                }
                                tmp = new OCRObject();
                            }
                        }

                    }

                }
            return _OCRList;
        }



        private string RemoveTrash(string Str)
        {
            string value = Str;
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                if ((c >= 'a' && c < 'z') || (c >= 'A' && c < 'Z') || (c >= '0' && c < '9'))
                    break;
                else
                    value = value.Remove(i, 1);
            }
            if (value.Length == 0) return value;

            for (int i = value.Length - 1; i >= 0; i--)
            {
                char c = value[i];
                if ((c >= 'a' && c < 'z') || (c >= 'A' && c < 'Z') || (c >= '0' && c < '9'))
                    break;
                else
                    value = value.Remove(i, 1);
            }
            return value;
        }

        private double BoldFactor(XmlNode Page) //Lấy hệ số trung bình của ký tự Bold
        {
            double chars = 0;
            double length = 0;
            XmlNodeList L = ((XmlElement)Page).GetElementsByTagName("Entity");
            foreach (XmlNode E in L)
            {
                chars += (E.ChildNodes[0].FirstChild).InnerText.Length;
                length += GetWidth(E);
            }
            return length/chars;
        }

        private bool IsBold(XmlNode Entity, double AvgBold)
        {
            double chars = GetContent(Entity).Length;
            double length = GetWidth(Entity);
            if (length / chars > AvgBold) return true;
            return false;
        }

        private bool IsCapital(XmlNode Entity)
        {
            string Text = GetContent(Entity);
            if (string.IsNullOrEmpty(Text))
                return false;
            for (int i = 0; i < Text.Length; i++)
            {
                if (Text[i] > 'a' && Text[i]<'z')
                    return false;
            }
            return true;
        }

        public static string GetContent(XmlNode E)
        {
            return (E.ChildNodes[0].FirstChild).InnerText;
        }

        public static int GetBeginX(XmlNode E)
        {
            return int.Parse((E.ChildNodes[1].FirstChild).InnerText.Split('-')[0]);
        }

        public static int GetWidth(XmlNode E)
        {
            return int.Parse((E.ChildNodes[1].FirstChild).InnerText.Split('-')[2]);
        }

        public static int GetEndX(XmlNode E)
        {
            return GetBeginX(E) + GetWidth(E);
        }

        public static int GetBeginY(XmlNode E)
        {
            return int.Parse((E.ChildNodes[1].FirstChild).InnerText.Split('-')[1]);
        }

        public static int getHeight(XmlNode E)
        {
            try
            {
                return int.Parse((E.ChildNodes[1].FirstChild).InnerText.Split('-')[3]);
            }
            catch
            {
                return 0;
            }
        }

        public static int GetEndY(XmlNode E)
        {
            return GetBeginY(E) + getHeight(E);
        }
    }
}
