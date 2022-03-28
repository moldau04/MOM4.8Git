using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DL_EmailLog
    {
        public void AddEmailLog(EmailLog emailLog)
        {
            try
            {
                //string query = "INSERT INTO tblEmailSendingLog(EmailDate,[From],Ref,Screen,Sender,[Status],SysErrMessage,[To],Username,UsrErrMessage,[Function],[SessionNo])"
                //                            + "VALUES(@EmailDate,@From,@Ref,@Screen,@Sender,@Status,@SysErrMessage,@To,@Username,@UsrErrMessage,@Function,@SessionNo)";

                string query = "spAddEmailLogs";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@EmailDate", emailLog.EmailDate));
                parameters.Add(new SqlParameter("@From", emailLog.From));
                parameters.Add(new SqlParameter("@Ref", emailLog.Ref));
                parameters.Add(new SqlParameter("@Screen", emailLog.Screen));
                parameters.Add(new SqlParameter("@Sender", emailLog.Sender));
                parameters.Add(new SqlParameter("@Status", emailLog.Status));
                parameters.Add(new SqlParameter("@SysErrMessage", emailLog.SysErrMessage));
                parameters.Add(new SqlParameter("@To", emailLog.To));
                parameters.Add(new SqlParameter("@Username", emailLog.Username));
                parameters.Add(new SqlParameter("@UsrErrMessage", emailLog.UsrErrMessage));
                parameters.Add(new SqlParameter("@Function", emailLog.Function));
                parameters.Add(new SqlParameter("@SessionNo", emailLog.SessionNo));

                //SqlHelper.ExecuteNonQuery(emailLog.ConnConfig, CommandType.Text, query.ToString(), parameters.ToArray());

                SqlHelper.ExecuteNonQuery(emailLog.ConnConfig, CommandType.StoredProcedure, query.ToString(), parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void AddEmailLog(AddEmailLogParam _AddEmailLog, string ConnectionString)
        {
            try
            {
                string query = "INSERT INTO tblEmailSendingLog(EmailDate,[From],Ref,Screen,Sender,[Status],SysErrMessage,[To],Username,UsrErrMessage,[Function],[SessionNo])"
                                            + "VALUES(@EmailDate,@From,@Ref,@Screen,@Sender,@Status,@SysErrMessage,@To,@Username,@UsrErrMessage,@Function,@SessionNo)";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@EmailDate", _AddEmailLog.EmailDate));
                parameters.Add(new SqlParameter("@From", _AddEmailLog.From));
                parameters.Add(new SqlParameter("@Ref", _AddEmailLog.Ref));
                parameters.Add(new SqlParameter("@Screen", _AddEmailLog.Screen));
                parameters.Add(new SqlParameter("@Sender", _AddEmailLog.Sender));
                parameters.Add(new SqlParameter("@Status", _AddEmailLog.Status));
                parameters.Add(new SqlParameter("@SysErrMessage", _AddEmailLog.SysErrMessage));
                parameters.Add(new SqlParameter("@To", _AddEmailLog.To));
                parameters.Add(new SqlParameter("@Username", _AddEmailLog.Username));
                parameters.Add(new SqlParameter("@UsrErrMessage", _AddEmailLog.UsrErrMessage));
                parameters.Add(new SqlParameter("@Function", _AddEmailLog.Function));
                parameters.Add(new SqlParameter("@SessionNo", _AddEmailLog.SessionNo));

                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, query.ToString(), parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEmailLogs(EmailLog emailLog)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Screen", emailLog.Screen));
                parameters.Add(new SqlParameter("@Function", emailLog.Function));
                parameters.Add(new SqlParameter("@Ref", emailLog.Ref));
                return SqlHelper.ExecuteDataset(emailLog.ConnConfig, CommandType.StoredProcedure, "spGetEmailSendingLogs", parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEmailLogsForInvoices(EmailLog emailLog)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Ref", emailLog.Ref));
                return SqlHelper.ExecuteDataset(emailLog.ConnConfig, CommandType.StoredProcedure, "spGetEmailLogsForInvoices", parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetEmailLogs(GetEmailLogsParam _GetEmailLogs, string ConnectionString)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Screen", _GetEmailLogs.Screen));
                parameters.Add(new SqlParameter("@Function", _GetEmailLogs.Function));
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetEmailSendingLogs", parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
