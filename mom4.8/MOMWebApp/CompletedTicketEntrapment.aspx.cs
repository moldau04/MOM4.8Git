﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using iTextSharp.text.pdf;
using Stimulsoft.Report;
using Stimulsoft.Report.Web;
using Telerik.Web.UI;

namespace MOMWebApp
{
    public partial class CompletedTicketEntrapment : System.Web.UI.Page
    {

        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        Customer objPropCustomer = new Customer();
        BL_Customer objBL_Customer = new BL_Customer();

        BL_Report bL_Report = new BL_Report();
        BL_Budgets bL_Budgets = new BL_Budgets();

        Chart objChart = new Chart();
        MapData objPropMapData = new MapData();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }

            if (!IsPostBack)
            {
                HighlightSideMenu("schMgr", "lnkListView", "schdMgrSub");

                var levelNames = "";
                if (!string.IsNullOrEmpty(Request["levNames"]))
                {
                    levelNames = HttpUtility.UrlDecode(Request.QueryString["levNames"]);
                }

                lblLevelNames.Text = levelNames;
            }
        }

        protected void lnkClose_Click(object sender, EventArgs e)
        {
            Session.Remove("TicketListFilters");
            Session.Remove("TicketListRadGVFilters");

            if (!string.IsNullOrEmpty(Request["redirect"]))
            {
                if (Request["redirect"].Contains("TicketListView"))
                {
                    Response.Redirect("TicketListView.aspx?fil=1");
                }
                else
                {
                    Response.Redirect(HttpUtility.UrlDecode(Request.QueryString["redirect"]));
                }
            }
            else
            {
                Response.Redirect("home.aspx");
            }
        }

        protected void StiWebViewerCompletedTicket_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
        {
            string reportPathStimul = Server.MapPath("StimulsoftReports/CompletedTicketEntrapment.mrt");
            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            e.Report = report;
        }

        protected void StiWebViewerCompletedTicket_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
        {
            var report = e.Report;
            try
            {
                DataSet dsC = new DataSet();

                var connString = Session["config"].ToString();
                objPropUser.ConnConfig = connString;

                dsC = objBL_User.getControl(objPropUser);

                DataTable cTable = BuildCompanyDetailsTable();
                var cRow = cTable.NewRow();

                DataSet companyInfo = new DataSet();
                companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());

                cRow["CompanyName"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Name"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Name"].ToString();
                cRow["CompanyAddress"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Address"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Address"].ToString();
                cRow["ContactNo"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Contact"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Contact"].ToString();
                cRow["Email"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Email"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Email"].ToString();

                cRow["City"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["City"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["City"].ToString();
                cRow["State"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["State"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["State"].ToString();
                cRow["Phone"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Phone"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Phone"].ToString();
                cRow["Fax"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Fax"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Fax"].ToString();
                cRow["Zip"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Zip"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Zip"].ToString();

                cTable.Rows.Add(cRow);
                report.RegData("CompanyDetails", cTable);

                if (Session["TicketListFilters"] != null)
                {
                    objPropMapData = (BusinessEntity.MapData)Session["TicketListFilters"];
                }

                objPropMapData.ConnConfig = connString;
                if (!string.IsNullOrEmpty(Request["sd"]))
                {
                    var startDate = Convert.ToDateTime(HttpUtility.UrlDecode(Request.QueryString["sd"]));
                    report.Dictionary.Variables["StartDate"].Value = startDate.ToLongDateString();
                    objPropMapData.StartDate = startDate;
                }

                if (!string.IsNullOrEmpty(Request["ed"]))
                {
                    var endDate = Convert.ToDateTime(HttpUtility.UrlDecode(Request.QueryString["ed"]));
                    report.Dictionary.Variables["EndDate"].Value = endDate.ToLongDateString();
                    objPropMapData.EndDate = endDate.AddDays(1).AddSeconds(-1);
                }

                // Get Completed Ticket report from Edit Location screen
                if (!string.IsNullOrEmpty(Request["lid"]))
                {
                    objPropMapData.LocID = Convert.ToInt32(HttpUtility.UrlDecode(Request.QueryString["lid"]));
                }

                // Get Completed Ticket report from Edit Customer screen
                if (!string.IsNullOrEmpty(Request["cid"]))
                {
                    objPropMapData.CustID = Convert.ToInt32(HttpUtility.UrlDecode(Request.QueryString["cid"]));
                }

                // Search text
                if (!string.IsNullOrEmpty(Request["stype"]) && !string.IsNullOrEmpty(Request["stext"]))
                {
                    objPropMapData.SearchBy = HttpUtility.UrlDecode(Request.QueryString["stype"]);
                    objPropMapData.SearchValue = HttpUtility.UrlDecode(Request.QueryString["stext"]);
                }

                List<RetainFilter> filters = new List<RetainFilter>();
                if (Session["TicketListRadGVFilters"] != null)
                {
                    ///Get  rad grid view  search filter value from TicketList View
                    filters = (List<RetainFilter>)Session["TicketListRadGVFilters"];
                }

                var levels = "";
                if (!string.IsNullOrEmpty(Request["lev"]))
                {
                    levels = HttpUtility.UrlDecode(Request.QueryString["lev"]);
                }

                var levelNames = "";
                if (!string.IsNullOrEmpty(Request["levNames"]))
                {
                    levelNames = HttpUtility.UrlDecode(Request.QueryString["levNames"]);
                }

                var ds = bL_Report.GetEntrapmentTickets(objPropMapData, filters, levels);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dtDetail = ds.Tables[0];

                    DataSet dsReportData = new DataSet();
                    DataTable dtData = dtDetail.Copy();
                    dtData.TableName = "ReportData";
                    dsReportData.Tables.Add(dtData);
                    dsReportData.DataSetName = "ReportData";

                    report.RegData("ReportData", dsReportData);
                }


                lblLevelNames.Text = levelNames;
                report.Dictionary.Variables["LevelNames"].Value = levelNames;
                report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

            }
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
                    mail.Title = "Completed Ticket Level Report";
                    if (txtBody.Text.Trim() != string.Empty)
                    {
                        mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                    }
                    else
                    {
                        mail.Text = "This is report email sent from Mobile Office Manager. Please find the Completed Ticket Level Report attached.";
                    }

                    // File attachment
                    StiWebViewer rvTemplate = new StiWebViewer();
                    List<byte[]> poToPrint = PrintTemplate(rvTemplate);

                    if (poToPrint != null && poToPrint.Count > 0)
                    {
                        mail.attachmentBytes = ConcatAndAddContent(poToPrint);
                        mail.FileName = "CompletedTicketLevel.pdf";
                    }

                    mail.DeleteFilesAfterSend = true;
                    mail.RequireAutentication = false;

                    WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                    mail.Send();

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

        private byte[] ExportReportToPDF()
        {
            byte[] bytes = null;
            var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            var service = new Stimulsoft.Report.Export.StiPdfExportService();
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            service.ExportTo(StiWebViewerCompletedTicket.Report, stream, settings);
            bytes = stream.ToArray();

            return bytes;
        }

        public static byte[] ConcatAndAddContent(List<byte[]> pdfByteContent)
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

        private List<byte[]> PrintTemplate(StiWebViewer rvTemplate)
        {
            // Export to PDF
            List<byte[]> templateAsBytes = new List<byte[]>();
            try
            {
                string reportPathStimul = string.Empty;
                reportPathStimul = Server.MapPath("StimulsoftReports/CompletedTicketEntrapment.mrt");
                StiReport report = new StiReport();
                report.Load(reportPathStimul);
                //report.Compile();

                var connString = Session["config"].ToString();
                objPropUser.ConnConfig = connString;

                DataSet dsC = new DataSet();
                dsC = objBL_User.getControl(objPropUser);

                DataTable cTable = BuildCompanyDetailsTable();
                var cRow = cTable.NewRow();

                DataSet companyInfo = new DataSet();
                companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());

                cRow["CompanyName"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Name"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Name"].ToString();
                cRow["CompanyAddress"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Address"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Address"].ToString();
                cRow["ContactNo"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Contact"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Contact"].ToString();
                cRow["Email"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Email"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Email"].ToString();

                cRow["City"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["City"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["City"].ToString();
                cRow["State"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["State"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["State"].ToString();
                cRow["Phone"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Phone"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Phone"].ToString();
                cRow["Fax"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Fax"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Fax"].ToString();
                cRow["Zip"] = string.IsNullOrEmpty(companyInfo.Tables[0].Rows[0]["Zip"].ToString()) ? "" : companyInfo.Tables[0].Rows[0]["Zip"].ToString();

                cTable.Rows.Add(cRow);

                Session["CompanyTable"] = cTable;
                report.RegData("CompanyDetails", cTable);

                objPropMapData.ConnConfig = connString;
                if (!string.IsNullOrEmpty(Request["sd"]))
                {
                    var startDate = Convert.ToDateTime(HttpUtility.UrlDecode(Request.QueryString["sd"]));
                    report.Dictionary.Variables["StartDate"].Value = startDate.ToLongDateString();
                    objPropMapData.StartDate = startDate;
                }

                if (!string.IsNullOrEmpty(Request["ed"]))
                {
                    var endDate = Convert.ToDateTime(HttpUtility.UrlDecode(Request.QueryString["ed"]));
                    report.Dictionary.Variables["EndDate"].Value = endDate.ToLongDateString();
                    objPropMapData.EndDate = endDate.AddDays(1).AddSeconds(-1);
                }

                // Get Completed Ticket report from Edit Location screen
                if (!string.IsNullOrEmpty(Request["lid"]))
                {
                    objPropMapData.LocID = Convert.ToInt32(HttpUtility.UrlDecode(Request.QueryString["lid"]));
                }

                // Get Completed Ticket report from Edit Customer screen
                if (!string.IsNullOrEmpty(Request["cid"]))
                {
                    objPropMapData.CustID = Convert.ToInt32(HttpUtility.UrlDecode(Request.QueryString["cid"]));
                }

                // Search text
                if (!string.IsNullOrEmpty(Request["stype"]) && !string.IsNullOrEmpty(Request["stext"]))
                {
                    objPropMapData.SearchBy = HttpUtility.UrlDecode(Request.QueryString["stype"]);
                    objPropMapData.SearchValue = HttpUtility.UrlDecode(Request.QueryString["stext"]);
                }

                List<RetainFilter> filters = new List<RetainFilter>();
                if (Session["TicketListRadGVFilters"] != null)
                {
                    ///Get  rad grid view  search filter value from TicketList View
                    filters = (List<RetainFilter>)Session["TicketListRadGVFilters"];
                }

                var levels = "";
                if (!string.IsNullOrEmpty(Request["lev"]))
                {
                    levels = HttpUtility.UrlDecode(Request.QueryString["lev"]);
                }

                var levelNames = "";
                if (!string.IsNullOrEmpty(Request["levNames"]))
                {
                    levelNames = HttpUtility.UrlDecode(Request.QueryString["levNames"]);
                }

                var ds = bL_Report.GetEntrapmentTickets(objPropMapData, filters, levels);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dtDetail = ds.Tables[0];

                    DataSet dsReportData = new DataSet();
                    DataTable dtData = dtDetail.Copy();
                    dtData.TableName = "ReportData";
                    dsReportData.Tables.Add(dtData);
                    dsReportData.DataSetName = "ReportData";

                    report.RegData("ReportData", dtDetail);
                }

                lblLevelNames.Text = levelNames;
                report.Dictionary.Variables["LevelNames"].Value = levelNames;
                report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();

                report.Dictionary.Synchronize();
                report.Render();
                rvTemplate.Report = report;
                byte[] buffer1 = null;
                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(rvTemplate.Report, stream, settings);
                buffer1 = stream.ToArray();
                templateAsBytes.Add(buffer1);

                return templateAsBytes;
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr753", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                return templateAsBytes;
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
    }
}