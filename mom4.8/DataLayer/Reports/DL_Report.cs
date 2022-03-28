using BusinessEntity;
using BusinessEntity.Reports.IncomeStatements;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataLayer
{
    public class DL_Report
    {
        //public DataSet GetChartByAcctType(Chart _objChart)
        //{
        //    try
        //    {
        //        return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT ID, Acct, fDesc, Balance, Type, Sub, Remarks, Control, InUse, Detail, CAlias, Status, Sub2, DAT, Branch, CostCenter, AcctRoot, QBAccountID, LastUpdateDate FROM Chart WHERE Balance <> '0.00' AND Type = " + _objChart.Type + " Order by ID");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public DataSet GetTypeForBalanceSheet(Chart _objChart) //For Balance sheet
        {
            try
            {
                //StringBuilder varname1 = new StringBuilder();
                //varname1.Append("SELECT Type AS ID,  \n");
                //varname1.Append("     (CASE Type WHEN 0 THEN 'Asset' \n");
                //varname1.Append("     WHEN 1 THEN 'Liability'  \n");
                //varname1.Append("     WHEN 2 THEN 'Equity'  \n");
                //varname1.Append("     WHEN 3 THEN 'Revenue' \n");
                //varname1.Append("     WHEN 4 THEN 'Cost' \n");
                //varname1.Append("     WHEN 5 THEN 'Expense' \n");
                //varname1.Append("     WHEN 6 THEN 'Bank' \n");
                //varname1.Append("     END) AS fDesc FROM Chart GROUP BY Type \n");

                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT Type AS ID,  \n");
                varname1.Append("     (CASE Type WHEN 0 THEN 'Asset' \n");
                varname1.Append("     WHEN 1 THEN 'Liability'  \n");
                varname1.Append("     WHEN 2 THEN 'Equity'  \n");
                varname1.Append("     END) AS fDesc FROM Chart \n");
                varname1.Append("     WHERE Type < 3  \n");
                varname1.Append("     GROUP BY Type \n");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetDataForBalanceSheet(Chart _objChart) //For Balance sheet
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT  \n");
                varname1.Append("	 c.fDesc, \n");
                varname1.Append("    t.Acct, \n");
                varname1.Append("    SUM(t.Amount) AS Balance,  \n");
                varname1.Append("    '" + _objChart.Type + "' AS Type  \n");
                varname1.Append("    FROM Trans as t, Chart as c \n");
                varname1.Append("    WHERE c.ID = t.Acct \n");
                //varname1.Append("    AND c.Type < 3  \n");
                varname1.Append("    AND c.Type = " + _objChart.Type + " AND t.fDate > '" + _objChart.StartDate.Date + "' AND t.fDate < '" + _objChart.EndDate.Date + "' ");
                varname1.Append("    GROUP BY t.Acct, c.fDesc \n");
                varname1.Append("    HAVING SUM(t.Amount) <> 0.00 \n");
                varname1.Append("    ORDER BY t.Acct \n");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTypeForTrialBalance(Chart _objChart) //For Trial Balance
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT Type AS ID,  \n");
                varname1.Append("     (CASE Type WHEN 0 THEN 'Asset' \n");
                varname1.Append("     WHEN 1 THEN 'Liability'  \n");
                varname1.Append("     WHEN 2 THEN 'Equity'  \n");
                varname1.Append("     WHEN 3 THEN 'Revenue'  \n");
                varname1.Append("     WHEN 5 THEN 'Expense'  \n");
                varname1.Append("     END) AS fDesc FROM Chart \n");
                varname1.Append("     WHERE Type < 4 OR Type = 5 \n");
                varname1.Append("     GROUP BY Type \n");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTypeForIncome(Chart _objChart) //For Income Statement
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT Type AS ID,  \n");
                varname1.Append("     (CASE Type WHEN 3 THEN 'Revenue' \n");
                varname1.Append("     WHEN 5 THEN 'Expense' \n");
                varname1.Append("     END) AS fDesc FROM Chart \n");
                varname1.Append("     WHERE  \n");
                //varname1.Append("     Type = 1 OR \n");
                varname1.Append("     Type = 3 OR Type = 5 \n");
                varname1.Append("     GROUP BY Type \n");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetIncomeTotal(Chart _objChart)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT Type AS ID,  \n");
                varname.Append("     (CASE Type WHEN 3 THEN 'Revenue' \n");
                varname.Append("     WHEN 5 THEN 'Expense' \n");
                varname.Append("     END) AS fDesc FROM Chart \n");
                varname.Append("     WHERE  \n");
                varname.Append("     Type = 3 OR Type = 5 \n");
                varname.Append("     GROUP BY Type \n");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetSubCategory(Chart _objChart)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT Sub \n");
                varname.Append("FROM Chart \n");
                varname.Append("WHERE Type = " + _objChart.Type + " \n");
                varname.Append("GROUP BY Sub \n");
                varname.Append("HAVING Sub <> '' \n");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAcctDetailsBySubCat(Chart _objChart)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT \n");
                varname.Append("c.fDesc, t.Acct, \n");
                varname.Append("SUM(t.Amount) AS Balance, \n");
                varname.Append("'" + _objChart.Type + "' AS Type \n");
                varname.Append("FROM Trans AS t, Chart AS c \n");
                varname.Append("WHERE c.ID = t.Acct AND c.Type = " + _objChart.Type + " AND t.fDate > '" + _objChart.StartDate.Date + "' AND t.fDate < '" + _objChart.EndDate.Date + "'  \n");
                varname.Append("AND c.Sub Like '" + _objChart.Sub + "' \n");
                varname.Append("GROUP BY t.Acct, c.fDesc \n");
                varname.Append("HAVING SUM(t.Amount) <> 0.00 \n");
                varname.Append("ORDER BY t.Acct \n");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetOtherAcctDetails(Chart _objChart)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT \n");
                varname.Append("c.fDesc, t.Acct, \n");
                varname.Append("SUM(t.Amount) AS Balance, \n");
                varname.Append("'" + _objChart.Type + "' AS Type \n");
                varname.Append("FROM Trans AS t, Chart AS c \n");
                varname.Append("WHERE c.ID = t.Acct AND c.Type = " + _objChart.Type + " AND t.fDate > '" + _objChart.StartDate.Date + "' AND t.fDate < '" + _objChart.EndDate.Date + "'  \n");
                varname.Append("AND (c.Sub IS NULL OR c.Sub = '') \n");
                varname.Append("GROUP BY t.Acct, c.fDesc \n");
                varname.Append("HAVING SUM(t.Amount) <> 0.00 \n");
                varname.Append("ORDER BY t.Acct \n");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetFiscalYearData(User _objPropUser)
        {
            try
            {
                return _objPropUser.YE = Convert.ToInt32(SqlHelper.ExecuteScalar(_objPropUser.ConnConfig, CommandType.Text, "SELECT ISNULL(YE,'') FROM Control"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //API
        public int GetFiscalYear(User _objPropUser)
        {
            try
            {
                return _objPropUser.YE = Convert.ToInt32(SqlHelper.ExecuteScalar(_objPropUser.ConnConfig, CommandType.Text, "spGetFiscalYear"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBalanceSheetDetails(Chart _objChart, bool includeZero = false)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("   SELECT *,(select min(isnull(fDate,getdate())) from Trans) as StartDate FROM (         \n");
                varname1.Append("   SELECT t.Acct, t.fDesc,t.NewAcct As NewAcct , t.NewfDesc As NewfDesc,sum(isnull(t.Amount,0)) as Amount, t.Type, t.TypeName, t.Sub, t.Url  from( ");
                varname1.Append("SELECT                         \n");
                varname1.Append("	 c.Acct+'  '+c.fDesc AS fDesc, \n");
                varname1.Append("c.Acct As NewAcct , c.fDesc As NewfDesc, \n");
                varname1.Append("    c.ID As Acct,                    \n");
                //varname1.Append("    SUM(t.Amount) AS Balance,  \n");
                //varname1.Append("    t.Amount,                  \n");
                varname1.Append("    (CASE c.Type WHEN 1 THEN   \n");
                varname1.Append("       (isnull(t.Amount,0) * -1)         \n");       //change by Mayuri on 16th Sep,16 to show Liability amount 
                varname1.Append("       WHEN 2 THEN (isnull(t.Amount,0) * -1)         \n");
                varname1.Append("       ELSE isnull(t.Amount,0) END) as Amount, \n");
                varname1.Append("   case c.Type when 6 then 0 else c.Type end as Type,    \n");
                varname1.Append("   (CASE c.Type WHEN 0 THEN 'Asset' \n");
                varname1.Append("    WHEN 1 THEN 'Liability'    \n");
                varname1.Append("    WHEN 2 THEN 'Equity'       \n");
                varname1.Append("    WHEN 3 THEN 'Revenue'      \n");
                varname1.Append("    WHEN 4 THEN 'Cost'         \n");
                varname1.Append("    WHEN 5 THEN 'Expense'      \n");
                varname1.Append("    WHEN 6 THEN 'Asset'  --Bank       \n");        //change to show bank account under Asset type accounts
                varname1.Append("    END) AS TypeName,          \n");
                varname1.Append("     (CASE c.Sub WHEN '' THEN  \n");
                varname1.Append("     (CASE c.Type WHEN 0 THEN 'Asset'  \n");
                varname1.Append("         WHEN 1 THEN 'Liability'   \n");
                varname1.Append("         WHEN 2 THEN 'Equity'      \n");
                varname1.Append("         WHEN 3 THEN 'Revenue'     \n");
                varname1.Append("         WHEN 4 THEN 'Cost'        \n");
                varname1.Append("         WHEN 5 THEN 'Expense'     \n");
                varname1.Append("         WHEN 6 THEN 'Asset'        \n");
                varname1.Append("   END)                            \n");
                varname1.Append("   ELSE c.Sub END) As Sub, '' as Url   \n");
                varname1.Append("    FROM Trans as t RIGHT JOIN Chart as c  ON c.ID = t.Acct  \n");
                varname1.Append("    WHERE (c.Type < 3 OR c.Type = 6) AND t.Amount <> 0.00    \n");
                varname1.Append("    AND t.fDate <= '" + _objChart.EndDate.Date + "'    \n");
                //varname1.Append("    AND t.fDate >= '" + _objChart.StartDate.Date + "' AND t.fDate <= '" + _objChart.EndDate.Date + "' \n");
                varname1.Append("      ) t  \n");
                varname1.Append("       GROUP BY t.Acct, t.fDesc, t.Type, t.TypeName, t.Sub, t.Url ,t.NewAcct,t.NewfDesc \n");

                if (!includeZero)
                {
                    varname1.Append("       HAVING SUM(ISNULL(Amount,0)) <> 0       \n");
                }

                varname1.Append("    UNION        \n");
                varname1.Append("       SELECT ID as Acct, Acct +'  '+fDesc as fDesc, Acct As NewAcct , fDesc As NewfDesc,    \n");
                varname1.Append("           isnull((select (sum((isnull(Amount,0))) *-1) as Amount from trans where acct = c.ID and fDate <= '" + _objChart.EndDate.Date + "'),0) as Amount,  \n");
                varname1.Append("           Type, 'Equity' as TypeName,             \n");
                varname1.Append("           (CASE Sub WHEN '' THEN                  \n");
                varname1.Append("               (CASE Type WHEN 0 THEN 'Asset'      \n");
                varname1.Append("                   WHEN 1 THEN 'Liability'         \n");
                varname1.Append("                   WHEN 2 THEN 'Equity'            \n");
                varname1.Append("                   WHEN 3 THEN 'Revenue'           \n");
                varname1.Append("                   WHEN 4 THEN 'Cost'              \n");
                varname1.Append("                   WHEN 5 THEN 'Expense'           \n");
                varname1.Append("                   WHEN 6 THEN 'Asset'             \n");
                varname1.Append("                END)                               \n");
                varname1.Append("           ELSE Sub END) As Sub,                   \n");
                varname1.Append("       '' as Url                                   \n");
                varname1.Append("       FROM Chart c WHERE ID IN (select ID FROM chart where DefaultNo = 'D3130')       \n");
                varname1.Append("           ) as t          \n");
                varname1.Append("           	   ORDER BY t.fDesc       \n");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetIncomeStatementDetails(Chart _objChart)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();

                varname1.Append("SELECT \n");
                varname1.Append("	c.ID AS Acct,\n");
                varname1.Append("	c.Acct + ' ' + c.fDesc AS fDesc, \n");
                varname1.Append("	SUM(CASE c.Type \n");
                varname1.Append("		WHEN 3 THEN (ISNULL(t.Amount, 0) * -1) \n");
                varname1.Append("		WHEN 8 THEN (ISNULL(t.Amount, 0) * -1) \n");
                varname1.Append("		ELSE ISNULL(t.Amount, 0) \n");
                varname1.Append("	END) AS Amount,\n");
                varname1.Append("	c.Type AS Type, \n");
                varname1.Append("	CASE c.Type \n");
                varname1.Append("		WHEN 0 THEN 'Asset'\n");
                varname1.Append("		WHEN 1 THEN 'Liability'\n");
                varname1.Append("		WHEN 2 THEN 'Equity' \n");
                varname1.Append("		WHEN 3 THEN 'Revenue'\n");
                varname1.Append("		WHEN 4 THEN 'Cost of Sales'\n");
                varname1.Append("		WHEN 5 THEN 'Expense'\n");
                varname1.Append("		WHEN 6 THEN 'Bank' \n");
                varname1.Append("		WHEN 7 THEN 'Non-Posting' \n");
                varname1.Append("		WHEN 8 THEN 'Other Income (Expense)' \n");
                varname1.Append("		WHEN 9 THEN 'Provisions for Income Taxes' \n");
                varname1.Append("	END AS TypeName,\n");
                varname1.Append("	CASE c.Sub WHEN '' THEN\n");
                varname1.Append("		(CASE c.Type WHEN 0 THEN 'Asset'\n");
                varname1.Append("			WHEN 1 THEN 'Liability' \n");
                varname1.Append("			WHEN 2 THEN 'Equity'\n");
                varname1.Append("			WHEN 3 THEN 'Revenue' \n");
                varname1.Append("			WHEN 4 THEN 'Cost of Sales' \n");
                varname1.Append("			WHEN 5 THEN 'Expense' \n");
                varname1.Append("			WHEN 6 THEN 'Bank'\n");
                varname1.Append("			WHEN 7 THEN 'Non-Posting'\n");
                varname1.Append("			WHEN 8 THEN 'Other Income (Expense)'\n");
                varname1.Append("			WHEN 9 THEN 'Provisions for Income Taxes'\n");
                varname1.Append("		END)\n");
                varname1.Append("	ELSE c.Sub END AS Sub,\n");
                varname1.Append("	'' As Url \n");
                varname1.Append("FROM Chart c \n");
                varname1.Append("	LEFT JOIN Trans t ON c.ID = t.Acct AND t.fDate >= '" + _objChart.StartDate.Date + "' AND t.fDate <= '" + _objChart.EndDate.Date + "' \n");
                varname1.Append("WHERE c.Type IN (3, 4, 5, 8, 9) AND (c.Status = 0 OR t.Amount IS NOT NULL)\n");
                varname1.Append("GROUP BY c.ID, c.Acct, c.fDesc, c.Type, c.Sub\n");
                varname1.Append("ORDER BY c.Acct\n");

                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetIncomeStatementSummary(Chart _objChart)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT \n");
                varname1.Append("	SUM(CASE c.Type \n");
                varname1.Append("		WHEN 3 THEN (ISNULL(t.Amount, 0) * -1) \n");
                varname1.Append("		WHEN 8 THEN (ISNULL(t.Amount, 0) * -1) \n");
                varname1.Append("		ELSE ISNULL(t.Amount, 0) \n");
                varname1.Append("	END) AS Amount,\n");
                varname1.Append("  c.Type AS Type, \n");
                varname1.Append("  (CASE c.Type \n");
                varname1.Append("	WHEN 0 THEN 'Asset' \n");
                varname1.Append("	WHEN 1 THEN 'Liability' \n");
                varname1.Append("	WHEN 2 THEN 'Equity' \n");
                varname1.Append("	WHEN 3 THEN 'Revenue' \n");
                varname1.Append("	WHEN 4 THEN 'Cost of Sales' \n");
                varname1.Append("	WHEN 5 THEN 'Expense' \n");
                varname1.Append("	WHEN 6 THEN 'Bank' \n");
                varname1.Append("  END) AS TypeName, \n");
                varname1.Append("  (CASE c.Sub WHEN '' THEN (\n");
                varname1.Append("      CASE c.Type \n");
                varname1.Append("		WHEN 0 THEN 'Asset' \n");
                varname1.Append("		WHEN 1 THEN 'Liability' \n");
                varname1.Append("		WHEN 2 THEN 'Equity' \n");
                varname1.Append("		WHEN 3 THEN 'Revenue' \n");
                varname1.Append("		WHEN 4 THEN 'Cost of Sales' \n");
                varname1.Append("		WHEN 5 THEN 'Expense' \n");
                varname1.Append("		WHEN 6 THEN 'Bank' \n");
                varname1.Append("		WHEN 7 THEN 'Non-Posting' \n");
                varname1.Append("		WHEN 8 THEN 'Other Income (Expense)' \n");
                varname1.Append("		WHEN 9 THEN 'Provisions for Income Taxes' \n");
                varname1.Append("    END) ELSE c.Sub END \n");
                varname1.Append("  ) As Sub \n");
                varname1.Append("FROM \n");
                varname1.Append("  Trans as t \n");
                varname1.Append("  INNER JOIN Chart as c ON c.ID = t.Acct \n");
                varname1.Append("WHERE \n");
                varname1.Append("  c.Type IN (3, 4, 5, 8, 9) \n");
                varname1.Append("  AND t.Amount <> 0 \n");
                varname1.Append("  AND t.fDate >= '" + _objChart.StartDate.Date + "' \n");
                varname1.Append("  AND t.fDate <= '" + _objChart.EndDate.Date + "' \n");
                varname1.Append("GROUP BY c.Type, c.Sub\n");

                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetIncomeStatementTotal(Chart _objChart)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();

                varname1.Append("SELECT             ");
                varname1.Append("	SUM(CASE WHEN c.Type = 3 THEN t.Amount * -1 ELSE 0 END) AS TotalRevenue, ");
                varname1.Append("	SUM(CASE WHEN c.Type = 4 THEN t.Amount ELSE 0 END) AS TotalCost,");
                varname1.Append("	SUM(CASE WHEN c.Type = 5 THEN t.Amount ELSE 0 END) AS TotalExpense           ");
                varname1.Append("FROM Trans as t ");
                varname1.Append("	INNER JOIN Chart as c ON c.ID = t.Acct            ");
                varname1.Append("WHERE c.Type IN (3, 4, 5)   ");
                varname1.Append("	AND t.Amount <> 0       ");
                varname1.Append("   AND t.fDate >= '" + _objChart.StartDate.Date + "' AND t.fDate <= '" + _objChart.EndDate.Date + "'  \n");

                varname1.Append("SELECT             ");
                varname1.Append("	SUM(CASE WHEN c.Type = 3 THEN t.Amount * -1 ELSE 0 END) AS TotalRevenue, ");
                varname1.Append("	SUM(CASE WHEN c.Type = 4 THEN t.Amount ELSE 0 END) AS TotalCost,");
                varname1.Append("	SUM(CASE WHEN c.Type = 5 THEN t.Amount ELSE 0 END) AS TotalExpense           ");
                varname1.Append("FROM Trans as t ");
                varname1.Append("	INNER JOIN Chart as c ON c.ID = t.Acct            ");
                varname1.Append("WHERE c.Type IN (3, 4, 5)   ");
                varname1.Append("	AND t.Amount <> 0       ");
                varname1.Append("   AND t.fDate <= '" + _objChart.EndDate.Date + "'  \n");

                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetIncomeStatementDetailsWithCenters(Chart _objChart)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();

                varname1.Append("SELECT             \n");
                varname1.Append("	 c.Acct AS AcctNo, \n");
                varname1.Append("	 c.fDesc AS fDesc, \n");
                varname1.Append("    t.Acct,                        \n");
                varname1.Append("	 c.Department AS Department, \n");
                varname1.Append("	 D.CentralName AS DepartmentName, \n");
                varname1.Append("   CASE c.Type                         \n");
                varname1.Append("       WHEN 3 THEN (t.Amount * -1)       \n");
                varname1.Append("       WHEN 8 THEN (t.Amount * -1)       \n");
                varname1.Append("       ELSE t.Amount END As Amount,    \n");
                varname1.Append("   c.Type AS Type,                     \n");
                varname1.Append("   (CASE c.Type WHEN 0 THEN 'Asset'    \n");
                varname1.Append("         WHEN 1 THEN 'Liability'            \n");
                varname1.Append("         WHEN 2 THEN 'Equity'               \n");
                varname1.Append("         WHEN 3 THEN 'Revenue'              \n");
                varname1.Append("         WHEN 4 THEN 'Cost of Sales'        \n");
                varname1.Append("         WHEN 5 THEN 'Expense'              \n");
                varname1.Append("         WHEN 6 THEN 'Bank'                 \n");
                varname1.Append("         WHEN 7 THEN 'Non-Posting' \n");
                varname1.Append("         WHEN 8 THEN 'Other Income (Expense)' \n");
                varname1.Append("         WHEN 9 THEN 'Provisions for Income Taxes' \n");
                varname1.Append("    END) AS TypeName,                  \n");
                varname1.Append("     (CASE c.Sub WHEN '' THEN          \n");
                varname1.Append("     (CASE c.Type WHEN 0 THEN 'Asset'        \n");
                varname1.Append("         WHEN 1 THEN 'Liability'       \n");
                varname1.Append("         WHEN 2 THEN 'Equity'          \n");
                varname1.Append("         WHEN 3 THEN 'Revenue'         \n");
                varname1.Append("         WHEN 4 THEN 'Cost of Sales'   \n");
                varname1.Append("         WHEN 5 THEN 'Expense'         \n");
                varname1.Append("         WHEN 6 THEN 'Bank'            \n");
                varname1.Append("         WHEN 7 THEN 'Non-Posting' \n");
                varname1.Append("         WHEN 8 THEN 'Other Income (Expense)' \n");
                varname1.Append("         WHEN 9 THEN 'Provisions for Income Taxes' \n");
                varname1.Append("   END)            \n");
                varname1.Append("   ELSE c.Sub END) As Sub,      \n");
                varname1.Append("   '' As Url               \n");
                varname1.Append("    FROM Trans as t INNER JOIN Chart as c    \n");
                varname1.Append("    ON c.ID = t.Acct            \n");
                varname1.Append(" INNER JOIN Central D ON c.Department = D.ID \n");
                varname1.Append("    WHERE c.Type IN (3, 4, 5, 8, 9)   \n");
                varname1.Append("    AND c.Department IN (" + _objChart.Departments + ")   \n");
                varname1.Append("    AND t.Amount <> 0       \n");
                varname1.Append("    AND t.fDate >= '" + _objChart.StartDate.Date + "' AND t.fDate <= '" + _objChart.EndDate.Date + "'  \n");
                varname1.Append("    ORDER BY c.Acct        \n");

                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetIncomeStatementDetailsWithCentersAndBudgets(Chart _objChart)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT                                 \n");
                varname1.Append("   tbCen.* ,                           \n");
                varname1.Append("   SUM(ISNULL((actD.Amount), 0)) AS Budget,    \n");
                varname1.Append("   (CASE tbCen.Type WHEN 0 THEN 'Asset'        \n");
                varname1.Append("    WHEN 1 THEN 'Liability'            \n");
                varname1.Append("    WHEN 2 THEN 'Equity'               \n");
                varname1.Append("    WHEN 3 THEN 'Revenue'              \n");
                varname1.Append("    WHEN 4 THEN 'Cost of Sales'        \n");
                varname1.Append("    WHEN 5 THEN 'Expense'              \n");
                varname1.Append("    WHEN 6 THEN 'Bank'                 \n");
                varname1.Append("    WHEN 7 THEN 'Non-Posting' \n");
                varname1.Append("    WHEN 8 THEN 'Other Income (Expense)' \n");
                varname1.Append("    WHEN 9 THEN 'Provisions for Income Taxes' \n");
                varname1.Append("    END) AS TypeName,                  \n");
                varname1.Append("    '' AS Url                          \n");
                varname1.Append("FROM                                   \n");
                varname1.Append("   (SELECT                             \n");
                varname1.Append("   c.Acct AS AcctNo,                   \n");
                varname1.Append("   c.fDesc AS fDesc,                   \n");
                varname1.Append("   t.Acct,                             \n");
                varname1.Append("   c.Department AS Department,         \n");
                varname1.Append("	dep.CentralName AS DepartmentName,  \n");
                varname1.Append("   (CASE c.Sub WHEN '' THEN            \n");
                varname1.Append("   (CASE c.Type WHEN 0 THEN 'Asset'    \n");
                varname1.Append("       WHEN 1 THEN 'Liability'         \n");
                varname1.Append("       WHEN 2 THEN 'Equity'            \n");
                varname1.Append("       WHEN 3 THEN 'Revenue'           \n");
                varname1.Append("       WHEN 4 THEN 'Cost of Sales'     \n");
                varname1.Append("       WHEN 5 THEN 'Expense'           \n");
                varname1.Append("       WHEN 6 THEN 'Bank'              \n");
                varname1.Append("       WHEN 7 THEN 'Non-Posting' \n");
                varname1.Append("       WHEN 8 THEN 'Other Income (Expense)' \n");
                varname1.Append("       WHEN 9 THEN 'Provisions for Income Taxes' \n");
                varname1.Append("   END)                                \n");
                varname1.Append("   ELSE c.Sub END) As Sub,             \n");
                varname1.Append("   SUM(CASE c.Type                     \n");
                varname1.Append("       WHEN 3 THEN (t.Amount * -1)     \n");
                varname1.Append("       WHEN 8 THEN (t.Amount * -1)     \n");
                varname1.Append("       ELSE t.Amount END) As Amount,   \n");
                varname1.Append("   c.Type AS Type                      \n");
                varname1.Append("FROM Trans as t                        \n");
                varname1.Append("   INNER JOIN Chart as c ON c.ID = t.Acct          \n");
                varname1.Append("   INNER JOIN Central dep ON c.Department = dep.ID \n");
                varname1.Append("   WHERE c.Type IN (3, 4, 5, 8, 9)   \n");
                varname1.Append("   AND c.Department IN (" + _objChart.Departments + ")   \n");
                varname1.Append("   AND t.Amount <> 0       \n");
                varname1.Append("   AND t.fDate >= '" + _objChart.StartDate.Date + "' AND t.fDate <= '" + _objChart.EndDate.Date + "'  \n");
                varname1.Append("   GROUP BY c.Acct, c.fDesc, t.Acct, c.Department, dep.CentralName, c.Sub, c.Type \n");
                varname1.Append("   ) AS tbCen                          \n");
                varname1.Append("   LEFT JOIN Account act ON tbCen.AcctNo = act.Acct                \n");
                varname1.Append("   LEFT JOIN AccountDetails actD ON act.AccountID = actD.AccountID AND actD.Period IN (" + _objChart.Periods + ") \n");
                varname1.Append("   LEFT JOIN Budget bud ON bud.BudgetID = actD.BudgetID            \n");

                if (!string.IsNullOrEmpty(_objChart.Budgets))
                {
                    varname1.Append("   AND bud.BudgetID IN (" + _objChart.Budgets + ")             \n");
                }

                varname1.Append("   GROUP BY tbCen.AcctNo, tbCen.fDesc, tbCen.Acct, tbCen.Department, tbCen.DepartmentName,tbCen.Amount, tbCen.Sub, tbCen.Type \n");
                varname1.Append("   ORDER BY tbCen.AcctNo               \n");

                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTrialBalanceDetails(Chart _objChart, bool isCloseOut)
        {
            try
            {
                //StringBuilder varname1 = new StringBuilder();
                //varname1.Append(" ( SELECT  \n");
                //varname1.Append("	 c.Acct +'  '+c.fDesc AS fDesc, \n");
                //varname1.Append("    c.id As Acct, \n");
                //varname1.Append("    sum(isnull(t.Amount,0)) As Amount,  \n");
                //varname1.Append("    c.Type AS Type,");
                //varname1.Append("   (CASE c.Type WHEN 0 THEN 'Asset' ");
                //varname1.Append("    WHEN 1 THEN 'Liability' ");
                //varname1.Append("    WHEN 2 THEN 'Equity' ");
                //varname1.Append("    WHEN 3 THEN 'Revenue' ");
                //varname1.Append("    WHEN 4 THEN 'Cost of Sales' \n");
                //varname1.Append("    WHEN 5 THEN 'Expense'  ");
                //varname1.Append("    WHEN 6 THEN 'Bank' \n");
                //varname1.Append("    END) AS TypeName, ");
                //varname1.Append("     (CASE c.Sub WHEN '' THEN  ");
                //varname1.Append("     (CASE c.Type WHEN 0 THEN 'Asset'  ");
                //varname1.Append("         WHEN 1 THEN 'Liability' ");
                //varname1.Append("         WHEN 2 THEN 'Equity' ");
                //varname1.Append("         WHEN 3 THEN 'Revenue' ");
                //varname1.Append("         WHEN 4 THEN 'Cost of Sales' \n");
                //varname1.Append("         WHEN 5 THEN 'Expense' ");
                //varname1.Append("         WHEN 6 THEN 'Bank' \n");
                //varname1.Append("   END)");
                //varname1.Append("   ELSE c.Sub END) As Sub, \n");
                //varname1.Append("   '' As Url               \n");
                //varname1.Append("    FROM Chart c inner join Trans t on c.id = t.Acct ");
                //varname1.Append("    WHERE (DefaultNo <> 'D3920' OR DefaultNo IS NULL) \n");
                ////Trail Balance Changes By Prateek
                //varname1.Append("    AND c.Type in (3,4,5,9,8) ");
                //varname1.Append("    AND (t.fDate >= '" + _objChart.StartDate.Date + "' AND t.fDate <= '" + _objChart.EndDate.Date + "' ) \n");
                //varname1.Append("    AND t.Amount <> 0.00 ");
                //varname1.Append("    GROUP BY c.fdesc, c.Acct, c.id ,c.type, c.Sub HAVING sum(isnull(t.Amount,0)) <> 0 \n");
                //varname1.Append("   ) \n");
                //varname1.Append(" UNION ALL ");
                //varname1.Append(" SELECT c.Acct +'  '+c.fDesc AS fDesc,  c.id As Acct, sum(isnull(t.Amount,0)) As Amount,c.Type as Type ,'' As typename ,'' As sub, '' As url  FROM Chart c inner join Trans t on c.id = t.Acct WHERE  t.Amount <> 0.00  AND  t.fDate <='" + _objChart.EndDate.Date + "'");
                //varname1.Append("    and c.type in (0,1,2,6) AND (DefaultNo <> 'D3920' OR DefaultNo IS NULL) group by c.fdesc, c.Acct, c.id ,c.type HAVING sum(isnull(t.Amount,0)) <> 0  \n ");
                //if (isCloseOut)
                //{
                //    varname1.Append(" SELECT c.Acct +'  '+c.fDesc AS fDesc,id As Acct, Balance= (Select ISNULL(SUM(Amount),0) + (SELECT Balance= (Select ISNULL(SUM(Amount),0) FROM Trans AS t WHERE c.ID = t.Acct AND t.fDate >= '" + _objChart.StartDate.Date + "' AND t.fDate <= '" + _objChart.EndDate.Date + "') from chart c where DefaultNo='D3130') FROM Trans AS t WHERE c.ID = t.Acct AND t.fDate <= '" + _objChart.EndDate.Date + "')  , type, '' As typename ,'' As sub, '' As url  from chart c where DefaultNo='D3920' \n ");
                //}
                //else
                //{
                //    varname1.Append(" SELECT c.Acct +'  '+c.fDesc AS fDesc, id As Acct, Balance= (Select ISNULL(SUM(Amount),0) FROM Trans AS t WHERE c.ID = t.Acct AND t.fDate <= '" + _objChart.EndDate.Date + "'), type, '' As typename ,'' As sub, '' As url  from chart c where DefaultNo='D3920' \n ");
                //}
                //varname1.Append(" SELECT t.Type, t.TypeName, sum(t.NAmt) as NAmt from     \n");
                //varname1.Append("        (select t.ID, c.Type, case c.type when 3 then 'Revenue' when 4 then 'Cost of sales' when 5 then 'Expenses' end as typename,     \n");
                //varname1.Append("        case c.type when 3 then (isnull(amount,0) * -1) else isnull(amount,0) end as NAmt       \n");
                //varname1.Append("        from trans t inner join chart c on c.ID = t.Acct                                        \n");
                //varname1.Append("            where c.type in (3,4,5) and t.fdate <= '" + _objChart.EndDate + "') as t   \n");
                //varname1.Append("            group by t.Type, t.TypeName     \n");
                var para = new SqlParameter[3];

                para[0] = new SqlParameter
                {
                    ParameterName = "@isCloseOut",
                    SqlDbType = SqlDbType.Bit,
                    Value = isCloseOut
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@StartDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objChart.StartDate.Date
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@EndDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _objChart.EndDate.Date
                };
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.StoredProcedure, "SpGetTrialBalanceDetails", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTrialBalanceActivity(Chart _objChart, bool isCloseOut)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT \n");
                sb.Append("	c.ID AS Acct, \n");
                sb.Append("	c.Acct +'  '+ c.fDesc AS fDesc, \n \n");
                sb.Append("	c.Type, \n");
                sb.Append("	CASE c.Type  \n");
                sb.Append("		WHEN 0 THEN 'Asset'  \n");
                sb.Append("		WHEN 1 THEN 'Liability'  \n");
                sb.Append("		WHEN 2 THEN 'Equity'  \n");
                sb.Append("		WHEN 3 THEN 'Revenue'  \n");
                sb.Append("		WHEN 4 THEN 'Cost of Sales'  \n");
                sb.Append("		WHEN 5 THEN 'Expense'   \n");
                sb.Append("		WHEN 6 THEN 'Bank'  \n");
                sb.Append("    END AS TypeName,  \n");
                sb.Append("    CASE c.Sub  \n");
                sb.Append("		WHEN '' THEN   \n");
                sb.Append("        (CASE c.Type WHEN 0 THEN 'Asset'   \n");
                sb.Append("            WHEN 1 THEN 'Liability'  \n");
                sb.Append("            WHEN 2 THEN 'Equity'  \n");
                sb.Append("            WHEN 3 THEN 'Revenue'  \n");
                sb.Append("            WHEN 4 THEN 'Cost of Sales'  \n");
                sb.Append("            WHEN 5 THEN 'Expense'  \n");
                sb.Append("            WHEN 6 THEN 'Bank'  \n");
                sb.Append("		END) \n");
                sb.Append("    ELSE c.Sub END As Sub,  \n");
                sb.Append("	op.Opening, \n");
                sb.Append("	ac.Debit, \n");
                sb.Append("	ac.Credit, \n");
                sb.Append("	ac.NetActivity, \n");
                sb.Append("	(ISNULL(ac.NetActivity, 0) + ISNULL(op.Opening, 0)) AS Ending, \n");
                sb.Append("	'' As Url  \n");
                sb.Append("FROM Chart c \n");
                sb.Append("LEFT JOIN \n");
                sb.Append("	(SELECT   \n");
                sb.Append("		c.ID AS Acct,  \n");
                sb.Append("		SUM(ISNULL(t.Amount,0)) AS Opening 	 \n");
                sb.Append("	FROM Chart c  \n");
                sb.Append("		INNER JOIN Trans t ON c.id = t.Acct  \n");
                sb.Append("	WHERE (DefaultNo <> 'D3920' OR DefaultNo IS NULL)  \n");
                sb.Append("		AND c.Type IN (3,4,5)  \n");
                sb.Append("		AND (t.fDate >= '" + _objChart.YearStartDate + "' AND t.fDate < '" + _objChart.StartDate + "' )  \n");
                sb.Append("		AND t.Amount <> 0.00  \n");
                sb.Append("	GROUP BY c.ID \n");
                sb.Append("	HAVING SUM(ISNULL(t.Amount,0)) <> 0  \n");
                sb.Append("	UNION ALL \n");
                sb.Append("	SELECT  \n");
                sb.Append("		c.ID AS Acct,  \n");
                sb.Append("		SUM(ISNULL(t.Amount,0)) AS Opening  \n");
                sb.Append("	FROM Chart c  \n");
                sb.Append("		INNER JOIN Trans t ON c.id = t.Acct  \n");
                sb.Append("	WHERE  t.Amount <> 0.00  AND  t.fDate <'" + _objChart.StartDate + "' \n");
                sb.Append("		AND c.type IN (0,1,2,6)  \n");
                sb.Append("	GROUP BY c.ID \n");
                sb.Append("	HAVING SUM(ISNULL(t.Amount,0)) <> 0 \n");
                sb.Append("	) AS op ON op.Acct = c.ID  \n");

                sb.Append("LEFT JOIN \n");
                sb.Append("	(SELECT   \n");
                sb.Append("		c.ID AS Acct,  \n");
                sb.Append("		SUM(CASE WHEN ISNULL(t.Amount,0) > 0 THEN ISNULL(t.Amount,0) END) AS Debit, \n");
                sb.Append("		SUM(CASE WHEN ISNULL(t.Amount,0) < 0 THEN ISNULL(t.Amount,0) * -1 END) AS Credit, \n");
                sb.Append("		SUM(ISNULL(t.Amount,0)) AS NetActivity \n");
                sb.Append("	FROM Chart c  \n");
                sb.Append("		INNER JOIN Trans t ON c.ID = t.Acct  \n");
                sb.Append("	WHERE c.Type IN (0,1,2,3,4,5,6)  \n");
                sb.Append("		AND (t.fDate >= '" + _objChart.StartDate + "' AND t.fDate <= '" + _objChart.EndDate + "' )  \n");
                sb.Append("		AND t.Amount <> 0.00  \n");
                sb.Append("	GROUP BY c.ID) AS ac ON ac.Acct = c.ID \n");
                sb.Append("WHERE op.Opening <> 0 OR ac.Debit <> 0 OR ac.Credit <> 0 \n");

                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDepartmentByCompanyID(User objPropUser)
        {
            try
            {
                return objPropUser.Ds = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "SELECT CostCenter FROM Branch WHERE ID = (SELECT CompanyID from tblUserCo where ID ='" + objPropUser.EN + "')");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDepartmentByDefaultCompanyID(User objPropUser)
        {
            try
            {
                return objPropUser.Ds = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "SELECT CostCenter FROM Branch WHERE ID = '" + objPropUser.EN + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPeriodClosedYear(User objPropUser)
        {
            try
            {
                return objPropUser.Ds = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "SELECT fUser, fStart, fEnd FROM tblUser WHERE fUser ='" + objPropUser.Username + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCenterNames(User objPropUser)
        {
            try
            {
                return objPropUser.Ds = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "SELECT ID, CentralName FROM Central ORDER BY CentralName");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCenters(User objPropUser)
        {
            try
            {
                return objPropUser.Ds = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "SELECT * FROM Central ORDER BY SortOrder");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetIncomestatementBalance(Chart objChart)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    select t.Type, t.TypeName, sum(t.NAmt) as NAmt from     \n");
                varname.Append("        (select t.ID, c.Type, case c.type when 3 then 'Revenue' when 4 then 'Cost of sales' when 5 then 'Expenses' end as typename,     \n");
                varname.Append("        case c.type when 3 then (isnull(amount,0) * -1) else isnull(amount,0) end as NAmt       \n");
                varname.Append("        from trans t inner join chart c on c.ID = t.Acct                                        \n");
                varname.Append("            where c.type in (3,4,5) and t.fdate <= '" + objChart.EndDate + "') as t   \n");
                varname.Append("            group by t.Type, t.TypeName     \n");
                return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Rahil's Implement

        public DataSet GetPurchaseJournal(OpenAP _objOpenAP)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT      p.ID,                   \n");
                varname1.Append("	 p.fDate as Post, \n");
                varname1.Append("    o.Due,p.Ref,p.fDesc,p.Vendor,                 \n");
                varname1.Append("  r.Name AS VendorName,  \n");
                varname1.Append("      DATEPART(wk, o.Due) as WeekCount,        \n");
                varname1.Append("      DATEADD(dd, -1, DATEADD(wk,DATEDIFF(wk,0,o.Due),0)) as WeekDate,        \n");
                //varname1.Append("     isnull(p.Amount,0) as Amount, \n");
                varname1.Append("     isnull(o.Balance,0) as Amount, \n");
                varname1.Append("   p.Status,    \n");
                varname1.Append("  (CASE p.Status WHEN 0 THEN 'Open'        \n");
                varname1.Append("   WHEN 1 THEN 'Closed'           \n");
                varname1.Append("    WHEN 2 THEN 'Void'  END) AS StatusName       \n");
                varname1.Append("   FROM PJ AS p            \n");
                varname1.Append("   inner join Vendor AS v on p.Vendor = v.ID             \n");
                varname1.Append("    inner join Rol AS r on v.Rol = r.ID        \n");
                varname1.Append("   left join openAP AS o on p.ID = o.PJID        \n");
                varname1.Append("    WHERE  o.Balance<>0      \n");
                //varname1.Append("       AND o.Original<>o.Selected    \n");
                varname1.Append("    AND o.Due <= '" + _objOpenAP.Due.Date + "'    \n");
                varname1.Append("    ORDER BY o.Due \n");
                return _objOpenAP.Ds = SqlHelper.ExecuteDataset(_objOpenAP.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPurchaseJournal(GetPurchaseJournalParam _GetPurchaseJournalParam, string ConnectionString)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT      p.ID,                   \n");
                varname1.Append("	 p.fDate as Post, \n");
                varname1.Append("    o.Due,p.Ref,p.fDesc,p.Vendor,                 \n");
                varname1.Append("  r.Name AS VendorName,  \n");
                varname1.Append("      DATEPART(wk, o.Due) as WeekCount,        \n");
                varname1.Append("      DATEADD(dd, -1, DATEADD(wk,DATEDIFF(wk,0,o.Due),0)) as WeekDate,        \n");
                //varname1.Append("     isnull(p.Amount,0) as Amount, \n");
                varname1.Append("     isnull(o.Balance,0) as Amount, \n");
                varname1.Append("   p.Status,    \n");
                varname1.Append("  (CASE p.Status WHEN 0 THEN 'Open'        \n");
                varname1.Append("   WHEN 1 THEN 'Closed'           \n");
                varname1.Append("    WHEN 2 THEN 'Void'  END) AS StatusName       \n");
                varname1.Append("   FROM PJ AS p            \n");
                varname1.Append("   inner join Vendor AS v on p.Vendor = v.ID             \n");
                varname1.Append("    inner join Rol AS r on v.Rol = r.ID        \n");
                varname1.Append("   left join openAP AS o on p.ID = o.PJID        \n");
                varname1.Append("    WHERE  o.Balance<>0      \n");
                //varname1.Append("       AND o.Original<>o.Selected    \n");
                varname1.Append("    AND o.Due <= '" + _GetPurchaseJournalParam.Due.Date + "'    \n");
                varname1.Append("    ORDER BY o.Due \n");
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustomerLabel(Customer objCust, Int32 IsSelesAsigned = 0)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("Select r.Name,r.Address,r.City,r.State,r.Zip  \n");
                varname1.Append(" from rol r inner join owner o on r.ID = o.Rol \n");

                varname1.Append(" where 1 = 1 \n");

                if (IsSelesAsigned > 0)
                {
                    varname1.Append(" and o.id in ( select l.Owner from loc l where l.Terr=(select isnull(id,0) FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSelesAsigned + ")) ) \n");
                }
                if (objCust.Name != null)
                {
                    varname1.Append(" and r.Name Like '%" + objCust.Name + "%' \n");
                }
                if (objCust.Address != null)
                {
                    varname1.Append(" and r.Address Like '%" + objCust.Address + "%' \n");
                }
                if (objCust.City != null)
                {
                    varname1.Append(" and r.City Like '%" + objCust.City + "%' \n");
                }
                if (objCust.State != null)
                {
                    varname1.Append(" and r.State Like '%" + objCust.State + "%' \n");
                }
                if (objCust.Zip != null)
                {
                    varname1.Append(" and r.Zip Like '%" + objCust.Zip + "%' \n");
                }
                //string test = varname1.ToString();
                return objCust.DsCustomer = SqlHelper.ExecuteDataset(objCust.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Customer Ticket Category by Last Service DL Function
        public DataSet GetCustomerTikcketCategoryByLastService(Customer objCust, string[] category)
        {
            try
            {

                string.Join(",", category);
                string CStype = string.Join(",", category);
                var para = new SqlParameter[1];

                para[0] = new SqlParameter
                {
                    ParameterName = "@Category",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = CStype
                };
                return objCust.DsCustomer = SqlHelper.ExecuteDataset(objCust.ConnConfig, CommandType.StoredProcedure, "spGetCustomerTicketByCategory", para);

                //return objCust.DsCustomer = SqlHelper.ExecuteDataset(objCust.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Payroll-Federal 940 Form Report
        public DataSet GetPayrollFederal940FormReport(Contracts objPropContracts)
        {
            try
            {
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "@StartDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPropContracts.StartDate
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@EndDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPropContracts.EndDate
                };
                return objPropContracts.Ds = SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetPayrollFederal940Form", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Payroll - Deduction Summary by Other Deduction report
        public DataSet GetPayrollDeductionSummarybyOtherDeductionReport(Contracts objPropContracts)
        {
            try
            {
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "@StartDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPropContracts.StartDate
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@EndDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPropContracts.EndDate
                };
                return objPropContracts.Ds = SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetPayrollDeductionSummarybyOtherDeduction", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Payroll Register GL Cross-Reference report
        public DataSet GetPayrollRegisterGLCrossReferenceReport(Contracts objPropContracts)
        {
            try
            {
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "@StartDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPropContracts.StartDate
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@EndDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPropContracts.EndDate
                };
                return objPropContracts.Ds = SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetPayrollGLCrossReference", para);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get BANK RECONCILIATION ITEMS CLEARED  REPORT
        public DataSet GetBankReconciliationItemsCleared(Customer objCust)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("Select   \n");
                varname1.Append("Bank.NAcct as BankAccount, \n");
                varname1.Append("Rol.Name as BankName, \n");
                varname1.Append("BankRecon.StatementDate as StatementDate, \n");
                varname1.Append("BankRecon.Begningbalance, \n");
                varname1.Append("Bank.Balance - Bank.Recon as BankBalance, \n");
                varname1.Append("BankRecon.Endbalance as ProofBalance, \n");
                varname1.Append("BankRecon.Endbalance as StatementBalance \n");
                varname1.Append("From BankRecon \n");
                varname1.Append("INNER Join Bank ON BankRecon.Bank= Bank.ID \n");
                varname1.Append("INNER Join Rol ON Bank.Rol= Rol.ID \n");
                varname1.Append("Where BankRecon.ID=" + objCust.CustomerID);

                return objCust.DsCustomer = SqlHelper.ExecuteDataset(objCust.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get BANK RECONCILIATION ITEMS CLEARED  REPORT List
        public DataSet GetBankReconciliationItemsClearedList(Customer objCust)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("Select   \n");
                varname1.Append("BankReconI.fDate as Date, \n");
                varname1.Append("case when BankReconI.Amount >0 then 'Deposit/Debit' else 'Check/Credit' End as Type, \n");
                varname1.Append("BankReconI.Ref as Ref, \n");
                varname1.Append("Trans.fDesc as Description, \n");
                varname1.Append("case when BankReconI.Amount > 0 then BankReconI.Amount else 0.00 end as Debits, \n");
                varname1.Append("case when BankReconI.Amount < 0 then BankReconI.Amount*-1 else 0.00 end as Credits  From BankReconI \n");
                varname1.Append("INNER Join Trans ON BankReconI.TRID= Trans.ID \n");
                varname1.Append("INNER Join BankRecon ON BankReconI.ReconID=BankRecon.ID \n");
                varname1.Append("Where BankReconI.fDate <= CAST('" + objCust.StartDate + "'  as date) \n");
                varname1.Append("AND BankRecon.Bank=" + objCust.ItemID);

                return objCust.DsCustomer = SqlHelper.ExecuteDataset(objCust.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetCustomerLabel(GetCustomerLabelParam _GetCustomerLabel, string ConnectionString, Int32 IsSalesAsigned = 0)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("Select r.Name,r.Address,r.City,r.State,r.Zip  \n");
                varname1.Append(" from rol r inner join owner o on r.ID = o.Rol \n");

                varname1.Append(" where 1 = 1 \n");

                if (IsSalesAsigned > 0)
                {
                    varname1.Append(" and o.id in ( select l.Owner from loc l where l.Terr=(select isnull(id,0) FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSalesAsigned + ")) ) \n");
                }
                if (_GetCustomerLabel.Name != null)
                {
                    varname1.Append(" and r.Name Like '%" + _GetCustomerLabel.Name + "%' \n");
                }
                if (_GetCustomerLabel.Address != null)
                {
                    varname1.Append(" and r.Address Like '%" + _GetCustomerLabel.Address + "%' \n");
                }
                if (_GetCustomerLabel.City != null)
                {
                    varname1.Append(" and r.City Like '%" + _GetCustomerLabel.City + "%' \n");
                }
                if (_GetCustomerLabel.State != null)
                {
                    varname1.Append(" and r.State Like '%" + _GetCustomerLabel.State + "%' \n");
                }
                if (_GetCustomerLabel.Zip != null)
                {
                    varname1.Append(" and r.Zip Like '%" + _GetCustomerLabel.Zip + "%' \n");
                }
                return _GetCustomerLabel.DsCustomer = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAccountLabel(Loc objLoc, Int32 IsSelesAsigned = 0)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("Select l.Tag,r.Address,r.City,r.State,r.Zip  \n");
                varname1.Append(" from rol r inner join Loc l on r.ID = l.Rol \n");

                varname1.Append(" where 1 = 1 \n");

                if (IsSelesAsigned > 0)
                {
                    varname1.Append(" and  l.Terr=(select isnull(id,0) FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSelesAsigned + ")) \n");
                }
                if (objLoc.Tag != null)
                {
                    varname1.Append(" and l.Tag Like '%" + objLoc.Tag + "%' \n");
                }
                if (objLoc.Address != null)
                {
                    varname1.Append(" and r.Address Like '%" + objLoc.Address + "%' \n");
                }
                if (objLoc.City != null)
                {
                    varname1.Append(" and r.City Like '%" + objLoc.City + "%' \n");
                }
                if (objLoc.State != null)
                {
                    varname1.Append(" and r.State Like '%" + objLoc.State + "%' \n");
                }
                if (objLoc.Type != null)
                {
                    varname1.Append(" and r.Zip Like '%" + objLoc.Type + "%' \n");
                }
                return objLoc.DsLoc = SqlHelper.ExecuteDataset(objLoc.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetAccountLabel(GetAccountLabelParam _GetAccountLabel, string ConnectionString, Int32 IsSelesAsigned = 0)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("Select l.Tag,r.Address,r.City,r.State,r.Zip  \n");
                varname1.Append(" from rol r inner join Loc l on r.ID = l.Rol \n");

                varname1.Append(" where 1 = 1 \n");

                if (IsSelesAsigned > 0)
                {
                    varname1.Append(" and  l.Terr=(select isnull(id,0) FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSelesAsigned + ")) \n");
                }
                if (_GetAccountLabel.Tag != null)
                {
                    varname1.Append(" and l.Tag Like '%" + _GetAccountLabel.Tag + "%' \n");
                }
                if (_GetAccountLabel.Address != null)
                {
                    varname1.Append(" and r.Address Like '%" + _GetAccountLabel.Address + "%' \n");
                }
                if (_GetAccountLabel.City != null)
                {
                    varname1.Append(" and r.City Like '%" + _GetAccountLabel.City + "%' \n");
                }
                if (_GetAccountLabel.State != null)
                {
                    varname1.Append(" and r.State Like '%" + _GetAccountLabel.State + "%' \n");
                }
                if (_GetAccountLabel.Type != null)
                {
                    varname1.Append(" and r.Zip Like '%" + _GetAccountLabel.Type + "%' \n");
                }
                return _GetAccountLabel.DsLoc = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetVendorLabel(Customer objCust)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("Select r.Name,r.Address,r.City,r.State,r.Zip  \n");
                varname1.Append(" from rol r inner join Vendor v on r.ID = v.Rol \n");
                if (objCust.Name != null)
                {
                    varname1.Append(" Where r.Name Like '%" + objCust.Name + "%' \n");
                }
                if (objCust.Address != null)
                {
                    varname1.Append(" Where r.Address Like '%" + objCust.Address + "%' \n");
                }
                if (objCust.City != null)
                {
                    varname1.Append(" Where r.City Like '%" + objCust.City + "%' \n");
                }
                if (objCust.State != null)
                {
                    varname1.Append(" Where r.State Like '%" + objCust.State + "%' \n");
                }
                if (objCust.Zip != null)
                {
                    varname1.Append(" Where r.Zip Like '%" + objCust.Zip + "%' \n");
                }
                return objCust.DsCustomer = SqlHelper.ExecuteDataset(objCust.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetVendorLabel(GetVendorLabelParam _GetVendorLabelParam, string ConnectionString)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("Select r.Name,r.Address,r.City,r.State,r.Zip  \n");
                varname1.Append(" from rol r inner join Vendor v on r.ID = v.Rol \n");
                if (_GetVendorLabelParam.Name != null)
                {
                    varname1.Append(" Where r.Name Like '%" + _GetVendorLabelParam.Name + "%' \n");
                }
                if (_GetVendorLabelParam.Address != null)
                {
                    varname1.Append(" Where r.Address Like '%" + _GetVendorLabelParam.Address + "%' \n");
                }
                if (_GetVendorLabelParam.City != null)
                {
                    varname1.Append(" Where r.City Like '%" + _GetVendorLabelParam.City + "%' \n");
                }
                if (_GetVendorLabelParam.State != null)
                {
                    varname1.Append(" Where r.State Like '%" + _GetVendorLabelParam.State + "%' \n");
                }
                if (_GetVendorLabelParam.Zip != null)
                {
                    varname1.Append(" Where r.Zip Like '%" + _GetVendorLabelParam.Zip + "%' \n");
                }
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetUserReport(Rol objRol, bool incInactive)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT \n");
                sb.Append("	e.ID,\n");
                sb.Append("	e.fFirst AS FirstName, \n");
                sb.Append("	e.Last AS LastName, \n");
                sb.Append("	r.Address, \n");
                sb.Append("	r.Phone, \n");
                sb.Append("	e.Pager AS TextMsg, \n");
                sb.Append(" e.DHired as StartDate,\n");
                sb.Append(" e.Status as Status,\n");
                sb.Append(" e.Title as Title,\n");
                sb.Append(" e.DFired as TerminationDate\n");
                sb.Append("FROM tblUser u \n");
                sb.Append("LEFT JOIN Emp e ON u.fUser = e.CallSign \n");
                sb.Append("INNER JOIN Rol r ON e.Rol = r.ID \n");
                sb.Append("WHERE e.Field = 1 \n");

                if (!incInactive)
                {
                    sb.Append("	AND e.Status = 0 \n");
                }

                return objRol.DsRol = SqlHelper.ExecuteDataset(objRol.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetUserPaymentReport(User objPropUser, List<RetainFilter> filters)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT \n");
                sb.Append("	e.ID,\n");
                sb.Append("	u.fUser,\n");
                sb.Append("	e.fFirst AS FirstName, \n");
                sb.Append("	e.Last AS LastName, \n");
                sb.Append("	r.Address, \n");
                sb.Append("	r.Phone, \n");
                sb.Append("	e.Pager AS TextMsg, \n");
                sb.Append("	pw.Wage, \n");
                sb.Append("	wg.fDesc AS WageName, \n");
                sb.Append("	pw.Reg, \n");
                sb.Append("	pw.OT, \n");
                sb.Append("	pw.DT, \n");
                sb.Append("	pw.TT, \n");
                sb.Append("	pw.NT, \n");
                sb.Append("	pw.CReg, \n");
                sb.Append("	pw.COT, \n");
                sb.Append("	pw.CDT, \n");
                sb.Append("	pw.CTT, \n");
                sb.Append("	pw.CNT \n");
                sb.Append("FROM tblUser u \n");
                sb.Append("LEFT JOIN Emp e ON u.fUser = e.CallSign \n");
                sb.Append("LEFT JOIN tblWork w on u.fUser = w.fDesc \n");
                sb.Append("INNER JOIN Rol r ON e.Rol = r.ID \n");
                sb.Append("LEFT JOIN PRWageItem pw ON pw.Emp = e.ID \n");
                sb.Append("LEFT JOIN PRWage wg ON wg.ID = pw.Wage \n");
                sb.Append("WHERE (e.Field = 0 OR e.Field = 1) \n");

                if (!objPropUser.inclInactive)
                {
                    sb.Append("	AND e.Status = 0 \n");
                }

                // Search value
                if (!string.IsNullOrEmpty(objPropUser.SearchBy) && !string.IsNullOrEmpty(objPropUser.SearchValue))
                {
                    if (objPropUser.SearchBy == "fUser")
                    {
                        sb.Append(" AND u.fUser LIKE '%" + objPropUser.SearchValue + "%' \n");
                    }
                    if (objPropUser.SearchBy == "fFirst")
                    {
                        sb.Append("	 AND e.fFirst LIKE '%" + objPropUser.SearchValue + "%' \n");
                    }
                    if (objPropUser.SearchBy == "e.Last")
                    {
                        sb.Append("	AND e.Last LIKE '%" + objPropUser.SearchValue + "%' \n");
                    }
                    if (objPropUser.SearchBy == "usertype")
                    {
                        sb.Append("	AND e.Field = " + objPropUser.SearchValue + " \n");
                    }
                    if (objPropUser.SearchBy == "u.Status")
                    {
                        sb.Append("	AND u.Status= " + objPropUser.SearchValue + " \n");
                    }
                    if (objPropUser.SearchBy == "w.super")
                    {
                        sb.Append("	AND w.Super LIKE '%" + objPropUser.SearchValue + "%' \n");
                    }
                }

                //Ticket filters
                foreach (var filter in filters)
                {
                    if (filter.FilterColumn == "fuser")
                    {
                        sb.Append(" AND u.fUser LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "ffirst")
                    {
                        sb.Append(" AND e.fFirst LIKE '%" + filter.FilterValue + "' \n");
                    }
                    if (filter.FilterColumn == "lLast")
                    {
                        sb.Append(" AND e.Last LIKE '%" + filter.FilterValue + "' \n");
                    }
                    if (filter.FilterColumn == "usertype")
                    {
                        sb.Append(" AND (CASE WHEN ISNULL(fWork, '') = '' THEN 'Office' ELSE 'Field' END) LIKE '%" + filter.FilterValue + "' \n");
                    }
                    if (filter.FilterColumn == "status")
                    {
                        sb.Append(" AND (CASE WHEN u.Status = 0 THEN 'Active' ELSE 'Inactive' END) LIKE '%" + filter.FilterValue + "' \n");
                    }
                    if (filter.FilterColumn == "super")
                    {
                        sb.Append(" AND w.Super LIKE '%" + filter.FilterValue + "' \n");
                    }
                }

                sb.Append("ORDER BY u.fUser \n");

                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPOWeekly(PO objPO)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT      ");
                varname.Append("            p.PO,                   \n");
                varname.Append("            p.fDate as Post,        \n");
                varname.Append("            p.Due,                  \n");
                varname.Append("            p.fDesc,                \n");
                varname.Append("            p.Vendor,               \n");
                varname.Append("            r.Name AS VendorName,   \n");
                varname.Append("            isnull(p.Amount,0) as Amount,   \n");
                varname.Append("            p.Status,               \n");
                varname.Append("            (CASE p.Status WHEN 0 THEN 'Open'        \n");
                varname.Append("                WHEN 1 THEN 'Closed'                 \n");
                varname.Append("                WHEN 2 THEN 'Void'  END) AS StatusName,   \n");
                varname.Append("            DATEPART(wk, p.Due) as WeekCount,        \n");
                varname.Append("            DATEADD(dd, -1, DATEADD(wk,DATEDIFF(wk,0,p.Due),0)) as WeekDate         \n");
                varname.Append("            FROM PO AS p        \n");
                varname.Append("                	INNER JOIN Vendor AS v on p.Vendor = v.ID             \n");
                varname.Append("                    INNER JOIN Rol AS r on v.Rol = r.ID                   \n");
                varname.Append("                WHERE  p.Status = 0     \n");
                varname.Append("                   AND p.fDate >='" + objPO.StartDate + "'  AND p.fDate <='" + objPO.EndDate + "' ");
                varname.Append("                    ORDER BY DATEADD(dd, -1, DATEADD(wk,DATEDIFF(wk,0,p.Due),0))      \n");
                return objPO.Ds = SqlHelper.ExecuteDataset(objPO.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetPOWeekly(GetPOWeeklyParam _GetPOWeekly, string ConnectionString)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    SELECT      ");
                varname.Append("            p.PO,                   \n");
                varname.Append("            p.fDate as Post,        \n");
                varname.Append("            p.Due,                  \n");
                varname.Append("            p.fDesc,                \n");
                varname.Append("            p.Vendor,               \n");
                varname.Append("            r.Name AS VendorName,   \n");
                varname.Append("            isnull(p.Amount,0) as Amount,   \n");
                varname.Append("            p.Status,               \n");
                varname.Append("            (CASE p.Status WHEN 0 THEN 'Open'        \n");
                varname.Append("                WHEN 1 THEN 'Closed'                 \n");
                varname.Append("                WHEN 2 THEN 'Void'  END) AS StatusName,   \n");
                varname.Append("            DATEPART(wk, p.Due) as WeekCount,        \n");
                varname.Append("            DATEADD(dd, -1, DATEADD(wk,DATEDIFF(wk,0,p.Due),0)) as WeekDate         \n");
                varname.Append("            FROM PO AS p        \n");
                varname.Append("                	INNER JOIN Vendor AS v on p.Vendor = v.ID             \n");
                varname.Append("                    INNER JOIN Rol AS r on v.Rol = r.ID                   \n");
                varname.Append("                WHERE  p.Status = 0     \n");
                varname.Append("                   AND p.fDate >='" + _GetPOWeekly.StartDate + "'  AND p.fDate <='" + _GetPOWeekly.EndDate + "' ");
                varname.Append("                    ORDER BY DATEADD(dd, -1, DATEADD(wk,DATEDIFF(wk,0,p.Due),0))      \n");
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetFederalReport(CD _objCD) // Get Vendors details who's expense is exists in PJ table.
        {
            try
            {
                int YR = _objCD.EndDate.Year;
                string SqlQuery = "SELECT v.ID, r.Name, Custom.Label, v.Acct, v.Balance,  ";
                SqlQuery = SqlQuery + " sum(c.Amount) as Paid, v.Rol, r.Address as Address, ";
                //+', '+Char(13)+CHAR(10)+ r.City+', '+r.State+', '+r.Zip  as Address, ";
                SqlQuery = SqlQuery + "  r.City, r.State, r.Zip, v.Remit, v.FID, v.intBox FROM Vendor v INNER JOIN CD c on v.ID = c.Vendor ";
                SqlQuery = SqlQuery + " INNER JOIN Rol r on r.ID = v.Rol ";
                SqlQuery = SqlQuery + " INNER JOIN Custom ON Custom.Name= 'FederalID'";
                SqlQuery = SqlQuery + "WHERE datepart(year,c.fDate) = " + YR + " and v.[1099] = 1";
                SqlQuery = SqlQuery + " GROUP By v.ID, v.Acct, v.Balance, v.Rol, r.Name, Custom.Label, r.Address, r.City, r.State, r.Zip, v.Remit, v.FID, v.intBox ";
                SqlQuery = SqlQuery + " HAVING sum(c.Amount) >= " + _objCD.Amount + "";
                return _objCD.Ds = SqlHelper.ExecuteDataset(_objCD.ConnConfig, CommandType.Text, SqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetFederalReport(GetFederalReportParam _GetFederalReportParam, string ConnectionString) // Get Vendors details who's expense is exists in PJ table.
        {
            try
            {
                int YR = _GetFederalReportParam.EndDate.Year;
                string SqlQuery = "SELECT v.ID, r.Name, Custom.Label, v.Acct, v.Balance,  ";
                SqlQuery = SqlQuery + " sum(c.Amount) as Paid, v.Rol, r.Address as Address, ";
                //+', '+Char(13)+CHAR(10)+ r.City+', '+r.State+', '+r.Zip  as Address, ";
                SqlQuery = SqlQuery + "  r.City, r.State, r.Zip, v.Remit, v.FID, v.intBox FROM Vendor v INNER JOIN CD c on v.ID = c.Vendor ";
                SqlQuery = SqlQuery + " INNER JOIN Rol r on r.ID = v.Rol ";
                SqlQuery = SqlQuery + " INNER JOIN Custom ON Custom.Name= 'FederalID'";
                SqlQuery = SqlQuery + "WHERE datepart(year,c.fDate) = " + YR + " and v.[1099] = 1";
                SqlQuery = SqlQuery + " GROUP By v.ID, v.Acct, v.Balance, v.Rol, r.Name, Custom.Label, r.Address, r.City, r.State, r.Zip, v.Remit, v.FID, v.intBox ";
                SqlQuery = SqlQuery + " HAVING sum(c.Amount) >= " + _GetFederalReportParam.Amount + "";
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, SqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetIncomeStatement12Period(Chart objChart)
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "fDate";
                param.SqlDbType = SqlDbType.DateTime;
                param.Value = objChart.EndDate;

                return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.StoredProcedure, "spGetIncomeStatement12Period", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetIncomeStatementYTD(Chart objChart)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT   \n");
                sb.Append("	c.ID AS Acct,  \n");
                sb.Append("	c.Acct+'  '+c.fDesc	AS fDesc,  \n");
                sb.Append("	c.Type, \n");
                sb.Append("	(CASE c.Type WHEN 0 THEN 'Asset'        \n");
                sb.Append("		WHEN 1 THEN 'Liability'             \n");
                sb.Append("		WHEN 2 THEN 'Equity'                \n");
                sb.Append("		WHEN 3 THEN 'Revenues'              \n");
                sb.Append("		WHEN 4 THEN 'Cost of Sales'         \n");
                sb.Append("		WHEN 5 THEN 'Expenses'              \n");
                sb.Append("		WHEN 6 THEN 'Bank'                  \n");
                sb.Append("		WHEN 7 THEN 'Non-Posting'           \n");
                sb.Append("		WHEN 8 THEN 'Other Income (Expense)'\n");
                sb.Append("		WHEN 9 THEN 'Provisions for Income Taxes' \n");
                sb.Append("	END) AS TypeName,                       \n");
                sb.Append("	(CASE c.Sub WHEN '' THEN                \n");
                sb.Append("		(CASE c.Type WHEN 0 THEN 'Asset'    \n");
                sb.Append("			WHEN 1 THEN 'Liability'         \n");
                sb.Append("			WHEN 2 THEN 'Equity'            \n");
                sb.Append("			WHEN 3 THEN 'Revenues'          \n");
                sb.Append("			WHEN 4 THEN 'Cost of Sales'     \n");
                sb.Append("			WHEN 5 THEN 'Expenses'          \n");
                sb.Append("			WHEN 6 THEN 'Bank'              \n");
                sb.Append("			WHEN 7 THEN 'Non-Posting'           \n");
                sb.Append("			WHEN 8 THEN 'Other Income (Expense)'\n");
                sb.Append("			WHEN 9 THEN 'Provisions for Income Taxes' \n");
                sb.Append("		END)             \n");
                sb.Append("	ELSE c.Sub END)		AS Sub,  \n");
                sb.Append("	SUM(CASE c.Type \n");
                sb.Append("		WHEN 3 THEN (ISNULL(t.Amount,0) * -1) \n");
                sb.Append("		WHEN 8 THEN (ISNULL(t.Amount,0) * -1) \n");
                sb.Append("	ELSE \n");
                sb.Append("		ISNULL(t.Amount,0) \n");
                sb.Append("	END) AS NTotal, \n");
                sb.Append("	'' As Url \n");
                sb.Append("FROM Chart c  \n");
                sb.Append("	LEFT JOIN Trans t ON t.Acct = c.ID AND t.fDate >= '" + objChart.StartDate + "' AND t.fDate <= '" + objChart.EndDate + "' \n");
                sb.Append("WHERE c.Type IN (3, 4, 5, 8, 9)  \n");
                sb.Append("	AND (c.Status = 0 OR t.Amount IS NOT NULL)  \n");
                sb.Append("GROUP BY c.ID, c.Acct, c.fDesc, c.Type, c.Sub \n");
                sb.Append("ORDER BY c.ID	 \n");

                sb.Append("SELECT   \n");
                sb.Append("	t.Acct,  \n");
                sb.Append("	c.Acct+'  '+c.fDesc	AS fDesc,  \n");
                sb.Append("	c.Type, \n");
                sb.Append("	(CASE c.Type WHEN 0 THEN 'Asset'        \n");
                sb.Append("		WHEN 1 THEN 'Liability'             \n");
                sb.Append("		WHEN 2 THEN 'Equity'                \n");
                sb.Append("		WHEN 3 THEN 'Revenues'              \n");
                sb.Append("		WHEN 4 THEN 'Cost of Sales'         \n");
                sb.Append("		WHEN 5 THEN 'Expenses'              \n");
                sb.Append("		WHEN 6 THEN 'Bank'                  \n");
                sb.Append("		WHEN 7 THEN 'Non-Posting'           \n");
                sb.Append("		WHEN 8 THEN 'Other Income (Expense)'\n");
                sb.Append("		WHEN 9 THEN 'Provisions for Income Taxes' \n");
                sb.Append("		END) AS TypeName,                   \n");
                sb.Append("	(CASE c.Sub WHEN '' THEN                \n");
                sb.Append("		(CASE c.Type WHEN 0 THEN 'Asset'    \n");
                sb.Append("			WHEN 1 THEN 'Liability'         \n");
                sb.Append("			WHEN 2 THEN 'Equity'            \n");
                sb.Append("			WHEN 3 THEN 'Revenues'          \n");
                sb.Append("			WHEN 4 THEN 'Cost of Sales'     \n");
                sb.Append("			WHEN 5 THEN 'Expenses'          \n");
                sb.Append("			WHEN 6 THEN 'Bank'              \n");
                sb.Append("			WHEN 7 THEN 'Non-Posting'       \n");
                sb.Append("			WHEN 8 THEN 'Other Income (Expense)'\n");
                sb.Append("			WHEN 9 THEN 'Provisions for Income Taxes' \n");
                sb.Append("		END)             \n");
                sb.Append("	ELSE c.Sub END)		AS Sub,  \n");
                sb.Append("	SUM(CASE c.Type \n");
                sb.Append("		WHEN 3 THEN (ISNULL(t.Amount,0) * -1) \n");
                sb.Append("		WHEN 8 THEN (ISNULL(t.Amount,0) * -1) \n");
                sb.Append("	ELSE \n");
                sb.Append("		ISNULL(t.Amount,0) \n");
                sb.Append("	END) AS NTotal, \n");
                sb.Append("	'' AS Url \n");
                sb.Append("FROM Trans t  \n");
                sb.Append("INNER JOIN Chart c ON t.Acct = c.ID  \n");
                sb.Append("WHERE c.Type IN (3, 4, 5, 8, 9)  \n");
                sb.Append("	AND t.Amount <> 0  \n");
                sb.Append("	AND t.fDate >= '" + objChart.YearStartDate + "' \n");
                sb.Append("	AND t.fDate <= '" + objChart.EndDate + "' \n");
                sb.Append("GROUP BY t.Acct, c.Acct, c.fDesc, c.Type, c.Sub \n");
                sb.Append("ORDER BY t.Acct \n");

                return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetIncomeStatement12PeriodWithCenters(Chart objChart)
        {
            try
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "fDate";
                param.SqlDbType = SqlDbType.DateTime;
                param.Value = objChart.EndDate;

                SqlParameter param1 = new SqlParameter();
                param.ParameterName = "Departments";
                param.SqlDbType = SqlDbType.VarChar;
                param.Value = objChart.Departments;

                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = param;
                parameters[1] = param1;
                return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.StoredProcedure, "spGetIncomeStatement12PeriodWithCenters", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetParentTables(string connString, string module)
        {
            String strConnString = connString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Report_GetParentTable";
            cmd.Parameters.Add("@Module", SqlDbType.VarChar).Value = module;
            cmd.Connection = con;
            try
            {
                con.Open();
                var ds = new DataSet();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "Reports");
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

        public DataSet GetChildTables(string connString, string parentTableName)
        {
            String strConnString = connString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Report_GetChildTable";
            cmd.Parameters.Add("@ParentTableName", SqlDbType.VarChar).Value = parentTableName;
            cmd.Connection = con;
            try
            {
                con.Open();
                var ds = new DataSet();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "Reports");
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

        public DataSet GetReportColumns(string connString, string reportTable, string module)
        {
            String strConnString = connString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Report_GetColumns";
            cmd.Parameters.Add("@ReportTable", SqlDbType.VarChar).Value = reportTable;
            cmd.Parameters.Add("@Module", SqlDbType.VarChar).Value = module;
            cmd.Connection = con;
            try
            {
                con.Open();
                var ds = new DataSet();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "Reports");
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

        public DataSet GetCustomColumns(string connString)
        {
            String strConnString = connString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spGetDynamicLocationsReport";
            cmd.Connection = con;
            try
            {
                con.Open();
                var ds = new DataSet();
                var sqlda = new SqlDataAdapter(cmd);
                ds = new DataSet();
                sqlda.Fill(ds, "Reports");
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

        public DataSet GetCompanyLogo(string connString)
        {
            String strConnString = connString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Budget_GetCompanyLogo";
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

        public DataSet GetCompanyDetails(string connString)
        {
            String strConnString = connString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Report_GetCompanyDetails";
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

        public DataSet GetCompanyDetails(GetCompanyDetailsParam _GetCompanyDetailsParam, string ConnectionString)
        {
            String strConnString = ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Report_GetCompanyDetails";
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


        public DataSet StandardIncomeStatementComparativeFsWithCenter(string connString, ComparativeFSWithCenterParam param)
        {
            try
            {
                var query = @"select c.Acct,c.ID as AcctId,c.Acct+'  '+c.fDesc AS fDesc,c.Type,d.CentralName,d.ID as CentralId
                                ,
                                CASE c.Type                         
                                       WHEN 3 THEN (acc.TotalAmount * -1)     
                                       ELSE acc.TotalAmount

                                 END As MonthTotalAmountPreviousYear
                                ,CASE c.Type                         
                                       WHEN 3 THEN (acc1.TotalAmount * -1)     
                                       ELSE acc1.TotalAmount 
	                                END
                                 As MonthTotalAmountCurrentYear
                                ,CASE c.Type                         
                                       WHEN 3 THEN (acc2.TotalAmount * -1)     
                                       ELSE acc2.TotalAmount
	                                END
                                 As YTDTotalAmountPreviousYear
                                ,CASE c.Type                         
                                       WHEN 3 THEN (acc3.TotalAmount * -1)     
                                       ELSE acc3.TotalAmount 
	                                END
                                 As YTDTotalAmountCurrentYear

                                 from Chart as c left join 

                                (select Acct,sum(Amount) as TotalAmount from Trans where  fDate >= @MonthStartDatePreviousYear AND fDate <= @MonthEndDatePreviousYear  
                                group by Acct) as acc

                                on c.ID = acc.Acct 

                                left join 
                                (select Acct,sum(Amount) as TotalAmount from Trans where  fDate >= @MonthStartDateCurrentYear AND fDate <= @MonthEndDateCurrentYear  
                                group by Acct ) as acc1
                                on c.ID = acc1.Acct

                                left join 
                                (select Acct,sum(Amount) as TotalAmount from Trans where  fDate >= @YTDStartDatePreviousYear AND fDate <= @YTDEndDatePreviousYear  
                                group by Acct ) as acc2
                                on c.ID = acc2.Acct

                                left join 
                                (select Acct,sum(Amount) as TotalAmount from Trans where  fDate >= @YTDStartDateCurrentYear AND fDate <= @YTDSEndDateCurrentYear  
                                group by Acct ) as acc3
                                on c.ID = acc3.Acct

                                INNER JOIN Central d ON c.Department = d.ID 
                                where 
                                 c.Type IN (3, 4, 5)   
                                AND c.Department IN (@Department)   
                                and (acc.TotalAmount is not null 
                                or acc1.TotalAmount is not null  
                                or acc2.TotalAmount is not null  
                                or acc3.TotalAmount is not null  
                                )
                                order by c.Acct";

                var dic = new Dictionary<string, DateTime>
                {
                    { "@MonthStartDatePreviousYear",param.MonthStartDatePreviousYear },
                    { "@MonthEndDatePreviousYear",param.MonthEndDatePreviousYear },
                    { "@MonthStartDateCurrentYear",param.MonthStartDateCurrentYear },
                    { "@MonthEndDateCurrentYear",param.MonthEndDateCurrentYear },

                    { "@YTDStartDatePreviousYear",param.YTDStartDatePreviousYear },
                    { "@YTDEndDatePreviousYear",param.YTDEndDatePreviousYear },
                    { "@YTDStartDateCurrentYear",param.YTDStartDateCurrentYear },
                    { "@YTDSEndDateCurrentYear",param.YTDEndDateCurrentYear }
                };

                query = query.Replace("@Department", param.Departments);

                var parameters = dic.Select(t => new SqlParameter
                {
                    ParameterName = t.Key,
                    SqlDbType = SqlDbType.DateTime,
                    Value = t.Value
                }
                ).ToList();

                return SqlHelper.ExecuteDataset(connString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDepartmentByReceiptID(string connString, int receiptID)
        {
            try
            {
                return SqlHelper.ExecuteDataset(connString, CommandType.Text, "SELECT Type FROM   JobType jt  WHERE  jt.ID = (SELECT Type FROM   Invoice WHERE  Ref = (select top 1 InvoiceID from PaymentDetails where ReceivedPaymentID =" + receiptID + "))");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddComparativeReport(string connString, int userID, string reportName, string departments, string states)
        {
            try
            {
                string query = "INSERT INTO [ComparativeReport](UserID, Name, Departments, States) VALUES(@UserID, @Name, @Departments, @States); SELECT SCOPE_IDENTITY()";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@UserID", userID));
                parameters.Add(new SqlParameter("@Name", reportName));
                parameters.Add(new SqlParameter("@Departments", departments));
                parameters.Add(new SqlParameter("@States", states));

                return Convert.ToInt32(SqlHelper.ExecuteScalar(connString, CommandType.Text, query, parameters.ToArray()));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateComparativeReport(string connString, int reportID, string reportName, string departments)
        {
            try
            {
                string query = "UPDATE [ComparativeReport] SET Name = @Name, Departments = @Departments WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", reportID));
                parameters.Add(new SqlParameter("@Name", reportName));
                parameters.Add(new SqlParameter("@Departments", departments));

                SqlHelper.ExecuteDataset(connString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetComparativeReportByID(string connString, int reportID)
        {
            try
            {
                return SqlHelper.ExecuteDataset(connString, CommandType.Text, "SELECT * FROM  ComparativeReport  WHERE ID = " + reportID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetComparativeReportByName(string connString, string name, string states, int reportID)
        {
            try
            {
                string queryString = string.Format("SELECT * FROM  ComparativeReport WHERE Name = '{0}' AND States = '{1}'", name, states);

                if (reportID > 0)
                {
                    queryString = string.Format("SELECT * FROM  ComparativeReport WHERE ID <> {0} AND Name = '{1}' AND States = '{2}'", reportID, name, states);
                }

                return SqlHelper.ExecuteDataset(connString, CommandType.Text, queryString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetComparativeReport(string connString, string states)
        {
            try
            {
                string queryString = string.Format("SELECT * FROM  ComparativeReport WHERE States = '{0}'", states);

                return SqlHelper.ExecuteDataset(connString, CommandType.Text, queryString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteComparativeReportByID(string connString, int reportID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ReportID", reportID));

                string query = "DELETE FROM ComparativeColumns WHERE ReportID = @ReportID; DELETE FROM ComparativeReport WHERE ID = @ReportID;";

                SqlHelper.ExecuteNonQuery(connString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetComparativeReportColumns(string connString, int reportID)
        {
            try
            {
                return SqlHelper.ExecuteDataset(connString, CommandType.Text, "SELECT [Index] AS Line, * FROM  ComparativeColumns  WHERE  ReportID = " + reportID + " ORDER BY [Index]");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddComparativeColumn(string connString, int reportID, ComparativeStatementRequest request)
        {
            try
            {
                string query = "INSERT INTO [dbo].[ComparativeColumns] ([ReportID],[Type],[Label],[FromDate],[ToDate],[Column1],[Column2],[Index]) VALUES (@ReportID,@Type,@Label,@FromDate,@ToDate,@Column1,@Column2,@Index)";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ReportID", reportID));
                parameters.Add(new SqlParameter("@Type", request.Type));
                parameters.Add(new SqlParameter("@Label", request.Label));
                parameters.Add(new SqlParameter("@FromDate", request.StartDate));
                parameters.Add(new SqlParameter("@ToDate", request.EndDate));
                parameters.Add(new SqlParameter("@Column1", request.Column1));
                parameters.Add(new SqlParameter("@Column2", request.Column2));
                parameters.Add(new SqlParameter("@Index", request.Index));

                SqlHelper.ExecuteDataset(connString, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteComparativeColumnByReportID(string connString, int reportID)
        {
            try
            {
                string query = $"DELETE FROM ComparativeColumns WHERE ReportID = {reportID}";

                SqlHelper.ExecuteDataset(connString, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLocationEquipment(string connString, int locID)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT ID, Unit FROM Elev WHERE Loc = " + locID + " ORDER BY Unit\n");
                sb.Append("SELECT  \n");
                sb.Append("	State, \n");
                sb.Append("	ID,\n");
                sb.Append("	Unit,\n");
                sb.Append("	CASE WHEN ISNULL(Type, '') = '' THEN 'None' ELSE Type END AS Type,\n");
                sb.Append("	fDesc,\n");
                sb.Append("	CASE WHEN Status = 0 THEN 'Active' ELSE 'Inactive' END AS Status\n");
                sb.Append("FROM Elev\n");
                sb.Append("WHERE Loc = " + locID + " \n");
                sb.Append("ORDER BY Unit\n");
                sb.Append("SELECT DISTINCT fDesc FROM ElevTItem WHERE  Elev IN (SELECT ID FROM Elev WHERE Loc = " + locID + ") ORDER BY fDesc\n");
                sb.Append("SELECT \n");
                sb.Append("	e.Unit,\n");
                sb.Append("	isnull(OrderNo,Line) as OrderNo, \n");
                sb.Append("	eti.ID,\n");
                sb.Append("	eti.Elev,\n");
                sb.Append("	eti.CustomID,\n");
                sb.Append("	eti.fDesc,\n");
                sb.Append("	eti.Line,\n");
                sb.Append("	eti.Value,\n");
                sb.Append("	eti.Format\n");
                sb.Append("FROM   \n");
                sb.Append("	ElevTItem eti\n");
                sb.Append("	INNER JOIN Elev e ON eti.Elev = e.ID\n");
                sb.Append("WHERE  eti.Elev IN (SELECT ID FROM Elev WHERE Loc = " + locID + ")\n");
                sb.Append("ORDER BY eti.fDesc, e.Unit\n");

                return SqlHelper.ExecuteDataset(connString, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetContractListingByRoute(Chart objChart, string routes, bool isActiveOnly)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[4];

                parameters[0] = new SqlParameter();
                parameters[0].ParameterName = "StartDate";
                parameters[0].SqlDbType = SqlDbType.DateTime;
                parameters[0].Value = objChart.StartDate;

                parameters[1] = new SqlParameter();
                parameters[1].ParameterName = "EndDate";
                parameters[1].SqlDbType = SqlDbType.DateTime;
                parameters[1].Value = objChart.EndDate;

                parameters[2] = new SqlParameter();
                parameters[2].ParameterName = "Routes";
                parameters[2].SqlDbType = SqlDbType.VarChar;
                parameters[2].Value = routes;

                parameters[3] = new SqlParameter();
                parameters[3].ParameterName = "IsActiveOnly";
                parameters[3].SqlDbType = SqlDbType.Bit;
                parameters[3].Value = isActiveOnly;

                return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.StoredProcedure, "spGetContractListingByRoute", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetProjectListingByRouteWithBudgetedHours(Chart objChart, string routes, string department, bool includeClose)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[5];

                parameters[0] = new SqlParameter();
                parameters[0].ParameterName = "StartDate";
                parameters[0].SqlDbType = SqlDbType.DateTime;
                parameters[0].Value = objChart.StartDate;

                parameters[1] = new SqlParameter();
                parameters[1].ParameterName = "EndDate";
                parameters[1].SqlDbType = SqlDbType.DateTime;
                parameters[1].Value = objChart.EndDate;

                parameters[2] = new SqlParameter();
                parameters[2].ParameterName = "Routes";
                parameters[2].SqlDbType = SqlDbType.VarChar;
                parameters[2].Value = routes;

                parameters[3] = new SqlParameter();
                parameters[3].ParameterName = "Department";
                parameters[3].SqlDbType = SqlDbType.VarChar;
                parameters[3].Value = department;

                parameters[4] = new SqlParameter();
                parameters[4].ParameterName = "IncludeClose";
                parameters[4].SqlDbType = SqlDbType.Bit;
                parameters[4].Value = includeClose;

                return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.StoredProcedure, "spGetProjectListingByRouteWithBudgetHours", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetContractRoute(Chart objChart)
        {
            try
            {
                string sql = "SELECT DISTINCT rou.ID, rou.Name FROM Contract ct INNER JOIN Job j ON j.ID = ct.Job INNER JOIN Loc l ON l.Loc = j.Loc INNER JOIN Route rou ON rou.ID = l.Route ORDER BY rou.Name";

                return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetContractGroupRoute(Chart objChart)
        {
            try
            {
                string sql = "SELECT DISTINCT RouteName FROM (SELECT LEFT(rou.Name, 6) AS RouteName FROM Contract ct INNER JOIN Job j ON j.ID = ct.Job INNER JOIN Loc l ON l.Loc = j.Loc INNER JOIN Route rou ON rou.ID = l.Route ) AS t ORDER BY RouteName";

                return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCompletedTicket(MapData objPropMapData, List<RetainFilter> filters)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT * FROM( \n");
                //if (objPropMapData.FilterReview != "1")
                //{
                #region TicketO table

                sb.Append("	SELECT  \n");
                sb.Append("			t.[ID] \n");
                sb.Append("			,t.[CDate] \n");
                sb.Append("			,t.[DDate] \n");
                sb.Append("			,t.[EDate] \n");
                sb.Append("			,t.[fWork] \n");
                sb.Append("			,t.[Job] \n");
                sb.Append("			,t.[LID] AS Loc \n");
                sb.Append("			,t.[LElev] AS Elev \n");
                sb.Append("			,t.[Type] \n");
                sb.Append("			,t.[fDesc] \n");
                sb.Append("			,dp.[DescRes] \n");
                sb.Append("			,dp.[Total] \n");
                sb.Append("			,dp.[Reg] \n");
                sb.Append("			,dp.[OT] \n");
                sb.Append("			,dp.[DT] \n");
                sb.Append("			,dp.[TT] \n");
                sb.Append("			,dp.[Zone] \n");
                sb.Append("			,dp.[Toll] \n");
                sb.Append("			,dp.[OtherE] \n");
                sb.Append("			,dp.[Status] \n");
                sb.Append("			,dp.[Invoice] \n");
                sb.Append("			,t.[Level] \n");
                sb.Append("			,t.[Est] \n");
                sb.Append("			,t.[Cat] \n");
                sb.Append("			,t.[Who] \n");
                sb.Append("			,t.[fBy] \n");
                sb.Append("			,t.[fLong] \n");
                sb.Append("			,t.[Latt] \n");
                sb.Append("			,dp.[WageC] \n");
                sb.Append("			,dp.[Phase] \n");
                sb.Append("			,dp.[Car] \n");
                sb.Append("			,dp.[CallIn] \n");
                sb.Append("			,dp.[Mileage] \n");
                sb.Append("			,dp.[NT] \n");
                sb.Append("			,dp.[CauseID] \n");
                sb.Append("			,dp.[CauseDesc] \n");
                sb.Append("			,dp.[Comments] \n");
                sb.Append("			,t.[fGroup] \n");
                sb.Append("			,t.[PriceL] \n");
                sb.Append("			,t.[WorkOrder] \n");
                sb.Append("			,t.[TimeRoute] \n");
                sb.Append("			,t.[TimeSite] \n");
                sb.Append("			,t.[TimeComp] \n");
                sb.Append("			,jt.[Type] AS JobType \n");
                sb.Append("			,ww.[fDesc] AS Mech \n");
                sb.Append("			,l.[Tag] \n");
                sb.Append("			,l.[Address]  \n");
                sb.Append("			,l.[City] \n");
                sb.Append("			,l.[State] \n");
                sb.Append("			,l.[Zip] \n");
                sb.Append("			,CASE \n");
                sb.Append("			    WHEN t.Assigned = 0 THEN 'Un-Assigned' \n");
                sb.Append("			    WHEN t.Assigned = 1 THEN 'Assigned' \n");
                sb.Append("			    WHEN t.Assigned = 2 THEN 'Enroute' \n");
                sb.Append("			    WHEN t.Assigned = 3 THEN 'Onsite' \n");
                sb.Append("			    WHEN t.Assigned = 4 THEN 'Completedd' \n");
                sb.Append("			    WHEN t.Assigned = 5 THEN 'Hold' \n");
                sb.Append("			    WHEN t.Assigned = 6 THEN 'Voided' \n");
                sb.Append("			END AS Assignname \n");
                sb.Append("         ,(SELECT STUFF((SELECT ', ' + CAST(e.Unit AS VARCHAR(1000))  \n");
                sb.Append("         FROM Elev e WHERE e.id IN  \n");
                sb.Append("             (SELECT me.elev_id FROM multiple_equipments me WHERE me.ticket_id = t.ID) \n");
                sb.Append("             FOR XML PATH(''), Type).value('.', 'VARCHAR(1000)'),1,2,' ')) AS Unit \n");
                sb.Append("         ,(SELECT TOP 1 Signature FROM   PDATicketSignature WHERE  PDATicketID = t.ID) AS Signature \n");
                sb.Append("		FROM TicketO t \n");
                sb.Append("			LEFT OUTER JOIN TicketDPDA dp ON t.ID = dp.ID \n");
                sb.Append("			INNER JOIN Loc l ON l.Loc = t.LID  	 \n");
                sb.Append("			LEFT JOIN Route rou ON rou.ID = l.Route \n");
                sb.Append("			LEFT JOIN Job j ON j.ID = t.Job \n");
                sb.Append("			LEFT JOIN JobType jt ON jt.ID = j.Type  \n");
                sb.Append("			LEFT JOIN tblWork m ON m.ID = rou.Mech \n");
                sb.Append("			LEFT JOIN  tblWork ww ON ww.ID = t.fWork \n");
                sb.Append("			LEFT JOIN Elev e ON e.ID = t.LElev \n");
                sb.Append("		WHERE t.Assigned = 4 \n");
                sb.Append("			AND t.EDate >= '" + objPropMapData.StartDate + "' AND t.EDate <= '" + objPropMapData.EndDate + "' \n");

                // If get from from Edit Location screen
                if (objPropMapData.LocID > 0)
                {
                    sb.Append("			AND t.LID = " + objPropMapData.LocID + " \n");
                }

                // If get from from Edit Customer screen
                if (objPropMapData.CustID > 0)
                {
                    sb.Append("			AND l.Owner = " + objPropMapData.CustID + " \n");
                }

                //Advanced Search
                if (!string.IsNullOrEmpty(objPropMapData.Supervisor))
                {
                    sb.Append("			AND m.Super ='" + objPropMapData.Supervisor + "' \n");
                }
                if (!string.IsNullOrEmpty(objPropMapData.FilterCharge))
                {
                    sb.Append("			AND (ISNULL(dp.Charge,0)= " + Convert.ToInt32(objPropMapData.FilterCharge));

                    if (objPropMapData.FilterCharge == "1")
                    {
                        sb.Append(" OR ISNULL(dp.Invoice,0) <> 0) \n");
                    }
                    else
                    {
                        sb.Append(" AND ISNULL(dp.Invoice,0) = 0) \n");
                    }
                }
                if (!string.IsNullOrEmpty(objPropMapData.FilterReview))
                {
                    sb.Append("			AND ISNULL(dp.ClearCheck,0)= " + Convert.ToInt32(objPropMapData.FilterReview) + " \n");
                }
                if (!string.IsNullOrEmpty(objPropMapData.Workorder))
                {
                    sb.Append("			AND t.Workorder= '" + objPropMapData.Workorder + "' \n");
                }
                if (!string.IsNullOrEmpty(objPropMapData.Route))
                {
                    if (Convert.ToInt32(objPropMapData.Route) == 0)
                    {
                        sb.Append("			AND l.Route = 0 \n");
                    }
                    else
                    {
                        sb.Append("			AND rou.ID = " + Convert.ToInt32(objPropMapData.Route) + " \n");
                    }
                }
                if (objPropMapData.Department >= 0)
                {
                    sb.Append("			AND t.type = " + objPropMapData.Department + " \n");
                }
                if (!string.IsNullOrEmpty(objPropMapData.IsPortal))
                {
                    if (objPropMapData.IsPortal == "1")
                    {
                        sb.Append("			AND t.fBy= 'portal' \n");
                    }
                    if (objPropMapData.IsPortal == "0")
                    {
                        sb.Append("			AND t.fBy <> 'portal' \n");
                    }
                }
                if (!string.IsNullOrEmpty(objPropMapData.Bremarks))
                {
                    if (objPropMapData.Bremarks == "1")
                    {
                        sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') <> '' \n");
                    }
                    else
                    {
                        sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') = '' \n");
                    }
                }
                if (objPropMapData.Mobile > 0)
                {
                    if (objPropMapData.Mobile == 2)
                    {
                        sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 2 \n");
                    }
                    if (objPropMapData.Mobile == 1)
                    {
                        sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 0 \n");
                    }
                }
                if (!string.IsNullOrEmpty(objPropMapData.Category))
                {
                    sb.Append("			AND t.Cat IN (" + objPropMapData.Category + ") \n");
                }
                if (objPropMapData.InvoiceID != 0)
                {
                    if (objPropMapData.InvoiceID == 1)
                    {
                        sb.Append("			AND ISNULL(dp.Invoice,0) <> 0 \n");
                    }
                    else if (objPropMapData.InvoiceID == 2)
                    {
                        sb.Append("			AND ISNULL(dp.Invoice,0) = 0 AND ISNULL(dp.Charge,0) = 1 \n");
                    }
                }
                if (objPropMapData.Assigned != -1)
                {
                    if (objPropMapData.Assigned == -2)
                    {
                        sb.Append("			AND t.Assigned <> 4 \n");
                    }
                    else
                    {
                        sb.Append("			AND t.Assigned = " + objPropMapData.Assigned + "  \n");
                    }
                }
                if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                {

                    if (objPropMapData.Worker == "Active")
                    {
                        sb.Append("			AND ww.Status = 0 \n");
                    }
                    else if (objPropMapData.Worker == "Inactive")
                    {
                        sb.Append("			AND ww.Status = 1 \n");
                    }
                    else
                    {
                        sb.Append("			AND ww.fDesc = '" + objPropMapData.Worker.Replace("'", "''") + "' \n");
                    }
                }

                // Search value
                if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && !string.IsNullOrEmpty(objPropMapData.SearchValue))
                {
                    if (objPropMapData.SearchBy == "t.ID")
                    {
                        sb.Append("			AND t.ID = " + objPropMapData.SearchValue + " \n");
                    }
                    if (objPropMapData.SearchBy == "t.cat")
                    {
                        sb.Append("			AND t.Cat LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "t.WorkOrder")
                    {
                        sb.Append("			AND t.WorkOrder LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "t.fdesc")
                    {
                        sb.Append("			AND t.fDesc LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "t.descres")
                    {
                        sb.Append("			AND dp.DescRes LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "r.name")
                    {
                        sb.Append("			AND rou.Name LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "l.tag")
                    {
                        sb.Append("			AND l.Tag LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "l.ldesc4")
                    {
                        sb.Append("			AND l.Address LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "l.City")
                    {
                        sb.Append("			AND l.City LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "l.state")
                    {
                        sb.Append("			AND l.State LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "l.Zip")
                    {
                        sb.Append("			AND l.Zip LIKE '%" + objPropMapData.SearchValue + "%'		 \n");
                    }
                }

                //Ticket filters
                foreach (var filter in filters)
                {
                    if (filter.FilterColumn == "ID")
                    {
                        sb.Append("			AND t.ID IN (" + filter.FilterValue + ") \n");
                    }
                    if (filter.FilterColumn == "WorkOrder")
                    {
                        sb.Append("			AND t.Workorder= '" + filter.FilterValue + "' \n");
                    }
                    if (filter.FilterColumn == "invoiceno")
                    {
                        sb.Append("			AND dp.Invoice = " + filter.FilterValue + " \n");
                    }
                    if (filter.FilterColumn == "Job")
                    {
                        sb.Append("			AND t.Job = " + filter.FilterValue + " \n");
                    }
                    if (filter.FilterColumn == "locname")
                    {
                        sb.Append("			AND l.Tag LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "City")
                    {
                        sb.Append("			AND l.City LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "fullAddress")
                    {
                        sb.Append("			AND l.Address LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "dwork")
                    {
                        sb.Append("			AND t.DWork LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "Name")
                    {
                        sb.Append("			AND rou.Name LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "cat")
                    {
                        sb.Append("			AND t.Cat LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "Tottime")
                    {
                        sb.Append("			AND dp.Total = " + filter.FilterValue + " \n");
                    }
                    if (filter.FilterColumn == "timediff")
                    {
                        sb.Append(@"		AND ROUND(CONVERT(NUMERIC(30, 2), (ISNULL(dp.Total, 0.00) - ( CONVERT(FLOAT, DATEDIFF(MILLISECOND, dp.TimeRoute, 
			                                    (CASE WHEN(CAST(dp.TimeSite AS TIME) < CAST(dp.TimeRoute AS TIME) AND CAST(dp.TimeComp AS TIME) < CAST(dp.TimeSite AS TIME)) THEN DATEADD(DAY, 2, dp.TimeComp)
				                                    ELSE((CASE
                                                        WHEN(CAST(dp.TimeSite AS TIME) < CAST(dp.TimeRoute AS TIME)
                                                            OR CAST(dp.TimeComp AS TIME) < CAST(dp.TimeSite AS TIME)) THEN DATEADD(DAY, 1, dp.TimeComp)
                                                        ELSE dp.TimeComp
                                                    END))
                                                END ))) / 1000 / 60 / 60 ) )), 1) = " + filter.FilterValue + " \n");
                    }
                    if (filter.FilterColumn == "department")
                    {
                        sb.Append("			AND jt.Type LIKE '%" + filter.FilterValue + "%' \n");
                    }
                }

                #endregion
                //}

                //if (objPropMapData.Assigned == 4 || objPropMapData.Assigned == -1)
                //{
                #region TicketD table

                sb.Append("		UNION ALL \n");
                sb.Append("		SELECT  \n");
                sb.Append("			t.[ID] \n");
                sb.Append("			,t.[CDate] \n");
                sb.Append("			,t.[DDate] \n");
                sb.Append("			,t.[EDate] \n");
                sb.Append("			,t.[fWork] \n");
                sb.Append("			,t.[Job] \n");
                sb.Append("			,t.[Loc] \n");
                sb.Append("			,t.[Elev] \n");
                sb.Append("			,t.[Type] \n");
                sb.Append("			,t.[fDesc] \n");
                sb.Append("			,t.[DescRes] \n");
                sb.Append("			,t.[Total] \n");
                sb.Append("			,t.[Reg] \n");
                sb.Append("			,t.[OT] \n");
                sb.Append("			,t.[DT] \n");
                sb.Append("			,t.[TT] \n");
                sb.Append("			,t.[Zone] \n");
                sb.Append("			,t.[Toll] \n");
                sb.Append("			,t.[OtherE] \n");
                sb.Append("			,t.[Status] \n");
                sb.Append("			,t.[Invoice] \n");
                sb.Append("			,t.[Level] \n");
                sb.Append("			,t.[Est] \n");
                sb.Append("			,t.[Cat] \n");
                sb.Append("			,t.[Who] \n");
                sb.Append("			,t.[fBy] \n");
                sb.Append("			,t.[fLong] \n");
                sb.Append("			,t.[Latt] \n");
                sb.Append("			,t.[WageC] \n");
                sb.Append("			,t.[Phase] \n");
                sb.Append("			,t.[Car] \n");
                sb.Append("			,t.[CallIn] \n");
                sb.Append("			,t.[Mileage] \n");
                sb.Append("			,t.[NT] \n");
                sb.Append("			,t.[CauseID] \n");
                sb.Append("			,t.[CauseDesc] \n");
                sb.Append("			,t.[Comments] \n");
                sb.Append("			,t.[fGroup] \n");
                sb.Append("			,t.[PriceL] \n");
                sb.Append("			,t.[WorkOrder] \n");
                sb.Append("			,t.[TimeRoute] \n");
                sb.Append("			,t.[TimeSite] \n");
                sb.Append("			,t.[TimeComp] \n");
                sb.Append("			,jt.[Type] AS JobType \n");
                sb.Append("			,w.[fDesc] AS Mech \n");
                sb.Append("			,l.[Tag] \n");
                sb.Append("			,l.[Address]  \n");
                sb.Append("			,l.[City] \n");
                sb.Append("			,l.[State] \n");
                sb.Append("			,l.[Zip] \n");
                sb.Append("			,CASE t.Assigned WHEN 6 THEN 'Voided' ELSE 'Completed' END AS Assignname \n");
                sb.Append("         ,(SELECT STUFF((SELECT ', ' + CAST(e.Unit AS VARCHAR(1000))  \n");
                sb.Append("         FROM Elev e WHERE e.id IN  \n");
                sb.Append("             (SELECT me.elev_id FROM multiple_equipments me WHERE me.ticket_id = t.ID) \n");
                sb.Append("             FOR XML PATH(''), Type).value('.', 'VARCHAR(1000)'),1,2,' ')) AS Unit \n");
                sb.Append("         ,(SELECT TOP 1 Signature FROM   PDATicketSignature WHERE  PDATicketID = t.ID) AS Signature \n");
                sb.Append("		FROM TicketD t \n");
                sb.Append("			INNER JOIN Loc l ON l.Loc = t.Loc  	 \n");
                sb.Append("			LEFT JOIN Route rou ON rou.ID = l.Route \n");
                sb.Append("			LEFT JOIN Job j ON j.ID = t.Job \n");
                sb.Append("			LEFT JOIN JobType jt ON jt.ID = j.Type  \n");
                sb.Append("			LEFT JOIN tblWork m ON m.ID = rou.Mech \n");
                sb.Append("			LEFT JOIN tblWork w ON w.ID = t.fWork \n");
                sb.Append("			LEFT JOIN Elev e ON e.ID = t.Elev \n");
                sb.Append("		WHERE t.EDate >= '" + objPropMapData.StartDate + "' AND t.EDate <= '" + objPropMapData.EndDate + "'  \n");

                // If get from from Edit Location screen
                if (objPropMapData.LocID > 0)
                {
                    sb.Append("			AND t.Loc = " + objPropMapData.LocID + " \n");
                }

                // If get from from Edit Customer screen
                if (objPropMapData.CustID > 0)
                {
                    sb.Append("			AND l.Owner = " + objPropMapData.CustID + " \n");
                }

                // Advanced Search
                if (!string.IsNullOrEmpty(objPropMapData.Supervisor))
                {
                    sb.Append("			AND m.Super ='" + objPropMapData.Supervisor + "' \n");
                }
                if (!string.IsNullOrEmpty(objPropMapData.FilterCharge))
                {
                    sb.Append("			AND (ISNULL(t.Charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge));

                    if (objPropMapData.FilterCharge == "1")
                    {
                        sb.Append(" OR ISNULL(t.Invoice,0) <> 0) \n");
                    }
                    else
                    {
                        sb.Append(" AND ISNULL(t.Invoice,0) = 0) \n");
                    }
                }
                if (!string.IsNullOrEmpty(objPropMapData.FilterReview))
                {
                    sb.Append("			AND ISNULL(t.ClearCheck,0)= " + Convert.ToInt32(objPropMapData.FilterReview) + " \n");
                }
                if (!string.IsNullOrEmpty(objPropMapData.Workorder))
                {
                    sb.Append("			AND t.Workorder= '" + objPropMapData.Workorder + "' \n");
                }
                if (!string.IsNullOrEmpty(objPropMapData.Route))
                {
                    if (Convert.ToInt32(objPropMapData.Route) == 0)
                    {
                        sb.Append("			AND l.Route = 0 \n");
                    }
                    else
                    {
                        sb.Append("			AND rou.ID = " + Convert.ToInt32(objPropMapData.Route) + " \n");
                    }
                }
                if (objPropMapData.Department >= 0)
                {
                    sb.Append("			AND t.type = " + objPropMapData.Department + " \n");
                }
                if (!string.IsNullOrEmpty(objPropMapData.IsPortal))
                {
                    if (objPropMapData.IsPortal == "1")
                    {
                        sb.Append("			AND t.fBy= 'portal' \n");
                    }
                    if (objPropMapData.IsPortal == "0")
                    {
                        sb.Append("			AND t.fBy <> 'portal' \n");
                    }
                }
                if (!string.IsNullOrEmpty(objPropMapData.Bremarks))
                {
                    if (objPropMapData.Bremarks == "1")
                    {
                        sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') <> '' \n");
                    }
                    else
                    {
                        sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') = '' \n");
                    }
                }
                if (!string.IsNullOrEmpty(objPropMapData.Timesheet))
                {
                    sb.Append("			AND ISNULL(t.TransferTime,0) = " + Convert.ToInt32(objPropMapData.Timesheet) + " \n");
                }
                if (objPropMapData.Mobile > 0)
                {
                    if (objPropMapData.Mobile == 2)
                    {
                        sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 2 \n");
                    }
                    if (objPropMapData.Mobile == 1)
                    {
                        sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 0 \n");
                    }
                }
                if (!string.IsNullOrEmpty(objPropMapData.Category))
                {
                    sb.Append("			AND t.Cat IN (" + objPropMapData.Category + ") \n");
                }
                if (objPropMapData.InvoiceID != 0)
                {
                    if (objPropMapData.InvoiceID == 1)
                    {
                        sb.Append("			AND ISNULL(t.Invoice,0) <> 0 \n");
                    }
                    else if (objPropMapData.InvoiceID == 2)
                    {
                        sb.Append("			AND ISNULL(t.Invoice,0) = 0 AND ISNULL(t.Charge,0) = 1 \n");
                    }
                }
                if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                {

                    if (objPropMapData.Worker == "Active")
                    {
                        sb.Append("			AND w.Status = 0 \n");
                    }
                    else if (objPropMapData.Worker == "Inactive")
                    {
                        sb.Append("			AND w.Status = 1 \n");
                    }
                    else
                    {
                        sb.Append("			AND w.fDesc = '" + objPropMapData.Worker.Replace("'", "''") + "' \n");
                    }
                }

                // Search value
                if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && !string.IsNullOrEmpty(objPropMapData.SearchValue))
                {
                    if (objPropMapData.SearchBy == "t.ID")
                    {
                        sb.Append("			AND t.ID = " + objPropMapData.SearchValue + " \n");
                    }
                    if (objPropMapData.SearchBy == "t.cat")
                    {
                        sb.Append("			AND t.Cat LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "t.WorkOrder")
                    {
                        sb.Append("			AND t.WorkOrder LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "t.fdesc")
                    {
                        sb.Append("			AND t.fDesc LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "t.descres")
                    {
                        sb.Append("			AND t.DescRes LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "r.name")
                    {
                        sb.Append("			AND rou.Name LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "l.tag")
                    {
                        sb.Append("			AND l.Tag LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "l.ldesc4")
                    {
                        sb.Append("			AND l.Address LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "l.City")
                    {
                        sb.Append("			AND l.City LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "l.state")
                    {
                        sb.Append("			AND l.State LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "l.Zip")
                    {
                        sb.Append("			AND l.Zip LIKE '%" + objPropMapData.SearchValue + "%' n");
                    }
                }

                //Ticket filters
                foreach (var filter in filters)
                {
                    if (filter.FilterColumn == "ID")
                    {
                        sb.Append("			AND t.ID IN (" + filter.FilterValue + ") \n");
                    }
                    if (filter.FilterColumn == "WorkOrder")
                    {
                        sb.Append("			AND t.Workorder= '" + filter.FilterValue + "' \n");
                    }
                    if (filter.FilterColumn == "invoiceno")
                    {
                        sb.Append("			AND t.Invoice = " + filter.FilterValue + " \n");
                    }
                    if (filter.FilterColumn == "Job")
                    {
                        sb.Append("			AND t.Job = " + filter.FilterValue + " \n");
                    }
                    if (filter.FilterColumn == "locname")
                    {
                        sb.Append("			AND l.Tag LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "City")
                    {
                        sb.Append("			AND l.City LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "fullAddress")
                    {
                        sb.Append("			AND l.Address LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "dwork")
                    {
                        sb.Append("			AND w.fDesc LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "Name")
                    {
                        sb.Append("			AND rou.Name LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "cat")
                    {
                        sb.Append("			AND t.Cat LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "Tottime")
                    {
                        sb.Append("			AND t.Total = " + filter.FilterValue + " \n");
                    }
                    if (filter.FilterColumn == "timediff")
                    {
                        sb.Append(@"		AND ROUND(CONVERT(NUMERIC(30, 2), (ISNULL(t.Total, 0.00) - ( CONVERT(FLOAT, DATEDIFF(MILLISECOND, t.TimeRoute, 
                                                (CASE WHEN(CAST(t.TimeSite AS TIME) < CAST(t.TimeRoute AS TIME) AND CAST(t.TimeComp AS TIME) < CAST(t.TimeSite AS TIME)) THEN DATEADD(DAY, 2, t.TimeComp)

                                                    ELSE((CASE
                                                        WHEN(CAST(t.TimeSite AS TIME) < CAST(t.TimeRoute AS TIME)
                                                            OR CAST(t.TimeComp AS TIME) < CAST(t.TimeSite AS TIME)) THEN DATEADD(DAY, 1, t.TimeComp)
                                                        ELSE t.TimeComp
                                                    END))
                                                END))) / 1000 / 60 / 60 ) )), 1) = " + filter.FilterValue + " \n");
                    }
                    if (filter.FilterColumn == "department")
                    {
                        sb.Append("			AND jt.Type LIKE '%" + filter.FilterValue + "%' \n");
                    }
                }

                #endregion
                //}

                sb.Append(") temp \n");

                sb.Append("WHERE 1 = 1 \n");

                if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && !string.IsNullOrEmpty(objPropMapData.SearchValue) && objPropMapData.SearchBy == "e.unit")
                {
                    sb.Append("	AND Unit LIKE '%" + objPropMapData.SearchValue + "%' \n");
                }

                var unitFilter = filters.FirstOrDefault(x => x.FilterColumn == "unit");
                if (unitFilter != null)
                {
                    sb.Append("			AND Unit LIKE '%" + unitFilter.FilterValue + "%' \n");
                }

                if (objPropMapData.Voided == 1)
                {
                    sb.Append("			AND temp.Assignname = 'Voided' \n");
                }

                if (objPropMapData.Assigned == 4 && objPropMapData.Voided != 1)
                {
                    sb.Append("			AND temp.Assignname <> 'Voided' \n");
                }

                sb.Append("ORDER BY temp.[ID] DESC \n");

                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetCompletedTicket(GetCompletedTicketParam _GetCompletedTicket, List<RetainFilter> filters, string ConnectionString)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT * FROM( \n");
                if (_GetCompletedTicket.FilterReview != "1")
                {
                    #region TicketO table

                    sb.Append("	SELECT  \n");
                    sb.Append("			t.[ID] \n");
                    sb.Append("			,t.[CDate] \n");
                    sb.Append("			,t.[DDate] \n");
                    sb.Append("			,t.[EDate] \n");
                    sb.Append("			,t.[fWork] \n");
                    sb.Append("			,t.[Job] \n");
                    sb.Append("			,t.[LID] AS Loc \n");
                    sb.Append("			,t.[LElev] AS Elev \n");
                    sb.Append("			,t.[Type] \n");
                    sb.Append("			,t.[fDesc] \n");
                    sb.Append("			,dp.[DescRes] \n");
                    sb.Append("			,dp.[Total] \n");
                    sb.Append("			,dp.[Reg] \n");
                    sb.Append("			,dp.[OT] \n");
                    sb.Append("			,dp.[DT] \n");
                    sb.Append("			,dp.[TT] \n");
                    sb.Append("			,dp.[Zone] \n");
                    sb.Append("			,dp.[Toll] \n");
                    sb.Append("			,dp.[OtherE] \n");
                    sb.Append("			,dp.[Status] \n");
                    sb.Append("			,dp.[Invoice] \n");
                    sb.Append("			,t.[Level] \n");
                    sb.Append("			,t.[Est] \n");
                    sb.Append("			,t.[Cat] \n");
                    sb.Append("			,t.[Who] \n");
                    sb.Append("			,t.[fBy] \n");
                    sb.Append("			,t.[fLong] \n");
                    sb.Append("			,t.[Latt] \n");
                    sb.Append("			,dp.[WageC] \n");
                    sb.Append("			,dp.[Phase] \n");
                    sb.Append("			,dp.[Car] \n");
                    sb.Append("			,dp.[CallIn] \n");
                    sb.Append("			,dp.[Mileage] \n");
                    sb.Append("			,dp.[NT] \n");
                    sb.Append("			,dp.[CauseID] \n");
                    sb.Append("			,dp.[CauseDesc] \n");
                    sb.Append("			,t.[fGroup] \n");
                    sb.Append("			,t.[PriceL] \n");
                    sb.Append("			,t.[WorkOrder] \n");
                    sb.Append("			,t.[TimeRoute] \n");
                    sb.Append("			,t.[TimeSite] \n");
                    sb.Append("			,t.[TimeComp] \n");
                    sb.Append("			,jt.[Type] AS JobType \n");
                    sb.Append("			,ww.[fDesc] AS Mech \n");
                    sb.Append("			,l.[Tag] \n");
                    sb.Append("			,l.[Address]  \n");
                    sb.Append("			,l.[City] \n");
                    sb.Append("			,l.[State] \n");
                    sb.Append("			,l.[Zip] \n");
                    sb.Append("			,CASE \n");
                    sb.Append("			    WHEN t.Assigned = 0 THEN 'Un-Assigned' \n");
                    sb.Append("			    WHEN t.Assigned = 1 THEN 'Assigned' \n");
                    sb.Append("			    WHEN t.Assigned = 2 THEN 'Enroute' \n");
                    sb.Append("			    WHEN t.Assigned = 3 THEN 'Onsite' \n");
                    sb.Append("			    WHEN t.Assigned = 4 THEN 'Completedd' \n");
                    sb.Append("			    WHEN t.Assigned = 5 THEN 'Hold' \n");
                    sb.Append("			    WHEN t.Assigned = 6 THEN 'Voided' \n");
                    sb.Append("			END AS Assignname \n");
                    sb.Append("         ,(SELECT STUFF((SELECT ', ' + CAST(e.Unit AS VARCHAR(1000))  \n");
                    sb.Append("         FROM Elev e WHERE e.id IN  \n");
                    sb.Append("             (SELECT me.elev_id FROM multiple_equipments me WHERE me.ticket_id = t.ID) \n");
                    sb.Append("             FOR XML PATH(''), Type).value('.', 'VARCHAR(1000)'),1,2,' ')) AS Unit \n");
                    sb.Append("         ,(SELECT TOP 1 Signature FROM   PDATicketSignature WHERE  PDATicketID = t.ID) AS Signature \n");
                    sb.Append("		FROM TicketO t \n");
                    sb.Append("			LEFT OUTER JOIN TicketDPDA dp ON t.ID = dp.ID \n");
                    sb.Append("			INNER JOIN Loc l ON l.Loc = t.LID  	 \n");
                    sb.Append("			LEFT JOIN Route rou ON rou.ID = l.Route \n");
                    sb.Append("			LEFT JOIN Job j ON j.ID = t.Job \n");
                    sb.Append("			LEFT JOIN JobType jt ON jt.ID = j.Type  \n");
                    sb.Append("			LEFT JOIN tblWork m ON m.ID = rou.Mech \n");
                    sb.Append("			LEFT JOIN  tblWork ww ON ww.ID = t.fWork \n");
                    sb.Append("			LEFT JOIN Elev e ON e.ID = t.LElev \n");
                    sb.Append("		WHERE t.Assigned = 4 \n");
                    sb.Append("			AND t.EDate >= '" + _GetCompletedTicket.StartDate + "' AND t.EDate <= '" + _GetCompletedTicket.EndDate + "' \n");

                    // If get from from Edit Location screen
                    if (_GetCompletedTicket.LocID > 0)
                    {
                        sb.Append("			AND t.LID = " + _GetCompletedTicket.LocID + " \n");
                    }

                    // If get from from Edit Customer screen
                    if (_GetCompletedTicket.CustID > 0)
                    {
                        sb.Append("			AND l.Owner = " + _GetCompletedTicket.CustID + " \n");
                    }

                    //Advanced Search
                    if (!string.IsNullOrEmpty(_GetCompletedTicket.Supervisor))
                    {
                        sb.Append("			AND m.Super ='" + _GetCompletedTicket.Supervisor + "' \n");
                    }
                    if (!string.IsNullOrEmpty(_GetCompletedTicket.FilterCharge))
                    {
                        sb.Append("			AND (ISNULL(dp.Charge,0)= " + Convert.ToInt32(_GetCompletedTicket.FilterCharge));

                        if (_GetCompletedTicket.FilterCharge == "1")
                        {
                            sb.Append(" OR ISNULL(dp.Invoice,0) <> 0) \n");
                        }
                        else
                        {
                            sb.Append(" AND ISNULL(dp.Invoice,0) = 0) \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(_GetCompletedTicket.FilterReview))
                    {
                        sb.Append("			AND ISNULL(dp.ClearCheck,0)= " + Convert.ToInt32(_GetCompletedTicket.FilterReview) + " \n");
                    }
                    if (!string.IsNullOrEmpty(_GetCompletedTicket.Workorder))
                    {
                        sb.Append("			AND t.Workorder= '" + _GetCompletedTicket.Workorder + "' \n");
                    }
                    if (!string.IsNullOrEmpty(_GetCompletedTicket.Route))
                    {
                        if (Convert.ToInt32(_GetCompletedTicket.Route) == 0)
                        {
                            sb.Append("			AND l.Route = 0 \n");
                        }
                        else
                        {
                            sb.Append("			AND rou.ID = " + Convert.ToInt32(_GetCompletedTicket.Route) + " \n");
                        }
                    }
                    if (_GetCompletedTicket.Department >= 0)
                    {
                        sb.Append("			AND t.type = " + _GetCompletedTicket.Department + " \n");
                    }
                    if (!string.IsNullOrEmpty(_GetCompletedTicket.IsPortal))
                    {
                        if (_GetCompletedTicket.IsPortal == "1")
                        {
                            sb.Append("			AND t.fBy= 'portal' \n");
                        }
                        if (_GetCompletedTicket.IsPortal == "0")
                        {
                            sb.Append("			AND t.fBy <> 'portal' \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(_GetCompletedTicket.Bremarks))
                    {
                        if (_GetCompletedTicket.Bremarks == "1")
                        {
                            sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') <> '' \n");
                        }
                        else
                        {
                            sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') = '' \n");
                        }
                    }
                    if (_GetCompletedTicket.Mobile > 0)
                    {
                        if (_GetCompletedTicket.Mobile == 2)
                        {
                            sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 2 \n");
                        }
                        if (_GetCompletedTicket.Mobile == 1)
                        {
                            sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 0 \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(_GetCompletedTicket.Category))
                    {
                        sb.Append("			AND t.Cat IN (" + _GetCompletedTicket.Category + ") \n");
                    }
                    if (_GetCompletedTicket.InvoiceID != 0)
                    {
                        if (_GetCompletedTicket.InvoiceID == 1)
                        {
                            sb.Append("			AND ISNULL(dp.Invoice,0) <> 0 \n");
                        }
                        else if (_GetCompletedTicket.InvoiceID == 2)
                        {
                            sb.Append("			AND ISNULL(dp.Invoice,0) = 0 AND ISNULL(dp.Charge,0) = 1 \n");
                        }
                    }
                    if (_GetCompletedTicket.Assigned != -1)
                    {
                        if (_GetCompletedTicket.Assigned == -2)
                        {
                            sb.Append("			AND t.Assigned <> 4 \n");
                        }
                        else
                        {
                            sb.Append("			AND t.Assigned = " + _GetCompletedTicket.Assigned + "  \n");
                        }
                    }
                    if (_GetCompletedTicket.Worker != string.Empty && _GetCompletedTicket.Worker != null)
                    {

                        if (_GetCompletedTicket.Worker == "Active")
                        {
                            sb.Append("			AND ww.Status = 0 \n");
                        }
                        else if (_GetCompletedTicket.Worker == "Inactive")
                        {
                            sb.Append("			AND ww.Status = 1 \n");
                        }
                        else
                        {
                            sb.Append("			AND ww.fDesc = '" + _GetCompletedTicket.Worker.Replace("'", "''") + "' \n");
                        }
                    }

                    // Search value
                    if (!string.IsNullOrEmpty(_GetCompletedTicket.SearchBy) && !string.IsNullOrEmpty(_GetCompletedTicket.SearchValue))
                    {
                        if (_GetCompletedTicket.SearchBy == "t.ID")
                        {
                            sb.Append("			AND t.ID = " + _GetCompletedTicket.SearchValue + " \n");
                        }
                        if (_GetCompletedTicket.SearchBy == "t.cat")
                        {
                            sb.Append("			AND t.Cat LIKE '%" + _GetCompletedTicket.SearchValue + "%' \n");
                        }
                        if (_GetCompletedTicket.SearchBy == "t.WorkOrder")
                        {
                            sb.Append("			AND t.WorkOrder LIKE '%" + _GetCompletedTicket.SearchValue + "%' \n");
                        }
                        if (_GetCompletedTicket.SearchBy == "t.fdesc")
                        {
                            sb.Append("			AND t.fDesc LIKE '%" + _GetCompletedTicket.SearchValue + "%' \n");
                        }
                        if (_GetCompletedTicket.SearchBy == "t.descres")
                        {
                            sb.Append("			AND dp.DescRes LIKE '%" + _GetCompletedTicket.SearchValue + "%' \n");
                        }
                        if (_GetCompletedTicket.SearchBy == "r.name")
                        {
                            sb.Append("			AND rou.Name LIKE '%" + _GetCompletedTicket.SearchValue + "%' \n");
                        }
                        if (_GetCompletedTicket.SearchBy == "l.tag")
                        {
                            sb.Append("			AND l.Tag LIKE '%" + _GetCompletedTicket.SearchValue + "%' \n");
                        }
                        if (_GetCompletedTicket.SearchBy == "l.ldesc4")
                        {
                            sb.Append("			AND l.Address LIKE '%" + _GetCompletedTicket.SearchValue + "%' \n");
                        }
                        if (_GetCompletedTicket.SearchBy == "l.City")
                        {
                            sb.Append("			AND l.City LIKE '%" + _GetCompletedTicket.SearchValue + "%' \n");
                        }
                        if (_GetCompletedTicket.SearchBy == "l.state")
                        {
                            sb.Append("			AND l.State LIKE '%" + _GetCompletedTicket.SearchValue + "%' \n");
                        }
                        if (_GetCompletedTicket.SearchBy == "l.Zip")
                        {
                            sb.Append("			AND l.Zip LIKE '%" + _GetCompletedTicket.SearchValue + "%'		 \n");
                        }
                    }

                    //Ticket filters
                    foreach (var filter in filters)
                    {
                        if (filter.FilterColumn == "ID")
                        {
                            sb.Append("			AND t.ID IN (" + filter.FilterValue + ") \n");
                        }
                        if (filter.FilterColumn == "WorkOrder")
                        {
                            sb.Append("			AND t.Workorder= '" + filter.FilterValue + "' \n");
                        }
                        if (filter.FilterColumn == "invoiceno")
                        {
                            sb.Append("			AND dp.Invoice = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "Job")
                        {
                            sb.Append("			AND t.Job = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "locname")
                        {
                            sb.Append("			AND l.Tag LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "City")
                        {
                            sb.Append("			AND l.City LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "fullAddress")
                        {
                            sb.Append("			AND l.Address LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "dwork")
                        {
                            sb.Append("			AND t.DWork LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Name")
                        {
                            sb.Append("			AND rou.Name LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "cat")
                        {
                            sb.Append("			AND t.Cat LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Tottime")
                        {
                            sb.Append("			AND dp.Total = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "timediff")
                        {
                            sb.Append(@"		AND ROUND(CONVERT(NUMERIC(30, 2), (ISNULL(dp.Total, 0.00) - ( CONVERT(FLOAT, DATEDIFF(MILLISECOND, dp.TimeRoute, 
			                                    (CASE WHEN(CAST(dp.TimeSite AS TIME) < CAST(dp.TimeRoute AS TIME) AND CAST(dp.TimeComp AS TIME) < CAST(dp.TimeSite AS TIME)) THEN DATEADD(DAY, 2, dp.TimeComp)
				                                    ELSE((CASE
                                                        WHEN(CAST(dp.TimeSite AS TIME) < CAST(dp.TimeRoute AS TIME)
                                                            OR CAST(dp.TimeComp AS TIME) < CAST(dp.TimeSite AS TIME)) THEN DATEADD(DAY, 1, dp.TimeComp)
                                                        ELSE dp.TimeComp
                                                    END))
                                                END ))) / 1000 / 60 / 60 ) )), 1) = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "department")
                        {
                            sb.Append("			AND jt.Type LIKE '%" + filter.FilterValue + "%' \n");
                        }
                    }

                    #endregion
                }

                if (_GetCompletedTicket.Assigned == 4 || _GetCompletedTicket.Assigned == -1)
                {
                    #region TicketD table

                    sb.Append("		UNION ALL \n");
                    sb.Append("		SELECT  \n");
                    sb.Append("			t.[ID] \n");
                    sb.Append("			,t.[CDate] \n");
                    sb.Append("			,t.[DDate] \n");
                    sb.Append("			,t.[EDate] \n");
                    sb.Append("			,t.[fWork] \n");
                    sb.Append("			,t.[Job] \n");
                    sb.Append("			,t.[Loc] \n");
                    sb.Append("			,t.[Elev] \n");
                    sb.Append("			,t.[Type] \n");
                    sb.Append("			,t.[fDesc] \n");
                    sb.Append("			,t.[DescRes] \n");
                    sb.Append("			,t.[Total] \n");
                    sb.Append("			,t.[Reg] \n");
                    sb.Append("			,t.[OT] \n");
                    sb.Append("			,t.[DT] \n");
                    sb.Append("			,t.[TT] \n");
                    sb.Append("			,t.[Zone] \n");
                    sb.Append("			,t.[Toll] \n");
                    sb.Append("			,t.[OtherE] \n");
                    sb.Append("			,t.[Status] \n");
                    sb.Append("			,t.[Invoice] \n");
                    sb.Append("			,t.[Level] \n");
                    sb.Append("			,t.[Est] \n");
                    sb.Append("			,t.[Cat] \n");
                    sb.Append("			,t.[Who] \n");
                    sb.Append("			,t.[fBy] \n");
                    sb.Append("			,t.[fLong] \n");
                    sb.Append("			,t.[Latt] \n");
                    sb.Append("			,t.[WageC] \n");
                    sb.Append("			,t.[Phase] \n");
                    sb.Append("			,t.[Car] \n");
                    sb.Append("			,t.[CallIn] \n");
                    sb.Append("			,t.[Mileage] \n");
                    sb.Append("			,t.[NT] \n");
                    sb.Append("			,t.[CauseID] \n");
                    sb.Append("			,t.[CauseDesc] \n");
                    sb.Append("			,t.[fGroup] \n");
                    sb.Append("			,t.[PriceL] \n");
                    sb.Append("			,t.[WorkOrder] \n");
                    sb.Append("			,t.[TimeRoute] \n");
                    sb.Append("			,t.[TimeSite] \n");
                    sb.Append("			,t.[TimeComp] \n");
                    sb.Append("			,jt.[Type] AS JobType \n");
                    sb.Append("			,w.[fDesc] AS Mech \n");
                    sb.Append("			,l.[Tag] \n");
                    sb.Append("			,l.[Address]  \n");
                    sb.Append("			,l.[City] \n");
                    sb.Append("			,l.[State] \n");
                    sb.Append("			,l.[Zip] \n");
                    sb.Append("			,CASE t.Assigned WHEN 6 THEN 'Voided' ELSE 'Completed' END AS Assignname \n");
                    sb.Append("         ,(SELECT STUFF((SELECT ', ' + CAST(e.Unit AS VARCHAR(1000))  \n");
                    sb.Append("         FROM Elev e WHERE e.id IN  \n");
                    sb.Append("             (SELECT me.elev_id FROM multiple_equipments me WHERE me.ticket_id = t.ID) \n");
                    sb.Append("             FOR XML PATH(''), Type).value('.', 'VARCHAR(1000)'),1,2,' ')) AS Unit \n");
                    sb.Append("         ,(SELECT TOP 1 Signature FROM   PDATicketSignature WHERE  PDATicketID = t.ID) AS Signature \n");
                    sb.Append("		FROM TicketD t \n");
                    sb.Append("			INNER JOIN Loc l ON l.Loc = t.Loc  	 \n");
                    sb.Append("			LEFT JOIN Route rou ON rou.ID = l.Route \n");
                    sb.Append("			LEFT JOIN Job j ON j.ID = t.Job \n");
                    sb.Append("			LEFT JOIN JobType jt ON jt.ID = j.Type  \n");
                    sb.Append("			LEFT JOIN tblWork m ON m.ID = rou.Mech \n");
                    sb.Append("			LEFT JOIN tblWork w ON w.ID = t.fWork \n");
                    sb.Append("			LEFT JOIN Elev e ON e.ID = t.Elev \n");
                    sb.Append("		WHERE t.EDate >= '" + _GetCompletedTicket.StartDate + "' AND t.EDate <= '" + _GetCompletedTicket.EndDate + "'  \n");

                    // If get from from Edit Location screen
                    if (_GetCompletedTicket.LocID > 0)
                    {
                        sb.Append("			AND t.Loc = " + _GetCompletedTicket.LocID + " \n");
                    }

                    // If get from from Edit Customer screen
                    if (_GetCompletedTicket.CustID > 0)
                    {
                        sb.Append("			AND l.Owner = " + _GetCompletedTicket.CustID + " \n");
                    }

                    // Advanced Search
                    if (!string.IsNullOrEmpty(_GetCompletedTicket.Supervisor))
                    {
                        sb.Append("			AND m.Super ='" + _GetCompletedTicket.Supervisor + "' \n");
                    }
                    if (!string.IsNullOrEmpty(_GetCompletedTicket.FilterCharge))
                    {
                        sb.Append("			AND (ISNULL(t.Charge,0)=" + Convert.ToInt32(_GetCompletedTicket.FilterCharge));

                        if (_GetCompletedTicket.FilterCharge == "1")
                        {
                            sb.Append(" OR ISNULL(t.Invoice,0) <> 0) \n");
                        }
                        else
                        {
                            sb.Append(" AND ISNULL(t.Invoice,0) = 0) \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(_GetCompletedTicket.FilterReview))
                    {
                        sb.Append("			AND ISNULL(t.ClearCheck,0)= " + Convert.ToInt32(_GetCompletedTicket.FilterReview) + " \n");
                    }
                    if (!string.IsNullOrEmpty(_GetCompletedTicket.Workorder))
                    {
                        sb.Append("			AND t.Workorder= '" + _GetCompletedTicket.Workorder + "' \n");
                    }
                    if (!string.IsNullOrEmpty(_GetCompletedTicket.Route))
                    {
                        if (Convert.ToInt32(_GetCompletedTicket.Route) == 0)
                        {
                            sb.Append("			AND l.Route = 0 \n");
                        }
                        else
                        {
                            sb.Append("			AND rou.ID = " + Convert.ToInt32(_GetCompletedTicket.Route) + " \n");
                        }
                    }
                    if (_GetCompletedTicket.Department >= 0)
                    {
                        sb.Append("			AND t.type = " + _GetCompletedTicket.Department + " \n");
                    }
                    if (!string.IsNullOrEmpty(_GetCompletedTicket.IsPortal))
                    {
                        if (_GetCompletedTicket.IsPortal == "1")
                        {
                            sb.Append("			AND t.fBy= 'portal' \n");
                        }
                        if (_GetCompletedTicket.IsPortal == "0")
                        {
                            sb.Append("			AND t.fBy <> 'portal' \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(_GetCompletedTicket.Bremarks))
                    {
                        if (_GetCompletedTicket.Bremarks == "1")
                        {
                            sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') <> '' \n");
                        }
                        else
                        {
                            sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') = '' \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(_GetCompletedTicket.Timesheet))
                    {
                        sb.Append("			AND ISNULL(t.TransferTime,0) = " + Convert.ToInt32(_GetCompletedTicket.Timesheet) + " \n");
                    }
                    if (_GetCompletedTicket.Mobile > 0)
                    {
                        if (_GetCompletedTicket.Mobile == 2)
                        {
                            sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 2 \n");
                        }
                        if (_GetCompletedTicket.Mobile == 1)
                        {
                            sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 0 \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(_GetCompletedTicket.Category))
                    {
                        sb.Append("			AND t.Cat IN (" + _GetCompletedTicket.Category + ") \n");
                    }
                    if (_GetCompletedTicket.InvoiceID != 0)
                    {
                        if (_GetCompletedTicket.InvoiceID == 1)
                        {
                            sb.Append("			AND ISNULL(t.Invoice,0) <> 0 \n");
                        }
                        else if (_GetCompletedTicket.InvoiceID == 2)
                        {
                            sb.Append("			AND ISNULL(t.Invoice,0) = 0 AND ISNULL(t.Charge,0) = 1 \n");
                        }
                    }
                    if (_GetCompletedTicket.Worker != string.Empty && _GetCompletedTicket.Worker != null)
                    {

                        if (_GetCompletedTicket.Worker == "Active")
                        {
                            sb.Append("			AND w.Status = 0 \n");
                        }
                        else if (_GetCompletedTicket.Worker == "Inactive")
                        {
                            sb.Append("			AND w.Status = 1 \n");
                        }
                        else
                        {
                            sb.Append("			AND w.fDesc = '" + _GetCompletedTicket.Worker.Replace("'", "''") + "' \n");
                        }
                    }

                    // Search value
                    if (!string.IsNullOrEmpty(_GetCompletedTicket.SearchBy) && !string.IsNullOrEmpty(_GetCompletedTicket.SearchValue))
                    {
                        if (_GetCompletedTicket.SearchBy == "t.ID")
                        {
                            sb.Append("			AND t.ID = " + _GetCompletedTicket.SearchValue + " \n");
                        }
                        if (_GetCompletedTicket.SearchBy == "t.cat")
                        {
                            sb.Append("			AND t.Cat LIKE '%" + _GetCompletedTicket.SearchValue + "%' \n");
                        }
                        if (_GetCompletedTicket.SearchBy == "t.WorkOrder")
                        {
                            sb.Append("			AND t.WorkOrder LIKE '%" + _GetCompletedTicket.SearchValue + "%' \n");
                        }
                        if (_GetCompletedTicket.SearchBy == "t.fdesc")
                        {
                            sb.Append("			AND t.fDesc LIKE '%" + _GetCompletedTicket.SearchValue + "%' \n");
                        }
                        if (_GetCompletedTicket.SearchBy == "t.descres")
                        {
                            sb.Append("			AND t.DescRes LIKE '%" + _GetCompletedTicket.SearchValue + "%' \n");
                        }
                        if (_GetCompletedTicket.SearchBy == "r.name")
                        {
                            sb.Append("			AND rou.Name LIKE '%" + _GetCompletedTicket.SearchValue + "%' \n");
                        }
                        if (_GetCompletedTicket.SearchBy == "l.tag")
                        {
                            sb.Append("			AND l.Tag LIKE '%" + _GetCompletedTicket.SearchValue + "%' \n");
                        }
                        if (_GetCompletedTicket.SearchBy == "l.ldesc4")
                        {
                            sb.Append("			AND l.Address LIKE '%" + _GetCompletedTicket.SearchValue + "%' \n");
                        }
                        if (_GetCompletedTicket.SearchBy == "l.City")
                        {
                            sb.Append("			AND l.City LIKE '%" + _GetCompletedTicket.SearchValue + "%' \n");
                        }
                        if (_GetCompletedTicket.SearchBy == "l.state")
                        {
                            sb.Append("			AND l.State LIKE '%" + _GetCompletedTicket.SearchValue + "%' \n");
                        }
                        if (_GetCompletedTicket.SearchBy == "l.Zip")
                        {
                            sb.Append("			AND l.Zip LIKE '%" + _GetCompletedTicket.SearchValue + "%' n");
                        }
                    }

                    //Ticket filters
                    foreach (var filter in filters)
                    {
                        if (filter.FilterColumn == "ID")
                        {
                            sb.Append("			AND t.ID IN (" + filter.FilterValue + ") \n");
                        }
                        if (filter.FilterColumn == "WorkOrder")
                        {
                            sb.Append("			AND t.Workorder= '" + filter.FilterValue + "' \n");
                        }
                        if (filter.FilterColumn == "invoiceno")
                        {
                            sb.Append("			AND t.Invoice = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "Job")
                        {
                            sb.Append("			AND t.Job = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "locname")
                        {
                            sb.Append("			AND l.Tag LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "City")
                        {
                            sb.Append("			AND l.City LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "fullAddress")
                        {
                            sb.Append("			AND l.Address LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "dwork")
                        {
                            sb.Append("			AND w.fDesc LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Name")
                        {
                            sb.Append("			AND rou.Name LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "cat")
                        {
                            sb.Append("			AND t.Cat LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Tottime")
                        {
                            sb.Append("			AND t.Total = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "timediff")
                        {
                            sb.Append(@"		AND ROUND(CONVERT(NUMERIC(30, 2), (ISNULL(t.Total, 0.00) - ( CONVERT(FLOAT, DATEDIFF(MILLISECOND, t.TimeRoute, 
                                                (CASE WHEN(CAST(t.TimeSite AS TIME) < CAST(t.TimeRoute AS TIME) AND CAST(t.TimeComp AS TIME) < CAST(t.TimeSite AS TIME)) THEN DATEADD(DAY, 2, t.TimeComp)

                                                    ELSE((CASE
                                                        WHEN(CAST(t.TimeSite AS TIME) < CAST(t.TimeRoute AS TIME)
                                                            OR CAST(t.TimeComp AS TIME) < CAST(t.TimeSite AS TIME)) THEN DATEADD(DAY, 1, t.TimeComp)
                                                        ELSE t.TimeComp
                                                    END))
                                                END))) / 1000 / 60 / 60 ) )), 1) = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "department")
                        {
                            sb.Append("			AND jt.Type LIKE '%" + filter.FilterValue + "%' \n");
                        }
                    }

                    #endregion
                }

                sb.Append(") temp \n");

                sb.Append("WHERE 1 = 1 \n");

                if (!string.IsNullOrEmpty(_GetCompletedTicket.SearchBy) && !string.IsNullOrEmpty(_GetCompletedTicket.SearchValue) && _GetCompletedTicket.SearchBy == "e.unit")
                {
                    sb.Append("	AND Unit LIKE '%" + _GetCompletedTicket.SearchValue + "%' \n");
                }

                var unitFilter = filters.FirstOrDefault(x => x.FilterColumn == "unit");
                if (unitFilter != null)
                {
                    sb.Append("			AND Unit LIKE '%" + unitFilter.FilterValue + "%' \n");
                }

                if (_GetCompletedTicket.Voided == 1)
                {
                    sb.Append("			AND temp.Assignname = 'Voided' \n");
                }

                if (_GetCompletedTicket.Assigned == 4 && _GetCompletedTicket.Voided != 1)
                {
                    sb.Append("			AND temp.Assignname <> 'Voided' \n");
                }

                sb.Append("ORDER BY temp.[ID] DESC \n");

                return SqlHelper.ExecuteDataset(_GetCompletedTicket.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEntrapmentTickets(MapData objPropMapData, List<RetainFilter> filters, string levels)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT * FROM( \n");

                if (objPropMapData.FilterReview != "1")
                {
                    #region TicketO table

                    sb.Append("	SELECT  \n");
                    sb.Append("			t.[ID] \n");
                    sb.Append("			,t.[CDate] \n");
                    sb.Append("			,t.[DDate] \n");
                    sb.Append("			,t.[EDate] \n");
                    sb.Append("			,t.[fWork] \n");
                    sb.Append("			,t.[Job] \n");
                    sb.Append("			,t.[LID] AS Loc \n");
                    sb.Append("			,t.[LElev] AS Elev \n");
                    sb.Append("			,t.[Type] \n");
                    sb.Append("			,t.[fDesc] \n");
                    sb.Append("			,dp.[DescRes] \n");
                    sb.Append("			,dp.[Total] \n");
                    sb.Append("			,dp.[Reg] \n");
                    sb.Append("			,dp.[OT] \n");
                    sb.Append("			,dp.[DT] \n");
                    sb.Append("			,dp.[TT] \n");
                    sb.Append("			,dp.[Zone] \n");
                    sb.Append("			,dp.[Toll] \n");
                    sb.Append("			,dp.[OtherE] \n");
                    sb.Append("			,dp.[Status] \n");
                    sb.Append("			,dp.[Invoice] \n");
                    sb.Append("			,t.[Level] \n");
                    sb.Append("			,t.[Est] \n");
                    sb.Append("			,t.[Cat] \n");
                    sb.Append("			,t.[Who] \n");
                    sb.Append("			,t.[fBy] \n");
                    sb.Append("			,t.[fLong] \n");
                    sb.Append("			,t.[Latt] \n");
                    sb.Append("			,dp.[WageC] \n");
                    sb.Append("			,dp.[Phase] \n");
                    sb.Append("			,dp.[Car] \n");
                    sb.Append("			,dp.[CallIn] \n");
                    sb.Append("			,dp.[Mileage] \n");
                    sb.Append("			,dp.[NT] \n");
                    sb.Append("			,dp.[CauseID] \n");
                    sb.Append("			,dp.[CauseDesc] \n");
                    sb.Append("			,t.[fGroup] \n");
                    sb.Append("			,t.[PriceL] \n");
                    sb.Append("			,t.[WorkOrder] \n");
                    sb.Append("			,t.[TimeRoute] \n");
                    sb.Append("			,t.[TimeSite] \n");
                    sb.Append("			,t.[TimeComp] \n");
                    sb.Append("			,jt.[Type] AS JobType \n");
                    sb.Append("			,ww.[fDesc] AS Mech \n");
                    sb.Append("			,l.[Tag] \n");
                    sb.Append("			,l.[Address]  \n");
                    sb.Append("			,l.[City] \n");
                    sb.Append("			,l.[State] \n");
                    sb.Append("			,l.[Zip] \n");
                    sb.Append("			,CASE \n");
                    sb.Append("			    WHEN t.Assigned = 0 THEN 'Un-Assigned' \n");
                    sb.Append("			    WHEN t.Assigned = 1 THEN 'Assigned' \n");
                    sb.Append("			    WHEN t.Assigned = 2 THEN 'Enroute' \n");
                    sb.Append("			    WHEN t.Assigned = 3 THEN 'Onsite' \n");
                    sb.Append("			    WHEN t.Assigned = 4 THEN 'Completedd' \n");
                    sb.Append("			    WHEN t.Assigned = 5 THEN 'Hold' \n");
                    sb.Append("			    WHEN t.Assigned = 6 THEN 'Voided' \n");
                    sb.Append("			END AS Assignname \n");
                    sb.Append("         ,(SELECT STUFF((SELECT ', ' + CAST(e.Unit AS VARCHAR(1000))  \n");
                    sb.Append("         FROM Elev e WHERE e.id IN  \n");
                    sb.Append("             (SELECT me.elev_id FROM multiple_equipments me WHERE me.ticket_id = t.ID) \n");
                    sb.Append("             FOR XML PATH(''), Type).value('.', 'VARCHAR(1000)'),1,2,' ')) AS Unit \n");
                    sb.Append("		FROM TicketO t \n");
                    sb.Append("			LEFT OUTER JOIN TicketDPDA dp ON t.ID = dp.ID \n");
                    sb.Append("			INNER JOIN Loc l ON l.Loc = t.LID  	 \n");
                    sb.Append("			LEFT JOIN Route rou ON rou.ID = l.Route \n");
                    sb.Append("			LEFT JOIN Job j ON j.ID = t.Job \n");
                    sb.Append("			LEFT JOIN JobType jt ON jt.ID = j.Type  \n");
                    sb.Append("			LEFT JOIN tblWork m ON m.ID = rou.Mech \n");
                    sb.Append("			LEFT JOIN tblWork ww ON ww.ID = t.fWork \n");
                    sb.Append("			LEFT JOIN Elev e ON e.ID = t.LElev \n");
                    sb.Append("		WHERE t.Assigned = 4 \n");
                    sb.Append("			AND t.EDate >= '" + objPropMapData.StartDate + "' AND t.EDate <= '" + objPropMapData.EndDate + "' \n");
                    if (!string.IsNullOrEmpty(levels))
                    {
                        sb.Append("			AND t.Level IN (" + levels + ") \n");
                    }

                    // If get from from Edit Location screen
                    if (objPropMapData.LocID > 0)
                    {
                        sb.Append("			AND t.LID = " + objPropMapData.LocID + " \n");
                    }

                    // If get from from Edit Customer screen
                    if (objPropMapData.CustID > 0)
                    {
                        sb.Append("			AND l.Owner = " + objPropMapData.CustID + " \n");
                    }

                    //Advanced Search
                    if (!string.IsNullOrEmpty(objPropMapData.Supervisor))
                    {
                        sb.Append("			AND m.Super ='" + objPropMapData.Supervisor + "' \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.FilterCharge))
                    {
                        sb.Append("			AND (ISNULL(dp.Charge,0)= " + Convert.ToInt32(objPropMapData.FilterCharge));

                        if (objPropMapData.FilterCharge == "1")
                        {
                            sb.Append(" OR ISNULL(dp.Invoice,0) <> 0) \n");
                        }
                        else
                        {
                            sb.Append(" AND ISNULL(dp.Invoice,0) = 0) \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.FilterReview))
                    {
                        sb.Append("			AND ISNULL(dp.ClearCheck,0)= " + Convert.ToInt32(objPropMapData.FilterReview) + " \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Workorder))
                    {
                        sb.Append("			AND t.Workorder= '" + objPropMapData.Workorder + "' \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Route))
                    {
                        if (Convert.ToInt32(objPropMapData.Route) == 0)
                        {
                            sb.Append("			AND l.Route = 0 \n");
                        }
                        else
                        {
                            sb.Append("			AND rou.ID = " + Convert.ToInt32(objPropMapData.Route) + " \n");
                        }
                    }
                    if (objPropMapData.Department >= 0)
                    {
                        sb.Append("			AND t.type = " + objPropMapData.Department + " \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.IsPortal))
                    {
                        if (objPropMapData.IsPortal == "1")
                        {
                            sb.Append("			AND t.fBy= 'portal' \n");
                        }
                        if (objPropMapData.IsPortal == "0")
                        {
                            sb.Append("			AND t.fBy <> 'portal' \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Bremarks))
                    {
                        if (objPropMapData.Bremarks == "1")
                        {
                            sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') <> '' \n");
                        }
                        else
                        {
                            sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') = '' \n");
                        }
                    }
                    if (objPropMapData.Mobile > 0)
                    {
                        if (objPropMapData.Mobile == 2)
                        {
                            sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 2 \n");
                        }
                        if (objPropMapData.Mobile == 1)
                        {
                            sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 0 \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Category))
                    {
                        sb.Append("			AND t.Cat IN (" + objPropMapData.Category + ") \n");
                    }
                    if (objPropMapData.InvoiceID != 0)
                    {
                        if (objPropMapData.InvoiceID == 1)
                        {
                            sb.Append("			AND ISNULL(dp.Invoice,0) <> 0 \n");
                        }
                        else if (objPropMapData.InvoiceID == 2)
                        {
                            sb.Append("			AND ISNULL(dp.Invoice,0) = 0 AND ISNULL(dp.Charge,0) = 1 \n");
                        }
                    }
                    if (objPropMapData.Assigned != -1)
                    {
                        if (objPropMapData.Assigned == -2)
                        {
                            sb.Append("			AND t.Assigned <> 4 \n");
                        }
                        else
                        {
                            sb.Append("			AND t.Assigned = " + objPropMapData.Assigned + "  \n");
                        }
                    }
                    if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                    {

                        if (objPropMapData.Worker == "Active")
                        {
                            sb.Append("			AND ww.Status = 0 \n");
                        }
                        else if (objPropMapData.Worker == "Inactive")
                        {
                            sb.Append("			AND ww.Status = 1 \n");
                        }
                        else
                        {
                            sb.Append("			AND ww.fDesc = '" + objPropMapData.Worker.Replace("'", "''") + "' \n");
                        }
                    }

                    // Search value
                    if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && !string.IsNullOrEmpty(objPropMapData.SearchValue))
                    {
                        if (objPropMapData.SearchBy == "t.ID")
                        {
                            sb.Append("			AND t.ID = " + objPropMapData.SearchValue + " \n");
                        }
                        if (objPropMapData.SearchBy == "t.cat")
                        {
                            sb.Append("			AND t.Cat LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "t.WorkOrder")
                        {
                            sb.Append("			AND t.WorkOrder LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "t.fdesc")
                        {
                            sb.Append("			AND t.fDesc LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "t.descres")
                        {
                            sb.Append("			AND dp.DescRes LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "r.name")
                        {
                            sb.Append("			AND rou.Name LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.tag")
                        {
                            sb.Append("			AND l.Tag LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.ldesc4")
                        {
                            sb.Append("			AND l.Address LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.City")
                        {
                            sb.Append("			AND l.City LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.state")
                        {
                            sb.Append("			AND l.State LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.Zip")
                        {
                            sb.Append("			AND l.Zip LIKE '%" + objPropMapData.SearchValue + "%'		 \n");
                        }
                    }

                    //Ticket filters
                    foreach (var filter in filters)
                    {
                        if (filter.FilterColumn == "ID")
                        {
                            sb.Append("			AND t.ID IN (" + filter.FilterValue + ") \n");
                        }
                        if (filter.FilterColumn == "WorkOrder")
                        {
                            sb.Append("			AND t.Workorder= '" + filter.FilterValue + "' \n");
                        }
                        if (filter.FilterColumn == "invoiceno")
                        {
                            sb.Append("			AND dp.Invoice = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "Job")
                        {
                            sb.Append("			AND t.Job = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "locname")
                        {
                            sb.Append("			AND l.Tag LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "City")
                        {
                            sb.Append("			AND l.City LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "fullAddress")
                        {
                            sb.Append("			AND l.Address LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "dwork")
                        {
                            sb.Append("			AND t.DWork LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Name")
                        {
                            sb.Append("			AND rou.Name LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "cat")
                        {
                            sb.Append("			AND t.Cat LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Tottime")
                        {
                            sb.Append("			AND dp.Total = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "timediff")
                        {
                            sb.Append(@"		AND ROUND(CONVERT(NUMERIC(30, 2), (ISNULL(dp.Total, 0.00) - ( CONVERT(FLOAT, DATEDIFF(MILLISECOND, dp.TimeRoute, 
			                                    (CASE WHEN(CAST(dp.TimeSite AS TIME) < CAST(dp.TimeRoute AS TIME) AND CAST(dp.TimeComp AS TIME) < CAST(dp.TimeSite AS TIME)) THEN DATEADD(DAY, 2, dp.TimeComp)
				                                    ELSE((CASE
                                                        WHEN(CAST(dp.TimeSite AS TIME) < CAST(dp.TimeRoute AS TIME)
                                                            OR CAST(dp.TimeComp AS TIME) < CAST(dp.TimeSite AS TIME)) THEN DATEADD(DAY, 1, dp.TimeComp)
                                                        ELSE dp.TimeComp
                                                    END))
                                                END ))) / 1000 / 60 / 60 ) )), 1) = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "department")
                        {
                            sb.Append("			AND jt.Type LIKE '%" + filter.FilterValue + "%' \n");
                        }
                    }

                    #endregion
                }

                if (objPropMapData.Assigned == 4 || objPropMapData.Assigned == -1)
                {
                    #region TicketD table

                    sb.Append("		UNION ALL \n");
                    sb.Append("		SELECT  \n");
                    sb.Append("			t.[ID] \n");
                    sb.Append("			,t.[CDate] \n");
                    sb.Append("			,t.[DDate] \n");
                    sb.Append("			,t.[EDate] \n");
                    sb.Append("			,t.[fWork] \n");
                    sb.Append("			,t.[Job] \n");
                    sb.Append("			,t.[Loc] \n");
                    sb.Append("			,t.[Elev] \n");
                    sb.Append("			,t.[Type] \n");
                    sb.Append("			,t.[fDesc] \n");
                    sb.Append("			,t.[DescRes] \n");
                    sb.Append("			,t.[Total] \n");
                    sb.Append("			,t.[Reg] \n");
                    sb.Append("			,t.[OT] \n");
                    sb.Append("			,t.[DT] \n");
                    sb.Append("			,t.[TT] \n");
                    sb.Append("			,t.[Zone] \n");
                    sb.Append("			,t.[Toll] \n");
                    sb.Append("			,t.[OtherE] \n");
                    sb.Append("			,t.[Status] \n");
                    sb.Append("			,t.[Invoice] \n");
                    sb.Append("			,t.[Level] \n");
                    sb.Append("			,t.[Est] \n");
                    sb.Append("			,t.[Cat] \n");
                    sb.Append("			,t.[Who] \n");
                    sb.Append("			,t.[fBy] \n");
                    sb.Append("			,t.[fLong] \n");
                    sb.Append("			,t.[Latt] \n");
                    sb.Append("			,t.[WageC] \n");
                    sb.Append("			,t.[Phase] \n");
                    sb.Append("			,t.[Car] \n");
                    sb.Append("			,t.[CallIn] \n");
                    sb.Append("			,t.[Mileage] \n");
                    sb.Append("			,t.[NT] \n");
                    sb.Append("			,t.[CauseID] \n");
                    sb.Append("			,t.[CauseDesc] \n");
                    sb.Append("			,t.[fGroup] \n");
                    sb.Append("			,t.[PriceL] \n");
                    sb.Append("			,t.[WorkOrder] \n");
                    sb.Append("			,t.[TimeRoute] \n");
                    sb.Append("			,t.[TimeSite] \n");
                    sb.Append("			,t.[TimeComp] \n");
                    sb.Append("			,jt.[Type] AS JobType \n");
                    sb.Append("			,w.[fDesc] AS Mech \n");
                    sb.Append("			,l.[Tag] \n");
                    sb.Append("			,l.[Address]  \n");
                    sb.Append("			,l.[City] \n");
                    sb.Append("			,l.[State] \n");
                    sb.Append("			,l.[Zip] \n");
                    sb.Append("			,CASE t.Assigned WHEN 6 THEN 'Voided' ELSE 'Completed' END AS Assignname \n");
                    sb.Append("         ,(SELECT STUFF((SELECT ', ' + CAST(e.Unit AS VARCHAR(1000))  \n");
                    sb.Append("         FROM Elev e WHERE e.id IN  \n");
                    sb.Append("             (SELECT me.elev_id FROM multiple_equipments me WHERE me.ticket_id = t.ID) \n");
                    sb.Append("             FOR XML PATH(''), Type).value('.', 'VARCHAR(1000)'),1,2,' ')) AS Unit \n");
                    sb.Append("		FROM TicketD t \n");
                    sb.Append("			INNER JOIN Loc l ON l.Loc = t.Loc  	 \n");
                    sb.Append("			LEFT JOIN Route rou ON rou.ID = l.Route \n");
                    sb.Append("			LEFT JOIN Job j ON j.ID = t.Job \n");
                    sb.Append("			LEFT JOIN JobType jt ON jt.ID = j.Type  \n");
                    sb.Append("			LEFT JOIN tblWork m ON m.ID = rou.Mech \n");
                    sb.Append("			LEFT JOIN tblWork w ON w.ID = t.fWork \n");
                    sb.Append("			LEFT JOIN Elev e ON e.ID = t.Elev \n");
                    sb.Append("		WHERE t.EDate >= '" + objPropMapData.StartDate + "' AND t.EDate <= '" + objPropMapData.EndDate + "'  \n");
                    if (!string.IsNullOrEmpty(levels))
                    {
                        sb.Append("			AND t.Level IN (" + levels + ") \n");
                    }

                    // If get from from Edit Location screen
                    if (objPropMapData.LocID > 0)
                    {
                        sb.Append("			AND t.Loc = " + objPropMapData.LocID + " \n");
                    }

                    // If get from from Edit Customer screen
                    if (objPropMapData.CustID > 0)
                    {
                        sb.Append("			AND l.Owner = " + objPropMapData.CustID + " \n");
                    }

                    // Advanced Search
                    if (!string.IsNullOrEmpty(objPropMapData.Supervisor))
                    {
                        sb.Append("			AND m.Super ='" + objPropMapData.Supervisor + "' \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.FilterCharge))
                    {
                        sb.Append("			AND (ISNULL(t.Charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge));

                        if (objPropMapData.FilterCharge == "1")
                        {
                            sb.Append(" OR ISNULL(t.Invoice,0) <> 0) \n");
                        }
                        else
                        {
                            sb.Append(" AND ISNULL(t.Invoice,0) = 0) \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.FilterReview))
                    {
                        sb.Append("			AND ISNULL(t.ClearCheck,0)= " + Convert.ToInt32(objPropMapData.FilterReview) + " \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Workorder))
                    {
                        sb.Append("			AND t.Workorder= '" + objPropMapData.Workorder + "' \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Route))
                    {
                        if (Convert.ToInt32(objPropMapData.Route) == 0)
                        {
                            sb.Append("			AND l.Route = 0 \n");
                        }
                        else
                        {
                            sb.Append("			AND rou.ID = " + Convert.ToInt32(objPropMapData.Route) + " \n");
                        }
                    }
                    if (objPropMapData.Department >= 0)
                    {
                        sb.Append("			AND t.type = " + objPropMapData.Department + " \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.IsPortal))
                    {
                        if (objPropMapData.IsPortal == "1")
                        {
                            sb.Append("			AND t.fBy= 'portal' \n");
                        }
                        if (objPropMapData.IsPortal == "0")
                        {
                            sb.Append("			AND t.fBy <> 'portal' \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Bremarks))
                    {
                        if (objPropMapData.Bremarks == "1")
                        {
                            sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') <> '' \n");
                        }
                        else
                        {
                            sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') = '' \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Timesheet))
                    {
                        sb.Append("			AND ISNULL(t.TransferTime,0) = " + Convert.ToInt32(objPropMapData.Timesheet) + " \n");
                    }
                    if (objPropMapData.Mobile > 0)
                    {
                        if (objPropMapData.Mobile == 2)
                        {
                            sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 2 \n");
                        }
                        if (objPropMapData.Mobile == 1)
                        {
                            sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 0 \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Category))
                    {
                        sb.Append("			AND t.Cat IN (" + objPropMapData.Category + ") \n");
                    }
                    if (objPropMapData.InvoiceID != 0)
                    {
                        if (objPropMapData.InvoiceID == 1)
                        {
                            sb.Append("			AND ISNULL(t.Invoice,0) <> 0 \n");
                        }
                        else if (objPropMapData.InvoiceID == 2)
                        {
                            sb.Append("			AND ISNULL(t.Invoice,0) = 0 AND ISNULL(t.Charge,0) = 1 \n");
                        }
                    }
                    if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                    {

                        if (objPropMapData.Worker == "Active")
                        {
                            sb.Append("			AND w.Status = 0 \n");
                        }
                        else if (objPropMapData.Worker == "Inactive")
                        {
                            sb.Append("			AND w.Status = 1 \n");
                        }
                        else
                        {
                            sb.Append("			AND w.fDesc = '" + objPropMapData.Worker.Replace("'", "''") + "' \n");
                        }
                    }

                    // Search value
                    if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && !string.IsNullOrEmpty(objPropMapData.SearchValue))
                    {
                        if (objPropMapData.SearchBy == "t.ID")
                        {
                            sb.Append("			AND t.ID = " + objPropMapData.SearchValue + " \n");
                        }
                        if (objPropMapData.SearchBy == "t.cat")
                        {
                            sb.Append("			AND t.Cat LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "t.WorkOrder")
                        {
                            sb.Append("			AND t.WorkOrder LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "t.fdesc")
                        {
                            sb.Append("			AND t.fDesc LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "t.descres")
                        {
                            sb.Append("			AND t.DescRes LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "r.name")
                        {
                            sb.Append("			AND rou.Name LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.tag")
                        {
                            sb.Append("			AND l.Tag LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.ldesc4")
                        {
                            sb.Append("			AND l.Address LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.City")
                        {
                            sb.Append("			AND l.City LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.state")
                        {
                            sb.Append("			AND l.State LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.Zip")
                        {
                            sb.Append("			AND l.Zip LIKE '%" + objPropMapData.SearchValue + "%' n");
                        }
                    }

                    //Ticket filters
                    foreach (var filter in filters)
                    {
                        if (filter.FilterColumn == "ID")
                        {
                            sb.Append("			AND t.ID IN (" + filter.FilterValue + ") \n");
                        }
                        if (filter.FilterColumn == "WorkOrder")
                        {
                            sb.Append("			AND t.Workorder= '" + filter.FilterValue + "' \n");
                        }
                        if (filter.FilterColumn == "invoiceno")
                        {
                            sb.Append("			AND t.Invoice = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "Job")
                        {
                            sb.Append("			AND t.Job = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "locname")
                        {
                            sb.Append("			AND l.Tag LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "City")
                        {
                            sb.Append("			AND l.City LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "fullAddress")
                        {
                            sb.Append("			AND l.Address LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "dwork")
                        {
                            sb.Append("			AND w.fDesc LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Name")
                        {
                            sb.Append("			AND rou.Name LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "cat")
                        {
                            sb.Append("			AND t.Cat LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Tottime")
                        {
                            sb.Append("			AND t.Total = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "timediff")
                        {
                            sb.Append(@"		AND ROUND(CONVERT(NUMERIC(30, 2), (ISNULL(t.Total, 0.00) - ( CONVERT(FLOAT, DATEDIFF(MILLISECOND, t.TimeRoute, 
                                                (CASE WHEN(CAST(t.TimeSite AS TIME) < CAST(t.TimeRoute AS TIME) AND CAST(t.TimeComp AS TIME) < CAST(t.TimeSite AS TIME)) THEN DATEADD(DAY, 2, t.TimeComp)

                                                    ELSE((CASE
                                                        WHEN(CAST(t.TimeSite AS TIME) < CAST(t.TimeRoute AS TIME)
                                                            OR CAST(t.TimeComp AS TIME) < CAST(t.TimeSite AS TIME)) THEN DATEADD(DAY, 1, t.TimeComp)
                                                        ELSE t.TimeComp
                                                    END))
                                                END))) / 1000 / 60 / 60 ) )), 1) = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "department")
                        {
                            sb.Append("			AND jt.Type LIKE '%" + filter.FilterValue + "%' \n");
                        }
                    }

                    #endregion
                }

                sb.Append(") temp \n");
                sb.Append("WHERE 1 = 1 \n");

                if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && !string.IsNullOrEmpty(objPropMapData.SearchValue) && objPropMapData.SearchBy == "e.unit")
                {
                    sb.Append("	AND Unit LIKE '%" + objPropMapData.SearchValue + "%' \n");
                }

                var unitFilter = filters.FirstOrDefault(x => x.FilterColumn == "unit");
                if (unitFilter != null)
                {
                    sb.Append("			AND Unit LIKE '%" + unitFilter.FilterValue + "%' \n");
                }

                if (objPropMapData.Voided == 1)
                {
                    sb.Append("			AND temp.Assignname = 'Voided' \n");
                }

                if (objPropMapData.Assigned == 4 && objPropMapData.Voided != 1)
                {
                    sb.Append("			AND temp.Assignname <> 'Voided' \n");
                }

                sb.Append("ORDER BY temp.[ID] DESC \n");

                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCompletedTicketWarning(MapData objPropMapData, List<RetainFilter> filters, bool isNewCall, string installationFilter)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("WITH #Temp AS (  \n");
                if (objPropMapData.Assigned == 4 || objPropMapData.Assigned == -1)
                {
                    #region TicketD table

                    sb.Append("SELECT  \n");
                    sb.Append("	t.[ID] \n");
                    sb.Append("	,t.[CDate] \n");
                    sb.Append("	,t.[DDate] \n");
                    sb.Append("	,t.[EDate] \n");
                    sb.Append("	,t.[fWork] \n");
                    sb.Append("	,t.[Job] \n");
                    sb.Append("	,t.[Loc] \n");
                    sb.Append("	,l.[ID] AS LocID \n");
                    sb.Append("	,t.[Type] \n");
                    sb.Append("	,t.[fDesc] \n");
                    sb.Append("	,t.[DescRes] \n");
                    sb.Append("	,t.[Total] \n");
                    sb.Append("	,t.[Reg] \n");
                    sb.Append("	,t.[OT] \n");
                    sb.Append("	,t.[DT] \n");
                    sb.Append("	,t.[TT] \n");
                    sb.Append("	,t.[Zone] \n");
                    sb.Append("	,t.[Toll] \n");
                    sb.Append("	,t.[OtherE] \n");
                    sb.Append("	,t.[Status] \n");
                    sb.Append("	,t.[Invoice] \n");
                    sb.Append("	,t.[Level] \n");
                    sb.Append("	,t.[Est] \n");
                    sb.Append("	,t.[Cat] \n");
                    sb.Append("	,t.[Who] \n");
                    sb.Append("	,t.[fBy] \n");
                    sb.Append("	,t.[fLong] \n");
                    sb.Append("	,t.[Latt] \n");
                    sb.Append("	,t.[WageC] \n");
                    sb.Append("	,t.[Phase] \n");
                    sb.Append("	,t.[Car] \n");
                    sb.Append("	,t.[CallIn] \n");
                    sb.Append("	,t.[Mileage] \n");
                    sb.Append("	,t.[NT] \n");
                    sb.Append("	,t.[CauseID] \n");
                    sb.Append("	,t.[CauseDesc] \n");
                    sb.Append("	,t.[fGroup] \n");
                    sb.Append("	,t.[PriceL] \n");
                    sb.Append("	,t.[WorkOrder] \n");
                    sb.Append("	,t.[TimeRoute] \n");
                    sb.Append("	,t.[TimeSite] \n");
                    sb.Append("	,t.[TimeComp] \n");
                    sb.Append("	,t.[Charge] \n");
                    sb.Append("	,jt.[Type] AS JobType \n");
                    sb.Append("	,m.[fDesc] AS Mech \n");
                    sb.Append("	,l.[Tag] \n");
                    sb.Append("	,l.[Address]  \n");
                    sb.Append("	,l.[City] \n");
                    sb.Append("	,l.[State] \n");
                    sb.Append("	,l.[Zip] \n");
                    sb.Append("	,e.[ID] AS Elev \n");
                    sb.Append("	,e.[Unit] \n");
                    sb.Append("	,e.[Install] \n");
                    sb.Append("	,rou.[Name] AS Route \n");
                    sb.Append("	,w.[fDesc] AS dWork \n");
                    sb.Append("	,te.Name AS Salesperson \n");
                    sb.Append("	,CASE t.Assigned WHEN 6 THEN 'Voided' ELSE 'Completed' END AS Assignname \n");
                    sb.Append("FROM TicketD t \n");
                    sb.Append("	INNER JOIN Loc l ON l.Loc = t.Loc  	 \n");
                    sb.Append("	LEFT JOIN Route rou ON rou.ID = l.Route \n");
                    sb.Append("	LEFT JOIN Job j ON j.ID = t.Job \n");
                    sb.Append("	LEFT JOIN JobType jt ON jt.ID = j.Type  \n");
                    sb.Append("	LEFT JOIN tblWork m ON m.ID = rou.Mech \n");
                    sb.Append("	LEFT JOIN tblWork w ON w.ID = t.fWork \n");
                    sb.Append("	INNER JOIN multiple_equipments mul ON t.ID = mul.ticket_id \n");
                    sb.Append("	INNER JOIN Elev e ON e.ID = mul.elev_id \n");
                    sb.Append("	LEFT JOIN Terr te on te.ID = l.Terr  \n");
                    sb.Append(" WHERE t.EDate >= '" + objPropMapData.StartDate + "' AND t.EDate <= '" + objPropMapData.EndDate + "'  \n");

                    if (isNewCall)
                    {
                        sb.Append(" AND e.Install IS NOT NULL \n");

                        if (!string.IsNullOrEmpty(installationFilter))
                        {
                            sb.Append(" AND e.Install " + installationFilter + "  \n");
                        }
                    }

                    // If get from from Edit Location screen
                    if (objPropMapData.LocID > 0)
                    {
                        sb.Append("			AND t.Loc = " + objPropMapData.LocID + " \n");
                    }

                    // If get from from Edit Customer screen
                    if (objPropMapData.CustID > 0)
                    {
                        sb.Append("			AND l.Owner = " + objPropMapData.CustID + " \n");
                    }

                    // Advanced Search
                    if (!string.IsNullOrEmpty(objPropMapData.Supervisor))
                    {
                        sb.Append("			AND m.Super ='" + objPropMapData.Supervisor + "' \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.FilterCharge))
                    {
                        sb.Append("			AND (ISNULL(t.Charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge));

                        if (objPropMapData.FilterCharge == "1")
                        {
                            sb.Append(" OR ISNULL(t.Invoice,0) <> 0) \n");
                        }
                        else
                        {
                            sb.Append(" AND ISNULL(t.Invoice,0) = 0) \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.FilterReview))
                    {
                        sb.Append("			AND ISNULL(t.ClearCheck,0)= " + Convert.ToInt32(objPropMapData.FilterReview) + " \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Workorder))
                    {
                        sb.Append("			AND t.Workorder= '" + objPropMapData.Workorder + "' \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Route))
                    {
                        if (Convert.ToInt32(objPropMapData.Route) == 0)
                        {
                            sb.Append("			AND l.Route = 0 \n");
                        }
                        else
                        {
                            sb.Append("			AND rou.ID = " + Convert.ToInt32(objPropMapData.Route) + " \n");
                        }
                    }
                    if (objPropMapData.Department >= 0)
                    {
                        sb.Append("			AND t.type = " + objPropMapData.Department + " \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.IsPortal))
                    {
                        if (objPropMapData.IsPortal == "1")
                        {
                            sb.Append("			AND t.fBy= 'portal' \n");
                        }
                        if (objPropMapData.IsPortal == "0")
                        {
                            sb.Append("			AND t.fBy <> 'portal' \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Bremarks))
                    {
                        if (objPropMapData.Bremarks == "1")
                        {
                            sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') <> '' \n");
                        }
                        else
                        {
                            sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') = '' \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Timesheet))
                    {
                        sb.Append("			AND ISNULL(t.TransferTime,0) = " + Convert.ToInt32(objPropMapData.Timesheet) + " \n");
                    }
                    if (objPropMapData.Mobile > 0)
                    {
                        if (objPropMapData.Mobile == 2)
                        {
                            sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 2 \n");
                        }
                        if (objPropMapData.Mobile == 1)
                        {
                            sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 0 \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Category))
                    {
                        sb.Append("			AND t.Cat IN (" + objPropMapData.Category + ") \n");
                    }
                    if (objPropMapData.InvoiceID != 0)
                    {
                        if (objPropMapData.InvoiceID == 1)
                        {
                            sb.Append("			AND ISNULL(t.Invoice,0) <> 0 \n");
                        }
                        else if (objPropMapData.InvoiceID == 2)
                        {
                            sb.Append("			AND ISNULL(t.Invoice,0) = 0 AND ISNULL(t.Charge,0) = 1 \n");
                        }
                    }
                    if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                    {

                        if (objPropMapData.Worker == "Active")
                        {
                            sb.Append("			AND w.Status = 0 \n");
                        }
                        else if (objPropMapData.Worker == "Inactive")
                        {
                            sb.Append("			AND w.Status = 1 \n");
                        }
                        else
                        {
                            sb.Append("			AND w.fDesc = '" + objPropMapData.Worker.Replace("'", "''") + "' \n");
                        }
                    }

                    // Search value
                    if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && !string.IsNullOrEmpty(objPropMapData.SearchValue))
                    {
                        if (objPropMapData.SearchBy == "t.ID")
                        {
                            sb.Append("			AND t.ID = " + objPropMapData.SearchValue + " \n");
                        }
                        if (objPropMapData.SearchBy == "t.cat")
                        {
                            sb.Append("			AND t.Cat LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "t.WorkOrder")
                        {
                            sb.Append("			AND t.WorkOrder LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "t.fdesc")
                        {
                            sb.Append("			AND t.fDesc LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "t.descres")
                        {
                            sb.Append("			AND t.DescRes LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "e.unit")
                        {
                            sb.Append("			AND e.Unit LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "r.name")
                        {
                            sb.Append("			AND rou.Name LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.tag")
                        {
                            sb.Append("			AND l.Tag LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.ldesc4")
                        {
                            sb.Append("			AND l.Address LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.City")
                        {
                            sb.Append("			AND l.City LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.state")
                        {
                            sb.Append("			AND l.State LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.Zip")
                        {
                            sb.Append("			AND l.Zip LIKE '%" + objPropMapData.SearchValue + "%' n");
                        }
                    }

                    //Ticket filters
                    foreach (var filter in filters)
                    {
                        if (filter.FilterColumn == "ID")
                        {
                            sb.Append("			AND t.ID IN (" + filter.FilterValue + ") \n");
                        }
                        if (filter.FilterColumn == "WorkOrder")
                        {
                            sb.Append("			AND t.Workorder= '" + filter.FilterValue + "' \n");
                        }
                        if (filter.FilterColumn == "unit")
                        {
                            sb.Append("			AND e.Unit LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "invoiceno")
                        {
                            sb.Append("			AND t.Invoice = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "Job")
                        {
                            sb.Append("			AND t.Job = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "locname")
                        {
                            sb.Append("			AND l.Tag LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "City")
                        {
                            sb.Append("			AND l.City LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "fullAddress")
                        {
                            sb.Append("			AND l.Address LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "dwork")
                        {
                            sb.Append("			AND w.fDesc LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Name")
                        {
                            sb.Append("			AND rou.Name LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "cat")
                        {
                            sb.Append("			AND t.Cat LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Tottime")
                        {
                            sb.Append("			AND t.Total = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "timediff")
                        {
                            sb.Append(@"		AND ROUND(CONVERT(NUMERIC(30, 2), (ISNULL(t.Total, 0.00) - ( CONVERT(FLOAT, DATEDIFF(MILLISECOND, t.TimeRoute, 
                                                (CASE WHEN(CAST(t.TimeSite AS TIME) < CAST(t.TimeRoute AS TIME) AND CAST(t.TimeComp AS TIME) < CAST(t.TimeSite AS TIME)) THEN DATEADD(DAY, 2, t.TimeComp)

                                                    ELSE((CASE
                                                        WHEN(CAST(t.TimeSite AS TIME) < CAST(t.TimeRoute AS TIME)
                                                            OR CAST(t.TimeComp AS TIME) < CAST(t.TimeSite AS TIME)) THEN DATEADD(DAY, 1, t.TimeComp)
                                                        ELSE t.TimeComp
                                                    END))
                                                END))) / 1000 / 60 / 60 ) )), 1) = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "department")
                        {
                            sb.Append("			AND jt.Type LIKE '%" + filter.FilterValue + "%' \n");
                        }
                    }

                    #endregion
                }

                if (objPropMapData.FilterReview != "1")
                {
                    #region TicketO table

                    sb.Append("UNION ALL  \n");
                    sb.Append("SELECT   \n");
                    sb.Append("	t.[ID]  \n");
                    sb.Append("	,t.[CDate]  \n");
                    sb.Append("	,t.[DDate]  \n");
                    sb.Append("	,t.[EDate]  \n");
                    sb.Append("	,t.[fWork]  \n");
                    sb.Append("	,t.[Job]  \n");
                    sb.Append("	,t.[LID]  AS Loc  \n");
                    sb.Append("	,l.[ID] AS LocID \n");
                    sb.Append("	,t.[Type]  \n");
                    sb.Append("	,t.[fDesc]  \n");
                    sb.Append("	,dp.[DescRes]  \n");
                    sb.Append("	,dp.[Total]  \n");
                    sb.Append("	,dp.[Reg]  \n");
                    sb.Append("	,dp.[OT]  \n");
                    sb.Append("	,dp.[DT]  \n");
                    sb.Append("	,dp.[TT]  \n");
                    sb.Append("	,dp.[Zone]  \n");
                    sb.Append("	,dp.[Toll]  \n");
                    sb.Append("	,dp.[OtherE]  \n");
                    sb.Append("	,dp.[Status]  \n");
                    sb.Append("	,dp.[Invoice]  \n");
                    sb.Append("	,t.[Level]  \n");
                    sb.Append("	,t.[Est]  \n");
                    sb.Append("	,t.[Cat]  \n");
                    sb.Append("	,t.[Who]  \n");
                    sb.Append("	,t.[fBy]  \n");
                    sb.Append("	,t.[fLong]  \n");
                    sb.Append("	,t.[Latt]  \n");
                    sb.Append("	,dp.[WageC]  \n");
                    sb.Append("	,dp.[Phase]  \n");
                    sb.Append("	,dp.[Car]  \n");
                    sb.Append("	,t.[CallIn]  \n");
                    sb.Append("	,dp.[Mileage]  \n");
                    sb.Append("	,dp.[NT]  \n");
                    sb.Append("	,dp.[CauseID]  \n");
                    sb.Append("	,dp.[CauseDesc]  \n");
                    sb.Append("	,t.[fGroup]  \n");
                    sb.Append("	,t.[PriceL]  \n");
                    sb.Append("	,t.[WorkOrder]  \n");
                    sb.Append("	,t.[TimeRoute]  \n");
                    sb.Append("	,t.[TimeSite]  \n");
                    sb.Append("	,t.[TimeComp]  \n");
                    sb.Append("	,dp.[Charge]  \n");
                    sb.Append("	,jt.[Type] AS JobType  \n");
                    sb.Append("	,m.[fDesc] AS Mech  \n");
                    sb.Append("	,l.[Tag]  \n");
                    sb.Append("	,l.[Address]   \n");
                    sb.Append("	,l.[City]  \n");
                    sb.Append("	,l.[State]  \n");
                    sb.Append("	,l.[Zip]  \n");
                    sb.Append("	,e.[ID] AS Elev \n");
                    sb.Append("	,e.[Unit]  \n");
                    sb.Append("	,e.[Install]  \n");
                    sb.Append("	,rou.[Name] AS Route  \n");
                    sb.Append("	,t.[DWork] AS dWork \n");
                    sb.Append("	,te.Name AS Salesperson \n");
                    sb.Append("	,CASE \n");
                    sb.Append("	    WHEN t.Assigned = 0 THEN 'Un-Assigned' \n");
                    sb.Append("		WHEN t.Assigned = 1 THEN 'Assigned' \n");
                    sb.Append("		WHEN t.Assigned = 2 THEN 'Enroute' \n");
                    sb.Append("		WHEN t.Assigned = 3 THEN 'Onsite' \n");
                    sb.Append("		WHEN t.Assigned = 4 THEN 'Completedd' \n");
                    sb.Append("		WHEN t.Assigned = 5 THEN 'Hold' \n");
                    sb.Append("		WHEN t.Assigned = 6 THEN 'Voided' \n");
                    sb.Append("	END AS Assignname \n");
                    sb.Append("FROM TicketO t  \n");
                    sb.Append("	LEFT OUTER JOIN TicketDPDA dp ON t.ID = dp.ID \n");
                    sb.Append("	INNER JOIN Loc l ON l.Loc = t.LID  	  \n");
                    sb.Append("	LEFT JOIN Route rou ON rou.ID = l.Route  \n");
                    sb.Append("	LEFT JOIN Job j ON j.ID = t.Job  \n");
                    sb.Append("	LEFT JOIN JobType jt ON jt.ID = j.Type   \n");
                    sb.Append("	LEFT JOIN tblWork m ON m.ID = rou.Mech  \n");
                    sb.Append("	LEFT JOIN tblWork w ON w.ID = t.fWork \n");
                    sb.Append("	INNER JOIN multiple_equipments mul WITH(NOLOCK) ON t.ID = mul.ticket_id  \n");
                    sb.Append("	INNER JOIN Elev e ON e.ID = mul.elev_id  \n");
                    sb.Append("	LEFT JOIN Terr te on te.ID = l.Terr  \n");
                    sb.Append(" WHERE t.Assigned = 4 \n");
                    sb.Append("   AND t.EDate >= '" + objPropMapData.StartDate + "' AND t.EDate <= '" + objPropMapData.EndDate + "'  \n");

                    if (isNewCall)
                    {
                        sb.Append(" AND e.Install IS NOT NULL \n");

                        if (!string.IsNullOrEmpty(installationFilter))
                        {
                            sb.Append(" AND e.Install " + installationFilter + "  \n");
                        }
                    }

                    // If get from from Edit Location screen
                    if (objPropMapData.LocID > 0)
                    {
                        sb.Append("			AND t.LID = " + objPropMapData.LocID + " \n");
                    }

                    // If get from from Edit Customer screen
                    if (objPropMapData.CustID > 0)
                    {
                        sb.Append("			AND l.Owner = " + objPropMapData.CustID + " \n");
                    }

                    //Advanced Search
                    if (!string.IsNullOrEmpty(objPropMapData.Supervisor))
                    {
                        sb.Append("			AND m.Super ='" + objPropMapData.Supervisor + "' \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.FilterCharge))
                    {
                        sb.Append("			AND (ISNULL(dp.Charge,0)= " + Convert.ToInt32(objPropMapData.FilterCharge));

                        if (objPropMapData.FilterCharge == "1")
                        {
                            sb.Append(" OR ISNULL(dp.Invoice,0) <> 0) \n");
                        }
                        else
                        {
                            sb.Append(" AND ISNULL(dp.Invoice,0) = 0) \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.FilterReview))
                    {
                        sb.Append("			AND ISNULL(dp.ClearCheck,0)= " + Convert.ToInt32(objPropMapData.FilterReview) + " \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Workorder))
                    {
                        sb.Append("			AND t.Workorder= '" + objPropMapData.Workorder + "' \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Route))
                    {
                        if (Convert.ToInt32(objPropMapData.Route) == 0)
                        {
                            sb.Append("			AND l.Route = 0 \n");
                        }
                        else
                        {
                            sb.Append("			AND rou.ID = " + Convert.ToInt32(objPropMapData.Route) + " \n");
                        }
                    }
                    if (objPropMapData.Department >= 0)
                    {
                        sb.Append("			AND t.type = " + objPropMapData.Department + " \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.IsPortal))
                    {
                        if (objPropMapData.IsPortal == "1")
                        {
                            sb.Append("			AND t.fBy= 'portal' \n");
                        }
                        if (objPropMapData.IsPortal == "0")
                        {
                            sb.Append("			AND t.fBy <> 'portal' \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Bremarks))
                    {
                        if (objPropMapData.Bremarks == "1")
                        {
                            sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') <> '' \n");
                        }
                        else
                        {
                            sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') = '' \n");
                        }
                    }
                    if (objPropMapData.Mobile > 0)
                    {
                        if (objPropMapData.Mobile == 2)
                        {
                            sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 2 \n");
                        }
                        if (objPropMapData.Mobile == 1)
                        {
                            sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 0 \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Category))
                    {
                        sb.Append("			AND t.Cat IN (" + objPropMapData.Category + ") \n");
                    }
                    if (objPropMapData.InvoiceID != 0)
                    {
                        if (objPropMapData.InvoiceID == 1)
                        {
                            sb.Append("			AND ISNULL(dp.Invoice,0) <> 0 \n");
                        }
                        else if (objPropMapData.InvoiceID == 2)
                        {
                            sb.Append("			AND ISNULL(dp.Invoice,0) = 0 AND ISNULL(dp.Charge,0) = 1 \n");
                        }
                    }
                    if (objPropMapData.Assigned != -1)
                    {
                        if (objPropMapData.Assigned == -2)
                        {
                            sb.Append("			AND t.Assigned <> 4 \n");
                        }
                        else
                        {
                            sb.Append("			AND t.Assigned = " + objPropMapData.Assigned + "  \n");
                        }
                    }
                    if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                    {

                        if (objPropMapData.Worker == "Active")
                        {
                            sb.Append("			AND w.Status = 0 \n");
                        }
                        else if (objPropMapData.Worker == "Inactive")
                        {
                            sb.Append("			AND w.Status = 1 \n");
                        }
                        else
                        {
                            sb.Append("			AND w.fDesc = '" + objPropMapData.Worker.Replace("'", "''") + "' \n");
                        }
                    }

                    // Search value
                    if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && !string.IsNullOrEmpty(objPropMapData.SearchValue))
                    {
                        if (objPropMapData.SearchBy == "t.ID")
                        {
                            sb.Append("			AND t.ID = " + objPropMapData.SearchValue + " \n");
                        }
                        if (objPropMapData.SearchBy == "t.cat")
                        {
                            sb.Append("			AND t.Cat LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "t.WorkOrder")
                        {
                            sb.Append("			AND t.WorkOrder LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "t.fdesc")
                        {
                            sb.Append("			AND t.fDesc LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "t.descres")
                        {
                            sb.Append("			AND dp.DescRes LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "e.unit")
                        {
                            sb.Append("			AND e.Unit LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "r.name")
                        {
                            sb.Append("			AND rou.Name LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.tag")
                        {
                            sb.Append("			AND l.Tag LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.ldesc4")
                        {
                            sb.Append("			AND l.Address LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.City")
                        {
                            sb.Append("			AND l.City LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.state")
                        {
                            sb.Append("			AND l.State LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.Zip")
                        {
                            sb.Append("			AND l.Zip LIKE '%" + objPropMapData.SearchValue + "%'		 \n");
                        }
                    }

                    //Ticket filters
                    foreach (var filter in filters)
                    {
                        if (filter.FilterColumn == "ID")
                        {
                            sb.Append("			AND t.ID IN (" + filter.FilterValue + ") \n");
                        }
                        if (filter.FilterColumn == "WorkOrder")
                        {
                            sb.Append("			AND t.Workorder= '" + filter.FilterValue + "' \n");
                        }
                        if (filter.FilterColumn == "unit")
                        {
                            sb.Append("			AND e.Unit LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "invoiceno")
                        {
                            sb.Append("			AND dp.Invoice = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "Job")
                        {
                            sb.Append("			AND t.Job = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "locname")
                        {
                            sb.Append("			AND l.Tag LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "City")
                        {
                            sb.Append("			AND l.City LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "fullAddress")
                        {
                            sb.Append("			AND l.Address LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "dwork")
                        {
                            sb.Append("			AND t.DWork LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Name")
                        {
                            sb.Append("			AND rou.Name LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "cat")
                        {
                            sb.Append("			AND t.Cat LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Tottime")
                        {
                            sb.Append("			AND dp.Total = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "timediff")
                        {
                            sb.Append(@"		AND ROUND(CONVERT(NUMERIC(30, 2), (ISNULL(dp.Total, 0.00) - ( CONVERT(FLOAT, DATEDIFF(MILLISECOND, dp.TimeRoute, 
			                                    (CASE WHEN(CAST(dp.TimeSite AS TIME) < CAST(dp.TimeRoute AS TIME) AND CAST(dp.TimeComp AS TIME) < CAST(dp.TimeSite AS TIME)) THEN DATEADD(DAY, 2, dp.TimeComp)
				                                    ELSE((CASE
                                                        WHEN(CAST(dp.TimeSite AS TIME) < CAST(dp.TimeRoute AS TIME)
                                                            OR CAST(dp.TimeComp AS TIME) < CAST(dp.TimeSite AS TIME)) THEN DATEADD(DAY, 1, dp.TimeComp)
                                                        ELSE dp.TimeComp
                                                    END))
                                                END ))) / 1000 / 60 / 60 ) )), 1) = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "department")
                        {
                            sb.Append("			AND jt.Type LIKE '%" + filter.FilterValue + "%' \n");
                        }
                    }

                    #endregion
                }

                sb.Append(")  \n");
                sb.Append("SELECT * FROM #Temp WHERE Elev IN (SELECT Elev FROM #Temp GROUP BY Elev HAVING COUNT(Elev) > 2)  \n");

                if (objPropMapData.Voided == 1)
                {
                    sb.Append("	AND Assignname = 'Voided' \n");
                }

                if (objPropMapData.Assigned == 4 && objPropMapData.Voided != 1)
                {
                    sb.Append("	AND Assignname <> 'Voided' \n");
                }

                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTicketListForReport(MapData objPropMapData, List<RetainFilter> filters)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT * FROM( \n");
                if (objPropMapData.FilterReview != "1")
                {
                    #region TicketO table

                    sb.Append("	SELECT  \n");
                    sb.Append("			t.[ID] \n");
                    sb.Append("			,t.[CDate] \n");
                    sb.Append("			,t.[DDate] \n");
                    sb.Append("			,t.[EDate] \n");
                    sb.Append("			,t.[fWork] \n");
                    sb.Append("			,t.[Job] \n");
                    sb.Append("			,t.[LID] AS Loc \n");
                    sb.Append("			,t.[LElev] AS Elev \n");
                    sb.Append("			,t.[Type] \n");
                    sb.Append("			,t.[fDesc] \n");
                    sb.Append("			,UPPER(t.[DWork]) AS DWork \n");
                    sb.Append("			,dp.[DescRes] \n");
                    sb.Append("			,dp.[Total] \n");
                    sb.Append("			,dp.[Reg] \n");
                    sb.Append("			,dp.[OT] \n");
                    sb.Append("			,dp.[DT] \n");
                    sb.Append("			,dp.[TT] \n");
                    sb.Append("			,dp.[Zone] \n");
                    sb.Append("			,dp.[Toll] \n");
                    sb.Append("			,dp.[OtherE] \n");
                    sb.Append("			,dp.[Status] \n");
                    sb.Append("			,dp.[Invoice] \n");
                    sb.Append("			,t.[Level] \n");
                    sb.Append("			,t.[Est] \n");
                    sb.Append("			,t.[Cat] \n");
                    sb.Append("			,t.[Who] \n");
                    sb.Append("			,t.[fBy] \n");
                    sb.Append("			,t.[fLong] \n");
                    sb.Append("			,t.[Latt] \n");
                    sb.Append("			,dp.[WageC] \n");
                    sb.Append("			,dp.[Phase] \n");
                    sb.Append("			,dp.[Car] \n");
                    sb.Append("			,dp.[CallIn] \n");
                    sb.Append("			,dp.[Mileage] \n");
                    sb.Append("			,dp.[NT] \n");
                    sb.Append("			,dp.[CauseID] \n");
                    sb.Append("			,dp.[CauseDesc] \n");
                    sb.Append("			,t.[fGroup] \n");
                    sb.Append("			,t.[PriceL] \n");
                    sb.Append("			,t.[WorkOrder] \n");
                    sb.Append("			,t.[TimeRoute] \n");
                    sb.Append("			,t.[TimeSite] \n");
                    sb.Append("			,t.[TimeComp] \n");
                    sb.Append("			,jt.[Type] AS JobType \n");
                    sb.Append("			,ww.[fDesc] AS Mech \n");
                    sb.Append("			,l.[Tag] \n");
                    sb.Append("			,l.[Address]  \n");
                    sb.Append("			,l.[City] \n");
                    sb.Append("			,l.[State] \n");
                    sb.Append("			,l.[Zip] \n");
                    sb.Append("			,CASE \n");
                    sb.Append("			    WHEN t.Assigned = 0 THEN 'Un-Assigned' \n");
                    sb.Append("			    WHEN t.Assigned = 1 THEN 'Assigned' \n");
                    sb.Append("			    WHEN t.Assigned = 2 THEN 'Enroute' \n");
                    sb.Append("			    WHEN t.Assigned = 3 THEN 'Onsite' \n");
                    sb.Append("			    WHEN t.Assigned = 4 THEN 'Completedd' \n");
                    sb.Append("			    WHEN t.Assigned = 5 THEN 'Hold' \n");
                    sb.Append("			    WHEN t.Assigned = 6 THEN 'Voided' \n");
                    sb.Append("			END AS Assignname \n");
                    sb.Append("         ,(SELECT STUFF((SELECT ', ' + CAST(e.Unit AS VARCHAR(1000))  \n");
                    sb.Append("         FROM Elev e WHERE e.id IN  \n");
                    sb.Append("             (SELECT me.elev_id FROM multiple_equipments me WHERE me.ticket_id = t.ID) \n");
                    sb.Append("             FOR XML PATH(''), Type).value('.', 'VARCHAR(1000)'),1,2,' ')) AS Unit \n");
                    sb.Append("         ,(SELECT TOP 1 Signature FROM   PDATicketSignature WHERE  PDATicketID = t.ID) AS Signature \n");
                    sb.Append("		FROM TicketO t \n");
                    sb.Append("			LEFT OUTER JOIN TicketDPDA dp ON t.ID = dp.ID \n");
                    sb.Append("			INNER JOIN Loc l ON l.Loc = t.LID  	 \n");
                    sb.Append("			LEFT JOIN Route rou ON rou.ID = l.Route \n");
                    sb.Append("			LEFT JOIN Job j ON j.ID = t.Job \n");
                    sb.Append("			LEFT JOIN JobType jt ON jt.ID = j.Type  \n");
                    sb.Append("			LEFT JOIN tblWork m ON m.ID = rou.Mech \n");
                    sb.Append("			LEFT JOIN  tblWork ww ON ww.ID = t.fWork \n");
                    sb.Append("			LEFT JOIN Elev e ON e.ID = t.LElev \n");
                    sb.Append("		WHERE t.EDate >= '" + objPropMapData.StartDate + "' AND t.EDate <= '" + objPropMapData.EndDate + "' \n");

                    // If get from from Edit Location screen
                    if (objPropMapData.LocID > 0)
                    {
                        sb.Append("			AND t.LID = " + objPropMapData.LocID + " \n");
                    }

                    // If get from from Edit Customer screen
                    if (objPropMapData.CustID > 0)
                    {
                        sb.Append("			AND l.Owner = " + objPropMapData.CustID + " \n");
                    }

                    //Advanced Search
                    if (!string.IsNullOrEmpty(objPropMapData.Supervisor))
                    {
                        sb.Append("			AND m.Super ='" + objPropMapData.Supervisor + "' \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.FilterCharge))
                    {
                        sb.Append("			AND (ISNULL(dp.Charge,0)= " + Convert.ToInt32(objPropMapData.FilterCharge));

                        if (objPropMapData.FilterCharge == "1")
                        {
                            sb.Append(" OR ISNULL(dp.Invoice,0) <> 0) \n");
                        }
                        else
                        {
                            sb.Append(" AND ISNULL(dp.Invoice,0) = 0) \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.FilterReview))
                    {
                        sb.Append("			AND ISNULL(dp.ClearCheck,0)= " + Convert.ToInt32(objPropMapData.FilterReview) + " \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Workorder))
                    {
                        sb.Append("			AND t.Workorder= '" + objPropMapData.Workorder + "' \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Route))
                    {
                        if (Convert.ToInt32(objPropMapData.Route) == 0)
                        {
                            sb.Append("			AND l.Route = 0 \n");
                        }
                        else
                        {
                            sb.Append("			AND rou.ID = " + Convert.ToInt32(objPropMapData.Route) + " \n");
                        }
                    }
                    if (objPropMapData.Department >= 0)
                    {
                        sb.Append("			AND t.type = " + objPropMapData.Department + " \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.IsPortal))
                    {
                        if (objPropMapData.IsPortal == "1")
                        {
                            sb.Append("			AND t.fBy= 'portal' \n");
                        }
                        if (objPropMapData.IsPortal == "0")
                        {
                            sb.Append("			AND t.fBy <> 'portal' \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Bremarks))
                    {
                        if (objPropMapData.Bremarks == "1")
                        {
                            sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') <> '' \n");
                        }
                        else
                        {
                            sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') = '' \n");
                        }
                    }
                    if (objPropMapData.Mobile > 0)
                    {
                        if (objPropMapData.Mobile == 2)
                        {
                            sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 2 \n");
                        }
                        if (objPropMapData.Mobile == 1)
                        {
                            sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 0 \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Category))
                    {
                        sb.Append("			AND t.Cat IN (" + objPropMapData.Category + ") \n");
                    }
                    if (objPropMapData.InvoiceID != 0)
                    {
                        if (objPropMapData.InvoiceID == 1)
                        {
                            sb.Append("			AND ISNULL(dp.Invoice,0) <> 0 \n");
                        }
                        else if (objPropMapData.InvoiceID == 2)
                        {
                            sb.Append("			AND ISNULL(dp.Invoice,0) = 0 AND ISNULL(dp.Charge,0) = 1 \n");
                        }
                    }
                    if (objPropMapData.Assigned != -1)
                    {
                        if (objPropMapData.Assigned == -2)
                        {
                            sb.Append("			AND t.Assigned <> 4 \n");
                        }
                        else
                        {
                            sb.Append("			AND t.Assigned = " + objPropMapData.Assigned + "  \n");
                        }
                    }
                    if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                    {

                        if (objPropMapData.Worker == "Active")
                        {
                            sb.Append("			AND ww.Status = 0 \n");
                        }
                        else if (objPropMapData.Worker == "Inactive")
                        {
                            sb.Append("			AND ww.Status = 1 \n");
                        }
                        else
                        {
                            sb.Append("			AND ww.fDesc = '" + objPropMapData.Worker.Replace("'", "''") + "' \n");
                        }
                    }

                    // Search value
                    if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && !string.IsNullOrEmpty(objPropMapData.SearchValue))
                    {
                        if (objPropMapData.SearchBy == "t.ID")
                        {
                            sb.Append("			AND t.ID = " + objPropMapData.SearchValue + " \n");
                        }
                        if (objPropMapData.SearchBy == "t.cat")
                        {
                            sb.Append("			AND t.Cat LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "t.WorkOrder")
                        {
                            sb.Append("			AND t.WorkOrder LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "t.fdesc")
                        {
                            sb.Append("			AND t.fDesc LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "t.descres")
                        {
                            sb.Append("			AND dp.DescRes LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "r.name")
                        {
                            sb.Append("			AND rou.Name LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.tag")
                        {
                            sb.Append("			AND l.Tag LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.ldesc4")
                        {
                            sb.Append("			AND l.Address LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.City")
                        {
                            sb.Append("			AND l.City LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.state")
                        {
                            sb.Append("			AND l.State LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.Zip")
                        {
                            sb.Append("			AND l.Zip LIKE '%" + objPropMapData.SearchValue + "%'		 \n");
                        }
                    }

                    //Ticket filters
                    foreach (var filter in filters)
                    {
                        if (filter.FilterColumn == "ID")
                        {
                            sb.Append("			AND t.ID IN (" + filter.FilterValue + ") \n");
                        }
                        if (filter.FilterColumn == "WorkOrder")
                        {
                            sb.Append("			AND t.Workorder= '" + filter.FilterValue + "' \n");
                        }
                        if (filter.FilterColumn == "invoiceno")
                        {
                            sb.Append("			AND dp.Invoice = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "Job")
                        {
                            sb.Append("			AND t.Job = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "locname")
                        {
                            sb.Append("			AND l.Tag LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "City")
                        {
                            sb.Append("			AND l.City LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "fullAddress")
                        {
                            sb.Append("			AND l.Address LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "dwork")
                        {
                            sb.Append("			AND t.DWork LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Name")
                        {
                            sb.Append("			AND rou.Name LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "cat")
                        {
                            sb.Append("			AND t.Cat LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Tottime")
                        {
                            sb.Append("			AND dp.Total = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "timediff")
                        {
                            sb.Append(@"		AND ROUND(CONVERT(NUMERIC(30, 2), (ISNULL(dp.Total, 0.00) - ( CONVERT(FLOAT, DATEDIFF(MILLISECOND, dp.TimeRoute, 
			                                    (CASE WHEN(CAST(dp.TimeSite AS TIME) < CAST(dp.TimeRoute AS TIME) AND CAST(dp.TimeComp AS TIME) < CAST(dp.TimeSite AS TIME)) THEN DATEADD(DAY, 2, dp.TimeComp)
				                                    ELSE((CASE
                                                        WHEN(CAST(dp.TimeSite AS TIME) < CAST(dp.TimeRoute AS TIME)
                                                            OR CAST(dp.TimeComp AS TIME) < CAST(dp.TimeSite AS TIME)) THEN DATEADD(DAY, 1, dp.TimeComp)
                                                        ELSE dp.TimeComp
                                                    END))
                                                END ))) / 1000 / 60 / 60 ) )), 1) = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "department")
                        {
                            sb.Append("			AND jt.Type LIKE '%" + filter.FilterValue + "%' \n");
                        }
                    }

                    #endregion
                }

                if (objPropMapData.Assigned == 4 || objPropMapData.Assigned == -1)
                {
                    #region TicketD table

                    sb.Append("		UNION ALL \n");
                    sb.Append("		SELECT  \n");
                    sb.Append("			t.[ID] \n");
                    sb.Append("			,t.[CDate] \n");
                    sb.Append("			,t.[DDate] \n");
                    sb.Append("			,t.[EDate] \n");
                    sb.Append("			,t.[fWork] \n");
                    sb.Append("			,t.[Job] \n");
                    sb.Append("			,t.[Loc] \n");
                    sb.Append("			,t.[Elev] \n");
                    sb.Append("			,t.[Type] \n");
                    sb.Append("			,t.[fDesc] \n");
                    sb.Append("			,UPPER(w.[fDesc]) AS DWork \n");
                    sb.Append("			,t.[DescRes] \n");
                    sb.Append("			,t.[Total] \n");
                    sb.Append("			,t.[Reg] \n");
                    sb.Append("			,t.[OT] \n");
                    sb.Append("			,t.[DT] \n");
                    sb.Append("			,t.[TT] \n");
                    sb.Append("			,t.[Zone] \n");
                    sb.Append("			,t.[Toll] \n");
                    sb.Append("			,t.[OtherE] \n");
                    sb.Append("			,t.[Status] \n");
                    sb.Append("			,t.[Invoice] \n");
                    sb.Append("			,t.[Level] \n");
                    sb.Append("			,t.[Est] \n");
                    sb.Append("			,t.[Cat] \n");
                    sb.Append("			,t.[Who] \n");
                    sb.Append("			,t.[fBy] \n");
                    sb.Append("			,t.[fLong] \n");
                    sb.Append("			,t.[Latt] \n");
                    sb.Append("			,t.[WageC] \n");
                    sb.Append("			,t.[Phase] \n");
                    sb.Append("			,t.[Car] \n");
                    sb.Append("			,t.[CallIn] \n");
                    sb.Append("			,t.[Mileage] \n");
                    sb.Append("			,t.[NT] \n");
                    sb.Append("			,t.[CauseID] \n");
                    sb.Append("			,t.[CauseDesc] \n");
                    sb.Append("			,t.[fGroup] \n");
                    sb.Append("			,t.[PriceL] \n");
                    sb.Append("			,t.[WorkOrder] \n");
                    sb.Append("			,t.[TimeRoute] \n");
                    sb.Append("			,t.[TimeSite] \n");
                    sb.Append("			,t.[TimeComp] \n");
                    sb.Append("			,jt.[Type] AS JobType \n");
                    sb.Append("			,w.[fDesc] AS Mech \n");
                    sb.Append("			,l.[Tag] \n");
                    sb.Append("			,l.[Address]  \n");
                    sb.Append("			,l.[City] \n");
                    sb.Append("			,l.[State] \n");
                    sb.Append("			,l.[Zip] \n");
                    sb.Append("			,CASE t.Assigned WHEN 6 THEN 'Voided' ELSE 'Completed' END AS Assignname \n");
                    sb.Append("         ,(SELECT STUFF((SELECT ', ' + CAST(e.Unit AS VARCHAR(1000))  \n");
                    sb.Append("         FROM Elev e WHERE e.id IN  \n");
                    sb.Append("             (SELECT me.elev_id FROM multiple_equipments me WHERE me.ticket_id = t.ID) \n");
                    sb.Append("             FOR XML PATH(''), Type).value('.', 'VARCHAR(1000)'),1,2,' ')) AS Unit \n");
                    sb.Append("         ,(SELECT TOP 1 Signature FROM   PDATicketSignature WHERE  PDATicketID = t.ID) AS Signature \n");
                    sb.Append("		FROM TicketD t \n");
                    sb.Append("			INNER JOIN Loc l ON l.Loc = t.Loc  	 \n");
                    sb.Append("			LEFT JOIN Route rou ON rou.ID = l.Route \n");
                    sb.Append("			LEFT JOIN Job j ON j.ID = t.Job \n");
                    sb.Append("			LEFT JOIN JobType jt ON jt.ID = j.Type  \n");
                    sb.Append("			LEFT JOIN tblWork m ON m.ID = rou.Mech \n");
                    sb.Append("			LEFT JOIN tblWork w ON w.ID = t.fWork \n");
                    sb.Append("			LEFT JOIN Elev e ON e.ID = t.Elev \n");
                    sb.Append("		WHERE t.EDate >= '" + objPropMapData.StartDate + "' AND t.EDate <= '" + objPropMapData.EndDate + "'  \n");

                    // If get from from Edit Location screen
                    if (objPropMapData.LocID > 0)
                    {
                        sb.Append("			AND t.Loc = " + objPropMapData.LocID + " \n");
                    }

                    // If get from from Edit Customer screen
                    if (objPropMapData.CustID > 0)
                    {
                        sb.Append("			AND l.Owner = " + objPropMapData.CustID + " \n");
                    }

                    // Advanced Search
                    if (!string.IsNullOrEmpty(objPropMapData.Supervisor))
                    {
                        sb.Append("			AND m.Super ='" + objPropMapData.Supervisor + "' \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.FilterCharge))
                    {
                        sb.Append("			AND (ISNULL(t.Charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge));

                        if (objPropMapData.FilterCharge == "1")
                        {
                            sb.Append(" OR ISNULL(t.Invoice,0) <> 0) \n");
                        }
                        else
                        {
                            sb.Append(" AND ISNULL(t.Invoice,0) = 0) \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.FilterReview))
                    {
                        sb.Append("			AND ISNULL(t.ClearCheck,0)= " + Convert.ToInt32(objPropMapData.FilterReview) + " \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Workorder))
                    {
                        sb.Append("			AND t.Workorder= '" + objPropMapData.Workorder + "' \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Route))
                    {
                        if (Convert.ToInt32(objPropMapData.Route) == 0)
                        {
                            sb.Append("			AND l.Route = 0 \n");
                        }
                        else
                        {
                            sb.Append("			AND rou.ID = " + Convert.ToInt32(objPropMapData.Route) + " \n");
                        }
                    }
                    if (objPropMapData.Department >= 0)
                    {
                        sb.Append("			AND t.type = " + objPropMapData.Department + " \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.IsPortal))
                    {
                        if (objPropMapData.IsPortal == "1")
                        {
                            sb.Append("			AND t.fBy= 'portal' \n");
                        }
                        if (objPropMapData.IsPortal == "0")
                        {
                            sb.Append("			AND t.fBy <> 'portal' \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Bremarks))
                    {
                        if (objPropMapData.Bremarks == "1")
                        {
                            sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') <> '' \n");
                        }
                        else
                        {
                            sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') = '' \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Timesheet))
                    {
                        sb.Append("			AND ISNULL(t.TransferTime,0) = " + Convert.ToInt32(objPropMapData.Timesheet) + " \n");
                    }
                    if (objPropMapData.Mobile > 0)
                    {
                        if (objPropMapData.Mobile == 2)
                        {
                            sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 2 \n");
                        }
                        if (objPropMapData.Mobile == 1)
                        {
                            sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 0 \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Category))
                    {
                        sb.Append("			AND t.Cat IN (" + objPropMapData.Category + ") \n");
                    }
                    if (objPropMapData.InvoiceID != 0)
                    {
                        if (objPropMapData.InvoiceID == 1)
                        {
                            sb.Append("			AND ISNULL(t.Invoice,0) <> 0 \n");
                        }
                        else if (objPropMapData.InvoiceID == 2)
                        {
                            sb.Append("			AND ISNULL(t.Invoice,0) = 0 AND ISNULL(t.Charge,0) = 1 \n");
                        }
                    }
                    if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                    {

                        if (objPropMapData.Worker == "Active")
                        {
                            sb.Append("			AND w.Status = 0 \n");
                        }
                        else if (objPropMapData.Worker == "Inactive")
                        {
                            sb.Append("			AND w.Status = 1 \n");
                        }
                        else
                        {
                            sb.Append("			AND w.fDesc = '" + objPropMapData.Worker.Replace("'", "''") + "' \n");
                        }
                    }

                    // Search value
                    if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && !string.IsNullOrEmpty(objPropMapData.SearchValue))
                    {
                        if (objPropMapData.SearchBy == "t.ID")
                        {
                            sb.Append("			AND t.ID = " + objPropMapData.SearchValue + " \n");
                        }
                        if (objPropMapData.SearchBy == "t.cat")
                        {
                            sb.Append("			AND t.Cat LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "t.WorkOrder")
                        {
                            sb.Append("			AND t.WorkOrder LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "t.fdesc")
                        {
                            sb.Append("			AND t.fDesc LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "t.descres")
                        {
                            sb.Append("			AND t.DescRes LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "r.name")
                        {
                            sb.Append("			AND rou.Name LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.tag")
                        {
                            sb.Append("			AND l.Tag LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.ldesc4")
                        {
                            sb.Append("			AND l.Address LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.City")
                        {
                            sb.Append("			AND l.City LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.state")
                        {
                            sb.Append("			AND l.State LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.Zip")
                        {
                            sb.Append("			AND l.Zip LIKE '%" + objPropMapData.SearchValue + "%' n");
                        }
                    }

                    //Ticket filters
                    foreach (var filter in filters)
                    {
                        if (filter.FilterColumn == "ID")
                        {
                            sb.Append("			AND t.ID IN (" + filter.FilterValue + ") \n");
                        }
                        if (filter.FilterColumn == "WorkOrder")
                        {
                            sb.Append("			AND t.Workorder= '" + filter.FilterValue + "' \n");
                        }
                        if (filter.FilterColumn == "invoiceno")
                        {
                            sb.Append("			AND t.Invoice = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "Job")
                        {
                            sb.Append("			AND t.Job = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "locname")
                        {
                            sb.Append("			AND l.Tag LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "City")
                        {
                            sb.Append("			AND l.City LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "fullAddress")
                        {
                            sb.Append("			AND l.Address LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "dwork")
                        {
                            sb.Append("			AND w.fDesc LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Name")
                        {
                            sb.Append("			AND rou.Name LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "cat")
                        {
                            sb.Append("			AND t.Cat LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Tottime")
                        {
                            sb.Append("			AND t.Total = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "timediff")
                        {
                            sb.Append(@"		AND ROUND(CONVERT(NUMERIC(30, 2), (ISNULL(t.Total, 0.00) - ( CONVERT(FLOAT, DATEDIFF(MILLISECOND, t.TimeRoute, 
                                                (CASE WHEN(CAST(t.TimeSite AS TIME) < CAST(t.TimeRoute AS TIME) AND CAST(t.TimeComp AS TIME) < CAST(t.TimeSite AS TIME)) THEN DATEADD(DAY, 2, t.TimeComp)

                                                    ELSE((CASE
                                                        WHEN(CAST(t.TimeSite AS TIME) < CAST(t.TimeRoute AS TIME)
                                                            OR CAST(t.TimeComp AS TIME) < CAST(t.TimeSite AS TIME)) THEN DATEADD(DAY, 1, t.TimeComp)
                                                        ELSE t.TimeComp
                                                    END))
                                                END))) / 1000 / 60 / 60 ) )), 1) = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "department")
                        {
                            sb.Append("			AND jt.Type LIKE '%" + filter.FilterValue + "%' \n");
                        }
                    }

                    #endregion
                }

                sb.Append(") temp \n");

                sb.Append("WHERE 1 = 1 \n");

                if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && !string.IsNullOrEmpty(objPropMapData.SearchValue) && objPropMapData.SearchBy == "e.unit")
                {
                    sb.Append("	AND Unit LIKE '%" + objPropMapData.SearchValue + "%' \n");
                }

                var unitFilter = filters.FirstOrDefault(x => x.FilterColumn == "unit");
                if (unitFilter != null)
                {
                    sb.Append("			AND Unit LIKE '%" + unitFilter.FilterValue + "%' \n");
                }

                if (objPropMapData.Voided == 1)
                {
                    sb.Append("			AND temp.Assignname = 'Voided' \n");
                }

                if (objPropMapData.Assigned == 4 && objPropMapData.Voided != 1)
                {
                    sb.Append("			AND temp.Assignname <> 'Voided' \n");
                }

                sb.Append("ORDER BY temp.[ID] DESC \n");

                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetMaintenanceUnitCount(Chart objChart, string routes)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("WITH #temp AS( \n");
                sb.Append("	SELECT  \n");
                sb.Append("		l.Loc \n");
                sb.Append("		,l.Tag  \n");
                sb.Append("		,l.Type AS LocType \n");
                sb.Append("		,l.Address \n");
                sb.Append("		,l.Route AS RouteID \n");
                sb.Append("		,ISNULL(r.Name, 'Unassigned') AS Route \n");
                sb.Append("		,ISNULL(m.fDesc, 'Unassigned') AS Mech \n");
                sb.Append("		,e.Type AS UnitType \n");
                sb.Append("		,e.Cat \n");
                sb.Append("		,COUNT(*) AS UnitCount \n");
                sb.Append("		,LEFT(r.Name, 6) AS Area \n");
                sb.Append("		,(SELECT STUFF((SELECT ', ' + CAST(c.Job AS VARCHAR(1000)) \n");
                sb.Append("			FROM Contract c WHERE c.Loc = l.Loc \n");
                sb.Append("			FOR XML PATH(''), Type).value('.', 'VARCHAR(1000)'),1,2,' ')) AS Jobs \n");
                sb.Append("	FROM Loc l \n");
                sb.Append("	LEFT JOIN Route r ON r.ID = l.Route \n");
                sb.Append("	LEFT JOIN tblWork m ON m.ID = r.Mech \n");
                sb.Append("	INNER JOIN Rol rl ON rl.ID = l.Rol \n");
                sb.Append("	INNER JOIN Elev e ON l.Loc = e.Loc \n");
                sb.Append("	WHERE l.Status = 0 AND l.Maint = 1 \n");
                sb.Append("		AND e.Status = 0 AND e.Unit NOT LIKE '%N/C%' COLLATE SQL_Latin1_General_CP1_CS_AS AND e.Unit NOT LIKE '%NC%' COLLATE SQL_Latin1_General_CP1_CS_AS \n");
                sb.Append("		AND ((r.Name <> '00-T/M' AND r.Name NOT LIKE '%Delete%') OR r.Name IS NULL) \n");

                if (!string.IsNullOrEmpty(routes) && routes != "all")
                {
                    if (routes.Contains("Unassigned"))
                    {
                        sb.Append(string.Format("AND (r.Name IS NULL OR r.Name IN ({0})) \n", routes));
                    }
                    else
                    {
                        sb.Append(string.Format("AND r.Name IN ({0}) \n", routes));
                    }
                }

                sb.Append("	GROUP BY l.Loc, l.Tag, l.Type, l.Address, l.Route, m.fDesc, e.Type, e.Cat, r.Name \n");
                sb.Append(") \n");
                sb.Append("SELECT  \n");
                sb.Append("	t.* \n");
                sb.Append("	,(SELECT SUM(UnitCount) FROM #temp) AS TotalCount \n");
                sb.Append("	,(SELECT SUM(UnitCount) FROM #temp WHERE Area = t.Area) AS AreaCount \n");
                sb.Append("	,(SELECT SUM(UnitCount) FROM #temp WHERE RouteID = t.RouteID) AS RouteCount \n");
                sb.Append("	,(SELECT SUM(UnitCount) FROM #temp WHERE Loc = t.Loc) AS LocCount \n");
                sb.Append("	,(SELECT SUM(UnitCount) FROM #temp WHERE RouteID = t.RouteID AND Cat = t.Cat AND UnitType = t. UnitType) AS RouteCatTypeCount \n");
                sb.Append("	,(SELECT SUM(UnitCount) FROM #temp WHERE Area = t.Area AND Cat = t.Cat AND UnitType = t. UnitType) AS AreaCatTypeCount \n");
                sb.Append("	,(SELECT SUM(UnitCount) FROM #temp WHERE Cat = t.Cat AND UnitType = t. UnitType) AS TotalCatTypeCount \n");
                sb.Append("FROM #temp t	 \n");

                if (!string.IsNullOrEmpty(routes) && routes != "all")
                {
                    sb.Append(string.Format("WHERE t.Route IN ({0}) \n", routes));
                }

                return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Maintenance Equipment Count By Start Date
        public DataSet GetMaintenanceUnitCountByDate(Contracts objPropContracts, List<RetainFilter> filters)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                //sb.Append("SELECT	ct.BStart AS [Billing Start Date],ct.SStart AS [Scheduled Start Date],ct.Job AS [Contract #],r.Name AS [Customer Name],	l.Tag AS [Location Name],l.Address AS [Location Address],e.Unit AS [Equiment Name],ISNULL(e.State, '') AS [Unique #],ISNULL(e.fDesc, '') AS [Description],ISNULL(e.Type, '') AS [Type],ISNULL(e.Category, '') AS [Category],ISNULL(e.Cat, '') AS [Svc. type],ISNULL(e.Building, '') AS [Building],(CASE e.Status WHEN 0 THEN 'Active' ELSE 'Inactive' END) AS Status,(CASE ISNULL(e.shut_down, 0) WHEN 0 THEN 'No' ELSE 'Yes' END) AS [Shut Down],ISNULL(el.Price, 0) AS Price,ISNULL(el.Hours, 0) AS Hours \n");
                //sb.Append("Round (CASE ct.BCycle \n");
                //sb.Append("WHEN 0 THEN ct.BAmt    --Monthly; \n");
                //sb.Append("WHEN 1 THEN ct.BAmt / 2  --Bi-Monthly; \n");
                //sb.Append("WHEN 2 THEN ct.BAmt / 3 --Quarterly; \n");
                //sb.Append("WHEN 3 THEN ct.BAmt / 4 --3 Times/Year; \n");
                //sb.Append("WHEN 4 THEN ct.BAmt / 6 --Semi-Annually; \n");
                //sb.Append("WHEN 5 THEN ct.BAmt / 12 --Annually; \n");
                //sb.Append("WHEN 6 THEN   0         --'Never'; \n");
                //sb.Append("WHEN 7 THEN ct.BAmt / (12*3)   --'3 Years'; \n");
                //sb.Append("WHEN 8 THEN ct.BAmt / (12*5)    --'5 Years'; \n");
                //sb.Append("WHEN 9 THEN ct.BAmt / (12*2)    --'2 Years'; \n");
                //sb.Append("else 0  \n");
                //sb.Append("END, 2) AS MonthlyBill  \n");
                //sb.Append("FROM Contract ct INNER JOIN tblJoinElevJob el ON el.Job = ct.Job INNER JOIN Elev e ON e.ID = el.Elev INNER JOIN Loc l ON l.Loc = ct.Loc	INNER JOIN Owner o ON o.ID = ct.Owner INNER JOIN Rol r on r.ID = o.Rol where ct.SStart is not null");
                sb.Append("SELECT \n");
                sb.Append("ct.BStart AS [Billing Start Date], \n");
                sb.Append("ct.SStart AS [Scheduled Start Date],\n");
                sb.Append("ct.Job AS [Contract #],\n");
                sb.Append("r.Name AS [Customer Name],\n");
                sb.Append("r.State AS [State],\n");
                sb.Append("l.Tag AS [Location Name], \n");
                sb.Append("l.Address AS [Location Address], \n");
                sb.Append("e.Unit AS [Equiment Name], \n");
                sb.Append("ISNULL(e.State, '') AS [Unique #], \n");
                sb.Append("ISNULL(e.fDesc, '') AS [Description], \n");
                sb.Append("ISNULL(e.Type, '') AS [Type], \n");
                sb.Append("ISNULL(e.Category, '') AS [Category], \n");
                sb.Append("ISNULL(e.Cat, '') AS [Svc. type], \n");
                sb.Append("ISNULL(e.Building, '') AS [Building], \n");
                sb.Append("(CASE e.Status WHEN 0 THEN 'Active' ELSE 'Inactive' END) AS Status, \n");
                sb.Append("(CASE ISNULL(e.shut_down, 0) WHEN 0 THEN 'No' ELSE 'Yes' END) AS [Shut Down], \n");
                sb.Append("ISNULL(el.Price, 0) AS Price, \n");
                sb.Append("ISNULL(el.Hours, 0) AS Hours, \n");
                sb.Append("Round (CASE ct.BCycle \n");
                sb.Append("WHEN 0 THEN ct.BAmt    --Monthly; \n");
                sb.Append("WHEN 1 THEN ct.BAmt / 2  --Bi-Monthly; \n");
                sb.Append("WHEN 2 THEN ct.BAmt / 3 --Quarterly; \n");
                sb.Append("WHEN 3 THEN ct.BAmt / 4 --3 Times/Year; \n");
                sb.Append("WHEN 4 THEN ct.BAmt / 6 --Semi-Annually; \n");
                sb.Append("WHEN 5 THEN ct.BAmt / 12 --Annually; \n");
                sb.Append("WHEN 6 THEN   0         --'Never'; \n");
                sb.Append("WHEN 7 THEN ct.BAmt / (12*3)   --'3 Years'; \n");
                sb.Append("WHEN 8 THEN ct.BAmt / (12*5)    --'5 Years'; \n");
                sb.Append("WHEN 9 THEN ct.BAmt / (12*2)    --'2 Years'; \n");
                sb.Append("else 0  \n");
                sb.Append("END, 2) AS MonthlyBill, \n");
                sb.Append("Round (CASE ct.SCycle  \n");
                sb.Append("WHEN 0 THEN ct.Hours --Monthly \n");
                sb.Append("WHEN 1 THEN ct.Hours / 2 --Bi-Monthly \n");
                sb.Append("WHEN 2 THEN ct.Hours / 3 --Quarterly \n");
                sb.Append("WHEN 3 THEN ct.Hours / 6 --Semi-Anually \n");
                sb.Append("WHEN 4 THEN ct.Hours / 12 --Anually \n");
                sb.Append("WHEN 5 THEN (ct.Hours * 4.3) --Weekly \n");
                sb.Append("WHEN 6 THEN (ct.Hours * (2.15))  --Bi-Weekly \n");
                sb.Append("WHEN 7 THEN ( ct.Hours / ( 2.9898 ) ) --Every 13 Weeks \n");
                sb.Append("WHEN 10 THEN ct.Hours / 12*2 --Every 2 Years \n");
                sb.Append("WHEN 8 THEN ct.Hours / 12*3 --Every 3 Years \n");
                sb.Append("WHEN 9 THEN ct.Hours / 12*5 --Every 5 Years \n");
                sb.Append("WHEN 11 THEN ct.Hours / 12*7 --Every 7 Years \n");
                sb.Append("WHEN 13 THEN (ct.Hours * ( CASE ct.SWE WHEN 1 THEN 30 ELSE   21.66 END) ) --Daily \n");
                sb.Append("WHEN 14 THEN (ct.Hours * (2) ) --Twice a Month \n");
                sb.Append("WHEN 15 THEN (ct.Hours / (4) ) --3 Times/Year \n");
                sb.Append("else 0 \n");
                sb.Append("END, 2) AS MonthlyHours, \n");
                sb.Append("CASE ct.bcycle \n");
                sb.Append("WHEN 0 THEN 'Monthly' \n");
                sb.Append("WHEN 1 THEN 'Bi-Monthly' \n");
                sb.Append("WHEN 2 THEN 'Quarterly' \n");
                sb.Append("WHEN 3 THEN '3 Times/Year' \n");
                sb.Append("WHEN 4 THEN 'Semi-Annually' \n");
                sb.Append("WHEN 5 THEN 'Annually' \n");
                sb.Append("WHEN 6 THEN 'Never' \n");
                sb.Append("WHEN 7 THEN '3 Years' \n");
                sb.Append("WHEN 8 THEN '5 Years' \n");
                sb.Append("WHEN 9 THEN '2 Years' \n");
                sb.Append("END Freqency, \n");
                sb.Append("CASE ct.scycle \n");
                sb.Append("WHEN -1 THEN 'Never' \n");
                sb.Append("WHEN 0 THEN 'Monthly' \n");
                sb.Append("WHEN 1 THEN 'Bi-Monthly' \n");
                sb.Append("WHEN 2 THEN 'Quarterly' \n");
                sb.Append("WHEN 3 THEN 'Semi-Annually' \n");
                sb.Append("WHEN 4 THEN 'Annually' \n");
                sb.Append("WHEN 5 THEN 'Weekly' \n");
                sb.Append("WHEN 6 THEN 'Bi-Weekly' \n");
                sb.Append("WHEN 7 THEN 'Every 13 Weeks' \n");
                sb.Append("WHEN 10 THEN 'Every 2 Years' \n");
                sb.Append("WHEN 8 THEN 'Every 3 Years' \n");
                sb.Append("WHEN 9 THEN 'Every 5 Years' \n");
                sb.Append("WHEN 11 THEN 'Every 7 Years' \n");
                sb.Append("WHEN 12 THEN 'On-Demand' \n");
                sb.Append("WHEN 13 THEN 'Daily' \n");
                sb.Append("WHEN 14 THEN 'Twice a Month' \n");
                sb.Append("WHEN 15 THEN '3 Times/Year' \n");
                sb.Append("END TicketFreq, \n");
                sb.Append("CASE ct.Status \n");
                sb.Append("WHEN 0 THEN 'Active' \n");
                sb.Append("WHEN 1 THEN 'Closed' \n");
                sb.Append("WHEN 2 THEN 'Hold' \n");
                sb.Append("WHEN 3 THEN 'Completed' \n");
                sb.Append("END Status, \n");
                sb.Append("CASE \n");
                sb.Append("WHEN l.route > 0 THEN \n");
                sb.Append("( select   (select( case  when ro.Name IS NULL  then ''   when tblwork.fdesc is null then  ro.Name    when tblwork.fdesc = ro.Name then ro.Name  else ro.Name +' - ' + tblwork.fdesc   end)from tblwork where tblwork.id = ro.mech   ) FROM Route ro where ro.ID =    l.route) \n");
                sb.Append("ELSE 'Unassigned' \n");
                sb.Append("END  AS Worker, \n");
                sb.Append("t.Name AS Salesperson, \n");
                sb.Append("t2.Name AS Salesperson2, \n");
                sb.Append("J.SREMARKS AS SREMARKS \n");
                sb.Append("FROM Contract ct \n");
                sb.Append("INNER JOIN tblJoinElevJob el ON el.Job = ct.Job \n");
                sb.Append("INNER JOIN Elev e ON e.ID = el.Elev \n");
                sb.Append("INNER JOIN Loc l ON l.Loc = ct.Loc \n");
                sb.Append("INNER JOIN Owner o ON o.ID = ct.Owner \n");
                sb.Append("INNER JOIN Rol r on r.ID = o.Rol  \n");
                sb.Append("INNER JOIN Terr t ON t.ID = l.Terr  \n");
                sb.Append("INNER JOIN Terr t2 ON t2.ID = t.ID \n");
                sb.Append("INNER JOIN Job J ON J.ID = Ct.Job  \n");
                sb.Append("where ct.SStart is not null \n");
                if (objPropContracts.SearchBy != null && objPropContracts.SearchValue != null)
                {
                    if (objPropContracts.SearchBy == "j.ctype")
                    {
                        objPropContracts.SearchBy = "e.Cat";
                    }
                    if (objPropContracts.SearchBy == "r.name" || objPropContracts.SearchBy == "l.tag" || objPropContracts.SearchBy == "r.State")
                    {
                        sb.Append("and " + objPropContracts.SearchBy + "  like '%" + objPropContracts.SearchValue + "%' ");
                    }
                    else
                    {
                        sb.Append("and " + objPropContracts.SearchBy + "='" + objPropContracts.SearchValue + "' ");
                    }

                }
                if (objPropContracts.SearchBy != null || objPropContracts.SearchBy != "" && objPropContracts.SearchValue != null || objPropContracts.SearchValue != "" && filters.Count != 0)
                {
                    foreach (var obj in filters)
                    {
                        if (obj.FilterColumn == "Job")
                        {
                            sb.Append("\n and ct.Job ='" + obj.FilterValue.Trim() + "' ");
                        }
                        if (obj.FilterColumn == "locid")
                        {
                            sb.Append("\n and l.ID like '%" + obj.FilterValue.Trim() + "%' ");
                        }
                        if (obj.FilterColumn == "Tag")
                        {
                            sb.Append("\n and l.Tag like '%" + obj.FilterValue.Trim() + "%' ");
                        }
                        if (obj.FilterColumn == "Name")
                        {
                            sb.Append("\n and r.Name like '%" + obj.FilterValue.Trim() + "%' ");
                        }
                        if (obj.FilterColumn == "CType")
                        {
                            sb.Append("\n and e.Cat like '%" + obj.FilterValue.Trim() + "%' ");
                        }
                        if (obj.FilterColumn == "fdesc")
                        {
                            sb.Append("\n and e.fDesc like '%" + obj.FilterValue.Trim() + "%' ");
                        }
                        if (obj.FilterColumn == "BAmt")
                        {
                            sb.Append("\n and ct.BAmt ='" + obj.FilterValue.Trim() + "'");
                        }
                        if (obj.FilterColumn == "Hours")
                        {
                            sb.Append("\n and ct.Hours ='" + obj.FilterValue.Trim() + "'");
                        }
                        if (obj.FilterColumn == "SREMARKS")
                        {
                            sb.Append("\n and J.SREMARKS like '%" + obj.FilterValue.Trim() + "%' ");
                        }
                        if (obj.FilterColumn == "Salesperson")
                        {
                            sb.Append("\n and t.Name like '%" + obj.FilterValue.Trim() + "%' ");
                        }
                        if (obj.FilterColumn == "Salesperson2")
                        {
                            sb.Append("\n and t2.Name like '%" + obj.FilterValue.Trim() + "%' ");
                        }
                        if (obj.FilterColumn == "MonthlyBill")
                        {
                            sb.Append("AND Round (CASE ct.BCycle WHEN 0 THEN ct.BAmt--Monthly; \n ");
                            sb.Append("WHEN 1 THEN ct.BAmt / 2--Bi - Monthly; \n");
                            sb.Append("WHEN 2 THEN ct.BAmt / 3--Quarterly; \n");
                            sb.Append(" WHEN 3 THEN ct.BAmt / 4--3 Times / Year; \n");
                            sb.Append("WHEN 4 THEN ct.BAmt / 6--Semi - Annually; \n");
                            sb.Append("WHEN 5 THEN ct.BAmt / 12--Annually; \n");
                            sb.Append("WHEN 6 THEN 0--'Never'; \n");
                            sb.Append("WHEN 7 THEN ct.BAmt / (12 * 3)--'3 Years'; \n");
                            sb.Append("WHEN 8 THEN ct.BAmt / (12 * 5)--'5 Years'; \n");
                            sb.Append("WHEN 9 THEN ct.BAmt / (12 * 2)--'2 Years'; \n");
                            sb.Append("else 0 END, 2) \n");
                            sb.Append("=" + obj.FilterValue + "\n");
                        }
                        if (obj.FilterColumn == "Freqency")
                        {
                            sb.Append("AND CASE ct.bcycle WHEN 0 THEN 'Monthly' \n");
                            sb.Append("WHEN 1 THEN 'Bi-Monthly' \n");
                            sb.Append("WHEN 2 THEN 'Quarterly' \n");
                            sb.Append("WHEN 3 THEN '3 Times/Year' \n");
                            sb.Append("WHEN 4 THEN 'Semi-Annually' \n");
                            sb.Append("WHEN 5 THEN 'Annually' \n");
                            sb.Append("WHEN 6 THEN 'Never' \n");
                            sb.Append("WHEN 7 THEN '3 Years' \n");
                            sb.Append("WHEN 8 THEN '5 Years' \n");
                            sb.Append("WHEN 9 THEN '2 Years' \n");
                            sb.Append("END like '%");
                            sb.Append(obj.FilterValue.Trim() + "%' \n");
                        }
                        if (obj.FilterColumn == "MonthlyHours")
                        {
                            sb.Append("AND Round (CASE ct.SCycle \n");
                            sb.Append("WHEN 0 THEN ct.Hours --Monthly \n");
                            sb.Append("WHEN 1 THEN ct.Hours / 2 --Bi-Monthly \n");
                            sb.Append("WHEN 2 THEN ct.Hours / 3 --Quarterly \n");
                            sb.Append("WHEN 3 THEN ct.Hours / 6 --Semi-Anually \n");
                            sb.Append("WHEN 4 THEN ct.Hours / 12 --Anually \n");
                            sb.Append("WHEN 5 THEN (ct.Hours * 4.3) --Weekly \n");
                            sb.Append("WHEN 6 THEN (ct.Hours * (2.15))  --Bi-Weekly \n");
                            sb.Append("WHEN 7 THEN ( ct.Hours / ( 2.9898 ) ) --Every 13 Weeks \n");
                            sb.Append("WHEN 10 THEN ct.Hours / 12*2 --Every 2 Years \n");
                            sb.Append("WHEN 8 THEN ct.Hours / 12*3 --Every 3 Years \n");
                            sb.Append("WHEN 9 THEN ct.Hours / 12*5 --Every 5 Years \n");
                            sb.Append("WHEN 11 THEN ct.Hours / 12*7 --Every 7 Years \n");
                            sb.Append("WHEN 13 THEN (ct.Hours * ( CASE ct.SWE WHEN 1 THEN 30 ELSE   21.66 END) ) --Daily \n");
                            sb.Append("WHEN 14 THEN (ct.Hours * (2) ) --Twice a Month \n");
                            sb.Append("WHEN 15 THEN (ct.Hours / (4) ) --3 Times/Year \n");
                            sb.Append("else 0 END, 2 =  \n");
                            sb.Append(obj.FilterValue + "\n");
                        }
                        if (obj.FilterColumn == "TicketFreq")
                        {
                            sb.Append("AND CASE ct.scycle \n");
                            sb.Append("WHEN - 1 THEN 'Never' \n");
                            sb.Append("WHEN 0 THEN 'Monthly' \n");
                            sb.Append("WHEN 1 THEN 'Bi-Monthly' \n");
                            sb.Append("WHEN 2 THEN 'Quarterly' \n");
                            sb.Append("WHEN 3 THEN 'Semi-Annually' \n");
                            sb.Append("WHEN 4 THEN 'Annually' \n");
                            sb.Append("WHEN 5 THEN 'Weekly' \n");
                            sb.Append("WHEN 6 THEN 'Bi-Weekly' \n");
                            sb.Append("WHEN 7 THEN 'Every 13 Weeks' \n");
                            sb.Append("WHEN 10 THEN 'Every 2 Years' \n");
                            sb.Append("WHEN 8 THEN 'Every 3 Years' \n");
                            sb.Append("WHEN 9 THEN 'Every 5 Years' \n");
                            sb.Append("WHEN 11 THEN 'Every 7 Years' \n");
                            sb.Append("WHEN 12 THEN 'On-Demand' \n");
                            sb.Append("WHEN 13 THEN 'Daily' \n");
                            sb.Append("WHEN 14 THEN 'Twice a Month' \n");
                            sb.Append("WHEN 15 THEN '3 Times/Year' \n");
                            sb.Append("END like '%");
                            sb.Append(obj.FilterValue.Trim() + "%' \n");
                        }
                        if (obj.FilterColumn == "Worker")
                        {
                            sb.Append("AND CASE \n");
                            sb.Append("WHEN l.route > 0 THEN \n");
                            sb.Append("(select(select( case  when ro.Name IS NULL  then ''   when tblwork.fdesc is null then  ro.Name    when tblwork.fdesc = ro.Name then ro.Name  else ro.Name + ' - ' + tblwork.fdesc   end)from tblwork where tblwork.id = ro.mech   ) FROM Route ro where ro.ID = l.route) \n");
                            sb.Append("ELSE 'Unassigned' \n");
                            sb.Append("END like '%");
                            sb.Append(obj.FilterValue.Trim() + "%'  \n");
                        }
                        if (obj.FilterColumn == "Status")
                        {
                            sb.Append("CASE ct.Status \n");
                            sb.Append("WHEN 0 THEN 'Active' \n");
                            sb.Append("WHEN 1 THEN 'Closed' \n");
                            sb.Append("WHEN 2 THEN 'Hold' \n");
                            sb.Append("WHEN 3 THEN 'Completed' \n");
                            sb.Append("END like '%");
                            sb.Append(obj.FilterValue.Trim() + "%'  \n");
                        }
                    }
                }
                sb.Append("\n ORDER BY ct.SStart, r.Name, l.Tag, e.Unit");
                return objPropContracts.Ds = SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //API
        public DataSet GetMaintenanceUnitCount(GetMaintenanceUnitCountParam _GetMaintenanceUnitCount, string ConnectionString, string routes)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("WITH #temp AS( \n");
                sb.Append("	SELECT  \n");
                sb.Append("		l.Loc \n");
                sb.Append("		,l.Tag  \n");
                sb.Append("		,l.Type AS LocType \n");
                sb.Append("		,l.Address \n");
                sb.Append("		,l.Route AS RouteID \n");
                sb.Append("		,ISNULL(r.Name, 'Unassigned') AS Route \n");
                sb.Append("		,ISNULL(m.fDesc, 'Unassigned') AS Mech \n");
                sb.Append("		,e.Type AS UnitType \n");
                sb.Append("		,e.Cat \n");
                sb.Append("		,COUNT(*) AS UnitCount \n");
                sb.Append("		,LEFT(r.Name, 6) AS Area \n");
                sb.Append("		,(SELECT STUFF((SELECT ', ' + CAST(c.Job AS VARCHAR(1000)) \n");
                sb.Append("			FROM Contract c WHERE c.Loc = l.Loc \n");
                sb.Append("			FOR XML PATH(''), Type).value('.', 'VARCHAR(1000)'),1,2,' ')) AS Jobs \n");
                sb.Append("	FROM Loc l \n");
                sb.Append("	LEFT JOIN Route r ON r.ID = l.Route \n");
                sb.Append("	LEFT JOIN tblWork m ON m.ID = r.Mech \n");
                sb.Append("	INNER JOIN Rol rl ON rl.ID = l.Rol \n");
                sb.Append("	INNER JOIN Elev e ON l.Loc = e.Loc \n");
                sb.Append("	WHERE l.Status = 0 AND l.Maint = 1 \n");
                sb.Append("		AND e.Status = 0 AND e.Unit NOT LIKE '%N/C%' COLLATE SQL_Latin1_General_CP1_CS_AS AND e.Unit NOT LIKE '%NC%' COLLATE SQL_Latin1_General_CP1_CS_AS \n");
                sb.Append("		AND ((r.Name <> '00-T/M' AND r.Name NOT LIKE '%Delete%') OR r.Name IS NULL) \n");

                if (!string.IsNullOrEmpty(routes) && routes != "all")
                {
                    if (routes.Contains("Unassigned"))
                    {
                        sb.Append(string.Format("AND (r.Name IS NULL OR r.Name IN ({0})) \n", routes));
                    }
                    else
                    {
                        sb.Append(string.Format("AND r.Name IN ({0}) \n", routes));
                    }
                }

                sb.Append("	GROUP BY l.Loc, l.Tag, l.Type, l.Address, l.Route, m.fDesc, e.Type, e.Cat, r.Name \n");
                sb.Append(") \n");
                sb.Append("SELECT  \n");
                sb.Append("	t.* \n");
                sb.Append("	,(SELECT SUM(UnitCount) FROM #temp) AS TotalCount \n");
                sb.Append("	,(SELECT SUM(UnitCount) FROM #temp WHERE Area = t.Area) AS AreaCount \n");
                sb.Append("	,(SELECT SUM(UnitCount) FROM #temp WHERE RouteID = t.RouteID) AS RouteCount \n");
                sb.Append("	,(SELECT SUM(UnitCount) FROM #temp WHERE Loc = t.Loc) AS LocCount \n");
                sb.Append("	,(SELECT SUM(UnitCount) FROM #temp WHERE RouteID = t.RouteID AND Cat = t.Cat AND UnitType = t. UnitType) AS RouteCatTypeCount \n");
                sb.Append("	,(SELECT SUM(UnitCount) FROM #temp WHERE Area = t.Area AND Cat = t.Cat AND UnitType = t. UnitType) AS AreaCatTypeCount \n");
                sb.Append("	,(SELECT SUM(UnitCount) FROM #temp WHERE Cat = t.Cat AND UnitType = t. UnitType) AS TotalCatTypeCount \n");
                sb.Append("FROM #temp t	 \n");

                if (!string.IsNullOrEmpty(routes) && routes != "all")
                {
                    sb.Append(string.Format("WHERE t.Route IN ({0}) \n", routes));
                }

                return _GetMaintenanceUnitCount.Ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetMaintenanceUnitCountRoute(Chart objChart)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT  DISTINCT \n");
                sb.Append("	r.Name AS Area 	 \n");
                sb.Append("FROM Loc l  \n");
                sb.Append("INNER JOIN Route r ON r.ID = l.Route  \n");
                sb.Append("INNER JOIN Rol rl ON rl.ID = l.Rol  \n");
                sb.Append("INNER JOIN Elev e ON l.Loc = e.Loc  \n");
                sb.Append("INNER JOIN tblWork m ON m.ID = r.Mech  \n");
                sb.Append("WHERE l.Status = 0 AND l.Maint = 1 --AND l.Type IN ('FUL', 'O&G', 'LTD')   \n");
                sb.Append("	AND e.Status = 0 AND e.Unit NOT LIKE '%N/C%' COLLATE SQL_Latin1_General_CP1_CS_AS AND e.Unit NOT LIKE '%NC%' COLLATE SQL_Latin1_General_CP1_CS_AS  \n");
                sb.Append("	AND r.Name <> '00-T/M' AND r.Name NOT LIKE '%Delete%' \n");

                return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetMaintenanceUnitCountRoute(GetMaintenanceUnitCountRouteParam _GetMaintenanceUnzitCountRoute, string ConnectionString)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT  DISTINCT \n");
                sb.Append("	r.Name AS Area 	 \n");
                sb.Append("FROM Loc l  \n");
                sb.Append("INNER JOIN Route r ON r.ID = l.Route  \n");
                sb.Append("INNER JOIN Rol rl ON rl.ID = l.Rol  \n");
                sb.Append("INNER JOIN Elev e ON l.Loc = e.Loc  \n");
                sb.Append("INNER JOIN tblWork m ON m.ID = r.Mech  \n");
                sb.Append("WHERE l.Status = 0 AND l.Maint = 1 --AND l.Type IN ('FUL', 'O&G', 'LTD')   \n");
                sb.Append("	AND e.Status = 0 AND e.Unit NOT LIKE '%N/C%' COLLATE SQL_Latin1_General_CP1_CS_AS AND e.Unit NOT LIKE '%NC%' COLLATE SQL_Latin1_General_CP1_CS_AS  \n");
                sb.Append("	AND r.Name <> '00-T/M' AND r.Name NOT LIKE '%Delete%' \n");

                return _GetMaintenanceUnzitCountRoute.Ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetMaintenanceControlPlan(Chart objChart, int elevID)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT  \n");
                sb.Append("	e.ID,  \n");
                sb.Append("	e.Unit, \n");
                sb.Append("	e.Loc, \n");
                sb.Append("	e.Owner, \n");
                sb.Append("	e.Cat, \n");
                sb.Append("	e.Type, \n");
                sb.Append("	e.fDesc, \n");
                sb.Append("	e.Status, \n");
                sb.Append("	l.Tag, \n");
                sb.Append("	l.Address, \n");
                sb.Append("	l.City, \n");
                sb.Append("	l.State, \n");
                sb.Append("	l.Zip, \n");
                sb.Append("	r.Name AS RolName \n");
                sb.Append("FROM Elev e \n");
                sb.Append("	INNER JOIN Loc l ON l.Loc = e.Loc \n");
                sb.Append("	INNER JOIN Owner o ON o.id = l.Owner  \n");
                sb.Append("	INNER JOIN Rol r ON o.rol = r.id \n");
                sb.Append("WHERE e.ID = " + elevID + " \n");

                sb.Append("SELECT  \n");
                sb.Append("	i.ID,  \n");
                sb.Append("	i.EquipT,  \n");
                sb.Append("	i.Code,  \n");
                sb.Append("	i.Section,  \n");
                sb.Append("	i.fDesc,  \n");
                sb.Append("	i.Frequency,  \n");
                sb.Append("	i.LastDate,  \n");
                sb.Append("	i.NextDateDue,  \n");
                sb.Append("	(CASE i.Frequency \n");
                sb.Append("		WHEN 0 THEN 'Daily' \n");
                sb.Append("		WHEN 1 THEN 'Weekly' \n");
                sb.Append("		WHEN 2 THEN 'Bi-Weekly' \n");
                sb.Append("		WHEN 3 THEN 'Monthly' \n");
                sb.Append("		WHEN 4 THEN 'Bi-Monthly' \n");
                sb.Append("		WHEN 5 THEN 'Quarterly' \n");
                sb.Append("		WHEN 6 THEN 'Semi-Annually' \n");
                sb.Append("		WHEN 7 THEN 'Annually' \n");
                sb.Append("		WHEN 8 THEN 'One Time' \n");
                sb.Append("		WHEN 9 THEN '3 Times a Year' \n");
                sb.Append("		WHEN 10 THEN 'Every 2 Year' \n");
                sb.Append("		WHEN 11 THEN 'Every 3 Year' \n");
                sb.Append("		WHEN 12 THEN 'Every 5 Year' \n");
                sb.Append("		WHEN 13 THEN 'Every 7 Year' \n");
                sb.Append("		WHEN 14 THEN 'On-Demand' \n");
                sb.Append("	END) AS FrequencyName, \n");
                sb.Append("	t.fDesc AS EquipDesc \n");
                sb.Append("FROM EquipTItem i  \n");
                sb.Append("	INNER JOIN EquipTemp t ON t.ID = i.EquipT \n");
                sb.Append("WHERE i.Elev = " + elevID + " \n");
                sb.Append("ORDER BY t.fDesc, i.Section, i.Code \n");

                return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetMCPBuildingHistory(Chart objChart, int elevID)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT \n");
                sb.Append("	eti.EquipT, \n");
                sb.Append("    et.fdesc AS EquipDesc\n");
                sb.Append("FROM RepDetail rd \n");
                sb.Append("	INNER JOIN EquipTItem eti ON eti.Elev = rd.Elev and eti.Code = rd.Code \n");
                sb.Append("	INNER JOIN EquipTemp et ON et.ID = eti.EquipT \n");
                sb.Append("WHERE rd.id IS NOT NULL \n");
                sb.Append("	AND  rd.Elev = " + elevID + " \n");
                sb.Append("GROUP BY eti.EquipT, et.fdesc\n");

                sb.Append("SELECT  \n");
                sb.Append("	i.ID,  \n");
                sb.Append("	i.EquipT,  \n");
                sb.Append("	i.Code,  \n");
                sb.Append("	i.Section,  \n");
                sb.Append("	i.fDesc,  \n");
                sb.Append("	i.Frequency,  \n");
                sb.Append("	(CASE i.Frequency \n");
                sb.Append("		WHEN 0 THEN 'Daily' \n");
                sb.Append("		WHEN 1 THEN 'Weekly' \n");
                sb.Append("		WHEN 2 THEN 'Bi-Weekly' \n");
                sb.Append("		WHEN 3 THEN 'Monthly' \n");
                sb.Append("		WHEN 4 THEN 'Bi-Monthly' \n");
                sb.Append("		WHEN 5 THEN 'Quarterly' \n");
                sb.Append("		WHEN 6 THEN 'Semi-Annually' \n");
                sb.Append("		WHEN 7 THEN 'Annually' \n");
                sb.Append("		WHEN 8 THEN 'One Time' \n");
                sb.Append("		WHEN 9 THEN '3 Times a Year' \n");
                sb.Append("		WHEN 10 THEN 'Every 2 Year' \n");
                sb.Append("		WHEN 11 THEN 'Every 3 Year' \n");
                sb.Append("		WHEN 12 THEN 'Every 5 Year' \n");
                sb.Append("		WHEN 13 THEN 'Every 7 Year' \n");
                sb.Append("		WHEN 14 THEN 'On-Demand' \n");
                sb.Append("	END) AS FrequencyName\n");
                sb.Append("FROM EquipTItem i  \n");
                sb.Append("WHERE i.Elev = " + elevID + " \n");
                sb.Append("ORDER BY i.EquipT, i.Code \n");

                sb.Append("SELECT \n");
                sb.Append("	eti.EquipT, \n");
                sb.Append("    rd.Lastdate, \n");
                sb.Append("    rd.NextDateDue, \n");
                sb.Append("    rd.TicketID, \n");
                sb.Append("    rd.Code, \n");
                sb.Append("	   Status \n");
                sb.Append("FROM RepDetail rd \n");
                sb.Append("	INNER JOIN EquipTItem eti ON eti.Elev = rd.Elev and eti.Code = rd.Code WHERE rd.id IS NOT NULL \n");
                sb.Append("	AND  rd.Elev = " + elevID + " \n");
                sb.Append("ORDER BY eti.EquipT, rd.Lastdate \n");

                return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetRecurringMaintenanceHistory(Chart objChart, int locID)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                #region Recurring Maintenance History

                sb.Append("SELECT  \n");
                sb.Append("	t.ID, \n");
                sb.Append("	t.Recurring, \n");
                sb.Append("	'Completed' AS Status, \n");
                sb.Append("	4 AS Assigned, \n");
                sb.Append("	t.fDesc, \n");
                sb.Append("	w.fDesc AS DWork, \n");
                sb.Append("	[dbo].[GetAlreadyCreatedTicketInCSV](t.Job, t.Recurring, " + locID + ", 'Equip') AS Equipments \n");
                sb.Append("FROM TicketD t \n");
                sb.Append("	LEFT OUTER JOIN tblWork w on t.fWork = w.ID \n");
                sb.Append("WHERE t.Loc = " + locID + " \n");
                sb.Append("	AND t.Recurring IS NOT NULL \n");
                if (objChart.StartDate != null && objChart.StartDate != DateTime.MinValue)
                {
                    sb.Append("	AND t.EDate >= '" + objChart.StartDate + "' \n");
                }
                if (objChart.EndDate != null && objChart.EndDate != DateTime.MinValue)
                {
                    sb.Append("	AND t.EDate <= '" + objChart.EndDate + "' \n");
                }

                sb.Append("UNION ALL \n");
                sb.Append("SELECT  \n");
                sb.Append("	t.ID, \n");
                sb.Append("	t.Recurring, \n");
                sb.Append("	CASE \n");
                sb.Append("		WHEN t.Assigned = 1 THEN 'Assigned' \n");
                sb.Append("		WHEN t.Assigned = 2 THEN 'Enroute' \n");
                sb.Append("		WHEN t.Assigned = 3 THEN 'Onsite' \n");
                sb.Append("		WHEN t.Assigned = 4 THEN 'Completed' \n");
                sb.Append("		WHEN t.Assigned = 5 THEN 'Hold' \n");
                sb.Append("    END AS Status, \n");
                sb.Append("	t.Assigned, \n");
                sb.Append("	t.fDesc, \n");
                sb.Append("	t.DWork, \n");
                sb.Append("	[dbo].[GetAlreadyCreatedTicketInCSV](t.Job, t.Recurring, " + locID + ", 'Equip') AS Equipments \n");
                sb.Append("FROM TicketO t \n");
                sb.Append("WHERE t.LID = " + locID + " \n");
                sb.Append("	AND t.Recurring IS NOT NULL \n");
                if (objChart.StartDate != null && objChart.StartDate != DateTime.MinValue)
                {
                    sb.Append("	AND t.EDate >= '" + objChart.StartDate + "' \n");
                }
                if (objChart.EndDate != null && objChart.EndDate != DateTime.MinValue)
                {
                    sb.Append("	AND t.EDate <= '" + objChart.EndDate + "' \n");
                }

                #endregion

                #region Other Ticket Categories History

                sb.Append("SELECT  \n");
                sb.Append("	t.[ID] \n");
                sb.Append("	,t.[CDate] \n");
                sb.Append("	,t.[DDate] \n");
                sb.Append("	,t.[EDate] \n");
                sb.Append("	,t.[fWork] \n");
                sb.Append("	,t.[Job] \n");
                sb.Append("	,t.[LID] AS Loc \n");
                sb.Append("	,t.[LElev] AS Elev \n");
                sb.Append("	,t.[Type] \n");
                sb.Append("	,t.[fDesc] \n");
                sb.Append("	,dp.[DescRes]\n");
                sb.Append("	,dp.[Charge] \n");
                sb.Append("	,dp.[Total] \n");
                sb.Append("	,dp.[Reg] \n");
                sb.Append("	,dp.[OT] \n");
                sb.Append("	,dp.[DT] \n");
                sb.Append("	,dp.[TT] \n");
                sb.Append("	,dp.[Zone] \n");
                sb.Append("	,dp.[Toll] \n");
                sb.Append("	,dp.[OtherE] \n");
                sb.Append("	,CASE \n");
                sb.Append("		WHEN t.Assigned = 1 THEN 'Assigned' \n");
                sb.Append("		WHEN t.Assigned = 2 THEN 'Enroute' \n");
                sb.Append("		WHEN t.Assigned = 3 THEN 'Onsite' \n");
                sb.Append("		WHEN t.Assigned = 4 THEN 'Completed' \n");
                sb.Append("		WHEN t.Assigned = 5 THEN 'Hold' \n");
                sb.Append("   END AS Status \n");
                sb.Append("	,dp.[Invoice] \n");
                sb.Append("	,t.[Level] \n");
                sb.Append("	,t.[Est] \n");
                sb.Append("	,t.[Cat] \n");
                sb.Append("	,t.[Who] \n");
                sb.Append("	,t.[fBy]  \n");
                sb.Append("	,dp.[WageC] \n");
                sb.Append("	,dp.[Phase] \n");
                sb.Append("	,dp.[Car] \n");
                sb.Append("	,dp.[CallIn] \n");
                sb.Append("	,t.[CPhone] \n");
                sb.Append("	,dp.[Mileage] \n");
                sb.Append("	,dp.[NT] \n");
                sb.Append("	,t.[WorkOrder] \n");
                sb.Append("	,t.[TimeRoute] \n");
                sb.Append("	,t.[TimeSite] \n");
                sb.Append("	,t.[TimeComp] \n");
                sb.Append("	,jt.[Type] AS JobType \n");
                sb.Append("	,t.[DWork] \n");
                sb.Append("	,l.[Tag] \n");
                sb.Append("	,l.[Address]  \n");
                sb.Append("	,l.[City] \n");
                sb.Append("	,l.[State] \n");
                sb.Append("	,l.[Zip] \n");
                sb.Append(",(SELECT STUFF((SELECT ', ' + CAST(e.Unit AS VARCHAR(1000))  \n");
                sb.Append("FROM Elev e WHERE e.id IN  \n");
                sb.Append("    (SELECT me.elev_id FROM multiple_equipments me WHERE me.ticket_id = t.ID) \n");
                sb.Append("FOR XML PATH(''), Type).value('.', 'VARCHAR(1000)'),1,2,' ')) AS Unit \n");
                sb.Append(" ,(SELECT TOP 1 Signature FROM   PDATicketSignature WHERE  PDATicketID = t.ID) AS Signature \n");
                sb.Append("FROM TicketO t \n");
                sb.Append("	LEFT OUTER JOIN TicketDPDA dp WITH(NOLOCK) ON t.ID = dp.ID \n");
                sb.Append("	INNER JOIN Loc l WITH(NOLOCK) ON l.Loc = t.LID  	 \n");
                sb.Append("	LEFT JOIN Route rou WITH(NOLOCK) ON rou.ID = l.Route \n");
                sb.Append("	LEFT JOIN Job j WITH(NOLOCK) ON j.ID = t.Job \n");
                sb.Append("	LEFT JOIN JobType jt WITH(NOLOCK) ON jt.ID = j.Type  \n");
                sb.Append("WHERE t.LID = " + locID + " \n");
                sb.Append("	AND t.Recurring IS NULL AND t.Cat <> 'Recurring'\n");
                if (objChart.StartDate != null && objChart.StartDate != DateTime.MinValue)
                {
                    sb.Append("	AND t.EDate >= '" + objChart.StartDate + "' \n");
                }
                if (objChart.EndDate != null && objChart.EndDate != DateTime.MinValue)
                {
                    sb.Append("	AND t.EDate <= '" + objChart.EndDate + "' \n");
                }

                sb.Append("UNION ALL \n");
                sb.Append("SELECT  \n");
                sb.Append("	t.[ID] \n");
                sb.Append("	,t.[CDate] \n");
                sb.Append("	,t.[DDate] \n");
                sb.Append("	,t.[EDate] \n");
                sb.Append("	,t.[fWork] \n");
                sb.Append("	,t.[Job] \n");
                sb.Append("	,t.[Loc] \n");
                sb.Append("	,t.[Elev] \n");
                sb.Append("	,t.[Type] \n");
                sb.Append("	,t.[fDesc] \n");
                sb.Append("	,t.[DescRes] \n");
                sb.Append("	,t.[Charge]\n");
                sb.Append("	,t.[Total] \n");
                sb.Append("	,t.[Reg] \n");
                sb.Append("	,t.[OT] \n");
                sb.Append("	,t.[DT] \n");
                sb.Append("	,t.[TT] \n");
                sb.Append("	,t.[Zone] \n");
                sb.Append("	,t.[Toll] \n");
                sb.Append("	,t.[OtherE] \n");
                sb.Append("	,'Completed' AS Status \n");
                sb.Append("	,t.[Invoice] \n");
                sb.Append("	,t.[Level] \n");
                sb.Append("	,t.[Est] \n");
                sb.Append("	,t.[Cat] \n");
                sb.Append("	,t.[Who] \n");
                sb.Append("	,t.[fBy]  \n");
                sb.Append("	,t.[WageC] \n");
                sb.Append("	,t.[Phase] \n");
                sb.Append("	,t.[Car] \n");
                sb.Append("	,t.[CallIn] \n");
                sb.Append("	,t.[CPhone] \n");
                sb.Append("	,t.[Mileage] \n");
                sb.Append("	,t.[NT] \n");
                sb.Append("	,t.[WorkOrder] \n");
                sb.Append("	,t.[TimeRoute] \n");
                sb.Append("	,t.[TimeSite] \n");
                sb.Append("	,t.[TimeComp] \n");
                sb.Append("	,jt.[Type] AS JobType \n");
                sb.Append("	,w.[fDesc] AS DWork \n");
                sb.Append("	,l.[Tag] \n");
                sb.Append("	,l.[Address]  \n");
                sb.Append("	,l.[City] \n");
                sb.Append("	,l.[State] \n");
                sb.Append("	,l.[Zip] \n");
                sb.Append(" ,(SELECT STUFF((SELECT ', ' + CAST(e.Unit AS VARCHAR(1000))  \n");
                sb.Append(" FROM Elev e WHERE e.id IN  \n");
                sb.Append("    (SELECT me.elev_id FROM multiple_equipments me WHERE me.ticket_id = t.ID) \n");
                sb.Append(" FOR XML PATH(''), Type).value('.', 'VARCHAR(1000)'),1,2,' ')) AS Unit \n");
                sb.Append(" ,(SELECT TOP 1 Signature FROM   PDATicketSignature WHERE  PDATicketID = t.ID) AS Signature \n");
                sb.Append("FROM TicketD t \n");
                sb.Append("	INNER JOIN Loc l WITH(NOLOCK) ON l.Loc = t.Loc  	 \n");
                sb.Append("	LEFT JOIN Route rou WITH(NOLOCK) ON rou.ID = l.Route \n");
                sb.Append("	LEFT JOIN Job j WITH(NOLOCK) ON j.ID = t.Job \n");
                sb.Append("	LEFT JOIN JobType jt WITH(NOLOCK) ON jt.ID = j.Type  \n");
                sb.Append("	LEFT JOIN tblWork w WITH(NOLOCK) ON w.ID = t.fWork \n");
                sb.Append("WHERE t.Loc = " + locID + " \n");
                sb.Append("	AND t.Recurring IS NULL AND t.Cat <> 'Recurring'\n");
                if (objChart.StartDate != null && objChart.StartDate != DateTime.MinValue)
                {
                    sb.Append("	AND t.EDate >= '" + objChart.StartDate + "' \n");
                }
                if (objChart.EndDate != null && objChart.EndDate != DateTime.MinValue)
                {
                    sb.Append("	AND t.EDate <= '" + objChart.EndDate + "' \n");
                }

                #endregion

                return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetShutDownHistory(Chart objChart, int locID)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT \n");
                sb.Append("	l.[id]\n");
                sb.Append("	,CASE ISNULL(l.ticket_id, 0) \n");
                sb.Append("		WHEN 0 THEN '' \n");
                sb.Append("		ELSE Convert(varchar(50),l.ticket_id) \n");
                sb.Append("	END ticket_id\n");
                sb.Append("    ,CASE l.[status] \n");
                sb.Append("		WHEN 1 THEN 'Yes'\n");
                sb.Append("		ELSE 'No' \n");
                sb.Append("	END AS [status]\n");
                sb.Append("    ,l.[elev_id]\n");
                sb.Append("    ,l.[created_on]\n");
                sb.Append("    ,CASE ISNULL(w.fDesc,'') \n");
                sb.Append("		WHEN '' THEN u.fUser \n");
                sb.Append("		ELSE w.fDesc \n");
                sb.Append("	END AS [created_by]\n");
                sb.Append("    ,l.[reason]\n");
                sb.Append("	,l.[longdesc]\n");
                sb.Append("FROM [dbo].[ElevShutDownLog] l\n");
                sb.Append("INNER JOIN Elev e ON e.ID = l.elev_id\n");
                sb.Append("INNER JOIN tblUser u ON u.ID = l.created_by\n");
                sb.Append("LEFT JOIN tblWork w on w.fDesc = u.fUser\n");
                sb.Append("WHERE e.Loc = " + locID + "\n");
                sb.Append("ORDER BY l.created_on DESC, l.id DESC\n");

                return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLocationAndContractDetail(Chart objChart, int locID)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT   \n");
                sb.Append("	l.ID,   \n");
                sb.Append("	l.Consult,   \n");
                sb.Append("	l.Tag,   \n");
                sb.Append("	l.Address AS locAddress,   \n");
                sb.Append("	l.City AS locCity,   \n");
                sb.Append("	l.State AS locState,  \n");
                sb.Append("	l.Zip AS locZip,   \n");
                sb.Append("	l.Rol,   \n");
                sb.Append("	l.Type ,   \n");
                sb.Append("	Route,   \n");
                sb.Append("	Terr,   \n");
                sb.Append("	Terr2,   \n");
                sb.Append("	r.City,   \n");
                sb.Append("	r.State,   \n");
                sb.Append("	r.Zip,   \n");
                sb.Append("	r.Country,   \n");
                sb.Append("	r.Address,   \n");
                sb.Append("	l.Remarks,   \n");
                sb.Append("	r.Contact AS MainContact,   \n");
                sb.Append("	r.Phone,   \n");
                sb.Append("	r.Website,   \n");
                sb.Append("	r.EMail,  \n");
                sb.Append("	r.Cellular,   \n");
                sb.Append("	r.Fax,    \n");
                sb.Append("	l.Owner,   \n");
                sb.Append("	l.Stax,   \n");
                sb.Append("	l.STax2,   \n");
                sb.Append("	l.UTax,   \n");
                sb.Append("	l.Zone,   \n");
                sb.Append("	l.Country AS LocCountry,   \n");
                sb.Append("	r.Lat, \n");
                sb.Append("	r.Lng, \n");
                sb.Append("	l.Custom1, \n");
                sb.Append("	l.Custom2, \n");
                sb.Append("	l.Custom14, \n");
                sb.Append("	l.Custom15, \n");
                sb.Append("	l.Custom12, \n");
                sb.Append("	l.Custom13, \n");
                sb.Append("	l.Status,   \n");
                sb.Append("	l.Billing,   \n");
                sb.Append("	isnull(l.BillRate,0) AS BillRate,   \n");
                sb.Append("	isnull(l.RateOT,0) AS RateOT,    \n");
                sb.Append("	isnull(l.RateNT,0) AS RateNT,    \n");
                sb.Append("	isnull(l.RateDT,0) AS RateDT,    \n");
                sb.Append("	isnull(l.RateTravel,0) AS RateTravel,   \n");
                sb.Append("	isnull(l.RateMileage,0) AS RateMileage,    \n");
                sb.Append("	isnull(stax.Rate,0) AS Rate,    \n");
                sb.Append("	l.PrintInvoice,    \n");
                sb.Append("	l.EmailInvoice,    \n");
                sb.Append("	l.Balance,    \n");
                sb.Append("	l.Loc,    \n");
                sb.Append("	isnull(l.NoCustomerStatement,0) AS NoCustomerStatement,   		 \n");
                sb.Append("	l.Address + ', '+ l.City + ', ' + l.State + ' ' + l.Zip   AS LocationName,   \n");
                sb.Append("	tr.Name AS Salesperson,   \n");
                sb.Append("	rt.Name AS RouteName,    \n");
                sb.Append("	ISNULL(cst.Name, 'None') AS ConsultantName,    \n");
                sb.Append("	o.Custom1 AS OwnerName,   rl.name AS Customer,    \n");
                sb.Append("	(SELECT COUNT(1) FROM  Elev e WHERE e.loc=l.loc) AS Elevs  	   \n");
                sb.Append("FROM Loc l   \n");
                sb.Append("	LEFT OUTER JOIN Owner o ON o.ID = l.Owner   \n");
                sb.Append("	LEFT OUTER JOIN Rol rl ON o.Rol = rl.ID   \n");
                sb.Append("	LEFT OUTER JOIN  Rol r ON l.Rol=r.ID and r.Type=4   \n");
                sb.Append("	LEFT OUTER JOIN Stax ON Stax.name = l.stax   \n");
                sb.Append("	LEFT OUTER JOIN Branch b ON b.ID = r.EN   \n");
                sb.Append("	LEFT OUTER JOIN Terr tr  ON l.Terr = tr.ID    \n");
                sb.Append("	LEFT OUTER JOIN Route rt  ON l.Route = rt.ID    \n");
                sb.Append("	LEFT OUTER JOIN tblConsult cst ON cst.ID = l.Consult   \n");
                sb.Append("WHERE l.Loc = " + locID + " \n");

                sb.Append("SELECT  \n");
                sb.Append("	c.Job, \n");
                sb.Append("	c.ExpirationDate,   \n");
                sb.Append("	j.CType,  \n");
                sb.Append("	j.fDesc,  \n");
                sb.Append("	c.BAmt,  \n");
                sb.Append("	c.Hours,  \n");
                sb.Append("	l.ID AS LocID,  \n");
                sb.Append("	l.Tag,  \n");
                sb.Append("	isnull(l.credit,0) AS Credit,     \n");
                sb.Append("	CASE c.bcycle  \n");
                sb.Append("		WHEN 0 THEN 'Monthly'  \n");
                sb.Append("		WHEN 1 THEN 'Bi-Monthly'  \n");
                sb.Append("		WHEN 2 THEN 'Quarterly'  \n");
                sb.Append("		WHEN 3 THEN '3 Times/Year'  \n");
                sb.Append("		WHEN 4 THEN 'Semi-Annually'  \n");
                sb.Append("		WHEN 5 THEN 'Annually'  \n");
                sb.Append("		WHEN 6 THEN 'Never'  \n");
                sb.Append("		WHEN 7 THEN '3 Years'  \n");
                sb.Append("		WHEN 8 THEN '5 Years'  \n");
                sb.Append("		WHEN 9 THEN '2 Years'  \n");
                sb.Append("	END AS Freqency,  \n");
                sb.Append("	CASE c.SCycle  \n");
                sb.Append("		WHEN -1 THEN 'Never'  \n");
                sb.Append("		WHEN 0 THEN 'Monthly'  \n");
                sb.Append("		WHEN 1 THEN 'Bi-Monthly'  \n");
                sb.Append("		WHEN 2 THEN 'Quarterly'  \n");
                sb.Append("		WHEN 3 THEN 'Semi-Annually'  \n");
                sb.Append("		WHEN 4 THEN 'Annually'  \n");
                sb.Append("		WHEN 5 THEN 'Weekly'  \n");
                sb.Append("		WHEN 6 THEN 'Bi-Weekly'  \n");
                sb.Append("		WHEN 7 THEN 'Every 13 Weeks'  \n");
                sb.Append("		WHEN 10 THEN 'Every 2 Years'  \n");
                sb.Append("		WHEN 8 THEN 'Every 3 Years'  \n");
                sb.Append("		WHEN 9 THEN 'Every 5 Years'  \n");
                sb.Append("		WHEN 11 THEN 'Every 7 Years'  \n");
                sb.Append("		WHEN 12 THEN 'On-Demand'  \n");
                sb.Append("		WHEN 13 THEN 'Daily'  \n");
                sb.Append("		WHEN 14 THEN 'Twice a Month'  \n");
                sb.Append("	END AS TicketFreq,  \n");
                sb.Append("	CASE c.Status  \n");
                sb.Append("		WHEN 0 THEN 'Active'  \n");
                sb.Append("		WHEN 1 THEN 'Closed'  \n");
                sb.Append("		WHEN 2 THEN 'Hold'  \n");
                sb.Append("		WHEN 3 THEN 'Completed'  \n");
                sb.Append("	END ASStatus,  \n");
                sb.Append("	CASE    \n");
                sb.Append("		WHEN j.Custom20 > 0 THEN   \n");
                sb.Append("		(SELECT TOP 1 NAME FROM  route WHERE  id = j.Custom20)  \n");
                sb.Append("		ELSE 'Unassigned'  \n");
                sb.Append("	END AS Worker  \n");
                sb.Append("FROM Job j  \n");
                sb.Append("    INNER JOIN Contract c ON j.ID = c.Job  \n");
                sb.Append("    LEFT OUTER JOIN Loc l ON l.Loc = c.Loc   \n");
                sb.Append("WHERE  j.Type = 0   \n");
                sb.Append("	AND c.Loc = " + locID + " \n");

                return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetUnitInspectedTrimNotCompleteReport(string connString)
        {
            try
            {

                return SqlHelper.ExecuteDataset(connString, CommandType.StoredProcedure, "Usp_UnitInspectedTrimNotCompleteReport");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetUnitFinishedTrimNotCompleteReport(string connString)
        {
            try
            {

                return SqlHelper.ExecuteDataset(connString, CommandType.StoredProcedure, "Usp_UnitFinishedTrimNotCompleteReport");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSubstantialCompletionDeliveryNotPaidReport(string connString)
        {
            try
            {

                return SqlHelper.ExecuteDataset(connString, CommandType.StoredProcedure, "Usp_SubstantialCompletionDeliveryNotPaidReport");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSubstantialCompletionFinalNotPaidReport(string connString)
        {
            try
            {

                return SqlHelper.ExecuteDataset(connString, CommandType.StoredProcedure, "Usp_SubstantialCompletionFinalNotPaidReport");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCompletedTicketByRoute(Chart objChart, string routes)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT \n");
                sb.Append("	rou.Name AS RouteName, \n");
                sb.Append("	COUNT(t.ID) AS TicketCount, \n");
                sb.Append("	SUM(ISNULL(t.OT,0)) AS OT, \n");
                sb.Append("	SUM(ISNULL(t.DT,0)) AS DT, \n");
                sb.Append("	SUM(ISNULL(t.TT,0)) AS Travel, \n");
                sb.Append("	SUM(ISNULL(t.Total,0)) AS Total, \n");
                sb.Append("	SUM(ISNULL(t.Reg,0)) AS Reg, \n");
                sb.Append("	w.fDesc AS Mech, \n");
                sb.Append("	LEFT(rou.Name, 6) AS GroupName \n");
                sb.Append("FROM TicketD t \n");
                sb.Append("	INNER JOIN Loc l ON l.Loc = t.Loc \n");
                sb.Append("	LEFT JOIN Route rou ON rou.ID = l.Route \n");
                sb.Append("	LEFT JOIN tblWork w ON w.ID = rou.Mech \n");
                sb.Append("WHERE t.Cat = 'Service Call' \n");
                sb.Append(" AND t.EDate >= '" + objChart.StartDate + "' AND t.EDate < '" + objChart.EndDate.AddDays(1) + "' \n");
                if (!string.IsNullOrEmpty(routes))
                {
                    sb.Append("	AND LEFT(rou.Name, 6) IN (" + routes + ") \n");
                }
                sb.Append("GROUP BY rou.Name, w.fDesc \n");

                return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetComparativeStatementData(Chart objChart, List<ComparativeStatementRequest> objComparative)
        {
            try
            {
                string selectData = string.Empty;
                string condition = string.Empty;
                foreach (var item in objComparative)
                {
                    if (!string.IsNullOrEmpty(selectData))
                    {
                        selectData += ",";
                    }

                    if (item.Type == "Difference")
                    {
                        selectData += string.Format("(ISNULL(table{0}.[{0}], 0) - ISNULL(table{1}.[{1}], 0)) AS Column{2}", item.Column1, item.Column2, item.Index);
                    }
                    else if (item.Type == "Variance")
                    {
                        selectData += string.Format("(CASE WHEN ISNULL(table{1}.[{1}], 0) = 0 then 0 ELSE (ISNULL(table{0}.[{0}], 0) - table{1}.[{1}]) / ABS(table{1}.[{1}]) END) AS Column{2}", item.Column1, item.Column2, item.Index);
                    }
                    else
                    {
                        selectData += string.Format("ISNULL(table{0}.[{0}], 0) AS Column{0}", item.Index);

                        if (!string.IsNullOrEmpty(condition))
                        {
                            condition += " OR ";
                        }

                        condition += string.Format(" table{0}.[{0}] <> 0", item.Index);
                    }

                    if (item.Type == "Actual")
                    {
                        selectData += string.Format(",'' AS Column{0}URL", item.Index);
                    }
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT                                   \n");
                sb.Append("	c.ID AS Acct,                           \n");
                sb.Append("	c.Acct AS AcctNo,                       \n");
                sb.Append("	c.fDesc AS AcctName,                    \n");
                sb.Append("	c.Acct + '  ' + c.fDesc	AS fDesc,       \n");
                sb.Append("	c.Type,                                 \n");
                sb.Append("	(CASE c.Type                            \n");
                sb.Append("		WHEN 0 THEN 'Asset'                 \n");
                sb.Append("		WHEN 1 THEN 'Liability'             \n");
                sb.Append("		WHEN 2 THEN 'Equity'                \n");
                sb.Append("		WHEN 3 THEN 'Revenues'              \n");
                sb.Append("		WHEN 4 THEN 'Cost of Sales'         \n");
                sb.Append("		WHEN 5 THEN 'Expenses'              \n");
                sb.Append("		WHEN 6 THEN 'Bank'                  \n");
                sb.Append("		WHEN 7 THEN 'Non-Posting'           \n");
                sb.Append("		WHEN 8 THEN 'Other Income (Expense)'\n");
                sb.Append("		WHEN 9 THEN 'Provisions for Income Taxes' \n");
                sb.Append("		END) AS TypeName,                   \n");
                sb.Append("	(CASE c.Sub WHEN '' THEN                \n");
                sb.Append("		(CASE c.Type                        \n");
                sb.Append("			WHEN 0 THEN 'Asset'             \n");
                sb.Append("			WHEN 1 THEN 'Liability'         \n");
                sb.Append("			WHEN 2 THEN 'Equity'            \n");
                sb.Append("			WHEN 3 THEN 'Revenues'          \n");
                sb.Append("			WHEN 4 THEN 'Cost of Sales'     \n");
                sb.Append("			WHEN 5 THEN 'Expenses'          \n");
                sb.Append("			WHEN 6 THEN 'Bank'              \n");
                sb.Append("			WHEN 7 THEN 'Non-Posting'       \n");
                sb.Append("			WHEN 8 THEN 'Other Income (Expense)' \n");
                sb.Append("			WHEN 9 THEN 'Provisions for Income Taxes' \n");
                sb.Append("		END)                                \n");
                sb.Append("	ELSE c.Sub END) AS Sub,                 \n");
                sb.Append("	ISNULL(c.Department, 0) AS Department,  \n");
                sb.Append("	ISNULL(ct.CentralName,'Undefined') AS CentralName \n");

                if (!string.IsNullOrEmpty(selectData))
                {
                    sb.Append("," + selectData + "                  \n");
                }
                sb.Append(" FROM Chart c                            \n");
                sb.Append(" LEFT JOIN Central ct ON c.Department = ct.ID \n");

                foreach (var item in objComparative)
                {
                    if (item.Type != "Difference" && item.Type != "Variance")
                    {
                        var tableName = string.Format("table{0}", item.Index);

                        sb.Append(" LEFT JOIN (                                 \n");
                        if (item.Type == "Actual")
                        {
                            sb.Append("SELECT                                   \n");
                            sb.Append("	c1.ID AS Acct,                          \n");
                            sb.Append("	c1.Acct AS AcctNo,                      \n");
                            sb.Append("	(CASE c1.Type                           \n");
                            sb.Append("		WHEN 3 THEN (ISNULL(SUM(t.Amount), 0) * -1) \n");
                            sb.Append("		WHEN 8 THEN (ISNULL(SUM(t.Amount), 0) * -1) \n");
                            sb.Append("	ELSE                                    \n");
                            sb.Append("		ISNULL(SUM(t.Amount),0)             \n");
                            sb.Append("	END) AS [" + item.Index + "]			\n");
                            sb.Append(" FROM Chart c1 LEFT OUTER JOIN Trans t ON t.Acct = c1.ID \n");
                            sb.Append(" WHERE c1.Type IN (3, 4, 5, 8, 9)                              \n");
                            sb.Append("	    AND t.fDate >= '" + item.StartDate.Value.Date + "'        \n");
                            sb.Append("	    AND t.fDate <= '" + item.EndDate.Value.Date + "'			\n");
                            sb.Append(" GROUP BY c1.ID, c1.Acct , c1.Type                       \n");
                        }
                        else
                        {
                            sb.Append(" SELECT DISTINCT                         \n");
                            sb.Append("  c.ID AS Acct,                          \n");
                            sb.Append("  Act.Acct AS AcctNo,                    \n");
                            sb.Append("  SUM(CASE ActD.Period                   \n");
                            sb.Append("	    WHEN " + (item.StartDate.Value.Year * 100 + item.StartDate.Value.Month));
                            sb.Append("         THEN ((ISNULL(ActD.Amount,0) / " + DateTime.DaysInMonth(item.StartDate.Value.Year, item.StartDate.Value.Month) + ") * " + (DateTime.DaysInMonth(item.StartDate.Value.Year, item.StartDate.Value.Month) - item.StartDate.Value.Day + 1) + ") \n");
                            sb.Append("	    WHEN " + (item.EndDate.Value.Year * 100 + item.EndDate.Value.Month));
                            sb.Append("         THEN ((ISNULL(ActD.Amount,0) / " + DateTime.DaysInMonth(item.EndDate.Value.Year, item.EndDate.Value.Month) + ") * " + item.EndDate.Value.Day + ")\n");
                            sb.Append("	    ELSE ISNULL(ActD.Amount,0)          \n");
                            sb.Append("  END) AS [" + item.Index + "]           \n");
                            sb.Append("  FROM Account Act                       \n");
                            sb.Append("	 INNER JOIN Chart c ON c.Acct = Act.Acct                                 \n");
                            sb.Append("	 INNER JOIN AccountDetails ActD ON Act.AccountID = ActD.AccountID        \n");
                            sb.Append("	 INNER JOIN Budget B ON B.BudgetID = ActD.BudgetID                       \n");
                            sb.Append(" WHERE  B.Budget = '" + item.Type + "'                                    \n");
                            sb.Append("	 AND ActD.Period >= " + (item.StartDate.Value.Year * 100 + item.StartDate.Value.Month));
                            sb.Append("  AND ActD.Period <= " + (item.EndDate.Value.Year * 100 + item.EndDate.Value.Month));
                            sb.Append(" GROUP BY  c.ID, Act.Acct                                                 \n");
                        }

                        sb.Append(" ) AS " + tableName + " ON c.ID = " + tableName + ".Acct \n");
                    }
                }

                sb.Append(" WHERE c.Type IN (3, 4, 5, 8, 9)                       \n");

                // Get Comparative Statement with Center
                if (!string.IsNullOrEmpty(objChart.Departments))
                {
                    var centers = objChart.Departments.Split(',');
                    if (Array.FindAll(centers, s => s.Equals("0")).Length > 0)
                    {
                        sb.Append("    AND (c.Department IN (" + objChart.Departments + ") OR c.Department IS NULL )   \n");
                    }
                    else
                    {
                        sb.Append("    AND c.Department IN (" + objChart.Departments + ")   \n");
                    }
                }

                if (!string.IsNullOrEmpty(condition))
                {
                    sb.Append("     AND (" + condition + ")                 \n");
                }
                sb.Append(" ORDER BY c.Type, c.Acct                         \n");

                return SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetComparativeStatementSummaryData(Chart objChart, List<ComparativeStatementRequest> objComparative)
        {
            try
            {
                string selectData = string.Empty;
                string condition = string.Empty;
                foreach (var item in objComparative)
                {
                    if (!string.IsNullOrEmpty(selectData))
                    {
                        selectData += ",";
                    }

                    if (item.Type == "Difference")
                    {
                        selectData += string.Format("SUM(ISNULL(table{0}.[{0}], 0) - ISNULL(table{1}.[{1}], 0)) AS Column{2}", item.Column1, item.Column2, item.Index);
                    }
                    else if (item.Type == "Variance")
                    {
                        selectData += string.Format("(CASE WHEN SUM(ISNULL(table{0}.[{0}], 0)) = 0 then 0 ELSE SUM(ISNULL(table{1}.[{1}], 0) - table{0}.[{0}]) / ABS(SUM(table{0}.[{0}])) END) AS Column{2}", item.Column1, item.Column2, item.Index);
                    }
                    else
                    {
                        selectData += string.Format("SUM(ISNULL(table{0}.[{0}], 0)) AS Column{0}", item.Index);

                        if (!string.IsNullOrEmpty(condition))
                        {
                            condition += " OR ";
                        }

                        condition += string.Format(" table{0}.[{0}] <> 0", item.Index);
                    }
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT                                   \n");
                sb.Append("	c.Type,                                 \n");
                sb.Append("	(CASE c.Type                            \n");
                sb.Append("		WHEN 0 THEN 'Asset'                 \n");
                sb.Append("		WHEN 1 THEN 'Liability'             \n");
                sb.Append("		WHEN 2 THEN 'Equity'                \n");
                sb.Append("		WHEN 3 THEN 'Revenues'              \n");
                sb.Append("		WHEN 4 THEN 'Cost of Sales'         \n");
                sb.Append("		WHEN 5 THEN 'Expenses'              \n");
                sb.Append("		WHEN 6 THEN 'Bank'                  \n");
                sb.Append("		WHEN 7 THEN 'Non-Posting'           \n");
                sb.Append("		WHEN 8 THEN 'Other Income (Expense)'\n");
                sb.Append("		WHEN 9 THEN 'Provisions for Income Taxes' \n");
                sb.Append("		END) AS TypeName,                   \n");
                sb.Append("	(CASE c.Sub WHEN '' THEN                \n");
                sb.Append("		(CASE c.Type                        \n");
                sb.Append("			WHEN 0 THEN 'Asset'             \n");
                sb.Append("			WHEN 1 THEN 'Liability'         \n");
                sb.Append("			WHEN 2 THEN 'Equity'            \n");
                sb.Append("			WHEN 3 THEN 'Revenues'          \n");
                sb.Append("			WHEN 4 THEN 'Cost of Sales'     \n");
                sb.Append("			WHEN 5 THEN 'Expenses'          \n");
                sb.Append("			WHEN 6 THEN 'Bank'              \n");
                sb.Append("			WHEN 7 THEN 'Non-Posting'       \n");
                sb.Append("			WHEN 8 THEN 'Other Income (Expense)' \n");
                sb.Append("			WHEN 9 THEN 'Provisions for Income Taxes' \n");
                sb.Append("		END)                                \n");
                sb.Append("	ELSE c.Sub END) AS Sub,                 \n");
                sb.Append("	ISNULL(c.Department, 0) AS Department,  \n");
                sb.Append("	ISNULL(ct.CentralName,'Undefined') AS CentralName \n");

                if (!string.IsNullOrEmpty(selectData))
                {
                    sb.Append("," + selectData + "                  \n");
                }
                sb.Append(" FROM Chart c                            \n");
                sb.Append(" LEFT JOIN Central ct ON c.Department = ct.ID \n");
                foreach (var item in objComparative)
                {
                    if (item.Type != "Difference" && item.Type != "Variance")
                    {
                        var tableName = string.Format("table{0}", item.Index);

                        sb.Append(" LEFT JOIN (                                 \n");
                        if (item.Type == "Actual")
                        {
                            sb.Append("SELECT                                   \n");
                            sb.Append("	c1.ID AS Acct,                          \n");
                            sb.Append("	c1.Acct AS AcctNo,                      \n");
                            sb.Append("	(CASE c1.Type                           \n");
                            sb.Append("		WHEN 3 THEN (ISNULL(SUM(t.Amount),0) * -1) \n");
                            sb.Append("		WHEN 8 THEN (ISNULL(SUM(t.Amount),0) * -1) \n");
                            sb.Append("	ELSE                                    \n");
                            sb.Append("		ISNULL(SUM(t.Amount),0)             \n");
                            sb.Append("	END) AS [" + item.Index + "]			\n");
                            sb.Append(" FROM Chart c1 LEFT OUTER JOIN Trans t ON t.Acct = c1.ID \n");
                            sb.Append(" WHERE c1.Type IN (3, 4, 5, 8, 9)                              \n");
                            sb.Append("	    AND t.fDate >= '" + item.StartDate.Value.Date + "'        \n");
                            sb.Append("	    AND t.fDate <= '" + item.EndDate.Value.Date + "'			\n");
                            sb.Append(" GROUP BY c1.ID, c1.Acct , c1.Type                       \n");
                        }
                        else
                        {
                            sb.Append(" SELECT DISTINCT                         \n");
                            sb.Append("  c.ID AS Acct,                          \n");
                            sb.Append("  Act.Acct AS AcctNo,                    \n");
                            sb.Append("  SUM(CASE ActD.Period                   \n");
                            sb.Append("	    WHEN " + (item.StartDate.Value.Year * 100 + item.StartDate.Value.Month));
                            sb.Append("         THEN ((ISNULL(ActD.Amount,0) / " + DateTime.DaysInMonth(item.StartDate.Value.Year, item.StartDate.Value.Month) + ") * " + (DateTime.DaysInMonth(item.StartDate.Value.Year, item.StartDate.Value.Month) - item.StartDate.Value.Day + 1) + ") \n");
                            sb.Append("	    WHEN " + (item.EndDate.Value.Year * 100 + item.EndDate.Value.Month));
                            sb.Append("         THEN ((ISNULL(ActD.Amount,0) / " + DateTime.DaysInMonth(item.EndDate.Value.Year, item.EndDate.Value.Month) + ") * " + item.EndDate.Value.Day + ")\n");
                            sb.Append("	    ELSE ISNULL(ActD.Amount,0)          \n");
                            sb.Append("  END) AS [" + item.Index + "]           \n");
                            sb.Append("  FROM Account Act                       \n");
                            sb.Append("	 INNER JOIN Chart c ON c.Acct = Act.Acct                                 \n");
                            sb.Append("	 INNER JOIN AccountDetails ActD ON Act.AccountID = ActD.AccountID        \n");
                            sb.Append("	 INNER JOIN Budget B ON B.BudgetID = ActD.BudgetID                       \n");
                            sb.Append(" WHERE  B.Budget = '" + item.Type + "'                                    \n");
                            sb.Append("	 AND ActD.Period >= " + (item.StartDate.Value.Year * 100 + item.StartDate.Value.Month));
                            sb.Append("  AND ActD.Period <= " + (item.EndDate.Value.Year * 100 + item.EndDate.Value.Month));
                            sb.Append(" GROUP BY  c.ID, Act.Acct                                                 \n");
                        }

                        sb.Append(" ) AS " + tableName + " ON c.ID = " + tableName + ".Acct \n");
                    }
                }

                sb.Append(" WHERE c.Type IN (3, 4, 5, 8, 9)                       \n");

                // Get Comparative Statement with Center
                if (!string.IsNullOrEmpty(objChart.Departments))
                {
                    var centers = objChart.Departments.Split(',');
                    if (Array.FindAll(centers, s => s.Equals("0")).Length > 0)
                    {
                        sb.Append("    AND (c.Department IN (" + objChart.Departments + ") OR c.Department IS NULL )   \n");
                    }
                    else
                    {
                        sb.Append("    AND c.Department IN (" + objChart.Departments + ")   \n");
                    }
                }

                if (!string.IsNullOrEmpty(condition))
                {
                    sb.Append("     AND (" + condition + ")                 \n");
                }

                sb.Append(" GROUP BY c.Type, c.Sub, c.Department, ct.CentralName \n");
                sb.Append(" ORDER BY c.Type, c.Sub                         \n");

                return SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetComparativeBalanceSheetData(Chart objChart, List<ComparativeStatementRequest> objComparative)
        {
            try
            {
                string selectData = string.Empty;
                string condition = string.Empty;
                foreach (var item in objComparative)
                {
                    if (!string.IsNullOrEmpty(selectData))
                    {
                        selectData += ",";
                    }

                    if (item.Type == "Difference")
                    {
                        selectData += string.Format("(ISNULL(table{0}.[{0}], 0) - ISNULL(table{1}.[{1}], 0)) AS Column{2}", item.Column1, item.Column2, item.Index);
                    }
                    else if (item.Type == "Variance")
                    {
                        selectData += string.Format("(CASE WHEN ISNULL(table{1}.[{1}], 0) = 0 then 0 ELSE (ISNULL(table{0}.[{0}], 0) - table{1}.[{1}]) / ABS(table{1}.[{1}]) END) AS Column{2}", item.Column1, item.Column2, item.Index);
                    }
                    else
                    {
                        selectData += string.Format("ISNULL(table{0}.[{0}], 0) AS Column{0}", item.Index);

                        if (!string.IsNullOrEmpty(condition))
                        {
                            condition += " OR ";
                        }

                        condition += string.Format(" table{0}.[{0}] <> 0", item.Index);
                    }

                    if (item.Type == "Actual")
                    {
                        selectData += string.Format(",'' AS Column{0}URL", item.Index);
                    }
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT                                   \n");
                sb.Append("	c.ID AS Acct,                           \n");
                sb.Append("	c.Acct AS AcctNo,                       \n");
                sb.Append("	c.fDesc AS AcctName,                    \n");
                sb.Append("	c.Acct + ' ' + c.fDesc	AS fDesc,       \n");
                sb.Append("	c.Type,                                 \n");
                sb.Append("	(CASE c.Type                            \n");
                sb.Append("		WHEN 0 THEN 'Asset'                 \n");
                sb.Append("		WHEN 1 THEN 'Liability'             \n");
                sb.Append("		WHEN 2 THEN 'Equity'                \n");
                sb.Append("		WHEN 3 THEN 'Revenues'              \n");
                sb.Append("		WHEN 4 THEN 'Cost of Sales'         \n");
                sb.Append("		WHEN 5 THEN 'Expenses'              \n");
                sb.Append("		WHEN 6 THEN 'Bank'                  \n");
                sb.Append("		END) AS TypeName,                   \n");
                sb.Append("	(CASE c.Sub WHEN '' THEN                \n");
                sb.Append("		(CASE c.Type                        \n");
                sb.Append("			WHEN 0 THEN 'Asset'             \n");
                sb.Append("			WHEN 1 THEN 'Liability'         \n");
                sb.Append("			WHEN 2 THEN 'Equity'            \n");
                sb.Append("			WHEN 3 THEN 'Revenues'          \n");
                sb.Append("			WHEN 4 THEN 'Cost of Sales'     \n");
                sb.Append("			WHEN 5 THEN 'Expenses'          \n");
                sb.Append("			WHEN 6 THEN 'Bank'              \n");
                sb.Append("		END)                                \n");
                sb.Append("	ELSE c.Sub END) AS Sub,                 \n");
                sb.Append("	ISNULL(c.Department, 0) AS Department,  \n");
                sb.Append("	ISNULL(ct.CentralName,'Undefined') AS CentralName \n");

                if (!string.IsNullOrEmpty(selectData))
                {
                    sb.Append("," + selectData + "                  \n");
                }
                sb.Append(" FROM Chart c                            \n");
                sb.Append(" LEFT JOIN Central ct ON c.Department = ct.ID \n");

                foreach (var item in objComparative)
                {
                    if (item.Type != "Difference" && item.Type != "Variance")
                    {
                        var tableName = string.Format("table{0}", item.Index);

                        sb.Append(" LEFT JOIN (                                 \n");
                        if (item.Type == "Actual")
                        {
                            sb.Append("SELECT                                   \n");
                            sb.Append("	c1.ID AS Acct,                          \n");
                            sb.Append("	SUM(CASE c1.Type                        \n");
                            sb.Append("		WHEN 1 THEN ISNULL(t.Amount,0) * -1 \n");
                            sb.Append("		WHEN 2 THEN ISNULL(t.Amount,0) * -1 \n");
                            sb.Append("		ELSE ISNULL(t.Amount,0) END) AS [" + item.Index + "] \n");
                            sb.Append("FROM Trans AS t                          \n");
                            sb.Append("	INNER JOIN Chart AS c1 ON c1.ID = t.Acct \n");
                            sb.Append("WHERE (c1.Type IN (0, 1, 2, 6) OR c1.DefaultNo = 'D3130') \n");
                            sb.Append("	AND t.Amount <> 0                       \n");
                            sb.Append("    AND t.fDate <= '" + item.EndDate.Value.Date + "' \n");
                            sb.Append("GROUP BY c1.ID \n");
                        }

                        sb.Append(" ) AS " + tableName + " ON c.ID = " + tableName + ".Acct \n");
                    }
                }

                sb.Append(" WHERE (c.Type IN (0, 1, 2, 6) OR c.DefaultNo = 'D3130')  \n");

                // Get Comparative Statement with Center
                if (!string.IsNullOrEmpty(objChart.Departments))
                {
                    var centers = objChart.Departments.Split(',');
                    if (Array.FindAll(centers, s => s.Equals("0")).Length > 0)
                    {
                        sb.Append("    AND (c.Department IN (" + objChart.Departments + ") OR c.Department IS NULL )   \n");
                    }
                    else
                    {
                        sb.Append("    AND c.Department IN (" + objChart.Departments + ")   \n");
                    }
                }

                if (!string.IsNullOrEmpty(condition))
                {
                    sb.Append("     AND (" + condition + ")                 \n");
                }
                sb.Append(" ORDER BY c.Type, c.Acct                         \n");

                // Get min date
                sb.Append(" SELECT MIN(ISNULL(fDate, GETDATE())) FROM Trans \n");

                // Get data without Center filter
                sb.Append("SELECT                                   \n");
                sb.Append("	c.ID AS Acct,                           \n");
                sb.Append("	c.Acct AS AcctNo,                       \n");
                sb.Append("	c.Type                                 \n");

                if (!string.IsNullOrEmpty(selectData))
                {
                    sb.Append("," + selectData + "                  \n");
                }
                sb.Append(" FROM Chart c                            \n");

                foreach (var item in objComparative)
                {
                    if (item.Type != "Difference" && item.Type != "Variance")
                    {
                        var tableName = string.Format("table{0}", item.Index);

                        sb.Append(" LEFT JOIN (                                 \n");
                        if (item.Type == "Actual")
                        {
                            sb.Append("SELECT                                   \n");
                            sb.Append("	c1.ID AS Acct,                          \n");
                            sb.Append("	SUM(CASE c1.Type                        \n");
                            sb.Append("		WHEN 1 THEN ISNULL(t.Amount,0) * -1 \n");
                            sb.Append("		WHEN 2 THEN ISNULL(t.Amount,0) * -1 \n");
                            sb.Append("		ELSE ISNULL(t.Amount,0) END) AS [" + item.Index + "] \n");
                            sb.Append("FROM Trans AS t                          \n");
                            sb.Append("	INNER JOIN Chart AS c1 ON c1.ID = t.Acct \n");
                            sb.Append("WHERE (c1.Type IN (0, 1, 2, 6) OR c1.DefaultNo = 'D3130') \n");
                            sb.Append("	AND t.Amount <> 0                       \n");
                            sb.Append("    AND t.fDate <= '" + item.EndDate.Value.Date + "' \n");
                            sb.Append("GROUP BY c1.ID \n");
                        }

                        sb.Append(" ) AS " + tableName + " ON c.ID = " + tableName + ".Acct \n");
                    }
                }

                sb.Append(" WHERE (c.Type IN (0, 1, 2, 6) OR c.DefaultNo = 'D3130')  \n");
                if (!string.IsNullOrEmpty(condition))
                {
                    sb.Append("     AND (" + condition + ")                 \n");
                }

                return SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPastDueMCPData(Chart objChart)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT \n");
                sb.Append("    l.Tag AS LocationName, \n");
                sb.Append("	   e.Unit, \n");
                sb.Append("	   r.Name AS DefaultWorker, \n");
                sb.Append("	   et.fdesc AS Name, \n");
                sb.Append("    i.ID,  \n");
                sb.Append("    i.EquipT,\n");
                sb.Append("    i.Code, \n");
                sb.Append("    i.Section, \n");
                sb.Append("    i.fDesc, \n");
                sb.Append("	   i.Lastdate, \n");
                sb.Append("	   i.NextDateDue, \n");
                sb.Append("    i.Frequency, \n");
                sb.Append("    (CASE i.Frequency \n");
                sb.Append("        WHEN 0 THEN 'Daily' \n");
                sb.Append("        WHEN 1 THEN 'Weekly' \n");
                sb.Append("        WHEN 2 THEN 'Bi-Weekly' \n");
                sb.Append("        WHEN 3 THEN 'Monthly' \n");
                sb.Append("        WHEN 4 THEN 'Bi-Monthly' \n");
                sb.Append("        WHEN 5 THEN 'Quarterly' \n");
                sb.Append("        WHEN 6 THEN 'Semi-Annually' \n");
                sb.Append("        WHEN 7 THEN 'Annually' \n");
                sb.Append("        WHEN 8 THEN 'One Time' \n");
                sb.Append("        WHEN 9 THEN '3 Times a Year' \n");
                sb.Append("        WHEN 10 THEN 'Every 2 Year' \n");
                sb.Append("        WHEN 11 THEN 'Every 3 Year' \n");
                sb.Append("        WHEN 12 THEN 'Every 5 Year' \n");
                sb.Append("        WHEN 13 THEN 'Every 7 Year' \n");
                sb.Append("        WHEN 14 THEN 'On-Demand' \n");
                sb.Append("    END) AS FrequencyName \n");
                sb.Append("FROM EquipTItem i \n");
                sb.Append("	INNER JOIN EquipTemp et ON et.ID = i.EquipT \n");
                sb.Append("	INNER JOIN Elev e ON e.ID = i.Elev \n");
                sb.Append("	INNER JOIN Loc l ON l.Loc = e.Loc \n");
                sb.Append("	LEFT JOIN Route r ON r.ID = l.Route \n");
                sb.Append("WHERE i.NextDateDue < GETDATE() \n");
                sb.Append("ORDER BY l.Tag, e.Unit, et.fdesc \n");

                return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetPastDueMCPData(GetPastDueMCPDataParam _GetPastDueMCPData, string ConnectionString)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT \n");
                sb.Append("    l.Tag AS LocationName, \n");
                sb.Append("	   e.Unit, \n");
                sb.Append("	   r.Name AS DefaultWorker, \n");
                sb.Append("	   et.fdesc AS Name, \n");
                sb.Append("    i.ID,  \n");
                sb.Append("    i.EquipT,\n");
                sb.Append("    i.Code, \n");
                sb.Append("    i.Section, \n");
                sb.Append("    i.fDesc, \n");
                sb.Append("	   i.Lastdate, \n");
                sb.Append("	   i.NextDateDue, \n");
                sb.Append("    i.Frequency, \n");
                sb.Append("    (CASE i.Frequency \n");
                sb.Append("        WHEN 0 THEN 'Daily' \n");
                sb.Append("        WHEN 1 THEN 'Weekly' \n");
                sb.Append("        WHEN 2 THEN 'Bi-Weekly' \n");
                sb.Append("        WHEN 3 THEN 'Monthly' \n");
                sb.Append("        WHEN 4 THEN 'Bi-Monthly' \n");
                sb.Append("        WHEN 5 THEN 'Quarterly' \n");
                sb.Append("        WHEN 6 THEN 'Semi-Annually' \n");
                sb.Append("        WHEN 7 THEN 'Annually' \n");
                sb.Append("        WHEN 8 THEN 'One Time' \n");
                sb.Append("        WHEN 9 THEN '3 Times a Year' \n");
                sb.Append("        WHEN 10 THEN 'Every 2 Year' \n");
                sb.Append("        WHEN 11 THEN 'Every 3 Year' \n");
                sb.Append("        WHEN 12 THEN 'Every 5 Year' \n");
                sb.Append("        WHEN 13 THEN 'Every 7 Year' \n");
                sb.Append("        WHEN 14 THEN 'On-Demand' \n");
                sb.Append("    END) AS FrequencyName \n");
                sb.Append("FROM EquipTItem i \n");
                sb.Append("	INNER JOIN EquipTemp et ON et.ID = i.EquipT \n");
                sb.Append("	INNER JOIN Elev e ON e.ID = i.Elev \n");
                sb.Append("	INNER JOIN Loc l ON l.Loc = e.Loc \n");
                sb.Append("	LEFT JOIN Route r ON r.ID = l.Route \n");
                sb.Append("WHERE i.NextDateDue < GETDATE() \n");
                sb.Append("ORDER BY l.Tag, e.Unit, et.fdesc \n");

                return _GetPastDueMCPData.Ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetMonthlyRecurringHours(Chart objChart, string routes)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT \n");
                sb.Append("	temp1.Job,\n");
                sb.Append("	temp1.Loc,  \n");
                sb.Append("	temp1.LocID, \n");
                sb.Append("	temp1.Tag, \n");
                sb.Append("	temp1.Address, \n");
                sb.Append("	temp1.City, \n");
                sb.Append("	temp1.State, \n");
                sb.Append("	temp1.Zip, \n");
                sb.Append("	temp1.MonthlyHours, \n");
                sb.Append("	temp1.RouteName, \n");
                sb.Append("	temp1.MechName, \n");
                sb.Append("	ISNULL(temp2.Total,0) AS ActualHours, \n");
                sb.Append("	(SELECT Count(*) FROM Elev WHERE Loc = temp1.Loc AND Status = 0) AS EquipCount \n");
                sb.Append("FROM( \n");
                sb.Append("	SELECT \n");
                sb.Append("		c.Job, \n");
                sb.Append("		l.Loc, \n");
                sb.Append("		l.ID AS LocID, \n");
                sb.Append("		l.Tag, \n");
                sb.Append("		l.Address, \n");
                sb.Append("		l.City, \n");
                sb.Append("		l.State, \n");
                sb.Append("		l.Zip, \n");
                sb.Append("		rou.Name AS RouteName, \n");
                sb.Append("		w.fDesc AS MechName, \n");
                sb.Append("		SUM(Round (CASE c.SCycle  \n");
                sb.Append("			WHEN 0 THEN c.Hours --Monthly  \n");
                sb.Append("			WHEN 1 THEN c.Hours / 2 --Bi-Monthly  \n");
                sb.Append("			WHEN 2 THEN c.Hours / 3 --Quarterly  \n");
                sb.Append("			WHEN 3 THEN c.Hours / 6 --Semi-Anually  \n");
                sb.Append("			WHEN 4 THEN c.Hours / 12 --Anually  \n");
                sb.Append("			WHEN 5 THEN (c.Hours * 4.3) --Weekly  \n");
                sb.Append("			WHEN 6 THEN (c.Hours * (2.15))  --Bi-Weekly  \n");
                sb.Append("			WHEN 7 THEN ( c.Hours / ( 2.9898 ) ) --Every 13 Weeks  \n");
                sb.Append("			WHEN 10 THEN c.Hours / 12*2 --Every 2 Years  \n");
                sb.Append("			WHEN 8 THEN c.Hours / 12*3 --Every 3 Years  \n");
                sb.Append("			WHEN 9 THEN c.Hours / 12*5 --Every 5 Years  \n");
                sb.Append("			WHEN 11 THEN c.Hours / 12*7 --Every 7 Years  \n");
                sb.Append("			WHEN 13 THEN (c.Hours * ( CASE c.SWE WHEN 1 THEN 30 ELSE 21.66 END) ) --Daily  \n");
                sb.Append("			WHEN 14 THEN (c.Hours * 2) --Twice a Month  \n");
                sb.Append("			else 0   \n");
                sb.Append("		END, 2)) AS MonthlyHours \n");
                sb.Append("	FROM   Job j  \n");
                sb.Append("		   INNER JOIN Contract c ON j.ID = c.Job  \n");
                sb.Append("		   INNER JOIN Loc l ON l.Loc = c.Loc  \n");
                sb.Append("		   LEFT JOIN Route rou ON rou.ID = l.Route \n");
                sb.Append("		   LEFT JOIN tblWork w ON w.ID = rou.Mech \n");
                sb.Append("	WHERE  j.Type = 0 AND l.Status = 0 \n");

                if (!string.IsNullOrEmpty(routes))
                {
                    sb.Append("	AND rou.ID IN (" + routes + ") \n");
                }

                sb.Append("	GROUP BY c.Job, l.Loc, l.ID, l.Tag,l.Address, l.City, l.State, l.Zip, rou.Name, w.fDesc) AS temp1 \n");
                sb.Append("LEFT  JOIN ( \n");
                sb.Append("	SELECT temp.Job, SUM(temp.Total) AS Total FROM ( \n");
                sb.Append("		SELECT\n");
                sb.Append("			TicketDPDA.Job,\n");
                sb.Append("			SUM(Total) as Total \n");
                sb.Append("		FROM TicketDPDA \n");
                sb.Append("		WHERE   EDate >= '" + objChart.StartDate + "'  and EDate <= '" + objChart.EndDate + "' \n");
                sb.Append("		AND (Cat = 'Maintenance' OR Cat = 'Recurring') \n");
                sb.Append("		GROUP BY Job \n");
                sb.Append("		UNION ALL \n");
                sb.Append("		SELECT  \n");
                sb.Append("			Job,\n");
                sb.Append("			SUM(Total) as Total \n");
                sb.Append("		FROM TicketD  \n");
                sb.Append("		WHERE   EDate >= '" + objChart.StartDate + "'  and EDate <= '" + objChart.EndDate + "' \n");
                sb.Append("		AND (Cat = 'Maintenance' OR Cat = 'Recurring') \n");
                sb.Append("		GROUP BY Job) AS temp \n");
                sb.Append("	GROUP BY temp.Job \n");
                sb.Append(") AS temp2 ON temp1.Job = temp2.Job \n");
                sb.Append("ORDER BY temp1.Address \n");

                return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetMonthlyRecurringHours(GetMonthlyRecurringHoursParam _GetMonthlyRecurringHours, string ConnectionString, string routes)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT \n");
                sb.Append("	temp1.Job,\n");
                sb.Append("	temp1.Loc,  \n");
                sb.Append("	temp1.LocID, \n");
                sb.Append("	temp1.Tag, \n");
                sb.Append("	temp1.Address, \n");
                sb.Append("	temp1.City, \n");
                sb.Append("	temp1.State, \n");
                sb.Append("	temp1.Zip, \n");
                sb.Append("	temp1.MonthlyHours, \n");
                sb.Append("	temp1.RouteName, \n");
                sb.Append("	temp1.MechName, \n");
                sb.Append("	ISNULL(temp2.Total,0) AS ActualHours, \n");
                sb.Append("	(SELECT Count(*) FROM Elev WHERE Loc = temp1.Loc AND Status = 0) AS EquipCount \n");
                sb.Append("FROM( \n");
                sb.Append("	SELECT \n");
                sb.Append("		c.Job, \n");
                sb.Append("		l.Loc, \n");
                sb.Append("		l.ID AS LocID, \n");
                sb.Append("		l.Tag, \n");
                sb.Append("		l.Address, \n");
                sb.Append("		l.City, \n");
                sb.Append("		l.State, \n");
                sb.Append("		l.Zip, \n");
                sb.Append("		rou.Name AS RouteName, \n");
                sb.Append("		w.fDesc AS MechName, \n");
                sb.Append("		SUM(Round (CASE c.SCycle  \n");
                sb.Append("			WHEN 0 THEN c.Hours --Monthly  \n");
                sb.Append("			WHEN 1 THEN c.Hours / 2 --Bi-Monthly  \n");
                sb.Append("			WHEN 2 THEN c.Hours / 3 --Quarterly  \n");
                sb.Append("			WHEN 3 THEN c.Hours / 6 --Semi-Anually  \n");
                sb.Append("			WHEN 4 THEN c.Hours / 12 --Anually  \n");
                sb.Append("			WHEN 5 THEN (c.Hours * 4.3) --Weekly  \n");
                sb.Append("			WHEN 6 THEN (c.Hours * (2.15))  --Bi-Weekly  \n");
                sb.Append("			WHEN 7 THEN ( c.Hours / ( 2.9898 ) ) --Every 13 Weeks  \n");
                sb.Append("			WHEN 10 THEN c.Hours / 12*2 --Every 2 Years  \n");
                sb.Append("			WHEN 8 THEN c.Hours / 12*3 --Every 3 Years  \n");
                sb.Append("			WHEN 9 THEN c.Hours / 12*5 --Every 5 Years  \n");
                sb.Append("			WHEN 11 THEN c.Hours / 12*7 --Every 7 Years  \n");
                sb.Append("			WHEN 13 THEN (c.Hours * ( CASE c.SWE WHEN 1 THEN 30 ELSE 21.66 END) ) --Daily  \n");
                sb.Append("			WHEN 14 THEN (c.Hours * 2) --Twice a Month  \n");
                sb.Append("			else 0   \n");
                sb.Append("		END, 2)) AS MonthlyHours \n");
                sb.Append("	FROM   Job j  \n");
                sb.Append("		   INNER JOIN Contract c ON j.ID = c.Job  \n");
                sb.Append("		   INNER JOIN Loc l ON l.Loc = c.Loc  \n");
                sb.Append("		   LEFT JOIN Route rou ON rou.ID = l.Route \n");
                sb.Append("		   LEFT JOIN tblWork w ON w.ID = rou.Mech \n");
                sb.Append("	WHERE  j.Type = 0 AND l.Status = 0 \n");

                if (!string.IsNullOrEmpty(routes))
                {
                    sb.Append("	AND rou.ID IN (" + routes + ") \n");
                }

                sb.Append("	GROUP BY c.Job, l.Loc, l.ID, l.Tag,l.Address, l.City, l.State, l.Zip, rou.Name, w.fDesc) AS temp1 \n");
                sb.Append("LEFT  JOIN ( \n");
                sb.Append("	SELECT temp.Job, SUM(temp.Total) AS Total FROM ( \n");
                sb.Append("		SELECT\n");
                sb.Append("			TicketDPDA.Job,\n");
                sb.Append("			SUM(Total) as Total \n");
                sb.Append("		FROM TicketDPDA \n");
                sb.Append("		WHERE   EDate >= '" + _GetMonthlyRecurringHours.StartDate + "'  and EDate <= '" + _GetMonthlyRecurringHours.EndDate + "' \n");
                sb.Append("		AND (Cat = 'Maintenance' OR Cat = 'Recurring') \n");
                sb.Append("		GROUP BY Job \n");
                sb.Append("		UNION ALL \n");
                sb.Append("		SELECT  \n");
                sb.Append("			Job,\n");
                sb.Append("			SUM(Total) as Total \n");
                sb.Append("		FROM TicketD  \n");
                sb.Append("		WHERE   EDate >= '" + _GetMonthlyRecurringHours.StartDate + "'  and EDate <= '" + _GetMonthlyRecurringHours.EndDate + "' \n");
                sb.Append("		AND (Cat = 'Maintenance' OR Cat = 'Recurring') \n");
                sb.Append("		GROUP BY Job) AS temp \n");
                sb.Append("	GROUP BY temp.Job \n");
                sb.Append(") AS temp2 ON temp1.Job = temp2.Job \n");
                sb.Append("ORDER BY temp1.Address \n");

                return _GetMonthlyRecurringHours.Ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetMonthlyRecurringHoursTEI(Chart objChart, string routes, bool isRecurring = true)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT  \n");
                sb.Append("	temp1.ID, \n");
                sb.Append("	temp1.Job, \n");
                sb.Append("	temp1.Loc,   \n");
                sb.Append("	temp1.LocID,  \n");
                sb.Append("	temp1.Tag,  \n");
                sb.Append("	temp1.Address,  \n");
                sb.Append("	temp1.City,  \n");
                sb.Append("	temp1.State,  \n");
                sb.Append("	temp1.Zip,  \n");
                sb.Append("	temp1.MonthlyHours,	 \n");
                sb.Append("	temp1.RouteName, \n");
                sb.Append("	temp1.MechName, \n");
                sb.Append("	(SELECT  \n");
                sb.Append("		STUFF((SELECT DISTINCT ', ' + CAST(Cat AS VARCHAR(1000))  \n");
                sb.Append("			FROM ( \n");
                sb.Append("				SELECT \n");
                sb.Append("					Cat  \n");
                sb.Append("				FROM TicketDPDA  \n");
                sb.Append("				WHERE Job = temp1.ID AND EDate >= '" + objChart.StartDate + "' AND EDate <= '" + objChart.EndDate + "'  \n");
                sb.Append("					AND (Cat = 'Maintenance'  \n");
                sb.Append("					OR Cat = 'Cat 1 Test Non Billable'  \n");
                sb.Append("					OR Cat = 'Cat 5 Test Non BIllable'  \n");
                sb.Append("					OR Cat = 'Cat Def Work'  \n");
                sb.Append("					OR Cat = 'Consult Rpt'  \n");
                sb.Append("					OR Cat = 'ECB' \n");
                sb.Append("					OR Cat = 'Maint-Cov-Rep' \n");
                sb.Append("			        OR Cat = 'MR - Non-Billable' \n");
                sb.Append("					OR Cat = 'Non-Billable Repair' \n");
                sb.Append("					OR Cat = 'Follow Up' \n");
                sb.Append("					OR Cat = 'PVT')  \n");
                sb.Append("				UNION ALL \n");
                sb.Append("				SELECT \n");
                sb.Append("					Cat  \n");
                sb.Append("				FROM TicketD \n");
                sb.Append("				WHERE Job = temp1.ID AND EDate >= '" + objChart.StartDate + "' AND EDate <= '" + objChart.EndDate + "'  \n");
                sb.Append("					AND (Cat = 'Maintenance'  \n");
                sb.Append("					OR Cat = 'Cat 1 Test Non Billable'  \n");
                sb.Append("					OR Cat = 'Cat 5 Test Non BIllable'  \n");
                sb.Append("					OR Cat = 'Cat Def Work'  \n");
                sb.Append("					OR Cat = 'Consult Rpt'  \n");
                sb.Append("					OR Cat = 'ECB' \n");
                sb.Append("					OR Cat = 'Maint-Cov-Rep' \n");
                sb.Append("			        OR Cat = 'MR - Non-Billable' \n");
                sb.Append("					OR Cat = 'Non-Billable Repair' \n");
                sb.Append("					OR Cat = 'Follow Up' \n");
                sb.Append("					OR Cat = 'PVT')  \n");
                sb.Append("			) AS t1 \n");
                sb.Append("		FOR XML PATH(''), Type).value('.', 'VARCHAR(1000)'),1,2,' ')) AS Cat,	  \n");
                sb.Append("	(SELECT   \n");
                sb.Append("		STUFF((SELECT DISTINCT ', ' + CAST([Type] AS VARCHAR(1000))   \n");
                sb.Append("			FROM (  \n");
                sb.Append("				SELECT  \n");
                sb.Append("					jt.Type   \n");
                sb.Append("				FROM TicketDPDA dp \n");
                sb.Append("				INNER JOIN TicketO t ON dp.ID = t.ID \n");
                sb.Append("				INNER JOIN JobType jt ON t.Type = jt.ID  \n");
                sb.Append("				WHERE dp.Job = temp1.ID AND dp.EDate >= '" + objChart.StartDate + "' AND dp.EDate <= '" + objChart.EndDate + "'   \n");
                sb.Append("					AND (dp.Cat = 'Maintenance'   \n");
                sb.Append("					OR dp.Cat = 'Cat 1 Test Non Billable'   \n");
                sb.Append("					OR dp.Cat = 'Cat 5 Test Non BIllable'   \n");
                sb.Append("					OR dp.Cat = 'Cat Def Work'   \n");
                sb.Append("					OR dp.Cat = 'Consult Rpt'   \n");
                sb.Append("					OR dp.Cat = 'ECB'  \n");
                sb.Append("					OR dp.Cat = 'Maint-Cov-Rep'  \n");
                sb.Append("			        OR dp.Cat = 'MR - Non-Billable'  \n");
                sb.Append("					OR dp.Cat = 'Non-Billable Repair'  \n");
                sb.Append("					OR dp.Cat = 'Follow Up'  \n");
                sb.Append("					OR dp.Cat = 'PVT')   \n");
                sb.Append("				UNION ALL  \n");
                sb.Append("				SELECT  \n");
                sb.Append("					jt.Type  \n");
                sb.Append("				FROM TicketD t \n");
                sb.Append("				INNER JOIN JobType jt ON t.Type = jt.ID  \n");
                sb.Append("				WHERE Job = temp1.ID AND EDate >= '" + objChart.StartDate + "' AND EDate <= '" + objChart.EndDate + "'   \n");
                sb.Append("					AND (Cat = 'Maintenance'   \n");
                sb.Append("					OR Cat = 'Cat 1 Test Non Billable'   \n");
                sb.Append("					OR Cat = 'Cat 5 Test Non BIllable'   \n");
                sb.Append("					OR Cat = 'Cat Def Work'   \n");
                sb.Append("					OR Cat = 'Consult Rpt'   \n");
                sb.Append("					OR Cat = 'ECB'  \n");
                sb.Append("					OR Cat = 'Maint-Cov-Rep'  \n");
                sb.Append("			        OR Cat = 'MR - Non-Billable'  \n");
                sb.Append("					OR Cat = 'Non-Billable Repair'  \n");
                sb.Append("					OR Cat = 'Follow Up'  \n");
                sb.Append("					OR Cat = 'PVT')   \n");
                sb.Append("			) AS t1  \n");
                sb.Append("		FOR XML PATH(''), Type).value('.', 'VARCHAR(1000)'),1,2,' ')) AS Department, \n");
                sb.Append("	ISNULL(temp2.Total,0) AS ActualHours,  \n");
                sb.Append("	(SELECT Count(*) FROM Elev WHERE Loc = temp1.Loc AND Status = 0) AS EquipCount  \n");
                sb.Append("FROM(  \n");
                sb.Append("	SELECT  \n");
                sb.Append("		j.ID,  \n");
                sb.Append("		c.Job,  \n");
                sb.Append("		l.Loc,  \n");
                sb.Append("		l.ID AS LocID,  \n");
                sb.Append("		l.Tag,  \n");
                sb.Append("		l.Address,  \n");
                sb.Append("		l.City,  \n");
                sb.Append("		l.State,  \n");
                sb.Append("		l.Zip, \n");
                sb.Append("		rou.Name AS RouteName, \n");
                sb.Append("		w.fDesc AS MechName, \n");
                sb.Append("		SUM(Round (CASE c.SCycle   \n");
                sb.Append("			WHEN 0 THEN c.Hours --Monthly   \n");
                sb.Append("			WHEN 1 THEN c.Hours / 2 --Bi-Monthly   \n");
                sb.Append("			WHEN 2 THEN c.Hours / 3 --Quarterly   \n");
                sb.Append("			WHEN 3 THEN c.Hours / 6 --Semi-Anually   \n");
                sb.Append("			WHEN 4 THEN c.Hours / 12 --Anually   \n");
                sb.Append("			WHEN 5 THEN (c.Hours * 4.3) --Weekly   \n");
                sb.Append("			WHEN 6 THEN (c.Hours * (2.15))  --Bi-Weekly   \n");
                sb.Append("			WHEN 7 THEN ( c.Hours / ( 2.9898 ) ) --Every 13 Weeks   \n");
                sb.Append("			WHEN 10 THEN c.Hours / 12*2 --Every 2 Years   \n");
                sb.Append("			WHEN 8 THEN c.Hours / 12*3 --Every 3 Years   \n");
                sb.Append("			WHEN 9 THEN c.Hours / 12*5 --Every 5 Years   \n");
                sb.Append("			WHEN 11 THEN c.Hours / 12*7 --Every 7 Years   \n");
                sb.Append("			WHEN 13 THEN (c.Hours * ( CASE c.SWE WHEN 1 THEN 30 ELSE 21.66 END) ) --Daily   \n");
                sb.Append("			WHEN 14 THEN (c.Hours * 2) --Twice a Month   \n");
                sb.Append("			else 0    \n");
                sb.Append("		END, 2)) AS MonthlyHours  \n");
                sb.Append("	FROM   Job j   \n");
                sb.Append("		   INNER JOIN Loc l ON l.Loc = j.Loc   \n");
                sb.Append("		   LEFT JOIN Contract c ON j.ID = c.Job   \n");
                sb.Append("		   LEFT JOIN Route rou ON rou.ID = l.Route \n");
                sb.Append("		   LEFT JOIN tblWork w ON w.ID = rou.Mech \n");
                sb.Append("	WHERE  j.Status = 0 AND l.Status = 0  \n");

                if (isRecurring)
                {
                    sb.Append("	AND j.Type = 0 AND j.Loc IN (SELECT DISTINCT Loc FROM Contract WHERE Status = 0) \n");
                }

                if (!string.IsNullOrEmpty(routes))
                {
                    sb.Append("	AND rou.ID IN (" + routes + ") \n");
                }

                sb.Append("	GROUP BY j.ID, c.Job, l.Loc, l.ID, l.Tag,l.Address, l.City, l.State, l.Zip, rou.Name, w.fDesc) AS temp1  \n");
                sb.Append("LEFT  JOIN (  \n");
                sb.Append("	SELECT temp.Job, SUM(temp.Total) AS Total FROM (  \n");
                sb.Append("		SELECT \n");
                sb.Append("			TicketDPDA.Job, \n");
                sb.Append("			SUM(Total) as Total  \n");
                sb.Append("		FROM TicketDPDA  \n");
                sb.Append("		WHERE   EDate >= '" + objChart.StartDate + "'  and EDate <= '" + objChart.EndDate + "'  \n");
                sb.Append("		AND (Cat = 'Maintenance'  \n");
                sb.Append("			OR Cat = 'Cat 1 Test Non Billable'  \n");
                sb.Append("			OR Cat = 'Cat 5 Test Non BIllable'  \n");
                sb.Append("			OR Cat = 'Cat Def Work'  \n");
                sb.Append("			OR Cat = 'Consult Rpt'  \n");
                sb.Append("			OR Cat = 'ECB' \n");
                sb.Append("			OR Cat = 'Maint-Cov-Rep' \n");
                sb.Append("			OR Cat = 'MR - Non-Billable' \n");
                sb.Append("			OR Cat = 'Non-Billable Repair' \n");
                sb.Append("			OR Cat = 'Follow Up' \n");
                sb.Append("			OR Cat = 'PVT')  \n");
                sb.Append("		GROUP BY Job  \n");
                sb.Append("		UNION ALL  \n");
                sb.Append("		SELECT   \n");
                sb.Append("			Job, \n");
                sb.Append("			SUM(Total) as Total  \n");
                sb.Append("		FROM TicketD   \n");
                sb.Append("		WHERE   EDate >= '" + objChart.StartDate + "'  and EDate <= '" + objChart.EndDate + "'  \n");
                sb.Append("		AND (Cat = 'Maintenance'  \n");
                sb.Append("			OR Cat = 'Cat 1 Test Non Billable'  \n");
                sb.Append("			OR Cat = 'Cat 5 Test Non BIllable'  \n");
                sb.Append("			OR Cat = 'Cat Def Work'  \n");
                sb.Append("			OR Cat = 'Consult Rpt'  \n");
                sb.Append("			OR Cat = 'ECB' \n");
                sb.Append("			OR Cat = 'Maint-Cov-Rep' \n");
                sb.Append("			OR Cat = 'MR - Non-Billable' \n");
                sb.Append("			OR Cat = 'Non-Billable Repair' \n");
                sb.Append("			OR Cat = 'Follow Up' \n");
                sb.Append("			OR Cat = 'PVT')  \n");
                sb.Append("		GROUP BY Job) AS temp  \n");
                sb.Append("	GROUP BY temp.Job  \n");
                sb.Append(") AS temp2 ON temp1.ID = temp2.Job  \n");
                sb.Append("WHERE temp1.Job IS NOT NULL OR temp2.Total <> 0  \n");
                sb.Append("ORDER BY temp1.Address  \n");

                return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetMonthlyRecurringHoursTEI(GetMonthlyRecurringHoursTEIParam _GetMonthlyRecurringHoursTEI, string ConnectionString, string routes)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT  \n");
                sb.Append("	temp1.ID, \n");
                sb.Append("	temp1.Job, \n");
                sb.Append("	temp1.Loc,   \n");
                sb.Append("	temp1.LocID,  \n");
                sb.Append("	temp1.Tag,  \n");
                sb.Append("	temp1.Address,  \n");
                sb.Append("	temp1.City,  \n");
                sb.Append("	temp1.State,  \n");
                sb.Append("	temp1.Zip,  \n");
                sb.Append("	temp1.MonthlyHours,	 \n");
                sb.Append("	temp1.RouteName, \n");
                sb.Append("	temp1.MechName, \n");
                sb.Append("	(SELECT  \n");
                sb.Append("		STUFF((SELECT DISTINCT ', ' + CAST(Cat AS VARCHAR(1000))  \n");
                sb.Append("			FROM ( \n");
                sb.Append("				SELECT \n");
                sb.Append("					Cat  \n");
                sb.Append("				FROM TicketDPDA  \n");
                sb.Append("				WHERE Job = temp1.ID AND EDate >= '" + _GetMonthlyRecurringHoursTEI.StartDate + "' AND EDate <= '" + _GetMonthlyRecurringHoursTEI.EndDate + "'  \n");
                sb.Append("					AND (Cat = 'Maintenance'  \n");
                sb.Append("					OR Cat = 'Cat 1 Test Non Billable'  \n");
                sb.Append("					OR Cat = 'Cat 5 Test Non BIllable'  \n");
                sb.Append("					OR Cat = 'Cat Def Work'  \n");
                sb.Append("					OR Cat = 'Consult Rpt'  \n");
                sb.Append("					OR Cat = 'ECB' \n");
                sb.Append("					OR Cat = 'Maint-Cov-Rep' \n");
                sb.Append("			        OR Cat = 'MR - Non-Billable' \n");
                sb.Append("					OR Cat = 'Non-Billable Repair' \n");
                sb.Append("					OR Cat = 'Follow Up' \n");
                sb.Append("					OR Cat = 'PVT')  \n");
                sb.Append("				UNION ALL \n");
                sb.Append("				SELECT \n");
                sb.Append("					Cat  \n");
                sb.Append("				FROM TicketD \n");
                sb.Append("				WHERE Job = temp1.ID AND EDate >= '" + _GetMonthlyRecurringHoursTEI.StartDate + "' AND EDate <= '" + _GetMonthlyRecurringHoursTEI.EndDate + "'  \n");
                sb.Append("					AND (Cat = 'Maintenance'  \n");
                sb.Append("					OR Cat = 'Cat 1 Test Non Billable'  \n");
                sb.Append("					OR Cat = 'Cat 5 Test Non BIllable'  \n");
                sb.Append("					OR Cat = 'Cat Def Work'  \n");
                sb.Append("					OR Cat = 'Consult Rpt'  \n");
                sb.Append("					OR Cat = 'ECB' \n");
                sb.Append("					OR Cat = 'Maint-Cov-Rep' \n");
                sb.Append("			        OR Cat = 'MR - Non-Billable' \n");
                sb.Append("					OR Cat = 'Non-Billable Repair' \n");
                sb.Append("					OR Cat = 'Follow Up' \n");
                sb.Append("					OR Cat = 'PVT')  \n");
                sb.Append("			) AS t1 \n");
                sb.Append("		FOR XML PATH(''), Type).value('.', 'VARCHAR(1000)'),1,2,' ')) AS Cat,	  \n");
                sb.Append("	(SELECT   \n");
                sb.Append("		STUFF((SELECT DISTINCT ', ' + CAST([Type] AS VARCHAR(1000))   \n");
                sb.Append("			FROM (  \n");
                sb.Append("				SELECT  \n");
                sb.Append("					jt.Type   \n");
                sb.Append("				FROM TicketDPDA dp \n");
                sb.Append("				INNER JOIN TicketO t ON dp.ID = t.ID \n");
                sb.Append("				INNER JOIN JobType jt ON t.Type = jt.ID  \n");
                sb.Append("				WHERE dp.Job = temp1.ID AND dp.EDate >= '" + _GetMonthlyRecurringHoursTEI.StartDate + "' AND dp.EDate <= '" + _GetMonthlyRecurringHoursTEI.EndDate + "'   \n");
                sb.Append("					AND (dp.Cat = 'Maintenance'   \n");
                sb.Append("					OR dp.Cat = 'Cat 1 Test Non Billable'   \n");
                sb.Append("					OR dp.Cat = 'Cat 5 Test Non BIllable'   \n");
                sb.Append("					OR dp.Cat = 'Cat Def Work'   \n");
                sb.Append("					OR dp.Cat = 'Consult Rpt'   \n");
                sb.Append("					OR dp.Cat = 'ECB'  \n");
                sb.Append("					OR dp.Cat = 'Maint-Cov-Rep'  \n");
                sb.Append("			        OR dp.Cat = 'MR - Non-Billable'  \n");
                sb.Append("					OR dp.Cat = 'Non-Billable Repair'  \n");
                sb.Append("					OR dp.Cat = 'Follow Up'  \n");
                sb.Append("					OR dp.Cat = 'PVT')   \n");
                sb.Append("				UNION ALL  \n");
                sb.Append("				SELECT  \n");
                sb.Append("					jt.Type  \n");
                sb.Append("				FROM TicketD t \n");
                sb.Append("				INNER JOIN JobType jt ON t.Type = jt.ID  \n");
                sb.Append("				WHERE Job = temp1.ID AND EDate >= '" + _GetMonthlyRecurringHoursTEI.StartDate + "' AND EDate <= '" + _GetMonthlyRecurringHoursTEI.EndDate + "'   \n");
                sb.Append("					AND (Cat = 'Maintenance'   \n");
                sb.Append("					OR Cat = 'Cat 1 Test Non Billable'   \n");
                sb.Append("					OR Cat = 'Cat 5 Test Non BIllable'   \n");
                sb.Append("					OR Cat = 'Cat Def Work'   \n");
                sb.Append("					OR Cat = 'Consult Rpt'   \n");
                sb.Append("					OR Cat = 'ECB'  \n");
                sb.Append("					OR Cat = 'Maint-Cov-Rep'  \n");
                sb.Append("			        OR Cat = 'MR - Non-Billable'  \n");
                sb.Append("					OR Cat = 'Non-Billable Repair'  \n");
                sb.Append("					OR Cat = 'Follow Up'  \n");
                sb.Append("					OR Cat = 'PVT')   \n");
                sb.Append("			) AS t1  \n");
                sb.Append("		FOR XML PATH(''), Type).value('.', 'VARCHAR(1000)'),1,2,' ')) AS Department, \n");
                sb.Append("	ISNULL(temp2.Total,0) AS ActualHours,  \n");
                sb.Append("	(SELECT Count(*) FROM Elev WHERE Loc = temp1.Loc AND Status = 0) AS EquipCount  \n");
                sb.Append("FROM(  \n");
                sb.Append("	SELECT  \n");
                sb.Append("		j.ID,  \n");
                sb.Append("		c.Job,  \n");
                sb.Append("		l.Loc,  \n");
                sb.Append("		l.ID AS LocID,  \n");
                sb.Append("		l.Tag,  \n");
                sb.Append("		l.Address,  \n");
                sb.Append("		l.City,  \n");
                sb.Append("		l.State,  \n");
                sb.Append("		l.Zip, \n");
                sb.Append("		rou.Name AS RouteName, \n");
                sb.Append("		w.fDesc AS MechName, \n");
                sb.Append("		SUM(Round (CASE c.SCycle   \n");
                sb.Append("			WHEN 0 THEN c.Hours --Monthly   \n");
                sb.Append("			WHEN 1 THEN c.Hours / 2 --Bi-Monthly   \n");
                sb.Append("			WHEN 2 THEN c.Hours / 3 --Quarterly   \n");
                sb.Append("			WHEN 3 THEN c.Hours / 6 --Semi-Anually   \n");
                sb.Append("			WHEN 4 THEN c.Hours / 12 --Anually   \n");
                sb.Append("			WHEN 5 THEN (c.Hours * 4.3) --Weekly   \n");
                sb.Append("			WHEN 6 THEN (c.Hours * (2.15))  --Bi-Weekly   \n");
                sb.Append("			WHEN 7 THEN ( c.Hours / ( 2.9898 ) ) --Every 13 Weeks   \n");
                sb.Append("			WHEN 10 THEN c.Hours / 12*2 --Every 2 Years   \n");
                sb.Append("			WHEN 8 THEN c.Hours / 12*3 --Every 3 Years   \n");
                sb.Append("			WHEN 9 THEN c.Hours / 12*5 --Every 5 Years   \n");
                sb.Append("			WHEN 11 THEN c.Hours / 12*7 --Every 7 Years   \n");
                sb.Append("			WHEN 13 THEN (c.Hours * ( CASE c.SWE WHEN 1 THEN 30 ELSE 21.66 END) ) --Daily   \n");
                sb.Append("			WHEN 14 THEN (c.Hours * 2) --Twice a Month   \n");
                sb.Append("			else 0    \n");
                sb.Append("		END, 2)) AS MonthlyHours  \n");
                sb.Append("	FROM   Job j   \n");
                sb.Append("		   INNER JOIN Loc l ON l.Loc = j.Loc   \n");
                sb.Append("		   LEFT JOIN Contract c ON j.ID = c.Job   \n");
                sb.Append("		   LEFT JOIN Route rou ON rou.ID = l.Route \n");
                sb.Append("		   LEFT JOIN tblWork w ON w.ID = rou.Mech \n");
                sb.Append("	WHERE  j.Type = 0 AND j.Status = 0 AND l.Status = 0 AND j.Loc IN (SELECT DISTINCT Loc FROM Contract WHERE Status = 0)  \n");

                if (!string.IsNullOrEmpty(routes))
                {
                    sb.Append("	AND rou.ID IN (" + routes + ") \n");
                }

                sb.Append("	GROUP BY j.ID, c.Job, l.Loc, l.ID, l.Tag,l.Address, l.City, l.State, l.Zip, rou.Name, w.fDesc) AS temp1  \n");
                sb.Append("LEFT  JOIN (  \n");
                sb.Append("	SELECT temp.Job, SUM(temp.Total) AS Total FROM (  \n");
                sb.Append("		SELECT \n");
                sb.Append("			TicketDPDA.Job, \n");
                sb.Append("			SUM(Total) as Total  \n");
                sb.Append("		FROM TicketDPDA  \n");
                sb.Append("		WHERE   EDate >= '" + _GetMonthlyRecurringHoursTEI.StartDate + "'  and EDate <= '" + _GetMonthlyRecurringHoursTEI.EndDate + "'  \n");
                sb.Append("		AND (Cat = 'Maintenance'  \n");
                sb.Append("			OR Cat = 'Cat 1 Test Non Billable'  \n");
                sb.Append("			OR Cat = 'Cat 5 Test Non BIllable'  \n");
                sb.Append("			OR Cat = 'Cat Def Work'  \n");
                sb.Append("			OR Cat = 'Consult Rpt'  \n");
                sb.Append("			OR Cat = 'ECB' \n");
                sb.Append("			OR Cat = 'Maint-Cov-Rep' \n");
                sb.Append("			OR Cat = 'MR - Non-Billable' \n");
                sb.Append("			OR Cat = 'Non-Billable Repair' \n");
                sb.Append("			OR Cat = 'Follow Up' \n");
                sb.Append("			OR Cat = 'PVT')  \n");
                sb.Append("		GROUP BY Job  \n");
                sb.Append("		UNION ALL  \n");
                sb.Append("		SELECT   \n");
                sb.Append("			Job, \n");
                sb.Append("			SUM(Total) as Total  \n");
                sb.Append("		FROM TicketD   \n");
                sb.Append("		WHERE   EDate >= '" + _GetMonthlyRecurringHoursTEI.StartDate + "'  and EDate <= '" + _GetMonthlyRecurringHoursTEI.EndDate + "'  \n");
                sb.Append("		AND (Cat = 'Maintenance'  \n");
                sb.Append("			OR Cat = 'Cat 1 Test Non Billable'  \n");
                sb.Append("			OR Cat = 'Cat 5 Test Non BIllable'  \n");
                sb.Append("			OR Cat = 'Cat Def Work'  \n");
                sb.Append("			OR Cat = 'Consult Rpt'  \n");
                sb.Append("			OR Cat = 'ECB' \n");
                sb.Append("			OR Cat = 'Maint-Cov-Rep' \n");
                sb.Append("			OR Cat = 'MR - Non-Billable' \n");
                sb.Append("			OR Cat = 'Non-Billable Repair' \n");
                sb.Append("			OR Cat = 'Follow Up' \n");
                sb.Append("			OR Cat = 'PVT')  \n");
                sb.Append("		GROUP BY Job) AS temp  \n");
                sb.Append("	GROUP BY temp.Job  \n");
                sb.Append(") AS temp2 ON temp1.ID = temp2.Job  \n");
                sb.Append("WHERE temp1.Job IS NOT NULL OR temp2.Total <> 0  \n");
                sb.Append("ORDER BY temp1.Address  \n");

                return _GetMonthlyRecurringHoursTEI.Ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetChecksReportData(MapData objPropMapData, List<RetainFilter> filters)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT  \n");
                sb.Append("	c.ID,  \n");
                sb.Append("	c.TransID,  \n");
                sb.Append("	c.fDate,  \n");
                sb.Append("	c.Ref,  \n");
                sb.Append("	c.fDesc,  \n");
                sb.Append("	c.Amount,  \n");
                sb.Append("	c.Vendor,  \n");
                sb.Append("	r.Name AS VendorName,  \n");
                sb.Append("	c.Bank,  \n");
                sb.Append("	b.fDesc AS BankName,  \n");
                sb.Append("	t.Batch, \n");
                sb.Append("	CASE c.Type  \n");
                sb.Append("		WHEN 0 THEN 'Check' \n");
                sb.Append("		WHEN 1 THEN 'Cash' \n");
                sb.Append("		WHEN 2 THEN 'Wire Transfer' \n");
                sb.Append("		WHEN 3 THEN 'ACH' \n");
                sb.Append("		WHEN 4 THEN 'Credit Card' \n");
                sb.Append("	END AS TypeName,  \n");
                sb.Append("	c.Status,  \n");
                sb.Append("	c.French,  \n");
                sb.Append("	c.Memo,  \n");
                sb.Append("	c.VoidR,  \n");
                sb.Append("	c.ACH,  \n");
                sb.Append("	ISNULL(tt.Sel,0) AS Sel, \n");
                sb.Append("	tt.Type, \n");
                sb.Append("	CASE  \n");
                sb.Append("WHEN ISNULL(tt.sel, 0) = 0 THEN 'Open' \n");
                sb.Append("WHEN tt.sel = 1 THEN 'Cleared' \n");
                sb.Append("	WHEN tt.sel = 2 THEN 'Voided' \n");
                sb.Append("	END AS StatusName, \n");
                sb.Append("	r.EN, \n");
                sb.Append("	br.Name AS Company \n");
                sb.Append("FROM CD AS c  \n");
                sb.Append("	LEFT JOIN Bank b ON c.Bank = b.ID  \n");
                sb.Append("	LEFT JOIN Vendor v ON v.ID = c.Vendor \n");
                sb.Append("	LEFT JOIN trans t ON c.TransID = t.ID \n");
                sb.Append("	LEFT JOIN Rol r ON r.ID = v.Rol \n");
                sb.Append("	LEFT OUTER JOIN Branch br ON br.ID = r.EN \n");
                sb.Append("	LEFT JOIN ( \n");
                sb.Append(" SELECT ct.ID, t.Batch, t.Sel, t.Type,t.AcctSub \n");
                sb.Append("FROM Trans t \n");
                sb.Append("INNER JOIN ( \n");
                sb.Append("	SELECT c.ID, t.batch  \n");
                sb.Append("FROM trans t INNER JOIN CD c ON t.ID = c.TransID \n");
                sb.Append(") ct ON ct.Batch = t.Batch \n");
                sb.Append("		WHERE Type = 20 And ISNULL(t.Acctsub,0) <> 0 \n");
                sb.Append("	) tt ON tt.Batch = t.Batch AND tt.ID = c.ID \n");

                if (objPropMapData.EN == 1)
                {
                    sb.Append("LEFT OUTER JOIN tblUserCo uc ON uc.CompanyID = r.EN \n");
                }

                sb.Append("WHERE c.fDate >= cast('" + objPropMapData.StartDate + "' as date) AND c.fDate <= cast('" + objPropMapData.EndDate + "' as date) \n");

                if (objPropMapData.EN == 1)
                {
                    sb.Append("AND uc.IsSel = 1 AND uc.UserID = " + objPropMapData.UserID + " \n");
                }

                // Search value
                if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && !string.IsNullOrEmpty(objPropMapData.SearchValue))
                {
                    if (objPropMapData.SearchBy == "Vendor")
                    {
                        sb.Append("AND r.Name LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "CheckNum")
                    {
                        sb.Append("AND c.Ref LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "Status")
                    {
                        sb.Append("AND tt.Sel= " + objPropMapData.SearchValue + " \n");
                    }
                    if (objPropMapData.SearchBy == "PayType")
                    {
                        sb.Append("AND c.Type= " + objPropMapData.SearchValue + " \n");
                    }
                    if (objPropMapData.SearchBy == "Bank")
                    {
                        sb.Append("AND b.fDesc LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                }

                //Ticket filters
                if (filters.Count > 0)
                {
                    sb.Insert(0, "SELECT * FROM ( \n");
                    sb.Append(") AS temp \n");
                    sb.Append("Where 1 = 1 \n");

                    foreach (var filter in filters)
                    {
                        if (filter.FilterColumn == "fDate")
                        {
                            sb.Append("AND FORMAT(temp.fDate, 'M/d/yyyy') LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Ref")
                        {
                            sb.Append("AND temp.Ref LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "fDesc")
                        {
                            sb.Append("AND temp.fDesc LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "BankName")
                        {
                            sb.Append("AND temp.BankName LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "TypeName")
                        {
                            sb.Append("AND temp.TypeName LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "StatusName")
                        {
                            sb.Append("AND temp.StatusName LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Amount")
                        {
                            sb.Append("AND temp.Amount= " + filter.FilterValue + " \n");
                        }
                    }
                }

                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetChecksReportData(GetChecksReportDataParam _GetChecksReportDataParam, string ConnectionString)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT  \n");
                sb.Append("	c.ID,  \n");
                sb.Append("	c.TransID,  \n");
                sb.Append("	c.fDate,  \n");
                sb.Append("	c.Ref,  \n");
                sb.Append("	c.fDesc,  \n");
                sb.Append("	c.Amount,  \n");
                sb.Append("	c.Vendor,  \n");
                sb.Append("	r.Name AS VendorName,  \n");
                sb.Append("	c.Bank,  \n");
                sb.Append("	b.fDesc AS BankName,  \n");
                sb.Append("	t.Batch, \n");
                sb.Append("	CASE c.Type  \n");
                sb.Append("		WHEN 0 THEN 'Check' \n");
                sb.Append("		WHEN 1 THEN 'Cash' \n");
                sb.Append("		WHEN 2 THEN 'Wire Transfer' \n");
                sb.Append("		WHEN 3 THEN 'ACH' \n");
                sb.Append("		WHEN 4 THEN 'Credit Card' \n");
                sb.Append("	END AS TypeName,  \n");
                sb.Append("	c.Status,  \n");
                sb.Append("	c.French,  \n");
                sb.Append("	c.Memo,  \n");
                sb.Append("	c.VoidR,  \n");
                sb.Append("	c.ACH,  \n");
                sb.Append("	ISNULL(tt.Sel,0) AS Sel, \n");
                sb.Append("	tt.Type, \n");
                sb.Append("	CASE  \n");
                sb.Append("		WHEN ISNULL(tt.sel, 0) = 0 THEN 'Open' \n");
                sb.Append("		WHEN tt.sel = 1 THEN 'Cleared' \n");
                sb.Append("		WHEN tt.sel = 2 THEN 'Voided' \n");
                sb.Append("	END AS StatusName, \n");
                sb.Append("	r.EN, \n");
                sb.Append("	br.Name AS Company \n");
                sb.Append("FROM CD AS c  \n");
                sb.Append("	LEFT JOIN Bank b ON c.Bank = b.ID  \n");
                sb.Append("	LEFT JOIN Vendor v ON v.ID = c.Vendor \n");
                sb.Append("	LEFT JOIN trans t ON c.TransID = t.ID \n");
                sb.Append("	LEFT JOIN Rol r ON r.ID = v.Rol \n");
                sb.Append("	LEFT OUTER JOIN Branch br ON br.ID = r.EN \n");
                sb.Append("	LEFT JOIN ( \n");
                sb.Append("		SELECT ct.ID, t.Batch, t.Sel, t.Type \n");
                sb.Append("		FROM Trans t \n");
                sb.Append("			INNER JOIN ( \n");
                sb.Append("				SELECT c.ID, t.batch  \n");
                sb.Append("				FROM trans t INNER JOIN CD c ON t.ID = c.TransID \n");
                sb.Append("			) ct ON ct.Batch = t.Batch \n");
                sb.Append("		WHERE Type = 20  \n");
                sb.Append("	) tt ON tt.Batch = t.Batch AND tt.ID = c.ID \n");

                if (_GetChecksReportDataParam.EN == 1)
                {
                    sb.Append("	LEFT OUTER JOIN tblUserCo uc ON uc.CompanyID = r.EN \n");
                }

                sb.Append("WHERE c.fDate >= '" + _GetChecksReportDataParam.StartDate + "' AND c.fDate <= '" + _GetChecksReportDataParam.EndDate + "' \n");

                if (_GetChecksReportDataParam.EN == 1)
                {
                    sb.Append("	AND uc.IsSel = 1 AND uc.UserID = " + _GetChecksReportDataParam.UserID + " \n");
                }

                // Search value
                if (!string.IsNullOrEmpty(_GetChecksReportDataParam.SearchBy) && !string.IsNullOrEmpty(_GetChecksReportDataParam.SearchValue))
                {
                    if (_GetChecksReportDataParam.SearchBy == "Vendor")
                    {
                        sb.Append("			AND r.Name LIKE '%" + _GetChecksReportDataParam.SearchValue + "%' \n");
                    }
                    if (_GetChecksReportDataParam.SearchBy == "CheckNum")
                    {
                        sb.Append("			AND c.Ref LIKE '%" + _GetChecksReportDataParam.SearchValue + "%' \n");
                    }
                    if (_GetChecksReportDataParam.SearchBy == "Status")
                    {
                        sb.Append("			AND tt.Sel= " + _GetChecksReportDataParam.SearchValue + " \n");
                    }
                    if (_GetChecksReportDataParam.SearchBy == "PayType")
                    {
                        sb.Append("			AND c.Type= " + _GetChecksReportDataParam.SearchValue + " \n");
                    }
                    if (_GetChecksReportDataParam.SearchBy == "Bank")
                    {
                        sb.Append("			AND b.fDesc LIKE '%" + _GetChecksReportDataParam.SearchValue + "%' \n");
                    }
                }

                //Ticket filters
                if (_GetChecksReportDataParam.filter.Count > 0)
                {
                    sb.Insert(0, "SELECT * FROM ( \n");
                    sb.Append(") AS temp \n");
                    sb.Append("WHERE 1 = 1 \n");

                    foreach (var filter in _GetChecksReportDataParam.filter)
                    {
                        if (filter.FilterColumn == "fDate")
                        {
                            sb.Append("			AND FORMAT(temp.fDate, 'M/d/yyyy') LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Ref")
                        {
                            sb.Append("			AND temp.Ref LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "fDesc")
                        {
                            sb.Append("			AND temp.fDesc LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "BankName")
                        {
                            sb.Append("			AND temp.BankName LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "TypeName")
                        {
                            sb.Append("			AND temp.TypeName LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "StatusName")
                        {
                            sb.Append("			AND temp.StatusName LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Amount")
                        {
                            sb.Append("			AND temp.Amount= " + filter.FilterValue + " \n");
                        }
                    }
                }

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLocationEquipmentList(MapData objPropMapData, List<RetainFilter> filters, bool includeInactive)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT  \n");
                sb.Append("	l.ID, \n");
                sb.Append("	l.Loc, \n");
                sb.Append("	l.Tag, \n");
                sb.Append("	l.Address, \n");
                sb.Append("	l.City, \n");
                sb.Append("	l.State, \n");
                sb.Append("	l.Zip, \n");
                sb.Append("	r.Name AS RolName, \n");
                sb.Append("	r.Address AS RolAddress, \n");
                sb.Append("	r.City AS RolCity, \n");
                sb.Append("	r.State AS RolState, \n");
                sb.Append("	r.Zip AS RolZip, \n");
                sb.Append("	r.Contact, \n");
                sb.Append("	e.Unit \n");
                sb.Append("FROM Loc l \n");
                sb.Append("INNER JOIN Elev e ON e.Loc = l.Loc \n");
                sb.Append("INNER JOIN Owner o ON l.Owner = o.ID \n");
                sb.Append("INNER JOIN Rol r ON o.Rol = r.ID \n");
                sb.Append("LEFT JOIN  Route rt ON l.Route = rt.ID   \n");
                sb.Append("LEFT JOIN Terr t ON t.ID = l.Terr \n");
                sb.Append("LEFT JOIN Terr t2 ON t2.ID = l.Terr2 \n");
                sb.Append("WHERE e.Status = 0 \n");
                sb.Append(" AND e.Unit NOT LIKE '%N/C%' COLLATE SQL_Latin1_General_CP1_CS_AS AND e.Unit NOT LIKE '%NC%' COLLATE SQL_Latin1_General_CP1_CS_AS \n");

                if (!includeInactive)
                {
                    sb.Append(" AND l.Status = 0 AND o.Status = 0 \n");
                }

                // Search value
                if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && !string.IsNullOrEmpty(objPropMapData.SearchValue))
                {
                    if (objPropMapData.SearchBy.Contains("l."))
                    {
                        if (objPropMapData.SearchBy == "l.Status"
                            || objPropMapData.SearchBy == "l.Billing"
                            || objPropMapData.SearchBy == "l.Credit"
                            || objPropMapData.SearchBy == "l.DispAlert"
                            || objPropMapData.SearchBy == "l.EmailInvoice"
                            || objPropMapData.SearchBy == "l.PrintInvoice"
                            || objPropMapData.SearchBy == "l.NoCustomerStatement"
                            || objPropMapData.SearchBy == "l.Terr"
                            || objPropMapData.SearchBy == "l.Terr2"
                            || objPropMapData.SearchBy == "l.Maint"
                            || objPropMapData.SearchBy == "l.Zone")
                        {
                            sb.Append($" AND {objPropMapData.SearchBy} = {objPropMapData.SearchValue} \n");
                        }
                        else
                        {
                            sb.Append($" AND {objPropMapData.SearchBy} LIKE '%{objPropMapData.SearchValue}%' \n");
                        }
                    }
                }

                // Grid filter
                foreach (var filter in filters)
                {
                    if (filter.FilterColumn == "Customer")
                    {
                        sb.Append(" AND r.Name LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "AcctNo")
                    {
                        sb.Append(" AND l.ID LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Location")
                    {
                        sb.Append(" AND l.Tag LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Address")
                    {
                        sb.Append(" AND l.Address LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "City")
                    {
                        sb.Append(" AND l.City LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "State")
                    {
                        sb.Append(" AND l.State LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Type")
                    {
                        sb.Append(" AND l.Type LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Types")
                    {
                        sb.Append(" AND l.Type IN (" + filter.FilterValue.Trim() + ") \n");
                    }
                    if (filter.FilterColumn == "Status")
                    {
                        sb.Append("	\n");
                    }
                    if (filter.FilterColumn == "Tickets")
                    {
                        sb.Append("	AND ((SELECT COUNT(*) FROM TicketO WHERE LID = l.Loc AND LType = 0) + (SELECT COUNT(*) FROM TicketD WHERE Loc = l.Loc )) = " + filter.FilterValue.Trim() + "\n");
                    }
                    if (filter.FilterColumn == "Equip")
                    {
                        sb.Append("	AND (SELECT COUNT(*) FROM Elev WHERE Loc = l.Loc AND Status = 0) = " + filter.FilterValue.Trim() + "\n");
                    }
                    if (filter.FilterColumn == "Balance")
                    {
                        sb.Append(" AND l.Balance = " + filter.FilterValue.Trim() + "	\n");
                    }
                    if (filter.FilterColumn == "Salesperson")
                    {
                        sb.Append(" AND t.Name LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Salesperson2")
                    {
                        sb.Append(" AND t2.Name LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                }

                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetLocationEquipmentList(GetLocationEquipmentListParam _GetLocationEquipmentList, string ConnectionString, List<RetainFilter> filters, bool includeInactive)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT  \n");
                sb.Append("	l.ID, \n");
                sb.Append("	l.Loc, \n");
                sb.Append("	l.Tag, \n");
                sb.Append("	l.Address, \n");
                sb.Append("	l.City, \n");
                sb.Append("	l.State, \n");
                sb.Append("	l.Zip, \n");
                sb.Append("	r.Name AS RolName, \n");
                sb.Append("	r.Address AS RolAddress, \n");
                sb.Append("	r.City AS RolCity, \n");
                sb.Append("	r.State AS RolState, \n");
                sb.Append("	r.Zip AS RolZip, \n");
                sb.Append("	r.Contact, \n");
                sb.Append("	e.Unit \n");
                sb.Append("FROM Loc l \n");
                sb.Append("INNER JOIN Elev e ON e.Loc = l.Loc \n");
                sb.Append("INNER JOIN Owner o ON l.Owner = o.ID \n");
                sb.Append("INNER JOIN Rol r ON o.Rol = r.ID \n");
                sb.Append("LEFT JOIN  Route rt ON l.Route = rt.ID   \n");
                sb.Append("LEFT JOIN Terr t ON t.ID = l.Terr \n");
                sb.Append("LEFT JOIN Terr t2 ON t2.ID = l.Terr2 \n");
                sb.Append("WHERE e.Status = 0 \n");
                sb.Append(" AND e.Unit NOT LIKE '%N/C%' COLLATE SQL_Latin1_General_CP1_CS_AS AND e.Unit NOT LIKE '%NC%' COLLATE SQL_Latin1_General_CP1_CS_AS \n");

                if (!includeInactive)
                {
                    sb.Append(" AND l.Status = 0 AND o.Status = 0 \n");
                }

                // Search value
                if (!string.IsNullOrEmpty(_GetLocationEquipmentList.SearchBy) && !string.IsNullOrEmpty(_GetLocationEquipmentList.SearchValue))
                {
                    if (_GetLocationEquipmentList.SearchBy.Contains("l."))
                    {
                        if (_GetLocationEquipmentList.SearchBy == "l.Status"
                            || _GetLocationEquipmentList.SearchBy == "l.Billing"
                            || _GetLocationEquipmentList.SearchBy == "l.Credit"
                            || _GetLocationEquipmentList.SearchBy == "l.DispAlert"
                            || _GetLocationEquipmentList.SearchBy == "l.EmailInvoice"
                            || _GetLocationEquipmentList.SearchBy == "l.PrintInvoice"
                            || _GetLocationEquipmentList.SearchBy == "l.NoCustomerStatement"
                            || _GetLocationEquipmentList.SearchBy == "l.Terr"
                            || _GetLocationEquipmentList.SearchBy == "l.Terr2"
                            || _GetLocationEquipmentList.SearchBy == "l.Maint"
                            || _GetLocationEquipmentList.SearchBy == "l.Zone")
                        {
                            sb.Append($" AND {_GetLocationEquipmentList.SearchBy} = {_GetLocationEquipmentList.SearchValue} \n");
                        }
                        else
                        {
                            sb.Append($" AND {_GetLocationEquipmentList.SearchBy} LIKE '%{_GetLocationEquipmentList.SearchValue}%' \n");
                        }
                    }
                }

                // Grid filter
                foreach (var filter in filters)
                {
                    if (filter.FilterColumn == "Customer")
                    {
                        sb.Append(" AND r.Name LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "AcctNo")
                    {
                        sb.Append(" AND l.ID LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Location")
                    {
                        sb.Append(" AND l.Tag LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Address")
                    {
                        sb.Append(" AND l.Address LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "City")
                    {
                        sb.Append(" AND l.City LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "State")
                    {
                        sb.Append(" AND l.State LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Type")
                    {
                        sb.Append(" AND l.Type LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Types")
                    {
                        sb.Append(" AND l.Type IN (" + filter.FilterValue.Trim() + ") \n");
                    }
                    if (filter.FilterColumn == "Status")
                    {
                        sb.Append("	\n");
                    }
                    if (filter.FilterColumn == "Tickets")
                    {
                        sb.Append("	AND ((SELECT COUNT(*) FROM TicketO WHERE LID = l.Loc AND LType = 0) + (SELECT COUNT(*) FROM TicketD WHERE Loc = l.Loc )) = " + filter.FilterValue.Trim() + "\n");
                    }
                    if (filter.FilterColumn == "Equip")
                    {
                        sb.Append("	AND (SELECT COUNT(*) FROM Elev WHERE Loc = l.Loc AND Status = 0) = " + filter.FilterValue.Trim() + "\n");
                    }
                    if (filter.FilterColumn == "Balance")
                    {
                        sb.Append(" AND l.Balance = " + filter.FilterValue.Trim() + "	\n");
                    }
                    if (filter.FilterColumn == "Salesperson")
                    {
                        sb.Append(" AND t.Name LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Salesperson2")
                    {
                        sb.Append(" AND t2.Name LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                }

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLocationByBusinessType(MapData objPropMapData, List<RetainFilter> filters, bool includeInactive)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                // Get and insert into #LocTemp table
                sb.Append("SELECT  \n");
                sb.Append("	l.ID, \n");
                sb.Append("	l.Loc, \n");
                sb.Append("	l.Tag, \n");
                sb.Append("	l.Address, \n");
                sb.Append("	l.City, \n");
                sb.Append("	l.State, \n");
                sb.Append("	l.Zip, \n");
                sb.Append("	l.Type, \n");
                sb.Append("	l.Terr, \n");
                sb.Append("	t.SMan, \n");
                sb.Append("	t.Name AS Salesperson, \n");
                sb.Append(" (SELECT STUFF((SELECT '; ' + CAST(e.Unit AS VARCHAR(1000))  \n");
                sb.Append(" FROM Elev e WHERE e.Loc = l.Loc AND e.Status = 0 \n");
                sb.Append("     FOR XML PATH(''), Type).value('.', 'VARCHAR(1000)'),1,2,' ')) AS Unit, \n");
                sb.Append("	(SELECT COUNT(ID) FROM Elev e WITH(nolock) WHERE e.Loc = l.Loc) AS Elevs, \n");
                sb.Append("	ISNULL(bt.Description,' NA') AS BusinessType  \n");
                sb.Append("INTO #LocTemp \n");
                sb.Append("FROM Loc l \n");
                sb.Append("LEFT JOIN BusinessType bt ON bt.ID = l.BusinessType \n");
                sb.Append("INNER JOIN Owner o ON l.Owner = o.ID \n");
                sb.Append("INNER JOIN Rol r ON o.Rol = r.ID \n");
                sb.Append("LEFT JOIN  Route rt ON l.Route = rt.ID   \n");
                sb.Append("LEFT JOIN Terr t ON t.ID = l.Terr \n");
                sb.Append("WHERE 1 = 1 \n");

                if (!includeInactive)
                {
                    sb.Append(" AND l.Status = 0 AND o.Status = 0 \n");
                }

                // Search value
                if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && !string.IsNullOrEmpty(objPropMapData.SearchValue))
                {
                    if (objPropMapData.SearchBy.Contains("l."))
                    {
                        if (objPropMapData.SearchBy == "l.Status"
                            || objPropMapData.SearchBy == "l.Billing"
                            || objPropMapData.SearchBy == "l.Credit"
                            || objPropMapData.SearchBy == "l.DispAlert"
                            || objPropMapData.SearchBy == "l.EmailInvoice"
                            || objPropMapData.SearchBy == "l.PrintInvoice"
                            || objPropMapData.SearchBy == "l.NoCustomerStatement"
                            || objPropMapData.SearchBy == "l.Terr"
                            || objPropMapData.SearchBy == "l.Terr2"
                            || objPropMapData.SearchBy == "l.Maint"
                            || objPropMapData.SearchBy == "l.Zone")
                        {
                            sb.Append($" AND {objPropMapData.SearchBy} = {objPropMapData.SearchValue} \n");
                        }
                        else
                        {
                            sb.Append($" AND {objPropMapData.SearchBy} LIKE '%{objPropMapData.SearchValue}%' \n");
                        }
                    }
                }

                // Grid filter
                foreach (var filter in filters)
                {
                    if (filter.FilterColumn == "Customer")
                    {
                        sb.Append(" AND r.Name LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "AcctNo")
                    {
                        sb.Append(" AND l.ID LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Location")
                    {
                        sb.Append(" AND l.Tag LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Address")
                    {
                        sb.Append(" AND l.Address LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "City")
                    {
                        sb.Append(" AND l.City LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "State")
                    {
                        sb.Append(" AND l.State LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Type")
                    {
                        sb.Append(" AND l.Type LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Types")
                    {
                        sb.Append(" AND l.Type IN (" + filter.FilterValue.Trim() + ") \n");
                    }
                    if (filter.FilterColumn == "Status")
                    {
                        sb.Append("	\n");
                    }
                    if (filter.FilterColumn == "Tickets")
                    {
                        sb.Append("	AND ((SELECT COUNT(*) FROM TicketO WHERE LID = l.Loc AND LType = 0) + (SELECT COUNT(*) FROM TicketD WHERE Loc = l.Loc )) = " + filter.FilterValue.Trim() + "\n");
                    }
                    if (filter.FilterColumn == "Equip")
                    {
                        sb.Append("	AND (SELECT COUNT(*) FROM Elev WHERE Loc = l.Loc AND Status = 0) = " + filter.FilterValue.Trim() + "\n");
                    }
                    if (filter.FilterColumn == "Balance")
                    {
                        sb.Append(" AND l.Balance = " + filter.FilterValue.Trim() + "	\n");
                    }
                    if (filter.FilterColumn == "Salesperson")
                    {
                        sb.Append(" AND t.Name LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Salesperson2")
                    {
                        sb.Append(" AND t2.Name LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                }

                // Select from #LocTemp table
                sb.Append("SELECT * FROM #LocTemp  \n");

                sb.Append("SELECT BusinessType, COUNT(*) AS LocCount  \n");
                sb.Append("FROM #LocTemp  \n");
                sb.Append("	INNER JOIN Emp ON #LocTemp.SMan = Emp.ID \n");
                sb.Append("WHERE BusinessType IS NOT NULL AND Emp.Status = 0 \n");
                sb.Append("GROUP BY BusinessType  \n");

                sb.Append("SELECT BusinessType, Terr, Salesperson, COUNT(*) AS LocCount  \n");
                sb.Append("FROM #LocTemp  \n");
                sb.Append("	INNER JOIN Emp ON #LocTemp.SMan = Emp.ID \n");
                sb.Append("WHERE BusinessType IS NOT NULL AND Emp.Status = 0 \n");
                sb.Append("GROUP BY BusinessType, Terr, Salesperson  \n");

                sb.Append("SELECT BusinessType, SUM(Elevs) AS ElevCount  \n");
                sb.Append("FROM #LocTemp  \n");
                sb.Append("	INNER JOIN Emp ON #LocTemp.SMan = Emp.ID \n");
                sb.Append("WHERE BusinessType IS NOT NULL AND Emp.Status = 0  \n");
                sb.Append("GROUP BY BusinessType \n");

                sb.Append("SELECT BusinessType, Terr, Salesperson, SUM(Elevs) AS ElevCount  \n");
                sb.Append("FROM #LocTemp  \n");
                sb.Append("	INNER JOIN Emp ON #LocTemp.SMan = Emp.ID \n");
                sb.Append("WHERE BusinessType IS NOT NULL AND Emp.Status = 0 AND Elevs > 0  \n");
                sb.Append("GROUP BY BusinessType, Terr, Salesperson \n");

                // Drop #LocTemp table
                sb.Append("DROP TABLE #LocTemp \n");

                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetLocationByBusinessType(GetLocationByBusinessTypeParam _GetLocationByBusinessType, string ConnectionString, List<RetainFilter> filters, bool includeInactive)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                // Get and insert into #LocTemp table
                sb.Append("SELECT  \n");
                sb.Append("	l.ID, \n");
                sb.Append("	l.Loc, \n");
                sb.Append("	l.Tag, \n");
                sb.Append("	l.Address, \n");
                sb.Append("	l.City, \n");
                sb.Append("	l.State, \n");
                sb.Append("	l.Zip, \n");
                sb.Append("	l.Type, \n");
                sb.Append("	l.Terr, \n");
                sb.Append("	t.SMan, \n");
                sb.Append("	t.Name AS Salesperson, \n");
                sb.Append(" (SELECT STUFF((SELECT '; ' + CAST(e.Unit AS VARCHAR(1000))  \n");
                sb.Append(" FROM Elev e WHERE e.Loc = l.Loc AND e.Status = 0 \n");
                sb.Append("     FOR XML PATH(''), Type).value('.', 'VARCHAR(1000)'),1,2,' ')) AS Unit, \n");
                sb.Append("	(SELECT COUNT(ID) FROM Elev e WITH(nolock) WHERE e.Status = 0 AND e.Loc = l.Loc) AS Elevs, \n");
                sb.Append("	bt.Description AS BusinessType \n");
                sb.Append("INTO #LocTemp \n");
                sb.Append("FROM Loc l \n");
                sb.Append("LEFT JOIN BusinessType bt ON bt.ID = l.BusinessType \n");
                sb.Append("INNER JOIN Owner o ON l.Owner = o.ID \n");
                sb.Append("INNER JOIN Rol r ON o.Rol = r.ID \n");
                sb.Append("LEFT JOIN  Route rt ON l.Route = rt.ID   \n");
                sb.Append("LEFT JOIN Terr t ON t.ID = l.Terr \n");
                sb.Append("WHERE 1 = 1 \n");

                if (!includeInactive)
                {
                    sb.Append(" AND l.Status = 0 AND o.Status = 0 \n");
                }

                // Search value
                if (!string.IsNullOrEmpty(_GetLocationByBusinessType.SearchBy) && !string.IsNullOrEmpty(_GetLocationByBusinessType.SearchValue))
                {
                    if (_GetLocationByBusinessType.SearchBy.Contains("l."))
                    {
                        if (_GetLocationByBusinessType.SearchBy == "l.Status"
                            || _GetLocationByBusinessType.SearchBy == "l.Billing"
                            || _GetLocationByBusinessType.SearchBy == "l.Credit"
                            || _GetLocationByBusinessType.SearchBy == "l.DispAlert"
                            || _GetLocationByBusinessType.SearchBy == "l.EmailInvoice"
                            || _GetLocationByBusinessType.SearchBy == "l.PrintInvoice"
                            || _GetLocationByBusinessType.SearchBy == "l.NoCustomerStatement"
                            || _GetLocationByBusinessType.SearchBy == "l.Terr"
                            || _GetLocationByBusinessType.SearchBy == "l.Terr2"
                            || _GetLocationByBusinessType.SearchBy == "l.Maint"
                            || _GetLocationByBusinessType.SearchBy == "l.Zone")
                        {
                            sb.Append($" AND {_GetLocationByBusinessType.SearchBy} = {_GetLocationByBusinessType.SearchValue} \n");
                        }
                        else
                        {
                            sb.Append($" AND {_GetLocationByBusinessType.SearchBy} LIKE '%{_GetLocationByBusinessType.SearchValue}%' \n");
                        }
                    }
                }

                // Grid filter
                foreach (var filter in filters)
                {
                    if (filter.FilterColumn == "Customer")
                    {
                        sb.Append(" AND r.Name LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "AcctNo")
                    {
                        sb.Append(" AND l.ID LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Location")
                    {
                        sb.Append(" AND l.Tag LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Address")
                    {
                        sb.Append(" AND l.Address LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "City")
                    {
                        sb.Append(" AND l.City LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "State")
                    {
                        sb.Append(" AND l.State LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Type")
                    {
                        sb.Append(" AND l.Type LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Types")
                    {
                        sb.Append(" AND l.Type IN (" + filter.FilterValue.Trim() + ") \n");
                    }
                    if (filter.FilterColumn == "Status")
                    {
                        sb.Append("	\n");
                    }
                    if (filter.FilterColumn == "Tickets")
                    {
                        sb.Append("	AND ((SELECT COUNT(*) FROM TicketO WHERE LID = l.Loc AND LType = 0) + (SELECT COUNT(*) FROM TicketD WHERE Loc = l.Loc )) = " + filter.FilterValue.Trim() + "\n");
                    }
                    if (filter.FilterColumn == "Equip")
                    {
                        sb.Append("	AND (SELECT COUNT(*) FROM Elev WHERE Loc = l.Loc AND Status = 0) = " + filter.FilterValue.Trim() + "\n");
                    }
                    if (filter.FilterColumn == "Balance")
                    {
                        sb.Append(" AND l.Balance = " + filter.FilterValue.Trim() + "	\n");
                    }
                    if (filter.FilterColumn == "Salesperson")
                    {
                        sb.Append(" AND t.Name LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Salesperson2")
                    {
                        sb.Append(" AND t2.Name LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                }

                // Select from #LocTemp table
                sb.Append("SELECT * FROM #LocTemp  \n");

                sb.Append("SELECT BusinessType, COUNT(*) AS LocCount  \n");
                sb.Append("FROM #LocTemp  \n");
                sb.Append("	INNER JOIN Emp ON #LocTemp.SMan = Emp.ID \n");
                sb.Append("WHERE BusinessType IS NOT NULL AND Emp.Status = 0 \n");
                sb.Append("GROUP BY BusinessType  \n");

                sb.Append("SELECT BusinessType, Terr, Salesperson, COUNT(*) AS LocCount  \n");
                sb.Append("FROM #LocTemp  \n");
                sb.Append("	INNER JOIN Emp ON #LocTemp.SMan = Emp.ID \n");
                sb.Append("WHERE BusinessType IS NOT NULL AND Emp.Status = 0 \n");
                sb.Append("GROUP BY BusinessType, Terr, Salesperson  \n");

                sb.Append("SELECT BusinessType, SUM(Elevs) AS ElevCount  \n");
                sb.Append("FROM #LocTemp  \n");
                sb.Append("	INNER JOIN Emp ON #LocTemp.SMan = Emp.ID \n");
                sb.Append("WHERE BusinessType IS NOT NULL AND Emp.Status = 0  \n");
                sb.Append("GROUP BY BusinessType \n");

                sb.Append("SELECT BusinessType, Terr, Salesperson, SUM(Elevs) AS ElevCount  \n");
                sb.Append("FROM #LocTemp  \n");
                sb.Append("	INNER JOIN Emp ON #LocTemp.SMan = Emp.ID \n");
                sb.Append("WHERE BusinessType IS NOT NULL AND Emp.Status = 0 AND Elevs > 0  \n");
                sb.Append("GROUP BY BusinessType, Terr, Salesperson \n");

                // Drop #LocTemp table
                sb.Append("DROP TABLE #LocTemp \n");

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLocationDetailsReport(MapData objPropMapData, List<RetainFilter> filters, bool includeInactive)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT \n");
                sb.Append("	l.Loc, \n");
                sb.Append("	l.ID, \n");
                sb.Append("	Tag, \n");
                sb.Append("	l.Address AS LocAddress, \n");
                sb.Append("	l.City AS LocCity, \n");
                sb.Append("	l.State AS LocState, \n");
                sb.Append("	l.Zip AS LocZip, \n");
                sb.Append("	l.Country AS LocCountry, \n");
                sb.Append("	l.Rol, \n");
                sb.Append("	l.Consult, \n");
                sb.Append("	l.Type, \n");
                sb.Append("	l.Route, \n");
                sb.Append("	l.Terr, \n");
                sb.Append("	l.Terr2, \n");
                sb.Append("	rl.City, \n");
                sb.Append("	rl.State, \n");
                sb.Append("	rl.Zip, \n");
                sb.Append("	rl.Country, \n");
                sb.Append("	rl.Address, \n");
                sb.Append("	l.Remarks, \n");
                sb.Append("	rl.Contact, \n");
                sb.Append("	rl.Contact AS Name, \n");
                sb.Append("	rl.Phone, \n");
                sb.Append("	rl.Website, \n");
                sb.Append("	rl.EMail, \n");
                sb.Append("	rl.Cellular, \n");
                sb.Append("	rl.Fax, \n");
                sb.Append("	l.Owner, \n");
                sb.Append("	l.Stax, \n");
                sb.Append("	l.STax2, \n");
                sb.Append("	l.UTax, \n");
                sb.Append("	l.Zone, \n");
                sb.Append("	l.PrintInvoice, \n");
                sb.Append("	l.EmailInvoice, \n");
                sb.Append("	l.Balance, \n");
                sb.Append("	l.Billing, \n");
                sb.Append("	l.QBLocID, \n");
                sb.Append("	l.DefaultTerms, \n");
                sb.Append("	rl.Lat, \n");
                sb.Append("	rl.Lng, \n");
                sb.Append("	l.Custom1, \n");
                sb.Append("	l.Custom2, \n");
                sb.Append("	l.Custom12, \n");
                sb.Append("	l.Custom13, \n");
                sb.Append("	l.Custom14, \n");
                sb.Append("	l.Custom15, \n");
                sb.Append("	l.Status, \n");
                sb.Append("	rt.Name AS DefWork, \n");
                sb.Append("	ISNULL(l.Credit, 0) AS Credit, \n");
                sb.Append("	ISNULL(l.Dispalert, 0) AS Dispalert, \n");
                sb.Append("	l.CreditReason, \n");
                sb.Append("	o.SageID AS CustomerSageID,	 \n");
                sb.Append("	ISNULL(l.BillRate,0) AS BillRate, \n");
                sb.Append("	ISNULL(l.RateOT,0) AS RateOT, \n");
                sb.Append("	ISNULL(l.RateNT,0) AS RateNT, \n");
                sb.Append("	ISNULL(l.RateDT,0) AS RateDT, \n");
                sb.Append("	ISNULL(l.RateTravel,0) AS RateTravel, \n");
                sb.Append("	ISNULL(l.RateMileage,0) AS RateMileage, \n");
                sb.Append("	ISNULL(st.Rate,0) AS Rate, \n");
                sb.Append("	CASE WHEN (SELECT Label FROM Custom WHERE Name ='Country') = 1  \n");
                sb.Append("		THEN  \n");
                sb.Append("			Convert(numeric(30,2),(SELECT Label AS GstRate FROM Custom WHERE Name = 'GSTRate')) \n");
                sb.Append("		ELSE 0.00  \n");
                sb.Append("	END AS GstRate, \n");
                sb.Append("	ISNULL(l.NoCustomerStatement, 0) AS NoCustomerStatement, \n");
                sb.Append("	t.Name AS Salesperson, \n");
                sb.Append("	rt.Name AS RouteName, \n");
                sb.Append("	ISNULL(cst.Name, 'None') AS ConsultantName, \n");
                sb.Append("	o.Custom1 AS OwnerName, \n");
                sb.Append("	r.Name AS CustomerName, \n");
                sb.Append("	l.BusinessType AS BusinessTypeID, \n");
                sb.Append("	st.Type AS sTaxType \n");
                sb.Append("FROM Loc l \n");
                sb.Append("	LEFT OUTER JOIN Owner o ON o.ID = l.Owner \n");
                sb.Append("	LEFT OUTER JOIN Rol r ON o.Rol = r.ID \n");
                sb.Append("	LEFT OUTER JOIN Rol rl ON l.Rol = rl.ID and rl.Type = 4 \n");
                sb.Append("	LEFT OUTER JOIN Stax st ON st.Name = l.STax \n");
                sb.Append("	LEFT OUTER JOIN Branch B ON B.ID = r.EN \n");
                sb.Append("	LEFT OUTER JOIN Terr t ON l.Terr = t.ID \n");
                sb.Append("	LEFT OUTER JOIN Terr t2 ON t2.ID = l.Terr2  \n");
                sb.Append("	LEFT OUTER JOIN Route rt  ON l.Route = rt.ID  \n");
                sb.Append("	LEFT OUTER JOIN tblConsult cst  ON cst.ID = l.Consult \n");
                sb.Append("WHERE 1 = 1 \n");

                if (!includeInactive)
                {
                    sb.Append(" AND l.Status = 0 AND o.Status = 0 \n");
                }

                // Search value
                if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && !string.IsNullOrEmpty(objPropMapData.SearchValue))
                {
                    if (objPropMapData.SearchBy.Contains("l."))
                    {
                        if (objPropMapData.SearchBy == "l.Status"
                            || objPropMapData.SearchBy == "l.Billing"
                            || objPropMapData.SearchBy == "l.Credit"
                            || objPropMapData.SearchBy == "l.DispAlert"
                            || objPropMapData.SearchBy == "l.EmailInvoice"
                            || objPropMapData.SearchBy == "l.PrintInvoice"
                            || objPropMapData.SearchBy == "l.NoCustomerStatement"
                            || objPropMapData.SearchBy == "l.Terr"
                            || objPropMapData.SearchBy == "l.Terr2"
                            || objPropMapData.SearchBy == "l.Maint"
                            || objPropMapData.SearchBy == "l.Zone")
                        {
                            sb.Append($" AND {objPropMapData.SearchBy} = {objPropMapData.SearchValue} \n");
                        }
                        else
                        {
                            sb.Append($" AND {objPropMapData.SearchBy} LIKE '%{objPropMapData.SearchValue}%' \n");
                        }
                    }
                }

                // Grid filter
                foreach (var filter in filters)
                {
                    if (filter.FilterColumn == "Customer")
                    {
                        sb.Append(" AND r.Name LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "AcctNo")
                    {
                        sb.Append(" AND l.ID LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Location")
                    {
                        sb.Append(" AND l.Tag LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Address")
                    {
                        sb.Append(" AND l.Address LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "City")
                    {
                        sb.Append(" AND l.City LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "State")
                    {
                        sb.Append(" AND l.State LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Type")
                    {
                        sb.Append(" AND l.Type LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Types")
                    {
                        sb.Append(" AND l.Type IN (" + filter.FilterValue.Trim() + ") \n");
                    }
                    if (filter.FilterColumn == "Status")
                    {
                        sb.Append("	\n");
                    }
                    if (filter.FilterColumn == "Tickets")
                    {
                        sb.Append("	AND ((SELECT COUNT(*) FROM TicketO WHERE LID = l.Loc AND LType = 0) + (SELECT COUNT(*) FROM TicketD WHERE Loc = l.Loc )) = " + filter.FilterValue.Trim() + "\n");
                    }
                    if (filter.FilterColumn == "Equip")
                    {
                        sb.Append("	AND (SELECT COUNT(*) FROM Elev WHERE Loc = l.Loc AND Status = 0) = " + filter.FilterValue.Trim() + "\n");
                    }
                    if (filter.FilterColumn == "Balance")
                    {
                        sb.Append(" AND l.Balance = " + filter.FilterValue.Trim() + "	\n");
                    }
                    if (filter.FilterColumn == "Salesperson")
                    {
                        sb.Append(" AND t.Name LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Salesperson2")
                    {
                        sb.Append(" AND t2.Name LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                }

                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //API
        public DataSet GetLocationDetailsReport(GetLocationDetailsReportParam _GetLocationDetailsReport, string ConnectionString, List<RetainFilter> filters, bool includeInactive)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT \n");
                sb.Append("	l.Loc, \n");
                sb.Append("	l.ID, \n");
                sb.Append("	Tag, \n");
                sb.Append("	l.Address AS LocAddress, \n");
                sb.Append("	l.City AS LocCity, \n");
                sb.Append("	l.State AS LocState, \n");
                sb.Append("	l.Zip AS LocZip, \n");
                sb.Append("	l.Country AS LocCountry, \n");
                sb.Append("	l.Rol, \n");
                sb.Append("	l.Consult, \n");
                sb.Append("	l.Type, \n");
                sb.Append("	l.Route, \n");
                sb.Append("	l.Terr, \n");
                sb.Append("	l.Terr2, \n");
                sb.Append("	rl.City, \n");
                sb.Append("	rl.State, \n");
                sb.Append("	rl.Zip, \n");
                sb.Append("	rl.Country, \n");
                sb.Append("	rl.Address, \n");
                sb.Append("	l.Remarks, \n");
                sb.Append("	rl.Contact, \n");
                sb.Append("	rl.Contact AS Name, \n");
                sb.Append("	rl.Phone, \n");
                sb.Append("	rl.Website, \n");
                sb.Append("	rl.EMail, \n");
                sb.Append("	rl.Cellular, \n");
                sb.Append("	rl.Fax, \n");
                sb.Append("	l.Owner, \n");
                sb.Append("	l.Stax, \n");
                sb.Append("	l.STax2, \n");
                sb.Append("	l.UTax, \n");
                sb.Append("	l.Zone, \n");
                sb.Append("	l.PrintInvoice, \n");
                sb.Append("	l.EmailInvoice, \n");
                sb.Append("	l.Balance, \n");
                sb.Append("	l.Billing, \n");
                sb.Append("	l.QBLocID, \n");
                sb.Append("	l.DefaultTerms, \n");
                sb.Append("	rl.Lat, \n");
                sb.Append("	rl.Lng, \n");
                sb.Append("	l.Custom1, \n");
                sb.Append("	l.Custom2, \n");
                sb.Append("	l.Custom12, \n");
                sb.Append("	l.Custom13, \n");
                sb.Append("	l.Custom14, \n");
                sb.Append("	l.Custom15, \n");
                sb.Append("	l.Status, \n");
                sb.Append("	rt.Name AS DefWork, \n");
                sb.Append("	ISNULL(l.Credit, 0) AS Credit, \n");
                sb.Append("	ISNULL(l.Dispalert, 0) AS Dispalert, \n");
                sb.Append("	l.CreditReason, \n");
                sb.Append("	o.SageID AS CustomerSageID,	 \n");
                sb.Append("	ISNULL(l.BillRate,0) AS BillRate, \n");
                sb.Append("	ISNULL(l.RateOT,0) AS RateOT, \n");
                sb.Append("	ISNULL(l.RateNT,0) AS RateNT, \n");
                sb.Append("	ISNULL(l.RateDT,0) AS RateDT, \n");
                sb.Append("	ISNULL(l.RateTravel,0) AS RateTravel, \n");
                sb.Append("	ISNULL(l.RateMileage,0) AS RateMileage, \n");
                sb.Append("	ISNULL(st.Rate,0) AS Rate, \n");
                sb.Append("	CASE WHEN (SELECT Label FROM Custom WHERE Name ='Country') = 1  \n");
                sb.Append("		THEN  \n");
                sb.Append("			Convert(numeric(30,2),(SELECT Label AS GstRate FROM Custom WHERE Name = 'GSTRate')) \n");
                sb.Append("		ELSE 0.00  \n");
                sb.Append("	END AS GstRate, \n");
                sb.Append("	ISNULL(l.NoCustomerStatement, 0) AS NoCustomerStatement, \n");
                sb.Append("	t.Name AS Salesperson, \n");
                sb.Append("	rt.Name AS RouteName, \n");
                sb.Append("	ISNULL(cst.Name, 'None') AS ConsultantName, \n");
                sb.Append("	o.Custom1 AS OwnerName, \n");
                sb.Append("	r.Name AS CustomerName, \n");
                sb.Append("	l.BusinessType AS BusinessTypeID, \n");
                sb.Append("	st.Type AS sTaxType \n");
                sb.Append("FROM Loc l \n");
                sb.Append("	LEFT OUTER JOIN Owner o ON o.ID = l.Owner \n");
                sb.Append("	LEFT OUTER JOIN Rol r ON o.Rol = r.ID \n");
                sb.Append("	LEFT OUTER JOIN Rol rl ON l.Rol = rl.ID and rl.Type = 4 \n");
                sb.Append("	LEFT OUTER JOIN Stax st ON st.Name = l.STax \n");
                sb.Append("	LEFT OUTER JOIN Branch B ON B.ID = r.EN \n");
                sb.Append("	LEFT OUTER JOIN Terr t ON l.Terr = t.ID \n");
                sb.Append("	LEFT OUTER JOIN Terr t2 ON t2.ID = l.Terr2  \n");
                sb.Append("	LEFT OUTER JOIN Route rt  ON l.Route = rt.ID  \n");
                sb.Append("	LEFT OUTER JOIN tblConsult cst  ON cst.ID = l.Consult \n");
                sb.Append("WHERE 1 = 1 \n");

                if (!includeInactive)
                {
                    sb.Append(" AND l.Status = 0 AND o.Status = 0 \n");
                }

                // Search value
                if (!string.IsNullOrEmpty(_GetLocationDetailsReport.SearchBy) && !string.IsNullOrEmpty(_GetLocationDetailsReport.SearchValue))
                {
                    if (_GetLocationDetailsReport.SearchBy.Contains("l."))
                    {
                        if (_GetLocationDetailsReport.SearchBy == "l.Status"
                            || _GetLocationDetailsReport.SearchBy == "l.Billing"
                            || _GetLocationDetailsReport.SearchBy == "l.Credit"
                            || _GetLocationDetailsReport.SearchBy == "l.DispAlert"
                            || _GetLocationDetailsReport.SearchBy == "l.EmailInvoice"
                            || _GetLocationDetailsReport.SearchBy == "l.PrintInvoice"
                            || _GetLocationDetailsReport.SearchBy == "l.NoCustomerStatement"
                            || _GetLocationDetailsReport.SearchBy == "l.Terr"
                            || _GetLocationDetailsReport.SearchBy == "l.Terr2"
                            || _GetLocationDetailsReport.SearchBy == "l.Maint"
                            || _GetLocationDetailsReport.SearchBy == "l.Zone")
                        {
                            sb.Append($" AND {_GetLocationDetailsReport.SearchBy} = {_GetLocationDetailsReport.SearchValue} \n");
                        }
                        else
                        {
                            sb.Append($" AND {_GetLocationDetailsReport.SearchBy} LIKE '%{_GetLocationDetailsReport.SearchValue}%' \n");
                        }
                    }
                }

                // Grid filter
                foreach (var filter in filters)
                {
                    if (filter.FilterColumn == "Customer")
                    {
                        sb.Append(" AND r.Name LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "AcctNo")
                    {
                        sb.Append(" AND l.ID LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Location")
                    {
                        sb.Append(" AND l.Tag LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Address")
                    {
                        sb.Append(" AND l.Address LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "City")
                    {
                        sb.Append(" AND l.City LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "State")
                    {
                        sb.Append(" AND l.State LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Type")
                    {
                        sb.Append(" AND l.Type LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Types")
                    {
                        sb.Append(" AND l.Type IN (" + filter.FilterValue.Trim() + ") \n");
                    }
                    if (filter.FilterColumn == "Status")
                    {
                        sb.Append("	\n");
                    }
                    if (filter.FilterColumn == "Tickets")
                    {
                        sb.Append("	AND ((SELECT COUNT(*) FROM TicketO WHERE LID = l.Loc AND LType = 0) + (SELECT COUNT(*) FROM TicketD WHERE Loc = l.Loc )) = " + filter.FilterValue.Trim() + "\n");
                    }
                    if (filter.FilterColumn == "Equip")
                    {
                        sb.Append("	AND (SELECT COUNT(*) FROM Elev WHERE Loc = l.Loc AND Status = 0) = " + filter.FilterValue.Trim() + "\n");
                    }
                    if (filter.FilterColumn == "Balance")
                    {
                        sb.Append(" AND l.Balance = " + filter.FilterValue.Trim() + "	\n");
                    }
                    if (filter.FilterColumn == "Salesperson")
                    {
                        sb.Append(" AND t.Name LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Salesperson2")
                    {
                        sb.Append(" AND t2.Name LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                }

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLocationWithHomeOwnerReport(MapData objPropMapData, List<RetainFilter> filters, bool includeInactive)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT \n");
                sb.Append("	l.Loc, \n");
                sb.Append("	l.ID, \n");
                sb.Append("	l.Tag, \n");
                sb.Append("	l.Address AS LocAddress, \n");
                sb.Append("	l.City AS LocCity, \n");
                sb.Append("	l.State AS LocState, \n");
                sb.Append("	l.Zip AS LocZip, \n");
                sb.Append("	l.Country AS LocCountry, \n");
                sb.Append("	l.Rol, \n");
                sb.Append("	l.Consult, \n");
                sb.Append("	l.Type, \n");
                sb.Append("	l.Route, \n");
                sb.Append("	l.Terr, \n");
                sb.Append("	l.Terr2, \n");
                sb.Append("	l.Remarks, \n");
                sb.Append("	l.Owner, \n");
                sb.Append("	l.Stax, \n");
                sb.Append("	l.STax2, \n");
                sb.Append("	l.UTax, \n");
                sb.Append("	l.Zone, \n");
                sb.Append("	l.PrintInvoice, \n");
                sb.Append("	l.EmailInvoice, \n");
                sb.Append("	l.Balance, \n");
                sb.Append("	l.Billing, \n");
                sb.Append("	l.QBLocID, \n");
                sb.Append("	l.DefaultTerms, \n");
                sb.Append("	l.Custom1, \n");
                sb.Append("	l.Custom2, \n");
                sb.Append("	l.Custom12, \n");
                sb.Append("	l.Custom13, \n");
                sb.Append("	l.Custom14, \n");
                sb.Append("	l.Custom15, \n");
                sb.Append("	l.Status, \n");
                sb.Append("	rt.Name AS DefWork, \n");
                sb.Append("	ISNULL(l.Credit, 0) AS Credit, \n");
                sb.Append("	ISNULL(l.Dispalert, 0) AS Dispalert, \n");
                sb.Append("	l.CreditReason, \n");
                sb.Append("	o.SageID AS CustomerSageID,	 \n");
                sb.Append("	ISNULL(l.BillRate,0) AS BillRate, \n");
                sb.Append("	ISNULL(l.RateOT,0) AS RateOT, \n");
                sb.Append("	ISNULL(l.RateNT,0) AS RateNT, \n");
                sb.Append("	ISNULL(l.RateDT,0) AS RateDT, \n");
                sb.Append("	ISNULL(l.RateTravel,0) AS RateTravel, \n");
                sb.Append("	ISNULL(l.RateMileage,0) AS RateMileage, \n");
                sb.Append("	ISNULL(l.NoCustomerStatement, 0) AS NoCustomerStatement, \n");
                sb.Append("	t.Name AS Salesperson, \n");
                sb.Append("	rt.Name AS RouteName, \n");
                sb.Append("	o.Custom1 AS OwnerName, \n");
                sb.Append("	LTRIM(RTRIM(r.Name)) AS CustomerName, \n");
                sb.Append("	r.Address AS CustomerAddress, \n");
                sb.Append("	r.City AS CustomerCity, \n");
                sb.Append("	r.State AS CustomerState, \n");
                sb.Append("	r.Zip AS CustomerZip, \n");
                sb.Append("	l.BusinessType AS BusinessTypeID, \n");
                sb.Append("	rh.Name AS HomeOwnerName, \n");
                sb.Append("	rh.Address AS HomeOwnerAddress, \n");
                sb.Append("	rh.City AS HomeOwnerCity, \n");
                sb.Append("	rh.State AS HomeOwnerState, \n");
                sb.Append("	rh.Zip AS HomeOwnerZip, \n");
                sb.Append("	rh.Phone AS HomeOwnerPhone, \n");
                sb.Append("	rh.Fax AS HomeOwnerFax, \n");
                sb.Append("	rh.Contact AS HomeOwnerContact, \n");
                sb.Append("	rh.EMail AS HomeOwnerEMail, \n");
                sb.Append("	rh.Country AS HomeOwnerCountry, \n");
                sb.Append("	rh.Cellular AS HomeOwnerCellular, \n");
                sb.Append("	rh.Remarks AS HomeOwnerRemarks \n");
                sb.Append("FROM Loc l \n");
                sb.Append("	INNER JOIN Owner o ON o.ID = l.Owner \n");
                sb.Append("	INNER JOIN Rol r ON o.Rol = r.ID \n");
                sb.Append("	LEFT OUTER JOIN Branch B ON B.ID = r.EN \n");
                sb.Append("	LEFT OUTER JOIN Terr t ON l.Terr = t.ID \n");
                sb.Append("	LEFT OUTER JOIN Terr t2 ON t2.ID = l.Terr2  \n");
                sb.Append("	LEFT OUTER JOIN Route rt  ON l.Route = rt.ID  \n");
                sb.Append("	LEFT OUTER JOIN Rol rh ON rh.ID = l.HomeOwnerID \n");

                sb.Append("WHERE 1 = 1 \n");

                if (!includeInactive)
                {
                    sb.Append(" AND l.Status = 0 AND o.Status = 0 \n");
                }

                // Search value
                if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && !string.IsNullOrEmpty(objPropMapData.SearchValue))
                {
                    if (objPropMapData.SearchBy.Contains("l."))
                    {
                        if (objPropMapData.SearchBy == "l.Status"
                            || objPropMapData.SearchBy == "l.Billing"
                            || objPropMapData.SearchBy == "l.Credit"
                            || objPropMapData.SearchBy == "l.DispAlert"
                            || objPropMapData.SearchBy == "l.EmailInvoice"
                            || objPropMapData.SearchBy == "l.PrintInvoice"
                            || objPropMapData.SearchBy == "l.NoCustomerStatement"
                            || objPropMapData.SearchBy == "l.Terr"
                            || objPropMapData.SearchBy == "l.Terr2"
                            || objPropMapData.SearchBy == "l.Maint"
                            || objPropMapData.SearchBy == "l.Zone")
                        {
                            sb.Append($" AND {objPropMapData.SearchBy} = {objPropMapData.SearchValue} \n");
                        }
                        else
                        {
                            sb.Append($" AND {objPropMapData.SearchBy} LIKE '%{objPropMapData.SearchValue}%' \n");
                        }
                    }
                }

                // Grid filter
                foreach (var filter in filters)
                {
                    if (filter.FilterColumn == "Customer")
                    {
                        sb.Append(" AND r.Name LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "AcctNo")
                    {
                        sb.Append(" AND l.ID LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Location")
                    {
                        sb.Append(" AND l.Tag LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Address")
                    {
                        sb.Append(" AND l.Address LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "City")
                    {
                        sb.Append(" AND l.City LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "State")
                    {
                        sb.Append(" AND l.State LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Type")
                    {
                        sb.Append(" AND l.Type LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Types")
                    {
                        sb.Append(" AND l.Type IN (" + filter.FilterValue.Trim() + ") \n");
                    }
                    if (filter.FilterColumn == "Status")
                    {
                        sb.Append("	\n");
                    }
                    if (filter.FilterColumn == "Tickets")
                    {
                        sb.Append("	AND ((SELECT COUNT(*) FROM TicketO WHERE LID = l.Loc AND LType = 0) + (SELECT COUNT(*) FROM TicketD WHERE Loc = l.Loc )) = " + filter.FilterValue.Trim() + "\n");
                    }
                    if (filter.FilterColumn == "Equip")
                    {
                        sb.Append("	AND (SELECT COUNT(*) FROM Elev WHERE Loc = l.Loc AND Status = 0) = " + filter.FilterValue.Trim() + "\n");
                    }
                    if (filter.FilterColumn == "Balance")
                    {
                        sb.Append(" AND l.Balance = " + filter.FilterValue.Trim() + "	\n");
                    }
                    if (filter.FilterColumn == "Salesperson")
                    {
                        sb.Append(" AND t.Name LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                    if (filter.FilterColumn == "Salesperson2")
                    {
                        sb.Append(" AND t2.Name LIKE '%" + filter.FilterValue.Trim() + "%' \n");
                    }
                }

                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Job Project Approval DCA
        /// Only for Accredited
        /// </summary>
        /// <param name="objChart"></param>
        /// <returns></returns>
        public DataSet GetJobProjectApprovalDCA(Chart objChart)
        {
            return SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.StoredProcedure, "spGetJobProjectApprovalDCA");
        }

        public DataSet GetCategoryDueReport(MapData objPropMapData, List<RetainFilter> filters)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT * FROM ( \n");

                if (objPropMapData.FilterReview != "1")
                {
                    #region TicketO table

                    sb.Append("	SELECT  \n");
                    sb.Append("			t.[ID] \n");
                    sb.Append("			,t.[CDate] \n");
                    sb.Append("			,t.[DDate] \n");
                    sb.Append("			,t.[EDate] \n");
                    sb.Append("			,t.[fWork] \n");
                    sb.Append("			,t.[Job] \n");
                    sb.Append("			,t.[LID] AS Loc \n");
                    sb.Append("			,t.[LElev] AS Elev \n");
                    sb.Append("			,t.[Type] \n");
                    sb.Append("			,t.[fDesc] \n");
                    sb.Append("			,dp.[DescRes] \n");
                    sb.Append("			,dp.[Status] \n");
                    sb.Append("			,dp.[Invoice] \n");
                    sb.Append("			,t.[Level] \n");
                    sb.Append("			,t.[Est] \n");
                    sb.Append("			,t.[Cat] \n");
                    sb.Append("			,t.[Who] \n");
                    sb.Append("			,t.[fBy] \n");
                    sb.Append("			,t.[fLong] \n");
                    sb.Append("			,t.[Latt] \n");
                    sb.Append("			,dp.[WageC] \n");
                    sb.Append("			,dp.[Phase] \n");
                    sb.Append("			,dp.[Car] \n");
                    sb.Append("			,dp.[CallIn] \n");
                    sb.Append("			,dp.[Mileage] \n");
                    sb.Append("			,dp.[NT] \n");
                    sb.Append("			,dp.[CauseID] \n");
                    sb.Append("			,dp.[CauseDesc] \n");
                    sb.Append("			,t.[fGroup] \n");
                    sb.Append("			,t.[PriceL] \n");
                    sb.Append("			,t.[WorkOrder] \n");
                    sb.Append("			,jt.[Type] AS JobType \n");
                    sb.Append("			,ww.[fDesc] AS Mech \n");
                    sb.Append("			,ro.[Name] AS OwnerName \n");
                    sb.Append("			,ro.[Address] AS OwnerAddress\n");
                    sb.Append("			,ro.[City] AS OwnerCity \n");
                    sb.Append("			,ro.[State] AS OwnerState \n");
                    sb.Append("			,ro.[Zip] AS OwnerZip \n");
                    sb.Append("			,ro.[Phone] AS OwnerPhone \n");
                    sb.Append("			,o.[Type] AS OwnerType \n");
                    sb.Append("			,t.[LDesc3] AS Address  \n");
                    sb.Append("			,t.[City] \n");
                    sb.Append("			,t.[State] \n");
                    sb.Append("			,t.[Zip] \n");
                    sb.Append("			,rl.[Contact] AS MainContact \n");
                    sb.Append("			,rl.[Phone] AS ContactPhone \n");
                    sb.Append("			,l.[Tag] AS LocName \n");
                    sb.Append("			,l.[Address] AS LocAddress  \n");
                    sb.Append("			,l.[City] AS LocCity \n");
                    sb.Append("			,l.[State] AS LocState \n");
                    sb.Append("			,l.[Zip] AS LocZip \n");
                    sb.Append("			,CASE \n");
                    sb.Append("			    WHEN t.Assigned = 0 THEN 'Un-Assigned' \n");
                    sb.Append("			    WHEN t.Assigned = 1 THEN 'Assigned' \n");
                    sb.Append("			    WHEN t.Assigned = 2 THEN 'Enroute' \n");
                    sb.Append("			    WHEN t.Assigned = 3 THEN 'Onsite' \n");
                    sb.Append("			    WHEN t.Assigned = 4 THEN 'Completedd' \n");
                    sb.Append("			    WHEN t.Assigned = 5 THEN 'Hold' \n");
                    sb.Append("			    WHEN t.Assigned = 6 THEN 'Voided' \n");
                    sb.Append("			END AS Assignname \n");
                    sb.Append("         ,(SELECT STUFF((SELECT ', ' + CAST(e.Unit AS VARCHAR(1000))  \n");
                    sb.Append("         FROM Elev e WHERE e.id IN  \n");
                    sb.Append("             (SELECT me.elev_id FROM multiple_equipments me WHERE me.ticket_id = t.ID) \n");
                    sb.Append("             FOR XML PATH(''), Type).value('.', 'VARCHAR(1000)'),1,2,' ')) AS Unit \n");
                    sb.Append("		FROM TicketO t \n");
                    sb.Append("			LEFT OUTER JOIN TicketDPDA dp ON t.ID = dp.ID \n");
                    sb.Append("			INNER JOIN Loc l ON l.Loc = t.LID  	 \n");
                    sb.Append("			INNER JOIN Rol rl ON rl.ID = l.Rol  	 \n");
                    sb.Append("			INNER JOIN Owner o ON o.ID = t.Owner  	 \n");
                    sb.Append("			INNER JOIN Rol ro ON ro.ID = o.Rol   \n");
                    sb.Append("			LEFT JOIN Route rou ON rou.ID = l.Route \n");
                    sb.Append("			LEFT JOIN Job j ON j.ID = t.Job \n");
                    sb.Append("			LEFT JOIN JobType jt ON jt.ID = j.Type  \n");
                    sb.Append("			LEFT JOIN tblWork m ON m.ID = rou.Mech \n");
                    sb.Append("			LEFT JOIN tblWork ww ON ww.ID = t.fWork \n");
                    sb.Append("			LEFT JOIN Elev e ON e.ID = t.LElev \n");
                    sb.Append("		WHERE t.EDate >= '" + objPropMapData.StartDate + "' AND t.EDate <= '" + objPropMapData.EndDate + "' \n");

                    // If get from from Edit Location screen
                    if (objPropMapData.LocID > 0)
                    {
                        sb.Append("			AND t.LID = " + objPropMapData.LocID + " \n");
                    }

                    // If get from from Edit Customer screen
                    if (objPropMapData.CustID > 0)
                    {
                        sb.Append("			AND l.Owner = " + objPropMapData.CustID + " \n");
                    }

                    //Advanced Search
                    if (!string.IsNullOrEmpty(objPropMapData.Supervisor))
                    {
                        sb.Append("			AND m.Super ='" + objPropMapData.Supervisor + "' \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.FilterCharge))
                    {
                        sb.Append("			AND (ISNULL(dp.Charge,0)= " + Convert.ToInt32(objPropMapData.FilterCharge));

                        if (objPropMapData.FilterCharge == "1")
                        {
                            sb.Append(" OR ISNULL(dp.Invoice,0) <> 0) \n");
                        }
                        else
                        {
                            sb.Append(" AND ISNULL(dp.Invoice,0) = 0) \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.FilterReview))
                    {
                        sb.Append("			AND ISNULL(dp.ClearCheck,0)= " + Convert.ToInt32(objPropMapData.FilterReview) + " \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Workorder))
                    {
                        sb.Append("			AND t.Workorder= '" + objPropMapData.Workorder + "' \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Route))
                    {
                        if (Convert.ToInt32(objPropMapData.Route) == 0)
                        {
                            sb.Append("			AND l.Route = 0 \n");
                        }
                        else
                        {
                            sb.Append("			AND rou.ID = " + Convert.ToInt32(objPropMapData.Route) + " \n");
                        }
                    }
                    if (objPropMapData.Department >= 0)
                    {
                        sb.Append("			AND t.type = " + objPropMapData.Department + " \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.IsPortal))
                    {
                        if (objPropMapData.IsPortal == "1")
                        {
                            sb.Append("			AND t.fBy= 'portal' \n");
                        }
                        if (objPropMapData.IsPortal == "0")
                        {
                            sb.Append("			AND t.fBy <> 'portal' \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Bremarks))
                    {
                        if (objPropMapData.Bremarks == "1")
                        {
                            sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') <> '' \n");
                        }
                        else
                        {
                            sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') = '' \n");
                        }
                    }
                    if (objPropMapData.Mobile > 0)
                    {
                        if (objPropMapData.Mobile == 2)
                        {
                            sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 2 \n");
                        }
                        if (objPropMapData.Mobile == 1)
                        {
                            sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 0 \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Category))
                    {
                        sb.Append("			AND t.Cat IN (" + objPropMapData.Category + ") \n");
                    }
                    if (objPropMapData.InvoiceID != 0)
                    {
                        if (objPropMapData.InvoiceID == 1)
                        {
                            sb.Append("			AND ISNULL(dp.Invoice,0) <> 0 \n");
                        }
                        else if (objPropMapData.InvoiceID == 2)
                        {
                            sb.Append("			AND ISNULL(dp.Invoice,0) = 0 AND ISNULL(dp.Charge,0) = 1 \n");
                        }
                    }
                    if (objPropMapData.Assigned != -1)
                    {
                        if (objPropMapData.Assigned == -2)
                        {
                            sb.Append("			AND t.Assigned <> 4 \n");
                        }
                        else
                        {
                            sb.Append("			AND t.Assigned = " + objPropMapData.Assigned + "  \n");
                        }
                    }
                    if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                    {

                        if (objPropMapData.Worker == "Active")
                        {
                            sb.Append("			AND ww.Status = 0 \n");
                        }
                        else if (objPropMapData.Worker == "Inactive")
                        {
                            sb.Append("			AND ww.Status = 1 \n");
                        }
                        else
                        {
                            sb.Append("			AND ww.fDesc = '" + objPropMapData.Worker.Replace("'", "''") + "' \n");
                        }
                    }

                    // Search value
                    if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && !string.IsNullOrEmpty(objPropMapData.SearchValue))
                    {
                        if (objPropMapData.SearchBy == "t.ID")
                        {
                            sb.Append("			AND t.ID = " + objPropMapData.SearchValue + " \n");
                        }
                        if (objPropMapData.SearchBy == "t.cat")
                        {
                            sb.Append("			AND t.Cat LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "t.WorkOrder")
                        {
                            sb.Append("			AND t.WorkOrder LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "t.fdesc")
                        {
                            sb.Append("			AND t.fDesc LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "t.descres")
                        {
                            sb.Append("			AND dp.DescRes LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "r.name")
                        {
                            sb.Append("			AND rou.Name LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.tag")
                        {
                            sb.Append("			AND l.Tag LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.ldesc4")
                        {
                            sb.Append("			AND l.Address LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.City")
                        {
                            sb.Append("			AND l.City LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.state")
                        {
                            sb.Append("			AND l.State LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.Zip")
                        {
                            sb.Append("			AND l.Zip LIKE '%" + objPropMapData.SearchValue + "%'		 \n");
                        }
                    }

                    //Ticket filters
                    foreach (var filter in filters)
                    {
                        if (filter.FilterColumn == "ID")
                        {
                            sb.Append("			AND t.ID IN (" + filter.FilterValue + ") \n");
                        }
                        if (filter.FilterColumn == "WorkOrder")
                        {
                            sb.Append("			AND t.Workorder= '" + filter.FilterValue + "' \n");
                        }
                        if (filter.FilterColumn == "invoiceno")
                        {
                            sb.Append("			AND dp.Invoice = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "Job")
                        {
                            sb.Append("			AND t.Job = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "locname")
                        {
                            sb.Append("			AND l.Tag LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "City")
                        {
                            sb.Append("			AND l.City LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "fullAddress")
                        {
                            sb.Append("			AND l.Address LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "dwork")
                        {
                            sb.Append("			AND t.DWork LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Name")
                        {
                            sb.Append("			AND rou.Name LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "cat")
                        {
                            sb.Append("			AND t.Cat LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Tottime")
                        {
                            sb.Append("			AND dp.Total = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "timediff")
                        {
                            sb.Append(@"		AND ROUND(CONVERT(NUMERIC(30, 2), (ISNULL(dp.Total, 0.00) - ( CONVERT(FLOAT, DATEDIFF(MILLISECOND, dp.TimeRoute, 
			                                    (CASE WHEN(CAST(dp.TimeSite AS TIME) < CAST(dp.TimeRoute AS TIME) AND CAST(dp.TimeComp AS TIME) < CAST(dp.TimeSite AS TIME)) THEN DATEADD(DAY, 2, dp.TimeComp)
				                                    ELSE((CASE
                                                        WHEN(CAST(dp.TimeSite AS TIME) < CAST(dp.TimeRoute AS TIME)
                                                            OR CAST(dp.TimeComp AS TIME) < CAST(dp.TimeSite AS TIME)) THEN DATEADD(DAY, 1, dp.TimeComp)
                                                        ELSE dp.TimeComp
                                                    END))
                                                END ))) / 1000 / 60 / 60 ) )), 1) = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "department")
                        {
                            sb.Append("			AND jt.Type LIKE '%" + filter.FilterValue + "%' \n");
                        }
                    }

                    #endregion
                }

                if (objPropMapData.Assigned == 4 || objPropMapData.Assigned == -1)
                {
                    #region TicketD table

                    sb.Append("		UNION ALL \n");
                    sb.Append("		SELECT  \n");
                    sb.Append("			t.[ID] \n");
                    sb.Append("			,t.[CDate] \n");
                    sb.Append("			,t.[DDate] \n");
                    sb.Append("			,t.[EDate] \n");
                    sb.Append("			,t.[fWork] \n");
                    sb.Append("			,t.[Job] \n");
                    sb.Append("			,t.[Loc] \n");
                    sb.Append("			,t.[Elev] \n");
                    sb.Append("			,t.[Type] \n");
                    sb.Append("			,t.[fDesc] \n");
                    sb.Append("			,t.[DescRes] \n");
                    sb.Append("			,t.[Status] \n");
                    sb.Append("			,t.[Invoice] \n");
                    sb.Append("			,t.[Level] \n");
                    sb.Append("			,t.[Est] \n");
                    sb.Append("			,t.[Cat] \n");
                    sb.Append("			,t.[Who] \n");
                    sb.Append("			,t.[fBy] \n");
                    sb.Append("			,t.[fLong] \n");
                    sb.Append("			,t.[Latt] \n");
                    sb.Append("			,t.[WageC] \n");
                    sb.Append("			,t.[Phase] \n");
                    sb.Append("			,t.[Car] \n");
                    sb.Append("			,t.[CallIn] \n");
                    sb.Append("			,t.[Mileage] \n");
                    sb.Append("			,t.[NT] \n");
                    sb.Append("			,t.[CauseID] \n");
                    sb.Append("			,t.[CauseDesc] \n");
                    sb.Append("			,t.[fGroup] \n");
                    sb.Append("			,t.[PriceL] \n");
                    sb.Append("			,t.[WorkOrder] \n");
                    sb.Append("			,jt.[Type] AS JobType \n");
                    sb.Append("			,w.[fDesc] AS Mech \n");
                    sb.Append("			,ro.[Name] AS OwnerName \n");
                    sb.Append("			,ro.[Address] AS OwnerAddress\n");
                    sb.Append("			,ro.[City] AS OwnerCity \n");
                    sb.Append("			,ro.[State] AS OwnerState \n");
                    sb.Append("			,ro.[Zip] AS OwnerZip \n");
                    sb.Append("			,ro.[Phone] AS OwnerPhone \n");
                    sb.Append("			,o.[Type] AS OwnerType \n");
                    sb.Append("			,l.[Address] AS Address  \n");
                    sb.Append("			,l.[City] \n");
                    sb.Append("			,l.[State] \n");
                    sb.Append("			,l.[Zip] \n");
                    sb.Append("			,rl.[Contact] AS MainContact \n");
                    sb.Append("			,rl.[Phone] AS ContactPhone \n");
                    sb.Append("			,l.[Tag] AS LocName \n");
                    sb.Append("			,l.[Address] AS LocAddress  \n");
                    sb.Append("			,l.[City] AS LocCity \n");
                    sb.Append("			,l.[State] AS LocState \n");
                    sb.Append("			,l.[Zip] AS LocZip \n");
                    sb.Append("			,CASE t.Assigned WHEN 6 THEN 'Voided' ELSE 'Completed' END AS Assignname \n");
                    sb.Append("         ,(SELECT STUFF((SELECT ', ' + CAST(e.Unit AS VARCHAR(1000))  \n");
                    sb.Append("         FROM Elev e WHERE e.id IN  \n");
                    sb.Append("             (SELECT me.elev_id FROM multiple_equipments me WHERE me.ticket_id = t.ID) \n");
                    sb.Append("             FOR XML PATH(''), Type).value('.', 'VARCHAR(1000)'),1,2,' ')) AS Unit \n");
                    sb.Append("		FROM TicketD t \n");
                    sb.Append("			INNER JOIN Loc l ON l.Loc = t.Loc  	 \n");
                    sb.Append("			INNER JOIN Rol rl ON rl.ID = l.Rol  	 \n");
                    sb.Append("			INNER JOIN Owner o ON o.ID = l.Owner  	 \n");
                    sb.Append("			INNER JOIN Rol ro ON ro.ID = o.Rol   \n");
                    sb.Append("			LEFT JOIN Route rou ON rou.ID = l.Route \n");
                    sb.Append("			LEFT JOIN Job j ON j.ID = t.Job \n");
                    sb.Append("			LEFT JOIN JobType jt ON jt.ID = j.Type  \n");
                    sb.Append("			LEFT JOIN tblWork m ON m.ID = rou.Mech \n");
                    sb.Append("			LEFT JOIN tblWork w ON w.ID = t.fWork \n");
                    sb.Append("			LEFT JOIN Elev e ON e.ID = t.Elev \n");
                    sb.Append("		WHERE t.EDate >= '" + objPropMapData.StartDate + "' AND t.EDate <= '" + objPropMapData.EndDate + "'  \n");

                    // If get from from Edit Location screen
                    if (objPropMapData.LocID > 0)
                    {
                        sb.Append("			AND t.Loc = " + objPropMapData.LocID + " \n");
                    }

                    // If get from from Edit Customer screen
                    if (objPropMapData.CustID > 0)
                    {
                        sb.Append("			AND l.Owner = " + objPropMapData.CustID + " \n");
                    }

                    // Advanced Search
                    if (!string.IsNullOrEmpty(objPropMapData.Supervisor))
                    {
                        sb.Append("			AND m.Super ='" + objPropMapData.Supervisor + "' \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.FilterCharge))
                    {
                        sb.Append("			AND (ISNULL(t.Charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge));

                        if (objPropMapData.FilterCharge == "1")
                        {
                            sb.Append(" OR ISNULL(t.Invoice,0) <> 0) \n");
                        }
                        else
                        {
                            sb.Append(" AND ISNULL(t.Invoice,0) = 0) \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.FilterReview))
                    {
                        sb.Append("			AND ISNULL(t.ClearCheck,0)= " + Convert.ToInt32(objPropMapData.FilterReview) + " \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Workorder))
                    {
                        sb.Append("			AND t.Workorder= '" + objPropMapData.Workorder + "' \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Route))
                    {
                        if (Convert.ToInt32(objPropMapData.Route) == 0)
                        {
                            sb.Append("			AND l.Route = 0 \n");
                        }
                        else
                        {
                            sb.Append("			AND rou.ID = " + Convert.ToInt32(objPropMapData.Route) + " \n");
                        }
                    }
                    if (objPropMapData.Department >= 0)
                    {
                        sb.Append("			AND t.type = " + objPropMapData.Department + " \n");
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.IsPortal))
                    {
                        if (objPropMapData.IsPortal == "1")
                        {
                            sb.Append("			AND t.fBy= 'portal' \n");
                        }
                        if (objPropMapData.IsPortal == "0")
                        {
                            sb.Append("			AND t.fBy <> 'portal' \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Bremarks))
                    {
                        if (objPropMapData.Bremarks == "1")
                        {
                            sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') <> '' \n");
                        }
                        else
                        {
                            sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') = '' \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Timesheet))
                    {
                        sb.Append("			AND ISNULL(t.TransferTime,0) = " + Convert.ToInt32(objPropMapData.Timesheet) + " \n");
                    }
                    if (objPropMapData.Mobile > 0)
                    {
                        if (objPropMapData.Mobile == 2)
                        {
                            sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 2 \n");
                        }
                        if (objPropMapData.Mobile == 1)
                        {
                            sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 0 \n");
                        }
                    }
                    if (!string.IsNullOrEmpty(objPropMapData.Category))
                    {
                        sb.Append("			AND t.Cat IN (" + objPropMapData.Category + ") \n");
                    }
                    if (objPropMapData.InvoiceID != 0)
                    {
                        if (objPropMapData.InvoiceID == 1)
                        {
                            sb.Append("			AND ISNULL(t.Invoice,0) <> 0 \n");
                        }
                        else if (objPropMapData.InvoiceID == 2)
                        {
                            sb.Append("			AND ISNULL(t.Invoice,0) = 0 AND ISNULL(t.Charge,0) = 1 \n");
                        }
                    }
                    if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                    {

                        if (objPropMapData.Worker == "Active")
                        {
                            sb.Append("			AND w.Status = 0 \n");
                        }
                        else if (objPropMapData.Worker == "Inactive")
                        {
                            sb.Append("			AND w.Status = 1 \n");
                        }
                        else
                        {
                            sb.Append("			AND w.fDesc = '" + objPropMapData.Worker.Replace("'", "''") + "' \n");
                        }
                    }

                    // Search value
                    if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && !string.IsNullOrEmpty(objPropMapData.SearchValue))
                    {
                        if (objPropMapData.SearchBy == "t.ID")
                        {
                            sb.Append("			AND t.ID = " + objPropMapData.SearchValue + " \n");
                        }
                        if (objPropMapData.SearchBy == "t.cat")
                        {
                            sb.Append("			AND t.Cat LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "t.WorkOrder")
                        {
                            sb.Append("			AND t.WorkOrder LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "t.fdesc")
                        {
                            sb.Append("			AND t.fDesc LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "t.descres")
                        {
                            sb.Append("			AND t.DescRes LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "r.name")
                        {
                            sb.Append("			AND rou.Name LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.tag")
                        {
                            sb.Append("			AND l.Tag LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.ldesc4")
                        {
                            sb.Append("			AND l.Address LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.City")
                        {
                            sb.Append("			AND l.City LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.state")
                        {
                            sb.Append("			AND l.State LIKE '%" + objPropMapData.SearchValue + "%' \n");
                        }
                        if (objPropMapData.SearchBy == "l.Zip")
                        {
                            sb.Append("			AND l.Zip LIKE '%" + objPropMapData.SearchValue + "%' n");
                        }
                    }

                    //Ticket filters
                    foreach (var filter in filters)
                    {
                        if (filter.FilterColumn == "ID")
                        {
                            sb.Append("			AND t.ID IN (" + filter.FilterValue + ") \n");
                        }
                        if (filter.FilterColumn == "WorkOrder")
                        {
                            sb.Append("			AND t.Workorder= '" + filter.FilterValue + "' \n");
                        }
                        if (filter.FilterColumn == "invoiceno")
                        {
                            sb.Append("			AND t.Invoice = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "Job")
                        {
                            sb.Append("			AND t.Job = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "locname")
                        {
                            sb.Append("			AND l.Tag LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "City")
                        {
                            sb.Append("			AND l.City LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "fullAddress")
                        {
                            sb.Append("			AND l.Address LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "dwork")
                        {
                            sb.Append("			AND w.fDesc LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Name")
                        {
                            sb.Append("			AND rou.Name LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "cat")
                        {
                            sb.Append("			AND t.Cat LIKE '%" + filter.FilterValue + "%' \n");
                        }
                        if (filter.FilterColumn == "Tottime")
                        {
                            sb.Append("			AND t.Total = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "timediff")
                        {
                            sb.Append(@"		AND ROUND(CONVERT(NUMERIC(30, 2), (ISNULL(t.Total, 0.00) - ( CONVERT(FLOAT, DATEDIFF(MILLISECOND, t.TimeRoute, 
                                                (CASE WHEN(CAST(t.TimeSite AS TIME) < CAST(t.TimeRoute AS TIME) AND CAST(t.TimeComp AS TIME) < CAST(t.TimeSite AS TIME)) THEN DATEADD(DAY, 2, t.TimeComp)

                                                    ELSE((CASE
                                                        WHEN(CAST(t.TimeSite AS TIME) < CAST(t.TimeRoute AS TIME)
                                                            OR CAST(t.TimeComp AS TIME) < CAST(t.TimeSite AS TIME)) THEN DATEADD(DAY, 1, t.TimeComp)
                                                        ELSE t.TimeComp
                                                    END))
                                                END))) / 1000 / 60 / 60 ) )), 1) = " + filter.FilterValue + " \n");
                        }
                        if (filter.FilterColumn == "department")
                        {
                            sb.Append("			AND jt.Type LIKE '%" + filter.FilterValue + "%' \n");
                        }
                    }

                    #endregion
                }

                sb.Append(") temp \n");
                sb.Append("WHERE 1 = 1 \n");

                if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && !string.IsNullOrEmpty(objPropMapData.SearchValue) && objPropMapData.SearchBy == "e.unit")
                {
                    sb.Append("	AND Unit LIKE '%" + objPropMapData.SearchValue + "%' \n");
                }

                var unitFilter = filters.FirstOrDefault(x => x.FilterColumn == "unit");
                if (unitFilter != null)
                {
                    sb.Append("			AND Unit LIKE '%" + unitFilter.FilterValue + "%' \n");
                }

                if (objPropMapData.Voided == 1)
                {
                    sb.Append("			AND temp.Assignname = 'Voided' \n");
                }

                if (objPropMapData.Assigned == 4 && objPropMapData.Voided != 1)
                {
                    sb.Append("			AND temp.Assignname <> 'Voided' \n");
                }

                sb.Append("ORDER BY temp.[ID] \n");

                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetOpenMaintenanceByEquipment(MapData objPropMapData, string defaultCategory)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT  \n");
                sb.Append("	t.ID \n");
                sb.Append("	,t.CDate \n");
                sb.Append("	,t.EDate \n");
                sb.Append("	,t.fWork \n");
                sb.Append("	,t.Job \n");
                sb.Append("	,t.LID AS Loc \n");
                sb.Append("	,t.LElev AS Elev \n");
                sb.Append("	,t.Type \n");
                sb.Append("	,t.fDesc \n");
                sb.Append("	,t.Cat \n");
                sb.Append("	,Upper(t.DWork) AS DWork \n");
                sb.Append("	,r.Name AS CustomerName \n");
                sb.Append("	,l.Tag AS LocName \n");
                sb.Append("	,l.Address AS LocAddress \n");
                sb.Append("	,e.Unit AS  EquipmentID \n");
                sb.Append("	,e.fDesc AS EquipmentDesc \n");
                sb.Append("	,e.Type AS EquipmentType \n");
                sb.Append("	,e.Category AS EquipmentCat \n");
                sb.Append("	,e.State AS EquipmentUnique \n");
                sb.Append("	,CASE c.SCycle \n");
                sb.Append("		WHEN -1 THEN 'Never' \n");
                sb.Append("		WHEN 0 THEN 'Monthly' \n");
                sb.Append("		WHEN 1 THEN 'Bi-Monthly' \n");
                sb.Append("		WHEN 2 THEN 'Quarterly' \n");
                sb.Append("		WHEN 3 THEN 'Semi-Annually' \n");
                sb.Append("		WHEN 4 THEN 'Annually' \n");
                sb.Append("		WHEN 5 THEN 'Weekly' \n");
                sb.Append("		WHEN 6 THEN 'Bi-Weekly' \n");
                sb.Append("		WHEN 7 THEN 'Every 13 Weeks' \n");
                sb.Append("		WHEN 10 THEN 'Every 2 Years' \n");
                sb.Append("		WHEN 8 THEN 'Every 3 Years' \n");
                sb.Append("		WHEN 9 THEN 'Every 5 Years' \n");
                sb.Append("		WHEN 11 THEN 'Every 7 Years' \n");
                sb.Append("		WHEN 12 THEN 'On-Demand' \n");
                sb.Append("		WHEN 13 THEN 'Daily' \n");
                sb.Append("		WHEN 14 THEN 'Twice a Month' \n");
                sb.Append("	END AS TicketFreq \n");
                sb.Append("FROM TicketO t \n");
                sb.Append("	INNER JOIN Loc l ON l.Loc = t.LID \n");
                sb.Append("	INNER JOIN Owner o ON o.ID = l.Owner \n");
                sb.Append("	INNER JOIN Rol r ON o.Rol = r.ID \n");
                sb.Append("	LEFT JOIN Contract c ON t.Job = c.Job \n");
                sb.Append("	LEFT JOIN multiple_equipments me ON me.ticket_id = t.ID \n");
                sb.Append("	LEFT JOIN Elev e ON e.ID = me.elev_id \n");
                sb.Append("WHERE t.Assigned <> 4 AND t.Cat = '" + defaultCategory + "' \n");

                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetOpenMaintenanceByEquipment(GetOpenMaintenanceByEquipmentParam _GetOpenMaintenanceByEquipment, string ConnectionString, string defaultCategory)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT  \n");
                sb.Append("	t.ID \n");
                sb.Append("	,t.CDate \n");
                sb.Append("	,t.EDate \n");
                sb.Append("	,t.fWork \n");
                sb.Append("	,t.Job \n");
                sb.Append("	,t.LID AS Loc \n");
                sb.Append("	,t.LElev AS Elev \n");
                sb.Append("	,t.Type \n");
                sb.Append("	,t.fDesc \n");
                sb.Append("	,t.Cat \n");
                sb.Append("	,Upper(t.DWork) AS DWork \n");
                sb.Append("	,r.Name AS CustomerName \n");
                sb.Append("	,l.Tag AS LocName \n");
                sb.Append("	,l.Address AS LocAddress \n");
                sb.Append("	,e.Unit AS  EquipmentID \n");
                sb.Append("	,e.fDesc AS EquipmentDesc \n");
                sb.Append("	,e.Type AS EquipmentType \n");
                sb.Append("	,e.Category AS EquipmentCat \n");
                sb.Append("	,e.State AS EquipmentUnique \n");
                sb.Append("	,CASE c.SCycle \n");
                sb.Append("		WHEN -1 THEN 'Never' \n");
                sb.Append("		WHEN 0 THEN 'Monthly' \n");
                sb.Append("		WHEN 1 THEN 'Bi-Monthly' \n");
                sb.Append("		WHEN 2 THEN 'Quarterly' \n");
                sb.Append("		WHEN 3 THEN 'Semi-Annually' \n");
                sb.Append("		WHEN 4 THEN 'Annually' \n");
                sb.Append("		WHEN 5 THEN 'Weekly' \n");
                sb.Append("		WHEN 6 THEN 'Bi-Weekly' \n");
                sb.Append("		WHEN 7 THEN 'Every 13 Weeks' \n");
                sb.Append("		WHEN 10 THEN 'Every 2 Years' \n");
                sb.Append("		WHEN 8 THEN 'Every 3 Years' \n");
                sb.Append("		WHEN 9 THEN 'Every 5 Years' \n");
                sb.Append("		WHEN 11 THEN 'Every 7 Years' \n");
                sb.Append("		WHEN 12 THEN 'On-Demand' \n");
                sb.Append("		WHEN 13 THEN 'Daily' \n");
                sb.Append("		WHEN 14 THEN 'Twice a Month' \n");
                sb.Append("	END AS TicketFreq \n");
                sb.Append("FROM TicketO t \n");
                sb.Append("	INNER JOIN Loc l ON l.Loc = t.LID \n");
                sb.Append("	INNER JOIN Owner o ON o.ID = l.Owner \n");
                sb.Append("	INNER JOIN Rol r ON o.Rol = r.ID \n");
                sb.Append("	LEFT JOIN Contract c ON t.Job = c.Job \n");
                sb.Append("	LEFT JOIN multiple_equipments me ON me.ticket_id = t.ID \n");
                sb.Append("	LEFT JOIN Elev e ON e.ID = me.elev_id \n");
                sb.Append("WHERE t.Assigned <> 4 AND t.Cat = '" + defaultCategory + "' \n");

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetListVendorDemand(Customer objPropCustomer)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT DISTINCT \n");
            sb.Append("	v.ID, \n");
            sb.Append("	r.Name \n");
            sb.Append("FROM Vendor v WITH(NOLOCK) \n");
            sb.Append("	INNER JOIN PO p WITH(NOLOCK) ON p.Vendor = v.ID \n");
            sb.Append("	INNER JOIN Rol r WITH(NOLOCK) ON r.ID = v.Rol \n");
            sb.Append("ORDER BY r.Name \n");

            return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, sb.ToString());
        }

        public DataSet GetProjectVendorDemand(Customer objPropCustomer, List<RetainFilter> filters, string vendor, string department, int includeClosedProject = 0, int includeClosedPO = 0)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT DISTINCT \n");
            sb.Append("	p.Vendor, \n");
            sb.Append("	r.Name AS VendorName, \n");
            sb.Append("	p.PO, \n");
            sb.Append("	p.fDate, \n");
            sb.Append("	p.Due, \n");
            sb.Append("	p.fDesc, \n");
            sb.Append("	p.Status, \n");
            sb.Append("	p.Amount, \n");
            sb.Append("	p.SalesOrderNo, \n");
            sb.Append(" (CASE ISNULL(p.Status, 0) \n");
            sb.Append("     WHEN 0 THEN 'Open' \n");
            sb.Append("     WHEN 1 THEN 'Closed' \n");
            sb.Append("     WHEN 2 THEN 'Void'  \n");
            sb.Append("     WHEN 3 THEN 'Partial-Quantity' \n");
            sb.Append("     WHEN 4 THEN 'Partial-Amount' \n");
            sb.Append("     WHEN 5 THEN 'Closed At Receive PO' \n");
            sb.Append(" END) AS StatusName, \n");
            sb.Append("	j.ID AS Job, \n");
            sb.Append("	rpo.ID AS ReceiveID, \n");
            sb.Append("	rpo.Amount AS ReceiveAmount, \n");
            sb.Append("	j.fDesc AS JobDesc, \n");
            sb.Append("	jt.Type AS Department, \n");
            sb.Append("	ro.Name AS CustomerName, \n");
            sb.Append("	l.Tag AS LocName, \n");
            sb.Append("	l.Address AS LocAddress, \n");
            sb.Append("	l.City AS LocCity, \n");
            sb.Append("	l.State AS LocState, \n");
            sb.Append("	l.Zip AS LocZip \n");
            sb.Append("FROM PO p WITH(NOLOCK) \n");
            sb.Append("	LEFT JOIN ReceivePO rpo WITH(NOLOCK) ON rpo.PO = p.PO \n");
            sb.Append("	INNER JOIN Vendor v WITH(NOLOCK) ON v.ID = p.Vendor \n");
            sb.Append("	INNER JOIN Rol r WITH(NOLOCK) ON r.ID = v.Rol \n");
            sb.Append("	INNER JOIN POItem poi WITH(NOLOCK) ON poi.PO = p.PO \n");
            sb.Append("	INNER JOIN Job j WITH(NOLOCK) ON j.ID = poi.Job \n");
            sb.Append("	INNER JOIN JobType jt WITH(NOLOCK) ON jt.ID = j.Type \n");
            sb.Append("	INNER JOIN Loc l WITH(NOLOCK) ON l.Loc = j.Loc \n");
            sb.Append("	INNER JOIN Owner o WITH(NOLOCK) ON o.ID = j.Owner \n");
            sb.Append("	INNER JOIN Rol ro WITH(NOLOCK) ON ro.ID = o.Rol \n");

            if (!string.IsNullOrEmpty(objPropCustomer.SearchBy) && !string.IsNullOrEmpty(objPropCustomer.SearchValue) && objPropCustomer.SearchBy == "t.title")
            {
                sb.Append("	LEFT JOIN Team t WITH(NOLOCK) ON t.JobID = j.ID \n");
            }

            // Join when there is a filter
            if (filters.FindIndex(x => x.FilterColumn == "TemplateDesc") > -1)
            {
                sb.Append("	LEFT JOIN JobT tem WITH(NOLOCK) ON tem.Id = j.Template \n");
            }

            if (filters.FindIndex(x => x.FilterColumn == "Salesperson") > -1)
            {
                sb.Append("	LEFT JOIN Terr ter WITH(NOLOCK) ON ter.Id = l.Terr \n");
            }

            if (filters.FindIndex(x => x.FilterColumn == "Route") > -1)
            {
                sb.Append("	LEFT JOIN Route rt WITH(NOLOCK) ON rt.Id = l.Route \n");
                sb.Append("	LEFT JOIN tblWork w WITH(NOLOCK) ON w.Id = rt.Mech \n");
            }

            if (filters.FindIndex(x => x.FilterColumn == "BuildingType") > -1)
            {
                sb.Append("	LEFT JOIN BusinessType bt WITH(NOLOCK) ON bt.ID = l.BusinessType \n");
            }

            if (filters.FindIndex(x => x.FilterColumn == "ProjectManagerUserName") > -1)
            {
                sb.Append("	LEFT JOIN Emp pm WITH(NOLOCK) ON pm.ID = j.ProjectManagerUserID \n");
            }

            if (filters.FindIndex(x => x.FilterColumn == "SupervisorUserName") > -1)
            {
                sb.Append("	LEFT JOIN Emp su WITH(NOLOCK) ON su.ID = j.SupervisorUserID \n");
            }

            if (filters.FindIndex(x => x.FilterColumn == "NotBilledYet" || x.FilterColumn == "NRev" || x.FilterColumn == "NProfit" || x.FilterColumn == "NRatio") > -1)
            {
                sb.Append("	LEFT JOIN ( \n");
                sb.Append("	    SELECT SUM(ISNULL(JobI.Amount,0)) Amt, JobI.Job FROM JobI WHERE ISNULL(JobI.Type, 0) <> 1 \n");

                if (objPropCustomer.Range == 2 || objPropCustomer.Range == 5)
                {
                    sb.Append("	    AND JobI.fDate >= '" + objPropCustomer.StartDate + "' AND JobI.fDate <= '" + objPropCustomer.EndDate + "' \n");
                }

                sb.Append("	    GROUP BY JobI.Job \n");
                sb.Append("	) jobCostNRev ON jobCostNRev.Job = j.ID \n");
            }

            if (filters.FindIndex(x => x.FilterColumn == "NLabor") > -1)
            {
                sb.Append("	LEFT JOIN ( \n");
                sb.Append("	    SELECT SUM(ISNULL(JobI.Amount,0)) Amt, JobI.Job FROM JobI \n");
                sb.Append("	    INNER JOIN JobTItem ON JobTItem.Line = JobI.Phase AND JobTItem.Job = JobI.Job \n");
                sb.Append("	    INNER JOIN BOM ON BOM.JobTItemID = JobTItem.ID \n");
                sb.Append("	    INNER JOIN BOMT ON BOMT.ID = BOM.Type \n");
                sb.Append("	    WHERE ISNULL(JobI.Type, 0) <> 0 \n");
                sb.Append("	    AND ISNULL(JobI.Labor,0) = 1 AND JobI.fDesc NOT IN ('Mileage on Ticket','Expenses on Ticket') \n");

                if (objPropCustomer.Range == 2 || objPropCustomer.Range == 5)
                {
                    sb.Append("	    AND JobI.fDate >= '" + objPropCustomer.StartDate + "' AND JobI.fDate <= '" + objPropCustomer.EndDate + "' \n");
                }

                sb.Append("	    GROUP BY JobI.Job \n");
                sb.Append("	) jobCostLabor ON jobCostLabor.Job = j.ID \n");
            }

            if (filters.FindIndex(x => x.FilterColumn == "NMat") > -1)
            {
                sb.Append("	LEFT JOIN ( \n");
                sb.Append("	    SELECT SUM(ISNULL(JobI.Amount,0)) Amt, JobI.Job FROM JobI \n");
                sb.Append("	    INNER JOIN JobTItem ON JobTItem.Line = JobI.Phase AND JobTItem.Job = JobI.Job \n");
                sb.Append("	    INNER JOIN BOM ON BOM.JobTItemID = JobTItem.ID \n");
                sb.Append("	    INNER JOIN BOMT ON BOMT.ID = BOM.Type AND (BOMT.Type = 'Materials' OR BOMT.Type = 'Inventory') \n");
                sb.Append("	    WHERE ISNULL(JobI.Type, 0) <> 0 \n");
                sb.Append("	    AND JobI.fDesc NOT IN ('Mileage on Ticket','Expenses on Ticket') \n");
                sb.Append("	    AND (JobI.TransID > 0 OR ISNULL(JobI.Labor, 0) = 0)  \n");

                if (objPropCustomer.Range == 2 || objPropCustomer.Range == 5)
                {
                    sb.Append("	    AND JobI.fDate >= '" + objPropCustomer.StartDate + "' AND JobI.fDate <= '" + objPropCustomer.EndDate + "' \n");
                }

                sb.Append("	    GROUP BY JobI.Job \n");
                sb.Append("	) jobCostMaterial ON jobCostMaterial.Job = j.ID \n");
            }

            if (filters.FindIndex(x => x.FilterColumn == "NOMat") > -1)
            {
                sb.Append("	LEFT JOIN ( \n");
                sb.Append("	    SELECT SUM(ISNULL(JobI.Amount,0)) Amt, JobI.Job FROM JobI \n");
                sb.Append("	    INNER JOIN JobTItem ON JobTItem.Line = JobI.Phase AND JobTItem.Job = JobI.Job \n");
                sb.Append("	    INNER JOIN BOM ON BOM.JobTItemID = JobTItem.ID \n");
                sb.Append("	    INNER JOIN BOMT ON BOMT.ID = BOM.Type \n");
                sb.Append("	    WHERE ISNULL(JobI.Type, 0) <> 0 \n");
                sb.Append("	    AND (((JobI.TransID > 0 OR ISNULL(JobI.Labor, 0) = 0) AND (BOMT.Type <> 'Materials' AND BOMT.Type <> 'Labor' AND BOMT.Type <> 'Inventory') \n");
                sb.Append("	    AND JobI.fDesc NOT IN ('Mileage on Ticket','Expenses on Ticket')) OR JobI.fDesc IN ('Mileage on Ticket','Expenses on Ticket'))  \n");

                if (objPropCustomer.Range == 2 || objPropCustomer.Range == 5)
                {
                    sb.Append("	    AND JobI.fDate >= '" + objPropCustomer.StartDate + "' AND JobI.fDate <= '" + objPropCustomer.EndDate + "' \n");
                }

                sb.Append("	    GROUP BY JobI.Job \n");
                sb.Append("	) jobCostOther ON jobCostOther.Job = j.ID \n");
            }

            if (filters.FindIndex(x => x.FilterColumn == "NCost" || x.FilterColumn == "NProfit" || x.FilterColumn == "NRatio") > -1)
            {
                sb.Append("	LEFT JOIN ( \n");
                sb.Append("	    SELECT SUM(ISNULL(JobI.Amount,0)) Amt, JobI.Job FROM JobI WHERE ISNULL(JobI.Type, 0) = 1 \n");

                if (objPropCustomer.Range == 2 || objPropCustomer.Range == 5)
                {
                    sb.Append("	    AND JobI.fDate >= '" + objPropCustomer.StartDate + "' AND JobI.fDate <= '" + objPropCustomer.EndDate + "' \n");
                }

                sb.Append("	    GROUP BY JobI.Job \n");
                sb.Append("	) jobCostNCost ON jobCostNCost.Job = j.ID \n");
            }

            if (filters.FindIndex(x => x.FilterColumn == "NHour") > -1)
            {
                sb.Append("	LEFT JOIN ( \n");
                sb.Append("	    SELECT SUM(ISNULL(t.Reg,0) + ISNULL(t.RegTrav,0) + ISNULL(t.OT, 0) + ISNULL(t.OTTrav, 0) + ISNULL(t.NT, 0) + ISNULL(t.NTTrav, 0) + ISNULL(t.DT, 0) + ISNULL(t.DTTrav, 0) + ISNULL(t.TT, 0)) AS ActualHr, \n");
                sb.Append("	    t.Job \n");
                sb.Append("	    FROM TicketD t \n");

                if (objPropCustomer.Range == 2 || objPropCustomer.Range == 5)
                {
                    sb.Append("	    AND t.EDate >= '" + objPropCustomer.StartDate + "' AND t.EDate <= '" + objPropCustomer.EndDate + "' \n");
                }

                sb.Append("	    GROUP BY t.Job \n");
                sb.Append("	) jobCostNHour ON jobCostNHour.Job = j.ID \n");
            }

            if (filters.FindIndex(x => x.FilterColumn == "NComm") > -1)
            {
                sb.Append("	LEFT JOIN ( \n");
                sb.Append("	    SELECT SUM(ISNULL(p.Balance,0)) AS Amt, p.Job \n");
                sb.Append("	    FROM POItem p \n");
                sb.Append("	    INNER JOIN PO ON p.PO = PO.PO \n");
                sb.Append("	    WHERE po.Status in (0,3,4) \n");

                if (objPropCustomer.Range == 2 || objPropCustomer.Range == 5)
                {
                    sb.Append("	    AND po.fDate >= '" + objPropCustomer.StartDate + "' AND po.fDate <= '" + objPropCustomer.EndDate + "' \n");
                }

                sb.Append("	    GROUP BY p.Job \n");
                sb.Append("	) jobCostComm ON jobCostComm.Job = j.ID \n");
            }

            if (filters.FindIndex(x => x.FilterColumn == "ReceivePO" || x.FilterColumn == "NComm") > -1)
            {
                sb.Append("	LEFT JOIN ( \n");
                sb.Append("	    SELECT SUM(ISNULL(rp.Amount,0)) AS Amt, p.Job \n");
                sb.Append("	    FROM RPOItem rp \n");
                sb.Append("	    INNER JOIN ReceivePO r ON r.ID = rp.ReceivePO \n");
                sb.Append("	    LEFT JOIN POItem p ON r.PO = p.PO AND rp.POLine = p.Line \n");
                sb.Append("	    WHERE ISNULL(r.Status,0) = 0 \n");

                if (objPropCustomer.Range == 2 || objPropCustomer.Range == 5)
                {
                    sb.Append("	    AND r.fDate >= '" + objPropCustomer.StartDate + "' AND r.fDate <= '" + objPropCustomer.EndDate + "' \n");
                }

                sb.Append("	    GROUP BY p.Job \n");
                sb.Append("	) jobCostRevPO ON jobCostRevPO.Job = j.ID \n");
            }

            // Condition query
            sb.Append("WHERE (" + includeClosedProject + " = 1 OR j.Status = 0) \n");
            sb.Append(" AND (" + includeClosedPO + " = 1 OR p.Status = 0 OR p.Status = 3) AND p.Status <> 2 \n");

            if (!string.IsNullOrEmpty(vendor))
            {
                sb.Append("	AND p.Vendor IN(" + vendor + ") \n");
            }

            if (!string.IsNullOrEmpty(department))
            {
                sb.Append("	AND j.Type IN(" + department + ") \n");
            }

            if (objPropCustomer.Range == 2 || objPropCustomer.Range == 5)
            {
                sb.Append("	    AND j.ID IN (SELECT DISTINCT Job FROM JobI WITH(NOLOCK) WHERE fDate >= '" + objPropCustomer.StartDate + "' AND fDate <= '" + objPropCustomer.EndDate + "') \n");
            }

            // Search value
            if (!string.IsNullOrEmpty(objPropCustomer.SearchBy) && !string.IsNullOrEmpty(objPropCustomer.SearchValue))
            {
                if (objPropCustomer.SearchBy == "j.id")
                {
                    sb.Append(" AND j.ID = " + objPropCustomer.SearchValue + " \n");
                }
                else if (objPropCustomer.SearchBy == "j.fdate")
                {
                    sb.Append(" AND j.fDate = " + objPropCustomer.SearchValue + " \n");
                }
                else if (objPropCustomer.SearchBy == "l.tag")
                {
                    sb.Append(" AND l.Tag LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                }
                else if (objPropCustomer.SearchBy == "l.City")
                {
                    sb.Append(" AND l.City LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                }
                else if (objPropCustomer.SearchBy == "l.State")
                {
                    sb.Append(" AND l.State LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                }
                else if (objPropCustomer.SearchBy == "j.Status")
                {
                    sb.Append(" AND j.Status = " + objPropCustomer.SearchValue + " \n");
                }
                else if (objPropCustomer.SearchBy == "j.fdesc")
                {
                    sb.Append(" AND j.fDesc LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                }
                else if (objPropCustomer.SearchBy == "r.Name")
                {
                    sb.Append(" AND ro.Name LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                }
                else if (objPropCustomer.SearchBy == "j.PWIP")
                {
                    sb.Append(" AND j.PWIP = " + objPropCustomer.SearchValue + " \n");
                }
                else if (objPropCustomer.SearchBy == "j.Certified")
                {
                    sb.Append(" AND j.Certified = " + objPropCustomer.SearchValue + " \n");
                }
                else if (objPropCustomer.SearchBy == "t.title")
                {
                    sb.Append(" AND t.Title = '" + objPropCustomer.SearchValue + "' \n");
                    sb.Append(" AND t.MomUserID LIKE '%" + objPropCustomer.Username + "%' \n");
                }
                else
                {
                    // For Custom1 -> Custom20
                    sb.Append(" AND " + objPropCustomer.SearchBy + " = " + objPropCustomer.SearchValue + "\n");
                }
            }

            // Filter on grid
            foreach (var filter in filters)
            {
                if (filter.FilterColumn == "Customer")
                {
                    sb.Append(" AND ro.Name LIKE '%" + filter.FilterValue + "%' \n");
                }
                if (filter.FilterColumn == "Tag")
                {
                    sb.Append(" AND l.Tag LIKE '%" + filter.FilterValue + "%' \n");
                }
                if (filter.FilterColumn == "ID")
                {
                    sb.Append(" AND j.ID = " + filter.FilterValue + " \n");
                }
                if (filter.FilterColumn == "fdesc")
                {
                    sb.Append(" AND j.fDesc LIKE '%" + filter.FilterValue + "%' \n");
                }
                if (filter.FilterColumn == "Status")
                {
                    sb.Append(" AND (CASE j.Status WHEN 0 THEN 'Open' WHEN 1 THEN 'Closed' WHEN 2 THEN 'Hold' WHEN 3 THEN 'Completed' END) LIKE '%" + filter.FilterValue + "%' \n");
                }
                if (filter.FilterColumn == "CType")
                {
                    sb.Append(" AND j.CType LIKE '%" + filter.FilterValue + "%' \n");
                }
                if (filter.FilterColumn == "TemplateDesc")
                {
                    sb.Append(" AND tem.fDesc LIKE '%" + filter.FilterValue + "%' \n");
                }
                if (filter.FilterColumn == "Type")
                {
                    sb.Append(" AND jt.Type LIKE '%" + filter.FilterValue + "%' \n");
                }
                if (filter.FilterColumn == "Salesperson")
                {
                    sb.Append(" AND ter.Name LIKE '%" + filter.FilterValue + "%' \n");
                }
                if (filter.FilterColumn == "Route")
                {
                    sb.Append(" AND (CASE WHEN w.fDesc = rt.Name THEN rt.Name ELSE rt.Name  + '-' + w.fDesc END) LIKE '%" + filter.FilterValue + "%' \n");
                }
                if (filter.FilterColumn == "ContractPrice")
                {
                    sb.Append(" AND ISNULL(j.BRev, 0) = " + decimal.Parse(filter.FilterValue) + " \n");
                }
                if (filter.FilterColumn == "NotBilledYet")
                {
                    sb.Append(" AND (ISNULL(j.BRev, 0) - ISNULL(jobCostNRev.Amt, 0)) = " + decimal.Parse(filter.FilterValue) + " \n");
                }
                if (filter.FilterColumn == "NRev")
                {
                    sb.Append(" AND ISNULL(jobCostNRev.Amt, 0) = " + decimal.Parse(filter.FilterValue) + " \n");
                }
                if (filter.FilterColumn == "NHour")
                {
                    sb.Append(" AND ISNULL(jobCostNHour.ActualHr, 0) = " + decimal.Parse(filter.FilterValue) + " \n");
                }
                if (filter.FilterColumn == "NLabor")
                {
                    sb.Append(" AND ISNULL(jobCostLabor.Amt, 0) = " + decimal.Parse(filter.FilterValue) + " \n");
                }
                if (filter.FilterColumn == "NMat")
                {
                    sb.Append(" AND ISNULL(jobCostMaterial.Amt, 0) = " + decimal.Parse(filter.FilterValue) + " \n");
                }
                if (filter.FilterColumn == "NOMat")
                {
                    sb.Append(" AND ISNULL(jobCostOther.Amt, 0) = " + decimal.Parse(filter.FilterValue) + " \n");
                }
                if (filter.FilterColumn == "NCost")
                {
                    sb.Append(" AND ISNULL(jobCostNCost.Amt, 0) = " + decimal.Parse(filter.FilterValue) + " \n");
                }
                if (filter.FilterColumn == "TotalBudgetedExpense")
                {
                    sb.Append(" AND (ISNULL(j.BMat,0) + ISNULL(j.BLabor,0) + ISNULL(j.BOther,0)) = " + filter.FilterValue + " \n");
                }
                if (filter.FilterColumn == "NComm")
                {
                    sb.Append(" AND (ISNULL(jobCostComm.Amt, 0) + ISNULL(jobCostRevPO.Amt, 0)) = " + decimal.Parse(filter.FilterValue) + " \n");
                }
                if (filter.FilterColumn == "ReceivePO")
                {
                    sb.Append(" AND ISNULL(jobCostRevPO.Amt, 0) = " + decimal.Parse(filter.FilterValue) + " \n");
                }
                if (filter.FilterColumn == "NProfit")
                {
                    sb.Append(" AND (ISNULL(jobCostNRev.Amt, 0) - ISNULL(jobCostNCost.Amt, 0)) = " + decimal.Parse(filter.FilterValue) + " \n");
                }
                if (filter.FilterColumn == "NRatio")
                {
                    sb.Append(" AND (CASE ISNULL(jobCostNRev.Amt, 0) WHEN 0 THEN 0 ELSE ROUND((ISNULL(jobCostNRev.Amt, 0) - ISNULL(jobCostNCost.Amt, 0)) / jobCostNRev.Amt  * 100, 2) END) = " + decimal.Parse(filter.FilterValue) + " \n");
                }
                if (filter.FilterColumn == "ProjectManagerUserName")
                {
                    sb.Append(" AND pm.CallSign LIKE '%" + filter.FilterValue + "%' \n");
                }
                if (filter.FilterColumn == "SupervisorUserName")
                {
                    sb.Append(" AND su.CallSign LIKE '%" + filter.FilterValue + "%' \n");
                }
                if (filter.FilterColumn == "LocationType")
                {
                    sb.Append(" AND l.Type LIKE '%" + filter.FilterValue + "%' \n");
                }
                if (filter.FilterColumn == "BuildingType")
                {
                    sb.Append(" AND bt.Description  LIKE '%" + filter.FilterValue + "%' \n");
                }
            }

            return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, sb.ToString());
        }

        public DataSet GetEquipmentContractByCustomer(Contracts objPropContracts, List<RetainFilter> filters, bool includeClose)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT  \n");
                sb.Append("	j.ID \n");
                sb.Append("	,r.Name AS Customer \n");
                sb.Append("	,l.Tag \n");
                sb.Append("	,l.Route \n");
                sb.Append("	,l.Maint	 \n");
                sb.Append("	,j.fDesc \n");
                sb.Append("	,j.Type \n");
                sb.Append("	,j.Loc \n");
                sb.Append("	,j.Owner \n");
                sb.Append("	,j.Elev \n");
                sb.Append("	,j.Status \n");
                sb.Append("	,j.PO \n");
                sb.Append("	,j.Remarks \n");
                sb.Append("	,j.Rev \n");
                sb.Append("	,j.Mat \n");
                sb.Append("	,j.Labor \n");
                sb.Append("	,j.Cost \n");
                sb.Append("	,j.Profit \n");
                sb.Append("	,j.Ratio \n");
                sb.Append("	,j.Reg \n");
                sb.Append("	,j.OT \n");
                sb.Append("	,j.DT \n");
                sb.Append("	,j.TT \n");
                sb.Append("	,j.Hour \n");
                sb.Append("	,j.BRev \n");
                sb.Append("	,j.BMat \n");
                sb.Append("	,j.BLabor \n");
                sb.Append("	,j.BCost \n");
                sb.Append("	,j.BProfit \n");
                sb.Append("	,j.BRatio \n");
                sb.Append("	,j.BHour \n");
                sb.Append("	,j.Template \n");
                sb.Append("	,j.fDate \n");
                sb.Append("	,j.Comm \n");
                sb.Append("	,j.WageC \n");
                sb.Append("	,j.NT \n");
                sb.Append("	,j.Post \n");
                sb.Append("	,j.EN \n");
                sb.Append("	,j.Certified \n");
                sb.Append("	,j.Apprentice \n");
                sb.Append("	,j.UseCat \n");
                sb.Append("	,j.UseDed \n");
                sb.Append("	,j.BillRate \n");
                sb.Append("	,j.Markup \n");
                sb.Append("	,j.PType \n");
                sb.Append("	,j.Charge \n");
                sb.Append("	,j.Amount \n");
                sb.Append("	,j.GL \n");
                sb.Append("	,j.GLRev \n");
                sb.Append("	,j.GandA \n");
                sb.Append("	,j.OHLabor \n");
                sb.Append("	,j.LastOH \n");
                sb.Append("	,j.etc \n");
                sb.Append("	,j.ETCModifier \n");
                sb.Append("	,j.FP \n");
                sb.Append("	,j.fGroup \n");
                sb.Append("	,j.CType \n");
                sb.Append("	,ElevsC.ElevC as Elevs \n");
                sb.Append(" ,l.Address \n");
                sb.Append("	,l.City \n");
                sb.Append("	,l.Zip \n");
                sb.Append("	,l.State \n");
                sb.Append("FROM Job j \n");
                sb.Append("	INNER JOIN Contract ct ON ct.Job = j.ID \n");
                sb.Append("	INNER JOIN Loc l ON l.Loc = ct.Loc \n");
                sb.Append("	INNER JOIN Owner o ON o.ID = ct.Owner \n");
                sb.Append("	INNER JOIN Rol r on r.ID = o.Rol  \n");
                sb.Append("	LEFT JOIN Terr t ON t.ID = l.Terr  \n");
                sb.Append("	LEFT JOIN Terr t2 ON t2.ID = t.ID \n");
                //sb.Append("	LEFT JOIN tblJoinElevJob el ON el.Job = ct.Job \n");
                //sb.Append("	LEFT JOIN Elev e ON e.ID = el.Elev \n");
                sb.Append("	LEFT JOIN (Select ct.Job,COUNT(tb.Elev) ElevC From tblJoinElevJob tb Right Join Contract ct on tb.Job=ct.Job Group By ct.Job ) ElevsC  on ElevsC.Job=ct.Job \n");

                if (objPropContracts.EN == 1)
                {
                    sb.Append("	LEFT JOIN JOIN Branch B ON r.EN = B.ID \n");
                    sb.Append("	LEFT JOIN JOIN tblUserCO UC ON UC.CompanyID = B.ID \n");
                }

                sb.Append("WHERE 1 = 1 AND  j.type = 0 \n");

                if (!includeClose)
                {
                    sb.Append("	AND ct.Status = 0 \n");
                }

                if (!string.IsNullOrEmpty(objPropContracts.SearchBy) && !string.IsNullOrEmpty(objPropContracts.SearchValue))
                {
                    if (objPropContracts.SearchBy == "r.name" || objPropContracts.SearchBy == "l.tag" || objPropContracts.SearchBy == "r.State")
                    {
                        sb.Append(" AND " + objPropContracts.SearchBy + " LIKE '%" + objPropContracts.SearchValue + "%'");
                    }
                    else if (objPropContracts.SearchBy == "B.Name" && objPropContracts.EN == 1)
                    {
                        sb.Append(" AND UC.IsSel= 1 AND r.EN =" + objPropContracts.SearchValue + " AND UC.UserID =" + objPropContracts.UserID);
                    }
                    else if (objPropContracts.SearchBy == "j.SPHandle")
                    {
                        if (objPropContracts.SearchValue != "-1")
                        {
                            sb.Append(" AND j.SPHandle  = '" + objPropContracts.SearchValue + "'");
                        }
                    }
                    else if(objPropContracts.SearchBy == "c.Status")
                    {
                        sb.Append(" AND ct.Status "  + " = '" + objPropContracts.SearchValue + "'");
                    }
                    else
                    {
                        sb.Append(" AND " + objPropContracts.SearchBy + " = '" + objPropContracts.SearchValue + "'");
                    }
                }

                if (objPropContracts.EN == 1)
                {
                    sb.Append("AND UC.IsSel= 1 AND UC.UserID = " + objPropContracts.UserID);
                }

                if (objPropContracts.ExpirationDate != DateTime.MinValue)
                {
                    int days = DateTime.DaysInMonth(objPropContracts.ExpirationDate.Year, objPropContracts.ExpirationDate.Month);
                    int date = days - objPropContracts.ExpirationDate.Day;
                    DateTime datec = objPropContracts.ExpirationDate.AddDays(date);

                    sb.Append("AND ExpirationDate <= '" + datec + "'");
                }

                foreach (var obj in filters)
                {
                    if (obj.FilterColumn == "Job")
                    {
                        sb.Append("\n and ct.Job ='" + obj.FilterValue.Trim() + "' ");
                    }
                    if (obj.FilterColumn == "locid")
                    {
                        sb.Append("\n and l.ID like '%" + obj.FilterValue.Trim() + "%' ");
                    }
                    if (obj.FilterColumn == "Tag")
                    {
                        sb.Append("\n and l.Tag like '%" + obj.FilterValue.Trim() + "%' ");
                    }
                    if (obj.FilterColumn == "Name")
                    {
                        sb.Append("\n and r.Name like '%" + obj.FilterValue.Trim() + "%' ");
                    }
                    if (obj.FilterColumn == "CType")
                    {
                        sb.Append("\n and j.CType like '%" + obj.FilterValue.Trim() + "%' ");
                    }
                    if (obj.FilterColumn == "fdesc")
                    {
                        sb.Append("\n and e.fDesc like '%" + obj.FilterValue.Trim() + "%' ");
                    }
                    if (obj.FilterColumn == "BAmt")
                    {
                        sb.Append("\n and ct.BAmt ='" + obj.FilterValue.Trim() + "'");
                    }
                    if (obj.FilterColumn == "Hours")
                    {
                        sb.Append("\n and ct.Hours ='" + obj.FilterValue.Trim() + "'");
                    }
                    if (obj.FilterColumn == "SREMARKS")
                    {
                        sb.Append("\n and J.SREMARKS like '%" + obj.FilterValue.Trim() + "%' ");
                    }
                    if (obj.FilterColumn == "Salesperson")
                    {
                        sb.Append("\n and t.Name like '%" + obj.FilterValue.Trim() + "%' ");
                    }
                    if (obj.FilterColumn == "Salesperson2")
                    {
                        sb.Append("\n and t2.Name like '%" + obj.FilterValue.Trim() + "%' ");
                    }
                    if (obj.FilterColumn == "MonthlyBill")
                    {
                        sb.Append("AND Round (CASE ct.BCycle WHEN 0 THEN ct.BAmt--Monthly; \n ");
                        sb.Append("WHEN 1 THEN ct.BAmt / 2--Bi - Monthly; \n");
                        sb.Append("WHEN 2 THEN ct.BAmt / 3--Quarterly; \n");
                        sb.Append(" WHEN 3 THEN ct.BAmt / 4--3 Times / Year; \n");
                        sb.Append("WHEN 4 THEN ct.BAmt / 6--Semi - Annually; \n");
                        sb.Append("WHEN 5 THEN ct.BAmt / 12--Annually; \n");
                        sb.Append("WHEN 6 THEN 0--'Never'; \n");
                        sb.Append("WHEN 7 THEN ct.BAmt / (12 * 3)--'3 Years'; \n");
                        sb.Append("WHEN 8 THEN ct.BAmt / (12 * 5)--'5 Years'; \n");
                        sb.Append("WHEN 9 THEN ct.BAmt / (12 * 2)--'2 Years'; \n");
                        sb.Append("else 0 END, 2) \n");
                        sb.Append("=" + obj.FilterValue + "\n");
                    }
                    if (obj.FilterColumn == "Freqency")
                    {
                        sb.Append("AND CASE ct.bcycle WHEN 0 THEN 'Monthly' \n");
                        sb.Append("WHEN 1 THEN 'Bi-Monthly' \n");
                        sb.Append("WHEN 2 THEN 'Quarterly' \n");
                        sb.Append("WHEN 3 THEN '3 Times/Year' \n");
                        sb.Append("WHEN 4 THEN 'Semi-Annually' \n");
                        sb.Append("WHEN 5 THEN 'Annually' \n");
                        sb.Append("WHEN 6 THEN 'Never' \n");
                        sb.Append("WHEN 7 THEN '3 Years' \n");
                        sb.Append("WHEN 8 THEN '5 Years' \n");
                        sb.Append("WHEN 9 THEN '2 Years' \n");
                        sb.Append("END like '%");
                        sb.Append(obj.FilterValue.Trim() + "%' \n");
                    }
                    if (obj.FilterColumn == "MonthlyHours")
                    {
                        sb.Append("AND Round (CASE ct.SCycle \n");
                        sb.Append("WHEN 0 THEN ct.Hours --Monthly \n");
                        sb.Append("WHEN 1 THEN ct.Hours / 2 --Bi-Monthly \n");
                        sb.Append("WHEN 2 THEN ct.Hours / 3 --Quarterly \n");
                        sb.Append("WHEN 3 THEN ct.Hours / 6 --Semi-Anually \n");
                        sb.Append("WHEN 4 THEN ct.Hours / 12 --Anually \n");
                        sb.Append("WHEN 5 THEN (ct.Hours * 4.3) --Weekly \n");
                        sb.Append("WHEN 6 THEN (ct.Hours * (2.15))  --Bi-Weekly \n");
                        sb.Append("WHEN 7 THEN ( ct.Hours / ( 2.9898 ) ) --Every 13 Weeks \n");
                        sb.Append("WHEN 10 THEN ct.Hours / 12*2 --Every 2 Years \n");
                        sb.Append("WHEN 8 THEN ct.Hours / 12*3 --Every 3 Years \n");
                        sb.Append("WHEN 9 THEN ct.Hours / 12*5 --Every 5 Years \n");
                        sb.Append("WHEN 11 THEN ct.Hours / 12*7 --Every 7 Years \n");
                        sb.Append("WHEN 13 THEN (ct.Hours * ( CASE ct.SWE WHEN 1 THEN 30 ELSE   21.66 END) ) --Daily \n");
                        sb.Append("WHEN 14 THEN (ct.Hours * (2) ) --Twice a Month \n");
                        sb.Append("WHEN 15 THEN (ct.Hours / (4) ) --3 Times/Year \n");
                        sb.Append("else 0 END, 2 =  \n");
                        sb.Append(obj.FilterValue + "\n");
                    }
                    if (obj.FilterColumn == "TicketFreq")
                    {
                        sb.Append("AND CASE ct.scycle \n");
                        sb.Append("WHEN - 1 THEN 'Never' \n");
                        sb.Append("WHEN 0 THEN 'Monthly' \n");
                        sb.Append("WHEN 1 THEN 'Bi-Monthly' \n");
                        sb.Append("WHEN 2 THEN 'Quarterly' \n");
                        sb.Append("WHEN 3 THEN 'Semi-Annually' \n");
                        sb.Append("WHEN 4 THEN 'Annually' \n");
                        sb.Append("WHEN 5 THEN 'Weekly' \n");
                        sb.Append("WHEN 6 THEN 'Bi-Weekly' \n");
                        sb.Append("WHEN 7 THEN 'Every 13 Weeks' \n");
                        sb.Append("WHEN 10 THEN 'Every 2 Years' \n");
                        sb.Append("WHEN 8 THEN 'Every 3 Years' \n");
                        sb.Append("WHEN 9 THEN 'Every 5 Years' \n");
                        sb.Append("WHEN 11 THEN 'Every 7 Years' \n");
                        sb.Append("WHEN 12 THEN 'On-Demand' \n");
                        sb.Append("WHEN 13 THEN 'Daily' \n");
                        sb.Append("WHEN 14 THEN 'Twice a Month' \n");
                        sb.Append("WHEN 15 THEN '3 Times/Year' \n");
                        sb.Append("END like '%");
                        sb.Append(obj.FilterValue.Trim() + "%' \n");
                    }
                    if (obj.FilterColumn == "Worker")
                    {
                        sb.Append("AND CASE \n");
                        sb.Append("WHEN l.route > 0 THEN \n");
                        sb.Append("(select(select( case  when ro.Name IS NULL  then ''   when tblwork.fdesc is null then  ro.Name    when tblwork.fdesc = ro.Name then ro.Name  else ro.Name + ' - ' + tblwork.fdesc   end)from tblwork where tblwork.id = ro.mech   ) FROM Route ro where ro.ID = l.route) \n");
                        sb.Append("ELSE 'Unassigned' \n");
                        sb.Append("END like '%");
                        sb.Append(obj.FilterValue.Trim() + "%'  \n");
                    }
                    if (obj.FilterColumn == "Status")
                    {
                        sb.Append("AND CASE ct.Status \n");
                        sb.Append("WHEN 0 THEN 'Active' \n");
                        sb.Append("WHEN 1 THEN 'Closed' \n");
                        sb.Append("WHEN 2 THEN 'Hold' \n");
                        sb.Append("WHEN 3 THEN 'Completed' \n");
                        sb.Append("END like '%");
                        sb.Append(obj.FilterValue.Trim() + "%'  \n");
                    }
                }

                sb.Append("ORDER BY j.ID \n");

                sb.Append("SELECT * FROM ( \n");
                sb.Append("SELECT \n");
                sb.Append("r.Name AS Customer  \n");
                sb.Append(",SUM(ISNULL(ElevsC.ElevC,0)) AS SumOfElevs \n");
                sb.Append(",SUM(ISNULL(j.Rev,0)) AS SumOfRev  \n");
                sb.Append("FROM Job j  \n");
                sb.Append("INNER JOIN Contract ct ON ct.Job = j.ID  \n");
                sb.Append("INNER JOIN Loc l ON l.Loc = ct.Loc \n");
                sb.Append("INNER JOIN Owner o ON o.ID = ct.Owner  \n");
                sb.Append("INNER JOIN Rol r on r.ID = o.Rol   \n");
                sb.Append("INNER JOIN (Select ct.Job,COUNT(tb.Elev) ElevC From tblJoinElevJob tb Right Join Contract ct on tb.Job=ct.Job Group By ct.Job ) ElevsC  on ElevsC.Job=ct.Job  \n");
                sb.Append("WHERE 1 = 1  \n");
                sb.Append("AND ct.Status = 0 \n");
                sb.Append("GROUP BY o.ID, r.Name) AS t \n");
                sb.Append("ORDER BY t.SumOfElevs DESC \n");

                sb.Append("SELECT TOP 25 \n");
                sb.Append("	ROW_NUMBER() OVER (ORDER BY t.SumOfElevs DESC) AS Rank, \n");
                sb.Append("	t.CustomerName,  \n");
                sb.Append("	t.SumOfElevs,  \n");
                sb.Append("	t2.Amount AS ContractRevenue,\n");
                sb.Append("	CASE WHEN t.SumOfElevs = 0 THEN 0 ELSE t2.Amount / t.SumOfElevs END AS AverageRev\n");
                sb.Append("FROM ( \n");
                sb.Append("	SELECT \n");
                sb.Append("		o.ID\n");
                sb.Append("		,r.Name AS CustomerName  \n");
                sb.Append("		,SUM(ISNULL(j.Elevs, 0)) AS SumOfElevs \n");
                sb.Append("	FROM Job j  \n");
                sb.Append("		INNER JOIN Contract ct ON ct.Job = j.ID  \n");
                sb.Append("		INNER JOIN Owner o ON o.ID = ct.Owner  \n");
                sb.Append("		INNER JOIN Rol r on r.ID = o.Rol  \n");
                sb.Append("	WHERE ct.Status = 0 AND (j.CType LIKE 'FSMC%' OR j.CType LIKE 'ASMC%' OR j.CType LIKE 'HSMC%') \n");
                sb.Append("	GROUP BY o.ID, r.Name) AS t \n");
                sb.Append("LEFT JOIN( \n");
                sb.Append("	SELECT \n");
                sb.Append("		o.ID, \n");
                sb.Append("		SUM(ji.Amount) AS Amount \n");
                sb.Append("	FROM Job j \n");
                sb.Append("		INNER JOIN Contract ct ON ct.Job = j.ID \n");
                sb.Append("		INNER JOIN Owner o ON o.ID = ct.Owner \n");
                sb.Append("		LEFT JOIN JobI ji ON ji.Job = j.ID AND ISNULL(ji.Type, 0) = 0 \n");
                sb.Append("	WHERE ct.Status = 0  and ji.fDate <= '" + objPropContracts.EndDate + "' \n");
                sb.Append("	GROUP BY o.ID) AS t2 ON t2.ID = t.ID \n");
                sb.Append("ORDER BY t.SumOfElevs DESC \n");

                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetProjectSummary(Customer objPropCustomer, List<RetainFilter> filters, string departments, int includeClose = 0, bool isExport = false, bool isDBTotalService = false)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT \n");
                sb.Append("	j.ID AS [Project#] \n");
                sb.Append("	,jt.Type AS LOB \n");
                sb.Append("	,l.Tag AS ProjectName \n");
                sb.Append("	,j.Elevs \n");
                sb.Append("	,CASE j.Status  \n");
                sb.Append("		WHEN 0 THEN 'Open' \n");
                sb.Append("		WHEN 1 THEN 'Closed' \n");
                sb.Append("		WHEN 2 THEN 'Hold' \n");
                sb.Append("		WHEN 3 THEN 'Completed' \n");
                sb.Append("		END AS ProjectStatus \n");
                sb.Append("	,j.Rev AS ActualRevenue \n");
                sb.Append("	,j.OtherExp AS ActualOtherExp \n");
                sb.Append("	,j.Mat AS ActualMaterial \n");
                sb.Append("	,j.BOther AS BudgetOtherExp \n");
                sb.Append("	,j.Labor AS ActualLabor \n");
                sb.Append("	,j.Cost AS ActualTtlCost \n");
                sb.Append("	,j.Profit AS ActualProfit \n");
                sb.Append("	,j.Ratio AS ActualProfitRatio \n");
                sb.Append("	,j.Hour AS ActualHours \n");
                sb.Append("	,j.BRev AS BudgetRevenue \n");
                sb.Append("	,j.BMat AS BudgetMaterial \n");
                sb.Append("	,j.BLabor AS BudgetLabor \n");
                sb.Append("	,j.BCost AS BudgetTtlCost \n");
                sb.Append("	,(CASE j.BCost WHEN 0 THEN 0 ELSE j.Cost / j.BCost END) AS CostPercentComplete \n");
                sb.Append("	,j.BProfit AS BudgetProfit \n");
                sb.Append("	,j.BRatio AS BudgetProfitRatio \n");
                sb.Append("	,j.BHour AS BudgetHours \n");
                sb.Append("	,j.fDate AS ContractDate \n");
                sb.Append("	,(ISNULL(j.Comm,0) + ISNULL(j.ReceivePO, 0)) AS CommittedCost \n");
                sb.Append("	,j.NT \n");
                sb.Append("	,j.Amount \n");
                sb.Append("	,j.GandA \n");
                sb.Append("	,j.OHLabor \n");
                sb.Append("	,j.LastOH \n");
                sb.Append("	,j.etc \n");
                sb.Append("	,j.ETCModifier \n");
                sb.Append("	,j.CType \n");
                sb.Append("	,j.CloseDate \n");
                sb.Append("	,j.Custom10 \n");
                sb.Append("	,ISNULL((SELECT SUM(it.Amount)  \n");
                sb.Append("		FROM InvoiceI it  \n");
                sb.Append("		INNER JOIN Invoice i ON i.Ref = it.Ref \n");
                sb.Append("		WHERE it.Job = j.ID AND i.Status = 1), 0) AS PaidInvoices \n");
                sb.Append("	,(SELECT MAX(i.fDate)  \n");
                sb.Append("		FROM InvoiceI it  \n");
                sb.Append("		INNER JOIN Invoice i ON i.Ref = it.Ref \n");
                sb.Append("		WHERE it.Job = j.ID) AS LastInvoiceDate \n");
                sb.Append("	,(SELECT MAX(p.fDate)  \n");
                sb.Append("		FROM JobI ji  \n");
                sb.Append("		LEFT OUTER JOIN Trans t ON ji.TransID = t.ID \n");
                sb.Append("		LEFT OUTER JOIN PJ p ON t.Batch = p.Batch \n");
                sb.Append("		WHERE ji.Job = j.ID AND ji.Type in (1,2) AND t.Type in (41)) AS MaxDtAP \n");
                sb.Append("	,(SELECT MAX(ji.fDate) FROM JobI ji WHERE ji.Job = j.ID AND ji.Labor = 1) AS MaxDtLabor \n");
                sb.Append("	,(SELECT COUNT(DISTINCT p.PO)  \n");
                sb.Append("		FROM POItem i  \n");
                sb.Append("		INNER JOIN PO p ON p.PO = i.PO \n");
                sb.Append("		WHERE i.Job = j.ID AND p.Status = 0) AS POCount \n");
                sb.Append("	,l.City AS Location \n");
              
                   

                //if (!isExport)
                //{
                    sb.Append("	,l.Loc \n");
                    sb.Append("	,j.Remarks \n");
                    sb.Append("	,CASE ISNULL(j.BHour, 0) WHEN 0 THEN 0 ELSE j.BLabor/j.BHour END AS BudgetLaborRate \n");
                    sb.Append("	,CASE ISNULL(j.Hour, 0) WHEN 0 THEN 0 ELSE j.Labor/j.Hour END AS ActualLaborRate \n");
                    sb.Append("	,CASE ISNULL(j.BRev, 0) WHEN 0 THEN 0 ELSE j.BProfit/j.BRev END AS BudgetGrossMargin \n");
                    sb.Append("	,CASE ISNULL(j.Rev, 0) WHEN 0 THEN 0 ELSE j.Profit/j.Rev END AS ActualGrossMargin \n");

                    sb.Append("	,ISNULL(j.BLabor, 0) - ISNULL(j.Labor, 0) AS VarianceLabor \n");
                    sb.Append("	,ISNULL(j.BHour, 0) - ISNULL(j.Hour, 0) AS VarianceHours  \n");
                    sb.Append("	,ISNULL(j.BMat, 0) - ISNULL(j.Mat, 0) AS VarianceMaterial  \n");
                    sb.Append("	,ISNULL(j.BOther,0) - ISNULL(j.OtherExp,0) AS VarianceOtherExp\n");
                    sb.Append("	,ISNULL(j.BCost, 0) - ISNULL(j.Cost, 0) AS VarianceTtlCost  \n");
                    sb.Append("	,ISNULL(j.BRev, 0) - ISNULL(j.Rev, 0) AS VarianceRevenue  \n");
                    sb.Append("	,ISNULL(j.BProfit, 0) - ISNULL(j.Profit, 0) AS VarianceProfit  \n");
               // }

                sb.Append("FROM Job j \n");
                sb.Append("	INNER JOIN Loc l ON l.Loc = j.Loc \n");
                sb.Append("	INNER JOIN Owner o ON o.ID = j.Owner \n");
                sb.Append("	INNER JOIN Rol ro ON ro.ID = o.Rol \n");
                sb.Append("	INNER JOIN JobType jt ON jt.ID = j.Type \n");

                if (!string.IsNullOrEmpty(objPropCustomer.SearchBy) && !string.IsNullOrEmpty(objPropCustomer.SearchValue) && objPropCustomer.SearchBy == "t.title")
                {
                    sb.Append("	LEFT JOIN Team t WITH(NOLOCK) ON t.JobID = j.ID \n");
                }

                // Join when there is a filter
                if (filters.FindIndex(x => x.FilterColumn == "TemplateDesc") > -1)
                {
                    sb.Append("	LEFT JOIN JobT tem WITH(NOLOCK) ON tem.Id = j.Template \n");
                }

                if (filters.FindIndex(x => x.FilterColumn == "Salesperson") > -1)
                {
                    sb.Append("	LEFT JOIN Terr ter WITH(NOLOCK) ON ter.Id = l.Terr \n");
                }

                if (filters.FindIndex(x => x.FilterColumn == "Route") > -1)
                {
                    sb.Append("	LEFT JOIN Route rt WITH(NOLOCK) ON rt.Id = l.Route \n");
                    sb.Append("	LEFT JOIN tblWork w WITH(NOLOCK) ON w.Id = rt.Mech \n");
                }

                if (filters.FindIndex(x => x.FilterColumn == "BuildingType") > -1)
                {
                    sb.Append("	LEFT JOIN BusinessType bt WITH(NOLOCK) ON bt.ID = l.BusinessType \n");
                }

                if (filters.FindIndex(x => x.FilterColumn == "ProjectManagerUserName") > -1)
                {
                    sb.Append("	LEFT JOIN Emp pm WITH(NOLOCK) ON pm.ID = j.ProjectManagerUserID \n");
                }

                if (filters.FindIndex(x => x.FilterColumn == "SupervisorUserName") > -1)
                {
                    sb.Append("	LEFT JOIN Emp su WITH(NOLOCK) ON su.ID = j.SupervisorUserID \n");
                }

                if (filters.FindIndex(x => x.FilterColumn == "NotBilledYet" || x.FilterColumn == "NRev" || x.FilterColumn == "NProfit" || x.FilterColumn == "NRatio") > -1)
                {
                    sb.Append("	LEFT JOIN ( \n");
                    sb.Append("	    SELECT SUM(ISNULL(JobI.Amount,0)) Amt, JobI.Job FROM JobI WHERE ISNULL(JobI.Type, 0) <> 1 \n");

                    if (objPropCustomer.Range == 2 || objPropCustomer.Range == 5)
                    {
                        sb.Append("	    AND JobI.fDate >= '" + objPropCustomer.StartDate + "' AND JobI.fDate <= '" + objPropCustomer.EndDate + "' \n");
                    }

                    sb.Append("	    GROUP BY JobI.Job \n");
                    sb.Append("	) jobCostNRev ON jobCostNRev.Job = j.ID \n");
                }

                if (filters.FindIndex(x => x.FilterColumn == "NLabor") > -1)
                {
                    sb.Append("	LEFT JOIN ( \n");
                    sb.Append("	    SELECT SUM(ISNULL(JobI.Amount,0)) Amt, JobI.Job FROM JobI \n");
                    sb.Append("	    INNER JOIN JobTItem ON JobTItem.Line = JobI.Phase AND JobTItem.Job = JobI.Job \n");
                    sb.Append("	    INNER JOIN BOM ON BOM.JobTItemID = JobTItem.ID \n");
                    sb.Append("	    INNER JOIN BOMT ON BOMT.ID = BOM.Type \n");
                    sb.Append("	    WHERE ISNULL(JobI.Type, 0) <> 0 \n");
                    sb.Append("	    AND ISNULL(JobI.Labor,0) = 1 AND JobI.fDesc NOT IN ('Mileage on Ticket','Expenses on Ticket') \n");

                    if (objPropCustomer.Range == 2 || objPropCustomer.Range == 5)
                    {
                        sb.Append("	    AND JobI.fDate >= '" + objPropCustomer.StartDate + "' AND JobI.fDate <= '" + objPropCustomer.EndDate + "' \n");
                    }

                    sb.Append("	    GROUP BY JobI.Job \n");
                    sb.Append("	) jobCostLabor ON jobCostLabor.Job = j.ID \n");
                }

                if (filters.FindIndex(x => x.FilterColumn == "NMat") > -1)
                {
                    sb.Append("	LEFT JOIN ( \n");
                    sb.Append("	    SELECT SUM(ISNULL(JobI.Amount,0)) Amt, JobI.Job FROM JobI \n");
                    sb.Append("	    INNER JOIN JobTItem ON JobTItem.Line = JobI.Phase AND JobTItem.Job = JobI.Job \n");
                    sb.Append("	    INNER JOIN BOM ON BOM.JobTItemID = JobTItem.ID \n");
                    sb.Append("	    INNER JOIN BOMT ON BOMT.ID = BOM.Type AND (BOMT.Type = 'Materials' OR BOMT.Type = 'Inventory') \n");
                    sb.Append("	    WHERE ISNULL(JobI.Type, 0) <> 0 \n");
                    sb.Append("	    AND JobI.fDesc NOT IN ('Mileage on Ticket','Expenses on Ticket') \n");
                    sb.Append("	    AND (JobI.TransID > 0 OR ISNULL(JobI.Labor, 0) = 0)  \n");

                    if (objPropCustomer.Range == 2 || objPropCustomer.Range == 5)
                    {
                        sb.Append("	    AND JobI.fDate >= '" + objPropCustomer.StartDate + "' AND JobI.fDate <= '" + objPropCustomer.EndDate + "' \n");
                    }

                    sb.Append("	    GROUP BY JobI.Job \n");
                    sb.Append("	) jobCostMaterial ON jobCostMaterial.Job = j.ID \n");
                }

                if (filters.FindIndex(x => x.FilterColumn == "NOMat") > -1)
                {
                    sb.Append("	LEFT JOIN ( \n");
                    sb.Append("	    SELECT SUM(ISNULL(JobI.Amount,0)) Amt, JobI.Job FROM JobI \n");
                    sb.Append("	    INNER JOIN JobTItem ON JobTItem.Line = JobI.Phase AND JobTItem.Job = JobI.Job \n");
                    sb.Append("	    INNER JOIN BOM ON BOM.JobTItemID = JobTItem.ID \n");
                    sb.Append("	    INNER JOIN BOMT ON BOMT.ID = BOM.Type \n");
                    sb.Append("	    WHERE ISNULL(JobI.Type, 0) <> 0 \n");
                    sb.Append("	    AND (((JobI.TransID > 0 OR ISNULL(JobI.Labor, 0) = 0) AND (BOMT.Type <> 'Materials' AND BOMT.Type <> 'Labor' AND BOMT.Type <> 'Inventory') \n");
                    sb.Append("	    AND JobI.fDesc NOT IN ('Mileage on Ticket','Expenses on Ticket')) OR JobI.fDesc IN ('Mileage on Ticket','Expenses on Ticket'))  \n");

                    if (objPropCustomer.Range == 2 || objPropCustomer.Range == 5)
                    {
                        sb.Append("	    AND JobI.fDate >= '" + objPropCustomer.StartDate + "' AND JobI.fDate <= '" + objPropCustomer.EndDate + "' \n");
                    }

                    sb.Append("	    GROUP BY JobI.Job \n");
                    sb.Append("	) jobCostOther ON jobCostOther.Job = j.ID \n");
                }

                if (filters.FindIndex(x => x.FilterColumn == "NCost" || x.FilterColumn == "NProfit" || x.FilterColumn == "NRatio") > -1)
                {
                    sb.Append("	LEFT JOIN ( \n");
                    sb.Append("	    SELECT SUM(ISNULL(JobI.Amount,0)) Amt, JobI.Job FROM JobI WHERE ISNULL(JobI.Type, 0) = 1 \n");

                    if (objPropCustomer.Range == 2 || objPropCustomer.Range == 5)
                    {
                        sb.Append("	    AND JobI.fDate >= '" + objPropCustomer.StartDate + "' AND JobI.fDate <= '" + objPropCustomer.EndDate + "' \n");
                    }

                    sb.Append("	    GROUP BY JobI.Job \n");
                    sb.Append("	) jobCostNCost ON jobCostNCost.Job = j.ID \n");
                }

                if (filters.FindIndex(x => x.FilterColumn == "NHour") > -1)
                {
                    sb.Append("	LEFT JOIN ( \n");
                    sb.Append("	    SELECT SUM(ISNULL(t.Reg,0) + ISNULL(t.RegTrav,0) + ISNULL(t.OT, 0) + ISNULL(t.OTTrav, 0) + ISNULL(t.NT, 0) + ISNULL(t.NTTrav, 0) + ISNULL(t.DT, 0) + ISNULL(t.DTTrav, 0) + ISNULL(t.TT, 0)) AS ActualHr, \n");
                    sb.Append("	    t.Job \n");
                    sb.Append("	    FROM TicketD t \n");

                    if (objPropCustomer.Range == 2 || objPropCustomer.Range == 5)
                    {
                        sb.Append("	    AND t.EDate >= '" + objPropCustomer.StartDate + "' AND t.EDate <= '" + objPropCustomer.EndDate + "' \n");
                    }

                    sb.Append("	    GROUP BY t.Job \n");
                    sb.Append("	) jobCostNHour ON jobCostNHour.Job = j.ID \n");
                }

                if (filters.FindIndex(x => x.FilterColumn == "NComm") > -1)
                {
                    sb.Append("	LEFT JOIN ( \n");
                    sb.Append("	    SELECT SUM(ISNULL(p.Balance,0)) AS Amt, p.Job \n");
                    sb.Append("	    FROM POItem p \n");
                    sb.Append("	    INNER JOIN PO ON p.PO = PO.PO \n");
                    sb.Append("	    WHERE po.Status in (0,3,4) \n");

                    if (objPropCustomer.Range == 2 || objPropCustomer.Range == 5)
                    {
                        sb.Append("	    AND po.fDate >= '" + objPropCustomer.StartDate + "' AND po.fDate <= '" + objPropCustomer.EndDate + "' \n");
                    }

                    sb.Append("	    GROUP BY p.Job \n");
                    sb.Append("	) jobCostComm ON jobCostComm.Job = j.ID \n");
                }

                if (filters.FindIndex(x => x.FilterColumn == "ReceivePO" || x.FilterColumn == "NComm") > -1)
                {
                    sb.Append("	LEFT JOIN ( \n");
                    sb.Append("	    SELECT SUM(ISNULL(rp.Amount,0)) AS Amt, p.Job \n");
                    sb.Append("	    FROM RPOItem rp \n");
                    sb.Append("	    INNER JOIN ReceivePO r ON r.ID = rp.ReceivePO \n");
                    sb.Append("	    LEFT JOIN POItem p ON r.PO = p.PO AND rp.POLine = p.Line \n");
                    sb.Append("	    WHERE ISNULL(r.Status,0) = 0 \n");

                    if (objPropCustomer.Range == 2 || objPropCustomer.Range == 5)
                    {
                        sb.Append("	    AND r.fDate >= '" + objPropCustomer.StartDate + "' AND r.fDate <= '" + objPropCustomer.EndDate + "' \n");
                    }

                    sb.Append("	    GROUP BY p.Job \n");
                    sb.Append("	) jobCostRevPO ON jobCostRevPO.Job = j.ID \n");
                }

                // Condition query
                if (objPropCustomer.Range == 3)
                {
                    sb.Append(" WHERE j.Status = 1  \n");
                    sb.Append("AND CAST(j.CloseDate as date) > = '" + objPropCustomer.StartDate + "' AND CAST(j.CloseDate as date) <= '" + objPropCustomer.EndDate + "' \n");
                    //sb.Append("AND CAST(j.CloseDate as date) > = '" + objPropCustomer.StartDate + "' AND fDate <= '" + objPropCustomer.EndDate + "'  \n");
                }

                else if (objPropCustomer.Range == 5)
                {
                    sb.Append(" WHERE j.Status <> 1 AND j.ID IN (SELECT DISTINCT Job FROM JobI WITH(NOLOCK) WHERE fDate >= '" + objPropCustomer.StartDate + "' AND fDate <= '" + objPropCustomer.EndDate + "') \n");
                }
                else if (objPropCustomer.Range == 1 || objPropCustomer.Range == 2)
                {
                    sb.Append(" WHERE j.Status <> 1\n");
                }

                else if (objPropCustomer.Range == 4)
                {
                    sb.Append(" WHERE cast( j.fDate as date)  > ='" + objPropCustomer.StartDate + "' AND cast( j.fDate as date) <= '" + objPropCustomer.EndDate + "'\n");
                    sb.Append("AND j.Status <>1");
                }

                else
                {
                    sb.Append("WHERE (" + includeClose + " = 1 OR j.Status = 0) \n");
                }

                if (!string.IsNullOrEmpty(departments))
                {
                    sb.Append("	AND j.Type IN(" + departments + ") \n");
                }

                // Search value
                if (!string.IsNullOrEmpty(objPropCustomer.SearchBy) && !string.IsNullOrEmpty(objPropCustomer.SearchValue))
                {
                    if (objPropCustomer.SearchBy == "j.id")
                    {
                        sb.Append(" AND j.ID = " + objPropCustomer.SearchValue + " \n");
                    }
                    else if (objPropCustomer.SearchBy == "j.fdate")
                    {
                        sb.Append(" AND j.fDate = " + objPropCustomer.SearchValue + " \n");
                    }
                    else if (objPropCustomer.SearchBy == "l.tag")
                    {
                        sb.Append(" AND l.Tag LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                    }
                    else if (objPropCustomer.SearchBy == "l.City")
                    {
                        sb.Append(" AND l.City LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                    }
                    else if (objPropCustomer.SearchBy == "l.State")
                    {
                        sb.Append(" AND l.State LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                    }
                    else if (objPropCustomer.SearchBy == "j.Status")
                    {
                        sb.Append(" AND j.Status = " + objPropCustomer.SearchValue + " \n");
                    }
                    else if (objPropCustomer.SearchBy == "j.fdesc")
                    {
                        sb.Append(" AND j.fDesc LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                    }
                    else if (objPropCustomer.SearchBy == "r.Name")
                    {
                        sb.Append(" AND ro.Name LIKE '%" + objPropCustomer.SearchValue + "%' \n");
                    }
                    else if (objPropCustomer.SearchBy == "j.PWIP")
                    {
                        sb.Append(" AND j.PWIP = " + objPropCustomer.SearchValue + " \n");
                    }
                    else if (objPropCustomer.SearchBy == "j.Certified")
                    {
                        sb.Append(" AND j.Certified = " + objPropCustomer.SearchValue + " \n");
                    }
                    else if (objPropCustomer.SearchBy == "t.title")
                    {
                        sb.Append(" AND t.Title = '" + objPropCustomer.SearchValue + "' \n");
                        sb.Append(" AND t.MomUserID LIKE '%" + objPropCustomer.Username + "%' \n");
                    }
                    else
                    {
                        // For Custom1 -> Custom20
                        sb.Append(" AND " + objPropCustomer.SearchBy + " = " + objPropCustomer.SearchValue + "\n");
                    }
                }

                // Filter on grid
                foreach (var filter in filters)
                {
                    if (filter.FilterColumn == "DepartmentList")
                    {
                        sb.Append(" AND jt.Type IN(" + filter.FilterValue + ") \n");
                    }
                    if (filter.FilterColumn == "Customer")
                    {
                        sb.Append(" AND ro.Name LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "Tag")
                    {
                        sb.Append(" AND l.Tag LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "ID")
                    {
                        sb.Append(" AND j.ID = " + filter.FilterValue + " \n");
                    }
                    if (filter.FilterColumn == "fdesc")
                    {
                        sb.Append(" AND j.fDesc LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "Status")
                    {
                        sb.Append(" AND (CASE j.Status WHEN 0 THEN 'Open' WHEN 1 THEN 'Closed' WHEN 2 THEN 'Hold' WHEN 3 THEN 'Completed' END) LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "CType")
                    {
                        sb.Append(" AND j.CType LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "TemplateDesc")
                    {
                        sb.Append(" AND tem.fDesc LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "Type")
                    {
                        sb.Append(" AND jt.Type LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "Salesperson")
                    {
                        sb.Append(" AND ter.Name LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "Route")
                    {
                        sb.Append(" AND (CASE WHEN w.fDesc = rt.Name THEN rt.Name ELSE rt.Name  + '-' + w.fDesc END) LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "ContractPrice")
                    {
                        sb.Append(" AND ISNULL(j.BRev, 0) = " + decimal.Parse(filter.FilterValue) + " \n");
                    }
                    if (filter.FilterColumn == "NotBilledYet")
                    {
                        sb.Append(" AND (ISNULL(j.BRev, 0) - ISNULL(jobCostNRev.Amt, 0)) = " + decimal.Parse(filter.FilterValue) + " \n");
                    }
                    if (filter.FilterColumn == "NRev")
                    {
                        sb.Append(" AND ISNULL(jobCostNRev.Amt, 0) = " + decimal.Parse(filter.FilterValue) + " \n");
                    }
                    if (filter.FilterColumn == "NHour")
                    {
                        sb.Append(" AND ISNULL(jobCostNHour.ActualHr, 0) = " + decimal.Parse(filter.FilterValue) + " \n");
                    }
                    if (filter.FilterColumn == "NLabor")
                    {
                        sb.Append(" AND ISNULL(jobCostLabor.Amt, 0) = " + decimal.Parse(filter.FilterValue) + " \n");
                    }
                    if (filter.FilterColumn == "NMat")
                    {
                        sb.Append(" AND ISNULL(jobCostMaterial.Amt, 0) = " + decimal.Parse(filter.FilterValue) + " \n");
                    }
                    if (filter.FilterColumn == "NOMat")
                    {
                        sb.Append(" AND ISNULL(jobCostOther.Amt, 0) = " + decimal.Parse(filter.FilterValue) + " \n");
                    }
                    if (filter.FilterColumn == "NCost")
                    {
                        sb.Append(" AND ISNULL(jobCostNCost.Amt, 0) = " + decimal.Parse(filter.FilterValue) + " \n");
                    }
                    if (filter.FilterColumn == "TotalBudgetedExpense")
                    {
                        sb.Append(" AND (ISNULL(j.BMat,0) + ISNULL(j.BLabor,0) + ISNULL(j.BOther,0)) = " + filter.FilterValue + " \n");
                    }
                    if (filter.FilterColumn == "NComm")
                    {
                        sb.Append(" AND (ISNULL(jobCostComm.Amt, 0) + ISNULL(jobCostRevPO.Amt, 0)) = " + decimal.Parse(filter.FilterValue) + " \n");
                    }
                    if (filter.FilterColumn == "ReceivePO")
                    {
                        sb.Append(" AND ISNULL(jobCostRevPO.Amt, 0) = " + decimal.Parse(filter.FilterValue) + " \n");
                    }
                    if (filter.FilterColumn == "NProfit")
                    {
                        sb.Append(" AND (ISNULL(jobCostNRev.Amt, 0) - ISNULL(jobCostNCost.Amt, 0)) = " + decimal.Parse(filter.FilterValue) + " \n");
                    }
                    if (filter.FilterColumn == "NRatio")
                    {
                        sb.Append(" AND (CASE ISNULL(jobCostNRev.Amt, 0) WHEN 0 THEN 0 ELSE ROUND((ISNULL(jobCostNRev.Amt, 0) - ISNULL(jobCostNCost.Amt, 0)) / jobCostNRev.Amt  * 100, 2) END) = " + decimal.Parse(filter.FilterValue) + " \n");
                    }
                    if (filter.FilterColumn == "ProjectManagerUserName")
                    {
                        sb.Append(" AND pm.CallSign LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "SupervisorUserName")
                    {
                        sb.Append(" AND su.CallSign LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "LocationType")
                    {
                        sb.Append(" AND l.Type LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "BuildingType")
                    {
                        sb.Append(" AND bt.Description  LIKE '%" + filter.FilterValue + "%' \n");
                    }
                }

                // Get AR Aging
               // if (!isExport)
               // {
                    if (isDBTotalService)
                    {
                        sb.Append("EXEC spGetARAgingByLocation '" + DateTime.Now.ToShortDateString() + "', 0 \n");
                    }
                    else
                    {
                        sb.Append("EXEC spGetARAgingByLocationNoneTS '" + DateTime.Now.ToShortDateString() + "', 0 \n");
                    }
               // }
                //if (isExport)
                //{
                //    sb.Append("EXEC spGetARAgingByLocationNoneTS '" + DateTime.Now.ToShortDateString() + "', 0 \n");
                //}

                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, sb.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDashboardReportData(Chart objChart, List<DashboardColumnRequest> objColumns)
        {
            try
            {
                string selectData = string.Empty;
                string condition = string.Empty;
                foreach (var item in objColumns)
                {
                    if (!string.IsNullOrEmpty(selectData))
                    {
                        selectData += ",";
                    }

                    if (item.Type == "Difference")
                    {
                        selectData += string.Format("SUM(ISNULL(table{0}.[{0}], 0) - ISNULL(table{1}.[{1}], 0)) AS Column{2}", item.Column1, item.Column2, item.Index);
                    }
                    else if (item.Type == "Variance")
                    {
                        selectData += string.Format("(CASE WHEN SUM(ISNULL(table{1}.[{1}], 0)) = 0 THEN 0 ELSE SUM(ISNULL(table{0}.[{0}], 0) - ISNULL(table{1}.[{1}], 0)) / SUM(table{1}.[{1}]) END) AS Column{2}", item.Column1, item.Column2, item.Index);
                    }
                    else
                    {
                        selectData += string.Format("SUM(ISNULL(table{0}.[{0}], 0)) AS Column{0}", item.Index);

                        if (!string.IsNullOrEmpty(condition))
                        {
                            condition += " OR ";
                        }

                        condition += string.Format(" table{0}.[{0}] <> 0", item.Index);
                    }
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT  \n");
                sb.Append("	c.Sub AS fDesc, \n");
                sb.Append("	c.Type, \n");
                sb.Append("	NULL AS SubDepartment, \n");
                sb.Append("	0 AS Department, \n");
                sb.Append("	'Total' AS CentralName \n");

                if (!string.IsNullOrEmpty(selectData))
                {
                    sb.Append("," + selectData + " \n");
                }
                sb.Append(" FROM Chart c \n");
                sb.Append(" LEFT JOIN Central ct ON c.Department = ct.ID \n");

                foreach (var item in objColumns)
                {
                    if (item.Type != "Difference" && item.Type != "Variance")
                    {
                        var tableName = string.Format("table{0}", item.Index);

                        sb.Append(" LEFT JOIN ( \n");
                        sb.Append("SELECT \n");
                        sb.Append("	c1.ID AS Acct, \n");
                        sb.Append("	c1.Acct AS AcctNo, \n");
                        sb.Append("	ISNULL(SUM(t.Amount),0) * -1 AS [" + item.Index + "] \n");
                        sb.Append(" FROM Chart c1 LEFT OUTER JOIN Trans t ON t.Acct = c1.ID \n");
                        sb.Append(" WHERE c1.Type IN (3, 4, 5, 8, 9) \n");
                        sb.Append("	    AND t.fDate >= '" + item.StartDate.Value.Date + "' \n");
                        sb.Append("	    AND t.fDate <= '" + item.EndDate.Value.Date + "' \n");
                        sb.Append(" GROUP BY c1.ID, c1.Acct , c1.Type \n");

                        sb.Append(" ) AS " + tableName + " ON c.ID = " + tableName + ".Acct \n");
                    }
                }

                sb.Append(" WHERE c.Type IN (3, 4, 5) \n");

                // Get Statement with Center
                if (!string.IsNullOrEmpty(objChart.Departments))
                {
                    var centers = objChart.Departments.Split(',');
                    if (Array.FindAll(centers, s => s.Equals("0")).Length > 0)
                    {
                        sb.Append("    AND (c.Department IN (" + objChart.Departments + ") OR c.Department IS NULL ) \n");
                    }
                    else
                    {
                        sb.Append("    AND c.Department IN (" + objChart.Departments + ") \n");
                    }
                }

                if (!string.IsNullOrEmpty(condition))
                {
                    sb.Append("     AND (" + condition + ") \n");
                }
                sb.Append(" GROUP BY c.Sub, c.Type \n");

                sb.Append("UNION \n");

                sb.Append("SELECT \n");
                sb.Append("	c.Sub AS fDesc, \n");
                sb.Append("	c.Type, \n");
                sb.Append("	c.Custom1 AS SubDepartment, \n");
                sb.Append("	ISNULL(c.Department, -1) AS Department, \n");
                sb.Append("	ISNULL(ct.CentralName,'Undefined') AS CentralName \n");

                if (!string.IsNullOrEmpty(selectData))
                {
                    sb.Append("," + selectData + " \n");
                }
                sb.Append(" FROM Chart c \n");
                sb.Append(" LEFT JOIN Central ct ON c.Department = ct.ID \n");

                foreach (var item in objColumns)
                {
                    if (item.Type != "Difference" && item.Type != "Variance")
                    {
                        var tableName = string.Format("table{0}", item.Index);

                        sb.Append("LEFT JOIN ( \n");
                        sb.Append("SELECT \n");
                        sb.Append("c1.ID AS Acct, \n");
                        sb.Append("c1.Acct AS AcctNo, \n");
                        sb.Append("ISNULL(SUM(t.Amount),0) * -1 AS [" + item.Index + "] \n");
                        sb.Append("FROM Chart c1 LEFT OUTER JOIN Trans t ON t.Acct = c1.ID \n");
                        sb.Append("WHERE c1.Type IN (3, 4, 5, 8, 9) \n");
                        sb.Append("AND t.fDate >= '" + item.StartDate.Value.Date + "' \n");
                        sb.Append("AND t.fDate <= '" + item.EndDate.Value.Date + "' \n");
                        sb.Append("GROUP BY c1.ID, c1.Acct , c1.Type  \n");

                        sb.Append(" ) AS " + tableName + " ON c.ID = " + tableName + ".Acct \n");
                    }
                }

                sb.Append(" WHERE c.Type IN (3, 4, 5) \n");

                // Get Statement with Center
                if (!string.IsNullOrEmpty(objChart.Departments))
                {
                    var centers = objChart.Departments.Split(',');
                    if (Array.FindAll(centers, s => s.Equals("0")).Length > 0)
                    {
                        sb.Append("    AND (c.Department IN (" + objChart.Departments + ") OR c.Department IS NULL ) \n");
                    }
                    else
                    {
                        sb.Append("    AND c.Department IN (" + objChart.Departments + ") \n");
                    }
                }

                if (!string.IsNullOrEmpty(condition))
                {
                    sb.Append("     AND (" + condition + ") \n");
                }
                sb.Append(" GROUP BY c.Sub, c.Type, c.Department, c.Custom1, ct.CentralName \n");

                // Invoices Billed By Week
                sb.Append("SELECT \n");
                sb.Append("	WeekEnd, \n");
                sb.Append("	SUM(Total) AS Total, \n");
                sb.Append("	SUM(ICount) AS ICount \n");
                sb.Append("FROM ( \n");
                sb.Append("	SELECT \n");
                sb.Append("		DATEADD(day, 6, DATEADD(week, DATEDIFF(week, -1, fDate), -1)) AS WeekEnd, \n");
                sb.Append("		SUM(Amount) AS Total, \n");
                sb.Append("		COUNT(*) AS ICount \n");
                sb.Append("	FROM Invoice \n");
                sb.Append("	WHERE fDate >= '" + objChart.StartDate + "' AND fDate <= '" + objChart.EndDate + "' \n");
                sb.Append("	GROUP BY DATEPART(week, fDate), DATEADD(week, DATEDIFF(week, -1, fDate), -1) \n");
                sb.Append(") AS t \n");
                sb.Append("GROUP BY WeekEnd \n");
                sb.Append("ORDER BY WeekEnd \n");

                return SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetServiceSalesCheckUpReport(Contracts objPropContracts, List<RetainFilter> filters, bool includeClose, bool isPassedInspection = false)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT  \n");
                sb.Append("	j.ID \n");
                sb.Append("	,ct.Job \n");
                sb.Append("	,r.Name AS CustomerName \n");
                sb.Append("	,r.Address AS CustomerAddress \n");
                sb.Append("	,r.City AS CustomerCity \n");
                sb.Append("	,r.State AS CustomerState \n");
                sb.Append("	,r.Zip AS CustomerZip \n");
                sb.Append("	,r.Contact AS HomeOwnerMainContact \n");
                sb.Append("	,r.Cellular AS HomeOwnerCellular \n");
                sb.Append("	,r.EMail AS HomeOwnerEmail \n");
                sb.Append("	,r.Phone AS HomeOwnerPhone \n");
                sb.Append("	,l.ID AS LocID \n");
                sb.Append("	,l.Tag AS LocationName \n");
                sb.Append(" ,l.Address \n");
                sb.Append("	,l.City \n");
                sb.Append("	,l.Zip \n");
                sb.Append("	,l.State \n");
                sb.Append("	,CASE l.Status WHEN 0 THEN 'Active' ELSE 'Inactive' END AS LocationStatus \n");
                sb.Append("	,rl.Contact AS LocationContact \n");
                sb.Append("	,rl.Phone AS LocationPhone \n");
                sb.Append("	,rl.Email AS LocationEmail \n");
                sb.Append("	,cj.Value AS PassedInspection \n");
                sb.Append("	,ct.Expiration \n");
                sb.Append("	,ct.ExpirationDate \n");
                sb.Append("	,j.CType \n");
                sb.Append("	,j.fDesc \n");
                sb.Append("	,CASE WHEN ct.Job IS NOT NULL THEN ISNULL(ct.LastRenew,j.fDate) ELSE NULL END AS LastRenew \n");
                sb.Append("	,j.BRev \n");
                sb.Append("	,ct.BAmt \n");
                sb.Append("	,CASE WHEN ct.Status IS NULL THEN '' WHEN ct.Status = 0 THEN 'Active' ELSE 'Inactive' END AS Status \n");
                sb.Append("FROM Job j \n");
                sb.Append("	INNER JOIN Loc l ON l.Loc = j.Loc \n");
                sb.Append("	INNER JOIN Rol rl on rl.ID = l.Rol  \n");
                sb.Append("	INNER JOIN Owner o ON o.ID = j.Owner \n");
                sb.Append("	INNER JOIN Rol r on r.ID = o.Rol  \n");
                sb.Append("	LEFT JOIN Contract ct ON ct.Job = j.ID \n");
                sb.Append("	LEFT JOIN Terr t ON t.ID = l.Terr  \n");
                sb.Append("	LEFT JOIN Terr t2 ON t2.ID = t.ID \n");
                sb.Append("	LEFT JOIN tblJoinElevJob el ON el.Job = j.ID \n");
                sb.Append("	LEFT JOIN Elev e ON e.ID = el.Elev \n");
                sb.Append("	LEFT JOIN tblCustomJob cj ON cj.JobID = j.ID \n");
                sb.Append("	LEFT JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID \n");

                if (objPropContracts.EN == 1)
                {
                    sb.Append("	LEFT JOIN JOIN Branch B ON r.EN = B.ID \n");
                    sb.Append("	LEFT JOIN JOIN tblUserCO UC ON UC.CompanyID = B.ID \n");
                }

                sb.Append("WHERE (cf.Label IS NULL OR cf.Label = 'Passed Inspection') \n");

                if (!includeClose)
                {
                    sb.Append("	AND j.Status = 0 \n");
                }

                if (isPassedInspection)
                {
                    sb.Append("	AND cj.Value IS NOT NULL AND cj.Value <> '' \n");
                }

                if (!string.IsNullOrEmpty(objPropContracts.SearchBy) && !string.IsNullOrEmpty(objPropContracts.SearchValue))
                {
                    if (objPropContracts.SearchBy == "r.name" || objPropContracts.SearchBy == "l.tag" || objPropContracts.SearchBy == "r.State")
                    {
                        sb.Append(" AND " + objPropContracts.SearchBy + " LIKE '%" + objPropContracts.SearchValue + "%'");
                    }
                    else if (objPropContracts.SearchBy == "B.Name" && objPropContracts.EN == 1)
                    {
                        sb.Append(" AND UC.IsSel= 1 AND r.EN =" + objPropContracts.SearchValue + " AND UC.UserID =" + objPropContracts.UserID);
                    }
                    else if (objPropContracts.SearchBy == "j.SPHandle")
                    {
                        if (objPropContracts.SearchValue != "-1")
                        {
                            sb.Append(" AND j.SPHandle  = '" + objPropContracts.SearchValue + "'");
                        }
                    }
                    else
                    {
                        sb.Append(" AND " + objPropContracts.SearchBy + " = '" + objPropContracts.SearchValue + "'");
                    }
                }

                if (objPropContracts.EN == 1)
                {
                    sb.Append("AND UC.IsSel= 1 AND UC.UserID = " + objPropContracts.UserID);
                }

                if (objPropContracts.ExpirationDate != DateTime.MinValue)
                {
                    int days = DateTime.DaysInMonth(objPropContracts.ExpirationDate.Year, objPropContracts.ExpirationDate.Month);
                    int date = days - objPropContracts.ExpirationDate.Day;
                    DateTime datec = objPropContracts.ExpirationDate.AddDays(date);

                    sb.Append("AND ExpirationDate <= '" + datec + "'");
                }

                foreach (var obj in filters)
                {
                    if (obj.FilterColumn == "Job")
                    {
                        sb.Append("\n and ct.Job ='" + obj.FilterValue.Trim() + "' ");
                    }
                    if (obj.FilterColumn == "locid")
                    {
                        sb.Append("\n and l.ID like '%" + obj.FilterValue.Trim() + "%' ");
                    }
                    if (obj.FilterColumn == "Tag")
                    {
                        sb.Append("\n and l.Tag like '%" + obj.FilterValue.Trim() + "%' ");
                    }
                    if (obj.FilterColumn == "Name")
                    {
                        sb.Append("\n and r.Name like '%" + obj.FilterValue.Trim() + "%' ");
                    }
                    if (obj.FilterColumn == "CType")
                    {
                        sb.Append("\n and j.CType like '%" + obj.FilterValue.Trim() + "%' ");
                    }
                    if (obj.FilterColumn == "fdesc")
                    {
                        sb.Append("\n and e.fDesc like '%" + obj.FilterValue.Trim() + "%' ");
                    }
                    if (obj.FilterColumn == "BAmt")
                    {
                        sb.Append("\n and ct.BAmt ='" + obj.FilterValue.Trim() + "'");
                    }
                    if (obj.FilterColumn == "Hours")
                    {
                        sb.Append("\n and ct.Hours ='" + obj.FilterValue.Trim() + "'");
                    }
                    if (obj.FilterColumn == "SREMARKS")
                    {
                        sb.Append("\n and J.SREMARKS like '%" + obj.FilterValue.Trim() + "%' ");
                    }
                    if (obj.FilterColumn == "Salesperson")
                    {
                        sb.Append("\n and t.Name like '%" + obj.FilterValue.Trim() + "%' ");
                    }
                    if (obj.FilterColumn == "Salesperson2")
                    {
                        sb.Append("\n and t2.Name like '%" + obj.FilterValue.Trim() + "%' ");
                    }
                    if (obj.FilterColumn == "MonthlyBill")
                    {
                        sb.Append("AND Round (CASE ct.BCycle WHEN 0 THEN ct.BAmt--Monthly; \n ");
                        sb.Append("WHEN 1 THEN ct.BAmt / 2--Bi - Monthly; \n");
                        sb.Append("WHEN 2 THEN ct.BAmt / 3--Quarterly; \n");
                        sb.Append(" WHEN 3 THEN ct.BAmt / 4--3 Times / Year; \n");
                        sb.Append("WHEN 4 THEN ct.BAmt / 6--Semi - Annually; \n");
                        sb.Append("WHEN 5 THEN ct.BAmt / 12--Annually; \n");
                        sb.Append("WHEN 6 THEN 0--'Never'; \n");
                        sb.Append("WHEN 7 THEN ct.BAmt / (12 * 3)--'3 Years'; \n");
                        sb.Append("WHEN 8 THEN ct.BAmt / (12 * 5)--'5 Years'; \n");
                        sb.Append("WHEN 9 THEN ct.BAmt / (12 * 2)--'2 Years'; \n");
                        sb.Append("else 0 END, 2) \n");
                        sb.Append("=" + obj.FilterValue + "\n");
                    }
                    if (obj.FilterColumn == "Freqency")
                    {
                        sb.Append("AND CASE ct.bcycle WHEN 0 THEN 'Monthly' \n");
                        sb.Append("WHEN 1 THEN 'Bi-Monthly' \n");
                        sb.Append("WHEN 2 THEN 'Quarterly' \n");
                        sb.Append("WHEN 3 THEN '3 Times/Year' \n");
                        sb.Append("WHEN 4 THEN 'Semi-Annually' \n");
                        sb.Append("WHEN 5 THEN 'Annually' \n");
                        sb.Append("WHEN 6 THEN 'Never' \n");
                        sb.Append("WHEN 7 THEN '3 Years' \n");
                        sb.Append("WHEN 8 THEN '5 Years' \n");
                        sb.Append("WHEN 9 THEN '2 Years' \n");
                        sb.Append("END like '%");
                        sb.Append(obj.FilterValue.Trim() + "%' \n");
                    }
                    if (obj.FilterColumn == "MonthlyHours")
                    {
                        sb.Append("AND Round (CASE ct.SCycle \n");
                        sb.Append("WHEN 0 THEN ct.Hours --Monthly \n");
                        sb.Append("WHEN 1 THEN ct.Hours / 2 --Bi-Monthly \n");
                        sb.Append("WHEN 2 THEN ct.Hours / 3 --Quarterly \n");
                        sb.Append("WHEN 3 THEN ct.Hours / 6 --Semi-Anually \n");
                        sb.Append("WHEN 4 THEN ct.Hours / 12 --Anually \n");
                        sb.Append("WHEN 5 THEN (ct.Hours * 4.3) --Weekly \n");
                        sb.Append("WHEN 6 THEN (ct.Hours * (2.15))  --Bi-Weekly \n");
                        sb.Append("WHEN 7 THEN ( ct.Hours / ( 2.9898 ) ) --Every 13 Weeks \n");
                        sb.Append("WHEN 10 THEN ct.Hours / 12*2 --Every 2 Years \n");
                        sb.Append("WHEN 8 THEN ct.Hours / 12*3 --Every 3 Years \n");
                        sb.Append("WHEN 9 THEN ct.Hours / 12*5 --Every 5 Years \n");
                        sb.Append("WHEN 11 THEN ct.Hours / 12*7 --Every 7 Years \n");
                        sb.Append("WHEN 13 THEN (ct.Hours * ( CASE ct.SWE WHEN 1 THEN 30 ELSE   21.66 END) ) --Daily \n");
                        sb.Append("WHEN 14 THEN (ct.Hours * (2) ) --Twice a Month \n");
                        sb.Append("WHEN 15 THEN (ct.Hours / (4) ) --3 Times/Year \n");
                        sb.Append("else 0 END, 2 =  \n");
                        sb.Append(obj.FilterValue + "\n");
                    }
                    if (obj.FilterColumn == "TicketFreq")
                    {
                        sb.Append("AND CASE ct.scycle \n");
                        sb.Append("WHEN - 1 THEN 'Never' \n");
                        sb.Append("WHEN 0 THEN 'Monthly' \n");
                        sb.Append("WHEN 1 THEN 'Bi-Monthly' \n");
                        sb.Append("WHEN 2 THEN 'Quarterly' \n");
                        sb.Append("WHEN 3 THEN 'Semi-Annually' \n");
                        sb.Append("WHEN 4 THEN 'Annually' \n");
                        sb.Append("WHEN 5 THEN 'Weekly' \n");
                        sb.Append("WHEN 6 THEN 'Bi-Weekly' \n");
                        sb.Append("WHEN 7 THEN 'Every 13 Weeks' \n");
                        sb.Append("WHEN 10 THEN 'Every 2 Years' \n");
                        sb.Append("WHEN 8 THEN 'Every 3 Years' \n");
                        sb.Append("WHEN 9 THEN 'Every 5 Years' \n");
                        sb.Append("WHEN 11 THEN 'Every 7 Years' \n");
                        sb.Append("WHEN 12 THEN 'On-Demand' \n");
                        sb.Append("WHEN 13 THEN 'Daily' \n");
                        sb.Append("WHEN 14 THEN 'Twice a Month' \n");
                        sb.Append("WHEN 15 THEN '3 Times/Year' \n");
                        sb.Append("END like '%");
                        sb.Append(obj.FilterValue.Trim() + "%' \n");
                    }
                    if (obj.FilterColumn == "Worker")
                    {
                        sb.Append("AND CASE \n");
                        sb.Append("WHEN l.route > 0 THEN \n");
                        sb.Append("(select(select( case  when ro.Name IS NULL  then ''   when tblwork.fdesc is null then  ro.Name    when tblwork.fdesc = ro.Name then ro.Name  else ro.Name + ' - ' + tblwork.fdesc   end)from tblwork where tblwork.id = ro.mech   ) FROM Route ro where ro.ID = l.route) \n");
                        sb.Append("ELSE 'Unassigned' \n");
                        sb.Append("END like '%");
                        sb.Append(obj.FilterValue.Trim() + "%'  \n");
                    }
                    if (obj.FilterColumn == "Status")
                    {
                        sb.Append("CASE ct.Status \n");
                        sb.Append("WHEN 0 THEN 'Active' \n");
                        sb.Append("WHEN 1 THEN 'Closed' \n");
                        sb.Append("WHEN 2 THEN 'Hold' \n");
                        sb.Append("WHEN 3 THEN 'Completed' \n");
                        sb.Append("END like '%");
                        sb.Append(obj.FilterValue.Trim() + "%'  \n");
                    }
                }

                sb.Append("ORDER BY j.ID \n");

                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTicketListPayrollHours(MapData objPropMapData, List<RetainFilter> filters)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("SELECT * FROM( \n");
                //if (objPropMapData.FilterReview != "1")
                //{
                #region TicketO table

                sb.Append("	SELECT  \n");
                sb.Append("			t.[ID] \n");
                sb.Append("			,t.[CDate] \n");
                sb.Append("			,t.[DDate] \n");
                sb.Append("			,t.[EDate] \n");
                sb.Append("			,t.[fWork] \n");
                sb.Append("			,t.[Job] \n");
                sb.Append("			,t.[LID] AS Loc \n");
                sb.Append("			,t.[LElev] AS Elev \n");
                sb.Append("			,t.[Type] \n");
                sb.Append("			,t.[fDesc] \n");
                sb.Append("			,dp.[DescRes] \n");
                sb.Append("			,dp.[Total] \n");
                sb.Append("			,dp.[Reg] \n");
                sb.Append("			,dp.[OT] \n");
                sb.Append("			,dp.[DT] \n");
                sb.Append("			,dp.[TT] \n");
                sb.Append("			,dp.[Zone] \n");
                sb.Append("			,dp.[Toll] \n");
                sb.Append("			,dp.[OtherE] \n");
                sb.Append("			,dp.[Status] \n");
                sb.Append("			,dp.[Invoice] \n");
                sb.Append("			,t.[Level] \n");
                sb.Append("			,t.[Est] \n");
                sb.Append("			,t.[Cat] \n");
                sb.Append("			,t.[Who] \n");
                sb.Append("			,t.[fBy] \n");
                sb.Append("			,t.[fLong] \n");
                sb.Append("			,t.[Latt] \n");
                sb.Append("			,dp.[WageC] \n");
                sb.Append("			,dp.[Phase] \n");
                sb.Append("			,dp.[Car] \n");
                sb.Append("			,dp.[CallIn] \n");
                sb.Append("			,dp.[Mileage] \n");
                sb.Append("			,dp.[NT] \n");
                sb.Append("			,dp.[CauseID] \n");
                sb.Append("			,dp.[CauseDesc] \n");
                sb.Append("			,dp.[Comments] \n");
                sb.Append("			,t.[fGroup] \n");
                sb.Append("			,t.[PriceL] \n");
                sb.Append("			,t.[WorkOrder] \n");
                sb.Append("			,t.[TimeRoute] \n");
                sb.Append("			,t.[TimeSite] \n");
                sb.Append("			,t.[TimeComp] \n");
                sb.Append("			,jt.[Type] AS JobType \n");
                sb.Append("			,ww.[fDesc] AS Mech \n");
                sb.Append("			,l.[Tag] \n");
                sb.Append("			,l.[Address]  \n");
                sb.Append("			,l.[City] \n");
                sb.Append("			,l.[State] \n");
                sb.Append("			,l.[Zip] \n");
                sb.Append("			,ep.ID AS EmployeeID \n");
                sb.Append("			,ep.Name AS EmployeeName \n");
                sb.Append("			,ep.Ref AS EmpCustom1 \n");
                sb.Append("			,pw.fDesc AS WageCode \n");
                sb.Append("			,tp.fDesc AS TemplateName \n");
                sb.Append("			,jl.Tag AS JobName \n");
                sb.Append("			,CASE \n");
                sb.Append("			    WHEN t.Assigned = 0 THEN 'Un-Assigned' \n");
                sb.Append("			    WHEN t.Assigned = 1 THEN 'Assigned' \n");
                sb.Append("			    WHEN t.Assigned = 2 THEN 'Enroute' \n");
                sb.Append("			    WHEN t.Assigned = 3 THEN 'Onsite' \n");
                sb.Append("			    WHEN t.Assigned = 4 THEN 'Completedd' \n");
                sb.Append("			    WHEN t.Assigned = 5 THEN 'Hold' \n");
                sb.Append("			    WHEN t.Assigned = 6 THEN 'Voided' \n");
                sb.Append("			END AS Assignname \n");
                sb.Append("		FROM TicketO t \n");
                sb.Append("			LEFT OUTER JOIN TicketDPDA dp ON t.ID = dp.ID \n");
                sb.Append("			INNER JOIN Loc l ON l.Loc = t.LID  	 \n");
                sb.Append("			INNER JOIN tblWork ww ON ww.ID = t.fWork \n");
                sb.Append("			INNER JOIN Emp ep ON ep.fWork = ww.ID \n");
                sb.Append("			LEFT JOIN Route rou ON rou.ID = l.Route \n");
                sb.Append("			LEFT JOIN Job j ON j.ID = t.Job \n");
                sb.Append("			LEFT JOIN Loc jl on jl.Loc = j.Loc \n");
                sb.Append("			LEFT JOIN JobType jt ON jt.ID = j.Type  \n");
                sb.Append("			LEFT JOIN JobT tp on tp.ID = j.Template \n");
                sb.Append("			LEFT JOIN PRWage pw ON dp.WageC = pw.ID \n");
                sb.Append("			LEFT JOIN tblWork m ON m.ID = rou.Mech \n");
                sb.Append("			LEFT JOIN Elev e ON e.ID = t.LElev \n");
                sb.Append("		WHERE t.Assigned = 4 \n");
                sb.Append("			AND t.EDate >= '" + objPropMapData.StartDate + "' AND t.EDate <= '" + objPropMapData.EndDate + "' \n");

                // If get from from Edit Location screen
                if (objPropMapData.LocID > 0)
                {
                    sb.Append("			AND t.LID = " + objPropMapData.LocID + " \n");
                }

                // If get from from Edit Customer screen
                if (objPropMapData.CustID > 0)
                {
                    sb.Append("			AND l.Owner = " + objPropMapData.CustID + " \n");
                }

                //Advanced Search
                if (!string.IsNullOrEmpty(objPropMapData.Supervisor))
                {
                    sb.Append("			AND m.Super ='" + objPropMapData.Supervisor + "' \n");
                }
                if (!string.IsNullOrEmpty(objPropMapData.FilterCharge))
                {
                    sb.Append("			AND (ISNULL(dp.Charge,0)= " + Convert.ToInt32(objPropMapData.FilterCharge));

                    if (objPropMapData.FilterCharge == "1")
                    {
                        sb.Append(" OR ISNULL(dp.Invoice,0) <> 0) \n");
                    }
                    else
                    {
                        sb.Append(" AND ISNULL(dp.Invoice,0) = 0) \n");
                    }
                }
                if (!string.IsNullOrEmpty(objPropMapData.FilterReview))
                {
                    sb.Append("			AND ISNULL(dp.ClearCheck,0)= " + Convert.ToInt32(objPropMapData.FilterReview) + " \n");
                }
                if (!string.IsNullOrEmpty(objPropMapData.Workorder))
                {
                    sb.Append("			AND t.Workorder= '" + objPropMapData.Workorder + "' \n");
                }
                if (!string.IsNullOrEmpty(objPropMapData.Route))
                {
                    if (Convert.ToInt32(objPropMapData.Route) == 0)
                    {
                        sb.Append("			AND l.Route = 0 \n");
                    }
                    else
                    {
                        sb.Append("			AND rou.ID = " + Convert.ToInt32(objPropMapData.Route) + " \n");
                    }
                }
                if (objPropMapData.Department >= 0)
                {
                    sb.Append("			AND t.type = " + objPropMapData.Department + " \n");
                }
                if (!string.IsNullOrEmpty(objPropMapData.IsPortal))
                {
                    if (objPropMapData.IsPortal == "1")
                    {
                        sb.Append("			AND t.fBy= 'portal' \n");
                    }
                    if (objPropMapData.IsPortal == "0")
                    {
                        sb.Append("			AND t.fBy <> 'portal' \n");
                    }
                }
                if (!string.IsNullOrEmpty(objPropMapData.Bremarks))
                {
                    if (objPropMapData.Bremarks == "1")
                    {
                        sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') <> '' \n");
                    }
                    else
                    {
                        sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') = '' \n");
                    }
                }
                if (objPropMapData.Mobile > 0)
                {
                    if (objPropMapData.Mobile == 2)
                    {
                        sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 2 \n");
                    }
                    if (objPropMapData.Mobile == 1)
                    {
                        sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 0 \n");
                    }
                }
                if (!string.IsNullOrEmpty(objPropMapData.Category))
                {
                    sb.Append("			AND t.Cat IN (" + objPropMapData.Category + ") \n");
                }
                if (objPropMapData.InvoiceID != 0)
                {
                    if (objPropMapData.InvoiceID == 1)
                    {
                        sb.Append("			AND ISNULL(dp.Invoice,0) <> 0 \n");
                    }
                    else if (objPropMapData.InvoiceID == 2)
                    {
                        sb.Append("			AND ISNULL(dp.Invoice,0) = 0 AND ISNULL(dp.Charge,0) = 1 \n");
                    }
                }
                if (objPropMapData.Assigned != -1)
                {
                    if (objPropMapData.Assigned == -2)
                    {
                        sb.Append("			AND t.Assigned <> 4 \n");
                    }
                    else
                    {
                        sb.Append("			AND t.Assigned = " + objPropMapData.Assigned + "  \n");
                    }
                }
                if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                {

                    if (objPropMapData.Worker == "Active")
                    {
                        sb.Append("			AND ww.Status = 0 \n");
                    }
                    else if (objPropMapData.Worker == "Inactive")
                    {
                        sb.Append("			AND ww.Status = 1 \n");
                    }
                    else
                    {
                        sb.Append("			AND ww.fDesc = '" + objPropMapData.Worker.Replace("'", "''") + "' \n");
                    }
                }

                // Search value
                if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && !string.IsNullOrEmpty(objPropMapData.SearchValue))
                {
                    if (objPropMapData.SearchBy == "t.ID")
                    {
                        sb.Append("			AND t.ID = " + objPropMapData.SearchValue + " \n");
                    }
                    if (objPropMapData.SearchBy == "t.cat")
                    {
                        sb.Append("			AND t.Cat LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "t.WorkOrder")
                    {
                        sb.Append("			AND t.WorkOrder LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "t.fdesc")
                    {
                        sb.Append("			AND t.fDesc LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "t.descres")
                    {
                        sb.Append("			AND dp.DescRes LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "r.name")
                    {
                        sb.Append("			AND rou.Name LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "l.tag")
                    {
                        sb.Append("			AND l.Tag LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "l.ldesc4")
                    {
                        sb.Append("			AND l.Address LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "l.City")
                    {
                        sb.Append("			AND l.City LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "l.state")
                    {
                        sb.Append("			AND l.State LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "l.Zip")
                    {
                        sb.Append("			AND l.Zip LIKE '%" + objPropMapData.SearchValue + "%'		 \n");
                    }
                }

                //Ticket filters
                foreach (var filter in filters)
                {
                    if (filter.FilterColumn == "ID")
                    {
                        sb.Append("			AND t.ID IN (" + filter.FilterValue + ") \n");
                    }
                    if (filter.FilterColumn == "WorkOrder")
                    {
                        sb.Append("			AND t.Workorder= '" + filter.FilterValue + "' \n");
                    }
                    if (filter.FilterColumn == "invoiceno")
                    {
                        sb.Append("			AND dp.Invoice = " + filter.FilterValue + " \n");
                    }
                    if (filter.FilterColumn == "Job")
                    {
                        sb.Append("			AND t.Job = " + filter.FilterValue + " \n");
                    }
                    if (filter.FilterColumn == "locname")
                    {
                        sb.Append("			AND l.Tag LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "City")
                    {
                        sb.Append("			AND l.City LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "fullAddress")
                    {
                        sb.Append("			AND l.Address LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "dwork")
                    {
                        sb.Append("			AND t.DWork LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "Name")
                    {
                        sb.Append("			AND rou.Name LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "cat")
                    {
                        sb.Append("			AND t.Cat LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "Tottime")
                    {
                        sb.Append("			AND dp.Total = " + filter.FilterValue + " \n");
                    }
                    if (filter.FilterColumn == "timediff")
                    {
                        sb.Append(@"		AND ROUND(CONVERT(NUMERIC(30, 2), (ISNULL(dp.Total, 0.00) - ( CONVERT(FLOAT, DATEDIFF(MILLISECOND, dp.TimeRoute, 
			                                    (CASE WHEN(CAST(dp.TimeSite AS TIME) < CAST(dp.TimeRoute AS TIME) AND CAST(dp.TimeComp AS TIME) < CAST(dp.TimeSite AS TIME)) THEN DATEADD(DAY, 2, dp.TimeComp)
				                                    ELSE((CASE
                                                        WHEN(CAST(dp.TimeSite AS TIME) < CAST(dp.TimeRoute AS TIME)
                                                            OR CAST(dp.TimeComp AS TIME) < CAST(dp.TimeSite AS TIME)) THEN DATEADD(DAY, 1, dp.TimeComp)
                                                        ELSE dp.TimeComp
                                                    END))
                                                END ))) / 1000 / 60 / 60 ) )), 1) = " + filter.FilterValue + " \n");
                    }
                    if (filter.FilterColumn == "department")
                    {
                        sb.Append("			AND jt.Type LIKE '%" + filter.FilterValue + "%' \n");
                    }
                }

                #endregion
                //}

                //if (objPropMapData.Assigned == 4 || objPropMapData.Assigned == -1)
                //{
                #region TicketD table

                sb.Append("		UNION ALL \n");
                sb.Append("		SELECT  \n");
                sb.Append("			t.[ID] \n");
                sb.Append("			,t.[CDate] \n");
                sb.Append("			,t.[DDate] \n");
                sb.Append("			,t.[EDate] \n");
                sb.Append("			,t.[fWork] \n");
                sb.Append("			,t.[Job] \n");
                sb.Append("			,t.[Loc] \n");
                sb.Append("			,t.[Elev] \n");
                sb.Append("			,t.[Type] \n");
                sb.Append("			,t.[fDesc] \n");
                sb.Append("			,t.[DescRes] \n");
                sb.Append("			,t.[Total] \n");
                sb.Append("			,t.[Reg] \n");
                sb.Append("			,t.[OT] \n");
                sb.Append("			,t.[DT] \n");
                sb.Append("			,t.[TT] \n");
                sb.Append("			,t.[Zone] \n");
                sb.Append("			,t.[Toll] \n");
                sb.Append("			,t.[OtherE] \n");
                sb.Append("			,t.[Status] \n");
                sb.Append("			,t.[Invoice] \n");
                sb.Append("			,t.[Level] \n");
                sb.Append("			,t.[Est] \n");
                sb.Append("			,t.[Cat] \n");
                sb.Append("			,t.[Who] \n");
                sb.Append("			,t.[fBy] \n");
                sb.Append("			,t.[fLong] \n");
                sb.Append("			,t.[Latt] \n");
                sb.Append("			,t.[WageC] \n");
                sb.Append("			,t.[Phase] \n");
                sb.Append("			,t.[Car] \n");
                sb.Append("			,t.[CallIn] \n");
                sb.Append("			,t.[Mileage] \n");
                sb.Append("			,t.[NT] \n");
                sb.Append("			,t.[CauseID] \n");
                sb.Append("			,t.[CauseDesc] \n");
                sb.Append("			,t.[Comments] \n");
                sb.Append("			,t.[fGroup] \n");
                sb.Append("			,t.[PriceL] \n");
                sb.Append("			,t.[WorkOrder] \n");
                sb.Append("			,t.[TimeRoute] \n");
                sb.Append("			,t.[TimeSite] \n");
                sb.Append("			,t.[TimeComp] \n");
                sb.Append("			,jt.[Type] AS JobType \n");
                sb.Append("			,w.[fDesc] AS Mech \n");
                sb.Append("			,l.[Tag] \n");
                sb.Append("			,l.[Address]  \n");
                sb.Append("			,l.[City] \n");
                sb.Append("			,l.[State] \n");
                sb.Append("			,l.[Zip] \n");
                sb.Append("			,ep.ID AS EmployeeID \n");
                sb.Append("			,ep.Name AS EmployeeName \n");
                sb.Append("			,ep.Ref AS EmpCustom1 \n");
                sb.Append("			,pw.fDesc AS WageCode \n");
                sb.Append("			,tp.fDesc AS TemplateName \n");
                sb.Append("			,jl.Tag AS JobName \n");
                sb.Append("			,CASE t.Assigned WHEN 6 THEN 'Voided' ELSE 'Completed' END AS Assignname \n");
                sb.Append("		FROM TicketD t \n");
                sb.Append("			INNER JOIN Loc l ON l.Loc = t.Loc  	 \n");
                sb.Append("			INNER JOIN tblWork w ON w.ID = t.fWork \n");
                sb.Append("			INNER JOIN Emp ep ON ep.fWork = w.ID \n");
                sb.Append("			LEFT JOIN Route rou ON rou.ID = l.Route \n");
                sb.Append("			LEFT JOIN Job j ON j.ID = t.Job \n");
                sb.Append("			LEFT JOIN Loc jl on jl.Loc = j.Loc \n");
                sb.Append("			LEFT JOIN JobType jt ON jt.ID = j.Type  \n");
                sb.Append("			LEFT JOIN JobT tp on tp.ID = j.Template \n");
                sb.Append("			LEFT JOIN PRWage pw ON t.WageC = pw.ID \n");
                sb.Append("			LEFT JOIN tblWork m ON m.ID = rou.Mech \n");
                sb.Append("			LEFT JOIN Elev e ON e.ID = t.Elev \n");
                sb.Append("		WHERE t.EDate >= '" + objPropMapData.StartDate + "' AND t.EDate <= '" + objPropMapData.EndDate + "'  \n");

                // If get from from Edit Location screen
                if (objPropMapData.LocID > 0)
                {
                    sb.Append("			AND t.Loc = " + objPropMapData.LocID + " \n");
                }

                // If get from from Edit Customer screen
                if (objPropMapData.CustID > 0)
                {
                    sb.Append("			AND l.Owner = " + objPropMapData.CustID + " \n");
                }

                // Advanced Search
                if (!string.IsNullOrEmpty(objPropMapData.Supervisor))
                {
                    sb.Append("			AND m.Super ='" + objPropMapData.Supervisor + "' \n");
                }
                if (!string.IsNullOrEmpty(objPropMapData.FilterCharge))
                {
                    sb.Append("			AND (ISNULL(t.Charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge));

                    if (objPropMapData.FilterCharge == "1")
                    {
                        sb.Append(" OR ISNULL(t.Invoice,0) <> 0) \n");
                    }
                    else
                    {
                        sb.Append(" AND ISNULL(t.Invoice,0) = 0) \n");
                    }
                }
                if (!string.IsNullOrEmpty(objPropMapData.FilterReview))
                {
                    sb.Append("			AND ISNULL(t.ClearCheck,0)= " + Convert.ToInt32(objPropMapData.FilterReview) + " \n");
                }
                if (!string.IsNullOrEmpty(objPropMapData.Workorder))
                {
                    sb.Append("			AND t.Workorder= '" + objPropMapData.Workorder + "' \n");
                }
                if (!string.IsNullOrEmpty(objPropMapData.Route))
                {
                    if (Convert.ToInt32(objPropMapData.Route) == 0)
                    {
                        sb.Append("			AND l.Route = 0 \n");
                    }
                    else
                    {
                        sb.Append("			AND rou.ID = " + Convert.ToInt32(objPropMapData.Route) + " \n");
                    }
                }
                if (objPropMapData.Department >= 0)
                {
                    sb.Append("			AND t.type = " + objPropMapData.Department + " \n");
                }
                if (!string.IsNullOrEmpty(objPropMapData.IsPortal))
                {
                    if (objPropMapData.IsPortal == "1")
                    {
                        sb.Append("			AND t.fBy= 'portal' \n");
                    }
                    if (objPropMapData.IsPortal == "0")
                    {
                        sb.Append("			AND t.fBy <> 'portal' \n");
                    }
                }
                if (!string.IsNullOrEmpty(objPropMapData.Bremarks))
                {
                    if (objPropMapData.Bremarks == "1")
                    {
                        sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') <> '' \n");
                    }
                    else
                    {
                        sb.Append("			AND ISNULL(RTRIM(LTRIM(t.Bremarks)),'') = '' \n");
                    }
                }
                if (!string.IsNullOrEmpty(objPropMapData.Timesheet))
                {
                    sb.Append("			AND ISNULL(t.TransferTime,0) = " + Convert.ToInt32(objPropMapData.Timesheet) + " \n");
                }
                if (objPropMapData.Mobile > 0)
                {
                    if (objPropMapData.Mobile == 2)
                    {
                        sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 2 \n");
                    }
                    if (objPropMapData.Mobile == 1)
                    {
                        sb.Append("			AND (SELECT CASE WHEN EXISTS ( SELECT 01 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS Comp) = 0 \n");
                    }
                }
                if (!string.IsNullOrEmpty(objPropMapData.Category))
                {
                    sb.Append("			AND t.Cat IN (" + objPropMapData.Category + ") \n");
                }
                if (objPropMapData.InvoiceID != 0)
                {
                    if (objPropMapData.InvoiceID == 1)
                    {
                        sb.Append("			AND ISNULL(t.Invoice,0) <> 0 \n");
                    }
                    else if (objPropMapData.InvoiceID == 2)
                    {
                        sb.Append("			AND ISNULL(t.Invoice,0) = 0 AND ISNULL(t.Charge,0) = 1 \n");
                    }
                }
                if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                {

                    if (objPropMapData.Worker == "Active")
                    {
                        sb.Append("			AND w.Status = 0 \n");
                    }
                    else if (objPropMapData.Worker == "Inactive")
                    {
                        sb.Append("			AND w.Status = 1 \n");
                    }
                    else
                    {
                        sb.Append("			AND w.fDesc = '" + objPropMapData.Worker.Replace("'", "''") + "' \n");
                    }
                }

                // Search value
                if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && !string.IsNullOrEmpty(objPropMapData.SearchValue))
                {
                    if (objPropMapData.SearchBy == "t.ID")
                    {
                        sb.Append("			AND t.ID = " + objPropMapData.SearchValue + " \n");
                    }
                    if (objPropMapData.SearchBy == "t.cat")
                    {
                        sb.Append("			AND t.Cat LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "t.WorkOrder")
                    {
                        sb.Append("			AND t.WorkOrder LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "t.fdesc")
                    {
                        sb.Append("			AND t.fDesc LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "t.descres")
                    {
                        sb.Append("			AND t.DescRes LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "r.name")
                    {
                        sb.Append("			AND rou.Name LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "l.tag")
                    {
                        sb.Append("			AND l.Tag LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "l.ldesc4")
                    {
                        sb.Append("			AND l.Address LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "l.City")
                    {
                        sb.Append("			AND l.City LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "l.state")
                    {
                        sb.Append("			AND l.State LIKE '%" + objPropMapData.SearchValue + "%' \n");
                    }
                    if (objPropMapData.SearchBy == "l.Zip")
                    {
                        sb.Append("			AND l.Zip LIKE '%" + objPropMapData.SearchValue + "%' n");
                    }
                }

                //Ticket filters
                foreach (var filter in filters)
                {
                    if (filter.FilterColumn == "ID")
                    {
                        sb.Append("			AND t.ID IN (" + filter.FilterValue + ") \n");
                    }
                    if (filter.FilterColumn == "WorkOrder")
                    {
                        sb.Append("			AND t.Workorder= '" + filter.FilterValue + "' \n");
                    }
                    if (filter.FilterColumn == "invoiceno")
                    {
                        sb.Append("			AND t.Invoice = " + filter.FilterValue + " \n");
                    }
                    if (filter.FilterColumn == "Job")
                    {
                        sb.Append("			AND t.Job = " + filter.FilterValue + " \n");
                    }
                    if (filter.FilterColumn == "locname")
                    {
                        sb.Append("			AND l.Tag LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "City")
                    {
                        sb.Append("			AND l.City LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "fullAddress")
                    {
                        sb.Append("			AND l.Address LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "dwork")
                    {
                        sb.Append("			AND w.fDesc LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "Name")
                    {
                        sb.Append("			AND rou.Name LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "cat")
                    {
                        sb.Append("			AND t.Cat LIKE '%" + filter.FilterValue + "%' \n");
                    }
                    if (filter.FilterColumn == "Tottime")
                    {
                        sb.Append("			AND t.Total = " + filter.FilterValue + " \n");
                    }
                    if (filter.FilterColumn == "timediff")
                    {
                        sb.Append(@"		AND ROUND(CONVERT(NUMERIC(30, 2), (ISNULL(t.Total, 0.00) - ( CONVERT(FLOAT, DATEDIFF(MILLISECOND, t.TimeRoute, 
                                                (CASE WHEN(CAST(t.TimeSite AS TIME) < CAST(t.TimeRoute AS TIME) AND CAST(t.TimeComp AS TIME) < CAST(t.TimeSite AS TIME)) THEN DATEADD(DAY, 2, t.TimeComp)

                                                    ELSE((CASE
                                                        WHEN(CAST(t.TimeSite AS TIME) < CAST(t.TimeRoute AS TIME)
                                                            OR CAST(t.TimeComp AS TIME) < CAST(t.TimeSite AS TIME)) THEN DATEADD(DAY, 1, t.TimeComp)
                                                        ELSE t.TimeComp
                                                    END))
                                                END))) / 1000 / 60 / 60 ) )), 1) = " + filter.FilterValue + " \n");
                    }
                    if (filter.FilterColumn == "department")
                    {
                        sb.Append("			AND jt.Type LIKE '%" + filter.FilterValue + "%' \n");
                    }
                }

                #endregion
                //}

                sb.Append(") temp \n");

                sb.Append("WHERE 1 = 1 \n");

                if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && !string.IsNullOrEmpty(objPropMapData.SearchValue) && objPropMapData.SearchBy == "e.unit")
                {
                    sb.Append("	AND Unit LIKE '%" + objPropMapData.SearchValue + "%' \n");
                }

                var unitFilter = filters.FirstOrDefault(x => x.FilterColumn == "unit");
                if (unitFilter != null)
                {
                    sb.Append("			AND Unit LIKE '%" + unitFilter.FilterValue + "%' \n");
                }

                if (objPropMapData.Voided == 1)
                {
                    sb.Append("			AND temp.Assignname = 'Voided' \n");
                }

                if (objPropMapData.Assigned == 4 && objPropMapData.Voided != 1)
                {
                    sb.Append("			AND temp.Assignname <> 'Voided' \n");
                }

                sb.Append("ORDER BY temp.[ID] DESC \n");

                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
