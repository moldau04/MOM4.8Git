using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using Stimulsoft.Report;
using Telerik.Web.UI;

public partial class PrintTicketReport : System.Web.UI.Page
{
    #region Variable
    GeneralFunctions objgn = new GeneralFunctions();
    ApprovePOStatus _objApprovePOStatus = new ApprovePOStatus();

    BL_Bills _objBLBills = new BL_Bills();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    BL_Report bL_Report = new BL_Report();

    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    #endregion

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
                string[] tokens = Session["config"].ToString().Split(';');
                if (tokens[1].ToString().ToLower().IndexOf("madden") == -1)
                {
                    ListItem laborReport = ddlReport.Items.FindByValue("12");
                    ddlReport.Items.Remove(laborReport);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["ddlSearchReportID"]))
                {
                    ddlReport.SelectedValue = Request.QueryString["ddlSearchReportID"];
                }

                GetSMTPUser();
                SetAddress();
                string FileName = "TicketReport.pdf";
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
                if (Request.QueryString["ddlSearchReportID"] == "20")
                {
                    divlblCategory.Visible = true;
                    divdrpCategory.Visible = true;
                    ddlcategory.DataBind();
                    if (Session["ddlCategorylist"] != null && Convert.ToString(Session["ddlCategorylist"]) != "")
                    {
                        string[] category = Session["ddlCategorylist"].ToString().Split(',');
                        foreach (var cat in category)
                        {
                            var item = ddlcategory.Items.FindItem(x => x.Value.ToUpper() == cat.Replace("'","").ToString().ToUpper());
                            if (item != null)
                            {
                                item.Checked = true;
                            }
                        }
                    }
                }
                else
                {
                    divlblCategory.Visible = false;
                    divdrpCategory.Visible = false;
                }
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
        Response.Redirect("TicketListView.aspx");
    }

    protected void btnLoad_Click(object sender, EventArgs e)
    {
        var routes = GetRadComboBoxSelectedItems(ddlcategory);
        Session["ddlCategorylist"] = routes;
        var url = Request.Url.ToString();
        url = RemoveQueryStringByKey(url, "ddlSearchReportID");
        url += "&ddlSearchReportID=" + ddlReport.SelectedValue;

        Response.Redirect(url);
    }

    protected void StiWebViewerTicket_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        e.Report = LoadTicketReport(e.Action.ToString());
    }

    protected void StiWebViewerTicket_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {

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

                mail.Title = "Ticket Report";
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This is report email sent from Mobile Office Manager. Please find the Ticket Report attached.";
                }

                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(LoadTicketReport("GetReport"), stream, settings);
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
                        if (strpath != "TicketReport.pdf")
                        {
                            mail.AttachmentFiles.Add(strpath);
                        }
                    }
                }

                mail.FileName = "TicketReport.pdf";

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
            if (DownloadFileName == "TicketReport.pdf")
            {
                byte[] buffer1 = null;

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(LoadTicketReport("GetReport"), stream, settings);
                buffer1 = stream.ToArray();

                Response.Clear();
                MemoryStream ms = new MemoryStream(buffer1);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=TicketReport.pdf");
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

    protected void RadComboBox1_DataBinding(object sender, EventArgs e)
    {
        ddlcategory.DataValueField = "Type";
        ddlcategory.DataTextField = "Type";
        ddlcategory.DataSource = GetSampleSource();
    }

    private DataSet GetSampleSource()
    {
        DataSet ds = new DataSet();
        objGeneral.ConnConfig = Session["config"].ToString();
        ds = objBL_General.getAllCategoryList(objGeneral);
        return ds;
    }

    private StiReport LoadTicketReport(string actiontype)
    {
        StiReport report = new StiReport();

        if (Request.QueryString["ddlSearchReportID"] != null)
        {
            string reportPathStimul = string.Empty;

            if (Request.QueryString["ddlSearchReportID"] == "0")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/TicketList.mrt");
                //reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/TicketListReport.mrt");
                if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings["NewTicketReport"]) && WebConfigurationManager.AppSettings["NewTicketReport"].ToLower().Contains(".mrt"))
                {
                    reportPathStimul = Server.MapPath($"StimulsoftReports/Tickets/{WebConfigurationManager.AppSettings["NewTicketReport"]}");
                }
            }
            else if (Request.QueryString["ddlSearchReportID"] == "1")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/TicketExpensesReport.mrt");
            }
            else if (Request.QueryString["ddlSearchReportID"] == "2")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/TicketListTimeReport.mrt");
            }
            else if (Request.QueryString["ddlSearchReportID"] == "3")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/CallbackReport.mrt");
            }
            else if (Request.QueryString["ddlSearchReportID"] == "4")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/TicketDetailsReport.mrt");
                if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings["TicketDetailsReport"]) && WebConfigurationManager.AppSettings["TicketDetailsReport"].ToLower().Contains(".mrt"))
                {
                    reportPathStimul = Server.MapPath($"StimulsoftReports/Tickets/{WebConfigurationManager.AppSettings["TicketDetailsReport"]}");
                }
            }
            else if (Request.QueryString["ddlSearchReportID"] == "5")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/TicketSignatureReport.mrt");
            }
            else if (Request.QueryString["ddlSearchReportID"] == "6")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/TicketListByWOReport.mrt");
            }
            else if (Request.QueryString["ddlSearchReportID"] == "7")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/TicketListWorkerReport.mrt");
                if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings["TicketListWorkerReport"]) && WebConfigurationManager.AppSettings["TicketListWorkerReport"].ToLower().Contains(".mrt"))
                {
                    reportPathStimul = Server.MapPath($"StimulsoftReports/Tickets/{WebConfigurationManager.AppSettings["TicketListWorkerReport"]}");
                }
            }
            else if (Request.QueryString["ddlSearchReportID"] == "8")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/TicketListReport.mrt");
                if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings["TicketListReport"]) && WebConfigurationManager.AppSettings["TicketListReport"].ToLower().Contains(".mrt"))
                {
                    reportPathStimul = Server.MapPath($"StimulsoftReports/Tickets/{WebConfigurationManager.AppSettings["TicketListReport"]}");
                }
            }
            else if (Request.QueryString["ddlSearchReportID"] == "9")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/InstallationScheduleReport.mrt");
            }
            else if (Request.QueryString["ddlSearchReportID"] == "10")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/MonthlyMaintenanceReport.mrt");
            }
            else if (Request.QueryString["ddlSearchReportID"] == "11")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/ServiceScheduleReport.mrt");
                if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings["ServiceScheduleReport"]) && WebConfigurationManager.AppSettings["ServiceScheduleReport"].ToLower().Contains(".mrt"))
                {
                    if (actiontype == "ExportReport" && WebConfigurationManager.AppSettings["CustomerName"].ToString().ToUpper() == "Colley".ToUpper())
                    {
                        reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/ServiceScheduleReportExcelFormat-Colley.mrt");
                    }
                    else
                    {
                        reportPathStimul = Server.MapPath($"StimulsoftReports/Tickets/{WebConfigurationManager.AppSettings["ServiceScheduleReport"]}");
                    }
                }
            }
            else if (Request.QueryString["ddlSearchReportID"] == "12")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/LaborReport.mrt");
            }
            else if (Request.QueryString["ddlSearchReportID"] == "13")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/TicketListTimeByDepartment.mrt");
            }
            else if (Request.QueryString["ddlSearchReportID"] == "14")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/TicketListTimeNoTTReport.mrt");
                if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings["TicketListTimeNoTTReport"]) && WebConfigurationManager.AppSettings["TicketListTimeNoTTReport"].ToLower().Contains(".mrt"))
                {
                    reportPathStimul = Server.MapPath($"StimulsoftReports/Tickets/{WebConfigurationManager.AppSettings["TicketListTimeNoTTReport"]}");
                }
            }
            else if (Request.QueryString["ddlSearchReportID"] == "15")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/TicketListTimeByWageCategory.mrt");
            }
            else if (Request.QueryString["ddlSearchReportID"] == "16")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/TimesheetCertifiedProjectReport.mrt");
            }
            else if (Request.QueryString["ddlSearchReportID"] == "17")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/ModTimesheetReport.mrt");
            }

            else if (Request.QueryString["ddlSearchReportID"] == "20")
            {
                reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/TicketCategoryHistory.mrt");
            }


            if (string.IsNullOrEmpty(reportPathStimul))
            {
                return null;
            }

            report.Load(reportPathStimul);
            //report.Compile();

            objPropUser.ConnConfig = Session["config"].ToString();
            DataSet dsC = objBL_User.getControl(objPropUser);

            report.RegData("CompanyDetails", dsC.Tables[0]);

            DataTable dt = new DataTable();
            string TicketListQuery = Session["TicketListQuery"].ToString();
            DataSet dsrpt = new BusinessLayer.Schedule.BL_Tickets().GetTicketListReportDatabyQuery(TicketListQuery, Session["config"].ToString());

            dt = dsrpt.Tables[0];

            if (Request.QueryString["ddlSearchReportID"] == "6")
            {
                var Tickets = dt.AsEnumerable()
               .OrderBy(o => o.Field<int>("id"))
               .GroupBy(g => g.Field<string>("workorder"))
               .Select(s => new { s, Count = s.Count() })
               .SelectMany(sm => sm.s.Select(s => s).Zip(Enumerable.Range(1, sm.Count), (row, index) =>
                  new
                  {
                      RowNo = index,
                      who = row.Field<object>("who"),
                      lid = row.Field<object>("lid"),
                      locid = row.Field<object>("locid"),
                      assigned = row.Field<object>("assigned"),
                      fulladdress = row.Field<object>("fulladdress"),
                      WorkOrder = row.Field<object>("workorder"),
                      Reg = row.Field<object>("Reg"),
                      OT = row.Field<object>("OT"),
                      NT = row.Field<object>("NT"),
                      DT = row.Field<object>("DT"),
                      TT = row.Field<object>("TT"),
                      Total = row.Field<object>("Total"),
                      ClearCheck = row.Field<object>("ClearCheck"),
                      charge = row.Field<object>("charge"),
                      fDesc = row.Field<object>("fDesc"),
                      TimeRoute = row.Field<object>("TimeRoute"),
                      TimeSite = row.Field<object>("TimeSite"),
                      TimeComp = row.Field<object>("TimeComp"),
                      comp = row.Field<object>("comp"),
                      dwork = row.Field<object>("dwork"),
                      lastname = row.Field<object>("lastname"),
                      hourlyrate = row.Field<object>("hourlyrate"),
                      ID = row.Field<object>("ID"),
                      customername = row.Field<object>("customername"),
                      locname = row.Field<object>("locname"),
                      address = row.Field<object>("address"),
                      phone = row.Field<object>("phone"),
                      Cat = row.Field<object>("Cat"),
                      edate = row.Field<object>("edate"),
                      CDate = row.Field<object>("CDate"),
                      descres = row.Field<object>("descres"),
                      assignname = row.Field<object>("assignname"),
                      Est = row.Field<object>("Est"),
                      Tottime = row.Field<object>("Tottime"),
                      timediff = row.Field<object>("timediff"),
                      workorder = row.Field<object>("workorder"),
                      expenses = row.Field<object>("expenses"),
                      zone = row.Field<object>("zone"),
                      toll = row.Field<object>("toll"),
                      othere = row.Field<object>("othere"),
                      extraexp = row.Field<object>("extraexp"),
                      mileagetravel = row.Field<object>("mileagetravel"),
                      mileage = row.Field<object>("mileage"),
                      signatureCount = row.Field<object>("signatureCount"),
                      DocumentCount = row.Field<object>("DocumentCount"),
                      workerid = row.Field<object>("workerid"),
                      description = row.Field<object>("description"),
                      fdescreason = row.Field<object>("fdescreason"),
                      invoice = row.Field<object>("invoice"),
                      Confirmed = row.Field<object>("Confirmed"),
                      manualinvoice = row.Field<object>("manualinvoice"),
                      invoiceno = row.Field<object>("invoiceno"),
                      ownerid = row.Field<object>("ownerid"),
                      QBinvoiceid = row.Field<object>("QBinvoiceid"),
                      TransferTime = row.Field<object>("TransferTime"),
                      serviceitem = row.Field<object>("serviceitem"),
                      PayrollItem = row.Field<object>("PayrollItem"),
                      RTOTTT = row.Field<object>("RTOTTT"),
                      timesign = row.Field<object>("timesign"),
                      dispalert = row.Field<object>("dispalert"),
                      credithold = row.Field<object>("credithold"),
                      high = row.Field<object>("high"),
                      unitid = row.Field<object>("unitid"),
                      unit = row.Field<object>("unit"),
                      defaultworker = row.Field<object>("defaultworker"),
                      defaultmech = row.Field<object>("defaultmech"),
                      department = row.Field<object>("department"),
                      bremarks = row.Field<object>("bremarks"),
                      laborexp = row.Field<object>("laborexp"),
                      signature = row.Field<object>("signature"),
                      state = row.Field<object>("state"),
                      mileagepr = row.Field<object>("mileagepr"),
                      afterhours = row.Field<object>("afterhours"),
                      weekends = row.Field<object>("weekends"),
                      EmailNotified = row.Field<object>("EmailNotified"),
                      EmailTime = row.Field<object>("EmailTime"),
                      PartsUsed = row.Field<object>("PartsUsed")
                  }));

                DataTable dtWo = new DataTable();
                if (Tickets.Any())
                {
                    dtWo = ConvertToDataTable(Tickets);
                }

                report.RegData("ReportData", dtWo);
            }

            else
            {
                if (Request.QueryString["ddlSearchReportID"] == "20")
                {
                    if (Session["ddlCategorylist"] != null && Convert.ToString(Session["ddlCategorylist"]) != "")
                    {
                        DataTable table = new DataTable();
                        string[] category = Session["ddlCategorylist"].ToString().Replace("'","").Split(',');
                        var rows = from row in dt.AsEnumerable()
                                   where category.Contains(row.Field<string>("Cat"))
                                   select row;
                        DataRow[] rowsArray = rows.ToArray();
                        if (rowsArray.Length > 0)
                        {
                            dt = rowsArray.CopyToDataTable();
                        }
                        else
                        {
                            dt = null;
                        }

                    }
                }
                report.RegData("ReportData", dt);

                if (Request.QueryString["ddlSearchReportID"] == "4")
                {
                    if (report.Dictionary.Variables.Contains("CustomLabel1"))
                    {
                        var custom1 = GetCustomFields("Loc1");
                        if (custom1.Tables[0].Rows.Count > 0)
                        {
                            report.Dictionary.Variables["CustomLabel1"].Value = custom1.Tables[0].Rows[0]["label"].ToString();
                        }
                    }

                    if (report.Dictionary.Variables.Contains("CustomLabel2"))
                    {
                        var custom2 = GetCustomFields("Loc2");
                        if (custom2.Tables[0].Rows.Count > 0)
                        {
                            report.Dictionary.Variables["CustomLabel2"].Value = custom2.Tables[0].Rows[0]["label"].ToString();
                        }
                    }

                    if (report.Dictionary.Variables.Contains("CustomLabel6"))
                    {
                        var custom6 = GetCustomFields("Ticket6");
                        if (custom6.Tables[0].Rows.Count > 0)
                        {
                            report.Dictionary.Variables["CustomLabel6"].Value = custom6.Tables[0].Rows[0]["label"].ToString();
                        }
                    }

                    if (report.Dictionary.Variables.Contains("CustomLabel7"))
                    {
                        var custom7 = GetCustomFields("Ticket7");
                        if (custom7.Tables[0].Rows.Count > 0)
                        {
                            report.Dictionary.Variables["CustomLabel7"].Value = custom7.Tables[0].Rows[0]["label"].ToString();
                        }
                    }

                    var listTicketID = dt.Rows.OfType<DataRow>()
                        .Select(dr => dr.Field<int>("ID")).ToList();

                    objMapData.ConnConfig = Session["config"].ToString();
                    DataSet dsEquips = objBL_MapData.GetElevByTicketIDs(objMapData, string.Join(",", listTicketID));
                    DataSet dsItems = objBL_MapData.GetTicketItemByIDs(objMapData, string.Join(",", listTicketID));

                    if (dsEquips != null)
                    {
                        report.RegData("dtEquipment", dsEquips.Tables[0]);
                    }

                    if (dsItems != null)
                    {
                        report.RegData("dtPOItem", dsItems.Tables[0]);
                        report.RegData("dtTicketItem", dsItems.Tables[1]);
                    }
                }

                if (report.Dictionary.Variables.Contains("TicketCustomLabel5"))
                {
                    var ticketcustom5 = GetCustomFields("TicketCst5");
                    if (ticketcustom5.Tables[0].Rows.Count > 0)
                    {
                        report.Dictionary.Variables["TicketCustomLabel5"].Value = ticketcustom5.Tables[0].Rows[0]["label"].ToString();
                    }
                }

                report.CacheAllData = true;
            }

            report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();
            report.Dictionary.Variables["StartDate"].Value = Request.QueryString["sd"];
            report.Dictionary.Variables["EndDate"].Value = Request.QueryString["ed"];

            report.Dictionary.Variables["Status"].Value = Request.QueryString["sn"];
            report.Dictionary.Variables["Supervisor"].Value = Request.QueryString["Sup"];
            report.Dictionary.Variables["Worker"].Value = Request.QueryString["Wor"];
            report.Dictionary.Variables["Chargeable"].Value = Request.QueryString["chr"];
            report.Dictionary.Variables["Reviewed"].Value = Request.QueryString["rev"];

            if (Request.QueryString["ddlSearchReportID"] == "10")
            {
                DateTime dateCurrent = Convert.ToDateTime(Request.QueryString["sd"].ToString());

                int month = dateCurrent.Month;

                int year = dateCurrent.Year;

                DateTime SWeek1 = new DateTime(year, month, 1);

                DateTime EWeek1 = SWeek1.AddDays(((int)DayOfWeek.Friday - (int)SWeek1.DayOfWeek + 7) % 7 + 1);

                DateTime SWeek2 = EWeek1.AddDays(1);

                DateTime EWeek2 = EWeek1.AddDays(7);

                DateTime SWeek3 = EWeek2.AddDays(1);

                DateTime EWeek3 = EWeek2.AddDays(7);

                DateTime SWeek4 = EWeek3.AddDays(1);

                DateTime EWeek4 = EWeek3.AddDays(7);

                DateTime SWeek5 = EWeek4.AddDays(1);

                DateTime EWeek5 = new DateTime(year, month, DateTime.DaysInMonth(year, month));

                report.Dictionary.Variables["SWeek1"].Value = SWeek1.ToString();
                report.Dictionary.Variables["SWeek2"].Value = SWeek2.ToString();
                report.Dictionary.Variables["SWeek3"].Value = SWeek3.ToString();
                report.Dictionary.Variables["SWeek4"].Value = SWeek4.ToString();
                report.Dictionary.Variables["SWeek5"].Value = SWeek5.ToString();

                report.Dictionary.Variables["EWeek1"].Value = EWeek1.AddDays(1).AddSeconds(-1).ToString();
                report.Dictionary.Variables["EWeek2"].Value = EWeek2.AddDays(1).AddSeconds(-1).ToString();
                report.Dictionary.Variables["EWeek3"].Value = EWeek3.AddDays(1).AddSeconds(-1).ToString();
                report.Dictionary.Variables["EWeek4"].Value = EWeek4.AddDays(1).AddSeconds(-1).ToString();
                report.Dictionary.Variables["EWeek5"].Value = EWeek5.AddDays(1).AddSeconds(-1).ToString();
            }

            report.Render();
        }

        return report;
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

    private DataSet GetCustomFields(string name)
    {
        DataSet ds = new DataSet();
        objGeneral.CustomName = name;
        objGeneral.ConnConfig = Session["config"].ToString();
        ds = objBL_General.getCustomFields(objGeneral);

        return ds;
    }

    private void SetAddress()
    {
        DataSet dsC = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();

        dsC = objBL_User.getControl(objPropUser);

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
            string mailBody = "Please review the attached Ticket Report.";
            address = mailBody + Environment.NewLine + "<br />" + Environment.NewLine + "<br />" + address;

            txtBody.Text = address;

        }
    }

    public DataTable ConvertToDataTable<T>(IEnumerable<T> varlist)
    {
        DataTable dtReturn = new DataTable();
        // column names 
        PropertyInfo[] oProps = null;
        if (varlist == null) return dtReturn;

        foreach (T rec in varlist)
        {
            // Use reflection to get property names, to create table, Only first time, others will follow 
            if (oProps == null)
            {
                oProps = ((Type)rec.GetType()).GetProperties();
                foreach (PropertyInfo pi in oProps)
                {
                    Type colType = pi.PropertyType;

                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }

                    dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                }
            }

            DataRow dr = dtReturn.NewRow();

            foreach (PropertyInfo pi in oProps)
            {
                dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                (rec, null);
            }

            dtReturn.Rows.Add(dr);
        }

        return dtReturn;
    }

    public string RemoveQueryStringByKey(string url, string key)
    {
        var uri = new Uri(url);

        // this gets all the query string key value pairs as a collection
        var newQueryString = HttpUtility.ParseQueryString(uri.Query);

        // this removes the key if exists
        newQueryString.Remove(key);

        // this gets the page path from root without QueryString
        string pagePathWithoutQueryString = uri.GetLeftPart(UriPartial.Path);

        return newQueryString.Count > 0
            ? String.Format("{0}?{1}", pagePathWithoutQueryString, newQueryString)
            : pagePathWithoutQueryString;
    }

    protected void ddlReport_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlReport.SelectedValue == "20")
        {
            divlblCategory.Visible = true;
            divdrpCategory.Visible = true;
            ddlcategory.DataBind();
            if (Session["ddlCategorylist"] != null && Convert.ToString(Session["ddlCategorylist"]) != "")
            {
                string[] category = Session["ddlCategorylist"].ToString().Split(',');
                foreach (var cat in category)
                {
                    var item = ddlcategory.Items.FindItem(x => x.Value.ToUpper() == cat.Replace("'", "").ToString().ToUpper());
                    if (item != null)
                    {
                        item.Checked = true;
                    }
                }
            }
        }
        else
        {
            divlblCategory.Visible = false;
            divdrpCategory.Visible = false;
        }
    }
}