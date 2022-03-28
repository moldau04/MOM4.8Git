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
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CommonModel;
using BusinessEntity.Payroll;
using BusinessEntity.Utility;
using BusinessLayer;
using MOMWebApp;
using Stimulsoft.Report;
using Stimulsoft.Report.Web;
using Telerik.Web.UI;

public partial class APAgingOver90DaysReport : System.Web.UI.Page
{
    #region Variables
    GeneralFunctions objgn = new GeneralFunctions();

    PJ objPJ = new PJ();
    BL_Bills objBL_Bills = new BL_Bills();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    BL_Report objBL_Report = new BL_Report();

    //API Variables
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    APIIntegrationModel _objAPIIntegration = new APIIntegrationModel();
    GetCompanyDetailsParam _getCompanyDetails = new GetCompanyDetailsParam();
    GetSMTPByUserIDParam _getSMTPByUserID = new GetSMTPByUserIDParam();
    getConnectionConfigParam _getConnectionConfig = new getConnectionConfigParam();
    GetBillsDetailsByDueParam _getBillsDetailsByDue = new GetBillsDetailsByDueParam();
    GetAPAgingByDateParam _getAPAgingByDate = new GetAPAgingByDateParam();

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
                    ddlInvoice.SelectedValue = HttpUtility.UrlDecode(Request.QueryString["stype"]);
                    StiWebViewerAPAging.Visible = true;
                }
                else
                {
                    ddlInvoice.SelectedValue = "3";
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

                if (!string.IsNullOrEmpty(Request["includeZero"]))
                {
                    int chkint = Convert.ToInt32(HttpUtility.UrlDecode(Request.QueryString["includeZero"]));
                    if (chkint == 1)
                    {
                        lnkChk.Checked = true;
                    }
                    else
                    {
                        lnkChk.Checked = false;
                    }
                }
                else
                {
                    lnkChk.Checked = false;
                }

                HighlightSideMenu("acctPayable", "lnkAddBill", "acctPayableSub");

                GetSMTPUser();
                SetAddress();
                string FileName = "APAgingOver90DaysReport.pdf";
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

    protected void StiWebViewerAPAging_GetReport(object sender, StiReportDataEventArgs e)
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

    protected void StiWebViewerAPAging_GetReportData(object sender, StiReportDataEventArgs e)
    {

    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("managebills.aspx");
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        byte[] buffer1 = null;
        StiReport report = new StiReport();
        report = LoadReport();
        var settings = new Stimulsoft.Report.Export.StiExcelExportSettings();
        var service = new Stimulsoft.Report.Export.StiExcelExportService();
        System.IO.MemoryStream stream = new System.IO.MemoryStream();
        service.ExportTo(report, stream, settings);
        buffer1 = stream.ToArray();

        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Content-Disposition", "attachment;filename=APAging.xls");
        Response.ContentType = "application/xls";
        Response.AddHeader("Content-Length", (buffer1.Length).ToString());
        Response.BinaryWrite(buffer1);
        Response.Flush();
        Response.Close();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        var type = rdExpandAll.Checked ? "detail" : "summary";
        var includeZero = lnkChk.Checked ? "1" : "0";
        var url = "APAgingOver90DaysReport.aspx?stype=" + ddlInvoice.SelectedValue + "&sdate=" + txtSearchDate.Text + "&type=" + type + "&includeZero=" + includeZero;

        Response.Redirect(url);
    }

    protected void rdExpCollAll_CheckedChanged(object sender, EventArgs e)
    {
        var type = rdExpandAll.Checked ? "detail" : "summary";
        var includeZero = lnkChk.Checked ? "1" : "0";
        var url = "APAgingOver90DaysReport.aspx?stype=" + ddlInvoice.SelectedValue + "&sdate=" + txtSearchDate.Text + "&type=" + type + "&includeZero=" + includeZero;

        Response.Redirect(url);
    }

    protected void lnkChk_CheckedChanged(object sender, EventArgs e)
    {
        var type = rdExpandAll.Checked ? "detail" : "summary";
        var includeZero = lnkChk.Checked ? "1" : "0";
        var url = "APAgingOver90DaysReport.aspx?stype=" + ddlInvoice.SelectedValue + "&sdate=" + txtSearchDate.Text + "&type=" + type + "&includeZero=" + includeZero;

        Response.Redirect(url);
    }

    private StiReport LoadReport()
    {
        try
        {
            string reportPathStimul = Server.MapPath("StimulsoftReports/APAgingOver90DaysReport.mrt");

            if (!string.IsNullOrEmpty(Request["type"]) && Request["type"] == "summary")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/APAgingOver90DaysSummaryReport.mrt");
            }

            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            //Get data
            var connString = Session["config"].ToString();
            objPropUser.ConnConfig = connString;

            DataSet companyInfo = objBL_Report.GetCompanyDetails(Session["config"].ToString());

            report.RegData("CompanyDetails", companyInfo.Tables[0]);

            objPJ.ConnConfig = connString;
            objPJ.UserID = Convert.ToInt32(Session["UserID"].ToString());

            if (!string.IsNullOrEmpty(Request["stype"]))
            {
                objPJ.SearchValue = Convert.ToInt16(Request.QueryString["stype"]);
            }
            else
            {
                objPJ.SearchValue = 3;
            }

            if (!string.IsNullOrEmpty(Request["sdate"]))
            {
                objPJ.SearchDate = Convert.ToDateTime(Request.QueryString["sdate"]);
            }
            else
            {
                objPJ.SearchDate = DateTime.Now;
            }

            if (!string.IsNullOrEmpty(Request["includeZero"]))
            {
                objPJ.Frequency = Convert.ToInt32(Request.QueryString["includeZero"]);
            }
            else
            {
                objPJ.Frequency = 0;
            }

            if (Session["CmpChkDefault"].ToString() == "1")
            {
                objPJ.EN = 1;
            }
            else
            {
                objPJ.EN = 0;
            }

            DataSet ds = new DataSet();

            if (objPJ.SearchValue != 3)
            {
                ds = objBL_Bills.GetBillsDetailsByDate(objPJ);
            }
            else
            {
                objPJ.fDate = objPJ.SearchDate;
                ds = objBL_Bills.GetAPAgingOver90DaysReport(objPJ);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                report.RegData("ReportData", ds.Tables[0]);
            }

            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();
            report.Dictionary.Variables["EndDate"].Value = objPJ.SearchDate.ToLongDateString();
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
            string reportPathStimul = Server.MapPath("StimulsoftReports/APAgingOver90DaysReportCSVFormate.mrt");

            if (!string.IsNullOrEmpty(Request["type"]) && Request["type"] == "summary")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/APAgingOver90DaysSummaryReportCSV.mrt");
            }

            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            //Get data
            var connString = Session["config"].ToString();
            objPropUser.ConnConfig = connString;

            DataSet companyInfo = objBL_Report.GetCompanyDetails(Session["config"].ToString());

            report.RegData("CompanyDetails", companyInfo.Tables[0]);

            objPJ.ConnConfig = connString;
            objPJ.UserID = Convert.ToInt32(Session["UserID"].ToString());

            if (!string.IsNullOrEmpty(Request["stype"]))
            {
                objPJ.SearchValue = Convert.ToInt16(Request.QueryString["stype"]);
            }
            else
            {
                objPJ.SearchValue = 3;
            }

            if (!string.IsNullOrEmpty(Request["sdate"]))
            {
                objPJ.SearchDate = Convert.ToDateTime(Request.QueryString["sdate"]);
            }
            else
            {
                objPJ.SearchDate = DateTime.Now;
            }

            if (!string.IsNullOrEmpty(Request["includeZero"]))
            {
                objPJ.Frequency = Convert.ToInt32(Request.QueryString["includeZero"]);
            }
            else
            {
                objPJ.Frequency = 0;
            }

            if (Session["CmpChkDefault"].ToString() == "1")
            {
                objPJ.EN = 1;
            }
            else
            {
                objPJ.EN = 0;
            }

            DataSet ds = new DataSet();

            if (objPJ.SearchValue != 3)
            {
                ds = objBL_Bills.GetBillsDetailsByDate(objPJ);
            }
            else
            {
                objPJ.fDate = objPJ.SearchDate;
                ds = objBL_Bills.GetAPAgingOver90DaysReport(objPJ);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                report.RegData("ReportData", ds.Tables[0]);
            }

            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();
            report.Dictionary.Variables["EndDate"].Value = objPJ.SearchDate.ToLongDateString();
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

        //API
        _getSMTPByUserID.ConnConfig = Session["config"].ToString();
        _getSMTPByUserID.UserID = Convert.ToInt32(Session["UserID"]);

        DataSet ds = new DataSet();

        List<SMTPEmailViewModel> _lstSMTPEmailViewModel = new List<SMTPEmailViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "BillAPI/BillsReport_GetSMTPByUserID";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getSMTPByUserID, true);
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;
            _lstSMTPEmailViewModel = serializer.Deserialize<List<SMTPEmailViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<SMTPEmailViewModel>(_lstSMTPEmailViewModel);
        }
        else
        {
            ds = objBL_User.getSMTPByUserID(objPropUser);
        }

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

                mail.Title = "AP Aging Report";
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This is report email sent from Mobile Office Manager. Please find the AP Aging Report attached.";
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
                        if (strpath != "APAgingReport.pdf")
                        {
                            mail.AttachmentFiles.Add(strpath);
                        }
                    }
                }

                mail.FileName = "APAgingReport.pdf";

                mail.DeleteFilesAfterSend = true;
                mail.RequireAutentication = false;
                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                mail.Send();

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            catch (Exception ex)
            {
                WebBaseUtility.ShowEmailErrorMessageBox(this, Page.GetType(), ex);
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
            if (DownloadFileName == "APAgingReport.pdf")
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
                Response.AddHeader("content-disposition", "attachment;filename=APAgingReport.pdf");
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
        //DataSet dsC = new DataSet();
        //objPropUser.ConnConfig = Session["config"].ToString();

        ////API
        //_getConnectionConfig.ConnConfig = Session["config"].ToString();

        //List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

        //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

        ////if (IsAPIIntegrationEnable == "YES")
        //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        //{
        //    string APINAME = "BillAPI/AddBills_GetControl";

        //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();

        //    serializer.MaxJsonLength = Int32.MaxValue;
        //    _GetControlViewModel = serializer.Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
        //    dsC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
        //}
        //else
        //{
        //    dsC = objBL_User.getControl(objPropUser);
        //}

        var address = WebBaseUtility.GetSignature();
        string mailBody = "Please review the attached AP Aging Report.";
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