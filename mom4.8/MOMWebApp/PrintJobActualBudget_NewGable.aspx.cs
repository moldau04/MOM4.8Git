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
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using BusinessEntity.Payroll;
using BusinessEntity.Utility;
using BusinessLayer;
using MOMWebApp;
using Stimulsoft.Report;
using Stimulsoft.Report.Export;
using Telerik.Web.UI;

public partial class PrintJobActualBudget_NewGable : System.Web.UI.Page
{
    GeneralFunctions objgn = new GeneralFunctions();

    Customer objCust = new Customer();
    BL_Report objBL_Report = new BL_Report();
    BL_Customer objBL_Customer = new BL_Customer();
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    GetSMTPByUserIDParam _getSMTPByUserID = new GetSMTPByUserIDParam();

    //API Variables
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    getConnectionConfigParam _getConnectionConfig = new getConnectionConfigParam();
    GetCompanyDetailsParam _GetCompanyDetails = new GetCompanyDetailsParam();
    GetSMTPByUserIDParam _GetSMTPByUserID = new GetSMTPByUserIDParam();
    GetCustomerLabelParam _GetCustomerLabel = new GetCustomerLabelParam();
    General objGeneral = new General();
    BL_General objBL_General = new BL_General();
    JobT objJob = new JobT();
    BL_Job objBL_Job = new BL_Job();
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
                GetSMTPUser();
                SetAddress();
                string FileName = "ProjectActualVsBudgetReport_Gable.pdf";
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

        }
    }

    //Serach Button Click Event
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Response.Redirect("PrintJobActualBudget_New.aspx");
    }

    //Get Report With Data Event
    protected void StiWebViewerCustomerCategoryDetailReport_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        e.Report = LoadCustomerTicketCategorybyLastServiceReport();
    }

    //Report Load Function
    //Get Data For Report
    private StiReport LoadCustomerTicketCategorybyLastServiceReport()
    {
        try
        {
            string reportPathStimul = Server.MapPath("StimulsoftReports/Project/ProjectActualVsBudgetReport-Gable.mrt");

            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            //Get data
            DataSet dsC = new DataSet();

            var connString = Session["config"].ToString();
            objPropUser.ConnConfig = connString;

            DataSet companyInfo = new DataSet();

            List<GetCompanyDetailsViewModel> _GetCompanyDetailsViewModel = new List<GetCompanyDetailsViewModel>();
            //API
            _GetCompanyDetails.ConnConfig = Session["config"].ToString();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "CustomersAPI/AddCustomer_ServiceHistoryReport_GetCompanyDetails";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCompanyDetails, true);

                _GetCompanyDetailsViewModel = (new JavaScriptSerializer()).Deserialize<List<GetCompanyDetailsViewModel>>(_APIResponse.ResponseData);
                companyInfo = CommonMethods.ToDataSet<GetCompanyDetailsViewModel>(_GetCompanyDetailsViewModel);
            }
            else
            {
                companyInfo = objBL_Report.GetCompanyDetails(Session["config"].ToString());
            }
            DataSet dsJob = new DataSet();

            objCust.ConnConfig = Session["config"].ToString();

            if (Session["PrintJobActualBudgetserachBy"] != null)
            {
                if (Session["PrintJobActualBudgetserachBy"].ToString() != "")
                {
                    objCust.SearchBy = Session["serachBy"].ToString();
                }
                else
                {
                    objCust.SearchBy = null;
                }
            }
            else
            {
                objCust.SearchBy = null;
            }
            if (Session["PrintJobActualBudgetsearchValue"] != null)
            {
                if (Session["PrintJobActualBudgetsearchValue"].ToString() != "")
                {
                    objCust.SearchValue = Session["searchValue"].ToString();
                }
                else
                {
                    objCust.SearchValue = null;
                }
            }
            else
            {
                objCust.SearchValue = null;
            }

            objCust.Range = Convert.ToInt16(Session["ddlDateRangeFieldforEditJob"].ToString());

            if (Session["txtfrmDtValforEditjob"].ToString() != "")
            {
                objCust.StartDate = Session["txtfrmDtValforEditjob"].ToString();
            }
            else
            {
                objCust.StartDate = null;

            }

            if (Session["txttoDtValforEditJob"].ToString() != "")
            {
                objCust.EndDate = Session["txttoDtValforEditJob"].ToString();
            }
            else
            {
                objCust.EndDate = null;

                objCust.JobType = null;
            }

            try
            {
                if (Session["JobListobjBL_Customer"] != null)
                {
                    objCust = (Customer)Session["JobListobjBL_Customer"];
                }

            }
            catch { }
            DataTable dtItem = CreateJobItemDataTable();
            DataTable dtFilters = CreateFiltersToDataTable();
            dtFilters = (DataTable)Session["New_Report_GridFilters"];
            dsJob = objBL_Customer.getJobProjectReport(objCust, dtFilters, new GeneralFunctions().GetSalesAsigned(), objCust.Status);

            try
            {
                for (int i = 0; i <= dsJob.Tables[0].Rows.Count - 1; i++)
                {
                    objJob.ConnConfig = Session["config"].ToString();
                    objJob.Job = Convert.ToInt32(dsJob.Tables[0].Rows[i][2]);
                    objJob.PageIndex = 1;
                    objJob.PageSize = 1000;

                    if (!string.IsNullOrEmpty(Session["txtfrmDtValforEditjob"].ToString()) && !string.IsNullOrEmpty(Session["txttoDtValforEditJob"].ToString()))
                    {
                        objJob.StartDate = Convert.ToDateTime(Session["txtfrmDtValforEditjob"].ToString());
                        objJob.EndDate = Convert.ToDateTime(Session["txttoDtValforEditJob"].ToString());
                    }

                    DataSet dsItems = objBL_Job.GetJobCostByJobReport(objJob);

                    //dtItem = dsItems.Tables[0]
                    for (int j = 0; j <= dsItems.Tables[0].Rows.Count - 1; j++)
                    {
                        DataRow dr = dtItem.NewRow();
                        dr["Job"] = dsItems.Tables[0].Rows[j]["Job"];
                        dr["Type"] = dsItems.Tables[0].Rows[j]["Type"];
                        dr["fDesc"] = dsItems.Tables[0].Rows[j]["fDesc"];
                        dr["Code"] = dsItems.Tables[0].Rows[j]["Code"];
                        dr["Actual"] = dsItems.Tables[0].Rows[j]["Actual"];
                        dr["Budget"] = dsItems.Tables[0].Rows[j]["Budget"];
                        dr["JobType"] = dsItems.Tables[0].Rows[j]["JobType"];
                        dr["BudgetRatio"] = dsItems.Tables[0].Rows[j]["BudgetRatio"];

                        dtItem.Rows.Add(dr);
                        dtItem.AcceptChanges();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            //report.CacheAllData = true;
            report.RegData("CompanyDetails", companyInfo.Tables[0]);
            report.RegData("JobDt", dsJob.Tables[0]);
            report.RegData("JobItem", dtItem);
            report.CacheAllData = true;
            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();

            report.Render();

            return report;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return null;
        }
    }

    private DataTable CreateJobItemDataTable()
    {
        DataTable dtJobItem = new DataTable();
        dtJobItem.Clear();
        dtJobItem.Columns.Add("Job", typeof(int));
        dtJobItem.Columns.Add("Type");
        dtJobItem.Columns.Add("fDesc");
        dtJobItem.Columns.Add("Code");
        dtJobItem.Columns.Add("Actual");
        dtJobItem.Columns.Add("Budget");
        dtJobItem.Columns.Add("JobType");
        dtJobItem.Columns.Add("BudgetRatio");
        DataRow dtJobitemRow = dtJobItem.NewRow();
        dtJobItem.Rows.Add(dtJobitemRow);

        return dtJobItem;

    }
    private DataTable CreateFiltersToDataTable()
    {
        //create new table to add filter values.
        DataTable dtFilters = new DataTable();
        dtFilters.Clear();
        dtFilters.Columns.Add("Customer");
        dtFilters.Columns.Add("Tag");
        dtFilters.Columns.Add("ID");
        dtFilters.Columns.Add("fdesc");
        dtFilters.Columns.Add("Status");
        dtFilters.Columns.Add("Stage");
        dtFilters.Columns.Add("Company");
        dtFilters.Columns.Add("CType");
        dtFilters.Columns.Add("TemplateDesc");
        dtFilters.Columns.Add("Type");
        dtFilters.Columns.Add("SalesPerson");
        dtFilters.Columns.Add("Route");
        dtFilters.Columns.Add("NHour");
        dtFilters.Columns.Add("ContractPrice");
        dtFilters.Columns.Add("NotBilledYet");
        dtFilters.Columns.Add("NComm");
        dtFilters.Columns.Add("NRev");
        dtFilters.Columns.Add("NLabor");
        dtFilters.Columns.Add("NMat");
        dtFilters.Columns.Add("NOMat");
        dtFilters.Columns.Add("NCost");
        dtFilters.Columns.Add("NProfit");
        dtFilters.Columns.Add("NRatio");
        dtFilters.Columns.Add("RouteFilters");
        dtFilters.Columns.Add("StageFilters");
        dtFilters.Columns.Add("DepartmentFilters");
        dtFilters.Columns.Add("ProjectManagerUserName");
        dtFilters.Columns.Add("LocationType");
        dtFilters.Columns.Add("BuildingType");
        dtFilters.Columns.Add("TotalBudgetedExpense");
        dtFilters.Columns.Add("SupervisorUserName");
        dtFilters.Columns.Add("OpenARBalance");
        dtFilters.Columns.Add("OpenAPBalance");
        dtFilters.Columns.Add("ExpectedClosingDate");
        DataRow dtFiltersRow = dtFilters.NewRow();
        dtFilters.Rows.Add(dtFiltersRow);

        return dtFilters;
    }

    //Report Event
    protected void StiWebViewerCustomerCategoryDetailReport_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {

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

                if (txtEmailBCC.Text.Trim() != string.Empty)
                {
                    mail.Bcc = txtEmailBCC.Text.Split(';', ',').OfType<string>().ToList();
                }

                mail.Title = "Project Actual Vs Budget Report-Gable";
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This is report email sent from Mobile Office Manager. Please find the Project Actual Vs Budget Report-Gable attached.";
                }

                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(LoadCustomerTicketCategorybyLastServiceReport(), stream, settings);
                buffer1 = stream.ToArray();

                if (hdnFirstAttachement.Value != "-1")
                {
                    mail.attachmentBytes = buffer1;
                }

                ArrayList lst = new ArrayList();
                if (ViewState["pathmailatt"] != null)
                {
                    lst = (ArrayList)ViewState["pathmailatt"];
                    foreach (string strpath in lst)
                    {
                        if (strpath != "ProjectActualVsBudgetReport_Gable.pdf")
                        {
                            mail.AttachmentFiles.Add(strpath);
                        }
                    }
                }

                mail.FileName = "ProjectActualVsBudgetReport_Gable.pdf";

                mail.DeleteFilesAfterSend = true;
                mail.RequireAutentication = false;
                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                mail.Send();

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            catch (Exception ex)
            {
                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }

    protected void btnAttachmentDel_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string path = btn.CommandArgument;
        DownloadDocument(path, Path.GetFileName(path));
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
            if (DownloadFileName == "ProjectActualVsBudgetReport_Gable.pdf")
            {
                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(LoadCustomerTicketCategorybyLastServiceReport(), stream, settings);
                buffer1 = stream.ToArray();

                Response.Clear();
                MemoryStream ms = new MemoryStream(buffer1);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=ProjectActualVsBudgetReport_Gable.pdf");
                Response.Buffer = true;
                ms.WriteTo(Response.OutputStream);
                Response.End();
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "FileaccessWarning", "alert('File not found.');", true);
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "FileaccessWarning", "alert('Please provide access permissions to the file path.');", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "FileerrorWarning", "alert('" + str + "');", true);
        }
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

    private void DeleteFile(string filepath)
    {
        ////this should delete the file in the next reboot, not now.
        if (System.IO.File.Exists(filepath))
        {
            // Use a try block to catch IOExceptions, to 
            // handle the case of the file already being 
            // opened by another process. 
            try
            {
                System.IO.File.Delete(filepath);
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e.Message);
                //return;
            }
        }
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
        RadGrid_Emails.Rebind();
    }

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
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("project.aspx?sb=" + HttpContext.Current.Request.QueryString["sb"].ToString() + "&sv=" + HttpContext.Current.Request.QueryString["sv"].ToString() + "&rng=" + HttpContext.Current.Request.QueryString["rng"].ToString() + "&df=" + HttpContext.Current.Request.QueryString["df"].ToString() + "&dt=" + HttpContext.Current.Request.QueryString["dt"].ToString());
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        ddlSearch.SelectedIndex = 0;
        txtSearch.Text = string.Empty;
        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
        RadGrid_Emails.Rebind();
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
    }

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
        }
        else
        {
            Session["Emails_FilterExpression"] = null;
            Session["Emails_Filters"] = null;
        }

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
    }

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

    protected void RadComboBox1_DataBinding(object sender, EventArgs e)
    {
        //ddlcategory.DataValueField = "Type";
        //ddlcategory.DataTextField = "Type";
        //ddlcategory.DataSource = GetSampleSource();
    }

    private DataSet GetSampleSource()
    {
        DataSet ds = new DataSet();
        objGeneral.ConnConfig = Session["config"].ToString();
        ds = objBL_General.getAllCategoryList(objGeneral);
        return ds;
    }

    private string GetRadComboBoxSelectedItems(RadComboBox radComboBox)
    {
        int itemschecked = radComboBox.CheckedItems.Count;
        String[] ServiceTypesArray = new String[itemschecked];

        var collection = radComboBox.CheckedItems;
        int i = 0;
        foreach (var item in collection)
        {
            String value = item.Value;
            ServiceTypesArray[i] = $"{value}";
            i++;
        }
        var ServiceTypes = String.Join(",", ServiceTypesArray);

        return ServiceTypes;
    }

    private void GetSMTPUser()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.UserID = Convert.ToInt32(Session["UserID"]);

        _getSMTPByUserID.ConnConfig = Session["config"].ToString();
        _getSMTPByUserID.UserID = Convert.ToInt32(Session["UserID"]);

        DataSet ds = new DataSet();

        List<SMTPEmailViewModel> _lstSMTPEmailViewModel = new List<SMTPEmailViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/EquipmentReport_GetSMTPByUserID";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getSMTPByUserID, true);
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;
            _lstSMTPEmailViewModel = serializer.Deserialize<List<SMTPEmailViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<SMTPEmailViewModel>(_lstSMTPEmailViewModel);
        }
        else
        {
            ds = objBL_User.getSMTPByUserID(objPropUser);
        }

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

    private void SetAddress()
    {
        DataSet dsC = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        _getConnectionConfig.ConnConfig = Session["config"].ToString();

        List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/EquipmentReport_GetControl";

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

        if (dsC.Tables[0].Rows.Count > 0)
        {
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
            string mailBody = "Please review the attached Project Actual Vs Budget Report-Gable.";
            address = mailBody + Environment.NewLine + "<br />" + Environment.NewLine + "<br />" + address;

            txtBody.Text = address;

        }
    }
}
