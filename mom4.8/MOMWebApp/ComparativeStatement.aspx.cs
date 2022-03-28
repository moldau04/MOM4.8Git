using BusinessEntity;
using BusinessLayer;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Stimulsoft.Report;
using Telerik.Web.UI;
using Stimulsoft.Report.Components;
using Stimulsoft.Base.Drawing;
using System.Drawing;
using Stimulsoft.Report.Dictionary;
using System.Configuration;
using System.Reflection;
using ReportLayer.IncomeStatements;
using Microsoft.ApplicationBlocks.Data;
using Stimulsoft.Report.Components.TextFormats;

public partial class ComparativeStatement : System.Web.UI.Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    Customer objPropCustomer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    Chart objChart = new Chart();
    BL_Chart objBL_Chart = new BL_Chart();

    BL_Report bL_Report = new BL_Report();
    BL_Budgets bL_Budgets = new BL_Budgets();

    protected DataSet _dsColumns = new DataSet();

    protected DataTable _revenuesTotal = new DataTable();
    protected DataTable _costOfSalesTotal = new DataTable();
    protected DataTable _expensesTotal = new DataTable();
    protected DataTable _otherIncomeTotal = new DataTable();
    protected DataTable _incomeTaxesTotal = new DataTable();
    protected DataTable _grossProfit = new DataTable();
    protected DataTable _grossProfitPercen = new DataTable();
    protected DataTable _netProfit = new DataTable();
    protected DataTable _netProfitPercen = new DataTable();
    protected DataTable _beforeProvisions = new DataTable();
    protected DataTable _beforeProvisionsPercen = new DataTable();
    protected DataTable _netIncome = new DataTable();
    protected DataTable _netIncomePercen = new DataTable();
    protected DataTable _consolidating = new DataTable();

    protected bool _includeProvisions = false;
    protected bool _includeProvisionsTotal = false;
    protected List<Tuple<int, bool, bool>> colIncludeProvisions;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {
            string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

            if (Request.Url.Scheme == "http" && SSL == "1")
            {
                string URL = Request.Url.ToString();
                URL = URL.Replace("http://", "https://");
                Response.Redirect(URL);
            }

            if (Session["SaveComparativeStatement"] != null)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Report saved successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                Session.Remove("SaveComparativeStatement");
            }

            if (Session["DeletePermissionError"] != null)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningSave", "noty({text: 'You do not have permission to delete this report!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                Session.Remove("DeletePermissionError");
            }

            if (Request.QueryString["id"] != null)
            {
                lnkCopyReport.Visible = true;
                linkDeleteReport.Visible = true;
            }
            else
            {
                lnkCopyReport.Visible = false;
                linkDeleteReport.Visible = false;
            }

            Session["ReportName"] = HttpUtility.UrlDecode(Request.QueryString["reportName"]);
            HighlightSideMenu("financialStatement", "lnkComparativeStatement", "financeStateSub");

            BindingCenter();
            LoadControl();
            BindingComparativeReport();
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    protected void StiWebViewerComparative_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        e.Report = LoadReport();
    }

    protected void StiWebViewerComparative_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {

    }

    protected void gvListColumns_DeleteCommand(object source, GridCommandEventArgs e)
    {
        string ID = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Index"].ToString();

        _dsColumns.Tables.Add(BuildColumnDetailsTable());

        int line = 1;
        foreach (GridDataItem row in gvListColumns.Items)
        {
            var rowIndex = Convert.ToInt32(row.GetDataKeyValue("Index").ToString());

            if (rowIndex.ToString() != ID)
            {
                DataRow dr = _dsColumns.Tables[0].NewRow();

                DropDownList lblDbType = (DropDownList)row.FindControl("lblDbType");
                TextBox lblDbLabel = (TextBox)row.FindControl("lblDbLabel");
                TextBox lblDbFromDate = (TextBox)row.FindControl("lblDbFromDate");
                TextBox lblDbToDate = (TextBox)row.FindControl("lblDbToDate");
                DropDownList lblDbColumn1 = (DropDownList)row.FindControl("lblDbColumn1");
                DropDownList lblDbColumn2 = (DropDownList)row.FindControl("lblDbColumn2");
                RadComboBox ddlBudget = (RadComboBox)row.FindControl("rcbDbBudget");

                dr["Line"] = line;
                dr["Index"] = rowIndex;
                dr["Type"] = lblDbType.SelectedValue;
                dr["Budget"] = ddlBudget.Text;
                dr["Label"] = lblDbLabel.Text;
                if (lblDbType.SelectedValue != "Difference" && lblDbType.SelectedValue != "Variance")
                {
                    dr["FromDate"] = lblDbFromDate.Text;
                    dr["ToDate"] = lblDbToDate.Text;
                    dr["Column1"] = "";
                    dr["Column2"] = "";
                }
                else
                {
                    dr["FromDate"] = "";
                    dr["ToDate"] = "";
                    dr["Column1"] = lblDbColumn1.SelectedValue;
                    dr["Column2"] = lblDbColumn2.SelectedValue;
                }

                _dsColumns.Tables[0].Rows.Add(dr);

                line++;
            }
        }

        gvListColumns.DataSource = _dsColumns.Tables[0];
        gvListColumns.DataBind();
    }

    protected void lnkAddnewRow_Click(object sender, EventArgs e)
    {
        _dsColumns.Tables.Add(BuildColumnDetailsTable());

        int index = 1;
        foreach (GridDataItem row in gvListColumns.Items)
        {
            DataRow dr = _dsColumns.Tables[0].NewRow();

            HiddenField rowLine = (HiddenField)row.FindControl("txtRowLine");
            DropDownList lblDbType = (DropDownList)row.FindControl("lblDbType");
            TextBox lblDbLabel = (TextBox)row.FindControl("lblDbLabel");
            TextBox lblDbFromDate = (TextBox)row.FindControl("lblDbFromDate");
            TextBox lblDbToDate = (TextBox)row.FindControl("lblDbToDate");
            DropDownList lblDbColumn1 = (DropDownList)row.FindControl("lblDbColumn1");
            DropDownList lblDbColumn2 = (DropDownList)row.FindControl("lblDbColumn2");
            RadComboBox ddlBudget = (RadComboBox)row.FindControl("rcbDbBudget");

            var rowIndex = Convert.ToInt32(row.GetDataKeyValue("Index").ToString());

            dr["Line"] = Convert.ToInt32(rowLine.Value);
            dr["Index"] = rowIndex;
            dr["Type"] = lblDbType.SelectedValue;
            dr["Budget"] = ddlBudget.Text;
            dr["Label"] = lblDbLabel.Text;
            if (lblDbType.SelectedValue != "Difference" && lblDbType.SelectedValue != "Variance")
            {
                dr["FromDate"] = lblDbFromDate.Text;
                dr["ToDate"] = lblDbToDate.Text;
                dr["Column1"] = "";
                dr["Column2"] = "";
            }
            else
            {
                dr["FromDate"] = "";
                dr["ToDate"] = "";
                dr["Column1"] = lblDbColumn1.SelectedValue;
                dr["Column2"] = lblDbColumn2.SelectedValue;
            }

            _dsColumns.Tables[0].Rows.Add(dr);

            index = Math.Max(index, rowIndex + 1);
        }

        var newRow = _dsColumns.Tables[0].NewRow();
        newRow["Line"] = gvListColumns.Items.Count + 1;
        newRow["Index"] = index;
        _dsColumns.Tables[0].Rows.Add(newRow);

        gvListColumns.DataSource = _dsColumns.Tables[0];
        gvListColumns.DataBind();
    }

    protected void gvListColumns_ItemDataBound(object source, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            GridEditableItem item = (GridEditableItem)e.Item;
            DropDownList ddlType = (DropDownList)item.FindControl("lblDbType");
            RadComboBox ddlBudget = (RadComboBox)item.FindControl("rcbDbBudget");

            Panel panelFromDate = (Panel)item.FindControl("panelFromDate");
            Panel panelToDate = (Panel)item.FindControl("panelToDate");
            Panel panelColumn1 = (Panel)item.FindControl("panelColumn1");
            Panel panelColumn2 = (Panel)item.FindControl("panelColumn2");

            if (ddlType != null)
            {
                var dsBudget = bL_Budgets.GetBudget(Session["config"].ToString(), null);
                ddlBudget.DataSource = dsBudget;
                ddlBudget.DataTextField = "Budget";
                ddlBudget.DataValueField = "Budget";
                ddlBudget.DataBind();

                DataTable typeTable = new DataTable();
                typeTable.Columns.Add("Type");
                typeTable.Columns.Add("Name");

                typeTable.Rows.Add("", "--Select--");
                typeTable.Rows.Add("Actual", "Actual");

                if (Request.QueryString["type"] == null || Request.QueryString["type"] == "ProfitAndLoss")
                {
                    typeTable.Rows.Add("Budget", "Budget");
                }

                typeTable.Rows.Add("Difference", "Difference");
                typeTable.Rows.Add("Variance", "% Variance");

                ddlType.DataSource = typeTable;
                ddlType.DataTextField = "Name";
                ddlType.DataValueField = "Type";
                ddlType.DataBind();

                ddlType.SelectedValue = DataBinder.Eval(item.DataItem, "Type").ToString();

                if (ddlType.SelectedValue == "Difference" || ddlType.SelectedValue == "Variance")
                {
                    panelFromDate.Attributes.Add("style", "display: none");
                    panelToDate.Attributes.Add("style", "display: none");
                    panelColumn1.Attributes.Add("style", "display: block");
                    panelColumn2.Attributes.Add("style", "display: block");
                    ddlBudget.Attributes.Add("style", "display: none");
                }
                else
                {
                    panelToDate.Attributes.Add("style", "display: block");
                    panelColumn1.Attributes.Add("style", "display: none");
                    panelColumn2.Attributes.Add("style", "display: none");

                    if (ddlStates.SelectedValue == "ProfitAndLoss")
                    {
                        panelFromDate.Attributes.Add("style", "display: block");

                        if (ddlType.SelectedValue == "Budget")
                        {
                            ddlBudget.Attributes.Add("style", "display: block");
                        }
                        else
                        {
                            ddlBudget.Attributes.Add("style", "display: none");
                        }
                    }
                    else if (ddlStates.SelectedValue == "BalanceSheet")
                    {
                        panelFromDate.Attributes.Add("style", "display: none");
                    }
                }
            }

            var listColumnIndex = GetColumnIndex();
            if (listColumnIndex.Rows.Count > 0)
            {
                DropDownList ddlDbColumn1 = (DropDownList)item.FindControl("lblDbColumn1");
                DropDownList ddlDbColumn2 = (DropDownList)item.FindControl("lblDbColumn2");

                if (ddlDbColumn1 != null)
                {
                    ddlDbColumn1.DataSource = listColumnIndex;
                    ddlDbColumn1.DataTextField = "Name";
                    ddlDbColumn1.DataValueField = "Index";
                    ddlDbColumn1.DataBind();
                    ddlDbColumn1.SelectedValue = DataBinder.Eval(item.DataItem, "Column1").ToString();
                }

                if (ddlDbColumn2 != null)
                {
                    ddlDbColumn2.DataSource = listColumnIndex;
                    ddlDbColumn2.DataTextField = "Name";
                    ddlDbColumn2.DataValueField = "Index";
                    ddlDbColumn2.DataBind();
                    ddlDbColumn2.SelectedValue = DataBinder.Eval(item.DataItem, "Column2").ToString();
                }
            }
        }
    }

    protected void ddlStates_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session.Remove("ComparativeStatementRequest");
        Session.Remove("ComparativeCenter");
        Session.Remove("SelectedCenters");

        string urlString = "ComparativeStatement.aspx?type=" + ddlStates.SelectedValue;

        Response.Redirect(urlString, true);
    }

    private void LoadControl()
    {
        if (!string.IsNullOrEmpty(Request.QueryString["showReport"]) && Convert.ToBoolean(Request.QueryString["showReport"]))
        {
            StiWebViewerComparative.Visible = true;
        }
        else
        {
            Session.Remove("ComparativeStatementRequest");
            Session.Remove("ComparativeCenter");
            Session.Remove("SelectedCenters");
        }

        if (!string.IsNullOrEmpty(Request.QueryString["reloadReport"]) && Convert.ToBoolean(Request.QueryString["reloadReport"]))
        {
            if (Session["ComparativeCenter"] != null)
            {
                var depts = Session["ComparativeCenter"].ToString();
                var deptArray = depts.Split(',');

                for (int i = 0; i < deptArray.Length; i++)
                {
                    RadComboBoxItem item = rcCenter.FindItemByValue(deptArray[i]);
                    if (item != null)
                        item.Checked = true;
                }
            }
        }
        else
        {
            Session.Remove("ComparativeStatementRequest");
            Session.Remove("ComparativeCenter");
            Session.Remove("SelectedCenters");

            // Load report by ID
            if (Request.QueryString["id"] != null)
            {
                LoadReportByID(Convert.ToInt32(Request.QueryString["id"]), true);
            }
            else if (Request.QueryString["cid"] != null)
            {
                LoadReportByID(Convert.ToInt32(Request.QueryString["cid"]), false);
            }
        }

        var reportType = "ExpandAll";
        if (!string.IsNullOrEmpty(Request.QueryString["reportType"]))
        {
            reportType = Request.QueryString["reportType"];
        }

        if (reportType == "ExpandAll")
        {
            rdExpandAll.Checked = true;
        }
        else if (reportType == "DetailWithSub")
        {
            rdDetailWithSub.Checked = true;
        }
        else if (reportType == "CollapseAll")
        {
            rdCollapseAll.Checked = true;
        }

        if (!string.IsNullOrEmpty(Request.QueryString["reportName"]))
        {
            txtReportTitle.Value = HttpUtility.UrlDecode(Request.QueryString["reportName"]);
        }

        if (Request.QueryString["type"] != null)
        {
            ddlStates.SelectedValue = Request.QueryString["type"];

            if (ddlStates.SelectedValue == "ProfitAndLoss")
            {
                pageTitle.Text = "Comparative Profit and Loss Statement";

                gvListColumns.Columns[4].HeaderText = "Start Date/Column";
                gvListColumns.Columns[5].HeaderText = "End Date/Column";
            }
            else if (ddlStates.SelectedValue == "BalanceSheet")
            {
                pageTitle.Text = "Comparative Balance Sheet";

                gvListColumns.Columns[4].HeaderText = "Column";
                gvListColumns.Columns[5].HeaderText = "As of Date/Column";
            }
        }

        // Load Columns
        if (Session["ComparativeStatementRequest"] != null)
        {
            var objComparative = (List<ComparativeStatementRequest>)Session["ComparativeStatementRequest"];
            var colTable = BuildColumnDetailsTable();

            foreach (var col in objComparative)
            {
                var dr = colTable.NewRow();
                dr["Line"] = col.Line;
                dr["Index"] = col.Index;
                dr["Label"] = col.Label;
                dr["FromDate"] = col.StartDate;
                dr["ToDate"] = col.EndDate;
                dr["Column1"] = col.Column1;
                dr["Column2"] = col.Column2;

                if (col.Type != "Actual" && col.Type != "Difference" && col.Type != "Variance")
                {
                    dr["Budget"] = col.Type;
                    dr["Type"] = "Budget";
                }
                else
                {
                    dr["Type"] = col.Type;
                }

                colTable.Rows.Add(dr);
            }

            _dsColumns.Tables.Add(colTable);
        }
        else
        {
            _dsColumns.Tables.Add(BuildColumnDetailsTable());
        }

        gvListColumns.DataSource = _dsColumns;
        gvListColumns.DataBind();
    }

    protected void lblDbLabel_TextChanged(object sender, EventArgs e)
    {
        TextBox lblDbLabel = (TextBox)sender;
        GridDataItem dataItem = (GridDataItem)lblDbLabel.NamingContainer;
        DropDownList ddlType = (DropDownList)dataItem.FindControl("lblDbType");

        if (ddlType.SelectedValue != "Difference" && ddlType.SelectedValue != "Variance")
        {
            var index = dataItem.GetDataKeyValue("Index").ToString();

            foreach (GridDataItem row in gvListColumns.Items)
            {
                DropDownList ddlDbColumn1 = (DropDownList)row.FindControl("lblDbColumn1");
                DropDownList ddlDbColumn2 = (DropDownList)row.FindControl("lblDbColumn2");

                if (ddlDbColumn1 != null)
                {
                    var ddlItem = ddlDbColumn1.Items.FindByValue(index);

                    if (ddlItem != null)
                    {
                        ddlItem.Text = lblDbLabel.Text;
                    }
                }

                if (ddlDbColumn2 != null)
                {
                    var ddlItem = ddlDbColumn2.Items.FindByValue(index);

                    if (ddlItem != null)
                    {
                        ddlItem.Text = lblDbLabel.Text;
                    }
                }
            }
        }
    }

    protected void lblDbType_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlType = (DropDownList)sender;
        GridDataItem dataItem = (GridDataItem)ddlType.NamingContainer;

        Panel panelFromDate = (Panel)dataItem.FindControl("panelFromDate");
        Panel panelToDate = (Panel)dataItem.FindControl("panelToDate");
        Panel panelColumn1 = (Panel)dataItem.FindControl("panelColumn1");
        Panel panelColumn2 = (Panel)dataItem.FindControl("panelColumn2");
        RadComboBox ddlBudget = (RadComboBox)dataItem.FindControl("rcbDbBudget");

        if (ddlType.SelectedValue == "Difference" || ddlType.SelectedValue == "Variance")
        {
            var index = dataItem.GetDataKeyValue("Index").ToString();

            foreach (GridDataItem row in gvListColumns.Items)
            {
                DropDownList ddlDbColumn1 = (DropDownList)row.FindControl("lblDbColumn1");
                DropDownList ddlDbColumn2 = (DropDownList)row.FindControl("lblDbColumn2");

                if (ddlDbColumn1 != null)
                {
                    var checkItem = ddlDbColumn1.Items.FindByValue(index);

                    if (checkItem != null)
                    {
                        ddlDbColumn1.Items.Remove(checkItem);
                    }
                }

                if (ddlDbColumn2 != null)
                {
                    var checkItem = ddlDbColumn2.Items.FindByValue(index);

                    if (checkItem != null)
                    {
                        ddlDbColumn2.Items.Remove(checkItem);
                    }
                }
            }

            panelFromDate.Attributes.Add("style", "display: none");
            panelToDate.Attributes.Add("style", "display: none");
            panelColumn1.Attributes.Add("style", "display: block");
            panelColumn2.Attributes.Add("style", "display: block");
            ddlBudget.Attributes.Add("style", "display: none");
        }
        else
        {
            var index = dataItem.GetDataKeyValue("Index").ToString();
            TextBox lblDbLabel = (TextBox)dataItem.FindControl("lblDbLabel");

            foreach (GridDataItem row in gvListColumns.Items)
            {
                DropDownList ddlDbColumn1 = (DropDownList)row.FindControl("lblDbColumn1");
                DropDownList ddlDbColumn2 = (DropDownList)row.FindControl("lblDbColumn2");

                if (ddlDbColumn1 != null)
                {
                    var checkItem = ddlDbColumn1.Items.FindByValue(index);

                    if (checkItem == null)
                    {
                        ddlDbColumn1.Items.Add(new ListItem(lblDbLabel.Text, index));
                    }
                }

                if (ddlDbColumn2 != null)
                {
                    var checkItem = ddlDbColumn2.Items.FindByValue(index);

                    if (checkItem == null)
                    {
                        ddlDbColumn2.Items.Add(new ListItem(lblDbLabel.Text, index));
                    }
                }
            }

            panelToDate.Attributes.Add("style", "display: block");
            panelColumn1.Attributes.Add("style", "display: none");
            panelColumn2.Attributes.Add("style", "display: none");

            if (ddlStates.SelectedValue == "ProfitAndLoss")
            {
                panelFromDate.Attributes.Add("style", "display: block");

                if (ddlType.SelectedValue == "Budget")
                {
                    ddlBudget.Attributes.Add("style", "display: block");
                }
                else
                {
                    ddlBudget.Attributes.Add("style", "display: none");
                }
            }
            else if (ddlStates.SelectedValue == "BalanceSheet")
            {
                panelFromDate.Attributes.Add("style", "display: none");
            }
        }
    }

    protected void rcbDbBudget_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["Budget"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["Budget"].ToString();
    }

    protected void lnkLoadReport_Click(object sender, EventArgs e)
    {
        if (ColumnValidation(false))
        {
            LoadReportData();

            string urlString = "ComparativeStatement.aspx?type=" + ddlStates.SelectedValue;
            if (Request.QueryString["id"] != null)
            {
                urlString += "&id=" + Request.QueryString["id"];
            }
            else if (Request.QueryString["cid"] != null)
            {
                urlString += "&cid=" + Request.QueryString["cid"];
            }

            var reportType = "ExpandAll";
            if (rdDetailWithSub.Checked)
            {
                reportType = "DetailWithSub";
            }
            else if (rdCollapseAll.Checked)
            {
                reportType = "CollapseAll";
            }

            urlString += "&reportType=" + reportType;
            urlString += "&reportName=" + HttpUtility.UrlEncode(txtReportTitle.Value);
            urlString += "&showReport=true";
            urlString += "&reloadReport=true";

            Response.Redirect(urlString, true);
        }
    }

    protected void rdExpCollAll_CheckedChanged(object sender, EventArgs e)
    {
        LoadReportData();

        string urlString = "ComparativeStatement.aspx?type=" + ddlStates.SelectedValue;
        if (Request.QueryString["id"] != null)
        {
            urlString += "&id=" + Request.QueryString["id"];
        }
        else if (Request.QueryString["cid"] != null)
        {
            urlString += "&cid=" + Request.QueryString["cid"];
        }

        var reportType = string.Empty;

        if (rdExpandAll.Checked)
        {
            reportType = "ExpandAll";
        }
        if (rdDetailWithSub.Checked)
        {
            reportType = "DetailWithSub";
        }
        else if (rdCollapseAll.Checked)
        {
            reportType = "CollapseAll";
        }

        urlString += "&reportType=" + reportType;
        urlString += "&reportName=" + HttpUtility.UrlEncode(txtReportTitle.Value);
        urlString += "&showReport=true";
        urlString += "&reloadReport=true";

        Response.Redirect(urlString, true);
    }

    protected void lnkAddNew_Click(object sender, EventArgs e)
    {
        string urlString = "ComparativeStatement.aspx?type=" + ddlStates.SelectedValue;

        Response.Redirect(urlString);
    }

    protected void lnkCopyReport_Click(object sender, EventArgs e)
    {
        Response.Redirect("ComparativeStatement.aspx?cid=" + Request.QueryString["id"]);
    }

    protected void linkDeleteReport_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["id"] != null)
            {
                var connString = Session["config"].ToString();
                var userId = Session["userid"].ToString();
                var reportID = Convert.ToInt32(Request.QueryString["id"]);

                var report = bL_Report.GetComparativeReportByID(connString, reportID);
                if (report.Tables.Count > 0 && report.Tables[0].Rows.Count > 0 && report.Tables[0].Rows[0]["UserID"].ToString() == userId)
                {
                    bL_Report.DeleteComparativeReportByID(connString, reportID);
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Report deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 2000,theme : 'noty_theme_default',  closable : false});", true);
                    Response.Redirect("ComparativeStatement.aspx");
                }
                else
                {
                    Session["DeletePermissionError"] = true;
                    Response.Redirect(Request.RawUrl);
                }

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkSaveReport_Click(object sender, EventArgs e)
    {
        try
        {
            if (ColumnValidation(true))
            {
                var reportID = Request.QueryString["id"] != null ? Convert.ToInt32(Request.QueryString["id"]) : 0;
                var connString = Session["config"].ToString();
                var userId = Convert.ToInt32(Session["userid"].ToString());

                var checkName = bL_Report.GetComparativeReportByName(connString, txtReportTitle.Value.ToString(), ddlStates.SelectedValue, reportID);
                if (checkName.Tables[0].Rows.Count == 0)
                {
                    if (reportID > 0)
                    {
                        bL_Report.UpdateComparativeReport(connString, reportID, txtReportTitle.Value.ToString(), GetRadComboBoxSelectedItems(rcCenter));
                        bL_Report.DeleteComparativeColumnByReportID(connString, reportID);
                    }
                    else
                    {
                        reportID = bL_Report.AddComparativeReport(connString, userId, txtReportTitle.Value.ToString(), GetRadComboBoxSelectedItems(rcCenter), ddlStates.SelectedValue);
                    }

                    List<ComparativeStatementRequest> objComparative = new List<ComparativeStatementRequest>();
                    foreach (GridDataItem row in gvListColumns.Items)
                    {
                        ComparativeStatementRequest obj = new ComparativeStatementRequest();

                        var rowIndex = Convert.ToInt32(row.GetDataKeyValue("Index").ToString());

                        HiddenField rowLine = (HiddenField)row.FindControl("txtRowLine");
                        DropDownList lblDbType = (DropDownList)row.FindControl("lblDbType");
                        TextBox lblDbLabel = (TextBox)row.FindControl("lblDbLabel");
                        TextBox lblDbFromDate = (TextBox)row.FindControl("lblDbFromDate");
                        TextBox lblDbToDate = (TextBox)row.FindControl("lblDbToDate");
                        DropDownList lblDbColumn1 = (DropDownList)row.FindControl("lblDbColumn1");
                        DropDownList lblDbColumn2 = (DropDownList)row.FindControl("lblDbColumn2");
                        RadComboBox ddlBudget = (RadComboBox)row.FindControl("rcbDbBudget");

                        obj.Line = Convert.ToInt32(rowLine.Value);
                        obj.Index = rowIndex;
                        obj.Type = lblDbType.SelectedValue;
                        obj.Label = lblDbLabel.Text;

                        if (lblDbType.SelectedValue == "Budget")
                        {
                            obj.Type = ddlBudget.Text;
                        }

                        if (lblDbType.SelectedValue != "Difference" && lblDbType.SelectedValue != "Variance")
                        {
                            if (ddlStates.SelectedValue == "ProfitAndLoss")
                            {
                                obj.StartDate = Convert.ToDateTime(lblDbFromDate.Text);
                            }

                            obj.EndDate = Convert.ToDateTime(lblDbToDate.Text);
                        }
                        else
                        {
                            obj.Column1 = Convert.ToInt32(lblDbColumn1.SelectedValue);
                            obj.Column2 = Convert.ToInt32(lblDbColumn2.SelectedValue);
                        }

                        objComparative.Add(obj);
                    }

                    // Sync index by line
                    foreach (var col in objComparative)
                    {
                        if (col.Type == "Difference" || col.Type == "Variance")
                        {
                            col.Column1 = objComparative.FirstOrDefault(x => x.Index == col.Column1).Line;
                            col.Column2 = objComparative.FirstOrDefault(x => x.Index == col.Column2).Line;
                        }
                    }

                    // Save Comparative column
                    foreach (var col in objComparative)
                    {
                        // Update index by line
                        col.Index = col.Line;

                        bL_Report.AddComparativeColumn(connString, reportID, col);
                    }

                    Session["SaveComparativeStatement"] = true;

                    string urlString = "ComparativeStatement.aspx?id=" + reportID + "&showReport=true";
                    Response.Redirect(urlString);
                }
                else
                {
                    StiWebViewerComparative.Visible = false;
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningSave", "noty({text: 'Report name already exists, please use different Report name!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    txtReportTitle.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private StiReport GetComparativeProfitAndLossReport(DataTable comparativeReportData, List<ComparativeStatementRequest> objComparative)
    {
        try
        {
            string reportPathStimul = string.Empty;
            reportPathStimul = Server.MapPath("StimulsoftReports/ComparativeStatementReport.mrt");
            StiReport report = new StiReport();
            report.Load(reportPathStimul);

            // Selected department
            var departments = comparativeReportData.AsEnumerable()
                                .Select(g =>
                                {
                                    return Tuple.Create(g.Field<int>("Department"), g.Field<string>("CentralName"));
                                }).Distinct().OrderBy(x => x.Item2);

            var objColumns = objComparative;
            foreach (var column in objColumns)
            {
                report.Dictionary.DataSources["Revenues"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["CostOfSales"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["Expenses"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["OtherIncome"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["IncomeTaxes"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["RevenuesTotal"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["CostOfSalesTotal"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["ExpensesTotal"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["OtherIncomeTotal"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["IncomeTaxesTotal"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["GrossProfit"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["GrossProfitPercen"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["NetProfit"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["NetProfitPercen"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["BeforeProvisions"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["BeforeProvisionsPercen"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["NetIncome"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["NetIncomePercen"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["SummaryTotal"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));

                if (column.Type == "Actual")
                {
                    report.Dictionary.DataSources["Revenues"].Columns.Add(string.Format("Column{0}URL", column.Index), typeof(string));
                    report.Dictionary.DataSources["CostOfSales"].Columns.Add(string.Format("Column{0}URL", column.Index), typeof(string));
                    report.Dictionary.DataSources["Expenses"].Columns.Add(string.Format("Column{0}URL", column.Index), typeof(string));
                    report.Dictionary.DataSources["OtherIncome"].Columns.Add(string.Format("Column{0}URL", column.Index), typeof(string));
                    report.Dictionary.DataSources["IncomeTaxes"].Columns.Add(string.Format("Column{0}URL", column.Index), typeof(string));
                }

                if (departments.Count() > 0)
                {
                    StiDataSource dtSourceRevenues = new StiDataTableSource($"DepSummaryRevenues{column.Index}", $"DepSummaryRevenues{column.Index}", $"DepSummaryRevenues{column.Index}");
                    BuildDepartmentSummaryDataSource(dtSourceRevenues, column, departments);
                    report.Dictionary.DataSources.Add(dtSourceRevenues);

                    StiDataSource dtSourceCostOfSales = new StiDataTableSource($"DepSummaryCostOfSales{column.Index}", $"DepSummaryCostOfSales{column.Index}", $"DepSummaryCostOfSales{column.Index}");
                    BuildDepartmentSummaryDataSource(dtSourceCostOfSales, column, departments);
                    report.Dictionary.DataSources.Add(dtSourceCostOfSales);

                    StiDataSource dtSourceExpenses = new StiDataTableSource($"DepSummaryExpenses{column.Index}", $"DepSummaryExpenses{column.Index}", $"DepSummaryExpenses{column.Index}");
                    BuildDepartmentSummaryDataSource(dtSourceExpenses, column, departments);
                    report.Dictionary.DataSources.Add(dtSourceExpenses);

                    StiDataSource dtSourceOtherIncome = new StiDataTableSource($"DepSummaryOtherIncome{column.Index}", $"DepSummaryOtherIncome{column.Index}", $"DepSummaryOtherIncome{column.Index}");
                    BuildDepartmentSummaryDataSource(dtSourceOtherIncome, column, departments);
                    report.Dictionary.DataSources.Add(dtSourceOtherIncome);

                    StiDataSource dtSourceIncomeTaxes = new StiDataTableSource($"DepSummaryIncomeTaxes{column.Index}", $"DepSummaryIncomeTaxes{column.Index}", $"DepSummaryIncomeTaxes{column.Index}");
                    BuildDepartmentSummaryDataSource(dtSourceIncomeTaxes, column, departments);
                    report.Dictionary.DataSources.Add(dtSourceIncomeTaxes);

                    StiDataSource dtConsolidating = new StiDataTableSource($"Consolidating{column.Index}", $"Consolidating{column.Index}", $"Consolidating{column.Index}");
                    BuildConsolidatingDataSource(dtConsolidating, column, departments);
                    report.Dictionary.DataSources.Add(dtConsolidating);
                }
            }

            StiPage page = report.Pages[0];
            page.CanGrow = true;
            page.CanShrink = true;

            var columnCount = objColumns.Count + 2;
            double columnWidth = page.Width / columnCount;
            double pos = 0;

            var data = comparativeReportData;
            DataTable filteredTable = data.Copy();
            DataView dView = filteredTable.DefaultView;

            _revenuesTotal = BuildTotalTable(objColumns);
            _costOfSalesTotal = BuildTotalTable(objColumns);
            _expensesTotal = BuildTotalTable(objColumns);
            _otherIncomeTotal = BuildTotalTable(objColumns);
            _incomeTaxesTotal = BuildTotalTable(objColumns);
            _grossProfit = BuildTotalTable(objColumns);
            _grossProfitPercen = BuildTotalTable(objColumns);
            _netProfit = BuildTotalTable(objColumns);
            _netProfitPercen = BuildTotalTable(objColumns);
            _beforeProvisions = BuildTotalTable(objColumns);
            _beforeProvisionsPercen = BuildTotalTable(objColumns);
            _netIncome = BuildTotalTable(objColumns);
            _netIncomePercen = BuildTotalTable(objColumns);

            // Build Consolidating Divisional Financial Statements
            //if (departments.Count() > 0)
            //{
            //    BuildConsolidatingDivisional(page, objComparative, departments);
            //}

            string[] deptArray = new string[] { };
            if (Session["ComparativeCenter"] != null)
            {
                var depts = Session["ComparativeCenter"].ToString();
                deptArray = depts.Split(',');
            }

            // Build for each Department
            if (deptArray.Length > 1)
            {
                var centers = (List<Tuple<int, string>>)Session["SelectedCenters"];
                foreach (var center in centers)
                {
                    dView.RowFilter = $"Department = {center.Item1}";
                    DataTable centerData = dView.ToTable();

                    if (centerData.Rows.Count > 0)
                    {
                        CenterSummaryTotal(centerData, objColumns, center.Item1);

                        var netProfitText = "Income From Operations";

                        //Add Center title
                        StiHeaderBand centerTitle = new StiHeaderBand();
                        centerTitle.Height = 0.5;
                        centerTitle.PrintIfEmpty = true;
                        page.Components.Add(centerTitle);

                        StiText centerTitleText = new StiText(new RectangleD(0, 0.1, page.Width, 0.25));
                        centerTitleText.Text.Value = $"{center.Item2} Division";
                        centerTitleText.HorAlignment = StiTextHorAlignment.Center;
                        centerTitleText.VertAlignment = StiVertAlignment.Center;
                        centerTitleText.Font = new Font("Arial", 18F, System.Drawing.FontStyle.Bold);
                        centerTitleText.WordWrap = true;
                        centerTitle.Components.Add(centerTitleText);

                        // Revenues
                        if (objColumns.Count > 0)
                        {
                            //Create HeaderBand
                            StiHeaderBand headerBand = new StiHeaderBand();
                            headerBand.Height = 0.25;
                            headerBand.Name = $"RevenuesHeaderBand{center.Item1}";
                            headerBand.Border.Style = StiPenStyle.None;
                            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            headerBand.PrintOnAllPages = false;
                            headerBand.PrintIfEmpty = true;
                            page.Components.Add(headerBand);

                            //Create Databand
                            StiDataBand dataBand = new StiDataBand();
                            dataBand.DataSourceName = "Revenues";
                            dataBand.Name = $"RevenuesData{center.Item1}";
                            dataBand.Border.Style = StiPenStyle.None;
                            dataBand.Filters.Add(new StiFilter());
                            dataBand.Filters[0].Item = StiFilterItem.Expression;
                            dataBand.Filters[0].Expression = new StiExpression($"Revenues.Department == {center.Item1}");
                            page.Components.Add(dataBand);

                            //Create DataBand item
                            StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctText.Text.Value = "Revenues";
                            acctText.HorAlignment = StiTextHorAlignment.Left;
                            acctText.VertAlignment = StiVertAlignment.Center;
                            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            acctText.Border.Side = StiBorderSides.All;
                            acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctText.Border.Style = StiPenStyle.None;
                            acctText.TextBrush = new StiSolidBrush(Color.White);
                            acctText.WordWrap = true;
                            headerBand.Components.Add(acctText);

                            StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.2));
                            acctDataText.Text.Value = "{Revenues.fDesc}";
                            acctDataText.HorAlignment = StiTextHorAlignment.Left;
                            acctDataText.VertAlignment = StiVertAlignment.Center;
                            acctDataText.Border.Style = StiPenStyle.None;
                            acctDataText.OnlyText = false;
                            acctDataText.Border.Side = StiBorderSides.All;
                            acctDataText.Font = new Font("Arial", 8F);
                            acctDataText.WordWrap = true;
                            acctDataText.CanGrow = true;
                            acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(acctDataText);

                            pos = (columnWidth * 2);

                            foreach (var column in objColumns)
                            {
                                //Create text on header
                                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                                hText.Text.Value = column.Label;
                                hText.HorAlignment = StiTextHorAlignment.Right;
                                hText.VertAlignment = StiVertAlignment.Center;
                                hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                                hText.Border.Side = StiBorderSides.All;
                                hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                hText.Border.Style = StiPenStyle.None;
                                hText.TextBrush = new StiSolidBrush(Color.White);
                                hText.WordWrap = true;
                                hText.CanGrow = true;
                                headerBand.Components.Add(hText);

                                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                                dataText.Text.Value = "{Revenues.Column" + column.Index + "}";

                                if (column.Type != "Variance")
                                {
                                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }

                                if (column.Type == "Actual")
                                {
                                    dataText.Interaction.Hyperlink = new StiHyperlinkExpression("{Revenues.Column" + column.Index + "URL}");
                                }

                                dataText.HorAlignment = StiTextHorAlignment.Right;
                                dataText.VertAlignment = StiVertAlignment.Top;
                                dataText.Border.Style = StiPenStyle.None;
                                dataText.OnlyText = false;
                                dataText.Border.Side = StiBorderSides.All;
                                dataText.Font = new Font("Arial", 8F);
                                dataText.WordWrap = true;
                                dataText.CanGrow = true;
                                dataText.Margins = new StiMargins(0, 1, 4, 0);
                                dataBand.Components.Add(dataText);

                                pos = pos + columnWidth;
                            }

                            //Create DataBand total
                            StiDataBand totalBand = new StiDataBand();
                            totalBand.DataSourceName = "RevenuesTotal";
                            totalBand.Name = $"RevenuesTotal{center.Item1}";
                            totalBand.Border.Style = StiPenStyle.None;
                            totalBand.Filters.Add(new StiFilter());
                            totalBand.Filters[0].Item = StiFilterItem.Expression;
                            totalBand.Filters[0].Expression = new StiExpression($"RevenuesTotal.Department == {center.Item1}");
                            page.Components.Add(totalBand);

                            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                            acctTotalText.Text.Value = "Total Revenues";
                            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                            acctTotalText.VertAlignment = StiVertAlignment.Center;
                            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctTotalText.OnlyText = false;
                            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctTotalText.WordWrap = true;
                            acctTotalText.Margins = new StiMargins(0, 1, 0, 10);
                            totalBand.Components.Add(acctTotalText);

                            pos = (columnWidth * 2);
                            foreach (var column in objColumns)
                            {
                                StiText footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                                footerText.Text.Value = "{RevenuesTotal.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    footerText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    footerText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                footerText.HorAlignment = StiTextHorAlignment.Right;
                                footerText.VertAlignment = StiVertAlignment.Center;
                                footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                footerText.OnlyText = false;
                                footerText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                footerText.WordWrap = true;
                                footerText.Margins = new StiMargins(0, 1, 0, 10);
                                totalBand.Components.Add(footerText);

                                pos = pos + columnWidth;
                            }
                        }

                        // Cost Of Sales
                        if (objColumns.Count > 0)
                        {
                            //Create HeaderBand
                            StiHeaderBand headerBand = new StiHeaderBand();
                            headerBand.Height = 0.25;
                            headerBand.Name = $"CostOfSalesHeaderBand{center.Item1}";
                            headerBand.Border.Style = StiPenStyle.None;
                            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            headerBand.PrintOnAllPages = false;
                            headerBand.PrintIfEmpty = true;
                            page.Components.Add(headerBand);

                            //Create Databand
                            StiDataBand dataBand = new StiDataBand();
                            dataBand.DataSourceName = "CostOfSales";
                            dataBand.Name = $"CostOfSalesData{center.Item1}";
                            dataBand.Border.Style = StiPenStyle.None;
                            dataBand.Filters.Add(new StiFilter());
                            dataBand.Filters[0].Item = StiFilterItem.Expression;
                            dataBand.Filters[0].Expression = new StiExpression($"CostOfSales.Department == {center.Item1}");
                            page.Components.Add(dataBand);

                            //Create DataBand item
                            StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctText.Text.Value = "Cost Of Sales";
                            acctText.HorAlignment = StiTextHorAlignment.Left;
                            acctText.VertAlignment = StiVertAlignment.Center;
                            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            acctText.Border.Side = StiBorderSides.All;
                            acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctText.Border.Style = StiPenStyle.None;
                            acctText.TextBrush = new StiSolidBrush(Color.White);
                            acctText.WordWrap = true;
                            headerBand.Components.Add(acctText);

                            StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.2));
                            acctDataText.Text.Value = "{CostOfSales.fDesc}";
                            acctDataText.HorAlignment = StiTextHorAlignment.Left;
                            acctDataText.VertAlignment = StiVertAlignment.Center;
                            acctDataText.Border.Style = StiPenStyle.None;
                            acctDataText.OnlyText = false;
                            acctDataText.Border.Side = StiBorderSides.All;
                            acctDataText.Font = new Font("Arial", 8F);
                            acctDataText.WordWrap = true;
                            acctDataText.CanGrow = true;
                            acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(acctDataText);

                            pos = (columnWidth * 2);

                            foreach (var column in objColumns)
                            {
                                //Create text on header
                                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                                hText.Text.Value = column.Label;
                                hText.HorAlignment = StiTextHorAlignment.Right;
                                hText.VertAlignment = StiVertAlignment.Center;
                                hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                                hText.Border.Side = StiBorderSides.All;
                                hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                hText.Border.Style = StiPenStyle.None;
                                hText.TextBrush = new StiSolidBrush(Color.White);
                                hText.WordWrap = true;
                                hText.CanGrow = true;
                                headerBand.Components.Add(hText);

                                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                                dataText.Text.Value = "{CostOfSales.Column" + column.Index + "}";

                                if (column.Type != "Variance")
                                {
                                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }

                                if (column.Type == "Actual")
                                {
                                    dataText.Interaction.Hyperlink = new StiHyperlinkExpression("{CostOfSales.Column" + column.Index + "URL}");
                                }

                                dataText.HorAlignment = StiTextHorAlignment.Right;
                                dataText.VertAlignment = StiVertAlignment.Top;
                                dataText.Border.Style = StiPenStyle.None;
                                dataText.OnlyText = false;
                                dataText.Border.Side = StiBorderSides.All;
                                dataText.Font = new Font("Arial", 8F);
                                dataText.WordWrap = true;
                                dataText.CanGrow = true;
                                dataText.Margins = new StiMargins(0, 1, 4, 0);
                                dataBand.Components.Add(dataText);

                                pos = pos + columnWidth;
                            }

                            //Create DataBand total
                            StiDataBand totalBand = new StiDataBand();
                            totalBand.DataSourceName = "CostOfSalesTotal";
                            totalBand.Name = $"CostOfSalesTotal{center.Item1}";
                            totalBand.Border.Style = StiPenStyle.None;
                            totalBand.Filters.Add(new StiFilter());
                            totalBand.Filters[0].Item = StiFilterItem.Expression;
                            totalBand.Filters[0].Expression = new StiExpression($"CostOfSalesTotal.Department == {center.Item1}");
                            page.Components.Add(totalBand);

                            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctTotalText.Text.Value = "Total Cost Of Sales";
                            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                            acctTotalText.VertAlignment = StiVertAlignment.Center;
                            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctTotalText.OnlyText = false;
                            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctTotalText.WordWrap = true;
                            acctTotalText.Margins = new StiMargins(0, 1, 0, 0);
                            totalBand.Components.Add(acctTotalText);

                            //Create DataBand Gross Profit
                            StiDataBand grossBand = new StiDataBand();
                            grossBand.DataSourceName = "GrossProfit";
                            grossBand.Name = $"GrossProfitBand{center.Item1}";
                            grossBand.Border.Style = StiPenStyle.None;
                            grossBand.Filters.Add(new StiFilter());
                            grossBand.Filters[0].Item = StiFilterItem.Expression;
                            grossBand.Filters[0].Expression = new StiExpression($"GrossProfit.Department == {center.Item1}");
                            page.Components.Add(grossBand);

                            StiText acctGrossText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                            acctGrossText.Text.Value = "Gross Profit";
                            acctGrossText.HorAlignment = StiTextHorAlignment.Left;
                            acctGrossText.VertAlignment = StiVertAlignment.Center;
                            acctGrossText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctGrossText.OnlyText = false;
                            acctGrossText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctGrossText.WordWrap = true;
                            acctGrossText.Margins = new StiMargins(0, 1, 0, 10);
                            grossBand.Components.Add(acctGrossText);

                            // Gross Profit Percen
                            StiDataBand percenBand = new StiDataBand();
                            percenBand.DataSourceName = "GrossProfitPercen";
                            percenBand.Name = $"GrossProfitPercenBand{center.Item1}";
                            percenBand.Border.Style = StiPenStyle.None;
                            percenBand.Filters.Add(new StiFilter());
                            percenBand.Filters[0].Item = StiFilterItem.Expression;
                            percenBand.Filters[0].Expression = new StiExpression($"GrossProfitPercen.Department == {center.Item1}");
                            page.Components.Add(percenBand);

                            StiText acctPercenText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctPercenText.Text.Value = "Gross Profit %";
                            acctPercenText.HorAlignment = StiTextHorAlignment.Left;
                            acctPercenText.VertAlignment = StiVertAlignment.Center;
                            acctPercenText.OnlyText = false;
                            acctPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctPercenText.WordWrap = true;
                            acctPercenText.Margins = new StiMargins(0, 1, 0, 10);
                            percenBand.Components.Add(acctPercenText);

                            // Item detail
                            pos = (columnWidth * 2);
                            foreach (var column in objColumns)
                            {
                                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                totalText.Text.Value = "{CostOfSalesTotal.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                totalText.HorAlignment = StiTextHorAlignment.Right;
                                totalText.VertAlignment = StiVertAlignment.Center;
                                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                totalText.OnlyText = false;
                                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                totalText.WordWrap = true;
                                totalText.Margins = new StiMargins(0, 1, 0, 0);
                                totalBand.Components.Add(totalText);

                                StiText grossText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                                grossText.Text.Value = "{GrossProfit.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    grossText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    grossText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                grossText.Type = StiSystemTextType.Expression;
                                grossText.HorAlignment = StiTextHorAlignment.Right;
                                grossText.VertAlignment = StiVertAlignment.Center;
                                grossText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                grossText.OnlyText = false;
                                grossText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                grossText.WordWrap = true;
                                grossText.Margins = new StiMargins(0, 1, 0, 10);
                                grossBand.Components.Add(grossText);

                                StiText depPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                if (column.Type != "Variance")
                                {
                                    depPercenText.Text.Value = "{GrossProfitPercen.Column" + column.Index + "}";
                                }
                                depPercenText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " "); ;
                                depPercenText.HorAlignment = StiTextHorAlignment.Right;
                                depPercenText.VertAlignment = StiVertAlignment.Center;
                                depPercenText.OnlyText = false;
                                depPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                depPercenText.WordWrap = true;
                                depPercenText.Margins = new StiMargins(0, 1, 0, 10);
                                percenBand.Components.Add(depPercenText);

                                pos = pos + columnWidth;
                            }
                        }

                        // Expenses
                        if (objColumns.Count > 0)
                        {
                            //Create HeaderBand
                            StiHeaderBand headerBand = new StiHeaderBand();
                            headerBand.Height = 0.25;
                            headerBand.Name = $"ExpensesHeaderBand{center.Item1}";
                            headerBand.Border.Style = StiPenStyle.None;
                            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            headerBand.PrintOnAllPages = false;
                            headerBand.PrintIfEmpty = true;
                            page.Components.Add(headerBand);

                            //Create Databand
                            StiDataBand dataBand = new StiDataBand();
                            dataBand.DataSourceName = "Expenses";
                            dataBand.Name = $"ExpensesData{center.Item1}";
                            dataBand.Border.Style = StiPenStyle.None;
                            dataBand.Filters.Add(new StiFilter());
                            dataBand.Filters[0].Item = StiFilterItem.Expression;
                            dataBand.Filters[0].Expression = new StiExpression($"Expenses.Department == {center.Item1}");
                            page.Components.Add(dataBand);

                            //Create DataBand item
                            StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctText.Text.Value = "Expenses";
                            acctText.HorAlignment = StiTextHorAlignment.Left;
                            acctText.VertAlignment = StiVertAlignment.Center;
                            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            acctText.Border.Side = StiBorderSides.All;
                            acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctText.Border.Style = StiPenStyle.None;
                            acctText.TextBrush = new StiSolidBrush(Color.White);
                            acctText.WordWrap = true;
                            headerBand.Components.Add(acctText);

                            StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.2));
                            acctDataText.Text.Value = "{Expenses.fDesc}";
                            acctDataText.HorAlignment = StiTextHorAlignment.Left;
                            acctDataText.VertAlignment = StiVertAlignment.Center;
                            acctDataText.Border.Style = StiPenStyle.None;
                            acctDataText.OnlyText = false;
                            acctDataText.Border.Side = StiBorderSides.All;
                            acctDataText.Font = new Font("Arial", 8F);
                            acctDataText.WordWrap = true;
                            acctDataText.CanGrow = true;
                            acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(acctDataText);

                            pos = (columnWidth * 2);

                            foreach (var column in objColumns)
                            {
                                //Create text on header
                                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                                hText.Text.Value = column.Label;
                                hText.HorAlignment = StiTextHorAlignment.Right;
                                hText.VertAlignment = StiVertAlignment.Center;
                                hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                                hText.Border.Side = StiBorderSides.All;
                                hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                hText.Border.Style = StiPenStyle.None;
                                hText.TextBrush = new StiSolidBrush(Color.White);
                                hText.WordWrap = true;
                                hText.CanGrow = true;
                                headerBand.Components.Add(hText);

                                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                                dataText.Text.Value = "{Expenses.Column" + column.Index + "}";

                                if (column.Type != "Variance")
                                {
                                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }

                                if (column.Type == "Actual")
                                {
                                    dataText.Interaction.Hyperlink = new StiHyperlinkExpression("{Expenses.Column" + column.Index + "URL}");
                                }

                                dataText.HorAlignment = StiTextHorAlignment.Right;
                                dataText.VertAlignment = StiVertAlignment.Top;
                                dataText.Border.Style = StiPenStyle.None;
                                dataText.OnlyText = false;
                                dataText.Border.Side = StiBorderSides.All;
                                dataText.Font = new Font("Arial", 8F);
                                dataText.WordWrap = true;
                                dataText.CanGrow = true;
                                dataText.Margins = new StiMargins(0, 1, 4, 0);
                                dataBand.Components.Add(dataText);

                                pos = pos + columnWidth;
                            }

                            //Create DataBand total
                            StiDataBand totalBand = new StiDataBand();
                            totalBand.DataSourceName = "ExpensesTotal";
                            totalBand.Name = $"ExpensesTotal{center.Item1}";
                            totalBand.Border.Style = StiPenStyle.None;
                            totalBand.Filters.Add(new StiFilter());
                            totalBand.Filters[0].Item = StiFilterItem.Expression;
                            totalBand.Filters[0].Expression = new StiExpression($"ExpensesTotal.Department == {center.Item1}");
                            page.Components.Add(totalBand);

                            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctTotalText.Text.Value = "Total Expenses";
                            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                            acctTotalText.VertAlignment = StiVertAlignment.Center;
                            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctTotalText.OnlyText = false;
                            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctTotalText.WordWrap = true;
                            acctTotalText.Margins = new StiMargins(0, 1, 0, 0);
                            totalBand.Components.Add(acctTotalText);

                            //Create DataBand Net Profit
                            StiDataBand netBand = new StiDataBand();
                            netBand.DataSourceName = "NetProfit";
                            netBand.Name = $"NetProfit{center.Item1}";
                            netBand.Border.Style = StiPenStyle.None;
                            netBand.Filters.Add(new StiFilter());
                            netBand.Filters[0].Item = StiFilterItem.Expression;
                            netBand.Filters[0].Expression = new StiExpression($"NetProfit.Department == {center.Item1}");
                            page.Components.Add(netBand);

                            StiText acctNetText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                            acctNetText.Text.Value = netProfitText;
                            acctNetText.HorAlignment = StiTextHorAlignment.Left;
                            acctNetText.VertAlignment = StiVertAlignment.Center;
                            acctNetText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctNetText.OnlyText = false;
                            acctNetText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctNetText.WordWrap = true;
                            acctNetText.Margins = new StiMargins(0, 1, 0, 10);
                            netBand.Components.Add(acctNetText);

                            // Net Profit Percen
                            StiDataBand percenBand = new StiDataBand();
                            percenBand.DataSourceName = "NetProfitPercen";
                            percenBand.Name = $"NetProfitPercenBand{center.Item1}";
                            percenBand.Border.Style = StiPenStyle.None;
                            percenBand.NewPageAfter = !_includeProvisions;
                            percenBand.Filters.Add(new StiFilter());
                            percenBand.Filters[0].Item = StiFilterItem.Expression;
                            percenBand.Filters[0].Expression = new StiExpression($"NetProfitPercen.Department == {center.Item1}");
                            page.Components.Add(percenBand);

                            StiText acctPercenText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctPercenText.Text.Value = $"{netProfitText} %";
                            acctPercenText.HorAlignment = StiTextHorAlignment.Left;
                            acctPercenText.VertAlignment = StiVertAlignment.Center;
                            acctPercenText.OnlyText = false;
                            acctPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctPercenText.WordWrap = true;
                            acctPercenText.Margins = new StiMargins(0, 1, 0, 10);
                            percenBand.Components.Add(acctPercenText);

                            pos = (columnWidth * 2);
                            foreach (var column in objColumns)
                            {
                                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                totalText.Text.Value = "{ExpensesTotal.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                totalText.HorAlignment = StiTextHorAlignment.Right;
                                totalText.VertAlignment = StiVertAlignment.Center;
                                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                totalText.OnlyText = false;
                                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                totalText.WordWrap = true;
                                totalText.Margins = new StiMargins(0, 1, 0, 0);
                                totalBand.Components.Add(totalText);

                                StiText netText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                                netText.Text.Value = "{NetProfit.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    netText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    netText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                netText.Type = StiSystemTextType.Expression;
                                netText.HorAlignment = StiTextHorAlignment.Right;
                                netText.VertAlignment = StiVertAlignment.Center;
                                netText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                netText.OnlyText = false;
                                netText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                netText.WordWrap = true;
                                netText.Margins = new StiMargins(0, 1, 0, 10);
                                netBand.Components.Add(netText);

                                StiText depPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                if (column.Type != "Variance")
                                {
                                    depPercenText.Text.Value = "{NetProfitPercen.Column" + column.Index + "}";
                                }
                                depPercenText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " "); ;
                                depPercenText.HorAlignment = StiTextHorAlignment.Right;
                                depPercenText.VertAlignment = StiVertAlignment.Center;
                                depPercenText.OnlyText = false;
                                depPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                depPercenText.WordWrap = true;
                                depPercenText.Margins = new StiMargins(0, 1, 0, 10);
                                percenBand.Components.Add(depPercenText);

                                pos = pos + columnWidth;
                            }
                        }

                        // Other Income (Expenses)
                        if (objColumns.Count > 0)
                        {
                            //Create HeaderBand
                            StiHeaderBand headerBand = new StiHeaderBand();
                            headerBand.Height = 0.25;
                            headerBand.Name = $"OtherIncomeHeaderBand{center.Item1}";
                            headerBand.Border.Style = StiPenStyle.None;
                            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            headerBand.PrintOnAllPages = false;
                            headerBand.PrintIfEmpty = false;
                            page.Components.Add(headerBand);

                            //Create Databand
                            StiDataBand dataBand = new StiDataBand();
                            dataBand.DataSourceName = "OtherIncome";
                            dataBand.Name = $"OtherIncomeData{center.Item1}";
                            dataBand.Border.Style = StiPenStyle.None;
                            dataBand.Filters.Add(new StiFilter());
                            dataBand.Filters[0].Item = StiFilterItem.Expression;
                            dataBand.Filters[0].Expression = new StiExpression($"OtherIncome.Department == {center.Item1}");
                            page.Components.Add(dataBand);

                            //Create DataBand item
                            StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctText.Text.Value = "Other Income (Expenses)";
                            acctText.HorAlignment = StiTextHorAlignment.Left;
                            acctText.VertAlignment = StiVertAlignment.Center;
                            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            acctText.Border.Side = StiBorderSides.All;
                            acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctText.Border.Style = StiPenStyle.None;
                            acctText.TextBrush = new StiSolidBrush(Color.White);
                            acctText.WordWrap = true;
                            headerBand.Components.Add(acctText);

                            StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.2));
                            acctDataText.Text.Value = "{OtherIncome.fDesc}";
                            acctDataText.HorAlignment = StiTextHorAlignment.Left;
                            acctDataText.VertAlignment = StiVertAlignment.Center;
                            acctDataText.Border.Style = StiPenStyle.None;
                            acctDataText.OnlyText = false;
                            acctDataText.Border.Side = StiBorderSides.All;
                            acctDataText.Font = new Font("Arial", 8F);
                            acctDataText.WordWrap = true;
                            acctDataText.CanGrow = true;
                            acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(acctDataText);

                            pos = (columnWidth * 2);

                            foreach (var column in objColumns)
                            {
                                //Create text on header
                                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                                hText.Text.Value = column.Label;
                                hText.HorAlignment = StiTextHorAlignment.Right;
                                hText.VertAlignment = StiVertAlignment.Center;
                                hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                                hText.Border.Side = StiBorderSides.All;
                                hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                hText.Border.Style = StiPenStyle.None;
                                hText.TextBrush = new StiSolidBrush(Color.White);
                                hText.WordWrap = true;
                                hText.CanGrow = true;
                                headerBand.Components.Add(hText);

                                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                                dataText.Text.Value = "{OtherIncome.Column" + column.Index + "}";

                                if (column.Type != "Variance")
                                {
                                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }

                                if (column.Type == "Actual")
                                {
                                    dataText.Interaction.Hyperlink = new StiHyperlinkExpression("{OtherIncome.Column" + column.Index + "URL}");
                                }

                                dataText.HorAlignment = StiTextHorAlignment.Right;
                                dataText.VertAlignment = StiVertAlignment.Top;
                                dataText.Border.Style = StiPenStyle.None;
                                dataText.OnlyText = false;
                                dataText.Border.Side = StiBorderSides.All;
                                dataText.Font = new Font("Arial", 8F);
                                dataText.WordWrap = true;
                                dataText.CanGrow = true;
                                dataText.Margins = new StiMargins(0, 1, 4, 0);
                                dataBand.Components.Add(dataText);

                                pos = pos + columnWidth;
                            }

                            //Create DataBand total
                            StiDataBand totalBand = new StiDataBand();
                            totalBand.DataSourceName = "OtherIncomeTotal";
                            totalBand.Name = $"OtherIncomeTotal{center.Item1}";
                            totalBand.Border.Style = StiPenStyle.None;
                            totalBand.Filters.Add(new StiFilter());
                            totalBand.Filters[0].Item = StiFilterItem.Expression;
                            totalBand.Filters[0].Expression = new StiExpression($"OtherIncomeTotal.Department == {center.Item1}");
                            page.Components.Add(totalBand);

                            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctTotalText.Text.Value = "Total Other Income (Expenses)";
                            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                            acctTotalText.VertAlignment = StiVertAlignment.Center;
                            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctTotalText.OnlyText = false;
                            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctTotalText.WordWrap = true;
                            acctTotalText.Margins = new StiMargins(0, 1, 0, 0);
                            totalBand.Components.Add(acctTotalText);

                            //Create DataBand Income Before Provisions For Income Taxes
                            StiDataBand grossBand = new StiDataBand();
                            grossBand.DataSourceName = "BeforeProvisions";
                            grossBand.Name = $"BeforeProvisionsBand{center.Item1}";
                            grossBand.Border.Style = StiPenStyle.None;
                            grossBand.Filters.Add(new StiFilter());
                            grossBand.Filters[0].Item = StiFilterItem.Expression;
                            grossBand.Filters[0].Expression = new StiExpression($"BeforeProvisions.Department == {center.Item1}");
                            page.Components.Add(grossBand);

                            StiText acctGrossText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                            acctGrossText.Text.Value = "Income Before Provisions For Income Taxes";
                            acctGrossText.HorAlignment = StiTextHorAlignment.Left;
                            acctGrossText.VertAlignment = StiVertAlignment.Center;
                            acctGrossText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctGrossText.OnlyText = false;
                            acctGrossText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctGrossText.WordWrap = true;
                            acctGrossText.Margins = new StiMargins(0, 1, 0, 10);
                            grossBand.Components.Add(acctGrossText);

                            // Income Before Provisions For Income Taxes Percen
                            StiDataBand percenBand = new StiDataBand();
                            percenBand.DataSourceName = "BeforeProvisionsPercen";
                            percenBand.Name = $"BeforeProvisionsPercenBand{center.Item1}";
                            percenBand.Border.Style = StiPenStyle.None;
                            percenBand.Filters.Add(new StiFilter());
                            percenBand.Filters[0].Item = StiFilterItem.Expression;
                            percenBand.Filters[0].Expression = new StiExpression($"BeforeProvisionsPercen.Department == {center.Item1}");
                            page.Components.Add(percenBand);

                            StiText acctPercenText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctPercenText.Text.Value = "Income Before Provisions For Income Taxes %";
                            acctPercenText.HorAlignment = StiTextHorAlignment.Left;
                            acctPercenText.VertAlignment = StiVertAlignment.Center;
                            acctPercenText.OnlyText = false;
                            acctPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctPercenText.WordWrap = true;
                            acctPercenText.Margins = new StiMargins(0, 1, 0, 10);
                            percenBand.Components.Add(acctPercenText);

                            // Item detail
                            pos = (columnWidth * 2);
                            foreach (var column in objColumns)
                            {
                                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                totalText.Text.Value = "{OtherIncomeTotal.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                totalText.HorAlignment = StiTextHorAlignment.Right;
                                totalText.VertAlignment = StiVertAlignment.Center;
                                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                totalText.OnlyText = false;
                                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                totalText.WordWrap = true;
                                totalText.Margins = new StiMargins(0, 1, 0, 0);
                                totalBand.Components.Add(totalText);

                                StiText grossText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                                grossText.Text.Value = "{BeforeProvisions.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    grossText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    grossText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                grossText.Type = StiSystemTextType.Expression;
                                grossText.HorAlignment = StiTextHorAlignment.Right;
                                grossText.VertAlignment = StiVertAlignment.Center;
                                grossText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                grossText.OnlyText = false;
                                grossText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                grossText.WordWrap = true;
                                grossText.Margins = new StiMargins(0, 1, 0, 10);
                                grossBand.Components.Add(grossText);

                                StiText depPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                if (column.Type != "Variance")
                                {
                                    depPercenText.Text.Value = "{BeforeProvisionsPercen.Column" + column.Index + "}";
                                }
                                depPercenText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " "); ;
                                depPercenText.HorAlignment = StiTextHorAlignment.Right;
                                depPercenText.VertAlignment = StiVertAlignment.Center;
                                depPercenText.OnlyText = false;
                                depPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                depPercenText.WordWrap = true;
                                depPercenText.Margins = new StiMargins(0, 1, 0, 10);
                                percenBand.Components.Add(depPercenText);

                                pos = pos + columnWidth;
                            }
                        }

                        // Provisions for Income Taxes
                        if (objColumns.Count > 0)
                        {
                            //Create HeaderBand
                            StiHeaderBand headerBand = new StiHeaderBand();
                            headerBand.Height = 0.25;
                            headerBand.Name = $"IncomeTaxesHeaderBand{center.Item1}";
                            headerBand.Border.Style = StiPenStyle.None;
                            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            headerBand.PrintOnAllPages = false;
                            headerBand.PrintIfEmpty = false;
                            page.Components.Add(headerBand);

                            //Create Databand
                            StiDataBand dataBand = new StiDataBand();
                            dataBand.DataSourceName = "IncomeTaxes";
                            dataBand.Name = $"IncomeTaxesData{center.Item1}";
                            dataBand.Border.Style = StiPenStyle.None;
                            dataBand.Filters.Add(new StiFilter());
                            dataBand.Filters[0].Item = StiFilterItem.Expression;
                            dataBand.Filters[0].Expression = new StiExpression($"IncomeTaxes.Department == {center.Item1}");
                            page.Components.Add(dataBand);

                            //Create DataBand item
                            StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctText.Text.Value = "Provisions for Income Taxes";
                            acctText.HorAlignment = StiTextHorAlignment.Left;
                            acctText.VertAlignment = StiVertAlignment.Center;
                            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            acctText.Border.Side = StiBorderSides.All;
                            acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctText.Border.Style = StiPenStyle.None;
                            acctText.TextBrush = new StiSolidBrush(Color.White);
                            acctText.WordWrap = true;
                            headerBand.Components.Add(acctText);

                            StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.2));
                            acctDataText.Text.Value = "{IncomeTaxes.fDesc}";
                            acctDataText.HorAlignment = StiTextHorAlignment.Left;
                            acctDataText.VertAlignment = StiVertAlignment.Center;
                            acctDataText.Border.Style = StiPenStyle.None;
                            acctDataText.OnlyText = false;
                            acctDataText.Border.Side = StiBorderSides.All;
                            acctDataText.Font = new Font("Arial", 8F);
                            acctDataText.WordWrap = true;
                            acctDataText.CanGrow = true;
                            acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(acctDataText);

                            pos = (columnWidth * 2);

                            foreach (var column in objColumns)
                            {
                                //Create text on header
                                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                                hText.Text.Value = column.Label;
                                hText.HorAlignment = StiTextHorAlignment.Right;
                                hText.VertAlignment = StiVertAlignment.Center;
                                hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                                hText.Border.Side = StiBorderSides.All;
                                hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                hText.Border.Style = StiPenStyle.None;
                                hText.TextBrush = new StiSolidBrush(Color.White);
                                hText.WordWrap = true;
                                hText.CanGrow = true;
                                headerBand.Components.Add(hText);

                                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                                dataText.Text.Value = "{IncomeTaxes.Column" + column.Index + "}";

                                if (column.Type != "Variance")
                                {
                                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }

                                if (column.Type == "Actual")
                                {
                                    dataText.Interaction.Hyperlink = new StiHyperlinkExpression("{IncomeTaxes.Column" + column.Index + "URL}");
                                }

                                dataText.HorAlignment = StiTextHorAlignment.Right;
                                dataText.VertAlignment = StiVertAlignment.Top;
                                dataText.Border.Style = StiPenStyle.None;
                                dataText.OnlyText = false;
                                dataText.Border.Side = StiBorderSides.All;
                                dataText.Font = new Font("Arial", 8F);
                                dataText.WordWrap = true;
                                dataText.CanGrow = true;
                                dataText.Margins = new StiMargins(0, 1, 4, 0);
                                dataBand.Components.Add(dataText);

                                pos = pos + columnWidth;
                            }

                            //Create DataBand total
                            StiDataBand totalBand = new StiDataBand();
                            totalBand.DataSourceName = "IncomeTaxesTotal";
                            totalBand.Name = $"IncomeTaxesTotal{center.Item1}";
                            totalBand.Border.Style = StiPenStyle.None;
                            totalBand.Filters.Add(new StiFilter());
                            totalBand.Filters[0].Item = StiFilterItem.Expression;
                            totalBand.Filters[0].Expression = new StiExpression($"IncomeTaxesTotal.Department == {center.Item1}");
                            page.Components.Add(totalBand);

                            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctTotalText.Text.Value = "Total Provisions for Income Taxes";
                            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                            acctTotalText.VertAlignment = StiVertAlignment.Center;
                            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctTotalText.OnlyText = false;
                            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctTotalText.WordWrap = true;
                            acctTotalText.Margins = new StiMargins(0, 1, 0, 0);
                            totalBand.Components.Add(acctTotalText);

                            //Create DataBand Net Income
                            StiDataBand netBand = new StiDataBand();
                            netBand.DataSourceName = "NetIncome";
                            netBand.Name = $"NetIncome{center.Item1}";
                            netBand.Border.Style = StiPenStyle.None;
                            netBand.Filters.Add(new StiFilter());
                            netBand.Filters[0].Item = StiFilterItem.Expression;
                            netBand.Filters[0].Expression = new StiExpression($"NetIncome.Department == {center.Item1}");
                            page.Components.Add(netBand);

                            StiText acctNetText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                            acctNetText.Text.Value = "Net Income";
                            acctNetText.HorAlignment = StiTextHorAlignment.Left;
                            acctNetText.VertAlignment = StiVertAlignment.Center;
                            acctNetText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctNetText.OnlyText = false;
                            acctNetText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctNetText.WordWrap = true;
                            acctNetText.Margins = new StiMargins(0, 1, 0, 10);
                            netBand.Components.Add(acctNetText);

                            // Net Income Percen
                            StiDataBand percenBand = new StiDataBand();
                            percenBand.DataSourceName = "NetIncomePercen";
                            percenBand.Name = $"NetIncomePercenBand{center.Item1}";
                            percenBand.Border.Style = StiPenStyle.None;
                            percenBand.NewPageAfter = true;
                            percenBand.Filters.Add(new StiFilter());
                            percenBand.Filters[0].Item = StiFilterItem.Expression;
                            percenBand.Filters[0].Expression = new StiExpression($"NetIncomePercen.Department == {center.Item1}");
                            page.Components.Add(percenBand);

                            StiText acctPercenText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctPercenText.Text.Value = "Net Income %";
                            acctPercenText.HorAlignment = StiTextHorAlignment.Left;
                            acctPercenText.VertAlignment = StiVertAlignment.Center;
                            acctPercenText.OnlyText = false;
                            acctPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctPercenText.WordWrap = true;
                            acctPercenText.Margins = new StiMargins(0, 1, 0, 10);
                            percenBand.Components.Add(acctPercenText);

                            pos = (columnWidth * 2);
                            foreach (var column in objColumns)
                            {
                                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                totalText.Text.Value = "{IncomeTaxesTotal.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                totalText.HorAlignment = StiTextHorAlignment.Right;
                                totalText.VertAlignment = StiVertAlignment.Center;
                                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                totalText.OnlyText = false;
                                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                totalText.WordWrap = true;
                                totalText.Margins = new StiMargins(0, 1, 0, 0);
                                totalBand.Components.Add(totalText);

                                StiText netText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                                netText.Text.Value = "{NetIncome.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    netText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    netText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                netText.Type = StiSystemTextType.Expression;
                                netText.HorAlignment = StiTextHorAlignment.Right;
                                netText.VertAlignment = StiVertAlignment.Center;
                                netText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                netText.OnlyText = false;
                                netText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                netText.WordWrap = true;
                                netText.Margins = new StiMargins(0, 1, 0, 10);
                                netBand.Components.Add(netText);

                                StiText depPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                if (column.Type != "Variance")
                                {
                                    depPercenText.Text.Value = "{NetIncomePercen.Column" + column.Index + "}";
                                }
                                depPercenText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " "); ;
                                depPercenText.HorAlignment = StiTextHorAlignment.Right;
                                depPercenText.VertAlignment = StiVertAlignment.Center;
                                depPercenText.OnlyText = false;
                                depPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                depPercenText.WordWrap = true;
                                depPercenText.Margins = new StiMargins(0, 1, 0, 10);
                                percenBand.Components.Add(depPercenText);

                                pos = pos + columnWidth;
                            }
                        }
                    }
                }
            }

            // Build Total Summary
            BuildAllDepartmentTotal(page, objComparative, false);

            // Build Department Summary
            if (departments.Count() > 0)
            {
                DepartmentSummaryDataSource(report, objComparative, departments, data);
            }

            if (deptArray.Length > 1)
            {
                BuildDepartmentSummary(page, objComparative, departments);
            }

            DataSet dsC = new DataSet();

            var connString = Session["config"].ToString();
            objPropUser.ConnConfig = connString;
            dsC = objBL_User.getControl(objPropUser);

            DataTable cTable = BuildCompanyDetailsTable();
            var cRow = cTable.NewRow();

            DataSet companyInfo = new DataSet();
            companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());

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

            report.RegData("CompanyDetails", cTable);
            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();
            report.Dictionary.Variables["ReportName"].Value = Session["ReportName"].ToString();

            dView.RowFilter = "Type = 3";
            DataTable Revenues = dView.ToTable();

            dView.RowFilter = "Type = 4";
            DataTable CostOfSales = dView.ToTable();

            dView.RowFilter = "Type = 5";
            DataTable Expenses = dView.ToTable();

            dView.RowFilter = "Type = 8";
            DataTable OtherIncome = dView.ToTable();

            dView.RowFilter = "Type = 9";
            DataTable IncomeTaxes = dView.ToTable();

            report.RegData("Revenues", Revenues);
            report.RegData("CostOfSales", CostOfSales);
            report.RegData("Expenses", Expenses);
            report.RegData("OtherIncome", OtherIncome);
            report.RegData("IncomeTaxes", IncomeTaxes);
            report.RegData("RevenuesTotal", _revenuesTotal);
            report.RegData("CostOfSalesTotal", _costOfSalesTotal);
            report.RegData("ExpensesTotal", _expensesTotal);
            report.RegData("OtherIncomeTotal", _otherIncomeTotal);
            report.RegData("IncomeTaxesTotal", _incomeTaxesTotal);
            report.RegData("GrossProfit", _grossProfit);
            report.RegData("GrossProfitPercen", _grossProfitPercen);
            report.RegData("NetProfit", _netProfit);
            report.RegData("NetProfitPercen", _netProfitPercen);
            report.RegData("BeforeProvisions", _beforeProvisions);
            report.RegData("BeforeProvisionsPercen", _beforeProvisionsPercen);
            report.RegData("NetIncome", _netIncome);
            report.RegData("NetIncomePercen", _netIncomePercen);
            report.RegData("SummaryTotal", SummaryTotal(data, objComparative));

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

    private StiReport GetComparativeProfitAndLossWithSubReport(DataTable comparativeReportData, List<ComparativeStatementRequest> objComparative)
    {
        try
        {
            string reportPathStimul = string.Empty;
            reportPathStimul = Server.MapPath("StimulsoftReports/ComparativeStatementReport.mrt");
            StiReport report = new StiReport();
            report.Load(reportPathStimul);

            // Selected department
            var departments = comparativeReportData.AsEnumerable()
                                .Select(g =>
                                {
                                    return Tuple.Create(g.Field<int>("Department"), g.Field<string>("CentralName"));
                                }).Distinct().OrderBy(x => x.Item2);

            var objColumns = objComparative;
            foreach (var column in objColumns)
            {
                report.Dictionary.DataSources["Revenues"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["CostOfSales"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["Expenses"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["OtherIncome"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["IncomeTaxes"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["RevenuesTotal"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["CostOfSalesTotal"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["ExpensesTotal"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["OtherIncomeTotal"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["IncomeTaxesTotal"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["GrossProfit"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["GrossProfitPercen"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["NetProfit"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["NetProfitPercen"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["BeforeProvisions"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["BeforeProvisionsPercen"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["NetIncome"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["NetIncomePercen"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["SummaryTotal"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));

                if (column.Type == "Actual")
                {
                    report.Dictionary.DataSources["Revenues"].Columns.Add(string.Format("Column{0}URL", column.Index), typeof(string));
                    report.Dictionary.DataSources["CostOfSales"].Columns.Add(string.Format("Column{0}URL", column.Index), typeof(string));
                    report.Dictionary.DataSources["Expenses"].Columns.Add(string.Format("Column{0}URL", column.Index), typeof(string));
                    report.Dictionary.DataSources["OtherIncome"].Columns.Add(string.Format("Column{0}URL", column.Index), typeof(string));
                    report.Dictionary.DataSources["IncomeTaxes"].Columns.Add(string.Format("Column{0}URL", column.Index), typeof(string));
                }

                if (departments.Count() > 0)
                {
                    StiDataSource dtSourceRevenues = new StiDataTableSource($"DepSummaryRevenues{column.Index}", $"DepSummaryRevenues{column.Index}", $"DepSummaryRevenues{column.Index}");
                    BuildDepartmentSummaryDataSource(dtSourceRevenues, column, departments);
                    report.Dictionary.DataSources.Add(dtSourceRevenues);

                    StiDataSource dtSourceCostOfSales = new StiDataTableSource($"DepSummaryCostOfSales{column.Index}", $"DepSummaryCostOfSales{column.Index}", $"DepSummaryCostOfSales{column.Index}");
                    BuildDepartmentSummaryDataSource(dtSourceCostOfSales, column, departments);
                    report.Dictionary.DataSources.Add(dtSourceCostOfSales);

                    StiDataSource dtSourceExpenses = new StiDataTableSource($"DepSummaryExpenses{column.Index}", $"DepSummaryExpenses{column.Index}", $"DepSummaryExpenses{column.Index}");
                    BuildDepartmentSummaryDataSource(dtSourceExpenses, column, departments);
                    report.Dictionary.DataSources.Add(dtSourceExpenses);

                    StiDataSource dtSourceOtherIncome = new StiDataTableSource($"DepSummaryOtherIncome{column.Index}", $"DepSummaryOtherIncome{column.Index}", $"DepSummaryOtherIncome{column.Index}");
                    BuildDepartmentSummaryDataSource(dtSourceOtherIncome, column, departments);
                    report.Dictionary.DataSources.Add(dtSourceOtherIncome);

                    StiDataSource dtSourceIncomeTaxes = new StiDataTableSource($"DepSummaryIncomeTaxes{column.Index}", $"DepSummaryIncomeTaxes{column.Index}", $"DepSummaryIncomeTaxes{column.Index}");
                    BuildDepartmentSummaryDataSource(dtSourceIncomeTaxes, column, departments);
                    report.Dictionary.DataSources.Add(dtSourceIncomeTaxes);

                    StiDataSource dtConsolidating = new StiDataTableSource($"Consolidating{column.Index}", $"Consolidating{column.Index}", $"Consolidating{column.Index}");
                    BuildConsolidatingDataSource(dtConsolidating, column, departments);
                    report.Dictionary.DataSources.Add(dtConsolidating);
                }
            }

            StiPage page = report.Pages[0];
            page.CanGrow = true;
            page.CanShrink = true;

            var columnCount = objColumns.Count + 2;
            double columnWidth = page.Width / columnCount;
            double pos = 0;

            var data = comparativeReportData;
            DataTable filteredTable = data.Copy();
            DataView dView = filteredTable.DefaultView;

            _revenuesTotal = BuildTotalTable(objColumns);
            _costOfSalesTotal = BuildTotalTable(objColumns);
            _expensesTotal = BuildTotalTable(objColumns);
            _otherIncomeTotal = BuildTotalTable(objColumns);
            _incomeTaxesTotal = BuildTotalTable(objColumns);
            _grossProfit = BuildTotalTable(objColumns);
            _grossProfitPercen = BuildTotalTable(objColumns);
            _netProfit = BuildTotalTable(objColumns);
            _netProfitPercen = BuildTotalTable(objColumns);
            _beforeProvisions = BuildTotalTable(objColumns);
            _beforeProvisionsPercen = BuildTotalTable(objColumns);
            _netIncome = BuildTotalTable(objColumns);
            _netIncomePercen = BuildTotalTable(objColumns);

            // Build Consolidating Divisional Financial Statements
            //if (departments.Count() > 0)
            //{
            //    BuildConsolidatingDivisional(page, objComparative, departments);
            //}

            string[] deptArray = new string[] { };
            if (Session["ComparativeCenter"] != null)
            {
                var depts = Session["ComparativeCenter"].ToString();
                deptArray = depts.Split(',');
            }

            // Build for each Department
            if (deptArray.Length > 1)
            {
                var centers = (List<Tuple<int, string>>)Session["SelectedCenters"];
                foreach (var center in centers)
                {
                    dView.RowFilter = $"Department = {center.Item1}";
                    DataTable centerData = dView.ToTable();

                    if (centerData.Rows.Count > 0)
                    {
                        CenterSummaryTotal(centerData, objColumns, center.Item1);

                        var netProfitText = "Income From Operations";

                        //Add Center title
                        StiHeaderBand centerTitle = new StiHeaderBand();
                        centerTitle.Height = 0.5;
                        centerTitle.PrintIfEmpty = true;
                        page.Components.Add(centerTitle);

                        StiText centerTitleText = new StiText(new RectangleD(0, 0.1, page.Width, 0.25));
                        centerTitleText.Text.Value = $"{center.Item2} Division";
                        centerTitleText.HorAlignment = StiTextHorAlignment.Center;
                        centerTitleText.VertAlignment = StiVertAlignment.Center;
                        centerTitleText.Font = new Font("Arial", 18F, System.Drawing.FontStyle.Bold);
                        centerTitleText.WordWrap = true;
                        centerTitle.Components.Add(centerTitleText);

                        // Revenues
                        if (objColumns.Count > 0)
                        {
                            //Create Header
                            StiHeaderBand header = new StiHeaderBand();
                            header.Height = 0.25;
                            header.Name = $"RevenuesHeaderBand{center.Item1}";
                            header.Border.Style = StiPenStyle.None;
                            header.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            header.PrintOnAllPages = false;
                            header.PrintIfEmpty = true;
                            page.Components.Add(header);

                            StiText acctText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                            acctText.Text.Value = "Revenues";
                            acctText.HorAlignment = StiTextHorAlignment.Left;
                            acctText.VertAlignment = StiVertAlignment.Center;
                            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            acctText.Border.Side = StiBorderSides.All;
                            acctText.Font = new Font("Arial", 10F, FontStyle.Bold);
                            acctText.Border.Style = StiPenStyle.None;
                            acctText.TextBrush = new StiSolidBrush(Color.White);
                            acctText.WordWrap = true;
                            header.Components.Add(acctText);

                            //Create group header band
                            StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                            groupHeader.Name = $"RevenuesGroupHeaderBand{center.Item1}";
                            groupHeader.PrintOnAllPages = false;
                            groupHeader.Condition = new StiGroupConditionExpression("{Revenues.Sub}");
                            page.Components.Add(groupHeader);

                            StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                            groupHeaderText.Text.Value = "{Revenues.Sub}";
                            groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                            groupHeaderText.VertAlignment = StiVertAlignment.Center;
                            groupHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            groupHeaderText.Border.Style = StiPenStyle.None;
                            groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                            groupHeaderText.WordWrap = true;
                            groupHeader.Components.Add(groupHeaderText);

                            //Create HeaderBand
                            StiHeaderBand headerBand = new StiHeaderBand();
                            headerBand.Height = 0.25;
                            headerBand.Name = $"RevenuesBand{center.Item1}";
                            headerBand.Border.Style = StiPenStyle.None;
                            headerBand.PrintOnAllPages = false;
                            headerBand.PrintIfEmpty = true;
                            page.Components.Add(headerBand);

                            //Create Databand
                            StiDataBand dataBand = new StiDataBand();
                            dataBand.DataSourceName = "Revenues";
                            dataBand.Name = $"RevenuesData{center.Item1}";
                            dataBand.Border.Style = StiPenStyle.None;
                            dataBand.Filters.Add(new StiFilter());
                            dataBand.Filters[0].Item = StiFilterItem.Expression;
                            dataBand.Filters[0].Expression = new StiExpression($"Revenues.Department == {center.Item1}");
                            page.Components.Add(dataBand);

                            //Create DataBand detail item           
                            StiText acctDataText = new StiText(new RectangleD(0.1, 0, columnWidth * 2 - 0.1, 0.2));
                            acctDataText.Text.Value = "{Revenues.fDesc}";
                            acctDataText.HorAlignment = StiTextHorAlignment.Left;
                            acctDataText.VertAlignment = StiVertAlignment.Center;
                            acctDataText.Border.Style = StiPenStyle.None;
                            acctDataText.OnlyText = false;
                            acctDataText.Border.Side = StiBorderSides.All;
                            acctDataText.Font = new Font("Arial", 8F);
                            acctDataText.WordWrap = true;
                            acctDataText.CanGrow = true;
                            acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(acctDataText);

                            pos = (columnWidth * 2);

                            foreach (var column in objColumns)
                            {
                                //Create text on header
                                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                                hText.Text.Value = column.Label;
                                hText.HorAlignment = StiTextHorAlignment.Right;
                                hText.VertAlignment = StiVertAlignment.Center;
                                hText.Border.Side = StiBorderSides.All;
                                hText.Font = new Font("Arial", 9F, FontStyle.Bold | FontStyle.Underline);
                                hText.Border.Style = StiPenStyle.None;
                                hText.TextBrush = new StiSolidBrush(Color.Black);
                                hText.WordWrap = true;
                                hText.CanGrow = true;
                                headerBand.Components.Add(hText);

                                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                                dataText.Text.Value = "{Revenues.Column" + column.Index + "}";

                                if (column.Type != "Variance")
                                {
                                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }

                                if (column.Type == "Actual")
                                {
                                    dataText.Interaction.Hyperlink = new StiHyperlinkExpression("{Revenues.Column" + column.Index + "URL}");
                                }

                                dataText.HorAlignment = StiTextHorAlignment.Right;
                                dataText.VertAlignment = StiVertAlignment.Top;
                                dataText.Border.Style = StiPenStyle.None;
                                dataText.OnlyText = false;
                                dataText.Border.Side = StiBorderSides.All;
                                dataText.Font = new Font("Arial", 8F);
                                dataText.WordWrap = true;
                                dataText.CanGrow = true;
                                dataText.Margins = new StiMargins(0, 1, 4, 0);
                                dataBand.Components.Add(dataText);

                                pos = pos + columnWidth;
                            }

                            //Create group footer band
                            StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.4));
                            groupFooter.Name = $"RevenuesGroupFooterBand{center.Item1}";
                            page.Components.Add(groupFooter);

                            StiText subTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            subTotalText.Text.Value = "Total {Revenues.Sub}";
                            subTotalText.HorAlignment = StiTextHorAlignment.Left;
                            subTotalText.VertAlignment = StiVertAlignment.Center;
                            subTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            subTotalText.Border.Style = StiPenStyle.None;
                            subTotalText.TextBrush = new StiSolidBrush(Color.Black);
                            subTotalText.WordWrap = true;
                            groupFooter.Components.Add(subTotalText);

                            pos = (columnWidth * 2);
                            foreach (var column in objColumns)
                            {
                                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                totalText.HorAlignment = StiTextHorAlignment.Right;
                                totalText.VertAlignment = StiVertAlignment.Center;
                                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                totalText.TextBrush = new StiSolidBrush(Color.Black);
                                totalText.WordWrap = true;

                                if (column.Type != "Variance")
                                {
                                    totalText.Text.Value = "{Sum(Revenues.Column" + column.Index + ")}";
                                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    totalText.Text.Value = "{(Sum(Revenues.Column" + column.Column1 + ") - Sum(Revenues.Column" + column.Column2 + ")) / Abs(Sum(Revenues.Column" + column.Column2 + "))}";
                                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                groupFooter.Components.Add(totalText);

                                pos = pos + columnWidth;
                            }

                            //Create DataBand total
                            StiDataBand totalBand = new StiDataBand();
                            totalBand.DataSourceName = "RevenuesTotal";
                            totalBand.Name = $"RevenuesTotal{center.Item1}";
                            totalBand.Border.Style = StiPenStyle.None;
                            totalBand.Filters.Add(new StiFilter());
                            totalBand.Filters[0].Item = StiFilterItem.Expression;
                            totalBand.Filters[0].Expression = new StiExpression($"RevenuesTotal.Department == {center.Item1}");
                            page.Components.Add(totalBand);

                            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                            acctTotalText.Text.Value = "Total Revenues";
                            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                            acctTotalText.VertAlignment = StiVertAlignment.Center;
                            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctTotalText.OnlyText = false;
                            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctTotalText.WordWrap = true;
                            acctTotalText.Margins = new StiMargins(0, 1, 0, 10);
                            totalBand.Components.Add(acctTotalText);

                            pos = (columnWidth * 2);
                            foreach (var column in objColumns)
                            {
                                StiText footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                                footerText.Text.Value = "{RevenuesTotal.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    footerText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    footerText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                footerText.HorAlignment = StiTextHorAlignment.Right;
                                footerText.VertAlignment = StiVertAlignment.Center;
                                footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                footerText.OnlyText = false;
                                footerText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                footerText.WordWrap = true;
                                footerText.Margins = new StiMargins(0, 1, 0, 10);
                                totalBand.Components.Add(footerText);

                                pos = pos + columnWidth;
                            }
                        }

                        // Cost Of Sales
                        if (objColumns.Count > 0)
                        {
                            //Create Header
                            StiHeaderBand header = new StiHeaderBand();
                            header.Height = 0.25;
                            header.Name = $"CostOfSalesHeaderBand{center.Item1}";
                            header.Border.Style = StiPenStyle.None;
                            header.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            header.PrintOnAllPages = false;
                            header.PrintIfEmpty = true;
                            page.Components.Add(header);

                            StiText acctText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                            acctText.Text.Value = "Cost Of Sales";
                            acctText.HorAlignment = StiTextHorAlignment.Left;
                            acctText.VertAlignment = StiVertAlignment.Center;
                            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            acctText.Border.Side = StiBorderSides.All;
                            acctText.Font = new Font("Arial", 10F, FontStyle.Bold);
                            acctText.Border.Style = StiPenStyle.None;
                            acctText.TextBrush = new StiSolidBrush(Color.White);
                            acctText.WordWrap = true;
                            header.Components.Add(acctText);

                            //Create group header band
                            StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                            groupHeader.Name = $"CostOfSalesGroupHeaderBand{center.Item1}";
                            groupHeader.PrintOnAllPages = false;
                            groupHeader.Condition = new StiGroupConditionExpression("{CostOfSales.Sub}");
                            page.Components.Add(groupHeader);

                            StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                            groupHeaderText.Text.Value = "{CostOfSales.Sub}";
                            groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                            groupHeaderText.VertAlignment = StiVertAlignment.Center;
                            groupHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            groupHeaderText.Border.Style = StiPenStyle.None;
                            groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                            groupHeaderText.WordWrap = true;
                            groupHeader.Components.Add(groupHeaderText);

                            //Create HeaderBand
                            StiHeaderBand headerBand = new StiHeaderBand();
                            headerBand.Height = 0.25;
                            headerBand.Name = $"CostOfSalesBand{center.Item1}";
                            headerBand.Border.Style = StiPenStyle.None;
                            headerBand.PrintOnAllPages = false;
                            headerBand.PrintIfEmpty = true;
                            page.Components.Add(headerBand);

                            //Create Databand
                            StiDataBand dataBand = new StiDataBand();
                            dataBand.DataSourceName = "CostOfSales";
                            dataBand.Name = $"CostOfSalesData{center.Item1}";
                            dataBand.Border.Style = StiPenStyle.None;
                            dataBand.Filters.Add(new StiFilter());
                            dataBand.Filters[0].Item = StiFilterItem.Expression;
                            dataBand.Filters[0].Expression = new StiExpression($"CostOfSales.Department == {center.Item1}");
                            page.Components.Add(dataBand);

                            //Create DataBand detail item
                            StiText acctDataText = new StiText(new RectangleD(0.1, 0, columnWidth * 2 - 0.1, 0.2));
                            acctDataText.Text.Value = "{CostOfSales.fDesc}";
                            acctDataText.HorAlignment = StiTextHorAlignment.Left;
                            acctDataText.VertAlignment = StiVertAlignment.Center;
                            acctDataText.Border.Style = StiPenStyle.None;
                            acctDataText.OnlyText = false;
                            acctDataText.Border.Side = StiBorderSides.All;
                            acctDataText.Font = new Font("Arial", 8F);
                            acctDataText.WordWrap = true;
                            acctDataText.CanGrow = true;
                            acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(acctDataText);

                            pos = (columnWidth * 2);

                            foreach (var column in objColumns)
                            {
                                //Create text on header
                                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                                hText.Text.Value = column.Label;
                                hText.HorAlignment = StiTextHorAlignment.Right;
                                hText.VertAlignment = StiVertAlignment.Center;
                                hText.Border.Side = StiBorderSides.All;
                                hText.Font = new Font("Arial", 9F, FontStyle.Bold | FontStyle.Underline);
                                hText.Border.Style = StiPenStyle.None;
                                hText.TextBrush = new StiSolidBrush(Color.Black);
                                hText.WordWrap = true;
                                hText.CanGrow = true;
                                headerBand.Components.Add(hText);

                                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                                dataText.Text.Value = "{CostOfSales.Column" + column.Index + "}";

                                if (column.Type != "Variance")
                                {
                                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }

                                if (column.Type == "Actual")
                                {
                                    dataText.Interaction.Hyperlink = new StiHyperlinkExpression("{CostOfSales.Column" + column.Index + "URL}");
                                }

                                dataText.HorAlignment = StiTextHorAlignment.Right;
                                dataText.VertAlignment = StiVertAlignment.Top;
                                dataText.Border.Style = StiPenStyle.None;
                                dataText.OnlyText = false;
                                dataText.Border.Side = StiBorderSides.All;
                                dataText.Font = new Font("Arial", 8F);
                                dataText.WordWrap = true;
                                dataText.CanGrow = true;
                                dataText.Margins = new StiMargins(0, 1, 4, 0);
                                dataBand.Components.Add(dataText);

                                pos = pos + columnWidth;
                            }

                            //Create group footer band
                            StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.4));
                            groupFooter.Name = $"CostOfSalesGroupFooterBand{center.Item1}";
                            page.Components.Add(groupFooter);

                            StiText subTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            subTotalText.Text.Value = "Total {CostOfSales.Sub}";
                            subTotalText.HorAlignment = StiTextHorAlignment.Left;
                            subTotalText.VertAlignment = StiVertAlignment.Center;
                            subTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            subTotalText.Border.Style = StiPenStyle.None;
                            subTotalText.TextBrush = new StiSolidBrush(Color.Black);
                            subTotalText.WordWrap = true;
                            groupFooter.Components.Add(subTotalText);

                            pos = (columnWidth * 2);
                            foreach (var column in objColumns)
                            {
                                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                totalText.HorAlignment = StiTextHorAlignment.Right;
                                totalText.VertAlignment = StiVertAlignment.Center;
                                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                totalText.TextBrush = new StiSolidBrush(Color.Black);
                                totalText.WordWrap = true;

                                if (column.Type != "Variance")
                                {
                                    totalText.Text.Value = "{Sum(CostOfSales.Column" + column.Index + ")}";
                                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    totalText.Text.Value = "{(Sum(CostOfSales.Column" + column.Column1 + ") - Sum(CostOfSales.Column" + column.Column2 + ")) / Abs(Sum(CostOfSales.Column" + column.Column2 + "))}";
                                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                groupFooter.Components.Add(totalText);

                                pos = pos + columnWidth;
                            }

                            //Create DataBand total
                            StiDataBand totalBand = new StiDataBand();
                            totalBand.DataSourceName = "CostOfSalesTotal";
                            totalBand.Name = $"CostOfSalesTotal{center.Item1}";
                            totalBand.Border.Style = StiPenStyle.None;
                            totalBand.Filters.Add(new StiFilter());
                            totalBand.Filters[0].Item = StiFilterItem.Expression;
                            totalBand.Filters[0].Expression = new StiExpression($"CostOfSalesTotal.Department == {center.Item1}");
                            page.Components.Add(totalBand);

                            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctTotalText.Text.Value = "Total Cost Of Sales";
                            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                            acctTotalText.VertAlignment = StiVertAlignment.Center;
                            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctTotalText.OnlyText = false;
                            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctTotalText.WordWrap = true;
                            acctTotalText.Margins = new StiMargins(0, 1, 0, 0);
                            totalBand.Components.Add(acctTotalText);

                            //Create DataBand Gross Profit
                            StiDataBand grossBand = new StiDataBand();
                            grossBand.DataSourceName = "GrossProfit";
                            grossBand.Name = $"GrossProfitBand{center.Item1}";
                            grossBand.Border.Style = StiPenStyle.None;
                            grossBand.Filters.Add(new StiFilter());
                            grossBand.Filters[0].Item = StiFilterItem.Expression;
                            grossBand.Filters[0].Expression = new StiExpression($"GrossProfit.Department == {center.Item1}");
                            page.Components.Add(grossBand);

                            StiText acctGrossText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                            acctGrossText.Text.Value = "Gross Profit";
                            acctGrossText.HorAlignment = StiTextHorAlignment.Left;
                            acctGrossText.VertAlignment = StiVertAlignment.Center;
                            acctGrossText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctGrossText.OnlyText = false;
                            acctGrossText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctGrossText.WordWrap = true;
                            acctGrossText.Margins = new StiMargins(0, 1, 0, 10);
                            grossBand.Components.Add(acctGrossText);

                            // Gross Profit Percen
                            StiDataBand percenBand = new StiDataBand();
                            percenBand.DataSourceName = "GrossProfitPercen";
                            percenBand.Name = $"GrossProfitPercenBand{center.Item1}";
                            percenBand.Border.Style = StiPenStyle.None;
                            percenBand.Filters.Add(new StiFilter());
                            percenBand.Filters[0].Item = StiFilterItem.Expression;
                            percenBand.Filters[0].Expression = new StiExpression($"GrossProfitPercen.Department == {center.Item1}");
                            page.Components.Add(percenBand);

                            StiText acctPercenText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctPercenText.Text.Value = "Gross Profit %";
                            acctPercenText.HorAlignment = StiTextHorAlignment.Left;
                            acctPercenText.VertAlignment = StiVertAlignment.Center;
                            acctPercenText.OnlyText = false;
                            acctPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctPercenText.WordWrap = true;
                            acctPercenText.Margins = new StiMargins(0, 1, 0, 10);
                            percenBand.Components.Add(acctPercenText);

                            pos = (columnWidth * 2);
                            foreach (var column in objColumns)
                            {
                                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                totalText.Text.Value = "{CostOfSalesTotal.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                totalText.HorAlignment = StiTextHorAlignment.Right;
                                totalText.VertAlignment = StiVertAlignment.Center;
                                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                totalText.OnlyText = false;
                                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                totalText.WordWrap = true;
                                totalText.Margins = new StiMargins(0, 1, 0, 0);
                                totalBand.Components.Add(totalText);

                                StiText grossText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                                grossText.Text.Value = "{GrossProfit.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    grossText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    grossText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                grossText.Type = StiSystemTextType.Expression;
                                grossText.HorAlignment = StiTextHorAlignment.Right;
                                grossText.VertAlignment = StiVertAlignment.Center;
                                grossText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                grossText.OnlyText = false;
                                grossText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                grossText.WordWrap = true;
                                grossText.Margins = new StiMargins(0, 1, 0, 10);
                                grossBand.Components.Add(grossText);

                                StiText depPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                if (column.Type != "Variance")
                                {
                                    depPercenText.Text.Value = "{GrossProfitPercen.Column" + column.Index + "}";
                                }
                                depPercenText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " "); ;
                                depPercenText.HorAlignment = StiTextHorAlignment.Right;
                                depPercenText.VertAlignment = StiVertAlignment.Center;
                                depPercenText.OnlyText = false;
                                depPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                depPercenText.WordWrap = true;
                                depPercenText.Margins = new StiMargins(0, 1, 0, 10);
                                percenBand.Components.Add(depPercenText);

                                pos = pos + columnWidth;
                            }
                        }

                        // Expenses
                        if (objColumns.Count > 0)
                        {
                            //Create Header
                            StiHeaderBand header = new StiHeaderBand();
                            header.Height = 0.25;
                            header.Name = $"ExpensesHeaderBand{center.Item1}";
                            header.Border.Style = StiPenStyle.None;
                            header.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            header.PrintOnAllPages = false;
                            header.PrintIfEmpty = true;
                            page.Components.Add(header);

                            StiText acctText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                            acctText.Text.Value = "Expenses";
                            acctText.HorAlignment = StiTextHorAlignment.Left;
                            acctText.VertAlignment = StiVertAlignment.Center;
                            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            acctText.Border.Side = StiBorderSides.All;
                            acctText.Font = new Font("Arial", 10F, FontStyle.Bold);
                            acctText.Border.Style = StiPenStyle.None;
                            acctText.TextBrush = new StiSolidBrush(Color.White);
                            acctText.WordWrap = true;
                            header.Components.Add(acctText);

                            //Create group header band
                            StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                            groupHeader.Name = $"ExpensesGroupHeaderBand{center.Item1}";
                            groupHeader.PrintOnAllPages = false;
                            groupHeader.Condition = new StiGroupConditionExpression("{Expenses.Sub}");
                            page.Components.Add(groupHeader);

                            StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                            groupHeaderText.Text.Value = "{Expenses.Sub}";
                            groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                            groupHeaderText.VertAlignment = StiVertAlignment.Center;
                            groupHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            groupHeaderText.Border.Style = StiPenStyle.None;
                            groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                            groupHeaderText.WordWrap = true;
                            groupHeader.Components.Add(groupHeaderText);

                            //Create HeaderBand
                            StiHeaderBand headerBand = new StiHeaderBand();
                            headerBand.Height = 0.25;
                            headerBand.Name = $"ExpensesBand{center.Item1}";
                            headerBand.Border.Style = StiPenStyle.None;
                            headerBand.PrintOnAllPages = false;
                            headerBand.PrintIfEmpty = true;
                            page.Components.Add(headerBand);

                            //Create Databand
                            StiDataBand dataBand = new StiDataBand();
                            dataBand.DataSourceName = "Expenses";
                            dataBand.Name = $"ExpensesData{center.Item1}";
                            dataBand.Border.Style = StiPenStyle.None;
                            dataBand.Filters.Add(new StiFilter());
                            dataBand.Filters[0].Item = StiFilterItem.Expression;
                            dataBand.Filters[0].Expression = new StiExpression($"Expenses.Department == {center.Item1}");
                            page.Components.Add(dataBand);

                            //Create DataBand detail item
                            StiText acctDataText = new StiText(new RectangleD(0.1, 0, columnWidth * 2 - 0.1, 0.2));
                            acctDataText.Text.Value = "{Expenses.fDesc}";
                            acctDataText.HorAlignment = StiTextHorAlignment.Left;
                            acctDataText.VertAlignment = StiVertAlignment.Center;
                            acctDataText.Border.Style = StiPenStyle.None;
                            acctDataText.OnlyText = false;
                            acctDataText.Border.Side = StiBorderSides.All;
                            acctDataText.Font = new Font("Arial", 8F);
                            acctDataText.WordWrap = true;
                            acctDataText.CanGrow = true;
                            acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(acctDataText);

                            pos = (columnWidth * 2);

                            foreach (var column in objColumns)
                            {
                                //Create text on header
                                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                                hText.Text.Value = column.Label;
                                hText.HorAlignment = StiTextHorAlignment.Right;
                                hText.VertAlignment = StiVertAlignment.Center;
                                hText.Border.Side = StiBorderSides.All;
                                hText.Font = new Font("Arial", 9F, FontStyle.Bold | FontStyle.Underline);
                                hText.Border.Style = StiPenStyle.None;
                                hText.TextBrush = new StiSolidBrush(Color.Black);
                                hText.WordWrap = true;
                                hText.CanGrow = true;
                                headerBand.Components.Add(hText);

                                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                                dataText.Text.Value = "{Expenses.Column" + column.Index + "}";

                                if (column.Type != "Variance")
                                {
                                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }

                                if (column.Type == "Actual")
                                {
                                    dataText.Interaction.Hyperlink = new StiHyperlinkExpression("{Expenses.Column" + column.Index + "URL}");
                                }

                                dataText.HorAlignment = StiTextHorAlignment.Right;
                                dataText.VertAlignment = StiVertAlignment.Top;
                                dataText.Border.Style = StiPenStyle.None;
                                dataText.OnlyText = false;
                                dataText.Border.Side = StiBorderSides.All;
                                dataText.Font = new Font("Arial", 8F);
                                dataText.WordWrap = true;
                                dataText.CanGrow = true;
                                dataText.Margins = new StiMargins(0, 1, 4, 0);
                                dataBand.Components.Add(dataText);

                                pos = pos + columnWidth;
                            }

                            //Create group footer band
                            StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.4));
                            groupFooter.Name = $"ExpensesGroupFooterBand{center.Item1}";
                            page.Components.Add(groupFooter);

                            StiText subTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            subTotalText.Text.Value = "Total {Expenses.Sub}";
                            subTotalText.HorAlignment = StiTextHorAlignment.Left;
                            subTotalText.VertAlignment = StiVertAlignment.Center;
                            subTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            subTotalText.Border.Style = StiPenStyle.None;
                            subTotalText.TextBrush = new StiSolidBrush(Color.Black);
                            subTotalText.WordWrap = true;
                            groupFooter.Components.Add(subTotalText);

                            pos = (columnWidth * 2);
                            foreach (var column in objColumns)
                            {
                                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                totalText.HorAlignment = StiTextHorAlignment.Right;
                                totalText.VertAlignment = StiVertAlignment.Center;
                                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                totalText.TextBrush = new StiSolidBrush(Color.Black);
                                totalText.WordWrap = true;

                                if (column.Type != "Variance")
                                {
                                    totalText.Text.Value = "{Sum(Expenses.Column" + column.Index + ")}";
                                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    totalText.Text.Value = "{(Sum(Expenses.Column" + column.Column1 + ") - Sum(Expenses.Column" + column.Column2 + ")) / Abs(Sum(Expenses.Column" + column.Column2 + "))}";
                                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                groupFooter.Components.Add(totalText);

                                pos = pos + columnWidth;
                            }

                            //Create DataBand total
                            StiDataBand totalBand = new StiDataBand();
                            totalBand.DataSourceName = "ExpensesTotal";
                            totalBand.Name = $"ExpensesTotal{center.Item1}";
                            totalBand.Border.Style = StiPenStyle.None;
                            totalBand.Filters.Add(new StiFilter());
                            totalBand.Filters[0].Item = StiFilterItem.Expression;
                            totalBand.Filters[0].Expression = new StiExpression($"ExpensesTotal.Department == {center.Item1}");
                            page.Components.Add(totalBand);

                            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctTotalText.Text.Value = "Total Expenses";
                            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                            acctTotalText.VertAlignment = StiVertAlignment.Center;
                            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctTotalText.OnlyText = false;
                            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctTotalText.WordWrap = true;
                            acctTotalText.Margins = new StiMargins(0, 1, 0, 0);
                            totalBand.Components.Add(acctTotalText);

                            //Create DataBand Net Profit
                            StiDataBand netBand = new StiDataBand();
                            netBand.DataSourceName = "NetProfit";
                            netBand.Name = $"NetProfit{center.Item1}";
                            netBand.Border.Style = StiPenStyle.None;
                            netBand.Filters.Add(new StiFilter());
                            netBand.Filters[0].Item = StiFilterItem.Expression;
                            netBand.Filters[0].Expression = new StiExpression($"NetProfit.Department == {center.Item1}");
                            page.Components.Add(netBand);

                            StiText acctNetText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                            acctNetText.Text.Value = netProfitText;
                            acctNetText.HorAlignment = StiTextHorAlignment.Left;
                            acctNetText.VertAlignment = StiVertAlignment.Center;
                            acctNetText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctNetText.OnlyText = false;
                            acctNetText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctNetText.WordWrap = true;
                            acctNetText.Margins = new StiMargins(0, 1, 0, 10);
                            netBand.Components.Add(acctNetText);

                            // Net Profit Percen
                            StiDataBand percenBand = new StiDataBand();
                            percenBand.DataSourceName = "NetProfitPercen";
                            percenBand.Name = $"NetProfitPercenBand{center.Item1}";
                            percenBand.Border.Style = StiPenStyle.None;
                            percenBand.NewPageAfter = !_includeProvisions;
                            percenBand.Filters.Add(new StiFilter());
                            percenBand.Filters[0].Item = StiFilterItem.Expression;
                            percenBand.Filters[0].Expression = new StiExpression($"NetProfitPercen.Department == {center.Item1}");
                            page.Components.Add(percenBand);

                            StiText acctPercenText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctPercenText.Text.Value = $"{netProfitText} %";
                            acctPercenText.HorAlignment = StiTextHorAlignment.Left;
                            acctPercenText.VertAlignment = StiVertAlignment.Center;
                            acctPercenText.OnlyText = false;
                            acctPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctPercenText.WordWrap = true;
                            acctPercenText.Margins = new StiMargins(0, 1, 0, 10);
                            percenBand.Components.Add(acctPercenText);

                            pos = (columnWidth * 2);
                            foreach (var column in objColumns)
                            {
                                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                totalText.Text.Value = "{ExpensesTotal.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                totalText.HorAlignment = StiTextHorAlignment.Right;
                                totalText.VertAlignment = StiVertAlignment.Center;
                                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                totalText.OnlyText = false;
                                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                totalText.WordWrap = true;
                                totalText.Margins = new StiMargins(0, 1, 0, 0);
                                totalBand.Components.Add(totalText);

                                StiText netText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                                netText.Text.Value = "{NetProfit.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    netText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    netText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                netText.Type = StiSystemTextType.Expression;
                                netText.HorAlignment = StiTextHorAlignment.Right;
                                netText.VertAlignment = StiVertAlignment.Center;
                                netText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                netText.OnlyText = false;
                                netText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                netText.WordWrap = true;
                                netText.Margins = new StiMargins(0, 1, 0, 10);
                                netBand.Components.Add(netText);

                                StiText depPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                if (column.Type != "Variance")
                                {
                                    depPercenText.Text.Value = "{NetProfitPercen.Column" + column.Index + "}";
                                }
                                depPercenText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " "); ;
                                depPercenText.HorAlignment = StiTextHorAlignment.Right;
                                depPercenText.VertAlignment = StiVertAlignment.Center;
                                depPercenText.OnlyText = false;
                                depPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                depPercenText.WordWrap = true;
                                depPercenText.Margins = new StiMargins(0, 1, 0, 10);
                                percenBand.Components.Add(depPercenText);

                                pos = pos + columnWidth;
                            }
                        }

                        // Other Income (Expenses)
                        if (objColumns.Count > 0)
                        {
                            //Create Header
                            StiHeaderBand header = new StiHeaderBand();
                            header.Height = 0.25;
                            header.Name = $"OtherIncomeHeaderBand{center.Item1}";
                            header.Border.Style = StiPenStyle.None;
                            header.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            header.PrintOnAllPages = false;
                            header.PrintIfEmpty = false;
                            page.Components.Add(header);

                            StiText acctText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                            acctText.Text.Value = "Other Income (Expenses)";
                            acctText.HorAlignment = StiTextHorAlignment.Left;
                            acctText.VertAlignment = StiVertAlignment.Center;
                            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            acctText.Border.Side = StiBorderSides.All;
                            acctText.Font = new Font("Arial", 10F, FontStyle.Bold);
                            acctText.Border.Style = StiPenStyle.None;
                            acctText.TextBrush = new StiSolidBrush(Color.White);
                            acctText.WordWrap = true;
                            header.Components.Add(acctText);

                            //Create group header band
                            StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                            groupHeader.Name = $"OtherIncomeGroupHeaderBand{center.Item1}";
                            groupHeader.PrintOnAllPages = false;
                            groupHeader.Condition = new StiGroupConditionExpression("{OtherIncome.Sub}");
                            page.Components.Add(groupHeader);

                            StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                            groupHeaderText.Text.Value = "{OtherIncome.Sub}";
                            groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                            groupHeaderText.VertAlignment = StiVertAlignment.Center;
                            groupHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            groupHeaderText.Border.Style = StiPenStyle.None;
                            groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                            groupHeaderText.WordWrap = true;
                            groupHeader.Components.Add(groupHeaderText);

                            //Create HeaderBand
                            StiHeaderBand headerBand = new StiHeaderBand();
                            headerBand.Height = 0.25;
                            headerBand.Name = $"OtherIncomeBand{center.Item1}";
                            headerBand.Border.Style = StiPenStyle.None;
                            headerBand.PrintOnAllPages = false;
                            headerBand.PrintIfEmpty = false;
                            page.Components.Add(headerBand);

                            //Create Databand
                            StiDataBand dataBand = new StiDataBand();
                            dataBand.DataSourceName = "OtherIncome";
                            dataBand.Name = $"OtherIncomeData{center.Item1}";
                            dataBand.Border.Style = StiPenStyle.None;
                            dataBand.Filters.Add(new StiFilter());
                            dataBand.Filters[0].Item = StiFilterItem.Expression;
                            dataBand.Filters[0].Expression = new StiExpression($"OtherIncome.Department == {center.Item1}");
                            page.Components.Add(dataBand);

                            //Create DataBand detail item
                            StiText acctDataText = new StiText(new RectangleD(0.1, 0, columnWidth * 2 - 0.1, 0.2));
                            acctDataText.Text.Value = "{OtherIncome.fDesc}";
                            acctDataText.HorAlignment = StiTextHorAlignment.Left;
                            acctDataText.VertAlignment = StiVertAlignment.Center;
                            acctDataText.Border.Style = StiPenStyle.None;
                            acctDataText.OnlyText = false;
                            acctDataText.Border.Side = StiBorderSides.All;
                            acctDataText.Font = new Font("Arial", 8F);
                            acctDataText.WordWrap = true;
                            acctDataText.CanGrow = true;
                            acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(acctDataText);

                            pos = (columnWidth * 2);

                            foreach (var column in objColumns)
                            {
                                //Create text on header
                                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                                hText.Text.Value = column.Label;
                                hText.HorAlignment = StiTextHorAlignment.Right;
                                hText.VertAlignment = StiVertAlignment.Center;
                                hText.Border.Side = StiBorderSides.All;
                                hText.Font = new Font("Arial", 9F, FontStyle.Bold | FontStyle.Underline);
                                hText.Border.Style = StiPenStyle.None;
                                hText.TextBrush = new StiSolidBrush(Color.Black);
                                hText.WordWrap = true;
                                hText.CanGrow = true;
                                headerBand.Components.Add(hText);

                                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                                dataText.Text.Value = "{OtherIncome.Column" + column.Index + "}";

                                if (column.Type != "Variance")
                                {
                                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }

                                if (column.Type == "Actual")
                                {
                                    dataText.Interaction.Hyperlink = new StiHyperlinkExpression("{OtherIncome.Column" + column.Index + "URL}");
                                }

                                dataText.HorAlignment = StiTextHorAlignment.Right;
                                dataText.VertAlignment = StiVertAlignment.Top;
                                dataText.Border.Style = StiPenStyle.None;
                                dataText.OnlyText = false;
                                dataText.Border.Side = StiBorderSides.All;
                                dataText.Font = new Font("Arial", 8F);
                                dataText.WordWrap = true;
                                dataText.CanGrow = true;
                                dataText.Margins = new StiMargins(0, 1, 4, 0);
                                dataBand.Components.Add(dataText);

                                pos = pos + columnWidth;
                            }

                            //Create group footer band
                            StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.4));
                            groupFooter.Name = $"OtherIncomeGroupFooterBand{center.Item1}";
                            page.Components.Add(groupFooter);

                            StiText subTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            subTotalText.Text.Value = "Total {OtherIncome.Sub}";
                            subTotalText.HorAlignment = StiTextHorAlignment.Left;
                            subTotalText.VertAlignment = StiVertAlignment.Center;
                            subTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            subTotalText.Border.Style = StiPenStyle.None;
                            subTotalText.TextBrush = new StiSolidBrush(Color.Black);
                            subTotalText.WordWrap = true;
                            groupFooter.Components.Add(subTotalText);

                            pos = (columnWidth * 2);
                            foreach (var column in objColumns)
                            {
                                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                totalText.HorAlignment = StiTextHorAlignment.Right;
                                totalText.VertAlignment = StiVertAlignment.Center;
                                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                totalText.TextBrush = new StiSolidBrush(Color.Black);
                                totalText.WordWrap = true;

                                if (column.Type != "Variance")
                                {
                                    totalText.Text.Value = "{Sum(OtherIncome.Column" + column.Index + ")}";
                                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    totalText.Text.Value = "{(Sum(OtherIncome.Column" + column.Column1 + ") - Sum(OtherIncome.Column" + column.Column2 + ")) / Abs(Sum(OtherIncome.Column" + column.Column2 + "))}";
                                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                groupFooter.Components.Add(totalText);

                                pos = pos + columnWidth;
                            }

                            //Create DataBand total
                            StiDataBand totalBand = new StiDataBand();
                            totalBand.DataSourceName = "OtherIncomeTotal";
                            totalBand.Name = $"OtherIncomeTotal{center.Item1}";
                            totalBand.Border.Style = StiPenStyle.None;
                            totalBand.Filters.Add(new StiFilter());
                            totalBand.Filters[0].Item = StiFilterItem.Expression;
                            totalBand.Filters[0].Expression = new StiExpression($"OtherIncomeTotal.Department == {center.Item1}");
                            page.Components.Add(totalBand);

                            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctTotalText.Text.Value = "Total Other Income (Expenses)";
                            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                            acctTotalText.VertAlignment = StiVertAlignment.Center;
                            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctTotalText.OnlyText = false;
                            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctTotalText.WordWrap = true;
                            acctTotalText.Margins = new StiMargins(0, 1, 0, 0);
                            totalBand.Components.Add(acctTotalText);

                            //Create DataBand Income Before Provisions For Income Taxes
                            StiDataBand grossBand = new StiDataBand();
                            grossBand.DataSourceName = "BeforeProvisions";
                            grossBand.Name = $"BeforeProvisionsBand{center.Item1}";
                            grossBand.Border.Style = StiPenStyle.None;
                            grossBand.Filters.Add(new StiFilter());
                            grossBand.Filters[0].Item = StiFilterItem.Expression;
                            grossBand.Filters[0].Expression = new StiExpression($"BeforeProvisions.Department == {center.Item1}");
                            page.Components.Add(grossBand);

                            StiText acctGrossText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                            acctGrossText.Text.Value = "Income Before Provisions For Income Taxes";
                            acctGrossText.HorAlignment = StiTextHorAlignment.Left;
                            acctGrossText.VertAlignment = StiVertAlignment.Center;
                            acctGrossText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctGrossText.OnlyText = false;
                            acctGrossText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctGrossText.WordWrap = true;
                            acctGrossText.Margins = new StiMargins(0, 1, 0, 10);
                            grossBand.Components.Add(acctGrossText);

                            // Income Before Provisions For Income Taxes Percen
                            StiDataBand percenBand = new StiDataBand();
                            percenBand.DataSourceName = "BeforeProvisionsPercen";
                            percenBand.Name = $"BeforeProvisionsPercenBand{center.Item1}";
                            percenBand.Border.Style = StiPenStyle.None;
                            percenBand.Filters.Add(new StiFilter());
                            percenBand.Filters[0].Item = StiFilterItem.Expression;
                            percenBand.Filters[0].Expression = new StiExpression($"BeforeProvisionsPercen.Department == {center.Item1}");
                            page.Components.Add(percenBand);

                            StiText acctPercenText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctPercenText.Text.Value = "Income Before Provisions For Income Taxes %";
                            acctPercenText.HorAlignment = StiTextHorAlignment.Left;
                            acctPercenText.VertAlignment = StiVertAlignment.Center;
                            acctPercenText.OnlyText = false;
                            acctPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctPercenText.WordWrap = true;
                            acctPercenText.Margins = new StiMargins(0, 1, 0, 10);
                            percenBand.Components.Add(acctPercenText);

                            pos = (columnWidth * 2);
                            foreach (var column in objColumns)
                            {
                                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                totalText.Text.Value = "{OtherIncomeTotal.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                totalText.HorAlignment = StiTextHorAlignment.Right;
                                totalText.VertAlignment = StiVertAlignment.Center;
                                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                totalText.OnlyText = false;
                                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                totalText.WordWrap = true;
                                totalText.Margins = new StiMargins(0, 1, 0, 0);
                                totalBand.Components.Add(totalText);

                                StiText grossText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                                grossText.Text.Value = "{BeforeProvisions.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    grossText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    grossText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                grossText.Type = StiSystemTextType.Expression;
                                grossText.HorAlignment = StiTextHorAlignment.Right;
                                grossText.VertAlignment = StiVertAlignment.Center;
                                grossText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                grossText.OnlyText = false;
                                grossText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                grossText.WordWrap = true;
                                grossText.Margins = new StiMargins(0, 1, 0, 10);
                                grossBand.Components.Add(grossText);

                                StiText depPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                if (column.Type != "Variance")
                                {
                                    depPercenText.Text.Value = "{BeforeProvisionsPercen.Column" + column.Index + "}";
                                }
                                depPercenText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " "); ;
                                depPercenText.HorAlignment = StiTextHorAlignment.Right;
                                depPercenText.VertAlignment = StiVertAlignment.Center;
                                depPercenText.OnlyText = false;
                                depPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                depPercenText.WordWrap = true;
                                depPercenText.Margins = new StiMargins(0, 1, 0, 10);
                                percenBand.Components.Add(depPercenText);

                                pos = pos + columnWidth;
                            }
                        }

                        // Provisions for Income Taxes
                        if (objColumns.Count > 0)
                        {
                            //Create Header
                            StiHeaderBand header = new StiHeaderBand();
                            header.Height = 0.25;
                            header.Name = $"IncomeTaxesHeaderBand{center.Item1}";
                            header.Border.Style = StiPenStyle.None;
                            header.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            header.PrintOnAllPages = false;
                            header.PrintIfEmpty = false;
                            page.Components.Add(header);

                            StiText acctText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                            acctText.Text.Value = "Provisions for Income Taxes";
                            acctText.HorAlignment = StiTextHorAlignment.Left;
                            acctText.VertAlignment = StiVertAlignment.Center;
                            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            acctText.Border.Side = StiBorderSides.All;
                            acctText.Font = new Font("Arial", 10F, FontStyle.Bold);
                            acctText.Border.Style = StiPenStyle.None;
                            acctText.TextBrush = new StiSolidBrush(Color.White);
                            acctText.WordWrap = true;
                            header.Components.Add(acctText);

                            //Create group header band
                            StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                            groupHeader.Name = $"IncomeTaxesGroupHeaderBand{center.Item1}";
                            groupHeader.PrintOnAllPages = false;
                            groupHeader.Condition = new StiGroupConditionExpression("{IncomeTaxes.Sub}");
                            page.Components.Add(groupHeader);

                            StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                            groupHeaderText.Text.Value = "{IncomeTaxes.Sub}";
                            groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                            groupHeaderText.VertAlignment = StiVertAlignment.Center;
                            groupHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            groupHeaderText.Border.Style = StiPenStyle.None;
                            groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                            groupHeaderText.WordWrap = true;
                            groupHeader.Components.Add(groupHeaderText);

                            //Create HeaderBand
                            StiHeaderBand headerBand = new StiHeaderBand();
                            headerBand.Height = 0.25;
                            headerBand.Name = $"IncomeTaxesBand{center.Item1}";
                            headerBand.Border.Style = StiPenStyle.None;
                            headerBand.PrintOnAllPages = false;
                            headerBand.PrintIfEmpty = false;
                            page.Components.Add(headerBand);

                            //Create Databand
                            StiDataBand dataBand = new StiDataBand();
                            dataBand.DataSourceName = "IncomeTaxes";
                            dataBand.Name = $"IncomeTaxesData{center.Item1}";
                            dataBand.Border.Style = StiPenStyle.None;
                            dataBand.Filters.Add(new StiFilter());
                            dataBand.Filters[0].Item = StiFilterItem.Expression;
                            dataBand.Filters[0].Expression = new StiExpression($"IncomeTaxes.Department == {center.Item1}");
                            page.Components.Add(dataBand);

                            //Create DataBand detail item
                            StiText acctDataText = new StiText(new RectangleD(0.1, 0, columnWidth * 2 - 0.1, 0.2));
                            acctDataText.Text.Value = "{IncomeTaxes.fDesc}";
                            acctDataText.HorAlignment = StiTextHorAlignment.Left;
                            acctDataText.VertAlignment = StiVertAlignment.Center;
                            acctDataText.Border.Style = StiPenStyle.None;
                            acctDataText.OnlyText = false;
                            acctDataText.Border.Side = StiBorderSides.All;
                            acctDataText.Font = new Font("Arial", 8F);
                            acctDataText.WordWrap = true;
                            acctDataText.CanGrow = true;
                            acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(acctDataText);

                            pos = (columnWidth * 2);

                            foreach (var column in objColumns)
                            {
                                //Create text on header
                                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                                hText.Text.Value = column.Label;
                                hText.HorAlignment = StiTextHorAlignment.Right;
                                hText.VertAlignment = StiVertAlignment.Center;
                                hText.Border.Side = StiBorderSides.All;
                                hText.Font = new Font("Arial", 9F, FontStyle.Bold | FontStyle.Underline);
                                hText.Border.Style = StiPenStyle.None;
                                hText.TextBrush = new StiSolidBrush(Color.Black);
                                hText.WordWrap = true;
                                hText.CanGrow = true;
                                headerBand.Components.Add(hText);

                                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                                dataText.Text.Value = "{IncomeTaxes.Column" + column.Index + "}";

                                if (column.Type != "Variance")
                                {
                                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }

                                if (column.Type == "Actual")
                                {
                                    dataText.Interaction.Hyperlink = new StiHyperlinkExpression("{IncomeTaxes.Column" + column.Index + "URL}");
                                }

                                dataText.HorAlignment = StiTextHorAlignment.Right;
                                dataText.VertAlignment = StiVertAlignment.Top;
                                dataText.Border.Style = StiPenStyle.None;
                                dataText.OnlyText = false;
                                dataText.Border.Side = StiBorderSides.All;
                                dataText.Font = new Font("Arial", 8F);
                                dataText.WordWrap = true;
                                dataText.CanGrow = true;
                                dataText.Margins = new StiMargins(0, 1, 4, 0);
                                dataBand.Components.Add(dataText);

                                pos = pos + columnWidth;
                            }

                            //Create group footer band
                            StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.4));
                            groupFooter.Name = $"IncomeTaxesGroupFooterBand{center.Item1}";
                            page.Components.Add(groupFooter);

                            StiText subTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            subTotalText.Text.Value = "Total {IncomeTaxes.Sub}";
                            subTotalText.HorAlignment = StiTextHorAlignment.Left;
                            subTotalText.VertAlignment = StiVertAlignment.Center;
                            subTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            subTotalText.Border.Style = StiPenStyle.None;
                            subTotalText.TextBrush = new StiSolidBrush(Color.Black);
                            subTotalText.WordWrap = true;
                            groupFooter.Components.Add(subTotalText);

                            pos = (columnWidth * 2);
                            foreach (var column in objColumns)
                            {
                                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                totalText.HorAlignment = StiTextHorAlignment.Right;
                                totalText.VertAlignment = StiVertAlignment.Center;
                                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                totalText.TextBrush = new StiSolidBrush(Color.Black);
                                totalText.WordWrap = true;

                                if (column.Type != "Variance")
                                {
                                    totalText.Text.Value = "{Sum(IncomeTaxes.Column" + column.Index + ")}";
                                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    totalText.Text.Value = "{(Sum(IncomeTaxes.Column" + column.Column1 + ") - Sum(IncomeTaxes.Column" + column.Column2 + ")) / Abs(Sum(IncomeTaxes.Column" + column.Column2 + "))}";
                                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                groupFooter.Components.Add(totalText);

                                pos = pos + columnWidth;
                            }

                            //Create DataBand total
                            StiDataBand totalBand = new StiDataBand();
                            totalBand.DataSourceName = "IncomeTaxesTotal";
                            totalBand.Name = $"IncomeTaxesTotal{center.Item1}";
                            totalBand.Border.Style = StiPenStyle.None;
                            totalBand.Filters.Add(new StiFilter());
                            totalBand.Filters[0].Item = StiFilterItem.Expression;
                            totalBand.Filters[0].Expression = new StiExpression($"IncomeTaxesTotal.Department == {center.Item1}");
                            page.Components.Add(totalBand);

                            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctTotalText.Text.Value = "Total Provisions for Income Taxes";
                            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                            acctTotalText.VertAlignment = StiVertAlignment.Center;
                            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctTotalText.OnlyText = false;
                            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctTotalText.WordWrap = true;
                            acctTotalText.Margins = new StiMargins(0, 1, 0, 0);
                            totalBand.Components.Add(acctTotalText);

                            //Create DataBand Net Income
                            StiDataBand netBand = new StiDataBand();
                            netBand.DataSourceName = "NetIncome";
                            netBand.Name = $"NetIncome{center.Item1}";
                            netBand.Border.Style = StiPenStyle.None;
                            netBand.Filters.Add(new StiFilter());
                            netBand.Filters[0].Item = StiFilterItem.Expression;
                            netBand.Filters[0].Expression = new StiExpression($"NetIncome.Department == {center.Item1}");
                            page.Components.Add(netBand);

                            StiText acctNetText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                            acctNetText.Text.Value = "Net Income";
                            acctNetText.HorAlignment = StiTextHorAlignment.Left;
                            acctNetText.VertAlignment = StiVertAlignment.Center;
                            acctNetText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctNetText.OnlyText = false;
                            acctNetText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctNetText.WordWrap = true;
                            acctNetText.Margins = new StiMargins(0, 1, 0, 10);
                            netBand.Components.Add(acctNetText);

                            // Net Income Percen
                            StiDataBand percenBand = new StiDataBand();
                            percenBand.DataSourceName = "NetIncomePercen";
                            percenBand.Name = $"NetIncomePercenBand{center.Item1}";
                            percenBand.Border.Style = StiPenStyle.None;
                            percenBand.NewPageAfter = true;
                            percenBand.Filters.Add(new StiFilter());
                            percenBand.Filters[0].Item = StiFilterItem.Expression;
                            percenBand.Filters[0].Expression = new StiExpression($"NetIncomePercen.Department == {center.Item1}");
                            page.Components.Add(percenBand);

                            StiText acctPercenText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctPercenText.Text.Value = "Net Income %";
                            acctPercenText.HorAlignment = StiTextHorAlignment.Left;
                            acctPercenText.VertAlignment = StiVertAlignment.Center;
                            acctPercenText.OnlyText = false;
                            acctPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctPercenText.WordWrap = true;
                            acctPercenText.Margins = new StiMargins(0, 1, 0, 10);
                            percenBand.Components.Add(acctPercenText);

                            pos = (columnWidth * 2);
                            foreach (var column in objColumns)
                            {
                                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                totalText.Text.Value = "{IncomeTaxesTotal.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                totalText.HorAlignment = StiTextHorAlignment.Right;
                                totalText.VertAlignment = StiVertAlignment.Center;
                                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                totalText.OnlyText = false;
                                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                totalText.WordWrap = true;
                                totalText.Margins = new StiMargins(0, 1, 0, 0);
                                totalBand.Components.Add(totalText);

                                StiText netText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                                netText.Text.Value = "{NetIncome.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    netText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    netText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                netText.Type = StiSystemTextType.Expression;
                                netText.HorAlignment = StiTextHorAlignment.Right;
                                netText.VertAlignment = StiVertAlignment.Center;
                                netText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                netText.OnlyText = false;
                                netText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                netText.WordWrap = true;
                                netText.Margins = new StiMargins(0, 1, 0, 10);
                                netBand.Components.Add(netText);

                                StiText depPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                if (column.Type != "Variance")
                                {
                                    depPercenText.Text.Value = "{NetIncomePercen.Column" + column.Index + "}";
                                }
                                depPercenText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " "); ;
                                depPercenText.HorAlignment = StiTextHorAlignment.Right;
                                depPercenText.VertAlignment = StiVertAlignment.Center;
                                depPercenText.OnlyText = false;
                                depPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                depPercenText.WordWrap = true;
                                depPercenText.Margins = new StiMargins(0, 1, 0, 10);
                                percenBand.Components.Add(depPercenText);

                                pos = pos + columnWidth;
                            }
                        }
                    }
                }
            }

            // Build Total Summary
            BuildAllDepartmentTotal(page, objComparative, true);

            // Build Department Summary
            if (departments.Count() > 0)
            {
                DepartmentSummaryDataSource(report, objComparative, departments, data);
            }

            if (deptArray.Length > 1)
            {
                BuildDepartmentSummary(page, objComparative, departments);
            }

            DataSet dsC = new DataSet();

            var connString = Session["config"].ToString();
            objPropUser.ConnConfig = connString;
            dsC = objBL_User.getControl(objPropUser);

            DataTable cTable = BuildCompanyDetailsTable();
            var cRow = cTable.NewRow();

            DataSet companyInfo = new DataSet();
            companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());

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

            report.RegData("CompanyDetails", cTable);
            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();
            report.Dictionary.Variables["ReportName"].Value = Session["ReportName"].ToString();

            dView.RowFilter = "Type = 3";
            DataTable Revenues = dView.ToTable();

            dView.RowFilter = "Type = 4";
            DataTable CostOfSales = dView.ToTable();

            dView.RowFilter = "Type = 5";
            DataTable Expenses = dView.ToTable();

            dView.RowFilter = "Type = 8";
            DataTable OtherIncome = dView.ToTable();

            dView.RowFilter = "Type = 9";
            DataTable IncomeTaxes = dView.ToTable();

            report.RegData("Revenues", Revenues);
            report.RegData("CostOfSales", CostOfSales);
            report.RegData("Expenses", Expenses);
            report.RegData("OtherIncome", OtherIncome);
            report.RegData("IncomeTaxes", IncomeTaxes);
            report.RegData("RevenuesTotal", _revenuesTotal);
            report.RegData("CostOfSalesTotal", _costOfSalesTotal);
            report.RegData("ExpensesTotal", _expensesTotal);
            report.RegData("OtherIncomeTotal", _otherIncomeTotal);
            report.RegData("IncomeTaxesTotal", _incomeTaxesTotal);
            report.RegData("GrossProfit", _grossProfit);
            report.RegData("GrossProfitPercen", _grossProfitPercen);
            report.RegData("NetProfit", _netProfit);
            report.RegData("NetProfitPercen", _netProfitPercen);
            report.RegData("BeforeProvisions", _beforeProvisions);
            report.RegData("BeforeProvisionsPercen", _beforeProvisionsPercen);
            report.RegData("NetIncome", _netIncome);
            report.RegData("NetIncomePercen", _netIncomePercen);
            report.RegData("SummaryTotal", SummaryTotal(data, objComparative));

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

    private StiReport GetComparativeProfitAndLossSummaryReport(DataTable comparativeReportData, List<ComparativeStatementRequest> objComparative)
    {
        try
        {
            string reportPathStimul = string.Empty;
            reportPathStimul = Server.MapPath("StimulsoftReports/ComparativeStatementReport.mrt");
            StiReport report = new StiReport();
            report.Load(reportPathStimul);

            // Selected department
            var departments = comparativeReportData.AsEnumerable()
                                .Select(g =>
                                {
                                    return Tuple.Create(g.Field<int>("Department"), g.Field<string>("CentralName"));
                                }).Distinct().OrderBy(x => x.Item2);

            var objColumns = objComparative;
            foreach (var column in objColumns)
            {
                report.Dictionary.DataSources["Revenues"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["CostOfSales"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["Expenses"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["OtherIncome"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["IncomeTaxes"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["RevenuesTotal"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["CostOfSalesTotal"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["ExpensesTotal"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["OtherIncomeTotal"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["IncomeTaxesTotal"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["GrossProfit"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["GrossProfitPercen"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["NetProfit"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["NetProfitPercen"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["BeforeProvisions"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["BeforeProvisionsPercen"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["NetIncome"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["NetIncomePercen"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["SummaryTotal"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));

                if (column.Type == "Actual")
                {
                    report.Dictionary.DataSources["Revenues"].Columns.Add(string.Format("Column{0}URL", column.Index), typeof(string));
                    report.Dictionary.DataSources["CostOfSales"].Columns.Add(string.Format("Column{0}URL", column.Index), typeof(string));
                    report.Dictionary.DataSources["Expenses"].Columns.Add(string.Format("Column{0}URL", column.Index), typeof(string));
                    report.Dictionary.DataSources["OtherIncome"].Columns.Add(string.Format("Column{0}URL", column.Index), typeof(string));
                    report.Dictionary.DataSources["IncomeTaxes"].Columns.Add(string.Format("Column{0}URL", column.Index), typeof(string));
                }

                if (departments.Count() > 0)
                {
                    StiDataSource dtSourceRevenues = new StiDataTableSource($"DepSummaryRevenues{column.Index}", $"DepSummaryRevenues{column.Index}", $"DepSummaryRevenues{column.Index}");
                    BuildDepartmentSummaryDataSource(dtSourceRevenues, column, departments);
                    report.Dictionary.DataSources.Add(dtSourceRevenues);

                    StiDataSource dtSourceCostOfSales = new StiDataTableSource($"DepSummaryCostOfSales{column.Index}", $"DepSummaryCostOfSales{column.Index}", $"DepSummaryCostOfSales{column.Index}");
                    BuildDepartmentSummaryDataSource(dtSourceCostOfSales, column, departments);
                    report.Dictionary.DataSources.Add(dtSourceCostOfSales);

                    StiDataSource dtSourceExpenses = new StiDataTableSource($"DepSummaryExpenses{column.Index}", $"DepSummaryExpenses{column.Index}", $"DepSummaryExpenses{column.Index}");
                    BuildDepartmentSummaryDataSource(dtSourceExpenses, column, departments);
                    report.Dictionary.DataSources.Add(dtSourceExpenses);

                    StiDataSource dtSourceOtherIncome = new StiDataTableSource($"DepSummaryOtherIncome{column.Index}", $"DepSummaryOtherIncome{column.Index}", $"DepSummaryOtherIncome{column.Index}");
                    BuildDepartmentSummaryDataSource(dtSourceOtherIncome, column, departments);
                    report.Dictionary.DataSources.Add(dtSourceOtherIncome);

                    StiDataSource dtSourceIncomeTaxes = new StiDataTableSource($"DepSummaryIncomeTaxes{column.Index}", $"DepSummaryIncomeTaxes{column.Index}", $"DepSummaryIncomeTaxes{column.Index}");
                    BuildDepartmentSummaryDataSource(dtSourceIncomeTaxes, column, departments);
                    report.Dictionary.DataSources.Add(dtSourceIncomeTaxes);

                    StiDataSource dtConsolidating = new StiDataTableSource($"Consolidating{column.Index}", $"Consolidating{column.Index}", $"Consolidating{column.Index}");
                    BuildConsolidatingDataSource(dtConsolidating, column, departments);
                    report.Dictionary.DataSources.Add(dtConsolidating);
                }
            }

            StiPage page = report.Pages[0];
            page.CanGrow = true;
            page.CanShrink = true;

            var columnCount = objColumns.Count + 2;
            double columnWidth = page.Width / columnCount;
            double pos = 0;

            var data = comparativeReportData;
            DataTable filteredTable = data.Copy();
            DataView dView = filteredTable.DefaultView;

            _revenuesTotal = BuildTotalTable(objColumns);
            _costOfSalesTotal = BuildTotalTable(objColumns);
            _expensesTotal = BuildTotalTable(objColumns);
            _otherIncomeTotal = BuildTotalTable(objColumns);
            _incomeTaxesTotal = BuildTotalTable(objColumns);
            _grossProfit = BuildTotalTable(objColumns);
            _grossProfitPercen = BuildTotalTable(objColumns);
            _netProfit = BuildTotalTable(objColumns);
            _netProfitPercen = BuildTotalTable(objColumns);
            _beforeProvisions = BuildTotalTable(objColumns);
            _beforeProvisionsPercen = BuildTotalTable(objColumns);
            _netIncome = BuildTotalTable(objColumns);
            _netIncomePercen = BuildTotalTable(objColumns);

            // Build Consolidating Divisional Financial Statements
            //if (departments.Count() > 0)
            //{
            //    BuildConsolidatingDivisional(page, objComparative, departments);
            //}

            string[] deptArray = new string[] { };
            if (Session["ComparativeCenter"] != null)
            {
                var depts = Session["ComparativeCenter"].ToString();
                deptArray = depts.Split(',');
            }

            // Build for each Department
            if (deptArray.Length > 1)
            {
                var centers = (List<Tuple<int, string>>)Session["SelectedCenters"];
                foreach (var center in centers)
                {
                    dView.RowFilter = $"Department = {center.Item1}";
                    DataTable centerData = dView.ToTable();

                    if (centerData.Rows.Count > 0)
                    {
                        CenterSummaryTotal(centerData, objColumns, center.Item1);

                        var netProfitText = "Income From Operations";

                        //Add Center title
                        StiHeaderBand centerTitle = new StiHeaderBand();
                        centerTitle.Height = 0.5;
                        centerTitle.PrintIfEmpty = true;
                        page.Components.Add(centerTitle);

                        StiText centerTitleText = new StiText(new RectangleD(0, 0.1, page.Width, 0.25));
                        centerTitleText.Text.Value = $"{center.Item2} Division";
                        centerTitleText.HorAlignment = StiTextHorAlignment.Center;
                        centerTitleText.VertAlignment = StiVertAlignment.Center;
                        centerTitleText.Font = new Font("Arial", 18F, System.Drawing.FontStyle.Bold);
                        centerTitleText.WordWrap = true;
                        centerTitle.Components.Add(centerTitleText);

                        // Revenues
                        if (objColumns.Count > 0)
                        {
                            //Create HeaderBand
                            StiHeaderBand headerBand = new StiHeaderBand();
                            headerBand.Height = 0.25;
                            headerBand.Name = $"RevenuesHeaderBand{center.Item1}";
                            headerBand.Border.Style = StiPenStyle.None;
                            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            headerBand.PrintOnAllPages = false;
                            headerBand.PrintIfEmpty = true;
                            page.Components.Add(headerBand);

                            //Create Databand
                            StiDataBand dataBand = new StiDataBand();
                            dataBand.DataSourceName = "Revenues";
                            dataBand.Name = $"RevenuesData{center.Item1}";
                            dataBand.Border.Style = StiPenStyle.None;
                            dataBand.Filters.Add(new StiFilter());
                            dataBand.Filters[0].Item = StiFilterItem.Expression;
                            dataBand.Filters[0].Expression = new StiExpression($"Revenues.Department == {center.Item1}");
                            page.Components.Add(dataBand);

                            //Create DataBand item
                            StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctText.Text.Value = "Revenues";
                            acctText.HorAlignment = StiTextHorAlignment.Left;
                            acctText.VertAlignment = StiVertAlignment.Center;
                            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            acctText.Border.Side = StiBorderSides.All;
                            acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctText.Border.Style = StiPenStyle.None;
                            acctText.TextBrush = new StiSolidBrush(Color.White);
                            acctText.WordWrap = true;
                            headerBand.Components.Add(acctText);

                            StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.2));
                            acctDataText.Text.Value = "{Revenues.Sub}";
                            acctDataText.HorAlignment = StiTextHorAlignment.Left;
                            acctDataText.VertAlignment = StiVertAlignment.Center;
                            acctDataText.Border.Style = StiPenStyle.None;
                            acctDataText.OnlyText = false;
                            acctDataText.Border.Side = StiBorderSides.All;
                            acctDataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                            acctDataText.WordWrap = true;
                            acctDataText.CanGrow = true;
                            acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(acctDataText);

                            pos = (columnWidth * 2);

                            foreach (var column in objColumns)
                            {
                                //Create text on header
                                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                                hText.Text.Value = column.Label;
                                hText.HorAlignment = StiTextHorAlignment.Right;
                                hText.VertAlignment = StiVertAlignment.Center;
                                hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                                hText.Border.Side = StiBorderSides.All;
                                hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                hText.Border.Style = StiPenStyle.None;
                                hText.TextBrush = new StiSolidBrush(Color.White);
                                hText.WordWrap = true;
                                hText.CanGrow = true;
                                headerBand.Components.Add(hText);

                                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                                dataText.Text.Value = "{Revenues.Column" + column.Index + "}";

                                if (column.Type != "Variance")
                                {
                                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }

                                dataText.HorAlignment = StiTextHorAlignment.Right;
                                dataText.VertAlignment = StiVertAlignment.Top;
                                dataText.Border.Style = StiPenStyle.None;
                                dataText.OnlyText = false;
                                dataText.Border.Side = StiBorderSides.All;
                                dataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                                dataText.WordWrap = true;
                                dataText.CanGrow = true;
                                dataText.Margins = new StiMargins(0, 1, 4, 0);
                                dataBand.Components.Add(dataText);

                                pos = pos + columnWidth;
                            }

                            //Create DataBand total
                            StiDataBand totalBand = new StiDataBand();
                            totalBand.DataSourceName = "RevenuesTotal";
                            totalBand.Name = $"RevenuesTotal{center.Item1}";
                            totalBand.Border.Style = StiPenStyle.None;
                            totalBand.Filters.Add(new StiFilter());
                            totalBand.Filters[0].Item = StiFilterItem.Expression;
                            totalBand.Filters[0].Expression = new StiExpression($"RevenuesTotal.Department == {center.Item1}");
                            page.Components.Add(totalBand);

                            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                            acctTotalText.Text.Value = "Total Revenues";
                            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                            acctTotalText.VertAlignment = StiVertAlignment.Center;
                            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctTotalText.OnlyText = false;
                            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctTotalText.WordWrap = true;
                            acctTotalText.Margins = new StiMargins(0, 1, 0, 10);
                            totalBand.Components.Add(acctTotalText);

                            pos = (columnWidth * 2);
                            foreach (var column in objColumns)
                            {
                                StiText footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                                footerText.Text.Value = "{RevenuesTotal.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    footerText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    footerText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                footerText.HorAlignment = StiTextHorAlignment.Right;
                                footerText.VertAlignment = StiVertAlignment.Center;
                                footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                footerText.OnlyText = false;
                                footerText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                footerText.WordWrap = true;
                                footerText.Margins = new StiMargins(0, 1, 0, 10);
                                totalBand.Components.Add(footerText);

                                pos = pos + columnWidth;
                            }
                        }

                        // Cost Of Sales
                        if (objColumns.Count > 0)
                        {
                            //Create HeaderBand
                            StiHeaderBand headerBand = new StiHeaderBand();
                            headerBand.Height = 0.25;
                            headerBand.Name = $"CostOfSalesHeaderBand{center.Item1}";
                            headerBand.Border.Style = StiPenStyle.None;
                            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            headerBand.PrintOnAllPages = false;
                            headerBand.PrintIfEmpty = true;
                            page.Components.Add(headerBand);

                            //Create Databand
                            StiDataBand dataBand = new StiDataBand();
                            dataBand.DataSourceName = "CostOfSales";
                            dataBand.Name = $"CostOfSalesData{center.Item1}";
                            dataBand.Border.Style = StiPenStyle.None;
                            dataBand.Filters.Add(new StiFilter());
                            dataBand.Filters[0].Item = StiFilterItem.Expression;
                            dataBand.Filters[0].Expression = new StiExpression($"CostOfSales.Department == {center.Item1}");
                            page.Components.Add(dataBand);

                            //Create DataBand item
                            StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctText.Text.Value = "Cost Of Sales";
                            acctText.HorAlignment = StiTextHorAlignment.Left;
                            acctText.VertAlignment = StiVertAlignment.Center;
                            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            acctText.Border.Side = StiBorderSides.All;
                            acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctText.Border.Style = StiPenStyle.None;
                            acctText.TextBrush = new StiSolidBrush(Color.White);
                            acctText.WordWrap = true;
                            headerBand.Components.Add(acctText);

                            StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.2));
                            acctDataText.Text.Value = "{CostOfSales.Sub}";
                            acctDataText.HorAlignment = StiTextHorAlignment.Left;
                            acctDataText.VertAlignment = StiVertAlignment.Center;
                            acctDataText.Border.Style = StiPenStyle.None;
                            acctDataText.OnlyText = false;
                            acctDataText.Border.Side = StiBorderSides.All;
                            acctDataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                            acctDataText.WordWrap = true;
                            acctDataText.CanGrow = true;
                            acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(acctDataText);

                            pos = (columnWidth * 2);

                            foreach (var column in objColumns)
                            {
                                //Create text on header
                                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                                hText.Text.Value = column.Label;
                                hText.HorAlignment = StiTextHorAlignment.Right;
                                hText.VertAlignment = StiVertAlignment.Center;
                                hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                                hText.Border.Side = StiBorderSides.All;
                                hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                hText.Border.Style = StiPenStyle.None;
                                hText.TextBrush = new StiSolidBrush(Color.White);
                                hText.WordWrap = true;
                                hText.CanGrow = true;
                                headerBand.Components.Add(hText);

                                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                                dataText.Text.Value = "{CostOfSales.Column" + column.Index + "}";

                                if (column.Type != "Variance")
                                {
                                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }

                                dataText.HorAlignment = StiTextHorAlignment.Right;
                                dataText.VertAlignment = StiVertAlignment.Top;
                                dataText.Border.Style = StiPenStyle.None;
                                dataText.OnlyText = false;
                                dataText.Border.Side = StiBorderSides.All;
                                dataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                                dataText.WordWrap = true;
                                dataText.CanGrow = true;
                                dataText.Margins = new StiMargins(0, 1, 4, 0);
                                dataBand.Components.Add(dataText);

                                pos = pos + columnWidth;
                            }

                            //Create DataBand total
                            StiDataBand totalBand = new StiDataBand();
                            totalBand.DataSourceName = "CostOfSalesTotal";
                            totalBand.Name = $"CostOfSalesTotal{center.Item1}";
                            totalBand.Border.Style = StiPenStyle.None;
                            totalBand.Filters.Add(new StiFilter());
                            totalBand.Filters[0].Item = StiFilterItem.Expression;
                            totalBand.Filters[0].Expression = new StiExpression($"CostOfSalesTotal.Department == {center.Item1}");
                            page.Components.Add(totalBand);

                            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctTotalText.Text.Value = "Total Cost Of Sales";
                            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                            acctTotalText.VertAlignment = StiVertAlignment.Center;
                            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctTotalText.OnlyText = false;
                            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctTotalText.WordWrap = true;
                            acctTotalText.Margins = new StiMargins(0, 1, 0, 0);
                            totalBand.Components.Add(acctTotalText);

                            //Create DataBand Gross Profit
                            StiDataBand grossBand = new StiDataBand();
                            grossBand.DataSourceName = "GrossProfit";
                            grossBand.Name = $"GrossProfitBand{center.Item1}";
                            grossBand.Border.Style = StiPenStyle.None;
                            grossBand.Filters.Add(new StiFilter());
                            grossBand.Filters[0].Item = StiFilterItem.Expression;
                            grossBand.Filters[0].Expression = new StiExpression($"GrossProfit.Department == {center.Item1}");
                            page.Components.Add(grossBand);

                            StiText acctGrossText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                            acctGrossText.Text.Value = "Gross Profit";
                            acctGrossText.HorAlignment = StiTextHorAlignment.Left;
                            acctGrossText.VertAlignment = StiVertAlignment.Center;
                            acctGrossText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctGrossText.OnlyText = false;
                            acctGrossText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctGrossText.WordWrap = true;
                            acctGrossText.Margins = new StiMargins(0, 1, 0, 10);
                            grossBand.Components.Add(acctGrossText);

                            // Gross Profit Percen
                            StiDataBand percenBand = new StiDataBand();
                            percenBand.DataSourceName = "GrossProfitPercen";
                            percenBand.Name = $"GrossProfitPercenBand{center.Item1}";
                            percenBand.Border.Style = StiPenStyle.None;
                            percenBand.Filters.Add(new StiFilter());
                            percenBand.Filters[0].Item = StiFilterItem.Expression;
                            percenBand.Filters[0].Expression = new StiExpression($"GrossProfitPercen.Department == {center.Item1}");
                            page.Components.Add(percenBand);

                            StiText acctPercenText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctPercenText.Text.Value = "Gross Profit %";
                            acctPercenText.HorAlignment = StiTextHorAlignment.Left;
                            acctPercenText.VertAlignment = StiVertAlignment.Center;
                            acctPercenText.OnlyText = false;
                            acctPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctPercenText.WordWrap = true;
                            acctPercenText.Margins = new StiMargins(0, 1, 0, 10);
                            percenBand.Components.Add(acctPercenText);

                            pos = (columnWidth * 2);
                            foreach (var column in objColumns)
                            {
                                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                totalText.Text.Value = "{CostOfSalesTotal.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                totalText.HorAlignment = StiTextHorAlignment.Right;
                                totalText.VertAlignment = StiVertAlignment.Center;
                                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                totalText.OnlyText = false;
                                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                totalText.WordWrap = true;
                                totalText.Margins = new StiMargins(0, 1, 0, 0);
                                totalBand.Components.Add(totalText);

                                StiText grossText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                                grossText.Text.Value = "{GrossProfit.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    grossText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    grossText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                grossText.Type = StiSystemTextType.Expression;
                                grossText.HorAlignment = StiTextHorAlignment.Right;
                                grossText.VertAlignment = StiVertAlignment.Center;
                                grossText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                grossText.OnlyText = false;
                                grossText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                grossText.WordWrap = true;
                                grossText.Margins = new StiMargins(0, 1, 0, 10);
                                grossBand.Components.Add(grossText);

                                StiText depPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                if (column.Type != "Variance")
                                {
                                    depPercenText.Text.Value = "{GrossProfitPercen.Column" + column.Index + "}";
                                }
                                depPercenText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " "); ;
                                depPercenText.HorAlignment = StiTextHorAlignment.Right;
                                depPercenText.VertAlignment = StiVertAlignment.Center;
                                depPercenText.OnlyText = false;
                                depPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                depPercenText.WordWrap = true;
                                depPercenText.Margins = new StiMargins(0, 1, 0, 10);
                                percenBand.Components.Add(depPercenText);

                                pos = pos + columnWidth;
                            }
                        }

                        // Expenses
                        if (objColumns.Count > 0)
                        {
                            //Create HeaderBand
                            StiHeaderBand headerBand = new StiHeaderBand();
                            headerBand.Height = 0.25;
                            headerBand.Name = $"ExpensesHeaderBand{center.Item1}";
                            headerBand.Border.Style = StiPenStyle.None;
                            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            headerBand.PrintOnAllPages = false;
                            headerBand.PrintIfEmpty = true;
                            page.Components.Add(headerBand);

                            //Create Databand
                            StiDataBand dataBand = new StiDataBand();
                            dataBand.DataSourceName = "Expenses";
                            dataBand.Name = $"ExpensesData{center.Item1}";
                            dataBand.Border.Style = StiPenStyle.None;
                            dataBand.Filters.Add(new StiFilter());
                            dataBand.Filters[0].Item = StiFilterItem.Expression;
                            dataBand.Filters[0].Expression = new StiExpression($"Expenses.Department == {center.Item1}");
                            page.Components.Add(dataBand);

                            //Create DataBand item
                            StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctText.Text.Value = "Expenses";
                            acctText.HorAlignment = StiTextHorAlignment.Left;
                            acctText.VertAlignment = StiVertAlignment.Center;
                            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            acctText.Border.Side = StiBorderSides.All;
                            acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctText.Border.Style = StiPenStyle.None;
                            acctText.TextBrush = new StiSolidBrush(Color.White);
                            acctText.WordWrap = true;
                            headerBand.Components.Add(acctText);

                            StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.2));
                            acctDataText.Text.Value = "{Expenses.Sub}";
                            acctDataText.HorAlignment = StiTextHorAlignment.Left;
                            acctDataText.VertAlignment = StiVertAlignment.Center;
                            acctDataText.Border.Style = StiPenStyle.None;
                            acctDataText.OnlyText = false;
                            acctDataText.Border.Side = StiBorderSides.All;
                            acctDataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                            acctDataText.WordWrap = true;
                            acctDataText.CanGrow = true;
                            acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(acctDataText);

                            pos = (columnWidth * 2);

                            foreach (var column in objColumns)
                            {
                                //Create text on header
                                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                                hText.Text.Value = column.Label;
                                hText.HorAlignment = StiTextHorAlignment.Right;
                                hText.VertAlignment = StiVertAlignment.Center;
                                hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                                hText.Border.Side = StiBorderSides.All;
                                hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                hText.Border.Style = StiPenStyle.None;
                                hText.TextBrush = new StiSolidBrush(Color.White);
                                hText.WordWrap = true;
                                hText.CanGrow = true;
                                headerBand.Components.Add(hText);

                                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                                dataText.Text.Value = "{Expenses.Column" + column.Index + "}";

                                if (column.Type != "Variance")
                                {
                                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }

                                dataText.HorAlignment = StiTextHorAlignment.Right;
                                dataText.VertAlignment = StiVertAlignment.Top;
                                dataText.Border.Style = StiPenStyle.None;
                                dataText.OnlyText = false;
                                dataText.Border.Side = StiBorderSides.All;
                                dataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                                dataText.WordWrap = true;
                                dataText.CanGrow = true;
                                dataText.Margins = new StiMargins(0, 1, 4, 0);
                                dataBand.Components.Add(dataText);

                                pos = pos + columnWidth;
                            }

                            //Create DataBand total
                            StiDataBand totalBand = new StiDataBand();
                            totalBand.DataSourceName = "ExpensesTotal";
                            totalBand.Name = $"ExpensesTotal{center.Item1}";
                            totalBand.Border.Style = StiPenStyle.None;
                            totalBand.Filters.Add(new StiFilter());
                            totalBand.Filters[0].Item = StiFilterItem.Expression;
                            totalBand.Filters[0].Expression = new StiExpression($"ExpensesTotal.Department == {center.Item1}");
                            page.Components.Add(totalBand);

                            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctTotalText.Text.Value = "Total Expenses";
                            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                            acctTotalText.VertAlignment = StiVertAlignment.Center;
                            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctTotalText.OnlyText = false;
                            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctTotalText.WordWrap = true;
                            acctTotalText.Margins = new StiMargins(0, 1, 0, 0);
                            totalBand.Components.Add(acctTotalText);

                            //Create DataBand Net Profit
                            StiDataBand netBand = new StiDataBand();
                            netBand.DataSourceName = "NetProfit";
                            netBand.Name = $"NetProfit{center.Item1}";
                            netBand.Border.Style = StiPenStyle.None;
                            netBand.Filters.Add(new StiFilter());
                            netBand.Filters[0].Item = StiFilterItem.Expression;
                            netBand.Filters[0].Expression = new StiExpression($"NetProfit.Department == {center.Item1}");
                            page.Components.Add(netBand);

                            StiText acctNetText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                            acctNetText.Text.Value = netProfitText;
                            acctNetText.HorAlignment = StiTextHorAlignment.Left;
                            acctNetText.VertAlignment = StiVertAlignment.Center;
                            acctNetText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctNetText.OnlyText = false;
                            acctNetText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctNetText.WordWrap = true;
                            acctNetText.Margins = new StiMargins(0, 1, 0, 10);
                            netBand.Components.Add(acctNetText);

                            // Net Profit Percen
                            StiDataBand percenBand = new StiDataBand();
                            percenBand.DataSourceName = "NetProfitPercen";
                            percenBand.Name = $"NetProfitPercenBand{center.Item1}";
                            percenBand.Border.Style = StiPenStyle.None;
                            percenBand.NewPageAfter = !_includeProvisions;
                            percenBand.Filters.Add(new StiFilter());
                            percenBand.Filters[0].Item = StiFilterItem.Expression;
                            percenBand.Filters[0].Expression = new StiExpression($"NetProfitPercen.Department == {center.Item1}");
                            page.Components.Add(percenBand);

                            StiText acctPercenText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctPercenText.Text.Value = $"{netProfitText} %";
                            acctPercenText.HorAlignment = StiTextHorAlignment.Left;
                            acctPercenText.VertAlignment = StiVertAlignment.Center;
                            acctPercenText.OnlyText = false;
                            acctPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctPercenText.WordWrap = true;
                            acctPercenText.Margins = new StiMargins(0, 1, 0, 10);
                            percenBand.Components.Add(acctPercenText);

                            pos = (columnWidth * 2);
                            foreach (var column in objColumns)
                            {
                                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                totalText.Text.Value = "{ExpensesTotal.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                totalText.HorAlignment = StiTextHorAlignment.Right;
                                totalText.VertAlignment = StiVertAlignment.Center;
                                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                totalText.OnlyText = false;
                                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                totalText.WordWrap = true;
                                totalText.Margins = new StiMargins(0, 1, 0, 0);
                                totalBand.Components.Add(totalText);

                                StiText netText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                                netText.Text.Value = "{NetProfit.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    netText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    netText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                netText.Type = StiSystemTextType.Expression;
                                netText.HorAlignment = StiTextHorAlignment.Right;
                                netText.VertAlignment = StiVertAlignment.Center;
                                netText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                netText.OnlyText = false;
                                netText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                netText.WordWrap = true;
                                netText.Margins = new StiMargins(0, 1, 0, 10);
                                netBand.Components.Add(netText);

                                StiText depPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                if (column.Type != "Variance")
                                {
                                    depPercenText.Text.Value = "{NetProfitPercen.Column" + column.Index + "}";
                                }
                                depPercenText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " "); ;
                                depPercenText.HorAlignment = StiTextHorAlignment.Right;
                                depPercenText.VertAlignment = StiVertAlignment.Center;
                                depPercenText.OnlyText = false;
                                depPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                depPercenText.WordWrap = true;
                                depPercenText.Margins = new StiMargins(0, 1, 0, 10);
                                percenBand.Components.Add(depPercenText);

                                pos = pos + columnWidth;
                            }
                        }

                        // Other Income (Expenses)
                        if (objColumns.Count > 0)
                        {
                            //Create HeaderBand
                            StiHeaderBand headerBand = new StiHeaderBand();
                            headerBand.Height = 0.25;
                            headerBand.Name = $"OtherIncomeHeaderBand{center.Item1}";
                            headerBand.Border.Style = StiPenStyle.None;
                            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            headerBand.PrintOnAllPages = false;
                            headerBand.PrintIfEmpty = false;
                            page.Components.Add(headerBand);

                            //Create Databand
                            StiDataBand dataBand = new StiDataBand();
                            dataBand.DataSourceName = "OtherIncome";
                            dataBand.Name = $"OtherIncomeData{center.Item1}";
                            dataBand.Border.Style = StiPenStyle.None;
                            dataBand.Filters.Add(new StiFilter());
                            dataBand.Filters[0].Item = StiFilterItem.Expression;
                            dataBand.Filters[0].Expression = new StiExpression($"OtherIncome.Department == {center.Item1}");
                            page.Components.Add(dataBand);

                            //Create DataBand item
                            StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctText.Text.Value = "Other Income (Expenses)";
                            acctText.HorAlignment = StiTextHorAlignment.Left;
                            acctText.VertAlignment = StiVertAlignment.Center;
                            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            acctText.Border.Side = StiBorderSides.All;
                            acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctText.Border.Style = StiPenStyle.None;
                            acctText.TextBrush = new StiSolidBrush(Color.White);
                            acctText.WordWrap = true;
                            headerBand.Components.Add(acctText);

                            StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.2));
                            acctDataText.Text.Value = "{OtherIncome.Sub}";
                            acctDataText.HorAlignment = StiTextHorAlignment.Left;
                            acctDataText.VertAlignment = StiVertAlignment.Center;
                            acctDataText.Border.Style = StiPenStyle.None;
                            acctDataText.OnlyText = false;
                            acctDataText.Border.Side = StiBorderSides.All;
                            acctDataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                            acctDataText.WordWrap = true;
                            acctDataText.CanGrow = true;
                            acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(acctDataText);

                            pos = (columnWidth * 2);

                            foreach (var column in objColumns)
                            {
                                //Create text on header
                                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                                hText.Text.Value = column.Label;
                                hText.HorAlignment = StiTextHorAlignment.Right;
                                hText.VertAlignment = StiVertAlignment.Center;
                                hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                                hText.Border.Side = StiBorderSides.All;
                                hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                hText.Border.Style = StiPenStyle.None;
                                hText.TextBrush = new StiSolidBrush(Color.White);
                                hText.WordWrap = true;
                                hText.CanGrow = true;
                                headerBand.Components.Add(hText);

                                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                                dataText.Text.Value = "{OtherIncome.Column" + column.Index + "}";

                                if (column.Type != "Variance")
                                {
                                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }

                                dataText.HorAlignment = StiTextHorAlignment.Right;
                                dataText.VertAlignment = StiVertAlignment.Top;
                                dataText.Border.Style = StiPenStyle.None;
                                dataText.OnlyText = false;
                                dataText.Border.Side = StiBorderSides.All;
                                dataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                                dataText.WordWrap = true;
                                dataText.CanGrow = true;
                                dataText.Margins = new StiMargins(0, 1, 4, 0);
                                dataBand.Components.Add(dataText);

                                pos = pos + columnWidth;
                            }

                            //Create DataBand total
                            StiDataBand totalBand = new StiDataBand();
                            totalBand.DataSourceName = "OtherIncomeTotal";
                            totalBand.Name = $"OtherIncomeTotal{center.Item1}";
                            totalBand.Border.Style = StiPenStyle.None;
                            totalBand.Filters.Add(new StiFilter());
                            totalBand.Filters[0].Item = StiFilterItem.Expression;
                            totalBand.Filters[0].Expression = new StiExpression($"OtherIncomeTotal.Department == {center.Item1}");
                            page.Components.Add(totalBand);

                            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctTotalText.Text.Value = "Total Other Income (Expenses)";
                            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                            acctTotalText.VertAlignment = StiVertAlignment.Center;
                            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctTotalText.OnlyText = false;
                            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctTotalText.WordWrap = true;
                            acctTotalText.Margins = new StiMargins(0, 1, 0, 0);
                            totalBand.Components.Add(acctTotalText);

                            //Create DataBand Income Before Provisions For Income Taxes
                            StiDataBand grossBand = new StiDataBand();
                            grossBand.DataSourceName = "BeforeProvisions";
                            grossBand.Name = $"BeforeProvisionsBand{center.Item1}";
                            grossBand.Border.Style = StiPenStyle.None;
                            grossBand.Filters.Add(new StiFilter());
                            grossBand.Filters[0].Item = StiFilterItem.Expression;
                            grossBand.Filters[0].Expression = new StiExpression($"BeforeProvisions.Department == {center.Item1}");
                            page.Components.Add(grossBand);

                            StiText acctGrossText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                            acctGrossText.Text.Value = "Income Before Provisions For Income Taxes";
                            acctGrossText.HorAlignment = StiTextHorAlignment.Left;
                            acctGrossText.VertAlignment = StiVertAlignment.Center;
                            acctGrossText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctGrossText.OnlyText = false;
                            acctGrossText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctGrossText.WordWrap = true;
                            acctGrossText.Margins = new StiMargins(0, 1, 0, 10);
                            grossBand.Components.Add(acctGrossText);

                            // Income Before Provisions For Income Taxes Percen
                            StiDataBand percenBand = new StiDataBand();
                            percenBand.DataSourceName = "BeforeProvisionsPercen";
                            percenBand.Name = $"BeforeProvisionsPercenBand{center.Item1}";
                            percenBand.Border.Style = StiPenStyle.None;
                            percenBand.Filters.Add(new StiFilter());
                            percenBand.Filters[0].Item = StiFilterItem.Expression;
                            percenBand.Filters[0].Expression = new StiExpression($"BeforeProvisionsPercen.Department == {center.Item1}");
                            page.Components.Add(percenBand);

                            StiText acctPercenText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctPercenText.Text.Value = "Income Before Provisions For Income Taxes %";
                            acctPercenText.HorAlignment = StiTextHorAlignment.Left;
                            acctPercenText.VertAlignment = StiVertAlignment.Center;
                            acctPercenText.OnlyText = false;
                            acctPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctPercenText.WordWrap = true;
                            acctPercenText.Margins = new StiMargins(0, 1, 0, 10);
                            percenBand.Components.Add(acctPercenText);

                            pos = (columnWidth * 2);
                            foreach (var column in objColumns)
                            {
                                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                totalText.Text.Value = "{OtherIncomeTotal.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                totalText.HorAlignment = StiTextHorAlignment.Right;
                                totalText.VertAlignment = StiVertAlignment.Center;
                                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                totalText.OnlyText = false;
                                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                totalText.WordWrap = true;
                                totalText.Margins = new StiMargins(0, 1, 0, 0);
                                totalBand.Components.Add(totalText);

                                StiText grossText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                                grossText.Text.Value = "{BeforeProvisions.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    grossText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    grossText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                grossText.Type = StiSystemTextType.Expression;
                                grossText.HorAlignment = StiTextHorAlignment.Right;
                                grossText.VertAlignment = StiVertAlignment.Center;
                                grossText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                grossText.OnlyText = false;
                                grossText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                grossText.WordWrap = true;
                                grossText.Margins = new StiMargins(0, 1, 0, 10);
                                grossBand.Components.Add(grossText);

                                StiText depPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                if (column.Type != "Variance")
                                {
                                    depPercenText.Text.Value = "{BeforeProvisionsPercen.Column" + column.Index + "}";
                                }
                                depPercenText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " "); ;
                                depPercenText.HorAlignment = StiTextHorAlignment.Right;
                                depPercenText.VertAlignment = StiVertAlignment.Center;
                                depPercenText.OnlyText = false;
                                depPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                depPercenText.WordWrap = true;
                                depPercenText.Margins = new StiMargins(0, 1, 0, 10);
                                percenBand.Components.Add(depPercenText);

                                pos = pos + columnWidth;
                            }
                        }

                        // Provisions for Income Taxes
                        if (objColumns.Count > 0)
                        {
                            //Create HeaderBand
                            StiHeaderBand headerBand = new StiHeaderBand();
                            headerBand.Height = 0.25;
                            headerBand.Name = $"IncomeTaxesHeaderBand{center.Item1}";
                            headerBand.Border.Style = StiPenStyle.None;
                            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            headerBand.PrintOnAllPages = false;
                            headerBand.PrintIfEmpty = false;
                            page.Components.Add(headerBand);

                            //Create Databand
                            StiDataBand dataBand = new StiDataBand();
                            dataBand.DataSourceName = "IncomeTaxes";
                            dataBand.Name = $"IncomeTaxesData{center.Item1}";
                            dataBand.Border.Style = StiPenStyle.None;
                            dataBand.Filters.Add(new StiFilter());
                            dataBand.Filters[0].Item = StiFilterItem.Expression;
                            dataBand.Filters[0].Expression = new StiExpression($"IncomeTaxes.Department == {center.Item1}");
                            page.Components.Add(dataBand);

                            //Create DataBand item
                            StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctText.Text.Value = "Provisions for Income Taxes";
                            acctText.HorAlignment = StiTextHorAlignment.Left;
                            acctText.VertAlignment = StiVertAlignment.Center;
                            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                            acctText.Border.Side = StiBorderSides.All;
                            acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctText.Border.Style = StiPenStyle.None;
                            acctText.TextBrush = new StiSolidBrush(Color.White);
                            acctText.WordWrap = true;
                            headerBand.Components.Add(acctText);

                            StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.2));
                            acctDataText.Text.Value = "{IncomeTaxes.Sub}";
                            acctDataText.HorAlignment = StiTextHorAlignment.Left;
                            acctDataText.VertAlignment = StiVertAlignment.Center;
                            acctDataText.Border.Style = StiPenStyle.None;
                            acctDataText.OnlyText = false;
                            acctDataText.Border.Side = StiBorderSides.All;
                            acctDataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                            acctDataText.WordWrap = true;
                            acctDataText.CanGrow = true;
                            acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                            dataBand.Components.Add(acctDataText);

                            pos = (columnWidth * 2);

                            foreach (var column in objColumns)
                            {
                                //Create text on header
                                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                                hText.Text.Value = column.Label;
                                hText.HorAlignment = StiTextHorAlignment.Right;
                                hText.VertAlignment = StiVertAlignment.Center;
                                hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                                hText.Border.Side = StiBorderSides.All;
                                hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                hText.Border.Style = StiPenStyle.None;
                                hText.TextBrush = new StiSolidBrush(Color.White);
                                hText.WordWrap = true;
                                hText.CanGrow = true;
                                headerBand.Components.Add(hText);

                                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                                dataText.Text.Value = "{IncomeTaxes.Column" + column.Index + "}";

                                if (column.Type != "Variance")
                                {
                                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }

                                dataText.HorAlignment = StiTextHorAlignment.Right;
                                dataText.VertAlignment = StiVertAlignment.Top;
                                dataText.Border.Style = StiPenStyle.None;
                                dataText.OnlyText = false;
                                dataText.Border.Side = StiBorderSides.All;
                                dataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                                dataText.WordWrap = true;
                                dataText.CanGrow = true;
                                dataText.Margins = new StiMargins(0, 1, 4, 0);
                                dataBand.Components.Add(dataText);

                                pos = pos + columnWidth;
                            }

                            //Create DataBand total
                            StiDataBand totalBand = new StiDataBand();
                            totalBand.DataSourceName = "IncomeTaxesTotal";
                            totalBand.Name = $"IncomeTaxesTotal{center.Item1}";
                            totalBand.Border.Style = StiPenStyle.None;
                            totalBand.Filters.Add(new StiFilter());
                            totalBand.Filters[0].Item = StiFilterItem.Expression;
                            totalBand.Filters[0].Expression = new StiExpression($"IncomeTaxesTotal.Department == {center.Item1}");
                            page.Components.Add(totalBand);

                            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctTotalText.Text.Value = "Total Provisions for Income Taxes";
                            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                            acctTotalText.VertAlignment = StiVertAlignment.Center;
                            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctTotalText.OnlyText = false;
                            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctTotalText.WordWrap = true;
                            acctTotalText.Margins = new StiMargins(0, 1, 0, 0);
                            totalBand.Components.Add(acctTotalText);

                            //Create DataBand Net Income
                            StiDataBand netBand = new StiDataBand();
                            netBand.DataSourceName = "NetIncome";
                            netBand.Name = $"NetIncome{center.Item1}";
                            netBand.Border.Style = StiPenStyle.None;
                            netBand.Filters.Add(new StiFilter());
                            netBand.Filters[0].Item = StiFilterItem.Expression;
                            netBand.Filters[0].Expression = new StiExpression($"NetIncome.Department == {center.Item1}");
                            page.Components.Add(netBand);

                            StiText acctNetText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                            acctNetText.Text.Value = "Net Income";
                            acctNetText.HorAlignment = StiTextHorAlignment.Left;
                            acctNetText.VertAlignment = StiVertAlignment.Center;
                            acctNetText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            acctNetText.OnlyText = false;
                            acctNetText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctNetText.WordWrap = true;
                            acctNetText.Margins = new StiMargins(0, 1, 0, 10);
                            netBand.Components.Add(acctNetText);

                            // Net Income Percen
                            StiDataBand percenBand = new StiDataBand();
                            percenBand.DataSourceName = "NetIncomePercen";
                            percenBand.Name = $"NetIncomePercenBand{center.Item1}";
                            percenBand.Border.Style = StiPenStyle.None;
                            percenBand.NewPageAfter = true;
                            percenBand.Filters.Add(new StiFilter());
                            percenBand.Filters[0].Item = StiFilterItem.Expression;
                            percenBand.Filters[0].Expression = new StiExpression($"NetIncomePercen.Department == {center.Item1}");
                            page.Components.Add(percenBand);

                            StiText acctPercenText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                            acctPercenText.Text.Value = "Net Income %";
                            acctPercenText.HorAlignment = StiTextHorAlignment.Left;
                            acctPercenText.VertAlignment = StiVertAlignment.Center;
                            acctPercenText.OnlyText = false;
                            acctPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                            acctPercenText.WordWrap = true;
                            acctPercenText.Margins = new StiMargins(0, 1, 0, 10);
                            percenBand.Components.Add(acctPercenText);

                            pos = (columnWidth * 2);
                            foreach (var column in objColumns)
                            {
                                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                totalText.Text.Value = "{IncomeTaxesTotal.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                totalText.HorAlignment = StiTextHorAlignment.Right;
                                totalText.VertAlignment = StiVertAlignment.Center;
                                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                totalText.OnlyText = false;
                                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                totalText.WordWrap = true;
                                totalText.Margins = new StiMargins(0, 1, 0, 0);
                                totalBand.Components.Add(totalText);

                                StiText netText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                                netText.Text.Value = "{NetIncome.Column" + column.Index + "}";
                                if (column.Type != "Variance")
                                {
                                    netText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                                }
                                else
                                {
                                    netText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                                }
                                netText.Type = StiSystemTextType.Expression;
                                netText.HorAlignment = StiTextHorAlignment.Right;
                                netText.VertAlignment = StiVertAlignment.Center;
                                netText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                netText.OnlyText = false;
                                netText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                netText.WordWrap = true;
                                netText.Margins = new StiMargins(0, 1, 0, 10);
                                netBand.Components.Add(netText);

                                StiText depPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                if (column.Type != "Variance")
                                {
                                    depPercenText.Text.Value = "{NetIncomePercen.Column" + column.Index + "}";
                                }
                                depPercenText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " "); ;
                                depPercenText.HorAlignment = StiTextHorAlignment.Right;
                                depPercenText.VertAlignment = StiVertAlignment.Center;
                                depPercenText.OnlyText = false;
                                depPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                                depPercenText.WordWrap = true;
                                depPercenText.Margins = new StiMargins(0, 1, 0, 10);
                                percenBand.Components.Add(depPercenText);

                                pos = pos + columnWidth;
                            }
                        }
                    }
                }
            }

            // Build Total Summary
            BuildAllDepartmentTotal(page, objComparative);

            // Build Department Summary
            if (departments.Count() > 0)
            {
                DepartmentSummaryDataSource(report, objComparative, departments);
            }

            if (deptArray.Length > 1)
            {
                BuildDepartmentSummary(page, objComparative, departments);
            }

            DataSet dsC = new DataSet();

            var connString = Session["config"].ToString();
            objPropUser.ConnConfig = connString;
            dsC = objBL_User.getControl(objPropUser);

            DataTable cTable = BuildCompanyDetailsTable();
            var cRow = cTable.NewRow();

            DataSet companyInfo = new DataSet();
            companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());

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

            report.RegData("CompanyDetails", cTable);
            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();
            report.Dictionary.Variables["ReportName"].Value = Session["ReportName"].ToString();

            dView.RowFilter = "Type = 3";
            DataTable Revenues = dView.ToTable();

            dView.RowFilter = "Type = 4";
            DataTable CostOfSales = dView.ToTable();

            dView.RowFilter = "Type = 5";
            DataTable Expenses = dView.ToTable();

            dView.RowFilter = "Type = 8";
            DataTable OtherIncome = dView.ToTable();

            dView.RowFilter = "Type = 9";
            DataTable IncomeTaxes = dView.ToTable();

            report.RegData("Revenues", Revenues);
            report.RegData("CostOfSales", CostOfSales);
            report.RegData("Expenses", Expenses);
            report.RegData("OtherIncome", OtherIncome);
            report.RegData("IncomeTaxes", IncomeTaxes);
            report.RegData("RevenuesTotal", _revenuesTotal);
            report.RegData("CostOfSalesTotal", _costOfSalesTotal);
            report.RegData("ExpensesTotal", _expensesTotal);
            report.RegData("OtherIncomeTotal", _otherIncomeTotal);
            report.RegData("IncomeTaxesTotal", _incomeTaxesTotal);
            report.RegData("GrossProfit", _grossProfit);
            report.RegData("GrossProfitPercen", _grossProfitPercen);
            report.RegData("NetProfit", _netProfit);
            report.RegData("NetProfitPercen", _netProfitPercen);
            report.RegData("BeforeProvisions", _beforeProvisions);
            report.RegData("BeforeProvisionsPercen", _beforeProvisionsPercen);
            report.RegData("NetIncome", _netIncome);
            report.RegData("NetIncomePercen", _netIncomePercen);
            report.RegData("SummaryTotal", SummaryTotal(data, objComparative));

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

    private void BuildAllDepartmentTotal(StiPage page, List<ComparativeStatementRequest> objColumns, bool isWithSub)
    {
        StiFormatService currencyFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
        StiFormatService percenFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");

        var netProfitText = "Income From Operations";

        StiHeaderBand centerTitle = new StiHeaderBand();
        centerTitle.Height = 0.5;
        centerTitle.PrintIfEmpty = true;
        page.Components.Add(centerTitle);

        StiText centerTitleText = new StiText(new RectangleD(0, 0.1, page.Width, 0.25));
        centerTitleText.Text.Value = $"{Session["ReportName"].ToString()} Totals";
        centerTitleText.HorAlignment = StiTextHorAlignment.Center;
        centerTitleText.VertAlignment = StiVertAlignment.Center;
        centerTitleText.Font = new Font("Arial", 18F, FontStyle.Bold);
        centerTitleText.WordWrap = true;
        centerTitle.Components.Add(centerTitleText);

        var columnCount = objColumns.Count + 2;
        double columnWidth = page.Width / columnCount;
        double pos = 0;

        // Revenues
        if (objColumns.Count > 0)
        {
            //Create HeaderBand
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.25;
            headerBand.Name = $"RevenuesHeaderBandSummary";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerBand.PrintOnAllPages = false;
            headerBand.PrintIfEmpty = true;
            page.Components.Add(headerBand);

            StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctText.Text.Value = "Revenues";
            acctText.HorAlignment = StiTextHorAlignment.Left;
            acctText.VertAlignment = StiVertAlignment.Center;
            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            acctText.Border.Side = StiBorderSides.All;
            acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctText.Border.Style = StiPenStyle.None;
            acctText.TextBrush = new StiSolidBrush(Color.White);
            acctText.WordWrap = true;
            headerBand.Components.Add(acctText);

            //Create group header band
            if (isWithSub)
            {
                StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                groupHeader.Name = "RevenuesGroupHeaderBandSummary";
                groupHeader.PrintOnAllPages = false;
                groupHeader.Condition = new StiGroupConditionExpression("{Revenues.Sub}");
                page.Components.Add(groupHeader);

                StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                groupHeaderText.Text.Value = "{Revenues.Sub}";
                groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                groupHeaderText.VertAlignment = StiVertAlignment.Center;
                groupHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
                groupHeaderText.Border.Style = StiPenStyle.None;
                groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                groupHeaderText.WordWrap = true;
                groupHeader.Components.Add(groupHeaderText);
            }

            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "Revenues";
            dataBand.Name = $"RevenuesDataSummary";
            dataBand.Border.Style = StiPenStyle.None;
            page.Components.Add(dataBand);

            //Create DataBand item
            StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.2));
            acctDataText.Text.Value = "{Revenues.fDesc}";
            acctDataText.HorAlignment = StiTextHorAlignment.Left;
            acctDataText.VertAlignment = StiVertAlignment.Center;
            acctDataText.Border.Style = StiPenStyle.None;
            acctDataText.OnlyText = false;
            acctDataText.Border.Side = StiBorderSides.All;
            acctDataText.Font = new Font("Arial", 8F);
            acctDataText.WordWrap = true;
            acctDataText.CanGrow = true;
            acctDataText.Margins = new StiMargins(0, 1, 4, 0);
            dataBand.Components.Add(acctDataText);

            pos = (columnWidth * 2);

            foreach (var column in objColumns)
            {
                //Create text on header
                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                hText.Text.Value = column.Label;
                hText.HorAlignment = StiTextHorAlignment.Right;
                hText.VertAlignment = StiVertAlignment.Center;
                hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                hText.Border.Side = StiBorderSides.All;
                hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                hText.Border.Style = StiPenStyle.None;
                hText.TextBrush = new StiSolidBrush(Color.White);
                hText.WordWrap = true;
                hText.CanGrow = true;
                headerBand.Components.Add(hText);

                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                dataText.Text.Value = "{Revenues.Column" + column.Index + "}";

                if (column.Type != "Variance")
                {
                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }

                if (column.Type == "Actual")
                {
                    dataText.Interaction.Hyperlink = new StiHyperlinkExpression("{Revenues.Column" + column.Index + "URL}");
                }

                dataText.HorAlignment = StiTextHorAlignment.Right;
                dataText.VertAlignment = StiVertAlignment.Top;
                dataText.Border.Style = StiPenStyle.None;
                dataText.OnlyText = false;
                dataText.Border.Side = StiBorderSides.All;
                dataText.Font = new Font("Arial", 8F);
                dataText.WordWrap = true;
                dataText.CanGrow = true;
                dataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(dataText);

                pos = pos + columnWidth;
            }

            //Create group footer band
            if (isWithSub)
            {
                StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.4));
                groupFooter.Name = $"RevenuesGroupFooterBandSummary";
                page.Components.Add(groupFooter);

                StiText subTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                subTotalText.Text.Value = "Total {Revenues.Sub}";
                subTotalText.HorAlignment = StiTextHorAlignment.Left;
                subTotalText.VertAlignment = StiVertAlignment.Center;
                subTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                subTotalText.Border.Style = StiPenStyle.None;
                subTotalText.TextBrush = new StiSolidBrush(Color.Black);
                subTotalText.WordWrap = true;
                groupFooter.Components.Add(subTotalText);

                pos = (columnWidth * 2);
                foreach (var column in objColumns)
                {
                    StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    totalText.HorAlignment = StiTextHorAlignment.Right;
                    totalText.VertAlignment = StiVertAlignment.Center;
                    totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    totalText.TextBrush = new StiSolidBrush(Color.Black);
                    totalText.WordWrap = true;

                    if (column.Type != "Variance")
                    {
                        totalText.Text.Value = "{Sum(Revenues.Column" + column.Index + ")}";
                        totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        totalText.Text.Value = "{(Sum(Revenues.Column" + column.Column1 + ") - Sum(Revenues.Column" + column.Column2 + ")) / Abs(Sum(Revenues.Column" + column.Column2 + "))}";
                        totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    groupFooter.Components.Add(totalText);

                    pos = pos + columnWidth;
                }
            }

            //Create DataBand total
            StiDataBand totalBand = new StiDataBand();
            totalBand.DataSourceName = "SummaryTotal";
            totalBand.Name = $"RevenuesTotalSummary";
            totalBand.Border.Style = StiPenStyle.None;
            totalBand.Filters.Add(new StiFilter());
            totalBand.Filters[0].Item = StiFilterItem.Expression;
            totalBand.Filters[0].Expression = new StiExpression("SummaryTotal.Type == 3");
            page.Components.Add(totalBand);

            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
            acctTotalText.Text.Value = "Total Revenues";
            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
            acctTotalText.VertAlignment = StiVertAlignment.Center;
            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
            acctTotalText.OnlyText = false;
            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctTotalText.WordWrap = true;
            acctTotalText.Margins = new StiMargins(0, 1, 0, 10);
            totalBand.Components.Add(acctTotalText);

            pos = (columnWidth * 2);
            foreach (var column in objColumns)
            {
                StiText footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                footerText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                if (column.Type != "Variance")
                {
                    footerText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    footerText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }
                footerText.HorAlignment = StiTextHorAlignment.Right;
                footerText.VertAlignment = StiVertAlignment.Center;
                footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                footerText.OnlyText = false;
                footerText.Font = new Font("Arial", 9F, FontStyle.Bold);
                footerText.WordWrap = true;
                footerText.Margins = new StiMargins(0, 1, 0, 10);
                totalBand.Components.Add(footerText);

                pos = pos + columnWidth;
            }
        }

        // Cost Of Sales
        if (objColumns.Count > 0)
        {
            //Create HeaderBand
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.25;
            headerBand.Name = $"CostOfSalesHeaderBandSummary";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerBand.PrintOnAllPages = false;
            headerBand.PrintIfEmpty = true;
            page.Components.Add(headerBand);

            StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctText.Text.Value = "Cost Of Sales";
            acctText.HorAlignment = StiTextHorAlignment.Left;
            acctText.VertAlignment = StiVertAlignment.Center;
            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            acctText.Border.Side = StiBorderSides.All;
            acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctText.Border.Style = StiPenStyle.None;
            acctText.TextBrush = new StiSolidBrush(Color.White);
            acctText.WordWrap = true;
            headerBand.Components.Add(acctText);

            //Create group header band
            if (isWithSub)
            {
                StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                groupHeader.Name = "CostOfSalesGroupHeaderBandSummary";
                groupHeader.PrintOnAllPages = false;
                groupHeader.Condition = new StiGroupConditionExpression("{CostOfSales.Sub}");
                page.Components.Add(groupHeader);

                StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                groupHeaderText.Text.Value = "{CostOfSales.Sub}";
                groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                groupHeaderText.VertAlignment = StiVertAlignment.Center;
                groupHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
                groupHeaderText.Border.Style = StiPenStyle.None;
                groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                groupHeaderText.WordWrap = true;
                groupHeader.Components.Add(groupHeaderText);
            }

            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "CostOfSales";
            dataBand.Name = "CostOfSalesDataSummary";
            dataBand.Border.Style = StiPenStyle.None;
            page.Components.Add(dataBand);

            //Create DataBand item
            StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.2));
            acctDataText.Text.Value = "{CostOfSales.fDesc}";
            acctDataText.HorAlignment = StiTextHorAlignment.Left;
            acctDataText.VertAlignment = StiVertAlignment.Center;
            acctDataText.Border.Style = StiPenStyle.None;
            acctDataText.OnlyText = false;
            acctDataText.Border.Side = StiBorderSides.All;
            acctDataText.Font = new Font("Arial", 8F);
            acctDataText.WordWrap = true;
            acctDataText.CanGrow = true;
            acctDataText.Margins = new StiMargins(0, 1, 4, 0);
            dataBand.Components.Add(acctDataText);

            pos = (columnWidth * 2);

            foreach (var column in objColumns)
            {
                //Create text on header
                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                hText.Text.Value = column.Label;
                hText.HorAlignment = StiTextHorAlignment.Right;
                hText.VertAlignment = StiVertAlignment.Center;
                hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                hText.Border.Side = StiBorderSides.All;
                hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                hText.Border.Style = StiPenStyle.None;
                hText.TextBrush = new StiSolidBrush(Color.White);
                hText.WordWrap = true;
                hText.CanGrow = true;
                headerBand.Components.Add(hText);

                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                dataText.Text.Value = "{CostOfSales.Column" + column.Index + "}";

                if (column.Type != "Variance")
                {
                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }

                if (column.Type == "Actual")
                {
                    dataText.Interaction.Hyperlink = new StiHyperlinkExpression("{CostOfSales.Column" + column.Index + "URL}");
                }

                dataText.HorAlignment = StiTextHorAlignment.Right;
                dataText.VertAlignment = StiVertAlignment.Top;
                dataText.Border.Style = StiPenStyle.None;
                dataText.OnlyText = false;
                dataText.Border.Side = StiBorderSides.All;
                dataText.Font = new Font("Arial", 8F);
                dataText.WordWrap = true;
                dataText.CanGrow = true;
                dataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(dataText);

                pos = pos + columnWidth;
            }

            //Create group footer band
            if (isWithSub)
            {
                StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.4));
                groupFooter.Name = $"CostOfSalesGroupFooterBandSummary";
                page.Components.Add(groupFooter);

                StiText subTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                subTotalText.Text.Value = "Total {CostOfSales.Sub}";
                subTotalText.HorAlignment = StiTextHorAlignment.Left;
                subTotalText.VertAlignment = StiVertAlignment.Center;
                subTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                subTotalText.Border.Style = StiPenStyle.None;
                subTotalText.TextBrush = new StiSolidBrush(Color.Black);
                subTotalText.WordWrap = true;
                groupFooter.Components.Add(subTotalText);

                pos = (columnWidth * 2);
                foreach (var column in objColumns)
                {
                    StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    totalText.HorAlignment = StiTextHorAlignment.Right;
                    totalText.VertAlignment = StiVertAlignment.Center;
                    totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    totalText.TextBrush = new StiSolidBrush(Color.Black);
                    totalText.WordWrap = true;

                    if (column.Type != "Variance")
                    {
                        totalText.Text.Value = "{Sum(CostOfSales.Column" + column.Index + ")}";
                        totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        totalText.Text.Value = "{(Sum(CostOfSales.Column" + column.Column1 + ") - Sum(CostOfSales.Column" + column.Column2 + ")) / Abs(Sum(CostOfSales.Column" + column.Column2 + "))}";
                        totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    groupFooter.Components.Add(totalText);

                    pos = pos + columnWidth;
                }
            }

            //Create DataBand total
            StiDataBand totalBand = new StiDataBand();
            totalBand.DataSourceName = "SummaryTotal";
            totalBand.Name = "CostOfSalesTotalSummary";
            totalBand.Border.Style = StiPenStyle.None;
            totalBand.Filters.Add(new StiFilter());
            totalBand.Filters[0].Item = StiFilterItem.Expression;
            totalBand.Filters[0].Expression = new StiExpression("SummaryTotal.Type == 4");
            page.Components.Add(totalBand);

            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctTotalText.Text.Value = "Total Cost Of Sales";
            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
            acctTotalText.VertAlignment = StiVertAlignment.Center;
            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
            acctTotalText.OnlyText = false;
            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctTotalText.WordWrap = true;
            acctTotalText.Margins = new StiMargins(0, 1, 0, 0);
            totalBand.Components.Add(acctTotalText);

            //Create DataBand Gross Profit
            StiDataBand grossBand = new StiDataBand();
            grossBand.DataSourceName = "SummaryTotal";
            grossBand.Name = $"GrossProfitBandSummary";
            grossBand.Border.Style = StiPenStyle.None;
            grossBand.Filters.Add(new StiFilter());
            grossBand.Filters[0].Item = StiFilterItem.Expression;
            grossBand.Filters[0].Expression = new StiExpression("SummaryTotal.Type == 41");
            page.Components.Add(grossBand);

            StiText acctGrossText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
            acctGrossText.Text.Value = "Gross Profit";
            acctGrossText.HorAlignment = StiTextHorAlignment.Left;
            acctGrossText.VertAlignment = StiVertAlignment.Center;
            acctGrossText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
            acctGrossText.OnlyText = false;
            acctGrossText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctGrossText.WordWrap = true;
            acctGrossText.Margins = new StiMargins(0, 1, 0, 10);
            grossBand.Components.Add(acctGrossText);

            pos = (columnWidth * 2);
            foreach (var column in objColumns)
            {
                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                totalText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                if (column.Type != "Variance")
                {
                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }
                totalText.HorAlignment = StiTextHorAlignment.Right;
                totalText.VertAlignment = StiVertAlignment.Center;
                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                totalText.OnlyText = false;
                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                totalText.WordWrap = true;
                totalText.Margins = new StiMargins(0, 1, 0, 0);
                totalBand.Components.Add(totalText);

                StiText grossText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                grossText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                if (column.Type != "Variance")
                {
                    grossText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    grossText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }
                grossText.Type = StiSystemTextType.Expression;
                grossText.HorAlignment = StiTextHorAlignment.Right;
                grossText.VertAlignment = StiVertAlignment.Center;
                grossText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                grossText.OnlyText = false;
                grossText.Font = new Font("Arial", 9F, FontStyle.Bold);
                grossText.WordWrap = true;
                grossText.Margins = new StiMargins(0, 1, 0, 10);
                grossBand.Components.Add(grossText);

                pos = pos + columnWidth;
            }

            // Gross Profit Percen
            StiDataBand percenBand = new StiDataBand();
            percenBand.DataSourceName = "SummaryTotal";
            percenBand.Name = "GrossPercenSummary";
            percenBand.Border.Style = StiPenStyle.None;
            percenBand.Filters.Add(new StiFilter());
            percenBand.Filters[0].Item = StiFilterItem.Expression;
            percenBand.Filters[0].Expression = new StiExpression($"SummaryTotal.Type == 42");
            page.Components.Add(percenBand);

            StiText acctPercenText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctPercenText.Text.Value = "Gross Profit %";
            acctPercenText.HorAlignment = StiTextHorAlignment.Left;
            acctPercenText.VertAlignment = StiVertAlignment.Center;
            acctPercenText.OnlyText = false;
            acctPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctPercenText.WordWrap = true;
            acctPercenText.Margins = new StiMargins(0, 1, 0, 10);
            percenBand.Components.Add(acctPercenText);

            pos = (columnWidth * 2);
            foreach (var column in objColumns)
            {

                StiText depPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                if (column.Type == "Actual" || column.Type == "Difference")
                {
                    depPercenText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                }
                depPercenText.TextFormat = percenFormat;
                depPercenText.HorAlignment = StiTextHorAlignment.Right;
                depPercenText.VertAlignment = StiVertAlignment.Center;
                depPercenText.OnlyText = false;
                depPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                depPercenText.WordWrap = true;
                depPercenText.Margins = new StiMargins(0, 1, 0, 10);
                percenBand.Components.Add(depPercenText);

                pos = pos + columnWidth;
            }
        }

        // Expenses
        if (objColumns.Count > 0)
        {
            //Create HeaderBand
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.25;
            headerBand.Name = $"ExpensesHeaderBandSummary";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerBand.PrintOnAllPages = false;
            headerBand.PrintIfEmpty = true;
            page.Components.Add(headerBand);

            StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctText.Text.Value = "Expenses";
            acctText.HorAlignment = StiTextHorAlignment.Left;
            acctText.VertAlignment = StiVertAlignment.Center;
            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            acctText.Border.Side = StiBorderSides.All;
            acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctText.Border.Style = StiPenStyle.None;
            acctText.TextBrush = new StiSolidBrush(Color.White);
            acctText.WordWrap = true;
            headerBand.Components.Add(acctText);

            //Create group header band
            if (isWithSub)
            {
                StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                groupHeader.Name = "ExpensesGroupHeaderBandSummary";
                groupHeader.PrintOnAllPages = false;
                groupHeader.Condition = new StiGroupConditionExpression("{Expenses.Sub}");
                page.Components.Add(groupHeader);

                StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                groupHeaderText.Text.Value = "{Expenses.Sub}";
                groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                groupHeaderText.VertAlignment = StiVertAlignment.Center;
                groupHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
                groupHeaderText.Border.Style = StiPenStyle.None;
                groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                groupHeaderText.WordWrap = true;
                groupHeader.Components.Add(groupHeaderText);
            }

            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "Expenses";
            dataBand.Name = $"ExpensesDataSummary";
            dataBand.Border.Style = StiPenStyle.None;
            page.Components.Add(dataBand);

            //Create DataBand item

            StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.2));
            acctDataText.Text.Value = "{Expenses.fDesc}";
            acctDataText.HorAlignment = StiTextHorAlignment.Left;
            acctDataText.VertAlignment = StiVertAlignment.Center;
            acctDataText.Border.Style = StiPenStyle.None;
            acctDataText.OnlyText = false;
            acctDataText.Border.Side = StiBorderSides.All;
            acctDataText.Font = new Font("Arial", 8F);
            acctDataText.WordWrap = true;
            acctDataText.CanGrow = true;
            acctDataText.Margins = new StiMargins(0, 1, 4, 0);
            dataBand.Components.Add(acctDataText);

            pos = (columnWidth * 2);

            foreach (var column in objColumns)
            {
                //Create text on header
                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                hText.Text.Value = column.Label;
                hText.HorAlignment = StiTextHorAlignment.Right;
                hText.VertAlignment = StiVertAlignment.Center;
                hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                hText.Border.Side = StiBorderSides.All;
                hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                hText.Border.Style = StiPenStyle.None;
                hText.TextBrush = new StiSolidBrush(Color.White);
                hText.WordWrap = true;
                hText.CanGrow = true;
                headerBand.Components.Add(hText);

                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                dataText.Text.Value = "{Expenses.Column" + column.Index + "}";

                if (column.Type != "Variance")
                {
                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }

                if (column.Type == "Actual")
                {
                    dataText.Interaction.Hyperlink = new StiHyperlinkExpression("{Expenses.Column" + column.Index + "URL}");
                }

                dataText.HorAlignment = StiTextHorAlignment.Right;
                dataText.VertAlignment = StiVertAlignment.Top;
                dataText.Border.Style = StiPenStyle.None;
                dataText.OnlyText = false;
                dataText.Border.Side = StiBorderSides.All;
                dataText.Font = new Font("Arial", 8F);
                dataText.WordWrap = true;
                dataText.CanGrow = true;
                dataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(dataText);

                pos = pos + columnWidth;
            }

            //Create group footer band
            if (isWithSub)
            {
                StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.4));
                groupFooter.Name = $"ExpensesGroupFooterBandSummary";
                page.Components.Add(groupFooter);

                StiText subTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                subTotalText.Text.Value = "Total {Expenses.Sub}";
                subTotalText.HorAlignment = StiTextHorAlignment.Left;
                subTotalText.VertAlignment = StiVertAlignment.Center;
                subTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                subTotalText.Border.Style = StiPenStyle.None;
                subTotalText.TextBrush = new StiSolidBrush(Color.Black);
                subTotalText.WordWrap = true;
                groupFooter.Components.Add(subTotalText);

                pos = (columnWidth * 2);
                foreach (var column in objColumns)
                {
                    StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    totalText.HorAlignment = StiTextHorAlignment.Right;
                    totalText.VertAlignment = StiVertAlignment.Center;
                    totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    totalText.TextBrush = new StiSolidBrush(Color.Black);
                    totalText.WordWrap = true;

                    if (column.Type != "Variance")
                    {
                        totalText.Text.Value = "{Sum(Expenses.Column" + column.Index + ")}";
                        totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        totalText.Text.Value = "{(Sum(Expenses.Column" + column.Column1 + ") - Sum(Expenses.Column" + column.Column2 + ")) / Abs(Sum(Expenses.Column" + column.Column2 + "))}";
                        totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    groupFooter.Components.Add(totalText);

                    pos = pos + columnWidth;
                }
            }

            //Create DataBand total
            StiDataBand totalBand = new StiDataBand();
            totalBand.DataSourceName = "SummaryTotal";
            totalBand.Name = "ExpensesTotalSummary";
            totalBand.Border.Style = StiPenStyle.None;
            totalBand.Filters.Add(new StiFilter());
            totalBand.Filters[0].Item = StiFilterItem.Expression;
            totalBand.Filters[0].Expression = new StiExpression($"SummaryTotal.Type == 5");
            page.Components.Add(totalBand);

            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctTotalText.Text.Value = "Total Expenses";
            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
            acctTotalText.VertAlignment = StiVertAlignment.Center;
            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
            acctTotalText.OnlyText = false;
            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctTotalText.WordWrap = true;
            acctTotalText.Margins = new StiMargins(0, 1, 0, 0);
            totalBand.Components.Add(acctTotalText);

            //Create DataBand Net Profit
            StiDataBand netBand = new StiDataBand();
            netBand.DataSourceName = "SummaryTotal";
            netBand.Name = "NetProfitSummary}";
            netBand.Border.Style = StiPenStyle.None;
            netBand.Filters.Add(new StiFilter());
            netBand.Filters[0].Item = StiFilterItem.Expression;
            netBand.Filters[0].Expression = new StiExpression("SummaryTotal.Type == 51");
            page.Components.Add(netBand);

            StiText acctNetText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
            acctNetText.Text.Value = netProfitText;
            acctNetText.HorAlignment = StiTextHorAlignment.Left;
            acctNetText.VertAlignment = StiVertAlignment.Center;
            acctNetText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
            acctNetText.OnlyText = false;
            acctNetText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctNetText.WordWrap = true;
            acctNetText.Margins = new StiMargins(0, 1, 0, 10);
            netBand.Components.Add(acctNetText);

            pos = (columnWidth * 2);
            foreach (var column in objColumns)
            {
                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                totalText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                if (column.Type != "Variance")
                {
                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }
                totalText.HorAlignment = StiTextHorAlignment.Right;
                totalText.VertAlignment = StiVertAlignment.Center;
                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                totalText.OnlyText = false;
                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                totalText.WordWrap = true;
                totalText.Margins = new StiMargins(0, 1, 0, 0);
                totalBand.Components.Add(totalText);

                StiText netText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                netText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                if (column.Type != "Variance")
                {
                    netText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    netText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }
                netText.Type = StiSystemTextType.Expression;
                netText.HorAlignment = StiTextHorAlignment.Right;
                netText.VertAlignment = StiVertAlignment.Center;
                netText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                netText.OnlyText = false;
                netText.Font = new Font("Arial", 9F, FontStyle.Bold);
                netText.WordWrap = true;
                netText.Margins = new StiMargins(0, 1, 0, 10);
                netBand.Components.Add(netText);

                pos = pos + columnWidth;
            }

            // Net Income Percen
            StiDataBand percenBand = new StiDataBand();
            percenBand.DataSourceName = "SummaryTotal";
            percenBand.Name = "NetPercenSummary";
            percenBand.Border.Style = StiPenStyle.None;
            percenBand.Filters.Add(new StiFilter());
            percenBand.Filters[0].Item = StiFilterItem.Expression;
            percenBand.Filters[0].Expression = new StiExpression($"SummaryTotal.Type == 52");
            percenBand.NewPageAfter = !_includeProvisionsTotal;
            page.Components.Add(percenBand);

            StiText acctPercenText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctPercenText.Text.Value = $"{netProfitText} %";
            acctPercenText.HorAlignment = StiTextHorAlignment.Left;
            acctPercenText.VertAlignment = StiVertAlignment.Center;
            acctPercenText.OnlyText = false;
            acctPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctPercenText.WordWrap = true;
            acctPercenText.Margins = new StiMargins(0, 1, 0, 10);
            percenBand.Components.Add(acctPercenText);

            pos = (columnWidth * 2);
            foreach (var column in objColumns)
            {

                StiText depPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                if (column.Type != "Variance")
                {
                    depPercenText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                }
                depPercenText.TextFormat = percenFormat;
                depPercenText.HorAlignment = StiTextHorAlignment.Right;
                depPercenText.VertAlignment = StiVertAlignment.Center;
                depPercenText.OnlyText = false;
                depPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                depPercenText.WordWrap = true;
                depPercenText.Margins = new StiMargins(0, 1, 0, 10);
                percenBand.Components.Add(depPercenText);

                pos = pos + columnWidth;
            }
        }

        // Other Income (Expenses)
        if (objColumns.Count > 0)
        {
            //Create HeaderBand
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.25;
            headerBand.Name = $"OtherIncomeHeaderBandSummary";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerBand.PrintOnAllPages = false;
            headerBand.PrintIfEmpty = true;
            page.Components.Add(headerBand);

            StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctText.Text.Value = "Other Income (Expenses)";
            acctText.HorAlignment = StiTextHorAlignment.Left;
            acctText.VertAlignment = StiVertAlignment.Center;
            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            acctText.Border.Side = StiBorderSides.All;
            acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctText.Border.Style = StiPenStyle.None;
            acctText.TextBrush = new StiSolidBrush(Color.White);
            acctText.WordWrap = true;
            headerBand.Components.Add(acctText);

            //Create group header band
            if (isWithSub)
            {
                StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                groupHeader.Name = "OtherIncomeGroupHeaderBandSummary";
                groupHeader.PrintOnAllPages = false;
                groupHeader.Condition = new StiGroupConditionExpression("{OtherIncome.Sub}");
                page.Components.Add(groupHeader);

                StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                groupHeaderText.Text.Value = "{OtherIncome.Sub}";
                groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                groupHeaderText.VertAlignment = StiVertAlignment.Center;
                groupHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
                groupHeaderText.Border.Style = StiPenStyle.None;
                groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                groupHeaderText.WordWrap = true;
                groupHeader.Components.Add(groupHeaderText);
            }

            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "OtherIncome";
            dataBand.Name = "OtherIncomeDataSummary";
            dataBand.Border.Style = StiPenStyle.None;
            page.Components.Add(dataBand);

            //Create DataBand item
            StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.2));
            acctDataText.Text.Value = "{OtherIncome.fDesc}";
            acctDataText.HorAlignment = StiTextHorAlignment.Left;
            acctDataText.VertAlignment = StiVertAlignment.Center;
            acctDataText.Border.Style = StiPenStyle.None;
            acctDataText.OnlyText = false;
            acctDataText.Border.Side = StiBorderSides.All;
            acctDataText.Font = new Font("Arial", 8F);
            acctDataText.WordWrap = true;
            acctDataText.CanGrow = true;
            acctDataText.Margins = new StiMargins(0, 1, 4, 0);
            dataBand.Components.Add(acctDataText);

            pos = (columnWidth * 2);

            foreach (var column in objColumns)
            {
                //Create text on header
                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                hText.Text.Value = column.Label;
                hText.HorAlignment = StiTextHorAlignment.Right;
                hText.VertAlignment = StiVertAlignment.Center;
                hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                hText.Border.Side = StiBorderSides.All;
                hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                hText.Border.Style = StiPenStyle.None;
                hText.TextBrush = new StiSolidBrush(Color.White);
                hText.WordWrap = true;
                hText.CanGrow = true;
                headerBand.Components.Add(hText);

                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                dataText.Text.Value = "{OtherIncome.Column" + column.Index + "}";

                if (column.Type != "Variance")
                {
                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }

                if (column.Type == "Actual")
                {
                    dataText.Interaction.Hyperlink = new StiHyperlinkExpression("{OtherIncome.Column" + column.Index + "URL}");
                }

                dataText.HorAlignment = StiTextHorAlignment.Right;
                dataText.VertAlignment = StiVertAlignment.Top;
                dataText.Border.Style = StiPenStyle.None;
                dataText.OnlyText = false;
                dataText.Border.Side = StiBorderSides.All;
                dataText.Font = new Font("Arial", 8F);
                dataText.WordWrap = true;
                dataText.CanGrow = true;
                dataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(dataText);

                pos = pos + columnWidth;
            }

            //Create group footer band
            if (isWithSub)
            {
                StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.4));
                groupFooter.Name = $"OtherIncomeGroupFooterBandSummary";
                page.Components.Add(groupFooter);

                StiText subTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                subTotalText.Text.Value = "Total {OtherIncome.Sub}";
                subTotalText.HorAlignment = StiTextHorAlignment.Left;
                subTotalText.VertAlignment = StiVertAlignment.Center;
                subTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                subTotalText.Border.Style = StiPenStyle.None;
                subTotalText.TextBrush = new StiSolidBrush(Color.Black);
                subTotalText.WordWrap = true;
                groupFooter.Components.Add(subTotalText);

                pos = (columnWidth * 2);
                foreach (var column in objColumns)
                {
                    StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    totalText.HorAlignment = StiTextHorAlignment.Right;
                    totalText.VertAlignment = StiVertAlignment.Center;
                    totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    totalText.TextBrush = new StiSolidBrush(Color.Black);
                    totalText.WordWrap = true;

                    if (column.Type != "Variance")
                    {
                        totalText.Text.Value = "{Sum(OtherIncome.Column" + column.Index + ")}";
                        totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        totalText.Text.Value = "{(Sum(OtherIncome.Column" + column.Column1 + ") - Sum(OtherIncome.Column" + column.Column2 + ")) / Abs(Sum(OtherIncome.Column" + column.Column2 + "))}";
                        totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    groupFooter.Components.Add(totalText);

                    pos = pos + columnWidth;
                }
            }

            //Create DataBand total
            StiDataBand totalBand = new StiDataBand();
            totalBand.DataSourceName = "SummaryTotal";
            totalBand.Name = "OtherIncomeTotalSummary";
            totalBand.Border.Style = StiPenStyle.None;
            totalBand.Filters.Add(new StiFilter());
            totalBand.Filters[0].Item = StiFilterItem.Expression;
            totalBand.Filters[0].Expression = new StiExpression("SummaryTotal.Type == 8");
            page.Components.Add(totalBand);

            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctTotalText.Text.Value = "Total Other Income (Expenses)";
            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
            acctTotalText.VertAlignment = StiVertAlignment.Center;
            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
            acctTotalText.OnlyText = false;
            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctTotalText.WordWrap = true;
            acctTotalText.Margins = new StiMargins(0, 1, 0, 0);
            totalBand.Components.Add(acctTotalText);

            //Create DataBand Income Before Provisions For Income Taxes
            StiDataBand grossBand = new StiDataBand();
            grossBand.DataSourceName = "SummaryTotal";
            grossBand.Name = $"BeforeProvisionsBandSummary";
            grossBand.Border.Style = StiPenStyle.None;
            grossBand.Filters.Add(new StiFilter());
            grossBand.Filters[0].Item = StiFilterItem.Expression;
            grossBand.Filters[0].Expression = new StiExpression("SummaryTotal.Type == 81");
            page.Components.Add(grossBand);

            StiText acctGrossText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
            acctGrossText.Text.Value = "Income Before Provisions For Income Taxes";
            acctGrossText.HorAlignment = StiTextHorAlignment.Left;
            acctGrossText.VertAlignment = StiVertAlignment.Center;
            acctGrossText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
            acctGrossText.OnlyText = false;
            acctGrossText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctGrossText.WordWrap = true;
            acctGrossText.Margins = new StiMargins(0, 1, 0, 10);
            grossBand.Components.Add(acctGrossText);

            pos = (columnWidth * 2);
            foreach (var column in objColumns)
            {
                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                totalText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                if (column.Type != "Variance")
                {
                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }
                totalText.HorAlignment = StiTextHorAlignment.Right;
                totalText.VertAlignment = StiVertAlignment.Center;
                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                totalText.OnlyText = false;
                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                totalText.WordWrap = true;
                totalText.Margins = new StiMargins(0, 1, 0, 0);
                totalBand.Components.Add(totalText);

                StiText grossText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                grossText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                if (column.Type != "Variance")
                {
                    grossText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    grossText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }
                grossText.Type = StiSystemTextType.Expression;
                grossText.HorAlignment = StiTextHorAlignment.Right;
                grossText.VertAlignment = StiVertAlignment.Center;
                grossText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                grossText.OnlyText = false;
                grossText.Font = new Font("Arial", 9F, FontStyle.Bold);
                grossText.WordWrap = true;
                grossText.Margins = new StiMargins(0, 1, 0, 10);
                grossBand.Components.Add(grossText);

                pos = pos + columnWidth;
            }

            // Income Before Provisions For Income Taxes Percen
            StiDataBand percenBand = new StiDataBand();
            percenBand.DataSourceName = "SummaryTotal";
            percenBand.Name = "BeforeProvisionsPercenSummary";
            percenBand.Border.Style = StiPenStyle.None;
            percenBand.Filters.Add(new StiFilter());
            percenBand.Filters[0].Item = StiFilterItem.Expression;
            percenBand.Filters[0].Expression = new StiExpression($"SummaryTotal.Type == 82");
            page.Components.Add(percenBand);

            StiText acctPercenText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctPercenText.Text.Value = "Income Before Provisions For Income Taxes %";
            acctPercenText.HorAlignment = StiTextHorAlignment.Left;
            acctPercenText.VertAlignment = StiVertAlignment.Center;
            acctPercenText.OnlyText = false;
            acctPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctPercenText.WordWrap = true;
            acctPercenText.Margins = new StiMargins(0, 1, 0, 10);
            percenBand.Components.Add(acctPercenText);

            pos = (columnWidth * 2);
            foreach (var column in objColumns)
            {

                StiText depPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                if (column.Type == "Actual" || column.Type == "Difference")
                {
                    depPercenText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                }
                depPercenText.TextFormat = percenFormat;
                depPercenText.HorAlignment = StiTextHorAlignment.Right;
                depPercenText.VertAlignment = StiVertAlignment.Center;
                depPercenText.OnlyText = false;
                depPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                depPercenText.WordWrap = true;
                depPercenText.Margins = new StiMargins(0, 1, 0, 10);
                percenBand.Components.Add(depPercenText);

                pos = pos + columnWidth;
            }
        }

        // Provisions for Income Taxes
        if (objColumns.Count > 0)
        {
            //Create HeaderBand
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.25;
            headerBand.Name = $"IncomeTaxesHeaderBandSummary";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerBand.PrintOnAllPages = false;
            headerBand.PrintIfEmpty = true;
            page.Components.Add(headerBand);

            StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctText.Text.Value = "Provisions for Income Taxes";
            acctText.HorAlignment = StiTextHorAlignment.Left;
            acctText.VertAlignment = StiVertAlignment.Center;
            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            acctText.Border.Side = StiBorderSides.All;
            acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctText.Border.Style = StiPenStyle.None;
            acctText.TextBrush = new StiSolidBrush(Color.White);
            acctText.WordWrap = true;
            headerBand.Components.Add(acctText);

            //Create group header band
            if (isWithSub)
            {
                StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                groupHeader.Name = "IncomeTaxesGroupHeaderBandSummary";
                groupHeader.PrintOnAllPages = false;
                groupHeader.Condition = new StiGroupConditionExpression("{IncomeTaxes.Sub}");
                page.Components.Add(groupHeader);

                StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                groupHeaderText.Text.Value = "{IncomeTaxes.Sub}";
                groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                groupHeaderText.VertAlignment = StiVertAlignment.Center;
                groupHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
                groupHeaderText.Border.Style = StiPenStyle.None;
                groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                groupHeaderText.WordWrap = true;
                groupHeader.Components.Add(groupHeaderText);
            }

            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "IncomeTaxes";
            dataBand.Name = $"IncomeTaxesDataSummary";
            dataBand.Border.Style = StiPenStyle.None;
            page.Components.Add(dataBand);

            //Create DataBand item

            StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.2));
            acctDataText.Text.Value = "{IncomeTaxes.fDesc}";
            acctDataText.HorAlignment = StiTextHorAlignment.Left;
            acctDataText.VertAlignment = StiVertAlignment.Center;
            acctDataText.Border.Style = StiPenStyle.None;
            acctDataText.OnlyText = false;
            acctDataText.Border.Side = StiBorderSides.All;
            acctDataText.Font = new Font("Arial", 8F);
            acctDataText.WordWrap = true;
            acctDataText.CanGrow = true;
            acctDataText.Margins = new StiMargins(0, 1, 4, 0);
            dataBand.Components.Add(acctDataText);

            pos = (columnWidth * 2);

            foreach (var column in objColumns)
            {
                //Create text on header
                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                hText.Text.Value = column.Label;
                hText.HorAlignment = StiTextHorAlignment.Right;
                hText.VertAlignment = StiVertAlignment.Center;
                hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                hText.Border.Side = StiBorderSides.All;
                hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                hText.Border.Style = StiPenStyle.None;
                hText.TextBrush = new StiSolidBrush(Color.White);
                hText.WordWrap = true;
                hText.CanGrow = true;
                headerBand.Components.Add(hText);

                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                dataText.Text.Value = "{IncomeTaxes.Column" + column.Index + "}";

                if (column.Type != "Variance")
                {
                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }

                if (column.Type == "Actual")
                {
                    dataText.Interaction.Hyperlink = new StiHyperlinkExpression("{IncomeTaxes.Column" + column.Index + "URL}");
                }

                dataText.HorAlignment = StiTextHorAlignment.Right;
                dataText.VertAlignment = StiVertAlignment.Top;
                dataText.Border.Style = StiPenStyle.None;
                dataText.OnlyText = false;
                dataText.Border.Side = StiBorderSides.All;
                dataText.Font = new Font("Arial", 8F);
                dataText.WordWrap = true;
                dataText.CanGrow = true;
                dataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(dataText);

                pos = pos + columnWidth;
            }

            //Create group footer band
            if (isWithSub)
            {
                StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.4));
                groupFooter.Name = $"IncomeTaxesGroupFooterBandSummary";
                page.Components.Add(groupFooter);

                StiText subTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                subTotalText.Text.Value = "Total {IncomeTaxes.Sub}";
                subTotalText.HorAlignment = StiTextHorAlignment.Left;
                subTotalText.VertAlignment = StiVertAlignment.Center;
                subTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                subTotalText.Border.Style = StiPenStyle.None;
                subTotalText.TextBrush = new StiSolidBrush(Color.Black);
                subTotalText.WordWrap = true;
                groupFooter.Components.Add(subTotalText);

                pos = (columnWidth * 2);
                foreach (var column in objColumns)
                {
                    StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    totalText.HorAlignment = StiTextHorAlignment.Right;
                    totalText.VertAlignment = StiVertAlignment.Center;
                    totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    totalText.TextBrush = new StiSolidBrush(Color.Black);
                    totalText.WordWrap = true;

                    if (column.Type != "Variance")
                    {
                        totalText.Text.Value = "{Sum(IncomeTaxes.Column" + column.Index + ")}";
                        totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        totalText.Text.Value = "{(Sum(IncomeTaxes.Column" + column.Column1 + ") - Sum(IncomeTaxes.Column" + column.Column2 + ")) / Abs(Sum(IncomeTaxes.Column" + column.Column2 + "))}";
                        totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    groupFooter.Components.Add(totalText);

                    pos = pos + columnWidth;
                }
            }

            //Create DataBand total
            StiDataBand totalBand = new StiDataBand();
            totalBand.DataSourceName = "SummaryTotal";
            totalBand.Name = "IncomeTaxesTotalSummary";
            totalBand.Border.Style = StiPenStyle.None;
            totalBand.Filters.Add(new StiFilter());
            totalBand.Filters[0].Item = StiFilterItem.Expression;
            totalBand.Filters[0].Expression = new StiExpression($"SummaryTotal.Type == 9");
            page.Components.Add(totalBand);

            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctTotalText.Text.Value = "Total Provisions for Income Taxes";
            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
            acctTotalText.VertAlignment = StiVertAlignment.Center;
            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
            acctTotalText.OnlyText = false;
            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctTotalText.WordWrap = true;
            acctTotalText.Margins = new StiMargins(0, 1, 0, 0);
            totalBand.Components.Add(acctTotalText);

            //Create DataBand Net Income
            StiDataBand netBand = new StiDataBand();
            netBand.DataSourceName = "SummaryTotal";
            netBand.Name = "NetIncomeSummary}";
            netBand.Border.Style = StiPenStyle.None;
            netBand.Filters.Add(new StiFilter());
            netBand.Filters[0].Item = StiFilterItem.Expression;
            netBand.Filters[0].Expression = new StiExpression("SummaryTotal.Type == 91");
            page.Components.Add(netBand);

            StiText acctNetText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
            acctNetText.Text.Value = "Net Income";
            acctNetText.HorAlignment = StiTextHorAlignment.Left;
            acctNetText.VertAlignment = StiVertAlignment.Center;
            acctNetText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
            acctNetText.OnlyText = false;
            acctNetText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctNetText.WordWrap = true;
            acctNetText.Margins = new StiMargins(0, 1, 0, 10);
            netBand.Components.Add(acctNetText);

            pos = (columnWidth * 2);
            foreach (var column in objColumns)
            {
                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                totalText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                if (column.Type != "Variance")
                {
                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }
                totalText.HorAlignment = StiTextHorAlignment.Right;
                totalText.VertAlignment = StiVertAlignment.Center;
                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                totalText.OnlyText = false;
                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                totalText.WordWrap = true;
                totalText.Margins = new StiMargins(0, 1, 0, 0);
                totalBand.Components.Add(totalText);

                StiText netText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                netText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                if (column.Type != "Variance")
                {
                    netText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    netText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }
                netText.Type = StiSystemTextType.Expression;
                netText.HorAlignment = StiTextHorAlignment.Right;
                netText.VertAlignment = StiVertAlignment.Center;
                netText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                netText.OnlyText = false;
                netText.Font = new Font("Arial", 9F, FontStyle.Bold);
                netText.WordWrap = true;
                netText.Margins = new StiMargins(0, 1, 0, 10);
                netBand.Components.Add(netText);

                pos = pos + columnWidth;
            }

            // Net Income Percen
            StiDataBand percenBand = new StiDataBand();
            percenBand.DataSourceName = "SummaryTotal";
            percenBand.Name = "NetIncomePercenSummary";
            percenBand.Border.Style = StiPenStyle.None;
            percenBand.Filters.Add(new StiFilter());
            percenBand.Filters[0].Item = StiFilterItem.Expression;
            percenBand.Filters[0].Expression = new StiExpression($"SummaryTotal.Type == 92");
            percenBand.NewPageAfter = true;
            page.Components.Add(percenBand);

            StiText acctPercenText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctPercenText.Text.Value = "Net Income %";
            acctPercenText.HorAlignment = StiTextHorAlignment.Left;
            acctPercenText.VertAlignment = StiVertAlignment.Center;
            acctPercenText.OnlyText = false;
            acctPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctPercenText.WordWrap = true;
            acctPercenText.Margins = new StiMargins(0, 1, 0, 10);
            percenBand.Components.Add(acctPercenText);

            pos = (columnWidth * 2);
            foreach (var column in objColumns)
            {

                StiText depPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                if (column.Type != "Variance")
                {
                    depPercenText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                }
                depPercenText.TextFormat = percenFormat;
                depPercenText.HorAlignment = StiTextHorAlignment.Right;
                depPercenText.VertAlignment = StiVertAlignment.Center;
                depPercenText.OnlyText = false;
                depPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                depPercenText.WordWrap = true;
                depPercenText.Margins = new StiMargins(0, 1, 0, 10);
                percenBand.Components.Add(depPercenText);

                pos = pos + columnWidth;
            }
        }
    }

    private void BuildAllDepartmentTotal(StiPage page, List<ComparativeStatementRequest> objColumns)
    {
        StiFormatService currencyFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
        StiFormatService percenFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");

        var netProfitText = "Income From Operations";

        StiHeaderBand centerTitle = new StiHeaderBand();
        centerTitle.Height = 0.5;
        centerTitle.PrintIfEmpty = true;
        page.Components.Add(centerTitle);

        StiText centerTitleText = new StiText(new RectangleD(0, 0.1, page.Width, 0.25));
        centerTitleText.Text.Value = "Total";
        centerTitleText.HorAlignment = StiTextHorAlignment.Center;
        centerTitleText.VertAlignment = StiVertAlignment.Center;
        centerTitleText.Font = new Font("Arial", 18F, FontStyle.Bold);
        centerTitleText.WordWrap = true;
        centerTitle.Components.Add(centerTitleText);

        var columnCount = objColumns.Count + 2;
        double columnWidth = page.Width / columnCount;
        double pos = 0;

        // Revenues
        if (objColumns.Count > 0)
        {
            //Create HeaderBand
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.25;
            headerBand.Name = $"RevenuesHeaderBandSummary";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerBand.PrintOnAllPages = false;
            headerBand.PrintIfEmpty = true;
            page.Components.Add(headerBand);

            StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctText.Text.Value = "Revenues";
            acctText.HorAlignment = StiTextHorAlignment.Left;
            acctText.VertAlignment = StiVertAlignment.Center;
            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            acctText.Border.Side = StiBorderSides.All;
            acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctText.Border.Style = StiPenStyle.None;
            acctText.TextBrush = new StiSolidBrush(Color.White);
            acctText.WordWrap = true;
            headerBand.Components.Add(acctText);

            //Create group header band
            StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.2));
            groupHeader.Name = "RevenuesGroupHeaderBandSummary";
            groupHeader.PrintOnAllPages = false;
            groupHeader.Condition = new StiGroupConditionExpression("{Revenues.Sub}");
            page.Components.Add(groupHeader);

            StiText groupHeaderText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.2));
            groupHeaderText.Text.Value = "{Revenues.Sub}";
            groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
            groupHeaderText.VertAlignment = StiVertAlignment.Center;
            groupHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
            groupHeaderText.Border.Style = StiPenStyle.None;
            groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
            groupHeaderText.WordWrap = true;
            groupHeader.Components.Add(groupHeaderText);

            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "Revenues";
            dataBand.Name = $"RevenuesDataSummary";
            dataBand.Border.Style = StiPenStyle.None;
            dataBand.Height = 0;
            page.Components.Add(dataBand);

            pos = (columnWidth * 2);

            foreach (var column in objColumns)
            {
                //Create text on header
                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                hText.Text.Value = column.Label;
                hText.HorAlignment = StiTextHorAlignment.Right;
                hText.VertAlignment = StiVertAlignment.Center;
                hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                hText.Border.Side = StiBorderSides.All;
                hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                hText.Border.Style = StiPenStyle.None;
                hText.TextBrush = new StiSolidBrush(Color.White);
                hText.WordWrap = true;
                hText.CanGrow = true;
                headerBand.Components.Add(hText);

                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));

                if (column.Type != "Variance")
                {
                    dataText.Text.Value = "{Sum(Revenues.Column" + column.Index + ")}";
                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    dataText.Text.Value = "{(Sum(Revenues.Column" + column.Column1 + ") - Sum(Revenues.Column" + column.Column2 + ")) / Abs(Sum(Revenues.Column" + column.Column2 + "))}";
                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }

                dataText.HorAlignment = StiTextHorAlignment.Right;
                dataText.VertAlignment = StiVertAlignment.Top;
                dataText.Border.Style = StiPenStyle.None;
                dataText.OnlyText = false;
                dataText.Border.Side = StiBorderSides.All;
                dataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                dataText.WordWrap = true;
                dataText.CanGrow = true;
                dataText.Margins = new StiMargins(0, 1, 4, 0);
                groupHeader.Components.Add(dataText);

                pos = pos + columnWidth;
            }

            //Create group footer band
            StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.4));
            groupFooter.Name = $"RevenuesGroupFooterBandSummary";
            groupFooter.Height = 0;
            page.Components.Add(groupFooter);

            //Create DataBand total
            StiDataBand totalBand = new StiDataBand();
            totalBand.DataSourceName = "SummaryTotal";
            totalBand.Name = $"RevenuesTotalSummary";
            totalBand.Border.Style = StiPenStyle.None;
            totalBand.Filters.Add(new StiFilter());
            totalBand.Filters[0].Item = StiFilterItem.Expression;
            totalBand.Filters[0].Expression = new StiExpression("SummaryTotal.Type == 3");
            page.Components.Add(totalBand);

            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
            acctTotalText.Text.Value = "Total Revenues";
            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
            acctTotalText.VertAlignment = StiVertAlignment.Center;
            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
            acctTotalText.OnlyText = false;
            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctTotalText.WordWrap = true;
            acctTotalText.Margins = new StiMargins(0, 1, 0, 10);
            totalBand.Components.Add(acctTotalText);

            pos = (columnWidth * 2);
            foreach (var column in objColumns)
            {
                StiText footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                footerText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                if (column.Type != "Variance")
                {
                    footerText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    footerText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }
                footerText.HorAlignment = StiTextHorAlignment.Right;
                footerText.VertAlignment = StiVertAlignment.Center;
                footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                footerText.OnlyText = false;
                footerText.Font = new Font("Arial", 9F, FontStyle.Bold);
                footerText.WordWrap = true;
                footerText.Margins = new StiMargins(0, 1, 0, 10);
                totalBand.Components.Add(footerText);

                pos = pos + columnWidth;
            }
        }

        // Cost Of Sales
        if (objColumns.Count > 0)
        {
            //Create HeaderBand
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.25;
            headerBand.Name = $"CostOfSalesHeaderBandSummary";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerBand.PrintOnAllPages = false;
            headerBand.PrintIfEmpty = true;
            page.Components.Add(headerBand);

            StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctText.Text.Value = "Cost Of Sales";
            acctText.HorAlignment = StiTextHorAlignment.Left;
            acctText.VertAlignment = StiVertAlignment.Center;
            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            acctText.Border.Side = StiBorderSides.All;
            acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctText.Border.Style = StiPenStyle.None;
            acctText.TextBrush = new StiSolidBrush(Color.White);
            acctText.WordWrap = true;
            headerBand.Components.Add(acctText);

            //Create group header band
            StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.2));
            groupHeader.Name = "CostOfSalesGroupHeaderBandSummary";
            groupHeader.PrintOnAllPages = false;
            groupHeader.Condition = new StiGroupConditionExpression("{CostOfSales.Sub}");
            page.Components.Add(groupHeader);

            StiText groupHeaderText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.2));
            groupHeaderText.Text.Value = "{CostOfSales.Sub}";
            groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
            groupHeaderText.VertAlignment = StiVertAlignment.Center;
            groupHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
            groupHeaderText.Border.Style = StiPenStyle.None;
            groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
            groupHeaderText.WordWrap = true;
            groupHeader.Components.Add(groupHeaderText);

            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "CostOfSales";
            dataBand.Name = $"CostOfSalesDataSummary";
            dataBand.Border.Style = StiPenStyle.None;
            dataBand.Height = 0;
            page.Components.Add(dataBand);

            pos = (columnWidth * 2);

            foreach (var column in objColumns)
            {
                //Create text on header
                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                hText.Text.Value = column.Label;
                hText.HorAlignment = StiTextHorAlignment.Right;
                hText.VertAlignment = StiVertAlignment.Center;
                hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                hText.Border.Side = StiBorderSides.All;
                hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                hText.Border.Style = StiPenStyle.None;
                hText.TextBrush = new StiSolidBrush(Color.White);
                hText.WordWrap = true;
                hText.CanGrow = true;
                headerBand.Components.Add(hText);

                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));

                if (column.Type != "Variance")
                {
                    dataText.Text.Value = "{Sum(CostOfSales.Column" + column.Index + ")}";
                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    dataText.Text.Value = "{(Sum(CostOfSales.Column" + column.Column1 + ") - Sum(CostOfSales.Column" + column.Column2 + ")) / Abs(Sum(CostOfSales.Column" + column.Column2 + "))}";
                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }

                dataText.HorAlignment = StiTextHorAlignment.Right;
                dataText.VertAlignment = StiVertAlignment.Top;
                dataText.Border.Style = StiPenStyle.None;
                dataText.OnlyText = false;
                dataText.Border.Side = StiBorderSides.All;
                dataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                dataText.WordWrap = true;
                dataText.CanGrow = true;
                dataText.Margins = new StiMargins(0, 1, 4, 0);
                groupHeader.Components.Add(dataText);

                pos = pos + columnWidth;
            }

            //Create group footer band
            StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.4));
            groupFooter.Name = $"CostOfSalesGroupFooterBandSummary";
            groupFooter.Height = 0;
            page.Components.Add(groupFooter);

            //Create DataBand total
            StiDataBand totalBand = new StiDataBand();
            totalBand.DataSourceName = "SummaryTotal";
            totalBand.Name = "CostOfSalesTotalSummary";
            totalBand.Border.Style = StiPenStyle.None;
            totalBand.Filters.Add(new StiFilter());
            totalBand.Filters[0].Item = StiFilterItem.Expression;
            totalBand.Filters[0].Expression = new StiExpression("SummaryTotal.Type == 4");
            page.Components.Add(totalBand);

            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctTotalText.Text.Value = "Total Cost Of Sales";
            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
            acctTotalText.VertAlignment = StiVertAlignment.Center;
            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
            acctTotalText.OnlyText = false;
            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctTotalText.WordWrap = true;
            acctTotalText.Margins = new StiMargins(0, 1, 0, 0);
            totalBand.Components.Add(acctTotalText);

            //Create DataBand Gross Profit
            StiDataBand grossBand = new StiDataBand();
            grossBand.DataSourceName = "SummaryTotal";
            grossBand.Name = $"GrossProfitBandSummary";
            grossBand.Border.Style = StiPenStyle.None;
            grossBand.Filters.Add(new StiFilter());
            grossBand.Filters[0].Item = StiFilterItem.Expression;
            grossBand.Filters[0].Expression = new StiExpression("SummaryTotal.Type == 41");
            page.Components.Add(grossBand);

            StiText acctGrossText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
            acctGrossText.Text.Value = "Gross Profit";
            acctGrossText.HorAlignment = StiTextHorAlignment.Left;
            acctGrossText.VertAlignment = StiVertAlignment.Center;
            acctGrossText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
            acctGrossText.OnlyText = false;
            acctGrossText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctGrossText.WordWrap = true;
            acctGrossText.Margins = new StiMargins(0, 1, 0, 10);
            grossBand.Components.Add(acctGrossText);

            pos = (columnWidth * 2);
            foreach (var column in objColumns)
            {
                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                totalText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                if (column.Type != "Variance")
                {
                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }
                totalText.HorAlignment = StiTextHorAlignment.Right;
                totalText.VertAlignment = StiVertAlignment.Center;
                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                totalText.OnlyText = false;
                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                totalText.WordWrap = true;
                totalText.Margins = new StiMargins(0, 1, 0, 0);
                totalBand.Components.Add(totalText);

                StiText grossText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                grossText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                if (column.Type != "Variance")
                {
                    grossText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    grossText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }
                grossText.Type = StiSystemTextType.Expression;
                grossText.HorAlignment = StiTextHorAlignment.Right;
                grossText.VertAlignment = StiVertAlignment.Center;
                grossText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                grossText.OnlyText = false;
                grossText.Font = new Font("Arial", 9F, FontStyle.Bold);
                grossText.WordWrap = true;
                grossText.Margins = new StiMargins(0, 1, 0, 10);
                grossBand.Components.Add(grossText);

                pos = pos + columnWidth;
            }

            // Gross Profit Percen
            StiDataBand percenBand = new StiDataBand();
            percenBand.DataSourceName = "SummaryTotal";
            percenBand.Name = "GrossPercenSummary";
            percenBand.Border.Style = StiPenStyle.None;
            percenBand.Filters.Add(new StiFilter());
            percenBand.Filters[0].Item = StiFilterItem.Expression;
            percenBand.Filters[0].Expression = new StiExpression($"SummaryTotal.Type == 42");
            page.Components.Add(percenBand);

            StiText acctPercenText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctPercenText.Text.Value = "Gross Profit %";
            acctPercenText.HorAlignment = StiTextHorAlignment.Left;
            acctPercenText.VertAlignment = StiVertAlignment.Center;
            acctPercenText.OnlyText = false;
            acctPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctPercenText.WordWrap = true;
            acctPercenText.Margins = new StiMargins(0, 1, 0, 10);
            percenBand.Components.Add(acctPercenText);

            pos = (columnWidth * 2);
            foreach (var column in objColumns)
            {

                StiText depPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                if (column.Type != "Variance")
                {
                    depPercenText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                }
                depPercenText.TextFormat = percenFormat;
                depPercenText.HorAlignment = StiTextHorAlignment.Right;
                depPercenText.VertAlignment = StiVertAlignment.Center;
                depPercenText.OnlyText = false;
                depPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                depPercenText.WordWrap = true;
                depPercenText.Margins = new StiMargins(0, 1, 0, 10);
                percenBand.Components.Add(depPercenText);

                pos = pos + columnWidth;
            }
        }

        // Expenses
        if (objColumns.Count > 0)
        {
            //Create HeaderBand
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.25;
            headerBand.Name = $"ExpensesHeaderBandSummary";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerBand.PrintOnAllPages = false;
            headerBand.PrintIfEmpty = true;
            page.Components.Add(headerBand);

            StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctText.Text.Value = "Expenses";
            acctText.HorAlignment = StiTextHorAlignment.Left;
            acctText.VertAlignment = StiVertAlignment.Center;
            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            acctText.Border.Side = StiBorderSides.All;
            acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctText.Border.Style = StiPenStyle.None;
            acctText.TextBrush = new StiSolidBrush(Color.White);
            acctText.WordWrap = true;
            headerBand.Components.Add(acctText);

            //Create group header band
            StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.2));
            groupHeader.Name = "ExpensesGroupHeaderBandSummary";
            groupHeader.PrintOnAllPages = false;
            groupHeader.Condition = new StiGroupConditionExpression("{Expenses.Sub}");
            page.Components.Add(groupHeader);

            StiText groupHeaderText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.2));
            groupHeaderText.Text.Value = "{Expenses.Sub}";
            groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
            groupHeaderText.VertAlignment = StiVertAlignment.Center;
            groupHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
            groupHeaderText.Border.Style = StiPenStyle.None;
            groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
            groupHeaderText.WordWrap = true;
            groupHeader.Components.Add(groupHeaderText);

            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "Expenses";
            dataBand.Name = $"ExpensesDataSummary";
            dataBand.Border.Style = StiPenStyle.None;
            dataBand.Height = 0;
            page.Components.Add(dataBand);

            pos = (columnWidth * 2);

            foreach (var column in objColumns)
            {
                //Create text on header
                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                hText.Text.Value = column.Label;
                hText.HorAlignment = StiTextHorAlignment.Right;
                hText.VertAlignment = StiVertAlignment.Center;
                hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                hText.Border.Side = StiBorderSides.All;
                hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                hText.Border.Style = StiPenStyle.None;
                hText.TextBrush = new StiSolidBrush(Color.White);
                hText.WordWrap = true;
                hText.CanGrow = true;
                headerBand.Components.Add(hText);

                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));

                if (column.Type != "Variance")
                {
                    dataText.Text.Value = "{Sum(Expenses.Column" + column.Index + ")}";
                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    dataText.Text.Value = "{(Sum(Expenses.Column" + column.Column1 + ") - Sum(Expenses.Column" + column.Column2 + ")) / Abs(Sum(Expenses.Column" + column.Column2 + "))}";
                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }

                dataText.HorAlignment = StiTextHorAlignment.Right;
                dataText.VertAlignment = StiVertAlignment.Top;
                dataText.Border.Style = StiPenStyle.None;
                dataText.OnlyText = false;
                dataText.Border.Side = StiBorderSides.All;
                dataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                dataText.WordWrap = true;
                dataText.CanGrow = true;
                dataText.Margins = new StiMargins(0, 1, 4, 0);
                groupHeader.Components.Add(dataText);

                pos = pos + columnWidth;
            }

            //Create group footer band
            StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.4));
            groupFooter.Name = $"ExpensesGroupFooterBandSummary";
            groupFooter.Height = 0;
            page.Components.Add(groupFooter);

            //Create DataBand total
            StiDataBand totalBand = new StiDataBand();
            totalBand.DataSourceName = "SummaryTotal";
            totalBand.Name = "ExpensesTotalSummary";
            totalBand.Border.Style = StiPenStyle.None;
            totalBand.Filters.Add(new StiFilter());
            totalBand.Filters[0].Item = StiFilterItem.Expression;
            totalBand.Filters[0].Expression = new StiExpression($"SummaryTotal.Type == 5");
            page.Components.Add(totalBand);

            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctTotalText.Text.Value = "Total Expenses";
            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
            acctTotalText.VertAlignment = StiVertAlignment.Center;
            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
            acctTotalText.OnlyText = false;
            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctTotalText.WordWrap = true;
            acctTotalText.Margins = new StiMargins(0, 1, 0, 0);
            totalBand.Components.Add(acctTotalText);

            //Create DataBand Net Profit
            StiDataBand netBand = new StiDataBand();
            netBand.DataSourceName = "SummaryTotal";
            netBand.Name = "NetProfitSummary";
            netBand.Border.Style = StiPenStyle.None;
            netBand.Filters.Add(new StiFilter());
            netBand.Filters[0].Item = StiFilterItem.Expression;
            netBand.Filters[0].Expression = new StiExpression("SummaryTotal.Type == 51");
            page.Components.Add(netBand);

            StiText acctNetText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
            acctNetText.Text.Value = netProfitText;
            acctNetText.HorAlignment = StiTextHorAlignment.Left;
            acctNetText.VertAlignment = StiVertAlignment.Center;
            acctNetText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
            acctNetText.OnlyText = false;
            acctNetText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctNetText.WordWrap = true;
            acctNetText.Margins = new StiMargins(0, 1, 0, 10);
            netBand.Components.Add(acctNetText);

            pos = (columnWidth * 2);
            foreach (var column in objColumns)
            {
                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                totalText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                if (column.Type != "Variance")
                {
                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }
                totalText.HorAlignment = StiTextHorAlignment.Right;
                totalText.VertAlignment = StiVertAlignment.Center;
                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                totalText.OnlyText = false;
                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                totalText.WordWrap = true;
                totalText.Margins = new StiMargins(0, 1, 0, 0);
                totalBand.Components.Add(totalText);

                StiText netText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                netText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                if (column.Type != "Variance")
                {
                    netText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    netText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }
                netText.Type = StiSystemTextType.Expression;
                netText.HorAlignment = StiTextHorAlignment.Right;
                netText.VertAlignment = StiVertAlignment.Center;
                netText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                netText.OnlyText = false;
                netText.Font = new Font("Arial", 9F, FontStyle.Bold);
                netText.WordWrap = true;
                netText.Margins = new StiMargins(0, 1, 0, 10);
                netBand.Components.Add(netText);

                pos = pos + columnWidth;
            }

            // Net Income Percen
            StiDataBand percenBand = new StiDataBand();
            percenBand.DataSourceName = "SummaryTotal";
            percenBand.Name = "NetPercenSummary";
            percenBand.Border.Style = StiPenStyle.None;
            percenBand.Filters.Add(new StiFilter());
            percenBand.Filters[0].Item = StiFilterItem.Expression;
            percenBand.Filters[0].Expression = new StiExpression($"SummaryTotal.Type == 52");
            percenBand.NewPageAfter = !_includeProvisionsTotal;
            page.Components.Add(percenBand);

            StiText acctPercenText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctPercenText.Text.Value = $"{netProfitText} %";
            acctPercenText.HorAlignment = StiTextHorAlignment.Left;
            acctPercenText.VertAlignment = StiVertAlignment.Center;
            acctPercenText.OnlyText = false;
            acctPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctPercenText.WordWrap = true;
            acctPercenText.Margins = new StiMargins(0, 1, 0, 10);
            percenBand.Components.Add(acctPercenText);

            pos = (columnWidth * 2);
            foreach (var column in objColumns)
            {

                StiText depPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                if (column.Type != "Variance")
                {
                    depPercenText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                }
                depPercenText.TextFormat = percenFormat;
                depPercenText.HorAlignment = StiTextHorAlignment.Right;
                depPercenText.VertAlignment = StiVertAlignment.Center;
                depPercenText.OnlyText = false;
                depPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                depPercenText.WordWrap = true;
                depPercenText.Margins = new StiMargins(0, 1, 0, 10);
                percenBand.Components.Add(depPercenText);

                pos = pos + columnWidth;
            }
        }

        // Other Income (Expenses)
        if (objColumns.Count > 0)
        {
            //Create HeaderBand
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.25;
            headerBand.Name = $"OtherIncomeHeaderBandSummary";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerBand.PrintOnAllPages = false;
            headerBand.PrintIfEmpty = true;
            page.Components.Add(headerBand);

            StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctText.Text.Value = "Other Income (Expenses)";
            acctText.HorAlignment = StiTextHorAlignment.Left;
            acctText.VertAlignment = StiVertAlignment.Center;
            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            acctText.Border.Side = StiBorderSides.All;
            acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctText.Border.Style = StiPenStyle.None;
            acctText.TextBrush = new StiSolidBrush(Color.White);
            acctText.WordWrap = true;
            headerBand.Components.Add(acctText);

            //Create group header band
            StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.2));
            groupHeader.Name = "OtherIncomeGroupHeaderBandSummary";
            groupHeader.PrintOnAllPages = false;
            groupHeader.Condition = new StiGroupConditionExpression("{OtherIncome.Sub}");
            page.Components.Add(groupHeader);

            StiText groupHeaderText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.2));
            groupHeaderText.Text.Value = "{OtherIncome.Sub}";
            groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
            groupHeaderText.VertAlignment = StiVertAlignment.Center;
            groupHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
            groupHeaderText.Border.Style = StiPenStyle.None;
            groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
            groupHeaderText.WordWrap = true;
            groupHeader.Components.Add(groupHeaderText);

            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "OtherIncome";
            dataBand.Name = $"OtherIncomeDataSummary";
            dataBand.Border.Style = StiPenStyle.None;
            dataBand.Height = 0;
            page.Components.Add(dataBand);

            pos = (columnWidth * 2);

            foreach (var column in objColumns)
            {
                //Create text on header
                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                hText.Text.Value = column.Label;
                hText.HorAlignment = StiTextHorAlignment.Right;
                hText.VertAlignment = StiVertAlignment.Center;
                hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                hText.Border.Side = StiBorderSides.All;
                hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                hText.Border.Style = StiPenStyle.None;
                hText.TextBrush = new StiSolidBrush(Color.White);
                hText.WordWrap = true;
                hText.CanGrow = true;
                headerBand.Components.Add(hText);

                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));

                if (column.Type != "Variance")
                {
                    dataText.Text.Value = "{Sum(OtherIncome.Column" + column.Index + ")}";
                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    dataText.Text.Value = "{(Sum(OtherIncome.Column" + column.Column1 + ") - Sum(OtherIncome.Column" + column.Column2 + ")) / Abs(Sum(OtherIncome.Column" + column.Column2 + "))}";
                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }

                dataText.HorAlignment = StiTextHorAlignment.Right;
                dataText.VertAlignment = StiVertAlignment.Top;
                dataText.Border.Style = StiPenStyle.None;
                dataText.OnlyText = false;
                dataText.Border.Side = StiBorderSides.All;
                dataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                dataText.WordWrap = true;
                dataText.CanGrow = true;
                dataText.Margins = new StiMargins(0, 1, 4, 0);
                groupHeader.Components.Add(dataText);

                pos = pos + columnWidth;
            }

            //Create group footer band
            StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.4));
            groupFooter.Name = $"OtherIncomeGroupFooterBandSummary";
            groupFooter.Height = 0;
            page.Components.Add(groupFooter);

            //Create DataBand total
            StiDataBand totalBand = new StiDataBand();
            totalBand.DataSourceName = "SummaryTotal";
            totalBand.Name = "OtherIncomeTotalSummary";
            totalBand.Border.Style = StiPenStyle.None;
            totalBand.Filters.Add(new StiFilter());
            totalBand.Filters[0].Item = StiFilterItem.Expression;
            totalBand.Filters[0].Expression = new StiExpression("SummaryTotal.Type == 8");
            page.Components.Add(totalBand);

            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctTotalText.Text.Value = "Total Other Income (Expenses)";
            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
            acctTotalText.VertAlignment = StiVertAlignment.Center;
            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
            acctTotalText.OnlyText = false;
            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctTotalText.WordWrap = true;
            acctTotalText.Margins = new StiMargins(0, 1, 0, 0);
            totalBand.Components.Add(acctTotalText);

            //Create DataBand Income Before Provisions For Income Taxes
            StiDataBand grossBand = new StiDataBand();
            grossBand.DataSourceName = "SummaryTotal";
            grossBand.Name = $"BeforeProvisionsBandSummary";
            grossBand.Border.Style = StiPenStyle.None;
            grossBand.Filters.Add(new StiFilter());
            grossBand.Filters[0].Item = StiFilterItem.Expression;
            grossBand.Filters[0].Expression = new StiExpression("SummaryTotal.Type == 81");
            page.Components.Add(grossBand);

            StiText acctGrossText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
            acctGrossText.Text.Value = "Income Before Provisions For Income Taxes";
            acctGrossText.HorAlignment = StiTextHorAlignment.Left;
            acctGrossText.VertAlignment = StiVertAlignment.Center;
            acctGrossText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
            acctGrossText.OnlyText = false;
            acctGrossText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctGrossText.WordWrap = true;
            acctGrossText.Margins = new StiMargins(0, 1, 0, 10);
            grossBand.Components.Add(acctGrossText);

            pos = (columnWidth * 2);
            foreach (var column in objColumns)
            {
                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                totalText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                if (column.Type != "Variance")
                {
                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }
                totalText.HorAlignment = StiTextHorAlignment.Right;
                totalText.VertAlignment = StiVertAlignment.Center;
                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                totalText.OnlyText = false;
                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                totalText.WordWrap = true;
                totalText.Margins = new StiMargins(0, 1, 0, 0);
                totalBand.Components.Add(totalText);

                StiText grossText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                grossText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                if (column.Type != "Variance")
                {
                    grossText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    grossText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }
                grossText.Type = StiSystemTextType.Expression;
                grossText.HorAlignment = StiTextHorAlignment.Right;
                grossText.VertAlignment = StiVertAlignment.Center;
                grossText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                grossText.OnlyText = false;
                grossText.Font = new Font("Arial", 9F, FontStyle.Bold);
                grossText.WordWrap = true;
                grossText.Margins = new StiMargins(0, 1, 0, 10);
                grossBand.Components.Add(grossText);

                pos = pos + columnWidth;
            }

            // Income Before Provisions For Income Taxes Percen
            StiDataBand percenBand = new StiDataBand();
            percenBand.DataSourceName = "SummaryTotal";
            percenBand.Name = "BeforeProvisionsPercenSummary";
            percenBand.Border.Style = StiPenStyle.None;
            percenBand.Filters.Add(new StiFilter());
            percenBand.Filters[0].Item = StiFilterItem.Expression;
            percenBand.Filters[0].Expression = new StiExpression($"SummaryTotal.Type == 82");
            page.Components.Add(percenBand);

            StiText acctPercenText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctPercenText.Text.Value = "Income Before Provisions For Income Taxes %";
            acctPercenText.HorAlignment = StiTextHorAlignment.Left;
            acctPercenText.VertAlignment = StiVertAlignment.Center;
            acctPercenText.OnlyText = false;
            acctPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctPercenText.WordWrap = true;
            acctPercenText.Margins = new StiMargins(0, 1, 0, 10);
            percenBand.Components.Add(acctPercenText);

            pos = (columnWidth * 2);
            foreach (var column in objColumns)
            {

                StiText depPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                if (column.Type != "Variance")
                {
                    depPercenText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                }
                depPercenText.TextFormat = percenFormat;
                depPercenText.HorAlignment = StiTextHorAlignment.Right;
                depPercenText.VertAlignment = StiVertAlignment.Center;
                depPercenText.OnlyText = false;
                depPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                depPercenText.WordWrap = true;
                depPercenText.Margins = new StiMargins(0, 1, 0, 10);
                percenBand.Components.Add(depPercenText);

                pos = pos + columnWidth;
            }
        }

        // Provisions for Income Taxes
        if (objColumns.Count > 0)
        {
            //Create HeaderBand
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.25;
            headerBand.Name = $"IncomeTaxesHeaderBandSummary";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerBand.PrintOnAllPages = false;
            headerBand.PrintIfEmpty = true;
            page.Components.Add(headerBand);

            StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctText.Text.Value = "Provisions for Income Taxes";
            acctText.HorAlignment = StiTextHorAlignment.Left;
            acctText.VertAlignment = StiVertAlignment.Center;
            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            acctText.Border.Side = StiBorderSides.All;
            acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctText.Border.Style = StiPenStyle.None;
            acctText.TextBrush = new StiSolidBrush(Color.White);
            acctText.WordWrap = true;
            headerBand.Components.Add(acctText);

            //Create group header band
            StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.2));
            groupHeader.Name = "IncomeTaxesGroupHeaderBandSummary";
            groupHeader.PrintOnAllPages = false;
            groupHeader.Condition = new StiGroupConditionExpression("{IncomeTaxes.Sub}");
            page.Components.Add(groupHeader);

            StiText groupHeaderText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.2));
            groupHeaderText.Text.Value = "{IncomeTaxes.Sub}";
            groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
            groupHeaderText.VertAlignment = StiVertAlignment.Center;
            groupHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
            groupHeaderText.Border.Style = StiPenStyle.None;
            groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
            groupHeaderText.WordWrap = true;
            groupHeader.Components.Add(groupHeaderText);

            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "IncomeTaxes";
            dataBand.Name = $"IncomeTaxesDataSummary";
            dataBand.Border.Style = StiPenStyle.None;
            dataBand.Height = 0;
            page.Components.Add(dataBand);

            pos = (columnWidth * 2);

            foreach (var column in objColumns)
            {
                //Create text on header
                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                hText.Text.Value = column.Label;
                hText.HorAlignment = StiTextHorAlignment.Right;
                hText.VertAlignment = StiVertAlignment.Center;
                hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                hText.Border.Side = StiBorderSides.All;
                hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                hText.Border.Style = StiPenStyle.None;
                hText.TextBrush = new StiSolidBrush(Color.White);
                hText.WordWrap = true;
                hText.CanGrow = true;
                headerBand.Components.Add(hText);

                StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));

                if (column.Type != "Variance")
                {
                    dataText.Text.Value = "{Sum(IncomeTaxes.Column" + column.Index + ")}";
                    dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    dataText.Text.Value = "{(Sum(IncomeTaxes.Column" + column.Column1 + ") - Sum(IncomeTaxes.Column" + column.Column2 + ")) / Abs(Sum(IncomeTaxes.Column" + column.Column2 + "))}";
                    dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }

                dataText.HorAlignment = StiTextHorAlignment.Right;
                dataText.VertAlignment = StiVertAlignment.Top;
                dataText.Border.Style = StiPenStyle.None;
                dataText.OnlyText = false;
                dataText.Border.Side = StiBorderSides.All;
                dataText.Font = new Font("Arial", 8F, FontStyle.Bold);
                dataText.WordWrap = true;
                dataText.CanGrow = true;
                dataText.Margins = new StiMargins(0, 1, 4, 0);
                groupHeader.Components.Add(dataText);

                pos = pos + columnWidth;
            }

            //Create group footer band
            StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.4));
            groupFooter.Name = $"IncomeTaxesGroupFooterBandSummary";
            groupFooter.Height = 0;
            page.Components.Add(groupFooter);

            //Create DataBand total
            StiDataBand totalBand = new StiDataBand();
            totalBand.DataSourceName = "SummaryTotal";
            totalBand.Name = "IncomeTaxesTotalSummary";
            totalBand.Border.Style = StiPenStyle.None;
            totalBand.Filters.Add(new StiFilter());
            totalBand.Filters[0].Item = StiFilterItem.Expression;
            totalBand.Filters[0].Expression = new StiExpression($"SummaryTotal.Type == 9");
            page.Components.Add(totalBand);

            StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctTotalText.Text.Value = "Total Provisions for Income Taxes";
            acctTotalText.HorAlignment = StiTextHorAlignment.Left;
            acctTotalText.VertAlignment = StiVertAlignment.Center;
            acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
            acctTotalText.OnlyText = false;
            acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctTotalText.WordWrap = true;
            acctTotalText.Margins = new StiMargins(0, 1, 0, 0);
            totalBand.Components.Add(acctTotalText);

            //Create DataBand Net Income
            StiDataBand netBand = new StiDataBand();
            netBand.DataSourceName = "SummaryTotal";
            netBand.Name = "NetIncomeSummary}";
            netBand.Border.Style = StiPenStyle.None;
            netBand.Filters.Add(new StiFilter());
            netBand.Filters[0].Item = StiFilterItem.Expression;
            netBand.Filters[0].Expression = new StiExpression("SummaryTotal.Type == 91");
            page.Components.Add(netBand);

            StiText acctNetText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
            acctNetText.Text.Value = "Net Income";
            acctNetText.HorAlignment = StiTextHorAlignment.Left;
            acctNetText.VertAlignment = StiVertAlignment.Center;
            acctNetText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
            acctNetText.OnlyText = false;
            acctNetText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctNetText.WordWrap = true;
            acctNetText.Margins = new StiMargins(0, 1, 0, 10);
            netBand.Components.Add(acctNetText);

            pos = (columnWidth * 2);
            foreach (var column in objColumns)
            {
                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                totalText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                if (column.Type != "Variance")
                {
                    totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }
                totalText.HorAlignment = StiTextHorAlignment.Right;
                totalText.VertAlignment = StiVertAlignment.Center;
                totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                totalText.OnlyText = false;
                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                totalText.WordWrap = true;
                totalText.Margins = new StiMargins(0, 1, 0, 0);
                totalBand.Components.Add(totalText);

                StiText netText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                netText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                if (column.Type != "Variance")
                {
                    netText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                }
                else
                {
                    netText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                }
                netText.Type = StiSystemTextType.Expression;
                netText.HorAlignment = StiTextHorAlignment.Right;
                netText.VertAlignment = StiVertAlignment.Center;
                netText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                netText.OnlyText = false;
                netText.Font = new Font("Arial", 9F, FontStyle.Bold);
                netText.WordWrap = true;
                netText.Margins = new StiMargins(0, 1, 0, 10);
                netBand.Components.Add(netText);

                pos = pos + columnWidth;
            }

            // Net Income Percen
            StiDataBand percenBand = new StiDataBand();
            percenBand.DataSourceName = "SummaryTotal";
            percenBand.Name = "NetIncomePercenSummary";
            percenBand.Border.Style = StiPenStyle.None;
            percenBand.Filters.Add(new StiFilter());
            percenBand.Filters[0].Item = StiFilterItem.Expression;
            percenBand.Filters[0].Expression = new StiExpression($"SummaryTotal.Type == 92");
            percenBand.NewPageAfter = true;
            page.Components.Add(percenBand);

            StiText acctPercenText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
            acctPercenText.Text.Value = "Net Income %";
            acctPercenText.HorAlignment = StiTextHorAlignment.Left;
            acctPercenText.VertAlignment = StiVertAlignment.Center;
            acctPercenText.OnlyText = false;
            acctPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctPercenText.WordWrap = true;
            acctPercenText.Margins = new StiMargins(0, 1, 0, 10);
            percenBand.Components.Add(acctPercenText);

            pos = (columnWidth * 2);
            foreach (var column in objColumns)
            {

                StiText depPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                if (column.Type != "Variance")
                {
                    depPercenText.Text.Value = "{SummaryTotal.Column" + column.Index + "}";
                }
                depPercenText.TextFormat = percenFormat;
                depPercenText.HorAlignment = StiTextHorAlignment.Right;
                depPercenText.VertAlignment = StiVertAlignment.Center;
                depPercenText.OnlyText = false;
                depPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                depPercenText.WordWrap = true;
                depPercenText.Margins = new StiMargins(0, 1, 0, 10);
                percenBand.Components.Add(depPercenText);

                pos = pos + columnWidth;
            }
        }
    }

    private void BuildDepartmentSummary(StiPage page, List<ComparativeStatementRequest> objComparative, IEnumerable<Tuple<int, string>> departments)
    {
        var columnWidth = page.Width / (departments.Count() + 3);

        foreach (var col in objComparative)
        {
            var includeData = colIncludeProvisions.FirstOrDefault(x => x.Item1 == col.Index);
            var includeProvisions = false;
            var netText = "Income From Operations"; ;
            
            if (includeData != null)
            {
                includeProvisions = includeData.Item2 || includeData.Item3;
            }

            double pos = 0;

            StiFormatService textFormat;
            if (col.Type != "Variance")
            {
                textFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
            }
            else
            {
                textFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
            }

            StiFormatService currencyFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
            StiFormatService percenFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");

            //Create HeaderBand
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.25;
            headerBand.Name = $"DepartmentSummary{col.Index}";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerBand.PrintOnAllPages = false;
            headerBand.PrintIfEmpty = true;
            headerBand.NewPageBefore = true;
            page.Components.Add(headerBand);

            StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2 - 0, 0.25));
            acctText.Text.Value = "Account";
            acctText.HorAlignment = StiTextHorAlignment.Left;
            acctText.VertAlignment = StiVertAlignment.Center;
            acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            acctText.Border.Side = StiBorderSides.All;
            acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
            acctText.Border.Style = StiPenStyle.None;
            acctText.TextBrush = new StiSolidBrush(Color.White);
            acctText.WordWrap = true;
            headerBand.Components.Add(acctText);

            pos = (columnWidth * 2);

            foreach (var dep in departments)
            {
                //Create text on header
                StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                hText.Text.Value = $"{dep.Item2} {col.Label}";
                hText.HorAlignment = StiTextHorAlignment.Right;
                hText.VertAlignment = StiVertAlignment.Center;
                hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                hText.Border.Side = StiBorderSides.All;
                hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                hText.Border.Style = StiPenStyle.None;
                hText.TextBrush = new StiSolidBrush(Color.White);
                hText.WordWrap = true;
                hText.CanGrow = true;
                headerBand.Components.Add(hText);

                pos = pos + columnWidth;
            }

            // Total header text
            StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
            totalText.Text.Value = $"Total {col.Label}";
            totalText.HorAlignment = StiTextHorAlignment.Right;
            totalText.VertAlignment = StiVertAlignment.Center;
            totalText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            totalText.Border.Side = StiBorderSides.All;
            totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
            totalText.Border.Style = StiPenStyle.None;
            totalText.TextBrush = new StiSolidBrush(Color.White);
            totalText.WordWrap = true;
            headerBand.Components.Add(totalText);

            // Revenues
            if (departments.Count() > 0)
            {
                //Create column header band
                StiColumnHeaderBand colHeader = new StiColumnHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                colHeader.Name = $"RevenuesDepSumColumnHeaderBand{col.Index}";
                colHeader.PrintOnAllPages = false;
                page.Components.Add(colHeader);

                StiText colHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                colHeaderText.Text.Value = "Revenues";
                colHeaderText.HorAlignment = StiTextHorAlignment.Left;
                colHeaderText.VertAlignment = StiVertAlignment.Center;
                colHeaderText.Font = new Font("Arial", 10F, FontStyle.Bold);
                colHeaderText.Border.Style = StiPenStyle.None;
                colHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                colHeaderText.WordWrap = true;
                colHeader.Components.Add(colHeaderText);

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = $"DepSummaryRevenues{col.Index}";
                dataBand.Name = $"RevenuesDepSummaryBand{col.Index}";
                dataBand.Height = 0;
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);

                //Create DataBand item
                StiText acctDataText = new StiText(new RectangleD(0.1, 0, columnWidth * 2 - 0.1, 0.2));
                acctDataText.Text.Value = "{DepSummaryRevenues" + col.Index + ".fDesc}";
                acctDataText.HorAlignment = StiTextHorAlignment.Left;
                acctDataText.VertAlignment = StiVertAlignment.Top;
                acctDataText.Border.Style = StiPenStyle.None;
                acctDataText.OnlyText = false;
                acctDataText.Border.Side = StiBorderSides.All;
                acctDataText.Font = new Font("Arial", 8F);
                acctDataText.WordWrap = true;
                acctDataText.CanGrow = true;
                acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(acctDataText);

                pos = (columnWidth * 2);

                foreach (var dep in departments)
                {
                    StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                    dataText.Text.Value = "{DepSummaryRevenues" + col.Index + ".Col" + col.Index + "_" + dep.Item1 + "}";
                    dataText.TextFormat = textFormat;
                    dataText.HorAlignment = StiTextHorAlignment.Right;
                    dataText.VertAlignment = StiVertAlignment.Top;
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.Font = new Font("Arial", 8F);
                    dataText.WordWrap = true;
                    dataText.CanGrow = true;
                    dataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(dataText);

                    pos = pos + columnWidth;
                }

                //Total column
                StiText totalDataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                totalDataText.Text.Value = "{DepSummaryRevenues" + col.Index + ".Total}";
                totalDataText.TextFormat = textFormat;
                totalDataText.HorAlignment = StiTextHorAlignment.Right;
                totalDataText.VertAlignment = StiVertAlignment.Top;
                totalDataText.Border.Style = StiPenStyle.None;
                totalDataText.OnlyText = false;
                totalDataText.Border.Side = StiBorderSides.All;
                totalDataText.Font = new Font("Arial", 8F);
                totalDataText.WordWrap = true;
                totalDataText.CanGrow = true;
                totalDataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(totalDataText);

                //Create group footer band
                StiColumnFooterBand colFooter = new StiColumnFooterBand(new RectangleD(0, 0, page.Width, 0.4));
                colFooter.Name = $"RevenuesDepSumColumnFooterBand{col.Index}";
                page.Components.Add(colFooter);

                StiText subTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                subTotalText.Text.Value = "Total Revenues";
                subTotalText.HorAlignment = StiTextHorAlignment.Left;
                subTotalText.VertAlignment = StiVertAlignment.Center;
                subTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                subTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                subTotalText.TextBrush = new StiSolidBrush(Color.Black);
                subTotalText.WordWrap = true;
                colFooter.Components.Add(subTotalText);

                pos = (columnWidth * 2);

                foreach (var dep in departments)
                {
                    StiText colFooterText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    colFooterText.HorAlignment = StiTextHorAlignment.Right;
                    colFooterText.VertAlignment = StiVertAlignment.Center;
                    colFooterText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    colFooterText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    colFooterText.TextBrush = new StiSolidBrush(Color.Black);
                    colFooterText.WordWrap = true;
                    if (col.Type != "Variance")
                    {
                        colFooterText.Text.Value = "{Sum(DepSummaryRevenues" + col.Index + ".Col" + col.Index + "_" + dep.Item1 + ")}";
                    }
                    else
                    {
                        var totalRev = _revenuesTotal.Select($"Department = '{dep.Item1}'").FirstOrDefault();
                        colFooterText.Text.Value = totalRev == null ? "0.00%" : Convert.ToDouble(totalRev[$"Column{col.Index}"]).ToString("P2");
                    }
                    colFooterText.TextFormat = textFormat;
                    colFooter.Components.Add(colFooterText);

                    pos = pos + columnWidth;
                }

                StiText totalFooterText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                if (col.Type != "Variance")
                {
                    totalFooterText.Text.Value = "{Sum(DepSummaryRevenues" + col.Index + ".Total)}";
                }
                totalFooterText.HorAlignment = StiTextHorAlignment.Right;
                totalFooterText.VertAlignment = StiVertAlignment.Center;
                totalFooterText.Font = new Font("Arial", 9F, FontStyle.Bold);
                totalFooterText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                totalFooterText.TextBrush = new StiSolidBrush(Color.Black);
                totalFooterText.WordWrap = true;
                totalFooterText.TextFormat = textFormat;
                colFooter.Components.Add(totalFooterText);
            }

            // Cost Of Sales
            if (departments.Count() > 0)
            {
                //Create column header band
                StiColumnHeaderBand colHeader = new StiColumnHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                colHeader.Name = $"CostOfSalesDepSumColumnHeaderBand{col.Index}";
                colHeader.PrintOnAllPages = false;
                page.Components.Add(colHeader);

                StiText colHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                colHeaderText.Text.Value = "Cost Of Sales";
                colHeaderText.HorAlignment = StiTextHorAlignment.Left;
                colHeaderText.VertAlignment = StiVertAlignment.Center;
                colHeaderText.Font = new Font("Arial", 10F, FontStyle.Bold);
                colHeaderText.Border.Style = StiPenStyle.None;
                colHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                colHeaderText.WordWrap = true;
                colHeader.Components.Add(colHeaderText);

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = $"DepSummaryCostOfSales{col.Index}";
                dataBand.Name = $"CostOfSalesDepSummaryBand{col.Index}";
                dataBand.Height = 0;
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);

                //Create DataBand item
                StiText acctDataText = new StiText(new RectangleD(0.1, 0, columnWidth * 2 - 0.1, 0.2));
                acctDataText.Text.Value = "{DepSummaryCostOfSales" + col.Index + ".fDesc}";
                acctDataText.HorAlignment = StiTextHorAlignment.Left;
                acctDataText.VertAlignment = StiVertAlignment.Top;
                acctDataText.Border.Style = StiPenStyle.None;
                acctDataText.OnlyText = false;
                acctDataText.Border.Side = StiBorderSides.All;
                acctDataText.Font = new Font("Arial", 8F);
                acctDataText.WordWrap = true;
                acctDataText.CanGrow = true;
                acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(acctDataText);

                pos = (columnWidth * 2);

                foreach (var dep in departments)
                {
                    StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                    dataText.Text.Value = "{DepSummaryCostOfSales" + col.Index + ".Col" + col.Index + "_" + dep.Item1 + "}";
                    dataText.TextFormat = textFormat;
                    dataText.HorAlignment = StiTextHorAlignment.Right;
                    dataText.VertAlignment = StiVertAlignment.Top;
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.Font = new Font("Arial", 8F);
                    dataText.WordWrap = true;
                    dataText.CanGrow = true;
                    dataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(dataText);

                    pos = pos + columnWidth;
                }

                //Total column
                StiText totalDataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                totalDataText.Text.Value = "{DepSummaryCostOfSales" + col.Index + ".Total}";
                totalDataText.TextFormat = textFormat;
                totalDataText.HorAlignment = StiTextHorAlignment.Right;
                totalDataText.VertAlignment = StiVertAlignment.Top;
                totalDataText.Border.Style = StiPenStyle.None;
                totalDataText.OnlyText = false;
                totalDataText.Border.Side = StiBorderSides.All;
                totalDataText.Font = new Font("Arial", 8F);
                totalDataText.WordWrap = true;
                totalDataText.CanGrow = true;
                totalDataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(totalDataText);

                //Create group footer band
                StiColumnFooterBand colFooter = new StiColumnFooterBand(new RectangleD(0, 0, page.Width, 0.6));
                colFooter.Name = $"CostOfSalesDepSumColumnFooterBand{col.Index}";
                page.Components.Add(colFooter);

                StiText subTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                subTotalText.Text.Value = "Total Cost Of Sales";
                subTotalText.HorAlignment = StiTextHorAlignment.Left;
                subTotalText.VertAlignment = StiVertAlignment.Center;
                subTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                subTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                subTotalText.TextBrush = new StiSolidBrush(Color.Black);
                subTotalText.WordWrap = true;
                colFooter.Components.Add(subTotalText);

                StiText grossProfitText = new StiText(new RectangleD(0, 0.25, columnWidth * 2, 0.25));
                grossProfitText.Text.Value = "Gross Profit";
                grossProfitText.HorAlignment = StiTextHorAlignment.Left;
                grossProfitText.VertAlignment = StiVertAlignment.Center;
                grossProfitText.Font = new Font("Arial", 9F, FontStyle.Bold);
                grossProfitText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                grossProfitText.TextBrush = new StiSolidBrush(Color.Black);
                grossProfitText.WordWrap = true;
                colFooter.Components.Add(grossProfitText);

                pos = (columnWidth * 2);
                foreach (var dep in departments)
                {
                    StiText colFooterText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    colFooterText.HorAlignment = StiTextHorAlignment.Right;
                    colFooterText.VertAlignment = StiVertAlignment.Center;
                    colFooterText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    colFooterText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    colFooterText.TextBrush = new StiSolidBrush(Color.Black);
                    colFooterText.WordWrap = true;
                    if (col.Type != "Variance")
                    {
                        colFooterText.Text.Value = "{Sum(DepSummaryCostOfSales" + col.Index + ".Col" + col.Index + "_" + dep.Item1 + ")}";
                    }
                    else
                    {
                        var totalCost = _costOfSalesTotal.Select($"Department = '{dep.Item1}'").FirstOrDefault();
                        colFooterText.Text.Value = totalCost == null ? "0.00%" : Convert.ToDouble(totalCost[$"Column{col.Index}"]).ToString("P2");
                    }
                    colFooterText.TextFormat = textFormat;
                    colFooter.Components.Add(colFooterText);

                    StiText grossProfitData = new StiText(new RectangleD(pos, 0.25, columnWidth, 0.25));
                    grossProfitData.HorAlignment = StiTextHorAlignment.Right;
                    grossProfitData.VertAlignment = StiVertAlignment.Center;
                    grossProfitData.Font = new Font("Arial", 9F, FontStyle.Bold);
                    grossProfitData.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    grossProfitData.TextBrush = new StiSolidBrush(Color.Black);
                    grossProfitData.WordWrap = true;
                    if (col.Type != "Variance")
                    {
                        grossProfitData.Text.Value = "{Sum(RevenuesDepSummaryBand" + col.Index + ",DepSummaryRevenues" + col.Index + ".Col" + col.Index + "_" + dep.Item1 + ") - Sum(CostOfSalesDepSummaryBand" + col.Index + ",DepSummaryCostOfSales" + col.Index + ".Col" + col.Index + "_" + dep.Item1 + ")}";
                    }
                    else
                    {
                        var grossProfit = _grossProfit.Select($"Department = '{dep.Item1}'").FirstOrDefault();
                        grossProfitData.Text.Value = grossProfit == null ? "0.00%" : Convert.ToDouble(grossProfit[$"Column{col.Index}"]).ToString("P2");
                    }
                    grossProfitData.TextFormat = textFormat;
                    colFooter.Components.Add(grossProfitData);

                    pos = pos + columnWidth;
                }

                StiText totalFooterText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                if (col.Type != "Variance")
                {
                    totalFooterText.Text.Value = "{Sum(DepSummaryCostOfSales" + col.Index + ".Total)}";
                }
                totalFooterText.HorAlignment = StiTextHorAlignment.Right;
                totalFooterText.VertAlignment = StiVertAlignment.Center;
                totalFooterText.Font = new Font("Arial", 9F, FontStyle.Bold);
                totalFooterText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                totalFooterText.TextBrush = new StiSolidBrush(Color.Black);
                totalFooterText.WordWrap = true;
                totalFooterText.TextFormat = textFormat;
                colFooter.Components.Add(totalFooterText);

                StiText totalGrossProfit = new StiText(new RectangleD(pos, 0.25, columnWidth, 0.25));
                if (col.Type != "Variance")
                {
                    totalGrossProfit.Text.Value = "{Sum(RevenuesDepSummaryBand" + col.Index + ",DepSummaryRevenues" + col.Index + ".Total) - Sum(CostOfSalesDepSummaryBand" + col.Index + ",DepSummaryCostOfSales" + col.Index + ".Total)}";
                }
                totalGrossProfit.HorAlignment = StiTextHorAlignment.Right;
                totalGrossProfit.VertAlignment = StiVertAlignment.Center;
                totalGrossProfit.Font = new Font("Arial", 9F, FontStyle.Bold);
                totalGrossProfit.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                totalGrossProfit.TextBrush = new StiSolidBrush(Color.Black);
                totalGrossProfit.WordWrap = true;
                totalGrossProfit.TextFormat = textFormat;
                colFooter.Components.Add(totalGrossProfit);

                // Gross Profit Percen
                if (col.Type != "Variance")
                {
                    //Create DataBand total
                    StiDataBand percenBand = new StiDataBand();
                    percenBand.DataSourceName = $"Consolidating{col.Index}";
                    percenBand.Name = $"GrossPercenConsolidatingDivisional";
                    percenBand.Border.Style = StiPenStyle.None;
                    percenBand.Filters.Add(new StiFilter());
                    percenBand.Filters[0].Item = StiFilterItem.Expression;
                    percenBand.Filters[0].Expression = new StiExpression($"Consolidating{col.Index}.Type == 42");
                    page.Components.Add(percenBand);

                    StiText acctPercenText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                    acctPercenText.Text.Value = "{Consolidating" + col.Index + ".fDesc}";
                    acctPercenText.HorAlignment = StiTextHorAlignment.Left;
                    acctPercenText.VertAlignment = StiVertAlignment.Center;
                    acctPercenText.OnlyText = false;
                    acctPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    acctPercenText.WordWrap = true;
                    acctPercenText.Margins = new StiMargins(0, 1, 0, 10);
                    percenBand.Components.Add(acctPercenText);

                    pos = (columnWidth * 2);
                    foreach (var dep in departments)
                    {
                        StiText depPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        depPercenText.Text.Value = "{Consolidating" + col.Index + ".Col" + dep.Item1 + "}";
                        depPercenText.TextFormat = percenFormat;
                        depPercenText.HorAlignment = StiTextHorAlignment.Right;
                        depPercenText.VertAlignment = StiVertAlignment.Center;
                        depPercenText.OnlyText = false;
                        depPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                        depPercenText.WordWrap = true;
                        depPercenText.Margins = new StiMargins(0, 1, 0, 10);
                        percenBand.Components.Add(depPercenText);

                        pos = pos + columnWidth;
                    }

                    //Total column
                    StiText totalPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    totalPercenText.Text.Value = "{Consolidating" + col.Index + ".Total}";
                    totalPercenText.TextFormat = percenFormat;
                    totalPercenText.HorAlignment = StiTextHorAlignment.Right;
                    totalPercenText.VertAlignment = StiVertAlignment.Top;
                    totalPercenText.Border.Style = StiPenStyle.None;
                    totalPercenText.OnlyText = false;
                    totalPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    totalPercenText.WordWrap = true;
                    totalPercenText.CanGrow = true;
                    totalPercenText.Margins = new StiMargins(0, 1, 4, 0);
                    percenBand.Components.Add(totalPercenText);
                }
            }

            // Expenses
            if (departments.Count() > 0)
            {
                //Create column header band
                StiColumnHeaderBand colHeader = new StiColumnHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                colHeader.Name = $"ExpensesDepSumColumnHeaderBand{col.Index}";
                colHeader.PrintOnAllPages = false;
                page.Components.Add(colHeader);

                StiText colHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                colHeaderText.Text.Value = "Expenses";
                colHeaderText.HorAlignment = StiTextHorAlignment.Left;
                colHeaderText.VertAlignment = StiVertAlignment.Center;
                colHeaderText.Font = new Font("Arial", 10F, FontStyle.Bold);
                colHeaderText.Border.Style = StiPenStyle.None;
                colHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                colHeaderText.WordWrap = true;
                colHeader.Components.Add(colHeaderText);

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = $"DepSummaryExpenses{col.Index}";
                dataBand.Name = $"ExpensesDepSummaryBand{col.Index}";
                dataBand.Height = 0;
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);

                //Create DataBand item
                StiText acctDataText = new StiText(new RectangleD(0.1, 0, columnWidth * 2 - 0.1, 0.2));
                acctDataText.Text.Value = "{DepSummaryExpenses" + col.Index + ".fDesc}";
                acctDataText.HorAlignment = StiTextHorAlignment.Left;
                acctDataText.VertAlignment = StiVertAlignment.Top;
                acctDataText.Border.Style = StiPenStyle.None;
                acctDataText.OnlyText = false;
                acctDataText.Border.Side = StiBorderSides.All;
                acctDataText.Font = new Font("Arial", 8F);
                acctDataText.WordWrap = true;
                acctDataText.CanGrow = true;
                acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(acctDataText);

                pos = (columnWidth * 2);

                foreach (var dep in departments)
                {
                    StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                    dataText.Text.Value = "{DepSummaryExpenses" + col.Index + ".Col" + col.Index + "_" + dep.Item1 + "}";
                    dataText.TextFormat = textFormat;
                    dataText.HorAlignment = StiTextHorAlignment.Right;
                    dataText.VertAlignment = StiVertAlignment.Top;
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.Font = new Font("Arial", 8F);
                    dataText.WordWrap = true;
                    dataText.CanGrow = true;
                    dataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(dataText);

                    pos = pos + columnWidth;
                }

                //Total column
                StiText totalDataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                totalDataText.Text.Value = "{DepSummaryExpenses" + col.Index + ".Total}";
                totalDataText.TextFormat = textFormat;
                totalDataText.HorAlignment = StiTextHorAlignment.Right;
                totalDataText.VertAlignment = StiVertAlignment.Top;
                totalDataText.Border.Style = StiPenStyle.None;
                totalDataText.OnlyText = false;
                totalDataText.Border.Side = StiBorderSides.All;
                totalDataText.Font = new Font("Arial", 8F);
                totalDataText.WordWrap = true;
                totalDataText.CanGrow = true;
                totalDataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(totalDataText);

                //Create group footer band
                StiColumnFooterBand colFooter = new StiColumnFooterBand(new RectangleD(0, 0, page.Width, 0.6));
                colFooter.Name = $"ExpensesDepSumColumnFooterBand{col.Index}";
                if (col.Type == "Variance")
                {
                    colFooter.NewPageAfter = true;
                }
                page.Components.Add(colFooter);

                StiText subTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                subTotalText.Text.Value = "Total Expenses";
                subTotalText.HorAlignment = StiTextHorAlignment.Left;
                subTotalText.VertAlignment = StiVertAlignment.Center;
                subTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                subTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                subTotalText.TextBrush = new StiSolidBrush(Color.Black);
                subTotalText.WordWrap = true;
                colFooter.Components.Add(subTotalText);

                StiText netProfitText = new StiText(new RectangleD(0, 0.25, columnWidth * 2, 0.25));
                netProfitText.Text.Value = netText;
                netProfitText.HorAlignment = StiTextHorAlignment.Left;
                netProfitText.VertAlignment = StiVertAlignment.Center;
                netProfitText.Font = new Font("Arial", 9F, FontStyle.Bold);
                netProfitText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                netProfitText.TextBrush = new StiSolidBrush(Color.Black);
                netProfitText.WordWrap = true;
                colFooter.Components.Add(netProfitText);

                pos = (columnWidth * 2);
                foreach (var dep in departments)
                {
                    StiText colFooterText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    colFooterText.HorAlignment = StiTextHorAlignment.Right;
                    colFooterText.VertAlignment = StiVertAlignment.Center;
                    colFooterText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    colFooterText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    colFooterText.TextBrush = new StiSolidBrush(Color.Black);
                    colFooterText.WordWrap = true;
                    if (col.Type != "Variance")
                    {
                        colFooterText.Text.Value = "{Sum(DepSummaryExpenses" + col.Index + ".Col" + col.Index + "_" + dep.Item1 + ")}";
                    }
                    else
                    {
                        var totalExp = _expensesTotal.Select($"Department = '{dep.Item1}'").FirstOrDefault();
                        colFooterText.Text.Value = totalExp == null ? "0.00%" : Convert.ToDouble(totalExp[$"Column{col.Index}"]).ToString("P2");
                    }
                    colFooterText.TextFormat = textFormat;
                    colFooter.Components.Add(colFooterText);

                    StiText netProfitData = new StiText(new RectangleD(pos, 0.25, columnWidth, 0.25));
                    netProfitData.HorAlignment = StiTextHorAlignment.Right;
                    netProfitData.VertAlignment = StiVertAlignment.Center;
                    netProfitData.Font = new Font("Arial", 9F, FontStyle.Bold);
                    netProfitData.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    netProfitData.TextBrush = new StiSolidBrush(Color.Black);
                    netProfitData.WordWrap = true;
                    if (col.Type != "Variance")
                    {
                        netProfitData.Text.Value = "{Sum(RevenuesDepSummaryBand" + col.Index + ",DepSummaryRevenues" + col.Index + ".Col" + col.Index + "_" + dep.Item1 + ") - Sum(CostOfSalesDepSummaryBand" + col.Index + ",DepSummaryCostOfSales" + col.Index + ".Col" + col.Index + "_" + dep.Item1 + ") - Sum(ExpensesDepSummaryBand" + col.Index + ",DepSummaryExpenses" + col.Index + ".Col" + col.Index + "_" + dep.Item1 + ")}";
                    }
                    else
                    {
                        var netProfit = _netProfit.Select($"Department = '{dep.Item1}'").FirstOrDefault();
                        netProfitData.Text = netProfit == null ? "0.00%" : Convert.ToDouble(netProfit[$"Column{col.Index}"]).ToString("P2");
                    }
                    netProfitData.TextFormat = textFormat;
                    colFooter.Components.Add(netProfitData);

                    pos = pos + columnWidth;
                }

                StiText totalFooterText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                if (col.Type != "Variance")
                {
                    totalFooterText.Text.Value = "{Sum(DepSummaryExpenses" + col.Index + ".Total)}";
                }
                totalFooterText.HorAlignment = StiTextHorAlignment.Right;
                totalFooterText.VertAlignment = StiVertAlignment.Center;
                totalFooterText.Font = new Font("Arial", 9F, FontStyle.Bold);
                totalFooterText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                totalFooterText.TextBrush = new StiSolidBrush(Color.Black);
                totalFooterText.WordWrap = true;
                totalFooterText.TextFormat = textFormat;
                colFooter.Components.Add(totalFooterText);

                StiText totalNetProfit = new StiText(new RectangleD(pos, 0.25, columnWidth, 0.25));
                if (col.Type != "Variance")
                {
                    totalNetProfit.Text.Value = "{Sum(RevenuesDepSummaryBand" + col.Index + ",DepSummaryRevenues" + col.Index + ".Total) - Sum(CostOfSalesDepSummaryBand" + col.Index + ",DepSummaryCostOfSales" + col.Index + ".Total) - Sum(ExpensesDepSummaryBand" + col.Index + ",DepSummaryExpenses" + col.Index + ".Total)}";
                }
                totalNetProfit.HorAlignment = StiTextHorAlignment.Right;
                totalNetProfit.VertAlignment = StiVertAlignment.Center;
                totalNetProfit.Font = new Font("Arial", 9F, FontStyle.Bold);
                totalNetProfit.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                totalNetProfit.TextBrush = new StiSolidBrush(Color.Black);
                totalNetProfit.WordWrap = true;
                totalNetProfit.TextFormat = textFormat;
                colFooter.Components.Add(totalNetProfit);

                // Net Income Percen
                if (col.Type != "Variance")
                {
                    //Create DataBand total
                    StiDataBand netPercenBand = new StiDataBand();
                    netPercenBand.DataSourceName = $"Consolidating{col.Index}";
                    netPercenBand.Name = $"NetPercenConsolidatingDivisional";
                    netPercenBand.Border.Style = StiPenStyle.None;
                    netPercenBand.Filters.Add(new StiFilter());
                    netPercenBand.Filters[0].Item = StiFilterItem.Expression;
                    netPercenBand.Filters[0].Expression = new StiExpression($"Consolidating{col.Index}.Type == 52");
                    netPercenBand.NewPageAfter = !includeProvisions;
                    page.Components.Add(netPercenBand);

                    StiText acctPercenText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                    acctPercenText.Text.Value = $"{netText} %";
                    acctPercenText.HorAlignment = StiTextHorAlignment.Left;
                    acctPercenText.VertAlignment = StiVertAlignment.Center;
                    acctPercenText.OnlyText = false;
                    acctPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    acctPercenText.WordWrap = true;
                    acctPercenText.Margins = new StiMargins(0, 1, 0, 10);
                    netPercenBand.Components.Add(acctPercenText);

                    pos = (columnWidth * 2);
                    foreach (var dep in departments)
                    {
                        StiText depPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        depPercenText.Text.Value = "{Consolidating" + col.Index + ".Col" + dep.Item1 + "}";
                        depPercenText.TextFormat = percenFormat;
                        depPercenText.HorAlignment = StiTextHorAlignment.Right;
                        depPercenText.VertAlignment = StiVertAlignment.Center;
                        depPercenText.OnlyText = false;
                        depPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                        depPercenText.WordWrap = true;
                        depPercenText.Margins = new StiMargins(0, 1, 0, 10);
                        netPercenBand.Components.Add(depPercenText);

                        pos = pos + columnWidth;
                    }

                    //Total column
                    StiText totalPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    totalPercenText.Text.Value = "{Consolidating" + col.Index + ".Total}";
                    totalPercenText.TextFormat = percenFormat;
                    totalPercenText.HorAlignment = StiTextHorAlignment.Right;
                    totalPercenText.VertAlignment = StiVertAlignment.Top;
                    totalPercenText.Border.Style = StiPenStyle.None;
                    totalPercenText.OnlyText = false;
                    totalPercenText.Border.Side = StiBorderSides.All;
                    totalPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    totalPercenText.WordWrap = true;
                    totalPercenText.CanGrow = true;
                    totalPercenText.Margins = new StiMargins(0, 1, 4, 0);
                    netPercenBand.Components.Add(totalPercenText);
                }
            }

            // Other Income (Expenses)
            if (departments.Count() > 0 && includeData.Item2)
            {
                //Create column header band
                StiColumnHeaderBand colHeader = new StiColumnHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                colHeader.Name = $"OtherIncomeDepSumColumnHeaderBand{col.Index}";
                colHeader.PrintOnAllPages = false;
                page.Components.Add(colHeader);

                StiText colHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                colHeaderText.Text.Value = "Other Income (Expenses)";
                colHeaderText.HorAlignment = StiTextHorAlignment.Left;
                colHeaderText.VertAlignment = StiVertAlignment.Center;
                colHeaderText.Font = new Font("Arial", 10F, FontStyle.Bold);
                colHeaderText.Border.Style = StiPenStyle.None;
                colHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                colHeaderText.WordWrap = true;
                colHeader.Components.Add(colHeaderText);

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = $"DepSummaryOtherIncome{col.Index}";
                dataBand.Name = $"OtherIncomeDepSummaryBand{col.Index}";
                dataBand.Height = 0;
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);

                //Create DataBand item
                StiText acctDataText = new StiText(new RectangleD(0.1, 0, columnWidth * 2 - 0.1, 0.2));
                acctDataText.Text.Value = "{DepSummaryOtherIncome" + col.Index + ".fDesc}";
                acctDataText.HorAlignment = StiTextHorAlignment.Left;
                acctDataText.VertAlignment = StiVertAlignment.Top;
                acctDataText.Border.Style = StiPenStyle.None;
                acctDataText.OnlyText = false;
                acctDataText.Border.Side = StiBorderSides.All;
                acctDataText.Font = new Font("Arial", 8F);
                acctDataText.WordWrap = true;
                acctDataText.CanGrow = true;
                acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(acctDataText);

                pos = (columnWidth * 2);

                foreach (var dep in departments)
                {
                    StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                    dataText.Text.Value = "{DepSummaryOtherIncome" + col.Index + ".Col" + col.Index + "_" + dep.Item1 + "}";
                    dataText.TextFormat = textFormat;
                    dataText.HorAlignment = StiTextHorAlignment.Right;
                    dataText.VertAlignment = StiVertAlignment.Top;
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.Font = new Font("Arial", 8F);
                    dataText.WordWrap = true;
                    dataText.CanGrow = true;
                    dataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(dataText);

                    pos = pos + columnWidth;
                }

                //Total column
                StiText totalDataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                totalDataText.Text.Value = "{DepSummaryOtherIncome" + col.Index + ".Total}";
                totalDataText.TextFormat = textFormat;
                totalDataText.HorAlignment = StiTextHorAlignment.Right;
                totalDataText.VertAlignment = StiVertAlignment.Top;
                totalDataText.Border.Style = StiPenStyle.None;
                totalDataText.OnlyText = false;
                totalDataText.Border.Side = StiBorderSides.All;
                totalDataText.Font = new Font("Arial", 8F);
                totalDataText.WordWrap = true;
                totalDataText.CanGrow = true;
                totalDataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(totalDataText);

                //Create group footer band
                StiColumnFooterBand colFooter = new StiColumnFooterBand(new RectangleD(0, 0, page.Width, 0.6));
                colFooter.Name = $"OtherIncomeDepSumColumnFooterBand{col.Index}";
                page.Components.Add(colFooter);

                StiText subTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                subTotalText.Text.Value = "Total Other Income (Expenses)";
                subTotalText.HorAlignment = StiTextHorAlignment.Left;
                subTotalText.VertAlignment = StiVertAlignment.Center;
                subTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                subTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                subTotalText.TextBrush = new StiSolidBrush(Color.Black);
                subTotalText.WordWrap = true;
                colFooter.Components.Add(subTotalText);

                StiText grossProfitText = new StiText(new RectangleD(0, 0.25, columnWidth * 2, 0.25));
                grossProfitText.Text.Value = "Income Before Provisions For Income Taxes";
                grossProfitText.HorAlignment = StiTextHorAlignment.Left;
                grossProfitText.VertAlignment = StiVertAlignment.Center;
                grossProfitText.Font = new Font("Arial", 9F, FontStyle.Bold);
                grossProfitText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                grossProfitText.TextBrush = new StiSolidBrush(Color.Black);
                grossProfitText.WordWrap = true;
                colFooter.Components.Add(grossProfitText);

                pos = (columnWidth * 2);
                foreach (var dep in departments)
                {
                    StiText colFooterText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    colFooterText.HorAlignment = StiTextHorAlignment.Right;
                    colFooterText.VertAlignment = StiVertAlignment.Center;
                    colFooterText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    colFooterText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    colFooterText.TextBrush = new StiSolidBrush(Color.Black);
                    colFooterText.WordWrap = true;
                    if (col.Type != "Variance")
                    {
                        colFooterText.Text.Value = "{Sum(DepSummaryOtherIncome" + col.Index + ".Col" + col.Index + "_" + dep.Item1 + ")}";
                    }
                    else
                    {
                        var totalOther = _otherIncomeTotal.Select($"Department = '{dep.Item1}'").FirstOrDefault();
                        colFooterText.Text.Value = totalOther == null ? "0.00%" : Convert.ToDouble(totalOther[$"Column{col.Index}"]).ToString("P2");
                    }
                    colFooterText.TextFormat = textFormat;
                    colFooter.Components.Add(colFooterText);

                    StiText beforeProvisionsData = new StiText(new RectangleD(pos, 0.25, columnWidth, 0.25));
                    beforeProvisionsData.HorAlignment = StiTextHorAlignment.Right;
                    beforeProvisionsData.VertAlignment = StiVertAlignment.Center;
                    beforeProvisionsData.Font = new Font("Arial", 9F, FontStyle.Bold);
                    beforeProvisionsData.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    beforeProvisionsData.TextBrush = new StiSolidBrush(Color.Black);
                    beforeProvisionsData.WordWrap = true;
                    if (col.Type != "Variance")
                    {
                        beforeProvisionsData.Text.Value = "{Sum(RevenuesDepSummaryBand" + col.Index + ",DepSummaryRevenues" + col.Index + ".Col" + col.Index + "_" + dep.Item1
                            + ") - Sum(CostOfSalesDepSummaryBand" + col.Index + ",DepSummaryCostOfSales" + col.Index + ".Col" + col.Index + "_" + dep.Item1
                            + ") - Sum(ExpensesDepSummaryBand" + col.Index + ",DepSummaryExpenses" + col.Index + ".Col" + col.Index + "_" + dep.Item1 
                            + ") + Sum(OtherIncomeDepSummaryBand" + col.Index + ",DepSummaryOtherIncome" + col.Index + ".Col" + col.Index + "_" + dep.Item1 + ")}";
                }
                    else
                    {
                        var beforeProvisions = _beforeProvisions.Select($"Department = '{dep.Item1}'").FirstOrDefault();
                        beforeProvisionsData.Text.Value = beforeProvisions == null ? "0.00%" : Convert.ToDouble(beforeProvisions[$"Column{col.Index}"]).ToString("P2");
                    }
                    beforeProvisionsData.TextFormat = textFormat;
                    colFooter.Components.Add(beforeProvisionsData);

                    pos = pos + columnWidth;
                }

                StiText totalFooterText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                if (col.Type != "Variance")
                {
                    totalFooterText.Text.Value = "{Sum(DepSummaryOtherIncome" + col.Index + ".Total)}";
                }
                totalFooterText.HorAlignment = StiTextHorAlignment.Right;
                totalFooterText.VertAlignment = StiVertAlignment.Center;
                totalFooterText.Font = new Font("Arial", 9F, FontStyle.Bold);
                totalFooterText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                totalFooterText.TextBrush = new StiSolidBrush(Color.Black);
                totalFooterText.WordWrap = true;
                totalFooterText.TextFormat = textFormat;
                colFooter.Components.Add(totalFooterText);

                StiText totalBeforeProvisions = new StiText(new RectangleD(pos, 0.25, columnWidth, 0.25));
                if (col.Type != "Variance")
                {
                    totalBeforeProvisions.Text.Value = "{Sum(RevenuesDepSummaryBand" + col.Index + ",DepSummaryRevenues" 
                        + col.Index + ".Total) - Sum(CostOfSalesDepSummaryBand" + col.Index + ",DepSummaryCostOfSales" 
                        + col.Index + ".Total) - Sum(ExpensesDepSummaryBand" + col.Index + ",DepSummaryExpenses" 
                        + col.Index + ".Total) + Sum(OtherIncomeDepSummaryBand" + col.Index + ",DepSummaryOtherIncome"
                        + col.Index + ".Total)}";
                }

                totalBeforeProvisions.HorAlignment = StiTextHorAlignment.Right;
                totalBeforeProvisions.VertAlignment = StiVertAlignment.Center;
                totalBeforeProvisions.Font = new Font("Arial", 9F, FontStyle.Bold);
                totalBeforeProvisions.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                totalBeforeProvisions.TextBrush = new StiSolidBrush(Color.Black);
                totalBeforeProvisions.WordWrap = true;
                totalBeforeProvisions.TextFormat = textFormat;
                colFooter.Components.Add(totalBeforeProvisions);

                // Income Before Provisions For Income Taxes Percen
                if (col.Type != "Variance")
                {
                    //Create DataBand total
                    StiDataBand percenBand = new StiDataBand();
                    percenBand.DataSourceName = $"Consolidating{col.Index}";
                    percenBand.Name = $"BeforeProvisionsPercenConsolidatingDivisional";
                    percenBand.Border.Style = StiPenStyle.None;
                    percenBand.Filters.Add(new StiFilter());
                    percenBand.Filters[0].Item = StiFilterItem.Expression;
                    percenBand.Filters[0].Expression = new StiExpression($"Consolidating{col.Index}.Type == 82");
                    page.Components.Add(percenBand);

                    StiText acctPercenText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                    acctPercenText.Text.Value = "{Consolidating" + col.Index + ".fDesc}";
                    acctPercenText.HorAlignment = StiTextHorAlignment.Left;
                    acctPercenText.VertAlignment = StiVertAlignment.Center;
                    acctPercenText.OnlyText = false;
                    acctPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    acctPercenText.WordWrap = true;
                    acctPercenText.Margins = new StiMargins(0, 1, 0, 10);
                    percenBand.Components.Add(acctPercenText);

                    pos = (columnWidth * 2);
                    foreach (var dep in departments)
                    {
                        StiText depPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        depPercenText.Text.Value = "{Consolidating" + col.Index + ".Col" + dep.Item1 + "}";
                        depPercenText.TextFormat = percenFormat;
                        depPercenText.HorAlignment = StiTextHorAlignment.Right;
                        depPercenText.VertAlignment = StiVertAlignment.Center;
                        depPercenText.OnlyText = false;
                        depPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                        depPercenText.WordWrap = true;
                        depPercenText.Margins = new StiMargins(0, 1, 0, 10);
                        percenBand.Components.Add(depPercenText);

                        pos = pos + columnWidth;
                    }

                    //Total column
                    StiText totalPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    totalPercenText.Text.Value = "{Consolidating" + col.Index + ".Total}";
                    totalPercenText.TextFormat = percenFormat;
                    totalPercenText.HorAlignment = StiTextHorAlignment.Right;
                    totalPercenText.VertAlignment = StiVertAlignment.Top;
                    totalPercenText.Border.Style = StiPenStyle.None;
                    totalPercenText.OnlyText = false;
                    totalPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    totalPercenText.WordWrap = true;
                    totalPercenText.CanGrow = true;
                    totalPercenText.Margins = new StiMargins(0, 1, 4, 0);
                    percenBand.Components.Add(totalPercenText);
                }
            }

            // Provisions for Income Taxes
            if (departments.Count() > 0 && (includeData.Item2 || includeData.Item3)) 
            {
                //Create column header band
                StiColumnHeaderBand colHeader = new StiColumnHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                colHeader.Name = $"IncomeTaxesDepSumColumnHeaderBand{col.Index}";
                colHeader.PrintOnAllPages = false;
                page.Components.Add(colHeader);

                StiText colHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                colHeaderText.Text.Value = "Provisions for Income Taxes";
                colHeaderText.HorAlignment = StiTextHorAlignment.Left;
                colHeaderText.VertAlignment = StiVertAlignment.Center;
                colHeaderText.Font = new Font("Arial", 10F, FontStyle.Bold);
                colHeaderText.Border.Style = StiPenStyle.None;
                colHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                colHeaderText.WordWrap = true;
                colHeader.Components.Add(colHeaderText);

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = $"DepSummaryIncomeTaxes{col.Index}";
                dataBand.Name = $"IncomeTaxesDepSummaryBand{col.Index}";
                dataBand.Height = 0;
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);

                //Create DataBand item
                StiText acctDataText = new StiText(new RectangleD(0.1, 0, columnWidth * 2 - 0.1, 0.2));
                acctDataText.Text.Value = "{DepSummaryIncomeTaxes" + col.Index + ".fDesc}";
                acctDataText.HorAlignment = StiTextHorAlignment.Left;
                acctDataText.VertAlignment = StiVertAlignment.Top;
                acctDataText.Border.Style = StiPenStyle.None;
                acctDataText.OnlyText = false;
                acctDataText.Border.Side = StiBorderSides.All;
                acctDataText.Font = new Font("Arial", 8F);
                acctDataText.WordWrap = true;
                acctDataText.CanGrow = true;
                acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(acctDataText);

                pos = (columnWidth * 2);

                foreach (var dep in departments)
                {
                    StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                    dataText.Text.Value = "{DepSummaryIncomeTaxes" + col.Index + ".Col" + col.Index + "_" + dep.Item1 + "}";
                    dataText.TextFormat = textFormat;
                    dataText.HorAlignment = StiTextHorAlignment.Right;
                    dataText.VertAlignment = StiVertAlignment.Top;
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.Font = new Font("Arial", 8F);
                    dataText.WordWrap = true;
                    dataText.CanGrow = true;
                    dataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(dataText);

                    pos = pos + columnWidth;
                }

                //Total column
                StiText totalDataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                totalDataText.Text.Value = "{DepSummaryIncomeTaxes" + col.Index + ".Total}";
                totalDataText.TextFormat = textFormat;
                totalDataText.HorAlignment = StiTextHorAlignment.Right;
                totalDataText.VertAlignment = StiVertAlignment.Top;
                totalDataText.Border.Style = StiPenStyle.None;
                totalDataText.OnlyText = false;
                totalDataText.Border.Side = StiBorderSides.All;
                totalDataText.Font = new Font("Arial", 8F);
                totalDataText.WordWrap = true;
                totalDataText.CanGrow = true;
                totalDataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(totalDataText);

                //Create group footer band
                StiColumnFooterBand colFooter = new StiColumnFooterBand(new RectangleD(0, 0, page.Width, 0.6));
                colFooter.Name = $"IncomeTaxesDepSumColumnFooterBand{col.Index}";
                if (col.Type == "Variance")
                {
                    colFooter.NewPageAfter = true;
                }
                page.Components.Add(colFooter);

                StiText subTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                subTotalText.Text.Value = "Total Provisions for Income Taxes";
                subTotalText.HorAlignment = StiTextHorAlignment.Left;
                subTotalText.VertAlignment = StiVertAlignment.Center;
                subTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                subTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                subTotalText.TextBrush = new StiSolidBrush(Color.Black);
                subTotalText.WordWrap = true;
                colFooter.Components.Add(subTotalText);

                StiText netProfitText = new StiText(new RectangleD(0, 0.25, columnWidth * 2, 0.25));
                netProfitText.Text.Value = "Net Income";
                netProfitText.HorAlignment = StiTextHorAlignment.Left;
                netProfitText.VertAlignment = StiVertAlignment.Center;
                netProfitText.Font = new Font("Arial", 9F, FontStyle.Bold);
                netProfitText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                netProfitText.TextBrush = new StiSolidBrush(Color.Black);
                netProfitText.WordWrap = true;
                colFooter.Components.Add(netProfitText);

                pos = (columnWidth * 2);
                foreach (var dep in departments)
                {
                    StiText colFooterText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    colFooterText.HorAlignment = StiTextHorAlignment.Right;
                    colFooterText.VertAlignment = StiVertAlignment.Center;
                    colFooterText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    colFooterText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    colFooterText.TextBrush = new StiSolidBrush(Color.Black);
                    colFooterText.WordWrap = true;
                    if (col.Type != "Variance")
                    {
                        colFooterText.Text.Value = "{Sum(DepSummaryIncomeTaxes" + col.Index + ".Col" + col.Index + "_" + dep.Item1 + ")}";
                    }
                    else
                    {
                        var totalExp = _incomeTaxesTotal.Select($"Department = '{dep.Item1}'").FirstOrDefault();
                        colFooterText.Text.Value = totalExp == null ? "0.00%" : Convert.ToDouble(totalExp[$"Column{col.Index}"]).ToString("P2");
                    }
                    colFooterText.TextFormat = textFormat;
                    colFooter.Components.Add(colFooterText);

                    StiText netProfitData = new StiText(new RectangleD(pos, 0.25, columnWidth, 0.25));
                    netProfitData.HorAlignment = StiTextHorAlignment.Right;
                    netProfitData.VertAlignment = StiVertAlignment.Center;
                    netProfitData.Font = new Font("Arial", 9F, FontStyle.Bold);
                    netProfitData.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    netProfitData.TextBrush = new StiSolidBrush(Color.Black);
                    netProfitData.WordWrap = true;
                    if (col.Type != "Variance")
                    {
                        netProfitData.Text.Value = "{Sum(RevenuesDepSummaryBand" + col.Index + ",DepSummaryRevenues" + col.Index + ".Col" + col.Index + "_" + dep.Item1
                            + ") - Sum(CostOfSalesDepSummaryBand" + col.Index + ",DepSummaryCostOfSales" + col.Index + ".Col" + col.Index + "_" + dep.Item1
                            + ") - Sum(ExpensesDepSummaryBand" + col.Index + ",DepSummaryExpenses" + col.Index + ".Col" + col.Index + "_" + dep.Item1
                            + ") + Sum(OtherIncomeDepSummaryBand" + col.Index + ",DepSummaryOtherIncome" + col.Index + ".Col" + col.Index + "_" + dep.Item1
                            + ") - Sum(IncomeTaxesDepSummaryBand" + col.Index + ",DepSummaryIncomeTaxes" + col.Index + ".Col" + col.Index + "_" + dep.Item1 + ")}";
                    }
                    else
                    {
                        var netProfit = _netProfit.Select($"Department = '{dep.Item1}'").FirstOrDefault();
                        netProfitData.Text = netProfit == null ? "0.00%" : Convert.ToDouble(netProfit[$"Column{col.Index}"]).ToString("P2");
                    }
                    netProfitData.TextFormat = textFormat;
                    colFooter.Components.Add(netProfitData);

                    pos = pos + columnWidth;
                }

                StiText totalFooterText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                if (col.Type != "Variance")
                {
                    totalFooterText.Text.Value = "{Sum(DepSummaryIncomeTaxes" + col.Index + ".Total)}";
                }
                totalFooterText.HorAlignment = StiTextHorAlignment.Right;
                totalFooterText.VertAlignment = StiVertAlignment.Center;
                totalFooterText.Font = new Font("Arial", 9F, FontStyle.Bold);
                totalFooterText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                totalFooterText.TextBrush = new StiSolidBrush(Color.Black);
                totalFooterText.WordWrap = true;
                totalFooterText.TextFormat = textFormat;
                colFooter.Components.Add(totalFooterText);

                StiText totalNetProfit = new StiText(new RectangleD(pos, 0.25, columnWidth, 0.25));
                if (col.Type != "Variance")
                {
                    totalNetProfit.Text.Value = "{Sum(RevenuesDepSummaryBand" + col.Index + ",DepSummaryRevenues"
                        + col.Index + ".Total) - Sum(CostOfSalesDepSummaryBand" + col.Index + ",DepSummaryCostOfSales"
                        + col.Index + ".Total) - Sum(ExpensesDepSummaryBand" + col.Index + ",DepSummaryExpenses"
                        + col.Index + ".Total) + Sum(OtherIncomeDepSummaryBand" + col.Index + ",DepSummaryOtherIncome"
                        + col.Index + ".Total) - Sum(IncomeTaxesDepSummaryBand" + col.Index + ",DepSummaryIncomeTaxes"
                        + col.Index + ".Total)}";
                }
                totalNetProfit.HorAlignment = StiTextHorAlignment.Right;
                totalNetProfit.VertAlignment = StiVertAlignment.Center;
                totalNetProfit.Font = new Font("Arial", 9F, FontStyle.Bold);
                totalNetProfit.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                totalNetProfit.TextBrush = new StiSolidBrush(Color.Black);
                totalNetProfit.WordWrap = true;
                totalNetProfit.TextFormat = textFormat;
                colFooter.Components.Add(totalNetProfit);

                // Net Income Percen
                if (col.Type != "Variance")
                {
                    //Create DataBand total
                    StiDataBand netPercenBand = new StiDataBand();
                    netPercenBand.DataSourceName = $"Consolidating{col.Index}";
                    netPercenBand.Name = $"NetIncomePercenConsolidatingDivisional";
                    netPercenBand.Border.Style = StiPenStyle.None;
                    netPercenBand.Filters.Add(new StiFilter());
                    netPercenBand.Filters[0].Item = StiFilterItem.Expression;
                    netPercenBand.Filters[0].Expression = new StiExpression($"Consolidating{col.Index}.Type == 92");
                    netPercenBand.NewPageAfter = true;
                    page.Components.Add(netPercenBand);

                    StiText acctPercenText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                    acctPercenText.Text.Value = "{Consolidating" + col.Index + ".fDesc}";
                    acctPercenText.HorAlignment = StiTextHorAlignment.Left;
                    acctPercenText.VertAlignment = StiVertAlignment.Center;
                    acctPercenText.OnlyText = false;
                    acctPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    acctPercenText.WordWrap = true;
                    acctPercenText.Margins = new StiMargins(0, 1, 0, 10);
                    netPercenBand.Components.Add(acctPercenText);

                    pos = (columnWidth * 2);
                    foreach (var dep in departments)
                    {
                        StiText depPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        depPercenText.Text.Value = "{Consolidating" + col.Index + ".Col" + dep.Item1 + "}";
                        depPercenText.TextFormat = percenFormat;
                        depPercenText.HorAlignment = StiTextHorAlignment.Right;
                        depPercenText.VertAlignment = StiVertAlignment.Center;
                        depPercenText.OnlyText = false;
                        depPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                        depPercenText.WordWrap = true;
                        depPercenText.Margins = new StiMargins(0, 1, 0, 10);
                        netPercenBand.Components.Add(depPercenText);

                        pos = pos + columnWidth;
                    }

                    //Total column
                    StiText totalPercenText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    totalPercenText.Text.Value = "{Consolidating" + col.Index + ".Total}";
                    totalPercenText.TextFormat = percenFormat;
                    totalPercenText.HorAlignment = StiTextHorAlignment.Right;
                    totalPercenText.VertAlignment = StiVertAlignment.Top;
                    totalPercenText.Border.Style = StiPenStyle.None;
                    totalPercenText.OnlyText = false;
                    totalPercenText.Border.Side = StiBorderSides.All;
                    totalPercenText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    totalPercenText.WordWrap = true;
                    totalPercenText.CanGrow = true;
                    totalPercenText.Margins = new StiMargins(0, 1, 4, 0);
                    netPercenBand.Components.Add(totalPercenText);
                }
            }
        }
    }

    private void BuildConsolidatingDivisional(StiPage page, List<ComparativeStatementRequest> objComparative, IEnumerable<Tuple<int, string>> departments)
    {
        var columnWidth = page.Width / (departments.Count() + 3);

        foreach (var col in objComparative)
        {
            if (col.Type == "Actual")
            {
                double pos = 0;
                StiFormatService currencyFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                StiFormatService percenFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");

                StiHeaderBand divisionalTitle = new StiHeaderBand();
                divisionalTitle.Height = 0.5;
                divisionalTitle.PrintIfEmpty = true;
                page.Components.Add(divisionalTitle);

                StiText centerTitleText = new StiText(new RectangleD(0, 0.1, page.Width, 0.25));
                centerTitleText.Text.Value = $"Consolidating Divisional Financial Statements Totals {col.Label}";
                centerTitleText.HorAlignment = StiTextHorAlignment.Center;
                centerTitleText.VertAlignment = StiVertAlignment.Center;
                centerTitleText.Font = new Font("Arial", 18F, FontStyle.Bold);
                centerTitleText.WordWrap = true;
                divisionalTitle.Components.Add(centerTitleText);

                //Create HeaderBand
                StiHeaderBand headerBand = new StiHeaderBand();
                headerBand.Height = 0.25;
                headerBand.Name = $"ConsolidatingDivisional{col.Index}";
                headerBand.Border.Style = StiPenStyle.None;
                headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                headerBand.PrintOnAllPages = false;
                headerBand.PrintIfEmpty = true;
                headerBand.NewPageBefore = true;
                page.Components.Add(headerBand);

                StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2 - 0, 0.25));
                acctText.Text.Value = "";
                acctText.HorAlignment = StiTextHorAlignment.Left;
                acctText.VertAlignment = StiVertAlignment.Center;
                acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                acctText.Border.Side = StiBorderSides.All;
                acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
                acctText.Border.Style = StiPenStyle.None;
                acctText.TextBrush = new StiSolidBrush(Color.White);
                acctText.WordWrap = true;
                headerBand.Components.Add(acctText);

                pos = (columnWidth * 2);

                foreach (var dep in departments)
                {
                    //Create text on header
                    StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                    hText.Text.Value = $"{dep.Item2}";
                    hText.HorAlignment = StiTextHorAlignment.Right;
                    hText.VertAlignment = StiVertAlignment.Center;
                    hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                    hText.Border.Side = StiBorderSides.All;
                    hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    hText.Border.Style = StiPenStyle.None;
                    hText.TextBrush = new StiSolidBrush(Color.White);
                    hText.WordWrap = true;
                    hText.CanGrow = true;
                    headerBand.Components.Add(hText);

                    pos = pos + columnWidth;
                }

                // Total header text
                StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                totalText.Text.Value = "Total";
                totalText.HorAlignment = StiTextHorAlignment.Right;
                totalText.VertAlignment = StiVertAlignment.Center;
                totalText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                totalText.Border.Side = StiBorderSides.All;
                totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                totalText.Border.Style = StiPenStyle.None;
                totalText.TextBrush = new StiSolidBrush(Color.White);
                totalText.WordWrap = true;
                headerBand.Components.Add(totalText);

                // Revenues
                if (objComparative.Count > 0)
                {
                    //Create DataBand total
                    StiDataBand totalBand = new StiDataBand();
                    totalBand.DataSourceName = $"Consolidating{col.Index}";
                    totalBand.Name = $"RevenuesConsolidatingDivisional";
                    totalBand.Border.Style = StiPenStyle.None;
                    totalBand.Filters.Add(new StiFilter());
                    totalBand.Filters[0].Item = StiFilterItem.Expression;
                    totalBand.Filters[0].Expression = new StiExpression($"Consolidating{col.Index}.Type == 3");
                    page.Components.Add(totalBand);

                    StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                    acctTotalText.Text.Value = "{Consolidating" + col.Index + ".fDesc}";
                    acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                    acctTotalText.VertAlignment = StiVertAlignment.Center;
                    acctTotalText.OnlyText = false;
                    acctTotalText.Font = new Font("Arial", 9F);
                    acctTotalText.WordWrap = true;
                    acctTotalText.Margins = new StiMargins(0, 1, 0, 10);
                    totalBand.Components.Add(acctTotalText);

                    pos = (columnWidth * 2);
                    foreach (var dep in departments)
                    {
                        StiText totalDepText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                        totalDepText.Text.Value = "{Consolidating" + col.Index + ".Col" + dep.Item1 + "}";
                        totalDepText.TextFormat = currencyFormat;
                        totalDepText.HorAlignment = StiTextHorAlignment.Right;
                        totalDepText.VertAlignment = StiVertAlignment.Center;
                        totalDepText.OnlyText = false;
                        totalDepText.Font = new Font("Arial", 9F);
                        totalDepText.WordWrap = true;
                        totalDepText.Margins = new StiMargins(0, 1, 0, 10);
                        totalBand.Components.Add(totalDepText);

                        pos = pos + columnWidth;
                    }

                    //Total column
                    StiText totalDataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                    totalDataText.Text.Value = "{Consolidating" + col.Index + ".Total}";
                    totalDataText.TextFormat = currencyFormat;
                    totalDataText.HorAlignment = StiTextHorAlignment.Right;
                    totalDataText.VertAlignment = StiVertAlignment.Top;
                    totalDataText.Border.Style = StiPenStyle.None;
                    totalDataText.OnlyText = false;
                    totalDataText.Border.Side = StiBorderSides.All;
                    totalDataText.Font = new Font("Arial", 8F);
                    totalDataText.WordWrap = true;
                    totalDataText.CanGrow = true;
                    totalDataText.Margins = new StiMargins(0, 1, 4, 0);
                    totalBand.Components.Add(totalDataText);
                }

                // Cost Of Sales
                if (objComparative.Count > 0)
                {
                    //Create DataBand total
                    StiDataBand totalBand = new StiDataBand();
                    totalBand.DataSourceName = $"Consolidating{col.Index}";
                    totalBand.Name = $"CostOfSalesConsolidatingDivisional";
                    totalBand.Border.Style = StiPenStyle.None;
                    totalBand.Filters.Add(new StiFilter());
                    totalBand.Filters[0].Item = StiFilterItem.Expression;
                    totalBand.Filters[0].Expression = new StiExpression($"Consolidating{col.Index}.Type == 4");
                    page.Components.Add(totalBand);

                    StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                    acctTotalText.Text.Value = "{Consolidating" + col.Index + ".fDesc}";
                    acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                    acctTotalText.VertAlignment = StiVertAlignment.Center;
                    acctTotalText.OnlyText = false;
                    acctTotalText.Font = new Font("Arial", 9F);
                    acctTotalText.WordWrap = true;
                    acctTotalText.Margins = new StiMargins(0, 1, 0, 10);
                    totalBand.Components.Add(acctTotalText);

                    pos = (columnWidth * 2);
                    foreach (var dep in departments)
                    {
                        StiText totalDepText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                        totalDepText.Text.Value = "{Consolidating" + col.Index + ".Col" + dep.Item1 + "}";
                        totalDepText.TextFormat = currencyFormat;
                        totalDepText.HorAlignment = StiTextHorAlignment.Right;
                        totalDepText.VertAlignment = StiVertAlignment.Center;
                        totalDepText.OnlyText = false;
                        totalDepText.Font = new Font("Arial", 9F);
                        totalDepText.WordWrap = true;
                        totalDepText.Margins = new StiMargins(0, 1, 0, 10);
                        totalBand.Components.Add(totalDepText);

                        pos = pos + columnWidth;
                    }

                    //Total column
                    StiText totalDataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                    totalDataText.Text.Value = "{Consolidating" + col.Index + ".Total}";
                    totalDataText.TextFormat = currencyFormat;
                    totalDataText.HorAlignment = StiTextHorAlignment.Right;
                    totalDataText.VertAlignment = StiVertAlignment.Top;
                    totalDataText.Border.Style = StiPenStyle.None;
                    totalDataText.OnlyText = false;
                    totalDataText.Border.Side = StiBorderSides.All;
                    totalDataText.Font = new Font("Arial", 8F);
                    totalDataText.WordWrap = true;
                    totalDataText.CanGrow = true;
                    totalDataText.Margins = new StiMargins(0, 1, 4, 0);
                    totalBand.Components.Add(totalDataText);
                }

                // Gross profit
                if (objComparative.Count > 0)
                {
                    //Create DataBand total
                    StiDataBand totalBand = new StiDataBand();
                    totalBand.DataSourceName = $"Consolidating{col.Index}";
                    totalBand.Name = $"GrossConsolidatingDivisional";
                    totalBand.Border.Style = StiPenStyle.None;
                    totalBand.Filters.Add(new StiFilter());
                    totalBand.Filters[0].Item = StiFilterItem.Expression;
                    totalBand.Filters[0].Expression = new StiExpression($"Consolidating{col.Index}.Type == 41");
                    page.Components.Add(totalBand);

                    StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                    acctTotalText.Text.Value = "{Consolidating" + col.Index + ".fDesc}";
                    acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                    acctTotalText.VertAlignment = StiVertAlignment.Center;
                    acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    acctTotalText.OnlyText = false;
                    acctTotalText.Font = new Font("Arial", 9F);
                    acctTotalText.WordWrap = true;
                    acctTotalText.Margins = new StiMargins(0, 1, 0, 10);
                    totalBand.Components.Add(acctTotalText);

                    pos = (columnWidth * 2);
                    foreach (var dep in departments)
                    {
                        StiText totalDepText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                        totalDepText.Text.Value = "{Consolidating" + col.Index + ".Col" + dep.Item1 + "}";
                        totalDepText.TextFormat = currencyFormat;
                        totalDepText.HorAlignment = StiTextHorAlignment.Right;
                        totalDepText.VertAlignment = StiVertAlignment.Center;
                        totalDepText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                        totalDepText.OnlyText = false;
                        totalDepText.Font = new Font("Arial", 9F);
                        totalDepText.WordWrap = true;
                        totalDepText.Margins = new StiMargins(0, 1, 0, 10);
                        totalBand.Components.Add(totalDepText);

                        pos = pos + columnWidth;
                    }

                    //Total column
                    StiText totalDataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                    totalDataText.Text.Value = "{Consolidating" + col.Index + ".Total}";
                    totalDataText.TextFormat = currencyFormat;
                    totalDataText.HorAlignment = StiTextHorAlignment.Right;
                    totalDataText.VertAlignment = StiVertAlignment.Top;
                    totalDataText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    totalDataText.OnlyText = false;
                    totalDataText.Font = new Font("Arial", 8F);
                    totalDataText.WordWrap = true;
                    totalDataText.CanGrow = true;
                    totalDataText.Margins = new StiMargins(0, 1, 4, 0);
                    totalBand.Components.Add(totalDataText);
                }

                // Gross Profit Percen
                if (objComparative.Count > 0)
                {
                    //Create DataBand total
                    StiDataBand totalBand = new StiDataBand();
                    totalBand.DataSourceName = $"Consolidating{col.Index}";
                    totalBand.Name = $"GrossPercenConsolidatingDivisional";
                    totalBand.Border.Style = StiPenStyle.None;
                    totalBand.Filters.Add(new StiFilter());
                    totalBand.Filters[0].Item = StiFilterItem.Expression;
                    totalBand.Filters[0].Expression = new StiExpression($"Consolidating{col.Index}.Type == 42");
                    page.Components.Add(totalBand);

                    StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                    acctTotalText.Text.Value = "{Consolidating" + col.Index + ".fDesc}";
                    acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                    acctTotalText.VertAlignment = StiVertAlignment.Center;
                    acctTotalText.OnlyText = false;
                    acctTotalText.Font = new Font("Arial", 9F);
                    acctTotalText.WordWrap = true;
                    acctTotalText.Margins = new StiMargins(0, 1, 0, 10);
                    totalBand.Components.Add(acctTotalText);

                    pos = (columnWidth * 2);
                    foreach (var dep in departments)
                    {
                        StiText totalDepText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                        totalDepText.Text.Value = "{Consolidating" + col.Index + ".Col" + dep.Item1 + "}";
                        totalDepText.TextFormat = percenFormat;
                        totalDepText.HorAlignment = StiTextHorAlignment.Right;
                        totalDepText.VertAlignment = StiVertAlignment.Center;
                        totalDepText.OnlyText = false;
                        totalDepText.Font = new Font("Arial", 9F);
                        totalDepText.WordWrap = true;
                        totalDepText.Margins = new StiMargins(0, 1, 0, 10);
                        totalBand.Components.Add(totalDepText);

                        pos = pos + columnWidth;
                    }

                    //Total column
                    StiText totalDataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                    totalDataText.Text.Value = "{Consolidating" + col.Index + ".Total}";
                    totalDataText.TextFormat = percenFormat;
                    totalDataText.HorAlignment = StiTextHorAlignment.Right;
                    totalDataText.VertAlignment = StiVertAlignment.Top;
                    totalDataText.Border.Style = StiPenStyle.None;
                    totalDataText.OnlyText = false;
                    totalDataText.Font = new Font("Arial", 8F);
                    totalDataText.WordWrap = true;
                    totalDataText.CanGrow = true;
                    totalDataText.Margins = new StiMargins(0, 1, 4, 0);
                    totalBand.Components.Add(totalDataText);
                }

                // Expenses
                if (objComparative.Count > 0)
                {
                    //Create DataBand total
                    StiDataBand totalBand = new StiDataBand();
                    totalBand.DataSourceName = $"Consolidating{col.Index}";
                    totalBand.Name = $"ExpensesConsolidatingDivisional";
                    totalBand.Border.Style = StiPenStyle.None;
                    totalBand.Filters.Add(new StiFilter());
                    totalBand.Filters[0].Item = StiFilterItem.Expression;
                    totalBand.Filters[0].Expression = new StiExpression($"Consolidating{col.Index}.Type == 5");
                    page.Components.Add(totalBand);

                    StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                    acctTotalText.Text.Value = "{Consolidating" + col.Index + ".fDesc}";
                    acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                    acctTotalText.VertAlignment = StiVertAlignment.Center;
                    acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    acctTotalText.OnlyText = false;
                    acctTotalText.Font = new Font("Arial", 9F);
                    acctTotalText.WordWrap = true;
                    acctTotalText.Margins = new StiMargins(0, 1, 0, 10);
                    totalBand.Components.Add(acctTotalText);

                    pos = (columnWidth * 2);
                    foreach (var dep in departments)
                    {
                        StiText totalDepText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                        totalDepText.Text.Value = "{Consolidating" + col.Index + ".Col" + dep.Item1 + "}";
                        totalDepText.TextFormat = currencyFormat;
                        totalDepText.HorAlignment = StiTextHorAlignment.Right;
                        totalDepText.VertAlignment = StiVertAlignment.Center;
                        totalDepText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                        totalDepText.OnlyText = false;
                        totalDepText.Font = new Font("Arial", 9F);
                        totalDepText.WordWrap = true;
                        totalDepText.Margins = new StiMargins(0, 1, 0, 10);
                        totalBand.Components.Add(totalDepText);

                        pos = pos + columnWidth;
                    }

                    //Total column
                    StiText totalDataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                    totalDataText.Text.Value = "{Consolidating" + col.Index + ".Total}";
                    totalDataText.TextFormat = currencyFormat;
                    totalDataText.HorAlignment = StiTextHorAlignment.Right;
                    totalDataText.VertAlignment = StiVertAlignment.Top;
                    totalDataText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    totalDataText.OnlyText = false;
                    totalDataText.Font = new Font("Arial", 8F);
                    totalDataText.WordWrap = true;
                    totalDataText.CanGrow = true;
                    totalDataText.Margins = new StiMargins(0, 1, 4, 0);
                    totalBand.Components.Add(totalDataText);
                }

                // Net Income
                if (objComparative.Count > 0)
                {
                    //Create DataBand total
                    StiDataBand totalBand = new StiDataBand();
                    totalBand.DataSourceName = $"Consolidating{col.Index}";
                    totalBand.Name = $"NetConsolidatingDivisional";
                    totalBand.Border.Style = StiPenStyle.None;
                    totalBand.Filters.Add(new StiFilter());
                    totalBand.Filters[0].Item = StiFilterItem.Expression;
                    totalBand.Filters[0].Expression = new StiExpression($"Consolidating{col.Index}.Type == 51");
                    page.Components.Add(totalBand);

                    StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                    acctTotalText.Text.Value = "{Consolidating" + col.Index + ".fDesc}";
                    acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                    acctTotalText.VertAlignment = StiVertAlignment.Center;
                    acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    acctTotalText.OnlyText = false;
                    acctTotalText.Font = new Font("Arial", 9F);
                    acctTotalText.WordWrap = true;
                    acctTotalText.Margins = new StiMargins(0, 1, 0, 10);
                    totalBand.Components.Add(acctTotalText);

                    pos = (columnWidth * 2);
                    foreach (var dep in departments)
                    {
                        StiText totalDepText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                        totalDepText.Text.Value = "{Consolidating" + col.Index + ".Col" + dep.Item1 + "}";
                        totalDepText.TextFormat = currencyFormat;
                        totalDepText.HorAlignment = StiTextHorAlignment.Right;
                        totalDepText.VertAlignment = StiVertAlignment.Center;
                        totalDepText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                        totalDepText.OnlyText = false;
                        totalDepText.Font = new Font("Arial", 9F);
                        totalDepText.WordWrap = true;
                        totalDepText.Margins = new StiMargins(0, 1, 0, 10);
                        totalBand.Components.Add(totalDepText);

                        pos = pos + columnWidth;
                    }

                    //Total column
                    StiText totalDataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                    totalDataText.Text.Value = "{Consolidating" + col.Index + ".Total}";
                    totalDataText.TextFormat = currencyFormat;
                    totalDataText.HorAlignment = StiTextHorAlignment.Right;
                    totalDataText.VertAlignment = StiVertAlignment.Top;
                    totalDataText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    totalDataText.OnlyText = false;
                    totalDataText.Font = new Font("Arial", 8F);
                    totalDataText.WordWrap = true;
                    totalDataText.CanGrow = true;
                    totalDataText.Margins = new StiMargins(0, 1, 4, 0);
                    totalBand.Components.Add(totalDataText);
                }

                // Net Income Percen
                if (objComparative.Count > 0)
                {
                    //Create DataBand total
                    StiDataBand totalBand = new StiDataBand();
                    totalBand.DataSourceName = $"Consolidating{col.Index}";
                    totalBand.Name = $"NetPercenConsolidatingDivisional";
                    totalBand.Border.Style = StiPenStyle.None;
                    totalBand.Filters.Add(new StiFilter());
                    totalBand.Filters[0].Item = StiFilterItem.Expression;
                    totalBand.Filters[0].Expression = new StiExpression($"Consolidating{col.Index}.Type == 52");
                    totalBand.NewPageAfter = true;
                    page.Components.Add(totalBand);

                    StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                    acctTotalText.Text.Value = "{Consolidating" + col.Index + ".fDesc}";
                    acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                    acctTotalText.VertAlignment = StiVertAlignment.Center;
                    acctTotalText.OnlyText = false;
                    acctTotalText.Font = new Font("Arial", 9F);
                    acctTotalText.WordWrap = true;
                    acctTotalText.Margins = new StiMargins(0, 1, 0, 10);
                    totalBand.Components.Add(acctTotalText);

                    pos = (columnWidth * 2);
                    foreach (var dep in departments)
                    {
                        StiText totalDepText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                        totalDepText.Text.Value = "{Consolidating" + col.Index + ".Col" + dep.Item1 + "}";
                        totalDepText.TextFormat = percenFormat;
                        totalDepText.HorAlignment = StiTextHorAlignment.Right;
                        totalDepText.VertAlignment = StiVertAlignment.Center;
                        totalDepText.OnlyText = false;
                        totalDepText.Font = new Font("Arial", 9F);
                        totalDepText.WordWrap = true;
                        totalDepText.Margins = new StiMargins(0, 1, 0, 10);
                        totalBand.Components.Add(totalDepText);

                        pos = pos + columnWidth;
                    }

                    //Total column
                    StiText totalDataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                    totalDataText.Text.Value = "{Consolidating" + col.Index + ".Total}";
                    totalDataText.TextFormat = percenFormat;
                    totalDataText.HorAlignment = StiTextHorAlignment.Right;
                    totalDataText.VertAlignment = StiVertAlignment.Top;
                    totalDataText.Border.Style = StiPenStyle.None;
                    totalDataText.OnlyText = false;
                    totalDataText.Border.Side = StiBorderSides.All;
                    totalDataText.Font = new Font("Arial", 8F);
                    totalDataText.WordWrap = true;
                    totalDataText.CanGrow = true;
                    totalDataText.Margins = new StiMargins(0, 1, 4, 0);
                    totalBand.Components.Add(totalDataText);
                }
            }
        }
    }

    private void DepartmentSummaryDataSource(StiReport report, List<ComparativeStatementRequest> objComparative, IEnumerable<Tuple<int, string>> departments, DataTable detailsData = null)
    {
        colIncludeProvisions = new List<Tuple<int, bool, bool>>();
        if (detailsData == null)
        {
            objChart.ConnConfig = Session["config"].ToString();

            if (Session["ComparativeCenter"] != null)
            {
                objChart.Departments = Session["ComparativeCenter"].ToString();
            }

            var ds = bL_Report.GetComparativeStatementData(objChart, objComparative);

            detailsData = ds.Tables[0];
        }

        foreach (var col in objComparative)
        {
            DataTable dtDep = BuildDepartmentSummaryTable(col, departments);

            foreach (DataRow row in detailsData.Rows)
            {
                DataRow dtRow = dtDep.NewRow();
                dtRow["Acct"] = row["Acct"];
                dtRow["AcctNo"] = row["AcctNo"];
                dtRow["AcctName"] = row["AcctName"];
                dtRow["fDesc"] = row["fDesc"];
                dtRow["Type"] = row["Type"];
                dtRow["TypeName"] = row["TypeName"];
                dtRow["Sub"] = row["Sub"];
                dtRow["Total"] = row[$"Column{col.Index}"];
                dtRow[$"Col{col.Index}_{row["Department"]}"] = row[$"Column{col.Index}"];

                dtDep.Rows.Add(dtRow);
            }

            DataTable filteredTable = dtDep.Copy();
            DataView dView = filteredTable.DefaultView;

            dView.RowFilter = "Type = 3";
            DataTable Revenues = dView.ToTable();

            dView.RowFilter = "Type = 4";
            DataTable CostOfSales = dView.ToTable();

            dView.RowFilter = "Type = 5";
            DataTable Expenses = dView.ToTable();

            dView.RowFilter = "Type = 8";
            DataTable OtherIncome = dView.ToTable();

            dView.RowFilter = "Type = 9";
            DataTable IncomeTaxes = dView.ToTable();

            report.RegData($"DepSummaryRevenues{col.Index}", Revenues);
            report.RegData($"DepSummaryCostOfSales{col.Index}", CostOfSales);
            report.RegData($"DepSummaryExpenses{col.Index}", Expenses);
            report.RegData($"DepSummaryOtherIncome{col.Index}", OtherIncome);
            report.RegData($"DepSummaryIncomeTaxes{col.Index}", IncomeTaxes);

            DataTable dtConsolidating = BuildConsolidatingTable(departments, col, Revenues, CostOfSales, Expenses, OtherIncome, IncomeTaxes);
            report.RegData($"Consolidating{col.Index}", dtConsolidating);

            colIncludeProvisions.Add(new Tuple<int, bool, bool>(col.Index, OtherIncome.Rows.Count > 0, IncomeTaxes.Rows.Count > 0));
        }
    }

    private DataTable BuildDepartmentSummaryTable(ComparativeStatementRequest column, IEnumerable<Tuple<int, string>> departments)
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("Acct");
        dt.Columns.Add("AcctNo");
        dt.Columns.Add("AcctName");
        dt.Columns.Add("fDesc");
        dt.Columns.Add("Type");
        dt.Columns.Add("TypeName");
        dt.Columns.Add("Sub");

        foreach (var dep in departments)
        {
            dt.Columns.Add($"Col{column.Index}_{dep.Item1}");
        }

        dt.Columns.Add("Total");

        return dt;
    }

    private DataTable BuildConsolidatingTable(IEnumerable<Tuple<int, string>> departments, ComparativeStatementRequest col, DataTable dtRevenues, DataTable dtCostOfSales, DataTable dtExpenses, DataTable dtOtherIncome, DataTable dtIncomeTaxes)
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("fDesc");
        dt.Columns.Add("Type");

        foreach (var dep in departments)
        {
            dt.Columns.Add($"Col{dep.Item1}");
        }

        dt.Columns.Add("Total");

        var revenue = dt.NewRow();
        var costOfSales = dt.NewRow();
        var expenses = dt.NewRow();
        var otherIncome = dt.NewRow();
        var incomeTaxes = dt.NewRow();
        var grossProfit = dt.NewRow();
        var netProfit = dt.NewRow();
        var beforeProvisions = dt.NewRow();
        var netIncome = dt.NewRow();
        var grossProfitPercen = dt.NewRow();
        var netProfitPercen = dt.NewRow();
        var beforeProvisionsPercen = dt.NewRow();
        var netIncomePercen = dt.NewRow();

        revenue["Type"] = 3;
        costOfSales["Type"] = 4;
        expenses["Type"] = 5;
        otherIncome["Type"] = 8;
        incomeTaxes["Type"] = 9;
        grossProfit["Type"] = 41;
        netProfit["Type"] = 51;
        beforeProvisions["Type"] = 81;
        netIncome["Type"] = 91;
        grossProfitPercen["Type"] = 42;
        netProfitPercen["Type"] = 52;
        beforeProvisionsPercen["Type"] = 82;
        netIncomePercen["Type"] = 92;

        revenue["fDesc"] = "Contract, Service and Repair Revenue";
        costOfSales["fDesc"] = "Cost of Earned Revenue";
        expenses["fDesc"] = "Expenses";
        otherIncome["fDesc"] = "Other Income (Expense)";
        incomeTaxes["fDesc"] = "Provisions for Income Taxes";
        grossProfit["fDesc"] = "Gross Profit";
        netProfit["fDesc"] = "NET INCOME";
        beforeProvisions["fDesc"] = "Income Before Provisions For Income Taxes";
        netIncome["fDesc"] = "NET INCOME";
        grossProfitPercen["fDesc"] = "Gross Profit %";
        netProfitPercen["fDesc"] = "Net Profit/Loss %";
        beforeProvisionsPercen["fDesc"] = "Income Before Provisions For Income Taxes %";
        netIncomePercen["fDesc"] = "Net Income %";

        foreach (var dep in departments)
        {
            var sumRevenues = dtRevenues.AsEnumerable().Sum(r => r[$"Col{col.Index}_{dep.Item1}"] == DBNull.Value ? 0 : Convert.ToDouble(r[$"Col{col.Index}_{dep.Item1}"].ToString()));
            var sumCostOfSales = dtCostOfSales.AsEnumerable().Sum(r => r[$"Col{col.Index}_{dep.Item1}"] == DBNull.Value ? 0 : Convert.ToDouble(r[$"Col{col.Index}_{dep.Item1}"].ToString()));
            var sumExpenses = dtExpenses.AsEnumerable().Sum(r => r[$"Col{col.Index}_{dep.Item1}"] == DBNull.Value ? 0 : Convert.ToDouble(r[$"Col{col.Index}_{dep.Item1}"].ToString()));
            var sumOtherIncome = dtOtherIncome.AsEnumerable().Sum(r => r[$"Col{col.Index}_{dep.Item1}"] == DBNull.Value ? 0 : Convert.ToDouble(r[$"Col{col.Index}_{dep.Item1}"].ToString()));
            var sumIncomeTaxes = dtIncomeTaxes.AsEnumerable().Sum(r => r[$"Col{col.Index}_{dep.Item1}"] == DBNull.Value ? 0 : Convert.ToDouble(r[$"Col{col.Index}_{dep.Item1}"].ToString()));

            revenue[$"Col{dep.Item1}"] = sumRevenues;
            costOfSales[$"Col{dep.Item1}"] = sumCostOfSales;
            expenses[$"Col{dep.Item1}"] = sumExpenses;
            otherIncome[$"Col{dep.Item1}"] = sumOtherIncome;
            incomeTaxes[$"Col{dep.Item1}"] = sumIncomeTaxes;

            grossProfit[$"Col{dep.Item1}"] = sumRevenues - sumCostOfSales;
            netProfit[$"Col{dep.Item1}"] = sumRevenues - sumCostOfSales - sumExpenses;
            beforeProvisions[$"Col{dep.Item1}"] = sumRevenues - sumCostOfSales - sumExpenses + sumOtherIncome;
            netIncome[$"Col{dep.Item1}"] = sumRevenues - sumCostOfSales - sumExpenses + sumOtherIncome - sumIncomeTaxes;

            grossProfitPercen[$"Col{dep.Item1}"] = sumRevenues == 0 ? 0 : (sumRevenues - sumCostOfSales) / sumRevenues;
            netProfitPercen[$"Col{dep.Item1}"] = sumRevenues == 0 ? 0 : (sumRevenues - sumCostOfSales - sumExpenses) / sumRevenues;
            beforeProvisionsPercen[$"Col{dep.Item1}"] = sumRevenues == 0 ? 0 : (sumRevenues - sumCostOfSales - sumExpenses + sumOtherIncome) / sumRevenues;
            netIncomePercen[$"Col{dep.Item1}"] = sumRevenues == 0 ? 0 : (sumRevenues - sumCostOfSales - sumExpenses + sumOtherIncome - sumIncomeTaxes) / sumRevenues;
        }

        // Total column
        var totalRevenues = dtRevenues.AsEnumerable().Sum(r => r["Total"] == DBNull.Value ? 0 : Convert.ToDouble(r["Total"].ToString()));
        var totalCostOfSales = dtCostOfSales.AsEnumerable().Sum(r => r["Total"] == DBNull.Value ? 0 : Convert.ToDouble(r["Total"].ToString()));
        var totalExpenses = dtExpenses.AsEnumerable().Sum(r => r["Total"] == DBNull.Value ? 0 : Convert.ToDouble(r["Total"].ToString()));
        var totalOtherIncome = dtOtherIncome.AsEnumerable().Sum(r => r["Total"] == DBNull.Value ? 0 : Convert.ToDouble(r["Total"].ToString()));
        var totalIncomeTaxes = dtIncomeTaxes.AsEnumerable().Sum(r => r["Total"] == DBNull.Value ? 0 : Convert.ToDouble(r["Total"].ToString()));

        revenue["Total"] = totalRevenues;
        costOfSales["Total"] = totalCostOfSales;
        expenses["Total"] = totalExpenses;
        otherIncome["Total"] = totalOtherIncome;
        incomeTaxes["Total"] = totalIncomeTaxes;

        grossProfit["Total"] = totalRevenues - totalCostOfSales;
        netProfit["Total"] = totalRevenues - totalCostOfSales - totalExpenses;
        beforeProvisions["Total"] = totalRevenues - totalCostOfSales - totalExpenses + totalOtherIncome;
        netIncome["Total"] = totalRevenues - totalCostOfSales - totalExpenses + totalOtherIncome - totalIncomeTaxes;
        grossProfitPercen["Total"] = totalRevenues == 0 ? 0 : (totalRevenues - totalCostOfSales) / totalRevenues;
        netProfitPercen["Total"] = totalRevenues == 0 ? 0 : (totalRevenues - totalCostOfSales - totalExpenses) / totalRevenues;
        beforeProvisionsPercen["Total"] = totalRevenues == 0 ? 0 : (totalRevenues - totalCostOfSales - totalExpenses + totalOtherIncome) / totalRevenues;
        netIncomePercen["Total"] = totalRevenues == 0 ? 0 : (totalRevenues - totalCostOfSales - totalExpenses + totalOtherIncome - totalIncomeTaxes) / totalRevenues;

        dt.Rows.Add(revenue);
        dt.Rows.Add(costOfSales);
        dt.Rows.Add(expenses);
        dt.Rows.Add(otherIncome);
        dt.Rows.Add(incomeTaxes);
        dt.Rows.Add(grossProfit);
        dt.Rows.Add(netProfit);
        dt.Rows.Add(beforeProvisions);
        dt.Rows.Add(netIncome);
        dt.Rows.Add(grossProfitPercen);
        dt.Rows.Add(netProfitPercen);
        dt.Rows.Add(beforeProvisionsPercen);
        dt.Rows.Add(netIncomePercen);

        return dt;
    }

    private void BuildDepartmentSummaryDataSource(StiDataSource dtSource, ComparativeStatementRequest column, IEnumerable<Tuple<int, string>> departments)
    {
        dtSource.Columns.Add("Acct", typeof(string));
        dtSource.Columns.Add("AcctNo", typeof(string));
        dtSource.Columns.Add("AcctName", typeof(string));
        dtSource.Columns.Add("fDesc", typeof(string));
        dtSource.Columns.Add("Type", typeof(int));
        dtSource.Columns.Add("TypeName", typeof(string));
        dtSource.Columns.Add("Sub", typeof(string));

        foreach (var dep in departments)
        {
            dtSource.Columns.Add($"Col{column.Index}_{dep.Item1}", typeof(double));
        }

        dtSource.Columns.Add("Total", typeof(double));
    }

    private void BuildConsolidatingDataSource(StiDataSource dtSource, ComparativeStatementRequest column, IEnumerable<Tuple<int, string>> departments)
    {
        dtSource.Columns.Add("fDesc", typeof(string));
        dtSource.Columns.Add("Type", typeof(int));

        foreach (var dep in departments)
        {
            dtSource.Columns.Add($"Col{dep.Item1}", typeof(double));
        }

        dtSource.Columns.Add("Total", typeof(double));
    }

    private StiReport GetComparativeBalanceSheetReport(DataSet comparativeReportData, List<ComparativeStatementRequest> objComparative)
    {
        try
        {
            string reportPathStimul = string.Empty;
            reportPathStimul = Server.MapPath("StimulsoftReports/ComparativeBalanceSheet.mrt");
            StiReport report = new StiReport();
            report.Load(reportPathStimul);

            var objColumns = objComparative;
            foreach (var column in objColumns)
            {
                report.Dictionary.DataSources["Assets"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["Equity"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["Liability"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["TotalLiabilityEquity"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));

                if (column.Type == "Actual")
                {
                    report.Dictionary.DataSources["Assets"].Columns.Add(string.Format("Column{0}URL", column.Index), typeof(string));
                    report.Dictionary.DataSources["Equity"].Columns.Add(string.Format("Column{0}URL", column.Index), typeof(string));
                    report.Dictionary.DataSources["Liability"].Columns.Add(string.Format("Column{0}URL", column.Index), typeof(string));
                }
            }

            StiPage page = report.Pages[0];
            page.CanGrow = true;
            page.CanShrink = true;

            var columnCount = objColumns.Count + 2;
            double columnWidth = page.Width / columnCount;
            double pos = 0;

            // Assets
            if (objColumns.Count > 0)
            {
                //Create HeaderBand
                StiHeaderBand headerBand = new StiHeaderBand();
                headerBand.Height = 0.25;
                headerBand.Name = "AssetsHeaderBand";
                headerBand.Border.Style = StiPenStyle.None;
                headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                headerBand.PrintOnAllPages = false;
                headerBand.PrintIfEmpty = true;
                page.Components.Add(headerBand);

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "Assets";
                dataBand.Name = $"AssetsData";
                dataBand.Sort = new string[2] { "ASC", "fDesc" };
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);

                //Create DataBand item
                StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                acctText.Text.Value = "Assets";
                acctText.HorAlignment = StiTextHorAlignment.Left;
                acctText.VertAlignment = StiVertAlignment.Center;
                acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                acctText.Border.Side = StiBorderSides.All;
                acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
                acctText.Border.Style = StiPenStyle.None;
                acctText.TextBrush = new StiSolidBrush(Color.White);
                acctText.WordWrap = true;
                headerBand.Components.Add(acctText);

                StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.2));
                acctDataText.Text.Value = "{Assets.fDesc}";
                acctDataText.HorAlignment = StiTextHorAlignment.Left;
                acctDataText.VertAlignment = StiVertAlignment.Center;
                acctDataText.Border.Style = StiPenStyle.None;
                acctDataText.OnlyText = false;
                acctDataText.Border.Side = StiBorderSides.All;
                acctDataText.Font = new Font("Arial", 8F);
                acctDataText.WordWrap = true;
                acctDataText.CanGrow = true;
                acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(acctDataText);

                pos = (columnWidth * 2);

                foreach (var column in objColumns)
                {
                    //Create text on header
                    StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                    hText.Text.Value = column.Label;
                    hText.HorAlignment = StiTextHorAlignment.Right;
                    hText.VertAlignment = StiVertAlignment.Center;
                    hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                    hText.Border.Side = StiBorderSides.All;
                    hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    hText.Border.Style = StiPenStyle.None;
                    hText.TextBrush = new StiSolidBrush(Color.White);
                    hText.WordWrap = true;
                    hText.CanGrow = true;
                    headerBand.Components.Add(hText);

                    StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                    dataText.Text.Value = "{Assets.Column" + column.Index + "}";

                    if (column.Type != "Variance")
                    {
                        dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }

                    if (column.Type == "Actual")
                    {
                        dataText.Interaction.Hyperlink = new StiHyperlinkExpression("{Assets.Column" + column.Index + "URL}");
                    }

                    dataText.HorAlignment = StiTextHorAlignment.Right;
                    dataText.VertAlignment = StiVertAlignment.Top;
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.Font = new Font("Arial", 8F);
                    dataText.WordWrap = true;
                    dataText.CanGrow = true;
                    dataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(dataText);

                    pos = pos + columnWidth;
                }

                //Create FooterBand total
                StiFooterBand footerBand = new StiFooterBand();
                footerBand.Name = $"AssetsTotal";
                footerBand.Border.Style = StiPenStyle.None;
                page.Components.Add(footerBand);

                StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                acctTotalText.Text.Value = "Total Assets";
                acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                acctTotalText.VertAlignment = StiVertAlignment.Center;
                acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                acctTotalText.OnlyText = false;
                acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                acctTotalText.WordWrap = true;
                acctTotalText.Margins = new StiMargins(0, 1, 0, 10);
                footerBand.Components.Add(acctTotalText);

                pos = (columnWidth * 2);
                foreach (var column in objColumns)
                {
                    StiText footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                    if (column.Type != "Variance")
                    {
                        footerText.Text.Value = "{Sum(Assets.Column" + column.Index + ")}";
                        footerText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        footerText.Text.Value = "{(Sum(Assets.Column" + column.Column1 + ") - Sum(Assets.Column" + column.Column2 + ")) / Abs(Sum(Assets.Column" + column.Column2 + "))}";
                        footerText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    footerText.HorAlignment = StiTextHorAlignment.Right;
                    footerText.VertAlignment = StiVertAlignment.Center;
                    footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    footerText.OnlyText = false;
                    footerText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    footerText.WordWrap = true;
                    footerText.Margins = new StiMargins(0, 1, 0, 10);
                    footerBand.Components.Add(footerText);

                    pos = pos + columnWidth;
                }
            }

            // Liability
            if (objColumns.Count > 0)
            {
                //Create HeaderBand
                StiHeaderBand headerBand = new StiHeaderBand();
                headerBand.Height = 0.25;
                headerBand.Name = $"LiabilityHeaderBand";
                headerBand.Border.Style = StiPenStyle.None;
                headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                headerBand.PrintOnAllPages = false;
                headerBand.PrintIfEmpty = true;
                page.Components.Add(headerBand);

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "Liability";
                dataBand.Name = $"LiabilityData";
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);

                //Create DataBand item
                StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                acctText.Text.Value = "Liability";
                acctText.HorAlignment = StiTextHorAlignment.Left;
                acctText.VertAlignment = StiVertAlignment.Center;
                acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                acctText.Border.Side = StiBorderSides.All;
                acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
                acctText.Border.Style = StiPenStyle.None;
                acctText.TextBrush = new StiSolidBrush(Color.White);
                acctText.WordWrap = true;
                headerBand.Components.Add(acctText);

                StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.2));
                acctDataText.Text.Value = "{Liability.fDesc}";
                acctDataText.HorAlignment = StiTextHorAlignment.Left;
                acctDataText.VertAlignment = StiVertAlignment.Center;
                acctDataText.Border.Style = StiPenStyle.None;
                acctDataText.OnlyText = false;
                acctDataText.Border.Side = StiBorderSides.All;
                acctDataText.Font = new Font("Arial", 8F);
                acctDataText.WordWrap = true;
                acctDataText.CanGrow = true;
                acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(acctDataText);

                pos = (columnWidth * 2);

                foreach (var column in objColumns)
                {
                    //Create text on header
                    StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                    hText.Text.Value = column.Label;
                    hText.HorAlignment = StiTextHorAlignment.Right;
                    hText.VertAlignment = StiVertAlignment.Center;
                    hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                    hText.Border.Side = StiBorderSides.All;
                    hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    hText.Border.Style = StiPenStyle.None;
                    hText.TextBrush = new StiSolidBrush(Color.White);
                    hText.WordWrap = true;
                    hText.CanGrow = true;
                    headerBand.Components.Add(hText);

                    StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                    dataText.Text.Value = "{Liability.Column" + column.Index + "}";

                    if (column.Type != "Variance")
                    {
                        dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }

                    if (column.Type == "Actual")
                    {
                        dataText.Interaction.Hyperlink = new StiHyperlinkExpression("{Liability.Column" + column.Index + "URL}");
                    }

                    dataText.HorAlignment = StiTextHorAlignment.Right;
                    dataText.VertAlignment = StiVertAlignment.Top;
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.Font = new Font("Arial", 8F);
                    dataText.WordWrap = true;
                    dataText.CanGrow = true;
                    dataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(dataText);

                    pos = pos + columnWidth;
                }

                //Create FooterBand total
                StiFooterBand footerBand = new StiFooterBand();
                footerBand.Name = $"LiabilityTotal";
                footerBand.Border.Style = StiPenStyle.None;
                page.Components.Add(footerBand);

                StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                acctTotalText.Text.Value = "Total Liability";
                acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                acctTotalText.VertAlignment = StiVertAlignment.Center;
                acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                acctTotalText.OnlyText = false;
                acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                acctTotalText.WordWrap = true;
                acctTotalText.Margins = new StiMargins(0, 1, 0, 10);
                footerBand.Components.Add(acctTotalText);

                pos = (columnWidth * 2);
                foreach (var column in objColumns)
                {
                    StiText footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                    if (column.Type != "Variance")
                    {
                        footerText.Text.Value = "{Sum(Liability.Column" + column.Index + ")}";
                        footerText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        footerText.Text.Value = "{(Sum(Liability.Column" + column.Column1 + ") - Sum(Liability.Column" + column.Column2 + ")) / Abs(Sum(Liability.Column" + column.Column2 + "))}";
                        footerText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    footerText.HorAlignment = StiTextHorAlignment.Right;
                    footerText.VertAlignment = StiVertAlignment.Center;
                    footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    footerText.OnlyText = false;
                    footerText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    footerText.WordWrap = true;
                    footerText.Margins = new StiMargins(0, 1, 0, 10);
                    footerBand.Components.Add(footerText);

                    pos = pos + columnWidth;
                }
            }

            // Equity
            if (objColumns.Count > 0)
            {
                //Create HeaderBand
                StiHeaderBand headerBand = new StiHeaderBand();
                headerBand.Height = 0.25;
                headerBand.Name = $"EquityHeaderBand";
                headerBand.Border.Style = StiPenStyle.None;
                headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                headerBand.PrintOnAllPages = false;
                headerBand.PrintIfEmpty = true;
                page.Components.Add(headerBand);

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "Equity";
                dataBand.Name = $"EquityData";
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);

                //Create DataBand item
                StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                acctText.Text.Value = "Equity";
                acctText.HorAlignment = StiTextHorAlignment.Left;
                acctText.VertAlignment = StiVertAlignment.Center;
                acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                acctText.Border.Side = StiBorderSides.All;
                acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
                acctText.Border.Style = StiPenStyle.None;
                acctText.TextBrush = new StiSolidBrush(Color.White);
                acctText.WordWrap = true;
                headerBand.Components.Add(acctText);

                StiText acctDataText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.2));
                acctDataText.Text.Value = "{Equity.fDesc}";
                acctDataText.HorAlignment = StiTextHorAlignment.Left;
                acctDataText.VertAlignment = StiVertAlignment.Center;
                acctDataText.Border.Style = StiPenStyle.None;
                acctDataText.OnlyText = false;
                acctDataText.Border.Side = StiBorderSides.All;
                acctDataText.Font = new Font("Arial", 8F);
                acctDataText.WordWrap = true;
                acctDataText.CanGrow = true;
                acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(acctDataText);

                pos = (columnWidth * 2);

                foreach (var column in objColumns)
                {
                    //Create text on header
                    StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                    hText.Text.Value = column.Label;
                    hText.HorAlignment = StiTextHorAlignment.Right;
                    hText.VertAlignment = StiVertAlignment.Center;
                    hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                    hText.Border.Side = StiBorderSides.All;
                    hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    hText.Border.Style = StiPenStyle.None;
                    hText.TextBrush = new StiSolidBrush(Color.White);
                    hText.WordWrap = true;
                    hText.CanGrow = true;
                    headerBand.Components.Add(hText);

                    StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                    dataText.Text.Value = "{Equity.Column" + column.Index + "}";

                    if (column.Type != "Variance")
                    {
                        dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }

                    if (column.Type == "Actual")
                    {
                        dataText.Interaction.Hyperlink = new StiHyperlinkExpression("{Equity.Column" + column.Index + "URL}");
                    }

                    dataText.HorAlignment = StiTextHorAlignment.Right;
                    dataText.VertAlignment = StiVertAlignment.Top;
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.Font = new Font("Arial", 8F);
                    dataText.WordWrap = true;
                    dataText.CanGrow = true;
                    dataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(dataText);

                    pos = pos + columnWidth;
                }

                //Create FooterBand total
                StiFooterBand footerBand = new StiFooterBand();
                footerBand.Name = $"EquityTotal";
                footerBand.Border.Style = StiPenStyle.None;
                page.Components.Add(footerBand);

                StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                acctTotalText.Text.Value = "Total Equity";
                acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                acctTotalText.VertAlignment = StiVertAlignment.Center;
                acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                acctTotalText.OnlyText = false;
                acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                acctTotalText.WordWrap = true;
                acctTotalText.Margins = new StiMargins(0, 1, 0, 10);
                footerBand.Components.Add(acctTotalText);

                //Create Total Liability & Equity
                StiDataBand totalLEBand = new StiDataBand();
                totalLEBand.DataSourceName = "TotalLiabilityEquity";
                totalLEBand.Name = "TotalLiabilityEquityBand";
                totalLEBand.Border.Style = StiPenStyle.None;
                page.Components.Add(totalLEBand);

                StiText totalLEText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                totalLEText.Text.Value = "Total Liability & Equity";
                totalLEText.HorAlignment = StiTextHorAlignment.Left;
                totalLEText.VertAlignment = StiVertAlignment.Center;
                totalLEText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                totalLEText.OnlyText = false;
                totalLEText.Font = new Font("Arial", 9F, FontStyle.Bold);
                totalLEText.WordWrap = true;
                totalLEText.Margins = new StiMargins(0, 1, 0, 10);
                totalLEBand.Components.Add(totalLEText);

                pos = (columnWidth * 2);
                foreach (var column in objColumns)
                {
                    // Total Equity
                    StiText footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                    if (column.Type != "Variance")
                    {
                        footerText.Text.Value = "{Sum(Equity.Column" + column.Index + ")}";
                        footerText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        footerText.Text.Value = "{(Sum(Equity.Column" + column.Column1 + ") - Sum(Equity.Column" + column.Column2 + ")) / Abs(Sum(Equity.Column" + column.Column2 + "))}";
                        footerText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    footerText.HorAlignment = StiTextHorAlignment.Right;
                    footerText.VertAlignment = StiVertAlignment.Center;
                    footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    footerText.OnlyText = false;
                    footerText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    footerText.WordWrap = true;
                    footerText.Margins = new StiMargins(0, 1, 0, 10);
                    footerBand.Components.Add(footerText);

                    StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                    totalText.Text.Value = "{TotalLiabilityEquity.Column" + column.Index + "}";
                    if (column.Type != "Variance")
                    {
                        totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    totalText.Type = StiSystemTextType.Expression;
                    totalText.HorAlignment = StiTextHorAlignment.Right;
                    totalText.VertAlignment = StiVertAlignment.Center;
                    totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    totalText.OnlyText = false;
                    totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    totalText.WordWrap = true;
                    totalText.Margins = new StiMargins(0, 1, 0, 10);
                    totalLEBand.Components.Add(totalText);

                    pos = pos + columnWidth;
                }
            }

            //report.Compile();

            DataSet dsC = new DataSet();

            var connString = Session["config"].ToString();
            objPropUser.ConnConfig = connString;
            dsC = objBL_User.getControl(objPropUser);

            DataTable cTable = BuildCompanyDetailsTable();
            var cRow = cTable.NewRow();

            DataSet companyInfo = new DataSet();
            companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());

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

            report.RegData("CompanyDetails", cTable);
            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();
            report.Dictionary.Variables["ReportName"].Value = Session["ReportName"].ToString();

            var data = AdjustmentBalanceSheet(comparativeReportData, objComparative);
            DataTable filteredTable = data.Copy();
            DataView dView = filteredTable.DefaultView;

            dView.RowFilter = "Type = 0 OR Type = 6";
            DataTable Assets = dView.ToTable();

            dView.RowFilter = "Type = 2";
            DataTable Equity = dView.ToTable();

            dView.RowFilter = "Type = 1";
            DataTable Liability = dView.ToTable();

            dView.RowFilter = "Type = 99";
            DataTable TotalLiabilityEquity = dView.ToTable();

            report.RegData("Assets", Assets);
            report.RegData("Equity", Equity);
            report.RegData("Liability", Liability);
            report.RegData("TotalLiabilityEquity", TotalLiabilityEquity);

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

    private StiReport GetComparativeBalanceSheetWithSubReport(DataSet comparativeReportData, List<ComparativeStatementRequest> objComparative)
    {
        try
        {
            string reportPathStimul = string.Empty;
            reportPathStimul = Server.MapPath("StimulsoftReports/ComparativeBalanceSheet.mrt");
            StiReport report = new StiReport();
            report.Load(reportPathStimul);

            var objColumns = objComparative;
            foreach (var column in objColumns)
            {
                report.Dictionary.DataSources["Assets"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["Equity"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["Liability"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["TotalLiabilityEquity"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));

                if (column.Type == "Actual")
                {
                    report.Dictionary.DataSources["Assets"].Columns.Add(string.Format("Column{0}URL", column.Index), typeof(string));
                    report.Dictionary.DataSources["Equity"].Columns.Add(string.Format("Column{0}URL", column.Index), typeof(string));
                    report.Dictionary.DataSources["Liability"].Columns.Add(string.Format("Column{0}URL", column.Index), typeof(string));
                }
            }

            StiPage page = report.Pages[0];
            page.CanGrow = true;
            page.CanShrink = true;

            var columnCount = objColumns.Count + 2;
            double columnWidth = page.Width / columnCount;
            double pos = 0;

            // Assets
            if (objColumns.Count > 0)
            {
                //Create HeaderBand
                StiHeaderBand headerBand = new StiHeaderBand();
                headerBand.Height = 0.25;
                headerBand.Name = "AssetsHeaderBand";
                headerBand.Border.Style = StiPenStyle.None;
                headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                headerBand.PrintOnAllPages = false;
                headerBand.PrintIfEmpty = true;
                page.Components.Add(headerBand);

                StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                acctText.Text.Value = "Assets";
                acctText.HorAlignment = StiTextHorAlignment.Left;
                acctText.VertAlignment = StiVertAlignment.Center;
                acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                acctText.Border.Side = StiBorderSides.All;
                acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
                acctText.Border.Style = StiPenStyle.None;
                acctText.TextBrush = new StiSolidBrush(Color.White);
                acctText.WordWrap = true;
                headerBand.Components.Add(acctText);

                //Create group header band
                StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                groupHeader.Name = "AssetsGroupHeaderBand";
                groupHeader.PrintOnAllPages = false;
                groupHeader.Condition = new StiGroupConditionExpression("{Assets.Sub}");
                page.Components.Add(groupHeader);

                StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                groupHeaderText.Text.Value = "{Assets.Sub}";
                groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                groupHeaderText.VertAlignment = StiVertAlignment.Center;
                groupHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
                groupHeaderText.Border.Style = StiPenStyle.None;
                groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                groupHeaderText.WordWrap = true;
                groupHeader.Components.Add(groupHeaderText);

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "Assets";
                dataBand.Name = $"AssetsData";
                dataBand.Sort = new string[2] { "ASC", "fDesc" };
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);

                StiText acctDataText = new StiText(new RectangleD(0.1, 0, columnWidth * 2 - 0.1, 0.2));
                acctDataText.Text.Value = "{Assets.fDesc}";
                acctDataText.HorAlignment = StiTextHorAlignment.Left;
                acctDataText.VertAlignment = StiVertAlignment.Center;
                acctDataText.Border.Style = StiPenStyle.None;
                acctDataText.OnlyText = false;
                acctDataText.Border.Side = StiBorderSides.All;
                acctDataText.Font = new Font("Arial", 8F);
                acctDataText.WordWrap = true;
                acctDataText.CanGrow = true;
                acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(acctDataText);

                pos = (columnWidth * 2);

                foreach (var column in objColumns)
                {
                    //Create text on header
                    StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                    hText.Text.Value = column.Label;
                    hText.HorAlignment = StiTextHorAlignment.Right;
                    hText.VertAlignment = StiVertAlignment.Center;
                    hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                    hText.Border.Side = StiBorderSides.All;
                    hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    hText.Border.Style = StiPenStyle.None;
                    hText.TextBrush = new StiSolidBrush(Color.White);
                    hText.WordWrap = true;
                    hText.CanGrow = true;
                    headerBand.Components.Add(hText);

                    StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                    dataText.Text.Value = "{Assets.Column" + column.Index + "}";

                    if (column.Type != "Variance")
                    {
                        dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }

                    if (column.Type == "Actual")
                    {
                        dataText.Interaction.Hyperlink = new StiHyperlinkExpression("{Assets.Column" + column.Index + "URL}");
                    }

                    dataText.HorAlignment = StiTextHorAlignment.Right;
                    dataText.VertAlignment = StiVertAlignment.Top;
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.Font = new Font("Arial", 8F);
                    dataText.WordWrap = true;
                    dataText.CanGrow = true;
                    dataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(dataText);

                    pos = pos + columnWidth;
                }

                //Create group footer band
                StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.4));
                groupFooter.Name = "AssetsGroupFooterBand";
                page.Components.Add(groupFooter);

                StiText subTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                subTotalText.Text.Value = "Total {Assets.Sub}";
                subTotalText.HorAlignment = StiTextHorAlignment.Left;
                subTotalText.VertAlignment = StiVertAlignment.Center;
                subTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                subTotalText.Border.Style = StiPenStyle.None;
                subTotalText.TextBrush = new StiSolidBrush(Color.Black);
                subTotalText.WordWrap = true;
                groupFooter.Components.Add(subTotalText);

                pos = (columnWidth * 2);
                foreach (var column in objColumns)
                {
                    StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    totalText.HorAlignment = StiTextHorAlignment.Right;
                    totalText.VertAlignment = StiVertAlignment.Center;
                    totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    totalText.TextBrush = new StiSolidBrush(Color.Black);
                    totalText.WordWrap = true;

                    if (column.Type != "Variance")
                    {
                        totalText.Text.Value = "{Sum(Assets.Column" + column.Index + ")}";
                        totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        totalText.Text.Value = "{(Sum(Assets.Column" + column.Column1 + ") - Sum(Assets.Column" + column.Column2 + ")) / Abs(Sum(Assets.Column" + column.Column2 + "))}";
                        totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    groupFooter.Components.Add(totalText);

                    pos = pos + columnWidth;
                }

                //Create FooterBand total
                StiFooterBand footerBand = new StiFooterBand();
                footerBand.Name = $"AssetsTotal";
                footerBand.Border.Style = StiPenStyle.None;
                page.Components.Add(footerBand);

                StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                acctTotalText.Text.Value = "Total Assets";
                acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                acctTotalText.VertAlignment = StiVertAlignment.Center;
                acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                acctTotalText.OnlyText = false;
                acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                acctTotalText.WordWrap = true;
                acctTotalText.Margins = new StiMargins(0, 1, 0, 10);
                footerBand.Components.Add(acctTotalText);

                pos = (columnWidth * 2);
                foreach (var column in objColumns)
                {
                    StiText footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                    if (column.Type != "Variance")
                    {
                        footerText.Text.Value = "{Sum(Assets.Column" + column.Index + ")}";
                        footerText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        footerText.Text.Value = "{(Sum(Assets.Column" + column.Column1 + ") - Sum(Assets.Column" + column.Column2 + ")) / Abs(Sum(Assets.Column" + column.Column2 + "))}";
                        footerText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    footerText.HorAlignment = StiTextHorAlignment.Right;
                    footerText.VertAlignment = StiVertAlignment.Center;
                    footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    footerText.OnlyText = false;
                    footerText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    footerText.WordWrap = true;
                    footerText.Margins = new StiMargins(0, 1, 0, 10);
                    footerBand.Components.Add(footerText);

                    pos = pos + columnWidth;
                }
            }

            // Liability
            if (objColumns.Count > 0)
            {
                //Create HeaderBand
                StiHeaderBand headerBand = new StiHeaderBand();
                headerBand.Height = 0.25;
                headerBand.Name = $"LiabilityHeaderBand";
                headerBand.Border.Style = StiPenStyle.None;
                headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                headerBand.PrintOnAllPages = false;
                headerBand.PrintIfEmpty = true;
                page.Components.Add(headerBand);

                StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                acctText.Text.Value = "Liability";
                acctText.HorAlignment = StiTextHorAlignment.Left;
                acctText.VertAlignment = StiVertAlignment.Center;
                acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                acctText.Border.Side = StiBorderSides.All;
                acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
                acctText.Border.Style = StiPenStyle.None;
                acctText.TextBrush = new StiSolidBrush(Color.White);
                acctText.WordWrap = true;
                headerBand.Components.Add(acctText);

                //Create group header band
                StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                groupHeader.Name = "LiabilityGroupHeaderBand";
                groupHeader.PrintOnAllPages = false;
                groupHeader.Condition = new StiGroupConditionExpression("{Liability.Sub}");
                page.Components.Add(groupHeader);

                StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                groupHeaderText.Text.Value = "{Liability.Sub}";
                groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                groupHeaderText.VertAlignment = StiVertAlignment.Center;
                groupHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
                groupHeaderText.Border.Style = StiPenStyle.None;
                groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                groupHeaderText.WordWrap = true;
                groupHeader.Components.Add(groupHeaderText);

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "Liability";
                dataBand.Name = $"LiabilityData";
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);

                StiText acctDataText = new StiText(new RectangleD(0.1, 0, columnWidth * 2 - 0.1, 0.2));
                acctDataText.Text.Value = "{Liability.fDesc}";
                acctDataText.HorAlignment = StiTextHorAlignment.Left;
                acctDataText.VertAlignment = StiVertAlignment.Center;
                acctDataText.Border.Style = StiPenStyle.None;
                acctDataText.OnlyText = false;
                acctDataText.Border.Side = StiBorderSides.All;
                acctDataText.Font = new Font("Arial", 8F);
                acctDataText.WordWrap = true;
                acctDataText.CanGrow = true;
                acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(acctDataText);

                pos = (columnWidth * 2);

                foreach (var column in objColumns)
                {
                    //Create text on header
                    StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                    hText.Text.Value = column.Label;
                    hText.HorAlignment = StiTextHorAlignment.Right;
                    hText.VertAlignment = StiVertAlignment.Center;
                    hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                    hText.Border.Side = StiBorderSides.All;
                    hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    hText.Border.Style = StiPenStyle.None;
                    hText.TextBrush = new StiSolidBrush(Color.White);
                    hText.WordWrap = true;
                    hText.CanGrow = true;
                    headerBand.Components.Add(hText);

                    StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                    dataText.Text.Value = "{Liability.Column" + column.Index + "}";

                    if (column.Type != "Variance")
                    {
                        dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }

                    if (column.Type == "Actual")
                    {
                        dataText.Interaction.Hyperlink = new StiHyperlinkExpression("{Liability.Column" + column.Index + "URL}");
                    }

                    dataText.HorAlignment = StiTextHorAlignment.Right;
                    dataText.VertAlignment = StiVertAlignment.Top;
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.Font = new Font("Arial", 8F);
                    dataText.WordWrap = true;
                    dataText.CanGrow = true;
                    dataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(dataText);

                    pos = pos + columnWidth;
                }

                //Create group footer band
                StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.4));
                groupFooter.Name = "LiabilityGroupFooterBand";
                page.Components.Add(groupFooter);

                StiText subTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                subTotalText.Text.Value = "Total {Liability.Sub}";
                subTotalText.HorAlignment = StiTextHorAlignment.Left;
                subTotalText.VertAlignment = StiVertAlignment.Center;
                subTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                subTotalText.Border.Style = StiPenStyle.None;
                subTotalText.TextBrush = new StiSolidBrush(Color.Black);
                subTotalText.WordWrap = true;
                groupFooter.Components.Add(subTotalText);

                pos = (columnWidth * 2);
                foreach (var column in objColumns)
                {
                    StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    totalText.HorAlignment = StiTextHorAlignment.Right;
                    totalText.VertAlignment = StiVertAlignment.Center;
                    totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    totalText.TextBrush = new StiSolidBrush(Color.Black);
                    totalText.WordWrap = true;

                    if (column.Type != "Variance")
                    {
                        totalText.Text.Value = "{Sum(Liability.Column" + column.Index + ")}";
                        totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        totalText.Text.Value = "{(Sum(Liability.Column" + column.Column1 + ") - Sum(Liability.Column" + column.Column2 + ")) / Abs(Sum(Liability.Column" + column.Column2 + "))}";
                        totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    groupFooter.Components.Add(totalText);

                    pos = pos + columnWidth;
                }

                //Create FooterBand total
                StiFooterBand footerBand = new StiFooterBand();
                footerBand.Name = $"LiabilityTotal";
                footerBand.Border.Style = StiPenStyle.None;
                page.Components.Add(footerBand);

                StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                acctTotalText.Text.Value = "Total Liability";
                acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                acctTotalText.VertAlignment = StiVertAlignment.Center;
                acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                acctTotalText.OnlyText = false;
                acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                acctTotalText.WordWrap = true;
                acctTotalText.Margins = new StiMargins(0, 1, 0, 10);
                footerBand.Components.Add(acctTotalText);

                pos = (columnWidth * 2);
                foreach (var column in objColumns)
                {
                    StiText footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                    if (column.Type != "Variance")
                    {
                        footerText.Text.Value = "{Sum(Liability.Column" + column.Index + ")}";
                        footerText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        footerText.Text.Value = "{(Sum(Liability.Column" + column.Column1 + ") - Sum(Liability.Column" + column.Column2 + ")) / Abs(Sum(Liability.Column" + column.Column2 + "))}";
                        footerText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    footerText.HorAlignment = StiTextHorAlignment.Right;
                    footerText.VertAlignment = StiVertAlignment.Center;
                    footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    footerText.OnlyText = false;
                    footerText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    footerText.WordWrap = true;
                    footerText.Margins = new StiMargins(0, 1, 0, 10);
                    footerBand.Components.Add(footerText);

                    pos = pos + columnWidth;
                }
            }

            // Equity
            if (objColumns.Count > 0)
            {
                //Create HeaderBand
                StiHeaderBand headerBand = new StiHeaderBand();
                headerBand.Height = 0.25;
                headerBand.Name = $"EquityHeaderBand";
                headerBand.Border.Style = StiPenStyle.None;
                headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                headerBand.PrintOnAllPages = false;
                headerBand.PrintIfEmpty = true;
                page.Components.Add(headerBand);

                StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                acctText.Text.Value = "Equity";
                acctText.HorAlignment = StiTextHorAlignment.Left;
                acctText.VertAlignment = StiVertAlignment.Center;
                acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                acctText.Border.Side = StiBorderSides.All;
                acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
                acctText.Border.Style = StiPenStyle.None;
                acctText.TextBrush = new StiSolidBrush(Color.White);
                acctText.WordWrap = true;
                headerBand.Components.Add(acctText);

                //Create group header band
                StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                groupHeader.Name = "EquityGroupHeaderBand";
                groupHeader.PrintOnAllPages = false;
                groupHeader.Condition = new StiGroupConditionExpression("{Equity.Sub}");
                page.Components.Add(groupHeader);

                StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                groupHeaderText.Text.Value = "{Equity.Sub}";
                groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                groupHeaderText.VertAlignment = StiVertAlignment.Center;
                groupHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
                groupHeaderText.Border.Style = StiPenStyle.None;
                groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                groupHeaderText.WordWrap = true;
                groupHeader.Components.Add(groupHeaderText);

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "Equity";
                dataBand.Name = $"EquityData";
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);

                StiText acctDataText = new StiText(new RectangleD(0.1, 0, columnWidth * 2 - 0.1, 0.2));
                acctDataText.Text.Value = "{Equity.fDesc}";
                acctDataText.HorAlignment = StiTextHorAlignment.Left;
                acctDataText.VertAlignment = StiVertAlignment.Center;
                acctDataText.Border.Style = StiPenStyle.None;
                acctDataText.OnlyText = false;
                acctDataText.Border.Side = StiBorderSides.All;
                acctDataText.Font = new Font("Arial", 8F);
                acctDataText.WordWrap = true;
                acctDataText.CanGrow = true;
                acctDataText.Margins = new StiMargins(0, 1, 4, 0);
                dataBand.Components.Add(acctDataText);

                pos = (columnWidth * 2);

                foreach (var column in objColumns)
                {
                    //Create text on header
                    StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                    hText.Text.Value = column.Label;
                    hText.HorAlignment = StiTextHorAlignment.Right;
                    hText.VertAlignment = StiVertAlignment.Center;
                    hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                    hText.Border.Side = StiBorderSides.All;
                    hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    hText.Border.Style = StiPenStyle.None;
                    hText.TextBrush = new StiSolidBrush(Color.White);
                    hText.WordWrap = true;
                    hText.CanGrow = true;
                    headerBand.Components.Add(hText);

                    StiText dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                    dataText.Text.Value = "{Equity.Column" + column.Index + "}";

                    if (column.Type != "Variance")
                    {
                        dataText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        dataText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }

                    if (column.Type == "Actual")
                    {
                        dataText.Interaction.Hyperlink = new StiHyperlinkExpression("{Equity.Column" + column.Index + "URL}");
                    }

                    dataText.HorAlignment = StiTextHorAlignment.Right;
                    dataText.VertAlignment = StiVertAlignment.Top;
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.Font = new Font("Arial", 8F);
                    dataText.WordWrap = true;
                    dataText.CanGrow = true;
                    dataText.Margins = new StiMargins(0, 1, 4, 0);
                    dataBand.Components.Add(dataText);

                    pos = pos + columnWidth;
                }

                //Create group footer band
                StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.4));
                groupFooter.Name = "EquityGroupFooterBand";
                page.Components.Add(groupFooter);

                StiText subTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                subTotalText.Text.Value = "Total {Equity.Sub}";
                subTotalText.HorAlignment = StiTextHorAlignment.Left;
                subTotalText.VertAlignment = StiVertAlignment.Center;
                subTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                subTotalText.Border.Style = StiPenStyle.None;
                subTotalText.TextBrush = new StiSolidBrush(Color.Black);
                subTotalText.WordWrap = true;
                groupFooter.Components.Add(subTotalText);

                pos = (columnWidth * 2);
                foreach (var column in objColumns)
                {
                    StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    totalText.HorAlignment = StiTextHorAlignment.Right;
                    totalText.VertAlignment = StiVertAlignment.Center;
                    totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    totalText.TextBrush = new StiSolidBrush(Color.Black);
                    totalText.WordWrap = true;

                    if (column.Type != "Variance")
                    {
                        totalText.Text.Value = "{Sum(Equity.Column" + column.Index + ")}";
                        totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        totalText.Text.Value = "{(Sum(Equity.Column" + column.Column1 + ") - Sum(Equity.Column" + column.Column2 + ")) / Abs(Sum(Equity.Column" + column.Column2 + "))}";
                        totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    groupFooter.Components.Add(totalText);

                    pos = pos + columnWidth;
                }

                //Create FooterBand total
                StiFooterBand footerBand = new StiFooterBand();
                footerBand.Name = $"EquityTotal";
                footerBand.Border.Style = StiPenStyle.None;
                page.Components.Add(footerBand);

                StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                acctTotalText.Text.Value = "Total Equity";
                acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                acctTotalText.VertAlignment = StiVertAlignment.Center;
                acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                acctTotalText.OnlyText = false;
                acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                acctTotalText.WordWrap = true;
                acctTotalText.Margins = new StiMargins(0, 1, 0, 10);
                footerBand.Components.Add(acctTotalText);

                //Create Total Liability & Equity
                StiDataBand totalLEBand = new StiDataBand();
                totalLEBand.DataSourceName = "TotalLiabilityEquity";
                totalLEBand.Name = "TotalLiabilityEquityBand";
                totalLEBand.Border.Style = StiPenStyle.None;
                page.Components.Add(totalLEBand);

                StiText totalLEText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                totalLEText.Text.Value = "Total Liability & Equity";
                totalLEText.HorAlignment = StiTextHorAlignment.Left;
                totalLEText.VertAlignment = StiVertAlignment.Center;
                totalLEText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                totalLEText.OnlyText = false;
                totalLEText.Font = new Font("Arial", 9F, FontStyle.Bold);
                totalLEText.WordWrap = true;
                totalLEText.Margins = new StiMargins(0, 1, 0, 10);
                totalLEBand.Components.Add(totalLEText);

                pos = (columnWidth * 2);
                foreach (var column in objColumns)
                {
                    // Total Equity
                    StiText footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                    if (column.Type != "Variance")
                    {
                        footerText.Text.Value = "{Sum(Equity.Column" + column.Index + ")}";
                        footerText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        footerText.Text.Value = "{(Sum(Equity.Column" + column.Column1 + ") - Sum(Equity.Column" + column.Column2 + ")) / Abs(Sum(Equity.Column" + column.Column2 + "))}";
                        footerText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    footerText.HorAlignment = StiTextHorAlignment.Right;
                    footerText.VertAlignment = StiVertAlignment.Center;
                    footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    footerText.OnlyText = false;
                    footerText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    footerText.WordWrap = true;
                    footerText.Margins = new StiMargins(0, 1, 0, 10);
                    footerBand.Components.Add(footerText);

                    StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                    totalText.Text.Value = "{TotalLiabilityEquity.Column" + column.Index + "}";
                    if (column.Type != "Variance")
                    {
                        totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    totalText.Type = StiSystemTextType.Expression;
                    totalText.HorAlignment = StiTextHorAlignment.Right;
                    totalText.VertAlignment = StiVertAlignment.Center;
                    totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    totalText.OnlyText = false;
                    totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    totalText.WordWrap = true;
                    totalText.Margins = new StiMargins(0, 1, 0, 10);
                    totalLEBand.Components.Add(totalText);

                    pos = pos + columnWidth;
                }
            }

            //report.Compile();

            DataSet dsC = new DataSet();

            var connString = Session["config"].ToString();
            objPropUser.ConnConfig = connString;
            dsC = objBL_User.getControl(objPropUser);

            DataTable cTable = BuildCompanyDetailsTable();
            var cRow = cTable.NewRow();

            DataSet companyInfo = new DataSet();
            companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());

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

            report.RegData("CompanyDetails", cTable);
            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();
            report.Dictionary.Variables["ReportName"].Value = Session["ReportName"].ToString();

            var data = AdjustmentBalanceSheet(comparativeReportData, objComparative);
            DataTable filteredTable = data.Copy();
            DataView dView = filteredTable.DefaultView;

            dView.RowFilter = "Type = 0 OR Type = 6";
            DataTable Assets = dView.ToTable();

            dView.RowFilter = "Type = 2";
            DataTable Equity = dView.ToTable();

            dView.RowFilter = "Type = 99";
            DataTable TotalLiabilityEquity = dView.ToTable();

            dView.RowFilter = "Type = 1";
            DataTable Liability = dView.ToTable();

            report.RegData("Assets", Assets);
            report.RegData("Equity", Equity);
            report.RegData("Liability", Liability);
            report.RegData("TotalLiabilityEquity", TotalLiabilityEquity);

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

    private StiReport GetComparativeBalanceSheetSummaryReport(DataSet comparativeReportData, List<ComparativeStatementRequest> objComparative)
    {
        try
        {
            string reportPathStimul = string.Empty;
            reportPathStimul = Server.MapPath("StimulsoftReports/ComparativeBalanceSheet.mrt");
            StiReport report = new StiReport();
            report.Load(reportPathStimul);

            var objColumns = objComparative;
            foreach (var column in objColumns)
            {
                report.Dictionary.DataSources["AllData"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["Assets"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["Equity"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["Liability"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));
                report.Dictionary.DataSources["TotalLiabilityEquity"].Columns.Add(string.Format("Column{0}", column.Index), typeof(double));

                if (column.Type == "Actual")
                {
                    report.Dictionary.DataSources["Assets"].Columns.Add(string.Format("Column{0}URL", column.Index), typeof(string));
                    report.Dictionary.DataSources["Equity"].Columns.Add(string.Format("Column{0}URL", column.Index), typeof(string));
                    report.Dictionary.DataSources["Liability"].Columns.Add(string.Format("Column{0}URL", column.Index), typeof(string));
                }
            }

            StiPage page = report.Pages[0];
            page.CanGrow = true;
            page.CanShrink = true;

            var columnCount = objColumns.Count + 2;
            double columnWidth = page.Width / columnCount;
            double pos = 0;

            // Assets
            if (objColumns.Count > 0)
            {
                //Create HeaderBand
                StiHeaderBand headerBand = new StiHeaderBand();
                headerBand.Height = 0.25;
                headerBand.Name = "AssetsHeaderBand";
                headerBand.Border.Style = StiPenStyle.None;
                headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                headerBand.PrintOnAllPages = false;
                headerBand.PrintIfEmpty = true;
                page.Components.Add(headerBand);

                StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                acctText.Text.Value = "Assets";
                acctText.HorAlignment = StiTextHorAlignment.Left;
                acctText.VertAlignment = StiVertAlignment.Center;
                acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                acctText.Border.Side = StiBorderSides.All;
                acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
                acctText.Border.Style = StiPenStyle.None;
                acctText.TextBrush = new StiSolidBrush(Color.White);
                acctText.WordWrap = true;
                headerBand.Components.Add(acctText);

                //Create group header band
                StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                groupHeader.Name = "AssetsGroupHeaderBand";
                groupHeader.PrintOnAllPages = false;
                groupHeader.Condition = new StiGroupConditionExpression("{Assets.Sub}");
                page.Components.Add(groupHeader);

                StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                groupHeaderText.Text.Value = "{Assets.Sub}";
                groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                groupHeaderText.VertAlignment = StiVertAlignment.Center;
                groupHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
                groupHeaderText.Border.Style = StiPenStyle.None;
                groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                groupHeaderText.WordWrap = true;
                groupHeader.Components.Add(groupHeaderText);

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "Assets";
                dataBand.Name = $"AssetsData";
                dataBand.Height = 0;
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);

                pos = (columnWidth * 2);

                foreach (var column in objColumns)
                {
                    // Create text on header
                    StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                    hText.Text.Value = column.Label;
                    hText.HorAlignment = StiTextHorAlignment.Right;
                    hText.VertAlignment = StiVertAlignment.Center;
                    hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                    hText.Border.Side = StiBorderSides.All;
                    hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    hText.Border.Style = StiPenStyle.None;
                    hText.TextBrush = new StiSolidBrush(Color.White);
                    hText.WordWrap = true;
                    hText.CanGrow = true;
                    headerBand.Components.Add(hText);

                    // Group item
                    StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    totalText.HorAlignment = StiTextHorAlignment.Right;
                    totalText.VertAlignment = StiVertAlignment.Center;
                    totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    totalText.TextBrush = new StiSolidBrush(Color.Black);
                    totalText.WordWrap = true;

                    if (column.Type != "Variance")
                    {
                        totalText.Text.Value = "{Sum(Assets.Column" + column.Index + ")}";
                        totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        totalText.Text.Value = "{(Sum(Assets.Column" + column.Column1 + ") - Sum(Assets.Column" + column.Column2 + ")) / Abs(Sum(Assets.Column" + column.Column2 + "))}";
                        totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    groupHeader.Components.Add(totalText);

                    pos = pos + columnWidth;
                }

                //Create FooterBand total
                StiFooterBand footerBand = new StiFooterBand();
                footerBand.Name = $"AssetsTotal";
                footerBand.Border.Style = StiPenStyle.None;
                page.Components.Add(footerBand);

                StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                acctTotalText.Text.Value = "Total Assets";
                acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                acctTotalText.VertAlignment = StiVertAlignment.Center;
                acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                acctTotalText.OnlyText = false;
                acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                acctTotalText.WordWrap = true;
                acctTotalText.Margins = new StiMargins(0, 1, 0, 10);
                footerBand.Components.Add(acctTotalText);

                pos = (columnWidth * 2);
                foreach (var column in objColumns)
                {
                    StiText footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                    if (column.Type != "Variance")
                    {
                        footerText.Text.Value = "{Sum(Assets.Column" + column.Index + ")}";
                        footerText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        footerText.Text.Value = "{(Sum(Assets.Column" + column.Column1 + ") - Sum(Assets.Column" + column.Column2 + ")) / Abs(Sum(Assets.Column" + column.Column2 + "))}";
                        footerText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    footerText.HorAlignment = StiTextHorAlignment.Right;
                    footerText.VertAlignment = StiVertAlignment.Center;
                    footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    footerText.OnlyText = false;
                    footerText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    footerText.WordWrap = true;
                    footerText.Margins = new StiMargins(0, 1, 0, 10);
                    footerBand.Components.Add(footerText);

                    pos = pos + columnWidth;
                }
            }

            // Liability
            if (objColumns.Count > 0)
            {
                //Create HeaderBand
                StiHeaderBand headerBand = new StiHeaderBand();
                headerBand.Height = 0.25;
                headerBand.Name = $"LiabilityHeaderBand";
                headerBand.Border.Style = StiPenStyle.None;
                headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                headerBand.PrintOnAllPages = false;
                headerBand.PrintIfEmpty = true;
                page.Components.Add(headerBand);

                StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                acctText.Text.Value = "Liability";
                acctText.HorAlignment = StiTextHorAlignment.Left;
                acctText.VertAlignment = StiVertAlignment.Center;
                acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                acctText.Border.Side = StiBorderSides.All;
                acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
                acctText.Border.Style = StiPenStyle.None;
                acctText.TextBrush = new StiSolidBrush(Color.White);
                acctText.WordWrap = true;
                headerBand.Components.Add(acctText);

                //Create group header band
                StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                groupHeader.Name = "LiabilityGroupHeaderBand";
                groupHeader.PrintOnAllPages = false;
                groupHeader.Condition = new StiGroupConditionExpression("{Liability.Sub}");
                page.Components.Add(groupHeader);

                StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                groupHeaderText.Text.Value = "{Liability.Sub}";
                groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                groupHeaderText.VertAlignment = StiVertAlignment.Center;
                groupHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
                groupHeaderText.Border.Style = StiPenStyle.None;
                groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                groupHeaderText.WordWrap = true;
                groupHeader.Components.Add(groupHeaderText);

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "Liability";
                dataBand.Name = "LiabilityData";
                dataBand.Height = 0;
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);

                pos = (columnWidth * 2);

                foreach (var column in objColumns)
                {
                    // Create text on header
                    StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                    hText.Text.Value = column.Label;
                    hText.HorAlignment = StiTextHorAlignment.Right;
                    hText.VertAlignment = StiVertAlignment.Center;
                    hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                    hText.Border.Side = StiBorderSides.All;
                    hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    hText.Border.Style = StiPenStyle.None;
                    hText.TextBrush = new StiSolidBrush(Color.White);
                    hText.WordWrap = true;
                    hText.CanGrow = true;
                    headerBand.Components.Add(hText);

                    // Group item
                    StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    totalText.HorAlignment = StiTextHorAlignment.Right;
                    totalText.VertAlignment = StiVertAlignment.Center;
                    totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    totalText.TextBrush = new StiSolidBrush(Color.Black);
                    totalText.WordWrap = true;

                    if (column.Type != "Variance")
                    {
                        totalText.Text.Value = "{Sum(Liability.Column" + column.Index + ")}";
                        totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        totalText.Text.Value = "{(Sum(Liability.Column" + column.Column1 + ") - Sum(Liability.Column" + column.Column2 + ")) / Abs(Sum(Liability.Column" + column.Column2 + "))}";
                        totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    groupHeader.Components.Add(totalText);

                    pos = pos + columnWidth;
                }

                //Create FooterBand total
                StiFooterBand footerBand = new StiFooterBand();
                footerBand.Name = $"LiabilityTotal";
                footerBand.Border.Style = StiPenStyle.None;
                page.Components.Add(footerBand);

                StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                acctTotalText.Text.Value = "Total Liability";
                acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                acctTotalText.VertAlignment = StiVertAlignment.Center;
                acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                acctTotalText.OnlyText = false;
                acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                acctTotalText.WordWrap = true;
                acctTotalText.Margins = new StiMargins(0, 1, 0, 10);
                footerBand.Components.Add(acctTotalText);

                pos = (columnWidth * 2);
                foreach (var column in objColumns)
                {
                    StiText footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                    if (column.Type != "Variance")
                    {
                        footerText.Text.Value = "{Sum(Liability.Column" + column.Index + ")}";
                        footerText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        footerText.Text.Value = "{(Sum(Liability.Column" + column.Column1 + ") - Sum(Liability.Column" + column.Column2 + ")) / Abs(Sum(Liability.Column" + column.Column2 + "))}";
                        footerText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    footerText.HorAlignment = StiTextHorAlignment.Right;
                    footerText.VertAlignment = StiVertAlignment.Center;
                    footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    footerText.OnlyText = false;
                    footerText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    footerText.WordWrap = true;
                    footerText.Margins = new StiMargins(0, 1, 0, 10);
                    footerBand.Components.Add(footerText);

                    pos = pos + columnWidth;
                }
            }

            // Equity
            if (objColumns.Count > 0)
            {
                //Create HeaderBand
                StiHeaderBand headerBand = new StiHeaderBand();
                headerBand.Height = 0.25;
                headerBand.Name = $"EquityHeaderBand";
                headerBand.Border.Style = StiPenStyle.None;
                headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                headerBand.PrintOnAllPages = false;
                headerBand.PrintIfEmpty = true;
                page.Components.Add(headerBand);

                StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                acctText.Text.Value = "Equity";
                acctText.HorAlignment = StiTextHorAlignment.Left;
                acctText.VertAlignment = StiVertAlignment.Center;
                acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                acctText.Border.Side = StiBorderSides.All;
                acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
                acctText.Border.Style = StiPenStyle.None;
                acctText.TextBrush = new StiSolidBrush(Color.White);
                acctText.WordWrap = true;
                headerBand.Components.Add(acctText);

                //Create group header band
                StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                groupHeader.Name = "EquityGroupHeaderBand";
                groupHeader.PrintOnAllPages = false;
                groupHeader.Condition = new StiGroupConditionExpression("{Equity.Sub}");
                page.Components.Add(groupHeader);

                StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                groupHeaderText.Text.Value = "{Equity.Sub}";
                groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                groupHeaderText.VertAlignment = StiVertAlignment.Center;
                groupHeaderText.Font = new Font("Arial", 9F, FontStyle.Bold);
                groupHeaderText.Border.Style = StiPenStyle.None;
                groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                groupHeaderText.WordWrap = true;
                groupHeader.Components.Add(groupHeaderText);

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "Equity";
                dataBand.Name = "EquityData";
                dataBand.Height = 0;
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);

                pos = (columnWidth * 2);

                foreach (var column in objColumns)
                {
                    // Create text on header
                    StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                    hText.Text.Value = column.Label;
                    hText.HorAlignment = StiTextHorAlignment.Right;
                    hText.VertAlignment = StiVertAlignment.Center;
                    hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                    hText.Border.Side = StiBorderSides.All;
                    hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    hText.Border.Style = StiPenStyle.None;
                    hText.TextBrush = new StiSolidBrush(Color.White);
                    hText.WordWrap = true;
                    hText.CanGrow = true;
                    headerBand.Components.Add(hText);

                    // Group item
                    StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    totalText.HorAlignment = StiTextHorAlignment.Right;
                    totalText.VertAlignment = StiVertAlignment.Center;
                    totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    totalText.TextBrush = new StiSolidBrush(Color.Black);
                    totalText.WordWrap = true;

                    if (column.Type != "Variance")
                    {
                        totalText.Text.Value = "{Sum(Equity.Column" + column.Index + ")}";
                        totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        totalText.Text.Value = "{(Sum(Equity.Column" + column.Column1 + ") - Sum(Equity.Column" + column.Column2 + ")) / Abs(Sum(Equity.Column" + column.Column2 + "))}";
                        totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    groupHeader.Components.Add(totalText);

                    pos = pos + columnWidth;
                }

                //Create FooterBand total
                StiFooterBand footerBand = new StiFooterBand();
                footerBand.Name = $"EquityTotal";
                footerBand.Border.Style = StiPenStyle.None;
                page.Components.Add(footerBand);

                StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                acctTotalText.Text.Value = "Total Equity";
                acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                acctTotalText.VertAlignment = StiVertAlignment.Center;
                acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                acctTotalText.OnlyText = false;
                acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                acctTotalText.WordWrap = true;
                acctTotalText.Margins = new StiMargins(0, 1, 0, 10);
                footerBand.Components.Add(acctTotalText);

                //Create Total Liability & Equity
                StiDataBand totalLEBand = new StiDataBand();
                totalLEBand.DataSourceName = "TotalLiabilityEquity";
                totalLEBand.Name = "TotalLiabilityEquityBand";
                totalLEBand.Border.Style = StiPenStyle.None;
                totalLEBand.NewPageAfter = true;
                page.Components.Add(totalLEBand);

                StiText totalLEText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                totalLEText.Text.Value = "Total Liability & Equity";
                totalLEText.HorAlignment = StiTextHorAlignment.Left;
                totalLEText.VertAlignment = StiVertAlignment.Center;
                totalLEText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                totalLEText.OnlyText = false;
                totalLEText.Font = new Font("Arial", 9F, FontStyle.Bold);
                totalLEText.WordWrap = true;
                totalLEText.Margins = new StiMargins(0, 1, 0, 10);
                totalLEBand.Components.Add(totalLEText);

                pos = (columnWidth * 2);
                foreach (var column in objColumns)
                {
                    // Total Equity
                    StiText footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                    if (column.Type != "Variance")
                    {
                        footerText.Text.Value = "{Sum(Equity.Column" + column.Index + ")}";
                        footerText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        footerText.Text.Value = "{(Sum(Equity.Column" + column.Column1 + ") - Sum(Equity.Column" + column.Column2 + ")) / Abs(Sum(Equity.Column" + column.Column2 + "))}";
                        footerText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    footerText.HorAlignment = StiTextHorAlignment.Right;
                    footerText.VertAlignment = StiVertAlignment.Center;
                    footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    footerText.OnlyText = false;
                    footerText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    footerText.WordWrap = true;
                    footerText.Margins = new StiMargins(0, 1, 0, 10);
                    footerBand.Components.Add(footerText);

                    StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                    totalText.Text.Value = "{TotalLiabilityEquity.Column" + column.Index + "}";
                    if (column.Type != "Variance")
                    {
                        totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    totalText.Type = StiSystemTextType.Expression;
                    totalText.HorAlignment = StiTextHorAlignment.Right;
                    totalText.VertAlignment = StiVertAlignment.Center;
                    totalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    totalText.OnlyText = false;
                    totalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    totalText.WordWrap = true;
                    totalText.Margins = new StiMargins(0, 1, 0, 10);
                    totalLEBand.Components.Add(totalText);

                    pos = pos + columnWidth;
                }
            }

            // Selected department 
            if (objColumns.Count > 0)
            {
                //Create HeaderBand
                StiHeaderBand headerBand = new StiHeaderBand();
                headerBand.Height = 0.25;
                headerBand.Name = "AllDataHeaderBand";
                headerBand.Border.Style = StiPenStyle.None;
                headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                headerBand.PrintOnAllPages = false;
                headerBand.PrintIfEmpty = true;
                page.Components.Add(headerBand);

                StiText acctText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.25));
                acctText.Text.Value = "Department";
                acctText.HorAlignment = StiTextHorAlignment.Left;
                acctText.VertAlignment = StiVertAlignment.Center;
                acctText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                acctText.Border.Side = StiBorderSides.All;
                acctText.Font = new Font("Arial", 9F, FontStyle.Bold);
                acctText.Border.Style = StiPenStyle.None;
                acctText.TextBrush = new StiSolidBrush(Color.White);
                acctText.WordWrap = true;
                headerBand.Components.Add(acctText);

                //Create group header band
                StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                groupHeader.Name = "AllDataGroupHeaderBand";
                groupHeader.PrintOnAllPages = false;
                groupHeader.Condition = new StiGroupConditionExpression("{AllData.CentralName}");
                page.Components.Add(groupHeader);

                StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                groupHeaderText.Text.Value = "{AllData.CentralName}";
                groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                groupHeaderText.VertAlignment = StiVertAlignment.Center;
                groupHeaderText.Font = new Font("Arial", 9F);
                groupHeaderText.Border.Style = StiPenStyle.None;
                groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                groupHeaderText.WordWrap = true;
                groupHeader.Components.Add(groupHeaderText);

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "AllData";
                dataBand.Name = "AllDataBand";
                dataBand.Height = 0;
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);

                pos = (columnWidth * 2);

                foreach (var column in objColumns)
                {
                    // Create text on header
                    StiText hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));

                    hText.Text.Value = column.Label;
                    hText.HorAlignment = StiTextHorAlignment.Right;
                    hText.VertAlignment = StiVertAlignment.Center;
                    hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                    hText.Border.Side = StiBorderSides.All;
                    hText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    hText.Border.Style = StiPenStyle.None;
                    hText.TextBrush = new StiSolidBrush(Color.White);
                    hText.WordWrap = true;
                    hText.CanGrow = true;
                    headerBand.Components.Add(hText);

                    // Group item
                    StiText totalText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    totalText.HorAlignment = StiTextHorAlignment.Right;
                    totalText.VertAlignment = StiVertAlignment.Center;
                    totalText.Font = new Font("Arial", 9F);
                    totalText.TextBrush = new StiSolidBrush(Color.Black);
                    totalText.WordWrap = true;

                    if (column.Type != "Variance")
                    {
                        totalText.Text.Value = "{Sum(AllData.Column" + column.Index + ")}";
                        totalText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        totalText.Text.Value = "{(Sum(AllData.Column" + column.Column1 + ") - Sum(AllData.Column" + column.Column2 + ")) / Abs(Sum(AllData.Column" + column.Column2 + "))}";
                        totalText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    groupHeader.Components.Add(totalText);

                    pos = pos + columnWidth;
                }

                //Create FooterBand total
                StiFooterBand footerBand = new StiFooterBand();
                footerBand.Name = $"AllDataTotal";
                footerBand.Border.Style = StiPenStyle.None;
                page.Components.Add(footerBand);

                StiText acctTotalText = new StiText(new RectangleD(0, 0, columnWidth * 2, 0.3));
                acctTotalText.Text.Value = "Total";
                acctTotalText.HorAlignment = StiTextHorAlignment.Left;
                acctTotalText.VertAlignment = StiVertAlignment.Center;
                acctTotalText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                acctTotalText.OnlyText = false;
                acctTotalText.Font = new Font("Arial", 9F, FontStyle.Bold);
                acctTotalText.WordWrap = true;
                acctTotalText.Margins = new StiMargins(0, 1, 0, 10);
                footerBand.Components.Add(acctTotalText);

                pos = (columnWidth * 2);
                foreach (var column in objColumns)
                {
                    StiText footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.3));
                    if (column.Type != "Variance")
                    {
                        footerText.Text.Value = "{Sum(AllData.Column" + column.Index + ")}";
                        footerText.TextFormat = new StiNumberFormatService(1, ".", 2, ",", 3, true, false, " ");
                    }
                    else
                    {
                        footerText.Text.Value = "{(Sum(AllData.Column" + column.Column1 + ") - Sum(AllData.Column" + column.Column2 + ")) / Abs(Sum(AllData.Column" + column.Column2 + "))}";
                        footerText.TextFormat = new StiPercentageFormatService(1, 1, ".", 2, ",", 3, "%", true, false, " ");
                    }
                    footerText.HorAlignment = StiTextHorAlignment.Right;
                    footerText.VertAlignment = StiVertAlignment.Center;
                    footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                    footerText.OnlyText = false;
                    footerText.Font = new Font("Arial", 9F, FontStyle.Bold);
                    footerText.WordWrap = true;
                    footerText.Margins = new StiMargins(0, 1, 0, 10);
                    footerBand.Components.Add(footerText);

                    pos = pos + columnWidth;
                }
            }

            //report.Compile();

            DataSet dsC = new DataSet();

            var connString = Session["config"].ToString();
            objPropUser.ConnConfig = connString;
            dsC = objBL_User.getControl(objPropUser);

            DataTable cTable = BuildCompanyDetailsTable();
            var cRow = cTable.NewRow();

            DataSet companyInfo = new DataSet();
            companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());

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

            report.RegData("CompanyDetails", cTable);
            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();
            report.Dictionary.Variables["ReportName"].Value = Session["ReportName"].ToString();

            var allData = comparativeReportData.Tables[0].Copy();
            var data = AdjustmentBalanceSheet(comparativeReportData, objComparative);
            DataTable filteredTable = data.Copy();
            DataView dView = filteredTable.DefaultView;

            dView.RowFilter = "Type = 0 OR Type = 6";
            DataTable Assets = dView.ToTable();

            dView.RowFilter = "Type = 2";
            DataTable Equity = dView.ToTable();

            dView.RowFilter = "Type = 99";
            DataTable TotalLiabilityEquity = dView.ToTable();

            dView.RowFilter = "Type = 1";
            DataTable Liability = dView.ToTable();

            report.RegData("AllData", allData);
            report.RegData("Assets", Assets);
            report.RegData("Equity", Equity);
            report.RegData("Liability", Liability);
            report.RegData("TotalLiabilityEquity", TotalLiabilityEquity);

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

    protected DataTable BuildColumnDetailsTable()
    {
        DataTable columnDetailsTable = new DataTable();
        columnDetailsTable.Columns.Add("Line");
        columnDetailsTable.Columns.Add("Index");
        columnDetailsTable.Columns.Add("Type");
        columnDetailsTable.Columns.Add("Budget");
        columnDetailsTable.Columns.Add("Label");
        columnDetailsTable.Columns.Add("FromDate");
        columnDetailsTable.Columns.Add("ToDate");
        columnDetailsTable.Columns.Add("Column1");
        columnDetailsTable.Columns.Add("Column2");

        return columnDetailsTable;
    }

    private void LoadReportByID(int reportID, bool isEdit)
    {
        var connString = Session["config"].ToString();
        var report = bL_Report.GetComparativeReportByID(connString, reportID);

        if (report.Tables[0].Rows.Count > 0)
        {
            if (isEdit)
            {
                txtReportTitle.Value = report.Tables[0].Rows[0]["Name"].ToString();
                Session["ReportName"] = report.Tables[0].Rows[0]["Name"].ToString();
            }

            ddlStates.SelectedValue = report.Tables[0].Rows[0]["States"].ToString();
            if (ddlStates.SelectedValue == "ProfitAndLoss")
            {
                pageTitle.Text = "Comparative Profit and Loss Statement";

                gvListColumns.Columns[4].HeaderText = "Start Date/Column";
                gvListColumns.Columns[5].HeaderText = "End Date/Column";
            }
            else if (ddlStates.SelectedValue == "BalanceSheet")
            {
                pageTitle.Text = "Comparative Balance Sheet";

                gvListColumns.Columns[4].HeaderText = "Column";
                gvListColumns.Columns[5].HeaderText = "As of Date/Column";
            }

            var depts = report.Tables[0].Rows[0]["Departments"].ToString();
            var deptArray = depts.Split(',');

            for (int i = 0; i < deptArray.Length; i++)
            {
                RadComboBoxItem item = rcCenter.FindItemByValue(deptArray[i]);
                if (item != null)
                    item.Checked = true;
            }

            List<ComparativeStatementRequest> objComparative = new List<ComparativeStatementRequest>();
            var dsReport = bL_Report.GetComparativeReportColumns(connString, reportID);

            if (dsReport != null && dsReport.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dsReport.Tables[0].Rows)
                {
                    ComparativeStatementRequest obj = new ComparativeStatementRequest();

                    obj.Line = Convert.ToInt32(row["Line"].ToString());
                    obj.Index = Convert.ToInt32(row["Index"].ToString());
                    obj.Type = row["Type"].ToString();
                    obj.Label = row["Label"].ToString();

                    if (obj.Type != "Difference" && obj.Type != "Variance")
                    {
                        if (ddlStates.SelectedValue == "ProfitAndLoss")
                        {
                            obj.StartDate = Convert.ToDateTime(row["FromDate"].ToString());
                        }

                        obj.EndDate = Convert.ToDateTime(row["ToDate"].ToString());
                    }
                    else
                    {
                        obj.Column1 = Convert.ToInt32(row["Column1"].ToString());
                        obj.Column2 = Convert.ToInt32(row["Column2"].ToString());
                    }

                    objComparative.Add(obj);
                }

                dsReport.Tables[0].Columns.Add("Budget");

                foreach (DataRow dr in dsReport.Tables[0].Rows)
                {
                    if (dr["Type"].ToString() != "Actual" && dr["Type"].ToString() != "Difference" && dr["Type"].ToString() != "Variance")
                    {
                        dr["Budget"] = dr["Type"];
                        dr["Type"] = "Budget";
                    }
                }

                gvListColumns.DataSource = dsReport;
                gvListColumns.DataBind();

                Session["ComparativeStatementRequest"] = objComparative;
                Session["ComparativeCenter"] = GetRadComboBoxSelectedItems(rcCenter);
                Session["SelectedCenters"] = GetSelectedCenters();
            }
        }
        else
        {
            Response.Redirect("ComparativeStatement.aspx");
        }
    }

    private void LoadReportData()
    {
        var connString = Session["config"].ToString();

        List<ComparativeStatementRequest> objComparative = new List<ComparativeStatementRequest>();
        foreach (GridDataItem row in gvListColumns.Items)
        {
            var rowIndex = Convert.ToInt32(row.GetDataKeyValue("Index").ToString());

            HiddenField rowLine = (HiddenField)row.FindControl("txtRowLine");
            DropDownList lblDbType = (DropDownList)row.FindControl("lblDbType");
            TextBox lblDbLabel = (TextBox)row.FindControl("lblDbLabel");
            TextBox lblDbFromDate = (TextBox)row.FindControl("lblDbFromDate");
            TextBox lblDbToDate = (TextBox)row.FindControl("lblDbToDate");
            DropDownList lblDbColumn1 = (DropDownList)row.FindControl("lblDbColumn1");
            DropDownList lblDbColumn2 = (DropDownList)row.FindControl("lblDbColumn2");
            RadComboBox ddlBudget = (RadComboBox)row.FindControl("rcbDbBudget");

            ComparativeStatementRequest obj = new ComparativeStatementRequest();
            obj.Line = Convert.ToInt32(rowLine.Value);
            obj.Index = rowIndex;
            obj.Label = lblDbLabel.Text;
            obj.Type = lblDbType.SelectedValue;

            if (lblDbType.SelectedValue == "Budget")
            {
                obj.Type = ddlBudget.Text;
            }

            if (lblDbType.SelectedValue != "Difference" && lblDbType.SelectedValue != "Variance")
            {
                if (ddlStates.SelectedValue == "ProfitAndLoss")
                {
                    obj.StartDate = Convert.ToDateTime(lblDbFromDate.Text);
                }

                obj.EndDate = Convert.ToDateTime(lblDbToDate.Text);
            }
            else
            {
                if (!string.IsNullOrEmpty(lblDbColumn1.SelectedValue))
                {
                    obj.Column1 = Convert.ToInt32(lblDbColumn1.SelectedValue);
                }

                if (!string.IsNullOrEmpty(lblDbColumn2.SelectedValue))
                {
                    obj.Column2 = Convert.ToInt32(lblDbColumn2.SelectedValue);
                }
            }

            objComparative.Add(obj);
        }

        // Sync index by line
        foreach (var col in objComparative)
        {
            if (col.Type == "Difference" || col.Type == "Variance")
            {
                col.Column1 = objComparative.FirstOrDefault(x => x.Index == col.Column1)?.Line;
                col.Column2 = objComparative.FirstOrDefault(x => x.Index == col.Column2)?.Line;
            }
        }

        objComparative.ForEach(x => x.Index = x.Line);

        Session["ComparativeStatementRequest"] = objComparative;
        Session["ComparativeCenter"] = GetRadComboBoxSelectedItems(rcCenter);
        Session["SelectedCenters"] = GetSelectedCenters();
    }

    private StiReport LoadReport()
    {
        try
        {
            var connString = Session["config"].ToString();
            objChart.ConnConfig = connString;

            List<ComparativeStatementRequest> objComparative = new List<ComparativeStatementRequest>();
            var type = "ProfitAndLoss";

            if (Session["ComparativeStatementRequest"] != null)
            {
                objComparative = (List<ComparativeStatementRequest>)Session["ComparativeStatementRequest"];
            }

            if (Session["ComparativeCenter"] != null)
            {
                objChart.Departments = Session["ComparativeCenter"].ToString();
            }

            if (!string.IsNullOrEmpty(Request.QueryString["type"]))
            {
                type = Request.QueryString["type"];
            }
            else
            {
                var reportID = 0;

                if (Request.QueryString["id"] != null)
                {
                    reportID = Convert.ToInt32(Request.QueryString["id"]);
                }
                else if (Request.QueryString["cid"] != null)
                {
                    reportID = Convert.ToInt32(Request.QueryString["cid"]);
                }

                if (reportID > 0)
                {
                    var report = bL_Report.GetComparativeReportByID(connString, reportID);
                    type = report.Tables[0].Rows[0]["States"].ToString();
                }
            }

            var reportType = "ExpandAll";
            if (!string.IsNullOrEmpty(Request.QueryString["reportType"]))
            {
                reportType = Request.QueryString["reportType"];
            }

            if (type == "ProfitAndLoss")
            {
                if (reportType == "CollapseAll")
                {
                    var ds = bL_Report.GetComparativeStatementSummaryData(objChart, objComparative);
                    return GetComparativeProfitAndLossSummaryReport(ds.Tables[0], objComparative);
                }
                else
                {
                    var ds = bL_Report.GetComparativeStatementData(objChart, objComparative);

                    // Set url
                    foreach (var col in objComparative)
                    {
                        if (col.Type == "Actual")
                        {
                            ds.Tables[0].AsEnumerable().ToList()
                            .ForEach(b => b[string.Format("Column{0}URL", col.Index)] =
                                (Request.Url.Scheme +
                                    (Uri.SchemeDelimiter +
                                        (Request.Url.Authority +
                                            (Request.ApplicationPath + "/accountledger.aspx?id=" + b["Acct"].ToString()
                                                + "&s=" + HttpUtility.UrlEncode(col.StartDate.Value.ToShortDateString())
                                                + "&e=" + HttpUtility.UrlEncode(col.EndDate.Value.ToShortDateString())
                                            )
                                        )
                                    )
                                )
                            );

                            ds.Tables[0].AcceptChanges();
                        }
                    }

                    if (reportType == "ExpandAll")
                    {
                        return GetComparativeProfitAndLossReport(ds.Tables[0], objComparative);
                    }
                    else if (reportType == "DetailWithSub")
                    {
                        return GetComparativeProfitAndLossWithSubReport(ds.Tables[0], objComparative);
                    }
                }
            }
            else if (type == "BalanceSheet")
            {
                var ds = bL_Report.GetComparativeBalanceSheetData(objChart, objComparative);
                if (reportType == "CollapseAll")
                {
                    return GetComparativeBalanceSheetSummaryReport(ds, objComparative);
                }
                else
                {
                    var startDate = Convert.ToDateTime(ds.Tables[1].Rows[0][0]).ToShortDateString();

                    // Set url
                    foreach (var col in objComparative)
                    {
                        if (col.Type == "Actual")
                        {
                            ds.Tables[0].AsEnumerable().ToList()
                            .ForEach(b => b[string.Format("Column{0}URL", col.Index)] =
                                (Request.Url.Scheme +
                                    (Uri.SchemeDelimiter +
                                        (Request.Url.Authority +
                                            (Request.ApplicationPath + "/accountledger.aspx?id=" + b["Acct"].ToString()
                                                + "&s=" + HttpUtility.UrlEncode(startDate)
                                                + "&e=" + HttpUtility.UrlEncode(col.EndDate.Value.ToShortDateString())
                                            )
                                        )
                                    )
                                )
                            );

                            ds.Tables[0].AcceptChanges();
                        }
                    }

                    if (reportType == "ExpandAll")
                    {
                        return GetComparativeBalanceSheetReport(ds, objComparative);
                    }
                    else if (reportType == "DetailWithSub")
                    {
                        return GetComparativeBalanceSheetWithSubReport(ds, objComparative);
                    }
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return null;
        }
    }

    private DataTable SummaryTotal(DataTable dt, List<ComparativeStatementRequest> objComparative)
    {
        var objColumns = objComparative;
        DataTable summaryTable = new DataTable();
        summaryTable.Columns.Add("Type");

        foreach (var column in objColumns)
        {
            summaryTable.Columns.Add("Column" + column.Index.ToString());
        }

        DataTable filteredTable = dt.Copy();
        DataView dView = filteredTable.DefaultView;

        dView.RowFilter = "Type = 3";
        DataTable dtRevenues = dView.ToTable();

        dView.RowFilter = "Type = 4";
        DataTable dtCostOfSales = dView.ToTable();

        dView.RowFilter = "Type = 5";
        DataTable dtExpenses = dView.ToTable();

        dView.RowFilter = "Type = 8";
        DataTable dtOtherIncomes = dView.ToTable();

        dView.RowFilter = "Type = 9";
        DataTable dtIncomeTaxes = dView.ToTable();

        DataRow drRevenues = summaryTable.NewRow();
        DataRow drCostOfSales = summaryTable.NewRow();
        DataRow drExpenses = summaryTable.NewRow();
        DataRow drOtherIncome = summaryTable.NewRow();
        DataRow drIncomeTaxes = summaryTable.NewRow();
        DataRow drGrossProfit = summaryTable.NewRow();
        DataRow drNetProfit = summaryTable.NewRow();
        DataRow drBeforeProvisions = summaryTable.NewRow();
        DataRow drNetIncome = summaryTable.NewRow();
        DataRow drGrossProfitPercen = summaryTable.NewRow();
        DataRow drNetProfitPercen = summaryTable.NewRow();
        DataRow drBeforeProvisionsPercen = summaryTable.NewRow();
        DataRow drNetIncomePercen = summaryTable.NewRow();

        drRevenues["Type"] = 3;
        drCostOfSales["Type"] = 4;
        drExpenses["Type"] = 5;
        drOtherIncome["Type"] = 8;
        drIncomeTaxes["Type"] = 9;
        drGrossProfit["Type"] = 41;
        drGrossProfitPercen["Type"] = 42;
        drNetProfit["Type"] = 51;
        drNetProfitPercen["Type"] = 52;
        drBeforeProvisions["Type"] = 81;
        drBeforeProvisionsPercen["Type"] = 82;
        drNetIncome["Type"] = 91;
        drNetIncomePercen["Type"] = 92;

        var revenRows = dtRevenues.Copy().Rows.OfType<DataRow>();
        var costRows = dtCostOfSales.Copy().Rows.OfType<DataRow>();
        var expenRows = dtExpenses.Copy().Rows.OfType<DataRow>();
        var otherRows = dtOtherIncomes.Copy().Rows.OfType<DataRow>();
        var taxRows = dtIncomeTaxes.Copy().Rows.OfType<DataRow>();

        foreach (var column in objColumns)
        {
            if (column.Type != "Variance")
            {
                var revenuTotal = revenRows.Sum(r => Convert.ToDouble(r["Column" + column.Index.ToString()].ToString()));
                var costTotal = costRows.Sum(r => Convert.ToDouble(r["Column" + column.Index.ToString()].ToString()));
                var expenTotal = expenRows.Sum(r => Convert.ToDouble(r["Column" + column.Index.ToString()].ToString()));
                var otherTotal = otherRows.Sum(r => Convert.ToDouble(r["Column" + column.Index.ToString()].ToString()));
                var taxTotal = taxRows.Sum(r => Convert.ToDouble(r["Column" + column.Index.ToString()].ToString()));

                drRevenues["Column" + column.Index.ToString()] = revenuTotal;
                drCostOfSales["Column" + column.Index.ToString()] = costTotal;
                drExpenses["Column" + column.Index.ToString()] = expenTotal;
                drOtherIncome["Column" + column.Index.ToString()] = otherTotal;
                drIncomeTaxes["Column" + column.Index.ToString()] = taxTotal;

                drGrossProfit["Column" + column.Index.ToString()] = revenuTotal - costTotal;
                drNetProfit["Column" + column.Index.ToString()] = revenuTotal - costTotal - expenTotal;
                drBeforeProvisions["Column" + column.Index.ToString()] = revenuTotal - costTotal - expenTotal + otherTotal;
                drNetIncome["Column" + column.Index.ToString()] = revenuTotal - costTotal - expenTotal + otherTotal - taxTotal;

                drGrossProfitPercen["Column" + column.Index.ToString()] = revenuTotal == 0 ? 0 : (revenuTotal - costTotal) / revenuTotal;
                drNetProfitPercen["Column" + column.Index.ToString()] = revenuTotal == 0 ? 0 : (revenuTotal - costTotal - expenTotal) / revenuTotal;
                drBeforeProvisionsPercen["Column" + column.Index.ToString()] = revenuTotal == 0 ? 0 : (revenuTotal - costTotal - expenTotal + otherTotal) / revenuTotal;
                drNetIncomePercen["Column" + column.Index.ToString()] = revenuTotal == 0 ? 0 : (revenuTotal - costTotal - expenTotal + otherTotal - taxTotal) / revenuTotal;
            }
        }

        summaryTable.Rows.Add(drRevenues);
        summaryTable.Rows.Add(drCostOfSales);
        summaryTable.Rows.Add(drExpenses);
        summaryTable.Rows.Add(drGrossProfit);
        summaryTable.Rows.Add(drNetProfit);
        summaryTable.Rows.Add(drGrossProfitPercen);
        summaryTable.Rows.Add(drNetProfitPercen);

        if (otherRows.Count() > 0 || taxRows.Count() > 0)
        {
            if (otherRows.Count() > 0)
            {
                summaryTable.Rows.Add(drOtherIncome);
                summaryTable.Rows.Add(drBeforeProvisions);
                summaryTable.Rows.Add(drBeforeProvisionsPercen);
            }

            if (taxRows.Count() > 0)
            {
                summaryTable.Rows.Add(drIncomeTaxes);
            }

            summaryTable.Rows.Add(drNetIncome);
            summaryTable.Rows.Add(drNetIncomePercen);
        }

        foreach (DataRow dr in summaryTable.Rows)
        {
            foreach (var column in objColumns)
            {
                if (column.Type == "Variance")
                {
                    var col1 = Convert.ToDouble(dr["Column" + column.Column1.ToString()].ToString());
                    var col2 = Convert.ToDouble(dr["Column" + column.Column2.ToString()].ToString());
                    dr["Column" + column.Index.ToString()] = col2 == 0 ? 0 : (col1 - col2) / Math.Abs(col2);
                }
            }
        }

        return summaryTable;
    }

    private DataTable GetColumnIndex()
    {
        DataTable columnIndex = new DataTable();
        columnIndex.Columns.Add("Index");
        columnIndex.Columns.Add("Name");

        foreach (GridDataItem row in gvListColumns.Items)
        {
            DropDownList lblDbType = (DropDownList)row.FindControl("lblDbType");
            TextBox lblDbLabel = (TextBox)row.FindControl("lblDbLabel");

            if (lblDbType.SelectedValue != "" && lblDbType.SelectedValue != "Difference" && lblDbType.SelectedValue != "Variance")
            {
                DataRow dr = columnIndex.NewRow();
                dr["Index"] = row.GetDataKeyValue("Index").ToString();
                dr["Name"] = lblDbLabel.Text;

                columnIndex.Rows.Add(dr);
            }
        }

        return columnIndex;
    }

    private DataTable BuildTotalTable(List<ComparativeStatementRequest> objComparative)
    {
        DataTable totalTable = new DataTable();
        var objColumns = objComparative;
        foreach (var column in objColumns)
        {
            totalTable.Columns.Add("Column" + column.Index.ToString());
        }

        totalTable.Columns.Add("Department");

        return totalTable;
    }

    private DataTable AdjustmentBalanceSheet(DataSet dsData, List<ComparativeStatementRequest> objComparative)
    {
        bool addNewCurrentEarnings = false;

        var dtReportData = dsData.Tables[0];
        var dtAllData = dsData.Tables[2];

        // Current Earnings account
        DataSet _dsCurrentEarn = objBL_Chart.GetCurrentEarn(objChart);
        int currAcct = Convert.ToInt32(_dsCurrentEarn.Tables[0].Rows[0]["ID"]);

        // Get Current Earnings row
        DataRow currRow = dtReportData.Select($"Acct = '{currAcct}'").FirstOrDefault();
        DataRow currRowToCal = dtAllData.Select($"Acct = '{currAcct}'").FirstOrDefault();

        // New Current Earnings row & Total row
        var newCurrRow = dtReportData.NewRow();
        var totalRow = dtReportData.NewRow();
        totalRow["Type"] = 99;

        // Calculate the new Current Earnings row
        if (currRow != null)
        {
            newCurrRow.ItemArray = currRow.ItemArray.Clone() as object[];

            // Remove Current Earnings row 
            dtReportData.Rows.Remove(currRow);
            dtReportData.AcceptChanges();

            dtAllData.Rows.Remove(currRowToCal);
            dtAllData.AcceptChanges();
        }
        else
        {
            var currentEarn = _dsCurrentEarn.Tables[0].Rows[0];

            newCurrRow["Acct"] = currentEarn["ID"];
            newCurrRow["AcctNo"] = currentEarn["Acct"];
            newCurrRow["AcctName"] = currentEarn["fDesc"];
            newCurrRow["fDesc"] = $"{currentEarn["Acct"]} {currentEarn["fDesc"]}";
            newCurrRow["Type"] = currentEarn["Type"];
            newCurrRow["TypeName"] = currentEarn["TypeName"];
            newCurrRow["Sub"] = string.IsNullOrEmpty(currentEarn["Sub"].ToString()) ? currentEarn["TypeName"] : currentEarn["Sub"];
            newCurrRow["Department"] = currentEarn["Department"];
            newCurrRow["CentralName"] = currentEarn["CentralName"];
        }

        foreach (var col in objComparative)
        {
            if (col.Type == "Actual")
            {
                var colName = string.Format("Column{0}", col.Index);

                var assetCurr = dtAllData.Compute($"Sum({colName})", "Type = 0 OR Type = 6");
                var liabilityCurr = dtAllData.Compute($"Sum({colName})", "Type = 1");
                var equityCurr = dtAllData.Compute($"Sum({colName})", "Type = 2");

                assetCurr = string.IsNullOrEmpty(assetCurr.ToString()) ? 0 : assetCurr;
                liabilityCurr = string.IsNullOrEmpty(liabilityCurr.ToString()) ? 0 : liabilityCurr;
                equityCurr = string.IsNullOrEmpty(equityCurr.ToString()) ? 0 : equityCurr;

                var diffAmount = Convert.ToDouble(assetCurr) - Convert.ToDouble(liabilityCurr) - Convert.ToDouble(equityCurr);

                newCurrRow[colName] = diffAmount;

                if (diffAmount != 0)
                {
                    addNewCurrentEarnings = true;
                }
            }
        }

        if (addNewCurrentEarnings)
        {
            dtReportData.Rows.Add(newCurrRow);
            dtReportData.AcceptChanges();
        }

        // Calculate the sum Liability + Equity
        foreach (var col in objComparative)
        {
            if (col.Type == "Actual")
            {
                var colName = string.Format("Column{0}", col.Index);

                var liabilityTotal = dtReportData.Compute($"Sum({colName})", "Type = 1");
                var equityTotal = dtReportData.Compute($"Sum({colName})", "Type = 2");

                liabilityTotal = string.IsNullOrEmpty(liabilityTotal.ToString()) ? 0 : liabilityTotal;
                equityTotal = string.IsNullOrEmpty(equityTotal.ToString()) ? 0 : equityTotal;

                totalRow[colName] = Convert.ToDouble(liabilityTotal) + Convert.ToDouble(equityTotal);
            }
        }

        // Calculate the Difference & Variance column
        foreach (var col in objComparative)
        {
            var colName = string.Format("Column{0}", col.Index);
            if (col.Type == "Difference")
            {
                newCurrRow[colName] = Convert.ToDouble(newCurrRow[$"Column{col.Column1}"]) - Convert.ToDouble(newCurrRow[$"Column{col.Column2}"]);
                totalRow[colName] = Convert.ToDouble(totalRow[$"Column{col.Column1}"]) - Convert.ToDouble(totalRow[$"Column{col.Column2}"]);
            }

            if (col.Type == "Variance")
            {
                newCurrRow[colName] = Convert.ToDouble(newCurrRow[$"Column{col.Column2}"]) == 0 ?
                    0 : (Convert.ToDouble(newCurrRow[$"Column{col.Column1}"]) - Convert.ToDouble(newCurrRow[$"Column{col.Column2}"])) / Math.Abs(Convert.ToDouble(newCurrRow[$"Column{col.Column2}"]));

                totalRow[colName] = Convert.ToDouble(totalRow[$"Column{col.Column2}"]) == 0 ?
                        0 : (Convert.ToDouble(totalRow[$"Column{col.Column1}"]) - Convert.ToDouble(totalRow[$"Column{col.Column2}"])) / Math.Abs(Convert.ToDouble(totalRow[$"Column{col.Column2}"]));
            }
        }

        dtReportData.Rows.Add(totalRow);
        dtReportData.AcceptChanges();

        return dtReportData;
    }

    private void CenterSummaryTotal(DataTable dt, List<ComparativeStatementRequest> objComparative, int centerID)
    {
        var objColumns = objComparative;

        DataRow drRevenues = _revenuesTotal.NewRow();
        DataRow drCostOfSales = _costOfSalesTotal.NewRow();
        DataRow drExpenses = _expensesTotal.NewRow();
        DataRow drOtherIncome = _otherIncomeTotal.NewRow();
        DataRow drIncomeTaxes = _incomeTaxesTotal.NewRow();
        DataRow drGross = _grossProfit.NewRow();
        DataRow drGrossPercen = _grossProfitPercen.NewRow();
        DataRow drNet = _netProfit.NewRow();
        DataRow drNetPercen = _netProfitPercen.NewRow();
        DataRow drBeforeProvisions = _beforeProvisions.NewRow();
        DataRow drBeforeProvisionsPercen = _beforeProvisionsPercen.NewRow();
        DataRow drNetIncome = _netIncome.NewRow();
        DataRow drNetIncomePercen = _netIncomePercen.NewRow();

        var revenues = dt.Copy().Rows.OfType<DataRow>().Where(x => x["Type"].ToString() == "3");
        var costOfSales = dt.Copy().Rows.OfType<DataRow>().Where(x => x["Type"].ToString() == "4");
        var expenses = dt.Copy().Rows.OfType<DataRow>().Where(x => x["Type"].ToString() == "5");
        var otherIncomes = dt.Copy().Rows.OfType<DataRow>().Where(x => x["Type"].ToString() == "8");
        var incomeTaxes = dt.Copy().Rows.OfType<DataRow>().Where(x => x["Type"].ToString() == "9");

        drRevenues["Department"] = centerID;
        drCostOfSales["Department"] = centerID;
        drExpenses["Department"] = centerID;
        drOtherIncome["Department"] = centerID;
        drIncomeTaxes["Department"] = centerID;
        drGross["Department"] = centerID;
        drGrossPercen["Department"] = centerID;
        drNet["Department"] = centerID;
        drNetPercen["Department"] = centerID;
        drBeforeProvisions["Department"] = centerID;
        drBeforeProvisionsPercen["Department"] = centerID;
        drNetIncome["Department"] = centerID;
        drNetIncomePercen["Department"] = centerID;

        // Calculator summary total
        foreach (var column in objColumns)
        {
            if (column.Type != "Variance")
            {
                var revenuesTotal = revenues.Sum(r => Convert.ToDouble(r["Column" + column.Index.ToString()].ToString()));
                var costOfSalesTotal = costOfSales.Sum(r => Convert.ToDouble(r["Column" + column.Index.ToString()].ToString()));
                var expensesTotal = expenses.Sum(r => Convert.ToDouble(r["Column" + column.Index.ToString()].ToString()));
                var otherIncomeTotal = otherIncomes.Sum(r => Convert.ToDouble(r["Column" + column.Index.ToString()].ToString()));
                var incomeTaxesTotal = incomeTaxes.Sum(r => Convert.ToDouble(r["Column" + column.Index.ToString()].ToString()));

                drRevenues["Column" + column.Index.ToString()] = revenuesTotal;
                drCostOfSales["Column" + column.Index.ToString()] = costOfSalesTotal;
                drExpenses["Column" + column.Index.ToString()] = expensesTotal;
                drOtherIncome["Column" + column.Index.ToString()] = otherIncomeTotal;
                drIncomeTaxes["Column" + column.Index.ToString()] = incomeTaxesTotal;

                drGross["Column" + column.Index.ToString()] = Math.Round(revenuesTotal, 2) - Math.Round(costOfSalesTotal, 2);
                drGrossPercen["Column" + column.Index.ToString()] = revenuesTotal == 0 ? 0 : (revenuesTotal - costOfSalesTotal) / revenuesTotal;

                drNet["Column" + column.Index.ToString()] = Math.Round(revenuesTotal, 2) - Math.Round(costOfSalesTotal, 2) - Math.Round(expensesTotal, 2);
                drNetPercen["Column" + column.Index.ToString()] = revenuesTotal == 0 ? 0 : (revenuesTotal - costOfSalesTotal - expensesTotal) / revenuesTotal;

                drBeforeProvisions["Column" + column.Index.ToString()] = Math.Round(revenuesTotal, 2) - Math.Round(costOfSalesTotal, 2) - Math.Round(expensesTotal, 2) + Math.Round(otherIncomeTotal, 2);
                drBeforeProvisionsPercen["Column" + column.Index.ToString()] = revenuesTotal == 0 ? 0 : (revenuesTotal - costOfSalesTotal - expensesTotal + otherIncomeTotal) / revenuesTotal;

                drNetIncome["Column" + column.Index.ToString()] = Math.Round(revenuesTotal, 2) - Math.Round(costOfSalesTotal, 2) - Math.Round(expensesTotal, 2) + Math.Round(otherIncomeTotal, 2) - Math.Round(incomeTaxesTotal, 2);
                drNetIncomePercen["Column" + column.Index.ToString()] = revenuesTotal == 0 ? 0 : (revenuesTotal - costOfSalesTotal - expensesTotal + otherIncomeTotal - incomeTaxesTotal) / revenuesTotal;
            }
        }

        foreach (var column in objColumns)
        {
            if (column.Type == "Variance")
            {
                var rCol1 = Convert.ToDouble(drRevenues["Column" + column.Column1.ToString()].ToString());
                var rCol2 = Convert.ToDouble(drRevenues["Column" + column.Column2.ToString()].ToString());
                drRevenues["Column" + column.Index.ToString()] = rCol2 == 0 ? 0 : (rCol1 - rCol2) / Math.Abs(rCol2);

                var cCol1 = Convert.ToDouble(drCostOfSales["Column" + column.Column1.ToString()].ToString());
                var cCol2 = Convert.ToDouble(drCostOfSales["Column" + column.Column2.ToString()].ToString());
                drCostOfSales["Column" + column.Index.ToString()] = cCol2 == 0 ? 0 : (cCol1 - cCol2) / Math.Abs(cCol2);

                var eCol1 = Convert.ToDouble(drExpenses["Column" + column.Column1.ToString()].ToString());
                var eCol2 = Convert.ToDouble(drExpenses["Column" + column.Column2.ToString()].ToString());
                drExpenses["Column" + column.Index.ToString()] = eCol2 == 0 ? 0 : (eCol1 - eCol2) / Math.Abs(eCol2);

                var oCol1 = Convert.ToDouble(drOtherIncome["Column" + column.Column1.ToString()].ToString());
                var oCol2 = Convert.ToDouble(drOtherIncome["Column" + column.Column2.ToString()].ToString());
                drOtherIncome["Column" + column.Index.ToString()] = oCol2 == 0 ? 0 : (oCol1 - oCol2) / Math.Abs(oCol2);

                var iCol1 = Convert.ToDouble(drIncomeTaxes["Column" + column.Column1.ToString()].ToString());
                var iCol2 = Convert.ToDouble(drIncomeTaxes["Column" + column.Column2.ToString()].ToString());
                drIncomeTaxes["Column" + column.Index.ToString()] = iCol2 == 0 ? 0 : (iCol1 - iCol2) / Math.Abs(iCol2);

                var gCol1 = Convert.ToDouble(drGross["Column" + column.Column1.ToString()].ToString());
                var gCol2 = Convert.ToDouble(drGross["Column" + column.Column2.ToString()].ToString());
                drGross["Column" + column.Index.ToString()] = gCol2 == 0 ? 0 : (gCol1 - gCol2) / Math.Abs(gCol2);

                var nCol1 = Convert.ToDouble(drNet["Column" + column.Column1.ToString()].ToString());
                var nCol2 = Convert.ToDouble(drNet["Column" + column.Column2.ToString()].ToString());
                drNet["Column" + column.Index.ToString()] = nCol2 == 0 ? 0 : (nCol1 - nCol2) / Math.Abs(nCol2);

                var bCol1 = Convert.ToDouble(drBeforeProvisions["Column" + column.Column1.ToString()].ToString());
                var bCol2 = Convert.ToDouble(drBeforeProvisions["Column" + column.Column2.ToString()].ToString());
                drBeforeProvisions["Column" + column.Index.ToString()] = bCol2 == 0 ? 0 : (bCol1 - bCol2) / Math.Abs(bCol2);

                var neCol1 = Convert.ToDouble(drNetIncome["Column" + column.Column1.ToString()].ToString());
                var neCol2 = Convert.ToDouble(drNetIncome["Column" + column.Column2.ToString()].ToString());
                drNetIncome["Column" + column.Index.ToString()] = neCol2 == 0 ? 0 : (neCol1 - neCol2) / Math.Abs(neCol2);
            }
        }

        _revenuesTotal.Rows.Add(drRevenues);
        _costOfSalesTotal.Rows.Add(drCostOfSales);
        _expensesTotal.Rows.Add(drExpenses);
        _grossProfit.Rows.Add(drGross);
        _grossProfitPercen.Rows.Add(drGrossPercen);
        _netProfit.Rows.Add(drNet);
        _netProfitPercen.Rows.Add(drNetPercen);

        if (otherIncomes.Count() > 0 || incomeTaxes.Count() > 0)
        {
            if (otherIncomes.Count() > 0)
            {
                _otherIncomeTotal.Rows.Add(drOtherIncome);
                _beforeProvisions.Rows.Add(drBeforeProvisions);
                _beforeProvisionsPercen.Rows.Add(drBeforeProvisionsPercen);
            }

            if (incomeTaxes.Count() > 0)
            {
                _incomeTaxesTotal.Rows.Add(drIncomeTaxes);
            }

            _netIncome.Rows.Add(drNetIncome);
            _netIncomePercen.Rows.Add(drNetIncomePercen);
            _includeProvisions = true;
            _includeProvisionsTotal = true;
        }
        else
        {
            _includeProvisions = false;
        }
    }

    private void BindingComparativeReport()
    {
        var dsReport = bL_Report.GetComparativeReport(Session["config"].ToString(), ddlStates.SelectedValue);

        listComparativeReport.DataSource = dsReport;
        listComparativeReport.DataBind();
    }

    private bool ColumnValidation(bool isSave)
    {
        if (string.IsNullOrEmpty(txtReportTitle.Value) && isSave)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningSave", "noty({text: 'Report name is required.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            txtReportTitle.Focus();
            return false;
        }
        else if (gvListColumns.Items.Count == 0)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningSave", "noty({text: 'Please add the report column.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            return false;
        }
        else
        {
            foreach (GridDataItem row in gvListColumns.Items)
            {
                ComparativeStatementRequest obj = new ComparativeStatementRequest();

                DropDownList lblDbType = (DropDownList)row.FindControl("lblDbType");

                if (lblDbType.SelectedValue != "Difference" && lblDbType.SelectedValue != "Variance")
                {
                    DateTime dDate;
                    TextBox lblDbFromDate = (TextBox)row.FindControl("lblDbFromDate");
                    TextBox lblDbToDate = (TextBox)row.FindControl("lblDbToDate");
                    RadComboBox ddlBudget = (RadComboBox)row.FindControl("rcbDbBudget");

                    if (lblDbType.SelectedValue == "Budget" && string.IsNullOrEmpty(ddlBudget.Text))
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningSave", "noty({text: 'Please select budget.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        ddlBudget.Focus();

                        return false;
                    }

                    if (ddlStates.SelectedValue == "ProfitAndLoss")
                    {
                        if (string.IsNullOrEmpty(lblDbFromDate.Text))
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningSave", "noty({text: 'Start date is required.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                            lblDbFromDate.Focus();

                            return false;
                        }
                        else if (!DateTime.TryParse(lblDbFromDate.Text, out dDate))
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningSave", "noty({text: 'The Start date format is invalid.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                            lblDbFromDate.Focus();

                            return false;
                        }
                    }

                    if (string.IsNullOrEmpty(lblDbToDate.Text))
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningSave", "noty({text: 'End date is required.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        lblDbToDate.Focus();

                        return false;
                    }
                    else if (!DateTime.TryParse(lblDbToDate.Text, out dDate))
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningSave", "noty({text: 'The End date format is invalid.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        lblDbToDate.Focus();

                        return false;
                    }
                }
                else
                {
                    DropDownList lblDbColumn1 = (DropDownList)row.FindControl("lblDbColumn1");
                    DropDownList lblDbColumn2 = (DropDownList)row.FindControl("lblDbColumn2");

                    if (string.IsNullOrEmpty(lblDbColumn1.SelectedValue))
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningSave", "noty({text: 'Column is required.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        lblDbColumn1.Focus();

                        return false;
                    }
                    else if (string.IsNullOrEmpty(lblDbColumn2.SelectedValue))
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningSave", "noty({text: 'Column is required.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        lblDbColumn2.Focus();

                        return false;
                    }
                }
            }
        }

        return true;
    }

    private void BindingCenter()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        var centers = bL_Report.GetCenterNames(objPropUser);

        rcCenter.DataSource = centers;
        rcCenter.DataTextField = "CentralName";
        rcCenter.DataValueField = "ID";
        rcCenter.DataBind();

        rcCenter.Items.Insert(0, new RadComboBoxItem("Undefined", "0"));
    }

    private string GetRadComboBoxSelectedItems(RadComboBox radComboBox)
    {
        int itemsChecked = radComboBox.CheckedItems.Count;
        String[] itemsArray = new String[itemsChecked];

        int i = 0;
        var collection = radComboBox.CheckedItems;
        foreach (var item in collection)
        {

            String value = item.Value;
            itemsArray[i] = value;
            i++;
        }

        var items = String.Join(",", itemsArray);

        return items;
    }

    private List<Tuple<int, string>> GetSelectedCenters()
    {
        List<Tuple<int, string>> centers = new List<Tuple<int, string>>();

        if (rcCenter.CheckedItems.Count > 0)
        {
            foreach (var item in rcCenter.CheckedItems)
            {
                centers.Add(Tuple.Create(Convert.ToInt32(item.Value), item.Text));
            }
        }
        else
        {
            foreach (RadComboBoxItem item in rcCenter.Items)
            {
                centers.Add(Tuple.Create(Convert.ToInt32(item.Value), item.Text));
            }
        }

        return centers;
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
