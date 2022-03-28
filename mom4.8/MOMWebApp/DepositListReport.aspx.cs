using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using BusinessEntity.Payroll;
using BusinessEntity.Utility;
using BusinessLayer;
using Microsoft.Reporting.WebForms;
using MOMWebApp;
using Stimulsoft.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class DepositListReport : System.Web.UI.Page
{
    #region Variables
    Contracts objContract = new Contracts();
    BL_Contracts objBLContracts = new BL_Contracts();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    Customer objPropCustomer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    BL_Report bL_Report = new BL_Report();
    Dep _objDep = new Dep();
    BL_Deposit _objBL_Deposit = new BL_Deposit();

    //API Variables
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    getConnectionConfigParam _getConnectionConfig = new getConnectionConfigParam();
    GetCompanyDetailsParam _getCompanyDetails = new GetCompanyDetailsParam();
    GetDepositListByDateParam _GetDepositListByDate = new GetDepositListByDateParam();
    GetAllDepositsParam _GetAllDeposits = new GetAllDepositsParam();
    #endregion

    #region events
    #region PAGELOAD
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        
    }
    #endregion


    protected void StiWebViewerARReport_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        string reportPathStimul = string.Empty;
        reportPathStimul = Server.MapPath("StimulsoftReports/DepositListReport.mrt");
        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        //report.Compile();

        e.Report = report;
    }

    protected void StiWebViewerARReport_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        var report = e.Report;
        try
        {
            DataSet dsC = new DataSet();

            objPropUser.ConnConfig = Session["config"].ToString();

            _getConnectionConfig.ConnConfig = Session["config"].ToString();

            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "MakeDepositAPI/DepositReport_GetControl";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig, true);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;
                _GetControlViewModel = serializer.Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                dsC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
            }
            else
            {
                dsC = objBL_User.getControl(objPropUser);
            }

            DataTable cTable = BuildCompanyDetailsTable();
            var cRow = cTable.NewRow();

            DataSet companyInfo = new DataSet();

            List<GetCompanyDetailsViewModel> _GetCompanyDetailsViewModel = new List<GetCompanyDetailsViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "MakeDepositAPI/DepositReport_GetCompanyDetails";

                _getCompanyDetails.ConnConfig = Session["config"].ToString();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCompanyDetails, true);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;
                _GetCompanyDetailsViewModel = serializer.Deserialize<List<GetCompanyDetailsViewModel>>(_APIResponse.ResponseData);
                companyInfo = CommonMethods.ToDataSet<GetCompanyDetailsViewModel>(_GetCompanyDetailsViewModel);
            }
            else
            {
                companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());
            }


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
           

            _objDep.ConnConfig = Session["config"].ToString();
            _objDep.StartDate = DateTime.Now;
            _objDep.EndDate = DateTime.Now;

            _GetDepositListByDate.ConnConfig = Session["config"].ToString();
            _GetDepositListByDate.StartDate = DateTime.Now;
            _GetDepositListByDate.EndDate = DateTime.Now;

            if (Request.QueryString["edate"] != null && !string.IsNullOrEmpty(Request.QueryString["edate"]))
            {
                _objDep.EndDate = DateTime.Parse(Request.QueryString["edate"].ToString());
                _GetDepositListByDate.EndDate = DateTime.Parse(Request.QueryString["edate"].ToString());
            }
            if (Request.QueryString["sdate"] != null && !string.IsNullOrEmpty(Request.QueryString["sdate"]))
            {
                _objDep.StartDate= DateTime.Parse(Request.QueryString["sdate"].ToString());
                _GetDepositListByDate.StartDate = DateTime.Parse(Request.QueryString["sdate"].ToString());
            }

            DataSet ds = new DataSet();
            List<GetDepositListByDateViewModel> _lstGetDepositListByDate = new List<GetDepositListByDateViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "MakeDepositAPI/DepositReport_GetDepositListByDate";

                _GetDepositListByDate.incZeroAmount = false;

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetDepositListByDate, true);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetDepositListByDate = serializer.Deserialize<List<GetDepositListByDateViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetDepositListByDateViewModel>(_lstGetDepositListByDate);
            }
            else
            {
                ds = _objBL_Deposit.GetDepositListByDate(_objDep, false);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dtAR = ds.Tables[0];

                DataSet dsReportData = new DataSet();
                DataTable dtARAging = dtAR.Copy();
                dtARAging.TableName = "ReportData";
                dsReportData.Tables.Add(dtARAging);
                dsReportData.DataSetName = "ReportData";

                report.RegData("ReportData", dsReportData);
            }

            report.RegData("CompanyDetails", CompanyDetails);
            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();
            report.Dictionary.Variables["startDate"].Value = String.Format("{0:M/d/yyyy}", _objDep.StartDate);
            report.Dictionary.Variables["endDate"].Value = String.Format("{0:M/d/yyyy}", _objDep.EndDate);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }


    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("managedeposit.aspx");

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
                mail.Title = "Deposit List Report";
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This report is generated from Mobile Office Manager. Please find attached the deposit list report";
                }
                //mail.AttachmentFiles.Add(ExportReportToPDF("Report_" + objGen.generateRandomString(10) + ".pdf"));
                mail.attachmentBytes = ExportReportToPDF();
                mail.FileName = "DepositListReport.pdf";

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
   
    #endregion

    #region Custom function
   
    private StiReport LoadDepositListReport()
    {
        string reportPathStimul = Server.MapPath("StimulsoftReports/DepositListReport.mrt");
      
        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        //report.Compile();

       
        try
        {
            DataSet dsC = new DataSet();

            objPropUser.ConnConfig = Session["config"].ToString();
            _getConnectionConfig.ConnConfig = Session["config"].ToString();
            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "MakeDepositAPI/DepositReport_GetControl";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig, true);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;
                _GetControlViewModel = serializer.Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                dsC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
            }
            else
            {
                dsC = objBL_User.getControl(objPropUser);
            }

            DataTable cTable = BuildCompanyDetailsTable();
            var cRow = cTable.NewRow();

            DataSet companyInfo = new DataSet();
            List<GetCompanyDetailsViewModel> _GetCompanyDetailsViewModel = new List<GetCompanyDetailsViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "MakeDepositAPI/DepositReport_GetCompanyDetails";

                _getCompanyDetails.ConnConfig = Session["config"].ToString();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCompanyDetails, true);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;
                _GetCompanyDetailsViewModel = serializer.Deserialize<List<GetCompanyDetailsViewModel>>(_APIResponse.ResponseData);
                companyInfo = CommonMethods.ToDataSet<GetCompanyDetailsViewModel>(_GetCompanyDetailsViewModel);
            }
            else
            {
                companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());
            }


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

            _objDep.ConnConfig = Session["config"].ToString();
            _objDep.StartDate = DateTime.Now;
            _objDep.EndDate = DateTime.Now;

            _GetAllDeposits.ConnConfig = Session["config"].ToString();
            _GetAllDeposits.StartDate = DateTime.Now;
            _GetAllDeposits.EndDate = DateTime.Now;

            if (Request.QueryString["edate"] != null && !string.IsNullOrEmpty(Request.QueryString["edate"]))
            {
                _objDep.EndDate = DateTime.Parse(Request.QueryString["edate"].ToString());
                _GetAllDeposits.EndDate = DateTime.Parse(Request.QueryString["edate"].ToString());
            }
            if (Request.QueryString["sdate"] != null && !string.IsNullOrEmpty(Request.QueryString["sdate"]))
            {
                _objDep.StartDate = DateTime.Parse(Request.QueryString["sdate"].ToString());
                _GetAllDeposits.StartDate = DateTime.Parse(Request.QueryString["sdate"].ToString());
            }

            DataSet ds = new DataSet();
            List<GetAllDepositsViewModel> _lstGetAllDeposits = new List<GetAllDepositsViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "MakeDepositAPI/ManageDepositList_GetAllDeposits";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetAllDeposits,true);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetAllDeposits = serializer.Deserialize<List<GetAllDepositsViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetAllDepositsViewModel>(_lstGetAllDeposits);
            }
            else
            {
                ds = _objBL_Deposit.GetAllDeposits(_objDep);
            }


            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dtAR = ds.Tables[0];

                DataSet dsReportData = new DataSet();
                DataTable dtARAging = dtAR.Copy();
                dtARAging.TableName = "ReportData";
                dsReportData.Tables.Add(dtARAging);
                dsReportData.DataSetName = "ReportData";

                report.RegData("ReportData", dsReportData);
            }

            report.RegData("CompanyDetails", CompanyDetails);
            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();
            report.Dictionary.Variables["startDate"].Value = String.Format("{0:M/d/yyyy}", _objDep.StartDate);
            report.Dictionary.Variables["endDate"].Value = String.Format("{0:M/d/yyyy}", _objDep.EndDate);
            report.Render();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
      
        return report;
    }

    private byte[] ExportReportToPDF()
    {
        byte[] bytes = null;
        var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
        var service = new Stimulsoft.Report.Export.StiPdfExportService();
        System.IO.MemoryStream stream = new System.IO.MemoryStream();
        service.ExportTo(LoadDepositListReport(), stream, settings);
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

    #endregion

   
}