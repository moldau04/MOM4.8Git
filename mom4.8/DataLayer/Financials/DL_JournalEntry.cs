using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataLayer
{
    public class DL_JournalEntry
    {
        public void DeleteGLA(Journal objJournal)
        {
            try
            {
                SqlParameter paraJe = new SqlParameter();
                paraJe.ParameterName = "Batch";
                paraJe.SqlDbType = SqlDbType.Int;
                paraJe.Value = objJournal.BatchID;

                SqlHelper.ExecuteNonQuery(objJournal.ConnConfig, CommandType.StoredProcedure, "spDeleteJE", paraJe);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public DataSet GetDataByRef(Journal objJournal)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("        SELECT Ref, fDate, Internal, fDesc,     \n");
                if (objJournal.IsRecurring.Equals(true))
                {
                    varname.Append("            Frequency, (SELECT CASE WHEN EXISTS(SELECT 1 FROM GLARecurI WHERE Job > 0 AND Ref = '" + objJournal.Ref + "') THEN 1 ELSE 0 END AS bit) AS IsJobSpec  \n");
                    varname.Append("            FROM GLARecur WHERE Ref = '" + objJournal.Ref + "'      \n");
                }
                else
                {
                    varname.Append("            Batch, ISNULL(OriginalJE,0) AS OriginalJE,(SELECT Internal from GLA WHERE Ref in (SELECT OriginalJE FROM GLA WHERE Ref= '" + objJournal.Ref + "')) AS InternalJE,(SELECT CASE WHEN EXISTS(SELECT 1 FROM Trans WHERE VInt > 0 AND (Type = 30 OR Type = 31 OR Type = 50) AND Ref ='" + objJournal.Ref + "') THEN 1 ELSE 0 END AS bit) AS IsJobSpec       \n");
                    varname.Append("            FROM GLA WHERE Ref = '" + objJournal.Ref + "'           \n");
                }
                return objJournal.DsGLA = SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetDataByBatch(Journal objJournal)
        {
            try
            {
                return objJournal.DsGLA = SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.Text, "SELECT distinct t.Acct, t.AcctSub, t.Amount, t.Batch, t.EN, t.ID, t.Line, t.Ref, t.Sel, t.Status, t.Type, t.VDoub, t.VInt, t.fDate, t.fDesc, t.strRef, isnull(td.IsRecon,0) as IsRecon FROM Trans as t left join TransDeposits as td on t.Batch=td.Batch WHERE t.Batch = "+objJournal.BatchID+" AND (t.Type = 50 or t.Type = 30 or t.Type = 31)");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetMaxTransID(Journal objJournal)
        {
            try
            {
                return objJournal.MaxTransID = Convert.ToInt32(SqlHelper.ExecuteScalar(objJournal.ConnConfig, CommandType.Text, "SELECT ISNULL(MAX(ID),0)+1 AS MAXID FROM Trans"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetMaxTransRef(Journal objJournal)
        {
            try
            {
                return objJournal.Ref = Convert.ToInt32(SqlHelper.ExecuteScalar(objJournal.ConnConfig, CommandType.Text, "SELECT ISNULL(MAX(Ref),0)+1 AS MAXRef FROM GLA"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetMaxTransRef(GetMaxTransRefParam objGetMaxTransRefParam, string ConnectionString)
        {
            try
            {
                return  Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, "SELECT ISNULL(MAX(Ref),0)+1 AS MAXRef FROM GLA"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetMaxTransBatch(Journal objJournal)
        {
            try
            {
                return objJournal.BatchID = Convert.ToInt32(SqlHelper.ExecuteScalar(objJournal.ConnConfig, CommandType.Text, "SELECT ISNULL(MAX(Batch),0)+1 AS MAXBatch FROM Trans"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetMaxTransBatch(GetMaxTransBatchParam objGetMaxTransBatchParam,string ConnectionString)
        {
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, "SELECT ISNULL(MAX(Batch),0)+1 AS MAXBatch FROM Trans"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllTransaction(Journal objJournal)
        {
            try
            {
                return objJournal.DsTrans = SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.Text, "SELECT Trans.ID AS TransID, Chart.Acct, Chart.fDesc as ChartDesc, Trans.fDesc AS TransDesc, Trans.Amount FROM Chart INNER JOIN Trans ON Trans.Acct = Chart.ID WHERE (Trans.Type = 50) AND (Trans.ID = " + objJournal.TransID + ")");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetYearEndClosedOutData(Journal objJournal)
        {
            try
            {
                StringBuilder varname = new StringBuilder();

                varname.Append("SELECT Ref, fDate, Internal, fDesc, Batch FROM GLA where fDesc LIKE 'Year-end Transfer%' and Internal = '" + objJournal.Internal + "' order by fDate DESC \n");
                varname.Append("SELECT TOP 1 Ref, fDate, Internal, fDesc, Batch FROM GLA WHERE fDesc LIKE 'Year-end Transfer%' AND Internal < '" + objJournal.Internal + "' ORDER BY fDate DESC");

                return objJournal.DsTrans = SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddGLA(Journal objJournal)
        {
            try
            {
                string query = "INSERT INTO GLA (Ref,fDate,Internal,fDesc,Batch) VALUES (@Ref,@Date,@Internal,@Desc,@Batch)";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Ref", objJournal.Ref));
                parameters.Add(new SqlParameter("@Date", objJournal.GLDate));
                parameters.Add(new SqlParameter("@Internal", objJournal.Internal));
                parameters.Add(new SqlParameter("@Desc", objJournal.GLDesc));
                parameters.Add(new SqlParameter("@Batch", objJournal.BatchID));

                int rowsAffected = SqlHelper.ExecuteNonQuery(objJournal.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddGLA(AddGLAParam objAddGLAParam,string ConnectionString)
        {
            try
            {
                string query = "INSERT INTO GLA (Ref,fDate,Internal,fDesc,Batch) VALUES (@Ref,@Date,@Internal,@Desc,@Batch)";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Ref", objAddGLAParam.Ref));
                parameters.Add(new SqlParameter("@Date", objAddGLAParam.GLDate));
                parameters.Add(new SqlParameter("@Internal", objAddGLAParam.Internal));
                parameters.Add(new SqlParameter("@Desc", objAddGLAParam.GLDesc));
                parameters.Add(new SqlParameter("@Batch", objAddGLAParam.BatchID));

                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddJournalTrans(Transaction objTrans)
        {
            var para = new SqlParameter[17];

            para[1] = new SqlParameter
            {
                ParameterName = "@Batch",
                SqlDbType = SqlDbType.Int,
                Value = objTrans.BatchID
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@fDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objTrans.TransDate
            };
            if (objTrans.Type != 0)
            {
                para[3] = new SqlParameter
                {
                    ParameterName = "@Type",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objTrans.Type
                };
            }
            para[4] = new SqlParameter
            {
                ParameterName = "@Line",
                SqlDbType = SqlDbType.Int,
                Value = objTrans.Line
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@Ref",
                SqlDbType = SqlDbType.Int,
                Value = objTrans.Ref
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@fDesc",
                SqlDbType = SqlDbType.VarChar,
                Value = objTrans.TransDescription
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@Amount",
                SqlDbType = SqlDbType.Float,
                Value = objTrans.Amount
            };
            para[8] = new SqlParameter
            {
                ParameterName = "@Acct",
                SqlDbType = SqlDbType.Int,
                Value = objTrans.Acct
            };
            if (objTrans.AcctSub != null)
            {
                para[9] = new SqlParameter
                {
                    ParameterName = "@AcctSub",
                    SqlDbType = SqlDbType.Int,
                    Value = objTrans.AcctSub
                };
            }
            para[10] = new SqlParameter
            {
                ParameterName = "@Sel",
                SqlDbType = SqlDbType.Int,
                Value = objTrans.Sel
            };
            if (objTrans.JobInt != 0)
            {
                para[12] = new SqlParameter
                {
                    ParameterName = "@VInt",
                    SqlDbType = SqlDbType.Int,
                    Value = objTrans.JobInt
                };
            }
            if (objTrans.PhaseDoub != 0.0)
            {
                para[13] = new SqlParameter
                {
                    ParameterName = "@VDoub",
                    SqlDbType = SqlDbType.Float,
                    Value = objTrans.PhaseDoub
                };
            }
            if (!string.IsNullOrEmpty(objTrans.strRef))
            {
                para[15] = new SqlParameter
                {
                    ParameterName = "@strRef",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objTrans.strRef
                };
            }
            para[16] = new SqlParameter
            {
                ParameterName = "returnval",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };
            try
            {
                //SqlHelper.ExecuteNonQuery(objTrans.ConnConfig, CommandType.StoredProcedure, "AddJournal", para);
                SqlHelper.ExecuteDataset(objTrans.ConnConfig, CommandType.StoredProcedure, "AddJournal", para);
                return Convert.ToInt32(para[16].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddJournalTrans(AddJournalTransParam objAddJournalTransParam,string ConnectionString)
        {
            var para = new SqlParameter[17];

            para[1] = new SqlParameter
            {
                ParameterName = "@Batch",
                SqlDbType = SqlDbType.Int,
                Value = objAddJournalTransParam.BatchID
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@fDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objAddJournalTransParam.TransDate
            };
            if (objAddJournalTransParam.Type != 0)
            {
                para[3] = new SqlParameter
                {
                    ParameterName = "@Type",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objAddJournalTransParam.Type
                };
            }
            para[4] = new SqlParameter
            {
                ParameterName = "@Line",
                SqlDbType = SqlDbType.Int,
                Value = objAddJournalTransParam.Line
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@Ref",
                SqlDbType = SqlDbType.Int,
                Value = objAddJournalTransParam.Ref
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@fDesc",
                SqlDbType = SqlDbType.VarChar,
                Value = objAddJournalTransParam.TransDescription
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@Amount",
                SqlDbType = SqlDbType.Float,
                Value = objAddJournalTransParam.Amount
            };
            para[8] = new SqlParameter
            {
                ParameterName = "@Acct",
                SqlDbType = SqlDbType.Int,
                Value = objAddJournalTransParam.Acct
            };
            if (objAddJournalTransParam.AcctSub != null)
            {
                para[9] = new SqlParameter
                {
                    ParameterName = "@AcctSub",
                    SqlDbType = SqlDbType.Int,
                    Value = objAddJournalTransParam.AcctSub
                };
            }
            para[10] = new SqlParameter
            {
                ParameterName = "@Sel",
                SqlDbType = SqlDbType.Int,
                Value = objAddJournalTransParam.Sel
            };
            if (objAddJournalTransParam.JobInt != 0)
            {
                para[12] = new SqlParameter
                {
                    ParameterName = "@VInt",
                    SqlDbType = SqlDbType.Int,
                    Value = objAddJournalTransParam.JobInt
                };
            }
            if (objAddJournalTransParam.PhaseDoub != 0.0)
            {
                para[13] = new SqlParameter
                {
                    ParameterName = "@VDoub",
                    SqlDbType = SqlDbType.Float,
                    Value = objAddJournalTransParam.PhaseDoub
                };
            }
            if (!string.IsNullOrEmpty(objAddJournalTransParam.strRef))
            {
                para[15] = new SqlParameter
                {
                    ParameterName = "@strRef",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objAddJournalTransParam.strRef
                };
            }
            para[16] = new SqlParameter
            {
                ParameterName = "returnval",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };
            try
            {
                //SqlHelper.ExecuteNonQuery(objTrans.ConnConfig, CommandType.StoredProcedure, "AddJournal", para);
                SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "AddJournal", para);
                return Convert.ToInt32(para[16].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateGLA(Journal objJournal)
        {
            try
            {

                string query = "UPDATE GLA SET fDate = @fDate, Internal = @Internal, fDesc = @fDesc WHERE Ref = @Ref";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Ref", objJournal.Ref));
                parameters.Add(new SqlParameter("@fDate", objJournal.GLDate));
                parameters.Add(new SqlParameter("@Internal", objJournal.Internal));
                parameters.Add(new SqlParameter("@fDesc", objJournal.GLDesc));

                int rowsAffected = SqlHelper.ExecuteNonQuery(objJournal.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateJournalTrans(Transaction objTrans)
        {
            try
            {
                string query = "UPDATE Trans SET fDate = @fDate, fDesc = @fDesc, Amount = @Amount, Acct = @Acct, VInt = @VInt, VDoub=@VDoub, Type = @Type, AcctSub = @AcctSub WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", objTrans.ID));
                parameters.Add(new SqlParameter("@fDate", objTrans.TransDate));
                parameters.Add(new SqlParameter("@fDesc", objTrans.TransDescription));
                parameters.Add(new SqlParameter("@Amount", objTrans.Amount));
                parameters.Add(new SqlParameter("@Acct", objTrans.Acct));
                parameters.Add(new SqlParameter("@VInt", objTrans.JobInt));
                parameters.Add(new SqlParameter("@VDoub", objTrans.PhaseDoub));
                parameters.Add(new SqlParameter("@Type", objTrans.Type));
                parameters.Add(new SqlParameter("@AcctSub", objTrans.AcctSub));

                int rowsAffected = SqlHelper.ExecuteNonQuery(objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public DataSet GetJobsLoc(Transaction objTrans)
        {
            try
            {
                return objTrans.DsTrans = SqlHelper.ExecuteDataset(objTrans.ConnConfig, "spGetJobLocSearch", objTrans.SearchValue, objTrans.IsJob, objTrans.EN, objTrans.UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetLocByJobID(Transaction objTrans)
        {
            try
            {
                return objTrans.DsTrans = SqlHelper.ExecuteDataset(objTrans.ConnConfig, CommandType.Text, "SELECT j.ID AS ID, j.fDesc AS fDesc, l.Tag AS Tag FROM Job as j, Loc as l WHERE j.Loc=l.Loc and j.ID=" + objTrans.JobInt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllJEByDate(Journal objJournal)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[2];

                para[0] = new SqlParameter();
                para[0].ParameterName = "@startdate";
                para[0].SqlDbType = SqlDbType.DateTime;
                para[0].Value = objJournal.StartDate;

                para[1] = new SqlParameter();
                para[1].ParameterName = "@enddate";
                para[1].SqlDbType = SqlDbType.DateTime;
                para[1].Value = objJournal.EndDate;

                return objJournal.DsGLA = SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.Text, "SELECT fDate, Ref, Internal, fDesc, Batch, ISNULL((SELECT TOP 1 Sel FROM Trans WHERE Batch = GLA.Batch AND Sel = 1),0) AS IsCleared,ISNULL((SELECT SUM(Amount) FROM Trans WHERE Batch = GLA.Batch AND Trans.Type in (30,31,50) And Amount > 0),0.00) AS Debit, ISNULL((SELECT SUM(Amount * -1) FROM Trans WHERE Batch = GLA.Batch And Trans.Type in (30,31,50) And Amount < 0),0.00) AS Credit FROM GLA WHERE fDate >= '" + objJournal.StartDate.ToShortDateString() + "' AND fDate <= '" + objJournal.EndDate.ToShortDateString() + "' ORDER BY fDate, Ref");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobDetailByID(Transaction objTrans)
        {
            try
            {
                return objTrans.DsTrans = SqlHelper.ExecuteDataset(objTrans.ConnConfig, CommandType.Text, "SELECT ID, fDesc, Loc FROM Job Where ID='" + objTrans.JobInt + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPhaseByID(Transaction objTrans)
        {
            try
            {
                return objTrans.DsTrans = SqlHelper.ExecuteDataset(objTrans.ConnConfig, CommandType.Text, "SELECT ID, JobT, Job, Type, fDesc, Code, Actual, Line FROM JobTItem WHERE ID='" + objTrans.PhaseDoub + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateJournalTransAmount(Transaction objTrans)
        {
            try
            {
                string query = "UPDATE Trans SET fDesc=@fDesc, fDate=@fDate, Amount = @Amount WHERE ID = @ID AND Type = @Type";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", objTrans.ID));
                parameters.Add(new SqlParameter("@fDate", objTrans.TransDate));
                parameters.Add(new SqlParameter("@Amount", objTrans.Amount));
                parameters.Add(new SqlParameter("@Type", objTrans.Type));
                parameters.Add(new SqlParameter("@fDesc", objTrans.TransDescription));

                int rowsAffected = SqlHelper.ExecuteNonQuery(objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPaymentTransByBatchRef(Transaction objTrans)
        {
            try
            {
                return objTrans.DsTrans = SqlHelper.ExecuteDataset(objTrans.ConnConfig, CommandType.Text, "SELECT Acct, Batch ,fDesc, Amount, EN, ID, Line, Ref, Sel, Status, Type, VDoub, VInt, fDate, strRef, AcctSub FROM Trans where batch=(select batch from Trans where ID=" + objTrans.ID+")");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTransByID(Transaction objTrans)
        {
            try
            {
                return objTrans.DsTrans = SqlHelper.ExecuteDataset(objTrans.ConnConfig, CommandType.Text, "SELECT ID,Batch,fDate,Type,Line,Ref,fDesc,Amount,Acct,AcctSub,Status,Sel,VInt,VDoub,EN,strRef FROM Trans WHERE ID=" + objTrans.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTransByID(GetTransByIDParam objGetTransByIDParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT ID,Batch,fDate,Type,Line,Ref,fDesc,Amount,Acct,AcctSub,Status,Sel,VInt,VDoub,EN,strRef FROM Trans WHERE ID=" + objGetTransByIDParam.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateInvoiceTransDetails(Transaction _objTrans)
        {
            try
            {
                string query = "UPDATE Trans SET Ref=@Ref WHERE Batch=@Batch";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Ref", _objTrans.Ref));
                parameters.Add(new SqlParameter("@Batch", _objTrans.BatchID));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public void UpdateBillTrans(Transaction _objTrans)
        {
            try
            {
                string query = "UPDATE Trans SET fDate=@fDate, Amount=@Amount WHERE ID=@ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objTrans.ID));
                parameters.Add(new SqlParameter("@fDate", _objTrans.TransDate));
                parameters.Add(new SqlParameter("@Amount", _objTrans.Amount));

                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public void AddTransBankAdj(TransBankAdj _objTrans)
        {
            var para = new SqlParameter[5];

            para[1] = new SqlParameter
            {
                ParameterName = "@Batch",
                SqlDbType = SqlDbType.Int,
                Value = _objTrans.Batch
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@Bank",
                SqlDbType = SqlDbType.Int,
                Value = _objTrans.Bank
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@IsRecon",
                SqlDbType = SqlDbType.Bit,
                Value = _objTrans.IsRecon
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@Amount",
                SqlDbType = SqlDbType.Decimal,
                Value = _objTrans.Amount
            };
            try
            {
                SqlHelper.ExecuteDataset(_objTrans.ConnConfig, CommandType.StoredProcedure, "spAddTransBankAdj", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTransCheckRecon(TransBankAdj _objTrans)
        {
            try
            {
                string query = "UPDATE TransChecks SET IsRecon=@IsRecon WHERE Batch=@Batch";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@IsRecon", _objTrans.IsRecon));
                parameters.Add(new SqlParameter("@Batch", _objTrans.Batch));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTransDepositRecon(TransBankAdj _objTrans)
        {
            try
            {
                string query = "UPDATE TransDeposits SET IsRecon=@IsRecon WHERE Batch=@Batch";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@IsRecon", _objTrans.IsRecon));
                parameters.Add(new SqlParameter("@Batch", _objTrans.Batch));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTransByBatchBank(Transaction _objTrans)
        {
            try
            {
                return _objTrans.DsTrans = SqlHelper.ExecuteDataset(_objTrans.ConnConfig, CommandType.Text, "SELECT t.Acct, t.AcctSub, t.Amount, t.Batch, t.EN, t.ID, t.Line, t.Ref, t.Sel, t.Status, t.Type, t.VDoub, t.VInt, t.fDate, t.fDesc, t.strRef, c.Acct AS AcctNo, c.fDesc AS AcctName FROM Trans as t, Chart as c WHERE t.Acct = c.ID AND t.Type = 41 AND t.Batch = " + _objTrans.BatchID + " Order By t.ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateClearItem(Transaction _objTrans)
        {
            try
            {
                SqlHelper.ExecuteDataset(_objTrans.ConnConfig, "spUpdateClearItems", _objTrans.BatchID, _objTrans.Sel, _objTrans.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTransByBatchRef(Transaction _objTrans) // Transaction details
        {
            try
            {
                return _objTrans.DsTrans = SqlHelper.ExecuteDataset(_objTrans.ConnConfig, CommandType.Text, " SELECT t.Acct, t.AcctSub, t.Amount, t.Batch, t.EN, t.ID, t.Line, t.Ref, t.Sel, t.Status, t.Type, t.VDoub, t.VInt, t.fDate, t.fDesc, t.strRef, c.Acct AS AcctNo, c.fDesc AS AcctName FROM Trans as t, Chart as c WHERE t.Acct = c.ID AND t.Ref = " + _objTrans.Ref + " AND t.Batch = " + _objTrans.BatchID + " Order By t.Line");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTransByBatch(Transaction _objTrans)
        {
            try
            {
                return _objTrans.DsTrans = SqlHelper.ExecuteDataset(_objTrans.ConnConfig, CommandType.Text, " SELECT t.Acct, t.AcctSub, t.Amount, t.Batch, t.EN, t.ID, t.Line, t.Ref, t.Sel, t.Status, t.Type, t.VDoub, t.VInt, t.fDate, t.fDesc, t.strRef, c.Acct AS AcctNo, c.fDesc AS AcctName FROM Trans as t, Chart as c WHERE t.Acct = c.ID AND t.Batch = " + _objTrans.BatchID + " Order By t.Line");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTransByBatch(GetTransByBatchParam _objGetTransByBatchParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, " SELECT t.Acct, t.AcctSub, t.Amount, t.Batch, t.EN, t.ID, t.Line, t.Ref, t.Sel, t.Status, t.Type, t.VDoub, t.VInt, t.fDate, t.fDesc, t.strRef, c.Acct AS AcctNo, c.fDesc AS AcctName FROM Trans as t, Chart as c WHERE t.Acct = c.ID AND t.Batch = " + _objGetTransByBatchParam.BatchID + " Order By t.Line");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBillAPTransByBatch(Transaction _objTrans)
        {
            try
            {
                return _objTrans.DsTrans = SqlHelper.ExecuteDataset(_objTrans.ConnConfig, CommandType.Text, "SELECT t.Acct, t.AcctSub, t.Amount, t.Batch, t.EN, t.ID, t.Line, t.Ref, isnull(t.Sel,0) as Sel, t.Status, t.Type, t.VDoub, t.VInt, t.fDate, t.fDesc, t.strRef, c.Acct AS AcctNo, c.fDesc AS AcctName FROM Trans as t, Chart as c WHERE t.Acct = c.ID AND t.Type = 40 AND t.Batch = " + _objTrans.BatchID + " Order By t.ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTransDateByBatch(Transaction _objTrans)
        {
            try
            {
                string query = "UPDATE Trans SET fDate=@fDate, Ref=@Ref WHERE Batch=@Batch";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Batch", _objTrans.BatchID));
                parameters.Add(new SqlParameter("@fDate", _objTrans.TransDate));
                parameters.Add(new SqlParameter("@Ref", _objTrans.Ref));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTransDateByBatch(UpdateTransDateByBatchParam _UpdateTransDateByBatchParam, string ConnectionString)
        {
            try
            {
                string query = "UPDATE Trans SET fDate=@fDate, Ref=@Ref WHERE Batch=@Batch";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Batch", _UpdateTransDateByBatchParam.BatchID));
                parameters.Add(new SqlParameter("@fDate", _UpdateTransDateByBatchParam.TransDate));
                parameters.Add(new SqlParameter("@Ref", _UpdateTransDateByBatchParam.Ref));
                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBatchDetailsByID(Transaction _objTrans)
        {
            try
            {
                return _objTrans.DsTrans = SqlHelper.ExecuteDataset(_objTrans.ConnConfig, CommandType.Text, "SELECT ID,Batch,fDate,Type,Line,Ref,fDesc,Amount,Acct,AcctSub,Status,Sel,VInt,VDoub,EN,strRef FROM Trans WHERE ID=" + _objTrans.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTransVoidCheck(Transaction _objTrans)
        {
            try
            {
                string query = "UPDATE Trans SET fDesc=@fDesc WHERE ID=@ID AND Type=@Type";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objTrans.ID));
                parameters.Add(new SqlParameter("@fDesc", _objTrans.TransDescription));
                parameters.Add(new SqlParameter("@Type", _objTrans.Type));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTransVoidCheck(UpdateTransVoidCheckParam _objUpdateTransVoidCheckParam, string ConnectionString)
        {
            try
            {
                string query = "UPDATE Trans SET fDesc=@fDesc WHERE ID=@ID AND Type=@Type";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objUpdateTransVoidCheckParam.ID));
                parameters.Add(new SqlParameter("@fDesc", _objUpdateTransVoidCheckParam.TransDescription));
                parameters.Add(new SqlParameter("@Type", _objUpdateTransVoidCheckParam.Type));
                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTransVoidCheckOpen(Transaction _objTrans)
        {
            try
            {
                string query = "UPDATE Trans SET fDesc=@fDesc,Amount= @Amount WHERE ID=@ID AND Type=@Type";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objTrans.ID));
                parameters.Add(new SqlParameter("@fDesc", _objTrans.TransDescription));
                parameters.Add(new SqlParameter("@Type", _objTrans.Type));
                parameters.Add(new SqlParameter("@Amount", _objTrans.Amount));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateTransVoidCheckOpen(UpdateTransVoidCheckOpenParam _objUpdateTransVoidCheckOpenParam,string ConnectionString)
        {
            try
            {
                string query = "UPDATE Trans SET fDesc=@fDesc,Amount= @Amount WHERE ID=@ID AND Type=@Type";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objUpdateTransVoidCheckOpenParam.ID));
                parameters.Add(new SqlParameter("@fDesc", _objUpdateTransVoidCheckOpenParam.TransDescription));
                parameters.Add(new SqlParameter("@Type", _objUpdateTransVoidCheckOpenParam.Type));
                parameters.Add(new SqlParameter("@Amount", _objUpdateTransVoidCheckOpenParam.Amount));
                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTransVoidCheckByBatch(Transaction _objTrans)
        {
            try
            {
                string query = "UPDATE Trans SET Sel=@Sel WHERE Batch=@Batch AND Type=@Type";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Batch", _objTrans.BatchID));
                parameters.Add(new SqlParameter("@fDesc", _objTrans.TransDescription));
                parameters.Add(new SqlParameter("@Type", _objTrans.Type));
                parameters.Add(new SqlParameter("@Sel", _objTrans.Sel));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTransVoidCheckByBatch(UpdateTransVoidCheckByBatchParam _objUpdateTransVoidCheckByBatchParam, string ConnectionString)
        {
            try
            {
                string query = "UPDATE Trans SET Sel=@Sel WHERE Batch=@Batch AND Type=@Type";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Batch", _objUpdateTransVoidCheckByBatchParam.BatchID));
                parameters.Add(new SqlParameter("@fDesc", _objUpdateTransVoidCheckByBatchParam.TransDescription));
                parameters.Add(new SqlParameter("@Type", _objUpdateTransVoidCheckByBatchParam.Type));
                parameters.Add(new SqlParameter("@Sel", _objUpdateTransVoidCheckByBatchParam.Sel));
                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTransVoidCheckByBatchOpen(Transaction _objTrans)
        {
            try
            {
                string query = "UPDATE Trans SET Sel=@Sel,Amount = @Amount WHERE Batch=@Batch AND Type=@Type";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Batch", _objTrans.BatchID));
                parameters.Add(new SqlParameter("@fDesc", _objTrans.TransDescription));
                parameters.Add(new SqlParameter("@Type", _objTrans.Type));
                parameters.Add(new SqlParameter("@Sel", _objTrans.Sel));
                parameters.Add(new SqlParameter("@Amount", _objTrans.Amount));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateTransVoidCheckByBatchOpen(UpdateTransVoidCheckByBatchOpenParam _objUpdateTransVoidCheckByBatchOpenParam,string ConnectionString)
        {
            try
            {
                string query = "UPDATE Trans SET Sel=@Sel,Amount = @Amount WHERE Batch=@Batch AND Type=@Type";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Batch", _objUpdateTransVoidCheckByBatchOpenParam.BatchID));
                parameters.Add(new SqlParameter("@fDesc", _objUpdateTransVoidCheckByBatchOpenParam.TransDescription));
                parameters.Add(new SqlParameter("@Type", _objUpdateTransVoidCheckByBatchOpenParam.Type));
                parameters.Add(new SqlParameter("@Sel", _objUpdateTransVoidCheckByBatchOpenParam.Sel));
                parameters.Add(new SqlParameter("@Amount", _objUpdateTransVoidCheckByBatchOpenParam.Amount));
                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTransByBatchType(Transaction _objTrans)
        {
            try
            {
                return _objTrans.DsTrans = SqlHelper.ExecuteDataset(_objTrans.ConnConfig, CommandType.Text, "SELECT ID,Batch,fDate,Type,Line,Ref,fDesc,Amount,Acct,AcctSub,Status,Sel,VInt,VDoub,EN,strRef FROM Trans WHERE Batch=" + _objTrans.BatchID +" AND Type="+_objTrans.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTransByBatchType(GetTransByBatchTypeParam _GetTransByBatchTypeParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT ID,Batch,fDate,Type,Line,Ref,fDesc,Amount,Acct,AcctSub,Status,Sel,VInt,VDoub,EN,strRef FROM Trans WHERE Batch=" + _GetTransByBatchTypeParam.BatchID + " AND Type=" + _GetTransByBatchTypeParam.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool ValidateByTimeStamp(Transaction _objTrans)
        {
            try
            {
                return _objTrans.IsAccessible = Convert.ToBoolean(SqlHelper.ExecuteScalar(_objTrans.ConnConfig, "spCheckTimeStampByID", _objTrans.TableName, _objTrans.ID, _objTrans.TimeStamp));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteBillTrans(Transaction _objTrans)
        {
            try
            {
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, "Delete from Trans where batch="+_objTrans.BatchID+" and type=41");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteTransDeposit(Transaction _objTrans)
        {
            try
            {
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, "Delete from TransDeposits where batch=" + _objTrans.BatchID + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteTransChecks(Transaction _objTrans)
        {
            try
            {
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, "Delete from TransChecks where batch=" + _objTrans.BatchID + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTransCheckNoByBatch(Transaction _objTrans)
        {
            try
            {
                string query = "UPDATE Trans SET Ref=@Ref WHERE Batch=@Batch";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Batch", _objTrans.BatchID));
                parameters.Add(new SqlParameter("@Ref", _objTrans.Ref));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTransCheckNoByBatch(UpdateTransCheckNoByBatchParam _UpdateTransCheckNoByBatchParam, string ConnectionString)
        {
            try
            {
                string query = "UPDATE Trans SET Ref=@Ref WHERE Batch=@Batch";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Batch", _UpdateTransCheckNoByBatchParam.BatchID));
                parameters.Add(new SqlParameter("@Ref", _UpdateTransCheckNoByBatchParam.Ref));
                int rowsAffected = SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPhaseByJobId(Transaction objTrans) // get phase expense type details
        {
            try
            {
                var para = new SqlParameter[3];

                para[0] = new SqlParameter
                {
                    ParameterName = "@JobId",
                    SqlDbType = SqlDbType.Int,
                    Value = objTrans.JobInt
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@Type",
                    SqlDbType = SqlDbType.SmallInt,
                    Value = objTrans.Type
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@SearchText",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objTrans.SearchValue
                };
                return objTrans.DsTrans = SqlHelper.ExecuteDataset(objTrans.ConnConfig, "spGetPhaseByJob", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTransDataByBatch(Transaction objTrans)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("   SELECT t.Acct as AcctID, t.AcctSub,(case when(t.Amount > 0) then (t.amount) else 0.00 end) as Debit,(case when(t.Amount < 0) then (t.amount*-1) else 0.00 end) as Credit, \n");
                varname1.Append("       t.Amount, t.Batch, t.EN, t.ID, t.Line, t.Ref, t.Sel,            \n");
                varname1.Append("       t.Status, t.Type, convert(Int, isnull(t.VDoub,0)) as PhaseID, isnull(t.VInt,0) as JobID, t.fDate, t.fDesc, t.fDesc as Description, t.strRef, c.Acct AS AcctNo, c.fDesc AS Account,           \n");
                varname1.Append("       t.TimeStamp,                                                    \n");
                varname1.Append("       isnull(td.IsRecon,0) as IsRecon,                                \n");
                varname1.Append("       l.Tag as Loc,                                                   \n");
                varname1.Append("       CASE WHEN isnull(t.VInt,0) <> 0 THEN convert(VARCHAR, isnull(t.VInt,0))+' '+j.fDesc ELSE j.fDesc END as JobName, jobt.fDesc as Phase, jobt.Line as PhaseID,Br.Name As Company,jobt.Type AS TypeID     \n");
                varname1.Append("       FROM Trans as t                                                 \n");
                varname1.Append("           inner join Chart as c on t.Acct = c.ID                      \n");
                varname1.Append("           left join (select batch as TDbatch, IsRecon FROM transdeposits WHERE batch = '"+ objTrans.BatchID +"'  group by batch, IsRecon) as td on td.TDbatch = t.batch  \n");
                varname1.Append("           left join Job as j on j.ID = t.VInt                         \n");
                varname1.Append("           left join Loc as l on l.Loc = j.Loc                         \n");
                varname1.Append("           LEFT JOIN JobI as JI ON JI.Job = t.VInt AND JI.TransID = t.ID                         \n");                
                varname1.Append("           left join JobTItem as jobt on jobt.Line = t.VDoub and jobt.Job = t.VInt and jobt.Type = JI.Type-- in(1,2,0)   \n");
                varname1.Append("            left join Bank B on C.ID=b.Chart                         \n");
                varname1.Append("           left join Rol  r on  B.Rol=r.ID                        \n");
                varname1.Append("           left outer join Branch br on br.ID = r.EN                        \n");
                varname1.Append("           WHERE (t.Type = 50 or t.Type = 30 or t.Type = 31)                                           \n");
                varname1.Append("           AND t.Batch = '" + objTrans.BatchID + "' Order By t.Line     \n");

                return objTrans.DsTrans = SqlHelper.ExecuteDataset(objTrans.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllPhaseByJobID(Transaction objTrans)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("        SELECT TOP 50 jobt.ID, jobt.JobT, jobt.Job, jobt.Type, jobt.fDesc, jobt.Code, jobt.Actual, jobt.Line, l.Tag as LocName, j.fDesc as JobName ,   \n");
                varname.Append("             (CASE jobt.Type WHEN 0 THEN  m.Type ELSE b.Type END) As bomtypeID, (CASE jobt.Type WHEN 0 THEN  m.MilestoneName ELSE bt.Type END) AS bomtype,(Select GroupName from tblEstimateGroup where Id=jobt.GroupId) As GroupName    \n");
                //varname.Append("             b.Type As bomtypeID,(CASE jobt.Type WHEN 0 THEN  'Revenues' ELSE bt.Type END) AS bomtype    \n");
                varname.Append("            FROM JobTItem as jobt   \n");
                varname.Append("            inner join job as j on j.ID = jobt.Job  \n");
                varname.Append("            inner join loc as l on l.Loc = j.Loc    \n");
                varname.Append("            left join BOM as b on b.JobTItemID = jobt.ID    \n");
                varname.Append("            left join BOMT as bt on b.Type = bt.ID    \n");
                varname.Append("            LEFT JOIN Milestone as m ON m.JobTItemID = jobt.ID     \n");
                varname.Append("            WHERE   jobt.Type in(1 ,2,0)                \n");
                if(objTrans.JobInt > 0)
                {
                    varname.Append("       AND  jobt.Job = '"+ objTrans.JobInt+"'  \n");
                }
                else
                {
                    varname.Append("        AND jobt.Job <> 0 OR jobt.Job <> null       \n");
                }

                if (objTrans.SearchValue.Trim() != "")
                {
                    varname.Append("       AND  (dbo.RemoveSpecialChars(jobt.fDesc) LIKE '%" + objTrans.SearchValue + "%')  \n");
                }
                
                return objTrans.DsTrans = SqlHelper.ExecuteDataset(objTrans.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int ProcessRecurJE(Journal objJournal)
        {
            SqlParameter paraRef = new SqlParameter();
            paraRef.ParameterName = "Ref";
            paraRef.SqlDbType = SqlDbType.Int;
            paraRef.Value = objJournal.Ref;

            SqlParameter paraReturn = new SqlParameter();
            paraReturn.ParameterName = "returnval";
            paraReturn.SqlDbType = SqlDbType.Int;
            paraReturn.Direction = ParameterDirection.ReturnValue;

            try
            {
                SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.StoredProcedure, "spProcessRecurTransaction", paraRef, paraReturn);
                return Convert.ToInt32(paraReturn.Value);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public int AddJE(Journal objJournal)
        {
            try
            {
                var para = new SqlParameter[10];

                para[0] = new SqlParameter
                {
                    ParameterName = "@JEItems",
                    SqlDbType = SqlDbType.Structured,
                    Value = objJournal.DtTrans
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objJournal.fDate
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@fDesc",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJournal.fDesc
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "@Internal",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJournal.Internal
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "@Frequency",
                    SqlDbType = SqlDbType.Int,
                    Value = objJournal.Frequency
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "@IsJobSpec",
                    SqlDbType = SqlDbType.Bit,
                    Value = objJournal.IsJobSpec
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "@IsRecur",
                    SqlDbType = SqlDbType.Bit,
                    Value = objJournal.IsRecurring
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "UpdatedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJournal.UserName
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "OriginalJE",
                    SqlDbType = SqlDbType.Int,
                    Value = objJournal.OriginalJE
                };
                SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.StoredProcedure, "spAddJE", para);
                return Convert.ToInt32(para[7].Value);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public int UpdateJE(Journal objJournal)
        {
            try
            {
                var para = new SqlParameter[11];

                para[0] = new SqlParameter
                {
                    ParameterName = "@JEItems",
                    SqlDbType = SqlDbType.Structured,
                    Value = objJournal.DtTrans
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objJournal.fDate
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@fDesc",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJournal.fDesc
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "@Internal",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJournal.Internal
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "@Frequency",
                    SqlDbType = SqlDbType.Int,
                    Value = objJournal.Frequency
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "@IsJobSpec",
                    SqlDbType = SqlDbType.Bit,
                    Value = objJournal.IsJobSpec
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "@IsRecur",
                    SqlDbType = SqlDbType.Bit,
                    Value = objJournal.IsRecurring
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "@Batch",
                    SqlDbType =  SqlDbType.Int,
                    Value = objJournal.BatchID
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "@Ref",
                    SqlDbType = SqlDbType.Int,
                    Value = objJournal.Ref
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "returnval",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.ReturnValue
                };

                para[10] = new SqlParameter
                {
                    ParameterName = "UpdatedBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objJournal.UserName
                };

                SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.StoredProcedure, "spUpdateJE", para);

                return Convert.ToInt32(para[9].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetJournalEntryItemData(Journal objJournal)
        {
            try
            {
                var para = new SqlParameter[1];

                para[0] = new SqlParameter
                {
                    ParameterName = "@CSVItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = objJournal.DtTrans
                };
                
                return SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.StoredProcedure, "spGetJournalEntryItemData", para); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetJournalEntryDetailed(Journal objJournal, List<RetainFilter> filters)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT \n");
                sb.Append("	fDate, \n");
                sb.Append("	Ref, \n");
                sb.Append("	Internal, \n");
                sb.Append("	fDesc, \n");
                sb.Append("	Batch, \n");
                sb.Append("	ISNULL((SELECT TOP 1 Sel FROM Trans WHERE Batch = GLA.Batch AND Sel = 1),0) AS IsCleared, \n");
                sb.Append("	ISNULL((SELECT SUM(Amount) FROM Trans WHERE Batch = GLA.Batch AND Trans.Type in (30,31,50) AND Amount > 0),0.00) AS Debit, \n");
                sb.Append("	ISNULL((SELECT SUM(Amount * -1) FROM Trans WHERE Batch = GLA.Batch AND Trans.Type in (30,31,50) AND Amount < 0),0.00) AS Credit \n");
                sb.Append("FROM GLA \n");
                sb.Append("WHERE fDate >= '" + objJournal.StartDate + "' \n");
                sb.Append("	AND fDate <= '" + objJournal.EndDate + "' \n");

                // Grid filter
                foreach (var filter in filters)
                {
                    if (filter.FilterColumn == "fDate")
                    {
                        sb.Append(" AND fDate LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "Ref")
                    {
                        sb.Append(" AND Ref LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "Internal")
                    {
                        sb.Append(" AND Internal LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "fDesc")
                    {
                        sb.Append(" AND fDesc LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "Debit")
                    {
                        sb.Append(" AND ISNULL((SELECT SUM(Amount) FROM Trans WHERE Batch = GLA.Batch AND Trans.Type in (30,31,50) AND Amount > 0),0.00) = " + filter.FilterValue + " \n");
                    }
                    if (filter.FilterColumn == "Credit")
                    {
                        sb.Append(" AND ISNULL((SELECT SUM(Amount * -1) FROM Trans WHERE Batch = GLA.Batch AND Trans.Type in (30,31,50) AND Amount < 0),0.00) = " + filter.FilterValue + " \n");
                    }
                }


                sb.Append("SELECT \n");
                sb.Append("	t.Acct AS AcctID, \n");
                sb.Append("	t.AcctSub,\n");
                sb.Append("	(CASE WHEN(t.Amount > 0) THEN (t.amount) ELSE 0.00 END) AS Debit, \n");
                sb.Append("	(CASE WHEN(t.Amount < 0) THEN (t.amount*-1) ELSE 0.00 END) AS Credit, \n");
                sb.Append("    t.Amount, \n");
                sb.Append("	t.Batch, \n");
                sb.Append("	t.ID, \n");
                sb.Append("	t.Line,\n");
                sb.Append("	t.Ref, \n");
                sb.Append("	t.Sel, \n");
                sb.Append("	t.Type, \n");
                sb.Append("	ISNULL(t.VInt,0) AS JobID, \n");
                sb.Append("	t.fDate, \n");
                sb.Append("	t.fDesc, \n");
                sb.Append("	t.fDesc AS Description, \n");
                sb.Append("	t.strRef, \n");
                sb.Append("	c.Acct AS AcctNo, \n");
                sb.Append("	c.fDesc AS Account, \n");
                sb.Append("    l.Tag AS Loc, \n");
                sb.Append("    j.fDesc AS JobName,\n");
                sb.Append("	jt.fDesc AS Phase, \n");
                sb.Append("	jt.Line AS PhaseID\n");
                sb.Append("FROM Trans AS t  \n");
                sb.Append("	INNER JOIN GLA gl ON gl.Batch = t.Batch \n");
                sb.Append("    INNER JOIN Chart AS c ON t.Acct = c.ID \n");
                sb.Append("    LEFT JOIN Job AS j ON j.ID = t.VInt \n");
                sb.Append("    LEFT JOIN Loc AS l ON l.Loc = j.Loc \n");
                sb.Append("	LEFT JOIN JobTItem jt ON jt.Line = t.VDoub AND jt.Job = t.VInt AND jt.Type in(1,2) \n");
                sb.Append("WHERE (t.Type = 50 or t.Type = 30 or t.Type = 31) \n");
                sb.Append("    AND gl.fDate >= '" + objJournal.StartDate + "' \n");
                sb.Append("	AND gl.fDate <= '" + objJournal.EndDate + "' \n");

                // Grid filter
                foreach (var filter in filters)
                {
                    if (filter.FilterColumn == "fDate")
                    {
                        sb.Append(" AND gl.fDate LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "Ref")
                    {
                        sb.Append(" AND gl.Ref LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "Internal")
                    {
                        sb.Append(" AND gl.Internal LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "fDesc")
                    {
                        sb.Append(" AND gl.fDesc LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "Debit")
                    {
                        sb.Append(" AND ISNULL((SELECT SUM(Amount) FROM Trans WHERE Batch = gl.Batch AND Trans.Type in (30,31,50) AND Amount > 0),0.00) = " + filter.FilterValue + " \n");
                    }
                    if (filter.FilterColumn == "Credit")
                    {
                        sb.Append(" AND ISNULL((SELECT SUM(Amount * -1) FROM Trans WHERE Batch = gl.Batch AND Trans.Type in (30,31,50) AND Amount < 0),0.00) = " + filter.FilterValue + " \n");
                    }
                }

                sb.Append("ORDER BY fDate, Ref  \n");

                return SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetJournalEntryByEntryNo(Journal objJournal)
        {
            try
            {
                var query = "SELECT * FROM GLA WHERE Internal = '" + objJournal.Internal + "'";
                if (objJournal.IsRecurring)
                {
                    query = "SELECT * FROM GLARecur WHERE Internal = '" + objJournal.Internal + "'";
                }

                return SqlHelper.ExecuteDataset(objJournal.ConnConfig, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
