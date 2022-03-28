using BusinessEntity;
using BusinessLayer;
using Stimulsoft.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;

public partial class ReceivePOReport : System.Web.UI.Page
{
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    GeneralFunctions objGeneralFunctions = new GeneralFunctions();
    BL_Report bL_Report = new BL_Report();

    BL_ReportsData objBL_ReportsData = new BL_ReportsData();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    PO _objPO = new PO();
    BL_Bills _objBLBills = new BL_Bills();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Session.Remove("GridFilters");
        Response.Redirect("ManageReceivePO.aspx");
    }

    protected void StiWebViewerReceivePO_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        string reportPathStimul = string.Empty;    
        reportPathStimul = Server.MapPath("StimulsoftReports/ReceivePOItemReport.mrt");
        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        //report.Compile();

        e.Report = report;
    }

    protected void StiWebViewerReceivePO_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        var report = e.Report;
        try
        {
            DataTable ds = (DataTable)Session["opps"];
            string _asOfDate;
            if (Request.QueryString["ID"] != null)
            {
                DateTime _endDate = DateTime.Now;
                _asOfDate = "As of " + _endDate.ToString("MMMM dd, yyyy");
            }
            else
            {
                DateTime _endDate = Convert.ToDateTime(Request.QueryString["E"].ToString());
                _asOfDate = "As of " + _endDate.ToString("MMMM dd, yyyy");
            }

            DataSet dsC = new DataSet();

            objPropUser.ConnConfig = Session["config"].ToString();
            if (Session["MSM"].ToString() != "TS")
            {
                dsC = objBL_User.getControl(objPropUser);
            }
            else
            {
                objPropUser.LocID = Convert.ToInt32(ds.Rows[0]["loc"]);
                dsC = objBL_User.getControlBranch(objPropUser);
            }

            DataTable dtOpportunities = BindReceivePO().Copy();

            if (Request.QueryString["ID"] != null)
            {
                report["paramSDate"] = DateTime.Now.ToString("MMMM dd, yyyy");
            }
            else
            {
                report["paramSDate"] = Request.QueryString["S"].ToString();
            }
            report["paramEDate"] = _asOfDate;
            report["paramUsername"] = Session["Username"].ToString();
            report.RegData("Opportunities", dtOpportunities);
            report.RegData("CompanyDetails", dsC.Tables[0]);

            report.Render();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private DataTable BindReceivePO()
    {
        try
        {
            DataSet _dsPJ = new DataSet();
            _objPO.ConnConfig = Session["config"].ToString();
            _objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());

            if (Request.QueryString["ID"] != null)
            {
                _objPO.SearchBy = "r.id"; ;
                _objPO.SearchValue = Request.QueryString["ID"].ToString();
                _objPO.ReceiveStartDate = "";
                _objPO.ReceiveEndDate = "";

                _dsPJ = _objBLBills.GetListReceivePOBySearchByID(_objPO);
            }
            else
            {
                _objPO.SearchBy = Request.QueryString["SBy"].ToString();
                _objPO.SearchValue = Request.QueryString["SVal"].ToString();

                _objPO.ReceiveStartDate = Request.QueryString["S"].ToString();
                _objPO.ReceiveEndDate = Request.QueryString["E"].ToString();


                if (Session["CmpChkDefault"].ToString() == "1")
                {
                    _objPO.EN = 1;
                }
                else
                {
                    _objPO.EN = 0;
                }

                List<RetainFilter> filters = Session["GridFilters"] != null ? (List<RetainFilter>)Session["GridFilters"] : new List<RetainFilter>();

                _dsPJ = _objBLBills.GetListReceivePOProjectBySearch(_objPO, filters);
            }

            // Status open/closed filter 
            DataTable filterdt = new DataTable();
            DataSet FilteredDs = new DataSet();

            if (Request.QueryString["ID"] != null)
            {
                FilteredDs = _dsPJ.Copy();
            }
            else
            {
                if (Convert.ToBoolean(Request.QueryString["IsCheck"].ToString()))
                {
                    //lnkChk.Checked = true;
                    FilteredDs = _dsPJ.Copy();
                }
                else
                {
                    if (_dsPJ.Tables[0].Rows.Count > 0)
                    {
                        DataRow[] dr = _dsPJ.Tables[0].Select("StatusName='Open'");
                        if (dr.Length > 0)
                        {
                            filterdt = dr.CopyToDataTable();
                            FilteredDs.Tables.Add(filterdt);
                        }
                        else
                        {
                            FilteredDs = _dsPJ.Clone();
                        }
                    }
                    else
                    {
                        FilteredDs = _dsPJ.Copy();
                    }
                } 
            }
            return FilteredDs.Tables[0];
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}