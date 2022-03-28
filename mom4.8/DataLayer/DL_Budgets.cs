using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using BusinessEntity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationBlocks.Data;

namespace DataLayer
{
    public class DL_Budgets
    {
        public int AddBudget(Budget budget)
        {
            String strConnString = budget.ConnConfig;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if (budget.BudgetId > 0)
            {
                cmd.CommandText = "Budget_Update";
            }
            else
            {
                cmd.CommandText = "Budget_Add";
            }
            SqlParameter param = new SqlParameter
            {
                ParameterName = "@BudgetID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };

            if (budget.BudgetId > 0)
            {
                param.Value = budget.BudgetId;
                cmd.Parameters.Add(param);
            }
            else
            {
                cmd.Parameters.Add(param);
            }
            cmd.Parameters.Add("@Budget", SqlDbType.VarChar).Value = budget.BudgetValue;
            cmd.Parameters.Add("@Year", SqlDbType.Int).Value = budget.Year;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            if (param.Value != DBNull.Value)
                return Convert.ToInt32(param.Value);
            else
                return 0;
        }

        public void UpdateBudgetName(Budget budget)
        {
            try
            {
                String strConnString = budget.ConnConfig;
                SqlConnection con = new SqlConnection(strConnString);
                String sql = "UPDATE Budget SET Budget = '" + budget.BudgetValue + "' WHERE BudgetID = " + budget.BudgetId;
                SqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddAccount(Account accounts)
        {
            String strConnString = accounts.ConnConfig;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Account_Update";
            SqlParameter param = new SqlParameter
            {
                ParameterName = "@AccountID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(param);
            cmd.Parameters.Add("@Acct", SqlDbType.VarChar).Value = accounts.Acct.Trim();
            cmd.Parameters.Add("@fDesc", SqlDbType.VarChar).Value = accounts.fDesc.Trim();
            cmd.Parameters.Add("@Balance", SqlDbType.VarChar).Value = accounts.Balance;
            cmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = accounts.Type;
            cmd.Parameters.Add("@Sub", SqlDbType.VarChar).Value = accounts.Sub;
            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = accounts.Remarks;
            cmd.Parameters.Add("@Control", SqlDbType.VarChar).Value = accounts.Control;
            cmd.Parameters.Add("@inUse", SqlDbType.VarChar).Value = accounts.InUse;
            cmd.Parameters.Add("@Detail", SqlDbType.VarChar).Value = accounts.Detail;
            cmd.Parameters.Add("@CAlias", SqlDbType.VarChar).Value = accounts.CAlias;
            cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = accounts.Status;
            cmd.Parameters.Add("@Sub2", SqlDbType.VarChar).Value = accounts.Sub2;
            cmd.Parameters.Add("@DAT", SqlDbType.VarChar).Value = accounts.DAT;
            cmd.Parameters.Add("@Branch", SqlDbType.VarChar).Value = accounts.Branch;
            cmd.Parameters.Add("@CostCenter", SqlDbType.VarChar).Value = accounts.CostCenter;
            cmd.Parameters.Add("@AccRoot", SqlDbType.VarChar).Value = accounts.AccRoot;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            return Convert.ToInt32(param.Value);
        }

        public void AddAccountDetails(AccountDetail accountDetail)
        {
            String strConnString = accountDetail.ConnConfig;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "AccountDetails_Update";
            SqlParameter param = new SqlParameter
            {
                ParameterName = "@AccountDetailID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(param);
            cmd.Parameters.Add("@AccountID", SqlDbType.Int).Value = accountDetail.AccountId;
            cmd.Parameters.Add("@BudgetID", SqlDbType.Int).Value = accountDetail.BudgetId;
            cmd.Parameters.Add("@Period", SqlDbType.Int).Value = accountDetail.Period;
            cmd.Parameters.Add("@Credit", SqlDbType.Decimal).Value = accountDetail.Credit;
            cmd.Parameters.Add("@Debit", SqlDbType.Decimal).Value = accountDetail.Debit;
            cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = accountDetail.Amount;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }

        public void AddBudgetAccountDetails(AccountDetail accountDetail)
        {
            String strConnString = accountDetail.ConnConfig;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "BudgetAccountDetails_Update";
            SqlParameter param = new SqlParameter
            {
                ParameterName = "@AccountDetailID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(param);
            cmd.Parameters.Add("@AccountID", SqlDbType.Int).Value = accountDetail.AccountId;
            cmd.Parameters.Add("@BudgetID", SqlDbType.Int).Value = accountDetail.BudgetId;
            cmd.Parameters.Add("@Total", SqlDbType.Decimal).Value = accountDetail.Total;
            cmd.Parameters.Add("@Jan", SqlDbType.Decimal).Value = accountDetail.Jan;
            cmd.Parameters.Add("@Feb", SqlDbType.Decimal).Value = accountDetail.Feb;
            cmd.Parameters.Add("@Mar", SqlDbType.Decimal).Value = accountDetail.Mar;
            cmd.Parameters.Add("@Apr", SqlDbType.Decimal).Value = accountDetail.Apr;
            cmd.Parameters.Add("@May", SqlDbType.Decimal).Value = accountDetail.May;
            cmd.Parameters.Add("@Jun", SqlDbType.Decimal).Value = accountDetail.Jun;
            cmd.Parameters.Add("@Jul", SqlDbType.Decimal).Value = accountDetail.Jul;
            cmd.Parameters.Add("@Aug", SqlDbType.Decimal).Value = accountDetail.Aug;
            cmd.Parameters.Add("@Sep", SqlDbType.Decimal).Value = accountDetail.Sep;
            cmd.Parameters.Add("@Oct", SqlDbType.Decimal).Value = accountDetail.Oct;
            cmd.Parameters.Add("@Nov", SqlDbType.Decimal).Value = accountDetail.Nov;
            cmd.Parameters.Add("@Dec", SqlDbType.Decimal).Value = accountDetail.Dec;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }

        public DataSet GetAccountID(string connString, string acct, string fDesc, string typeName)
        {
            String strConnString = connString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetAccountID";
            cmd.Parameters.Add("@fDesc", SqlDbType.VarChar).Value = fDesc;
            cmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = typeName;
            cmd.Parameters.Add("@Acct", SqlDbType.VarChar).Value = acct;
            cmd.Connection = con;
            try
            {
                con.Open();
                var ds = new DataSet();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "Accounts");
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }

        public int GetBudgetID(string connString, string budgetName)
        {
            int budgetID = 0;
            String strConnString = connString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "Budget_GetBudgetID";
            SqlParameter param = new SqlParameter
            {
                ParameterName = "@BudgetID",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(param);
            cmd.Parameters.Add("@Budget", SqlDbType.VarChar).Value = budgetName;
            cmd.Connection = con;
            try
            {
                con.Open();
                budgetID = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            if (param.Value != DBNull.Value)
                return Convert.ToInt32(param.Value);
            else
                return 0;
        }

        public DataSet GetBudgets(string connString, int? year)
        {
            String strConnString = connString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Budget_GetAllBudgets";
            cmd.Parameters.Add("@Year", SqlDbType.Int).Value = year;
            cmd.Connection = con;
            try
            {
                con.Open();
                var ds = new DataSet();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "Budgets");
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }

        public DataSet GetIncomeStatement12PeriodForBudgets(Chart objChart)
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "fDate";
                param.SqlDbType = SqlDbType.DateTime;
                param.Value = objChart.EndDate;

                return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.StoredProcedure, "spGetIncomeStatement12PeriodForBudgets", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetBudgetData(string connString, string budgetName)
        {
            String strConnString = connString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Budget_GetBudgetDataByBudgetName";
            cmd.Parameters.Add("@BudgetName", SqlDbType.VarChar).Value = budgetName;
            cmd.Connection = con;
            try
            {
                con.Open();
                var ds = new DataSet();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "BudgetData");
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }

        public DataSet GetSummaryBudgetData(Chart objChart, string budgetName)
        {
            String strConnString = objChart.ConnConfig;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetSummaryBudgetDataByBudgetName";
            cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = objChart.StartDate;
            cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = objChart.EndDate;
            cmd.Parameters.Add("@BudgetName", SqlDbType.VarChar).Value = budgetName;
            cmd.Connection = con;
            try
            {
                con.Open();
                var ds = new DataSet();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "BudgetData");
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }

        public DataSet GetBudgetsData(string connString, string budgetName, string types)
        {
            String strConnString = connString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Budget_GetBudgetsDataByBudgetName";
            cmd.Parameters.Add("@BudgetName", SqlDbType.VarChar).Value = budgetName;
            cmd.Parameters.Add("@AccountType", SqlDbType.VarChar).Value = types;

            cmd.Connection = con;
            try
            {
                con.Open();
                var ds = new DataSet();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "BudgetData");
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }

        public void DeleteBudget(string connString, string budgetID)
        {
            String strConnString = connString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Budget_Delete";
            cmd.Parameters.Add("@BudgetID", SqlDbType.Int).Value = int.Parse(budgetID);
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }

        public DataSet GetTotalBudget(string connString, DateTime startDate, DateTime endDate, string budgetName)
        {
            String strConnString = connString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetTotalAmountByPeriod";
            cmd.Parameters.Add("@BudgetName", SqlDbType.VarChar).Value = budgetName;
            cmd.Parameters.Add("@StartDate", SqlDbType.VarChar).Value = startDate;
            cmd.Parameters.Add("@EndDate", SqlDbType.VarChar).Value = endDate;
            cmd.Connection = con;
            try
            {
                con.Open();
                var ds = new DataSet();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "BudgetData");
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }

        public DataSet GetTotalActual(string connString, DateTime startDate, DateTime endDate)
        {
            String strConnString = connString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetTotalActualByPeriod";
            cmd.Parameters.Add("@StartDate", SqlDbType.VarChar).Value = startDate;
            cmd.Parameters.Add("@EndDate", SqlDbType.VarChar).Value = endDate;
            cmd.Connection = con;
            try
            {
                con.Open();
                var ds = new DataSet();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "Trans");
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }

        public DataSet GetBudgetsByYear(string connString, int startDate, int endDate)
        {
            String strConnString = connString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Budget_GetBudgetsByDate";
            cmd.Parameters.Add("@FromYear", SqlDbType.Int).Value = startDate;
            cmd.Parameters.Add("@ToYear", SqlDbType.Int).Value = endDate;
            cmd.Connection = con;
            try
            {
                con.Open();
                var ds = new DataSet();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "BudgetVSActual");
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }

        public DataSet GetBudgetVSActual(string connString, DateTime startDate, DateTime endDate, string budgetName, bool includeZero)
        {
            String strConnString = connString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetBudgetVSActualData";
            cmd.Parameters.Add("@BudgetName", SqlDbType.VarChar).Value = budgetName;
            cmd.Parameters.Add("@StartDate", SqlDbType.VarChar).Value = startDate;
            cmd.Parameters.Add("@EndDate", SqlDbType.VarChar).Value = endDate;
            cmd.Parameters.Add("@IncludeZero", SqlDbType.Bit).Value = includeZero;
            cmd.Connection = con;
            try
            {
                con.Open();
                var ds = new DataSet();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "BudgetData");
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }       
       
        public DataSet Get12MonthBudgetVSActual(string connString, DateTime startDate, DateTime endDate, string budgetName, string centers)
        {
            String strConnString = connString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Get12MonthActualvsBudgetData";
            cmd.Parameters.Add("@BudgetName", SqlDbType.VarChar).Value = budgetName;
            cmd.Parameters.Add("@StartDate", SqlDbType.VarChar).Value = startDate;
            cmd.Parameters.Add("@EndDate", SqlDbType.VarChar).Value = endDate;
            cmd.Parameters.Add("@Centers", SqlDbType.VarChar).Value = centers;
            cmd.Connection = con;

            try
            {
                con.Open();
                var ds = new DataSet();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "BudgetData");
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }

        }

        public DataSet GetChildAccounts(string connString, string acct)
        {
            String strConnString = connString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetChildAccounts";
            cmd.Parameters.Add("@Acct", SqlDbType.VarChar).Value = acct;
            cmd.Connection = con;
            try
            {
                con.Open();
                var ds = new DataSet();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "Accounts");
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        public DataSet GetGLAccounts(string connString)
        {
            String strConnString = connString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetAllGLAccounts";
            cmd.Connection = con;
            try
            {
                con.Open();
                var ds = new DataSet();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "GLAccounts");
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }


        }

        public DataSet GetAllActiveAccounts(string connString)
        {
            String strConnString = connString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetAllActiveAccounts";
            cmd.Connection = con;
            try
            {
                con.Open();
                var ds = new DataSet();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "GLAccounts");
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }


        }

    }

}
