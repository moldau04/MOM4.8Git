using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer
{
    public class DL_TimeCard
    {
        public DataSet GetInputCard(TimeCard timeCard)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = SqlHelper.ExecuteDataset(timeCard.ConnConfig, CommandType.StoredProcedure, "spGetTimeCardInput");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataSet GetTimeCardJob(TimeCard timeCard)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter param1 = new SqlParameter("@SearchText", SqlDbType.NVarChar, 128) {Value= timeCard.SearchText };
                SqlParameter param2 = new SqlParameter("@IsJob", SqlDbType.Int) { Value = timeCard.IsJob };
                SqlParameter param3 = new SqlParameter("@Loc", SqlDbType.Int) { Value = timeCard.Loc };
                SqlParameter param4 = new SqlParameter("@JobId", SqlDbType.Int) { Value = timeCard.JobId };
                SqlParameter param5 = new SqlParameter("@Worker", SqlDbType.NVarChar, 128) { Value = timeCard.Worker };
                SqlParameter param6 = new SqlParameter("@Userid", SqlDbType.Int) { Value = timeCard.Userid };
                SqlParameter param7 = new SqlParameter("@EN", SqlDbType.Int) { Value = timeCard.EN };

                ds = SqlHelper.ExecuteDataset(timeCard.ConnConfig, CommandType.StoredProcedure, "spGetTimeCardJob", param1,param2,param3,param4, param5,param6,param7);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataSet GetInvoiceByJobID(string prefixText,string Conn)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter param1 = new SqlParameter("@SearchText", SqlDbType.NVarChar, 128) { Value = prefixText };
                

                ds = SqlHelper.ExecuteDataset(Conn, CommandType.StoredProcedure, "spGetInvoiceByJobID", param1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public int SaveTimeCardJob(TimeCard timeCard, string super, string worker, string category,
            int markedReview,string username, DataTable dts,int timesheet)
        {
            int res = 0;
            DataSet ds = new DataSet();
            try
            {
                SqlParameter param1 = new SqlParameter("@Super", SqlDbType.NVarChar, 100) { Value = super };
                SqlParameter param2 = new SqlParameter("@Worker", SqlDbType.NVarChar,100) { Value = worker };
                SqlParameter param3 = new SqlParameter("@Category", SqlDbType.NVarChar, 100) { Value = category };
                SqlParameter param4 = new SqlParameter("@DataTable", SqlDbType.Structured) { Value = dts };
                SqlParameter param5 = new SqlParameter("@MarkedReview", SqlDbType.Int) { Value = markedReview };
                SqlParameter param6 = new SqlParameter("@UserName", SqlDbType.NVarChar,100) { Value = username };
                SqlParameter param7 = new SqlParameter("@Timesheet", SqlDbType.Int) { Value = timesheet };
                SqlParameter output = new SqlParameter("@Result", SqlDbType.Int);
                output.Direction = ParameterDirection.Output;

                 SqlHelper.ExecuteNonQuery(timeCard.ConnConfig, CommandType.StoredProcedure, "spSaveTimeCardJob", param1, param2,
                     param3,param4, param5,param6, param7, output);
                res = 1;

            }
            catch (Exception ex)
            {
                res = 0;
                throw ex;
            }
            return res;
        }
    }
}
