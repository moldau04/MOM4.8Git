using System;
//using System.Collections;
//using System.Configuration;
using System.Data;
using System.Web;
//using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Xml.Linq;
using BusinessLayer;
using BusinessEntity;
using System.Collections.Generic;
using System.IO;
using System.Text;
//using AjaxControlToolkit;
//using System.Globalization;
using System.Drawing;
using Telerik.Web.UI;

public partial class addProjectDocument : System.Web.UI.Page
{
    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    JobT objJob = new JobT();
    BL_Job objBL_Job = new BL_Job();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["HideMaster"] == "true")
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<script language='javascript'>");
            sb.Append(@"var header = document.getElementById('header');");
            sb.Append(@"if(header) header.style.display='none';");
            sb.Append(@"var side = document.getElementById('ctl00_menu');");
            sb.Append(@"if(side) side.style.display='none';");
            sb.Append(@"var main = document.getElementById('main');");
            sb.Append(@"if(main) main.style.padding='0px';");
            sb.Append(@"</script>");
            ClientScript.RegisterStartupScript(this.GetType(), "HideMaster", sb.ToString());
            //lnkClose.Visible = false;
        }
        objJob.ConnConfig = Session["config"].ToString();
        //if (!IsPostBack)
        //{
        //    GetAttachment();
        //}
    }

    //private void Permission()
    //{

    //    //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
    //    //hm.Enabled = false;
    //    //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("SalesMgrSub");
    //    ////ul.Attributes.Remove("class");
    //    //ul.Style.Add("display", "block");

    //    if (Session["type"].ToString() != "am")
    //    {
    //        DataTable ds = new DataTable();
    //        ds = (DataTable)Session["userinfo"];

    //        //Document
    //        string DocumentPermission = ds.Rows[0]["DocumentPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["DocumentPermission"].ToString();
    //        hdnAddeDocument.Value = DocumentPermission.Length < 1 ? "Y" : DocumentPermission.Substring(0, 1);
    //        hdnEditeDocument.Value = DocumentPermission.Length < 2 ? "Y" : DocumentPermission.Substring(1, 1);
    //        hdnDeleteDocument.Value = DocumentPermission.Length < 3 ? "Y" : DocumentPermission.Substring(2, 1);
    //        hdnViewDocument.Value = DocumentPermission.Length < 4 ? "Y" : DocumentPermission.Substring(3, 1);

    //        //if (hdnAddeDocument.Value == "N")
    //        //{
    //        //    lnkUploadDoc.Enabled = false;
    //        //}

    //        //pnlDocPermission.Visible = hdnViewDocument.Value == "N" ? false : true;
    //    }

    //}

    //private void GetAttachment()
    //{

    //    List<ListItem> lsttypes = new List<ListItem>();
    //    lsttypes.Add(new ListItem("Project", "Project"));
    //    lsttypes.Add(new ListItem("Tickets", "Tickets"));
    //    lsttypes.Add(new ListItem("Customer", "Customer"));
    //    lsttypes.Add(new ListItem("Location", "Location"));

    //    rptattachmenttype.DataSource = lsttypes;
    //    rptattachmenttype.DataBind();

    //}

    //private DataTable GetAttachmentByTypes(string type)
    //{
    //    //List<ListItem> files = new List<ListItem>();
    //    DataTable dtAttach = new DataTable();
    //    dtAttach.Columns.Add("Text");
    //    dtAttach.Columns.Add("value");
    //    dtAttach.Columns.Add("ID");
    //    dtAttach.Columns.Add("content");
    //    dtAttach.Columns.Add("path");
    //    dtAttach.Columns.Add("msvisible");
    //    dtAttach.Columns.Add("thumb");
    //    dtAttach.Columns.Add("Screen");
    //    try
    //    {
    //        DataSet ds = new DataSet();
    //        objJob.Job = Convert.ToInt32(Request.QueryString["uid"] == null ? "0" : Request.QueryString["uid"].ToString());
    //        objJob.TypeName = type;
    //        //objJob.sort = Convert.ToInt16(ddlSortAttachment.SelectedValue);
    //        objJob.sort = 2;
    //        ds = objBL_Job.GetAttachment(objJob);

    //        if (ds.Tables.Count > 0)
    //        {
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //                {
    //                    DataRow dr = dtAttach.NewRow();

    //                    bool exists = false;
    //                    string filename = ds.Tables[0].Rows[i]["Filename"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["Filename"]) : string.Empty;
    //                    string localPath = ds.Tables[0].Rows[i]["Path"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["Path"]).Replace(filename, "") : string.Empty;
    //                    if (Directory.Exists(localPath))
    //                    {
    //                        if (File.Exists(localPath + filename))
    //                        {
    //                            Byte[] bytes = null;
    //                            string contenttype = MimeMapping.GetMimeMapping(filename);
    //                            string content = contenttype.Split('/')[0].ToString().ToLower();
    //                            if (content != "image")
    //                            {
    //                                System.Drawing.Icon iconForFile = System.Drawing.SystemIcons.WinLogo;
    //                                iconForFile = System.Drawing.Icon.ExtractAssociatedIcon(localPath + filename);
    //                                System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
    //                                bytes = (byte[])converter.ConvertTo(iconForFile.ToBitmap(), typeof(byte[]));
    //                                contenttype = "image/jpg";
    //                            }
    //                            else
    //                            {
    //                                //FileStream fs = new FileStream(localPath + filename, FileMode.Open, FileAccess.Read);
    //                                //BinaryReader br = new BinaryReader(fs);
    //                                //bytes = br.ReadBytes((Int32)fs.Length);
    //                                using (var ms = new MemoryStream())
    //                                {
    //                                    System.Drawing.Image image = System.Drawing.Image.FromFile(localPath + filename);
    //                                    Size thumbnailSize = GetThumbnailSize(image);
    //                                    Bitmap bitmap = CreateThumbnail(localPath + filename, thumbnailSize.Width, thumbnailSize.Height);
    //                                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
    //                                    bytes = ms.ToArray();
    //                                }
    //                            }

    //                            if (bytes == null)
    //                            {
    //                                continue;
    //                            }
    //                            string base64String = System.Convert.ToBase64String(bytes, 0, bytes.Length);


    //                            dr["Text"] = filename;
    //                            dr["ID"] = ds.Tables[0].Rows[i]["id"].ToString();
    //                            dr["content"] = content;
    //                            dr["path"] = localPath + filename;
    //                            dr["msvisible"] = Convert.ToBoolean(ds.Tables[0].Rows[i]["MSVisible"].ToString());
    //                            dr["value"] = "data:" + contenttype + ";base64," + base64String;
    //                            dr["thumb"] = "AttachmentImage.ashx?thumb=1&docid=" + ds.Tables[0].Rows[i]["id"].ToString();
    //                            dr["Screen"] = ds.Tables[0].Rows[i]["Screen"].ToString();
    //                            dtAttach.Rows.Add(dr);
    //                            exists = true;
    //                        }
    //                    }

    //                    if (!exists)
    //                    {
    //                        Byte[] bytes = null;
    //                        FileStream fs = new FileStream(Server.MapPath(@"~\images\NotFound.png"), FileMode.Open, FileAccess.Read);
    //                        BinaryReader br = new BinaryReader(fs);
    //                        bytes = br.ReadBytes((Int32)fs.Length);
    //                        string base64String;
    //                        base64String = System.Convert.ToBase64String(bytes, 0, bytes.Length);
    //                        //files.Add(new ListItem(localPath + filename, "data:" + string.Empty + ";base64," + base64String));
    //                        dr["Text"] = filename;
    //                        dr["ID"] = ds.Tables[0].Rows[i]["id"].ToString();
    //                        dr["content"] = "none";
    //                        dr["path"] = localPath + filename;
    //                        dr["msvisible"] = Convert.ToBoolean(ds.Tables[0].Rows[i]["MSVisible"].ToString());
    //                        dr["value"] = "data:image/png;base64," + base64String;
    //                        dtAttach.Rows.Add(dr);
    //                    }

    //                }
    //            }
    //        }

    //    }
    //    catch (FileNotFoundException ex)
    //    {
    //        //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
    //        //"FileaccessWarning", "alert('File not found.');", true);
    //    }
    //    catch (UnauthorizedAccessException ex)
    //    {
    //        //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
    //        //"FileaccessWarning", "alert('Please provide access permissions to the file path.');", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

    //        //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
    //        //"FileerrorWarning", "alert('" + str + "');", true);
    //    }
    //    return dtAttach;
    //}

    //private void BindAttachmentGrid()
    //{
    //    List<ListItem> lsttypes = new List<ListItem>();
    //    //if (ddlAttachment.SelectedValue == "All")
    //    //{
    //        lsttypes.Add(new ListItem("Project", "Project"));
    //        lsttypes.Add(new ListItem("Tickets", "Tickets"));
    //        lsttypes.Add(new ListItem("Customer", "Customer"));
    //        lsttypes.Add(new ListItem("Location", "Location"));

    //    //}
    //    //else
    //    //{
    //    //    lsttypes.Add(new ListItem(ddlAttachment.SelectedValue, ddlAttachment.SelectedValue));
    //    //}

    //    rptattachmenttype.DataSource = lsttypes;
    //    rptattachmenttype.DataBind();
    //}

    //protected void rptattachmenttype_ItemDataBound(object sender, RepeaterItemEventArgs e)
    //{
    //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
    //    {
    //        string hdntype = (e.Item.FindControl("hdntype") as HiddenField).Value;
    //        Repeater rptattachment = e.Item.FindControl("rptattachment") as Repeater;

    //        //HiddenField hdnpages = e.Item.FindControl("hdnpages") as HiddenField;
    //        //HiddenField hdnpagescount = e.Item.FindControl("hdnpagescount") as HiddenField;
    //        //LinkButton lnkprevious = e.Item.FindControl("lnkprevious") as LinkButton;
    //        //LinkButton lnknext = e.Item.FindControl("lnknext") as LinkButton;
    //        //lnknext.Enabled = true;
    //        //lnkprevious.Enabled = true;

    //        //rptattachment.DataSource = Paginate(GetAttachmentByTypes(hdntype), 1, hdnpages, hdnpagescount);
    //        rptattachment.DataSource = GetAttachmentByTypes(hdntype);
    //        rptattachment.DataBind();

    //        //if (Convert.ToInt32(hdnpages.Value) == Convert.ToInt32(hdnpagescount.Value) || Convert.ToInt32(hdnpagescount.Value) <= 0)
    //        //{

    //        //    lnknext.Enabled = false;
    //        //}
    //        //if (Convert.ToInt32(hdnpages.Value) <= 1)
    //        //    lnkprevious.Enabled = false;
    //    }
    //}

    //protected void rptattachment_ItemDataBound(object sender, RepeaterItemEventArgs e)
    //{
    //    LinkButton lnkDownload = e.Item.FindControl("lnkDownload") as LinkButton;
    //    ScriptManager.GetCurrent(this).RegisterPostBackControl(lnkDownload);


    //    ImageButton imgattachment = e.Item.FindControl("imgattachment") as ImageButton;
    //    ScriptManager.GetCurrent(this).RegisterPostBackControl(imgattachment);
    //}

    //protected void rptattachment_ItemCommand(object source, RepeaterCommandEventArgs e)
    //{
    //    if (e.CommandName == "OpenAttachment")
    //    {
    //        try
    //        {
    //            string filePath = e.CommandArgument.ToString();
    //            System.IO.FileInfo FileName = new System.IO.FileInfo(filePath);
    //            FileStream myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
    //            BinaryReader _BinaryReader = new BinaryReader(myFile);
    //            try
    //            {
    //                long startBytes = 0;
    //                string lastUpdateTiemStamp = File.GetLastWriteTimeUtc(filePath).ToString("r");
    //                string _EncodedData = HttpUtility.UrlEncode(myFile.Name, Encoding.UTF8) + lastUpdateTiemStamp;

    //                Response.Clear();
    //                Response.Buffer = false;
    //                Response.AddHeader("Accept-Ranges", "bytes");
    //                Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
    //                Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);
    //                Response.ContentType = "application/octet-stream";
    //                Response.AddHeader("Content-Disposition", "inline;filename=" + HttpUtility.UrlEncode(FileName.Name));
    //                Response.AddHeader("Content-Length", (FileName.Length - startBytes).ToString());
    //                Response.AddHeader("Connection", "Keep-Alive");
    //                Response.ContentEncoding = Encoding.UTF8;

    //                //Send data
    //                _BinaryReader.BaseStream.Seek(startBytes, SeekOrigin.Begin);

    //                //Dividing the data in 1024 bytes package
    //                int maxCount = (int)Math.Ceiling((FileName.Length - startBytes + 0.0) / 1024);

    //                //Download in block of 1024 bytes
    //                int i;
    //                for (i = 0; i < maxCount && Response.IsClientConnected; i++)
    //                {
    //                    Response.BinaryWrite(_BinaryReader.ReadBytes(1024));
    //                    Response.Flush();
    //                }
    //                ////if blocks transfered not equals total number of blocks
    //                //if (i < maxCount)
    //                //    return false;
    //                //return true; 
    //            }
    //            catch (Exception ex)
    //            {
    //                throw ex;
    //            }
    //            finally
    //            {
    //                Response.End();
    //                _BinaryReader.Close();
    //                myFile.Close();
    //            }
    //        }
    //        catch (FileNotFoundException ex)
    //        {
    //            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
    //            "FileaccessWarning", "alert('File not found.');", true);
    //        }
    //        catch (UnauthorizedAccessException ex)
    //        {
    //            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
    //            "FileaccessWarning", "alert('Please provide access permissions to the file path.');", true);
    //        }
    //        catch (Exception ex)
    //        {
    //            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

    //            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
    //            "FileerrorWarning", "alert('" + str + "');", true);
    //        }
    //    }
    //    else if (e.CommandName == "UpdateMS")
    //    {
    //        int docID = Convert.ToInt32(e.CommandArgument.ToString());
    //        objPropUser.ConnConfig = Session["config"].ToString();
    //        objPropUser.DocID = docID;
    //        objBL_User.UpdateDoc(objPropUser);
    //        CheckBox chkkMs = (CheckBox)e.Item.FindControl("chkMS");
    //    }
    //    else if (e.CommandName == "DeleteAttachment")
    //    {

    //        int docID = Convert.ToInt32(e.CommandArgument.ToString());
    //        objMapData.ConnConfig = Session["config"].ToString();
    //        objMapData.DocumentID = docID;
    //        objBL_MapData.DeleteFile(objMapData);
    //        BindAttachmentGrid();
    //    }
    //    else if (e.CommandName == "RotatedImgright")
    //    {
    //        // get the full path of image url
    //        string path = e.CommandArgument.ToString();//Server.MapPath(Image1.ImageUrl);

    //        // creating image from the image url
    //        System.Drawing.Image i = System.Drawing.Image.FromFile(path);

    //        // rotate Image 90' Degree
    //        i.RotateFlip(RotateFlipType.Rotate270FlipXY);

    //        // save it to its actual path
    //        i.Save(path);

    //        // release Image File
    //        i.Dispose();

    //        GetAttachment();

    //    }
    //    else if (e.CommandName == "RotatedImgleft")
    //    {
    //        // get the full path of image url
    //        string path = e.CommandArgument.ToString();//Server.MapPath(Image1.ImageUrl);

    //        // creating image from the image url
    //        System.Drawing.Image i = System.Drawing.Image.FromFile(path);

    //        // rotate Image 90' Degree
    //        i.RotateFlip(RotateFlipType.Rotate270FlipNone);

    //        // save it to its actual path
    //        i.Save(path);

    //        // release Image File
    //        i.Dispose();

    //        GetAttachment();
    //    }
    //}

    protected void lnkUploadProjectDoc_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["uid"] != null)
            {
                string filename = string.Empty;
                string fullpath = string.Empty;
                string MIME = string.Empty;
                //if (FU_Project.HasFile)
                foreach (HttpPostedFile postedFile in FU_Project.PostedFiles)
                {
                    string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                    string savepath = savepathconfig + @"\" + Session["dbname"] + @"\ld_" + Request.QueryString["uid"].ToString() + @"\";
                    //filename = FU_Project.FileName;
                    filename = postedFile.FileName;

                    fullpath = savepath + filename;
                    MIME = System.IO.Path.GetExtension(postedFile.FileName).Substring(1);

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

                    FU_Project.SaveAs(fullpath);

                    objMapData.Screen = "Project";
                    objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                    objMapData.TempId = "0";
                    objMapData.FileName = filename;
                    objMapData.DocTypeMIME = MIME;
                    objMapData.FilePath = fullpath;
                    objMapData.DocID = 0;
                    objMapData.Mode = 0;
                    objMapData.ConnConfig = Session["config"].ToString();
                    objBL_MapData.AddFile(objMapData);
                }
                //BindAttachmentGrid();
                RadGrid_Documents.Rebind();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private Size GetThumbnailSize(System.Drawing.Image original)
    {
        // Maximum size of any dimension.
        const int maxPixels = 800;

        // Width and height.
        int originalWidth = original.Width;
        int originalHeight = original.Height;

        // Compute best factor to scale entire image based on larger dimension.
        double factor;
        if (originalWidth > originalHeight)
        {
            factor = (double)maxPixels / originalWidth;
        }
        else
        {
            factor = (double)maxPixels / originalHeight;
        }

        // Return thumbnail size.
        return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
    }

    private Bitmap CreateThumbnail(string lcFilename, int lnWidth, int lnHeight)
    {

        System.Drawing.Bitmap bmpOut = null;
        try
        {
            Bitmap loBMP = new Bitmap(lcFilename);
            System.Drawing.Imaging.ImageFormat loFormat = loBMP.RawFormat;

            decimal lnRatio;
            int lnNewWidth = 0;
            int lnNewHeight = 0;

            //*** If the image is smaller than a thumbnail just return it
            if (loBMP.Width < lnWidth && loBMP.Height < lnHeight)
                return loBMP;


            if (loBMP.Width > loBMP.Height)
            {
                lnRatio = (decimal)lnWidth / loBMP.Width;
                lnNewWidth = lnWidth;
                decimal lnTemp = loBMP.Height * lnRatio;
                lnNewHeight = (int)lnTemp;
            }
            else
            {
                lnRatio = (decimal)lnHeight / loBMP.Height;
                lnNewHeight = lnHeight;
                decimal lnTemp = loBMP.Width * lnRatio;
                lnNewWidth = (int)lnTemp;
            }

            bmpOut = new Bitmap(lnNewWidth, lnNewHeight);
            Graphics g = Graphics.FromImage(bmpOut);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);
            g.DrawImage(loBMP, 0, 0, lnNewWidth, lnNewHeight);

            loBMP.Dispose();
        }
        catch
        {
            return null;
        }

        return bmpOut;
    }

    //protected void ddlSortAttachment_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    BindAttachmentGrid();
    //}

    //protected void ddlAttachment_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    BindAttachmentGrid();
    //    if (ddlAttachment.SelectedValue == "All" || ddlAttachment.SelectedValue == "Project")
    //    {
    //        fuspan.Visible = FU_Project.Visible = true;
    //    }
    //    else
    //    {
    //        fuspan.Visible = FU_Project.Visible = false;
    //    }
    //}

    protected void RadGrid_Documents_PreRender(object sender, EventArgs e)
    {
        RowSelect();
    }

    protected void RadGrid_Documents_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        GetDocuments(chkShowAllDocs.Checked);
    }

    private void GetDocuments(bool isShowAll)
    {
        if (Request.QueryString["uid"] != null)
        {
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objMapData.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            ds = objBL_MapData.GetProjectDocuments(objMapData, isShowAll);
            //gvDocuments.DataSource = ds.Tables[0];
            //gvDocuments.DataBind();
            RadGrid_Documents.DataSource = ds.Tables[0];
            RadGrid_Documents.VirtualItemCount = ds.Tables[0].Rows.Count;
            //RadGrid_Documents.DataBind();
        }
    }

    //private void RowSelectDocuments()
    //{
    //    //if (hdnEditDocument.Value == "N")
    //    if (true)
    //    {
    //        foreach (GridDataItem item in RadGrid_Documents.Items)
    //        {
    //            //CheckBox chkSelected = (CheckBox)item.FindControl("chkSelect");
    //            //CheckBox chkPortal = (CheckBox)item.FindControl("chkPortal");
    //            TextBox txtremarks = (TextBox)item.FindControl("txtremarks");
    //            //chkSelected.Enabled = 
    //            //chkPortal.Enabled = false;
    //            txtremarks.Enabled = false;
    //            item.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";
    //        }
    //    }
    //}

    protected void lblName_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;

        string[] CommandArgument = btn.CommandArgument.Replace(btn.Text," ").Split(',');

        string FileName = btn.Text;
        string FilePath = CommandArgument[1].Trim()+btn.Text.Trim();


        DownloadDocument(FilePath, FileName);
    }

    private void DownloadDocument(string filePath, string DownloadFileName)
    {
        try
        {
            using (new NetworkConnection())
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
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileerrorWarning", "alert('" + str + "');", true);
        }
    }

    protected void RadGrid_Documents_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "UpdateMS")
        {
            int docID = Convert.ToInt32(e.CommandArgument.ToString());
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.DocID = docID;
            objBL_User.UpdateDoc(objPropUser);
            CheckBox chkkMs = (CheckBox)e.Item.FindControl("chkMS");
        }
    }

    private void RowSelect()
    {
        foreach (GridDataItem gr in RadGrid_Documents.Items)
        {

            Label lblScreen = (Label)gr.FindControl("lblScreen");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelectSelectCheckBox");
            if (lblScreen.Text.ToUpper() == "PROJECT")
            {
                chkSelect.Visible = true;
            }
            else
            {
                chkSelect.Visible = false;
            }

        }
    }

    protected void lnkDeleteDoc_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Documents.SelectedItems)
        {
            Label lblID = (Label)item.FindControl("lblId");
            CheckBox chkSelect = (CheckBox)item.FindControl("chkSelectSelectCheckBox");
            //HiddenField hdnTempId = (HiddenField)item.FindControl("hdnTempId");
            //if (lblID.Text == "0")
            //{
            //    DeleteDocFromTempTable(hdnTempId.Value);
            //}

            if (chkSelect.Checked)
            {
                DeleteFileFromFolder(string.Empty, Convert.ToInt32(lblID.Text));
            }
        }
        RadGrid_Documents.Rebind();
        //ScriptManager.RegisterStartupScript(this, GetType(), "DeleteDoc", "$('.dropify').dropify();", true);
    }

    public void DeleteFileFromFolder(string StrFilename, int DocumentID)
    {
        try
        {
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
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            ScriptManager.RegisterStartupScript(this, GetType(),
            "FileDeleteErrorWarning", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void DeleteFile(int DocumentID)
    {
        try
        {
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.DocumentID = DocumentID;
            objMapData.Worker = Session["User"].ToString();
            objBL_MapData.DeleteFile(objMapData);
            //UpdateDocInfo();
            //GetDocuments();
            //RadGrid_Documents.Rebind();
            //adGrid_gvLogs.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrdelete", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void chkShowAllDocs_CheckedChanged(object sender, EventArgs e)
    {
        RadGrid_Documents.Rebind();
    }

    //private void UpdateDocInfo()
    //{
    //    objPropUser.ConnConfig = Session["config"].ToString();
    //    objPropUser.dtDocs = SaveDocInfo();
    //    objPropUser.Username = Session["User"].ToString();
    //    objBL_User.UpdateDocInfo(objPropUser);
    //}
}