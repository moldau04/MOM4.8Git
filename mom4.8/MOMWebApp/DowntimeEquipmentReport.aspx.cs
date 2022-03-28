using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using BusinessEntity.Utility;
using BusinessLayer;
using MOMWebApp;
using Stimulsoft.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class DowntimeEquipmentReport : System.Web.UI.Page
{
    #region ::Declaration::
    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();
    GeneralFunctions objGeneralFunctions = new GeneralFunctions();

    protected DataTable dtBomType = new DataTable();
    protected DataTable dtBomItem = new DataTable();
    protected DataTable dtCurrency = new DataTable();
    JobT _objJob = new JobT();
    BL_Job objBL_Job = new BL_Job();

    Wage _objWage = new Wage();
    BL_User objBL_User = new BL_User();
    User _objUser = new User();
    MapData objMapData = new MapData();
    BL_MapData objBL_MapData = new BL_MapData();
    protected DataTable dtMat = new DataTable();
    protected DataTable dtLab = new DataTable();

    EstimateForm objEF = new EstimateForm();
    Lead leadData = new Lead();
    BL_Lead objBL_Lead = new BL_Lead();

    STax staxData = new STax();
    BL_STax objBL_STax = new BL_STax();

    BL_EstimateForm objBL_EF = new BL_EstimateForm();

    EstimateTemplate objET = new EstimateTemplate();
    BL_EstimateTemplate objBL_ET = new BL_EstimateTemplate();

    Contracts objContract = new Contracts();
    BL_Contracts objBLContracts = new BL_Contracts();

    User objPropUser = new User();

    Customer objPropCustomer = new Customer();
    BL_Report bL_Report = new BL_Report();

    //API Variables
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    GetCompanyDetailsParam _getCompanyDetails = new GetCompanyDetailsParam();
    GetSMTPByUserIDParam _getSMTPByUserID = new GetSMTPByUserIDParam();
    getConnectionConfigParam _getConnectionConfig = new getConnectionConfigParam();
    GetEquipmentShutdownForReportParam _GetEquipmentShutdownForReport = new GetEquipmentShutdownForReportParam();
    GetEquipShutdownActivityForReportParam _GetEquipShutdownActivityForReport = new GetEquipShutdownActivityForReportParam();

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            HighlightSideMenu("cstmMgr", "lnkEquipmentsSMenu", "cstmMgrSub");

            if (!string.IsNullOrEmpty(Request["sd"]))
            {
                txtFromDate.Text = HttpUtility.UrlDecode(Request.QueryString["sd"]);
            }
            else
            {
                txtFromDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("MM/dd/yyyy");
            }

            if (!string.IsNullOrEmpty(Request["ed"]))
            {
                txtToDate.Text = HttpUtility.UrlDecode(Request.QueryString["ed"]);
            }
            else
            {
                txtToDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            }
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("Equipments.aspx");
    }

    protected void StiWebViewerDowntimeReport_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        e.Report = GetReportDataEquipmentShutdown();
    }

    protected void StiWebViewerDowntimeReport_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {

    }

    private StiReport GetReportDataEquipmentShutdown()
    {
        StiReport report = new StiReport();

        try
        {
            var reportPathStimul = Server.MapPath("StimulsoftReports/DowntimeEquipmentReport.mrt");
            report.Load(reportPathStimul);
            report.CacheAllData = true;

            var companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());
            
            objProp_Customer.ConnConfig = Session["config"].ToString();

            if (!string.IsNullOrEmpty(Request["sd"]))
            {
                objProp_Customer.StartDate = Request.QueryString["sd"];
            }
            else
            {
                objProp_Customer.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToShortDateString();
            }

            var endDate = DateTime.Now;
            if (!string.IsNullOrEmpty(Request["ed"]))
            {
                endDate = Convert.ToDateTime(Request.QueryString["ed"]);
                objProp_Customer.EndDate = endDate.AddDays(1).ToShortDateString();
            }
            else
            {
                objProp_Customer.EndDate = endDate.Date.AddDays(1).ToShortDateString();
            }

            if (!string.IsNullOrEmpty(Request.QueryString["stype"]) && !string.IsNullOrEmpty(Request.QueryString["stext"]))
            {
                objProp_Customer.SearchBy = HttpUtility.UrlDecode(Request.QueryString["stype"].ToLower());
                objProp_Customer.SearchValue = HttpUtility.UrlDecode(Request.QueryString["stext"]);
            }

            var inclInactive = false;
            if (!string.IsNullOrEmpty(Request.QueryString["inclInactive"]))
            {
                inclInactive = Convert.ToBoolean(Request.QueryString["inclInactive"]);
            }

            List<RetainFilter> filters = new List<RetainFilter>();
            if (Session["Equip_Filters"] != null)
            {
                filters = (List<RetainFilter>)Session["Equip_Filters"];
            }

            DataSet ds = objBL_Customer.GetDowntimeEquipmentReport(objProp_Customer, filters, inclInactive);

            report.RegData("CompanyTable", companyInfo.Tables[0]);
            report.RegData("dtEquipShutdownHistory", ds.Tables[0]);

            //Set parameter
            report.Dictionary.Variables["username"].Value = Session["Username"].ToString();
            report.Dictionary.Variables["startDate"].Value = objProp_Customer.StartDate;
            report.Dictionary.Variables["endDate"].Value = endDate.ToShortDateString();
            report.Render();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
        return report;
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        var url = Request.Url.ToString();

        url = RemoveQueryStringByKey(url, "sd");
        url = RemoveQueryStringByKey(url, "ed");
        url += "&sd=" + txtFromDate.Text + "&ed=" + txtToDate.Text;

        Response.Redirect(url);
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

    public string RemoveQueryStringByKey(string url, string key)
    {
        var uri = new Uri(url);

        // this gets all the query string key value pairs as a collection
        var newQueryString = HttpUtility.ParseQueryString(uri.Query);

        // this removes the key if exists
        newQueryString.Remove(key);

        // this gets the page path from root without QueryString
        string pagePathWithoutQueryString = uri.GetLeftPart(UriPartial.Path);

        return newQueryString.Count > 0
            ? String.Format("{0}?{1}", pagePathWithoutQueryString, newQueryString)
            : pagePathWithoutQueryString;
    }
}