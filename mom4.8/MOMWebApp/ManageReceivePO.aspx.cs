using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using Telerik.Web.UI;

public partial class ManageReceivePO : System.Web.UI.Page
{
    #region "Variables"
    public static bool isChecked = false;
    PO _objPO = new PO();
    BL_Bills _objBLBills = new BL_Bills();
    BL_User _objBLUser = new BL_User();
    GeneralFunctions objGeneralFunctions = new GeneralFunctions();
    private static readonly string CookieName = "Rad_ReceivePO";
    protected void Page_Init(object sender, EventArgs e)
    {
        //RadPersistenceReceivePO.StorageProviderKey = CookieName;
        //RadPersistenceReceivePO.StorageProvider = new CookieStorageProvider(CookieName);
    }
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
                string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

                if (Request.Url.Scheme == "http" && SSL == "1")
                {
                    string URL = Request.Url.ToString();
                    URL = URL.Replace("http://", "https://");
                    Response.Redirect(URL);
                }

                //#region Show Selected Filter
                //if (Session["from_ReceivePO"] != null && Session["end_ReceivePO"] != null)
                //{
                //    txtFromDate.Text = Convert.ToString(Session["from_ReceivePO"]);
                //    txtToDate.Text = Convert.ToString(Session["end_ReceivePO"]);
                //}
                //else
                //{
                //    txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                //    txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                //}
                //if (Session["ddlSearch_ReceivePO"] != null)
                //{
                //    String selectedValue = Convert.ToString(Session["ddlSearch_ReceivePO"]);
                //    ddlSearch.SelectedValue = selectedValue;

                //    String searchValue = Convert.ToString(Session["ddlSearch_Value_ReceivePO"]);
                //    if (selectedValue == "p.po" || selectedValue == "r.id" || selectedValue == "p.vendor")
                //    {
                //        txtSearch.Visible = false;
                //        txtReceivePoSearch.Visible = true;
                //        txtReceivePoSearch.Text = searchValue;
                //    }
                //    else if (selectedValue == "r.status")
                //    {
                //        ddlSearch_SelectedIndexChanged(sender, e);
                //        ddlStatus.SelectedValue = searchValue;
                //    }
                //    else
                //    {
                //        txtSearch.Visible = true;
                //        txtSearch.Text = searchValue;
                //        txtReceivePoSearch.Visible = false;
                //    }
                //}
                //if (Request.Cookies[CookieName] != null)
                //{

                //    RadPersistenceReceivePO.LoadState();
                //    RadGrid_ReceivePO.Rebind();
                //    updpnl.Update();
                //}
                //#endregion

                if (Session["from_ReceivePO"] != null && Session["end_ReceivePO"] != null)
                {
                    txtFromDate.Text = Convert.ToString(Session["from_ReceivePO"]);
                    txtToDate.Text = Convert.ToString(Session["end_ReceivePO"]);
                }
                else
                {
                    //DateTime _now = DateTime.Now;
                    //var _startDate = new DateTime(_now.Year, _now.Month, 1);
                    //var _endDate = _startDate.AddMonths(1).AddDays(-1);
                    //txtFromDate.Text = _startDate.ToShortDateString();
                    //txtToDate.Text = _endDate.ToShortDateString();

                    txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                    txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();

                    Session["from_ReceivePO"] = txtFromDate.Text;
                    Session["end_ReceivePO"] = txtToDate.Text;
                    lblWeek.Attributes.Add("class", "labelactive");
                    
                     
                }
                #region Show Selected Filter
                if (Convert.ToString(Request.QueryString["f"]) != "c")
                {
                    if (Session["from_ReceivePO"] != null && Session["end_ReceivePO"] != null)
                    {
                        txtFromDate.Text = Convert.ToString(Session["from_ReceivePO"]);
                        txtToDate.Text = Convert.ToString(Session["end_ReceivePO"]);
                    }
                    else
                    {
                        txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                        txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                        lblWeek.Attributes.Add("class", "labelactive");
                    }
                    if (Session["ddlSearch_ReceivePO"] != null)
                    {
                        String selectedValue = Convert.ToString(Session["ddlSearch_ReceivePO"]);
                        ddlSearch.SelectedValue = selectedValue;

                        String searchValue = Convert.ToString(Session["ddlSearch_Value_ReceivePO"]);
                        if (selectedValue == "p.po" || selectedValue == "r.id" || selectedValue == "p.vendor")
                        {
                            txtSearch.Visible = false;
                            txtReceivePoSearch.Visible = true;
                            txtReceivePoSearch.Text = searchValue;
                        }
                        else if (selectedValue == "r.status")
                        {
                            ddlSearch_SelectedIndexChanged(sender, e);
                            ddlStatus.SelectedValue = searchValue;
                        }
                        else
                        {
                            txtSearch.Visible = true;
                            txtSearch.Text = searchValue;
                            txtReceivePoSearch.Visible = false;
                        }
                    }
                    //if (Request.Cookies[CookieName] != null)
                    //{

                    //    //RadPersistenceReceivePO.LoadState();
                    //    RadGrid_ReceivePO.Rebind();
                    //    updpnl.Update();
                    //}
                }
                else
                {
                    Session["ddlSearch_ReceivePO"] = null;
                    Session["ddlSearch_Value_ReceivePO"] = null;
                    Session["from_ReceivePO"] = null;
                    Session["end_ReceivePO"] = null;
                  
                    DateTime _now = DateTime.Now;
                    //var _startDate = new DateTime(_now.Year, _now.Month, 1);
                    //var _endDate = _startDate.AddMonths(1).AddDays(-1);
                    //txtFromDate.Text = _startDate.ToShortDateString();
                    //txtToDate.Text = _endDate.ToShortDateString();

                    txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                    txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();

                    Session["from_ReceivePO"] = txtFromDate.Text;
                    Session["end_ReceivePO"] = txtToDate.Text;
                    lblWeek.Attributes.Add("class", "labelactive");

                }
                #endregion

                Permission();
                HighlightSideMenu("purchaseMgr", "lnkReceivePO", "purchaseMgrSub");

                if (Session["DelRPOErrMess"] != null)
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + Session["DelRPOErrMess"] + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                    Session["DelRPOErrMess"] = null;
                }

                if (Session["DelRPOSuccMess"] != null)
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "noty({text: '" + Session["DelRPOSuccMess"] + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    Session["DelRPOSuccMess"] = null;
                }
            }
            CompanyPermission();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region Custom function

    private void HighlightSideMenu(string MenuParent, string PageLink, string SubMenuDiv)
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl(MenuParent);
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        //HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl(PageLink);
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl(SubMenuDiv);
        div.Style.Add("display", "block");
    }
    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            RadGrid_ReceivePO.Columns[7].Visible = true;
        }
        else
        {
            RadGrid_ReceivePO.Columns[7].Visible = false;
        }
    }   
    private void BindReceivePO()
    {
        //try
        //{
            DataSet _dsPJ = new DataSet();
            _objPO.ConnConfig = Session["config"].ToString();
            _objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());
            _objPO.SearchBy = ddlSearch.SelectedValue;

            

            if (string.IsNullOrEmpty(txtFromDate.Text) && string.IsNullOrEmpty(txtToDate.Text))
            {
                //txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                //txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                txtFromDate.Text = Convert.ToDateTime("1900-01-01").ToShortDateString();
                txtToDate.Text = DateTime.Now.AddYears(100).ToShortDateString();
            }
            if (ddlSearch.SelectedValue == "r.status")
            {
                _objPO.SearchValue = ddlStatus.SelectedValue;
                ViewState["SearchVal"] = ddlStatus.SelectedValue;
            }
            else if (ddlSearch.SelectedValue == "ro.Name")
            {
                _objPO.SearchValue = txtSearch.Text;
                ViewState["SearchVal"] = txtSearch.Text;
            }
            else if (ddlSearch.SelectedValue == "p.inventorytype")
            {
                _objPO.SearchValue = "p.inventorytype";
                ViewState["SearchVal"] = "p.inventorytype";
            }
            else
            {
                _objPO.SearchValue = txtReceivePoSearch.Text;
                ViewState["SearchVal"] = txtReceivePoSearch.Text;
            }
            _objPO.ReceiveStartDate = txtFromDate.Text;
            _objPO.ReceiveEndDate = txtToDate.Text;

            if (Session["CmpChkDefault"].ToString() == "1")
            {
                _objPO.EN = 1;
            }
            else
            {
                _objPO.EN = 0;
            }
            _dsPJ = _objBLBills.GetListReceivePOBySearch(_objPO);

            // Status open/closed filter 
            DataTable filterdt = new DataTable();
            DataSet FilteredDs = new DataSet();
            if (isChecked)
            {
                lnkChk.Checked = true;
                FilteredDs = _dsPJ.Copy();
            }
            else
            {
                if (_dsPJ.Tables[0].Rows.Count > 0)
                {
                    DataRow[] dr = _dsPJ.Tables[0].Select("StatusName='Open'");
                    if (dr.Length > 0)
                    {
                        filterdt = dr.CopyToDataTable();
                        FilteredDs.Tables.Add(filterdt);
                    }
                    else
                    {
                        FilteredDs = _dsPJ.Clone();
                    }
                }
                else
                {
                    FilteredDs = _dsPJ.Copy();

                }
            }
            RadGrid_ReceivePO.VirtualItemCount = FilteredDs.Tables[0].Rows.Count;
            RadGrid_ReceivePO.DataSource = FilteredDs.Tables[0];
        
            //RadPersistenceReceivePO.SaveState();            
            Session["ReceivePOList"] = FilteredDs.Tables[0];
            txtFromDate.Text = Session["from_ReceivePO"].ToString();
            txtToDate.Text = Session["end_ReceivePO"].ToString();
            
        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
    }
    private void Permission()
    {
        
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderCstm");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("cstmMgrSub");
        //ul.Style.Add("display", "block");
        //ul.Style.Add("visibility", "visible");

        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
            //Response.Redirect("addcustomer.aspx?uid=" + Session["userid"].ToString());
        }

        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx");
            //pnlGridButtons.Visible = false;
        }

        // Removed this for ES-4346 BL - User joe unable to acees the modules he have access
        //if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        //{
        //    Response.Redirect("home.aspx");
        //}


        // User Permission 
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
          //  ds = (DataTable)Session["userinfo"];
            ds = GetUserById();

            /// PurchasingmodulePermission ///////////////////------->

            string PurchasingmodulePermission = ds.Rows[0]["PurchasingmodulePermission"] == DBNull.Value ? "Y" : ds.Rows[0]["PurchasingmodulePermission"].ToString();

            if (PurchasingmodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }

            /// RPOPermission ///////////////////------->

            string POPermission = ds.Rows[0]["RPO"] == DBNull.Value ? "YYYY" : ds.Rows[0]["RPO"].ToString();
            string ADD = POPermission.Length < 1 ? "Y" : POPermission.Substring(0, 1);
            string Edit = POPermission.Length < 2 ? "Y" : POPermission.Substring(1, 1);
            string Delete = POPermission.Length < 2 ? "Y" : POPermission.Substring(2, 1);
            string View = POPermission.Length < 4 ? "Y" : POPermission.Substring(3, 1);

            if (ADD == "N")
            {
                lnkAddnew.Visible = false;
                
            }

            if (Edit == "N")
            {
                lnkEdit.Visible = false;
            }

            if (Delete == "N")
            {
                lnkDelete.Visible = false;
            }

            if (View == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
        }
    }
    private DataTable GetUserById()
    {
        User objPropUser = new User();
        objPropUser.TypeID = Convert.ToInt32(Session["usertypeid"]);
        objPropUser.UserID = Convert.ToInt32(Session["userid"]);
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.DBName = Session["dbname"].ToString();
        DataSet ds = new DataSet();
        ds = _objBLUser.GetUserPermissionByUserID(objPropUser);
        return ds.Tables[0];
    }
    #endregion

    #region Events
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }
    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_ReceivePO.SelectedItems)
        {
            HiddenField hdnID = (HiddenField)di.FindControl("hdnID");
            Response.Redirect("addreceivepo.aspx?id=" + hdnID.Value);
        }
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        _objPO.ConnConfig = Session["config"].ToString();
        _objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());
        _objPO.MOMUSer = Session["User"].ToString();
        try
        {
            foreach (GridDataItem di in RadGrid_ReceivePO.SelectedItems)
            {
                HiddenField hdnID = (HiddenField)di.FindControl("hdnID");
                _objPO.RID = Convert.ToInt32(hdnID.Value);
                _objBLBills.DeleteReceivePO(_objPO);
               // _objBLBills.ReverseReceivePOInvetoryItem(_objPO.RID, Session["config"].ToString());
            }


            //InventoryWHTrans obj = new InventoryWHTrans();
            //obj.ConnConfig = Session["config"].ToString();
            //obj.InvID = Convert.ToInt32(hdnInvID.Value);
            //obj.WarehouseID = invWarehouseLoc.WarehouseID;
            //obj.LocationID = invWarehouseLoc.locationID;
            //obj.Hand = invWarehouseLoc.Hand;
            //obj.Balance = invWarehouseLoc.Balance;
            //obj.fOrder = 0;
            //obj.Committed = 0;
            //obj.Available = 0;
            //obj.Screen = "RPO";
            //obj.ScreenID = BatchID;
            //obj.Mode = "Add";
            //obj.TransType = "In";
            //_objBLBills.AddReceiveInventoryWHTrans(obj);


            if (_objPO.RID != 0)
                Session["DelRPOSuccMess"] = "Successfully deleted! </br> <b> RPO# : " + _objPO.RID + "</b>";
            else Session["DelRPOSuccMess"] = null;
            Session["DelRPOErrMess"] = null;
        }
        catch (Exception ex)
        {
            Session["DelRPOErrMess"] = ex.Message;
            Session["DelRPOSuccMess"] = null;
            if (ex.Message.ToString().Contains("This receive PO was closed!"))
            {
                Session["DelRPOErrMess"] = "The selected Received PO is closed and cannot be deleted.";
                
            }
        }
        

        Response.Redirect(Request.RawUrl);
    }

    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        Response.Redirect("addreceivepo.aspx");
    }
    protected void lnkChk_CheckedChanged(object sender, EventArgs e)
    {
        if (lnkChk.Checked)
        {
            isChecked = true;
        }
        else
        {
            isChecked = false;
        }
        BindReceivePO();
        RadGrid_ReceivePO.Rebind();
    }
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        #region Search Filter
        String selectedValue = ddlSearch.SelectedValue;
        Session["ddlSearch_ReceivePO"] = selectedValue;

        Session["from_ReceivePO"] = txtFromDate.Text;
        Session["end_ReceivePO"] = txtToDate.Text;

        if (selectedValue == "p.po" || selectedValue == "r.id" || selectedValue == "p.vendor")
        {
            Session["ddlSearch_Value_ReceivePO"] = txtReceivePoSearch.Text;
        }
        else if (selectedValue == "r.status")
        {
            Session["ddlSearch_Value_ReceivePO"] = ddlStatus.SelectedValue;
        }
        else
        {
            Session["ddlSearch_Value_ReceivePO"] = txtSearch.Text;
        }
        if (hdnCssActive.Value == "CssActive")
        {
            Session["lblReceivePOActive"] = "1";
        }
        else
        {
            Session["lblReceivePOActive"] = "2";
            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "CssClearLabel()", true);
        }
        #endregion
        //BindReceivePO();
        RadGrid_ReceivePO.Rebind();
    }
    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        String selectedValue = ddlSearch.SelectedValue;
        ShowHideFilterSearch(selectedValue);
    }
    private void ShowHideFilterSearch(String selectedValue)
    {
        if (selectedValue == "r.status")
        {
            txtSearch.Visible = false;
            txtReceivePoSearch.Visible = false;
            ddlStatus.Visible = true;
        }
        else if (selectedValue == "ro.Name")
        {
            txtSearch.Visible = true;
            txtReceivePoSearch.Visible = false;
            ddlStatus.Visible = false;
        }
        else
        {
            txtSearch.Visible = false;
            txtReceivePoSearch.Visible = true;
            ddlStatus.Visible = false;
        }
    }
    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        objGeneralFunctions.ResetFormControlValues(this);
        //txtFromDate.Text = Convert.ToDateTime("1900-01-01").ToShortDateString();
        //txtToDate.Text = DateTime.Now.ToShortDateString();        
        txtFromDate.Text = string.Empty;
        txtToDate.Text = string.Empty;
        Session["from_ReceivePO"] = txtFromDate.Text;
        Session["end_ReceivePO"] = txtToDate.Text;

        BindReceivePO();
        RadGrid_ReceivePO.Rebind();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "CssClearLabel()", true);
    }

    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_ReceivePO.MasterTableView.FilterExpression != "" ||
            (RadGrid_ReceivePO.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_ReceivePO.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_ReceivePO_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_ReceivePO.AllowCustomPaging = !ShouldApplySortFilterOrGroup();

        

        
        #region Set the Grid Filters
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["ReceivePO_FilterExpression"] != null && Convert.ToString(Session["ReceivePO_FilterExpression"]) != "" && Session["ReceivePO_Filters"] != null)
                {
                    RadGrid_ReceivePO.MasterTableView.FilterExpression = Convert.ToString(Session["ReceivePO_FilterExpression"]);
                    var filtersGet = Session["ReceivePO_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_ReceivePO.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
            }
            else
            {
                Session["ReceivePO_FilterExpression"] = null;
                Session["ReceivePO_Filters"] = null;
            }
        }
        else
        {
            #region Save the Grid Filter
            String filterExpression = Convert.ToString(RadGrid_ReceivePO.MasterTableView.FilterExpression);
            if (filterExpression == "")
            {
                filterExpression = Session["ReceivePO_FilterExpression"] != null ? Session["ReceivePO_FilterExpression"].ToString() : "";
            }

            if (filterExpression != "")
            {
                Session["ReceivePO_FilterExpression"] = filterExpression;
                List<RetainFilter> filters = new List<RetainFilter>();

                foreach (GridColumn column in RadGrid_ReceivePO.MasterTableView.OwnerGrid.Columns)
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

                Session["ReceivePO_Filters"] = filters;
            }
            else
            {
                Session["ReceivePO_FilterExpression"] = null;
                Session["ReceivePO_Filters"] = null;
            }
            #endregion
        }
        BindReceivePO();
        #endregion
    }
    protected void RadGrid_ReceivePO_ItemEvent(object sender, GridItemEventArgs e)
    {
        int rowCount = 0;
        if (e.EventInfo is GridInitializePagerItem)
        {
            rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
        }
        lblRecordCount.Text = rowCount + " Record(s) found";
        updpnl.Update();
    }
    protected void RadGrid_ReceivePO_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
                if (totalCount == 0) totalCount = 1000;
                GeneralFunctions obj = new GeneralFunctions();
                var sizes = obj.TelerikPageSize(totalCount);
                dropDown.Items.Clear();

                foreach (var size in sizes)
                {
                    var cboItem = new RadComboBoxItem() { Text = size.Key, Value = size.Value };
                    cboItem.Attributes.Add("ownerTableViewId", e.Item.OwnerTableView.ClientID);
                    if (e.Item.OwnerTableView.PageSize.ToString() == size.Value) cboItem.Selected = true;
                    dropDown.Items.Add(cboItem);
                }
            }
        }
        catch { }
    }
    private void RowSelect()
    {
        foreach (GridDataItem gr in RadGrid_ReceivePO.Items)
        {
            Label lblID = (Label)gr.FindControl("lblID");
            HyperLink lnkName = (HyperLink)gr.FindControl("lnkRef");
            lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='addreceivepo.aspx?id=" + lblID.Text + "'";
        }
    }
    protected void RadGrid_ReceivePO_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_ReceivePO.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["ReceivePO_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_ReceivePO.MasterTableView.OwnerGrid.Columns)
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
            Session["ReceivePO_Filters"] = filters;
        }
        else
        {
            Session["ReceivePO_FilterExpression"] = null;
            Session["ReceivePO_Filters"] = null;
        }

        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_ReceivePO);
        #endregion  

        RowSelect();
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        ResetFormControlValues(this);
        txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
        txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();

        Session["from_ReceivePO"] = txtFromDate.Text;
        Session["end_ReceivePO"] = txtToDate.Text;
        lblWeek.Attributes.Add("class", "labelactive");

        BindReceivePO();
        foreach (GridColumn column in RadGrid_ReceivePO.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        RadGrid_ReceivePO.MasterTableView.FilterExpression = string.Empty;
        RadGrid_ReceivePO.Rebind();
    }

    private void ResetFormControlValues(Control parent)
    {
        foreach (Control c in parent.Controls)
        {
            if (c.Controls.Count > 0)
            {
                ResetFormControlValues(c);
            }
            else
            {
                switch (c.GetType().ToString())
                {
                    case "System.Web.UI.WebControls.DropDownList":
                        ((DropDownList)c).SelectedIndex = -1;
                        break;
                    case "System.Web.UI.WebControls.TextBox":
                        ((TextBox)c).Text = "";
                        break;
                    case "System.Web.UI.WebControls.CheckBox":
                        ((CheckBox)c).Checked = false;
                        break;
                    case "System.Web.UI.WebControls.RadioButton":
                        ((RadioButton)c).Checked = false;
                        break;
                }
            }
        }
    }

    protected void lnkReceivePOReport_Click(object sender, EventArgs e)
    {
        String filterExpression = RadGrid_ReceivePO.MasterTableView.FilterExpression;

        if (!string.IsNullOrEmpty(filterExpression))
        {
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_ReceivePO.MasterTableView.OwnerGrid.Columns)
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

            Session["GridFilters"] = filters;
        }

        Response.Redirect("ReceivePOReport.aspx?S=" + txtFromDate.Text + "&E=" + txtToDate.Text +"&SBy=" + ddlSearch.SelectedValue + "&SVal=" + ViewState["SearchVal"]+ "&IsCheck=" + isChecked);
    }
}
#endregion
