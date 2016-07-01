using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using Libs;

namespace NexGenFlow.Manage
{
    public class UserAction
    {
        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly object lockGetDocKeyer = new object();

        public DataObject.Document Get_doc_for_keyer(string username)
        {
            lock (lockGetDocKeyer)
            {
                
                string strCmdModFieldSelect = string.Format("( select    d.id  from    document d  join    package pac              on pac.id=d.packageid  join plan_doc_keyer pl 	on pl.docid=d.id where    (             pl.lockedby='{0}'                         and (                pl.is_deleted=false                                or pl.is_deleted is null                          )                          and (                pl.lockedfinish is null                                or pl.lockedfinish=false                          )                    )              order by          pac.priority limit 1 )   union all (    select       d.id        from       document d      join       plan_doc_keyer pl                          on d.id=pl.docid              join       package pac                          on pac.id=d.packageid              where       (          pl.lockedby is null                          and (             pl.is_deleted=false                                or pl.is_deleted is null                          )                    )         order by       pac.priority,       banner_id limit 1     )     limit 1", username);
                
                string strCmdUpdate = string.Format("WITH updated AS (update plan_doc_keyer set lockedby='{0}', lockeddate=now() where docid=(" + strCmdModFieldSelect + ") RETURNING *)", username);
                string strSelectAfterUpdate = " select * from document where id = (SELECT docid FROM updated);";
                string connectionString = NexGenFlow.NexGen.ConnectionHelper.ConnectionString;
                try
                {
                    Libs.Database db = new Database();
                    db.Conn = new Npgsql.NpgsqlConnection(connectionString.Replace("XpoProvider=Postgres;", string.Empty));
                    
                    return ((List<DataObject.Document>)db.ExecuteReader<DataObject.Document>(strCmdUpdate + strSelectAfterUpdate, true)).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    logger.Error(ex.StackTrace);
                }
                finally
                {
                    
                }
                return null;
            }
        }
        public DataObject.Document Get_doc_for_keyer(string username,string agent)
        {
            lock (lockGetDocKeyer)
            {
                string with_package_assign = "and packageid=1232";
                string strCmdModFieldSelect = string.Format("( select    d.id  from    document d  join    package pac              on pac.id=d.packageid  join plan_doc_keyer pl 	on pl.docid=d.id where    (             pl.lockedby='{0}'                         and (                pl.is_deleted=false                                or pl.is_deleted is null                          )                          and (                pl.lockedfinish is null                                or pl.lockedfinish=false                          )                    )              order by          pac.priority limit 1 )   union all (    select       d.id        from       document d      join       plan_doc_keyer pl                          on d.id=pl.docid              join       package pac                          on pac.id=d.packageid              where       (          pl.lockedby is null                          and (             pl.is_deleted=false                                or pl.is_deleted is null                          )                    )         order by       pac.priority,       banner_id limit 1     )     limit 1", username);
                
                string strCmdUpdate = string.Format("WITH updated AS (update plan_doc_keyer set lockedby='{0}', lockeddate=now(),client_agent='"+agent+"' where docid=(" + strCmdModFieldSelect + ") RETURNING *)", username);
                //string strSelectAfterUpdate = " select * from document where id = (SELECT docid FROM updated);";
                string strSelectAfterUpdate = " select d.*,l.username as Reset_by,l.comment as Reset_comment from document d left join write_log_reset l on d.id=l.docid where d.id = (SELECT docid FROM updated) order by l.id desc limit 1;";
                string connectionString = NexGenFlow.NexGen.ConnectionHelper.ConnectionString;
                try
                {
                    
                    Libs.Database db = new Database();
                    db.Conn = new Npgsql.NpgsqlConnection(connectionString.Replace("XpoProvider=Postgres;", string.Empty));
                    var rs = ((List<DataObject.Document>)db.ExecuteReader<DataObject.Document>(strCmdUpdate + strSelectAfterUpdate, true));
                    return ((List<DataObject.Document>)db.ExecuteReader<DataObject.Document>(strCmdUpdate + strSelectAfterUpdate, true)).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    logger.Error(ex.StackTrace);
                }
                finally
                {
                    
                }
                return null;
            }
        }
    }
}