using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DataObject.OCR;
using Libs;
namespace DataObject.OCR
{
    public class OCRCollectData
    {
        private CoordinateList OCRData;
        private Dictionary<string, List<string>> hashData=new Dictionary<string,List<string>>();
        private Regex regex_DocNumber = new Regex("^([0-9]{4,})$");//co 4 so tro len
        private Regex regex_date = new Regex("^([0-9]{8,})$");//co 4 so tro len
        public const string DOC_NUM = "DOC_NUM";
        public const string DATE = "DATE";

        public Dictionary<string, List<string>> getData()
        {
            return hashData;
        }
        public OCRCollectData(CoordinateList ocr_data)
        {
            hashData = new Dictionary<string, List<string>>();
            OCRData = ocr_data;
            collectData();
        }
        public OCRCollectData(string ocr_data)
        {
            hashData = new Dictionary<string, List<string>>();
            OCRData = ocr_data.XmlSerialize().XMLStringToObject<DataObject.OCR.CoordinateList>();
            collectData();
        }
        private void collectData()
        {
            List<string> possibleDocNum = new List<string>();
            List<string> possibleDocDate = new List<string>();
            foreach(var page in OCRData.Pages)
            {
                foreach(var line in page.Lines)
                {
                    foreach(var entity in line.Entities)
                    {
                        //doc num
                        if(regex_DocNumber.IsMatch(entity.String))
                        {
                            possibleDocNum.Add(entity.String);
                        }
                        //date
                        if (regex_date.IsMatch(entity.String))
                        {
                            possibleDocDate.Add(entity.String);
                        }
                    }
                }
            }
            if (possibleDocNum.Count>0)
                hashData.Add(DOC_NUM, possibleDocNum);
            if(possibleDocDate.Count>0)
                hashData.Add(DATE, possibleDocNum);
        }
    }
}
