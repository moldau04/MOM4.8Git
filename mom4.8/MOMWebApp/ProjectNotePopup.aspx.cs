using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class ProjectNotePopup : System.Web.UI.Page
{
    GeneralFunctions objgn = new GeneralFunctions();
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    protected void Page_PreInit(object sender, System.EventArgs e)

    {
        Control header = Page.Master.FindControl("divHeader");
        header.Visible = false;
        Control menu = Page.Master.FindControl("menu");
        menu.Visible = false;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Title = "Email";

        if (!IsPostBack)
        {
            GetSMTPUser();
            var notesContent = string.Empty;
            if (Request.QueryString["subject"] != null && Request.QueryString["subject"] != "")
            {
                txtSubject.Text = HttpUtility.UrlDecode(Request.QueryString["subject"].ToString());
            }

            if(Request.QueryString["ls"] != null && Request.QueryString["ls"].ToString() != "")
            {
                notesContent = BuildNotesTable(Request.QueryString["ls"].ToString());
            }

            SetAddress(notesContent);

            //if (Session["ProjNoteEmailStatus"] != null && Session["ProjNoteEmailStatus"].ToString() != "")
            //{
            //    if (Session["ProjNoteEmailStatus"].ToString() == "1")
            //    {
            //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            //        Session["ProjNoteEmailStatus"] = null;
            //    }
            //    else if (Session["ProjNoteEmailStatus"].ToString() == "0")
            //    {
            //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'There are some errors on send email!',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            //        Session["ProjNoteEmailStatus"] = null;
            //    }
            //}
        }
    }

    private string BuildNotesTable(String ls)
    {
        StringBuilder notesTable = new StringBuilder();
        
        BL_Customer objBL_Customer = new BL_Customer();
        Customer objProp_Note = new Customer();
        objProp_Note.ConnConfig = Session["config"].ToString();
        objProp_Note.job = Convert.ToInt32(Request.QueryString["uid"].ToString());
        DataSet dsNotes = objBL_Customer.GetJobProject_NotesExport(objProp_Note, ls);
        if (dsNotes.Tables[0].Rows.Count > 0)
        {
            notesTable.Append("<p>");
            notesTable.Append("<table style='border:none;'>");
            notesTable.Append("<tr><td style='border:none;min-width:200px;max-width:500px;'>Note</td><td style='border:none;width:200px;'>Date/Time</td><td style='border:none;min-width:150px;max-width:300px;'>User</td></tr>");
            foreach (DataRow item in dsNotes.Tables[0].Rows)
            {
                notesTable.AppendFormat("<tr><td style='border:none;min-width:200px;max-width:500px;'>{0}</td><td style='border:none;width:200px;'>{1}</td><td style='border:none;min-width:150px;max-width:300px;'>{2}</td></tr>"
                    , item["Note"], ((DateTime)item["CreatedDate"]).ToString("MM/dd/yyyy hh:mm tt"), item["CreatedBy"]);
            }
            notesTable.Append("</table>");
        }

        return notesTable.ToString();
    }

    //protected void RadGrid_CollectionNotes_Export_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    //{
    //    LoadProjectNoteExport(Request.QueryString["ls"].ToString());
    //}
    //private void LoadProjectNoteExport(String ls)
    //{
    //    BL_Customer objBL_Customer = new BL_Customer();
    //    Customer objProp_Note = new Customer();
    //    objProp_Note.ConnConfig = Session["config"].ToString();
    //    objProp_Note.job = Convert.ToInt32(Request.QueryString["uid"].ToString());
    //    DataSet dsNotes = objBL_Customer.GetJobProject_NotesExport(objProp_Note, ls);
    //    if (dsNotes.Tables[0].Rows.Count > 0)
    //    {
    //        RadGrid_CollectionNotes_Export.DataSource = dsNotes.Tables[0];
    //    }
    //}

    //protected void RadGrid_CollectionNotes_Export_GridExporting(object sender, GridExportingArgs e)
    //{
    //    MemoryStream gridMemoryStream = new MemoryStream(new ASCIIEncoding().GetBytes(e.ExportOutput));
    //    try
    //    {
    //        SendEmailWithAttachment(gridMemoryStream);

    //        gridMemoryStream.Close();
    //        Session["ProjNoteEmailStatus"] = "1";
    //        Response.Redirect(Request.RawUrl, false);
    //        Context.ApplicationInstance.CompleteRequest();
    //    }
    //    catch(Exception ex)
    //    {
    //        gridMemoryStream.Close();
    //        string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
    //        if(str.IndexOf("Get SentItems Error") == 0)
    //        {
    //            Session["ProjNoteEmailStatus"] = "1";
    //        }
    //        else
    //        {
    //            Session["ProjNoteEmailStatus"] = "0";
    //        }

    //        Response.Redirect(Request.RawUrl, false);
    //        Context.ApplicationInstance.CompleteRequest();
    //    }
    //}
    //private void SendEmailWithAttachment(MemoryStream gridMemoryStream)
    //{
    //    if (txtTo.Text.Trim() != string.Empty)
    //    {
    //        Mail mail = new Mail();
    //        mail.From = txtFrom.Text.Trim();
    //        mail.To = txtTo.Text.Split(';', ',').OfType<string>().ToList();
    //        if (txtCC.Text.Trim() != string.Empty)
    //        {
    //            mail.Cc = txtCC.Text.Split(';', ',').OfType<string>().ToList();
    //        }

    //        if (txtEmailBCC.Text.Trim() != string.Empty)
    //        {
    //            mail.Bcc = txtEmailBCC.Text.Split(';', ',').OfType<string>().ToList();
    //        }

    //        mail.Title = txtSubject.Text;
    //        if (txtBody.Text.Trim() != string.Empty)
    //        {
    //            mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
    //        }
    //        else
    //        {
    //            mail.Text = "This is report email sent from Mobile Office Manager. Please find the Note.xls attached.";
    //        }


    //        mail.attachmentBytes = gridMemoryStream.ToArray();
    //        ArrayList lst = new ArrayList();
    //        if (ViewState["pathmailatt"] != null)
    //        {
    //            lst = (ArrayList)ViewState["pathmailatt"];
    //            foreach (string strpath in lst)
    //            {
    //                mail.AttachmentFiles.Add(strpath);

    //            }
    //        }

    //        mail.FileName = "Note.xls";

    //        mail.DeleteFilesAfterSend = true;
    //        WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
    //        mail.Send();
    //    }
    //}

    private void SendEmail()
    {
        if (txtTo.Text.Trim() != string.Empty)
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
                mail.Text = txtBody.Text;//.Replace(Environment.NewLine, "<BR/>");
            }
            //else
            //{
            //    mail.Text = "This is report email sent from Mobile Office Manager. Please find the Note.xls attached.";
            //}


            //mail.attachmentBytes = gridMemoryStream.ToArray();
            ArrayList lst = new ArrayList();
            if (ViewState["pathmailatt"] != null)
            {
                lst = (ArrayList)ViewState["pathmailatt"];
                foreach (string strpath in lst)
                {
                    mail.AttachmentFiles.Add(strpath);
                }
            }

            //mail.FileName = "Note.xls";

            //mail.DeleteFilesAfterSend = true;
            WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
            mail.IsBodyHtml = true;
            mail.Send();
        }
    }

    #region Email
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
        RadGrid_Emails.Rebind();
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
    }


    protected void lnkClear_Click(object sender, EventArgs e)
    {
        ddlSearch.SelectedIndex = 0;
        txtSearch.Text = string.Empty;
        FillDistributionList(ddlSearch.SelectedValue, txtSearch.Text);
        RadGrid_Emails.Rebind();
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "UpdateSelectedRows", "UpdateSelectedRowsForGrid();", true);
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
        if (txtTo.Text != "")
        {
            //try
            //{
            //    RadGrid_CollectionNotes_Export.Rebind();

            //    RadGrid_CollectionNotes_Export.ExportSettings.HideStructureColumns = true;
            //    RadGrid_CollectionNotes_Export.ExportSettings.IgnorePaging = true;
            //    RadGrid_CollectionNotes_Export.ExportSettings.OpenInNewWindow = false;
            //    RadGrid_CollectionNotes_Export.ExportSettings.ExportOnlyData = true;
            //    RadGrid_CollectionNotes_Export.MasterTableView.ExportToExcel();
            //}
            //catch (Exception ex)
            //{
            //    string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            //}

            try
            {
                SendEmail();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                //Session["ProjNoteEmailStatus"] = "1";
                //Response.Redirect(Request.RawUrl, false);
                //Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                if (str.IndexOf("Get SentItems Error") == 0)
                {
                    //Session["ProjNoteEmailStatus"] = "1";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Email sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                }
                else
                {
                    //Session["ProjNoteEmailStatus"] = "0";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

                }

                //Response.Redirect(Request.RawUrl, false);
                //Context.ApplicationInstance.CompleteRequest();
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
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "FileaccessWarning", "alert('File not found.');", true);
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

    private void SetAddress(string mailBody)
    {
        DataSet dsC = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();

        dsC = objBL_User.getControl(objPropUser);

        if (dsC.Tables[0].Rows.Count > 0)
        {
            StringBuilder address = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["name"])))
            {
                address.AppendFormat("{0}<br/>", dsC.Tables[0].Rows[0]["name"].ToString());// + Environment.NewLine;
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["Address"])))
            {
                address.AppendFormat("{0}<br/>", dsC.Tables[0].Rows[0]["Address"].ToString());// + Environment.NewLine;
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["city"])))
            {
                address.AppendFormat("{0}, ", dsC.Tables[0].Rows[0]["city"].ToString());// + ", ";
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["state"])))
            {
                address.AppendFormat("{0}, ", dsC.Tables[0].Rows[0]["state"].ToString());// + ", ";
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["zip"])))
            {
                address.AppendFormat("{0}<br/>", dsC.Tables[0].Rows[0]["zip"].ToString());// + Environment.NewLine;
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["Phone"])))
            {
                address.AppendFormat("Phone: {0}<br/>", dsC.Tables[0].Rows[0]["Phone"].ToString());// + Environment.NewLine;
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["fax"])))
            {
                address.AppendFormat("Fax: {0}<br/>", dsC.Tables[0].Rows[0]["fax"].ToString());// + Environment.NewLine;
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["email"])))
            {
                if (!ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("QAE"))
                    address.AppendFormat("Email: {0}<br/>", dsC.Tables[0].Rows[0]["email"].ToString());// + Environment.NewLine;
            }
            //string mailBody = "Please review the attached.";
            address.Insert(0, mailBody + "<p>");

            txtBody.Text = address.ToString();
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


    #endregion
}