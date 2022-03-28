using BusinessEntity;
using BusinessLayer;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Planner : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
            return;
        }
        else
        {
            WebBaseUtility.UpdatePageTitle(this, "Planner", Request.QueryString["plnid"], "");
            Session["GanttPlannerRefID"] = null;
            Session["GanttPlannerID"] = null;
            Session["GanttPlannerType"] = null;

            if (Request.QueryString["projid"] != null)
            {
                Session["GanttPlannerRefID"] = Request.QueryString["projid"].ToString();
                var url = "<span style='float :left'>Project #</span><a style='float :left' href='addproject?uid=" + Request.QueryString["projid"].ToString() + "'>" + Request.QueryString["projid"].ToString() + "</a>";
                trProj.InnerHtml = url.ToString();
            }
            if (Request.QueryString["plnid"] != null)
            {
                Session["GanttPlannerID"] = Request.QueryString["plnid"].ToString();
                Session["GanttPlannerType"] = "Project";

                lblPlannerNo.Text = "Planner #" + Request.QueryString["plnid"];
            }
        }

        //if (IsPostBack)
        //{
        //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "keyOpenPopup", "reOpenPopup();", true);
        //}
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["projid"] != null)
        {
            Response.Redirect("addproject.aspx?uid=" + Request.QueryString["projid"]);
        }
    }

    protected void hdnRadGanttTasks_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (Request.QueryString["plnid"] != null)
        {
            BusinessEntity.Planner planner = new BusinessEntity.Planner();
            BL_Planner bL_Planner = new BL_Planner();

            planner.ConnConfig = Session["config"].ToString();
            planner.PlannerID = Convert.ToInt32(Request.QueryString["plnid"]);
            DataSet ds = new DataSet();
            ds = bL_Planner.GetGanttTasksByPlannerID(planner);
            DataTable dataTable = ds.Tables[0];
            foreach (DataRow item in dataTable.Rows)
            {
                item["Start"] = ((DateTime)item["Start"]).ToLocalTime();
                item["End"] = ((DateTime)item["End"]).ToLocalTime();
            }
            hdnRadGanttTasks.DataSource = ds.Tables[0];
        }
        else
        {
            hdnRadGanttTasks.DataSource = null;
        }
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        hdnRadGanttTasks.ExportSettings.FileName = "GanttTasks";
        hdnRadGanttTasks.ExportSettings.IgnorePaging = true;
        hdnRadGanttTasks.ExportSettings.ExportOnlyData = true;
        hdnRadGanttTasks.ExportSettings.OpenInNewWindow = true;
        hdnRadGanttTasks.ExportSettings.HideStructureColumns = true;
        hdnRadGanttTasks.MasterTableView.UseAllDataFields = true;
        hdnRadGanttTasks.ExportSettings.Excel.Format = Telerik.Web.UI.GridExcelExportFormat.ExcelML;
        hdnRadGanttTasks.MasterTableView.ExportToExcel();
    }

    //protected void RadGrid_PO_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    //{
    //    if (Request.QueryString["projid"] != null)
    //    {
    //        BusinessEntity.Planner planner = new BusinessEntity.Planner();
    //        BL_Planner bL_Planner = new BL_Planner();

    //        planner.ConnConfig = Session["config"].ToString();
    //        planner.ProjectID = Convert.ToInt32(Request.QueryString["projid"]);
    //        DataSet ds = new DataSet();
    //        ds = bL_Planner.GetProjectPOs(planner);
    //        DataTable dataTable = ds.Tables[0];

    //        RadGrid_PO.DataSource = ds.Tables[0];
    //    }
    //    else
    //    {
    //        RadGrid_PO.DataSource = null;
    //    }
    //}

    protected void lnkDeleteDoc_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Documents.SelectedItems)
        {
            Label lblID = (Label)item.FindControl("lblId");
            HiddenField hdnTempId = (HiddenField)item.FindControl("hdnTempId");
            if (lblID.Text == "0")
            {
                DeleteDocFromTempTable(hdnTempId.Value);
            }

            DeleteFileFromFolder(string.Empty, Convert.ToInt32(lblID.Text));
        }

        ScriptManager.RegisterStartupScript(this, GetType(), "DeleteDoc", "$('.dropify').dropify();", true);
    }

    private void DeleteDocFromTempTable(string tempId)
    {
        if (string.IsNullOrWhiteSpace(tempId))
        {
            return;
        }

        var tempAttachedFiles = ViewState["AttachedFiles"] as DataTable;

        if (tempAttachedFiles == null)
        {
            return;
        }

        var deleteFileRow = tempAttachedFiles.AsEnumerable().FirstOrDefault(t => t.Field<string>("TempId") == tempId);

        if (deleteFileRow != null)
        {
            tempAttachedFiles.Rows.Remove(deleteFileRow);
        }
    }

    public void DeleteFileFromFolder(string StrFilename, int DocumentID)
    {
        try
        {
            //File.Delete(StrFilename);
            DeleteFile(DocumentID);
        }
        catch (FileNotFoundException ex)
        {
            DeleteFile(DocumentID);
        }
        catch (UnauthorizedAccessException ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(),
            "FileDeleteAccessWarning", "noty({text: 'Please provide delete permissions to the file path.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);

            ScriptManager.RegisterStartupScript(this, GetType(),
            "FileDeleteErrorWarning", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void DeleteFile(int DocumentID)
    {
        try
        {
            MapData objMapData = new MapData();
            BL_MapData objBL_MapData = new BL_MapData();
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.DocumentID = DocumentID;
            objBL_MapData.DeleteFile(objMapData);
            UpdateDocInfo();
            GetDocuments();
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrdelete", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkUploadDoc_Click(object sender, EventArgs e)
    {
        try
        {
            string filename = string.Empty;
            string fullpath = string.Empty;
            string mime = string.Empty;
            var savepath = string.Empty;

            var mainDirectory = "GanttDocs";

            if (Request.QueryString["projid"] != null)
            {
                mainDirectory += "\\Proj_" + Request.QueryString["projid"];
            }

            if (!string.IsNullOrEmpty(hdnTaskID.Value))
            {
                mainDirectory += "\\GanttTask_" + hdnTaskID.Value;
            }
            //else
            //{

            //    if (ViewState["TempUploadDirectory"] == null)
            //    {
            //        ViewState["TempUploadDirectory"] = Guid.NewGuid().ToString("N");
            //    }

            //    mainDirectory = ViewState["TempUploadDirectory"] as string;
            //}

            savepath = GetUploadDirectory(mainDirectory);


            //if (FileUpload1.HasFile)
            if (!string.IsNullOrEmpty(FileUpload1.FileName))
            {
                filename = FileUpload1.FileName;
                fullpath = savepath + filename;
                mime = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).Substring(1);

                if (File.Exists(fullpath))
                {
                    GeneralFunctions objGeneralFunctions = new GeneralFunctions();
                    filename = objGeneralFunctions.generateRandomString(4) + "_" + filename;
                    fullpath = savepath + filename;
                }

                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }

                FileUpload1.SaveAs(fullpath);
            }


            if (!string.IsNullOrEmpty(hdnTaskID.Value))
            {
                MapData objMapData = new MapData();
                BL_MapData objBL_MapData = new BL_MapData();

                objMapData.Screen = "GanttTask";
                objMapData.TicketID = Convert.ToInt32(hdnTaskID.Value);
                objMapData.TempId = "0";
                objMapData.FileName = filename;
                objMapData.DocTypeMIME = mime;
                objMapData.FilePath = fullpath;

                objMapData.DocID = 0;
                objMapData.Mode = 0;
                objMapData.ConnConfig = Session["config"].ToString();
                objBL_MapData.AddFile(objMapData);
                UpdateDocInfo();
                GetDocuments();

                
            }
            else
            {
                var tempTable = SaveAttachedFilesWhenAddingEstimate(filename, fullpath, mime);
                RadGrid_Documents.DataSource = tempTable;
                RadGrid_Documents.VirtualItemCount = tempTable.Rows.Count;
                RadGrid_Documents.DataBind();
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyUploadErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private string GetUploadDirectory(string mainDirectory)
    {
        var savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
        return savepathconfig + @"\" + Session["dbname"] + @"\" + mainDirectory + @"\";
    }

    private void UpdateDocInfo()
    {
        BL_User objBL_User = new BL_User();
        User _objUser = new User();
        _objUser.ConnConfig = Session["config"].ToString();
        _objUser.dtDocs = SaveDocInfo();
        objBL_User.UpdateDocInfo(_objUser);
    }

    private DataTable SaveDocInfo()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Portal", typeof(int));
        dt.Columns.Add("Remarks", typeof(string));
        dt.Columns.Add("MSVisible", typeof(byte));


        foreach (GridDataItem item in RadGrid_Documents.Items)
        {
            Label lblID = (Label)item.FindControl("lblID");
            //TextBox txtRemarks = (TextBox)item.FindControl("txtRemarks");
            //CheckBox chkPortal = (CheckBox)item.FindControl("chkPortal");
            DataRow dr = dt.NewRow();
            dr["ID"] = lblID.Text;
            dr["Portal"] = false;//chkPortal.Checked;
            dr["Remarks"] = "";//txtRemarks.Text;
            dr["MSVisible"] = false;
            dt.Rows.Add(dr);
        }

        return dt;
    }

    private void GetDocuments()
    {
        if (!string.IsNullOrEmpty(hdnTaskID.Value))
        {
            MapData objMapData = new MapData();
            BL_MapData objBL_MapData = new BL_MapData();
            objMapData.Screen = "GanttTask";
            objMapData.TicketID = Convert.ToInt32(hdnTaskID.Value);
            objMapData.TempId = "0";
            objMapData.Mode = 1;
            objMapData.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            ds = objBL_MapData.GetDocuments(objMapData);
            //gvDocuments.DataSource = ds.Tables[0];
            //gvDocuments.DataBind();
            RadGrid_Documents.DataSource = ds.Tables[0];
            RadGrid_Documents.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGrid_Documents.DataBind();
        }
        else
        {
            var source = ViewState["AttachedFiles"] as DataTable;
            //pnlDocumentButtons.Visible = true;
            RadGrid_Documents.DataSource = source;
            RadGrid_Documents.VirtualItemCount = source != null ? source.Rows.Count : 0;
            RadGrid_Documents.DataBind();
        }
    }

    private DataTable SaveAttachedFilesWhenAddingEstimate(string fileName, string fullPath, string doctype)
    {
        var tempAttachedFiles = ViewState["AttachedFiles"] as DataTable;

        if (tempAttachedFiles == null)
        {
            tempAttachedFiles = new DataTable();
            tempAttachedFiles.Columns.Add("id", typeof(int));
            tempAttachedFiles.Columns.Add("filename", typeof(string));
            tempAttachedFiles.Columns.Add("doctype", typeof(string));
            tempAttachedFiles.Columns.Add("Portal", typeof(bool));
            tempAttachedFiles.Columns.Add("Path", typeof(string));
            tempAttachedFiles.Columns.Add("remarks", typeof(string));
            tempAttachedFiles.Columns.Add("MSVisible", typeof(byte));
            tempAttachedFiles.Columns.Add("TempId", typeof(string));
            ViewState["AttachedFiles"] = tempAttachedFiles;
        }

        var row = tempAttachedFiles.NewRow();
        row["id"] = 0;
        row["filename"] = fileName;
        row["doctype"] = doctype;
        row["Path"] = fullPath;
        row["remarks"] = string.Empty;
        row["MSVisible"] = false;
        row["TempId"] = Guid.NewGuid().ToString("N");
        tempAttachedFiles.Rows.Add(row);
        return tempAttachedFiles;
    }

    protected void lblName_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;

        string[] CommandArgument = btn.CommandArgument.Split(',');

        string FileName = CommandArgument[0];

        string FilePath = CommandArgument[1];

        DownloadDocument(FilePath, FileName);
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
                string _EncodedData = HttpUtility.UrlEncode(DownloadFileName, System.Text.Encoding.UTF8) + lastUpdateTiemStamp;

                Response.Clear();
                Response.Buffer = false;
                Response.AddHeader("Accept-Ranges", "bytes");
                Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFileName));
                Response.AddHeader("Content-Length", (FileName.Length - startBytes).ToString());
                Response.AddHeader("Connection", "Keep-Alive");
                Response.ContentEncoding = System.Text.Encoding.UTF8;

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
                ////if blocks transfered not equals total number of blocks
                //if (i < maxCount)
                //    return false;
                //return true; 
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
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('File not found.');", true);
        }
        catch (UnauthorizedAccessException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('Please provide access permissions to the file path.');", true);
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileerrorWarning", "alert('" + str + "');", true);
        }

    }

    protected void lnkPostback_Click(object sender, EventArgs e)
    {
        GetDocuments();
        GetPOsForGanttTask();
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "keyOpenPopup", "showDialogById();", true);
    }

    private void GetPOsForGanttTask()
    {
        if (!string.IsNullOrEmpty(hdnTaskID.Value))
        {
            BusinessEntity.Planner planner = new BusinessEntity.Planner();
            BL_Planner bL_Planner = new BL_Planner();

            planner.ConnConfig = Session["config"].ToString();
            planner.ProjectID = Convert.ToInt32(Request.QueryString["projid"]);
            planner.idx = Convert.ToInt32(hdnTaskID.Value);

            DataSet ds = new DataSet();
            //ds = bL_Planner.GetProjectPOs(planner);
            ds = bL_Planner.GetPOsForGanttTask(planner);
            DataTable dataTable = ds.Tables[0];

            RadGrid_PO.DataSource = ds.Tables[0];
            RadGrid_PO.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGrid_PO.DataBind();
        }
        else
        {
            RadGrid_PO.DataSource = null;
            RadGrid_PO.VirtualItemCount = 0;
            RadGrid_PO.DataBind();
        }
    }

    //protected void lnkPOPostback_Click(object sender, EventArgs e)
    //{
    //    GetPOsForGanttTask();
    //    //RadGrid_PO.Rebind();
    //}
}

