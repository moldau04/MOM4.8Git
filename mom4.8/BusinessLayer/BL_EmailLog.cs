using BusinessEntity;
using BusinessEntity.CustomersModel;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class BL_EmailLog
    {
        DL_EmailLog objDL_EmailLog = new DL_EmailLog();

        public void AddEmailLog(EmailLog emailLog)
        {
            if(emailLog.Refs != null && emailLog.Refs.Count > 0)
            {
                foreach (var item in emailLog.Refs)
                {
                    emailLog.Ref = item;
                    objDL_EmailLog.AddEmailLog(emailLog);
                }
            }
            else
            {
                objDL_EmailLog.AddEmailLog(emailLog);
            }
        }

        //API
        public void AddEmailLog(AddEmailLogParam _AddEmailLog, string ConnectionString)
        {
            objDL_EmailLog.AddEmailLog(_AddEmailLog, ConnectionString);
        }

        public DataSet GetEmailLogs(EmailLog emailLog)
        {
            return objDL_EmailLog.GetEmailLogs(emailLog);
        }

        public DataSet GetEmailLogsForInvoices(EmailLog emailLog)
        {
            return objDL_EmailLog.GetEmailLogsForInvoices(emailLog);
        }
        //API
        public List<GetEmailLogsViewModel> GetEmailLogs(GetEmailLogsParam _GetEmailLogs, string ConnectionString)
        {
            DataSet ds = objDL_EmailLog.GetEmailLogs(_GetEmailLogs, ConnectionString);

            List<GetEmailLogsViewModel> _lstGetEmailLogs = new List<GetEmailLogsViewModel>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                _lstGetEmailLogs.Add(
                    new GetEmailLogsViewModel()
                    {
                        Username = Convert.ToString(dr["Username"]),
                        EmailDate = Convert.ToDateTime(DBNull.Value.Equals(dr["EmailDate"]) ? null : dr["EmailDate"]),
                        Status = Convert.ToString(dr["Status"]),
                        Ref = Convert.ToInt32(DBNull.Value.Equals(dr["Ref"]) ? 0 : dr["Ref"]),
                        EmailFunction = Convert.ToString(dr["EmailFunction"]),
                        SessionNo = Convert.ToString(dr["SessionNo"]),
                        UsrErrMessage = Convert.ToString(dr["UsrErrMessage"]),
                        SysErrMessage = Convert.ToString(dr["SysErrMessage"]),
                    }
                    );
            }

            return _lstGetEmailLogs;
        }
    }
}
