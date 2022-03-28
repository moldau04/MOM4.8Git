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
using System.Configuration;

public partial class PrintInvoice : System.Web.UI.Page
{
    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    BusinessEntity.User objPropUser = new BusinessEntity.User();
    BL_User objBL_User = new BL_User();

    GeneralFunctions objGen = new GeneralFunctions();
    BL_General objBL_General = new BL_General();
    General objGeneral = new General();


    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

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
            if (Session["type"].ToString() == "c")
            {
                lnkPayment.Visible = true;
            }

            if (Request.QueryString["cl"] != null)
            {
                lnkCancelContact.Visible = false;
            }

            DataSet ds = new DataSet();

            DataTable dtEquip = new DataTable();
            DataTable dtTicket = new DataTable();
            DataTable dtTicketPO = new DataTable();
            DataTable dtTicketI = new DataTable();
            DataTable dtDetails = new DataTable();

            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.InvoiceID = Convert.ToInt32(Request.QueryString["uid"]);
            /****Get from MS_Invoice tables the invoices masrked as pending from Mobile Service in case of TS database****/
            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }
            /***/
            string sessval = (string)Session["InvoiceName"];
            string Report = string.Empty;
            string ReportCheck = string.Empty;
            if (sessval == "InvoiceMaint")
            {
                ReportCheck = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesMaintReport"].Trim();
            }

            else if (sessval == "InvoiceException")
            {
                ReportCheck = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesExceptionReport"].Trim();
            }
            else if (sessval == "Invoice")
            {
                ReportCheck = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceReport"].Trim();
            }
            else if (sessval == "InvoiceLNY")
            {
                ReportCheck = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceLNY"].Trim();
            }
            else if (sessval == "InvoiceTicketLNY")
            {
                ReportCheck = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceLNYWithTicket"].Trim();
            }
            if (ReportCheck == "PESMTC_InvoicesMaint.rdlc" || ReportCheck == "PESMTC_InvoicesExceptions.rdlc" || ReportCheck == "Invoice-LNY.rdlc" || ReportCheck == "Invoice_Ticket-LNY.rdlc")
            {

                ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);
                Session["InvoiceSubreport"] = ds.Tables[0];
            }
            else
            {
                ds = objBL_Contracts.GetInvoicesByID(objProp_Contracts);
            }

            if (ReportCheck == "Invoice_Ticket-LNY.rdlc")
            {
                int i = 0;
                DataSet TicketID = objBL_Contracts.GetTicketID(objProp_Contracts);
                foreach (DataRow item in TicketID.Tables[0].Rows)
                {
                    objMapData.ConnConfig = Session["config"].ToString();
                    objMapData.TicketID = (int)item[0];
                    DataSet dsEquip = objBL_MapData.getElevByTicket(objMapData);
                    DataSet dsTicket = objBL_MapData.GetTicketByID(objMapData);
                    objPropUser.ConnConfig = Session["config"].ToString();
                    objPropUser.EquipID = 0;
                    objPropUser.SearchBy = "rd.ticketID";
                    objPropUser.SearchValue = item[0].ToString();
                    DataSet dsDetails = objBL_User.getequipREPDetails(objPropUser);
                    if (i == 0)
                    {
                        dtEquip = dsEquip.Tables[0];
                        dtTicket = dsTicket.Tables[0];
                        dtTicketPO = dsTicket.Tables[1];
                        dtTicketI = dsTicket.Tables[2];
                        dtDetails = dsDetails.Tables[0];
                        i++;
                    }
                    else
                    {
                        dtEquip.Rows.Add(dsEquip.Tables[0].Rows[0].ItemArray);
                        dtTicket.Rows.Add(dsTicket.Tables[0].Rows[0].ItemArray);
                        //dtTicketPO.Rows.Add(dsTicket.Tables[1].Rows[0].ItemArray);
                        //dtTicketI.Rows.Add(dsTicket.Tables[2].Rows[0].ItemArray);
                        //dtDetails.Rows.Add(dsDetails.Tables[0].Rows[0].ItemArray);
                        i++;
                    }
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
            if (dsC.Tables[0].Rows.Count > 0)
            {
                if (Session["MSM"].ToString() != "TS")
                {
                    if (txtFrom.Text.Trim() == string.Empty)
                    {
                        txtFrom.Text = dsC.Tables[0].Rows[0]["Email"].ToString();
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
            bool IsGst = false;
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
            string subject = string.Empty;
            DataSet dccust = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.CustomerID = Convert.ToInt32(Request.QueryString["uid"]);
            dccust = objBL_User.getCustomerForReport(objPropUser);

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
            //if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c" && Session["MSM"].ToString() != "TS")
            //{
            //    objPropUser.ConnConfig = Session["config"].ToString();
            //    objPropUser.Username = Session["username"].ToString();
            //    string strTo = objBL_User.getUserEmail(objPropUser);
            //    if (txtTo.Text.Trim() == string.Empty && strTo != string.Empty)
            //    {
            //        txtTo.Text = strTo;
            //    }
            //    else if (txtTo.Text.Trim() != string.Empty && strTo != string.Empty)
            //    {
            //        txtTo.Text += "," + strTo;
            //    }
            //}
            ViewState["subject"] = subject;
            txtSubject.Text = "Invoice " + Request.QueryString["uid"].ToString() + " - " + subject;
            //string billTo = ds.Tables[0].Rows[0]["Billto"].ToString();
            //billTo = Regex.Replace(billTo, @"( |\r?\n)\1+", "$1");
            //ds.Tables[0].Rows[0]["Billto"] = billTo;

            //string billTo = Regex.Replace(ds.Tables[0].Rows[0]["Billto"].ToString(), @"\t|\n|\r", "");          // to remove all new lines.
            //billTo = Regex.Replace(billTo, @"^,+|,+$|,+(,\w)", "$1");
            //billTo = billTo.Split(new[] { ',' }, 2).First() + ",\n" + billTo.Split(new[] { ',' }, 2).Last();
            //ds.Tables[0].Rows[0]["Billto"] = billTo;

            ReportViewer1.LocalReport.SubreportProcessing += LocalReport_SubreportProcessing;

            if (ReportCheck == "PESMTC_InvoicesMaint.rdlc" || ReportCheck == "PESMTC_InvoicesExceptions.rdlc" || ReportCheck == "Invoice-LNY.rdlc" || ReportCheck == "Invoice_Ticket-LNY.rdlc")
            {
                //ReportViewer1.LocalReport.SubreportProcessing += LocalReport_SubreportProcessing;
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Invoice_PESdtInvoice", ds.Tables[0]));
            }
            else
            {
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Invoice_dtInvoice", ds.Tables[0]));
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Invoice_dtInvoiceDetails", ds.Tables[1]));

            }
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));

            if (ReportCheck == "Invoice_Ticket-LNY.rdlc")
            {
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dtEquipDetails", dtEquip));
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicket", dtTicket));
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtMCP", dtDetails));
                //if (ds.Tables.Count > 1)
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtPOItem", dtTicketPO));
                //if (ds.Tables.Count > 2)
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicketI", dtTicketI));
            }

            string reportPath = "reports/"+ System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceReport"].Trim(); ;
            Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceReport"].Trim();
            if (string.IsNullOrEmpty(sessval))
                Session["InvoiceName"] = "Invoice";

            if (sessval == "InvoiceMaint")
            {
                Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesMaintReport"].Trim();
            }

            else if (sessval == "InvoiceException")
            {
                Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesExceptionReport"].Trim();
            }
            else if (sessval == "Invoice")
            {
                Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceReport"].Trim();
            }
            else if (sessval == "InvoiceLNY")
            {
                Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceLNY"].Trim();
            }

            else if (sessval == "InvoiceTicketLNY")
            {
                Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceLNYWithTicket"].Trim();
            }
            else if (sessval == "AdamMaintenance")
            {
                Report = "ReportInvoiceAdamMaintenance.rdlc";
            }
            else if (sessval == "AdamBilling")
            {
                Report = "ReportInvoiceAdamBilling.rdlc";
            }
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                reportPath = "Reports/" + Report.Trim();
            }
            ReportViewer1.LocalReport.ReportPath = reportPath;

            ReportViewer1.LocalReport.EnableExternalImages = true;
            List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
            string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
            param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
            if (Report == "" || Report == "ReportInvoicePME.rdlc" || Report == "ReportInvoiceAdamMaintenance.rdlc" || Report=="InvoicesAdams.rdlc")
            {
                param1.Add(new ReportParameter("IsGstTax", IsGst.ToString()));
            }

            ReportViewer1.LocalReport.SetParameters(param1);
            ReportViewer1.LocalReport.Refresh();
        }
        if (Convert.ToInt16(Session["payment"]) != 1)
        {
            lnkPayment.Visible = false;
        }
        //permission();
    }

    void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
    {
        try
        {
            int count_inv = 0;
            DataSet dst = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);
            // DataTable dt = (DataTable)Session["InvoicesSubReport"];
            DataTable dt = dst.Tables[0];
            DataTable dtItems = new DataTable();

            objProp_Contracts.InvoiceID = Convert.ToInt32(dt.Rows[count_inv]["Ref"]);
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            DataSet ds = objBL_Contracts.GetInvoiceItemByRef(objProp_Contracts);
            dtItems = ds.Tables[0];

            string sessval = (string)Session["InvoiceName"];
            string Report = string.Empty;

            if (sessval == "InvoiceMaint")
            {
                Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesMaintReport"].Trim();
            }

            if (sessval == "InvoiceException")
            {
                Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesExceptionReport"].Trim();
            }

            if (sessval == "InvoiceLNY")
            {
                Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceLNY"].Trim();
            }

            if (sessval == "InvoiceTicketLNY")
            {
                Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceLNYWithTicket"].Trim();
            }

            DataTable dtEquip = new DataTable();
            if (Report == "Invoice_Ticket-LNY.rdlc")
            {
                int i = 0;
                DataSet TicketID = objBL_Contracts.GetTicketID(objProp_Contracts);
                foreach (DataRow item in TicketID.Tables[0].Rows)
                {
                    objMapData.ConnConfig = Session["config"].ToString();
                    objMapData.TicketID = (int)item[0];
                    DataSet dsEquip = objBL_MapData.getElevByTicketID(objMapData);
                    if (i == 0)
                    {
                        dtEquip = dsEquip.Tables[0];
                        i++;
                    }
                    else
                    {
                        dtEquip.Rows.Add(dsEquip.Tables[0].Rows[0].ItemArray);
                        i++;
                    }
                }
            }

            ReportDataSource rdsItems = null;

            if (dtItems.Rows.Count > 0)
            {
                //string Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesReport"].Trim();


                /*string sessval = (string)Session["InvoiceName"];
                string Report = string.Empty;

                if (sessval == "InvoiceMaint")
                {
                    Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesMaintReport"].Trim();
                }

                if (sessval == "InvoiceException")
                {
                    Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesExceptionReport"].Trim();
                }

                if (sessval == "InvoiceLNY")
                {
                    Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceLNY"].Trim();
                }

                if (sessval == "InvoiceTicketLNY")
                {
                    Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceLNYWithTicket"].Trim();
                }*/

                /*DataTable dtEquip = new DataTable();
                if (Report == "Invoice_Ticket-LNY.rdlc")
                {
                    int i = 0;
                    DataSet TicketID = objBL_Contracts.GetTicketID(objProp_Contracts);
                    foreach (DataRow item in TicketID.Tables[0].Rows)
                    {
                        objMapData.ConnConfig = Session["config"].ToString();
                        objMapData.TicketID = (int)item[0];
                        DataSet dsEquip = objBL_MapData.getElevByTicketID(objMapData);                       
                        if (i == 0)
                        {
                            dtEquip = dsEquip.Tables[0];                           
                            i++;
                        }
                        else
                        {
                            dtEquip.Rows.Add(dsEquip.Tables[0].Rows[0].ItemArray);                            
                            i++;
                        }
                    }
                }*/


                if (sessval == "Invoice")
                {
                    if (Report == "Madden_Invoices.rdlc" || Report == string.Empty)
                    {
                        if (!string.IsNullOrEmpty(Report.Trim()))
                        {
                            e.DataSources.Add(rdsItems = new ReportDataSource("dtInvoiceItems", dtItems));
                        }
                        else
                        {
                            e.DataSources.Add(rdsItems = new ReportDataSource("dtInvoiceItems", dtItems));
                        }
                    }
                }
                if (sessval == "InvoiceMaint")
                {
                    if (Report == "PESMTC_InvoicesMaint.rdlc")
                    {
                        if (!string.IsNullOrEmpty(Report.Trim()))
                        {
                            e.DataSources.Add(rdsItems = new ReportDataSource("dtPESInvoiceItems", dtItems));
                            //rdsItems = new ReportDataSource("dtPESInvoiceItems", dtItems);
                        }
                    }
                }
                if (sessval == "InvoiceLNY")
                {
                    if (Report == "Invoice-LNY.rdlc")
                    {
                        if (!string.IsNullOrEmpty(Report.Trim()))
                        {
                            e.DataSources.Add(rdsItems = new ReportDataSource("dtPESInvoiceItems", dtItems));
                        }
                    }
                }

                if (sessval == "InvoiceTicketLNY")
                {
                    if (Report == "Invoice_Ticket-LNY.rdlc")
                    {
                        if (!string.IsNullOrEmpty(Report.Trim()))
                        {
                            e.DataSources.Add(rdsItems = new ReportDataSource("dtPESInvoiceItems", dtItems));
                            e.DataSources.Add(rdsItems = new ReportDataSource("dtEquipDetailsID", dtEquip));
                        }
                    }
                }

                else if (sessval == "InvoiceException")
                {
                    if (Report == "PESMTC_InvoicesExceptions.rdlc")
                    {
                        if (!string.IsNullOrEmpty(Report.Trim()))
                        {
                            e.DataSources.Add(rdsItems = new ReportDataSource("dtPESInvoiceItems", dtItems));
                        }
                    }

                }
                //e.DataSources.Add(rdsItems);
            }
            //if (count_inv == dt.Rows.Count - 1)
            //{
            //    ViewState["InvoicesSubReport"] = null;
            //}
            //count_inv++;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
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

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        this.programmaticModalPopup.Show();
        //txtTo.Text = string.Empty;
        //txtCC.Text = string.Empty;
        //ExportReportToPDF("Report_" + generateRandomString(10) + ".pdf");
    }

    private byte[] ExportReportToPDF(string reportName)
    {
        Warning[] warnings;
        string[] streamids;
        string mimeType;
        string encoding;
        string filenameExtension;
        byte[] bytes = ReportViewer1.LocalReport.Render(
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
                //if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("Adams"))
                //    mail.SendOld();
                //else
                    mail.Send();
                this.programmaticModalPopup.Hide();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Mail sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
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

}
