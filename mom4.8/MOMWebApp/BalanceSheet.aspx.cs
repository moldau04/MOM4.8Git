using BusinessEntity;
using BusinessLayer;
using Microsoft.Reporting.WebForms;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Stimulsoft.Report;
using Stimulsoft.Report.Web;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Collections;
using System.Net.Configuration;
using System.Collections.Generic;
using AjaxControlToolkit;
using Telerik.Web.UI;

public partial class BalanceSheet : System.Web.UI.Page
{
    #region Variables
    GeneralFunctions objgn = new GeneralFunctions();
    Chart objChart = new Chart();
    BL_Chart objBL_Chart = new BL_Chart();
    BL_JournalEntry objBL_JournalEntry = new BL_JournalEntry();
    Journal _journal = new Journal();
    User objPropUser = new User();
    BL_Report objBL_Report = new BL_Report();
    BL_User objBL_User = new BL_User();

    ChartDetails objChartDetail = new ChartDetails();
    AcctDetails objAcct = new AcctDetails();
    SubAcctDetails objSubAcct = new SubAcctDetails();
    #endregion

    #region Events

    #region PAGELOAD
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
                if (Request.QueryString["EndDate"] != null)
                {
                    StiWebViewerBalanceSheet.Visible = true;
                    txtEndDate.Text = Request.QueryString["EndDate"].ToString();
                }
                else
                {
                    int year = DateTime.Now.Year;
                    DateTime firstDay = new DateTime(year, 1, 1);
                    txtEndDate.Text = DateTime.Now.Date.ToString("MM/dd/yyyy");
                }

                HighlightSideMenu("financialStatement", "lnkBalanceSheet", "financeStateSub");
                txtFrom.Text = WebBaseUtility.GetFromEmailAddress();
                GetSMTPUser();
                SetAddress();
                string FileName = "BalanceSheet.pdf";
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
            else
            {
                if (rdCollapseAll.Checked)
                {
                    Session["BalanceSheetSummary"] = true;
                }
                else
                {
                    if (rdDetailWithSub.Checked)
                    {
                        Session["BalanceSheetWithSub"] = true;
                    }
                    else
                    {
                        Session["BalanceSheetWithSub"] = false;
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
    #endregion

    private void SetAddress()
    {
        var address = WebBaseUtility.GetSignature();

        string mailBody = "Please review the attached Balance Sheet Report.";
        address = mailBody + Environment.NewLine + "<br />" + Environment.NewLine + "<br />" + address;

        txtBody.Text = address;


    }

    private void HighlightSideMenu(string MenuParent, string PageLink, string SubMenuDiv)
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl(MenuParent);
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        //HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl(PageLink);
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl(SubMenuDiv);
        div.Style.Add("display", "block");
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Session.Remove("BalanceSheetWithSub");
        Session.Remove("BalanceSheetSummary");
        Session.Remove("EndDate");
        Session.Remove("IncludeZero");

        Response.Redirect("home.aspx");
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

                mail.Title = "Balance Sheet Report";
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This is report email sent from Mobile Office Manager. Please find the Balance Sheet Report attached.";
                }
                //mail.AttachmentFiles.Add(ExportReportToPDF("Report_" + objGen.generateRandomString(10) + ".pdf"));

                //StiReport report = GetBalanceSheetReport("");
                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(LoadBalanceSheetReport(), stream, settings);
                buffer1 = stream.ToArray();
                //mail.attachmentBytes = buffer1;

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
                        if (strpath != "BalanceSheet.pdf")
                        {
                            mail.AttachmentFiles.Add(strpath);
                        }
                    }
                }

                mail.FileName = "BalanceSheet.pdf";

                mail.DeleteFilesAfterSend = true;
                mail.RequireAutentication = false;
                // ES-33:Task#2
                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                mail.Send();
                //this.programmaticModalPopup.Hide();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            catch (Exception ex)
            {
                //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                //string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                WebBaseUtility.ShowEmailErrorMessageBox(this, Page.GetType(), ex);
            }
        }
    }

    protected void lnkSearchOld_Click(object sender, EventArgs e)
    {
        DateTime EndDate = DateTime.Parse(txtEndDate.Text);
        Session["EndDate"] = EndDate;
        Session["IncludeZero"] = chkIncludeZero.Checked;
        StiWebViewerBalanceSheet.Visible = true;
        if (rdCollapseAll.Checked)
        {
            Session["BalanceSheetSummary"] = true;
        }
        else
        {
            if (rdDetailWithSub.Checked)
            {
                Session["BalanceSheetWithSub"] = true;
            }
            else
            {
                Session["BalanceSheetWithSub"] = false;
            }
        }
        var url = "BalanceSheet.aspx?EndDate=" + EndDate.ToShortDateString();

        Response.Redirect(url);
    }

    private double GetNetProfitAmount(DataTable dt)
    {
        double netAmount = 0.00;
        try
        {
            if (dt.Rows.Count > 0)
            {
                double revenueTotal = Convert.ToDouble(dt.Rows[0]["TotalRevenue"]);
                double costsaleTotal = Convert.ToDouble(dt.Rows[0]["TotalCost"]);
                double expenseTotal = Convert.ToDouble(dt.Rows[0]["TotalExpense"]);

                netAmount = revenueTotal - costsaleTotal - expenseTotal;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

        return netAmount;
    }

    #endregion

    protected void rdExpCollAll_CheckedChanged(object sender, EventArgs e)
    {
        DateTime EndDate = DateTime.Parse(txtEndDate.Text);
        Session["EndDate"] = EndDate;
        Session["IncludeZero"] = chkIncludeZero.Checked;
        StiWebViewerBalanceSheet.Visible = true;

        if (rdCollapseAll.Checked)
        {
            Session["BalanceSheetSummary"] = true;
        }
        else
        {
            if (rdDetailWithSub.Checked)
            {
                Session["BalanceSheetWithSub"] = true;
            }
            else
            {
                Session["BalanceSheetWithSub"] = false;
            }
        }
    }

    protected void StiWebViewerBalanceSheet_GetReport(object sender, StiReportDataEventArgs e)
    {
        try
        {
            string reportPathStimul = "";
            if (e.RequestParams.ExportFormat == StiExportFormat.Csv)
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/BalanceSheetCSV.mrt");
            }
            else
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/" + ConfigurationManager.AppSettings["BalanceSheetReport"].ToString());
            }

            if (Session["BalanceSheetSummary"] != null && (bool)Session["BalanceSheetSummary"])
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/BalanceSheetSummary.mrt");
            }
            else if (Session["BalanceSheetWithSub"] != null && (bool)Session["BalanceSheetWithSub"])
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/BalanceSheetWithSub.mrt");
            }

            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            e.Report = report;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void StiWebViewerBalanceSheet_GetReportData(object sender, StiReportDataEventArgs e)
    {
        var report = e.Report;
        try
        {
            bool paramCurrentAccountExists = false;
            objChart.ConnConfig = Session["config"].ToString();

            #region start-end date
            if (string.IsNullOrEmpty(Convert.ToDateTime(Request.QueryString["EndDate"]).ToString()))
            {
                if (!string.IsNullOrEmpty(txtEndDate.Text.ToString()))
                    objChart.EndDate = Convert.ToDateTime(txtEndDate.Text);
                else
                    objChart.EndDate = DateTime.Now.Date;
            }
            else
            {
                if (Request.QueryString["EndDate"] != null)
                    objChart.EndDate = Convert.ToDateTime(Request.QueryString["EndDate"]);
                else
                {
                    if (!string.IsNullOrEmpty(txtEndDate.Text.ToString()))
                        objChart.EndDate = Convert.ToDateTime(txtEndDate.Text);
                    else
                    {
                        if (Session["EndDate"] != null)
                            objChart.EndDate = Convert.ToDateTime(Session["EndDate"].ToString());
                        else
                            objChart.EndDate = DateTime.Now.Date;
                    }
                }

            }
            #endregion

            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objPropUser);

            #region calculate Net Profit
            objChart.ConnConfig = Session["config"].ToString();

            var year = objChart.EndDate.Year;
            string strQuery = "select [dbo].[Control].[YE] from [dbo].[control]";
            int? yearEnd; int startMonth = 0;

            var dataSet = SqlHelper.ExecuteDataset(Session["config"].ToString(), CommandType.Text, strQuery);
            if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
            {
                yearEnd = int.Parse(dataSet.Tables[0].Rows[0][0].ToString());
                startMonth = int.Parse(yearEnd.ToString()) + 2;

                if (startMonth > 12)
                {
                    startMonth = startMonth - 12;
                }
            }

            //if (!string.IsNullOrEmpty(txtEndDate.Text.ToString()))
            //{
            //    objChart.EndDate = Convert.ToDateTime(txtEndDate.Text);
            //}
            //else
            //{
            //    if (Session["EndDate"] != null)
            //        objChart.EndDate = Convert.ToDateTime(Session["EndDate"].ToString());
            //    else
            //        objChart.EndDate = DateTime.Now.Date;
            //}

            var fiscalYear = objChart.EndDate.Year;
            if (startMonth != 1)
            {
                if (objChart.EndDate.Month < startMonth)
                    fiscalYear = fiscalYear - 1;
            }

            // Get year end close out
            _journal.ConnConfig = Session["config"].ToString();
            _journal.Internal = objChart.EndDate.Year.ToString();
            var jes = objBL_JournalEntry.GetYearEndClosedOutData(_journal);

            if (jes != null && jes.Tables[1].Rows.Count > 0)
            {
                var date = Convert.ToDateTime(jes.Tables[1].Rows[0]["fDate"].ToString());
                objChart.StartDate = date.AddDays(1);
            }
            else
            {
                objChart.StartDate = new DateTime(fiscalYear, startMonth, 1);
            }

            DataSet _dsIncome = objBL_Report.GetIncomeStatementTotal(objChart);
            double _netAmount = GetNetProfitAmount(_dsIncome.Tables[0]);
            double netAmount = GetNetProfitAmount(_dsIncome.Tables[1]);

            #endregion

            objChart.ConnConfig = Session["config"].ToString();
            DataSet _dsCurrentEarn = objBL_Chart.GetCurrentEarn(objChart);
            int currAcct = Convert.ToInt32(_dsCurrentEarn.Tables[0].Rows[0]["ID"]);

            int valExpCol = 0;
            if (rdExpandAll.Checked.Equals(true))
            {
                valExpCol = 1;
            }

            bool includeZero = false;
            if (Session["IncludeZero"] != null)
            {
                includeZero = Convert.ToBoolean(Session["IncludeZero"]);
            }

            DataSet ds = objBL_Report.GetBalanceSheetDetails(objChart, includeZero);

            var isTransEmpty = false;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                object value = row["StartDate"];
                if (value == DBNull.Value)
                {
                    isTransEmpty = true;
                }
            }

            // Set URL
            if (!isTransEmpty)
            {
                ds.Tables[0].AsEnumerable().ToList()
                .ForEach(b => b["Url"] =
                    (Request.Url.Scheme +
                        (Uri.SchemeDelimiter +
                            (Request.Url.Authority +
                                (Request.ApplicationPath + "/accountledger.aspx?id=" + b["Acct"].ToString() + "&s=" + System.Web.HttpUtility.UrlEncode(Convert.ToDateTime(b["StartDate"].ToString()).ToShortDateString()).ToString()
                                                                                                                    + "&e=" + System.Web.HttpUtility.UrlEncode(objChart.EndDate.ToShortDateString()).ToString()
                                )
                            )
                        )
                    )
                );

                ds.Tables[0].AcceptChanges();
            }

            DataTable dt = ds.Tables[0].Copy();
            DataSet finalDS = new DataSet();
            finalDS.Tables.Add(dt);

            // Total Assets
            var dtAssets = new DataTable();
            dtAssets = ds.Tables[0].Copy();
            var da = ds.Tables[0].Copy().DefaultView;
            da.RowFilter = "TypeName = 'Asset'";
            dtAssets = da.ToTable();
            dtAssets.TableName = "dsAssetDetails";
            var dsAssets = new DataSet();
            dsAssets.Tables.Add(dtAssets);
            dsAssets.DataSetName = "dsAssetDetails";

            double sumAssetsAmount = 0.00, sumLiabilityAmount = 0.00, sumEquityAmount = 0.00, sumEquityAmountWithoutCE = 0.00;
            for (int i = 0; i <= dsAssets.Tables[0].Rows.Count - 1; i++)
            {
                Double dsvalue = Convert.ToDouble(dsAssets.Tables[0].Rows[i]["Amount"]);
                sumAssetsAmount += dsvalue;
            }

            // Total Liabiliy
            var dtLiabiliy = new DataTable();
            dtLiabiliy = ds.Tables[0].Copy();
            var dl = ds.Tables[0].Copy().DefaultView;
            dl.RowFilter = "TypeName = 'Liability'";
            dtLiabiliy = dl.ToTable();
            dtLiabiliy.TableName = "dsLiabilityDetails";
            var dsLiabiliy = new DataSet();
            dsLiabiliy.Tables.Add(dtLiabiliy);
            dsLiabiliy.DataSetName = "dsLiabilityDetails";

            for (int i = 0; i <= dsLiabiliy.Tables[0].Rows.Count - 1; i++)
            {
                Double dsvalue = Convert.ToDouble(dsLiabiliy.Tables[0].Rows[i]["Amount"]);
                sumLiabilityAmount += dsvalue;
            }

            // Total Equity
            var dtEquity = new DataTable();
            dtEquity = ds.Tables[0].Copy();
            var de = ds.Tables[0].Copy().DefaultView;
            de.RowFilter = "TypeName = 'Equity'";
            dtEquity = de.ToTable();
            dtEquity.TableName = "dsEquityDetails";
            var dsEquity = new DataSet();
            dsEquity.Tables.Add(dtEquity);
            dsEquity.DataSetName = "dsEquityDetails";

            var dsEquityCopy = dsEquity.Copy();
            var currAcctRemoved = false;
            int curtRow = -1;

            for (int i = 0; i <= dsEquity.Tables[0].Rows.Count - 1; i++)
            {
                string Acct = dsEquity.Tables[0].Rows[i]["Acct"].ToString();
                if (Acct == currAcct.ToString())
                {
                    curtRow = i;
                }
            }

            if (jes != null)
            {
                if (jes.Tables[0].Rows.Count > 0)
                {
                    var date = Convert.ToDateTime(jes.Tables[0].Rows[0]["fDate"].ToString());
                    if (objChart.EndDate == date && curtRow != -1)
                    {
                        dsEquity.Tables[0].Rows.Remove(dsEquity.Tables[0].Rows[curtRow]);
                        currAcctRemoved = true;
                    }
                }
            }
            dsEquity.AcceptChanges();

            if (!currAcctRemoved && curtRow != -1)
            {
                if (dsEquity.Tables[0].Rows.Count > 0)
                {
                    dsEquity.Tables[0].Rows[curtRow]["Amount"] = _netAmount;
                }
            }
            dsEquity.Tables[0].AcceptChanges();

            for (int i = 0; i <= dsEquity.Tables[0].Rows.Count - 1; i++)
            {
                Double dsvalue = Convert.ToDouble(dsEquity.Tables[0].Rows[i]["Amount"]);

                if (dsEquity.Tables[0].Rows[i]["Acct"].ToString().Equals(currAcct.ToString()))
                {
                    paramCurrentAccountExists = true;
                }
                else
                {
                    sumEquityAmountWithoutCE += dsvalue;
                }

                sumEquityAmount += dsvalue;
            }

            if (Math.Round(sumAssetsAmount - sumLiabilityAmount - sumEquityAmount, 2) != 0)
            {
                if (!currAcctRemoved && curtRow != -1)
                {
                    dsEquity.Tables[0].Rows[curtRow]["Amount"] = sumAssetsAmount - sumLiabilityAmount - sumEquityAmountWithoutCE;
                }
                else
                {
                    if (curtRow != -1)
                    {
                        dsEquityCopy.Tables[0].Rows[curtRow]["Amount"] = sumAssetsAmount - sumLiabilityAmount - sumEquityAmount;
                        dsEquity = dsEquityCopy;
                    }
                    else
                    {
                        var dtRow = dsEquity.Tables[0].NewRow();
                        dtRow["Acct"] = currAcct;
                        dtRow["fDesc"] = _dsCurrentEarn.Tables[0].Rows[0]["Acct"] + " " + _dsCurrentEarn.Tables[0].Rows[0]["fDesc"];
                        dtRow["Amount"] = sumAssetsAmount - sumLiabilityAmount - sumEquityAmount;

                        dsEquity.Tables[0].Rows.Add(dtRow);
                    }
                }

                dsEquity.AcceptChanges();
                sumEquityAmount = sumAssetsAmount - sumLiabilityAmount;
            }

            string asOfDate = "As of " + Convert.ToDateTime(objChart.EndDate).ToString("MMMM dd, yyyy");
            string url = (Request.Url.Scheme + (Uri.SchemeDelimiter + (Request.Url.Authority + (Request.ApplicationPath + "/"))));

            report.Dictionary.Variables["paramExpCllAll"].Value = valExpCol.ToString();
            report.Dictionary.Variables["paramUsername"].Value = Session["Username"].ToString();
            report.Dictionary.Variables["paramAsOfDate"].Value = asOfDate;
            report.Dictionary.Variables["paramCurrentEarnAcct"].Value = currAcct.ToString();
            report.Dictionary.Variables["paramCurrentEarnAmount"].Value = netAmount.ToString();

            report.Dictionary.Variables["AssetAmount"].Value = sumAssetsAmount.ToString();
            report.Dictionary.Variables["LiabilityAmount"].Value = sumLiabilityAmount.ToString();
            report.Dictionary.Variables["EquityAmount"].Value = sumEquityAmount.ToString();

            report.RegData("dsAcctDetails", finalDS.Tables[0]);
            report.RegData("dsAssetDetails", dsAssets);
            report.RegData("dsLiabilityDetails", dsLiabiliy);
            report.RegData("dsEquityDetails", dsEquity);
            report.RegData("dsCompany", dsC.Tables[0]);
            report.Render();

            // Remove session
            Session.Remove("BalanceSheetWithSub");
            Session.Remove("BalanceSheetSummary");
            Session.Remove("EndDate");
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private StiReport LoadBalanceSheetReport()
    {
        string reportPathStimul = Server.MapPath("StimulsoftReports/" + ConfigurationManager.AppSettings["BalanceSheetReport"].ToString());

        if (rdCollapseAll.Checked)
        {
            reportPathStimul = Server.MapPath("StimulsoftReports/BalanceSheetSummary.mrt");
        }
        else if (rdDetailWithSub.Checked)
        {
            reportPathStimul = Server.MapPath("StimulsoftReports/BalanceSheetWithSub.mrt");
        }

        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        //report.Compile();

        bool paramCurrentAccountExists = false;
        objChart.ConnConfig = Session["config"].ToString();

        #region start-end date
        if (string.IsNullOrEmpty(Convert.ToDateTime(Request.QueryString["EndDate"]).ToString()))
        {
            if (!string.IsNullOrEmpty(txtEndDate.Text.ToString()))
                objChart.EndDate = Convert.ToDateTime(txtEndDate.Text);
            else
                objChart.EndDate = DateTime.Now.Date;
        }
        else
        {
            if (Request.QueryString["EndDate"] != null)
                objChart.EndDate = Convert.ToDateTime(Request.QueryString["EndDate"]);
            else
            {
                if (!string.IsNullOrEmpty(txtEndDate.Text.ToString()))
                    objChart.EndDate = Convert.ToDateTime(txtEndDate.Text);
                else
                {
                    if (Session["EndDate"] != null)
                        objChart.EndDate = Convert.ToDateTime(Session["EndDate"].ToString());
                    else
                        objChart.EndDate = DateTime.Now.Date;
                }
            }

        }
        #endregion

        DataSet dsC = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        dsC = objBL_User.getControl(objPropUser);

        #region calculate Net Profit
        objChart.ConnConfig = Session["config"].ToString();

        var year = objChart.EndDate.Year;
        string strQuery = "select [dbo].[Control].[YE] from [dbo].[control]";
        int? yearEnd; int startMonth = 0;

        var dataSet = SqlHelper.ExecuteDataset(Session["config"].ToString(), CommandType.Text, strQuery);
        if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
        {
            yearEnd = int.Parse(dataSet.Tables[0].Rows[0][0].ToString());
            startMonth = int.Parse(yearEnd.ToString()) + 2;

            if (startMonth > 12)
            {
                startMonth = startMonth - 12;
            }
        }

        if (!string.IsNullOrEmpty(txtEndDate.Text.ToString()))
        {
            objChart.EndDate = Convert.ToDateTime(txtEndDate.Text);
        }
        else
        {
            if (Session["EndDate"] != null)
                objChart.EndDate = Convert.ToDateTime(Session["EndDate"].ToString());
            else
                objChart.EndDate = DateTime.Now.Date;
        }

        var fiscalYear = objChart.EndDate.Year;
        if (startMonth != 1)
        {
            if (objChart.EndDate.Month < startMonth)
                fiscalYear = fiscalYear - 1;
        }

        // Get year end close out
        _journal.ConnConfig = Session["config"].ToString();
        _journal.Internal = objChart.EndDate.Year.ToString();
        var jes = objBL_JournalEntry.GetYearEndClosedOutData(_journal);

        if (jes != null && jes.Tables[1].Rows.Count > 0)
        {
            var date = Convert.ToDateTime(jes.Tables[1].Rows[0]["fDate"].ToString());
            objChart.StartDate = date.AddDays(1);
        }
        else
        {
            objChart.StartDate = new DateTime(fiscalYear, startMonth, 1);
        }

        DataSet _dsIncome = objBL_Report.GetIncomeStatementTotal(objChart);
        double _netAmount = GetNetProfitAmount(_dsIncome.Tables[0]);
        double netAmount = GetNetProfitAmount(_dsIncome.Tables[1]);

        #endregion

        objChart.ConnConfig = Session["config"].ToString();
        DataSet _dsCurrentEarn = objBL_Chart.GetCurrentEarn(objChart);
        int currAcct = Convert.ToInt32(_dsCurrentEarn.Tables[0].Rows[0]["ID"]);

        int valExpCol = 0;
        if (rdExpandAll.Checked.Equals(true))
        {
            valExpCol = 1;
        }

        DataSet ds = objBL_Report.GetBalanceSheetDetails(objChart);

        var isTransEmpty = false;
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            object value = row["StartDate"];
            if (value == DBNull.Value)
            {
                isTransEmpty = true;
            }
        }

        // Set URL
        if (!isTransEmpty)
        {
            ds.Tables[0].AsEnumerable().ToList()
            .ForEach(b => b["Url"] =
                (Request.Url.Scheme +
                    (Uri.SchemeDelimiter +
                        (Request.Url.Authority +
                            (Request.ApplicationPath + "/accountledger.aspx?id=" + b["Acct"].ToString() + "&s=" + System.Web.HttpUtility.UrlEncode(Convert.ToDateTime(b["StartDate"].ToString()).ToShortDateString()).ToString()
                                                                                                                + "&e=" + System.Web.HttpUtility.UrlEncode(objChart.EndDate.ToShortDateString()).ToString()
                            )
                        )
                    )
                )
            );

            ds.Tables[0].AcceptChanges();
        }

        DataTable dt = ds.Tables[0].Copy();
        DataSet finalDS = new DataSet();
        finalDS.Tables.Add(dt);

        // Total Assets
        var dtAssets = new DataTable();
        dtAssets = ds.Tables[0].Copy();
        var da = ds.Tables[0].Copy().DefaultView;
        da.RowFilter = "TypeName = 'Asset'";
        dtAssets = da.ToTable();
        dtAssets.TableName = "dsAssetDetails";
        var dsAssets = new DataSet();
        dsAssets.Tables.Add(dtAssets);
        dsAssets.DataSetName = "dsAssetDetails";

        double sumAssetsAmount = 0.00, sumLiabilityAmount = 0.00, sumEquityAmount = 0.00, sumEquityAmountWithoutCE = 0.00;
        for (int i = 0; i <= dsAssets.Tables[0].Rows.Count - 1; i++)
        {
            Double dsvalue = Convert.ToDouble(dsAssets.Tables[0].Rows[i]["Amount"]);
            sumAssetsAmount += dsvalue;
        }

        // Total Liabiliy
        var dtLiabiliy = new DataTable();
        dtLiabiliy = ds.Tables[0].Copy();
        var dl = ds.Tables[0].Copy().DefaultView;
        dl.RowFilter = "TypeName = 'Liability'";
        dtLiabiliy = dl.ToTable();
        dtLiabiliy.TableName = "dsLiabilityDetails";
        var dsLiabiliy = new DataSet();
        dsLiabiliy.Tables.Add(dtLiabiliy);
        dsLiabiliy.DataSetName = "dsLiabilityDetails";

        for (int i = 0; i <= dsLiabiliy.Tables[0].Rows.Count - 1; i++)
        {
            Double dsvalue = Convert.ToDouble(dsLiabiliy.Tables[0].Rows[i]["Amount"]);
            sumLiabilityAmount += dsvalue;
        }

        // Total Equity
        var dtEquity = new DataTable();
        dtEquity = ds.Tables[0].Copy();
        var de = ds.Tables[0].Copy().DefaultView;
        de.RowFilter = "TypeName = 'Equity'";
        dtEquity = de.ToTable();
        dtEquity.TableName = "dsEquityDetails";
        var dsEquity = new DataSet();
        dsEquity.Tables.Add(dtEquity);
        dsEquity.DataSetName = "dsEquityDetails";

        var dsEquityCopy = dsEquity.Copy();
        var currAcctRemoved = false;
        int curtRow = -1;

        for (int i = 0; i <= dsEquity.Tables[0].Rows.Count - 1; i++)
        {
            string Acct = dsEquity.Tables[0].Rows[i]["Acct"].ToString();

            if (Acct == currAcct.ToString())
            {
                curtRow = i;
            }
        }

        if (jes != null)
        {
            if (jes.Tables[0].Rows.Count > 0)
            {
                var date = Convert.ToDateTime(jes.Tables[0].Rows[0]["fDate"].ToString());
                if (objChart.EndDate == date && curtRow != -1)
                {
                    dsEquity.Tables[0].Rows.Remove(dsEquity.Tables[0].Rows[curtRow]);
                    currAcctRemoved = true;
                }
            }
        }
        dsEquity.AcceptChanges();

        if (!currAcctRemoved && curtRow != -1)
        {
            if (dsEquity.Tables[0].Rows.Count > 0)
            {
                dsEquity.Tables[0].Rows[curtRow]["Amount"] = _netAmount;
            }
        }
        dsEquity.Tables[0].AcceptChanges();

        for (int i = 0; i <= dsEquity.Tables[0].Rows.Count - 1; i++)
        {
            Double dsvalue = Convert.ToDouble(dsEquity.Tables[0].Rows[i]["Amount"]);

            if (dsEquity.Tables[0].Rows[i]["Acct"].ToString().Equals(currAcct.ToString()))
            {
                paramCurrentAccountExists = true;
            }
            else
            {
                sumEquityAmountWithoutCE += dsvalue;
            }

            sumEquityAmount += dsvalue;
        }

        if (Math.Round(sumAssetsAmount - sumLiabilityAmount - sumEquityAmount, 2) != 0)
        {
            if (!currAcctRemoved && curtRow != -1)
            {
                dsEquity.Tables[0].Rows[curtRow]["Amount"] = sumAssetsAmount - sumLiabilityAmount - sumEquityAmountWithoutCE;
            }
            else
            {
                if (curtRow != -1)
                {
                    dsEquityCopy.Tables[0].Rows[curtRow]["Amount"] = sumAssetsAmount - sumLiabilityAmount - sumEquityAmount;
                    dsEquity = dsEquityCopy;
                }
                else
                {
                    var dtRow = dsEquity.Tables[0].NewRow();
                    dtRow["Acct"] = currAcct;
                    dtRow["fDesc"] = _dsCurrentEarn.Tables[0].Rows[0]["Acct"] + " " + _dsCurrentEarn.Tables[0].Rows[0]["fDesc"];
                    dtRow["Amount"] = sumAssetsAmount - sumLiabilityAmount - sumEquityAmount;

                    dsEquity.Tables[0].Rows.Add(dtRow);
                }
            }

            dsEquity.AcceptChanges();
            sumEquityAmount = sumAssetsAmount - sumLiabilityAmount;
        }

        string asOfDate = "As of " + Convert.ToDateTime(objChart.EndDate).ToString("MMMM dd, yyyy");
        string url = (Request.Url.Scheme + (Uri.SchemeDelimiter + (Request.Url.Authority + (Request.ApplicationPath + "/"))));

        report.Dictionary.Variables["paramExpCllAll"].Value = valExpCol.ToString();
        report.Dictionary.Variables["paramUsername"].Value = Session["Username"].ToString();
        report.Dictionary.Variables["paramAsOfDate"].Value = asOfDate;
        report.Dictionary.Variables["paramCurrentEarnAcct"].Value = currAcct.ToString();
        report.Dictionary.Variables["paramCurrentEarnAmount"].Value = netAmount.ToString();

        report.Dictionary.Variables["AssetAmount"].Value = sumAssetsAmount.ToString();
        report.Dictionary.Variables["LiabilityAmount"].Value = sumLiabilityAmount.ToString();
        report.Dictionary.Variables["EquityAmount"].Value = sumEquityAmount.ToString();

        report.RegData("dsAcctDetails", ds.Tables[0]);
        report.RegData("dsAssetDetails", dsAssets);
        report.RegData("dsLiabilityDetails", dsLiabiliy);
        report.RegData("dsEquityDetails", dsEquity);
        report.RegData("dsCompany", dsC.Tables[0]);
        report.Render();
        return report;
    }

    //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    //public static string[] GetContactEmails(string prefixText, int count, string contextKey)
    //{
    //    //Customer objProp_Customer = new Customer();
    //    //BL_Customer objBL_Customer = new BL_Customer();

    //    //DataSet ds = new DataSet();
    //    //if (contextKey != string.Empty)
    //    //{
    //    //    objProp_Customer.ROL = Convert.ToInt32(contextKey);
    //    //}
    //    //objProp_Customer.ConnConfig = HttpContext.Current.Session["config"].ToString();
    //    //ds = objBL_Customer.getContactByRolID(objProp_Customer);

    //    //DataTable dt = ds.Tables[0];

    //    //List<string> txtItems = new List<string>();
    //    //String dbValues;

    //    //foreach (DataRow row in dt.Rows)
    //    //{
    //    //    dbValues = AutoCompleteExtender.CreateAutoCompleteItem(row["Name"].ToString() + "(" + row["email"].ToString() + ")", row["email"].ToString());
    //    //    txtItems.Add(dbValues);
    //    //}
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
        //if (Session["DistributionList"] != null)
        //{
        //    distributionList = (DataTable)Session["DistributionList"];
        //}
        //else
        //{
        //    DataTable distributionList1 = new DataTable();
        //    if (!string.IsNullOrEmpty(txtTo.Text))
        //    {
        //        distributionList1.Columns.Add("MemberEmail");
        //        distributionList1.Columns.Add("MemberName");
        //        distributionList1.Columns.Add("GroupName");
        //        distributionList1.Columns.Add("Type");
        //        DataRow dr = distributionList1.NewRow();
        //        dr[0] = txtTo.Text;
        //        dr[1] = txtTo.Text;
        //        dr[2] = "";
        //        dr[3] = "";
        //        distributionList1.Rows.InsertAt(dr, 0);
        //    }
        //    distributionList = WebBaseUtility.GetContactListOnExchangeServer("0", "");
        //    distributionList.Merge(distributionList1);
        //    Session["DistributionList"] = distributionList;
        //}

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

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
        RadGrid_Emails.Rebind();
        //UpdateSelectedRows
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
                ////if blocks transfered not equals total number of blocks
                //if (i < maxCount)
                //    return false;
                //return true; 
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
            if (DownloadFileName == "BalanceSheet.pdf")
            {
                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(LoadBalanceSheetReport(), stream, settings);
                buffer1 = stream.ToArray();

                Response.Clear();
                MemoryStream ms = new MemoryStream(buffer1);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=BalanceSheet.pdf");
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
}