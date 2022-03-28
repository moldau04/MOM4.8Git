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

public partial class TicketReport : System.Web.UI.Page
{
    GeneralFunctions objgn = new GeneralFunctions();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    #region PAGELOAD
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {
            HighlightSideMenu("schMgr", "lnkListView", "schdMgrSub");

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

            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
            objMapData.ISTicketD = 0;
            DataSet ds = objBL_MapData.GetTicketByID(objMapData);

            if (ds != null)
            {
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.DBName = Session["dbname"].ToString();
                objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["lid"].ToString());
                DataSet dsloc = objBL_User.getLocationByID(objPropUser);

                txtSubject.Text = "Ticket " + Request.QueryString["id"].ToString() + " - " + dsloc.Tables[0].Rows[0]?["tag"].ToString();
            }
        }
    }
    #endregion

    protected void StiWebViewerTicket_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        e.Report = LoadTicketReport();
    }

    protected void StiWebViewerTicket_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {

    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddTicket.aspx" + HttpContext.Current.Request.Url.Query);
    }

    private StiReport LoadTicketReport()
    {
        string reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/TicketReport.mrt");
        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TicketReport"]) )
        {
            reportPathStimul = Server.MapPath("StimulsoftReports/Tickets/" + ConfigurationManager.AppSettings["TicketReport"]);
        }

        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        //report.Compile();

        report.Dictionary.Variables["Username"].Value = Session["Username"].ToString();

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

        // Company details
        DataSet dsCompany = new DataSet();
        BL_Report bL_Report = new BL_Report();
        dsCompany = bL_Report.GetCompanyDetails(Session["config"].ToString());

        report.RegData("CompanyDetails", dsCompany.Tables[0]);

        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
        objMapData.ISTicketD = 0;
        DataSet ds = objBL_MapData.GetTicketByID(objMapData);
        DataSet dsEquip = objBL_MapData.getElevByTicket(objMapData);

        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
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
                }

                ds.Tables[0].Columns.Add("rtaddress");
                ds.Tables[0].Columns.Add("osaddress");
                ds.Tables[0].Columns.Add("ctaddress");
                string tech = ds.Tables[0].Rows[0]["dwork"].ToString();
                string date = ds.Tables[0].Rows[0]["edate"].ToString();
                string time = ds.Tables[0].Rows[0]["timeroute"].ToString();

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

            report.RegData("ReportData", ds.Tables[0]);

            if (ds.Tables.Count > 1)
            {
                report.RegData("dtPOItem", ds.Tables[1]);
            }

            if (ds.Tables.Count > 2)
            {
                report.RegData("dtTicketItem", ds.Tables[2]);
            }
        }

        if (dsEquip != null)
        {
            report.RegData("dtEquipment", dsEquip.Tables[0]);
        }

        report.RegData("dtMCP", FillREPHistory());
        report.CacheAllData = true;
        report.Render();

        return report;
    }

    private DataSet GetCustomFields(string name)
    {
        DataSet ds = new DataSet();
        objGeneral.CustomName = name;
        objGeneral.ConnConfig = Session["config"].ToString();
        ds = objBL_General.getCustomFields(objGeneral);

        return ds;
    }

    private DataTable FillREPHistory()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.EquipID = 0;
        objPropUser.SearchBy = "rd.ticketID";
        objPropUser.SearchValue = Request.QueryString["id"].ToString();

        DataSet ds = new DataSet();
        ds = objBL_User.getequipREPDetails(objPropUser);

        return ds.Tables[0];
    }

    protected void chkCoord_CheckedChanged(object sender, EventArgs e)
    {
        StiWebViewerTicket.Visible = true;
        StiWebViewerTicket.Report = LoadTicketReport();
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

                mail.Title = txtSubject.Text;
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
                service.ExportTo(LoadTicketReport(), stream, settings);
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
                service.ExportTo(LoadTicketReport(), stream, settings);
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

    private void SetAddress()
    {
        var address = WebBaseUtility.GetSignature();

        string mailBody = "Please review the attached Ticket Report.";
        address = mailBody + "<br /><br />" + address;

        txtBody.Text = address;

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