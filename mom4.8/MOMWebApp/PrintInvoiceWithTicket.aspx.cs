using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.Reporting.WebForms;
using Microsoft.ReportingServices.ReportRendering;
using System.IO;
using System.Web.Script.Serialization;
using System.Net.Configuration;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Threading;
using System.Net.Mail;
using System.Configuration;

public partial class PrintInvoiceWithTicket : System.Web.UI.Page
{
    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    GeneralFunctions objGen = new GeneralFunctions();
    int count_inv = 0;

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    bool IsGst = false;

    #region Events
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        if (Request.QueryString["o"] != null)
        {
            Page.MasterPageFile = "popup.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();
        if (SSL == "1")
        {
            bool isLocal = HttpContext.Current.Request.IsLocal;
            if (!isLocal)
            {
                bool isSecure = HttpContext.Current.Request.IsSecureConnection;
                string webPath = System.Web.Configuration.WebConfigurationManager.AppSettings["webPath"].Trim();
                if (!isSecure)
                {
                    if (Session["type"].ToString() == "c")
                    {
                        bool port = HttpContext.Current.Request.Url.IsDefaultPort;
                        string Auth = HttpContext.Current.Request.Url.Authority;
                        if (!port)
                        {
                            Auth = HttpContext.Current.Request.Url.DnsSafeHost;
                        }
                        string URL = Auth + webPath;
                        string redirect = "HTTPS://" + URL + "/PrintInvoiceWithTicket.aspx";
                        int ii = 0;
                        foreach (String key in Request.QueryString.AllKeys)
                        {
                            if (ii == 0)
                                redirect += "?" + key + "=" + Request.QueryString[key];
                            else
                                redirect += "&" + key + "=" + Request.QueryString[key];
                            ii++;
                        }
                        Response.Redirect(redirect);
                    }
                }
            }
        }

        if (!IsPostBack)
        {
            ShowGstRate();

            if (Session["type"].ToString() == "c")
            {
                lnkPayment.Visible = true;
            }

            if (Request.QueryString["cl"] != null)
            {
                lnkCancelContact.Visible = false;
            }

            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            /****Get from MS_Invoice tables the invoices masrked as pending from Mobile Service in case of TS database****/
            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            objProp_Contracts.InvoiceID = Int32.Parse(Request.QueryString["uid"].ToString());
            ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);

            DataTable dtNew = ds.Tables[0];

            ViewState["amount"] = ds.Tables[0].Rows[0]["balance"].ToString();
            string paid = ds.Tables[0].Rows[0]["paidcc"].ToString();
            string status = ds.Tables[0].Rows[0]["status"].ToString();

            if (status == "0" && paid == "0")
                lnkPayment.Visible = true;
            else
                lnkPayment.Visible = false;

            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.Username = Session["username"].ToString();
            txtFrom.Text = objBL_User.getUserEmail(objPropUser);

            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() != "TS")
            {
                dsC = objBL_User.getControl(objPropUser);
            }
            else
            {
                objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
                dsC = objBL_User.getControlBranch(objPropUser);
            }

            ViewState["RecurCompany"] = dsC.Tables[0];
            ViewState["EmailFrom"] = "";

            if (dsC.Tables[0].Rows.Count > 0)
            {
                if (Session["MSM"].ToString() != "TS")
                {
                    if (txtFrom.Text.Trim() == string.Empty)
                    {
                        txtFrom.Text = dsC.Tables[0].Rows[0]["Email"].ToString();
                        ViewState["EmailFrom"] = dsC.Tables[0].Rows[0]["Email"].ToString();
                    }
                }

                string address = dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
                address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
                address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
                address += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
                address += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;
                address += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;
                address = "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine + address;
                ViewState["company"] = address;
                txtBody.Text = address;
            }

            if (txtFrom.Text.Trim() == string.Empty)
            {
                System.Configuration.Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
                MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
                string username = mailSettings.Smtp.Network.UserName;
                txtFrom.Text = username;
                ////txtFrom.ReadOnly = true;
            }

            string subject = string.Empty;

            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
            DataSet dsloc = new DataSet();
            dsloc = objBL_User.getLocationByID(objPropUser);

            if (dsloc.Tables[0].Rows.Count > 0)
            {
                if (Session["MSM"].ToString() != "TS")
                {
                    txtTo.Text = dsloc.Tables[0].Rows[0]["custom12"].ToString();
                    txtCC.Text = dsloc.Tables[0].Rows[0]["custom13"].ToString();
                }
                subject = dsloc.Tables[0].Rows[0]["tag"].ToString();
            }

            ViewState["subject"] = subject;
            ViewState["RecurrInvoice"] = dtNew;
            ViewState["InvoicesSubReportResult"] = dtNew;

            GenerateReport(rvRecInvoices, Convert.ToInt32(Request.QueryString["uid"].ToString()));
        }

        if (Convert.ToInt16(Session["payment"]) != 1)
        {
            lnkPayment.Visible = false;
        }

        permission();
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["page"] != null && Request.QueryString["page"].ToString() == "recurringinvoices")
        {
            Response.Redirect("recurringinvoices.aspx");
        }

        if (Session["type"].ToString() != "c")
        {
            if (Session["MSM"].ToString() != "TS")
            {
                if (Request.QueryString["o"] == null)
                {
                    Response.Redirect("addinvoice.aspx?uid=" + Request.QueryString["uid"].ToString());
                }
                else
                {
                    Response.Redirect("addinvoice.aspx?uid=" + Request.QueryString["uid"].ToString() + "&o=1");
                }
            }
            else
            {
                Response.Redirect("invoices.aspx?fil=1");
            }
        }
        else
        {
            Response.Redirect("invoices.aspx?fil=1");
        }
    }

    protected void btnYes_Click(object sender, EventArgs e)
    {

        ReportViewer _reporviewer2 = new ReportViewer();
        DataSet ds = new DataSet();
        DataSet dsInv = new DataSet();
        objProp_Contracts.ConnConfig = Session["config"].ToString();

        /****Get from MS_Invoice tables the invoices masrked as pending from Mobile Service in case of TS database****/
        if (Session["MSM"].ToString() == "TS")
        {
            if (Session["type"].ToString() != "c")
                objProp_Contracts.isTS = 1;
        }

        if (!string.IsNullOrEmpty(Request.QueryString["uid"]))
        {
            #region Bind ReportViewer

            objProp_Contracts.InvoiceID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);

            DataTable dtNew = ds.Tables[0];
            ViewState["RecurrInvoice"] = dtNew;

            string paid = ds.Tables[0].Rows[0]["paidcc"].ToString();
            string status = ds.Tables[0].Rows[0]["status"].ToString();

            if (status == "0" && paid == "0")
                lnkPayment.Visible = true;
            else
                lnkPayment.Visible = false;

            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.Username = Session["username"].ToString();
            txtFrom.Text = objBL_User.getUserEmail(objPropUser);

            GenerateReport(_reporviewer2, Convert.ToInt32(Request.QueryString["uid"].ToString()));
            #endregion

            Response.ContentType = "Application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; InvoiceWithTicket-" + Request.QueryString["uid"].ToString() + ".pdf");

            byte[] getPDF = ExportReportToPDF1("", _reporviewer2);
            Response.Write(getPDF);
            MemoryStream ms = new MemoryStream(getPDF);
            ms.WriteTo(Response.OutputStream);
            ms.Close();
            Response.Flush();
        }
    }

    protected void btnNo_Click(object sender, EventArgs e)
    {
        Response.Redirect("Printinvoices.aspx?uid=" + Request.QueryString["uid"].ToString() + "&eid=" + Request.QueryString["eid"].ToString());
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
                mail.Title = txtSubject.Text.Trim();
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace("\n", "<BR/>");
                }
                else
                {
                    mail.Text = ViewState["company"].ToString().Replace(Environment.NewLine, "<BR/>");
                }

                mail.FileName = "Invoice-" + Request.QueryString["uid"].ToString() + ".pdf";
                mail.attachmentBytes = ExportReportToPDF("");

                mail.DeleteFilesAfterSend = true;
                mail.RequireAutentication = false;
                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);

                mail.Send();
                this.programmaticModalPopup.Hide();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
            catch (Exception ex)
            {
                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }

    protected void lnkPayment_Click(object sender, EventArgs e)
    {
        if (Convert.ToDouble(objGen.IsNull(ViewState["amount"].ToString(), "0")) == 0)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Amount can not be zero.',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            return;
        }
        bool port = HttpContext.Current.Request.Url.IsDefaultPort;
        string Auth = HttpContext.Current.Request.Url.Authority;
        if (!port)
        {
            Auth = HttpContext.Current.Request.Url.DnsSafeHost;
        }
        string webPath = System.Web.Configuration.WebConfigurationManager.AppSettings["webPath"].Trim();
        string URL = Auth + webPath;
        string strQuery = "[{'inv':'" + Request.QueryString["uid"].ToString() + "','amt':'" + ViewState["amount"].ToString() + "'}]";
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<string, string>> lstInv = sr.Deserialize<List<Dictionary<string, string>>>(strQuery);
        Session["uidv"] = lstInv;
        string paymentscreen = System.Web.Configuration.WebConfigurationManager.AppSettings["PayGateway"].Trim();
        Response.Redirect("https://" + URL + paymentscreen);
    }

    protected void lnkMailReport_Click(object sender, EventArgs e)
    {
        try
        {
            string _todayDate = DateTime.Now.Date.ToString("MM-dd-yyyy");
            DataTable dtInv = (DataTable)ViewState["RecurrInvoice"];
            DataTable dtC = (DataTable)ViewState["RecurCompany"];
            DataTable dt = dtInv.AsEnumerable()
                       .GroupBy(r => r.Field<int>("Loc"))
                       .Select(g => g.First())
                       .CopyToDataTable();
            string _fromEmail = ViewState["EmailFrom"].ToString();
            if (string.IsNullOrEmpty(_fromEmail))
            {
                _fromEmail = GetFromEmailAddress();
            }
            List<string> lstLoc = new List<string>();
            string strLoc;
            bool isUnscuss = false;
            int mailCount = 0;
            foreach (DataRow _dr in dt.Rows)
            {
                count_inv = 0;
                int _ref = Convert.ToInt32(_dr["Ref"]);
                objProp_Contracts.ConnConfig = Session["config"].ToString();
                objProp_Contracts.Ref = _ref;
                DataSet _dsCon = objBL_Contracts.GetEmailDetailByLoc(objProp_Contracts);
                if (_dsCon.Tables[0].Rows.Count > 0)
                {
                    string _toEmail = "";
                    string _ccEmail = "";
                    if (!string.IsNullOrEmpty(_dsCon.Tables[0].Rows[0]["custom12"].ToString()))
                    {
                        #region Generate Report

                        int loc = Convert.ToInt32(_dr["Loc"]);
                        DataTable dtRecur = dtInv
                                .Select("Loc = " + loc)
                                .CopyToDataTable();

                        ViewState["InvoicesSubReportResult"] = dtRecur;
                        ReportViewer rvInvoices = new ReportViewer();
                        GenerateReport(rvInvoices, Convert.ToInt32(Request.QueryString["uid"].ToString()));

                        if (mailCount == 4)
                        {
                            Thread.Sleep(10000);
                            mailCount = 0;
                        }

                        #endregion

                        #region Email

                        _toEmail = _dsCon.Tables[0].Rows[0]["custom12"].ToString();

                        if (!string.IsNullOrEmpty(_dsCon.Tables[0].Rows[0]["custom13"].ToString()))
                        {
                            _ccEmail = _dsCon.Tables[0].Rows[0]["custom13"].ToString();
                        }

                        List<string> _toEmaillst = new List<string>();
                        _toEmaillst.Add(_toEmail);

                        List<string> _ccEmaillst = new List<string>();
                        _ccEmaillst.Add(_ccEmail);

                        Mail mail = new Mail();
                        mail.From = _fromEmail;

                        foreach (var address in _toEmail.Split(','))
                        {
                            if (!string.IsNullOrEmpty(address))
                                mail.To.Add(new MailAddress(address.Trim(), "").Address);
                        }

                        foreach (var address in _ccEmail.Split(','))
                        {
                            if (!string.IsNullOrEmpty(address))
                                mail.Cc.Add(new MailAddress(address.Trim(), "").Address);
                        }
                        mail.Title = "InvoiceWithTicket - " + _dsCon.Tables[0].Rows[0]["ID"].ToString() + " " + _dsCon.Tables[0].Rows[0]["Tag"].ToString();

                        if (txtBody.Text.Trim() != string.Empty)
                        {
                            mail.Text = txtBody.Text.Replace("\n", "<BR/>");
                        }
                        else
                        {
                            mail.Text = ViewState["company"].ToString().Replace(Environment.NewLine, "<BR/>");
                        }
                        mail.attachmentBytes = ExportReportToPDF1("", rvInvoices);
                        mail.FileName = "Invoices_" + _todayDate + ".pdf";

                        mail.DeleteFilesAfterSend = true;
                        mail.RequireAutentication = false;
                        WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);

                        mail.Send();
                        mailCount = mailCount + 1;

                        #endregion
                    }
                    else
                    {
                        lstLoc.Add(_dsCon.Tables[0].Rows[0]["Tag"].ToString());
                        isUnscuss = true;
                    }
                }
            }
            if (isUnscuss)
            {
                strLoc = lstLoc.Aggregate((x, y) => x + ", " + y);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "unsuccessMesg('" + strLoc + "');", true);
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkPrint_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(Request.QueryString["uid"]))
            {
                string filename = "InvoiceWithTicket-" + Request.QueryString["uid"];
                ReportViewer rvInvoices = new ReportViewer();
                GenerateReport(rvInvoices, Convert.ToInt32(Request.QueryString["uid"].ToString()));
                byte[] getPDF = ExportReportToPDF1("", rvInvoices);

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + filename + ".pdf");
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Length", (getPDF.Length).ToString());
                Response.BinaryWrite(getPDF);
                Response.Flush();
                Response.Close();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No Invoice found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Custom functions

    private void GenerateReport(ReportViewer rvInvoices, int invoiceNo)
    {
        try
        {
            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            DataSet dsTicket = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataTable _dtInvoice = new DataTable();
            DataSet _dsInvoice = new DataSet();
            objProp_Contracts.InvoiceID = invoiceNo;
            ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);
            _dtInvoice = ds.Tables[0];

            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() != "TS")
            {
                dsC = objBL_User.getControl(objPropUser);
            }
            else
            {
                objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
                dsC = objBL_User.getControlBranch(objPropUser);
            }

            int _days = 0;
            for (int i = 0; i < _dtInvoice.Rows.Count; i++)
            {

                #region Determine Pay Terms
                if (_dtInvoice.Rows[i]["payterms"].ToString() == "0")
                {
                    _days = 0;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "1")
                {
                    _days = 10;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "2")
                {
                    _days = 15;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "3")
                {
                    _days = 30;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "4")
                {
                    _days = 45;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "5")
                {
                    _days = 60;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "6")
                {
                    _days = 30;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "7")
                {
                    _days = 90;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "8")
                {
                    _days = 180;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "9")
                {
                    _days = 0;
                }
                #endregion
                if (!string.IsNullOrEmpty(_dtInvoice.Rows[i]["IDate"].ToString()))
                {
                    _dtInvoice.Rows[i]["DueDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString()).AddDays(_days);
                    _dtInvoice.Rows[i]["fDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString());
                }
            }

            rvInvoices.LocalReport.DataSources.Clear();

            DataTable _dtInvItems = GetInvoiceItems(invoiceNo);

            //Get Ticket
            DataTable dtTicket = new DataTable();
            int ii = 0;

            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.InvoiceID = invoiceNo;

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataSet TicketID = objBL_Contracts.GetTicketID(objProp_Contracts);

            foreach (DataRow item in TicketID.Tables[0].Rows)
            {
                objMapData.ConnConfig = Session["config"].ToString();
                objMapData.TicketID = (int)item[0];
                dsTicket = objBL_MapData.GetTicketByID(objMapData);
                if (ii == 0)
                {
                    dtTicket = dsTicket.Tables[0];
                    ii++;
                }
                else
                {
                    dtTicket.Rows.Add(dsTicket.Tables[0].Rows[0].ItemArray);
                    ii++;
                }
            }

            List<ReportParameter> param1 = new List<ReportParameter>();
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("dtInvoiceItems", _dtInvItems));
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("Invoice_dtInvoice", _dtInvoice));
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicket", dtTicket));

            if (dtTicket.Rows.Count > 0)
            {
                param1.Add(new ReportParameter("ISTicket", "1"));
            }
            else
            {
                param1.Add(new ReportParameter("ISTicket", "0"));
            }

            var reportName = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceLNYWithTicket"].Trim();
            string reportPath = "Reports/" + reportName;

            rvInvoices.LocalReport.ReportPath = reportPath;
            rvInvoices.LocalReport.EnableExternalImages = true;
            string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
            param1.Add(new ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
            param1.Add(new ReportParameter("IsGstTax", ViewState["IsGst"].ToString()));
            rvInvoices.LocalReport.SetParameters(param1);
            rvInvoices.LocalReport.Refresh();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private DataTable GetInvoiceItems(int _refId)
    {
        DataTable _dtItem = new DataTable();
        try
        {
            objProp_Contracts.InvoiceID = _refId;
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            DataSet _dsItemDetails = objBL_Contracts.GetInvoiceItemByRef(objProp_Contracts);
            if (_dsItemDetails.Tables[0].Rows.Count < 1)
            {
                _dtItem = LoadInvoiceDetails(_dsItemDetails.Tables[0], _refId);
            }
            else
                _dtItem = _dsItemDetails.Tables[0];
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

        return _dtItem;
    }

    private void ItemSubReportProcessing(object sender, SubreportProcessingEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["InvoicesSubReportResult"];
            DataTable dtItems = new DataTable();

            objProp_Contracts.InvoiceID = Convert.ToInt32(dt.Rows[count_inv]["Ref"]);
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            DataSet ds = objBL_Contracts.GetInvoiceItemByRef(objProp_Contracts);
            if (ds.Tables[0].Rows.Count < 1)
            {
                dtItems = LoadInvoiceDetails(ds.Tables[0], objProp_Contracts.InvoiceID);
            }
            else
                dtItems = ds.Tables[0];

            ReportDataSource rdsItems = null;
            if (dtItems.Rows.Count > 0)
            {
                string sessval = (string)Session["InvoiceName"];
                string Report = string.Empty;

                if (sessval == "Invoice")
                {
                    Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesReport"].Trim();
                }
                if (sessval == "InvoiceMaint")
                {
                    Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesMaintReport"].Trim();
                }

                if (sessval == "InvoiceException")
                {
                    Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesExceptionReport"].Trim();
                }

                if (sessval == "Invoice-LNY")
                {
                    Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceLNY"].Trim();
                }

                if (sessval == "Invoice")
                {
                    if (Report == "Madden_Invoices.rdlc" || Report == "InvoicesAdams.rdlc" || Report == string.Empty || Report == "InvoicesInFrench.rdlc")
                    {
                        if (!string.IsNullOrEmpty(Report.Trim()))
                        {
                            rdsItems = new ReportDataSource("dtInvoiceItems", dtItems);
                        }
                        else
                        {
                            rdsItems = new ReportDataSource("dtInvoiceItems", dtItems);
                        }
                    }
                }
                else if (sessval == "InvoiceMaint")
                {
                    if (Report == "PESMTC_InvoicesMaint.rdlc")
                    {
                        if (!string.IsNullOrEmpty(Report.Trim()))
                        {
                            rdsItems = new ReportDataSource("dtPESInvoiceItems", dtItems);
                        }
                    }
                }
                else if (sessval == "InvoiceException")
                {
                    if (Report == "PESMTC_InvoicesExceptions.rdlc")
                    {
                        if (!string.IsNullOrEmpty(Report.Trim()))
                        {
                            rdsItems = new ReportDataSource("dtPESInvoiceItems", dtItems);
                        }
                    }

                }

                else if (sessval == "Invoice-LNY")
                {
                    if (Report == "Invoice-LNY.rdlc")
                    {
                        if (!string.IsNullOrEmpty(Report.Trim()))
                        {
                            rdsItems = new ReportDataSource("dtPESInvoiceItems", dtItems);
                        }
                    }

                }
                e.DataSources.Add(rdsItems);
            }
            if (count_inv == dt.Rows.Count - 1)
            {
                ViewState["InvoicesSubReportResult"] = null;
            }
            count_inv++;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private DataTable LoadInvoiceDetails(DataTable _dt, int _idRef)
    {
        DataRow _dr = _dt.NewRow();
        _dr["Ref"] = _idRef;
        _dr["Acct"] = 0;
        _dr["Quan"] = 0;
        _dr["fDesc"] = string.Empty;
        _dr["Price"] = 0.00;
        _dr["Amount"] = 0.00;
        _dr["STax"] = 0.00;
        _dr["billcode"] = string.Empty;
        _dr["staxAmt"] = 0.00;
        _dr["balance"] = 0.00;
        _dr["amtpaid"] = 0.00;
        _dr["total"] = 0.00;
        _dt.Rows.Add(_dr);
        return _dt;
    }

    private byte[] ExportReportToPDF(string reportName)
    {
        Warning[] warnings;
        string[] streamids;
        string mimeType;
        string encoding;
        string filenameExtension;
        byte[] bytes = rvRecInvoices.LocalReport.Render(
            "PDF", null, out mimeType, out encoding, out filenameExtension,
             out streamids, out warnings);

        return bytes;
    }

    private byte[] ExportReportToPDF1(string reportName, ReportViewer reportviewer1)
    {
        Warning[] warnings;
        string[] streamids;
        string mimeType;
        string encoding;
        string filenameExtension;
        reportviewer1.ProcessingMode = ProcessingMode.Local;
        byte[] bytes = reportviewer1.LocalReport.Render(
            "PDF", null, out mimeType, out encoding, out filenameExtension,
             out streamids, out warnings);

        return bytes;
    }

    private void permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("AcctMgr");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("billingLink");
        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkInvoicesSmenu");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
    }

    private string GetFromEmailAddress()
    {
        string fromEmail = "";
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Username = Session["username"].ToString();
        try
        {
            fromEmail = objBL_User.getUserEmail(objPropUser);

            if (fromEmail == string.Empty)
            {
                System.Configuration.Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
                MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
                string username = mailSettings.Smtp.Network.UserName;
                fromEmail = username;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return fromEmail;
    }

    private void ShowGstRate()
    {
        // For canadian company show GST rate in Invoice template.
        objGeneral.ConnConfig = Session["config"].ToString();
        objGeneral.CustomName = "Country";
        DataSet dsCustom = objBL_General.getCustomFields(objGeneral);

        if (dsCustom.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(dsCustom.Tables[0].Rows[0]["Label"].ToString()) && dsCustom.Tables[0].Rows[0]["Label"].ToString().Equals("1"))
            {
                IsGst = true;
            }
        }
        ViewState["IsGst"] = IsGst;
    }

    #endregion
}