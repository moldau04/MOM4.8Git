using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using BusinessEntity;
using System.Collections.Generic;

namespace DataLayer
{
    public class DL_Contracts
    {

        public DataSet getContractsData(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT j.SREMARKS ,c.ExpirationDate, c.Job, \n");
            varname1.Append("       j.ctype, \n");
            varname1.Append("       j.fdesc, \n");
            varname1.Append("       c.BAmt, \n");
            varname1.Append("       c.Hours, \n");
            varname1.Append("       ISNULL(c.OriginalContract, j.fDate) AS OriginalContract, \n");
            varname1.Append("       ISNULL(c.LastRenew,j.fDate) AS LastRenew, \n");
            varname1.Append("       c.BLenght, \n");
            varname1.Append("       c.BStart, \n");
            varname1.Append("       c.SStart, \n");
            varname1.Append("       CONVERT(VARCHAR(8),c.STime,108) AS ScheduledTime, \n");
            varname1.Append("       j.RenewalNotes, \n");
            varname1.Append("       ISNULL(j.BillRate,0) as BillingRate, \n");
            varname1.Append("       ISNULL(j.RateOT,0) as RateOT, \n");
            varname1.Append("       ISNULL(j.RateNT,0) as RateNT, \n");
            varname1.Append("       ISNULL(j.RateMileage,0) as RateMileage, \n");
            varname1.Append("       ISNULL(j.RateDT,0) as RateDT, \n");
            varname1.Append("       ISNULL(j.RateTravel,0) as RateTravel, \n");
            varname1.Append("       c.BEscCycle, \n");
            varname1.Append("       c.BEscFact, \n");
            varname1.Append("       c.EscLast, \n");
            varname1.Append("       CASE c.BEscType \n");
            varname1.Append("           WHEN 0 THEN 'Commodity Index' \n");
            varname1.Append("           WHEN 1 THEN 'Escalation' \n");
            varname1.Append("           WHEN 2 THEN 'Return' \n");
            varname1.Append("           WHEN 3 THEN 'Manual' \n");
            varname1.Append("       END AS EscalationType, \n");
            varname1.Append("       CASE c.Expiration WHEN 1 THEN 'Contract expiration date' ELSE 'Indefinitely' END AS ExpirationType, \n");
            varname1.Append("       l.ID AS locid, \n");
            varname1.Append("       l.Tag, \n");
            varname1.Append("       isnull(l.credit,0) as credit, \n");
            varname1.Append("       r.EN, \n");
            varname1.Append("       B.Name As Company, \n");
            varname1.Append("       r.State, \n");
            varname1.Append("       r.Name, \n");
            varname1.Append("       Round (CASE c.BCycle \n");
            varname1.Append("                WHEN 0 THEN c.BAmt    --Monthly  \n");
            varname1.Append("                WHEN 1 THEN c.BAmt / 2  --Bi-Monthly  \n");
            varname1.Append("                WHEN 2 THEN c.BAmt / 3 --Quarterly  \n");
            varname1.Append("                WHEN 3 THEN c.BAmt / 4 --3 Times/Year  \n");
            varname1.Append("                WHEN 4 THEN c.BAmt / 6 --Semi-Annually   \n");
            varname1.Append("				 WHEN 5 THEN c.BAmt / 12 --Annually \n");
            varname1.Append("                WHEN 6 THEN   0         --'Never'  \n");
            varname1.Append("                WHEN 7 THEN c.BAmt / (12*3)   --'3 Years'  \n");
            varname1.Append("                WHEN 8 THEN c.BAmt / (12*5)    --'5 Years'  \n");
            varname1.Append("                WHEN 9 THEN c.BAmt / (12*2)    --'2 Years'  \n");
            varname1.Append("                else 0  \n");
            varname1.Append("              END, 2)         AS MonthlyBill, \n");
            varname1.Append("       Round (CASE c.SCycle \n");
            varname1.Append("                WHEN 0 THEN c.Hours --Monthly \n");
            varname1.Append("                WHEN 1 THEN c.Hours / 2 --Bi-Monthly \n");
            varname1.Append("                WHEN 2 THEN c.Hours / 3 --Quarterly \n");
            varname1.Append("                WHEN 3 THEN c.Hours / 6 --Semi-Anually \n");
            varname1.Append("                WHEN 4 THEN c.Hours / 12 --Anually \n");
            varname1.Append("                WHEN 5 THEN (c.Hours * 4.3) --Weekly \n");
            varname1.Append("                WHEN 6 THEN (c.Hours * (2.15))  --Bi-Weekly \n");
            varname1.Append("                WHEN 7 THEN ( c.Hours / ( 2.9898 ) ) --Every 13 Weeks  \n");
            varname1.Append("                WHEN 10 THEN c.Hours / 12*2 --Every 2 Years \n");
            varname1.Append("                WHEN 8 THEN c.Hours / 12*3 --Every 3 Years \n");
            varname1.Append("                WHEN 9 THEN c.Hours / 12*5 --Every 5 Years \n");
            varname1.Append("                WHEN 11 THEN c.Hours / 12*7 --Every 7 Years \n");
            varname1.Append("                WHEN 13 THEN (c.Hours * ( CASE c.SWE WHEN 1 THEN 30 ELSE   21.66 END) ) --Daily \n");
            varname1.Append("                WHEN 14 THEN (c.Hours * (2) ) --Twice a Month \n");
            varname1.Append("                WHEN 15 THEN (c.Hours / (4) ) --3 Times/Year  \n");
            varname1.Append("                else 0  \n");
            varname1.Append("              END, 2) AS MonthlyHours, \n");
            varname1.Append("       CASE c.bcycle \n");
            varname1.Append("         WHEN 0 THEN 'Monthly' \n");
            varname1.Append("         WHEN 1 THEN 'Bi-Monthly' \n");
            varname1.Append("         WHEN 2 THEN 'Quarterly' \n");
            varname1.Append("         WHEN 3 THEN '3 Times/Year' \n");
            varname1.Append("         WHEN 4 THEN 'Semi-Annually' \n");
            varname1.Append("         WHEN 5 THEN 'Annually' \n");
            varname1.Append("         WHEN 6 THEN 'Never' \n");
            varname1.Append("         WHEN 7 THEN '3 Years' \n");
            varname1.Append("         WHEN 8 THEN '5 Years' \n");
            varname1.Append("         WHEN 9 THEN '2 Years' \n");
            varname1.Append("       END Freqency, \n");
            varname1.Append("       CASE c.scycle \n");
            varname1.Append("         WHEN -1 THEN 'Never' \n");
            varname1.Append("         WHEN 0 THEN 'Monthly' \n");
            varname1.Append("         WHEN 1 THEN 'Bi-Monthly' \n");
            varname1.Append("         WHEN 2 THEN 'Quarterly' \n");
            varname1.Append("         WHEN 3 THEN 'Semi-Annually' \n");
            varname1.Append("         WHEN 4 THEN 'Annually' \n");
            varname1.Append("         WHEN 5 THEN 'Weekly' \n");
            varname1.Append("         WHEN 6 THEN 'Bi-Weekly' \n");
            varname1.Append("         WHEN 7 THEN 'Every 13 Weeks' \n");
            varname1.Append("         WHEN 10 THEN 'Every 2 Years' \n");
            varname1.Append("         WHEN 8 THEN 'Every 3 Years' \n");
            varname1.Append("         WHEN 9 THEN 'Every 5 Years' \n");
            varname1.Append("         WHEN 11 THEN 'Every 7 Years' \n");
            varname1.Append("         WHEN 12 THEN 'On-Demand' \n");
            varname1.Append("         WHEN 13 THEN 'Daily' \n");
            varname1.Append("         WHEN 14 THEN 'Twice a Month' \n");
            varname1.Append("         WHEN 15 THEN '3 Times/Year' \n");
            varname1.Append("       END TicketFreq, \n");
            varname1.Append("       CASE c.Status \n");
            varname1.Append("         WHEN 0 THEN 'Active' \n");
            varname1.Append("         WHEN 1 THEN 'Closed' \n");
            varname1.Append("         WHEN 2 THEN 'Hold' \n");
            varname1.Append("         WHEN 3 THEN 'Completed' \n");
            varname1.Append("       END  Status, \n");
            varname1.Append("         CASE   \n");
            varname1.Append("         WHEN l.route > 0 THEN  \n");
            varname1.Append("         ( select   (select( case  when ro.Name IS NULL  then ''   when tblwork.fdesc is null then  ro.Name    when tblwork.fdesc = ro.Name then ro.Name  else ro.Name +' - ' + tblwork.fdesc   end)from tblwork where tblwork.id = ro.mech   ) FROM Route ro where ro.ID =    l.route) \n");
            varname1.Append("         ELSE 'Unassigned' \n");
            varname1.Append("         END  AS Worker, \n");
            varname1.Append("         t.Name AS Salesperson, \n");
            varname1.Append("         t2.Name AS Salesperson2, \n");
            varname1.Append("         ElevCount.ElevCount AS ElevCount \n");
            varname1.Append("FROM   Job j \n");
            varname1.Append("       INNER JOIN Contract c ON j.id = c.Job \n");
            varname1.Append("       LEFT OUTER JOIN Loc l ON l.Loc = c.Loc \n");
            varname1.Append("       LEFT OUTER JOIN Owner o ON o.ID = l.Owner \n");
            varname1.Append("       LEFT OUTER JOIN Terr t ON t.ID = l.Terr \n");
            varname1.Append("       LEFT OUTER JOIN Terr t2 ON t2.ID = l.Terr2 \n");
            varname1.Append("       LEFT OUTER JOIN Rol r ON o.Rol = r.ID \n");
            varname1.Append("       LEFT OUTER JOIN Branch B on r.EN = B.ID \n");
            varname1.Append("LEFT JOIN (Select ct.Job,COUNT(tb.Elev) ElevCount From tblJoinElevJob tb Right Join Contract ct on tb.Job=ct.Job Group By ct.Job ) ElevCount  on ElevCount.Job=c.Job \n");
            if (objPropContracts.EN == 1)
                varname1.Append("       LEFT OUTER JOIN tblUserCO UC on UC.CompanyID = B.ID \n");
            varname1.Append(" WHERE  j.type = 0  \n");

            if (!string.IsNullOrEmpty(objPropContracts.SearchBy))
            {
                if (objPropContracts.SearchBy == "r.name" || objPropContracts.SearchBy == "l.tag" || objPropContracts.SearchBy == "r.State")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " like '%" + objPropContracts.SearchValue + "%'");
                }
                else if (objPropContracts.SearchBy == "B.Name" && objPropContracts.EN == 1)
                {
                    varname1.Append(" and UC.IsSel= 1 and r.EN =" + objPropContracts.SearchValue + "  and UC.UserID =" + objPropContracts.UserID);
                }
                else if (objPropContracts.SearchBy == "j.SPHandle")
                {
                    if (objPropContracts.SearchValue != "-1") varname1.Append(" and j.SPHandle  = '" + objPropContracts.SearchValue + "'");
                }
                else
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " = '" + objPropContracts.SearchValue + "'");
                }
            }
            if (objPropContracts.EN == 1)
            {
                varname1.Append("AND UC.IsSel= 1  and UC.UserID = " + objPropContracts.UserID);
            }
            if (objPropContracts.ExpirationDate != DateTime.MinValue)
            {
                int days = DateTime.DaysInMonth(objPropContracts.ExpirationDate.Year, objPropContracts.ExpirationDate.Month);
                int date = days - objPropContracts.ExpirationDate.Day;
                DateTime datec = objPropContracts.ExpirationDate.AddDays(date);
                varname1.Append("and ExpirationDate <= '" + datec + "'");
                //varname1.Append(" and month(ExpirationDate) = month('" + objPropContracts.ExpirationDate + "') and year(ExpirationDate) = year('" + objPropContracts.ExpirationDate + "') ");
            }

            varname1.Append("order by c.job ");

            try
            {
                return objPropContracts.Ds = SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getContractsData(GetContractsDataParam _GetContractsData, string ConnectionString)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT j.SREMARKS ,c.expirationdate, c.Job, \n");
            varname1.Append("       j.ctype, \n");
            varname1.Append("       j.fdesc, \n");
            varname1.Append("       c.BAmt, \n");
            varname1.Append("       c.Hours, \n");
            varname1.Append("       l.ID                   AS locid, \n");
            varname1.Append("       l.Tag, \n");
            varname1.Append("       isnull(l.credit,0) as credit, \n");
            varname1.Append("       r.EN, \n");
            varname1.Append("       B.Name As Company, \n");
            varname1.Append("       r.State, \n");
            varname1.Append("       (SELECT TOP 1 name \n");
            varname1.Append("        FROM   rol r \n");
            varname1.Append("        WHERE  o.rol = r.id \n");
            varname1.Append("               )AS name, \n");
            varname1.Append("       Round (CASE c.BCycle \n");

            varname1.Append("                WHEN 0 THEN c.BAmt    --Monthly  \n");
            varname1.Append("                WHEN 1 THEN c.BAmt / 2  --Bi-Monthly  \n");
            varname1.Append("                WHEN 2 THEN c.BAmt / 3 --Quarterly  \n");
            varname1.Append("                WHEN 3 THEN c.BAmt / 4 --3 Times/Year  \n");
            varname1.Append("                WHEN 4 THEN c.BAmt / 6 --Semi-Annually   \n");
            varname1.Append("				WHEN 5 THEN c.BAmt / 12 --Annually \n");
            varname1.Append("                WHEN 6 THEN   0         --'Never'  \n");
            varname1.Append("                WHEN 7 THEN c.BAmt / (12*3)   --'3 Years'  \n");
            varname1.Append("                WHEN 8 THEN c.BAmt / (12*5)    --'5 Years'  \n");
            varname1.Append("                WHEN 9 THEN c.BAmt / (12*2)    --'2 Years'  \n");
            varname1.Append("                else 0  \n");
            varname1.Append("              END, 2)         AS MonthlyBill, \n");
            varname1.Append("       Round (CASE c.SCycle \n");
            varname1.Append("                WHEN 0 THEN c.Hours --Monthly \n");
            varname1.Append("                WHEN 1 THEN c.Hours / 2 --Bi-Monthly \n");
            varname1.Append("                WHEN 2 THEN c.Hours / 3 --Quarterly \n");
            varname1.Append("                WHEN 3 THEN c.Hours / 6 --Semi-Anually \n");
            varname1.Append("                WHEN 4 THEN c.Hours / 12 --Anually \n");
            varname1.Append("                WHEN 5 THEN (c.Hours * 4.3) --Weekly \n");
            varname1.Append("                WHEN 6 THEN (c.Hours * (2.15))  --Bi-Weekly \n");
            varname1.Append("                WHEN 7 THEN ( c.Hours / ( 2.9898 ) ) --Every 13 Weeks  \n");
            varname1.Append("                WHEN 10 THEN c.Hours / 12*2 --Every 2 Years \n");
            varname1.Append("                WHEN 8 THEN c.Hours / 12*3 --Every 3 Years \n");
            varname1.Append("                WHEN 9 THEN c.Hours / 12*5 --Every 5 Years \n");
            varname1.Append("                WHEN 11 THEN c.Hours / 12*7 --Every 7 Years \n");
            varname1.Append("                WHEN 13 THEN (c.Hours * ( CASE c.SWE WHEN 1 THEN 30 ELSE   21.66 END) ) --Daily \n");
            varname1.Append("                WHEN 14 THEN (c.Hours * (2) ) --Twice a Month \n");
            varname1.Append("                WHEN 15 THEN (c.Hours / (4) ) --3 Times/Year  \n");
            varname1.Append("                else 0  \n");
            varname1.Append("              END, 2)         AS MonthlyHours, \n");
            varname1.Append("       CASE c.bcycle \n");
            varname1.Append("         WHEN 0 THEN 'Monthly' \n");
            varname1.Append("         WHEN 1 THEN 'Bi-Monthly' \n");
            varname1.Append("         WHEN 2 THEN 'Quarterly' \n");
            varname1.Append("         WHEN 3 THEN '3 Times/Year' \n");
            varname1.Append("         WHEN 4 THEN 'Semi-Annually' \n");
            varname1.Append("         WHEN 5 THEN 'Annually' \n");
            varname1.Append("         WHEN 6 THEN 'Never' \n");
            varname1.Append("         WHEN 7 THEN '3 Years' \n");
            varname1.Append("         WHEN 8 THEN '5 Years' \n");
            varname1.Append("         WHEN 9 THEN '2 Years' \n");
            varname1.Append("       END                    Freqency, \n");
            varname1.Append("       CASE c.scycle \n");
            varname1.Append("         WHEN -1 THEN 'Never' \n");
            varname1.Append("         WHEN 0 THEN 'Monthly' \n");
            varname1.Append("         WHEN 1 THEN 'Bi-Monthly' \n");
            varname1.Append("         WHEN 2 THEN 'Quarterly' \n");
            varname1.Append("         WHEN 3 THEN 'Semi-Annually' \n");
            varname1.Append("         WHEN 4 THEN 'Annually' \n");
            varname1.Append("         WHEN 5 THEN 'Weekly' \n");
            varname1.Append("         WHEN 6 THEN 'Bi-Weekly' \n");
            varname1.Append("         WHEN 7 THEN 'Every 13 Weeks' \n");
            varname1.Append("         WHEN 10 THEN 'Every 2 Years' \n");
            varname1.Append("         WHEN 8 THEN 'Every 3 Years' \n");
            varname1.Append("         WHEN 9 THEN 'Every 5 Years' \n");
            varname1.Append("         WHEN 11 THEN 'Every 7 Years' \n");
            varname1.Append("         WHEN 12 THEN 'On-Demand' \n");
            varname1.Append("         WHEN 13 THEN 'Daily' \n");
            varname1.Append("         WHEN 14 THEN 'Twice a Month' \n");
            varname1.Append("         WHEN 15 THEN '3 Times/Year' \n");
            varname1.Append("       END                    TicketFreq, \n");
            varname1.Append("       CASE c.Status \n");
            varname1.Append("         WHEN 0 THEN 'Active' \n");
            varname1.Append("         WHEN 1 THEN 'Closed' \n");
            varname1.Append("         WHEN 2 THEN 'Hold' \n");
            varname1.Append("         WHEN 3 THEN 'Completed' \n");
            varname1.Append("       END                    Status, \n");
            varname1.Append("         CASE   \n");
            varname1.Append("         WHEN l.route > 0 THEN  \n");
            varname1.Append("         ( select   (select( case  when ro.Name IS NULL  then ''   when tblwork.fdesc is null then  ro.Name    when tblwork.fdesc = ro.Name then ro.Name  else ro.Name +' - ' + tblwork.fdesc   end)from tblwork where tblwork.id = ro.mech   ) FROM Route ro where ro.ID =    l.route) \n");
            varname1.Append("         ELSE 'Unassigned' \n");
            varname1.Append("         END  AS Worker \n");
            varname1.Append("FROM   job j \n");
            varname1.Append("       INNER JOIN Contract c \n");
            varname1.Append("               ON j.id = c.Job \n");
            varname1.Append("       LEFT OUTER JOIN Loc l \n");
            varname1.Append("                    ON l.Loc = c.Loc \n");
            varname1.Append("       LEFT OUTER JOIN owner o \n");
            varname1.Append("                    ON o.id = l.owner \n");
            varname1.Append("       LEFT OUTER JOIN rol r \n");
            varname1.Append("                    ON o.rol = r.id \n");
            varname1.Append("       LEFT OUTER JOIN Branch B on r.EN = B.ID \n");
            if (_GetContractsData.EN == 1)
                varname1.Append("       LEFT OUTER JOIN tblUserCO UC on UC.CompanyID = B.ID \n");
            varname1.Append(" WHERE  j.type = 0  \n");

            if (!string.IsNullOrEmpty(_GetContractsData.SearchBy))
            {
                if (_GetContractsData.SearchBy == "r.name" || _GetContractsData.SearchBy == "l.tag" || _GetContractsData.SearchBy == "r.State")
                {
                    varname1.Append(" and " + _GetContractsData.SearchBy + " like '%" + _GetContractsData.SearchValue + "%'");
                }
                else if (_GetContractsData.SearchBy == "B.Name" && _GetContractsData.EN == 1)
                {
                    varname1.Append(" and UC.IsSel= 1 and r.EN =" + _GetContractsData.SearchValue + "  and UC.UserID =" + _GetContractsData.UserID);
                }
                else if (_GetContractsData.SearchBy == "j.SPHandle" && _GetContractsData.EN == 1)
                {
                    if (_GetContractsData.SearchValue != "-1") varname1.Append(" and j.SPHandle  = '" + _GetContractsData.SearchValue + "'");
                }
                else
                {
                    varname1.Append(" and " + _GetContractsData.SearchBy + " = '" + _GetContractsData.SearchValue + "'");
                }
            }
            if (_GetContractsData.EN == 1)
            {
                varname1.Append("        AND UC.IsSel= 1  and UC.UserID = " + _GetContractsData.UserID);
            }
            if (_GetContractsData.ExpirationDate != DateTime.MinValue)
            {
                int days = DateTime.DaysInMonth(_GetContractsData.ExpirationDate.Year, _GetContractsData.ExpirationDate.Month);
                int date = days - _GetContractsData.ExpirationDate.Day;
                DateTime datec = _GetContractsData.ExpirationDate.AddDays(date);
                varname1.Append("    and ExpirationDate <= '" + datec + "'");
                //varname1.Append(" and month(ExpirationDate) = month('" + objPropContracts.ExpirationDate + "') and year(ExpirationDate) = year('" + objPropContracts.ExpirationDate + "') ");
            }

            varname1.Append("        order by c.job ");

            try
            {
                return _GetContractsData.Ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetContract(Contracts objPropContracts)
        {
            try
            {

                SqlParameter[] para = new SqlParameter[1];

                para[0] = new SqlParameter();
                para[0].ParameterName = "ContractbyID";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = objPropContracts.JobId;

                return objPropContracts.Ds = SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetContractbyID", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetContract(GetContractParam _GetContract, string ConnectionString)
        {
            StringBuilder QueryText = new StringBuilder();
            QueryText.Append("SELECT j.Loc, \n");
            QueryText.Append("       j.Owner, \n");
            QueryText.Append("       Custom20, \n");
            QueryText.Append("       c.Status, \n");
            QueryText.Append("       BStart, \n");
            QueryText.Append("       BCycle, \n");
            QueryText.Append("       BAmt, \n");
            QueryText.Append("       c.SStart, \n");
            QueryText.Append("       sCycle, \n");
            QueryText.Append("       SDate, \n");
            QueryText.Append("       SDay, \n");
            QueryText.Append("       STime, \n");
            QueryText.Append("       CreditCard, \n");
            QueryText.Append("       j.Remarks, \n");
            QueryText.Append("       l.tag AS locname, \n");
            QueryText.Append("       isnull(l.credit,0) as credit, \n");
            QueryText.Append("       swe, \n");
            QueryText.Append("       c.hours, \n");
            QueryText.Append("       j.ctype, \n");
            QueryText.Append("       j.fdesc, \n");
            QueryText.Append("       j.id, \n");
            QueryText.Append("       ExpirationDate, Expiration, frequencies, \n");
            QueryText.Append("       l.Billing, \n");
            QueryText.Append("       o.Billing AS CustBilling, \n");
            QueryText.Append("       o.Central, \n");
            QueryText.Append("       c.Chart, \n");
            QueryText.Append("       ch.fDesc as GLAcct, \n");
            QueryText.Append("       BEscType, \n");
            QueryText.Append("       BEscCycle, \n");
            QueryText.Append("       BEscFact, \n");
            QueryText.Append("       EscLast, \n");
            QueryText.Append("       isnull(j.BillRate,0) as BillRate, \n");
            QueryText.Append("       isnull(j.RateOT,0) as RateOT, \n");
            QueryText.Append("       isnull(j.RateNT,0) as RateNT, \n");
            QueryText.Append("       isnull(j.RateMileage,0) as RateMileage, \n");
            QueryText.Append("       isnull(j.RateDT,0) as RateDT, \n");
            QueryText.Append("       isnull(j.RateTravel,0) as RateTravel, \n");
            QueryText.Append("       isnull(j.PO,'') as PO, \n");
            QueryText.Append("       j.SPHandle, \n");
            QueryText.Append("       j.SRemarks, \n");
            QueryText.Append("       j.IsRenewalNotes, \n");
            QueryText.Append("       j.RenewalNotes, \n");
            QueryText.Append("       c.Detail, \n");
            QueryText.Append("       ISNULL(j.Type,0) as DepartmentID, \n");
            QueryText.Append("       isnull(j.TaskCategory,'') as TaskCategory   ,  isnull(l.route,0) Route  \n");
            QueryText.Append("FROM   Job j \n");
            QueryText.Append("       INNER JOIN Contract c \n");
            QueryText.Append("               ON c.Job = j.ID \n");
            QueryText.Append("       LEFT JOIN Chart ch ON ch.ID = c.Chart \n");
            QueryText.Append("       INNER JOIN Loc l \n");
            QueryText.Append("               ON l.Loc = j.Loc \n");
            QueryText.Append("       INNER JOIN Owner o \n");
            QueryText.Append("               ON o.ID = l.Owner \n");
            QueryText.Append("WHERE  j.ID = " + _GetContract.JobId);

            try
            {
                return _GetContract.Ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, QueryText.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetElevContract(Contracts objPropContracts)
        {
            try
            {
                return objPropContracts.Ds = SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, "select Elev,Price,hours from tblJoinElevJob where Job=" + objPropContracts.JobId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetElevContract(GetElevContractParam _GetElevContract, string ConnectionString)
        {
            try
            {
                return _GetElevContract.Ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select Elev,Price,hours from tblJoinElevJob where Job=" + _GetElevContract.JobId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getJstatus(Contracts objPropContracts)
        {
            try
            {
                return objPropContracts.Ds = SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, "select status from jstatus");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getJstatus(GetJstatusParam _GetJstatus, string ConnectionString)
        {
            try
            {
                return _GetJstatus.Ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select status from jstatus");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddContract(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[52];

            para[0] = new SqlParameter();
            para[0].ParameterName = "loc";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = objPropContracts.Loc;

            para[1] = new SqlParameter();
            para[1].ParameterName = "owner";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = objPropContracts.Owner;

            para[2] = new SqlParameter();
            para[2].ParameterName = "date";
            para[2].SqlDbType = SqlDbType.DateTime;
            para[2].Value = objPropContracts.Date;

            para[3] = new SqlParameter();
            para[3].ParameterName = "status";
            para[3].SqlDbType = SqlDbType.Int;
            para[3].Value = objPropContracts.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "CreditCard";
            para[4].SqlDbType = SqlDbType.Int;
            para[4].Value = objPropContracts.CreditCard;

            para[5] = new SqlParameter();
            para[5].ParameterName = "Remarks";
            para[5].SqlDbType = SqlDbType.Text;
            para[5].Value = objPropContracts.Remarks;

            para[6] = new SqlParameter();
            para[6].ParameterName = "BStart";
            para[6].SqlDbType = SqlDbType.DateTime;
            para[6].Value = objPropContracts.BStart;

            para[7] = new SqlParameter();
            para[7].ParameterName = "BCycle";
            para[7].SqlDbType = SqlDbType.Int;
            para[7].Value = objPropContracts.BCycle;

            para[8] = new SqlParameter();
            para[8].ParameterName = "BAmt";
            para[8].SqlDbType = SqlDbType.Money;
            para[8].Value = objPropContracts.BAMT;

            para[9] = new SqlParameter();
            para[9].ParameterName = "SStart";
            para[9].SqlDbType = SqlDbType.DateTime;
            para[9].Value = objPropContracts.SStart;

            para[10] = new SqlParameter();
            para[10].ParameterName = "Cycle";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropContracts.Cycle;

            para[11] = new SqlParameter();
            para[11].ParameterName = "SWE";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropContracts.SWE;

            para[12] = new SqlParameter();
            para[12].ParameterName = "stime";
            para[12].SqlDbType = SqlDbType.DateTime;
            para[12].Value = objPropContracts.STime;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Sday";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropContracts.Sday;

            para[14] = new SqlParameter();
            para[14].ParameterName = "SDate";
            para[14].SqlDbType = SqlDbType.Int;
            para[14].Value = objPropContracts.Sdate;

            para[15] = new SqlParameter();
            para[15].ParameterName = "ElevJobData";
            para[15].SqlDbType = SqlDbType.Structured;
            para[15].Value = objPropContracts.DtElevJob;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Route";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropContracts.Route;

            para[17] = new SqlParameter();
            para[17].ParameterName = "hours";
            para[17].SqlDbType = SqlDbType.Decimal;
            para[17].Value = objPropContracts.Hours;

            para[18] = new SqlParameter();
            para[18].ParameterName = "fdesc";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropContracts.Description;

            para[19] = new SqlParameter();
            para[19].ParameterName = "ctype";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropContracts.Ctype;

            para[20] = new SqlParameter();
            para[20].ParameterName = "ExpirationDate";
            para[20].SqlDbType = SqlDbType.DateTime;

            if (objPropContracts.ExpirationDate != System.DateTime.MinValue) para[20].Value = objPropContracts.ExpirationDate;
            else para[20].Value = DBNull.Value;

            para[21] = new SqlParameter();
            para[21].ParameterName = "ExpirationFreq";
            para[21].SqlDbType = SqlDbType.SmallInt;
            para[21].Value = objPropContracts.expirationfreq;

            para[22] = new SqlParameter();
            para[22].ParameterName = "Expiration";
            para[22].SqlDbType = SqlDbType.SmallInt;
            para[22].Value = objPropContracts.Expiration;

            para[23] = new SqlParameter();                   //added by Mayuri 25th dec,15
            para[23].ParameterName = "ContractBill";
            para[23].SqlDbType = SqlDbType.TinyInt;
            para[23].Value = objPropContracts.ContractBill;

            para[24] = new SqlParameter();
            para[24].ParameterName = "CustomerBill";
            para[24].SqlDbType = SqlDbType.TinyInt;
            para[24].Value = objPropContracts.CustBilling;

            para[25] = new SqlParameter();
            para[25].ParameterName = "Central";
            para[25].SqlDbType = SqlDbType.Int;
            para[25].Value = objPropContracts.Central;

            para[26] = new SqlParameter();
            para[26].ParameterName = "Chart";
            para[26].SqlDbType = SqlDbType.Int;
            para[26].Value = objPropContracts.Chart;

            para[27] = new SqlParameter();
            para[27].ParameterName = "JobT";
            para[27].SqlDbType = SqlDbType.Int;
            para[27].Value = objPropContracts.JobTID;

            para[28] = new SqlParameter();
            para[28].ParameterName = "CustomItems";
            para[28].SqlDbType = SqlDbType.Structured;
            para[28].Value = objPropContracts.DtCustom;

            para[29] = new SqlParameter();
            para[29].ParameterName = "@EscalationType";
            para[29].SqlDbType = SqlDbType.Int;
            para[29].Value = objPropContracts.EscalationType;

            para[30] = new SqlParameter();
            para[30].ParameterName = "@EscalationCycle";
            para[30].SqlDbType = SqlDbType.Int;
            para[30].Value = objPropContracts.EscalationCycle;

            para[31] = new SqlParameter();
            para[31].ParameterName = "@EscalationFactor";
            para[31].SqlDbType = SqlDbType.Decimal;
            para[31].Value = objPropContracts.EscalationFactor;

            para[32] = new SqlParameter();
            para[32].ParameterName = "@EscalationLast";
            para[32].SqlDbType = SqlDbType.DateTime;

            if (objPropContracts.EscalationLast != DateTime.MinValue) para[32].Value = objPropContracts.EscalationLast;
            else para[32].Value = DBNull.Value;

            para[33] = new SqlParameter();
            para[33].ParameterName = "BillRate";
            para[33].SqlDbType = SqlDbType.Decimal;
            para[33].Value = objPropContracts.BillRate;

            para[34] = new SqlParameter();
            para[34].ParameterName = "RateOT";
            para[34].SqlDbType = SqlDbType.Decimal;
            para[34].Value = objPropContracts.RateOT;

            para[35] = new SqlParameter();
            para[35].ParameterName = "RateNT";
            para[35].SqlDbType = SqlDbType.Decimal;
            para[35].Value = objPropContracts.RateNT;

            para[36] = new SqlParameter();
            para[36].ParameterName = "RateDT";
            para[36].SqlDbType = SqlDbType.Decimal;
            para[36].Value = objPropContracts.RateDT;

            para[37] = new SqlParameter();
            para[37].ParameterName = "RateTravel";
            para[37].SqlDbType = SqlDbType.Decimal;
            para[37].Value = objPropContracts.RateTravel;

            para[38] = new SqlParameter();
            para[38].ParameterName = "Mileage";
            para[38].SqlDbType = SqlDbType.Decimal;
            para[38].Value = objPropContracts.Mileage;

            para[39] = new SqlParameter();
            para[39].ParameterName = "PO";
            para[39].SqlDbType = SqlDbType.VarChar;
            para[39].Value = objPropContracts.PO;

            para[40] = new SqlParameter();
            para[40].ParameterName = "SPHandle";
            para[40].SqlDbType = SqlDbType.SmallInt;
            para[40].Value = objPropContracts.Handel;

            para[41] = new SqlParameter();
            para[41].ParameterName = "SPRemarks";
            para[41].SqlDbType = SqlDbType.Text;
            para[41].Value = objPropContracts.Notes;

            para[42] = new SqlParameter();
            para[42].ParameterName = "RenewalNotes";
            para[42].SqlDbType = SqlDbType.Text;
            para[42].Value = objPropContracts.RenewalNotes;

            para[43] = new SqlParameter();
            para[43].ParameterName = "IsRenewalNotes";
            para[43].SqlDbType = SqlDbType.SmallInt;
            para[43].Value = objPropContracts.IsRenewalNotes;

            para[44] = new SqlParameter();
            para[44].ParameterName = "Detail";
            para[44].SqlDbType = SqlDbType.SmallInt;
            para[44].Value = objPropContracts.Detail;

            para[45] = new SqlParameter();
            para[45].ParameterName = "taskcategory";
            para[45].SqlDbType = SqlDbType.VarChar;
            para[45].Value = objPropContracts.taskcategory;

            para[46] = new SqlParameter();
            para[46].ParameterName = "@UpdatedBy";
            para[46].SqlDbType = SqlDbType.VarChar;
            para[46].Value = objPropContracts.MOMUSer;

            para[47] = new SqlParameter();
            para[47].ParameterName = "@EstimateId";
            para[47].SqlDbType = SqlDbType.Int;
            para[47].Value = objPropContracts.EstimateId;


            para[48] = new SqlParameter();
            para[48].ParameterName = "@ContractLength";
            para[48].SqlDbType = SqlDbType.Int;
            para[48].Value = objPropContracts.ContractLength;

            para[49] = new SqlParameter();
            para[49].ParameterName = "@OriginalContract";
            para[49].SqlDbType = SqlDbType.DateTime;
            para[49].Value = objPropContracts.OriginalContract;


            para[50] = new SqlParameter();
            para[50].ParameterName = "@LastRenew";
            para[50].SqlDbType = SqlDbType.DateTime;
            para[50].Value = objPropContracts.LastRenew;

            para[51] = new SqlParameter();
            para[51].ParameterName = "@TicketCategory";
            para[51].SqlDbType = SqlDbType.VarChar;
            para[51].Value = objPropContracts.TicketCategory;


            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spAddContract", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void AddContract(AddContractParam _AddContract, string ConnectionString)
        {
            SqlParameter[] para = new SqlParameter[47];

            para[0] = new SqlParameter();
            para[0].ParameterName = "loc";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = _AddContract.Loc;

            para[1] = new SqlParameter();
            para[1].ParameterName = "owner";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = _AddContract.Owner;

            para[2] = new SqlParameter();
            para[2].ParameterName = "date";
            para[2].SqlDbType = SqlDbType.DateTime;
            para[2].Value = _AddContract.Date;

            para[3] = new SqlParameter();
            para[3].ParameterName = "status";
            para[3].SqlDbType = SqlDbType.Int;
            para[3].Value = _AddContract.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "CreditCard";
            para[4].SqlDbType = SqlDbType.Int;
            para[4].Value = _AddContract.CreditCard;

            para[5] = new SqlParameter();
            para[5].ParameterName = "Remarks";
            para[5].SqlDbType = SqlDbType.Text;
            para[5].Value = _AddContract.Remarks;

            para[6] = new SqlParameter();
            para[6].ParameterName = "BStart";
            para[6].SqlDbType = SqlDbType.DateTime;
            para[6].Value = _AddContract.BStart;

            para[7] = new SqlParameter();
            para[7].ParameterName = "BCycle";
            para[7].SqlDbType = SqlDbType.Int;
            para[7].Value = _AddContract.BCycle;

            para[8] = new SqlParameter();
            para[8].ParameterName = "BAmt";
            para[8].SqlDbType = SqlDbType.Money;
            para[8].Value = _AddContract.BAMT;

            para[9] = new SqlParameter();
            para[9].ParameterName = "SStart";
            para[9].SqlDbType = SqlDbType.DateTime;
            para[9].Value = _AddContract.SStart;

            para[10] = new SqlParameter();
            para[10].ParameterName = "Cycle";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = _AddContract.Cycle;

            para[11] = new SqlParameter();
            para[11].ParameterName = "SWE";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = _AddContract.SWE;

            para[12] = new SqlParameter();
            para[12].ParameterName = "stime";
            para[12].SqlDbType = SqlDbType.DateTime;
            para[12].Value = _AddContract.STime;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Sday";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = _AddContract.Sday;

            para[14] = new SqlParameter();
            para[14].ParameterName = "SDate";
            para[14].SqlDbType = SqlDbType.Int;
            para[14].Value = _AddContract.Sdate;

            para[15] = new SqlParameter();
            para[15].ParameterName = "ElevJobData";
            para[15].SqlDbType = SqlDbType.Structured;
            para[15].Value = _AddContract.DtElevJob;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Route";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = _AddContract.Route;

            para[17] = new SqlParameter();
            para[17].ParameterName = "hours";
            para[17].SqlDbType = SqlDbType.Decimal;
            para[17].Value = _AddContract.Hours;

            para[18] = new SqlParameter();
            para[18].ParameterName = "fdesc";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = _AddContract.Description;

            para[19] = new SqlParameter();
            para[19].ParameterName = "ctype";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = _AddContract.Ctype;

            para[20] = new SqlParameter();
            para[20].ParameterName = "ExpirationDate";
            para[20].SqlDbType = SqlDbType.DateTime;
            if (_AddContract.ExpirationDate != System.DateTime.MinValue)
                para[20].Value = _AddContract.ExpirationDate;
            else
                para[20].Value = DBNull.Value;

            para[21] = new SqlParameter();
            para[21].ParameterName = "ExpirationFreq";
            para[21].SqlDbType = SqlDbType.SmallInt;
            para[21].Value = _AddContract.expirationfreq;

            para[22] = new SqlParameter();
            para[22].ParameterName = "Expiration";
            para[22].SqlDbType = SqlDbType.SmallInt;
            para[22].Value = _AddContract.Expiration;

            para[23] = new SqlParameter();                   //added by Mayuri 25th dec,15
            para[23].ParameterName = "ContractBill";
            para[23].SqlDbType = SqlDbType.TinyInt;
            para[23].Value = _AddContract.ContractBill;

            para[24] = new SqlParameter();
            para[24].ParameterName = "CustomerBill";
            para[24].SqlDbType = SqlDbType.TinyInt;
            para[24].Value = _AddContract.CustBilling;

            para[25] = new SqlParameter();
            para[25].ParameterName = "Central";
            para[25].SqlDbType = SqlDbType.Int;
            para[25].Value = _AddContract.Central;

            para[26] = new SqlParameter();
            para[26].ParameterName = "Chart";
            para[26].SqlDbType = SqlDbType.Int;
            para[26].Value = _AddContract.Chart;

            para[27] = new SqlParameter();
            para[27].ParameterName = "JobT";
            para[27].SqlDbType = SqlDbType.Int;
            para[27].Value = _AddContract.JobTID;

            para[28] = new SqlParameter();
            para[28].ParameterName = "CustomItems";
            para[28].SqlDbType = SqlDbType.Structured;
            para[28].Value = _AddContract.DtCustom;

            para[29] = new SqlParameter();
            para[29].ParameterName = "@EscalationType";
            para[29].SqlDbType = SqlDbType.Int;
            para[29].Value = _AddContract.EscalationType;

            para[30] = new SqlParameter();
            para[30].ParameterName = "@EscalationCycle";
            para[30].SqlDbType = SqlDbType.Int;
            para[30].Value = _AddContract.EscalationCycle;

            para[31] = new SqlParameter();
            para[31].ParameterName = "@EscalationFactor";
            para[31].SqlDbType = SqlDbType.Decimal;
            para[31].Value = _AddContract.EscalationFactor;

            para[32] = new SqlParameter();
            para[32].ParameterName = "@EscalationLast";
            para[32].SqlDbType = SqlDbType.DateTime;
            if (_AddContract.EscalationLast != DateTime.MinValue)
                para[32].Value = _AddContract.EscalationLast;
            else
                para[32].Value = DBNull.Value;

            para[33] = new SqlParameter();
            para[33].ParameterName = "BillRate";
            para[33].SqlDbType = SqlDbType.Decimal;
            para[33].Value = _AddContract.BillRate;

            para[34] = new SqlParameter();
            para[34].ParameterName = "RateOT";
            para[34].SqlDbType = SqlDbType.Decimal;
            para[34].Value = _AddContract.RateOT;

            para[35] = new SqlParameter();
            para[35].ParameterName = "RateNT";
            para[35].SqlDbType = SqlDbType.Decimal;
            para[35].Value = _AddContract.RateNT;

            para[36] = new SqlParameter();
            para[36].ParameterName = "RateDT";
            para[36].SqlDbType = SqlDbType.Decimal;
            para[36].Value = _AddContract.RateDT;

            para[37] = new SqlParameter();
            para[37].ParameterName = "RateTravel";
            para[37].SqlDbType = SqlDbType.Decimal;
            para[37].Value = _AddContract.RateTravel;

            para[38] = new SqlParameter();
            para[38].ParameterName = "Mileage";
            para[38].SqlDbType = SqlDbType.Decimal;
            para[38].Value = _AddContract.Mileage;

            para[39] = new SqlParameter();
            para[39].ParameterName = "PO";
            para[39].SqlDbType = SqlDbType.VarChar;
            para[39].Value = _AddContract.PO;

            para[40] = new SqlParameter();
            para[40].ParameterName = "SPHandle";
            para[40].SqlDbType = SqlDbType.SmallInt;
            para[40].Value = _AddContract.Handel;

            para[41] = new SqlParameter();
            para[41].ParameterName = "SPRemarks";
            para[41].SqlDbType = SqlDbType.Text;
            para[41].Value = _AddContract.Notes;

            para[42] = new SqlParameter();
            para[42].ParameterName = "RenewalNotes";
            para[42].SqlDbType = SqlDbType.Text;
            para[42].Value = _AddContract.RenewalNotes;

            para[43] = new SqlParameter();
            para[43].ParameterName = "IsRenewalNotes";
            para[43].SqlDbType = SqlDbType.SmallInt;
            para[43].Value = _AddContract.IsRenewalNotes;

            para[44] = new SqlParameter();
            para[44].ParameterName = "Detail";
            para[44].SqlDbType = SqlDbType.SmallInt;
            para[44].Value = _AddContract.Detail;

            para[45] = new SqlParameter();
            para[45].ParameterName = "taskcategory";
            para[45].SqlDbType = SqlDbType.VarChar;
            para[45].Value = _AddContract.taskcategory;

            para[46] = new SqlParameter();
            para[46].ParameterName = "@UpdatedBy";
            para[46].SqlDbType = SqlDbType.VarChar;
            para[46].Value = _AddContract.MOMUSer;

            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spAddContract", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddContractTemp(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[20];

            para[0] = new SqlParameter();
            para[0].ParameterName = "loc";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropContracts.Locaname;

            para[1] = new SqlParameter();
            para[1].ParameterName = "owner";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = objPropContracts.Owner;

            para[2] = new SqlParameter();
            para[2].ParameterName = "date";
            para[2].SqlDbType = SqlDbType.DateTime;
            para[2].Value = objPropContracts.Date;

            para[3] = new SqlParameter();
            para[3].ParameterName = "status";
            para[3].SqlDbType = SqlDbType.Int;
            para[3].Value = objPropContracts.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "CreditCard";
            para[4].SqlDbType = SqlDbType.Int;
            para[4].Value = objPropContracts.CreditCard;

            para[5] = new SqlParameter();
            para[5].ParameterName = "Remarks";
            para[5].SqlDbType = SqlDbType.Text;
            para[5].Value = objPropContracts.Remarks;

            para[6] = new SqlParameter();
            para[6].ParameterName = "BStart";
            para[6].SqlDbType = SqlDbType.DateTime;
            para[6].Value = objPropContracts.BStart;

            para[7] = new SqlParameter();
            para[7].ParameterName = "BCycle";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropContracts.BcycleName;

            para[8] = new SqlParameter();
            para[8].ParameterName = "BAmt";
            para[8].SqlDbType = SqlDbType.Money;
            para[8].Value = objPropContracts.BAMT;

            para[9] = new SqlParameter();
            para[9].ParameterName = "SStart";
            para[9].SqlDbType = SqlDbType.DateTime;
            para[9].Value = objPropContracts.SStart;

            para[10] = new SqlParameter();
            para[10].ParameterName = "Cycle";
            para[10].SqlDbType = SqlDbType.VarChar;
            para[10].Value = objPropContracts.ScycleName;

            para[11] = new SqlParameter();
            para[11].ParameterName = "SWE";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropContracts.SWE;

            para[12] = new SqlParameter();
            para[12].ParameterName = "stime";
            para[12].SqlDbType = SqlDbType.DateTime;
            para[12].Value = objPropContracts.STime;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Sday";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropContracts.Sday;

            para[14] = new SqlParameter();
            para[14].ParameterName = "SDate";
            para[14].SqlDbType = SqlDbType.Int;
            para[14].Value = objPropContracts.Sdate;

            para[15] = new SqlParameter();
            para[15].ParameterName = "ElevJobData";
            para[15].SqlDbType = SqlDbType.Structured;
            para[15].Value = objPropContracts.DtElevJob;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Route";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropContracts.Route;

            para[17] = new SqlParameter();
            para[17].ParameterName = "hours";
            para[17].SqlDbType = SqlDbType.Decimal;
            para[17].Value = objPropContracts.Hours;

            para[18] = new SqlParameter();
            para[18].ParameterName = "fdesc";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropContracts.Description;

            para[19] = new SqlParameter();
            para[19].ParameterName = "ctype";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropContracts.Ctype;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spADDContractTemp", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateContract(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[51];

            para[0] = new SqlParameter();
            para[0].ParameterName = "loc";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = objPropContracts.Loc;

            para[1] = new SqlParameter();
            para[1].ParameterName = "owner";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = objPropContracts.Owner;

            para[2] = new SqlParameter();
            para[2].ParameterName = "date";
            para[2].SqlDbType = SqlDbType.DateTime;
            para[2].Value = objPropContracts.Date;

            para[3] = new SqlParameter();
            para[3].ParameterName = "status";
            para[3].SqlDbType = SqlDbType.Int;
            para[3].Value = objPropContracts.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "CreditCard";
            para[4].SqlDbType = SqlDbType.Int;
            para[4].Value = objPropContracts.CreditCard;

            para[5] = new SqlParameter();
            para[5].ParameterName = "Remarks";
            para[5].SqlDbType = SqlDbType.Text;
            para[5].Value = objPropContracts.Remarks;

            para[6] = new SqlParameter();
            para[6].ParameterName = "BStart";
            para[6].SqlDbType = SqlDbType.DateTime;
            para[6].Value = objPropContracts.BStart;

            para[7] = new SqlParameter();
            para[7].ParameterName = "BCycle";
            para[7].SqlDbType = SqlDbType.Int;
            para[7].Value = objPropContracts.BCycle;

            para[8] = new SqlParameter();
            para[8].ParameterName = "BAmt";
            para[8].SqlDbType = SqlDbType.Money;
            para[8].Value = objPropContracts.BAMT;

            para[9] = new SqlParameter();
            para[9].ParameterName = "SStart";
            para[9].SqlDbType = SqlDbType.DateTime;
            para[9].Value = objPropContracts.SStart;

            para[10] = new SqlParameter();
            para[10].ParameterName = "Cycle";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropContracts.Cycle;

            para[11] = new SqlParameter();
            para[11].ParameterName = "SWE";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropContracts.SWE;

            para[12] = new SqlParameter();
            para[12].ParameterName = "stime";
            para[12].SqlDbType = SqlDbType.DateTime;
            para[12].Value = objPropContracts.STime;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Sday";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropContracts.Sday;

            para[14] = new SqlParameter();
            para[14].ParameterName = "SDate";
            para[14].SqlDbType = SqlDbType.Int;
            para[14].Value = objPropContracts.Sdate;

            para[15] = new SqlParameter();
            para[15].ParameterName = "ElevJobData";
            para[15].SqlDbType = SqlDbType.Structured;
            para[15].Value = objPropContracts.DtElevJob;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Route";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropContracts.Route;

            para[17] = new SqlParameter();
            para[17].ParameterName = "job";
            para[17].SqlDbType = SqlDbType.Int;
            para[17].Value = objPropContracts.JobId;

            para[18] = new SqlParameter();
            para[18].ParameterName = "hours";
            para[18].SqlDbType = SqlDbType.Decimal;
            para[18].Value = objPropContracts.Hours;

            para[19] = new SqlParameter();
            para[19].ParameterName = "fdesc";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropContracts.Description;

            para[20] = new SqlParameter();
            para[20].ParameterName = "ctype";
            para[20].SqlDbType = SqlDbType.VarChar;
            para[20].Value = objPropContracts.Ctype;

            para[21] = new SqlParameter();
            para[21].ParameterName = "ExpirationDate";
            para[21].SqlDbType = SqlDbType.DateTime;
            if (objPropContracts.ExpirationDate != System.DateTime.MinValue)
                para[21].Value = objPropContracts.ExpirationDate;
            else
                para[21].Value = DBNull.Value;

            para[22] = new SqlParameter();
            para[22].ParameterName = "ExpirationFreq";
            para[22].SqlDbType = SqlDbType.SmallInt;
            para[22].Value = objPropContracts.expirationfreq;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Expiration";
            para[23].SqlDbType = SqlDbType.SmallInt;
            para[23].Value = objPropContracts.Expiration;

            para[24] = new SqlParameter();
            para[24].ParameterName = "ContractBill";
            para[24].SqlDbType = SqlDbType.SmallInt;
            para[24].Value = objPropContracts.ContractBill;

            para[25] = new SqlParameter();
            para[25].ParameterName = "CustomerBill";
            para[25].SqlDbType = SqlDbType.TinyInt;
            para[25].Value = objPropContracts.CustBilling;

            para[26] = new SqlParameter();
            para[26].ParameterName = "Central";
            para[26].SqlDbType = SqlDbType.Int;
            para[26].Value = objPropContracts.Central;

            para[27] = new SqlParameter();
            para[27].ParameterName = "Chart";
            para[27].SqlDbType = SqlDbType.Int;
            para[27].Value = objPropContracts.Chart;

            para[28] = new SqlParameter();
            para[28].ParameterName = "JobT";
            para[28].SqlDbType = SqlDbType.Int;
            para[28].Value = objPropContracts.JobTID;

            para[29] = new SqlParameter();
            para[29].ParameterName = "CustomItems";
            para[29].SqlDbType = SqlDbType.Structured;
            para[29].Value = objPropContracts.DtCustom;

            para[30] = new SqlParameter();
            para[30].ParameterName = "@EscalationCycle";
            para[30].SqlDbType = SqlDbType.Int;
            para[30].Value = objPropContracts.EscalationCycle;

            para[31] = new SqlParameter();
            para[31].ParameterName = "@EscalationFactor";
            para[31].SqlDbType = SqlDbType.Decimal;
            para[31].Value = objPropContracts.EscalationFactor;

            para[32] = new SqlParameter();
            para[32].ParameterName = "@EscalationLast";
            para[32].SqlDbType = SqlDbType.DateTime;
            if (objPropContracts.EscalationLast != DateTime.MinValue)
                para[32].Value = objPropContracts.EscalationLast;
            else
                para[32].Value = DBNull.Value;

            para[33] = new SqlParameter();
            para[33].ParameterName = "@EscalationType";
            para[33].SqlDbType = SqlDbType.Int;
            para[33].Value = objPropContracts.EscalationType;

            para[34] = new SqlParameter();
            para[34].ParameterName = "BillRate";
            para[34].SqlDbType = SqlDbType.Decimal;
            para[34].Value = objPropContracts.BillRate;

            para[35] = new SqlParameter();
            para[35].ParameterName = "RateOT";
            para[35].SqlDbType = SqlDbType.Decimal;
            para[35].Value = objPropContracts.RateOT;

            para[36] = new SqlParameter();
            para[36].ParameterName = "RateNT";
            para[36].SqlDbType = SqlDbType.Decimal;
            para[36].Value = objPropContracts.RateNT;

            para[37] = new SqlParameter();
            para[37].ParameterName = "RateDT";
            para[37].SqlDbType = SqlDbType.Decimal;
            para[37].Value = objPropContracts.RateDT;

            para[38] = new SqlParameter();
            para[38].ParameterName = "RateTravel";
            para[38].SqlDbType = SqlDbType.Decimal;
            para[38].Value = objPropContracts.RateTravel;

            para[39] = new SqlParameter();
            para[39].ParameterName = "Mileage";
            para[39].SqlDbType = SqlDbType.Decimal;
            para[39].Value = objPropContracts.Mileage;

            para[40] = new SqlParameter();
            para[40].ParameterName = "PO";
            para[40].SqlDbType = SqlDbType.VarChar;
            para[40].Value = objPropContracts.PO;

            para[41] = new SqlParameter();
            para[41].ParameterName = "SPHandle";
            para[41].SqlDbType = SqlDbType.SmallInt;
            para[41].Value = objPropContracts.Handel;

            para[42] = new SqlParameter();
            para[42].ParameterName = "SPRemarks";
            para[42].SqlDbType = SqlDbType.VarChar;
            para[42].Value = objPropContracts.Notes;

            para[43] = new SqlParameter();
            para[43].ParameterName = "RenewalNotes";
            para[43].SqlDbType = SqlDbType.Text;
            para[43].Value = objPropContracts.RenewalNotes;

            para[44] = new SqlParameter();
            para[44].ParameterName = "IsRenewalNotes";
            para[44].SqlDbType = SqlDbType.SmallInt;
            para[44].Value = objPropContracts.IsRenewalNotes;

            para[45] = new SqlParameter();
            para[45].ParameterName = "Detail";
            para[45].SqlDbType = SqlDbType.SmallInt;
            para[45].Value = objPropContracts.Detail;

            para[46] = new SqlParameter();
            para[46].ParameterName = "@UpdatedBy";
            para[46].SqlDbType = SqlDbType.VarChar;
            para[46].Value = objPropContracts.MOMUSer;

            para[47] = new SqlParameter();
            para[47].ParameterName = "@ContractLength";
            para[47].SqlDbType = SqlDbType.Int;
            para[47].Value = objPropContracts.ContractLength;

            para[48] = new SqlParameter();
            para[48].ParameterName = "@OriginalContract";
            para[48].SqlDbType = SqlDbType.DateTime;
            para[48].Value = objPropContracts.OriginalContract;


            para[49] = new SqlParameter();
            para[49].ParameterName = "@LastRenew";
            para[49].SqlDbType = SqlDbType.DateTime;
            para[49].Value = objPropContracts.LastRenew;

            para[50] = new SqlParameter();
            para[50].ParameterName = "@TicketCategory";
            para[50].SqlDbType = SqlDbType.VarChar;
            para[50].Value = objPropContracts.TicketCategory;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spUpdateContract", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void UpdateContract(UpdateContractParam _UpdateContract, string ConnectionString)
        {
            SqlParameter[] para = new SqlParameter[47];

            para[0] = new SqlParameter();
            para[0].ParameterName = "loc";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = _UpdateContract.Loc;

            para[1] = new SqlParameter();
            para[1].ParameterName = "owner";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = _UpdateContract.Owner;

            para[2] = new SqlParameter();
            para[2].ParameterName = "date";
            para[2].SqlDbType = SqlDbType.DateTime;
            para[2].Value = _UpdateContract.Date;

            para[3] = new SqlParameter();
            para[3].ParameterName = "status";
            para[3].SqlDbType = SqlDbType.Int;
            para[3].Value = _UpdateContract.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "CreditCard";
            para[4].SqlDbType = SqlDbType.Int;
            para[4].Value = _UpdateContract.CreditCard;

            para[5] = new SqlParameter();
            para[5].ParameterName = "Remarks";
            para[5].SqlDbType = SqlDbType.Text;
            para[5].Value = _UpdateContract.Remarks;

            para[6] = new SqlParameter();
            para[6].ParameterName = "BStart";
            para[6].SqlDbType = SqlDbType.DateTime;
            para[6].Value = _UpdateContract.BStart;

            para[7] = new SqlParameter();
            para[7].ParameterName = "BCycle";
            para[7].SqlDbType = SqlDbType.Int;
            para[7].Value = _UpdateContract.BCycle;

            para[8] = new SqlParameter();
            para[8].ParameterName = "BAmt";
            para[8].SqlDbType = SqlDbType.Money;
            para[8].Value = _UpdateContract.BAMT;

            para[9] = new SqlParameter();
            para[9].ParameterName = "SStart";
            para[9].SqlDbType = SqlDbType.DateTime;
            para[9].Value = _UpdateContract.SStart;

            para[10] = new SqlParameter();
            para[10].ParameterName = "Cycle";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = _UpdateContract.Cycle;

            para[11] = new SqlParameter();
            para[11].ParameterName = "SWE";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = _UpdateContract.SWE;

            para[12] = new SqlParameter();
            para[12].ParameterName = "stime";
            para[12].SqlDbType = SqlDbType.DateTime;
            para[12].Value = _UpdateContract.STime;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Sday";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = _UpdateContract.Sday;

            para[14] = new SqlParameter();
            para[14].ParameterName = "SDate";
            para[14].SqlDbType = SqlDbType.Int;
            para[14].Value = _UpdateContract.Sdate;

            para[15] = new SqlParameter();
            para[15].ParameterName = "ElevJobData";
            para[15].SqlDbType = SqlDbType.Structured;
            para[15].Value = _UpdateContract.DtElevJob;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Route";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = _UpdateContract.Route;

            para[17] = new SqlParameter();
            para[17].ParameterName = "job";
            para[17].SqlDbType = SqlDbType.Int;
            para[17].Value = _UpdateContract.JobId;

            para[18] = new SqlParameter();
            para[18].ParameterName = "hours";
            para[18].SqlDbType = SqlDbType.Decimal;
            para[18].Value = _UpdateContract.Hours;

            para[19] = new SqlParameter();
            para[19].ParameterName = "fdesc";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = _UpdateContract.Description;

            para[20] = new SqlParameter();
            para[20].ParameterName = "ctype";
            para[20].SqlDbType = SqlDbType.VarChar;
            para[20].Value = _UpdateContract.Ctype;

            para[21] = new SqlParameter();
            para[21].ParameterName = "ExpirationDate";
            para[21].SqlDbType = SqlDbType.DateTime;
            if (_UpdateContract.ExpirationDate != System.DateTime.MinValue)
                para[21].Value = _UpdateContract.ExpirationDate;
            else
                para[21].Value = DBNull.Value;

            para[22] = new SqlParameter();
            para[22].ParameterName = "ExpirationFreq";
            para[22].SqlDbType = SqlDbType.SmallInt;
            para[22].Value = _UpdateContract.expirationfreq;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Expiration";
            para[23].SqlDbType = SqlDbType.SmallInt;
            para[23].Value = _UpdateContract.Expiration;

            para[24] = new SqlParameter();
            para[24].ParameterName = "ContractBill";
            para[24].SqlDbType = SqlDbType.SmallInt;
            para[24].Value = _UpdateContract.ContractBill;

            para[25] = new SqlParameter();
            para[25].ParameterName = "CustomerBill";
            para[25].SqlDbType = SqlDbType.TinyInt;
            para[25].Value = _UpdateContract.CustBilling;

            para[26] = new SqlParameter();
            para[26].ParameterName = "Central";
            para[26].SqlDbType = SqlDbType.Int;
            para[26].Value = _UpdateContract.Central;

            para[27] = new SqlParameter();
            para[27].ParameterName = "Chart";
            para[27].SqlDbType = SqlDbType.Int;
            para[27].Value = _UpdateContract.Chart;

            para[28] = new SqlParameter();
            para[28].ParameterName = "JobT";
            para[28].SqlDbType = SqlDbType.Int;
            para[28].Value = _UpdateContract.JobTID;

            para[29] = new SqlParameter();
            para[29].ParameterName = "CustomItems";
            para[29].SqlDbType = SqlDbType.Structured;
            para[29].Value = _UpdateContract.DtCustom;

            para[30] = new SqlParameter();
            para[30].ParameterName = "@EscalationCycle";
            para[30].SqlDbType = SqlDbType.Int;
            para[30].Value = _UpdateContract.EscalationCycle;

            para[31] = new SqlParameter();
            para[31].ParameterName = "@EscalationFactor";
            para[31].SqlDbType = SqlDbType.Decimal;
            para[31].Value = _UpdateContract.EscalationFactor;

            para[32] = new SqlParameter();
            para[32].ParameterName = "@EscalationLast";
            para[32].SqlDbType = SqlDbType.DateTime;
            if (_UpdateContract.EscalationLast != DateTime.MinValue)
                para[32].Value = _UpdateContract.EscalationLast;
            else
                para[32].Value = DBNull.Value;

            para[33] = new SqlParameter();
            para[33].ParameterName = "@EscalationType";
            para[33].SqlDbType = SqlDbType.Int;
            para[33].Value = _UpdateContract.EscalationType;

            para[34] = new SqlParameter();
            para[34].ParameterName = "BillRate";
            para[34].SqlDbType = SqlDbType.Decimal;
            para[34].Value = _UpdateContract.BillRate;

            para[35] = new SqlParameter();
            para[35].ParameterName = "RateOT";
            para[35].SqlDbType = SqlDbType.Decimal;
            para[35].Value = _UpdateContract.RateOT;

            para[36] = new SqlParameter();
            para[36].ParameterName = "RateNT";
            para[36].SqlDbType = SqlDbType.Decimal;
            para[36].Value = _UpdateContract.RateNT;

            para[37] = new SqlParameter();
            para[37].ParameterName = "RateDT";
            para[37].SqlDbType = SqlDbType.Decimal;
            para[37].Value = _UpdateContract.RateDT;

            para[38] = new SqlParameter();
            para[38].ParameterName = "RateTravel";
            para[38].SqlDbType = SqlDbType.Decimal;
            para[38].Value = _UpdateContract.RateTravel;

            para[39] = new SqlParameter();
            para[39].ParameterName = "Mileage";
            para[39].SqlDbType = SqlDbType.Decimal;
            para[39].Value = _UpdateContract.Mileage;

            para[40] = new SqlParameter();
            para[40].ParameterName = "PO";
            para[40].SqlDbType = SqlDbType.VarChar;
            para[40].Value = _UpdateContract.PO;

            para[41] = new SqlParameter();
            para[41].ParameterName = "SPHandle";
            para[41].SqlDbType = SqlDbType.SmallInt;
            para[41].Value = _UpdateContract.Handel;

            para[42] = new SqlParameter();
            para[42].ParameterName = "SPRemarks";
            para[42].SqlDbType = SqlDbType.VarChar;
            para[42].Value = _UpdateContract.Notes;

            para[43] = new SqlParameter();
            para[43].ParameterName = "RenewalNotes";
            para[43].SqlDbType = SqlDbType.Text;
            para[43].Value = _UpdateContract.RenewalNotes;

            para[44] = new SqlParameter();
            para[44].ParameterName = "IsRenewalNotes";
            para[44].SqlDbType = SqlDbType.SmallInt;
            para[44].Value = _UpdateContract.IsRenewalNotes;

            para[45] = new SqlParameter();
            para[45].ParameterName = "Detail";
            para[45].SqlDbType = SqlDbType.SmallInt;
            para[45].Value = _UpdateContract.Detail;

            para[46] = new SqlParameter();
            para[46].ParameterName = "@UpdatedBy";
            para[46].SqlDbType = SqlDbType.VarChar;
            para[46].Value = _UpdateContract.MOMUSer;
            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spUpdateContract", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteContract(Contracts objPropContracts)
        {
            //StringBuilder QueryText = new StringBuilder();
            //QueryText.Append("DELETE FROM Job \n");
            //QueryText.Append("WHERE  ID = " + objPropContracts.JobId);
            //QueryText.Append(" \n");
            //QueryText.Append("DELETE FROM Contract \n");
            //QueryText.Append("WHERE  Job = " + objPropContracts.JobId);
            //QueryText.Append(" \n");
            //QueryText.Append("DELETE FROM tblJoinElevJob \n");
            //QueryText.Append("WHERE  Job = " + objPropContracts.JobId);

            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, "Spdeletecontract", objPropContracts.JobId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void DeleteContract(DeleteContractParam _DeleteContract, string ConnectionString)
        {
            //StringBuilder QueryText = new StringBuilder();
            //QueryText.Append("DELETE FROM Job \n");
            //QueryText.Append("WHERE  ID = " + objPropContracts.JobId);
            //QueryText.Append(" \n");
            //QueryText.Append("DELETE FROM Contract \n");
            //QueryText.Append("WHERE  Job = " + objPropContracts.JobId);
            //QueryText.Append(" \n");
            //QueryText.Append("DELETE FROM tblJoinElevJob \n");
            //QueryText.Append("WHERE  Job = " + objPropContracts.JobId);

            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "Spdeletecontract", _DeleteContract.JobId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet AddRecurringTickets(Contracts objPropContracts)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, "spAddRecurringTickets", objPropContracts.Loc, objPropContracts.Remarks, objPropContracts.PerContract, objPropContracts.ContractRemarks, objPropContracts.Owner, objPropContracts.Route, objPropContracts.StartDt, objPropContracts.EndDt, objPropContracts.OnDemand, objPropContracts.FlagEN, objPropContracts.UserID, objPropContracts.StateVal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLastProcessDate(Contracts objPropContracts)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, "select top 1  custom19,Custom16,LastProcessed from job order by CONVERT (datetime,custom19) desc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetInvoiceLastProcessDate(Contracts objPropContracts)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, "select top 1  custom17,custom15 from job order by CONVERT (datetime,custom17) desc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateRecurringTickets(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[5];
            para[0] = new SqlParameter();
            para[0].ParameterName = "RecurringTicket";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = objPropContracts.DtRecContr;

            para[1] = new SqlParameter();
            para[1].ParameterName = "RemarksOpt";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropContracts.Remarks;

            para[2] = new SqlParameter();
            para[2].ParameterName = "JobRemarksOpt";
            para[2].SqlDbType = SqlDbType.Int;
            para[2].Value = objPropContracts.ContractRemarks;

            para[3] = new SqlParameter();
            para[3].ParameterName = "ProcessPeriod";
            para[3].SqlDbType = SqlDbType.VarChar;
            para[3].Value = objPropContracts.ProcessPeriod;

            para[4] = new SqlParameter();
            para[4].ParameterName = "LastProcessedBy";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropContracts.lastUpdatedby;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spCreateRecurringTickets", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet CreateRecurringInvoices(Contracts objPropContracts, int IncludeContractRemarks)
        {
            SqlParameter[] para = new SqlParameter[10];
            para[0] = new SqlParameter();
            para[0].ParameterName = "RecurringInvoice";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = objPropContracts.DtRecContr;

            para[1] = new SqlParameter();
            para[1].ParameterName = "InvoiceDate";
            para[1].SqlDbType = SqlDbType.DateTime;
            para[1].Value = objPropContracts.Date;

            para[2] = new SqlParameter();
            para[2].ParameterName = "PayTerms";
            para[2].SqlDbType = SqlDbType.Int;
            para[2].Value = objPropContracts.PaymentTerms;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Notes";
            para[3].SqlDbType = SqlDbType.VarChar;
            para[3].Value = objPropContracts.Remarks;

            para[4] = new SqlParameter();
            para[4].ParameterName = "ParaStax";
            para[4].SqlDbType = SqlDbType.Int;
            para[4].Value = objPropContracts.Taxable;

            para[5] = new SqlParameter();
            para[5].ParameterName = "ProcessPeriod";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropContracts.ProcessPeriod;

            para[6] = new SqlParameter();
            para[6].ParameterName = "cfUser";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropContracts.Fuser;

            para[7] = new SqlParameter();
            para[7].ParameterName = "PostingDate";
            para[7].SqlDbType = SqlDbType.DateTime;
            para[7].Value = objPropContracts.PostDate;

            para[8] = new SqlParameter();
            para[8].ParameterName = "DueDate";
            para[8].SqlDbType = SqlDbType.DateTime;
            para[8].Value = objPropContracts.DueDate;

            para[9] = new SqlParameter();
            para[9].ParameterName = "IncludeContractRemarks";
            para[9].SqlDbType = SqlDbType.Int;
            para[9].Value = IncludeContractRemarks;


            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spCreateRecurringInvoices", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetBillingFieldByLoc(Contracts objPropContracts)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, "select Billing from loc where Loc=" + objPropContracts.Loc + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CreateInvoice(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[32];
            para[0] = new SqlParameter();
            para[0].ParameterName = "Invoice";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = objPropContracts.DtRecContr;

            para[1] = new SqlParameter();
            para[1].ParameterName = "fdate";
            para[1].SqlDbType = SqlDbType.DateTime;
            para[1].Value = objPropContracts.Date;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Fdesc";
            para[2].SqlDbType = SqlDbType.Text;
            para[2].Value = objPropContracts.Remarks;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Amount";
            para[3].SqlDbType = SqlDbType.Money;
            para[3].Value = objPropContracts.Amount;

            para[4] = new SqlParameter();
            para[4].ParameterName = "stax";
            para[4].SqlDbType = SqlDbType.Money;
            para[4].Value = objPropContracts.Staxtotal;

            para[5] = new SqlParameter();
            para[5].ParameterName = "total";
            para[5].SqlDbType = SqlDbType.Money;
            para[5].Value = objPropContracts.Total;

            para[6] = new SqlParameter();
            para[6].ParameterName = "taxRegion";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropContracts.TaxRegion;

            para[7] = new SqlParameter();
            para[7].ParameterName = "taxrate";
            para[7].SqlDbType = SqlDbType.Money;
            para[7].Value = objPropContracts.Taxrate;

            para[8] = new SqlParameter();
            para[8].ParameterName = "Taxfactor";
            para[8].SqlDbType = SqlDbType.Money;
            para[8].Value = objPropContracts.Taxfactor;

            para[9] = new SqlParameter();
            para[9].ParameterName = "taxable";
            para[9].SqlDbType = SqlDbType.Money;
            para[9].Value = objPropContracts.Taxable;

            para[10] = new SqlParameter();
            para[10].ParameterName = "type";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropContracts.Type;

            para[11] = new SqlParameter();
            para[11].ParameterName = "job";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropContracts.JobId;

            para[12] = new SqlParameter();
            para[12].ParameterName = "loc";
            para[12].SqlDbType = SqlDbType.Int;
            para[12].Value = objPropContracts.Loc;

            para[13] = new SqlParameter();
            para[13].ParameterName = "terms";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropContracts.Terms;

            para[14] = new SqlParameter();
            para[14].ParameterName = "po";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropContracts.PO;

            para[15] = new SqlParameter();
            para[15].ParameterName = "status";
            para[15].SqlDbType = SqlDbType.Int;
            para[15].Value = objPropContracts.Status;

            //para[16] = new SqlParameter();
            //para[16].ParameterName = "batch";
            //para[16].SqlDbType = SqlDbType.Int;
            //para[16].Value = objPropContracts.Batch;

            para[16] = new SqlParameter();
            para[16].ParameterName = "remarks";
            para[16].SqlDbType = SqlDbType.Text;
            para[16].Value = objPropContracts.Remarks;

            para[17] = new SqlParameter();
            para[17].ParameterName = "gtax";
            para[17].SqlDbType = SqlDbType.Money;
            para[17].Value = objPropContracts.Gtax;

            para[18] = new SqlParameter();
            para[18].ParameterName = "mech";
            para[18].SqlDbType = SqlDbType.Int;
            para[18].Value = objPropContracts.Mech;

            para[19] = new SqlParameter();
            para[19].ParameterName = "TaxRegion2";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropContracts.TaxRegion2;

            para[20] = new SqlParameter();
            para[20].ParameterName = "Taxrate2";
            para[20].SqlDbType = SqlDbType.Money;
            para[20].Value = objPropContracts.Taxrate2;

            para[21] = new SqlParameter();
            para[21].ParameterName = "BillTo";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = objPropContracts.BillTo;

            para[22] = new SqlParameter();
            para[22].ParameterName = "Idate";
            para[22].SqlDbType = SqlDbType.DateTime;
            para[22].Value = objPropContracts.Idate;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Fuser";
            para[23].SqlDbType = SqlDbType.VarChar;
            para[23].Value = objPropContracts.Fuser;

            para[24] = new SqlParameter();
            para[24].ParameterName = "staxI";
            para[24].SqlDbType = SqlDbType.Int;
            para[24].Value = objPropContracts.StaxI;

            para[25] = new SqlParameter();
            para[25].ParameterName = "invoiceID";
            para[25].SqlDbType = SqlDbType.VarChar;
            para[25].Value = objPropContracts.InvoiceIDCustom;

            para[26] = new SqlParameter();
            para[26].ParameterName = "TicketIDs";
            para[26].SqlDbType = SqlDbType.VarChar;
            para[26].Value = objPropContracts.Tickets;

            para[27] = new SqlParameter();
            para[27].ParameterName = "ddate";
            para[27].SqlDbType = SqlDbType.DateTime;
            para[27].Value = objPropContracts.DueDate;

            para[28] = new SqlParameter();
            para[28].ParameterName = "returnval";
            para[28].SqlDbType = SqlDbType.Int;
            para[28].Direction = ParameterDirection.ReturnValue;

            para[29] = new SqlParameter();
            para[29].ParameterName = "AssignedTo";
            para[29].SqlDbType = SqlDbType.Int;
            para[29].Value = objPropContracts.AssignedTo;

            para[30] = new SqlParameter();
            para[30].ParameterName = "IsRecurring";
            para[30].SqlDbType = SqlDbType.Int;
            para[30].Value = objPropContracts.IsRecurring;


            para[31] = new SqlParameter();
            para[31].ParameterName = "TaxType";
            para[31].SqlDbType = SqlDbType.Int;
            para[31].Value = objPropContracts.TaxType;
            try
            {
                //DataSet _ds =
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spCreateInvoice", para);
                return Convert.ToInt32(para[28].Value);
                //return Convert.ToInt32(_ds.Tables[0].Rows[0]["Ref"]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CreateOnlinePayment(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[5];
            para[0] = new SqlParameter();
            para[0].ParameterName = "clientId";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = objPropContracts.Owner;

            para[1] = new SqlParameter();
            para[1].ParameterName = "InvoiceId";
            para[1].SqlDbType = SqlDbType.NVarChar;
            para[1].Value = objPropContracts.InvoiceID;

            para[2] = new SqlParameter();
            para[2].ParameterName = "ConnectionString";
            para[2].SqlDbType = SqlDbType.NVarChar;
            para[2].Value = objPropContracts.ConnConfig;

            para[3] = new SqlParameter();
            para[3].ParameterName = "LocId";
            para[3].SqlDbType = SqlDbType.Int;
            para[3].Value = objPropContracts.Loc;

            para[4] = new SqlParameter();
            para[4].ParameterName = "DBName";
            para[4].SqlDbType = SqlDbType.NVarChar;
            para[4].Value = objPropContracts.FileName;
            try
            {
                int InvId = Convert.ToInt32(SqlHelper.ExecuteScalar(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spCreateOnlinePayment", para));
                return InvId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateQBInvoice(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[30];
            para[0] = new SqlParameter();
            para[0].ParameterName = "Invoice";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = objPropContracts.DtRecContr;

            para[1] = new SqlParameter();
            para[1].ParameterName = "fdate";
            para[1].SqlDbType = SqlDbType.DateTime;
            para[1].Value = objPropContracts.Date;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Fdesc";
            para[2].SqlDbType = SqlDbType.Text;
            para[2].Value = objPropContracts.Remarks;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Amount";
            para[3].SqlDbType = SqlDbType.Money;
            para[3].Value = objPropContracts.Amount;

            para[4] = new SqlParameter();
            para[4].ParameterName = "stax";
            para[4].SqlDbType = SqlDbType.Money;
            para[4].Value = objPropContracts.Staxtotal;

            para[5] = new SqlParameter();
            para[5].ParameterName = "total";
            para[5].SqlDbType = SqlDbType.Money;
            para[5].Value = objPropContracts.Total;

            para[6] = new SqlParameter();
            para[6].ParameterName = "taxRegion";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropContracts.TaxRegion;

            para[7] = new SqlParameter();
            para[7].ParameterName = "taxrate";
            para[7].SqlDbType = SqlDbType.Money;
            para[7].Value = objPropContracts.Taxrate;

            para[8] = new SqlParameter();
            para[8].ParameterName = "Taxfactor";
            para[8].SqlDbType = SqlDbType.Money;
            para[8].Value = objPropContracts.Taxfactor;

            para[9] = new SqlParameter();
            para[9].ParameterName = "taxable";
            para[9].SqlDbType = SqlDbType.Int;
            para[9].Value = objPropContracts.Taxable;

            para[10] = new SqlParameter();
            para[10].ParameterName = "job";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropContracts.JobId;

            para[11] = new SqlParameter();
            para[11].ParameterName = "po";
            para[11].SqlDbType = SqlDbType.VarChar;
            para[11].Value = objPropContracts.PO;

            para[12] = new SqlParameter();
            para[12].ParameterName = "status";
            para[12].SqlDbType = SqlDbType.Int;
            para[12].Value = objPropContracts.Status;

            para[13] = new SqlParameter();
            para[13].ParameterName = "batch";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropContracts.Batch;

            para[14] = new SqlParameter();
            para[14].ParameterName = "remarks";
            para[14].SqlDbType = SqlDbType.Text;
            para[14].Value = objPropContracts.Remarks;

            para[15] = new SqlParameter();
            para[15].ParameterName = "gtax";
            para[15].SqlDbType = SqlDbType.Money;
            para[15].Value = objPropContracts.Gtax;

            para[16] = new SqlParameter();
            para[16].ParameterName = "mech";
            para[16].SqlDbType = SqlDbType.Int;
            para[16].Value = objPropContracts.Mech;

            para[17] = new SqlParameter();
            para[17].ParameterName = "TaxRegion2";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropContracts.TaxRegion2;

            para[18] = new SqlParameter();
            para[18].ParameterName = "Taxrate2";
            para[18].SqlDbType = SqlDbType.Money;
            para[18].Value = objPropContracts.Taxrate2;

            para[19] = new SqlParameter();
            para[19].ParameterName = "BillTo";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropContracts.BillTo;

            para[20] = new SqlParameter();
            para[20].ParameterName = "Idate";
            para[20].SqlDbType = SqlDbType.DateTime;
            para[20].Value = objPropContracts.Idate;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Fuser";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = objPropContracts.Fuser;

            para[22] = new SqlParameter();
            para[22].ParameterName = "staxI";
            para[22].SqlDbType = SqlDbType.Int;
            para[22].Value = objPropContracts.StaxI;

            para[23] = new SqlParameter();
            para[23].ParameterName = "invoiceID";
            para[23].SqlDbType = SqlDbType.VarChar;
            para[23].Value = objPropContracts.InvoiceIDCustom;

            para[24] = new SqlParameter();
            para[24].ParameterName = "QBLOCID";
            para[24].SqlDbType = SqlDbType.VarChar;
            para[24].Value = objPropContracts.QBCustomerID;

            para[25] = new SqlParameter();
            para[25].ParameterName = "QBTERMSID";
            para[25].SqlDbType = SqlDbType.VarChar;
            para[25].Value = objPropContracts.QBTermsID;

            para[26] = new SqlParameter();
            para[26].ParameterName = "QBjobtypeID";
            para[26].SqlDbType = SqlDbType.VarChar;
            para[26].Value = objPropContracts.QBJobtypeID;

            para[27] = new SqlParameter();
            para[27].ParameterName = "QBInvoiceID";
            para[27].SqlDbType = SqlDbType.VarChar;
            para[27].Value = objPropContracts.QBInvID;

            para[28] = new SqlParameter();
            para[28].ParameterName = "TicketID";
            para[28].SqlDbType = SqlDbType.Int;
            para[28].Value = objPropContracts.TicketID;

            para[29] = new SqlParameter();
            para[29].ParameterName = "LastUpdateDate";
            para[29].SqlDbType = SqlDbType.DateTime;
            para[29].Value = objPropContracts.LastUpdateDate;

            try
            {
                SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spQBCreateInvoice", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateQBInvoiceMapping(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[30];
            para[0] = new SqlParameter();
            para[0].ParameterName = "Invoice";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = objPropContracts.DtRecContr;

            para[1] = new SqlParameter();
            para[1].ParameterName = "fdate";
            para[1].SqlDbType = SqlDbType.DateTime;
            para[1].Value = objPropContracts.Date;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Fdesc";
            para[2].SqlDbType = SqlDbType.Text;
            para[2].Value = objPropContracts.Remarks;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Amount";
            para[3].SqlDbType = SqlDbType.Money;
            para[3].Value = objPropContracts.Amount;

            para[4] = new SqlParameter();
            para[4].ParameterName = "stax";
            para[4].SqlDbType = SqlDbType.Money;
            para[4].Value = objPropContracts.Staxtotal;

            para[5] = new SqlParameter();
            para[5].ParameterName = "total";
            para[5].SqlDbType = SqlDbType.Money;
            para[5].Value = objPropContracts.Total;

            para[6] = new SqlParameter();
            para[6].ParameterName = "taxRegion";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropContracts.TaxRegion;

            para[7] = new SqlParameter();
            para[7].ParameterName = "taxrate";
            para[7].SqlDbType = SqlDbType.Money;
            para[7].Value = objPropContracts.Taxrate;

            para[8] = new SqlParameter();
            para[8].ParameterName = "Taxfactor";
            para[8].SqlDbType = SqlDbType.Money;
            para[8].Value = objPropContracts.Taxfactor;

            para[9] = new SqlParameter();
            para[9].ParameterName = "taxable";
            para[9].SqlDbType = SqlDbType.Int;
            para[9].Value = objPropContracts.Taxable;

            para[10] = new SqlParameter();
            para[10].ParameterName = "job";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropContracts.JobId;

            para[11] = new SqlParameter();
            para[11].ParameterName = "po";
            para[11].SqlDbType = SqlDbType.VarChar;
            para[11].Value = objPropContracts.PO;

            para[12] = new SqlParameter();
            para[12].ParameterName = "status";
            para[12].SqlDbType = SqlDbType.Int;
            para[12].Value = objPropContracts.Status;

            para[13] = new SqlParameter();
            para[13].ParameterName = "batch";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropContracts.Batch;

            para[14] = new SqlParameter();
            para[14].ParameterName = "remarks";
            para[14].SqlDbType = SqlDbType.Text;
            para[14].Value = objPropContracts.Remarks;

            para[15] = new SqlParameter();
            para[15].ParameterName = "gtax";
            para[15].SqlDbType = SqlDbType.Money;
            para[15].Value = objPropContracts.Gtax;

            para[16] = new SqlParameter();
            para[16].ParameterName = "mech";
            para[16].SqlDbType = SqlDbType.Int;
            para[16].Value = objPropContracts.Mech;

            para[17] = new SqlParameter();
            para[17].ParameterName = "TaxRegion2";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropContracts.TaxRegion2;

            para[18] = new SqlParameter();
            para[18].ParameterName = "Taxrate2";
            para[18].SqlDbType = SqlDbType.Money;
            para[18].Value = objPropContracts.Taxrate2;

            para[19] = new SqlParameter();
            para[19].ParameterName = "BillTo";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropContracts.BillTo;

            para[20] = new SqlParameter();
            para[20].ParameterName = "Idate";
            para[20].SqlDbType = SqlDbType.DateTime;
            para[20].Value = objPropContracts.Idate;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Fuser";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = objPropContracts.Fuser;

            para[22] = new SqlParameter();
            para[22].ParameterName = "staxI";
            para[22].SqlDbType = SqlDbType.Int;
            para[22].Value = objPropContracts.StaxI;

            para[23] = new SqlParameter();
            para[23].ParameterName = "invoiceID";
            para[23].SqlDbType = SqlDbType.VarChar;
            para[23].Value = objPropContracts.InvoiceIDCustom;

            para[24] = new SqlParameter();
            para[24].ParameterName = "QBLOCID";
            para[24].SqlDbType = SqlDbType.VarChar;
            para[24].Value = objPropContracts.QBCustomerID;

            para[25] = new SqlParameter();
            para[25].ParameterName = "QBTERMSID";
            para[25].SqlDbType = SqlDbType.VarChar;
            para[25].Value = objPropContracts.QBTermsID;

            para[26] = new SqlParameter();
            para[26].ParameterName = "QBjobtypeID";
            para[26].SqlDbType = SqlDbType.VarChar;
            para[26].Value = objPropContracts.QBJobtypeID;

            para[27] = new SqlParameter();
            para[27].ParameterName = "QBInvoiceID";
            para[27].SqlDbType = SqlDbType.VarChar;
            para[27].Value = objPropContracts.QBInvID;

            para[28] = new SqlParameter();
            para[28].ParameterName = "TicketID";
            para[28].SqlDbType = SqlDbType.Int;
            para[28].Value = objPropContracts.TicketID;

            para[29] = new SqlParameter();
            para[29].ParameterName = "LastUpdateDate";
            para[29].SqlDbType = SqlDbType.DateTime;
            para[29].Value = objPropContracts.LastUpdateDate;

            try
            {
                SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spQBCreateInvoiceMapping", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateInvoice(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[30];
            para[0] = new SqlParameter();
            para[0].ParameterName = "Invoice";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = objPropContracts.DtRecContr;

            para[1] = new SqlParameter();
            para[1].ParameterName = "fdate";
            para[1].SqlDbType = SqlDbType.DateTime;
            para[1].Value = objPropContracts.Date;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Fdesc";
            para[2].SqlDbType = SqlDbType.Text;
            para[2].Value = objPropContracts.Remarks;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Amount";
            para[3].SqlDbType = SqlDbType.Money;
            para[3].Value = objPropContracts.Amount;

            para[4] = new SqlParameter();
            para[4].ParameterName = "stax";
            para[4].SqlDbType = SqlDbType.Money;
            para[4].Value = objPropContracts.Staxtotal;

            para[5] = new SqlParameter();
            para[5].ParameterName = "total";
            para[5].SqlDbType = SqlDbType.Money;
            para[5].Value = objPropContracts.Total;

            para[6] = new SqlParameter();
            para[6].ParameterName = "taxRegion";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropContracts.TaxRegion;

            para[7] = new SqlParameter();
            para[7].ParameterName = "taxrate";
            para[7].SqlDbType = SqlDbType.Money;
            para[7].Value = objPropContracts.Taxrate;

            para[8] = new SqlParameter();
            para[8].ParameterName = "Taxfactor";
            para[8].SqlDbType = SqlDbType.Money;
            para[8].Value = objPropContracts.Taxfactor;

            para[9] = new SqlParameter();
            para[9].ParameterName = "taxable";
            para[9].SqlDbType = SqlDbType.Money;
            para[9].Value = objPropContracts.Taxable;

            para[10] = new SqlParameter();
            para[10].ParameterName = "type";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropContracts.Type;

            para[11] = new SqlParameter();
            para[11].ParameterName = "job";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropContracts.JobId;

            para[12] = new SqlParameter();
            para[12].ParameterName = "loc";
            para[12].SqlDbType = SqlDbType.Int;
            para[12].Value = objPropContracts.Loc;

            para[13] = new SqlParameter();
            para[13].ParameterName = "terms";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropContracts.Terms;

            para[14] = new SqlParameter();
            para[14].ParameterName = "po";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropContracts.PO;

            para[15] = new SqlParameter();
            para[15].ParameterName = "status";
            para[15].SqlDbType = SqlDbType.Int;
            para[15].Value = objPropContracts.Status;

            //para[16] = new SqlParameter();
            //para[16].ParameterName = "batch";
            //para[16].SqlDbType = SqlDbType.Int;
            //para[16].Value = objPropContracts.Batch;

            para[16] = new SqlParameter();
            para[16].ParameterName = "remarks";
            para[16].SqlDbType = SqlDbType.Text;
            para[16].Value = objPropContracts.Remarks;

            para[17] = new SqlParameter();
            para[17].ParameterName = "gtax";
            para[17].SqlDbType = SqlDbType.Money;
            para[17].Value = objPropContracts.Gtax;

            para[18] = new SqlParameter();
            para[18].ParameterName = "mech";
            para[18].SqlDbType = SqlDbType.Int;
            para[18].Value = objPropContracts.Mech;

            para[19] = new SqlParameter();
            para[19].ParameterName = "TaxRegion2";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropContracts.TaxRegion2;

            para[20] = new SqlParameter();
            para[20].ParameterName = "Taxrate2";
            para[20].SqlDbType = SqlDbType.Money;
            para[20].Value = objPropContracts.Taxrate2;

            para[21] = new SqlParameter();
            para[21].ParameterName = "BillTo";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = objPropContracts.BillTo;

            para[22] = new SqlParameter();
            para[22].ParameterName = "Idate";
            para[22].SqlDbType = SqlDbType.DateTime;
            para[22].Value = objPropContracts.Idate;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Fuser";
            para[23].SqlDbType = SqlDbType.VarChar;
            para[23].Value = objPropContracts.Fuser;

            para[24] = new SqlParameter();
            para[24].ParameterName = "staxI";
            para[24].SqlDbType = SqlDbType.Int;
            para[24].Value = objPropContracts.StaxI;

            para[25] = new SqlParameter();
            para[25].ParameterName = "invoiceID";
            para[25].SqlDbType = SqlDbType.VarChar;
            para[25].Value = objPropContracts.InvoiceIDCustom;

            para[26] = new SqlParameter();
            para[26].ParameterName = "InvID";
            para[26].SqlDbType = SqlDbType.Int;
            para[26].Value = objPropContracts.InvoiceID;

            para[27] = new SqlParameter();
            para[27].ParameterName = "ddate";
            para[27].SqlDbType = SqlDbType.DateTime;
            para[27].Value = objPropContracts.DueDate;

            para[28] = new SqlParameter();
            para[28].ParameterName = "AssignedTo";
            para[28].SqlDbType = SqlDbType.Int;
            para[28].Value = objPropContracts.AssignedTo;

            para[29] = new SqlParameter();
            para[29].ParameterName = "TaxType";
            para[29].SqlDbType = SqlDbType.Int;
            para[29].Value = objPropContracts.TaxType;

            //para[28] = new SqlParameter();
            //para[28].ParameterName = "TransID";
            //para[28].SqlDbType = SqlDbType.Int;
            //para[28].Value = objPropContracts.TransID;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spUpdateInvoice", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetRecurringInvoices(Contracts objPropContracts)
        {
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("SELECT Getdate()                           AS fdate, \n");
            //varname1.Append("       ''                                  AS fdesc, \n");
            //varname1.Append("       c.BAmt as amount, \n");
            //varname1.Append("       0                                   AS stax, \n");
            //varname1.Append("       0.00                                AS Taxregion, \n");
            //varname1.Append("       ( st.Rate+c.BAmt )                  AS total, \n");
            //varname1.Append("       st.Rate                             AS taxrate, \n");
            //varname1.Append("       100.00                              AS taxfactor, \n");
            //varname1.Append("       0                                   AS taxable, \n");
            //varname1.Append("       0                                   AS type, \n");
            //varname1.Append("       j.ID                                AS job, \n");
            //varname1.Append("       l.Loc, \n");
            //varname1.Append("       ''                                  AS terms, \n");
            //varname1.Append("       j.PO, \n");
            //varname1.Append("       --CASE l.Status      \n");
            //varname1.Append("       --  WHEN 0 THEN 'open'      \n");
            //varname1.Append("       --  WHEN 1 THEN 'paid'      \n");
            //varname1.Append("       --END                                 AS Status,      \n");
            //varname1.Append("       l.status, \n");
            //varname1.Append("       '0'                                 AS batch, \n");
            //varname1.Append("       'Recurring'                         AS remarks, \n");
            //varname1.Append("       0                                   AS gtax, \n");
            //varname1.Append("       j.Custom20                          AS worker, \n");
            //varname1.Append("       ''                                  AS Taxregion2, \n");
            //varname1.Append("       0.00                                AS taxrate2, \n");
            //varname1.Append("       ''                                  AS billto, \n");
            //varname1.Append("       Getdate()                           AS Idate, \n");
            //varname1.Append("       ''                                  AS fuser, \n");
            //varname1.Append("       0                                   AS acct, \n");
            //varname1.Append("       1.00                                AS Quan, \n");
            //varname1.Append("       0                                   AS price, \n");
            //varname1.Append("       1                                   AS jobitem, \n");
            //varname1.Append("       (SELECT measure \n");
            //varname1.Append("        FROM   Inv I \n");
            //varname1.Append("        WHERE  I.Name = 'Recurring')       AS measure, \n");
            //varname1.Append("       CASE c.BCycle \n");
            //varname1.Append("         WHEN 0 THEN 'Monthly recurring billing' \n");
            //varname1.Append("         WHEN 1 THEN 'Bi-Monthly recurring billing' \n");
            //varname1.Append("         WHEN 2 THEN 'Quarterly recurring billing' \n");
            //varname1.Append("         WHEN 3 THEN 'Semi-Anually recurring billing' \n");
            //varname1.Append("         WHEN 4 THEN 'Anually recurring billing' \n");
            //varname1.Append("       END                                 AS fdescI, \n");
            //varname1.Append("       CASE c.bcycle \n");
            //varname1.Append("         WHEN 0 THEN 'Monthly' \n");
            //varname1.Append("         WHEN 1 THEN 'Bi-Monthly' \n");
            //varname1.Append("         WHEN 2 THEN 'Quarterly' \n");
            //varname1.Append("         WHEN 3 THEN '3 Times/Year' \n");
            //varname1.Append("         WHEN 4 THEN 'Semi-Anually' \n");
            //varname1.Append("         WHEN 5 THEN 'Anually' \n");
            //varname1.Append("         WHEN 6 THEN 'Never' \n");
            //varname1.Append("       END                                 Frequency, \n");
            //varname1.Append("       --case when @stax=1 then l.STax               \n");
            //varname1.Append("       --else null end as  staxI,             \n");
            //varname1.Append("       st.Name, \n");
            //varname1.Append("       (SELECT TOP 1 name \n");
            //varname1.Append("        FROM   rol \n");
            //varname1.Append("        WHERE  id = (SELECT TOP 1 rol \n");
            //varname1.Append("                     FROM   owner \n");
            //varname1.Append("                     WHERE  id = l.Owner)) AS customername, \n");
            //varname1.Append("       (SELECT TOP 1 Tag \n");
            //varname1.Append("        FROM   Loc \n");
            //varname1.Append("        WHERE  Loc = l.Loc)                AS locid, \n");
            //varname1.Append("       (SELECT TOP 1 name \n");
            //varname1.Append("        FROM   rol \n");
            //varname1.Append("        WHERE  id = (SELECT TOP 1 rol \n");
            //varname1.Append("                     FROM   Loc \n");
            //varname1.Append("                     WHERE  Loc = l.Loc))  AS locname, \n");
            //varname1.Append("       (SELECT Name \n");
            //varname1.Append("        FROM   Route ro \n");
            //varname1.Append("        WHERE  ro.ID = j.Custom20)         AS dworker \n");
            //varname1.Append("FROM   Loc l \n");
            //varname1.Append("       LEFT OUTER JOIN STax st \n");
            //varname1.Append("                    ON l.STax = st.Name \n");
            //varname1.Append("       INNER JOIN Job j \n");
            //varname1.Append("               ON l.Loc = j.Loc \n");
            //varname1.Append("       INNER JOIN Contract c \n");
            //varname1.Append("               ON j.ID = c.Job ");
            //varname1.Append("WHERE               j.custom17 is null \n");
            //varname1.Append("and  c.BStart >= '"+ objPropContracts.StartDt+"' \n");
            //varname1.Append("       AND c.BStart <= '" + objPropContracts.EndDt + "'");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, "spAddRecurringInvoices", objPropContracts.Loc, objPropContracts.Owner, objPropContracts.Month, objPropContracts.Year, objPropContracts.Handel, objPropContracts.FlagEN, objPropContracts.UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CheckFilterHasCommaDelimited(List<RetainFilter> filters)
        {
            bool isFilterHasCommaDelimited = false;
            if (filters.Count > 0)
            {
                foreach (var items in filters)
                {
                    if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                    {
                        /// Int Filter
                        int FilterValue = 0; string[] filterArrayValue;
                        if (items.FilterColumn == "InvoiceRef")
                        {
                            filterArrayValue = items.FilterValue.ToString().Split(',');
                            foreach (var filtered in filterArrayValue)
                            {
                                if (int.TryParse(filtered, out FilterValue))
                                {

                                    isFilterHasCommaDelimited = true;
                                    break;

                                }
                            }
                        }
                    }
                }
            }
            return isFilterHasCommaDelimited;
        }

        public DataSet GetInvoices(Contracts objPropContracts, List<RetainFilter> filters, string fromDate, string toDate)
        {
            bool isFilterHasCommaDelimited = false;
            if (filters != null)
            {
                isFilterHasCommaDelimited = CheckFilterHasCommaDelimited(filters);
                if (!isFilterHasCommaDelimited)
                {
                    if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                    {
                        objPropContracts.StartDate = DateTime.Parse(fromDate);
                        objPropContracts.EndDate = DateTime.Parse(toDate);
                    }
                }
            }

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT i.Ref as Ref, \n");
            varname1.Append("CONVERT(varchar(50), rp.PaymentReceivedDate, 101) As   PaymentReceivedDate, \n");
            varname1.Append("te.SDesc, \n");
            varname1.Append("CONVERT(varchar(50), i.fDate, 101) As       fDate, \n");
            varname1.Append("l.Loc, \n");
            varname1.Append("l.Owner, \n");
            varname1.Append(" l.ID, \n");
            varname1.Append(" l.Tag, \n");
            varname1.Append(" i.fdesc, \n");
            varname1.Append(" i.Job, \n");
            varname1.Append(" isnull(l.Remarks,'') as locRemarks, \n");
            varname1.Append(" isnull(j.Remarks,'') as JobRemarks, \n");
            varname1.Append(" i.Amount, \n");
            varname1.Append(" i.STax+ISNULL(i.GTax,0) As STax, \n");
            varname1.Append("                i.Total, \n");
            varname1.Append("                isnull(i.status,0) as InvStatus,");
            varname1.Append("                i.custom1 as manualInv, \n");
            varname1.Append("                (CASE i.status \n");
            varname1.Append("                  WHEN 0 THEN 'Open' \n");
            varname1.Append("                  WHEN 1 THEN 'Paid' \n");
            varname1.Append("                  WHEN 2 THEN 'Voided' \n");
            varname1.Append("                  WHEN 4 THEN 'Marked as Pending' \n");
            varname1.Append("                  WHEN 5 THEN 'Paid by Credit Card' \n");
            varname1.Append("                  WHEN 3 THEN 'Partially Paid' \n");
            varname1.Append("                END + case isnull( ip.paid ,0) WHEN 1 THEN '/Paid by MOM' else '' end )                    AS status, \n");
            varname1.Append("                r.EN, \n");
            varname1.Append("                isnull(B.Name, '') As Company, \n");
            varname1.Append("                i.PO, \n");
            varname1.Append("                r.Name                  AS customername, \n");
            varname1.Append("               jt.Type              AS type, \n");
            //varname1.Append("                (SELECT Type \n");
            //varname1.Append("                 FROM   JobType jt \n");
            //varname1.Append("                 WHERE  jt.ID = i.Type) AS type, \n");
            varname1.Append("                  i.Batch, \n");
            varname1.Append("    case isnull( i.status ,0) WHEN 1 THEN 0 else convert(numeric(30,2), (isnull(i.total,0) - isnull(ip.balance,0)) ) end as Invbalance, ");
            varname1.Append("                 isnull(ar.Balance, 0) AS balance, ar.due as ddate, \n");
            varname1.Append("   DATEADD(dd, -1, DATEADD(wk,DATEDIFF(wk,0,ar.due),0)) as WeekDate, \n");
            varname1.Append("   isnull(wip.id,0) as WipInvoice, \n");
            varname1.Append("   isnull(j.Status,0) as JobStatus, \n");
            varname1.Append("   isnull(i.fUser,'') as CreatedBy, \n");
            varname1.Append("   isnull(i.IsRecurring, 0) AS IsRecurring \n");
            varname1.Append("FROM   Invoice i \n");
            varname1.Append("       LEFT OUTER JOIN Terr te \n");
            varname1.Append("               ON te.ID = i.AssignedTo \n");
            varname1.Append("       INNER JOIN Loc l \n");
            varname1.Append("               ON l.Loc = i.Loc \n");
            varname1.Append("       INNER JOIN owner o \n");
            varname1.Append("               ON o.id = l.owner \n");
            varname1.Append("       INNER JOIN rol r \n");
            varname1.Append("               ON o.rol = r.id \n");
            varname1.Append("            LEFT OUTER JOIN PaymentDetails p ON p.InvoiceID = i.Ref AND IsInvoice = 1 \n");
            varname1.Append("               LEFT OUTER join ReceivedPayment rp ON rp.ID = p.ReceivedPaymentID \n");
            varname1.Append("       LEFT OUTER JOIN Branch B on B.ID = r.EN \n");
            if (objPropContracts.EN == 1)
            {
                varname1.Append("       LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN \n");
            }
            varname1.Append("       LEFT OUTER JOIN tblInvoicePayment ip \n");
            varname1.Append("               ON i.ref = ip.ref \n");
            varname1.Append("       LEFT OUTER JOIN Job j ON i.Job=j.ID \n");
            varname1.Append("       LEFT JOIN OpenAR ar  \n");
            varname1.Append("               ON ar.Ref = i.Ref AND ar.Type = 0   \n");
            varname1.Append("       LEFT JOIN WIPHeader wip  \n");
            varname1.Append("               ON wip.InvoiceId=i.Ref  \n");

            varname1.Append("       LEFT JOIN JobType jt  \n");
            varname1.Append("               ON jt.ID=i.Type  \n");

            varname1.Append("       WHERE i.ref is not null \n");

            if (objPropContracts.EN == 1)
            {
                varname1.Append(" and UC.IsSel = 1 and UC.UserID =" + objPropContracts.UserID);
            }
            if (objPropContracts.SearchBy != string.Empty && objPropContracts.SearchBy != null)
            {
                if (objPropContracts.SearchBy == "i.fdate")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " between '" + objPropContracts.strSdate + "' and '" + objPropContracts.strEdate + "' \n");
                }
                else if (objPropContracts.SearchBy == "l.owner")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " = '" + objPropContracts.SearchValue + "' \n");
                }
                else if (objPropContracts.SearchBy == "i.loc")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " = '" + objPropContracts.SearchValue + "' \n");
                }
                else if (objPropContracts.SearchBy == "l.loc")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " = " + objPropContracts.SearchValue + " \n");
                }
                else if (objPropContracts.SearchBy == "i.ref")
                {
                    // varname1.Append("\n and i.ref  in (" + objPropContracts.SearchValue.Replace("=", "") + ")" + " \n");
                    varname1.Append(" and " + objPropContracts.SearchBy + objPropContracts.SearchValue + " \n");
                }
                else if (objPropContracts.SearchBy == "i.Type")
                {
                    if (objPropContracts.SearchValue != string.Empty && objPropContracts.SearchValue != null)
                    {
                        varname1.Append(" and " + objPropContracts.SearchBy + " = " + objPropContracts.SearchValue + " \n");
                    }
                    else
                    {
                        varname1.Append(" and " + objPropContracts.SearchBy + " = -1" + " \n");
                    }

                }
                else
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " like '%" + objPropContracts.SearchValue + "%' \n");
                }
            }

            if (!isFilterHasCommaDelimited)
            {
                if (objPropContracts.StartDate != System.DateTime.MinValue)
                {
                    varname1.Append(" and i.fdate >='" + objPropContracts.StartDate + "'\n");
                }
                if (objPropContracts.EndDate != System.DateTime.MinValue)
                {
                    varname1.Append(" and i.fdate <'" + objPropContracts.EndDate.AddDays(1) + "'");
                }
            }

            if (objPropContracts.CustID != 0)
            {
                varname1.Append(" and l.owner =" + objPropContracts.CustID + "");
            }
            if (objPropContracts.Loc != 0)
            {
                varname1.Append(" and l.loc =" + objPropContracts.Loc + "");
            }
            if (objPropContracts.jobid != 0)
            {
                varname1.Append(" and i.job =" + objPropContracts.jobid + "");
            }
            if (objPropContracts.Paid == 1)
            {
                varname1.Append(" and i.ref not in (SELECT isnull(InvoiceID,0) FROM  tblPaymentHistory where Approved='sent' and isnull(PayType,'')='ACH' )  \n");
                varname1.Append(" and isnull( ip.paid,0) = 0 and i.status = 0");
            }
            if (objPropContracts.RoleId != 0)
            {
                varname1.Append(" and isnull(l.roleid,0)= " + objPropContracts.RoleId);
            }
            if (!string.IsNullOrEmpty(objPropContracts.SearchAmtPaidUnpaid))
            {
                varname1.Append(" and i.Status in " + objPropContracts.SearchAmtPaidUnpaid);
                //if (objPropContracts.SearchAmtPaidUnpaid.Trim().ToLower() == "p")
                //{
                //    varname1.Append(" and isnull(ar.Balance, 0.00)<=0 and  p.InvoiceID is not null");
                //}
                //else if (objPropContracts.SearchAmtPaidUnpaid.Trim().ToLower() == "o")
                //{
                //    varname1.Append(" and i.Status != 1 AND i.Status != 2  ");
                //}
            }
            if (!string.IsNullOrEmpty(objPropContracts.SearchPrintMail))
            {
                if (objPropContracts.SearchPrintMail.Trim().ToLower() == "p")
                {
                    varname1.Append(" and l.PrintInvoice ='True' ");
                }
                else if (objPropContracts.SearchPrintMail.Trim().ToLower() == "m")
                {
                    varname1.Append(" and l.EmailInvoice ='True'    ");
                }
            }

            if (filters != null && filters.Count > 0)
            {
                foreach (var items in filters)
                {
                    if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                    {
                        /// Int Filter
                        int FilterValue = 0;

                        if (items.FilterColumn == "InvoiceRef")
                        {
                            StringBuilder filteredQuery = new StringBuilder();
                            string[] filterArrayValue = items.FilterValue.ToString().Split(',');
                            foreach (var filtered in filterArrayValue)
                            {
                                if (int.TryParse(filtered, out FilterValue))
                                {
                                    if (filteredQuery.Length == 0)
                                    {
                                        filteredQuery.Append(filtered);
                                    }
                                    else
                                    {
                                        filteredQuery.Append("," + filtered);
                                    }
                                }
                            }
                            varname1.Append("\n and i.ref  in (" + filteredQuery.ToString() + ")");
                        }

                        if (items.FilterColumn == "ManualInv")
                        {
                            varname1.Append("\n and i.custom1 like '%" + items.FilterValue.Replace("'", "''") + "%'");
                        }

                        if (items.FilterColumn == "ManualInv")
                        {
                            varname1.Append("\n and i.custom1 like '%" + items.FilterValue.Replace("'", "''") + "%'");
                        }

                        if (items.FilterColumn == "ID")
                        {
                            varname1.Append("\n and  l.ID like '%" + items.FilterValue.Replace("'", "''") + "%'");
                        }
                        if (items.FilterColumn == "Tag")
                        {
                            varname1.Append("\n and  l.Tag like '%" + items.FilterValue.Replace("'", "''") + "%'");
                        }

                        decimal amountFilter;
                        if (items.FilterColumn == "Amount" & Decimal.TryParse(items.FilterValue, out amountFilter))
                        {
                            varname1.Append("\n and  i.Amount = " + amountFilter);
                        }
                        amountFilter = 0;
                        if (items.FilterColumn == "SalesTax" & Decimal.TryParse(items.FilterValue, out amountFilter))
                        {
                            varname1.Append("\n and  (i.STax + ISNULL(i.GTax,0)) = " + amountFilter);
                        }
                        amountFilter = 0;
                        if (items.FilterColumn == "Total" & Decimal.TryParse(items.FilterValue, out amountFilter))
                        {
                            varname1.Append("\n and   i.Total = " + amountFilter);
                        }

                        if (items.FilterColumn == "Status")
                        {
                            StringBuilder filteredQuery = new StringBuilder();
                            String strQuery = items.FilterValue.ToString().ToLower();
                            strQuery = strQuery.Replace("open", "0").Replace("voided", "2").Replace("marked as pending", "4").Replace("paid by credit card", "5").Replace("partially paid", "3").Replace("paid", "1");

                            varname1.Append("\n and i.status  in (" + strQuery + ")");
                        }

                        if (items.FilterColumn == "PO")
                        {
                            varname1.Append("\n and  i.PO like '%" + items.FilterValue.Replace("'", "''") + "%'");
                        }

                        if (items.FilterColumn == "Job" & int.TryParse(items.FilterValue, out FilterValue))
                        {
                            varname1.Append("\n and   i.Job = " + FilterValue);
                        }

                        if (items.FilterColumn == "CustomerName")
                        {
                            varname1.Append("\n and  r.Name like '%" + items.FilterValue.Replace("'", "''") + "%'");
                        }
                        if (items.FilterColumn == "DType")
                        {
                            varname1.Append("\n and  jt.Type like '%" + items.FilterValue.Replace("'", "''") + "%'");
                        }
                        amountFilter = 0;
                        if (items.FilterColumn == "Balance" & Decimal.TryParse(items.FilterValue, out amountFilter))
                        {
                            varname1.Append("\n and   ar.Balance = " + amountFilter);
                        }

                        if (items.FilterColumn == "SDesc")
                        {
                            varname1.Append("\n and  te.SDesc like '%" + items.FilterValue.Replace("'", "''") + "%'");
                        }

                        if (items.FilterColumn == "LocRemarks")
                        {
                            varname1.Append("\n and  l.Remarks like '%" + items.FilterValue.Replace("'", "''") + "%'");
                        }

                        if (items.FilterColumn == "JobRemarks")
                        {
                            varname1.Append("\n and  j.Remarks like '%" + items.FilterValue.Replace("'", "''") + "%'");
                        }
                    }
                }
            }

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Maintenance Cancelled Report Data
        public DataSet GetMaintenanaceCancelled(Contracts objPropContracts, List<RetainFilter> filters, string fromDate, string toDate)
        {

            try
            {
                DateTime SDate = objPropContracts.StartDate.AddMilliseconds(-1);
                DateTime EDate = objPropContracts.EndDate.AddDays(1).AddMilliseconds(-1);


                SqlParameter stratDate = new SqlParameter();
                stratDate.ParameterName = "StartDate";
                stratDate.SqlDbType = SqlDbType.DateTime;
                stratDate.Value = SDate;

                SqlParameter endDate = new SqlParameter();
                endDate.ParameterName = "EndDate";
                endDate.SqlDbType = SqlDbType.DateTime;
                endDate.Value = EDate;

                return objPropContracts.Ds = SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetMaintenanceCancelled", stratDate, endDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //Get Check report Data
        public DataSet GetPayRollReportData(Contracts objPropContracts, List<RetainFilter> filters, string fromDate, string toDate)
        {

            try
            {
                string SDate = objPropContracts.StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string EDate = objPropContracts.EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT \n");
                varname1.Append("e.Name,\n");
                varname1.Append("p.fDate,\n");
                varname1.Append("p.Ref,\n");
                varname1.Append("p.TInc,\n");
                varname1.Append("p.TDed,\n");
                varname1.Append("p.Net,\n");
                varname1.Append("p.FIT, \n");
                varname1.Append("p.FICA, \n");
                varname1.Append("p.MEDI, \n");
                varname1.Append("p.SIT, \n");
                varname1.Append("p.Local, \n");
                varname1.Append("p.TOTher, \n");
                varname1.Append("p.ID, \n");
                varname1.Append("e.DDType, \n");
                varname1.Append("j.Type as category, \n");
                varname1.Append("s.fDesc as Supervisor \n");
                varname1.Append("FROM \n");
                varname1.Append("PRReg p \n");
                varname1.Append("INNER JOIN Emp e ON p.EmpID=e.ID \n");
                varname1.Append("Inner join JobType j on e.DDType = j.ID \n");
                varname1.Append("Inner join tblWork s on e.fWork=s.ID \n");

                varname1.Append("Where \n");
                varname1.Append("p.fDate between '" + SDate + "' AND '" + EDate + "' \n");
                if (objPropContracts.filterValue != null)
                {
                    varname1.Append("AND e.ID =" + objPropContracts.filterValue + " \n");
                }
                if (objPropContracts.SearchValue != null)
                {
                    varname1.Append("AND s.fDesc ='" + objPropContracts.SearchValue + "' \n");
                }
                if (objPropContracts.Ctype != null)
                {
                    varname1.Append("AND ( \n");
                    string[] Department = objPropContracts.Ctype.Split(',');
                    int i = 0;
                    foreach (var obj in Department)
                    {
                        if (i == 0)
                        {
                            varname1.Append("e.DDType =" + obj + " \n");
                        }
                        else
                        {
                            varname1.Append("OR e.DDType =" + obj + " \n");
                        }
                        i++;
                    }

                    varname1.Append(" ) \n");
                }
                varname1.Append("ORDER BY p.ID \n");
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Payroll Liability Report Data
        public DataSet GetPayrollLiabilityReportData(Contracts objPropContracts, List<RetainFilter> filters, string fromDate, string toDate)
        {

            try
            {
                string SDate = objPropContracts.StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string EDate = objPropContracts.EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                SqlParameter stratDate = new SqlParameter();
                stratDate.ParameterName = "StartDate";
                stratDate.SqlDbType = SqlDbType.DateTime;
                stratDate.Value = SDate;

                SqlParameter endDate = new SqlParameter();
                endDate.ParameterName = "EndDate";
                endDate.SqlDbType = SqlDbType.DateTime;
                endDate.Value = EDate;

                return objPropContracts.Ds = SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetPayrollLiability", stratDate, endDate);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Payroll Liability Min Date
        public DateTime GetPayrollLiabilityMinDate(Contracts objPropContracts)
        {
            return Convert.ToDateTime(SqlHelper.ExecuteScalar(objPropContracts.ConnConfig, CommandType.Text, "Select Min(PRReg.fDate) as FDate From PRReg "));
        }

        //Get Payroll Liability Max Date
        public DateTime GetPayrollLiabilityMaxDate(Contracts objPropContracts)
        {
            return Convert.ToDateTime(SqlHelper.ExecuteScalar(objPropContracts.ConnConfig, CommandType.Text, "Select Max(PRReg.fDate) as FDate From PRReg "));
        }

        //Get Check Report By Title Data
        public DataSet GetCheckReportByTitle(Contracts objPropContracts, List<RetainFilter> filters, string fromDate, string toDate)
        {

            try
            {
                string SDate = objPropContracts.StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string EDate = objPropContracts.EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT \n");
                varname1.Append("Emp.Name, \n");
                varname1.Append("PRReg.fDate,\n");
                varname1.Append("PRReg.Ref,\n");
                varname1.Append("PRReg.TInc,\n");
                varname1.Append("PRReg.TDed,\n");
                varname1.Append("PRReg.Net,\n");
                varname1.Append("PRReg.FIT, \n");
                varname1.Append("PRReg.FICA, \n");
                varname1.Append("PRReg.MEDI, \n");
                varname1.Append("PRReg.SIT, \n");
                varname1.Append("PRReg.Local, \n");
                varname1.Append("PRReg.TOTher, \n");
                varname1.Append("Emp.Title, \n");
                varname1.Append("PRReg.ID \n");
                varname1.Append("FROM \n");
                varname1.Append("PRReg PRReg \n");
                varname1.Append("INNER JOIN Emp Emp ON PRReg.EmpID=Emp.ID \n");

                varname1.Append("Where \n");
                varname1.Append("PRReg.fDate between '" + SDate + "' AND '" + EDate + "' \n");
                if (objPropContracts.Ctype != null)
                {
                    varname1.Append("AND ( \n");
                    string[] Title = objPropContracts.Ctype.Split(',');
                    int i = 0;
                    foreach (var obj in Title)
                    {

                        if (i == 0)
                        {
                            if (obj == "XYZ-Empty")
                            {
                                varname1.Append("Emp.Title ='" + "" + "' \n");
                            }
                            else if (obj == "ABC-Null")
                            {
                                varname1.Append("Emp.Title ='" + null + "' \n");
                            }
                            else
                            {
                                varname1.Append("Emp.Title ='" + obj + "' \n");
                            }
                        }
                        else
                        {
                            if (obj == "XYZ-Empty")
                            {
                                varname1.Append("OR Emp.Title ='" + "" + "' \n");
                            }
                            else if (obj == "ABC-Null")
                            {
                                varname1.Append("OR Emp.Title ='" + null + "' \n");
                            }
                            else
                            {
                                varname1.Append("OR Emp.Title ='" + obj + "' \n");
                            }
                        }
                        i++;
                    }

                    varname1.Append(" ) \n");
                }
                varname1.Append("ORDER BY Emp.Title, PRReg.ID \n");
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Comprehensive Report Data
        public DataSet GetComprehensiveReport(Contracts objPropContracts, List<RetainFilter> filters, string fromDate, string toDate)
        {

            try
            {
                string SDate = objPropContracts.StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string EDate = objPropContracts.EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT \n");
                varname1.Append("Emp.Name, \n");
                varname1.Append("Rol.Address,\n");
                varname1.Append("PRReg.fDate,\n");
                varname1.Append("PRReg.Ref,\n");
                varname1.Append("PRReg.TInc,\n");
                varname1.Append("PRReg.TDed,\n");
                varname1.Append("PRReg.Net, \n");
                varname1.Append("Rol.City, \n");
                varname1.Append("Rol.State, \n");
                varname1.Append("Rol.Zip, \n");
                varname1.Append("Emp.Title, \n");
                varname1.Append("PRReg.Reg, \n");
                varname1.Append("PRReg.OT, \n");
                varname1.Append("PRReg.DT, \n");
                varname1.Append("PRReg.TT, \n");
                varname1.Append("PRReg.Hol, \n");
                varname1.Append("PRReg.Vac, \n");
                varname1.Append("PRReg.Reimb, \n");
                varname1.Append("PRReg.Mile, \n");
                varname1.Append("PRReg.Bonus, \n");
                varname1.Append("PRReg.FIT, \n");
                varname1.Append("PRReg.FICA, \n");
                varname1.Append("PRReg.MEDI, \n");
                varname1.Append("PRReg.SIT, \n");
                varname1.Append("PRReg.Local, \n");
                varname1.Append("PRReg.TOTher, \n");
                varname1.Append("PRReg.FUTA, \n");
                varname1.Append("PRReg.NT, \n");
                varname1.Append("PRReg.Zone, \n");
                varname1.Append("PRReg.CompMedi, \n");
                varname1.Append("PRReg.ID  \n");
                varname1.Append("FROM \n");
                varname1.Append("(PRReg PRReg \n");
                varname1.Append("INNER JOIN Emp Emp ON PRReg.EmpID=Emp.ID) \n");
                varname1.Append("INNER JOIN Rol Rol ON Emp.Rol=Rol.ID \n");

                varname1.Append("Where \n");
                varname1.Append("PRReg.fDate between '" + SDate + "' AND '" + EDate + "' \n");
                if (objPropContracts.Ctype != null)
                {
                    varname1.Append("AND ( \n");
                    string[] Title = objPropContracts.Ctype.Split(',');
                    int i = 0;
                    foreach (var obj in Title)
                    {

                        if (i == 0)
                        {
                            if (obj == "XYZ-Empty")
                            {
                                varname1.Append("Emp.Title ='" + "" + "' \n");
                            }
                            else if (obj == "ABC-Null")
                            {
                                varname1.Append("Emp.Title ='" + null + "' \n");
                            }
                            else
                            {
                                varname1.Append("Emp.Title ='" + obj + "' \n");
                            }
                        }
                        else
                        {
                            if (obj == "XYZ-Empty")
                            {
                                varname1.Append("OR Emp.Title ='" + "" + "' \n");
                            }
                            else if (obj == "ABC-Null")
                            {
                                varname1.Append("OR Emp.Title ='" + null + "' \n");
                            }
                            else
                            {
                                varname1.Append("OR Emp.Title ='" + obj + "' \n");
                            }
                        }
                        i++;
                    }

                    varname1.Append(" ) \n");
                }
                varname1.Append(" ORDER BY Emp.Title, Emp.Name, PRReg.ID \n");
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetProjectARInvoices(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT i.Ref, \n");
            varname1.Append("                te.SDesc, \n");
            varname1.Append("        CONVERT(varchar(50), i.fDate, 101) As       fDate, \n");
            varname1.Append("                l.Loc, \n");
            varname1.Append("                l.ID, \n");
            varname1.Append("                l.Tag, \n");
            varname1.Append("                i.fdesc, \n");
            varname1.Append("                i.Job, \n");
            varname1.Append("                isnull(l.Remarks,'') as locRemarks, \n");
            varname1.Append("  	             isnull(j.Remarks,'') as JobRemarks, \n");
            varname1.Append("                i.Amount, \n");
            varname1.Append("                i.STax+ISNULL(i.GTax,0) As STax, \n");
            varname1.Append("                i.Total, \n");
            varname1.Append("                isnull(i.status,0) as InvStatus,");
            varname1.Append("                i.custom1 as manualInv, \n");
            varname1.Append("                (CASE i.status \n");
            varname1.Append("                  WHEN 0 THEN 'Open' \n");
            varname1.Append("                  WHEN 1 THEN 'Paid' \n");
            varname1.Append("                  WHEN 2 THEN 'Voided' \n");
            varname1.Append("                  WHEN 4 THEN 'Marked as Pending' \n");
            varname1.Append("                  WHEN 5 THEN 'Paid by Credit Card' \n");
            varname1.Append("                  WHEN 3 THEN 'Partially Paid' \n");
            varname1.Append("                END + case isnull( ip.paid ,0) WHEN 1 THEN '/Paid by MOM' else '' end )                    AS status, \n");
            varname1.Append("                r.EN, \n");
            varname1.Append("                isnull(B.Name, '') As Company, \n");
            varname1.Append("                i.PO, \n");
            varname1.Append("                r.Name                  AS customername, \n");
            varname1.Append("                (SELECT Type \n");
            varname1.Append("                 FROM   JobType jt \n");
            varname1.Append("                 WHERE  jt.ID = i.Type) AS type, \n");
            varname1.Append("                  i.Batch, \n");
            varname1.Append("    case isnull( i.status ,0) WHEN 1 THEN 0 else convert(numeric(30,2), (isnull(i.total,0) - isnull(ip.balance,0)) ) end as Invbalance, ");
            varname1.Append("                 isnull(ar.Balance, 0) AS balance, ar.due as ddate \n");
            varname1.Append("                 ,isnull(i.STax, 0) AS StaxAmount, isnull(i.GTax, 0) AS GSTAmount \n");
            //if(objPropContracts.isTS==0)
            varname1.Append("FROM   Invoice i \n");
            //else
            //    varname1.Append("FROM   MS_Invoice i \n");
            varname1.Append("       LEFT OUTER JOIN Terr te \n");
            varname1.Append("               ON te.ID = i.AssignedTo \n");
            varname1.Append("       INNER JOIN Loc l \n");
            varname1.Append("               ON l.Loc = i.Loc \n");
            varname1.Append("       INNER JOIN owner o \n");
            varname1.Append("               ON o.id = l.owner \n");
            varname1.Append("       INNER JOIN rol r \n");
            varname1.Append("               ON o.rol = r.id \n");
            varname1.Append("       LEFT OUTER JOIN Branch B on B.ID = r.EN \n");
            if (objPropContracts.EN == 1)
                varname1.Append("       LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN \n");
            varname1.Append("       LEFT OUTER JOIN tblInvoicePayment ip \n");
            varname1.Append("               ON i.ref = ip.ref \n");
            varname1.Append("       LEFT OUTER JOIN Job j ON i.Job=j.ID \n");
            varname1.Append("       LEFT JOIN OpenAR ar  \n");
            varname1.Append("               ON ar.Ref = i.Ref AND ar.Type = 0   \n");
            varname1.Append("       WHERE i.ref is not null \n");
            if (objPropContracts.EN == 1)
            {
                varname1.Append(" and UC.IsSel = 1 and UC.UserID =" + objPropContracts.UserID);
                // varname1.Append(" and UC.UserID =" + objPropContracts.UserID);
            }
            if (objPropContracts.SearchBy != string.Empty && objPropContracts.SearchBy != null)
            {
                if (objPropContracts.SearchBy == "i.fdate")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " = '" + objPropContracts.SearchValue + "' \n");
                }
                else if (objPropContracts.SearchBy == "l.owner")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " = '" + objPropContracts.SearchValue + "' \n");
                }
                else if (objPropContracts.SearchBy == "i.loc")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " = '" + objPropContracts.SearchValue + "' \n");
                }
                else if (objPropContracts.SearchBy == "l.loc")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " = " + objPropContracts.SearchValue + " \n");
                }
                else if (objPropContracts.SearchBy == "i.ref")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + objPropContracts.SearchValue + " \n");
                }
                else if (objPropContracts.SearchBy == "i.Status")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " = " + objPropContracts.SearchValue + " \n");
                }
                else
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " like '" + objPropContracts.SearchValue + "%' \n");
                }
            }

            if (objPropContracts.StartDate != System.DateTime.MinValue)
            {
                varname1.Append(" and i.fdate >='" + objPropContracts.StartDate + "'\n");
            }
            if (objPropContracts.EndDate != System.DateTime.MinValue)
            {
                varname1.Append(" and i.fdate <'" + objPropContracts.EndDate.AddDays(1) + "'");
            }

            //if (objPropContracts.StartDate != System.DateTime.MinValue && objPropContracts.EndDate != System.DateTime.MinValue)
            //{
            //   // varname1.Append
            //}



            if (objPropContracts.CustID != 0)
            {
                varname1.Append(" and l.owner =" + objPropContracts.CustID + "");
            }
            if (objPropContracts.Loc != 0)
            {
                varname1.Append(" and l.loc =" + objPropContracts.Loc + "");
            }
            if (objPropContracts.jobid != 0)
            {
                varname1.Append(" and i.job =" + objPropContracts.jobid + "");
            }
            if (objPropContracts.Paid == 1)
            {
                //varname1.Append(" and i.status = 0");
                //varname1.Append(" and isnull(i.paid,0) = 0 and i.status = 0");
                varname1.Append(" and i.ref not in (SELECT isnull(InvoiceID,0) FROM  tblPaymentHistory where Approved='sent' and isnull(PayType,'')='ACH' )  \n");

                varname1.Append(" and isnull( ip.paid,0) = 0 and i.status = 0");
            }
            if (objPropContracts.RoleId != 0)
                varname1.Append(" and isnull(l.roleid,0)= " + objPropContracts.RoleId);
            if (!string.IsNullOrEmpty(objPropContracts.SearchAmtPaidUnpaid))
            {
                if (objPropContracts.SearchAmtPaidUnpaid.Trim().ToLower() == "p")
                {
                    varname1.Append(" and isnull(ar.Balance, 0.00)<=0");
                }
                else if (objPropContracts.SearchAmtPaidUnpaid.Trim().ToLower() == "o")
                {
                    //varname1.Append(" and i.Status != 1 AND i.Status != 2 and  ar.Balance <> 0       ");
                    varname1.Append(" and i.Status != 1 AND i.Status != 2  ");
                }
            }
            if (!string.IsNullOrEmpty(objPropContracts.SearchPrintMail))
            {
                if (objPropContracts.SearchPrintMail.Trim().ToLower() == "p")
                {
                    varname1.Append(" and l.PrintInvoice ='True' ");
                }
                else if (objPropContracts.SearchPrintMail.Trim().ToLower() == "m")
                {
                    varname1.Append(" and l.EmailInvoice ='True'    ");
                }
            }

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvoicesReceivePayment(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT i.Ref, \n");
            varname1.Append("                i.fDate, \n");
            varname1.Append("                l.Loc, \n");
            varname1.Append("                l.ID, \n");
            varname1.Append("                l.Tag, \n");
            varname1.Append("                i.fdesc, \n");
            varname1.Append("                isnull(l.Remarks,'') as locRemarks, \n");
            varname1.Append("  	             isnull(j.Remarks,'') as JobRemarks, \n");
            varname1.Append("                i.Amount, \n");
            varname1.Append("                i.STax+ISNULL(i.GTax,0) As STax, \n");
            varname1.Append("                i.Total, \n");
            varname1.Append("                isnull(i.status,0) as InvStatus,");
            varname1.Append("                i.custom1 as manualInv, \n");
            varname1.Append("                (CASE i.status \n");
            varname1.Append("                  WHEN 0 THEN 'Open' \n");
            varname1.Append("                  WHEN 1 THEN 'Paid' \n");
            varname1.Append("                  WHEN 2 THEN 'Voided' \n");
            varname1.Append("                  WHEN 4 THEN 'Marked as Pending' \n");
            varname1.Append("                  WHEN 5 THEN 'Paid by Credit Card' \n");
            varname1.Append("                  WHEN 3 THEN 'Partially Paid' \n");
            varname1.Append("                END + case isnull( ip.paid ,0) WHEN 1 THEN '/Paid by MOM' else '' end )                    AS status, \n");
            varname1.Append("                r.EN, \n");
            varname1.Append("                isnull(B.Name, '') As Company, \n");
            varname1.Append("                i.PO, \n");
            varname1.Append("                r.Name                  AS customername, \n");
            varname1.Append("                (SELECT Type \n");
            varname1.Append("                 FROM   JobType jt \n");
            varname1.Append("                 WHERE  jt.ID = i.Type) AS type, \n");
            varname1.Append("                  i.Batch, \n");
            varname1.Append("    case isnull( i.status ,0) WHEN 1 THEN 0 else convert(numeric(30,2), (isnull(i.total,0) - isnull(ip.balance,0)) ) end as Invbalance, ");
            varname1.Append("                 isnull(ar.Balance, 0) AS balance, ar.due as ddate, \n");
            varname1.Append("         rp.Amount as CheckAmount,rp.PaymentReceivedDate,  ");
            varname1.Append("                rp.PaymentMethod,rp.CheckNumber,rp.AmountDue,rp.fDesc \n");


            //if(objPropContracts.isTS==0)
            varname1.Append("FROM   Invoice i \n");
            //else
            //    varname1.Append("FROM   MS_Invoice i \n");
            varname1.Append("       INNER JOIN Loc l \n");
            varname1.Append("               ON l.Loc = i.Loc \n");
            varname1.Append("       INNER JOIN owner o \n");
            varname1.Append("               ON o.id = l.owner \n");
            varname1.Append("       INNER JOIN rol r \n");
            varname1.Append("               ON o.rol = r.id \n");
            varname1.Append("            LEFT OUTER JOIN PaymentDetails p ON p.InvoiceID = i.Ref AND IsInvoice = 1 \n");
            varname1.Append("               LEFT OUTER join ReceivedPayment rp ON rp.ID = p.ReceivedPaymentID \n");
            varname1.Append("       LEFT OUTER JOIN Branch B on B.ID = r.EN \n");
            if (objPropContracts.EN == 1)
                varname1.Append("       LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN \n");
            varname1.Append("       LEFT OUTER JOIN tblInvoicePayment ip \n");
            varname1.Append("               ON i.ref = ip.ref \n");
            varname1.Append("       LEFT OUTER JOIN Job j ON i.Job=j.ID \n");
            varname1.Append("       LEFT JOIN OpenAR ar  \n");
            varname1.Append("               ON ar.Ref = i.Ref AND ar.Type = 0   \n");
            varname1.Append("       WHERE i.ref is not null \n");
            if (objPropContracts.EN == 1)
            {
                varname1.Append(" and UC.IsSel = 1 and UC.UserID =" + objPropContracts.UserID);
            }
            if (objPropContracts.SearchBy != string.Empty && objPropContracts.SearchBy != null)
            {
                if (objPropContracts.SearchBy == "i.fdate")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " = '" + objPropContracts.SearchValue + "' \n");
                }
                else if (objPropContracts.SearchBy == "l.owner")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " = '" + objPropContracts.SearchValue + "' \n");
                }
                else if (objPropContracts.SearchBy == "i.loc")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " = '" + objPropContracts.SearchValue + "' \n");
                }
                else if (objPropContracts.SearchBy == "l.loc")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " = " + objPropContracts.SearchValue + " \n");
                }
                else if (objPropContracts.SearchBy == "i.ref")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + objPropContracts.SearchValue + " \n");
                }
                else
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " like '" + objPropContracts.SearchValue + "%' \n");
                }
            }
            if (objPropContracts.StartDate != System.DateTime.MinValue)
            {
                varname1.Append(" and i.fdate >='" + objPropContracts.StartDate.ToString("MM/dd/yyyy") + "'\n");
            }
            if (objPropContracts.EndDate != System.DateTime.MinValue)
            {
                varname1.Append(" and i.fdate <'" + objPropContracts.EndDate.AddDays(1).ToString("MM/dd/yyyy") + "'");
            }
            if (objPropContracts.CustID != 0)
            {
                varname1.Append(" and l.owner =" + objPropContracts.CustID + "");
            }
            if (objPropContracts.Loc != 0)
            {
                varname1.Append(" and l.loc =" + objPropContracts.Loc + "");
            }
            if (objPropContracts.jobid != 0)
            {
                varname1.Append(" and i.job =" + objPropContracts.jobid + "");
            }
            if (objPropContracts.Paid == 1)
            {
                //varname1.Append(" and i.status = 0");
                //varname1.Append(" and isnull(i.paid,0) = 0 and i.status = 0");
                varname1.Append(" and i.ref not in (SELECT isnull(InvoiceID,0) FROM  tblPaymentHistory where Approved='sent' and isnull(PayType,'')='ACH' )  \n");

                varname1.Append(" and isnull( ip.paid,0) = 0 and i.status = 0");
            }
            if (objPropContracts.RoleId != 0)
                varname1.Append(" and isnull(l.roleid,0)= " + objPropContracts.RoleId);
            if (!string.IsNullOrEmpty(objPropContracts.SearchAmtPaidUnpaid))
            {
                if (objPropContracts.SearchAmtPaidUnpaid.Trim().ToLower() == "p")
                {
                    varname1.Append(" and isnull(ar.Balance, 0.00)<=0");
                }
                else if (objPropContracts.SearchAmtPaidUnpaid.Trim().ToLower() == "o")
                {
                    varname1.Append(" and i.Status != 1 AND i.Status != 2 and  ar.Balance <> 0       ");
                }
            }
            if (!string.IsNullOrEmpty(objPropContracts.SearchPrintMail))
            {
                if (objPropContracts.SearchPrintMail.Trim().ToLower() == "p")
                {
                    varname1.Append(" and l.PrintInvoice ='True' ");
                }
                else if (objPropContracts.SearchPrintMail.Trim().ToLower() == "m")
                {
                    varname1.Append(" and l.EmailInvoice !='True'    ");
                }
            }

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAPInvoices(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT  jobi.Ref,pj.ID,PJ.fDate,PJ.Ref,PJ.fDesc,PJ.Amount,PJ.Vendor,PJ.Status,PJ.Batch,PJ.Terms,PJ.PO,PJ.TRID	,PJ.Spec,PJ.IDate,PJ.UseTax,PJ.Disc,PJ.Custom1	, PJ.Custom2, PJ.ReqBy, PJ.VoidR, PJ.ReceivePO,");
            varname1.Append(" Sum(JobI.Amount) As PAmount \n");
            varname1.Append("FROM   JobI \n");
            varname1.Append(" LEFT OUTER JOIN Trans \n");
            varname1.Append(" ON JobI.TransID = Trans.ID \n");
            varname1.Append(" LEFT OUTER JOIN PJ \n");
            varname1.Append(" ON Trans.Batch = PJ.Batch \n");
            varname1.Append("WHERE  JobI.Job = " + objPropContracts.jobid + " \n");
            varname1.Append(" And  JobI.Type in (1,2)\n");
            varname1.Append(" AND Trans.Type in (41) \n");
            if (objPropContracts.StartDate != System.DateTime.MinValue) varname1.Append(" and JobI.fDate >='" + objPropContracts.StartDate + "'\n");
            if (objPropContracts.EndDate != System.DateTime.MinValue) varname1.Append(" and JobI.fDate <'" + objPropContracts.EndDate.AddDays(1) + "'");
            varname1.Append(" Group by jobi.Ref,pj.ID,PJ.fDate,PJ.Ref,PJ.fDesc,PJ.Amount,PJ.Vendor,PJ.Status,PJ.Batch,PJ.Terms,PJ.PO,PJ.TRID,PJ.Spec,PJ.IDate,PJ.UseTax,PJ.Disc,PJ.Custom1, PJ.Custom2, PJ.ReqBy, PJ.VoidR, PJ.ReceivePO  ");
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetJobCostItems(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT JobI.fDate, \n");
            varname1.Append("       JobI.Ref, \n");
            varname1.Append("       LEFT(JobI.fDesc, 100) AS fDesc, \n");
            varname1.Append("       JobI.Amount, \n");
            varname1.Append("       ( CASE JobI.Type \n");
            varname1.Append("           WHEN 1 THEN NULL \n");
            varname1.Append("           ELSE JobI.Amount \n");
            varname1.Append("         END )               AS RAmt, \n");
            varname1.Append("       ( CASE JobI.Type \n");
            varname1.Append("           WHEN 1 THEN JobI.Amount \n");
            varname1.Append("           ELSE NULL \n");
            varname1.Append("         END )               AS EAmt, \n");
            varname1.Append("       JobI.Type, \n");
            varname1.Append("       JobI.Phase, \n");
            varname1.Append("       JobI.TransID, \n");
            varname1.Append("       ( CASE \n");
            varname1.Append("           WHEN JobI.TransID > 0 THEN Trans.Type \n");
            varname1.Append("           WHEN JobI.TransID = 0 THEN 1 \n");
            varname1.Append("           ELSE -1 \n");
            varname1.Append("         END )               AS TType \n");
            varname1.Append("FROM   JobI \n");
            varname1.Append("       LEFT JOIN Trans \n");
            varname1.Append("              ON Abs(JobI.TransID) = Trans.ID \n");
            varname1.Append("WHERE  JobI.Job = " + objPropContracts.jobid + " \n");
            varname1.Append("--and  \n");
            varname1.Append("--APTicket=0 and JobI.Type= 1 and Trans.Type=41  \n");
            varname1.Append("ORDER  BY JobI.fDate, \n");
            varname1.Append("          JobI.Type, \n");
            varname1.Append("          JobI.Ref ");
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetInvoicesByID(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT i.*, \n");
            varname1.Append("       JobDecs = (SELECT fDesc FROM Job AS j WHERE j.ID = i.Job),\n");
            varname1.Append("       JobRemarks = (SELECT remarks FROM Job AS j WHERE j.ID = i.Job),\n");
            varname1.Append("       SPHandle = (SELECT SPHandle FROM Job AS j WHERE j.ID = i.Job),\n");
            varname1.Append("       SRemarks = (SELECT SRemarks FROM Job AS j WHERE j.ID = i.Job),\n");
            varname1.Append("       InvServ = (SELECT GLRev FROM Job AS j WHERE j.ID = i.Job),\n");
            // varname1.Append("       ProgressBillingNo=(select ProgressBillingNo from WIPHeader as w where w.InvoiceId=i.Ref),");
            varname1.Append("       Isnull(i.Stax,0)+Isnull(i.GTax,0) As TotalTax,  ");
            varname1.Append("       (SELECT TOP 1 Name \n");
            varname1.Append("        FROM   rol \n");
            varname1.Append("        WHERE  ID = (SELECT TOP 1 Rol \n");
            varname1.Append("                     FROM   Owner \n");
            varname1.Append("                     WHERE  ID = l.Owner)) AS customerName, \n");
            varname1.Append("       (SELECT TOP 1 Contact \n");
            varname1.Append("        FROM   rol \n");
            varname1.Append("        WHERE  ID = (SELECT TOP 1 Rol \n");
            varname1.Append("                     FROM   Owner \n");
            varname1.Append("                     WHERE  ID = l.Owner)) AS Contact, \n");
            varname1.Append("       (SELECT TOP 1 Phone \n");
            varname1.Append("        FROM   rol \n");
            varname1.Append("        WHERE  ID = (SELECT TOP 1 Rol \n");
            varname1.Append("                     FROM   Owner \n");
            varname1.Append("                     WHERE  ID = l.Owner)) AS Phone, \n");
            varname1.Append("      l.Custom12 AS EMail, \n");
            varname1.Append("      l.Custom13 AS CCEMail, \n");
            varname1.Append("       l.tag                               AS locname, \n");
            varname1.Append("       l.owner, \n");
            varname1.Append("       l.Address, \n");
            varname1.Append("       (CASE i.status \n");
            varname1.Append("         WHEN 0 THEN 'Open' \n");
            varname1.Append("         WHEN 1 THEN 'Paid' \n");
            varname1.Append("         WHEN 2 THEN 'Voided' \n");
            varname1.Append("         WHEN 4 THEN 'Marked as Pending' \n");
            varname1.Append("         WHEN 5 THEN 'Paid by Credit Card' \n");
            varname1.Append("         WHEN 3 THEN 'Partially Paid' \n");
            varname1.Append("       END  + case (select paid from tblinvoicepayment where ref=i.ref) WHEN 1 THEN '/Paid by MOM' else '' end )     AS statusname, \n");
            varname1.Append("       (SELECT fdesc \n");
            varname1.Append("        FROM   tblWork \n");
            varname1.Append("        WHERE  ID = i.Mech)                AS MechName, \n");
            varname1.Append("       (SELECT Type \n");
            varname1.Append("        FROM   JobType jt \n");
            varname1.Append("        WHERE  jt.ID = i.Type)             AS typeName, \n");
            varname1.Append("       CASE i.Terms \n");
            varname1.Append("         WHEN 0 THEN 'Upon Receipt' \n");
            varname1.Append("         WHEN 1 THEN 'Net 10 Days' \n");
            varname1.Append("         WHEN 2 THEN 'Net 15 Days' \n");
            varname1.Append("         WHEN 3 THEN 'Net 30 Days' \n");
            varname1.Append("         WHEN 4 THEN 'Net 45 Days' \n");
            varname1.Append("         WHEN 5 THEN 'Net 60 Days' \n");
            varname1.Append("         WHEN 6 THEN '2%-10/Net 30 Days' \n");
            varname1.Append("         WHEN 7 THEN 'Net 90 Days' \n");
            varname1.Append("         WHEN 8 THEN 'Net 180 Days' \n");
            varname1.Append("         WHEN 9 THEN 'COD' \n");
            varname1.Append("       END                                 AS termsText, \n");
            varname1.Append("       isnull((select paid from tblinvoicepayment where ref=i.ref),0)                                 AS paidcc, \n");
            varname1.Append("      convert(numeric(30,2), (isnull(i.total,0) - isnull((select balance from tblinvoicepayment where ref=i.ref),0) )) AS balance, \n");
            varname1.Append("      convert(numeric(30,2),   isnull((select balance from tblinvoicepayment where ref=i.ref),0) ) AS amtpaid \n");

            //For Adams
            varname1.Append("         , l.ID as LocID   \n");
            varname1.Append("         , l.custom12 as EmailTo   \n");
            varname1.Append("         , l.custom13 as EmailCC   \n");
            varname1.Append("         ,  isnull(j.Status, 0) as jobStatus   \n");
            varname1.Append("         ,  isnull(l.Status, 0) as locStatus   \n");

            //if (objPropContracts.isTS == 0)
            varname1.Append("FROM   Invoice i \n");
            //else
            //    varname1.Append("FROM   MS_Invoice i \n");
            varname1.Append("       INNER JOIN Loc l \n");
            varname1.Append("               ON l.Loc = i.Loc \n");
            varname1.Append("       LEFT JOIN Job j \n");
            varname1.Append("               ON j.ID = i.Job \n");

            varname1.Append("WHERE  Ref =  " + objPropContracts.InvoiceID + "");


            StringBuilder varname11 = new StringBuilder();
            varname11.Append(" \n");
            varname11.Append("SELECT i.Ref, \n");
            varname11.Append("       i.Line, \n");
            varname11.Append("       i.Acct, \n");
            varname11.Append("       i.Quan, \n");
            varname11.Append("       i.fDesc, \n");
            varname11.Append("       i.Price, \n");
            varname11.Append("       case isnull(i.stax,0) when 1 then Convert(numeric(30,2),( i.Quan * i.Price ) + ( ( ( i.Quan * i.Price ) * isnull(inv.TaxRate,0) ) / 100 ) + ( ( ( i.Quan * i.Price ) * isnull(inv.GSTRate,0) ) / 100 )) else convert(numeric(30,2),( i.Quan * i.Price )) end AS Amount, \n");
            varname11.Append("       i.STax,        \n");
            varname11.Append("       case isnull(i.stax,0) when 1 then Convert(numeric(30,2),(((ISNULL(i.Quan,0) * ISNULL(i.Price,0)) * ISNULL(inv.GSTRate,0))/100)) else 0 end As GTaxAmt,  \n");
            varname11.Append("       i.Job, \n");
            varname11.Append("       i.JobItem, \n");
            varname11.Append("       i.TransID, \n");
            varname11.Append("       i.Measure, \n");
            varname11.Append("       i.Disc, \n");
            varname11.Append(" i.JobOrg, \n");
            varname11.Append("       case isnull(i.stax,0) when 1 then convert(numeric(30,2),( ( i.Quan * i.Price ) * isnull(inv.TaxRate,0) ) / 100)  else 0 end                       AS StaxAmt, \n");
            varname11.Append("       isnull((case isnull(i.stax,0) when 1 then Convert(numeric(30,2),(((ISNULL(i.Quan,0) * ISNULL(i.Price,0)) * ISNULL(inv.GSTRate,0))/100)) else 0 end),0) + ");
            varname11.Append("       (case isnull(i.stax,0) when 1 then convert(numeric(30,2),( ( i.Quan * i.Price ) * isnull(inv.TaxRate,0) ) / 100)  else 0 end) As TotalTax, ");
            varname11.Append("       ( i.Quan * i.Price )                                                    AS pricequant, \n");
            varname11.Append("       (SELECT Name \n");
            varname11.Append("        FROM   Inv \n");
            varname11.Append("        WHERE  ID = i.Acct) AS billcode," +
                " isnull(i.jobitem,0) as code , \n");
            varname11.Append("  (SELECT Type        FROM   Inv    WHERE  ID = i.Acct)    as INVType  ," +
                "      i.Warehouse , " +
                "   isnull(i.WHLocID,0) WHLocID, \n");
            //if (objPropContracts.isTS == 0)
            varname11.Append("(SELECT Status FROM Inv WHERE ID = i.Acct) AS InvStatus, \n");
            varname11.Append("(SELECT top 1 Status FROM Chart WHERE ID in(SELECT SAcct FROM Inv WHERE ID = i.Acct)) AS AStatus, \n");
            varname11.Append("ProgressBillingNo=(select ProgressBillingNo from WIPHeader as w where w.InvoiceId=i.Ref )  \n");
            varname11.Append("FROM   InvoiceI i \n");
            //else
            //    varname11.Append("FROM   MS_InvoiceI i \n");
            //if(objPropContracts.isTS==0)
            varname11.Append("       INNER JOIN Invoice inv \n");
            //else
            //    varname11.Append("       INNER JOIN MS_Invoice inv \n");
            varname11.Append("               ON inv.Ref = i.Ref \n");
            varname11.Append("WHERE  i.Ref = " + objPropContracts.InvoiceID + "");
            varname11.Append(" order by Line");
            //StringBuilder varname11 = new StringBuilder();
            //varname11.Append(" \n");
            //varname11.Append("SELECT *, \n");
            //varname11.Append("       ( quan * price )     AS pricequant, \n");
            //varname11.Append("       (SELECT Name \n");
            //varname11.Append("        FROM   Inv \n");
            //varname11.Append("        WHERE  ID = i.Acct) AS billcode \n");
            //varname11.Append("FROM   InvoiceI i \n");
            //varname11.Append("WHERE  Ref = " + objPropContracts.InvoiceID + "");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString() + Environment.NewLine + varname11.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //API
        public DataSet GetInvoicesByID(GetInvoicesByIDParam _GetInvoicesByID, string ConnectionString)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT i.*, \n");
            varname1.Append("       JobDecs = (SELECT fDesc FROM Job AS j WHERE j.ID = i.Job),\n");
            varname1.Append("       JobRemarks = (SELECT remarks FROM Job AS j WHERE j.ID = i.Job),\n");
            varname1.Append("       SPHandle = (SELECT SPHandle FROM Job AS j WHERE j.ID = i.Job),\n");
            varname1.Append("       SRemarks = (SELECT SRemarks FROM Job AS j WHERE j.ID = i.Job),\n");
            varname1.Append("       InvServ = (SELECT GLRev FROM Job AS j WHERE j.ID = i.Job),\n");
            // varname1.Append("       ProgressBillingNo=(select ProgressBillingNo from WIPHeader as w where w.InvoiceId=i.Ref),");
            varname1.Append("       Isnull(i.Stax,0)+Isnull(i.GTax,0) As TotalTax,  ");
            varname1.Append("       (SELECT TOP 1 Name \n");
            varname1.Append("        FROM   rol \n");
            varname1.Append("        WHERE  ID = (SELECT TOP 1 Rol \n");
            varname1.Append("                     FROM   Owner \n");
            varname1.Append("                     WHERE  ID = l.Owner)) AS customerName, \n");
            varname1.Append("       (SELECT TOP 1 Contact \n");
            varname1.Append("        FROM   rol \n");
            varname1.Append("        WHERE  ID = (SELECT TOP 1 Rol \n");
            varname1.Append("                     FROM   Owner \n");
            varname1.Append("                     WHERE  ID = l.Owner)) AS Contact, \n");
            varname1.Append("       (SELECT TOP 1 Phone \n");
            varname1.Append("        FROM   rol \n");
            varname1.Append("        WHERE  ID = (SELECT TOP 1 Rol \n");
            varname1.Append("                     FROM   Owner \n");
            varname1.Append("                     WHERE  ID = l.Owner)) AS Phone, \n");
            varname1.Append("      l.Custom12 AS EMail, \n");
            varname1.Append("      l.Custom13 AS CCEMail, \n");
            varname1.Append("       l.tag                               AS locname, \n");
            varname1.Append("       l.owner, \n");
            varname1.Append("       l.Address, \n");
            varname1.Append("       (CASE i.status \n");
            varname1.Append("         WHEN 0 THEN 'Open' \n");
            varname1.Append("         WHEN 1 THEN 'Paid' \n");
            varname1.Append("         WHEN 2 THEN 'Voided' \n");
            varname1.Append("         WHEN 4 THEN 'Marked as Pending' \n");
            varname1.Append("         WHEN 5 THEN 'Paid by Credit Card' \n");
            varname1.Append("         WHEN 3 THEN 'Partially Paid' \n");
            varname1.Append("       END  + case (select paid from tblinvoicepayment where ref=i.ref) WHEN 1 THEN '/Paid by MOM' else '' end )     AS statusname, \n");
            varname1.Append("       (SELECT fdesc \n");
            varname1.Append("        FROM   tblWork \n");
            varname1.Append("        WHERE  ID = i.Mech)                AS MechName, \n");
            varname1.Append("       (SELECT Type \n");
            varname1.Append("        FROM   JobType jt \n");
            varname1.Append("        WHERE  jt.ID = i.Type)             AS typeName, \n");
            varname1.Append("       CASE i.Terms \n");
            varname1.Append("         WHEN 0 THEN 'Upon Receipt' \n");
            varname1.Append("         WHEN 1 THEN 'Net 10 Days' \n");
            varname1.Append("         WHEN 2 THEN 'Net 15 Days' \n");
            varname1.Append("         WHEN 3 THEN 'Net 30 Days' \n");
            varname1.Append("         WHEN 4 THEN 'Net 45 Days' \n");
            varname1.Append("         WHEN 5 THEN 'Net 60 Days' \n");
            varname1.Append("         WHEN 6 THEN '2%-10/Net 30 Days' \n");
            varname1.Append("         WHEN 7 THEN 'Net 90 Days' \n");
            varname1.Append("         WHEN 8 THEN 'Net 180 Days' \n");
            varname1.Append("         WHEN 9 THEN 'COD' \n");
            varname1.Append("       END                                 AS termsText, \n");
            varname1.Append("       isnull((select paid from tblinvoicepayment where ref=i.ref),0)                                 AS paidcc, \n");
            varname1.Append("      convert(numeric(30,2), (isnull(i.total,0) - isnull((select balance from tblinvoicepayment where ref=i.ref),0) )) AS balance, \n");
            varname1.Append("      convert(numeric(30,2),   isnull((select balance from tblinvoicepayment where ref=i.ref),0) ) AS amtpaid \n");

            //For Adams
            varname1.Append("         , l.ID as LocID   \n");
            varname1.Append("         , l.custom12 as EmailTo   \n");
            varname1.Append("         , l.custom13 as EmailCC   \n");
            varname1.Append("         ,  isnull(j.Status, 0) as jobStatus   \n");
            varname1.Append("         ,  isnull(l.Status, 0) as locStatus   \n");

            //if (objPropContracts.isTS == 0)
            varname1.Append("FROM   Invoice i \n");
            //else
            //    varname1.Append("FROM   MS_Invoice i \n");
            varname1.Append("       INNER JOIN Loc l \n");
            varname1.Append("               ON l.Loc = i.Loc \n");
            varname1.Append("       LEFT JOIN Job j \n");
            varname1.Append("               ON j.ID = i.Job \n");

            varname1.Append("WHERE  Ref =  " + _GetInvoicesByID.InvoiceID + "");


            StringBuilder varname11 = new StringBuilder();
            varname11.Append(" \n");
            varname11.Append("SELECT i.Ref, \n");
            varname11.Append("       i.Line, \n");
            varname11.Append("       i.Acct, \n");
            varname11.Append("       i.Quan, \n");
            varname11.Append("       i.fDesc, \n");
            varname11.Append("       i.Price, \n");
            varname11.Append("       case isnull(i.stax,0) when 1 then Convert(numeric(30,2),( i.Quan * i.Price ) + ( ( ( i.Quan * i.Price ) * isnull(inv.TaxRate,0) ) / 100 ) + ( ( ( i.Quan * i.Price ) * isnull(inv.GSTRate,0) ) / 100 )) else convert(numeric(30,2),( i.Quan * i.Price )) end AS Amount, \n");
            varname11.Append("       i.STax,        \n");
            varname11.Append("       case isnull(i.stax,0) when 1 then Convert(numeric(30,2),(((ISNULL(i.Quan,0) * ISNULL(i.Price,0)) * ISNULL(inv.GSTRate,0))/100)) else 0 end As GTaxAmt,  \n");
            varname11.Append("       i.Job, \n");
            varname11.Append("       i.JobItem, \n");
            varname11.Append("       i.TransID, \n");
            varname11.Append("       i.Measure, \n");
            varname11.Append("       i.Disc, \n");
            varname11.Append(" i.JobOrg, \n");
            varname11.Append("       case isnull(i.stax,0) when 1 then convert(numeric(30,2),( ( i.Quan * i.Price ) * isnull(inv.TaxRate,0) ) / 100)  else 0 end                       AS StaxAmt, \n");
            varname11.Append("       isnull((case isnull(i.stax,0) when 1 then Convert(numeric(30,2),(((ISNULL(i.Quan,0) * ISNULL(i.Price,0)) * ISNULL(inv.GSTRate,0))/100)) else 0 end),0) + ");
            varname11.Append("       (case isnull(i.stax,0) when 1 then convert(numeric(30,2),( ( i.Quan * i.Price ) * isnull(inv.TaxRate,0) ) / 100)  else 0 end) As TotalTax, ");
            varname11.Append("       ( i.Quan * i.Price )                                                    AS pricequant, \n");
            varname11.Append("       (SELECT Name \n");
            varname11.Append("        FROM   Inv \n");
            varname11.Append("        WHERE  ID = i.Acct) AS billcode," +
                " isnull(i.jobitem,0) as code , \n");
            varname11.Append("  (SELECT Type        FROM   Inv    WHERE  ID = i.Acct)    as INVType  ," +
                "      i.Warehouse , " +
                "   isnull(i.WHLocID,0) WHLocID, \n");
            //if (objPropContracts.isTS == 0)
            varname11.Append("(SELECT Status FROM Inv WHERE ID = i.Acct) AS InvStatus, \n");
            varname11.Append("(SELECT top 1 Status FROM Chart WHERE ID in(SELECT SAcct FROM Inv WHERE ID = i.Acct)) AS AStatus, \n");
            varname11.Append("ProgressBillingNo=(select ProgressBillingNo from WIPHeader as w where w.InvoiceId=i.Ref )  \n");
            varname11.Append("FROM   InvoiceI i \n");
            //else
            //    varname11.Append("FROM   MS_InvoiceI i \n");
            //if(objPropContracts.isTS==0)
            varname11.Append("       INNER JOIN Invoice inv \n");
            //else
            //    varname11.Append("       INNER JOIN MS_Invoice inv \n");
            varname11.Append("               ON inv.Ref = i.Ref \n");
            varname11.Append("WHERE  i.Ref = " + _GetInvoicesByID.InvoiceID + "");
            varname11.Append(" order by Line");
            //StringBuilder varname11 = new StringBuilder();
            //varname11.Append(" \n");
            //varname11.Append("SELECT *, \n");
            //varname11.Append("       ( quan * price )     AS pricequant, \n");
            //varname11.Append("       (SELECT Name \n");
            //varname11.Append("        FROM   Inv \n");
            //varname11.Append("        WHERE  ID = i.Acct) AS billcode \n");
            //varname11.Append("FROM   InvoiceI i \n");
            //varname11.Append("WHERE  Ref = " + objPropContracts.InvoiceID + "");

            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname1.ToString() + Environment.NewLine + varname11.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetStatusNameByInvoiceId(Contracts objPropContracts)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT STATUS");
            sql.Append(" 	,(");
            sql.Append(" 		CASE i.STATUS");
            sql.Append(" 			WHEN 0");
            sql.Append(" 				THEN 'Open'");
            sql.Append(" 			WHEN 1");
            sql.Append(" 				THEN 'Paid'");
            sql.Append(" 			WHEN 2");
            sql.Append(" 				THEN 'Voided'");
            sql.Append(" 			WHEN 4");
            sql.Append(" 				THEN 'Marked as Pending'");
            sql.Append(" 			WHEN 5");
            sql.Append(" 				THEN 'Paid by Credit Card'");
            sql.Append(" 			WHEN 3");
            sql.Append(" 				THEN 'Partially Paid'");
            sql.Append(" 			END + CASE (");
            sql.Append(" 				SELECT paid");
            sql.Append(" 				FROM tblinvoicepayment");
            sql.Append(" 				WHERE ref = i.ref");
            sql.Append(" 				)");
            sql.Append(" 			WHEN 1");
            sql.Append(" 				THEN '/Paid by MOM'");
            sql.Append(" 			ELSE ''");
            sql.Append(" 			END");
            sql.Append(" 		) AS statusname");
            sql.Append(" FROM Invoice i");
            sql.Append(" WHERE  i.Ref = " + objPropContracts.InvoiceID + "");
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, sql.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvoicesAmount(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT i.Ref, \n");
            varname1.Append("       ( Isnull(i.total, 0) - Isnull(ip.balance, 0) ) AS balance \n");
            varname1.Append("FROM   invoice i \n");
            varname1.Append("       LEFT OUTER JOIN tblInvoicePayment ip \n");
            varname1.Append("                    ON ip.Ref = i.Ref \n");
            varname1.Append("WHERE  i.Ref IN(" + objPropContracts.InvoiceIDCustom + ") ");
            // "select isnull(total,0) as total, ref from invoice where Ref in(" + objPropContracts.InvoiceIDCustom + ")"
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvoicesStatus(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT i.ref \n");
            varname1.Append("FROM   invoice i \n");
            varname1.Append("left outer join tblInvoicePayment ip on i.ref=ip.ref \n");
            varname1.Append("WHERE  i.Ref IN ( " + objPropContracts.InvoiceIDCustom + " ) \n");
            varname1.Append("       AND ( Isnull(ip.paid, 0) = 1 \n");
            varname1.Append("              OR Isnull(i.status, 0) = 1 ) ");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetACHCustomerAccounts(int OwnerID, string ConnConfig)
        {
            SqlParameter[] para = new SqlParameter[3];

            para[0] = new SqlParameter();
            para[0].ParameterName = "OwnerID";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = OwnerID;

            try
            {
                return SqlHelper.ExecuteDataset(ConnConfig, CommandType.StoredProcedure, "SpGetACHCustomerAccounts", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetInvoiceByRecurringFrequency(Contracts objPropContracts)
        {
            try
            {

                #region All parameters
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "FromDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropContracts.StartDate
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "Todate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objPropContracts.EndDate
                };

                #endregion

                return objPropContracts.Ds = SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, "spGetInvoiceByRecurringFrequency", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddACHCustomerAccounts(int OwnerID, string RoutingNo, string AccountNo, string Name, string ConnConfig)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[4];

                para[0] = new SqlParameter();
                para[0].ParameterName = "OwnerID";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = OwnerID;

                para[1] = new SqlParameter();
                para[1].ParameterName = "RoutingNo";
                para[1].SqlDbType = SqlDbType.VarChar;
                para[1].Value = RoutingNo;

                para[2] = new SqlParameter();
                para[2].ParameterName = "AccountNo";
                para[2].SqlDbType = SqlDbType.VarChar;
                para[2].Value = AccountNo;

                para[3] = new SqlParameter();
                para[3].ParameterName = "Name";
                para[3].SqlDbType = SqlDbType.VarChar;
                para[3].Value = Name;

                SqlHelper.ExecuteScalar(ConnConfig, CommandType.StoredProcedure, "SpAddACHCustomerAccounts", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteInvoice(Contracts objPropContracts)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[3];

                para[0] = new SqlParameter();
                para[0].ParameterName = "Ref";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = objPropContracts.Ref;

                para[1] = new SqlParameter();
                para[1].ParameterName = "Batch";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = objPropContracts.Batch;

                para[2] = new SqlParameter();
                para[2].ParameterName = "Loc";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = objPropContracts.Loc;

                SqlHelper.ExecuteScalar(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spDeleteInvoice", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteInvoiceByListID(Contracts objPropContracts)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, "spDeleteInvoiceByListID", objPropContracts.QBInvID);// CommandType.Text, "delete from invoicei where ref=(select ref from invoice where isnull(qbinvoiceid,'')<>'' and qbinvoiceid='" + objPropContracts.QBInvID + "') delete from invoice where isnull(qbinvoiceid,'')<>'' and qbinvoiceid='" + objPropContracts.QBInvID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetBillcodesforticket(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT 0    AS Ref, \n");
            varname1.Append("       0    AS line, \n");
            varname1.Append("       id   AS acct, \n");
            varname1.Append("       0.00    AS Quan, \n");
            varname1.Append("       fDesc, \n");
            varname1.Append("       0.00    AS price, \n");
            varname1.Append("       0.00    AS amount, \n");
            varname1.Append("       0.00    AS stax, \n");
            varname1.Append("       0    AS Job, \n");
            varname1.Append("       0    AS JobItem, \n");
            varname1.Append("       0    AS TransID, \n");
            varname1.Append("       ''   AS Measure, \n");
            varname1.Append("       0    AS Disc, \n");
            varname1.Append("       0.00    AS STaxAmt, \n");
            varname1.Append("       0.00    AS pricequant, \n");
            varname1.Append("       Name AS billcode, 0 as code, \n");
            varname1.Append("       0.00 AS GTaxAmt ,      \n");
            varname1.Append("       i.status as InvStatus     \n");
            varname1.Append("      ,convert (int,(SELECT top 1 Status FROM Chart WHERE ID in(SELECT SAcct FROM Inv WHERE ID = i.SAcct))) AS AStatus \n");
            varname1.Append("       , i.Type as INVType , '' Warehouse , 0 WHLocID    \n");

            varname1.Append("FROM   Inv i \n");
            varname1.Append("WHERE  Name IN (" + objPropContracts.TicketLineItems + " ) \n");
            varname1.Append("ORDER  BY billcode ");
            //'expenses', 'mileage', 'Time Spent' 
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetBillcodesforQBChargeableticket(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT 0    AS Ref, \n");
            varname1.Append("       0    AS line, \n");
            varname1.Append("       id   AS acct, \n");
            varname1.Append("       qbinvid , \n");
            varname1.Append("       0.00    AS Quan, \n");
            varname1.Append("       fDesc, \n");
            varname1.Append("       isnull(Price1,0)    AS price, \n");
            varname1.Append("       0.00    AS amount, \n");
            varname1.Append("       0.00    AS stax, \n");
            varname1.Append("       0    AS Job, \n");
            varname1.Append("       0    AS JobItem, \n");
            varname1.Append("       0    AS TransID, \n");
            varname1.Append("       ''   AS Measure, \n");
            varname1.Append("       0    AS Disc, \n");
            varname1.Append("       0.00    AS STaxAmt, \n");
            varname1.Append("       0.00    AS pricequant, \n");
            varname1.Append("       Name AS billcode \n");
            varname1.Append("FROM   Inv i \n");
            varname1.Append("WHERE  i.QBInvID ='" + objPropContracts.QBInvID + "' \n");
            if (objPropContracts.TicketLineItems != string.Empty)
            {
                varname1.Append(" or Name IN (" + objPropContracts.TicketLineItems + " ) \n");
            }
            if (objPropContracts.MileageItem != string.Empty)
            {
                varname1.Append(" or i.QBInvID = '" + objPropContracts.MileageItem + "' \n");
            }
            if (objPropContracts.LaborItem != string.Empty)
            {
                varname1.Append(" or i.QBInvID = '" + objPropContracts.LaborItem + "' \n");
            }
            if (objPropContracts.ExpenseItem != string.Empty)
            {
                varname1.Append(" or i.QBInvID = '" + objPropContracts.ExpenseItem + "' \n");
            }
            varname1.Append("ORDER  BY billcode ");
            //'expenses', 'mileage', 'Time Spent' 

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object AddPayment(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[20];

            para[0] = new SqlParameter();
            para[0].ParameterName = "InvoiceID";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = objPropContracts.InvoiceID;

            para[1] = new SqlParameter();
            para[1].ParameterName = "TransDate";
            para[1].SqlDbType = SqlDbType.DateTime;
            para[1].Value = objPropContracts.TransDate;

            para[2] = new SqlParameter();
            para[2].ParameterName = "CardNumber";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = objPropContracts.CardNumber;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Amount";
            para[3].SqlDbType = SqlDbType.Money;
            para[3].Value = objPropContracts.Amount;

            para[4] = new SqlParameter();
            para[4].ParameterName = "response";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropContracts.Response;

            para[5] = new SqlParameter();
            para[5].ParameterName = "refid";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropContracts.PaymentRefID;

            para[6] = new SqlParameter();
            para[6].ParameterName = "UserID";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropContracts.UserID;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Screen";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropContracts.Screen;

            para[8] = new SqlParameter();
            para[8].ParameterName = "ResponseCodes";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objPropContracts.ResponseCodes;

            para[9] = new SqlParameter();
            para[9].ParameterName = "Approved";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropContracts.Approved;

            para[10] = new SqlParameter();
            para[10].ParameterName = "IsSuccess";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropContracts.IsSuccess;

            para[11] = new SqlParameter();
            para[11].ParameterName = "CustomerID";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropContracts.CustID;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Status";
            para[12].SqlDbType = SqlDbType.Int;
            para[12].Value = objPropContracts.Status;

            para[13] = new SqlParameter();
            para[13].ParameterName = "PaymentUID";
            para[13].SqlDbType = SqlDbType.UniqueIdentifier;
            para[13].Value = objPropContracts.PaymentUID;

            para[14] = new SqlParameter();
            para[14].ParameterName = "GatewayOrderID";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropContracts.GatewayOrderID;

            para[15] = new SqlParameter();
            para[15].ParameterName = "@Routing";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropContracts.Routing;

            para[16] = new SqlParameter();
            para[16].ParameterName = "@BankAc";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropContracts.BankAC;

            para[17] = new SqlParameter();
            para[17].ParameterName = "@Name";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropContracts.BankAcHolName;

            para[18] = new SqlParameter();
            para[18].ParameterName = "@Filename";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropContracts.FileName;

            para[19] = new SqlParameter();
            para[19].ParameterName = "@PayType";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropContracts.PayType;

            try
            {
                return SqlHelper.ExecuteScalar(objPropContracts.ConnConfig, CommandType.StoredProcedure, "Spaddpayment", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object UpdateACHpaymentHistry(Contracts objPropContracts, string Responsestatus, string PaymentUID)
        {
            SqlParameter[] para = new SqlParameter[20];

            para[0] = new SqlParameter();
            para[0].ParameterName = "InvoiceID";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = objPropContracts.InvoiceID;

            para[1] = new SqlParameter();
            para[1].ParameterName = "CustomerID";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = objPropContracts.CustID;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Status";
            para[2].SqlDbType = SqlDbType.Int;
            para[2].Value = objPropContracts.Status;

            para[3] = new SqlParameter();
            para[3].ParameterName = "PaymentUID";
            para[3].SqlDbType = SqlDbType.NChar;
            para[3].Value = PaymentUID;

            para[4] = new SqlParameter();
            para[4].ParameterName = "Responsestatus";
            para[4].SqlDbType = SqlDbType.NChar;
            para[4].Value = Responsestatus;
            try
            {
                return SqlHelper.ExecuteScalar(objPropContracts.ConnConfig, CommandType.StoredProcedure, "SpUpdateACHpaymentHistry", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPaymentHistory(Contracts objPropContracts)
        {
            string strQuery = "select p.*,(select name from rol r where r.id = (select rol from owner o where o.id= p.customerid )) as owner, r.EN, isnull(B.Name, '') As Company, l.tag from tblPaymentHistory p ";
            strQuery += " inner join Invoice i on i.Ref=p.InvoiceID inner join loc l on l.loc=i.loc ";
            strQuery += " INNER JOIN Rol r  ON l.Rol = r.ID LEFT OUTER JOIN Branch B on B.ID = r.EN ";
            if (objPropContracts.EN == 1)
            {
                strQuery += " LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN ";
            }
            strQuery += " where transactionid is not null";
            strQuery += " and isnull(p.PayType,'') !='ACH' ";
            if (objPropContracts.EN == 1)
            {
                strQuery += " and UC.IsSel = 1 and UC.UserID =" + objPropContracts.UserID;
            }
            if (objPropContracts.RoleId != 0)
            {
                strQuery += " and isnull( l.RoleID,0) =" + objPropContracts.RoleId;
            }
            if (objPropContracts.StartDate != System.DateTime.MinValue)
            {
                strQuery += " and transdate >='" + objPropContracts.StartDate + "'";
            }
            if (objPropContracts.EndDate != System.DateTime.MinValue)
            {
                strQuery += " and transdate <'" + objPropContracts.EndDate.AddDays(1) + "'";
            }
            if (objPropContracts.InvoiceID != 0)
            {
                strQuery += " and InvoiceID = " + objPropContracts.InvoiceID;
            }
            if (objPropContracts.Medium != string.Empty)
            {
                strQuery += " and Medium ='" + objPropContracts.Medium + "'";
            }
            //if (objPropContracts.UserID != string.Empty)
            //{
            //    strQuery += " and userid='" + objPropContracts.UserID + "'";
            //}
            if (objPropContracts.CustID != 0)
            {
                strQuery += " and isnull( customerid ,0) =" + objPropContracts.CustID;
            }
            if (objPropContracts.IsSuccess != -1)
                strQuery += " and isnull( isSuccess ,0) =" + objPropContracts.IsSuccess;

            strQuery += " order by transdate desc";

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetACHPaymentHistory(Contracts objPropContracts, string status)
        {
            string strQuery = @" Select  x.PaymentUID,sum(x.Amount) As Amount , max (x.TransDate) TransDate , x.owner  ,x.Routing,x.BankAccNo,x.NameAccHolder,
          x.Response  as Response 
          , (select STUFF((SELECT ', ' + CAST(p.InvoiceID AS VARCHAR(10)) [text()]
          FROM tblPaymentHistory  p
		  where p.PaymentUID=x.PaymentUID
          FOR XML PATH(''), TYPE)
         .value('.','NVARCHAR(MAX)'),1,2,' ') ) as Invoice 
          From (";
            strQuery += "select p.*,(select name from rol r where r.id = (select rol from owner o where o.id= p.customerid )) as owner, l.tag from tblPaymentHistory p ";
            strQuery += " inner join Invoice i on i.Ref=p.InvoiceID inner join loc l on l.loc=i.loc ";
            strQuery += " where transactionid is not null";
            strQuery += " and isnull(p.PayType ,'') ='ACH' ";

            if (objPropContracts.StartDate != System.DateTime.MinValue)
            {
                strQuery += " and transdate >='" + objPropContracts.StartDate + "'";
            }
            if (objPropContracts.EndDate != System.DateTime.MinValue)
            {
                strQuery += " and transdate <'" + objPropContracts.EndDate.AddDays(1) + "'";
            }
            if (objPropContracts.InvoiceID != 0)
            {
                strQuery += " and  PaymentUID in ( SELECT PaymentUID FROM  tblPaymentHistory where InvoiceID like '%" + objPropContracts.InvoiceID + "%' )";
            }

            if (objPropContracts.CustID != 0)
            {
                strQuery += " and isnull( customerid ,0) =" + objPropContracts.CustID;
            }
            if (status != "All")
                strQuery += " and p.Approved ='" + status + "'";

            strQuery += @"  ) as X 

            group by x.PaymentUID, x.owner ,x.Response,x.Routing,x.BankAccNo,x.NameAccHolder";

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getPaymentGatewayInfo(Contracts objPropContracts)
        {
            string strQuery = "select * from tblGatewayInfo";

            if (!string.IsNullOrEmpty(objPropContracts.MerchantID))
            {
                strQuery += " where id=" + Convert.ToInt32(objPropContracts.MerchantID);
            }

            try
            {
                return objPropContracts.Ds = SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddMerchant(Contracts objPropContracts)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, "Spaddmerchant", objPropContracts.MerchantID, objPropContracts.LoginID, objPropContracts.PaymentUser, objPropContracts.PaymentPass, objPropContracts.MerchantInfoID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteMerchant(Contracts objPropContracts)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, CommandType.Text, "delete from tblgatewayinfo where id=" + objPropContracts.MerchantInfoID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetMaxQBInvoiceID(Contracts objPropContracts)
        {
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropContracts.ConnConfig, CommandType.Text, "select isnull(MAX(cast(Custom1 as int)),0)+1 from Invoice where QBInvoiceID is not null and IsNumeric(Custom1) = 1"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetBillcodesforTimeSheet(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT ID, \n");
            varname1.Append("       QBInvID, \n");
            varname1.Append("       Name AS billcode \n");
            varname1.Append("FROM   Inv i \n");
            varname1.Append("WHERE  QBInvID IS NOT NULL \n");
            varname1.Append("       AND type = 1 \n");
            varname1.Append("ORDER  BY billcode ");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPayrollforTimeSheet(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT ID, \n");
            varname1.Append("       QBwageID, \n");
            varname1.Append("       fdesc  \n");
            varname1.Append("FROM   prwage i \n");
            varname1.Append("WHERE  QBwageID IS NOT NULL \n");
            varname1.Append("ORDER  BY fdesc ");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPayrollByAccount(Contracts objPropContracts)
        {
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("SELECT TOP 1 QBwageID \n");
            //varname1.Append("FROM   prwage \n");
            //varname1.Append("WHERE  Isnull(QBAccountID, '') IS NOT NULL \n");
            //varname1.Append("       AND QBAccountID = (SELECT TOP 1 QBAccountID \n");
            //varname1.Append("                          FROM   Inv \n");
            //varname1.Append("                          WHERE  Isnull(QBAccountID, '') IS NOT NULL \n");
            //varname1.Append("                                 AND QBInvID = '"+objPropContracts.QBInvID+"') ");

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT TOP 1 QBPayrollItem \n");
            varname1.Append("FROM   TicketD \n");
            varname1.Append("WHERE  QBServiceItem = '" + objPropContracts.QBInvID + "' \n");
            varname1.Append("GROUP  BY QBPayrollItem \n");
            varname1.Append("ORDER  BY Count(QBPayrollItem) DESC ");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomerAddress(Contracts objPropContracts)
        {
            try
            {
                return objPropContracts.Ds = SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, "select r.Name,r.City,r.State,r.Zip,r.Phone, r.Fax,r.Contact,r.Address,r.EMail,r.Country from rol r inner join Owner o on o.Rol=r.ID where o.ID=" + objPropContracts.Owner);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvoicesByBatch(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT i.*, \n");
            varname1.Append("       (SELECT TOP 1 Name \n");
            varname1.Append("        FROM   rol \n");
            varname1.Append("        WHERE  ID = (SELECT TOP 1 Rol \n");
            varname1.Append("                     FROM   Owner \n");
            varname1.Append("                     WHERE  ID = l.Owner)) AS customerName, \n");
            varname1.Append("       l.tag                               AS locname, \n");
            varname1.Append("       l.owner, \n");
            varname1.Append("       l.Address, \n");
            varname1.Append("       i.Ref, \n");
            varname1.Append("       (CASE i.status \n");
            varname1.Append("         WHEN 0 THEN 'Open' \n");
            varname1.Append("         WHEN 1 THEN 'Paid' \n");
            varname1.Append("         WHEN 2 THEN 'Voided' \n");
            varname1.Append("         WHEN 4 THEN 'Marked as Pending' \n");
            varname1.Append("         WHEN 5 THEN 'Paid by Credit Card' \n");
            varname1.Append("         WHEN 3 THEN 'Partially Paid' \n");
            varname1.Append("       END  + case (select paid from tblinvoicepayment where ref=i.ref) WHEN 1 THEN '/Paid by MOM' else '' end )     AS statusname, \n");
            varname1.Append("       (SELECT fdesc \n");
            varname1.Append("        FROM   tblWork \n");
            varname1.Append("        WHERE  ID = i.Mech)                AS MechName, \n");
            varname1.Append("       (SELECT Type \n");
            varname1.Append("        FROM   JobType jt \n");
            varname1.Append("        WHERE  jt.ID = i.Type)             AS typeName, \n");
            varname1.Append("       CASE i.Terms \n");
            varname1.Append("         WHEN 0 THEN 'Upon Receipt' \n");
            varname1.Append("         WHEN 1 THEN 'Net 10 Days' \n");
            varname1.Append("         WHEN 2 THEN 'Net 15 Days' \n");
            varname1.Append("         WHEN 3 THEN 'Net 30 Days' \n");
            varname1.Append("         WHEN 4 THEN 'Net 45 Days' \n");
            varname1.Append("         WHEN 5 THEN 'Net 60 Days' \n");
            varname1.Append("         WHEN 6 THEN '2%-10/Net 30 Days' \n");
            varname1.Append("         WHEN 7 THEN 'Net 90 Days' \n");
            varname1.Append("         WHEN 8 THEN 'Net 180 Days' \n");
            varname1.Append("         WHEN 9 THEN 'COD' \n");
            varname1.Append("       END                                 AS termsText, \n");
            varname1.Append("       isnull((select paid from tblinvoicepayment where ref=i.ref),0)                                 AS paidcc, \n");
            varname1.Append("      convert(numeric(30,2), (isnull(i.total,0) - isnull((select balance from tblinvoicepayment where ref=i.ref),0) )) AS balance, \n");
            varname1.Append("      convert(numeric(30,2),   isnull((select balance from tblinvoicepayment where ref=i.ref),0) ) AS amtpaid \n");
            //if(objPropContracts.isTS==0)
            varname1.Append("FROM   Invoice i \n");
            //else
            //    varname1.Append("FROM   MS_Invoice i \n");
            varname1.Append("       INNER JOIN Loc l \n");
            varname1.Append("               ON l.Loc = i.Loc \n");

            varname1.Append("WHERE  i.Batch =  " + objPropContracts.Batch + "");
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateCustomerBalance(Contracts objPropContracts)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, "spUpdateCustomerLocBalance", objPropContracts.Loc, objPropContracts.Amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void UpdateCustomerBalance(UpdateCustomerBalanceParam _UpdateCustomerBalance, string ConnectionString)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "spUpdateCustomerLocBalance", _UpdateCustomerBalance.Loc, _UpdateCustomerBalance.Amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistContractByLoc(Contracts objPropContracts)
        {
            try
            {
                return objPropContracts.IsExistContract = Convert.ToBoolean(SqlHelper.ExecuteScalar(objPropContracts.ConnConfig, CommandType.Text, "SELECT  CAST( CASE WHEN EXISTS(SELECT Job, Loc, Owner FROM Contract WHERE Loc=" + objPropContracts.Loc + ")THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public bool IsExistContractByLoc(IsExistContractByLocParam _IsExistContractByLoc, string ConnectionString)
        {
            try
            {
                return _IsExistContractByLoc.IsExistContract = Convert.ToBoolean(SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, "SELECT  CAST( CASE WHEN EXISTS(SELECT Job, Loc, Owner FROM Contract WHERE Loc=" + _IsExistContractByLoc.Loc + ")THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetLastProcessDateOfInvoice(Contracts objPropContracts)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, "select top 1  custom15,Custom17 from job order by CONVERT (datetime,custom17) desc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvoicesDetailsByID(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT i.*, \n");
            varname1.Append("       (SELECT TOP 1 Name \n");
            varname1.Append("        FROM   rol \n");
            varname1.Append("        WHERE  ID = (SELECT TOP 1 Rol \n");
            varname1.Append("                     FROM   Owner \n");
            varname1.Append("                     WHERE  ID = l.Owner)) AS customerName, \n");
            varname1.Append("       l.tag                               AS locname, \n");
            //varname1.Append("(Select TOP 1 (case when Billing = '1' then (select l.tag from Loc as l where l.Loc = Central) when Billing = '0' then l.Tag end) \n");
            //varname1.Append("from owner where ID = l.Owner) as locname, \n");
            varname1.Append("       l.owner, \n");
            varname1.Append("       l.Address, \n");
            varname1.Append("       (CASE i.status \n");
            varname1.Append("         WHEN 0 THEN 'Open' \n");
            varname1.Append("         WHEN 1 THEN 'Paid' \n");
            varname1.Append("         WHEN 2 THEN 'Voided' \n");
            varname1.Append("         WHEN 4 THEN 'Marked as Pending' \n");
            varname1.Append("         WHEN 5 THEN 'Paid by Credit Card' \n");
            varname1.Append("         WHEN 3 THEN 'Partially Paid' \n");
            varname1.Append("       END  + case (select paid from tblinvoicepayment where ref=i.ref) WHEN 1 THEN '/Paid by MOM' else '' end )     AS statusname, \n");
            varname1.Append("       (SELECT fdesc \n");
            varname1.Append("        FROM   tblWork \n");
            varname1.Append("        WHERE  ID = i.Mech)                AS MechName, \n");
            varname1.Append("       (SELECT Type \n");
            varname1.Append("        FROM   JobType jt \n");
            varname1.Append("        WHERE  jt.ID = i.Type)             AS typeName, \n");
            varname1.Append("       CASE i.Terms \n");
            varname1.Append("         WHEN 0 THEN 'Upon Receipt' \n");
            varname1.Append("         WHEN 1 THEN 'Net 10 Days' \n");
            varname1.Append("         WHEN 2 THEN 'Net 15 Days' \n");
            varname1.Append("         WHEN 3 THEN 'Net 30 Days' \n");
            varname1.Append("         WHEN 4 THEN 'Net 45 Days' \n");
            varname1.Append("         WHEN 5 THEN 'Net 60 Days' \n");
            varname1.Append("         WHEN 6 THEN '2%-10/Net 30 Days' \n");
            varname1.Append("         WHEN 7 THEN 'Net 90 Days' \n");
            varname1.Append("         WHEN 8 THEN 'Net 180 Days' \n");
            varname1.Append("         WHEN 9 THEN 'COD' \n");
            varname1.Append("       END                                 AS termsText, \n");
            varname1.Append("       i.Terms as payterms, \n");
            varname1.Append("       isnull((select paid from tblinvoicepayment where ref=i.ref),0)                                 AS paidcc, \n");
            varname1.Append("      convert(numeric(30,2), (isnull(i.total,0) - isnull((select balance from tblinvoicepayment where ref=i.ref),0) )) AS balance, \n");
            varname1.Append("      convert(numeric(30,2),   isnull((select balance from tblinvoicepayment where ref=i.ref),0) ) AS amtpaid \n");
            //if(objPropContracts.isTS==0)
            varname1.Append("FROM   Invoice i \n");
            //else
            //    varname1.Append("FROM   MS_Invoice i \n");
            varname1.Append("       INNER JOIN Loc l \n");
            varname1.Append("               ON l.Loc = i.Loc \n");

            varname1.Append("WHERE  Ref =  " + objPropContracts.InvoiceID + "");


            StringBuilder varname11 = new StringBuilder();
            varname11.Append(" \n");
            varname11.Append("SELECT  \n");
            varname11.Append("       i.Line, \n");
            varname11.Append("       i.Acct, \n");
            varname11.Append("       i.Quan, \n");
            varname11.Append("       i.fDesc as ifDesc, \n");
            varname11.Append("       i.Price, \n");
            varname11.Append("       case isnull(i.stax,0) when 1 then ( i.Quan * i.Price ) + ( ( ( i.Quan * i.Price ) * isnull(inv.TaxRate,0) ) / 100 ) else ( i.Quan * i.Price ) end AS iAmount, \n");
            varname11.Append("       i.STax as iSTax, \n");
            //varname11.Append("       i.Job, \n");
            varname11.Append("       i.JobItem, \n");
            //arname11.Append("       i.TransID, \n");
            varname11.Append("       i.Measure, \n");
            varname11.Append("       i.Disc, \n");
            varname11.Append("       case isnull(i.stax,0) when 1 then convert(numeric(30,2),( ( i.Quan * i.Price ) * isnull(inv.TaxRate,0) ) / 100)  else 0 end                       AS StaxAmt, \n");
            varname11.Append("       ( i.Quan * i.Price )                                                    AS pricequant, \n");
            varname11.Append("       (SELECT Name \n");
            varname11.Append("        FROM   Inv \n");
            varname11.Append("        WHERE  ID = i.Acct)                                                    AS billcode, isnull(i.jobitem,0) as code \n");
            //if(objPropContracts.isTS==0)
            varname11.Append("FROM   InvoiceI i \n");
            //else
            //    varname11.Append("FROM   MS_InvoiceI i \n");
            //if(objPropContracts.isTS==0)
            varname11.Append("       INNER JOIN Invoice inv \n");
            //else
            //    varname11.Append("       INNER JOIN MS_Invoice inv \n");
            varname11.Append("               ON inv.Ref = i.Ref \n");
            varname11.Append("WHERE  i.Ref = " + objPropContracts.InvoiceID + "");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString() + Environment.NewLine + varname11.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetEmailDetailByLoc(Contracts objPropContracts)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, "SELECT i.Loc,l.ID, l.Tag, l.custom12, l.custom13 FROM Invoice i INNER JOIN Loc l ON l.Loc = i.Loc where i.Ref=" + objPropContracts.Ref);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //API
        public DataSet GetEmailDetailByLoc(GetEmailDetailByLocParam _GetEmailDetailByLoc, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "SELECT i.Loc,l.ID, l.Tag, l.custom12, l.custom13 FROM Invoice i INNER JOIN Loc l ON l.Loc = i.Loc where i.Ref=" + _GetEmailDetailByLoc.Ref);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetAllTicketID(Contracts objPropContracts)
        {
            try
            {
                StringBuilder varname3 = new StringBuilder();
                varname3.Append("SELECT ID FROM TicketD Where Invoice= " + objPropContracts.InvoiceID);
                int TicketID = Convert.ToInt32(SqlHelper.ExecuteScalar(objPropContracts.ConnConfig, CommandType.Text, varname3.ToString()));
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname3.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetAllTicketID(GetAllTicketIDParam _GetAllTicketID, string ConnectionString)
        {
            try
            {
                StringBuilder varname3 = new StringBuilder();
                varname3.Append("SELECT ID FROM TicketD Where Invoice= " + _GetAllTicketID.InvoiceID);
                int TicketID = Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, varname3.ToString()));
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname3.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTicketID(Contracts objPropContracts)
        {
            try
            {
                StringBuilder varname3 = new StringBuilder();
                varname3.Append("SELECT ID FROM TicketD Where Charge=0 and Invoice= " + objPropContracts.InvoiceID);
                int TicketID = Convert.ToInt32(SqlHelper.ExecuteScalar(objPropContracts.ConnConfig, CommandType.Text, varname3.ToString()));
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname3.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetTicketID(GetTicketIDParam _GetTicketID, string ConnectionString)
        {
            try
            {
                StringBuilder varname3 = new StringBuilder();
                varname3.Append("SELECT ID FROM TicketD Where Charge=0 and Invoice= " + _GetTicketID.InvoiceID);
                int TicketID = Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, CommandType.Text, varname3.ToString()));
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname3.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetInvoicesByRef(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT  \n");
            varname1.Append("	i.*,  \n");
            varname1.Append("	i.DDate AS DueDate,  \n");
            varname1.Append("	ro.Name AS CustomerName,  \n");
            varname1.Append("	o.Billing,  \n");
            varname1.Append("	l.Tag AS LocName,  \n");
            varname1.Append("	r.Address AS BillToAddress,  \n");
            varname1.Append("	r.City AS BillToCity,  \n");
            varname1.Append("	r.State AS BillToState,  \n");
            varname1.Append("	r.Zip AS BillToZip,  \n");
            varname1.Append("	l.owner,  \n");
            varname1.Append("	l.Address,  \n");
            varname1.Append("	l.ID,  \n");
            varname1.Append("	l.City,  \n");
            varname1.Append("	l.State,  \n");
            varname1.Append("	l.Zip,  \n");
            varname1.Append("	l.Custom12 AS EMail,  \n");
            varname1.Append("	l.Custom13 AS CCEMail,  \n");
            varname1.Append("	t.Name AS TerrName,  \n");
            varname1.Append("	rt.Name AS RouteName,  \n");
            varname1.Append("	(CASE i.status  \n");
            varname1.Append("		WHEN 0 THEN 'Open'  \n");
            varname1.Append("		WHEN 1 THEN 'Paid'  \n");
            varname1.Append("		WHEN 2 THEN 'Voided'  \n");
            varname1.Append("		WHEN 4 THEN 'Marked AS Pending'  \n");
            varname1.Append("		WHEN 5 THEN 'Paid by Credit Card'  \n");
            varname1.Append("		WHEN 3 THEN 'Partially Paid'  \n");
            varname1.Append("	END + CASE (SELECT paid FROM tblinvoicepayment WHERE Ref = i.Ref) WHEN 1 THEN '/Paid by MOM' ELSE '' END) AS StatusName,  \n");
            varname1.Append("	tb.fDesc AS MechName,  \n");
            varname1.Append("	jt.Type AS TypeName,  \n");
            varname1.Append("	tm.Name AS TermsText,  \n");
            varname1.Append("	i.Terms AS Payterms,  \n");
            varname1.Append("	i.PO,  \n");
            varname1.Append("	(CASE i.Status WHEN 0 THEN 0 WHEN 1 THEN i.Total WHEN 3 THEN ISNULL((SELECT Selected FROM OpenAR WHERE Ref= i.Ref AND Type = 0),0) END) AS PaidCC, \n");
            varname1.Append("	(CASE i.Status WHEN 0 THEN 0 WHEN 1 THEN i.Total WHEN 3 THEN ISNULL((SELECT Selected FROM OpenAR WHERE Ref= i.Ref AND Type = 0),0) END) AS Paid, \n");
            varname1.Append("	(CASE i.Status WHEN 0 THEN i.Total WHEN 1 THEN 0  WHEN 3 THEN ISNULL((SELECT Balance FROM OpenAR WHERE Ref= i.Ref AND Type = 0),0) END) AS Balance, \n");
            varname1.Append("	(CASE i.Status WHEN 0 THEN 0 WHEN 1 THEN i.Total WHEN 3 THEN ISNULL((SELECT Selected FROM OpenAR WHERE Ref= i.Ref AND Type = 0),0) END) AS AmtPaid, \n");
            varname1.Append("	CASE WHEN (l.custom12 = '' OR l.custom12 is null) THEN  0 ELSE 1 END AS IsExistsEmail,    \n");
            varname1.Append("	l.EmailInvoice, \n");
            varname1.Append("	l.ID AS LocID, \n");
            varname1.Append("	l.custom12 AS EmailTo, \n");
            varname1.Append("	l.custom13 AS EmailCC, \n");
            varname1.Append("	j.Remarks AS JobRemarks, \n");
            varname1.Append("	w.ProgressBillingNo, \n");
            varname1.Append("	st.Rate, \n");
            varname1.Append("	st.PSTReg, \n");
            varname1.Append("	st.Type AS STaxType, \n");
            varname1.Append("	j.SRemarks as SpecialNote \n");
            varname1.Append("FROM Invoice i  \n");
            varname1.Append("   INNER JOIN Loc l ON l.Loc = i.Loc \n");
            varname1.Append("   INNER JOIN Rol r ON l.Rol = r.ID \n");
            varname1.Append("	INNER JOIN Owner o ON l.Owner = o.ID \n");
            varname1.Append("   INNER JOIN Rol ro ON o.Rol = ro.ID \n");
            varname1.Append("	LEFT JOIN Job j ON j.ID = i.Job \n");
            varname1.Append("	LEFT JOIN Terr t ON t.ID = l.Terr \n");
            varname1.Append("	LEFT JOIN tblTerms tm ON tm.ID = i.Terms \n");
            varname1.Append("	LEFT JOIN Route rt ON rt.ID = l.Route \n");
            varname1.Append("	LEFT JOIN WIPHeader w ON w.InvoiceId = i.Ref \n");
            varname1.Append("	LEFT JOIN tblWork tb ON tb.ID = i.Mech \n");
            varname1.Append("	LEFT JOIN JobType jt ON jt.ID = i.Type \n");
            varname1.Append("	LEFT JOIN STax st ON st.Name = i.TaxRegion AND st.UType = 0 \n");
            varname1.Append("WHERE i.Ref = " + objPropContracts.InvoiceID + "\n");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetInvoicesByRef(GetInvoicesByRefParam _GetInvoicesByRef, string ConnectionString)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT  \n");
            varname1.Append("	i.*,  \n");
            varname1.Append("	i.DDate AS DueDate,  \n");
            varname1.Append("	ro.Name AS CustomerName,  \n");
            varname1.Append("	o.Billing,  \n");
            varname1.Append("	l.Tag AS LocName,  \n");
            varname1.Append("	r.Address AS BillToAddress,  \n");
            varname1.Append("	r.City AS BillToCity,  \n");
            varname1.Append("	r.State AS BillToState,  \n");
            varname1.Append("	r.Zip AS BillToZip,  \n");
            varname1.Append("	l.owner,  \n");
            varname1.Append("	l.Address,  \n");
            varname1.Append("	l.ID,  \n");
            varname1.Append("	l.City,  \n");
            varname1.Append("	l.State,  \n");
            varname1.Append("	l.Zip,  \n");
            varname1.Append("	l.Custom12 AS EMail,  \n");
            varname1.Append("	l.Custom13 AS CCEMail,  \n");
            varname1.Append("	t.Name AS TerrName,  \n");
            varname1.Append("	rt.Name AS RouteName,  \n");
            varname1.Append("	(CASE i.status  \n");
            varname1.Append("		WHEN 0 THEN 'Open'  \n");
            varname1.Append("		WHEN 1 THEN 'Paid'  \n");
            varname1.Append("		WHEN 2 THEN 'Voided'  \n");
            varname1.Append("		WHEN 4 THEN 'Marked AS Pending'  \n");
            varname1.Append("		WHEN 5 THEN 'Paid by Credit Card'  \n");
            varname1.Append("		WHEN 3 THEN 'Partially Paid'  \n");
            varname1.Append("	END + CASE (SELECT paid FROM tblinvoicepayment WHERE Ref = i.Ref) WHEN 1 THEN '/Paid by MOM' ELSE '' END) AS StatusName,  \n");
            varname1.Append("	tb.fDesc AS MechName,  \n");
            varname1.Append("	jt.Type AS TypeName,  \n");
            varname1.Append("	tm.Name AS TermsText,  \n");
            varname1.Append("	i.Terms AS Payterms,  \n");
            varname1.Append("	i.PO,  \n");
            varname1.Append("	(CASE i.Status WHEN 0 THEN 0   WHEN 1 THEN i.Total WHEN 3 THEN ISNULL((SELECT Selected FROM OpenAR WHERE Ref= i.Ref),0) END) AS PaidCC, \n");
            varname1.Append("	(CASE i.Status WHEN 0 THEN 0   WHEN 1 THEN i.Total WHEN 3 THEN ISNULL((SELECT Selected FROM OpenAR WHERE Ref= i.Ref),0) END) AS Paid, \n");
            varname1.Append("	(CASE i.Status WHEN 0 THEN i.Total WHEN 1 THEN 0  WHEN 3 THEN ISNULL((SELECT balance FROM OpenAR WHERE Ref= i.Ref),0) END) AS Balance, \n");
            varname1.Append("	(CASE i.Status WHEN 0 THEN 0   WHEN 1 THEN i.Total WHEN 3 THEN ISNULL((SELECT Selected FROM OpenAR WHERE Ref= i.Ref),0) END) AS AmtPaid, \n");
            varname1.Append("	CASE WHEN (l.custom12 = '' OR l.custom12 is null) THEN  0 ELSE 1 END AS IsExistsEmail,    \n");
            varname1.Append("	l.EmailInvoice,    \n");
            varname1.Append("	l.ID AS LocID,    \n");
            varname1.Append("	l.custom12 AS EmailTo,    \n");
            varname1.Append("	l.custom13 AS EmailCC,    \n");
            varname1.Append("	JobRemarks = (SELECT remarks FROM Job AS j WHERE j.ID = i.Job),  \n");
            varname1.Append("	w.ProgressBillingNo, \n");
            varname1.Append("	st.Rate, \n");
            varname1.Append("	st.PSTReg,   \n");
            varname1.Append("	st.Type AS STaxType   \n");
            varname1.Append("FROM Invoice i  \n");
            varname1.Append("   INNER JOIN Loc l ON l.Loc = i.Loc  \n");
            varname1.Append("   INNER JOIN Rol r ON l.Rol = r.ID  \n");
            varname1.Append("	INNER JOIN Owner o ON l.Owner = o.ID \n");
            varname1.Append("   INNER JOIN Rol ro ON o.Rol = ro.ID \n");
            varname1.Append("	LEFT JOIN Terr t ON t.ID = l.Terr \n");
            varname1.Append("	LEFT JOIN tblTerms tm ON tm.ID = i.Terms \n");
            varname1.Append("	LEFT JOIN Route rt ON rt.ID = l.Route \n");
            varname1.Append("	LEFT JOIN WIPHeader w ON w.InvoiceId = i.Ref \n");
            varname1.Append("	LEFT JOIN tblWork tb ON tb.ID = i.Mech \n");
            varname1.Append("	LEFT JOIN JobType jt ON jt.ID = i.Type \n");
            varname1.Append("	LEFT JOIN STax st ON st.Name = i.TaxRegion AND st.UType = 0 \n");
            varname1.Append("WHERE i.Ref = " + _GetInvoicesByRef.InvoiceID + "\n");

            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetInvoiceItemByRef(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append(" \n");
            varname1.Append("SELECT i.Ref, \n");
            varname1.Append("       i.Line, \n");
            varname1.Append("       i.Acct, \n");
            varname1.Append("       i.Quan, \n");
            varname1.Append("       i.fDesc as fDesc, \n");
            varname1.Append("       i.Price, \n");
            //varname1.Append("       case isnull(i.stax,0) when 1 then ( i.Quan * i.Price ) + ( ( ( i.Quan * i.Price ) * isnull(inv.TaxRate,0) ) / 100 ) else ( i.Quan * i.Price ) end AS Amount, \n");
            varname1.Append("       i.Amount, \n");
            varname1.Append("       i.STax as STax, \n");
            varname1.Append("       i.JobItem, \n");
            varname1.Append("       i.Measure, \n");
            varname1.Append("       i.Disc, \n");
            varname1.Append("       ProgressBillingNo=(select ProgressBillingNo from WIPHeader as w where w.InvoiceId=i.Ref ),  \n");
            varname1.Append("       isnull(i.staxAmt,0) as staxAmt, \n ");
            varname1.Append("       case isnull(i.stax,0) when 1 then Convert(numeric(30,2),(((ISNULL(i.Quan,0) * ISNULL(i.Price,0)) * ISNULL(inv.GSTRate,0))/100)) else 0 end As GTaxAmt,   \n");
            varname1.Append("       isnull((case isnull(i.stax,0) when 1 then Convert(numeric(30,2),(((ISNULL(i.Quan,0) * ISNULL(i.Price,0)) * ISNULL(inv.GSTRate,0))/100)) else 0 end),0) + \n");
            varname1.Append("       (case isnull(i.stax,0) when 1 then convert(numeric(30,2),( ( i.Quan * i.Price ) * isnull(inv.TaxRate,0) ) / 100)  else 0 end) As TotalTax,               \n");
            //varname1.Append("       case isnull(i.stax,0) when 1 then convert(numeric(30,2),( ( i.Quan * i.Price ) * isnull(inv.TaxRate,0) ) / 100)  else 0 end                       AS StaxAmt, \n");
            varname1.Append("       ( i.Quan * i.Price )                                                    AS pricequant, \n");
            varname1.Append("       (SELECT Name \n");
            varname1.Append("        FROM   Inv \n");
            varname1.Append("        WHERE  ID = i.Acct)                                                    AS billcode, isnull(i.jobitem,0) as code, \n");
            varname1.Append("        convert(numeric(30,2), (isnull(inv.total,0) - isnull((select balance from tblinvoicepayment where ref=inv.ref),0) )) AS balance, \n");
            varname1.Append("        convert(numeric(30,2),   isnull((select balance from tblinvoicepayment where ref=i.ref),0) ) AS amtpaid,");
            varname1.Append("        inv.Total,");
            varname1.Append("         i.JobOrg,");
            //Rahil
            varname1.Append("        inv.STax As INVSTax,");
            varname1.Append("        inv.Amount As INVAmount,");
            varname1.Append("        inv.Taxable,");
            varname1.Append("        inv.fDesc As Description,");
            //Rahil

            varname1.Append("         CASE WHEN inv.TaxRegion = '' THEN 'Non-Taxable' else inv.TaxRegion END As TaxRegion ");

            //if (objPropContracts.isTS == 0)
            varname1.Append("FROM   InvoiceI i \n");
            //else
            //    varname1.Append("FROM   MS_InvoiceI i \n");
            //if (objPropContracts.isTS == 0)
            varname1.Append("       INNER JOIN Invoice inv \n");
            //else
            //    varname1.Append("       INNER JOIN MS_Invoice inv \n");
            varname1.Append("               ON inv.Ref = i.Ref \n");
            varname1.Append("WHERE  i.Ref = " + objPropContracts.InvoiceID + "");
            varname1.Append("order by line");
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetInvoiceItemByRef(GetInvoiceItemByRefParam _GetInvoiceItemByRef, string ConnectionString)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append(" \n");
            varname1.Append("SELECT i.Ref, \n");
            varname1.Append("       i.Line, \n");
            varname1.Append("       i.Acct, \n");
            varname1.Append("       i.Quan, \n");
            varname1.Append("       i.fDesc as fDesc, \n");
            varname1.Append("       i.Price, \n");
            //varname1.Append("       case isnull(i.stax,0) when 1 then ( i.Quan * i.Price ) + ( ( ( i.Quan * i.Price ) * isnull(inv.TaxRate,0) ) / 100 ) else ( i.Quan * i.Price ) end AS Amount, \n");
            varname1.Append("       i.Amount, \n");
            varname1.Append("       i.STax as STax, \n");
            varname1.Append("       i.JobItem, \n");
            varname1.Append("       i.Measure, \n");
            varname1.Append("       i.Disc, \n");
            varname1.Append("       ProgressBillingNo=(select ProgressBillingNo from WIPHeader as w where w.InvoiceId=i.Ref ),  \n");
            varname1.Append("       isnull(i.staxAmt,0) as staxAmt, \n ");
            varname1.Append("       case isnull(i.stax,0) when 1 then Convert(numeric(30,2),(((ISNULL(i.Quan,0) * ISNULL(i.Price,0)) * ISNULL(inv.GSTRate,0))/100)) else 0 end As GTaxAmt,   \n");
            varname1.Append("       isnull((case isnull(i.stax,0) when 1 then Convert(numeric(30,2),(((ISNULL(i.Quan,0) * ISNULL(i.Price,0)) * ISNULL(inv.GSTRate,0))/100)) else 0 end),0) + \n");
            varname1.Append("       (case isnull(i.stax,0) when 1 then convert(numeric(30,2),( ( i.Quan * i.Price ) * isnull(inv.TaxRate,0) ) / 100)  else 0 end) As TotalTax,               \n");
            //varname1.Append("       case isnull(i.stax,0) when 1 then convert(numeric(30,2),( ( i.Quan * i.Price ) * isnull(inv.TaxRate,0) ) / 100)  else 0 end                       AS StaxAmt, \n");
            varname1.Append("       ( i.Quan * i.Price )                                                    AS pricequant, \n");
            varname1.Append("       (SELECT Name \n");
            varname1.Append("        FROM   Inv \n");
            varname1.Append("        WHERE  ID = i.Acct)                                                    AS billcode, isnull(i.jobitem,0) as code, \n");
            varname1.Append("        convert(numeric(30,2), (isnull(inv.total,0) - isnull((select balance from tblinvoicepayment where ref=inv.ref),0) )) AS balance, \n");
            varname1.Append("        convert(numeric(30,2),   isnull((select balance from tblinvoicepayment where ref=i.ref),0) ) AS amtpaid,");
            varname1.Append("        inv.Total,");
            varname1.Append("         i.JobOrg,");
            //Rahil
            varname1.Append("        inv.STax As INVSTax,");
            varname1.Append("        inv.Amount As INVAmount,");
            varname1.Append("        inv.Taxable,");
            varname1.Append("        inv.fDesc As Description,");
            //Rahil

            varname1.Append("         CASE WHEN inv.TaxRegion = '' THEN 'Non-Taxable' else inv.TaxRegion END As TaxRegion ");

            //if (objPropContracts.isTS == 0)
            varname1.Append("FROM   InvoiceI i \n");
            //else
            //    varname1.Append("FROM   MS_InvoiceI i \n");
            //if (objPropContracts.isTS == 0)
            varname1.Append("       INNER JOIN Invoice inv \n");
            //else
            //    varname1.Append("       INNER JOIN MS_Invoice inv \n");
            varname1.Append("               ON inv.Ref = i.Ref \n");
            varname1.Append("WHERE  i.Ref = " + _GetInvoiceItemByRef.InvoiceID + "");
            varname1.Append("order by line");
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateVoidInvoiceDetails(Contracts objPropContracts)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, "spVoidInvoice", objPropContracts.Ref, objPropContracts.Date.ToShortDateString(), objPropContracts.Fuser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateExpirationDate(Contracts objPropContracts)
        {
            try
            {
                string str = "update contract set expirationdate = '" + objPropContracts.ExpirationDate + "' where job in (" + objPropContracts.jobids + ")";
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void UpdateExpirationDate(UpdateExpirationDateParam _UpdateExpirationDate, string ConnectionString)
        {
            try
            {
                string str = "update contract set expirationdate = '" + _UpdateExpirationDate.ExpirationDate + "' where job in (" + _UpdateExpirationDate.jobids + ")";
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustomerStatementInvoices(Contracts objPropContracts, bool includeCredit)
        {
            StringBuilder varname = new StringBuilder();

            varname.Append("       SELECT * FROM (         \n");
            varname.Append("           SELECT   OpenAR.Loc,                                         \n");
            varname.Append("                    OpenAR.fDate,                                       \n");
            varname.Append("                    'Invoice' as Type,                                  \n");
            varname.Append("                    CONVERT(VARCHAR(50),OpenAR.Ref) AS Ref,             \n");
            varname.Append("                    OpenAR.fDesc,                                       \n");
            varname.Append("                    OpenAR.Balance,                                     \n");
            varname.Append("                    ISNULL(DATEDIFF(Day, ISNULL(OpenAR.Due, Invoice.DDate), GETDATE()),0) AS Days   \n");
            varname.Append("               FROM Invoice INNER JOIN OpenAR ON Invoice.Ref = OpenAR.Ref AND OpenAR.Type = 0   \n");
            varname.Append("               WHERE	    OpenAR.Loc = '" + objPropContracts.Loc + "' \n");
            varname.Append("                   	AND Invoice.Status <> 1                             \n");
            varname.Append("                    AND Invoice.Status <> 2                             \n");
            if (includeCredit)
            {
                varname.Append("                    AND OpenAR.Balance <> 0                         \n");
            }
            else
            {
                varname.Append("                    AND OpenAR.Balance > 0                          \n");
            }

            if (objPropContracts.IsOverDue)
            {
                varname.Append("        AND ISNULL(DATEDIFF(Day, ISNULL(OpenAR.Due, Invoice.DDate), GETDATE()),0) > 0      \n");
            }
            varname.Append("       UNION       \n");
            varname.Append("       SELECT   OpenAR.Loc,                                             \n");
            varname.Append("                OpenAR.fDate,                                           \n");
            varname.Append("                    (CASE OpenAR.Type                                   \n");
            varname.Append("                        WHEN 2 THEN 'Credit Invoice'                    \n");
            varname.Append("                        WHEN 3 THEN 'Deposit'                           \n");
            varname.Append("                    END)                                                \n");
            varname.Append("                AS Type,                                                \n");
            varname.Append("                    (CASE OpenAR.Type                                   \n");
            varname.Append("                        WHEN 2 THEN ReceivedPayment.CheckNumber         \n");
            varname.Append("                        ELSE CONVERT(VARCHAR(50),OpenAR.Ref)            \n");
            varname.Append("                    END)                                                \n");
            varname.Append("                AS Ref,                                                 \n");
            varname.Append("                OpenAR.fDesc,                                           \n");
            varname.Append("                OpenAR.Balance,                                         \n");
            varname.Append("                ISNULL(DATEDIFF(Day, OpenAR.Due, GETDATE()),0) AS Days  \n");
            varname.Append("           FROM OpenAR                                                  \n");
            varname.Append("                LEFT JOIN ReceivedPayment ON ReceivedPayment.ID = OpenAR.Ref AND OpenAR.Type = 2    \n");
            varname.Append("           WHERE  OpenAR.Loc = '" + objPropContracts.Loc + "'           \n");
            varname.Append("              AND OpenAR.Balance <> 0                                   \n");

            if (includeCredit)
            {
                varname.Append("              AND OpenAR.Type IN (2,3)                              \n");
            }
            else
            {
                varname.Append("              AND OpenAR.Type IN (3)                                 \n");
            }

            if (objPropContracts.IsOverDue)
            {
                varname.Append("        AND ISNULL(DATEDIFF(Day, OpenAR.Due, GETDATE()),0) > 0      \n");
            }

            if (includeCredit)
            {

                varname.Append("     UNION                                                          \n");
                varname.Append("		SELECT                                                      \n");
                varname.Append("			OpenAR.Loc,                                             \n");
                varname.Append("            OpenAR.fDate,                                           \n");
                varname.Append("            'Credit Invoice' AS Type,                               \n");
                varname.Append("            CONVERT(VARCHAR(50),OpenAR.Ref) AS Ref,                 \n");
                varname.Append("            OpenAR.fDesc,                                           \n");
                varname.Append("            OpenAR.Balance,                                         \n");
                varname.Append("            ISNULL(DATEDIFF(Day, OpenAR.Due, GETDATE()),0) AS Days  \n");
                varname.Append("		FROM OpenAR                                                 \n");
                varname.Append("			 INNER JOIN Dep  ON Dep.Ref = OpenAR.Ref             	\n");
                varname.Append("		WHERE OpenAR.Loc = '" + objPropContracts.Loc + "'           \n");
                varname.Append("			AND OpenAR.type = 1                                     \n");
                varname.Append("			AND OpenAR.Balance <> 0  AND  OpenAR.InvoiceID IS NULL  \n");
            }

            varname.Append("   ) AS OpenARInv               \n");
            varname.Append("   ORDER BY fDate, Ref          \n");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetCustomerStatementInvoices(GetCustomerStatementInvoicesParam _GetCustomerStatementInvoices, string ConnectionString, bool includeCredit)
        {
            StringBuilder varname = new StringBuilder();

            varname.Append("       SELECT * FROM (         \n");
            varname.Append("           SELECT   OpenAR.Loc,                                         \n");
            varname.Append("                    OpenAR.fDate,                                       \n");
            varname.Append("                    'Invoice' as Type,                                  \n");
            varname.Append("                    CONVERT(VARCHAR(50),OpenAR.Ref) AS Ref,             \n");
            varname.Append("                    OpenAR.fDesc,                                       \n");
            varname.Append("                    OpenAR.Balance,                                     \n");
            varname.Append("                    ISNULL(DATEDIFF(Day, ISNULL(OpenAR.Due, Invoice.DDate), GETDATE()),0) AS Days   \n");
            varname.Append("               FROM Invoice INNER JOIN OpenAR ON Invoice.Ref = OpenAR.Ref AND OpenAR.Type = 0   \n");
            varname.Append("               WHERE	    OpenAR.Loc = '" + _GetCustomerStatementInvoices.Loc + "' \n");
            varname.Append("                   	AND Invoice.Status <> 1                             \n");
            varname.Append("                    AND Invoice.Status <> 2                             \n");
            if (includeCredit)
            {
                varname.Append("                    AND OpenAR.Balance <> 0                         \n");
            }
            else
            {
                varname.Append("                    AND OpenAR.Balance > 0                          \n");
            }

            if (_GetCustomerStatementInvoices.IsOverDue)
            {
                varname.Append("        AND ISNULL(DATEDIFF(Day, ISNULL(OpenAR.Due, Invoice.DDate), GETDATE()),0) > 0      \n");
            }
            varname.Append("       UNION       \n");
            varname.Append("       SELECT   OpenAR.Loc,                                             \n");
            varname.Append("                OpenAR.fDate,                                           \n");
            varname.Append("                    (CASE OpenAR.Type                                   \n");
            varname.Append("                        WHEN 2 THEN 'Credit Invoice'                    \n");
            varname.Append("                        WHEN 3 THEN 'Deposit'                           \n");
            varname.Append("                    END)                                                \n");
            varname.Append("                AS Type,                                                \n");
            varname.Append("                    (CASE OpenAR.Type                                   \n");
            varname.Append("                        WHEN 2 THEN ReceivedPayment.CheckNumber         \n");
            varname.Append("                        ELSE CONVERT(VARCHAR(50),OpenAR.Ref)            \n");
            varname.Append("                    END)                                                \n");
            varname.Append("                AS Ref,                                                 \n");
            varname.Append("                OpenAR.fDesc,                                           \n");
            varname.Append("                OpenAR.Balance,                                         \n");
            varname.Append("                ISNULL(DATEDIFF(Day, OpenAR.Due, GETDATE()),0) AS Days  \n");
            varname.Append("           FROM OpenAR                                                  \n");
            varname.Append("                LEFT JOIN ReceivedPayment ON ReceivedPayment.ID = OpenAR.Ref AND OpenAR.Type = 2    \n");
            varname.Append("           WHERE  OpenAR.Loc = '" + _GetCustomerStatementInvoices.Loc + "'           \n");
            varname.Append("              AND OpenAR.Balance <> 0                                   \n");

            if (includeCredit)
            {
                varname.Append("              AND OpenAR.Type IN (2,3)                              \n");
            }
            else
            {
                varname.Append("              AND OpenAR.Type IN (3)                                 \n");
            }

            if (_GetCustomerStatementInvoices.IsOverDue)
            {
                varname.Append("        AND ISNULL(DATEDIFF(Day, OpenAR.Due, GETDATE()),0) > 0      \n");
            }

            if (includeCredit)
            {

                varname.Append("     UNION                                                          \n");
                varname.Append("		SELECT                                                      \n");
                varname.Append("			OpenAR.Loc,                                             \n");
                varname.Append("            OpenAR.fDate,                                           \n");
                varname.Append("            'Credit Invoice' AS Type,                               \n");
                varname.Append("            CONVERT(VARCHAR(50),OpenAR.Ref) AS Ref,                 \n");
                varname.Append("            OpenAR.fDesc,                                           \n");
                varname.Append("            OpenAR.Balance,                                         \n");
                varname.Append("            ISNULL(DATEDIFF(Day, OpenAR.Due, GETDATE()),0) AS Days  \n");
                varname.Append("		FROM OpenAR                                                 \n");
                varname.Append("			 INNER JOIN Dep  ON Dep.Ref = OpenAR.Ref             	\n");
                varname.Append("		WHERE OpenAR.Loc = '" + _GetCustomerStatementInvoices.Loc + "'           \n");
                varname.Append("			AND OpenAR.type = 1                                     \n");
                varname.Append("			AND OpenAR.Balance <> 0  AND  OpenAR.InvoiceID IS NULL  \n");
            }

            varname.Append("   ) AS OpenARInv               \n");
            varname.Append("   ORDER BY fDate, Ref          \n");

            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustomerStatementInvoicesByOwner(Contracts objPropContracts, bool includeCredit)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT * FROM (                                          \n");
            sb.Append("	SELECT                                                  \n");
            sb.Append("		OpenAR.Loc,                                         \n");
            sb.Append("		OpenAR.fDate,                                       \n");
            sb.Append("		'Invoice' as Type,                                  \n");
            sb.Append("		CONVERT(VARCHAR(50),OpenAR.Ref) AS Ref,             \n");
            sb.Append("		OpenAR.fDesc,                                       \n");
            sb.Append("		OpenAR.Balance,                                     \n");
            sb.Append("		ISNULL(DATEDIFF(Day, ISNULL(OpenAR.Due, Invoice.DDate), GETDATE()),0) AS Days   \n");
            sb.Append("	FROM Invoice                                            \n");
            sb.Append("		INNER JOIN OpenAR ON Invoice.Ref = OpenAR.Ref AND OpenAR.Type = 0 \n");
            sb.Append("		INNER JOIN Loc ON Loc.Loc = OpenAR.Loc              \n");
            sb.Append("	WHERE Loc.NoCustomerStatement <> 1 AND Invoice.Status <> 1 \n");
            sb.Append("		AND Invoice.Status <> 2                             \n");
            if (includeCredit)
            {
                sb.Append("		AND OpenAR.Balance <> 0                         \n");
            }
            else
            {
                sb.Append("		AND OpenAR.Balance > 0                          \n");
            }

            if (objPropContracts.Owner > 0)
            {
                sb.Append("		AND Loc.Owner = " + objPropContracts.Owner + "  \n");
            }

            sb.Append(" UNION                                                   \n");
            sb.Append("	SELECT                                                  \n");
            sb.Append("		OpenAR.Loc,                                         \n");
            sb.Append("		OpenAR.fDate,                                       \n");
            sb.Append("		(CASE OpenAR.Type                                   \n");
            sb.Append("			WHEN 2 THEN 'Credit Invoice'                    \n");
            sb.Append("			WHEN 3 THEN 'Deposit'                           \n");
            sb.Append("		END) AS Type,                                       \n");
            sb.Append("		(CASE OpenAR.Type                                   \n");
            sb.Append("			WHEN 2 THEN ReceivedPayment.CheckNumber         \n");
            sb.Append("			ELSE CONVERT(VARCHAR(50),OpenAR.Ref)            \n");
            sb.Append("		END) AS Ref,                                        \n");
            sb.Append("		OpenAR.fDesc,                                       \n");
            sb.Append("		OpenAR.Balance,                                     \n");
            sb.Append("		ISNULL(DATEDIFF(Day, OpenAR.Due, GETDATE()),0) AS Days  \n");
            sb.Append("	FROM OpenAR                                             \n");
            sb.Append("		LEFT JOIN ReceivedPayment ON ReceivedPayment.ID = OpenAR.Ref AND OpenAR.Type = 2    \n");
            sb.Append("		INNER JOIN Loc ON Loc.Loc = OpenAR.Loc              \n");
            sb.Append("	WHERE  OpenAR.Balance <> 0 AND Loc.NoCustomerStatement <> 1  \n");
            if (includeCredit)
            {
                sb.Append("		AND OpenAR.Type IN (2,3)                        \n");
            }
            else
            {
                sb.Append("		AND OpenAR.Type IN (3)                          \n");
            }

            if (objPropContracts.Owner > 0)
            {
                sb.Append("		AND Loc.Owner = " + objPropContracts.Owner + "  \n");
            }

            if (includeCredit)
            {

                sb.Append(" UNION                                               \n");
                sb.Append(" SELECT                                              \n");
                sb.Append("		OpenAR.Loc,                                     \n");
                sb.Append("     OpenAR.fDate,                                   \n");
                sb.Append("     'Credit Invoice' AS Type,                       \n");
                sb.Append("     CONVERT(VARCHAR(50),OpenAR.Ref) AS Ref,         \n");
                sb.Append("     OpenAR.fDesc,                                   \n");
                sb.Append("     OpenAR.Balance,                                 \n");
                sb.Append("     ISNULL(DATEDIFF(Day, OpenAR.Due, GETDATE()),0) AS Days  \n");
                sb.Append("	FROM OpenAR                                         \n");
                sb.Append("		INNER JOIN Dep  ON Dep.Ref = OpenAR.Ref         \n");
                sb.Append("		INNER JOIN Loc ON Loc.Loc = OpenAR.Loc          \n");
                sb.Append("	WHERE OpenAR.type = 1                               \n");
                sb.Append("		AND OpenAR.Balance <> 0 and  OpenAR.InvoiceID IS NULL   \n");
                if (objPropContracts.Owner > 0)
                {
                    sb.Append("		AND Loc.Owner = " + objPropContracts.Owner + "     \n");
                }
            }

            sb.Append("   ) AS OpenARInv                                        \n");
            sb.Append("ORDER BY fDate, Ref                                      \n");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //By Customer (Owner)
        public DataSet GetCustomerStatementInvoicesByOwnerByCustId(Contracts objPropContracts, bool includeCredit, String Customer)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT * FROM (                                          \n");
            sb.Append("	SELECT                                                  \n");
            sb.Append("		OpenAR.Loc,                                         \n");
            sb.Append("		OpenAR.fDate,                                       \n");
            sb.Append("		'Invoice' as Type,                                  \n");
            sb.Append("		CONVERT(VARCHAR(50),OpenAR.Ref) AS Ref,             \n");
            sb.Append("		OpenAR.fDesc,                                       \n");
            sb.Append("		OpenAR.Balance,                                     \n");
            sb.Append("		ISNULL(DATEDIFF(Day, ISNULL(OpenAR.Due, Invoice.DDate), GETDATE()),0) AS Days   \n");
            sb.Append("	FROM Invoice                                            \n");
            sb.Append("		INNER JOIN OpenAR ON Invoice.Ref = OpenAR.Ref AND OpenAR.Type = 0 \n");
            sb.Append("		INNER JOIN Loc ON Loc.Loc = OpenAR.Loc              \n");
            sb.Append("	WHERE Loc.NoCustomerStatement <> 1 AND Invoice.Status <> 1 \n");
            sb.Append("		AND Invoice.Status <> 2                             \n");
            if (includeCredit)
            {
                sb.Append("		AND OpenAR.Balance <> 0                         \n");
            }
            else
            {
                sb.Append("		AND OpenAR.Balance > 0                          \n");
            }
            sb.Append("AND Loc.Owner IN(" + Customer + ")\n");
            if (objPropContracts.Owner > 0)
            {
                sb.Append("		AND Loc.Owner = " + objPropContracts.Owner + "  \n");
            }

            sb.Append(" UNION                                                   \n");
            sb.Append("	SELECT                                                  \n");
            sb.Append("		OpenAR.Loc,                                         \n");
            sb.Append("		OpenAR.fDate,                                       \n");
            sb.Append("		(CASE OpenAR.Type                                   \n");
            sb.Append("			WHEN 2 THEN 'Credit Invoice'                    \n");
            sb.Append("			WHEN 3 THEN 'Deposit'                           \n");
            sb.Append("		END) AS Type,                                       \n");
            sb.Append("		(CASE OpenAR.Type                                   \n");
            sb.Append("			WHEN 2 THEN ReceivedPayment.CheckNumber         \n");
            sb.Append("			ELSE CONVERT(VARCHAR(50),OpenAR.Ref)            \n");
            sb.Append("		END) AS Ref,                                        \n");
            sb.Append("		OpenAR.fDesc,                                       \n");
            sb.Append("		OpenAR.Balance,                                     \n");
            sb.Append("		ISNULL(DATEDIFF(Day, OpenAR.Due, GETDATE()),0) AS Days  \n");
            sb.Append("	FROM OpenAR                                             \n");
            sb.Append("		LEFT JOIN ReceivedPayment ON ReceivedPayment.ID = OpenAR.Ref AND OpenAR.Type = 2    \n");
            sb.Append("		INNER JOIN Loc ON Loc.Loc = OpenAR.Loc              \n");
            sb.Append("	WHERE  OpenAR.Balance <> 0 AND Loc.NoCustomerStatement <> 1  \n");
            if (includeCredit)
            {
                sb.Append("		AND OpenAR.Type IN (2,3)                        \n");
            }
            else
            {
                sb.Append("		AND OpenAR.Type IN (3)                          \n");
            }
            sb.Append("AND Loc.Owner IN(" + Customer + ")\n");
            if (objPropContracts.Owner > 0)
            {
                sb.Append("		AND Loc.Owner = " + objPropContracts.Owner + "  \n");
            }

            if (includeCredit)
            {

                sb.Append(" UNION                                               \n");
                sb.Append(" SELECT                                              \n");
                sb.Append("		OpenAR.Loc,                                     \n");
                sb.Append("     OpenAR.fDate,                                   \n");
                sb.Append("     'Credit Invoice' AS Type,                       \n");
                sb.Append("     CONVERT(VARCHAR(50),OpenAR.Ref) AS Ref,         \n");
                sb.Append("     OpenAR.fDesc,                                   \n");
                sb.Append("     OpenAR.Balance,                                 \n");
                sb.Append("     ISNULL(DATEDIFF(Day, OpenAR.Due, GETDATE()),0) AS Days  \n");
                sb.Append("	FROM OpenAR                                         \n");
                sb.Append("		INNER JOIN Dep  ON Dep.Ref = OpenAR.Ref         \n");
                sb.Append("		INNER JOIN Loc ON Loc.Loc = OpenAR.Loc          \n");
                sb.Append("	WHERE OpenAR.type = 1                               \n");
                sb.Append("		AND OpenAR.Balance <> 0 and  OpenAR.InvoiceID IS NULL   \n");
                sb.Append("AND Loc.Owner IN(" + Customer + ")\n");
                if (objPropContracts.Owner > 0)
                {
                    sb.Append("		AND Loc.Owner = " + objPropContracts.Owner + "     \n");
                }
            }

            sb.Append("   ) AS OpenARInv                                        \n");
            sb.Append("ORDER BY fDate, Ref                                      \n");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustomerStatementInvoicesByLocation(Contracts objPropContracts, bool includeCredit)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT * FROM (                                          \n");
            sb.Append("	SELECT                                                  \n");
            sb.Append("		OpenAR.Loc,                                         \n");
            sb.Append("		OpenAR.fDate,                                       \n");
            sb.Append("		'Invoice' as Type,                                  \n");
            sb.Append("		CONVERT(VARCHAR(50),OpenAR.Ref) AS Ref,             \n");
            sb.Append("		OpenAR.fDesc,                                       \n");
            sb.Append("		OpenAR.Balance,                                     \n");
            sb.Append("		ISNULL(DATEDIFF(Day, ISNULL(OpenAR.Due, Invoice.DDate), GETDATE()),0) AS Days   \n");
            sb.Append("	FROM Invoice                                            \n");
            sb.Append("		INNER JOIN OpenAR ON Invoice.Ref = OpenAR.Ref AND OpenAR.Type = 0 \n");
            sb.Append("		INNER JOIN Loc ON Loc.Loc = OpenAR.Loc              \n");
            sb.Append("	WHERE Loc.NoCustomerStatement <> 1 AND Invoice.Status <> 1    \n");
            sb.Append("		AND Invoice.Status <> 2                             \n");
            if (includeCredit)
            {
                sb.Append("		AND OpenAR.Balance <> 0                         \n");
            }
            else
            {
                sb.Append("		AND OpenAR.Balance > 0                          \n");
            }

            if (!string.IsNullOrEmpty(objPropContracts.LocationIDs))
            {
                sb.Append("		AND Loc.Loc IN(" + objPropContracts.LocationIDs + ")  \n");
            }

            sb.Append(" UNION                                                   \n");
            sb.Append("	SELECT                                                  \n");
            sb.Append("		OpenAR.Loc,                                         \n");
            sb.Append("		OpenAR.fDate,                                       \n");
            sb.Append("		(CASE OpenAR.Type                                   \n");
            sb.Append("			WHEN 2 THEN 'Credit Invoice'                    \n");
            sb.Append("			WHEN 3 THEN 'Deposit'                           \n");
            sb.Append("		END) AS Type,                                       \n");
            sb.Append("		(CASE OpenAR.Type                                   \n");
            sb.Append("			WHEN 2 THEN ReceivedPayment.CheckNumber         \n");
            sb.Append("			ELSE CONVERT(VARCHAR(50),OpenAR.Ref)            \n");
            sb.Append("		END) AS Ref,                                        \n");
            sb.Append("		OpenAR.fDesc,                                       \n");
            sb.Append("		OpenAR.Balance,                                     \n");
            sb.Append("		ISNULL(DATEDIFF(Day, OpenAR.Due, GETDATE()),0) AS Days  \n");
            sb.Append("	FROM OpenAR                                             \n");
            sb.Append("		LEFT JOIN ReceivedPayment ON ReceivedPayment.ID = OpenAR.Ref AND OpenAR.Type = 2    \n");
            sb.Append("		INNER JOIN Loc ON Loc.Loc = OpenAR.Loc              \n");
            sb.Append("	WHERE  OpenAR.Balance <> 0 AND Loc.NoCustomerStatement <> 1 \n");
            if (includeCredit)
            {
                sb.Append("		AND OpenAR.Type IN (2,3)                        \n");
            }
            else
            {
                sb.Append("		AND OpenAR.Type IN (3)                          \n");
            }

            if (!string.IsNullOrEmpty(objPropContracts.LocationIDs))
            {
                sb.Append("		AND Loc.Loc IN(" + objPropContracts.LocationIDs + ")  \n");
            }

            if (includeCredit)
            {

                sb.Append(" UNION                                               \n");
                sb.Append(" SELECT                                              \n");
                sb.Append("		OpenAR.Loc,                                     \n");
                sb.Append("     OpenAR.fDate,                                   \n");
                sb.Append("     'Credit Invoice' AS Type,                       \n");
                sb.Append("     CONVERT(VARCHAR(50),OpenAR.Ref) AS Ref,         \n");
                sb.Append("     OpenAR.fDesc,                                   \n");
                sb.Append("     OpenAR.Balance,                                 \n");
                sb.Append("     ISNULL(DATEDIFF(Day, OpenAR.Due, GETDATE()),0) AS Days  \n");
                sb.Append("	FROM OpenAR                                         \n");
                sb.Append("		INNER JOIN Dep  ON Dep.Ref = OpenAR.Ref         \n");
                sb.Append("		INNER JOIN Loc ON Loc.Loc = OpenAR.Loc          \n");
                sb.Append("	WHERE OpenAR.type = 1                               \n");
                sb.Append("		AND OpenAR.Balance <> 0 and  OpenAR.InvoiceID IS NULL   \n");
                if (!string.IsNullOrEmpty(objPropContracts.LocationIDs))
                {
                    sb.Append("		AND Loc.Loc IN(" + objPropContracts.LocationIDs + ")  \n");
                }
            }

            sb.Append("   ) AS OpenARInv                                        \n");
            sb.Append("ORDER BY fDate, Ref                                      \n");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetCustomerStatementInvoicesByLocation(GetCustStatementInvByLocationParam _GetCustStatementInvByLocation, string ConnectionString, bool includeCredit)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT * FROM (                                          \n");
            sb.Append("	SELECT                                                  \n");
            sb.Append("		OpenAR.Loc,                                         \n");
            sb.Append("		OpenAR.fDate,                                       \n");
            sb.Append("		'Invoice' as Type,                                  \n");
            sb.Append("		CONVERT(VARCHAR(50),OpenAR.Ref) AS Ref,             \n");
            sb.Append("		OpenAR.fDesc,                                       \n");
            sb.Append("		OpenAR.Balance,                                     \n");
            sb.Append("		ISNULL(DATEDIFF(Day, ISNULL(OpenAR.Due, Invoice.DDate), GETDATE()),0) AS Days   \n");
            sb.Append("	FROM Invoice                                            \n");
            sb.Append("		INNER JOIN OpenAR ON Invoice.Ref = OpenAR.Ref AND OpenAR.Type = 0 \n");
            sb.Append("		INNER JOIN Loc ON Loc.Loc = OpenAR.Loc              \n");
            sb.Append("	WHERE Loc.NoCustomerStatement <> 1 AND Invoice.Status <> 1    \n");
            sb.Append("		AND Invoice.Status <> 2                             \n");
            if (includeCredit)
            {
                sb.Append("		AND OpenAR.Balance <> 0                         \n");
            }
            else
            {
                sb.Append("		AND OpenAR.Balance > 0                          \n");
            }

            if (!string.IsNullOrEmpty(_GetCustStatementInvByLocation.LocationIDs))
            {
                sb.Append("		AND Loc.Loc IN(" + _GetCustStatementInvByLocation.LocationIDs + ")  \n");
            }

            sb.Append(" UNION                                                   \n");
            sb.Append("	SELECT                                                  \n");
            sb.Append("		OpenAR.Loc,                                         \n");
            sb.Append("		OpenAR.fDate,                                       \n");
            sb.Append("		(CASE OpenAR.Type                                   \n");
            sb.Append("			WHEN 2 THEN 'Credit Invoice'                    \n");
            sb.Append("			WHEN 3 THEN 'Deposit'                           \n");
            sb.Append("		END) AS Type,                                       \n");
            sb.Append("		(CASE OpenAR.Type                                   \n");
            sb.Append("			WHEN 2 THEN ReceivedPayment.CheckNumber         \n");
            sb.Append("			ELSE CONVERT(VARCHAR(50),OpenAR.Ref)            \n");
            sb.Append("		END) AS Ref,                                        \n");
            sb.Append("		OpenAR.fDesc,                                       \n");
            sb.Append("		OpenAR.Balance,                                     \n");
            sb.Append("		ISNULL(DATEDIFF(Day, OpenAR.Due, GETDATE()),0) AS Days  \n");
            sb.Append("	FROM OpenAR                                             \n");
            sb.Append("		LEFT JOIN ReceivedPayment ON ReceivedPayment.ID = OpenAR.Ref AND OpenAR.Type = 2    \n");
            sb.Append("		INNER JOIN Loc ON Loc.Loc = OpenAR.Loc              \n");
            sb.Append("	WHERE  OpenAR.Balance <> 0 AND Loc.NoCustomerStatement <> 1 \n");
            if (includeCredit)
            {
                sb.Append("		AND OpenAR.Type IN (2,3)                        \n");
            }
            else
            {
                sb.Append("		AND OpenAR.Type IN (3)                          \n");
            }

            if (!string.IsNullOrEmpty(_GetCustStatementInvByLocation.LocationIDs))
            {
                sb.Append("		AND Loc.Loc IN(" + _GetCustStatementInvByLocation.LocationIDs + ")  \n");
            }

            if (includeCredit)
            {

                sb.Append(" UNION                                               \n");
                sb.Append(" SELECT                                              \n");
                sb.Append("		OpenAR.Loc,                                     \n");
                sb.Append("     OpenAR.fDate,                                   \n");
                sb.Append("     'Credit Invoice' AS Type,                       \n");
                sb.Append("     CONVERT(VARCHAR(50),OpenAR.Ref) AS Ref,         \n");
                sb.Append("     OpenAR.fDesc,                                   \n");
                sb.Append("     OpenAR.Balance,                                 \n");
                sb.Append("     ISNULL(DATEDIFF(Day, OpenAR.Due, GETDATE()),0) AS Days  \n");
                sb.Append("	FROM OpenAR                                         \n");
                sb.Append("		INNER JOIN Dep  ON Dep.Ref = OpenAR.Ref         \n");
                sb.Append("		INNER JOIN Loc ON Loc.Loc = OpenAR.Loc          \n");
                sb.Append("	WHERE OpenAR.type = 1                               \n");
                sb.Append("		AND OpenAR.Balance <> 0 and  OpenAR.InvoiceID IS NULL   \n");
                if (!string.IsNullOrEmpty(_GetCustStatementInvByLocation.LocationIDs))
                {
                    sb.Append("		AND Loc.Loc IN(" + _GetCustStatementInvByLocation.LocationIDs + ")  \n");
                }
            }

            sb.Append("   ) AS OpenARInv                                        \n");
            sb.Append("ORDER BY fDate, Ref                                      \n");

            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustomerStatementInvoicesSouthern(Contracts objPropContracts, bool includeCredit)
        {
            StringBuilder varname = new StringBuilder();

            varname.Append("       SELECT * FROM (         \n");
            varname.Append("           SELECT   OpenAR.Loc,                             \n");
            varname.Append("                    OpenAR.fDate,                           \n");
            varname.Append("                    'Invoice' as Type,                      \n");
            varname.Append("                    CONVERT(VARCHAR(50),OpenAR.Ref) AS Ref, \n");
            varname.Append("                    OpenAR.fDesc,                           \n");
            varname.Append("                    OpenAR.Balance,                          \n");
            varname.Append("                    ISNULL(DATEDIFF(Day, ISNULL(OpenAR.Due, Invoice.DDate), GETDATE()),0) AS Days   \n");
            varname.Append("               FROM Invoice INNER JOIN OpenAR ON Invoice.Ref = OpenAR.Ref AND OpenAR.Type = 0   \n");
            varname.Append("               WHERE	    OpenAR.Loc = " + objPropContracts.Loc + "                           \n");
            varname.Append("                   	AND Invoice.Status <> 1                                                     \n");
            varname.Append("                    AND Invoice.Status <> 2                                                     \n");
            if (includeCredit)
            {
                varname.Append("                    AND OpenAR.Balance <> 0                                                 \n");
            }
            else
            {
                varname.Append("                    AND OpenAR.Balance > 0                                                 \n");
            }

            if (objPropContracts.IsOverDue)
            {
                varname.Append("        AND ISNULL(DATEDIFF(Day, ISNULL(OpenAR.Due, Invoice.DDate), GETDATE()),0) > 0      \n");
            }
            varname.Append("       UNION       \n");
            varname.Append("       SELECT   OpenAR.Loc,                                             \n");
            varname.Append("                OpenAR.fDate,                                           \n");
            varname.Append("                    (CASE OpenAR.Type                                   \n");
            varname.Append("                        WHEN 2 THEN 'Credit Invoice'                    \n");
            varname.Append("                        WHEN 3 THEN 'Deposit'                           \n");
            varname.Append("                    END)                                                \n");
            varname.Append("                AS Type,                                                \n");
            varname.Append("                    (CASE OpenAR.Type                                   \n");
            varname.Append("                        WHEN 2 THEN ReceivedPayment.CheckNumber         \n");
            varname.Append("                        ELSE CONVERT(VARCHAR(50),OpenAR.Ref)            \n");
            varname.Append("                    END)                                                \n");
            varname.Append("                AS Ref,                                                 \n");
            varname.Append("                OpenAR.fDesc,                                           \n");
            varname.Append("                OpenAR.Balance,                                         \n");
            varname.Append("                ISNULL(DATEDIFF(Day, OpenAR.Due, GETDATE()),0) AS Days  \n");
            varname.Append("           FROM OpenAR                                                  \n");
            varname.Append("                LEFT JOIN ReceivedPayment ON ReceivedPayment.ID = OpenAR.Ref AND OpenAR.Type = 2    \n");
            varname.Append("           WHERE  OpenAR.Loc = " + objPropContracts.Loc + "             \n");
            varname.Append("              AND OpenAR.Balance <> 0                                   \n");

            if (includeCredit)
            {
                varname.Append("              AND OpenAR.Type IN (2,3)                              \n");
            }
            else
            {
                varname.Append("              AND OpenAR.Type IN (3)                                \n");
            }

            if (objPropContracts.IsOverDue)
            {
                varname.Append("        AND ISNULL(DATEDIFF(Day, OpenAR.Due, GETDATE()),0) > 0      \n");
            }

            if (includeCredit)
            {

                varname.Append("     UNION                                                          \n");
                varname.Append("		SELECT                                                      \n");
                varname.Append("			OpenAR.Loc,                                             \n");
                varname.Append("            OpenAR.fDate,                                           \n");
                varname.Append("            'Credit Invoice' AS Type,                               \n");
                varname.Append("            CONVERT(VARCHAR(50),OpenAR.Ref) AS Ref,                 \n");
                varname.Append("            OpenAR.fDesc,                                           \n");
                varname.Append("            OpenAR.Balance,                                         \n");
                varname.Append("            ISNULL(DATEDIFF(Day, OpenAR.Due, GETDATE()),0) AS Days  \n");
                varname.Append("		FROM OpenAR                                                 \n");
                varname.Append("			 INNER JOIN Dep  ON Dep.Ref = OpenAR.Ref             	\n");
                varname.Append("		WHERE OpenAR.Loc = " + objPropContracts.Loc + "             \n");
                varname.Append("			AND OpenAR.type =1                                      \n");
                varname.Append("			AND OpenAR.Balance <>0                                  \n");
                varname.Append("			AND InvoiceID IS NULL                                   \n");

                if (objPropContracts.IsOverDue)
                {
                    varname.Append("        AND ISNULL(DATEDIFF(Day, OpenAR.Due, GETDATE()),0) > 0  \n");
                }
            }

            varname.Append("   ) AS OpenARInv      \n");
            varname.Append("   ORDER BY fDate        \n");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetCustomerStatementInvoicesSouthern(GetCustomerStatementInvoicesSouthernParam _GetCustStatementInvSouthern, string ConnectionString, bool includeCredit)
        {
            StringBuilder varname = new StringBuilder();

            varname.Append("       SELECT * FROM (         \n");
            varname.Append("           SELECT   OpenAR.Loc,                             \n");
            varname.Append("                    OpenAR.fDate,                           \n");
            varname.Append("                    'Invoice' as Type,                      \n");
            varname.Append("                    CONVERT(VARCHAR(50),OpenAR.Ref) AS Ref, \n");
            varname.Append("                    OpenAR.fDesc,                           \n");
            varname.Append("                    OpenAR.Balance,                          \n");
            varname.Append("                    ISNULL(DATEDIFF(Day, ISNULL(OpenAR.Due, Invoice.DDate), GETDATE()),0) AS Days   \n");
            varname.Append("               FROM Invoice INNER JOIN OpenAR ON Invoice.Ref = OpenAR.Ref AND OpenAR.Type = 0   \n");
            varname.Append("               WHERE	    OpenAR.Loc = " + _GetCustStatementInvSouthern.Loc + "                           \n");
            varname.Append("                   	AND Invoice.Status <> 1                                                     \n");
            varname.Append("                    AND Invoice.Status <> 2                                                     \n");
            if (includeCredit)
            {
                varname.Append("                    AND OpenAR.Balance <> 0                                                 \n");
            }
            else
            {
                varname.Append("                    AND OpenAR.Balance > 0                                                 \n");
            }

            if (_GetCustStatementInvSouthern.IsOverDue)
            {
                varname.Append("        AND ISNULL(DATEDIFF(Day, ISNULL(OpenAR.Due, Invoice.DDate), GETDATE()),0) > 0      \n");
            }
            varname.Append("       UNION       \n");
            varname.Append("       SELECT   OpenAR.Loc,                                             \n");
            varname.Append("                OpenAR.fDate,                                           \n");
            varname.Append("                    (CASE OpenAR.Type                                   \n");
            varname.Append("                        WHEN 2 THEN 'Credit Invoice'                    \n");
            varname.Append("                        WHEN 3 THEN 'Deposit'                           \n");
            varname.Append("                    END)                                                \n");
            varname.Append("                AS Type,                                                \n");
            varname.Append("                    (CASE OpenAR.Type                                   \n");
            varname.Append("                        WHEN 2 THEN ReceivedPayment.CheckNumber         \n");
            varname.Append("                        ELSE CONVERT(VARCHAR(50),OpenAR.Ref)            \n");
            varname.Append("                    END)                                                \n");
            varname.Append("                AS Ref,                                                 \n");
            varname.Append("                OpenAR.fDesc,                                           \n");
            varname.Append("                OpenAR.Balance,                                         \n");
            varname.Append("                ISNULL(DATEDIFF(Day, OpenAR.Due, GETDATE()),0) AS Days  \n");
            varname.Append("           FROM OpenAR                                                  \n");
            varname.Append("                LEFT JOIN ReceivedPayment ON ReceivedPayment.ID = OpenAR.Ref AND OpenAR.Type = 2    \n");
            varname.Append("           WHERE  OpenAR.Loc = " + _GetCustStatementInvSouthern.Loc + "             \n");
            varname.Append("              AND OpenAR.Balance <> 0                                   \n");

            if (includeCredit)
            {
                varname.Append("              AND OpenAR.Type IN (2,3)                              \n");
            }
            else
            {
                varname.Append("              AND OpenAR.Type IN (3)                                \n");
            }

            if (_GetCustStatementInvSouthern.IsOverDue)
            {
                varname.Append("        AND ISNULL(DATEDIFF(Day, OpenAR.Due, GETDATE()),0) > 0      \n");
            }

            if (includeCredit)
            {

                varname.Append("     UNION                                                          \n");
                varname.Append("		SELECT                                                      \n");
                varname.Append("			OpenAR.Loc,                                             \n");
                varname.Append("            OpenAR.fDate,                                           \n");
                varname.Append("            'Credit Invoice' AS Type,                               \n");
                varname.Append("            CONVERT(VARCHAR(50),OpenAR.Ref) AS Ref,                 \n");
                varname.Append("            OpenAR.fDesc,                                           \n");
                varname.Append("            OpenAR.Balance,                                         \n");
                varname.Append("            ISNULL(DATEDIFF(Day, OpenAR.Due, GETDATE()),0) AS Days  \n");
                varname.Append("		FROM OpenAR                                                 \n");
                varname.Append("			 INNER JOIN Dep  ON Dep.Ref = OpenAR.Ref             	\n");
                varname.Append("		WHERE OpenAR.Loc = " + _GetCustStatementInvSouthern.Loc + "             \n");
                varname.Append("			AND OpenAR.type =1                                      \n");
                varname.Append("			AND OpenAR.Balance <>0                                  \n");
                varname.Append("			AND InvoiceID IS NULL                                   \n");

                if (_GetCustStatementInvSouthern.IsOverDue)
                {
                    varname.Append("        AND ISNULL(DATEDIFF(Day, OpenAR.Due, GETDATE()),0) > 0  \n");
                }
            }

            varname.Append("   ) AS OpenARInv      \n");
            varname.Append("   ORDER BY fDate        \n");

            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustomerStatement(Contracts objPropContracts, bool includeCredit, bool includeCustomerCredit)
        {
            try
            {
                SqlParameter paraOwner = new SqlParameter();
                paraOwner.ParameterName = "Owner";
                paraOwner.SqlDbType = SqlDbType.Int;
                paraOwner.Value = objPropContracts.Owner;

                SqlParameter paraStrOwners = new SqlParameter();
                paraStrOwners.ParameterName = "strOwners";
                paraStrOwners.SqlDbType = SqlDbType.VarChar;
                paraStrOwners.Value = objPropContracts.strOwners;

                SqlParameter paraIsOverDue = new SqlParameter();
                paraIsOverDue.ParameterName = "IsOverDue";
                paraIsOverDue.SqlDbType = SqlDbType.Bit;
                paraIsOverDue.Value = objPropContracts.IsOverDue;

                SqlParameter paramEN = new SqlParameter();
                paramEN.ParameterName = "EN";
                paramEN.SqlDbType = SqlDbType.Int;
                paramEN.Value = objPropContracts.EN;

                SqlParameter paramUserID = new SqlParameter();
                paramUserID.ParameterName = "UserID";
                paramUserID.SqlDbType = SqlDbType.Int;
                paramUserID.Value = Convert.ToInt32(objPropContracts.UserID);

                SqlParameter paramIncludeCredit = new SqlParameter();
                paramIncludeCredit.ParameterName = "IncludeCredit";
                paramIncludeCredit.SqlDbType = SqlDbType.Bit;
                paramIncludeCredit.Value = includeCredit;

                SqlParameter paramIncludeCustomerCredit = new SqlParameter();
                paramIncludeCustomerCredit.ParameterName = "IncludeCustomerCredit";
                paramIncludeCustomerCredit.SqlDbType = SqlDbType.Bit;
                paramIncludeCustomerCredit.Value = includeCustomerCredit;

                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetCustomerStatement", paraOwner, paraStrOwners, paraIsOverDue, paramEN, paramUserID, paramIncludeCredit, paramIncludeCustomerCredit);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get Customer Statement By Customer 
        public DataSet GetCustomerStatementByCustomer(Contracts objPropContracts, bool includeCredit, bool includeCustomerCredit, string Customer)
        {
            try
            {

                //string.Join(",", Customer);
                //string CStype = string.Join(",", Customer);

                SqlParameter paraOwner = new SqlParameter();
                paraOwner.ParameterName = "Owner";
                paraOwner.SqlDbType = SqlDbType.Int;
                paraOwner.Value = objPropContracts.Owner;

                SqlParameter paraStrOwners = new SqlParameter();
                paraStrOwners.ParameterName = "strOwners";
                paraStrOwners.SqlDbType = SqlDbType.VarChar;
                paraStrOwners.Value = objPropContracts.strOwners;

                SqlParameter paraIsOverDue = new SqlParameter();
                paraIsOverDue.ParameterName = "IsOverDue";
                paraIsOverDue.SqlDbType = SqlDbType.Bit;
                paraIsOverDue.Value = objPropContracts.IsOverDue;

                SqlParameter paramEN = new SqlParameter();
                paramEN.ParameterName = "EN";
                paramEN.SqlDbType = SqlDbType.Int;
                paramEN.Value = objPropContracts.EN;

                SqlParameter paramUserID = new SqlParameter();
                paramUserID.ParameterName = "UserID";
                paramUserID.SqlDbType = SqlDbType.Int;
                paramUserID.Value = Convert.ToInt32(objPropContracts.UserID);

                SqlParameter paramIncludeCredit = new SqlParameter();
                paramIncludeCredit.ParameterName = "IncludeCredit";
                paramIncludeCredit.SqlDbType = SqlDbType.Bit;
                paramIncludeCredit.Value = includeCredit;

                SqlParameter paramIncludeCustomerCredit = new SqlParameter();
                paramIncludeCustomerCredit.ParameterName = "IncludeCustomerCredit";
                paramIncludeCustomerCredit.SqlDbType = SqlDbType.Bit;
                paramIncludeCustomerCredit.Value = includeCustomerCredit;

                SqlParameter paramCustomer = new SqlParameter();
                paramCustomer.ParameterName = "Customer";
                paramCustomer.SqlDbType = SqlDbType.NVarChar;
                paramCustomer.Value = Customer;

                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetCustomerStatementByCustomer", paraOwner, paraStrOwners, paraIsOverDue, paramEN, paramUserID, paramIncludeCredit, paramIncludeCustomerCredit, paramCustomer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustomerStatementDetails(Contracts objPropContracts)
        {
            try
            {
                SqlParameter paraOwner = new SqlParameter();
                paraOwner.ParameterName = "Owner";
                paraOwner.SqlDbType = SqlDbType.Int;
                paraOwner.Value = objPropContracts.Owner;


                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetCustomerStatementDetails", paraOwner);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustomerStatementByLoc(Contracts objPropContracts)
        {
            try
            {
                SqlParameter paraOwner = new SqlParameter();
                paraOwner.ParameterName = "Loc";
                paraOwner.SqlDbType = SqlDbType.Int;
                paraOwner.Value = objPropContracts.Loc;

                SqlParameter paramUserID = new SqlParameter();
                paramUserID.ParameterName = "UserID";
                paramUserID.SqlDbType = SqlDbType.Int;
                paramUserID.Value = objPropContracts.UserID;

                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetCustomerStatementByLoc", paraOwner, paramUserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetCustomerStatementByLoc(GetCustomerStatementByLocParam _GetCustomerStatementByLoc, string ConnectionString)
        {
            try
            {
                SqlParameter paraOwner = new SqlParameter();
                paraOwner.ParameterName = "Loc";
                paraOwner.SqlDbType = SqlDbType.Int;
                paraOwner.Value = _GetCustomerStatementByLoc.Loc;

                SqlParameter paramUserID = new SqlParameter();
                paramUserID.ParameterName = "UserID";
                paramUserID.SqlDbType = SqlDbType.Int;
                paramUserID.Value = _GetCustomerStatementByLoc.UserID;

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetCustomerStatementByLoc", paraOwner, paramUserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustomerStatementByLocs(Contracts objPropContracts)
        {
            try
            {
                SqlParameter paraOwner = new SqlParameter();
                paraOwner.ParameterName = "Locs";
                paraOwner.SqlDbType = SqlDbType.VarChar;
                paraOwner.Value = objPropContracts.LocationIDs;

                SqlParameter paramUserID = new SqlParameter();
                paramUserID.ParameterName = "UserID";
                paramUserID.SqlDbType = SqlDbType.Int;
                paramUserID.Value = objPropContracts.UserID;

                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetCustomerStatementByLocs", paraOwner, paramUserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetCustomerStatementByLocs(GetCustomerStatementByLocsParam _GetCustomerStatementByLocs, string ConnectionString)
        {
            try
            {
                SqlParameter paraOwner = new SqlParameter();
                paraOwner.ParameterName = "Locs";
                paraOwner.SqlDbType = SqlDbType.VarChar;
                paraOwner.Value = _GetCustomerStatementByLocs.LocationIDs;

                SqlParameter paramUserID = new SqlParameter();
                paramUserID.ParameterName = "UserID";
                paramUserID.SqlDbType = SqlDbType.Int;
                paramUserID.Value = _GetCustomerStatementByLocs.UserID;

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetCustomerStatementByLocs", paraOwner, paramUserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustomerStatementCollection(Contracts objPropContracts, bool includeCredit, bool includeCustomerCredit)
        {
            try
            {
                SqlParameter LocationIDs = new SqlParameter();
                LocationIDs.ParameterName = "LocationIDs";
                LocationIDs.SqlDbType = SqlDbType.VarChar;
                LocationIDs.Value = objPropContracts.LocationIDs;

                SqlParameter CustomerIDs = new SqlParameter();
                CustomerIDs.ParameterName = "CustomerIDs";
                CustomerIDs.SqlDbType = SqlDbType.VarChar;
                CustomerIDs.Value = objPropContracts.CustomerIDs;

                SqlParameter paraIsOverDue = new SqlParameter();
                paraIsOverDue.ParameterName = "IsOverDue";
                paraIsOverDue.SqlDbType = SqlDbType.Bit;
                paraIsOverDue.Value = objPropContracts.IsOverDue;

                SqlParameter paramEN = new SqlParameter();
                paramEN.ParameterName = "EN";
                paramEN.SqlDbType = SqlDbType.Int;
                paramEN.Value = objPropContracts.EN;

                SqlParameter paramUserID = new SqlParameter();
                paramUserID.ParameterName = "UserID";
                paramUserID.SqlDbType = SqlDbType.Int;
                paramUserID.Value = Convert.ToInt32(objPropContracts.UserID);

                SqlParameter paramIncludeCredit = new SqlParameter();
                paramIncludeCredit.ParameterName = "IncludeCredit";
                paramIncludeCredit.SqlDbType = SqlDbType.Bit;
                paramIncludeCredit.Value = includeCredit;

                SqlParameter paramIncludeCustomerCredit = new SqlParameter();
                paramIncludeCustomerCredit.ParameterName = "IncludeCustomerCredit";
                paramIncludeCustomerCredit.SqlDbType = SqlDbType.Bit;
                paramIncludeCustomerCredit.Value = includeCustomerCredit;

                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetCustomerStatementCollection", LocationIDs, CustomerIDs, paraIsOverDue, paramEN, paramUserID, paramIncludeCredit, paramIncludeCustomerCredit);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetCustomerStatementCollection(GetCustomerStatementCollectionParam _GetCustomerStatementCollection, string ConnectionString, bool includeCredit, bool includeCustomerCredit)
        {
            try
            {
                SqlParameter LocationIDs = new SqlParameter();
                LocationIDs.ParameterName = "LocationIDs";
                LocationIDs.SqlDbType = SqlDbType.VarChar;
                LocationIDs.Value = _GetCustomerStatementCollection.LocationIDs;

                SqlParameter CustomerIDs = new SqlParameter();
                CustomerIDs.ParameterName = "CustomerIDs";
                CustomerIDs.SqlDbType = SqlDbType.VarChar;
                CustomerIDs.Value = _GetCustomerStatementCollection.CustomerIDs;

                SqlParameter paraIsOverDue = new SqlParameter();
                paraIsOverDue.ParameterName = "IsOverDue";
                paraIsOverDue.SqlDbType = SqlDbType.Bit;
                paraIsOverDue.Value = _GetCustomerStatementCollection.IsOverDue;

                SqlParameter paramEN = new SqlParameter();
                paramEN.ParameterName = "EN";
                paramEN.SqlDbType = SqlDbType.Int;
                paramEN.Value = _GetCustomerStatementCollection.EN;

                SqlParameter paramUserID = new SqlParameter();
                paramUserID.ParameterName = "UserID";
                paramUserID.SqlDbType = SqlDbType.Int;
                paramUserID.Value = Convert.ToInt32(_GetCustomerStatementCollection.UserID);

                SqlParameter paramIncludeCredit = new SqlParameter();
                paramIncludeCredit.ParameterName = "IncludeCredit";
                paramIncludeCredit.SqlDbType = SqlDbType.Bit;
                paramIncludeCredit.Value = includeCredit;

                SqlParameter paramIncludeCustomerCredit = new SqlParameter();
                paramIncludeCustomerCredit.ParameterName = "IncludeCustomerCredit";
                paramIncludeCustomerCredit.SqlDbType = SqlDbType.Bit;
                paramIncludeCustomerCredit.Value = includeCustomerCredit;

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetCustomerStatementCollection", LocationIDs, CustomerIDs, paraIsOverDue, paramEN, paramUserID, paramIncludeCredit, paramIncludeCustomerCredit);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetARAgingByAsOfDate(Contracts objPropContracts, int creditFlag = 0)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = objPropContracts.Date;

                SqlParameter HidePartial = new SqlParameter();
                HidePartial.ParameterName = "HidePartial";
                HidePartial.SqlDbType = SqlDbType.Bit;
                HidePartial.Value = objPropContracts.HidePartial;

                SqlParameter creditFlagPara = new SqlParameter();
                creditFlagPara.ParameterName = "CreditFlag";
                creditFlagPara.SqlDbType = SqlDbType.TinyInt;
                creditFlagPara.Value = creditFlag;

                if (objPropContracts.isDBTotalService)
                {
                    return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetARAgingByDate", paraDate, HidePartial, creditFlagPara);
                }
                else
                {
                    return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetARAgingByDateNoneTS", paraDate, creditFlagPara);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetARAgingByAsOfDateOver90DaysReport(Contracts objPropContracts, int creditFlag = 0)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "@fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = objPropContracts.Date;

                SqlParameter HidePartial = new SqlParameter();
                HidePartial.ParameterName = "@HidePartial";
                HidePartial.SqlDbType = SqlDbType.Bit;
                HidePartial.Value = objPropContracts.HidePartial;

                SqlParameter creditFlagPara = new SqlParameter();
                creditFlagPara.ParameterName = "@CreditFlag";
                creditFlagPara.SqlDbType = SqlDbType.TinyInt;
                creditFlagPara.Value = creditFlag;

                if (objPropContracts.isDBTotalService)
                {
                    return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetARAgingByDateOver90DaysReport", paraDate, HidePartial, creditFlagPara);
                }
                else
                {
                    return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetARAgingByDateNoneTSOver90DaysReport", paraDate, creditFlagPara);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetARAgingByLocation(Contracts objPropContracts)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = objPropContracts.Date;

                SqlParameter paraLoc = new SqlParameter();
                paraLoc.ParameterName = "@Loc";
                paraLoc.SqlDbType = SqlDbType.Int;
                paraLoc.Value = objPropContracts.Loc;

                if (objPropContracts.isDBTotalService)
                {
                    return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetARAgingByLocation", paraDate, paraLoc);
                }
                else
                {
                    return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetARAgingByLocationNoneTS", paraDate, paraLoc);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetARAgingSummaryByLocation(Contracts objPropContracts, int creditFlag = 0)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = objPropContracts.Date;

                SqlParameter HidePartial = new SqlParameter();
                HidePartial.ParameterName = "HidePartial";
                HidePartial.SqlDbType = SqlDbType.Bit;
                HidePartial.Value = objPropContracts.HidePartial;

                SqlParameter creditFlagPara = new SqlParameter();
                creditFlagPara.ParameterName = "CreditFlag";
                creditFlagPara.SqlDbType = SqlDbType.TinyInt;
                creditFlagPara.Value = creditFlag;

                if (objPropContracts.isDBTotalService)
                {
                    return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetARAgingSummaryByLocation", paraDate, HidePartial, creditFlagPara);
                }
                else
                {
                    return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetARAgingSummaryByLocationNoneTS", paraDate, creditFlagPara);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //API
        public DataSet GetARAgingByAsOfDate(GetARAgingByAsOfDateParam _GetARAgingByAsOfDate, string ConnectionString)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = _GetARAgingByAsOfDate.Date;

                SqlParameter HidePartial = new SqlParameter();
                HidePartial.ParameterName = "HidePartial";
                HidePartial.SqlDbType = SqlDbType.Bit;
                HidePartial.Value = _GetARAgingByAsOfDate.HidePartial;

                if (_GetARAgingByAsOfDate.isDBTotalService)
                {
                    return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetARAgingByDate", paraDate, HidePartial);
                }
                else
                {
                    return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetARAgingByDateNoneTS", paraDate);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetARAgingByAsOfDateDep(Contracts objPropContracts, int creditFlag = 0)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = objPropContracts.Date;

                SqlParameter paraDep = new SqlParameter();
                paraDep.ParameterName = "DepartmentType";
                paraDep.SqlDbType = SqlDbType.VarChar;
                paraDep.Value = objPropContracts.DepartmentTypes;

                SqlParameter creditFlagPara = new SqlParameter();
                creditFlagPara.ParameterName = "CreditFlag";
                creditFlagPara.SqlDbType = SqlDbType.TinyInt;
                creditFlagPara.Value = creditFlag;

                if (objPropContracts.isDBTotalService)
                {
                    return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetARAgingByDateDep", paraDate, paraDep, creditFlagPara);
                }
                else
                {
                    return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetARAgingByDateDepNoneTS", paraDate, paraDep, creditFlagPara);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetARAgingByAsOfDateDep(GetARAgingByAsOfDateDepParam _GetARAgingByAsOfDateDep, string ConnectionString)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = _GetARAgingByAsOfDateDep.Date;

                SqlParameter paraDep = new SqlParameter();
                paraDep.ParameterName = "DepartmentType";
                paraDep.SqlDbType = SqlDbType.VarChar;
                paraDep.Value = _GetARAgingByAsOfDateDep.DepartmentTypes;

                if (_GetARAgingByAsOfDateDep.isDBTotalService)
                {
                    return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetARAgingByDateDep", paraDate, paraDep);
                }
                else
                {
                    return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetARAgingByDateDepNoneTS", paraDate, paraDep);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetARAging(Contracts objPropContracts, int creditFlag = 0)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = objPropContracts.Date;

                SqlParameter paraOwner = new SqlParameter();
                paraOwner.ParameterName = "Owner";
                paraOwner.SqlDbType = SqlDbType.Int;
                paraOwner.Value = objPropContracts.Owner;

                SqlParameter HidePartial = new SqlParameter();
                HidePartial.ParameterName = "HidePartial";
                HidePartial.SqlDbType = SqlDbType.Bit;
                HidePartial.Value = objPropContracts.HidePartial;

                SqlParameter creditFlagPara = new SqlParameter();
                creditFlagPara.ParameterName = "CreditFlag";
                creditFlagPara.SqlDbType = SqlDbType.TinyInt;
                creditFlagPara.Value = creditFlag;


                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetARAgingByDate", paraDate, HidePartial, creditFlagPara);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetARAgingOver90DaysReport(Contracts objPropContracts, int creditFlag = 0)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = objPropContracts.Date;

                SqlParameter paraOwner = new SqlParameter();
                paraOwner.ParameterName = "Owner";
                paraOwner.SqlDbType = SqlDbType.Int;
                paraOwner.Value = objPropContracts.Owner;

                SqlParameter HidePartial = new SqlParameter();
                HidePartial.ParameterName = "HidePartial";
                HidePartial.SqlDbType = SqlDbType.Bit;
                HidePartial.Value = objPropContracts.HidePartial;

                SqlParameter creditFlagPara = new SqlParameter();
                creditFlagPara.ParameterName = "CreditFlag";
                creditFlagPara.SqlDbType = SqlDbType.TinyInt;
                creditFlagPara.Value = creditFlag;


                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetARAgingByDateOver90DaysReport", paraDate, HidePartial, creditFlagPara);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetARAging(GetARAgingParam _GetARAging, string ConnectionString)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = _GetARAging.Date;

                SqlParameter paraOwner = new SqlParameter();
                paraOwner.ParameterName = "Owner";
                paraOwner.SqlDbType = SqlDbType.Int;
                paraOwner.Value = _GetARAging.Owner;

                SqlParameter HidePartial = new SqlParameter();
                HidePartial.ParameterName = "HidePartial";
                HidePartial.SqlDbType = SqlDbType.Bit;
                HidePartial.Value = _GetARAging.HidePartial;


                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetARAgingByDate", paraDate, HidePartial);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetARAgingDep(Contracts objPropContracts, int creditFlag = 0)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = objPropContracts.Date;

                SqlParameter paraDep = new SqlParameter();
                paraDep.ParameterName = "DepartmentType";
                paraDep.SqlDbType = SqlDbType.VarChar;
                paraDep.Value = objPropContracts.DepartmentTypes;

                SqlParameter creditFlagPara = new SqlParameter();
                creditFlagPara.ParameterName = "CreditFlag";
                creditFlagPara.SqlDbType = SqlDbType.TinyInt;
                creditFlagPara.Value = creditFlag;

                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetARAgingByDateDep", paraDate, paraDep, creditFlagPara);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetARAgingDep(GetARAgingDepParam _GetARAgingDep, string ConnectionString)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = _GetARAgingDep.Date;

                SqlParameter paraDep = new SqlParameter();
                paraDep.ParameterName = "DepartmentType";
                paraDep.SqlDbType = SqlDbType.VarChar;
                paraDep.Value = _GetARAgingDep.DepartmentTypes;


                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetARAgingByDateDep", paraDate, paraDep);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEscalationContracts(Contracts objPropContracts, string Load, bool include)
        {
            try
            {
                SqlParameter paraOwner = new SqlParameter();
                paraOwner.ParameterName = "@EscDate";
                paraOwner.SqlDbType = SqlDbType.DateTime;
                paraOwner.Value = objPropContracts.EscalationLast;

                SqlParameter paraUser = new SqlParameter();
                paraUser.ParameterName = "@UserId";
                paraUser.SqlDbType = SqlDbType.Int;
                paraUser.Value = Convert.ToInt32(objPropContracts.UserID);

                SqlParameter paraEn = new SqlParameter();
                paraEn.ParameterName = "@EN";
                paraEn.SqlDbType = SqlDbType.Int;
                paraEn.Value = Convert.ToInt32(objPropContracts.EN);

                SqlParameter paraLoad = new SqlParameter();
                paraLoad.ParameterName = "@Load";
                paraLoad.SqlDbType = SqlDbType.Char;
                paraLoad.Value = Load;

                SqlParameter parainclude = new SqlParameter();
                parainclude.ParameterName = "@include";
                parainclude.SqlDbType = SqlDbType.Bit;
                parainclude.Value = include;


                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetEscalationContracts", paraOwner, paraUser, paraEn, paraLoad, parainclude);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetEscalationContracts(GetEscalationContractsParam _GetEscalationContracts, string ConnectionString)
        {
            try
            {
                SqlParameter paraOwner = new SqlParameter();
                paraOwner.ParameterName = "@EscDate";
                paraOwner.SqlDbType = SqlDbType.DateTime;
                paraOwner.Value = _GetEscalationContracts.EscalationLast;

                SqlParameter paraUser = new SqlParameter();
                paraUser.ParameterName = "@UserId";
                paraUser.SqlDbType = SqlDbType.Int;
                paraUser.Value = Convert.ToInt32(_GetEscalationContracts.UserID);

                SqlParameter paraEn = new SqlParameter();
                paraEn.ParameterName = "@EN";
                paraEn.SqlDbType = SqlDbType.Int;
                paraEn.Value = Convert.ToInt32(_GetEscalationContracts.EN);


                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetEscalationContracts", paraOwner, paraUser, paraEn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EscalateContract(Contracts objPropContracts)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, "spEscalateContract", objPropContracts.EscalationPriorto, objPropContracts.jobid, objPropContracts.Fuser, objPropContracts.EscalationDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateEscalationFactor(Contracts objPropContracts)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, "spupdateEscalationFactor", objPropContracts.jobid, objPropContracts.Fuser, objPropContracts.EscalationFactor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public void EscalateContract(EscalateContractParam _EscalateContract, string ConnectionString)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, "spEscalateContract", _EscalateContract.ExpirationDate, _EscalateContract.jobids, _EscalateContract.Fuser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetARGLReg(Contracts objContract)
        {
            try
            {
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "StartDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objContract.strSdate
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "EndDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objContract.strEdate
                };
                return objContract.Ds = SqlHelper.ExecuteDataset(objContract.ConnConfig, CommandType.StoredProcedure, "spGetARGLRegNew", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Invoice By Billing Code For ES-6940 by PS
        public DataSet GetARGLRegByBillingCode(Contracts objContract)
        {
            try
            {
                var para = new SqlParameter[3];

                para[0] = new SqlParameter
                {
                    ParameterName = "StartDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objContract.strSdate
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "EndDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objContract.strEdate
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "BillingCode",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objContract.Ctype
                };
                return objContract.Ds = SqlHelper.ExecuteDataset(objContract.ConnConfig, CommandType.StoredProcedure, "spGetARGLRegByBillingCode", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetBillableServiceReport(Contracts objContract)
        {
            try
            {
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "StartDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objContract.strSdate
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "EndDate",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objContract.strEdate
                };
                return objContract.Ds = SqlHelper.ExecuteDataset(objContract.ConnConfig, CommandType.StoredProcedure, "spGetBillableServiceByInvoice", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCollectionInvoices(Contracts objPropContracts)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spCollectionInvoices");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCollectionInvoicesCompany(Contracts objPropContracts)
        {
            try
            {
                SqlParameter paraUserID = new SqlParameter();
                paraUserID.ParameterName = "@UserID";
                paraUserID.SqlDbType = SqlDbType.NVarChar;
                paraUserID.Value = objPropContracts.UserID;
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spCollectionInvoicesCompany", paraUserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetOutstandingInvoice(Contracts objPropContracts)
        {
            try
            {
                SqlParameter paraOwner = new SqlParameter();
                paraOwner.ParameterName = "@strOwners";
                paraOwner.SqlDbType = SqlDbType.VarChar;
                paraOwner.Value = objPropContracts.strOwners;

                SqlParameter paraIsOverDue = new SqlParameter();
                paraIsOverDue.ParameterName = "@IsOverDue";
                paraIsOverDue.SqlDbType = SqlDbType.Bit;
                paraIsOverDue.Value = objPropContracts.IsOverDue;

                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetOutstandingInvoices", paraOwner, paraIsOverDue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetEquipmentByInvoice(Contracts objPropContracts)
        {
            try
            {

                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, "select  e.Unit, e.Unit , ej.Price, ej.Hours,C.Detail ,j.ID from elev e inner join  tbljoinElevJob ej on e.ID=ej.Elev inner join Job j on ej.Job=j.ID inner join   contract c on j.ID=c.Job where j.Loc=" + objPropContracts.JobId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetRecurringContractLogs(Contracts objPropContracts)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, "select * from Log2 where ref =" + objPropContracts.JobId + "  and Screen='Job' order by CreatedStamp desc ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetRecurringContractLogs(GetRecurringContractLogsParam _GetRecurringContractLogs, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select * from Log2 where ref =" + _GetRecurringContractLogs.JobId + "  and Screen='Job' order by CreatedStamp desc ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetInvoiceLogs(Contracts objPropContracts)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, "select * from Log2 where ref =" + objPropContracts.InvoiceID + "  and Screen='Invoice' order by CreatedStamp desc ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetARAgingByTerritory(Contracts objPropContracts, string territories, int creditFlag = 0)
        {
            try
            {
                var para = new SqlParameter[3];

                para[0] = new SqlParameter
                {
                    ParameterName = "fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPropContracts.Date
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "Territories",
                    SqlDbType = SqlDbType.VarChar,
                    Value = territories
                };

                para[2] = new SqlParameter
                {
                    ParameterName = "CreditFlag",
                    SqlDbType = SqlDbType.TinyInt,
                    Value = creditFlag
                };

                if (objPropContracts.isDBTotalService)
                {
                    return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetARAgingByTerritory", para);
                }
                else
                {
                    return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetARAgingByTerritoryNoneTS", para);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetARAgingByTerritory(GetARAgingByTerritoryParam _GetARAgingByTerritory, string ConnectionString, string territories)
        {
            try
            {
                var para = new SqlParameter[2];

                para[0] = new SqlParameter
                {
                    ParameterName = "fDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = _GetARAgingByTerritory.Date
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "Territories",
                    SqlDbType = SqlDbType.VarChar,
                    Value = territories
                };
                if (_GetARAgingByTerritory.isDBTotalService)
                {
                    return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetARAgingByTerritory", para);
                }
                else
                {
                    return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetARAgingByTerritoryNoneTS", para);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetARAgingByJobType(Contracts objPropContracts, int creditFlag = 0)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = objPropContracts.Date;

                SqlParameter paraDep = new SqlParameter();
                paraDep.ParameterName = "DepartmentType";
                paraDep.SqlDbType = SqlDbType.VarChar;
                paraDep.Value = objPropContracts.DepartmentTypes;

                SqlParameter paraCreditFlag = new SqlParameter();
                paraCreditFlag.ParameterName = "CreditFlag";
                paraCreditFlag.SqlDbType = SqlDbType.TinyInt;
                paraCreditFlag.Value = creditFlag;

                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetARAgingByJobType", paraDate, paraDep, paraCreditFlag);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvoiceByDate(Contracts objPropContracts)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = objPropContracts.Date;

                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetInvoiceByDate", paraDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetInvoiceByDate(GetInvoiceByDateParam _GetInvoiceByDate, string ConnectionString)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = _GetInvoiceByDate.Date;

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetInvoiceByDate", paraDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetHistoryPayment(Contracts objPropContracts)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "@ID";
                paraDate.SqlDbType = SqlDbType.Int;
                paraDate.Value = objPropContracts.InvoiceID;

                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetInvoiceHistoryPayment", paraDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetARAgingByJobTypeDetail(Contracts objPropContracts)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = objPropContracts.Date;

                SqlParameter paraDep = new SqlParameter();
                paraDep.ParameterName = "DepartmentType";
                paraDep.SqlDbType = SqlDbType.VarChar;
                paraDep.Value = objPropContracts.DepartmentTypes;


                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetARAgingByJobTypeDetail", paraDate, paraDep);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetARAgingByLocTypeDetail(Contracts objPropContracts, int creditFlag = 0)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = objPropContracts.Date;

                SqlParameter paraDep = new SqlParameter();
                paraDep.ParameterName = "LocType";
                paraDep.SqlDbType = SqlDbType.VarChar;
                paraDep.Value = objPropContracts.DepartmentTypes;

                SqlParameter paraCreditFlag = new SqlParameter();
                paraCreditFlag.ParameterName = "CreditFlag";
                paraCreditFlag.SqlDbType = SqlDbType.TinyInt;
                paraCreditFlag.Value = creditFlag;

                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetARAgingByLocTypeDetail", paraDate, paraDep, paraCreditFlag);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetARAgingByLocTypeDetail(GetARAgingByLocTypeDetailParam _GetARAgingByLocTypeDetail, string ConnectionString)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = _GetARAgingByLocTypeDetail.Date;

                SqlParameter paraDep = new SqlParameter();
                paraDep.ParameterName = "LocType";
                paraDep.SqlDbType = SqlDbType.VarChar;
                paraDep.Value = _GetARAgingByLocTypeDetail.DepartmentTypes;


                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetARAgingByLocTypeDetail", paraDate, paraDep);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetARAgingByLocType(Contracts objPropContracts, int creditFlag = 0)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = objPropContracts.Date;

                SqlParameter paraDep = new SqlParameter();
                paraDep.ParameterName = "LocType";
                paraDep.SqlDbType = SqlDbType.VarChar;
                paraDep.Value = objPropContracts.DepartmentTypes;

                SqlParameter paraCreditFlag = new SqlParameter();
                paraCreditFlag.ParameterName = "CreditFlag";
                paraCreditFlag.SqlDbType = SqlDbType.TinyInt;
                paraCreditFlag.Value = creditFlag;

                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetARAgingByLocType", paraDate, paraDep, paraCreditFlag);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetARAgingByLocType(GetARAgingByLocTypeParam _GetARAgingByLocType, string ConnectionString)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = _GetARAgingByLocType.Date;

                SqlParameter paraDep = new SqlParameter();
                paraDep.ParameterName = "LocType";
                paraDep.SqlDbType = SqlDbType.VarChar;
                paraDep.Value = _GetARAgingByLocType.DepartmentTypes;


                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetARAgingByLocType", paraDate, paraDep);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetInvoiceStatus(Contracts objPropContracts)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT  DISTINCT \n");
                varname1.Append("	(CASE i.status  \n");
                varname1.Append("		WHEN 0 THEN 'Open'  \n");
                varname1.Append("		WHEN 1 THEN 'Paid'  \n");
                varname1.Append("		WHEN 2 THEN 'Voided'  \n");
                varname1.Append("		WHEN 4 THEN 'Marked AS Pending'  \n");
                varname1.Append("		WHEN 5 THEN 'Paid by Credit Card'  \n");
                varname1.Append("		WHEN 3 THEN 'Partially Paid'  \n");
                varname1.Append("	END ) AS Status from Invoice i Where i.status is not null  \n");

                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public DataSet GetInvoiceByRef(String config, int id)
        //{
        //    try
        //    {
        //        SqlParameter paraDate = new SqlParameter();
        //        paraDate.ParameterName = "Ref";
        //        paraDate.SqlDbType = SqlDbType.Int;
        //        paraDate.Value =id;              

        //        return SqlHelper.ExecuteDataset(config, CommandType.StoredProcedure, "spGetInvoiceByID", paraDate);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataSet GetInvoicesPaging(Contracts objInvoice)
        {
            try
            {
                var para = new SqlParameter[20];

                para[0] = new SqlParameter
                {
                    ParameterName = "strSdate",
                    SqlDbType = SqlDbType.DateTime
                };
                if (objInvoice.StartDate == System.DateTime.MinValue)
                {
                    para[0].Value = DBNull.Value;
                }
                else
                {
                    para[0].Value = objInvoice.StartDate;
                }
                para[1] = new SqlParameter
                {
                    ParameterName = "strEdate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objInvoice.EndDate
                };

                if (objInvoice.EndDate == System.DateTime.MinValue)
                {
                    para[1].Value = DBNull.Value;
                }
                else
                {
                    para[1].Value = objInvoice.EndDate;
                }
                para[2] = new SqlParameter
                {
                    ParameterName = "SearchBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objInvoice.SearchBy
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "SearchValue",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objInvoice.SearchValue
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Bit,
                    Value = objInvoice.EN
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "isGridFilterInvoice",
                    SqlDbType = SqlDbType.Bit,
                    Value = objInvoice.isGridFilterInvoice
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = objInvoice.UserID
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "CustID",
                    SqlDbType = SqlDbType.Int,
                    Value = objInvoice.CustID
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "Loc",
                    SqlDbType = SqlDbType.Int,
                    Value = objInvoice.Loc
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "jobid",
                    SqlDbType = SqlDbType.Int,
                    Value = objInvoice.JobId
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "Paid",
                    SqlDbType = SqlDbType.Int,
                    Value = objInvoice.Paid
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "RoleID",
                    SqlDbType = SqlDbType.Int,
                    Value = objInvoice.RoleId
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "isShowAll",
                    SqlDbType = SqlDbType.Bit,
                    Value = objInvoice.isShowAll
                };
                para[13] = new SqlParameter
                {
                    ParameterName = "SearchAmtPaidUnpaid",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objInvoice.SearchAmtPaidUnpaid
                };
                para[14] = new SqlParameter
                {
                    ParameterName = "SearchPrintMail",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objInvoice.SearchPrintMail
                };
                para[15] = new SqlParameter
                {
                    ParameterName = "FilterByColumn",
                    SqlDbType = SqlDbType.Structured,
                    Value = objInvoice.dtFilterByColumn
                };
                para[16] = new SqlParameter
                {
                    ParameterName = "PageNumber",
                    SqlDbType = SqlDbType.Int,
                    Value = objInvoice.PageNumber
                };
                para[17] = new SqlParameter
                {
                    ParameterName = "PageSize",
                    SqlDbType = SqlDbType.Int,
                    Value = objInvoice.PageSize
                };
                para[18] = new SqlParameter
                {
                    ParameterName = "SortOrderBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objInvoice.SortOrderBy
                };
                para[19] = new SqlParameter
                {
                    ParameterName = "SortType",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objInvoice.SortType
                };
                return SqlHelper.ExecuteDataset(objInvoice.ConnConfig, CommandType.StoredProcedure, "spGetInvoicesPaging", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvoicesToExport(Contracts objInvoice)
        {
            try
            {
                var para = new SqlParameter[18];

                para[0] = new SqlParameter
                {
                    ParameterName = "strSdate",
                    SqlDbType = SqlDbType.DateTime
                };
                if (objInvoice.StartDate == System.DateTime.MinValue)
                {
                    para[0].Value = DBNull.Value;
                }
                else
                {
                    para[0].Value = objInvoice.StartDate;
                }
                para[1] = new SqlParameter
                {
                    ParameterName = "strEdate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objInvoice.EndDate
                };

                if (objInvoice.EndDate == System.DateTime.MinValue)
                {
                    para[1].Value = DBNull.Value;
                }
                else
                {
                    para[1].Value = objInvoice.EndDate;
                }
                para[2] = new SqlParameter
                {
                    ParameterName = "SearchBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objInvoice.SearchBy
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "SearchValue",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objInvoice.SearchValue
                };
                para[4] = new SqlParameter
                {
                    ParameterName = "EN",
                    SqlDbType = SqlDbType.Bit,
                    Value = objInvoice.EN
                };
                para[5] = new SqlParameter
                {
                    ParameterName = "isGridFilterInvoice",
                    SqlDbType = SqlDbType.Bit,
                    Value = objInvoice.isGridFilterInvoice
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "UserID",
                    SqlDbType = SqlDbType.Int,
                    Value = objInvoice.UserID
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "CustID",
                    SqlDbType = SqlDbType.Int,
                    Value = objInvoice.CustID
                };
                para[8] = new SqlParameter
                {
                    ParameterName = "Loc",
                    SqlDbType = SqlDbType.Int,
                    Value = objInvoice.Loc
                };
                para[9] = new SqlParameter
                {
                    ParameterName = "jobid",
                    SqlDbType = SqlDbType.Int,
                    Value = objInvoice.JobId
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "Paid",
                    SqlDbType = SqlDbType.Int,
                    Value = objInvoice.Paid
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "RoleID",
                    SqlDbType = SqlDbType.Int,
                    Value = objInvoice.RoleId
                };
                para[12] = new SqlParameter
                {
                    ParameterName = "isShowAll",
                    SqlDbType = SqlDbType.Bit,
                    Value = objInvoice.isShowAll
                };
                para[13] = new SqlParameter
                {
                    ParameterName = "SearchAmtPaidUnpaid",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objInvoice.SearchAmtPaidUnpaid
                };
                para[14] = new SqlParameter
                {
                    ParameterName = "SearchPrintMail",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objInvoice.SearchPrintMail
                };
                para[15] = new SqlParameter
                {
                    ParameterName = "FilterByColumn",
                    SqlDbType = SqlDbType.Structured,
                    Value = objInvoice.dtFilterByColumn
                };

                para[16] = new SqlParameter
                {
                    ParameterName = "SortOrderBy",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objInvoice.SortOrderBy
                };
                para[17] = new SqlParameter
                {
                    ParameterName = "SortType",
                    SqlDbType = SqlDbType.VarChar,
                    Value = objInvoice.SortType
                };
                return SqlHelper.ExecuteDataset(objInvoice.ConnConfig, CommandType.StoredProcedure, "spGetInvoicesToExport", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvoiceByInvoiceID(Contracts objPropContracts)
        {

            try
            {
                var para = new SqlParameter[1];

                para[0] = new SqlParameter
                {
                    ParameterName = "Ref",
                    SqlDbType = SqlDbType.Int,
                    Value = objPropContracts.InvoiceID
                };


                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetInvoiceByRef", para);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddInvoice(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[32];
            para[0] = new SqlParameter();
            para[0].ParameterName = "Invoice";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = objPropContracts.DtRecContr;

            para[1] = new SqlParameter();
            para[1].ParameterName = "fdate";
            para[1].SqlDbType = SqlDbType.DateTime;
            para[1].Value = objPropContracts.Date;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Fdesc";
            para[2].SqlDbType = SqlDbType.Text;
            para[2].Value = objPropContracts.Remarks;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Amount";
            para[3].SqlDbType = SqlDbType.Money;
            para[3].Value = objPropContracts.Amount;

            para[4] = new SqlParameter();
            para[4].ParameterName = "stax";
            para[4].SqlDbType = SqlDbType.Money;
            para[4].Value = objPropContracts.Staxtotal;

            para[5] = new SqlParameter();
            para[5].ParameterName = "total";
            para[5].SqlDbType = SqlDbType.Money;
            para[5].Value = objPropContracts.Total;

            para[6] = new SqlParameter();
            para[6].ParameterName = "taxRegion";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropContracts.TaxRegion;

            para[7] = new SqlParameter();
            para[7].ParameterName = "taxrate";
            para[7].SqlDbType = SqlDbType.Money;
            para[7].Value = objPropContracts.Taxrate;

            para[8] = new SqlParameter();
            para[8].ParameterName = "Taxfactor";
            para[8].SqlDbType = SqlDbType.Money;
            para[8].Value = objPropContracts.Taxfactor;

            para[9] = new SqlParameter();
            para[9].ParameterName = "taxable";
            para[9].SqlDbType = SqlDbType.Money;
            para[9].Value = objPropContracts.Taxable;

            para[10] = new SqlParameter();
            para[10].ParameterName = "type";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropContracts.Type;

            para[11] = new SqlParameter();
            para[11].ParameterName = "job";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropContracts.JobId;

            para[12] = new SqlParameter();
            para[12].ParameterName = "loc";
            para[12].SqlDbType = SqlDbType.Int;
            para[12].Value = objPropContracts.Loc;

            para[13] = new SqlParameter();
            para[13].ParameterName = "terms";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropContracts.Terms;

            para[14] = new SqlParameter();
            para[14].ParameterName = "po";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropContracts.PO;

            para[15] = new SqlParameter();
            para[15].ParameterName = "status";
            para[15].SqlDbType = SqlDbType.Int;
            para[15].Value = objPropContracts.Status;

            para[16] = new SqlParameter();
            para[16].ParameterName = "remarks";
            para[16].SqlDbType = SqlDbType.Text;
            para[16].Value = objPropContracts.Remarks;

            para[17] = new SqlParameter();
            para[17].ParameterName = "gtax";
            para[17].SqlDbType = SqlDbType.Money;
            para[17].Value = objPropContracts.Gtax;

            para[18] = new SqlParameter();
            para[18].ParameterName = "mech";
            para[18].SqlDbType = SqlDbType.Int;
            para[18].Value = objPropContracts.Mech;

            para[19] = new SqlParameter();
            para[19].ParameterName = "TaxRegion2";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropContracts.TaxRegion2;

            para[20] = new SqlParameter();
            para[20].ParameterName = "Taxrate2";
            para[20].SqlDbType = SqlDbType.Money;
            para[20].Value = objPropContracts.Taxrate2;

            para[21] = new SqlParameter();
            para[21].ParameterName = "BillTo";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = objPropContracts.BillTo;

            para[22] = new SqlParameter();
            para[22].ParameterName = "Idate";
            para[22].SqlDbType = SqlDbType.DateTime;
            para[22].Value = objPropContracts.Idate;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Fuser";
            para[23].SqlDbType = SqlDbType.VarChar;
            para[23].Value = objPropContracts.Fuser;

            para[24] = new SqlParameter();
            para[24].ParameterName = "staxI";
            para[24].SqlDbType = SqlDbType.Int;
            para[24].Value = objPropContracts.StaxI;

            para[25] = new SqlParameter();
            para[25].ParameterName = "invoiceID";
            para[25].SqlDbType = SqlDbType.VarChar;
            para[25].Value = objPropContracts.InvoiceIDCustom;

            para[26] = new SqlParameter();
            para[26].ParameterName = "TicketIDs";
            para[26].SqlDbType = SqlDbType.VarChar;
            para[26].Value = objPropContracts.Tickets;

            para[27] = new SqlParameter();
            para[27].ParameterName = "ddate";
            para[27].SqlDbType = SqlDbType.DateTime;
            para[27].Value = objPropContracts.DueDate;

            para[28] = new SqlParameter();
            para[28].ParameterName = "returnval";
            para[28].SqlDbType = SqlDbType.Int;
            para[28].Direction = ParameterDirection.ReturnValue;

            para[29] = new SqlParameter();
            para[29].ParameterName = "AssignedTo";
            para[29].SqlDbType = SqlDbType.Int;
            para[29].Value = objPropContracts.AssignedTo;

            para[30] = new SqlParameter();
            para[30].ParameterName = "IsRecurring";
            para[30].SqlDbType = SqlDbType.Int;
            para[30].Value = objPropContracts.IsRecurring;

            para[31] = new SqlParameter();
            para[31].ParameterName = "TaxType";
            para[31].SqlDbType = SqlDbType.Int;
            para[31].Value = objPropContracts.TaxType;
            try
            {
                //DataSet _ds =
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spAddInvoice", para);
                return Convert.ToInt32(para[28].Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void EditInvoice(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[30];
            para[0] = new SqlParameter();
            para[0].ParameterName = "Invoice";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = objPropContracts.DtRecContr;

            para[1] = new SqlParameter();
            para[1].ParameterName = "fdate";
            para[1].SqlDbType = SqlDbType.DateTime;
            para[1].Value = objPropContracts.Date;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Fdesc";
            para[2].SqlDbType = SqlDbType.Text;
            para[2].Value = objPropContracts.Remarks;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Amount";
            para[3].SqlDbType = SqlDbType.Money;
            para[3].Value = objPropContracts.Amount;

            para[4] = new SqlParameter();
            para[4].ParameterName = "stax";
            para[4].SqlDbType = SqlDbType.Money;
            para[4].Value = objPropContracts.Staxtotal;

            para[5] = new SqlParameter();
            para[5].ParameterName = "total";
            para[5].SqlDbType = SqlDbType.Money;
            para[5].Value = objPropContracts.Total;

            para[6] = new SqlParameter();
            para[6].ParameterName = "taxRegion";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropContracts.TaxRegion;

            para[7] = new SqlParameter();
            para[7].ParameterName = "taxrate";
            para[7].SqlDbType = SqlDbType.Money;
            para[7].Value = objPropContracts.Taxrate;

            para[8] = new SqlParameter();
            para[8].ParameterName = "Taxfactor";
            para[8].SqlDbType = SqlDbType.Money;
            para[8].Value = objPropContracts.Taxfactor;

            para[9] = new SqlParameter();
            para[9].ParameterName = "taxable";
            para[9].SqlDbType = SqlDbType.Money;
            para[9].Value = objPropContracts.Taxable;

            para[10] = new SqlParameter();
            para[10].ParameterName = "type";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropContracts.Type;

            para[11] = new SqlParameter();
            para[11].ParameterName = "job";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropContracts.JobId;

            para[12] = new SqlParameter();
            para[12].ParameterName = "loc";
            para[12].SqlDbType = SqlDbType.Int;
            para[12].Value = objPropContracts.Loc;

            para[13] = new SqlParameter();
            para[13].ParameterName = "terms";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropContracts.Terms;

            para[14] = new SqlParameter();
            para[14].ParameterName = "po";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropContracts.PO;

            para[15] = new SqlParameter();
            para[15].ParameterName = "status";
            para[15].SqlDbType = SqlDbType.Int;
            para[15].Value = objPropContracts.Status;

            para[16] = new SqlParameter();
            para[16].ParameterName = "remarks";
            para[16].SqlDbType = SqlDbType.Text;
            para[16].Value = objPropContracts.Remarks;

            para[17] = new SqlParameter();
            para[17].ParameterName = "gtax";
            para[17].SqlDbType = SqlDbType.Money;
            para[17].Value = objPropContracts.Gtax;

            para[18] = new SqlParameter();
            para[18].ParameterName = "mech";
            para[18].SqlDbType = SqlDbType.Int;
            para[18].Value = objPropContracts.Mech;

            para[19] = new SqlParameter();
            para[19].ParameterName = "TaxRegion2";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropContracts.TaxRegion2;

            para[20] = new SqlParameter();
            para[20].ParameterName = "Taxrate2";
            para[20].SqlDbType = SqlDbType.Money;
            para[20].Value = objPropContracts.Taxrate2;

            para[21] = new SqlParameter();
            para[21].ParameterName = "BillTo";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = objPropContracts.BillTo;

            para[22] = new SqlParameter();
            para[22].ParameterName = "Idate";
            para[22].SqlDbType = SqlDbType.DateTime;
            para[22].Value = objPropContracts.Idate;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Fuser";
            para[23].SqlDbType = SqlDbType.VarChar;
            para[23].Value = objPropContracts.Fuser;

            para[24] = new SqlParameter();
            para[24].ParameterName = "staxI";
            para[24].SqlDbType = SqlDbType.Int;
            para[24].Value = objPropContracts.StaxI;

            para[25] = new SqlParameter();
            para[25].ParameterName = "invoiceID";
            para[25].SqlDbType = SqlDbType.VarChar;
            para[25].Value = objPropContracts.InvoiceIDCustom;

            para[26] = new SqlParameter();
            para[26].ParameterName = "InvID";
            para[26].SqlDbType = SqlDbType.Int;
            para[26].Value = objPropContracts.InvoiceID;

            para[27] = new SqlParameter();
            para[27].ParameterName = "ddate";
            para[27].SqlDbType = SqlDbType.DateTime;
            para[27].Value = objPropContracts.DueDate;

            para[28] = new SqlParameter();
            para[28].ParameterName = "AssignedTo";
            para[28].SqlDbType = SqlDbType.Int;
            para[28].Value = objPropContracts.AssignedTo;

            para[29] = new SqlParameter();
            para[29].ParameterName = "TaxType";
            para[29].SqlDbType = SqlDbType.Int;
            para[29].Value = objPropContracts.TaxType;
            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spEditInvoice", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetListRecurringInvoices(Contracts objPropContracts, string GriDCust, string GriDLoc, string GriDLocAcc)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, "spGetRecurringInvoices", objPropContracts.Loc, objPropContracts.Owner, objPropContracts.Month, objPropContracts.Year, objPropContracts.Handel, objPropContracts.FlagEN, objPropContracts.UserID, GriDCust, GriDLoc, GriDLocAcc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet AddRecurringInvoices(Contracts objPropContracts, int IncludeContractRemarks)
        {
            SqlParameter[] para = new SqlParameter[10];
            para[0] = new SqlParameter();
            para[0].ParameterName = "RecurringInvoice";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = objPropContracts.DtRecContr;

            para[1] = new SqlParameter();
            para[1].ParameterName = "InvoiceDate";
            para[1].SqlDbType = SqlDbType.DateTime;
            para[1].Value = objPropContracts.Date;

            para[2] = new SqlParameter();
            para[2].ParameterName = "PayTerms";
            para[2].SqlDbType = SqlDbType.Int;
            para[2].Value = objPropContracts.PaymentTerms;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Notes";
            para[3].SqlDbType = SqlDbType.VarChar;
            para[3].Value = objPropContracts.Remarks;

            para[4] = new SqlParameter();
            para[4].ParameterName = "ProcessPeriod";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropContracts.ProcessPeriod;

            para[5] = new SqlParameter();
            para[5].ParameterName = "cfUser";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropContracts.Fuser;

            para[6] = new SqlParameter();
            para[6].ParameterName = "PostingDate";
            para[6].SqlDbType = SqlDbType.DateTime;
            para[6].Value = objPropContracts.PostDate;

            para[7] = new SqlParameter();
            para[7].ParameterName = "DueDate";
            para[7].SqlDbType = SqlDbType.DateTime;
            para[7].Value = objPropContracts.DueDate;


            para[8] = new SqlParameter();
            para[8].ParameterName = "ApplyTaxType";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objPropContracts.TaxApply;

            para[9] = new SqlParameter();
            para[9].ParameterName = "IncludeContractRemarks";
            para[9].SqlDbType = SqlDbType.Int;
            para[9].Value = IncludeContractRemarks;
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spCreateRecurringInvoicesNew", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddRecurringInvoicesUIHistory(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[6];
            para[0] = new SqlParameter();
            para[0].ParameterName = "TaxType";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropContracts.TaxApply;

            para[1] = new SqlParameter();
            para[1].ParameterName = "IsCanadaCompany";
            para[1].SqlDbType = SqlDbType.Bit;
            para[1].Value = objPropContracts.isCanadaCompany;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Taxable";
            para[2].SqlDbType = SqlDbType.Bit;
            para[2].Value = objPropContracts.Taxable;

            para[3] = new SqlParameter();
            para[3].ParameterName = "PaymentTerms";
            para[3].SqlDbType = SqlDbType.Int;
            para[3].Value = objPropContracts.PaymentTerms;

            para[4] = new SqlParameter();
            para[4].ParameterName = "Remarks";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropContracts.Remarks;

            para[5] = new SqlParameter();
            para[5].ParameterName = "UpdatedBy";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropContracts.lastUpdatedby;
            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spAddRecurringInvoicesUIHistory", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeletedRecurringinvoices(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[5];
            para[0] = new SqlParameter();
            para[0].ParameterName = "ContractID";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = objPropContracts.jobid;

            para[1] = new SqlParameter();
            para[1].ParameterName = "Years";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = objPropContracts.Year;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Months";
            para[2].SqlDbType = SqlDbType.Int;
            para[2].Value = objPropContracts.Month;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Deletedby";
            para[3].SqlDbType = SqlDbType.NVarChar;
            para[3].Value = objPropContracts.lastUpdatedby;

            para[4] = new SqlParameter();
            para[4].ParameterName = "DeletedDate";
            para[4].SqlDbType = SqlDbType.Date;
            para[4].Value = System.DateTime.Now;
            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, CommandType.StoredProcedure, "SpDeletedRecurringinvoices", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeletedRecurringTicket(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[5];
            para[0] = new SqlParameter();
            para[0].ParameterName = "ContractID";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = objPropContracts.jobid;

            para[1] = new SqlParameter();
            para[1].ParameterName = "StartDate";
            para[1].SqlDbType = SqlDbType.Date;
            para[1].Value = objPropContracts.StartDt;

            para[2] = new SqlParameter();
            para[2].ParameterName = "EndDate";
            para[2].SqlDbType = SqlDbType.Date;
            para[2].Value = objPropContracts.EndDt;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Deletedby";
            para[3].SqlDbType = SqlDbType.NVarChar;
            para[3].Value = objPropContracts.lastUpdatedby;

            para[4] = new SqlParameter();
            para[4].ParameterName = "DeletedDate";
            para[4].SqlDbType = SqlDbType.Date;
            para[4].Value = System.DateTime.Now;
            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, CommandType.StoredProcedure, "SpDeletedRecurringTicket", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetRecurringInvoicesUIHistory(Contracts objPropContracts)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, "spGetRecurringInvoicesUIHistory");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetRecurringInvoicesLogs(String conn)
        {
            try
            {
                return SqlHelper.ExecuteDataset(conn, CommandType.Text, "select * from Log2 where Screen='Recurringinvoices' order by CreatedStamp desc ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetARAging360ByAsOfDate(Contracts objPropContracts, int creditFlag = 0)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = objPropContracts.Date;

                SqlParameter paraDep = new SqlParameter();
                paraDep.ParameterName = "LocType";
                paraDep.SqlDbType = SqlDbType.VarChar;
                paraDep.Value = objPropContracts.DepartmentTypes;

                SqlParameter paraCreditFlag = new SqlParameter();
                paraCreditFlag.ParameterName = "CreditFlag";
                paraCreditFlag.SqlDbType = SqlDbType.TinyInt;
                paraCreditFlag.Value = creditFlag;

                if (objPropContracts.isDBTotalService)
                {
                    return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetARAging360ByDate", paraDate, paraDep, paraCreditFlag);
                }
                else
                {
                    return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetARAging360ByDateNoneTS", paraDate, paraDep, paraCreditFlag);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetProjectGLCrossReference(Contracts objPropContracts, int dateRangeType)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                // Job info
                sb.Append("SELECT  \n");
                sb.Append("	j.*, \n");
                sb.Append("	ISNULL(BLabor,0) + ISNULL(BMat,0) + ISNULL(BOther,0) AS BExpense, \n");
                sb.Append("	r.Name AS OwnerName, \n");
                sb.Append("	l.Tag AS LocName, \n");
                sb.Append("	l.Address AS LocAddress, \n");
                sb.Append("	l.City AS LocCity, \n");
                sb.Append("	l.State AS LocState, \n");
                sb.Append("	l.Zip AS LocZip \n");
                sb.Append("FROM Job j \n");
                sb.Append("	INNER JOIN Loc l ON l.Loc = j.Loc \n");
                sb.Append("	INNER JOIN Owner o ON o.ID = j.Owner \n");
                sb.Append("	INNER JOIN Rol r ON r.ID = o.Rol \n");
                sb.Append("WHERE j.ID = " + objPropContracts.JobId + " \n");

                // Income
                sb.Append("SELECT \n");
                sb.Append("	c.Acct, \n");
                sb.Append("	c.fDesc, \n");
                sb.Append("	SUM(i.Amount) AS Amount, \n");
                sb.Append("	SUM(i.Total) AS Total \n");
                sb.Append("FROM   Invoice i \n");
                sb.Append("	INNER JOIN Trans t ON t.ID = i.TransID \n");
                sb.Append("	INNER JOIN Chart c ON c.ID = t.Acct \n");
                sb.Append("WHERE i.Ref IS NOT NULL  \n");
                sb.Append("	AND i.Job = " + objPropContracts.JobId + " \n");

                if (dateRangeType != 1)
                {
                    sb.Append("	AND i.fDate >= '" + objPropContracts.StartDate + "' \n");
                    sb.Append("	AND i.fDate <= '" + objPropContracts.EndDate + "' \n");
                }

                sb.Append("GROUP BY c.Acct, c.fDesc \n");

                // Expense
                sb.Append("SELECT   \n");
                sb.Append("	Acct, \n");
                sb.Append("	fDesc, \n");
                sb.Append(" SUM(Amount) AS Amount, \n");
                sb.Append("	SUM(Amount) AS Total, \n");
                sb.Append("	NULL AS Budget \n");
                sb.Append("FROM(   \n");

                //Material
                sb.Append("SELECT   \n");
                sb.Append("	c.Acct, \n");
                sb.Append("	c.fDesc, \n");
                sb.Append(" SUM(ji.Amount) AS Amount, \n");
                sb.Append("	SUM(ji.Amount) AS Total, \n");
                sb.Append("	NULL AS Budget \n");
                sb.Append("FROM JobI ji \n");
                sb.Append("	INNER JOIN JobTItem jt ON jt.Line = ji.Phase AND jt.Job= " + objPropContracts.JobId + " \n");
                sb.Append("	INNER JOIN BOM b ON b.JobTItemID = jt.ID AND ISNULL(ji.Labor, 0) <> 1 \n");
                sb.Append("	INNER JOIN BOMT bt ON bt.ID =b.Type AND (bt.Type = 'Materials' OR bt.Type = 'Inventory') \n");
                sb.Append(" LEFT OUTER JOIN Trans t ON ji.TransID = t.ID  \n");
                sb.Append("	LEFT OUTER JOIN Chart c on c.ID = t.Acct \n");
                sb.Append("WHERE  ji.Job = " + objPropContracts.JobId + "  \n");
                sb.Append("	AND (TransID > 0 OR ISNULL(ji.Labor, 0) = 0) \n");
                sb.Append("	AND ji.fDesc NOT IN ('Mileage on Ticket','Expenses on Ticket') \n");
                sb.Append(" AND ISNULL(ji.Type, 0) <> 0 \n");
                sb.Append(" AND (ji.TransID > 0 OR ISNULL(ji.Labor, 0) = 0) AND bt.Type <> 'Labor' \n");

                if (dateRangeType != 1)
                {
                    sb.Append("	AND ji.fDate >= '" + objPropContracts.StartDate + "' \n");
                    sb.Append("	AND ji.fDate <= '" + objPropContracts.EndDate + "' \n");
                }

                sb.Append("GROUP BY c.Acct, c.fDesc  \n");

                // Other Expense
                sb.Append("UNION  \n");
                sb.Append("SELECT \n");
                sb.Append("	c.Acct, \n");
                sb.Append("	c.fDesc, \n");
                sb.Append(" SUM(ji.Amount) AS Amount, \n");
                sb.Append("	SUM(ji.Amount) AS Total, \n");
                sb.Append("	NULL AS Budget \n");
                sb.Append("FROM JobI ji \n");
                sb.Append("	INNER JOIN JobTItem jt ON jt.Line = ji.Phase AND jt.Job= " + objPropContracts.JobId + " \n");
                sb.Append("	INNER JOIN BOM b ON b.JobTItemID = jt.ID \n");
                sb.Append("	INNER JOIN BOMT bt ON bt.ID =b.Type \n");
                sb.Append(" LEFT OUTER JOIN Trans t ON ji.TransID = t.ID  \n");
                sb.Append("	LEFT OUTER JOIN Chart c on c.ID = t.Acct \n");
                sb.Append("WHERE ji.Amount <> 0 AND ISNULL(ji.Type, 0) <> 0 \n");
                sb.Append("	AND (((TransID > 0 OR ISNULL(ji.Labor, 0) = 0) AND (bt.Type <> 'Materials' AND bt.Type <> 'Labor' AND bt.Type <> 'Inventory') \n");
                sb.Append("	    AND ji.fDesc NOT IN ('Mileage on Ticket','Expenses on Ticket')) OR ji.fDesc IN ('Mileage on Ticket','Expenses on Ticket')) \n");
                sb.Append("	AND ji.Job = " + objPropContracts.JobId + " \n");

                if (dateRangeType != 1)
                {
                    sb.Append("	AND ji.fDate >= '" + objPropContracts.StartDate + "'  \n");
                    sb.Append("	AND ji.fDate <= '" + objPropContracts.EndDate + "'  \n");
                }

                sb.Append("GROUP BY c.Acct, c.fDesc  \n");
                sb.Append(") AS temp GROUP BY Acct, fDesc  \n");

                // Labor Expense
                sb.Append("UNION  \n");
                sb.Append("SELECT  \n");
                sb.Append("	'Labor Expense' AS Acct, \n");
                sb.Append("	'' AS fDesc, \n");
                sb.Append("	ISNULL((SELECT SUM(ISNULL(ji.Amount,0)) \n");
                sb.Append("		FROM JobI ji \n");
                sb.Append("			INNER JOIN JobTItem jt ON jt.Line = ji.Phase AND jt.Job = " + objPropContracts.JobId + " \n");
                sb.Append("			INNER JOIN Bom b ON b.JobTItemID = jt.ID \n");
                sb.Append("			INNER JOIN BOMT bt ON bt.ID = b.Type AND bt.Type='Labor' AND ISNULL(ji.Labor,0) <> 1 \n");
                sb.Append("		WHERE ji.Job = " + objPropContracts.JobId + " AND ji.fDesc NOT IN ('Mileage on Ticket','Expenses on Ticket') \n");
                sb.Append("		AND ISNULL(ji.Type, 0) <> 0    \n");

                if (dateRangeType != 1)
                {
                    sb.Append("		AND ji.fDate >= '" + objPropContracts.StartDate + "'  \n");
                    sb.Append("		AND ji.fDate <= '" + objPropContracts.EndDate + "'  \n");
                }

                sb.Append("	), 0) +  \n");
                sb.Append("	ISNULL((SELECT SUM(ISNULL(ji.Amount,0)) \n");
                sb.Append("		FROM JobI ji \n");
                sb.Append("			INNER JOIN JobTItem jt ON jt.Line= ji.Phase AND jt.Job = " + objPropContracts.JobId + " \n");
                sb.Append("			INNER JOIN Bom b ON b.JobTItemID = jt.ID \n");
                sb.Append("			INNER JOIN BOMT bt ON bt.ID = b.Type AND ISNULL(ji.Labor,0) = 1 \n");
                sb.Append("		WHERE ji.Job = " + objPropContracts.JobId + " AND ji.fDesc NOT IN ('Mileage on Ticket','Expenses on Ticket') \n");
                sb.Append("		AND ISNULL(ji.Type,0) <> 0 \n");

                if (dateRangeType != 1)
                {
                    sb.Append("		AND ji.fDate >= '" + objPropContracts.StartDate + "'  \n");
                    sb.Append("		AND ji.fDate <= '" + objPropContracts.EndDate + "'  \n");
                }

                sb.Append("	), 0) AS Amount, \n");
                sb.Append("	0 AS Total,  \n");
                sb.Append("	(SELECT ISNULL(BLabor,0) FROM Job WHERE ID = " + objPropContracts.JobId + ") AS Budget  \n");

                // Retainage Receivable account
                sb.Append("SELECT c.* \n");
                sb.Append("FROM Job j \n");
                sb.Append("	INNER JOIN JobT jt ON j.Template = jt.ID \n");
                sb.Append("	INNER JOIN Chart c ON c.ID = jt.RetainageReceivable \n");
                sb.Append("WHERE j.ID = " + objPropContracts.JobId + " \n");

                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetARAging360ByAsOfDate(GetARAging360ByAsOfDateParam _GetARAging360ByAsOfDate, string ConnectionString)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = _GetARAging360ByAsOfDate.Date;


                if (_GetARAging360ByAsOfDate.isDBTotalService)
                {
                    return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetARAging360ByDate", paraDate);
                }
                else
                {
                    return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetARAging360ByDateNoneTS", paraDate);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCreditByID(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT ar.Loc,ar.fDate,ar.Original,ar.Balance,l.Tag as locname \n");
            varname1.Append("       ,(SELECT TOP 1 Name \n");
            varname1.Append("        FROM   rol \n");
            varname1.Append("        WHERE  ID = (SELECT TOP 1 Rol \n");
            varname1.Append("                     FROM   Owner \n");
            varname1.Append("                     WHERE  ID = l.Owner)) AS customerName \n");
            varname1.Append("FROM   OpenAR ar \n");
            varname1.Append("       INNER JOIN Loc l \n");
            varname1.Append("               ON l.Loc = ar.Loc \n"); varname1.Append("WHERE ar.type=2 and ar.Ref =  " + objPropContracts.InvoiceID + "");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetARAgingByBusinessTypeOfDate(Contracts objPropContracts, int creditFlag = 0)
        {
            try
            {
                SqlParameter paraDate = new SqlParameter();
                paraDate.ParameterName = "fDate";
                paraDate.SqlDbType = SqlDbType.DateTime;
                paraDate.Value = objPropContracts.Date;

                SqlParameter creditFlagPara = new SqlParameter();
                creditFlagPara.ParameterName = "CreditFlag";
                creditFlagPara.SqlDbType = SqlDbType.TinyInt;
                creditFlagPara.Value = creditFlag;



                if (objPropContracts.isDBTotalService)
                {
                    return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetARAgingByBusinessType", paraDate, creditFlagPara);
                }
                else
                {
                    return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetARAgingByBusinessTypeNoneTS", paraDate, creditFlagPara);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDepositByID(Contracts objPropContracts)
        {
            try
            {

                SqlParameter para = new SqlParameter();
                para.ParameterName = "ID";
                para.SqlDbType = SqlDbType.Int;
                para.Value = objPropContracts.InvoiceID;

                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spDepositInfoByID", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetContractsExpireIn10Days(Contracts objPropContracts)
        {
            try
            {

                //SqlParameter para = new SqlParameter();
                //para.ParameterName = "ID";
                //para.SqlDbType = SqlDbType.Int;
                //para.Value = objPropContracts.InvoiceID;

                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spGetContractsExpireIn10Days");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}

