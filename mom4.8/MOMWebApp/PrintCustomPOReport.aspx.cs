using AjaxControlToolkit;
using BusinessEntity;
using BusinessLayer;
using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;
using Stimulsoft.Report;
using Stimulsoft.Report.Web;
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
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PrintCustomPOReport : System.Web.UI.Page
{
    #region Variable
    GeneralFunctions objgn = new GeneralFunctions();
    PO _objPO = new PO();
    Contracts _objContracts = new Contracts();
    User _objPropUser = new User();

    BL_Bills _objBLBills = new BL_Bills();
    BL_User objBL_User = new BL_User();
    BL_Report bL_Report = new BL_Report();

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
                HighlightSideMenu("purchaseMgr", "lnkPO", "purchaseMgrSub");

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

                string fileName = string.Empty;
                if (Request.QueryString["report"] != null)
                {
                    Session["ReportName"] = HttpUtility.UrlDecode(Request.QueryString["report"]);

                    fileName = string.Format("{0}_{1}.pdf", Session["ReportName"].ToString(), Request.QueryString["id"]);
                    ArrayList lstPath = new ArrayList();
                    if (ViewState["pathmailatt"] != null)
                    {
                        lstPath = (ArrayList)ViewState["pathmailatt"];
                        lstPath.Add(fileName);
                    }
                    else
                    {
                        lstPath.Add(fileName);
                    }

                    ViewState["pathmailatt"] = lstPath;
                    dlAttachmentsDelete.DataSource = lstPath;
                    dlAttachmentsDelete.DataBind();

                    hdnFirstAttachement.Value = fileName;
                }

                GetSMTPUser();
                FillDistributionList("0", "");

                _objPropUser.ConnConfig = Session["config"].ToString();
                var dsC = objBL_User.getControl(_objPropUser);
                if (dsC.Tables[0].Rows.Count > 0)
                {
                    txtFrom.Text = WebBaseUtility.GetFromEmailAddress();
                    txtSubject.Text = string.Format("{0} {1} - {2}", ConfigurationManager.AppSettings["CustomerName"], Session["ReportName"].ToString(), Request.QueryString["id"]);
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

                    address = "Please review the attached Purchase Order from: " + Environment.NewLine + "<br />" + Environment.NewLine + "<br />" + address;
                    ViewState["company"] = address;
                    txtBody.Text = address;
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
                StiWebViewer rvTemplate = new StiWebViewer();
                List<byte[]> poToPrint = PrintTemplate(rvTemplate, string.Format("{0}.mrt", Session["ReportName"].ToString()));

                if (poToPrint != null)
                {
                    buffer = ConcatAndAddContent(poToPrint);
                    string FileName = string.Format("{0}_{1}.pdf", Session["ReportName"].ToString(), Request.QueryString["id"]);

                    SendMail(buffer, FileName);

                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'No records are available to send a mail',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception exp)
        {
            //string str = exp.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(exp.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
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
        if (hdnFirstAttachement.Value != "-1")
        {
            mail.attachmentBytes = buffer;
            mail.DeleteFilesAfterSend = true;
        }
        mail.RequireAutentication = false;

        ArrayList lst = new ArrayList();
        if (ViewState["pathmailatt"] != null)
        {
            string _fileName = string.Format("{0}_{1}.pdf", Session["ReportName"].ToString(), Request.QueryString["id"]);
            lst = (ArrayList)ViewState["pathmailatt"];
            foreach (string strpath in lst)
            {
                if (strpath != _fileName)
                {
                    mail.AttachmentFiles.Add(strpath);
                }
            }
        }

        WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
        mail.Send();
    }

    protected void StiWebViewerCustomPO_GetReport(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        if (Session["ReportName"] != null)
        {
            string reportPathStimul = string.Empty;
            reportPathStimul = Server.MapPath(string.Format("Templates/POTemplates/{0}.mrt", Session["ReportName"].ToString()));
            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            e.Report = report;
        }
    }

    protected void StiWebViewerCustomPO_GetReportData(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        var report = e.Report;
        try
        {
            DataSet dsC = new DataSet();

            _objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(_objPropUser);

            DataSet companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());
            report.RegData("CompanyDetails", companyInfo.Tables[0]);

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
            DataSet dsPO = _objBLBills.GetPOById(_objPO);

            if (dsPO != null)
            {
                report.RegData("POInfo", dsPO.Tables[0]);
                report.RegData("POItems", dsPO.Tables[1]);
            }

            e.Report = report;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    #endregion

    protected void btnPrintReport_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["ReportName"] != null)
            {
                StiWebViewer rvTemplate = new StiWebViewer();
                List<byte[]> poToPrint = PrintTemplate(rvTemplate, string.Format("{0}.mrt", Session["ReportName"].ToString()));

                if (poToPrint != null)
                {
                    byte[] buffer =  ConcatAndAddContent(poToPrint);

                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}_{1}.pdf", Session["ReportName"].ToString(), Request.QueryString["id"]));
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Length", (buffer.Length).ToString());
                    Response.BinaryWrite(buffer);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No Record found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
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

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
        RadGrid_Emails.Rebind();
        //UpdateSelectedRows
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        // ddlSearch_SelectedIndexChanged(sender, e);
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
            //FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
        }
        else
        {
            Session["Emails_FilterExpression"] = null;
            Session["Emails_Filters"] = null;
        }
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
    }

    //protected override void OnPreRender(EventArgs e)
    //{
    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "pageLoadHandler", startscript + endscript, false);

    //    base.OnPreRender(e);
    //}

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
        _objPropUser.ConnConfig = Session["config"].ToString();
        _objPropUser.UserID = Convert.ToInt32(Session["UserID"]);
        DataSet ds = new DataSet();
        ds = objBL_User.getSMTPByUserID(_objPropUser);
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
        //MoveFileEx(filepath, null, MoveFileFlags.MOVEFILE_DELAY_UNTIL_REBOOT);

        if (System.IO.File.Exists(filepath))
        {
            // Use a try block to catch IOExceptions, to 
            // handle the case of the file already being 
            // opened by another process. 
            try
            {
                System.IO.File.Delete(filepath);
            }
            catch //(System.IO.IOException e)
            {
                //Console.WriteLine(e.Message);
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
            string POID = Request.QueryString["id"];
            byte[] buffer = null;

            if (POID != "")
            {
                StiWebViewer rvTemplate = new StiWebViewer();
                List<byte[]> poToPrint = PrintTemplate(rvTemplate, string.Format("{0}.mrt", Session["ReportName"].ToString()));

                if (poToPrint != null)
                {
                    buffer = ConcatAndAddContent(poToPrint);

                    Response.Clear();
                    MemoryStream ms = new MemoryStream(buffer);
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + DownloadFileName);
                    Response.Buffer = true;
                    ms.WriteTo(Response.OutputStream);
                    Response.End();
                }
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "FileaccessWarning", "alert('File not found.');", true);
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('Please provide access permissions to the file path.');", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileerrorWarning", "alert('" + str + "');", true);
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

    private List<byte[]> PrintTemplate(StiWebViewer rvTemplate, string templateName)
    {
        // Export to PDF
        List<byte[]> templateAsBytes = new List<byte[]>();
        try
        {
            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            _objContracts.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    _objContracts.isTS = 1;
            }


            DataSet dsC = new DataSet();
            _objPropUser.ConnConfig = Session["config"].ToString();
            if (Session["MSM"].ToString() != "TS")
            {
                dsC = objBL_User.getControl(_objPropUser);
            }
            else
            {
                _objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
                dsC = objBL_User.getControlBranch(_objPropUser);
            }

            string reportPathStimul = string.Empty;
            reportPathStimul = Server.MapPath(string.Format("Templates/POTemplates/{0}", templateName));
            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();


            DataSet companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());
            report.RegData("CompanyDetails", companyInfo.Tables[0]);


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
            DataSet dsPO = _objBLBills.GetPOById(_objPO);

            if (dsPO != null)
            {
                report.RegData("POInfo", dsPO.Tables[0]);
                report.RegData("POItems", dsPO.Tables[1]);
            }

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