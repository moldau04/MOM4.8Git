using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;
using iTextSharp.text;
using System.Net.Configuration;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Collections;
using System.Threading;
using System.Text.RegularExpressions;

public partial class OpenJobReport : System.Web.UI.Page
{
    BL_ReportsData objBL_ReportsData = new BL_ReportsData();
    CustomerReport objCustReport = new CustomerReport();

    BusinessEntity.User objPropUser = new BusinessEntity.User();
    BL_User objBL_User = new BL_User();

    BL_Job objBL_Job = new BL_Job();
    JobT objJob = new JobT();

    GeneralFunctions objgn = new GeneralFunctions();

    public static string _sortBy = string.Empty;
    string _reportType = "OpenJob";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx", false);
        }

        if (!IsPostBack)
        {
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }

            if (Request.QueryString["noty"] != null)
            {
                if (Request.QueryString["noty"] == "savesuccess")
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Report added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }

                if (Request.QueryString["noty"] == "updatedsuccess")
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Report updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }

                if (Request.QueryString["noty"] == "customizedsuccess")
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Report customized successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }

                if (Request.QueryString["noty"] == "deletesuccess")
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Report deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }

                if (Request.QueryString["noty"] == "sendsuccess")
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }

            HighlightSideMenu("ProjectMgr", "lnkProject", "ProjectMgrSub");

            GetCustomerDetails();
            GetReportsName();
            GetCustReportFiltersValue();
            ConvertToJSON();

            dvSaveReport.Attributes.Add("style", "display:none");

            if (Request.QueryString["reportId"] != null && Convert.ToInt32(Request.QueryString["reportId"]) != 0)
            {
                var reportId = Convert.ToInt32(Request.QueryString["reportId"]);

                DefineGridStructure(reportId);
                BindHeaderDetails(reportId);
                GetReportDetailByRptId(reportId);
                GetReportColumnsByRepId(reportId);

                btnSaveReport2.Visible = true;
                btnDeleteReport.Visible = true;
                btnCustomizeReport.Visible = true;
                btnPrint.Visible = true;
                btnEmailReport.Visible = true;
                btnExportReport.Visible = true;
                hdnCustomizeReportName.Value = drpReports.SelectedItem.Text;

                GetSMTPUser();
                SetAddress();
            }
            else
            {
                btnSaveReport2.Visible = false;
                btnDeleteReport.Visible = false;
                btnCustomizeReport.Visible = false;
                btnPrint.Visible = false;
                btnEmailReport.Visible = false;
                btnExportReport.Visible = false;

                Session.Remove("OpenJobData");
                Session.Remove("Query");
            }
        }
    }

    private void DefineGridStructure(int reportId)
    {
        RadGrid_Project.MasterTableView.Columns.Clear();
        RadGrid_Project.MasterTableView.AutoGenerateColumns = false;

        DataSet dsGetColumns = new DataSet();
        objCustReport.DBName = Session["dbname"].ToString();
        objCustReport.ConnConfig = Session["config"].ToString();
        objCustReport.ReportId = reportId;
        dsGetColumns = objBL_ReportsData.GetReportColByRepId(objCustReport);

        if (dsGetColumns.Tables[0].Rows.Count > 0)
        {
            var listColumns = dsGetColumns.Tables[0].AsEnumerable().Select(s => s.Field<string>("ColumnName")).ToArray<string>();
            var gridWidth = 0;

            foreach (DataRow col in dsGetColumns.Tables[0].Rows)
            {
                var width = "150";
                if (!string.IsNullOrEmpty(col["ColumnWidth"].ToString()))
                {
                    width = col["ColumnWidth"].ToString();
                }

                GridBoundColumn boundColumn = new GridBoundColumn();
                RadGrid_Project.MasterTableView.Columns.Add(boundColumn);

                // Remove special characters
                boundColumn.DataField = col["ColumnName"].ToString();
                boundColumn.HeaderText = col["ColumnName"].ToString();
                boundColumn.HeaderStyle.Width = new Unit(width);
                boundColumn.AllowFiltering = false;
                boundColumn.AutoPostBackOnFilter = false;
                boundColumn.ShowFilterIcon = false;

                gridWidth += Convert.ToInt32(width.Replace("px", ""));
            }

            Session["OpenJobWidth"] = gridWidth;
        }
    }

    protected void RadGrid_Project_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;

                if (totalCount == 0) totalCount = 1000;
                GeneralFunctions obj = new GeneralFunctions();
                var sizes = obj.TelerikPageSize(totalCount);
                dropDown.Items.Clear();

                foreach (var size in sizes)
                {
                    var cboItem = new RadComboBoxItem() { Text = size.Key, Value = size.Value };
                    cboItem.Attributes.Add("ownerTableViewId", e.Item.OwnerTableView.ClientID);
                    if (e.Item.OwnerTableView.PageSize.ToString() == size.Value) cboItem.Selected = true;
                    dropDown.Items.Add(cboItem);
                }
            }
        }
        catch { }
    }

    protected void RadGrid_Project_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (Session["OpenJobData"] != null)
        {
            DataSet ds = (DataSet)Session["OpenJobData"];
            DataTable dt = ds.Tables[0].Clone();

            var filterString = RadGrid_Project.MasterTableView.FilterExpression;

            if (!string.IsNullOrEmpty(filterString))
            {
                var data = ds.Tables[0].Select(filterString, "");

                if (data.Count() > 0)
                {
                    dt = data.CopyToDataTable();
                }
            }
            else
            {
                dt = ds.Tables[0];
            }

            RadGrid_Project.DataSource = dt;
            lblRecordCount.Text = dt.Rows.Count.ToString() + " Record(s) found";
        }
        else if (Session["Query"] != null)
        {
            GetGridData(Session["Query"].ToString());
        }
    }

    protected void RadGrid_Project_GridExporting(object source, GridExportingArgs e)
    {
        // Delete export file
        DeleteExcelFiles();
        DeletePDFFiles();

        var fileName = Regex.Replace(drpReports.SelectedItem.Text, @"[^0-9a-zA-Z\._]", "_");

        if (e.ExportType == ExportType.ExcelML)
        {
            if (!Directory.Exists(Server.MapPath("ReportFiles/Excel")))
            {
                Directory.CreateDirectory(Server.MapPath("ReportFiles/Excel"));
            }

            var filePath = Server.MapPath($"ReportFiles/Excel/{fileName}.xls");
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                byte[] pdfdata = Encoding.GetEncoding(1252).GetBytes(e.ExportOutput);
                fs.Write(pdfdata, 0, pdfdata.Length);
            }
        }

        if (e.ExportType == ExportType.Pdf)
        {
            if (!Directory.Exists(Server.MapPath("ReportFiles/PDF")))
            {
                Directory.CreateDirectory(Server.MapPath("ReportFiles/PDF"));
            }

            var filePath = Server.MapPath($"ReportFiles/PDF/{fileName}.pdf");
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                byte[] pdfdata = Encoding.GetEncoding(1252).GetBytes(e.ExportOutput);
                fs.Write(pdfdata, 0, pdfdata.Length);
            }
        }

        if (ViewState["btnEmailReport"] != null && ViewState["btnEmailReport"].ToString() == "1")
        {
            SendEmail();

            var url = Request.Url.ToString();
            url = RemoveQueryStringByKey(url, "noty");
            url += "&noty=sendsuccess";

            Response.Redirect(url);
        }
    }

    protected void drpReports_SelectedIndexChanged(object sender, EventArgs e)
    {
        var url = "OpenJobReport.aspx?type=OpenJob&reportId=" + drpReports.SelectedValue;

        Response.Redirect(url);
    }

    private void GetReportDetailByRptId(int reportId)
    {
        try
        {
            DataSet dsGetRptDetails = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            objCustReport.ReportId = reportId;
            dsGetRptDetails = objBL_ReportsData.GetReportDetailById(objCustReport);

            if (dsGetRptDetails.Tables[0].Rows.Count > 0)
            {
                bool isGlobal = Convert.ToBoolean(dsGetRptDetails.Tables[0].Rows[0]["IsGlobal"]);
                bool isAscending = Convert.ToBoolean(dsGetRptDetails.Tables[0].Rows[0]["IsAscendingOrder"]);

                if (isGlobal)
                {
                    chkIsGlobal.Checked = true;
                }
                else
                {
                    chkIsGlobal.Checked = false;
                }

                if (isAscending)
                {
                    rdbOrders.SelectedValue = "1";
                }
                else
                {
                    rdbOrders.SelectedValue = "2";
                }

                hdnDrpSortBy.Value = GetCustomLabel(dsGetRptDetails.Tables[0].Rows[0]["SortBy"].ToString());
                hdnIsStock.Value = dsGetRptDetails.Tables[0].Rows[0]["IsStock"].ToString();

                _sortBy = dsGetRptDetails.Tables[0].Rows[0]["SortBy"].ToString();
                _sortBy = "[" + _sortBy + "]";
                _sortBy = _sortBy + " " + (isAscending == true ? "Asc" : "Desc");
            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void GetCustomerType()
    {
        try
        {
            DataSet dsGetCustType = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            dsGetCustType = objBL_ReportsData.GetCustomerType(objCustReport);
            if (dsGetCustType.Tables[0].Rows.Count > 0)
            {
                drpType.DataSource = dsGetCustType.Tables[0];
                drpType.DataTextField = "Type";
                drpType.DataValueField = "Type";
                drpType.DataBind();
            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void GetCustReportFiltersValue()
    {
        DataSet dsGetCustReportFiltersValue = new DataSet();
        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.ConnConfig = Session["config"].ToString();
        dsGetCustReportFiltersValue = objBL_ReportsData.GetOpenJobReportFiltersValue(objPropUser);

        if (dsGetCustReportFiltersValue.Tables[0].Rows.Count > 0)
        {
            drpCustomer.DataSource = dsGetCustReportFiltersValue.Tables[0];
            drpCustomer.DataTextField = "Customer";
            drpCustomer.DataValueField = "Customer";
            drpCustomer.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[1].Rows.Count > 0)
        {
            drpLocation.DataSource = dsGetCustReportFiltersValue.Tables[1];
            drpLocation.DataTextField = "Location";
            drpLocation.DataValueField = "Location";
            drpLocation.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[2].Rows.Count > 0)
        {
            drpCity.DataSource = dsGetCustReportFiltersValue.Tables[2];
            drpCity.DataTextField = "City";
            drpCity.DataValueField = "City";
            drpCity.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[3].Rows.Count > 0)
        {
            ddlState.DataSource = dsGetCustReportFiltersValue.Tables[3];
            ddlState.DataTextField = "State";
            ddlState.DataValueField = "State";
            ddlState.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[4].Rows.Count > 0)
        {
            drpDefaultSalesPerson.DataSource = dsGetCustReportFiltersValue.Tables[4];
            drpDefaultSalesPerson.DataTextField = "SalesPerson";
            drpDefaultSalesPerson.DataValueField = "SalesPerson";
            drpDefaultSalesPerson.DataBind();
        }
    }

    private void GetCustomerAddress()
    {
        try
        {
            DataSet dsGetCustAddress = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            dsGetCustAddress = objBL_ReportsData.GetCustomerAddress(objCustReport);
            if (dsGetCustAddress.Tables[0].Rows.Count > 0)
            {
                drpAddress.DataSource = dsGetCustAddress.Tables[0];
                drpAddress.DataTextField = "Address";
                drpAddress.DataValueField = "Address";
                drpAddress.DataBind();

            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void GetCustomerCity()
    {
        try
        {
            DataSet dsGetCustCity = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            dsGetCustCity = objBL_ReportsData.GetCustomerCity(objCustReport);
            if (dsGetCustCity.Tables[0].Rows.Count > 0)
            {
                drpCity.DataSource = dsGetCustCity.Tables[0];
                drpCity.DataTextField = "City";
                drpCity.DataValueField = "City";
                drpCity.DataBind();

            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void GetReportsName()
    {
        try
        {
            DataSet dsGetReports = new DataSet();
            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.UserID = Convert.ToInt32(Session["UserID"].ToString());
            dsGetReports = objBL_ReportsData.GetDynamicReports(objPropUser, _reportType);

            if (dsGetReports.Tables.Count > 0)
            {
                drpReports.DataSource = dsGetReports.Tables[0];
                drpReports.DataTextField = "ReportName";
                drpReports.DataValueField = "Id";
                drpReports.DataBind();
                drpReports.Items.Insert(0, new System.Web.UI.WebControls.ListItem { Text = "--Select report--", Value = "0" });
                drpReports.SelectedValue = Request.QueryString["reportId"];
            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void GetReportColumnsByRepId(int reportId)
    {
        try
        {
            DataSet dsGetColumns = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            objCustReport.ReportId = reportId;

            dsGetColumns = objBL_ReportsData.GetReportColByRepId(objCustReport);
            string[] checkedColumns = new string[] { };
            string[] selectedFiltersColumns = new string[] { };
            string[] selectedFiltersValues = new string[] { };
            if (dsGetColumns.Tables[0].Rows.Count > 0)
            {
                checkedColumns = dsGetColumns.Tables[0].AsEnumerable().Select(s => s.Field<string>("ColumnName")).ToArray<string>();

                var validationColumns = dsGetColumns.Tables[0].AsEnumerable().Select(s => s.Field<string>("ColumnName")).ToArray<string>();
                foreach (var col in validationColumns)
                {
                    var checkCol = chkColumnList.Items.FindByText(col);
                    if (checkCol == null)
                    {
                        checkedColumns = checkedColumns.Where(x => x != col).ToArray();
                    }
                }

                // Custom labels
                var customColums = checkedColumns.ToList();
                var dtCustomLabel = GetJobCustomLabel();

                foreach (DataRow dr in dtCustomLabel.Rows)
                {
                    var customField = string.Format("Custom{0}", dr["ID"]);
                    int index = customColums.FindIndex(x => x == customField);

                    if (index >= 0)
                    {
                        customColums[index] = dr["CustomLabel"].ToString();
                    }
                }

                hdnColumnList.Value = string.Join(",", customColums);
            }
            else
            {
                hdnColumnList.Value = string.Empty;
            }

            DataSet dsSelectedFilters = new DataSet();
            dsSelectedFilters = objBL_ReportsData.GetReportFiltersByRepId(objCustReport);
            if (dsSelectedFilters.Tables[0].Rows.Count > 0)
            {
                selectedFiltersColumns = dsSelectedFilters.Tables[0].AsEnumerable().Select(s => s.Field<string>("FilterColumn")).ToArray<string>();
                selectedFiltersValues = dsSelectedFilters.Tables[0].AsEnumerable().Select(s => s.Field<string>("FilterSet")).ToArray<string>();
            }

            if (checkedColumns.Count() > 0)
            {
                BindGridReport(checkedColumns, selectedFiltersColumns, selectedFiltersValues, _sortBy);
            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private List<CustomerReport> GetReportFilters()
    {
        List<CustomerReport> lstCustomerReport = new List<CustomerReport>();
        try
        {
            DataSet dsGetFilters = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            objCustReport.ReportId = Convert.ToInt32(Request.QueryString["reportId"]);
            dsGetFilters = objBL_ReportsData.GetReportFiltersByRepId(objCustReport);
            for (int i = 0; i <= dsGetFilters.Tables[0].Rows.Count - 1; i++)
            {
                CustomerReport objCustmerReport = new CustomerReport();
                objCustmerReport.FilterColumns = dsGetFilters.Tables[0].Rows[i]["FilterColumn"].ToString();
                objCustmerReport.FilterValues = dsGetFilters.Tables[0].Rows[i]["FilterSet"].ToString();

                lstCustomerReport.Add(objCustmerReport);
            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }

        return lstCustomerReport;
    }

    public void ConvertToJSON()
    {
        JavaScriptSerializer jss1 = new JavaScriptSerializer();
        string _myJSONstring = jss1.Serialize(GetReportFilters());
        string filters = "var filters=" + _myJSONstring + ";";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "reportsr123", filters, true);
    }

    private DataTable GetJobCustomLabel()
    {
        objJob.ConnConfig = Session["config"].ToString();
        objJob.Job = 0;
        var ds = objBL_Job.GetJobCustomValueByJobId(objJob);

        return ds.Tables[0];
    }

    private string GetOriginLabel(string name, DataTable customLabels = null)
    {
        if (customLabels == null)
        {
            customLabels = GetJobCustomLabel();
        }

        DataRow[] dr = customLabels.Select($"CustomLabel = '{name}'");

        if (dr.Count() > 0)
        {
            return $"Custom{dr[0]["ID"]}";
        }

        return name;
    }

    private string GetCustomLabel(string name, DataTable customLabels = null)
    {
        if (name.Contains("Custom") && name != "Customer")
        {
            int parsedInt;
            var id = int.TryParse(name.Replace("Custom", ""), out parsedInt) ? parsedInt : 0;

            if (id > 0)
            {
                if (customLabels == null)
                {
                    customLabels = GetJobCustomLabel();
                }

                DataRow[] dr = customLabels.Select($"ID = {id}");

                if (dr.Count() > 0)
                {
                    return $"{dr[0]["CustomLabel"]}";
                }
            }
        }

        return name;
    }

    private void GetCustomerDetails()
    {
        try
        {
            chkColumnList.Items.Clear();
            lstFilter.Items.Clear();
            DataSet dsGetCustDetails = new DataSet();
            DataSet dsGetAccountSummaryListing = new DataSet();
            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsGetCustDetails = objBL_ReportsData.GetOpenJobDetails(objPropUser);
            dsGetAccountSummaryListing = objBL_ReportsData.GetAccountSummaryListingDetail(objPropUser);

            if (dsGetCustDetails.Tables.Count > 0)
            {
                DataTable table = dsGetCustDetails.Tables[0];

                List<string> lstHeaders = (from dr in table.Rows.Cast<DataRow>()
                                           select dr["name"].ToString()).OrderBy(d => d).ToList();

                System.Web.UI.WebControls.ListItem _newLstBox1 = new System.Web.UI.WebControls.ListItem();
                _newLstBox1.Text = "Open Job";
                _newLstBox1.Attributes.CssStyle.Add("font-size", "15px");
                _newLstBox1.Attributes.CssStyle.Add("font-weight", "bolder !important");
                _newLstBox1.Attributes.CssStyle.Add("padding", "7px 0");
                lstFilter.Items.Add(_newLstBox1);

                // Custom labels
                var dtCustomLabel = GetJobCustomLabel();

                foreach (DataRow dr in dtCustomLabel.Rows)
                {
                    var customField = string.Format("Custom{0}", dr["ID"]);
                    int index = lstHeaders.FindIndex(x => x == customField);

                    if (index >= 0)
                    {
                        lstHeaders[index] = dr["CustomLabel"].ToString();
                    }
                }

                foreach (var _header in lstHeaders)
                {
                    chkColumnList.Items.Add(_header);
                    lstFilter.Items.Add(_header);
                }

                chkColumnList.DataBind();

                lstFilter.DataBind();
                ClientScript.RegisterStartupScript(Page.GetType(), "removeCheckbox", "removeCheckbox();", true);
            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void btnSaveReport_Click(object sender, EventArgs e)
    {
        try
        {
            // Delete export file
            DeleteExcelFiles();
            DeletePDFFiles();

            var checkColumns = hdnLstColumns.Value.TrimEnd('^').Split('^').ToList();
            var columns = hdnLstColumns.Value.TrimEnd('^').Split('^').ToList();
            var columnWidth = hdnColumnWidth.Value.TrimEnd('^').Split('^').ToList();

            foreach (var col in checkColumns)
            {
                var checkCol = chkColumnList.Items.FindByText(col);
                if (checkCol == null)
                {
                    int index = columns.FindIndex(x => x == col);
                    columns.RemoveAt(index);

                    if (columnWidth.Count > index)
                    {
                        columnWidth.RemoveAt(index);
                    }
                }
            }

            // Get origin labels
            var dtCustomLabel = GetJobCustomLabel();

            foreach (DataRow dr in dtCustomLabel.Rows)
            {
                int index = columns.FindIndex(x => x == dr["CustomLabel"].ToString());

                if (index >= 0)
                {
                    columns[index] = string.Format("Custom{0}", dr["ID"]);
                }
            }

            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            if (drpReports.Items.Count > 0)
            {
                objCustReport.ReportId = Convert.ToInt32(drpReports.SelectedValue);
            }

            objCustReport.ReportName = txtReportName.Text;
            objCustReport.ReportType = !string.IsNullOrEmpty(Request.QueryString["type"]) ? objCustReport.ReportType = Request.QueryString["type"].ToString() : objCustReport.ReportType = _reportType;
            objCustReport.UserId = Convert.ToInt32(Session["UserID"].ToString());
            objCustReport.IsGlobal = chkIsGlobal.Checked ? true : false;
            objCustReport.ColumnName = string.Join("^", columns);
            objCustReport.ColumnWidth = string.Join("^", columnWidth);
            objCustReport.FilterColumns = hdnFilterColumns.Value.TrimEnd('^');
            objCustReport.FilterValues = HttpUtility.HtmlDecode(hdnFilterValues.Value.Trim().TrimEnd('^').TrimEnd('|'));
            objCustReport.IsAscending = rdbOrders.SelectedItem.Value == "1" ? true : false;
            objCustReport.SortBy = GetOriginLabel(hdnDrpSortBy.Value);
            objCustReport.MainHeader = chkMainHeader.Checked ? true : false;
            objCustReport.CompanyName = txtCompanyName.Text;
            objCustReport.ReportTitle = txtReportTitle.Text;
            objCustReport.SubTitle = txtSubtitle.Text;
            objCustReport.IsStock = false;
            objCustReport.Module = "OpenJob";

            if (chkDatePrepared.Checked)
            {
                objCustReport.DatePrepared = drpDatePrepared.SelectedValue.ToString();
            }
            else
            {
                objCustReport.DatePrepared = "";
            }

            objCustReport.TimePrepared = chkTimePrepared.Checked ? true : false;
            if (chkPageNumber.Checked)
            {
                objCustReport.PageNumber = drpPageNumber.SelectedValue.ToString();
            }
            else
            {
                objCustReport.PageNumber = "";
            }

            objCustReport.ExtraFooterLine = txtExtraFooterLine.Text;
            objCustReport.Alignment = drpAlignment.SelectedValue.ToString();
            objCustReport.PDFSize = drpPDFPageSize.SelectedValue.ToString();

            if (objBL_ReportsData.CheckExistingReport(objCustReport, hdnReportAction.Value) == true)
            {
                dvSaveReport.Attributes.Add("style", "display:block");
                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Report with this name already exists!',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                return;
            }

            DataSet ds = new DataSet();
            if (hdnReportAction.Value == "Save")
            {
                ds = objBL_ReportsData.InsertCustomerReport(objCustReport);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    var url = "OpenJobReport.aspx?type=OpenJob&reportId=" + ds.Tables[0].Rows[0]["ReportId"] + "&noty=savesuccess";
                    Response.Redirect(url);
                }
            }
            else
            {
                if (objBL_ReportsData.IsStockReportExist(objCustReport, hdnReportAction.Value) == true)
                {
                    objBL_ReportsData.UpdateCustomerReport(objCustReport);

                    var url = "OpenJobReport.aspx?type=OpenJob&reportId=" + objCustReport.ReportId + "&noty=customizedsuccess";
                    Response.Redirect(url);
                }
                else
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'You dont have permission to update this report. Please choose another title for this report',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }

            hdnCustomizeReportName.Value = objCustReport.ReportName;
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void btnSaveReport2_Click(object sender, EventArgs e)
    {
        try
        {
            // Delete export file
            DeleteExcelFiles();
            DeletePDFFiles();

            if (!string.IsNullOrEmpty(Request.QueryString["reportId"]) && Convert.ToInt32(Request.QueryString["reportId"]) != 0)
            {
                var reportId = Convert.ToInt32(Request.QueryString["reportId"]);

                SaveResizedAndReorderReport();
                GetReportColumnsByRepId(reportId);

                var url = "OpenJobReport.aspx?type=OpenJob&reportId=" + reportId + "&noty=updatedsuccess";
                Response.Redirect(url);
            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void btnDeleteReport_Click(object sender, EventArgs e)
    {
        try
        {
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();

            if (drpReports.Items.Count > 0)
            {
                objCustReport.ReportId = Convert.ToInt32(drpReports.SelectedValue);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Please select report to delete.',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                return;
            }

            objCustReport.UserId = Convert.ToInt32(Session["UserID"].ToString());
            objBL_ReportsData.DeleteCustomerReport(objCustReport);

            var url = "OpenJobReport.aspx?type=OpenJob&noty=deletesuccess";
            Response.Redirect(url);
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Session.Remove("OpenJobData");
        Session.Remove("Query");
        Response.Redirect("project.aspx?fil=1");
    }

    private void BindHeaderDetails(int reportId)
    {
        try
        {
            DataSet dsCompDetail = new DataSet();
            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsCompDetail = objBL_ReportsData.GetControlForReports(objPropUser);

            if (dsCompDetail.Tables[0].Rows.Count > 0)
            {
                byte[] compLogo = null;
                if (!Convert.IsDBNull(dsCompDetail.Tables[0].Rows[0]["Logo"]))
                {
                    compLogo = (byte[])dsCompDetail.Tables[0].Rows[0]["Logo"];
                }
            }

            objCustReport.ReportId = reportId;

            DataSet dsGetHeaderFooterDetail = objBL_ReportsData.GetHeaderFooterDetail(objCustReport);

            if (dsGetHeaderFooterDetail.Tables[0].Rows.Count > 0)
            {
                hdnMainHeader.Value = dsGetHeaderFooterDetail.Tables[0].Rows[0]["MainHeader"].ToString();

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["MainHeader"].ToString() == "True")
                {
                    chkMainHeader.Checked = true;
                }
                else
                {
                    chkMainHeader.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["CompanyName"].ToString() != "")
                {
                    txtCompanyName.Enabled = true;
                    txtCompanyName.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["CompanyName"].ToString();
                    chkCompanyName.Checked = true;
                }
                else
                {
                    txtCompanyName.Enabled = false;
                    txtCompanyName.Text = "";
                    chkCompanyName.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["ReportTitle"].ToString() != "")
                {
                    txtReportTitle.Enabled = true;
                    txtReportTitle.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["ReportTitle"].ToString();
                    hdnCustomizeReportTitle.Value = dsGetHeaderFooterDetail.Tables[0].Rows[0]["ReportTitle"].ToString();
                    chkReportTitle.Checked = true;
                }
                else
                {
                    txtReportTitle.Enabled = false;
                    txtReportTitle.Text = "";
                    hdnCustomizeReportTitle.Value = "";
                    chkReportTitle.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["SubTitle"].ToString() != "")
                {
                    txtSubtitle.Enabled = true;
                    txtSubtitle.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["SubTitle"].ToString();
                    hdnCustomizeReportSubject.Value = dsGetHeaderFooterDetail.Tables[0].Rows[0]["SubTitle"].ToString();
                    chkSubtitle.Checked = true;
                }
                else
                {
                    txtSubtitle.Enabled = false;
                    txtSubtitle.Text = "";
                    hdnCustomizeReportSubject.Value = "";
                    chkSubtitle.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() != "")
                {
                    drpDatePrepared.Enabled = true;
                    drpDatePrepared.SelectedValue = dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString();
                    chkDatePrepared.Checked = true;
                }
                else
                {
                    drpDatePrepared.Enabled = false;
                    drpDatePrepared.SelectedIndex = 0;
                    chkDatePrepared.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["TimePrepared"].ToString() == "True")
                {
                    chkTimePrepared.Checked = true;
                }
                else
                {
                    chkTimePrepared.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["PageNumber"].ToString() != "")
                {
                    drpPageNumber.Enabled = true;
                    drpPageNumber.SelectedValue = dsGetHeaderFooterDetail.Tables[0].Rows[0]["PageNumber"].ToString();
                    chkPageNumber.Checked = true;
                }
                else
                {
                    drpPageNumber.Enabled = false;
                    drpPageNumber.SelectedIndex = 0;
                    chkPageNumber.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["ExtraFooterLine"].ToString() != "")
                {
                    txtExtraFooterLine.Enabled = true;
                    txtExtraFooterLine.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["ExtraFooterLine"].ToString();
                    chkExtraFootLine.Checked = true;
                }
                else
                {
                    txtExtraFooterLine.Enabled = false;
                    txtExtraFooterLine.Text = "";
                    chkExtraFootLine.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() != "")
                {
                    drpAlignment.SelectedValue = dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString();
                }
                else
                {
                    drpDatePrepared.SelectedIndex = 0;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["PDFSize"].ToString() != "")
                {
                    drpPDFPageSize.SelectedValue = dsGetHeaderFooterDetail.Tables[0].Rows[0]["PDFSize"].ToString();
                }
            }
        }
        catch (Exception e)
        {
        }
    }

    private void BindGridReport(string[] checkedColumns, string[] selectedFiltersColumns, string[] selectedFiltersValues, string sortBy)
    {
        string query = "SELECT distinct ";

        foreach (var item in checkedColumns)
        {
            query += "[" + item + "],";
        }

        query = query.Substring(0, query.Length - 1);
        if (selectedFiltersColumns == null)
        {
            query += " FROM vw_OpenJobReport order by " + sortBy;
        }
        else
        {
            string filters = string.Empty;
            if (selectedFiltersColumns != null)
            {
                for (int i = 0; i <= selectedFiltersColumns.Count() - 1; i++)
                {

                    if (selectedFiltersColumns[i] == "Project#")
                    {
                        selectedFiltersColumns[i] = "Project #";
                    }

                    if (selectedFiltersColumns[i] == "NewContractSigned")
                    {
                        selectedFiltersColumns[i] = "New Contract Signed";
                    }

                    if (selectedFiltersColumns[i] == "OrderPrints")
                    {
                        selectedFiltersColumns[i] = "Order Prints";
                    }

                    if (selectedFiltersColumns[i] == "DCAApproval")
                    {
                        selectedFiltersColumns[i] = "DCA Approval";
                    }

                    if (selectedFiltersColumns[i] == "DownPymtRecd")
                    {
                        selectedFiltersColumns[i] = "Down Pymt Recd";
                    }

                    if (selectedFiltersColumns[i] == "FinalSelectionsReceived")
                    {
                        selectedFiltersColumns[i] = "Final Selections Received";
                    }

                    if (selectedFiltersColumns[i] == "ElevatorOrdered")
                    {
                        selectedFiltersColumns[i] = "Elevator Ordered";
                    }

                    if (selectedFiltersColumns[i] == "ElevatorDelivered")
                    {
                        selectedFiltersColumns[i] = "Elevator Delivered";
                    }

                    if (selectedFiltersColumns[i] == "UnitFinished")
                    {
                        selectedFiltersColumns[i] = "Unit Finished";
                    }

                    if (selectedFiltersColumns[i].ToLower() != "balance" && selectedFiltersColumns[i].ToLower() != "loc" && selectedFiltersColumns[i].ToLower() != "equip" && selectedFiltersColumns[i].ToLower() != "opencall" && selectedFiltersColumns[i].ToLower() != "equipmentprice" && selectedFiltersColumns[i].ToLower() != "equipmentcounts")
                    {
                        if (!selectedFiltersValues[i].Contains("'") && !selectedFiltersValues[i].Contains("|"))
                        {
                            filters += selectedFiltersColumns[i] + "=" + "'" + selectedFiltersValues[i] + "'" + " AND ";
                        }
                        else
                        {
                            int indexOfSingleQuote = selectedFiltersValues[i].IndexOf("'");
                            if (indexOfSingleQuote == 0)
                            {
                                filters += selectedFiltersColumns[i] + " in (" + selectedFiltersValues[i].Replace('|', ',') + ")" + " AND ";
                            }
                            else
                            {
                                filters += selectedFiltersColumns[i] + " in ('" + selectedFiltersValues[i].Replace('|', ',') + ")" + " AND ";
                            }
                        }
                    }
                    else
                    {
                        if (selectedFiltersValues[i].Contains("and"))
                        {
                            filters += selectedFiltersColumns[i] + selectedFiltersValues[i].Replace("and", "and " + selectedFiltersColumns[i] + "") + " AND ";
                        }
                        else
                        {
                            filters += selectedFiltersColumns[i] + selectedFiltersValues[i] + " AND ";
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(filters))
            {
                filters = filters.Substring(0, filters.Length - 4);
                query += " FROM vw_OpenJobReport where " + filters + " order by " + sortBy;
            }
            else
            {
                query += " FROM vw_OpenJobReport order by " + sortBy;
            }
        }

        Session["Query"] = query;
        GetGridData(query, true);
    }

    private void GetGridData(string query, bool isDataBind = false)
    {
        try
        {
            DataSet ds = new DataSet();
            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.ConnConfig = Session["config"].ToString();
            ds = objBL_ReportsData.GetOwners(query, objPropUser);

            Session["OpenJobData"] = ds;
            DataTable dt = ds.Tables[0].Clone();

            var filterString = RadGrid_Project.MasterTableView.FilterExpression;

            if (!string.IsNullOrEmpty(filterString))
            {
                var data = ds.Tables[0].Select(filterString, "");

                if (data.Count() > 0)
                {
                    dt = data.CopyToDataTable();
                }
            }
            else
            {
                dt = ds.Tables[0];
            }

            RadGrid_Project.DataSource = dt;
            lblRecordCount.Text = dt.Rows.Count.ToString() + " Record(s) found";

            if (isDataBind)
            {
                RadGrid_Project.DataBind();
            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void SaveResizedAndReorderReport()
    {
        try
        {
            var columnsInGrid = new List<Tuple<int, string>>();
            var columnWidthsInGrid = new List<Tuple<int, string>>();
            var gridWidth = 0;

            foreach (GridBoundColumn col in RadGrid_Project.MasterTableView.Columns)
            {
                columnsInGrid.Add(Tuple.Create(col.OrderIndex, col.DataField));
                columnWidthsInGrid.Add(Tuple.Create(col.OrderIndex, col.HeaderStyle.Width.Value.ToString()));

                gridWidth += Convert.ToInt32(col.HeaderStyle.Width.Value);
            }

            Session["OpenJobWidth"] = gridWidth;

            var columns = columnsInGrid.OrderBy(x => x.Item1).Select(x => x.Item2).ToList();
            var columnWidths = columnWidthsInGrid.OrderBy(x => x.Item1).Select(x => x.Item2).ToList();

            // Get origin labels
            var dtCustomLabel = GetJobCustomLabel();

            foreach (DataRow dr in dtCustomLabel.Rows)
            {
                int index = columns.FindIndex(x => x == dr["CustomLabel"].ToString());

                if (index >= 0)
                {
                    columns[index] = string.Format("Custom{0}", dr["ID"]);
                }
            }

            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            objCustReport.UserId = Convert.ToInt32(Session["UserID"].ToString());
            objCustReport.ReportId = Convert.ToInt32(Request.QueryString["reportId"]);
            objCustReport.ColumnName = string.Join("^", columns);
            objCustReport.ColumnWidth = string.Join("^", columnWidths);
            objBL_ReportsData.UpdateCustomerReportResizedWidth(objCustReport);
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void lnkExportPDF_Click(object sender, EventArgs e)
    {
        DeletePDFFiles();

        ViewState["btnEmailReport"] = "0";
        ExportPDFFiles();
    }

    protected void lnkExportExcel_Click(object sender, EventArgs e)
    {
        DeleteExcelFiles();

        ViewState["btnEmailReport"] = "0";
        ExportExcelFiles();
    }

    protected void lnkEmailAsPDF_Click(object sender, EventArgs e)
    {
        var name = Regex.Replace(drpReports.SelectedItem.Text, @"[^0-9a-zA-Z\._]", "_");
        string fileName = $"{name}.pdf";

        ArrayList lstPath = new ArrayList();
        lstPath.Add(fileName);

        ViewState["pathmailatt"] = lstPath;
        dlAttachmentsDelete.DataSource = lstPath;
        dlAttachmentsDelete.DataBind();

        hdnFirstAttachement.Value = fileName;
        ClientScript.RegisterStartupScript(Page.GetType(), "showMailReport", "showMailReport();", true);
    }

    protected void lnkEmailAsExcel_Click(object sender, EventArgs e)
    {
        var name = Regex.Replace(drpReports.SelectedItem.Text, @"[^0-9a-zA-Z\._]", "_");
        string fileName = $"{name}.xls";
        ArrayList lstPath = new ArrayList();
        lstPath.Add(fileName);

        ViewState["pathmailatt"] = lstPath;
        dlAttachmentsDelete.DataSource = lstPath;
        dlAttachmentsDelete.DataBind();

        hdnFirstAttachement.Value = fileName;
        ClientScript.RegisterStartupScript(Page.GetType(), "showMailReport", "showMailReport();", true);
    }

    private void ExportPDFFiles()
    {
        try
        {
            var width = 0;
            var height = 0;
            var fileName = Regex.Replace(drpReports.SelectedItem.Text, @"[^0-9a-zA-Z\._]", "_");

            if (drpPDFPageSize.SelectedValue == "Auto")
            {
                width = Session["OpenJobWidth"] != null ? (int)Session["OpenJobWidth"] + 80 : 0;

                RadGrid_Project.ExportSettings.Pdf.PageWidth = new Unit(width);
            }
            else
            {
                width = Convert.ToInt32(PageSize.GetRectangle(drpPDFPageSize.SelectedValue).Width);
                height = Convert.ToInt32(PageSize.GetRectangle(drpPDFPageSize.SelectedValue).Height);

                RadGrid_Project.ExportSettings.Pdf.PageWidth = new Unit(width);
                RadGrid_Project.ExportSettings.Pdf.PageHeight = new Unit(height);
            }

            RadGrid_Project.ExportSettings.FileName = fileName;
            RadGrid_Project.ExportSettings.ExportOnlyData = true;
            RadGrid_Project.ExportSettings.OpenInNewWindow = true;
            RadGrid_Project.ExportSettings.IgnorePaging = true;

            RadGrid_Project.ExportSettings.Pdf.PageTopMargin = new Unit(40);
            RadGrid_Project.ExportSettings.Pdf.PageBottomMargin = new Unit(40);
            RadGrid_Project.ExportSettings.Pdf.PageLeftMargin = new Unit(40);
            RadGrid_Project.ExportSettings.Pdf.PageRightMargin = new Unit(40);
            RadGrid_Project.ExportSettings.Pdf.PageHeader.MiddleCell.Text = string.Empty;
            RadGrid_Project.ExportSettings.Pdf.PageHeader.LeftCell.Text = string.Empty;
            RadGrid_Project.ExportSettings.Pdf.PageFooter.MiddleCell.Text = string.Empty;
            RadGrid_Project.ExportSettings.Pdf.BorderType = GridPdfSettings.GridPdfBorderType.AllBorders;
            RadGrid_Project.ExportSettings.Pdf.ContentFilter = GridPdfFilter.NoFilter;
            RadGrid_Project.MasterTableView.ExportToPdf();
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void ExportExcelFiles()
    {
        try
        {
            var fileName = Regex.Replace(drpReports.SelectedItem.Text, @"[^0-9a-zA-Z\._]", "_");

            RadGrid_Project.ExportSettings.FileName = fileName;
            RadGrid_Project.ExportSettings.ExportOnlyData = true;
            RadGrid_Project.ExportSettings.OpenInNewWindow = true;
            RadGrid_Project.ExportSettings.HideStructureColumns = true;
            RadGrid_Project.MasterTableView.UseAllDataFields = true;
            RadGrid_Project.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
            RadGrid_Project.MasterTableView.ExportToExcel();
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void DeletePDFFiles()
    {
        if (Directory.Exists(Server.MapPath("ReportFiles/PDF")))
        {
            string[] filePaths = Directory.GetFiles(Server.MapPath("ReportFiles/PDF"));
            foreach (string filePath in filePaths)
            {
                try
                {
                    File.Delete(filePath);
                }
                catch
                {
                }
            }
        }
    }

    private void DeleteExcelFiles()
    {
        if (Directory.Exists(Server.MapPath("ReportFiles/Excel")))
        {
            string[] filePaths = Directory.GetFiles(Server.MapPath("ReportFiles/Excel"));
            foreach (string filePath in filePaths)
            {
                try
                {
                    File.Delete(filePath);
                }
                catch
                {
                }
            }
        }
    }

    #region Send Email

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

        lblEmailRecordCount.Text = emailList.Rows.Count + " Record(s) found";
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
            txtSubject.Text = drpReports.SelectedItem.Text;
            //txtFrom.ReadOnly = true;
        }
    }

    protected void hideModalPopupViaServerConfirm_Click(object sender, EventArgs e)
    {
        var fileName = Regex.Replace(drpReports.SelectedItem.Text, @"[^0-9a-zA-Z\._]", "_");

        if (hdnFirstAttachement.Value.Contains(".pdf"))
        {
            var filePath = Server.MapPath($"ReportFiles/PDF/{fileName}.pdf");

            if (File.Exists(filePath))
            {
                SendEmail();
            }
            else
            {
                ViewState["btnEmailReport"] = "1";
                ExportPDFFiles();
            }
        }
        else
        {
            var filePath = Server.MapPath($"ReportFiles/Excel/{fileName}.xls");

            if (File.Exists(filePath))
            {
                SendEmail();
            }
            else
            {
                ViewState["btnEmailReport"] = "1";
                ExportExcelFiles();
            }
        }

    }

    private void SendEmail()
    {
        if (txtTo.Text.Trim() != string.Empty)
        {
            try
            {
                var fileName = Regex.Replace(drpReports.SelectedItem.Text, @"[^0-9a-zA-Z\._]", "_");

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

                mail.Title = txtSubject.Text;
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = $"This is report email sent from Mobile Office Manager. Please find the {fileName} attached.";
                }

                var filePath = string.Empty;
                if (hdnFirstAttachement.Value.Contains(".pdf"))
                {
                    filePath = Server.MapPath($"ReportFiles/PDF/{fileName}.pdf");
                }
                else
                {
                    filePath = Server.MapPath($"ReportFiles/Excel/{fileName}.xls");
                }

                byte[] buffer = null;

                if (File.Exists(filePath))
                {
                    buffer = File.ReadAllBytes(filePath);
                }

                if (hdnFirstAttachement.Value != "-1")
                {
                    mail.attachmentBytes = buffer;
                }

                ArrayList lst = new ArrayList();
                if (ViewState["pathmailatt"] != null)
                {
                    lst = (ArrayList)ViewState["pathmailatt"];
                    foreach (string strpath in lst)
                    {
                        if (!strpath.Contains(fileName))
                        {
                            mail.AttachmentFiles.Add(strpath);
                        }
                        else
                        {
                            mail.FileName = strpath;
                        }
                    }
                }

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

        var fileName = Regex.Replace(drpReports.SelectedItem.Text, @"[^0-9a-zA-Z\._]", "_");

        if (path.Contains($"{fileName}.pdf"))
        {
            path = Server.MapPath($"ReportFiles/PDF/{fileName}.pdf");
        }
        else if (path.Contains($"{fileName}.xls"))
        {
            path = Server.MapPath($"ReportFiles/Excel/{fileName}.xls");
        }

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
                Response.AddHeader("Content-Disposition", "attachment;filename=" + DownloadFileName);
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
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "FileaccessWarning", "alert('File not found.');", true);
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

        string mailBody = $"Please review the attached {drpReports.SelectedItem.Text}.";
        address = mailBody + Environment.NewLine + "<br />" + Environment.NewLine + "<br />" + address;

        txtBody.Text = address;

        
    }

    #endregion

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