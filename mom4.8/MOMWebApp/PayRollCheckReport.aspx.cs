using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Web;
using Telerik.Web.UI;


public partial class PayRollCheckReport : System.Web.UI.Page
{
    GeneralFunctions objgn = new GeneralFunctions();
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    Contracts objPropContracts = new Contracts();
    BL_Contracts objBL_Contracts = new BL_Contracts();
    BL_Report objBL_Report = new BL_Report();
    Emp objProp_Emp = new Emp();

    //Page Load Event
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
                if (!string.IsNullOrEmpty(Request["ed"]) && !string.IsNullOrEmpty(Request["sd"]))
                {
                    StiWebViewerPayrollCheck.Visible = true;
                }
                if (!string.IsNullOrEmpty(Request["sd"]))
                {
                    txtfromDate.Text = HttpUtility.UrlDecode(Request.QueryString["sd"]);
                }
                else
                {
                    txtfromDate.Text = DateTime.Now.ToString();
                }

                if (!string.IsNullOrEmpty(Request["ed"]))
                {
                    txtToDate.Text = HttpUtility.UrlDecode(Request.QueryString["ed"]);
                }
                else
                {
                    txtToDate.Text = DateTime.Now.ToString();
                }
                if (!string.IsNullOrEmpty(Request["e.ID"]))
                {
                    rbEmployee.SelectedValue = Request["e.ID"].ToString();
                }
                if (!string.IsNullOrEmpty(Request["e.fWork"]))
                {
                    rbSupervisor.SelectedValue = Request["e.fWork"].ToString();
                }

                HighlightSideMenu("prID", "payrollregisterlink", "payrollmenutab");

                GetSMTPUser();
                SetAddress();
                string FileName = "CheckReport.pdf";
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
                BindEmployee();
                FillDepartment();
                FillSupervisor();
                if (!string.IsNullOrEmpty(Request["d.ID"]))
                {
                    var routes = HttpUtility.UrlDecode(Request["d.ID"]).Split(',');
                    foreach (var route in routes)
                    {
                        var item = rbDepartment.Items.FindItem(x => x.Value == route);
                        if (item != null)
                        {
                            item.Checked = true;
                        }
                    }
                }
                ViewState["pathmailatt"] = lstPath;
                dlAttachmentsDelete.DataSource = lstPath;
                dlAttachmentsDelete.DataBind();
                hdnFirstAttachement.Value = FileName;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    //Button Attachment Download Event
    protected void btnAttachmentDel_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string path = btn.CommandArgument;
        DownloadDocument(path, Path.GetFileName(path));
    }

    //Image Delete Attach Event
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

    //Upload Document Link Event 
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

    //Hide Modal Popup Event
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

                mail.Title = "Payroll Check Report";
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This is report email sent from Mobile Office Manager. Please find the Payroll Check Report attached.";
                }

                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(LoadReport(), stream, settings);
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
                        if (strpath != "PayrollCheck.pdf")
                        {
                            mail.AttachmentFiles.Add(strpath);
                        }
                    }
                }

                mail.FileName = "PayrollCheck.pdf";

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

    //RAD Grid Email Event
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

    //RAD Grid Email Event
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

    //Button Clear Click Event
    protected void lnkClear_Click(object sender, EventArgs e)
    {
        ddlSearch.SelectedIndex = 0;
        txtSearch.Text = string.Empty;
        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
        RadGrid_Emails.Rebind();
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
    }

    //Button Search Click Event
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
        RadGrid_Emails.Rebind();
    }

    //Button Search - Report Filter Event
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        var url = "PayRollCheckReport.aspx?sd=" + txtfromDate.Text + "&ed=" + txtToDate.Text;
        if (rbEmployee.SelectedValue != "0")
        {
            url = url + "&e.ID=" + rbEmployee.SelectedValue;
        }
        if (rbSupervisor.SelectedValue != "" && rbSupervisor.SelectedValue != null && rbSupervisor.SelectedValue != "All")
        {
            url = url + "&e.fWork=" + rbSupervisor.SelectedValue;
        }
        if (rbDepartment.SelectedValue != "" && rbDepartment.SelectedValue != null)
        {
            var routes = GetRadComboBoxSelectedItems(rbDepartment);
            url = url + "&d.ID=" + routes;
        }
        Response.Redirect(url);

    }

    //button Close Event
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("PayrollRegister");
    }

    //Report Get Data Event
    protected void StiWebViewerPayrollCheck_GetReport(object sender, StiReportDataEventArgs e)
    {
        e.Report = LoadReport();
    }

    //Report Data Set Event
    protected void StiWebViewerPayrollCheck_GetReportData(object sender, StiReportDataEventArgs e)
    {

    }

    //Funtion For HighlightSideMenu
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

    //Get SMPT USER For Email
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

    //Set Address For Email
    private void SetAddress()
    {
        var address = WebBaseUtility.GetSignature();

        string mailBody = "Please review the Payroll Check Report.";
        address = mailBody + Environment.NewLine + "<br />" + Environment.NewLine + "<br />" + address;

        txtBody.Text = address;

        
    }

    //Dowload Report 
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
            if (DownloadFileName == "CheckReport.pdf")
            {
                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(LoadReport(), stream, settings);
                buffer1 = stream.ToArray();

                Response.Clear();
                MemoryStream ms = new MemoryStream(buffer1);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=CheckReport.pdf");
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

    //Report Load Function
    private StiReport LoadReport()
    {
        try
        {
            string reportPathStimul = Server.MapPath("StimulsoftReports/PayrollCheckReport.mrt");
            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //Get data
            DataSet dsC = new DataSet();
            var connString = Session["config"].ToString();
            objPropUser.ConnConfig = connString;
            DataSet companyInfo = new DataSet();
            companyInfo = objBL_Report.GetCompanyDetails(Session["config"].ToString());
            report.RegData("CompanyDetails", companyInfo.Tables[0]);
            objPropContracts.ConnConfig = connString;

            if (!string.IsNullOrEmpty(Request["sd"]))
            {
                objPropContracts.StartDate = Convert.ToDateTime(Request["sd"]);
            }

            if (!string.IsNullOrEmpty(Request["ed"]))
            {
                objPropContracts.EndDate = Convert.ToDateTime(Request["ed"]);
            }
            if (!string.IsNullOrEmpty(Request["e.ID"]))
            {
                objPropContracts.filterValue = Request["e.ID"].ToString();
            }
            if (!string.IsNullOrEmpty(Request["e.fWork"]))
            {
                objPropContracts.SearchValue = Request["e.fWork"].ToString();
            }
            string[] Category = new string[] { };
            if (!string.IsNullOrEmpty(Request.QueryString["d.ID"]))
            {
                //if (Request.QueryString["ctype"] == "Name")
                //{
                Category = HttpUtility.UrlDecode(Request["d.ID"]).Split(',');
                objPropContracts.Ctype = HttpUtility.UrlDecode(Request.QueryString["d.ID"]);
                //}
            }
            DataSet ds = objBL_Contracts.GetPayrollCheck(objPropContracts, null, null, null);
            if (ds != null && ds.Tables.Count > 0)
            {
                var dl = ds.Tables[0].Copy().DefaultView;
                report.RegData("ReportData", dl.ToTable());
            }
            //if (!string.IsNullOrEmpty(Request.QueryString["e.ID"]))
            //{
            //    var group = report.GetComponentByName("GroupHeaderBand2");
            //    (group as StiGroupHeaderBand).Condition.Value = "{ReportData.Name}";
            //}
            //if (!string.IsNullOrEmpty(Request.QueryString["e.fWork"]))
            //{
            //    var group = report.GetComponentByName("GroupHeaderBand3");
            //    (group as StiGroupHeaderBand).Condition.Value = "{ReportData.Supervisor}";
            //}
            //if (!string.IsNullOrEmpty(Request.QueryString["d.Id"]))
            //{
            //    var group = report.GetComponentByName("GroupHeaderBand1");
            //    (group as StiGroupHeaderBand).Condition.Value = "{ReportData.category}";
            //}

            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();
            report.Dictionary.Variables["StartDate"].Value = objPropContracts.StartDate.ToLongDateString();
            report.Dictionary.Variables["EndDate"].Value = objPropContracts.EndDate.ToLongDateString();
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
    //Delete File
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

    //Distribution Function For rad grid 
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
    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //SelectSearch();
    }
    private void FillDepartment()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getDepartment(objPropUser);

        rbDepartment.DataSource = ds.Tables[0];
        rbDepartment.DataTextField = "type";
        rbDepartment.DataValueField = "id";
        rbDepartment.DataBind();
    }
    private void FillSupervisor()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getSupervisorActive(objPropUser);

        rbSupervisor.DataSource = ds.Tables[0];
        rbSupervisor.DataTextField = "fuser";
        rbSupervisor.DataValueField = "fuser";
        rbSupervisor.DataBind();
        rbSupervisor.Items.Insert(0, (new System.Web.UI.WebControls.ListItem("All", "All")));
    }
    private void BindEmployee()
    {
        try
        {

            DataSet ds = new DataSet();
            objProp_Emp.ConnConfig = Session["config"].ToString();

            ds = new BL_Wage().GetEmployeeList(objProp_Emp);
            if (rbEmployee.Items.Count > 0)
            {
                rbEmployee.Items.Clear();
            }
            rbEmployee.DataSource = ds.Tables[0];
            rbEmployee.DataTextField = "Name";
            rbEmployee.DataValueField = "ID";
            rbEmployee.DataBind();
            rbEmployee.Items.Insert(0, (new System.Web.UI.WebControls.ListItem("All", "0")));

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

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

}
