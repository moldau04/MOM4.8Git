using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Artem.Web.UI.Controls;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using BusinessEntity;
using BusinessLayer;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web;

public partial class GoogleMap : Page
{
    #region Property

    BL_User objBL_User = new BL_User();
    User objProp_User = new User();

    BL_MapData objBL_MapData = new BL_MapData();
    MapData objpropMapData = new MapData();

    General objgeneral = new General();
    BL_General objBL_General = new BL_General();

    GeneralFunctions genFunction = new GeneralFunctions();

    string OpenAddress;
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    double toten = 0;
    double totco = 0;
    double tottot = 0;
    public Double Latitude = Convert.ToDouble("39.0917394");
    public Double Longitude = Convert.ToDouble("-94.5828553");
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        Permission();
        if (!IsPostBack)
        {
            hdnwebconfig.Value = SSTCryptographer.Encrypt(Session["config"].ToString(), "webconfig");

            GetDefaultLat_Lng();
            txtDate.SelectedDate = DateTime.Now;
            FillFieldWorkers();
            FillCategory();
            FillOpenCallsGrid();
            string tech = ddlTech.SelectedItem.Text;
            if (tech != "select")
                FillTimeStGrid();

        }
        CompanyPermission();
    }

    private void GetDefaultLat_Lng()
    {
        DataSet dsLat_Lng = new DataSet();
        dsLat_Lng = SqlHelper.ExecuteDataset(Session["config"].ToString(), CommandType.Text, "select isnull(Lat,0) as Lat , isnull(Lng,0) as Lng from control");
        if (dsLat_Lng.Tables[0].Rows.Count > 0)
        {
            double.TryParse(dsLat_Lng.Tables[0].Rows[0]["Lat"].ToString(), out Latitude);
            double.TryParse(dsLat_Lng.Tables[0].Rows[0]["Lng"].ToString(), out Longitude);
        }
    }


    protected void Page_PreRender(Object o, EventArgs e)
    {

        foreach (GridDataItem gr in RadgvTimeStmp.Items)
        {


            Label TicketID = (Label)gr.FindControl("lblId");
            Label lblEdate = (Label)gr.FindControl("lblEdate");
            Label lblTimert = (Label)gr.FindControl("lblTimert");
            Label lblTimesite = (Label)gr.FindControl("lblTimesite");
            Label lblTimecomp = (Label)gr.FindControl("lblTimecomp");

            Label lblAddressER = (Label)gr.FindControl("lblAddressOR");
            Label lblAddressOS = (Label)gr.FindControl("lblAddressOS");
            Label lblAddressCT = (Label)gr.FindControl("lblAddressCM");

            string st = txtDate.SelectedDate.ToString();

            st = Convert.ToDateTime(st).ToString("MM/dd/yyyy");

            lblTimert.Attributes["onmouseover"] = "LocateAddress('" + ddlTech.SelectedItem.Text + "', '" + st + " " + lblEdate.Text + "', '" + st + " " + lblTimert.Text + "', '" + lblAddressER.ClientID + "', '" + TicketID.Text + "', '3');";

            lblTimesite.Attributes["onmouseover"] = "LocateAddress('" + ddlTech.SelectedItem.Text + "', '" + st + " " + lblEdate.Text + "', '" + st + " " + lblTimesite.Text + "', '" + lblAddressOS.ClientID + "', '" + TicketID.Text + "', '2');";

            lblTimecomp.Attributes["onmouseover"] = "LocateAddress('" + ddlTech.SelectedItem.Text + "', '" + st + " " + lblEdate.Text + "', '" + st + " " + lblTimecomp.Text + "', '" + lblAddressCT.ClientID + "', '" + TicketID.Text + "', '1');";


            Label lbldistENOS = (Label)gr.FindControl("lbldistENOS");
            Label lbldistCOER = (Label)gr.FindControl("lbldistCOER");
            Label lbldisttot = (Label)gr.FindControl("lbldisttot");

            if (lbldistENOS.Text == "") { lbldistENOS.Text = "0"; }
            if (lbldistCOER.Text == "") { lbldistCOER.Text = "0"; }
            if (lbldisttot.Text == "") { lbldisttot.Text = "0"; }

            lbldisttot.Text = (Convert.ToDouble(lbldistCOER.Text) + Convert.ToDouble(lbldistENOS.Text)).ToString();

            toten = toten + Convert.ToDouble(lbldistENOS.Text);
            totco = totco + Convert.ToDouble(lbldistCOER.Text);
            tottot = tottot + Convert.ToDouble(lbldisttot.Text);

            GridFooterItem footeritem = (GridFooterItem)RadgvTimeStmp.MasterTableView.GetItems(GridItemType.Footer)[0];

            Label lbldistENOSfooter = (Label)footeritem.FindControl("lbldistENOSfooter");
            Label lbldistCOERfooter = (Label)footeritem.FindControl("lbldistCOERfooter");
            Label lbldisttotfooter = (Label)footeritem.FindControl("lbldisttotfooter");

            lbldistENOSfooter.Text = toten.ToString();
            lbldistCOERfooter.Text = totco.ToString();
            lbldisttotfooter.Text = tottot.ToString();
        }


        foreach (GridDataItem gr in RadGridOpenCalls.Items)
        {
            RadCheckBox chkSelect = (RadCheckBox)gr.FindControl("chkSelect");
            Label lblTicketId = (Label)gr.FindControl("lblTicketId");
            Label lblStatus = (Label)gr.FindControl("lblStatus");

            gr.Attributes["onclick"] = "SelectRowChk('" + gr.ClientID + "','" + chkSelect.ClientID + "','" + RadGridOpenCalls.ClientID + "',event);";
            if (string.Equals(lblStatus.Text, "un-assigned", StringComparison.InvariantCultureIgnoreCase))
            {
                lblTicketId.BackColor = System.Drawing.Color.Wheat;
            }

        }


        HighlightSideMenu("schMgr", "lnkMapMenu", "schdMgrSub");

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

    private void Permission()
    {


        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("Home.aspx?permission=no");
        }
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = GetUserById();
            string MapRtPermission = ds.Rows[0]["MapR"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["MapR"].ToString();
            string view = MapRtPermission.Length < 4 ? "Y" : MapRtPermission.Substring(3, 1);
            if (view == "N")
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
        ds = objBL_User.GetUserPermissionByUserID(objPropUser);

        return ds.Tables[0];
    }
    private void userpermissions()
    {
        if (Session["type"].ToString() != "c")
        {
            if (Session["type"].ToString() != "am")
            {
                objProp_User.ConnConfig = Session["config"].ToString();
                objProp_User.Username = Session["username"].ToString();
                objProp_User.PageName = "GoogleMap.aspx";
                DataSet dspage = objBL_User.getScreensByUser(objProp_User);
                if (dspage.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dspage.Tables[0].Rows[0]["access"].ToString()) == false)
                    {
                        Response.Redirect("home.aspx");
                    }
                }
                else
                {
                    Response.Redirect("home.aspx");
                }
            }
        }
    }

    private void CompanyPermission()
    {

    }


    #region Field Workers

    private void FillFieldWorkers()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();


        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            objProp_User.Supervisor = Session["username"].ToString().ToUpper();
        }
        ds = objBL_User.getEMPwithDeviceID(objProp_User);
        ddlTech.DataSource = ds.Tables[0];
        ddlTech.DataTextField = "fDesc";
        ddlTech.DataValueField = "id";
        ddlTech.DataBind();

        ddlTech.Items.Insert(0, new ListItem("Select", ""));
    }

    #endregion

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FillOpenCallsGrid();
        RadGridOpenCalls.Rebind();
        FillTimeStGrid();
        RadgvTimeStmp.Rebind();

        ScriptManager.RegisterStartupScript(Page, GetType(), "JavaFunction", "chkStateLiveData();", true);


    }




    protected void lnkReset_Click(object sender, EventArgs e)
    {

        txtDate.SelectedDate = DateTime.Now;
        ddlTech.SelectedIndex = -1;

        chkAssignedCall.Checked = false;
        chkOpencalls.Checked = false;
        chkEffective.Checked = false;
        ddlCategory.Items.Clear();
        FillCategory();
        FillOpenCallsGrid();
        FillTimeStGrid();
        RadGridOpenCalls.Rebind();
        RadgvTimeStmp.Rebind();
        chkLiveData.Checked = false;
    }

    public double Distance(string Lat1, string Lon1, string Lat2, string Lon2)
    {

        double Latitude1 = Convert.ToDouble(Lat1);
        double Longitude1 = Convert.ToDouble(Lon1);
        double Latitude2 = Convert.ToDouble(Lat2);
        double Longitude2 = Convert.ToDouble(Lon2);

        double R = 6371;
        double dLat = this.toRadian(Latitude2 - Latitude1);
        double dLon = this.toRadian(Longitude2 - Longitude1);

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(this.toRadian(Latitude1)) * Math.Cos(this.toRadian(Latitude2)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
        double d = R * c;

        return d;
    }

    private double toRadian(double val)
    {
        return (Math.PI / 180) * val;
    }



    #region MAP Feature



    #endregion

    #region OpenCalls OF Field Worker

    private void FillOpenCallsGrid()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.FieldEmp = ddlTech.SelectedItem.Text == "Select" ? string.Empty : ddlTech.SelectedItem.Text;
        objProp_User.Edate = Convert.ToDateTime(txtDate.SelectedDate);
        //objProp_User.CategoryName = ddlCategory.SelectedItem.Text == "All" ? string.Empty : ddlCategory.SelectedItem.Text; 
        objProp_User.CategoryName = GetSelectedCategory();
        #region Company Check
        objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            objProp_User.EN = 1;
        }
        else
        {
            objProp_User.EN = 0;
        }
        #endregion
        ds = objBL_User.getOpenCallsMapScreen(objProp_User);
        Session["opencalldata"] = ds.Tables[0];
        if (ds.Tables.Count > 0)
        {
            RadGridOpenCalls.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGridOpenCalls.DataSource = ds.Tables[0];
        }
        else
        {
            RadGridOpenCalls.VirtualItemCount = 0;
            RadGridOpenCalls.DataSource = null;
        }
    }

    #region Paging OpenCallsGrid
    protected void gvOpenCalls_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }

    private DataTable PageSortData()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["opencalldata"];
        return dt;
    }

    protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
    {

    }

    protected void Paginate(object sender, CommandEventArgs e)
    {

    }

    private void FillGridPaged()
    {

    }

    protected void gvOpenCalls_DataBound(object sender, EventArgs e)
    {
        //GridViewRow gvrPager = RadGridOpenCalls.BottomPagerRow;

        //if (gvrPager == null) return;

        //// get your controls from the gridview
        //DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        //Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        //if (ddlPages != null)
        //{
        //    // populate pager
        //    for (int i = 0; i < gvOpenCalls.PageCount; i++)
        //    {

        //        int intPageNumber = i + 1;
        //        ListItem lstItem = new ListItem(intPageNumber.ToString());

        //        if (i == gvOpenCalls.PageIndex)
        //            lstItem.Selected = true;

        //        ddlPages.Items.Add(lstItem);
        //    }
        //}

        //// populate page count
        //if (lblPageCount != null)
        //    lblPageCount.Text = gvOpenCalls.PageCount.ToString();
    }

    protected void gvOpenCalls_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //Paginate(sender, e);
    }

    protected void gvOpenCalls_Sorting(object sender, GridViewSortEventArgs e)
    {

    }

    private void SortGridView(string sortExpression, string direction)
    {

    }


    #endregion



    protected void btnchkOpencalls_Click(object sender, EventArgs e)
    {
        if (chkOpencalls.Checked == true)
        {

            RadGridOpenCalls.Rebind();

        }
    }

    #endregion

    #region  TimeStmpGrid OF Field Worker

    private void FillTimeStGrid()
    {
        DateTime Date = Convert.ToDateTime(txtDate.SelectedDate);
        string tech = ddlTech.SelectedItem.Text;
        string connstr = Session["config"].ToString();

        DataSet ds = new DataSet();
        objpropMapData.ConnConfig = connstr;
        objpropMapData.Tech = tech;
        objpropMapData.Date = Date;
        ds = objBL_MapData.GetTimestmpLocationList(objpropMapData);

        ds.Tables[0].Columns.Add("AddressER");
        ds.Tables[0].Columns.Add("AddressOS");
        ds.Tables[0].Columns.Add("AddressCM");


        Session["timedata"] = ds.Tables[0];
        RadgvTimeStmp.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadgvTimeStmp.DataSource = ds.Tables[0];
        RadgvTimeStmp.ShowFooter = true;
        if (ds.Tables[0].Rows.Count == 0)
        {
            RadgvTimeStmp.ShowFooter = false;
        }
    }

    protected void gvTimeStmp_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    #region Paging TimeStmpGrid

    protected void gvTimeStmp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }

    private DataTable PageSortDataTime()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["timedata"];
        return dt;
    }

    private void FillGridPagedTime()
    {


    }

    protected void ddlPagesTime_SelectedIndexChanged(Object sender, EventArgs e)
    {

    }

    protected void PaginateTime(object sender, CommandEventArgs e)
    {

    }

    protected void gvTimeStmp_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        PaginateTime(sender, e);
    }

    protected void gvTimeStmp_DataBound(object sender, EventArgs e)
    {

    }

    protected void gvTimeStmp_Sorting(object sender, GridViewSortEventArgs e)
    {

    }

    private void SortGridViewTime(string sortExpression, string direction)
    {

    }

    #endregion

    #endregion




    #region Find Nearest Tech

    protected void btnNearest_Click(object sender, EventArgs e)
    {
        DataSet ds1 = new DataSet();

        DataTable dt2 = new DataTable("");
        DataColumn dc2 = dt2.Columns.Add("dist", typeof(String));
        dt2.Columns.Add("Time", typeof(String));
        dt2.Columns.Add("dwork", typeof(String));
        dt2.Columns.Add("lat", typeof(String));
        dt2.Columns.Add("lng", typeof(String));
        dt2.Columns.Add("address", typeof(String));
        dt2.Columns.Add("GPS", typeof(String));
        dt2.Columns.Add("cat", typeof(String));
        dt2.Columns.Add("assignname", typeof(String));
        ds1.Tables.Add(dt2);


        foreach (GridDataItem di in RadGridOpenCalls.Items)
        {
            RadCheckBox chkSelected = (RadCheckBox)di.FindControl("chkSelect");

            Label lblAddress = (Label)di.FindControl("lbladdress");

            Label lblLat = (Label)di.FindControl("lblLat");

            Label lblLng = (Label)di.FindControl("lblLng");

            Label lblTicketId = (Label)di.FindControl("lblTicketId");

            Label lblStatus = (Label)di.FindControl("lblStatus");

            DataTable table1 = new DataTable();

            if (chkSelected.Checked == true)
            {
                table1 = GetNearestWorker(lblAddress.Text, lblLat.Text.Trim(), lblLng.Text.Trim());

                if (table1.Rows.Count > 0)
                {

                    DataTable table2 = table1.Clone();
                    DataRow row;
                    row = table2.NewRow();
                    row["dist"] = "0";
                    row["Time"] = "";
                    row["dwork"] = lblTicketId.Text.Trim();
                    row["lat"] = lblLat.Text.Trim();
                    row["lng"] = lblLng.Text.Trim();
                    row["address"] = lblAddress.Text.Trim();
                    row["GPS"] = "0";
                    row["cat"] = "1";
                    row["assignname"] = lblStatus.Text.Trim();



                    table2.Rows.Add(row);
                    table1.Merge(table2);
                    ds1.Tables[0].Merge(table1);
                }
            }


        }



        string str = "";

        JavaScriptSerializer sr = new JavaScriptSerializer();

        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        GeneralFunctions objGeneral = new GeneralFunctions();

        dictListEval = objGeneral.RowsToDictionary(ds1.Tables[0]);

        str = sr.Serialize(dictListEval);

        hdnNearest.Value = str;

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "dsfsdf", "drawPathNearestWorker();", true);
    }



    private DataTable GetNearestWorker(string add, string lat, string lng)
    {
        DataSet ds = new DataSet();
        try
        {


            if (lat == string.Empty || lng == string.Empty)
            {
                string mapsAPIKey = System.Web.Configuration.WebConfigurationManager.AppSettings["MapsAPIKey"].Trim();

                GeoJsonData g = genFunction.GeoRequest(add, mapsAPIKey);

                if (g.results.Length > 0)
                {
                    var p = g.results[0].geometry.location; lat = p.lat.ToString(); lat = p.lng.ToString();
                }
            }
            StateCollection<GoogleMarker> scMarker = new StateCollection<GoogleMarker>();

            if (lat != string.Empty && lng != string.Empty)
            {


                objpropMapData.ConnConfig = Session["config"].ToString();

                objpropMapData.Lat = lat; objpropMapData.Lng = lng;

                objpropMapData.Worker = "";

                if (ddlTech.SelectedIndex != 0)

                {
                    objpropMapData.Worker = ddlTech.SelectedItem.Text;
                }
                ds = objBL_MapData.GetNearWorkersByTime(objpropMapData);


            }

            return ds.Tables[0];
        }
        catch (Exception ex)
        {


            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrNear", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return ds.Tables[0];
        }
    }

    #endregion


    protected void RadGridOpenCalls_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {

        RadGridOpenCalls.AllowCustomPaging = !ShouldApplySortFilterOrGroup(RadGridOpenCalls);
        FillOpenCallsGrid();

    }

    bool isGrouping = false;

    public bool ShouldApplySortFilterOrGroup(RadGrid RD)
    {
        return RD.MasterTableView.FilterExpression != "" ||
            (RD.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RD.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadgvTimeStmp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadgvTimeStmp.AllowCustomPaging = !ShouldApplySortFilterOrGroup(RadgvTimeStmp);
        FillTimeStGrid();


    }

    private void FillCategory()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getCategory(objProp_User);

        ddlCategory.DataSource = ds.Tables[0];
        ddlCategory.DataTextField = "type";
        ddlCategory.DataValueField = "type";
        ddlCategory.DataBind();

    }
    private string GetSelectedCategory()
    {
        string selectedvals = string.Empty;

        foreach (RadComboBoxItem item in ddlCategory.CheckedItems)
        {
            if (item.Checked == true)
            {
                selectedvals += "'" + item.Value + "'" + ',';
            }
        }
        return selectedvals.TrimEnd(',');
    }

}
