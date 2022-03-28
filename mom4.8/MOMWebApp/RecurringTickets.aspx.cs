using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessEntity;
using BusinessLayer;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Linq.Dynamic;
using Telerik.Web.UI.GridExcelBuilder;

public partial class RecurringTickets : System.Web.UI.Page
{
    User objPropUser = new User();

    BL_User objBL_User = new BL_User();

    BL_Contracts objBL_Contracts = new BL_Contracts();

    Contracts objProp_Contracts = new Contracts();

    BL_MapData objBL_MapData = new BL_MapData();

    MapData objMapData = new MapData();

    private const string ASCENDING = " ASC";

    private const string DESCENDING = " DESC";

    private int totalItemCount;

    public class GetTotalItemModel
    {
        public string TotalHours { get; set; }
    }

    GetTotalItemModel getTotalItem;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        hdnCon.Value = Session["config"].ToString();
        if (!IsPostBack)
        {
            string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

            if (Request.Url.Scheme == "http" && SSL == "1")
            {
                string URL = Request.Url.ToString();
                URL = URL.Replace("http://", "https://");
                Response.Redirect(URL);
            }

            DateTime baseDate = DateTime.Today;
            ViewState["RecTickets"] = null;
            var today = baseDate;
           
            var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
            var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
             

            txtStartDt.Text = thisMonthStart.ToShortDateString();
            txtEndDate.Text = thisMonthEnd.ToShortDateString();

            SetDefaultWorker();
            FillRoute();

            GetRecurringContracts(true);
        }
        Permission();
        CompanyPermission();
        HighlightSideMenu("cntractsMgr", "lnkTicketsMenu", "recurMgrSub");
    }

    private void HighlightSideMenu(string MenuParent, string PageLink, string SubMenuDiv)
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl(MenuParent);
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        

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
            Response.Redirect("home.aspx");
        }
        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx");
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }
        if (Session["type"].ToString() != "am")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];

            /// Ticket ///////////////////------->

            string ticketPermission = ds.Rows[0]["TicketPermission"] == DBNull.Value ? "YYNYYY" : ds.Rows[0]["TicketPermission"].ToString();
            hdnAddeTicket.Value = ticketPermission.Length < 1 ? "Y" : ticketPermission.Substring(0, 1);
        }

        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = GetUserById();
            /// RCmodulePermission ///////////////////------->

            string RCmodulePermission = ds.Rows[0]["RCmodulePermission"] == DBNull.Value ? "Y" : ds.Rows[0]["RCmodulePermission"].ToString();

        if (RCmodulePermission == "N")
        {
            Response.Redirect("Home.aspx?permission=no"); return;
        }

        /// BillPay ///////////////////------->

        string ProcessT = ds.Rows[0]["ProcessT"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["ProcessT"].ToString();
        string ADD = ProcessT.Length < 1 ? "Y" : ProcessT.Substring(0, 1);
        string Edit = ProcessT.Length < 2 ? "Y" : ProcessT.Substring(1, 1);
        string Delete = ProcessT.Length < 2 ? "Y" : ProcessT.Substring(2, 1);
        string View = ProcessT.Length < 4 ? "Y" : ProcessT.Substring(3, 1);
        if (ADD == "N")
        {
            lnkProcess.Visible = false;
        }
        if (Edit == "N")
        {         
          
        }
        if (Delete == "N")
        {
            btnDelete.Visible = false;

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
    ds = objBL_User.GetUserPermissionByUserID(objPropUser);
    return ds.Tables[0];
}
private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            //gvOpenCalls.Columns[8].Visible = true;
            gvOpenCalls.Columns.FindByDataField("Company").Visible = true;
        }
        else
        {
            //gvOpenCalls.Columns[8].Visible = false;
            gvOpenCalls.Columns.FindByDataField("Company").Visible = false;
            Session["CmpChkDefault"] = "2";
        }
    }

    protected void Page_PreRender(Object o, EventArgs e)
    {
        foreach (GridDataItem gr in gvOpenCalls.Items)
        {
           


            Label lblID = (Label)gr.FindControl("lblId");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            Label lblComp = (Label)gr.FindControl("lblComp");
 
            // Create Hyperlink For Already Created Tickets

            HiddenField hdnTicketID = (HiddenField)gr.FindControl("hdnTicketID");

            PlaceHolder PlaceHolder1 = (PlaceHolder)gr.FindControl("PlaceHolder1");

            

            string workerstatus = ((Label)gr.FindControl("RCTlblworkerstatus")).Text.Trim();

            string credit = ((Label)gr.FindControl("RCTlblcredit")).Text.Trim();

            if (hdnTicketID.Value != "")
            {

                if (credit != "1" && workerstatus != "1")
                {
                    chkSelect.Visible = false;
                }

                List<string> TicketIdsList = new List<string>();

                TicketIdsList = hdnTicketID.Value.Split(',').ToList();

                HtmlGenericControl myDiv = new HtmlGenericControl("div");
                int i = 0;
                foreach (var item in TicketIdsList)
                {
                    HyperLink myLnkBtn = new HyperLink();
                    Label lblspan = new Label();
                    if (item != TicketIdsList[TicketIdsList.Count - 1])
                    {
                        lblspan.Text = ", ";
                    }
                    myLnkBtn.ID = "myLnkBtn" + i;
                    myLnkBtn.Text = item;
                    myLnkBtn.Target = "_blank";
                    myLnkBtn.NavigateUrl = "addticket.aspx?id=" + item;
                    myDiv.Controls.Add(myLnkBtn);
                    myDiv.Controls.Add(lblspan);
                    i++;
                }
                PlaceHolder1.Controls.Add(myDiv);
            }
        }
       
    }

    protected void hideModalPopupViaServer_Click(object sender, EventArgs e)
    {
        this.ModalPopupExtender1.Hide();
        GetRecurringContracts();

    }

    private void FillRoute()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getRoute(objPropUser);
        ddlRoute.DataSource = ds.Tables[0];
        ddlRoute.DataTextField = "label";
        ddlRoute.DataValueField = "name";
        ddlRoute.DataBind();

        ddlRoute.Items.Insert(0, new ListItem(":: Select ::", ""));
        ddlRoute.Items.Insert(1, new ListItem("Unassigned", "0"));

    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    protected void lnkAddReport_Click(object sender, EventArgs e)
    {
        Response.Redirect("customersreport.aspx");
    }

    protected void btnSelectLoc_Click(object sender, EventArgs e)
    {
        FillLocInfo();
    }

    private void FillLocInfo()
    {
        if (hdnLocId.Value == "")
        {
            return;
        }
        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.LocID = Convert.ToInt32(hdnLocId.Value);
        DataSet ds = new DataSet();
        ds = objBL_User.getLocationByID(objPropUser);

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtCustomer.Text = ds.Tables[0].Rows[0]["custname"].ToString();
            hdnPatientId.Value = ds.Tables[0].Rows[0]["owner"].ToString();
        }
    }

    private void GetRecurringContracts(bool GetEmptyGriD = false)
    {
         




        if (hdnLocId.Value != string.Empty)
        {
            objProp_Contracts.Loc = Convert.ToInt32(hdnLocId.Value);
        }
        else
        {
            objProp_Contracts.Loc = 0;
        }

        if (hdnPatientId.Value != string.Empty)
        {
            objProp_Contracts.Owner = Convert.ToInt32(hdnPatientId.Value);
        }
        else
        {
            objProp_Contracts.Owner = 0;
        }

        objProp_Contracts.ConnConfig = Session["config"].ToString();
        objProp_Contracts.Remarks = notes.Text.Replace("'", "''");
        objProp_Contracts.PerContract = Convert.ToInt32(chkPerEquip.Checked);
        objProp_Contracts.ContractRemarks = Convert.ToInt32(chkContrRemarks.Checked);
        objProp_Contracts.Route = ddlRoute.SelectedValue;
        objProp_Contracts.StartDt = Convert.ToDateTime(txtStartDt.Text);
        objProp_Contracts.EndDt = Convert.ToDateTime(txtEndDate.Text);
        objProp_Contracts.OnDemand = Convert.ToInt16(chkDemand.Checked);
        ViewState["daterange"] = txtStartDt.Text + " - " + txtEndDate.Text;

        DataSet dsProcess = new DataSet();
        dsProcess = objBL_Contracts.GetLastProcessDate(objProp_Contracts);
        if (dsProcess.Tables[0].Rows.Count > 0)
        {
            string strLastProcess = dsProcess.Tables[0].Rows[0]["custom19"].ToString();
            string strLastProcessPeriod = dsProcess.Tables[0].Rows[0]["custom16"].ToString();
            string strLastProcessBy = dsProcess.Tables[0].Rows[0]["LastProcessed"].ToString();
            if (strLastProcess != string.Empty)
            {
                DateTime lastprocessdate = Convert.ToDateTime(strLastProcess);
                DateTime lastprocessperiod = Convert.ToDateTime(strLastProcessPeriod.Split('-')[1].Trim());
                lblUserName.Text = strLastProcessBy;
                lblLastProcessDate.Text = "Last Date: " + lastprocessdate.ToString("MM/dd/yyyy (hh:mm tt)");

                lblProcessPeriod.Text = "Last Period: " + lastprocessperiod.ToString("MMMM yyyy");
            }
        }

        DataSet ds = new DataSet();
        objProp_Contracts.UserID = Session["UserID"].ToString();
        if (Session["CmpChkDefault"].ToString() == "1")
        {
            objProp_Contracts.FlagEN = 1;
        }
        else
        {
            objProp_Contracts.FlagEN = 0;
        }

        if (GetEmptyGriD)
        {
            objProp_Contracts.Owner = -1;
            objProp_Contracts.Loc = -1;


        }
        objProp_Contracts.StateVal = ddlState.SelectedValue;
        ds = objBL_Contracts.AddRecurringTickets(objProp_Contracts);
        if (chkisAllTicketsUnassigned.Checked)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dr["assigned"] = 0;
                dr["workerstatus"] = 0;
                dr["worker"] = "";

            }
            ds.AcceptChanges();
        }
        ViewState["RecTickets"] = ds;

        BindGridDatatable(ds.Tables[0]);
    }

    protected void btnSelectCustomer_Click(object sender, EventArgs e)
    {
        FillLoc();
    }

    public void FillLoc()
    {
        DataSet ds = new DataSet();
        objPropUser.SearchValue = "";
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.CustomerID = Convert.ToInt32(hdnPatientId.Value);
        ds = objBL_User.getLocationAutojquery(objPropUser);

        if (ds.Tables[0].Rows.Count == 1)
        {
            hdnLocId.Value = ds.Tables[0].Rows[0]["value"].ToString();
            txtLocation.Text = ds.Tables[0].Rows[0]["label"].ToString();
            FillLocInfo();
        }
    }

    protected void lnkProcess_Click(object sender, EventArgs e)
    {
        if (hdnLocId.Value != string.Empty)
        {
            objProp_Contracts.Loc = Convert.ToInt32(hdnLocId.Value);
        }
        else
        {
            objProp_Contracts.Loc = 0;
        }

        if (hdnPatientId.Value != string.Empty)
        {
            objProp_Contracts.Owner = Convert.ToInt32(hdnPatientId.Value);
        }
        else
        {
            objProp_Contracts.Owner = 0;
        }

        objProp_Contracts.ConnConfig = Session["config"].ToString();
        objProp_Contracts.Remarks = notes.Text;
        objProp_Contracts.PerContract = Convert.ToInt32(chkPerEquip.Checked);
        objProp_Contracts.ContractRemarks = Convert.ToInt32(chkContrRemarks.Checked);
        objProp_Contracts.ProcessPeriod = Convert.ToString(ViewState["daterange"]);
        objProp_Contracts.ProcessPeriod = txtStartDt.Text + " - " + txtEndDate.Text;
        objProp_Contracts.lastUpdatedby = Session["username"].ToString();

        try
        {
            if (ViewState["RecTicketSrch"] != null)
            {


                DataTable dtr = CreatetblTypeRecurringTicket();

                objProp_Contracts.DtRecContr = dtr;

                if (dtr.Rows.Count > 0)
                {
                    objBL_Contracts.CreateRecurringTickets(objProp_Contracts);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "noty({text: 'Tickets processed successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 9000,theme : 'noty_theme_default',  closable : false}); document.getElementById('overlay').style.display='block'; ReloadPage();", true);
                }
                 

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            GetRecurringContracts();
        }
    }

    private DataTable CreatetblTypeRecurringTicket()
    {

        DataTable dt = new DataTable();

        dt.Columns.Add("Loc", typeof(int));
        dt.Columns.Add("Address", typeof(string));
        dt.Columns.Add("city", typeof(string));
        dt.Columns.Add("state", typeof(string));
        dt.Columns.Add("zip", typeof(string));
        dt.Columns.Add("calldate", typeof(DateTime));
        dt.Columns.Add("scheduledt", typeof(DateTime));
        dt.Columns.Add("assigned", typeof(int));
        dt.Columns.Add("worker", typeof(string));
        dt.Columns.Add("category", typeof(string));
        dt.Columns.Add("Elev", typeof(int));
        dt.Columns.Add("Owner", typeof(int));
        dt.Columns.Add("Jobremarks", typeof(string));
        dt.Columns.Add("remarks", typeof(string));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("Est", typeof(float));

        gvOpenCalls.MasterTableView.AllowPaging = false;
        gvOpenCalls.Rebind(); 

        foreach (GridDataItem gr in gvOpenCalls.MasterTableView.Items)
        {
            string lblTicketID = ((Label)gr.FindControl("RCTlblTicketID")).Text.Trim();

            string workerstatus = ((Label)gr.FindControl("RCTlblworkerstatus")).Text.Trim();

            string credit = ((Label)gr.FindControl("RCTlblcredit")).Text.Trim();

           
            if (string.IsNullOrEmpty(lblTicketID) && credit != "1"  && workerstatus != "1")
            {
                DataRow dr = dt.NewRow();

                dr["Loc"] = ((Label)gr.FindControl("RCTlblLocid")).Text.Trim();
                dr["Address"] = ((Label)gr.FindControl("RCTlblAddress")).Text.Trim();
                dr["city"] = ((Label)gr.FindControl("RCTlblcity")).Text.Trim();
                dr["state"] = ((Label)gr.FindControl("RCTlblstate")).Text.Trim();
                dr["zip"] = ((Label)gr.FindControl("RCTlblZip")).Text.Trim();
                string calldate=  ((Label)gr.FindControl("RCTlblcalldate")).Text.Trim();
                if (!string.IsNullOrEmpty(calldate))
                {
                    dr["calldate"] = ((Label)gr.FindControl("RCTlblcalldate")).Text.Trim();
                }
                string scheduledt =  ((Label)gr.FindControl("RCTlblscheduledt")).Text.Trim();
                if (!string.IsNullOrEmpty(scheduledt)) {
                    dr["scheduledt"] = ((Label)gr.FindControl("RCTlblscheduledt")).Text.Trim();
                }
                if (chkIsAllTicketsOnHold.Checked) { dr["assigned"] = "5"; }
                else if (chkisAllTicketsUnassigned.Checked) { dr["assigned"] = "0"; }
                else { dr["assigned"] = ((Label)gr.FindControl("RCTlblassigned")).Text.Trim(); }

                if (chkisAllTicketsUnassigned.Checked) { dr["worker"] = ""; }
                else dr["worker"] = ((Label)gr.FindControl("RCTlblworker")).Text.Trim();

                dr["category"] = ((Label)gr.FindControl("RCTlblcategory")).Text.Trim();
                int EQuip;
                int.TryParse(((Label)gr.FindControl("RCTlblElev")).Text.Trim(), out EQuip);
                if (EQuip > 0)
                {
                    dr["Elev"] = EQuip;
                }
                dr["Owner"] = ((Label)gr.FindControl("RCTlblOwner")).Text.Trim();
                dr["Jobremarks"] = ((Label)gr.FindControl("RCTlblJobremarks")).Text.Trim();
                dr["remarks"] = ((Label)gr.FindControl("RCTlblremarks")).Text.Trim();
                dr["Job"] = ((Label)gr.FindControl("RCTlblJob")).Text.Trim();
                float EST;
                float.TryParse(((Label)gr.FindControl("RCTlblEst")).Text.Trim(), out EST);
                if (EST > 0)
                {
                    dr["Est"] = EST;
                }
                dt.Rows.Add(dr);
            }

        }

        return dt;

    }

    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        try
        {

            List<DataRow> lstrows = new List<DataRow>();

            DataTable dtRecTicketSrch = (DataTable)ViewState["RecTicketSrch"];

            foreach (GridDataItem gr in gvOpenCalls.Items)
            {
                     
                    Label lblCustID = (Label)gr.FindControl("lblId");
                    CheckBox chkbox = (CheckBox)gr.FindControl("chkSelect");
                    HiddenField hdnTicketID = (HiddenField)gr.FindControl("hdnTicketID");
                    if (chkbox.Checked)
                    {
                        foreach (DataRow dr in dtRecTicketSrch.Rows)
                        {

                            if (dr["Loc"].ToString() == ((Label)gr.FindControl("RCTlblLocid")).Text.Trim() &&
                             dr["calldate"].ToString() == ((Label)gr.FindControl("RCTlblcalldate")).Text.Trim() &&
                             dr["scheduledt"].ToString() == ((Label)gr.FindControl("RCTlblscheduledt")).Text.Trim() &&
                             dr["Owner"].ToString() == ((Label)gr.FindControl("RCTlblOwner")).Text.Trim() &&
                             dr["Elev"].ToString() == ((Label)gr.FindControl("RCTlblElev")).Text.Trim())
                            {
                                lstrows.Add(dr);
                            }
                        }
                    }
                
            }

            foreach (var item in lstrows)
            {
                dtRecTicketSrch.Rows.Remove(item);
            }

            ViewState["RecTicketSrch"] = dtRecTicketSrch;

            gvOpenCalls.Rebind();

            SetRecurringTicketListInViewState(dtRecTicketSrch);

            string str = "Deleted Successfully";

            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrdelete", "noty({text: '" + str + "',  type : 'Success', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private bool ContainColumn(string columnName, DataTable table)
    {
        bool contain = false;
        DataColumnCollection columns = table.Columns;
        if (columns.Contains(columnName))
        {
            contain = true;
        }
        return contain;
    }

    private void BindGridDatatable(DataTable dt)
    {

        ViewState["RecTicketSrch"] = dt;
        upSearch.Update();
        gvOpenCalls.Rebind();
        SetRecurringTicketListInViewState(dt);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GetRecurringContracts();

    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        GetRecurringContracts();
    }

    private void SetRecurringTicketListInViewState(DataTable dt)
    {

        ViewState["RecTicketSrch"] = dt;

        List<object> InactiveUsers = (from row in dt.AsEnumerable() select (row["workerstatus"])).ToList();
        if (!InactiveUsers.Any(p => p.ToString() == "1")) { HdnConfirm.Value = "0"; } else { HdnConfirm.Value = "1"; }

        List<object> InCreditHold = (from row in dt.AsEnumerable() select (row["credit"])).ToList();
        if (!InCreditHold.Any(p => p.ToString() == "1")) { hdnCreditHold.Value = "0"; } else { hdnCreditHold.Value = "1"; }


    }

    protected void gvOpenCalls_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    { 

        DataTable dt = (DataTable)ViewState["RecTicketSrch"];
        gvOpenCalls.DataSource = dt;

        if (chkPerEquip.Checked)
        {
            gvOpenCalls.Columns.FindByDataField("unit").Visible = true;
            gvOpenCalls.Columns.FindByDataField("TempEquipmentstr").Visible = false;
        }
        else
        {
            gvOpenCalls.Columns.FindByDataField("unit").Visible = false;
            gvOpenCalls.Columns.FindByDataField("TempEquipmentstr").Visible = true;
        }
        

        SetRecurringTicketListInViewState(dt);

    }

    protected void gvOpenCalls_ItemEvent(object sender, GridItemEventArgs e)
    {

        if (e.EventInfo is GridInitializePagerItem)
        {
            totalItemCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
            if (totalItemCount < 2)
            {
                lblRecordCount.Text = totalItemCount.ToString() + " Record found";
            }
            else if (totalItemCount > 1)
            {
                lblRecordCount.Text = totalItemCount.ToString() + " Record(s) found";
            }

            if (totalItemCount == 0)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "hideSelectAllChkb", "hideSelectAllChkb();", true);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "showSelectAllChkb", "showSelectAllChkb();", true);

            }
        }
    }

    protected void gvOpenCalls_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                RadComboBox dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;

                if (totalCount == 0)  totalCount=1000;

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
        catch (Exception ex) { }
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

                if (!CheckControlExist(c.UniqueID))
                {
                    switch (c.GetType().ToString())
                    {
                        case "System.Web.UI.WebControls.DropDownList":
                            ((DropDownList)c).SelectedIndex = -1;
                            break;
                        case "System.Web.UI.WebControls.TextBox":
                            ((TextBox)c).Text = string.Empty;
                            break;
                        case "System.Web.UI.WebControls.CheckBox":
                            ((CheckBox)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.RadioButton":
                            ((RadioButton)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.HtmlTextArea":
                            ((HtmlTextArea)c).Value = string.Empty;
                            break;

                    }
                }
            }
        }
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        ResetFormControlValues(this);

        hdnLocId.Value = string.Empty;
        hdnPatientId.Value = string.Empty;

        foreach (GridColumn column in gvOpenCalls.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        gvOpenCalls.MasterTableView.FilterExpression = string.Empty;
        GetRecurringContracts(true);

    }

    private List<string> ControlNotReset()
    {
        List<string> control = new List<string>();
        control.Add("txtStartDt");
        control.Add("txtEndDate");
        return control;
    }

    private bool CheckControlExist(string controlId)
    {
        bool check = false;
        foreach (var item in ControlNotReset())
        {
            if (controlId.Contains(item))
            {
                check = true;
            }
        }
        return check;
    }

    protected void gvOpenCalls_ItemDataBound(object sender, GridItemEventArgs e)
    {
    
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        gvOpenCalls.MasterTableView.AllowPaging = true;
        GetRecurringContracts(true);
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        gvOpenCalls.MasterTableView.GetColumn("Comp").Visible = false;
        gvOpenCalls.ExportSettings.FileName = "RecurringTicket";
        gvOpenCalls.ExportSettings.IgnorePaging = true;
        gvOpenCalls.ExportSettings.ExportOnlyData = true;
        gvOpenCalls.ExportSettings.OpenInNewWindow = true;
        gvOpenCalls.ExportSettings.HideStructureColumns = true;
        gvOpenCalls.MasterTableView.UseAllDataFields = true;
        gvOpenCalls.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        gvOpenCalls.MasterTableView.ExportToExcel();
    }

    protected void gvOpenCalls_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 5;
        else
            currentItem = 6;
        if (e.Worksheet.Table.Rows.Count == gvOpenCalls.Items.Count + 1)
        {
            GridFooterItem footerItem = gvOpenCalls.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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

    private void SetDefaultWorker()
    {
        Customer objCustomer = new Customer();
        BL_Customer objBL_Customer = new BL_Customer();
        var masterTableView = gvOpenCalls.MasterTableView;
        var column = masterTableView.GetColumn("dwork");
        objCustomer.ConnConfig = Session["config"].ToString();
        string getValue = objBL_Customer.GetDefaultWorkerHeader(objCustomer);
        if (!string.IsNullOrEmpty(getValue))
        {
            spnWorker.InnerText = "Preferred "+getValue;
            column.HeaderText= "Preferred " + getValue;
        }
        else
        {
            spnWorker.InnerText = "Preferred Default Worker";
            column.HeaderText = "Preferred Default Worker";
        }
    }
}
