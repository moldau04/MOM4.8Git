using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
//using Microsoft.SqlServer.Management.Smo;
using BusinessLayer;
using BusinessEntity;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using System.Globalization;
using Telerik.Web.UI;
using Stimulsoft.Report;
using System.IO;
using Stimulsoft.Report.Web;
using iTextSharp.text.pdf;
using System.Configuration;
using Microsoft.Reporting.WebForms;
using BusinessLayer.Billing;
using System.Web;

public partial class AddInvoice : System.Web.UI.Page
{
    User objPropUser = new User();

    BL_User objBL_User = new BL_User();

    BL_Report bL_Report = new BL_Report();

    int count_inv = 0;

    BL_Contracts objBL_Contracts = new BL_Contracts();

    Contracts objProp_Contracts = new Contracts();

    BL_MapData objBL_MapData = new BL_MapData();

    MapData objMapData = new MapData();

    BL_Customer objBL_Customer = new BL_Customer();

    Customer objCustomer = new Customer();

    BL_General objBL_General = new BL_General();

    General objGenerals = new General();

    Owner _objOwner = new Owner();

    bool success;

    protected DataTable dtBillingCodeData = new DataTable();

    protected DataTable dtProjectCodeData = new DataTable();

    GeneralFunctions objGeneral = new GeneralFunctions();

    Inv _objInv = new Inv();

    Transaction _objTrans = new Transaction();

    Journal _objJournal = new Journal();

    BL_JournalEntry _objBL_Journal = new BL_JournalEntry();

    BusinessEntity.Invoices _objInvoices = new BusinessEntity.Invoices();

    BL_Invoice objBL_Invoice = new BL_Invoice();

    Chart _objChart = new Chart();

    BL_Chart _objBL_Chart = new BL_Chart();

    JobT objJob = new JobT();

    BL_Job objBL_Job = new BL_Job();

    BL_Inventory DL_INV = new BL_Inventory();

    int _batch = 0;

    double taxable = 0;


    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        //if (Request.QueryString["o"] != null)
        //{
        //    Page.MasterPageFile = "popup.master";
        //}
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        ViewState["isCanada"] = isCanadaCompany();

        var reportFormat = ConfigurationManager.AppSettings["InvoiceReportFormat"].ToString();
        if (reportFormat.ToUpper().Equals("RDLC"))
            lnkPDF.Visible = false;
        divSuccess.Visible = false;
        divProjectClose.Visible = false;



        if (!IsPostBack)
        {

            hdnISInventoryTrackingON.Value = DL_INV.ISINVENTORYTRACKINGISON(Session["config"].ToString()) == true ? "1" : "0";

            ViewState["othertickets"] = "0";
            //FillBillCodes(0);
            imgVoid.Visible = false;
            imgPaid.Visible = false;
            divSuccess.Visible = false;
            divProjectClose.Visible = false;
            //ddlStatus.Enabled = true;
            FillDepartment();
            FillWorker(0);
            FillSaleperson();
            FillTerms();
            LoadData();
            GetPeriodDetails(Convert.ToDateTime(txtInvoiceDate.Text));
            getControl();
            SetPaidBtn();
            SetDefaultSalesPerson();
            SetupNonCanadaGrid();
            //Bind grid to empty dataset 
            if (Request.QueryString["uid"] == null)
            {

                Page.Title = "Add Invoice || MOM";
                lnkReceiptPayment.Visible = false;
            }
            else
            {
                Page.Title = "Edit Invoice || MOM";
                liLogs.Style["display"] = "inline-block";
                liEmailLogs.Style["display"] = "inline-block";
                liHistoryPayment.Style["display"] = "inline-block";
                tbLogs.Style["display"] = "block";
                tbEmailLogs.Style["display"] = "block";
                tblPayment.Style["display"] = "block";
                ViewState["ResetGrid"] = 1;
            }

            HighlightSideMenu("acctMgr", "lnkInvoicesSMenu", "billMgrSub");



            Permission();

            CompanyPermission();

            if (!string.IsNullOrEmpty(hdnFocus.Value))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "keyFocus", "document.getElementById('" + hdnFocus.Value + "').focus();", true);
            }

            string Report1 = string.Empty;
            string Report2 = string.Empty;
            string Report3 = string.Empty;
            string Report4 = string.Empty;

            Report1 = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesMaintReport"].Trim();
            Report2 = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesExceptionReport"].Trim();
            Report3 = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceLNY"].Trim();
            Report4 = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceLNYWithTicket"].Trim();

            if (Report1 == string.Empty || Report2 == string.Empty)
            {
                lnk_InvoiceMaint.Visible = false;
                lnk_InvoiceException.Visible = false;
            }
            if (Report3 == string.Empty || Report4 == string.Empty)
            {
                lnk_InvoiceLNY.Visible = false;
                lnkInvTkt.Visible = false;
            }
            if (Session["dbname"].ToString() == "adams")
            {
                //lnk_InvoiceAdamBilling.Visible = true;
                lnk_InvoiceAdamMaintenance.Visible = true;

            }
            else
            {
                //lnk_InvoiceAdamBilling.Visible = false;
                lnk_InvoiceAdamMaintenance.Visible = false;
            }

            //Set Hyperlink  For Project


            if (hdnProjectId.Value != "0" && hdnProjectId.Value != "")
            {
                lnkProjectID.NavigateUrl = "addProject.aspx?uid=" + hdnProjectId.Value; lnkProjectID.Visible = true;
            }
            else
            {
                lnkProjectID.NavigateUrl = "javascript:void(0);"; //lnkProjectID.Visible = false;
                iProIcon.Attributes.Add("style", "color:#5815c02b !important;");
            }
            if (Session["UpdateInvoiceSuccess"] != null && Convert.ToBoolean(Session["UpdateInvoiceSuccess"]) == true)
            {
                Session["UpdateInvoiceSuccess"] = null;
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Invoice Updated Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
            if (Request.QueryString["uid"] == null && Session["AddInvoiceSuccess"] != null && Session["AddInvoiceSuccess"].ToString() != "")
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: '" + Session["AddInvoiceSuccess"] + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                Session["AddInvoiceSuccess"] = null;

            }

        }
        else
        {
            dtBillingCodeData = (DataTable)ViewState["dtBillingCodeData"];
        }
        //RadGrid_gvLogs.Rebind();
        //RadGrid_gvPayment.Rebind();

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
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr6543", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

        return _dtItem;
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

    protected DataTable BuildCompanyDetailsTable()
    {
        DataTable companyDetailsTable = new DataTable();
        companyDetailsTable.Columns.Add("CompanyAddress");
        companyDetailsTable.Columns.Add("CompanyName");
        companyDetailsTable.Columns.Add("ContactNo");
        companyDetailsTable.Columns.Add("Email");
        companyDetailsTable.Columns.Add("Logo");
        companyDetailsTable.Columns.Add("LogoURL");
        companyDetailsTable.Columns.Add("City");
        companyDetailsTable.Columns.Add("State");
        companyDetailsTable.Columns.Add("Zip");
        companyDetailsTable.Columns.Add("Fax");
        companyDetailsTable.Columns.Add("Phone");
        companyDetailsTable.Columns.Add("GSTreg");

        return companyDetailsTable;
    }

    static byte[] mergedPdf = null;

    public static byte[] concatAndAddContent(List<byte[]> pdfByteContent)
    {
        MemoryStream ms = new MemoryStream();
        iTextSharp.text.Document doc = new iTextSharp.text.Document();
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
        //Return just before disposing
        return ms.ToArray();
    }

    protected void lnkPDF_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtInv = (DataTable)Session["InvoiceSrch"];
            var reportFormat = ConfigurationManager.AppSettings["InvoiceReportFormat"].ToString();
            var invoiceNum = Request.QueryString["uid"] != null ? Request.QueryString["uid"].ToString().Trim() : "";
            string fileName = string.Format("Invoice{0}.pdf", invoiceNum);
            string fullPath = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF", fileName);
            if (reportFormat.ToUpper().Equals("MRT"))
            {
                //if (dtInv.Rows.Count > 0)
                //{
                StiWebViewer rvInvoices = new StiWebViewer();

                List<byte[]> invoicesToPrint = PrintInvoices(rvInvoices);

                if (invoicesToPrint != null)
                {
                    byte[] buffer1 = null;

                    buffer1 = concatAndAddContent(invoicesToPrint);

                    if (File.Exists(fullPath))
                        File.Delete(fullPath);
                    using (var fs = new FileStream(fullPath, FileMode.Create))
                    {
                        fs.Write(buffer1, 0, buffer1.Length);
                        fs.Close();
                    }
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Length", (buffer1.Length).ToString());
                    Response.BinaryWrite(buffer1);
                }
            }
            else if (reportFormat.ToUpper().Equals("RDLC"))
            {
                if (dtInv.Rows.Count > 0)
                {
                    ReportViewer rvInvoices = new ReportViewer();

                    PrintInvoices(rvInvoices);

                    byte[] buffer = null;
                    buffer = ExportReportToPDF("", rvInvoices);
                    Response.ClearContent();
                    Response.ClearHeaders();
                    //Response.AddHeader("Content-Disposition", "attachment;filename=Invoices.pdf");
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Length", (buffer.Length).ToString());
                    Response.BinaryWrite(buffer);
                    // Response.Flush();
                    //Response.Close();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No Invoice(s) found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }


        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr951", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private byte[] ExportReportToPDF(string reportName, ReportViewer ReportViewer1)
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
    }

    private List<byte[]> PrintInvoices(StiWebViewer rvInvoices)
    {
        // Export to PDF
        List<byte[]> invoicesAsBytes = new List<byte[]>();
        try
        {
            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            DataSet dsEquip = new DataSet();
            DataTable dtInstallationItems = new DataTable();

            objProp_Contracts.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataTable dtNew = (DataTable)Session["InvoiceSrch"];
            DataTable _dtInvoice = new DataTable();
            DataSet _dsInvoice = new DataSet();
            int j = 0;

            //foreach (DataRow _dr in dtNew.Rows)
            //{
            int _ref = Convert.ToInt32(Request.QueryString["uid"].ToString());

            objProp_Contracts.InvoiceID = _ref;
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
            #region Get Company Address

            string address = dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
            address += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
            address += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;
            address += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;
            if (Session["dbname"].ToString() == "adams" || Session["dbname"].ToString() == "adamstest")
            {
                address = "Cher client : " + Environment.NewLine + "Veuillez consulter la facture ci-jointe pour paiement. " + Environment.NewLine +
                "Veuillez noter qu’il peut y avoir plusieurs factures contenues " + Environment.NewLine +
                "dans chaque pièce jointe. Si vous avez besoin de clarifications, " + Environment.NewLine +
                "n’hésitez pas à nous contacter.  " + Environment.NewLine + Environment.NewLine +

                "Nous vous remercions d'avoir fair affaire avec notre entreprise." + Environment.NewLine + Environment.NewLine +


                "Dear Valued Customer: " + Environment.NewLine + Environment.NewLine +

                "Please review the attached invoice(s) for processing." + Environment.NewLine +
                "Please note there may be multiple invoices contained " + Environment.NewLine +
                "in each attachment. Should you have any questions, " + Environment.NewLine +
                "Please feel free to contact us." + Environment.NewLine + Environment.NewLine +
                "We appreciate your business!" + Environment.NewLine + Environment.NewLine + address;

            }
            else
            {
                address = "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine + address;
            }

            ViewState["CompanyAddress"] = address;

            ViewState["EmailFrom"] = "";
            if (Session["MSM"].ToString() != "TS")
            {
                ViewState["EmailFrom"] = dsC.Tables[0].Rows[0]["Email"].ToString();
            }
            #endregion

            ViewState["InvoiceReport"] = _dtInvoice;
            ViewState["CompanyReport"] = dsC.Tables[0];
            Session["InvoiceReportDetails"] = _dtInvoice;

            DataTable dt = (DataTable)ViewState["InvoiceReport"];
            DataTable dtCompany = (DataTable)ViewState["CompanyReport"];
            int refId = Convert.ToInt32(Request.QueryString["uid"].ToString());
            DataTable _dtInvItems = GetInvoiceItems(refId);

            DataSet companyInfo = new DataSet();
            companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());

            DataTable cTable = BuildCompanyDetailsTable();
            var cRow = cTable.NewRow();

            cRow["CompanyName"] = companyInfo.Tables[0].Rows[0]["Name"].ToString();
            cRow["CompanyAddress"] = companyInfo.Tables[0].Rows[0]["Address"].ToString();
            cRow["ContactNo"] = companyInfo.Tables[0].Rows[0]["Contact"].ToString();
            cRow["Email"] = companyInfo.Tables[0].Rows[0]["Email"].ToString();
            cRow["Logo"] = companyInfo.Tables[0].Rows[0]["Logo"].ToString();
            cRow["City"] = companyInfo.Tables[0].Rows[0]["City"].ToString();
            cRow["State"] = companyInfo.Tables[0].Rows[0]["State"].ToString();
            cRow["Phone"] = companyInfo.Tables[0].Rows[0]["Phone"].ToString();
            cRow["Fax"] = companyInfo.Tables[0].Rows[0]["Fax"].ToString();
            cRow["Zip"] = companyInfo.Tables[0].Rows[0]["Zip"].ToString();
            cRow["GSTreg"] = companyInfo.Tables[0].Rows[0]["GSTreg"].ToString();
            cTable.Rows.Add(cRow);

            // Template name
            string configRepostName = ConfigurationManager.AppSettings["InvoiceReport"].ToString();

            // Load for Port City Installation template
            bool pceInstallation = false;

            if (ConfigurationManager.AppSettings["CustomerName"] == "PCE")
            {
                if (_dtInvoice.Rows.Count > 0)
                {
                    var _objUser = new User();
                    _objUser.ConnConfig = Session["config"].ToString();
                    _objUser.SearchBy = string.Empty;

                    _objUser.LocID = Convert.ToInt32(_dtInvoice.Rows[0]["Loc"]);
                    _objUser.InstallDate = string.Empty;
                    _objUser.ServiceDate = string.Empty;
                    _objUser.Price = string.Empty;
                    _objUser.Manufacturer = string.Empty;
                    _objUser.Status = -1;
                    _objUser.building = "All";

                    dsEquip = objBL_User.getElev(_objUser);

                    if (_dtInvoice.Rows[0]["TypeName"] != null && _dtInvoice.Rows[0]["TypeName"].ToString() == "Installation")
                    {
                        pceInstallation = true;
                        configRepostName = "InstallationInvoicesDefault-PortCity.mrt";
                        dtInstallationItems = GetPCEInstallationInvoiceItems(_dtInvoice, _dtInvItems);
                    }

                    var cBilling = _dtInvItems.Select("billcode like '%-C'");
                    if (cBilling.Count() > 0)
                    {
                        cTable.Rows[0]["CompanyAddress"] = "2652 Bonds Avenue, Suite 101";
                        cTable.Rows[0]["City"] = "North Charleston";
                        cTable.Rows[0]["State"] = "SC";
                        cTable.Rows[0]["Zip"] = "29405";
                        cTable.Rows[0]["Phone"] = "843-729-1006";
                    }
                }
            }

            // Load template
            string reportPathStimul = Server.MapPath(string.Format("StimulsoftReports/Invoices/{0}", configRepostName));
            StiReport report = new StiReport();

            report.Load(reportPathStimul);
            report.RegData("CompanyDetails", cTable);
            report.RegData("Invoices", _dtInvoice);
            report.RegData("Invoice_dtInvoice", ds.Tables[0]);
            report.RegData("Ticket_Company", dsC.Tables[0]);

            if (pceInstallation)
            {
                report.RegData("InvoiceItems", dtInstallationItems);
            }
            else
            {
                report.RegData("InvoiceItems", _dtInvItems);
            }

            if (report.DataSources.Contains("Equipments") && dsEquip.Tables.Count > 0)
            {
                report.RegData("Equipments", dsEquip.Tables[0]);
            }

            report.CacheAllData = true;
            report.Render();
            rvInvoices.Report = report;

            byte[] buffer1 = null;
            var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            var service = new Stimulsoft.Report.Export.StiPdfExportService();
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            service.ExportTo(rvInvoices.Report, stream, settings);
            buffer1 = stream.ToArray();
            invoicesAsBytes.Add(buffer1);

            j++;

            return invoicesAsBytes;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr753", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return invoicesAsBytes;
        }
    }

    private DataTable GetPCEInstallationInvoiceItems(DataTable dtInvoice, DataTable dtInvItems)
    {
        DataTable dtInstallationItems = dtInvItems.Clone();

        var positiveAmount = dtInvItems.AsEnumerable()
            .Where(y => y.Field<decimal>("Amount") > 0)
            .Sum(x => x.Field<decimal>("Amount"));

        if (positiveAmount != 0)
        {
            var dr = dtInstallationItems.NewRow();
            dr["fDesc"] = dtInvoice.Rows[0]["fDesc"];
            dr["Amount"] = positiveAmount;

            dtInstallationItems.Rows.Add(dr);
        }

        var negativeAmount = dtInvItems.AsEnumerable()
            .Where(y => y.Field<decimal>("Amount") < 0)
            .Sum(x => x.Field<decimal>("Amount"));

        if (negativeAmount != 0)
        {
            var dr = dtInstallationItems.NewRow();
            dr["fDesc"] = "Deposit";
            dr["Amount"] = negativeAmount;

            dtInstallationItems.Rows.Add(dr);
        }

        return dtInstallationItems;
    }

    public void ItemDetailsSubReportProcessing(object sender, SubreportProcessingEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["InvoiceReport"];
            DataTable dtCompany = (DataTable)ViewState["CompanyReport"];
            int refId = Convert.ToInt32(dt.Rows[count_inv]["Ref"]);
            DataTable _dtInvItems = GetInvoiceItems(refId);

            if (_dtInvItems.Rows.Count > 0)
            {
                ReportDataSource rdsItems = new ReportDataSource("dtInvoiceItems", _dtInvItems);

                e.DataSources.Add(rdsItems);
            }
            if (count_inv == dt.Rows.Count - 1)
            {
                ViewState["InvoiceReport"] = null;
                ViewState["CompanyReport"] = null;
            }
            count_inv++;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr78954", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void PrintInvoices(ReportViewer rvInvoices)
    {
        // Export to PDF
        try
        {
            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataTable dtNew = (DataTable)Session["InvoiceSrch"];
            DataTable _dtInvoice = new DataTable();
            DataSet _dsInvoice = new DataSet();
            int j = 0;

            //foreach (DataRow _dr in dtNew.Rows)
            //{
            int _ref = Convert.ToInt32(Request.QueryString["uid"].ToString());

            objProp_Contracts.InvoiceID = _ref;
            ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);

            if (j > 0)
            {
                _dtInvoice.Merge(ds.Tables[0], true);
            }
            else
            {
                _dtInvoice = ds.Tables[0];
            }
            j++;
            //}
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
                //_dtInvoice.Rows[i]["iAmount"] = _dtInvoice.Rows[i]["Total"];
                //_dtInvoice.Rows[i]["iSTax"] = _dtInvoice.Rows[i]["STax"];

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
            #region Get Company Address

            string address = dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
            address += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
            address += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;
            address += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;
            if (Session["dbname"].ToString() == "adams" || Session["dbname"].ToString() == "adamstest")
            {
                address = "Cher client : " + Environment.NewLine + "Veuillez consulter la facture ci-jointe pour paiement. " + Environment.NewLine +
"Veuillez noter qu’il peut y avoir plusieurs factures contenues " + Environment.NewLine +
"dans chaque pièce jointe. Si vous avez besoin de clarifications, " + Environment.NewLine +
"n’hésitez pas à nous contacter.  " + Environment.NewLine + Environment.NewLine +

"Nous vous remercions d'avoir fair affaire avec notre entreprise." + Environment.NewLine + Environment.NewLine +


"Dear Valued Customer: " + Environment.NewLine + Environment.NewLine +

"Please review the attached invoice(s) for processing." + Environment.NewLine +
"Please note there may be multiple invoices contained " + Environment.NewLine +
"in each attachment. Should you have any questions, " + Environment.NewLine +
"Please feel free to contact us." + Environment.NewLine + Environment.NewLine +
"We appreciate your business!" + Environment.NewLine + Environment.NewLine + address;

            }
            else
            {
                address = "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine + address;
            }

            ViewState["CompanyAddress"] = address;

            ViewState["EmailFrom"] = "";
            if (Session["MSM"].ToString() != "TS")
            {
                ViewState["EmailFrom"] = dsC.Tables[0].Rows[0]["Email"].ToString();
            }
            #endregion

            //foreach (DataRow dr in _dtInvoice.Rows)
            //{
            //    //billTo = Regex.Replace(billTo, @"( |\r?\n)\1+", "$1");  // to remove first new line.
            //    //string billTo = Regex.Replace(dr["Billto"].ToString(), @"\t|\n|\r", "");          // to remove all new lines.
            //    //billTo = Regex.Replace(billTo, @"^,+|,+$|,+(,\w)", "$1");
            //    //billTo = billTo.Split(new[] { ',' }, 2).First() + ",\n" + billTo.Split(new[] { ',' }, 2).Last();                
            //    //dr["Billto"] = billTo;
            //}
            ViewState["InvoiceReport"] = _dtInvoice;
            ViewState["CompanyReport"] = dsC.Tables[0];
            Session["InvoiceReportDetails"] = _dtInvoice;

            rvInvoices.LocalReport.DataSources.Clear();

            rvInvoices.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemDetailsSubReportProcessing);
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("Invoice_dtInvoice", _dtInvoice));
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));

            string reportPath = "Reports/Invoices.rdlc";
            string Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceDetailsReport"].Trim();
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                reportPath = "Reports/" + Report.Trim();
            }
            string eventTarget = this.Request.Params.Get("__EVENTTARGET");
            if (eventTarget.Contains("lnkAdamMaintenance"))
            {
                reportPath = "Reports/InvoicesForAdamMaintenance.rdlc";
            }
            if (eventTarget.Contains("lnkAdamBilling"))
            {
                reportPath = "Reports/InvoicesForAdamBill.rdlc";
            }

            rvInvoices.LocalReport.ReportPath = reportPath;

            rvInvoices.LocalReport.EnableExternalImages = true;
            List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
            string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
            param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
            if (string.IsNullOrEmpty(Report.Trim()) || reportPath == "Reports/InvoicesInFrench.rdlc" || reportPath == "Reports/InvoicesForAdamMaintenance.rdlc" || reportPath == "Reports/InvoicesAdams.rdlc")
            {
                if (ViewState["IsGst"] != null)
                    param1.Add(new ReportParameter("IsGstTax", ViewState["IsGst"].ToString()));
                //else
                //    param1.Add(new ReportParameter("IsGstTax", ""));
            }

            rvInvoices.LocalReport.SetParameters(param1);

            rvInvoices.LocalReport.Refresh();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr301", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void IntializeData()
    {
        ViewState["mode"] = 0;

        ViewState["editcon"] = 0;

        ViewState["invoicetable"] = null;

        CreateTable();

        FillProjectCodes(false, out int GLAcct);

        if (Request.QueryString["tickid"] != null)
        {
            ViewState["ResetGrid"] = 1;
        }
        BindGrid();

        txtInvoiceDate.Text = DateTime.Now.ToShortDateString();
    }

    private void SetPaidBtn()
    {
        _objInvoices.ConnConfig = Session["config"].ToString();
        _objInvoices.Ref = Convert.ToInt32(Request.QueryString["uid"]);
        DataSet ds = objBL_Invoice.GetReceivePayInvoice(_objInvoices);
        string imgLnk = "";
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                imgLnk += " window.open('addreceivepayment.aspx?id=" + dr["ReceivedPaymentID"].ToString() + "');";
            }
        }
        else
        {
            ds = objBL_Invoice.GetAppliedDeposit(_objInvoices);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    imgLnk += " window.open('adddeposit.aspx?id=" + dr["Ref"].ToString() + "');";
                }
            }
        }
        // imgPaid.Attributes.Add("onClick", imgLnk);
    }

    private void getControl()
    {
        DataSet dsC = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        dsC = objBL_User.getControl(objPropUser);

        if (dsC.Tables[0].Rows.Count > 0)
        {
            lblCompNme.Text = dsC.Tables[0].Rows[0]["Name"].ToString();
            lblCompAddress.Text = dsC.Tables[0].Rows[0]["Address"].ToString();
            lblCompCity.Text = dsC.Tables[0].Rows[0]["city"].ToString();
            lblCompState.Text = dsC.Tables[0].Rows[0]["state"].ToString();
            lblCompZip.Text = dsC.Tables[0].Rows[0]["Zip"].ToString();
            lblCompphone.Text = dsC.Tables[0].Rows[0]["phone"].ToString();
        }
    }

    private void LoadData()
    {
        IntializeData();

        if (Request.QueryString["c"] != null)
        {
            if (Request.QueryString["uid"] != null)
            {
                lblHeader.Text = "Copy Invoice";
                lnkReceiptPayment.Visible = false;
                SetForEdit();
            }
        }
        else if (Request.QueryString["uid"] != null)
        {
            //ddlStatus.Enabled = true;
            lblInv.Visible = true;

            pnlNext.Visible = true;
            ViewState["mode"] = 1;
            lblHeader.Text = "Edit Invoice";

            SetForEdit();
        }
        else if (Request.QueryString["lid"] != null)
        {
            if (Request.QueryString["page"] == "addcustomer")
            {
                hdnPatientId.Value = Request.QueryString["lid"].ToString();
                FillLoc();
            }
            else if (Request.QueryString["page"] == "addlocation")
            {
                hdnLocId.Value = Request.QueryString["lid"].ToString();

                FillLocInfo();

                GetDataProject();

                FillInvoiceProjectInfo();
            }
        }
        LoadTicketData();
        loadLog();
    }

    private void SetForEdit()
    {
        try
        {
            int LocStatus = 0;
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.InvoiceID = Convert.ToInt32(Request.QueryString["uid"]);
            DataSet ds = new DataSet();
            //todo
            if (Convert.ToBoolean(ViewState["isCanada"]))
            {
                ds = objBL_Contracts.GetInvoiceByInvoiceID(objProp_Contracts);
            }
            else
            {
                ds = objBL_Contracts.GetInvoicesByID(objProp_Contracts);
            }


            if (ds.Tables[0].Rows.Count > 0)
            {
                chkrecurring.Enabled = false;
                chkrecurring.Checked = false;
                if (Convert.ToString(ds.Tables[0].Rows[0]["isrecurring"]) == "1")
                {
                    chkrecurring.Checked = true;
                }


                // lblInvoiceName.Text = ds.Tables[0].Rows[0]["ref"].ToString();
                if (Convert.ToString(ds.Tables[0].Rows[0]["ref"]) != "")
                {
                    lblInv.Text = "Invoice# " + Convert.ToString(ds.Tables[0].Rows[0]["ref"]);
                }
                txtInvoiceNo.Text = ds.Tables[0].Rows[0]["custom1"].ToString();
                txtCustomer.Text = ds.Tables[0].Rows[0]["customername"].ToString();
                txtLocation.Text = ds.Tables[0].Rows[0]["locname"].ToString();
                txtAddress.Text = ds.Tables[0].Rows[0]["billto"].ToString();
                txtRemarks.Text = ds.Tables[0].Rows[0]["fdesc"].ToString();
                txtPO.Text = ds.Tables[0].Rows[0]["po"].ToString();
                //ddlTerms.SelectedValue = ds.Tables[0].Rows[0]["terms"].ToString();
                hdnTaxRegion.Value = ds.Tables[0].Rows[0]["taxregion"].ToString();
                LocStatus = Convert.ToInt16(ds.Tables[0].Rows[0]["locStatus"]);
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["job"].ToString()))
                {
                    ddlDepartment.Enabled = false;

                    if (Convert.ToString(ds.Tables[0].Rows[0]["job"]) != "0")
                    {
                        ViewState["DefaultAcct"] = Convert.ToInt32(ds.Tables[0].Rows[0]["InvServ"] == DBNull.Value ? "0" : ds.Tables[0].Rows[0]["InvServ"]);
                    }
                }


                if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["job"].ToString()) && string.IsNullOrEmpty(ds.Tables[0].Rows[0]["JobDecs"].ToString()))
                {
                    txtProject.Text = "";
                }
                else
                {
                    if (Convert.ToString(ds.Tables[0].Rows[0]["job"]) != "0")
                    {
                        txtProject.Text = ds.Tables[0].Rows[0]["job"].ToString() + "-" + ds.Tables[0].Rows[0]["JobDecs"].ToString();
                        //saleperson     
                        objCustomer.ConnConfig = Session["config"].ToString();
                        objCustomer.ProjectJobID = int.Parse(ds.Tables[0].Rows[0]["job"].ToString());
                        DataSet dsSale = objBL_Customer.GetSalePersonByJob(objCustomer);
                        if (dsSale.Tables[0].Rows.Count > 0)
                        {
                            ddlsaleperson.SelectedValue = dsSale.Tables[0].Rows[0]["ID"].ToString();
                        }

                        hdnProjectId.Value = (ds.Tables[0].Rows[0]["job"].ToString());
                        hdnProjectStatus.Value = ds.Tables[0].Rows[0]["jobStatus"].ToString();



                        FillProjectCodes(true, out int GLAcct);

                        if (Request.QueryString["c"] == null)
                        {

                            if (hdnProjectStatus.Value == "1")
                            {
                                btnSubmit.Visible = false;
                                divProjectClose.Visible = true;

                            }
                        }



                        ViewState["dtBillingCodeData"] = getBillingCodeData(GLAcct);
                    }
                    else
                    {
                        ddlDepartment.Enabled = true;
                    }
                }

                txtJobRemarks.Text = ds.Tables[0].Rows[0]["JobRemarks"].ToString();

                if (ds.Tables[0].Rows[0]["SPHandle"].ToString() == "1")
                {
                    lblJobSRemarks.Visible = true;
                    txtJobSRemarks.Visible = true;
                    txtJobSRemarks.Text = ds.Tables[0].Rows[0]["SRemarks"].ToString();
                }
                else
                {
                    txtJobSRemarks.Text = "";
                    lblJobSRemarks.Visible = false;
                    txtJobSRemarks.Visible = false;
                }

                txtStaxrate.Text = ds.Tables[0].Rows[0]["taxregion"].ToString();

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ddate"].ToString()))
                {
                    txtDueDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["ddate"]).ToShortDateString();
                }
                if (Convert.ToString(ds.Tables[0].Rows[0]["type"]) != "-1")
                {
                    ddlDepartment.SelectedValue = ds.Tables[0].Rows[0]["type"].ToString();
                }

                if (Request.QueryString["c"] == null)
                {
                    ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
                }
                else
                    ddlStatus.SelectedValue = "0";
                txtInvoiceDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["fdate"]).ToShortDateString();
                if (ds.Tables[0].Rows[0]["mech"].ToString() != string.Empty)
                {
                    FillWorker(Convert.ToInt32(ds.Tables[0].Rows[0]["mech"].ToString()));
                    ddlRoute.SelectedValue = ds.Tables[0].Rows[0]["mech"].ToString();
                }
                //saleperson
                if (ds.Tables[0].Rows[0]["AssignedTo"].ToString() != string.Empty)
                {
                    ddlsaleperson.SelectedValue = ds.Tables[0].Rows[0]["AssignedTo"].ToString();
                }

                hdnLocId.Value = ds.Tables[0].Rows[0]["loc"].ToString();
                hdnPatientId.Value = ds.Tables[0].Rows[0]["owner"].ToString();

                //Set Hyperlink  For Loc / Customer
                if (hdnLocId.Value != "0" && hdnLocId.Value != "")
                {
                    lnkLocationID.NavigateUrl = "addlocation.aspx?uid=" + hdnLocId.Value;

                }
                else
                    lnkLocationID.NavigateUrl = "";


                if (hdnPatientId.Value != "0" && hdnPatientId.Value != "")
                    lnkCustomerID.NavigateUrl = "addcustomer.aspx?uid=" + hdnPatientId.Value;
                else
                    lnkCustomerID.NavigateUrl = "";


                FillCompanyInfo();


                ListItem item = ddlTerms.Items.FindByValue(ds.Tables[0].Rows[0]["terms"].ToString());
                if (item != null)
                {
                    ddlTerms.SelectedValue = ds.Tables[0].Rows[0]["terms"].ToString();
                }

                UpdateDueByTerms();

                GetDataProject();

                FillInvoiceProjectInfo();

                hdnStax.Value = ds.Tables[0].Rows[0]["TaxRate"].ToString();
                txtStaxrate.Text = ds.Tables[0].Rows[0]["taxregion"].ToString() + " - " + hdnStax.Value + " %";
                hdnsTaxType.Value = !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TaxType"].ToString()) ? ds.Tables[0].Rows[0]["TaxType"].ToString() : "0";

                //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["taxregion"].ToString()))
                //{
                //    objPropUser.Stax = ds.Tables[0].Rows[0]["taxregion"].ToString();
                //    DataSet dsStax = new DataSet();
                //    dsStax = objBL_User.getSalesTaxByID(objPropUser);

                //    if (dsStax.Tables[0].Rows.Count > 0)
                //    {
                //        hdnStax.Value = dsStax.Tables[0].Rows[0]["rate"].ToString();
                //        txtStaxrate.Text = ds.Tables[0].Rows[0]["taxregion"].ToString() + " - " + hdnStax.Value + " %";
                //        hdnsTaxType.Value = dsStax.Tables[0].Rows[0]["Type"].ToString();
                //    }
                //}
                if (Request.QueryString["c"] == null)
                {
                    if (ds.Tables[0].Rows[0]["status"].ToString().Equals("2"))
                    {
                        imgVoid.Visible = true;
                        btnSubmit.Visible = false;
                    }
                    else if (ds.Tables[0].Rows[0]["status"].ToString().Equals("1"))
                    {
                        imgPaid.Visible = true;
                        btnSubmit.Visible = false;
                        lnkReceiptPayment.Visible = false;
                        disableControl();
                    }
                }
            }


            if (ds.Tables[1].Rows.Count > 0)
            {


                DataTable _dtInvItems = ds.Tables[1];

                ViewState["invoicetable"] = _dtInvItems;

                foreach (DataRow row in _dtInvItems.Rows)
                {
                    string find = "Line = '" + Convert.ToString(row["Code"]) + "'";
                    DataRow[] foundRows = dtProjectCodeData.Select(find);
                    if (foundRows.Length == 0)
                    {
                        row["Code"] = "0";
                        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr5362", "noty({text: 'Project does not have BOM/ Budget data',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
                BindGrid();
            }
            string _returnValues = calculateTotal();
            if (!string.IsNullOrEmpty(_returnValues))
            {
                var _arrreturnValues = _returnValues.Split('|');

                hdnTotalAmount.Value = _arrreturnValues[0].ToString(); // change by Mayuri 19th dec, 15
            }
            //check old data
            if (isCanadaCompany())
            {
                if (ds.Tables[2] != null)
                {
                    hdnIsOldData.Value = ds.Tables[2].Rows[0]["IsOldData"].ToString();
                }

            }
            //FillProjectCodes(false);
            //if (LocStatus == 1)
            //{
            //    ClientScript.RegisterStartupScript(Page.GetType(), "LocInactive", "noty({text: 'This location is inactive. Please change the location name before proceeding.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            //}
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void Permission()
    {
        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("portalhome.aspx");
            //if (Request.QueryString["uid"] == null)
            //{
            //    Response.Redirect("home.aspx");
            //}
            //else
            //{
            //    //pnlSave.Visible = false;
            //    //lnkReceiptPayment.Visible = false;
            //    btnSubmit.Visible = false;
            //    lblHeader.Text = "Invoice";
            //}
        }

        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx");
            //btnSubmit.Visible = false;
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }

        // User Permission 
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = GetUserById();

            /// BillingmodulePermission ///////////////////------->

            string BillingmodulePermission = ds.Rows[0]["BillingmodulePermission"] == DBNull.Value ? "Y" : ds.Rows[0]["BillingmodulePermission"].ToString();

            if (BillingmodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }


            /// Invoice ///////////////////------->

            string InvoicePermission = ds.Rows[0]["Invoice"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Invoice"].ToString();
            string ADD = InvoicePermission.Length < 1 ? "Y" : InvoicePermission.Substring(0, 1);
            string Edit = InvoicePermission.Length < 2 ? "Y" : InvoicePermission.Substring(1, 1);
            string Delete = InvoicePermission.Length < 2 ? "Y" : InvoicePermission.Substring(2, 1);
            string View = InvoicePermission.Length < 4 ? "Y" : InvoicePermission.Substring(3, 1);

            string ApplyPermission = ds.Rows[0]["Apply"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Apply"].ToString();
            string stAddeApply = ApplyPermission.Length < 1 ? "Y" : ApplyPermission.Substring(0, 1);
            string stEditApply = ApplyPermission.Length < 2 ? "Y" : ApplyPermission.Substring(1, 1);
            string stDeleteApply = ApplyPermission.Length < 3 ? "Y" : ApplyPermission.Substring(2, 1);
            string stViewApply = ApplyPermission.Length < 4 ? "Y" : ApplyPermission.Substring(3, 1);

            if (View == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
            else if (Request.QueryString["uid"] == null)
            {
                if (ADD == "N")
                {
                    Response.Redirect("Home.aspx?permission=no"); return;
                }
            }
            else if (Edit == "N")
            {
                if (View == "Y")
                {
                    btnSubmit.Visible = false;
                    //lnkReceiptPayment.Visible = false;
                }
                else
                {
                    Response.Redirect("Home.aspx?permission=no"); return;
                }
            }

            if (stAddeApply == "N")
            {
                lnkReceiptPayment.Visible = false;
            }

        }
    }

    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            dvCompanyPermission.Visible = true;
        }
        else
        {
            dvCompanyPermission.Visible = false;
        }
    }

    private DataTable GetUserById()
    {
        User objPropUser = new User();
        objPropUser.TypeID = Convert.ToInt32(Session["usertypeid"]);
        objPropUser.UserID = Convert.ToInt32(Session["userid"]);
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.DBName = Session["dbname"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_User.GetUserPermissionByUserID(objPropUser);
        return ds.Tables[0];
    }
    private void FillWorker(int Worker)
    {
        ddlRoute.Items.Clear();
        ddlRoute.SelectedIndex = -1;
        ddlRoute.SelectedValue = null;
        ddlRoute.ClearSelection();
        DataSet ds = new DataSet();
        objPropUser.WorkId = Worker;
        objPropUser.Status = 0;
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getEMP(objPropUser);
        ddlRoute.DataSource = ds.Tables[0];
        ddlRoute.DataTextField = "fDesc";
        ddlRoute.DataValueField = "id";
        ddlRoute.DataBind();

        ddlRoute.Items.Insert(0, new ListItem(":: Select ::", "0"));
    }

    private void FillSaleperson()
    {
        int refID = Request.QueryString["uid"] != null ? Convert.ToInt32(Request.QueryString["uid"].ToString()) : 0;
        ddlsaleperson.Items.Clear();
        ddlsaleperson.SelectedIndex = -1;
        ddlsaleperson.SelectedValue = null;
        ddlsaleperson.ClearSelection();
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        //ds = objBL_User.getTerritory(objPropUser, new GeneralFunctions().GetSalesAsigned());
        ds = objBL_User.GetSalesPerson(objPropUser, new GeneralFunctions().GetSalesAsigned(), refID, "INVOICE", "t.SDesc");
        ddlsaleperson.DataSource = ds.Tables[0];
        ddlsaleperson.DataTextField = "SDesc";
        ddlsaleperson.DataValueField = "ID";
        ddlsaleperson.DataBind();
        ddlsaleperson.Items.Insert(0, new ListItem(":: Select ::", "0"));
    }

    private void FillBillCodes(int GLAcct)
    {
        DataSet ds = new DataSet();

        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.GLAccount = GLAcct;
        //if (Request.QueryString["uid"] != null)
        //{
        //    ds = new BL_BillCodes().GetAllBillCodes(objPropUser);
        //}
        //else
        //{
        ds = new BL_BillCodes().getBillCodes(objPropUser);
        //}


        DataTable dt = (DataTable)ViewState["invoicetable"];
        if (dt.Rows.Count == 0)
        {
            DataRow dr = dt.NewRow();
            dr["acct"] = DBNull.Value;
            dr["line"] = dt.Rows.Count;
            dr["stax"] = 0;
            dr["Code"] = 0;
            dr["GTaxAmt"] = 0.00;
            dr["InvStatus"] = 0;

            DataRow dr1 = dt.NewRow();
            dr1["acct"] = DBNull.Value;
            dr1["line"] = dt.Rows.Count;
            dr1["stax"] = 0;
            dr1["Code"] = 0;
            dr["GTaxAmt"] = 0.00;

            dt.Rows.Add(dr);
            dt.Rows.Add(dr1);

            ViewState["invoicetable"] = dt;
        }
        if (ViewState["invoicetable"] != null)
        {
            foreach (DataRow dr in dt.Rows)
            {
                //dr["code"] = dr["code"].ToString() == "0" ? CodeValue : dr["code"];
                dr["acct"] = Convert.ToInt32(ds.Tables[0].Rows[0]["id"]);
                dr["InvStatus"] = Convert.ToString(ds.Tables[0].Rows[0]["statusid"]);
                dr["AStatus"] = ds.Tables[0].Rows[0]["Status"].ToString().ToLower() == "active" ? 0 : 1;
                dr["INVType"] = Convert.ToInt32(ds.Tables[0].Rows[0]["Type"]);
                dr["fDesc"] = Convert.ToString(ds.Tables[0].Rows[0]["fDesc"]);
                dr["Price"] = Convert.ToDouble(ds.Tables[0].Rows[0]["Price1"]);
                dr["billcode"] = Convert.ToString(ds.Tables[0].Rows[0]["BillType"]);

            }
            RadGrid_gvInvoices.DataSource = dt;
            RadGrid_gvInvoices.Rebind();
        }

        DataRow drr = ds.Tables[0].NewRow();

        ds.Tables[0].Rows.InsertAt(drr, 0);

        ViewState["dtBillingCodeData"] = dtBillingCodeData = ds.Tables[0];

        //List<Dictionary<object, object>> dictionary = new List<Dictionary<object, object>>();

        //JavaScriptSerializer sr = new JavaScriptSerializer();

        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    dictionary = objGeneral.RowsToDictionary(ds.Tables[0]);

        //    string str = sr.Serialize(dictionary);

        //    hdnBillCodeJSON.Value = str;
        //}


    }

    private void FillProjectCodes(bool GetfromServer, out int GLAcct)
    {
        GLAcct = 0;
        try
        {
            if (hdnProjectId.Value != string.Empty)
            {
                Customer objProp_Customer = new Customer();

                BL_Customer objBL_Customer = new BL_Customer();

                objProp_Customer.ConnConfig = Session["config"].ToString();

                objProp_Customer.ProjectJobID = Convert.ToInt32(hdnProjectId.Value);

                objProp_Customer.Type = "0";
                if (GetfromServer)
                {
                    DataSet ds = objBL_Customer.getJobProjectByJobID(objProp_Customer);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        GLAcct = Convert.ToInt32(ds.Tables[0].Rows[0]["InvServ"] == DBNull.Value ? 0 : ds.Tables[0].Rows[0]["InvServ"]);
                        DataRow dr = ds.Tables[1].NewRow();

                        dr["Line"] = 0;

                        dr["billtype"] = "-Select-";

                        ds.Tables[1].Rows.InsertAt(dr, 0);

                        ViewState["dtProjectCodeData"] = dtProjectCodeData = ds.Tables[1];

                        if (ds.Tables[0].Rows[0]["type"].ToString() != string.Empty && ds.Tables[0].Rows[0]["type"].ToString() != "0")
                        {

                            ddlDepartment.SelectedValue = ds.Tables[0].Rows[0]["type"].ToString();
                        }
                    }
                    else
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("billtype");
                        dt.Columns.Add("Line");
                        DataRow dr = dt.NewRow();
                        dr["Line"] = 0;
                        dr["billtype"] = "-Select-";
                        dt.Rows.Add(dr);
                        dtProjectCodeData = dt;

                        ViewState["dtProjectCodeData"] = dtProjectCodeData = dt;
                    }

                }
                else
                {
                    if (ViewState["dtProjectCodeData"] == null)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("billtype");
                        dt.Columns.Add("Line");
                        DataRow dr = dt.NewRow();
                        dr["Line"] = 0;
                        dr["billtype"] = "-Select-";
                        dt.Rows.Add(dr);
                        dtProjectCodeData = dt;

                        ViewState["dtProjectCodeData"] = dtProjectCodeData = dt;
                    }
                    else
                    {
                        dtProjectCodeData = (DataTable)ViewState["dtProjectCodeData"];
                    }

                }
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("billtype");
                dt.Columns.Add("Line");
                DataRow dr = dt.NewRow();
                dr["Line"] = 0;
                dr["billtype"] = "-Select-";
                dt.Rows.Add(dr);
                dtProjectCodeData = dt;

                ViewState["dtProjectCodeData"] = dtProjectCodeData = dt;
            }


        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr87458", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void FillDepartment()
    {
        try
        {
            DataSet ds = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();

            ds = objBL_User.getDepartment(objPropUser);

            ddlDepartment.DataSource = ds.Tables[0];
            ddlDepartment.DataTextField = "type";
            ddlDepartment.DataValueField = "id";
            ddlDepartment.DataBind();

            ddlDepartment.Items.Insert(0, new ListItem(":: Select ::", ""));
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr9693", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void FillTerms()
    {
        try
        {
            DataSet ds = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();

            ds = objBL_User.getTerms(objPropUser);

            ddlTerms.DataSource = ds.Tables[0];
            ddlTerms.DataTextField = "name";
            ddlTerms.DataValueField = "id";
            ddlTerms.DataBind();

            ddlTerms.Items.Insert(0, new ListItem(":: Select ::", ""));
            if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("PCE"))
            {
                if (ds.Tables[0].Select("name ='Due on receipt'").Count() > 0)
                {
                    DataRow[] rows = ds.Tables[0].Select("name ='Due on receipt'");
                    ddlTerms.SelectedValue = rows[0]["id"].ToString();
                }

            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr754", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    /// <summary>
    ///  Invoice Grid Table
    /// </summary>
    ///

    private void CreateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Ref", typeof(int));
        dt.Columns.Add("line", typeof(int));
        dt.Columns.Add("Acct", typeof(int));
        dt.Columns.Add("Quan", typeof(double));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Price", typeof(double));
        dt.Columns.Add("Amount", typeof(double));
        dt.Columns.Add("STax", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("JobItem", typeof(int));
        dt.Columns.Add("TransID", typeof(int));
        dt.Columns.Add("Measure", typeof(string));
        dt.Columns.Add("Disc", typeof(double));
        dt.Columns.Add("billcode", typeof(string));
        dt.Columns.Add("PriceQuant", typeof(double));
        dt.Columns.Add("StaxAmt", typeof(double));
        dt.Columns.Add("GTaxAmt", typeof(double));
        dt.Columns.Add("Code", typeof(int));
        dt.Columns.Add("INVType", typeof(int));
        dt.Columns.Add("Warehouse", typeof(string));
        dt.Columns.Add("WHLocID", typeof(int));
        dt.Columns.Add("InvStatus", typeof(int));
        dt.Columns.Add("AStatus", typeof(int));
        //todo
        if (Convert.ToBoolean(ViewState["isCanada"]))
        {
            dt.Columns.Add("EnableGSTTax", typeof(Boolean));
        }
        //DataRow dr = dt.NewRow();
        //dr["acct"] = DBNull.Value;
        //dr["line"] = dt.Rows.Count;
        //dr["stax"] = 0;
        //dr["Code"] = 0;
        //dr["GTaxAmt"] = 0;
        //dr["InvStatus"] = 0;

        //DataRow dr1 = dt.NewRow();
        //dr1["acct"] = DBNull.Value;
        //dr1["line"] = dt.Rows.Count;
        //dr1["stax"] = 0;
        //dr1["Code"] = 0;
        //dr["GTaxAmt"] = 0;

        //dt.Rows.Add(dr);
        //dt.Rows.Add(dr1);

        ViewState["invoicetable"] = dt;
    }

    public void FillLoc()
    {
        try
        {
            DataSet ds = new DataSet();
            objPropUser.SearchValue = "";
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.CustomerID = Convert.ToInt32(hdnPatientId.Value);
            ds = objBL_User.getLocationAutojquery(objPropUser);

            if (ds.Tables[0].Rows.Count == 1)
            {
                hdnLocId.Value = ds.Tables[0].Rows[0]["value"].ToString();
                txtLocation.Text = ds.Tables[0].Rows[0]["label"].ToString();

                FillLocInfo();

                GetDataProject();

                FillInvoiceProjectInfo();
            }

            //Set Hyperlink  For Loc / Customer
            if (hdnLocId.Value != "0" && hdnLocId.Value != "")
            {
                lnkLocationID.NavigateUrl = "addlocation.aspx?uid=" + hdnLocId.Value;

            }
            else
                lnkLocationID.NavigateUrl = "";


            if (hdnPatientId.Value != "0" && hdnPatientId.Value != "")
                lnkCustomerID.NavigateUrl = "addcustomer.aspx?uid=" + hdnPatientId.Value;
            else
                lnkCustomerID.NavigateUrl = "";


        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr598", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void FillLocInfo()
    {
        try
        {
            if (hdnLocId.Value == "")
            {
                return;
            }
            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.LocID = Convert.ToInt32(hdnLocId.Value);
            DataSet ds = new DataSet();
            ds = objBL_User.getLocationByID(objPropUser);

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtLocation.Text = ds.Tables[0].Rows[0]["Tag"].ToString();
                txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString() + "," + Environment.NewLine + ds.Tables[0].Rows[0]["city"].ToString() + ", " + ds.Tables[0].Rows[0]["State"].ToString() + " " + ds.Tables[0].Rows[0]["Zip"].ToString();
                txtCustomer.Text = ds.Tables[0].Rows[0]["custname"].ToString();
                hdnPatientId.Value = ds.Tables[0].Rows[0]["owner"].ToString();
                //txtStaxrate.Text = ds.Tables[0].Rows[0]["stax"].ToString();
                txtCompany.Text = ds.Tables[0].Rows[0]["Company"].ToString();
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["stax"].ToString()))
                {
                    txtStaxrate.Text = ds.Tables[0].Rows[0]["stax"].ToString() + " - " + ds.Tables[0].Rows[0]["Rate"].ToString() + " %";
                    hdnStax.Value = ds.Tables[0].Rows[0]["Rate"].ToString();

                }
                hdnTaxRegion.Value = ds.Tables[0].Rows[0]["stax"].ToString();
                hdnsTaxType.Value = !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["sTaxType"].ToString()) ? ds.Tables[0].Rows[0]["sTaxType"].ToString() : "0";


                ListItem item = ddlTerms.Items.FindByValue(ds.Tables[0].Rows[0]["defaultterms"].ToString());
                if (item != null)
                {
                    ddlTerms.SelectedValue = ds.Tables[0].Rows[0]["defaultterms"].ToString();
                }

                UpdateDueByTerms();


            }

            //Set Hyperlink  For Loc / Customer
            if (hdnLocId.Value != "0" && hdnLocId.Value != "")
            {
                lnkLocationID.NavigateUrl = "addlocation.aspx?uid=" + hdnLocId.Value;

            }
            else
                lnkLocationID.NavigateUrl = "";


            if (hdnPatientId.Value != "0" && hdnPatientId.Value != "")
                lnkCustomerID.NavigateUrl = "addcustomer.aspx?uid=" + hdnPatientId.Value;
            else
                lnkCustomerID.NavigateUrl = "";
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr5623", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void FillCompanyInfo()
    {
        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.LocID = Convert.ToInt32(hdnLocId.Value);
        objPropUser.ConnConfig = Session["config"].ToString();
        DataSet dc = new DataSet();
        dc = objBL_User.getLocationByID(objPropUser);

        if (dc.Tables[0].Rows.Count > 0)
        {
            txtCompany.Text = dc.Tables[0].Rows[0]["Company"].ToString();
        }
    }

    protected void btnSelectCustomer_Click(object sender, EventArgs e)
    {
        txtProject.Text = string.Empty;

        hdnProjectId.Value = string.Empty;

        FillLoc();
        IntializeData();
        //Set Hyperlink  For Project 


        lnkProjectID.NavigateUrl = "javascript:void(0);"; //lnkProjectID.Visible = false;
        iProIcon.Attributes.Add("style", "color:#5815c02b !important;");

    }

    protected void btnSelectLoc_Click(object sender, EventArgs e)
    {
        txtProject.Text = string.Empty;

        hdnProjectId.Value = string.Empty;

        FillLocInfo();

        GetDataProject();

        FillInvoiceProjectInfo();

        //Set Hyperlink  For Project 
        SetDefaultSalesPerson();

        lnkProjectID.NavigateUrl = "javascript:void(0);"; //lnkProjectID.Visible = false;
        iProIcon.Attributes.Add("style", "color:#5815c02b !important;");

        IntializeData();
        RadGrid_gvInvoices.Rebind();
        //todo
        // BindGridWithTax();
        FillProjectCodes(false, out int GLAcct);
        AddNew(0);
        ViewState["ResetGrid"] = 1;
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["InvoiceSrch"];
            Response.Redirect("addinvoice.aspx?uid=" + dt.Rows[0]["ref"]);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr56222", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["InvoiceSrch"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ref"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
            int index = dt.Rows.IndexOf(d);

            if (index > 0)
            {
                Response.Redirect("addinvoice.aspx?uid=" + dt.Rows[index - 1]["ref"]);
            }
            if (index == 0)
            {
                Response.Redirect("addinvoice.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["ref"]);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr5899", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["InvoiceSrch"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ref"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;
            if (index < c)
            {
                Response.Redirect("addinvoice.aspx?uid=" + dt.Rows[index + 1]["ref"]);
            }
            if (index == c)
            {
                Response.Redirect("addinvoice.aspx?uid=" + dt.Rows[0]["ref"]);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr333", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkLast_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["InvoiceSrch"];
        Response.Redirect("addinvoice.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["ref"]);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Boolean isProjectClose = false;
        try
        {
            foreach (GridDataItem gr in RadGrid_gvInvoices.Items)
            {
                TextBox lblQuantity = (TextBox)gr.FindControl("lblQuantity");
                HiddenField hdnstatus = (HiddenField)gr.FindControl("hdnStatus");
                HiddenField hdnAStatus = (HiddenField)gr.FindControl("hdnAStatus");
                if (hdnAStatus.Value == "1" || hdnstatus.Value == "1")
                {

                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningAcctMessage", "warningAcctMessage();", true);
                    return;
                }
            }
            if (txtProject.Text != string.Empty)
            {
                foreach (GridDataItem gr in RadGrid_gvInvoices.Items)
                {
                    DropDownList ddlProjectCode = (DropDownList)gr.FindControl("ddlProjectCode");
                    // DropDownList ddlBillingCode = (DropDownList)gr.FindControl("ddlBillingCode");
                    HiddenField txtBillingCodeID = (HiddenField)gr.FindControl("txtBCodeID");
                    TextBox lblQuantity = (TextBox)gr.FindControl("lblQuantity");
                    if (ddlProjectCode.SelectedItem.Text == "-Select-" && txtBillingCodeID.Value != string.Empty && lblQuantity.Text != string.Empty && lblQuantity.Text != "0")
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Please select the Code.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                        return;
                    }

                }

                JobT objJob = new JobT();
                objJob.ConnConfig = Session["config"].ToString();
                objJob.ID = Convert.ToInt32(hdnProjectId.Value);

                DataSet dsJ = objBL_Job.GetJobById(objJob);
                if (dsJ != null && dsJ.Tables.Count > 0)
                {
                    if (dsJ.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dsJ.Tables[0].Rows[0]["Status"]) == 1)
                        {
                            isProjectClose = true;
                        }
                    }

                }
            }
            //if (Convert.ToInt32(ViewState["mode"]) != 1)
            //{
            //    GetPeriodDetails(Convert.ToDateTime(txtInvoiceDate.Text));
            //}
            //if (!(bool)ViewState["FlagPeriodClose"])
            //{
            if (isProjectClose)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Please note this project is closed. You will need to change it before saving.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
            else
            {
                Submit(0);
                if (Request.QueryString["o"] != null)
                {
                    if (success == true)
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "keyClose", "setTimeout('window.close();', 3000);", true);
                    }
                }
            }

            //}
            //Response.Redirect(Request.RawUrl);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr4545", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private bool Validate(DataTable dt)
    {
        try
        {
            DataRow[] rows = dt.Select("Acct = 0 or Acct is null ");
            if (rows.Count() > 0)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Please select billing code.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                return false;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr8999", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return true;
    }

    private void Submit(int isPreview)
    {

        //if (Convert.ToInt32(ViewState["mode"]) != 1)
        //{
        GetPeriodDetails(Convert.ToDateTime(txtInvoiceDate.Text));
        //}
        if ((bool)ViewState["FlagPeriodClose"])
        {

            GridData(0, true);
            DataTable dtInvoice1 = new DataTable();
            dtInvoice1 = (DataTable)ViewState["invoicetable"];

            if (dtInvoice1.Columns.Contains("InvStatus"))
            {
                dtInvoice1.Columns.Remove("InvStatus");
            }
            if (dtInvoice1.Columns.Contains("AStatus"))
            {
                dtInvoice1.Columns.Remove("AStatus");
            }

            CalculateTotals(dtInvoice1);
            DataTable dtInvoiceCopy = dtInvoice1.Copy();
            DataTable dtInvoiceTrans = dtInvoice1.Copy();
            dtInvoiceCopy.Columns.Remove("priceQuant");

            dtInvoiceCopy.Columns.Remove("billcode");
            //todo
            if (!Convert.ToBoolean(ViewState["isCanada"]))
            {
                if (dtInvoiceCopy.Columns.Contains("GTaxAmt"))
                {
                    dtInvoiceCopy.Columns.Remove("GTaxAmt");
                }
                if (dtInvoiceCopy.Columns.Contains("EnableGSTTax"))
                {
                    dtInvoiceCopy.Columns.Remove("EnableGSTTax");
                }
            }

            if (dtInvoiceCopy.Columns.Contains("TotalTax"))
            {
                dtInvoiceCopy.Columns.Remove("TotalTax");
            }
            if (dtInvoiceCopy.Columns.Contains("ProgressBillingNo"))
            {
                dtInvoiceCopy.Columns.Remove("ProgressBillingNo");
            }
            try
            {
                double TotalPretaxAmount = 0;
                double TotalSalesTaxrate = 0;
                double PricePerTotal = 0;
                if (dtInvoice1.Rows.Count > 0)
                {
                    PricePerTotal = Math.Round(Convert.ToDouble(dtInvoice1.Compute("Sum(Price)", "")), 2, MidpointRounding.AwayFromZero);
                    TotalPretaxAmount = Math.Round(Convert.ToDouble(dtInvoice1.Compute("Sum(priceQuant)", "")), 2, MidpointRounding.AwayFromZero);
                    TotalSalesTaxrate = Math.Round(Convert.ToDouble(dtInvoice1.Compute("Sum(STaxAmt)", "")), 2, MidpointRounding.AwayFromZero);
                }

                if (Validate(dtInvoiceCopy))
                {
                    objProp_Contracts.Total = PricePerTotal;
                    objProp_Contracts.Amount = TotalPretaxAmount;
                    objProp_Contracts.Staxtotal = TotalSalesTaxrate;
                    if (hdnStax.Value != string.Empty)
                    {
                        objProp_Contracts.Taxrate = Convert.ToDouble(hdnStax.Value);
                    }
                    else
                    {
                        objProp_Contracts.Taxrate = 0;
                    }

                    objProp_Contracts.ConnConfig = Session["config"].ToString();
                    //to add extra column JobOrg 

                    if (dtInvoiceCopy.Columns.Contains("JobOrg"))
                    {

                    }
                    else
                    {
                        dtInvoiceCopy.Columns.Add("JobOrg");
                    }
                    dtInvoiceCopy.Columns["JobOrg"].SetOrdinal(15);
                    if (dtInvoiceCopy.Columns.Contains("GTaxAmt"))
                    {
                        dtInvoiceCopy.Columns["GTaxAmt"].SetOrdinal(14);
                    }


                    //Reorder column

                    dtInvoiceCopy.Columns["Ref"].SetOrdinal(0);
                    dtInvoiceCopy.Columns["Line"].SetOrdinal(1);
                    dtInvoiceCopy.Columns["Acct"].SetOrdinal(2);
                    dtInvoiceCopy.Columns["Quan"].SetOrdinal(3);
                    dtInvoiceCopy.Columns["fDesc"].SetOrdinal(4);
                    dtInvoiceCopy.Columns["Price"].SetOrdinal(5);
                    dtInvoiceCopy.Columns["Amount"].SetOrdinal(6);
                    dtInvoiceCopy.Columns["STax"].SetOrdinal(7);
                    dtInvoiceCopy.Columns["Job"].SetOrdinal(8);
                    dtInvoiceCopy.Columns["JobItem"].SetOrdinal(9);
                    dtInvoiceCopy.Columns["TransID"].SetOrdinal(10);
                    dtInvoiceCopy.Columns["Measure"].SetOrdinal(11);
                    dtInvoiceCopy.Columns["Disc"].SetOrdinal(12);
                    dtInvoiceCopy.Columns["StaxAmt"].SetOrdinal(13);
                    if (dtInvoiceCopy.Columns.Contains("GTaxAmt"))
                    {

                        dtInvoiceCopy.Columns["GTaxAmt"].SetOrdinal(14);
                        dtInvoiceCopy.Columns["Code"].SetOrdinal(15);
                        dtInvoiceCopy.Columns["JobOrg"].SetOrdinal(16);
                        dtInvoiceCopy.Columns["INVType"].SetOrdinal(17);
                        dtInvoiceCopy.Columns["Warehouse"].SetOrdinal(18);
                        dtInvoiceCopy.Columns["WHLocID"].SetOrdinal(19);
                        dtInvoiceCopy.Columns["EnableGSTTax"].SetOrdinal(20);
                    }
                    else
                    {

                        //dtInvoiceCopy.Columns["GSTAmt"].SetOrdinal(14);
                        dtInvoiceCopy.Columns["Code"].SetOrdinal(14);
                        dtInvoiceCopy.Columns["JobOrg"].SetOrdinal(15);
                        dtInvoiceCopy.Columns["INVType"].SetOrdinal(16);
                        dtInvoiceCopy.Columns["Warehouse"].SetOrdinal(17);
                        dtInvoiceCopy.Columns["WHLocID"].SetOrdinal(18);
                        //dtInvoiceCopy.Columns["EnableGSTTax"].SetOrdinal(19);
                    }




                    //todo                   

                    objProp_Contracts.DtRecContr = dtInvoiceCopy;

                    objProp_Contracts.Date = Convert.ToDateTime(txtInvoiceDate.Text);
                    objProp_Contracts.Remarks = txtRemarks.Text;
                    objProp_Contracts.TaxRegion = hdnTaxRegion.Value;
                    objProp_Contracts.Taxfactor = 100;
                    objProp_Contracts.Taxable = taxable;    //change by Mayuri on 28th feb, 17
                    if (ddlDepartment.SelectedValue != "")
                    {
                        objProp_Contracts.Type = Convert.ToInt32(ddlDepartment.SelectedValue);
                    }
                    else
                    {
                        try
                        {
                            var otherSelectValue = ddlDepartment.Items.FindByText("Other");
                            if (otherSelectValue != null)
                            {
                                objProp_Contracts.Type = Convert.ToInt32(otherSelectValue.Value);
                            }
                            else
                            {
                                objProp_Contracts.Type = -1;
                            }
                        }
                        catch
                        {
                            objProp_Contracts.Type = -1;
                        }

                    }
                    objProp_Contracts.JobId = 0;
                    if (hdnProjectId.Value != "")
                    {
                        objProp_Contracts.JobId = Convert.ToInt32(hdnProjectId.Value);
                    }
                    objProp_Contracts.Loc = Convert.ToInt32(hdnLocId.Value);
                    objProp_Contracts.Terms = Convert.ToInt32(ddlTerms.SelectedValue);
                    objProp_Contracts.PO = txtPO.Text;
                    objProp_Contracts.Status = Convert.ToInt32(ddlStatus.SelectedValue);
                    objProp_Contracts.Batch = 0;
                    objProp_Contracts.Remarks = txtRemarks.Text;
                    objProp_Contracts.Gtax = 0.00;
                    objProp_Contracts.Mech = Convert.ToInt32(ddlRoute.SelectedValue);
                    objProp_Contracts.Taxrate2 = 0;
                    objProp_Contracts.TaxRegion2 = string.Empty;
                    objProp_Contracts.BillTo = txtAddress.Text;
                    objProp_Contracts.Idate = string.IsNullOrEmpty(txtInvoiceDate.Text) ? DateTime.MinValue : Convert.ToDateTime(txtInvoiceDate.Text); //Convert.ToDateTime(txtInvoiceDate.Text); //txtInvoiceDate.Text
                    objProp_Contracts.DueDate = string.IsNullOrEmpty(txtDueDate.Text) ? (Nullable<DateTime>)null : Convert.ToDateTime(txtDueDate.Text);

                    objProp_Contracts.Fuser = Session["Username"].ToString();
                    objProp_Contracts.StaxI = 1;
                    objProp_Contracts.InvoiceIDCustom = txtInvoiceNo.Text;
                    objProp_Contracts.TaxType = Convert.ToInt32(hdnsTaxType.Value);
                    if (Request.QueryString["tickid"] != null)
                    {
                        if (Request.QueryString["tickid"] != string.Empty)
                        {
                            //objProp_Contracts.TicketID = Convert.ToInt32(Request.QueryString["tickid"]);
                            objProp_Contracts.Tickets = ViewState["othertickets"].ToString();
                        }
                    }
                    objProp_Contracts.AssignedTo = int.Parse(ddlsaleperson.SelectedValue.ToString());
                    if (Convert.ToInt32(ViewState["mode"]) == 1)
                    {
                        String page = "";
                        String redirect = "";
                        if (Request.QueryString["page"] != null)
                        {
                            redirect = "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl);
                            page = "&page=" + Request.QueryString["page"].ToString();
                        }



                        objProp_Contracts.InvoiceID = Convert.ToInt32(Request.QueryString["uid"]);
                        //todo
                        if (Convert.ToBoolean(ViewState["isCanada"]))
                        {
                            objBL_Contracts.EditInvoice(objProp_Contracts);
                        }
                        else
                        {
                            objBL_Contracts.UpdateInvoice(objProp_Contracts);
                        }


                        #region Voided Invoice
                        if (ddlStatus.SelectedValue == "2")
                        {
                            objProp_Contracts.ConnConfig = Session["config"].ToString();
                            objProp_Contracts.Ref = Convert.ToInt32(Request.QueryString["uid"]);
                            objProp_Contracts.Date = DateTime.Now;
                            objProp_Contracts.Fuser = Session["username"].ToString();
                            objBL_Contracts.UpdateVoidInvoiceDetails(objProp_Contracts);
                            Response.Redirect("addinvoice.aspx?uid=" + Request.QueryString["uid"] + page);
                        }
                        #endregion
                        Session["UpdateInvoiceSuccess"] = true;
                        //if (IsPreview == true)
                        if (isPreview == 0)
                        {
                            String strLocID = "";
                            String strpid = "";

                            if (Request.QueryString["page"] != null)
                            {
                                if (Request.QueryString["lid"] != null)
                                {
                                    strLocID = "&lid=" + Request.QueryString["lid"].ToString();
                                }
                                else if (Request.QueryString["pid"] != null)
                                {
                                    strpid = "&pid=" + Request.QueryString["pid"].ToString();
                                }
                            }
                            Response.Redirect("addinvoice.aspx?uid=" + Request.QueryString["uid"] + page + strLocID + strpid);
                        }
                        if (isPreview == 1)
                        {
                            // Response.Redirect("PreviewInvoice.aspx?uid=" + Convert.ToString(objProp_Contracts.InvoiceID));
                            Session["UpdateInvoiceSuccess"] = null;
                            var reportFormat = ConfigurationManager.AppSettings["InvoiceReportFormat"].ToString();
                            if (reportFormat.ToUpper().Equals("MRT"))
                                Response.Redirect("PreviewInvoice.aspx?uid=" + Convert.ToString(objProp_Contracts.InvoiceID) + redirect);
                            else if (reportFormat.ToUpper().Equals("RDLC"))
                                Response.Redirect("PrintInvoice.aspx?uid=" + Convert.ToString(objProp_Contracts.InvoiceID) + redirect);
                        }
                        if (isPreview == 2)
                        {
                            Session["UpdateInvoiceSuccess"] = null;

                            var reportFormat = ConfigurationManager.AppSettings["InvoiceReportFormat"].ToString();

                            if (reportFormat.ToUpper().Equals("MRT"))
                            {
                                Response.Redirect("PreviewInvoiceWithTicket.aspx?uid=" + Convert.ToString(objProp_Contracts.InvoiceID) + redirect);
                            }
                            else if (reportFormat.ToUpper().Equals("RDLC"))
                            {
                                Response.Redirect("PrintInvoiceWithTicket.aspx?uid=" + Convert.ToString(objProp_Contracts.InvoiceID) + page);
                            }
                        }
                    }
                    else
                    {
                        //todo

                        objProp_Contracts.IsRecurring = 0;


                        if (chkrecurring.Checked) objProp_Contracts.IsRecurring = 1;


                        if (Convert.ToBoolean(ViewState["isCanada"]))
                        {
                            objProp_Contracts.InvoiceID = objBL_Contracts.AddInvoice(objProp_Contracts);
                        }
                        else
                        {
                            objProp_Contracts.InvoiceID = objBL_Contracts.CreateInvoice(objProp_Contracts);
                        }


                        ResetFormControlValues(this);

                        // Remove log tab and edit title's page   
                        liLogs.Visible = false;
                        tbLogs.Visible = false;
                        liEmailLogs.Visible = false;
                        tbEmailLogs.Visible = false;
                        lblHeader.Text = "Add Invoice";

                        if (isPreview == 1)
                        {

                            var reportFormat = ConfigurationManager.AppSettings["InvoiceReportFormat"].ToString();
                            if (reportFormat.ToUpper().Equals("MRT"))
                                Response.Redirect("PreviewInvoice.aspx?uid=" + Convert.ToString(objProp_Contracts.InvoiceID));
                            else if (reportFormat.ToUpper().Equals("RDLC"))
                                Response.Redirect("PrintInvoice.aspx?uid=" + Convert.ToString(objProp_Contracts.InvoiceID));
                        }
                        if (isPreview == 2)
                        {
                            Session["UpdateInvoiceSuccess"] = null;
                            Response.Redirect("PreviewInvoiceWithTicket.aspx?uid=" + Convert.ToString(objProp_Contracts.InvoiceID));
                        }


                        if (Request.QueryString["tickid"] != null)
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateparent", "if(window.opener && !window.opener.closed) { if(window.opener.document.getElementById('lnkRefressTicketList')) window.opener.document.getElementById('lnkRefressTicketList').click();setTimeout('window.close();',3000);}", true);
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateparentmsg", "noty({text: 'Invoice Created Successfully! </br> <b> Invoice# : " + objProp_Contracts.InvoiceID.ToString() + "</b>', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                        }
                        else
                        {
                            Session["AddInvoiceSuccess"] = "Invoice Created Successfully! </br> <b> Invoice# :" + objProp_Contracts.InvoiceID.ToString();
                            String page = "";
                            String strLocID = "";
                            String strpid = "";
                            if (Request.QueryString["page"] != null)
                            {
                                page = "?page=" + Request.QueryString["page"].ToString();
                                if (Request.QueryString["lid"] != null)
                                {
                                    strLocID = "&lid=" + Request.QueryString["lid"].ToString();
                                }
                                else if (Request.QueryString["pid"] != null)
                                {
                                    strpid = "&pid=" + Request.QueryString["pid"].ToString();
                                }
                            }
                            Response.Redirect("addinvoice.aspx" + page + strLocID + strpid);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                string errortype = "error";

                if (str.Contains("You do not have enough on hand for item") || str.Contains("This location is inactive. Please change the location name before proceeding."))
                {
                    errortype = "warning";
                }
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "LocInactive", "noty({text: '" + str + "',  type : '" + errortype + "', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

                success = false;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'These month/year period is closed out. You do not have permission to add/update this record. ', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);

            btnSubmit.Visible = true;
            divSuccess.Visible = true;
            divProjectClose.Visible = false;
            // set success =true , to print if the period is closed out 
            //if (Request.QueryString["uid"].ToString() != null)
            {
                success = true;
            }
        }
    }

    private void ResetFormControlValues(Control parent)
    {
        foreach (Control c in parent.Controls)
        {
            if (c.Controls.Count > 0)
            {
                ResetFormControlValues(c);
            }
            else
            {
                switch (c.GetType().ToString())
                {
                    case "System.Web.UI.WebControls.DropDownList":
                        ((DropDownList)c).SelectedIndex = -1;
                        break;
                    case "System.Web.UI.WebControls.TextBox":
                        ((TextBox)c).Text = "";
                        break;
                    case "System.Web.UI.WebControls.CheckBox":
                        ((CheckBox)c).Checked = false;
                        break;
                    case "System.Web.UI.WebControls.RadioButton":
                        ((RadioButton)c).Checked = false;
                        break;
                    case "System.Web.UI.WebControls.HiddenField":
                        ((HiddenField)c).Value = "";
                        break;
                }
            }
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString() == "Collection")
            {
                Response.Redirect("iCollections.aspx");
            }
            else
            {
                if (Request.QueryString["page"].ToString() == "AccountLedger")
                {
                    Response.Redirect("AccountLedger?id=" + Session["alId"]);
                }
                else
                {
                    if (Request.QueryString["lid"] != null)
                    {
                        Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + Request.QueryString["lid"].ToString() + "&tab=inv");
                    }
                    else if (Request.QueryString["pid"] != null)
                    {
                        Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + Request.QueryString["pid"].ToString() + "&tab=budget");
                    }
                }

            }
        }
        else
        {
            Response.Redirect("invoices.aspx?fil=1");
        }
    }

    protected void lnkCustSave_Click(object sender, EventArgs e)
    {
        //DataTable dt = (DataTable)ViewState["invoicetable"];

        //DataRow dr = dt.NewRow();

        //dr["Ref"] = 0;
        //dr["line"] = dt.Rows.Count;
        //dr["Acct"] = ddlBillingCode.SelectedValue;
        //dr["Quan"] =Convert.ToDouble( txtQuantity.Text);
        //dr["fDesc"] = txtRemarks0.Text;
        //dr["PriceQuant"] = Convert.ToDouble(txtQuantity.Text) * Convert.ToDouble(txtpricePer.Text);
        //dr["Amount"] = Convert.ToDouble(txtQuantity.Text) * Convert.ToDouble(txtpricePer.Text) * Convert.ToDouble(txtSalesTax.Text);
        //dr["STax"] =Convert.ToDouble(txtSalesTax.Text);
        //dr["Job"] = 0;
        //dr["JobItem"] = 0;
        //dr["TransID"] = 0;
        //dr["Measure"] = string.Empty;
        //dr["Disc"] = 0;
        //dr["Price"] = Convert.ToDouble(txtpricePer.Text);
        //dr["billcode"] = ddlBillingCode.SelectedItem.Text;

        //if (ViewState["editcon"].ToString() == "1")
        //{
        //    dt.Rows.RemoveAt(Convert.ToInt32(ViewState["index"]));
        //    dt.Rows.InsertAt(dr, Convert.ToInt32(ViewState["index"]));
        //    ViewState["editcon"] = 0;
        //}
        //else
        //{
        //    dt.Rows.Add(dr);
        //}

        //dt.AcceptChanges();

        //ViewState["invoicetable"] = dt;

        //BindGrid();

        ////ClearContact();
        ////TogglePopup();

        ////if (ViewState["mode"].ToString() == "1")
        ////{
        ////SubmitContact();
        ////}
    }

    private void ClearDetails()
    {
        //txtQuantity.Text = string.Empty;
        //ddlBillingCode.SelectedIndex = 0;
        //txtRemarks0.Text = string.Empty;
        //txtpricePer.Text = string.Empty;
        //chkTaxable.Checked = false;
        //txtPretax.Text = string.Empty;
        //txtpricePer.Text = string.Empty;
        //txtAmount.Text = string.Empty;
        //txtSalesTax.Text = string.Empty;
    }

    private void BindGrid()
    {
        try
        {
            DataTable dt = new DataTable();

            dt = (DataTable)ViewState["invoicetable"];

            try
            {
                ///IF Code is Zero then Set Top 1 Default code 

                if (Request.QueryString["tickid"] != null)
                {
                    dtProjectCodeData = (DataTable)ViewState["dtProjectCodeData"];

                    if (dtProjectCodeData.Rows.Count > 1)
                    {
                        int CodeValue = 0;
                        int.TryParse(Convert.ToString(dtProjectCodeData.Rows[1]["Line"]), out CodeValue);
                        foreach (DataRow dr in dt.Rows)
                        {
                            dr["code"] = dr["code"].ToString() == "0" ? CodeValue : dr["code"];
                        }
                    }
                }
            }
            catch { }

            RadGrid_gvInvoices.DataSource = dt;

            objGenerals.ConnConfig = Session["config"].ToString();

            objGenerals.CustomName = "Country";

            DataSet dsCustom = objBL_General.getCustomFields(objGenerals);

            if (dsCustom.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dsCustom.Tables[0].Rows[0]["Label"].ToString()) && dsCustom.Tables[0].Rows[0]["Label"].ToString().Equals("1"))
                {
                    RadGrid_gvInvoices.Columns.FindByUniqueName("STaxAmt").HeaderText = "Provincial Tax";
                    RadGrid_gvInvoices.Columns.FindByUniqueName("GTaxAmt").Visible = true;

                    //RadGrid_gvInvoices.Columns[9].HeaderText = "Provincial Tax";
                    //RadGrid_gvInvoices.Columns[10].Visible = true;
                    hdnGstTax.Value = dsCustom.Tables[0].Rows[0]["GstRate"].ToString();
                }
                else
                {
                    RadGrid_gvInvoices.Columns.FindByUniqueName("GTaxAmt").Visible = false;
                    RadGrid_gvInvoices.Columns.FindByUniqueName("STaxAmt").HeaderText = "Sales Tax Amount";

                    //RadGrid_gvInvoices.Columns[10].Visible = false;
                    //RadGrid_gvInvoices.Columns[9].HeaderText = "Sales Tax Amount";
                }
            }
            else
            {
                RadGrid_gvInvoices.Columns.FindByUniqueName("GTaxAmt").Visible = false;
                RadGrid_gvInvoices.Columns.FindByUniqueName("STaxAmt").HeaderText = "Sales Tax Amount";
                //RadGrid_gvInvoices.Columns[10].Visible = false;
                //RadGrid_gvInvoices.Columns[9].HeaderText = "Sales Tax Amount";
            }


            //todo
            // BindGridWithTax();

            //RadGrid_gvInvoices.Rebind();

            CalculateTotals(dt);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr11", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void CalculateTotals(DataTable dt)
    {
        try
        {
            if (dt.Rows.Count > 0)
            {
                if (RadGrid_gvInvoices.MasterTableView.GetItems(GridItemType.Footer).Count() > 0)
                {
                    GridFooterItem footerItem = (GridFooterItem)RadGrid_gvInvoices.MasterTableView.GetItems(GridItemType.Footer)[0];
                    Label lblPricePerTotal = (Label)footerItem.FindControl("lblPricePerTotal");
                    Label lblTotalPretaxAmt = (Label)footerItem.FindControl("lblPretaxAmountTotal");
                    Label lblTotalSalesTax = (Label)footerItem.FindControl("lblSalesTaxTotal");
                    Label lblTotalInvoice = (Label)footerItem.FindControl("lblAmountTotal");
                    Label lblTotalGstTax = (Label)footerItem.FindControl("lblGstTaxTotal");

                    double TotalPricePer = 0;
                    double TotalPretaxAmt = 0;
                    double TotalSalesTax = 0;
                    double TotalInvoice = 0;
                    double TotalGstTax = 0;

                    double PricePer = 0;
                    double PretaxAmt = 0;
                    double SalesTax = 0;
                    double Invoice = 0;
                    double GstTax = 0;

                    foreach (DataRow dr in dt.Rows)
                    {


                        if (dr["Price"] != DBNull.Value)
                        {
                            PricePer = Math.Round(Convert.ToDouble(dr["Price"]) * 100) / 100;
                            TotalPricePer += PricePer;
                        }

                        if (dr["PriceQuant"] != DBNull.Value)
                        {
                            PretaxAmt = Math.Round(Convert.ToDouble(dr["PriceQuant"]) * 100) / 100;
                            TotalPretaxAmt += PretaxAmt;
                        }

                        if (dr["staxAmt"] != DBNull.Value)
                        {
                            SalesTax = Math.Round(Convert.ToDouble(dr["staxAmt"]) * 100) / 100;
                            TotalSalesTax += SalesTax;
                        }

                        if (dr["amount"] != DBNull.Value)
                        {
                            Invoice = Math.Round(Convert.ToDouble(dr["amount"]) * 100) / 100;
                            TotalInvoice += Invoice;
                        }
                        if (dt.Columns.Contains("GTaxAmt"))
                        {
                            if (dr["GTaxAmt"] != DBNull.Value)
                            {
                                if (!string.IsNullOrEmpty(dr["GTaxAmt"].ToString()))
                                {
                                    GstTax = Math.Round(Convert.ToDouble(dr["GTaxAmt"]) * 100) / 100;
                                    TotalGstTax += GstTax;
                                }
                            }
                        }
                    }
                    lblPricePerTotal.Text = (Math.Round(TotalPricePer * 100) / 100).ToString("N", CultureInfo.InvariantCulture);
                    lblTotalPretaxAmt.Text = (Math.Round(TotalPretaxAmt * 100) / 100).ToString("N", CultureInfo.InvariantCulture);
                    lblTotalSalesTax.Text = (Math.Round(TotalSalesTax * 100) / 100).ToString("N", CultureInfo.InvariantCulture);
                    lblTotalInvoice.Text = (Math.Round(TotalInvoice * 100) / 100).ToString("N", CultureInfo.InvariantCulture);
                    lblTotalGstTax.Text = (Math.Round(TotalGstTax * 100) / 100).ToString("N", CultureInfo.InvariantCulture);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr15", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkCancelCust_Click(object sender, EventArgs e)
    {
        //programmaticModalPopup.Hide();
    }

    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        ViewState["ResetGrid"] = 1;
        FillProjectCodes(false, out int GLAcct);
        AddNew(0);
    }

    private void AddNew(int resetJobCode)
    {
        DataTable dtBilling = new DataTable();
        GridData(resetJobCode);
        bool check = true;
        DataTable dt = new DataTable();
        dtBilling = (DataTable)ViewState["dtBillingCodeData"];
        dt = (DataTable)ViewState["invoicetable"];
        if (RadGrid_gvInvoices.Items.Count > 0)
        {
            GridDataItem lastRow = RadGrid_gvInvoices.Items[RadGrid_gvInvoices.Items.Count - 1];
            TextBox txtBillingCode = (TextBox)lastRow.FindControl("txtBillingCode");
            TextBox lblPricePer = (TextBox)lastRow.FindControl("lblPricePer");
            TextBox lblQuantity = (TextBox)lastRow.FindControl("lblQuantity");

            if (String.IsNullOrEmpty(txtBillingCode.Text) || String.IsNullOrEmpty(lblPricePer.Text) || String.IsNullOrEmpty(lblQuantity.Text))
            {
                check = false;
            }
        }

        if (check)
        {

            DataRow dr = dt.NewRow();

            //dr["Quan"] = 1;
            dr["line"] = dt.Rows.Count;
            dr["stax"] = 0;
            dr["code"] = 0;
            if (Convert.ToString(ViewState["DefaultAcct"]) != "" && Convert.ToString(ViewState["DefaultAcct"]) != "0")
            {
                dr["Acct"] = Convert.ToString(ViewState["DefaultAcct"]);
            }
            if (hdnProjectId.Value != "0" && hdnProjectId.Value != "" && dtBilling != null)
            {
                if (dtBilling.Rows.Count > 1)
                {
                    dr["billcode"] = dtBilling.Rows[1]["BillType"].ToString();
                    dr["InvStatus"] = Convert.ToString(dtBilling.Rows[1]["statusid"]);
                    dr["AStatus"] = dtBilling.Rows[1]["Status"].ToString().ToLower() == "active" ? "0" : "1";
                    dr["INVType"] = Convert.ToInt32(dtBilling.Rows[1]["Type"]);
                }

            }
            dt.Rows.Add(dr);

            ViewState["invoicetable"] = dt;

            BindGrid();
        }
        else
        {
            ViewState["ResetGrid"] = 0;
        }

    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["ResetGrid"] = 1;
            GridData(0);
            FillProjectCodes(false, out int GLAcct);
            DataTable dt = (DataTable)ViewState["invoicetable"];
            int count = 0;
            for (int i = 0; i < RadGrid_gvInvoices.Items.Count; i++)
            {
                CheckBox chkSelect = (CheckBox)RadGrid_gvInvoices.Items[i].FindControl("chkSelect");
                Label lblindex = (Label)RadGrid_gvInvoices.Items[i].FindControl("lblindex");

                if (chkSelect.Checked == true)
                {
                    //if (count == 0)
                    //{
                    // dt.Rows.RemoveAt(Convert.ToInt32(lblindex.Text) - 1);
                    //    count = 1;
                    //}


                    int index = Convert.ToInt32(lblindex.Text) - 1;
                    dt.Rows.RemoveAt(index - count);
                    count++;
                    dt.AcceptChanges();

                }
            }
            dt.AcceptChanges();
            if (dt.Rows.Count == 0)
            {
                // If Row is Empty
                DataRow dr = dt.NewRow();
                dr["Quan"] = 1;
                dr["line"] = dt.Rows.Count;
                dr["stax"] = 0;
                dr["code"] = 0;
                dt.Rows.Add(dr);
            }
            ViewState["invoicetable"] = dt;
            BindGrid();
            hdnFocus.Value = string.Empty;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr45", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        //foreach (GridViewRow di in gvInvoices.Rows)
        //{
        //    DataTable dt = (DataTable)ViewState["invoicetable"];
        //    CheckBox chkSelect = (CheckBox)di.FindControl("chkSelect");
        //    Label lblindex = (Label)di.Cells[1].FindControl("lblindex");

        //    if (chkSelect.Checked== true)
        //    {
        //        programmaticModalPopup.Show();

        //        DataRow dr = dt.Rows[Convert.ToInt32(lblindex.Text)];

        //        txtQuantity.Text = dr["Quan"].ToString();
        //        ddlBillingCode.SelectedValue = dr["Acct"].ToString();
        //        txtRemarks0.Text = dr["fDesc"].ToString();
        //        txtpricePer.Text = dr["Price"].ToString();
        //        //chkTaxable.Checked= dr["Email"].ToString();
        //        txtPretax.Text = dr["PriceQuant"].ToString();
        //        txtSalesTax.Text = dr["STax"].ToString();
        //        txtAmount.Text = dr["Amount"].ToString();
        //        ViewState["editcon"] = 1;
        //        ViewState["index"] = lblindex.Text;
        //    }
        //}
    }

    protected void lnkPrint_Click(object sender, EventArgs e)
    {
        //btnSubmit_Click(sender, e);
        try
        {
            Session["InvoiceName"] = "Invoice";
            Submit(0);
            string eventTarget = this.Request.Params.Get("__EVENTTARGET");
            if (eventTarget.Contains("lnk_InvoiceAdamMaintenance"))
            {
                Session["InvoiceName"] = "AdamMaintenance";
            }
            if (eventTarget.Contains("lnk_InvoiceAdamBilling"))
            {
                Session["InvoiceName"] = "AdamBilling";
            }

            if (success == true)
            {
                //reassign invoice id 
                var reportFormat = ConfigurationManager.AppSettings["InvoiceReportFormat"].ToString();
                if (Request.QueryString["uid"] == null)
                {
                    if (reportFormat.ToUpper().Equals("MRT"))
                        Response.Redirect("PreviewInvoice.aspx?uid=" + objProp_Contracts.InvoiceID.ToString());
                    else if (reportFormat.ToUpper().Equals("RDLC"))
                        Response.Redirect("PrintInvoice.aspx?uid=" + objProp_Contracts.InvoiceID.ToString());

                }
                else
                {
                    objProp_Contracts.InvoiceID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                    if (reportFormat.ToUpper().Equals("MRT"))
                        Response.Redirect("PreviewInvoice.aspx?uid=" + objProp_Contracts.InvoiceID.ToString());
                    else if (reportFormat.ToUpper().Equals("RDLC"))
                        Response.Redirect("PrintInvoice.aspx?uid=" + objProp_Contracts.InvoiceID.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr69", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private bool CheckValidGridRow(string lblQuantity, string lblPricePer, bool IsbtnSaveClick = false)
    {
        if (IsbtnSaveClick)
        {
            if (lblQuantity.Trim() != string.Empty && lblPricePer.Trim() != string.Empty && lblQuantity.Trim() != "0")   //&& ddlProjectCode.SelectedItem.Text !="-Select-")  
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    private void GridData(int resetJobCode, bool IsbtnSaveClick = false)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["invoicetable"];

            DataTable dtDetails = dt.Clone();
            taxable = 0;
            foreach (GridDataItem gr in RadGrid_gvInvoices.Items)
            {
                Label lblIndex = (Label)gr.FindControl("lblIndex");
                TextBox lblQuantity = (TextBox)gr.FindControl("lblQuantity");

                TextBox lblPricePer = (TextBox)gr.FindControl("lblPricePer");
                TextBox lblSalesTax = (TextBox)gr.FindControl("lblSalesTax");
                TextBox lblGstTax = (TextBox)gr.FindControl("lblGstTax");
                HiddenField hdnSalesTax = (HiddenField)gr.FindControl("hdnSalesTax");
                TextBox lblDescription = (TextBox)gr.FindControl("lblDescription");

                TextBox txtBillingCode = (TextBox)gr.FindControl("txtBillingCode");
                HiddenField txtBillingCodeID = (HiddenField)gr.FindControl("txtBCodeID");
                HiddenField hdnStatus = (HiddenField)gr.FindControl("hdnStatus");
                HiddenField hdnAStatus = (HiddenField)gr.FindControl("hdnAStatus");
                HiddenField hdnIndex = (HiddenField)gr.FindControl("hdnIndex");

                CheckBox chkTaxable = (CheckBox)gr.FindControl("chkTaxable");
                DropDownList ddlProjectCode = (DropDownList)gr.FindControl("ddlProjectCode");
                CheckBox chkEnableGSTTax = (CheckBox)gr.FindControl("chkEnableGSTTax");



                if (CheckValidGridRow(lblQuantity.Text.Trim(), lblPricePer.Text.Trim(), IsbtnSaveClick))
                {
                    DataRow dr = dtDetails.NewRow();

                    dr["Ref"] = 0;
                    // dr["line"] = lblIndex.Text;
                    dr["line"] = hdnIndex.Value;
                    if (txtBillingCodeID.Value != "")
                    {
                        dr["Acct"] = txtBillingCodeID.Value;
                    }
                    if (lblQuantity.Text != "")
                    {
                        dr["Quan"] = Convert.ToDouble(lblQuantity.Text);
                    }

                    dr["fDesc"] = lblDescription.Text;
                    if (lblQuantity.Text != "" && lblPricePer.Text != "")
                    {
                        dr["PriceQuant"] = Math.Round(Convert.ToDouble(lblQuantity.Text) * Convert.ToDouble(lblPricePer.Text), 2, MidpointRounding.AwayFromZero);

                    }
                    else
                    {
                        dr["PriceQuant"] = 0.00;
                    }

                    //todo
                    Double gstAmount = 0;
                    if (hdnsTaxType.Value == "3")
                    {
                        dr["GTaxAmt"] = 0.00;
                        dr["STaxAmt"] = 0.00;
                    }
                    else
                    {
                        //GST
                        if (dtDetails.Columns.Contains("GTaxAmt"))
                        {
                            if (lblQuantity.Text != "" && lblPricePer.Text != "" && hdnGstTax.Value != "")
                            {
                                if (chkEnableGSTTax.Checked == true)
                                {
                                    //gstAmount = Math.Round((((Convert.ToDouble(lblQuantity.Text) * Convert.ToDouble(lblPricePer.Text)) * Convert.ToDouble(hdnGstTax.Value)) / 100), 2, MidpointRounding.AwayFromZero);
                                    //dr["GTaxAmt"] = gstAmount;
                                    dr["GTaxAmt"] = Convert.ToDouble(lblGstTax.Text);

                                }
                                else
                                {
                                    dr["GTaxAmt"] = 0.00;
                                }
                            }
                            else
                            {
                                dr["GTaxAmt"] = 0.00;
                            }
                        }
                    }



                    if (lblQuantity.Text != "" && lblPricePer.Text != "" && hdnStax.Value != "")
                    {
                        if (hdnsTaxType.Value != "3")
                        {
                            if (chkTaxable.Checked == true)
                            {
                                if (hdnsTaxType.Value == "1")
                                {
                                    //  dr["STaxAmt"] = Math.Round(((((Convert.ToDouble(lblQuantity.Text) * Convert.ToDouble(lblPricePer.Text)) + gstAmount) * Convert.ToDouble(hdnStax.Value)) / 100), 2, MidpointRounding.AwayFromZero);
                                    dr["STaxAmt"] = Convert.ToDouble(lblSalesTax.Text);
                                }
                                else
                                {
                                    // dr["STaxAmt"] = Math.Round((((Convert.ToDouble(lblQuantity.Text) * Convert.ToDouble(lblPricePer.Text)) * Convert.ToDouble(hdnStax.Value)) / 100), 2, MidpointRounding.AwayFromZero);
                                    dr["STaxAmt"] = Convert.ToDouble(lblSalesTax.Text);
                                }

                            }
                            else
                            {
                                dr["STaxAmt"] = 0.00;
                            }
                        }

                    }
                    else
                    {
                        dr["STaxAmt"] = 0.00;
                    }

                    if (lblQuantity.Text != "" && lblPricePer.Text != "" && hdnStax.Value != "")
                    {
                        dr["Amount"] = Math.Round(Convert.ToDouble(dr["PriceQuant"]) + Convert.ToDouble(dr["STaxAmt"]), 2, MidpointRounding.AwayFromZero);
                        if (hdnsTaxType.Value != "3")
                        {
                            if (chkTaxable.Checked)
                            {
                                taxable = taxable + (Convert.ToDouble(dr["PriceQuant"]));
                            }
                        }

                    }
                    else
                    {
                        dr["Amount"] = 0.00;
                    }
                    dr["STax"] = 0;
                    if (hdnsTaxType.Value != "3")
                    {
                        dr["STax"] = Convert.ToInt32(chkTaxable.Checked);
                    }

                    dr["Job"] = 0;
                    dr["JobItem"] = 0;
                    dr["TransID"] = 0;
                    dr["Measure"] = string.Empty;
                    dr["Disc"] = 0;
                    if (lblPricePer.Text != "")
                    {
                        dr["Price"] = Convert.ToDouble(lblPricePer.Text);
                    }
                    else
                    {
                        dr["Price"] = 0.00;
                    }

                    dr["billcode"] = txtBillingCode.Text;

                    if (resetJobCode == 1)
                        dr["Code"] = 0;
                    else
                        dr["Code"] = ddlProjectCode.SelectedValue;

                    HiddenField txtBCodeType = (HiddenField)gr.FindControl("txtBCodeType");

                    HiddenField hdnWarehouse = (HiddenField)gr.FindControl("hdnWarehouse");

                    HiddenField hdnWarehouseLocationID = (HiddenField)gr.FindControl("hdnWarehouseLocationID");

                    int INVType_1 = -1; int WHLocID_1 = 0;

                    int.TryParse(txtBCodeType.Value, out INVType_1);

                    int.TryParse(hdnWarehouseLocationID.Value, out WHLocID_1);

                    dr["INVType"] = INVType_1;



                    if (DL_INV.ISINVENTORYTRACKINGISON(Session["config"].ToString())) { dr["Warehouse"] = hdnWarehouse.Value; }
                    else
                    {
                        dr["Warehouse"] = "";
                    }

                    dr["WHLocID"] = WHLocID_1;

                    dr["InvStatus"] = (hdnStatus.Value == "") ? 0 : Convert.ToInt32(hdnStatus.Value);
                    dr["AStatus"] = (hdnAStatus.Value == "") ? 0 : Convert.ToInt32(hdnAStatus.Value);
                    //todo
                    if (Convert.ToBoolean(ViewState["isCanada"]))
                    {
                        if (hdnsTaxType.Value != "3")
                        {
                            dr["EnableGSTTax"] = chkEnableGSTTax.Checked;
                        }
                    }



                    dtDetails.Rows.Add(dr);
                }
            }

            ViewState["invoicetable"] = dtDetails;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr78", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    //protected void ddlBillingCode_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    /// Pending.....

    //    try
    //    {
    //        bool IsValid = true;
    //        string BillCode = "";
    //        if (Request.QueryString["uid"] != null)
    //        {
    //            DropDownList ddlBillingCode = (DropDownList)sender;
    //            //DataTable dt = dtBillingCodeData;
    //            if (!string.IsNullOrEmpty(ddlBillingCode.SelectedValue))
    //            {
    //                DataRow dr = dt.Select("ID = " + ddlBillingCode.SelectedValue).SingleOrDefault();
    //                if (dr != null)
    //                {
    //                    if (!Convert.ToInt16(dr["statusid"]).Equals(0))
    //                    {
    //                        BillCode = ddlBillingCode.SelectedItem.Text;
    //                        ddlBillingCode.SelectedValue = "";
    //                        IsValid = false;
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                IsValid = false;
    //            }
    //        }

    //        if (!IsValid)
    //        {
    //            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr98", "InActiveBillCode('" + BillCode + "');", true);
    //        }

    //        //ToDo
    //        //FillProjectCodes();
    //        //GridData(0);
    //        //BindGrid();
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr744", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}

    protected void ddlTerms_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateDueByTerms();
    }

    protected void lnkMakePayment_Click(object sender, EventArgs e)
    {
        try
        {
            //btnSubmit_Click(sender, e);

            Submit(0);

            if (success == true)
            {
                if (Request.QueryString["o"] == null)
                    Response.Redirect("payment.aspx?uid=" + objProp_Contracts.InvoiceID + "&amt=" + objProp_Contracts.Total, true);
                else
                    Response.Redirect("payment.aspx?uid=" + objProp_Contracts.InvoiceID + "&amt=" + objProp_Contracts.Total + "&o=1", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr32", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void hideModalPopupViaServerConfirm_Click(object sender, EventArgs e)
    {
        //AddOtherTickets();
    }

    private DataSet BuildLineItems(double totalTime, double Expenses, int mileage)
    {
        DataSet dsLineItem = new DataSet();
        try
        {
            //int count = 0;  //int countTT = 0;       //int countMil = 0;
            string line = string.Empty;
            if (Expenses != 0)
            {
                //count += 1;  
                line = "'expenses'";
            }
            if (mileage != 0)
            {
                //countMil = count;
                //count += 1;
                if (line != string.Empty)
                {
                    line += ",";
                }
                line += "'mileage'";
            }
            if (totalTime != 0)
            {
                //countTT = count;        
                if (line != string.Empty)
                {
                    line += ",";
                }
                line += "'Time Spent'";
            }

            if (line.Trim() == string.Empty)
            {
                line = "'Time Spent'";
            }

            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.TicketLineItems = line;
            dsLineItem = objBL_Contracts.GetBillcodesforticket(objProp_Contracts);


            foreach (DataRow dr in dsLineItem.Tables[0].Rows)
            {
                if (Expenses != 0)
                {
                    if (string.Equals(dr["billcode"].ToString(), "expenses", StringComparison.InvariantCultureIgnoreCase))
                    {
                        dr["Quan"] = 1;
                        dr["price"] = Expenses;
                    }
                }
                if (mileage != 0)
                {
                    if (string.Equals(dr["billcode"].ToString(), "mileage", StringComparison.InvariantCultureIgnoreCase))
                        dr["Quan"] = mileage;
                }
                if (totalTime != 0)
                {
                    if (string.Equals(dr["billcode"].ToString(), "Time Spent", StringComparison.InvariantCultureIgnoreCase))
                        dr["Quan"] = totalTime;
                }
            }
            if (Request.QueryString["tickid"] != null)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script language='javascript'>");
                sb.Append(@"var header = document.getElementById('header');");
                sb.Append(@"header.style.display='none';");
                sb.Append(@"var side = document.getElementById('ctl00_menu');");
                sb.Append(@"side.style.display='none';");
                sb.Append(@"var main = document.getElementById('main');");
                sb.Append(@"main.style.padding='0px';");
                sb.Append(@"</script>");
                ClientScript.RegisterStartupScript(this.GetType(), "HideMaster", sb.ToString());

                #region Spilt Time Spent(TotalTime)



                //////////////////
                DataSet dsLineItem2 = new DataSet();

                DataTable tblRT = new DataTable();
                DataTable tblOT = new DataTable();
                DataTable tblNT = new DataTable();
                DataTable tblDT = new DataTable();
                DataTable tblTT = new DataTable();
                /////////////
                string RTvalue = "0";
                string OTvalue = "0";
                string NTvalue = "0";
                string DTvalue = "0";
                string TTvalue = "0";
                //string RT_TT = "0";
                ///////////////
                double BillRatePrice = 0;
                double RateOTPrice = 0;
                double RateDTPrice = 0;
                double RateNTPrice = 0;
                double RateMileagePrice = 0;
                double RateTravelPrice =  0 ;
                // string BillRate_RateTravel = "0";
                ////////
                Int32 TicketID, Combind, Project, Reviewonly, Workorderonly = 0;

                TicketID = Convert.ToInt32(Request.QueryString["tickid"].ToString());
                Combind = Convert.ToInt32(Request.QueryString["Combind"].ToString());
                Project = Convert.ToInt32(Request.QueryString["Project"].ToString());
                Reviewonly = Convert.ToInt32(Request.QueryString["Reviewonly"].ToString());
                Workorderonly = Convert.ToInt32(Request.QueryString["Workorderonly"].ToString());

                InvoicingFromTicketScreen _InvoicingFromTicketScreen = new InvoicingFromTicketScreen();

                _InvoicingFromTicketScreen.ConnConfig = Session["config"].ToString();
                _InvoicingFromTicketScreen.TicketID = TicketID;
                _InvoicingFromTicketScreen.Combind = Combind;
                _InvoicingFromTicketScreen.Project = Project;
                _InvoicingFromTicketScreen.Reviewonly = Reviewonly;
                _InvoicingFromTicketScreen.Workorderonly = Workorderonly;

                ViewState["othertickets"] = Request.QueryString["tickid"];

                dsLineItem2 = new BL_Invoice().InvoicingFromTicketScreen(_InvoicingFromTicketScreen);

                if (dsLineItem2.Tables[0].Rows.Count > 0 && dsLineItem2.Tables[1].Rows.Count > 0 && dsLineItem2.Tables[2].Rows.Count > 0 && dsLineItem2.Tables[3].Rows.Count > 0 && dsLineItem2.Tables[4].Rows.Count > 0 && dsLineItem2.Tables[5].Rows.Count > 0 && dsLineItem2.Tables[6].Rows.Count > 0)
                {

                    ViewState["othertickets"] = dsLineItem2.Tables[7].Rows[0]["TicketsID"].ToString().Trim();

                    RTvalue = dsLineItem2.Tables[5].Rows[0]["RT"].ToString().Trim();
                    OTvalue = dsLineItem2.Tables[5].Rows[0]["OT"].ToString().Trim();
                    NTvalue = dsLineItem2.Tables[5].Rows[0]["NT"].ToString().Trim();
                    DTvalue = dsLineItem2.Tables[5].Rows[0]["DT"].ToString().Trim();
                    TTvalue = dsLineItem2.Tables[5].Rows[0]["TT"].ToString().Trim();
                    // RT_TT = dsLineItem2.Tables[5].Rows[0]["RT_TT"].ToString().Trim();
                    #region ////ES-8761 West coast PDT- Creating invoice from newly created ticket is giving error.
                    // double.TryParse( , out );
                    double.TryParse(dsLineItem2.Tables[6].Rows[0]["BillRatePrice"].ToString().Trim() ,    out BillRatePrice );
                     double.TryParse(dsLineItem2.Tables[6].Rows[0]["RateOTPrice"].ToString().Trim(),      out RateOTPrice);
                     double.TryParse(dsLineItem2.Tables[6].Rows[0]["RateDTPrice"].ToString().Trim(),      out RateDTPrice);
                     double.TryParse(dsLineItem2.Tables[6].Rows[0]["RateNTPrice"].ToString().Trim(),      out RateNTPrice);
                     double.TryParse(dsLineItem2.Tables[6].Rows[0]["RateMileagePrice"].ToString().Trim(), out RateMileagePrice);
                     double.TryParse(dsLineItem2.Tables[6].Rows[0]["RateTravelPrice"].ToString().Trim(),  out RateTravelPrice);
                    // BillRate_RateTravel = dsLineItem2.Tables[6].Rows[0]["BillRate_RateTravel"].ToString().Trim();
                    #endregion
                    //RT
                    tblRT = dsLineItem2.Tables[0];
                    tblRT.Rows[0]["Quan"] = RTvalue;
                    if (Convert.ToDouble(BillRatePrice) > 0)
                    {
                        tblRT.Rows[0]["price"] = BillRatePrice;
                    }

                    //RT
                    tblOT = dsLineItem2.Tables[1];
                    tblOT.Rows[0]["Quan"] = OTvalue;
                    if (Convert.ToDouble(RateOTPrice) > 0)
                    {
                        tblOT.Rows[0]["price"] = RateOTPrice;
                    }

                    //DT
                    tblDT = dsLineItem2.Tables[2];
                    tblDT.Rows[0]["Quan"] = DTvalue;
                    if (Convert.ToDouble(RateDTPrice) > 0)
                    {
                        tblDT.Rows[0]["price"] = RateDTPrice;
                    }
                    //NT
                    tblNT = dsLineItem2.Tables[3];
                    tblNT.Rows[0]["Quan"] = NTvalue;
                    if (Convert.ToDouble(RateNTPrice) > 0)
                    {
                        tblNT.Rows[0]["price"] = RateNTPrice;
                    }

                    //TT
                    tblTT = dsLineItem2.Tables[4];
                    tblTT.Rows[0]["Quan"] = TTvalue;
                    if (Convert.ToDouble(RateTravelPrice) > 0)
                    {
                        tblTT.Rows[0]["price"] = RateTravelPrice;
                    }

                    DataTable Alltbl = new DataTable();
                    Alltbl.Merge(tblRT);
                    Alltbl.Merge(tblOT);
                    Alltbl.Merge(tblNT);
                    Alltbl.Merge(tblDT);
                    Alltbl.Merge(tblTT);

                    #region Invoice for inventory used billable items on a ticket 
                    try
                    {
                        DataTable INVitems = dsLineItem2.Tables[8];
                        Alltbl.Merge(INVitems);
                    }
                    catch { }

                    #endregion

                    dsLineItem.Tables[0].Merge(Alltbl);


                    foreach (DataRow dr in dsLineItem.Tables[0].Rows)
                    {
                        if (string.Equals(dr["billcode"].ToString(), "Time Spent", StringComparison.InvariantCultureIgnoreCase))
                        {
                            dr.Delete();
                        }
                        // Remove 0 Hours Row 
                        else if (Convert.ToDouble(dr["Quan"].ToString()) == 0)
                        {
                            dr.Delete();
                        }

                    }

                    dsLineItem.Tables[0].AcceptChanges();

                    foreach (DataRow dr in dsLineItem.Tables[0].Rows)
                    {
                        if (mileage != 0)
                        {
                            if (string.Equals(dr["billcode"].ToString(), "mileage", StringComparison.InvariantCultureIgnoreCase))
                                dr["price"] = RateMileagePrice;
                        }


                    }
                }

                #endregion

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr88", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return dsLineItem;
    }

    private void LoadTicketData()
    {
        try
        {
            if (Request.QueryString["o"] != null)
            {
                lnkClose.Attributes["onclick"] = "window.close(); return;";
                pnlNext.Visible = false;
                if (Request.QueryString["tickid"] != null)
                {

                    DataSet ds = new DataSet();
                    objMapData.ConnConfig = Session["config"].ToString();
                    objMapData.ISTicketD = 1;
                    objMapData.TicketID = Convert.ToInt32(Request.QueryString["tickid"].ToString());
                    ds = objBL_MapData.GetTicketByID(objMapData);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["invoice"] != DBNull.Value)
                        {
                            if (ds.Tables[0].Rows[0]["invoice"].ToString().Trim() != string.Empty && ds.Tables[0].Rows[0]["invoice"].ToString().Trim() != "0")
                            {
                                Response.Redirect("addinvoice.aspx?o=1&uid=" + ds.Tables[0].Rows[0]["invoice"].ToString());
                            }
                        }

                        double totalTime = Convert.ToDouble(ds.Tables[0].Rows[0]["total"]);

                        double Expenses = Convert.ToDouble(ds.Tables[0].Rows[0]["othere"]) + Convert.ToDouble(ds.Tables[0].Rows[0]["toll"]) + Convert.ToDouble(ds.Tables[0].Rows[0]["zone"]);

                        int mileage = Convert.ToInt32(ds.Tables[0].Rows[0]["emile"]) - Convert.ToInt32(ds.Tables[0].Rows[0]["Smile"]);

                        txtLocation.Text = ds.Tables[0].Rows[0]["locname"].ToString();
                        hdnLocId.Value = ds.Tables[0].Rows[0]["lid"].ToString();
                        hdnPatientId.Value = ds.Tables[0].Rows[0]["owner"].ToString();
                        ddlDepartment.SelectedValue = ds.Tables[0].Rows[0]["type"].ToString();
                        FillWorker(Convert.ToInt32(ds.Tables[0].Rows[0]["fwork"].ToString()));
                        ddlRoute.SelectedValue = ds.Tables[0].Rows[0]["fwork"].ToString();

                        txtDueDate.Text = System.DateTime.Now.Date.ToShortDateString();

                        hdnProjectId.Value = ds.Tables[0].Rows[0]["job"].ToString();

                        FillProjectCodes(true, out int GLAcct);

                        txtProject.Text = ds.Tables[0].Rows[0]["jobdesc"].ToString();

                        FillLocInfo();

                        GetDataProject();

                        FillInvoiceProjectInfo();

                        DataSet dsLineItem = BuildLineItems(totalTime, Expenses, mileage);

                        //todo                      

                        DataColumn colEnableGSTTax = new System.Data.DataColumn("EnableGSTTax", typeof(Boolean));

                        colEnableGSTTax.DefaultValue = 0;

                        dsLineItem.Tables[0].Columns.Add(colEnableGSTTax);

                        ViewState["invoicetable"] = dsLineItem.Tables[0];

                        ViewState["tickets"] = Request.QueryString["tickid"].ToString();

                        string stticket = "";

                        ///////////---------------------------->   ref  ES-6579-Port City-Date and Tech name on invoice to customer

                        if (Request.QueryString["tickid"] != null && Request.QueryString["Workorderonly"] != null && Request.QueryString["Combind"] != null)
                        {


                            if (Request.QueryString["Project"].ToString() == "0" && Request.QueryString["Workorderonly"].ToString() == "0" && Request.QueryString["Combind"].ToString() == "0")
                            {

                                stticket = " Work Performed on " + Convert.ToDateTime(ds.Tables[0].Rows[0]["edate"].ToString()).ToShortDateString() + " by " + ddlRoute.SelectedItem.Text;

                            }
                        }

                        txtRemarks.Text = "Invoice for Ticket# " + ViewState["othertickets"].ToString() + stticket + Environment.NewLine;

                        txtRemarks.Text += "Reason for service: " + Environment.NewLine + ds.Tables[0].Rows[0]["fdesc"].ToString() + Environment.NewLine;

                        txtRemarks.Text += "Work complete desc.: " + Environment.NewLine + ds.Tables[0].Rows[0]["descres"].ToString();

                        BindGrid();

                        //Set Hyperlink  For Loc / Customer
                        if (hdnLocId.Value != "0" && hdnLocId.Value != "")
                        {
                            lnkLocationID.NavigateUrl = "addlocation.aspx?uid=" + hdnLocId.Value;

                        }
                        else
                        {
                            lnkLocationID.NavigateUrl = "";
                        }

                        if (hdnPatientId.Value != "0" && hdnPatientId.Value != "")
                        {
                            lnkCustomerID.NavigateUrl = "addcustomer.aspx?uid=" + hdnPatientId.Value;
                        }
                        else
                        {
                            lnkCustomerID.NavigateUrl = "";
                        }
                        // AddOtherTicketsAlert();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr87", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void GetDataProject()
    {
        try
        {
            DataSet ds = new DataSet();
            objCustomer.ConnConfig = Session["config"].ToString();
            objCustomer.LocID = Convert.ToInt32(hdnLocId.Value);
            objCustomer.Mode = Convert.ToInt32(ViewState["mode"]);
            ds = objBL_Customer.getJobEstimate(objCustomer);
            //RadGrid_gvProject.DataSource = ds.Tables[0];
            //RadGrid_gvProject.Rebind();

            RadProjectListView.DataSource = ds.Tables[0];
            RadProjectListView.Rebind();

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr6632", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private string calculateTotal()     // added by Mayuri 7th Dec,15
    {
        double _totalAmount = 0.00;
        double _taxAmt = 0.00;
        double _taxgstAmt = 0.00;
        double _pretaxAmount = 0.00;
        try
        {
            foreach (GridDataItem gr in RadGrid_gvInvoices.Items)
            {
                Label lblIndex = (Label)gr.FindControl("lblIndex");
                TextBox lblQuantity = (TextBox)gr.FindControl("lblQuantity");
                TextBox lblDescription = (TextBox)gr.FindControl("lblDescription");
                TextBox lblPricePer = (TextBox)gr.FindControl("lblPricePer");
                CheckBox chkTaxable = (CheckBox)gr.FindControl("chkTaxable");
                Label lblPretaxAmount = (Label)gr.FindControl("lblPretaxAmount");


                CheckBox chkEnableGSTTax = (CheckBox)gr.FindControl("chkEnableGSTTax");

                if (lblQuantity.Text.Trim() != string.Empty && lblPricePer.Text.Trim() != string.Empty)
                {
                    if (hdnStax.Value != "")
                    {
                        if (chkTaxable.Checked == true)
                        {
                            _pretaxAmount = _pretaxAmount + Convert.ToDouble(lblPretaxAmount.Text);
                            _taxAmt = (((Convert.ToDouble(lblQuantity.Text) * Convert.ToDouble(lblPricePer.Text)) * Convert.ToDouble(hdnStax.Value)) / 100);
                        }


                    }
                    if (hdnGstTax.Value != null)
                    {
                        if (chkEnableGSTTax.Checked == true)
                        {
                            _pretaxAmount = _pretaxAmount + Convert.ToDouble(lblPretaxAmount.Text);
                            _taxgstAmt = (((Convert.ToDouble(lblQuantity.Text) * Convert.ToDouble(lblPricePer.Text)) * Convert.ToDouble(hdnGstTax.Value)) / 100);
                        }
                    }

                    //todo
                    _totalAmount = _totalAmount + (Convert.ToDouble(lblQuantity.Text) * Convert.ToDouble(lblPricePer.Text)) + Convert.ToDouble(_taxAmt) + Convert.ToDouble(_taxgstAmt);


                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr782", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return _totalAmount + "|" + _pretaxAmount;
    }

    private void GetPeriodDetails(DateTime _invDate)
    {
        bool Flag = CommonHelper.GetPeriodDetails(_invDate);
        ViewState["FlagPeriodClose"] = Flag;
        if (!Flag)
        {
            btnSubmit.Visible = false;
            if (Request.QueryString["c"] != null)
            {
                btnSubmit.Visible = true;
            }

            if (divProjectClose.Visible == false)
            {
                divSuccess.Visible = true;
            }

        }

    }

    protected void btnGetCode_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["ResetGrid"] = 1;
            hdnIsProjectPO.Value = "1";
            if (hdnIsProjectSearch.Value == "1")
            {
                FillLocInfo();
                GetDataProject();
            }

            FillProjectCodes(true, out int GLAcct);

            FillBillCodes(GLAcct);
            FillInvoiceProjectInfo();

            BindGrid();
            SetDefaultSalesPerson();
            ddlDepartment.Enabled = false;

            RequiredFieldValidator32.Enabled = true;

            lnkProjectID.NavigateUrl = "addProject.aspx?uid=" + hdnProjectId.Value;
            iProIcon.Attributes.Add("style", "color:#1565C0 !important;");
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr7885", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void FillInvoiceProjectInfo()
    {
        try
        {
            if (txtProject.Text != string.Empty)
            {
                objCustomer.ConnConfig = Session["config"].ToString();

                //  objCustomer.ProjectJobID = Convert.ToInt32(txtProject.Text);

                objCustomer.ProjectJobID = objPropUser.JobId = Convert.ToInt32(hdnProjectId.Value);

                objCustomer.Type = string.Empty;

                //DataSet ds = objBL_Customer.getJobProjectByJobID(objCustomer);

                //objPropUser.ConnConfig = Session["config"].ToString();

                //DataSet dsJob = objBL_User.GetJobBillRatesById(objPropUser);
                ////saleperson                
                //DataSet dsSale = objBL_Customer.GetSalePersonByJob(objCustomer);

                DataSet ds = objBL_Customer.getJobProjectByJobIDRatesByIdPersonByJob(objCustomer);

                //if (dsSale.Tables[0].Rows.Count > 0)
                if (ds.Tables[2].Rows.Count > 0)
                {
                    //ddlsaleperson.SelectedValue = dsSale.Tables[0].Rows[0]["ID"].ToString();
                    //ddlsaleperson.SelectedValue = ds.Tables[2].Rows[0]["ID"].ToString();
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    if (string.IsNullOrEmpty(dr["ID"].ToString()) && string.IsNullOrEmpty(dr["fDesc"].ToString()))
                        txtProject.Text = "";
                    else
                        txtProject.Text = dr["ID"].ToString() + "-" + dr["fDesc"].ToString();

                    txtJobRemarks.Text = dr["Remarks"].ToString();
                    if (dr["SPHandle"].ToString() == "1")
                    {
                        lblJobSRemarks.Visible = true;
                        txtJobSRemarks.Visible = true;
                        txtJobSRemarks.Text = dr["SRemarks"].ToString();
                    }
                    else
                    {
                        txtJobSRemarks.Text = "";
                        lblJobSRemarks.Visible = false;
                        txtJobSRemarks.Visible = false;
                    }
                    if (Request.QueryString["o"] != null)
                    {
                        hdnIsProjectPO.Value = "1";
                    }

                    //todo
                    if (txtPO.Text.Trim() == "")
                    {
                        if (!string.IsNullOrEmpty(hdnIsProjectPO.Value) && hdnIsProjectPO.Value == "1")
                        {
                            txtPO.Text = dr["PO"].ToString();
                        }
                    }

                    if (!string.IsNullOrEmpty(dr["Type"].ToString()))
                    {
                        ddlDepartment.SelectedValue = dr["Type"].ToString();
                    }

                    ViewState["InvServ"] = dr["InvServ"].ToString();
                    double billrate = 0;
                    //double quan = 1;
                    //if (dsJob.Tables[0].Rows.Count > 0)
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        //billrate = Convert.ToDouble(dsJob.Tables[0].Rows[0]["BillRate"]);
                        billrate = Convert.ToDouble(ds.Tables[1].Rows[0]["BillRate"]);
                    }
                    ViewState["BillRate"] = billrate;
                    if (billrate > 0)
                    {
                        hdnBillRate.Value = billrate.ToString();
                    }
                    DataTable dtInv = (DataTable)ViewState["invoicetable"];
                    if (!string.IsNullOrEmpty(dr["InvServ"].ToString()) && dr["InvServ"].ToString() != "0")
                    {
                        int i = 0;
                        foreach (DataRow drInv in dtInv.Rows)
                        {
                            drInv["acct"] = dr["InvServ"].ToString();
                            drInv["InvStatus"] = Convert.ToString(dr["InvStatus"]);
                            ViewState["DefaultAcct"] = Convert.ToInt32(dr["InvServ"]);
                            if (i == 0)
                            {
                                drInv["Quan"] = 1;
                            }
                            i++;
                        }
                    }

                    dtInv.AsEnumerable().ToList().
                        ForEach(t => t["Price"] = billrate);



                    ViewState["invoicetable"] = dtInv;

                    ///// ProjectBillingInfo
                    try
                    {
                        string ptype = ds.Tables[0].Rows[0]["PType"].ToString();

                        if (ptype == "1") { txtBillingType.Text = "Quoted"; }
                        else if (ptype == "2") { txtBillingType.Text = "Maximum"; }
                        else { txtBillingType.Text = "None"; }

                        txtjobamt.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["Amount"]));
                        txtBillRate.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["BillRate"].ToString()));
                        txtOt.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateOT"].ToString()));
                        txtNt.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateNT"].ToString()));
                        txtDt.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateDT"].ToString()));
                        txtMileage.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateMileage"].ToString()));
                        txtTravel.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateTravel"].ToString()));


                    }
                    catch { }
                }

                ProjectBillingInfo.Visible = true;
            }
            else
            {
                ProjectBillingInfo.Visible = false;
                txtBillingType.Text = "";
                txtjobamt.Text = "";
                txtBillRate.Text = "";
                txtOt.Text = "";
                txtNt.Text = "";
                txtDt.Text = "";
                txtMileage.Text = "";
                txtTravel.Text = "";
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void ddlProjectCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ViewState["ResetGrid"] = 0;
            if (Request.QueryString["tickid"] != null) { return; }////not change price if invoce create from ticket
            DropDownList ddlProjectCode = (DropDownList)sender;
            GridDataItem gridrow = (GridDataItem)ddlProjectCode.NamingContainer;
            int rowIndex = gridrow.RowIndex;

            if (!ddlProjectCode.SelectedValue.Equals("0"))
            {
                objJob.ConnConfig = Session["config"].ToString();
                objJob.Job = Convert.ToInt32(hdnProjectId.Value);
                objJob.Line = Convert.ToInt16(ddlProjectCode.SelectedValue);
                DataSet ds = objBL_Job.GetRevenueJobItemsByJob(objJob);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    TextBox lblDescription = (TextBox)gridrow.FindControl("lblDescription");
                    TextBox lblPricePer = (TextBox)gridrow.FindControl("lblPricePer");
                    Label lblPretaxAmount = (Label)gridrow.FindControl("lblPretaxAmount");
                    TextBox lblSalesTax = (TextBox)gridrow.FindControl("lblSalesTax");
                    Label lblAmount = (Label)gridrow.FindControl("lblAmount");
                    TextBox lblQuantity = (TextBox)gridrow.FindControl("lblQuantity");


                    if (dr["Quantity"] != null && !string.IsNullOrEmpty(dr["Quantity"].ToString()))
                        lblQuantity.Text = Convert.ToDouble(dr["Quantity"]).ToString("N", CultureInfo.InvariantCulture);
                    else
                        lblQuantity.Text = "1";

                    if (dr["Price"] != null && !string.IsNullOrEmpty(dr["Price"].ToString()))
                        lblPricePer.Text = Convert.ToDouble(dr["Price"]).ToString("N", CultureInfo.InvariantCulture);
                    else
                        lblPricePer.Text = Convert.ToDouble(dr["Amount"]).ToString("N", CultureInfo.InvariantCulture);

                    lblDescription.Text = dr["fDesc"].ToString();

                    lblSalesTax.Text = "0.00";
                    lblPretaxAmount.Text = Convert.ToDouble(dr["Amount"]).ToString("N", CultureInfo.InvariantCulture);
                    lblAmount.Text = Convert.ToDouble(dr["Amount"]).ToString("N", CultureInfo.InvariantCulture);

                }
            }
            //ToDo
            //GridData(0);
            //DataTable dt = (DataTable)ViewState["invoicetable"];
            //CalculateTotals(dt);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr9856", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void UpdateDueByTerms()
    {
        if (ddlTerms.SelectedValue == "0")
        {
            txtDueDate.Text = txtInvoiceDate.Text;
        }
        else if (ddlTerms.SelectedValue == "1")
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(10).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "2")
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(15).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "3")
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(30).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "4")
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(45).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "5")
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(60).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "6")
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(30).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "7")
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(90).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "8")
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(180).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "9")
        {
            txtDueDate.Text = txtInvoiceDate.Text;
        }
        else if (ddlTerms.SelectedValue == "10") //120 days
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(120).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "11") //150 days
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(150).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "12") //210 days
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(210).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "13") //240 days
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(240).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "14") //270 days
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(270).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "15") //300 days
        {
            txtDueDate.Text = Convert.ToDateTime(txtInvoiceDate.Text).AddDays(300).ToShortDateString();
        }
        else if (ddlTerms.SelectedValue == "16") //net due on 10th
        {
            txtDueDate.Text = "";
        }
        else if (ddlTerms.SelectedValue == "17") //net due
        {
            txtDueDate.Text = "";
        }
        else if (ddlTerms.SelectedValue == "18") //Credit card
        {
            txtDueDate.Text = "";
        }
    }

    protected void lnk_InvoiceMaint_Click(object sender, EventArgs e)
    {
        try
        {
            Session["InvoiceName"] = "InvoiceMaint";
            Submit(0);

            if (success == true)
            {
                var reportFormat = ConfigurationManager.AppSettings["InvoiceReportFormat"].ToString();
                if (reportFormat.ToUpper().Equals("MRT"))
                    Response.Redirect("PreviewInvoice.aspx?uid=" + objProp_Contracts.InvoiceID.ToString());
                else if (reportFormat.ToUpper().Equals("RDLC"))
                    Response.Redirect("PrintInvoice.aspx?uid=" + objProp_Contracts.InvoiceID.ToString());

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr784", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnk_InvoiceException_Click(object sender, EventArgs e)
    {
        try
        {
            Session["InvoiceName"] = "InvoiceException";
            Submit(0);

            if (success == true)
            {
                var reportFormat = ConfigurationManager.AppSettings["InvoiceReportFormat"].ToString();
                if (reportFormat.ToUpper().Equals("MRT"))
                    Response.Redirect("PreviewInvoice.aspx?uid=" + objProp_Contracts.InvoiceID.ToString());
                else if (reportFormat.ToUpper().Equals("RDLC"))
                    Response.Redirect("PrintInvoice.aspx?uid=" + objProp_Contracts.InvoiceID.ToString());

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr7541", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnk_InvoiceLNY_Click(object sender, EventArgs e)
    {
        try
        {
            Session["InvoiceName"] = "InvoiceLNY";
            Submit(0);

            if (success == true)
            {
                var reportFormat = ConfigurationManager.AppSettings["InvoiceReportFormat"].ToString();
                if (reportFormat.ToUpper().Equals("MRT"))
                    Response.Redirect("PreviewInvoice.aspx?uid=" + objProp_Contracts.InvoiceID.ToString());
                else if (reportFormat.ToUpper().Equals("RDLC"))
                    Response.Redirect("PrintInvoice.aspx?uid=" + objProp_Contracts.InvoiceID.ToString());

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr356", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkInvTkt_InvoiceLNY_Click(object sender, EventArgs e)
    {
        try
        {
            Session["InvoiceName"] = "InvoiceTicketLNY";
            Submit(0);

            if (success == true)
            {
                var reportFormat = ConfigurationManager.AppSettings["InvoiceReportFormat"].ToString();
                if (reportFormat.ToUpper().Equals("MRT"))
                    Response.Redirect("PreviewInvoice.aspx?uid=" + objProp_Contracts.InvoiceID.ToString());
                else if (reportFormat.ToUpper().Equals("RDLC"))
                    Response.Redirect("PrintInvoice.aspx?uid=" + objProp_Contracts.InvoiceID.ToString());

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr4512", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    //protected void RadGrid_gvProject_PreRender(object sender, EventArgs e)
    //{
    //    foreach (GridDataItem gr in RadGrid_gvProject.Items)
    //    {
    //        Label lblID = (Label)gr.FindControl("lblID");
    //        Label lblname = (Label)gr.FindControl("lblID");
    //        CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

    //        gr.Attributes["onmousedown"] = "SelectRowFill('" + RadGrid_gvProject.ClientID + "','" + lblID.ClientID + "','" + lblname.ClientID + "','" + hdnProjectId.ClientID + "','" + txtProject.ClientID + "','divproject');  document.getElementById('" + btnGetCode.ClientID + "').click();";
    //    }
    //}

    protected void RadGrid_gvInvoices_PreRender(object sender, EventArgs e)
    {
        //if (Convert.ToBoolean(ViewState["isCanada"]) == false)
        //{
        //    SetupNonCanadaGrid();
        //}
        //else
        //{
        if (ViewState["ResetGrid"] != null && Convert.ToInt32(ViewState["ResetGrid"]) == 1)
        {
            SetupGrid();
        }
        //}

        foreach (GridDataItem gr in RadGrid_gvInvoices.Items)
        {
            Label lblIndex = (Label)gr.FindControl("lblIndex");
            Label lblID = (Label)gr.FindControl("lblId");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            CheckBox chkTaxable = (CheckBox)gr.FindControl("chkTaxable");
            TextBox lblQuantity = (TextBox)gr.FindControl("lblQuantity");
            TextBox lblPricePer = (TextBox)gr.FindControl("lblPricePer");
            TextBox lblSalesTax = (TextBox)gr.FindControl("lblSalesTax");
            TextBox lblDescription = (TextBox)gr.FindControl("lblDescription");
            TextBox txtBillingCode = (TextBox)gr.FindControl("txtBillingCode");
            HiddenField txtBillingCodeID = (HiddenField)gr.FindControl("txtBCodeID");
            RequiredFieldValidator rfvQuantity = (RequiredFieldValidator)gr.FindControl("rfvQuantity");
            RequiredFieldValidator rfvBillCode = (RequiredFieldValidator)gr.FindControl("rfvBillCode");
            RequiredFieldValidator rfvPricePer = (RequiredFieldValidator)gr.FindControl("rfvPricePer");
            lblQuantity.Attributes["onkeyup"] = "document.getElementById('" + hdnFocus.ClientID + "').value='" + lblQuantity.ClientID + "'; CalculateGridAmount(); ";
            lblPricePer.Attributes["onkeyup"] = "document.getElementById('" + hdnFocus.ClientID + "').value='" + lblPricePer.ClientID + "'; CalculateGridAmount(); ";
            lblSalesTax.Attributes["onkeyup"] = "document.getElementById('" + hdnFocus.ClientID + "').value='" + lblSalesTax.ClientID + "'; CalculateGridAmount(); ";
            chkTaxable.Attributes["onclick"] = "CalculateGridAmount();";

            CheckBox chkEnableGSTTax = (CheckBox)gr.FindControl("chkEnableGSTTax");
            chkEnableGSTTax.Attributes["onclick"] = "CalculateGridAmount();";
        }
        try
        {
            if (RadGrid_gvInvoices.Items.Count > 1)
            {
                int lastRow = RadGrid_gvInvoices.Items.Count;

                GridDataItem gr = (GridDataItem)RadGrid_gvInvoices.Items[lastRow - 1];
                DropDownList ddlProjectCode = (DropDownList)gr.FindControl("ddlProjectCode");
                ddlProjectCode.Focus();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr9856", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void RadGrid_gvInvoices_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem gd = (GridDataItem)e.Item;
            CheckBox chk = (CheckBox)gd.FindControl("chkSelect");
            int index = gd.ItemIndex;
        }

    }

    protected void lnkPreviewEmail_Click(object sender, EventArgs e)
    {
        try
        {
            String page = "";
            if (Request.QueryString["page"] != null)
            {
                page = "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl);
            }

            var reportFormat = ConfigurationManager.AppSettings["InvoiceReportFormat"].ToString();
            if (Request.QueryString["uid"] != null)
            {
                if (Request.QueryString["c"] != null && Request.QueryString["c"] == "1")
                {
                    Submit(1);
                }
                else
                {
                    if (imgPaid.Visible == false && btnSubmit.Visible == true)
                    {
                        Submit(1);
                    }

                    if (reportFormat.ToUpper().Equals("MRT"))
                    {
                        Response.Redirect("PreviewInvoice.aspx?uid=" + Request.QueryString["uid"].ToString() + page);
                    }
                    else if (reportFormat.ToUpper().Equals("RDLC"))
                    {
                        Response.Redirect("PrintInvoice.aspx?uid=" + Request.QueryString["uid"].ToString() + page);
                    }
                }

            }
            else
            {
                if (txtProject.Text != string.Empty)
                {
                    foreach (GridDataItem gr in RadGrid_gvInvoices.Items)
                    {
                        DropDownList ddlProjectCode = (DropDownList)gr.FindControl("ddlProjectCode");
                        HiddenField txtBillingCodeID = (HiddenField)gr.FindControl("txtBCodeID");
                        TextBox lblQuantity = (TextBox)gr.FindControl("lblQuantity");
                        if (ddlProjectCode.SelectedItem.Text == "-Select-" && txtBillingCodeID.Value != string.Empty && lblQuantity.Text != string.Empty && lblQuantity.Text != "0")
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Please select the Code.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                            return;
                        }
                    }
                }

                Submit(1);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr852", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void btnResetProject_Click(object sender, EventArgs e)
    {
        if (txtProject.Text == "")
        {
            ddlDepartment.Enabled = true;
            DataTable dt = (DataTable)ViewState["invoicetable"];
            dt.AsEnumerable().ToList().ForEach(row => row["code"] = "0");
            dt.AcceptChanges();
            ViewState["invoicetable"] = dt;
            FillProjectCodes(false, out int GLAcct);
            FillInvoiceProjectInfo();
            BindGrid();
            hdnProjectId.Value = "";

            //Set Hyperlink  For Project 

            lnkProjectID.NavigateUrl = "javascript:void(0);"; //lnkProjectID.Visible = false;
            iProIcon.Attributes.Add("style", "color:#5815c02b !important;");

        }
    }

    #region logs

    protected void RadGrid_gvLogs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        loadLog();
    }
    private void loadLog()
    {
        try
        {
            RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();
            if (Request.QueryString["uid"] != null)
            {
                DataSet dsLog = new DataSet();
                objProp_Contracts.ConnConfig = Session["config"].ToString();
                objProp_Contracts.InvoiceID = Convert.ToInt32(Request.QueryString["uid"]);
                dsLog = objBL_Contracts.GetInvoiceLogs(objProp_Contracts);
                if (dsLog.Tables[0].Rows.Count > 0)
                {
                    RadGrid_gvLogs.VirtualItemCount = dsLog.Tables[0].Rows.Count;
                    RadGrid_gvLogs.DataSource = dsLog.Tables[0];
                }
                else
                {
                    RadGrid_gvLogs.DataSource = string.Empty;
                }
            }
        }
        catch { }
    }
    bool isGroupLog = false;

    public bool ShouldApplySortFilterOrGroupLogs()
    {
        return RadGrid_gvLogs.MasterTableView.FilterExpression != "" ||
            (RadGrid_gvLogs.MasterTableView.GroupByExpressions.Count > 0 || isGroupLog) ||
            RadGrid_gvLogs.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_gvLogs_ItemCreated(object sender, GridItemEventArgs e)
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
    #endregion

    private void SetDefaultSalesPerson()
    {
        if (!string.IsNullOrEmpty(hdnLocId.Value) && hdnLocId.Value != "0")
        {
            objCustomer.ConnConfig = Session["config"].ToString();
            objCustomer.LocID = Convert.ToInt32(hdnLocId.Value);
            string result = objBL_Customer.GetDefaultSalesPerson(objCustomer);
            if (!string.IsNullOrEmpty(result) && ddlsaleperson.SelectedIndex == 0)
            {
                ddlsaleperson.SelectedValue = result;
            }
        }
    }

    protected void btnCopyPrevious_Click(object sender, EventArgs e)
    {
        FillProjectCodes(false, out int GLAcct);
        try
        {
            var selectIndex = 0;

            if (!string.IsNullOrEmpty(hdnSelectPOIndex.Value))
            {
                selectIndex = Convert.ToInt32(hdnSelectPOIndex.Value);
            }
            else
            {
                var selectItem = RadGrid_gvInvoices.MasterTableView.GetSelectedItems();
                if (selectItem.Count() > 0)
                {
                    selectIndex = selectItem[0].ClientRowIndex;
                }
            }

            GridData(0);
            var dt = (DataTable)ViewState["invoicetable"];
            if (dt.Rows.Count > 0 && selectIndex > 0)
            {
                var copyRow = dt.Rows[selectIndex - 1];
                var dr = dt.Rows[selectIndex];
                int i_ref = Convert.ToInt32(dr["Ref"]);
                int i_line = Convert.ToInt32(dr["line"]);
                dr.ItemArray = copyRow.ItemArray.Clone() as object[];
                dr["Ref"] = i_ref;
                dr["line"] = i_line;
                dt.AcceptChanges();

                ViewState["invoicetable"] = dt;
                BindGrid();

                // ScriptManager.RegisterStartupScript(this, Page.GetType(), "CalculateTotalUseTaxExpense", "CalculateTotalUseTaxExpense();", true);
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private DataTable getBillingCodeData(int GLAcct)
    {

        DataSet dsBilling = new DataSet();

        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.GLAccount = GLAcct;

        dsBilling = new BL_BillCodes().getBillCodes(objPropUser);
        DataRow drr = dsBilling.Tables[0].NewRow();

        dsBilling.Tables[0].Rows.InsertAt(drr, 0);

        return dsBilling.Tables[0];

    }
    protected void RadGrid_gvPayment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {

            if (Request.QueryString["uid"] != null)
            {
                DataSet dsPaymentLog = new DataSet();
                objProp_Contracts.ConnConfig = Session["config"].ToString();
                objProp_Contracts.InvoiceID = Convert.ToInt32(Request.QueryString["uid"]);
                dsPaymentLog = objBL_Contracts.GetHistoryPayment(objProp_Contracts);
                if (dsPaymentLog.Tables[0].Rows.Count > 0)
                {
                    RadGrid_gvPayment.DataSource = dsPaymentLog.Tables[0];
                }
                else
                {
                    RadGrid_gvPayment.DataSource = string.Empty;
                }
            }
        }
        catch { }
    }

    protected void lnkReceiptPayment_Click(object sender, EventArgs e)
    {
        Response.Redirect("addreceivepayment.aspx?page=addinvoice&uid=" + Request.QueryString["uid"].ToString(), true);
    }

    private void disableControl()
    {
        txtCustomer.Enabled = false;
        txtLocation.Enabled = false;
        txtAddress.Enabled = false;
        txtRemarks.Enabled = false;
        txtJobRemarks.Enabled = false;
        txtInvoiceDate.Enabled = false;
        txtProject.Enabled = false;
        txtInvoiceNo.Enabled = false;
        txtPO.Enabled = false;
        txtStaxrate.Enabled = false;
        ddlTerms.Enabled = false;
        ddlDepartment.Enabled = false;
        txtDueDate.Enabled = false;
        ddlStatus.Enabled = false;
        ddlRoute.Enabled = false;
        ddlsaleperson.Enabled = false;
        lnkAddnew.Enabled = false;
        btnDelete.Enabled = false;
        // RadGrid_gvInvoices.Enabled = false;
    }
    //private void BindGridWithTax()
    //{
    //    //todo
    //    try
    //    {
    //        objGenerals.ConnConfig = Session["config"].ToString();

    //        objGenerals.CustomName = "Country";

    //        DataSet dsCustom = objBL_General.getCustomFields(objGenerals);

    //        if (Convert.ToBoolean(ViewState["isCanada"]))
    //        {

    //                if (hdnsTaxType.Value == "2")
    //                {
    //                    RadGrid_gvInvoices.Columns.FindByUniqueName("stax").HeaderText = "HST";
    //                    RadGrid_gvInvoices.Columns.FindByUniqueName("stax").Visible = true;

    //                    RadGrid_gvInvoices.Columns.FindByUniqueName("STaxAmt").HeaderText = "HST Tax";
    //                    RadGrid_gvInvoices.Columns.FindByUniqueName("STaxAmt").Visible = true;

    //                    RadGrid_gvInvoices.Columns.FindByUniqueName("GTaxAmt").Visible = false;
    //                    RadGrid_gvInvoices.Columns.FindByUniqueName("EnableGSTTax").Visible = false;
    //                }
    //                else
    //                {


    //                    RadGrid_gvInvoices.Columns.FindByUniqueName("stax").HeaderText = "PST";
    //                    RadGrid_gvInvoices.Columns.FindByUniqueName("stax").Visible = true;

    //                    RadGrid_gvInvoices.Columns.FindByUniqueName("STaxAmt").HeaderText = "PST Tax";
    //                    RadGrid_gvInvoices.Columns.FindByUniqueName("STaxAmt").Visible = true;


    //                    RadGrid_gvInvoices.Columns.FindByUniqueName("GTaxAmt").Visible = true;
    //                    RadGrid_gvInvoices.Columns.FindByUniqueName("GTaxAmt").HeaderText = "GST Tax";

    //                    RadGrid_gvInvoices.Columns.FindByUniqueName("EnableGSTTax").HeaderText = "GST";
    //                    RadGrid_gvInvoices.Columns.FindByUniqueName("EnableGSTTax").Visible = true;
    //                    hdnGstTax.Value = dsCustom.Tables[0].Rows[0]["GstRate"].ToString();
    //                }

    //        }
    //        else
    //        {
    //            RadGrid_gvInvoices.Columns.FindByUniqueName("GTaxAmt").Visible = false;
    //            RadGrid_gvInvoices.Columns.FindByUniqueName("stax").HeaderText = "Taxable";
    //            RadGrid_gvInvoices.Columns.FindByUniqueName("stax").Visible = true;
    //            RadGrid_gvInvoices.Columns.FindByUniqueName("STaxAmt").HeaderText = "Sales Tax Amount";
    //            RadGrid_gvInvoices.Columns.FindByUniqueName("STaxAmt").Visible = true;
    //            RadGrid_gvInvoices.Columns.FindByUniqueName("EnableGSTTax").Visible = false;

    //        }


    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr11", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}

    public Boolean isCanadaCompany()
    {
        General _objPropGeneral = new General();
        BL_General _objBLGeneral = new BL_General();
        Boolean flag = false;
        _objPropGeneral.ConnConfig = Session["config"].ToString();
        DataSet _dsCustom = _objBLGeneral.getCompanyCountry(_objPropGeneral);
        try
        {
            if (_dsCustom.Tables[0].Rows[0]["Country"].ToString() == "Canada")
            {
                flag = true;
            }
        }
        catch (Exception ex)
        {
            flag = false;
        }
        return flag;
    }


    //private void SetupCanadaCompanyUI()
    //{
    //    BL_General objBL_General = new BL_General();
    //    General objGenerals = new General();

    //    if (Convert.ToBoolean(ViewState["isCanada"]))
    //    {
    //        RadGrid_gvInvoices.Columns.FindByUniqueName("STaxAmt").HeaderText = "HST/PST Tax";
    //        RadGrid_gvInvoices.Columns.FindByUniqueName("GTaxAmt").Visible = true;
    //        RadGrid_gvInvoices.Columns.FindByUniqueName("EnableGSTTax").Visible = true;
    //        RadGrid_gvInvoices.Columns.FindByUniqueName("EnableGSTTax").HeaderText = "GST";

    //    }
    //    else
    //    {
    //        RadGrid_gvInvoices.Columns.FindByUniqueName("stax").HeaderText = "Taxable";
    //        RadGrid_gvInvoices.Columns.FindByUniqueName("STaxAmt").HeaderText = "Sales Tax Amount";
    //        RadGrid_gvInvoices.Columns.FindByUniqueName("GTaxAmt").Visible = false;
    //        RadGrid_gvInvoices.Columns.FindByUniqueName("EnableGSTTax").Visible = false;
    //    }

    //}

    protected void lnkPreviewEmailInvTicket_Click(object sender, EventArgs e)
    {
        try
        {
            String page = "";
            if (Request.QueryString["page"] != null)
            {
                page = "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl);
            }

            var reportFormat = ConfigurationManager.AppSettings["InvoiceReportFormat"].ToString();
            if (Request.QueryString["uid"] != null)
            {
                if (Request.QueryString["c"] != null && Request.QueryString["c"] == "1")
                {
                    Submit(2);
                }
                else
                {
                    if (imgPaid.Visible == false && btnSubmit.Visible == true)
                    {
                        Submit(2);
                    }

                    if (reportFormat.ToUpper().Equals("MRT"))
                    {
                        Response.Redirect("PreviewInvoiceWithTicket.aspx?uid=" + Request.QueryString["uid"].ToString() + page);
                    }
                    else if (reportFormat.ToUpper().Equals("RDLC"))
                    {
                        Response.Redirect("PrintInvoiceWithTicket.aspx?uid=" + Request.QueryString["uid"].ToString() + page);
                    }
                }
            }
            else
            {
                if (txtProject.Text != string.Empty)
                {
                    foreach (GridDataItem gr in RadGrid_gvInvoices.Items)
                    {
                        DropDownList ddlProjectCode = (DropDownList)gr.FindControl("ddlProjectCode");
                        // DropDownList ddlBillingCode = (DropDownList)gr.FindControl("ddlBillingCode");
                        HiddenField txtBillingCodeID = (HiddenField)gr.FindControl("txtBCodeID");
                        TextBox lblQuantity = (TextBox)gr.FindControl("lblQuantity");
                        if (ddlProjectCode.SelectedItem.Text == "-Select-" && txtBillingCodeID.Value != string.Empty && lblQuantity.Text != string.Empty && lblQuantity.Text != "0")
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Please select the Code.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                            return;
                        }
                    }
                }

                Submit(2);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr852", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void SetupGrid()
    {
        //todo
        try
        {
            objGenerals.ConnConfig = Session["config"].ToString();

            objGenerals.CustomName = "Country";

            DataSet dsCustom = objBL_General.getCustomFields(objGenerals);

            if (Convert.ToBoolean(ViewState["isCanada"]))
            {

                if (hdnsTaxType.Value == "2")
                {
                    RadGrid_gvInvoices.Columns.FindByUniqueName("stax").HeaderText = "HST";

                    RadGrid_gvInvoices.Columns.FindByUniqueName("stax").Visible = true;

                    RadGrid_gvInvoices.Columns.FindByUniqueName("STaxAmt").HeaderText = "HST Tax";
                    RadGrid_gvInvoices.Columns.FindByUniqueName("STaxAmt").Visible = true;

                    RadGrid_gvInvoices.Columns.FindByUniqueName("GTaxAmt").Visible = false;
                    RadGrid_gvInvoices.Columns.FindByUniqueName("EnableGSTTax").Visible = false;
                }
                else
                {


                    RadGrid_gvInvoices.Columns.FindByUniqueName("stax").HeaderText = "PST";
                    RadGrid_gvInvoices.Columns.FindByUniqueName("stax").Visible = true;

                    RadGrid_gvInvoices.Columns.FindByUniqueName("STaxAmt").HeaderText = "PST Tax";
                    RadGrid_gvInvoices.Columns.FindByUniqueName("STaxAmt").Visible = true;


                    RadGrid_gvInvoices.Columns.FindByUniqueName("GTaxAmt").Visible = true;
                    RadGrid_gvInvoices.Columns.FindByUniqueName("GTaxAmt").HeaderText = "GST Tax";

                    RadGrid_gvInvoices.Columns.FindByUniqueName("EnableGSTTax").HeaderText = "GST";
                    RadGrid_gvInvoices.Columns.FindByUniqueName("EnableGSTTax").Visible = true;
                    hdnGstTax.Value = dsCustom.Tables[0].Rows[0]["GstRate"].ToString();
                }

            }
            else
            {
                RadGrid_gvInvoices.Columns.FindByUniqueName("GTaxAmt").Visible = false;
                RadGrid_gvInvoices.Columns.FindByUniqueName("stax").HeaderText = "Taxable";
                RadGrid_gvInvoices.Columns.FindByUniqueName("stax").Visible = true;
                RadGrid_gvInvoices.Columns.FindByUniqueName("STaxAmt").HeaderText = "Sales Tax Amount";
                RadGrid_gvInvoices.Columns.FindByUniqueName("STaxAmt").Visible = true;
                RadGrid_gvInvoices.Columns.FindByUniqueName("EnableGSTTax").Visible = false;

            }
            RadGrid_gvInvoices.Rebind();
            foreach (GridHeaderItem headerItem in RadGrid_gvInvoices.MasterTableView.GetItems(GridItemType.Header))
            {
                //todo
                if (!Convert.ToBoolean(ViewState["isCanada"]))
                {

                    GridHeaderItem gd = headerItem;
                    CheckBox chk = (CheckBox)gd.FindControl("chkSelectAllStax");
                    Label lbPSTHeader = (Label)gd.FindControl("lbPSTHeader");

                    if (chk != null)
                    {
                        chk.Visible = false;
                        lbPSTHeader.Text = "Taxable";
                    }

                }
                else
                {
                    GridHeaderItem gd = headerItem;
                    Label lbPSTHeader = (Label)gd.FindControl("lbPSTHeader");
                    if (hdnsTaxType.Value == "2")
                    {
                        lbPSTHeader.Text = "HST";
                    }
                    else
                    {
                        lbPSTHeader.Text = "PST";
                    }
                    if (hdnsTaxType.Value == "3")
                    {
                        CheckBox chkSelectAllGtax = (CheckBox)gd.FindControl("chkSelectAllGtax");
                        CheckBox chk = (CheckBox)gd.FindControl("chkSelectAllStax");

                        if (chk != null)
                        {
                            chk.Enabled = false;
                        }
                        if (chkSelectAllGtax != null)
                        {
                            chkSelectAllGtax.Enabled = false;
                        }
                    }
                }

            }



            foreach (GridDataItem item in RadGrid_gvInvoices.MasterTableView.Items)
            {
                if (Convert.ToBoolean(ViewState["isCanada"]))
                {
                    if (hdnsTaxType.Value == "3")
                    {
                        GridItem gd = item;
                        CheckBox chkTaxable = (CheckBox)gd.FindControl("chkTaxable");
                        CheckBox chkEnableGSTTax = (CheckBox)gd.FindControl("chkEnableGSTTax");
                        if (chkTaxable != null)
                        {
                            chkTaxable.Checked = false;
                            chkTaxable.Enabled = false;

                        }
                        if (chkEnableGSTTax != null)
                        {
                            chkEnableGSTTax.Checked = false;
                            chkEnableGSTTax.Enabled = false;
                        }
                    }
                }


            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr11", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }

    private void SetupNonCanadaGrid()
    {
        //todo
        try
        {
            objGenerals.ConnConfig = Session["config"].ToString();

            objGenerals.CustomName = "Country";

            DataSet dsCustom = objBL_General.getCustomFields(objGenerals);

            if (!Convert.ToBoolean(ViewState["isCanada"]))
            {
                RadGrid_gvInvoices.Columns.FindByUniqueName("GTaxAmt").Visible = false;
                RadGrid_gvInvoices.Columns.FindByUniqueName("stax").HeaderText = "Taxable";
                RadGrid_gvInvoices.Columns.FindByUniqueName("stax").Visible = true;
                RadGrid_gvInvoices.Columns.FindByUniqueName("STaxAmt").HeaderText = "Sales Tax Amount";
                RadGrid_gvInvoices.Columns.FindByUniqueName("STaxAmt").Visible = true;
                RadGrid_gvInvoices.Columns.FindByUniqueName("EnableGSTTax").Visible = false;

            }

            RadGrid_gvInvoices.Rebind();
            foreach (GridHeaderItem headerItem in RadGrid_gvInvoices.MasterTableView.GetItems(GridItemType.Header))
            {
                //todo
                if (!Convert.ToBoolean(ViewState["isCanada"]))
                {

                    GridHeaderItem gd = headerItem;
                    CheckBox chk = (CheckBox)gd.FindControl("chkSelectAllStax");
                    Label lbPSTHeader = (Label)gd.FindControl("lbPSTHeader");

                    if (chk != null)
                    {
                        chk.Visible = false;
                        lbPSTHeader.Text = "Taxable";
                    }
                }

            }



        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr11", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }

    protected void RadGrid_EmailLogs_ItemCreated(object sender, GridItemEventArgs e)
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

    protected void RadGrid_EmailLogs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["uid"]))
        {
            RadGrid_EmailLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupEmailLogs();
            DataTable dtEmailLog = new DataTable();
            EmailLog emailLog = new EmailLog();
            BL_EmailLog bL_EmailLog = new BL_EmailLog();

            emailLog.ConnConfig = Session["config"].ToString();
            emailLog.Ref = Convert.ToInt32(Request.QueryString["uid"]);
            DataSet dsLog = bL_EmailLog.GetEmailLogsForInvoices(emailLog);
            dtEmailLog = dsLog.Tables[0];

            //emailLog.Screen = "Invoice";
            //emailLog.ConnConfig = Session["config"].ToString();
            //emailLog.Ref = Convert.ToInt32(Request.QueryString["uid"]);
            //DataSet dsLog = bL_EmailLog.GetEmailLogs(emailLog);

            //dtEmailLog = dsLog.Tables[0];

            //emailLog.Screen = "Collections";
            //emailLog.Function = "Invoice All";
            //emailLog.ConnConfig = Session["config"].ToString();
            //emailLog.Ref = Convert.ToInt32(Request.QueryString["uid"]);
            //DataSet dsLog1 = bL_EmailLog.GetEmailLogs(emailLog);

            //dtEmailLog.Merge(dsLog1.Tables[0]);

            //emailLog.Screen = "Collections";
            //emailLog.Function = "Invoice Selected";
            //emailLog.ConnConfig = Session["config"].ToString();
            //emailLog.Ref = Convert.ToInt32(Request.QueryString["uid"]);
            //DataSet dsLog2 = bL_EmailLog.GetEmailLogs(emailLog);

            //dtEmailLog.Merge(dsLog2.Tables[0]);

            if (dtEmailLog.Rows.Count > 0)
            {
                var userinfo = (DataTable)Session["userinfo"];
                int usertypeid = 0;
                if (userinfo != null)
                {
                    usertypeid = Convert.ToInt32(userinfo.Rows[0]["usertypeid"]);
                }

                if (usertypeid == 2)
                {
                    RadGrid_EmailLogs.DataSource = string.Empty;
                }
                else
                {
                    RadGrid_EmailLogs.VirtualItemCount = dtEmailLog.Rows.Count;
                    RadGrid_EmailLogs.DataSource = dtEmailLog;
                }
            }
            else
            {
                RadGrid_EmailLogs.DataSource = string.Empty;
            }
        }
        else
        {
            RadGrid_EmailLogs.DataSource = string.Empty;
        }
    }

    bool isGroupEmailLog = false;
    public bool ShouldApplySortFilterOrGroupEmailLogs()
    {
        return RadGrid_EmailLogs.MasterTableView.FilterExpression != "" ||
            (RadGrid_EmailLogs.MasterTableView.GroupByExpressions.Count > 0 || isGroupEmailLog) ||
            RadGrid_EmailLogs.MasterTableView.SortExpressions.Count > 0;
    }
}
