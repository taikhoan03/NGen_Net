﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace NexGenFlow.NexGen
{

    public partial class doc_rin_rsd : XPLiteObject
    {
        long fid;
        [Key]
        public long id
        {
            get { return fid; }
            set { SetPropertyValue<long>("id", ref fid, value); }
        }
        long fdocid;
        public long docid
        {
            get { return fdocid; }
            set { SetPropertyValue<long>("docid", ref fdocid, value); }
        }
        string ftranscription_type;
        [Size(50)]
        public string transcription_type
        {
            get { return ftranscription_type; }
            set { SetPropertyValue<string>("transcription_type", ref ftranscription_type, value); }
        }
        double ftotal;
        public double total
        {
            get { return ftotal; }
            set { SetPropertyValue<double>("total", ref ftotal, value); }
        }
        DateTime ftransaction_date;
        public DateTime transaction_date
        {
            get { return ftransaction_date; }
            set { SetPropertyValue<DateTime>("transaction_date", ref ftransaction_date, value); }
        }
        string fsr_no;
        [Size(255)]
        public string sr_no
        {
            get { return fsr_no; }
            set { SetPropertyValue<string>("sr_no", ref fsr_no, value); }
        }
        string ftype;
        [Size(50)]
        public string type
        {
            get { return ftype; }
            set { SetPropertyValue<string>("type", ref ftype, value); }
        }
        int fqty;
        public int qty
        {
            get { return fqty; }
            set { SetPropertyValue<int>("qty", ref fqty, value); }
        }
        string fitem_number;
        [Size(50)]
        public string item_number
        {
            get { return fitem_number; }
            set { SetPropertyValue<string>("item_number", ref fitem_number, value); }
        }
        string fdescription;
        [Size(500)]
        public string description
        {
            get { return fdescription; }
            set { SetPropertyValue<string>("description", ref fdescription, value); }
        }
        double fprice;
        public double price
        {
            get { return fprice; }
            set { SetPropertyValue<double>("price", ref fprice, value); }
        }
        double fper_item;
        public double per_item
        {
            get { return fper_item; }
            set { SetPropertyValue<double>("per_item", ref fper_item, value); }
        }
        string fremarks;
        [Size(255)]
        public string remarks
        {
            get { return fremarks; }
            set { SetPropertyValue<string>("remarks", ref fremarks, value); }
        }
        string freceipt_images;
        [Size(255)]
        public string receipt_images
        {
            get { return freceipt_images; }
            set { SetPropertyValue<string>("receipt_images", ref freceipt_images, value); }
        }
        string fimage_2;
        [Size(255)]
        public string image_2
        {
            get { return fimage_2; }
            set { SetPropertyValue<string>("image_2", ref fimage_2, value); }
        }
        string frole_author;
        [Size(50)]
        public string role_author
        {
            get { return frole_author; }
            set { SetPropertyValue<string>("role_author", ref frole_author, value); }
        }
        DateTime fcreated_date;
        public DateTime created_date
        {
            get { return fcreated_date; }
            set { SetPropertyValue<DateTime>("created_date", ref fcreated_date, value); }
        }
        string fitem_type;
        [Size(50)]
        public string item_type
        {
            get { return fitem_type; }
            set { SetPropertyValue<string>("item_type", ref fitem_type, value); }
        }
    }

}