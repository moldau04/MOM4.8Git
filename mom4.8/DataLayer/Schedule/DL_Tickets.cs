using System;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using BusinessEntity;
using System.Collections.Generic;
using System.Text;
using DataLayer.Utility;

namespace DataLayer.SchduleModule
{
    public class DL_Tickets
    {

        private bool CheckFilterHasCommaDelimited(List<RetainFilter> filters)
        {
            bool isFilterHasCommaDelimited = false;
            if (filters != null && filters.Count > 0)
            {
                foreach (var items in filters)
                {
                    if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                    {
                        /// Int Filter
                        int FilterValue = 0; string[] filterArrayValue;
                        if (items.FilterColumn == "ID")
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


        /// <summary>
        /// GetTicketListData
        /// </summary>
        /// <param name="objPropMapData"></param>
        /// <param name="filters"></param>
        /// <param name="IsSalesAsigned"></param>
        /// <param name="strORDERBY"></param>
        /// <param name="RadGvTicketListminimumRows"></param>
        /// <param name="RadGvTicketListmaximumRows"></param>
        /// <returns></returns>

        /// <summary>
        /// GetTicketListData
        /// </summary>
        /// <param name="objPropMapData"></param>
        /// <param name="filters"></param>
        /// <param name="IsSalesAsigned"></param>
        /// <param name="strORDERBY"></param>
        /// <param name="RadGvTicketListminimumRows"></param>
        /// <param name="RadGvTicketListmaximumRows"></param>
        /// <returns></returns>
        public DataSet GetTicketListData(MapData objPropMapData, List<RetainFilter> filters, string fromDate, string toDate,
            Int32 IsSalesAsigned = 0, string strORDERBY = "EDATE ASC", int RadGvTicketListminimumRows = 0,
            int RadGvTicketListmaximumRows = 50, bool inclCustomField = false)
        {

            bool isFilterHasCommaDelimited = CheckFilterHasCommaDelimited(filters);
            if (!isFilterHasCommaDelimited && !string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                objPropMapData.StartDate = DateTime.Parse(fromDate);
                objPropMapData.EndDate = DateTime.Parse(toDate);
            }
            else
            {
                objPropMapData.StartDate = System.DateTime.MinValue;
                objPropMapData.EndDate = System.DateTime.MinValue;
            }
            //else
            //{
            //    objPropMapData.SearchBy = "";
            //    objPropMapData.SearchValue = "";
            //}

            if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && objPropMapData.SearchBy.Equals("t.ID", StringComparison.InvariantCultureIgnoreCase)
                && !string.IsNullOrEmpty(objPropMapData.SearchValue))
            {
                objPropMapData.StartDate = System.DateTime.MinValue;
                objPropMapData.EndDate = System.DateTime.MinValue;
            }

            string str = " \n   WITH FINALTICKETDATA  AS (   SELECT ROW_NUMBER() OVER(ORDER BY " + strORDERBY + " ) AS ROWNO,* FROM (";

            #region Status != Assigned  $$$ Get Ticket from Ticketo and TicketDPDA
            if (objPropMapData.Status != 1)
            {
                #region FilterReview != "1"
                if (objPropMapData.FilterReview != "1")
                {
                    #region Column
                    str += "\n    SELECT CASE WHEN t.Recurring IS NULL THEN 0 ELSE 1 END isRecurring , " +
                        "\n   isnull(dp.OT, 0.00) as OT, " +
                        "\n   isnull(dp.Reg, 0.00) as Reg, " +
                        "\n   isnull(dp.NT, 0.00) as NT, " +
                        "\n   isnull(dp.DT, 0.00) as DT, " +
                        "\n   isnull(dp.TT, 0.00) as TT, " +
                        "\n     (select( case  when ro.Name IS NULL  then 'Unassigned'   when tblwork.fdesc is null then  ro.Name    when tblwork.fdesc = ro.Name then ro.Name  else ro.Name +' - ' + tblwork.fdesc   end)from tblwork where tblwork.id = ro.mech   )  AS Name, " +
                        "\n   t.who," +
                        "\n  t.CPhone," +
                        "\n   t.lid, l.id as locid," +
                        "\n   assigned, ( l.address + ', ' + l.city + ', ' + l.state + ', ' + l.zip ) AS fulladdress," +
                        "\n  l.city," +
                        "\n  l.state," +
                        "\n  l.zip," +
                        "\n   t.WorkOrder," +
                        "\n  dp.Total, 0 AS ClearCheck," +
                        "\n   case when t.charge =1 then 1  when dp.charge =1 then 1 else 0 end as charge, " +
                        "\n  t.fDesc," +
                        "\n   t.TimeRoute," +
                        "\n   t.TimeSite, " +
                        "\n  t.TimeComp," +
                        "\n   CASE WHEN EXISTS (SELECT 1 FROM TicketDPDA WITH(NOLOCK) WHERE ID = t.ID  AND t.Assigned=4) THEN 2 ELSE 0 END AS comp," +
                        " UPPER(dwork) AS dwork, " +

                        "\n   t.ID, r.Name AS customername," +
                        "\n   r.EN," +
                        "\n   isnull(B.Name, '') As Company,";
                    str += "\n   l.Tag  AS locname," +
                        "\n   l.Address  AS address," +
                        "\n   l.Custom1 as LocCustom1," +
                        "\n   l.Custom1 as LocCustom2," +
                        "\n  r.phone," +
                        "\n  r.Email," +
                        "\n   t.Cat, " +
                        "\n   t.Level, " +
                          ///"\n  t.EDate AS edate,  " + 
                          "\n  t.EDate AS edate,  CONVERT(VARCHAR(10), t.EDate, 101) as EdateWithDateOnly," +
                        "\n   t.CDate, " +
                        "\n  dp.descres," +
                        "\n   CASE " +
                        "WHEN assigned = 0 THEN 'Un-Assigned' " +
                        "WHEN assigned = 1 THEN 'Assigned' " +
                        "WHEN assigned = 2 THEN 'Enroute' " +
                        "WHEN assigned = 3 THEN 'Onsite' " +
                        "WHEN assigned = 4 THEN 'Completed' " +
                        "WHEN assigned = 5 THEN 'Hold' " +
                        "WHEN assigned = 6 THEN 'Voided' " +
                        "END AS assignname," +
                        "\n   t.Est, Isnull(dp.Total, 0.00) AS Tottime ,";

                    #region 24 HOURS FEATURE

                    str += @"  Round(CONVERT(NUMERIC(30, 2), ( Isnull(dp.Total, 0.00) - ( CONVERT(FLOAT, Datediff(MILLISECOND, dp.TimeRoute, ( CASE
                                                                                                                   WHEN(Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME)
                                                                                                                           AND Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME)) THEN Dateadd(day, 2, dp.TimeComp)
                                                                                                                   ELSE((CASE
                                                                                                                             WHEN(Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME)
                                                                                                                                    OR Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME)) THEN Dateadd(day, 1, dp.TimeComp)
                                                                                                                             ELSE dp.TimeComp
                                                                                                                           END))
                                                                                                                 END ))) / 1000 / 60 / 60 ) )), 1)  AS timediff, ";
                    #endregion

                    str += "\n   (isnull(dp.emile,0)-isnull(dp.smile,0)) as mileage " +
                        ",(select top 1 count(1) as signatureCount  from pdaticketsignature WITH(NOLOCK) where pdaticketid=t.ID ) as signatureCount, " +
                        "\n  (select count(1) from documents WITH(NOLOCK) where screen='Ticket' and screenid=t.id) as DocumentCount," +
                        "\n   t.fwork as workerid, " +
                        "\n  (select top 1 * from dbo.split(descres,'|')) as description," +
                        "\n   (select top 1 * from dbo.split(t.fdesc,'|')) as fdescreason," +
                        "\n   0 as invoice," +
                        "\n   isnull(Confirmed,0) as Confirmed," +
                        "\n   '' as manualinvoice,'' AS statusName, convert(varchar(20),dp.Invoice) as invoiceno," +
                        "\n   t.owner as ownerid, '' as QBinvoiceid," +
                        "\n   0 as TransferTime, ";

                    str += "\n    isnull(l.dispalert,0)as dispalert," +
                        "\n   isnull(l.credit,0)as credithold, " +
                        "\n  isnull(t.high,0) as high," +
                        "\n  e.id as unitid,";

                    str += "\n   (select  STUFF((SELECT ', ' + CAST(e.Unit AS VARCHAR(1000)) ";
                    str += "\n  FROM elev e WITH(NOLOCK) where  e.id in ";
                    str += "\n  (select me.elev_id from multiple_equipments me WITH(NOLOCK) where me.ticket_id = t.ID)";
                    str += "\n   FOR XML PATH(''), Type)  .value('.', 'VARCHAR(1000)'),1,2,' ') ) as unit, ";



                    str += "\n    ( jt.type) as department," +

                        "\n    rtrim(ltrim(t.bremarks)) as bremarks,";

                    str += "\n  isnull(EmailNotified,0) as EmailNotified, EmailTime,";

                    str += "\n      t.Job";

                    if (inclCustomField)
                    {
                        str += "\n ,(SELECT cj.Value FROM tblCustomJob cj LEFT JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID WHERE cf.Label = 'Passed Inspection' AND cj.JobID = t.Job) AS PassedInspection ";
                    }
                    else
                    {
                        str += "\n ,'' AS PassedInspection";
                    }

                    str += "\n    , ( convert(varchar(20),t.Job )+'-'+ j.fdesc) as ProjectDescription ";
                    str += "\n    , j.SRemarks as SpecialRemark";
                    str += "\n  , t.Custom3,  t.Custom4,  t.Custom6, t.Custom7 ";
                    str += "\n  , t.fBy , 0 ClearPR ";
                    str += "\n  , CONVERT(VARCHAR(10), i.fDate, 101) as InvoiceDate , 0 IsPartsUsedIcon , isnull((select top 1 1 from POItem where (POItem.Ticket)=t.id) ,0) ispoicon ";
                    #endregion

                    str += "\n   FROM ticketo t WITH(NOLOCK) " +

                        "\n  LEFT OUTER JOIN TicketDPDA dp WITH(NOLOCK) " +
                        "\n  ON t.ID = dp.ID inner join Loc l WITH(NOLOCK)" +
                        "\n  on l.Loc=t.lid  inner join Owner o WITH(NOLOCK)" +
                        "\n  on l.Owner=o.ID INNER JOIN Rol r WITH(NOLOCK)" +
                        "\n  ON r.ID = o.Rol " +
                        "\n  LEFT OUTER JOIN Branch B WITH(NOLOCK) " +
                        "\n  on B.ID = r.EN  " +
                        "\n  left outer join Elev e WITH(NOLOCK) " +
                        "\n  on e.ID=t.LElev " +
                        "\n  left join Route ro WITH(NOLOCK) on ro.ID=l.Route " +
                        "\n  left join job j WITH(NOLOCK) on j.ID = t.Job " +
                        "\n  left join jobtype jt WITH(NOLOCK) on jt.ID = t.type " +
                        "\n  left join Invoice i WITH(NOLOCK) on dp.Invoice = i.Ref ";

                    //assigned NOT IN ( 0 )
                    if (objPropMapData.EN == 1) // check for company
                    {
                        str += "\n   LEFT OUTER JOIN tblUserCo UC WITH(NOLOCK) on UC.CompanyID = r.EN ";
                    }
                    str += "\n   WHERE t.id is not null and t.owner is not null  ";

                    if (objPropMapData.IsAssignedProject == true) // check for company
                    {
                        str += "\n   and j.ProjectManagerUserID= " + objPropMapData.EmpID;
                    }
                    #region FILTER FOR Ticket O

                    if (objPropMapData.IsList != 1)
                    {
                        str += " and assigned NOT IN ( 0 )";
                    }
                    if (objPropMapData.EN == 1) // check for company
                    {
                        str += " and UC.IsSel = 1 and UC.UserID =" + objPropMapData.UserID;
                    }
                    if (objPropMapData.Assigned != -1)//&& objPropMapData.Assigned != 0
                    {
                        if (objPropMapData.Assigned == -2)
                        {
                            str += " and t.Assigned <> 4";
                        }
                        else
                        {
                            str += " and t.Assigned=" + objPropMapData.Assigned;
                        }
                    }

                    if (!isFilterHasCommaDelimited && objPropMapData.InvoiceDate == null)
                    {
                        if (objPropMapData.StartDate != System.DateTime.MinValue)
                        {
                            str += " and isnull(t.edate,t.cdate) >='" + objPropMapData.StartDate.ToString("MM/dd/yyyy") + "'";
                        }
                        if (objPropMapData.EndDate != System.DateTime.MinValue)
                        {
                            str += " and isnull(t.edate,t.cdate) <'" + objPropMapData.EndDate.AddDays(1).ToString("MM/dd/yyyy") + "'";
                        }
                    }


                    if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                    {

                        if (objPropMapData.Worker == "Active")
                        {
                            str += " and t.fwork in (select w.ID from tblWork w WITH(NOLOCK) where  w.Status=0 )";
                        }
                        else if (objPropMapData.Worker == "Inactive")
                        {
                            str += " and t.fwork in (select w.ID from tblWork w WITH(NOLOCK) where  w.Status=1 )";
                        }
                        else
                        {
                            str += " and (select w.fdesc from tblWork w WITH(NOLOCK) where w.ID=t.fwork )='" + objPropMapData.Worker.Replace("'", "''") + "'";
                        }
                    }
                    if (objPropMapData.LocID != 0)
                    {
                        str += " and t.LID=" + objPropMapData.LocID;
                    }
                    if (objPropMapData.CustID != 0)
                    {
                        str += " and t.Owner=" + objPropMapData.CustID;
                    }
                    if (objPropMapData.jobid != 0)
                    {
                        str += " and t.job =" + objPropMapData.jobid;
                    }
                    if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
                    {
                        str += " and (isnull(t.charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge);
                        if (objPropMapData.FilterCharge == "1")
                            str += " or isnull(Invoice,0) <> 0)";
                        else
                            str += " and isnull(Invoice,0) = 0)";
                    }
                    if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                    {
                        str += " and (select Super from tblWork w WITH(NOLOCK) where w.fdesc=t.dwork ) ='" + objPropMapData.Supervisor + "'";
                    }
                    if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
                    {
                        str += " and (select Super from tblWork w WITH(NOLOCK) where w.fdesc=t.dwork ) <>'" + objPropMapData.NonSuper + "'";
                    }

                    
                    if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
                    {
                        str += " and t.workorder='" + objPropMapData.Workorder + "'";
                    }
                    if (objPropMapData.Department != -1)
                    {
                        str += " and t.type=" + objPropMapData.Department;
                    }
                    if (objPropMapData.Category != string.Empty && objPropMapData.Category != null)
                    {

                        str += " and t.cat in (" + objPropMapData.Category + ")";
                    }
                    #region ////Filter for ticketo IF ticket Create from Portal
                    if (objPropMapData.IsPortal != string.Empty && objPropMapData.IsPortal != null)
                    {
                        if (objPropMapData.IsPortal == "1")
                        {
                            str += " and t.fBy='portal' ";
                        }
                        if (objPropMapData.IsPortal == "0")
                        {
                            str += " and t.fBy <> 'portal' ";
                        }
                    }
                    #endregion

                    #region default route check

                    if (!string.IsNullOrEmpty(objPropMapData.Route))
                    {
                        if (Convert.ToInt32(objPropMapData.Route) == 0)
                        {
                            str += " and l.Route=0 ";
                        }
                        else
                        {
                            str += " and ro.ID=" + Convert.ToInt32(objPropMapData.Route) + " ";
                        }
                    }

                    #endregion

                    if (objPropMapData.Bremarks != null && objPropMapData.Bremarks != string.Empty)
                    {
                        if (objPropMapData.Bremarks == "1")
                            str += " and isnull( rtrim(ltrim(t.bremarks)),'')<>''";
                        else
                            str += " and isnull( rtrim(ltrim(t.bremarks)),'')=''";
                    }

                    if (objPropMapData.SearchBy != "" && objPropMapData.SearchBy != null && !string.IsNullOrWhiteSpace(objPropMapData.SearchValue))
                    {
                        string SearchBy = objPropMapData.SearchBy.Replace("t.descres", "dp.descres");

                        if (SearchBy == "t.ID")
                        {
                            str += " and " + SearchBy + " in (" + UpdateListTicketIDsForSearch(objPropMapData.SearchValue) + ")";
                        }
                        else if (SearchBy == "t.Level")
                        {
                            str += "\n  and t.Level in (" + objPropMapData.SearchValue + ")";
                        }
                        else
                        {
                            str += " and " + SearchBy + " like '%" + objPropMapData.SearchValue + "%'";
                        }
                    }

                    if (objPropMapData.InvoiceID != 0)
                    {
                        if (objPropMapData.InvoiceID == 1)
                        {
                            str += " and isnull(Invoice,0) <> 0";
                        }
                        else if (objPropMapData.InvoiceID == 2)
                        {
                            str += " and isnull(Invoice,0) = 0 and isnull(dp.charge,0)= 1";
                        }
                    }
                    //  If the user is a salesperson or Second salesperson only show assigned  locations,Ticket.
                    if (IsSalesAsigned > 0)
                    {
                        str += " AND  (  l.Terr=(" + "select id FROM  Terr WITH(NOLOCK) WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSalesAsigned + ")) or  isnull(l.Terr2,0)=(" + "select id FROM  Terr WITH(NOLOCK) WHERE Name=(SELECT fUser FROM  tblUser WITH(NOLOCK) WHERE id=" + IsSalesAsigned + "))) ";
                    }

                    if (filters.Count > 0)
                    {
                        foreach (var items in filters)
                        {
                            if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                            {
                                /// Int Filter
                                int FilterValue = 0; string[] filterArrayValue;
                                StringBuilder filteredQuery = new StringBuilder();
                                if (items.FilterColumn == "ID")
                                {
                                    filterArrayValue = items.FilterValue.ToString().Split(',');
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
                                    str += " and t.ID  in (" + filteredQuery.ToString() + ")";
                                }

                                if (items.FilterColumn == "Job" & int.TryParse(items.FilterValue, out FilterValue))
                                {
                                    str += " and t.job  = " + FilterValue;
                                }

                                /// string Filter

                                if (items.FilterColumn == "WorkOrder")
                                {
                                    str += " and t.WorkOrder  = '" + items.FilterValue + "'";
                                }


                                if (items.FilterColumn == "locname")
                                {
                                    str += " and l.Tag  like '%" + items.FilterValue + "%'";
                                }
                                if (items.FilterColumn == "City")
                                {
                                    str += " and l.City  like '%" + items.FilterValue + "%'";
                                }
                                if (items.FilterColumn == "cat")
                                {
                                    str += " and t.cat  like '%" + items.FilterValue + "%'";
                                }


                                if (items.FilterColumn == "fullAddress")
                                {
                                    str += " and ( l.address + ', ' + l.city + ', ' + l.state + ', ' + l.zip )  like '%" + items.FilterValue + "%'";
                                }

                            }
                        }
                    }

                    #endregion End Filter For TicketO

                    #region LocID == 0

                    if (objPropMapData.LocID == 0)
                    {
                        str += "\n  \n  \n   Union all ";

                        str += "\n   SELECT  CASE WHEN t.Recurring IS NULL THEN 0 ELSE 1 END isRecurring , " +
                            "\n  isnull(dp.OT, 0.00), " +
                            "\n  isnull(dp.Reg, 0.00), " +
                            "\n  isnull(dp.NT, 0.00), " +
                            "\n  isnull(dp.DT, 0.00), " +
                            "\n  isnull(dp.TT, 0.00) as TT, " +
                            "\n  '', " +
                            "\n  t.who," +
                            "\n  t.CPhone," +
                            "\n   t.lid, '--' as locid," +
                            "\n   assigned, ( r.address + ', ' + r.city + ', ' + r.state + ', ' + r.zip ) AS fulladdress, " +
                            "\n  l.city," +
                            "\n  l.state," +
                            "\n  l.zip," +
                            "\n  t.WorkOrder , " +
                            "\n  dp.Total, 0 AS ClearCheck, " +
                            "\n  case when t.charge =1 then 1  when dp.charge =1 then 1 else 0 end as charge, t.fDesc," +
                            "\n   t.TimeRoute, " +
                            "\n  t.TimeSite, " +
                            "\n  t.TimeComp," +
                            "\n   CASE WHEN EXISTS (SELECT 1 FROM TicketDPDA  WITH(NOLOCK) WHERE ID = t.ID and t.Assigned=4) THEN 2 ELSE 0 END AS comp, " +
                            "\n  UPPER(dwork) AS dwork," +

                            "\n  t.ID," +
                            "\n   r.Name AS customername, " +
                            "\n   r.EN, isnull(B.Name, '') As Company,";
                        str += "\n   r.name  AS locname," +
                            "\n   l.Address  AS address," +
                            "\n   l.Custom1 as LocCustom1," +
                            "\n   l.Custom1 as LocCustom2," +
                            "\n   (select top 1 phone from rol lr where lr.id = l.rol) phone, " +
                            "\n   (select top 1 Email from rol lr where lr.id = l.rol) Email, " +
                            "\n  t.Cat, " +
                            "\n   t.Level, " +
                            // "\n  t.EDate AS edate," +
                            "\n  t.EDate AS edate,  CONVERT(VARCHAR(10), t.EDate, 101) as EdateWithDateOnly," +
                            "\n   t.CDate, dp.descres, " +
                            "\n  CASE " +
                            "WHEN assigned = 0 THEN 'Un-Assigned' " +
                            "WHEN assigned = 1 THEN 'Assigned' " +
                            "WHEN assigned = 2 THEN 'Enroute' " +
                            "WHEN assigned = 3 THEN 'Onsite' " +
                            "WHEN assigned = 4 THEN 'Completed' " +
                            "WHEN assigned = 5 THEN 'Hold' " +
                            "WHEN assigned = 6 THEN 'Voided' " +
                            "END AS assignname, t.Est, Isnull(dp.Total, 0.00) AS Tottime ,";

                        #region 24 Hours Feature 

                        str += @"  Round(CONVERT(NUMERIC(30, 2), ( Isnull(dp.Total, 0.00) - ( CONVERT(FLOAT, Datediff(MILLISECOND, dp.TimeRoute, ( CASE
                                                                                                                   WHEN ( Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME)
                                                                                                                           AND Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME) ) THEN Dateadd(day, 2, dp.TimeComp)
                                                                                                                   ELSE (( CASE
                                                                                                                             WHEN ( Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME)
                                                                                                                                    OR Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME) ) THEN Dateadd(day, 1, dp.TimeComp)
                                                                                                                             ELSE dp.TimeComp
                                                                                                                           END ))
                                                                                                                 END ))) / 1000 / 60 / 60 ) )), 1)  AS timediff,";

                        #endregion




                        str += "\n   (isnull(dp.emile,0)-isnull(dp.smile,0)) as mileage ," +
                            "\n  (select top 1 count(1) as signatureCount  from pdaticketsignature WITH(NOLOCK) where pdaticketid=t.ID ) as signatureCount," +
                            "\n   (select count(1) from documents WITH(NOLOCK) where screen='Ticket' and screenid=t.id) as DocumentCount, " +
                            "\n  t.fwork as workerid," +
                            "\n   (select top 1 * from dbo.split(descres,'|')) as description, " +
                            "\n  (select top 1 * from dbo.split(t.fdesc,'|')) as fdescreason, 0 as invoice, " +
                            "\n  isnull(Confirmed,0) as Confirmed," +
                            "\n   '' as manualinvoice,'' AS statusName, convert(varchar(20),dp.invoice) as invoiceno," +
                            "\n   t.owner as ownerid, " +
                            "\n  '' as QBinvoiceid," +
                            "\n   0 as TransferTime, ";

                        str += "\n    0 as dispalert, " +
                            "\n  0 as credithold, " +
                            "\n  0 as high,e.id as unitid,";

                        str += "\n   (select  STUFF((SELECT ', ' + CAST(e.Unit AS VARCHAR(1000)) ";
                        str += "\n  FROM elev e WITH(NOLOCK) where  e.id in ";
                        str += "\n  (select me.elev_id from multiple_equipments me WITH(NOLOCK) where me.ticket_id = t.ID)";
                        str += "\n   FOR XML PATH(''), Type)  .value('.', 'VARCHAR(1000)'),1,2,' ') ) as unit, ";

                        str += "\n    (jt.type) as department, " +
                            " rtrim(ltrim(t.bremarks)) as bremarks,";

                        str += "\n   isnull(EmailNotified,0) as EmailNotified, EmailTime,";

                        str += "\n      t.Job ";

                        if (inclCustomField)
                        {
                            str += "\n ,(SELECT cj.Value FROM tblCustomJob cj LEFT JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID WHERE cf.Label = 'Passed Inspection' AND cj.JobID = t.Job) AS PassedInspection ";
                        }
                        else
                        {
                            str += "\n ,'' AS PassedInspection";
                        }

                        str += "\n      , (  convert(varchar(20),t.Job )+'-'+ j.fdesc) as ProjectDescription ";
                        str += "\n,j.SRemarks as SpecialRemark";
                        str += "\n  ,   t.Custom3,  t.Custom4,   t.Custom6, t.Custom7 ";
                        str += "\n  , t.fBy , 0 ClearPR, ";
                        str += "\n  CONVERT(VARCHAR(10), i.fDate, 101) as InvoiceDate , 0 IsPartsUsedIcon , isnull((select top 1 1 from POItem where (POItem.Ticket)=t.id) ,0) ispOicon   ";
                        str += "\n   FROM ticketo t WITH(NOLOCK) " +
                            "\n  LEFT OUTER JOIN TicketDPDA dp WITH(NOLOCK) ON t.ID = dp.ID " +
                            "\n  inner join prospect l WITH(NOLOCK) on l.ID=t.lid " +
                            "\n  INNER JOIN Rol r WITH(NOLOCK) ON r.ID = l.Rol " +
                            "\n  LEFT OUTER JOIN Branch B WITH(NOLOCK) on B.ID = r.EN  " +
                            "\n  left outer join Elev e WITH(NOLOCK) on e.ID=t.LElev  " +
                            "\n  left join job j WITH(NOLOCK) on j.ID = t.Job " +
                            "\n  left join jobtype jt WITH(NOLOCK) on jt.ID = t.type " +
                            "\n  left join Invoice i WITH(NOLOCK) on dp.Invoice = i.Ref ";
                        if (objPropMapData.EN == 1) // check for company
                        {
                            str += "\n   LEFT OUTER JOIN tblUserCo UC WITH(NOLOCK) on UC.CompanyID = r.EN ";
                        }
                        str += "\n   WHERE (t.id is not null and t.owner is null and t.LType=1  ";

                        if (objPropMapData.IsAssignedProject == true) // check for company
                        {
                            str += "\n   and j.ProjectManagerUserID= " + objPropMapData.EmpID;
                        }

                        #region FILTER FOR TICKET_DPDA
                        if (objPropMapData.IsList != 1)
                        {
                            str += " and assigned NOT IN ( 0 )";
                        }
                        if (objPropMapData.EN == 1) // check for company
                        {
                            str += " and UC.IsSel = 1 and UC.UserID =" + objPropMapData.UserID;
                        }
                        if (objPropMapData.Assigned != -1)
                        {
                            if (objPropMapData.Assigned == -2)
                            {
                                str += " and t.Assigned <> 4";
                            }
                            else
                            {
                                str += " and t.Assigned=" + objPropMapData.Assigned;
                            }
                        }
                        if (!isFilterHasCommaDelimited && objPropMapData.InvoiceDate == null)
                        {
                            if (objPropMapData.StartDate != System.DateTime.MinValue)
                            {
                                str += " and isnull(t.edate,t.cdate) >='" + objPropMapData.StartDate + "'";
                            }
                            if (objPropMapData.EndDate != System.DateTime.MinValue)
                            {
                                str += " and isnull(t.edate,t.cdate) <'" + objPropMapData.EndDate.AddDays(1) + "'";
                            }

                        }

                        if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                        {

                            if (objPropMapData.Worker == "Active")
                            {
                                str += " and t.fwork in (select w.ID from tblWork w WITH(NOLOCK) where  w.Status=0 )";
                            }
                            else if (objPropMapData.Worker == "Inactive")
                            {
                                str += " and t.fwork in (select w.ID from tblWork w WITH(NOLOCK) where  w.Status=1 )";
                            }
                            else
                            {
                                str += " and (select w.fdesc from tblWork w WITH(NOLOCK) where w.ID=t.fwork )='" + objPropMapData.Worker.Replace("'", "''") + "'";
                            }
                        }
                        if (objPropMapData.LocID != 0)
                        {
                            str += " and t.LID=" + objPropMapData.LocID;
                        }
                        if (objPropMapData.CustID != 0)
                        {
                            str += " and t.Owner=" + objPropMapData.CustID;
                        }
                        if (objPropMapData.jobid != 0)
                        {
                            str += " and t.job =" + objPropMapData.jobid;
                        }
                        if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
                        {
                            str += " and (isnull(dp.charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge);
                            if (objPropMapData.FilterCharge == "1")
                                str += " or isnull(Invoice,0) <> 0)";
                            else
                                str += " and isnull(Invoice,0) = 0)";
                        }
                        if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                        {
                            str += "\n  and (select Super from tblWork w WITH(NOLOCK) where w.fdesc=t.dwork ) ='" + objPropMapData.Supervisor + "'";
                        }
                        if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
                        {
                            str += "\n  and (select Super from tblWork w WITH(NOLOCK) where w.fdesc=t.dwork ) <>'" + objPropMapData.NonSuper + "'";
                        }
                        
                        if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
                        {
                            str += "\n  and t.Workorder='" + objPropMapData.Workorder + "'";
                        }
                        if (objPropMapData.Department != -1)
                        {
                            str += "\n  and t.type=" + objPropMapData.Department;
                        }
                        if (objPropMapData.Category != string.Empty && objPropMapData.Category != null)
                        {
                            //str += " and t.cat='" + objPropMapData.Category + "'";
                            str += "\n  and t.cat in (" + objPropMapData.Category + ")";
                        }
                        #region ////Filter for TicketDPDA IF Create from Portal
                        if (objPropMapData.IsPortal != string.Empty && objPropMapData.IsPortal != null)
                        {
                            if (objPropMapData.IsPortal == "1")
                            {
                                str += "\n  and t.fBy='portal' ";
                            }
                            if (objPropMapData.IsPortal == "0")
                            {
                                str += "\n  and t.fBy <> 'portal' ";
                            }
                        }
                        #endregion




                        if (objPropMapData.Bremarks != null && objPropMapData.Bremarks != string.Empty)
                        {
                            if (objPropMapData.Bremarks == "1")
                                str += "\n  and isnull( rtrim(ltrim(t.bremarks)),'')<>''";
                            else
                                str += "\n  and isnull( rtrim(ltrim(t.bremarks)),'')=''";
                        }
                        if (objPropMapData.SearchBy != "" && objPropMapData.SearchBy != null && !string.IsNullOrWhiteSpace(objPropMapData.SearchValue))
                        {
                            string SearchBy = objPropMapData.SearchBy.Replace("t.descres", "dp.descres");
                            if (SearchBy == "t.ID")
                            {
                                //str += " and " + SearchBy + " = '" + objPropMapData.SearchValue + "'";
                                str += " and " + SearchBy + " in (" + UpdateListTicketIDsForSearch(objPropMapData.SearchValue) + ")";
                            }
                            else if (SearchBy == "t.Level")
                            {
                                str += "and t.Level in (" + objPropMapData.SearchValue + ")";
                            }
                            else
                            {
                                if (SearchBy == "l.tag")
                                    SearchBy = "r.name";
                                str += " and " + SearchBy + " like '%" + objPropMapData.SearchValue + "%'";
                            }
                        }
                        if (objPropMapData.InvoiceID != 0)
                        {
                            if (objPropMapData.InvoiceID == 1)
                            {
                                str += "\n  and isnull(Invoice,0) <> 0";
                            }
                            else if (objPropMapData.InvoiceID == 2)
                            {
                                str += "\n  and isnull(Invoice,0) = 0 and isnull(dp.charge,0)= 1";
                            }
                        }
                        //  If the user is a salesperson only show assigned  locations,Ticket.
                        if (IsSalesAsigned > 0)
                        {
                            str += "\n  AND  ( l.Terr=(" + "select id FROM  Terr WITH(NOLOCK) WHERE Name=(SELECT fUser FROM  tblUser WITH(NOLOCK) WHERE id=" + IsSalesAsigned + ")) )";
                        }

                        if (filters.Count > 0)
                        {
                            foreach (var items in filters)
                            {
                                if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                                {
                                    /// Int Filter
                                    int FilterValue = 0;
                                    //if (items.FilterColumn == "ID" & int.TryParse(items.FilterValue, out FilterValue))
                                    //{
                                    //    str += "\n  and t.ID  = " + FilterValue;
                                    //}

                                    if (items.FilterColumn == "ID")
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
                                        str += "\n and t.ID  in (" + filteredQuery.ToString() + ")";
                                    }

                                    if (items.FilterColumn == "Job" & int.TryParse(items.FilterValue, out FilterValue))
                                    {
                                        str += "\n  and t.job  = " + FilterValue;
                                    }

                                    /// string Filter

                                    if (items.FilterColumn == "WorkOrder")
                                    {
                                        str += "\n  and t.WorkOrder  = '" + items.FilterValue + "'";
                                    }

                                    if (items.FilterColumn == "locname")
                                    {
                                        str += "\n  and r.name  like '%" + items.FilterValue + "%'";
                                    }
                                    if (items.FilterColumn == "City")
                                    {
                                        str += "\n  and r.City  like '%" + items.FilterValue + "%'";
                                    }
                                    if (items.FilterColumn == "fullAddress")
                                    {
                                        str += "\n  and ( r.address + ', ' + r.city + ', ' + r.state + ', ' + r.zip )  like '%" + items.FilterValue + "%'";
                                    }

                                }
                            }
                        }

                        str += " )";
                        if (!string.IsNullOrEmpty(objPropMapData.Route))
                        {
                            str += " AND 1<>1";
                        }

                        #endregion FILTER FOR TICKET_DPDA
                    }

                    #endregion LocID == 0

                }

                #endregion FilterReview != "1"
            }

            #endregion Status != Assigned

            #region Status =  Completed/ALL Get Ticket From TicketD
            if (objPropMapData.Assigned == 4 || objPropMapData.Assigned == -1)
            {
                //if (objPropMapData.Mobile != 2)
                {

                    if (objPropMapData.Status != 1)
                    {
                        if (objPropMapData.FilterReview != "1")
                        {
                            str += "\n  \n  \n   UNION ALL";
                        }
                    }

                    str += "\n   SELECT CASE WHEN t.Recurring IS NULL THEN 0 ELSE 1 END isRecurring , " +
                        "\n   isnull(t.OT, 0.00) as OT, " +
                        "\n   isnull(t.Reg, 0.00) as Reg, " +
                        "\n   isnull(t.NT, 0.00) as NT, " +
                        "\n   isnull(t.DT, 0.00) as DT, " +
                        "\n   isnull(t.TT, 0.00) as TT, " +
                        "\n    (select( case  when ro.Name IS NULL  then 'Unassigned'   when tblwork.fdesc is null then  ro.Name    when tblwork.fdesc = ro.Name then ro.Name else ro.Name +' - ' + tblwork.fdesc   end)from tblwork where tblwork.id = ro.mech   ) as Name, " +
                        "\n  t.who," +
                        "\n  t.CPhone," +
                        "\n   t.loc as lid," +
                        "\n   l.id as locid," +
                        "\n   4 as assigned," +
                        "\n   (l.address+'," +
                        "\n   '+l.city+'," +
                        "\n   '+l.state+', " +
                        "\n '+l.zip) as fulladdress," +
                        "\n  l.city," +
                        "\n  l.state," +
                        "\n  l.zip," +
                        "\n  t.WorkOrder ," +

                        "\n isnull(t.Total,0.00) as Total, " +
                        "\n isnull( t.ClearCheck ,0) as ClearCheck ," +
                        "\n t.charge, " +
                        "\n t.fdesc," +
                        "\n timeroute, " +
                        "\n timesite,timecomp," +
                        "\n  1 as comp, " +
                        "\n (select UPPER(w.fdesc) from tblWork w WITH(NOLOCK) where t.fwork = w.id) AS dwork," +

                        "\n t.id,  r.Name  AS customername, " +
                        "\n r.EN, " +
                        "\n isnull(B.Name, '') As Company," +
                        "\n  l.tag AS locname, " +
                        "\n l.address," +
                        "\n l.Custom1 as LocCustom1," +
                        "\n l.Custom1 as LocCustom2," +
                        "\n r.phone, " +
                        "\n r.Email, " +
                        "\n  t.cat," +
                         "\n   t.Level, " +
                           // "\n  edate, " +
                           "\n  EDate AS edate,  CONVERT(VARCHAR(10), EDate, 101) as EdateWithDateOnly," +
                        "\n cdate, " +
                        "\n descres," +
                        "\n  case   t.Assigned when 6 then 'Voided' else 'Completed' end  AS assignname," +
                        "\n  est,t.Total as tottime , ";

                    #region  24 HOURS FEATURE
                    str += @" Round(CONVERT(NUMERIC(30, 2), ( Isnull(t.Total, 0.00) - ( CONVERT(FLOAT, Datediff(MILLISECOND, TimeRoute, ( CASE
                                                                                                                   WHEN(Cast(TimeSite AS TIME) < Cast(TimeRoute AS TIME)
                                                                                                                           AND Cast(TimeComp AS TIME) < Cast(TimeSite AS TIME)) THEN Dateadd(day, 2, TimeComp)
                                                                                                                   ELSE((CASE
                                                                                                                             WHEN(Cast(TimeSite AS TIME) < Cast(TimeRoute AS TIME)
                                                                                                                                    OR Cast(TimeComp AS TIME) < Cast(TimeSite AS TIME)) THEN Dateadd(day, 1, TimeComp)
                                                                                                                             ELSE TimeComp
                                                                                                                           END))
                                                                                                                 END ))) / 1000 / 60 / 60 ) )), 1)  AS timediff ,";
                    #endregion



                    str += "\n  (isnull(t.emile,0)-isnull(t.smile,0)) as mileage," +
                        " (select top 1 count(1) as signatureCount  from pdaticketsignature WITH(NOLOCK) where pdaticketid=t.ID ) as signatureCount, " +
                        " (select count(1) from documents WITH(NOLOCK) where screen='Ticket' and screenid=t.id) as DocumentCount," +
                        " t.fwork as workerid, (select top 1 * from dbo.split(descres,'|')) as description," +
                        "(select top 1 * from dbo.split(t.fdesc,'|')) as fdescreason, isnull(invoice,0) as invoice, 0 as Confirmed, " +
                        " manualinvoice, (CASE (i.Status)" +
                        " WHEN 0 THEN 'Open'" +
                        " WHEN 1 THEN 'Paid'" +
                        " WHEN 2 THEN 'Voided'" +
                        " WHEN 4 THEN 'Marked as Pending'" +
                        " WHEN 5 THEN 'Paid by Credit Card'" +
                        " WHEN 3 THEN 'Partially Paid'" +
                        " END + case (ipmt.Paid) WHEN 1 THEN '/Paid by MOM' else '' end )     AS statusname,  " +

                        " case  when ( Isnull(convert(varchar(20),invoice), '0') =  '0' ) then Manualinvoice else CONVERT(varchar(20), Invoice) end as invoiceno, 0 as ownerid, isnull(t.QBinvoiceID,'')as QBinvoiceid, isnull(TransferTime,0) as TransferTime, ";

                    str += "\n    isnull(l.dispalert,0)as dispalert," +

                        " isnull(l.credit,0)as credithold, 0 as high ," +

                        "e.id as unitid ,";

                    str += "\n  (select  STUFF((SELECT ', ' + CAST(e.Unit AS VARCHAR(1000)) ";

                    str += "\n FROM elev e WITH(NOLOCK) where  e.id in ";

                    str += "\n (select me.elev_id from multiple_equipments me WITH(NOLOCK) where me.ticket_id = t.ID)";

                    str += "\n  FOR XML PATH(''), Type)  .value('.', 'VARCHAR(1000)'),1,2,' ') ) as unit, ";


                    str += "\n    (jt.type) as department," +
                        " rtrim(ltrim(t.bremarks)) as bremarks, ";

                    str += "0 as EmailNotified, " +
                    "null as EmailTime,";
                    str += "  t.Job ";

                    if (inclCustomField)
                    {
                        str += "\n ,(SELECT cj.Value FROM tblCustomJob cj LEFT JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID WHERE cf.Label = 'Passed Inspection' AND cj.JobID = t.Job) AS PassedInspection ";
                    }
                    else
                    {
                        str += "\n ,'' AS PassedInspection";
                    }

                    str += "  , (  convert(varchar(20),t.Job )+'-'+ j.fdesc ) as ProjectDescription ";
                    str += ",j.SRemarks as SpecialRemark";
                    str += ",   t.Custom3, t.Custom4, t.Custom6, t.Custom7 ";
                    str += ", t.fBy , isnull(t.ClearPR,0) ClearPR ";
                    str += ", CONVERT(VARCHAR(10), i.fDate, 101) as InvoiceDate , len(isnull(PartsUsed,'')) IsPartsUsedIcon , isnull((select top 1 1 from POItem where (POItem.Ticket)=t.id) ,0) ispoicon ";
                    str += " \n FROM ticketd " +
                        "\n  t WITH(NOLOCK) INNER JOIN loc l WITH(NOLOCK)" +
                        "\n ON l.loc = t.loc  " +
                        "\n inner join Owner o WITH(NOLOCK)" +
                        "\n on l.Owner=o.ID " +
                        "\n INNER JOIN Rol r WITH(NOLOCK)" +
                        "\n ON r.ID = o.Rol " +
                        "\n LEFT OUTER JOIN " +
                        "\n Branch B WITH(NOLOCK) on B.ID = r.EN   " +
                        "\n left outer join Elev e WITH(NOLOCK) on e.ID=t.Elev  " +
                        "\n left join Route ro WITH(NOLOCK) on ro.ID=l.Route " +
                        "\n left join job j WITH(NOLOCK) on j.ID = t.Job " +
                        "\n left join jobtype jt WITH(NOLOCK) on jt.ID = t.type " +
                        "\n left join Invoice i WITH(NOLOCK) on t.Invoice=i.Ref  " +
                        "\n left join tblinvoicepayment ipmt WITH(NOLOCK) on cast(ipmt.ref as varchar(50))= t.ManualInvoice";

                    if (objPropMapData.EN == 1) // check for company
                    {
                        str += "\n LEFT OUTER JOIN tblUserCo UC WITH(NOLOCK) on UC.CompanyID = r.EN ";
                    }
                    str += "\n WHERE t.id is not null  ";
                    if (objPropMapData.IsAssignedProject == true) // check for company
                    {
                        str += "\n   and j.ProjectManagerUserID= " + objPropMapData.EmpID;
                    }
                    #region FILTER FOR TICKETD
                    if (!isFilterHasCommaDelimited && objPropMapData.InvoiceDate == null)
                    {
                        if (objPropMapData.StartDate != System.DateTime.MinValue)
                        {
                            str += "\n  and edate >='" + objPropMapData.StartDate.ToString("MM/dd/yyyy") + "'";
                        }
                        if (objPropMapData.EndDate != System.DateTime.MinValue)
                        {
                            str += "\n  and edate <  '" + objPropMapData.EndDate.AddDays(1).ToString("MM/dd/yyyy") + "'";
                        }
                    }


                    if (objPropMapData.EN == 1) // check for company
                    {
                        str += "\n  and UC.IsSel = 1 and UC.UserID =" + objPropMapData.UserID;
                    }
                    if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                    {

                        if (objPropMapData.Worker == "Active")
                        {
                            str += "\n  and t.fwork in (select w.ID from tblWork w WITH(NOLOCK) where  w.Status=0 )";
                        }
                        else if (objPropMapData.Worker == "Inactive")
                        {
                            str += "\n  and t.fwork in (select w.ID from tblWork w WITH(NOLOCK) where  w.Status=1 )";
                        }
                        else
                        {
                            str += "\n  and (select w.fdesc from tblWork w WITH(NOLOCK) where w.ID=t.fwork )='" + objPropMapData.Worker.Replace("'", "''") + "'";
                        }
                    }
                    if (objPropMapData.LocID != 0)
                    {
                        str += "\n  and l.loc=" + objPropMapData.LocID;
                    }
                    if (objPropMapData.CustID != 0)
                    {
                        str += "\n  and l.Owner=" + objPropMapData.CustID;
                    }
                    if (objPropMapData.jobid != 0)
                    {
                        str += "\n  and t.job =" + objPropMapData.jobid;
                    }
                    if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
                    {
                        str += "\n  and (isnull(t.charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge);
                        if (objPropMapData.FilterCharge == "1")
                            str += " or isnull(Invoice,0) <> 0)";
                        else
                            str += "\n  and isnull(Invoice,0) = 0)";
                    }
                    if (objPropMapData.Timesheet != null && objPropMapData.Timesheet != string.Empty)
                    {
                        str += "\n  and isnull(TransferTime,0)=" + Convert.ToInt32(objPropMapData.Timesheet);
                    }
                    if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                    {
                        str += "\n  and (select Super from tblWork w WITH(NOLOCK) where w.ID=t.fwork ) ='" + objPropMapData.Supervisor + "'";
                    }
                    if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
                    {
                        str += "\n  and (select Super from tblWork w WITH(NOLOCK) where w.ID=t.fwork ) <>'" + objPropMapData.NonSuper + "'";
                    }
                    if (objPropMapData.FilterReview != null && objPropMapData.FilterReview != string.Empty)
                    {
                        str += "\n  and isnull( ClearCheck ,0) =" + Convert.ToInt32(objPropMapData.FilterReview);
                    }
                    if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
                    {
                        str += "\n  and t.Workorder='" + objPropMapData.Workorder + "'";
                    }
                    if (objPropMapData.Department != -1)
                    {
                        str += "\n  and t.type=" + objPropMapData.Department;
                    }
                    if (objPropMapData.Category != string.Empty && objPropMapData.Category != null)
                    {

                        str += "\n  and t.cat in (" + objPropMapData.Category + ")";
                    }
                    #region ////Filter For TicketD IF Create from Portal
                    if (objPropMapData.IsPortal != string.Empty && objPropMapData.IsPortal != null)
                    {
                        if (objPropMapData.IsPortal == "1")
                        {
                            str += "\n  and t.fBy='portal' ";
                        }
                        if (objPropMapData.IsPortal == "0")
                        {
                            str += "\n  and t.fBy <> 'portal' ";
                        }
                    }
                    #endregion

                    #region default route check

                    if (!string.IsNullOrEmpty(objPropMapData.Route))
                    {
                        if (Convert.ToInt32(objPropMapData.Route) == 0)
                        {
                            str += "\n  and l.Route=0 ";
                        }
                        else
                        {
                            str += "\n  and ro.ID=" + Convert.ToInt32(objPropMapData.Route) + " ";
                        }
                    }

                    #endregion

                    if (objPropMapData.Bremarks != null && objPropMapData.Bremarks != string.Empty)
                    {
                        if (objPropMapData.Bremarks == "1")
                            str += "\n and isnull( rtrim(ltrim(t.bremarks)),'')<>''";
                        else
                            str += "\n and isnull( rtrim(ltrim(t.bremarks)),'')=''";
                    }
                    if (objPropMapData.SearchBy != "" && objPropMapData.SearchBy != null && !string.IsNullOrWhiteSpace(objPropMapData.SearchValue))
                    {
                        if (objPropMapData.SearchBy == "t.ldesc4")
                        {
                            str += "\n and l.address like '%" + objPropMapData.SearchValue + "%'";
                        }
                        else if (objPropMapData.SearchBy == "t.ID")
                        {

                            //str += "\n and " + objPropMapData.SearchBy + " = '" + objPropMapData.SearchValue + "'";
                            str += "\n and " + objPropMapData.SearchBy + " in (" + UpdateListTicketIDsForSearch(objPropMapData.SearchValue) + ")";
                        }
                        else if (objPropMapData.SearchBy == "t.Level")
                        {
                            str += "\n and t.Level in (" + objPropMapData.SearchValue + ")";
                        }
                        else
                        {
                            str += "\n and " + objPropMapData.SearchBy + " like '%" + objPropMapData.SearchValue + "%'";
                        }
                    }
                    if (objPropMapData.Status == 1)
                    {
                        str += "\n and isnull( t.status ,0) <> 1 and isnull(t.internet,0) = 1";
                    }

                    if (objPropMapData.RoleID != 0)
                        str += "\n and isnull(l.roleid,0)=" + objPropMapData.RoleID;

                    //}
                    if (objPropMapData.InvoiceID != 0)
                    {
                        if (objPropMapData.InvoiceID == 1)
                        {
                            str += "\n and (isnull(Invoice,0) <> 0  or manualinvoice <> '')";
                        }
                        else if (objPropMapData.InvoiceID == 2)
                        {
                            str += "\n and (isnull(Invoice,0) = 0 or manualinvoice = '') and isnull(t.charge,0)= 1";
                        }
                    }
                    //  If the user is a salesperson only show assigned  locations,Ticket.
                    if (IsSalesAsigned > 0)
                    {
                        str += "\n AND   (l.Terr=(" + "select id FROM  Terr WITH(NOLOCK) WHERE Name=(SELECT fUser FROM  tblUser WITH(NOLOCK) WHERE id=" + IsSalesAsigned + ")) or isnull(l.Terr2,0)=(" + "select id FROM  Terr WITH(NOLOCK) WHERE Name=(SELECT fUser  FROM  tblUser WITH(NOLOCK) WHERE id=" + IsSalesAsigned + ")))";
                    }

                    if (filters.Count > 0)
                    {
                        foreach (var items in filters)
                        {
                            if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                            {
                                /// Int Filter
                                int FilterValue = 0;
                                //if (items.FilterColumn == "ID" & int.TryParse(items.FilterValue, out FilterValue))
                                //{
                                //    str += "\n and t.ID  = " + FilterValue;
                                //}

                                if (items.FilterColumn == "ID")
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
                                    str += "\n and t.ID  in (" + filteredQuery.ToString() + ")";
                                }

                                if (items.FilterColumn == "Job" & int.TryParse(items.FilterValue, out FilterValue))
                                {
                                    str += "\n and t.job  = " + FilterValue;
                                }

                                /// string Filter

                                if (items.FilterColumn == "WorkOrder")
                                {
                                    str += "\n and t.WorkOrder  = '" + items.FilterValue + "'";
                                }
                                if (items.FilterColumn == "locname")
                                {
                                    str += "\n and l.Tag  like '%" + items.FilterValue + "%'";
                                }
                                if (items.FilterColumn == "City")
                                {
                                    str += "\n and l.City  like '%" + items.FilterValue + "%'";
                                }
                                if (items.FilterColumn == "cat")
                                {
                                    str += "\n and t.cat  like '%" + items.FilterValue + "%'";
                                }

                                if (items.FilterColumn == "fullAddress")
                                {
                                    str += "\n  and ( l.address + ', ' + l.city + ', ' + l.state + ', ' + l.zip )  like '%" + items.FilterValue + "%'";
                                }


                            }
                        }
                    }
                    #endregion FILTER FOR TICKETD
                }
            }
            #endregion Completed/ALL


            ///////////

            str += "\n  \n   ) xTicket ";


            str += "\n  \n  WHERE 1 = 1  ";

            if (objPropMapData.Mobile == 2)
            {
                str += "\n and xTicket.fBy  = 'MOBILEUSER' ";
            }

            if (objPropMapData.Mobile == 1) {

                str += "\n and xTicket.fBy  <> 'MOBILEUSER' ";
            }

                if (objPropMapData.InvoiceDate == "Yes")
            {
                str += "\n AND xTicket.InvoiceDate >=CAST('" + objPropMapData.StartDate.ToString("MM/dd/yyyy") + "' as date) ";
                str += "AND xTicket.InvoiceDate <= cast('" + objPropMapData.EndDate.ToString("MM/dd/yyyy") + "' as date)";
            }

            if (objPropMapData.IsEmailSend == 1)
            {
                str += "\n  and xTicket.ID in ( SELECT Ref     FROM [dbo].[tblEmailSendingLog] WHERE Screen='TicketList' and Status =1)";
            }

            else if (objPropMapData.IsEmailSend == 2)
            {
                str += "\n  and xTicket.ID not in ( SELECT Ref AS TicketID   FROM [dbo].[tblEmailSendingLog] WHERE Screen='TicketList' and Status =1 ) ";
            }

            if (objPropMapData.Voided == 1)
            {
                str += "\n and xTicket.assignname  = 'Voided' ";
            }

            if (objPropMapData.Assigned == 4 && objPropMapData.Voided != 1)
            {
                str += "\n and xTicket.assignname  <> 'Voided' ";
            }

            if (filters.Count > 0)
            {
                foreach (var items in filters)
                {
                    if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                    {
                        /// Int Filter
                        int FilterValue = 0;
                        if (items.FilterColumn == "Tottime" & int.TryParse(items.FilterValue, out FilterValue))
                        {
                            str += "\n and xTicket.Tottime  = " + FilterValue;
                        }
                        if (items.FilterColumn == "invoiceno" & int.TryParse(items.FilterValue, out FilterValue))
                        {
                            str += "\n and xTicket.invoiceno  = " + FilterValue;
                        }
                        if (items.FilterColumn == "timediff" & int.TryParse(items.FilterValue, out FilterValue))
                        {
                            str += "\n and xTicket.timediff  = " + FilterValue;
                        }
                        /// string Filter

                        if (items.FilterColumn == "assignname")
                        {
                            str += "\n and xTicket.assignname  like '%" + items.FilterValue + "%'";
                        }

                        if (items.FilterColumn == "locname")
                        {
                            str += "\n and xTicket.locname  like '%" + items.FilterValue + "%'";
                        }


                        if (items.FilterColumn == "assignname" && items.FilterValue == "Completed" && objPropMapData.Voided != 1)
                        {
                            str += "\n and xTicket.assignname  <> 'Voided' ";
                        }
                        if (items.FilterColumn == "fulladdress")
                        {
                            str += "\n and xTicket.fulladdress  like '%" + items.FilterValue + "%'";
                        }
                        if (items.FilterColumn == "dwork")
                        {
                            str += "\n and xTicket.dwork  like '%" + items.FilterValue + "%'";
                        }
                        if (items.FilterColumn == "Company")
                        {
                            str += "\n and xTicket.Company  like '%" + items.FilterValue + "%'";
                        }
                        if (items.FilterColumn == "department")
                        {
                            str += "\n and xTicket.department  like '%" + items.FilterValue + "%'";
                        }
                        if (items.FilterColumn == "Name")
                        {
                            str += "\n and xTicket.Name  like '%" + items.FilterValue + "%'";
                        }
                        if (items.FilterColumn == "unit")
                        {
                            str += "\n and (1 = (Select distinct 1 from multiple_equipments me WITH(NOLOCK)  inner join elev e WITH(NOLOCK) on e.id = me.elev_id where me.ticket_id = xTicket.ID and Unit like  '%" + items.FilterValue + "%'))";
                        }

                        if (items.FilterColumn == "manualinvoice")
                        {
                            str += "\n and isnull(xTicket.ManualInvoice,'')  like '%" + items.FilterValue + "%'";
                        }
                        if (items.FilterColumn == "InvoiceDate")
                        {
                            str += "\n AND xTicket.InvoiceDate >=CAST('" + items.FilterValue + "' as date) ";
                            str += "AND xTicket.InvoiceDate <= cast('" + items.FilterValue + "' as date)";
                        }

                    }
                }
            }


            if (objPropMapData.IsPayroll == 1)
            {
                str += "\n  and xTicket.ClearPR= 1";
            }
            else if (objPropMapData.IsPayroll == 0)
            {
                str += "\n  and xTicket.ClearPR = 0 ";
            }

            str += " )  ";

            str += "\n  \n  \n  select (select Top 1 ROWNO from FinalTicketData order by ROWNO desc) MAXROWNO,(select sum(isnull(Tottime,0)) from FinalTicketData) as SumOfTotalTime,* from FinalTicketData ";

            if (RadGvTicketListmaximumRows > 0)
            {
                str += "\n  WHERE ROWNO >" + RadGvTicketListminimumRows;

                str += "\n AND ROWNO <=" + RadGvTicketListmaximumRows;
            }

            if (str != string.Empty)
            {
                if (!string.IsNullOrEmpty(objPropMapData.OrderBy))
                {
                    string order = objPropMapData.OrderBy;
                    if (order == "Workorder  ASC") order = "t.Workorder  ASC";
                    else if (order == "Workorder  DESC") order = "t.Workorder  DESC";


                }
            }


            try
            {
                if (str != string.Empty)

                {
                    try
                    {
                        SqlParameter[] para = new SqlParameter[1];
                        para[0] = new SqlParameter
                        {
                            ParameterName = "@dscript",
                            SqlDbType = SqlDbType.NVarChar,
                            Value = str
                        };
                        return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.StoredProcedure, "spGetTicketListData", para);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }


                }
                   // return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.StoredProcedure, "spGetTicketListData", str);
                else
                    return objPropMapData.Ds = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// GetTicketListData
        /// </summary>
        /// <param name="objPropMapData"></param>
        /// <param name="filters"></param>
        /// <param name="IsSalesAsigned"></param>
        /// <param name="strORDERBY"></param>
        /// <param name="RadGvTicketListminimumRows"></param>
        /// <param name="RadGvTicketListmaximumRows"></param>
        /// <returns></returns>
        public string ExportTicketListDataToExcel(MapData objPropMapData, List<RetainFilter> filters, string fromDate, string toDate,
            Int32 IsSalesAsigned = 0, string strORDERBY = "EDATE ASC", string fileName = "", int rowsPerSheet = 500)
        {

            bool isFilterHasCommaDelimited = CheckFilterHasCommaDelimited(filters);
            if (!isFilterHasCommaDelimited && !string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                objPropMapData.StartDate = DateTime.Parse(fromDate);
                objPropMapData.EndDate = DateTime.Parse(toDate);
            }
            else
            {
                objPropMapData.StartDate = System.DateTime.MinValue;
                objPropMapData.EndDate = System.DateTime.MinValue;
            }
            //else
            //{
            //    objPropMapData.SearchBy = "";
            //    objPropMapData.SearchValue = "";
            //}

            if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && objPropMapData.SearchBy.Equals("t.ID", StringComparison.InvariantCultureIgnoreCase)
                && !string.IsNullOrEmpty(objPropMapData.SearchValue))
            {
                objPropMapData.StartDate = System.DateTime.MinValue;
                objPropMapData.EndDate = System.DateTime.MinValue;
            }

            string str = " \n   WITH FINALTICKETDATA  AS (   SELECT ROW_NUMBER() OVER(ORDER BY " + strORDERBY + " ) AS ROWNO,* FROM (";

            #region Status != Assigned  $$$ Get Ticket from Ticketo and TicketDPDA
            if (objPropMapData.Status != 1)
            {
                #region FilterReview != "1"
                if (objPropMapData.FilterReview != "1")
                {
                    #region Column
                    str += "\n    SELECT CASE WHEN t.Recurring IS NULL THEN 0 ELSE 1 END isRecurring , " +
                        "\n   isnull(dp.OT, 0.00) as OT, " +
                        "\n   isnull(dp.Reg, 0.00) as Reg, " +
                        "\n   isnull(dp.NT, 0.00) as NT, " +
                        "\n   isnull(dp.DT, 0.00) as DT, " +
                        "\n   isnull(dp.TT, 0.00) as TT, " +
                        "\n     (select( case  when ro.Name IS NULL  then 'Unassigned'   when tblwork.fdesc is null then  ro.Name    when tblwork.fdesc = ro.Name then ro.Name  else ro.Name +' - ' + tblwork.fdesc   end)from tblwork where tblwork.id = ro.mech   )  AS Name, " +
                        "\n   t.who," +
                        "\n  t.CPhone," +
                        "\n   t.lid, l.id as locid," +
                        "\n   assigned, ( l.address + ', ' + l.city + ', ' + l.state + ', ' + l.zip ) AS fulladdress," +
                        "\n  l.city," +
                        "\n   t.WorkOrder," +
                        "\n  dp.Total, 0 AS ClearCheck," +
                        "\n   case when t.charge =1 then 1  when dp.charge =1 then 1 else 0 end as charge, " +
                        "\n  t.fDesc," +
                        "\n   t.TimeRoute," +
                        "\n   t.TimeSite, " +
                        "\n  t.TimeComp," +
                        "\n   CASE WHEN EXISTS (SELECT 1 FROM TicketDPDA WITH(NOLOCK) WHERE ID = t.ID) THEN 2 ELSE 0 END AS comp," +
                        " UPPER(dwork) AS dwork, " +

                        "\n   t.ID, r.Name AS customername," +
                        "\n   r.EN," +
                        "\n   isnull(B.Name, '') As Company,";
                    str += "\n   l.Tag  AS locname," +
                        "\n   l.Address  AS address," +
                        "\n   l.Custom1 as LocCustom1," +
                        "\n   l.Custom1 as LocCustom2," +
                        "\n  r.phone," +
                        "\n   t.Cat, " +
                        "\n   t.Level, " +
                          ///"\n  t.EDate AS edate,  " + 
                          "\n  t.EDate AS edate,  CONVERT(VARCHAR(10), t.EDate, 101) as EdateWithDateOnly," +
                        "\n   t.CDate, " +
                        "\n  dp.descres," +
                        "\n   CASE " +
                        "WHEN assigned = 0 THEN 'Un-Assigned' " +
                        "WHEN assigned = 1 THEN 'Assigned' " +
                        "WHEN assigned = 2 THEN 'Enroute' " +
                        "WHEN assigned = 3 THEN 'Onsite' " +
                        "WHEN assigned = 4 THEN 'Completed' " +
                        "WHEN assigned = 5 THEN 'Hold' " +
                        "WHEN assigned = 6 THEN 'Voided' " +
                        "END AS assignname," +
                        "\n   t.Est, Isnull(dp.Total, 0.00) AS Tottime ,";

                    #region 24 HOURS FEATURE

                    str += @"  Round(CONVERT(NUMERIC(30, 2), ( Isnull(dp.Total, 0.00) - ( CONVERT(FLOAT, Datediff(MILLISECOND, dp.TimeRoute, ( CASE
                                                                                                                   WHEN(Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME)
                                                                                                                           AND Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME)) THEN Dateadd(day, 2, dp.TimeComp)
                                                                                                                   ELSE((CASE
                                                                                                                             WHEN(Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME)
                                                                                                                                    OR Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME)) THEN Dateadd(day, 1, dp.TimeComp)
                                                                                                                             ELSE dp.TimeComp
                                                                                                                           END))
                                                                                                                 END ))) / 1000 / 60 / 60 ) )), 1)  AS timediff, ";
                    #endregion

                    str += "\n   (isnull(dp.emile,0)-isnull(dp.smile,0)) as mileage " +
                        ",(select top 1 count(1) as signatureCount  from pdaticketsignature WITH(NOLOCK) where pdaticketid=t.ID ) as signatureCount, " +
                        "\n  (select count(1) from documents WITH(NOLOCK) where screen='Ticket' and screenid=t.id) as DocumentCount," +
                        "\n   t.fwork as workerid, " +
                        "\n  (select top 1 * from dbo.split(descres,'|')) as description," +
                        "\n   (select top 1 * from dbo.split(t.fdesc,'|')) as fdescreason," +
                        "\n   0 as invoice," +
                        "\n   isnull(Confirmed,0) as Confirmed," +
                        "\n   '' as manualinvoice,'' AS statusName, convert(varchar(20),dp.Invoice) as invoiceno," +
                        "\n   t.owner as ownerid, '' as QBinvoiceid," +
                        "\n   0 as TransferTime, ";

                    str += "\n    isnull(l.dispalert,0)as dispalert," +
                        "\n   isnull(l.credit,0)as credithold, " +
                        "\n  isnull(t.high,0) as high," +
                        "\n  e.id as unitid,";

                    str += "\n   (select  STUFF((SELECT ', ' + CAST(e.Unit AS VARCHAR(1000)) ";
                    str += "\n  FROM elev e WITH(NOLOCK) where  e.id in ";
                    str += "\n  (select me.elev_id from multiple_equipments me WITH(NOLOCK) where me.ticket_id = t.ID)";
                    str += "\n   FOR XML PATH(''), Type)  .value('.', 'VARCHAR(1000)'),1,2,' ') ) as unit, ";



                    str += "\n    ( jt.type) as department," +

                        "\n    rtrim(ltrim(t.bremarks)) as bremarks,";

                    str += "\n  isnull(EmailNotified,0) as EmailNotified, EmailTime,";

                    str += "\n      t.Job";
                    str += "\n    , ( convert(varchar(20),t.Job )+'-'+ j.fdesc) as ProjectDescription ";
                    str += "\n  , t.Custom3,  t.Custom4,  t.Custom6, t.Custom7 ";
                    str += "\n  , t.fBy , 0 ClearPR ";
                    str += "\n  , CONVERT(VARCHAR(10), i.fDate, 101) as InvoiceDate ";
                    #endregion

                    str += "\n   FROM ticketo t WITH(NOLOCK) " +

                        "\n  LEFT OUTER JOIN TicketDPDA dp WITH(NOLOCK) " +
                        "\n  ON t.ID = dp.ID inner join Loc l WITH(NOLOCK)" +
                        "\n   on l.Loc=t.lid  inner join Owner o WITH(NOLOCK)" +
                        "\n   on l.Owner=o.ID INNER JOIN Rol r WITH(NOLOCK)" +
                        "\n  ON r.ID = o.Rol " +
                        "\n  LEFT OUTER JOIN Branch B WITH(NOLOCK) " +
                        "\n  on B.ID = r.EN  " +
                        "\n  left outer join Elev e WITH(NOLOCK) " +
                        "\n  on e.ID=t.LElev " +
                        "\n  left join Route ro WITH(NOLOCK) on ro.ID=l.Route " +
                        "\n  left join job j WITH(NOLOCK) on j.ID = t.Job " +
                        "\n  left join jobtype jt WITH(NOLOCK) on jt.ID = t.type " +
                         "\n  left join Invoice i WITH(NOLOCK) on  dp.Invoices = i.Ref ";

                    //assigned NOT IN ( 0 )
                    if (objPropMapData.EN == 1) // check for company
                    {
                        str += "\n   LEFT OUTER JOIN tblUserCo UC WITH(NOLOCK) on UC.CompanyID = r.EN ";
                    }
                    str += "\n   WHERE t.id is not null and t.owner is not null  ";

                    if (objPropMapData.IsAssignedProject == true) // check for company
                    {
                        str += "\n   and j.ProjectManagerUserID= " + objPropMapData.EmpID;
                    }
                    #region FILTER FOR Ticket O

                    if (objPropMapData.IsList != 1)
                    {
                        str += " and assigned NOT IN ( 0 )";
                    }
                    if (objPropMapData.EN == 1) // check for company
                    {
                        str += " and UC.IsSel = 1 and UC.UserID =" + objPropMapData.UserID;
                    }
                    if (objPropMapData.Assigned != -1)//&& objPropMapData.Assigned != 0
                    {
                        if (objPropMapData.Assigned == -2)
                        {
                            str += " and t.Assigned <> 4";
                        }
                        else
                        {
                            str += " and t.Assigned=" + objPropMapData.Assigned;
                        }
                    }

                    if (!isFilterHasCommaDelimited)
                    {
                        if (objPropMapData.StartDate != System.DateTime.MinValue)
                        {
                            str += " and isnull(t.edate,t.cdate) >='" + objPropMapData.StartDate.ToString("MM/dd/yyyy") + "'";
                        }
                        if (objPropMapData.EndDate != System.DateTime.MinValue)
                        {
                            str += " and isnull(t.edate,t.cdate) <'" + objPropMapData.EndDate.AddDays(1).ToString("MM/dd/yyyy") + "'";
                        }


                    }


                    if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                    {

                        if (objPropMapData.Worker == "Active")
                        {
                            str += " and t.fwork in (select w.ID from tblWork w WITH(NOLOCK) where  w.Status=0 )";
                        }
                        else if (objPropMapData.Worker == "Inactive")
                        {
                            str += " and t.fwork in (select w.ID from tblWork w WITH(NOLOCK) where  w.Status=1 )";
                        }
                        else
                        {
                            str += " and (select w.fdesc from tblWork w WITH(NOLOCK) where w.ID=t.fwork )='" + objPropMapData.Worker.Replace("'", "''") + "'";
                        }
                    }
                    if (objPropMapData.LocID != 0)
                    {
                        str += " and t.LID=" + objPropMapData.LocID;
                    }
                    if (objPropMapData.CustID != 0)
                    {
                        str += " and t.Owner=" + objPropMapData.CustID;
                    }
                    if (objPropMapData.jobid != 0)
                    {
                        str += " and t.job =" + objPropMapData.jobid;
                    }
                    if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
                    {
                        str += " and (isnull(t.charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge);
                        if (objPropMapData.FilterCharge == "1")
                            str += " or isnull(Invoice,0) <> 0)";
                        else
                            str += " and isnull(Invoice,0) = 0)";
                    }
                    if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                    {
                        str += " and (select Super from tblWork w WITH(NOLOCK) where w.fdesc=t.dwork ) ='" + objPropMapData.Supervisor + "'";
                    }
                    if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
                    {
                        str += " and (select Super from tblWork w WITH(NOLOCK) where w.fdesc=t.dwork ) <>'" + objPropMapData.NonSuper + "'";
                    }

                    
                    if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
                    {
                        str += " and t.workorder='" + objPropMapData.Workorder + "'";
                    }
                    if (objPropMapData.Department != -1)
                    {
                        str += " and t.type=" + objPropMapData.Department;
                    }
                    if (objPropMapData.Category != string.Empty && objPropMapData.Category != null)
                    {

                        str += " and t.cat in (" + objPropMapData.Category + ")";
                    }
                    #region ////Filter for ticketo IF ticket Create from Portal
                    if (objPropMapData.IsPortal != string.Empty && objPropMapData.IsPortal != null)
                    {
                        if (objPropMapData.IsPortal == "1")
                        {
                            str += " and t.fBy='portal' ";
                        }
                        if (objPropMapData.IsPortal == "0")
                        {
                            str += " and t.fBy <> 'portal' ";
                        }
                    }
                    #endregion

                    #region default route check

                    if (!string.IsNullOrEmpty(objPropMapData.Route))
                    {
                        if (Convert.ToInt32(objPropMapData.Route) == 0)
                        {
                            str += " and l.Route=0 ";
                        }
                        else
                        {
                            str += " and ro.ID=" + Convert.ToInt32(objPropMapData.Route) + " ";
                        }
                    }

                    #endregion

                    if (objPropMapData.Bremarks != null && objPropMapData.Bremarks != string.Empty)
                    {
                        if (objPropMapData.Bremarks == "1")
                            str += " and isnull( rtrim(ltrim(t.bremarks)),'')<>''";
                        else
                            str += " and isnull( rtrim(ltrim(t.bremarks)),'')=''";
                    }

                    if (objPropMapData.SearchBy != "" && objPropMapData.SearchBy != null && !string.IsNullOrWhiteSpace(objPropMapData.SearchValue))
                    {
                        string SearchBy = objPropMapData.SearchBy.Replace("t.descres", "dp.descres");

                        if (SearchBy == "t.ID")
                        {
                            str += " and " + SearchBy + " in (" + UpdateListTicketIDsForSearch(objPropMapData.SearchValue) + ")";
                        }
                        else if (SearchBy == "t.Level")
                        {
                            str += "\n  and t.Level in (" + objPropMapData.SearchValue + ")";
                        }
                        else
                        {
                            str += " and " + SearchBy + " like '%" + objPropMapData.SearchValue + "%'";
                        }
                    }

                    if (objPropMapData.InvoiceID != 0)
                    {
                        if (objPropMapData.InvoiceID == 1)
                        {
                            str += " and isnull(Invoice,0) <> 0";
                        }
                        else if (objPropMapData.InvoiceID == 2)
                        {
                            str += " and isnull(Invoice,0) = 0 and isnull(dp.charge,0)= 1";
                        }
                    }
                    //  If the user is a salesperson or Second salesperson only show assigned  locations,Ticket.
                    if (IsSalesAsigned > 0)
                    {
                        str += " AND  (  l.Terr=(" + "select id FROM  Terr WITH(NOLOCK) WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSalesAsigned + ")) or  isnull(l.Terr2,0)=(" + "select id FROM  Terr WITH(NOLOCK) WHERE Name=(SELECT fUser FROM  tblUser WITH(NOLOCK) WHERE id=" + IsSalesAsigned + "))) ";
                    }

                    if (filters.Count > 0)
                    {
                        foreach (var items in filters)
                        {
                            if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                            {
                                /// Int Filter
                                int FilterValue = 0; string[] filterArrayValue;
                                StringBuilder filteredQuery = new StringBuilder();
                                if (items.FilterColumn == "ID")
                                {
                                    filterArrayValue = items.FilterValue.ToString().Split(',');
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
                                    str += " and t.ID  in (" + filteredQuery.ToString() + ")";
                                }

                                if (items.FilterColumn == "Job" & int.TryParse(items.FilterValue, out FilterValue))
                                {
                                    str += " and t.job  = " + FilterValue;
                                }

                                /// string Filter

                                if (items.FilterColumn == "WorkOrder")
                                {
                                    str += " and t.WorkOrder  = '" + items.FilterValue + "'";
                                }


                                if (items.FilterColumn == "locname")
                                {
                                    str += " and l.Tag  like '%" + items.FilterValue + "%'";
                                }
                                if (items.FilterColumn == "City")
                                {
                                    str += " and l.City  like '%" + items.FilterValue + "%'";
                                }
                                if (items.FilterColumn == "cat")
                                {
                                    str += " and t.cat  like '%" + items.FilterValue + "%'";
                                }


                                if (items.FilterColumn == "fullAddress")
                                {
                                    str += " and ( l.address + ', ' + l.city + ', ' + l.state + ', ' + l.zip )  like '%" + items.FilterValue + "%'";
                                }

                            }
                        }
                    }

                    #endregion End Filter For TicketO

                    #region LocID == 0

                    if (objPropMapData.LocID == 0)
                    {
                        str += "\n  \n  \n   Union all ";

                        str += "\n   SELECT  CASE WHEN t.Recurring IS NULL THEN 0 ELSE 1 END isRecurring , " +
                            "\n  isnull(dp.OT, 0.00), " +
                            "\n  isnull(dp.Reg, 0.00), " +
                            "\n  isnull(dp.NT, 0.00), " +
                            "\n  isnull(dp.DT, 0.00), " +
                            "\n  isnull(dp.TT, 0.00) as TT, " +
                            "\n  '', " +
                            "\n  t.who," +
                            "\n  t.CPhone," +
                            "\n   t.lid, '--' as locid," +
                            "\n   assigned, ( r.address + ', ' + r.city + ', ' + r.state + ', ' + r.zip ) AS fulladdress, " +
                            "\n  l.city," +
                            "\n  t.WorkOrder , " +
                            "\n  dp.Total, 0 AS ClearCheck, " +
                            "\n  case when t.charge =1 then 1  when dp.charge =1 then 1 else 0 end as charge, t.fDesc," +
                            "\n   t.TimeRoute, " +
                            "\n  t.TimeSite, " +
                            "\n  t.TimeComp," +
                            "\n   CASE WHEN EXISTS (SELECT 1 FROM TicketDPDA  WITH(NOLOCK) WHERE ID = t.ID) THEN 2 ELSE 0 END AS comp, " +
                            "\n  UPPER(dwork) AS dwork," +

                            "\n  t.ID," +
                            "\n   r.Name AS customername, " +
                            "\n   r.EN, isnull(B.Name, '') As Company,";
                        str += "\n   r.name  AS locname," +
                            "\n   l.Address  AS address," +
                            "\n   l.Custom1 as LocCustom1," +
                            "\n   l.Custom1 as LocCustom2," +
                            "\n   (select top 1 phone from rol lr where lr.id = l.rol) phone, " +
                            "\n  t.Cat, " +
                            "\n   t.Level, " +
                            // "\n  t.EDate AS edate," +
                            "\n  t.EDate AS edate,  CONVERT(VARCHAR(10), t.EDate, 101) as EdateWithDateOnly," +
                            "\n   t.CDate, dp.descres, " +
                            "\n  CASE " +
                            "WHEN assigned = 0 THEN 'Un-Assigned' " +
                            "WHEN assigned = 1 THEN 'Assigned' " +
                            "WHEN assigned = 2 THEN 'Enroute' " +
                            "WHEN assigned = 3 THEN 'Onsite' " +
                            "WHEN assigned = 4 THEN 'Completed' " +
                            "WHEN assigned = 5 THEN 'Hold' " +
                            "WHEN assigned = 6 THEN 'Voided' " +
                            "END AS assignname, t.Est, Isnull(dp.Total, 0.00) AS Tottime ,";

                        #region 24 Hours Feature 

                        str += @"  Round(CONVERT(NUMERIC(30, 2), ( Isnull(dp.Total, 0.00) - ( CONVERT(FLOAT, Datediff(MILLISECOND, dp.TimeRoute, ( CASE
                                                                                                                   WHEN ( Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME)
                                                                                                                           AND Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME) ) THEN Dateadd(day, 2, dp.TimeComp)
                                                                                                                   ELSE (( CASE
                                                                                                                             WHEN ( Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME)
                                                                                                                                    OR Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME) ) THEN Dateadd(day, 1, dp.TimeComp)
                                                                                                                             ELSE dp.TimeComp
                                                                                                                           END ))
                                                                                                                 END ))) / 1000 / 60 / 60 ) )), 1)  AS timediff,";

                        #endregion




                        str += "\n   (isnull(dp.emile,0)-isnull(dp.smile,0)) as mileage ," +
                            "\n  (select top 1 count(1) as signatureCount  from pdaticketsignature WITH(NOLOCK) where pdaticketid=t.ID ) as signatureCount," +
                            "\n   (select count(1) from documents WITH(NOLOCK) where screen='Ticket' and screenid=t.id) as DocumentCount, " +
                            "\n  t.fwork as workerid," +
                            "\n   (select top 1 * from dbo.split(descres,'|')) as description, " +
                            "\n  (select top 1 * from dbo.split(t.fdesc,'|')) as fdescreason, 0 as invoice, " +
                            "\n  isnull(Confirmed,0) as Confirmed," +
                            "\n   '' as manualinvoice,'' AS statusName, convert(varchar(20),dp.invoice) as invoiceno," +
                            "\n   t.owner as ownerid, " +
                            "\n  '' as QBinvoiceid," +
                            "\n   0 as TransferTime, ";

                        str += "\n    0 as dispalert, " +
                            "\n  0 as credithold, " +
                            "\n  0 as high,e.id as unitid,";

                        str += "\n   (select  STUFF((SELECT ', ' + CAST(e.Unit AS VARCHAR(1000)) ";
                        str += "\n  FROM elev e WITH(NOLOCK) where  e.id in ";
                        str += "\n  (select me.elev_id from multiple_equipments me WITH(NOLOCK) where me.ticket_id = t.ID)";
                        str += "\n   FOR XML PATH(''), Type)  .value('.', 'VARCHAR(1000)'),1,2,' ') ) as unit, ";

                        str += "\n    (jt.type) as department, " +
                            " rtrim(ltrim(t.bremarks)) as bremarks,";

                        str += "\n   isnull(EmailNotified,0) as EmailNotified, EmailTime,";

                        str += "\n      t.Job ";
                        str += "\n         , (  convert(varchar(20),t.Job )+'-'+ j.fdesc) as ProjectDescription ";
                        str += "\n  ,   t.Custom3,  t.Custom4,   t.Custom6, t.Custom7 ";
                        str += "\n  , t.fBy , 0 ClearPR ";
                        str += "\n  , CONVERT(VARCHAR(10), i.fDate, 101) as InvoiceDate ";
                        str += "\n   FROM ticketo t WITH(NOLOCK) " +
                            "\n  LEFT OUTER JOIN TicketDPDA dp WITH(NOLOCK) ON t.ID = dp.ID " +
                            "\n  inner join prospect l WITH(NOLOCK) on l.ID=t.lid " +
                            "\n  INNER JOIN Rol r WITH(NOLOCK) ON r.ID = l.Rol " +
                            "\n  LEFT OUTER JOIN Branch B WITH(NOLOCK) on B.ID = r.EN  " +
                            "\n  left outer join Elev e WITH(NOLOCK) on e.ID=t.LElev  " +
                        "\n  left join job j WITH(NOLOCK) on j.ID = t.Job " +
                        "\n  left join jobtype jt WITH(NOLOCK) on jt.ID = t.type " +
                        "\n  left join Invoice i WITH(NOLOCK) on dp.Invoice = i.Ref ";
                        if (objPropMapData.EN == 1) // check for company
                        {
                            str += "\n   LEFT OUTER JOIN tblUserCo UC WITH(NOLOCK) on UC.CompanyID = r.EN ";
                        }
                        str += "\n   WHERE (t.id is not null and t.owner is null and t.LType=1  ";

                        if (objPropMapData.IsAssignedProject == true) // check for company
                        {
                            str += "\n   and j.ProjectManagerUserID= " + objPropMapData.EmpID;
                        }

                        #region FILTER FOR TICKET_DPDA
                        if (objPropMapData.IsList != 1)
                        {
                            str += " and assigned NOT IN ( 0 )";
                        }
                        if (objPropMapData.EN == 1) // check for company
                        {
                            str += " and UC.IsSel = 1 and UC.UserID =" + objPropMapData.UserID;
                        }
                        if (objPropMapData.Assigned != -1)
                        {
                            if (objPropMapData.Assigned == -2)
                            {
                                str += " and t.Assigned <> 4";
                            }
                            else
                            {
                                str += " and t.Assigned=" + objPropMapData.Assigned;
                            }
                        }
                        if (!isFilterHasCommaDelimited)
                        {
                            if (objPropMapData.StartDate != System.DateTime.MinValue)
                            {
                                str += " and isnull(t.edate,t.cdate) >='" + objPropMapData.StartDate + "'";
                            }
                            if (objPropMapData.EndDate != System.DateTime.MinValue)
                            {
                                str += " and isnull(t.edate,t.cdate) <'" + objPropMapData.EndDate.AddDays(1) + "'";
                            }

                        }

                        if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                        {

                            if (objPropMapData.Worker == "Active")
                            {
                                str += " and t.fwork in (select w.ID from tblWork w WITH(NOLOCK) where  w.Status=0 )";
                            }
                            else if (objPropMapData.Worker == "Inactive")
                            {
                                str += " and t.fwork in (select w.ID from tblWork w WITH(NOLOCK) where  w.Status=1 )";
                            }
                            else
                            {
                                str += " and (select w.fdesc from tblWork w WITH(NOLOCK) where w.ID=t.fwork )='" + objPropMapData.Worker.Replace("'", "''") + "'";
                            }
                        }
                        if (objPropMapData.LocID != 0)
                        {
                            str += " and t.LID=" + objPropMapData.LocID;
                        }
                        if (objPropMapData.CustID != 0)
                        {
                            str += " and t.Owner=" + objPropMapData.CustID;
                        }
                        if (objPropMapData.jobid != 0)
                        {
                            str += " and t.job =" + objPropMapData.jobid;
                        }
                        if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
                        {
                            str += " and (isnull(dp.charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge);
                            if (objPropMapData.FilterCharge == "1")
                                str += " or isnull(Invoice,0) <> 0)";
                            else
                                str += " and isnull(Invoice,0) = 0)";
                        }
                        if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                        {
                            str += "\n  and (select Super from tblWork w WITH(NOLOCK) where w.fdesc=t.dwork ) ='" + objPropMapData.Supervisor + "'";
                        }
                        if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
                        {
                            str += "\n  and (select Super from tblWork w WITH(NOLOCK) where w.fdesc=t.dwork ) <>'" + objPropMapData.NonSuper + "'";
                        }
 
                        if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
                        {
                            str += "\n  and t.Workorder='" + objPropMapData.Workorder + "'";
                        }
                        if (objPropMapData.Department != -1)
                        {
                            str += "\n  and t.type=" + objPropMapData.Department;
                        }
                        if (objPropMapData.Category != string.Empty && objPropMapData.Category != null)
                        {
                            //str += " and t.cat='" + objPropMapData.Category + "'";
                            str += "\n  and t.cat in (" + objPropMapData.Category + ")";
                        }
                        #region ////Filter for TicketDPDA IF Create from Portal
                        if (objPropMapData.IsPortal != string.Empty && objPropMapData.IsPortal != null)
                        {
                            if (objPropMapData.IsPortal == "1")
                            {
                                str += "\n  and t.fBy='portal' ";
                            }
                            if (objPropMapData.IsPortal == "0")
                            {
                                str += "\n  and t.fBy <> 'portal' ";
                            }
                        }
                        #endregion




                        if (objPropMapData.Bremarks != null && objPropMapData.Bremarks != string.Empty)
                        {
                            if (objPropMapData.Bremarks == "1")
                                str += "\n  and isnull( rtrim(ltrim(t.bremarks)),'')<>''";
                            else
                                str += "\n  and isnull( rtrim(ltrim(t.bremarks)),'')=''";
                        }
                        if (objPropMapData.SearchBy != "" && objPropMapData.SearchBy != null && !string.IsNullOrWhiteSpace(objPropMapData.SearchValue))
                        {
                            string SearchBy = objPropMapData.SearchBy.Replace("t.descres", "dp.descres");
                            if (SearchBy == "t.ID")
                            {
                                //str += " and " + SearchBy + " = '" + objPropMapData.SearchValue + "'";
                                str += " and " + SearchBy + " in (" + UpdateListTicketIDsForSearch(objPropMapData.SearchValue) + ")";
                            }
                            else if (SearchBy == "t.Level")
                            {
                                str += "and t.Level in (" + objPropMapData.SearchValue + ")";
                            }
                            else
                            {
                                if (SearchBy == "l.tag")
                                    SearchBy = "r.name";
                                str += " and " + SearchBy + " like '%" + objPropMapData.SearchValue + "%'";
                            }
                        }
                        if (objPropMapData.InvoiceID != 0)
                        {
                            if (objPropMapData.InvoiceID == 1)
                            {
                                str += "\n  and isnull(Invoice,0) <> 0";
                            }
                            else if (objPropMapData.InvoiceID == 2)
                            {
                                str += "\n  and isnull(Invoice,0) = 0 and isnull(dp.charge,0)= 1";
                            }
                        }
                        //  If the user is a salesperson only show assigned  locations,Ticket.
                        if (IsSalesAsigned > 0)
                        {
                            str += "\n  AND  ( l.Terr=(" + "select id FROM  Terr WITH(NOLOCK) WHERE Name=(SELECT fUser FROM  tblUser WITH(NOLOCK) WHERE id=" + IsSalesAsigned + ")) )";
                        }

                        if (filters.Count > 0)
                        {
                            foreach (var items in filters)
                            {
                                if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                                {
                                    /// Int Filter
                                    int FilterValue = 0;
                                    //if (items.FilterColumn == "ID" & int.TryParse(items.FilterValue, out FilterValue))
                                    //{
                                    //    str += "\n  and t.ID  = " + FilterValue;
                                    //}

                                    if (items.FilterColumn == "ID")
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
                                        str += "\n and t.ID  in (" + filteredQuery.ToString() + ")";
                                    }

                                    if (items.FilterColumn == "Job" & int.TryParse(items.FilterValue, out FilterValue))
                                    {
                                        str += "\n  and t.job  = " + FilterValue;
                                    }

                                    /// string Filter

                                    if (items.FilterColumn == "WorkOrder")
                                    {
                                        str += "\n  and t.WorkOrder  = '" + items.FilterValue + "'";
                                    }

                                    if (items.FilterColumn == "locname")
                                    {
                                        str += "\n  and r.name  like '%" + items.FilterValue + "%'";
                                    }
                                    if (items.FilterColumn == "City")
                                    {
                                        str += "\n  and r.City  like '%" + items.FilterValue + "%'";
                                    }
                                    if (items.FilterColumn == "fullAddress")
                                    {
                                        str += "\n  and ( r.address + ', ' + r.city + ', ' + r.state + ', ' + r.zip )  like '%" + items.FilterValue + "%'";
                                    }

                                }
                            }
                        }

                        str += " )";
                        if (!string.IsNullOrEmpty(objPropMapData.Route))
                        {
                            str += " AND 1<>1";
                        }

                        #endregion FILTER FOR TICKET_DPDA
                    }

                    #endregion LocID == 0

                }

                #endregion FilterReview != "1"
            }

            #endregion Status != Assigned

            #region Status =  Completed/ALL Get Ticket From TicketD
            if (objPropMapData.Assigned == 4 || objPropMapData.Assigned == -1)
            {
                //if (objPropMapData.Mobile != 2)
                {

                    if (objPropMapData.Status != 1)
                    {
                        if (objPropMapData.FilterReview != "1")
                        {
                            str += "\n  \n  \n   UNION ALL";
                        }
                    }

                    str += "\n   SELECT CASE WHEN t.Recurring IS NULL THEN 0 ELSE 1 END isRecurring , " +
                        "\n   isnull(t.OT, 0.00) as OT, " +
                        "\n   isnull(t.Reg, 0.00) as Reg, " +
                        "\n   isnull(t.NT, 0.00) as NT, " +
                        "\n   isnull(t.DT, 0.00) as DT, " +
                        "\n   isnull(t.TT, 0.00) as TT, " +
                        "\n    (select( case  when ro.Name IS NULL  then 'Unassigned'   when tblwork.fdesc is null then  ro.Name    when tblwork.fdesc = ro.Name then ro.Name else ro.Name +' - ' + tblwork.fdesc   end)from tblwork where tblwork.id = ro.mech   ) as Name, " +
                        "\n  t.who," +
                        "\n  t.CPhone," +
                        "\n   t.loc as lid," +
                        "\n   l.id as locid," +
                        "\n   4 as assigned," +
                        "\n   (l.address+'," +
                        "\n   '+l.city+'," +
                        "\n   '+l.state+', " +
                        "\n '+l.zip) as fulladdress," +
                        "\n l.city," +
                        "\n  t.WorkOrder ," +

                        "\n isnull(t.Total,0.00) as Total, " +
                        "\n isnull( t.ClearCheck ,0) as ClearCheck ," +
                        "\n t.charge, " +
                        "\n t.fdesc," +
                        "\n timeroute, " +
                        "\n timesite,timecomp," +
                        "\n  1 as comp, " +
                        "\n (select UPPER(w.fdesc) from tblWork w WITH(NOLOCK) where t.fwork = w.id) AS dwork," +

                        "\n t.id,  r.Name  AS customername, " +
                        "\n r.EN, " +
                        "\n isnull(B.Name, '') As Company," +
                        "\n  l.tag AS locname, " +
                        "\n l.address," +
                        "\n l.Custom1 as LocCustom1," +
                        "\n l.Custom1 as LocCustom2," +
                        "\n r.phone, " +
                        "\n  t.cat," +
                         "\n   t.Level, " +
                           // "\n  edate, " +
                           "\n  EDate AS edate,  CONVERT(VARCHAR(10), EDate, 101) as EdateWithDateOnly," +
                        "\n cdate, " +
                        "\n descres," +
                        "\n  case   t.Assigned when 6 then 'Voided' else 'Completed' end  AS assignname," +
                        "\n  est,t.Total as tottime , ";

                    #region  24 HOURS FEATURE
                    str += @" Round(CONVERT(NUMERIC(30, 2), ( Isnull(t.Total, 0.00) - ( CONVERT(FLOAT, Datediff(MILLISECOND, TimeRoute, ( CASE
                                                                                                                   WHEN(Cast(TimeSite AS TIME) < Cast(TimeRoute AS TIME)
                                                                                                                           AND Cast(TimeComp AS TIME) < Cast(TimeSite AS TIME)) THEN Dateadd(day, 2, TimeComp)
                                                                                                                   ELSE((CASE
                                                                                                                             WHEN(Cast(TimeSite AS TIME) < Cast(TimeRoute AS TIME)
                                                                                                                                    OR Cast(TimeComp AS TIME) < Cast(TimeSite AS TIME)) THEN Dateadd(day, 1, TimeComp)
                                                                                                                             ELSE TimeComp
                                                                                                                           END))
                                                                                                                 END ))) / 1000 / 60 / 60 ) )), 1)  AS timediff ,";
                    #endregion



                    str += "\n  (isnull(t.emile,0)-isnull(t.smile,0)) as mileage," +
                        " (select top 1 count(1) as signatureCount  from pdaticketsignature WITH(NOLOCK) where pdaticketid=t.ID ) as signatureCount, " +
                        " (select count(1) from documents WITH(NOLOCK) where screen='Ticket' and screenid=t.id) as DocumentCount," +
                        " t.fwork as workerid, (select top 1 * from dbo.split(descres,'|')) as description," +
                        "(select top 1 * from dbo.split(t.fdesc,'|')) as fdescreason, isnull(invoice,0) as invoice, 0 as Confirmed, " +
                        " manualinvoice, (CASE (i.Status)" +
                        " WHEN 0 THEN 'Open'" +
                        " WHEN 1 THEN 'Paid'" +
                        " WHEN 2 THEN 'Voided'" +
                        " WHEN 4 THEN 'Marked as Pending'" +
                        " WHEN 5 THEN 'Paid by Credit Card'" +
                        " WHEN 3 THEN 'Partially Paid'" +
                        " END + case (ipmt.Paid) WHEN 1 THEN '/Paid by MOM' else '' end )     AS statusname,  " +

                        " case  when ( Isnull(convert(varchar(20),invoice), '0') =  '0' ) then Manualinvoice else CONVERT(varchar(20), Invoice) end as invoiceno, 0 as ownerid, isnull(t.QBinvoiceID,'')as QBinvoiceid, isnull(TransferTime,0) as TransferTime, ";

                    str += "\n    isnull(l.dispalert,0)as dispalert," +

                        " isnull(l.credit,0)as credithold, 0 as high ," +

                        "e.id as unitid ,";

                    str += "\n  (select  STUFF((SELECT ', ' + CAST(e.Unit AS VARCHAR(1000)) ";

                    str += "\n FROM elev e WITH(NOLOCK) where  e.id in ";

                    str += "\n (select me.elev_id from multiple_equipments me WITH(NOLOCK) where me.ticket_id = t.ID)";

                    str += "\n  FOR XML PATH(''), Type)  .value('.', 'VARCHAR(1000)'),1,2,' ') ) as unit, ";


                    str += "\n    (jt.type) as department," +
                        " rtrim(ltrim(t.bremarks)) as bremarks, ";

                    str += "0 as EmailNotified, " +
                    "null as EmailTime,";
                    str += "  t.Job ";
                    str += "  , (  convert(varchar(20),t.Job )+'-'+ j.fdesc ) as ProjectDescription ";
                    str += ",   t.Custom3, t.Custom4, t.Custom6, t.Custom7 ";
                    str += ", t.fBy , isnull(t.ClearPR,0) ClearPR ";
                    str += ", CONVERT(VARCHAR(10), i.fDate, 101) as InvoiceDate ";
                    str += " \n FROM ticketd " +
                        "\n  t WITH(NOLOCK) INNER JOIN loc l WITH(NOLOCK)" +
                        "\n ON l.loc = t.loc  " +
                        "\n inner join Owner o WITH(NOLOCK)" +
                        "\n on l.Owner=o.ID " +
                        "\n INNER JOIN Rol r WITH(NOLOCK)" +
                        "\n ON r.ID = o.Rol " +
                        "\n LEFT OUTER JOIN " +
                        "\n Branch B WITH(NOLOCK) on B.ID = r.EN   " +
                        "\n left outer join Elev e WITH(NOLOCK) on e.ID=t.Elev  " +
                        "\n left join Route ro WITH(NOLOCK) on ro.ID=l.Route " +
                        "\n left join job j WITH(NOLOCK) on j.ID = t.Job " +
                        "\n left join jobtype jt WITH(NOLOCK) on jt.ID = t.type " +
                        "\n left join Invoice i WITH(NOLOCK) on t.Invoice = i.Ref  " +
                        "\n left join tblinvoicepayment ipmt WITH(NOLOCK) on cast(ipmt.ref as varchar(50))= t.ManualInvoice";

                    if (objPropMapData.EN == 1) // check for company
                    {
                        str += "\n LEFT OUTER JOIN tblUserCo UC WITH(NOLOCK) on UC.CompanyID = r.EN ";
                    }
                    str += "\n WHERE t.id is not null  ";
                    if (objPropMapData.IsAssignedProject == true) // check for company
                    {
                        str += "\n   and j.ProjectManagerUserID= " + objPropMapData.EmpID;
                    }
                    #region FILTER FOR TICKETD
                    if (!isFilterHasCommaDelimited)
                    {
                        if (objPropMapData.StartDate != System.DateTime.MinValue)
                        {
                            str += "\n  and edate >='" + objPropMapData.StartDate.ToString("MM/dd/yyyy") + "'";
                        }
                        if (objPropMapData.EndDate != System.DateTime.MinValue)
                        {
                            str += "\n  and edate <  '" + objPropMapData.EndDate.AddDays(1).ToString("MM/dd/yyyy") + "'";
                        }
                    }


                    if (objPropMapData.EN == 1) // check for company
                    {
                        str += "\n  and UC.IsSel = 1 and UC.UserID =" + objPropMapData.UserID;
                    }
                    if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                    {

                        if (objPropMapData.Worker == "Active")
                        {
                            str += "\n  and t.fwork in (select w.ID from tblWork w WITH(NOLOCK) where  w.Status=0 )";
                        }
                        else if (objPropMapData.Worker == "Inactive")
                        {
                            str += "\n  and t.fwork in (select w.ID from tblWork w WITH(NOLOCK) where  w.Status=1 )";
                        }
                        else
                        {
                            str += "\n  and (select w.fdesc from tblWork w WITH(NOLOCK) where w.ID=t.fwork )='" + objPropMapData.Worker.Replace("'", "''") + "'";
                        }
                    }
                    if (objPropMapData.LocID != 0)
                    {
                        str += "\n  and l.loc=" + objPropMapData.LocID;
                    }
                    if (objPropMapData.CustID != 0)
                    {
                        str += "\n  and l.Owner=" + objPropMapData.CustID;
                    }
                    if (objPropMapData.jobid != 0)
                    {
                        str += "\n  and t.job =" + objPropMapData.jobid;
                    }
                    if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
                    {
                        str += "\n  and (isnull(t.charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge);
                        if (objPropMapData.FilterCharge == "1")
                            str += " or isnull(Invoice,0) <> 0)";
                        else
                            str += "\n  and isnull(Invoice,0) = 0)";
                    }
                    if (objPropMapData.Timesheet != null && objPropMapData.Timesheet != string.Empty)
                    {
                        str += "\n  and isnull(TransferTime,0)=" + Convert.ToInt32(objPropMapData.Timesheet);
                    }
                    if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                    {
                        str += "\n  and (select Super from tblWork w WITH(NOLOCK) where w.ID=t.fwork ) ='" + objPropMapData.Supervisor + "'";
                    }
                    if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
                    {
                        str += "\n  and (select Super from tblWork w WITH(NOLOCK) where w.ID=t.fwork ) <>'" + objPropMapData.NonSuper + "'";
                    }
                    if (objPropMapData.FilterReview != null && objPropMapData.FilterReview != string.Empty)
                    {
                        str += "\n  and isnull( ClearCheck ,0) =" + Convert.ToInt32(objPropMapData.FilterReview);
                    }
                    if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
                    {
                        str += "\n  and t.Workorder='" + objPropMapData.Workorder + "'";
                    }
                    if (objPropMapData.Department != -1)
                    {
                        str += "\n  and t.type=" + objPropMapData.Department;
                    }
                    if (objPropMapData.Category != string.Empty && objPropMapData.Category != null)
                    {

                        str += "\n  and t.cat in (" + objPropMapData.Category + ")";
                    }
                    #region ////Filter For TicketD IF Create from Portal
                    if (objPropMapData.IsPortal != string.Empty && objPropMapData.IsPortal != null)
                    {
                        if (objPropMapData.IsPortal == "1")
                        {
                            str += "\n  and t.fBy='portal' ";
                        }
                        if (objPropMapData.IsPortal == "0")
                        {
                            str += "\n  and t.fBy <> 'portal' ";
                        }
                    }
                    #endregion

                    #region default route check

                    if (!string.IsNullOrEmpty(objPropMapData.Route))
                    {
                        if (Convert.ToInt32(objPropMapData.Route) == 0)
                        {
                            str += "\n  and l.Route=0 ";
                        }
                        else
                        {
                            str += "\n  and ro.ID=" + Convert.ToInt32(objPropMapData.Route) + " ";
                        }
                    }

                    #endregion

                    if (objPropMapData.Bremarks != null && objPropMapData.Bremarks != string.Empty)
                    {
                        if (objPropMapData.Bremarks == "1")
                            str += "\n and isnull( rtrim(ltrim(t.bremarks)),'')<>''";
                        else
                            str += "\n and isnull( rtrim(ltrim(t.bremarks)),'')=''";
                    }
                    if (objPropMapData.SearchBy != "" && objPropMapData.SearchBy != null && !string.IsNullOrWhiteSpace(objPropMapData.SearchValue))
                    {
                        if (objPropMapData.SearchBy == "t.ldesc4")
                        {
                            str += "\n and l.address like '%" + objPropMapData.SearchValue + "%'";
                        }
                        else if (objPropMapData.SearchBy == "t.ID")
                        {

                            //str += "\n and " + objPropMapData.SearchBy + " = '" + objPropMapData.SearchValue + "'";
                            str += "\n and " + objPropMapData.SearchBy + " in (" + UpdateListTicketIDsForSearch(objPropMapData.SearchValue) + ")";
                        }
                        else if (objPropMapData.SearchBy == "t.Level")
                        {
                            str += "\n and t.Level in (" + objPropMapData.SearchValue + ")";
                        }
                        else
                        {
                            str += "\n and " + objPropMapData.SearchBy + " like '%" + objPropMapData.SearchValue + "%'";
                        }
                    }
                    if (objPropMapData.Status == 1)
                    {
                        str += "\n and isnull( t.status ,0) <> 1 and isnull(t.internet,0) = 1";
                    }

                    if (objPropMapData.RoleID != 0)
                        str += "\n and isnull(l.roleid,0)=" + objPropMapData.RoleID;

                    //}
                    if (objPropMapData.InvoiceID != 0)
                    {
                        if (objPropMapData.InvoiceID == 1)
                        {
                            str += "\n and (isnull(Invoice,0) <> 0  or manualinvoice <> '')";
                        }
                        else if (objPropMapData.InvoiceID == 2)
                        {
                            str += "\n and (isnull(Invoice,0) = 0 or manualinvoice = '') and isnull(t.charge,0)= 1";
                        }
                    }
                    //  If the user is a salesperson only show assigned  locations,Ticket.
                    if (IsSalesAsigned > 0)
                    {
                        str += "\n AND   (l.Terr=(" + "select id FROM  Terr WITH(NOLOCK) WHERE Name=(SELECT fUser FROM  tblUser WITH(NOLOCK) WHERE id=" + IsSalesAsigned + ")) or isnull(l.Terr2,0)=(" + "select id FROM  Terr WITH(NOLOCK) WHERE Name=(SELECT fUser  FROM  tblUser WITH(NOLOCK) WHERE id=" + IsSalesAsigned + ")))";
                    }

                    if (filters.Count > 0)
                    {
                        foreach (var items in filters)
                        {
                            if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                            {
                                /// Int Filter
                                int FilterValue = 0;
                                //if (items.FilterColumn == "ID" & int.TryParse(items.FilterValue, out FilterValue))
                                //{
                                //    str += "\n and t.ID  = " + FilterValue;
                                //}

                                if (items.FilterColumn == "ID")
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
                                    str += "\n and t.ID  in (" + filteredQuery.ToString() + ")";
                                }

                                if (items.FilterColumn == "Job" & int.TryParse(items.FilterValue, out FilterValue))
                                {
                                    str += "\n and t.job  = " + FilterValue;
                                }

                                /// string Filter

                                if (items.FilterColumn == "WorkOrder")
                                {
                                    str += "\n and t.WorkOrder  = '" + items.FilterValue + "'";
                                }
                                if (items.FilterColumn == "locname")
                                {
                                    str += "\n and l.Tag  like '%" + items.FilterValue + "%'";
                                }
                                if (items.FilterColumn == "City")
                                {
                                    str += "\n and l.City  like '%" + items.FilterValue + "%'";
                                }
                                if (items.FilterColumn == "cat")
                                {
                                    str += "\n and t.cat  like '%" + items.FilterValue + "%'";
                                }

                                if (items.FilterColumn == "fullAddress")
                                {
                                    str += "\n  and ( l.address + ', ' + l.city + ', ' + l.state + ', ' + l.zip )  like '%" + items.FilterValue + "%'";
                                }
                            }
                        }
                    }
                    #endregion FILTER FOR TICKETD
                }
            }
            #endregion Completed/ALL


            ///////////

            str += "\n  \n   ) xTicket WHERE 1=1";

            if (objPropMapData.Mobile == 2)
            {
                str += "\n and xTicket.fBy  = 'MOBILEUSER' ";
            }

            if (objPropMapData.Mobile == 1)
            {

                str += "\n and xTicket.fBy  <> 'MOBILEUSER' ";
            }

            if (objPropMapData.Voided == 1)
            {
                str += "\n and xTicket.assignname  = 'Voided' ";
            }

            if (objPropMapData.Assigned == 4 && objPropMapData.Voided != 1)
            {
                str += "\n and xTicket.assignname  <> 'Voided' ";
            }

            if (filters.Count > 0)
            {
                foreach (var items in filters)
                {
                    if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                    {
                        /// Int Filter
                        int FilterValue = 0;
                        if (items.FilterColumn == "Tottime" & int.TryParse(items.FilterValue, out FilterValue))
                        {
                            str += "\n and xTicket.Tottime  = " + FilterValue;
                        }
                        if (items.FilterColumn == "invoiceno" & int.TryParse(items.FilterValue, out FilterValue))
                        {
                            str += "\n and xTicket.invoiceno  = " + FilterValue;
                        }
                        if (items.FilterColumn == "timediff" & int.TryParse(items.FilterValue, out FilterValue))
                        {
                            str += "\n and xTicket.timediff  = " + FilterValue;
                        }
                        /// string Filter

                        if (items.FilterColumn == "assignname")
                        {
                            str += "\n and xTicket.assignname  like '%" + items.FilterValue + "%'";
                        }

                        if (items.FilterColumn == "locname")
                        {
                            str += "\n and xTicket.locname  like '%" + items.FilterValue + "%'";
                        }


                        if (items.FilterColumn == "assignname" && items.FilterValue == "Completed" && objPropMapData.Voided != 1)
                        {
                            str += "\n and xTicket.assignname  <> 'Voided' ";
                        }
                        if (items.FilterColumn == "fulladdress")
                        {
                            str += "\n and xTicket.fulladdress  like '%" + items.FilterValue + "%'";
                        }
                        if (items.FilterColumn == "dwork")
                        {
                            str += "\n and xTicket.dwork  like '%" + items.FilterValue + "%'";
                        }
                        if (items.FilterColumn == "Company")
                        {
                            str += "\n and xTicket.Company  like '%" + items.FilterValue + "%'";
                        }
                        if (items.FilterColumn == "department")
                        {
                            str += "\n and xTicket.department  like '%" + items.FilterValue + "%'";
                        }
                        if (items.FilterColumn == "Name")
                        {
                            str += "\n and xTicket.Name  like '%" + items.FilterValue + "%'";
                        }
                        if (items.FilterColumn == "unit")
                        {
                            str += "\n and (1 = (Select distinct 1 from multiple_equipments me WITH(NOLOCK)  inner join elev e WITH(NOLOCK) on e.id = me.elev_id where me.ticket_id = xTicket.ID and Unit like  '%" + items.FilterValue + "%'))";
                        }

                    }
                }
            }


            if (objPropMapData.IsPayroll == 1)
            {
                str += "\n  and xTicket.ClearPR= 1";
            }
            else if (objPropMapData.IsPayroll == 0)
            {
                str += "\n  and xTicket.ClearPR = 0 ";
            }
            //if (objPropMapData.InvoiceDate != null)
            //{
            //    str += "\n  and xTicket.InvoiceDate =" + objPropMapData.InvoiceDate;
            //}

            str += " )  ";

            str += "\n  \n  \n  select (select Top 1 ROWNO from FinalTicketData order by ROWNO desc) MAXROWNO,(select sum(isnull(Tottime,0)) from FinalTicketData) as SumOfTotalTime,* from FinalTicketData ";

            if (str != string.Empty)
            {
                if (!string.IsNullOrEmpty(objPropMapData.OrderBy))
                {
                    string order = objPropMapData.OrderBy;
                    if (order == "Workorder  ASC") order = "t.Workorder  ASC";
                    else if (order == "Workorder  DESC") order = "t.Workorder  DESC";


                }
            }

            try
            {
                if (str != string.Empty)
                {
                    DL_Utility dL_Utility = new DL_Utility();
                    return dL_Utility.ExportToExcel(objPropMapData.ConnConfig, str, fileName, rowsPerSheet);
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string ExportTicketListDataToExcelNew(MapData objPropMapData, List<RetainFilter> filters, string fromDate, string toDate,
            Int32 IsSalesAsigned = 0, string strORDERBY = "EDATE ASC", string fileName = "", int rowsPerSheet = 500, bool inclCustomField = false)
        {
            bool isFilterHasCommaDelimited = CheckFilterHasCommaDelimited(filters);
            if (!isFilterHasCommaDelimited && !string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                objPropMapData.StartDate = DateTime.Parse(fromDate);
                objPropMapData.EndDate = DateTime.Parse(toDate);
            }
            else
            {
                objPropMapData.StartDate = System.DateTime.MinValue;
                objPropMapData.EndDate = System.DateTime.MinValue;
            }

            if (!string.IsNullOrEmpty(objPropMapData.SearchBy) && objPropMapData.SearchBy.Equals("t.ID", StringComparison.InvariantCultureIgnoreCase)
                && !string.IsNullOrEmpty(objPropMapData.SearchValue))
            {
                objPropMapData.StartDate = System.DateTime.MinValue;
                objPropMapData.EndDate = System.DateTime.MinValue;
            }

            StringBuilder strQuery = new StringBuilder();
            StringBuilder strQueryO = new StringBuilder();
            StringBuilder strQueryPDA = new StringBuilder();
            StringBuilder strQueryD = new StringBuilder();

            #region Status != Assigned  $$$ Get Ticket from Ticketo and TicketDPDA
            if (objPropMapData.Status != 1)
            {
                #region FilterReview != "1"
                if (objPropMapData.FilterReview != "1")
                {
                    strQueryO.Append("SELECT");
                    strQueryO.Append("     t.ID [Ticket#], ");
                    strQueryO.Append("     t.WorkOrder [WO#],");
                    strQueryO.Append("     t.EDate AS [Date],");
                    strQueryO.Append("     r.Name  AS [Customer Name],");
                    strQueryO.Append("     l.Tag  AS Location,");
                    strQueryO.Append("     (select  STUFF((SELECT ', ' + CAST(e.Unit AS VARCHAR(1000))");
                    strQueryO.Append("         FROM elev e WITH(NOLOCK) where  e.id in (select me.elev_id from multiple_equipments me WITH(NOLOCK) where me.ticket_id = t.ID)");
                    strQueryO.Append("         FOR XML PATH(''), Type)  .value('.', 'VARCHAR(1000)'), 1, 2, ' ') ");
                    strQueryO.Append("     ) as Equipment,");
                    strQueryO.Append("     Case WHEN ISNULL(t.Job, 0) = 0 THEN '' ELSE Convert(varchar(50),t.Job) END [Project#],");
                    strQueryO.Append("     convert(varchar(20), dp.Invoice) as [Invoice#],");
                    strQueryO.Append("     '' as [ManualInv.#],");
                    strQueryO.Append("     (convert(varchar(20), t.Job) + '-' + j.fdesc) as ProjectDesc,");
                    strQueryO.Append("      l.city, l.state, l.zip,");
                    strQueryO.Append("      lr.Phone, lr.Email,");
                    strQueryO.Append("      (l.address + ', ' + l.city + ', ' + l.state + ', ' + l.zip) AS Fulladdress,");
                    strQueryO.Append("      UPPER(dwork) AS Assignedto,");
                    strQueryO.Append("      (select( case  when ro.Name IS NULL  then 'Unassigned'   when tblwork.fdesc is null then  ro.Name    when tblwork.fdesc = ro.Name then ro.Name  else ro.Name + ' - ' + tblwork.fdesc   end)from tblwork where tblwork.id = ro.mech   )  AS Name,");
                    strQueryO.Append("       t.Cat Category,");
                    strQueryO.Append("       CASE WHEN assigned = 0 THEN 'Un-Assigned' WHEN assigned = 1 THEN 'Assigned' WHEN assigned = 2 THEN 'Enroute' WHEN assigned = 3 THEN 'Onsite' WHEN assigned = 4 THEN 'Completed' WHEN assigned = 5 THEN 'Hold' WHEN assigned = 6 THEN 'Voided' END AS[Status],");
                    strQueryO.Append("       isnull(B.Name, '') As Company,");
                    strQueryO.Append("       Isnull(dp.Total, 0.00) AS TotalTime,");
                    strQueryO.Append("       Round(CONVERT(NUMERIC(30, 2), (Isnull(dp.Total, 0.00) - (CONVERT(FLOAT, Datediff(MILLISECOND, dp.TimeRoute,");
                    strQueryO.Append("               (CASE");
                    strQueryO.Append("               WHEN(Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME)");
                    strQueryO.Append("                       AND Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME)) THEN Dateadd(day, 2, dp.TimeComp)");
                    strQueryO.Append("               ELSE((CASE");
                    strQueryO.Append("                       WHEN(Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME)");
                    strQueryO.Append("                           OR Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME)) THEN Dateadd(day, 1, dp.TimeComp)");
                    strQueryO.Append("                       ELSE dp.TimeComp");
                    strQueryO.Append("                   END))");
                    strQueryO.Append("               END))) / 1000 / 60 / 60) )), 1)  AS TimeDiff,");
                    strQueryO.Append("     ( jt.type) as department,");
                    strQueryO.Append("     t.fDesc[Reason for Service],");
                    strQueryO.Append("    dp.descres[Work Complete Desc],");
                    strQueryO.Append("    t.who Caller,");
                    strQueryO.Append("    t.CPhone[Caller Phone],");
                    strQueryO.Append("    t.CDate[Date Called In],");
                    strQueryO.Append("    t.Custom3,");
                    strQueryO.Append("    t.Custom4,");
                    strQueryO.Append("    0 ClearPR,");
                    strQueryO.Append("    CONVERT(VARCHAR(10), i.fDate, 101) as InvoiceDate");

                    if (inclCustomField)
                    {
                        strQueryO.Append("  ,(SELECT cj.Value FROM tblCustomJob cj LEFT JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID WHERE cf.Label = 'Passed Inspection' AND cj.JobID = t.Job) AS [Last Inspection Date] ");
                    }

                    strQueryO.Append(" FROM ticketo t WITH(NOLOCK)");
                    strQueryO.Append(" LEFT OUTER JOIN TicketDPDA dp WITH(NOLOCK) ON t.ID = dp.ID");
                    strQueryO.Append(" INNER JOIN Loc l WITH(NOLOCK) on l.Loc = t.lid");
                    strQueryO.Append(" INNER JOIN Rol lr WITH(NOLOCK) on lr.ID = l.Rol");
                    strQueryO.Append(" INNER JOIN Owner o WITH(NOLOCK) on l.Owner = o.ID");
                    strQueryO.Append(" INNER JOIN Rol r WITH(NOLOCK) ON r.ID = o.Rol");
                    strQueryO.Append(" LEFT OUTER JOIN Branch B WITH(NOLOCK) on B.ID = r.EN");
                    strQueryO.Append(" LEFT OUTER JOIN Elev e WITH(NOLOCK) on e.ID = t.LElev");
                    strQueryO.Append(" LEFT JOIN Route ro WITH(NOLOCK) on ro.ID = l.Route");
                    strQueryO.Append(" LEFT JOIN job j WITH(NOLOCK) on j.ID = t.Job");
                    strQueryO.Append(" LEFT JOIN jobtype jt WITH(NOLOCK) on jt.ID = t.type");
                    strQueryO.Append(" LEFT JOIN Invoice i WITH(NOLOCK) on  dp.Invoice = i.Ref");

                    if (objPropMapData.EN == 1) // check for company
                    {
                        strQueryO.Append(" LEFT OUTER JOIN tblUserCo UC WITH(NOLOCK) on UC.CompanyID = r.EN ");
                    }
                    strQueryO.Append("   WHERE t.id is not null and t.owner is not null  ");

                    if (objPropMapData.IsAssignedProject == true) // check for company
                    {
                        strQueryO.AppendFormat(" and j.ProjectManagerUserID= {0}", objPropMapData.EmpID);
                    }
                    #region FILTER FOR Ticket O

                    if (objPropMapData.IsList != 1)
                    {
                        strQueryO.Append(" and assigned NOT IN ( 0 )");
                    }
                    if (objPropMapData.EN == 1) // check for company
                    {
                        strQueryO.AppendFormat(" and UC.IsSel = 1 and UC.UserID ={0}", objPropMapData.UserID);
                    }
                    if (objPropMapData.Assigned != -1)//&& objPropMapData.Assigned != 0
                    {
                        if (objPropMapData.Assigned == -2)
                        {
                            strQueryO.Append(" and t.Assigned <> 4");
                        }
                        else
                        {
                            strQueryO.AppendFormat(" and t.Assigned={0}", objPropMapData.Assigned);
                        }
                    }

                    if (!isFilterHasCommaDelimited && objPropMapData.InvoiceDate == null)
                    {
                        if (objPropMapData.StartDate != System.DateTime.MinValue)
                        {
                            strQueryO.AppendFormat(" and isnull(t.edate,t.cdate) >='{0}'", objPropMapData.StartDate.ToString("MM/dd/yyyy"));
                        }
                        if (objPropMapData.EndDate != System.DateTime.MinValue)
                        {
                            strQueryO.AppendFormat(" and isnull(t.edate,t.cdate) <'{0}'", objPropMapData.EndDate.AddDays(1).ToString("MM/dd/yyyy"));
                        }
                    }


                    if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                    {

                        if (objPropMapData.Worker == "Active")
                        {
                            strQueryO.Append(" and t.fwork in (select w.ID from tblWork w WITH(NOLOCK) where  w.Status=0 )");
                        }
                        else if (objPropMapData.Worker == "Inactive")
                        {
                            strQueryO.Append(" and t.fwork in (select w.ID from tblWork w WITH(NOLOCK) where  w.Status=1 )");
                        }
                        else
                        {
                            strQueryO.AppendFormat(" and (select w.fdesc from tblWork w WITH(NOLOCK) where w.ID=t.fwork )='{0}'", objPropMapData.Worker.Replace("'", "''"));
                        }
                    }
                    if (objPropMapData.LocID != 0)
                    {
                        strQueryO.AppendFormat(" and t.LID={0}", objPropMapData.LocID);
                    }
                    if (objPropMapData.CustID != 0)
                    {
                        strQueryO.AppendFormat(" and t.Owner={0}", objPropMapData.CustID);
                    }
                    if (objPropMapData.jobid != 0)
                    {
                        strQueryO.AppendFormat(" and t.job ={0}", objPropMapData.jobid);
                    }
                    if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
                    {
                        strQueryO.AppendFormat(" and (isnull(t.charge,0)={0}", Convert.ToInt32(objPropMapData.FilterCharge));
                        if (objPropMapData.FilterCharge == "1")
                            strQueryO.Append(" or isnull(Invoice,0) <> 0)");
                        else
                            strQueryO.Append(" and isnull(Invoice,0) = 0)");
                    }
                    if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                    {
                        strQueryO.AppendFormat(" and (select Super from tblWork w WITH(NOLOCK) where w.fdesc=t.dwork ) ='{0}'", objPropMapData.Supervisor);
                    }
                    if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
                    {
                        strQueryO.AppendFormat(" and (select Super from tblWork w WITH(NOLOCK) where w.fdesc=t.dwork ) <>'{0}'", objPropMapData.NonSuper);
                    }

                    
                    if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
                    {
                        strQueryO.AppendFormat(" and t.workorder='{0}", objPropMapData.Workorder);
                    }
                    if (objPropMapData.Department != -1)
                    {
                        strQueryO.AppendFormat(" and t.type={0}", objPropMapData.Department);
                    }
                    if (objPropMapData.Category != string.Empty && objPropMapData.Category != null)
                    {
                        strQueryO.AppendFormat(" and t.cat in ({0})", objPropMapData.Category);
                    }
                    #region ////Filter for ticketo IF ticket Create from Portal
                    if (objPropMapData.IsPortal != string.Empty && objPropMapData.IsPortal != null)
                    {
                        if (objPropMapData.IsPortal == "1")
                        {
                            strQueryO.Append(" and t.fBy='portal' ");
                        }
                        if (objPropMapData.IsPortal == "0")
                        {
                            strQueryO.Append(" and t.fBy <> 'portal' ");
                        }
                    }
                    #endregion

                    #region default route check

                    if (!string.IsNullOrEmpty(objPropMapData.Route))
                    {
                        if (Convert.ToInt32(objPropMapData.Route) == 0)
                        {
                            strQueryO.Append(" and l.Route=0 ");
                        }
                        else
                        {
                            strQueryO.AppendFormat(" and ro.ID={0}", Convert.ToInt32(objPropMapData.Route));
                        }
                    }

                    #endregion

                    if (objPropMapData.Bremarks != null && objPropMapData.Bremarks != string.Empty)
                    {
                        if (objPropMapData.Bremarks == "1")
                            strQueryO.Append(" and isnull( rtrim(ltrim(t.bremarks)),'')<>''");
                        else
                            strQueryO.Append(" and isnull( rtrim(ltrim(t.bremarks)),'')=''");
                    }

                    if (objPropMapData.SearchBy != "" && objPropMapData.SearchBy != null && !string.IsNullOrWhiteSpace(objPropMapData.SearchValue))
                    {
                        string SearchBy = objPropMapData.SearchBy.Replace("t.descres", "dp.descres");

                        if (SearchBy == "t.ID")
                        {
                            strQueryO.Append(" and " + SearchBy + " in (" + UpdateListTicketIDsForSearch(objPropMapData.SearchValue) + ")");
                        }
                        else if (SearchBy == "t.Level")
                        {
                            strQueryO.Append("\n  and t.Level in (" + objPropMapData.SearchValue + ")");
                        }
                        else
                        {
                            strQueryO.Append(" and " + SearchBy + " like '%" + objPropMapData.SearchValue + "%'");
                        }
                    }

                    if (objPropMapData.InvoiceID != 0)
                    {
                        if (objPropMapData.InvoiceID == 1)
                        {
                            strQueryO.Append(" and isnull(Invoice,0) <> 0");
                        }
                        else if (objPropMapData.InvoiceID == 2)
                        {
                            strQueryO.Append(" and isnull(Invoice,0) = 0 and isnull(dp.charge,0)= 1");
                        }
                    }
                    //  If the user is a salesperson or Second salesperson only show assigned  locations,Ticket.
                    if (IsSalesAsigned > 0)
                    {
                        strQueryO.Append(" AND  (  l.Terr=(" + "select id FROM  Terr WITH(NOLOCK) WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSalesAsigned + ")) or  isnull(l.Terr2,0)=(" + "select id FROM  Terr WITH(NOLOCK) WHERE Name=(SELECT fUser FROM  tblUser WITH(NOLOCK) WHERE id=" + IsSalesAsigned + "))) ");
                    }

                    if (filters.Count > 0)
                    {
                        foreach (var items in filters)
                        {
                            if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                            {
                                /// Int Filter
                                int FilterValue = 0; string[] filterArrayValue;
                                StringBuilder filteredQuery = new StringBuilder();
                                if (items.FilterColumn == "ID")
                                {
                                    filterArrayValue = items.FilterValue.ToString().Split(',');
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
                                    strQueryO.Append(" and t.ID  in (" + filteredQuery.ToString() + ")");
                                }

                                if (items.FilterColumn == "Job" & int.TryParse(items.FilterValue, out FilterValue))
                                {
                                    strQueryO.Append(" and t.job  = " + FilterValue);
                                }

                                /// string Filter

                                if (items.FilterColumn == "WorkOrder")
                                {
                                    strQueryO.Append(" and t.WorkOrder  = '" + items.FilterValue + "'");
                                }
                                if (items.FilterColumn == "locname")
                                {
                                    strQueryO.Append(" and l.Tag  like '%" + items.FilterValue + "%'");
                                }
                                if (items.FilterColumn == "City")
                                {
                                    strQueryO.Append(" and l.City  like '%" + items.FilterValue + "%'");
                                }
                                if (items.FilterColumn == "cat")
                                {
                                    strQueryO.Append(" and t.cat  like '%" + items.FilterValue + "%'");
                                }
                                if (items.FilterColumn == "fullAddress")
                                {
                                    strQueryO.Append(" and ( l.address + ', ' + l.city + ', ' + l.state + ', ' + l.zip )  like '%" + items.FilterValue + "%'");
                                }
                            }
                        }
                    }

                    if (strQueryO.Length > 0)
                        strQuery.Append(strQueryO);

                    #endregion End Filter For TicketO

                    #region LocID == 0

                    if (objPropMapData.LocID == 0)
                    {
                        //str += "\n  \n  \n   Union all ";

                        strQueryPDA.Append(" SELECT");
                        strQueryPDA.Append("     t.ID[Ticket#], ");
                        strQueryPDA.Append("     t.WorkOrder[WO#],");
                        strQueryPDA.Append("     t.EDate AS[Date],");
                        strQueryPDA.Append("     r.Name AS [Customer Name],");
                        strQueryPDA.Append("     r.name AS Location,");
                        strQueryPDA.Append("     (select  STUFF((SELECT ', ' + CAST(e.Unit AS VARCHAR(1000))");
                        strQueryPDA.Append("         FROM elev e WITH(NOLOCK) where  e.id in");
                        strQueryPDA.Append("             (select me.elev_id from multiple_equipments me WITH(NOLOCK) where me.ticket_id = t.ID)");
                        strQueryPDA.Append("             FOR XML PATH(''), Type)  .value('.', 'VARCHAR(1000)'), 1, 2, ' ') ) as Equipment, ");
                        strQueryPDA.Append("     Case WHEN ISNULL(t.Job, 0) = 0 THEN '' ELSE Convert(varchar(50),t.Job) END[Project#],");
                        strQueryPDA.Append("     convert(varchar(20), dp.invoice) as [Invoice#],");
                        strQueryPDA.Append("     '' as [ManualInv.#],");
                        strQueryPDA.Append("     (convert(varchar(20), t.Job) + '-' + j.fdesc) as ProjectDesc,");
                        strQueryPDA.Append("      l.city, l.state, l.zip,");
                        strQueryPDA.Append("      r.Phone, r.Email,");
                        strQueryPDA.Append("      (r.address + ', ' + r.city + ', ' + r.state + ', ' + r.zip) AS Fulladdress,");
                        strQueryPDA.Append("      UPPER(dwork) AS Assignedto,");
                        strQueryPDA.Append("      '' name,");
                        strQueryPDA.Append("      t.Cat Category,");
                        strQueryPDA.Append("      CASE WHEN assigned = 0 THEN 'Un-Assigned' WHEN assigned = 1 THEN 'Assigned' WHEN assigned = 2 THEN 'Enroute' WHEN assigned = 3 THEN 'Onsite' WHEN assigned = 4 THEN 'Completed' WHEN assigned = 5 THEN 'Hold' WHEN assigned = 6 THEN 'Voided' END AS[Status],");
                        strQueryPDA.Append("      isnull(B.Name, '') As Company,");
                        strQueryPDA.Append("      Isnull(dp.Total, 0.00) AS TotalTime,");
                        strQueryPDA.Append("      Round(CONVERT(NUMERIC(30, 2), (Isnull(dp.Total, 0.00) - (CONVERT(FLOAT, Datediff(MILLISECOND, dp.TimeRoute, (CASE");
                        strQueryPDA.Append("          WHEN(Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME)");
                        strQueryPDA.Append("                  AND Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME)) THEN Dateadd(day, 2, dp.TimeComp)");
                        strQueryPDA.Append("          ELSE((CASE");
                        strQueryPDA.Append("                      WHEN(Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME)");
                        strQueryPDA.Append("                          OR Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME)) THEN Dateadd(day, 1, dp.TimeComp)");
                        strQueryPDA.Append("                      ELSE dp.TimeComp");
                        strQueryPDA.Append("                  END))");
                        strQueryPDA.Append("          END))) / 1000 / 60 / 60))), 1)  AS TimeDiff,");
                        strQueryPDA.Append("     (jt.type) as department,  ");
                        strQueryPDA.Append("     t.fDesc[Reason for Service],");
                        strQueryPDA.Append("    dp.descres[Work Complete Desc],");
                        strQueryPDA.Append("    t.who Caller,");
                        strQueryPDA.Append("    t.CPhone[Caller Phone],");
                        strQueryPDA.Append("    t.CDate[Date Called In],");
                        strQueryPDA.Append("    t.Custom3,");
                        strQueryPDA.Append("    t.Custom4,");
                        strQueryPDA.Append("    0 ClearPR,");
                        strQueryPDA.Append("    CONVERT(VARCHAR(10), i.fDate, 101) as InvoiceDate");

                        if (inclCustomField)
                        {
                            strQueryPDA.Append("  ,(SELECT cj.Value FROM tblCustomJob cj LEFT JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID WHERE cf.Label = 'Passed Inspection' AND cj.JobID = t.Job) AS [Last Inspection Date] ");
                        }

                        strQueryPDA.Append(" FROM ticketo t WITH(NOLOCK)");
                        strQueryPDA.Append(" LEFT OUTER JOIN TicketDPDA dp WITH(NOLOCK) ON t.ID = dp.ID");
                        strQueryPDA.Append(" INNER JOIN prospect l WITH(NOLOCK) on l.ID = t.lid");
                        strQueryPDA.Append(" INNER JOIN Rol r WITH(NOLOCK) ON r.ID = l.Rol");
                        strQueryPDA.Append(" LEFT OUTER JOIN Branch B WITH(NOLOCK) on B.ID = r.EN");
                        strQueryPDA.Append(" LEFT OUTER JOIN Elev e WITH(NOLOCK) on e.ID = t.LElev");
                        strQueryPDA.Append(" LEFT JOIN job j WITH(NOLOCK) on j.ID = t.Job");
                        strQueryPDA.Append(" LEFT JOIN jobtype jt WITH(NOLOCK) on jt.ID = t.type");
                        strQueryPDA.Append(" LEFT JOIN Invoice i WITH(NOLOCK) on dp.Invoice = i.Ref");

                        if (objPropMapData.EN == 1) // check for company
                        {
                            strQueryPDA.Append("\n   LEFT OUTER JOIN tblUserCo UC WITH(NOLOCK) on UC.CompanyID = r.EN ");
                        }
                        strQueryPDA.Append("\n   WHERE (t.id is not null and t.owner is null and t.LType=1  ");

                        if (objPropMapData.IsAssignedProject == true) // check for company
                        {
                            strQueryPDA.Append("\n   and j.ProjectManagerUserID= " + objPropMapData.EmpID);
                        }

                        #region FILTER FOR TICKET_DPDA
                        if (objPropMapData.IsList != 1)
                        {
                            strQueryPDA.Append(" and assigned NOT IN ( 0 )");
                        }
                        if (objPropMapData.EN == 1) // check for company
                        {
                            strQueryPDA.Append(" and UC.IsSel = 1 and UC.UserID =" + objPropMapData.UserID);
                        }
                        if (objPropMapData.Assigned != -1)
                        {
                            if (objPropMapData.Assigned == -2)
                            {
                                strQueryPDA.Append(" and t.Assigned <> 4");
                            }
                            else
                            {
                                strQueryPDA.Append(" and t.Assigned=" + objPropMapData.Assigned);
                            }
                        }
                        if (!isFilterHasCommaDelimited && objPropMapData.InvoiceDate == null)
                        {
                            if (objPropMapData.StartDate != System.DateTime.MinValue)
                            {
                                strQueryPDA.Append(" and isnull(t.edate,t.cdate) >='" + objPropMapData.StartDate + "'");
                            }
                            if (objPropMapData.EndDate != System.DateTime.MinValue)
                            {
                                strQueryPDA.Append(" and isnull(t.edate,t.cdate) <'" + objPropMapData.EndDate.AddDays(1) + "'");
                            }
                        }

                        if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                        {

                            if (objPropMapData.Worker == "Active")
                            {
                                strQueryPDA.Append(" and t.fwork in (select w.ID from tblWork w WITH(NOLOCK) where  w.Status=0 )");
                            }
                            else if (objPropMapData.Worker == "Inactive")
                            {
                                strQueryPDA.Append(" and t.fwork in (select w.ID from tblWork w WITH(NOLOCK) where  w.Status=1 )");
                            }
                            else
                            {
                                strQueryPDA.Append(" and (select w.fdesc from tblWork w WITH(NOLOCK) where w.ID=t.fwork )='" + objPropMapData.Worker.Replace("'", "''") + "'");
                            }
                        }
                        if (objPropMapData.LocID != 0)
                        {
                            strQueryPDA.Append(" and t.LID=" + objPropMapData.LocID);
                        }
                        if (objPropMapData.CustID != 0)
                        {
                            strQueryPDA.Append(" and t.Owner=" + objPropMapData.CustID);
                        }
                        if (objPropMapData.jobid != 0)
                        {
                            strQueryPDA.Append(" and t.job =" + objPropMapData.jobid);
                        }
                        if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
                        {
                            strQueryPDA.Append(" and (isnull(dp.charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge));
                            if (objPropMapData.FilterCharge == "1")
                                strQueryPDA.Append(" or isnull(Invoice,0) <> 0)");
                            else
                                strQueryPDA.Append(" and isnull(Invoice,0) = 0)");
                        }
                        if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                        {
                            strQueryPDA.Append("\n  and (select Super from tblWork w WITH(NOLOCK) where w.fdesc=t.dwork ) ='" + objPropMapData.Supervisor + "'");
                        }
                        if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
                        {
                            strQueryPDA.Append("\n  and (select Super from tblWork w WITH(NOLOCK) where w.fdesc=t.dwork ) <>'" + objPropMapData.NonSuper + "'");
                        }
                       
                        if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
                        {
                            strQueryPDA.Append("\n  and t.Workorder='" + objPropMapData.Workorder + "'");
                        }
                        if (objPropMapData.Department != -1)
                        {
                            strQueryPDA.Append("\n  and t.type=" + objPropMapData.Department);
                        }
                        if (objPropMapData.Category != string.Empty && objPropMapData.Category != null)
                        {
                            //strQueryPDA.Append(" and t.cat='" + objPropMapData.Category + "'";
                            strQueryPDA.Append("\n  and t.cat in (" + objPropMapData.Category + ")");
                        }
                        #region ////Filter for TicketDPDA IF Create from Portal
                        if (objPropMapData.IsPortal != string.Empty && objPropMapData.IsPortal != null)
                        {
                            if (objPropMapData.IsPortal == "1")
                            {
                                strQueryPDA.Append("\n  and t.fBy='portal' ");
                            }
                            if (objPropMapData.IsPortal == "0")
                            {
                                strQueryPDA.Append("\n  and t.fBy <> 'portal' ");
                            }
                        }
                        #endregion

                        if (objPropMapData.Bremarks != null && objPropMapData.Bremarks != string.Empty)
                        {
                            if (objPropMapData.Bremarks == "1")
                                strQueryPDA.Append("\n  and isnull( rtrim(ltrim(t.bremarks)),'')<>''");
                            else
                                strQueryPDA.Append("\n  and isnull( rtrim(ltrim(t.bremarks)),'')=''");
                        }
                        if (objPropMapData.SearchBy != "" && objPropMapData.SearchBy != null && !string.IsNullOrWhiteSpace(objPropMapData.SearchValue))
                        {
                            string SearchBy = objPropMapData.SearchBy.Replace("t.descres", "dp.descres");
                            if (SearchBy == "t.ID")
                            {
                                //strQueryPDA.Append(" and " + SearchBy + " = '" + objPropMapData.SearchValue + "'";
                                strQueryPDA.Append(" and " + SearchBy + " in (" + UpdateListTicketIDsForSearch(objPropMapData.SearchValue) + ")");
                            }
                            else if (SearchBy == "t.Level")
                            {
                                strQueryPDA.Append("and t.Level in (" + objPropMapData.SearchValue + ")");
                            }
                            else
                            {
                                if (SearchBy == "l.tag")
                                    SearchBy = "r.name";
                                strQueryPDA.Append(" and " + SearchBy + " like '%" + objPropMapData.SearchValue + "%'");
                            }
                        }
                        if (objPropMapData.InvoiceID != 0)
                        {
                            if (objPropMapData.InvoiceID == 1)
                            {
                                strQueryPDA.Append("\n  and isnull(Invoice,0) <> 0");
                            }
                            else if (objPropMapData.InvoiceID == 2)
                            {
                                strQueryPDA.Append("\n  and isnull(Invoice,0) = 0 and isnull(dp.charge,0)= 1");
                            }
                        }
                        //  If the user is a salesperson only show assigned  locations,Ticket.
                        if (IsSalesAsigned > 0)
                        {
                            strQueryPDA.Append("\n  AND  ( l.Terr=(" + "select id FROM  Terr WITH(NOLOCK) WHERE Name=(SELECT fUser FROM  tblUser WITH(NOLOCK) WHERE id=" + IsSalesAsigned + ")) )");
                        }

                        if (filters.Count > 0)
                        {
                            foreach (var items in filters)
                            {
                                if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                                {
                                    /// Int Filter
                                    int FilterValue = 0;
                                    //if (items.FilterColumn == "ID" & int.TryParse(items.FilterValue, out FilterValue))
                                    //{
                                    //    strQueryPDA.Append("\n  and t.ID  = " + FilterValue;
                                    //}

                                    if (items.FilterColumn == "ID")
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
                                        strQueryPDA.Append("\n and t.ID  in (" + filteredQuery.ToString() + ")");
                                    }

                                    if (items.FilterColumn == "Job" & int.TryParse(items.FilterValue, out FilterValue))
                                    {
                                        strQueryPDA.Append("\n  and t.job  = " + FilterValue);
                                    }

                                    /// string Filter

                                    if (items.FilterColumn == "WorkOrder")
                                    {
                                        strQueryPDA.Append("\n  and t.WorkOrder  = '" + items.FilterValue + "'");
                                    }

                                    if (items.FilterColumn == "locname")
                                    {
                                        strQueryPDA.Append("\n  and r.name  like '%" + items.FilterValue + "%'");
                                    }
                                    if (items.FilterColumn == "City")
                                    {
                                        strQueryPDA.Append("\n  and r.City  like '%" + items.FilterValue + "%'");
                                    }
                                    if (items.FilterColumn == "fullAddress")
                                    {
                                        strQueryPDA.Append("\n  and ( r.address + ', ' + r.city + ', ' + r.state + ', ' + r.zip )  like '%" + items.FilterValue + "%'");
                                    }

                                }
                            }
                        }

                        strQueryPDA.Append(" )");
                        if (!string.IsNullOrEmpty(objPropMapData.Route))
                        {
                            strQueryPDA.Append(" AND 1<>1");
                        }

                        #endregion FILTER FOR TICKET_DPDA
                        if (strQuery.Length > 0)
                            strQuery.AppendFormat(" UNION ALL {0}", strQueryPDA);
                        else
                            strQuery.Append(strQueryPDA);
                    }

                    #endregion LocID == 0
                }

                #endregion FilterReview != "1"
            }

            #endregion Status != Assigned

            #region Status =  Completed/ALL Get Ticket From TicketD
            if (objPropMapData.Assigned == 4 || objPropMapData.Assigned == -1)
            {
                
                {

                    if (objPropMapData.Status != 1)
                    {
                        if (objPropMapData.FilterReview != "1")
                        {
                            //str += "\n  \n  \n   UNION ALL";
                        }
                    }

                    strQueryD.Append(" SELECT");
                    strQueryD.Append("     t.ID [Ticket#], ");
                    strQueryD.Append("     t.WorkOrder [WO#],");
                    strQueryD.Append("     t.EDate AS [Date],");
                    strQueryD.Append("     r.Name  AS [Customer Name],");
                    strQueryD.Append("     l.Tag  AS Location,");
                    strQueryD.Append("     (select  STUFF((SELECT ', ' + CAST(e.Unit AS VARCHAR(1000))");
                    strQueryD.Append("         FROM elev e WITH(NOLOCK) where  e.id in");
                    strQueryD.Append("             (select me.elev_id from multiple_equipments me WITH(NOLOCK) where me.ticket_id = t.ID)");
                    strQueryD.Append("             FOR XML PATH(''), Type)  .value('.', 'VARCHAR(1000)'), 1, 2, ' ') ) as Equipment, ");
                    strQueryD.Append("     Case WHEN ISNULL(t.Job, 0) = 0 THEN '' ELSE Convert(varchar(50),t.Job) END[Project#],");
                    strQueryD.Append("     case  when(Isnull(convert(varchar(20), invoice), '0') = '0') then Manualinvoice else CONVERT(varchar(20), Invoice) end as [Invoice#],");
                    strQueryD.Append("     manualinvoice[ManualInv.#], ");
                    strQueryD.Append("     (convert(varchar(20), t.Job) + '-' + j.fdesc) as ProjectDesc,");
                    strQueryD.Append("  l.city, l.state, l.zip,");
                    strQueryD.Append("  lr.Phone, lr.Email,");
                    strQueryD.Append("  (l.address + ',' + l.city + ',' + l.state + ', ' + l.zip) as Fulladdress,");
                    strQueryD.Append("  (select UPPER(w.fdesc) from tblWork w WITH(NOLOCK) where t.fwork = w.id) AS Assignedto,");
                    strQueryD.Append("  (select(case  when ro.Name IS NULL  then 'Unassigned'");
                    strQueryD.Append("                  when tblwork.fdesc is null then  ro.Name");
                    strQueryD.Append("                  when tblwork.fdesc = ro.Name then ro.Name else ro.Name + ' - ' + tblwork.fdesc  end)");
                    strQueryD.Append("             from tblwork where tblwork.id = ro.mech");
                    strQueryD.Append("     ) as Name, ");
                    strQueryD.Append("     t.cat Category,");
                    strQueryD.Append("     case   t.Assigned when 6 then 'Voided' else 'Completed' end AS[Status],");
                    strQueryD.Append("     isnull(B.Name, '') As Company,");
                    strQueryD.Append("     t.Total as TotalTime, ");
                    strQueryD.Append("     Round(CONVERT(NUMERIC(30, 2), (Isnull(t.Total, 0.00) - (CONVERT(FLOAT, Datediff(MILLISECOND, TimeRoute, (CASE");
                    strQueryD.Append("         WHEN(Cast(TimeSite AS TIME) < Cast(TimeRoute AS TIME)");
                    strQueryD.Append("             AND Cast(TimeComp AS TIME) < Cast(TimeSite AS TIME)) THEN Dateadd(day, 2, TimeComp)");
                    strQueryD.Append("         ELSE((CASE");
                    strQueryD.Append("                 WHEN(Cast(TimeSite AS TIME) < Cast(TimeRoute AS TIME)");
                    strQueryD.Append("                     OR Cast(TimeComp AS TIME) < Cast(TimeSite AS TIME)) THEN Dateadd(day, 1, TimeComp)");
                    strQueryD.Append("                 ELSE TimeComp");
                    strQueryD.Append("             END))");
                    strQueryD.Append("         END))) / 1000 / 60 / 60))), 1)  AS TimeDiff,");
                    strQueryD.Append("     (jt.type) as department, ");
                    strQueryD.Append("     t.fdesc [Reason for Service],");
                    strQueryD.Append("    descres [Work Complete Desc],");
                    strQueryD.Append("    t.who Caller,");
                    strQueryD.Append("    t.CPhone [Caller Phone],");
                    strQueryD.Append("    cdate [Date Called In],");
                    strQueryD.Append("    t.Custom3,");
                    strQueryD.Append("    t.Custom4,");
                    strQueryD.Append("    isnull(t.ClearPR,0) ClearPR,");
                    strQueryD.Append("    CONVERT(VARCHAR(10), i.fDate, 101) as InvoiceDate");

                    if (inclCustomField)
                    {
                        strQueryD.Append("  ,(SELECT cj.Value FROM tblCustomJob cj LEFT JOIN tblCustomFields cf ON cf.id = cj.tblCustomFieldsID WHERE cf.Label = 'Passed Inspection' AND cj.JobID = t.Job) AS [Last Inspection Date] ");
                    }

                    strQueryD.Append(" FROM ticketd t WITH(NOLOCK)");
                    strQueryD.Append(" INNER JOIN loc l WITH(NOLOCK) ON l.loc = t.loc");
                    strQueryD.Append(" INNER JOIN Rol lr WITH(NOLOCK) on lr.ID = l.Rol");
                    strQueryD.Append(" INNER JOIN Owner o WITH(NOLOCK) on l.Owner = o.ID");
                    strQueryD.Append(" INNER JOIN Rol r WITH(NOLOCK) ON r.ID = o.Rol");
                    strQueryD.Append(" LEFT OUTER JOIN Branch B WITH(NOLOCK) on B.ID = r.EN");
                    strQueryD.Append(" LEFT OUTER JOIN Elev e WITH(NOLOCK) on e.ID = t.Elev");
                    strQueryD.Append(" LEFT JOIN Route ro WITH(NOLOCK) on ro.ID = l.Route");
                    strQueryD.Append(" LEFT JOIN job j WITH(NOLOCK) on j.ID = t.Job");
                    strQueryD.Append(" LEFT JOIN jobtype jt WITH(NOLOCK) on jt.ID = t.type");
                    strQueryD.Append(" LEFT JOIN Invoice i WITH(NOLOCK) on t.Invoice = i.ref");
                    strQueryD.Append(" LEFT JOIN tblinvoicepayment ipmt WITH(NOLOCK) on cast(ipmt.ref as varchar(50)) = t.ManualInvoice");

                    if (objPropMapData.EN == 1) // check for company
                    {
                        strQueryD.Append("\n LEFT OUTER JOIN tblUserCo UC WITH(NOLOCK) on UC.CompanyID = r.EN ");
                    }
                    strQueryD.Append("\n WHERE t.id is not null  ");
                    if (objPropMapData.IsAssignedProject == true) // check for company
                    {
                        strQueryD.Append("\n   and j.ProjectManagerUserID= " + objPropMapData.EmpID);
                    }
                    #region FILTER FOR TICKETD
                    if (!isFilterHasCommaDelimited && objPropMapData.InvoiceDate == null)
                    {
                        if (objPropMapData.StartDate != System.DateTime.MinValue)
                        {
                            strQueryD.Append("\n  and edate >='" + objPropMapData.StartDate.ToString("MM/dd/yyyy") + "'");
                        }
                        if (objPropMapData.EndDate != System.DateTime.MinValue)
                        {
                            strQueryD.Append("\n  and edate <  '" + objPropMapData.EndDate.AddDays(1).ToString("MM/dd/yyyy") + "'");
                        }
                    }


                    if (objPropMapData.EN == 1) // check for company
                    {
                        strQueryD.Append("\n  and UC.IsSel = 1 and UC.UserID =" + objPropMapData.UserID);
                    }
                    if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                    {

                        if (objPropMapData.Worker == "Active")
                        {
                            strQueryD.Append("\n  and t.fwork in (select w.ID from tblWork w WITH(NOLOCK) where  w.Status=0 )");
                        }
                        else if (objPropMapData.Worker == "Inactive")
                        {
                            strQueryD.Append("\n  and t.fwork in (select w.ID from tblWork w WITH(NOLOCK) where  w.Status=1 )");
                        }
                        else
                        {
                            strQueryD.Append("\n  and (select w.fdesc from tblWork w WITH(NOLOCK) where w.ID=t.fwork )='" + objPropMapData.Worker.Replace("'", "''") + "'");
                        }
                    }
                    if (objPropMapData.LocID != 0)
                    {
                        strQueryD.Append("\n  and l.loc=" + objPropMapData.LocID);
                    }
                    if (objPropMapData.CustID != 0)
                    {
                        strQueryD.Append("\n  and l.Owner=" + objPropMapData.CustID);
                    }
                    if (objPropMapData.jobid != 0)
                    {
                        strQueryD.Append("\n  and t.job =" + objPropMapData.jobid);
                    }
                    if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
                    {
                        strQueryD.Append("\n  and (isnull(t.charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge));
                        if (objPropMapData.FilterCharge == "1")
                            strQueryD.Append(" or isnull(Invoice,0) <> 0)");
                        else
                            strQueryD.Append("\n  and isnull(Invoice,0) = 0)");
                    }
                    if (objPropMapData.Timesheet != null && objPropMapData.Timesheet != string.Empty)
                    {
                        strQueryD.Append("\n  and isnull(TransferTime,0)=" + Convert.ToInt32(objPropMapData.Timesheet));
                    }
                    if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                    {
                        strQueryD.Append("\n  and (select Super from tblWork w WITH(NOLOCK) where w.ID=t.fwork ) ='" + objPropMapData.Supervisor + "'");
                    }
                    if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
                    {
                        strQueryD.Append("\n  and (select Super from tblWork w WITH(NOLOCK) where w.ID=t.fwork ) <>'" + objPropMapData.NonSuper + "'");
                    }
                    if (objPropMapData.FilterReview != null && objPropMapData.FilterReview != string.Empty)
                    {
                        strQueryD.Append("\n  and isnull( ClearCheck ,0) =" + Convert.ToInt32(objPropMapData.FilterReview));
                    }
                    if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
                    {
                        strQueryD.Append("\n  and t.Workorder='" + objPropMapData.Workorder + "'");
                    }
                    if (objPropMapData.Department != -1)
                    {
                        strQueryD.Append("\n  and t.type=" + objPropMapData.Department);
                    }
                    if (objPropMapData.Category != string.Empty && objPropMapData.Category != null)
                    {

                        strQueryD.Append("\n  and t.cat in (" + objPropMapData.Category + ")");
                    }
                    #region ////Filter For TicketD IF Create from Portal
                    if (objPropMapData.IsPortal != string.Empty && objPropMapData.IsPortal != null)
                    {
                        if (objPropMapData.IsPortal == "1")
                        {
                            strQueryD.Append("\n  and t.fBy='portal' ");
                        }
                        if (objPropMapData.IsPortal == "0")
                        {
                            strQueryD.Append("\n  and t.fBy <> 'portal' ");
                        }
                    }
                    #endregion

                    #region default route check

                    if (!string.IsNullOrEmpty(objPropMapData.Route))
                    {
                        if (Convert.ToInt32(objPropMapData.Route) == 0)
                        {
                            strQueryD.Append("\n  and l.Route=0 ");
                        }
                        else
                        {
                            strQueryD.Append("\n  and ro.ID=" + Convert.ToInt32(objPropMapData.Route) + " ");
                        }
                    }

                    #endregion

                    if (objPropMapData.Bremarks != null && objPropMapData.Bremarks != string.Empty)
                    {
                        if (objPropMapData.Bremarks == "1")
                            strQueryD.Append("\n and isnull( rtrim(ltrim(t.bremarks)),'')<>''");
                        else
                            strQueryD.Append("\n and isnull( rtrim(ltrim(t.bremarks)),'')=''");
                    }
                    if (objPropMapData.SearchBy != "" && objPropMapData.SearchBy != null && !string.IsNullOrWhiteSpace(objPropMapData.SearchValue))
                    {
                        if (objPropMapData.SearchBy == "t.ldesc4")
                        {
                            strQueryD.Append("\n and l.address like '%" + objPropMapData.SearchValue + "%'");
                        }
                        else if (objPropMapData.SearchBy == "t.ID")
                        {

                            //strQueryD.Append("\n and " + objPropMapData.SearchBy + " = '" + objPropMapData.SearchValue + "'";
                            strQueryD.Append("\n and " + objPropMapData.SearchBy + " in (" + UpdateListTicketIDsForSearch(objPropMapData.SearchValue) + ")");
                        }
                        else if (objPropMapData.SearchBy == "t.Level")
                        {
                            strQueryD.Append("\n and t.Level in (" + objPropMapData.SearchValue + ")");
                        }
                        else
                        {
                            strQueryD.Append("\n and " + objPropMapData.SearchBy + " like '%" + objPropMapData.SearchValue + "%'");
                        }
                    }
                    if (objPropMapData.Status == 1)
                    {
                        strQueryD.Append("\n and isnull( t.status ,0) <> 1 and isnull(t.internet,0) = 1");
                    }

                    if (objPropMapData.RoleID != 0)
                        strQueryD.Append("\n and isnull(l.roleid,0)=" + objPropMapData.RoleID);

                    //}
                    if (objPropMapData.InvoiceID != 0)
                    {
                        if (objPropMapData.InvoiceID == 1)
                        {
                            strQueryD.Append("\n and (isnull(Invoice,0) <> 0  or manualinvoice <> '')");
                        }
                        else if (objPropMapData.InvoiceID == 2)
                        {
                            strQueryD.Append("\n and (isnull(Invoice,0) = 0 or manualinvoice = '') and isnull(t.charge,0)= 1");
                        }
                    }
                    //  If the user is a salesperson only show assigned  locations,Ticket.
                    if (IsSalesAsigned > 0)
                    {
                        strQueryD.Append("\n AND   (l.Terr=(" + "select id FROM  Terr WITH(NOLOCK) WHERE Name=(SELECT fUser FROM  tblUser WITH(NOLOCK) WHERE id=" + IsSalesAsigned + ")) or isnull(l.Terr2,0)=(" + "select id FROM  Terr WITH(NOLOCK) WHERE Name=(SELECT fUser  FROM  tblUser WITH(NOLOCK) WHERE id=" + IsSalesAsigned + ")))");
                    }

                    if (filters.Count > 0)
                    {
                        foreach (var items in filters)
                        {
                            if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                            {
                                /// Int Filter
                                int FilterValue = 0;
                                //if (items.FilterColumn == "ID" & int.TryParse(items.FilterValue, out FilterValue))
                                //{
                                //    strQueryD.Append("\n and t.ID  = " + FilterValue;
                                //}

                                if (items.FilterColumn == "ID")
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
                                    strQueryD.Append("\n and t.ID  in (" + filteredQuery.ToString() + ")");
                                }

                                if (items.FilterColumn == "Job" & int.TryParse(items.FilterValue, out FilterValue))
                                {
                                    strQueryD.Append("\n and t.job  = " + FilterValue);
                                }

                                /// string Filter

                                if (items.FilterColumn == "WorkOrder")
                                {
                                    strQueryD.Append("\n and t.WorkOrder  = '" + items.FilterValue + "'");
                                }
                                if (items.FilterColumn == "locname")
                                {
                                    strQueryD.Append("\n and l.Tag  like '%" + items.FilterValue + "%'");
                                }
                                if (items.FilterColumn == "City")
                                {
                                    strQueryD.Append("\n and l.City  like '%" + items.FilterValue + "%'");
                                }
                                if (items.FilterColumn == "cat")
                                {
                                    strQueryD.Append("\n and t.cat  like '%" + items.FilterValue + "%'");
                                }

                                if (items.FilterColumn == "fullAddress")
                                {
                                    strQueryD.Append("\n  and ( l.address + ', ' + l.city + ', ' + l.state + ', ' + l.zip )  like '%" + items.FilterValue + "%'");
                                }
                            }
                        }
                    }

                    if (strQuery.Length > 0)
                        strQuery.AppendFormat(" UNION ALL {0}", strQueryD);
                    else
                        strQuery.Append(strQueryD);
                    #endregion FILTER FOR TICKETD
                }
            }
            #endregion Completed/ALL
            var strSelectColumns = " * ";
            if (objPropMapData.EN == 1)
            {
                strSelectColumns = " [Ticket#], [WO#],[Date],[Customer Name],Location,Equipment, [Project#],[Invoice#],InvoiceDate,[ManualInv.#], ProjectDesc,City,State,Zip,Fulladdress as Address,Phone,Email," +
                       "Assignedto,Name as [Default Worker], Category,[Status],Company,TotalTime,TimeDiff,Department, [Reason for Service]," +
                       "[Work Complete Desc],Caller,[Caller Phone],[Date Called In],Custom3, Custom4";

                if (inclCustomField)
                {
                    strSelectColumns += ",[Last Inspection Date]";
                }
            }
            else
            {
                strSelectColumns = " [Ticket#], [WO#],[Date],[Customer Name],Location,Equipment, [Project#],[Invoice#],InvoiceDate,[ManualInv.#], ProjectDesc,City,State,Zip,Fulladdress as Address,Phone,Email," +
                        "Assignedto,Name as [Default Worker], Category,[Status],TotalTime,TimeDiff,Department, [Reason for Service]," +
                        "[Work Complete Desc],Caller,[Caller Phone],[Date Called In],Custom3, Custom4";

                if (inclCustomField)
                {
                    strSelectColumns += ",[Last Inspection Date]";
                }
            }
            strQuery.Insert(0, "SELECT " + strSelectColumns + " FROM (");

            strQuery.Append(") xTicket WHERE 1=1 ");

            if (objPropMapData.Mobile == 2)
            {
                strQuery.Append("\n and xTicket.fBy  = 'MOBILEUSER' ");
            }

            if (objPropMapData.Mobile == 1)
            {

                strQuery.Append("\n and xTicket.fBy  <> 'MOBILEUSER' ");
            }

            if (objPropMapData.InvoiceDate == "Yes")
            {
                strQuery.Append("\n AND xTicket.InvoiceDate >=CAST('" + objPropMapData.StartDate.ToString("MM/dd/yyyy") + "' as date) ");
                strQuery.Append("\n AND xTicket.InvoiceDate <= cast('" + objPropMapData.EndDate.ToString("MM/dd/yyyy") + "' as date)");
            }
            ///////////

            //strQueryD.Append("\n  \n   ) xTicket WHERE 1=1");

            if (objPropMapData.Voided == 1)
            {
                strQuery.Append("\n and xTicket.[Status]  = 'Voided' ");
            }

            if (objPropMapData.Assigned == 4 && objPropMapData.Voided != 1)
            {
                strQueryD.Append("\n and xTicket.[Status]  <> 'Voided' ");
            }

            if (filters.Count > 0)
            {
                foreach (var items in filters)
                {
                    if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                    {
                        /// Int Filter
                        int FilterValue = 0;
                        if (items.FilterColumn == "Tottime" & int.TryParse(items.FilterValue, out FilterValue))
                        {
                            strQuery.Append("\n and xTicket.TotalTime  = " + FilterValue);
                        }
                        if (items.FilterColumn == "invoiceno" & int.TryParse(items.FilterValue, out FilterValue))
                        {
                            strQuery.Append("\n and xTicket.[Invoice#]  = " + FilterValue);
                        }
                        if (items.FilterColumn == "timediff" & int.TryParse(items.FilterValue, out FilterValue))
                        {
                            strQuery.Append("\n and xTicket.[Time Diff]  = " + FilterValue);
                        }
                        /// string Filter

                        if (items.FilterColumn == "assignname")
                        {
                            strQuery.Append("\n and xTicket.[Status]  like '%" + items.FilterValue + "%'");
                        }

                        if (items.FilterColumn == "locname")
                        {
                            strQuery.Append("\n and xTicket.Location  like '%" + items.FilterValue + "%'");
                        }

                        if (items.FilterColumn == "assignname" && items.FilterValue == "Completed" && objPropMapData.Voided != 1)
                        {
                            strQuery.Append("\n and xTicket.[Status]  <> 'Voided' ");
                        }
                        if (items.FilterColumn == "fulladdress")
                        {
                            strQuery.Append("\n and xTicket.fulladdress  like '%" + items.FilterValue + "%'");
                        }
                        if (items.FilterColumn == "dwork")
                        {
                            strQuery.Append("\n and xTicket.Assignedto  like '%" + items.FilterValue + "%'");
                        }
                        if (items.FilterColumn == "Company")
                        {
                            strQuery.Append("\n and xTicket.Company  like '%" + items.FilterValue + "%'");
                        }
                        if (items.FilterColumn == "department")
                        {
                            strQuery.Append("\n and xTicket.department  like '%" + items.FilterValue + "%'");
                        }
                        if (items.FilterColumn == "Name")
                        {
                            strQuery.Append("\n and xTicket.Name  like '%" + items.FilterValue + "%'");
                        }
                        if (items.FilterColumn == "unit")
                        {
                            strQuery.Append("\n and (1 = (Select distinct 1 from multiple_equipments me WITH(NOLOCK)  inner join elev e WITH(NOLOCK) on e.id = me.elev_id where me.ticket_id = xTicket.ID and xTicket.Equipment like  '%" + items.FilterValue + "%'))");
                        }
                        if (items.FilterColumn == "InvoiceDate")
                        {
                            strQuery.Append("\n AND xTicket.InvoiceDate >=CAST('" + items.FilterValue + "' as date) ");
                            strQuery.Append("AND xTicket.InvoiceDate <= cast('" + items.FilterValue + "' as date)");
                        }
                    }
                }
            }
            if (objPropMapData.IsPayroll == 1)
            {
                strQuery.Append("\n  and xTicket.ClearPR= 1");
            }
            else if (objPropMapData.IsPayroll == 0)
            {
                strQuery.Append("\n  and xTicket.ClearPR = 0 ");
            }
            try
            {
                if (strQuery.Length > 0)
                {
                    DL_Utility dL_Utility = new DL_Utility();
                    return dL_Utility.ExportToExcel(objPropMapData.ConnConfig, strQuery.ToString(), fileName, rowsPerSheet);
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// GetTicketListReportData
        /// </summary>
        /// <param name="objPropMapData"></param>
        /// <param name="filters"></param>
        /// <param name="IsSalesAsigned"></param>
        /// <param name="IsCallForTicketReport"></param>
        /// <param name="strORDERBY"></param>
        /// <returns></returns> 
        public DataSet GetTicketListReportData(MapData objPropMapData, List<RetainFilter> filters, string fromDate, string toDate,
            Int32 IsSalesAsigned = 0, int IsCallForTicketReport = 0, string strORDERBY = "EDATE ASC", bool GetReportData = false)

        {
            string str = "";

            bool isFilterHasCommaDelimited = CheckFilterHasCommaDelimited(filters);
            if (!isFilterHasCommaDelimited)
            {
                objPropMapData.StartDate = DateTime.Parse(fromDate);
                objPropMapData.EndDate = DateTime.Parse(toDate);
            }
            else
            {
                objPropMapData.SearchBy = "";
                objPropMapData.SearchValue = "";
            }

            str = @"IF OBJECT_ID('tempdb..#jobitemptable') IS NOT NULL  DROP TABLE #jobitemptable
                    CREATE TABLE #jobitemptable  (  Amount NUMERIC(30, 2),  ref  int primary key not null   )
                    INSERT INTO #jobitemptable SELECT Sum(Isnull(Amount, 0)) AS Amount,   ref FROM   jobi WHERE  Labor = 1 AND TransID < 0 GROUP  BY Ref   ";

            str += " SELECT ROW_NUMBER() OVER(ORDER BY " + strORDERBY + " ) AS ROWNO,* FROM (";

            #region Status != Assigned
            if (objPropMapData.Status != 1)
            {
                #region FilterReview != "1"
                if (objPropMapData.FilterReview != "1")
                {
                    str += "  SELECT t.who,t.CPhone,t.lid, l.id as locid, assigned, ( l.address + ', ' + l.city + ', ' + l.state + ', ' + l.zip ) AS fulladdress,l.city, t.WorkOrder, dp.Reg, dp.OT, dp.NT, dp.DT, dp.TT,dp.break_time as BT, dp.Total, 0 AS ClearCheck, t.charge, t.fDesc, t.TimeRoute, t.TimeSite, t.TimeComp, CASE WHEN EXISTS (SELECT 1 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS comp, UPPER(dwork) AS dwork, dwork as lastname,(select isnull(hourlyrate,0) from tblwork where id=t.fwork) as hourlyrate, t.ID, r.Name AS customername, r.EN, isnull(B.Name, '') As Company, dp.PartsUsed,";
                    str += " l.Tag  AS locname, l.Type as LocType, l.Custom1 as LocCustom1, l.Custom2 as LocCustom2, l.Address  AS address,r.phone, t.Cat, t.EDate AS edate, t.CDate, dp.descres, CASE WHEN assigned = 0 THEN 'Un-Assigned' WHEN assigned = 1 THEN 'Assigned' WHEN assigned = 2 THEN 'Enroute' WHEN assigned = 3 THEN 'Onsite' WHEN assigned = 4 THEN 'Completed' WHEN assigned = 5 THEN 'Hold' END AS assignname, t.Est, Isnull(dp.Total, 0.00) AS Tottime ,";



                    //,round(convert(numeric(30,2), (Isnull(dp.Total, 0.00) - (convert(float,DATEDIFF(MILLISECOND,dp.TimeRoute,dp.TimeComp))/1000/60/60))),1) as timediff


                    #region 24 HOURS FEATURE

                    str += @" Round(CONVERT(NUMERIC(30, 2), ( Isnull(dp.Total, 0.00) - ( CONVERT(FLOAT, Datediff(MILLISECOND, dp.TimeRoute, ( CASE
                                                                                                                   WHEN(Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME)
                                                                                                                           AND Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME)) THEN Dateadd(day, 2, dp.TimeComp)
                                                                                                                   ELSE((CASE
                                                                                                                             WHEN(Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME)
                                                                                                                                    OR Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME)) THEN Dateadd(day, 1, dp.TimeComp)
                                                                                                                             ELSE dp.TimeComp
                                                                                                                           END))
                                                                                                                 END ))) / 1000 / 60 / 60 ) )), 1)  AS timediff ";
                    #endregion


                    str += "  , t.workorder as WO, (isnull(dp.zone,0)+ isnull(dp.toll,0) + isnull(dp.othere,0)) as expenses, isnull( dp.zone,0) as zone, isnull( dp.toll,0) as toll , isnull(dp.othere,0) as othere, case ISNUMERIC(dbo.udf_GetNumeric(dp.custom2)) when 1 then CONVERT(money ,dbo.udf_GetNumeric(dp.custom2)) else 0 end as extraexp, ((isnull(dp.emile,0)-isnull(dp.smile,0))*0.26) as mileagetravel, ";
                    str += " (isnull(dp.emile,0)-isnull(dp.smile,0)) as mileage ,(select top 1 count(1) as signatureCount  from pdaticketsignature where pdaticketid=t.ID ) as signatureCount, (select count(1) from documents where screen='Ticket' and screenid=t.id) as DocumentCount, t.fwork as workerid, (select top 1 * from dbo.split(descres,'|')) as description, (select top 1 * from dbo.split(t.fdesc,'|')) as fdescreason, 0 as invoice, isnull(Confirmed,0) as Confirmed, '' as manualinvoice, '' as invoiceno, t.owner as ownerid, '' as QBinvoiceid, 0 as TransferTime, ";
                    str += "  (select name from inv where qbinvid= t.QBserviceitem) as serviceitem, ";
                    str += "  (select fdesc from prwage where QBwageID= t.QBPayrollItem) as PayrollItem, ";
                    str += "  (ISNULL( dp.reg ,0) + ISNULL( dp.OT ,0) +ISNULL( dp.TT ,0))     as RTOTTT ";
                    str += "  ,(select top 1 (last+', '+ffirst) from emp em where em.callsign = t.dwork) as WorkerLastName ";
                    if (objPropMapData.StartDate != System.DateTime.MinValue || objPropMapData.EndDate != System.DateTime.MinValue)
                    {
                        str += " ,(SELECT top 1 Signature	FROM   PDATimeSign p	WHERE  p.fWork = t.fwork and (p.EDate BETWEEN '" + objPropMapData.StartDate + "' and '" + objPropMapData.EndDate + "')	ORDER  BY EDate DESC ) as timesign";
                    }
                    str += ", isnull(l.dispalert,0)as dispalert, isnull(l.credit,0)as credithold, isnull(t.high,0) as high,e.id as unitid, dbo.TicketEquips(t.ID) as unit, dbo.TicketEquipsColumns(t.ID, 'type') as unittype, (select Name from Route where ID=l.Route ) as defaultworker, (select top 1 fdesc from tblwork where id = (select top 1 mech from Route where ID=l.Route )) as defaultmech";
                    str += ", (select type from jobtype where id = t.type) as department,  rtrim(ltrim(t.bremarks)) as bremarks, 0 as laborexp ";
                    str += ", (select top 1 signature from pdaticketsignature where pdaticketid=t.ID ) as signature, l.state, l.zip ";
                    str += ", (select top 1 isnull( Rate,0)  from PROther where Emp = (select top 1 id from emp where fWork = t.fwork) and Cat = 5) as mileagepr ";
                    str += ", dbo.Afterhours(t.timeroute,t.timecomp) as afterhours, dbo.weekends(t.edate) as weekends, isnull(EmailNotified,0) as EmailNotified, EmailTime";
                    if (IsCallForTicketReport > 0)
                    {
                        // Show OtherWorker For  same date , same workorder, same location and same project
                        str += @", ([dbo].Getotherworker(t.fWork,t.WorkOrder,t.LID,t.Job,t.EDate)) AS OtherWorker , CAST(lt.ThirdPartyName + '  ' + lt.ThirdPartyPhone as VARCHAR(1000)) AS ThirdPartyInfo";
                    }
                    str += ",   t.Job";
                    str += "       , (select convert(varchar(20),t.Job )+'-'+ fdesc from job where ID = t.Job) as ProjectDescription ";
                    str += ", t.Custom6, t.Custom7, t.CustomTick1, t.CustomTick5";
                    str += ", j.Certified ";
                    str += ",j.SRemarks as SpecialRemark ";
                    str += ", t.fBy ,isNUll(( Select type  from JobType where id = ( select top 1 department from  tbljoinempdepartment where emp = (select top 1 ID from  emp where callsign = (select  top 1 fDesc from tblWork where id = t.fwork)))),'unassigned') as EmpDepartment ";
                    str += ",(select fDesc from PRWage where id = dp.WageC) as WageCategory,l.Custom14,l.Custom15,i.fDate as InvoiceDate ";
                    str += " FROM ticketo t Left Join LoadTestItem fh on t.ID =fh.Ticket LEFT OUTER JOIN LoadTestItemHistory lh ON t.ID = lh.TicketID LEFT OUTER JOIN LoadTestItemHistoryPrice lt ON fh.LID=lt.LID LEFT OUTER JOIN TicketDPDA dp ON t.ID = dp.ID LEFT JOIN Job j ON j.ID = t.Job inner join Loc l on l.Loc=t.lid left join Route ro on ro.ID = l.Route inner join Owner o on l.Owner=o.ID INNER JOIN Rol r ON r.ID = o.Rol LEFT OUTER JOIN Branch B on B.ID = r.EN  left outer join Elev e on e.ID=t.LElev left join Invoice i on dp.Invoice = i.Ref"; //assigned NOT IN ( 0 )
                    if (objPropMapData.EN == 1) // check for company
                    {
                        str += " LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN ";
                    }
                    str += " WHERE t.id is not null and t.owner is not null  ";
                    #region FILTER FOR Ticket O

                    if (objPropMapData.IsList != 1)
                    {
                        str += " and assigned NOT INii ( 0 )";
                    }
                    if (objPropMapData.EN == 1) // check for company
                    {
                        str += " and UC.IsSel = 1 and UC.UserID =" + objPropMapData.UserID;
                    }
                    if (objPropMapData.Assigned != -1)//&& objPropMapData.Assigned != 0
                    {
                        if (objPropMapData.Assigned == -2)
                        {
                            str += " and t.Assigned <> 4";
                        }
                        else
                        {
                            str += " and t.Assigned=" + objPropMapData.Assigned;
                        }
                    }
                    if (!isFilterHasCommaDelimited && objPropMapData.InvoiceDate == null)
                    {
                        if (objPropMapData.StartDate != System.DateTime.MinValue)
                        {
                            str += " and isnull(t.edate,t.cdate) >='" + objPropMapData.StartDate.ToString("MM/dd/yyyy") + "'";
                        }
                        if (objPropMapData.EndDate != System.DateTime.MinValue)
                        {
                            str += " and isnull(t.edate,t.cdate) <'" + objPropMapData.EndDate.AddDays(1).ToString("MM/dd/yyyy") + "'";
                        }
                    }

                    if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                    {

                        if (objPropMapData.Worker == "Active")
                        {
                            str += " and t.fwork in (select w.ID from tblWork w where  w.Status=0 )";
                        }
                        else if (objPropMapData.Worker == "Inactive")
                        {
                            str += " and t.fwork in (select w.ID from tblWork w where  w.Status=1 )";
                        }
                        else
                        {
                            str += " and (select w.fdesc from tblWork w where w.ID=t.fwork )='" + objPropMapData.Worker.Replace("'", "''") + "'";
                        }
                    }
                    if (objPropMapData.LocID != 0)
                    {
                        str += " and t.LID=" + objPropMapData.LocID;
                    }
                    if (objPropMapData.CustID != 0)
                    {
                        str += " and t.Owner=" + objPropMapData.CustID;
                    }
                    if (objPropMapData.jobid != 0)
                    {
                        str += " and t.job =" + objPropMapData.jobid;
                    }
                    if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
                    {
                        str += " and (isnull(t.charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge);
                        if (objPropMapData.FilterCharge == "1")
                            str += " or isnull(Invoice,0) <> 0)";
                        else
                            str += " and isnull(Invoice,0) = 0)";
                    }
                    if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                    {
                        str += " and (select Super from tblWork w where w.fdesc=t.dwork ) ='" + objPropMapData.Supervisor + "'";
                    }
                    if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
                    {
                        str += " and (select Super from tblWork w where w.fdesc=t.dwork ) <>'" + objPropMapData.NonSuper + "'";
                    }
                    //if (objPropMapData.FilterReview == "1")
                    //{
                    //    str += " and charge =9";                   
                    //}                
                   
                    if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
                    {
                        str += " and t.workorder='" + objPropMapData.Workorder + "'";
                    }
                    if (objPropMapData.Department != -1)
                    {
                        str += " and t.type=" + objPropMapData.Department;
                    }
                    if (objPropMapData.Category != string.Empty && objPropMapData.Category != null)
                    {
                        //str += " and t.cat='" + objPropMapData.Category + "'";
                        str += " and t.cat in (" + objPropMapData.Category + ")";
                    }
                    #region ////Filter for ticketo IF ticket Create from Portal
                    if (objPropMapData.IsPortal != string.Empty && objPropMapData.IsPortal != null)
                    {
                        if (objPropMapData.IsPortal == "1")
                        {
                            str += " and t.fBy='portal' ";
                        }
                        if (objPropMapData.IsPortal == "0")
                        {
                            str += " and t.fBy <> 'portal' ";
                        }
                    }
                    #endregion

                    if (objPropMapData.Bremarks != null && objPropMapData.Bremarks != string.Empty)
                    {
                        if (objPropMapData.Bremarks == "1")
                            str += " and isnull( rtrim(ltrim(t.bremarks)),'')<>''";
                        else
                            str += " and isnull( rtrim(ltrim(t.bremarks)),'')=''";
                    }

                    if (objPropMapData.SearchBy != "" && objPropMapData.SearchBy != null && !string.IsNullOrWhiteSpace(objPropMapData.SearchValue))
                    {
                        string SearchBy = objPropMapData.SearchBy.Replace("t.descres", "dp.descres");
                        if (SearchBy == "t.ID")
                        {
                            str += " and " + SearchBy + " = '" + objPropMapData.SearchValue + "'";
                        }
                        else
                        {
                            if (SearchBy == "t.Level")
                                str += " and " + SearchBy + " like '%" + objPropMapData.SearchValue.Replace("'", "") + "%'";
                            else
                                str += " and " + SearchBy + " like '%" + objPropMapData.SearchValue + "%'";

                        }
                    }

                    if (objPropMapData.InvoiceID != 0)
                    {
                        if (objPropMapData.InvoiceID == 1)
                        {
                            str += " and isnull(Invoice,0) <> 0";
                        }
                        else if (objPropMapData.InvoiceID == 2)
                        {
                            str += " and isnull(Invoice,0) = 0 and isnull(t.charge,0)= 1";
                        }
                    }
                    //  If the user is a salesperson or Second salesperson only show assigned  locations,Ticket.
                    if (IsSalesAsigned > 0)
                    {
                        str += " AND  (  l.Terr=(" + "select id FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSalesAsigned + ")) or  isnull(l.Terr2,0)=(" + "select id FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSalesAsigned + "))) ";
                    }

                    if (filters.Count > 0)
                    {
                        foreach (var items in filters)
                        {
                            if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                            {
                                /// Int Filter
                                int FilterValue = 0; string[] filterArrayValue;
                                StringBuilder filteredQuery = new StringBuilder();
                                if (items.FilterColumn == "ID")
                                {
                                    filterArrayValue = items.FilterValue.ToString().Split(',');
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
                                    str += " and t.ID  in (" + filteredQuery.ToString() + ")";
                                }

                                if (items.FilterColumn == "Job" & int.TryParse(items.FilterValue, out FilterValue))
                                {
                                    str += " and t.job  = " + FilterValue;
                                }

                                /// string Filter

                                if (items.FilterColumn == "WorkOrder")
                                {
                                    str += " and t.WorkOrder  = '" + items.FilterValue + "'";
                                }


                                if (items.FilterColumn == "locname")
                                {
                                    str += " and l.Tag  like '%" + items.FilterValue + "%'";
                                }
                                if (items.FilterColumn == "City")
                                {
                                    str += " and l.City  like '%" + items.FilterValue + "%'";
                                }
                                if (items.FilterColumn == "cat")
                                {
                                    str += " and t.cat  like '%" + items.FilterValue + "%'";
                                }

                                if (items.FilterColumn == "Name")
                                {
                                    str += " and ro.Name  like '%" + items.FilterValue + "%'";
                                }
                                if (items.FilterColumn == "fullAddress")
                                {
                                    str += " and ( l.address + ', ' + l.city + ', ' + l.state + ', ' + l.zip )  like '%" + items.FilterValue + "%'";
                                }

                            }
                        }
                    }
                    #endregion End Filter For TicketO

                    #region LocID == 0

                    if (objPropMapData.LocID == 0)
                    {
                        str += " Union all ";
                        str += " SELECT t.who,t.CPhone, t.lid, '--' as locid, assigned, ( r.address + ', ' + r.city + ', ' + r.state + ', ' + r.zip ) AS fulladdress,r.city, t.WorkOrder, dp.Reg, dp.OT, dp.NT, dp.DT, dp.TT,dp.break_time as BT, dp.Total, 0 AS ClearCheck, t.charge, t.fDesc, t.TimeRoute, t.TimeSite, t.TimeComp, CASE WHEN EXISTS (SELECT 1 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS comp, UPPER(dwork) AS dwork,dwork as lastname,(select isnull(hourlyrate,0) from tblwork where id=t.fwork) as hourlyrate, t.ID, r.Name AS customername,  r.EN, isnull(B.Name, '') As Company,  dp.PartsUsed,";
                        str += " r.name  AS locname, l.Type as LocType, l.Custom1 as LocCustom1, l.Custom2 as LocCustom2, l.Address  AS address, (select top 1 phone from rol lr where lr.id = l.rol) phone, t.Cat, t.EDate AS edate, t.CDate, dp.descres, CASE WHEN assigned = 0 THEN 'Un-Assigned' WHEN assigned = 1 THEN 'Assigned' WHEN assigned = 2 THEN 'Enroute' WHEN assigned = 3 THEN 'Onsite' WHEN assigned = 4 THEN 'Completed' WHEN assigned = 5 THEN 'Hold' END AS assignname, t.Est, Isnull(dp.Total, 0.00) AS Tottime ,";

                        #region 24 Hours Feature

                        //  str += "   round(convert(numeric(30,2),(Isnull(dp.Total, 0.00) - (convert(float,DATEDIFF(MILLISECOND,dp.TimeRoute,dp.TimeComp))/1000/60/60))),1) as timediff, ";

                        str += @" Round(CONVERT(NUMERIC(30, 2), ( Isnull(dp.Total, 0.00) - ( CONVERT(FLOAT, Datediff(MILLISECOND, dp.TimeRoute, ( CASE
                                                                                                                   WHEN ( Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME)
                                                                                                                           AND Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME) ) THEN Dateadd(day, 2, dp.TimeComp)
                                                                                                                   ELSE (( CASE
                                                                                                                             WHEN ( Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME)
                                                                                                                                    OR Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME) ) THEN Dateadd(day, 1, dp.TimeComp)
                                                                                                                             ELSE dp.TimeComp
                                                                                                                           END ))
                                                                                                                 END ))) / 1000 / 60 / 60 ) )), 1)  AS timediff,";

                        #endregion

                        str += "   t.workorder as WO, (isnull(dp.zone,0)+ isnull(dp.toll,0) + isnull(dp.othere,0)) as expenses, isnull( dp.zone,0) as zone, isnull( dp.toll,0) as toll , isnull(dp.othere,0) as othere, case ISNUMERIC(dbo.udf_GetNumeric(dp.custom2)) when 1 then CONVERT(money ,dbo.udf_GetNumeric(dp.custom2)) else 0 end as extraexp, ((isnull(dp.emile,0)-isnull(dp.smile,0))*0.26) as mileagetravel, ";


                        str += " (isnull(dp.emile,0)-isnull(dp.smile,0)) as mileage ,(select top 1 count(1) as signatureCount  from pdaticketsignature where pdaticketid=t.ID ) as signatureCount, (select count(1) from documents where screen='Ticket' and screenid=t.id) as DocumentCount, t.fwork as workerid, (select top 1 * from dbo.split(descres,'|')) as description, (select top 1 * from dbo.split(t.fdesc,'|')) as fdescreason, 0 as invoice, isnull(Confirmed,0) as Confirmed, '' as manualinvoice, '' as invoiceno, t.owner as ownerid, '' as QBinvoiceid, 0 as TransferTime, ";
                        str += "  (select name from inv where qbinvid= t.QBserviceitem) as serviceitem, ";
                        str += "  (select fdesc from prwage where QBwageID= t.QBPayrollItem) as PayrollItem, ";
                        str += "  (ISNULL( dp.reg ,0) + ISNULL( dp.OT ,0) +ISNULL( dp.TT ,0))     as RTOTTT ";
                        str += "  ,(select top 1 (last+', '+ffirst) from emp em where em.callsign = t.dwork) as WorkerLastName ";

                        if (objPropMapData.StartDate != System.DateTime.MinValue || objPropMapData.EndDate != System.DateTime.MinValue)
                        {
                            str += " ,(SELECT top 1 Signature	FROM   PDATimeSign p	WHERE  p.fWork = t.fwork and (p.EDate BETWEEN '" + objPropMapData.StartDate + "' and '" + objPropMapData.EndDate + "')	ORDER  BY EDate DESC ) as timesign";
                        }

                        str += ", 0 as dispalert, 0 as credithold, 0 as high,e.id as unitid, dbo.TicketEquips(t.ID) as unit, dbo.TicketEquipsColumns(t.ID, 'type') as unittype, '' as defaultworker, '' as defaultmech";
                        str += ", (select type from jobtype where id = t.type) as department,  rtrim(ltrim(t.bremarks)) as bremarks, 0 as laborexp ";
                        str += ", (select top 1 signature  from pdaticketsignature where pdaticketid=t.ID ) as signature , l.state, l.zip ";
                        str += ",  (select top 1 isnull( Rate,0)  from PROther where Emp = (select top 1 id from emp where fWork = t.fwork) and Cat = 5) as mileagepr ";
                        str += ", dbo.Afterhours(t.timeroute,t.timecomp) as afterhours, dbo.weekends(t.edate) as weekends, isnull(EmailNotified,0) as EmailNotified, EmailTime";
                        if (IsCallForTicketReport > 0)
                        {
                            // Show OtherWorker For  same date , same workorder, same location and same project
                            str += @", ([dbo].Getotherworker(t.fWork,t.WorkOrder,t.LID,t.Job,t.EDate)) AS OtherWorker , CAST(lt.ThirdPartyName + '  ' + lt.ThirdPartyPhone as VARCHAR(1000)) AS ThirdPartyInfo";
                        }
                        str += ",   t.Job ";
                        str += "       , (select convert(varchar(20),t.Job )+'-'+ fdesc from job where ID = t.Job) as ProjectDescription ";
                        str += ",   t.Custom6, t.Custom7, t.CustomTick1, t.CustomTick5 ";
                        str += ",   j.Certified ";
                        str += ",j.SRemarks as SpecialRemark ";

                        str += ", t.fBy,isNUll(( Select type  from JobType where id = ( select top 1 department from  tbljoinempdepartment where emp = (select top 1 ID from  emp where callsign = (select  top 1 fDesc from tblWork where id = t.fwork)))),'unassigned') as EmpDepartment ";
                        str += ",(select fDesc from PRWage where id = dp.WageC) as WageCategory,'','',i.fDate as InvoiceDate  ";
                        str += " FROM ticketo t Left Join LoadTestItem fh on t.ID =fh.Ticket LEFT OUTER JOIN LoadTestItemHistory lh ON t.ID = lh.TicketID LEFT OUTER JOIN LoadTestItemHistoryPrice lt ON fh.LID=lt.LID LEFT OUTER JOIN TicketDPDA dp ON t.ID = dp.ID LEFT JOIN Job j ON j.ID = t.Job inner join Loc l on l.Loc = t.lid left join Route ro on ro.ID = l.Route INNER JOIN Rol r ON r.ID = l.Rol LEFT OUTER JOIN Branch B on B.ID = r.EN  left outer join Elev e on e.ID=t.LElev left join Invoice i on dp.Invoice = i.Ref  ";
                        if (objPropMapData.EN == 1) // check for company
                        {
                            str += " LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN ";
                        }
                        str += " WHERE t.id is not null and t.owner is null and t.LType=1  ";

                        #region FILTER FOR TICKET_DPDA
                        if (objPropMapData.IsList != 1)
                        {
                            str += " and assigned NOT IN ( 0 )";
                        }
                        if (objPropMapData.EN == 1) // check for company
                        {
                            str += " and UC.IsSel = 1 and UC.UserID =" + objPropMapData.UserID;
                        }
                        if (objPropMapData.Assigned != -1)
                        {
                            if (objPropMapData.Assigned == -2)
                            {
                                str += " and t.Assigned <> 4";
                            }
                            else
                            {
                                str += " and t.Assigned=" + objPropMapData.Assigned;
                            }
                        }
                        if (!isFilterHasCommaDelimited && objPropMapData.InvoiceDate == null)
                        {
                            if (objPropMapData.StartDate != System.DateTime.MinValue)
                            {
                                str += " and isnull(t.edate,t.cdate) >='" + objPropMapData.StartDate.ToString("MM/dd/yyyy") + "'";
                            }
                            if (objPropMapData.EndDate != System.DateTime.MinValue)
                            {
                                str += " and isnull(t.edate,t.cdate) <'" + objPropMapData.EndDate.AddDays(1).ToString("MM/dd/yyyy") + "'";
                            }
                        }
                        if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                        {

                            if (objPropMapData.Worker == "Active")
                            {
                                str += " and t.fwork in (select w.ID from tblWork w where  w.Status=0 )";
                            }
                            else if (objPropMapData.Worker == "Inactive")
                            {
                                str += " and t.fwork in (select w.ID from tblWork w where  w.Status=1 )";
                            }
                            else
                            {
                                str += " and (select w.fdesc from tblWork w where w.ID=t.fwork )='" + objPropMapData.Worker.Replace("'", "''") + "'";
                            }
                        }
                        if (objPropMapData.LocID != 0)
                        {
                            str += " and t.LID=" + objPropMapData.LocID;
                        }
                        if (objPropMapData.CustID != 0)
                        {
                            str += " and t.Owner=" + objPropMapData.CustID;
                        }
                        if (objPropMapData.jobid != 0)
                        {
                            str += " and t.job =" + objPropMapData.jobid;
                        }
                        if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
                        {
                            str += " and (isnull(t.charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge);
                            if (objPropMapData.FilterCharge == "1")
                                str += " or isnull(Invoice,0) <> 0)";
                            else
                                str += " and isnull(Invoice,0) = 0)";
                        }
                        if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                        {
                            str += " and (select Super from tblWork w where w.fdesc=t.dwork ) ='" + objPropMapData.Supervisor + "'";
                        }
                        if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
                        {
                            str += " and (select Super from tblWork w where w.fdesc=t.dwork ) <>'" + objPropMapData.NonSuper + "'";
                        }
                        
                        if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
                        {
                            str += " and t.workorder='" + objPropMapData.Workorder + "'";
                        }
                        if (objPropMapData.Department != -1)
                        {
                            str += " and t.type=" + objPropMapData.Department;
                        }
                        if (objPropMapData.Category != string.Empty && objPropMapData.Category != null)
                        {
                            //str += " and t.cat='" + objPropMapData.Category + "'";
                            str += " and t.cat in (" + objPropMapData.Category + ")";
                        }
                        #region ////Filter for TicketDPDA IF Create from Portal
                        if (objPropMapData.IsPortal != string.Empty && objPropMapData.IsPortal != null)
                        {
                            if (objPropMapData.IsPortal == "1")
                            {
                                str += " and t.fBy='portal' ";
                            }
                            if (objPropMapData.IsPortal == "0")
                            {
                                str += " and t.fBy <> 'portal' ";
                            }
                        }
                        #endregion

                        if (objPropMapData.Bremarks != null && objPropMapData.Bremarks != string.Empty)
                        {
                            if (objPropMapData.Bremarks == "1")
                                str += " and isnull( rtrim(ltrim(t.bremarks)),'')<>''";
                            else
                                str += " and isnull( rtrim(ltrim(t.bremarks)),'')=''";
                        }
                        if (objPropMapData.SearchBy != "" && objPropMapData.SearchBy != null && !string.IsNullOrWhiteSpace(objPropMapData.SearchValue))
                        {
                            string SearchBy = objPropMapData.SearchBy.Replace("t.descres", "dp.descres");
                            if (SearchBy == "t.ID")
                            {
                                //str += " and " + SearchBy + " in '" + objPropMapData.SearchValue + "'";
                                str += " and " + SearchBy + " in (" + objPropMapData.SearchValue + ")";
                            }
                            else
                            {
                                if (SearchBy == "l.tag")
                                    SearchBy = "r.name";
                                if (SearchBy == "t.Level")
                                    str += " and " + SearchBy + " like '%" + objPropMapData.SearchValue.Replace("'", "") + "%'";
                                else
                                    str += " and " + SearchBy + " like '%" + objPropMapData.SearchValue + "%'";
                            }
                        }
                        if (objPropMapData.InvoiceID != 0)
                        {
                            if (objPropMapData.InvoiceID == 1)
                            {
                                str += " and isnull(Invoice,0) <> 0";
                            }
                            else if (objPropMapData.InvoiceID == 2)
                            {
                                str += " and isnull(Invoice,0) = 0 and isnull(t.charge,0)= 1";
                            }
                        }
                        //  If the user is a salesperson only show assigned  locations,Ticket.
                        if (IsSalesAsigned > 0)
                        {
                            str += " AND  ( l.Terr=(" + "select id FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSalesAsigned + ")) )";
                        }
                        if (filters.Count > 0)
                        {
                            foreach (var items in filters)
                            {
                                if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                                {
                                    /// Int Filter
                                    int FilterValue = 0;
                                    //if (items.FilterColumn == "ID" & int.TryParse(items.FilterValue, out FilterValue))
                                    //{
                                    //    str += "\n  and t.ID  = " + FilterValue;
                                    //}

                                    if (items.FilterColumn == "ID")
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
                                        str += "\n and t.ID  in (" + filteredQuery.ToString() + ")";
                                    }

                                    if (items.FilterColumn == "Job" & int.TryParse(items.FilterValue, out FilterValue))
                                    {
                                        str += "\n  and t.job  = " + FilterValue;
                                    }

                                    /// string Filter

                                    if (items.FilterColumn == "WorkOrder")
                                    {
                                        str += "\n  and t.WorkOrder  = '" + items.FilterValue + "'";
                                    }

                                    if (items.FilterColumn == "locname")
                                    {
                                        str += "\n  and r.name  like '%" + items.FilterValue + "%'";
                                    }
                                    if (items.FilterColumn == "City")
                                    {
                                        str += "\n  and r.City  like '%" + items.FilterValue + "%'";
                                    }
                                    if (items.FilterColumn == "fullAddress")
                                    {
                                        str += "\n  and ( r.address + ', ' + r.city + ', ' + r.state + ', ' + r.zip )  like '%" + items.FilterValue + "%'";
                                    }

                                }
                            }
                        }

                        #endregion FILTER FOR TICKET_DPDA
                    }

                    #endregion LocID == 0

                }

                #endregion FilterReview != "1"
            }

            #endregion Status != Assigned

            #region Status=Completed/ALL
            if (objPropMapData.Assigned == 4 || objPropMapData.Assigned == -1)//|| objPropMapData.Assigned == 0
            {
           
                {
                    //if (objPropMapData.Mobile == 0)
                    //{
                    if (objPropMapData.Status != 1)
                    {
                        if (objPropMapData.FilterReview != "1")
                        {
                            str += " UNION ALL";
                        }
                    }
                    //}
                    //if (objPropMapData.Mobile != 2)
                    //{
                    str += " SELECT t.who,t.CPhone, t.loc as lid, l.id as locid, 4 as assigned, (l.address+', '+l.city+', '+l.state+', '+l.zip) as fulladdress,l.city, t.WorkOrder, t.Reg, t.OT, t.NT, t.DT, t.TT, break_time as BT, t.Total,isnull( ClearCheck ,0) as ClearCheck ,t.charge, t.fdesc,timeroute, timesite,timecomp, 1 as comp, (select UPPER(w.fdesc) from tblWork w where t.fwork = w.id) AS dwork,(select w.fdesc from tblWork w where t.fwork = w.id) as lastname,(select isnull(hourlyrate,0) from tblwork where id=t.fwork) as hourlyrate, t.id,  r.Name  AS customername, r.EN, isnull(B.Name, '') As Company, t.PartsUsed, l.tag AS locname, l.Type as LocType, l.Custom1 as LocCustom1, l.Custom2 as LocCustom2, l.address,r.phone,  t.cat, edate, cdate, descres, 'Completed' AS assignname, est,t.Total as tottime , ";

                    //  str += "   round(convert(numeric(30,2),(Isnull(dp.Total, 0.00) - (convert(float,DATEDIFF(MILLISECOND,TimeRoute,TimeComp))/1000/60/60))),1) as timediff, ";


                    #region  24 HOURS FEATURE
                    str += @" Round(CONVERT(NUMERIC(30, 2), ( Isnull(t.Total, 0.00) - ( CONVERT(FLOAT, Datediff(MILLISECOND, TimeRoute, ( CASE
                                                                                                                   WHEN(Cast(TimeSite AS TIME) < Cast(TimeRoute AS TIME)
                                                                                                                           AND Cast(TimeComp AS TIME) < Cast(TimeSite AS TIME)) THEN Dateadd(day, 2, TimeComp)
                                                                                                                   ELSE((CASE
                                                                                                                             WHEN(Cast(TimeSite AS TIME) < Cast(TimeRoute AS TIME)
                                                                                                                                    OR Cast(TimeComp AS TIME) < Cast(TimeSite AS TIME)) THEN Dateadd(day, 1, TimeComp)
                                                                                                                             ELSE TimeComp
                                                                                                                           END))
                                                                                                                 END ))) / 1000 / 60 / 60 ) )), 1)  AS timediff ,";
                    #endregion

                    str += "   t.workorder as WO, (isnull(t.zone,0)+ isnull(t.toll,0) + isnull(t.othere,0)) as expenses, isnull( t.zone,0) as zone, isnull( t.toll,0) as toll , isnull(t.othere,0) as othere, case ISNUMERIC(dbo.udf_GetNumeric(t.custom2)) when 1 then CONVERT(money ,dbo.udf_GetNumeric(t.custom2)) else 0 end as extraexp, ((isnull(t.emile,0)-isnull(t.smile,0))*0.26) as mileagetravel, (isnull(t.emile,0)-isnull(t.smile,0)) as mileage, (select top 1 count(1) as signatureCount  from pdaticketsignature where pdaticketid=t.ID ) as signatureCount,  (select count(1) from documents where screen='Ticket' and screenid=t.id) as DocumentCount, t.fwork as workerid, (select top 1 * from dbo.split(descres,'|')) as description,(select top 1 * from dbo.split(t.fdesc,'|')) as fdescreason, isnull(invoice,0) as invoice, 0 as Confirmed,  manualinvoice, case  when ( Isnull(invoice, 0) =  0 ) then Manualinvoice else CONVERT(varchar(50), Invoice) end as invoiceno, 0 as ownerid, isnull(t.QBinvoiceID,'')as QBinvoiceid, isnull(TransferTime,0) as TransferTime, ";
                    str += "  (select name from inv where qbinvid= t.QBserviceitem) as serviceitem, ";
                    str += "  (select fdesc from prwage where QBwageID= t.QBPayrollItem) as PayrollItem, ";
                    str += "  (ISNULL( t.reg ,0) + ISNULL( t.OT ,0) +ISNULL( t.TT ,0))     as RTOTTT ";
                    str += "  ,(select top 1 (last+', '+ffirst) from emp em where em.callsign =(select top 1 fdesc from tblwork wo where wo.id = t.fwork)) as WorkerLastName ";
                    if (objPropMapData.StartDate != System.DateTime.MinValue || objPropMapData.EndDate != System.DateTime.MinValue)
                    {
                        str += " , (SELECT top 1 Signature	FROM   PDATimeSign p	WHERE  p.fWork = t.fwork and (p.EDate BETWEEN '" + objPropMapData.StartDate + "' and '" + objPropMapData.EndDate + "')	ORDER  BY EDate DESC ) as timesign";
                    }
                    str += " , isnull(l.dispalert,0)as dispalert, isnull(l.credit,0)as credithold, 0 as high ,e.id as unitid , dbo.TicketEquips(t.ID) as unit, dbo.TicketEquipsColumns(t.ID, 'type') as unittype, (select Name from Route where ID=l.Route ) as defaultworker, (select top 1 fdesc from tblwork where id = (select top 1 mech from Route where ID=l.Route )) as defaultmech";
                    str += " , (select type from jobtype where id = t.type) as department, rtrim(ltrim(t.bremarks)) as bremarks ";
                    //str += ", CONVERT(NUMERIC(30, 2),(((isnull(t.Reg,0) + isnull(t.RegTrav,0)) +  ((isnull(t.OT,0) + isnull(t.OTTrav,0)) * 1.5) + ((isnull(t.DT,0) + isnull(t.DTTrav,0)) * 2) + ((isnull(t.NT,0) + isnull(t.NTTrav,0)) * 1.7) + (isnull(t.TT,0))) * (SELECT Isnull(w.HourlyRate, 0)FROM   tblWork w WHERE  w.ID = t.fWork))) AS LaborExp ";

                    // str += " , (select sum( isnull(Amount ,0))from jobi where Labor = 1 and TransID<0 and ref = t.id group by Ref) as laborexp";

                    str += @", (SELECT Isnull(Amount, 0)   FROM   #jobitemptable   WHERE  ref = t.id)   AS laborexp";

                    str += " , (select top 1 signature from pdaticketsignature where pdaticketid=t.ID ) as signature , l.state, l.zip ";
                    str += " ,  (select top 1 isnull( Rate,0)  from PROther where Emp = (select top 1 id from emp where fWork = t.fwork) and Cat = 5) as mileagepr ";
                    str += " , dbo.Afterhours(t.timeroute,t.timecomp) as afterhours, dbo.weekends(t.edate) as weekends, 0 as EmailNotified, null as EmailTime";
                    if (IsCallForTicketReport > 0)
                    {
                        // Show OtherWorker For  same date , same workorder, same location and same project
                        str += @",  ([dbo].Getotherworker(t.fWork,t.WorkOrder,t.Loc,t.Job,t.EDate)) AS OtherWorker , CAST(lt.ThirdPartyName + '  ' + lt.ThirdPartyPhone as VARCHAR(1000)) AS ThirdPartyInfo";
                    }
                    str += ",   t.Job ";
                    str += "       , (select convert(varchar(20),t.Job )+'-'+ fdesc from job where ID = t.Job) as ProjectDescription ";
                    str += ",   t.Custom6, t.Custom7, t.CustomTick1, t.CustomTick5 ";
                    str += ",   j.Certified ";
                    str += ",j.SRemarks as SpecialRemark";
                    str += ", t.fBy ,isNUll(( Select type  from JobType where id = ( select top 1 department from  tbljoinempdepartment where emp = (select top 1 ID from  emp where callsign = (select  top 1 fDesc from tblWork where id = t.fwork)))),'unassigned') as EmpDepartment ";
                    str += ",(select fDesc from PRWage where id = t.WageC) as WageCategory,l.Custom14,l.Custom15,i.fDate as InvoiceDate ";
                    str += " FROM ticketd t Left Join LoadTestItem fh on t.ID =fh.Ticket LEFT OUTER JOIN LoadTestItemHistory lh ON t.ID = lh.TicketID LEFT OUTER JOIN LoadTestItemHistoryPrice lt ON fh.LID=lt.LID LEFT JOIN Job j ON j.ID = t.Job INNER JOIN loc l ON l.loc = t.loc LEFT JOIN Route ro on ro.ID = l.Route inner join Owner o on l.Owner=o.ID INNER JOIN Rol r ON r.ID = o.Rol LEFT OUTER JOIN Branch B on B.ID = r.EN   left outer join Elev e on e.ID=t.Elev left outer join Invoice i on t.Invoice = i.Ref  ";
                    if (objPropMapData.EN == 1) // check for company
                    {
                        str += " LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN ";
                    }
                    str += " WHERE t.id is not null  ";
                    #region FILTER FOR TICKETD
                    if (!isFilterHasCommaDelimited && objPropMapData.InvoiceDate == null)
                    {
                        if (objPropMapData.StartDate != System.DateTime.MinValue)
                        {
                            str += " and edate >='" + objPropMapData.StartDate.ToString("MM/dd/yyyy") + "'";
                        }
                        if (objPropMapData.EndDate != System.DateTime.MinValue)
                        {
                            str += " and edate <'" + objPropMapData.EndDate.AddDays(1).ToString("MM/dd/yyyy") + "'";
                        }
                    }
                    if (objPropMapData.EN == 1) // check for company
                    {
                        str += " and UC.IsSel = 1 and UC.UserID =" + objPropMapData.UserID;
                    }
                    if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                    {

                        if (objPropMapData.Worker == "Active")
                        {
                            str += " and t.fwork in (select w.ID from tblWork w where  w.Status=0 )";
                        }
                        else if (objPropMapData.Worker == "Inactive")
                        {
                            str += " and t.fwork in (select w.ID from tblWork w where  w.Status=1 )";
                        }
                        else
                        {
                            str += " and (select w.fdesc from tblWork w where w.ID=t.fwork )='" + objPropMapData.Worker.Replace("'", "''") + "'";
                        }
                    }
                    if (objPropMapData.LocID != 0)
                    {
                        str += " and l.loc=" + objPropMapData.LocID;
                    }
                    if (objPropMapData.CustID != 0)
                    {
                        str += " and l.Owner=" + objPropMapData.CustID;
                    }
                    if (objPropMapData.jobid != 0)
                    {
                        str += " and t.job =" + objPropMapData.jobid;
                    }
                    if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
                    {
                        str += " and (isnull(t.charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge);
                        if (objPropMapData.FilterCharge == "1")
                            str += " or isnull(Invoice,0) <> 0)";
                        else
                            str += " and isnull(Invoice,0) = 0)";
                    }
                    if (objPropMapData.Timesheet != null && objPropMapData.Timesheet != string.Empty)
                    {
                        str += " and isnull(TransferTime,0)=" + Convert.ToInt32(objPropMapData.Timesheet);
                    }
                    if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                    {
                        str += " and (select Super from tblWork w where w.ID=t.fwork ) ='" + objPropMapData.Supervisor + "'";
                    }
                    if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
                    {
                        str += " and (select Super from tblWork w where w.ID=t.fwork ) <>'" + objPropMapData.NonSuper + "'";
                    }
                    if (objPropMapData.FilterReview != null && objPropMapData.FilterReview != string.Empty)
                    {
                        str += " and isnull( ClearCheck ,0) =" + Convert.ToInt32(objPropMapData.FilterReview);
                    }
                    if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
                    {
                        str += " and t.workorder='" + objPropMapData.Workorder + "'";
                    }
                    if (objPropMapData.Department != -1)
                    {
                        str += " and t.type=" + objPropMapData.Department;
                    }
                    if (objPropMapData.Category != string.Empty && objPropMapData.Category != null)
                    {
                        //str += " and t.cat='" + objPropMapData.Category + "'";
                        str += " and t.cat in (" + objPropMapData.Category + ")";
                    }
                    #region ////Filter For TicketD IF Create from Portal
                    if (objPropMapData.IsPortal != string.Empty && objPropMapData.IsPortal != null)
                    {
                        if (objPropMapData.IsPortal == "1")
                        {
                            str += " and t.fBy='portal' ";
                        }
                        if (objPropMapData.IsPortal == "0")
                        {
                            str += " and t.fBy <> 'portal' ";
                        }
                    }
                    #endregion
                    if (objPropMapData.Bremarks != null && objPropMapData.Bremarks != string.Empty)
                    {
                        if (objPropMapData.Bremarks == "1")
                            str += " and isnull( rtrim(ltrim(t.bremarks)),'')<>''";
                        else
                            str += " and isnull( rtrim(ltrim(t.bremarks)),'')=''";
                    }
                    if (objPropMapData.SearchBy != "" && objPropMapData.SearchBy != null && !string.IsNullOrWhiteSpace(objPropMapData.SearchValue))
                    {
                        if (objPropMapData.SearchBy == "t.ldesc4")
                        {
                            str += " and l.address like '%" + objPropMapData.SearchValue + "%'";
                        }
                        else if (objPropMapData.SearchBy == "t.ID")
                        {
                            //str += " and " + objPropMapData.SearchBy + " = '" + objPropMapData.SearchValue + "'";
                            str += " and " + objPropMapData.SearchBy + " in (" + objPropMapData.SearchValue + ")";
                        }
                        else
                        {
                            if (objPropMapData.SearchBy == "t.Level")
                                str += " and " + objPropMapData.SearchBy + " like '%" + objPropMapData.SearchValue.Replace("'", "") + "%'";
                            else
                                str += " and " + objPropMapData.SearchBy + " like '%" + objPropMapData.SearchValue + "%'";
                        }
                    }
                    if (objPropMapData.Status == 1)
                    {
                        str += " and isnull( t.status ,0) <> 1 and isnull(t.internet,0) = 1";
                    }
                    //if (!string.IsNullOrEmpty( objPropMapData.LocIDs ))
                    //{
                    //    str += " and l.loc in (" + objPropMapData.LocIDs + ") ";
                    //}
                    if (objPropMapData.RoleID != 0)
                        str += " and isnull(l.roleid,0)=" + objPropMapData.RoleID;

                    //}
                    if (objPropMapData.InvoiceID != 0)
                    {
                        if (objPropMapData.InvoiceID == 1)
                        {
                            str += " and (isnull(Invoice,0) <> 0  or manualinvoice <> '')";
                        }
                        else if (objPropMapData.InvoiceID == 2)
                        {
                            str += " and (isnull(Invoice,0) = 0 or manualinvoice = '') and isnull(t.charge,0)= 1";
                        }
                    }
                    //  If the user is a salesperson only show assigned  locations,Ticket.
                    if (IsSalesAsigned > 0)
                    {
                        str += " AND   (l.Terr=(" + "select id FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSalesAsigned + ")) or isnull(l.Terr2,0)=(" + "select id FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSalesAsigned + ")))";
                    }

                    if (filters.Count > 0)
                    {
                        foreach (var items in filters)
                        {
                            if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                            {
                                /// Int Filter
                                int FilterValue = 0;
                                //if (items.FilterColumn == "ID" & int.TryParse(items.FilterValue, out FilterValue))
                                //{
                                //    str += "\n and t.ID  = " + FilterValue;
                                //}

                                if (items.FilterColumn == "ID")
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
                                    str += "\n and t.ID  in (" + filteredQuery.ToString() + ")";
                                }

                                if (items.FilterColumn == "Job" & int.TryParse(items.FilterValue, out FilterValue))
                                {
                                    str += "\n and t.job  = " + FilterValue;
                                }

                                /// string Filter

                                if (items.FilterColumn == "WorkOrder")
                                {
                                    str += "\n and t.WorkOrder  = '" + items.FilterValue + "'";
                                }
                                if (items.FilterColumn == "locname")
                                {
                                    str += "\n and l.Tag  like '%" + items.FilterValue + "%'";
                                }
                                if (items.FilterColumn == "City")
                                {
                                    str += "\n and l.City  like '%" + items.FilterValue + "%'";
                                }
                                if (items.FilterColumn == "cat")
                                {
                                    str += "\n and t.cat  like '%" + items.FilterValue + "%'";
                                }
                                if (items.FilterColumn == "Name")
                                {
                                    str += "\n and ro.Name  like '%" + items.FilterValue + "%'";
                                }
                                if (items.FilterColumn == "fullAddress")
                                {
                                    str += "\n  and ( l.address + ', ' + l.city + ', ' + l.state + ', ' + l.zip )  like '%" + items.FilterValue + "%'";
                                }

                            }
                        }
                    }

                    #endregion FILTER FOR TICKETD
                }
            }
            #endregion Completed/ALL

            str += " ) xTicket WHERE 1=1";


            if (objPropMapData.Mobile == 2)
            {
                str += "\n and xTicket.fBy  = 'MOBILEUSER' ";
            }

            if (objPropMapData.Mobile == 1)
            {

                str += "\n and xTicket.fBy  <> 'MOBILEUSER' ";
            }

            if (objPropMapData.InvoiceDate == "Yes")
            {
                str += "\n AND xTicket.InvoiceDate >=CAST('" + objPropMapData.StartDate.ToString("MM/dd/yyyy") + "' as date) ";
                str += "AND xTicket.InvoiceDate <= cast('" + objPropMapData.EndDate.ToString("MM/dd/yyyy") + "' as date)";
            }

            if (filters.Count > 0)
            {
                foreach (var items in filters)
                {
                    if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                    {
                        /// Int Filter
                        int FilterValue = 0;
                        if (items.FilterColumn == "Tottime" & int.TryParse(items.FilterValue, out FilterValue))
                        {
                            str += "\n and xTicket.Tottime  = " + FilterValue;
                        }
                        if (items.FilterColumn == "invoiceno" & int.TryParse(items.FilterValue, out FilterValue))
                        {
                            str += "\n and xTicket.invoiceno  = " + FilterValue;
                        }
                        if (items.FilterColumn == "timediff" & int.TryParse(items.FilterValue, out FilterValue))
                        {
                            str += "\n and xTicket.timediff  = " + FilterValue;
                        }
                        /// string Filter

                        if (items.FilterColumn == "assignname")
                        {
                            str += "\n and xTicket.assignname  like '%" + items.FilterValue + "%'";
                        }

                        if (items.FilterColumn == "fulladdress")
                        {
                            str += "\n and xTicket.fulladdress  like '%" + items.FilterValue + "%'";
                        }
                        if (items.FilterColumn == "dwork")
                        {
                            str += "\n and xTicket.dwork  like '%" + items.FilterValue + "%'";
                        }
                        if (items.FilterColumn == "Company")
                        {
                            str += "\n and xTicket.Company  like '%" + items.FilterValue + "%'";
                        }
                        if (items.FilterColumn == "department")
                        {
                            str += "\n and xTicket.department  like '%" + items.FilterValue + "%'";
                        }

                        if (items.FilterColumn == "unit")
                        {
                            str += "\n and (1 = (Select 1 from multiple_equipments me WITH(NOLOCK)  inner join elev e WITH(NOLOCK) on e.id = me.elev_id where me.ticket_id = xTicket.ID and Unit like  '%" + items.FilterValue + "%'))";
                        }
                        if (items.FilterColumn == "InvoiceDate")
                        {
                            str += "\n AND xTicket.InvoiceDate >=CAST('" + items.FilterValue + "' as date) ";
                            str += "AND xTicket.InvoiceDate <= cast('" + items.FilterValue + "' as date)";
                        }
                    }
                }
            }

            str += "  IF OBJECT_ID('tempdb..#jobitemptable') IS NOT NULL   DROP TABLE #jobitemptable  ";

            try
            {
                objPropMapData.fBy = str;
                if (str != string.Empty && GetReportData == true)
                    return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, str);
                else
                    return objPropMapData.Ds = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ConnConfig"></param>
        /// <returns></returns>
        public DataSet GetTicketListReportDatabyQuery(string query, string ConnConfig)
        {

            try
            {
                if (query != string.Empty)
                    return SqlHelper.ExecuteDataset(ConnConfig, CommandType.Text, query);
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  UpdateReviewStatus
        /// </summary>
        /// <param name="objPropMapData"></param>
        public int UpdateReviewStatus(MapData objPropMapData)
        {
            int res = 0;
            SqlParameter[] para = new SqlParameter[6];
            para[0] = new SqlParameter
            {
                ParameterName = "Data",
                SqlDbType = SqlDbType.Structured,
                Value = objPropMapData.dtReview
            };

            para[1] = new SqlParameter
            {
                ParameterName = "department",
                SqlDbType = SqlDbType.Int,
                Value = objPropMapData.Department
            };

            para[2] = new SqlParameter
            {
                ParameterName = "QBpayroll",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropMapData.QBPayrollID
            };

            para[3] = new SqlParameter
            {
                ParameterName = "QBservice",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropMapData.QBServiceID
            };

            para[4] = new SqlParameter
            {
                ParameterName = "Payroll",
                SqlDbType = SqlDbType.Int,
                Value = objPropMapData.PayRoll
            };

            para[5] = new SqlParameter
            {
                ParameterName = "UpdateBy",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropMapData.LastUpdatedBy
            };



            try
            {
                res = Convert.ToInt16(SqlHelper.ExecuteScalar(objPropMapData.ConnConfig, CommandType.StoredProcedure, "spMassUpdateReview", para));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }

        /// <summary>
        ///  UpdateReviewStatus
        /// </summary>
        /// <param name="objPropMapData"></param>



        /// <summary>
        ///  UpdateReviewStatus
        /// </summary>
        /// <param name="objPropMapData"></param>
        public void VoidedTickets(string ConnConfig, int LocID, string UpdatedBy, int Tickets)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(ConnConfig, "spVoidedTicket", LocID, UpdatedBy, Tickets);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        #region Get  getCallHistory 
        public DataSet getCallHistory(MapData objPropMapData, Int32 IsSalesAsigned = 0, int IsCallForTicketReport = 0)
        {
            string str = "";

            str = @" IF OBJECT_ID('tempdb..#jobitemptable') IS NOT NULL  DROP TABLE #jobitemptable
                    CREATE TABLE #jobitemptable  (  Amount NUMERIC(30, 2),  ref  int primary key not null   )
                    INSERT INTO #jobitemptable SELECT Sum(Isnull(Amount, 0)) AS Amount,   ref FROM   jobi WHERE  Labor = 1 AND TransID < 0 GROUP  BY Ref   ";

            //if (objPropMapData.Mobile != 1)
            //{
            #region Status != Assigned
            if (objPropMapData.Status != 1)
            {
                #region FilterReview != "1"
                if (objPropMapData.FilterReview != "1")
                {
                    str += "  SELECT t.who,t.CPhone, t.lid, l.id as locid, assigned, ( l.address + ', ' + l.city + ', ' + l.state + ', ' + l.zip ) AS fulladdress,l.city, t.WorkOrder, dp.Reg, dp.OT, dp.NT, dp.DT, dp.TT,dp.break_time as BT, dp.Total, 0 AS ClearCheck, t.charge, t.fDesc, t.TimeRoute, t.TimeSite, t.TimeComp, CASE WHEN EXISTS (SELECT 1 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS comp, UPPER(dwork) AS dwork, dwork as lastname,(select isnull(hourlyrate,0) from tblwork where id=t.fwork) as hourlyrate, t.ID, r.Name AS customername, r.EN, isnull(B.Name, '') As Company,";
                    str += " l.Tag  AS locname, l.Address  AS address,r.phone, t.Cat, t.EDate AS edate, t.CDate, dp.descres, CASE WHEN assigned = 0 THEN 'Un-Assigned' WHEN assigned = 1 THEN 'Assigned' WHEN assigned = 2 THEN 'Enroute' WHEN assigned = 3 THEN 'Onsite' WHEN assigned = 4 THEN 'Completed' WHEN assigned = 5 THEN 'Hold' END AS assignname, t.Est, Isnull(dp.Total, 0.00) AS Tottime ,";



                    //,round(convert(numeric(30,2), (Isnull(dp.Total, 0.00) - (convert(float,DATEDIFF(MILLISECOND,dp.TimeRoute,dp.TimeComp))/1000/60/60))),1) as timediff


                    #region 24 HOURS FEATURE

                    str += @" Round(CONVERT(NUMERIC(30, 2), ( Isnull(dp.Total, 0.00) - ( CONVERT(FLOAT, Datediff(MILLISECOND, dp.TimeRoute, ( CASE
                                                                                                                   WHEN(Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME)
                                                                                                                           AND Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME)) THEN Dateadd(day, 2, dp.TimeComp)
                                                                                                                   ELSE((CASE
                                                                                                                             WHEN(Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME)
                                                                                                                                    OR Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME)) THEN Dateadd(day, 1, dp.TimeComp)
                                                                                                                             ELSE dp.TimeComp
                                                                                                                           END))
                                                                                                                 END ))) / 1000 / 60 / 60 ) )), 1)  AS timediff ";
                    #endregion


                    str += "  , t.workorder, (isnull(dp.zone,0)+ isnull(dp.toll,0) + isnull(dp.othere,0)) as expenses, isnull( dp.zone,0) as zone, isnull( dp.toll,0) as toll , isnull(dp.othere,0) as othere, case ISNUMERIC(dbo.udf_GetNumeric(dp.custom2)) when 1 then CONVERT(money ,dbo.udf_GetNumeric(dp.custom2)) else 0 end as extraexp, ((isnull(dp.emile,0)-isnull(dp.smile,0))*0.26) as mileagetravel, ";
                    str += " (isnull(dp.emile,0)-isnull(dp.smile,0)) as mileage ,(select top 1 count(1) as signatureCount  from pdaticketsignature where pdaticketid=t.ID ) as signatureCount, (select count(1) from documents where screen='Ticket' and screenid=t.id) as DocumentCount, t.fwork as workerid, (select top 1 * from dbo.split(descres,'|')) as description, (select top 1 * from dbo.split(t.fdesc,'|')) as fdescreason, 0 as invoice, isnull(Confirmed,0) as Confirmed, '' as manualinvoice, '' as invoiceno, t.owner as ownerid, '' as QBinvoiceid, 0 as TransferTime, ";
                    str += "  (select name from inv where qbinvid= t.QBserviceitem) as serviceitem, ";
                    str += "  (select fdesc from prwage where QBwageID= t.QBPayrollItem) as PayrollItem, ";
                    str += "  (ISNULL( dp.reg ,0) + ISNULL( dp.OT ,0) +ISNULL( dp.TT ,0))     as RTOTTT ";
                    str += "  ,(select top 1 (last+', '+ffirst) from emp em where em.callsign = t.dwork) as WorkerLastName ";
                    if (objPropMapData.StartDate != System.DateTime.MinValue || objPropMapData.EndDate != System.DateTime.MinValue)
                    {
                        str += " ,(SELECT top 1 Signature	FROM   PDATimeSign p	WHERE  p.fWork = t.fwork and (p.EDate BETWEEN '" + objPropMapData.StartDate + "' and '" + objPropMapData.EndDate + "')	ORDER  BY EDate DESC ) as timesign";
                    }
                    str += ", isnull(l.dispalert,0)as dispalert, isnull(l.credit,0)as credithold, isnull(t.high,0) as high,e.id as unitid, dbo.TicketEquips(t.ID) as unit, dbo.TicketEquipsColumns(t.ID, 'type') as unittype, (select Name from Route where ID=l.Route ) as defaultworker, (select top 1 fdesc from tblwork where id = (select top 1 mech from Route where ID=l.Route )) as defaultmech";
                    str += ", (select type from jobtype where id = t.type) as department,  rtrim(ltrim(t.bremarks)) as bremarks, 0 as laborexp ";
                    str += ", (select top 1 signature from pdaticketsignature where pdaticketid=t.ID ) as signature, l.state ";
                    str += ", (select top 1 isnull( Rate,0)  from PROther where Emp = (select top 1 id from emp where fWork = t.fwork) and Cat = 5) as mileagepr ";
                    str += ", dbo.Afterhours(t.timeroute,t.timecomp) as afterhours, dbo.weekends(t.edate) as weekends, isnull(EmailNotified,0) as EmailNotified, EmailTime";
                    if (IsCallForTicketReport > 0)
                    {
                        // Show OtherWorker For  same date , same workorder, same location and same project
                        str += @", ([dbo].Getotherworker(t.fWork,t.WorkOrder,t.LID,t.Job,t.EDate)) AS OtherWorker";
                    }
                    str += ",   t.Job";
                    str += "       , (select convert(varchar(20),t.Job )+'-'+ fdesc from job where ID = t.Job) as ProjectDescription ";
                    str += ", t.Custom6, t.Custom7 ";
                    str += ", t.fBy ";
                    str += ",(select fDesc from PRWage where id = dp.WageC) as WageCategory ";
                    str += " FROM ticketo t LEFT OUTER JOIN TicketDPDA dp ON t.ID = dp.ID inner join Loc l on l.Loc=t.lid  inner join Owner o on l.Owner=o.ID INNER JOIN Rol r ON r.ID = o.Rol LEFT OUTER JOIN Branch B on B.ID = r.EN  left outer join Elev e on e.ID=t.LElev "; //assigned NOT IN ( 0 )
                    if (objPropMapData.EN == 1) // check for company
                    {
                        str += " LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN ";
                    }
                    str += " WHERE t.id is not null and t.owner is not null  ";
                    #region FILTER FOR Ticket O

                    if (objPropMapData.IsList != 1)
                    {
                        str += " and assigned NOT IN ( 0 )";
                    }
                    if (objPropMapData.EN == 1) // check for company
                    {
                        str += " and UC.IsSel = 1 and UC.UserID =" + objPropMapData.UserID;
                    }
                    if (objPropMapData.Assigned != -1)//&& objPropMapData.Assigned != 0
                    {
                        if (objPropMapData.Assigned == -2)
                        {
                            str += " and t.Assigned <> 4";
                        }
                        else
                        {
                            str += " and t.Assigned=" + objPropMapData.Assigned;
                        }
                    }
                    if (objPropMapData.StartDate != System.DateTime.MinValue)
                    {
                        str += " and isnull(t.edate,t.cdate) >='" + objPropMapData.StartDate + "'";
                    }
                    if (objPropMapData.EndDate != System.DateTime.MinValue)
                    {
                        str += " and isnull(t.edate,t.cdate) <='" + objPropMapData.EndDate + "'";
                    }
                    if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                    {

                        if (objPropMapData.Worker == "Active")
                        {
                            str += " and t.fwork in (select w.ID from tblWork w where  w.Status=0 )";
                        }
                        else if (objPropMapData.Worker == "Inactive")
                        {
                            str += " and t.fwork in (select w.ID from tblWork w where  w.Status=1 )";
                        }
                        else
                        {
                            str += " and (select w.fdesc from tblWork w where w.ID=t.fwork )='" + objPropMapData.Worker.Replace("'", "''") + "'";
                        }
                    }
                    if (objPropMapData.LocID != 0)
                    {
                        str += " and t.LID=" + objPropMapData.LocID;
                    }
                    if (objPropMapData.CustID != 0)
                    {
                        str += " and t.Owner=" + objPropMapData.CustID;
                    }
                    if (objPropMapData.jobid != 0)
                    {
                        str += " and t.job =" + objPropMapData.jobid;
                    }
                    if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
                    {
                        str += " and (isnull(t.charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge);
                        if (objPropMapData.FilterCharge == "1")
                            str += " or isnull(Invoice,0) <> 0)";
                        else
                            str += " and isnull(Invoice,0) = 0)";
                    }
                    if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                    {
                        str += " and (select Super from tblWork w where w.fdesc=t.dwork ) ='" + objPropMapData.Supervisor + "'";
                    }
                    if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
                    {
                        str += " and (select Super from tblWork w where w.fdesc=t.dwork ) <>'" + objPropMapData.NonSuper + "'";
                    }
                    //if (objPropMapData.FilterReview == "1")
                    //{
                    //    str += " and charge =9";                   
                    //}                
                   
                    if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
                    {
                        str += " and t.workorder='" + objPropMapData.Workorder + "'";
                    }
                    if (objPropMapData.Department != -1)
                    {
                        str += " and t.type=" + objPropMapData.Department;
                    }
                    if (objPropMapData.Category != string.Empty && objPropMapData.Category != null)
                    {
                        //str += " and t.cat='" + objPropMapData.Category + "'";
                        str += " and t.cat in ('" + objPropMapData.Category + "')";
                    }
                    #region ////Filter for ticketo IF ticket Create from Portal
                    if (objPropMapData.IsPortal != string.Empty && objPropMapData.IsPortal != null)
                    {
                        if (objPropMapData.IsPortal == "1")
                        {
                            str += " and t.fBy='portal' ";
                        }
                        if (objPropMapData.IsPortal == "0")
                        {
                            str += " and t.fBy <> 'portal' ";
                        }
                    }
                    #endregion

                    if (objPropMapData.Bremarks != null && objPropMapData.Bremarks != string.Empty)
                    {
                        if (objPropMapData.Bremarks == "1")
                            str += " and isnull( rtrim(ltrim(t.bremarks)),'')<>''";
                        else
                            str += " and isnull( rtrim(ltrim(t.bremarks)),'')=''";
                    }

                    if (objPropMapData.SearchBy != "" && objPropMapData.SearchBy != null && !string.IsNullOrWhiteSpace(objPropMapData.SearchValue))
                    {
                        string SearchBy = objPropMapData.SearchBy.Replace("t.descres", "dp.descres");
                        if (SearchBy == "t.ID")
                        {
                            //str += " and " + SearchBy + " = '" + objPropMapData.SearchValue + "'";
                            str += " and " + SearchBy + " in (" + objPropMapData.SearchValue + ")";
                        }
                        else if (SearchBy == "t.cat")
                        {
                            str += " and " + SearchBy + " in ('" + objPropMapData.SearchValue + "')";
                        }
                        else
                        {
                            str += " and " + SearchBy + " like '%" + objPropMapData.SearchValue + "%'";
                        }
                    }

                    if (objPropMapData.InvoiceID != 0)
                    {
                        if (objPropMapData.InvoiceID == 1)
                        {
                            str += " and isnull(Invoice,0) <> 0";
                        }
                        else if (objPropMapData.InvoiceID == 2)
                        {
                            str += " and isnull(Invoice,0) = 0 and isnull(t.charge,0)= 1";
                        }
                    }
                    //  If the user is a salesperson or Second salesperson only show assigned  locations,Ticket.
                    if (IsSalesAsigned > 0)
                    {
                        str += " AND  (  l.Terr=(" + "select id FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSalesAsigned + ")) or  isnull(l.Terr2,0)=(" + "select id FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSalesAsigned + "))) ";
                    }

                    #endregion End Filter For TicketO

                    #region LocID == 0

                    if (objPropMapData.LocID == 0)
                    {
                        str += " Union all ";
                        str += " SELECT t.who ,t.CPhone , t.lid, '--' as locid, assigned, ( r.address + ', ' + r.city + ', ' + r.state + ', ' + r.zip ) AS fulladdress,r.city, t.WorkOrder, dp.Reg, dp.OT, dp.NT, dp.DT, dp.TT,dp.break_time as BT, dp.Total, 0 AS ClearCheck, t.charge, t.fDesc, t.TimeRoute, t.TimeSite, t.TimeComp, CASE WHEN EXISTS (SELECT 1 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS comp, UPPER(dwork) AS dwork,dwork as lastname,(select isnull(hourlyrate,0) from tblwork where id=t.fwork) as hourlyrate, t.ID, r.Name AS customername,  r.EN, isnull(B.Name, '') As Company,";
                        str += " r.name  AS locname, l.Address  AS address, (select top 1 phone from rol lr where lr.id = l.rol) phone, t.Cat, t.EDate AS edate, t.CDate, dp.descres, CASE WHEN assigned = 0 THEN 'Un-Assigned' WHEN assigned = 1 THEN 'Assigned' WHEN assigned = 2 THEN 'Enroute' WHEN assigned = 3 THEN 'Onsite' WHEN assigned = 4 THEN 'Completed' WHEN assigned = 5 THEN 'Hold' END AS assignname, t.Est, Isnull(dp.Total, 0.00) AS Tottime ,";

                        #region 24 Hours Feature

                        //  str += "   round(convert(numeric(30,2),(Isnull(dp.Total, 0.00) - (convert(float,DATEDIFF(MILLISECOND,dp.TimeRoute,dp.TimeComp))/1000/60/60))),1) as timediff, ";

                        str += @" Round(CONVERT(NUMERIC(30, 2), ( Isnull(dp.Total, 0.00) - ( CONVERT(FLOAT, Datediff(MILLISECOND, dp.TimeRoute, ( CASE
                                                                                                                   WHEN ( Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME)
                                                                                                                           AND Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME) ) THEN Dateadd(day, 2, dp.TimeComp)
                                                                                                                   ELSE (( CASE
                                                                                                                             WHEN ( Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME)
                                                                                                                                    OR Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME) ) THEN Dateadd(day, 1, dp.TimeComp)
                                                                                                                             ELSE dp.TimeComp
                                                                                                                           END ))
                                                                                                                 END ))) / 1000 / 60 / 60 ) )), 1)  AS timediff,";

                        #endregion

                        str += "   t.workorder, (isnull(dp.zone,0)+ isnull(dp.toll,0) + isnull(dp.othere,0)) as expenses, isnull( dp.zone,0) as zone, isnull( dp.toll,0) as toll , isnull(dp.othere,0) as othere, case ISNUMERIC(dbo.udf_GetNumeric(dp.custom2)) when 1 then CONVERT(money ,dbo.udf_GetNumeric(dp.custom2)) else 0 end as extraexp, ((isnull(dp.emile,0)-isnull(dp.smile,0))*0.26) as mileagetravel, ";


                        str += " (isnull(dp.emile,0)-isnull(dp.smile,0)) as mileage ,(select top 1 count(1) as signatureCount  from pdaticketsignature where pdaticketid=t.ID ) as signatureCount, (select count(1) from documents where screen='Ticket' and screenid=t.id) as DocumentCount, t.fwork as workerid, (select top 1 * from dbo.split(descres,'|')) as description, (select top 1 * from dbo.split(t.fdesc,'|')) as fdescreason, 0 as invoice, isnull(Confirmed,0) as Confirmed, '' as manualinvoice, '' as invoiceno, t.owner as ownerid, '' as QBinvoiceid, 0 as TransferTime, ";
                        str += "  (select name from inv where qbinvid= t.QBserviceitem) as serviceitem, ";
                        str += "  (select fdesc from prwage where QBwageID= t.QBPayrollItem) as PayrollItem, ";
                        str += "  (ISNULL( dp.reg ,0) + ISNULL( dp.OT ,0) +ISNULL( dp.TT ,0))     as RTOTTT ";
                        str += "  ,(select top 1 (last+', '+ffirst) from emp em where em.callsign = t.dwork) as WorkerLastName ";
                        if (objPropMapData.StartDate != System.DateTime.MinValue || objPropMapData.EndDate != System.DateTime.MinValue)
                        {
                            str += " ,(SELECT top 1 Signature	FROM   PDATimeSign p	WHERE  p.fWork = t.fwork and (p.EDate BETWEEN '" + objPropMapData.StartDate + "' and '" + objPropMapData.EndDate + "')	ORDER  BY EDate DESC ) as timesign";
                        }
                        str += ", 0 as dispalert, 0 as credithold, 0 as high,e.id as unitid, dbo.TicketEquips(t.ID) as unit, dbo.TicketEquipsColumns(t.ID, 'type') as unittype, '' as defaultworker, '' as defaultmech";
                        str += ", (select type from jobtype where id = t.type) as department,  rtrim(ltrim(t.bremarks)) as bremarks, 0 as laborexp ";
                        str += ", (select top 1 signature  from pdaticketsignature where pdaticketid=t.ID ) as signature , l.state";
                        str += ",  (select top 1 isnull( Rate,0)  from PROther where Emp = (select top 1 id from emp where fWork = t.fwork) and Cat = 5) as mileagepr ";
                        str += ", dbo.Afterhours(t.timeroute,t.timecomp) as afterhours, dbo.weekends(t.edate) as weekends, isnull(EmailNotified,0) as EmailNotified, EmailTime";
                        if (IsCallForTicketReport > 0)
                        {
                            // Show OtherWorker For  same date , same workorder, same location and same project
                            str += @", ([dbo].Getotherworker(t.fWork,t.WorkOrder,t.LID,t.Job,t.EDate)) AS OtherWorker";
                        }
                        str += ",   t.Job ";
                        str += "       , (select convert(varchar(20),t.Job )+'-'+ fdesc from job where ID = t.Job) as ProjectDescription ";
                        str += ",   t.Custom6, t.Custom7 ";
                        str += ", t.fBy ";
                        str += ",(select fDesc from PRWage where id = dp.WageC) as WageCategory ";
                        str += " FROM ticketo t LEFT OUTER JOIN TicketDPDA dp ON t.ID = dp.ID inner join prospect l on l.ID=t.lid INNER JOIN Rol r ON r.ID = l.Rol LEFT OUTER JOIN Branch B on B.ID = r.EN  left outer join Elev e on e.ID=t.LElev  ";
                        if (objPropMapData.EN == 1) // check for company
                        {
                            str += " LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN ";
                        }
                        str += " WHERE t.id is not null and t.owner is null and t.LType=1  ";

                        #region FILTER FOR TICKET_DPDA
                        if (objPropMapData.IsList != 1)
                        {
                            str += " and assigned NOT IN ( 0 )";
                        }
                        if (objPropMapData.EN == 1) // check for company
                        {
                            str += " and UC.IsSel = 1 and UC.UserID =" + objPropMapData.UserID;
                        }
                        if (objPropMapData.Assigned != -1)
                        {
                            if (objPropMapData.Assigned == -2)
                            {
                                str += " and t.Assigned <> 4";
                            }
                            else
                            {
                                str += " and t.Assigned=" + objPropMapData.Assigned;
                            }
                        }
                        if (objPropMapData.StartDate != System.DateTime.MinValue)
                        {
                            str += " and isnull(t.edate,t.cdate) >='" + objPropMapData.StartDate + "'";
                        }
                        if (objPropMapData.EndDate != System.DateTime.MinValue)
                        {
                            str += " and isnull(t.edate,t.cdate) <='" + objPropMapData.EndDate + "'";
                        }
                        if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                        {

                            if (objPropMapData.Worker == "Active")
                            {
                                str += " and t.fwork in (select w.ID from tblWork w where  w.Status=0 )";
                            }
                            else if (objPropMapData.Worker == "Inactive")
                            {
                                str += " and t.fwork in (select w.ID from tblWork w where  w.Status=1 )";
                            }
                            else
                            {
                                str += " and (select w.fdesc from tblWork w where w.ID=t.fwork )='" + objPropMapData.Worker.Replace("'", "''") + "'";
                            }
                        }
                        if (objPropMapData.LocID != 0)
                        {
                            str += " and t.LID=" + objPropMapData.LocID;
                        }
                        if (objPropMapData.CustID != 0)
                        {
                            str += " and t.Owner=" + objPropMapData.CustID;
                        }
                        if (objPropMapData.jobid != 0)
                        {
                            str += " and t.job =" + objPropMapData.jobid;
                        }
                        if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
                        {
                            str += " and (isnull(t.charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge);
                            if (objPropMapData.FilterCharge == "1")
                                str += " or isnull(Invoice,0) <> 0)";
                            else
                                str += " and isnull(Invoice,0) = 0)";
                        }
                        if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                        {
                            str += " and (select Super from tblWork w where w.fdesc=t.dwork ) ='" + objPropMapData.Supervisor + "'";
                        }
                        if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
                        {
                            str += " and (select Super from tblWork w where w.fdesc=t.dwork ) <>'" + objPropMapData.NonSuper + "'";
                        }
                      
                        if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
                        {
                            str += " and t.workorder='" + objPropMapData.Workorder + "'";
                        }
                        if (objPropMapData.Department != -1)
                        {
                            str += " and t.type=" + objPropMapData.Department;
                        }
                        if (objPropMapData.Category != string.Empty && objPropMapData.Category != null)
                        {
                            //str += " and t.cat='" + objPropMapData.Category + "'";
                            str += " and t.cat in ('" + objPropMapData.Category + "')";
                        }
                        #region ////Filter for TicketDPDA IF Create from Portal
                        if (objPropMapData.IsPortal != string.Empty && objPropMapData.IsPortal != null)
                        {
                            if (objPropMapData.IsPortal == "1")
                            {
                                str += " and t.fBy='portal' ";
                            }
                            if (objPropMapData.IsPortal == "0")
                            {
                                str += " and t.fBy <> 'portal' ";
                            }
                        }
                        #endregion

                        if (objPropMapData.Bremarks != null && objPropMapData.Bremarks != string.Empty)
                        {
                            if (objPropMapData.Bremarks == "1")
                                str += " and isnull( rtrim(ltrim(t.bremarks)),'')<>''";
                            else
                                str += " and isnull( rtrim(ltrim(t.bremarks)),'')=''";
                        }
                        if (objPropMapData.SearchBy != "" && objPropMapData.SearchBy != null && !string.IsNullOrWhiteSpace(objPropMapData.SearchValue))
                        {
                            string SearchBy = objPropMapData.SearchBy.Replace("t.descres", "dp.descres");
                            if (SearchBy == "t.ID")
                            {
                                //str += " and " + SearchBy + " = '" + objPropMapData.SearchValue + "'";
                                str += " and " + SearchBy + " in (" + objPropMapData.SearchValue + ")";
                            }
                            else if (SearchBy == "t.cat")
                            {
                                str += " and " + SearchBy + " in ('" + objPropMapData.SearchValue + "')";
                            }
                            else
                            {
                                if (SearchBy == "l.tag")
                                    SearchBy = "r.name";
                                str += " and " + SearchBy + " like '%" + objPropMapData.SearchValue + "%'";
                            }
                        }
                        if (objPropMapData.InvoiceID != 0)
                        {
                            if (objPropMapData.InvoiceID == 1)
                            {
                                str += " and isnull(Invoice,0) <> 0";
                            }
                            else if (objPropMapData.InvoiceID == 2)
                            {
                                str += " and isnull(Invoice,0) = 0 and isnull(t.charge,0)= 1";
                            }
                        }
                        //  If the user is a salesperson only show assigned  locations,Ticket.
                        if (IsSalesAsigned > 0)
                        {
                            str += " AND  ( l.Terr=(" + "select id FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSalesAsigned + ")) )";
                        }
                        #endregion FILTER FOR TICKET_DPDA
                    }

                    #endregion LocID == 0

                }

                #endregion FilterReview != "1"
            }

            #endregion Status != Assigned

            #region Status=Completed/ALL
            if (objPropMapData.Assigned == 4 || objPropMapData.Assigned == -1)//|| objPropMapData.Assigned == 0
            {
             
                {
                    //if (objPropMapData.Mobile == 0)
                    //{
                    if (objPropMapData.Status != 1)
                    {
                        if (objPropMapData.FilterReview != "1")
                        {
                            str += " UNION ALL";
                        }
                    }
                    //}
                    //if (objPropMapData.Mobile != 2)
                    //{
                    str += " SELECT t.who ,t.CPhone, t.loc as lid, l.id as locid, 4 as assigned, (l.address+', '+l.city+', '+l.state+', '+l.zip) as fulladdress,l.city, t.WorkOrder, Reg, OT, NT, DT,TT,break_time as BT, Total,isnull( ClearCheck ,0) as ClearCheck ,t.charge, t.fdesc,timeroute, timesite,timecomp, 1 as comp, (select UPPER(w.fdesc) from tblWork w where t.fwork = w.id) AS dwork,(select w.fdesc from tblWork w where t.fwork = w.id) as lastname,(select isnull(hourlyrate,0) from tblwork where id=t.fwork) as hourlyrate, t.id,  r.Name  AS customername, r.EN, isnull(B.Name, '') As Company, l.tag AS locname, l.address,r.phone,  t.cat, edate, cdate, descres, 'Completed' AS assignname, est,Total as tottime , ";

                    //  str += "   round(convert(numeric(30,2),(Isnull(t.Total, 0.00) - (convert(float,DATEDIFF(MILLISECOND,TimeRoute,TimeComp))/1000/60/60))),1) as timediff, ";


                    #region  24 HOURS FEATURE
                    str += @" Round(CONVERT(NUMERIC(30, 2), ( Isnull(t.Total, 0.00) - ( CONVERT(FLOAT, Datediff(MILLISECOND, TimeRoute, ( CASE
                                                                                                                   WHEN(Cast(TimeSite AS TIME) < Cast(TimeRoute AS TIME)
                                                                                                                           AND Cast(TimeComp AS TIME) < Cast(TimeSite AS TIME)) THEN Dateadd(day, 2, TimeComp)
                                                                                                                   ELSE((CASE
                                                                                                                             WHEN(Cast(TimeSite AS TIME) < Cast(TimeRoute AS TIME)
                                                                                                                                    OR Cast(TimeComp AS TIME) < Cast(TimeSite AS TIME)) THEN Dateadd(day, 1, TimeComp)
                                                                                                                             ELSE TimeComp
                                                                                                                           END))
                                                                                                                 END ))) / 1000 / 60 / 60 ) )), 1)  AS timediff ,";
                    #endregion

                    str += "   t.workorder, (isnull(t.zone,0)+ isnull(t.toll,0) + isnull(t.othere,0)) as expenses, isnull( t.zone,0) as zone, isnull( t.toll,0) as toll , isnull(t.othere,0) as othere, case ISNUMERIC(dbo.udf_GetNumeric(t.custom2)) when 1 then CONVERT(money ,dbo.udf_GetNumeric(t.custom2)) else 0 end as extraexp, ((isnull(t.emile,0)-isnull(t.smile,0))*0.26) as mileagetravel, (isnull(t.emile,0)-isnull(t.smile,0)) as mileage, (select top 1 count(1) as signatureCount  from pdaticketsignature where pdaticketid=t.ID ) as signatureCount,  (select count(1) from documents where screen='Ticket' and screenid=t.id) as DocumentCount, t.fwork as workerid, (select top 1 * from dbo.split(descres,'|')) as description,(select top 1 * from dbo.split(t.fdesc,'|')) as fdescreason, isnull(invoice,0) as invoice, 0 as Confirmed,  manualinvoice, case  when ( Isnull(invoice, 0) =  0 ) then Manualinvoice else CONVERT(varchar(50), Invoice) end as invoiceno, 0 as ownerid, isnull(QBinvoiceID,'')as QBinvoiceid, isnull(TransferTime,0) as TransferTime, ";
                    str += "  (select name from inv where qbinvid= t.QBserviceitem) as serviceitem, ";
                    str += "  (select fdesc from prwage where QBwageID= t.QBPayrollItem) as PayrollItem, ";
                    str += "  (ISNULL( reg ,0) + ISNULL( OT ,0) +ISNULL( TT ,0))     as RTOTTT ";
                    str += "  ,(select top 1 (last+', '+ffirst) from emp em where em.callsign =(select top 1 fdesc from tblwork wo where wo.id = t.fwork)) as WorkerLastName ";
                    if (objPropMapData.StartDate != System.DateTime.MinValue || objPropMapData.EndDate != System.DateTime.MinValue)
                    {
                        str += " , (SELECT top 1 Signature	FROM   PDATimeSign p	WHERE  p.fWork = t.fwork and (p.EDate BETWEEN '" + objPropMapData.StartDate + "' and '" + objPropMapData.EndDate + "')	ORDER  BY EDate DESC ) as timesign";
                    }
                    str += " , isnull(l.dispalert,0)as dispalert, isnull(l.credit,0)as credithold, 0 as high ,e.id as unitid , dbo.TicketEquips(t.ID) as unit, dbo.TicketEquipsColumns(t.ID, 'type') as unittype, (select Name from Route where ID=l.Route ) as defaultworker, (select top 1 fdesc from tblwork where id = (select top 1 mech from Route where ID=l.Route )) as defaultmech";
                    str += " , (select type from jobtype where id = t.type) as department, rtrim(ltrim(t.bremarks)) as bremarks ";
                    //str += ", CONVERT(NUMERIC(30, 2),(((isnull(t.Reg,0) + isnull(t.RegTrav,0)) +  ((isnull(t.OT,0) + isnull(t.OTTrav,0)) * 1.5) + ((isnull(t.DT,0) + isnull(t.DTTrav,0)) * 2) + ((isnull(t.NT,0) + isnull(t.NTTrav,0)) * 1.7) + (isnull(t.TT,0))) * (SELECT Isnull(w.HourlyRate, 0)FROM   tblWork w WHERE  w.ID = t.fWork))) AS LaborExp ";

                    // str += " , (select sum( isnull(Amount ,0))from jobi where Labor = 1 and TransID<0 and ref = t.id group by Ref) as laborexp";

                    str += @", (SELECT Isnull(Amount, 0)   FROM   #jobitemptable   WHERE  ref = t.id)   AS laborexp";

                    str += " , (select top 1 signature from pdaticketsignature where pdaticketid=t.ID ) as signature , l.state";
                    str += " ,  (select top 1 isnull( Rate,0)  from PROther where Emp = (select top 1 id from emp where fWork = t.fwork) and Cat = 5) as mileagepr ";
                    str += " , dbo.Afterhours(t.timeroute,t.timecomp) as afterhours, dbo.weekends(t.edate) as weekends, 0 as EmailNotified, null as EmailTime";
                    if (IsCallForTicketReport > 0)
                    {
                        // Show OtherWorker For  same date , same workorder, same location and same project
                        str += @",  ([dbo].Getotherworker(t.fWork,t.WorkOrder,t.Loc,t.Job,t.EDate)) AS OtherWorker";
                    }
                    str += ",   t.Job ";
                    str += "       , (select convert(varchar(20),t.Job )+'-'+ fdesc from job where ID = t.Job) as ProjectDescription ";
                    str += ",   t.Custom6, t.Custom7 ";
                    str += ", t.fBy ";
                    str += ",(select fDesc from PRWage where id = t.WageC) as WageCategory ";
                    str += " FROM ticketd t INNER JOIN loc l ON l.loc = t.loc  inner join Owner o on l.Owner=o.ID INNER JOIN Rol r ON r.ID = o.Rol LEFT OUTER JOIN Branch B on B.ID = r.EN   left outer join Elev e on e.ID=t.Elev  ";
                    if (objPropMapData.EN == 1) // check for company
                    {
                        str += " LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN ";
                    }
                    str += " WHERE t.id is not null  ";
                    #region FILTER FOR TICKETD
                    if (objPropMapData.StartDate != System.DateTime.MinValue)
                    {
                        str += " and edate >='" + objPropMapData.StartDate + "'";
                    }
                    if (objPropMapData.EndDate != System.DateTime.MinValue)
                    {
                        str += " and edate <='" + objPropMapData.EndDate + "'";
                    }
                    if (objPropMapData.EN == 1) // check for company
                    {
                        str += " and UC.IsSel = 1 and UC.UserID =" + objPropMapData.UserID;
                    }
                    if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                    {

                        if (objPropMapData.Worker == "Active")
                        {
                            str += " and t.fwork in (select w.ID from tblWork w where  w.Status=0 )";
                        }
                        else if (objPropMapData.Worker == "Inactive")
                        {
                            str += " and t.fwork in (select w.ID from tblWork w where  w.Status=1 )";
                        }
                        else
                        {
                            str += " and (select w.fdesc from tblWork w where w.ID=t.fwork )='" + objPropMapData.Worker.Replace("'", "''") + "'";
                        }
                    }
                    if (objPropMapData.LocID != 0)
                    {
                        str += " and l.loc=" + objPropMapData.LocID;
                    }
                    if (objPropMapData.CustID != 0)
                    {
                        str += " and l.Owner=" + objPropMapData.CustID;
                    }
                    if (objPropMapData.jobid != 0)
                    {
                        str += " and t.job =" + objPropMapData.jobid;
                    }
                    if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
                    {
                        str += " and (isnull(t.charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge);
                        if (objPropMapData.FilterCharge == "1")
                            str += " or isnull(Invoice,0) <> 0)";
                        else
                            str += " and isnull(Invoice,0) = 0)";
                    }
                    if (objPropMapData.Timesheet != null && objPropMapData.Timesheet != string.Empty)
                    {
                        str += " and isnull(TransferTime,0)=" + Convert.ToInt32(objPropMapData.Timesheet);
                    }
                    if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                    {
                        str += " and (select Super from tblWork w where w.ID=t.fwork ) ='" + objPropMapData.Supervisor + "'";
                    }
                    if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
                    {
                        str += " and (select Super from tblWork w where w.ID=t.fwork ) <>'" + objPropMapData.NonSuper + "'";
                    }
                    if (objPropMapData.FilterReview != null && objPropMapData.FilterReview != string.Empty)
                    {
                        str += " and isnull( ClearCheck ,0) =" + Convert.ToInt32(objPropMapData.FilterReview);
                    }
                    if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
                    {
                        str += " and t.workorder='" + objPropMapData.Workorder + "'";
                    }
                    if (objPropMapData.Department != -1)
                    {
                        str += " and t.type=" + objPropMapData.Department;
                    }
                    if (objPropMapData.Category != string.Empty && objPropMapData.Category != null)
                    {
                        //str += " and t.cat='" + objPropMapData.Category + "'";
                        str += " and t.cat in (" + objPropMapData.Category + ")";
                    }
                    #region ////Filter For TicketD IF Create from Portal
                    if (objPropMapData.IsPortal != string.Empty && objPropMapData.IsPortal != null)
                    {
                        if (objPropMapData.IsPortal == "1")
                        {
                            str += " and t.fBy='portal' ";
                        }
                        if (objPropMapData.IsPortal == "0")
                        {
                            str += " and t.fBy <> 'portal' ";
                        }
                    }
                    #endregion
                    if (objPropMapData.Bremarks != null && objPropMapData.Bremarks != string.Empty)
                    {
                        if (objPropMapData.Bremarks == "1")
                            str += " and isnull( rtrim(ltrim(t.bremarks)),'')<>''";
                        else
                            str += " and isnull( rtrim(ltrim(t.bremarks)),'')=''";
                    }
                    if (objPropMapData.SearchBy != "" && objPropMapData.SearchBy != null && !string.IsNullOrWhiteSpace(objPropMapData.SearchValue))
                    {
                        if (objPropMapData.SearchBy == "t.ldesc4")
                        {
                            str += " and l.address like '%" + objPropMapData.SearchValue + "%'";
                        }
                        else if (objPropMapData.SearchBy == "t.ID")
                        {
                            //str += " and " + objPropMapData.SearchBy + " = '" + objPropMapData.SearchValue + "'";
                            str += " and " + objPropMapData.SearchBy + " in (" + objPropMapData.SearchValue + ")";
                        }
                        else if (objPropMapData.SearchBy == "t.cat")
                        {
                            str += " and " + objPropMapData.SearchBy + " in ('" + objPropMapData.SearchValue + "')";
                        }
                        else
                        {
                            str += " and " + objPropMapData.SearchBy + " like '%" + objPropMapData.SearchValue + "%'";
                        }
                    }
                    if (objPropMapData.Status == 1)
                    {
                        str += " and isnull( t.status ,0) <> 1 and isnull(t.internet,0) = 1";
                    }
                    //if (!string.IsNullOrEmpty( objPropMapData.LocIDs ))
                    //{
                    //    str += " and l.loc in (" + objPropMapData.LocIDs + ") ";
                    //}
                    if (objPropMapData.RoleID != 0)
                        str += " and isnull(l.roleid,0)=" + objPropMapData.RoleID;

                    //}
                    if (objPropMapData.InvoiceID != 0)
                    {
                        if (objPropMapData.InvoiceID == 1)
                        {
                            str += " and (isnull(Invoice,0) <> 0  or manualinvoice <> '')";
                        }
                        else if (objPropMapData.InvoiceID == 2)
                        {
                            str += " and (isnull(Invoice,0) = 0 or manualinvoice = '') and isnull(t.charge,0)= 1";
                        }
                    }
                    //  If the user is a salesperson only show assigned  locations,Ticket.
                    if (IsSalesAsigned > 0)
                    {
                        str += " AND   (l.Terr=(" + "select id FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSalesAsigned + ")) or isnull(l.Terr2,0)=(" + "select id FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSalesAsigned + ")))";
                    }
                    #endregion FILTER FOR TICKETD
                }
            }
            #endregion Completed/ALL

            if (str != string.Empty)
            {
                if (!string.IsNullOrEmpty(objPropMapData.OrderBy))
                {
                    string order = objPropMapData.OrderBy;
                    if (order == "Workorder  ASC") order = "t.Workorder  ASC";
                    else if (order == "Workorder  DESC") order = "t.Workorder  DESC";

                    str += " order by " + order;
                }
                else
                {
                    str += " order by edate desc";
                }
            }

            str += "  IF OBJECT_ID('tempdb..#jobitemptable') IS NOT NULL   DROP TABLE #jobitemptable  ";

            try
            {
                if (str != string.Empty)
                    return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, str);
                else
                    return objPropMapData.Ds = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //API
        public DataSet getCallHistory(GetCallHistoryParam _GetCallHistory, string ConnectionString, Int32 IsSalesAsigned = 0, int IsCallForTicketReport = 0)
        {
            string str = "";

            str = @" IF OBJECT_ID('tempdb..#jobitemptable') IS NOT NULL  DROP TABLE #jobitemptable
                    CREATE TABLE #jobitemptable  (  Amount NUMERIC(30, 2),  ref  int primary key not null   )
                    INSERT INTO #jobitemptable SELECT Sum(Isnull(Amount, 0)) AS Amount,   ref FROM   jobi WHERE  Labor = 1 AND TransID < 0 GROUP  BY Ref   ";

            //if (objPropMapData.Mobile != 1)
            //{
            #region Status != Assigned
            if (_GetCallHistory.Status != 1)
            {
                #region FilterReview != "1"
                if (_GetCallHistory.FilterReview != "1")
                {
                    str += "  SELECT t.who,t.CPhone, t.lid, l.id as locid, assigned, ( l.address + ', ' + l.city + ', ' + l.state + ', ' + l.zip ) AS fulladdress,l.city, t.WorkOrder, dp.Reg, dp.OT, dp.NT, dp.DT, dp.TT,dp.break_time as BT, dp.Total, 0 AS ClearCheck, t.charge, t.fDesc, t.TimeRoute, t.TimeSite, t.TimeComp, CASE WHEN EXISTS (SELECT 1 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS comp, UPPER(dwork) AS dwork, dwork as lastname,(select isnull(hourlyrate,0) from tblwork where id=t.fwork) as hourlyrate, t.ID, r.Name AS customername, r.EN, isnull(B.Name, '') As Company,";
                    str += " l.Tag  AS locname, l.Address  AS address,r.phone, t.Cat, t.EDate AS edate, t.CDate, dp.descres, CASE WHEN assigned = 0 THEN 'Un-Assigned' WHEN assigned = 1 THEN 'Assigned' WHEN assigned = 2 THEN 'Enroute' WHEN assigned = 3 THEN 'Onsite' WHEN assigned = 4 THEN 'Completed' WHEN assigned = 5 THEN 'Hold' END AS assignname, t.Est, Isnull(dp.Total, 0.00) AS Tottime ,";



                    //,round(convert(numeric(30,2), (Isnull(dp.Total, 0.00) - (convert(float,DATEDIFF(MILLISECOND,dp.TimeRoute,dp.TimeComp))/1000/60/60))),1) as timediff


                    #region 24 HOURS FEATURE

                    str += @" Round(CONVERT(NUMERIC(30, 2), ( Isnull(dp.Total, 0.00) - ( CONVERT(FLOAT, Datediff(MILLISECOND, dp.TimeRoute, ( CASE
                                                                                                                   WHEN(Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME)
                                                                                                                           AND Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME)) THEN Dateadd(day, 2, dp.TimeComp)
                                                                                                                   ELSE((CASE
                                                                                                                             WHEN(Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME)
                                                                                                                                    OR Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME)) THEN Dateadd(day, 1, dp.TimeComp)
                                                                                                                             ELSE dp.TimeComp
                                                                                                                           END))
                                                                                                                 END ))) / 1000 / 60 / 60 ) )), 1)  AS timediff ";
                    #endregion


                    str += "  , t.workorder, (isnull(dp.zone,0)+ isnull(dp.toll,0) + isnull(dp.othere,0)) as expenses, isnull( dp.zone,0) as zone, isnull( dp.toll,0) as toll , isnull(dp.othere,0) as othere, case ISNUMERIC(dbo.udf_GetNumeric(dp.custom2)) when 1 then CONVERT(money ,dbo.udf_GetNumeric(dp.custom2)) else 0 end as extraexp, ((isnull(dp.emile,0)-isnull(dp.smile,0))*0.26) as mileagetravel, ";
                    str += " (isnull(dp.emile,0)-isnull(dp.smile,0)) as mileage ,(select top 1 count(1) as signatureCount  from pdaticketsignature where pdaticketid=t.ID ) as signatureCount, (select count(1) from documents where screen='Ticket' and screenid=t.id) as DocumentCount, t.fwork as workerid, (select top 1 * from dbo.split(descres,'|')) as description, (select top 1 * from dbo.split(t.fdesc,'|')) as fdescreason, 0 as invoice, isnull(Confirmed,0) as Confirmed, '' as manualinvoice, '' as invoiceno, t.owner as ownerid, '' as QBinvoiceid, 0 as TransferTime, ";
                    str += "  (select name from inv where qbinvid= t.QBserviceitem) as serviceitem, ";
                    str += "  (select fdesc from prwage where QBwageID= t.QBPayrollItem) as PayrollItem, ";
                    str += "  (ISNULL( reg ,0) + ISNULL( OT ,0) +ISNULL( TT ,0))     as RTOTTT ";
                    str += "  ,(select top 1 (last+', '+ffirst) from emp em where em.callsign = t.dwork) as WorkerLastName ";
                    if (_GetCallHistory.StartDate != System.DateTime.MinValue || _GetCallHistory.EndDate != System.DateTime.MinValue)
                    {
                        str += " ,(SELECT top 1 Signature	FROM   PDATimeSign p	WHERE  p.fWork = t.fwork and (p.EDate BETWEEN '" + _GetCallHistory.StartDate + "' and '" + _GetCallHistory.EndDate + "')	ORDER  BY EDate DESC ) as timesign";
                    }
                    str += ", isnull(l.dispalert,0)as dispalert, isnull(l.credit,0)as credithold, isnull(t.high,0) as high,e.id as unitid, dbo.TicketEquips(t.ID) as unit, dbo.TicketEquipsColumns(t.ID, 'type') as unittype, (select Name from Route where ID=l.Route ) as defaultworker, (select top 1 fdesc from tblwork where id = (select top 1 mech from Route where ID=l.Route )) as defaultmech";
                    str += ", (select type from jobtype where id = t.type) as department,  rtrim(ltrim(t.bremarks)) as bremarks, 0 as laborexp ";
                    str += ", (select top 1 signature from pdaticketsignature where pdaticketid=t.ID ) as signature, l.state ";
                    str += ", (select top 1 isnull( Rate,0)  from PROther where Emp = (select top 1 id from emp where fWork = t.fwork) and Cat = 5) as mileagepr ";
                    str += ", dbo.Afterhours(t.timeroute,t.timecomp) as afterhours, dbo.weekends(t.edate) as weekends, isnull(EmailNotified,0) as EmailNotified, EmailTime";
                    if (IsCallForTicketReport > 0)
                    {
                        // Show OtherWorker For  same date , same workorder, same location and same project
                        str += @", ([dbo].Getotherworker(t.fWork,t.WorkOrder,t.LID,t.Job,t.EDate)) AS OtherWorker";
                    }
                    str += ",   t.Job";
                    str += "       , (select convert(varchar(20),t.Job )+'-'+ fdesc from job where ID = t.Job) as ProjectDescription ";
                    str += ", t.Custom6, t.Custom7 ";
                    str += ", t.fBy ";
                    str += ",(select fDesc from PRWage where id = dp.WageC) as WageCategory ";
                    str += " FROM ticketo t LEFT OUTER JOIN TicketDPDA dp ON t.ID = dp.ID inner join Loc l on l.Loc=t.lid  inner join Owner o on l.Owner=o.ID INNER JOIN Rol r ON r.ID = o.Rol LEFT OUTER JOIN Branch B on B.ID = r.EN  left outer join Elev e on e.ID=t.LElev "; //assigned NOT IN ( 0 )
                    if (_GetCallHistory.EN == 1) // check for company
                    {
                        str += " LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN ";
                    }
                    str += " WHERE t.id is not null and t.owner is not null  ";
                    #region FILTER FOR Ticket O

                    if (_GetCallHistory.IsList != 1)
                    {
                        str += " and assigned NOT IN ( 0 )";
                    }
                    if (_GetCallHistory.EN == 1) // check for company
                    {
                        str += " and UC.IsSel = 1 and UC.UserID =" + _GetCallHistory.UserID;
                    }
                    if (_GetCallHistory.Assigned != -1)//&& _GetCallHistory.Assigned != 0
                    {
                        if (_GetCallHistory.Assigned == -2)
                        {
                            str += " and t.Assigned <> 4";
                        }
                        else
                        {
                            str += " and t.Assigned=" + _GetCallHistory.Assigned;
                        }
                    }
                    if (_GetCallHistory.StartDate != System.DateTime.MinValue)
                    {
                        str += " and isnull(t.edate,t.cdate) >='" + _GetCallHistory.StartDate + "'";
                    }
                    if (_GetCallHistory.EndDate != System.DateTime.MinValue)
                    {
                        str += " and isnull(t.edate,t.cdate) <='" + _GetCallHistory.EndDate + "'";
                    }
                    if (_GetCallHistory.Worker != string.Empty && _GetCallHistory.Worker != null)
                    {

                        if (_GetCallHistory.Worker == "Active")
                        {
                            str += " and t.fwork in (select w.ID from tblWork w where  w.Status=0 )";
                        }
                        else if (_GetCallHistory.Worker == "Inactive")
                        {
                            str += " and t.fwork in (select w.ID from tblWork w where  w.Status=1 )";
                        }
                        else
                        {
                            str += " and (select w.fdesc from tblWork w where w.ID=t.fwork )='" + _GetCallHistory.Worker.Replace("'", "''") + "'";
                        }
                    }
                    if (_GetCallHistory.LocID != 0)
                    {
                        str += " and t.LID=" + _GetCallHistory.LocID;
                    }
                    if (_GetCallHistory.CustID != 0)
                    {
                        str += " and t.Owner=" + _GetCallHistory.CustID;
                    }
                    if (_GetCallHistory.jobid != 0)
                    {
                        str += " and t.job =" + _GetCallHistory.jobid;
                    }
                    if (_GetCallHistory.FilterCharge != null && _GetCallHistory.FilterCharge != string.Empty)
                    {
                        str += " and (isnull(t.charge,0)=" + Convert.ToInt32(_GetCallHistory.FilterCharge);
                        if (_GetCallHistory.FilterCharge == "1")
                            str += " or isnull(Invoice,0) <> 0)";
                        else
                            str += " and isnull(Invoice,0) = 0)";
                    }
                    if (_GetCallHistory.Supervisor != null && _GetCallHistory.Supervisor != string.Empty)
                    {
                        str += " and (select Super from tblWork w where w.fdesc=t.dwork ) ='" + _GetCallHistory.Supervisor + "'";
                    }
                    if (_GetCallHistory.NonSuper != null && _GetCallHistory.NonSuper != string.Empty)
                    {
                        str += " and (select Super from tblWork w where w.fdesc=t.dwork ) <>'" + _GetCallHistory.NonSuper + "'";
                    }
                    //if (_GetCallHistory.FilterReview == "1")
                    //{
                    //    str += " and charge =9";                   
                    //}                
                    if (_GetCallHistory.Mobile == 2)
                    {
                        str += " and (select case when exists ( select 01 from TicketDPDA where ID=t.ID) then 2 else 0 end as comp)=2";
                    }
                    if (_GetCallHistory.Mobile == 1)
                    {
                        str += " and (select case when exists ( select 01 from TicketDPDA where ID=t.ID) then 2 else 0 end as comp)=0";
                    }
                    if (_GetCallHistory.Workorder != string.Empty && _GetCallHistory.Workorder != null)
                    {
                        str += " and t.workorder='" + _GetCallHistory.Workorder + "'";
                    }
                    if (_GetCallHistory.Department != -1)
                    {
                        str += " and t.type=" + _GetCallHistory.Department;
                    }
                    if (_GetCallHistory.Category != string.Empty && _GetCallHistory.Category != null)
                    {
                        //str += " and t.cat='" + _GetCallHistory.Category + "'";
                        str += " and t.cat in (" + _GetCallHistory.Category + ")";
                    }
                    #region ////Filter for ticketo IF ticket Create from Portal
                    if (_GetCallHistory.IsPortal != string.Empty && _GetCallHistory.IsPortal != null)
                    {
                        if (_GetCallHistory.IsPortal == "1")
                        {
                            str += " and t.fBy='portal' ";
                        }
                        if (_GetCallHistory.IsPortal == "0")
                        {
                            str += " and t.fBy <> 'portal' ";
                        }
                    }
                    #endregion

                    if (_GetCallHistory.Bremarks != null && _GetCallHistory.Bremarks != string.Empty)
                    {
                        if (_GetCallHistory.Bremarks == "1")
                            str += " and isnull( rtrim(ltrim(t.bremarks)),'')<>''";
                        else
                            str += " and isnull( rtrim(ltrim(t.bremarks)),'')=''";
                    }

                    if (_GetCallHistory.SearchBy != "" && _GetCallHistory.SearchBy != null && !string.IsNullOrWhiteSpace(_GetCallHistory.SearchValue))
                    {
                        string SearchBy = _GetCallHistory.SearchBy.Replace("t.descres", "dp.descres");
                        if (SearchBy == "t.ID")
                        {
                            //str += " and " + SearchBy + " = '" + _GetCallHistory.SearchValue + "'";
                            str += " and " + SearchBy + " in (" + _GetCallHistory.SearchValue + ")";
                        }
                        else
                        {
                            str += " and " + SearchBy + " like '%" + _GetCallHistory.SearchValue + "%'";
                        }
                    }

                    if (_GetCallHistory.InvoiceID != 0)
                    {
                        if (_GetCallHistory.InvoiceID == 1)
                        {
                            str += " and isnull(Invoice,0) <> 0";
                        }
                        else if (_GetCallHistory.InvoiceID == 2)
                        {
                            str += " and isnull(Invoice,0) = 0 and isnull(t.charge,0)= 1";
                        }
                    }
                    //  If the user is a salesperson or Second salesperson only show assigned  locations,Ticket.
                    if (IsSalesAsigned > 0)
                    {
                        str += " AND  (  l.Terr=(" + "select id FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSalesAsigned + ")) or  isnull(l.Terr2,0)=(" + "select id FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSalesAsigned + "))) ";
                    }

                    #endregion End Filter For TicketO

                    #region LocID == 0

                    if (_GetCallHistory.LocID == 0)
                    {
                        str += " Union all ";
                        str += " SELECT t.who ,t.CPhone , t.lid, '--' as locid, assigned, ( r.address + ', ' + r.city + ', ' + r.state + ', ' + r.zip ) AS fulladdress,r.city, t.WorkOrder, dp.Reg, dp.OT, dp.NT, dp.DT, dp.TT,dp.break_time as BT, dp.Total, 0 AS ClearCheck, t.charge, t.fDesc, t.TimeRoute, t.TimeSite, t.TimeComp, CASE WHEN EXISTS (SELECT 1 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS comp, UPPER(dwork) AS dwork,dwork as lastname,(select isnull(hourlyrate,0) from tblwork where id=t.fwork) as hourlyrate, t.ID, r.Name AS customername,  r.EN, isnull(B.Name, '') As Company,";
                        str += " r.name  AS locname, l.Address  AS address, (select top 1 phone from rol lr where lr.id = l.rol) phone, t.Cat, t.EDate AS edate, t.CDate, dp.descres, CASE WHEN assigned = 0 THEN 'Un-Assigned' WHEN assigned = 1 THEN 'Assigned' WHEN assigned = 2 THEN 'Enroute' WHEN assigned = 3 THEN 'Onsite' WHEN assigned = 4 THEN 'Completed' WHEN assigned = 5 THEN 'Hold' END AS assignname, t.Est, Isnull(dp.Total, 0.00) AS Tottime ,";

                        #region 24 Hours Feature

                        //  str += "   round(convert(numeric(30,2),(Isnull(dp.Total, 0.00) - (convert(float,DATEDIFF(MILLISECOND,dp.TimeRoute,dp.TimeComp))/1000/60/60))),1) as timediff, ";

                        str += @" Round(CONVERT(NUMERIC(30, 2), ( Isnull(dp.Total, 0.00) - ( CONVERT(FLOAT, Datediff(MILLISECOND, dp.TimeRoute, ( CASE
                                                                                                                   WHEN ( Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME)
                                                                                                                           AND Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME) ) THEN Dateadd(day, 2, dp.TimeComp)
                                                                                                                   ELSE (( CASE
                                                                                                                             WHEN ( Cast(dp.TimeSite AS TIME) < Cast(dp.TimeRoute AS TIME)
                                                                                                                                    OR Cast(dp.TimeComp AS TIME) < Cast(dp.TimeSite AS TIME) ) THEN Dateadd(day, 1, dp.TimeComp)
                                                                                                                             ELSE dp.TimeComp
                                                                                                                           END ))
                                                                                                                 END ))) / 1000 / 60 / 60 ) )), 1)  AS timediff,";

                        #endregion

                        str += "   t.workorder, (isnull(dp.zone,0)+ isnull(dp.toll,0) + isnull(dp.othere,0)) as expenses, isnull( dp.zone,0) as zone, isnull( dp.toll,0) as toll , isnull(dp.othere,0) as othere, case ISNUMERIC(dbo.udf_GetNumeric(dp.custom2)) when 1 then CONVERT(money ,dbo.udf_GetNumeric(dp.custom2)) else 0 end as extraexp, ((isnull(dp.emile,0)-isnull(dp.smile,0))*0.26) as mileagetravel, ";


                        str += " (isnull(dp.emile,0)-isnull(dp.smile,0)) as mileage ,(select top 1 count(1) as signatureCount  from pdaticketsignature where pdaticketid=t.ID ) as signatureCount, (select count(1) from documents where screen='Ticket' and screenid=t.id) as DocumentCount, t.fwork as workerid, (select top 1 * from dbo.split(descres,'|')) as description, (select top 1 * from dbo.split(t.fdesc,'|')) as fdescreason, 0 as invoice, isnull(Confirmed,0) as Confirmed, '' as manualinvoice, '' as invoiceno, t.owner as ownerid, '' as QBinvoiceid, 0 as TransferTime, ";
                        str += "  (select name from inv where qbinvid= t.QBserviceitem) as serviceitem, ";
                        str += "  (select fdesc from prwage where QBwageID= t.QBPayrollItem) as PayrollItem, ";
                        str += "  (ISNULL( reg ,0) + ISNULL( OT ,0) +ISNULL( TT ,0))     as RTOTTT ";
                        str += "  ,(select top 1 (last+', '+ffirst) from emp em where em.callsign = t.dwork) as WorkerLastName ";
                        if (_GetCallHistory.StartDate != System.DateTime.MinValue || _GetCallHistory.EndDate != System.DateTime.MinValue)
                        {
                            str += " ,(SELECT top 1 Signature	FROM   PDATimeSign p	WHERE  p.fWork = t.fwork and (p.EDate BETWEEN '" + _GetCallHistory.StartDate + "' and '" + _GetCallHistory.EndDate + "')	ORDER  BY EDate DESC ) as timesign";
                        }
                        str += ", 0 as dispalert, 0 as credithold, 0 as high,e.id as unitid, dbo.TicketEquips(t.ID) as unit, dbo.TicketEquipsColumns(t.ID, 'type') as unittype, '' as defaultworker, '' as defaultmech";
                        str += ", (select type from jobtype where id = t.type) as department,  rtrim(ltrim(t.bremarks)) as bremarks, 0 as laborexp ";
                        str += ", (select top 1 signature  from pdaticketsignature where pdaticketid=t.ID ) as signature , l.state";
                        str += ",  (select top 1 isnull( Rate,0)  from PROther where Emp = (select top 1 id from emp where fWork = t.fwork) and Cat = 5) as mileagepr ";
                        str += ", dbo.Afterhours(t.timeroute,t.timecomp) as afterhours, dbo.weekends(t.edate) as weekends, isnull(EmailNotified,0) as EmailNotified, EmailTime";
                        if (IsCallForTicketReport > 0)
                        {
                            // Show OtherWorker For  same date , same workorder, same location and same project
                            str += @", ([dbo].Getotherworker(t.fWork,t.WorkOrder,t.LID,t.Job,t.EDate)) AS OtherWorker";
                        }
                        str += ",   t.Job ";
                        str += "       , (select convert(varchar(20),t.Job )+'-'+ fdesc from job where ID = t.Job) as ProjectDescription ";
                        str += ",   t.Custom6, t.Custom7 ";
                        str += ", t.fBy ";
                        str += ",(select fDesc from PRWage where id = dp.WageC) as WageCategory ";
                        str += " FROM ticketo t LEFT OUTER JOIN TicketDPDA dp ON t.ID = dp.ID inner join prospect l on l.ID=t.lid INNER JOIN Rol r ON r.ID = l.Rol LEFT OUTER JOIN Branch B on B.ID = r.EN  left outer join Elev e on e.ID=t.LElev  ";
                        if (_GetCallHistory.EN == 1) // check for company
                        {
                            str += " LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN ";
                        }
                        str += " WHERE t.id is not null and t.owner is null and t.LType=1  ";

                        #region FILTER FOR TICKET_DPDA
                        if (_GetCallHistory.IsList != 1)
                        {
                            str += " and assigned NOT IN ( 0 )";
                        }
                        if (_GetCallHistory.EN == 1) // check for company
                        {
                            str += " and UC.IsSel = 1 and UC.UserID =" + _GetCallHistory.UserID;
                        }
                        if (_GetCallHistory.Assigned != -1)
                        {
                            if (_GetCallHistory.Assigned == -2)
                            {
                                str += " and t.Assigned <> 4";
                            }
                            else
                            {
                                str += " and t.Assigned=" + _GetCallHistory.Assigned;
                            }
                        }
                        if (_GetCallHistory.StartDate != System.DateTime.MinValue)
                        {
                            str += " and isnull(t.edate,t.cdate) >='" + _GetCallHistory.StartDate + "'";
                        }
                        if (_GetCallHistory.EndDate != System.DateTime.MinValue)
                        {
                            str += " and isnull(t.edate,t.cdate) <='" + _GetCallHistory.EndDate + "'";
                        }
                        if (_GetCallHistory.Worker != string.Empty && _GetCallHistory.Worker != null)
                        {

                            if (_GetCallHistory.Worker == "Active")
                            {
                                str += " and t.fwork in (select w.ID from tblWork w where  w.Status=0 )";
                            }
                            else if (_GetCallHistory.Worker == "Inactive")
                            {
                                str += " and t.fwork in (select w.ID from tblWork w where  w.Status=1 )";
                            }
                            else
                            {
                                str += " and (select w.fdesc from tblWork w where w.ID=t.fwork )='" + _GetCallHistory.Worker.Replace("'", "''") + "'";
                            }
                        }
                        if (_GetCallHistory.LocID != 0)
                        {
                            str += " and t.LID=" + _GetCallHistory.LocID;
                        }
                        if (_GetCallHistory.CustID != 0)
                        {
                            str += " and t.Owner=" + _GetCallHistory.CustID;
                        }
                        if (_GetCallHistory.jobid != 0)
                        {
                            str += " and t.job =" + _GetCallHistory.jobid;
                        }
                        if (_GetCallHistory.FilterCharge != null && _GetCallHistory.FilterCharge != string.Empty)
                        {
                            str += " and (isnull(t.charge,0)=" + Convert.ToInt32(_GetCallHistory.FilterCharge);
                            if (_GetCallHistory.FilterCharge == "1")
                                str += " or isnull(Invoice,0) <> 0)";
                            else
                                str += " and isnull(Invoice,0) = 0)";
                        }
                        if (_GetCallHistory.Supervisor != null && _GetCallHistory.Supervisor != string.Empty)
                        {
                            str += " and (select Super from tblWork w where w.fdesc=t.dwork ) ='" + _GetCallHistory.Supervisor + "'";
                        }
                        if (_GetCallHistory.NonSuper != null && _GetCallHistory.NonSuper != string.Empty)
                        {
                            str += " and (select Super from tblWork w where w.fdesc=t.dwork ) <>'" + _GetCallHistory.NonSuper + "'";
                        }
                        if (_GetCallHistory.Mobile == 2)
                        {
                            str += " and (select case when exists ( select 01 from TicketDPDA where ID=t.ID) then 2 else 0 end as comp)=2";
                        }
                        if (_GetCallHistory.Mobile == 1)
                        {
                            str += " and (select case when exists ( select 01 from TicketDPDA where ID=t.ID) then 2 else 0 end as comp)=0";
                        }
                        if (_GetCallHistory.Workorder != string.Empty && _GetCallHistory.Workorder != null)
                        {
                            str += " and t.workorder='" + _GetCallHistory.Workorder + "'";
                        }
                        if (_GetCallHistory.Department != -1)
                        {
                            str += " and t.type=" + _GetCallHistory.Department;
                        }
                        if (_GetCallHistory.Category != string.Empty && _GetCallHistory.Category != null)
                        {
                            //str += " and t.cat='" + _GetCallHistory.Category + "'";
                            str += " and t.cat in (" + _GetCallHistory.Category + ")";
                        }
                        #region ////Filter for TicketDPDA IF Create from Portal
                        if (_GetCallHistory.IsPortal != string.Empty && _GetCallHistory.IsPortal != null)
                        {
                            if (_GetCallHistory.IsPortal == "1")
                            {
                                str += " and t.fBy='portal' ";
                            }
                            if (_GetCallHistory.IsPortal == "0")
                            {
                                str += " and t.fBy <> 'portal' ";
                            }
                        }
                        #endregion

                        if (_GetCallHistory.Bremarks != null && _GetCallHistory.Bremarks != string.Empty)
                        {
                            if (_GetCallHistory.Bremarks == "1")
                                str += " and isnull( rtrim(ltrim(t.bremarks)),'')<>''";
                            else
                                str += " and isnull( rtrim(ltrim(t.bremarks)),'')=''";
                        }
                        if (_GetCallHistory.SearchBy != "" && _GetCallHistory.SearchBy != null && !string.IsNullOrWhiteSpace(_GetCallHistory.SearchValue))
                        {
                            string SearchBy = _GetCallHistory.SearchBy.Replace("t.descres", "dp.descres");
                            if (SearchBy == "t.ID")
                            {
                                //str += " and " + SearchBy + " = '" + _GetCallHistory.SearchValue + "'";
                                str += " and " + SearchBy + " in (" + _GetCallHistory.SearchValue + ")";
                            }
                            else
                            {
                                if (SearchBy == "l.tag")
                                    SearchBy = "r.name";
                                str += " and " + SearchBy + " like '%" + _GetCallHistory.SearchValue + "%'";
                            }
                        }
                        if (_GetCallHistory.InvoiceID != 0)
                        {
                            if (_GetCallHistory.InvoiceID == 1)
                            {
                                str += " and isnull(Invoice,0) <> 0";
                            }
                            else if (_GetCallHistory.InvoiceID == 2)
                            {
                                str += " and isnull(Invoice,0) = 0 and isnull(t.charge,0)= 1";
                            }
                        }
                        //  If the user is a salesperson only show assigned  locations,Ticket.
                        if (IsSalesAsigned > 0)
                        {
                            str += " AND  ( l.Terr=(" + "select id FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSalesAsigned + ")) )";
                        }
                        #endregion FILTER FOR TICKET_DPDA
                    }

                    #endregion LocID == 0

                }

                #endregion FilterReview != "1"
            }

            #endregion Status != Assigned

            #region Status=Completed/ALL
            if (_GetCallHistory.Assigned == 4 || _GetCallHistory.Assigned == -1)//|| _GetCallHistory.Assigned == 0
            {
                if (_GetCallHistory.Mobile != 2)
                {
                    //if (_GetCallHistory.Mobile == 0)
                    //{
                    if (_GetCallHistory.Status != 1)
                    {
                        if (_GetCallHistory.FilterReview != "1")
                        {
                            str += " UNION ALL";
                        }
                    }
                    //}
                    //if (_GetCallHistory.Mobile != 2)
                    //{
                    str += " SELECT t.who ,t.CPhone, t.loc as lid, l.id as locid, 4 as assigned, (l.address+', '+l.city+', '+l.state+', '+l.zip) as fulladdress,l.city, t.WorkOrder, Reg, OT, NT, DT,TT,break_time as BT, Total,isnull( ClearCheck ,0) as ClearCheck ,t.charge, t.fdesc,timeroute, timesite,timecomp, 1 as comp, (select UPPER(w.fdesc) from tblWork w where t.fwork = w.id) AS dwork,(select w.fdesc from tblWork w where t.fwork = w.id) as lastname,(select isnull(hourlyrate,0) from tblwork where id=t.fwork) as hourlyrate, t.id,  r.Name  AS customername, r.EN, isnull(B.Name, '') As Company, l.tag AS locname, l.address,r.phone,  t.cat, edate, cdate, descres, 'Completed' AS assignname, est,Total as tottime , ";

                    //  str += "   round(convert(numeric(30,2),(Isnull(dp.Total, 0.00) - (convert(float,DATEDIFF(MILLISECOND,TimeRoute,TimeComp))/1000/60/60))),1) as timediff, ";


                    #region  24 HOURS FEATURE
                    str += @" Round(CONVERT(NUMERIC(30, 2), ( Isnull(dp.Total, 0.00) - ( CONVERT(FLOAT, Datediff(MILLISECOND, TimeRoute, ( CASE
                                                                                                                   WHEN(Cast(TimeSite AS TIME) < Cast(TimeRoute AS TIME)
                                                                                                                           AND Cast(TimeComp AS TIME) < Cast(TimeSite AS TIME)) THEN Dateadd(day, 2, TimeComp)
                                                                                                                   ELSE((CASE
                                                                                                                             WHEN(Cast(TimeSite AS TIME) < Cast(TimeRoute AS TIME)
                                                                                                                                    OR Cast(TimeComp AS TIME) < Cast(TimeSite AS TIME)) THEN Dateadd(day, 1, TimeComp)
                                                                                                                             ELSE TimeComp
                                                                                                                           END))
                                                                                                                 END ))) / 1000 / 60 / 60 ) )), 1)  AS timediff ,";
                    #endregion

                    str += "   t.workorder, (isnull(t.zone,0)+ isnull(t.toll,0) + isnull(t.othere,0)) as expenses, isnull( t.zone,0) as zone, isnull( t.toll,0) as toll , isnull(t.othere,0) as othere, case ISNUMERIC(dbo.udf_GetNumeric(t.custom2)) when 1 then CONVERT(money ,dbo.udf_GetNumeric(t.custom2)) else 0 end as extraexp, ((isnull(t.emile,0)-isnull(t.smile,0))*0.26) as mileagetravel, (isnull(t.emile,0)-isnull(t.smile,0)) as mileage, (select top 1 count(1) as signatureCount  from pdaticketsignature where pdaticketid=t.ID ) as signatureCount,  (select count(1) from documents where screen='Ticket' and screenid=t.id) as DocumentCount, t.fwork as workerid, (select top 1 * from dbo.split(descres,'|')) as description,(select top 1 * from dbo.split(t.fdesc,'|')) as fdescreason, isnull(invoice,0) as invoice, 0 as Confirmed,  manualinvoice, case  when ( Isnull(invoice, 0) =  0 ) then Manualinvoice else CONVERT(varchar(50), Invoice) end as invoiceno, 0 as ownerid, isnull(t.QBinvoiceID,'')as QBinvoiceid, isnull(TransferTime,0) as TransferTime, ";
                    str += "  (select name from inv where qbinvid= t.QBserviceitem) as serviceitem, ";
                    str += "  (select fdesc from prwage where QBwageID= t.QBPayrollItem) as PayrollItem, ";
                    str += "  (ISNULL( reg ,0) + ISNULL( OT ,0) +ISNULL( TT ,0))     as RTOTTT ";
                    str += "  ,(select top 1 (last+', '+ffirst) from emp em where em.callsign =(select top 1 fdesc from tblwork wo where wo.id = t.fwork)) as WorkerLastName ";
                    if (_GetCallHistory.StartDate != System.DateTime.MinValue || _GetCallHistory.EndDate != System.DateTime.MinValue)
                    {
                        str += " , (SELECT top 1 Signature	FROM   PDATimeSign p	WHERE  p.fWork = t.fwork and (p.EDate BETWEEN '" + _GetCallHistory.StartDate + "' and '" + _GetCallHistory.EndDate + "')	ORDER  BY EDate DESC ) as timesign";
                    }
                    str += " , isnull(l.dispalert,0)as dispalert, isnull(l.credit,0)as credithold, 0 as high ,e.id as unitid , dbo.TicketEquips(t.ID) as unit, dbo.TicketEquipsColumns(t.ID, 'type') as unittype, (select Name from Route where ID=l.Route ) as defaultworker, (select top 1 fdesc from tblwork where id = (select top 1 mech from Route where ID=l.Route )) as defaultmech";
                    str += " , (select type from jobtype where id = t.type) as department, rtrim(ltrim(t.bremarks)) as bremarks ";
                    //str += ", CONVERT(NUMERIC(30, 2),(((isnull(t.Reg,0) + isnull(t.RegTrav,0)) +  ((isnull(t.OT,0) + isnull(t.OTTrav,0)) * 1.5) + ((isnull(t.DT,0) + isnull(t.DTTrav,0)) * 2) + ((isnull(t.NT,0) + isnull(t.NTTrav,0)) * 1.7) + (isnull(t.TT,0))) * (SELECT Isnull(w.HourlyRate, 0)FROM   tblWork w WHERE  w.ID = t.fWork))) AS LaborExp ";

                    // str += " , (select sum( isnull(Amount ,0))from jobi where Labor = 1 and TransID<0 and ref = t.id group by Ref) as laborexp";

                    str += @", (SELECT Isnull(Amount, 0)   FROM   #jobitemptable   WHERE  ref = t.id)   AS laborexp";

                    str += " , (select top 1 signature from pdaticketsignature where pdaticketid=t.ID ) as signature , l.state";
                    str += " ,  (select top 1 isnull( Rate,0)  from PROther where Emp = (select top 1 id from emp where fWork = t.fwork) and Cat = 5) as mileagepr ";
                    str += " , dbo.Afterhours(t.timeroute,t.timecomp) as afterhours, dbo.weekends(t.edate) as weekends, 0 as EmailNotified, null as EmailTime";
                    if (IsCallForTicketReport > 0)
                    {
                        // Show OtherWorker For  same date , same workorder, same location and same project
                        str += @",  ([dbo].Getotherworker(t.fWork,t.WorkOrder,t.Loc,t.Job,t.EDate)) AS OtherWorker";
                    }
                    str += ",   t.Job ";
                    str += "       , (select convert(varchar(20),t.Job )+'-'+ fdesc from job where ID = t.Job) as ProjectDescription ";
                    str += ",   t.Custom6, t.Custom7 ";
                    str += ", t.fBy ";
                    str += ",(select fDesc from PRWage where id = t.WageC) as WageCategory ";
                    str += " FROM ticketd t INNER JOIN loc l ON l.loc = t.loc  inner join Owner o on l.Owner=o.ID INNER JOIN Rol r ON r.ID = o.Rol LEFT OUTER JOIN Branch B on B.ID = r.EN   left outer join Elev e on e.ID=t.Elev  ";
                    if (_GetCallHistory.EN == 1) // check for company
                    {
                        str += " LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN ";
                    }
                    str += " WHERE t.id is not null  ";
                    #region FILTER FOR TICKETD
                    if (_GetCallHistory.StartDate != System.DateTime.MinValue)
                    {
                        str += " and edate >='" + _GetCallHistory.StartDate + "'";
                    }
                    if (_GetCallHistory.EndDate != System.DateTime.MinValue)
                    {
                        str += " and edate <='" + _GetCallHistory.EndDate + "'";
                    }
                    if (_GetCallHistory.EN == 1) // check for company
                    {
                        str += " and UC.IsSel = 1 and UC.UserID =" + _GetCallHistory.UserID;
                    }
                    if (_GetCallHistory.Worker != string.Empty && _GetCallHistory.Worker != null)
                    {

                        if (_GetCallHistory.Worker == "Active")
                        {
                            str += " and t.fwork in (select w.ID from tblWork w where  w.Status=0 )";
                        }
                        else if (_GetCallHistory.Worker == "Inactive")
                        {
                            str += " and t.fwork in (select w.ID from tblWork w where  w.Status=1 )";
                        }
                        else
                        {
                            str += " and (select w.fdesc from tblWork w where w.ID=t.fwork )='" + _GetCallHistory.Worker.Replace("'", "''") + "'";
                        }
                    }
                    if (_GetCallHistory.LocID != 0)
                    {
                        str += " and l.loc=" + _GetCallHistory.LocID;
                    }
                    if (_GetCallHistory.CustID != 0)
                    {
                        str += " and l.Owner=" + _GetCallHistory.CustID;
                    }
                    if (_GetCallHistory.jobid != 0)
                    {
                        str += " and t.job =" + _GetCallHistory.jobid;
                    }
                    if (_GetCallHistory.FilterCharge != null && _GetCallHistory.FilterCharge != string.Empty)
                    {
                        str += " and (isnull(t.charge,0)=" + Convert.ToInt32(_GetCallHistory.FilterCharge);
                        if (_GetCallHistory.FilterCharge == "1")
                            str += " or isnull(Invoice,0) <> 0)";
                        else
                            str += " and isnull(Invoice,0) = 0)";
                    }
                    if (_GetCallHistory.Timesheet != null && _GetCallHistory.Timesheet != string.Empty)
                    {
                        str += " and isnull(TransferTime,0)=" + Convert.ToInt32(_GetCallHistory.Timesheet);
                    }
                    if (_GetCallHistory.Supervisor != null && _GetCallHistory.Supervisor != string.Empty)
                    {
                        str += " and (select Super from tblWork w where w.ID=t.fwork ) ='" + _GetCallHistory.Supervisor + "'";
                    }
                    if (_GetCallHistory.NonSuper != null && _GetCallHistory.NonSuper != string.Empty)
                    {
                        str += " and (select Super from tblWork w where w.ID=t.fwork ) <>'" + _GetCallHistory.NonSuper + "'";
                    }
                    if (_GetCallHistory.FilterReview != null && _GetCallHistory.FilterReview != string.Empty)
                    {
                        str += " and isnull( ClearCheck ,0) =" + Convert.ToInt32(_GetCallHistory.FilterReview);
                    }
                    if (_GetCallHistory.Workorder != string.Empty && _GetCallHistory.Workorder != null)
                    {
                        str += " and t.workorder='" + _GetCallHistory.Workorder + "'";
                    }
                    if (_GetCallHistory.Department != -1)
                    {
                        str += " and t.type=" + _GetCallHistory.Department;
                    }
                    if (_GetCallHistory.Category != string.Empty && _GetCallHistory.Category != null)
                    {
                        //str += " and t.cat='" + _GetCallHistory.Category + "'";
                        str += " and t.cat in (" + _GetCallHistory.Category + ")";
                    }
                    #region ////Filter For TicketD IF Create from Portal
                    if (_GetCallHistory.IsPortal != string.Empty && _GetCallHistory.IsPortal != null)
                    {
                        if (_GetCallHistory.IsPortal == "1")
                        {
                            str += " and t.fBy='portal' ";
                        }
                        if (_GetCallHistory.IsPortal == "0")
                        {
                            str += " and t.fBy <> 'portal' ";
                        }
                    }
                    #endregion
                    if (_GetCallHistory.Bremarks != null && _GetCallHistory.Bremarks != string.Empty)
                    {
                        if (_GetCallHistory.Bremarks == "1")
                            str += " and isnull( rtrim(ltrim(t.bremarks)),'')<>''";
                        else
                            str += " and isnull( rtrim(ltrim(t.bremarks)),'')=''";
                    }
                    if (_GetCallHistory.SearchBy != "" && _GetCallHistory.SearchBy != null && !string.IsNullOrWhiteSpace(_GetCallHistory.SearchValue))
                    {
                        if (_GetCallHistory.SearchBy == "t.ldesc4")
                        {
                            str += " and l.address like '%" + _GetCallHistory.SearchValue + "%'";
                        }
                        else if (_GetCallHistory.SearchBy == "t.ID")
                        {
                            //str += " and " + _GetCallHistory.SearchBy + " = '" + _GetCallHistory.SearchValue + "'";
                            str += " and " + _GetCallHistory.SearchBy + " in (" + _GetCallHistory.SearchValue + ")";
                        }
                        else
                        {
                            str += " and " + _GetCallHistory.SearchBy + " like '%" + _GetCallHistory.SearchValue + "%'";
                        }
                    }
                    if (_GetCallHistory.Status == 1)
                    {
                        str += " and isnull( t.status ,0) <> 1 and isnull(t.internet,0) = 1";
                    }
                    //if (!string.IsNullOrEmpty( _GetCallHistory.LocIDs ))
                    //{
                    //    str += " and l.loc in (" + _GetCallHistory.LocIDs + ") ";
                    //}
                    if (_GetCallHistory.RoleID != 0)
                        str += " and isnull(l.roleid,0)=" + _GetCallHistory.RoleID;

                    //}
                    if (_GetCallHistory.InvoiceID != 0)
                    {
                        if (_GetCallHistory.InvoiceID == 1)
                        {
                            str += " and (isnull(Invoice,0) <> 0  or manualinvoice <> '')";
                        }
                        else if (_GetCallHistory.InvoiceID == 2)
                        {
                            str += " and (isnull(Invoice,0) = 0 or manualinvoice = '') and isnull(t.charge,0)= 1";
                        }
                    }
                    //  If the user is a salesperson only show assigned  locations,Ticket.
                    if (IsSalesAsigned > 0)
                    {
                        str += " AND   (l.Terr=(" + "select id FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSalesAsigned + ")) or isnull(l.Terr2,0)=(" + "select id FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSalesAsigned + ")))";
                    }
                    #endregion FILTER FOR TICKETD
                }
            }
            #endregion Completed/ALL

            if (str != string.Empty)
            {
                if (!string.IsNullOrEmpty(_GetCallHistory.OrderBy))
                {
                    string order = _GetCallHistory.OrderBy;
                    if (order == "Workorder  ASC") order = "t.Workorder  ASC";
                    else if (order == "Workorder  DESC") order = "t.Workorder  DESC";

                    str += " order by " + order;
                }
                else
                {
                    str += " order by edate desc";
                }
            }

            str += "  IF OBJECT_ID('tempdb..#jobitemptable') IS NOT NULL   DROP TABLE #jobitemptable  ";

            try
            {
                if (str != string.Empty)
                    return _GetCallHistory.Ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, str);
                else
                    return _GetCallHistory.Ds = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Get ServiceHistory



        //inventory
        public DataSet IsItemOnHand(TicketI ticket)
        {
            try
            {
                StringBuilder query = new StringBuilder();

                query.Append("select InvID as ID from IWarehouseLocAdj where  Hand > 0 and InvID='" + ticket.Item + "'\n");

                if (ticket.WarehouseID != "" && ticket.WarehouseID != "0") { query.Append(" and WarehouseID='" + ticket.WarehouseID + "'\n"); }
                if (ticket.WHLocationID > 0) { query.Append(" and LocationID='" + ticket.WHLocationID + "'\n"); }

                query.Append("select LCost from INV where ID='" + ticket.Item + "'\n");

                return ticket.DS = SqlHelper.ExecuteDataset(ticket.ConnConfig, CommandType.Text, query.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTicketIByID(TicketI ticket)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter
                {
                    ParameterName = "@TicketID",
                    SqlDbType = SqlDbType.Int,
                    Value = ticket.TicketID
                };
                return ticket.DS = SqlHelper.ExecuteDataset(ticket.ConnConfig, CommandType.StoredProcedure, "GetTicketINVInfo", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetTicketDataIByID(int TicketID, string ConnConfig)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter
                {
                    ParameterName = "@TicketID",
                    SqlDbType = SqlDbType.Int,
                    Value = TicketID
                };
                return SqlHelper.ExecuteDataset(ConnConfig, CommandType.StoredProcedure, "spGetTicketDataByID", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string UpdateListTicketIDsForSearch(string strTicketIDs)
        {
            if (!string.IsNullOrWhiteSpace(strTicketIDs))
            {
                var arr = strTicketIDs.Split(',');
                List<string> list = new List<string>(arr);
                for (int i = 0; i < list.Count; i++)
                {
                    try
                    {
                        int.Parse(list[i]);
                    }
                    catch (Exception)
                    {
                        list[i] = "0";
                    }
                }
                //foreach (var item in list)
                //{
                //    try
                //    {
                //        int.Parse(item);
                //    }
                //    catch (Exception)
                //    {
                //        list.Remove(item);
                //    }
                //}

                return string.Join(",", list.ToArray());
            }
            else
            {
                return strTicketIDs;
            }
        }

    }
}
