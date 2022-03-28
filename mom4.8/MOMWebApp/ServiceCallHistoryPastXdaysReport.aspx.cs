using BusinessEntity;
using BusinessLayer;
using Microsoft.ApplicationBlocks.Data;
using Stimulsoft.Report;
using System;
using System.Data;

public partial class ServiceCallHistoryPastXdaysReport : System.Web.UI.Page
{

    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    String CompanyAddress = "";
    String DefaultCategory = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
            return;
        }

        if (!IsPostBack)
        {
            #region Report Design Button Hide
            if (Convert.ToString(Session["type"]) == "am")
            {
                StiWebViewerServiceCallHistoryPastXdaysReport.ShowDesignButton = true;
            }
            else
            {
                StiWebViewerServiceCallHistoryPastXdaysReport.ShowDesignButton = false;
            }

            #endregion

        }
    }
    private void GetAddress()
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.DBName = Session["dbname"].ToString();
        CompanyAddress = objBL_User.getCompanyAddress(objProp_User);
    }

    protected void StiWebViewerServiceCallHistoryPastXdaysReport_DesignReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {

    }

    protected void StiWebViewerServiceCallHistoryPastXdaysReport_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        int Days = 30;

        int.TryParse(Request.QueryString["PastXDays"], out Days); 

        string reportPath = Server.MapPath("StimulsoftReports/Tickets/ServiceCallHistoryReportPastXDays.mrt");

        GetAddress();
        string lblTicket = " >= 2 Service calls";
        if (Days == 7) { lblTicket = " > = 2 Service calls"; }
        if (Days == 30) { lblTicket = " > = to 5 Service calls"; }
        if (Days == 90) { lblTicket = " > = 10 Service calls"; }
        if (Days == 180) { lblTicket = " > = 15 Service calls"; }
        if (Days == 365) { lblTicket = " > = 20 Service calls"; }
        StiReport report = new StiReport();
        report.Load(reportPath);
        report.Compile();
        report["ReportH"] = "CALL HISTORY PAST " + Days + " DAYS";  
        report["paramUsername"] = Session["username"];
        report["paramSDate"] = DateTime.Now.AddDays(-Days).ToString("MM/dd/yyy");
        report["paramEDate"] = DateTime.Now.AddDays(-1).ToString("MM/dd/yyy");
        report["lblTicket"] = lblTicket;
        e.Report = report; 
    }

    protected void StiWebViewerServiceCallHistoryPastXdaysReport_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        var report = e.Report;

        int Days = 30;

        int GreaterorequalTo = 2;

       

        int.TryParse(Request.QueryString["PastXDays"], out Days);

        if (Days == 7) { GreaterorequalTo = 2; }
        if (Days == 30) { GreaterorequalTo = 5; }
        if (Days == 90) { GreaterorequalTo = 10; }
        if (Days == 180) { GreaterorequalTo = 15; }
        if (Days == 365) { GreaterorequalTo = 20; } 
        

        string stdate = DateTime.Now.AddDays(-Days).ToString("MM/dd/yyyy") + " 00:00:00";

        string enddate = DateTime.Now.ToString("MM/dd/yyyy") + " 23:59:59";

        DateTime StartDate = Convert.ToDateTime(stdate);

        DateTime EndDate = Convert.ToDateTime(enddate);

        DataSet ServiceCallHistoryReportDataSet = new DataSet("ServiceCallHistoryReport");

        objMapData.ConnConfig = Session["config"].ToString();

        objMapData.StartDate = StartDate;

        objMapData.EndDate = EndDate;

        objMapData.UserID = Convert.ToInt32(Session["UserID"].ToString());

        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            objMapData.EN = 1;
        }
        else
        {
            objMapData.EN = 0;
        }

        ServiceCallHistoryReportDataSet = GetReportData(objMapData, new GeneralFunctions().GetSalesAsigned(), GreaterorequalTo);

        DataSet ServiceCall = new DataSet();

        DataTable CompletedTickets = new DataTable();

        CompletedTickets = ServiceCallHistoryReportDataSet.Tables[0].Copy();

        CompletedTickets.TableName = "ServiceCallHistoryReport";

        ServiceCall.Tables.Add(CompletedTickets);

        report.RegData("ServiceCallHistoryReport", ServiceCall);

        /////Comp info

        DataSet dsC = new DataSet();
        User objPropUser = new User();
        objPropUser.ConnConfig = Session["config"].ToString();
        dsC = objBL_User.getControl(objPropUser); 
        DataSet dsCompany = new DataSet();
        DataTable tblcomp = new DataTable();
        tblcomp= dsC.Tables[0].Copy();
        tblcomp.TableName = "dsCompany";
        dsCompany.Tables.Add(tblcomp); 
        report.RegData("dsCompany", dsCompany);

    }

    public DataSet GetReportData(MapData objPropMapData, int IsSalesAsigned = 0, int GreaterorequalTo=2)
    {
        try
        {

            #region  QUERY  
            string query = @"   CREATE TABLE #tempTicketD
           (  TicketID  INT,   Edate  DATETIME,   WORKER    VARCHAR(50), Category   VARCHAR(50),    ElevID    INT,  UNIT   VARCHAR(20),  OwnerID   INT,  CUSTOMERNAME  VARCHAR(75),    LocID     INT,  Locname   VARCHAR(75)  )  
            ----TicketD
          INSERT INTO #tempTicketD  
          Select  D.ID,D.EDate ,(SELECT isNULL(name,'Unasssigned') from route where ID=l.Route), D.Cat ,E.ID, E.Unit,l.Owner,(SELECT NAME FROM ROL WHERE ID=O.Rol)      Customername,D.Loc,L.Tag from TicketD d
          inner join multiple_equipments me on me.ticket_id=d.ID
          inner join Elev e on e.ID=me.elev_id
          inner join Loc l on l.Loc=d.Loc
          inner join rol r on r.id=l.rol
          inner join OWNER o on l.Owner=o.ID";
            if (objPropMapData.EN == 1) // check for company
            {
                query += " LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN ";
            }

            query += "  Where d.Cat = 'Service Call' ";

            query += " and isnull(D.edate,D.cdate) >='" + objPropMapData.StartDate + "'";
            query += " and isnull(D.edate,D.cdate) <'" + objPropMapData.EndDate + "'";

            if (objPropMapData.EN == 1) // check for company
            {
                query += " and UC.IsSel = 1 and UC.UserID =" + objPropMapData.UserID;
            }
            ///TicketO
            query += @" INSERT INTO #tempTicketD 
            Select  D.ID,D.EDate ,(SELECT isNULL(name, 'Unasssigned') from route where ID = l.Route), D.Cat ,E.ID, E.Unit,l.Owner,(SELECT NAME FROM ROL WHERE ID = O.Rol)      Customername,D.LID as Loc,L.Tag from TicketO d
          inner join multiple_equipments me on me.ticket_id = d.ID
          inner join Elev e on e.ID = me.elev_id
          inner join Loc l on l.Loc = d.LID
          inner join rol r on r.id=l.rol
          inner join OWNER o on l.Owner = o.ID";
            if (objPropMapData.EN == 1) // check for company
            {
                query += " LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN ";
            }

            if (IsSalesAsigned > 0)
            {
                query += " AND  (  l.Terr=(" + "select id FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSalesAsigned + ")) or  isnull(l.Terr2,0)=(" + "select id FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSalesAsigned + "))) ";
            }
            query += " Where d.Cat = 'Service Call' ";
            
            query += " and isnull(D.edate,D.cdate) >='" + objPropMapData.StartDate + "'";
            query += " and isnull(D.edate,D.cdate) <'" + objPropMapData.EndDate + "'";
             

            if (objPropMapData.EN == 1) // check for company
            {
                query += " and UC.IsSel = 1 and UC.UserID =" + objPropMapData.UserID;
            }
            if (IsSalesAsigned > 0)
            {
                query += " AND  (  l.Terr=(" + "select id FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSalesAsigned + ")) or  isnull(l.Terr2,0)=(" + "select id FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=" + IsSalesAsigned + "))) ";
            }


            query += " SELECT D.Locname,d.ElevID,D.UNIT,D.WORKER as RouteMechanic,Count(D.TicketID) TicketCount FROM #tempTicketD   D  GROUP BY d.Locname,d.ElevID ,d.UNIT ,D.WORKER ";

            query += "  having Count(D.TicketID) >= "+ GreaterorequalTo; 


            query += "  DROP TABLE #tempTicketD";
            #endregion

            return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, query);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}