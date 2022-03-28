using BusinessEntity;
using BusinessLayer;
using Stimulsoft.Report;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;


public partial class EditReportTemplate : System.Web.UI.Page
{

    #region Variables
    BL_ReportsData objBL_ReportsData = new BL_ReportsData();

    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    BusinessEntity.User objPropUser = new BusinessEntity.User();
    BL_User objBL_User = new BL_User();

    GeneralFunctions objGeneralFunction = new GeneralFunctions();

    Journal _objJe = new Journal();
    Transaction _objTrans = new Transaction();
    BL_JournalEntry _objBLJe = new BL_JournalEntry();

    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();

    int count_inv = 0;
    bool _filteredbyloc = false;

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();
    BL_Report bL_Report = new BL_Report();

    bool IsGst = false;

    byte[] buffer = null;
    byte[] array = null;
    
    #endregion
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
                        string redirect = "HTTPS://" + URL + "/EditReportTemplate.aspx";
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
            ViewState["IncDays"] = 0;
            #region Check IsGstRate
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
            #endregion
            ViewState["IsGst"] = IsGst;
            ViewState["pay"] = 0;

            if (!IsPostBack)
            {
                string InvoicesCheckpath = Server.MapPath("StimulsoftReports/Invoices/");
                DirectoryInfo dirPath = new DirectoryInfo(InvoicesCheckpath);
                FileInfo[] Files = dirPath.GetFiles("*.mrt");
                foreach (FileInfo file in Files)
                {
                    string FileName = string.Empty;
                    if (file.Name.Contains(".mrt"))
                        FileName = file.Name.Replace(".mrt", " ");
                    ddlInvoicesForLoad.Items.Add((FileName));
                }


                ddlInvoicesForLoad.DataBind();
                ddlInvoicesForLoad.SelectedIndex = 0;
                string reportName = ddlInvoicesForLoad.SelectedItem.Text.Trim();

                Session["Report"] = reportName;


                StiReport report = FillDataSetToReport(reportName);

                StiWebViewerInvoice.Report = report;
                //  ReportModalPopupExtender.Show();
                StiWebViewerInvoice.Visible = true;
            }
        }


    }

    public StiReport FillDataSetToReport(string reportName)
    {
        StiReport report = new StiReport();
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

            var _dr = dtNew.Rows[0];
            //foreach (DataRow _dr in dtNew.Rows)
            //{
                int _ref = Convert.ToInt32(_dr["Ref"]);

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
                if (dtNew.Rows.Count > 0)
                    //for (int i = 0; i < _dtInvoice.Rows.Count; i++)
                    {
                    int i = 0;

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
                //}
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

                //rvInvoices.LocalReport.DataSources.Clear();
                DataTable dt = (DataTable)ViewState["InvoiceReport"];
                DataTable dtCompany = (DataTable)ViewState["CompanyReport"];
                int refId = Convert.ToInt32(dt.Rows[count_inv]["Ref"]);
                DataTable _dtInvItems1 = GetInvoiceItems(refId);


                string reportPathStimul = string.Empty;
                reportPathStimul = Server.MapPath("StimulsoftReports/Invoices/" + ddlInvoicesForLoad.SelectedItem.Text.Trim() + ".mrt");
                // string reportPathStimul = Server.MapPath("StimulsoftReports/Invoices.mrt");

                report.Load(reportPathStimul);
                report.Compile();

                DataSet companyLogo = new DataSet();
                companyLogo = bL_Report.GetCompanyDetails(Session["config"].ToString());
                var imageString = companyLogo.Tables[0].Rows[0]["Logo"].ToString();
                byte[] barrImg = (byte[])(companyLogo.Tables[0].Rows[0]["Logo"]);
                string strfn = Convert.ToString(Server.MapPath(Request.ApplicationPath) + "/TempImages/" + DateTime.Now.ToFileTime().ToString());
                FileStream fs = new FileStream(strfn,
                                  FileMode.CreateNew, FileAccess.Write);
                fs.Write(barrImg, 0, barrImg.Length);
                fs.Flush();
                fs.Close();

                System.Uri uri = new Uri(strfn);
                DataTable cTable = BuildCompanyDetailsTable();
                var cRow = cTable.NewRow();
                cRow["LogoURL"] = uri.AbsolutePath;
                cRow["CompanyName"] = companyLogo.Tables[0].Rows[0]["Name"].ToString();
                cRow["CompanyAddress"] = companyLogo.Tables[0].Rows[0]["Address"].ToString();
                cRow["ContactNo"] = companyLogo.Tables[0].Rows[0]["Contact"].ToString();
                cRow["Email"] = companyLogo.Tables[0].Rows[0]["Email"].ToString();

                cRow["City"] = companyLogo.Tables[0].Rows[0]["City"].ToString();
                cRow["State"] = companyLogo.Tables[0].Rows[0]["State"].ToString();
                cRow["Phone"] = companyLogo.Tables[0].Rows[0]["Phone"].ToString();
                cRow["Fax"] = companyLogo.Tables[0].Rows[0]["Fax"].ToString();
                cRow["Zip"] = companyLogo.Tables[0].Rows[0]["Zip"].ToString();


                cTable.Rows.Add(cRow);

                DataSet CompanyDetails = new DataSet();
                cTable.TableName = "CompanyDetails";
                CompanyDetails.Tables.Add(cTable);
                CompanyDetails.DataSetName = "CompanyDetails";


                DataSet Invoices = new DataSet();
                DataTable dtInvoice1 = _dtInvoice.Copy();
                dtInvoice1.TableName = "Invoices";
                Invoices.Tables.Add(dtInvoice1.Copy());
                Invoices.DataSetName = "Invoices";

                DataSet InvoiceItems = new DataSet();
                DataTable dtIInvItems = _dtInvItems1.Copy();
                dtIInvItems.TableName = "InvoiceItems";
                InvoiceItems.Tables.Add(dtIInvItems);
                InvoiceItems.DataSetName = "InvoiceItems";


                DataSet Ticket_Company = new DataSet();
                DataTable dtTicketCompany = new DataTable();
                dtTicketCompany = dsC.Tables[0].Copy();
                Ticket_Company.Tables.Add(dtTicketCompany);
                dtTicketCompany.TableName = "Ticket_Company";
                Ticket_Company.DataSetName = "Ticket_Company";


                DataSet Invoice_dtInvoice = new DataSet();
                DataTable dtInvoice = new DataTable();
                dtInvoice = ds.Tables[0].Copy();
                Invoice_dtInvoice.Tables.Add(dtInvoice);
                dtInvoice.TableName = "Invoice_dtInvoice";
                Invoice_dtInvoice.DataSetName = "Invoice_dtInvoice";

                report.RegData("Invoices", Invoices);
                report.RegData("CompanyDetails", CompanyDetails);

                report.RegData("Invoice_dtInvoice", Invoice_dtInvoice);

                report.RegData("Ticket_Company", Ticket_Company);
                report.RegData("InvoiceItems", InvoiceItems);
                report.Dictionary.Synchronize();
                report.Render();
                
            }
            if(dtNew.Rows.Count == 0){
                string reportPathStimul = string.Empty;
                
                    reportPathStimul = Server.MapPath("StimulsoftReports/Invoices/"+ ddlInvoicesForLoad.SelectedItem.Text.Trim() +".mrt");
                
                // string reportPathStimul = Server.MapPath("StimulsoftReports/Invoices.mrt");

                report.Load(reportPathStimul);
                report.Compile();
                report.Render();
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
    protected void lnkSaveAsDefault_Click(object sender, EventArgs e)
    {
        try
        {
            string defaultpath = Server.MapPath("StimulsoftReports/Invoices/" + ConfigurationManager.AppSettings["InvoiceReport"].ToString());
            string filePath = Server.MapPath("StimulsoftReports/Invoices");
            string tempPath = Server.MapPath("StimulsoftReports/Invoices");
            string selValue = ddlInvoicesForLoad.SelectedItem.Text.TrimEnd();
            if (selValue != null)
            {
                filePath = filePath + "\\" + selValue + ".mrt";
                tempPath = tempPath + "\\" + selValue + "temp.mrt";
                if (File.Exists(defaultpath))
                {
                    string[] lines = System.IO.File.ReadAllLines(defaultpath);
                    var myfile = File.Create(tempPath);
                    myfile.Close();
                    using (TextWriter tw = new StreamWriter(tempPath))
                        foreach (string line in lines)
                        {
                            tw.WriteLine(line);
                        }
                    File.Delete(defaultpath);
                    if (File.Exists(filePath))
                    {
                        string[] lines1 = System.IO.File.ReadAllLines(filePath);
                        var myfile1 = File.Create(defaultpath);
                        myfile1.Close();
                        using (TextWriter tw1 = new StreamWriter(defaultpath))
                            foreach (string line1 in lines1)
                            {
                                tw1.WriteLine(line1);
                            }
                        File.Delete(filePath);
                    }
                    if (File.Exists(tempPath))
                    {
                        string[] lines2 = System.IO.File.ReadAllLines(tempPath);
                        var myfile2 = File.Create(filePath);
                        myfile2.Close();
                        using (TextWriter tw2 = new StreamWriter(filePath))
                            foreach (string line2 in lines2)
                            {
                                tw2.WriteLine(line2);
                            }
                        File.Delete(tempPath);
                    }
                    Response.Redirect("Invoices.aspx");
                    string str = "Template " + selValue + " Saved Successfully!--";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Invoice# " + str + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
                else
                    throw new Exception("Invoices.mrt is not available");
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }

    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        string filePath = Server.MapPath("StimulsoftReports/Invoices");
        string selValue = ddlInvoicesForLoad.SelectedItem.Text.Trim();
        if (selValue != null)
        {
            filePath = filePath + "\\" + selValue + ".mrt";
            if (!selValue.Equals("InvoicesDefault"))
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                ddlInvoicesForLoad.Items.Clear();

                string Invoicepath = Server.MapPath("StimulsoftReports/Invoices/");
                DirectoryInfo dirPath = new DirectoryInfo(Invoicepath);
                FileInfo[] Files = dirPath.GetFiles("*.mrt");
                foreach (FileInfo file in Files)
                {
                    string FileName = string.Empty;
                    if (file.Name.Contains(".mrt"))
                        FileName = file.Name.Replace(".mrt", " ");
                    ddlInvoicesForLoad.Items.Add((FileName));
                }

                ddlInvoicesForLoad.DataBind();


                string str = "Template " + selValue + " Deleted!--";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'EditReportInvoice# " + str + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
    }

    protected void lnkCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("EditReportTemplate.aspx");
    }

    protected void StiWebDesigner1_SaveReport(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/Invoices/" + e.FileName));

        string str = "Template " + e.FileName + " Saved Successfullly!--";
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'EditReportInvoice# " + str + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
    }
    protected void StiWebDesigner1_SaveReportAs(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/Invoices/" + e.FileName));

        string str = "Template " + e.FileName + " Save As Successfullly!--";
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'EditReportInvoice# " + str + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
    }
    protected void StiWebDesigner1_Exit(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        Response.Redirect("EditReportTemplate.aspx");
    }

    protected void ddlInvoicesForLoad_SelectedIndexChanged(object sender, EventArgs e)
    {


        string reportName = ddlInvoicesForLoad.SelectedItem.Text.Trim();
        StiReport report = FillDataSetToReport(reportName);

        StiWebViewerInvoice.Report = report;
        StiWebViewerInvoice.Visible = true;
       
        StiWebDesigner1.Visible = false;
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("invoices.aspx?fil=1");
    }

    protected void StiWebViewerInvoice_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        //ddlInvoicesForLoad.SelectedIndex = 1;
        string reportName = Session["Report"].ToString();
        StiReport report = FillDataSetToReport(reportName);

        e.Report = report;

        StiWebDesigner1.Visible = false;
        StiWebViewerInvoice.Visible = true;
    }

    protected void StiWebViewerInvoice_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        //ddlInvoicesForLoad.SelectedIndex = 1;
        string reportName = Session["Report"].ToString();
        StiReport report = FillDataSetToReport(reportName);

        e.Report = report;

        StiWebDesigner1.Visible = false;
        StiWebViewerInvoice.Visible = true;
    }

    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        if (StiWebViewerInvoice.Visible)
        {
            string reportName = ddlInvoicesForLoad.SelectedItem.Text.Trim();
            StiReport report = FillDataSetToReport(reportName);

            StiWebDesigner1.Report = report;

            StiWebDesigner1.Visible = true;
            StiWebViewerInvoice.Visible = false;
        }
        else
        {
            string reportName = ddlInvoicesForLoad.SelectedItem.Text.Trim();
            StiReport report = FillDataSetToReport(reportName);

            StiWebViewerInvoice.Report = report;

            StiWebDesigner1.Visible = false;
            StiWebViewerInvoice.Visible = true;
        }
    }

    protected void lnkPreview_Click(object sender, EventArgs e)
    {
        string reportName = ddlInvoicesForLoad.SelectedItem.Text.Trim();
        StiReport report = FillDataSetToReport(reportName);

        StiWebViewerInvoice.Report = report;

        StiWebDesigner1.Visible = false;
        StiWebViewerInvoice.Visible = true;
    }
}