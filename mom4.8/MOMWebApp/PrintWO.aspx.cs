using BusinessEntity;
using BusinessLayer;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

public partial class PrintWO : System.Web.UI.Page
{
    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();
    General objPropGeneral = new General();
    BL_General objBL_General = new BL_General();
    GeneralFunctions objGen = new GeneralFunctions();
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("timeout.htm");
            return;
        }

        if (!IsPostBack)
        {
            PrintReport();
        }
    }

    private void PrintReport()
    {
        
        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.LocID = Convert.ToInt32(Request.QueryString["lid"].ToString());
        DataSet dsloc = new DataSet();
        dsloc = objBL_User.getLocationByID(objPropUser);
        
        DataSet ds = new DataSet();
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.Workorder = Request.QueryString["wo"].ToString();
         DataTable dtuserinfo = (DataTable)Session["userinfo"];
         if (dtuserinfo.Rows[0]["openticket"].ToString() == "0")
             objMapData.CustID = Convert.ToInt32(Session["custid"].ToString());
         else
             objMapData.CustID = 0;
        objMapData.LocID = Convert.ToInt32(Request.QueryString["lid"].ToString());
        ds = objBL_MapData.GetTicketbyWorkorder(objMapData);
       
        var distinctValues = ds.Tables[0].AsEnumerable()
                        .Select(row => new
                        {
                            comp = row.Field<int>("comp")                            
                        })
                        .Distinct().ToArray();
        string status = (distinctValues.Any(x => x.comp == 1))? "Closed" : "Open";

        string startdate = string.Empty;
        string nextdate = string.Empty;
        string Reason = string.Empty;
        string who = string.Empty;
        string cdate = string.Empty;
        string calldate = string.Empty;
        string calltime = string.Empty;
        DataSet dsWODate = new DataSet();
        dsWODate = objBL_MapData.GetWorkorderDate(objMapData);
        
        if (dsWODate.Tables[0].Rows.Count > 0)
        {
            startdate = dsWODate.Tables[0].Rows[0]["edate"].ToString();
            Reason = dsWODate.Tables[0].Rows[0]["fdesc"].ToString();
            who = dsWODate.Tables[0].Rows[0]["who"].ToString();
            cdate = dsWODate.Tables[0].Rows[0]["cdate"].ToString();
            if (cdate != string.Empty)
            {
                calldate = Convert.ToDateTime(cdate).ToShortDateString();
                calltime = Convert.ToDateTime(cdate).ToShortTimeString();
            }
        }
        if (dsWODate.Tables.Count > 1)
        {
            if (dsWODate.Tables[1].Rows.Count > 0)
                nextdate = dsWODate.Tables[1].Rows[0]["edate"].ToString();
        }

        DataSet dsC = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        if (Session["MSM"].ToString() != "TS")
        {
            dsC = objBL_User.getControl(objPropUser);
        }
        else
        {
            objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["lid"].ToString());
            dsC = objBL_User.getControlBranch(objPropUser);
        }

        ReportViewer1.LocalReport.DataSources.Clear();
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DsTicket", ds.Tables[0]));
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsLocation", dsloc.Tables[0]));
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsEquipDetails", ds.Tables[1]));
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsCompany", dsC.Tables[0]));
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dsDwork", ds.Tables[2]));

        string reportPath = "Reports/WorkOrder.rdlc";
        ReportViewer1.LocalReport.ReportPath = reportPath;

        List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("WorkOrder", Request.QueryString["wo"].ToString()));
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("startdate", startdate));
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("nextdate", nextdate));
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("reason", Reason));
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("status", status));
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("who", who));
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("calldate", calldate));
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("calltime", calltime));

        ReportViewer1.LocalReport.SetParameters(param1);
        ReportViewer1.LocalReport.Refresh();
    }          
}