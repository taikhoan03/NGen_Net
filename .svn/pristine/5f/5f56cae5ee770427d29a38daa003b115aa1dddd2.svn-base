using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;

namespace NexGen.Common
{
    public class OCRObject
    {
        int _Page;
        public int Page
        {
            get { return _Page; }
            set { _Page = value; }
        }

        int _Line;
        public int Line
        {
            get { return _Line; }
            set { _Line = value; }
        }



        int _MarginLeft;
        public int MarginLeft
        {
            get { return _MarginLeft; }
            set { _MarginLeft = value; }
        }

        int _MarrginRight;
        public int MarrginRight
        {
            get { return _MarrginRight; }
            set { _MarrginRight = value; }
        }

        int _Width;
        public int Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        int _Height;
        public int Height
        {
            get { return _Height; }
            set { _Height = value; }
        }

        string _Text;
        public string Text
        {
            get { return _Text; }
            set { _Text = value; }
        }

        bool _IsBold;
        public bool IsBold
        {
            get { return _IsBold; }
            set { _IsBold = value; }
        }

        bool _IsCapital;
        public bool IsCapital
        {
            get { return _IsCapital; }
            set { _IsCapital = value; }
        }

        bool _IsBigSize;
        public bool IsBigSize
        {
            get { return _IsBigSize; }
            set { _IsBigSize = value; }
        }

        bool _IsMiddle;
        public bool IsMiddle
        {
            get { return _IsMiddle; }
            set { _IsMiddle = value; }
        }

        OCRObject _Next;
        internal OCRObject Next
        {
            get { return _Next; }
            set { _Next = value; }
        }

        OCRObject _Previous;
        internal OCRObject Previous
        {
            get { return _Previous; }
            set { _Previous = value; }
        }

        public OCRObject()
        {
            _Page = 0;
            _Line = 0;
            _MarginLeft = 0;
            _MarrginRight = 0;
            _Width = 0;
            _Height = 0;
            _Text = "";
            _Next = null;
            _Previous = null;
            _IsBold = true;
            _IsCapital = true;
        }

    }
}
