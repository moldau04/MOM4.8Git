using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLayer;
using BusinessEntity;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Collections;
using Telerik.Web.UI.Scheduler.Views;
using System.Linq;
using System.Text;
using Microsoft.ApplicationBlocks.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Globalization;

[Telerik.Web.UI.RadCompressionSettings(HttpCompression = Telerik.Web.UI.CompressionType.GZip)]
public partial class Scheduler : System.Web.UI.Page
{
    BL_User objBL_User = new BL_User();
    User objProp_User = new User();

    BL_MapData objBL_MapData = new BL_MapData();
    MapData objpropMapData = new MapData();

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    protected void Page_Init(object sender, EventArgs e)
    {

        //if (!Page.IsPostBack)
        //BindTimeZones();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.Now);
        Response.Cache.SetNoServerCaching();
        Response.Cache.SetNoStore();
        hdnCheckActionScheduler.Value = "0";

        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!Page.IsPostBack)
        {
            SetBusinessTime();
            userpermissions();
            Permission();
            FillSupervisor();
            FillWorker();
            FillDepartment();
            FillCategory();
            FillScheduler();
            if (Session["type"].ToString() != "c")
            {
                GetPreferences();
                GetPreferencesShedularDefaultView();
            }
            RadGrid1.Rebind();
            HighlightSideMenu("schMgr", "lnkScheduleMenu", "schdMgrSub");
        }
        else
        {
            //   AppointmentMove();
        }

    }

    private void HighlightSideMenu(string MenuParent, string PageLink, string SubMenuDiv)
    {

    }
    #region Bind Controls

    private void FillScheduler()
    {
        DataSet dsdTickets = new DataSet();
        dsdTickets = GetTicketData();
        RadScheduler1.DataStartField = "start1";
        RadScheduler1.DataSubjectField = "NAME";
        RadScheduler1.DataEndField = "end1";
        RadScheduler1.DataKeyField = "id";
        RadScheduler1.DataSource = dsdTickets;
        try
        {

            RadScheduler1.WorkDayStartTime = (TimeSpan)ViewState["WorkDayStartTime"];
            RadScheduler1.WorkDayEndTime = (TimeSpan)ViewState["WorkDayEndTime"];

            RadScheduler1.DayStartTime = (TimeSpan)ViewState["DayStartTime"];
            RadScheduler1.DayEndTime = (TimeSpan)ViewState["DayEndTime"];
        }
        catch
        {
            RadScheduler1.WorkDayStartTime = new TimeSpan(01, 0, 0);
            RadScheduler1.WorkDayEndTime = new TimeSpan(23, 0, 0);
            RadScheduler1.DayStartTime = new TimeSpan(01, 0, 0);
            RadScheduler1.DayEndTime = new TimeSpan(23, 0, 0);
        }

        //Group BY worker         
        DataSet WorkerResources = GetResourceWorker();
        if (WorkerResources.Tables.Count > 0)
        {
            DataTable dt = WorkerResources.Tables[0];
            int countRow = dt.Rows.Count;
            for (int iRow = 0; iRow < countRow; iRow++)
            {
                int key = Convert.ToInt16(dt.Rows[iRow]["id"]);
                string Text = dt.Rows[iRow]["fdesc"].ToString();
                string profileImage = dt.Rows[iRow].Field<string>("ProfileImage");
                RadScheduler1.Resources.Add(new Resource() { Type = "worker", Key = key, Text = Text, });
                RadScheduler1.Resources.Add(new Resource() { Type = "workerProfileImage", Key = key, Text = profileImage, });

            }
        }
        RadShedulerViewDirection();
        RadScheduler1.DataBind();
        RadScheduler1.SelectedView = SchedulerViewType.DayView;
        RadScheduler1.SelectedDate = DateTime.Now;
        RadScheduler1.Visible = true;

    }
    private void SetBusinessTime()
    {

        try
        {
            var businessTimeds = GetBusinessTime();
            if (businessTimeds != null)
            {
                if (businessTimeds.Tables[0] != null && businessTimeds.Tables[0].Rows.Count > 0)
                {
                    if (businessTimeds.Tables[0].Rows[0]["businessstart"] != null)
                    {
                        var businessStartTime = DateTime.Parse(businessTimeds.Tables[0].Rows[0]["businessstart"].ToString());
                        ViewState["WorkDayStartTime"] = new TimeSpan(businessStartTime.Hour, businessStartTime.Minute, businessStartTime.Second);
                    }
                    else
                    {
                        ViewState["WorkDayStartTime"] = new TimeSpan(01, 0, 0);
                    }

                    if (businessTimeds.Tables[0].Rows[0]["businessend"] != null)
                    {
                        var businessEndTime = DateTime.Parse(businessTimeds.Tables[0].Rows[0]["businessend"].ToString());
                        ViewState["WorkDayEndTime"] = new TimeSpan(businessEndTime.Hour, businessEndTime.Minute, businessEndTime.Second);
                    }
                    else
                    {
                        ViewState["WorkDayEndTime"] = new TimeSpan(23, 0, 0);
                    }
                }
            }
            else
            {
                ViewState["WorkDayStartTime"] = new TimeSpan(01, 0, 0);
                ViewState["WorkDayEndTime"] = new TimeSpan(23, 0, 0);
            }

            ViewState["DayStartTime"] = ViewState["WorkDayStartTime"];
            ViewState["DayEndTime"] = ViewState["WorkDayEndTime"];
        }
        catch
        {
            ViewState["WorkDayStartTime"] = new TimeSpan(01, 0, 0);
            ViewState["WorkDayEndTime"] = new TimeSpan(23, 0, 0);
            ViewState["DayStartTime"] = ViewState["WorkDayStartTime"];
            ViewState["DayEndTime"] = ViewState["WorkDayEndTime"];

            ScriptManager.RegisterStartupScript(this, this.GetType(), "setWorkDayStartTime", "noty({ text: 'Please set business hours in the default setup.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 1000, theme: 'noty_theme_default', closable: false,dismissQueue: true });", true);
        }

    }
    private void RadShedulerViewDirection()
    {
        RadScheduler1.GroupBy = "worker";

        if (IsPostBack)
        {
            if (RadScheduler1.SelectedView == SchedulerViewType.DayView)
            {
                RadScheduler1.GroupingDirection = GroupingDirection.Horizontal;
                RadScheduler1.ColumnWidth = 150;
                RadScheduler1.MinutesPerRow = 15;
                addPreferencesDefaultShedularView(1);
                GetPreferencesShedularDefaultView();
            }
            else if (RadScheduler1.SelectedView == SchedulerViewType.WeekView)
            {
                RadScheduler1.GroupBy = "date,worker";
                RadScheduler1.GroupingDirection = GroupingDirection.Vertical;
                RadScheduler1.ColumnWidth = 150;
                RadScheduler1.MinutesPerRow = 30;
                addPreferencesDefaultShedularView(2);
                GetPreferencesShedularDefaultView();
            }
            else if (RadScheduler1.SelectedView == SchedulerViewType.MonthView)
            {
                RadScheduler1.GroupBy = "";
                RadScheduler1.GroupingDirection = GroupingDirection.Vertical;
                RadScheduler1.ColumnWidth = 130;
                addPreferencesDefaultShedularView(3);
                GetPreferencesShedularDefaultView();

            }
            else if (RadScheduler1.SelectedView == SchedulerViewType.TimelineView)
            {
                RadScheduler1.GroupingDirection = GroupingDirection.Vertical;
                RadScheduler1.TimelineView.NumberOfSlots = 16;
                RadScheduler1.TimelineView.SlotDuration = TimeSpan.Parse("01:00");
                RadScheduler1.TimelineView.StartTime = TimeSpan.Parse("09:00");
                RadScheduler1.TimelineView.ColumnHeaderDateFormat = "hh:mm tt";
                RadScheduler1.ColumnWidth = 100;
            }
        }
    }

    private void RadSchedulerRebind(bool isSearch = false, string value = "")
    {

        RadShedulerViewDirection();
        DataSet dsdTickets = new DataSet();
        dsdTickets = GetTicketData();
        RadScheduler1.DataSource = dsdTickets;
        DataTable dtTicketId = new DataTable();
        dtTicketId = (DataTable)Session["dsOpenCall"];
        List<int> listTicketId = new List<int>();
        foreach (DataRow dr in dtTicketId.Rows)
        {
            int workerid = Int32.Parse(dr["id"].ToString());
            listTicketId.Add(workerid);
        }

        List<int> listId = new List<int>();
        foreach (DataRow dr in dsdTickets.Tables[0].Rows)
        {
            if (!string.IsNullOrEmpty(dr["workerid"].ToString()) && listTicketId.Contains(Int32.Parse(dr["Id"].ToString())))
            {
                int workerid = Int32.Parse(dr["workerid"].ToString());
                listId.Add(workerid);
            }

        }
        RadScheduler1.Resources.Clear();
        //Group BY worker  
        DataSet WorkerResources = GetResourceWorker();
        if (WorkerResources.Tables.Count > 0)
        {
            DataTable dt = new DataTable();
            if (!isSearch || (isSearch && string.IsNullOrEmpty(value)) || listId.Count() == 0)
            {
                dt = WorkerResources.Tables[0];
            }
            else
            {
                dt = WorkerResources.Tables[0].Clone();
                foreach (DataRow dr in WorkerResources.Tables[0].Rows)
                {
                    if (listId.Contains(Int32.Parse(dr["ID"].ToString())))
                    {
                        dt.ImportRow(dr);
                    }
                }
            }

            int countRow = dt.Rows.Count;
            if (countRow > 0)
            {
                for (int iRow = 0; iRow < countRow; iRow++)
                {
                    int key = Convert.ToInt16(dt.Rows[iRow]["id"]);
                    string Text = dt.Rows[iRow]["fdesc"].ToString();
                    string profileImage = dt.Rows[iRow].Field<string>("ProfileImage");
                    if (ddlworker.SelectedItem.Value == "")
                    {
                        RadScheduler1.Resources.Add(new Resource() { Type = "worker", Key = key, Text = Text, });
                        RadScheduler1.Resources.Add(new Resource() { Type = "workerProfileImage", Key = key, Text = profileImage, });
                    }
                    else
                    {
                        if (Text == ddlworker.SelectedItem.Text)
                        {
                            RadScheduler1.Resources.Add(new Resource() { Type = "worker", Key = key, Text = Text, });
                            RadScheduler1.Resources.Add(new Resource() { Type = "workerProfileImage", Key = key, Text = profileImage, });
                        }
                    }

                }
            }
            else
            {
                RadScheduler1.Resources.Add(new Resource() { Type = "worker", Key = 0, Text = "", });

            }
        }
        else
        {
            RadScheduler1.Resources.Add(new Resource() { Type = "worker", Key = 0, Text = "", });

        }



        RadScheduler1.Rebind();
    }

    private DataSet GetResourceWorker()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.Supervisor = ddlSuper.SelectedValue;
        if (Convert.ToString(Session["ISsupervisor"]) == "1")
        {
            objProp_User.Supervisor = Session["username"].ToString();
        }
        objProp_User.IsTS = Session["MSM"].ToString();
        ds = objBL_User.getEMPScheduler(objProp_User);
        return ds;
    }

    private DataSet GetTicketData()
    {
        DataSet dsOpen = new DataSet();
        objpropMapData.ConnConfig = Session["config"].ToString();
        objpropMapData.Worker = ddlworker.SelectedValue;
        objpropMapData.Assigned = Convert.ToInt32(ddlStatus.SelectedValue);
        objpropMapData.Supervisor = ddlSuper.SelectedValue;
        if (Convert.ToString(Session["ISsupervisor"]) == "1")
        {
            objpropMapData.Supervisor = Session["username"].ToString();
        }
        objpropMapData.StartDate = RadScheduler1.VisibleRangeStart;
        objpropMapData.EndDate = RadScheduler1.VisibleRangeEnd;
        objpropMapData.Department = Convert.ToInt32(ddlDepartment.SelectedValue);
        objpropMapData.Category = ddlCategory.SelectedItem.Text == "All" ? string.Empty : ddlCategory.SelectedItem.Text;
        objpropMapData.LocTag = txtSearch.Text;
        dsOpen = objBL_MapData.GetOpenTicketScheduler(objpropMapData);
        ViewState["TicketsAppt"] = LoadTickets(dsOpen);
        return dsOpen;
    }

    private void FillDepartment()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getDepartment(objProp_User);

        ddlDepartment.DataSource = ds.Tables[0];
        ddlDepartment.DataTextField = "type";
        ddlDepartment.DataValueField = "id";
        ddlDepartment.DataBind();
        ddlDepartment.Items.Insert(0, new ListItem(":: All ::", "0"));
    }

    private void FillWorker()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.Supervisor = ddlSuper.SelectedValue;
        if (Convert.ToString(Session["ISsupervisor"]) == "1")
        {
            objProp_User.Supervisor = Session["username"].ToString();
        }
        objProp_User.IsTS = Session["MSM"].ToString();
        ds = objBL_User.getEMPScheduler(objProp_User);
        ddlworker.DataSource = ds.Tables[0];
        ddlworker.DataTextField = "fDesc";
        ddlworker.DataValueField = "fDesc";
        ddlworker.DataBind();
        ddlworker.Items.Insert(0, new ListItem(":: All ::", ""));

    }

    private void FillSupervisor()
    {

        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getSupervisor(objProp_User);

        ddlSuper.DataSource = ds.Tables[0];
        ddlSuper.DataTextField = "fuser";
        ddlSuper.DataValueField = "fuser";
        ddlSuper.DataBind();

        ddlSuper.Items.Insert(0, new ListItem(":: All ::", ""));
        string str = Session["username"].ToString();
        if (Convert.ToString(Session["ISsupervisor"]) == "1")
        {
            ddlSuper.SelectedValue = Session["username"].ToString().ToUpper();
            ddlSuper.Enabled = false;
        }
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
        ddlCategory.Items.Insert(0, new ListItem("All", ""));

    }

    //Add User Preferences for Scheduler
    protected void chkHideTicketDesc_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            addPreferences();
            GetPreferences();
        }
        catch (Exception ex) { throw ex; }
    }
    public void GetPreferences()
    {

        DataSet ds = new DataSet();
        objProp_User.DBName = Session["dbname"].ToString();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objProp_User.PreferenceID = 1;
        objProp_User.PageID = 2;
        ds = objBL_User.getPreferences(objProp_User);

        if (ds.Tables[0].Rows.Count > 0)
        {
            string st = ds.Tables[0].Rows[0]["Preferencevalue"] == DBNull.Value ? "0" : ds.Tables[0].Rows[0]["Preferencevalue"].ToString();
            chkHideTicketDesc.Checked = st == "1" ? true : false;
        }
        else
        {
            chkHideTicketDesc.Checked = false;
        }

    }

    public void GetPreferencesShedularDefaultView()
    {

        DataSet ds = new DataSet();
        objProp_User.DBName = Session["dbname"].ToString();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objProp_User.PreferenceID = 2;
        objProp_User.PageID = 2;
        ds = objBL_User.getPreferences(objProp_User);

        if (ds.Tables[0].Rows.Count > 0)
        {
            string st = ds.Tables[0].Rows[0]["Preferencevalue"] == DBNull.Value ? "0" : ds.Tables[0].Rows[0]["Preferencevalue"].ToString();
            if(st == "1")
            {
                RadScheduler1.SelectedView = SchedulerViewType.DayView;
            }
            if (st == "2")
            {
                RadScheduler1.SelectedView = SchedulerViewType.WeekView;
            }
            if (st == "3")
            {
                RadScheduler1.SelectedView = SchedulerViewType.MonthView;
            }
        }
        else
        {
            RadScheduler1.SelectedView = SchedulerViewType.DayView;
        }

    }
    public void addPreferences()
    {
        try
        {
            int val = chkHideTicketDesc.Checked ? 1 : 0;
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
            objProp_User.PreferenceID = 1;
            objProp_User.PageID = 2;
            objProp_User.PreferenceValues = val;
            objBL_User.AddPreferences(objProp_User);
        }
        catch { }
    }

    public void addPreferencesDefaultShedularView(int value)
    {
        try
        {
            int val = value;
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
            objProp_User.PreferenceID = 2;
            objProp_User.PageID = 2;
            objProp_User.PreferenceValues = val;
            objBL_User.AddPreferences(objProp_User);
        }
        catch { }
    }
    //

    public string ChargeableImage(string charge, string invoice, string manualinvoice, string QBinvoiceid)
    {
        string img = string.Empty;
        if (charge == "1")
        {
            img = "images/dollarRed.png";
        }
        if (charge == "0" && invoice != "0" && invoice.Trim() != "")
        {
            img = "images/dollar.png";
        }
        if (charge == "0" && manualinvoice.Trim() != "")
        {
            img = "images/dollar.png";
        }
        if (QBinvoiceid != "")
        {
            img = "images/dollarblue.png";
        }
        return img;
    }

    public string SignatureIcon(string ticketid, string workerid)
    {
        DataSet ds = new DataSet();
        int count = 0;
        string image = string.Empty;
        if (workerid.Trim() != string.Empty && ticketid.Trim() != string.Empty)
        {
            objpropMapData.ConnConfig = Session["config"].ToString();
            objpropMapData.TicketID = Convert.ToInt32(ticketid);
            objpropMapData.WorkID = Convert.ToInt32(workerid);
            ds = objBL_MapData.GetSignature(objpropMapData);

            if (ds.Tables[0].Rows.Count > 0)
            {
                count = Convert.ToInt32(ds.Tables[0].Rows[0]["signatureCount"].ToString());
                if (count > 0)
                {
                    count = 1;
                }

                if (count == 1)
                {
                    image = "images/Signature.png";
                }
            }
        }
        return image;
    }

    private void userpermissions()
    {
        if (Session["type"].ToString() != "c")
        {
            if (Session["type"].ToString() != "am")
            {
                objProp_User.ConnConfig = Session["config"].ToString();
                objProp_User.Username = Session["username"].ToString();
                objProp_User.PageName = "scheduler.aspx";
                DataSet dspage = objBL_User.getScreensByUser(objProp_User);
                if (dspage.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dspage.Tables[0].Rows[0]["access"].ToString()) == false)
                    {
                        //Response.Redirect("home.aspx");
                    }
                }
                else
                {
                    //Response.Redirect("home.aspx");
                }
            }
        }
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


            string TicketVoidPermission = ds.Rows[0]["TicketVoidPermission"] == DBNull.Value ? "N" : ds.Rows[0]["TicketVoidPermission"].ToString();

            hdnTicketVoidPermission.Value = TicketVoidPermission == "1" ? "Y" : "N";


            /// Ticket ///////////////////------->
            string ticketPermission = ds.Rows[0]["Ticket"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Ticket"].ToString();
            hdnViewTicket.Value = ticketPermission.Length < 4 ? "Y" : ticketPermission.Substring(3, 1);
            if (hdnViewTicket.Value == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
            string strTicketPermission = ds.Rows[0]["ticket"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["ticket"].ToString();
            hdnEditeTicket.Value = strTicketPermission.Length < 2 ? "Y" : strTicketPermission.Substring(1, 1);
            hdnDeleteTicket.Value = strTicketPermission.Length < 3 ? "Y" : strTicketPermission.Substring(2, 1);
            hdnAddeTicket.Value = strTicketPermission.Length < 1 ? "Y" : strTicketPermission.Substring(0, 1);

            if (hdnAddeTicket.Value == "N")
            {
                lnkAddticket.NavigateUrl = "";
                hdnAddeTicket.Visible = false;
            }
        }
        else
        {
            hdnAddeTicket.Value = "Y";
            hdnEditeTicket.Value = "Y";
            hdnDeleteTicket.Value = "Y";
            hdnViewTicket.Value = "Y";
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
    #endregion Bind Controle
    private DataSet GetBusinessTime()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getControl(objProp_User);
        return ds;
    }

    protected void RadScheduler1_DataBound(object sender, EventArgs e)
    {
        // Turn off the support for multiple resource values.
        //foreach (ResourceType resType in RadScheduler1.ResourceTypes)
        //{
        //    resType.AllowMultipleValues = false;
        //}
        RadToolTipManager1.TargetControls.Clear();



        //ScriptManager.RegisterStartupScript(this, typeof(Page), "HideToolTip", "hideActiveToolTip();", true);
    }

    protected void RadScheduler1_AppointmentCreated(object sender, AppointmentCreatedEventArgs e)
    {
        if (e.Appointment.Visible && !IsAppointmentRegisteredForTooltip(e.Appointment))
        {
            string id = e.Appointment.ID.ToString();

            //foreach (string domElementID in e.Appointment.DomElements)
            //{
            ////    RadToolTipManager1.TargetControls.Add(domElementID, id, true);


            //    string ttipValue = e.Appointment.ID.ToString() + "|" + e.Appointment.ID.ToString();
            //    this.RadToolTipManager1.TargetControls.Add(e.Appointment.ClientID, ttipValue, true);
            //}
        }

        if (e.Appointment != null)
        {
            if (e.Appointment.ID.ToString() != null)
            {
                #region Ticket Icon
                //Icon doc

                string Isdoc = "0";
                string Isalert = "0";
                string Iscredithold = "0";
                string isMS = "0";
                string isSignature = "0";
                string isConfirmed = "0";
                string isdollarRed = "0";
                string isdollarblue = "0";
                string isdollar = "0";
                string Isreview = "0";
                string isRecommendation = "0";
                Image ImgDocument = e.Container.FindControl("ImgDocument") as Image;
                string DocumentCount = e.Appointment.Attributes["DocumentCount"].ToString();
                if (DocumentCount != "0")
                {
                    Isdoc = "1";
                    ImgDocument.ImageUrl = "images/Document.png";
                    ImgDocument.Visible = true;
                    ImgDocument.ToolTip = "Documents";
                }

                // Icon Alert
                Image imgalert = e.Container.FindControl("imgalert") as Image;
                string dispalert = e.Appointment.Attributes["dispalert"].ToString();
                if (dispalert == "1")
                {
                    Isalert = "1";
                    imgalert.ImageUrl = "images/alert.png";
                    imgalert.Visible = true;
                    imgalert.ToolTip = "Dispatch Alert";
                }

                // Icon credithold
                string credithold = e.Appointment.Attributes["credithold"].ToString();
                Image imgcredithold = e.Container.FindControl("imgcredithold") as Image;
                if (credithold == "1")
                {
                    Iscredithold = "1";
                    imgcredithold.ImageUrl = "images/MSCreditHold.png";
                    imgcredithold.Visible = true;
                    imgcredithold.ToolTip = "Credit Hold";
                }

                // Icon MS
                string comp = e.Appointment.Attributes["comp"].ToString();
                Image imgMS = e.Container.FindControl("imgMS") as Image;
                if (comp == "2")
                {
                    isMS = "1";
                    imgMS.ImageUrl = "images/1331034893_pda.png";
                    imgMS.Visible = true;
                    imgMS.ToolTip = "MS";
                }


                // icon Confirmed

                string Confirmed = e.Appointment.Attributes["Confirmed"].ToString();
                Image imgConfirmed = e.Container.FindControl("imgConfirmed") as Image;
                if (Confirmed == "1")
                {
                    isConfirmed = "1";
                    imgConfirmed.ImageUrl = "images/1331036429_Check.png";
                    imgConfirmed.Visible = true;
                    imgConfirmed.ToolTip = "Confirmed";
                }


                string clearcheck = e.Appointment.Attributes["clearcheck"].ToString();
                Image imgreview = e.Container.FindControl("imgreview") as Image;
                if (clearcheck == "1")
                {
                    Isreview = "1";
                    imgreview.ImageUrl = "images/review.png";
                    imgreview.Visible = true;
                    imgreview.ToolTip = "Reviewed";
                }

                ///  Recommendation  

                string RecommendationIMG = e.Appointment.Attributes["Recommendation"].ToString();
                Image ImgRecommendation = e.Container.FindControl("ImgRecommendation") as Image;
                if (RecommendationIMG != "0")
                {
                    isRecommendation = "1";
                    ImgRecommendation.ImageUrl = "images/thumb_up.png";
                    ImgRecommendation.Visible = true;
                    ImgRecommendation.ToolTip = "Recommendation";
                }


                // Img Signature

                string SignatureIMG = e.Appointment.Attributes["SignatureIMG"].ToString();
                Image ImgSignature = e.Container.FindControl("ImgSignature") as Image;
                if (SignatureIMG != string.Empty)
                {
                    isSignature = "1";
                    ImgSignature.ImageUrl = SignatureIMG;
                    ImgSignature.Visible = true;
                    ImgSignature.ToolTip = "Signature";
                }

                // Img  Chargeable

                string charge = e.Appointment.Attributes["charge"].ToString();

                string invoice = e.Appointment.Attributes["invoice"].ToString();

                string manualinvoice = e.Appointment.Attributes["manualinvoice"].ToString();

                string qbinvoiceid = e.Appointment.Attributes["qbinvoiceid"].ToString();

                Image ImgChargeable = e.Container.FindControl("ImgChargeable") as Image;
                string Chargeableurl = ChargeableImage(charge, invoice, manualinvoice, qbinvoiceid);
                if (Chargeableurl != string.Empty)
                {
                    if (Chargeableurl == "images/dollarRed.png") { isdollarRed = "1"; }
                    if (Chargeableurl == "images/dollarblue.png") { isdollarblue = "1"; }
                    if (Chargeableurl == "images/dollar.png") { isdollar = "1"; }
                    ImgChargeable.ImageUrl = Chargeableurl;
                    ImgChargeable.Visible = true;
                    ImgChargeable.ToolTip = "Chargeable";
                }



                string TicketID = e.Appointment.ID.ToString();
                HyperLink HyperLink1 = e.Container.FindControl("HLTicketID") as HyperLink;
                HyperLink1.Text = TicketID;
                HyperLink1.NavigateUrl = "AddTicket.aspx?id=" + TicketID + "&comp=" + comp;
                if (hdnAddeTicket.Value == "N") { HyperLink1.Enabled = false; }
                //Location
                string Location = e.Appointment.Attributes["Location"].ToString();

                Label lblLocation = e.Container.FindControl("lblLocation") as Label;
                lblLocation.Text = Location;

                //Category and icon
                string Category = e.Appointment.Attributes["cat"].ToString();

                Label lblCategory = e.Container.FindControl("lblCategory") as Label;

                lblCategory.Text = Category;

                Image imgCategory = e.Container.FindControl("imgCategory") as Image;

                imgCategory.Attributes.Add("src", "imagehandler.ashx?catid=" + Category);

                //Location
                string City = e.Appointment.Attributes["City"].ToString();

                Label lblCity = e.Container.FindControl("lblCity") as Label;
                lblCity.Text = City;



                string address = e.Appointment.Attributes["address"].ToString();
                address = Regex.Replace(address, @"[^0-9a-zA-Z]+", ",");

                if (address.Length > 500) address = address.Substring(0, 500) + "....";
                string edate = e.Appointment.Attributes["edate"].ToString();

                string fdesc = e.Appointment.Attributes["fdesc"].Trim().ToString();
                fdesc = Regex.Replace(fdesc, @"[^0-9a-zA-Z]+", ",");
                if (fdesc.Length > 500) fdesc = fdesc.Substring(0, 500) + "....";

                string descres = e.Appointment.Attributes["descres"].Trim().ToString();
                descres = Regex.Replace(descres, @"[^0-9a-zA-Z]+", ",");
                if (descres.Length > 500) descres = descres.Substring(0, 500) + "....";

                string WorkOrder = e.Appointment.Attributes["WorkOrder"].ToString();
                string assigned = e.Appointment.Attributes["assigned"].ToString();
                string phone = e.Appointment.Attributes["phone"].ToString();

                //Customer
                string Customer = e.Appointment.Attributes["Customer"].ToString();

                Label lblCustomer = e.Container.FindControl("lblCustomer") as Label;
                lblCustomer.Text = Customer;



                string NKimgicon =
                  Isdoc +
                  Isalert +
                  Iscredithold +
                  isMS +
                  isSignature +
                  isConfirmed +
                  isdollarRed +
                  isdollarblue +
                  isdollar +
                  Isreview + isRecommendation;

                if (assigned == "0") assigned = "Unassigned";
                if (assigned == "1") assigned = "Assigned";
                if (assigned == "2") assigned = "Enroute";
                if (assigned == "3") assigned = "Onsite";
                if (assigned == "4") assigned = "Completed";
                if (assigned == "5") assigned = "Hold";

                string ttipValue = NKimgicon + "NK|" +
                    e.Appointment.ID.ToString() + "NK|" +
                    Location.ToString() + "NK|" +
                    Customer.ToString() + "NK|" +
                    Category.ToString() + "NK|" +
                    address.ToString() + "NK|" +
                    City.ToString() + "NK|" +
                    phone.ToString() + "NK|" +
                    edate.ToString() + "NK|" +
                    fdesc.ToString() + "NK|" +
                    descres.ToString() + "NK|" +
                    WorkOrder.ToString() + "NK|" +
                    assigned.ToString() + "NK|"
                    ;

                this.RadToolTipManager1.TargetControls.Add(e.Appointment.ClientID, ttipValue, true);

            }
            #endregion
        }
    }

    protected void RadScheduler1_AppointmentDataBound(object sender, SchedulerEventArgs e)
    {
        //Label lblTicketID = e.Container.FindControl("lblTicketID") as Label;
        //Panel AppointmentPanel = e.Container.FindControl("AppointmentPanel") as Panel;



        if (e.Appointment != null)
        {
            if (e.Appointment.ID.ToString() != "")
            {



                string apptid = e.Appointment.ID.ToString();
                List<Appointment> appts = GetTicketsAppointment();
                Appointment appt = null;
                foreach (Appointment item in appts)
                {
                    if (Convert.ToString(item.ID) == apptid)
                    {
                        appt = item;
                        break;
                    }
                }
                #region Ticket Attributes 




                string workerid = appt.Attributes["workerid"].ToString();
                e.Appointment.Attributes.Add("workerid", appt.Attributes["workerid"].ToString());

                string assigned = appt.Attributes["assigned"].ToString();

                e.Appointment.Attributes.Add("assigned", appt.Attributes["assigned"].ToString());

                string comp = appt.Attributes["comp"].ToString();
                e.Appointment.Attributes.Add("comp", appt.Attributes["comp"].ToString());

                string DocumentCount = appt.Attributes["DocumentCount"].ToString();
                e.Appointment.Attributes.Add("DocumentCount", appt.Attributes["DocumentCount"].ToString());

                string Recommendation = appt.Attributes["Recommendation"].ToString();
                e.Appointment.Attributes.Add("Recommendation", appt.Attributes["Recommendation"].ToString());




                string dispalert = appt.Attributes["dispalert"].ToString();
                e.Appointment.Attributes.Add("dispalert", appt.Attributes["dispalert"].ToString());

                string credithold = appt.Attributes["credithold"].ToString();
                e.Appointment.Attributes.Add("credithold", appt.Attributes["credithold"].ToString());

                string Confirmed = appt.Attributes["Confirmed"].ToString();
                e.Appointment.Attributes.Add("Confirmed", appt.Attributes["Confirmed"].ToString());

                string clearcheck = appt.Attributes["clearcheck"].ToString();
                e.Appointment.Attributes.Add("clearcheck", appt.Attributes["clearcheck"].ToString());

                string color = appt.Attributes["color"].ToString();
                e.Appointment.Attributes.Add("color", appt.Attributes["color"].ToString());

                string cat = appt.Attributes["cat"].ToString();
                e.Appointment.Attributes.Add("cat", appt.Attributes["cat"].ToString());

                string charge = appt.Attributes["charge"].ToString();
                e.Appointment.Attributes.Add("charge", appt.Attributes["charge"].ToString());

                string invoice = appt.Attributes["invoice"].ToString();
                e.Appointment.Attributes.Add("invoice", appt.Attributes["invoice"].ToString());

                string manualinvoice = appt.Attributes["manualinvoice"].ToString();
                e.Appointment.Attributes.Add("manualinvoice", appt.Attributes["manualinvoice"].ToString());

                string qbinvoiceid = appt.Attributes["qbinvoiceid"].ToString();
                e.Appointment.Attributes.Add("qbinvoiceid", appt.Attributes["qbinvoiceid"].ToString());

                string SignatureIMG = SignatureIcon(e.Appointment.ID.ToString(), workerid);
                e.Appointment.Attributes.Add("SignatureIMG", SignatureIMG);

                string assignname = appt.Attributes["assignname"].ToString();
                e.Appointment.Attributes.Add("assignname", assignname);

                string address = appt.Attributes["address"].ToString();
                e.Appointment.Attributes.Add("address", address);

                string phone = appt.Attributes["phone"].ToString();
                e.Appointment.Attributes.Add("phone", phone);

                string edate = appt.Attributes["edate"].ToString();
                e.Appointment.Attributes.Add("edate", edate);

                string fdesc = appt.Attributes["fdesc"].ToString();
                e.Appointment.Attributes.Add("fdesc", fdesc);

                string descres = appt.Attributes["descres"].ToString();
                e.Appointment.Attributes.Add("descres", descres);

                string Customer = appt.Attributes["Customer"].ToString();
                e.Appointment.Attributes.Add("Customer", Customer);


                string Location = appt.Attributes["Location"].ToString();
                e.Appointment.Attributes.Add("Location", Location);

                string City = appt.Attributes["City"].ToString();
                e.Appointment.Attributes.Add("City", City);

                string WorkOrder = appt.Attributes["WorkOrder"].ToString();
                e.Appointment.Attributes.Add("WorkOrder", WorkOrder);

                #endregion Ticket Attributes


                //if (assigned == "1") e.Appointment.BackColor = System.Drawing.Color.WhiteSmoke;
                //if (assigned == "2") e.Appointment.BackColor = System.Drawing.Color.FromArgb(158, 247, 103);
                //if (assigned == "3") e.Appointment.BackColor = System.Drawing.Color.Orange;
                //if (assigned == "4") e.Appointment.BackColor = System.Drawing.Color.DeepSkyBlue;
                //if (assigned == "5") e.Appointment.BackColor = System.Drawing.Color.Yellow;

                // Ticket Background Color Accordingly to Ticket Status

                if (assigned == "1") e.Appointment.CssClass = "tckt-one";
                if (assigned == "2") e.Appointment.CssClass = "tckt-two";
                if (assigned == "3") e.Appointment.CssClass = "tckt-three";
                if (assigned == "4") e.Appointment.CssClass = "tckt-four";
                if (assigned == "5") e.Appointment.CssClass = "tckt-Five";
                if (assigned == "6") e.Appointment.CssClass = "tckt-Five";

                e.Appointment.ForeColor = System.Drawing.Color.Black;
                e.Appointment.BorderColor = System.Drawing.Color.Black;
                e.Appointment.BorderStyle = BorderStyle.Solid;
                e.Appointment.BorderWidth = Unit.Pixel(2);



                //RadSchedulerContextMenu menu = new RadSchedulerContextMenu();
                //RadMenuItem radMenuItem = new RadMenuItem();
                //radMenuItem.Text = "Open";
                //menu.Items.Add(radMenuItem);
                //RadScheduler1.AppointmentContextMenus.Add(menu);
            }

        }



    }

    private bool IsAppointmentRegisteredForTooltip(Appointment apt)
    {
        foreach (ToolTipTargetControl targetControl in RadToolTipManager1.TargetControls)
        {
            if (apt.DomElements.Contains(targetControl.TargetControlID))
            {
                return true;
            }
        }

        return false;
    }

    protected void RadScheduler1_ResourceHeaderCreated(object sender, ResourceHeaderCreatedEventArgs e)
    {
        Panel ResourceImageWrapper = e.Container.FindControl("ResourceImageWrapper") as Panel;
        ResourceImageWrapper.CssClass = "Resource" + e.Container.Resource.Key.ToString();
        Image img = e.Container.FindControl("SpeakerImage") as Image;
        var resources = RadScheduler1.Resources.GetResourcesByType("workerProfileImage");
        var image = resources.FirstOrDefault(t => t.Key.Equals(e.Container.Resource.Key));

        Panel SpeakerImageDiv = e.Container.FindControl("SpeakerImageDiv") as Panel;

        if (image != null && !string.IsNullOrWhiteSpace(image.Text))
        {
            img.ImageUrl = image.Text;
        }
        else
        {
            img.ImageUrl = "images/User.png";
        }

        if (RadScheduler1.SelectedView == SchedulerViewType.WeekView) { SpeakerImageDiv.Visible = img.Visible = false; } else { SpeakerImageDiv.Visible = img.Visible = true; }

        img.ToolTip = e.Container.Resource.Text;
        ((Label)e.Container.FindControl("ResourceLabel")).Text = e.Container.Resource.Text;
        ResourceImageWrapper.BackColor = System.Drawing.Color.WhiteSmoke;
        if (e.Container.Resource.Text == "") { img.Visible = false; }
    }

    protected void ddlSuper_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillWorker();
        RadSchedulerRebind();
    }

    protected void ddlworker_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadSchedulerRebind();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        RadGrid1.Rebind();
        RadSchedulerRebind(true, txtSearch.Text);
        //RadGridRebind();
    }

    private List<Appointment> LoadTickets(DataSet Tickets)
    {
        List<Appointment> appts = new List<Appointment>();
        if (Tickets.Tables.Count > 0)
        {
            if (Tickets.Tables[0].Rows.Count > 0)
            {
                DataTable dt = Tickets.Tables[0];
                int countRow = dt.Rows.Count;
                for (int iRow = 0; iRow < countRow; iRow++)
                {
                    #region  Appointment

                    int id = Convert.ToInt32(dt.Rows[iRow]["id"]);
                    int workerid = Convert.ToInt32(dt.Rows[iRow]["workerid"] == DBNull.Value ? "0" : dt.Rows[iRow]["workerid"]);
                    DateTime start = Convert.ToDateTime(dt.Rows[iRow]["start1"] == DBNull.Value ? DateTime.Now.ToString() : dt.Rows[iRow]["start1"].ToString());
                    DateTime end = Convert.ToDateTime(dt.Rows[iRow]["End1"] == DBNull.Value ? DateTime.Now.ToString() : dt.Rows[iRow]["End1"].ToString());

                    string NAME = dt.Rows[iRow]["NAME"].ToString();
                    string assigned = dt.Rows[iRow]["assigned"].ToString();
                    string comp = dt.Rows[iRow]["comp"].ToString();
                    string DocumentCount = dt.Rows[iRow]["DocumentCount"].ToString();
                    string dispalert = dt.Rows[iRow]["dispalert"].ToString();
                    string credithold = dt.Rows[iRow]["credithold"].ToString();
                    string Confirmed = dt.Rows[iRow]["Confirmed"].ToString();
                    string clearcheck = dt.Rows[iRow]["clearcheck"].ToString();
                    string color = dt.Rows[iRow]["color"].ToString();
                    string cat = dt.Rows[iRow]["cat"].ToString();
                    string charge = dt.Rows[iRow]["charge"].ToString();
                    string invoice = dt.Rows[iRow]["invoice"].ToString();
                    string manualinvoice = dt.Rows[iRow]["manualinvoice"].ToString();
                    string qbinvoiceid = dt.Rows[iRow]["qbinvoiceid"].ToString();
                    string assignname = dt.Rows[iRow]["assignname"].ToString();
                    string address = dt.Rows[iRow]["address"].ToString();
                    string phone = dt.Rows[iRow]["phone"].ToString();
                    string edate = dt.Rows[iRow]["edate"].ToString();
                    string fdesc = dt.Rows[iRow]["fdesc"].ToString();
                    string descres = dt.Rows[iRow]["descres"].ToString();
                    string Customer = dt.Rows[iRow]["Customer"].ToString();
                    string Location = dt.Rows[iRow]["Location"].ToString();
                    string City = dt.Rows[iRow]["City"].ToString();
                    string WorkOrder = dt.Rows[iRow]["WorkOrder"].ToString();
                    string Recommendation = dt.Rows[iRow]["Recommendation"].ToString();

                    Appointment appt = new Appointment(id, start, end, NAME);
                    appt.Attributes.Add("id", id.ToString());
                    appt.Attributes.Add("start", start.ToString());
                    appt.Attributes.Add("End", end.ToString());
                    appt.Attributes.Add("workerid", workerid.ToString());
                    appt.Attributes.Add("NAME", NAME);
                    appt.Attributes.Add("assigned", assigned);
                    appt.Attributes.Add("comp", comp);
                    appt.Attributes.Add("DocumentCount", DocumentCount);
                    appt.Attributes.Add("dispalert", dispalert);
                    appt.Attributes.Add("credithold", credithold);
                    appt.Attributes.Add("Confirmed", Confirmed);
                    appt.Attributes.Add("clearcheck", clearcheck);
                    appt.Attributes.Add("color", color);
                    appt.Attributes.Add("cat", cat);
                    appt.Attributes.Add("charge", charge);
                    appt.Attributes.Add("invoice", invoice);
                    appt.Attributes.Add("manualinvoice", manualinvoice);
                    appt.Attributes.Add("qbinvoiceid", manualinvoice);
                    appt.Attributes.Add("assignname", assignname);
                    appt.Attributes.Add("address", address);
                    appt.Attributes.Add("phone", phone);
                    appt.Attributes.Add("edate", edate);
                    appt.Attributes.Add("fdesc", fdesc);
                    appt.Attributes.Add("descres", descres);
                    appt.Attributes.Add("Customer", Customer);
                    appt.Attributes.Add("Location", Location);
                    appt.Attributes.Add("City", City);
                    appt.Attributes.Add("WorkOrder", WorkOrder);
                    appt.Attributes.Add("Recommendation", Recommendation);
                    appts.Add(appt);


                    #endregion


                }
            }
        }

        return appts;
    }

    private List<Appointment> GetTicketsAppointment()
    {
        List<Appointment> appts = new List<Appointment>();
        if (ViewState["TicketsAppt"] != null)
            appts = (List<Appointment>)ViewState["TicketsAppt"];
        return appts;
    }


    protected void TicketDelete_Click(object sender, EventArgs e)
    {

        if (hdnDeleteTicketId.Value != null & hdnDeleteTicketId.Value != "")
        {
            if (hdnDeleteTicket.Value == "N")
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "DeleteTicket123", "noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue: true });", true);
            }
            else if (Session["MSM"].ToString() != "TS")
            {
                DeleteTicket(Convert.ToInt32(hdnDeleteTicketId.Value));
                RadSchedulerRebind();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "DeleteTicket", "noty({ text: 'Ticket#" + hdnDeleteTicketId.Value + " deleted.', type: 'success', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue: true });", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "DeleteTicketts123", "noty({ text: 'Please delete ticket from Total Service.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue: true });", true);
            }

        }
        hdnDeleteTicketId.Value = "";
    }

    private void DeleteTicket(int TicketID)
    {
        objpropMapData.ConnConfig = Session["config"].ToString();
        objpropMapData.TicketID = TicketID;
        objpropMapData.Worker = Session["username"].ToString();
        objBL_MapData.DeleteTicket(objpropMapData);
    }

    protected void RadScheduler1_TimeSlotContextMenuItemClicked(object sender, TimeSlotContextMenuItemClickedEventArgs e)
    {
        string MenuItem = e.MenuItem.Value;
        if (MenuItem != null)
        {
            string URL = string.Empty;
            if (MenuItem == "Open")
            {

                if (hdnAddeTicket.Value == "N")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "PermissionaddTicket646", "noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue: true });", true);
                }
                else
                {
                    string Start = e.StartSlot.Start.ToString();
                    string End = e.EndSlot.End.ToString();
                    string User = e.TimeSlot.Resource.Text;
                    string url = "addticket.aspx?timer=1&start=" + Start + "&end=" + End + "&r=" + User;
                    URL = "window.open('" + url + "', '_blank');";
                }
            }
            if (MenuItem == "Refresh")
            {
                RadSchedulerRebind();
            }
            if (MenuItem == "Go to today")
            {
                RadSchedulerRebind();
            }
            if (e.MenuItem.Text == "Show 24 hours..." && MenuItem == "CommandShow24Hours")
            {
                e.MenuItem.Text = "Show business hours...";
                ViewState["DayStartTime"] = new TimeSpan(01, 0, 0);
                ViewState["DayEndTime"] = new TimeSpan(23, 0, 0);


            }
            else if (e.MenuItem.Text == "Show business hours..." && MenuItem == "CommandShow24Hours")
            {
                e.MenuItem.Text = "Show 24 hours...";
                ViewState["DayStartTime"] = ViewState["WorkDayStartTime"];
                ViewState["DayEndTime"] = ViewState["WorkDayEndTime"];

            }
            if (URL != string.Empty)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "TimeSlotTicketURL", URL, true);
            }
        }
    }

    protected void RadScheduler1_FormCreating(object sender, SchedulerFormCreatingEventArgs e)
    {
        e.Cancel = true;
        string URL = string.Empty;
        if (e.Appointment.ID != null)
        {
            string apptid = e.Appointment.ID.ToString();
            List<Appointment> appts = GetTicketsAppointment();
            Appointment appt = null;
            foreach (Appointment item in appts)
            {
                if (Convert.ToString(item.ID) == apptid)
                {
                    appt = item;
                    break;
                }
            }
            if (apptid != null && appt != null)
            {
                if (hdnEditeTicket.Value == "N")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "PermissionaddTicket646", "noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue: true });", true);
                }
                else
                {
                    string assigned = appt.Attributes["assigned"].ToString();
                    string comp = appt.Attributes["comp"].ToString();
                    URL = "window.open('addticket.aspx?id=" + apptid + "&comp=" + comp + "', '_blank');";
                }

            }
        }
        else if (e.Appointment.Start != null && e.Appointment.End != null)
        {
            if (hdnAddeTicket.Value == "N")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PermissionaddTicket646", "noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue: true });", true);
            }
            else
            {
                Resource rs = e.Appointment.Resources.GetResourceByType("worker");
                string Start = e.Appointment.Start.ToString();
                string End = e.Appointment.End.ToString();
                string User = rs.Text;
                string url = "addticket.aspx?timer=1&start=" + Start + "&end=" + End + "&r=" + User;
                URL = "window.open('" + url + "', '_blank');";
            }

        }
        if (URL != string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "TicketURL", URL, true);
        }
    }

    protected void RadScheduler1_NavigationComplete(object sender, SchedulerNavigationCompleteEventArgs e)
    {

        RadSchedulerRebind();
    }

    #region DragAndDrop
    protected void RadGrid1_RowDrop(object sender, GridDragDropEventArgs e)
    {
        GridDataItem dataItem = e.DraggedItems[0];

        Hashtable values = new Hashtable();
        dataItem.ExtractValues(values);

        int id = (int)dataItem.GetDataKeyValue("id");

        string targetSlotIndex = TargetSlotHiddenField.Value;

        var index = targetSlotIndex.Split(':')[0];

        if (targetSlotIndex != string.Empty)
        {
            HandleSchedulerDrop(id, index, targetSlotIndex);
            TargetSlotHiddenField.Value = string.Empty;
        }

        // ScriptManager.RegisterStartupScript(this, Page.GetType(), "trigger", "document.getElementById('ctl00_ContentPlaceHolder1_lnkTicketDragDrop').click();", true);


    }

    private void HandleSchedulerDrop(int id, string Index, string targetSlotIndex)
    {

        RadScheduler1.Rebind();

        ISchedulerTimeSlot slot = RadScheduler1.GetTimeSlotFromIndex(targetSlotIndex);
        // var appt = RadScheduler1.Appointments.GetAppointmentsInRange(slot.Start, slot.End).;
        Resource rs = RadScheduler1.Resources[Convert.ToInt16(Index) * 2];

        TimeSpan duration = TimeSpan.FromHours(1);
        if (slot.Duration == TimeSpan.FromDays(1))
        {
            duration = slot.Duration;
        }

        ScheduleAppointment(id, rs, slot.Start, slot.Start.Add(duration), duration);


    }


    private void ScheduleAppointment(int UnassignedCallsID, Resource Worker, DateTime start, DateTime end, TimeSpan duration)
    {
        if (RadScheduler1.SelectedView == SchedulerViewType.DayView || RadScheduler1.SelectedView == SchedulerViewType.WeekView || RadScheduler1.SelectedView == SchedulerViewType.MonthView)
        {
            string Assigndate = hdnAssigndate.Value;

            string Assignworker = hdnAssignworker.Value;


            if (UnassignedCallsID != 0 && Assignworker != null && start != null && end != null)
            {
                #region Simulation of database update

                if (Assignworker != "")
                {
                    objpropMapData.ConnConfig = Session["config"].ToString();
                    objpropMapData.Tech = Assignworker;
                    objpropMapData.TicketID = Convert.ToInt32(UnassignedCallsID);
                    objpropMapData.Date = start;
                    objpropMapData.Assigned = 1;
                    objBL_MapData.UpdateTicket(objpropMapData);

                    objpropMapData.Resize = duration.TotalHours;
                    objBL_MapData.UpdateTicketResize(objpropMapData);
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMoved", "noty({ text: 'Ticket assigned to " + Worker.Text + ".', type: 'success', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue: true });", true);
                RadSchedulerRebind();
                RadGrid1.Rebind();
                #endregion
            }
        }

        TargetSlotHiddenField.Value = string.Empty;
        hdnAssignworker.Value = string.Empty;
        hdnAssigndate.Value = string.Empty;
    }

    private static bool OnDataSourceOperationComplete(int count, Exception e)
    {
        if (e != null)
        {
            throw e;
        }
        return true;
    }

    protected void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
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

    private void FillUnassignedCalls()
    {
        DataSet ds = new DataSet();
        ds = GetUnassignedCalls();

        if (ds.Tables[0].Rows.Count > 0)
        {
            RadGrid1.VirtualItemCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            RadGrid1.DataSource = ds.Tables[0];
            RadSlidingPane1.Title = "Unassigned Calls (" + RadGrid1.VirtualItemCount + ") ";

        }
        else
        {
            RadGrid1.VirtualItemCount = 0;
            RadGrid1.DataSource = null;
            RadSlidingPane1.Title = "Unassigned Calls (0)";
        }
    }

    private DataSet GetUnassignedCalls()
    {

        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.SearchValue = txtSearch.Text;

        objProp_User.SearchValueUnAssignedCalls = txtSearchUnassignedTicket.Text;

        objProp_User.CategoryName = ddlCategory.SelectedItem.Text == "All" ? string.Empty : ddlCategory.SelectedItem.Text;

        objProp_User.Department = ddlDepartment.SelectedItem.Text == ":: All ::" ? string.Empty : ddlDepartment.SelectedItem.Text;

        objProp_User.DepartmentID = ddlDepartment.SelectedItem.Text == ":: All ::" ? 0 : Convert.ToInt16(ddlDepartment.SelectedValue);

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

        ds = getUnassignedCallsPaging(objProp_User, RadGrid1.CurrentPageIndex, RadGrid1.PageSize);
        DataTable dt = new DataTable();
        if (ds.Tables[0].Select("Assigned > 0").Count() > 0)
        {
            dt = ds.Tables[0].Select("Assigned > 0").CopyToDataTable();
        }
        Session["dsOpenCall"] = dt;
        return ds;
    }

    #endregion




    protected void OnAjaxUpdate(object sender, ToolTipUpdateEventArgs e)
    {
        int aptId;
        Appointment apt;
        if (!int.TryParse(e.Value, out aptId))//The appointment is occurrence and FindByID expects a string
            apt = RadScheduler1.Appointments.FindByID(e.Value);
        else //The appointment is not occurrence and FindByID expects an int
            apt = RadScheduler1.Appointments.FindByID(aptId);

        AppointmentToolTip toolTip = (AppointmentToolTip)LoadControl("AppointmentToolTip.ascx");
        toolTip.ID = "UcAppointmentTooltip1";
        toolTip.TargetAppointment = apt;
        e.UpdatePanel.ContentTemplateContainer.Controls.Add(toolTip);
    }

    protected void RadScheduler1_TimeSlotCreated(object sender, TimeSlotCreatedEventArgs e)
    {
        RadScheduler scheduler = (RadScheduler)sender;
        if (scheduler.SelectedView == SchedulerViewType.DayView)
        {
            if (e.TimeSlot.Start.Month == DateTime.Now.Month && e.TimeSlot.Start.Day == DateTime.Now.Day && e.TimeSlot.Start.Hour == DateTime.Now.Hour && e.TimeSlot.Start.Minute == 0 && DateTime.Now.Minute < 30)
            {
                //Set the CssClass property to visually distinguish your current  TimeSlot.
                e.TimeSlot.CssClass = "TimeNow1";
            }

            if (e.TimeSlot.Start.Month == DateTime.Now.Month && e.TimeSlot.Start.Day == DateTime.Now.Day && e.TimeSlot.Start.Hour == DateTime.Now.Hour && e.TimeSlot.Start.Minute == 30 && DateTime.Now.Minute >= 30)
            {
                //Set the CssClass property to visually distinguish your current  TimeSlot.
                e.TimeSlot.CssClass = "TimeNow2";
            }
        }


    }

    protected void btnToggle_Click(object sender, EventArgs e)
    {
        RadSchedulerRebind();
    }

    public void Timer1_Tick(object sender, EventArgs e)
    {
        RadSchedulerRebind();
    }

    private void BindTimeZones()
    {

    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid1.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        FillUnassignedCalls();

    }

    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid1.MasterTableView.FilterExpression != "" ||
            (RadGrid1.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid1.MasterTableView.SortExpressions.Count > 0;
    }

    protected void btnSearchUnassignedTicket_Click(object sender, EventArgs e)
    {
        RadGrid1.Rebind();
    }

    //protected void AppointmentMove()
    //{
    //    try
    //    {
    //        if (RadScheduler1.SelectedView == SchedulerViewType.DayView || RadScheduler1.SelectedView == SchedulerViewType.WeekView || RadScheduler1.SelectedView == SchedulerViewType.MonthView)
    //        {
    //            string MoveTicketId = hdnMoveTicketId.Value;
    //            string MoveNewTimeSlotIndex = hdnMoveNewTimeSlotIndex.Value;
    //            var NewTimeSlot = hdnStartTime.Value;

    //            //if (hdnMoveTicketId.Value != "" && hdnMoveNewTimeSlotIndex.Value != "" && hdnCheckActionScheduler.Value== "NKAppointmentMove")
    //            if (hdnMoveTicketId.Value != "" && hdnMoveNewTimeSlotIndex.Value != "")
    //            {

    //                ISchedulerTimeSlot slot = RadScheduler1.GetTimeSlotFromIndex(MoveNewTimeSlotIndex);                    

    //                var MoveNewTimeSlot = (DateTime.TryParseExact(NewTimeSlot, "M/d/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var dt)) ? dt : null as DateTime?;                    
    //                var index = MoveNewTimeSlotIndex.Split(':')[0];

    //                Resource rs = RadScheduler1.Resources[Convert.ToInt16(index) * 2];

    //                TimeSpan duration = TimeSpan.FromHours(1);
    //                if (slot.Duration == TimeSpan.FromDays(1))
    //                {
    //                    duration = slot.Duration;
    //                }

    //                #region Simulation of database update
    //                if (rs.Text != null)
    //                {
    //                    objpropMapData.ConnConfig = Session["config"].ToString();
    //                    objpropMapData.Tech = rs.Text;
    //                    objpropMapData.TicketID = Convert.ToInt32(MoveTicketId);
    //                    objpropMapData.Date = Convert.ToDateTime(MoveNewTimeSlot);
    //                    //objpropMapData.Date = slot.Start;
    //                    objpropMapData.Assigned = 1;
    //                    objBL_MapData.UpdateTicket(objpropMapData);
    //                }
    //                #endregion

    //                hdnCheckActionScheduler.Value = hdnMoveTicketId.Value = hdnMoveNewTimeSlotIndex.Value = "";
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}


    //protected void btnAppointmentResize_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (RadScheduler1.SelectedView == SchedulerViewType.DayView || RadScheduler1.SelectedView == SchedulerViewType.WeekView)
    //        {
    //            string ResizeTicketId = hdnResizeTicketId.Value;
    //            string ResizeNewStartDate = hdnResizeNewStartDate.Value;
    //            string ResizeNewEndDate = hdnResizeNewEndDate.Value;
    //            string ResizeNewTimeSlotIndex = hdnResizeNewTimeSlotIndex.Value;

    //            if (hdnResizeTicketId.Value != "" && hdnResizeNewStartDate.Value != "" && hdnResizeNewEndDate.Value != "" && hdnResizeNewTimeSlotIndex.Value != "")
    //            {
    //                DateTime Start = ConvertInTOLocaldate(ResizeNewStartDate);
    //                DateTime End = ConvertInTOLocaldate(ResizeNewEndDate);
    //                if (End > Start)
    //                {
    //                    TimeSpan duration = End - Start;


    //                    #region Simulation of database update

    //                    objpropMapData.ConnConfig = Session["config"].ToString();
    //                    objpropMapData.TicketID = Convert.ToInt32(ResizeTicketId);
    //                    objpropMapData.Date = Convert.ToDateTime(Start);
    //                    objpropMapData.Resize = duration.TotalHours;
    //                    objBL_MapData.UpdateTicketResize(objpropMapData);

    //                    #endregion
    //                    // ScriptManager.RegisterStartupScript(this, this.GetType(), "TickeResize1", "noty({ text: 'Ticket resized.', type: 'success', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue: true });", true);
    //                    //RadSchedulerRebind();
    //                    hdnResizeTicketId.Value = hdnResizeNewStartDate.Value = hdnResizeNewEndDate.Value = hdnResizeNewTimeSlotIndex.Value = "";
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}


    public DataSet getUnassignedCallsPaging(User objPropUser, int STARTROWINDEX, int MAXIMUMROWS = 10)
    {

        STARTROWINDEX = STARTROWINDEX <= 0 ? 0 : STARTROWINDEX;
        StringBuilder varname1 = new StringBuilder();
        varname1.Append(";WITH FINALTICKETDATA  AS (SELECT ROW_NUMBER() OVER(ORDER BY ID) AS ROWNO,* FROM( \n");
        varname1.Append("SELECT t.id, \n");
        varname1.Append("                CASE \n");
        varname1.Append("                  WHEN t.Owner IS NULL THEN LDesc2 \n");
        varname1.Append("                  ELSE (SELECT top 1 l.Tag FROM   Loc l  WHERE  l.Loc = t.LID) \n");
        varname1.Append("                END                                                        AS ldesc1, \n");
        varname1.Append("                cdate, \n");
        varname1.Append("                edate, \n");
        varname1.Append("                ( LDesc3 + ', ' + t.City + ', ' + t.State + ', ' + t.Zip ) AS address, \n");
        varname1.Append("                t.cat, \n");
        varname1.Append("                r.EN, \n");
        varname1.Append("                Isnull(B.Name, '')       As Company, \n");
        varname1.Append("                Isnull(t.high, 0)                                          AS high, \n");
        varname1.Append("                (SELECT r.Lat \n");
        varname1.Append("                 FROM   Rol r \n");
        varname1.Append("                        INNER JOIN Loc l \n");
        varname1.Append("                                ON l.Rol = r.ID \n");
        varname1.Append("                 WHERE  l.Loc = t.LID)                                     AS lat, \n");
        varname1.Append("                (SELECT r.Lng \n");
        varname1.Append("                 FROM   Rol r \n");
        varname1.Append("                        INNER JOIN Loc l \n");
        varname1.Append("                                ON l.Rol = r.ID \n");
        varname1.Append("                 WHERE  l.Loc = t.LID)                                     AS lng, \n");
        varname1.Append("                CONVERT(VARCHAR(max), t.fdesc)                             AS fdesc, \n");
        varname1.Append("                Isnull((SELECT Isnull(dispalert, 0) \n");
        varname1.Append("                        FROM   Loc l \n");
        varname1.Append("                        WHERE  l.Loc = t.LID \n");
        varname1.Append("                               AND t.LType = 0), 0)                        AS dispalert, \n");
        varname1.Append("                Isnull((SELECT Isnull(credit, 0) \n");
        varname1.Append("                        FROM   Loc l \n");
        varname1.Append("                        WHERE  l.Loc = t.LID \n");
        varname1.Append("                               AND t.LType = 0), 0)                        AS credithold, \n");
        varname1.Append("               r.Name, \n");
        varname1.Append("               r.Phone, \n");
        varname1.Append("               r.Contact, \n");
        varname1.Append("               r.City, \n");
        varname1.Append("               t.Assigned \n");
        varname1.Append("FROM   TicketO t \n");
        varname1.Append(" INNER JOIN Loc l  ON l.Loc = t.LID \n");
        varname1.Append(" LEFT JOIN Rol r on r.ID = l.Rol \n");
        varname1.Append(" LEFT JOIN Branch B on B.ID = r.EN \n");
        varname1.Append(" LEFT JOIN tblUserCo UC on UC.CompanyID = r.EN \n");
        varname1.Append("WHERE  Assigned = 0 \n");
        if (!string.IsNullOrEmpty(objPropUser.SearchValue))
        {
            varname1.Append(" AND ( r.Name like '%" + objPropUser.SearchValue + "%'  \n");
            varname1.Append(" OR r.Address like '%" + objPropUser.SearchValue + "%'  \n");
            varname1.Append(" OR r.Contact like '%" + objPropUser.SearchValue + "%'  \n");
            varname1.Append(" OR r.Phone like '%" + objPropUser.SearchValue + "%'  \n");
            varname1.Append(" OR r.City like '%" + objPropUser.SearchValue + "%'  \n");
            varname1.Append(" OR l.Tag like '%" + objPropUser.SearchValue + "%'  \n");
            varname1.Append(" OR t.WorkOrder like '%" + objPropUser.SearchValue + "%'  \n");
            varname1.Append(" OR t.ID like '%" + objPropUser.SearchValue + "%' ) \n");
        }

        if (!string.IsNullOrEmpty(objPropUser.SearchValueUnAssignedCalls))
        {
            varname1.Append(" AND ( r.Address like '%" + objPropUser.SearchValueUnAssignedCalls + "%'  \n");
            varname1.Append(" OR r.City like '%" + objPropUser.SearchValueUnAssignedCalls + "%'  \n");
            varname1.Append(" OR l.Tag like '%" + objPropUser.SearchValueUnAssignedCalls + "%'  \n");
            varname1.Append(" OR t.ID like '%" + objPropUser.SearchValueUnAssignedCalls + "%'  \n");
            varname1.Append(" OR t.CDate like '%" + objPropUser.SearchValueUnAssignedCalls + "%'  \n");
            varname1.Append(" OR t.Cat like '%" + objPropUser.SearchValueUnAssignedCalls + "%' )  \n");
        }

        if (!string.IsNullOrEmpty(objPropUser.CategoryName)) { varname1.Append(" AND   t.Cat = '" + objPropUser.CategoryName + "' \n"); }

        if (objPropUser.Department != ":: All ::" & !string.IsNullOrEmpty(objPropUser.Department)) { varname1.Append(" AND   t.type = '" + objPropUser.DepartmentID + "' \n"); }

        if (objPropUser.EN == 1) { varname1.Append("AND UC.IsSel = 1 and UC.UserID = " + objPropUser.UserID); }

        varname1.Append(")as tab) \n");
        varname1.Append("select(select max(ROWNO) from FINALTICKETDATA) AS TotalCount, * from FINALTICKETDATA where 1 = 1 \n");

        varname1.Append("AND ROWNO > " + @MAXIMUMROWS * (@STARTROWINDEX) + "  \n");
        varname1.Append("AND ROWNO <=" + @MAXIMUMROWS * (@STARTROWINDEX + 1) + "  \n");
        try
        {
            return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());//  
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnTicketVoid_Click(object sender, EventArgs e)
    {
        if (hdnTicketVoidID.Value != null & hdnTicketVoidID.Value != "")
        {
            if (hdnTicketVoidPermission.Value == "N")
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "voidTicket123", "noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue: true });", true);
            }
            else if (Session["MSM"].ToString() != "TS")
            {
                int Tickets = 0;

                int LocID = 0;

                string ConnConfig = Session["config"].ToString();

                string UpdatedBy = Session["username"].ToString();

                Tickets = Convert.ToInt32(hdnTicketVoidID.Value);

                new BusinessLayer.Schedule.BL_Tickets().VoidedTickets(ConnConfig, LocID, UpdatedBy, Tickets);

                RadSchedulerRebind();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "voidTicket", "noty({ text: 'Ticket#" + hdnDeleteTicketId.Value + " Voided.', type: 'success', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue: true });", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "voidTicketts123", "noty({ text: 'Please Void ticket from Total Service.', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue: true });", true);
            }

        }
        hdnTicketVoidID.Value = "";
    }
}
