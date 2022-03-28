using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using BusinessEntity.Payroll;
using BusinessEntity.Utility;
using BusinessLayer;
using Microsoft.Reporting.WebForms;
using MOMWebApp;
using Stimulsoft.Report;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class ARAgingReportByLocType : System.Web.UI.Page
{
    #region Variables
    GeneralFunctions objgn = new GeneralFunctions();
    Contracts objContract = new Contracts();
    BL_Contracts objBLContracts = new BL_Contracts();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    Customer objPropCustomer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    BL_Report bL_Report = new BL_Report();

    //API Variables 
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    GetCompanyDetailsParam _GetCompanyDetails = new GetCompanyDetailsParam();
    GetSMTPByUserIDParam _GetSMTPByUserID = new GetSMTPByUserIDParam();
    getConnectionConfigParam _getConnectionConfig = new getConnectionConfigParam();
    GetARAgingByLocTypeParam _GetARAgingByLocType = new GetARAgingByLocTypeParam();
    GetARAgingByLocTypeDetailParam _GetARAgingByLocTypeDetail = new GetARAgingByLocTypeDetailParam();
    getlocationTypeParam _getlocationType = new getlocationTypeParam();
    #endregion

    #region PAGELOAD

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {
            FillLocationType();
            rdExpandAll.Checked = false;
            rdCollapseAll.Checked = false;
            if (Request.QueryString["edate"] != null && !string.IsNullOrEmpty(Request.QueryString["edate"]))
            {
                txtEndDate.Text = DateTime.Parse(Request.QueryString["edate"]).ToString("MM/dd/yyyy");
            }
            else
            {
                txtEndDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            }

            if (Request.QueryString["t"] != null && !string.IsNullOrEmpty(Request.QueryString["t"]))
            {
                if (Request.QueryString["t"].ToString() == "d")
                {
                    rdExpandAll.Checked = true;
                }
                else
                {
                    rdCollapseAll.Checked = true;
                }
            }
            else
            {
                rdCollapseAll.Checked = true;
            }

            if (Request.QueryString["ltype"] != null && !string.IsNullOrEmpty(Request.QueryString["ltype"]))
            {
                var loctype = Request.QueryString["ltype"].Trim();
                var loctypeArray = loctype.Split(',');

                for (int i = 0; i < loctypeArray.Length; i++)
                {
                    RadComboBoxItem item = rcDepartment.FindItemByValue(loctypeArray[i]);
                    if (item != null)
                        item.Checked = true;
                }
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

            DataTable distributionList = WebBaseUtility.GetContactListOnExchangeServer();

            //Email
            GetSMTPUser();
            SetAddress();
            string FileName = "ARAgingReportByLocType.pdf";
            ArrayList lstPath = new ArrayList();
            if (ViewState["pathmailatt"] != null)
            {
                lstPath = (ArrayList)ViewState["pathmailatt"];
                if (!lstPath.Contains(FileName))
                {
                    lstPath.Add(FileName);
                }
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

        if (string.IsNullOrEmpty(Request.QueryString["t"]))
        {
            StiWebViewerARReport.Visible = false;
        }
        else
        {
            StiWebViewerARReport.Visible = true;
        }
    }

    #endregion

    protected void StiWebViewerARReport_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["t"]))
        {
            if (Request.QueryString["t"] == "d")
            {
                e.Report = LoadReportDetail();
            }
            else
            {
                e.Report = LoadReport();
            }

        }
    }

    protected void StiWebViewerARReport_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {

    }

    private void FillLocationType()
    {
        try
        {

            BusinessEntity.User objPropUser = new BusinessEntity.User();
            BL_User objBL_User = new BL_User();
            DataSet ds = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            _getlocationType.ConnConfig = Session["config"].ToString();

            if (IsAPIIntegrationEnable == "YES")
            {
                List<GetLocationTypeViewModel> _lstgetlocationType = new List<GetLocationTypeViewModel>();

                string APINAME = "iCollectionsAPI/iCollectionsReport_getlocationType";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getlocationType);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstgetlocationType = serializer.Deserialize<List<GetLocationTypeViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetLocationTypeViewModel>(_lstgetlocationType);
            }
            else
            {
                ds = objBL_User.getlocationType(objPropUser);
            }

            rcDepartment.DataSource = ds.Tables[0];
            rcDepartment.DataTextField = "Type";
            rcDepartment.DataValueField = "Type";
            rcDepartment.DataBind();

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
                Response.Redirect("Invoices.aspx?fil=1");
            }
            else
            {
                Response.Redirect("iCollections.aspx");
            }
        }
        else
        {
            Response.Redirect("Invoices.aspx?fil=1");
        }

    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        var endDate = Convert.ToDateTime(txtEndDate.Text);

        string ltype = string.Empty;
        List<string> selectedItem = new List<string>();
        foreach (RadComboBoxItem item in rcDepartment.Items)
        {
            if (item.Checked == true)
            {
                selectedItem.Add(item.Value);
            }
        }

        if (selectedItem.Count > 0)
        {
            ltype = string.Join(",", selectedItem);
        }

        String type = "s";
        if (rdExpandAll.Checked)
        {
            type = "d";
        }

        String page = "invoices.aspx";
        if (Request.QueryString["page"] != null && !string.IsNullOrEmpty(Request.QueryString["page"]))
        {
            page = Request.QueryString["page"].ToString();
        }

        String url = page + "&edate=" + endDate.ToString("MM/dd/yyyy") + "&t=" + type + "&ltype=" + HttpUtility.UrlEncode(ltype) + "&inclNotes=" + chkIncludeNotes.Checked + "&creditFlag=" + chkCreditFlag.Checked;
        this.Response.Redirect("ARAgingReportByLocType.aspx?page=" + url, true);
    }

    protected void rdExpCollAll_CheckedChanged(object sender, EventArgs e)
    {
        var endDate = Convert.ToDateTime(txtEndDate.Text);

        string ltype = string.Empty;
        List<string> selectedItem = new List<string>();
        foreach (RadComboBoxItem item in rcDepartment.Items)
        {
            if (item.Checked == true)
            {
                selectedItem.Add(item.Value);
            }
        }

        if (selectedItem.Count > 0)
        {
            ltype = string.Join(",", selectedItem);
        }

        String type = "s";
        if (rdExpandAll.Checked)
        {
            type = "d";
        }

        String page = "invoices.aspx";
        if (Request.QueryString["page"] != null && !string.IsNullOrEmpty(Request.QueryString["page"]))
        {
            page = Request.QueryString["page"].ToString();
        }

        String url = page + "&edate=" + endDate.ToString("MM/dd/yyyy") + "&t=" + type + "&ltype=" + HttpUtility.UrlEncode(ltype) + "&inclNotes=" + chkIncludeNotes.Checked + "&creditFlag=" + chkCreditFlag.Checked;
        this.Response.Redirect("ARAgingReportByLocType.aspx?page=" + url, true);
    }

    private StiReport getReportForEmail()
    {
        StiWebViewerARReport.Visible = true;
        if (rdExpandAll.Checked == true)
        {
            return LoadReportDetail();
        }
        else
        {
            return LoadReport();
        }
    }

    private StiReport LoadReport()
    {
        try
        {
            string reportPathStimul = Server.MapPath("StimulsoftReports/ARAgingReportByLocType.mrt");

            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            //Get data
            try
            {
                DataSet CompanyDetails = getCompanyInfo();
                var ds = getDataReport(0);
                if (ds != null && ds.Tables.Count > 0)
                {                    
                    report.RegData("ReportData", ds.Tables[0]);

                    if (!string.IsNullOrEmpty(Request["inclNotes"]) && Convert.ToBoolean(Request["inclNotes"]))
                    {
                        objPropCustomer.ConnConfig = Session["config"].ToString(); ;
                        var dsNotes = objBL_Customer.GetRecentCollectionNotes(objPropCustomer);

                        if (dsNotes != null)
                        {
                            report.RegData("LocationNotes", dsNotes.Tables[0]);
                        }

                        report.Dictionary.Variables["IncludeNotes"].Value = Request["inclNotes"];
                    }
                }

                report.RegData("CompanyDetails", CompanyDetails);
                report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();
                if (Request.QueryString["edate"] != null && !string.IsNullOrEmpty(Request.QueryString["edate"]))
                {
                    report.Dictionary.Variables["EndDate"].Value = DateTime.ParseExact(Request.QueryString["edate"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture).ToLongDateString();
                }

                report.CacheAllData = true;
                report.Render();
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

            }
            return report;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return null;
        }
    }

    private StiReport LoadReportDetail()
    {
        try
        {
            string reportPathStimul = Server.MapPath("StimulsoftReports/ARAgingReportByLocType_Detail.mrt");

            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            //Get data
            try
            {
                DataSet CompanyDetails = getCompanyInfo();

                var ds = getDataReport(1);

                if (ds != null && ds.Tables.Count > 0)
                {
                    report.RegData("ReportData", ds.Tables[0]);

                    if (!string.IsNullOrEmpty(Request["inclNotes"]) && Convert.ToBoolean(Request["inclNotes"]))
                    {
                        objPropCustomer.ConnConfig = Session["config"].ToString(); ;
                        var dsNotes = objBL_Customer.GetRecentCollectionNotes(objPropCustomer);

                        if (dsNotes != null)
                        {
                            report.RegData("LocationNotes", dsNotes.Tables[0]);
                        }

                        report.Dictionary.Variables["IncludeNotes"].Value = Request["inclNotes"];
                    }
                }

                report.RegData("CompanyDetails", CompanyDetails);
                report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();

                if (Request.QueryString["edate"] != null && !string.IsNullOrEmpty(Request.QueryString["edate"]))
                {
                    report.Dictionary.Variables["EndDate"].Value = DateTime.ParseExact(Request.QueryString["edate"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture).ToLongDateString();
                }

                report.CacheAllData = true;
                report.Render();
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

            }

            return report;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return null;
        }
    }

    private DataSet getCompanyInfo()
    {
        DataSet dsC = new DataSet();

        objPropUser.ConnConfig = Session["config"].ToString();
        _getConnectionConfig.ConnConfig = Session["config"].ToString();
        if (IsAPIIntegrationEnable == "YES")
        {
            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

            string APINAME = "iCollectionsAPI/iCollectionsList_GetControl";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _GetControlViewModel = serializer.Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
            dsC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
        }
        else
        {
            dsC = objBL_User.getControl(objPropUser);
        }

        DataTable cTable = BuildCompanyDetailsTable();
        var cRow = cTable.NewRow();

        DataSet companyInfo = new DataSet();
        if (IsAPIIntegrationEnable == "YES")
        {
            List<GetCompanyDetailsViewModel> _GetCompanyDetailsViewModel = new List<GetCompanyDetailsViewModel>();

            string APINAME = "iCollectionsAPI/iCollectionsList_GetCompanyDetails";

            _GetCompanyDetails.ConnConfig = Session["config"].ToString();

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCompanyDetails);
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;
            _GetCompanyDetailsViewModel = serializer.Deserialize<List<GetCompanyDetailsViewModel>>(_APIResponse.ResponseData);
            companyInfo = CommonMethods.ToDataSet<GetCompanyDetailsViewModel>(_GetCompanyDetailsViewModel);
        }
        else
        {
            companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());
        }


        cRow["CompanyName"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Name"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Name"].ToString();
        cRow["CompanyAddress"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Address"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Address"].ToString();
        cRow["ContactNo"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Contact"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Contact"].ToString();
        cRow["Email"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Email"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Email"].ToString();

        cRow["City"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["City"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["City"].ToString();
        cRow["State"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["State"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["State"].ToString();
        cRow["Phone"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Phone"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Phone"].ToString();
        cRow["Fax"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Fax"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Fax"].ToString();
        cRow["Zip"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Zip"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Zip"].ToString();

        cTable.Rows.Add(cRow);

        Session["CompanyTable"] = cTable;

        DataSet CompanyDetails = new DataSet();
        cTable.TableName = "CompanyDetails";
        CompanyDetails.Tables.Add(cTable);
        CompanyDetails.DataSetName = "CompanyDetails";

        return CompanyDetails;
    }

    private DataSet getDataReport(int type)
    {
        objContract.ConnConfig = Session["config"].ToString();

        if (Request.QueryString["edate"] != null && !string.IsNullOrEmpty(Request.QueryString["edate"]))
        {
            objContract.Date = DateTime.ParseExact(Request.QueryString["edate"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
        }

        var ltype = string.Empty;
        if (Request.QueryString["ltype"] != null && !string.IsNullOrEmpty(Request.QueryString["ltype"]))
        {
            ltype = Request.QueryString["ltype"].Trim();
        }

        objContract.DepartmentTypes = ltype;

        var creditFlag = 0;
        if (!string.IsNullOrEmpty(Request["creditFlag"]) && Convert.ToBoolean(Request.QueryString["creditFlag"]))
        {
            creditFlag = 1;
        }

        DataSet ds = new DataSet();
        if (type == 0)
        {
            return objBLContracts.GetARAgingByLocType(objContract, creditFlag);
        }
        else
        {
            return objBLContracts.GetARAgingByLocTypeDetail(objContract, creditFlag);
        }
    }

    protected DataTable BuildCompanyDetailsTable()
    {
        DataTable companyDetailsTable = new DataTable();
        companyDetailsTable.Columns.Add("CompanyAddress");
        companyDetailsTable.Columns.Add("CompanyName");
        companyDetailsTable.Columns.Add("ContactNo");
        companyDetailsTable.Columns.Add("Email");
        companyDetailsTable.Columns.Add("LogoURL");
        companyDetailsTable.Columns.Add("City");
        companyDetailsTable.Columns.Add("State");
        companyDetailsTable.Columns.Add("Zip");
        companyDetailsTable.Columns.Add("Fax");
        companyDetailsTable.Columns.Add("Phone");
        return companyDetailsTable;
    }

    #region Email

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

        _GetSMTPByUserID.ConnConfig = Session["config"].ToString();
        _GetSMTPByUserID.UserID = Convert.ToInt32(Session["UserID"]);
        DataSet ds = new DataSet();

        if (IsAPIIntegrationEnable == "YES")
        {
            List<SMTPEmailViewModel> _lstSMTPEmailViewModel = new List<SMTPEmailViewModel>();

            string APINAME = "iCollectionsAPI/iCollectionsReport_GetSMTPByUserID";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetSMTPByUserID, true);
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

                mail.Title = "AR Aging by Location Type Report";
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This is report email sent from Mobile Office Manager. Please find the AR Aging by Location Type Report attached.";
                }

                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                StiReport report = getReportForEmail();
                service.ExportTo(report, stream, settings);
                StiWebViewerARReport.Report = report;
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
                        if (strpath != "ARAgingReportByLocType.pdf")
                        {
                            mail.AttachmentFiles.Add(strpath);
                        }
                    }
                }

                mail.FileName = "ARAgingReportByLocType.pdf";

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
            if (DownloadFileName == "ARAgingReportByLocType.pdf")
            {
                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(getReportForEmail(), stream, settings);
                buffer1 = stream.ToArray();

                Response.Clear();
                MemoryStream ms = new MemoryStream(buffer1);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=ARAgingReportByLocType.pdf");
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

        string mailBody = "Please review the attached AR Aging by Location Type Report.";
        address = mailBody + Environment.NewLine + "<br />" + Environment.NewLine + "<br />" + address;

        txtBody.Text = address;

        
    }

    private string GetRadComboBoxSelectedItems(RadComboBox radComboBox)
    {
        int itemschecked = radComboBox.CheckedItems.Count;
        String[] ServiceTypesArray = new String[itemschecked];

        var collection = radComboBox.CheckedItems;
        int i = 0;
        foreach (var item in collection)
        {
            String value = item.Value;
            ServiceTypesArray[i] = $"{value}";
            i++;
        }
        var ServiceTypes = String.Join(",", ServiceTypesArray);

        return ServiceTypes;
    }

    #endregion

    protected void lnkSearchEmail_Click(object sender, EventArgs e)
    {
        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
        RadGrid_Emails.Rebind();
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
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