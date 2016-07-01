﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using log4net;
using DevExpress.Xpo;

namespace NexGenFlow.Manage
{
    public class User
    {
        public static readonly ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DataObject.User login(string username, string pwd)
        {
            var user = GetUser(username);
            if (user == null)
                return null;
            if(user.pwd==DataObject.Util.Encrypt.EncodePassword(pwd))
            {
                if(user.is_deleted!=true)
                {
                    logger.Info(string.Format("User [{0}] login success.", user.username));
                    return new DataObject.User()
                    {
                        Id=user.id,
                        Is_deleted=user.is_deleted,
                        Last_login_date=DateTime.Now,
                        Role=0,
                        Team=user.teamid,
                        Username=user.username
                    };
                }
            }
            return null;
        }
        public NexGenFlow.NexGen.user GetUser(string username)
        {
            var session = new Session();
                CriteriaOperator criteria = CriteriaOperator.Parse("username = ?",username);
            var cl = new XPCollection(session, typeof(NexGen.user), criteria);
            cl.TopReturnedObjects = 1;

            return (NexGen.user)cl[0];
        }
    }
}
