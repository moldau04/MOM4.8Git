using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Web.Script.Serialization;
using System.Net.Configuration;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Threading;
using Stimulsoft.Report.Web;
using Stimulsoft.Report;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using Telerik.Web.UI;

public partial class CustomerStatementReport : System.Web.UI.Page
{
    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objContract = new Contracts();

    BL_Report objBL_Report = new BL_Report();

    BL_User objBL_User = new BL_User();
    User objPropUser = new User();

    DataSet _dsCom = new DataSet();
    General objGeneral = new General();
    BL_General objBL_General = new BL_General();

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
                ddlcustomer.DataBind();
                if (Request.QueryString["uid"] != null)
                {
                    Session["Customer"] = Request.QueryString["uid"].ToString();
                }
                if (Session["Customer"] != null)
                {
                    StiWebViewerStatement.Visible = true;
                    var routes = Session["Customer"].ToString().Split(',');
                    foreach (var route in routes)
                    {
                        var item = ddlcustomer.Items.FindItem(x => x.Value == route);
                        if (item != null)
                        {
                            item.Checked = true;
                        }
                    }
                }
                if (Request.QueryString["page"] != null && Request.QueryString["page"] == "invoices")
                {
                    HighlightSideMenu("acctMgr", "lnkInvoicesSMenu", "billMgrSub");
                }
                else
                {
                    HighlightSideMenu("cstmMgr", "lnkCustomersSmenu", "cstmMgrSub");
                }

                if (Request.QueryString["includeCredit"] != null)
                {
                    chkIncludeCredit.Checked = Convert.ToBoolean(Request.QueryString["includeCredit"]);
                }

                if (Request.QueryString["includeCustomerCredit"] != null)
                {
                    chkIncludeCustomerCredit.Checked = Convert.ToBoolean(Request.QueryString["includeCustomerCredit"]);
                }

                objPropUser.ConnConfig = Session["config"].ToString();
                _dsCom = objBL_User.getControl(objPropUser);

                if (_dsCom.Tables[0].Rows.Count > 0)
                {
                    string address = _dsCom.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
                    address += _dsCom.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
                    address += _dsCom.Tables[0].Rows[0]["city"].ToString() + ", " + _dsCom.Tables[0].Rows[0]["state"].ToString() + ", " + _dsCom.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
                    address += "Phone: " + _dsCom.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
                    address += "Fax: " + _dsCom.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;
                    address += "Email: " + _dsCom.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;
                    address = "Please review the attached customer statement from: " + Environment.NewLine + Environment.NewLine + address;
                    ViewState["company"] = address;
                    txtBody.Text = address;
                }
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
        Session["Customer"] = null;
        if (Request.QueryString["page"] != null)
        {
            var pageUrl = Request.QueryString["page"].ToString() + ".aspx";
            if (Request.QueryString["page"] == "invoices")
            {
                Response.Redirect("Invoices.aspx?fil=1");
            }
            else
            {
                if (Request.QueryString["uid"] != null)
                {
                    pageUrl += "?uid=" + Request.QueryString["uid"];
                }
                Response.Redirect(pageUrl);
            }
        }
        else
        {
            Response.Redirect("invoices.aspx");
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (ddlcustomer.CheckedItems.Count == 0)
        {
            //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "FileaccessWarning", "alert('Please Select Category!');", true);
            Session["Customer"] = null;
            ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Please Select Customer!',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        else
        {
            var routes = GetRadComboBoxSelectedItems(ddlcustomer);
            Session["Customer"] = routes.ToString();
            var url = "CustomerStatementReport.aspx?includeCredit=" + chkIncludeCredit.Checked + "&includeCustomerCredit=" + chkIncludeCustomerCredit.Checked;
            if (Request.QueryString["page"] != null)
            {
                url += "&page=" + Request.QueryString["page"];
            }

            //if (Request.QueryString["uid"] != null)
            //{
            //    url += "&uid=" + Request.QueryString["uid"];
            //}

            Response.Redirect(url);
        }
    }

    protected void StiWebViewerStatement_GetReport(object sender, StiReportDataEventArgs e)
    {
        e.Report = LoadReport();
    }

    protected void StiWebViewerStatement_GetReportData(object sender, StiReportDataEventArgs e)
    {

    }

    private StiReport LoadReport()
    {
        try
        {
            string reportPathStimul = Server.MapPath("StimulsoftReports/CustomerStatement.mrt");

            string reportCustom = System.Web.Configuration.WebConfigurationManager.AppSettings["CustomerInvoieStatement"];
            if (!string.IsNullOrEmpty(reportCustom) && reportCustom.ToLower().Contains(".mrt"))
            {
                reportPathStimul = Server.MapPath($"StimulsoftReports/{reportCustom}");
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
            if (!string.IsNullOrEmpty(Request.QueryString["uid"]))
            {
                objContract.Owner = Convert.ToInt32(Request.QueryString["uid"]);
            }

            #region Company Check
            objContract.UserID = Session["UserID"].ToString();
            if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            {
                objContract.EN = 1;
            }
            else
            {
                objContract.EN = 0;
            }
            #endregion

            var includeCredit = true;
            var includeCustomerCredit = true;

            if (!string.IsNullOrEmpty(Request.QueryString["includeCredit"]))
            {
                includeCredit = Convert.ToBoolean(Request.QueryString["includeCredit"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["includeCustomerCredit"]))
            {
                includeCustomerCredit = Convert.ToBoolean(Request.QueryString["includeCustomerCredit"]);
            }
            string[] Customer = new string[] { };
            string Cust = null;
            if (Session["Customer"] != null)
            {
                Customer = Session["Customer"].ToString().Split(',');
                Cust = Session["Customer"].ToString();
            }

            //DataSet ds = objBL_Contracts.GetCustomerStatement(objContract, includeCredit, includeCustomerCredit);

            DataSet ds = objBL_Contracts.GetCustomerStatementByCustomer(objContract, includeCredit, includeCustomerCredit, Cust);
            if (ds != null)
            {
                report.RegData("Invoices", ds.Tables[0]);

                DataSet dsItem = objBL_Contracts.GetCustomerStatementInvoicesByOwnerByCustId(objContract, includeCredit, Cust);
                if (dsItem != null)
                {
                    report.RegData("InvoiceItem", dsItem.Tables[0]);
                }
            }

            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                report.Dictionary.Variables["IsEmpty"].Value = "true";
            }

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

    protected void lnkMailReport_Click(object sender, EventArgs e)
    {
        try
        {
            string _todayDate = DateTime.Now.Date.ToString("MM-dd-yyyy");

            objContract.ConnConfig = Session["config"].ToString();
            if (!string.IsNullOrEmpty(Request.QueryString["uid"]))
            {
                objContract.Owner = Convert.ToInt32(Request.QueryString["uid"]);
            }

            #region Company Check
            objContract.UserID = Session["UserID"].ToString();
            if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            {
                objContract.EN = 1;
            }
            else
            {
                objContract.EN = 0;
            }
            #endregion

            var includeCredit = true;
            var includeCustomerCredit = true;

            if (!string.IsNullOrEmpty(Request.QueryString["includeCredit"]))
            {
                includeCredit = Convert.ToBoolean(Request.QueryString["includeCredit"]);
            }

            if (!string.IsNullOrEmpty(Request.QueryString["includeCustomerCredit"]))
            {
                includeCustomerCredit = Convert.ToBoolean(Request.QueryString["includeCustomerCredit"]);
            }

            DataSet ds = objBL_Contracts.GetCustomerStatement(objContract, includeCredit, includeCustomerCredit);
            DataTable dtInv = ds.Tables[0];

            DataTable dt = dtInv.AsEnumerable()
                       .GroupBy(r => r.Field<int>("Loc"))
                       .Select(g => g.First())
                       .CopyToDataTable();

            string _fromEmail = WebBaseUtility.GetFromEmailAddress();

            List<string> lstLoc = new List<string>();
            string strLoc;
            bool isUnscuss = false;
            int mailCount = 0;
            foreach (DataRow _dr in dt.Rows)
            {
                string _toEmail = "";
                string _ccEmail = "";
                if (!string.IsNullOrEmpty(_dr["custom12"].ToString()))
                {
                    #region Generate Report

                    int loc = Convert.ToInt32(_dr["Loc"]);
                    DataTable dtInvoice = dtInv
                            .Select("Loc = " + loc)
                            .CopyToDataTable();

                    if (mailCount == 4)
                    {
                        Thread.Sleep(10000);
                        mailCount = 0;
                    }
                    #endregion

                    #region Email

                    _toEmail = _dr["custom12"].ToString();

                    if (!string.IsNullOrEmpty(_dr["custom13"].ToString()))
                    {
                        _ccEmail = _dr["custom13"].ToString();
                    }

                    List<string> _toEmaillst = new List<string>();
                    _toEmaillst = _toEmail.Split(';', ',').OfType<string>().ToList();
                    List<string> _ccEmaillst = new List<string>();
                    _ccEmaillst = _ccEmail.Split(';', ',').OfType<string>().ToList();

                    Mail mail = new Mail();
                    mail.From = _fromEmail;
                    mail.To = _toEmaillst;
                    mail.Cc = _ccEmaillst;

                    mail.Title = "Customer Statement - " + _dr["LocID"].ToString() + " " + _dr["locname"].ToString();

                    if (txtBody.Text.Trim() != string.Empty)
                    {
                        mail.Text = txtBody.Text.Replace("\n", "<BR/>");
                    }
                    else
                    {
                        mail.Text = ViewState["company"].ToString().Replace(Environment.NewLine, "<BR/>");
                    }
                    mail.attachmentBytes = GetReportAsAttachmentFile(dtInvoice);
                    mail.FileName = "Customer_Statement" + _dr["LocID"].ToString() + "_" + _todayDate + ".pdf";

                    mail.DeleteFilesAfterSend = true;
                    mail.RequireAutentication = false;
                    WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);

                    mail.Send();
                    mailCount = mailCount + 1;

                    #endregion
                }
                else
                {
                    lstLoc.Add(_dr["locname"].ToString());
                    isUnscuss = true;
                }
            }

            if (isUnscuss)
            {
                strLoc = lstLoc.Aggregate((x, y) => x + ", " + y);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "unsuccessMesg('" + strLoc + "');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Send all emails successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private byte[] GetReportAsAttachmentFile(DataTable dtInvoices)
    {
        try
        {
            DataTable dtInv = dtInvoices;

            if (dtInv != null)
            {
                if (dtInv.Rows.Count > 0)
                {
                    List<byte[]> invoicesToPrint = new List<byte[]>();
                    invoicesToPrint = PrintStatement(dtInv);

                    if (invoicesToPrint != null)
                    {
                        byte[] buffer1 = null;
                        buffer1 = concatAndAddContent(invoicesToPrint);

                        return buffer1;
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

    private List<byte[]> PrintStatement(DataTable dtInvoices)
    {
        // Export to PDF
        List<StiReport> reports = new List<StiReport>();
        List<byte[]> invoicesAsBytes = new List<byte[]>();
        DataTable mergedInvoiceItems = new DataTable();

        string reportPathStimul = Server.MapPath("StimulsoftReports/CustomerStatement.mrt");
        string reportCustom = System.Web.Configuration.WebConfigurationManager.AppSettings["CustomerInvoieStatement"];
        if (!string.IsNullOrEmpty(reportCustom) && reportCustom.ToLower().Contains(".mrt"))
        {
            reportPathStimul = Server.MapPath($"StimulsoftReports/{reportCustom}");
        }

        try
        {
            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            objContract.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objContract.isTS = 1;
            }

            foreach (DataRow dr in dtInvoices.Rows)
            {
                int loc = Convert.ToInt32(dr["Loc"]);

                StiReport report = new StiReport();
                report.Load(reportPathStimul);
                //report.Compile();

                objPropUser.ConnConfig = Session["config"].ToString();
                var dsCom = objBL_User.getControl(objPropUser);

                objContract.ConnConfig = Session["config"].ToString();
                objContract.Loc = loc;

                var includeCredit = true;
                if (!string.IsNullOrEmpty(Request.QueryString["includeCredit"]))
                {
                    includeCredit = Convert.ToBoolean(Request.QueryString["includeCredit"]);
                }

                var dsItem = objBL_Contracts.GetCustomerStatementInvoices(objContract, includeCredit);

                report.CacheAllData = true;
                report.RegData("CompanyDetails", dsCom.Tables[0]);
                report.RegData("Invoices", dtInvoices);
                report.RegData("InvoiceItem", dsItem.Tables[0]);
                report.Dictionary.Synchronize();
                report.Render();
                reports.Add(report);
            }

            for (int i = 0; i < reports.Count; i++)
            {
                byte[] buffer1 = null;
                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(reports[i], stream, settings);
                buffer1 = stream.ToArray();
                invoicesAsBytes.Add(buffer1);
            }

            return invoicesAsBytes;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return invoicesAsBytes;
        }
    }

    public static byte[] concatAndAddContent(List<byte[]> pdfByteContent)
    {
        MemoryStream ms = new MemoryStream();
        Document doc = new Document();
        PdfSmartCopy copy = new PdfSmartCopy(doc, ms);

        doc.Open();

        //Loop through each byte array
        foreach (var p in pdfByteContent)
        {
            PdfReader reader = new PdfReader(p);
            int n = reader.NumberOfPages;

            for (int i = 1; i <= n; i++)
            {
                byte[] red = reader.GetPageContent(i);
                if (red.Length < 1000)
                {
                    n = n - 1;
                }
            }
            for (int page = 0; page < n;)
            {
                copy.AddPage(copy.GetImportedPage(reader, ++page));
            }
        }
        doc.Close();

        return ms.ToArray();
    }

    protected void RadComboBox1_DataBinding(object sender, EventArgs e)
    {
        ddlcustomer.DataValueField = "ID";
        ddlcustomer.DataTextField = "Name";
        ddlcustomer.DataSource = GetSampleSource();
    }

    private DataSet GetSampleSource()
    {
        DataSet ds = new DataSet();
        objGeneral.ConnConfig = Session["config"].ToString();
        ds = objBL_General.getAllCustomerList(objGeneral);
        return ds;
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
}