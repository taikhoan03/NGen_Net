﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;

namespace NexGenFlow.Manage
{
    public class Plan_doc_keyer
    {
        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void FinishLock(long docid, Session session)
        {
            CriteriaOperator criteria = CriteriaOperator.Parse("docid = ?",docid);
            XPCollection cl = new XPCollection(session, typeof(NexGen.plan_doc_keyer), criteria);
            cl.TopReturnedObjects = 1;
            var document = (NexGen.plan_doc_keyer)cl[0];// _list.FirstOrDefault();
            if(document!=null)
            {
                document.lockedfinish = true;
                document.Save();
            }
        }
        public void UnLock(string username)
        {
            
            CriteriaOperator criteria = CriteriaOperator.Parse("lockedby = ? and (lockedfinish=false or lockedfinish is null)",username);
            XPCollection cl = new XPCollection(new Session(), typeof(NexGen.plan_doc_keyer), criteria);
            cl.TopReturnedObjects = 1;
            if (cl.Count == 1)
            {
                var document = (NexGen.plan_doc_keyer)cl[0];
                if (document != null)
                {
                    document.lockedby = null;
                    document.lockedfinish = false;
                    document.Save();
                }
            }
            
        }
        public DataObject.QueryResult Discard_doc(long docid,  string remark,string username_discard)
        {
            DataObject.QueryResult rs = new DataObject.QueryResult();
            Session session = new Session();
            session.BeginTransaction();
            try
            {
                FinishLock(docid, session);
                var discard_record = new NexGenFlow.NexGen.discard_doc(session);
                discard_record.author_role = DataObject.EVRData.Role_author_keyer;
                discard_record.created_date = DateTime.Now;
                discard_record.remarks = remark;
                discard_record.discard_by = username_discard;
                discard_record.docid = docid;
                discard_record.Save();
                session.CommitTransaction();
                return null;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);

                rs.MessageType = DataObject.EVRData.Query_Message_Type_Errors;
                rs.Message = ex.Message;
                return rs;
            }
        }
        public void Update_comment(long docid, string comment)
        {
            CriteriaOperator criteria = CriteriaOperator.Parse("docid = ?", docid);
            XPCollection cl = new XPCollection(new Session(), typeof(NexGen.plan_doc_keyer), criteria);
            cl.TopReturnedObjects = 1;
            var document = (NexGen.plan_doc_keyer)cl[0];// _list.FirstOrDefault();
            if (document != null)
            {
                document.comment = comment;
                document.Save();
            }
        }
    }
}
