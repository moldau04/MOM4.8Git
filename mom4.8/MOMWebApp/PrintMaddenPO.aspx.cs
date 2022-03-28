using BusinessEntity;
using BusinessLayer;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class PrintMaddenPO : System.Web.UI.Page
{
    #region Variable
    PO _objPO = new PO();
    BL_Bills _objBLBills = new BL_Bills();

    BusinessEntity.User objPropUser = new BusinessEntity.User();
    BL_User objBL_User = new BL_User();
    ApprovePOStatus _objApprovePOStatus = new ApprovePOStatus();
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
                string FileName = ConfigurationManager.AppSettings["CustomerName"]+ "POReport_" + POID + ".pdf";
                SendMail(buffer, FileName);
                this.programmaticModalPopup.Hide();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'No records are available to send an email',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
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

        if (txtTo.Text.Trim() == string.Empty)
        {
            txtTo.Text = ds.Tables[0].Rows[0]["Email"].ToString();
        }

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

        if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PORemit"].ToString()))
            if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PORemit"].ToString()))
                ds.Tables[0].Rows[0]["PORemit"] = dsC.Tables[0].Rows[0]["Name"].ToString() + Environment.NewLine + dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine +
                                                  dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString();

        string vendorAdd = ds.Tables[0].Rows[0]["VendorAddress"].ToString() + Environment.NewLine;
        vendorAdd += ds.Tables[0].Rows[0]["VendorCity"].ToString() + ", " + ds.Tables[0].Rows[0]["VendorState"].ToString() + ", " + ds.Tables[0].Rows[0]["VendorZip"].ToString();
        ds.Tables[0].Rows[0]["Address"] = vendorAdd;
        rvPO.LocalReport.DataSources.Clear();
        rvPO.LocalReport.DataSources.Add(new ReportDataSource("dsPO", ds.Tables[0]));
        rvPO.LocalReport.DataSources.Add(new ReportDataSource("dsMaddenPOItem", ds.Tables[1]));
        rvPO.LocalReport.DataSources.Add(new ReportDataSource("dsTicket", dsC.Tables[0]));
        string reportPath = "Reports/"+ConfigurationManager.AppSettings["CustomerName"]+"POReport.rdlc";

        rvPO.LocalReport.ReportPath = reportPath;
        rvPO.LocalReport.EnableExternalImages = true;

        List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
        string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
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
        mail.attachmentBytes = buffer;
        mail.DeleteFilesAfterSend = true;
        mail.RequireAutentication = false;
        // ES-33:Task#2: add 
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

            if (txtTo.Text.Trim() == string.Empty)
            {
                txtTo.Text = ds.Tables[0].Rows[0]["Email"].ToString();
            }

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
                // ES-33:Task#2: added new
                txtFrom.Text = WebBaseUtility.GetFromEmailAddress();
                txtSubject.Text = ConfigurationManager.AppSettings["CustomerName"]+" PO Report - " + Request.QueryString["id"];
                string address = dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
                address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
                address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
                address += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
                address += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;
                address += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;
                address = "Please review the attached Purchase Order from: " + Environment.NewLine + Environment.NewLine + address;
                ViewState["company"] = address;
                txtBody.Text = address;
            }

            DataSet dsTerm = _objBLBills.GetAddPOTerms(_objPO);
            if (dsTerm.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].Rows[0]["TC"] = dsTerm.Tables[0].Rows[0]["TermsConditions"].ToString();
            }
            if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PORemit"].ToString()))
                ds.Tables[0].Rows[0]["PORemit"] = dsC.Tables[0].Rows[0]["Name"].ToString() + Environment.NewLine + dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine  +
                                                  dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString();
            string vendorAdd = ds.Tables[0].Rows[0]["VendorAddress"].ToString() + Environment.NewLine;
            vendorAdd += ds.Tables[0].Rows[0]["VendorCity"].ToString() + ", " + ds.Tables[0].Rows[0]["VendorState"].ToString() + ", " + ds.Tables[0].Rows[0]["VendorZip"].ToString();
            ds.Tables[0].Rows[0]["Address"] = vendorAdd;
            string compAddress = dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
            compAddress += dsC.Tables[0].Rows[0]["City"].ToString() + ", " + dsC.Tables[0].Rows[0]["State"].ToString() + ", " + dsC.Tables[0].Rows[0]["Zip"].ToString();
            dsC.Tables[0].Rows[0]["Address"] = compAddress;
            rvPO.LocalReport.DataSources.Clear();
            rvPO.LocalReport.DataSources.Add(new ReportDataSource("dsPO", ds.Tables[0]));
            rvPO.LocalReport.DataSources.Add(new ReportDataSource("dsMaddenPOItem", ds.Tables[1]));
            rvPO.LocalReport.DataSources.Add(new ReportDataSource("dsTicket", dsC.Tables[0]));
            string reportPath = "Reports/"+ConfigurationManager.AppSettings["CustomerName"]+"POReport.rdlc";

            rvPO.LocalReport.ReportPath = reportPath;
            rvPO.LocalReport.EnableExternalImages = true;

            List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
            string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
            param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
            rvPO.LocalReport.SetParameters(param1);
            rvPO.LocalReport.Refresh();
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

    //// ES-33:Task#2: new functions
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
}