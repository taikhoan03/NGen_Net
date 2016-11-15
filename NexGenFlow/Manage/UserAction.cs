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
                string strCmd_package_assign = string.Format("select packageid from package_assignment where username='{0}' and is_enabled=true order by priority limit 1", username); ;
                string strCmd_doc_assign = string.Format("select d.packageid from document d join plan_doc_keyer pl on d.id=pl.docid    where (pl.lockedfinish = false or pl.lockedfinish is null) and  packageid = (select packageid from package_assignment where username = '{0}' and is_enabled = true order by priority desc limit 1) limit 1; ", username);

                
                string strCmdModFieldSelect = string.Format("( select    d.id  from    document d  join    package pac              on pac.id=d.packageid  join plan_doc_keyer pl 	on pl.docid=d.id where    (             pl.lockedby='{0}'                         and (                pl.is_deleted=false                                or pl.is_deleted is null                          )                          and (                pl.lockedfinish is null                                or pl.lockedfinish=false                          )                    )              order by          pac.priority limit 1 )   union all (    select       d.id        from       document d      join       plan_doc_keyer pl                          on d.id=pl.docid              join       package pac                          on pac.id=d.packageid              where       (          pl.lockedby is null                          and (             pl.is_deleted=false                                or pl.is_deleted is null                          )                    )         order by       pac.priority,       banner_id limit 1     )     limit 1", username);

                

                string connectionString = NexGenFlow.NexGen.ConnectionHelper.ConnectionString;
                try
                {
                    
                    Libs.Database db = new Database();
                    db.Conn = new Npgsql.NpgsqlConnection(connectionString.Replace("XpoProvider=Postgres;", string.Empty));
                    //kiem tra co du lieu user_package_assign
                    var package_assinged= (db.ExecuteReader<string>(strCmd_doc_assign, CloseConnectionOnDone:false));
                    
                    if (package_assinged.GetType().IsGenericType)
                    {
                        //null: khong co du lieu
                        // clear tat ca du lieu assign cua user: is_enable
                        string str_update_assign = string.Format("update package_assignment set is_enabled=false where username='{0}' and is_enabled=true", username);
                        db.ExecuteReader<string>(str_update_assign, false);
                    }
                    else
                    {
                        var package_assign = package_assinged.ToString();
                        string with_package_assign = " and packageid="+ package_assign + " ";
                        strCmdModFieldSelect = string.Format("( select    d.id  from    document d  join    package pac              on pac.id=d.packageid  join plan_doc_keyer pl 	on pl.docid=d.id where    (             pl.lockedby='{0}'                         and (                pl.is_deleted=false                                or pl.is_deleted is null                          )                          and (                pl.lockedfinish is null                                or pl.lockedfinish=false                          )                    )              order by          pac.priority limit 1 )   union all (    select       d.id        from       document d      join       plan_doc_keyer pl                          on d.id=pl.docid              join       package pac                          on pac.id=d.packageid              where       (          pl.lockedby is null                          and (             pl.is_deleted=false                                or pl.is_deleted is null                          )             "+ with_package_assign + "       )         order by       pac.priority,       banner_id limit 1     )     limit 1", username);
                    }
                    string strCmdUpdate = string.Format("WITH updated AS (update plan_doc_keyer set lockedby='{0}', lockeddate=now(),client_agent='" + agent + "' where docid=(" + strCmdModFieldSelect + ") RETURNING *)", username);
                    //string strSelectAfterUpdate = " select * from document where id = (SELECT docid FROM updated);";
                    string strSelectAfterUpdate = " select d.*,l.username as Reset_by,l.comment as Reset_comment from document d left join write_log_reset l on d.id=l.docid where d.id = (SELECT docid FROM updated) order by l.id desc limit 1;";
                    var rs = ((List<DataObject.Document>)db.ExecuteReader<DataObject.Document>(strCmdUpdate + strSelectAfterUpdate, CloseConnectionOnDone: false));
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