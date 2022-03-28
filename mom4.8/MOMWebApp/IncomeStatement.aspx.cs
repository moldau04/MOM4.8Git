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
using System.IO;
using System.Net.Configuration;
using System.Text;
using System.Collections;
using AjaxControlToolkit;

public partial class IncomeStatement : System.Web.UI.Page
{
    GeneralFunctions objgn = new GeneralFunctions();
    private static string AppDirectory = HttpContext.Current.Server.MapPath(string.Empty);

    Dictionary<string, double> totalRevenues = new Dictionary<string, double>();
    Dictionary<string, double> totalCostOfSales = new Dictionary<string, double>();
    Dictionary<string, double> totalExpenses = new Dictionary<string, double>();
    Dictionary<string, double> totalOtherIncome = new Dictionary<string, double>();
    Dictionary<string, double> totalIncomeTaxes = new Dictionary<string, double>();

    BL_Budgets bL_Budgets = new BL_Budgets();

    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();

    BL_Report _objBLReport = new BL_Report();
    ChartDetails _objChartDetail = new ChartDetails();

    AcctDetails _objAcct = new AcctDetails();
    SubAcctDetails _objSubAcct = new SubAcctDetails();

    BL_General objBL_General = new BL_General();
    General objGenerals = new General();

    CompanyOffice objCompany = new BusinessEntity.CompanyOffice();
    BL_Company objBL_Company = new BL_Company();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    double percent = 0;
    double NRev = 0;

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
                string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

                if (Request.Url.Scheme == "http" && SSL == "1")
                {
                    string URL = Request.Url.ToString();
                    URL = URL.Replace("http://", "https://");
                    Response.Redirect(URL);
                }

                GetSMTPUser();
                BindingBudget(true);
                BindingCenter();
                HighlightSideMenu("financialStatement", "lnkIncomeStatement", "financeStateSub");

                if (Request.QueryString["departments"] != null)
                {
                    var depts = Request.QueryString["departments"].Trim();
                    var deptArray = depts.Split(',');

                    for (int i = 0; i < deptArray.Length; i++)
                    {
                        RadComboBoxItem item = rcCenter.FindItemByValue(deptArray[i]);
                        if (item != null)
                            item.Checked = true;
                    }

                    rcCenter.Visible = true;
                }

                if (Request.QueryString["centers"] != null)
                {
                    var centers = Request.QueryString["centers"].Trim();
                    var cenArray = centers.Split(',');

                    for (int i = 0; i < cenArray.Length; i++)
                    {
                        RadComboBoxItem item = rcCenter1.FindItemByValue(cenArray[i]);
                        if (item != null)
                            item.Checked = true;
                    }

                    rcCenter.Visible = true;
                }

                if (Request.QueryString["budgets"] != null)
                {
                    rcCenter.Visible = true;
                }

                if (Request.QueryString["officeCenter"] != null)
                {
                    ddlOfficeCenter.SelectedValue = Request.QueryString["officeCenter"];
                }

                if (Request.QueryString["cType"] != null)
                {
                    if (Request.QueryString["cType"] == "summary")
                    {
                        rdCollapseAll.Checked = true;
                    }
                    else if (Request.QueryString["cType"] == "sub")
                    {
                        rdDetailWithSub.Checked = true;
                    }
                    else
                    {
                        rdExpandAll.Checked = true;
                    }
                }
                else
                {
                    rdExpandAll.Checked = true;
                }

                if (Request.QueryString["includeZero"] != null)
                {
                    chkIncludeZero.Checked = Convert.ToBoolean(Request.QueryString["includeZero"]);
                }

                int year = DateTime.Now.Year;
                DateTime firstDay = new DateTime(year, 1, 1);
                txtStartDate.Text = firstDay.Date.ToShortDateString();
                txtEndDate.Text = DateTime.Now.Date.ToShortDateString();
                txtYearEnd.Text = new DateTime(DateTime.Now.Year, 1, 1).AddSeconds(12).AddDays(-1).ToShortDateString();

                if (!string.IsNullOrEmpty(Request.QueryString["startDate"]))
                {
                    txtStartDate.Text = Request.QueryString["startDate"].ToString();
                }

                if (!string.IsNullOrEmpty(Request.QueryString["endDate"]))
                {
                    txtEndDate.Text = Request.QueryString["endDate"].ToString();
                }

                if (!string.IsNullOrEmpty(Request.QueryString["fromDate"]))
                {
                    txtFromDt.Text = Request.QueryString["fromDate"].ToString();
                }

                if (!string.IsNullOrEmpty(Request.QueryString["toDate"]))
                {
                    txtToDt.Text = Request.QueryString["toDate"].ToString();
                }

                if (!string.IsNullOrEmpty(Request.QueryString["yearEndingDate"]))
                {
                    txtYearEnd.Text = Request.QueryString["yearEndingDate"].ToString();
                }

                if (Request.QueryString["fromDate"] != null && Request.QueryString["toDate"] != null && Request.QueryString["budgetName"] != null)
                {
                    if (Request.QueryString["budgetName"] != null)
                    {
                        var currentDate = DateTime.Parse(txtToDt.Text).Year;
                        DataSet ds = bL_Budgets.GetBudgetsByYear(Session["config"].ToString(), DateTime.Parse(txtFromDt.Text).Year, currentDate);
                        drpBudgetsList.Items.Clear();
                        drpBudgetsList.DataSource = ds;
                        drpBudgetsList.DataBind();
                        drpBudgetsList.DataTextField = "Budget";
                        drpBudgetsList.DataValueField = "BudgetID";
                        drpBudgetsList.DataBind();
                        drpBudgetsList.Items.Insert(0, new ListItem("-- Select Budget --", ""));
                        drpBudgetsList.Visible = true;
                        if (!string.IsNullOrEmpty(Request.QueryString["budgetName"]))
                        {
                            var budgetName = HttpUtility.UrlDecode(Request.QueryString["budgetName"]);
                            var budget = drpBudgetsList.Items.FindByText(budgetName);
                            if (budget != null)
                            {
                                budget.Selected = true;
                            }
                        }
                    }
                }


                string reportType = Page.Request.QueryString.Get("reportType");

                if (reportType != null)
                {
                    ddlReport.SelectedValue = reportType;

                    if (reportType == "0")
                    {
                        StiWebViewerIncomeStatemnet.Visible = true;
                    }
                    else if (reportType == "1")
                    {
                        StiWebViewerIncomeStatement12Period.Visible = true;
                    }
                    else if (reportType == "2" || reportType == "5")
                    {
                        StiWebViewerBudgetVsActual2.Visible = true;
                    }
                    else if (reportType == "3")
                    {
                        StiWebViewerBudgetVsActual.Visible = true;
                    }
                    else if (reportType == "4")
                    {
                        StiWebViewerIncomeStatementWithCenters.Visible = true;
                    }
                    else if (reportType == "6")
                    {
                        StiWebViewerStandardIncomeStatementComparativeFsWithCenter.Visible = true;
                    }
                    else if (reportType == "7")
                    {
                        StiWebViewerProfitAndLossYTD.Visible = true;
                    }
                    else if (reportType == "8")
                    {
                        StiWebViewerIncomeStatementWithCentersBudgets.Visible = true;
                    }
                }

                LoadMailContent();
            }
            else
            {
                Session["DateRangeType"] = hdnReportSelectDtRange.Value;
            }

            SetAddress();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #region Get Stimulsoft report

    private StiReport PrintStatements(bool isAttachment)
    {
        // Export to PDF        
        try
        {
            StiReport report = new StiReport();
            if (ddlReport.SelectedValue == "0")
            {
                if (!string.IsNullOrEmpty(Request.QueryString["cType"]) && Request.QueryString["cType"] == "summary")
                {
                    report = GetIncomeStatementSummaryReport();
                }
                else
                {
                    report = GetIncomeStatementReport();
                }
            }
            else if (ddlReport.SelectedValue == "1")
            {
                report = Get12PeriodIncomeStatement();
            }
            else if (ddlReport.SelectedValue == "2" || ddlReport.SelectedValue == "5")
            {
                report = GetBudgetVsActualReport();
            }
            else if (ddlReport.SelectedValue == "3")
            {
                report = Get12MonthBudgetVsActualReort();
            }
            else if (ddlReport.SelectedValue == "4")
            {
                if (!string.IsNullOrEmpty(Request.QueryString["cType"]) && Request.QueryString["cType"] == "summary")
                {
                    report = GetIncomeStatementWithCentersSummaryReport();
                }
                else
                {
                    report = GetIncomeStatementWithCentersReport();
                }
            }
            else if (ddlReport.SelectedValue == "6")
            {
                // Todo:
            }
            else if (ddlReport.SelectedValue == "7")
            {
                report = GetProfitAndLossYTDReport();
            }
            else if (ddlReport.SelectedValue == "8")
            {
                if (!string.IsNullOrEmpty(Request.QueryString["cType"]) && Request.QueryString["cType"] == "summary")
                {
                    report = GetIncomeStatementWithCentersBudgetsSummaryReport();
                }
                else
                {
                    report = GetIncomeStatementWithCentersBudgetsReport();
                }
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

    #region Standard Income Statement (reportType = 0)

    private StiReport GetIncomeStatementReport()
    {
        StiReport report = new StiReport();
        try
        {
            string reportPath = Server.MapPath("StimulsoftReports/" + ConfigurationManager.AppSettings["IncomeStatementReport"].ToString());

            if (!string.IsNullOrEmpty(Request.QueryString["cType"]) && Request.QueryString["cType"] == "sub")
            {
                reportPath = Server.MapPath("StimulsoftReports/IncomeStatementWithSub.mrt");
            }

            report.Load(reportPath);
            report.Compile();

            _objChart.ConnConfig = Session["config"].ToString();

            #region Start-End date

            if (!string.IsNullOrEmpty(Request.QueryString["startDate"]))
            {
                _objChart.StartDate = Convert.ToDateTime(Request.QueryString["startDate"]);
            }
            else
            {
                int year = DateTime.Now.Year;
                _objChart.StartDate = new DateTime(year, 1, 1);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["endDate"]))
            {
                _objChart.EndDate = Convert.ToDateTime(Request.QueryString["endDate"]);
            }
            else
            {
                _objChart.EndDate = DateTime.Now.Date;
            }

            #endregion

            #region Set Header

            string _FromDate = "From : " + _objChart.StartDate.ToString("MMMM dd, yyyy");
            string _ToDate = "To : " + _objChart.EndDate.ToString("MMMM dd, yyyy");
            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objPropUser);

            #endregion

            DataSet _dsIncome = _objBLReport.GetIncomeStatementDetails(_objChart);

            if (Request.QueryString["includeZero"] != null && !Convert.ToBoolean(Request.QueryString["includeZero"]))
            {
                DataRow[] drr = _dsIncome.Tables[0].Select("Amount = 0");
                foreach (DataRow row in drr)
                {
                    row.Delete();
                }

                _dsIncome.Tables[0].AcceptChanges();
            }

            _dsIncome.Tables[0].AsEnumerable().ToList()
              .ForEach(b => b["Url"] = (Request.Url.Scheme +
                                          (Uri.SchemeDelimiter +
                                              (Request.Url.Authority +
                                                  (Request.ApplicationPath + "/accountledger.aspx?id=" + b["Acct"].ToString() + "&s=" + System.Web.HttpUtility.UrlEncode(_objChart.StartDate.ToShortDateString()).ToString()
                                                                                                                                      + "&e=" + System.Web.HttpUtility.UrlEncode(_objChart.EndDate.ToShortDateString()).ToString()
                                                  )
                                              )
                                          )
                                       )
                                   );

            _dsIncome.Tables[0].AcceptChanges();

            var dTable = _dsIncome.Tables[0];
            var dView = dTable.DefaultView;
            dView.RowFilter = "Type = 3";
            DataTable Revenues = dView.ToTable();
            Revenues.TableName = "Revenues";

            // Total Revenue
            DataTable TRevenues = Revenues.Clone();
            TRevenues.TableName = "TRevenues";
            var dr = TRevenues.NewRow();
            dr["Acct"] = 1000;
            dr["Type"] = 3;
            dr["TypeName"] = "Revenues";
            dr["Sub"] = "";
            dr["URL"] = "";
            dr["fDesc"] = "Total Revenues";

            double rAmount = 0.00;
            for (int i = 0; i < Revenues.Rows.Count; i++)
            {
                rAmount += double.Parse(Revenues.Rows[i]["Amount"].ToString());
            }

            dr["Amount"] = rAmount;
            TRevenues.Rows.Add(dr.ItemArray);

            // Total Cost Of Sales
            dTable = _dsIncome.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "Type = 4";
            DataTable CostOfSales = dView.ToTable();
            CostOfSales.TableName = "CostOfSales";

            DataTable TCostOfSales = CostOfSales.Clone();
            TCostOfSales.TableName = "TCostOfSales";
            var dr1 = TCostOfSales.NewRow();
            dr1["Acct"] = 2000;
            dr1["Type"] = 4;
            dr1["TypeName"] = "Cost Of Sales";
            dr1["Sub"] = "";
            dr1["URL"] = "";
            dr1["fDesc"] = "Total Cost Of Sales";

            double cAmount = 0.00;
            for (int i = 0; i < CostOfSales.Rows.Count; i++)
            {
                cAmount += double.Parse(CostOfSales.Rows[i]["Amount"].ToString());
            }

            dr1["Amount"] = cAmount;
            TCostOfSales.Rows.Add(dr1.ItemArray);
            var cPercent = rAmount == 0 ? 0 : cAmount / rAmount;

            // Total Expenses
            dTable = _dsIncome.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "Type = 5";
            DataTable Expenses = dView.ToTable();
            Expenses.TableName = "Expenses";

            DataTable TExpenses = Expenses.Clone();
            TExpenses.TableName = "TExpenses";
            var dr2 = TExpenses.NewRow();
            dr2["Acct"] = 3000;
            dr2["Type"] = 5;
            dr2["TypeName"] = "Expenses";
            dr2["Sub"] = "";
            dr2["URL"] = "";
            dr2["fDesc"] = "Total Expenses";

            double eAmount = 0.00;
            for (int i = 0; i < Expenses.Rows.Count; i++)
            {
                eAmount += double.Parse(Expenses.Rows[i]["Amount"].ToString());
            }

            dr2["Amount"] = eAmount;
            TExpenses.Rows.Add(dr2.ItemArray);
            var ePercent = rAmount == 0 ? 0 : eAmount / rAmount;

            // Total Other Income (Expenses)
            dTable = _dsIncome.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "Type = 8";
            DataTable OtherIncome = dView.ToTable();
            OtherIncome.TableName = "OtherIncome";

            DataTable TOtherIncome = OtherIncome.Clone();
            TOtherIncome.TableName = "TOtherIncome";
            var dr8 = TOtherIncome.NewRow();
            dr8["Acct"] = 8000;
            dr8["Type"] = 8;
            dr8["TypeName"] = "Other Income (Expenses)";
            dr8["Sub"] = "";
            dr8["URL"] = "";
            dr8["fDesc"] = "Total Other Income (Expenses)";

            double oAmount = 0.00;
            for (int i = 0; i < OtherIncome.Rows.Count; i++)
            {
                oAmount += double.Parse(OtherIncome.Rows[i]["Amount"].ToString());
            }

            dr8["Amount"] = oAmount;
            TExpenses.Rows.Add(dr8.ItemArray);
            var oPercent = rAmount == 0 ? 0 : oAmount / rAmount;

            // Total Provisions for Income Taxes
            dTable = _dsIncome.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "Type = 9";
            DataTable IncomeTaxes = dView.ToTable();
            IncomeTaxes.TableName = "IncomeTaxes";

            DataTable TIncomeTaxes = IncomeTaxes.Clone();
            IncomeTaxes.TableName = "TIncomeTaxes";
            var dr9 = IncomeTaxes.NewRow();
            dr9["Acct"] = 9000;
            dr9["Type"] = 9;
            dr9["TypeName"] = "Provisions for Income Taxes";
            dr9["Sub"] = "";
            dr9["URL"] = "";
            dr9["fDesc"] = "Total Provisions for Income Taxes";

            double iAmount = 0.00;
            for (int i = 0; i < IncomeTaxes.Rows.Count; i++)
            {
                iAmount += double.Parse(IncomeTaxes.Rows[i]["Amount"].ToString());
            }

            dr9["Amount"] = iAmount;
            TIncomeTaxes.Rows.Add(dr9.ItemArray);
            var iPercent = rAmount == 0 ? 0 : iAmount / rAmount;

            // Gross Profit
            DataTable GrossProfit = Expenses.Clone();
            GrossProfit.TableName = "GrossProfit";
            var dr3 = GrossProfit.NewRow();
            dr3["Acct"] = 4000;
            dr3["Type"] = 4.5;
            dr3["TypeName"] = "GrossProfit";
            dr3["Sub"] = "";
            dr3["URL"] = "";
            dr3["fDesc"] = "Gross Profit";
            double gAmount = rAmount + cAmount;
            dr3["Amount"] = gAmount;
            GrossProfit.Rows.Add(dr3);

            DataTable NetProfit = Expenses.Clone();
            NetProfit.TableName = "GrossProfit";
            var dr4 = NetProfit.NewRow();
            dr4["Acct"] = 7000;
            dr4["Type"] = 6;
            dr4["TypeName"] = "Net Profit";
            dr4["Sub"] = "";
            dr4["URL"] = "";
            dr4["fDesc"] = "Net Profit";

            double nAmount = gAmount - eAmount;
            dr4["Amount"] = nAmount;
            NetProfit.Rows.Add(dr4);

            var grossAmount = rAmount - cAmount;
            var grossPercent = rAmount == 0 ? 0 : grossAmount / rAmount;

            var netAmount = rAmount - cAmount - eAmount;
            var netPercent = rAmount == 0 ? 0 : netAmount / rAmount;

            var incomeBeforeAmount = rAmount - cAmount - eAmount + oAmount;
            var incomeBeforePercent = rAmount == 0 ? 0 : incomeBeforeAmount / rAmount;

            var lastNetAmount = rAmount - cAmount - eAmount + oAmount - iAmount;
            var lastNetPercent = rAmount == 0 ? 0 : lastNetAmount / rAmount;

            string netText = string.Empty;
            bool showLastNet = false;

            if (OtherIncome.Rows.Count > 0 || IncomeTaxes.Rows.Count > 0)
            {
                netText = "Income From Operations";
                showLastNet = true;
            }
            else
            {
                netText = "Net ";

                if (Convert.ToDouble(netAmount) < 0)
                {
                    netText += "Loss";
                }
                else
                {
                    netText += "Income";
                }
            }

            report["TotalRevenue"] = rAmount;
            report["paramUsername"] = Session["Username"].ToString();
            report["paramSDate"] = _FromDate;
            report["paramEDate"] = _ToDate;
            report["paramRev"] = rAmount;
            report["paramCPercent"] = cPercent;
            report["paramEPercent"] = ePercent;
            report["paramOPercent"] = oPercent;
            report["paramIPercent"] = iPercent;
            report["paramGrossAmount"] = grossAmount;
            report["paramGrossPercent"] = grossPercent;
            report["paramNetAmount"] = netAmount;
            report["paramNetPercent"] = netPercent;
            report["paramIncomeBeforeAmount"] = incomeBeforeAmount;
            report["paramIncomeBeforePercent"] = incomeBeforePercent;
            report["paramLastNetAmount"] = lastNetAmount;
            report["paramLastNetPercent"] = lastNetPercent;
            report["paramNetText"] = netText;
            report["paramShowLastNet"] = showLastNet;

            report.RegData("Revenues", Revenues);
            report.RegData("TRevenues", TRevenues);
            report.RegData("CostOfSales", CostOfSales);
            report.RegData("TCostOfSales", TCostOfSales);
            report.RegData("Expenses", Expenses);
            report.RegData("TExpenses", TExpenses);
            report.RegData("OtherIncome", OtherIncome);
            report.RegData("TOtherIncome", TOtherIncome);
            report.RegData("IncomeTaxes", IncomeTaxes);
            report.RegData("TIncomeTaxes", TIncomeTaxes);
            report.RegData("GrossProfit", GrossProfit);
            report.RegData("NetProfit", NetProfit);
            report.RegData("dsInome", _dsIncome.Tables[0]);
            report.RegData("dsCompany", dsC.Tables[0]);
            report.Render();

            return report;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return report;
    }

    private StiReport GetIncomeStatementSummaryReport()
    {
        StiReport report = new StiReport();
        try
        {
            string reportPath = Server.MapPath("StimulsoftReports/IncomeStatementSummary.mrt");
            report.Load(reportPath);
            report.Compile();

            _objChart.ConnConfig = Session["config"].ToString();

            #region Start-End date

            if (!string.IsNullOrEmpty(Request.QueryString["startDate"]))
            {
                _objChart.StartDate = Convert.ToDateTime(Request.QueryString["startDate"]);
            }
            else
            {
                int year = DateTime.Now.Year;
                _objChart.StartDate = new DateTime(year, 1, 1);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["endDate"]))
            {
                _objChart.EndDate = Convert.ToDateTime(Request.QueryString["endDate"]);
            }
            else
            {
                _objChart.EndDate = DateTime.Now.Date;
            }

            #endregion

            string _FromDate = "From : " + _objChart.StartDate.ToString("MMMM dd, yyyy");
            string _ToDate = "To :    " + _objChart.EndDate.ToString("MMMM dd, yyyy");

            #region Set Header

            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objPropUser);

            #endregion

            DataSet _dsIncome = _objBLReport.GetIncomeStatementSummary(_objChart);

            // Revenues
            var dTable = _dsIncome.Tables[0];
            var dView = dTable.DefaultView;
            dView.RowFilter = "Type = 3";
            DataTable Revenues = dView.ToTable();
            Revenues.TableName = "Revenues";

            DataTable TRevenues = Revenues.Clone();
            TRevenues.TableName = "TRevenues";
            var dr = TRevenues.NewRow();
            dr["Type"] = 3;
            dr["TypeName"] = "Revenues";
            dr["Sub"] = "";
            double rAmount = 0.00;
            for (int i = 0; i < Revenues.Rows.Count; i++)
            {
                rAmount += double.Parse(Revenues.Rows[i]["Amount"].ToString());
            }
            dr["Amount"] = rAmount;
            TRevenues.Rows.Add(dr.ItemArray);

            // Cost Of Sales
            dTable = _dsIncome.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "Type = 4";
            DataTable CostOfSales = dView.ToTable();
            CostOfSales.TableName = "CostOfSales";

            DataTable TCostOfSales = CostOfSales.Clone();
            TCostOfSales.TableName = "TCostOfSales";
            var dr1 = TCostOfSales.NewRow();
            dr1["Type"] = 4;
            dr1["TypeName"] = "Cost Of Sales";
            dr1["Sub"] = "";
            double cAmount = 0.00;
            for (int i = 0; i < CostOfSales.Rows.Count; i++)
            {
                cAmount += double.Parse(CostOfSales.Rows[i]["Amount"].ToString());
            }
            dr1["Amount"] = cAmount;
            TCostOfSales.Rows.Add(dr1.ItemArray);
            var cPercent = cAmount / rAmount;

            // Expenses
            dTable = _dsIncome.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "Type = 5";
            DataTable Expenses = dView.ToTable();
            Expenses.TableName = "Expenses";

            DataTable TExpenses = Expenses.Clone();
            TExpenses.TableName = "TExpenses";
            var dr2 = TExpenses.NewRow();
            dr2["Type"] = 5;
            dr2["TypeName"] = "Expenses";
            dr2["Sub"] = "";
            double eAmount = 0.00;
            for (int i = 0; i < Expenses.Rows.Count; i++)
            {
                eAmount += double.Parse(Expenses.Rows[i]["Amount"].ToString());
            }

            dr2["Amount"] = eAmount;
            TExpenses.Rows.Add(dr2.ItemArray);
            var ePercent = eAmount / rAmount;

            // Total Other Income (Expenses)
            dTable = _dsIncome.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "Type = 8";
            DataTable OtherIncome = dView.ToTable();
            OtherIncome.TableName = "OtherIncome";

            DataTable TOtherIncome = OtherIncome.Clone();
            TOtherIncome.TableName = "TOtherIncome";
            var dr8 = TOtherIncome.NewRow();
            dr8["Type"] = 8;
            dr8["TypeName"] = "Other Income (Expenses)";
            dr8["Sub"] = "";

            double oAmount = 0.00;
            for (int i = 0; i < OtherIncome.Rows.Count; i++)
            {
                oAmount += double.Parse(OtherIncome.Rows[i]["Amount"].ToString());
            }

            dr8["Amount"] = oAmount;
            TExpenses.Rows.Add(dr8.ItemArray);
            var oPercent = rAmount == 0 ? 0 : oAmount / rAmount;

            // Total Provisions for Income Taxes
            dTable = _dsIncome.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "Type = 9";
            DataTable IncomeTaxes = dView.ToTable();
            IncomeTaxes.TableName = "IncomeTaxes";

            DataTable TIncomeTaxes = IncomeTaxes.Clone();
            IncomeTaxes.TableName = "TIncomeTaxes";
            var dr9 = IncomeTaxes.NewRow();
            dr9["Type"] = 9;
            dr9["TypeName"] = "Provisions for Income Taxes";
            dr9["Sub"] = "";

            double iAmount = 0.00;
            for (int i = 0; i < IncomeTaxes.Rows.Count; i++)
            {
                iAmount += double.Parse(IncomeTaxes.Rows[i]["Amount"].ToString());
            }

            dr9["Amount"] = iAmount;
            TIncomeTaxes.Rows.Add(dr9.ItemArray);
            var iPercent = rAmount == 0 ? 0 : iAmount / rAmount;

            // Gross Profit
            DataTable GrossProfit = Expenses.Clone();
            GrossProfit.TableName = "GrossProfit";
            var dr3 = GrossProfit.NewRow();
            dr3["Type"] = 4.5;
            dr3["TypeName"] = "GrossProfit";
            dr3["Sub"] = "";
            double gAmount = rAmount + cAmount;
            dr3["Amount"] = gAmount;
            GrossProfit.Rows.Add(dr3);

            DataTable NetProfit = Expenses.Clone();
            NetProfit.TableName = "GrossProfit";
            var dr4 = NetProfit.NewRow();
            dr4["Type"] = 6;
            dr4["TypeName"] = "Net Profit";
            dr4["Sub"] = "";
            double nAmount = gAmount - eAmount;
            dr4["Amount"] = nAmount;
            NetProfit.Rows.Add(dr4);

            var grossAmount = rAmount - cAmount;
            var grossPercent = rAmount == 0 ? 0 : grossAmount / rAmount;

            var netAmount = rAmount - cAmount - eAmount;
            var netPercent = rAmount == 0 ? 0 : netAmount / rAmount;

            var incomeBeforeAmount = rAmount - cAmount - eAmount + oAmount;
            var incomeBeforePercent = rAmount == 0 ? 0 : incomeBeforeAmount / rAmount;

            var lastNetAmount = rAmount - cAmount - eAmount + oAmount - iAmount;
            var lastNetPercent = rAmount == 0 ? 0 : lastNetAmount / rAmount;

            string netText = string.Empty;
            bool showLastNet = false;

            if (OtherIncome.Rows.Count > 0 || IncomeTaxes.Rows.Count > 0)
            {
                netText = "Income From Operations";
                showLastNet = true;
            }
            else
            {
                netText = "Net ";

                if (Convert.ToDouble(netAmount) < 0)
                {
                    netText += "Loss";
                }
                else
                {
                    netText += "Income";
                }
            }

            report["TotalRevenue"] = rAmount;
            report["paramUsername"] = Session["Username"].ToString();
            report["paramSDate"] = _FromDate;
            report["paramEDate"] = _ToDate;
            report["paramRev"] = rAmount;
            report["paramCPercent"] = cPercent;
            report["paramEPercent"] = ePercent;
            report["paramOPercent"] = oPercent;
            report["paramIPercent"] = iPercent;
            report["paramGrossAmount"] = grossAmount;
            report["paramGrossPercent"] = grossPercent;
            report["paramNetAmount"] = netAmount;
            report["paramNetPercent"] = netPercent;
            report["paramIncomeBeforeAmount"] = incomeBeforeAmount;
            report["paramIncomeBeforePercent"] = incomeBeforePercent;
            report["paramLastNetAmount"] = lastNetAmount;
            report["paramLastNetPercent"] = lastNetPercent;
            report["paramNetText"] = netText;
            report["paramShowLastNet"] = showLastNet;

            report.RegData("Revenues", Revenues);
            report.RegData("TRevenues", TRevenues);
            report.RegData("CostOfSales", CostOfSales);
            report.RegData("TCostOfSales", TCostOfSales);
            report.RegData("Expenses", Expenses);
            report.RegData("TExpenses", TExpenses);
            report.RegData("OtherIncome", OtherIncome);
            report.RegData("TOtherIncome", TOtherIncome);
            report.RegData("IncomeTaxes", IncomeTaxes);
            report.RegData("TIncomeTaxes", TIncomeTaxes);
            report.RegData("GrossProfit", GrossProfit);
            report.RegData("NetProfit", NetProfit);
            report.RegData("dsInome", _dsIncome.Tables[0]);
            report.RegData("dsCompany", dsC.Tables[0]);
            report.Render();

            return report;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return report;
    }

    protected void StiWebViewerIncomeStatemnet_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        try
        {
            StiReport report = new StiReport();

            if (!string.IsNullOrEmpty(Request.QueryString["cType"]) && Request.QueryString["cType"] == "summary")
            {
                report = GetIncomeStatementSummaryReport();
            }
            else
            {
                report = GetIncomeStatementReport();
            }

            e.Report = report;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void StiWebViewerIncomeStatemnet_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {

    }

    #endregion

    #region 12 Period Report (reportType = 1)

    private StiReport Get12PeriodIncomeStatement()
    {
        StiReport report = new StiReport();
        try
        {
            string reportPath = Server.MapPath("StimulsoftReports/IncomeStatement12Period.mrt");
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["12PeriodReport"]))
            {
                reportPath = Server.MapPath("StimulsoftReports/" + ConfigurationManager.AppSettings["12PeriodReport"].ToString());
            }

            if (!string.IsNullOrEmpty(Request.QueryString["cType"]))
            {
                if (Request.QueryString["cType"] == "sub")
                {
                    reportPath = Server.MapPath("StimulsoftReports/IncomeStatement12PeriodWithSub.mrt");
                }
                else if (Request.QueryString["cType"] == "summary")
                {
                    reportPath = Server.MapPath("StimulsoftReports/IncomeStatement12PeriodSummary.mrt");
                }
            }

            report.Load(reportPath);
            report.Compile();

            if (!string.IsNullOrEmpty(Request.QueryString["yearEndingDate"]))
            {
                _objChart.EndDate = Convert.ToDateTime(Request.QueryString["yearEndingDate"].ToString());
            }
            else
            {
                _objChart.EndDate = new DateTime(DateTime.Now.Year, 1, 1).AddSeconds(12).AddDays(-1);
            }

            _objChart.StartDate = _objChart.EndDate.AddYears(-1).AddDays(1);
            _objChart.ConnConfig = Session["config"].ToString();

            DataSet ds = _objBLReport.GetIncomeStatement12Period(_objChart);

            if (Request.QueryString["includeZero"] != null && !Convert.ToBoolean(Request.QueryString["includeZero"]))
            {
                DataRow[] drr = ds.Tables[0].Select("NTotal = 0");
                foreach (DataRow row in drr)
                {
                    row.Delete();
                }

                ds.Tables[0].AcceptChanges();
            }

            ds.Tables[0].AsEnumerable().ToList()
              .ForEach(b => b["Url"] = (Request.Url.Scheme +
                                          (Uri.SchemeDelimiter +
                                              (Request.Url.Authority +
                                                  (Request.ApplicationPath + "/accountledger.aspx?id=" + b["Acct"].ToString() + "&s=" + System.Web.HttpUtility.UrlEncode(_objChart.StartDate.ToShortDateString()).ToString()
                                                                                                                                      + "&e=" + System.Web.HttpUtility.UrlEncode(_objChart.EndDate.ToShortDateString()).ToString()
                                                  )
                                              )
                                          )
                                       )
                                   );

            ds.Tables[0].AcceptChanges();

            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objPropUser);

            ReportParameter rpUser = new ReportParameter("paramUsername", Session["Username"].ToString());
            ReportParameter rpEDate = new ReportParameter("paramEDate", _objChart.EndDate.ToShortDateString());
            var finalds = ProcessAndBuildDataForIncomeStatemnt(ds);

            report["paramUsername"] = Session["Username"].ToString();
            report["paramEDate"] = Convert.ToDateTime(_objChart.EndDate).ToString("MMMM dd, yyyy");

            // Revenues
            var dTable = finalds.Tables[0];
            var dView = dTable.DefaultView;
            dView.RowFilter = "Type = 3";
            DataTable Revenues = dView.ToTable();
            Revenues.TableName = "Revenues";

            DataTable TRevenues = Revenues.Clone();
            TRevenues.TableName = "TRevenues";
            var dr = TRevenues.NewRow();
            dr["Acct"] = 1000;
            dr["Type"] = 3;
            dr["TypeName"] = "Revenues";
            dr["fDesc"] = "Total Revenues";

            double jan = 0.00, feb = 0.00, mar = 0.00, apr = 0.00, may = 0.00, jun = 0.00, jul = 0.00, aug = 0.00, sep = 0.00, oct = 0.00, nov = 0.00, dec = 0.00, total = 0.00;
            for (int i = 0; i < Revenues.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(Revenues.Rows[i][setMonth(1, false)].ToString()))
                    jan += double.Parse(Revenues.Rows[i][setMonth(1, false)].ToString());
                if (!string.IsNullOrEmpty(Revenues.Rows[i][setMonth(2, false)].ToString()))
                    feb += double.Parse(Revenues.Rows[i][setMonth(2, false)].ToString());
                if (!string.IsNullOrEmpty(Revenues.Rows[i][setMonth(3, false)].ToString()))
                    mar += double.Parse(Revenues.Rows[i][setMonth(3, false)].ToString());
                if (!string.IsNullOrEmpty(Revenues.Rows[i][setMonth(4, false)].ToString()))
                    apr += double.Parse(Revenues.Rows[i][setMonth(4, false)].ToString());
                if (!string.IsNullOrEmpty(Revenues.Rows[i][setMonth(5, false)].ToString()))
                    may += double.Parse(Revenues.Rows[i][setMonth(5, false)].ToString());
                if (!string.IsNullOrEmpty(Revenues.Rows[i][setMonth(6, false)].ToString()))
                    jun += double.Parse(Revenues.Rows[i][setMonth(6, false)].ToString());
                if (!string.IsNullOrEmpty(Revenues.Rows[i][setMonth(7, false)].ToString()))
                    jul += double.Parse(Revenues.Rows[i][setMonth(7, false)].ToString());
                if (!string.IsNullOrEmpty(Revenues.Rows[i][setMonth(8, false)].ToString()))
                    aug += double.Parse(Revenues.Rows[i][setMonth(8, false)].ToString());
                if (!string.IsNullOrEmpty(Revenues.Rows[i][setMonth(9, false)].ToString()))
                    sep += double.Parse(Revenues.Rows[i][setMonth(9, false)].ToString());
                if (!string.IsNullOrEmpty(Revenues.Rows[i][setMonth(10, false)].ToString()))
                    oct += double.Parse(Revenues.Rows[i][setMonth(10, false)].ToString());
                if (!string.IsNullOrEmpty(Revenues.Rows[i][setMonth(11, false)].ToString()))
                    nov += double.Parse(Revenues.Rows[i][setMonth(11, false)].ToString());
                if (!string.IsNullOrEmpty(Revenues.Rows[i][setMonth(12, false)].ToString()))
                    dec += double.Parse(Revenues.Rows[i][setMonth(12, false)].ToString());
                if (!string.IsNullOrEmpty(Revenues.Rows[i]["Total"].ToString()))
                    total += double.Parse(Revenues.Rows[i]["Total"].ToString());
            }

            dr[setMonth(1, false)] = jan;
            dr[setMonth(2, false)] = feb;
            dr[setMonth(3, false)] = mar;
            dr[setMonth(4, false)] = apr;
            dr[setMonth(5, false)] = may;
            dr[setMonth(6, false)] = jun;
            dr[setMonth(7, false)] = jul;
            dr[setMonth(8, false)] = aug;
            dr[setMonth(9, false)] = sep;
            dr[setMonth(10, false)] = oct;
            dr[setMonth(11, false)] = nov;
            dr[setMonth(12, false)] = dec;
            dr["Total"] = total;
            TRevenues.Rows.Add(dr.ItemArray);

            // Cost Of Sales
            dTable = finalds.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "Type = 4";
            DataTable CostOfSales = dView.ToTable();
            CostOfSales.TableName = "CostOfSales";

            DataTable TCostofSales = CostOfSales.Clone();
            TCostofSales.TableName = "TCostOfSales";
            var dr1 = CostOfSales.NewRow();
            dr1["Acct"] = 1000;
            dr1["Type"] = 4;
            dr1["TypeName"] = "Cost of Sales";
            dr1["fDesc"] = "Total Cost of Sales";

            jan = feb = mar = apr = may = jun = jul = aug = sep = oct = nov = dec = total = 0.00;
            for (int i = 0; i < CostOfSales.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(CostOfSales.Rows[i][setMonth(1, false)].ToString()))
                    jan += double.Parse(CostOfSales.Rows[i][setMonth(1, false)].ToString());
                if (!string.IsNullOrEmpty(CostOfSales.Rows[i][setMonth(2, false)].ToString()))
                    feb += double.Parse(CostOfSales.Rows[i][setMonth(2, false)].ToString());
                if (!string.IsNullOrEmpty(CostOfSales.Rows[i][setMonth(3, false)].ToString()))
                    mar += double.Parse(CostOfSales.Rows[i][setMonth(3, false)].ToString());
                if (!string.IsNullOrEmpty(CostOfSales.Rows[i][setMonth(4, false)].ToString()))
                    apr += double.Parse(CostOfSales.Rows[i][setMonth(4, false)].ToString());
                if (!string.IsNullOrEmpty(CostOfSales.Rows[i][setMonth(5, false)].ToString()))
                    may += double.Parse(CostOfSales.Rows[i][setMonth(5, false)].ToString());
                if (!string.IsNullOrEmpty(CostOfSales.Rows[i][setMonth(6, false)].ToString()))
                    jun += double.Parse(CostOfSales.Rows[i][setMonth(6, false)].ToString());
                if (!string.IsNullOrEmpty(CostOfSales.Rows[i][setMonth(7, false)].ToString()))
                    jul += double.Parse(CostOfSales.Rows[i][setMonth(7, false)].ToString());
                if (!string.IsNullOrEmpty(CostOfSales.Rows[i][setMonth(8, false)].ToString()))
                    aug += double.Parse(CostOfSales.Rows[i][setMonth(8, false)].ToString());
                if (!string.IsNullOrEmpty(CostOfSales.Rows[i][setMonth(9, false)].ToString()))
                    sep += double.Parse(CostOfSales.Rows[i][setMonth(9, false)].ToString());
                if (!string.IsNullOrEmpty(CostOfSales.Rows[i][setMonth(10, false)].ToString()))
                    oct += double.Parse(CostOfSales.Rows[i][setMonth(10, false)].ToString());
                if (!string.IsNullOrEmpty(CostOfSales.Rows[i][setMonth(11, false)].ToString()))
                    nov += double.Parse(CostOfSales.Rows[i][setMonth(11, false)].ToString());
                if (!string.IsNullOrEmpty(CostOfSales.Rows[i][setMonth(12, false)].ToString()))
                    dec += double.Parse(CostOfSales.Rows[i][setMonth(12, false)].ToString());
                if (!string.IsNullOrEmpty(CostOfSales.Rows[i]["Total"].ToString()))
                    total += double.Parse(CostOfSales.Rows[i]["Total"].ToString());
            }
            dr1[setMonth(1, false)] = jan;
            dr1[setMonth(2, false)] = feb;
            dr1[setMonth(3, false)] = mar;
            dr1[setMonth(4, false)] = apr;
            dr1[setMonth(5, false)] = may;
            dr1[setMonth(6, false)] = jun;
            dr1[setMonth(7, false)] = jul;
            dr1[setMonth(8, false)] = aug;
            dr1[setMonth(9, false)] = sep;
            dr1[setMonth(10, false)] = oct;
            dr1[setMonth(11, false)] = nov;
            dr1[setMonth(12, false)] = dec;
            dr1["Total"] = total;
            TCostofSales.Rows.Add(dr1.ItemArray);

            // Expenses
            dTable = finalds.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "Type = 5";
            DataTable Expenses = dView.ToTable();
            Expenses.TableName = "Expenses";

            DataTable TExpenses = Expenses.Clone();
            TExpenses.TableName = "TExpenses";
            var dr3 = Expenses.NewRow();
            dr3["Acct"] = 1000;
            dr3["Type"] = 5;
            dr3["TypeName"] = "Expenses";
            dr3["fDesc"] = "Total Expenses";

            jan = feb = mar = apr = may = jun = jul = aug = sep = oct = nov = dec = total = 0.00;
            for (int i = 0; i < Expenses.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(Expenses.Rows[i][setMonth(1, false)].ToString()))
                    jan += double.Parse(Expenses.Rows[i][setMonth(1, false)].ToString());
                if (!string.IsNullOrEmpty(Expenses.Rows[i][setMonth(2, false)].ToString()))
                    feb += double.Parse(Expenses.Rows[i][setMonth(2, false)].ToString());
                if (!string.IsNullOrEmpty(Expenses.Rows[i][setMonth(3, false)].ToString()))
                    mar += double.Parse(Expenses.Rows[i][setMonth(3, false)].ToString());
                if (!string.IsNullOrEmpty(Expenses.Rows[i][setMonth(4, false)].ToString()))
                    apr += double.Parse(Expenses.Rows[i][setMonth(4, false)].ToString());
                if (!string.IsNullOrEmpty(Expenses.Rows[i][setMonth(5, false)].ToString()))
                    may += double.Parse(Expenses.Rows[i][setMonth(5, false)].ToString());
                if (!string.IsNullOrEmpty(Expenses.Rows[i][setMonth(6, false)].ToString()))
                    jun += double.Parse(Expenses.Rows[i][setMonth(6, false)].ToString());
                if (!string.IsNullOrEmpty(Expenses.Rows[i][setMonth(7, false)].ToString()))
                    jul += double.Parse(Expenses.Rows[i][setMonth(7, false)].ToString());
                if (!string.IsNullOrEmpty(Expenses.Rows[i][setMonth(8, false)].ToString()))
                    aug += double.Parse(Expenses.Rows[i][setMonth(8, false)].ToString());
                if (!string.IsNullOrEmpty(Expenses.Rows[i][setMonth(9, false)].ToString()))
                    sep += double.Parse(Expenses.Rows[i][setMonth(9, false)].ToString());
                if (!string.IsNullOrEmpty(Expenses.Rows[i][setMonth(10, false)].ToString()))
                    oct += double.Parse(Expenses.Rows[i][setMonth(10, false)].ToString());
                if (!string.IsNullOrEmpty(Expenses.Rows[i][setMonth(11, false)].ToString()))
                    nov += double.Parse(Expenses.Rows[i][setMonth(11, false)].ToString());
                if (!string.IsNullOrEmpty(Expenses.Rows[i][setMonth(12, false)].ToString()))
                    dec += double.Parse(Expenses.Rows[i][setMonth(12, false)].ToString());
                if (!string.IsNullOrEmpty(Expenses.Rows[i]["Total"].ToString()))
                    total += double.Parse(Expenses.Rows[i]["Total"].ToString());
            }
            dr3[setMonth(1, false)] = jan;
            dr3[setMonth(2, false)] = feb;
            dr3[setMonth(3, false)] = mar;
            dr3[setMonth(4, false)] = apr;
            dr3[setMonth(5, false)] = may;
            dr3[setMonth(6, false)] = jun;
            dr3[setMonth(7, false)] = jul;
            dr3[setMonth(8, false)] = aug;
            dr3[setMonth(9, false)] = sep;
            dr3[setMonth(10, false)] = oct;
            dr3[setMonth(11, false)] = nov;
            dr3[setMonth(12, false)] = dec;
            dr3["Total"] = total;
            TExpenses.Rows.Add(dr3.ItemArray);

            // Other Income (Expenses)
            dTable = finalds.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "Type = 8";
            DataTable OtherIncome = dView.ToTable();
            OtherIncome.TableName = "OtherIncome";

            DataTable TOtherIncome = OtherIncome.Clone();
            TOtherIncome.TableName = "TOtherIncome";
            var dr8 = OtherIncome.NewRow();
            dr8["Acct"] = 8000;
            dr8["Type"] = 8;
            dr8["TypeName"] = "Other Income (Expenses)";
            dr8["fDesc"] = "Total Other Income (Expenses)";

            jan = feb = mar = apr = may = jun = jul = aug = sep = oct = nov = dec = total = 0.00;
            for (int i = 0; i < OtherIncome.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(OtherIncome.Rows[i][setMonth(1, false)].ToString()))
                    jan += double.Parse(OtherIncome.Rows[i][setMonth(1, false)].ToString());
                if (!string.IsNullOrEmpty(OtherIncome.Rows[i][setMonth(2, false)].ToString()))
                    feb += double.Parse(OtherIncome.Rows[i][setMonth(2, false)].ToString());
                if (!string.IsNullOrEmpty(OtherIncome.Rows[i][setMonth(3, false)].ToString()))
                    mar += double.Parse(OtherIncome.Rows[i][setMonth(3, false)].ToString());
                if (!string.IsNullOrEmpty(OtherIncome.Rows[i][setMonth(4, false)].ToString()))
                    apr += double.Parse(OtherIncome.Rows[i][setMonth(4, false)].ToString());
                if (!string.IsNullOrEmpty(OtherIncome.Rows[i][setMonth(5, false)].ToString()))
                    may += double.Parse(OtherIncome.Rows[i][setMonth(5, false)].ToString());
                if (!string.IsNullOrEmpty(OtherIncome.Rows[i][setMonth(6, false)].ToString()))
                    jun += double.Parse(OtherIncome.Rows[i][setMonth(6, false)].ToString());
                if (!string.IsNullOrEmpty(OtherIncome.Rows[i][setMonth(7, false)].ToString()))
                    jul += double.Parse(OtherIncome.Rows[i][setMonth(7, false)].ToString());
                if (!string.IsNullOrEmpty(OtherIncome.Rows[i][setMonth(8, false)].ToString()))
                    aug += double.Parse(OtherIncome.Rows[i][setMonth(8, false)].ToString());
                if (!string.IsNullOrEmpty(OtherIncome.Rows[i][setMonth(9, false)].ToString()))
                    sep += double.Parse(OtherIncome.Rows[i][setMonth(9, false)].ToString());
                if (!string.IsNullOrEmpty(OtherIncome.Rows[i][setMonth(10, false)].ToString()))
                    oct += double.Parse(OtherIncome.Rows[i][setMonth(10, false)].ToString());
                if (!string.IsNullOrEmpty(OtherIncome.Rows[i][setMonth(11, false)].ToString()))
                    nov += double.Parse(OtherIncome.Rows[i][setMonth(11, false)].ToString());
                if (!string.IsNullOrEmpty(OtherIncome.Rows[i][setMonth(12, false)].ToString()))
                    dec += double.Parse(OtherIncome.Rows[i][setMonth(12, false)].ToString());
                if (!string.IsNullOrEmpty(OtherIncome.Rows[i]["Total"].ToString()))
                    total += double.Parse(OtherIncome.Rows[i]["Total"].ToString());
            }
            dr8[setMonth(1, false)] = jan;
            dr8[setMonth(2, false)] = feb;
            dr8[setMonth(3, false)] = mar;
            dr8[setMonth(4, false)] = apr;
            dr8[setMonth(5, false)] = may;
            dr8[setMonth(6, false)] = jun;
            dr8[setMonth(7, false)] = jul;
            dr8[setMonth(8, false)] = aug;
            dr8[setMonth(9, false)] = sep;
            dr8[setMonth(10, false)] = oct;
            dr8[setMonth(11, false)] = nov;
            dr8[setMonth(12, false)] = dec;
            dr8["Total"] = total;
            TOtherIncome.Rows.Add(dr8.ItemArray);

            // Provisions for Income Taxes
            dTable = finalds.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "Type = 9";
            DataTable IncomeTaxes = dView.ToTable();
            IncomeTaxes.TableName = "IncomeTaxes";

            DataTable TIncomeTaxes = IncomeTaxes.Clone();
            TIncomeTaxes.TableName = "TIncomeTaxes";
            var dr9 = IncomeTaxes.NewRow();
            dr9["Acct"] = 9000;
            dr9["Type"] = 9;
            dr9["TypeName"] = "Provisions for Income Taxes";
            dr9["fDesc"] = "Total Provisions for Income Taxes";

            jan = feb = mar = apr = may = jun = jul = aug = sep = oct = nov = dec = total = 0.00;
            for (int i = 0; i < IncomeTaxes.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(IncomeTaxes.Rows[i][setMonth(1, false)].ToString()))
                    jan += double.Parse(IncomeTaxes.Rows[i][setMonth(1, false)].ToString());
                if (!string.IsNullOrEmpty(IncomeTaxes.Rows[i][setMonth(2, false)].ToString()))
                    feb += double.Parse(IncomeTaxes.Rows[i][setMonth(2, false)].ToString());
                if (!string.IsNullOrEmpty(IncomeTaxes.Rows[i][setMonth(3, false)].ToString()))
                    mar += double.Parse(IncomeTaxes.Rows[i][setMonth(3, false)].ToString());
                if (!string.IsNullOrEmpty(IncomeTaxes.Rows[i][setMonth(4, false)].ToString()))
                    apr += double.Parse(IncomeTaxes.Rows[i][setMonth(4, false)].ToString());
                if (!string.IsNullOrEmpty(IncomeTaxes.Rows[i][setMonth(5, false)].ToString()))
                    may += double.Parse(IncomeTaxes.Rows[i][setMonth(5, false)].ToString());
                if (!string.IsNullOrEmpty(IncomeTaxes.Rows[i][setMonth(6, false)].ToString()))
                    jun += double.Parse(IncomeTaxes.Rows[i][setMonth(6, false)].ToString());
                if (!string.IsNullOrEmpty(IncomeTaxes.Rows[i][setMonth(7, false)].ToString()))
                    jul += double.Parse(IncomeTaxes.Rows[i][setMonth(7, false)].ToString());
                if (!string.IsNullOrEmpty(IncomeTaxes.Rows[i][setMonth(8, false)].ToString()))
                    aug += double.Parse(IncomeTaxes.Rows[i][setMonth(8, false)].ToString());
                if (!string.IsNullOrEmpty(IncomeTaxes.Rows[i][setMonth(9, false)].ToString()))
                    sep += double.Parse(IncomeTaxes.Rows[i][setMonth(9, false)].ToString());
                if (!string.IsNullOrEmpty(IncomeTaxes.Rows[i][setMonth(10, false)].ToString()))
                    oct += double.Parse(IncomeTaxes.Rows[i][setMonth(10, false)].ToString());
                if (!string.IsNullOrEmpty(IncomeTaxes.Rows[i][setMonth(11, false)].ToString()))
                    nov += double.Parse(IncomeTaxes.Rows[i][setMonth(11, false)].ToString());
                if (!string.IsNullOrEmpty(IncomeTaxes.Rows[i][setMonth(12, false)].ToString()))
                    dec += double.Parse(IncomeTaxes.Rows[i][setMonth(12, false)].ToString());
                if (!string.IsNullOrEmpty(IncomeTaxes.Rows[i]["Total"].ToString()))
                    total += double.Parse(IncomeTaxes.Rows[i]["Total"].ToString());
            }
            dr9[setMonth(1, false)] = jan;
            dr9[setMonth(2, false)] = feb;
            dr9[setMonth(3, false)] = mar;
            dr9[setMonth(4, false)] = apr;
            dr9[setMonth(5, false)] = may;
            dr9[setMonth(6, false)] = jun;
            dr9[setMonth(7, false)] = jul;
            dr9[setMonth(8, false)] = aug;
            dr9[setMonth(9, false)] = sep;
            dr9[setMonth(10, false)] = oct;
            dr9[setMonth(11, false)] = nov;
            dr9[setMonth(12, false)] = dec;
            dr9["Total"] = total;
            TIncomeTaxes.Rows.Add(dr9.ItemArray);

            // Gross Profit
            DataTable GrossProfit = Expenses.Clone();
            GrossProfit.TableName = "GrossTotal";
            var dr4 = Expenses.NewRow();
            dr4["Acct"] = 1000;
            dr4["Type"] = 4.5;
            dr4["TypeName"] = "GrossTotal";
            dr4["fDesc"] = "Gross Profit";

            jan = feb = mar = apr = may = jun = jul = aug = sep = oct = nov = dec = total = 0.00;
            jan += double.Parse(TRevenues.Rows[0][setMonth(1, false)].ToString()) + double.Parse(TCostofSales.Rows[0][setMonth(1, false)].ToString());
            feb += double.Parse(TRevenues.Rows[0][setMonth(2, false)].ToString()) + double.Parse(TCostofSales.Rows[0][setMonth(2, false)].ToString());
            mar += double.Parse(TRevenues.Rows[0][setMonth(3, false)].ToString()) + double.Parse(TCostofSales.Rows[0][setMonth(3, false)].ToString());
            apr += double.Parse(TRevenues.Rows[0][setMonth(4, false)].ToString()) + double.Parse(TCostofSales.Rows[0][setMonth(4, false)].ToString());
            may += double.Parse(TRevenues.Rows[0][setMonth(5, false)].ToString()) + double.Parse(TCostofSales.Rows[0][setMonth(5, false)].ToString());
            jun += double.Parse(TRevenues.Rows[0][setMonth(6, false)].ToString()) + double.Parse(TCostofSales.Rows[0][setMonth(6, false)].ToString());
            jul += double.Parse(TRevenues.Rows[0][setMonth(7, false)].ToString()) + double.Parse(TCostofSales.Rows[0][setMonth(7, false)].ToString());
            aug += double.Parse(TRevenues.Rows[0][setMonth(8, false)].ToString()) + double.Parse(TCostofSales.Rows[0][setMonth(8, false)].ToString());
            sep += double.Parse(TRevenues.Rows[0][setMonth(9, false)].ToString()) + double.Parse(TCostofSales.Rows[0][setMonth(9, false)].ToString());
            oct += double.Parse(TRevenues.Rows[0][setMonth(10, false)].ToString()) + double.Parse(TCostofSales.Rows[0][setMonth(10, false)].ToString());
            nov += double.Parse(TRevenues.Rows[0][setMonth(11, false)].ToString()) + double.Parse(TCostofSales.Rows[0][setMonth(11, false)].ToString());
            dec += double.Parse(TRevenues.Rows[0][setMonth(12, false)].ToString()) + double.Parse(TCostofSales.Rows[0][setMonth(12, false)].ToString());
            total += double.Parse(TRevenues.Rows[0]["Total"].ToString()) + double.Parse(TCostofSales.Rows[0]["Total"].ToString());

            dr4[setMonth(1, false)] = jan;
            dr4[setMonth(2, false)] = feb;
            dr4[setMonth(3, false)] = mar;
            dr4[setMonth(4, false)] = apr;
            dr4[setMonth(5, false)] = may;
            dr4[setMonth(6, false)] = jun;
            dr4[setMonth(7, false)] = jul;
            dr4[setMonth(8, false)] = aug;
            dr4[setMonth(9, false)] = sep;
            dr4[setMonth(10, false)] = oct;
            dr4[setMonth(11, false)] = nov;
            dr4[setMonth(12, false)] = dec;
            dr4["Total"] = total;
            GrossProfit.Rows.Add(dr4.ItemArray);

            // Net Profit
            DataTable NetProfit = Expenses.Clone();
            NetProfit.TableName = "NetProfit";
            var dr5 = Expenses.NewRow();
            dr5["Acct"] = 1000;
            dr5["Type"] = 6;
            dr5["TypeName"] = "NetProfit";
            dr5["fDesc"] = "Net Profit";

            jan = feb = mar = apr = may = jun = jul = aug = sep = oct = nov = dec = total = 0.00;
            jan += double.Parse(GrossProfit.Rows[0][setMonth(1, false)].ToString()) - double.Parse(TExpenses.Rows[0][setMonth(1, false)].ToString());
            feb += double.Parse(GrossProfit.Rows[0][setMonth(2, false)].ToString()) - double.Parse(TExpenses.Rows[0][setMonth(2, false)].ToString());
            mar += double.Parse(GrossProfit.Rows[0][setMonth(3, false)].ToString()) - double.Parse(TExpenses.Rows[0][setMonth(3, false)].ToString());
            apr += double.Parse(GrossProfit.Rows[0][setMonth(4, false)].ToString()) - double.Parse(TExpenses.Rows[0][setMonth(4, false)].ToString());
            may += double.Parse(GrossProfit.Rows[0][setMonth(5, false)].ToString()) - double.Parse(TExpenses.Rows[0][setMonth(5, false)].ToString());
            jun += double.Parse(GrossProfit.Rows[0][setMonth(6, false)].ToString()) - double.Parse(TExpenses.Rows[0][setMonth(6, false)].ToString());
            jul += double.Parse(GrossProfit.Rows[0][setMonth(7, false)].ToString()) - double.Parse(TExpenses.Rows[0][setMonth(7, false)].ToString());
            aug += double.Parse(GrossProfit.Rows[0][setMonth(8, false)].ToString()) - double.Parse(TExpenses.Rows[0][setMonth(8, false)].ToString());
            sep += double.Parse(GrossProfit.Rows[0][setMonth(9, false)].ToString()) - double.Parse(TExpenses.Rows[0][setMonth(9, false)].ToString());
            oct += double.Parse(GrossProfit.Rows[0][setMonth(10, false)].ToString()) - double.Parse(TExpenses.Rows[0][setMonth(10, false)].ToString());
            nov += double.Parse(GrossProfit.Rows[0][setMonth(11, false)].ToString()) - double.Parse(TExpenses.Rows[0][setMonth(11, false)].ToString());
            dec += double.Parse(GrossProfit.Rows[0][setMonth(12, false)].ToString()) - double.Parse(TExpenses.Rows[0][setMonth(12, false)].ToString());
            total += double.Parse(GrossProfit.Rows[0]["Total"].ToString()) - double.Parse(TCostofSales.Rows[0]["Total"].ToString());

            dr5[setMonth(1, false)] = jan;
            dr5[setMonth(2, false)] = feb;
            dr5[setMonth(3, false)] = mar;
            dr5[setMonth(4, false)] = apr;
            dr5[setMonth(5, false)] = may;
            dr5[setMonth(6, false)] = jun;
            dr5[setMonth(7, false)] = jul;
            dr5[setMonth(8, false)] = aug;
            dr5[setMonth(9, false)] = sep;
            dr5[setMonth(10, false)] = oct;
            dr5[setMonth(11, false)] = nov;
            dr5[setMonth(12, false)] = dec;
            dr5["Total"] = total;
            NetProfit.Rows.Add(dr5.ItemArray);

            string netText = string.Empty;
            bool showLastNet = false;

            if (OtherIncome.Rows.Count > 0 || IncomeTaxes.Rows.Count > 0)
            {
                netText = "INCOME FROM OPERATIONS";
                showLastNet = true;
            }
            else
            {
                netText = "NET PROFIT";
            }

            report["paramNetText"] = netText;
            report["paramShowLastNet"] = showLastNet;

            report.RegData("Revenues", Revenues);
            report.RegData("TRevenues", TRevenues);
            report.RegData("CostOfSales", CostOfSales);
            report.RegData("TCostOfSales", TCostofSales);
            report.RegData("Expenses", Expenses);
            report.RegData("TExpenses", TExpenses);
            report.RegData("OtherIncome", OtherIncome);
            report.RegData("TOtherIncome", TOtherIncome);
            report.RegData("IncomeTaxes", IncomeTaxes);
            report.RegData("TIncomeTaxes", TIncomeTaxes);
            report.RegData("GrossProfit", GrossProfit);
            report.RegData("NetProfit", NetProfit);
            report.RegData("dsCompany", dsC.Tables[0]);
            report.Render();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

        return report;
    }

    protected void StiWebViewerIncomeStatement12Period_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        try
        {
            e.Report = Get12PeriodIncomeStatement();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void StiWebViewerIncomeStatement12Period_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {

    }

    #endregion

    #region Actual vs Budget Standard Report With/Without Variance (reportType = 2 || 5)

    private StiReport GetBudgetVsActualReport()
    {
        StiReport report = new StiReport();
        string reportPath = string.Empty;
        if (Request.QueryString["reportType"] == "2")
        {
            reportPath = Server.MapPath("StimulsoftReports/" + ConfigurationManager.AppSettings["ActualVsBudgetReport"].ToString());

            if (!string.IsNullOrEmpty(Request.QueryString["cType"]))
            {
                if (Request.QueryString["cType"] == "sub")
                {
                    reportPath = Server.MapPath("StimulsoftReports/BudgetVSActualWithSubReport.mrt");
                }
                else if (Request.QueryString["cType"] == "summary")
                {
                    reportPath = Server.MapPath("StimulsoftReports/BudgetVSActualSummaryReport.mrt");
                }
            }
        }
        else if (Request.QueryString["reportType"] == "5")
        {
            reportPath = Server.MapPath("StimulsoftReports/" + ConfigurationManager.AppSettings["ActualVsBudgetReportWithoutVariance"].ToString());

            if (!string.IsNullOrEmpty(Request.QueryString["cType"]))
            {
                if (Request.QueryString["cType"] == "sub")
                {
                    reportPath = Server.MapPath("StimulsoftReports/BudgetVSActualWithoutVarianceWithSubReport.mrt");
                }
                else if (Request.QueryString["cType"] == "summary")
                {
                    reportPath = Server.MapPath("StimulsoftReports/BudgetVSActualWithoutVarianceSummaryReport.mrt");
                }
            }
        }

        report.Load(reportPath);
        report.Compile();

        if (Request.QueryString["fromDate"] != null && Request.QueryString["toDate"] != null && Request.QueryString["budgetName"] != null)
        {
            DateTime startDate = Convert.ToDateTime(Request.QueryString["fromDate"]);
            DateTime endDate = Convert.ToDateTime(Request.QueryString["toDate"]);
            string budgetName = HttpUtility.UrlDecode(Request.QueryString["budgetName"]);

            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objPropUser);

            bool includeZero = false;
            if (Request.QueryString["includeZero"] != null && Convert.ToBoolean(Request.QueryString["includeZero"]))
            {
                includeZero = true;
            }

            DataSet BudgetVSActualDataSet = new DataSet("BudgetVSActualDataSet");
            DataSet BudgetVSActualDataSetRaw = bL_Budgets.GetBudgetVSActual(Session["config"].ToString(), startDate, endDate, budgetName, includeZero);
            BudgetVSActualDataSetRaw.Tables[0].AsEnumerable().ToList()
            .ForEach(b => b["Url"] = (Request.Url.Scheme +
                                      (Uri.SchemeDelimiter +
                                          (Request.Url.Authority +
                                              (Request.ApplicationPath + "/accountledger.aspx?id=" + b["Acct"].ToString() + "&s=" + System.Web.HttpUtility.UrlEncode(startDate.ToShortDateString()).ToString()
                                                                                                                                  + "&e=" + System.Web.HttpUtility.UrlEncode(endDate.ToShortDateString()).ToString()
                                              )
                                          )
                                      )
                                   )
                               );

            BudgetVSActualDataSetRaw.Tables[0].AcceptChanges();
            BudgetVSActualDataSetRaw = ProcessAndIncludeSummaryRows(BudgetVSActualDataSetRaw);

            // Revenues
            var dTable = BudgetVSActualDataSetRaw.Tables[0];
            var dView = dTable.DefaultView;
            dView.RowFilter = "Type = 3";
            DataTable Revenues = dView.ToTable();
            Revenues.TableName = "Revenues";

            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "TypeName = 'Total Revenues'";
            DataTable TRevenues = dView.ToTable();
            TRevenues.TableName = "TRevenues";

            // Cost Of Sales  
            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "Type = 4";
            DataTable CostOfSales = dView.ToTable();
            CostOfSales.TableName = "CostOfSales";

            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "TypeName = 'Total Cost Of Sales'";
            DataTable TCostOfSales = dView.ToTable();
            TCostOfSales.TableName = "TCostOfSales";

            // Expenses
            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "Type = 5";
            DataTable Expenses = dView.ToTable();
            Expenses.TableName = "Expenses";

            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "TypeName = 'Total Expenses'";
            DataTable TExpenses = dView.ToTable();
            TExpenses.TableName = "TExpenses";

            // Other Income (Expenses)
            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "Type = 8";
            DataTable OtherIncome = dView.ToTable();
            OtherIncome.TableName = "OtherIncome";

            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "TypeName = 'Total Other Income (Expense)'";
            DataTable TOtherIncome = dView.ToTable();
            TOtherIncome.TableName = "TOtherIncome";

            // Provisions for Income Taxes
            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "Type = 9";
            DataTable IncomeTaxes = dView.ToTable();
            IncomeTaxes.TableName = "IncomeTaxes";

            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "TypeName = 'Total Provisions for Income Taxes'";
            DataTable TIncomeTaxes = dView.ToTable();
            TIncomeTaxes.TableName = "TIncomeTaxes";

            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "TypeName = 'Gross Profit'";
            DataTable GrossTotal = dView.ToTable();
            GrossTotal.TableName = "GrossTotal";

            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "TypeName = 'Net Profit'";
            DataTable NetProfit = dView.ToTable();
            NetProfit.TableName = "NetProfit";

            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "TypeName = 'Income Before Provisions'";
            DataTable BeforeProvisions = dView.ToTable();
            BeforeProvisions.TableName = "BeforeProvisions";

            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "TypeName = 'Net Income'";
            DataTable NetIncome = dView.ToTable();
            NetIncome.TableName = "NetIncome";

            string netText = string.Empty;
            bool showLastNet = false;

            if (OtherIncome.Rows.Count > 0 || IncomeTaxes.Rows.Count > 0)
            {
                netText = "Income From Operations";
                showLastNet = true;

                report.RegData("BeforeProvisions", BeforeProvisions);
                report.RegData("NetIncome", NetIncome);
            }
            else
            {
                netText = "Net Profit/Loss";
            }

            report["StartDate"] = startDate.ToString("MM/dd/yyyy");
            report["EndDate"] = endDate.ToString("MM/dd/yyyy");
            report["BudgetName"] = budgetName;
            report["paramUsername"] = Session["Username"].ToString();
            report["paramNetText"] = netText;
            report["paramShowLastNet"] = showLastNet;

            report.RegData("Revenues", Revenues);
            report.RegData("TRevenues", TRevenues);
            report.RegData("CostOfSales", CostOfSales);
            report.RegData("TCostOfSales", TCostOfSales);
            report.RegData("Expenses", Expenses);
            report.RegData("TExpenses", TExpenses);
            report.RegData("OtherIncome", OtherIncome);
            report.RegData("TOtherIncome", TOtherIncome);
            report.RegData("IncomeTaxes", IncomeTaxes);
            report.RegData("TIncomeTaxes", TIncomeTaxes);
            report.RegData("GrossTotal", GrossTotal);
            report.RegData("NetProfit", NetProfit);
            report.RegData("dsCompany", dsC.Tables[0]);
        }

        report.Render();

        return report;
    }

    protected void StiWebViewerBudgetVsActual2_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        try
        {
            e.Report = GetBudgetVsActualReport();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void StiWebViewerBudgetVsActual2_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {

    }

    #endregion

    #region Actual vs Budget 12 Period Report (reportType = 3)

    private StiReport Get12MonthBudgetVsActualReort()
    {
        StiReport report = new StiReport();
        string reportPath = Server.MapPath("StimulsoftReports/BudgetVSActual12MonthReport.mrt");
        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActualVsBudget12MonthReport"]))
        {
            reportPath = Server.MapPath("StimulsoftReports/" + ConfigurationManager.AppSettings["ActualVsBudget12MonthReport"].ToString());
        }

        report.Load(reportPath);
        report.Compile();

        if (Request.QueryString["fromDate"] != null && Request.QueryString["toDate"] != null && Request.QueryString["budgetName"] != null)
        {
            DateTime startDate = Convert.ToDateTime((Request.QueryString["fromDate"]));
            DateTime endDate = Convert.ToDateTime((Request.QueryString["toDate"]));
            string budgetName = HttpUtility.UrlDecode(Request.QueryString["budgetName"]);
            string centers = string.Empty;

            if (!string.IsNullOrEmpty(Request.QueryString["centers"]))
            {
                centers = Request.QueryString["centers"].Trim();
            }

            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objPropUser);

            DataSet BudgetVSActualDataSet = new DataSet("BudgetVSActualDataSet");
            DataSet BudgetVSActualDataSetRaw = bL_Budgets.Get12MonthBudgetVSActual(Session["config"].ToString(), startDate, endDate, budgetName, centers);
            BudgetVSActualDataSetRaw.Tables[0].AsEnumerable().ToList()
                .ForEach(b => b["Url"] = (Request.Url.Scheme +
                        (Uri.SchemeDelimiter +
                            (Request.Url.Authority +
                                (Request.ApplicationPath + "/accountledger.aspx?id=" + b["Acct"].ToString() + "&s=" + System.Web.HttpUtility.UrlEncode(startDate.ToShortDateString()).ToString()
                                                                                                                    + "&e=" + System.Web.HttpUtility.UrlEncode(endDate.ToShortDateString()).ToString()
                                )
                            )
                        )
                    )
                );

            BudgetVSActualDataSetRaw.Tables[0].AcceptChanges();
            BudgetVSActualDataSetRaw = ProcessAndBuildData(BudgetVSActualDataSetRaw);
            BudgetVSActualDataSetRaw = ProcessAndAddSummaryRows(BudgetVSActualDataSetRaw);

            // Revenues
            var dTable = BudgetVSActualDataSetRaw.Tables[0];
            var dView = dTable.DefaultView;
            dView.RowFilter = "Type = 3";
            DataTable Revenues = dView.ToTable();
            Revenues.TableName = "Revenues";

            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "TypeName = 'Total Revenues'";
            DataTable TRevenues = dView.ToTable();
            TRevenues.TableName = "TRevenues";

            // Cost Of Sales
            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "Type = 4";
            DataTable CostOfSales = dView.ToTable();
            CostOfSales.TableName = "CostOfSales";

            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "TypeName = 'Total Cost Of Sales'";
            DataTable TCostOfSales = dView.ToTable();
            TCostOfSales.TableName = "TCostOfSales";

            // Expenses 
            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "Type = 5";
            DataTable Expenses = dView.ToTable();
            Expenses.TableName = "Expenses";

            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "TypeName = 'Total Expenses'";
            DataTable TExpenses = dView.ToTable();
            TExpenses.TableName = "TExpenses";

            // Other Income (Expenses)
            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "Type = 8";
            DataTable OtherIncome = dView.ToTable();
            OtherIncome.TableName = "OtherIncome";

            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "TypeName = 'Total Other Income (Expenses)'";
            DataTable TOtherIncome = dView.ToTable();
            TOtherIncome.TableName = "TOtherIncome";

            // Provisions for Income Taxes
            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "Type = 9";
            DataTable IncomeTaxes = dView.ToTable();
            IncomeTaxes.TableName = "IncomeTaxes";

            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "TypeName = 'Total Provisions for Income Taxes'";
            DataTable TIncomeTaxes = dView.ToTable();
            TIncomeTaxes.TableName = "TIncomeTaxes";

            // Gross Profit 
            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "TypeName = 'Gross Profit'";
            DataTable GrossTotal = dView.ToTable();
            GrossTotal.TableName = "GrossProfit";

            // Net Profit
            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "TypeName = 'Net Profit'";
            DataTable NetProfit = dView.ToTable();
            NetProfit.TableName = "NetProfit";

            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "TypeName = 'Income Before Provisions'";
            DataTable BeforeProvisions = dView.ToTable();
            BeforeProvisions.TableName = "BeforeProvisions";

            dTable = BudgetVSActualDataSetRaw.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "TypeName = 'Net Income'";
            DataTable NetIncome = dView.ToTable();
            NetIncome.TableName = "NetIncome";

            string netText = string.Empty;
            bool showLastNet = false;

            if (OtherIncome.Rows.Count > 0 || IncomeTaxes.Rows.Count > 0)
            {
                netText = "Income From Operations";
                showLastNet = true;

                report.RegData("BeforeProvisions", BeforeProvisions);
                report.RegData("NetIncome", NetIncome);
            }
            else
            {
                netText = "Net Profit/Loss";
            }

            report["StartDate"] = startDate.ToString("MM/dd/yyyy");
            report["EndDate"] = endDate.ToString("MM/dd/yyyy");
            report["BudgetName"] = budgetName;
            report["paramUsername"] = Session["Username"].ToString();
            report["paramNetText"] = netText;
            report["paramShowLastNet"] = showLastNet;

            report.RegData("Revenues", Revenues);
            report.RegData("TRevenues", TRevenues);
            report.RegData("CostOfSales", CostOfSales);
            report.RegData("TCostOfSales", TCostOfSales);
            report.RegData("Expenses", Expenses);
            report.RegData("TExpenses", TExpenses);
            report.RegData("OtherIncome", OtherIncome);
            report.RegData("TOtherIncome", TOtherIncome);
            report.RegData("IncomeTaxes", IncomeTaxes);
            report.RegData("TIncomeTaxes", TIncomeTaxes);
            report.RegData("GrossProfit", GrossTotal);
            report.RegData("NetProfit", NetProfit);
            report.RegData("dsCompany", dsC.Tables[0]);
        }

        report.Render();

        return report;
    }

    protected void StiWebViewerBudgetVsActual_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        try
        {
            e.Report = Get12MonthBudgetVsActualReort();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void StiWebViewerBudgetVsActual_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {

    }

    #endregion

    #region Standard Income Statement With Center (reportType = 4)

    private StiReport GetIncomeStatementWithCentersReport()
    {
        StiReport report = new StiReport();
        try
        {
            string reportPath = Server.MapPath("StimulsoftReports/IncomeStatementCentersReport.mrt");
            report.Load(reportPath);
            report.Compile();

            bool isWithSub = false;
            if (!string.IsNullOrEmpty(Request.QueryString["cType"]) && Request.QueryString["cType"] == "sub")
            {
                isWithSub = true;
            }

            // Add Departments into DataSource
            objPropUser.ConnConfig = Session["config"].ToString();
            var centers = _objBLReport.GetCenterNames(objPropUser);
            if (centers != null && centers.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in centers.Tables[0].Rows)
                {
                    var centralName = dr["CentralName"].ToString();
                    report.Dictionary.DataSources["Revenues"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["RevenuesTotal"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["CostOfSales"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["CostOfSalesTotal"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["Expenses"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["ExpensesTotal"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["GrossProfit"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["NetProfitTotal"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["OtherIncome"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["OtherIncomeTotal"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["IncomeTaxes"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["IncomeTaxesTotal"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["BeforeProvisions"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["NetIncome"].Columns.Add(centralName, typeof(double));
                }
            }

            _objChart.ConnConfig = Session["config"].ToString();

            #region Start-End date

            if (!string.IsNullOrEmpty(Request.QueryString["startDate"]))
            {
                _objChart.StartDate = Convert.ToDateTime(Request.QueryString["startDate"]);
            }
            else
            {
                int year = DateTime.Now.Year;
                _objChart.StartDate = new DateTime(year, 1, 1);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["endDate"]))
            {
                _objChart.EndDate = Convert.ToDateTime(Request.QueryString["endDate"]);
            }
            else
            {
                _objChart.EndDate = DateTime.Now.Date;
            }

            #endregion  

            string _FromDate = "From : " + _objChart.StartDate.ToString("MMMM dd, yyyy");
            string _ToDate = "To :    " + _objChart.EndDate.ToString("MMMM dd, yyyy");
            #region Set Header
            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objPropUser);

            #endregion

            _objChart.ConnConfig = Session["config"].ToString();

            if (!string.IsNullOrEmpty(Request.QueryString["departments"]))
            {
                _objChart.Departments = Request.QueryString["departments"].Trim();
            }

            DataSet _dsIncome = _objBLReport.GetIncomeStatementDetailsWithCenters(_objChart);
            string _netAmount = GetNetAmount(_dsIncome.Tables[0]).ToString();
            string _netText = "Net ";

            if (Convert.ToDouble(_netAmount) < 0)
            {
                _netText += "Loss";
            }
            else
            {
                _netText += "Income";
            }

            _dsIncome.Tables[0].AsEnumerable().ToList()
              .ForEach(b => b["Url"] = (Request.Url.Scheme +
                                          (Uri.SchemeDelimiter +
                                              (Request.Url.Authority +
                                                  (Request.ApplicationPath + "/accountledger.aspx?id=" + b["Acct"].ToString() + "&s=" + System.Web.HttpUtility.UrlEncode(_objChart.StartDate.ToShortDateString()).ToString()
                                                                                                                                      + "&e=" + System.Web.HttpUtility.UrlEncode(_objChart.EndDate.ToShortDateString()).ToString()
                                                  )
                                              )
                                          )
                                       )
                                   );

            _dsIncome.Tables[0].AcceptChanges();
            _dsIncome = ProcessAndBuildDataForCenters(_dsIncome);

            DataTable fTable = new DataTable();
            fTable = _dsIncome.Tables[0].Copy();
            DataSet finalDs = new DataSet();
            fTable.TableName = "Centers";
            finalDs.Tables.Add(fTable);
            finalDs.DataSetName = "Centers";

            DataSet _dsSummary = ProcessAndBuildSummaryRowForCenters(_dsIncome);

            DataTable RevenuesTotal = new DataTable();
            DataTable CostOfSalesTotal = new DataTable();
            DataTable ExpensesTotal = new DataTable();
            DataTable GrossTotal = new DataTable();
            DataTable NetProfit = new DataTable();
            DataTable OtherIncomeTotal = new DataTable();
            DataTable IncomeTaxesTotal = new DataTable();
            DataTable BeforeProvisions = new DataTable();
            DataTable NetIncome = new DataTable();
     
            foreach (DataTable dt in _dsSummary.Tables)
            {
                DataTable dtSummary = dt.Copy();

                if (dt.TableName == "RevenuesTotal")
                {
                    RevenuesTotal = dtSummary;
                }
                else if (dt.TableName == "CostOfSalesTotal")
                {
                    CostOfSalesTotal = dtSummary;
                }
                else if (dt.TableName == "ExpensesTotal")
                {
                    ExpensesTotal = dtSummary;
                }
                else if (dt.TableName == "GrossProfit")
                {
                    GrossTotal = dtSummary;
                }
                else if (dt.TableName == "NetProfitTotal")
                {
                    NetProfit = dtSummary;
                }
                else if (dt.TableName == "OtherIncomeTotal")
                {
                    OtherIncomeTotal = dtSummary;
                }
                else if (dt.TableName == "IncomeTaxesTotal")
                {
                    IncomeTaxesTotal = dtSummary;
                }
                else if (dt.TableName == "BeforeProvisions")
                {
                    BeforeProvisions = dtSummary;
                }
                else if (dt.TableName == "NetIncome")
                {
                    NetIncome = dtSummary;
                }
            }

            // Filter by type 
            DataTable filteredTable = finalDs.Tables[0].Copy();
            filteredTable.Columns.Remove("Acctnumber");
            filteredTable.Columns.Remove("AcctName");
            filteredTable.Columns.Remove("AnnualTotal");
            filteredTable.Columns.Remove("TypeName");
            filteredTable.Columns.Remove("Url");
            filteredTable.Columns.Remove("fDesc");

            DataView dView = filteredTable.DefaultView;
            dView.RowFilter = "Type = 3";
            DataTable Revenues = dView.ToTable();
            Revenues.Columns.Remove("Type");

            dView.RowFilter = "Type = 4";
            DataTable CostOfSales = dView.ToTable();
            CostOfSales.Columns.Remove("Type");

            dView.RowFilter = "Type = 5";
            DataTable Expenses = dView.ToTable();
            Expenses.Columns.Remove("Type");

            dView.RowFilter = "Type = 8";
            DataTable OtherIncome = dView.ToTable();
            OtherIncome.Columns.Remove("Type");

            dView.RowFilter = "Type = 9";
            DataTable IncomeTaxes = dView.ToTable();
            IncomeTaxes.Columns.Remove("Type");

            if (OtherIncome.Rows.Count > 0 || IncomeTaxes.Rows.Count > 0)
            {
                _netText = "Income From Operations";
            }

            report.RegData("Revenues", Revenues);
            report.RegData("CostOfSales", CostOfSales);
            report.RegData("Expenses", Expenses);
            report.RegData("OtherIncome", OtherIncome);
            report.RegData("IncomeTaxes", IncomeTaxes);
            report.RegData("RevenuesTotal", RevenuesTotal);
            report.RegData("CostOfSalesTotal", CostOfSalesTotal);
            report.RegData("ExpensesTotal", ExpensesTotal);
            report.RegData("GrossProfit", GrossTotal);
            report.RegData("NetProfitTotal", NetProfit);
            report.RegData("OtherIncomeTotal", OtherIncomeTotal);
            report.RegData("IncomeTaxesTotal", IncomeTaxesTotal);
            report.RegData("BeforeProvisions", BeforeProvisions);
            report.RegData("NetIncome", NetIncome);
            report.RegData("dsCompany", dsC.Tables[0]);

            report.Dictionary.Variables["paramUsername"].Value = Session["Username"].ToString();
            report.Dictionary.Variables["paramSDate"].Value = _FromDate;
            report.Dictionary.Variables["paramEDate"].Value = _ToDate;

            StiPage page = report.Pages[0];
            StiText txt = report.GetComponentByName("ReportHeader") as StiText;
            StiHeaderBand titleBand = new StiHeaderBand();
            StiHeaderBand titleBand1 = new StiHeaderBand();
            StiHeaderBand titleBand2 = new StiHeaderBand();
            StiHeaderBand titleBand3 = new StiHeaderBand();
            StiHeaderBand titleBand4 = new StiHeaderBand();

            if (Revenues.Rows.Count > 0)
            {
                titleBand.Height = 0.2;
                titleBand.Name = "Revenues";
                titleBand.Brush = new StiSolidBrush(Color.White);
                page.Components.Add(titleBand);

                //Create Title text on header
                StiText headerText = new StiText(new RectangleD(0, 0, page.Width, 0.2));
                headerText.Text = "Revenues";
                headerText.HorAlignment = StiTextHorAlignment.Left;
                headerText.VertAlignment = StiVertAlignment.Center;
                headerText.Name = "Revenues";
                headerText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
                headerText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                headerText.TextBrush = new StiSolidBrush(Color.White);
                titleBand.Components.Add(headerText);

                //Create HeaderBand
                StiHeaderBand headerBand = new StiHeaderBand();
                headerBand.Height = 0.2;
                headerBand.Name = "HeaderBand";
                headerBand.Border.Style = StiPenStyle.None;
                headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                page.Components.Add(headerBand);

                //Create GroupHeaderBand
                if (isWithSub)
                {
                    StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                    groupHeader.Name = "RevenuesGroupHeaderBand";
                    groupHeader.PrintOnAllPages = false;
                    groupHeader.Condition = new StiGroupConditionExpression("{Revenues.Sub}");
                    page.Components.Add(groupHeader);

                    StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                    groupHeaderText.Text.Value = "{Revenues.Sub}";
                    groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                    groupHeaderText.VertAlignment = StiVertAlignment.Center;
                    groupHeaderText.Font = new Font("Arial", 8F, FontStyle.Bold);
                    groupHeaderText.Border.Style = StiPenStyle.None;
                    groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                    groupHeaderText.WordWrap = true;
                    groupHeader.Components.Add(groupHeaderText);
                }

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "Revenues";
                dataBand.Height = 0.2;
                dataBand.Name = "dbRevenues";
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);

                StiDataSource dataSource = report.Dictionary.DataSources[0];
                page.CanGrow = true;
                page.CanShrink = true;

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in Revenues.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        //Create text on header
                        StiText hText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (Revenues.Columns.Count > 2)
                            {
                                hText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                            }
                            else
                            {
                                hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            }

                            hText.HorAlignment = StiTextHorAlignment.Left;
                        }
                        else
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            hText.HorAlignment = StiTextHorAlignment.Right;
                        }

                        hText.Text.Value = dataColumn.ColumnName;
                        hText.VertAlignment = StiVertAlignment.Center;
                        hText.Name = "HeaderText" + nameIndex.ToString();
                        hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                        hText.Border.Side = StiBorderSides.All;
                        hText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
                        hText.Border.Style = StiPenStyle.None;
                        hText.TextBrush = new StiSolidBrush(Color.White);
                        hText.WordWrap = true;
                        headerBand.Components.Add(hText);

                        StiText dataText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (Revenues.Columns.Count > 2)
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                            }
                            else
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            }
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        }

                        if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                        {
                            dataText.Text.Value = "{Revenues." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.HorAlignment = StiTextHorAlignment.Left;
                        }
                        else
                        {
                            dataText.Text.Value = "{Revenues." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                        }

                        dataText.VertAlignment = StiVertAlignment.Center;
                        dataText.Name = "DataText" + nameIndex.ToString();
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Border.Side = StiBorderSides.All;
                        dataText.Font = new System.Drawing.Font("Arial", 8F);
                        dataText.WordWrap = true;
                        dataText.Margins = new StiMargins(0, 1, 0, 0);
                        dataBand.Components.Add(dataText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }

                //Create GroupFooterBand
                if (isWithSub)
                {
                    pos = 0;

                    StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.25));
                    groupFooter.Name = "RevenuesGroupFooterBand";
                    page.Components.Add(groupFooter);

                    foreach (DataColumn dataColumn in Revenues.Columns)
                    {
                        if (dataColumn.ColumnName != "Sub")
                        {
                            //GroupFooterBand data
                            StiText footerText = null;
                            if (dataColumn.ColumnName == "Acct")
                            {
                                if (Revenues.Columns.Count > 2)
                                {
                                    footerText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                                }
                                else
                                {
                                    footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                }

                                footerText.Text.Value = "Total {Revenues.Sub}";
                                footerText.HorAlignment = StiTextHorAlignment.Left;
                                footerText.Border.Style = StiPenStyle.None;
                            }
                            else
                            {
                                footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                footerText.HorAlignment = StiTextHorAlignment.Right;
                                footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                footerText.Text.Value = "{Sum(Revenues." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + ")}";
                                footerText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            }

                            footerText.VertAlignment = StiVertAlignment.Center;
                            footerText.Font = new Font("Arial", 8F, FontStyle.Bold);
                            footerText.TextBrush = new StiSolidBrush(Color.Black);
                            footerText.WordWrap = true;
                            groupFooter.Components.Add(footerText);

                            if (dataColumn.ColumnName == "Acct")
                            {
                                pos = pos + (columnWidth * 2);
                            }
                            else
                            {
                                pos = pos + (columnWidth);
                            }
                        }
                    }
                }
            }

            if (RevenuesTotal.Rows.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "RevenuesTotal";
                dataBand.Height = 0.25;

                dataBand.Name = "RevenuesTotal";
                dataBand.Border.Style = StiPenStyle.Solid;
                dataBand.Border.Side = StiBorderSides.Bottom;
                dataBand.Border.Side = StiBorderSides.Top;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in Revenues.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        StiText dataText = null;

                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (Revenues.Columns.Count > 2)
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }

                        if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                        {
                            dataText.Text.Value = "{RevenuesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.HorAlignment = StiTextHorAlignment.Left;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }
                        else
                        {
                            dataText.Text.Value = "{RevenuesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }

                        dataText.VertAlignment = StiVertAlignment.Center;
                        dataText.Name = "DataText" + nameIndex.ToString();
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Border.Side = StiBorderSides.All;
                        dataText.WordWrap = true;
                        dataText.Margins = new StiMargins(0, 1, 0, 0);

                        dataBand.Components.Add(dataText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }
            }

            if (CostOfSales.Rows.Count > 0)
            {
                titleBand1.Height = 0.2;
                titleBand1.Name = String.Empty;
                titleBand1.Name = "Cost Of Sales";
                titleBand1.Brush = new StiSolidBrush(Color.White);
                page.Components.Add(titleBand1);

                StiText headerText = new StiText(new RectangleD(0, 0, page.Width, 0.2));
                headerText.Text = String.Empty;
                headerText.Text = "Cost Of Sales";
                headerText.HorAlignment = StiTextHorAlignment.Left;
                headerText.VertAlignment = StiVertAlignment.Center;
                headerText.Name = "CostOfSales";
                headerText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
                headerText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                headerText.TextBrush = new StiSolidBrush(Color.White);
                titleBand1.Components.Add(headerText);

                //Create HeaderBand
                StiHeaderBand headerBand = new StiHeaderBand();
                headerBand.Height = 0.2;
                headerBand.Name = "HeaderBand";
                headerBand.Border.Style = StiPenStyle.None;
                headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                page.Components.Add(headerBand);

                //Create GroupHeaderBand
                if (isWithSub)
                {
                    StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                    groupHeader.Name = "CostOfSalesGroupHeaderBand";
                    groupHeader.PrintOnAllPages = false;
                    groupHeader.Condition = new StiGroupConditionExpression("{CostOfSales.Sub}");
                    page.Components.Add(groupHeader);

                    StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                    groupHeaderText.Text.Value = "{CostOfSales.Sub}";
                    groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                    groupHeaderText.VertAlignment = StiVertAlignment.Center;
                    groupHeaderText.Font = new Font("Arial", 8F, FontStyle.Bold);
                    groupHeaderText.Border.Style = StiPenStyle.None;
                    groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                    groupHeaderText.WordWrap = true;
                    groupHeader.Components.Add(groupHeaderText);
                }

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "CostOfSales";
                dataBand.Height = 0.2;
                dataBand.Name = "CostOfSales";
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in CostOfSales.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        //Create text on header
                        StiText hText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (CostOfSales.Columns.Count > 2)
                            {
                                hText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                            }
                            else
                            {
                                hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            }

                            hText.HorAlignment = StiTextHorAlignment.Left;
                        }
                        else
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            hText.HorAlignment = StiTextHorAlignment.Right;
                        }

                        hText.Text.Value = dataColumn.ColumnName;
                        hText.VertAlignment = StiVertAlignment.Center;
                        hText.Name = "HeaderText" + nameIndex.ToString();
                        hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                        hText.Border.Side = StiBorderSides.All;
                        hText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
                        hText.Border.Style = StiPenStyle.None;
                        hText.TextBrush = new StiSolidBrush(Color.White);
                        hText.WordWrap = true;
                        headerBand.Components.Add(hText);

                        StiText dataText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (CostOfSales.Columns.Count > 2)
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                            }
                            else
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                            }

                            dataText.HorAlignment = StiTextHorAlignment.Left;
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                        }

                        dataText.Text.Value = "{CostOfSales." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.VertAlignment = StiVertAlignment.Center;
                        dataText.Name = "DataText" + nameIndex.ToString();
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Border.Side = StiBorderSides.All;
                        dataText.Font = new System.Drawing.Font("Arial", 8F);
                        dataText.WordWrap = true;
                        dataText.Margins = new StiMargins(0, 1, 0, 0);
                        dataBand.Components.Add(dataText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }

                //Create GroupFooterBand
                if (isWithSub)
                {
                    pos = 0;

                    StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.25));
                    groupFooter.Name = "CostOfSalesGroupFooterBand";
                    page.Components.Add(groupFooter);

                    foreach (DataColumn dataColumn in CostOfSales.Columns)
                    {
                        if (dataColumn.ColumnName != "Sub")
                        {
                            //GroupFooterBand data
                            StiText footerText = null;
                            if (dataColumn.ColumnName == "Acct")
                            {
                                if (Revenues.Columns.Count > 2)
                                {
                                    footerText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                                }
                                else
                                {
                                    footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                }

                                footerText.Text.Value = "Total {CostOfSales.Sub}";
                                footerText.HorAlignment = StiTextHorAlignment.Left;
                                footerText.Border.Style = StiPenStyle.None;
                            }
                            else
                            {
                                footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                footerText.HorAlignment = StiTextHorAlignment.Right;
                                footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                footerText.Text.Value = "{Sum(CostOfSales." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + ")}";
                                footerText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            }

                            footerText.VertAlignment = StiVertAlignment.Center;
                            footerText.Font = new Font("Arial", 8F, FontStyle.Bold);
                            footerText.TextBrush = new StiSolidBrush(Color.Black);
                            footerText.WordWrap = true;
                            groupFooter.Components.Add(footerText);

                            if (dataColumn.ColumnName == "Acct")
                            {
                                pos = pos + (columnWidth * 2);
                            }
                            else
                            {
                                pos = pos + (columnWidth);
                            }
                        }
                    }
                }
            }

            if (CostOfSalesTotal.Rows.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "CostOfSalesTotal";
                dataBand.Height = 0.25;
                dataBand.Name = "CostOfSalesTotal";
                dataBand.Border.Style = StiPenStyle.Solid;
                dataBand.Border.Side = StiBorderSides.Bottom;
                dataBand.Border.Side = StiBorderSides.Top;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in CostOfSales.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        StiText dataText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (CostOfSales.Columns.Count > 2)
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }

                        if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                        {
                            dataText.Text.Value = "{CostOfSalesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.HorAlignment = StiTextHorAlignment.Left;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }
                        else
                        {
                            dataText.Text.Value = "{CostOfSalesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }

                        dataText.VertAlignment = StiVertAlignment.Center;
                        dataText.Name = "DataText" + nameIndex.ToString();
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Border.Side = StiBorderSides.All;
                        dataText.WordWrap = true;
                        dataText.Margins = new StiMargins(0, 1, 0, 0);
                        dataBand.Components.Add(dataText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }
            }

            if (GrossTotal.Rows.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "GrossProfit";
                dataBand.Height = 0.3;
                dataBand.Name = "GrossProfit";
                dataBand.Border.Style = StiPenStyle.Solid;
                dataBand.Border.Side = StiBorderSides.Bottom;
                dataBand.Border.Side = StiBorderSides.Top;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in CostOfSales.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        StiText dataText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (CostOfSales.Columns.Count > 2)
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }

                        if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                        {
                            dataText.Text.Value = "{GrossProfit." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.HorAlignment = StiTextHorAlignment.Left;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }
                        else
                        {
                            dataText.Text.Value = "{GrossProfit." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }

                        dataText.VertAlignment = StiVertAlignment.Center;
                        dataText.Name = "DataText" + nameIndex.ToString();
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Border.Side = StiBorderSides.All;
                        dataText.WordWrap = true;
                        dataText.Margins = new StiMargins(0, 1, 0, 0);

                        dataBand.Components.Add(dataText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }
            }

            if (Expenses.Rows.Count > 0)
            {
                titleBand2.Height = 0.2;
                titleBand2.Name = String.Empty;
                titleBand2.Name = "Expenses";
                titleBand2.Brush = new StiSolidBrush(Color.White);
                page.Components.Add(titleBand2);

                StiText headerText = new StiText(new RectangleD(0, 0, page.Width, 0.2));
                headerText.Text = String.Empty;
                headerText.Text = "Expenses";
                headerText.HorAlignment = StiTextHorAlignment.Left;
                headerText.VertAlignment = StiVertAlignment.Center;
                headerText.Name = "Expenses";
                headerText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
                headerText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                headerText.TextBrush = new StiSolidBrush(Color.White);
                titleBand2.Components.Add(headerText);

                //Create HeaderBand
                StiHeaderBand headerBand = new StiHeaderBand();
                headerBand.Height = 0.25;
                headerBand.Name = "HeaderBand";
                headerBand.Border.Style = StiPenStyle.None;
                headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                page.Components.Add(headerBand);

                //Create GroupHeaderBand
                if (isWithSub)
                {
                    StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                    groupHeader.Name = "ExpensesGroupHeaderBand";
                    groupHeader.PrintOnAllPages = false;
                    groupHeader.Condition = new StiGroupConditionExpression("{Expenses.Sub}");
                    page.Components.Add(groupHeader);

                    StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                    groupHeaderText.Text.Value = "{Expenses.Sub}";
                    groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                    groupHeaderText.VertAlignment = StiVertAlignment.Center;
                    groupHeaderText.Font = new Font("Arial", 8F, FontStyle.Bold);
                    groupHeaderText.Border.Style = StiPenStyle.None;
                    groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                    groupHeaderText.WordWrap = true;
                    groupHeader.Components.Add(groupHeaderText);
                }

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "Expenses";
                dataBand.Height = 0.2;
                dataBand.Name = "Expenses";
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in Expenses.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        //Create text on header
                        StiText hText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (Expenses.Columns.Count > 2)
                            {
                                hText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                            }
                            else
                            {
                                hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            }

                            hText.HorAlignment = StiTextHorAlignment.Left;
                        }
                        else
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            hText.HorAlignment = StiTextHorAlignment.Right;
                        }

                        hText.Text.Value = dataColumn.ColumnName;
                        hText.VertAlignment = StiVertAlignment.Center;
                        hText.Name = "HeaderText" + nameIndex.ToString();
                        hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                        hText.Border.Side = StiBorderSides.All;
                        hText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
                        hText.Border.Style = StiPenStyle.None;
                        hText.TextBrush = new StiSolidBrush(Color.White);
                        hText.WordWrap = true;
                        headerBand.Components.Add(hText);

                        StiText dataText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (Expenses.Columns.Count > 2)
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                            }
                            else
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            }

                            dataText.HorAlignment = StiTextHorAlignment.Left;
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                        }

                        dataText.Text.Value = "{Expenses." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.VertAlignment = StiVertAlignment.Center;
                        dataText.Name = "DataText" + nameIndex.ToString();

                        if ((Expenses.Columns.Count / Expenses.Columns.Count) == 0)
                        {
                            dataText.Border.Style = StiPenStyle.Solid;
                            dataText.Border.Side = StiBorderSides.Top;
                        }
                        else
                        {
                            dataText.Border.Style = StiPenStyle.None;
                        }

                        dataText.OnlyText = false;
                        dataText.Border.Side = StiBorderSides.All;
                        dataText.Font = new System.Drawing.Font("Arial", 8F);
                        dataText.WordWrap = true;
                        dataText.Margins = new StiMargins(0, 1, 0, 0);
                        dataBand.Components.Add(dataText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }

                //Create GroupFooterBand
                if (isWithSub)
                {
                    pos = 0;

                    StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.25));
                    groupFooter.Name = "ExpensesGroupFooterBand";
                    page.Components.Add(groupFooter);

                    foreach (DataColumn dataColumn in Expenses.Columns)
                    {
                        if (dataColumn.ColumnName != "Sub")
                        {
                            //GroupFooterBand data
                            StiText footerText = null;
                            if (dataColumn.ColumnName == "Acct")
                            {
                                if (Revenues.Columns.Count > 2)
                                {
                                    footerText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                                }
                                else
                                {
                                    footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                }

                                footerText.Text.Value = "Total {Expenses.Sub}";
                                footerText.HorAlignment = StiTextHorAlignment.Left;
                                footerText.Border.Style = StiPenStyle.None;
                            }
                            else
                            {
                                footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                footerText.HorAlignment = StiTextHorAlignment.Right;
                                footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                footerText.Text.Value = "{Sum(Expenses." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + ")}";
                                footerText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            }

                            footerText.VertAlignment = StiVertAlignment.Center;
                            footerText.Font = new Font("Arial", 8F, FontStyle.Bold);
                            footerText.TextBrush = new StiSolidBrush(Color.Black);
                            footerText.WordWrap = true;
                            groupFooter.Components.Add(footerText);

                            if (dataColumn.ColumnName == "Acct")
                            {
                                pos = pos + (columnWidth * 2);
                            }
                            else
                            {
                                pos = pos + (columnWidth);
                            }
                        }
                    }
                }
            }

            if (ExpensesTotal.Rows.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "ExpensesTotal";
                dataBand.Height = 0.25;
                dataBand.Name = "ExpensesTotal";
                dataBand.Border.Style = StiPenStyle.Double;
                dataBand.Border.Side = StiBorderSides.Bottom;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in Expenses.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        StiText dataText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (Expenses.Columns.Count > 2)
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }

                        if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                        {
                            dataText.Text.Value = "{ExpensesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.HorAlignment = StiTextHorAlignment.Left;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }
                        else
                        {
                            dataText.Text.Value = "{ExpensesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }

                        dataText.VertAlignment = StiVertAlignment.Center;
                        dataText.Name = "DataText" + nameIndex.ToString();
                        dataText.Border.Style = StiPenStyle.Solid;
                        dataText.OnlyText = false;
                        dataText.Border.Side = StiBorderSides.Top;
                        dataText.WordWrap = true;
                        dataText.Margins = new StiMargins(0, 1, 0, 0);

                        dataBand.Components.Add(dataText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }
            }

            if (NetProfit.Rows.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "NetProfitTotal";
                dataBand.Height = 0.3;
                dataBand.Name = "NetProfitTotal";
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in Expenses.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        StiText dataText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (Expenses.Columns.Count > 2)
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }

                        if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                        {
                            if (dataColumn.ColumnName == "Acct")
                            {
                                dataText.Text.Value = _netText;
                            }
                            else
                            {
                                dataText.Text.Value = "{NetProfitTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            }

                            dataText.HorAlignment = StiTextHorAlignment.Left;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }
                        else
                        {
                            dataText.Text.Value = "{NetProfitTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }

                        dataText.VertAlignment = StiVertAlignment.Center;
                        dataText.Name = "DataText" + nameIndex.ToString();
                        dataText.Border.Style = StiPenStyle.Double;
                        dataText.Border.Side = StiBorderSides.Bottom;
                        dataText.OnlyText = false;
                        dataText.WordWrap = true;
                        dataText.Margins = new StiMargins(0, 1, 0, 0);

                        dataBand.Components.Add(dataText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }
            }

            if (OtherIncome.Rows.Count > 0)
            {
                titleBand3.Height = 0.2;
                titleBand3.Name = String.Empty;
                titleBand3.Name = "OtherIncome";
                titleBand3.Brush = new StiSolidBrush(Color.White);
                page.Components.Add(titleBand3);

                StiText headerText = new StiText(new RectangleD(0, 0, page.Width, 0.2));
                headerText.Text = String.Empty;
                headerText.Text = "Other Income (Expenses)";
                headerText.HorAlignment = StiTextHorAlignment.Left;
                headerText.VertAlignment = StiVertAlignment.Center;
                headerText.Name = "OtherIncome";
                headerText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
                headerText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                headerText.TextBrush = new StiSolidBrush(Color.White);
                titleBand3.Components.Add(headerText);

                //Create HeaderBand
                StiHeaderBand headerBand = new StiHeaderBand();
                headerBand.Height = 0.2;
                headerBand.Name = "HeaderBand";
                headerBand.Border.Style = StiPenStyle.None;
                headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                page.Components.Add(headerBand);

                //Create GroupHeaderBand
                if (isWithSub)
                {
                    StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                    groupHeader.Name = "OtherIncomeGroupHeaderBand";
                    groupHeader.PrintOnAllPages = false;
                    groupHeader.Condition = new StiGroupConditionExpression("{OtherIncome.Sub}");
                    page.Components.Add(groupHeader);

                    StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                    groupHeaderText.Text.Value = "{OtherIncome.Sub}";
                    groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                    groupHeaderText.VertAlignment = StiVertAlignment.Center;
                    groupHeaderText.Font = new Font("Arial", 8F, FontStyle.Bold);
                    groupHeaderText.Border.Style = StiPenStyle.None;
                    groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                    groupHeaderText.WordWrap = true;
                    groupHeader.Components.Add(groupHeaderText);
                }

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "OtherIncome";
                dataBand.Height = 0.2;
                dataBand.Name = "OtherIncome";
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in OtherIncome.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        //Create text on header
                        StiText hText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (OtherIncome.Columns.Count > 2)
                            {
                                hText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                            }
                            else
                            {
                                hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            }

                            hText.HorAlignment = StiTextHorAlignment.Left;
                        }
                        else
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            hText.HorAlignment = StiTextHorAlignment.Right;
                        }

                        hText.Text.Value = dataColumn.ColumnName;
                        hText.VertAlignment = StiVertAlignment.Center;
                        hText.Name = "HeaderText" + nameIndex.ToString();
                        hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                        hText.Border.Side = StiBorderSides.All;
                        hText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
                        hText.Border.Style = StiPenStyle.None;
                        hText.TextBrush = new StiSolidBrush(Color.White);
                        hText.WordWrap = true;
                        headerBand.Components.Add(hText);

                        StiText dataText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (OtherIncome.Columns.Count > 2)
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                            }
                            else
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                            }

                            dataText.HorAlignment = StiTextHorAlignment.Left;
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                        }

                        dataText.Text.Value = "{OtherIncome." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.VertAlignment = StiVertAlignment.Center;
                        dataText.Name = "DataText" + nameIndex.ToString();
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Border.Side = StiBorderSides.All;
                        dataText.Font = new System.Drawing.Font("Arial", 8F);
                        dataText.WordWrap = true;
                        dataText.Margins = new StiMargins(0, 1, 0, 0);
                        dataBand.Components.Add(dataText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }

                //Create GroupFooterBand
                if (isWithSub)
                {
                    pos = 0;

                    StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.25));
                    groupFooter.Name = "OtherIncomeGroupFooterBand";
                    page.Components.Add(groupFooter);

                    foreach (DataColumn dataColumn in OtherIncome.Columns)
                    {
                        if (dataColumn.ColumnName != "Sub")
                        {
                            //GroupFooterBand data
                            StiText footerText = null;
                            if (dataColumn.ColumnName == "Acct")
                            {
                                if (Revenues.Columns.Count > 2)
                                {
                                    footerText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                                }
                                else
                                {
                                    footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                }

                                footerText.Text.Value = "Total {OtherIncome.Sub}";
                                footerText.HorAlignment = StiTextHorAlignment.Left;
                                footerText.Border.Style = StiPenStyle.None;
                            }
                            else
                            {
                                footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                footerText.HorAlignment = StiTextHorAlignment.Right;
                                footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                footerText.Text.Value = "{Sum(OtherIncome." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + ")}";
                                footerText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            }

                            footerText.VertAlignment = StiVertAlignment.Center;
                            footerText.Font = new Font("Arial", 8F, FontStyle.Bold);
                            footerText.TextBrush = new StiSolidBrush(Color.Black);
                            footerText.WordWrap = true;
                            groupFooter.Components.Add(footerText);

                            if (dataColumn.ColumnName == "Acct")
                            {
                                pos = pos + (columnWidth * 2);
                            }
                            else
                            {
                                pos = pos + (columnWidth);
                            }
                        }
                    }
                }
            }

            if (OtherIncomeTotal.Rows.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "OtherIncomeTotal";
                dataBand.Height = 0.25;
                dataBand.Name = "OtherIncomeTotal";
                dataBand.Border.Style = StiPenStyle.Solid;
                dataBand.Border.Side = StiBorderSides.Bottom;
                dataBand.Border.Side = StiBorderSides.Top;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in OtherIncome.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        StiText dataText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (OtherIncome.Columns.Count > 2)
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }

                        if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                        {
                            dataText.Text.Value = "{OtherIncomeTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.HorAlignment = StiTextHorAlignment.Left;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }
                        else
                        {
                            dataText.Text.Value = "{OtherIncomeTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }

                        dataText.VertAlignment = StiVertAlignment.Center;
                        dataText.Name = "DataText" + nameIndex.ToString();
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Border.Side = StiBorderSides.All;
                        dataText.WordWrap = true;
                        dataText.Margins = new StiMargins(0, 1, 0, 0);
                        dataBand.Components.Add(dataText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }
            }

            if (BeforeProvisions.Rows.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "BeforeProvisions";
                dataBand.Height = 0.3;
                dataBand.Name = "BeforeProvisions";
                dataBand.Border.Style = StiPenStyle.Solid;
                dataBand.Border.Side = StiBorderSides.Bottom;
                dataBand.Border.Side = StiBorderSides.Top;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in OtherIncome.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        StiText dataText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (OtherIncome.Columns.Count > 2)
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }

                        if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                        {
                            dataText.Text.Value = "{BeforeProvisions." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.HorAlignment = StiTextHorAlignment.Left;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }
                        else
                        {
                            dataText.Text.Value = "{BeforeProvisions." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }

                        dataText.VertAlignment = StiVertAlignment.Center;
                        dataText.Name = "DataText" + nameIndex.ToString();
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Border.Side = StiBorderSides.All;
                        dataText.WordWrap = true;
                        dataText.Margins = new StiMargins(0, 1, 0, 0);

                        dataBand.Components.Add(dataText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }
            }

            if (IncomeTaxes.Rows.Count > 0)
            {
                titleBand4.Height = 0.2;
                titleBand4.Name = String.Empty;
                titleBand4.Name = "IncomeTaxes";
                titleBand4.Brush = new StiSolidBrush(Color.White);
                page.Components.Add(titleBand4);

                StiText headerText = new StiText(new RectangleD(0, 0, page.Width, 0.2));
                headerText.Text = "Provisions for Income Taxes";
                headerText.HorAlignment = StiTextHorAlignment.Left;
                headerText.VertAlignment = StiVertAlignment.Center;
                headerText.Name = "IncomeTaxes";
                headerText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
                headerText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                headerText.TextBrush = new StiSolidBrush(Color.White);
                titleBand4.Components.Add(headerText);

                //Create HeaderBand
                StiHeaderBand headerBand = new StiHeaderBand();
                headerBand.Height = 0.2;
                headerBand.Name = "HeaderBand";
                headerBand.Border.Style = StiPenStyle.None;
                headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                page.Components.Add(headerBand);

                //Create GroupHeaderBand
                if (isWithSub)
                {
                    StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                    groupHeader.Name = "IncomeTaxesGroupHeaderBand";
                    groupHeader.PrintOnAllPages = false;
                    groupHeader.Condition = new StiGroupConditionExpression("{IncomeTaxes.Sub}");
                    page.Components.Add(groupHeader);

                    StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                    groupHeaderText.Text.Value = "{IncomeTaxes.Sub}";
                    groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                    groupHeaderText.VertAlignment = StiVertAlignment.Center;
                    groupHeaderText.Font = new Font("Arial", 8F, FontStyle.Bold);
                    groupHeaderText.Border.Style = StiPenStyle.None;
                    groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                    groupHeaderText.WordWrap = true;
                    groupHeader.Components.Add(groupHeaderText);
                }

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "IncomeTaxes";
                dataBand.Height = 0.2;
                dataBand.Name = "IncomeTaxes";
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in IncomeTaxes.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        //Create text on header
                        StiText hText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (IncomeTaxes.Columns.Count > 2)
                            {
                                hText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                            }
                            else
                            {
                                hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            }

                            hText.HorAlignment = StiTextHorAlignment.Left;
                        }
                        else
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            hText.HorAlignment = StiTextHorAlignment.Right;
                        }

                        hText.Text.Value = dataColumn.ColumnName;
                        hText.VertAlignment = StiVertAlignment.Center;
                        hText.Name = "HeaderText" + nameIndex.ToString();
                        hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                        hText.Border.Side = StiBorderSides.All;
                        hText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
                        hText.Border.Style = StiPenStyle.None;
                        hText.TextBrush = new StiSolidBrush(Color.White);
                        hText.WordWrap = true;
                        headerBand.Components.Add(hText);

                        StiText dataText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (IncomeTaxes.Columns.Count > 2)
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                            }
                            else
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                            }

                            dataText.HorAlignment = StiTextHorAlignment.Left;
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                        }

                        dataText.Text.Value = "{IncomeTaxes." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.VertAlignment = StiVertAlignment.Center;
                        dataText.Name = "DataText" + nameIndex.ToString();
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Border.Side = StiBorderSides.All;
                        dataText.Font = new System.Drawing.Font("Arial", 8F);
                        dataText.WordWrap = true;
                        dataText.Margins = new StiMargins(0, 1, 0, 0);
                        dataBand.Components.Add(dataText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }

                //Create GroupFooterBand
                if (isWithSub)
                {
                    pos = 0;

                    StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.25));
                    groupFooter.Name = "IncomeTaxesGroupFooterBand";
                    page.Components.Add(groupFooter);

                    foreach (DataColumn dataColumn in IncomeTaxes.Columns)
                    {
                        if (dataColumn.ColumnName != "Sub")
                        {
                            //GroupFooterBand data
                            StiText footerText = null;
                            if (dataColumn.ColumnName == "Acct")
                            {
                                if (Revenues.Columns.Count > 2)
                                {
                                    footerText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                                }
                                else
                                {
                                    footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                }

                                footerText.Text.Value = "Total {IncomeTaxes.Sub}";
                                footerText.HorAlignment = StiTextHorAlignment.Left;
                                footerText.Border.Style = StiPenStyle.None;
                            }
                            else
                            {
                                footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                                footerText.HorAlignment = StiTextHorAlignment.Right;
                                footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                                footerText.Text.Value = "{Sum(IncomeTaxes." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + ")}";
                                footerText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            }

                            footerText.VertAlignment = StiVertAlignment.Center;
                            footerText.Font = new Font("Arial", 8F, FontStyle.Bold);
                            footerText.TextBrush = new StiSolidBrush(Color.Black);
                            footerText.WordWrap = true;
                            groupFooter.Components.Add(footerText);

                            if (dataColumn.ColumnName == "Acct")
                            {
                                pos = pos + (columnWidth * 2);
                            }
                            else
                            {
                                pos = pos + (columnWidth);
                            }
                        }
                    }
                }
            }

            if (IncomeTaxesTotal.Rows.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "IncomeTaxesTotal";
                dataBand.Height = 0.25;
                dataBand.Name = "IncomeTaxesTotal";
                dataBand.Border.Style = StiPenStyle.Solid;
                dataBand.Border.Side = StiBorderSides.Bottom;
                dataBand.Border.Side = StiBorderSides.Top;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in IncomeTaxes.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        StiText dataText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (IncomeTaxes.Columns.Count > 2)
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }

                        if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                        {
                            dataText.Text.Value = "{IncomeTaxesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.HorAlignment = StiTextHorAlignment.Left;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }
                        else
                        {
                            dataText.Text.Value = "{IncomeTaxesTotal" +
                                "." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }

                        dataText.VertAlignment = StiVertAlignment.Center;
                        dataText.Name = "DataText" + nameIndex.ToString();
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Border.Side = StiBorderSides.All;
                        dataText.WordWrap = true;
                        dataText.Margins = new StiMargins(0, 1, 0, 0);
                        dataBand.Components.Add(dataText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }
            }

            if (NetIncome.Rows.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "NetIncome";
                dataBand.Height = 0.25;
                dataBand.Name = "NetIncome";
                dataBand.Border.Style = StiPenStyle.Double;
                dataBand.Border.Side = StiBorderSides.Bottom | StiBorderSides.Top;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in IncomeTaxes.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        StiText dataText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (IncomeTaxes.Columns.Count > 2)
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }

                        if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                        {
                            dataText.Text.Value = "{NetIncome." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.HorAlignment = StiTextHorAlignment.Left;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }
                        else
                        {
                            dataText.Text.Value = "{NetIncome." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }

                        dataText.VertAlignment = StiVertAlignment.Center;
                        dataText.Name = "DataText" + nameIndex.ToString();
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Border.Side = StiBorderSides.All;
                        dataText.WordWrap = true;
                        dataText.Margins = new StiMargins(0, 1, 0, 0);

                        dataBand.Components.Add(dataText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }
            }

            report.Render();

            return report;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

        return report;
    }

    private StiReport GetIncomeStatementWithCentersSummaryReport()
    {
        StiReport report = new StiReport();
        try
        {
            string reportPath = Server.MapPath("StimulsoftReports/IncomeStatementCentersReport.mrt");
            report.Load(reportPath);
            report.Compile();

            // Add Departments into DataSource
            objPropUser.ConnConfig = Session["config"].ToString();
            var centers = _objBLReport.GetCenterNames(objPropUser);
            if (centers != null && centers.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in centers.Tables[0].Rows)
                {
                    var centralName = dr["CentralName"].ToString();
                    report.Dictionary.DataSources["Revenues"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["RevenuesTotal"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["CostOfSales"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["CostOfSalesTotal"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["Expenses"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["ExpensesTotal"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["GrossProfit"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["NetProfitTotal"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["OtherIncome"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["OtherIncomeTotal"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["IncomeTaxes"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["IncomeTaxesTotal"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["BeforeProvisions"].Columns.Add(centralName, typeof(double));
                    report.Dictionary.DataSources["NetIncome"].Columns.Add(centralName, typeof(double));
                }
            }

            _objChart.ConnConfig = Session["config"].ToString();

            #region Start-End date

            if (!string.IsNullOrEmpty(Request.QueryString["startDate"]))
            {
                _objChart.StartDate = Convert.ToDateTime(Request.QueryString["startDate"]);
            }
            else
            {
                int year = DateTime.Now.Year;
                _objChart.StartDate = new DateTime(year, 1, 1);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["endDate"]))
            {
                _objChart.EndDate = Convert.ToDateTime(Request.QueryString["endDate"]);
            }
            else
            {
                _objChart.EndDate = DateTime.Now.Date;
            }

            #endregion  

            string _FromDate = "From : " + _objChart.StartDate.ToString("MMMM dd, yyyy");
            string _ToDate = "To :    " + _objChart.EndDate.ToString("MMMM dd, yyyy");
            #region Set Header
            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objPropUser);

            #endregion

            _objChart.ConnConfig = Session["config"].ToString();

            if (!string.IsNullOrEmpty(Request.QueryString["departments"]))
            {
                _objChart.Departments = Request.QueryString["departments"].Trim();
            }

            DataSet _dsIncome = _objBLReport.GetIncomeStatementDetailsWithCenters(_objChart);
            string _netAmount = GetNetAmount(_dsIncome.Tables[0]).ToString();
            string _netText = "Net ";

            if (Convert.ToDouble(_netAmount) < 0)
            {
                _netText += "Loss";
            }
            else
            {
                _netText += "Income";
            }

            _dsIncome.Tables[0].AsEnumerable().ToList()
              .ForEach(b => b["Url"] = (Request.Url.Scheme +
                                          (Uri.SchemeDelimiter +
                                              (Request.Url.Authority +
                                                  (Request.ApplicationPath + "/accountledger.aspx?id=" + b["Acct"].ToString() + "&s=" + System.Web.HttpUtility.UrlEncode(_objChart.StartDate.ToShortDateString()).ToString()
                                                                                                                                      + "&e=" + System.Web.HttpUtility.UrlEncode(_objChart.EndDate.ToShortDateString()).ToString()
                                                  )
                                              )
                                          )
                                       )
                                   );

            _dsIncome.Tables[0].AcceptChanges();
            _dsIncome = ProcessAndBuildDataForCenters(_dsIncome);

            DataTable fTable = new DataTable();
            fTable = _dsIncome.Tables[0].Copy();
            DataSet finalDs = new DataSet();
            fTable.TableName = "Centers";
            finalDs.Tables.Add(fTable);
            finalDs.DataSetName = "Centers";

            DataSet _dsSummary = ProcessAndBuildSummaryRowForCenters(_dsIncome);

            DataTable RevenuesTotal = new DataTable();
            DataTable CostOfSalesTotal = new DataTable();
            DataTable ExpensesTotal = new DataTable();
            DataTable GrossTotal = new DataTable();
            DataTable NetProfit = new DataTable();
            DataTable OtherIncomeTotal = new DataTable();
            DataTable IncomeTaxesTotal = new DataTable();
            DataTable BeforeProvisions = new DataTable();
            DataTable NetIncome = new DataTable();

            foreach (DataTable dt in _dsSummary.Tables)
            {
                DataTable dtSummary = dt.Copy();

                if (dt.TableName == "RevenuesTotal")
                {
                    RevenuesTotal = dtSummary;
                }
                else if (dt.TableName == "CostOfSalesTotal")
                {
                    CostOfSalesTotal = dtSummary;
                }
                else if (dt.TableName == "ExpensesTotal")
                {
                    ExpensesTotal = dtSummary;
                }
                else if (dt.TableName == "GrossProfit")
                {
                    GrossTotal = dtSummary;
                }
                else if (dt.TableName == "NetProfitTotal")
                {
                    NetProfit = dtSummary;
                }
                else if (dt.TableName == "OtherIncomeTotal")
                {
                    OtherIncomeTotal = dtSummary;
                }
                else if (dt.TableName == "IncomeTaxesTotal")
                {
                    IncomeTaxesTotal = dtSummary;
                }
                else if (dt.TableName == "BeforeProvisions")
                {
                    BeforeProvisions = dtSummary;
                }
                else if (dt.TableName == "NetIncome")
                {
                    NetIncome = dtSummary;
                }
            }

            // Filter by type 
            DataTable filteredTable = finalDs.Tables[0].Copy();
            filteredTable.Columns.Remove("Acctnumber");
            filteredTable.Columns.Remove("AcctName");
            filteredTable.Columns.Remove("AnnualTotal");
            filteredTable.Columns.Remove("TypeName");
            filteredTable.Columns.Remove("Url");
            filteredTable.Columns.Remove("fDesc");

            DataView dView = filteredTable.DefaultView;
            dView.RowFilter = "Type = 3";
            DataTable Revenues = dView.ToTable();
            Revenues.Columns.Remove("Type");

            dView.RowFilter = "Type = 4";
            DataTable CostOfSales = dView.ToTable();
            CostOfSales.Columns.Remove("Type");

            dView.RowFilter = "Type = 5";
            DataTable Expenses = dView.ToTable();
            Expenses.Columns.Remove("Type");

            dView.RowFilter = "Type = 8";
            DataTable OtherIncome = dView.ToTable();
            OtherIncome.Columns.Remove("Type");

            dView.RowFilter = "Type = 9";
            DataTable IncomeTaxes = dView.ToTable();
            IncomeTaxes.Columns.Remove("Type");

            if (OtherIncome.Rows.Count > 0 || IncomeTaxes.Rows.Count > 0)
            {
                _netText = "Income From Operations";
            }

            report.RegData("Revenues", Revenues);
            report.RegData("CostOfSales", CostOfSales);
            report.RegData("Expenses", Expenses);
            report.RegData("OtherIncome", OtherIncome);
            report.RegData("IncomeTaxes", IncomeTaxes);
            report.RegData("RevenuesTotal", RevenuesTotal);
            report.RegData("CostOfSalesTotal", CostOfSalesTotal);
            report.RegData("ExpensesTotal", ExpensesTotal);
            report.RegData("GrossProfit", GrossTotal);
            report.RegData("NetProfitTotal", NetProfit);
            report.RegData("OtherIncomeTotal", OtherIncomeTotal);
            report.RegData("IncomeTaxesTotal", IncomeTaxesTotal);
            report.RegData("BeforeProvisions", BeforeProvisions);
            report.RegData("NetIncome", NetIncome);
            report.RegData("dsCompany", dsC.Tables[0]);

            report.Dictionary.Variables["paramUsername"].Value = Session["Username"].ToString();
            report.Dictionary.Variables["paramSDate"].Value = _FromDate;
            report.Dictionary.Variables["paramEDate"].Value = _ToDate;

            StiPage page = report.Pages[0];
            StiText txt = report.GetComponentByName("ReportHeader") as StiText;
            StiHeaderBand titleBand = new StiHeaderBand();
            StiHeaderBand titleBand1 = new StiHeaderBand();
            StiHeaderBand titleBand2 = new StiHeaderBand();
            StiHeaderBand titleBand3 = new StiHeaderBand();
            StiHeaderBand titleBand4 = new StiHeaderBand();

            if (Revenues.Rows.Count > 0)
            {
                titleBand.Height = 0.2;
                titleBand.Name = "Revenues";
                titleBand.Brush = new StiSolidBrush(Color.White);
                page.Components.Add(titleBand);

                //Create Title text on header
                StiText headerText = new StiText(new RectangleD(0, 0, page.Width, 0.2));
                headerText.Text = "Revenues";
                headerText.HorAlignment = StiTextHorAlignment.Left;
                headerText.VertAlignment = StiVertAlignment.Center;
                headerText.Name = "Revenues";
                headerText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
                headerText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                headerText.TextBrush = new StiSolidBrush(Color.White);
                titleBand.Components.Add(headerText);

                //Create HeaderBand
                StiHeaderBand headerBand = new StiHeaderBand();
                headerBand.Height = 0.2;
                headerBand.Name = "HeaderBand";
                headerBand.Border.Style = StiPenStyle.None;
                headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                page.Components.Add(headerBand);

                //Create GroupHeaderBand
                StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                groupHeader.Name = "RevenuesGroupHeaderBand";
                groupHeader.PrintOnAllPages = false;
                groupHeader.Condition = new StiGroupConditionExpression("{Revenues.Sub}");
                page.Components.Add(groupHeader);

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "Revenues";
                dataBand.Height = 0;
                dataBand.Name = "dbRevenues";
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in Revenues.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        //Create text on header
                        StiText hText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (Revenues.Columns.Count > 2)
                            {
                                hText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                            }
                            else
                            {
                                hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            }

                            hText.HorAlignment = StiTextHorAlignment.Left;
                        }
                        else
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            hText.HorAlignment = StiTextHorAlignment.Right;
                        }

                        hText.Text.Value = dataColumn.ColumnName;
                        hText.VertAlignment = StiVertAlignment.Center;
                        hText.Name = "HeaderText" + nameIndex.ToString();
                        hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                        hText.Border.Side = StiBorderSides.All;
                        hText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
                        hText.Border.Style = StiPenStyle.None;
                        hText.TextBrush = new StiSolidBrush(Color.White);
                        hText.WordWrap = true;
                        headerBand.Components.Add(hText);

                        StiText groupText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (Revenues.Columns.Count > 2)
                            {
                                groupText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                groupText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }

                            groupText.Text.Value = "{Revenues.Sub}";
                            groupText.HorAlignment = StiTextHorAlignment.Left;
                        }
                        else
                        {
                            groupText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            groupText.HorAlignment = StiTextHorAlignment.Right;
                            groupText.Text.Value = "{Sum(Revenues." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + ")}";
                            groupText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        }

                        groupText.VertAlignment = StiVertAlignment.Center;
                        groupText.Font = new Font("Arial", 8F, FontStyle.Bold);
                        groupText.Border.Style = StiPenStyle.None;
                        groupText.TextBrush = new StiSolidBrush(Color.Black);
                        groupText.WordWrap = true;
                        groupHeader.Components.Add(groupText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }
            }

            if (RevenuesTotal.Rows.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "RevenuesTotal";
                dataBand.Height = 0.25;

                dataBand.Name = "RevenuesTotal";
                dataBand.Border.Style = StiPenStyle.Solid;
                dataBand.Border.Side = StiBorderSides.Bottom;
                dataBand.Border.Side = StiBorderSides.Top;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];
                page.CanGrow = true;
                page.CanShrink = true;

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in Revenues.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        StiText dataText = null;

                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (Revenues.Columns.Count > 2)
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }

                        if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                        {
                            dataText.Text.Value = "{RevenuesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.HorAlignment = StiTextHorAlignment.Left;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }
                        else
                        {
                            dataText.Text.Value = "{RevenuesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }

                        dataText.VertAlignment = StiVertAlignment.Center;
                        dataText.Name = "DataText" + nameIndex.ToString();
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Border.Side = StiBorderSides.All;
                        dataText.WordWrap = true;
                        dataText.Margins = new StiMargins(0, 1, 0, 0);

                        dataBand.Components.Add(dataText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }
            }

            if (CostOfSales.Rows.Count > 0)
            {
                titleBand1.Height = 0.2;
                titleBand1.Name = String.Empty;
                titleBand1.Name = "Cost Of Sales";
                titleBand1.Brush = new StiSolidBrush(Color.White);
                page.Components.Add(titleBand1);

                StiText headerText = new StiText(new RectangleD(0, 0, page.Width, 0.2));
                headerText.Text = String.Empty;
                headerText.Text = "Cost Of Sales";
                headerText.HorAlignment = StiTextHorAlignment.Left;
                headerText.VertAlignment = StiVertAlignment.Center;
                headerText.Name = "CostOfSales";
                headerText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
                headerText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                headerText.TextBrush = new StiSolidBrush(Color.White);
                titleBand1.Components.Add(headerText);

                //Create HeaderBand
                StiHeaderBand headerBand = new StiHeaderBand();
                headerBand.Height = 0.2;
                headerBand.Name = "HeaderBand";
                headerBand.Border.Style = StiPenStyle.None;
                headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                page.Components.Add(headerBand);

                //Create GroupHeaderBand
                StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                groupHeader.Name = "CostOfSalesGroupHeaderBand";
                groupHeader.PrintOnAllPages = false;
                groupHeader.Condition = new StiGroupConditionExpression("{CostOfSales.Sub}");
                page.Components.Add(groupHeader);

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "CostOfSales";
                dataBand.Height = 0;
                dataBand.Name = "CostOfSales";
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in CostOfSales.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        //Create text on header
                        StiText hText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (CostOfSales.Columns.Count > 2)
                            {
                                hText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                            }
                            else
                            {
                                hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            }

                            hText.HorAlignment = StiTextHorAlignment.Left;
                        }
                        else
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            hText.HorAlignment = StiTextHorAlignment.Right;
                        }

                        hText.Text.Value = dataColumn.ColumnName;
                        hText.VertAlignment = StiVertAlignment.Center;
                        hText.Name = "HeaderText" + nameIndex.ToString();
                        hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                        hText.Border.Side = StiBorderSides.All;
                        hText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
                        hText.Border.Style = StiPenStyle.None;
                        hText.TextBrush = new StiSolidBrush(Color.White);
                        hText.WordWrap = true;
                        headerBand.Components.Add(hText);

                        StiText groupText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (Revenues.Columns.Count > 2)
                            {
                                groupText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                groupText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }

                            groupText.Text.Value = "{CostOfSales.Sub}";
                            groupText.HorAlignment = StiTextHorAlignment.Left;
                        }
                        else
                        {
                            groupText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            groupText.HorAlignment = StiTextHorAlignment.Right;
                            groupText.Text.Value = "{Sum(CostOfSales." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + ")}";
                            groupText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        }

                        groupText.VertAlignment = StiVertAlignment.Center;
                        groupText.Font = new Font("Arial", 8F, FontStyle.Bold);
                        groupText.Border.Style = StiPenStyle.None;
                        groupText.TextBrush = new StiSolidBrush(Color.Black);
                        groupText.WordWrap = true;
                        groupHeader.Components.Add(groupText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }
            }

            if (CostOfSalesTotal.Rows.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "CostOfSalesTotal";
                dataBand.Height = 0.25;
                dataBand.Name = "CostOfSalesTotal";
                dataBand.Border.Style = StiPenStyle.Solid;
                dataBand.Border.Side = StiBorderSides.Bottom;
                dataBand.Border.Side = StiBorderSides.Top;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];
                page.CanGrow = true;
                page.CanShrink = true;

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in CostOfSales.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        StiText dataText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (CostOfSales.Columns.Count > 2)
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }

                        if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                        {
                            dataText.Text.Value = "{CostOfSalesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.HorAlignment = StiTextHorAlignment.Left;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }
                        else
                        {
                            dataText.Text.Value = "{CostOfSalesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }

                        dataText.VertAlignment = StiVertAlignment.Center;
                        dataText.Name = "DataText" + nameIndex.ToString();
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Border.Side = StiBorderSides.All;
                        dataText.WordWrap = true;
                        dataText.Margins = new StiMargins(0, 1, 0, 0);
                        dataBand.Components.Add(dataText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }
            }

            if (GrossTotal.Rows.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "GrossProfit";
                dataBand.Height = 0.3;
                dataBand.Name = "GrossProfit";
                dataBand.Border.Style = StiPenStyle.Solid;
                dataBand.Border.Side = StiBorderSides.Bottom;
                dataBand.Border.Side = StiBorderSides.Top;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];
                page.CanGrow = true;
                page.CanShrink = true;

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in CostOfSales.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        StiText dataText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (CostOfSales.Columns.Count > 2)
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }

                        if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                        {
                            dataText.Text.Value = "{GrossProfit." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.HorAlignment = StiTextHorAlignment.Left;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }
                        else
                        {
                            dataText.Text.Value = "{GrossProfit." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }

                        dataText.VertAlignment = StiVertAlignment.Center;
                        dataText.Name = "DataText" + nameIndex.ToString();
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Border.Side = StiBorderSides.All;
                        dataText.WordWrap = true;
                        dataText.Margins = new StiMargins(0, 1, 0, 0);

                        dataBand.Components.Add(dataText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }
            }

            if (Expenses.Rows.Count > 0)
            {
                titleBand2.Height = 0.2;
                titleBand2.Name = String.Empty;
                titleBand2.Name = "Expenses";
                titleBand2.Brush = new StiSolidBrush(Color.White);
                page.Components.Add(titleBand2);

                StiText headerText = new StiText(new RectangleD(0, 0, page.Width, 0.2));
                headerText.Text = String.Empty;
                headerText.Text = "Expenses";
                headerText.HorAlignment = StiTextHorAlignment.Left;
                headerText.VertAlignment = StiVertAlignment.Center;
                headerText.Name = "Expenses";
                headerText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
                headerText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                headerText.TextBrush = new StiSolidBrush(Color.White);
                titleBand2.Components.Add(headerText);

                //Create HeaderBand
                StiHeaderBand headerBand = new StiHeaderBand();
                headerBand.Height = 0.25;
                headerBand.Name = "HeaderBand";
                headerBand.Border.Style = StiPenStyle.None;
                headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                page.Components.Add(headerBand);

                //Create GroupHeaderBand
                StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                groupHeader.Name = "ExpensesGroupHeaderBand";
                groupHeader.PrintOnAllPages = false;
                groupHeader.Condition = new StiGroupConditionExpression("{Expenses.Sub}");
                page.Components.Add(groupHeader);

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "Expenses";
                dataBand.Height = 0;
                dataBand.Name = "Expenses";
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in Expenses.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        //Create text on header
                        StiText hText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (Expenses.Columns.Count > 2)
                            {
                                hText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                            }
                            else
                            {
                                hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            }

                            hText.HorAlignment = StiTextHorAlignment.Left;
                        }
                        else
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            hText.HorAlignment = StiTextHorAlignment.Right;
                        }

                        hText.Text.Value = dataColumn.ColumnName;
                        hText.VertAlignment = StiVertAlignment.Center;
                        hText.Name = "HeaderText" + nameIndex.ToString();
                        hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                        hText.Border.Side = StiBorderSides.All;
                        hText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
                        hText.Border.Style = StiPenStyle.None;
                        hText.TextBrush = new StiSolidBrush(Color.White);
                        hText.WordWrap = true;
                        headerBand.Components.Add(hText);

                        StiText groupText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (Revenues.Columns.Count > 2)
                            {
                                groupText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                groupText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }

                            groupText.Text.Value = "{Expenses.Sub}";
                            groupText.HorAlignment = StiTextHorAlignment.Left;
                        }
                        else
                        {
                            groupText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            groupText.HorAlignment = StiTextHorAlignment.Right;
                            groupText.Text.Value = "{Sum(Expenses." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + ")}";
                            groupText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        }

                        groupText.VertAlignment = StiVertAlignment.Center;
                        groupText.Font = new Font("Arial", 8F, FontStyle.Bold);
                        groupText.Border.Style = StiPenStyle.None;
                        groupText.TextBrush = new StiSolidBrush(Color.Black);
                        groupText.WordWrap = true;
                        groupHeader.Components.Add(groupText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }
            }

            if (ExpensesTotal.Rows.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "ExpensesTotal";
                dataBand.Height = 0.25;
                dataBand.Name = "ExpensesTotal";
                dataBand.Border.Style = StiPenStyle.Double;
                dataBand.Border.Side = StiBorderSides.Bottom;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];
                page.CanGrow = true;
                page.CanShrink = true;

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in Expenses.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        StiText dataText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (Expenses.Columns.Count > 2)
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }

                        if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                        {
                            dataText.Text.Value = "{ExpensesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.HorAlignment = StiTextHorAlignment.Left;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }
                        else
                        {
                            dataText.Text.Value = "{ExpensesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }

                        dataText.VertAlignment = StiVertAlignment.Center;
                        dataText.Name = "DataText" + nameIndex.ToString();
                        dataText.Border.Style = StiPenStyle.Solid;
                        dataText.OnlyText = false;
                        dataText.Border.Side = StiBorderSides.Top;
                        dataText.WordWrap = true;
                        dataText.Margins = new StiMargins(0, 1, 0, 0);

                        dataBand.Components.Add(dataText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }
            }

            if (NetProfit.Rows.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "NetProfitTotal";
                dataBand.Height = 0.3;
                dataBand.Name = "NetProfitTotal";
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];
                page.CanGrow = true;
                page.CanShrink = true;

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in Expenses.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        StiText dataText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (Expenses.Columns.Count > 2)
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }

                        if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                        {
                            if (dataColumn.ColumnName == "Acct")
                            {
                                dataText.Text.Value = _netText;
                            }
                            else
                            {
                                dataText.Text.Value = "{NetProfitTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            }

                            dataText.HorAlignment = StiTextHorAlignment.Left;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }
                        else
                        {
                            dataText.Text.Value = "{NetProfitTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }

                        dataText.VertAlignment = StiVertAlignment.Center;
                        dataText.Name = "DataText" + nameIndex.ToString();
                        dataText.Border.Style = StiPenStyle.Double;
                        dataText.Border.Side = StiBorderSides.Bottom | StiBorderSides.Top;
                        dataText.OnlyText = false;
                        dataText.WordWrap = true;
                        dataText.Margins = new StiMargins(0, 1, 0, 0);

                        dataBand.Components.Add(dataText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }
            }

            if (OtherIncome.Rows.Count > 0)
            {
                titleBand3.Height = 0.2;
                titleBand3.Name = String.Empty;
                titleBand3.Name = "Other Income (Expenses)";
                titleBand3.Brush = new StiSolidBrush(Color.White);
                page.Components.Add(titleBand3);

                StiText headerText = new StiText(new RectangleD(0, 0, page.Width, 0.2));
                headerText.Text = String.Empty;
                headerText.Text = "Other Income (Expenses)";
                headerText.HorAlignment = StiTextHorAlignment.Left;
                headerText.VertAlignment = StiVertAlignment.Center;
                headerText.Name = "OtherIncome";
                headerText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
                headerText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                headerText.TextBrush = new StiSolidBrush(Color.White);
                titleBand3.Components.Add(headerText);

                //Create HeaderBand
                StiHeaderBand headerBand = new StiHeaderBand();
                headerBand.Height = 0.2;
                headerBand.Name = "HeaderBand";
                headerBand.Border.Style = StiPenStyle.None;
                headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                page.Components.Add(headerBand);

                //Create GroupHeaderBand
                StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                groupHeader.Name = "OtherIncomeGroupHeaderBand";
                groupHeader.PrintOnAllPages = false;
                groupHeader.Condition = new StiGroupConditionExpression("{OtherIncome.Sub}");
                page.Components.Add(groupHeader);

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "OtherIncome";
                dataBand.Height = 0;
                dataBand.Name = "OtherIncome";
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in OtherIncome.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        //Create text on header
                        StiText hText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (OtherIncome.Columns.Count > 2)
                            {
                                hText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                            }
                            else
                            {
                                hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            }

                            hText.HorAlignment = StiTextHorAlignment.Left;
                        }
                        else
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            hText.HorAlignment = StiTextHorAlignment.Right;
                        }

                        hText.Text.Value = dataColumn.ColumnName;
                        hText.VertAlignment = StiVertAlignment.Center;
                        hText.Name = "HeaderText" + nameIndex.ToString();
                        hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                        hText.Border.Side = StiBorderSides.All;
                        hText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
                        hText.Border.Style = StiPenStyle.None;
                        hText.TextBrush = new StiSolidBrush(Color.White);
                        hText.WordWrap = true;
                        headerBand.Components.Add(hText);

                        StiText groupText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (Revenues.Columns.Count > 2)
                            {
                                groupText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                groupText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }

                            groupText.Text.Value = "{OtherIncome.Sub}";
                            groupText.HorAlignment = StiTextHorAlignment.Left;
                        }
                        else
                        {
                            groupText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            groupText.HorAlignment = StiTextHorAlignment.Right;
                            groupText.Text.Value = "{Sum(OtherIncome." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + ")}";
                            groupText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        }

                        groupText.VertAlignment = StiVertAlignment.Center;
                        groupText.Font = new Font("Arial", 8F, FontStyle.Bold);
                        groupText.Border.Style = StiPenStyle.None;
                        groupText.TextBrush = new StiSolidBrush(Color.Black);
                        groupText.WordWrap = true;
                        groupHeader.Components.Add(groupText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }
            }

            if (OtherIncomeTotal.Rows.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "OtherIncomeTotalTotal";
                dataBand.Height = 0.25;
                dataBand.Name = "OtherIncomeTotalTotal";
                dataBand.Border.Style = StiPenStyle.Solid;
                dataBand.Border.Side = StiBorderSides.Bottom;
                dataBand.Border.Side = StiBorderSides.Top;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];
                page.CanGrow = true;
                page.CanShrink = true;

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in OtherIncome.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        StiText dataText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (OtherIncome.Columns.Count > 2)
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }

                        if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                        {
                            dataText.Text.Value = "{OtherIncomeTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.HorAlignment = StiTextHorAlignment.Left;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }
                        else
                        {
                            dataText.Text.Value = "{OtherIncomeTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }

                        dataText.VertAlignment = StiVertAlignment.Center;
                        dataText.Name = "DataText" + nameIndex.ToString();
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Border.Side = StiBorderSides.All;
                        dataText.WordWrap = true;
                        dataText.Margins = new StiMargins(0, 1, 0, 0);
                        dataBand.Components.Add(dataText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }
            }

            if (BeforeProvisions.Rows.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "BeforeProvisions";
                dataBand.Height = 0.3;
                dataBand.Name = "BeforeProvisions";
                dataBand.Border.Style = StiPenStyle.Solid;
                dataBand.Border.Side = StiBorderSides.Bottom;
                dataBand.Border.Side = StiBorderSides.Top;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];
                page.CanGrow = true;
                page.CanShrink = true;

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in OtherIncome.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        StiText dataText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (BeforeProvisions.Columns.Count > 2)
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }

                        if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                        {
                            dataText.Text.Value = "{BeforeProvisions." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.HorAlignment = StiTextHorAlignment.Left;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }
                        else
                        {
                            dataText.Text.Value = "{BeforeProvisions." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }

                        dataText.VertAlignment = StiVertAlignment.Center;
                        dataText.Name = "DataText" + nameIndex.ToString();
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.Border.Side = StiBorderSides.All;
                        dataText.WordWrap = true;
                        dataText.Margins = new StiMargins(0, 1, 0, 0);

                        dataBand.Components.Add(dataText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }
            }

            if (IncomeTaxes.Rows.Count > 0)
            {
                titleBand4.Height = 0.2;
                titleBand4.Name = String.Empty;
                titleBand4.Name = "IncomeTaxes";
                titleBand4.Brush = new StiSolidBrush(Color.White);
                page.Components.Add(titleBand4);

                StiText headerText = new StiText(new RectangleD(0, 0, page.Width, 0.2));
                headerText.Text = "Provisions for Income Taxes";
                headerText.HorAlignment = StiTextHorAlignment.Left;
                headerText.VertAlignment = StiVertAlignment.Center;
                headerText.Name = "IncomeTaxes";
                headerText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
                headerText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                headerText.TextBrush = new StiSolidBrush(Color.White);
                titleBand4.Components.Add(headerText);

                //Create HeaderBand
                StiHeaderBand headerBand = new StiHeaderBand();
                headerBand.Height = 0.25;
                headerBand.Name = "HeaderBand";
                headerBand.Border.Style = StiPenStyle.None;
                headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                page.Components.Add(headerBand);

                //Create GroupHeaderBand
                StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                groupHeader.Name = "IncomeTaxesGroupHeaderBand";
                groupHeader.PrintOnAllPages = false;
                groupHeader.Condition = new StiGroupConditionExpression("{IncomeTaxes.Sub}");
                page.Components.Add(groupHeader);

                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "IncomeTaxes";
                dataBand.Height = 0;
                dataBand.Name = "IncomeTaxes";
                dataBand.Border.Style = StiPenStyle.None;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in IncomeTaxes.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        //Create text on header
                        StiText hText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (IncomeTaxes.Columns.Count > 2)
                            {
                                hText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                            }
                            else
                            {
                                hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            }

                            hText.HorAlignment = StiTextHorAlignment.Left;
                        }
                        else
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                            hText.HorAlignment = StiTextHorAlignment.Right;
                        }

                        hText.Text.Value = dataColumn.ColumnName;
                        hText.VertAlignment = StiVertAlignment.Center;
                        hText.Name = "HeaderText" + nameIndex.ToString();
                        hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                        hText.Border.Side = StiBorderSides.All;
                        hText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
                        hText.Border.Style = StiPenStyle.None;
                        hText.TextBrush = new StiSolidBrush(Color.White);
                        hText.WordWrap = true;
                        headerBand.Components.Add(hText);

                        StiText groupText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (Revenues.Columns.Count > 2)
                            {
                                groupText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                groupText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }

                            groupText.Text.Value = "{IncomeTaxes.Sub}";
                            groupText.HorAlignment = StiTextHorAlignment.Left;
                        }
                        else
                        {
                            groupText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            groupText.HorAlignment = StiTextHorAlignment.Right;
                            groupText.Text.Value = "{Sum(IncomeTaxes." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + ")}";
                            groupText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        }

                        groupText.VertAlignment = StiVertAlignment.Center;
                        groupText.Font = new Font("Arial", 8F, FontStyle.Bold);
                        groupText.Border.Style = StiPenStyle.None;
                        groupText.TextBrush = new StiSolidBrush(Color.Black);
                        groupText.WordWrap = true;
                        groupHeader.Components.Add(groupText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }
            }

            if (IncomeTaxesTotal.Rows.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "IncomeTaxesTotal";
                dataBand.Height = 0.25;
                dataBand.Name = "IncomeTaxesTotal";
                dataBand.Border.Style = StiPenStyle.Double;
                dataBand.Border.Side = StiBorderSides.Bottom;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];
                page.CanGrow = true;
                page.CanShrink = true;

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in IncomeTaxes.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        StiText dataText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (IncomeTaxes.Columns.Count > 2)
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }

                        if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                        {
                            dataText.Text.Value = "{IncomeTaxesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.HorAlignment = StiTextHorAlignment.Left;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }
                        else
                        {
                            dataText.Text.Value = "{IncomeTaxesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }

                        dataText.VertAlignment = StiVertAlignment.Center;
                        dataText.Name = "DataText" + nameIndex.ToString();
                        dataText.Border.Style = StiPenStyle.Solid;
                        dataText.OnlyText = false;
                        dataText.Border.Side = StiBorderSides.Top;
                        dataText.WordWrap = true;
                        dataText.Margins = new StiMargins(0, 1, 0, 0);

                        dataBand.Components.Add(dataText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }
            }

            if (NetIncome.Rows.Count > 0)
            {
                //Create Databand
                StiDataBand dataBand = new StiDataBand();
                dataBand.DataSourceName = "NetIncome";
                dataBand.Height = 0.25;
                dataBand.Name = "NetIncome";
                dataBand.Border.Style = StiPenStyle.Double;
                dataBand.Border.Side = StiBorderSides.Bottom | StiBorderSides.Top;
                page.Components.Add(dataBand);
                StiDataSource dataSource = report.Dictionary.DataSources[0];
                page.CanGrow = true;
                page.CanShrink = true;

                double pos = 0;
                var columnCount = 0;
                if (Revenues.Columns.Count > 2)
                {
                    columnCount = Revenues.Columns.Count;
                }
                else
                {
                    columnCount = Revenues.Columns.Count + 1;
                }

                double columnWidth = page.Width / columnCount;
                int nameIndex = 1;

                foreach (DataColumn dataColumn in IncomeTaxes.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        StiText dataText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (IncomeTaxes.Columns.Count > 2)
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }

                        if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                        {
                            dataText.Text.Value = "{NetIncome." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.HorAlignment = StiTextHorAlignment.Left;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }
                        else
                        {
                            dataText.Text.Value = "{NetIncome." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                            dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                            dataText.HorAlignment = StiTextHorAlignment.Right;
                            dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                        }

                        dataText.VertAlignment = StiVertAlignment.Center;
                        dataText.Name = "DataText" + nameIndex.ToString();
                        dataText.Border.Style = StiPenStyle.None;
                        dataText.OnlyText = false;
                        dataText.WordWrap = true;
                        dataText.Margins = new StiMargins(0, 1, 0, 0);

                        dataBand.Components.Add(dataText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }

                        nameIndex++;
                    }
                }
            }

            report.Render();

            return report;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

        return report;
    }

    protected void StiWebViewerIncomeStatementWithCenters_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(Request.QueryString["cType"]) && Request.QueryString["cType"] == "summary")
            {
                e.Report = GetIncomeStatementWithCentersSummaryReport();
            }
            else
            {
                e.Report = GetIncomeStatementWithCentersReport();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void StiWebViewerIncomeStatementWithCenters_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {

    }

    #endregion

    #region Standard Income Statement Comparative FS with Center (reportType = 6)

    protected void StiWebViewerStandardIncomeStatementComparativeFsWithCenter_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        var builder = new StandardIncomeStatementComparativeFsWithCenterReportBuilder();
        builder.ConnConfig = Session["config"].ToString();
        e.Report = builder.GetReportTemplate();
    }

    protected void StiWebViewerStandardIncomeStatementComparativeFsWithCenter_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        var builder = new StandardIncomeStatementComparativeFsWithCenterReportBuilder();
        builder.ConnConfig = Session["config"].ToString();
        var startDate = Convert.ToDateTime(Request.QueryString["startDate"]);
        var endDate = Convert.ToDateTime(Request.QueryString["endDate"]);
        objPropUser.ConnConfig = Session["config"].ToString();
        var centers = _objBLReport.GetCenterNames(objPropUser);
        var dsC = objBL_User.getControl(objPropUser);
        var yearEnd = dsC.Tables[0].Rows[0].Field<short>("Year");

        _objChart.ConnConfig = Session["config"].ToString();

        #region Start-End date

        if (!string.IsNullOrEmpty(Request.QueryString["startDate"]))
        {
            _objChart.StartDate = Convert.ToDateTime(Request.QueryString["startDate"]);
        }
        else
        {
            int year = DateTime.Now.Year;
            _objChart.StartDate = new DateTime(year, 1, 1);
        }

        if (!string.IsNullOrEmpty(Request.QueryString["endDate"]))
        {
            _objChart.EndDate = Convert.ToDateTime(Request.QueryString["endDate"]);
        }
        else
        {
            _objChart.EndDate = DateTime.Now.Date;
        }

        #endregion

        string fromDate = "From : " + _objChart.StartDate.ToString("MMMM dd, yyyy");
        string toDate = "To :    " + _objChart.EndDate.ToString("MMMM dd, yyyy");

        Session["Username"].ToString();

        var reportParameters = new Dictionary<string, object>
        {
            {"paramUsername", Session["Username"].ToString()},
            {"paramSDate", fromDate},
            {"paramEDate", toDate}
        };

        var input = new SISComparativeWithCenterInput
        {
            StartDate = startDate,
            EndDate = endDate,
            Centrals = centers,
            Departments = Request.QueryString["departments"].Trim(),
            DsCompany = dsC,
            OfficeCenter = Request.QueryString["OfficeCenter"].Trim(),
            YearEnd = yearEnd
        };

        builder.Build(e.Report, input, reportParameters);
        e.Report.Render();
    }

    protected void StiWebViewerStandardIncomeStatementComparativeFsWithCenter_ExportReport(object sender, Stimulsoft.Report.Web.StiExportReportEventArgs e)
    {
        if (e.Format == StiExportFormat.Excel || e.Format == StiExportFormat.Excel2007 || e.Format == StiExportFormat.ExcelXml)
        {
            //Remove hyper link when exporting to excel.
            RemoveHyperWhenExporting(e.Report);
        }
    }

    #endregion

    #region Standard Income Statement With YTD (reportType = 7)

    private StiReport GetProfitAndLossYTDReport()
    {
        StiReport report = new StiReport();
        try
        {
            string reportPath = Server.MapPath("StimulsoftReports/IncomeStatementYTD.mrt");

            if (!string.IsNullOrEmpty(Request.QueryString["cType"]))
            {
                if (Request.QueryString["cType"] == "sub")
                {
                    reportPath = Server.MapPath("StimulsoftReports/IncomeStatementYTDWithSub.mrt");
                }
                else if (Request.QueryString["cType"] == "summary")
                {
                    reportPath = Server.MapPath("StimulsoftReports/IncomeStatementYTDSummary.mrt");
                }
            }

            report.Load(reportPath);
            report.Compile();

            _objChart.ConnConfig = Session["config"].ToString();

            #region Start-End date

            if (!string.IsNullOrEmpty(Request.QueryString["startDate"]))
            {
                _objChart.StartDate = Convert.ToDateTime(Request.QueryString["startDate"]);
            }
            else
            {
                int year = DateTime.Now.Year;
                _objChart.StartDate = new DateTime(year, 1, 1);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["endDate"]))
            {
                _objChart.EndDate = Convert.ToDateTime(Request.QueryString["endDate"]);
            }
            else
            {
                _objChart.EndDate = DateTime.Now.Date;
            }

            #endregion

            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objPropUser);

            // Get the start of the financial year
            string strQuery = "select [dbo].[Control].[YE] from [dbo].[control]";
            int? yearEndMonth;
            int startMonth = 1;
            int endMonth = 12;

            var dataSet = SqlHelper.ExecuteDataset(Session["config"].ToString(), CommandType.Text, strQuery);
            if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
            {
                yearEndMonth = int.Parse(dataSet.Tables[0].Rows[0][0].ToString());
                startMonth = int.Parse(yearEndMonth.ToString()) + 2;
                endMonth = int.Parse(yearEndMonth.ToString()) + 1;

                if (startMonth > 12)
                {
                    startMonth = startMonth - 12;
                }
            }

            var fiscalYear = _objChart.EndDate.Year;
            if (startMonth != 1)
            {
                if (_objChart.EndDate.Month < startMonth)
                    fiscalYear = fiscalYear - 1;
            }

            // YTD column will get data from the start of the fiscal year to end date
            _objChart.YearStartDate = new DateTime(fiscalYear, startMonth, 1);

            DataSet ds = _objBLReport.GetIncomeStatementYTD(_objChart);

            bool includeZero = false;
            if (Request.QueryString["includeZero"] != null && Convert.ToBoolean(Request.QueryString["includeZero"]))
            {
                includeZero = true;
            }

            var finalds = ProcessAndBuildDataForIncomeStatementYTD(ds, includeZero);

            finalds.Tables[0].AsEnumerable().ToList()
              .ForEach(b => b["Url"] = (Request.Url.Scheme +
                        (Uri.SchemeDelimiter +
                            (Request.Url.Authority +
                                (Request.ApplicationPath + "/accountledger.aspx?id=" + b["Acct"].ToString() + "&s=" + System.Web.HttpUtility.UrlEncode(_objChart.StartDate.ToShortDateString()).ToString()
                                                                                                                    + "&e=" + System.Web.HttpUtility.UrlEncode(_objChart.EndDate.ToShortDateString()).ToString()
                                )
                            )
                        )
                    )
                );

            finalds.Tables[0].AcceptChanges();

            // Revenues
            var dTable = finalds.Tables[0];
            var dView = dTable.DefaultView;
            dView.RowFilter = "Type = 3";
            DataTable Revenues = dView.ToTable();
            Revenues.TableName = "Revenues";

            DataTable TRevenues = Revenues.Clone();
            TRevenues.TableName = "TRevenues";
            var dr = TRevenues.NewRow();
            dr["Acct"] = 1000;
            dr["Type"] = 3;
            dr["TypeName"] = "Revenues";
            dr["fDesc"] = "Total Revenues";

            double ytd = 0.00, curTotal = 0.00;
            for (int i = 0; i < Revenues.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(Revenues.Rows[i]["Month"].ToString()))
                    curTotal += double.Parse(Revenues.Rows[i]["Month"].ToString());

                if (!string.IsNullOrEmpty(Revenues.Rows[i]["YTD"].ToString()))
                    ytd += double.Parse(Revenues.Rows[i]["YTD"].ToString());
            }

            dr["Month"] = curTotal;
            dr["YTD"] = ytd;
            TRevenues.Rows.Add(dr.ItemArray);

            // Cost of Sales
            dTable = finalds.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "Type = 4";
            DataTable CostOfSales = dView.ToTable();
            CostOfSales.TableName = "CostOfSales";

            DataTable TCostofSales = CostOfSales.Clone();
            TCostofSales.TableName = "TCostOfSales";
            var dr1 = CostOfSales.NewRow();
            dr1["Acct"] = 1000;
            dr1["Type"] = 3;
            dr1["TypeName"] = "Cost of Sales";
            dr1["fDesc"] = "Total Cost of Sales";

            ytd = curTotal = 0.00;
            for (int i = 0; i < CostOfSales.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(CostOfSales.Rows[i]["Month"].ToString()))
                    curTotal += double.Parse(CostOfSales.Rows[i]["Month"].ToString());

                if (!string.IsNullOrEmpty(CostOfSales.Rows[i]["YTD"].ToString()))
                    ytd += double.Parse(CostOfSales.Rows[i]["YTD"].ToString());
            }

            dr1["Month"] = curTotal;
            dr1["YTD"] = ytd;
            TCostofSales.Rows.Add(dr1.ItemArray);

            // Expenses
            dTable = finalds.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "Type = 5";
            DataTable Expenses = dView.ToTable();
            Expenses.TableName = "Expenses";

            DataTable TExpenses = Expenses.Clone();
            TExpenses.TableName = "TExpenses";
            var dr3 = Expenses.NewRow();
            dr3["Acct"] = 1000;
            dr3["Type"] = 3;
            dr3["TypeName"] = "Expenses";
            dr3["fDesc"] = "Total Expenses";

            ytd = curTotal = 0.00;
            for (int i = 0; i < Expenses.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(Expenses.Rows[i]["Month"].ToString()))
                    curTotal += double.Parse(Expenses.Rows[i]["Month"].ToString());

                if (!string.IsNullOrEmpty(Expenses.Rows[i]["YTD"].ToString()))
                    ytd += double.Parse(Expenses.Rows[i]["YTD"].ToString());
            }

            dr3["Month"] = curTotal;
            dr3["YTD"] = ytd;
            TExpenses.Rows.Add(dr3.ItemArray);

            // Total Other Income (Expenses)
            dTable = finalds.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "Type = 8";
            DataTable OtherIncome = dView.ToTable();
            OtherIncome.TableName = "OtherIncome";

            DataTable TOtherIncome = OtherIncome.Clone();
            TOtherIncome.TableName = "TOtherIncome";
            var dr8 = OtherIncome.NewRow();
            dr8["Acct"] = 8000;
            dr8["Type"] = 8;
            dr8["TypeName"] = "Other Income (Expenses)";
            dr8["fDesc"] = "Total Other Income (Expenses)";

            ytd = curTotal = 0.00;
            for (int i = 0; i < OtherIncome.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(OtherIncome.Rows[i]["Month"].ToString()))
                    curTotal += double.Parse(OtherIncome.Rows[i]["Month"].ToString());

                if (!string.IsNullOrEmpty(OtherIncome.Rows[i]["YTD"].ToString()))
                    ytd += double.Parse(OtherIncome.Rows[i]["YTD"].ToString());
            }

            dr8["Month"] = curTotal;
            dr8["YTD"] = ytd;
            TOtherIncome.Rows.Add(dr8.ItemArray);

            // Total Provisions for Income Taxes
            dTable = finalds.Tables[0];
            dView = dTable.DefaultView;
            dView.RowFilter = "Type = 9";
            DataTable IncomeTaxes = dView.ToTable();
            IncomeTaxes.TableName = "IncomeTaxes";

            DataTable TIncomeTaxes = IncomeTaxes.Clone();
            TIncomeTaxes.TableName = "TIncomeTaxes";
            var dr9 = IncomeTaxes.NewRow();
            dr9["Acct"] = 9000;
            dr9["Type"] = 9;
            dr9["TypeName"] = "Provisions for Income Taxes";
            dr9["fDesc"] = "Total Provisions for Income Taxes";

            ytd = curTotal = 0.00;
            for (int i = 0; i < IncomeTaxes.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(IncomeTaxes.Rows[i]["Month"].ToString()))
                    curTotal += double.Parse(IncomeTaxes.Rows[i]["Month"].ToString());

                if (!string.IsNullOrEmpty(IncomeTaxes.Rows[i]["YTD"].ToString()))
                    ytd += double.Parse(IncomeTaxes.Rows[i]["YTD"].ToString());
            }

            dr9["Month"] = curTotal;
            dr9["YTD"] = ytd;
            TIncomeTaxes.Rows.Add(dr9.ItemArray);

            // Gross Profit 
            DataTable GrossProfit = Expenses.Clone();
            GrossProfit.TableName = "GrossTotal";
            var dr4 = Expenses.NewRow();
            dr4["Acct"] = 1000;
            dr4["Type"] = 4.5;
            dr4["TypeName"] = "GrossTotal";
            dr4["fDesc"] = "Gross Profit";

            ytd = curTotal = 0.00;
            curTotal += double.Parse(TRevenues.Rows[0]["Month"].ToString()) - double.Parse(TCostofSales.Rows[0]["Month"].ToString());
            ytd += double.Parse(TRevenues.Rows[0]["YTD"].ToString()) - double.Parse(TCostofSales.Rows[0]["YTD"].ToString());

            dr4["Month"] = curTotal;
            dr4["YTD"] = ytd;
            GrossProfit.Rows.Add(dr4.ItemArray);

            DataTable NetProfit = Expenses.Clone();
            NetProfit.TableName = "NetProfit";
            var dr5 = Expenses.NewRow();
            dr5["Acct"] = 1000;
            dr5["Type"] = 6;
            dr5["TypeName"] = "NetProfit";
            dr5["fDesc"] = "Net Profit";

            ytd = curTotal = 0.00;
            curTotal += double.Parse(GrossProfit.Rows[0]["Month"].ToString()) - double.Parse(TExpenses.Rows[0]["Month"].ToString());
            ytd += double.Parse(GrossProfit.Rows[0]["YTD"].ToString()) - double.Parse(TExpenses.Rows[0]["YTD"].ToString());
            dr5["YTD"] = ytd;
            dr5["Month"] = curTotal;
            NetProfit.Rows.Add(dr5.ItemArray);

            //Calculate Cost Of Sales percentages
            var rPercent = double.Parse(TRevenues.Rows[0]["Month"].ToString()) / double.Parse(TRevenues.Rows[0]["Month"].ToString());
            var rYTDPercent = double.Parse(TRevenues.Rows[0]["YTD"].ToString()) / double.Parse(TRevenues.Rows[0]["YTD"].ToString());

            var cPercent = double.Parse(TCostofSales.Rows[0]["Month"].ToString()) / double.Parse(TRevenues.Rows[0]["Month"].ToString());
            var cYTDPercent = double.Parse(TCostofSales.Rows[0]["YTD"].ToString()) / double.Parse(TRevenues.Rows[0]["YTD"].ToString());

            var ePercent = double.Parse(TExpenses.Rows[0]["Month"].ToString()) / double.Parse(TRevenues.Rows[0]["Month"].ToString());
            var eYTDPercent = double.Parse(TExpenses.Rows[0]["YTD"].ToString()) / double.Parse(TRevenues.Rows[0]["YTD"].ToString());

            var oPercent = double.Parse(TOtherIncome.Rows[0]["Month"].ToString()) / double.Parse(TRevenues.Rows[0]["Month"].ToString());
            var oYTDPercent = double.Parse(TOtherIncome.Rows[0]["YTD"].ToString()) / double.Parse(TRevenues.Rows[0]["YTD"].ToString());

            var iPercent = double.Parse(TIncomeTaxes.Rows[0]["Month"].ToString()) / double.Parse(TRevenues.Rows[0]["Month"].ToString());
            var iYTDPercent = double.Parse(TIncomeTaxes.Rows[0]["YTD"].ToString()) / double.Parse(TRevenues.Rows[0]["YTD"].ToString());

            var grossPercent = rPercent - cPercent;
            var grossYTDPercent = rYTDPercent - cYTDPercent;

            var netPercent = rPercent - cPercent - ePercent;
            var netYTDPercent = rYTDPercent - cYTDPercent - eYTDPercent;

            var incomeBeforePercent = rPercent - cPercent - ePercent + oPercent;
            var incomeBeforeYTDPercent = rYTDPercent - cYTDPercent - eYTDPercent + oYTDPercent;

            var lastNetPercent = rPercent - cPercent - ePercent + oPercent - iPercent;
            var lastNetYTDPercent = rYTDPercent - cYTDPercent - eYTDPercent + oYTDPercent - iYTDPercent;

            string netText = string.Empty;
            bool showLastNet = false;

            if (OtherIncome.Rows.Count > 0 || IncomeTaxes.Rows.Count > 0)
            {
                netText = "Income From Operations";
                showLastNet = true;
            }
            else
            {
                netText = "Net Income";
            }

            report["paramUsername"] = Session["Username"].ToString();
            report["paramSDate"] = Convert.ToDateTime(_objChart.StartDate).ToString("MMMM dd, yyyy");
            report["paramEDate"] = Convert.ToDateTime(_objChart.EndDate).ToString("MMMM dd, yyyy");
            report["TotalRevenue"] = double.Parse(TRevenues.Rows[0]["Month"].ToString());
            report["TotalRevenueYTD"] = double.Parse(TRevenues.Rows[0]["YTD"].ToString());
            report["paramCPercent"] = cPercent;
            report["paramEPercent"] = ePercent;
            report["paramYTDCPercent"] = cYTDPercent;
            report["paramYTDEPercent"] = eYTDPercent;
            report["paramGrossAmount"] = double.Parse(GrossProfit.Rows[0]["Month"].ToString());
            report["paramGrossYTDAmount"] = double.Parse(GrossProfit.Rows[0]["YTD"].ToString());
            report["paramGrossPercent"] = grossPercent;
            report["paramGrossYTDPercent"] = grossYTDPercent;
            report["paramNetAmount"] = double.Parse(NetProfit.Rows[0]["Month"].ToString());
            report["paramNetYTDAmount"] = double.Parse(NetProfit.Rows[0]["YTD"].ToString());
            report["paramNetPercent"] = netPercent;
            report["paramNetYTDPercentage"] = netYTDPercent;

            report["paramIncomeBeforePercent"] = incomeBeforePercent;
            report["paramIncomeBeforeYTDPercent"] = incomeBeforeYTDPercent;
            report["paramLastNetPercent"] = lastNetPercent;
            report["paramLastNetYTDPercent"] = lastNetYTDPercent;
            report["paramNetText"] = netText;
            report["paramShowLastNet"] = showLastNet;

            report.RegData("Revenues", Revenues);
            report.RegData("TRevenues", TRevenues);
            report.RegData("CostOfSales", CostOfSales);
            report.RegData("TCostOfSales", TCostofSales);
            report.RegData("Expenses", Expenses);
            report.RegData("TExpenses", TExpenses);
            report.RegData("OtherIncome", OtherIncome);
            report.RegData("TOtherIncome", TOtherIncome);
            report.RegData("IncomeTaxes", IncomeTaxes);
            report.RegData("TIncomeTaxes", TIncomeTaxes);
            report.RegData("GrossProfit", GrossProfit);
            report.RegData("NetProfit", NetProfit);
            report.RegData("dsCompany", dsC.Tables[0]);
            report.Render();

            return report;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

        return report;
    }

    protected void StiWebViewerProfitAndLossYTD_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        try
        {
            e.Report = GetProfitAndLossYTDReport();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void StiWebViewerProfitAndLossYTD_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {

    }

    #endregion

    #region Standard Income Statement With Center With Budgets (reportType = 8)

    private StiReport GetIncomeStatementWithCentersBudgetsReport()
    {
        StiReport report = new StiReport();
        string reportPath = Server.MapPath("StimulsoftReports/IncomeStatementCentersBudgetsReport.mrt");
        report.Load(reportPath);
        report.Compile();

        bool isWithSub = false;
        if (!string.IsNullOrEmpty(Request.QueryString["cType"]) && Request.QueryString["cType"] == "sub")
        {
            isWithSub = true;
        }

        // Add Departments into DataSource
        objPropUser.ConnConfig = Session["config"].ToString();
        var centers = _objBLReport.GetCenterNames(objPropUser);
        if (centers != null && centers.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in centers.Tables[0].Rows)
            {
                var centralName = dr["CentralName"].ToString();
                report.Dictionary.DataSources["Revenues"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["RevenuesTotal"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["CostOfSales"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["CostOfSalesTotal"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["Expenses"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["ExpensesTotal"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["GrossProfit"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["NetProfitTotal"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["OtherIncome"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["OtherIncomeTotal"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["IncomeTaxes"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["IncomeTaxesTotal"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["BeforeProvisions"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["NetIncome"].Columns.Add(centralName, typeof(double));
            }
        }

        _objChart.ConnConfig = Session["config"].ToString();

        #region Start-End date

        if (!string.IsNullOrEmpty(Request.QueryString["startDate"]))
        {
            _objChart.StartDate = Convert.ToDateTime(Request.QueryString["startDate"]);
        }
        else
        {
            int year = DateTime.Now.Year;
            _objChart.StartDate = new DateTime(year, 1, 1);
        }

        if (!string.IsNullOrEmpty(Request.QueryString["endDate"]))
        {
            _objChart.EndDate = Convert.ToDateTime(Request.QueryString["endDate"]);
        }
        else
        {
            _objChart.EndDate = DateTime.Now.Date;
        }

        #endregion

        string _FromDate = "From : " + _objChart.StartDate.ToString("MMMM dd, yyyy");
        string _ToDate = "To :    " + _objChart.EndDate.ToString("MMMM dd, yyyy");

        #region Set Header

        DataSet dsC = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        dsC = objBL_User.getControl(objPropUser);

        #endregion

        _objChart.ConnConfig = Session["config"].ToString();

        if (!string.IsNullOrEmpty(Request.QueryString["departments"]))
        {
            _objChart.Departments = Request.QueryString["departments"].Trim();
        }

        if (!string.IsNullOrEmpty(Request.QueryString["budgets"]))
        {
            _objChart.Budgets = Request.QueryString["budgets"].Trim();
        }

        _objChart.Periods = GetPeriods(_objChart.StartDate, _objChart.EndDate);

        DataSet _dsIncome = _objBLReport.GetIncomeStatementDetailsWithCentersAndBudgets(_objChart);
        string _netAmount = GetNetAmount(_dsIncome.Tables[0]).ToString();

        _dsIncome.Tables[0].AcceptChanges();
        _dsIncome = ProcessAndBuildDataForCentersAndBudgets(_dsIncome);

        DataTable fTable = new DataTable();
        fTable = _dsIncome.Tables[0].Copy();
        DataSet finalDs = new DataSet();
        fTable.TableName = "Centers";
        finalDs.Tables.Add(fTable);
        finalDs.DataSetName = "Centers";

        DataSet _dsSummary = ProcessAndBuildSummaryRowForCentersAndBudgets(_dsIncome);

        DataTable RevenuesTotal = new DataTable();
        DataTable CostOfSalesTotal = new DataTable();
        DataTable ExpensesTotal = new DataTable();
        DataTable GrossTotal = new DataTable();
        DataTable NetProfit = new DataTable();
        DataTable OtherIncomeTotal = new DataTable();
        DataTable IncomeTaxesTotal = new DataTable();
        DataTable BeforeProvisions = new DataTable();
        DataTable NetIncome = new DataTable();

        foreach (DataTable dt in _dsSummary.Tables)
        {
            DataTable dtSummary = dt.Copy();

            if (dt.TableName == "RevenuesTotal")
            {
                RevenuesTotal = dtSummary;
            }
            else if (dt.TableName == "CostOfSalesTotal")
            {
                CostOfSalesTotal = dtSummary;
            }
            else if (dt.TableName == "ExpensesTotal")
            {
                ExpensesTotal = dtSummary;
            }
            else if (dt.TableName == "GrossProfit")
            {
                GrossTotal = dtSummary;
            }
            else if (dt.TableName == "NetProfitTotal")
            {
                NetProfit = dtSummary;
            }
            else if (dt.TableName == "OtherIncomeTotal")
            {
                OtherIncomeTotal = dtSummary;
            }
            else if (dt.TableName == "IncomeTaxesTotal")
            {
                IncomeTaxesTotal = dtSummary;
            }
            else if (dt.TableName == "BeforeProvisions")
            {
                BeforeProvisions = dtSummary;
            }
            else if (dt.TableName == "NetIncome")
            {
                NetIncome = dtSummary;
            }
        }

        // Filter by type 
        DataTable filteredTable = finalDs.Tables[0].Copy();
        filteredTable.Columns.Remove("Acctnumber");
        filteredTable.Columns.Remove("AcctName");
        filteredTable.Columns.Remove("AnnualTotal");
        filteredTable.Columns.Remove("TypeName");
        filteredTable.Columns.Remove("Url");
        filteredTable.Columns.Remove("fDesc");

        DataView dView = filteredTable.DefaultView;
        dView.RowFilter = "Type = 3";
        DataTable Revenues = dView.ToTable();
        Revenues.Columns.Remove("Type");

        dView.RowFilter = "Type = 4";
        DataTable CostOfSales = dView.ToTable();
        CostOfSales.Columns.Remove("Type");

        dView.RowFilter = "Type = 5";
        DataTable Expenses = dView.ToTable();
        Expenses.Columns.Remove("Type");

        dView.RowFilter = "Type = 8";
        DataTable OtherIncome = dView.ToTable();
        OtherIncome.Columns.Remove("Type");

        dView.RowFilter = "Type = 9";
        DataTable IncomeTaxes = dView.ToTable();
        IncomeTaxes.Columns.Remove("Type");

        string _netText = "Net Income";
        if (OtherIncome.Rows.Count > 0 || IncomeTaxes.Rows.Count > 0)
        {
            _netText = "Income From Operations";
        }

        report.RegData("Revenues", Revenues);
        report.RegData("CostOfSales", CostOfSales);
        report.RegData("Expenses", Expenses);
        report.RegData("OtherIncome", OtherIncome);
        report.RegData("IncomeTaxes", IncomeTaxes);
        report.RegData("RevenuesTotal", RevenuesTotal);
        report.RegData("CostOfSalesTotal", CostOfSalesTotal);
        report.RegData("ExpensesTotal", ExpensesTotal);
        report.RegData("GrossProfit", GrossTotal);
        report.RegData("NetProfitTotal", NetProfit);
        report.RegData("OtherIncomeTotal", OtherIncomeTotal);
        report.RegData("IncomeTaxesTotal", IncomeTaxesTotal);
        report.RegData("BeforeProvisions", BeforeProvisions);
        report.RegData("NetIncome", NetIncome);
        report.RegData("dsCompany", dsC.Tables[0]);

        report.Dictionary.Variables["paramUsername"].Value = Session["Username"].ToString();
        report.Dictionary.Variables["paramSDate"].Value = _FromDate;
        report.Dictionary.Variables["paramEDate"].Value = _ToDate;

        StiPage page = report.Pages[0];
        StiText txt = report.GetComponentByName("ReportHeader") as StiText;
        StiHeaderBand titleBand = new StiHeaderBand();
        StiHeaderBand titleBand1 = new StiHeaderBand();
        StiHeaderBand titleBand2 = new StiHeaderBand();
        StiHeaderBand titleBand3 = new StiHeaderBand();
        StiHeaderBand titleBand4 = new StiHeaderBand();

        if (Revenues.Rows.Count > 0)
        {
            titleBand.Height = 0.2;
            titleBand.Name = "Revenues";
            titleBand.Brush = new StiSolidBrush(Color.White);
            page.Components.Add(titleBand);

            //Create Title text on header
            StiText headerText = new StiText(new RectangleD(0, 0, page.Width, 0.2));
            headerText.Text = "Revenues";
            headerText.HorAlignment = StiTextHorAlignment.Left;
            headerText.VertAlignment = StiVertAlignment.Center;
            headerText.Name = "Revenues";
            headerText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            headerText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerText.TextBrush = new StiSolidBrush(Color.White);
            titleBand.Components.Add(headerText);

            //Create HeaderBand
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.2;
            headerBand.Name = "HeaderBand";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            page.Components.Add(headerBand);

            //Create GroupHeaderBand
            if (isWithSub)
            {
                StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                groupHeader.Name = "RevenuesGroupHeaderBand";
                groupHeader.PrintOnAllPages = false;
                groupHeader.Condition = new StiGroupConditionExpression("{Revenues.Sub}");
                page.Components.Add(groupHeader);

                StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                groupHeaderText.Text.Value = "{Revenues.Sub}";
                groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                groupHeaderText.VertAlignment = StiVertAlignment.Center;
                groupHeaderText.Font = new Font("Arial", 8F, FontStyle.Bold);
                groupHeaderText.Border.Style = StiPenStyle.None;
                groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                groupHeaderText.WordWrap = true;
                groupHeader.Components.Add(groupHeaderText);
            }

            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "Revenues";
            dataBand.Height = 0.2;
            dataBand.Name = "dbRevenues";
            dataBand.Border.Style = StiPenStyle.None;
            page.Components.Add(dataBand);

            StiDataSource dataSource = report.Dictionary.DataSources[0];
            page.CanGrow = true;
            page.CanShrink = true;

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in Revenues.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    //Create text on header
                    StiText hText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (Revenues.Columns.Count > 2)
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                        }
                        else
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        }

                        hText.HorAlignment = StiTextHorAlignment.Left;
                    }
                    else
                    {
                        hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        hText.HorAlignment = StiTextHorAlignment.Right;
                    }

                    hText.Text.Value = dataColumn.ColumnName;
                    hText.VertAlignment = StiVertAlignment.Center;
                    hText.Name = "HeaderText" + nameIndex.ToString();
                    hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                    hText.Border.Side = StiBorderSides.All;
                    hText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
                    hText.Border.Style = StiPenStyle.None;
                    hText.TextBrush = new StiSolidBrush(Color.White);
                    hText.WordWrap = true;
                    headerBand.Components.Add(hText);

                    StiText dataText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (Revenues.Columns.Count > 2)
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        }
                    }
                    else
                    {
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                    }

                    if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                    {
                        dataText.Text.Value = "{Revenues." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.HorAlignment = StiTextHorAlignment.Left;
                    }
                    else
                    {
                        dataText.Text.Value = "{Revenues." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                    }

                    dataText.VertAlignment = StiVertAlignment.Center;
                    dataText.Name = "DataText" + nameIndex.ToString();
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.Font = new System.Drawing.Font("Arial", 8F);
                    dataText.WordWrap = true;
                    dataText.Margins = new StiMargins(0, 1, 0, 0);
                    dataBand.Components.Add(dataText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }

            //Create GroupFooterBand
            if (isWithSub)
            {
                pos = 0;

                StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.25));
                groupFooter.Name = "RevenuesGroupFooterBand";
                page.Components.Add(groupFooter);

                foreach (DataColumn dataColumn in Revenues.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        //GroupFooterBand data
                        StiText footerText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (Revenues.Columns.Count > 2)
                            {
                                footerText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }

                            footerText.Text.Value = "Total {Revenues.Sub}";
                            footerText.HorAlignment = StiTextHorAlignment.Left;
                            footerText.Border.Style = StiPenStyle.None;
                        }
                        else
                        {
                            footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            footerText.HorAlignment = StiTextHorAlignment.Right;
                            footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            footerText.Text.Value = "{Sum(Revenues." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + ")}";
                            footerText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        }

                        footerText.VertAlignment = StiVertAlignment.Center;
                        footerText.Font = new Font("Arial", 8F, FontStyle.Bold);
                        footerText.TextBrush = new StiSolidBrush(Color.Black);
                        footerText.WordWrap = true;
                        groupFooter.Components.Add(footerText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }
                    }
                }
            }
        }

        if (RevenuesTotal.Rows.Count > 0)
        {
            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "RevenuesTotal";
            dataBand.Height = 0.25;

            dataBand.Name = "RevenuesTotal";
            dataBand.Border.Style = StiPenStyle.Solid;
            dataBand.Border.Side = StiBorderSides.Bottom;
            dataBand.Border.Side = StiBorderSides.Top;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in Revenues.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    StiText dataText = null;

                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (Revenues.Columns.Count > 2)
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }
                    }
                    else
                    {
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    }

                    if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                    {
                        dataText.Text.Value = "{RevenuesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.HorAlignment = StiTextHorAlignment.Left;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }
                    else
                    {
                        dataText.Text.Value = "{RevenuesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }

                    dataText.VertAlignment = StiVertAlignment.Center;
                    dataText.Name = "DataText" + nameIndex.ToString();
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.WordWrap = true;
                    dataText.Margins = new StiMargins(0, 1, 0, 0);

                    dataBand.Components.Add(dataText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }
        }

        if (CostOfSales.Rows.Count > 0)
        {
            titleBand1.Height = 0.2;
            titleBand1.Name = String.Empty;
            titleBand1.Name = "Cost Of Sales";
            titleBand1.Brush = new StiSolidBrush(Color.White);
            page.Components.Add(titleBand1);

            StiText headerText = new StiText(new RectangleD(0, 0, page.Width, 0.2));
            headerText.Text = String.Empty;
            headerText.Text = "Cost Of Sales";
            headerText.HorAlignment = StiTextHorAlignment.Left;
            headerText.VertAlignment = StiVertAlignment.Center;
            headerText.Name = "CostOfSales";
            headerText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            headerText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerText.TextBrush = new StiSolidBrush(Color.White);
            titleBand1.Components.Add(headerText);

            //Create HeaderBand
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.2;
            headerBand.Name = "HeaderBand";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            page.Components.Add(headerBand);

            //Create GroupHeaderBand
            if (isWithSub)
            {
                StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                groupHeader.Name = "CostOfSalesGroupHeaderBand";
                groupHeader.PrintOnAllPages = false;
                groupHeader.Condition = new StiGroupConditionExpression("{CostOfSales.Sub}");
                page.Components.Add(groupHeader);

                StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                groupHeaderText.Text.Value = "{CostOfSales.Sub}";
                groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                groupHeaderText.VertAlignment = StiVertAlignment.Center;
                groupHeaderText.Font = new Font("Arial", 8F, FontStyle.Bold);
                groupHeaderText.Border.Style = StiPenStyle.None;
                groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                groupHeaderText.WordWrap = true;
                groupHeader.Components.Add(groupHeaderText);
            }

            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "CostOfSales";
            dataBand.Height = 0.2;
            dataBand.Name = "CostOfSales";
            dataBand.Border.Style = StiPenStyle.None;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in CostOfSales.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    //Create text on header
                    StiText hText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (CostOfSales.Columns.Count > 2)
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                        }
                        else
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        }

                        hText.HorAlignment = StiTextHorAlignment.Left;
                    }
                    else
                    {
                        hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        hText.HorAlignment = StiTextHorAlignment.Right;
                    }

                    hText.Text.Value = dataColumn.ColumnName;
                    hText.VertAlignment = StiVertAlignment.Center;
                    hText.Name = "HeaderText" + nameIndex.ToString();
                    hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                    hText.Border.Side = StiBorderSides.All;
                    hText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
                    hText.Border.Style = StiPenStyle.None;
                    hText.TextBrush = new StiSolidBrush(Color.White);
                    hText.WordWrap = true;
                    headerBand.Components.Add(hText);

                    StiText dataText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (CostOfSales.Columns.Count > 2)
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                        }

                        dataText.HorAlignment = StiTextHorAlignment.Left;
                    }
                    else
                    {
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                    }

                    dataText.Text.Value = "{CostOfSales." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                    dataText.VertAlignment = StiVertAlignment.Center;
                    dataText.Name = "DataText" + nameIndex.ToString();
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.Font = new System.Drawing.Font("Arial", 8F);
                    dataText.WordWrap = true;
                    dataText.Margins = new StiMargins(0, 1, 0, 0);
                    dataBand.Components.Add(dataText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }

            //Create GroupFooterBand
            if (isWithSub)
            {
                pos = 0;

                StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.25));
                groupFooter.Name = "CostOfSalesGroupFooterBand";
                page.Components.Add(groupFooter);

                foreach (DataColumn dataColumn in CostOfSales.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        //GroupFooterBand data
                        StiText footerText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (Revenues.Columns.Count > 2)
                            {
                                footerText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }

                            footerText.Text.Value = "Total {CostOfSales.Sub}";
                            footerText.HorAlignment = StiTextHorAlignment.Left;
                            footerText.Border.Style = StiPenStyle.None;
                        }
                        else
                        {
                            footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            footerText.HorAlignment = StiTextHorAlignment.Right;
                            footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            footerText.Text.Value = "{Sum(CostOfSales." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + ")}";
                            footerText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        }

                        footerText.VertAlignment = StiVertAlignment.Center;
                        footerText.Font = new Font("Arial", 8F, FontStyle.Bold);
                        footerText.TextBrush = new StiSolidBrush(Color.Black);
                        footerText.WordWrap = true;
                        groupFooter.Components.Add(footerText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }
                    }
                }
            }
        }

        if (CostOfSalesTotal.Rows.Count > 0)
        {
            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "CostOfSalesTotal";
            dataBand.Height = 0.25;
            dataBand.Name = "CostOfSalesTotal";
            dataBand.Border.Style = StiPenStyle.Solid;
            dataBand.Border.Side = StiBorderSides.Bottom;
            dataBand.Border.Side = StiBorderSides.Top;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in CostOfSales.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    StiText dataText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (CostOfSales.Columns.Count > 2)
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }
                    }
                    else
                    {
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    }

                    if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                    {
                        dataText.Text.Value = "{CostOfSalesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.HorAlignment = StiTextHorAlignment.Left;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }
                    else
                    {
                        dataText.Text.Value = "{CostOfSalesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }

                    dataText.VertAlignment = StiVertAlignment.Center;
                    dataText.Name = "DataText" + nameIndex.ToString();
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.WordWrap = true;
                    dataText.Margins = new StiMargins(0, 1, 0, 0);
                    dataBand.Components.Add(dataText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }
        }

        if (GrossTotal.Rows.Count > 0)
        {
            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "GrossProfit";
            dataBand.Height = 0.3;
            dataBand.Name = "GrossProfit";
            dataBand.Border.Style = StiPenStyle.Solid;
            dataBand.Border.Side = StiBorderSides.Bottom;
            dataBand.Border.Side = StiBorderSides.Top;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in CostOfSales.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    StiText dataText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (CostOfSales.Columns.Count > 2)
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }
                    }
                    else
                    {
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    }

                    if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                    {
                        dataText.Text.Value = "{GrossProfit." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.HorAlignment = StiTextHorAlignment.Left;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }
                    else
                    {
                        dataText.Text.Value = "{GrossProfit." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }

                    dataText.VertAlignment = StiVertAlignment.Center;
                    dataText.Name = "DataText" + nameIndex.ToString();
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.WordWrap = true;
                    dataText.Margins = new StiMargins(0, 1, 0, 0);

                    dataBand.Components.Add(dataText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }
        }

        if (Expenses.Rows.Count > 0)
        {
            titleBand2.Height = 0.2;
            titleBand2.Name = String.Empty;
            titleBand2.Name = "Expenses";
            titleBand2.Brush = new StiSolidBrush(Color.White);
            page.Components.Add(titleBand2);

            StiText headerText = new StiText(new RectangleD(0, 0, page.Width, 0.2));
            headerText.Text = String.Empty;
            headerText.Text = "Expenses";
            headerText.HorAlignment = StiTextHorAlignment.Left;
            headerText.VertAlignment = StiVertAlignment.Center;
            headerText.Name = "Expenses";
            headerText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            headerText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerText.TextBrush = new StiSolidBrush(Color.White);
            titleBand2.Components.Add(headerText);

            //Create HeaderBand
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.25;
            headerBand.Name = "HeaderBand";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            page.Components.Add(headerBand);

            //Create GroupHeaderBand
            if (isWithSub)
            {
                StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                groupHeader.Name = "ExpensesGroupHeaderBand";
                groupHeader.PrintOnAllPages = false;
                groupHeader.Condition = new StiGroupConditionExpression("{Expenses.Sub}");
                page.Components.Add(groupHeader);

                StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                groupHeaderText.Text.Value = "{Expenses.Sub}";
                groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                groupHeaderText.VertAlignment = StiVertAlignment.Center;
                groupHeaderText.Font = new Font("Arial", 8F, FontStyle.Bold);
                groupHeaderText.Border.Style = StiPenStyle.None;
                groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                groupHeaderText.WordWrap = true;
                groupHeader.Components.Add(groupHeaderText);
            }

            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "Expenses";
            dataBand.Height = 0.2;
            dataBand.Name = "Expenses";
            dataBand.Border.Style = StiPenStyle.None;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in Expenses.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    //Create text on header
                    StiText hText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (Expenses.Columns.Count > 2)
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                        }
                        else
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        }

                        hText.HorAlignment = StiTextHorAlignment.Left;
                    }
                    else
                    {
                        hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        hText.HorAlignment = StiTextHorAlignment.Right;
                    }

                    hText.Text.Value = dataColumn.ColumnName;
                    hText.VertAlignment = StiVertAlignment.Center;
                    hText.Name = "HeaderText" + nameIndex.ToString();
                    hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                    hText.Border.Side = StiBorderSides.All;
                    hText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
                    hText.Border.Style = StiPenStyle.None;
                    hText.TextBrush = new StiSolidBrush(Color.White);
                    hText.WordWrap = true;
                    headerBand.Components.Add(hText);

                    StiText dataText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (Expenses.Columns.Count > 2)
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        }

                        dataText.HorAlignment = StiTextHorAlignment.Left;
                    }
                    else
                    {
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                    }

                    dataText.Text.Value = "{Expenses." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                    dataText.VertAlignment = StiVertAlignment.Center;
                    dataText.Name = "DataText" + nameIndex.ToString();

                    if ((Expenses.Columns.Count / Expenses.Columns.Count) == 0)
                    {
                        dataText.Border.Style = StiPenStyle.Solid;
                        dataText.Border.Side = StiBorderSides.Top;
                    }
                    else
                    {
                        dataText.Border.Style = StiPenStyle.None;
                    }

                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.Font = new System.Drawing.Font("Arial", 8F);
                    dataText.WordWrap = true;
                    dataText.Margins = new StiMargins(0, 1, 0, 0);
                    dataBand.Components.Add(dataText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }

            //Create GroupFooterBand
            if (isWithSub)
            {
                pos = 0;

                StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.25));
                groupFooter.Name = "ExpensesGroupFooterBand";
                page.Components.Add(groupFooter);

                foreach (DataColumn dataColumn in Expenses.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        //GroupFooterBand data
                        StiText footerText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (Revenues.Columns.Count > 2)
                            {
                                footerText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }

                            footerText.Text.Value = "Total {Expenses.Sub}";
                            footerText.HorAlignment = StiTextHorAlignment.Left;
                            footerText.Border.Style = StiPenStyle.None;
                        }
                        else
                        {
                            footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            footerText.HorAlignment = StiTextHorAlignment.Right;
                            footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            footerText.Text.Value = "{Sum(Expenses." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + ")}";
                            footerText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        }

                        footerText.VertAlignment = StiVertAlignment.Center;
                        footerText.Font = new Font("Arial", 8F, FontStyle.Bold);
                        footerText.TextBrush = new StiSolidBrush(Color.Black);
                        footerText.WordWrap = true;
                        groupFooter.Components.Add(footerText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }
                    }
                }
            }
        }

        if (ExpensesTotal.Rows.Count > 0)
        {
            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "ExpensesTotal";
            dataBand.Height = 0.25;
            dataBand.Name = "ExpensesTotal";
            dataBand.Border.Style = StiPenStyle.Double;
            dataBand.Border.Side = StiBorderSides.Bottom;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in Expenses.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    StiText dataText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (Expenses.Columns.Count > 2)
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }
                    }
                    else
                    {
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    }

                    if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                    {
                        dataText.Text.Value = "{ExpensesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.HorAlignment = StiTextHorAlignment.Left;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }
                    else
                    {
                        dataText.Text.Value = "{ExpensesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }

                    dataText.VertAlignment = StiVertAlignment.Center;
                    dataText.Name = "DataText" + nameIndex.ToString();
                    dataText.Border.Style = StiPenStyle.Solid;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.Top;
                    dataText.WordWrap = true;
                    dataText.Margins = new StiMargins(0, 1, 0, 0);

                    dataBand.Components.Add(dataText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }
        }

        if (NetProfit.Rows.Count > 0)
        {
            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "NetProfitTotal";
            dataBand.Height = 0.3;
            dataBand.Name = "NetProfitTotal";
            dataBand.Border.Style = StiPenStyle.None;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in Expenses.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    StiText dataText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (Expenses.Columns.Count > 2)
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }
                    }
                    else
                    {
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    }

                    if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                    {
                        if (dataColumn.ColumnName == "Acct")
                        {
                            dataText.Text.Value = _netText;
                        }
                        else
                        {
                            dataText.Text.Value = "{NetProfitTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        }

                        dataText.HorAlignment = StiTextHorAlignment.Left;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }
                    else
                    {
                        dataText.Text.Value = "{NetProfitTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }

                    dataText.VertAlignment = StiVertAlignment.Center;
                    dataText.Name = "DataText" + nameIndex.ToString();
                    dataText.Border.Style = StiPenStyle.Double;
                    dataText.Border.Side = StiBorderSides.Bottom;
                    dataText.OnlyText = false;
                    dataText.WordWrap = true;
                    dataText.Margins = new StiMargins(0, 1, 0, 0);

                    dataBand.Components.Add(dataText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }
        }

        if (OtherIncome.Rows.Count > 0)
        {
            titleBand3.Height = 0.2;
            titleBand3.Name = String.Empty;
            titleBand3.Name = "OtherIncome";
            titleBand3.Brush = new StiSolidBrush(Color.White);
            page.Components.Add(titleBand3);

            StiText headerText = new StiText(new RectangleD(0, 0, page.Width, 0.2));
            headerText.Text = String.Empty;
            headerText.Text = "Other Income (Expenses)";
            headerText.HorAlignment = StiTextHorAlignment.Left;
            headerText.VertAlignment = StiVertAlignment.Center;
            headerText.Name = "OtherIncome";
            headerText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            headerText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerText.TextBrush = new StiSolidBrush(Color.White);
            titleBand3.Components.Add(headerText);

            //Create HeaderBand
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.2;
            headerBand.Name = "HeaderBand";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            page.Components.Add(headerBand);

            //Create GroupHeaderBand
            if (isWithSub)
            {
                StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                groupHeader.Name = "OtherIncomeGroupHeaderBand";
                groupHeader.PrintOnAllPages = false;
                groupHeader.Condition = new StiGroupConditionExpression("{OtherIncome.Sub}");
                page.Components.Add(groupHeader);

                StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                groupHeaderText.Text.Value = "{OtherIncome.Sub}";
                groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                groupHeaderText.VertAlignment = StiVertAlignment.Center;
                groupHeaderText.Font = new Font("Arial", 8F, FontStyle.Bold);
                groupHeaderText.Border.Style = StiPenStyle.None;
                groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                groupHeaderText.WordWrap = true;
                groupHeader.Components.Add(groupHeaderText);
            }

            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "OtherIncome";
            dataBand.Height = 0.2;
            dataBand.Name = "OtherIncome";
            dataBand.Border.Style = StiPenStyle.None;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in OtherIncome.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    //Create text on header
                    StiText hText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (OtherIncome.Columns.Count > 2)
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                        }
                        else
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        }

                        hText.HorAlignment = StiTextHorAlignment.Left;
                    }
                    else
                    {
                        hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        hText.HorAlignment = StiTextHorAlignment.Right;
                    }

                    hText.Text.Value = dataColumn.ColumnName;
                    hText.VertAlignment = StiVertAlignment.Center;
                    hText.Name = "HeaderText" + nameIndex.ToString();
                    hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                    hText.Border.Side = StiBorderSides.All;
                    hText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
                    hText.Border.Style = StiPenStyle.None;
                    hText.TextBrush = new StiSolidBrush(Color.White);
                    hText.WordWrap = true;
                    headerBand.Components.Add(hText);

                    StiText dataText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (OtherIncome.Columns.Count > 2)
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                        }

                        dataText.HorAlignment = StiTextHorAlignment.Left;
                    }
                    else
                    {
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                    }

                    dataText.Text.Value = "{OtherIncome." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                    dataText.VertAlignment = StiVertAlignment.Center;
                    dataText.Name = "DataText" + nameIndex.ToString();
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.Font = new System.Drawing.Font("Arial", 8F);
                    dataText.WordWrap = true;
                    dataText.Margins = new StiMargins(0, 1, 0, 0);
                    dataBand.Components.Add(dataText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }

            //Create GroupFooterBand
            if (isWithSub)
            {
                pos = 0;

                StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.25));
                groupFooter.Name = "OtherIncomeGroupFooterBand";
                page.Components.Add(groupFooter);

                foreach (DataColumn dataColumn in OtherIncome.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        //GroupFooterBand data
                        StiText footerText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (Revenues.Columns.Count > 2)
                            {
                                footerText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }

                            footerText.Text.Value = "Total {OtherIncome.Sub}";
                            footerText.HorAlignment = StiTextHorAlignment.Left;
                            footerText.Border.Style = StiPenStyle.None;
                        }
                        else
                        {
                            footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            footerText.HorAlignment = StiTextHorAlignment.Right;
                            footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            footerText.Text.Value = "{Sum(OtherIncome." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + ")}";
                            footerText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        }

                        footerText.VertAlignment = StiVertAlignment.Center;
                        footerText.Font = new Font("Arial", 8F, FontStyle.Bold);
                        footerText.TextBrush = new StiSolidBrush(Color.Black);
                        footerText.WordWrap = true;
                        groupFooter.Components.Add(footerText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }
                    }
                }
            }
        }

        if (OtherIncomeTotal.Rows.Count > 0)
        {
            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "OtherIncomeTotal";
            dataBand.Height = 0.25;
            dataBand.Name = "OtherIncomeTotal";
            dataBand.Border.Style = StiPenStyle.Solid;
            dataBand.Border.Side = StiBorderSides.Bottom;
            dataBand.Border.Side = StiBorderSides.Top;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in OtherIncome.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    StiText dataText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (OtherIncome.Columns.Count > 2)
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }
                    }
                    else
                    {
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    }

                    if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                    {
                        dataText.Text.Value = "{OtherIncomeTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.HorAlignment = StiTextHorAlignment.Left;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }
                    else
                    {
                        dataText.Text.Value = "{OtherIncomeTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }

                    dataText.VertAlignment = StiVertAlignment.Center;
                    dataText.Name = "DataText" + nameIndex.ToString();
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.WordWrap = true;
                    dataText.Margins = new StiMargins(0, 1, 0, 0);
                    dataBand.Components.Add(dataText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }
        }

        if (BeforeProvisions.Rows.Count > 0)
        {
            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "BeforeProvisions";
            dataBand.Height = 0.3;
            dataBand.Name = "BeforeProvisions";
            dataBand.Border.Style = StiPenStyle.Solid;
            dataBand.Border.Side = StiBorderSides.Bottom;
            dataBand.Border.Side = StiBorderSides.Top;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in OtherIncome.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    StiText dataText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (OtherIncome.Columns.Count > 2)
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }
                    }
                    else
                    {
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    }

                    if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                    {
                        dataText.Text.Value = "{BeforeProvisions." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.HorAlignment = StiTextHorAlignment.Left;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }
                    else
                    {
                        dataText.Text.Value = "{BeforeProvisions." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }

                    dataText.VertAlignment = StiVertAlignment.Center;
                    dataText.Name = "DataText" + nameIndex.ToString();
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.WordWrap = true;
                    dataText.Margins = new StiMargins(0, 1, 0, 0);

                    dataBand.Components.Add(dataText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }
        }

        if (IncomeTaxes.Rows.Count > 0)
        {
            titleBand4.Height = 0.2;
            titleBand4.Name = String.Empty;
            titleBand4.Name = "IncomeTaxes";
            titleBand4.Brush = new StiSolidBrush(Color.White);
            page.Components.Add(titleBand4);

            StiText headerText = new StiText(new RectangleD(0, 0, page.Width, 0.2));
            headerText.Text = "Provisions for Income Taxes";
            headerText.HorAlignment = StiTextHorAlignment.Left;
            headerText.VertAlignment = StiVertAlignment.Center;
            headerText.Name = "IncomeTaxes";
            headerText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            headerText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerText.TextBrush = new StiSolidBrush(Color.White);
            titleBand4.Components.Add(headerText);

            //Create HeaderBand
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.2;
            headerBand.Name = "HeaderBand";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            page.Components.Add(headerBand);

            //Create GroupHeaderBand
            if (isWithSub)
            {
                StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
                groupHeader.Name = "IncomeTaxesGroupHeaderBand";
                groupHeader.PrintOnAllPages = false;
                groupHeader.Condition = new StiGroupConditionExpression("{IncomeTaxes.Sub}");
                page.Components.Add(groupHeader);

                StiText groupHeaderText = new StiText(new RectangleD(0, 0, page.Width, 0.25));
                groupHeaderText.Text.Value = "{IncomeTaxes.Sub}";
                groupHeaderText.HorAlignment = StiTextHorAlignment.Left;
                groupHeaderText.VertAlignment = StiVertAlignment.Center;
                groupHeaderText.Font = new Font("Arial", 8F, FontStyle.Bold);
                groupHeaderText.Border.Style = StiPenStyle.None;
                groupHeaderText.TextBrush = new StiSolidBrush(Color.Black);
                groupHeaderText.WordWrap = true;
                groupHeader.Components.Add(groupHeaderText);
            }

            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "IncomeTaxes";
            dataBand.Height = 0.2;
            dataBand.Name = "IncomeTaxes";
            dataBand.Border.Style = StiPenStyle.None;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in IncomeTaxes.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    //Create text on header
                    StiText hText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (IncomeTaxes.Columns.Count > 2)
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                        }
                        else
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        }

                        hText.HorAlignment = StiTextHorAlignment.Left;
                    }
                    else
                    {
                        hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        hText.HorAlignment = StiTextHorAlignment.Right;
                    }

                    hText.Text.Value = dataColumn.ColumnName;
                    hText.VertAlignment = StiVertAlignment.Center;
                    hText.Name = "HeaderText" + nameIndex.ToString();
                    hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                    hText.Border.Side = StiBorderSides.All;
                    hText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
                    hText.Border.Style = StiPenStyle.None;
                    hText.TextBrush = new StiSolidBrush(Color.White);
                    hText.WordWrap = true;
                    headerBand.Components.Add(hText);

                    StiText dataText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (IncomeTaxes.Columns.Count > 2)
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                        }

                        dataText.HorAlignment = StiTextHorAlignment.Left;
                    }
                    else
                    {
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                    }

                    dataText.Text.Value = "{IncomeTaxes." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                    dataText.VertAlignment = StiVertAlignment.Center;
                    dataText.Name = "DataText" + nameIndex.ToString();
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.Font = new System.Drawing.Font("Arial", 8F);
                    dataText.WordWrap = true;
                    dataText.Margins = new StiMargins(0, 1, 0, 0);
                    dataBand.Components.Add(dataText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }

            //Create GroupFooterBand
            if (isWithSub)
            {
                pos = 0;

                StiGroupFooterBand groupFooter = new StiGroupFooterBand(new RectangleD(0, 0, page.Width, 0.25));
                groupFooter.Name = "IncomeTaxesGroupFooterBand";
                page.Components.Add(groupFooter);

                foreach (DataColumn dataColumn in IncomeTaxes.Columns)
                {
                    if (dataColumn.ColumnName != "Sub")
                    {
                        //GroupFooterBand data
                        StiText footerText = null;
                        if (dataColumn.ColumnName == "Acct")
                        {
                            if (Revenues.Columns.Count > 2)
                            {
                                footerText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                            }
                            else
                            {
                                footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            }

                            footerText.Text.Value = "Total {IncomeTaxes.Sub}";
                            footerText.HorAlignment = StiTextHorAlignment.Left;
                            footerText.Border.Style = StiPenStyle.None;
                        }
                        else
                        {
                            footerText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                            footerText.HorAlignment = StiTextHorAlignment.Right;
                            footerText.Border = new StiBorder(StiBorderSides.Top, Color.Black, 1, StiPenStyle.Solid);
                            footerText.Text.Value = "{Sum(IncomeTaxes." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + ")}";
                            footerText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        }

                        footerText.VertAlignment = StiVertAlignment.Center;
                        footerText.Font = new Font("Arial", 8F, FontStyle.Bold);
                        footerText.TextBrush = new StiSolidBrush(Color.Black);
                        footerText.WordWrap = true;
                        groupFooter.Components.Add(footerText);

                        if (dataColumn.ColumnName == "Acct")
                        {
                            pos = pos + (columnWidth * 2);
                        }
                        else
                        {
                            pos = pos + (columnWidth);
                        }
                    }
                }
            }
        }

        if (IncomeTaxesTotal.Rows.Count > 0)
        {
            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "IncomeTaxesTotal";
            dataBand.Height = 0.25;
            dataBand.Name = "IncomeTaxesTotal";
            dataBand.Border.Style = StiPenStyle.Solid;
            dataBand.Border.Side = StiBorderSides.Bottom;
            dataBand.Border.Side = StiBorderSides.Top;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in IncomeTaxes.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    StiText dataText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (IncomeTaxes.Columns.Count > 2)
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }
                    }
                    else
                    {
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    }

                    if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                    {
                        dataText.Text.Value = "{IncomeTaxesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.HorAlignment = StiTextHorAlignment.Left;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }
                    else
                    {
                        dataText.Text.Value = "{IncomeTaxesTotal" +
                            "." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }

                    dataText.VertAlignment = StiVertAlignment.Center;
                    dataText.Name = "DataText" + nameIndex.ToString();
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.WordWrap = true;
                    dataText.Margins = new StiMargins(0, 1, 0, 0);
                    dataBand.Components.Add(dataText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }
        }

        if (NetIncome.Rows.Count > 0)
        {
            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "NetIncome";
            dataBand.Height = 0.25;
            dataBand.Name = "NetIncome";
            dataBand.Border.Style = StiPenStyle.Double;
            dataBand.Border.Side = StiBorderSides.Bottom | StiBorderSides.Top;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in IncomeTaxes.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    StiText dataText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (IncomeTaxes.Columns.Count > 2)
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }
                    }
                    else
                    {
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    }

                    if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                    {
                        dataText.Text.Value = "{NetIncome." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.HorAlignment = StiTextHorAlignment.Left;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }
                    else
                    {
                        dataText.Text.Value = "{NetIncome." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }

                    dataText.VertAlignment = StiVertAlignment.Center;
                    dataText.Name = "DataText" + nameIndex.ToString();
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.WordWrap = true;
                    dataText.Margins = new StiMargins(0, 1, 0, 0);

                    dataBand.Components.Add(dataText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }
        }

        report.Render();

        return report;
    }

    private StiReport GetIncomeStatementWithCentersBudgetsSummaryReport()
    {
        StiReport report = new StiReport();
        string reportPath = Server.MapPath("StimulsoftReports/IncomeStatementCentersBudgetsReport.mrt");
        report.Load(reportPath);
        report.Compile();

        // Add Departments into DataSource
        objPropUser.ConnConfig = Session["config"].ToString();
        var centers = _objBLReport.GetCenterNames(objPropUser);
        if (centers != null && centers.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in centers.Tables[0].Rows)
            {
                var centralName = dr["CentralName"].ToString();
                report.Dictionary.DataSources["Revenues"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["RevenuesTotal"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["CostOfSales"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["CostOfSalesTotal"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["Expenses"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["ExpensesTotal"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["GrossProfit"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["NetProfitTotal"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["OtherIncome"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["OtherIncomeTotal"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["IncomeTaxes"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["IncomeTaxesTotal"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["BeforeProvisions"].Columns.Add(centralName, typeof(double));
                report.Dictionary.DataSources["NetIncome"].Columns.Add(centralName, typeof(double));
            }
        }

        _objChart.ConnConfig = Session["config"].ToString();

        #region Start-End date

        if (!string.IsNullOrEmpty(Request.QueryString["startDate"]))
        {
            _objChart.StartDate = Convert.ToDateTime(Request.QueryString["startDate"]);
        }
        else
        {
            int year = DateTime.Now.Year;
            _objChart.StartDate = new DateTime(year, 1, 1);
        }

        if (!string.IsNullOrEmpty(Request.QueryString["endDate"]))
        {
            _objChart.EndDate = Convert.ToDateTime(Request.QueryString["endDate"]);
        }
        else
        {
            _objChart.EndDate = DateTime.Now.Date;
        }

        #endregion

        string _FromDate = "From : " + _objChart.StartDate.ToString("MMMM dd, yyyy");
        string _ToDate = "To :    " + _objChart.EndDate.ToString("MMMM dd, yyyy");

        #region Set Header

        DataSet dsC = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        dsC = objBL_User.getControl(objPropUser);

        #endregion

        _objChart.ConnConfig = Session["config"].ToString();

        if (!string.IsNullOrEmpty(Request.QueryString["departments"]))
        {
            _objChart.Departments = Request.QueryString["departments"].Trim();
        }

        if (!string.IsNullOrEmpty(Request.QueryString["budgets"]))
        {
            _objChart.Budgets = Request.QueryString["budgets"].Trim();
        }

        _objChart.Periods = GetPeriods(_objChart.StartDate, _objChart.EndDate);

        DataSet _dsIncome = _objBLReport.GetIncomeStatementDetailsWithCentersAndBudgets(_objChart);
        string _netAmount = GetNetAmount(_dsIncome.Tables[0]).ToString();

        _dsIncome.Tables[0].AcceptChanges();
        _dsIncome = ProcessAndBuildDataForCentersAndBudgets(_dsIncome);

        DataTable fTable = new DataTable();
        fTable = _dsIncome.Tables[0].Copy();
        DataSet finalDs = new DataSet();
        fTable.TableName = "Centers";
        finalDs.Tables.Add(fTable);
        finalDs.DataSetName = "Centers";

        DataSet _dsSummary = ProcessAndBuildSummaryRowForCentersAndBudgets(_dsIncome);

        DataTable RevenuesTotal = new DataTable();
        DataTable CostOfSalesTotal = new DataTable();
        DataTable ExpensesTotal = new DataTable();
        DataTable GrossTotal = new DataTable();
        DataTable NetProfit = new DataTable();
        DataTable OtherIncomeTotal = new DataTable();
        DataTable IncomeTaxesTotal = new DataTable();
        DataTable BeforeProvisions = new DataTable();
        DataTable NetIncome = new DataTable();

        foreach (DataTable dt in _dsSummary.Tables)
        {
            DataTable dtSummary = dt.Copy();

            if (dt.TableName == "RevenuesTotal")
            {
                RevenuesTotal = dtSummary;
            }
            else if (dt.TableName == "CostOfSalesTotal")
            {
                CostOfSalesTotal = dtSummary;
            }
            else if (dt.TableName == "ExpensesTotal")
            {
                ExpensesTotal = dtSummary;
            }
            else if (dt.TableName == "GrossProfit")
            {
                GrossTotal = dtSummary;
            }
            else if (dt.TableName == "NetProfitTotal")
            {
                NetProfit = dtSummary;
            }
            else if (dt.TableName == "OtherIncomeTotal")
            {
                OtherIncomeTotal = dtSummary;
            }
            else if (dt.TableName == "IncomeTaxesTotal")
            {
                IncomeTaxesTotal = dtSummary;
            }
            else if (dt.TableName == "BeforeProvisions")
            {
                BeforeProvisions = dtSummary;
            }
            else if (dt.TableName == "NetIncome")
            {
                NetIncome = dtSummary;
            }
        }

        // Filter by type 
        DataTable filteredTable = finalDs.Tables[0].Copy();
        filteredTable.Columns.Remove("Acctnumber");
        filteredTable.Columns.Remove("AcctName");
        filteredTable.Columns.Remove("AnnualTotal");
        filteredTable.Columns.Remove("TypeName");
        filteredTable.Columns.Remove("Url");
        filteredTable.Columns.Remove("fDesc");

        DataView dView = filteredTable.DefaultView;
        dView.RowFilter = "Type = 3";
        DataTable Revenues = dView.ToTable();
        Revenues.Columns.Remove("Type");

        dView.RowFilter = "Type = 4";
        DataTable CostOfSales = dView.ToTable();
        CostOfSales.Columns.Remove("Type");

        dView.RowFilter = "Type = 5";
        DataTable Expenses = dView.ToTable();
        Expenses.Columns.Remove("Type");

        dView.RowFilter = "Type = 8";
        DataTable OtherIncome = dView.ToTable();
        OtherIncome.Columns.Remove("Type");

        dView.RowFilter = "Type = 9";
        DataTable IncomeTaxes = dView.ToTable();
        IncomeTaxes.Columns.Remove("Type");

        string _netText = "Net Income";
        if (OtherIncome.Rows.Count > 0 || IncomeTaxes.Rows.Count > 0)
        {
            _netText = "Income From Operations";
        }

        report.RegData("Revenues", Revenues);
        report.RegData("CostOfSales", CostOfSales);
        report.RegData("Expenses", Expenses);
        report.RegData("OtherIncome", OtherIncome);
        report.RegData("IncomeTaxes", IncomeTaxes);
        report.RegData("RevenuesTotal", RevenuesTotal);
        report.RegData("CostOfSalesTotal", CostOfSalesTotal);
        report.RegData("ExpensesTotal", ExpensesTotal);
        report.RegData("GrossProfit", GrossTotal);
        report.RegData("NetProfitTotal", NetProfit);
        report.RegData("OtherIncomeTotal", OtherIncomeTotal);
        report.RegData("IncomeTaxesTotal", IncomeTaxesTotal);
        report.RegData("BeforeProvisions", BeforeProvisions);
        report.RegData("NetIncome", NetIncome);
        report.RegData("dsCompany", dsC.Tables[0]);

        report.Dictionary.Variables["paramUsername"].Value = Session["Username"].ToString();
        report.Dictionary.Variables["paramSDate"].Value = _FromDate;
        report.Dictionary.Variables["paramEDate"].Value = _ToDate;

        StiPage page = report.Pages[0];
        StiText txt = report.GetComponentByName("ReportHeader") as StiText;
        StiHeaderBand titleBand = new StiHeaderBand();
        StiHeaderBand titleBand1 = new StiHeaderBand();
        StiHeaderBand titleBand2 = new StiHeaderBand();
        StiHeaderBand titleBand3 = new StiHeaderBand();
        StiHeaderBand titleBand4 = new StiHeaderBand();

        if (Revenues.Rows.Count > 0)
        {
            titleBand.Height = 0.2;
            titleBand.Name = "Revenues";
            titleBand.Brush = new StiSolidBrush(Color.White);
            page.Components.Add(titleBand);

            //Create Title text on header
            StiText headerText = new StiText(new RectangleD(0, 0, page.Width, 0.2));
            headerText.Text = "Revenues";
            headerText.HorAlignment = StiTextHorAlignment.Left;
            headerText.VertAlignment = StiVertAlignment.Center;
            headerText.Name = "Revenues";
            headerText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            headerText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerText.TextBrush = new StiSolidBrush(Color.White);
            titleBand.Components.Add(headerText);

            //Create HeaderBand
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.2;
            headerBand.Name = "HeaderBand";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            page.Components.Add(headerBand);

            //Create GroupHeaderBand
            StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
            groupHeader.Name = "RevenuesGroupHeaderBand";
            groupHeader.PrintOnAllPages = false;
            groupHeader.Condition = new StiGroupConditionExpression("{Revenues.Sub}");
            page.Components.Add(groupHeader);

            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "Revenues";
            dataBand.Height = 0;
            dataBand.Name = "dbRevenues";
            dataBand.Border.Style = StiPenStyle.None;
            page.Components.Add(dataBand);

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in Revenues.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    //Create text on header
                    StiText hText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (Revenues.Columns.Count > 2)
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                        }
                        else
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        }

                        hText.HorAlignment = StiTextHorAlignment.Left;
                    }
                    else
                    {
                        hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        hText.HorAlignment = StiTextHorAlignment.Right;
                    }

                    hText.Text.Value = dataColumn.ColumnName;
                    hText.VertAlignment = StiVertAlignment.Center;
                    hText.Name = "HeaderText" + nameIndex.ToString();
                    hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                    hText.Border.Side = StiBorderSides.All;
                    hText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
                    hText.Border.Style = StiPenStyle.None;
                    hText.TextBrush = new StiSolidBrush(Color.White);
                    hText.WordWrap = true;
                    headerBand.Components.Add(hText);

                    StiText groupText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (Revenues.Columns.Count > 2)
                        {
                            groupText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                        }
                        else
                        {
                            groupText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }

                        groupText.Text.Value = "{Revenues.Sub}";
                        groupText.HorAlignment = StiTextHorAlignment.Left;
                    }
                    else
                    {
                        groupText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        groupText.HorAlignment = StiTextHorAlignment.Right;
                        groupText.Text.Value = "{Sum(Revenues." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + ")}";
                        groupText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                    }

                    groupText.VertAlignment = StiVertAlignment.Center;
                    groupText.Font = new Font("Arial", 8F, FontStyle.Bold);
                    groupText.Border.Style = StiPenStyle.None;
                    groupText.TextBrush = new StiSolidBrush(Color.Black);
                    groupText.WordWrap = true;
                    groupHeader.Components.Add(groupText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }
        }

        if (RevenuesTotal.Rows.Count > 0)
        {
            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "RevenuesTotal";
            dataBand.Height = 0.25;

            dataBand.Name = "RevenuesTotal";
            dataBand.Border.Style = StiPenStyle.Solid;
            dataBand.Border.Side = StiBorderSides.Bottom;
            dataBand.Border.Side = StiBorderSides.Top;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];
            page.CanGrow = true;
            page.CanShrink = true;

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in Revenues.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    StiText dataText = null;

                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (Revenues.Columns.Count > 2)
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }
                    }
                    else
                    {
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    }

                    if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                    {
                        dataText.Text.Value = "{RevenuesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.HorAlignment = StiTextHorAlignment.Left;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }
                    else
                    {
                        dataText.Text.Value = "{RevenuesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }

                    dataText.VertAlignment = StiVertAlignment.Center;
                    dataText.Name = "DataText" + nameIndex.ToString();
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.WordWrap = true;
                    dataText.Margins = new StiMargins(0, 1, 0, 0);

                    dataBand.Components.Add(dataText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }
        }

        if (CostOfSales.Rows.Count > 0)
        {
            titleBand1.Height = 0.2;
            titleBand1.Name = String.Empty;
            titleBand1.Name = "Cost Of Sales";
            titleBand1.Brush = new StiSolidBrush(Color.White);
            page.Components.Add(titleBand1);

            StiText headerText = new StiText(new RectangleD(0, 0, page.Width, 0.2));
            headerText.Text = String.Empty;
            headerText.Text = "Cost Of Sales";
            headerText.HorAlignment = StiTextHorAlignment.Left;
            headerText.VertAlignment = StiVertAlignment.Center;
            headerText.Name = "CostOfSales";
            headerText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            headerText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerText.TextBrush = new StiSolidBrush(Color.White);
            titleBand1.Components.Add(headerText);

            //Create HeaderBand
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.2;
            headerBand.Name = "HeaderBand";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            page.Components.Add(headerBand);

            //Create GroupHeaderBand
            StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
            groupHeader.Name = "CostOfSalesGroupHeaderBand";
            groupHeader.PrintOnAllPages = false;
            groupHeader.Condition = new StiGroupConditionExpression("{CostOfSales.Sub}");
            page.Components.Add(groupHeader);

            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "CostOfSales";
            dataBand.Height = 0;
            dataBand.Name = "CostOfSales";
            dataBand.Border.Style = StiPenStyle.None;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in CostOfSales.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    //Create text on header
                    StiText hText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (CostOfSales.Columns.Count > 2)
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                        }
                        else
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        }

                        hText.HorAlignment = StiTextHorAlignment.Left;
                    }
                    else
                    {
                        hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        hText.HorAlignment = StiTextHorAlignment.Right;
                    }

                    hText.Text.Value = dataColumn.ColumnName;
                    hText.VertAlignment = StiVertAlignment.Center;
                    hText.Name = "HeaderText" + nameIndex.ToString();
                    hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                    hText.Border.Side = StiBorderSides.All;
                    hText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
                    hText.Border.Style = StiPenStyle.None;
                    hText.TextBrush = new StiSolidBrush(Color.White);
                    hText.WordWrap = true;
                    headerBand.Components.Add(hText);

                    StiText groupText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (Revenues.Columns.Count > 2)
                        {
                            groupText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                        }
                        else
                        {
                            groupText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }

                        groupText.Text.Value = "{CostOfSales.Sub}";
                        groupText.HorAlignment = StiTextHorAlignment.Left;
                    }
                    else
                    {
                        groupText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        groupText.HorAlignment = StiTextHorAlignment.Right;
                        groupText.Text.Value = "{Sum(CostOfSales." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + ")}";
                        groupText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                    }

                    groupText.VertAlignment = StiVertAlignment.Center;
                    groupText.Font = new Font("Arial", 8F, FontStyle.Bold);
                    groupText.Border.Style = StiPenStyle.None;
                    groupText.TextBrush = new StiSolidBrush(Color.Black);
                    groupText.WordWrap = true;
                    groupHeader.Components.Add(groupText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }
        }

        if (CostOfSalesTotal.Rows.Count > 0)
        {
            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "CostOfSalesTotal";
            dataBand.Height = 0.25;
            dataBand.Name = "CostOfSalesTotal";
            dataBand.Border.Style = StiPenStyle.Solid;
            dataBand.Border.Side = StiBorderSides.Bottom;
            dataBand.Border.Side = StiBorderSides.Top;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];
            page.CanGrow = true;
            page.CanShrink = true;

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in CostOfSales.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    StiText dataText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (CostOfSales.Columns.Count > 2)
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }
                    }
                    else
                    {
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    }

                    if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                    {
                        dataText.Text.Value = "{CostOfSalesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.HorAlignment = StiTextHorAlignment.Left;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }
                    else
                    {
                        dataText.Text.Value = "{CostOfSalesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }

                    dataText.VertAlignment = StiVertAlignment.Center;
                    dataText.Name = "DataText" + nameIndex.ToString();
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.WordWrap = true;
                    dataText.Margins = new StiMargins(0, 1, 0, 0);
                    dataBand.Components.Add(dataText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }
        }

        if (GrossTotal.Rows.Count > 0)
        {
            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "GrossProfit";
            dataBand.Height = 0.3;
            dataBand.Name = "GrossProfit";
            dataBand.Border.Style = StiPenStyle.Solid;
            dataBand.Border.Side = StiBorderSides.Bottom;
            dataBand.Border.Side = StiBorderSides.Top;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];
            page.CanGrow = true;
            page.CanShrink = true;

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in CostOfSales.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    StiText dataText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (CostOfSales.Columns.Count > 2)
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }
                    }
                    else
                    {
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    }

                    if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                    {
                        dataText.Text.Value = "{GrossProfit." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.HorAlignment = StiTextHorAlignment.Left;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }
                    else
                    {
                        dataText.Text.Value = "{GrossProfit." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }

                    dataText.VertAlignment = StiVertAlignment.Center;
                    dataText.Name = "DataText" + nameIndex.ToString();
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.WordWrap = true;
                    dataText.Margins = new StiMargins(0, 1, 0, 0);

                    dataBand.Components.Add(dataText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }
        }

        if (Expenses.Rows.Count > 0)
        {
            titleBand2.Height = 0.2;
            titleBand2.Name = String.Empty;
            titleBand2.Name = "Expenses";
            titleBand2.Brush = new StiSolidBrush(Color.White);
            page.Components.Add(titleBand2);

            StiText headerText = new StiText(new RectangleD(0, 0, page.Width, 0.2));
            headerText.Text = String.Empty;
            headerText.Text = "Expenses";
            headerText.HorAlignment = StiTextHorAlignment.Left;
            headerText.VertAlignment = StiVertAlignment.Center;
            headerText.Name = "Expenses";
            headerText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            headerText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerText.TextBrush = new StiSolidBrush(Color.White);
            titleBand2.Components.Add(headerText);

            //Create HeaderBand
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.25;
            headerBand.Name = "HeaderBand";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            page.Components.Add(headerBand);

            //Create GroupHeaderBand
            StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
            groupHeader.Name = "ExpensesGroupHeaderBand";
            groupHeader.PrintOnAllPages = false;
            groupHeader.Condition = new StiGroupConditionExpression("{Expenses.Sub}");
            page.Components.Add(groupHeader);

            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "Expenses";
            dataBand.Height = 0;
            dataBand.Name = "Expenses";
            dataBand.Border.Style = StiPenStyle.None;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in Expenses.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    //Create text on header
                    StiText hText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (Expenses.Columns.Count > 2)
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                        }
                        else
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        }

                        hText.HorAlignment = StiTextHorAlignment.Left;
                    }
                    else
                    {
                        hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        hText.HorAlignment = StiTextHorAlignment.Right;
                    }

                    hText.Text.Value = dataColumn.ColumnName;
                    hText.VertAlignment = StiVertAlignment.Center;
                    hText.Name = "HeaderText" + nameIndex.ToString();
                    hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                    hText.Border.Side = StiBorderSides.All;
                    hText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
                    hText.Border.Style = StiPenStyle.None;
                    hText.TextBrush = new StiSolidBrush(Color.White);
                    hText.WordWrap = true;
                    headerBand.Components.Add(hText);

                    StiText groupText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (Revenues.Columns.Count > 2)
                        {
                            groupText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                        }
                        else
                        {
                            groupText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }

                        groupText.Text.Value = "{Expenses.Sub}";
                        groupText.HorAlignment = StiTextHorAlignment.Left;
                    }
                    else
                    {
                        groupText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        groupText.HorAlignment = StiTextHorAlignment.Right;
                        groupText.Text.Value = "{Sum(Expenses." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + ")}";
                        groupText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                    }

                    groupText.VertAlignment = StiVertAlignment.Center;
                    groupText.Font = new Font("Arial", 8F, FontStyle.Bold);
                    groupText.Border.Style = StiPenStyle.None;
                    groupText.TextBrush = new StiSolidBrush(Color.Black);
                    groupText.WordWrap = true;
                    groupHeader.Components.Add(groupText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }
        }

        if (ExpensesTotal.Rows.Count > 0)
        {
            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "ExpensesTotal";
            dataBand.Height = 0.25;
            dataBand.Name = "ExpensesTotal";
            dataBand.Border.Style = StiPenStyle.Double;
            dataBand.Border.Side = StiBorderSides.Bottom;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];
            page.CanGrow = true;
            page.CanShrink = true;

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in Expenses.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    StiText dataText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (Expenses.Columns.Count > 2)
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }
                    }
                    else
                    {
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    }

                    if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                    {
                        dataText.Text.Value = "{ExpensesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.HorAlignment = StiTextHorAlignment.Left;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }
                    else
                    {
                        dataText.Text.Value = "{ExpensesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }

                    dataText.VertAlignment = StiVertAlignment.Center;
                    dataText.Name = "DataText" + nameIndex.ToString();
                    dataText.Border.Style = StiPenStyle.Solid;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.Top;
                    dataText.WordWrap = true;
                    dataText.Margins = new StiMargins(0, 1, 0, 0);

                    dataBand.Components.Add(dataText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }
        }

        if (NetProfit.Rows.Count > 0)
        {
            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "NetProfitTotal";
            dataBand.Height = 0.3;
            dataBand.Name = "NetProfitTotal";
            dataBand.Border.Style = StiPenStyle.None;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];
            page.CanGrow = true;
            page.CanShrink = true;

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in Expenses.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    StiText dataText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (Expenses.Columns.Count > 2)
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }
                    }
                    else
                    {
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    }

                    if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                    {
                        if (dataColumn.ColumnName == "Acct")
                        {
                            dataText.Text.Value = _netText;
                        }
                        else
                        {
                            dataText.Text.Value = "{NetProfitTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        }

                        dataText.HorAlignment = StiTextHorAlignment.Left;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }
                    else
                    {
                        dataText.Text.Value = "{NetProfitTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }

                    dataText.VertAlignment = StiVertAlignment.Center;
                    dataText.Name = "DataText" + nameIndex.ToString();
                    dataText.Border.Style = StiPenStyle.Double;
                    dataText.Border.Side = StiBorderSides.Bottom | StiBorderSides.Top;
                    dataText.OnlyText = false;
                    dataText.WordWrap = true;
                    dataText.Margins = new StiMargins(0, 1, 0, 0);

                    dataBand.Components.Add(dataText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }
        }

        if (OtherIncome.Rows.Count > 0)
        {
            titleBand3.Height = 0.2;
            titleBand3.Name = String.Empty;
            titleBand3.Name = "Other Income (Expenses)";
            titleBand3.Brush = new StiSolidBrush(Color.White);
            page.Components.Add(titleBand3);

            StiText headerText = new StiText(new RectangleD(0, 0, page.Width, 0.2));
            headerText.Text = String.Empty;
            headerText.Text = "Other Income (Expenses)";
            headerText.HorAlignment = StiTextHorAlignment.Left;
            headerText.VertAlignment = StiVertAlignment.Center;
            headerText.Name = "OtherIncome";
            headerText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            headerText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerText.TextBrush = new StiSolidBrush(Color.White);
            titleBand3.Components.Add(headerText);

            //Create HeaderBand
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.2;
            headerBand.Name = "HeaderBand";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            page.Components.Add(headerBand);

            //Create GroupHeaderBand
            StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
            groupHeader.Name = "OtherIncomeGroupHeaderBand";
            groupHeader.PrintOnAllPages = false;
            groupHeader.Condition = new StiGroupConditionExpression("{OtherIncome.Sub}");
            page.Components.Add(groupHeader);

            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "OtherIncome";
            dataBand.Height = 0;
            dataBand.Name = "OtherIncome";
            dataBand.Border.Style = StiPenStyle.None;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in OtherIncome.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    //Create text on header
                    StiText hText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (OtherIncome.Columns.Count > 2)
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                        }
                        else
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        }

                        hText.HorAlignment = StiTextHorAlignment.Left;
                    }
                    else
                    {
                        hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        hText.HorAlignment = StiTextHorAlignment.Right;
                    }

                    hText.Text.Value = dataColumn.ColumnName;
                    hText.VertAlignment = StiVertAlignment.Center;
                    hText.Name = "HeaderText" + nameIndex.ToString();
                    hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                    hText.Border.Side = StiBorderSides.All;
                    hText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
                    hText.Border.Style = StiPenStyle.None;
                    hText.TextBrush = new StiSolidBrush(Color.White);
                    hText.WordWrap = true;
                    headerBand.Components.Add(hText);

                    StiText groupText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (Revenues.Columns.Count > 2)
                        {
                            groupText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                        }
                        else
                        {
                            groupText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }

                        groupText.Text.Value = "{OtherIncome.Sub}";
                        groupText.HorAlignment = StiTextHorAlignment.Left;
                    }
                    else
                    {
                        groupText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        groupText.HorAlignment = StiTextHorAlignment.Right;
                        groupText.Text.Value = "{Sum(OtherIncome." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + ")}";
                        groupText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                    }

                    groupText.VertAlignment = StiVertAlignment.Center;
                    groupText.Font = new Font("Arial", 8F, FontStyle.Bold);
                    groupText.Border.Style = StiPenStyle.None;
                    groupText.TextBrush = new StiSolidBrush(Color.Black);
                    groupText.WordWrap = true;
                    groupHeader.Components.Add(groupText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }
        }

        if (OtherIncomeTotal.Rows.Count > 0)
        {
            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "OtherIncomeTotalTotal";
            dataBand.Height = 0.25;
            dataBand.Name = "OtherIncomeTotalTotal";
            dataBand.Border.Style = StiPenStyle.Solid;
            dataBand.Border.Side = StiBorderSides.Bottom;
            dataBand.Border.Side = StiBorderSides.Top;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];
            page.CanGrow = true;
            page.CanShrink = true;

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in OtherIncome.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    StiText dataText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (OtherIncome.Columns.Count > 2)
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }
                    }
                    else
                    {
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    }

                    if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                    {
                        dataText.Text.Value = "{OtherIncomeTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.HorAlignment = StiTextHorAlignment.Left;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }
                    else
                    {
                        dataText.Text.Value = "{OtherIncomeTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }

                    dataText.VertAlignment = StiVertAlignment.Center;
                    dataText.Name = "DataText" + nameIndex.ToString();
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.WordWrap = true;
                    dataText.Margins = new StiMargins(0, 1, 0, 0);
                    dataBand.Components.Add(dataText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }
        }

        if (BeforeProvisions.Rows.Count > 0)
        {
            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "BeforeProvisions";
            dataBand.Height = 0.3;
            dataBand.Name = "BeforeProvisions";
            dataBand.Border.Style = StiPenStyle.Solid;
            dataBand.Border.Side = StiBorderSides.Bottom;
            dataBand.Border.Side = StiBorderSides.Top;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];
            page.CanGrow = true;
            page.CanShrink = true;

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in OtherIncome.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    StiText dataText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (BeforeProvisions.Columns.Count > 2)
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }
                    }
                    else
                    {
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    }

                    if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                    {
                        dataText.Text.Value = "{BeforeProvisions." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.HorAlignment = StiTextHorAlignment.Left;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }
                    else
                    {
                        dataText.Text.Value = "{BeforeProvisions." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }

                    dataText.VertAlignment = StiVertAlignment.Center;
                    dataText.Name = "DataText" + nameIndex.ToString();
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.All;
                    dataText.WordWrap = true;
                    dataText.Margins = new StiMargins(0, 1, 0, 0);

                    dataBand.Components.Add(dataText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }
        }

        if (IncomeTaxes.Rows.Count > 0)
        {
            titleBand4.Height = 0.2;
            titleBand4.Name = String.Empty;
            titleBand4.Name = "IncomeTaxes";
            titleBand4.Brush = new StiSolidBrush(Color.White);
            page.Components.Add(titleBand4);

            StiText headerText = new StiText(new RectangleD(0, 0, page.Width, 0.2));
            headerText.Text = "Provisions for Income Taxes";
            headerText.HorAlignment = StiTextHorAlignment.Left;
            headerText.VertAlignment = StiVertAlignment.Center;
            headerText.Name = "IncomeTaxes";
            headerText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            headerText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerText.TextBrush = new StiSolidBrush(Color.White);
            titleBand4.Components.Add(headerText);

            //Create HeaderBand
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.25;
            headerBand.Name = "HeaderBand";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            page.Components.Add(headerBand);

            //Create GroupHeaderBand
            StiGroupHeaderBand groupHeader = new StiGroupHeaderBand(new RectangleD(0, 0, page.Width, 0.25));
            groupHeader.Name = "IncomeTaxesGroupHeaderBand";
            groupHeader.PrintOnAllPages = false;
            groupHeader.Condition = new StiGroupConditionExpression("{IncomeTaxes.Sub}");
            page.Components.Add(groupHeader);

            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "IncomeTaxes";
            dataBand.Height = 0;
            dataBand.Name = "IncomeTaxes";
            dataBand.Border.Style = StiPenStyle.None;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in IncomeTaxes.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    //Create text on header
                    StiText hText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (IncomeTaxes.Columns.Count > 2)
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.2));
                        }
                        else
                        {
                            hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        }

                        hText.HorAlignment = StiTextHorAlignment.Left;
                    }
                    else
                    {
                        hText = new StiText(new RectangleD(pos, 0, columnWidth, 0.2));
                        hText.HorAlignment = StiTextHorAlignment.Right;
                    }

                    hText.Text.Value = dataColumn.ColumnName;
                    hText.VertAlignment = StiVertAlignment.Center;
                    hText.Name = "HeaderText" + nameIndex.ToString();
                    hText.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
                    hText.Border.Side = StiBorderSides.All;
                    hText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
                    hText.Border.Style = StiPenStyle.None;
                    hText.TextBrush = new StiSolidBrush(Color.White);
                    hText.WordWrap = true;
                    headerBand.Components.Add(hText);

                    StiText groupText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (Revenues.Columns.Count > 2)
                        {
                            groupText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                        }
                        else
                        {
                            groupText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }

                        groupText.Text.Value = "{IncomeTaxes.Sub}";
                        groupText.HorAlignment = StiTextHorAlignment.Left;
                    }
                    else
                    {
                        groupText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        groupText.HorAlignment = StiTextHorAlignment.Right;
                        groupText.Text.Value = "{Sum(IncomeTaxes." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + ")}";
                        groupText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                    }

                    groupText.VertAlignment = StiVertAlignment.Center;
                    groupText.Font = new Font("Arial", 8F, FontStyle.Bold);
                    groupText.Border.Style = StiPenStyle.None;
                    groupText.TextBrush = new StiSolidBrush(Color.Black);
                    groupText.WordWrap = true;
                    groupHeader.Components.Add(groupText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }
        }

        if (IncomeTaxesTotal.Rows.Count > 0)
        {
            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "IncomeTaxesTotal";
            dataBand.Height = 0.25;
            dataBand.Name = "IncomeTaxesTotal";
            dataBand.Border.Style = StiPenStyle.Double;
            dataBand.Border.Side = StiBorderSides.Bottom;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];
            page.CanGrow = true;
            page.CanShrink = true;

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in IncomeTaxes.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    StiText dataText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (IncomeTaxes.Columns.Count > 2)
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }
                    }
                    else
                    {
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    }

                    if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                    {
                        dataText.Text.Value = "{IncomeTaxesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.HorAlignment = StiTextHorAlignment.Left;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }
                    else
                    {
                        dataText.Text.Value = "{IncomeTaxesTotal." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }

                    dataText.VertAlignment = StiVertAlignment.Center;
                    dataText.Name = "DataText" + nameIndex.ToString();
                    dataText.Border.Style = StiPenStyle.Solid;
                    dataText.OnlyText = false;
                    dataText.Border.Side = StiBorderSides.Top;
                    dataText.WordWrap = true;
                    dataText.Margins = new StiMargins(0, 1, 0, 0);

                    dataBand.Components.Add(dataText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }
        }

        if (NetIncome.Rows.Count > 0)
        {
            //Create Databand
            StiDataBand dataBand = new StiDataBand();
            dataBand.DataSourceName = "NetIncome";
            dataBand.Height = 0.25;
            dataBand.Name = "NetIncome";
            dataBand.Border.Style = StiPenStyle.Double;
            dataBand.Border.Side = StiBorderSides.Bottom | StiBorderSides.Top;
            page.Components.Add(dataBand);
            StiDataSource dataSource = report.Dictionary.DataSources[0];
            page.CanGrow = true;
            page.CanShrink = true;

            double pos = 0;
            var columnCount = 0;
            if (Revenues.Columns.Count > 2)
            {
                columnCount = Revenues.Columns.Count;
            }
            else
            {
                columnCount = Revenues.Columns.Count + 1;
            }

            double columnWidth = page.Width / columnCount;
            int nameIndex = 1;

            foreach (DataColumn dataColumn in IncomeTaxes.Columns)
            {
                if (dataColumn.ColumnName != "Sub")
                {
                    StiText dataText = null;
                    if (dataColumn.ColumnName == "Acct")
                    {
                        if (IncomeTaxes.Columns.Count > 2)
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth * 2, 0.25));
                        }
                        else
                        {
                            dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                        }
                    }
                    else
                    {
                        dataText = new StiText(new RectangleD(pos, 0, columnWidth, 0.25));
                    }

                    if (dataColumn.ColumnName == "fDesc" || dataColumn.ColumnName == "Acct")
                    {
                        dataText.Text.Value = "{NetIncome." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.HorAlignment = StiTextHorAlignment.Left;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }
                    else
                    {
                        dataText.Text.Value = "{NetIncome." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";
                        dataText.TextFormat = new Stimulsoft.Report.Components.TextFormats.StiCurrencyFormatService();
                        dataText.HorAlignment = StiTextHorAlignment.Right;
                        dataText.Font = new System.Drawing.Font("Arial", 8F, FontStyle.Bold);
                    }

                    dataText.VertAlignment = StiVertAlignment.Center;
                    dataText.Name = "DataText" + nameIndex.ToString();
                    dataText.Border.Style = StiPenStyle.None;
                    dataText.OnlyText = false;
                    dataText.WordWrap = true;
                    dataText.Margins = new StiMargins(0, 1, 0, 0);

                    dataBand.Components.Add(dataText);

                    if (dataColumn.ColumnName == "Acct")
                    {
                        pos = pos + (columnWidth * 2);
                    }
                    else
                    {
                        pos = pos + (columnWidth);
                    }

                    nameIndex++;
                }
            }
        }

        report.Render();

        return report;
    }

    protected void StiWebViewerIncomeStatementWithCentersBudgets_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(Request.QueryString["cType"]) && Request.QueryString["cType"] == "summary")
            {
                e.Report = GetIncomeStatementWithCentersBudgetsSummaryReport();
            }
            else
            {
                e.Report = GetIncomeStatementWithCentersBudgetsReport();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void StiWebViewerIncomeStatementWithCentersBudgets_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {

    }

    #endregion

    #endregion

    #region Events

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        var cType = string.Empty;

        if (!rdCollapseAll.Checked)
        {
            if (rdDetailWithSub.Checked)
            {
                cType = "sub";
            }
            else
            {
                cType = "detail";
            }
        }
        else
        {
            cType = "summary";
        }

        if (ddlReport.SelectedValue == "0")
        {
            this.Response.Redirect("incomeStatement.aspx?reportType=" + ddlReport.SelectedValue + "&startDate=" + txtStartDate.Text + "&endDate=" + txtEndDate.Text + "&cType=" + cType + "&includeZero=" + chkIncludeZero.Checked, true);
        }
        else if (ddlReport.SelectedValue == "1")
        {
            this.Response.Redirect("incomeStatement.aspx?reportType=" + ddlReport.SelectedValue + "&yearEndingDate=" + txtYearEnd.Text + "&cType=" + cType + "&includeZero=" + chkIncludeZero.Checked, true);
        }
        else if (ddlReport.SelectedValue == "4")
        {
            this.Response.Redirect("incomeStatement.aspx?reportType=" + ddlReport.SelectedValue + "&startDate=" + txtStartDate.Text + "&endDate=" + txtEndDate.Text + "&cType=" + cType + "&departments=" + GetRadComboBoxSelectedItems(rcCenter), true);
        }
        else if (ddlReport.SelectedValue == "6")
        {
            if (rcCenter.CheckedItems.Count > 0 && !string.IsNullOrEmpty(ddlOfficeCenter.SelectedValue))
            {
                this.Response.Redirect("incomeStatement.aspx?reportType=" + ddlReport.SelectedValue + "&startDate=" + txtStartDate.Text + "&endDate=" + txtEndDate.Text + "&departments=" + GetRadComboBoxSelectedItems(rcCenter) + "&officeCenter=" + ddlOfficeCenter.SelectedValue, true);
            }
            else
            {
                StiWebViewerBudgetVsActual.Visible = false;
                StiWebViewerBudgetVsActual2.Visible = false;
                StiWebViewerIncomeStatemnet.Visible = false;
                StiWebViewerIncomeStatement12Period.Visible = false;
                StiWebViewerIncomeStatementWithCenters.Visible = false;
                StiWebViewerIncomeStatementWithCentersBudgets.Visible = false;
                StiWebViewerStandardIncomeStatementComparativeFsWithCenter.Visible = false;
                StiWebViewerProfitAndLossYTD.Visible = false;

                if (rcCenter.CheckedItems.Count == 0)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'Please select the Center!',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'Please select the Office Center!',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
        }
        else if (ddlReport.SelectedValue == "7")
        {
            this.Response.Redirect("incomeStatement.aspx?reportType=" + ddlReport.SelectedValue + "&startDate=" + txtStartDate.Text + "&endDate=" + txtEndDate.Text + "&cType=" + cType + "&includeZero=" + chkIncludeZero.Checked, true);
        }
        else if (ddlReport.SelectedValue == "8")
        {
            if (rcCenter.CheckedItems.Count > 0)
            {
                this.Response.Redirect("incomeStatement.aspx?reportType=" + ddlReport.SelectedValue + "&startDate=" + txtStartDate.Text + "&endDate=" + txtEndDate.Text + "&cType=" + cType + "&departments=" + GetRadComboBoxSelectedItems(rcCenter) + "&budgets=" + GetRadComboBoxSelectedItems(rcBudget), true);
            }
            else
            {
                StiWebViewerBudgetVsActual.Visible = false;
                StiWebViewerBudgetVsActual2.Visible = false;
                StiWebViewerIncomeStatemnet.Visible = false;
                StiWebViewerIncomeStatement12Period.Visible = false;
                StiWebViewerIncomeStatementWithCenters.Visible = false;
                StiWebViewerIncomeStatementWithCentersBudgets.Visible = false;
                StiWebViewerStandardIncomeStatementComparativeFsWithCenter.Visible = false;
                StiWebViewerProfitAndLossYTD.Visible = false;

                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'Please select the Center!',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        else
        {
            string budgetName = string.Empty;
            if (drpBudgetsList.SelectedItem != null && !string.IsNullOrEmpty(drpBudgetsList.SelectedItem.Value))
            {
                budgetName = HttpUtility.UrlEncode(drpBudgetsList.SelectedItem.Text.Trim());
            }

            if (ddlReport.SelectedValue == "3")
            {
                this.Response.Redirect("incomeStatement.aspx?reportType=" + ddlReport.SelectedValue + "&fromDate=" + txtFromDt.Text + "&toDate=" + txtToDt.Text + "&centers=" + GetRadComboBoxSelectedItems(rcCenter1) + "&budgetName=" + budgetName, true);
            }
            else if (ddlReport.SelectedValue == "2")
            {
                this.Response.Redirect("incomeStatement.aspx?reportType=" + ddlReport.SelectedValue + "&fromDate=" + txtFromDt.Text + "&toDate=" + txtToDt.Text + "&cType=" + cType + "&budgetName=" + budgetName + "&includeZero=" + chkIncludeZero.Checked, true);
            }
            else if (ddlReport.SelectedValue == "5")
            {
                this.Response.Redirect("incomeStatement.aspx?reportType=" + ddlReport.SelectedValue + "&fromDate=" + txtFromDt.Text + "&toDate=" + txtToDt.Text + "&cType=" + cType + "&budgetName=" + budgetName + "&includeZero=" + chkIncludeZero.Checked, true);
            }
        }
    }

    protected void rdExpCollAll_CheckedChanged(object sender, EventArgs e)
    {
        var cType = string.Empty;

        if (!rdCollapseAll.Checked)
        {
            if (rdDetailWithSub.Checked)
            {
                cType = "sub";
            }
            else
            {
                cType = "detail";
            }
        }
        else
        {
            cType = "summary";
        }

        if (ddlReport.SelectedValue == "0")
        {
            this.Response.Redirect("incomeStatement.aspx?reportType=" + ddlReport.SelectedValue + "&startDate=" + txtStartDate.Text + "&endDate=" + txtEndDate.Text + "&cType=" + cType + "&includeZero=" + chkIncludeZero.Checked, true);
        }
        if (ddlReport.SelectedValue == "1")
        {
            this.Response.Redirect("incomeStatement.aspx?reportType=" + ddlReport.SelectedValue + "&yearEndingDate=" + txtYearEnd.Text + "&cType=" + cType + "&includeZero=" + chkIncludeZero.Checked, true);
        }
        else if (ddlReport.SelectedValue == "4")
        {
            this.Response.Redirect("incomeStatement.aspx?reportType=" + ddlReport.SelectedValue + "&startDate=" + txtStartDate.Text + "&endDate=" + txtEndDate.Text + "&cType=" + cType + "&departments=" + GetRadComboBoxSelectedItems(rcCenter), true);
        }
        else if (ddlReport.SelectedValue == "7")
        {
            this.Response.Redirect("incomeStatement.aspx?reportType=" + ddlReport.SelectedValue + "&startDate=" + txtStartDate.Text + "&endDate=" + txtEndDate.Text + "&cType=" + cType + "&includeZero=" + chkIncludeZero.Checked, true);
        }
        else if (ddlReport.SelectedValue == "8")
        {
            this.Response.Redirect("incomeStatement.aspx?reportType=" + ddlReport.SelectedValue + "&startDate=" + txtStartDate.Text + "&endDate=" + txtEndDate.Text + "&cType=" + cType + "&departments=" + GetRadComboBoxSelectedItems(rcCenter) + "&budgets=" + GetRadComboBoxSelectedItems(rcBudget), true);
        }
        else
        {
            string budgetName = string.Empty;
            if (drpBudgetsList.SelectedItem != null && !string.IsNullOrEmpty(drpBudgetsList.SelectedItem.Value))
            {
                budgetName = HttpUtility.UrlEncode(drpBudgetsList.SelectedItem.Text.Trim());
            }

            if (ddlReport.SelectedValue == "2")
            {
                this.Response.Redirect("incomeStatement.aspx?reportType=" + ddlReport.SelectedValue + "&fromDate=" + txtFromDt.Text + "&toDate=" + txtToDt.Text + "&cType=" + cType + "&budgetName=" + budgetName + "&includeZero=" + chkIncludeZero.Checked, true);
            }
            else if (ddlReport.SelectedValue == "5")
            {
                this.Response.Redirect("incomeStatement.aspx?reportType=" + ddlReport.SelectedValue + "&fromDate=" + txtFromDt.Text + "&toDate=" + txtToDt.Text + "&cType=" + cType + "&budgetName=" + budgetName + "&includeZero=" + chkIncludeZero.Checked, true);
            }
        }
    }

    protected void drpBudgetsList_SelectedIndexChanged(object sender, EventArgs e)
    {
        var cType = string.Empty;

        if (!rdCollapseAll.Checked)
        {
            if (rdDetailWithSub.Checked)
            {
                cType = "sub";
            }
            else
            {
                cType = "detail";
            }
        }
        else
        {
            cType = "summary";
        }

        string budgetName = string.Empty;
        if (drpBudgetsList.SelectedItem != null && !string.IsNullOrEmpty(drpBudgetsList.SelectedItem.Value))
        {
            budgetName = HttpUtility.UrlEncode(drpBudgetsList.SelectedItem.Text.Trim());
        }

        if (ddlReport.SelectedValue == "3")
        {
            this.Response.Redirect("incomeStatement.aspx?reportType=" + ddlReport.SelectedValue + "&fromDate=" + txtFromDt.Text + "&toDate=" + txtToDt.Text + "&centers=" + GetRadComboBoxSelectedItems(rcCenter1) + "&budgetName=" + budgetName, true);
        }
        else if (ddlReport.SelectedValue == "2")
        {
            this.Response.Redirect("incomeStatement.aspx?reportType=" + ddlReport.SelectedValue + "&fromDate=" + txtFromDt.Text + "&toDate=" + txtToDt.Text + "&cType=" + cType + "&budgetName=" + budgetName + "&includeZero=" + chkIncludeZero.Checked, true);
        }
        else if (ddlReport.SelectedValue == "5")
        {
            this.Response.Redirect("incomeStatement.aspx?reportType=" + ddlReport.SelectedValue + "&fromDate=" + txtFromDt.Text + "&toDate=" + txtToDt.Text + "&cType=" + cType + "&budgetName=" + budgetName + "&includeZero=" + chkIncludeZero.Checked, true);
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    protected void LnkSend_Click(object sender, EventArgs e)
    {
        if (txtTo.Text.Trim() != string.Empty)
        {
            try
            {
                var fileName = $"{ddlReport.SelectedItem.Text.Replace(" ", "")}.pdf";
                DataTable cTable = (DataTable)Session["CompanyTable"];
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

                mail.Title = "Profit and Loss Statement";
                mail.Text = txtBody.Text;
                mail.FileName = fileName;
                mail.RequireAutentication = false;

                if (hdnFirstAttachement.Value != "-1")
                {
                    mail.attachmentBytes = GetReportAsAttachment();
                }

                ArrayList lst = new ArrayList();
                if (ViewState["pathmailatt"] != null)
                {
                    lst = (ArrayList)ViewState["pathmailatt"];
                    foreach (string strpath in lst)
                    {
                        if (strpath != fileName)
                        {
                            mail.AttachmentFiles.Add(strpath);
                        }
                    }
                }

                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                mail.Send();

                ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false}); ", true);

            }
            catch (Exception ex)
            {
                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }

    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {
        rcBudget.Items.Clear();
        BindingBudget();
    }

    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {
        rcBudget.Items.Clear();
        BindingBudget();
    }

    protected void txtFromDt_TextChanged(object sender, EventArgs e)
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        var monthEnd = _objBLReport.GetFiscalYearData(objPropUser);

        int startYear = DateTime.Now.Year;
        if (!string.IsNullOrEmpty(txtFromDt.Text))
        {
            var fromDate = DateTime.Parse(txtFromDt.Text);

            if (fromDate.Month > monthEnd + 1)
            {
                startYear = fromDate.Year + 1;
            }
            else
            {
                startYear = fromDate.Year;
            }
        }

        int endYear = startYear;
        if (!string.IsNullOrEmpty(txtToDt.Text))
        {
            var toDate = DateTime.Parse(txtToDt.Text);

            if (toDate.Month <= monthEnd + 1)
            {
                endYear = toDate.Year;
            }
            else
            {
                endYear = toDate.Year + 1;
            }
        }

        DataSet ds = bL_Budgets.GetBudgetsByYear(Session["config"].ToString(), startYear, endYear);
        drpBudgetsList.Items.Clear();
        drpBudgetsList.DataSource = ds;
        drpBudgetsList.DataBind();
        drpBudgetsList.DataTextField = "Budget";
        drpBudgetsList.DataValueField = "BudgetID";
        drpBudgetsList.DataBind();
        drpBudgetsList.Items.Insert(0, new ListItem("-- Select Budget --", ""));
        drpBudgetsList.Visible = true;
    }

    protected void txtToDt_TextChanged(object sender, EventArgs e)
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        var monthEnd = _objBLReport.GetFiscalYearData(objPropUser);

        int startYear = DateTime.Now.Year;
        if (!string.IsNullOrEmpty(txtFromDt.Text))
        {
            var fromDate = DateTime.Parse(txtFromDt.Text);

            if (fromDate.Month > monthEnd + 1)
            {
                startYear = fromDate.Year + 1;
            }
            else
            {
                startYear = fromDate.Year;
            }
        }

        int endYear = startYear;
        if (!string.IsNullOrEmpty(txtToDt.Text))
        {
            var toDate = DateTime.Parse(txtToDt.Text);

            if (toDate.Month <= monthEnd + 1)
            {
                endYear = toDate.Year;
            }
            else
            {
                endYear = toDate.Year + 1;
            }
        }

        DataSet ds = bL_Budgets.GetBudgetsByYear(Session["config"].ToString(), startYear, endYear);
        drpBudgetsList.Items.Clear();
        drpBudgetsList.DataSource = ds;
        drpBudgetsList.DataBind();
        drpBudgetsList.DataTextField = "Budget";
        drpBudgetsList.DataValueField = "BudgetID";
        drpBudgetsList.DataBind();
        drpBudgetsList.Items.Insert(0, new ListItem("-- Select Budget --", ""));
        drpBudgetsList.Visible = true;

    }

    #endregion

    private byte[] GetReportAsAttachment()
    {
        byte[] buffer = null;
        var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
        var service = new Stimulsoft.Report.Export.StiPdfExportService();
        System.IO.MemoryStream stream = new System.IO.MemoryStream();
        service.ExportTo(PrintStatements(true), stream, settings);
        buffer = stream.ToArray();

        return buffer;
    }

    private void SetAddress()
    {
        var address = WebBaseUtility.GetSignature();

        string mailBody = "Please review the attached Profit and Loss Report.";

        address = mailBody + Environment.NewLine + "<br />" + Environment.NewLine + "<br />" + address;

        txtBody.Text = address;

        txtSubject.Text = "Profit and Loss Report";

        
    }

    private void LoadMailContent()
    {
        txtFrom.Text = WebBaseUtility.GetFromEmailAddress();

        objGenerals.ConnConfig = Session["config"].ToString();
        objGenerals.CustomName = "Country";
        DataSet dsCustom = objBL_General.getCustomFields(objGenerals);


        string subject = string.Empty;
        DataSet dccust = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.CustomerID = Convert.ToInt32(Request.QueryString["uid"]);
        dccust = objBL_User.getCustomerForReport(objPropUser);

        objPropUser.DBName = Session["dbname"].ToString();
        DataSet dsloc = new DataSet();
        dsloc = objBL_User.getLocationByID(objPropUser);

        ViewState["subject"] = subject;

        string fileName = "ProfitandLoss.pdf";
        if (!string.IsNullOrEmpty(ddlReport.SelectedItem.Text))
        {
            fileName = $"{ddlReport.SelectedItem.Text.Replace(" ", "")}.pdf";
        }

        ArrayList lstPath = new ArrayList();
        if (ViewState["pathmailatt"] != null)
        {
            lstPath = (ArrayList)ViewState["pathmailatt"];
            lstPath.Add(fileName);
        }
        else
        {
            lstPath.Add(fileName);
        }

        ViewState["pathmailatt"] = lstPath;
        dlAttachmentsDelete.DataSource = lstPath;
        dlAttachmentsDelete.DataBind();
        hdnFirstAttachement.Value = fileName;
    }

    private double GetNetAmount(DataTable dt)
    {
        double netAmount = 0.00;
        try
        {
            if (dt.Rows.Count > 0)
            {
                double perRev = 0;
                double preCost = 0;
                double perExp = 0;
                percent = 0;
                NRev = 0;
                double revenueTotal = 0.00; double expenseTotal = 0.00;
                double costsaleTotal = 0.00;
                double grossProfit = 0.00;

                DataRow[] drI = dt.Select("Type = 3");
                if (drI.Count() > 0)
                {
                    revenueTotal = Convert.ToDouble(dt.Compute("SUM(Amount)", "Type = 3"));
                    if (revenueTotal != 0)
                    {
                        perRev = Math.Round((revenueTotal / revenueTotal) * 100 * 100) / 100;
                    }
                }

                drI = dt.Select("Type = 4");
                if (drI.Count() > 0)
                {
                    costsaleTotal = Convert.ToDouble(dt.Compute("SUM(Amount)", "Type = 4"));
                    if (revenueTotal != 0)
                    {
                        preCost = Math.Round((costsaleTotal / revenueTotal) * 100 * 100) / 100;
                    }
                }

                drI = dt.Select("Type = 5");
                if (drI.Count() > 0)
                {
                    expenseTotal = Convert.ToDouble(dt.Compute("SUM(Amount)", "Type = 5"));
                    if (revenueTotal != 0)
                    {
                        perExp = Math.Round((expenseTotal / revenueTotal) * 100 * 100) / 100;
                    }
                }
                NRev = revenueTotal;
                percent = (perRev - preCost) - perExp;
                grossProfit = revenueTotal - costsaleTotal;
                netAmount = grossProfit - expenseTotal;

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return netAmount;
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
            ServiceTypesArray[i] = value;
            i++;
        }

        var ServiceTypes = String.Join(",", ServiceTypesArray);

        return ServiceTypes;
    }

    private DataTable BuildBudgetTable()
    {
        DataTable budgetDataTable = new DataTable();
        budgetDataTable.Columns.Add("Type");
        budgetDataTable.Columns.Add("Acct");
        budgetDataTable.Columns.Add("fDesc");
        budgetDataTable.Columns.Add("AcctNumber");
        budgetDataTable.Columns.Add("AcctName");
        budgetDataTable.Columns.Add("AnnualTotal");
        budgetDataTable.Columns.Add("TypeName");
        budgetDataTable.Columns.Add("Url");
        //AddMonthColumns();
        for (int i = 1; i <= 12; i++)
        {
            var columnName = setMonth(i, true) + "-Actual";
            budgetDataTable.Columns.Add(columnName);
            columnName = setMonth(i, true) + "-Budget";
            budgetDataTable.Columns.Add(columnName);
            columnName = setMonth(i, true) + "-Difference";
            budgetDataTable.Columns.Add(columnName);
            columnName = setMonth(i, true) + "-Variance";
            budgetDataTable.Columns.Add(columnName);

        }
        budgetDataTable.Columns.Add("Total-Actual");
        budgetDataTable.Columns.Add("Total-Budget");
        budgetDataTable.Columns.Add("Total-Difference");
        budgetDataTable.Columns.Add("Total-Variance");
        budgetDataTable.Columns.Add("Clear");
        return budgetDataTable;
    }

    private DataTable BuildBudgetTableForIncomeStatement()
    {
        DataTable budgetDataTable = new DataTable();
        budgetDataTable.Columns.Add("Type");
        budgetDataTable.Columns.Add("Acct");
        budgetDataTable.Columns.Add("fDesc");
        budgetDataTable.Columns.Add("AcctNumber");
        budgetDataTable.Columns.Add("AcctName");
        budgetDataTable.Columns.Add("AnnualTotal");
        budgetDataTable.Columns.Add("TypeName");
        budgetDataTable.Columns.Add("Url");
        budgetDataTable.Columns.Add("Sub");

        for (int i = 1; i <= 12; i++)
        {
            var columnName = setMonth(i, false);
            budgetDataTable.Columns.Add(columnName);
        }

        budgetDataTable.Columns.Add("Total");
        budgetDataTable.Columns.Add("Clear");

        return budgetDataTable;
    }

    private DataTable BuildBudgetTableForStandardIncomeStatement()
    {
        DataTable budgetDataTable = new DataTable();
        budgetDataTable.Columns.Add("Type");
        budgetDataTable.Columns.Add("TypeName");
        budgetDataTable.Columns.Add("Acct");
        budgetDataTable.Columns.Add("fDesc");
        budgetDataTable.Columns.Add("Sub");
        budgetDataTable.Columns.Add("Url");
        budgetDataTable.Columns.Add("Amount");

        return budgetDataTable;
    }

    private DataSet ProcessAndBuildData(DataSet ds)
    {
        var budgetTable = BuildBudgetTable();
        var finalDs = new DataSet();
        var dt = ds.Tables[0];
        bool valueAdded = false;
        double mActual = 0.00, mBudget = 0.00, mDiff = 0.00;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            var actn = dt.Rows[i]["Acct"].ToString().Trim();
            var actNo = dt.Rows[i]["AcctNo"].ToString().Trim();
            var actName = dt.Rows[i]["AcctName"].ToString().Trim();
            var fDesc = dt.Rows[i]["fDesc"].ToString().Trim();
            var type = dt.Rows[i]["Type"].ToString().Trim();
            var typeName = dt.Rows[i]["TypeName"].ToString().Trim();
            var monthName = dt.Rows[i]["NMonth"].ToString().Trim().Substring(0, 3);
            var url = dt.Rows[i]["Url"].ToString().Trim();
            var dt1 = budgetTable.Copy();
            var dv1 = dt1.DefaultView;

            dv1.RowFilter = " AcctNumber = '"
                + dt.Rows[i]["AcctNo"].ToString().Trim() + "' AND TypeName = '"
                + dt.Rows[i]["TypeName"].ToString().Trim() + "'";
            if (dv1.ToTable().Rows.Count > 0)
            {
                valueAdded = true;
            }

            if (valueAdded)
            {
                valueAdded = false;
                continue;
            }

            var dr = budgetTable.NewRow();

            dr["Acct"] = actn;
            dr["AcctNumber"] = actNo;
            dr["AcctName"] = actName;
            dr["fDesc"] = fDesc;
            dr["Type"] = type;
            dr["TypeName"] = typeName;
            dr["Url"] = url;
            var cdt = dt.Copy();
            var dv = cdt.DefaultView;
            dv.RowFilter = "AcctNo = '" + actNo + "'";
            var filteredDataTable = dv.ToTable();

            foreach (DataRow row in filteredDataTable.Rows)
            {
                monthName = row["NMonth"].ToString().Trim().Substring(0, 3);
                dr[monthName + "-Actual"] = row["NTotal"];
                dr[monthName + "-Budget"] = row["NBudget"];
                dr[monthName + "-Difference"] = row["Difference"];
                dr[monthName + "-Variance"] = row["Variance"];

                if (!string.IsNullOrEmpty(row["NTotal"].ToString()))
                    mActual += double.Parse(row["NTotal"].ToString());
                if (!string.IsNullOrEmpty(row["NBudget"].ToString()))
                    mBudget += double.Parse(row["NBudget"].ToString());
                if (!string.IsNullOrEmpty(row["Difference"].ToString()))
                    mDiff += double.Parse(row["Difference"].ToString());
            }

            dr["Total-Actual"] = mActual;
            dr["Total-Budget"] = mBudget;
            dr["Total-Difference"] = mDiff;
            if (mBudget != 0)
            {
                dr["Total-Variance"] = mDiff / Math.Abs(mBudget);
            }
            else
            {
                dr["Total-Variance"] = 0;
            }

            mActual = mBudget = mDiff = 0.00;

            budgetTable.Rows.Add(dr);
        }

        finalDs.Tables.Add(budgetTable.DefaultView.ToTable());

        return finalDs;
    }

    private DataSet ProcessAndBuildDataForIncomeStatemnt(DataSet ds)
    {
        var budgetTable = BuildBudgetTableForIncomeStatement();
        var finalDs = new DataSet();
        var dt = ds.Tables[0];
        bool valueAdded = false;
        double mActual = 0.00;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            mActual = 0.00;
            var actn = dt.Rows[i]["Acct"].ToString().Trim();
            var fDesc = dt.Rows[i]["fDesc"].ToString().Trim();
            var type = dt.Rows[i]["Type"].ToString().Trim();
            var typeName = dt.Rows[i]["TypeName"].ToString().Trim();
            var monthName = dt.Rows[i]["NMonth"].ToString().Trim();
            var url = dt.Rows[i]["Url"].ToString().Trim();
            var sub = dt.Rows[i]["Sub"].ToString().Trim();

            var dt1 = budgetTable.Copy();
            var dv1 = dt1.DefaultView;

            dv1.RowFilter = " Acct = '"
                + dt.Rows[i]["Acct"].ToString().Trim() + "' AND TypeName = '"
                + dt.Rows[i]["TypeName"].ToString().Trim() + "'";
            if (dv1.ToTable().Rows.Count > 0)
            {
                valueAdded = true;
            }

            if (valueAdded)
            {
                valueAdded = false;
                continue;
            }

            var dr = budgetTable.NewRow();

            dr["Acct"] = actn;
            dr["fDesc"] = fDesc;
            dr["Type"] = type;
            dr["TypeName"] = typeName;
            dr["Url"] = url;
            dr["Sub"] = sub;

            var cdt = dt.Copy();
            var dv = cdt.DefaultView;
            dv.RowFilter = "Acct = '" + actn + "'";
            var filteredDataTable = dv.ToTable();

            foreach (DataRow row in filteredDataTable.Rows)
            {
                monthName = row["NMonth"].ToString().Trim();
                dr[monthName] = Convert.ToDouble(row["NTotal"].ToString());

                if (monthName != "Total")
                    mActual += double.Parse(row["NTotal"].ToString());
            }

            dr["Total"] = mActual;
            mActual = 0.00;
            budgetTable.Rows.Add(dr);
        }

        finalDs.Tables.Add(budgetTable.DefaultView.ToTable());

        return finalDs;
    }

    private DataSet ProcessAndBuildDataForIncomeStatementYTD(DataSet ds, bool isIncludeZero)
    {
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add("Acct");
        dataTable.Columns.Add("Type");
        dataTable.Columns.Add("TypeName");
        dataTable.Columns.Add("fDesc");
        dataTable.Columns.Add("Sub");
        dataTable.Columns.Add("Url");
        dataTable.Columns.Add("Month");
        dataTable.Columns.Add("YTD");

        DataTable dtTotal = ds.Tables[1];
        foreach (DataRow dr in dtTotal.Rows)
        {
            var row = dataTable.NewRow();

            row["Acct"] = dr["Acct"];
            row["Type"] = dr["Type"];
            row["TypeName"] = dr["TypeName"];
            row["fDesc"] = dr["fDesc"];
            row["Url"] = dr["Url"];
            row["Sub"] = dr["Sub"];
            row["Month"] = "0.00";
            row["YTD"] = dr["NTotal"];

            dataTable.Rows.Add(row);
        }

        DataTable dtMonth = ds.Tables[0];
        foreach (DataRow dr in dtMonth.Rows)
        {
            DataRow row = dataTable.AsEnumerable()
                            .FirstOrDefault(r => r.Field<String>("Acct") == dr["Acct"].ToString());
            if (row != null)
            {
                row["Month"] = dr["NTotal"];
            }
            else
            {
                row = dataTable.NewRow();
                row["Acct"] = dr["Acct"];
                row["Type"] = dr["Type"];
                row["TypeName"] = dr["TypeName"];
                row["fDesc"] = dr["fDesc"];
                row["Url"] = dr["Url"];
                row["Sub"] = dr["Sub"];
                row["Month"] = dr["NTotal"];
                row["YTD"] = "0.00";

                dataTable.Rows.Add(row);
            }
        }

        var dView = dataTable.DefaultView;

        if (!isIncludeZero)
        {
            dView.RowFilter = "Month <> '0.00' OR YTD <> '0.00'";
        }

        DataSet finalDataSet = new DataSet();
        dataTable.AcceptChanges();
        finalDataSet.Tables.Add(dView.ToTable());

        return finalDataSet;
    }

    private DataSet ProcessAndBuildDataForStandardIncomeStatementWithCenters(DataSet ds)
    {
        var budgetTable = BuildBudgetTableForStandardIncomeStatement();
        budgetTable.Columns.Add("Department");
        budgetTable.Columns.Add("DepartmentName");
        var finalDs = new DataSet();
        var dt = ds.Tables[0];
        bool valueAdded = false;
        var amount = 0.00;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            amount = 0.00;
            var actn = dt.Rows[i]["Acct"].ToString().Trim();
            var fDesc = dt.Rows[i]["fDesc"].ToString().Trim();
            var type = dt.Rows[i]["Type"].ToString().Trim();
            var typeName = dt.Rows[i]["TypeName"].ToString().Trim();
            var sub = dt.Rows[i]["Sub"].ToString().Trim();
            var url = dt.Rows[i]["Url"].ToString().Trim();
            var department = dt.Rows[i]["Department"].ToString().Trim();
            var departmentName = dt.Rows[i]["DepartmentName"].ToString().Trim();
            var dt1 = budgetTable.Copy();
            var dv1 = dt1.DefaultView;

            dv1.RowFilter = " Acct = '"
            + dt.Rows[i]["Acct"].ToString().Trim() + "' AND TypeName = '"
            + typeName + "'";
            if (dv1.ToTable().Rows.Count > 0)
            {
                valueAdded = true;
            }

            if (valueAdded)
            {
                valueAdded = false;
                continue;
            }

            var dr = budgetTable.NewRow();

            dr["Acct"] = actn;
            dr["fDesc"] = fDesc;
            dr["Type"] = type;
            dr["TypeName"] = typeName;
            dr["Sub"] = sub;
            dr["Url"] = url;
            dr["Department"] = department;
            dr["DepartmentName"] = departmentName;

            var cdt = dt.Copy();
            var dv = cdt.DefaultView;
            dv.RowFilter = "Department = '" + department + "'";
            var filteredDataTable = dv.ToTable();

            foreach (DataRow row in filteredDataTable.Rows)
            {
                amount += double.Parse(row["Amount"].ToString());
            }

            dr["Amount"] = amount;
            amount = 0.00;
            budgetTable.Rows.Add(dr);
        }
        finalDs.Tables.Add(budgetTable.DefaultView.ToTable());
        return finalDs;
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

    private string GetAccountType(int i)
    {
        var acctType = string.Empty;
        switch (i)
        {
            case 0:
                return "Asset";
            case 1:
                return "Liability";
            case 2:
                return "Equity";
            case 3:
                return "Revenues";
            case 4:
                return "Cost of Sales";
            case 5:
                return "Expenses";
            case 6:
                return "Bank";
            case 7:
                return "Non-Posting";
            case 8:
                return "Other Income (Expenses)";
            case 9:
                return "Provisions for Income Taxes";
            default:
                return "Total";
        }
    }

    private DataSet ProcessAndAddSummaryRows(DataSet ds)
    {
        DataView dview = new DataView();
        DataSet resultSet = new DataSet();
        decimal _variance = 0;
        if (ds != null)
        {
            DataTable dt = ds.Tables[0];
            DataTable fDt = new DataTable();
            DataTable resultTable = dt.Copy();
            DataTable finalTable = dt.Copy();
            DataView dv = dt.DefaultView;
            double actualTotal = 0.00, budgetTotal = 0.00, diff = 0.00, TactualTotal = 0.00, TbudgetTotal = 0.00, Tdiff = 0.00;
            decimal variance = 0;
            var typeName = string.Empty;

            int[] types = { 3, 4, 5, 8, 9 };

            foreach (int i in types)
            {
                var currentTypeDictionary = new Dictionary<string, double>();

                if (i == 3)
                {
                    currentTypeDictionary = totalRevenues;
                }
                else if (i == 4)
                {
                    currentTypeDictionary = totalCostOfSales;
                }
                else if (i == 5)
                {
                    currentTypeDictionary = totalExpenses;
                }
                else if (i == 8)
                {
                    currentTypeDictionary = totalOtherIncome;
                }
                else if (i == 9)
                {
                    currentTypeDictionary = totalIncomeTaxes;
                }

                dv.RowFilter = "Type = " + i.ToString();
                fDt = dv.ToTable();
                DataRow dr = finalTable.NewRow();
                dr["AcctName"] = "Total " + GetAccountType(i);
                dr["fDesc"] = "Total " + GetAccountType(i);
                dr["Type"] = -1;
                dr["TypeName"] = "Total " + GetAccountType(i);

                for (int k = 1; k <= 12; k++)
                {
                    var actualColumn = setMonth(k, true) + "-Actual";
                    var budgetColumn = setMonth(k, true) + "-Budget";
                    var diffColumn = setMonth(k, true) + "-Difference";
                    var varColumn = setMonth(k, true) + "-Variance";

                    for (int j = 0; j < fDt.Rows.Count; j++)
                    {
                        if (!string.IsNullOrEmpty(fDt.Rows[j][actualColumn].ToString()))
                            actualTotal += double.Parse(fDt.Rows[j][actualColumn].ToString());

                        if (!string.IsNullOrEmpty(fDt.Rows[j][budgetColumn].ToString()))
                            budgetTotal += double.Parse(fDt.Rows[j][budgetColumn].ToString());
                    }

                    diff = actualTotal - budgetTotal;

                    if (budgetTotal != 0)
                    {
                        if (!decimal.TryParse(((actualTotal - budgetTotal) / Math.Abs(budgetTotal)).ToString(), out variance))
                        {
                            variance = 0;
                        }
                    }
                    else
                    {
                        variance = 0;
                    }

                    dr[actualColumn] = actualTotal;
                    dr[budgetColumn] = budgetTotal;
                    dr[diffColumn] = diff;
                    dr[varColumn] = variance;


                    if (!currentTypeDictionary.ContainsKey(actualColumn))
                        currentTypeDictionary.Add(actualColumn, actualTotal);

                    if (!currentTypeDictionary.ContainsKey(budgetColumn))
                        currentTypeDictionary.Add(budgetColumn, budgetTotal);

                    if (!currentTypeDictionary.ContainsKey(diffColumn))
                        currentTypeDictionary.Add(diffColumn, diff);

                    if (!currentTypeDictionary.ContainsKey(varColumn))
                        currentTypeDictionary.Add(varColumn, double.Parse(variance.ToString()));

                    actualTotal = budgetTotal = diff = 0;
                    variance = 0;
                }

                for (int j = 0; j < fDt.Rows.Count; j++)
                {
                    if (!string.IsNullOrEmpty(fDt.Rows[j]["Total-Actual"].ToString()))
                        TactualTotal += double.Parse(fDt.Rows[j]["Total-Actual"].ToString());

                    if (!string.IsNullOrEmpty(fDt.Rows[j]["Total-Budget"].ToString()))
                        TbudgetTotal += double.Parse(fDt.Rows[j]["Total-Budget"].ToString());
                }

                dr["Total-Actual"] = TactualTotal;
                dr["Total-Budget"] = TbudgetTotal;
                dr["Total-Difference"] = TactualTotal - TbudgetTotal;
                if (TbudgetTotal != 0)
                {
                    if (!decimal.TryParse((((TactualTotal - TbudgetTotal) / Math.Abs(TbudgetTotal))).ToString(), out _variance))
                    {
                        _variance = 0;
                    }

                    dr["Total-Variance"] = _variance;
                }
                else
                {
                    dr["Total-Variance"] = 0;
                }

                if (!currentTypeDictionary.ContainsKey("Total-Actual"))
                    currentTypeDictionary.Add("Total-Actual", TactualTotal);

                if (!currentTypeDictionary.ContainsKey("Total-Budget"))
                    currentTypeDictionary.Add("Total-Budget", TbudgetTotal);

                if (!currentTypeDictionary.ContainsKey("Total-Difference"))
                    currentTypeDictionary.Add("Total-Difference", (TactualTotal - TbudgetTotal));

                if (!currentTypeDictionary.ContainsKey("Total-Variance"))
                    currentTypeDictionary.Add("Total-Variance", double.Parse(variance.ToString()));

                TactualTotal = TbudgetTotal = Tdiff = 0.00;
                finalTable.Rows.Add(dr);
            }

            finalTable = AddCalculationRows(finalTable);
            dview = finalTable.DefaultView;
            dview.Sort = "Type asc";
        }

        resultSet.Tables.Add(dview.ToTable());

        return resultSet;
    }

    private DataTable AddCalculationRows(DataTable finalTable)
    {
        double revenuesActual = 0, reveneuesBudget = 0,
           costOfSalesActual = 0, costOfSalesBudget = 0,
           expensesActual = 0, expensesBudget = 0,
           otherIncomeActual = 0, otherIncomeBudget = 0,
           incomeTaxesActual = 0, incomeTaxesBudget = 0;

        double actualGrossProfit = 0, budgetGrossProfit = 0;
        double actualNetProfit = 0, budgetNetProfit = 0;
        double actualIncomeBefore = 0, budgetIncomeBefore = 0;
        double actualNetIncome = 0, budgetNetIncome = 0;

        decimal _variance = 0;

        DataRow drGrossProfit = finalTable.NewRow();
        drGrossProfit["AcctName"] = "Gross Profit";
        drGrossProfit["fDesc"] = "Gross Profit";
        drGrossProfit["Type"] = -1;
        drGrossProfit["TypeName"] = "Gross Profit";

        DataRow drNetProfit = finalTable.NewRow();
        drNetProfit["AcctName"] = "Net Profit/Loss";
        drNetProfit["fDesc"] = "Net Profit/Loss";
        drNetProfit["Type"] = -1;
        drNetProfit["TypeName"] = "Net Profit";

        DataRow drIncomeBefore = finalTable.NewRow();
        drIncomeBefore["AcctName"] = "Income Before Provisions For Income Taxes";
        drIncomeBefore["fDesc"] = "Income Before Provisions For Income Taxes";
        drIncomeBefore["Type"] = -1;
        drIncomeBefore["TypeName"] = "Income Before Provisions";

        DataRow drNetIncome = finalTable.NewRow();
        drNetIncome["AcctName"] = "Net Income";
        drNetIncome["fDesc"] = "Net Income";
        drNetIncome["Type"] = -1;
        drNetIncome["TypeName"] = "Net Income";

        for (int i = 1; i <= 12; i++)
        {
            // Actual
            var columnName = setMonth(i, true) + "-Actual";
            revenuesActual = double.Parse(totalRevenues[columnName].ToString());
            costOfSalesActual = double.Parse(totalCostOfSales[columnName].ToString());
            expensesActual = double.Parse(totalExpenses[columnName].ToString());
            otherIncomeActual = double.Parse(totalOtherIncome[columnName].ToString());
            incomeTaxesActual = double.Parse(totalIncomeTaxes[columnName].ToString());

            actualGrossProfit = revenuesActual - costOfSalesActual;
            actualNetProfit = actualGrossProfit - expensesActual;
            actualIncomeBefore = actualNetProfit + otherIncomeActual;
            actualNetIncome = actualIncomeBefore - incomeTaxesActual;

            drGrossProfit[columnName] = actualGrossProfit;
            drNetProfit[columnName] = actualNetProfit;
            drIncomeBefore[columnName] = actualIncomeBefore;
            drNetIncome[columnName] = actualNetIncome;

            // Budget
            columnName = setMonth(i, true) + "-Budget";
            reveneuesBudget = double.Parse(totalRevenues[columnName].ToString());
            costOfSalesBudget = double.Parse(totalCostOfSales[columnName].ToString());
            expensesBudget = double.Parse(totalExpenses[columnName].ToString());
            otherIncomeBudget = double.Parse(totalOtherIncome[columnName].ToString());
            incomeTaxesBudget = double.Parse(totalIncomeTaxes[columnName].ToString());

            budgetGrossProfit = reveneuesBudget - costOfSalesBudget;
            budgetNetProfit = budgetGrossProfit - expensesBudget;
            budgetIncomeBefore = budgetNetProfit + otherIncomeBudget;
            budgetNetIncome = budgetIncomeBefore - incomeTaxesBudget;

            drGrossProfit[columnName] = budgetGrossProfit;
            drNetProfit[columnName] = budgetNetProfit;
            drIncomeBefore[columnName] = budgetIncomeBefore;
            drNetIncome[columnName] = incomeTaxesBudget;

            // Difference
            columnName = setMonth(i, true) + "-Difference";
            drGrossProfit[columnName] = actualGrossProfit - budgetGrossProfit;
            drNetProfit[columnName] = actualNetProfit - budgetNetProfit;
            drIncomeBefore[columnName] = actualIncomeBefore - budgetIncomeBefore;
            drNetIncome[columnName] = actualNetIncome - budgetNetIncome;

            // Variance
            columnName = setMonth(i, true) + "-Variance";

            _variance = 0;
            if (budgetGrossProfit != 0)
            {
                if (!decimal.TryParse(((actualGrossProfit - budgetGrossProfit) / Math.Abs(budgetGrossProfit)).ToString(), out _variance))
                {
                    _variance = 0;
                }
            }

            drGrossProfit[columnName] = _variance;

            _variance = 0;
            if (budgetNetProfit != 0)
            {
                if (!decimal.TryParse(((actualNetProfit - budgetNetProfit) / Math.Abs(budgetNetProfit)).ToString(), out _variance))
                {
                    _variance = 0;
                }
            }

            drNetProfit[columnName] = _variance;

            _variance = 0;
            if (budgetIncomeBefore != 0)
            {
                if (!decimal.TryParse(((actualIncomeBefore - budgetIncomeBefore) / Math.Abs(budgetIncomeBefore)).ToString(), out _variance))
                {
                    _variance = 0;
                }
            }

            drIncomeBefore[columnName] = _variance;

            _variance = 0;
            if (budgetNetIncome != 0)
            {
                if (!decimal.TryParse(((actualNetIncome - budgetNetIncome) / Math.Abs(budgetNetIncome)).ToString(), out _variance))
                {
                    _variance = 0;
                }
            }

            drNetIncome[columnName] = _variance;
        }

        // Total
        double revenuesActualTotal = totalRevenues["Total-Actual"], reveneuesBudgetTotal = totalRevenues["Total-Budget"],
           costOfSalesActualTotal = totalCostOfSales["Total-Actual"], costOfSalesBudgetTotal = totalCostOfSales["Total-Budget"],
           expensesActualTotal = totalExpenses["Total-Actual"], expensesBudgetTotal = totalExpenses["Total-Budget"],
           otherIncomeActualTotal = totalOtherIncome["Total-Actual"], otherIncomeBudgetTotal = totalOtherIncome["Total-Budget"],
           incomeTaxesActualTotal = totalIncomeTaxes["Total-Actual"], incomeTaxesBudgetTotal = totalIncomeTaxes["Total-Budget"];

        var grossProfitActualTotal = revenuesActualTotal - costOfSalesActualTotal;
        var grossProfitBudgetTotal = reveneuesBudgetTotal - costOfSalesBudgetTotal;

        drGrossProfit["Total-Actual"] = grossProfitActualTotal;
        drGrossProfit["Total-Budget"] = grossProfitBudgetTotal;
        drGrossProfit["Total-Difference"] = grossProfitActualTotal - grossProfitBudgetTotal;

        _variance = 0;
        if (grossProfitBudgetTotal != 0)
        {
            if (!decimal.TryParse(((grossProfitActualTotal - grossProfitBudgetTotal) / Math.Abs(grossProfitBudgetTotal)).ToString(), out _variance))
            {
                _variance = 0;
            }
        }
        drGrossProfit["Total-Variance"] = _variance;

        var netProfitActualTotal = grossProfitActualTotal - expensesActualTotal;
        var netProfitBudgetTotal = grossProfitBudgetTotal - expensesBudgetTotal;

        drNetProfit["Total-Actual"] = netProfitActualTotal;
        drNetProfit["Total-Budget"] = netProfitBudgetTotal;
        drNetProfit["Total-Difference"] = netProfitActualTotal - netProfitBudgetTotal;

        _variance = 0;
        if (netProfitBudgetTotal != 0)
        {
            if (!decimal.TryParse(((netProfitActualTotal - netProfitBudgetTotal) / Math.Abs(netProfitBudgetTotal)).ToString(), out _variance))
            {
                _variance = 0;
            }
        }
        drNetProfit["Total-Variance"] = _variance;

        var incomeBeforeActualTotal = netProfitActualTotal + otherIncomeActualTotal;
        var incomeBeforeBudgetTotal = netProfitBudgetTotal + otherIncomeBudgetTotal;

        drIncomeBefore["Total-Actual"] = incomeBeforeActualTotal;
        drIncomeBefore["Total-Budget"] = incomeBeforeBudgetTotal;
        drIncomeBefore["Total-Difference"] = incomeBeforeActualTotal - incomeBeforeBudgetTotal;

        _variance = 0;
        if (incomeBeforeBudgetTotal != 0)
        {
            if (!decimal.TryParse(((incomeBeforeActualTotal - incomeBeforeBudgetTotal) / Math.Abs(incomeBeforeBudgetTotal)).ToString(), out _variance))
            {
                _variance = 0;
            }
        }
        drIncomeBefore["Total-Variance"] = _variance;

        var netIncomeActualTotal = incomeBeforeActualTotal - incomeTaxesActualTotal;
        var netIncomeBudgetTotal = incomeBeforeBudgetTotal - incomeTaxesBudgetTotal;

        drNetIncome["Total-Actual"] = netIncomeActualTotal;
        drNetIncome["Total-Budget"] = netIncomeBudgetTotal;
        drNetIncome["Total-Difference"] = netIncomeActualTotal - netIncomeBudgetTotal;

        _variance = 0;
        if (netIncomeBudgetTotal != 0)
        {
            if (!decimal.TryParse(((netIncomeActualTotal - netIncomeBudgetTotal) / Math.Abs(netIncomeBudgetTotal)).ToString(), out _variance))
            {
                _variance = 0;
            }
        }
        drNetIncome["Total-Variance"] = _variance;

        finalTable.Rows.Add(drGrossProfit);
        finalTable.Rows.Add(drNetProfit);
        finalTable.Rows.Add(drIncomeBefore);
        finalTable.Rows.Add(drNetIncome);

        return finalTable;
    }

    private DataSet ProcessAndIncludeSummaryRows(DataSet ds)
    {
        DataView dview = new DataView();
        DataSet resultSet = new DataSet();
        decimal _variance = 0;

        if (ds != null)
        {
            DataTable dt = ds.Tables[0];
            DataTable fDt = new DataTable();
            DataTable resultTable = dt.Copy();
            DataTable finalTable = dt.Clone();
            DataView dv = dt.DefaultView;
            double actualTotal = 0, budgetTotal = 0;
            var typeName = string.Empty;

            int[] types = { 3, 4, 5, 8, 9 };

            foreach (int i in types)
            {
                dv.RowFilter = "Type = " + i.ToString();
                fDt = dv.ToTable();

                if (fDt.Rows.Count > 0)
                {
                    for (int j = 0; j < fDt.Rows.Count; j++)
                    {
                        try
                        {
                            actualTotal += double.Parse(fDt.Rows[j]["NTotal"].ToString());
                        }
                        catch
                        {
                            actualTotal += 0.00;
                        }

                        try
                        {
                            budgetTotal += double.Parse(fDt.Rows[j]["NBudget"].ToString());
                        }
                        catch
                        {
                            budgetTotal += 0.00;
                        }

                        typeName = fDt.Rows[j]["TypeName"].ToString();
                        finalTable.Rows.Add(fDt.Rows[j].ItemArray);
                    }

                    DataRow dr = finalTable.NewRow();
                    dr["Acct"] = 100 * i;
                    dr["AcctNo"] = 100 * i;
                    dr["AcctName"] = "Total " + typeName;
                    dr["fDesc"] = "Total " + typeName;
                    dr["Type"] = -1;
                    dr["TypeName"] = "Total " + typeName;
                    dr["NTotal"] = actualTotal;
                    dr["NBudget"] = budgetTotal;
                    dr["Difference"] = actualTotal - budgetTotal;

                    if (budgetTotal != 0)
                    {
                        if (!decimal.TryParse((((actualTotal - budgetTotal) / Math.Abs(budgetTotal))).ToString(), out _variance))
                        {
                            _variance = 0;
                        }

                        dr["Variance"] = _variance;
                    }
                    else
                    {
                        dr["Variance"] = 0;
                    }

                    finalTable.Rows.Add(dr);
                    actualTotal = budgetTotal = 0.00;
                }
            }

            finalTable = IncludeCalculationRows(finalTable);
            dview = finalTable.DefaultView;
            dview.Sort = "Type asc";
        }

        resultSet.Tables.Add(dview.ToTable());

        return resultSet;
    }

    private DataTable IncludeCalculationRows(DataTable finalTable)
    {
        double revenuesActual = 0, reveneuesBudget = 0,
            costOfSalesActual = 0, costOfSalesBudget = 0,
            expensesActual = 0, expensesBudget = 0,
            otherIncomeActual = 0, otherIncomeBudget = 0,
            incomeTaxesActual = 0, incomeTaxesBudget = 0;
        decimal variance = 0;

        foreach (DataRow dRow in finalTable.Rows)
        {
            if (dRow["fDesc"].ToString().Trim().Contains("Total Revenues"))
            {
                revenuesActual = double.Parse(dRow["NTotal"].ToString());
                reveneuesBudget = double.Parse(dRow["NBudget"].ToString());
            }

            if (dRow["fDesc"].ToString().Trim().Contains("Total Cost of Sales"))
            {
                costOfSalesActual = double.Parse(dRow["NTotal"].ToString());
                costOfSalesBudget = double.Parse(dRow["NBudget"].ToString());
            }

            if (dRow["fDesc"].ToString().Trim().Contains("Total Expenses"))
            {
                expensesActual = double.Parse(dRow["NTotal"].ToString());
                expensesBudget = double.Parse(dRow["NBudget"].ToString());
            }

            if (dRow["fDesc"].ToString().Trim().Contains("Total Other Income (Expense)"))
            {
                otherIncomeActual = double.Parse(dRow["NTotal"].ToString());
                otherIncomeBudget = double.Parse(dRow["NBudget"].ToString());
            }

            if (dRow["fDesc"].ToString().Trim().Contains("Total Provisions for Income Taxes"))
            {
                incomeTaxesActual = double.Parse(dRow["NTotal"].ToString());
                incomeTaxesBudget = double.Parse(dRow["NBudget"].ToString());
            }
        }

        // Gross Profit
        var grossActual = revenuesActual - costOfSalesActual;
        var grossBudget = reveneuesBudget - costOfSalesBudget;

        DataRow drGrossProfit = finalTable.NewRow();
        drGrossProfit["Acct"] = 10011;
        drGrossProfit["AcctNo"] = 10011;
        drGrossProfit["AcctName"] = "Gross Profit";
        drGrossProfit["fDesc"] = "Gross Profit";
        drGrossProfit["Type"] = -1;
        drGrossProfit["TypeName"] = "Gross Profit";
        drGrossProfit["NTotal"] = grossActual;
        drGrossProfit["NBudget"] = grossBudget;
        drGrossProfit["Difference"] = grossActual - grossBudget;

        if (grossBudget != 0)
        {
            if (decimal.TryParse(((grossActual - grossBudget) / Math.Abs(grossBudget)).ToString(), out variance))
            {
                drGrossProfit["Variance"] = variance;
            }
            else
            {
                drGrossProfit["Variance"] = 0;
            }
        }
        else
        {
            drGrossProfit["Variance"] = 0;
        }

        finalTable.Rows.Add(drGrossProfit);

        // Net Profit/Loss
        var netProfitActual = grossActual - expensesActual;
        var netProfitBudget = grossBudget - expensesBudget;

        DataRow drNetProfit = finalTable.NewRow();
        drNetProfit["Acct"] = 100113;
        drNetProfit["AcctNo"] = 100113;
        drNetProfit["AcctName"] = "Net Profit/Loss";
        drNetProfit["fDesc"] = "Net Profit/Loss";
        drNetProfit["Type"] = -1;
        drNetProfit["TypeName"] = "Net Profit";
        drNetProfit["NTotal"] = netProfitActual;
        drNetProfit["NBudget"] = netProfitBudget;
        drNetProfit["Difference"] = netProfitActual - netProfitBudget;

        if (netProfitBudget != 0)
        {
            if (decimal.TryParse(((netProfitActual - netProfitBudget) / Math.Abs(netProfitBudget)).ToString(), out variance))
            {
                drNetProfit["Variance"] = variance;
            }
            else
            {
                drNetProfit["Variance"] = 0;
            }
        }
        else
        {
            drNetProfit["Variance"] = 0;
        }

        finalTable.Rows.Add(drNetProfit);

        // Income Before Provisions
        var incomeBeforeActual = netProfitActual + otherIncomeActual;
        var incomeBeforeBudget = netProfitBudget + otherIncomeBudget;

        DataRow drIncomeBefore = finalTable.NewRow();
        drIncomeBefore["Acct"] = 100114;
        drIncomeBefore["AcctNo"] = 100114;
        drIncomeBefore["AcctName"] = "Income Before Provisions For Income Taxes";
        drIncomeBefore["fDesc"] = "Income Before Provisions For Income Taxes";
        drIncomeBefore["Type"] = -1;
        drIncomeBefore["TypeName"] = "Income Before Provisions";
        drIncomeBefore["NTotal"] = incomeBeforeActual;
        drIncomeBefore["NBudget"] = incomeBeforeBudget;
        drIncomeBefore["Difference"] = incomeBeforeActual - incomeBeforeBudget;

        if (incomeBeforeBudget != 0)
        {
            if (decimal.TryParse(((incomeBeforeActual - incomeBeforeBudget) / Math.Abs(incomeBeforeBudget)).ToString(), out variance))
            {
                drIncomeBefore["Variance"] = variance;
            }
            else
            {
                drIncomeBefore["Variance"] = 0;
            }
        }
        else
        {
            drIncomeBefore["Variance"] = 0;
        }

        finalTable.Rows.Add(drIncomeBefore);

        // Net Income
        var netIncomeActual = incomeBeforeActual - incomeTaxesActual;
        var netIncomeBudget = incomeBeforeBudget - incomeTaxesBudget;

        DataRow drNetIncome = finalTable.NewRow();
        drNetIncome["Acct"] = 100115;
        drNetIncome["AcctNo"] = 100115;
        drNetIncome["AcctName"] = "Net Income";
        drNetIncome["fDesc"] = "Net Income";
        drNetIncome["Type"] = -1;
        drNetIncome["TypeName"] = "Net Income";
        drNetIncome["NTotal"] = netIncomeActual;
        drNetIncome["NBudget"] = netIncomeBudget;
        drNetIncome["Difference"] = netIncomeActual - netIncomeBudget;

        if (netIncomeBudget != 0)
        {
            if (decimal.TryParse(((netIncomeActual - netIncomeBudget) / Math.Abs(netIncomeBudget)).ToString(), out variance))
            {
                drNetIncome["Variance"] = variance;
            }
            else
            {
                drNetIncome["Variance"] = 0;
            }
        }
        else
        {
            drNetIncome["Variance"] = 0;
        }

        finalTable.Rows.Add(drNetIncome);

        return finalTable;
    }

    private DataSet ProcessAndBuildSummaryRowForCenters(DataSet ds)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = ds.Tables[0].Copy();
            dt.Columns.Remove("Acctnumber");
            dt.Columns.Remove("AcctName");
            dt.Columns.Remove("AnnualTotal");
            dt.Columns.Remove("TypeName");
            dt.Columns.Remove("Url");
            var finalDs = new DataSet();

            DataTable RevenuesTotal = BuildCentersTable();
            RevenuesTotal.TableName = "RevenuesTotal";

            DataTable CostOfSalesTotal = BuildCentersTable();
            CostOfSalesTotal.TableName = "CostOfSalesTotal";

            DataTable ExpensesTotal = BuildCentersTable();
            ExpensesTotal.TableName = "ExpensesTotal";

            DataTable GrossProfit = ExpensesTotal.Clone();
            GrossProfit.TableName = "GrossProfit";

            DataTable NetProfitTotal = ExpensesTotal.Clone();
            NetProfitTotal.TableName = "NetProfitTotal";

            DataTable OtherIncomeTotal = BuildCentersTable();
            OtherIncomeTotal.TableName = "OtherIncomeTotal";

            DataTable IncomeTaxesTotal = BuildCentersTable();
            IncomeTaxesTotal.TableName = "IncomeTaxesTotal";

            DataTable BeforeProvisions = BuildCentersTable();
            BeforeProvisions.TableName = "BeforeProvisions";

            DataTable NetIncome = BuildCentersTable();
            NetIncome.TableName = "NetIncome";

            int[] types = { 3, 4, 5, 8, 9 };

            foreach (int j in types)
            {
                DataTable Summary = new DataTable();
                DataView dv = dt.DefaultView;
                dv.RowFilter = "Type = " + j.ToString();
                var fDt = dv.ToTable();

                if (fDt.Rows.Count > 0)
                {
                    var desc = "";

                    if (j == 3)
                    {
                        Summary = RevenuesTotal;
                        desc = "Total Revenues:";
                    }
                    else if (j == 4)
                    {
                        Summary = CostOfSalesTotal;
                        desc = "Total Cost Of Sales:";
                    }
                    else if (j == 5)
                    {
                        Summary = ExpensesTotal;
                        desc = "Total Expenses:";
                    }
                    else if (j == 8)
                    {
                        Summary = OtherIncomeTotal;
                        desc = "Total Other Income (Expenses):";
                    }
                    else if (j == 9)
                    {
                        Summary = IncomeTaxesTotal;
                        desc = "Total Provisions for Income Taxes:";
                    }

                    var columns = fDt.Columns;
                    var rows = fDt.Rows.OfType<DataRow>();
                    var sumRow = Summary.NewRow();
                    sumRow["Acct"] = desc;
                    sumRow["Type"] = j;
                    for (int i = 1; i < columns.Count; i++)
                    {
                        var columnName = columns[i].ColumnName;
                        if (columnName != "Acct" && columnName != "fDesc" && columnName != "Sub")
                        {
                            var columnTotal = rows.Sum(r => Convert.ToDouble(r[columnName].ToString()));
                            sumRow[columnName] = columnTotal;
                        }
                    }

                    Summary.Rows.Add(sumRow.ItemArray);
                    finalDs.Tables.Add(Summary);
                }
            }

            // Gross Profit
            var Columns = dt.Columns;
            var gr = GrossProfit.NewRow();
            gr["Acct"] = "Gross Profit:";
            gr["Type"] = -1;

            for (int i = 1; i < Columns.Count; i++)
            {
                var columnName = Columns[i].ColumnName;
                if (columnName != "Acct" && columnName != "fDesc" && columnName != "Sub")
                {
                    double revenues = 0;
                    if (RevenuesTotal.Rows.Count > 0)
                    {
                        revenues = double.Parse(RevenuesTotal.Rows[0][columnName].ToString());
                    }

                    double costOfSales = 0;
                    if (CostOfSalesTotal.Rows.Count > 0)
                    {
                        costOfSales = double.Parse(CostOfSalesTotal.Rows[0][columnName].ToString());
                    }

                    gr[columnName] = revenues - costOfSales;
                }
            }

            GrossProfit.Rows.Add(gr.ItemArray);
            finalDs.Tables.Add(GrossProfit);

            // Net Profit
            var nr = NetProfitTotal.NewRow();
            nr["Acct"] = "Net Profit:";
            nr["Type"] = -1;

            for (int i = 1; i < Columns.Count; i++)
            {
                var columnName = Columns[i].ColumnName;
                if (columnName != "Acct" && columnName != "fDesc" && columnName != "Sub")
                {
                    double expenses = 0;
                    if (ExpensesTotal.Rows.Count > 0)
                    {
                        expenses = double.Parse(ExpensesTotal.Rows[0][columnName].ToString());
                    }

                    nr[columnName] = double.Parse(GrossProfit.Rows[0][columnName].ToString()) - expenses;
                }
            }

            NetProfitTotal.Rows.Add(nr.ItemArray);
            finalDs.Tables.Add(NetProfitTotal);

            // Income Before Provisions For Income Taxes
            if (OtherIncomeTotal.Rows.Count > 0)
            {
                var bp = BeforeProvisions.NewRow();
                bp["Acct"] = "Income Before Provisions For Income Taxes:";
                bp["Type"] = -1;

                for (int i = 1; i < Columns.Count; i++)
                {
                    var columnName = Columns[i].ColumnName;
                    if (columnName != "Acct" && columnName != "fDesc" && columnName != "Sub")
                    {
                        bp[columnName] = double.Parse(NetProfitTotal.Rows[0][columnName].ToString()) + double.Parse(OtherIncomeTotal.Rows[0][columnName].ToString());
                    }
                }

                BeforeProvisions.Rows.Add(bp.ItemArray);
            }

            finalDs.Tables.Add(BeforeProvisions);

            // Net Income
            if (OtherIncomeTotal.Rows.Count > 0 || IncomeTaxesTotal.Rows.Count > 0)
            {
                var it = NetIncome.NewRow();
                it["Acct"] = "Net Income:";
                it["Type"] = -1;

                for (int i = 1; i < Columns.Count; i++)
                {
                    var columnName = Columns[i].ColumnName;
                    if (columnName != "Acct" && columnName != "fDesc" && columnName != "Sub")
                    {
                        double incomeTax = 0;
                        if(IncomeTaxesTotal.Rows.Count > 0)
                        {
                            incomeTax = double.Parse(IncomeTaxesTotal.Rows[0][columnName].ToString());
                        }

                        if (BeforeProvisions.Rows.Count > 0)
                        {
                            it[columnName] = double.Parse(BeforeProvisions.Rows[0][columnName].ToString()) - incomeTax;
                        }
                        else
                        {
                            it[columnName] = double.Parse(NetProfitTotal.Rows[0][columnName].ToString()) - incomeTax;
                        }
                    }
                }

                NetIncome.Rows.Add(it.ItemArray);
            }

            finalDs.Tables.Add(NetIncome);

            return finalDs;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private DataTable BuildCentersTable(bool isTotal = false)
    {
        var depts = Request.QueryString["departments"].Trim();
        var depArray = depts.Split(',');
        DataTable budgetDataTable = new DataTable();
        budgetDataTable.Columns.Add("Type");
        budgetDataTable.Columns.Add("Acct");
        budgetDataTable.Columns.Add("fDesc");
        budgetDataTable.Columns.Add("Sub");

        if (!isTotal)
        {
            budgetDataTable.Columns.Add("AcctNumber");
            budgetDataTable.Columns.Add("AcctName");
            budgetDataTable.Columns.Add("AnnualTotal");
            budgetDataTable.Columns.Add("TypeName");
            budgetDataTable.Columns.Add("Url");
        }

        objPropUser.ConnConfig = Session["config"].ToString();
        var centers = _objBLReport.GetCenterNames(objPropUser);
        var dv = centers.Tables[0].DefaultView;
        dv.Sort = "CentralName ASC";
        var centersDt = dv.ToTable();
        var columnName = "";
        Array.Sort(depArray, StringComparer.InvariantCulture);
        string[] deptNames = new string[depArray.Length];
        for (int i = 0; i < depArray.Length; i++)
        {
            for (int j = 0; j < centersDt.Rows.Count; j++)
            {
                if (centersDt.Rows[j]["ID"].ToString().Equals(depArray[i]))
                {
                    columnName = centersDt.Rows[j]["CentralName"].ToString();
                    deptNames[i] = columnName;
                    break;
                }
            }
        }

        for (int i = 0; i < deptNames.Length; i++)
        {
            if (deptNames[i] != null)
                budgetDataTable.Columns.Add(deptNames[i]);
        }

        budgetDataTable.Columns.Add("Total");

        return budgetDataTable;
    }

    private DataSet ProcessAndBuildDataForCenters(DataSet ds)
    {
        try
        {
            var budgetTable = BuildCentersTable();
            var finalDs = new DataSet();
            var dt = ds.Tables[0];
            bool valueAdded = false;
            var depts = Request.QueryString["departments"].Trim();
            var depArray = depts.Split(',');

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var actn = dt.Rows[i]["AcctNo"].ToString().Trim();
                var actNo = dt.Rows[i]["Acct"].ToString().Trim();
                var fDesc = dt.Rows[i]["fDesc"].ToString().Trim();
                var type = dt.Rows[i]["Type"].ToString().Trim();
                var typeName = dt.Rows[i]["TypeName"].ToString().Trim();
                var url = dt.Rows[i]["Url"].ToString().Trim();
                var sub = dt.Rows[i]["Sub"].ToString().Trim();
                var dt1 = budgetTable.Copy();
                var dv1 = dt1.DefaultView;

                dv1.RowFilter = " Acct LIKE '"
                    + dt.Rows[i]["AcctNo"].ToString().Trim() + "%' AND TypeName = '"
                    + dt.Rows[i]["TypeName"].ToString().Trim() + "'";

                if (dv1.ToTable().Rows.Count > 0)
                {
                    valueAdded = true;
                }

                if (valueAdded)
                {
                    valueAdded = false;
                    continue;
                }

                var dr = budgetTable.NewRow();
                dr["Acct"] = actn + " " + fDesc;
                dr["AcctNumber"] = actNo;
                dr["Type"] = type;
                dr["TypeName"] = typeName;
                dr["Url"] = url;
                dr["Sub"] = sub;

                var cdt = dt.Copy();
                var dv = cdt.DefaultView;
                dv.RowFilter = "AcctNo ='" + actn + "' AND Type = '" + type + "'";
                var filteredDataTable = dv.ToTable();

                double deptTotal = 0.00;
                objPropUser.ConnConfig = Session["config"].ToString();
                var centers = _objBLReport.GetCenterNames(objPropUser);
                var columnName = "";
                double finalTotal = 0;

                foreach (var dept in depArray)
                {
                    var fdt = filteredDataTable.Copy();
                    var fdv = fdt.DefaultView;
                    fdv.RowFilter = "Department = '" + dept + "'";
                    var finalFilteredDataTable = fdv.ToTable();

                    foreach (DataRow row in finalFilteredDataTable.Rows)
                    {
                        if (!string.IsNullOrEmpty(row["Amount"].ToString()))
                        {
                            deptTotal += double.Parse(row["Amount"].ToString());
                        }
                    }
                    for (int j = 0; j < centers.Tables[0].Rows.Count; j++)
                    {
                        if (centers.Tables[0].Rows[j]["ID"].ToString().Equals(dept))
                        {
                            columnName = centers.Tables[0].Rows[j]["CentralName"].ToString();
                            dr[columnName] = deptTotal;
                            break;
                        }
                    }

                    finalTotal += deptTotal;
                    deptTotal = 0;
                }

                dr["Total"] = finalTotal;
                finalTotal = 0;
                budgetTable.Rows.Add(dr);
            }
            if (depArray.Length == 1)
            {
                budgetTable.Columns.Remove("Total");
            }

            finalDs.Tables.Add(budgetTable.DefaultView.ToTable());

            return finalDs;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private void RemoveHyperWhenExporting(StiReport report)
    {
        var components = report.GetComponents();

        var databands = components.OfType<StiDataBand>().ToList();

        foreach (var databand in databands)
        {
            var dataTextList = databand.GetComponents().OfType<StiText>();
            foreach (var item in dataTextList)
            {

                if (!string.IsNullOrWhiteSpace(item.Hyperlink.Value))
                {
                    item.Hyperlink = new StiHyperlinkExpression(string.Empty);
                }
            }
        }

        report.Render();
    }

    private DataSet ProcessAndBuildDataForCentersAndBudgets(DataSet ds)
    {
        try
        {
            var budgetTable = BuildCentersAndBudgetsTable();
            var finalDs = new DataSet();
            var dt = ds.Tables[0];
            bool valueAdded = false;
            var depts = Request.QueryString["departments"].Trim();
            var depArray = depts.Split(',');

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var actn = dt.Rows[i]["AcctNo"].ToString().Trim();
                var actNo = dt.Rows[i]["Acct"].ToString().Trim();
                var fDesc = dt.Rows[i]["fDesc"].ToString().Trim();
                var type = dt.Rows[i]["Type"].ToString().Trim();
                var typeName = dt.Rows[i]["TypeName"].ToString().Trim();
                var url = dt.Rows[i]["Url"].ToString().Trim();
                var sub = dt.Rows[i]["Sub"].ToString().Trim();
                var dt1 = budgetTable.Copy();
                var dv1 = dt1.DefaultView;

                dv1.RowFilter = " Acct LIKE '"
                    + dt.Rows[i]["AcctNo"].ToString().Trim() + "%' AND TypeName = '"
                    + dt.Rows[i]["TypeName"].ToString().Trim() + "'";

                if (dv1.ToTable().Rows.Count > 0)
                {
                    valueAdded = true;
                }

                if (valueAdded)
                {
                    valueAdded = false;
                    continue;
                }

                var dr = budgetTable.NewRow();
                dr["Acct"] = actn + " " + fDesc;
                dr["AcctNumber"] = actNo;
                dr["Type"] = type;
                dr["TypeName"] = typeName;
                dr["Url"] = url;
                dr["Sub"] = sub;
                var cdt = dt.Copy();
                var dv = cdt.DefaultView;
                dv.RowFilter = "AcctNo ='" + actn + "' AND Type = '" + type + "'";
                var filteredDataTable = dv.ToTable();

                double deptTotal = 0.00;
                objPropUser.ConnConfig = Session["config"].ToString();
                var centers = _objBLReport.GetCenterNames(objPropUser);
                var columnName = "";
                double finalTotal = 0;

                foreach (var dept in depArray)
                {
                    var fdt = filteredDataTable.Copy();
                    var fdv = fdt.DefaultView;
                    fdv.RowFilter = "Department = '" + dept + "'";
                    var finalFilteredDataTable = fdv.ToTable();

                    foreach (DataRow row in finalFilteredDataTable.Rows)
                    {
                        if (!string.IsNullOrEmpty(row["Amount"].ToString()))
                        {
                            deptTotal += double.Parse(row["Amount"].ToString());
                        }

                        if (!string.IsNullOrEmpty(row["Budget"].ToString()))
                        {
                            finalTotal += double.Parse(row["Budget"].ToString());
                        }
                    }

                    for (int j = 0; j < centers.Tables[0].Rows.Count; j++)
                    {
                        if (centers.Tables[0].Rows[j]["ID"].ToString().Equals(dept))
                        {
                            columnName = centers.Tables[0].Rows[j]["CentralName"].ToString();
                            dr[columnName] = deptTotal;
                            break;
                        }
                    }

                    deptTotal = 0;
                }

                dr["Budgets"] = finalTotal;
                finalTotal = 0;
                budgetTable.Rows.Add(dr);
            }

            finalDs.Tables.Add(budgetTable.DefaultView.ToTable());

            return finalDs;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private DataSet ProcessAndBuildSummaryRowForCentersAndBudgets(DataSet ds)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = ds.Tables[0].Copy();
            dt.Columns.Remove("Acctnumber");
            dt.Columns.Remove("AcctName");
            dt.Columns.Remove("AnnualTotal");
            dt.Columns.Remove("TypeName");
            dt.Columns.Remove("Url");
            var finalDs = new DataSet();

            DataTable RevenuesTotal = BuildCentersAndBudgetsTable(true);
            RevenuesTotal.TableName = "RevenuesTotal";

            DataTable CostOfSalesTotal = BuildCentersAndBudgetsTable(true);
            CostOfSalesTotal.TableName = "CostOfSalesTotal";

            DataTable ExpensesTotal = BuildCentersAndBudgetsTable(true);
            ExpensesTotal.TableName = "ExpensesTotal";

            DataTable GrossProfit = ExpensesTotal.Clone();
            GrossProfit.TableName = "GrossProfit";

            DataTable NetProfitTotal = ExpensesTotal.Clone();
            NetProfitTotal.TableName = "NetProfitTotal";

            DataTable OtherIncomeTotal = BuildCentersAndBudgetsTable(true);
            OtherIncomeTotal.TableName = "OtherIncomeTotal";

            DataTable IncomeTaxesTotal = BuildCentersAndBudgetsTable(true);
            IncomeTaxesTotal.TableName = "IncomeTaxesTotal";

            DataTable BeforeProvisions = BuildCentersAndBudgetsTable(true);
            BeforeProvisions.TableName = "BeforeProvisions";

            DataTable NetIncome = BuildCentersAndBudgetsTable(true);
            NetIncome.TableName = "NetIncome";

            int[] types = { 3, 4, 5, 8, 9 };

            foreach (int j in types)
            {
                DataTable Summary = new DataTable();
                DataView dv = dt.DefaultView;
                dv.RowFilter = "Type = " + j.ToString();
                var fDt = dv.ToTable();

                if (fDt.Rows.Count > 0)
                {
                    var desc = "";

                    if (j == 3)
                    {
                        Summary = RevenuesTotal;
                        desc = "Total Revenues:";
                    }
                    else if (j == 4)
                    {
                        Summary = CostOfSalesTotal;
                        desc = "Total Cost Of Sales:";
                    }
                    else if (j == 5)
                    {
                        Summary = ExpensesTotal;
                        desc = "Total Expenses:";
                    }
                    else if (j == 8)
                    {
                        Summary = OtherIncomeTotal;
                        desc = "Total Other Income (Expenses):";
                    }
                    else if (j == 9)
                    {
                        Summary = IncomeTaxesTotal;
                        desc = "Total Provisions for Income Taxes:";
                    }

                    var columns = fDt.Columns;
                    var rows = fDt.Rows.OfType<DataRow>();
                    var sumRow = Summary.NewRow();
                    sumRow["Acct"] = desc;
                    sumRow["Type"] = j;

                    for (int i = 1; i < columns.Count; i++)
                    {
                        var columnName = columns[i].ColumnName;
                        if (columnName != "Acct" && columnName != "fDesc" && columnName != "Sub")
                        {
                            var columnTotal = rows.Sum(r => Convert.ToDouble(r[columnName].ToString()));
                            sumRow[columnName] = columnTotal;
                        }
                    }

                    Summary.Rows.Add(sumRow.ItemArray);
                    finalDs.Tables.Add(Summary);
                }
            }

            // Gross Profit
            var Columns = dt.Columns;
            var gr = GrossProfit.NewRow();
            gr["Acct"] = "Gross Profit:";
            gr["Type"] = -1;
            for (int i = 1; i < Columns.Count; i++)
            {
                var columnName = Columns[i].ColumnName;
                if (columnName != "Acct" && columnName != "fDesc" && columnName != "Sub")
                {
                    double revenues = 0;
                    if (RevenuesTotal.Rows.Count > 0)
                    {
                        revenues = double.Parse(RevenuesTotal.Rows[0][columnName].ToString());
                    }

                    double costOfSales = 0;
                    if (CostOfSalesTotal.Rows.Count > 0)
                    {
                        costOfSales = double.Parse(CostOfSalesTotal.Rows[0][columnName].ToString());
                    }

                    gr[columnName] = revenues - costOfSales;
                }
            }
            GrossProfit.Rows.Add(gr.ItemArray);
            finalDs.Tables.Add(GrossProfit);

            // Net Profit 
            var nr = NetProfitTotal.NewRow();
            nr["Acct"] = "Net Profit:";
            nr["Type"] = -1;
            for (int i = 1; i < Columns.Count; i++)
            {
                var columnName = Columns[i].ColumnName;
                if (columnName != "Acct" && columnName != "fDesc" && columnName != "Sub")
                {
                    double expenses = 0;
                    if (ExpensesTotal.Rows.Count > 0)
                    {
                        expenses = double.Parse(ExpensesTotal.Rows[0][columnName].ToString());
                    }

                    nr[columnName] = double.Parse(GrossProfit.Rows[0][columnName].ToString()) - expenses;
                }
            }

            NetProfitTotal.Rows.Add(nr.ItemArray);
            finalDs.Tables.Add(NetProfitTotal);

            // Income Before Provisions For Income Taxes
            if (OtherIncomeTotal.Rows.Count > 0)
            {
                var bp = BeforeProvisions.NewRow();
                bp["Acct"] = "Income Before Provisions For Income Taxes:";
                bp["Type"] = -1;

                for (int i = 1; i < Columns.Count; i++)
                {
                    var columnName = Columns[i].ColumnName;
                    if (columnName != "Acct" && columnName != "fDesc" && columnName != "Sub")
                    {
                        bp[columnName] = double.Parse(NetProfitTotal.Rows[0][columnName].ToString()) + double.Parse(OtherIncomeTotal.Rows[0][columnName].ToString());
                    }
                }

                BeforeProvisions.Rows.Add(bp.ItemArray);
            }

            finalDs.Tables.Add(BeforeProvisions);

            // Net Income
            if (OtherIncomeTotal.Rows.Count > 0 || IncomeTaxesTotal.Rows.Count > 0)
            {
                var it = NetIncome.NewRow();
                it["Acct"] = "Net Income:";
                it["Type"] = -1;

                for (int i = 1; i < Columns.Count; i++)
                {
                    var columnName = Columns[i].ColumnName;
                    if (columnName != "Acct" && columnName != "fDesc" && columnName != "Sub")
                    {
                        double incomeTax = 0;
                        if (IncomeTaxesTotal.Rows.Count > 0)
                        {
                            incomeTax = double.Parse(IncomeTaxesTotal.Rows[0][columnName].ToString());
                        }

                        if (BeforeProvisions.Rows.Count > 0)
                        {
                            it[columnName] = double.Parse(BeforeProvisions.Rows[0][columnName].ToString()) - incomeTax;
                        }
                        else
                        {
                            it[columnName] = double.Parse(NetProfitTotal.Rows[0][columnName].ToString()) - incomeTax;
                        }
                    }
                }

                NetIncome.Rows.Add(it.ItemArray);
            }

            finalDs.Tables.Add(NetIncome);

            return finalDs;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private DataTable BuildCentersAndBudgetsTable(bool isTotal = false)
    {
        var depts = Request.QueryString["departments"].Trim();
        var depArray = depts.Split(',');
        DataTable budgetDataTable = new DataTable();
        budgetDataTable.Columns.Add("Type");
        budgetDataTable.Columns.Add("Acct");
        budgetDataTable.Columns.Add("fDesc");
        budgetDataTable.Columns.Add("Sub");

        if (!isTotal)
        {
            budgetDataTable.Columns.Add("AcctNumber");
            budgetDataTable.Columns.Add("AcctName");
            budgetDataTable.Columns.Add("AnnualTotal");
            budgetDataTable.Columns.Add("TypeName");
            budgetDataTable.Columns.Add("Url");
        }

        objPropUser.ConnConfig = Session["config"].ToString();

        var centers = _objBLReport.GetCenterNames(objPropUser);
        var dv = centers.Tables[0].DefaultView;
        dv.Sort = "CentralName ASC";
        var centersDt = dv.ToTable();
        var columnName = "";
        Array.Sort(depArray, StringComparer.InvariantCulture);
        string[] deptNames = new string[depArray.Length];
        for (int i = 0; i < depArray.Length; i++)
        {
            for (int j = 0; j < centersDt.Rows.Count; j++)
            {
                if (centersDt.Rows[j]["ID"].ToString().Equals(depArray[i]))
                {
                    columnName = centersDt.Rows[j]["CentralName"].ToString();
                    deptNames[i] = columnName;
                    break;
                }
            }
        }

        for (int i = 0; i < deptNames.Length; i++)
        {
            if (deptNames[i] != null)
                budgetDataTable.Columns.Add(deptNames[i]);
        }

        budgetDataTable.Columns.Add("Budgets");

        return budgetDataTable;
    }

    private string GetPeriods(DateTime startDate, DateTime endDate)
    {
        List<int> listPeriod = new List<int>();
        var start = new DateTime(startDate.Year, startDate.Month, 1);
        while (start <= endDate)
        {
            listPeriod.Add(start.Year * 100 + start.Month);

            start = start.AddMonths(1);
        }

        return string.Join(",", listPeriod);
    }

    private void BindingBudget(bool isLoad = false)
    {
        // Binding budgets
        objPropUser.ConnConfig = Session["config"].ToString();
        var monthEnd = _objBLReport.GetFiscalYearData(objPropUser);

        int startYear = DateTime.Now.Year;
        if (isLoad)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["startDate"]))
            {
                var fromDate = Convert.ToDateTime(Request.QueryString["startDate"]);
                startYear = fromDate.Year;
            }
        }
        else if (!string.IsNullOrEmpty(txtStartDate.Text))
        {
            var fromDate = DateTime.Parse(txtStartDate.Text);
            startYear = fromDate.Year;
        }

        int endYear = DateTime.Now.Year;
        if (isLoad)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["endDate"]))
            {
                var toDate = Convert.ToDateTime(Request.QueryString["endDate"]);
                if (toDate.Month <= monthEnd + 1)
                {
                    endYear = toDate.Year;
                }
                else
                {
                    endYear = toDate.Year + 1;
                }
            }
        }
        else if (!string.IsNullOrEmpty(txtEndDate.Text))
        {
            var toDate = DateTime.Parse(txtEndDate.Text);

            if (toDate.Month <= monthEnd + 1)
            {
                endYear = toDate.Year;
            }
            else
            {
                endYear = toDate.Year + 1;
            }
        }

        DataSet dsBudget = bL_Budgets.GetBudgetsByYear(Session["config"].ToString(), startYear, endYear);
        rcBudget.DataSource = dsBudget;
        rcBudget.DataTextField = "Budget";
        rcBudget.DataValueField = "BudgetID";
        rcBudget.DataBind();

        if (Request.QueryString["budgets"] != null)
        {
            var buds = Request.QueryString["budgets"].Trim();
            var budgetArray = buds.Split(',');

            for (int i = 0; i < budgetArray.Length; i++)
            {
                RadComboBoxItem item = rcBudget.FindItemByValue(budgetArray[i]);
                if (item != null)
                    item.Checked = true;
            }
        }
    }

    private void BindingCenter()
    {
        // Binding centers
        objCompany.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();

        string[] company = { "" };
        string selectedCompanies = "";
        if (Session["CompIDs"] != null)
            selectedCompanies = Session["CompIDs"].ToString();
        if (selectedCompanies != null)
            company = selectedCompanies.Split(',');

        objPropUser.ConnConfig = Session["config"].ToString();
        var centers = _objBLReport.GetCenterNames(objPropUser);
        DataSet centerDs = new DataSet();

        DataTable centerDt = new DataTable();
        centerDt.Columns.Add(new DataColumn("ID"));
        centerDt.Columns.Add(new DataColumn("CentralName"));

        DataView dView = centers.Tables[0].DefaultView;
        if (Session["cmpchkDefault"].ToString() == "1")
        {
            if (company.Length > 1)
            {
                for (int i = 0; i < company.Length; i++)
                {
                    var en = company[i].ToString();
                    if (en != String.Empty)
                        objPropUser.EN = Convert.ToInt32(en);

                    DataRow dr = centerDt.NewRow();
                    var centersForUser = _objBLReport.GetDepartmentByCompanyID(objPropUser);
                    for (int j = 0; j < centersForUser.Tables[0].Rows.Count; j++)
                    {
                        var center = centersForUser.Tables[0].Rows[j]["CostCenter"].ToString().Trim();
                        var centerID = 0;
                        for (int k = 0; k < centers.Tables[0].Rows.Count; k++)
                        {
                            if (centers.Tables[0].Rows[k]["CentralName"].ToString().Trim().Equals(center))
                            {
                                centerID = Convert.ToInt32(centers.Tables[0].Rows[k]["ID"].ToString());
                                break;
                            }
                        }
                        dr["ID"] = centerID;
                        dr["CentralName"] = center;

                        centerDt.Rows.Add(dr.ItemArray);
                    }
                }

                var centerDv = centerDt.DefaultView.ToTable(true, "ID", "CentralName");
                var dV = centerDv.DefaultView;
                dV.Sort = "CentralName ASC";
                centerDs.Tables.Add(dV.ToTable());
            }
            else
            {
                DataSet defaultCompany = new DataSet();
                defaultCompany = objBL_Company.getUserDefaultCompany(objCompany);

                DataView dbView = centers.Tables[0].DefaultView;
                for (int i = 0; i <= defaultCompany.Tables[0].Rows.Count; i++)
                {
                    if (defaultCompany.Tables[0].Rows.Count > 0)
                    {
                        var en = defaultCompany.Tables[0].Rows[0]["EN"].ToString();
                        objPropUser.EN = Convert.ToInt32(en);
                    }
                }

                var centersForUser = _objBLReport.GetDepartmentByDefaultCompanyID(objPropUser);
                for (int i = 0; i < centersForUser.Tables[0].Rows.Count; i++)
                {
                    var center = centersForUser.Tables[0].Rows[i]["CostCenter"].ToString();
                    dView.RowFilter = "CentralName = '" + center + "'";
                }

                dView.Sort = "CentralName ASC";
                centerDt = dView.ToTable();
                centerDs.Tables.Add(centerDt);
            }
        }

        rcCenter.DataSource = centers;
        rcCenter.DataBind();
        rcCenter.DataTextField = "CentralName";
        rcCenter.DataValueField = "ID";
        rcCenter.DataBind();

        rcCenter1.DataSource = centers;
        rcCenter1.DataBind();
        rcCenter1.DataTextField = "CentralName";
        rcCenter1.DataValueField = "ID";
        rcCenter1.DataBind();

        ddlOfficeCenter.Visible = true;
        ddlOfficeCenter.DataSource = centers;
        ddlOfficeCenter.DataBind();
        ddlOfficeCenter.DataTextField = "CentralName";
        ddlOfficeCenter.DataValueField = "ID";
        ddlOfficeCenter.DataBind();
        ddlOfficeCenter.Items.Insert(0, new ListItem("--Select Center--", string.Empty));
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
                //distributionList = distributionList.AsEnumerable().Distinct().OrderBy(e=>e.Field<string>("GroupName")).CopyToDataTable();
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

    #region Send email

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
        RadGrid_Emails.Rebind();
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        // ddlSearch_SelectedIndexChanged(sender, e);
        ddlSearch.SelectedIndex = 0;
        txtSearch.Text = string.Empty;
        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
        RadGrid_Emails.Rebind();
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
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
            //FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
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
        DeleteAttachment(path);
    }

    private void DeleteAttachment(string path)
    {
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
        //MoveFileEx(filepath, null, MoveFileFlags.MOVEFILE_DELAY_UNTIL_REBOOT);

        if (System.IO.File.Exists(filepath))
        {
            // Use a try block to catch IOExceptions, to 
            // handle the case of the file already being 
            // opened by another process. 
            try
            {
                System.IO.File.Delete(filepath);
            }
            catch //(System.IO.IOException e)
            {
                //Console.WriteLine(e.Message);
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
            var fileName = $"{ddlReport.SelectedItem.Text.Replace(" ", "")}.pdf";
            if (DownloadFileName == fileName)
            {
                Response.Clear();
                MemoryStream ms = new MemoryStream(GetReportAsAttachment());
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                Response.Buffer = true;
                ms.WriteTo(Response.OutputStream);
                Response.End();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
                "FileaccessWarning", "alert('File not found.');", true);
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('Please provide access permissions to the file path.');", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileerrorWarning", "alert('" + str + "');", true);
        }
    }

    #endregion

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
