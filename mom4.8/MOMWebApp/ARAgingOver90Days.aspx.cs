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
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Web;
using Telerik.Web.UI;

public partial class ARAgingOver90Days : System.Web.UI.Page
{
    #region Variables
    GeneralFunctions objgn = new GeneralFunctions();

    Contracts objContract = new Contracts();
    BL_Contracts objBL_Contracts = new BL_Contracts();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    Customer objPropCustomer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    BL_Report objBL_Report = new BL_Report();

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request["stype"]))
                {
                    ddlType.SelectedValue = HttpUtility.UrlDecode(Request.QueryString["stype"]);
                }
                else
                {
                    ddlType.SelectedValue = "2";
                }

                if (!string.IsNullOrEmpty(Request["sdate"]))
                {
                    txtSearchDate.Text = HttpUtility.UrlDecode(Request.QueryString["sdate"]);
                }
                else
                {
                    txtSearchDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                }

                if (!string.IsNullOrEmpty(Request["type"]) && Request["type"] == "summary")
                {
                    rdExpandAll.Checked = false;
                    rdCollapseAll.Checked = true;
                }
                else
                {
                    rdExpandAll.Checked = true;
                    rdCollapseAll.Checked = false;
                }

                if (!string.IsNullOrEmpty(Request["pHide"]) && Convert.ToBoolean(Request.QueryString["pHide"]))
                {
                    chkHidePartial.Checked = true;
                }
                else
                {
                    chkHidePartial.Checked = false;
                }

                if (!string.IsNullOrEmpty(Request["inclNotes"]) && Convert.ToBoolean(Request["inclNotes"]))
                {
                    chkIncludeNotes.Checked = true;
                }
                else
                {
                    chkIncludeNotes.Checked = false;
                }

                if (!string.IsNullOrEmpty(Request["creditFlag"]) && Convert.ToBoolean(Request["creditFlag"]))
                {
                    chkCreditFlag.Checked = true;
                }
                else
                {
                    chkCreditFlag.Checked = false;
                }
                if (!string.IsNullOrEmpty(Request["showRetainage"]) && Convert.ToBoolean(Request["showRetainage"]))
                {
                    chkRetainageAmount.Checked = true;
                }
                else
                {
                    chkRetainageAmount.Checked = false;
                }

                if (Request.QueryString["page"] != null)
                {
                    if (Request.QueryString["page"] == "invoices")
                    {
                        HighlightSideMenu("acctMgr", "lnkInvoicesSMenu", "billMgrSub");
                    }
                    else
                    {
                        HighlightSideMenu("cstmMgr", "lnkCollections", "cstmMgrSub");
                    }
                }
                else
                {
                    HighlightSideMenu("acctMgr", "lnkInvoicesSMenu", "billMgrSub");
                }

                GetSMTPUser();
                SetAddress();
                string FileName = "ARAgingOver90DaysReport.pdf";
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
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"] == "invoices")
            {
                Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?fil=1");
            }
            else
            {
                if (Request.QueryString["lid"] != null)
                {
                    Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + Request.QueryString["lid"].ToString());
                }
                else
                {
                    Response.Redirect(Request.QueryString["page"].ToString() + ".aspx");
                }
            }
        }
        else
        {
            Response.Redirect("home.aspx");
        }
    }

    protected void StiWebViewerARAging_GetReport(object sender, StiReportDataEventArgs e)
    {
        if (e.RequestParams.ExportFormat == StiExportFormat.Csv && System.Web.Configuration.WebConfigurationManager.AppSettings["CustomerName"].Trim().ToUpper() == "gable".ToUpper())
        {            
            e.Report = LoadExportCSVformte();
        }
        else
        {
            e.Report = LoadReport();
        }
            
    }

    protected void StiWebViewerARAging_GetReportData(object sender, StiReportDataEventArgs e)
    {

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        var type = rdExpandAll.Checked ? "detail" : "summary";

        var url = "ARAgingOver90Days.aspx?stype=" + ddlType.SelectedValue + "&sdate=" + txtSearchDate.Text + "&type=" + type + "&pHide=" + chkHidePartial.Checked + "&inclNotes=" + chkIncludeNotes.Checked + "&creditFlag=" + chkCreditFlag.Checked + "&showRetainage=" + chkRetainageAmount.Checked;

        if (Request.QueryString["uid"] != null)
        {
            url += "&uid=" + Request.QueryString["uid"].ToString();
        }

        if (Request.QueryString["lid"] != null)
        {
            url += "&lid=" + Request.QueryString["lid"].ToString();
        }

        if (Request.QueryString["page"] != null)
        {
            url += "&page=" + Request.QueryString["page"].ToString();
        }

        Response.Redirect(url);
    }

    protected void rdExpCollAll_CheckedChanged(object sender, EventArgs e)
    {
        var type = rdExpandAll.Checked ? "detail" : "summary";

        var url = "ARAgingOver90Days.aspx?stype=" + ddlType.SelectedValue + "&sdate=" + txtSearchDate.Text + "&type=" + type + "&pHide=" + chkHidePartial.Checked + "&inclNotes=" + chkIncludeNotes.Checked + "&creditFlag=" + "&showRetainage=" + chkRetainageAmount.Checked;

        if (Request.QueryString["uid"] != null)
        {
            url += "&uid=" + Request.QueryString["uid"].ToString();
        }

        if (Request.QueryString["lid"] != null)
        {
            url += "&lid=" + Request.QueryString["lid"].ToString();
        }

        if (Request.QueryString["page"] != null)
        {
            url += "&page=" + Request.QueryString["page"].ToString();
        }

        Response.Redirect(url);
    }

    private StiReport LoadReport()
    {
        try
        {
            string reportPathStimul = Server.MapPath("StimulsoftReports/ARAgingOver90DaysReport.mrt");

            //if (!string.IsNullOrEmpty(Request["showRetainage"]) && Request["showRetainage"] == "True")
            //{
            //    reportPathStimul = Server.MapPath("StimulsoftReports/ARAgingReport_Retainage.mrt");
            //    if (!string.IsNullOrEmpty(Request["type"]) && Request["type"] == "summary")
            //    {
            //        reportPathStimul = Server.MapPath("StimulsoftReports/ARAgingSummaryReport_Retainage.mrt");
            //    }
            //}
            //else
            //{
            //reportPathStimul = Server.MapPath("StimulsoftReports/ARAgingReport.mrt");
            if (!string.IsNullOrEmpty(Request["type"]) && Request["type"] == "summary")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/ARAgingOver90DaysSummaryReport.mrt");
            }
            //}

            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            //Get data
            var connString = Session["config"].ToString();
            objPropUser.ConnConfig = connString;

            DataSet companyInfo = new DataSet();
            companyInfo = objBL_Report.GetCompanyDetails(Session["config"].ToString());

            report.RegData("CompanyDetails", companyInfo.Tables[0]);

            objContract.ConnConfig = connString;
            objContract.isDBTotalService = false;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["isDBTotalService"]))
            {
                objContract.isDBTotalService = Convert.ToBoolean(ConfigurationManager.AppSettings["isDBTotalService"]);
            }

            if (Request.QueryString["uid"] != null)
            {
                objContract.Owner = Convert.ToInt32(Request.QueryString["uid"].ToString());
            }

            if (!string.IsNullOrEmpty(Request["sdate"]))
            {
                objContract.Date = Convert.ToDateTime(Request.QueryString["sdate"]);
            }
            else
            {
                objContract.Date = DateTime.Now;
            }

            objContract.HidePartial = false;
            if (!string.IsNullOrEmpty(Request["pHide"]))
            {
                objContract.HidePartial = Convert.ToBoolean(Request.QueryString["pHide"]);
            }

            var creditFlag = 0;
            if (!string.IsNullOrEmpty(Request["creditFlag"]) && Convert.ToBoolean(Request.QueryString["creditFlag"]))
            {
                creditFlag = 1;
            }

            DataSet ds = new DataSet();
            if (!string.IsNullOrEmpty(Request["stype"]) && Request["stype"] == "1")
            {
                ds = objBL_Contracts.GetARAgingOver90DaysReport(objContract, creditFlag);
            }
            else
            {
                ds = objBL_Contracts.GetARAgingByAsOfDateOver90DaysReport(objContract, creditFlag);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                report.RegData("ReportData", ds.Tables[0]);
            }

            if (!string.IsNullOrEmpty(Request["inclNotes"]) && Convert.ToBoolean(Request["inclNotes"]))
            {
                objPropCustomer.ConnConfig = connString;
                var dsNotes = objBL_Customer.GetRecentCollectionNotes(objPropCustomer);

                if (dsNotes != null && dsNotes.Tables.Count > 1)
                {
                    report.RegData("LocationNotes", dsNotes.Tables[0]);
                    report.RegData("CustomerNotes", dsNotes.Tables[1]);
                }

                report.Dictionary.Variables["IncludeNotes"].Value = Request["inclNotes"];
            }

            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();
            report.Dictionary.Variables["EndDate"].Value = objContract.Date.ToLongDateString();
            report.CacheAllData = true;
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
    private StiReport LoadExportCSVformte()
    {
        try
        {
            string reportPathStimul = Server.MapPath("StimulsoftReports/ARAgingOver90DaysCSVFormate.mrt");
            if (!string.IsNullOrEmpty(Request["type"]) && Request["type"] == "summary")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/ARAgingOver90DaysSummaryReportCSVFormate.mrt");
            }

            StiReport report = new StiReport();
            report.Load(reportPathStimul);

            //report.Compile();

            //Get data
            var connString = Session["config"].ToString();
            objPropUser.ConnConfig = connString;

            DataSet companyInfo = new DataSet();
            companyInfo = objBL_Report.GetCompanyDetails(Session["config"].ToString());

            report.RegData("CompanyDetails", companyInfo.Tables[0]);

            objContract.ConnConfig = connString;
            objContract.isDBTotalService = false;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["isDBTotalService"]))
            {
                objContract.isDBTotalService = Convert.ToBoolean(ConfigurationManager.AppSettings["isDBTotalService"]);
            }

            if (Request.QueryString["uid"] != null)
            {
                objContract.Owner = Convert.ToInt32(Request.QueryString["uid"].ToString());
            }

            if (!string.IsNullOrEmpty(Request["sdate"]))
            {
                objContract.Date = Convert.ToDateTime(Request.QueryString["sdate"]);
            }
            else
            {
                objContract.Date = DateTime.Now;
            }

            objContract.HidePartial = false;
            if (!string.IsNullOrEmpty(Request["pHide"]))
            {
                objContract.HidePartial = Convert.ToBoolean(Request.QueryString["pHide"]);
            }

            var creditFlag = 0;
            if (!string.IsNullOrEmpty(Request["creditFlag"]) && Convert.ToBoolean(Request.QueryString["creditFlag"]))
            {
                creditFlag = 1;
            }

            DataSet ds = new DataSet();
            if (!string.IsNullOrEmpty(Request["stype"]) && Request["stype"] == "1")
            {
                ds = objBL_Contracts.GetARAgingOver90DaysReport(objContract, creditFlag);
            }
            else
            {
                ds = objBL_Contracts.GetARAgingByAsOfDateOver90DaysReport(objContract, creditFlag);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                report.RegData("ReportData", ds.Tables[0]);
            }

            //if (!string.IsNullOrEmpty(Request["inclNotes"]) && Convert.ToBoolean(Request["inclNotes"]))
            //{
            //    objPropCustomer.ConnConfig = connString;
            //    var dsNotes = objBL_Customer.GetRecentCollectionNotes(objPropCustomer);

            //    if (dsNotes != null && dsNotes.Tables.Count > 1)
            //    {
            //        report.RegData("LocationNotes", dsNotes.Tables[0]);
            //        report.RegData("CustomerNotes", dsNotes.Tables[1]);
            //    }

            //    report.Dictionary.Variables["IncludeNotes"].Value = Request["inclNotes"];
            //}

            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();
            report.Dictionary.Variables["EndDate"].Value = objContract.Date.ToLongDateString();
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

                mail.Title = "AR Aging Over 90 Days Report";
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This is report email sent from Mobile Office Manager. Please find the AR Aging Over 90 Days Report attached.";
                }

                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(LoadReport(), stream, settings);
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
                        if (strpath != "ARAgingOver90DaysReport.pdf")
                        {
                            mail.AttachmentFiles.Add(strpath);
                        }
                    }
                }

                mail.FileName = "ARAgingOver90DaysReport.pdf";

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
            if (DownloadFileName == "ARAgingOver90DaysReport.pdf")
            {
                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(LoadReport(), stream, settings);
                buffer1 = stream.ToArray();

                Response.Clear();
                MemoryStream ms = new MemoryStream(buffer1);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=ARAgingOver90DaysReport.pdf");
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

        string mailBody = "Please review the attached AR Aging Over 90 Days Report.";
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

    protected void StiWebViewerARAging_ExportReport(object sender, StiExportReportEventArgs e)
    {
    }
}
