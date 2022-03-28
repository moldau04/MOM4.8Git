using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Web.UI.HtmlControls;
using QBFC12Lib;
using System.Web.Script.Serialization;
using System.Globalization;
using Telerik.Web.UI;
using System.Data.OleDb;
using Microsoft.ApplicationBlocks.Data;
using System.Linq;
using System.IO;

public partial class Budgets : System.Web.UI.Page
{
    BL_Budgets bL_Budgets = new BL_Budgets();
    BL_Report _objBLReport = new BL_Report();

    protected void Page_Init(object sender, EventArgs e)
    {
        //AddMonthColumns();
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

            if (Request.Url.Scheme == "http" && SSL == "1")
            {
                string URL = Request.Url.ToString();
                URL = URL.Replace("http://", "https://");
                Response.Redirect(URL);
            }

            HighlightSideMenu("financialStatement", "lnkBudgets", "financeStateSub");
        }

        if (IsPostBack)
        {
            Session["groupExpandedState"] = null;

            var request = Request["__EVENTTARGET"];
            if (request.Contains("lnkSearch"))
            {
                lnkSearch_Click(sender, e);
            }
            if (request.Contains("lnkClear"))
            {

            }
            if (request.Contains("lnkFilter"))
            {
                lnkFilter_Click(sender, e);
            }
        }
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

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string strQuery = "select [dbo].[Control].[YE] from [dbo].[control]";
            int? yearEnd; int startMonth = 0;

            var ds = SqlHelper.ExecuteDataset(Session["config"].ToString(), CommandType.Text, strQuery);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0] != DBNull.Value)
                {
                    yearEnd = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                    startMonth = int.Parse(yearEnd.ToString()) + 2;

                    if (startMonth > 12)
                    {
                        startMonth = startMonth - 12;
                    }
                }
            }

            var colIndex = startMonth;
            foreach (Telerik.Web.UI.GridColumn col in RadGrid_Budget.MasterTableView.RenderColumns)
            {
                if (col.UniqueName.Equals(setMonth(startMonth, true)))
                {
                    col.OrderIndex = 40;
                    colIndex = 41;
                    break;
                }
            }

            var nextmonth = startMonth + 1;
            for (int j = nextmonth; j <= 12; j++)
            {
                foreach (Telerik.Web.UI.GridColumn col in RadGrid_Budget.MasterTableView.RenderColumns)
                {
                    if (col.UniqueName.Equals(setMonth(j, true)))
                    {
                        col.OrderIndex = colIndex;
                        colIndex++;
                        nextmonth++;
                        break;
                    }
                }
                if (nextmonth > 12)
                {
                    nextmonth = 0;
                    for (int k = nextmonth + 1; k < startMonth; k++)
                    {
                        foreach (Telerik.Web.UI.GridColumn col in RadGrid_Budget.MasterTableView.RenderColumns)
                        {
                            if (col.UniqueName.Equals(setMonth(k, true)))
                            {
                                col.OrderIndex = colIndex;
                                colIndex++;
                                nextmonth++;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    protected void RadGrid_Budget_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (!e.IsFromDetailTable && IsPostBack)
        {
            if (Session["Budgets"] != null)
            {
                RadGrid_Budget.DataSource = (DataTable)Session["Budgets"];
            }
        }
    }

    private void StyleBudgetGrid()
    {
        foreach (GridDataItem item in RadGrid_Budget.Items)
        {
            var lblName = (Label)item.FindControl("lblName");
            if (lblName.Text.Trim().Contains("Net Profit Total"))
            {
                item.BackColor = System.Drawing.Color.LightBlue;
                item.Font.Bold = true;
                item.Font.Size = FontUnit.Parse("12px");
            }
        }

    }

    private void AddMonthColumnsToRadGrid()
    {
        for (int i = 1; i <= 12; i++)
        {
            var txtBoxId = "txtMonth" + i;
            GridTemplateColumn gtc = new GridTemplateColumn();
            gtc.HeaderText = setMonth(i, true);
            gtc.UniqueName = setMonth(i, true);
            gtc.DataField = setMonth(i, true);
            gtc.Visible = true;
            gtc.AllowFiltering = false;
            gtc.ShowFilterIcon = false;
            if (!RadGrid_Budget.Columns.Contains(setMonth(i, true)))
            {
                RadGrid_Budget.Columns.Add(gtc);
            }
        }
    }

    private DataTable BuildBudgetTable()
    {
        DataTable budgetDataTable = new DataTable();
        budgetDataTable.Columns.Add("Type", typeof(Decimal));
        budgetDataTable.Columns.Add("Status");
        budgetDataTable.Columns.Add("Acct");
        budgetDataTable.Columns.Add("fDesc");
        budgetDataTable.Columns.Add("AcctNumber");
        budgetDataTable.Columns.Add("AcctName");
        budgetDataTable.Columns.Add("AnnualTotal");
        budgetDataTable.Columns.Add("TypeName");

        //AddMonthColumns();
        for (int i = 1; i <= 12; i++)
        {
            var columnName = setMonth(i, true);
            budgetDataTable.Columns.Add(columnName);

        }
        budgetDataTable.Columns.Add("Clear");

        return budgetDataTable;
    }

    private void GetBudgetData(bool fillEmpty)
    {
        DataTable budgetDataTable = BuildBudgetTable();
        DataTable dt = new DataTable();
        if (drpBudgets.SelectedIndex <= 0 && (drpBudgetType.SelectedValue.Equals("Actuals") || drpBudgetType.SelectedValue.Equals("Blank Sheet")))
        {
            int year = DateTime.Now.Year;
            if (drpBudgetType.SelectedValue.Equals("Actuals"))
            {
                if (txtYear.Value > 0)
                    year = Convert.ToInt32(txtYear.Value);
            }
            else
            {
                fillEmpty = true;
            }

            if (!fillEmpty)
            {
                Chart _objChart = new Chart();
                if (!string.IsNullOrEmpty(year.ToString()))
                {
                    _objChart.EndDate = Convert.ToDateTime("12/31/" + year);
                }
                else
                {
                    _objChart.EndDate = new DateTime(DateTime.Now.Year, 1, 1).AddSeconds(12).AddDays(-1);
                }

                _objChart.ConnConfig = Session["config"].ToString();
                DataSet ds = bL_Budgets.GetIncomeStatement12PeriodForBudgets(_objChart);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    if (!string.IsNullOrEmpty(txtYearLoad.Value.ToString()))
                    {
                        _objChart.EndDate = Convert.ToDateTime("12/31/" + txtYearLoad.Value.ToString());
                    }
                    else
                    {
                        _objChart.EndDate = Convert.ToDateTime("12/31/" + DateTime.Now.Year);
                        fillEmpty = true;
                    }

                    ds = bL_Budgets.GetIncomeStatement12PeriodForBudgets(_objChart);
                }

                if (!chkInclInactive.Checked)
                {
                    DataRow[] drr = ds.Tables[0].Select("Status = 1");
                    foreach (DataRow row in drr)
                    {
                        row.Delete();
                    }

                    ds.Tables[0].AcceptChanges();
                }

                dt = ProcessAndBuildRawData(budgetDataTable, ds, fillEmpty);

                //Get GL Accounts
                DataSet glAccountsDataSet = bL_Budgets.GetGLAccounts(Session["config"].ToString());
                var dtAccounts = glAccountsDataSet.Tables[0];

                if (!chkInclInactive.Checked)
                {
                    dtAccounts = glAccountsDataSet.Tables[0].Select("Status = 0").CopyToDataTable();
                }

                //Build datable with the GL Accounts Data set
                bool accountExists = false;
                foreach (DataRow drow1 in dtAccounts.Rows)
                {
                    foreach (DataRow drow in dt.Rows)
                    {
                        if (drow["AcctNumber"].ToString().Trim().Contains(drow1["Acct"].ToString().Trim()) && !(drow["fDesc"].ToString().Contains("Summary")))
                        {
                            accountExists = true;
                            break;
                        }
                    }

                    if (!accountExists)
                    {
                        DataRow newRow = dt.NewRow();

                        newRow["fDesc"] = drow1["fDesc"].ToString();
                        newRow["Acct"] = drow1["ID"].ToString();
                        newRow["AcctName"] = drow1["fDesc"].ToString();
                        newRow["Type"] = drow1["Type"].ToString();
                        newRow["Status"] = drow1["Status"].ToString() == "1" ? "InActive" : "Active";
                        newRow["TypeName"] = drow1["TypeName"].ToString();
                        newRow["AcctNumber"] = drow1["Acct"].ToString();
                        newRow["AnnualTotal"] = "0.00";

                        for (int i = 1; i <= 12; i++)
                        {
                            newRow[setMonth(i, true)] = 0.00;
                        }

                        dt.Rows.Add(newRow);
                    }
                    else
                    {
                        accountExists = false;
                    }
                }

                dt.DefaultView.Sort = "Type ASC, fDesc ASC, AcctNumber ASC";
                dt = dt.DefaultView.ToTable();
            }
            else
            {
                var dataSet = bL_Budgets.GetAllActiveAccounts(Session["config"].ToString());
                if (dataSet != null)
                {
                    if (!chkInclInactive.Checked)
                    {
                        DataRow[] drr = dataSet.Tables[0].Select("Status = 'InActive'");
                        foreach (DataRow row in drr)
                        {
                            row.Delete();
                        }

                        dataSet.Tables[0].AcceptChanges();
                    }

                    dt = dataSet.Tables[0];
                    dt = ProcessAndIncludeSummaryRows(dt, true);
                    dt = AddGrossAndNetProfitRow(dt, false);
                }

                Session["totalAccounts"] = dt.Rows.Count - 5;
            }

            dt.DefaultView.Sort = "Type ASC, fDesc ASC, AcctNumber ASC";
            dt = dt.DefaultView.ToTable();

            //Merge the 2 datatables and assign it as DataSource
            Session["Budgets"] = dt;
            RadGrid_Budget.DataSource = dt;
            RadGrid_Budget.MasterTableView.VirtualItemCount = budgetDataTable.Rows.Count;
            RadGrid_Budget.CurrentPageIndex = RadGrid_Budget.MasterTableView.CurrentPageIndex;
        }
        else if (drpBudgetType.SelectedValue.Contains("Saved Budgets"))
        {
            DataSet ds = bL_Budgets.GetBudgetsData(Session["config"].ToString(), drpBudgetsList.SelectedItem?.Text.Trim(), GetSelectedAccountType());
            if (!chkInclInactive.Checked)
            {
                DataRow[] drr = ds.Tables[0].Select("Status = 1");
                foreach (DataRow row in drr)
                {
                    row.Delete();
                }

                ds.Tables[0].AcceptChanges();
            }

            dt = new DataTable();
            if (!drpBudgetType.SelectedValue.Trim().Equals("Blank Sheet"))
            {
                dt = ProcessAndBuildData(budgetDataTable, ds, false);
            }
            else
            {
                dt = ProcessAndBuildData(budgetDataTable, ds, true);
            }

            dt.DefaultView.Sort = "Type ASC, fDesc ASC, AcctNumber ASC";
            dt = dt.DefaultView.ToTable();

            Session["Budgets"] = dt;

            //Merge the 2 datatables and assign it as DataSource
            RadGrid_Budget.DataSource = dt;
            RadGrid_Budget.MasterTableView.VirtualItemCount = budgetDataTable.Rows.Count;
            RadGrid_Budget.CurrentPageIndex = RadGrid_Budget.MasterTableView.CurrentPageIndex;

            var accountCount = Convert.ToInt32(Session["totalAccounts"].ToString());
            if (accountCount > 0)
            {
                Session["totalAccounts"] = accountCount;
                var lblTotalAccounts = (Label)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblTotalAccounts");
                lblTotalAccounts.Text = "Total Accounts : " + accountCount;
                var budgetNamePanel = (HtmlGenericControl)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("budgetHeader");
                budgetNamePanel.Visible = true;
            }
        }
        else if(drpBudgets.SelectedItem != null && !string.IsNullOrEmpty(drpBudgets.SelectedItem.Text))
        {
            DataSet ds = bL_Budgets.GetBudgetsData(Session["config"].ToString(), drpBudgets.SelectedItem.Text.Trim(), GetSelectedAccountType());

            if (!chkInclInactive.Checked)
            {
                DataRow[] drr = ds.Tables[0].Select("Status = 1");
                foreach (DataRow row in drr)
                {
                    row.Delete();
                }

                ds.Tables[0].AcceptChanges();
            }

            dt = ProcessAndBuildData(budgetDataTable, ds, false);
            dt.DefaultView.Sort = "Type ASC, fDesc ASC, AcctNumber ASC";
            dt = dt.DefaultView.ToTable();

            Session["Budgets"] = dt;

            //Merge the 2 datatables and assign it as DataSource
            RadGrid_Budget.DataSource = dt;
            RadGrid_Budget.MasterTableView.VirtualItemCount = budgetDataTable.Rows.Count;
            RadGrid_Budget.CurrentPageIndex = RadGrid_Budget.MasterTableView.CurrentPageIndex;
            RadGrid_Budget.DataBind();
        }

        StyleBudgetGrid();

        if (!IsPostBack)
        {
            txtYear.Value = DateTime.Now.Year - 1;
            txtYearLoad.Value = DateTime.Now.Year;
            DataSet ds = bL_Budgets.GetBudget(Session["config"].ToString(), Convert.ToInt32(txtYearLoad.Value));

            if (ds != null && ds.Tables[0] != null)
            {

                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblSelectBudget.Visible = true;
                    drpBudgets.Visible = true;
                }
                else
                {
                    lblSelectBudget.Visible = false;
                    drpBudgets.Visible = false;
                }

                drpBudgets.DataSource = ds;
                drpBudgets.DataBind();
                drpBudgets.DataTextField = "Budget";
                drpBudgets.DataValueField = "BudgetID";
                drpBudgets.DataBind();
            }
        }
    }

    public DataTable ProcessAndBuildRawData(DataTable budgetTable, DataSet ds, bool fillEmpty)
    {
        DataSet budgetDataSet = new DataSet();
        var valueAdded = false;

        DataTable dt = ds.Tables[0];
        DataView dataView = dt.DefaultView;
        string currentTypeName = "", oldTypeName = "";
        string oldType = "", currentType = "";
        string filterTypeName = string.Empty;

        double annualTotal = 0;
        var filteredDataTable = dataView.ToTable();
        for (int i = 1; i < filteredDataTable.Rows.Count; i++)
        {
            DataRow dr = budgetTable.NewRow();

            var dt1 = budgetTable.Copy();
            var dv1 = dt1.DefaultView;

            //"fDesc = '" + filteredDataTable.Rows[i]["fDesc"].ToString().Trim().Replace("'", "''") + "' AND" +
            dv1.RowFilter = " Acct = '"
                + filteredDataTable.Rows[i]["Acct"].ToString().Trim() + "' AND TypeName = '"
                + filteredDataTable.Rows[i]["TypeName"].ToString().Trim() + "'";
            if (dv1.ToTable().Rows.Count > 0)
            {
                valueAdded = true;

            }

            if (valueAdded)
            {
                valueAdded = false;
                continue;
            }
            else
            {
                if ((budgetTable.Rows.Count > 0 && budgetTable.Rows[budgetTable.Rows.Count - 1]["TypeName"].ToString().Trim() != filteredDataTable.Rows[i]["TypeName"].ToString()))
                {
                    oldTypeName = budgetTable.Rows[budgetTable.Rows.Count - 1]["TypeName"].ToString().Trim();
                    currentTypeName = filteredDataTable.Rows[i]["TypeName"].ToString();
                    oldType = budgetTable.Rows[budgetTable.Rows.Count - 1]["Type"].ToString().Trim();
                    currentType = filteredDataTable.Rows[i]["Type"].ToString();
                }
            }

            dr["fDesc"] = filteredDataTable.Rows[i]["fDesc"];
            var fDesc = filteredDataTable.Rows[i]["fDesc"].ToString().Split(new char[] { ' ' }, 3);
            dr["AcctNumber"] = fDesc[0];
            dr["AcctName"] = fDesc[2];
            dr["Acct"] = filteredDataTable.Rows[i]["Acct"];
            dr["Type"] = filteredDataTable.Rows[i]["Type"];
            dr["Status"] = filteredDataTable.Rows[i]["Status"].ToString() == "1" ? "InActive" : "Active";
            dr["TypeName"] = filteredDataTable.Rows[i]["TypeName"].ToString();

            if (!fillEmpty)
            {
                var descFilteredTable = filteredDataTable.DefaultView;
                descFilteredTable.RowFilter = " Acct = '" + dr["Acct"].ToString().Trim() + "'";
                var dtFTable = descFilteredTable.ToTable();
                if (descFilteredTable.ToTable().Rows.Count > 0)
                {
                    for (int k = 0; k < descFilteredTable.ToTable().Rows.Count; k++)
                    {
                        int n;
                        var isNumeric = int.TryParse(descFilteredTable.ToTable().Rows[k]["NMonth"].ToString().Trim(), out n);
                        if (!isNumeric)
                        {
                            annualTotal += Convert.ToDouble(descFilteredTable.ToTable().Rows[k]["NTotal"].ToString());
                            double month = Convert.ToDouble(descFilteredTable.ToTable().Rows[k]["NTotal"]);
                            if (!descFilteredTable.ToTable().Rows[k]["NMonth"].ToString().Trim().Equals("Total"))
                                dr[descFilteredTable.ToTable().Rows[k]["NMonth"].ToString().Trim().Substring(0, 3)] = month;
                            else
                                dr["AnnualTotal"] = month;
                        }
                        else
                        {
                            var monthName = getMonth(descFilteredTable.ToTable().Rows[k]["NMonth"].ToString().Trim(), false);
                            if (descFilteredTable.ToTable().Rows[k]["fDesc"].ToString().Trim() == dr["fDesc"].ToString().Trim())
                            {
                                annualTotal += Convert.ToDouble(descFilteredTable.ToTable().Rows[k]["NTotal"].ToString());
                                double month = Convert.ToDouble(descFilteredTable.ToTable().Rows[k]["NTotal"]);
                                dr[monthName.ToString().Trim().Substring(0, 3)] = month;
                            }
                        }
                    }
                }
                if (string.IsNullOrEmpty(dr["AnnualTotal"].ToString()))
                    dr["AnnualTotal"] = Math.Round(annualTotal, 2);
                annualTotal = 0;
            }

            budgetTable.Rows.Add(dr);
        }

        budgetTable = ProcessAndIncludeSummaryRows(budgetTable, false);
        budgetTable = AddGrossAndNetProfitRow(budgetTable, false);
        if (budgetTable != null)
        {
            if (!string.IsNullOrEmpty(filterTypeName))
            {
                budgetTable.DefaultView.RowFilter = filterTypeName;
                RadGrid_Budget.AllowCustomPaging = true;
            }
        }

        Session["totalAccounts"] = budgetTable.Rows.Count - 5;

        return budgetTable.DefaultView.ToTable();
    }

    public DataTable ProcessAndBuildData(DataTable budgetTableInput, DataSet ds, bool fillEmpty)
    {
        bool isOldDataModel = false;
        if (ds == null)
        {
            ds = bL_Budgets.GetBudgetData(Session["config"].ToString(), drpBudgetsList.SelectedItem.Text.Trim());

            if (ds != null)
            {
                DataSet budgetDataSet = new DataSet();
                var valueAdded = false;
                DataTable dt = ds.Tables[0];
                DataView dataView = dt.DefaultView;
                string currentTypeName = "", oldTypeName = "";
                string oldType = "", currentType = "";
                string filterTypeName = string.Empty;

                double annualTotal = 0;
                var filteredDataTable = dataView.ToTable();
                for (int i = 1; i < filteredDataTable.Rows.Count; i++)
                {
                    DataRow dr = budgetTableInput.NewRow();

                    var dt1 = budgetTableInput.Copy();
                    var dv1 = dt1.DefaultView;

                    dv1.RowFilter = " Acct = '"
                        + filteredDataTable.Rows[i]["Acct"].ToString().Trim() + "' AND TypeName = '"
                        + filteredDataTable.Rows[i]["TypeName"].ToString().Trim() + "'";
                    if (dv1.ToTable().Rows.Count > 0)
                    {
                        valueAdded = true;
                    }

                    if (valueAdded)
                    {
                        valueAdded = false;
                        continue;
                    }
                    else
                    {
                        if ((budgetTableInput.Rows.Count > 0 && budgetTableInput.Rows[budgetTableInput.Rows.Count - 1]["TypeName"].ToString().Trim() != filteredDataTable.Rows[i]["TypeName"].ToString()))
                        {
                            oldTypeName = budgetTableInput.Rows[budgetTableInput.Rows.Count - 1]["TypeName"].ToString().Trim();
                            currentTypeName = filteredDataTable.Rows[i]["TypeName"].ToString();
                            oldType = budgetTableInput.Rows[budgetTableInput.Rows.Count - 1]["Type"].ToString().Trim();
                            currentType = filteredDataTable.Rows[i]["Type"].ToString();
                        }
                    }

                    dr["fDesc"] = filteredDataTable.Rows[i]["fDesc"];
                    var fDesc = filteredDataTable.Rows[i]["fDesc"].ToString().Split(new char[] { ' ' }, 3);
                    dr["AcctNumber"] = fDesc[0];
                    dr["AcctName"] = fDesc[2];
                    dr["Acct"] = filteredDataTable.Rows[i]["Acct"];
                    dr["Type"] = filteredDataTable.Rows[i]["Type"];
                    dr["Status"] = filteredDataTable.Rows[i]["Status"].ToString() == "1" ? "InActive" : "Active";
                    dr["TypeName"] = filteredDataTable.Rows[i]["TypeName"].ToString();

                    if (!fillEmpty)
                    {
                        var descFilteredTable = filteredDataTable.DefaultView;
                        descFilteredTable.RowFilter = " Acct = '" + dr["Acct"].ToString().Trim() + "'";
                        var dtFTable = descFilteredTable.ToTable();
                        if (descFilteredTable.ToTable().Rows.Count > 0)
                        {
                            for (int k = 0; k < descFilteredTable.ToTable().Rows.Count; k++)
                            {
                                int n;
                                var isNumeric = int.TryParse(descFilteredTable.ToTable().Rows[k]["NMonth"].ToString().Trim(), out n);
                                if (!isNumeric)
                                {
                                    annualTotal += Convert.ToDouble(descFilteredTable.ToTable().Rows[k]["NTotal"].ToString());
                                    double month = Convert.ToDouble(descFilteredTable.ToTable().Rows[k]["NTotal"]);
                                    if (!descFilteredTable.ToTable().Rows[k]["NMonth"].ToString().Trim().Equals("Total"))
                                        dr[descFilteredTable.ToTable().Rows[k]["NMonth"].ToString().Trim().Substring(0, 3)] = month.ToString("N");
                                    else
                                        dr["AnnualTotal"] = month.ToString("N");
                                }
                                else
                                {
                                    var monthName = getMonth(descFilteredTable.ToTable().Rows[k]["NMonth"].ToString().Trim(), false);
                                    if (descFilteredTable.ToTable().Rows[k]["fDesc"].ToString().Trim() == dr["fDesc"].ToString().Trim())
                                    {
                                        annualTotal += Convert.ToDouble(descFilteredTable.ToTable().Rows[k]["NTotal"].ToString());
                                        double month = Convert.ToDouble(descFilteredTable.ToTable().Rows[k]["NTotal"]);
                                        dr[monthName.ToString().Trim().Substring(0, 3)] = month.ToString("N");
                                    }
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(dr["AnnualTotal"].ToString()))
                            dr["AnnualTotal"] = Math.Round(annualTotal, 2).ToString("N");
                        annualTotal = 0;
                    }

                    budgetTableInput.Rows.Add(dr);
                }

                isOldDataModel = true;
            }
        }

        DataTable budgetTable = new DataTable();
        if (!isOldDataModel)
        {
            budgetTable = BuildBudgetTable();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var drNew = budgetTable.NewRow();
                drNew["Type"] = dr["Type"];
                drNew["Status"] = dr["Status"].ToString() == "1" ? "InActive" : "Active";
                drNew["Acct"] = dr["Acct"];
                drNew["fDesc"] = dr["fDesc"];
                drNew["AcctNumber"] = dr["AcctNumber"];
                drNew["AcctName"] = dr["AcctName"];
                drNew["AnnualTotal"] = dr["AnnualTotal"];
                drNew["TypeName"] = dr["TypeName"];

                for (int i = 1; i <= 12; i++)
                {
                    var columnName = setMonth(i, true);
                    drNew[columnName] = dr[columnName];
                }

                budgetTable.Rows.Add(drNew);
            }
        }
        else
        {
            budgetTable = budgetTableInput.Copy();
        }

        budgetTable = ProcessAndIncludeSummaryRows(budgetTable, false);
        budgetTable = AddGrossAndNetProfitRow(budgetTable, false);

        budgetTable.DefaultView.Sort = "Type ASC, fDesc ASC, AcctNumber ASC";
        budgetTable = budgetTable.DefaultView.ToTable();
        Session["totalAccounts"] = budgetTable.Rows.Count - 5;

        return budgetTable.DefaultView.ToTable();
    }

    private DataTable ProcessAndIncludeSummaryRows(DataTable budgetTable, bool fillEmpty)
    {
        DataView dview = new DataView();
        DataSet resultSet = new DataSet();
        DataTable fDt = new DataTable();
        DataTable resultTable = new DataTable();
        DataTable finalTable = budgetTable.Clone();
        if (fillEmpty)
            finalTable = budgetTable.Copy();

        if (budgetTable != null)
        {
            for (int i = 3; i <= 5; i++)
            {
                var typeName = string.Empty;
                var type = 0.00;
                double[] summaryTotal = new double[13];
                resultTable = budgetTable.Copy();
                DataView dv = resultTable.DefaultView;
                dv.RowFilter = "Type = '" + i.ToString() + "'";
                fDt = dv.ToTable();

                if (!fillEmpty)
                {
                    foreach (DataRow dRow in fDt.Rows)
                    {
                        if (dRow["AnnualTotal"] != System.DBNull.Value)
                        {
                            summaryTotal[0] += Convert.ToDouble(dRow["AnnualTotal"]);
                            for (int j = 1; j <= 12; j++)
                            {
                                if (dRow[setMonth(j, true)] != System.DBNull.Value)
                                    summaryTotal[j] += Convert.ToDouble(dRow[setMonth(j, true)]);
                            }
                        }

                        typeName = dRow["TypeName"].ToString();
                        type = Convert.ToDouble(dRow["Type"].ToString());
                        finalTable.Rows.Add(dRow.ItemArray);
                    }
                }
                else
                {
                    typeName = fDt.Rows[0]["TypeName"].ToString();
                    type = Convert.ToDouble(fDt.Rows[0]["Type"].ToString());
                    summaryTotal[0] += 0.00;
                    for (int j = 1; j <= 12; j++)
                    {
                        summaryTotal[j] += 0.00;
                    }
                }

                if (!CheckIfSummaryRowExists(budgetTable, typeName))
                {
                    DataRow sumRow = finalTable.NewRow();
                    sumRow["AnnualTotal"] = summaryTotal[0].ToString("N");
                    sumRow["fDesc"] = "zzzSummary";

                    try
                    {
                        sumRow["Acct"] = typeName;
                    }
                    catch
                    {
                        sumRow["Acct"] = type;
                    }

                    sumRow["TypeName"] = typeName.Trim();
                    sumRow["Type"] = i;
                    sumRow["AcctName"] = "Total " + typeName;

                    for (int j = 1; j <= 12; j++)
                    {
                        sumRow[setMonth(j, true)] = summaryTotal[j].ToString("N");
                    }

                    finalTable.Rows.Add(sumRow);
                }
            }

            dview = finalTable.DefaultView;
        }

        resultSet.Tables.Add(dview.ToTable());

        return finalTable;
    }

    private DataTable AddGrossAndNetProfitRow(DataTable budgetTable, bool fillEmpty)
    {
        var budgetTableCopy = new DataTable();
        budgetTableCopy = budgetTable.Copy();
        double[] revenuesTotal = new double[13];
        double[] costOfSalesTotal = new double[13];
        double[] expensesTotal = new double[13];

        for (int i = 0; i < 13; i++)
        {
            revenuesTotal[i] = 0.00;
            costOfSalesTotal[i] = 0.00;
            expensesTotal[i] = 0.00;
        }

        if (!fillEmpty)
        {
            foreach (DataRow dRow in budgetTableCopy.Rows)
            {
                if (dRow["AcctName"].ToString().Trim().Contains("Total Revenues"))
                {
                    revenuesTotal[0] = Convert.ToDouble(dRow["AnnualTotal"].ToString());
                    for (int j = 1; j <= 12; j++)
                    {
                        if (dRow[setMonth(j, true)] != System.DBNull.Value)
                            revenuesTotal[j] = Convert.ToDouble(dRow[setMonth(j, true)]);
                    }

                }
                if (dRow["AcctName"].ToString().Trim().Contains("Total Cost of Sales"))
                {
                    costOfSalesTotal[0] = Convert.ToDouble(dRow["AnnualTotal"].ToString());
                    for (int j = 1; j <= 12; j++)
                    {
                        if (dRow[setMonth(j, true)] != System.DBNull.Value)
                            costOfSalesTotal[j] = Convert.ToDouble(dRow[setMonth(j, true)]);
                    }

                }
                if (dRow["AcctName"].ToString().Trim().Contains("Total Expenses"))
                {
                    expensesTotal[0] = Convert.ToDouble(dRow["AnnualTotal"].ToString());
                    for (int j = 1; j <= 12; j++)
                    {
                        if (dRow[setMonth(j, true)] != System.DBNull.Value)
                            expensesTotal[j] = Convert.ToDouble(dRow[setMonth(j, true)]);
                    }

                }

            }
        }

        if (!CheckIfSummaryRowExists(budgetTable, "Net Profit Total"))
        {
            DataRow netProfitRow = budgetTable.NewRow();
            netProfitRow["fDesc"] = "zzzz Net Profit Total";
            netProfitRow["Acct"] = 0;
            netProfitRow["TypeName"] = "Expenses";
            netProfitRow["Type"] = "5";
            netProfitRow["AcctName"] = "Net Profit Total";

            double netProfitTotals = Convert.ToDouble(revenuesTotal[0]) - Convert.ToDouble(costOfSalesTotal[0]) - Convert.ToDouble(expensesTotal[0]);
            netProfitRow["AnnualTotal"] = netProfitTotals.ToString("N");

            for (int j = 1; j <= 12; j++)
            {
                netProfitRow[setMonth(j, true)] = (Convert.ToDouble(revenuesTotal[j]) - Convert.ToDouble(costOfSalesTotal[j]) - Convert.ToDouble(expensesTotal[j])).ToString("N");
            }

            budgetTable.Rows.Add(netProfitRow);
        }

        if (!CheckIfSummaryRowExists(budgetTable, "Gross Profit"))
        {
            DataRow grossProfitRow = budgetTable.NewRow();
            grossProfitRow["fDesc"] = "zzzz Gross Profit";
            grossProfitRow["Acct"] = 0;
            grossProfitRow["TypeName"] = "Cost of Sales";
            grossProfitRow["Type"] = "4";
            grossProfitRow["AcctName"] = "Total Gross Profit";

            double netProfitTotals = Convert.ToDouble(revenuesTotal[0]) - Convert.ToDouble(costOfSalesTotal[0]);
            grossProfitRow["AnnualTotal"] = netProfitTotals.ToString("N");

            for (int j = 1; j <= 12; j++)
            {
                grossProfitRow[setMonth(j, true)] = Convert.ToDouble(revenuesTotal[j]) - Convert.ToDouble(costOfSalesTotal[j]);
            }

            budgetTable.Rows.Add(grossProfitRow);
        }

        return budgetTable;
    }

    private void ClearBlankColumns()
    {
        for (int i = 0; i < RadGrid_Budget.MasterTableView.Columns.Count; i++)
        {
            if (RadGrid_Budget.Columns[i].HeaderText.Equals(string.Empty))
                RadGrid_Budget.Columns.RemoveAt(i);
        }
    }

    private void AddMonthColumns()
    {

        for (int i = 1; i <= 12; i++)
        {
            var columnName = setMonth(i, true);

            GridTemplateColumn column = new GridTemplateColumn();
            column.HeaderText = columnName + txtYear.Value;
            column.UniqueName = columnName;
            GridColumn col = RadGrid_Budget.MasterTableView.Columns.FindByUniqueNameSafe(columnName);
            if (col != null)
                RadGrid_Budget.Columns.Remove(col);

            RadGrid_Budget.MasterTableView.Columns.Add(column);

        }
    }

    public string setMonth(int i, bool IsAbbreviation)
    {
        string month = string.Empty;
        switch (i)
        {
            case 1:
                if (IsAbbreviation)
                    month = "Jan";
                else
                    month = "January";
                break;
            case 2:
                if (IsAbbreviation)
                    month = "Feb";
                else
                    month = "February";
                break;
            case 3:
                if (IsAbbreviation)
                    month = "Mar";
                else
                    month = "March";
                break;
            case 4:
                if (IsAbbreviation)
                    month = "Apr";
                else
                    month = "April";
                break;
            case 5:
                month = "May";
                break;
            case 6:
                if (IsAbbreviation)
                    month = "Jun";
                else
                    month = "June";
                break;
            case 7:
                if (IsAbbreviation)
                    month = "Jul";
                else
                    month = "July";
                break;
            case 8:
                if (IsAbbreviation)
                    month = "Aug";
                else
                    month = "August";
                break;
            case 9:
                if (IsAbbreviation)
                    month = "Sep";
                else
                    month = "September";
                break;
            case 10:
                if (IsAbbreviation)
                    month = "Oct";
                else
                    month = "October";
                break;
            case 11:
                if (IsAbbreviation)
                    month = "Nov";
                else
                    month = "November";
                break;
            case 12:
                if (IsAbbreviation)
                    month = "Dec";
                else
                    month = "December";
                break;
        }
        return month;
    }

    public string getMonth(string i, bool IsAbbreviation)
    {
        string month;
        if (i.Length > 2)
            month = i.Substring(4, 2);
        else
            month = i;
        switch (month)
        {
            case "01":
            case "1":
                if (IsAbbreviation)
                    month = "Jan";
                else
                    month = "January";
                break;
            case "02":
            case "2":
                if (IsAbbreviation)
                    month = "Feb";
                else
                    month = "February";
                break;
            case "03":
            case "3":
                if (IsAbbreviation)
                    month = "Mar";
                else
                    month = "March";
                break;
            case "04":
            case "4":
                if (IsAbbreviation)
                    month = "Apr";
                else
                    month = "April";
                break;
            case "05":
            case "5":
                month = "May";
                break;
            case "06":
            case "6":
                if (IsAbbreviation)
                    month = "Jun";
                else
                    month = "June";
                break;
            case "07":
            case "7":
                if (IsAbbreviation)
                    month = "Jul";
                else
                    month = "July";
                break;
            case "08":
            case "8":
                if (IsAbbreviation)
                    month = "Aug";
                else
                    month = "August";
                break;
            case "09":
            case "9":
                if (IsAbbreviation)
                    month = "Sep";
                else
                    month = "September";
                break;
            case "10":
                if (IsAbbreviation)
                    month = "Oct";
                else
                    month = "October";
                break;
            case "11":
                if (IsAbbreviation)
                    month = "Nov";
                else
                    month = "November";
                break;
            case "12":
                if (IsAbbreviation)
                    month = "Dec";
                else
                    month = "December";
                break;
        }
        return month;
    }

    protected void RadGrid_Budget_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridFilteringItem)
        {
            GridFilteringItem filteringItem = e.Item as GridFilteringItem;
            //set dimensions for the filter textbox 
            TextBox box = filteringItem["AcctNumber"].Controls[0] as TextBox;
            box.Width = Unit.Pixel(75);
        }
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            TextBox tBox = (TextBox)item.FindControl("txtAnnualTotal");
            Label lblName = (Label)item.FindControl("lblName");

            HiddenField hdn = (HiddenField)item.FindControl("hdnType");
            tBox.Attributes.Add("onchange", "txtAnnualTotalValueChange('" + tBox.ClientID + "', '" + item.ItemIndex + "', '" + lblName.ClientID + "', '" + hdn.ClientID + "'" + ")");
            for (int i = 1; i <= 12; i++)
            {
                tBox = (TextBox)item.FindControl("txtMonth" + i);
                tBox.Attributes.Add("onchange", "txtMonthValueChange('" + tBox.ClientID + "', '" + item.ItemIndex + "', '" + lblName.ClientID + "', '" + hdn.ClientID + "'" + ")");
            }
        }

    }

    private DataTable ProcessAndBuildChildAccounts(DataSet ds, DataTable dtable, string accountNumber)
    {

        var dt = ds.Tables[0];
        var dt1 = dtable.Copy();
        for (int i = 1; i < dt.Rows.Count; i++)
        {
            DataRow dr = dt1.NewRow();

            dr["fDesc"] = dt.Rows[i]["fDesc"];
            var fDesc = dt.Rows[i]["fDesc"].ToString().Split(new char[] { ' ' }, 3);
            dr["AcctNumber"] = fDesc[0];
            dr["AcctName"] = fDesc[2];
            dr["Acct"] = dt.Rows[i]["Acct"];
            dr["Status"] = dt.Rows[i]["Status"];
            dr["Type"] = dt.Rows[i]["Type"];
            dr["TypeName"] = dt.Rows[i]["TypeName"].ToString();
            dr["ParentAccNumber"] = accountNumber;

            for (int j = 1; j <= 12; j++)
            {
                var columnName = setMonth(j, true);
                dr[columnName] = 0.00;

            }


            dr["AnnualTotal"] = "0.00";

            dt1.Rows.Add(dr);
        }
        return dt1;
    }

    protected void RadGrid_Budget_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            var lblName = (Label)item.FindControl("lblName");
            var hdnStatus = (HiddenField)item.FindControl("hdnStatus"); TableCell celltoVerify1 = item["Status"];
            HiddenField hdn = (HiddenField)item.FindControl("hdnType");

            if (lblName.Text.Contains("Total"))
            {
                item.Font.Bold = true;
                item.Font.Size = FontUnit.Parse("12px");
                var chk = (CheckBox)item.FindControl("cboxSelect");
                chk.Enabled = false;
                chk.Visible = false;
                item.SetChildrenVisible(false);
                TableCell cell = item["ExpandColumn"];
                cell.Controls[0].Visible = false;
                cell.Text = " ";
            }
        }

        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            var lblName = (Label)item.FindControl("lblName");
            var hdnStatus = (HiddenField)item.FindControl("hdnStatus");
            TableCell celltoVerify1 = item["Status"];
            if (celltoVerify1.Text == "1" && !(lblName.Text.Contains("Total")))
            {
                celltoVerify1.ForeColor = System.Drawing.Color.Red;
                celltoVerify1.BackColor = System.Drawing.Color.Red;
            }
        }

        if (e.Item is GridHeaderItem)
        {
            GridItem dataItem = (GridHeaderItem)e.Item;
            dataItem.SetChildrenVisible(false);
        }

        if (e.Item is GridGroupHeaderItem)
        {
            GridGroupHeaderItem GroupHeader = (GridGroupHeaderItem)e.Item;
            DataRowView groupDataRow = (DataRowView)e.Item.DataItem;
            if (!groupDataRow["TypeName"].ToString().Contains("Summary") && !groupDataRow["TypeName"].ToString().Contains("NET PROFIT TOTAL"))
                GroupHeader.DataCell.Text = groupDataRow["TypeName"].ToString();
            else
            {
                GroupHeader.DataCell.Text = string.Empty;
            }
            string strText = GroupHeader.DataCell.Text;
            if (strText.Trim() == "")
            {
                GroupHeader.Attributes.Add("style", "display:none");
            }
            else
            {
                GroupHeader.Attributes.Add("onDblClick ", "return Expand('" + strText + "');");
            }
        }
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        DataTable budgetDataTable = BuildBudgetTable();
        if (drpBudgetType.SelectedValue.Equals("Actuals"))
        {
            GetBudgetData(false);
            RadGrid_Budget.DataBind();
            var lblBudgetName = (Label)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblBudgetName");
            lblBudgetName.Text = string.Empty;
            var lblBudgetYear = (Label)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblBudgetYear");
            lblBudgetYear.Text = string.Empty;
            var budgetNamePanel = (HtmlGenericControl)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("budgetHeader");
            budgetNamePanel.Visible = false;
            lnkDeleteBudget.Visible = false;
            lnkRefresh.Visible = false;
            lblMessage.Text = "Budget loaded from Actual data of Year " + txtYear.Text;
            lblMessage.Visible = true;
            lblMessage.ForeColor = System.Drawing.Color.Black;
        }
        else if (drpBudgetType.SelectedValue.Equals("Saved Budgets"))
        {
            budgetDataTable = BuildBudgetTable();
            DataSet ds = bL_Budgets.GetBudgetsData(Session["config"].ToString(), drpBudgetsList.SelectedItem.Text.Trim(), GetSelectedAccountType());

            if (!chkInclInactive.Checked)
            {
                DataRow[] drr = ds.Tables[0].Select("Status = 1");
                foreach (DataRow row in drr)
                {
                    row.Delete();
                }

                ds.Tables[0].AcceptChanges();
            }

            Session["Budgets"] = ProcessAndBuildData(budgetDataTable, ds, false);

            RadGrid_Budget.DataSource = (DataTable)Session["Budgets"];
            RadGrid_Budget.MasterTableView.VirtualItemCount = budgetDataTable.Rows.Count;
            RadGrid_Budget.CurrentPageIndex = RadGrid_Budget.MasterTableView.CurrentPageIndex;
            RadGrid_Budget.DataBind();

            var lblBudgetName = (Label)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblBudgetName");
            lblBudgetName.Text = string.Empty;
            var lblBudgetYear = (Label)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblBudgetYear");
            lblBudgetYear.Text = string.Empty;
            var budgetNamePanel = (HtmlGenericControl)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("budgetHeader");
            budgetNamePanel.Visible = true;
            lnkDeleteBudget.Visible = true;
            lnkRefresh.Visible = true;
            lblMessage.Text = "Budget loaded from Saved Budget " + drpBudgetsList.SelectedItem.Text;
            lblMessage.Visible = true;
            lblMessage.ForeColor = System.Drawing.Color.Black;
        }
        else if (drpBudgetType.SelectedValue.Equals("Blank Sheet"))
        {
            lnkDeleteBudget.Visible = false;
            lnkRefresh.Visible = false;
            lblMessage.Visible = false;
            GetBudgetData(true);
            RadGrid_Budget.DataBind();
        }

        var accountCount = 0;

        if (Session["totalAccounts"] != null)
        {
            accountCount = Convert.ToInt32(Session["totalAccounts"].ToString());
        }

        if (accountCount > 0)
        {
            try
            {
                var lblTotalAccounts = (Label)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblTotalAccounts");
                lblTotalAccounts.Text = "Total Accounts : " + accountCount;
                var budgetNamePanel = (HtmlGenericControl)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("budgetHeader");
                budgetNamePanel.Visible = true;
                Session["totalAccounts"] = accountCount;
            }
            catch (Exception ex)
            { }
        }

        lnkSaveBudget.Text = "Save Budget";
        budgetSavePanel.Visible = true;
        lnkDeleteBudget.Visible = false;
        lnkRefresh.Visible = false;

        if (drpBudgets.Items.Count > 0)
        {
            drpBudgets.SelectedIndex = 0;
            drpBudgets.Visible = false;
        }
        StyleBudgetGrid();
        Hidden1.Value = string.Empty;

        if (RadGrid_Budget.Items.Count > 1)
        {
            var lastItem = RadGrid_Budget.Items[RadGrid_Budget.Items.Count - 1];
            var lbl = (Label)lastItem.FindControl("lblName");
            if (lbl.Text.Contains("Total"))
                lastItem.BackColor = System.Drawing.Color.LightBlue;
        }
    }

    protected void RadGrid_Budget_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("ClearRow"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            GridDataItem dataItem = e.Item as GridDataItem;
            var txtAnnual = (TextBox)dataItem["AnnualTotal"].FindControl("txtAnnualTotal");
            if (txtAnnual != null)
                txtAnnual.Text = string.Empty;
            for (int j = 1; j <= 12; j++)
            {
                //var txtHead = (TextBox)item.FindControl(setMonth(j, true) + txtYear.Value);
                var txt = (TextBox)item.FindControl("txtMonth" + j);
                txt.Text = string.Empty;
                txt.BorderStyle = BorderStyle.None;
                //txtHead.Text = setMonth(j, true).ToString();
            }
        }
        if (e.CommandName.Equals("Clear"))
        {
            for (int i = 0; i < RadGrid_Budget.Items.Count; i++)
            {
                GridDataItem item = RadGrid_Budget.Items[i];
                if (item.ItemType == GridItemType.Item || item.ItemType == GridItemType.AlternatingItem)
                {
                    for (int j = 1; j <= 12; j++)
                    {
                        var txt = (TextBox)item.FindControl("txtMonth" + j);
                        txt.Text = string.Empty;
                        txt.BorderStyle = BorderStyle.None;
                    }
                }
            }
            ClearBlankColumns();
            //AddMonthColumns();
            GetBudgetData(true);
            RadGrid_Budget.Rebind();
        }
        if (e.CommandName == "ExpandCollapse")
        {
            //ClearBlankColumns();
            //AddMonthColumns();

            //GetBudgetData(false);
            //RadGrid_Budget.DataBind();
            GridDataItem item = (GridDataItem)e.Item;
            GridDataItem dataItem = e.Item as GridDataItem;
            var currentState = e.Item.Expanded;
            e.Item.Expanded = true;
            //         RadGrid_Budget.MasterTableView.Items[dataItem.ItemIndex].Expanded = true;
            //         RadGrid_Budget.MasterTableView.Items[0].ChildItem.NestedTableViews[0].Items[0].Expanded = true;
            //         RadGrid_Budget.MasterTableView.Items[dataItem.ItemIndex].Expanded = true;
            RadGrid_Budget.Rebind();
            RadGrid_Budget.MasterTableView.Items[dataItem.ItemIndex].Expanded = !currentState;
            //         dataItem.ChildItem.NestedTableViews[0].Items[0].Expanded = true;
            //dataItem.ChildItem.Expanded = true;
            //GridTableView detailtable = (GridTableView)dataItem.ChildItem.NestedTableViews[0];
            //detailtable.Visible = true;

            //foreach (GridItem item in e.Item.OwnerTableView.Items)
            //{
            //    if (item.Expanded && item != e.Item)
            //    {
            //        item.Expanded = false;
            //    }
            //}
        }
        if (e.CommandName == RadGrid.ExportToExcelCommandName)
        {
            RadGrid_Budget.ExportSettings.FileName = drpBudgetsList.SelectedItem.Text.Trim();
        }
        if (e.CommandName.Equals("AddChildAccount"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            GridTableView detailtable = (GridTableView)item.ChildItem.NestedTableViews[0];
            //add new item to detail table
        }

    }

    /// <summary>
    /// Saves the state of the groupping on screen
    /// </summary>
    ///The RadGrid that needs to preserve it’s grouping state
    private void SaveGroupsExpandedState(RadGrid grid)
    {
        GridItem[] groupItems = grid.MasterTableView.GetItems(GridItemType.GroupHeader);
        if (groupItems.Length > 0)
        {
            var listCollapsedIndexes = new List<int>();
            foreach (GridItem item in groupItems)
            {
                if (!item.Expanded)
                {
                    listCollapsedIndexes.Add(item.RowIndex);
                }
            }
            Session["groupExpandedState"] = listCollapsedIndexes;
        }
    }

    /// <summary>
    /// Restores the state of the groupping on screen
    /// </summary>
    ///The RadGrid that needs to be restored to its grouping state
    private void LoadGroupsExpandedState(RadGrid grid)
    {
        var listCollapsedIndexes = Session["groupExpandedState"] as List<int>;
        if (listCollapsedIndexes != null)
        {
            foreach (GridItem item in grid.MasterTableView.GetItems(GridItemType.GroupHeader))
            {
                if (listCollapsedIndexes.Contains(item.RowIndex))
                {
                    item.Expanded = false;

                }
            }
        }
    }

    protected void RadGrid_Budget_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        //RadGrid_Budget.CurrentPageIndex = e.NewPageIndex;
        //GetBudgetData(false);
        //RadGrid_Budget.DataBind();
        //RadGrid_Budget.Rebind();
    }

    protected void RadGrid_Budget_DataBinding(object sender, EventArgs e)
    {
        //SaveGroupsExpandedState(this.RadGrid_Budget);
    }

    protected void lnkFilter_Click(object sender, EventArgs e)
    {
        //AddMonthColumns();
        GetBudgetData(false);
        RadGrid_Budget.CurrentPageIndex = 0;
        RadGrid_Budget.DataBind();
    }

    protected void chkInclInactive_Click(object sender, EventArgs e)
    {
        GetBudgetData(false);
        RadGrid_Budget.CurrentPageIndex = 0;
        RadGrid_Budget.DataBind();
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        if (txtYearLoad.Value != null)
        {
            var lblBudgetName = (Label)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblBudgetName");
            var lblBudgetYear = (Label)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblBudgetYear");
            if (string.IsNullOrEmpty(lblBudgetName.Text) || lblBudgetName.Text.Contains("Select Budget"))
            {
                Budget objBudget = new Budget();
                if (!string.IsNullOrEmpty(txtBudgetName.Text))
                {
                    objBudget.BudgetValue = txtBudgetName.Text;
                }
                else
                {
                    objBudget.BudgetValue = drpBudgets.SelectedItem.Text.Trim();
                }

                objBudget.Year = Convert.ToInt32(txtYearLoad.Value);

                int budgetId = Convert.ToInt32(bL_Budgets.GetBudgetID(Session["config"].ToString(), objBudget.BudgetValue));
                if (budgetId > 0)
                {
                    //Budget Name already exists
                    lblMessage.Text = "Budget " + txtBudgetName.Text.Trim() + " already exists";
                    return;
                }

                // Bet end year month
                string strQuery = "select [dbo].[Control].[YE] from [dbo].[control]";
                int? yearEnd; int startMonth = 0;

                var ds1 = SqlHelper.ExecuteDataset(Session["config"].ToString(), CommandType.Text, strQuery);
                if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
                {
                    yearEnd = int.Parse(ds1.Tables[0].Rows[0][0].ToString());
                    startMonth = int.Parse(yearEnd.ToString()) + 2;

                    if (startMonth > 12)
                    {
                        startMonth = startMonth - 12;
                    }
                }

                objBudget.ConnConfig = Session["config"].ToString();
                budgetId = bL_Budgets.AddBudget(objBudget);

                foreach (GridItem item in RadGrid_Budget.MasterTableView.Items)
                {
                    GridDataItem dataItem = (GridDataItem)item;
                    if (item.ItemType == GridItemType.Item || item.ItemType == GridItemType.AlternatingItem)
                    {
                        Account objAccount = new Account();
                        int accountId = 0;
                        var lblAcct = (Label)dataItem.FindControl("lblId");
                        var lblName = (Label)dataItem.FindControl("lblName");
                        var hdnType = (HiddenField)dataItem.FindControl("hdnType");
                        objAccount.Acct = lblAcct.Text;
                        objAccount.fDesc = lblName.Text;
                        objAccount.Type = hdnType.Value;
                        objAccount.AccRoot = objAccount.Balance = objAccount.Branch = objAccount.CAlias = objAccount.Control = objAccount.CostCenter = "NULL";
                        objAccount.DAT = objAccount.Detail = objAccount.InUse = objAccount.Remarks = objAccount.Status = objAccount.Sub = objAccount.Sub2 = "NULL";
                        objAccount.ConnConfig = Session["config"].ToString();
                        if (!objAccount.fDesc.Trim().ToUpper().Contains("TOTAL"))
                        {
                            accountId = bL_Budgets.AddAccount(objAccount);
                        }

                        if (!string.IsNullOrEmpty(lblName.Text) && accountId != 0)
                        {
                            AccountDetail objAccountDetail = new AccountDetail();
                            objAccountDetail.AccountId = accountId;
                            objAccountDetail.BudgetId = budgetId;

                            var currentMonth = startMonth;

                            for (int i = 1; i <= 12; i++)
                            {
                                var txt = (TextBox)item.FindControl("txtMonth" + i);
                                if (txt != null)
                                {
                                    if (!string.IsNullOrEmpty(txt.Text))
                                    {
                                        objAccountDetail = SetValues(i.ToString(), txt.Text, objAccountDetail);
                                    }
                                    else
                                    {
                                        Convert.ToDouble("0.00");
                                    }
                                }

                                var budgetYear = txtYearLoad.Value;
                                if (startMonth != 1)
                                {
                                    if (currentMonth >= startMonth && currentMonth <= 12)
                                    {
                                        budgetYear = budgetYear - 1;
                                    }
                                    else
                                    {
                                        budgetYear = txtYearLoad.Value;
                                    }
                                }

                                objAccountDetail.Period = Convert.ToInt32(budgetYear) * 100 + currentMonth;

                                if (txt != null)
                                {
                                    if (!string.IsNullOrEmpty(txt.Text))
                                    {
                                        objAccountDetail.Credit = objAccountDetail.Debit = objAccountDetail.Amount = Convert.ToDouble(txt.Text);
                                    }
                                    else
                                    {
                                        objAccountDetail.Credit = objAccountDetail.Debit = objAccountDetail.Amount = Convert.ToDouble("0.00");
                                    }
                                }

                                objAccountDetail.ConnConfig = Session["config"].ToString();
                                bL_Budgets.AddAccountDetails(objAccountDetail);

                                if (currentMonth < 12)
                                {
                                    currentMonth++;
                                }
                                else if (currentMonth == 12)
                                {
                                    currentMonth = 1;
                                }
                            }

                            var txtTotal = (TextBox)item.FindControl("txtAnnualTotal");
                            if (txtTotal != null)
                            {
                                if (!string.IsNullOrEmpty(txtTotal.Text))
                                {
                                    objAccountDetail.Total = Convert.ToDouble(txtTotal.Text);
                                }
                                else
                                {
                                    objAccountDetail.Total = Convert.ToDouble("0.00");
                                }
                            }

                            objAccountDetail.ConnConfig = Session["config"].ToString();
                            bL_Budgets.AddBudgetAccountDetails(objAccountDetail);
                        }
                    }
                }

                lblBudgetName.Text = txtBudgetName.Text;
                lblBudgetName.Visible = true;
                lblBudgetYear.Text = " - " + txtYearLoad.Text;
                lblBudgetYear.Visible = true;
                txtBudgetName.Text = string.Empty;
                var budgetNamePanel = (HtmlGenericControl)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("budgetHeader");
                budgetNamePanel.Visible = true;
                lnkDeleteBudget.Visible = true;
                lnkRefresh.Visible = true;
                lblMessage.Text = lblBudgetName.Text + " for " + txtYearLoad.Value + " saved successfully";
                lnkSaveBudget.Text = "Update";
                var lblTotalAccounts = (Label)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblTotalAccounts");
                lblTotalAccounts.Text = "Total Accounts : " + Session["totalAccounts"];

                DataSet ds = bL_Budgets.GetBudget(Session["config"].ToString(), Convert.ToInt32(txtYearLoad.Value));
                drpBudgets.Items.Clear();
                drpBudgets.DataSource = ds;
                drpBudgets.DataBind();
                drpBudgets.DataTextField = "Budget";
                drpBudgets.DataValueField = "BudgetID";
                drpBudgets.DataBind();
                drpBudgets.Items.FindItemByText(lblBudgetName.Text.Trim()).Selected = true;
                drpBudgets.Visible = true;
                drpBudgets_SelectedIndexChanged(sender, e);
                GetBudgetData(false);
            }
            else
            {
                cmdUpdateBudget_Click(sender, e);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningSave", "noty({text: 'Please specify a year to save the budget!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            txtYearLoad.Focus();
        }
    }

    private AccountDetail SetValues(string month, string value, AccountDetail accountDetail)
    {
        switch (month)
        {
            case "1":
                accountDetail.Jan = double.Parse(value);
                break;
            case "2":
                accountDetail.Feb = double.Parse(value);
                break;
            case "3":
                accountDetail.Mar = double.Parse(value);
                break;
            case "4":
                accountDetail.Apr = double.Parse(value);
                break;
            case "5":
                accountDetail.May = double.Parse(value);
                break;
            case "6":
                accountDetail.Jun = double.Parse(value);
                break;
            case "7":
                accountDetail.Jul = double.Parse(value);
                break;
            case "8":
                accountDetail.Aug = double.Parse(value);
                break;
            case "9":
                accountDetail.Sep = double.Parse(value);
                break;
            case "10":
                accountDetail.Oct = double.Parse(value);
                break;
            case "11":
                accountDetail.Nov = double.Parse(value);
                break;
            case "12":
                accountDetail.Dec = double.Parse(value);
                break;
        }
        return accountDetail;
    }

    protected void drpBudgets_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable budgetDataTable = BuildBudgetTable();
        DataSet ds = bL_Budgets.GetBudgetsData(Session["config"].ToString(), drpBudgets.SelectedItem.Text.Trim(), GetSelectedAccountType());

        if (!chkInclInactive.Checked)
        {
            DataRow[] drr = ds.Tables[0].Select("Status = 1");
            foreach (DataRow row in drr)
            {
                row.Delete();
            }

            ds.Tables[0].AcceptChanges();
        }

        var dt = ProcessAndBuildData(budgetDataTable, ds, false);
        dt.DefaultView.Sort = "Type ASC, fDesc ASC, AcctNumber ASC";
        dt = dt.DefaultView.ToTable();

        Session["Budgets"] = dt;

        //Merge the 2 datatables and assign it as DataSource
        RadGrid_Budget.DataSource = dt;
        RadGrid_Budget.MasterTableView.VirtualItemCount = budgetDataTable.Rows.Count;
        RadGrid_Budget.CurrentPageIndex = RadGrid_Budget.MasterTableView.CurrentPageIndex;
        RadGrid_Budget.DataBind();

        var lblBudgetName = (Label)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblBudgetName");
        var lblBudgetYear = (Label)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblBudgetYear");
        lblBudgetName.Text = drpBudgets.SelectedItem.Text;
        lblBudgetYear.Text = " - " + txtYearLoad.Text;
        var budgetNamePanel = (HtmlGenericControl)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("budgetHeader");
        budgetNamePanel.Visible = true;
        lnkDeleteBudget.Visible = true;
        lnkRefresh.Visible = true;
        lnkSaveBudget.Text = "Update";
        budgetSavePanel.Visible = true;
        lblMessage.Text = "Budget loaded from Saved Budget " + drpBudgets.SelectedItem.Text;
        var accountCount = Convert.ToInt32(Session["totalAccounts"].ToString());
        if (accountCount > 0)
        {
            var lblTotalAccounts = (Label)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblTotalAccounts");
            lblTotalAccounts.Text = "Total Accounts : " + accountCount;
            budgetNamePanel.Visible = true;

        }
        StyleBudgetGrid();
        Hidden1.Value = string.Empty;
        if (RadGrid_Budget.Items.Count > 1)
        {
            var lastItem = RadGrid_Budget.Items[RadGrid_Budget.Items.Count - 1];
            var lbl = (Label)lastItem.FindControl("lblName");
            if (lbl.Text.Contains("Total"))
                lastItem.BackColor = System.Drawing.Color.LightBlue;
        }
    }

    protected void txtYearLoad_TextChanged(object sender, EventArgs e)
    {
        DataSet ds = bL_Budgets.GetBudget(Session["config"].ToString(), Convert.ToInt32(txtYearLoad.Value));
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblSelectBudget.Visible = true;
            drpBudgets.Visible = true;
        }
        else
        {
            lblSelectBudget.Visible = false;
            drpBudgets.Visible = false;
        }
        drpBudgets.DataSource = ds;
        drpBudgets.DataBind();
        drpBudgets.DataTextField = "Budget";
        drpBudgets.DataValueField = "BudgetID";
        drpBudgets.DataBind();
    }

    protected void txtAnnualTotal_TextChanged(object sender, EventArgs e)
    {
        var txtAnnualTotal = (TextBox)sender;
        txtAnnualTotal.Text = (Convert.ToDouble(txtAnnualTotal.Text)).ToString("N");
        GridDataItem item = (GridDataItem)txtAnnualTotal.Parent.Parent;
        var hdnType = (HiddenField)item.FindControl("hdnType");


        for (int j = 1; j <= 12; j++)
        {
            var txt = (TextBox)item.FindControl("txtMonth" + j);
            //txt.Text = (Convert.ToDouble(txtAnnualTotal.Text) / 12).ToString("N");
            txt.Text = String.Format("{0:n}", Convert.ToDouble(txtAnnualTotal.Text) / 12);
            txt.BorderStyle = BorderStyle.None;
            TextBox txtColTotal = new TextBox();
            var monthTotal = 0.00;

            for (int k = 0; k < RadGrid_Budget.Items.Count; k++)
            {

                var rowHdn = (HiddenField)RadGrid_Budget.Items[k].FindControl("hdnType");
                var lblName = (Label)RadGrid_Budget.Items[k].FindControl("lblName");
                if (lblName.Text.Contains("Total") && rowHdn.Value.Contains(hdnType.Value))
                    txtColTotal = (TextBox)RadGrid_Budget.Items[k].FindControl("txtMonth" + j);
                else
                {
                    if (rowHdn.Value == hdnType.Value)
                    {
                        txt = (TextBox)RadGrid_Budget.Items[k].FindControl("txtMonth" + j);
                        if (!string.IsNullOrEmpty(txt.Text))
                            monthTotal += Convert.ToDouble(txt.Text);
                    }
                }
            }
            txtColTotal.Text = monthTotal.ToString("N");
        }

        RecalculateTotals(hdnType.Value);
    }

    protected void cmdExportToExcel_Click(object sender, EventArgs e)
    {

        if (drpBudgets.SelectedItem != null)
            RadGrid_Budget.ExportSettings.FileName = drpBudgets.SelectedItem.Text.Trim();
        RadGrid_Budget.HeaderStyle.Font.Size = FontUnit.Larger;
        RadGrid_Budget.HeaderStyle.Font.Bold = true;
        RadGrid_Budget.MasterTableView.ExportToExcel();
        Page.Response.ClearHeaders();
        Page.Response.ClearContent();
    }

    protected void checkAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox checkAll = sender as CheckBox;

        foreach (GridDataItem item in RadGrid_Budget.MasterTableView.Items)
        {
            CheckBox cboxSelect = item.FindControl("cboxSelect") as CheckBox;
            cboxSelect.Checked = checkAll.Checked;
        }
    }

    protected void cmdClear_Click(object sender, EventArgs e)
    {
        //GridDataItem item = (GridDataItem)cmdClear.NamingContainer;
        foreach (GridDataItem item in RadGrid_Budget.MasterTableView.Items)
        {
            CheckBox cboxSelect = item.FindControl("cboxSelect") as CheckBox;
            var hdnType = (HiddenField)item.FindControl("hdnType");
            TextBox txtColTotal = new TextBox();
            var monthTotal = 0.00;
            if (cboxSelect.Checked == true)
            {
                var txtAnnual = (TextBox)item.FindControl("txtAnnualTotal");
                if (txtAnnual != null)
                    txtAnnual.Text = string.Empty;
                for (int j = 1; j <= 12; j++)
                {
                    var txt = (TextBox)item.FindControl("txtMonth" + j);
                    txt.Text = string.Empty;
                    txt.BorderStyle = BorderStyle.None;

                    for (int k = 0; k < RadGrid_Budget.Items.Count; k++)
                    {
                        var rowHdn = (HiddenField)RadGrid_Budget.Items[k].FindControl("hdnType");
                        var lblName = (Label)RadGrid_Budget.Items[k].FindControl("lblName");
                        if (lblName.Text.Contains("Total") && rowHdn.Value.Contains(hdnType.Value))
                            txtColTotal = (TextBox)RadGrid_Budget.Items[k].FindControl("txtMonth" + j);
                        else
                        {
                            if (rowHdn.Value == hdnType.Value)
                            {
                                txt = (TextBox)RadGrid_Budget.Items[k].FindControl("txtMonth" + j);
                                if (!string.IsNullOrEmpty(txt.Text))
                                    monthTotal += Convert.ToDouble(txt.Text);
                            }
                        }
                    }
                    txtColTotal.Text = monthTotal.ToString("N");
                    monthTotal = 0.00;

                }
                RecalculateTotals(hdnType.Value);
                cboxSelect.Checked = false;
            }


        }
    }

    protected void rcAccountType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GetBudgetData(false);
        RadGrid_Budget.CurrentPageIndex = 0;
        RadGrid_Budget.DataBind();
    }

    protected void RadGrid_Budget_FilterCheckListItemsRequested(object sender, GridFilterCheckListItemsRequestedEventArgs e)
    {
        string DataField = (e.Column as IGridDataColumn).GetActiveDataField();
    }

    protected void RadGrid_Budget_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && RadGrid_Budget.MasterTableView.Items.Count > 0)
        {
            RadGrid_Budget.MasterTableView.Items[0].Expanded = true;
        }
        foreach (GridGroupHeaderItem GroupHeader in RadGrid_Budget.MasterTableView.GetItems(GridItemType.GroupHeader))
        {
            GroupHeader.Controls[0].Visible = false;

            if (GroupHeader.DataCell.Text == Hidden1.Value && !(string.IsNullOrEmpty(GroupHeader.DataCell.Text)))
            {
                GroupHeader.Expanded = !GroupHeader.Expanded;
            }
        }
    }

    protected void cmdUpdateBudget_Click(object sender, EventArgs e)
    {
        string[] indexes = rowNumbersEdited.Value.Split(',');
        var lblBudgetName = (Label)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblBudgetName");
        var lblBudgetYear = (Label)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblBudgetYear");
        Budget objBudget = new Budget();
        objBudget.BudgetValue = lblBudgetName.Text;
        objBudget.Year = Convert.ToInt32(txtYearLoad.Value);
        int budgetId = Convert.ToInt32(bL_Budgets.GetBudgetID(Session["config"].ToString(), objBudget.BudgetValue));
        if (budgetId > 0)
        {
            objBudget.BudgetId = budgetId;
            if (!string.IsNullOrEmpty(txtBudgetName.Text))
            {
                objBudget.BudgetValue = txtBudgetName.Text.Trim();
                objBudget.Year = Convert.ToInt32(txtYearLoad.Value);
                budgetId = Convert.ToInt32(bL_Budgets.GetBudgetID(Session["config"].ToString(), objBudget.BudgetValue));
                if (budgetId > 0)
                {
                    lblMessage.Text = "Budget " + txtBudgetName.Text.Trim() + " already exists";
                    return;
                }
                else
                {
                    objBudget.ConnConfig = Session["config"].ToString();
                    bL_Budgets.UpdateBudgetName(objBudget);
                }
            }
            else
            {
                objBudget.BudgetValue = lblBudgetName.Text.Trim();
            }
        }
        else
            return;

        // Get year end month
        string strQuery = "select [dbo].[Control].[YE] from [dbo].[control]";
        int? yearEnd; int startMonth = 0;

        var ds1 = SqlHelper.ExecuteDataset(Session["config"].ToString(), CommandType.Text, strQuery);
        if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
        {
            yearEnd = int.Parse(ds1.Tables[0].Rows[0][0].ToString());
            startMonth = int.Parse(yearEnd.ToString()) + 2;

            if (startMonth > 12)
            {
                startMonth = startMonth - 12;
            }
        }

        for (int k = 0; k < indexes.Length; k++)
        {
            var index = Convert.ToInt32(indexes[k]);
            GridItem item = RadGrid_Budget.Items[index];

            GridDataItem dataItem = (GridDataItem)item;
            if (item.ItemType == GridItemType.Item || item.ItemType == GridItemType.AlternatingItem)
            {
                Account objAccount = new Account();
                int accountId = 0;
                var lblAcct = (Label)dataItem.FindControl("lblId");
                var lblName = (Label)dataItem.FindControl("lblName");
                var hdnType = (HiddenField)dataItem.FindControl("hdnType");
                objAccount.Acct = lblAcct.Text;
                objAccount.fDesc = lblName.Text;
                objAccount.Type = hdnType.Value;
                objAccount.AccRoot = objAccount.Balance = objAccount.Branch = objAccount.CAlias = objAccount.Control = objAccount.CostCenter = "NULL";
                objAccount.DAT = objAccount.Detail = objAccount.InUse = objAccount.Remarks = objAccount.Status = objAccount.Sub = objAccount.Sub2 = "NULL";
                objAccount.ConnConfig = Session["config"].ToString();

                if (!objAccount.fDesc.Trim().ToUpper().Contains("TOTAL"))
                {
                    accountId = bL_Budgets.AddAccount(objAccount);
                }

                if (!string.IsNullOrEmpty(lblName.Text) && accountId != 0)
                {
                    AccountDetail objAccountDetail = new AccountDetail();
                    objAccountDetail.AccountId = accountId;
                    objAccountDetail.BudgetId = budgetId;

                    var currentMonth = startMonth;
                    for (int i = 1; i <= 12; i++)
                    {
                        var txt = (TextBox)item.FindControl("txtMonth" + i);
                        if (txt != null)
                        {
                            if (!string.IsNullOrEmpty(txt.Text))
                            {
                                objAccountDetail = SetValues(i.ToString(), txt.Text, objAccountDetail);
                            }
                            else
                                Convert.ToDouble("0.00");
                        }

                        var budgetYear = txtYearLoad.Value;
                        if (startMonth != 1)
                        {
                            if (currentMonth >= startMonth && currentMonth <= 12)
                            {
                                budgetYear = budgetYear - 1;
                            }
                            else
                            {
                                budgetYear = txtYearLoad.Value;
                            }
                        }

                        objAccountDetail.Period = Convert.ToInt32(budgetYear) * 100 + currentMonth;

                        if (txt != null)
                        {
                            if (!string.IsNullOrEmpty(txt.Text))
                            {
                                objAccountDetail.Credit = objAccountDetail.Debit = objAccountDetail.Amount = Convert.ToDouble(txt.Text);
                            }
                            else
                            {
                                objAccountDetail.Credit = objAccountDetail.Debit = objAccountDetail.Amount = Convert.ToDouble("0.00");
                            }
                        }

                        objAccountDetail.ConnConfig = Session["config"].ToString();
                        bL_Budgets.AddAccountDetails(objAccountDetail);

                        if (currentMonth < 12)
                        {
                            currentMonth++;
                        }
                        else if (currentMonth == 12)
                        {
                            currentMonth = 1;
                        }
                    }

                    var txtTotal = (TextBox)item.FindControl("txtAnnualTotal");
                    if (txtTotal != null)
                    {
                        if (!string.IsNullOrEmpty(txtTotal.Text))
                        {
                            objAccountDetail.Total = Convert.ToDouble(txtTotal.Text);
                        }
                        else
                        {
                            objAccountDetail.Total = Convert.ToDouble("0.00");
                        }
                    }

                    objAccountDetail.ConnConfig = Session["config"].ToString();
                    bL_Budgets.AddBudgetAccountDetails(objAccountDetail);
                }
            }
        }

        lblBudgetName.Text = objBudget.BudgetValue;
        lblBudgetYear.Text = " - " + txtYearLoad.Text;
        lblMessage.Text = objBudget.BudgetValue + " for " + txtYearLoad.Value + " updated successfully";
        rowNumbersEdited.Value = "";
        GetBudgetData(false);
    }

    protected void RadGrid_Budget_DataBound(object sender, EventArgs e)
    {
        if (Session["totalAccounts"] != null)
        {
            var accountCount = Convert.ToInt32(Session["totalAccounts"].ToString());
            if (accountCount > 0)
            {
                try
                {
                    var lblTotalAccounts = (Label)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblTotalAccounts");
                    lblTotalAccounts.Text = "Total Accounts : " + accountCount;
                    var budgetNamePanel = (HtmlGenericControl)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("budgetHeader");
                    budgetNamePanel.Visible = true;

                    Session["totalAccounts"] = accountCount;
                    var lblBudgetName = (Label)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblBudgetName");
                    var lblBudgetYear = (Label)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblBudgetYear");
                    if (drpBudgets.SelectedItem != null)
                        lblBudgetName.Text = drpBudgets.SelectedItem.Text;
                    else
                        lblBudgetName.Text = "-- Select Budget --";

                    lblBudgetYear.Text = " - " + txtYearLoad.Text;
                }
                catch (Exception ex)
                {

                }

            }
        }
    }

    private bool CheckIfSummaryRowExists(DataTable budgetTable, string typeName)
    {
        DataTable dt = new DataTable();
        dt = budgetTable.Copy();
        var dv = dt.DefaultView;
        dv.RowFilter = "AcctName = 'Total " + typeName + "'";
        if (dv.ToTable().Rows.Count > 0)
            return true;

        return false;
    }

    private void RecalculateTotals(string typename)
    {
        var annualTotal = 0.00;
        double[] revenuesTotal = new double[13];
        double[] costOfSalesTotal = new double[13];
        double[] expensesTotal = new double[13];
        for (int k = 0; k < RadGrid_Budget.Items.Count; k++)
        {
            var rowHdn = (HiddenField)RadGrid_Budget.Items[k].FindControl("hdnType");
            var lblName = (Label)RadGrid_Budget.Items[k].FindControl("lblName");
            if (!(lblName.Text.Contains("Total " + typename)) && rowHdn.Value.Contains(typename))
            {

                var txt = (TextBox)RadGrid_Budget.Items[k].FindControl("txtAnnualTotal");
                if (!string.IsNullOrEmpty(txt.Text))
                    annualTotal += Convert.ToDouble(txt.Text);


            }
            if (lblName.Text.Contains("Total " + typename) && rowHdn.Value.Contains(typename))
            {
                var txtAnnual = (TextBox)RadGrid_Budget.Items[k].FindControl("txtAnnualTotal");
                txtAnnual.Text = annualTotal.ToString("N");

            }
            if (lblName.Text.Trim().Contains("Total Revenues"))
            {
                var txtAnnual = (TextBox)RadGrid_Budget.Items[k].FindControl("txtAnnualTotal");
                revenuesTotal[0] = Convert.ToDouble(txtAnnual.Text);
                for (int j = 1; j <= 12; j++)
                {
                    var txtMonth = (TextBox)RadGrid_Budget.Items[k].FindControl("txtMonth" + j);
                    if (!string.IsNullOrEmpty(txtMonth.Text))
                        revenuesTotal[j] = Convert.ToDouble(txtMonth.Text);
                }
            }
            else if (lblName.Text.Trim().Contains("Total Cost of Sales"))
            {
                var txtAnnual = (TextBox)RadGrid_Budget.Items[k].FindControl("txtAnnualTotal");
                costOfSalesTotal[0] = Convert.ToDouble(txtAnnual.Text);
                for (int j = 1; j <= 12; j++)
                {
                    var txtMonth = (TextBox)RadGrid_Budget.Items[k].FindControl("txtMonth" + j);
                    if (!string.IsNullOrEmpty(txtMonth.Text))
                        costOfSalesTotal[j] = Convert.ToDouble(txtMonth.Text);
                }
            }
            else if (lblName.Text.Trim().Contains("Total Expenses"))
            {
                var txtAnnual = (TextBox)RadGrid_Budget.Items[k].FindControl("txtAnnualTotal");
                expensesTotal[0] = Convert.ToDouble(txtAnnual.Text);
                for (int j = 1; j <= 12; j++)
                {
                    var txtMonth = (TextBox)RadGrid_Budget.Items[k].FindControl("txtMonth" + j);
                    if (!string.IsNullOrEmpty(txtMonth.Text))
                        expensesTotal[j] = Convert.ToDouble(txtMonth.Text);
                }
            }
            else if (lblName.Text.Trim().ToUpper().Equals("NET PROFIT TOTAL"))
            {
                double netProfitTotals = Convert.ToDouble(revenuesTotal[0]) - Convert.ToDouble(costOfSalesTotal[0]) - Convert.ToDouble(expensesTotal[0]);
                var txtAnnual = (TextBox)RadGrid_Budget.Items[k].FindControl("txtAnnualTotal");
                txtAnnual.Text = netProfitTotals.ToString("N");
                for (int j = 1; j <= 12; j++)
                {
                    var txtMonth = (TextBox)RadGrid_Budget.Items[k].FindControl("txtMonth" + j);
                    txtMonth.Text = (Convert.ToDouble(revenuesTotal[j]) - Convert.ToDouble(costOfSalesTotal[j]) - Convert.ToDouble(expensesTotal[j])).ToString("N");
                }
            }
        }
    }

    protected void txtMonth_TextChanged(object sender, EventArgs e)
    {
        var txtMonth = (TextBox)sender;
        TextBox txtColTotal = new TextBox();
        if (!string.IsNullOrEmpty(txtMonth.Text))
            txtMonth.Text = Convert.ToDouble(txtMonth.Text).ToString("N");
        TextBox txt = new TextBox();
        //txt.Text = Convert.ToDouble(txt.Text).ToString("N");
        var monthID = txtMonth.ID;
        var monthNumber = Convert.ToInt32(string.Join(null, System.Text.RegularExpressions.Regex.Split(monthID, "[^\\d]")));

        GridDataItem item = (GridDataItem)txtMonth.Parent.Parent;
        var annualTotal = 0.00;
        var monthTotal = 0.00;
        for (int j = 1; j <= 12; j++)
        {
            txt = (TextBox)item.FindControl("txtMonth" + j);
            if (!string.IsNullOrEmpty(txt.Text))
                annualTotal += Convert.ToDouble(txt.Text);
        }
        var txtAnnual = (TextBox)item.FindControl("txtAnnualTotal");
        txtAnnual.Text = annualTotal.ToString("N");

        var month = setMonth(monthNumber, true);
        var hdnType = (HiddenField)item.FindControl("hdnType");
        for (int k = 0; k < RadGrid_Budget.Items.Count; k++)
        {
            var rowHdn = (HiddenField)RadGrid_Budget.Items[k].FindControl("hdnType");
            var lblName = (Label)RadGrid_Budget.Items[k].FindControl("lblName");
            if (lblName.Text.Contains("Total") && rowHdn.Value.Contains(hdnType.Value))
                txtColTotal = (TextBox)RadGrid_Budget.Items[k].FindControl("txtMonth" + monthNumber);
            else
            {
                if (rowHdn.Value == hdnType.Value)
                {
                    txt = (TextBox)RadGrid_Budget.Items[k].FindControl("txtMonth" + monthNumber);
                    if (!string.IsNullOrEmpty(txt.Text))
                        monthTotal += Convert.ToDouble(txt.Text);
                }
            }
        }
        txtColTotal.Text = monthTotal.ToString("N");
        RecalculateTotals(hdnType.Value);

    }

    protected void txtPercentage_TextChanged(object sender, EventArgs e)
    {
        var percentage = (TextBox)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("txtPercentage");
        for (int k = 0; k < RadGrid_Budget.Items.Count; k++)
        {
            GridDataItem item = (GridDataItem)RadGrid_Budget.Items[k];
            for (int j = 1; j <= 12; j++)
            {
                var txt = (TextBox)item.FindControl("txtMonth" + j);
                if (!string.IsNullOrEmpty(txt.Text))
                {
                    var currentValue = Convert.ToDouble(txt.Text);
                    txt.Text = (currentValue + (currentValue * Convert.ToDouble(percentage.Text) / 100)).ToString("N");
                    txt.BorderStyle = BorderStyle.None;
                }
            }
            var txtAnnualTotal = (TextBox)item.FindControl("txtAnnualTotal");
            var currentAnnualValue = Convert.ToDouble(txtAnnualTotal.Text);
            txtAnnualTotal.Text = (currentAnnualValue + (currentAnnualValue * Convert.ToDouble(percentage.Text) / 100)).ToString("N");
        }
    }

    protected void cmdCopyAcross_Click(object sender, EventArgs e)
    {
        var hdnType = new HiddenField();
        foreach (GridDataItem item in RadGrid_Budget.MasterTableView.Items)
        {
            CheckBox cboxSelect = item.FindControl("cboxSelect") as CheckBox;
            hdnType = (HiddenField)item.FindControl("hdnType");
            var txt = new TextBox();
            TextBox txtColTotal = new TextBox();
            var monthTotal = 0.00;
            if (cboxSelect.Checked == true)
            {
                var txtJan = (TextBox)item.FindControl("txtMonth1");
                for (int j = 1; j <= 12; j++)
                {
                    if (j > 1)
                    {
                        var txtM = (TextBox)item.FindControl("txtMonth" + j);
                        txtM.Text = txtJan.Text;
                        for (int k = 0; k < RadGrid_Budget.Items.Count; k++)
                        {
                            var rowHdn = (HiddenField)RadGrid_Budget.Items[k].FindControl("hdnType");
                            var lblName = (Label)RadGrid_Budget.Items[k].FindControl("lblName");
                            if (lblName.Text.Contains("Total") && rowHdn.Value.Contains(hdnType.Value))
                            {
                                txtColTotal = (TextBox)RadGrid_Budget.Items[k].FindControl("txtMonth" + j);
                            }
                            else
                            {
                                if (rowHdn.Value == hdnType.Value)
                                {
                                    txt = (TextBox)RadGrid_Budget.Items[k].FindControl("txtMonth" + j);
                                    if (!string.IsNullOrEmpty(txt.Text))
                                        monthTotal += Convert.ToDouble(txt.Text);
                                }
                            }
                        }
                        txtColTotal.Text = monthTotal.ToString("N");

                        monthTotal = 0.00;
                    }
                }

                var txtAnnualTotal = (TextBox)item.FindControl("txtAnnualTotal");
                txtAnnualTotal.Text = (double.Parse(txtJan.Text) * 12).ToString("N");
            }

            RecalculateTotals(hdnType.Value);
        }
    }

    protected void txtYear_TextChanged(object sender, EventArgs e)
    {
        DataSet ds = bL_Budgets.GetBudget(Session["config"].ToString(), Convert.ToInt32(txtYear.Value));
        if (ds.Tables[0].Rows.Count > 0 && drpBudgetType.SelectedValue.Equals("Saved Budgets"))
        {
            lblSelectBudget.Visible = true;
            drpBudgetsList.Visible = true;
            lblSavedBudgets.Visible = true;
        }
        else
        {
            lblSelectBudget.Visible = false;
            drpBudgetsList.Visible = false;
            lblSavedBudgets.Visible = false;
        }

        drpBudgetsList.DataSource = ds;
        drpBudgetsList.DataBind();
        drpBudgetsList.DataTextField = "Budget";
        drpBudgetsList.DataValueField = "BudgetID";
        drpBudgetsList.DataBind();
        drpBudgetsList.Items.Insert(0, new ListItem("-- Select Budget --", "0"));
    }

    protected void cmdIncrese_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Budget.MasterTableView.Items)
        {
            CheckBox cboxSelect = item.FindControl("cboxSelect") as CheckBox;
            if (cboxSelect.Checked == true)
            {
                var percentage = (TextBox)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("txtPercentage");
                for (int k = 0; k < RadGrid_Budget.Items.Count; k++)
                {
                    GridDataItem items = (GridDataItem)RadGrid_Budget.Items[k];
                    for (int j = 1; j <= 12; j++)
                    {
                        var txt = (TextBox)item.FindControl("txtMonth" + j);
                        if (!string.IsNullOrEmpty(txt.Text))
                        {
                            var currentValue = Convert.ToDouble(txt.Text);
                            txt.Text = (currentValue + (currentValue * Convert.ToDouble(percentage.Text) / 100)).ToString("N");
                            txt.BorderStyle = BorderStyle.None;
                        }
                    }
                    var txtAnnualTotal = (TextBox)item.FindControl("txtAnnualTotal");
                    var currentAnnualValue = Convert.ToDouble(txtAnnualTotal.Text);
                    txtAnnualTotal.Text = (currentAnnualValue + (currentAnnualValue * Convert.ToDouble(percentage.Text) / 100)).ToString("N");
                }
            }
        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    protected void RadGrid_Budget_ExcelExportCellFormatting(object sender, ExcelExportCellFormattingEventArgs e)
    {
        e.Cell.Text = String.Format("&nbsp;{0}", e.Cell.Text);
    }

    protected void RadGrid_Budget_InfrastructureExporting(object sender, GridInfrastructureExportingEventArgs e)
    {
        foreach (Telerik.Web.UI.ExportInfrastructure.Cell cell in e.ExportStructure.Tables[0].Cells)
        {
            if (!cell.Text.Contains("-") && cell.Text.Contains("."))
                cell.Format = @"0.00";
            else
                cell.Format = @"@";
        }
    }

    protected void RadGrid_Budget_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
    {
        GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;

        switch (e.DetailTableView.Name)
        {
            case "ChildAccounts":
                {
                    var lblName = (Label)dataItem.FindControl("lblId");
                    string accountNumber = lblName.Text.Trim().ToString();
                    var ds = bL_Budgets.GetChildAccounts(Session["Config"].ToString(), accountNumber);
                    var dt = BuildBudgetTable();
                    dt.Columns.Add("ParentAccNumber");
                    dt = ProcessAndBuildChildAccounts(ds, dt, accountNumber);
                    e.DetailTableView.DataSource = dt;
                    //e.DetailTableView.DataBind();
                    break;
                }
        }
    }

    protected void RemoveExpandIconWhenNoRecords(GridTableView view)
    {
        if (view.Controls.Count > 0)
        {
            foreach (GridItem item in view.Controls[0].Controls)
            {
                if (item is GridNestedViewItem)
                {
                    GridNestedViewItem nestedViewItem = (GridNestedViewItem)item;
                    if (nestedViewItem.NestedTableViews[0].Items.Count == 0)
                    {
                        TableCell cell = nestedViewItem.NestedTableViews[0].ParentItem["ExpandColumn"];
                        cell.Controls[0].Visible = false;
                        cell.Text = " ";
                        nestedViewItem.Visible = false;
                    }
                    else
                    {
                        RemoveExpandIconWhenNoRecords(nestedViewItem.NestedTableViews[0]);
                    }
                }
            }
        }
    }

    protected void newRow_Click(object sender, EventArgs e)
    {

    }

    protected void RadGrid_Budget_ItemInserted(object sender, GridInsertedEventArgs e)
    {

    }

    protected void RadGrid_Budget_InsertCommand(object sender, GridCommandEventArgs e)
    {
        switch (e.Item.OwnerTableView.Name)
        {
            case "ChildAccounts":
                {
                    GridDataItem parentItem = (GridDataItem)e.Item.OwnerTableView.ParentItem;
                    var lblAcct = (Label)parentItem.FindControl("lblId");
                    var parentAcctNumber = lblAcct.Text.Trim();
                    var childActCtrl = (TextBox)e.Item.FindControl("TB_AccountNumber");
                    var childAcctNumber = childActCtrl.Text.Trim();
                    var childActNameCtrl = (TextBox)e.Item.FindControl("TB_AccountName");
                    var childActName = childActNameCtrl.Text.Trim();
                    var childActDescCtrl = (TextBox)e.Item.FindControl("TB_fDesc");
                    var childActDesc = childActDescCtrl.Text.Trim();

                    Account objAccount = new Account();
                    int accountId = 0;
                    var hdnType = (HiddenField)parentItem.FindControl("hdnType");
                    objAccount.Acct = childAcctNumber;
                    objAccount.fDesc = childActName;
                    objAccount.Type = hdnType.Value;
                    objAccount.AccRoot = parentAcctNumber;
                    objAccount.Balance = objAccount.Branch = objAccount.CAlias = objAccount.Control = objAccount.CostCenter = "NULL";
                    objAccount.DAT = objAccount.Detail = objAccount.InUse = objAccount.Remarks = objAccount.Status = objAccount.Sub = objAccount.Sub2 = "NULL";
                    objAccount.ConnConfig = Session["config"].ToString();
                    if (!objAccount.fDesc.Trim().ToUpper().Contains("TOTAL"))
                        accountId = bL_Budgets.AddAccount(objAccount);
                }
                break;

        }
    }

    protected void LnkDelete_Click(object sender, EventArgs e)
    {
        var lblBudgetName = (Label)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblBudgetName");
        var lblBudgetYear = (Label)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblBudgetYear");
        Budget objBudget = new Budget();

        if (!string.IsNullOrEmpty(lblBudgetName.Text))
        {
            objBudget.BudgetValue = lblBudgetName.Text;
        }
        else if (!string.IsNullOrEmpty(drpBudgets.SelectedItem.Text))
        {
            objBudget.BudgetValue = drpBudgets.SelectedItem.Text.Trim();
        }

        objBudget.Year = Convert.ToInt32(txtYear.Value);
        int budgetId = bL_Budgets.GetBudgetID(Session["config"].ToString(), objBudget.BudgetValue);
        bL_Budgets.DeleteBudget(Session["config"].ToString(), budgetId.ToString());

        GetBudgetData(true);
        RadGrid_Budget.CurrentPageIndex = 0;
        RadGrid_Budget.DataBind();
        DataSet ds = bL_Budgets.GetBudget(Session["config"].ToString(), Convert.ToInt32(txtYearLoad.Value));

        drpBudgets.Items.Clear();

        drpBudgets.DataSource = ds;
        drpBudgets.DataBind();
        drpBudgets.DataTextField = "Budget";
        drpBudgets.DataValueField = "BudgetID";
        drpBudgets.DataBind();
        drpBudgets.SelectedIndex = 0;
        txtBudgetName.Text = String.Empty;

        if (drpBudgets.Items.Count == 0)
        {
            budgetSavePanel.Visible = false;
        }

        lblMessage.Text = "Budget " + objBudget.BudgetValue + " for " + txtYearLoad.Value + " deleted successfully";
    }

    protected void drpBudgetType_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = bL_Budgets.GetBudget(Session["config"].ToString(), Convert.ToInt32(txtYear.Value));
        if (ds != null && drpBudgetType.SelectedValue != null)
        {
            if (ds.Tables[0].Rows.Count > 0 && drpBudgetType.SelectedValue.Equals("Saved Budgets"))
            {
                lblSelectBudget.Visible = true;
                drpBudgetsList.Visible = true;
                lblSavedBudgets.Visible = true;
            }
        }
        else
        {
            lblSelectBudget.Visible = false;
            drpBudgetsList.Visible = false;
            lblSavedBudgets.Visible = false;
        }

        drpBudgetsList.DataSource = ds;
        drpBudgetsList.DataBind();
        drpBudgetsList.DataTextField = "Budget";
        drpBudgetsList.DataValueField = "BudgetID";
        drpBudgetsList.DataBind();
        drpBudgetsList.Items.Insert(0, new ListItem("-- Select Budget --", "0"));
    }

    protected void RadGrid_Budget_ItemUpdated(object sender, GridUpdatedEventArgs e)
    {
        var item = e.Item.FindControl("txtAnnualTotal");
    }

    protected void txtAnnualTotal_TextChanged1(object sender, EventArgs e)
    {
        TextBox txt = (TextBox)sender;

    }

    protected void lnkRefresh_Click(object sender, EventArgs e)
    {
        DataTable budgetDataTable = BuildBudgetTable();
        DataSet ds = bL_Budgets.GetBudgetsData(Session["config"].ToString(), drpBudgets.SelectedItem.Text.Trim(), GetSelectedAccountType());
        var dt = ProcessAndBuildData(budgetDataTable, ds, false);

        dt.DefaultView.Sort = "Type ASC, fDesc ASC, AcctNumber ASC";
        dt = dt.DefaultView.ToTable();

        Session["Budgets"] = dt;

        //Merge the 2 datatables and assign it as DataSource
        RadGrid_Budget.DataSource = dt;
        RadGrid_Budget.MasterTableView.VirtualItemCount = budgetDataTable.Rows.Count;
        RadGrid_Budget.CurrentPageIndex = RadGrid_Budget.MasterTableView.CurrentPageIndex;
        RadGrid_Budget.DataBind();

        var lblBudgetName = (Label)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblBudgetName");
        var lblBudgetYear = (Label)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblBudgetYear");
        lblBudgetName.Text = drpBudgets.SelectedItem.Text;
        lblBudgetYear.Text = " - " + txtYearLoad.Text;
        var budgetNamePanel = (HtmlGenericControl)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("budgetHeader");
        budgetNamePanel.Visible = true;
        lnkDeleteBudget.Visible = true;
        lnkRefresh.Visible = true;
        lnkSaveBudget.Text = "Update";
        budgetSavePanel.Visible = true;
        lblMessage.Text = "Budget loaded from Saved Budget " + drpBudgets.SelectedItem.Text;

        var accountCount = Convert.ToInt32(Session["totalAccounts"].ToString());
        if (accountCount > 0)
        {
            var lblTotalAccounts = (Label)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblTotalAccounts");
            lblTotalAccounts.Text = "Total Accounts : " + accountCount;
            budgetNamePanel.Visible = true;
        }

        StyleBudgetGrid();
        Hidden1.Value = string.Empty;

        if (RadGrid_Budget.Items.Count > 1)
        {
            var lastItem = RadGrid_Budget.Items[RadGrid_Budget.Items.Count - 1];
            var lbl = (Label)lastItem.FindControl("lblName");
            if (lbl.Text.Contains("Total"))
                lastItem.BackColor = System.Drawing.Color.LightBlue;
        }
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        if (txtYearLoad.Value != null && !string.IsNullOrEmpty(drpBudgets.SelectedValue) && drpBudgets.SelectedValue != "--Select budget--")
        {
            RadGrid_Budget.ExportSettings.FileName = "Budgets";
            RadGrid_Budget.ExportSettings.IgnorePaging = true;
            RadGrid_Budget.ExportSettings.ExportOnlyData = true;
            RadGrid_Budget.ExportSettings.OpenInNewWindow = true;
            RadGrid_Budget.ExportSettings.HideStructureColumns = true;
            RadGrid_Budget.MasterTableView.UseAllDataFields = true;
            RadGrid_Budget.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
            RadGrid_Budget.MasterTableView.ExportToExcel();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningExport", "noty({text: 'Please specify a year and select a budget before exporting to excel!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            txtYearLoad.Focus();
        }
    }

    protected void lnkImport_Click(object sender, EventArgs e)
    {
        try
        {
            string[] validFileTypes = { ".csv", ".xls", ".xlsx" };
            string ext = System.IO.Path.GetExtension(FileUploadControl.PostedFile.FileName).ToLower();
            var results = Array.FindAll(validFileTypes, s => s.Equals(ext));
            if (results.Length == 0)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'Please upload a csv or excel file.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
            else
            {
                var ds = ExcelToDataset();
                if(ds != null)
                {
                    var dt = BuildExcelBudgetTable(ds.Tables[0]);
                    dt.DefaultView.Sort = "Type ASC, AcctNumber ASC, fDesc ASC ";
                    dt = dt.DefaultView.ToTable();

                    dt = ProcessAndIncludeSummaryRows(dt, false);
                    dt = AddGrossAndNetProfitRow(dt, false);

                    Session["totalAccounts"] = dt.Rows.Count - 5;
                    Session["Budgets"] = dt;

                    RadGrid_Budget.DataSource = dt;
                    RadGrid_Budget.Rebind();

                    lnkSaveBudget.Text = "Save Budget";
                    budgetSavePanel.Visible = true;
                    lnkDeleteBudget.Visible = false;
                    lnkRefresh.Visible = false;
                    lblMessage.Text = "Budget loaded from Excel file: " + FileUploadControl.FileName;
                    lblMessage.Visible = true;

                    var lblTotalAccounts = (Label)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("lblTotalAccounts");
                    lblTotalAccounts.Text = "Total Accounts : " + (dt.Rows.Count - 5);
                    var budgetNamePanel = (HtmlGenericControl)RadGrid_Budget.MasterTableView.GetItems(GridItemType.CommandItem)[0].FindControl("budgetHeader");
                    budgetNamePanel.Visible = true;
                    lnkDeleteBudget.Visible = false;
                    lnkRefresh.Visible = false;

                    if (drpBudgets.Items.Count > 0)
                    {
                        drpBudgets.SelectedIndex = 0;
                        drpBudgets.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private string GetSelectedAccountType()
    {
        List<string> types = new List<string>();
        string result = string.Empty;

        if (rcAccountType.CheckedItems.Count > 0)
        {
            foreach (var item in rcAccountType.CheckedItems)
            {
                types.Add(item.Value);
            }

            result = string.Join(",", types);
        }

        return result;
    }

    private DataSet ExcelToDataset()
    {
        try
        {
            string ext = System.IO.Path.GetExtension(FileUploadControl.PostedFile.FileName).ToLower();

            string FileSaveWithPath = Server.MapPath("~\\TempPDF" + System.DateTime.Now.ToString("ddMMyyyy_hhmmss") + ext);
            FileUploadControl.SaveAs(FileSaveWithPath);

            OleDbConnection oledbConn = new OleDbConnection();
            if (ext == ".xls")
            {
                oledbConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileSaveWithPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"");
            }
            else if (ext == ".xlsx")
            {
                oledbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileSaveWithPath + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';");
            }

            oledbConn.Open();

            var sheetName = oledbConn.GetSchema("Tables").Rows[0]["TABLE_NAME"];
            OleDbCommand ocmd = new OleDbCommand(string.Format("select * from [{0}]", sheetName), oledbConn);
            OleDbDataAdapter oleda = new OleDbDataAdapter(ocmd);
            DataSet ds = new DataSet();
            oleda.Fill(ds);
            oledbConn.Close();

            if (File.Exists(FileSaveWithPath))
                File.Delete(FileSaveWithPath);

            return ds;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private DataTable BuildExcelBudgetTable(DataTable dt)
    {
        var dtBudget = BuildBudgetTable();

        //Get GL Accounts
        DataSet glAccountsDataSet = bL_Budgets.GetGLAccounts(Session["config"].ToString());
        var dtGL = glAccountsDataSet.Tables[0];

        //Build datable with the GL Accounts Data set
        foreach (DataRow dr in dtGL.Rows)
        {
            var budget = dt.Rows.OfType<DataRow>().FirstOrDefault(x => x["Account Number"].ToString().Trim() == dr["Acct"].ToString().Trim());

            DataRow newRow = dtBudget.NewRow();
            newRow["fDesc"] = dr["fDesc"].ToString();
            newRow["Acct"] = dr["ID"].ToString();
            newRow["AcctName"] = dr["fDesc"].ToString();
            newRow["Type"] = dr["Type"].ToString();
            newRow["Status"] = dr["Status"].ToString() == "1" ? "InActive" : "Active";
            newRow["TypeName"] = dr["TypeName"].ToString();
            newRow["AcctNumber"] = dr["Acct"].ToString();

            if (budget != null)
            {
                newRow["AnnualTotal"] = budget["Total"];
                for (int i = 1; i <= 12; i++)
                {
                    newRow[setMonth(i, true)] = budget[setMonth(i, true)];
                }
            }
            else
            {
                newRow["AnnualTotal"] = "0.00";
                for (int i = 1; i <= 12; i++)
                {
                    newRow[setMonth(i, true)] = 0.00;
                }
            }

            dtBudget.Rows.Add(newRow);
        }

        return dtBudget;
    }
}


