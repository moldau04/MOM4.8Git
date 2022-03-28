using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Web.Script.Serialization;
using BusinessEntity;
using BusinessLayer;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using System.Web;

public class Location
{
    public int locid { get; set; }
    public int workerid { get; set; }
    public string workername { get; set; }
    public int polid { get; set; }
}

public class LocAssignedWorker
{
    public List<Location> locations { get; set; }
}

public partial class RouteBuilder : System.Web.UI.Page
{
    Customer objCustomer = new Customer();

    BL_Customer objBL_Customer = new BL_Customer();

    User objPropUser = new User();

    BL_User objBL_User = new BL_User();

    GeneralFunctions objGeneralFunctions = new GeneralFunctions();
    Random randomGen = new Random();

    public char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    protected void Page_Load(object sender, EventArgs e)
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
            //userpermissions();
            lnkUpdateLocs.Visible = Session["MSM"].ToString() == "TS" ? false : true;
            btnAssignWorkerMarker.Visible = Session["MSM"].ToString() == "TS" ? false : true;
            Session["polydata"] = null;
            GetRouteTemplate();
            FillRoute();
            ViewState["CreateMarker"] = true;
            btnLocationChange.Attributes["class"] = "active";
            SetDefaultWorker();
        }

        Permission();
        CompanyPermission();

        HighlightSideMenu("schMgr", "lnkRouteBuilder", "schdMgrSub");

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


        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("Home.aspx?permission=no"); return;
        }
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = GetUserById();

            /// Ticket ///////////////////------->
            string MTimesheetPermission = ds.Rows[0]["RouteBuilder"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["RouteBuilder"].ToString();
            string view = MTimesheetPermission.Length < 4 ? "Y" : MTimesheetPermission.Substring(3, 1);
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
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.Username = Session["username"].ToString();
                objPropUser.PageName = "routebuilder.aspx";
                DataSet dspage = objBL_User.getScreensByUser(objPropUser);
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
        if (Session["COPer"].ToString() == "1")
        {
            gvLocations.Columns.FindByUniqueName("Company").Visible = true;
        }
        else
        {
            gvLocations.Columns.FindByUniqueName("Company").Visible = false;
        }
    }
    protected void Page_PreRender(Object o, EventArgs e)
    {


        foreach (GridDataItem gr in gvLocChanges.Items)
        {
            HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");
            gr.Attributes["onclick"] = "GridHover(" + hdnID.Value + ")";
        }
    }

    protected void ddlRoute_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtSearchLoc.Text = string.Empty;
        chkNulls.Checked = false;
        ViewState["CreateMarker"] = false;
        gvLocations.Rebind();
    }

    protected void lnkDeleteRoute_Click(object sender, EventArgs e)
    {

        objCustomer.ConnConfig = Session["config"].ToString();
        objCustomer.Mode = 2;
        objCustomer.TemplateID = Convert.ToInt32(ddlTemplates.SelectedValue);
        try
        {
            objBL_Customer.AddRouteTemplate(objCustomer);
            GetRouteTemplate();
            ddlTemplates.SelectedValue = "0";
            txtTemplate.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            gvLocChanges.Rebind();

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "noty({text: 'Template Deleted Successfully!',  type : 'success', dismissQueue: true, layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void btnAssign_Click(object sender, EventArgs e)
    {
        AssignWorker(0, null);
        ModalPopupExtender1.Hide();
        hdnEdited.Value = "1";
        //lnkSaveTemplate.ImageUrl = "images/saveicon.png";
        lnkSaveTemplate.Enabled = true;
        chkMap.Checked = false;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "clearOverlays(GooglemarkersArrayAssigned);", true);
    }

    protected void ddlTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTemplates.SelectedValue != "0" && ddlTemplates.SelectedValue != "-1")
        {
            GetTemplate();
            hdnEdited.Value = "0";
            //lnkSaveTemplate.ImageUrl = "images/saveiconblack.png";
            lnkSaveTemplate.Enabled = false;
        }
        else
        {
            txtTemplate.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            hdnEdited.Value = "1";
            hdnAssignedWorker.Value = string.Empty;
            ViewState["gvLocChanges"] = null;
            gvLocChanges.Rebind();
            gvWorkerChanges.DataSource = string.Empty;
            gvWorkerChanges.DataBind();
            //lnkSaveTemplate.ImageUrl = "images/saveicon.png";
            lnkSaveTemplate.Enabled = true;

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "ClearPolygons();", true);


        }

        chkMap.Checked = false;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "clearOverlays(GooglemarkersArrayAssigned);", true);
    }

    protected void lnkUpdateLocs_Click(object sender, EventArgs e)
    {
        UpdateLocs();
    }

    private void UpdateLocs()
    {
        DataTable dtLocChange = new DataTable();
        dtLocChange.Columns.Add("TemplateID", typeof(int));
        dtLocChange.Columns.Add("Loc", typeof(int));
        dtLocChange.Columns.Add("Worker", typeof(int));
        dtLocChange.Columns.Add("polyid", typeof(int));
        dtLocChange.Columns.Add("WorkerName", typeof(string));

        foreach (GridDataItem gr in gvLocChanges.Items)
        {
            HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");
            HiddenField hdnWorker = (HiddenField)gr.FindControl("hdnWorker");
            HiddenField hdnPolyid = (HiddenField)gr.FindControl("hdnPolyid");
            Label lblCurWorker = (Label)gr.FindControl("lblCurWorker");
            Label lblNewWorker = (Label)gr.FindControl("lblNewWorker");
            DataRow dr = dtLocChange.NewRow();

            dr["TemplateID"] = Convert.ToInt32(ddlTemplates.SelectedValue);
            dr["Loc"] = Convert.ToInt32(hdnID.Value);
            dr["Worker"] = Convert.ToInt32(hdnWorker.Value);
            dr["polyid"] = Convert.ToInt32(hdnPolyid.Value);
            dr["WorkerName"] = lblNewWorker.Text;

            dtLocChange.Rows.Add(dr);
        }



        if (dtLocChange.Rows.Count > 0)
        {
            DataTable dtloc = dtLocChange.Copy();
            dtLocChange.Columns.Remove("polyid");
            dtLocChange.Columns.Remove("WorkerName");

            objCustomer.ConnConfig = Session["config"].ToString();
            objCustomer.DtTemplateData = dtLocChange;
            objBL_Customer.UpdateLocRoute(objCustomer);

            txtSearchLoc.Text = string.Empty;
            chkNulls.Checked = false;
            ddlWorker.SelectedIndex = -1;
            ViewState["CreateMarker"] = true;
            gvLocations.Rebind();
            gvWorkers.Rebind();
            AssignWorker(2, dtloc);
            chkMap.Checked = false;
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "noty({text: 'Locations Updated Successfully!',  type : 'success', dismissQueue: true, layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false}); AddAllMarkers();", true);

        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAlertUpdateLoc", "alert('There are no locations to update.')", true);
        }
    }

    private void Clear()
    {
        txtSearchLoc.Text = string.Empty;
        chkNulls.Checked = false;
        ddlWorker.SelectedIndex = -1;
        ViewState["CreateMarker"] = false;
        gvLocations.Rebind();
    }

    private void GetRouteTemplate()
    {
        objCustomer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Customer.getRouteTemplate(objCustomer);

        ddlTemplates.DataSource = ds.Tables[0];
        ddlTemplates.DataTextField = "Name";
        ddlTemplates.DataValueField = "templateID";
        ddlTemplates.DataBind();

        ddlTemplates.Items.Insert(0, new ListItem("- Add New -", "0"));
    }

    private void GetTemplate()
    {
        objCustomer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        if (Session["MSM"].ToString() == "TS")
        {
            objCustomer.Status = 1;
        }
        else
        {
            objCustomer.Status = 0;
        }
        objCustomer.TemplateID = Convert.ToInt32(ddlTemplates.SelectedValue);
        ds = objBL_Customer.getTemplateByID(objCustomer);
        var dt = ds.Tables[0];
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtTemplate.Text = ds.Tables[0].Rows[0]["name"].ToString();
            txtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
            hdnOverlay.Value = ds.Tables[0].Rows[0]["overlay"].ToString();

            if (hdnOverlay.Value.ToLower() == "polygon")
            {
                hdnPolygon.Value = ds.Tables[0].Rows[0]["polygoncoord"].ToString();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPoly", "AddPolygonFromArray();", true);
            }
        }
    }

    private void BindMarkers(DataTable dt, bool assignedOnly)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        foreach (DataRow dr in dt.Rows)
        {
            if (dr["lat"] != DBNull.Value && dr["lat"].ToString().Trim() != string.Empty)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    if (col.ColumnName == "loc" || col.ColumnName == "title" || col.ColumnName == "lat" || col.ColumnName == "lng" || col.ColumnName == "description" || col.ColumnName == "worker" || col.ColumnName == "assdwrkr")
                        row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
        }
        string serialised = serializer.Serialize(rows);
        if (!assignedOnly)
            hdnMarkers.Value = serialised;
        else
            hdnAssignedMarkers.Value = serialised;
    }

    private void AssignWorkerColor(DataTable dtLocation)
    {
        //List<string> lst = new List<string>();
        //foreach (DataRow dr in dtLocation.Rows)
        //{
        //    lst.Add(dr["name"].ToString());
        //}

        //var workers =
        //       (from worker in lst
        //        select worker).Distinct().ToList();

        //WorkerColor(workers);

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        foreach (DataRow dr in dtLocation.Rows)
        {
            row = new Dictionary<string, object>();

            row.Add("worker", dr["name"].ToString());
            row.Add("color", dr["Color"].ToString());// RandomColor());

            rows.Add(row);
        }
        hdnColor.Value = serializer.Serialize(rows);
        ViewState["workercol"] = rows;
    }



    public string getWorkerColor(object worker)
    {

        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>((List<Dictionary<string, object>>)ViewState["workercol"]);
        string color = string.Empty;
        foreach (Dictionary<string, object> dict in rows)
        {
            if (dict["worker"].ToString().Trim().ToLower() == worker.ToString().Trim().ToLower())
            {
                color = dict["color"].ToString();
            }
        }

        string url = "http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=|" + color + "|ffffff";
        return url;
    }

    private string RandomColor()
    {
        string color = String.Format("{0:X6}", randomGen.Next(0x1000000));
        return color;
    }

    private void FillRoute()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getRoute(objPropUser, 1);

        ddlWorker.DataSource = ds.Tables[0];
        ddlWorker.DataTextField = "Name";
        ddlWorker.DataValueField = "ID";
        ddlWorker.DataBind();
        ddlWorker.Items.Insert(0, new ListItem("- All -", "-1"));
        ddlWorker.Items.Insert(1, new ListItem("Unassigned", "0"));
        lstWorker.DataSource = ds.Tables[0];
        lstWorker.DataTextField = "Name";
        lstWorker.DataValueField = "ID";
        lstWorker.DataBind();

        lstWorkerAssignMarker.DataSource = ds.Tables[0];
        lstWorkerAssignMarker.DataTextField = "Name";
        lstWorkerAssignMarker.DataValueField = "ID";
        lstWorkerAssignMarker.DataBind();

        if (!IsPostBack)
        {
            AssignWorkerColor(ds.Tables[0]);
        }
    }

    private void AssignWorker(int IsTemplate, DataTable dtloc)
    {
        switch (hdnOverlay.Value.ToLower())
        {
            case "circle":
                AssignWorkerCircle();
                break;
            case "polygon":
                AssignPolygonWorkers(IsTemplate, dtloc);
                break;
        }
    }

    private void AssignWorkerCircle()
    {


        objCustomer.ConnConfig = Session["config"].ToString();
        objCustomer.SearchValue = string.Empty;
        if (Session["MSM"].ToString() == "TS")
        {
            objCustomer.Status = 1;
        }
        else
        {
            objCustomer.Status = 0;
        }
        DataSet ds = new DataSet();


        #region Company Check
        objCustomer.UserID = Convert.ToInt32(Session["UserID"].ToString());

        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            objCustomer.EN = 1;
        }
        else
        {
            objCustomer.EN = 0;
        }
        #endregion

        var worker = int.Parse(ddlWorker.SelectedValue);

        objCustomer.Worker = worker;


        ds = objBL_Customer.getLocCoordinates(objCustomer);


        double lat1 = Convert.ToDouble(hdnCenter.Value.Split(',')[0]);

        double long1 = Convert.ToDouble(hdnCenter.Value.Split(',')[1]);

        double radius = Convert.ToDouble(hdnRadius.Value) / 1000;

        List<int> lstLoc = new List<int>();

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr["lat"] != DBNull.Value)
            {
                if (dr["lat"].ToString().Trim() != string.Empty)
                {
                    double lat2 = Convert.ToDouble(dr["lat"]);
                    double long2 = Convert.ToDouble(dr["lng"]);
                    double dist = Distance(lat1, long1, lat2, long2);
                    if (dist <= radius)
                    {
                        if (!lstLoc.Contains(Convert.ToInt32(dr["loc"])))
                            lstLoc.Add(Convert.ToInt32(dr["loc"]));
                    }
                }
            }
        }

        DataTable dtChanges = ds.Tables[0].Clone();

        foreach (int loc in lstLoc)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["loc"].ToString() == loc.ToString())
                {
                    dtChanges.ImportRow(dr);
                }
            }

        }

        ViewState["gvLocChanges"] = dtChanges;
        gvLocChanges.Rebind();


        BindMarkers(dtChanges, true);

        WorkerChanges(dtChanges);

    }

    private void AssignPolygonWorkers(int IsTemplate, DataTable dtloc)
    {

        objCustomer.ConnConfig = Session["config"].ToString();
        objCustomer.SearchValue = string.Empty;
        if (Session["MSM"].ToString() == "TS")
        {
            objCustomer.Status = 1;
        }
        else
        {
            objCustomer.Status = 0;
        }

        objCustomer.LocIDs = hdnLocsInPolygon.Value;

        if (IsTemplate == 2)
        {
            string strLocs = string.Empty;
            string[] arrLocs = new string[dtloc.Rows.Count];
            int count = 0;
            foreach (DataRow dr in dtloc.Rows)
            {
                arrLocs[count] = dr["Loc"].ToString();
                count++;
            }
            strLocs = string.Join(",", arrLocs);
            objCustomer.LocIDs = strLocs;
        }

        DataSet ds = new DataSet();


        #region Company Check

        objCustomer.UserID = Convert.ToInt32(Session["UserID"].ToString());

        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            objCustomer.EN = 1;
        }
        else
        {
            objCustomer.EN = 0;
        }
        #endregion

        var worker = int.Parse(ddlWorker.SelectedValue);

        objCustomer.Worker = worker;


        ds = objBL_Customer.getLocCoordinates(objCustomer);



        var dt = ds.Tables[0];

        dt.Columns.Add("assdwrkr", typeof(string));
        dt.Columns.Add("assdwrkrid", typeof(int));
        dt.Columns.Add("polyid", typeof(int));
        foreach (DataRow dr in dt.Rows)
        {
            if (IsTemplate == 1)
            {
                Session["polydata"] = null;
                if (hdnAssignedWorker.Value.Trim() != string.Empty)
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    LocAssignedWorker AssignedWorker = ser.Deserialize<LocAssignedWorker>(hdnAssignedWorker.Value.Trim());
                    if (AssignedWorker.locations.Count > 0)
                    {
                        foreach (Location loc in AssignedWorker.locations)
                        {
                            if (Convert.ToInt32(dr["loc"]) == loc.locid)
                            {
                                dr["assdwrkr"] = loc.workername;
                                dr["assdwrkrid"] = loc.workerid;
                                dr["polyid"] = loc.polid;
                            }
                        }
                    }
                }
            }
            else if (IsTemplate == 2)
            {
                Session["polydata"] = null;
                foreach (DataRow drloc in dtloc.Rows)
                {
                    if (Convert.ToInt32(dr["loc"]) == Convert.ToInt32(drloc["Loc"]))
                    {
                        dr["assdwrkr"] = drloc["WorkerName"];
                        dr["assdwrkrid"] = drloc["Worker"];
                        dr["polyid"] = drloc["polyid"];
                    }
                }
            }
            else
            {
                dr["assdwrkr"] = lstWorker.SelectedItem.Text;
                dr["assdwrkrid"] = Convert.ToInt32(lstWorker.SelectedValue);
                dr["polyid"] = Convert.ToInt32(hdnPolyID.Value);
            }
        }

        if (Session["polydata"] != null)
        {
            DataTable dtSession = (DataTable)Session["polydata"];
            DataView dv = new DataView(dtSession);
            dv.RowFilter = "polyid <> " + hdnPolyID.Value;

            dt.Merge(dv.ToTable());
        }

        Session["polydata"] = dt;
        ViewState["gvLocChanges"] = dt;
        gvLocChanges.Rebind();

        //CalculateTotal(dt, "TEMPLATE");

        BindMarkers(dt, true);

        WorkerChanges(dt);
    }

    private void WorkerChanges(DataTable dt)
    {
        try
        {
            if (dt.Rows.Count > 0)
            {
                #region Get workerarray
                List<string> workerlist = new List<string>();
                List<string> assdworkerlist = new List<string>();

                foreach (DataRow dr in dt.Rows)
                {

                    workerlist.Add(dr["worker"].ToString());
                    assdworkerlist.Add(dr["assdwrkr"].ToString());
                }
                var distassdworkerlist = assdworkerlist.Distinct().ToArray();
                workerlist.AddRange(distassdworkerlist);
                var distinctworkerlist = workerlist.Distinct().ToArray();



                string strWorker = string.Join(",", Array.ConvertAll(distinctworkerlist, x => SurroundWith(x.ToString(), "'")));
                #endregion

                if (strWorker.Trim() != string.Empty)
                {
                    #region get worker from DB to grid
                    objCustomer.Status = Session["MSM"].ToString() == "TS" ? 1 : 0;
                    objCustomer.Name = strWorker;
                    objCustomer.ConnConfig = Session["config"].ToString();
                    DataSet dsWorker = new DataSet();
                    dsWorker = objBL_Customer.getWorkers(objCustomer);
                    //dsWorker = objBL_Customer.getWorkerMonthly(objCustomer);

                    gvWorkerChanges.DataSource = dsWorker.Tables[0];
                    gvWorkerChanges.DataBind();

                    #endregion


                    if (dsWorker.Tables[0].Rows.Count > 0)
                    {
                        double totalhours = 0;
                        double totalamt = 0;
                        int totalcontract = 0;
                        int totalunits = 0;

                        #region get assigned workerstotals

                        var dtAssdworkers = new DataTable();
                        dtAssdworkers.Columns.Add("worker");
                        dtAssdworkers.Columns.Add("totalhours");
                        dtAssdworkers.Columns.Add("totalamt");
                        dtAssdworkers.Columns.Add("totalunits");
                        dtAssdworkers.Columns.Add("totalcontract");

                        foreach (string s in distassdworkerlist)
                        {
                            totalhours = 0;
                            totalamt = 0;
                            totalcontract = 0;
                            totalunits = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (String.Equals(s, dr["assdwrkr"].ToString(), StringComparison.CurrentCultureIgnoreCase))//  &&   s.ToUpper() != dr["worker"].ToString().ToUpper())
                                {
                                    if (!String.Equals(s, dr["worker"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        totalhours += Convert.ToDouble(dr["MonthlyHours"]);
                                        totalamt += Convert.ToDouble(dr["MonthlyBill"]);
                                        totalunits += Convert.ToInt32(dr["elevcount"]);
                                        totalcontract++;
                                    }
                                }
                                else if (String.Equals(s, dr["worker"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                                {
                                    totalhours -= Convert.ToDouble(dr["MonthlyHours"]);
                                    totalamt -= Convert.ToDouble(dr["MonthlyBill"]);
                                    totalunits -= Convert.ToInt32(dr["elevcount"]);
                                    totalcontract--;
                                }
                            }

                            DataRow drAssdworkersRow = dtAssdworkers.NewRow();
                            drAssdworkersRow["worker"] = s;
                            drAssdworkersRow["totalhours"] = totalhours;
                            drAssdworkersRow["totalamt"] = totalamt;
                            drAssdworkersRow["totalunits"] = totalunits;
                            drAssdworkersRow["totalcontract"] = totalcontract;
                            dtAssdworkers.Rows.Add(drAssdworkersRow);

                        }
                        #endregion

                        #region Calulate new changes on worker grid

                        double GtotalWorkerhours = 0;
                        double GtotalWorkeramount = 0;
                        int GtotalWorkercontr = 0;
                        int GtotalWorkerunits = 0;
                        double GHours = 0;
                        double GAmt = 0;
                        int GContr = 0;
                        int GUnits = 0;

                        foreach (GridDataItem gr in gvWorkerChanges.Items)
                        {
                            Label lblNewamt = (Label)gr.FindControl("lblNewamt");
                            Label lblNewHours = (Label)gr.FindControl("lblNewHours");
                            Label lblCurWorker = (Label)gr.FindControl("lblCurWorker");
                            Label lblHours = (Label)gr.FindControl("lblHours");
                            HiddenField lblAmt = (HiddenField)gr.FindControl("hdnAmt");
                            Label lblContr = (Label)gr.FindControl("lblContr");
                            Label lblUnits = (Label)gr.FindControl("lblUnitsGd");
                            Label lblNewContr = (Label)gr.FindControl("lblNewContr");
                            Label lblNewUnits = (Label)gr.FindControl("lblNewUnits");

                            double hours = 0;
                            double totalWorkerhours = 0;
                            double amount = 0;
                            double totalWorkeramount = 0;
                            int contr = 0;
                            int totalWorkercontr = 0;
                            int units = 0;
                            int totalWorkerunits = 0;

                            if (lblHours.Text.Trim() != string.Empty)
                            {
                                hours = Convert.ToDouble(lblHours.Text);
                            }
                            if (lblAmt.Value.Trim() != string.Empty)
                            {
                                amount = Convert.ToDouble(lblAmt.Value);
                            }
                            if (lblContr.Text.Trim() != string.Empty)
                            {
                                contr = Convert.ToInt32(lblContr.Text);
                            }
                            if (lblUnits.Text.Trim() != string.Empty)
                            {
                                units = Convert.ToInt32(lblUnits.Text);
                            }

                            int IsAssignedWorker = 0;
                            foreach (DataRow dr in dtAssdworkers.Rows)
                            {
                                if (lblCurWorker.Text.ToUpper() == dr["worker"].ToString().ToUpper())
                                {
                                    totalWorkerhours = hours + Convert.ToDouble(dr["totalhours"]);
                                    lblNewHours.Text = string.Format("{0:n}", totalWorkerhours);

                                    totalWorkeramount = amount + Convert.ToDouble(dr["totalamt"]);
                                    lblNewamt.Text = string.Format("{0:c}", totalWorkeramount);

                                    totalWorkercontr = contr + Convert.ToInt32(dr["totalcontract"]);
                                    lblNewContr.Text = totalWorkercontr.ToString();

                                    totalWorkerunits = units + Convert.ToInt32(dr["totalunits"]);
                                    lblNewUnits.Text = totalWorkerunits.ToString();

                                    IsAssignedWorker = 1;
                                }
                            }
                            #region if not assigned worker
                            if (IsAssignedWorker == 0)
                            {
                                totalhours = 0;
                                totalamt = 0;
                                totalcontract = 0;
                                totalunits = 0;
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (lblCurWorker.Text.ToUpper() == dr["Worker"].ToString().ToUpper())
                                    {
                                        if (dr["MonthlyHours"].ToString().Trim() != string.Empty)
                                            totalhours += Convert.ToDouble(dr["MonthlyHours"]);

                                        if (dr["MonthlyBill"].ToString().Trim() != string.Empty)
                                            totalamt += Convert.ToDouble(dr["MonthlyBill"]);

                                        if (dr["elevcount"].ToString().Trim() != string.Empty)
                                            totalunits += Convert.ToInt32(dr["elevcount"]);

                                        totalcontract++;
                                    }
                                }
                                totalWorkerhours = hours - totalhours;
                                lblNewHours.Text = string.Format("{0:n}", totalWorkerhours);

                                totalWorkeramount = amount - totalamt;
                                lblNewamt.Text = string.Format("{0:c}", totalWorkeramount);

                                totalWorkercontr = contr - totalcontract;
                                lblNewContr.Text = totalWorkercontr.ToString();

                                totalWorkerunits = units - totalunits;
                                lblNewUnits.Text = totalWorkerunits.ToString();
                            }
                            #endregion

                            #region grandtotals
                            GtotalWorkerhours += totalWorkerhours;
                            GtotalWorkeramount += totalWorkeramount;
                            GtotalWorkercontr += totalWorkercontr;
                            GtotalWorkerunits += totalWorkerunits;
                            GHours += hours;
                            GAmt += amount;
                            GContr += contr;
                            GUnits += units;
                            #endregion
                        }

                        var footerItem = (GridFooterItem)gvWorkerChanges.MasterTableView.GetItems(GridItemType.Footer)[0];
                        Label lbltotalCU = (Label)footerItem.FindControl("lbltotalCU");
                        Label lbltotalNCU = (Label)footerItem.FindControl("lbltotalNCU");
                        Label lbltotalHA = (Label)footerItem.FindControl("lbltotalHA");
                        Label lbltotalNHA = (Label)footerItem.FindControl("lbltotalNHA");
                        lbltotalCU.Text = GContr.ToString() + " - " + GtotalWorkercontr.ToString();
                        lbltotalNCU.Text = GUnits.ToString() + " - " + GtotalWorkerunits.ToString();
                        lbltotalHA.Text = string.Format("{0:n}", GHours) + " - " + string.Format("{0:n}", GtotalWorkerhours);
                        lbltotalNHA.Text = string.Format("{0:c}", GAmt) + " - " + string.Format("{0:c}", GtotalWorkeramount);
                        #endregion

                    }
                }
                else
                {
                    gvWorkerChanges.DataSource = string.Empty;
                    gvWorkerChanges.DataBind();
                }
            }
            else
            {
                gvWorkerChanges.DataSource = string.Empty;
                gvWorkerChanges.DataBind();
            }
        }
        catch (Exception ex)
        {
            //var t = ex.Message;
        }
    }

    private string SurroundWith(string text, string ends)
    {
        return ends + text + ends;
    }
    private string SurroundWith(string text, string starts, string ends)
    {
        return starts + text + ends;
    }

    public double Distance(double Latitude1, double Longitude1, double Latitude2, double Longitude2)
    {
        //1- miles
        //double R = (type == 1) ? 3960 : 6371;          // R is earth radius.
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

    protected void btnAssignMethodCall_Click(object sender, EventArgs e)
    {
        AssignWorker(1, null);
    }

    protected void btnAssignWorkerMarker_Click(object sender, EventArgs e)
    {
        DataTable dtLocChange = new DataTable();
        dtLocChange.Columns.Add("TemplateID", typeof(int));
        dtLocChange.Columns.Add("Loc", typeof(int));
        dtLocChange.Columns.Add("Worker", typeof(int));

        DataRow dr = dtLocChange.NewRow();
        dr["TemplateID"] = 0;
        dr["Loc"] = Convert.ToInt32(hdnMarkerLoc.Value);
        dr["Worker"] = Convert.ToInt32(lstWorkerAssignMarker.SelectedValue);
        dtLocChange.Rows.Add(dr);

        if (dtLocChange.Rows.Count > 0)
        {
            objCustomer.ConnConfig = Session["config"].ToString();
            objCustomer.DtTemplateData = dtLocChange;
            objBL_Customer.UpdateLocRoute(objCustomer);

            txtSearchLoc.Text = string.Empty;
            chkNulls.Checked = false;
            ddlWorker.SelectedIndex = -1;
            ViewState["CreateMarker"] = true;
            gvLocations.Rebind();
            gvWorkers.Rebind();
            UpdateTemplateOnLocationUpdate();

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "noty({text: '" + lstWorkerAssignMarker.SelectedItem.Text + " assigned to location successfully!',  type : 'success', dismissQueue: true, layout:'topCenter',closeOnSelfClick:true, timeout : 10000,theme : 'noty_theme_default',  closable : false});  AddLocationMarker() ", true);
            ModalPopupExtender2.Hide();
        }
    }

    private void UpdateTemplateOnLocationUpdate()
    {
        DataTable dtLocChange = new DataTable();
        dtLocChange.Columns.Add("TemplateID", typeof(int));
        dtLocChange.Columns.Add("Loc", typeof(int));
        dtLocChange.Columns.Add("Worker", typeof(int));
        dtLocChange.Columns.Add("polyid", typeof(int));
        dtLocChange.Columns.Add("WorkerName", typeof(string));

        foreach (GridDataItem gr in gvLocChanges.Items)
        {
            HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");
            HiddenField hdnWorker = (HiddenField)gr.FindControl("hdnWorker");
            HiddenField hdnPolyid = (HiddenField)gr.FindControl("hdnPolyid");
            Label lblCurWorker = (Label)gr.FindControl("lblCurWorker");
            Label lblNewWorker = (Label)gr.FindControl("lblNewWorker");
            DataRow dr = dtLocChange.NewRow();

            dr["TemplateID"] = Convert.ToInt32(ddlTemplates.SelectedValue);
            dr["Loc"] = Convert.ToInt32(hdnID.Value);
            dr["Worker"] = Convert.ToInt32(hdnWorker.Value);
            dr["polyid"] = Convert.ToInt32(hdnPolyid.Value);
            dr["WorkerName"] = lblNewWorker.Text;

            dtLocChange.Rows.Add(dr);
        }

        if (dtLocChange.Rows.Count > 0)
        {
            AssignWorker(2, dtLocChange);
            chkMap.Checked = false;
        }
    }

    protected void btnSubmitAddress_Click(object sender, EventArgs e)
    {

    }

    protected void btnClearClick_Click(object sender, EventArgs e)
    {
        //Clear();
        txtSearchLoc.Text = string.Empty;
        ddlWorker.SelectedIndex = -1;
        chkNulls.Checked = false;
        ViewState["CreateMarker"] = true;
        ViewState["CreateMarker"] = true;
        gvLocations.Rebind();
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "AddAllMarkers();", true);
        ModalPopupExtender3.Hide();
    }

    protected void chkNulls_CheckedChanged(object sender, EventArgs e)
    {
        txtSearchLoc.Text = string.Empty;
        ddlWorker.SelectedIndex = -1;
        ViewState["CreateMarker"] = false;
        gvLocations.Rebind();
    }
    protected void ddlSuper_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    private void FillSupervisor()
    {

        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getSupervisor(objPropUser);

        ddlSuper.DataSource = ds.Tables[0];
        ddlSuper.DataTextField = "fuser";
        ddlSuper.DataValueField = "fuser";
        ddlSuper.DataBind();

        ddlSuper.Items.Insert(0, new ListItem("- All -", ""));
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

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = false;
                HttpContext.Current.Response.AddHeader("Accept-Ranges", "bytes");
                HttpContext.Current.Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                HttpContext.Current.Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFileName));
                HttpContext.Current.Response.AddHeader("Content-Length", (FileName.Length - startBytes).ToString());
                HttpContext.Current.Response.AddHeader("Connection", "Keep-Alive");
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;

                //Send data
                _BinaryReader.BaseStream.Seek(startBytes, SeekOrigin.Begin);

                //Dividing the data in 1024 bytes package
                int maxCount = (int)Math.Ceiling((FileName.Length - startBytes + 0.0) / 1024);

                //Download in block of 1024 bytes
                int i;
                for (i = 0; i < maxCount && Response.IsClientConnected; i++)
                {
                    HttpContext.Current.Response.BinaryWrite(_BinaryReader.ReadBytes(1024));
                    HttpContext.Current.Response.Flush();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _BinaryReader.Close();
                myFile.Close();
                HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
                //HttpContext.Current.Response.End();
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

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    protected void btnSearch_Click1(object sender, EventArgs e)
    {
        ViewState["CreateMarker"] = true;
        gvLocations.Rebind();
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "sdgtdsrfg", "initialize();", true);
        // google.maps.event.addDomListener();
    }

    protected void lnkSaveTemplate_Click(object sender, EventArgs e)
    {
        if (txtTemplate.Text.Trim() == string.Empty)
        {
            return;
        }
        int Mode = 1;
        string Msg = "Updated";
        if (ddlTemplates.SelectedValue == "0" || ddlTemplates.SelectedValue == "-1")
        {
            Mode = 0;
            Msg = "Added";
        }

        objCustomer.ConnConfig = Session["config"].ToString();
        objCustomer.Name = txtTemplate.Text.Trim();
        objCustomer.RouteSequence = hdnRouteSeq.Value;
        objCustomer.Remarks = txtRemarks.Text;
        objCustomer.Mode = Mode;
        objCustomer.TemplateID = Convert.ToInt32(ddlTemplates.SelectedValue);

        objCustomer.Overlay = hdnOverlay.Value;
        if (hdnOverlay.Value.ToLower() == "circle")
        {
            objCustomer.Center = hdnCenter.Value;
            objCustomer.Radius = hdnRadius.Value;
        }
        else if (hdnOverlay.Value.ToLower() == "polygon")
        {
            objCustomer.PolygonCoord = hdnPolygon.Value;
        }

        try
        {
            int TemplateID = objBL_Customer.AddRouteTemplate(objCustomer);
            GetRouteTemplate();
            ddlTemplates.SelectedValue = TemplateID.ToString();
            hdnEdited.Value = "0";
            //lnkSaveTemplate.ImageUrl = "images/saveiconblack.png";
            lnkSaveTemplate.Enabled = false;
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "noty({text: 'Template " + Msg + " Successfully!',  type : 'success', dismissQueue: true, layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }


    protected void btnClear_Click1(object sender, EventArgs e)
    {
        Clear();
    }

    protected void gvLocations_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        objCustomer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        objCustomer.SearchValue = txtSearchLoc.Text.Trim();
        objCustomer.NullAddressOnly = chkNulls.Checked;
        objCustomer.Status = Session["MSM"].ToString() == "TS" ? 1 : 0;
        #region Company Check
        objCustomer.UserID = Convert.ToInt32(Session["UserID"].ToString());

        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            objCustomer.EN = 1;
        }
        else
        {
            objCustomer.EN = 0;
        }
        #endregion

        var worker = int.Parse(ddlWorker.SelectedValue);

        objCustomer.Worker = worker;

        ds = objBL_Customer.getLocCoordinates(objCustomer);



        AddFullAddress(ds.Tables[0]);
        gvLocations.DataSource = ds.Tables[0];
        gvLocations.VirtualItemCount = ds.Tables[0].Rows.Count;

        var createMarkers = (bool)ViewState["CreateMarker"];

        if (createMarkers == true)
        {
            BindMarkers(ds.Tables[0], false);
        }
    }

    private void AddFullAddress(DataTable dataTable)
    {
        dataTable.Columns.Add("FullAddress", typeof(string), "address + ', ' + City");
    }

    protected void gvLocations_PreRender(object sender, EventArgs e)
    {
        foreach (GridDataItem gr in gvLocations.Items)
        {
            HiddenField hdnCoordinate = (HiddenField)gr.FindControl("hdnCoordinate");
            HyperLink lnkLoc = (HyperLink)gr.FindControl("lnkLoc");
            Label lblLoc = (Label)gr.FindControl("lblLoc");
            Label lblAddress = (Label)gr.FindControl("lblAddress");
            string[] coordinate = hdnCoordinate.Value.Split(',');

            if (coordinate[0] != string.Empty)
                gr.Attributes["onclick"] = "GridHover(" + lblLoc.Text + ")";
            else
                gr.Attributes["onclick"] = "alert('Coordinates not available. Please edit the address.');";

            lblAddress.Attributes["onclick"] = "document.getElementById('" + iframeAddr.ClientID + "').src = 'locationaddress.aspx?uid=" + lblLoc.Text + "'; $find('PMPAddress').show();";

            if (Session["MSM"].ToString() == "TS")
            {
                lnkLoc.Enabled = false;
            }
        }


    }

    protected void gvLocations_ItemCreated(object sender, GridItemEventArgs e)
    {
        UpdatePageSize(e);
    }

    protected void gvWorkers_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        objCustomer.ConnConfig = Session["config"].ToString();
        objCustomer.Status = Session["MSM"].ToString() == "TS" ? 1 : 0;
        objCustomer.Name = string.Empty;
        DataSet ds = objBL_Customer.getWorkers(objCustomer);
        gvWorkers.DataSource = ds.Tables[0];
        gvWorkers.VirtualItemCount = ds.Tables[0].Rows.Count;


    }

    protected void gvWorkers_PreRender(object sender, EventArgs e)
    {
    }

    protected void gvWorkers_ItemCreated(object sender, GridItemEventArgs e)
    {
        UpdatePageSize(e);
    }

    protected void gvWorkers_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToLowerInvariant() == "select")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            txtSearchLoc.Text = string.Empty;
            chkNulls.Checked = false;
            ViewState["CreateMarker"] = false;
            ddlWorker.SelectedValue = id.ToString();
            gvLocations.Rebind();
        }
    }

    private static void UpdatePageSize(GridItemEventArgs e)
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

    protected void btnLocationChange_Click(object sender, EventArgs e)
    {
        ResetActiveArea();
        btnLocationChange.Attributes["class"] = "active";
        locationChangesView.Visible = true;
    }

    protected void btnWorkerChange_Click(object sender, EventArgs e)
    {
        ResetActiveArea();
        btnWorkerChange.Attributes["class"] = "active";
        workerChangesView.Visible = true;
    }

    private void ResetActiveArea()
    {
        btnLocationChange.Attributes["class"] = string.Empty;
        btnWorkerChange.Attributes["class"] = string.Empty;
        locationChangesView.Visible = false;
        workerChangesView.Visible = false;
    }

    protected void gvLocations_ItemEvent(object sender, GridItemEventArgs e)
    {
        int rowCount = 0;
        if (e.EventInfo is GridInitializePagerItem)
        {
            rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
        }
        lblTotalRecLoc.Text = rowCount + " Record(s) found";
    }

    protected void gvWorkers_ItemEvent(object sender, GridItemEventArgs e)
    {
        int rowCount = 0;
        if (e.EventInfo is GridInitializePagerItem)
        {
            rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
        }
        lblTotalRecWork.Text = rowCount + " Record(s) found";
    }

    protected void gvLocChanges_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        var source = ViewState["gvLocChanges"] as DataTable;
        if (source != null)
        {
            gvLocChanges.DataSource = source;
            gvLocChanges.VirtualItemCount = source.Rows.Count;
        }
        else
        {
            gvLocChanges.VirtualItemCount = 0;
            gvLocChanges.DataSource = string.Empty;
        }
    }

    private void SetDefaultWorker()
    {
        Customer objCustomer = new Customer();
        BL_Customer objBL_Customer = new BL_Customer();
        var masterTableView = gvLocations.MasterTableView;
        var column = masterTableView.GetColumn("DRoute");

        var masterTableView1 = gvWorkers.MasterTableView;
        var column1 = masterTableView1.GetColumn("WRoute");

        var masterTableView2 = gvLocChanges.MasterTableView;
        var column2 = masterTableView2.GetColumn("CurrentWorker");

        var masterTableView3 = gvLocChanges.MasterTableView;
        var column3 = masterTableView3.GetColumn("NewWoker");

        objCustomer.ConnConfig = Session["config"].ToString();
        string getValue = objBL_Customer.GetDefaultWorkerHeader(objCustomer);
        if (!string.IsNullOrEmpty(getValue))
        {
            ddlWorker.Items.Insert(0, new ListItem("-" + getValue + "-", "-1"));
            column.HeaderText = getValue;
            column1.HeaderText = getValue;
            column2.HeaderText = "Current " + getValue;
            column3.HeaderText = "New " + getValue;
            Pane1.Title = getValue + " Info";
            anAdd.InnerText = getValue;
        }
        else
        {
            ddlWorker.Items.Insert(0, new ListItem("-Default Worker-", "-1"));
            column.HeaderText = "Default Worker";
            column1.HeaderText = "Default Worker";
            column2.HeaderText = "Current Default Worker";
            column3.HeaderText = "New Default Worker";
            Pane1.Title = "Default Worker Info";
            anAdd.InnerText = "Default Worker";
        }
        gvLocations.Rebind();
    }

    protected void gvLocChanges_ExcelMLExportRowCreated(object sender, GridExportExcelMLRowCreatedArgs e)
    {
        try
        {
            int currentItem = 2;
            if (e.Worksheet.Table.Rows.Count == gvLocChanges.Items.Count + 1)
            {
                GridFooterItem footerItem = gvLocChanges.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
                RowElement row = new RowElement(); //create new row for the footer aggregates
                for (int i = currentItem; i < footerItem.Cells.Count; i++)
                {
                    TableCell fcell = footerItem.Cells[i];
                    CellElement cell = new CellElement();
                    // cell.Data.DataItem =  fcell.Text == "&nbsp;" ? "" : fcell.Text;
                    if (i == currentItem)
                        cell.Data.DataItem = "Total:-";
                    else
                        cell.Data.DataItem = fcell.Text == "&nbsp;" ? "" : fcell.Text;
                    row.Cells.Add(cell);
                }
                e.Worksheet.Table.Rows.Add(row);

            }
        }
        catch { }
    }

    protected void lnkLocExportPostback_Click(object sender, EventArgs e)
    {
        if (gvLocChanges.Items.Count > 0)
        {
            gvLocChanges.ExportSettings.FileName = "Location Changes";
            gvLocChanges.ExportSettings.IgnorePaging = true;
            gvLocChanges.ExportSettings.ExportOnlyData = true;
            gvLocChanges.ExportSettings.OpenInNewWindow = true;
            gvLocChanges.ExportSettings.HideStructureColumns = true;
            gvLocChanges.MasterTableView.UseAllDataFields = true;
            gvLocChanges.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
            gvLocChanges.MasterTableView.ExportToExcel();

            //var source = ViewState["gvLocChanges"] as DataTable;
            //BusinessLayer.Utility.BL_Utility dd = new BusinessLayer.Utility.BL_Utility();

            //var tempFile = string.Format("LocationExport{0}.xlsx", DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"));
            //var fileName = Server.MapPath(string.Format("ReportFiles/ExportExcel/{0}", tempFile));
            //var strFileName = dd.ExportDataTableToExcel(source, fileName);
            //if (strFileName.Length > 0)
            //{
            //    try
            //    {
            //        DownloadDocument(strFileName, "LocationExport.xlsx");
            //        // Delete after downloaded
            //        if (File.Exists(strFileName))
            //            File.Delete(strFileName);
            //    }
            //    catch (Exception)
            //    {
            //        // Delete after downloaded
            //        if (File.Exists(strFileName))
            //            File.Delete(strFileName);
            //    }
            //}
            //else
            //{
            //    string str = "Export failed!";
            //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrExportExcel", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false,dismissQueue: true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            //}
        }
    }

    protected void lnkWorkerExportPostback_Click(object sender, EventArgs e)
    {
        if (gvWorkerChanges.Items.Count > 0)
        {
            var dt = ViewState["gvLocChanges"] as DataTable;

            if (dt.Rows.Count > 0)
            {
                DataTable workerExport = WorkerChangesExport(dt);
                BusinessLayer.Utility.BL_Utility dd = new BusinessLayer.Utility.BL_Utility();

                var tempFile = string.Format("WorkerExport{0}.xlsx", DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"));
                var fileName = Server.MapPath(string.Format("ReportFiles/ExportExcel/{0}", tempFile));
                var strFileName = dd.ExportDataTableToExcel(workerExport, fileName);
                if (strFileName.Length > 0)
                {
                    try
                    {
                        DownloadDocument(strFileName, "WorkerExport.xlsx");
                        // Delete after downloaded
                        if (File.Exists(strFileName))
                            File.Delete(strFileName);
                    }
                    catch (Exception)
                    {
                        // Delete after downloaded
                        if (File.Exists(strFileName))
                            File.Delete(strFileName);
                    }
                }
                else
                {
                    string str = "Export failed!";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrExportExcel", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false,dismissQueue: true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
        }
    }

    private DataTable WorkerChangesExport(DataTable dt)
    {
        try
        {
            DataTable returnDt = new DataTable();
            returnDt.Columns.Add("Worker", typeof(string));
            returnDt.Columns.Add("Contracts", typeof(string));
            returnDt.Columns.Add("Units", typeof(string));
            returnDt.Columns.Add("Hours(Monthly)", typeof(string));
            returnDt.Columns.Add("Amount(Monthly)", typeof(string));

            if (dt.Rows.Count > 0)
            {
                #region Get workerarray
                List<string> workerlist = new List<string>();
                List<string> assdworkerlist = new List<string>();

                foreach (DataRow dr in dt.Rows)
                {

                    workerlist.Add(dr["worker"].ToString());
                    assdworkerlist.Add(dr["assdwrkr"].ToString());
                }
                var distassdworkerlist = assdworkerlist.Distinct().ToArray();
                workerlist.AddRange(distassdworkerlist);
                var distinctworkerlist = workerlist.Distinct().ToArray();

                string strWorker = string.Join(",", Array.ConvertAll(distinctworkerlist, x => SurroundWith(x.ToString(), "'")));
                #endregion

                if (strWorker.Trim() != string.Empty)
                {
                    #region get worker from DB to grid
                    objCustomer.Status = Session["MSM"].ToString() == "TS" ? 1 : 0;
                    objCustomer.Name = strWorker;
                    objCustomer.ConnConfig = Session["config"].ToString();
                    DataSet dsWorker = new DataSet();
                    dsWorker = objBL_Customer.getWorkers(objCustomer);
                    #endregion

                    if (dsWorker.Tables[0].Rows.Count > 0)
                    {


                        double totalhours = 0;
                        double totalamt = 0;
                        int totalcontract = 0;
                        int totalunits = 0;

                        #region get assigned workerstotals

                        var dtAssdworkers = new DataTable();
                        dtAssdworkers.Columns.Add("worker");
                        dtAssdworkers.Columns.Add("totalhours");
                        dtAssdworkers.Columns.Add("totalamt");
                        dtAssdworkers.Columns.Add("totalunits");
                        dtAssdworkers.Columns.Add("totalcontract");

                        foreach (string s in distassdworkerlist)
                        {
                            totalhours = 0;
                            totalamt = 0;
                            totalcontract = 0;
                            totalunits = 0;
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (String.Equals(s, dr["assdwrkr"].ToString(), StringComparison.CurrentCultureIgnoreCase))//  &&   s.ToUpper() != dr["worker"].ToString().ToUpper())
                                {
                                    if (!String.Equals(s, dr["worker"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        totalhours += Convert.ToDouble(dr["MonthlyHours"]);
                                        totalamt += Convert.ToDouble(dr["MonthlyBill"]);
                                        totalunits += Convert.ToInt32(dr["elevcount"]);
                                        totalcontract++;
                                    }
                                }
                                else if (String.Equals(s, dr["worker"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                                {
                                    totalhours -= Convert.ToDouble(dr["MonthlyHours"]);
                                    totalamt -= Convert.ToDouble(dr["MonthlyBill"]);
                                    totalunits -= Convert.ToInt32(dr["elevcount"]);
                                    totalcontract--;
                                }
                            }

                            DataRow drAssdworkersRow = dtAssdworkers.NewRow();
                            drAssdworkersRow["worker"] = s;
                            drAssdworkersRow["totalhours"] = totalhours;
                            drAssdworkersRow["totalamt"] = totalamt;
                            drAssdworkersRow["totalunits"] = totalunits;
                            drAssdworkersRow["totalcontract"] = totalcontract;
                            dtAssdworkers.Rows.Add(drAssdworkersRow);

                        }
                        #endregion

                        #region Calulate new changes on worker grid

                        double GtotalWorkerhours = 0;
                        double GtotalWorkeramount = 0;
                        int GtotalWorkercontr = 0;
                        int GtotalWorkerunits = 0;
                        double GHours = 0;
                        double GAmt = 0;
                        int GContr = 0;
                        int GUnits = 0;

                        foreach (GridDataItem gr in gvWorkerChanges.Items)
                        {
                            DataRow dataRow = returnDt.NewRow();

                            Label lblNewamt = (Label)gr.FindControl("lblNewamt");
                            Label lblNewHours = (Label)gr.FindControl("lblNewHours");
                            Label lblCurWorker = (Label)gr.FindControl("lblCurWorker");
                            Label lblHours = (Label)gr.FindControl("lblHours");
                            HiddenField lblAmt = (HiddenField)gr.FindControl("hdnAmt");
                            Label lblContr = (Label)gr.FindControl("lblContr");
                            Label lblUnits = (Label)gr.FindControl("lblUnitsGd");
                            Label lblNewContr = (Label)gr.FindControl("lblNewContr");
                            Label lblNewUnits = (Label)gr.FindControl("lblNewUnits");

                            Label lblUnits1 = (Label)gr.FindControl("lblUnits");
                            Label lblAmt1 = (Label)gr.FindControl("lblAmt");

                            double hours = 0;
                            double totalWorkerhours = 0;
                            double amount = 0;
                            double totalWorkeramount = 0;
                            int contr = 0;
                            int totalWorkercontr = 0;
                            int units = 0;
                            int totalWorkerunits = 0;

                            if (lblHours.Text.Trim() != string.Empty)
                            {
                                hours = Convert.ToDouble(lblHours.Text);
                            }
                            if (lblAmt.Value.Trim() != string.Empty)
                            {
                                amount = Convert.ToDouble(lblAmt.Value);
                            }
                            if (lblContr.Text.Trim() != string.Empty)
                            {
                                contr = Convert.ToInt32(lblContr.Text);
                            }
                            if (lblUnits.Text.Trim() != string.Empty)
                            {
                                units = Convert.ToInt32(lblUnits.Text);
                            }

                            int IsAssignedWorker = 0;
                            foreach (DataRow dr in dtAssdworkers.Rows)
                            {
                                if (lblCurWorker.Text.ToUpper() == dr["worker"].ToString().ToUpper())
                                {
                                    totalWorkerhours = hours + Convert.ToDouble(dr["totalhours"]);
                                    lblNewHours.Text = string.Format("{0:n}", totalWorkerhours);

                                    totalWorkeramount = amount + Convert.ToDouble(dr["totalamt"]);
                                    lblNewamt.Text = string.Format("{0:c}", totalWorkeramount);

                                    totalWorkercontr = contr + Convert.ToInt32(dr["totalcontract"]);
                                    lblNewContr.Text = totalWorkercontr.ToString();

                                    totalWorkerunits = units + Convert.ToInt32(dr["totalunits"]);
                                    lblNewUnits.Text = totalWorkerunits.ToString();

                                    IsAssignedWorker = 1;
                                }
                            }
                            #region if not assigned worker
                            if (IsAssignedWorker == 0)
                            {
                                totalhours = 0;
                                totalamt = 0;
                                totalcontract = 0;
                                totalunits = 0;
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (lblCurWorker.Text.ToUpper() == dr["Worker"].ToString().ToUpper())
                                    {
                                        if (dr["MonthlyHours"].ToString().Trim() != string.Empty)
                                            totalhours += Convert.ToDouble(dr["MonthlyHours"]);

                                        if (dr["MonthlyBill"].ToString().Trim() != string.Empty)
                                            totalamt += Convert.ToDouble(dr["MonthlyBill"]);

                                        if (dr["elevcount"].ToString().Trim() != string.Empty)
                                            totalunits += Convert.ToInt32(dr["elevcount"]);

                                        totalcontract++;
                                    }
                                }
                                totalWorkerhours = hours - totalhours;
                                lblNewHours.Text = string.Format("{0:n}", totalWorkerhours);

                                totalWorkeramount = amount - totalamt;
                                lblNewamt.Text = string.Format("{0:c}", totalWorkeramount);

                                totalWorkercontr = contr - totalcontract;
                                lblNewContr.Text = totalWorkercontr.ToString();

                                totalWorkerunits = units - totalunits;
                                lblNewUnits.Text = totalWorkerunits.ToString();
                            }
                            #endregion

                            dataRow[0] = lblCurWorker.Text;
                            dataRow[1] = lblContr.Text + " - " + lblNewContr.Text;
                            dataRow[2] = lblUnits1.Text + " - " + lblNewUnits.Text;
                            dataRow[3] = lblHours.Text + " - " + lblNewHours.Text;
                            dataRow[4] = lblAmt1.Text + " - " + lblNewamt.Text;

                            returnDt.Rows.Add(dataRow);

                            #region grandtotals
                            GtotalWorkerhours += totalWorkerhours;
                            GtotalWorkeramount += totalWorkeramount;
                            GtotalWorkercontr += totalWorkercontr;
                            GtotalWorkerunits += totalWorkerunits;
                            GHours += hours;
                            GAmt += amount;
                            GContr += contr;
                            GUnits += units;
                            #endregion
                        }

                        var footerItem = (GridFooterItem)gvWorkerChanges.MasterTableView.GetItems(GridItemType.Footer)[0];
                        Label lbltotalCU = (Label)footerItem.FindControl("lbltotalCU");
                        Label lbltotalNCU = (Label)footerItem.FindControl("lbltotalNCU");
                        Label lbltotalHA = (Label)footerItem.FindControl("lbltotalHA");
                        Label lbltotalNHA = (Label)footerItem.FindControl("lbltotalNHA");
                        lbltotalCU.Text = GContr.ToString() + " - " + GtotalWorkercontr.ToString();
                        lbltotalNCU.Text = GUnits.ToString() + " - " + GtotalWorkerunits.ToString();
                        lbltotalHA.Text = string.Format("{0:n}", GHours) + " - " + string.Format("{0:n}", GtotalWorkerhours);
                        lbltotalNHA.Text = string.Format("{0:c}", GAmt) + " - " + string.Format("{0:c}", GtotalWorkeramount);

                        DataRow footer = returnDt.NewRow();
                        footer[1] = lbltotalCU.Text;
                        footer[2] = lbltotalNCU.Text;
                        footer[3] = lbltotalHA.Text;
                        footer[4] = lbltotalNHA.Text;
                        returnDt.Rows.Add(footer);
                        #endregion

                    }
                }
            }

            return returnDt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
