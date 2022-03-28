using BusinessEntity;
using BusinessLayer;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Library : System.Web.UI.Page
{
    BusinessEntity.User objPropUser = new BusinessEntity.User();
    BL_User objBL_User = new BL_User();
    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {
            //BindGridDatatable(GetDocuments());        
        }
    }

    private DataTable GetDocuments()
    {
        if (Session["type"].ToString() == "c")
        {
            DataTable dtcust = new DataTable();
            dtcust = (DataTable)Session["userinfo"];
            int RoleID = 0;
            if (dtcust.Rows.Count > 0)
            {
                RoleID = Convert.ToInt32(dtcust.Rows[0]["roleid"]);
                objMapData.roleid = RoleID;
            }
        }
        objMapData.SearchBy = ddlSearch.SelectedValue;
        if (ddlSearch.SelectedValue == "loc")
        {
            objMapData.SearchValue = ddllocation.SelectedValue;
        }
        else
        {
            objMapData.SearchValue = txtSearch.Text.Trim().Replace("'","''");
        }

        if (txtStartDate.Text != string.Empty)
        {
            objMapData.StartDate = Convert.ToDateTime(txtStartDate.Text);
        }
        else
        {
            objMapData.StartDate = System.DateTime.MinValue;
        }

        if (txtEndDate.Text != string.Empty)
        {
            objMapData.EndDate = Convert.ToDateTime(txtEndDate.Text);
        }
        else
        {
            objMapData.EndDate = System.DateTime.MinValue;
        }

        objMapData.TicketID = Convert.ToInt32(Session["custid"]);
        objMapData.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_MapData.GetLibrary(objMapData);
        //lblRecordCount.Text = ds.Tables[0].Rows.Count.ToString()+ " File(s) found.";        
        return ds.Tables[0];
    }

    protected void lblName_Click(object sender, EventArgs e)
    {        
        ImageButton btn = (ImageButton)sender;

        string[] CommandArgument = btn.CommandArgument.Split(',');

        string FileName = CommandArgument[0];

        string FilePath = CommandArgument[1];
        //RadAjaxPanel_SharedDocument.ResponseScripts.Add(String.Format(@"window.location.href = ""{0}"";", FilePath));
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
                HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.                
                HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
                //HttpContext.Current.Response.End();
                //Response.End();
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
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileerrorWarning", "alert('" + str + "');", true);
        }
    }

    public string ImageThumb(string filename)
    {
        string extension = Path.GetExtension(filename);
        string image = "";
        if (extension.Equals(".jpg", StringComparison.CurrentCultureIgnoreCase) || extension.Equals(".jpeg", StringComparison.CurrentCultureIgnoreCase) || extension.Equals(".bmp", StringComparison.CurrentCultureIgnoreCase) || extension.Equals(".gif", StringComparison.CurrentCultureIgnoreCase) || extension.Equals(".png", StringComparison.CurrentCultureIgnoreCase))
        {
            string ThumbsDir = Path.GetDirectoryName(filename) + "\\thumbs\\";
            string Thumbs = ThumbsDir + Path.GetFileNameWithoutExtension(filename) + ".jpg";

            if (!File.Exists(Thumbs))
            {
                try
                {
                    using (System.Drawing.Image bigImage = new Bitmap(filename))
                    {
                        // Algorithm simplified for purpose of example.
                        int height = bigImage.Height / 10;
                        int width = bigImage.Width / 10;

                        // Now create a thumbnail
                        using (System.Drawing.Image smallImage = bigImage.GetThumbnailImage(width,
                                                                            height,
                                                                            new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero))
                        {

                            if (!Directory.Exists(ThumbsDir))
                                Directory.CreateDirectory(ThumbsDir);
                            smallImage.Save(Thumbs, ImageFormat.Jpeg);

                        }
                    }

                }
                catch { }
                }

            try { 
            using (var ms = new MemoryStream())
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(Thumbs);
                img.Save(ms, ImageFormat.Jpeg);
                string base64String = Convert.ToBase64String(ms.ToArray(), 0, ms.ToArray().Length);
                image = "data:image/png;base64," + base64String;
            }
            }
            catch { }
        }
        else if (extension.Equals(".pdf", StringComparison.CurrentCultureIgnoreCase))
            image = "images/thumb_pdfs.png";
        else if (extension.Equals(".doc", StringComparison.CurrentCultureIgnoreCase) || extension.Equals(".docx", StringComparison.CurrentCultureIgnoreCase))
            image = "images/thumb_docs.png";
        else if (extension.Equals(".txt", StringComparison.CurrentCultureIgnoreCase))
            image = "images/thumb_txt.png";
        else if (extension.Equals(".xls", StringComparison.CurrentCultureIgnoreCase) || extension.Equals(".xlsx", StringComparison.CurrentCultureIgnoreCase))
            image = "images/thumb_excel.png";
        else if (extension.Equals(".zip", StringComparison.CurrentCultureIgnoreCase) || extension.Equals(".rar", StringComparison.CurrentCultureIgnoreCase))
            image = "images/thumb_zip.png";
        else
            image = "images/thumb_file.png";
        return image;
    }
    public bool ThumbnailCallback()
    {
        return false;
    }
    protected void gvDocs_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING);
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING);
        }
    }

    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }

    private void SortGridView(string sortExpression, string direction)
    {
        DataTable dt = GetDocuments();

        DataView dv = new DataView(dt);
        dv.Sort = sortExpression + direction;

        BindGridDatatable(dv.ToTable());
    }
    private void BindGridDatatable(DataTable dt)
    {
        RadGrid_SharedDocument.VirtualItemCount = dt.Rows.Count;
        //gvDocs.DataSource = dt;
        //gvDocs.DataBind();
        RadGrid_SharedDocument.DataSource = dt;        
        //RadGrid_SharedDocument.DataBind();
    }
    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSearch.SelectedValue == "loc")
        {
            Locations();
            txtSearch.Visible = false;
            ddllocation.Visible = true;
        }
        else
        {
            txtSearch.Visible = true;
            ddllocation.Visible = false;
        }
    }
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        //BindGridDatatable(GetDocuments());
        RadGrid_SharedDocument.Rebind();
    }

    private void Locations()
    {
        if (Session["type"].ToString() == "c")
        {
            DataTable dtcust = new DataTable();
            dtcust = (DataTable)Session["userinfo"];
            int RoleID = 0;
            if (dtcust.Rows.Count > 0)
            {
                RoleID = Convert.ToInt32(dtcust.Rows[0]["roleid"]);
                objPropUser.RoleID = RoleID;
            }
        }

        DataSet ds = new DataSet();
        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.CustomerID = Convert.ToInt32(Session["custid"].ToString());
        if (Convert.ToInt32(Session["custid"].ToString()) != 0)
        {
            ds = objBL_User.getLocationByCustomerID(objPropUser);
        }
        else
        {
            ds = objBL_User.getLocations(objPropUser);
        }

        ddllocation.DataSource = ds.Tables[0];
        ddllocation.DataTextField = "tag";
        ddllocation.DataValueField = "loc";
        ddllocation.DataBind();
    }

    
    protected void RadGrid_SharedDocument_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        //RadGrid_SharedDocument.ShowStatusBar = true;
        BindGridDatatable(GetDocuments());
        uplblcount.Update();
    }

    protected void RadGrid_SharedDocument_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridPagerItem)
        {
            var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
            var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;


            GeneralFunctions obj = new GeneralFunctions();
            var sizes = obj.TelerikPageSize(totalCount);

            dropDown.Items.Clear();
            foreach (var size in sizes)
            {
                var cboItem = new RadComboBoxItem() { Text = size.Key, Value = size.Value };
                cboItem.Attributes.Add("ownerTableViewId", e.Item.OwnerTableView.ClientID);
                dropDown.Items.Add(cboItem);
            }
            dropDown.FindItemByValue(e.Item.OwnerTableView.PageSize.ToString()).Selected = true;

        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("portalhome.aspx");
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        ddlSearch.SelectedIndex = 0;
        ddllocation.Visible = false;
        txtSearch.Visible = true;
        txtSearch.Text = string.Empty;
        foreach (GridColumn column in RadGrid_SharedDocument.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        RadGrid_SharedDocument.MasterTableView.FilterExpression = string.Empty;
        RadGrid_SharedDocument.Rebind();
    }
    protected void RadGrid_SharedDocument_PreRender(object sender, EventArgs e)
    {
        //if (Convert.ToString(RadGrid_SharedDocument.MasterTableView.FilterExpression) != "")
        //{
        //    lblRecordCount.Text = RadGrid_SharedDocument.MasterTableView.Items.Count + " Record(s) found";
        //}
        //else
        //{
        //    lblRecordCount.Text = RadGrid_SharedDocument.VirtualItemCount + " Record(s) found";
        //}
    }

    protected void RadGrid_SharedDocument_ItemEvent(object sender, GridItemEventArgs e)
    {
        int rowCount = 0;
        if (e.EventInfo is GridInitializePagerItem)
        {
            rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
        }
        lblRecordCount.Text = rowCount + " Record(s) found";
        uplblcount.Update();
    }
}