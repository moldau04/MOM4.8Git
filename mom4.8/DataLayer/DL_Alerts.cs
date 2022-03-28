using System;
using System.Text;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using BusinessEntity;

namespace DataLayer
{
    public class DL_Alerts
    {
        public DataSet GetAlertType(Alerts objAlert)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objAlert.ConnConfig, CommandType.Text, "Select * from tblAlertTypes order by alertname");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetAlertType(GetAlertTypeParam _GetAlertType, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "Select * from tblAlertTypes order by alertname");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getAlertContactSearch(Alerts objAlert)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT fDesc AS name, \n");
            varname1.Append("       email, \n");
            varname1.Append("       ''    AS pager, 'phone' as screenname, id as screenid \n");
            varname1.Append("FROM   phone \n");
            varname1.Append("WHERE  rol = (SELECT rol \n");
            varname1.Append("              FROM   loc \n");
            varname1.Append("              WHERE  loc = 1) \n");
            if (!string.IsNullOrEmpty(objAlert.SearchValue))
            {
                varname1.Append(" and fdesc like '%" + objAlert.SearchValue + "%'");
            }
            varname1.Append("UNION \n");
            varname1.Append("SELECT r.name, \n");
            varname1.Append("       r.email, \n");
            varname1.Append("       Pager,'emp' as screenname, e.ID as screenid \n");
            varname1.Append("FROM   emp e \n");
            varname1.Append("       INNER JOIN rol r \n");
            varname1.Append("               ON r.id = e.rol \n");
            varname1.Append("WHERE  Status = 0 \n");
            if (!string.IsNullOrEmpty(objAlert.SearchValue))
            {
                varname1.Append(" and r.NAME like '%" + objAlert.SearchValue + "%'");
            }

            varname1.Append("ORDER  BY screenname ,name ");

            try
            {
                return SqlHelper.ExecuteDataset(objAlert.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getAlerts(Alerts objAlert)
        {
            StringBuilder varname11 = new StringBuilder();
            varname11.Append(" \n");
            varname11.Append("SELECT * \n");
            varname11.Append("FROM   tblAlerts \n");
            varname11.Append("WHERE  ScreenName = 'Loc' \n");
            varname11.Append("       AND ScreenID = " + objAlert.loc);

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT *, \n");
            varname1.Append("       (SELECT TOP 1 alertcode \n");
            varname1.Append("        FROM   tblAlerts \n");
            varname1.Append("        WHERE  AlertID = c.AlertID) as AlertCode, \n");
            varname1.Append("       CASE screenname \n");
            varname1.Append("         WHEN 'emp' THEN (SELECT TOP 1 NAME \n");
            varname1.Append("                          FROM   emp \n");
            varname1.Append("                          WHERE  id = ScreenID) \n");
            varname1.Append("         WHEN 'tbluser' THEN (SELECT TOP 1 fuser \n");
            varname1.Append("                              FROM   tblUser \n");
            varname1.Append("                              WHERE  id = ScreenID) \n");
            varname1.Append("         WHEN 'phone' THEN (SELECT TOP 1 fdesc \n");
            varname1.Append("                            FROM   phone \n");
            varname1.Append("                            WHERE  id = ScreenID) \n");
            varname1.Append("         ELSE '' \n");
            varname1.Append("       END AS NAME \n");
            varname1.Append("FROM   tblAlertContacts c \n");
            varname1.Append("WHERE  AlertID IN (SELECT AlertID \n");
            varname1.Append("                   FROM   tblAlerts \n");
            varname1.Append("                   WHERE  ScreenName = 'Loc' \n");
            varname1.Append("                          AND ScreenID = " + objAlert.loc +") ");
                        
            try
            {
                return SqlHelper.ExecuteDataset(objAlert.ConnConfig, CommandType.Text, varname11.ToString() + varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getAlerts(GetAlertsParam _GetAlerts, string ConnectionString)
        {
            StringBuilder varname11 = new StringBuilder();
            varname11.Append(" \n");
            varname11.Append("SELECT * \n");
            varname11.Append("FROM   tblAlerts \n");
            varname11.Append("WHERE  ScreenName = 'Loc' \n");
            varname11.Append("       AND ScreenID = " + _GetAlerts.loc);

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT *, \n");
            varname1.Append("       (SELECT TOP 1 alertcode \n");
            varname1.Append("        FROM   tblAlerts \n");
            varname1.Append("        WHERE  AlertID = c.AlertID) as AlertCode, \n");
            varname1.Append("       CASE screenname \n");
            varname1.Append("         WHEN 'emp' THEN (SELECT TOP 1 NAME \n");
            varname1.Append("                          FROM   emp \n");
            varname1.Append("                          WHERE  id = ScreenID) \n");
            varname1.Append("         WHEN 'tbluser' THEN (SELECT TOP 1 fuser \n");
            varname1.Append("                              FROM   tblUser \n");
            varname1.Append("                              WHERE  id = ScreenID) \n");
            varname1.Append("         WHEN 'phone' THEN (SELECT TOP 1 fdesc \n");
            varname1.Append("                            FROM   phone \n");
            varname1.Append("                            WHERE  id = ScreenID) \n");
            varname1.Append("         ELSE '' \n");
            varname1.Append("       END AS NAME \n");
            varname1.Append("FROM   tblAlertContacts c \n");
            varname1.Append("WHERE  AlertID IN (SELECT AlertID \n");
            varname1.Append("                   FROM   tblAlerts \n");
            varname1.Append("                   WHERE  ScreenName = 'Loc' \n");
            varname1.Append("                          AND ScreenID = " + _GetAlerts.loc + ") ");

            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname11.ToString() + varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
