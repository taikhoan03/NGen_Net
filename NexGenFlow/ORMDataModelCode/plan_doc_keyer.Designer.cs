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

    [OptimisticLockingReadBehavior(OptimisticLockingReadBehavior.Mixed)]
    public partial class plan_doc_keyer : XPLiteObject
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
        string fdoctype;
        [Size(255)]
        public string doctype
        {
            get { return fdoctype; }
            set { SetPropertyValue<string>("doctype", ref fdoctype, value); }
        }
        DateTime fcreated_date;
        public DateTime created_date
        {
            get { return fcreated_date; }
            set { SetPropertyValue<DateTime>("created_date", ref fcreated_date, value); }
        }
        string flockedby;
        public string lockedby
        {
            get { return flockedby; }
            set { SetPropertyValue<string>("lockedby", ref flockedby, value); }
        }
        DateTime flockeddate;
        public DateTime lockeddate
        {
            get { return flockeddate; }
            set { SetPropertyValue<DateTime>("lockeddate", ref flockeddate, value); }
        }
        bool flockedfinish;
        public bool lockedfinish
        {
            get { return flockedfinish; }
            set { SetPropertyValue<bool>("lockedfinish", ref flockedfinish, value); }
        }
        bool fis_deleted;
        public bool is_deleted
        {
            get { return fis_deleted; }
            set { SetPropertyValue<bool>("is_deleted", ref fis_deleted, value); }
        }
        string fclient_agent;
        [Size(50)]
        public string client_agent
        {
            get { return fclient_agent; }
            set { SetPropertyValue<string>("client_agent", ref fclient_agent, value); }
        }
        string fcomment;
        [Size(512)]
        public string comment
        {
            get { return fcomment; }
            set { SetPropertyValue<string>("comment", ref fcomment, value); }
        }
    }

}
