using AjaxControlToolkit;
using BusinessEntity;
using BusinessLayer;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PrintPO : System.Web.UI.Page
{
    #region Variable
    GeneralFunctions objgn = new GeneralFunctions();
    PO _objPO = new PO();
    BL_Bills _objBLBills = new BL_Bills();
    ApprovePOStatus _objApprovePOStatus = new ApprovePOStatus();
    BusinessEntity.User objPropUser = new BusinessEntity.User();
    BL_User objBL_User = new BL_User();
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
                _objApprovePOStatus.ConnConfig = Session["config"].ToString();
                _objApprovePOStatus.POIDs = Request.QueryString["id"];
                _objApprovePOStatus.UserID = Convert.ToInt32(Session["userid"]);

                DataSet DS = _objBLBills.GetVenderDetailsForMailALL(_objApprovePOStatus);
                if (DS.Tables[0].Rows.Count > 0)
                {
                    if (DS.Tables[0].Rows[0]["Email"] != null || DS.Tables[0].Rows[0]["Email"].ToString() != string.Empty)
                    {
                        txtTo.Text = Convert.ToString(DS.Tables[0].Rows[0]["Email"]);
                    }
                }

                DisplayPOReport();
                GetSMTPUser();
                FillDistributionList("0", "");

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("addpo.aspx?id=" + Request.QueryString["id"]);
    }
    protected void hideModalPopupViaServerConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            string POID = Request.QueryString["id"];
            if (POID != "")
            {
                byte[] buffer = null;

                ReportViewer rvPO = new ReportViewer();
                PrintPOReport(rvPO, Convert.ToInt32(POID));
                buffer = ExportReportToPDF("", rvPO);
                string FileName = "POReport_" + POID + ".pdf";
                SendMail(buffer, FileName);
                //this.programmaticModalPopup.Hide();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'No records are available to send a mail',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception exp)
        {
            //string str = exp.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(exp.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void PrintPOReport(ReportViewer rvPO, int PO_ID)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();
        _objPO.ConnConfig = Session["config"].ToString();
        _objPO.POID = Convert.ToInt32(PO_ID);
        DataSet ds = _objBLBills.GetPOById(_objPO);

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


        DataSet dsTerm = _objBLBills.GetAddPOTerms(_objPO);
        if (dsTerm.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].Rows[0]["TC"] = dsTerm.Tables[0].Rows[0]["TermsConditions"].ToString();
        }

        string vendorAdd = ds.Tables[0].Rows[0]["VendorAddress"].ToString() + Environment.NewLine;
        vendorAdd += ds.Tables[0].Rows[0]["VendorCity"].ToString() + ", " + ds.Tables[0].Rows[0]["VendorState"].ToString() + ", " + ds.Tables[0].Rows[0]["VendorZip"].ToString();
        ds.Tables[0].Rows[0]["Address"] = vendorAdd;
        rvPO.LocalReport.DataSources.Clear();
        rvPO.LocalReport.DataSources.Add(new ReportDataSource("dsPO", ds.Tables[0]));
        rvPO.LocalReport.DataSources.Add(new ReportDataSource("dsPOItem", ds.Tables[1]));
        rvPO.LocalReport.DataSources.Add(new ReportDataSource("dsTicket", dsC.Tables[0]));
        string reportPath;
        if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("QAE"))
            reportPath = "Reports/QAEPOReport.rdlc";
        else if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("A4Access"))
            reportPath = "Reports/A4AccessPOReport.rdlc";
        else
            reportPath = "Reports/POReport.rdlc";

        rvPO.LocalReport.ReportPath = reportPath;
        rvPO.LocalReport.EnableExternalImages = true;

        List<ReportParameter> param1 = new List<ReportParameter>();
        string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
        param1.Add(new ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
        rvPO.LocalReport.SetParameters(param1);
        rvPO.LocalReport.Refresh();
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
    protected void SendMail(byte[] buffer, string FileName)
    {
        Mail mail = new Mail();
        mail.From = txtFrom.Text == "" ? "info@mom.com" : txtFrom.Text;
        mail.To = txtTo.Text.ToString().Split(';', ',').OfType<string>().ToList();
        mail.Cc = txtCC.Text.ToString().Split(';', ',').OfType<string>().ToList();
        mail.Title = txtSubject.Text;
        mail.Text = txtBody.Text;
        mail.FileName = FileName;
        if (hdnFirstAttachement.Value != "-1")
        {
            mail.attachmentBytes = buffer;
            mail.DeleteFilesAfterSend = true;
        }
        mail.RequireAutentication = false;

        ArrayList lst = new ArrayList();
        if (ViewState["pathmailatt"] != null)
        {
            string _fileName = "POReport_" + Request.QueryString["id"] + ".pdf";
            lst = (ArrayList)ViewState["pathmailatt"];
            foreach (string strpath in lst)
            {
                if (strpath != _fileName)
                {
                    mail.AttachmentFiles.Add(strpath);
                }
            }
        }

        WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
        mail.Send();
    }
    #endregion

    #region Custom function
    private void DisplayPOReport()
    {
        try
        {
            _objPO.ConnConfig = Session["config"].ToString();
            _objPO.POID = Convert.ToInt32(Request.QueryString["id"]);
            #region Company Check
            _objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());
            if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            {
                _objPO.EN = 1;
            }
            else
            {
                _objPO.EN = 0;
            }
            #endregion
            DataSet ds = _objBLBills.GetPOById(_objPO);

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
                // ES-33:Task#2: commented
                //if (Session["MSM"].ToString() != "TS")
                //{
                //    if (txtFrom.Text.Trim() == string.Empty)
                //    {
                //        txtFrom.Text = dsC.Tables[0].Rows[0]["Email"].ToString();
                //    }
                //}
                //txtFrom.Text = "info@mom.com";
                // ES-33:Task#2: Added new line
                txtFrom.Text = WebBaseUtility.GetFromEmailAddress();
                txtSubject.Text = ConfigurationManager.AppSettings["CustomerName"] + " PO Report - " + Request.QueryString["id"];
                string address = string.Empty;
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["name"])))
                {
                    address += dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine + "</br>";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["Address"])))
                {
                    address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["city"])))
                {
                    address += dsC.Tables[0].Rows[0]["city"].ToString() + ", ";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["state"])))
                {
                    address += dsC.Tables[0].Rows[0]["state"].ToString() + ", ";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["zip"])))
                {
                    address += dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine + "</br>";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["Phone"])))
                {
                    address += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine + "</br>";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["fax"])))
                {
                    address += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine + "</br>";
                }
                if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["email"])))
                {
                    if (!ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("QAE"))
                        address += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine + "<br />";
                }

                address = "Please review the attached Purchase Order from: " + Environment.NewLine + "<br />" + Environment.NewLine + "<br />" + address;
                ViewState["company"] = address;
                txtBody.Text = address;
            }

            DataSet dsTerm = _objBLBills.GetAddPOTerms(_objPO);
            if (dsTerm.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Rows[0]["TC"] = dsTerm.Tables[0].Rows[0]["TermsConditions"].ToString();
            }

            string vendorAdd = ds.Tables[0].Rows[0]["VendorAddress"].ToString() + Environment.NewLine;
            vendorAdd += ds.Tables[0].Rows[0]["VendorCity"].ToString() + ", " + ds.Tables[0].Rows[0]["VendorState"].ToString() + ", " + ds.Tables[0].Rows[0]["VendorZip"].ToString();
            ds.Tables[0].Rows[0]["Address"] = vendorAdd;
            rvPO.LocalReport.DataSources.Clear();
            rvPO.LocalReport.DataSources.Add(new ReportDataSource("dsPO", ds.Tables[0]));
            rvPO.LocalReport.DataSources.Add(new ReportDataSource("dsPOItem", ds.Tables[1]));
            rvPO.LocalReport.DataSources.Add(new ReportDataSource("dsTicket", dsC.Tables[0]));
            string reportPath;
            if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("QAE"))
                reportPath = "Reports/QAEPOReport.rdlc";
            else
                reportPath = "Reports/POReport.rdlc";

            rvPO.LocalReport.ReportPath = reportPath;
            rvPO.LocalReport.EnableExternalImages = true;

            List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
            string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
            param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
            rvPO.LocalReport.SetParameters(param1);
            rvPO.LocalReport.Refresh();

            string POID = Request.QueryString["id"];
            if (POID != "")
            {
                string FileName = "POReport_" + POID + ".pdf";
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
            throw ex;
        }
    }
    private byte[] ExportReportToPDF(string reportName)
    {
        Warning[] warnings;
        string[] streamids;
        string mimeType;
        string encoding;
        string filenameExtension;
        byte[] bytes = rvPO.LocalReport.Render(
            "PDF", null, out mimeType, out encoding, out filenameExtension,
             out streamids, out warnings);

        return bytes;
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

    //        mail.InUsername = dsEmailacc.Tables[0].Rows[0]["InUsername"].ToString();
    //        mail.InPassword = dsEmailacc.Tables[0].Rows[0]["InPassword"].ToString();
    //        mail.InHost = dsEmailacc.Tables[0].Rows[0]["InServer"].ToString();
    //        mail.InPort = Convert.ToInt32(dsEmailacc.Tables[0].Rows[0]["InPort"].ToString());
    //    }
    //}

    //private string GetFromEmailAddress()
    //{
    //    string strFrom = string.Empty;
    //    General _objGeneral = new General();
    //    _objGeneral.ConnConfig = Session["config"].ToString();
    //    _objGeneral.userid = Convert.ToInt32(Session["UserID"]);
    //    BL_General _objBL_General = new BL_General();
    //    DataSet dsEmailacc = _objBL_General.GetEmailAcc(_objGeneral);
    //    if (dsEmailacc.Tables[0].Rows.Count > 0)
    //    {
    //        strFrom = dsEmailacc.Tables[0].Rows[0]["OutUsername"].ToString();
    //    }
    //    else
    //    {
    //        System.Configuration.Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
    //        MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
    //        string username = mailSettings.Smtp.Network.UserName;
    //        strFrom = username;
    //    }

    //    return strFrom;
    //}
    #endregion

    protected void btnPrintReport_Click(object sender, EventArgs e)
    {
        byte[] array = null;
        List<byte[]> lstbyte = new List<byte[]>();
        try
        {
            DataTable dt = new DataTable();

            DataTable dtPO = (DataTable)Session["PO"];
            if (dtPO.Rows.Count > 0)
            {

                int PIID = Convert.ToInt32(Request.QueryString["id"]);
                ReportViewer rvPO = new ReportViewer();

                PrintPOReport(rvPO, PIID);

                array = ExportReportToPDF("", rvPO);
                lstbyte.Add(array);


                byte[] allbyte = ManagePO.concatAndAddContent(lstbyte);
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Buffer = true;
                Response.AddHeader("Content-Disposition", "attachment;filename=POReport.pdf");
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Length", (allbyte.Length).ToString());
                Response.BinaryWrite(allbyte);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No Record found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex) { }
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

    //protected void radOpen_Click(object sender, EventArgs e)
    //{
    //    //business logic goes here

    //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "OPenPopupWindow", "OpenEmailsSelectionWindow();", true);
    //}
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
    }

    //protected override void OnPreRender(EventArgs e)
    //{
    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "pageLoadHandler", startscript + endscript, false);

    //    base.OnPreRender(e);
    //}

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
            string POID = Request.QueryString["id"];
            string FileName = "POReport_" + POID + ".pdf";
            if (DownloadFileName == FileName)
            {
                byte[] buffer = null;

                if (POID != "")
                {
                    ReportViewer rvPO = new ReportViewer();
                    PrintPOReport(rvPO, Convert.ToInt32(POID));
                    buffer = ExportReportToPDF("", rvPO);

                    Response.Clear();
                    MemoryStream ms = new MemoryStream(buffer);
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
                    Response.Buffer = true;
                    ms.WriteTo(Response.OutputStream);
                    Response.End();
                }
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