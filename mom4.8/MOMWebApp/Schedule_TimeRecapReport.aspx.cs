using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using Microsoft.ApplicationBlocks.Data;
using Stimulsoft.Report;
using Telerik.Web.UI;

public partial class Schedule_TimeRecapReport : System.Web.UI.Page
{
    GeneralFunctions objgn = new GeneralFunctions();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    MapData objMapData = new MapData();
    BL_MapData objBL_MapData = new BL_MapData();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
            return;
        }

        if (!IsPostBack)
        {
            HighlightSideMenu("schMgr", "lnkTimesheet", "schdMgrSub");

            GetSMTPUser();
            SetAddress();
            string FileName = "TimeRecapReport.pdf";
            ArrayList lstPath = new ArrayList();
            if (ViewState["pathmailatt"] != null)
            {
                lstPath = (ArrayList)ViewState["pathmailatt"];
                lstPath.Add(FileName);
            }
            else
            {
                lstPath.Add(FileName);
            }

            ViewState["pathmailatt"] = lstPath;
            dlAttachmentsDelete.DataSource = lstPath;
            dlAttachmentsDelete.DataBind();

            hdnFirstAttachement.Value = FileName;
        }
    }

    protected void StiWebViewerTimeRecapReport_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        e.Report = LoadTimeRecapReport();
    }

    protected void StiWebViewerTimeRecapReport_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        
    }

    public DataSet GetReportData()
    {
        MapData objPropMapData = new MapData();
        objPropMapData.ConnConfig = Session["config"].ToString();

        if (Request.QueryString["Screen"] == "ticketList" && Session["TicketListFilters"] != null)
        {
            objPropMapData = (MapData)Session["TicketListFilters"];
        }

        int IsSalesAsigned = new GeneralFunctions().GetSalesAsigned();

        string stdate = Request.QueryString["StartDate"] + " 00:00:00";
        string enddate = Request.QueryString["EndDate"] + " 23:59:59";

        DateTime StartDate = Convert.ToDateTime(stdate);
        DateTime EndDate = Convert.ToDateTime(enddate);

        string strD = string.Empty;

        #region FILTER FOR TICKETD

        strD += " AND t.EDate >='" + StartDate + "'";
        strD += " AND t.EDate <='" + EndDate + "'";

        objPropMapData.UserID = Convert.ToInt32(Session["UserID"].ToString());

        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            objMapData.EN = 1;
        }
        else
        {
            objMapData.EN = 0;
        }

        if (objPropMapData.EN == 1) // check for company
        {
            strD += " AND UC.IsSel = 1 AND UC.UserID =" + objPropMapData.UserID;
        }
        if (Request.QueryString["ddlDeprt"] != "-1")
        {
            strD += " AND t.Type=" + objPropMapData.Department;
        }
        if (IsSalesAsigned > 0)
        {
            strD += " AND (l.Terr=(" + " SELECT  ID FROM Terr WHERE Name = ( SELECT  fUser FROM  tblUser WHERE ID=" + IsSalesAsigned + ")) or ISNULL(l.Terr2, 0)=(" + " SELECT  ID FROM Terr WHERE Name = ( SELECT  fUser FROM  tblUser WHERE ID =" + IsSalesAsigned + ")))";
        }

        if (Request.QueryString["Screen"] == "ticketList")
        {
            if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
            {
                if (objPropMapData.Worker == "Active")
                {
                    strD += " AND t.fwork in ( SELECT  w.ID FROM tblWork w WHERE w.Status = 0 )";
                }
                else if (objPropMapData.Worker == "Inactive")
                {
                    strD += " AND t.fwork in ( SELECT  w.ID FROM tblWork w WHERE w.Status = 1 )";
                }
                else
                {
                    strD += " AND ( SELECT  w.fdesc FROM tblWork w WHERE w.ID = t.fWork )='" + objPropMapData.Worker.Replace("'", "''") + "'";
                }
            }
            if (objPropMapData.LocID != 0)
            {
                strD += " AND l.Loc=" + objPropMapData.LocID;
            }
            if (objPropMapData.CustID != 0)
            {
                strD += " AND l.Owner=" + objPropMapData.CustID;
            }
            if (objPropMapData.jobid != 0)
            {
                strD += " AND t.Job =" + objPropMapData.jobid;
            }
            if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
            {
                strD += " AND (ISNULL(t.Charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge);
                if (objPropMapData.FilterCharge == "1")
                {
                    strD += " or ISNULL(Invoice,0) <> 0)";
                }
                else
                {
                    strD += " AND ISNULL(Invoice,0) = 0)";
                }
            }
            if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
            {
                strD += " AND ( SELECT  Super FROM tblWork w WHERE w.ID = t.fwork ) = '" + objPropMapData.Supervisor + "'";
            }
            if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
            {
                strD += " AND ( SELECT  Super FROM tblWork w WHERE w.ID = t.fwork ) <> '" + objPropMapData.NonSuper + "'";
            }
            if (objPropMapData.FilterReview != null && objPropMapData.FilterReview != string.Empty)
            {
                strD += " AND ISNULL( ClearCheck, 0) =" + Convert.ToInt32(objPropMapData.FilterReview);
            }
            if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
            {
                strD += " AND t.Workorder = '" + objPropMapData.Workorder + "'";
            }
            if (objPropMapData.Category != string.Empty && objPropMapData.Category != null)
            {
                strD += " AND t.Cat IN (" + objPropMapData.Category + ")";
            }
        }

        #endregion FILTER FOR TICKETD

        #region FILTER FOR TICKETO

        string strO = string.Empty;
        strO += " AND DPDA.EDate >='" + StartDate + "'";
        strO += " AND DPDA.EDate <='" + EndDate + "'";

        objPropMapData.UserID = Convert.ToInt32(Session["UserID"].ToString());

        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            objMapData.EN = 1;
        }
        else
        {
            objMapData.EN = 0;
        }

        if (objPropMapData.EN == 1) // check for company
        {
            strO += " AND UC.IsSel = 1 AND UC.UserID =" + objPropMapData.UserID;
        }
        if (Request.QueryString["ddlDeprt"] != "-1")
        {
            strO += " AND DPDA.Type = " + objPropMapData.Department;
        }
        if (IsSalesAsigned > 0)
        {
            strO += " AND  (l.Terr = (" + "SELECT  ID FROM  Terr WHERE Name=(SELECT  fUser FROM tblUser WHERE ID = " + IsSalesAsigned + ")) OR ISNULL(l.Terr2,0) = (" + "SELECT  ID FROM Terr WHERE Name = (SELECT  fUser FROM  tblUser WHERE ID = " + IsSalesAsigned + ")))";
        }
        if (Request.QueryString["Screen"] == "ticketList")
        {
            if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
            {

                if (objPropMapData.Worker == "Active")
                {
                    strO += " AND t.fWork IN (SELECT w.ID FROM tblWork w WHERE  w.Status = 0 )";
                }
                else if (objPropMapData.Worker == "Inactive")
                {
                    strO += " AND t.fWork IN (SELECT w.ID FROM tblWork w WHERE w.Status = 1 )";
                }
                else
                {
                    strO += " AND (SELECT w.fDesc FROM tblWork w WHERE w.ID = t.fWork )= '" + objPropMapData.Worker.Replace("'", "''") + "'";
                }
            }
            if (objPropMapData.LocID != 0)
            {
                strO += " AND l.Loc = " + objPropMapData.LocID;
            }
            if (objPropMapData.CustID != 0)
            {
                strO += " AND l.Owner = " + objPropMapData.CustID;
            }
            if (objPropMapData.jobid != 0)
            {
                strO += " AND DPDA.job = " + objPropMapData.jobid;
            }
            if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
            {
                strO += " AND (ISNULL(DPDA.Charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge);
                if (objPropMapData.FilterCharge == "1")
                {
                    strO += " or ISNULL(Invoice,0) <> 0)";
                }
                else
                {
                    strO += " AND ISNULL(Invoice,0) = 0)";
                }
            }
            if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
            {
                strO += " AND (SELECT Super FROM tblWork w WHERE w.ID = t.fWork ) ='" + objPropMapData.Supervisor + "'";
            }
            if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
            {
                strO += " AND (SELECT Super FROM tblWork w WHERE w.ID = t.fWork ) <>'" + objPropMapData.NonSuper + "'";
            }
            if (objPropMapData.FilterReview != null && objPropMapData.FilterReview != string.Empty)
            {
                strO += " AND ISNULL(ClearCheck, 0) = " + Convert.ToInt32(objPropMapData.FilterReview);
            }
            if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
            {
                strO += " AND t.Workorder = '" + objPropMapData.Workorder + "'";
            }
            if (objPropMapData.Category != string.Empty && objPropMapData.Category != null)
            {

                strO += " AND t.Cat IN (" + objPropMapData.Category + ")";
            }
        }

        #endregion FILTER FOR TICKETO

        try
        {
            #region  QUERY  
            string query =
                @"SELECT
                    e.Ref AS EmpID,
                    (e.Last + ', ' + e.fFirst) AS EmpName, 
                    tbl.fDesc AS CallSign,
                    CASE e.Field WHEN 1 THEN 'Field' ELSE 'Office' END AS Type, 
                    tbl.*
			    FROM
                (
                SELECT  
                    SUM(ISNULL(Reg,0)) AS RT, 
                    SUM(ISNULL(OT,0)) AS OT , 
                    SUM(ISNULL(DT,0)) AS DT, 
                    SUM(ISNULL(TT,0)) AS TT, 
                    SUM(ISNULL(NT,0)) AS NT, 
                    SUM(ISNULL(Total,0)) AS Total, 
                    SUM(ISNULL(Zone,0)) AS Zone, 
                    SUM(ISNULL(Toll,0)) AS Toll, 
                    SUM(ISNULL(OtherE,0)) AS Misc,
                    fDesc
	            FROM 
	            (
		            SELECT  
                        w.fDesc, 
		                Reg, 
                        OT, 
                        DT, 
                        TT, 
                        NT, 
                        Total, 
                        t.Zone, 
                        t.Toll, 
                        t.OtherE  
		            FROM tblWork w
		            LEFT OUTER JOIN TicketD t ON w.ID = t.fWork 
                    INNER JOIN Loc l ON l.Loc = t.Loc
                    INNER JOIN Owner o ON l.Owner = o.ID 
                    INNER JOIN Rol r ON r.ID = l.Rol 
                    LEFT OUTER JOIN Branch B ON B.ID = r.EN
                    LEFT OUTER JOIN Elev e ON e.ID = t.Elev";

            // Check for company
            if (objPropMapData.EN == 1)
            {
                strD += " LEFT OUTER JOIN tblUserCo UC ON UC.CompanyID = r.EN ";
            }

            query += " WHERE 1 = 1 ";

            // ES-5179: Report exclude Location "Survey MR" just for TEI
            if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("Transel"))
            {
                query += " AND l.Tag NOT LIKE 'Survey MR'";
            }

            query += strD;

            if (Request.QueryString["Screen"] == "ticketList")
            {
                query +=
                    @" UNION ALL
		            SELECT  
                        ISNULL((SELECT w.fdesc FROM tblWork w WITH(NOLOCK)WHERE t.fWork = w.id),'') AS fDesc,  
                        DPDA.Reg, 
                        DPDA.OT, 
                        DPDA.DT, 
                        DPDA.TT, 
                        DPDA.NT, 
                        DPDA.Total, 
                        DPDA.Zone, 
                        DPDA.Toll, 
                        DPDA.OtherE  
		            FROM TicketO t
		            LEFT OUTER JOIN TicketDPDA DPDA ON t.ID = DPDA.ID  
                    INNER JOIN Loc l ON l.Loc = DPDA.Loc  
                    INNER JOIN Owner o ON l.Owner = o.ID 
                    INNER JOIN Rol r ON r.ID = l.Rol 
                    LEFT OUTER JOIN Branch B ON B.ID = r.EN  ";

                // Check for company
                if (objPropMapData.EN == 1)
                {
                    strO += " LEFT OUTER JOIN tblUserCo UC ON UC.CompanyID = r.EN ";
                }

                query += " WHERE  1=1 AND t.id IS NOT NULL AND t.Owner IS NOT NULL";

                // ES-5179: Report exclude Location "Survey MR" just for TEI
                if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("Transel"))
                {
                    query += " AND l.Tag NOT LIKE 'Survey MR'";
                }

                query += strO;
            }

            query += @" 
	            ) AS tickets	 
                LEFT JOIN Emp AS e ON fdesc = e.CallSign 
                GROUP BY fDesc
                ) tbl
                LEFT JOIN Emp e ON tbl.fdesc = e.CallSign AND e.Status = 0";

            #endregion

            return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, query);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["Screen"] == "ticketList")
        {
            Response.Redirect("TicketListView.aspx?fil=1");
        }
        else
        {
            Response.Redirect("Etimesheet.aspx?f=c");
        }
    }

    private StiReport LoadTimeRecapReport()
    {
        try
        {
            string reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/TimeRecapReport.mrt");
            if(!string.IsNullOrEmpty(Request.QueryString["type"]) && Request.QueryString["type"] == "all")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/TimeRecapAllHoursReport.mrt");
            }

            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            // Company info
            DataSet dsC = new DataSet();
            User objPropUser = new User();
            objPropUser.ConnConfig = Session["config"].ToString();

            dsC = objBL_User.getControl(objPropUser);

            report.RegData("dsCompany", dsC.Tables[0]);

            //Get data
            DataSet ServiceCallHistoryReportDataSet = new DataSet();
            ServiceCallHistoryReportDataSet = GetReportData();

            DataTable CompletedTickets = new DataTable();
            CompletedTickets = ServiceCallHistoryReportDataSet.Tables[0].Copy();

            report.RegData("dsTimeRecap", CompletedTickets);

            report.Dictionary.Variables["paramUsername"].Value = Session["username"].ToString();
            report.Dictionary.Variables["paramSDate"].Value = Request.QueryString["StartDate"];
            report.Dictionary.Variables["paramEDate"].Value = Request.QueryString["EndDate"];

            report.Render();

            return report;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return null;
        }
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
        RadGrid_Emails.Rebind();
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        ddlSearch.SelectedIndex = 0;
        txtSearch.Text = string.Empty;
        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
        RadGrid_Emails.Rebind();
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
    }

    //public static string[] GetContactEmails(string prefixText, int count, string contextKey)
    //{
    //    //DataTable dt = (DataTable)HttpContext.Current.Session["DistributionList"];
    //    DataTable dt = WebBaseUtility.GetContactListOnExchangeServer();

    //    List<string> txtItems = new List<string>();
    //    String dbValues;

    //    foreach (DataRow row in dt.Rows)
    //    {
    //        dbValues = AutoCompleteExtender.CreateAutoCompleteItem(row["MemberName"].ToString() + "(" + row["MemberEmail"].ToString() + ")", row["MemberEmail"].ToString());
    //        txtItems.Add(dbValues);
    //    }

    //    return txtItems.ToArray();
    //}

    private void FillDistributionList(string searchType, string searchValue)
    {
        DataTable distributionList = new DataTable();
        DataTable distributionList1 = new DataTable();
        if (!string.IsNullOrEmpty(txtTo.Text))
        {
            distributionList1.Columns.Add("MemberEmail");
            distributionList1.Columns.Add("MemberName");
            distributionList1.Columns.Add("GroupName");
            distributionList1.Columns.Add("Type");
            DataRow dr = distributionList1.NewRow();
            dr[0] = txtTo.Text;
            dr[1] = txtTo.Text;
            dr[2] = "";
            dr[3] = "";
            distributionList1.Rows.InsertAt(dr, 0);
        }
        distributionList = WebBaseUtility.GetContactListOnExchangeServer();
        distributionList.Merge(distributionList1);
        IEnumerable<DataRow> rowSources;

        var emailList = distributionList.Clone();
        switch (searchType)
        {
            case "1":
                if (searchValue != "")
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("MemberName").ToLower().Contains(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail")).OrderBy(e => e.Field<string>("GroupName"))
                                        .OrderBy(e => e.Field<string>("Type"));
                else
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("MemberName").ToLower().Equals(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail")).OrderBy(e => e.Field<string>("GroupName"))
                                        .OrderBy(e => e.Field<string>("Type"));
                break;
            case "2":
                if (searchValue != "")
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("MemberEmail").ToLower().Contains(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("GroupName")).OrderBy(e => e.Field<string>("MemberEmail"))
                                        .OrderBy(e => e.Field<string>("Type"));
                else
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("MemberEmail").ToLower().Equals(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("GroupName")).OrderBy(e => e.Field<string>("MemberEmail"))
                                    .OrderBy(e => e.Field<string>("Type"));
                break;
            case "3":
                if (searchValue != "")
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("GroupName").ToLower().Contains(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail")).OrderBy(e => e.Field<string>("GroupName"))
                                        .OrderBy(e => e.Field<string>("Type"));
                else
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("GroupName").ToLower().Equals(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail")).OrderBy(e => e.Field<string>("GroupName"))
                                    .OrderBy(e => e.Field<string>("Type"));
                break;
            case "4":
                if (searchValue != "")
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("Type").ToLower().Contains(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail")).OrderBy(e => e.Field<string>("GroupName"))
                                        .OrderBy(e => e.Field<string>("Type"));
                else
                    rowSources = (from myRow in distributionList.AsEnumerable()
                                  where myRow.Field<string>("Type").ToLower().Equals(searchValue.ToLower())
                                  select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail")).OrderBy(e => e.Field<string>("GroupName"))
                                        .OrderBy(e => e.Field<string>("Type"));
                break;
            default:
                rowSources = (from myRow in distributionList.AsEnumerable()
                              where myRow.Field<string>("GroupName").ToLower().Contains(searchValue.ToLower())
                                  || myRow.Field<string>("MemberEmail").ToLower().Contains(searchValue.ToLower())
                                  || myRow.Field<string>("MemberName").ToLower().Contains(searchValue.ToLower())
                                  || myRow.Field<string>("Type").ToLower().Contains(searchValue.ToLower())
                              select myRow).Distinct().OrderBy(e => e.Field<string>("MemberEmail"))
                                        .OrderBy(e => e.Field<string>("GroupName"))
                                        .OrderBy(e => e.Field<string>("Type"));
                break;
        }

        if (rowSources.Any())
        {
            emailList = rowSources.CopyToDataTable();
        }
        else
        {
            emailList = distributionList.Clone();
        }

        lblRecordCount.Text = emailList.Rows.Count + " Record(s) found";
        RadGrid_Emails.DataSource = emailList;
        RadGrid_Emails.VirtualItemCount = emailList.Rows.Count;

    }

    protected void RadGrid_Emails_PreRender(object sender, EventArgs e)
    {
        String filterExpression = Convert.ToString(RadGrid_Emails.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Emails_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Emails.MasterTableView.OwnerGrid.Columns)
            {
                String filterValues = column.CurrentFilterValue;
                if (filterValues != "")
                {
                    String columnName = column.UniqueName;
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = columnName;
                    filter.FilterValue = filterValues;
                    filters.Add(filter);
                }
            }

            Session["Emails_Filters"] = filters;
        }
        else
        {
            Session["Emails_FilterExpression"] = null;
            Session["Emails_Filters"] = null;
        }

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
    }

    protected void RadGrid_Emails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (!IsPostBack)
        {

            if (Session["Emails_FilterExpression"] != null && Convert.ToString(Session["Emails_FilterExpression"]) != "" && Session["Emails_Filters"] != null)
            {
                RadGrid_Emails.MasterTableView.FilterExpression = Convert.ToString(Session["Emails_FilterExpression"]);
                var filtersGet = Session["Emails_Filters"] as List<RetainFilter>;
                if (filtersGet != null)
                {
                    foreach (var _filter in filtersGet)
                    {
                        GridColumn column = RadGrid_Emails.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                        column.CurrentFilterValue = _filter.FilterValue;
                    }
                }
            }
        }

        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
    }

    private void GetSMTPUser()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.UserID = Convert.ToInt32(Session["UserID"]);
        DataSet ds = new DataSet();
        ds = objBL_User.getSMTPByUserID(objPropUser);
        if (ds.Tables[0].Rows.Count > 0)
        {
            String emailFrom = "";
            emailFrom = Convert.ToString(ds.Tables[0].Rows[0]["From"]);
            if (emailFrom == "")
            {
                SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

                string user = section.Network.UserName;
                txtFrom.Text = user;
            }
            else
            {
                txtFrom.Text = emailFrom;
            }
            txtEmailBCC.Text = Convert.ToString(ds.Tables[0].Rows[0]["BCCEmail"]);
            //txtFrom.ReadOnly = true;
        }
    }

    protected void hideModalPopupViaServerConfirm_Click(object sender, EventArgs e)
    {
        if (txtTo.Text.Trim() != string.Empty)
        {
            try
            {
                Mail mail = new Mail();
                mail.From = txtFrom.Text.Trim();
                mail.To = txtTo.Text.Split(';', ',').OfType<string>().ToList();
                if (txtCC.Text.Trim() != string.Empty)
                {
                    mail.Cc = txtCC.Text.Split(';', ',').OfType<string>().ToList();
                }

                if (txtEmailBCC.Text.Trim() != string.Empty)
                {
                    mail.Bcc = txtEmailBCC.Text.Split(';', ',').OfType<string>().ToList();
                }

                mail.Title = "Time Recap Report";
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This is report email sent from Mobile Office Manager. Please find the Time Recap Report attached.";
                }

                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(LoadTimeRecapReport(), stream, settings);
                buffer1 = stream.ToArray();

                if (hdnFirstAttachement.Value != "-1")
                {
                    mail.attachmentBytes = buffer1;
                }

                ArrayList lst = new ArrayList();
                if (ViewState["pathmailatt"] != null)
                {
                    lst = (ArrayList)ViewState["pathmailatt"];
                    foreach (string strpath in lst)
                    {
                        if (strpath != "TimeRecapReport.pdf")
                        {
                            mail.AttachmentFiles.Add(strpath);
                        }
                    }
                }

                mail.FileName = "TimeRecapReport.pdf";

                mail.DeleteFilesAfterSend = true;
                mail.RequireAutentication = false;
                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                mail.Send();

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            catch (Exception ex)
            {
                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }

    protected void lnkUploadDoc_Click(object sender, EventArgs e)
    {
        string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
        string savepath = savepathconfig + @"\mailattach\";
        string filename = FileUpload1.FileName;
        string fullpath = savepath + filename;

        if (File.Exists(fullpath))
        {
            filename = objgn.generateRandomString(4) + "_" + filename;
            fullpath = savepath + filename;
        }

        if (!Directory.Exists(savepath))
        {
            Directory.CreateDirectory(savepath);
        }
        FileUpload1.SaveAs(fullpath);


        ArrayList lstPath = new ArrayList();
        if (ViewState["pathmailatt"] != null)
        {
            lstPath = (ArrayList)ViewState["pathmailatt"];
            lstPath.Add(fullpath);
        }
        else
        {
            lstPath.Add(fullpath);
        }

        ViewState["pathmailatt"] = lstPath;
        dlAttachmentsDelete.DataSource = lstPath;
        dlAttachmentsDelete.DataBind();

        txtBody.Focus();
    }

    protected void imgDelAttach_Click(object sender, EventArgs e)
    {
        ImageButton btn = (ImageButton)sender;
        string path = btn.CommandArgument;
        if (hdnFirstAttachement.Value == path)
        {
            hdnFirstAttachement.Value = "-1";
        }
        ArrayList lstPath = (ArrayList)ViewState["pathmailatt"];
        lstPath.Remove(path);
        ViewState["pathmailatt"] = lstPath;
        dlAttachmentsDelete.DataSource = lstPath;
        dlAttachmentsDelete.DataBind();
        DeleteFile(path);
    }

    protected void btnAttachmentDel_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string path = btn.CommandArgument;
        DownloadDocument(path, Path.GetFileName(path));
    }

    private void DeleteFile(string filepath)
    {
        ////this should delete the file in the next reboot, not now.
        if (System.IO.File.Exists(filepath))
        {
            // Use a try block to catch IOExceptions, to 
            // handle the case of the file already being 
            // opened by another process. 
            try
            {
                System.IO.File.Delete(filepath);
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e.Message);
                //return;
            }
        }
    }

    private void DownloadDocument(string filePath, string DownloadFileName)
    {
        try
        {
            System.IO.FileInfo FileName = new System.IO.FileInfo(filePath);
            FileStream myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader _BinaryReader = new BinaryReader(myFile);

            try
            {
                long startBytes = 0;
                string lastUpdateTiemStamp = File.GetLastWriteTimeUtc(filePath).ToString("r");
                string _EncodedData = HttpUtility.UrlEncode(DownloadFileName, Encoding.UTF8) + lastUpdateTiemStamp;

                Response.Clear();
                Response.Buffer = false;
                Response.AddHeader("Accept-Ranges", "bytes");
                Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFileName));
                Response.AddHeader("Content-Length", (FileName.Length - startBytes).ToString());
                Response.AddHeader("Connection", "Keep-Alive");
                Response.ContentEncoding = Encoding.UTF8;

                //Send data
                _BinaryReader.BaseStream.Seek(startBytes, SeekOrigin.Begin);

                //Dividing the data in 1024 bytes package
                int maxCount = (int)Math.Ceiling((FileName.Length - startBytes + 0.0) / 1024);

                //Download in block of 1024 bytes
                int i;
                for (i = 0; i < maxCount && Response.IsClientConnected; i++)
                {
                    Response.BinaryWrite(_BinaryReader.ReadBytes(1024));
                    Response.Flush();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Response.End();
                _BinaryReader.Close();
                myFile.Close();
            }
        }
        catch (FileNotFoundException ex)
        {
            if (DownloadFileName == "TimeRecapReport.pdf")
            {
                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(LoadTimeRecapReport(), stream, settings);
                buffer1 = stream.ToArray();

                Response.Clear();
                MemoryStream ms = new MemoryStream(buffer1);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=TimeRecapReport.pdf");
                Response.Buffer = true;
                ms.WriteTo(Response.OutputStream);
                Response.End();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "FileaccessWarning", "alert('File not found.');", true);
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "FileaccessWarning", "alert('Please provide access permissions to the file path.');", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "FileerrorWarning", "alert('" + str + "');", true);
        }
    }

    private void SetAddress()
    {
        var address = WebBaseUtility.GetSignature();

        string mailBody = "Please review the attached Time Recap Report.";
        address = mailBody + Environment.NewLine + "<br />" + Environment.NewLine + "<br />" + address;

        txtBody.Text = address;
        
    }

    private void HighlightSideMenu(string MenuParent, string PageLink, string SubMenuDiv)
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl(MenuParent);
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl(PageLink);
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");

        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl(SubMenuDiv);
        div.Style.Add("display", "block");
    }
}