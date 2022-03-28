using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.Net.Configuration;
using System.Configuration;

public partial class PrintTicket : System.Web.UI.Page
{
    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    General objPropGeneral = new General();
    BL_General objBL_General = new BL_General();

    GeneralFunctions objGen = new GeneralFunctions();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        if (Request.QueryString["popup"] != null)
        {
            Page.MasterPageFile = "popup.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            //string jScript;
            //jScript = "<script>window.parent.location.reload(1);</script>";
            //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", jScript);
            Response.Redirect("timeout.htm");
            return;
        }

        if (!IsPostBack)
        {
            if (Request.QueryString["cl"] != null)
            {
                lnkCancelContact.Visible = false;
            }

            if (Session["type"].ToString() == "c")
            {
                chkCoord.Visible = false;
            }

            PrintReport();

        }
    }

    private void PrintReport()
    {
        string subject = string.Empty;
        string Signedby = string.Empty;
        objPropGeneral.ConnConfig = Session["config"].ToString();
        objPropGeneral.CustomName = "Ticket1";

        DataSet ds = new DataSet();
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
        objMapData.ISTicketD = Convert.ToInt32(Request.QueryString["c"].ToString());
        ds = objBL_MapData.GetTicketByID(objMapData);

        if (ds.Tables[0].Rows.Count > 0)
        {
            Signedby = ds.Tables[0].Rows[0]["custom1"].ToString();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["lid"].ToString());
            DataSet dsloc = new DataSet();
            dsloc = objBL_User.getLocationByID(objPropUser);
            if (dsloc.Tables[0].Rows.Count > 0)
            {
                if (Session["MSM"].ToString() != "TS")
                {
                    txtTo.Text = dsloc.Tables[0].Rows[0]["custom14"].ToString();
                    txtCC.Text = dsloc.Tables[0].Rows[0]["custom15"].ToString();
                }
                subject = dsloc.Tables[0].Rows[0]["tag"].ToString();
            }

            ds.Tables[0].Columns.Add("rtaddress");
            ds.Tables[0].Columns.Add("osaddress");
            ds.Tables[0].Columns.Add("ctaddress");
            string tech = ds.Tables[0].Rows[0]["dwork"].ToString();
            string date = ds.Tables[0].Rows[0]["edate"].ToString();
            string time = ds.Tables[0].Rows[0]["timeroute"].ToString();

            if (!string.IsNullOrEmpty(tech.Trim()))
                subject += " - " + tech.ToUpper();

            ViewState["subject"] = subject;
            txtSubject.Text = "Ticket " + Request.QueryString["id"].ToString() + " - " + subject;

            string rtadd = string.Empty;
            string osadd = string.Empty;
            string ctadd = string.Empty;

            if (chkCoord.Checked == true)
            {
                if (time != string.Empty)
                    rtadd = GetAddress(tech, date, time);

                time = ds.Tables[0].Rows[0]["timesite"].ToString();
                if (time != string.Empty)
                    osadd = GetAddress(tech, date, time);

                time = ds.Tables[0].Rows[0]["timecomp"].ToString();
                if (time != string.Empty)
                    ctadd = GetAddress(tech, date, time);
            }


            ds.Tables[0].Rows[0]["rtaddress"] = rtadd;
            ds.Tables[0].Rows[0]["osaddress"] = osadd;
            ds.Tables[0].Rows[0]["ctaddress"] = ctadd;



        }
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Username = Session["username"].ToString();
        txtFrom.Text = WebBaseUtility.GetFromEmailAddress();

        DataSet dsC = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        if (Session["MSM"].ToString() != "TS")
        {
            dsC = objBL_User.getControl(objPropUser);
        }
        else
        {
            objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["lid"].ToString());
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
            address = "Please review the attached ticket from: " + Environment.NewLine + Environment.NewLine + address;
            ViewState["company"] = address;
            txtBody.Text = address;
        }
        if (txtFrom.Text.Trim() == string.Empty)
        {
            System.Configuration.Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
            MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
            string username = mailSettings.Smtp.Network.UserName;
            txtFrom.Text = username;
        }

        DataSet dsEquip = objBL_MapData.getElevByTicket(objMapData);
        DataSet dsOtherWorker = new DataSet();
        ReportViewer1.LocalReport.DataSources.Clear();
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dtEquipDetails", dsEquip.Tables[0]));
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicket", ds.Tables[0]));
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));
        ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtMCP", fillREPHistory()));
        if (ds.Tables.Count > 1)
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtPOItem", ds.Tables[1]));
        if (ds.Tables.Count > 2)
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicketI", ds.Tables[2]));

        string reportPath = "Reports/Ticket.rdlc";
        string Report = System.Web.Configuration.WebConfigurationManager.AppSettings["TicketReport"].Trim();
        if (!string.IsNullOrEmpty(Report.Trim()) && Report.Contains(".rdlc"))
        {
            reportPath = "Reports/" + Report.Trim();
        }

        string RDLC = Request.QueryString["RDLC"] == null ? "" : Request.QueryString["RDLC"].Trim();
        if (RDLC == "Ticket-Adams.rdlc" || RDLC == "TimeSheet_Template.rdlc")
        {
            reportPath = "Reports/" + RDLC;
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
            dsOtherWorker = objBL_MapData.getticketdtlbywday(objMapData);
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dsOtherWorker.Tables[0]));
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", dsOtherWorker.Tables[1]));
        }

        ReportViewer1.LocalReport.ReportPath = reportPath;

        ReportViewer1.LocalReport.EnableExternalImages = true;
        List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
        string logo = "~/companylogo.ashx";
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", logo));
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Custom", Signedby));
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("TicketID", Request.QueryString["id"].ToString()));

        var paras = ReportViewer1.LocalReport.GetParameters();
        var dsCustom1 = GetCustomFields("Loc1");
        if (dsCustom1.Tables[0].Rows.Count > 0 && !string.IsNullOrEmpty(dsCustom1.Tables[0].Rows[0]["label"].ToString()) && paras.FirstOrDefault(x => x.Name == "Custom1Lable") != null)
        {
            param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Custom1Lable", dsCustom1.Tables[0].Rows[0]["label"].ToString()));
        }

        var dsCustom2 = GetCustomFields("Loc2");
        if (dsCustom2.Tables[0].Rows.Count > 0 &&  !string.IsNullOrEmpty(dsCustom2.Tables[0].Rows[0]["label"].ToString()) && paras.FirstOrDefault(x => x.Name == "Custom2Lable") != null)
        {
            param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Custom2Lable", dsCustom2.Tables[0].Rows[0]["label"].ToString()));
        }

        ReportViewer1.LocalReport.SetParameters(param1);
        ReportViewer1.LocalReport.Refresh();
    }

    private DataSet GetCustomFields(string name)
    {
        DataSet ds = new DataSet();
        objPropGeneral.CustomName = name;
        objPropGeneral.ConnConfig = Session["config"].ToString();
        ds = objBL_General.getCustomFields(objPropGeneral);

        return ds;
    }

    private DataTable fillREPHistory()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.EquipID = 0;
        objPropUser.SearchBy = "rd.ticketID";
        objPropUser.SearchValue = Request.QueryString["id"].ToString();

        DataSet ds = new DataSet();
        ds = objBL_User.getequipREPDetails(objPropUser);

        return ds.Tables[0];
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
                mail.Title = txtSubject.Text.Trim(); //"Ticket " + Request.QueryString["id"].ToString() +" - "+ ViewState["subject"].ToString();
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace("\n", "<BR/>");
                }
                else
                {
                    mail.Text = ViewState["company"].ToString().Replace(Environment.NewLine, "<BR/>");
                }
                //mail.AttachmentFiles.Add(ExportReportToPDF("Report_" + objGen.generateRandomString(10) + ".pdf"));
                mail.attachmentBytes = ExportReportToPDF("");
                mail.FileName = "Ticket-" + Request.QueryString["id"].ToString() + ".pdf";

                mail.DeleteFilesAfterSend = true;
                mail.RequireAutentication = false;
                //if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("Adams"))
                //    mail.SendOld();
                //else
                    mail.Send();
                this.programmaticModalPopup.Hide();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

            }
        }
    }

    private string GetAddress(string Tech, string Date, string Time)
    {
        string strResponse = "Location Not Available";
        GeneralFunctions genFunction = new GeneralFunctions();

        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.Tech = Tech;
        objMapData.Date = Convert.ToDateTime(Date);
        objMapData.CallDate = Convert.ToDateTime(Time);
        try
        {
            DataSet dsLoc = objBL_MapData.getlocationAddress(objMapData);

            if (dsLoc.Tables[0].Rows.Count > 0)
            {
                GeoJsonData g = genFunction.GeoRequest(dsLoc.Tables[0].Rows[0]["latitude"].ToString(), dsLoc.Tables[0].Rows[0]["longitude"].ToString());

                if (g.results.Count() > 0)
                {
                    strResponse = g.results[0].formatted_address;
                }
            }
        }
        catch
        {

        }
        return strResponse;
    }

    protected void chkCoord_CheckedChanged(object sender, EventArgs e)
    {
        PrintReport();
    }

    protected void lnkMail_Click(object sender, EventArgs e)
    {
        this.programmaticModalPopup.Show();
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["fr"] != null)
            Response.Redirect("addticket.aspx?id=" + Request.QueryString["id"] + "&comp=0&pop=1&fr="+ Request.QueryString["fr"], false);
        else
        {
            Response.Redirect("addticket.aspx?id=" + Request.QueryString["id"] + "&comp=0&pop=1", false);
        }
    }
}