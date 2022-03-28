using AjaxControlToolkit;
using BusinessEntity;
using BusinessLayer;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.Reporting.WebForms;
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
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class TrialBalance : System.Web.UI.Page
{

    #region "Variables"
    GeneralFunctions objgn = new GeneralFunctions();
    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();
    BL_Report _objBLReport = new BL_Report();

    Journal _journal = new Journal();
    BL_JournalEntry objBL_JournalEntry = new BL_JournalEntry();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    #endregion

    #region "events"

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

                if (!string.IsNullOrEmpty(Request.QueryString["type"]))
                {
                    StiWebViewerTrialBalance.Visible = true;
                    ddlReport.SelectedValue = Request.QueryString["type"];
                }

                if (!string.IsNullOrEmpty(Request.QueryString["sd"]))
                {
                    txtStartDate.Text = HttpUtility.UrlDecode(Request.QueryString["sd"]);
                }
                else
                {
                    var now = DateTime.Now;
                    txtStartDate.Text = new DateTime(now.Year, now.Month, 1).ToShortDateString();
                }

                if (!string.IsNullOrEmpty(Request.QueryString["ed"]))
                {
                    txtEndDate.Text = HttpUtility.UrlDecode(Request.QueryString["ed"]);
                }
                else
                {
                    txtEndDate.Text = DateTime.Now.ToShortDateString();
                }

                HighlightSideMenu("financialStatement", "lnkTrialBalance", "financeStateSub");
                txtFrom.Text = WebBaseUtility.GetFromEmailAddress();
                GetSMTPUser();
                SetAddress();
                string FileName = "TrialBalance.pdf";
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
    #endregion

    private void SetAddress()
    {
        var address = WebBaseUtility.GetSignature();

        string mailbody = "Please review the attached Trial Balanace report.";
        address = mailbody + Environment.NewLine + "<br />" + Environment.NewLine + "<br />" + address;

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

    protected void lnkSearchOld_Click(object sender, EventArgs e)
    {
        var url = "TrialBalance.aspx?type=" + ddlReport.SelectedValue;

        if (ddlReport.SelectedValue == "1")
        {
            url += "&ed=" + txtEndDate.Text;
        }
        else
        {
            url += "&sd=" + txtStartDate.Text + "&ed=" + txtEndDate.Text;
        }

        Response.Redirect(url);
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
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
                mail.Title = "Trial Balance Report";
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This is report email sent from Mobile Office Manager. Please find the Trial Balance Report attached.";
                }
                //mail.AttachmentFiles.Add(ExportReportToPDF("Report_" + objGen.generateRandomString(10) + ".pdf"));
                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(LoadTrialBalanceReport(), stream, settings);
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
                        if (strpath != "TrialBalance.pdf")
                        {
                            mail.AttachmentFiles.Add(strpath);
                        }
                    }
                }

                //mail.attachmentBytes = buffer1;
                mail.FileName = "TrialBalance.pdf";

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
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                //string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }

    #endregion

    protected void StiWebViewerTrialBalance_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        try
        {
            e.Report = LoadTrialBalanceReport();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void StiWebViewerTrialBalance_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {

    }

    private StiReport LoadTrialBalanceReport()
    {
        if (!string.IsNullOrEmpty(Request.QueryString["type"]) && Request.QueryString["type"] == "1")
        {
            return LoadTrialBalanceStandardReport();
        }
        else
        {
            return LoadTrialBalanceActivityReport();
        }
    }

    private StiReport LoadTrialBalanceStandardReport()
    {
        try
        {
            string reportPathStimul = Server.MapPath("StimulsoftReports/TrialBalance.mrt");
            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            _objChart.ConnConfig = Session["config"].ToString();
            string strQuery = "select [dbo].[Control].[YE] from [dbo].[control]";
            int? yearEnd;
            int startMonth = 0;

            var ds1 = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, strQuery);
            if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
            {
                yearEnd = int.Parse(ds1.Tables[0].Rows[0][0].ToString());
                startMonth = int.Parse(yearEnd.ToString()) + 2;

                if (startMonth > 12)
                {
                    startMonth = startMonth - 12;
                }
            }

            if (!string.IsNullOrEmpty(Request.QueryString["ed"]))
            {
                _objChart.EndDate = Convert.ToDateTime(HttpUtility.UrlDecode(Request.QueryString["ed"]));
            }
            else
            {
                _objChart.EndDate = DateTime.Now.Date;
            }

            var fiscalYear = _objChart.EndDate.Year;
            if (startMonth != 1)
            {
                if (_objChart.EndDate.Month < startMonth)
                    fiscalYear = fiscalYear - 1;
            }

            _objChart.StartDate = new DateTime(fiscalYear, startMonth, 1);

            #region Set Header

            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objPropUser);

            string _asOfDate = "As of " + _objChart.EndDate.ToString("MMMM dd, yyyy");

            #endregion

            var isCloseOut = false;
            _journal.ConnConfig = Session["config"].ToString();
            _journal.Internal = _objChart.EndDate.Year.ToString();
            var jes = objBL_JournalEntry.GetYearEndClosedOutData(_journal);
            if (jes != null)
            {
                if (jes.Tables[0].Rows.Count > 0)
                {
                    var date = Convert.ToDateTime(jes.Tables[0].Rows[0]["fDate"].ToString());
                    if (_objChart.EndDate == date)
                    {
                        isCloseOut = true;
                    }
                }
            }

            DataSet _dsTrial = _objBLReport.GetTrialBalanceDetails(_objChart, isCloseOut);
            DataSet dsIncome = _objBLReport.GetIncomestatementBalance(_objChart);
            BL_Chart objBL_Chart = new BL_Chart();

            DataSet _dsRetainedEarn = objBL_Chart.GetRetainedEarn(_objChart);
            DataSet _dsCurrentEarn = objBL_Chart.GetCurrentEarn(_objChart);

            int retAcct = Convert.ToInt32(_dsRetainedEarn.Tables[0].Rows[0]["ID"]);
            string currentDesc = _dsCurrentEarn.Tables[0].Rows[0]["fDesc"].ToString();

            _dsTrial.Tables[0].AcceptChanges();
            if (Convert.ToDouble(_dsTrial.Tables[1].Rows[0]["Balance"]) != 0)
            {
                DataRow dr = _dsTrial.Tables[0].NewRow();
                _dsTrial.Tables[0].Rows.Add(_dsTrial.Tables[1].Rows[0].ItemArray);
            }

            _dsTrial.Tables[0].AcceptChanges();
            _dsTrial.Tables[0].DefaultView.Sort = "[fDesc] DESC";

            for (int i = 1; i < _dsTrial.Tables[0].Rows.Count; i++)
            {
                var description = _dsTrial.Tables[0].Rows[i]["fDesc"].ToString();
                if (description.ToLower().Contains(currentDesc.ToLower()))
                {
                    _dsTrial.Tables[0].Rows.Remove(_dsTrial.Tables[0].Rows[i]);
                }
            }
            _dsTrial.Tables[0].AcceptChanges();

            #region Calculate Total

            double _totalDebit = 0.00; double _totalCredit = 0.00;
            if (_dsTrial.Tables[0].Rows.Count > 0)
            {
                var _acctResult = (from row in _dsTrial.Tables[0].AsEnumerable()
                                   group row by row.Field<Int32>("Acct") into grp
                                   select new
                                   {
                                       Id = grp.Key,
                                       sum = grp.Sum(r => Convert.ToDouble(r["Amount"]))
                                   }).ToList();

                _totalDebit = Convert.ToDouble(_acctResult.Where(d => d.sum > 0).Sum(m => m.sum)); //Sum Debit Amount
                _totalCredit = Convert.ToDouble(_acctResult.Where(d => d.sum < 0).Sum(m => (m.sum * -1))); //Sum Credit Amount
            }

            double _totalAmount = 0.00; double _askDebitAmt = 0.00; double _askCreditAmt = 0.00; double _balanceAmt = 0.00;
            _totalAmount = _totalDebit - _totalCredit;

            #region check Total debit credit Balance

            if (_totalDebit > _totalCredit)
            {
                _balanceAmt = _totalDebit;
            }
            else
            {
                _balanceAmt = _totalCredit;
            }

            #endregion

            if (_totalAmount < 0)
            {
                _askDebitAmt = _totalAmount * -1;
                _askCreditAmt = 0.00;
            }
            else
            {
                _askDebitAmt = 0.00;
                _askCreditAmt = _totalAmount;
            }

            var newDt = new DataTable();
            newDt.Columns.Add("fDesc");
            newDt.Columns.Add("Debit");
            newDt.Columns.Add("Credit");
            newDt.Columns.Add("Acct");
            newDt.Columns.Add("Url");
            newDt.Columns.Add("Type");
            var currDesc = "";
            var newDesc = "";
            var acct = "";
            var type = "";

            double tbAmount = 0.00;
            for (int i = 0; i < _dsTrial.Tables[0].Rows.Count; i++)
            {
                currDesc = _dsTrial.Tables[0].Rows[i]["fDesc"].ToString();

                if (!currDesc.ToLower().Contains("current earnings"))
                {
                    acct = _dsTrial.Tables[0].Rows[i]["Acct"].ToString();
                    type = _dsTrial.Tables[0].Rows[i]["Type"].ToString();
                    newDesc = "";

                    if (i + 1 < _dsTrial.Tables[0].Rows.Count)
                    {
                        newDesc = _dsTrial.Tables[0].Rows[i + 1]["fDesc"].ToString();
                    }

                    if (currDesc == newDesc)
                    {
                        tbAmount += double.Parse(_dsTrial.Tables[0].Rows[i]["Amount"].ToString());
                    }
                    else
                    {
                        tbAmount += double.Parse(_dsTrial.Tables[0].Rows[i]["Amount"].ToString());
                        DataRow dr = newDt.NewRow();
                        dr["fDesc"] = currDesc;
                        dr["Acct"] = acct;
                        if (tbAmount > 0)

                        {
                            dr["Credit"] = 0.00;
                            dr["Debit"] = tbAmount;
                        }
                        else
                        {
                            dr["Debit"] = 0.00;
                            dr["Credit"] = tbAmount * -1;
                        }

                        newDt.Rows.Add(dr);
                        tbAmount = 0.00;
                    }
                }

            }

            foreach (DataRow dr in newDt.Rows)
            {
                String _date = "";
                if (dr["Type"].ToString() == "0" || dr["Type"].ToString() == "1" || dr["Type"].ToString() == "2" || dr["Type"].ToString() == "6")
                {
                    //set date
                    _date = "01/01/1980";
                }
                else
                {
                    // set date 
                    _date = _objChart.StartDate.ToShortDateString();
                }

                String Path = Request.Url.Scheme
                              + (Uri.SchemeDelimiter +
                                    (Request.Url.Authority +
                                        (Request.ApplicationPath + "/accountledger.aspx?id=" + dr["Acct"].ToString() + "&s=" +
                                        _date + "&e=" + _objChart.EndDate.ToShortDateString().ToString()
                                        )
                                    )
                                );

                dr["Url"] = Path;
            }

            newDt.AcceptChanges();
            var dView = newDt.DefaultView;
            dView.Sort = "[fDesc] ASC";

            DataTable dt = dView.ToTable().Copy();
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                var description = dt.Rows[i]["fDesc"].ToString();
                if (description.ToLower().Contains("current earnings"))
                {
                    dt.Rows.Remove(dt.Rows[i]);
                }
            }

            DataSet finalDS = new DataSet();
            finalDS.Tables.Add(dt);

            #endregion

            report.Dictionary.Variables["paramSDate"].Value = _objChart.StartDate.ToShortDateString();
            report.Dictionary.Variables["paramEDate"].Value = _asOfDate;
            report.Dictionary.Variables["paramTotalDebit"].Value = _balanceAmt.ToString("0.00", CultureInfo.InvariantCulture);
            report.Dictionary.Variables["paramTotalCredit"].Value = _balanceAmt.ToString("0.00", CultureInfo.InvariantCulture);
            report.Dictionary.Variables["paramAskDebit"].Value = _askDebitAmt.ToString("0.00", CultureInfo.InvariantCulture);
            report.Dictionary.Variables["paramAskCredit"].Value = _askCreditAmt.ToString("0.00", CultureInfo.InvariantCulture);
            report.Dictionary.Variables["paramUsername"].Value = Session["Username"].ToString();

            report.RegData("SumTable", finalDS.Tables[0]);
            report.RegData("dsTrialBalance", _dsTrial.Tables[0]);
            report.RegData("dsCompany", dsC.Tables[0]);
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

    private StiReport LoadTrialBalanceActivityReport()
    {
        try
        {
            string reportPathStimul = Server.MapPath("StimulsoftReports/TrialBalanceActivity.mrt");
            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            _objChart.ConnConfig = Session["config"].ToString();
            string strQuery = "select [dbo].[Control].[YE] from [dbo].[control]";
            int? yearEnd;
            int startMonth = 0;

            var ds1 = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, strQuery);
            if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
            {
                yearEnd = int.Parse(ds1.Tables[0].Rows[0][0].ToString());
                startMonth = int.Parse(yearEnd.ToString()) + 2;

                if (startMonth > 12)
                {
                    startMonth = startMonth - 12;
                }
            }

            if (!string.IsNullOrEmpty(Request.QueryString["sd"]))
            {
                _objChart.StartDate = Convert.ToDateTime(HttpUtility.UrlDecode(Request.QueryString["sd"]));
            }
            else
            {
                var now = DateTime.Now;
                _objChart.StartDate = new DateTime(now.Year, now.Month, 1);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["ed"]))
            {
                _objChart.EndDate = Convert.ToDateTime(HttpUtility.UrlDecode(Request.QueryString["ed"]));
            }
            else
            {
                _objChart.EndDate = DateTime.Now.Date;
            }

            var fiscalYear = _objChart.EndDate.Year;
            if (startMonth != 1)
            {
                if (_objChart.EndDate.Month < startMonth)
                    fiscalYear = fiscalYear - 1;
            }

            _objChart.YearStartDate = new DateTime(fiscalYear, startMonth, 1);

            #region Set Header

            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objPropUser);

            #endregion

            var isCloseOut = false;
            _journal.ConnConfig = Session["config"].ToString();
            _journal.Internal = _objChart.EndDate.Year.ToString();
            var jes = objBL_JournalEntry.GetYearEndClosedOutData(_journal);
            if (jes != null)
            {
                if (jes.Tables[0].Rows.Count > 0)
                {
                    var date = Convert.ToDateTime(jes.Tables[0].Rows[0]["fDate"].ToString());
                    if (_objChart.EndDate == date)
                    {
                        isCloseOut = true;
                    }
                }
            }

            DataSet _dsTrial = _objBLReport.GetTrialBalanceActivity(_objChart, isCloseOut);
            BL_Chart objBL_Chart = new BL_Chart();

            DataSet _dsRetainedEarn = objBL_Chart.GetRetainedEarn(_objChart);
            DataSet _dsCurrentEarn = objBL_Chart.GetCurrentEarn(_objChart);

            int retAcct = Convert.ToInt32(_dsRetainedEarn.Tables[0].Rows[0]["ID"]);
            string currentDesc = _dsCurrentEarn.Tables[0].Rows[0]["fDesc"].ToString();

            _dsTrial.Tables[0].AcceptChanges();
            _dsTrial.Tables[0].DefaultView.Sort = "[fDesc] DESC";

            for (int i = 1; i < _dsTrial.Tables[0].Rows.Count; i++)
            {
                var description = _dsTrial.Tables[0].Rows[i]["fDesc"].ToString();
                if (description.ToLower().Contains(currentDesc.ToLower()))
                {
                    _dsTrial.Tables[0].Rows.Remove(_dsTrial.Tables[0].Rows[i]);
                }
            }
            _dsTrial.Tables[0].AcceptChanges();

            foreach (DataRow dr in _dsTrial.Tables[0].Rows)
            {
                String _date = "";
                if (dr["Type"].ToString() == "0" || dr["Type"].ToString() == "1" || dr["Type"].ToString() == "2" || dr["Type"].ToString() == "6")
                {
                    //set date
                    _date = "01/01/1980";
                }
                else
                {
                    // set date 
                    _date = _objChart.YearStartDate.ToShortDateString();
                }

                String Path = Request.Url.Scheme
                              + (Uri.SchemeDelimiter +
                                    (Request.Url.Authority +
                                        (Request.ApplicationPath + "/accountledger.aspx?id=" + dr["Acct"].ToString() + "&s=" +
                                        _date + "&e=" + _objChart.EndDate.ToShortDateString().ToString()
                                        )
                                    )
                                );

                dr["Url"] = Path;
            }

            _dsTrial.Tables[0].AcceptChanges();
            var dView = _dsTrial.Tables[0].DefaultView;
            dView.Sort = "[fDesc] ASC";

            DataTable dt = dView.ToTable().Copy();

            report.Dictionary.Variables["StartDate"].Value = _objChart.StartDate.ToShortDateString();
            report.Dictionary.Variables["EndDate"].Value = _objChart.EndDate.ToShortDateString();
            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();

            report.RegData("dsTrialBalance", dt);
            report.RegData("dsCompany", dsC.Tables[0]);
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
            if (DownloadFileName == "TrialBalance.pdf")
            {
                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(LoadTrialBalanceReport(), stream, settings);
                buffer1 = stream.ToArray();

                Response.Clear();
                MemoryStream ms = new MemoryStream(buffer1);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=TrialBalance.pdf");
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