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

public partial class PrintInvoices : System.Web.UI.Page
{
    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    GeneralFunctions objGen = new GeneralFunctions();
    int count_inv = 0;

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

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
        //string url_path_current = HttpContext.Current.Request.Url.ToString();
        //if (url_path_current.StartsWith("https:") == true)
        //{
        //    HttpContext.Current.Response.Redirect("http" + url_path_current.Remove(0, 5), false);
        //} 

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
                        string redirect = "HTTPS://" + URL + "/PrintInvoice.aspx";
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
            //objProp_Contracts.InvoiceID = Convert.ToInt32(Request.QueryString["uid"]);
            /****Get from MS_Invoice tables the invoices masrked as pending from Mobile Service in case of TS database****/
            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }
            DataTable dtNew = new DataTable();
            int j = 0;

            int check = Int16.Parse(Request.QueryString["Check"].ToString());
            if (check == 0)
            {
                dtNew = (DataTable)Session["InvoiceReportDetails"];
                ds.Tables.Add(dtNew.Copy());
            }
            else
            {

                int _sInvID = Int32.Parse(Request.QueryString["uid"].ToString());
                int _eInvID = Int32.Parse(Request.QueryString["eid"].ToString());
                for (int i = _sInvID; i <= _eInvID; i++)
                {
                    objProp_Contracts.InvoiceID = Convert.ToInt32(i);
                    ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);

                    if (j > 0)
                    {
                        dtNew.Merge(ds.Tables[0], true);
                    }
                    else
                    {
                        dtNew = ds.Tables[0];
                    }
                    j++;
                }

            }
            ViewState["amount"] = ds.Tables[0].Rows[0]["balance"].ToString();
            string paid = ds.Tables[0].Rows[0]["paidcc"].ToString();
            string status = ds.Tables[0].Rows[0]["status"].ToString();
            //if (Status == "1" || Status == "5")
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
            if (check != 0)
            {
                txtSubject.Text = "Invoice " + Request.QueryString["uid"].ToString() + " - " + subject;
            }

            ViewState["RecurrInvoice"] = dtNew;             // this viewstate stores all invoices, show up on preview of RDLC report.
            ViewState["InvoicesSubReportResult"] = dtNew;   // this viewstate used to store filtered data of Invoices, We are filtering invoice on PrintOnly and Mail all functionalities.


            GenerateReport(rvRecInvoices, dtNew);
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
        //objProp_Contracts.InvoiceID = Convert.ToInt32(Request.QueryString["uid"]);
        /****Get from MS_Invoice tables the invoices masrked as pending from Mobile Service in case of TS database****/
        if (Session["MSM"].ToString() == "TS")
        {
            if (Session["type"].ToString() != "c")
                objProp_Contracts.isTS = 1;
        }

        string _unsuccesfullInvoices = hdnUnsuccessfulEmail.Value.Trim('|');
        if (!string.IsNullOrEmpty(_unsuccesfullInvoices))
        {
            #region Bind ReportViewer
            string[] _uInvoices = _unsuccesfullInvoices.Split('|');
            DataTable dtNew1 = new DataTable();
            int j = 0;
            for (int i = 0; i <= _uInvoices.Length - 1; i++)
            {
                objProp_Contracts.InvoiceID = Convert.ToInt32(i);
                ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);

                if (j > 0)
                {
                    dtNew1.Merge(ds.Tables[0], true);
                }
                else
                {
                    dtNew1 = ds.Tables[0];
                }
                j++;
            }

            //ViewState["amount"] = ds.Tables[0].Rows[0]["balance"].ToString();
            string paid = ds.Tables[0].Rows[0]["paidcc"].ToString();
            string status = ds.Tables[0].Rows[0]["status"].ToString();
            //if (Status == "1" || Status == "5")
            if (status == "0" && paid == "0")
                lnkPayment.Visible = true;
            else
                lnkPayment.Visible = false;


            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.Username = Session["username"].ToString();
            txtFrom.Text = objBL_User.getUserEmail(objPropUser);

            ViewState["RecurrInvoice"] = dtNew1;             // added by dev 26 th feb, 16 

            GenerateReport(_reporviewer2, dtNew1);
            #endregion

            Response.ContentType = "Application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; Invoice-" + "i.ToString()" + ".pdf");

            byte[] getPDF = ExportReportToPDF1("", _reporviewer2);
            Response.Write(getPDF);
            MemoryStream ms = new MemoryStream(getPDF);
            ms.WriteTo(Response.OutputStream);
            ms.Close();
            Response.Flush();
        }

        //var Response = new HttpContext(Response);
        //Response.Close();

        // Response.End();
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
                mail.Title = txtSubject.Text.Trim(); //"Invoice " + Request.QueryString["uid"].ToString() + " - " + ViewState["subject"].ToString();
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace("\n", "<BR/>");
                }
                else
                {
                    mail.Text = ViewState["company"].ToString().Replace(Environment.NewLine, "<BR/>");
                }
                //mail.AttachmentFiles.Add(ExportReportToPDF("Report_" + objGen.generateRandomString(10) + ".pdf"));
                mail.FileName = "Invoice-" + Request.QueryString["uid"].ToString() + ".pdf";
                mail.attachmentBytes = ExportReportToPDF("");

                mail.DeleteFilesAfterSend = true;
                mail.RequireAutentication = false;
                // ES-33:Task#2: Added
                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                //if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("Adams"))
                //    mail.SendOld();
                //else
                    mail.Send();
                this.programmaticModalPopup.Hide();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            catch (Exception ex)
            {
                //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
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
        //Response.Redirect("https://" + URL + "fdggpay.aspx");
        Response.Redirect("https://" + URL + paymentscreen);
    }
    protected void lnkMailReport_Click(object sender, EventArgs e)      // added by dev 26th feb, 16 mail invoice report 
    {
        try
        {
            string _todayDate = DateTime.Now.Date.ToString("MM-dd-yyyy");
            DataTable dtInv = (DataTable)ViewState["RecurrInvoice"];
            DataTable dtC = (DataTable)ViewState["RecurCompany"];
            DataTable dt = dtInv.AsEnumerable()                 // Group by location to send invoices
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
                        GenerateReport(rvInvoices, dtRecur);
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
                        //mail.To = _toEmaillst;
                        foreach (var address in _toEmail.Split(','))
                        {
                            if (!string.IsNullOrEmpty(address))
                                mail.To.Add(new MailAddress(address.Trim(), "").Address);
                        }
                        //mail.Cc = _ccEmaillst;
                        foreach (var address in _ccEmail.Split(','))
                        {
                            if(!string.IsNullOrEmpty(address))
                                mail.Cc.Add(new MailAddress(address.Trim(), "").Address);
                        }
                        mail.Title = "Invoices - " + _dsCon.Tables[0].Rows[0]["ID"].ToString() + " " + _dsCon.Tables[0].Rows[0]["Tag"].ToString();

                        //mail.Text = ViewState["CompanyAddress"].ToString().Replace(Environment.NewLine, "<BR/>");
                        if (txtBody.Text.Trim() != string.Empty)
                        {
                            mail.Text = txtBody.Text.Replace("\n", "<BR/>");
                        }
                        else
                        {
                            mail.Text = ViewState["company"].ToString().Replace(Environment.NewLine, "<BR/>");
                        }
                        mail.attachmentBytes = ExportReportToPDF1("", rvInvoices);
                        mail.FileName = "Invoices_" + _todayDate+".pdf";

                        mail.DeleteFilesAfterSend = true;
                        mail.RequireAutentication = false;
                        // ES-33:Task#2: Added
                        WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                        //if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("Adams"))
                        //    mail.SendOld();
                        //else
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
            if(isUnscuss)
            {
                strLoc = lstLoc.Aggregate((x, y) => x + ", " + y);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "unsuccessMesg('"+strLoc+"');", true);
                //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "dispWarningMesg('" + strLoc + "');", true);
            }

            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "dispWarningMesg(" + valuepass +");", true);
        }
        catch (Exception ex)
        {
            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkPrint_Click(object sender, EventArgs e)
    {
        //Print invoices which are not emailed to locations.
        try
        {
            string filename = "PrintOnly-" + Request.QueryString["m"].ToString() + Request.QueryString["y"].ToString();
            DataTable dtFilter = new DataTable();

            DataTable dtInv = (DataTable)ViewState["RecurrInvoice"];
            DataTable dtC = (DataTable)ViewState["RecurCompany"];
            var rows = dtInv.AsEnumerable()
                .Where(x => x.Field<int>("IsExistsEmail").Equals(0));

            if (rows.Any())
                dtFilter = rows.CopyToDataTable();
            //DataTable dtFilter = dtInv.AsEnumerable()
            //    .Where(x => x.Field<int>("IsExistsEmail").Equals('0')).CopyToDataTable();
            ViewState["InvoicesSubReportResult"] = dtFilter;
            count_inv = 0;
            if(dtFilter != null)
            {
                if (dtFilter.Rows.Count > 0)
                {
                    ReportViewer rvInvoices = new ReportViewer();
                    GenerateReport(rvInvoices, dtFilter);
                    byte[] getPDF = ExportReportToPDF1("", rvInvoices);
                    //Response.Write(getPDF);
                    //MemoryStream ms = new MemoryStream(getPDF);
                    //ms.WriteTo(Response.OutputStream);
                    //ms.Close();
                    //Response.Flush();

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
                    ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No Invoice(s) found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No Invoice(s) found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
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
    private void GenerateReport(ReportViewer rv, DataTable dtInvoice)
    {
        DataTable dtCompany = new DataTable();
        if (ViewState["RecurCompany"] == null)
        {
            DataSet dsCompany = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            //objPropUser.DBName = Session["dbname"].ToString();
            //objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
            //DataSet dsloc = new DataSet();
            //dsloc = objBL_User.getLocationByID(objPropUser);

            //if (Session["MSM"].ToString() != "TS")
            //{
            dsCompany = objBL_User.getControl(objPropUser);
            //}
            //else
            //{
            //    objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
            //    dsCompany = objBL_User.getControlBranch(objPropUser);
            //}
            ViewState["RecurCompany"] = dsCompany.Tables[0];
            dtCompany = dsCompany.Tables[0];
        }
        else
        {
            dtCompany = (DataTable)ViewState["RecurCompany"];
        }

        foreach (DataRow dr in dtInvoice.Rows)
        {
            //billTo = Regex.Replace(billTo, @"( |\r?\n)\1+", "$1");  // to remove first new line.
            string billTo = Regex.Replace(dr["Billto"].ToString(), @"\t|\n|\r", "");          // to remove all new lines.
            billTo = Regex.Replace(billTo, @"^,+|,+$|,+(,\w)", "$1");
            billTo = billTo.Split(new[] { ',' }, 2).First() + ",\n" + billTo.Split(new[] { ',' }, 2).Last();
            dr["Billto"] = billTo;
        }

        rv.LocalReport.DataSources.Clear();  //added by dev 15th march, 16

        rv.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemSubReportProcessing);

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


        if (Report == "Madden_Invoices.rdlc" || Report == "InvoicesAdams.rdlc" || Report == string.Empty || Report == "InvoicesInFrench.rdlc" || Report == "ReportInvoicePME.rdlc")
        {
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                rv.LocalReport.DataSources.Add(new ReportDataSource("Invoice_dtInvoice", dtInvoice));
            }
            else
            {
                rv.LocalReport.DataSources.Add(new ReportDataSource("Invoice_dtInvoice", dtInvoice));
            }
        }
        else if (Report == "PESMTC_InvoicesMaint.rdlc" || Report == "PESMTC_InvoicesExceptions.rdlc" || Report == "Invoice-LNY.rdlc")
        {
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                rv.LocalReport.DataSources.Add(new ReportDataSource("Invoice_PESdtInvoice", dtInvoice));
            }
        }

        rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dtCompany));

        string reportPath = string.Empty;

        if (sessval == "Invoice")
        {
            reportPath = "Reports/Invoices.rdlc";

            if (Report == "Madden_Invoices.rdlc" || Report == "InvoicesInFrench.rdlc")
            {
                if (!string.IsNullOrEmpty(Report.Trim()))
                {
                    reportPath = "Reports/" + Report.Trim();
                }
            }
        }
        else if (sessval == "InvoiceMaint")
        {
            if (Report == "PESMTC_InvoicesMaint.rdlc")
            {
                if (!string.IsNullOrEmpty(Report.Trim()))
                {
                    reportPath = "Reports/" + Report.Trim();
                }
            }
        }
        else if (sessval == "InvoiceException")
        {
            if (Report == "PESMTC_InvoicesExceptions.rdlc")
            {
                if (!string.IsNullOrEmpty(Report.Trim()))
                {
                    reportPath = "Reports/" + Report.Trim();
                }
            }
        }

        else if (sessval == "Invoice-LNY")
        {
            if (Report == "Invoice-LNY.rdlc")
            {
                if (!string.IsNullOrEmpty(Report.Trim()))
                {
                    reportPath = "Reports/" + Report.Trim();
                }
            }
        }

        if (Report == "InvoicesAdams.rdlc")
        {
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                reportPath = "Reports/" + Report.Trim();
            }
        }
        rv.LocalReport.ReportPath = reportPath;

        rv.LocalReport.EnableExternalImages = true;
        List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
        string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
        if (Report == "InvoicesInFrench.rdlc" || Report == "InvoicesAdams.rdlc" || Report == "" )
        {
            param1.Add(new ReportParameter("IsGstTax", ViewState["IsGst"].ToString()));
        }
        rv.LocalReport.SetParameters(param1);

        rv.LocalReport.Refresh();
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
                dtItems = LoadInvoiceDetails(ds.Tables[0], objProp_Contracts.InvoiceID);    // if none line item exists of invoice
            }
            else
                dtItems = ds.Tables[0];

            ReportDataSource rdsItems = null;
            if (dtItems.Rows.Count > 0)
            {
                //string Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesReport"].Trim();
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

        //string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF", reportName);
        //using (var fs = new FileStream(filename, FileMode.Create))
        //{
        //    fs.Write(bytes, 0, bytes.Length);
        //    fs.Close();
        //}

        //return filename;
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

        //string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF", reportName);
        //using (var fs = new FileStream(filename, FileMode.Create))
        //{
        //    fs.Write(bytes, 0, bytes.Length);
        //    fs.Close();
        //}

        //return filename;
    }
    private void permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("AcctMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("billingLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkInvoicesSmenu");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
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
                ////txtFrom.ReadOnly = true;
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

    // ES-33:Task#2: new functions
    //private void GetMailConfigurationFromUser(Mail mail)
    //{
    //    if (mail == null)
    //        mail = new Mail();
    //    mail.RequireAutentication = false;
    //    General _objGeneral = new General();
    //    _objGeneral.ConnConfig = Session["config"].ToString();
    //    _objGeneral.userid = Convert.ToInt32(Session["UserID"]);
    //    BL_General _objBL_General = new BL_General();
    //    DataSet dsEmailacc = _objBL_General.GetEmailAcc(_objGeneral);
    //    if (dsEmailacc.Tables[0].Rows.Count > 0)
    //    {
    //        mail.RequireAutentication = true;
    //        mail.Username = dsEmailacc.Tables[0].Rows[0]["OutUsername"].ToString();
    //        mail.Password = dsEmailacc.Tables[0].Rows[0]["OutPassword"].ToString();
    //        mail.SMTPHost = dsEmailacc.Tables[0].Rows[0]["OutServer"].ToString();
    //        mail.SMTPPort = Convert.ToInt32(dsEmailacc.Tables[0].Rows[0]["OutPort"].ToString());
    //        mail.From = dsEmailacc.Tables[0].Rows[0]["OutUsername"].ToString();
    //    }
    //}
    #endregion

    #region Comments
    //#region PrintInvoice
    //private DataTable GetInvoiceItems(int _refId)
    //{
    //    DataTable _dtItem = new DataTable();
    //    try
    //    {
    //        objProp_Contracts.InvoiceID = _refId;
    //        objProp_Contracts.ConnConfig = Session["config"].ToString();
    //        DataSet _dsItemDetails = objBL_Contracts.GetInvoiceItemByRef(objProp_Contracts);
    //        if (_dsItemDetails.Tables[0].Rows.Count < 1)
    //        {
    //            _dtItem = LoadInvoiceDetails(_dsItemDetails.Tables[0], _refId);
    //        }
    //        else
    //            _dtItem = _dsItemDetails.Tables[0];
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }

    //    return _dtItem;
    //}
    //#endregion
    //protected void LinkButton1_Click(object sender, EventArgs e)
    //{
    //    string _locname = "";
    //    int sid = Convert.ToInt32(Request.QueryString["uid"]);
    //    int eid = Convert.ToInt32(Request.QueryString["eid"]);
    //    string _toEmail = "";
    //    string _ccEmail = "";
    //    int index = 0;
    //    for (int i = sid; i <= eid; i++)
    //    {
    //        objProp_Contracts.ConnConfig = Session["config"].ToString();
    //        objProp_Contracts.Ref = i;
    //        DataSet _dsCon = objBL_Contracts.GetEmailDetailByLoc(objProp_Contracts);
    //        if (_dsCon.Tables[0].Rows.Count > 0)
    //        {
    //            if (!string.IsNullOrEmpty(_dsCon.Tables[0].Rows[0]["custom12"].ToString()))
    //            {
    //                _toEmail = _dsCon.Tables[0].Rows[0]["custom12"].ToString();

    //                if (!string.IsNullOrEmpty(_dsCon.Tables[0].Rows[0]["custom13"].ToString()))
    //                {
    //                    _ccEmail = _dsCon.Tables[0].Rows[0]["custom13"].ToString();
    //                }

    //                List<string> _toEmaillst = new List<string>();
    //                _toEmaillst.Add(_toEmail);
    //                List<string> _ccEmaillst = new List<string>();
    //                _ccEmaillst.Add(_ccEmail);

    //                Mail mail = new Mail();
    //                mail.From = txtFrom.Text.Trim();
    //                mail.To = _toEmaillst;
    //                if (txtCC.Text.Trim() != string.Empty)
    //                {
    //                    mail.Cc = _ccEmaillst;
    //                }
    //                mail.Title = txtSubject.Text.Trim(); //"Invoice " + Request.QueryString["uid"].ToString() + " - " + ViewState["subject"].ToString();
    //                if (txtBody.Text.Trim() != string.Empty)
    //                {
    //                    mail.Text = txtBody.Text.Replace("\n", "<BR/>");
    //                }
    //                else
    //                {
    //                    mail.Text = ViewState["company"].ToString().Replace(Environment.NewLine, "<BR/>");
    //                }
    //                //mail.AttachmentFiles.Add(ExportReportToPDF("Report_" + objGen.generateRandomString(10) + ".pdf"));
    //                mail.FileName = "Invoice-" + i.ToString() + ".pdf";
    //                mail.attachmentBytes = ExportReportToPDF("");

    //                mail.DeleteFilesAfterSend = true;
    //                mail.RequireAutentication = false;

    //                mail.Send();
    //            }
    //            else
    //            {
    //                if (!string.IsNullOrEmpty(_dsCon.Tables[0].Rows[0]["Tag"].ToString()))
    //                {
    //                    if (index > 0)
    //                        _locname += ",";
    //                    _locname += _dsCon.Tables[0].Rows[0]["Tag"].ToString();
    //                    index++;
    //                }
    //            }
    //        }
    //    }

    //    string valuepass = string.Concat(_locname, "|", _toEmail);

    //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "dispWarningMesg('" + _locname + "');", true);
    //    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "dispWarningMesg(" + valuepass +");", true);
    //}
    //protected void lnkMailAll_Click(object sender, EventArgs e) // added by dev 26 th feb, 16 Mail all 
    //{
    //    try
    //    {
    //        DataTable dtNew = (DataTable)ViewState["RecurrInvoice"];
    //        DataTable dt = dtNew.AsEnumerable()                 // Group by location to send invoices
    //                .GroupBy(r => r.Field<int>("Loc"))
    //                .Select(g => g.First())
    //                .CopyToDataTable();

    //        string _fromEmail = ViewState["EmailFrom"].ToString();
    //        if (string.IsNullOrEmpty(_fromEmail))
    //        {
    //            _fromEmail = GetFromEmailAddress();
    //        }
    //        foreach (DataRow _dr in dt.Rows)
    //        {
    //            int _ref = Convert.ToInt32(_dr["Ref"]);
    //            objProp_Contracts.ConnConfig = Session["config"].ToString();
    //            objProp_Contracts.Ref = _ref;
    //            DataSet _dsCon = objBL_Contracts.GetEmailDetailByLoc(objProp_Contracts);
    //            if (_dsCon.Tables[0].Rows.Count > 0)
    //            {
    //                string _toEmail = "";
    //                string _ccEmail = "";
    //                if (!string.IsNullOrEmpty(_dsCon.Tables[0].Rows[0]["custom12"].ToString()))
    //                {
    //                    _toEmail = _dsCon.Tables[0].Rows[0]["custom12"].ToString();

    //                    if (!string.IsNullOrEmpty(_dsCon.Tables[0].Rows[0]["custom13"].ToString()))
    //                    {
    //                        _ccEmail = _dsCon.Tables[0].Rows[0]["custom13"].ToString();
    //                    }

    //                    List<string> _toEmaillst = new List<string>();
    //                    _toEmaillst.Add(_toEmail);
    //                    List<string> _ccEmaillst = new List<string>();
    //                    _ccEmaillst.Add(_ccEmail);


    //                    Mail mail = new Mail();
    //                    mail.From = _fromEmail;
    //                    mail.To = _toEmaillst;
    //                    mail.Cc = _ccEmaillst;

    //                    mail.Title = "Invoices - " +_dsCon.Tables[0].Rows[0]["ID"].ToString() +" "+ _dsCon.Tables[0].Rows[0]["Tag"].ToString();

    //                    //mail.Text = ViewState["CompanyAddress"].ToString().Replace(Environment.NewLine, "<BR/>");
    //                    if (txtBody.Text.Trim() != string.Empty)
    //                    {
    //                        mail.Text = txtBody.Text.Replace("\n", "<BR/>");
    //                    }
    //                    else
    //                    {
    //                        mail.Text = ViewState["company"].ToString().Replace(Environment.NewLine, "<BR/>");
    //                    }
    //                    mail.attachmentBytes = ExportReportToPDF("");
    //                    mail.FileName = "Invoices.pdf";

    //                    mail.DeleteFilesAfterSend = true;
    //                    mail.RequireAutentication = false;

    //                    mail.Send();
    //                }
    //            }
    //        }
    //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Mail sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
    //    }
    //    catch(Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}
    //public void ItemFilterSubReportProcessing(object sender, SubreportProcessingEventArgs e)
    //{
    //    try
    //    {
    //        DataTable dt = (DataTable)ViewState["FilterInvoice"];
    //        DataTable dtCompany = (DataTable)ViewState["RecurCompany"];
    //        int refId = Convert.ToInt32(dt.Rows[count_inv]["Ref"]);

    //        DataTable _dtInvItems = GetInvoiceItems(refId);

    //        if (_dtInvItems.Rows.Count > 0)
    //        {
    //            ReportDataSource rdsItems = new ReportDataSource("dtInvoiceItems", _dtInvItems);

    //            e.DataSources.Add(rdsItems);
    //        }
    //        if (count_inv == dt.Rows.Count - 1)
    //        {
    //            //ViewState["InvoiceReport"] = null;
    //            //ViewState["CompanyReport"] = null;
    //        }
    //        count_inv++;
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}
    //public void ItemDetailsSubReportProcessing(object sender, SubreportProcessingEventArgs e)
    //{
    //    try
    //    {
    //        DataTable dt = (DataTable)ViewState["RecurrInvoice"];
    //        DataTable dtCompany = (DataTable)ViewState["RecurCompany"];
    //        int refId = Convert.ToInt32(dt.Rows[count_inv]["Ref"]);

    //        DataTable _dtInvItems = GetInvoiceItems(refId);

    //        if (_dtInvItems.Rows.Count > 0)
    //        {
    //            ReportDataSource rdsItems = new ReportDataSource("dtInvoiceItems", _dtInvItems);

    //            e.DataSources.Add(rdsItems);
    //        }
    //        if (count_inv == dt.Rows.Count - 1)
    //        {
    //            //ViewState["InvoiceReport"] = null;
    //            //ViewState["CompanyReport"] = null;
    //        }
    //        count_inv++;
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}
    //public void ItemDetailsMailSubReportProcessing(object sender, SubreportProcessingEventArgs e)
    //{
    //    try
    //    {
    //        DataTable dt = (DataTable)ViewState["InvoicesReport"];
    //        DataTable dtCompany = (DataTable)ViewState["RecurCompany"];
    //        int refId = Convert.ToInt32(dt.Rows[count_inv]["Ref"]);

    //        DataTable _dtInvItems = GetInvoiceItems(refId);

    //        if (_dtInvItems.Rows.Count > 0)
    //        {
    //            ReportDataSource rdsItems = new ReportDataSource("dtInvoiceItems", _dtInvItems);

    //            e.DataSources.Add(rdsItems);
    //        }
    //        if (count_inv == dt.Rows.Count - 1)
    //        {
    //            //ViewState["InvoiceReport"] = null;
    //            //ViewState["CompanyReport"] = null;
    //        }
    //        count_inv++;
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}
    #endregion
}