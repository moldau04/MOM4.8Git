using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Newtonsoft.Json;

public partial class HomeDemo : System.Web.UI.Page
{
    static Contracts objContract = new Contracts();
    static BL_Contracts objBLContracts = new BL_Contracts();
    public SQLNotifier Notifier { get; set; }
    static BL_User objBL_User = new BL_User();
    static BusinessEntity.User objPropUser = new BusinessEntity.User();

    static DateTime? GetFullDate(object dt)
    {
        try
        {
            var dtString = dt.ToString();
            var year = int.Parse(dtString.Substring(0, 4));
            var month = int.Parse(dtString.Substring(4, 2));
            return new DateTime(year, month, 1);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private bool _dockStateCleared = false;
    private string _defaultCategory = string.Empty;
    private List<DockState> CurrentDockStates
    {
        get
        {
            //Store the info about the added docks in the session. For real life
            // applications we recommend using database or other storage medium 
            // for persisting this information.
            List<DockState> _currentDockStates = new List<DockState>();

            if (_dashboardId > 0)
            {
                objPropUser.ConnConfig = Session["config"].ToString();
                var ds = objBL_User.GetDashboardByID(objPropUser, _dashboardId);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var dockStates = ds.Tables[0].Rows[0]["DockStates"].ToString();
                    if (!string.IsNullOrEmpty(dockStates))
                    {
                        _currentDockStates = JsonConvert.DeserializeObject<List<DockState>>(dockStates);
                    }
                }
            }
            else
            {
                _currentDockStates = (List<DockState>)Session["CurrentDockStatesMyPortal"];
                if(_currentDockStates == null)
                {
                    _currentDockStates = new List<DockState>();
                }
            }

            // Session["CurrentDockStatesMyPortal"] = _currentDockStates;

            return _currentDockStates;
        }
        set
        {
            if (_dashboardId > 0)
            {
                objPropUser.ConnConfig = Session["config"].ToString();
                objBL_User.UpdateDashboardDockStates(objPropUser, _dashboardId, JsonConvert.SerializeObject(value));
            }
            else
            {
                Session["CurrentDockStatesMyPortal"] = value;
            }
        }
    }
    private int _dashboardId { get; set; }
    private DataTable _listKPIs { get; set; }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["MSM"] == null)
        {
            Response.Redirect("login.aspx");
        }

        var userId = Session["userid"];
        objPropUser.UserID = Convert.ToInt32(userId);
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();

        if (Request.QueryString["id"] != null)
        {
            _dashboardId = Convert.ToInt32(Request.QueryString["id"]);
        }

        if (!Page.IsPostBack)
        {
            if (_dashboardId > 0)
            {
                var ds = objBL_User.GetDashboardByID(objPropUser, _dashboardId);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (userId.ToString() != ds.Tables[0].Rows[0]["UserID"].ToString())
                    {
                        Response.Redirect("HomeDemo.aspx");
                    }
                }
                else
                {
                    Response.Redirect("HomeDemo.aspx");
                }

                txtDashboardName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                LoadGraphDropdown(_dashboardId);
            }
            else
            {
                var dsDefault = objBL_User.GetDashboardDefault(objPropUser);
                if (dsDefault.Tables.Count > 0 && dsDefault.Tables[0].Rows.Count > 0)
                {
                    Response.Redirect(string.Format("HomeDemo.aspx?id={0}", dsDefault.Tables[0].Rows[0]["ID"]));
                }

                txtDashboardName.Text = "KPI";
                LoadGraphDropdown(0);
            }

            CheckPermissionsDropdownItem();
        }

        _defaultCategory = objBL_User.getDefaultCategory(objPropUser);

        //Recreate the docks in order to ensure their proper operation
        for (int i = 0; i < CurrentDockStates.Count; i++)
        {
            // clears the closed docks from the dock state, this line is 
            // optional and its purpose is to keep the dock state as small 
            // as possible
            if (CurrentDockStates[i].Closed == true) continue;

            RadDock dock = CreateRadDockFromState(CurrentDockStates[i]);
            //We will just add the RadDock control to the RadDockLayout.
            // You could use any other control for that purpose, just ensure
            // that it is inside the RadDockLayout control.
            // The RadDockLayout control will automatically move the RadDock
            // controls to their corresponding zone in the LoadDockLayout
            // event (see below).
            RadDockLayout1.Controls.Add(dock);
            //We want to save the dock state every time a dock is moved.
            CreateSaveStateTrigger(dock);
            //Load the selected widget
            LoadWidget(dock);

            // prevents the rendering of closed docks, used for improving 
            // performance
            if (CurrentDockStates[i].Closed == true)
            {
                dock.Visible = false;
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            Session["ARCollectionSummayry"] = GetCollectionSummayry();

            if (CurrentDockStates.Count == 0)
            {
                if (Request.QueryString["id"] != null)
                {
                    for (int i = 0; i < _listKPIs.Rows.Count; i++)
                    {
                        AddGraph(_listKPIs.Rows[i]["UserControl"].ToString());
                    }
                }
                else
                {
                    // Add default graphs
                    AddContentDock("", "~/NewKPI/Components/Contents/OneTwentyDayAccountsReceivable.ascx");
                    AddContentDock("", "~/NewKPI/Components/Contents/NinetyDayAccountsReceivable.ascx");
                    AddContentDock("", "~/NewKPI/Components/Contents/SixtyDayAccountsReceivable.ascx");
                    AddContentDock("", "~/NewKPI/Components/Contents/AvgEstimateConversionRate.ascx");

                    AddGraphDock("Equipment by Type", "~/NewKPI/Components/EquipmentTypeChart.ascx");
                    AddGraphDock("Trouble Calls by Equipment", "~/NewKPI/Components/TroubleCallsByEquipment.ascx");
                    AddGraphDock("Monthly Revenue by Department", "~/NewKPI/Components/MonthlyRevenueByDeptChart.ascx");
                    AddGraphDock(string.Format("Monthly Default Recurring {0} Open vs Completed", _defaultCategory), "~/NewKPI/Components/TicketRecurringChart.ascx");
                    AddGraphDock(string.Format("{0} Recurring Hours Remaining for Current Month by Route", _defaultCategory), "~/NewKPI/Components/RecurringHoursRemaining.ascx");
                    AddGraphDock("Equipment By Building", "~/NewKPI/Components/EquipmentBuildingChart.ascx");
                }
            }
        }
    }

    private void LoadGraphDropdown(int dashboardID)
    {
        objPropUser.ConnConfig = Session["config"].ToString();

        if (dashboardID == 0)
        {
            var ds = objBL_User.GetListKPIs(objPropUser);
            _listKPIs = ds.Tables[0];
        }
        else
        {
            var ds = objBL_User.GetListDashKPI(objPropUser, dashboardID);
            _listKPIs = ds.Tables[0];
        }

        RadComboBox1.DataSource = _listKPIs;
        RadComboBox1.DataTextField = "Name";
        RadComboBox1.DataValueField = "UserControl";
        RadComboBox1.DataBind();
    }

    private void CheckPermissionsDropdownItem()
    {
        // Change the title of the special graphs in dropdown
        var recurringHoursRemainingItem = RadComboBox1.Items.FindItem(x => x.Value == "~/NewKPI/Components/RecurringHoursRemaining.ascx");
        if (recurringHoursRemainingItem != null)
        {
            recurringHoursRemainingItem.Text = string.Format("{0} Recurring Hours Remaining for Current Month by Route", _defaultCategory);
        }

        var ticketRecurringItem = RadComboBox1.Items.FindItem(x => x.Value == "~/NewKPI/Components/TicketRecurringChart.ascx");
        if (ticketRecurringItem != null)
        {
            ticketRecurringItem.Text = string.Format("Monthly Recurring {0} Open vs Completed", _defaultCategory);
        }

        // Check permissions 
        var financeStatement = (bool)Session["FinanceStatement"];
        if (!financeStatement)
        {
            var removeItem = RadComboBox1.Items.FindItem(x => x.Value == "~/NewKPI/Components/ActualBudgetedRevenueChart.ascx");
            if (removeItem != null)
            {
                RadComboBox1.Items.Remove(removeItem);
            }
        }
    }

    private DataTable GetCollectionSummayry()
    {
        BL_Collection obj = new BL_Collection();

        CollectionModel data = new CollectionModel();
        data.ConnConfig = Convert.ToString(Session["config"]);
        data.Date = DateTime.Now;

        #region Company Check
        data.UserID = Convert.ToInt32(Session["UserID"].ToString());

        if (Convert.ToString(Session["CmpChkDefault"]) == "1" || Convert.ToString(Session["chkCompanyName"]) != "")
        {
            data.EN = 1;
        }
        else
        {
            data.EN = 0;
        }
        #endregion

        data.CustomerIDs = "";
        data.LocationIDs = "";
        data.DepartmentIDs = "";
        data.CustomDay = 0;

        DataSet ds = obj.GetCollectionsSummary(data);

        return ds.Tables[0];
    }

    private void AddGraphDock(string dockTitle, string dockTag)
    {
        if (CurrentDockStates.FirstOrDefault(x => x.Tag == dockTag && !x.Closed) == null)
        {
            RadDock dock = CreateRadDock();
            dock.DefaultCommands = Telerik.Web.UI.Dock.DefaultCommands.Close;
            dock.Resizable = true;
            dock.Width = Unit.Percentage(48);
            dock.Title = dockTitle;
            dock.CssClass = "dock-normal";
            dock.Commands.Add(new DockCloseCommand());
            dock.Command += RadDock_Command;
            dock.DockPositionChanged += RadDock_DockPositionChanged;

            DockCommand customCommandMaximize = new DockCommand();
            customCommandMaximize.Name = "Maximize";
            customCommandMaximize.Text = "Maximize";
            customCommandMaximize.AutoPostBack = true;
            customCommandMaximize.CssClass = "maximize";
            dock.Commands.Add(customCommandMaximize);

            //find the target zone and add the new dock there
            RadDockZone dz = RadDockZoneGraph;

            //adding the dock to the dock layout and then docking it to the zone to avoid ViewState issues on subsequent postback
            RadDockLayout1.Controls.Add(dock);
            dock.Dock(dz);

            CreateSaveStateTrigger(dock);

            // Ensure that the dock is opened, should be used when closed docks are
            // cleared from the dock state on Page_Init
            dock.Closed = false;

            //Load the selected widget in the RadDock control
            dock.Tag = dockTag;
            LoadWidget(dock);
            CheckDefault(dockTag);
        }
    }

    private void AddContentDock(string dockTitle, string dockTag)
    {
        if (CurrentDockStates.FirstOrDefault(x => x.Tag == dockTag && !x.Closed) == null)
        {
            RadDock dock = CreateRadDock();
            dock.DefaultCommands = Telerik.Web.UI.Dock.DefaultCommands.Close;
            dock.Resizable = false;
            dock.Width = Unit.Percentage(23);
            dock.Title = dockTitle;
            dock.Commands.Add(new DockCloseCommand());

            //find the target zone and add the new dock there
            RadDockZone dz = RadDockZoneGraph;

            //adding the dock to the dock layout and then docking it to the zone to avoid ViewState issues on subsequent postback
            RadDockLayout1.Controls.Add(dock);
            dock.Dock(dz);

            CreateSaveStateTrigger(dock);

            // Ensure that the dock is opened, should be used when closed docks are
            // cleared from the dock state on Page_Init
            dock.Closed = false;

            //Load the selected widget in the RadDock control
            dock.Tag = dockTag;
            LoadWidget(dock);
            CheckDefault(dockTag);
        }
    }

    protected void RadDockLayout1_LoadDockLayout(object sender, DockLayoutEventArgs e)
    {
        //Populate the event args with the state information. The RadDockLayout control
        // will automatically move the docks according that information.
        foreach (DockState state in CurrentDockStates)
        {
            e.Positions[state.UniqueName] = state.DockZoneID;
            e.Indices[state.UniqueName] = state.Index;
        }
    }

    protected void RadDockLayout1_SaveDockLayout(object sender, DockLayoutEventArgs e)
    {
        if (!_dockStateCleared)
        {
            //Save the dock state in the session. This will enable us 
            // to recreate the dock in the next Page_Init.
            CurrentDockStates = RadDockLayout1.GetRegisteredDocksState();
        }
        else
        {
            //the clear state button was clicked, so we refresh the page and start over.
            Response.Redirect(Request.RawUrl, false);
        }
    }

    protected void RadDock_Command(object sender, DockCommandEventArgs e)
    {
        RadDock dock = (RadDock)sender;
        var index = dock.Index;
        if (e.Command.Name == "Maximize")
        {
            dock.Commands.Remove(e.Command);

            DockCommand customCommandMinimize = new DockCommand();
            customCommandMinimize.Name = "Minimize";
            customCommandMinimize.Text = "Minimize";
            customCommandMinimize.AutoPostBack = true;
            customCommandMinimize.CssClass = "minimize";
            dock.Commands.Add(customCommandMinimize);
            dock.Width = Unit.Percentage(98);
            dock.CssClass = "dock-wide";

            index = GetIndex(RadDockZoneGraph, dock, true);
        }
        else if (e.Command.Name == "Minimize")
        {
            dock.Commands.Remove(e.Command);

            DockCommand customCommandMaximize = new DockCommand();
            customCommandMaximize.Name = "Maximize";
            customCommandMaximize.Text = "Maximize";
            customCommandMaximize.AutoPostBack = true;
            customCommandMaximize.CssClass = "maximize";
            dock.Commands.Add(customCommandMaximize);
            dock.Width = Unit.Percentage(48);
            dock.CssClass = "dock-normal";

            index = GetIndex(RadDockZoneGraph, dock, false);
        }
        else if (e.Command.Name == "Close")
        {
            var item = RadComboBox1.Items.FindItem(x => x.Value == dock.Tag);
            if (item != null)
            {
                item.Checked = false;
            }
        }
        CurrentDockStates = RadDockLayout1.GetRegisteredDocksState();
        dock.Index = index;
    }

    protected void RadDock_DockPositionChanged(object sender, DockPositionChangedEventArgs e)
    {
        RadDock dock = (RadDock)sender;
        var controls = RadDockLayout1.Controls.OfType<RadDock>().Where(x => x.DockZoneID == e.DockZoneID).OrderBy(x => x.Index);

        if (dock.Width == Unit.Percentage(98))
        {
            if (dock.Index > e.Index)
            {
                var smallControls = controls.Where(x => x.Width == Unit.Percentage(48) && x.Index < e.Index);
                if (smallControls.Count() % 2 != 0)
                {
                    var beforeControl = controls.LastOrDefault(x => x.Index < e.Index && x.Width == Unit.Percentage(48));
                    if (beforeControl != null)
                    {
                        var lastLarge = controls.LastOrDefault(x => x.Width == Unit.Percentage(98));
                        //dock.Index = dock.Index - 1;
                        if (lastLarge != null)
                        {
                            beforeControl.Index = lastLarge.Index + 1;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "MoveDock" + Guid.NewGuid(),
                                string.Format(@"function _moveDock() {{
                            Sys.Application.remove_load(_moveDock);
                            $find('{1}').dock($find('{0}'),{2});
                            }};
                            Sys.Application.add_load(_moveDock);", beforeControl.ClientID, RadDockZoneGraph.ClientID, lastLarge.Index + 1), true);
                        }
                    }
                }
            }
            else
            {
                var smallControls = controls.Where(x => x.Width == Unit.Percentage(48) && x.Index > dock.Index && x.Index <= e.Index);
                if (smallControls.Count() % 2 != 0)
                {
                    var afterControl = controls.FirstOrDefault(x => x.Width == Unit.Percentage(48) && x.Index > e.Index);
                    if (afterControl != null)
                    {
                        afterControl.Index = e.Index - 1;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MoveDock" + Guid.NewGuid(),
                            string.Format(@"function _moveDock() {{
                        Sys.Application.remove_load(_moveDock);
                        $find('{1}').dock($find('{0}'),{2});
                        }};
                        Sys.Application.add_load(_moveDock);", afterControl.ClientID, RadDockZoneGraph.ClientID, e.Index - 1), true);
                    }
                    else
                    {
                        var beforeControl = controls.LastOrDefault(x => x.Index <= e.Index && x.Width == Unit.Percentage(48));
                        if (beforeControl != null)
                        {
                            beforeControl.Index = beforeControl.Index + 2;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "MoveDock" + Guid.NewGuid(),
                                string.Format(@"function _moveDock() {{
                            Sys.Application.remove_load(_moveDock);
                            $find('{1}').dock($find('{0}'),{2});
                            }};
                            Sys.Application.add_load(_moveDock);", beforeControl.ClientID, RadDockZoneGraph.ClientID, beforeControl.Index), true);
                        }
                    }
                }
            }
        }
        else
        {

        }

        CurrentDockStates = RadDockLayout1.GetRegisteredDocksState();
    }

    private RadDock CreateRadDockFromState(DockState state)
    {
        RadDock dock = new RadDock();
        dock.DockMode = DockMode.Docked;
        dock.ID = string.Format("RadDock{0}", state.UniqueName);
        dock.ApplyState(state);
        dock.Title = GetDockTitle(state.Tag);
        dock.Commands.Add(new DockCloseCommand());
        dock.Command += RadDock_Command;
        dock.DockPositionChanged += RadDock_DockPositionChanged;

        if (state.Tag != "~/NewKPI/Components/Contents/SixtyDayAccountsReceivable.ascx"
            && state.Tag != "~/NewKPI/Components/Contents/NinetyDayAccountsReceivable.ascx"
            && state.Tag != "~/NewKPI/Components/Contents/OneTwentyDayAccountsReceivable.ascx"
            && state.Tag != "~/NewKPI/Components/Contents/AvgEstimateConversionRate.ascx")
        {
            if (state.Width == Unit.Percentage(48))
            {
                DockCommand customCommandMaximize = new DockCommand();
                customCommandMaximize.Name = "Maximize";
                customCommandMaximize.Text = "Maximize";
                customCommandMaximize.AutoPostBack = true;
                customCommandMaximize.CssClass = "maximize";
                dock.Commands.Add(customCommandMaximize);
                dock.CssClass = "dock-normal";
            }
            else
            {
                DockCommand customCommandMinimize = new DockCommand();
                customCommandMinimize.Name = "Minimize";
                customCommandMinimize.Text = "Minimize";
                customCommandMinimize.AutoPostBack = true;
                customCommandMinimize.CssClass = "minimize";
                dock.Commands.Add(customCommandMinimize);
                dock.CssClass = "dock-wide";
            }
        }
        else
        {
            dock.Width = Unit.Percentage(23);
            dock.Resizable = false;
        }

        // Check item in dropdown
        CheckDefault(state.Tag);

        return dock;
    }

    private RadDock CreateRadDock()
    {
        RadDock dock = new RadDock();
        dock.DockMode = DockMode.Docked;
        dock.UniqueName = Guid.NewGuid().ToString().Replace("-", "a");
        dock.ID = string.Format("RadDock{0}", dock.UniqueName);
        dock.Title = "Dock";
        dock.Text = string.Format("Added at {0}", DateTime.Now);
        dock.Width = Unit.Pixel(300);
        dock.DockPositionChanged += RadDock_DockPositionChanged;

        dock.Commands.Add(new DockCloseCommand());
        //dock.Commands.Add(new DockExpandCollapseCommand());

        return dock;
    }

    private void CreateSaveStateTrigger(RadDock dock)
    {
        //Ensure that the RadDock control will initiate postback
        // when its position changes on the client or any of the commands is clicked.
        //Using the trigger we will "ajaxify" that postback.
        dock.AutoPostBack = true;
        dock.CommandsAutoPostBack = true;

        AsyncPostBackTrigger saveStateTrigger = new AsyncPostBackTrigger();
        saveStateTrigger.ControlID = dock.ID;
        saveStateTrigger.EventName = "DockPositionChanged";
        UpdatePanel1.Triggers.Add(saveStateTrigger);

        saveStateTrigger = new AsyncPostBackTrigger();
        saveStateTrigger.ControlID = dock.ID;
        saveStateTrigger.EventName = "Command";
        UpdatePanel1.Triggers.Add(saveStateTrigger);
    }

    private void LoadWidget(RadDock dock)
    {
        if (string.IsNullOrEmpty(dock.Tag) || dock.Closed)
        {
            return;
        }
        Control widget = LoadControl(dock.Tag);
        dock.ContentContainer.Controls.Add(widget);
    }

    private int GetIndex(RadDockZone radDockZone, RadDock currentDock, bool isMaximize)
    {
        var index = currentDock.Index;
        var controls = RadDockLayout1.Controls.OfType<RadDock>().Where(x => x.DockZoneID == radDockZone.ClientID).OrderBy(x => x.Index);

        if (isMaximize)
        {
            var smallControls = controls.Where(x => x.Width == Unit.Percentage(48) && x.Index < currentDock.Index);
            if (smallControls.Count() % 2 != 0)
            {
                index = currentDock.Index - 1;
                var beforeControl = controls.First(x => x.Index == currentDock.Index - 1);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "MoveDock" + Guid.NewGuid(),
                    string.Format(@"function _moveDock() {{
			            Sys.Application.remove_load(_moveDock);
			            $find('{1}').dock($find('{0}'),{2});
			            }};
			            Sys.Application.add_load(_moveDock);", currentDock.ClientID, RadDockZoneGraph.ClientID, index), true);

                var lastLarge = controls.LastOrDefault(x => x.Width == Unit.Percentage(98));
                if (lastLarge != null)
                {
                    beforeControl.Index = lastLarge.Index + 1;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "MoveDock" + Guid.NewGuid(),
                        string.Format(@"function _moveDock() {{
                            Sys.Application.remove_load(_moveDock);
                            $find('{1}').dock($find('{0}'),{2});
                            }};
                            Sys.Application.add_load(_moveDock);", beforeControl.ClientID, RadDockZoneGraph.ClientID, lastLarge.Index + 1), true);
                }
            }
            else
            {
                var largeControls = controls.Where(x => x.Width == Unit.Percentage(98) && x.Index > currentDock.Index);
                if (largeControls.Count() > 0)
                {
                    var afterControl = controls.First(x => x.Index == currentDock.Index + 1);

                    var lastLarge = controls.LastOrDefault(x => x.Width == Unit.Percentage(98));
                    if (lastLarge != null)
                    {
                        afterControl.Index = lastLarge.Index + 1;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "MoveDock" + Guid.NewGuid(),
                            string.Format(@"function _moveDock() {{
                                Sys.Application.remove_load(_moveDock);
                                $find('{1}').dock($find('{0}'),{2});
                                }};
                                Sys.Application.add_load(_moveDock);", afterControl.ClientID, RadDockZoneGraph.ClientID, lastLarge.Index + 1), true);
                    }
                }
            }
        }
        else
        {
            var lastLarge = controls.LastOrDefault(x => x.Width == Unit.Percentage(98));
            if (lastLarge != null)
            {
                index = lastLarge.Index + 1;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "MoveDock" + Guid.NewGuid(),
                    string.Format(@"function _moveDock() {{
                        Sys.Application.remove_load(_moveDock);
                        $find('{1}').dock($find('{0}'),{2});
                        }};
                        Sys.Application.add_load(_moveDock);", currentDock.ClientID, RadDockZoneGraph.ClientID, lastLarge.Index + 1), true);
            }
        }

        return index;
    }

    #region Button click

    protected void btnSaveDashboardName_Click(object sender, EventArgs e)
    {
        var ds = objBL_User.GetDashboardByID(objPropUser, _dashboardId);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            objBL_User.UpdateDashboard(objPropUser, _dashboardId, txtDashboardName.Text, Convert.ToBoolean(ds.Tables[0].Rows[0]["IsDefault"].ToString()));
            txtDashboardName.Attributes.Add("readonly", "readonly");
        }
    }

    protected void RadComboBox1_OnItemChecked(object sender, RadComboBoxItemEventArgs e)
    {
        if (e.Item.Checked)
        {
            AddGraph(e.Item.Value);
        }
        else
        {
            var radDock = RadDockLayout1.Controls.OfType<RadDock>().FirstOrDefault(x => x.Tag == e.Item.Value);
            if (radDock != null)
            {
                radDock.Closed = true;
            }
        }
    }

    protected void RadComboBox1_CheckAllCheck(object sender, RadComboBoxCheckAllCheckEventArgs e)
    {
        if (e.CheckAllChecked)
        {
            foreach (var item in RadComboBox1.Items.ToList())
            {
                AddGraph(item.Value);
            }
        }
        else
        {
            foreach (var item in RadDockLayout1.Controls.OfType<RadDock>())
            {
                item.Closed = true;
            }
        }
    }

    private void AddGraph(string tag)
    {
        var dockTitle = GetDockTitle(tag);
        if (tag == "~/NewKPI/Components/Contents/SixtyDayAccountsReceivable.ascx"
            || tag == "~/NewKPI/Components/Contents/NinetyDayAccountsReceivable.ascx"
            || tag == "~/NewKPI/Components/Contents/OneTwentyDayAccountsReceivable.ascx"
            || tag == "~/NewKPI/Components/Contents/AvgEstimateConversionRate.ascx")
        {
            AddContentDock("", tag);
        }
        else
        {
            AddGraphDock(dockTitle, tag);
        }
    }

    private void CheckDefault(string tag)
    {
        var item = RadComboBox1.Items.FindItem(x => x.Value == tag);
        if (item != null)
        {
            item.Checked = true;
        }
    }

    private string GetDockTitle(string tag)
    {
        string title = string.Empty;
        switch (tag)
        {
            case "~/NewKPI/Components/EquipmentTypeChart.ascx":
                title = "Equipment by Type";
                break;
            case "~/NewKPI/Components/EquipmentBuildingChart.ascx":
                title = "Equipment by Building";
                break;
            case "~/NewKPI/Components/ActualBudgetedRevenueChart.ascx":
                title = "Actual vs Budgeted Revenue";
                break;
            case "~/NewKPI/Components/RecurringHoursChart.ascx":
                title = "Converted Estimates By Salesperson Avg. Days";
                break;
            case "~/NewKPI/Components/RecurringHoursRemaining.ascx":
                title = string.Format("{0} Recurring Hours Remaining for Current Month by Route", _defaultCategory);
                break;
            case "~/NewKPI/Components/TicketRecurringChart.ascx":
                title = string.Format("Monthly Recurring {0} Open vs Completed", _defaultCategory);
                break;
            case "~/NewKPI/Components/MonthlyRevenueByDeptChart.ascx":
                title = "Monthly Revenue by Department";
                break;
            case "~/NewKPI/Components/TroubleCallsByEquipment.ascx":
                title = "Trouble Calls by Equipment";
                break;
        }

        return title;
    }

    #endregion
}