using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Stimulsoft.Report;
using Stimulsoft.Report.Web;
using System.Configuration;
using Microsoft.ApplicationBlocks.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Collections;
using System.Net.Configuration;
using System.Collections.Generic;
using AjaxControlToolkit;
using BusinessEntity;
using BusinessLayer;
using Microsoft.Reporting.WebForms;
using Telerik.Web.UI;

public partial class ProjectGLCrossReferenceReport : System.Web.UI.Page
{
    GeneralFunctions objgn = new GeneralFunctions();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    #region PAGELOAD
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {
            HighlightSideMenu("ProjectMgr", "lnkProject", "ProjectMgrSub");

            GetSMTPUser();
            SetAddress();
            string FileName = "ProjectGLCrossReferenceReport.pdf";
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
    #endregion

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("addProject.aspx?uid=" + Request.QueryString["id"]);
    }

    protected void StiWebViewerProjectGLCross_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        e.Report = LoadProjectGLCrossReport();
    }

    protected void StiWebViewerProjectGLCross_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {

    }

    private StiReport LoadProjectGLCrossReport()
    {
        string reportPathStimul = Server.MapPath("StimulsoftReports/Project/ProjectGLCrossReferenceReport.mrt");

        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        //report.Compile();

        // Company details
        DataSet dsCompany = new DataSet();
        BL_Report bL_Report = new BL_Report();
        dsCompany = bL_Report.GetCompanyDetails(Session["config"].ToString());

        report.RegData("dsCompany", dsCompany.Tables[0]);

        // Get parameters
        DateTime startDate = new DateTime();
        DateTime endDate = new DateTime();

        objProp_Contracts.ConnConfig = Session["config"].ToString();
 
        if (!string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            objProp_Contracts.JobId = Convert.ToInt32(Request.QueryString["id"].ToString());
        }

        var dateRangeType = 1;
        if (!string.IsNullOrEmpty(Request.QueryString["type"]))
        {
            dateRangeType = Convert.ToInt32(Request.QueryString["type"].ToString());
        }

        if (!string.IsNullOrEmpty(Request.QueryString["df"]) && !string.IsNullOrEmpty(Request.QueryString["dt"]))
        {
            startDate = Convert.ToDateTime(Request.QueryString["df"].ToString());
            endDate = Convert.ToDateTime(Request.QueryString["dt"].ToString());

            objProp_Contracts.StartDate = startDate;
            objProp_Contracts.EndDate = endDate;
        }
        else
        {
            dateRangeType = 1;
        }

        DataSet ds = objBL_Contracts.GetProjectGLCrossReference(objProp_Contracts, dateRangeType);

        if (ds != null && ds.Tables.Count > 3)
        {
            var dtJob = ds.Tables[0];
            var dtIncome = ds.Tables[1];
            var dtExpenses = ds.Tables[2];

            if (ds.Tables[1].Rows.Count > 0 && ds.Tables[3].Rows.Count > 0)
            {
                var drAcct = ds.Tables[3].Rows[0];

                dtIncome = ds.Tables[1].AsEnumerable()
                    .GroupBy(r => r.Field<string>("Acct"))
                    .Select(g =>
                    {
                        var row = ds.Tables[1].NewRow();

                        row["Acct"] = drAcct["Acct"];
                        row["fDesc"] = drAcct["fDesc"];
                        row["Amount"] = g.Sum(r => r.Field<decimal>("Amount"));
                        row["Total"] = g.Sum(r => r.Field<decimal>("Total"));

                        return row;
                    }).CopyToDataTable();
            }

            report.RegData("dsJob", dtJob);
            report.RegData("dsIncome", dtIncome);
            report.RegData("dsExpenses", dtExpenses);
        }

        report.Dictionary.Variables["StartDate"].Value = Request.QueryString["df"];
        report.Dictionary.Variables["EndDate"].Value = Request.QueryString["dt"];
        report.Dictionary.Variables["DateRangeType"].Value = dateRangeType.ToString();
        report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();
        report.Render();

        return report;
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
        RadGrid_Emails.Rebind();
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        ddlSearch.SelectedIndex = 0;
        txtSearch.Text = string.Empty;
        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
        RadGrid_Emails.Rebind();
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
    }

    //public static string[] GetContactEmails(string prefixText, int count, string contextKey)
    //{
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

                mail.Title = "Project GL Cross Reference Report";
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This is report email sent from Mobile Office Manager. Please find the Project GL Cross Reference Report attached.";
                }

                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(LoadProjectGLCrossReport(), stream, settings);
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
                        if (strpath != "ProjectGLCrossReferenceReport.pdf")
                        {
                            mail.AttachmentFiles.Add(strpath);
                        }
                    }
                }

                mail.FileName = "ProjectGLCrossReferenceReport.pdf";

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
            if (DownloadFileName == "ProjectGLCrossReferenceReport.pdf")
            {
                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(LoadProjectGLCrossReport(), stream, settings);
                buffer1 = stream.ToArray();

                Response.Clear();
                MemoryStream ms = new MemoryStream(buffer1);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=ProjectGLCrossReferenceReport.pdf");
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

    private void SetAddress()
    {
        var address = WebBaseUtility.GetSignature();

        string mailBody = "Please review the attached Project GL Cross Reference Report.";
        address = mailBody + Environment.NewLine + "<br />" + Environment.NewLine + "<br />" + address;

        txtBody.Text = address;

        
    }

    private DataTable CreateFiltersToDataTable(List<RetainFilter> filters)
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
        DataRow dtFiltersRow = dtFilters.NewRow();

        if (filters != null && filters.Count > 0)
        {
            decimal filterValue = -1900;

            dtFiltersRow["NHour"] = filterValue;
            dtFiltersRow["TotalBudgetedExpense"] = filterValue;
            dtFiltersRow["NRatio"] = filterValue;
            dtFiltersRow["ContractPrice"] = filterValue;
            dtFiltersRow["NComm"] = filterValue;
            dtFiltersRow["NotBilledYet"] = filterValue;
            dtFiltersRow["NRev"] = filterValue;
            dtFiltersRow["NLabor"] = filterValue;
            dtFiltersRow["NMat"] = filterValue;
            dtFiltersRow["NOMat"] = filterValue;
            dtFiltersRow["NCost"] = filterValue;
            dtFiltersRow["NProfit"] = filterValue;
            foreach (var items in filters)
            {
                if (!string.IsNullOrEmpty(items.FilterValue) && !string.IsNullOrWhiteSpace(items.FilterValue))
                {

                    if (items.FilterColumn == "LocationType")
                    {
                        dtFiltersRow["LocationType"] = items.FilterValue;
                    }

                    if (items.FilterColumn == "BuildingType")
                    {
                        dtFiltersRow["BuildingType"] = items.FilterValue;
                    }

                    if (items.FilterColumn == "Customer")
                    {
                        dtFiltersRow["Customer"] = items.FilterValue;
                    }

                    if (items.FilterColumn == "Tag")
                    {
                        dtFiltersRow["Tag"] = items.FilterValue;
                    }

                    if (items.FilterColumn == "ProjectManagerUserName")
                    {
                        dtFiltersRow["ProjectManagerUserName"] = items.FilterValue;
                    }

                    if (items.FilterColumn == "SupervisorUserName")
                    {
                        dtFiltersRow["SupervisorUserName"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "SupervisorUserName")
                    {
                        dtFiltersRow["SupervisorUserName"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "OpenARBalance")
                    {
                        dtFiltersRow["OpenARBalance"] = items.FilterValue;
                    }
                    /// Int Filter
                    int FilterValue = 0; string[] filterArrayValue;

                    StringBuilder filteredQuery = new StringBuilder();

                    if (items.FilterColumn == "ID")
                    {
                        filterArrayValue = items.FilterValue.ToString().Split(',');
                        foreach (var filtered in filterArrayValue)
                        {
                            if (int.TryParse(filtered, out FilterValue))
                            {
                                if (filteredQuery.Length == 0)
                                {
                                    filteredQuery.Append(filtered);
                                }
                                else
                                {
                                    filteredQuery.Append("," + filtered);
                                }
                            }


                        }
                        dtFiltersRow["ID"] = filteredQuery.ToString();
                    }

                    if (items.FilterColumn == "fdesc")
                    {
                        dtFiltersRow["fdesc"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "Status")
                    {
                        dtFiltersRow["Status"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "Stage")
                    {
                        dtFiltersRow["Stage"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "Company")
                    {
                        dtFiltersRow["Company"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "CType")
                    {
                        dtFiltersRow["CType"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "TemplateDesc")
                    {
                        dtFiltersRow["TemplateDesc"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "Department")
                    {
                        dtFiltersRow["Type"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "Salesperson")
                    {
                        dtFiltersRow["SalesPerson"] = items.FilterValue;
                    }
                    if (items.FilterColumn == "DRoute")
                    {
                        dtFiltersRow["Route"] = items.FilterValue;
                    }

                    if (items.FilterColumn == "LocationType")
                    {
                        dtFiltersRow["LocationType"] = items.FilterValue;
                    }

                    if (items.FilterColumn == "BuildingType")
                    {
                        dtFiltersRow["BuildingType"] = items.FilterValue;
                    }
                    filterValue = -1900;


                    if (items.FilterColumn == "ContractPrice" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["ContractPrice"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NotBilledYet" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NotBilledYet"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NComm" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NComm"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NRev" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NRev"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NLabor" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NLabor"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NMat" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NMat"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NOMat" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NOMat"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NCost" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NCost"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NProfit" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NProfit"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NRatio" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NRatio"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "TotalBudgetedExpense" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["TotalBudgetedExpense"] = filterValue;
                    }

                    filterValue = -1900;

                    if (items.FilterColumn == "NHour" && decimal.TryParse(items.FilterValue, out filterValue))
                    {
                        dtFiltersRow["NHour"] = filterValue;
                    }

                }
            }
        }

        if (Session["PROJ_RouteFilters"] != null && Session["PROJ_RouteFilters"].ToString() != string.Empty)
        {
            dtFiltersRow["RouteFilters"] = Session["PROJ_RouteFilters"].ToString();
        }

        if (Session["PROJ_StageFilters"] != null && Session["PROJ_StageFilters"].ToString() != string.Empty)
        {
            dtFiltersRow["StageFilters"] = Session["PROJ_StageFilters"].ToString();
        }

        if (Session["PROJ_DepartmentFilters"] != null && Session["PROJ_DepartmentFilters"].ToString() != string.Empty)
        {
            dtFiltersRow["DepartmentFilters"] = Session["PROJ_DepartmentFilters"].ToString();
        }

        dtFilters.Rows.Add(dtFiltersRow);

        return dtFilters;
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