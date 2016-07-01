using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using Libs;

namespace NexGenFlow.Manage
{
    public class Record_upc
    {
        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DataObject.QueryResult Insert(DataObject.Record_UPC record)
        {
            try
            {
                var new_record = new NexGenFlow.NexGen.doc_upc(new Session());
                //new_record.image_2 = record.Image_2;
                new_record.upc_code = record.Upc_code ;
                new_record.per_item = record.Per_Item;
                new_record.price = record.Price;
                new_record.qty = record.Qty;
                //new_record.receipt_images = record.Receipt_Images_1;
                new_record.remarks = record.Remarks;
                new_record.sr_no = record.Sr_No + string.Empty;
                new_record.total = 0;
                new_record.transaction_date = record.Transaction_Date;
                new_record.transcription_type = record.Transaction_Type;
                new_record.role_author = record.Role_author;
                new_record.docid = record.Docid;
                new_record.item_type = record.Item_type;
                new_record.Save();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);

                DataObject.QueryResult rs = new DataObject.QueryResult();
                rs.MessageType = DataObject.EVRData.Query_Message_Type_Errors;
                rs.Message = ex.Message;
                return rs;
            }
            return null;
            
        }
        public DataObject.QueryResult Insert(DataObject.Record_UPC record, Session session)
        {
            try
            {
                var new_record = new NexGenFlow.NexGen.doc_upc(session);
                //new_record.image_2 = record.Image_2;
                new_record.upc_code = record.Upc_code;
                new_record.per_item = record.Per_Item;
                new_record.price = record.Price;
                new_record.qty = record.Qty;
                //new_record.receipt_images = record.Receipt_Images_1;
                new_record.remarks = record.Remarks;
                new_record.sr_no = record.Sr_No + string.Empty;
                new_record.total = 0;
                new_record.transaction_date = record.Transaction_Date;
                new_record.transcription_type = record.Transaction_Type;
                new_record.role_author = record.Role_author;
                new_record.docid = record.Docid;
                new_record.item_type = record.Item_type;
                new_record.Save();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);

                DataObject.QueryResult rs = new DataObject.QueryResult();
                rs.MessageType = DataObject.EVRData.Query_Message_Type_Errors;
                rs.Message = ex.Message;
                return rs;
            }
            return null;
            
        }
        public DataObject.QueryResult Insert(List<DataObject.Record_UPC> records,long documentid,long keying_time,string username,string transaction_date, string transaction_time,double total_update)
        {
            var session = new Session();
            session.BeginTransaction();
            try
            {
                CriteriaOperator criteria_lockeddoc = CriteriaOperator.Parse("docid=?", documentid);
                XPCollection cl_lockeddoc = new XPCollection(session, typeof(NexGen.plan_doc_keyer), criteria_lockeddoc);
                var doc_locked = (NexGen.plan_doc_keyer)cl_lockeddoc[0];
                if (doc_locked != null)
                {
                    if (doc_locked.lockedby != username)
                    {
                        session.RollbackTransaction();
                        DataObject.QueryResult rs = new DataObject.QueryResult();
                        rs.MessageType = DataObject.EVRData.Query_Message_Type_Errors;
                        rs.Message = "Your Doc has been changed to other user.";
                        return rs;
                    }
                }
                CriteriaOperator criteria_docs = CriteriaOperator.Parse("id=?", documentid);

                XPCollection cl_doc = new XPCollection(session, typeof(NexGen.document), criteria_docs);

                var doc = (NexGen.document)cl_doc[0];
                if(doc!=null)
                {
                    doc.transaction_date = transaction_date;
                    doc.transaction_time = transaction_time;
                    doc.total = total_update;
                }
                doc.Save();
                var keying_time_record=new NexGenFlow.NexGen.doc_keying_time(session);
                keying_time_record.docid = documentid;
                keying_time_record.created_date = DateTime.Now;
                keying_time_record.keying_time = keying_time;
                keying_time_record.role_author = "keyer";
                keying_time_record.username = username;
                keying_time_record.Save();
                foreach (var record in records)
                {
                    var insert_record_result = Insert(record,session);
                    if (insert_record_result != null)
                        throw new Exception(insert_record_result.Message);
                }
                (new Manage.Plan_doc_keyer()).FinishLock(documentid,session);
                session.CommitTransaction();
                return null;
            }
            catch (Exception ex)
            {
                session.RollbackTransaction();

                DataObject.QueryResult rs = new DataObject.QueryResult();
                rs.MessageType = DataObject.EVRData.Query_Message_Type_Errors;
                rs.Message = ex.Message;
                return rs;
            }
            
        }
    }
}