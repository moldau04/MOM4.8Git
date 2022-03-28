using BusinessEntity;
using BusinessLayer;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Reports : System.Web.UI.Page
{
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();

    GeneralFunctions objGeneralFunctions = new GeneralFunctions();

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    BL_ReportsData objBL_ReportsData = new BL_ReportsData();
    ReportsEnum objReportEnum = new ReportsEnum();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["type"]))
            {
                GetReport(Request.QueryString["type"].ToString());
            }
        }
        //else
        //{
        //    GetReport("Customer");
        //}

        Permission();
    }

    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("cstmMgr");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("cstmlink");
        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkCustomersSmenu");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
        }

        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx");
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }
    }

    private DataSet GetReportsName(string _reportType)
    {
        DataSet dsGetReports = new DataSet();
        try
        {
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
            objProp_User.Type = _reportType;
            dsGetReports = objBL_ReportsData.GetStockReports(objProp_User);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return dsGetReports;
    }
    protected void lnkRepCustMgr_Click(object sender, EventArgs e)
    {
        string _reportType = "Customer";
        GetReport(_reportType);
        //Response.Redirect("~/Reports.aspx?type=" + _reportType);
    }

    private Array GetEnumByReportType(string _reportType)
    {
        var EnumValue = (Array)null;
        switch (_reportType)
        {
            case "Customer":
                EnumValue = Enum.GetValues(typeof(ReportsEnum.Customer));
                break;

            case "Recurring":
                EnumValue = Enum.GetValues(typeof(ReportsEnum.Recurring));
                break;
        }
        return EnumValue;
    }

    private void GetReport(string _reportType)
    {
        DataSet dsGetReports = new DataSet();
        objProp_User.DBName = Session["dbname"].ToString();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objProp_User.Type = "Customer";
        //PlaceHolder1.Controls.Remove(Literal.)

        StringBuilder html = new StringBuilder();
        Array EnumValues = GetEnumByReportType(_reportType);
        if (EnumValues != null)
        {
            foreach (var _enumValue in EnumValues)
            {
                html.Append("<div class='col-sm-12 col-md-12 col-lg-12'><div class='cr-topTitle'><strong>" + _enumValue + "</strong></div></div>");
                html.Append("<div class='clearfix'></div>");
                DataSet dsReportData = GetReportsName(_enumValue.ToString());
                if (dsReportData.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsReportData.Tables[0].Rows.Count; i++)
                    {
                        html.Append("<div class='col-sm-3 col-md-3 col-lg-3'>");
                        html.Append("<div class='cr-box'>");
                        html.Append("<div class='cr-title'>" + dsReportData.Tables[0].Rows[i]["ReportName"] + "</div>");
                        html.Append("<div class='cr-img'>");
                        html.Append("<img src='images/ReportImages/StaticPreview.png' alt=''>");
                        html.Append("</div>");
                        html.Append("<div class='cr-date'>");
                        html.Append("<div class='date'>Dates: <span>12/02/2015</span></div>");
                        html.Append("<div class='date'>Today: <span>12/02/2015</span></div>");
                        html.Append("<div class='cr-iocn'>");
                        html.Append("<a href='CustomersReport.aspx?reportId=" + dsReportData.Tables[0].Rows[i]["Id"] + "&type=" + dsReportData.Tables[0].Rows[i]["ReportType"] + "&reportName=" + dsReportData.Tables[0].Rows[i]["ReportName"] + "'><img src='images/ReportImages/cr-iocn1.png' alt=''></a>");
                        html.Append("<a href='#'><img src='images/ReportImages/cr-iocn2.png' alt=''></a>");
                        html.Append("<a href='#'><img src='images/ReportImages/cr-iocn3.png' alt=''></a>");
                        html.Append("<a href='#'><img src='images/ReportImages/cr-iocn4.png' alt=''></a>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");
                        html.Append("</div>");
                    }
                }
            }
        }
        else
        {
            html.Append("<div>No Reports Found</div>");
        }
        PlaceHolder1.Controls.Add(new Literal { Text = html.ToString() });
        
    }
    protected void lnkRepRecMgt_Click(object sender, EventArgs e)
    {
        string _reportType = "Recurring";
        GetReport(_reportType);
        //Response.Redirect("~/Reports.aspx?type=" + _reportType);
    }
}