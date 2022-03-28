using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using BusinessEntity.Payroll;
using BusinessEntity.Utility;
using BusinessLayer;
using Microsoft.Reporting.WebForms;
using Stimulsoft.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace MOMWebApp
{
    public partial class ARAgingReportByTerritoryCollection : System.Web.UI.Page
    {
        #region Variables
        Contracts objContract = new Contracts();
        BL_Contracts objBLContracts = new BL_Contracts();

        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        Customer objPropCustomer = new Customer();
        BL_Customer objBL_Customer = new BL_Customer();

        BL_Report bL_Report = new BL_Report();

        //API Variables 
        string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
        getConnectionConfigParam _getConnectionConfig = new getConnectionConfigParam();
        GetCompanyDetailsParam _GetCompanyDetails = new GetCompanyDetailsParam();
        GetARAgingByTerritoryParam _GetARAgingByTerritory = new GetARAgingByTerritoryParam();
        GetAllTerritoryParam _GetAllTerritory = new GetAllTerritoryParam();
        #endregion

        #region PAGELOAD

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }

            if (!IsPostBack)
            {
                FillTerritory();
                txtEndDate.Text = DateTime.Today.ToShortDateString();

                if (Request.QueryString["EndDate"] != null && !string.IsNullOrEmpty(Request.QueryString["EndDate"]))
                {
                    txtEndDate.Text = DateTime.ParseExact(Request.QueryString["EndDate"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                }

                if (Request.QueryString["Territories"] != null && !string.IsNullOrEmpty(Request.QueryString["Territories"]))
                {
                    var terrs = Request.QueryString["Territories"].Trim();
                    var terrArray = terrs.Split(',');

                    for (int i = 0; i < terrArray.Length; i++)
                    {
                        RadComboBoxItem item = rcTerritory.FindItemByValue(terrArray[i]);
                        if (item != null)
                            item.Checked = true;
                    }
                }
                if (Request.QueryString["type"] != null && !string.IsNullOrEmpty(Request.QueryString["type"]))
                {
                    if (Request.QueryString["type"] == "1")
                    {
                        rdExpandAll.Checked = true;
                        rdCollapseAll.Checked = false;
                    }
                    else
                    {
                        rdExpandAll.Checked = false;
                        rdCollapseAll.Checked = true;
                    }
                }

                if (!string.IsNullOrEmpty(Request["inclNotes"]) && Convert.ToBoolean(Request["inclNotes"]))
                {
                    chkIncludeNotes.Checked = true;
                }
                else
                {
                    chkIncludeNotes.Checked = false;
                }

                if (!string.IsNullOrEmpty(Request["creditFlag"]) && Convert.ToBoolean(Request["creditFlag"]))
                {
                    chkCreditFlag.Checked = true;
                }
                else
                {
                    chkCreditFlag.Checked = false;
                }

                HighlightSideMenu("cstmMgr", "lnkCollections", "cstmMgrSub");
            }

            if (string.IsNullOrEmpty(Request.QueryString["type"]))
            {
                StiWebViewerARReport.Visible = false;
            }
            else
            {
                StiWebViewerARReport.Visible = true;
            }
        }

        #endregion

        protected void StiWebViewerARReport_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
        {
            string reportPathStimul = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["type"]))
            {                
                if (Request.QueryString["type"]=="1")
                {
                    reportPathStimul = Server.MapPath("StimulsoftReports/ARAgingReportByTerritoryReport.mrt");
                }
                else
                {
                    reportPathStimul = Server.MapPath("StimulsoftReports/ARAgingReportByTerritorySummaryReport.mrt");
                }
                StiReport report = new StiReport();
                report.Load(reportPathStimul);
                //report.Compile();

                e.Report = report;
            }              
           
        }

        protected void StiWebViewerARReport_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
        {
            var report = e.Report;
            try
            {
                DataTable cTable = BuildCompanyDetailsTable();
                var cRow = cTable.NewRow();

                DataSet companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());
               
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

                DataSet CompanyDetails = new DataSet();
                cTable.TableName = "CompanyDetails";
                CompanyDetails.Tables.Add(cTable);
                CompanyDetails.DataSetName = "CompanyDetails";

                objContract.ConnConfig = Session["config"].ToString();
                _GetARAgingByTerritory.ConnConfig = Session["config"].ToString();

                objContract.Date = DateTime.Now;
                _GetARAgingByTerritory.Date = DateTime.Now;

                var terrs = string.Empty;
                if (Request.QueryString["Territories"] != null && !string.IsNullOrEmpty(Request.QueryString["Territories"]))
                {
                    terrs = Request.QueryString["Territories"].Trim();
                }

                objContract.Date = DateTime.Now;

                if (Request.QueryString["EndDate"] != null && !string.IsNullOrEmpty(Request.QueryString["EndDate"]))
                {
                    objContract.Date = DateTime.ParseExact(Request.QueryString["EndDate"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                }

                var creditFlag = 0;
                if (!string.IsNullOrEmpty(Request["creditFlag"]) && Convert.ToBoolean(Request.QueryString["creditFlag"]))
                {
                    creditFlag = 1;
                }

                DataSet ds = objBLContracts.GetARAgingByTerritory(objContract, terrs, creditFlag);

                if (ds != null && ds.Tables.Count > 0)
                {
                    report.RegData("ReportData", ds.Tables[0]);

                    if (!string.IsNullOrEmpty(Request["inclNotes"]) && Convert.ToBoolean(Request["inclNotes"]))
                    {
                        objPropCustomer.ConnConfig = Session["config"].ToString(); ;
                        var dsNotes = objBL_Customer.GetRecentCollectionNotes(objPropCustomer);

                        if (dsNotes != null)
                        {
                            report.RegData("LocationNotes", dsNotes.Tables[0]);
                        }

                        report.Dictionary.Variables["IncludeNotes"].Value = Request["inclNotes"];
                    }
                }

                report.RegData("CompanyDetails", CompanyDetails);
                report["Username"] = Session["Username"].ToString();
                report.CacheAllData = true;
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }

        private void FillTerritory()
        {
            try
            {
                DataSet ds = new DataSet();
                objPropUser.ConnConfig = Session["config"].ToString();
                _GetAllTerritory.ConnConfig = Session["config"].ToString();

                if (IsAPIIntegrationEnable == "YES")
                {
                    List<GetTerritoryViewModel> _lstGetTerritory = new List<GetTerritoryViewModel>();
                    string APINAME = "iCollectionsAPI/iCollectionsReport_GetAllTerritory";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetAllTerritory);
                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;
                    _lstGetTerritory = serializer.Deserialize<List<GetTerritoryViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetTerritoryViewModel>(_lstGetTerritory);
                }
                else
                {
                    ds = objBL_User.GetAllTerritory(objPropUser);
                }
                rcTerritory.DataSource = ds.Tables[0];
                rcTerritory.DataTextField = "Name";
                rcTerritory.DataValueField = "ID";
                rcTerritory.DataBind();
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }

        protected void lnkClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("iCollections.aspx");
        }

        protected void lnkSearch_Click(object sender, EventArgs e)
        {         
            GetARAgingReport();
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
                    mail.Title = "AR Aging by Salesperson Report";
                    if (txtBody.Text.Trim() != string.Empty)
                    {
                        mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                    }
                    else
                    {
                        mail.Text = "This is report email sent from Mobile Office Manager. Please find the AR Aging by Salesperson Report attached.";
                    }
                    //mail.AttachmentFiles.Add(ExportReportToPDF("Report_" + objGen.generateRandomString(10) + ".pdf"));
                    mail.attachmentBytes = ExportReportToPDF();
                    mail.FileName = "ARAging.pdf";

                    mail.DeleteFilesAfterSend = true;
                    mail.RequireAutentication = false;
                    // ES-33:Task#2: Added
                    WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                    mail.Send();
                    //this.programmaticModalPopup.Hide();
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

        protected void rdExpCollAll_CheckedChanged(object sender, EventArgs e)
        {
            GetARAgingReport();
        }

        #region Custom function

        private void GetARAgingReport()
        {
            try
            {              
                var endDate = Convert.ToDateTime(txtEndDate.Text);
                string territories = string.Empty;
                List<string> selectedItem = new List<string>();
                foreach (RadComboBoxItem item in rcTerritory.Items)
                {
                    if (item.Checked == true)
                    {
                        selectedItem.Add(item.Value);
                    }
                }

                if (selectedItem.Count > 0)
                {
                    territories = string.Join(",", selectedItem);
                }

                String type = "0";

                if (rdExpandAll.Checked)
                {
                    type = "1";
                }

                String url = "ARAgingReportByTerritoryCollection.aspx?page=iCollections&EndDate=" + endDate.ToString("MM/dd/yyyy") + "&Territories=" + territories + "&type=" + type + "&inclNotes=" + chkIncludeNotes.Checked + "&creditFlag=" + chkCreditFlag.Checked; ;
                this.Response.Redirect(url, true);
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }

        private byte[] ExportReportToPDF()
        {
            byte[] bytes = null;
            var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            var service = new Stimulsoft.Report.Export.StiPdfExportService();
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            service.ExportTo(StiWebViewerARReport.Report, stream, settings);
            bytes = stream.ToArray();

            return bytes;
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

        #endregion
    }
}