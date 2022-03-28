using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Artem.Web.UI.Controls;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using BusinessEntity;
using BusinessLayer;
//using Microsoft.SqlServer.Management.Smo;
//using Microsoft.SqlServer.Management.Common;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using MobilePushNotification;
using System.Web.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using Telerik.Web.UI;

public partial class Map : Page
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

        string mapsAPIKey = System.Web.Configuration.WebConfigurationManager.AppSettings["MapsAPIKey"].Trim();
        GoogleMap1.Key = mapsAPIKey;

        Permission();

        if (!IsPostBack)
        {

            hdnwebconfig.Value= SSTCryptographer.Encrypt(Session["config"].ToString(), "webconfig");

            string SSL = WebConfigurationManager.AppSettings["SSL"].Trim();

            if (Request.Url.Scheme == "https" && SSL=="1")
            {
                string URL = Request.Url.ToString(); 
                URL = URL.Replace("https://", "http://");
                Response.Redirect(URL);
            }

            GetDefaultLat_Lng();           
            txtDate.SelectedDate = DateTime.Now;
            FillFieldWorkers();
            FillCategory();          
            FillTechCurrentLocation();
            FillOpenCallsGrid();
            ShowOpenCallsOnMap(); 
        }
        CompanyPermission();
    }

    private void GetDefaultLat_Lng()
    {
        DataSet dsLat_Lng = new DataSet();
        DataTable dt = new DataTable(); 
        dsLat_Lng = SqlHelper.ExecuteDataset(Session["config"].ToString(), CommandType.Text, "SELECT ISNULL(NULLIF(Lat, ''), 0) Lat, ISNULL(NULLIF(Lng, ''), 0) Lng FROM CONTROL");
        
        if (dsLat_Lng.Tables[0].Rows.Count > 0)
        {
            if (dsLat_Lng.Tables[0].Rows[0]["Lat"].ToString() != "0" && dsLat_Lng.Tables[0].Rows[0]["Lat"].ToString() != "")
                double.TryParse(dsLat_Lng.Tables[0].Rows[0]["Lat"].ToString(), out Latitude);
            else
                Latitude = Convert.ToDouble("39.0917394");
            if (dsLat_Lng.Tables[0].Rows[0]["Lat"].ToString() != "0" && dsLat_Lng.Tables[0].Rows[0]["Lat"].ToString() != "")
                double.TryParse(dsLat_Lng.Tables[0].Rows[0]["Lng"].ToString(), out Longitude);
            else
                Longitude = Convert.ToDouble("-94.5828553");
        }
        else
        {
            dsLat_Lng = new DataSet();
            dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("Lat");
            dt.Columns.Add("Lng");
            DataRow _ravi = dt.NewRow();
            _ravi["Lat"] = "39.0917394";
            _ravi["Lng"] = "-94.5828553";
            dt.Rows.Add(_ravi);
            dsLat_Lng.Tables.Add(dt);
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
            //  gr.Attributes["ondblclick"] = "showModalPopupViaClientCust(" + lblTicketId.Text + ",0);";
        }

        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alertScript", "SelectedRowStyle('" + gvOpenCalls.ClientID + "');", true);


        HighlightSideMenu("schMgr", "lnkMapMenu", "schdMgrSub");

    }

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

    private void Permission()
    {

        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderSchd");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("schdMgrSub");
        //ul.Style.Add("display", "block");
        //ul.Style.Add("visibility", "visible");
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
                objProp_User.PageName = "map.aspx";
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
        //if (Session["COPer"].ToString() == "1")
        //{
        //    gvOpenCalls.Columns[5].Visible = true;
        //}
        //else
        //{
        //    gvOpenCalls.Columns[5].Visible = false;
        //}
    }

    #region FillCurrentLocation
    private void FillCurrentLocation()
    {
        StateCollection<GoogleMarker> scMarker = new StateCollection<GoogleMarker>();
        string connstr = Session["config"].ToString();
        DataSet ds = new DataSet();
        objpropMapData.ConnConfig = connstr;

        try
        {
            ds = objBL_MapData.GetCurrentLocation(objpropMapData);
            if (ds.Tables[0].Rows.Count > 0)
            {
                GoogleMap1.Zoom = 4;
                GoogleMap1.Latitude = Convert.ToDouble(ds.Tables[0].Rows[0]["Latitude"]);
                GoogleMap1.Longitude = Convert.ToDouble(ds.Tables[0].Rows[0]["Longitude"]);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    GoogleMarker bm = new GoogleMarker();
                    bm.Latitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Latitude"]);
                    bm.Longitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Longitude"]);
                    bm.Clickable = true;
                    string info = "Position of " + ds.Tables[0].Rows[i]["callsign"] + " on " + Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["date"]).ToShortTimeString());//ToUniversalTime()
                    bm.Text = info;

                    scMarker.Add(bm);
                }
                GoogleMap1.Markers.AddRange(scMarker);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrCurrent", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }
    #endregion

    private void ShowOpenCallsOnMap()
    {
        if (chkOpencalls.Checked == true)
        {
            StateCollection<GoogleMarker> scMarker = new StateCollection<GoogleMarker>();
            //string connstr = Session["config"].ToString();
            //DataSet ds = new DataSet();
            //objpropMapData.ConnConfig = connstr;

            GoogleSize gsLoc = new GoogleSize();
            gsLoc.Height = 25; //23;
            gsLoc.Width = 25;//13;

            try
            {
                DataTable dtopencall = (DataTable)Session["opencalldata"];
                var uniqueContacts = dtopencall.AsEnumerable()
                           .GroupBy(x => x.Field<string>("ldesc1"))
                           .Select(g => g.First());
                DataTable dt = uniqueContacts.CopyToDataTable();
                //ds = objBL_MapData.GetTechCurrentLocation(objpropMapData);
                if (dt.Rows.Count > 0)
                {
                    GoogleMap1.Zoom = 4;
                    //GoogleMap1.Address = dt.Rows[0]["address"].ToString();
                    int cnt = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //if (i < 5)
                        //{
                        GoogleMarker bm = new GoogleMarker();
                        //bm.Address = dt.Rows[i]["address"].ToString();
                        if (dt.Rows[i]["Lat"].ToString() != string.Empty)
                        {
                            if (cnt == 0)
                            {
                                GoogleMap1.Latitude = Convert.ToDouble(dt.Rows[i]["Lat"]);
                                GoogleMap1.Longitude = Convert.ToDouble(dt.Rows[i]["Lng"]);
                            }
                            cnt = 1;
                            bm.Latitude = Convert.ToDouble(dt.Rows[i]["Lat"].ToString());
                            bm.Longitude = Convert.ToDouble(dt.Rows[i]["lng"].ToString());
                            bm.Clickable = true;
                            string info = string.Empty;
                            string Cat = dt.Rows[i]["cat"].ToString();
                            string IconStr = "imagehandler.ashx?catid=" + Cat;

                            if (string.Equals(dt.Rows[i]["assignname"].ToString(), "un-assigned", StringComparison.InvariantCultureIgnoreCase))
                            {
                                //bm.IconUrl = "http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=U|F2AC09|000";
                                bm.IconUrl = "imagehandler.ashx?assign=U&catid=" + Cat;
                                bm.IconSize = gsLoc;
                                info = "<b>Ticket # " + dt.Rows[i]["id"].ToString() + "</b> <br/> <img src='" + IconStr + "' /> Category:" + Cat + " </BR></BR>" + dt.Rows[i]["ldesc1"].ToString() + "<BR/>" + dt.Rows[i]["address"].ToString();
                            }
                            else
                            {
                                //bm.IconUrl = "http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=A|0947F2|000";
                                bm.IconUrl = "imagehandler.ashx?assign=A&catid=" + Cat;
                                bm.IconSize = gsLoc;
                                info = "<b>Ticket # " + dt.Rows[i]["id"].ToString() + "</b> - " + dt.Rows[i]["dwork"].ToString() + " <br/> <img src='" + IconStr + "' /> Category:" + Cat + " </BR></BR>" + dt.Rows[i]["ldesc1"].ToString() + "<BR/>" + dt.Rows[i]["address"].ToString();
                            }
                            bm.Text = info;
                            scMarker.Add(bm);
                        }
                        //}
                    }
                    GoogleMap1.Markers.AddRange(scMarker);
                }
                else
                {

                    GoogleMap1.Zoom = 4;
                    GoogleMap1.Latitude = Latitude;
                    GoogleMap1.Longitude = Longitude;

                }
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErrCurrent", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }

    private void FillTechCurrentLocation()
    {
        StateCollection<GoogleMarker> scMarker = new StateCollection<GoogleMarker>();
        string connstr = Session["config"].ToString();
        DataSet ds = new DataSet();
        objpropMapData.ConnConfig = connstr;

        try
        {
            ds = objBL_MapData.GetTechCurrentLocation(objpropMapData);
            if (ds.Tables[0].Rows.Count > 0)
            {
                GoogleMap1.Zoom = 4;
                GoogleMap1.Latitude = Convert.ToDouble(ds.Tables[0].Rows[0]["Latitude"]);
                GoogleMap1.Longitude = Convert.ToDouble(ds.Tables[0].Rows[0]["Longitude"]);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    GoogleMarker bm = new GoogleMarker();
                    bm.Latitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Latitude"]);
                    bm.Longitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Longitude"]);
                    bm.Clickable = true;
                    string info = "Position of " + ds.Tables[0].Rows[i]["callsign"] + " on " + Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["date"]).ToShortTimeString());
                    bm.Text = info;
                    if (ds.Tables[0].Rows[i]["GPS"].ToString() == "0")
                    {
                        GoogleSize gsLoc = new GoogleSize();
                        gsLoc.Height = 30;
                        gsLoc.Width = 30;
                        //bm.IconUrl = "http://maps.google.com/mapfiles/ms/icons/green-dot.png";
                        bm.IconUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["IconUrlTechCurrentLocation"].Trim() + "mapfiles/ms/icons/green-dot.png";
                        bm.IconSize = gsLoc;
                    }
                    bm.Title = ds.Tables[0].Rows[i]["callsign"].ToString();
                    scMarker.Add(bm);
                }
                GoogleMap1.Markers.AddRange(scMarker);
            }

            else
            {

                GoogleMap1.Zoom = 4;
                GoogleMap1.Latitude = Latitude;
                GoogleMap1.Longitude = Longitude;

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrCurrent", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #region Field Workers

    private void FillFieldWorkers()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();

        //ds = objBL_User.getFieldUser(objProp_User);
        //ddlTech.DataSource = ds.Tables[0];
        //ddlTech.DataTextField = "fdesc";
        //ddlTech.DataValueField = "id";
        //ddlTech.DataBind();

        //ddlTech.Items.Insert(0,new ListItem("Select",""));
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
        btnSearch.Enabled = false;
        FillOpenCallsGrid();
        RadGridOpenCalls.Rebind();
        //if (chkAssignedCall.Checked == true)
        //{
        FillTimeStGrid();
        RadgvTimeStmp.Rebind();
        //}
        LoadMapWithGSPDataAndTicketData(); //If ddl Tech Dropdown Selected
        ShowOpenCallsOnMap(); //If ShowOpenCallsOnMap checkbox checked
        btnSearch.Enabled = true;
    }


    protected void btnRrefresh_Click(object sender, EventArgs e)
    {
        GoogleMap1.Zoom = 15;
        GoogleMap1.Markers.Clear();
        GoogleMap1.Polylines.Clear();
        GoogleMap1.Directions.Clear();

        StateCollection<GooglePolyline> scPolyline = new StateCollection<GooglePolyline>();

        GoogleSize gs = new GoogleSize();
        gs.Height = 30;
        gs.Width = 20;

        GoogleSize gsArrow = new GoogleSize();
        gsArrow.Height = 16;
        gsArrow.Width = 16;

        GoogleSize gsTime = new GoogleSize();
        gsTime.Height = 45;
        gsTime.Width = 137;

        GoogleSize gsLoc = new GoogleSize();
        gsLoc.Height = 50;
        gsLoc.Width = 30;

        GooglePolyline pl = new GooglePolyline();
        pl.Color = System.Drawing.Color.Red;
        pl.Opacity = Convert.ToSingle(1.0);
        pl.Weight = 2;

        //GooglePolyline pl3 = new GooglePolyline();
        //pl3.Color = System.Drawing.Color.Cyan;

        string dbname = Session["dbname"].ToString();
        string connstr = Session["config"].ToString();
        DateTime Date = Convert.ToDateTime(txtDate.SelectedDate);
        string tech = ddlTech.SelectedItem.Text;

        objpropMapData.ConnConfig = connstr;
        objpropMapData.Date = Date;
        objpropMapData.Tech = tech;

        try
        {

            #region Populate data from gps data

            DataSet ds = new DataSet();
            ds = objBL_MapData.GetTimestmpLocation(objpropMapData);
            int locate = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                GoogleMap1.Latitude = Convert.ToDouble(ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1]["Latitude"]);
                GoogleMap1.Longitude = Convert.ToDouble(ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1]["Longitude"]);
                locate = 1;
                StateCollection<GoogleMarker> scMarker = new StateCollection<GoogleMarker>();
                double distance = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string info = Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["date"]).ToShortTimeString());

                    if (i == 0)
                    {
                        GoogleMarker bm = new GoogleMarker();
                        bm.Latitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Latitude"]);
                        bm.Longitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Longitude"]);
                        //bm.IconUrl = "http://www.googlemapsmarkers.com/v1/S/0099FF/FFFFFF/FF0000/";
                        bm.IconUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["IconMarkerUrl"].Trim() + "S/0099FF/FFFFFF/FF0000/";
                        bm.IconSize = gs;
                        bm.Text = "Starting point - " + info;
                        bm.Title = "Starting point - " + info;
                        bm.Clickable = true;
                        bm.InfoWindowAnchor = new GooglePoint(0, 0);
                        scMarker.Add(bm);
                    }
                    if (i == ds.Tables[0].Rows.Count - 1)
                    {
                        GoogleMarker bm = new GoogleMarker();
                        bm.Latitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Latitude"]);
                        bm.Longitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Longitude"]);
                        bm.IconUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["IconMarkerUrl"].Trim() + "E/0099FF/FFFFFF/FF0000/";
                        //bm.IconUrl = "http://www.googlemapsmarkers.com/v1/E/0099FF/FFFFFF/FF0000/";
                        //bm.IconUrl = "http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=E|FF0202|000";
                        bm.IconSize = gs;
                        bm.Text = "End point - " + info;
                        bm.Title = "End point - " + info;
                        bm.Clickable = true;
                        bm.InfoWindowAnchor = new GooglePoint(0, 0);
                        scMarker.Add(bm);
                    }
                    //CT
                    if (ds.Tables[0].Rows[i]["timestm"].ToString() == "1")
                    {
                        string GPSaddress = string.Empty;
                        GoogleMarker bm = new GoogleMarker();
                        bm.Latitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Latitude"]);
                        bm.Longitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Longitude"]);
                        GeoJsonData g = genFunction.GeoRequest(bm.Latitude.ToString(), bm.Longitude.ToString());
                        if (g.results.Count() > 0)
                        {
                            GPSaddress = g.results[0].formatted_address;
                        }
                        info = "<strong> Ticket #: " + ds.Tables[0].Rows[i]["id"].ToString() + "</strong></BR></BR><strong> Name: " + ds.Tables[0].Rows[i]["Name"].ToString() + "</strong> </BR></BR>" + "Current Address: " + GPSaddress + " </BR></BR>" + Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["date"]));//.ToUniversalTime()
                        bm.Text = info;
                        bm.IconUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["IconMarkerUrl"].Trim() + "CT/0099FF/";
                        //bm.IconUrl = "http://www.googlemapsmarkers.com/v1/CT/0099FF/";
                        bm.IconSize = gs;
                        bm.Clickable = true;
                        bm.Title = Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["date"]).ToShortTimeString());
                        bm.InfoWindowAnchor = new GooglePoint(0, 0);
                        scMarker.Add(bm);
                    }
                    //OS
                    else if (ds.Tables[0].Rows[i]["timestm"].ToString() == "2")
                    {
                        string GPSaddress = string.Empty;
                        GoogleMarker bm = new GoogleMarker();
                        bm.Latitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Latitude"]);
                        bm.Longitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Longitude"]);
                        GeoJsonData g = genFunction.GeoRequest(bm.Latitude.ToString(), bm.Longitude.ToString());
                        if (g.results.Count() > 0)
                        {
                            GPSaddress = g.results[0].formatted_address;
                        }
                        info = "<strong> Ticket #: " + ds.Tables[0].Rows[i]["id"].ToString() + "</strong></BR></BR><strong> Name: " + ds.Tables[0].Rows[i]["Name"].ToString() + "</strong> </BR></BR>" + "Current Address: " + GPSaddress + " </BR></BR>" + Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["date"]));//.ToUniversalTime()+ "Current Address: " + GPSaddress + " </BR></BR>" 
                        bm.Text = info;
                        bm.IconUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["IconMarkerUrl"].Trim() + "OS/0099FF/";
                        //bm.IconUrl = "http://www.googlemapsmarkers.com/v1/OS/0099FF/";
                        bm.IconSize = gs;
                        bm.Clickable = true;
                        bm.Title = Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["date"]).ToShortTimeString());
                        bm.InfoWindowAnchor = new GooglePoint(0, 0);
                        scMarker.Add(bm);
                    }
                    //ER
                    else if (ds.Tables[0].Rows[i]["timestm"].ToString() == "3")
                    {
                        string GPSaddress = string.Empty;
                        GoogleMarker bm = new GoogleMarker();
                        bm.Latitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Latitude"]);
                        bm.Longitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Longitude"]);
                        GeoJsonData g = genFunction.GeoRequest(bm.Latitude.ToString(), bm.Longitude.ToString());
                        if (g.results.Count() > 0)
                        {
                            GPSaddress = g.results[0].formatted_address;
                        }
                        info = "<strong> Ticket #: " + ds.Tables[0].Rows[i]["id"].ToString() + "</strong></BR></BR><strong> Name: " + ds.Tables[0].Rows[i]["Name"].ToString() + "</strong> </BR></BR>" + "Current Address: " + GPSaddress + " </BR></BR>" + Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["date"]));//.ToUniversalTime() "Current Address: " + GPSaddress + " </BR></BR>" +
                        bm.Text = info;
                        bm.IconUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["IconMarkerUrl"].Trim() + "ER/0099FF/";
                        //bm.IconUrl = "http://www.googlemapsmarkers.com/v1/ER/0099FF/";
                        bm.IconSize = gs;
                        bm.Clickable = true;
                        bm.Title = Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["date"]).ToShortTimeString());
                        bm.InfoWindowAnchor = new GooglePoint(0, 0);
                        scMarker.Add(bm);
                    }

                    if (i != ds.Tables[0].Rows.Count - 1)
                    {
                        //double distance = genFunction.GetDistance(ds.Tables[0].Rows[i]["Latitude"].ToString(), ds.Tables[0].Rows[i]["Longitude"].ToString(), ds.Tables[0].Rows[i + 1]["Latitude"].ToString(), ds.Tables[0].Rows[i + 1]["Longitude"].ToString());
                        double latdistance = Distance(ds.Tables[0].Rows[i]["Latitude"].ToString(), ds.Tables[0].Rows[i]["Longitude"].ToString(), ds.Tables[0].Rows[i + 1]["Latitude"].ToString(), ds.Tables[0].Rows[i + 1]["Longitude"].ToString());
                        distance += latdistance;
                        if (Convert.ToInt32(ds.Tables[0].Rows[0]["MAPID"].ToString()) == 0 || distance >= 2.5)//0.05
                        {
                            distance = 0;
                            double p1 = Convert.ToDouble(ds.Tables[0].Rows[i]["Latitude"]);
                            double p2 = Convert.ToDouble(ds.Tables[0].Rows[i]["Longitude"]);
                            double p3 = Convert.ToDouble(ds.Tables[0].Rows[i + 1]["Latitude"]);
                            double p4 = Convert.ToDouble(ds.Tables[0].Rows[i + 1]["Longitude"]);
                            GoogleLocation p5 = new GoogleLocation((p1 + p3) / 2, (p2 + p4) / 2);

                            double dir = genFunction.bearing(p1, p2, p3, p4);
                            // == round it to a multiple of 3 and cast out 120s
                            dir = Math.Round(dir / 3) * 3;
                            while (dir >= 120) { dir -= 120; }

                            info = Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["date"]).ToShortTimeString());

                            GoogleMarker bmArrow = new GoogleMarker();
                            bmArrow.Latitude = p5.Latitude;
                            bmArrow.Longitude = p5.Longitude;
                            bmArrow.Clickable = true;
                            bmArrow.IconUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["IconDirUrl"].Trim() + "/intl/en_ALL/mapfiles/dir_" + dir + ".png";
                            //bmArrow.IconUrl = "http://www.google.com/intl/en_ALL/mapfiles/dir_" + dir + ".png";
                            bmArrow.IconSize = gsArrow;
                            bmArrow.IconAnchor = new GooglePoint(8, 8);
                            bmArrow.Title = info;
                            bmArrow.Text = info;
                            scMarker.Add(bmArrow);
                        }
                    }

                    GoogleLocation gl = new GoogleLocation();
                    gl.Latitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Latitude"]);
                    gl.Longitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Longitude"]);
                    pl.Points.Add(gl);

                }
                GoogleMap1.Markers.AddRange(scMarker);
            }
            else
            {

                GoogleMap1.Zoom = 4;
                GoogleMap1.Latitude = Latitude;
                GoogleMap1.Longitude = Longitude;

            }

            #endregion


            #region Populate data for open tickets

            if (chkAssignedCall.Checked == true)
            {
                DataSet dsOpenTicket = new DataSet();
                dsOpenTicket = objBL_MapData.GetOpenTicket(objpropMapData);

                if (dsOpenTicket.Tables[0].Rows.Count > 0)
                {
                    StateCollection<GoogleDirection> scDirection = new StateCollection<GoogleDirection>();
                    if (locate == 0)
                        GoogleMap1.Address = dsOpenTicket.Tables[0].Rows[0]["address"].ToString();

                    for (int i = 0; i < dsOpenTicket.Tables[0].Rows.Count; i++)
                    {
                        string add = dsOpenTicket.Tables[0].Rows[i]["address"].ToString();
                        string assign = dsOpenTicket.Tables[0].Rows[i]["assigned"].ToString();
                        string ldesc = dsOpenTicket.Tables[0].Rows[i]["ldesc1"].ToString();
                        string edate = dsOpenTicket.Tables[0].Rows[i]["edate"].ToString();
                        string TicketID = dsOpenTicket.Tables[0].Rows[i]["id"].ToString();
                        string Cat = dsOpenTicket.Tables[0].Rows[i]["cat"].ToString();
                        string IconStr = "imagehandler.ashx?catid=" + Cat;

                        //GeoJsonData g =genFunction.GeoRequest(add);

                        //if (g.results.Length > 0)
                        //{
                        //    var p = g.results[0].geometry.location;
                        //    double latitude = p.lat;
                        //    double longitude = p.lng;

                        //    GoogleLocation gl = new GoogleLocation();
                        //    gl.Latitude = latitude;
                        //    gl.Longitude = longitude;

                        //    pl3.Points.Add(gl);
                        //}

                        GoogleMarker bm = new GoogleMarker();
                        bm.Address = add;
                        bm.Clickable = true;
                        bm.Title = ldesc;
                        bm.Text = "<strong> Ticket #: " + TicketID + "</strong></BR></BR><img src='" + IconStr + "' /> Category:" + Cat + " </BR></BR> <strong>" + ldesc + "</strong></BR></BR> " + add + " </BR></BR>" + Convert.ToDateTime(edate).ToShortTimeString();
                        if (assign == "1")
                        {
                            bm.IconUrl = "~/images/white.png";
                            bm.IconSize = gsLoc;
                        }
                        else if (assign == "2")
                        {
                            bm.IconUrl = "~/images/green.png";
                            bm.IconSize = gsLoc;
                        }
                        else if (assign == "3")
                        {
                            bm.IconUrl = "~/images/orange.png";
                            bm.IconSize = gsLoc;
                        }
                        else if (assign == "4")
                        {
                            bm.IconUrl = "~/images/blue.png";
                            bm.IconSize = gsLoc;
                        }
                        else if (assign == "5")
                        {
                            bm.IconUrl = "~/images/yellow.png";
                            bm.IconSize = gsLoc;
                        }
                        GoogleMap1.Markers.Add(bm);

                        GoogleDirection d = new GoogleDirection();
                        d.RoutePanelId = "ctl00_ContentPlaceHolder1_route";

                        if (i != 0)
                        {
                            d.Query = OpenAddress + " to " + add;
                            if (chkEffective.Checked == true)
                            {
                                scDirection.Add(d);
                            }
                        }

                        OpenAddress = add;
                    }

                    GoogleMap1.Directions.AddRange(scDirection);
                }
            }

            #endregion


            scPolyline.Add(pl);
            //scPolyline.Add(pl3);

            GoogleMap1.Polylines.AddRange(scPolyline);

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrMain", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }



    //private void GetUpd()
    //{        
    //    objgeneral.ConnConfig = HttpContext.Current.Session["config"].ToString();

    //    //objGeneral.DeviceID = "123";
    //    objgeneral.DeviceID = "b2aa1c6d-fb69-37c8-9257-65882afb83d5";
    //    objgeneral.RegID = ViewState["random"].ToString();
    //    DataSet dspingResponse = objBL_General.getPing(objgeneral);

    //    //return pingResponse.ToString();
    //    if (dspingResponse.Tables[0].Rows.Count > 0)
    //    {
    //        string strRunning = "GPS Tracking : OFF";
    //        if (dspingResponse.Tables[0].Rows[0]["isrunning"].ToString() == "1")
    //        {
    //            strRunning = "GPS Tracking : ON";
    //        }

    //        string strGPS = "GPS : OFF";
    //        if (dspingResponse.Tables[0].Rows[0]["IsGPSEnabled"].ToString() == "1")
    //        {
    //            strGPS = "GPS : ON";
    //        }

    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "keyPing", "noty({text: '" + strRunning + "</BR>" + strGPS + "',  type : 'information', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

    //        Timer1.Enabled = false;
    //        Timer2.Enabled = false;
    //    }
    //}
    //protected void Timer1_Tick(object sender, EventArgs e)
    //{
    //    GetUpd();
    //}
    //protected void Timer2_Tick(object sender, EventArgs e)
    //{
    //    Timer1.Enabled = false;
    //    Timer2.Enabled = false;
    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "keyPingTimeout", "noty({text: '<strong>Request Timeout!</strong></br></br> Please check if the device </br>1) Is not switched off </br>2) GPS tracking app is not installed </br>3) Has no google account configured </br>4) Has no internet connectivity',  type : 'information', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //}

    protected void lnkReset_Click(object sender, EventArgs e)
    {
        GoogleMap1.Markers.Clear();
        GoogleMap1.Polylines.Clear();
        GoogleMap1.Directions.Clear();
        txtDate.SelectedDate = DateTime.Now;
        ddlTech.SelectedIndex = -1;
        //ddlCategory.SelectedIndex = -1;
        chkAssignedCall.Checked = false;
        chkOpencalls.Checked = false;
        chkEffective.Checked = false;
        ddlCategory.Items.Clear();
        FillCategory();
        //FillCurrentLocation();
        FillTechCurrentLocation();
        FillOpenCallsGrid();
        ShowOpenCallsOnMap(); //If ShowOpenCallsOnMap checkbox checked
        FillTimeStGrid();
        RadGridOpenCalls.Rebind();
        RadgvTimeStmp.Rebind();
    }

    public double Distance(string Lat1, string Lon1, string Lat2, string Lon2)
    {
        //1- miles
        //double R = (type == 1) ? 3960 : 6371;          // R is earth radius.
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

    #region Push Notification Feature

    //protected void btnPing_Click(object sender, EventArgs e)
    //{
    //    GoogleMap1.Markers.Clear();
    //    GoogleMap1.Polylines.Clear();
    //    GoogleMap1.Directions.Clear();
    //    SendPush();
    //}

    //private void SendPush()
    //{
    //    AndroidPushNotification theAndroidPn = new AndroidPushNotification();

    //    //objgeneral.DeviceID = "66c441f9-cb2c-3cce-b132-1531ad3088f9"; 
    //    //objgeneral.DeviceID = "b2aa1c6d-fb69-37c8-9257-65882afb83d5";
    //    objProp_User.ConnConfig = Session["config"].ToString();
    //    objProp_User.Username = ddlTech.SelectedItem.Text;
    //    objgeneral.DeviceID = objBL_User.getUserDeviceID(objProp_User);
    //    hdnDeviceID.Value = objgeneral.DeviceID;

    //    string tokenID = objBL_General.GetDeviceTokenID(objgeneral);
    //    string strRandom = genFunction.generateRandomString(10);
    //    hdnRandomid.Value = strRandom;
    //    if (tokenID != "")
    //    {
    //        string sResponseFromServer = "";
    //        sResponseFromServer = theAndroidPn.PushToAndroid(WebConfigurationManager.AppSettings["AndroidPNServerUrl"].Trim(),
    //                                         strRandom,
    //                                          WebConfigurationManager.AppSettings["GoogleAppID"].Trim(),
    //                                          WebConfigurationManager.AppSettings["SENDER_ID"].Trim(),
    //                                          DateTime.UtcNow.ToString(),
    //                                          tokenID);

    //        if (sResponseFromServer.Contains("Error") == false)
    //        {
    //            //ScriptManager.RegisterStartupScript(this, this.GetType(), "keyalertpush", "alert('"+sResponseFromServer+"');", true);
    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "keypush", "ajaxcall();", true);
    //        }
    //        else
    //        {
    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "keyErrorPing", "noty({text: 'Error pinging the device : " + sResponseFromServer + "',dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

    //        }
    //    }
    //    else
    //    {
    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "keyNR", "noty({text: 'Device not registered for ping.', dismissQueue: true, type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}

    #endregion

    #region MAP Feature
    private bool chkOneMonthExist()
    {
        bool isExist = objBL_MapData.chkOneMonthExist();
        return isExist;
     }
    private void LoadMapWithGSPDataAndTicketData()
    {
        GoogleMap1.Zoom = 15;
        GoogleMap1.Markers.Clear();
        GoogleMap1.Polylines.Clear();
        GoogleMap1.Directions.Clear();

        StateCollection<GooglePolyline> scPolyline = new StateCollection<GooglePolyline>();

        GoogleSize gs = new GoogleSize();
        gs.Height = 30;
        gs.Width = 20;

        GoogleSize gsArrow = new GoogleSize();
        gsArrow.Height = 16;
        gsArrow.Width = 16;

        GoogleSize gsTime = new GoogleSize();
        gsTime.Height = 45;
        gsTime.Width = 137;

        GoogleSize gsLoc = new GoogleSize();
        gsLoc.Height = 50;
        gsLoc.Width = 30;

        GooglePolyline pl = new GooglePolyline();
        pl.Color = System.Drawing.Color.Red;
        pl.Opacity = Convert.ToSingle(1.0);
        pl.Weight = 2;

        //GooglePolyline pl3 = new GooglePolyline();
        //pl3.Color = System.Drawing.Color.Cyan;

        string dbname = Session["dbname"].ToString();
        string connstr = Session["config"].ToString();
        DateTime Date = Convert.ToDateTime(txtDate.SelectedDate);       
        DateTime  LastMonthDate = DateTime.Today.AddMonths(-1);
        string tech = ddlTech.SelectedItem.Text;
        objpropMapData.ConnConfig = connstr;
        objpropMapData.Date = Date;
        objpropMapData.Tech = tech;
        objpropMapData.Category = GetSelectedCategory();
        objpropMapData.ISTicketD = Convert.ToInt16(chkAssignedCall.Checked);

        try
        {

            #region Populate data from gps data

            DataSet ds = new DataSet();
           
            ds = objBL_MapData.GetTimestmpLocationLatest(objpropMapData); 
            
            int locate = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                GoogleMap1.Latitude = Convert.ToDouble(ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1]["Latitude"]);
                GoogleMap1.Longitude = Convert.ToDouble(ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1]["Longitude"]);
                locate = 1;
                StateCollection<GoogleMarker> scMarker = new StateCollection<GoogleMarker>();
                double distance = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string info = Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["date"]).ToShortTimeString());

                    if (i == 0)
                    {
                        GoogleMarker bm = new GoogleMarker();
                        bm.Latitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Latitude"]);
                        bm.Longitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Longitude"]);
                        bm.IconUrl = "http://www.googlemapsmarkers.com/v1/S/0099FF/FFFFFF/FF0000/";
                        //bm.IconUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["IconMarkerUrl"].Trim() + "S/0099FF/FFFFFF/FF0000/";
                        bm.IconSize = gs;
                        bm.Text = "Starting point - " + info;
                        bm.Title = "Starting point - " + info;
                        bm.Clickable = true;
                        bm.InfoWindowAnchor = new GooglePoint(0, 0);
                        scMarker.Add(bm);
                    }
                    if (i == ds.Tables[0].Rows.Count - 1)
                    {
                        GoogleMarker bm = new GoogleMarker();
                        bm.Latitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Latitude"]);
                        bm.Longitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Longitude"]);
                        bm.IconUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["IconMarkerUrl"].Trim() + "E/0099FF/FFFFFF/FF0000/";
                        bm.IconUrl = "http://www.googlemapsmarkers.com/v1/E/0099FF/FFFFFF/FF0000/";
                        //bm.IconUrl = "http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=E|FF0202|000";
                        bm.IconSize = gs;
                        bm.Text = "End point - " + info;
                        bm.Title = "End point - " + info;
                        bm.Clickable = true;
                        bm.InfoWindowAnchor = new GooglePoint(0, 0);
                        scMarker.Add(bm);
                    }

                    if (chkAssignedCall.Checked==true)
                    {
                        //CT
                        if (    (ds.Tables[0].Rows[i]["timestm"].ToString()) == "1")
                        {
                            string GPSaddress = string.Empty;
                            GoogleMarker bm = new GoogleMarker();
                            bm.Latitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Latitude"]);
                            bm.Longitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Longitude"]);
                            GeoJsonData g = genFunction.GeoRequest(bm.Latitude.ToString(), bm.Longitude.ToString());
                            if (g.results.Count() > 0)
                            {
                                GPSaddress = g.results[0].formatted_address;
                            }
                            info = "<strong> Ticket #: " + ds.Tables[0].Rows[i]["id"].ToString() + "</strong></BR></BR><strong> Name: " + ds.Tables[0].Rows[i]["Name"].ToString() + "</strong> </BR></BR>" + "Current Address: " + GPSaddress + " </BR></BR>" + Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["date"]));//.ToUniversalTime()
                            bm.Text = info;
                            //bm.IconUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["IconMarkerUrl"].Trim() + "CT/0099FF/";
                            bm.IconUrl = "http://www.googlemapsmarkers.com/v1/CT/0099FF/";
                            bm.IconSize = gs;
                            bm.Clickable = true;
                            bm.Title = Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["date"]).ToShortTimeString());
                            bm.InfoWindowAnchor = new GooglePoint(0, 0);
                            scMarker.Add(bm);
                        }
                        //OS
                        else if (ds.Tables[0].Rows[i]["timestm"].ToString() == "2")
                        {
                            string GPSaddress = string.Empty;
                            GoogleMarker bm = new GoogleMarker();
                            bm.Latitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Latitude"]);
                            bm.Longitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Longitude"]);
                            GeoJsonData g = genFunction.GeoRequest(bm.Latitude.ToString(), bm.Longitude.ToString());
                            if (g.results.Count() > 0)
                            {
                                GPSaddress = g.results[0].formatted_address;
                            }
                            info = "<strong> Ticket #: " + ds.Tables[0].Rows[i]["id"].ToString() + "</strong></BR></BR><strong> Name: " + ds.Tables[0].Rows[i]["Name"].ToString() + "</strong> </BR></BR>" + "Current Address: " + GPSaddress + " </BR></BR>" + Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["date"]));//.ToUniversalTime()+ "Current Address: " + GPSaddress + " </BR></BR>" 
                            bm.Text = info;
                            //bm.IconUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["IconMarkerUrl"].Trim() + "OS/0099FF/";
                            bm.IconUrl = "http://www.googlemapsmarkers.com/v1/OS/0099FF/";
                            bm.IconSize = gs;
                            bm.Clickable = true;
                            bm.Title = Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["date"]).ToShortTimeString());
                            bm.InfoWindowAnchor = new GooglePoint(0, 0);
                            scMarker.Add(bm);
                        }
                        //ER
                        else if (ds.Tables[0].Rows[i]["timestm"].ToString() == "3")
                        {
                            string GPSaddress = string.Empty;
                            GoogleMarker bm = new GoogleMarker();
                            bm.Latitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Latitude"]);
                            bm.Longitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Longitude"]);
                            GeoJsonData g = genFunction.GeoRequest(bm.Latitude.ToString(), bm.Longitude.ToString());
                            if (g.results.Count() > 0)
                            {
                                GPSaddress = g.results[0].formatted_address;
                            }
                            info = "<strong> Ticket #: " + ds.Tables[0].Rows[i]["id"].ToString() + "</strong></BR></BR><strong> Name: " + ds.Tables[0].Rows[i]["Name"].ToString() + "</strong> </BR></BR>" + "Current Address: " + GPSaddress + " </BR></BR>" + Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["date"]));//.ToUniversalTime() "Current Address: " + GPSaddress + " </BR></BR>" +
                            bm.Text = info;
                            //bm.IconUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["IconMarkerUrl"].Trim() + "ER/0099FF/";
                            bm.IconUrl = "http://www.googlemapsmarkers.com/v1/ER/0099FF/";
                            bm.IconSize = gs;
                            bm.Clickable = true;
                            bm.Title = Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["date"]).ToShortTimeString());
                            bm.InfoWindowAnchor = new GooglePoint(0, 0);
                            scMarker.Add(bm);
                        }

                    }

                    if (i != ds.Tables[0].Rows.Count - 1)
                    {
                       
                        double latdistance = Distance(ds.Tables[0].Rows[i]["Latitude"].ToString(), ds.Tables[0].Rows[i]["Longitude"].ToString(), ds.Tables[0].Rows[i + 1]["Latitude"].ToString(), ds.Tables[0].Rows[i + 1]["Longitude"].ToString());
                        distance += latdistance;
                        if (Convert.ToInt32(ds.Tables[0].Rows[0]["MAPID"].ToString()) == 0 || distance >= 2.5)//0.05
                        {
                            distance = 0;
                            double p1 = Convert.ToDouble(ds.Tables[0].Rows[i]["Latitude"]);
                            double p2 = Convert.ToDouble(ds.Tables[0].Rows[i]["Longitude"]);
                            double p3 = Convert.ToDouble(ds.Tables[0].Rows[i + 1]["Latitude"]);
                            double p4 = Convert.ToDouble(ds.Tables[0].Rows[i + 1]["Longitude"]);
                            GoogleLocation p5 = new GoogleLocation((p1 + p3) / 2, (p2 + p4) / 2);

                            double dir = genFunction.bearing(p1, p2, p3, p4);
                            // == round it to a multiple of 3 and cast out 120s
                            dir = Math.Round(dir / 3) * 3;
                            while (dir >= 120) { dir -= 120; }

                            info = Convert.ToString(Convert.ToDateTime(ds.Tables[0].Rows[i]["date"]).ToShortTimeString());

                            GoogleMarker bmArrow = new GoogleMarker();
                            bmArrow.Latitude = p5.Latitude;
                            bmArrow.Longitude = p5.Longitude;
                            bmArrow.Clickable = true;
                           // bmArrow.IconUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["IconDirUrl"].Trim() + "/intl/en_ALL/mapfiles/dir_" + dir + ".png";
                            bmArrow.IconUrl = "http://www.google.com/intl/en_ALL/mapfiles/dir_" + dir + ".png";
                            bmArrow.IconSize = gsArrow;
                            bmArrow.IconAnchor = new GooglePoint(8, 8);
                            bmArrow.Title = info;
                            bmArrow.Text = info;
                            scMarker.Add(bmArrow);
                        }
                    }

                    GoogleLocation gl = new GoogleLocation();
                    gl.Latitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Latitude"]);
                    gl.Longitude = Convert.ToDouble(ds.Tables[0].Rows[i]["Longitude"]);
                    pl.Points.Add(gl);

                }
                GoogleMap1.Markers.AddRange(scMarker);
            }
            else
            {

                GoogleMap1.Zoom = 4;
                GoogleMap1.Latitude = Latitude;
                GoogleMap1.Longitude = Longitude;

            }

            #endregion


            #region Populate data for open tickets

            if (chkAssignedCall.Checked == true)
            {
                DataSet dsOpenTicket = new DataSet();
                dsOpenTicket = objBL_MapData.GetOpenTicket(objpropMapData);

                if (dsOpenTicket.Tables[0].Rows.Count > 0)
                {
                    StateCollection<GoogleDirection> scDirection = new StateCollection<GoogleDirection>();
                    if (locate == 0)
                        GoogleMap1.Address = dsOpenTicket.Tables[0].Rows[0]["address"].ToString();

                    for (int i = 0; i < dsOpenTicket.Tables[0].Rows.Count; i++)
                    {
                        string add = dsOpenTicket.Tables[0].Rows[i]["address"].ToString();
                        string assign = dsOpenTicket.Tables[0].Rows[i]["assigned"].ToString();
                        string ldesc = dsOpenTicket.Tables[0].Rows[i]["ldesc1"].ToString();
                        string edate = dsOpenTicket.Tables[0].Rows[i]["edate"].ToString();
                        string TicketID = dsOpenTicket.Tables[0].Rows[i]["id"].ToString();
                        string Cat = dsOpenTicket.Tables[0].Rows[i]["cat"].ToString();
                        string IconStr = "imagehandler.ashx?catid=" + Cat;

                        //GeoJsonData g =genFunction.GeoRequest(add);

                        //if (g.results.Length > 0)
                        //{
                        //    var p = g.results[0].geometry.location;
                        //    double latitude = p.lat;
                        //    double longitude = p.lng;

                        //    GoogleLocation gl = new GoogleLocation();
                        //    gl.Latitude = latitude;
                        //    gl.Longitude = longitude;

                        //    pl3.Points.Add(gl);
                        //}

                        GoogleMarker bm = new GoogleMarker();
                        bm.Address = add;
                        bm.Clickable = true;
                        bm.Title = ldesc;
                        bm.Text = "<strong> Ticket #: " + TicketID + "</strong></BR></BR><img src='" + IconStr + "' /> Category:" + Cat + " </BR></BR> <strong>" + ldesc + "</strong></BR></BR> " + add + " </BR></BR>" + Convert.ToDateTime(edate).ToShortTimeString();
                        if (assign == "1")
                        {
                            bm.IconUrl = "~/images/white.png";
                            bm.IconSize = gsLoc;
                        }
                        else if (assign == "2")
                        {
                            bm.IconUrl = "~/images/green.png";
                            bm.IconSize = gsLoc;
                        }
                        else if (assign == "3")
                        {
                            bm.IconUrl = "~/images/orange.png";
                            bm.IconSize = gsLoc;
                        }
                        else if (assign == "4")
                        {
                            bm.IconUrl = "~/images/blue.png";
                            bm.IconSize = gsLoc;
                        }
                        else if (assign == "5")
                        {
                            bm.IconUrl = "~/images/yellow.png";
                            bm.IconSize = gsLoc;
                        }
                        GoogleMap1.Markers.Add(bm);

                        GoogleDirection d = new GoogleDirection();
                        d.RoutePanelId = "ctl00_ContentPlaceHolder1_route";

                        if (i != 0)
                        {
                            d.Query = OpenAddress + " to " + add;
                            if (chkEffective.Checked == true)
                            {
                                scDirection.Add(d);
                            }
                        }

                        OpenAddress = add;
                    }

                    GoogleMap1.Directions.AddRange(scDirection);
                }
            }

            #endregion


            scPolyline.Add(pl);
            //scPolyline.Add(pl3);

            GoogleMap1.Polylines.AddRange(scPolyline);

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrMain", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

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
        //gvOpenCalls.PageIndex = e.NewPageIndex;
        //DataTable dt = new DataTable();
        //dt = PageSortData();
        //gvOpenCalls.DataSource = dt;
        //gvOpenCalls.DataBind();
    }

    private DataTable PageSortData()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["opencalldata"];
        return dt;
    }

    protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
    {
        //GridViewRow gvrPager = gvOpenCalls.BottomPagerRow;
        //DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");

        //gvOpenCalls.PageIndex = ddlPages.SelectedIndex;

        //// a method to populate your grid
        //FillGridPaged();
    }

    protected void Paginate(object sender, CommandEventArgs e)
    {
        //// get the current page selected
        //int intCurIndex = gvOpenCalls.PageIndex;

        //switch (e.CommandArgument.ToString().ToLower())
        //{
        //    case "first":
        //        gvOpenCalls.PageIndex = 0;
        //        break;
        //    case "prev":
        //        gvOpenCalls.PageIndex = intCurIndex - 1;
        //        break;
        //    case "next":
        //        gvOpenCalls.PageIndex = intCurIndex + 1;
        //        break;
        //    case "last":
        //        gvOpenCalls.PageIndex = gvOpenCalls.PageCount;
        //        break;
        //}

        //// popultate the gridview control
        //FillGridPaged();
    }

    private void FillGridPaged()
    {
        //DataTable dt = new DataTable();

        //dt = PageSortData();
        //gvOpenCalls.DataSource = dt;
        //gvOpenCalls.DataBind();
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
        //string sortExpression = e.SortExpression;

        //if (GridViewSortDirection == SortDirection.Ascending)
        //{
        //    GridViewSortDirection = SortDirection.Descending;
        //    SortGridView(sortExpression, DESCENDING);
        //}
        //else
        //{
        //    GridViewSortDirection = SortDirection.Ascending;
        //    SortGridView(sortExpression, ASCENDING);
        //}
    }

    private void SortGridView(string sortExpression, string direction)
    {
        //DataTable dt = PageSortData();

        //DataView dv = new DataView(dt);
        //dv.Sort = sortExpression + direction;

        //gvOpenCalls.DataSource = dv;
        //gvOpenCalls.DataBind();
    }

    //public SortDirection GridViewSortDirection
    //{
    //    //get
    //    //{
    //    //    if (ViewState["sortDirection"] == null)
    //    //        ViewState["sortDirection"] = SortDirection.Ascending;

    //    //    return (SortDirection)ViewState["sortDirection"];
    //    //}
    //    //set { ViewState["sortDirection"] = value; }
    //}
    #endregion

    //protected void chkOpencalls_CheckedChanged(object sender, EventArgs e)
    //{

    //}

    protected void btnchkOpencalls_Click(object sender, EventArgs e)
    {
        if (chkOpencalls.Checked == true)
        {
            if (ddlTech.SelectedIndex != 0)
                LoadMapWithGSPDataAndTicketData();
            else
                FillTechCurrentLocation();

            ShowOpenCallsOnMap();
            RadGridOpenCalls.Rebind();
            //If ShowOpenCallsOnMap checkbox checked
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

        #region code
        //foreach (DataRow dr in ds.Tables[0].Rows)
        //{
        //    if (dr["Latcom"] != DBNull.Value)
        //    {
        //        //GeoLocation geol = new GeoLocation();
        //        //geol.Latitude = Convert.ToDouble(dr["Latcom"]);
        //        //geol.Longitude = Convert.ToDouble(dr["Loncom"]);
        //        //GeoRequest request = new GeoRequest(geol);
        //        //GeoResponse response = request.GetResponse();
        //        //string GPSaddress = response.Results[0].FormattedAddress;
        //        GeoJsonData g = GeoRequest(dr["Latcom"].ToString(), dr["Loncom"].ToString());
        //        if (g.results.Count() > 0)
        //        {
        //            string GPSaddress = g.results[0].formatted_address;
        //            dr["AddressCM"] = GPSaddress;
        //        }
        //        else
        //        {
        //            dr["AddressCM"] = "Position not available";
        //        }
        //    }
        //    else
        //    {
        //        dr["AddressCM"] = "Position not available";
        //    }

        //    if (dr["Latenr"] != DBNull.Value)
        //    {
        //        //GeoLocation geol = new GeoLocation();
        //        //geol.Latitude = Convert.ToDouble(dr["Latenr"]);
        //        //geol.Longitude = Convert.ToDouble(dr["Lonenr"]);
        //        //GeoRequest request = new GeoRequest(geol);
        //        //GeoResponse response = request.GetResponse();
        //        //string GPSaddress = response.Results[0].FormattedAddress;
        //        GeoJsonData g = GeoRequest(dr["Latenr"].ToString(), dr["Lonenr"].ToString());

        //        if (g.results.Count() > 0)
        //        {
        //            string GPSaddress = g.results[0].formatted_address;
        //            dr["AddressER"] = GPSaddress;
        //        }
        //        else
        //        {
        //            dr["AddressER"] = "Position not available";
        //        }
        //    }
        //    else
        //    {
        //        dr["AddressER"] = "Position not available";
        //    }

        //    if (dr["Latsite"] != DBNull.Value)
        //    {
        //        //GeoLocation geol = new GeoLocation();
        //        //geol.Latitude = Convert.ToDouble(dr["Latsite"]);
        //        //geol.Longitude = Convert.ToDouble(dr["Lonsite"]);
        //        //GeoRequest request = new GeoRequest(geol);
        //        //GeoResponse response = request.GetResponse();
        //        //string GPSaddress = response.Results[0].FormattedAddress;
        //        GeoJsonData g = GeoRequest(dr["Latsite"].ToString(), dr["Lonsite"].ToString());

        //        if (g.results.Count() > 0)
        //        {
        //            string GPSaddress = g.results[0].formatted_address;
        //            dr["AddressOS"] = GPSaddress;
        //        }
        //        else
        //        {
        //            dr["AddressOS"] = "Position not available";
        //        }
        //    }
        //    else
        //    {
        //        dr["AddressOS"] = "Position not available";
        //    }
        //}

        #endregion

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
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    Label lbldistENOS = (Label)e.Row.FindControl("lbldistENOS");
        //    Label lbldistCOER = (Label)e.Row.FindControl("lbldistCOER");
        //    Label lbldisttot = (Label)e.Row.FindControl("lbldisttot");

        //    if (lbldistENOS.Text == "") { lbldistENOS.Text = "0"; }
        //    if (lbldistCOER.Text == "") { lbldistCOER.Text = "0"; }
        //    if (lbldisttot.Text == "") { lbldisttot.Text = "0"; }

        //    lbldisttot.Text = (Convert.ToDouble(lbldistCOER.Text) + Convert.ToDouble(lbldistENOS.Text)).ToString();

        //    toten = toten + Convert.ToDouble(lbldistENOS.Text);
        //    totco = totco + Convert.ToDouble(lbldistCOER.Text);
        //    tottot = tottot + Convert.ToDouble(lbldisttot.Text);
        //}
        //if (e.Row.RowType == DataControlRowType.Footer)
        //{
        //    Label lbldistENOSfooter = (Label)e.Row.FindControl("lbldistENOSfooter");
        //    Label lbldistCOERfooter = (Label)e.Row.FindControl("lbldistCOERfooter");
        //    Label lbldisttotfooter = (Label)e.Row.FindControl("lbldisttotfooter");

        //    lbldistENOSfooter.Text = toten.ToString();
        //    lbldistCOERfooter.Text = totco.ToString();
        //    lbldisttotfooter.Text = tottot.ToString();
        //}
    }

    #region Paging TimeStmpGrid

    protected void gvTimeStmp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //gvTimeStmp.PageIndex = e.NewPageIndex;
        //DataTable dt = new DataTable();
        //dt = PageSortDataTime();
        //gvTimeStmp.DataSource = dt;
        //gvTimeStmp.DataBind();
    }

    private DataTable PageSortDataTime()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["timedata"];
        return dt;
    }

    private void FillGridPagedTime()
    {
        //DataTable dt = new DataTable();

        //dt = PageSortDataTime();
        //gvTimeStmp.DataSource = dt;
        //gvTimeStmp.DataBind();

    }

    protected void ddlPagesTime_SelectedIndexChanged(Object sender, EventArgs e)
    {
        //GridViewRow gvrPager = gvTimeStmp.BottomPagerRow;
        //DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");

        //gvTimeStmp.PageIndex = ddlPages.SelectedIndex;

        //// a method to populate your grid
        //FillGridPagedTime();
    }

    protected void PaginateTime(object sender, CommandEventArgs e)
    {
        //// get the current page selected
        //int intCurIndex = gvTimeStmp.PageIndex;

        //switch (e.CommandArgument.ToString().ToLower())
        //{
        //    case "first":
        //        gvTimeStmp.PageIndex = 0;
        //        break;
        //    case "prev":
        //        gvTimeStmp.PageIndex = intCurIndex - 1;
        //        break;
        //    case "next":
        //        gvTimeStmp.PageIndex = intCurIndex + 1;
        //        break;
        //    case "last":
        //        gvTimeStmp.PageIndex = gvTimeStmp.PageCount;
        //        break;
        //}

        //// popultate the gridview control
        //FillGridPagedTime();
    }

    protected void gvTimeStmp_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        PaginateTime(sender, e);
    }

    protected void gvTimeStmp_DataBound(object sender, EventArgs e)
    {
        //GridViewRow gvrPager = gvTimeStmp.BottomPagerRow;

        //if (gvrPager == null) return;

        //// get your controls from the gridview
        //DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        //Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        //if (ddlPages != null)
        //{
        //    // populate pager
        //    for (int i = 0; i < gvTimeStmp.PageCount; i++)
        //    {

        //        int intPageNumber = i + 1;
        //        ListItem lstItem = new ListItem(intPageNumber.ToString());

        //        if (i == gvTimeStmp.PageIndex)
        //            lstItem.Selected = true;

        //        ddlPages.Items.Add(lstItem);
        //    }
        //}

        //// populate page count
        //if (lblPageCount != null)
        //    lblPageCount.Text = gvTimeStmp.PageCount.ToString();
    }

    protected void gvTimeStmp_Sorting(object sender, GridViewSortEventArgs e)
    {
        //string sortExpression = e.SortExpression;

        //if (GridViewSortDirection == SortDirection.Ascending)
        //{
        //    GridViewSortDirection = SortDirection.Descending;
        //    SortGridViewTime(sortExpression, DESCENDING);
        //}
        //else
        //{
        //    GridViewSortDirection = SortDirection.Ascending;
        //    SortGridViewTime(sortExpression, ASCENDING);
        //}
    }

    private void SortGridViewTime(string sortExpression, string direction)
    {
        //DataTable dt = PageSortDataTime();

        //DataView dv = new DataView(dt);
        //dv.Sort = sortExpression + direction;

        //gvTimeStmp.DataSource = dv;
        //gvTimeStmp.DataBind();
    }

    #endregion

    #endregion

    #region Find Nearest Tech

    protected void btnNearest_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGridOpenCalls.Items)
        {
            RadCheckBox chkSelected = (RadCheckBox)di.FindControl("chkSelect");
            Label lblAddress = (Label)di.FindControl("lbladdress");
            Label lblLat = (Label)di.FindControl("lblLat");
            Label lblLng = (Label)di.FindControl("lblLng");

            if (chkSelected.Checked == true)
            {
                GetNearestWorker(lblAddress.Text, lblLat.Text.Trim(), lblLng.Text.Trim());
            }
        }
    }

    private void GetNearestWorker(string add)
    {
        GoogleMap1.Polylines.Clear();
        GoogleMap1.Markers.Clear();
        GoogleMap1.Directions.Clear();

        //GeoRequest request = new GeoRequest(add);
        //GeoResponse response = request.GetResponse();
        //GeoLocation location = response.Results[0].Geometry.Location;
        string mapsAPIKey = System.Web.Configuration.WebConfigurationManager.AppSettings["MapsAPIKey"].Trim();
        try
        {

            GeoJsonData g = genFunction.GeoRequest(add, mapsAPIKey);

            if (g.results.Length > 0)
            {
                var p = g.results[0].geometry.location;
                double latitude = p.lat;
                double longitude = p.lng;

                StateCollection<GoogleMarker> scMarker = new StateCollection<GoogleMarker>();
                //DateTime Date = Convert.ToDateTime(txtDate.Text);
                DateTime d = System.DateTime.Now;//.ToUniversalTime()
                //DateTime stdt = Convert.ToDateTime("1/5/2012 08:01:00 AM");
                string connstr = Session["config"].ToString();

                DataSet ds = new DataSet();
                objpropMapData.Date = d;
                objpropMapData.ConnConfig = connstr;
                objpropMapData.Latitude = latitude;
                objpropMapData.Longitude = longitude;
                ds = objBL_MapData.GetNearWorkers(objpropMapData);
                //ds = SqlHelper.ExecuteDataset(connstr, "spGetNearWorkers", d , location.Latitude,location.Longitude);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //GeoLocation geol = new GeoLocation();
                    //geol.Latitude = Convert.ToDouble(dr["Latitude"]);
                    //geol.Longitude = Convert.ToDouble(dr["Longitude"]);
                    //GeoRequest requestrev = new GeoRequest(geol);
                    //GeoResponse responserev = requestrev.GetResponse();
                    //string GPSaddress = responserev.Results[0].FormattedAddress;
                    string GPSaddress = string.Empty;
                    GeoJsonData gadd = genFunction.GeoRequest(dr["Latitude"].ToString(), dr["Longitude"].ToString());
                    if (gadd.results.Count() > 0)
                    {
                        GPSaddress = gadd.results[0].formatted_address;
                    }
                    GoogleMarker bm = new GoogleMarker();
                    bm.Latitude = Convert.ToDouble(dr["Latitude"]);
                    bm.Longitude = Convert.ToDouble(dr["Longitude"]);
                    bm.Clickable = true;
                    bm.Text = dr["emp"].ToString() + " </BR></BR>" + Convert.ToDateTime(dr["date"]).ToShortTimeString() + " </BR></BR>" + GPSaddress;//.ToUniversalTime()

                    scMarker.Add(bm);
                }

                GoogleMap1.Markers.AddRange(scMarker);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrNear", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    private void GetNearestWorker(string add, string lat, string lng)
    {
        GoogleMap1.Polylines.Clear();
        GoogleMap1.Markers.Clear();
        GoogleMap1.Directions.Clear();

        try
        {
            if (lat == string.Empty || lng == string.Empty)
            {
                string mapsAPIKey = System.Web.Configuration.WebConfigurationManager.AppSettings["MapsAPIKey"].Trim();

                GeoJsonData g = genFunction.GeoRequest(add, mapsAPIKey);

                if (g.results.Length > 0)
                {
                    var p = g.results[0].geometry.location;
                    lat = p.lat.ToString();
                    lat = p.lng.ToString();
                }
            }
            StateCollection<GoogleMarker> scMarker = new StateCollection<GoogleMarker>();

            if (lat != string.Empty && lng != string.Empty)
            {
                DataSet ds = new DataSet();
                objpropMapData.ConnConfig = Session["config"].ToString();
                objpropMapData.Lat = lat;
                objpropMapData.Lng = lng;
                objpropMapData.Worker = string.Empty;
                ds = objBL_MapData.GetNearWorkersByTime(objpropMapData);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string address = ds.Tables[0].Rows[0]["address"].ToString();
                    if (address == string.Empty)
                    {
                        GeoJsonData gadd = genFunction.GeoRequest(dr["Latitude"].ToString(), dr["Longitude"].ToString());
                        if (gadd.results.Count() > 0)
                        {
                            address = gadd.results[0].formatted_address;
                        }
                    }
                    GoogleMarker bm = new GoogleMarker();
                    bm.Latitude = Convert.ToDouble(dr["Latitude"]);
                    bm.Longitude = Convert.ToDouble(dr["Longitude"]);
                    bm.Clickable = true;
                    bm.Text = dr["worker"].ToString() + " </BR></BR>" + dr["time"].ToString() + " </BR></BR>" + address;

                    scMarker.Add(bm);
                }

                GoogleSize gsLoc = new GoogleSize();
                gsLoc.Height = 50;
                gsLoc.Width = 30;
                GoogleMarker bmLoc = new GoogleMarker();
                bmLoc.Latitude = Convert.ToDouble(lat);
                bmLoc.Longitude = Convert.ToDouble(lng);
                bmLoc.Clickable = true;
                bmLoc.Text = add;
                bmLoc.IconUrl = "~/images/blue.png";
                bmLoc.IconSize = gsLoc;
                scMarker.Add(bmLoc);

                GoogleMap1.Latitude = Convert.ToDouble(lat);
                GoogleMap1.Longitude = Convert.ToDouble(lng);
                GoogleMap1.Markers.AddRange(scMarker);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrNear", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #endregion

    #region WebMethod
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string getpingResponse(string Randomid, string deviceID, string DeviceType)
    {
        BL_General objBL_General = new BL_General();
        General objGeneral = new General();

        //objGeneral.DeviceID = "66c441f9-cb2c-3cce-b132-1531ad3088f9"; 
        //objGeneral.DeviceID = "b2aa1c6d-fb69-37c8-9257-65882afb83d5";
        objGeneral.DeviceID = deviceID;
        objGeneral.RegID = Randomid;
        DataSet dspingResponse = objBL_General.getPing(objGeneral);

        string strResponse = "0";
        string strRunning = "";
        string strGPS = "";
        if (dspingResponse.Tables[0].Rows.Count > 0)
        {
            if (DeviceType == "iOS")
            {
                strRunning = "Location Service : OFF";
                if (dspingResponse.Tables[0].Rows[0]["IsGPSEnabled"].ToString() == "1")
                {
                    strRunning = "Location Service : ON";
                }

                string strLocationAccess = "Location Access : Never";

                if (dspingResponse.Tables[0].Rows[0]["isrunning"].ToString() == "0")
                {
                    strLocationAccess = "Location Access : User has not yet made a choice with regards to this application";
                }
                else if (dspingResponse.Tables[0].Rows[0]["isrunning"].ToString() == "1")
                {
                    strLocationAccess = "Location Access : This application is not authorized to use location services.";
                }
                else if (dspingResponse.Tables[0].Rows[0]["isrunning"].ToString() == "2")
                {
                    strLocationAccess = "Location Access : User has explicitly denied authorization for this application, or location services are disabled in Settings.";
                }
                else if (dspingResponse.Tables[0].Rows[0]["isrunning"].ToString() == "3")
                {
                    strLocationAccess = "Location Access : Always";
                }
                else if (dspingResponse.Tables[0].Rows[0]["isrunning"].ToString() == "4")
                {
                    strLocationAccess = "Location Access : User has granted authorization to use their location only when your app is visible to them";
                }
                else if (dspingResponse.Tables[0].Rows[0]["isrunning"].ToString() == "5")
                {
                    strLocationAccess = "Location Access : User has authorized this application to use location services.";
                }


                string strBackgroundRefresh = "Background Refresh : NA";
                if (dspingResponse.Tables[0].Rows[0]["backgroundRefresh"].ToString() == "0")
                {
                    strBackgroundRefresh = "Background Refresh : OFF";
                }
                else if (dspingResponse.Tables[0].Rows[0]["backgroundRefresh"].ToString() == "1")
                {
                    strBackgroundRefresh = "Background Refresh : ON";
                }

                strResponse = strRunning + "</BR>" + strLocationAccess + "</BR>" + strBackgroundRefresh;
            }
            else
            {
                strRunning = "GPS Tracking : OFF";
                if (dspingResponse.Tables[0].Rows[0]["isrunning"].ToString() == "1")
                {
                    strRunning = "GPS Tracking : ON";
                }

                strGPS = "GPS : OFF";
                if (dspingResponse.Tables[0].Rows[0]["IsGPSEnabled"].ToString() == "1")
                {
                    strGPS = "GPS : ON";
                }

                strResponse = strRunning + "</BR>" + strGPS;
            }
        }

        return strResponse;
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string SendPushNoti(string Username)
    {
        JavaScriptSerializer js = new JavaScriptSerializer();
        string strResp = string.Empty;
        string[] strResponse = new string[4];
        BL_General objBL_General = new BL_General();
        General objGeneral = new General();
        BL_User objBL_User = new BL_User();
        User objProp_User = new User();
        GeneralFunctions genFunction = new GeneralFunctions();
        AndroidPushNotification theAndroidPn = new AndroidPushNotification();
        IOSPushNotification theIOSPn = new IOSPushNotification();
        objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objProp_User.Username = Username;
        objGeneral.DeviceID = objBL_User.getUserDeviceID(objProp_User);
        strResponse[0] = objGeneral.DeviceID;

        string tokenID = objBL_General.GetDeviceTokenID(objGeneral);

        string DeviceType = "";

        DeviceType = objBL_General.GetDeviceType(objGeneral);

        string strRandom = genFunction.generateRandomString(10);
        strResponse[1] = strRandom;

        if (tokenID != "")
        {

            if (DeviceType == "iOS")
            {
                string message = "Requesting your location..";
                string CertificatePath = (WebConfigurationManager.AppSettings["IOSPushNPath"].Trim());
                string notificationType = "1";
                string sResponseFromServer = "";
                String Hostname = WebConfigurationManager.AppSettings["IOSPushNHostName"].Trim();
                sResponseFromServer = theIOSPn.PushToiPhone(tokenID, message, CertificatePath, notificationType, Hostname, strRandom);

                if (sResponseFromServer.Contains("Error") == false)
                {
                    strResponse[2] = "1";
                }
                else
                {
                    strResponse[2] = "Error pinging the device : " + sResponseFromServer;
                }
            }

            else
            {
                DeviceType = "Android";
                string sResponseFromServer = "";
                sResponseFromServer = theAndroidPn.PushToAndroid(WebConfigurationManager.AppSettings["AndroidPNServerUrl"].Trim(),
                                                 strRandom,
                                                  WebConfigurationManager.AppSettings["GoogleAppID"].Trim(),
                                                  WebConfigurationManager.AppSettings["SENDER_ID"].Trim(),
                                                  DateTime.UtcNow.ToString(),
                                                  tokenID);

                if (sResponseFromServer.Contains("Error") == false)
                {
                    strResponse[2] = "1";
                }
                else
                {
                    strResponse[2] = "Error pinging the device : " + sResponseFromServer;
                }

            }
        }
        else
        {
            strResponse[2] = "Device not registered for ping.";

        }
        strResponse[3] = DeviceType;
        strResp = js.Serialize(strResponse);

        return strResp;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string getlocationAddress(string Tech, string Date, string Time, string TicketID, string timestamp)
    {
        string strResponse = "Not Available";
        BL_MapData objBL_MapData = new BL_MapData();
        MapData objpropMapData = new MapData();

        GeneralFunctions genFunction = new GeneralFunctions();

        objpropMapData.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objpropMapData.Tech = Tech;
        objpropMapData.Date = Convert.ToDateTime(Date);
        objpropMapData.CallDate = Convert.ToDateTime(Time);
        objpropMapData.TicketID = Convert.ToInt32(TicketID);
        objpropMapData.TempId = timestamp;

        DataSet dsLoc = objBL_MapData.getlocationAddress(objpropMapData);

        if (dsLoc.Tables[0].Rows.Count > 0)
        {
            GeoJsonData g = genFunction.GeoRequest(dsLoc.Tables[0].Rows[0]["latitude"].ToString(), dsLoc.Tables[0].Rows[0]["longitude"].ToString());

            if (g.results.Count() > 0)
            {
                strResponse = g.results[0].formatted_address;
            }
            else
            {
                strResponse = "Not Available";
            }
        }

        return strResponse;
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
