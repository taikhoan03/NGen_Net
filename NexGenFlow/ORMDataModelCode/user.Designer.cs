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

    public partial class user : XPLiteObject
    {
        int fid;
        [Key]
        public int id
        {
            get { return fid; }
            set { SetPropertyValue<int>("id", ref fid, value); }
        }
        string fusername;
        [Size(255)]
        public string username
        {
            get { return fusername; }
            set { SetPropertyValue<string>("username", ref fusername, value); }
        }
        string fpwd;
        [Size(255)]
        public string pwd
        {
            get { return fpwd; }
            set { SetPropertyValue<string>("pwd", ref fpwd, value); }
        }
        int fteamid;
        public int teamid
        {
            get { return fteamid; }
            set { SetPropertyValue<int>("teamid", ref fteamid, value); }
        }
        bool fis_deleted;
        public bool is_deleted
        {
            get { return fis_deleted; }
            set { SetPropertyValue<bool>("is_deleted", ref fis_deleted, value); }
        }
        string frank;
        [Size(1)]
        public string rank
        {
            get { return frank; }
            set { SetPropertyValue<string>("rank", ref frank, value); }
        }
    }

}