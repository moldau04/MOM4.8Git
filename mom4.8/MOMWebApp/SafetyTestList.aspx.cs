using BusinessEntity;
using BusinessEntity.Recurring;
using BusinessEntity.Utility;
using BusinessLayer;
using BusinessLayer.Utility;
using Microsoft.ApplicationBlocks.Data;
using MOMWebApp;
using Newtonsoft.Json;
using Novacode;
using Spire.Pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using static CommonHelper;
using Table = Novacode.Table;

public partial class SafetyTestList : System.Web.UI.Page
{
    List<String> lsControlName = new List<string>();
    Dictionary<string, CustomField> DynamicControls = new Dictionary<string, CustomField>();
    List<String> lsControlFormual = new List<string>();
    List<String> ls = new List<string>();
    List<CustomFieldID> lsFormula = new List<CustomFieldID>();
    private const int AssignedStatusValue = 1;
    BL_User objBL_User = new BL_User();
    private bool IsGridPageIndexChanged = false;
    Boolean isGridFilterSafetyTest = false;
    GetDefaultWorkerHeaderParam _GetDefaultWorkerHeader = new GetDefaultWorkerHeaderParam();
    //API Variables 
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    private void createDataSource()
    {
        ArrayList item = new ArrayList();
        //dynamic column
        DataSet dst = new DataSet();
        BL_SafetyTest _objbltesttypes = new BL_SafetyTest();
        dst = _objbltesttypes.GetAllTestCustom(Session["config"].ToString(), Session["dbname"].ToString());
        DataTable dtColumns = dst.Tables[0];
        RadGrid_SafetyTest.AutoGenerateColumns = false;
        foreach (DataRow row in dtColumns.Rows)
        {
            if (row["Format"].ToString() == "4")
            {
                if (!item.Contains(row["Label"].ToString() + "_" + row["ID"].ToString()))
                {
                    SqlDataSource sourceID = new SqlDataSource();
                    sourceID.ID = "CustomField_" + row["Label"].ToString().Replace(" ", "") + "_" + row["ID"].ToString();
                    sourceID.ConnectionString = Session["config"].ToString();
                    sourceID.SelectCommand = "SELECT '' AS [Value] UNION SELECT [Value] FROM tblTestCustom where [tblTestCustomFieldsID]=" + row["ID"].ToString();
                    sourceID.DataBind();
                    this.Page.Controls.Add(sourceID);
                    item.Add(sourceID.ID);
                }

            }
            if (row["Format"].ToString() == "5")
            {
                if (!item.Contains(row["Label"].ToString() + "_" + row["ID"].ToString()))
                {
                    SqlDataSource sourceID = new SqlDataSource();
                    sourceID.ID = "CustomField_" + row["Label"].ToString().Replace(" ", "") + "_" + row["ID"].ToString();
                    sourceID.ConnectionString = Session["config"].ToString();
                    sourceID.SelectCommand = "select 'True'  as [Value] union select 'False'  as [Value]";
                    sourceID.DataBind();
                    this.Page.Controls.Add(sourceID);
                    item.Add(sourceID.ID);
                }

            }

        }
    }

    private void DefineCustomColumnStructure()
    {
        String FireTestDate = "";
        //dynamic column
        DataSet dst = new DataSet();
        BL_SafetyTest _objbltesttypes = new BL_SafetyTest();
        dst = _objbltesttypes.GetAllTestCustom(Session["config"].ToString(), Session["dbname"].ToString());
        DataTable dtColumns = dst.Tables[0];
        foreach (DataRow row in dtColumns.Rows)
        {
            DynamicControls.Add("CustomField_" + row["Label"].ToString().Replace(" ", "") + "_" + row["ID"].ToString()
                        , (CustomField)Enum.Parse(typeof(CustomField), row["Format"].ToString()));
            if (Convert.ToInt32(row["Format"].ToString()) != 5)
            {

                if (Convert.ToBoolean(row["UseFormula"]) == true)
                {
                    //  [Schedule date] + Days(50) 
                    if (row["Formula"].ToString() != "")
                    {

                        //Formular : if    [Fire Test Date] == ''   then [Schedule date] + Days(90)  Else [Fire Test Date]   + Days(90)
                        Regex regex = new Regex(@"Else *.*");
                        Match match = regex.Match(row["Formula"].ToString());
                        String strElse = match.Value;
                        //Get  control Name
                        Regex regex_ControlID = new Regex(@"\[.*?\]");
                        Match match_ThenControlID = regex_ControlID.Match(strElse);
                        String strControlID = match_ThenControlID.Value.Replace("[", "").Replace("]", "");
                        //Get Date
                        Regex regex_XDay = new Regex(@"\(.*?\)");
                        Match match_XDay = regex_XDay.Match(strElse);
                        String strXDay = match_XDay.Value.Replace("(", "").Replace(")", "");

                        //Get Shcedule Date
                        Match match_XScheduleDay = regex_XDay.Match(row["Formula"].ToString());
                        String strXScheduleDay = match_XScheduleDay.Value.Replace("(", "").Replace(")", "");

                        if (!ls.Contains(strControlID) && strXDay != "")
                        {
                            CustomFieldID custom = new CustomFieldID();
                            custom.controlName = strControlID;
                            custom.ControlUpdate = "CustomField_" + row["Label"].ToString().Replace(" ", "") + "_" + row["ID"].ToString();
                            try
                            {
                                custom.xFireDay = Convert.ToInt32(strXDay);
                                custom.xScheduleDay = Convert.ToInt32(strXScheduleDay);
                            }
                            catch (Exception ex)
                            {
                                custom.xFireDay = 0;
                                custom.xScheduleDay = 0;
                            }

                            lsFormula.Add(custom);
                        }

                    }

                }
                else
                {
                    lsControlName.Add("CustomField_" + row["Label"].ToString().Replace(" ", "") + "_" + row["ID"].ToString());                    
                }
            }
            else
            {
                lsControlName.Add("CustomField_" + row["Label"].ToString().Replace(" ", "") + "_" + row["ID"].ToString());                
            }
        }

        RadGrid_SafetyTest.AutoGenerateColumns = false;
        foreach (DataRow row in dtColumns.Rows)
        {



            if (row["Label"].ToString().Replace(" ", "") == "FireTestDate")
            {
                FireTestDate = "CustomField_" + row["Label"].ToString().Replace(" ", "") + "_" + row["ID"].ToString();
            }

            if (Convert.ToInt32(row["Format"].ToString()) == 4)
            {


                GridDropDownColumn ncol = new GridDropDownColumn();
                RadGrid_SafetyTest.MasterTableView.Columns.Add(ncol);
                ncol.DataField = "CustomField_" + row["Label"].ToString().Replace(" ", "") + "_" + row["ID"].ToString();
                ncol.HeaderText = row["Label"].ToString();
                ncol.HeaderStyle.Width = 100;
                ncol.AutoPostBackOnFilter = true;
                ncol.ShowFilterIcon = false;
                ncol.DataSourceID = "CustomField_" + row["Label"].ToString().Replace(" ", "") + "_" + row["ID"].ToString();
                ncol.ListValueField = "Value";
                ncol.ListTextField = "Value";
                ncol.DataType = typeof(System.String);

            }
            else
            {
                if (Convert.ToInt32(row["Format"].ToString()) == 5)
                {

                    GridDropDownColumn ncol = new GridDropDownColumn();
                    RadGrid_SafetyTest.MasterTableView.Columns.Add(ncol);
                    ncol.DataField = "CustomField_" + row["Label"].ToString().Replace(" ", "") + "_" + row["ID"].ToString();
                    ncol.HeaderText = row["Label"].ToString();
                    ncol.HeaderStyle.Width = 100;
                    ncol.AutoPostBackOnFilter = true;
                    ncol.ShowFilterIcon = false;
                    ncol.DataSourceID = "CustomField_" + row["Label"].ToString().Replace(" ", "") + "_" + row["ID"].ToString();
                    ncol.ListValueField = "Value";
                    ncol.ListTextField = "Value";
                    ncol.DataType = typeof(System.String);
                }
                else
                {
                    String id = "CustomField_" + row["Label"].ToString().Replace(" ", "") + "_" + row["ID"].ToString();

                    GridBoundColumn ncol = new GridBoundColumn();
                    RadGrid_SafetyTest.MasterTableView.Columns.Add(ncol);
                    ncol.DataField = id;
                    ncol.HeaderText = row["Label"].ToString();
                    ncol.HeaderStyle.Width = 100;
                    ncol.AutoPostBackOnFilter = true;
                    ncol.ShowFilterIcon = false;
                    ncol.DataType = typeof(System.String);
                    ncol.CurrentFilterFunction = GridKnownFunction.Contains;

                    foreach (CustomFieldID item in lsFormula)
                    {
                        if (item.controlName == row["Label"].ToString())
                        {
                            item.controlID = id;
                        }
                    }


                }
            }






        }
        Session["FormulaControl"] = lsFormula;
        Session["CustomControl"] = lsControlName;
        Session["DynamicControls"] = DynamicControls;

        hdnFormularFieldID.Value = String.Join(",", ls);
        hdnFormularValue.Value = String.Join(",", lsFormula);
        hdnFireTestDate.Value = FireTestDate;
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        createDataSource();
        DefineGridStructure();

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["defaultPageLoad"] = "1";

            string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

            if (Request.Url.Scheme == "http" && SSL == "1")
            {
                string URL = Request.Url.ToString();
                URL = URL.Replace("http://", "https://");
                Response.Redirect(URL);
            }
            DefineCustomColumnStructure();
            if (string.IsNullOrWhiteSpace(GetDefaultGridColumnSettingsFromDb()))
            {

                BL_User objBL_User = new BL_User();
                User objProp_User = new User(); ;

                // Get initial grid settings
                var gridDefault = GetGridColumnSettings();
                // Save default settings to database
                objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
                objProp_User.UserID = 0;// UserId = 0 for default
                objProp_User.PageName = "SafetyTest.aspx";
                objProp_User.GridId = "RadGrid_SafetyTest";

                objBL_User.UpdateUserGridCustomSettings(objProp_User, gridDefault);


            }

            var currentDate = DateTime.Today;
            txtStartDate.Text = currentDate.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
            txtEndDate.Text = currentDate.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek) + 1).ToShortDateString();
            lblWeek.Attributes["class"] = "labelactive";
            BindDropDownlist();
            Session["PageType"] = "Safety";

            #region Show Selected Filter
            if (Request.QueryString["fil"] != null)
            {
                UpdateControl();
            }
            else
            {
                if (Convert.ToString(Request.QueryString["f"]) != "c")
                {
                    if (Session["STStartDate"] == null && Session["STEndDate"] == null)
                    {
                        txtStartDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                        txtEndDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                        lblWeek.Attributes.Add("class", "labelactive");
                        Session["STFreqToDate"] = txtEndDate.Text;
                        Session["STFreqfromDate"] = txtStartDate.Text;
                    }
                    else
                    {
                        txtStartDate.Text = Session["STFreqfromDate"].ToString();
                        txtEndDate.Text = Session["STFreqToDate"].ToString();
                    }
                }
                else
                {
                    Session["STStartDate"] = null;
                    Session["STEndDate"] = null;

                    txtStartDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                    txtEndDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                    lblWeek.Attributes.Add("class", "labelactive");
                    Session["STFreqToDate"] = txtEndDate.Text;
                    Session["STFreqfromDate"] = txtStartDate.Text;
                }
            }






            //if (Session["type"].ToString() == "c")
            //{
            //    BindDataSourceToGrid();
            //}

            #endregion

        }

        var reportDisplay = "";
        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SafetyReportDisplay"]))
        {
            reportDisplay = ConfigurationManager.AppSettings["SafetyReportDisplay"].ToString();
        }
        if (string.IsNullOrEmpty(reportDisplay))
        {
            LI1pnlGridButtons.Visible = false;
        }
        Permission();
        HighlightSideMenu("cntractsMgr", "lnk", "recurMgrSub");
        // GetTeamMember();
    }

    #region ::Method::

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

    private void BindDropDownlist()
    {
        if (string.IsNullOrEmpty(WebBaseUtility.ConnectionString))
            return;

        ddlTestTypes.Items.Clear();
        ddlSearch.Items.Clear();
        TestTypes _objProptesttypes = new TestTypes();
        BL_SafetyTest _objbltesttypes = new BL_SafetyTest();

        _objProptesttypes.ConnConfig = WebBaseUtility.ConnectionString;

        DataSet ds = _objbltesttypes.GetAllTestTypes(_objProptesttypes);
        ddlTestTypes.Items.Add(new ListItem("All", "0"));
        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlTestTypes.Items.Add(new ListItem((string)ds.Tables[0].Rows[i]["Name"], Convert.ToString((int)ds.Tables[0].Rows[i]["ID"])));
                }
            }
        }



        ddlSearch.Items.Add(new ListItem("All", string.Empty));
        ddlSearch.Items.Add(new ListItem("Customer", "Name"));
        ddlSearch.Items.Add(new ListItem("Location ID", "ID"));
        ddlSearch.Items.Add(new ListItem("Location Name", "Tag"));
        ddlSearch.Items.Add(new ListItem("Location Address", "Address"));
        ddlSearch.Items.Add(new ListItem("Equipment ID", "Unit"));
        ddlSearch.Items.Add(new ListItem("Status", "Status"));
        ddlSearch.Items.Add(new ListItem("Classification", "Classification"));
        ddlSearch.Items.Add(new ListItem("Billing Amount", "IsDefaultAmount"));
        if (Session["COPer"].ToString() == "1")
        {
            ddlSearch.Items.Add(new ListItem("Company", "Company"));
        }
    }

    private void Permission()
    {
        if (Convert.ToString(Session["type"]) == "c")
        {
            Response.Redirect("home.aspx");
        }
        if (Convert.ToString(Session["MSM"]) == "TS")
        {
            Response.Redirect("home.aspx");
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }
        // User Permission 
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

            /// SafetyTestsPermission ///////////////////------->

            string SafetyTestsPermission = ds.Rows[0]["SafetyTestsPermission"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["SafetyTestsPermission"].ToString();
            string ADD = SafetyTestsPermission.Length < 1 ? "Y" : SafetyTestsPermission.Substring(0, 1);
            string Edit = SafetyTestsPermission.Length < 2 ? "Y" : SafetyTestsPermission.Substring(1, 1);
            string Delete = SafetyTestsPermission.Length < 2 ? "Y" : SafetyTestsPermission.Substring(2, 1);
            string View = SafetyTestsPermission.Length < 4 ? "Y" : SafetyTestsPermission.Substring(3, 1);
            if (ADD == "N")
            {

                lnkAddnew.Visible = false;
            }
            if (Edit == "N")
            {
                btnEdit.Visible = false;
                lnkAssignTicket.Visible = false;
                LnkGenerateProposal.Visible = false;
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
        ds = objBL_User.GetUserPermissionByUserID(objPropUser);
        return ds.Tables[0];
    }

    private void RowSelect()
    {
        foreach (GridDataItem gr in RadGrid_SafetyTest.Items)
        {
            HiddenField hduid = (HiddenField)gr.FindControl("hduid");

            HiddenField hdnid = (HiddenField)gr.FindControl("hdnid");
            HiddenField hdnTestYear = (HiddenField)gr.FindControl("hdnTestYear");

            gr.Attributes["ondblclick"] = string.Format("location.href='AddTests.aspx?elv={0}&LID={1}&tyear={2}'", hduid.Value, hdnid.Value, hdnTestYear.Value);
        }
    }

    protected void RadGrid_SafetyTest_PreRender(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            //Rename column Header
            RadGrid_SafetyTest.Columns.FindByUniqueName("RouteName").HeaderText = getRouteLabel();

            BL_User objBL_User = new BL_User();
            User objProp_User = new User();

            DataSet ds = new DataSet();
            objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
            objProp_User.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
            objProp_User.PageName = "SafetyTest.aspx";
            objProp_User.GridId = "RadGrid_SafetyTest";
            ds = objBL_User.GetGridUserSettings(objProp_User);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //string columnSettings = "[{Name: \"BType\", Display: true, Width: 300},{Name: \"MatItem\", Display: false, Width: 300}]";
                var columnSettings = ds.Tables[0].Rows[0][0].ToString();
                var columnsArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ColumnSettings>>(columnSettings);

                var colIndex = 0;

                foreach (GridColumn column in RadGrid_SafetyTest.MasterTableView.OwnerGrid.Columns)
                {
                    colIndex++;
                    var clSetting = columnsArr.Where(t => t.Name.Equals(column.UniqueName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    if (clSetting != null)
                    {
                        column.Display = clSetting.Display;
                        if (colIndex >= 3 && clSetting.Width != 0)
                            column.HeaderStyle.Width = clSetting.Width;

                        column.OrderIndex = clSetting.OrderIndex;
                    }
                }

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "showhidebutton", "ShowRestoreGridSettingsButton();", true);
            }
        }


        // just for TEI
        if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("Transel"))
        {
            RadGrid_SafetyTest.MasterTableView.GetColumn("Capacity").Visible = true;
        }
        else
        {
            RadGrid_SafetyTest.MasterTableView.GetColumn("Capacity").Visible = false;
        }


        RadGrid_SafetyTest.MasterTableView.GetColumn("Company").Visible = Session["COPer"].ToString() == "1";

        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_SafetyTest);

        RowSelect();

    }

    protected void RadGrid_SafetyTest_ItemCreated(object sender, GridItemEventArgs e)
    {

        if (Convert.ToString(RadGrid_SafetyTest.MasterTableView.FilterExpression) != "")
        {
            lblRecordCount.Text = RadGrid_SafetyTest.MasterTableView.DataSourceCount + " Record(s) found";
        }
        else
        {
            lblRecordCount.Text = RadGrid_SafetyTest.VirtualItemCount + " Record(s) found";
        }

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
            //dropDown.FindItemByValue(e.Item.OwnerTableView.PageSize.ToString()).Selected = true;

        }

    }

    protected void RadGrid_SafetyTest_ItemEvent(object sender, GridItemEventArgs e)
    {
        int rowCount = 0;
        if (e.EventInfo is GridInitializePagerItem)
        {
            rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
        }
        lblRecordCount.Text = rowCount + " Record(s) found";
        //updpnl.Update();
    }

    bool isGrouping = false;
    private bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_SafetyTest.MasterTableView.FilterExpression != "" ||
            (RadGrid_SafetyTest.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_SafetyTest.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_SafetyTest_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindDataSourceToGrid();
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        RadGrid_SafetyTest.ExportSettings.FileName = "SafetyTest";
        RadGrid_SafetyTest.ExportSettings.IgnorePaging = true;
        RadGrid_SafetyTest.ExportSettings.ExportOnlyData = true;
        RadGrid_SafetyTest.ExportSettings.OpenInNewWindow = true;
        RadGrid_SafetyTest.ExportSettings.HideStructureColumns = true;
        RadGrid_SafetyTest.MasterTableView.UseAllDataFields = true;
        RadGrid_SafetyTest.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_SafetyTest.MasterTableView.ExportToExcel();
    }

    protected void RadGrid_SafetyTest_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 4;
        else
            currentItem = 5;
        if (e.Worksheet.Table.Rows.Count == RadGrid_SafetyTest.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_SafetyTest.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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

    private void RemoveActiveLabel()
    {
        lblDay.Attributes["class"] = string.Empty;
        lblWeek.Attributes["class"] = string.Empty;
        lblMonth.Attributes["class"] = string.Empty;
        lblQuarter.Attributes["class"] = string.Empty;
        lblYear.Attributes["class"] = string.Empty;
    }

    private void BindDataSourceToGrid()
    {



        DataTable dtFilter = new DataTable();
        dtFilter.Columns.Add("ColumnName", typeof(string));
        dtFilter.Columns.Add("ColumnValue", typeof(string));

        List<RetainFilter> filters = new List<RetainFilter>();
        String filterExpression = Convert.ToString(RadGrid_SafetyTest.MasterTableView.FilterExpression);
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["ST_FilterExpression"] != null && Convert.ToString(Session["ST_FilterExpression"]) != "" && Session["ST_Filters"] != null)
                {
                    RadGrid_SafetyTest.MasterTableView.FilterExpression = Convert.ToString(Session["ST_FilterExpression"]);
                    var filtersGet = Session["ST_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_SafetyTest.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
                if (Session["ST_SortOrderBy"] != null)
                {
                    GridSortExpression expression = new GridSortExpression();
                    expression.FieldName = Session["ST_SortOrderBy"].ToString();
                    expression.SortOrder = GridSortOrder.Descending;
                    if (Session["ST_SortType"].ToString() == "ASC")
                    {
                        expression.SortOrder = GridSortOrder.Ascending;
                    }
                    else
                    {
                        if (Session["ST_SortType"].ToString() == "")
                        {
                            expression.SortOrder = GridSortOrder.None;
                        }
                    }

                    RadGrid_SafetyTest.MasterTableView.SortExpressions.AddSortExpression(expression);

                }
            }
            else
            {
                Session["ST_FilterExpression"] = null;
                Session["ST_Filters"] = null;
                Session["ST_SortType"] = null;
                Session["ST_SortOrderBy"] = null;
            }
        }


        if (!IsGridPageIndexChanged)
        {
            RadGrid_SafetyTest.CurrentPageIndex = 0;
            Session["RadGrid_SafetyTestCurrentPageIndex"] = 0;
            ViewState["RadGrid_SafetyTestminimumRows"] = 0;
            ViewState["RadGrid_SafetyTestmaximumRows"] = RadGrid_SafetyTest.PageSize;
        }
        else
        {
            if (Session["RadGrid_SafetyTestCurrentPageIndex"] != null && Convert.ToInt32(Session["RadGrid_SafetyTestCurrentPageIndex"].ToString()) != 0
                && Request.QueryString["fil"] != null && Request.QueryString["fil"].ToString() == "1")
            {
                RadGrid_SafetyTest.CurrentPageIndex = Convert.ToInt32(Session["RadGrid_SafetyTestCurrentPageIndex"].ToString());
                ViewState["RadGrid_SafetyTestminimumRows"] = RadGrid_SafetyTest.CurrentPageIndex * RadGrid_SafetyTest.PageSize;
                ViewState["RadGrid_SafetyTestmaximumRows"] = (RadGrid_SafetyTest.CurrentPageIndex + 1) * RadGrid_SafetyTest.PageSize;

            }
        }

        if (string.IsNullOrEmpty(filterExpression) && Session["ST_FilterExpression"] != null)
        {
            filterExpression = Convert.ToString(Session["ST_FilterExpression"]);
        }


        isGridFilterSafetyTest = false;




        #region Save the Grid Filter
        //String filterExpression = Convert.ToString(RadGrid_SafetyTest.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["ST_FilterExpression"] = filterExpression;
            //List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_SafetyTest.MasterTableView.OwnerGrid.Columns)
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

            Session["ST_Filters"] = filters;
        }
        else
        {
            Session["ST_FilterExpression"] = null;
            Session["ST_Filters"] = null;
        }
        #endregion




        //objProp_Contracts.dtFilterByColumn = dtFilter;


        SafetyTest objproptest = new SafetyTest();
        BL_SafetyTest objtestbl = new BL_SafetyTest();
        objproptest.ConnConfig = WebBaseUtility.ConnectionString;
        DataSet ds = null;

        var searchTerm = txtSearch.Text.Trim();
        var SearchValue = ddlSearch.SelectedValue;
        var type = ddlTestTypes.SelectedValue;
        var startDate = txtStartDate.Text + " 00:00:00";
        var endDate = txtEndDate.Text + " 23:59:59";
        var SortOrderBy = "LID";
        var SortType = "desc";

        objproptest.Typeid = string.IsNullOrEmpty(type) ? 0 : Convert.ToInt32(type);
        objproptest.Proposal = ddlProposal.SelectedValue;
        try
        {

            objproptest.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
            if (HttpContext.Current.Session["CmpChkDefault"].ToString() == "1")
            {
                objproptest.FlagEN = 1;
            }
            else
            {
                objproptest.FlagEN = 0;
            }
            if (txtStartDate.Text == "")
            {
                objproptest.Startdate = "01/01/1900";
                objproptest.Enddate = "01/01/3000";
            }
            else
            {
                objproptest.Startdate = startDate;
                objproptest.Enddate = endDate;
            }
            if (ddlSearch.SelectedValue == "IsDefaultAmount")
            {
                searchTerm = ddlBillingAmount.SelectedValue;
            }
            if (RadGrid_SafetyTest.MasterTableView.SortExpressions.Count > 0)
            {

                SortOrderBy = RadGrid_SafetyTest.MasterTableView.SortExpressions[0].FieldName;
                SortType = RadGrid_SafetyTest.MasterTableView.SortExpressions[0].SortOrderAsString();
                Session["ST_SortOrderBy"] = SortOrderBy;
                Session["ST_SortType"] = SortType;
            }


            {
                if (txtStartDate.Text == "")
                {
                    txtStartDate.Text = new DateTime(DateTime.Now.Year, 1, 1).Date.ToShortDateString();
                    txtEndDate.Text = new DateTime(DateTime.Now.Year, 12, 31).Date.ToShortDateString();
                    Session["STEndDate"] = txtEndDate.Text;
                    Session["STStartDate"] = txtStartDate.Text;
                    Session["STFreqToDate"] = txtEndDate.Text;
                    Session["STFreqfromDate"] = txtStartDate.Text;

                    objproptest.Startdate = txtStartDate.Text;
                    objproptest.Enddate = txtEndDate.Text;

                    lblWeek.Attributes.Remove("class");
                    lblMonth.Attributes.Remove("class");
                    lblQuarter.Attributes.Remove("class");
                    lblYear.Attributes.Add("class", "labelactive");
                    hdnRcvPymtSelectDtRange.Value = "Year";
                    IsGridPageIndexChanged = false;
                }
                else
                {
                    objproptest.Startdate = startDate;
                    objproptest.Enddate = endDate;
                }
                lnkChk.Visible = true;
            }
            String sqlFilter = getFilterOnGrid();
            Session["RP_SqlFilter"] = sqlFilter;
            Session["RP_SearchTerm"] = searchTerm;
            Session["RP_SortType"] = SortType;
            Session["RP_SortOrderBy"] = SortOrderBy;
            Session["RP_StartDate"] = objproptest.Startdate;
            Session["RP_EndDate"] = objproptest.Enddate;
            Session["RP_Proposal"] = objproptest.Proposal;




            string CustomerFiter = "NA";
            string LocationFiter = "NA";
            string LocationAddressFiter = "NA";
            string LocationAcctFiter = "NA";
            string LocationCityFiter = "NA";
            string LocationStateFiter = "NA";
            string EuipmentIDFiter = "NA";
            string UnitFiter = "NA";





            foreach (GridColumn column in RadGrid_SafetyTest.MasterTableView.OwnerGrid.Columns)
            {
                String filterValues = column.CurrentFilterValue;
                String columnName = column.UniqueName;
                if (filterValues != "")
                {

                    if (columnName == "Name" && filterValues.Length > 0) { CustomerFiter = filterValues; }

                    else if (columnName == "Tag" && filterValues.Length > 0) { LocationFiter = filterValues; }

                    else if (columnName == "Address" && filterValues.Length > 0) { LocationAddressFiter = filterValues; }

                    else if (columnName == "ID" && filterValues.Length > 0) { LocationAcctFiter = filterValues; }

                    else if (columnName == "City" && filterValues.Length > 0) { LocationCityFiter = filterValues; }

                    else if (columnName == "LocState" && filterValues.Length > 0) { LocationStateFiter = filterValues; }

                    else if (columnName == "Unit" && filterValues.Length > 0) { EuipmentIDFiter = filterValues; }

                    else if (columnName == "State" && filterValues.Length > 0) { UnitFiter = filterValues; }



                }
            }

            if (ViewState["defaultPageLoad"].ToString() == "1" && Request.QueryString["fil"] == null)
            {
                CustomerFiter = "ABC000";
                LocationFiter = "ABC000";
            }

            ds = objtestbl.GetTestDetailsAjaxSearch(objproptest, SearchValue, searchTerm, sqlFilter, SortOrderBy, SortType, ddldateRage123.SelectedValue
                , CustomerFiter,
                  LocationFiter,
                  LocationAddressFiter,
                  LocationAcctFiter,
                  LocationCityFiter,
                  LocationStateFiter,
                  EuipmentIDFiter,
                  UnitFiter);

            ViewState["defaultPageLoad"] = "0";

            if (ds != null)
            {
                // var temp = processDataFilter(ds.Tables[0]);

                var dt = ds.Tables[0];
                var dt2 = dt;
                // RadGrid_SafetyTest.AllowFilteringByColumn = false;
                RadGrid_SafetyTest.DataSource = dt2;
                RadGrid_SafetyTest.MasterTableView.FilterExpression = string.Empty;
                // RadGrid_SafetyTest.AllowFilteringByColumn = true;
                Session["safetytest"] = dt2;
                var rowCount = dt2.Rows.Count;
                RadGrid_SafetyTest.VirtualItemCount = rowCount;
                lblRecordCount.Text = rowCount + " Record(s) found";
                // ViewState["SafetyResultForEmail"] = dt;


            }
            else
            {
                RadGrid_SafetyTest.DataSource = string.Empty;
                RadGrid_SafetyTest.VirtualItemCount = 0;
                lnkEmail.Visible = false;
                lblRecordCount.Text = " 0 Record(s) found";
            }


            // RadPersistence_SafetyTest.SaveState();           
        }
        catch
        {
            RadGrid_SafetyTest.DataSource = string.Empty;
            RadGrid_SafetyTest.VirtualItemCount = 0;
            //TODO: Log error
        }

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
                        case "System.Web.UI.WebControls.HiddenField":
                            ((HiddenField)c).Value = string.Empty;
                            break;
                    }
                }
            }
        }
    }

    private List<string> ControlNotReset()
    {
        List<string> control = new List<string>();
        control.Add("txtStartDate");
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

    #endregion

    #region ::Events::
    protected void lnkAddTests_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddTests.aspx");
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {

        string s1 = ""; string s2 = ""; string s3 = "";

        bool chk = false;

        foreach (GridDataItem gr in RadGrid_SafetyTest.Items)
        {
            var chkSelect = (CheckBox)gr.FindControl("chkSelect");

            if (chkSelect.Checked)
            {

                var hduid = (HiddenField)gr.FindControl("hduid");

                var hdnid = (HiddenField)gr.FindControl("hdnid");

                var hdnYear = (HiddenField)gr.FindControl("hdnTestYear");

                chk = chkSelect.Checked; s1 = hduid.Value; s2 = hdnid.Value; s3 = hdnYear.Value;

                break;
            }


        }
        if (chk)
        {
            var redirect = string.Format("AddTests.aspx?elv={0}&LID={1}&tyear={2}", s1, s2, s3);

            Response.Redirect(redirect);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "keyTESTEDIT", "noty({text: 'Please select test',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {


        bool chk = false;

        foreach (GridDataItem gr in RadGrid_SafetyTest.Items)
        {
            var chkSelect = (CheckBox)gr.FindControl("chkSelect");

            if (chkSelect.Checked)
            {


                chk = chkSelect.Checked;

                break;
            }


        }
        if (chk)

        {

            int success = 0;
            bool hasError = false;
            var msg = new List<string>();

            foreach (GridDataItem gr in RadGrid_SafetyTest.Items)
            {
                var chkSelect = (CheckBox)gr.FindControl("chkSelect");

                if (chkSelect.Checked)
                {
                    var hdnid = (HiddenField)gr.FindControl("hdnid");
                    var hdnProposalId = (HiddenField)gr.FindControl("hdnProposalId");

                    var id = hdnid.Value;

                    SafetyTest objproptest = new SafetyTest();

                    BL_SafetyTest objtestbl = new BL_SafetyTest();

                    objproptest.ConnConfig = WebBaseUtility.ConnectionString;
                    objproptest.LID = Convert.ToInt32(id);

                    DataSet test = objtestbl.GetTestDetails(objproptest);
                    //Validate test
                    if (test.Tables[0].Rows.Count > 0)
                    {
                        success = -1;
                        //idTicket
                        if (test.Tables[0].Rows[0]["idTicket"] == DBNull.Value)
                        {
                            if (string.IsNullOrEmpty(Convert.ToString(test.Tables[0].Rows[0]["idTicket"])))
                            {
                                //delete test

                                objproptest.Typeid = test.Tables[0].Rows[0]["LTID"] != DBNull.Value ? Convert.ToInt32(test.Tables[0].Rows[0]["LTID"]) : 0;
                                objproptest.Equipid = test.Tables[0].Rows[0]["NID"] != DBNull.Value ? Convert.ToInt32(test.Tables[0].Rows[0]["NID"]) : 0;
                                objproptest.ProposalId = hdnProposalId.Value.ToString() == "" ? -1 : Convert.ToInt32(hdnProposalId.Value);
                                success = objtestbl.DeleteTest(objproptest);
                            }
                        }

                    }

                    switch (success)
                    {
                        case 0:
                            hasError = true;
                            msg.Add("Test " + objproptest.LID + " could not be deleted.");

                            break;
                        case 1:

                            msg.Add("Test " + objproptest.LID + " deleted successfully.");
                            break;
                        case -1:
                            hasError = true;

                            msg.Add("Test " + objproptest.LID + " has already been scheduled. You must cancel the ticket for this test prior to being able to delete the test itself.");
                            break;


                        default:
                            break;
                    }
                }

                if (msg.Any())
                {
                    var messageType = hasError ? "error" : "success";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showMessage", string.Format("ShowMessage('{0}','{1}');", string.Join(",", msg), messageType), true);
                }

            }
            RadGrid_SafetyTest.Rebind();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showMessage", string.Format("ShowMessage('{0}','{1}');", "Please select at least one record to delete.", "warning"), true);
            return;
        }
    }

    private bool IsCreditHold(int LID)
    {
        BL_General objBL_General = new BL_General();

        int _IsCreditHold = Convert.ToInt16(objBL_General.GetLocCredit(LID, Session["config"].ToString()));

        if (_IsCreditHold == 1)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "keyIsCreditHold", "noty({text: 'Location on credit hold!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            return true;
        }

        return false;

    }

    protected void lnkAssignTicket_Click(object sender, EventArgs e)
    {

        int RadGrid_SafetyTestSelectedItemsCount = 0;
        int RadGrid_PageCount = RadGrid_SafetyTest.PageSize;
        bool ForAll = true;

        foreach (GridDataItem gr in RadGrid_SafetyTest.Items)
        {
            var chkSelect = (CheckBox)gr.FindControl("chkSelect");
            if (chkSelect.Checked)
            {
                RadGrid_SafetyTestSelectedItemsCount += 1;

            }
        }

        if (RadGrid_SafetyTestSelectedItemsCount == 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "showMessage", string.Format("ShowMessage('{0}','{1}');", "Please select at least one record to assign.", "warning"), true);
            return;
        }

        if (RadGrid_SafetyTestSelectedItemsCount == RadGrid_PageCount)
        {
            RadGrid_SafetyTest.AllowPaging = false;
            RadGrid_SafetyTest.Rebind();
            foreach (GridDataItem gr in RadGrid_SafetyTest.Items)
            {
                var chkSelect = (CheckBox)gr.FindControl("chkSelect");
                chkSelect.Checked = true;
            }
            ForAll = false;
        }

        bool hasError = false;
        string msg = "";
        var success = 0;

        foreach (GridDataItem gr in RadGrid_SafetyTest.Items)
        {
            var chkSelect = (CheckBox)gr.FindControl("chkSelect");

            if (chkSelect.Checked)
            {
                var hdnid = (HiddenField)gr.FindControl("hdnid");
                var hdnTestYear = (HiddenField)gr.FindControl("hdnTestYear");
                var id = hdnid.Value;
                if (!IsCreditHold(Convert.ToInt32(hdnid.Value)))
                {
                    try
                    {
                        SafetyTest objproptest = new SafetyTest(); BL_SafetyTest objtestbl = new BL_SafetyTest();
                        objproptest.ConnConfig = WebBaseUtility.ConnectionString;
                        objproptest.LID = Convert.ToInt32(id);
                        objproptest.PriceYear = Convert.ToInt32(hdnTestYear.Value);
                        DataSet test = objtestbl.GetTestDetailsByYear(objproptest);
                        //Validate test
                        if (test.Tables[0].Rows.Count > 0)
                        {
                            string strticket = test.Tables[0].Rows[0]["idTicket"] != DBNull.Value ? Convert.ToString(test.Tables[0].Rows[0]["idTicket"]) : string.Empty;
                            objproptest.OldStatus = Convert.ToInt32(test.Tables[0].Rows[0]["StatusValue"].ToString());
                            DateTime? Lastdate = null;
                            if (test.Tables[0].Rows[0]["Last"] != DBNull.Value) { Lastdate = Convert.ToDateTime(test.Tables[0].Rows[0]["Last"]); }
                            //No tickets assigned

                            if (string.IsNullOrEmpty(strticket))
                            {
                                objproptest.Status = AssignedStatusValue;
                                objproptest.Statusstr = "Assigned";
                                objproptest.Lastdate = Lastdate;
                                objproptest.UserName = Convert.ToString(HttpContext.Current.Session["username"]);

                                if (hdnCreateTicketForAll.Value == "1" && ForAll == true)
                                {
                                    success = objtestbl.CreateTicketsByYearForAllTestInLocation(objproptest);
                                }
                                else
                                {
                                    success = objtestbl.CreateTicketByYear(objproptest);
                                }
                            }
                            else success = -2;
                        }
                    }
                    catch
                    {
                        hasError = true;
                    }
                    finally
                    {
                        if (success == 0)
                        {
                            hasError = true; msg = "Ticket could not be created.";
                        }
                        if (success == 1)
                        {
                            hasError = false; msg = "Ticket created successfully.";
                        }
                        if (success == -1)
                        {
                            hasError = false; msg = "Ticket could not be created.";
                        }
                        if (success == -2)
                        {
                            hasError = true; msg = "Cannot create ticket .Already assigned.";
                        }
                    }
                }
            }
        }


        if (msg.Length > 5)
        {
            var messageType = hasError ? "warning" : "success";

            string str = msg;

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "NoShowMessageError", "noty({text: '" + str + "', dismissQueue: true,  type :  '" + messageType.ToString() + "', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }

        RadGrid_SafetyTest.AllowPaging = true;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "keyRefreshGrid", "document.getElementById('ctl00_ContentPlaceHolder1_lnkSearch').click();", true);
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        try
        {


            IsGridPageIndexChanged = false;
            isShowAll.Value = "0";
            hdnRcvPymtSelectDtRange.Value = "";
            lblDay.Attributes.Remove("class");
            lblWeek.Attributes.Remove("class");
            lblMonth.Attributes.Remove("class");
            lblQuarter.Attributes.Remove("class");
            lblYear.Attributes.Remove("class");
            if (txtEndDate.Text == "" || txtStartDate.Text == "")
            {
                txtStartDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                txtEndDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
            }
            Session["STEndDate"] = txtEndDate.Text;
            Session["STStartDate"] = txtStartDate.Text;
            Session["STFreqToDate"] = txtEndDate.Text;
            Session["STFreqfromDate"] = txtStartDate.Text;
            SaveFilter();
            RadGrid_SafetyTest.Rebind();
            //showFilterSearch();
            UpdateControl();


        }
        catch { }
    }

    private bool IsDateSelectionValid()
    {
        DateTime result;
        return DateTime.TryParse(txtStartDate.Text, out result) && DateTime.TryParse(txtEndDate.Text, out result);
    }

    protected void lnkShowAll_Click(object sender, EventArgs e)
    {

        txtStartDate.Text = new DateTime(DateTime.Now.Year, 1, 1).Date.ToShortDateString();
        txtEndDate.Text = new DateTime(DateTime.Now.Year, 12, 31).Date.ToShortDateString();
        Session["STEndDate"] = txtEndDate.Text;
        Session["STStartDate"] = txtStartDate.Text;
        Session["STFreqToDate"] = txtEndDate.Text;
        Session["STFreqfromDate"] = txtStartDate.Text;
        ddlSearch.SelectedIndex = 0;
        ddlTestTypes.SelectedIndex = 0;
        ddlProposal.SelectedIndex = 0;
        ddlBillingAmount.SelectedIndex = 0;
        ddlBillingAmount.SelectedIndex = 0;
        ddlBillingAmount.Style.Add("display", "none");
        txtSearch.Style.Add("display", "block");
        txtSearch.Text = "";
        if (Session["ST_FilterExpression"] != null && Convert.ToString(Session["ST_FilterExpression"]) != "" && Session["ST_Filters"] != null)
        {

            foreach (GridColumn column in RadGrid_SafetyTest.MasterTableView.OwnerGrid.Columns)
            {
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                column.CurrentFilterValue = string.Empty;
                column.ListOfFilterValues = null;
            }
            RadGrid_SafetyTest.MasterTableView.FilterExpression = string.Empty;
        }
        Session["ST_FilterExpression"] = null;
        Session["ST_Filters"] = null;

        //Uncheck Show Inactive checkbox
        lnkIncludeInActiveTest.Checked = false;

        //GetInvoices(0);
        RadGrid_SafetyTest.Rebind();
        SaveFilter();

        //txtStartDate.Text = string.Empty;
        //txtEndDate.Text = string.Empty;
        isShowAll.Value = "1";
        lblWeek.Attributes.Remove("class");
        lblMonth.Attributes.Remove("class");
        lblQuarter.Attributes.Remove("class");
        lblYear.Attributes.Add("class", "labelactive");
        hdnRcvPymtSelectDtRange.Value = "Year";
        IsGridPageIndexChanged = false;
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        IsGridPageIndexChanged = false;
        Session["STfilterstate"] = null;

        if (Session["ST_FilterExpression"] != null && Convert.ToString(Session["ST_FilterExpression"]) != "" && Session["ST_Filters"] != null)
        {

            foreach (GridColumn column in RadGrid_SafetyTest.MasterTableView.OwnerGrid.Columns)
            {
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                column.CurrentFilterValue = string.Empty;
                column.ListOfFilterValues = null;
            }
            RadGrid_SafetyTest.MasterTableView.FilterExpression = string.Empty;
        }


        Session["ST_FilterExpression"] = null;
        Session["ST_Filters"] = null;
        if (lnkIncludeInActiveTest.Checked)
        {
            resetClearForInactiveChecked();
        }
        else
        {
            if (isShowAll.Value == "1")
            {
                txtStartDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                txtEndDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();

                resetShowAll();
            }
            else
            {
                txtStartDate.Text = Session["STFreqfromDate"].ToString();
                txtEndDate.Text = Session["STFreqToDate"].ToString();

                resetClear();
            }
        }

        // isShowAll.Value = "0";
        ddlSearch.SelectedIndex = 0;
        ddlTestTypes.SelectedIndex = 0;
        ddlProposal.SelectedIndex = 0;
        ddlBillingAmount.SelectedIndex = 0;
        ddlBillingAmount.Style.Add("display", "none");
        txtSearch.Style.Add("display", "block");
        txtSearch.Text = "";


        RadGrid_SafetyTest.Rebind();
        if (isShowAll.Value == "1")
        {
            txtStartDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
            txtEndDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
        }
        else
        {
            txtStartDate.Text = Session["STFreqfromDate"].ToString();
            txtEndDate.Text = Session["STFreqToDate"].ToString();
        }
        isShowAll.Value = "0";
        Session["STFreqToDate"] = txtEndDate.Text;
        Session["STFreqfromDate"] = txtStartDate.Text;
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    protected void rdDay_CheckedChanged(object sender, EventArgs e)
    {
        ddlSearch.SelectedIndex = 0;
        ddlTestTypes.SelectedIndex = 0;
        txtSearch.Text = "";
        RemoveActiveLabel();
        lblDay.Attributes["class"] = "labelactive";
        txtStartDate.Text = DateTime.Now.ToShortDateString();
        txtEndDate.Text = DateTime.Now.ToShortDateString();
        Session["ToDate"] = txtEndDate.Text;
        Session["fromDate"] = txtStartDate.Text;
        foreach (GridColumn column in RadGrid_SafetyTest.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        RadGrid_SafetyTest.MasterTableView.FilterExpression = string.Empty;
        RadGrid_SafetyTest.Rebind();

    }

    protected void rdWeek_CheckedChanged(object sender, EventArgs e)
    {
        RemoveActiveLabel();
        lblWeek.Attributes["class"] = "labelactive";
        var now = System.DateTime.Now;
        var FisrtDay = now.AddDays(-((now.DayOfWeek - System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek + 7) % 7)).Date;
        txtStartDate.Text = FisrtDay.ToShortDateString();
        var LastDay = FisrtDay.AddDays(7).AddSeconds(-1);

        txtEndDate.Text = LastDay.ToShortDateString();
        Session["ToDate"] = txtEndDate.Text;
        Session["fromDate"] = txtStartDate.Text;
    }

    protected void rdMonth_CheckedChanged(object sender, EventArgs e)
    {
        RemoveActiveLabel();
        lblMonth.Attributes["class"] = "labelactive";
        var Date = System.DateTime.Now;
        var firstDayOfMonth = new DateTime(Date.Year, Date.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        txtStartDate.Text = firstDayOfMonth.ToShortDateString();
        txtEndDate.Text = lastDayOfMonth.ToShortDateString();
        Session["ToDate"] = txtEndDate.Text;
        Session["fromDate"] = txtStartDate.Text;
    }

    protected void rdQuarter_CheckedChanged(object sender, EventArgs e)
    {
        RemoveActiveLabel();
        lblQuarter.Attributes["class"] = "labelactive";
        var date = System.DateTime.Now;
        int quarterNumber = (date.Month - 1) / 3 + 1;
        DateTime firstDayOfQuarter = new DateTime(date.Year, (quarterNumber - 1) * 3 + 1, 1);
        DateTime lastDayOfQuarter = firstDayOfQuarter.AddMonths(3).AddDays(-1);
        txtStartDate.Text = firstDayOfQuarter.ToShortDateString();
        txtEndDate.Text = lastDayOfQuarter.ToShortDateString();
        Session["ToDate"] = txtEndDate.Text;
        Session["fromDate"] = txtStartDate.Text;
    }

    protected void rdYear_CheckedChanged(object sender, EventArgs e)
    {
        RemoveActiveLabel();
        lblYear.Attributes["class"] = "labelactive";
        int year = DateTime.Now.Year;
        DateTime firstDay = new DateTime(year, 1, 1);
        DateTime lastDay = new DateTime(year, 12, 31);
        txtStartDate.Text = firstDay.ToShortDateString();
        txtEndDate.Text = lastDay.ToShortDateString();
        Session["ToDate"] = txtEndDate.Text;
        Session["fromDate"] = txtStartDate.Text;
    }

    #endregion
    #region GeneralProposal
    protected void LnkGenerateProposal_Click(object sender, EventArgs e)
    {


        ProposalData proposal = new ProposalData();
        TestSetupForm formPro = getTestSetupForm(1);
        if (formPro != null)
        {
            proposal.FromDate = Convert.ToDateTime(txtStartDate.Text);
            proposal.ToDate = Convert.ToDateTime(txtEndDate.Text);

            DataTable dtResult = processDataFilter((DataTable)Session["safetytest"]);

            if (dtResult != null)
            {


                proposal.lsTestProposalDetail = getDataTestCoverToGeneralProposal(dtResult, 1, 1);
                doGenerateTestCoverProposalTemp(proposal.lsTestProposalDetail, formPro, 1, 1);

                proposal.lsTestProposalDetail = getDataTestCoverToGeneralProposal(dtResult, 0, 1);
                doGenerateTestCoverProposalTemp(proposal.lsTestProposalDetail, formPro, 0, 1);

                proposal.lsTestProposalDetail = getDataChargeableToGeneralProposal(dtResult, 1);
                doGenerateProposalTemp(proposal.lsTestProposalDetail, formPro, 1);


                proposal.lsTestProposalDetail = getDataChargeableToGeneralProposal(dtResult, 0);
                doGenerateProposalTemp(proposal.lsTestProposalDetail, formPro, 0);

                String message = "Generated proposal successfully";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySafetyTest", "noty({text: '" + message + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }

        }
        else
        {
            string str = "You do not have Form. Please add Form in Setup page";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySafetyTest", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

        }
    }

    private DataTable processDataFilter(DataTable dt)
    {
        DataTable result = dt;
        try
        {
            String sql = "1=1";

            foreach (GridColumn column in RadGrid_SafetyTest.MasterTableView.OwnerGrid.Columns)
            {
                String filterValues = String.Empty;
                String columnName = column.UniqueName;
                filterValues = column.CurrentFilterValue;
                if (filterValues.Trim() != "")
                {
                    if (column.UniqueName == "ProposalID")
                    {
                        sql = sql + " And " + column.UniqueName + "=" + filterValues.Trim() + "";
                    }
                    else
                    {
                        if (column.UniqueName == "BillingAmount")
                        {
                            sql = sql + " And " + column.UniqueName + "=" + filterValues.Trim() + "";
                        }
                        else
                        {
                            if (column.UniqueName == "EDate" || column.UniqueName == "Next")
                            {
                                sql = sql + " And " + column.UniqueName + "='" + filterValues.Trim() + "'";
                            }
                            else
                            {
                                sql = sql + " And " + column.UniqueName + " like '%" + filterValues.Trim() + "%'";
                            }
                        }

                    }


                }




            }
            if (result.Select(sql).Count() > 0)
            {
                return result.Select(sql).CopyToDataTable();
            }
            else
            {
                return null;
            }

        }
        catch (Exception ex)
        {
            return dt;
        }
    }

    private String getFilterOnGrid()
    {

        try
        {
            String sql = " 1=1 ";

            foreach (GridColumn column in RadGrid_SafetyTest.MasterTableView.OwnerGrid.Columns)
            {
                String filterValues = String.Empty;
                String columnName = column.UniqueName;
                filterValues = column.CurrentFilterValue;
                if (filterValues.Trim() != "" && columnName != "Unit" && columnName != "State")
                {
                    if (column.UniqueName == "ProposalID")
                    {
                        sql = sql + " And " + column.UniqueName + "=" + filterValues.Trim() + "";
                    }
                    else
                    {
                        if (column.UniqueName == "BillingAmount")
                        {
                            sql = sql + " And " + column.UniqueName + "=" + filterValues.Trim() + "";
                        }
                        else
                        {
                            if (column.UniqueName == "EDate" || column.UniqueName == "Next")
                            {
                                char delimiter = '-';
                                bool isFilterDateRange = false;
                                if (filterValues.Contains(delimiter))
                                {
                                    isFilterDateRange = true;
                                }
                                if (isFilterDateRange)
                                    sql = sql + " And " + $"CAST({column.UniqueName} AS DATE)" + " BETWEEN " + $"'{filterValues.Trim().Split(delimiter)[0]}'" + " AND " + $"'{filterValues.Trim().Split(delimiter)[1]}'" + " ";
                                else
                                    sql = sql + " And " + $"CAST({column.UniqueName} AS DATE)" + "=" + $"'{filterValues.Trim()}'" + "";

                            }
                            else
                            {
                                Dictionary<string, CustomField> dynamicControls = Session["DynamicControls"] as Dictionary<string, CustomField>;

                                if (dynamicControls.Any() && dynamicControls.Any(x => x.Key.ToLower() == column.UniqueName.ToLower()))
                                {

                                    char delimiter = '-';
                                    bool isFilterDateRange = false;
                                    if (filterValues.Contains(delimiter))
                                    {
                                        isFilterDateRange = true;
                                    }
                                    foreach (var control in dynamicControls)
                                    {
                                        if (column.UniqueName == control.Key)
                                        {

                                            switch (control.Value)
                                            {

                                                case CustomField.Date:
                                                    if (isFilterDateRange)
                                                        sql = sql + " And " + $"CAST({column.UniqueName} AS DATE)" + " BETWEEN " + $"'{filterValues.Trim().Split(delimiter)[0]}'" + " AND " + $"'{filterValues.Trim().Split(delimiter)[1]}'" + " ";
                                                    else
                                                        sql = sql + " And " + $"CAST({column.UniqueName} AS DATE)" + "=" + $"'{filterValues.Trim()}'" + "";
                                                    break;
                                                case CustomField.Currency:
                                                case CustomField.Text:
                                                case CustomField.Dropdown:
                                                case CustomField.Checkbox:
                                                    sql = sql + " And " + column.UniqueName + " like '%" + filterValues.Trim() + "%'";
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    sql = sql + " And " + column.UniqueName + " like '%" + filterValues.Trim() + "%'";
                                }
                            }
                        }

                    }
                }
            }


            if (!lnkIncludeInActiveTest.Checked)
            {
                sql += "  and Status <>  'InActive'  ";
            }
            else { sql += "  and Status =  'InActive'  "; }

            if (!lnkChk.Checked)
            {
                sql += "  and   TicketStatusText<> 'Assigned'  and TicketStatusText<> 'Completed'    ";
            }


            return sql;

        }
        catch (Exception ex)
        {
            return "1=1";
        }
    }
    private TestSetupForm getTestSetupForm(int type)
    {
        TestSetupForm obj = new TestSetupForm();
        BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
        DataSet ds = bl_SafetyTest.GetTestSetupFormsByType(Session["config"].ToString(), type);
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                PopulateFields(obj, ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }

        }
        else
        {
            return null;
        }
        return obj;
    }

    public void PopulateFields(TestSetupForm et, DataRow dr)
    {
        et.Id = Convert.ToInt32(dr["ID"]);
        et.Name = dr["Name"].ToString();
        et.FileName = dr["FileName"].ToString();
        et.FilePath = dr["FilePath"].ToString();
        et.MIME = dr["MIME"].ToString();
        et.Type = Convert.ToInt32(dr["Type"]);
        et.AddedBy = dr["AddedBy"].ToString();
        et.UpdatedBy = dr["UpdatedBy"].ToString();
        if (dr["UpdatedOn"].ToString() != "")
        {
            et.UpdatedOn = Convert.ToDateTime(dr["UpdatedOn"]);
        }
        if (dr["AddedOn"].ToString() != "")
        {
            et.AddedOn = Convert.ToDateTime(dr["AddedOn"]);
        }

    }

    public DataTable GetCompanyDetails()
    {
        //Company details
        var connString = Session["config"].ToString();
        BL_Report objBL_Report = new BL_Report();
        BL_User objBL_User = new BL_User();

        User objPropUser = new User();
        objPropUser.ConnConfig = connString;

        DataSet companyInfo = companyInfo = objBL_Report.GetCompanyDetails(Session["config"].ToString());

        return companyInfo.Tables[0];
    }

    public void doGenerateTestCoverProposalTemp(List<TestProposalDetail> lsTestProposal, TestSetupForm formPro, int isChargeable, int hasChild)
    {
        string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
        string savepath = savepathconfig + "\\" + Session["dbname"] + "\\SafetyTest\\";
        if (!Directory.Exists(savepath))
        {
            Directory.CreateDirectory(savepath);
        }

        string guid = System.Guid.NewGuid().ToString();
        string pathDoc = String.Empty;
        string pathPDF = String.Empty;
        try
        {

            foreach (TestProposalDetail testProposal in lsTestProposal)
            {
                if (testProposal.ProposalEquipment.Count > 0)
                {
                    //get company information
                    DataTable dtCompany = GetCompanyDetails();
                    string CompanyName = dtCompany.Rows[0]["Name"].ToString();
                    string CompanyAddress = dtCompany.Rows[0]["Address"].ToString();
                    string CompanyPhoneNumber = dtCompany.Rows[0]["Phone"].ToString();
                    string CompanyFax = dtCompany.Rows[0]["Fax"].ToString();
                    string CompanyEmail = dtCompany.Rows[0]["Email"].ToString();
                    string CompanyCity = dtCompany.Rows[0]["City"].ToString();
                    string CompanyState = dtCompany.Rows[0]["State"].ToString();
                    string CompanyZip = dtCompany.Rows[0]["Zip"].ToString();

                    //get location information
                    DataTable dtLoc = testProposal.LocationInfo;
                    String LocName = dtLoc.Rows[0]["Tag"].ToString();
                    String LocAddress = dtLoc.Rows[0]["locAddress"].ToString();

                    //get customer information
                    DataTable dtCus = testProposal.CustomerInfo;
                    String customername = dtCus.Rows[0]["custname"].ToString();
                    String customerAddress = dtCus.Rows[0]["custaddress"].ToString();
                    String customerCity = dtCus.Rows[0]["custcity"].ToString();
                    String customerState = dtCus.Rows[0]["custstate"].ToString();
                    String customerZip = dtCus.Rows[0]["custzip"].ToString();

                    String mainContact = dtLoc.Rows[0]["Contact"].ToString();
                    String LocationTag = dtLoc.Rows[0]["tag"].ToString();
                    Boolean ThirdParty = false;
                    String ThirdPartyName = "";

                    String ClassificationName = "";
                    String Remark = "";
                    String TestType = "";
                    ClassificationName = testProposal.Classification == null ? "" : testProposal.Classification;
                    Remark = testProposal.Remark == null ? "" : testProposal.Remark;
                    TestType = testProposal.TestType == null ? "" : testProposal.TestType;
                    //CreateFile
                    String proposalFileName = LocName.Replace(" ", "").Replace("\\", "").Replace("/", "").Replace("*", "") + "_" + ClassificationName + DateTime.Now.ToString("MMddyyyyhhmmssfff");
                    proposalFileName = proposalFileName.Replace(" ", "").Replace("\\", "").Replace("/", "").Replace("*", "").Replace(",", "").Replace("?", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace(":", "");
                    pathDoc = savepath + proposalFileName + "." + formPro.MIME;
                    pathPDF = savepath + proposalFileName + "." + "pdf";

                    File.Copy(formPro.FilePath, pathDoc);
                    using (DocX document = DocX.Load(pathDoc))
                    {
                        document.ReplaceText("{LocationTag}", LocationTag, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{MainContract}", mainContact, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{CustomerName}", customername, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{CustomerAddress}", customerAddress, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{CustomerCity}", customerCity, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{CustomerState}", customerState, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{CustomerZip}", customerZip, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{Location}", LocName, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{LocationAddress}", LocAddress.Replace("\n", ""), false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{Year}", testProposal.YearProposal.ToString(), false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{FromDate}", txtStartDate.Text, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{ToDate}", txtEndDate.Text, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{ProposalDate}", DateTime.Now.ToString("MMM dd, yyyy"), false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{LocationCity}", dtLoc.Rows[0]["locCity"].ToString(), false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{LocationState}", dtLoc.Rows[0]["locState"].ToString(), false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{LocationZip}", dtLoc.Rows[0]["locZip"].ToString(), false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{Phone}", dtLoc.Rows[0]["Phone"].ToString(), false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{Mobile}", dtLoc.Rows[0]["Cellular"].ToString(), false, RegexOptions.IgnoreCase);

                        document.ReplaceText("{Classification}", ClassificationName, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{Remark}", Remark, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{TestType}", TestType, false, RegexOptions.IgnoreCase);

                        document.ReplaceText("{DefaultAmount}", testProposal.DefaultAmount.ToString("C"), false, RegexOptions.IgnoreCase);

                        //Company information
                        document.ReplaceText("{CompanyName}", CompanyName, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{CompanyAddress}", CompanyAddress, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{CompanyPhoneNumber}", CompanyPhoneNumber, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{CompanyFax}", CompanyFax, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{CompanyEmail}", CompanyEmail, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{CompanyCity}", CompanyCity, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{CompanyState}", CompanyState, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{CompanyZip}", CompanyZip, false, RegexOptions.IgnoreCase);

                        //Location Billing Information
                        document.ReplaceText("{LocationBillingAddress}", dtLoc.Rows[0]["Address"].ToString(), false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{LocationBillingCity}", dtLoc.Rows[0]["City"].ToString(), false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{LocationBillingState}", dtLoc.Rows[0]["State"].ToString(), false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{LocationBillingZip}", dtLoc.Rows[0]["Zip"].ToString(), false, RegexOptions.IgnoreCase);

                        if (hasChild == 0)
                        {
                            document.ReplaceText("{TestTypeCoverNote}", TestType, false, RegexOptions.IgnoreCase);
                        }
                        else
                        {
                            if (testProposal.TestTypeCoverName == "" || testProposal.TestTypeCoverName == null)
                            {
                                document.ReplaceText("{TestTypeCoverNote}", TestType, false, RegexOptions.IgnoreCase);
                            }
                            else
                            {
                                document.ReplaceText("{TestTypeCoverNote}", testProposal.TestTypeCoverName + " and " + TestType + " inspections", false, RegexOptions.IgnoreCase);

                            }
                        }


                        List<String> lsStrTemplate = document.FindUniqueByPattern(@"(\[\[\[)([^\]]*)(\]\]\])", RegexOptions.IgnoreCase);
                        List<String> lsStrReplace = new List<string>();

                        List<String> lsEquipID = new List<string>();

                        StringBuilder strContent = new StringBuilder();



                        var rowCount = testProposal.ProposalEquipment.Count / 4;
                        Table t = document.AddTable(rowCount + 1, 4);
                        // Specify some properties for this Table.                          
                        t.Alignment = Alignment.left;

                        //t.Paragraphs[0].Font(new FontFamily("Times New Roman"));
                        //t.Paragraphs[0].FontSize(12);
                        t.SetColumnWidth(0, 2400);
                        t.SetColumnWidth(1, 2400);
                        t.SetColumnWidth(2, 2400);
                        t.SetColumnWidth(3, 2400);
                        Border c = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.one, 0, Color.Transparent);

                        int n = 0;
                        int m = 0;
                        List<Double> lsPrice = new List<double>();
                        foreach (ProposalEquipment itemEquip in testProposal.ProposalEquipment)
                        {
                            t.Rows[n].Cells[m].Paragraphs.First().Append(itemEquip.unit.ToString());
                            t.Rows[n].Cells[m].Paragraphs[0].Font(new FontFamily("Times New Roman"));
                            t.Rows[n].Cells[m].Paragraphs[0].FontSize(12);
                            t.Rows[n].Cells[m].Paragraphs[0].SpacingBefore(0);
                            t.Rows[n].Cells[m].Paragraphs[0].SpacingAfter(0);
                            m = m + 1;
                            if (m == 4)
                            {
                                m = 0; n = n + 1;
                            }

                            lsEquipID.Add(itemEquip.ID.ToString());
                            lsPrice.Add(itemEquip.Amount);
                            if (itemEquip.ThirdPartyRequired == true)
                            {
                                ThirdParty = true;
                                ThirdPartyName = itemEquip.ThirdPartyName;
                            }

                        }
                        Border b = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.one, 0, Color.Transparent);
                        t.SetBorder(TableBorderType.InsideH, b);
                        t.SetBorder(TableBorderType.InsideV, b);
                        t.SetBorder(TableBorderType.Bottom, b);
                        t.SetBorder(TableBorderType.Top, b);
                        t.SetBorder(TableBorderType.Left, b);
                        t.SetBorder(TableBorderType.Right, b);

                        // Insert the Table into the document.
                        try
                        {
                            foreach (var paragraph in document.Paragraphs)
                            {
                                paragraph.FindAll("{UnitNumber}").ForEach(index => paragraph.InsertTableAfterSelf((t)));
                            }
                            document.ReplaceText("{UnitNumber}", "", false, RegexOptions.IgnoreCase);
                        }
                        catch (Exception)
                        {
                            document.InsertTable(t);
                        }

                        var groupPrice = lsPrice
                            .GroupBy(i => i) //Group the words
                            .Select(i => new { Amount = i.Key, Count = i.Count() });
                        String ReguiredInformation = "";
                        String ThridPartyRequired = "";
                        String ThirdPartyContact = "";
                        String ThirdPartyNameContent = "";
                        String ThirdPartyWarning = "";
                        String ThirdPartyMsg = "Kindly sign and return to us immediately so we may schedule your inspection accordingly";
                        if (ThirdParty == true)
                        {
                            ThridPartyRequired = "This inspection requires your hiring a Private Elevator Inspection Agency to witness this inspection.";
                            ThirdPartyNameContent = "Name of 3rd  Party Company:" + ThirdPartyName;
                            ThirdPartyContact = "Contact Person:_________________";
                            ThirdPartyWarning = "**WE CAN NOT ACCEPT PROPOSAL, IF THE THIRD PARTY INFORMATION IS NOT PROVIDED";
                            ThirdPartyMsg = "Kindly sign and provide your Third Party Witnessing Company information below and return to us immediately so we may schedule your inspection accordingly.";
                            ReguiredInformation = "** REQUIRED INFORMATION:";

                            document.ReplaceText("{ThirdpartyName}", ThirdPartyName, false, RegexOptions.IgnoreCase);
                        }
                        document.ReplaceText("{ThridPartyRequired}", ThridPartyRequired, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{ThirdPartyName}", ThirdPartyNameContent, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{ThirdPartyContact}", ThirdPartyContact, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{ThirdPartyWarning}", ThirdPartyWarning, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{ThirdPartyMsg}", ThirdPartyMsg, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{ReguiredInformation}", ReguiredInformation, false, RegexOptions.IgnoreCase);
                        if (isChargeable == 1)
                        {
                            //Our proposal is based on providing a team to perform the test for a three (3) hour period, PER ELEVATOR.
                            //Should this inspection exceed the three(3) hour period per elevator, our standard billing rates will apply.

                            String message = "Our proposal is based on providing a team to perform the test for " + testProposal.DefaultHour.ToString() + " hour period, PER " + ClassificationName + ". Should this inspection exceed the ";
                            message += testProposal.DefaultHour.ToString() + " hour inspection period, PER " + ClassificationName + " our standard billing rates will apply.";


                            String strPrice = "PRICE: ";
                            Double totalAmount = 0;
                            foreach (var item in groupPrice)
                            {
                                if (groupPrice.Count() == 1)
                                {
                                    strPrice = "PRICE: " + new BL_Utility().ConvertToWords(item.Amount.ToString()) + ", " + (item.Amount).ToString("C") + " PER " + ClassificationName + ", for a total of: " + (item.Amount * item.Count).ToString("C");
                                    totalAmount = item.Amount;
                                }
                                else
                                {
                                    strPrice += Environment.NewLine + "-" + new BL_Utility().ConvertToWords(item.Amount.ToString()) + ", " + (item.Amount).ToString("C") + " PER " + ClassificationName + ", for a total of: " + (item.Amount * item.Count).ToString("C");
                                    totalAmount += item.Amount;
                                }

                            }
                            document.ReplaceText("{TotalAmount}", totalAmount.ToString("C"), false, RegexOptions.IgnoreCase);

                            //strPrice += new BL_Utility().ConvertToWords(testProposal.DefaultAmount.ToString()) + ", " + (testProposal.DefaultAmount).ToString("C") + " PER " + ClassificationName + ", for a total of:" + (testProposal.DefaultAmount * lsEquipID.Count()).ToString("C");
                            document.ReplaceText("{chargeableMessage}", message, false, RegexOptions.IgnoreCase);
                            document.ReplaceText("{TestPricing}", strPrice.ToString(), false, RegexOptions.IgnoreCase);


                        }
                        else
                        {
                            //This inspection is covered under the terms of your maintenance service agreement with TEI Group. However,
                            //should this inspection exceed the proposed({ hour }) hour inspection period, PER { Classification}, our standard billing rates will apply.

                            String message = "This inspection is covered under the terms of your maintenance service agreement with TEI Group. However, should this inspection exceed the proposed ";
                            message += testProposal.DefaultHour.ToString() + " hour inspection period, PER " + ClassificationName + " our standard billing rates will apply.";
                            document.ReplaceText("{chargeableMessage}", message, false, RegexOptions.IgnoreCase);
                            document.ReplaceText("{TestPricing}", "0.00", false, RegexOptions.IgnoreCase);
                            document.ReplaceText("{TotalAmount}", "0.00", false, RegexOptions.IgnoreCase);
                        }

                        #region "Save Proposal to DB"
                        ProposalForm objForm = new ProposalForm();
                        objForm.FileName = proposalFileName;
                        objForm.FilePath = pathDoc;
                        objForm.PdfFilePath = pathPDF;
                        objForm.FromDate = Convert.ToDateTime(txtStartDate.Text + " 00:00:00");
                        objForm.ToDate = Convert.ToDateTime(txtEndDate.Text + " 23:59:59");
                        objForm.AddedBy = Session["username"].ToString();
                        objForm.LocID = Convert.ToInt32(dtLoc.Rows[0]["Loc"]);
                        objForm.Classification = ClassificationName;
                        objForm.Type = 1;
                        objForm.Status = "Pending";//Sold, Declined, Pending
                        objForm.AlertEmail = "";
                        objForm.ListEquipment = String.Join(",", lsEquipID);
                        objForm.ConnConfig = Session["config"].ToString();
                        objForm.YearProposal = testProposal.YearProposal;
                        objForm.Chargable = Convert.ToBoolean(isChargeable);
                        objForm.TestTypeID = testProposal.TestTypeID;

                        BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
                        int id = bl_SafetyTest.CreateProposalForm(objForm);
                        // int id = bl_SafetyTest.AddProposalForm(objForm);
                        //if (id != 0)
                        //{
                        //    foreach (ProposalEquipment itemEquip in testProposal.ProposalEquipment)
                        //    {
                        //        ProposalFormDetail detail = new ProposalFormDetail();
                        //        detail.ProposalID = id;
                        //        detail.EquipmentID = itemEquip.ID;
                        //        detail.TestID = itemEquip.TestID;
                        //        detail.Status = "Pending";
                        //        detail.ConnConfig = Session["config"].ToString();
                        //        detail.YearProposal = testProposal.YearProposal;
                        //        detail.Chargable = Convert.ToBoolean(isChargeable);
                        //        bl_SafetyTest = new BL_SafetyTest();
                        //        int detailId = bl_SafetyTest.AddProposalFormDetail(detail);

                        //    }
                        //}

                        //Generate Estimate and Opportunity 
                        #endregion

                        //Replace Estimate No with Proposal ID for now.
                        document.ReplaceText("{ProposalID}", id.ToString());

                        //Generate Estimate/Opportunity
                        document.Save();

                        #region Convert Docx file into PDF
                        //Free version of Spire.Doc has limitations of first three pages more details at https://www.e-iceblue.com/Introduce/free-doc-component.html
                        Spire.Doc.Document doc = new Spire.Doc.Document();
                        doc.LoadFromFile(pathDoc);

                        doc.SaveToFile(pathDoc, Spire.Doc.FileFormat.Docx);
                        doc.SaveToFile(pathPDF, Spire.Doc.FileFormat.PDF);

                        #endregion
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddLoctype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

        }

    }
    public void doGenerateProposalTemp(List<TestProposalDetail> lsTestProposal, TestSetupForm formPro, int isChargeable)
    {
        string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
        string savepath = savepathconfig + "\\" + Session["dbname"] + "\\SafetyTest\\";
        if (!Directory.Exists(savepath))
        {
            Directory.CreateDirectory(savepath);
        }

        string guid = System.Guid.NewGuid().ToString();
        string pathDoc = String.Empty;
        string pathPDF = String.Empty;
        try
        {
            foreach (TestProposalDetail testProposal in lsTestProposal)
            {
                //get company information
                DataTable dtCompany = GetCompanyDetails();
                string CompanyName = dtCompany.Rows[0]["Name"].ToString();
                string CompanyAddress = dtCompany.Rows[0]["Address"].ToString();
                string CompanyPhoneNumber = dtCompany.Rows[0]["Phone"].ToString();
                string CompanyFax = dtCompany.Rows[0]["Fax"].ToString();
                string CompanyEmail = dtCompany.Rows[0]["Email"].ToString();
                string CompanyCity = dtCompany.Rows[0]["City"].ToString();
                string CompanyState = dtCompany.Rows[0]["State"].ToString();
                string CompanyZip = dtCompany.Rows[0]["Zip"].ToString();

                //get location information
                DataTable dtLoc = testProposal.LocationInfo;
                String LocName = dtLoc.Rows[0]["Tag"].ToString();
                String LocAddress = dtLoc.Rows[0]["locAddress"].ToString();

                //get customer information
                DataTable dtCus = testProposal.CustomerInfo;
                String customername = dtCus.Rows[0]["custname"].ToString();
                String customerAddress = dtCus.Rows[0]["custaddress"].ToString();
                String customerCity = dtCus.Rows[0]["custcity"].ToString();
                String customerState = dtCus.Rows[0]["custstate"].ToString();
                String customerZip = dtCus.Rows[0]["custzip"].ToString();

                String mainContact = dtLoc.Rows[0]["Contact"].ToString();
                String LocationTag = dtLoc.Rows[0]["tag"].ToString();
                Boolean ThirdParty = false;
                String ThirdPartyName = "";

                if (testProposal.ProposalEquipment.Count > 0)
                {

                    String ClassificationName = "";
                    String Remark = "";
                    String TestType = "";
                    ClassificationName = testProposal.Classification == null ? "" : testProposal.Classification;
                    Remark = testProposal.Remark == null ? "" : testProposal.Remark;
                    TestType = testProposal.TestType == null ? "" : testProposal.TestType;
                    //CreateFile
                    String proposalFileName = LocName.Replace(" ", "").Replace("\\", "").Replace("/", "").Replace("*", "") + "_" + ClassificationName + DateTime.Now.ToString("MMddyyyyhhmmssfff");
                    proposalFileName = proposalFileName.Replace(" ", "").Replace("\\", "").Replace("/", "").Replace("*", "").Replace(",", "").Replace("?", "").Replace("<", "").Replace(">", "").Replace("|", "").Replace(":", "");
                    pathDoc = savepath + proposalFileName + "." + formPro.MIME;
                    pathPDF = savepath + proposalFileName + "." + "pdf";

                    File.Copy(formPro.FilePath, pathDoc);

                    using (DocX document = DocX.Load(pathDoc))
                    {


                        document.ReplaceText("{LocationTag}", LocationTag, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{MainContract}", mainContact, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{CustomerName}", customername, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{CustomerAddress}", customerAddress, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{CustomerCity}", customerCity, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{CustomerState}", customerState, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{CustomerZip}", customerZip, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{Location}", LocName, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{LocationAddress}", LocAddress.Replace("\n", ""), false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{Year}", testProposal.YearProposal.ToString(), false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{FromDate}", txtStartDate.Text, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{ToDate}", txtEndDate.Text, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{ProposalDate}", DateTime.Now.ToString("MMM dd, yyyy"), false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{LocationCity}", dtLoc.Rows[0]["locCity"].ToString(), false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{LocationState}", dtLoc.Rows[0]["locState"].ToString(), false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{LocationZip}", dtLoc.Rows[0]["locZip"].ToString(), false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{Phone}", dtLoc.Rows[0]["Phone"].ToString(), false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{Mobile}", dtLoc.Rows[0]["Cellular"].ToString(), false, RegexOptions.IgnoreCase);

                        document.ReplaceText("{Classification}", ClassificationName, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{Remark}", Remark, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{TestType}", TestType, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{TestTypeCoverNote}", TestType, false, RegexOptions.IgnoreCase);

                        document.ReplaceText("{DefaultAmount}", testProposal.DefaultAmount.ToString("C"), false, RegexOptions.IgnoreCase);

                        //Company information
                        document.ReplaceText("{CompanyName}", CompanyName, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{CompanyAddress}", CompanyAddress, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{CompanyPhoneNumber}", CompanyPhoneNumber, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{CompanyFax}", CompanyFax, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{CompanyEmail}", CompanyEmail, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{CompanyCity}", CompanyCity, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{CompanyState}", CompanyState, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{CompanyZip}", CompanyZip, false, RegexOptions.IgnoreCase);

                        //Location Billing Information
                        document.ReplaceText("{LocationBillingAddress}", dtLoc.Rows[0]["Address"].ToString(), false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{LocationBillingCity}", dtLoc.Rows[0]["City"].ToString(), false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{LocationBillingState}", dtLoc.Rows[0]["State"].ToString(), false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{LocationBillingZip}", dtLoc.Rows[0]["Zip"].ToString(), false, RegexOptions.IgnoreCase);


                        //if (testProposal.TestTypeCoverName == "" || testProposal.TestTypeCoverName == null)
                        //{
                        //    document.ReplaceText("{TestTypeCoverNote}", TestType, false, RegexOptions.IgnoreCase);
                        //}
                        //else
                        //{
                        //    document.ReplaceText("{TestTypeCoverNote}", testProposal.TestTypeCoverName + " and " + TestType + " inspections", false, RegexOptions.IgnoreCase);

                        //}

                        List<String> lsStrTemplate = document.FindUniqueByPattern(@"(\[\[\[)([^\]]*)(\]\]\])", RegexOptions.IgnoreCase);
                        List<String> lsStrReplace = new List<string>();

                        List<String> lsEquipID = new List<string>();

                        StringBuilder strContent = new StringBuilder();



                        var rowCount = testProposal.ProposalEquipment.Count / 4;
                        Table t = document.AddTable(rowCount + 1, 4);
                        // Specify some properties for this Table.                          
                        t.Alignment = Alignment.left;
                        //t.Paragraphs[0].Font(new FontFamily("Times New Roman"));
                        //t.Paragraphs[0].FontSize(12);
                        t.SetColumnWidth(0, 2400);
                        t.SetColumnWidth(1, 2400);
                        t.SetColumnWidth(2, 2400);
                        t.SetColumnWidth(3, 2400);

                        Border c = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.one, 0, Color.Transparent);

                        int n = 0;
                        int m = 0;
                        List<Double> lsPrice = new List<double>();
                        foreach (ProposalEquipment itemEquip in testProposal.ProposalEquipment)
                        {
                            t.Rows[n].Cells[m].Paragraphs.First().Append(itemEquip.unit.ToString());
                            t.Rows[n].Cells[m].Paragraphs[0].Font(new FontFamily("Times New Roman"));
                            t.Rows[n].Cells[m].Paragraphs[0].FontSize(12);
                            t.Rows[n].Cells[m].Paragraphs[0].SpacingBefore(0);
                            t.Rows[n].Cells[m].Paragraphs[0].SpacingAfter(0);
                            m = m + 1;
                            if (m == 4)
                            {
                                m = 0; n = n + 1;
                            }

                            lsEquipID.Add(itemEquip.ID.ToString());
                            lsPrice.Add(itemEquip.Amount);
                            if (itemEquip.ThirdPartyRequired == true)
                            {
                                ThirdParty = true;
                                ThirdPartyName = itemEquip.ThirdPartyName;

                            }
                        }
                        Border b = new Border(Novacode.BorderStyle.Tcbs_none, BorderSize.one, 0, Color.Transparent);
                        t.SetBorder(TableBorderType.InsideH, b);
                        t.SetBorder(TableBorderType.InsideV, b);
                        t.SetBorder(TableBorderType.Bottom, b);
                        t.SetBorder(TableBorderType.Top, b);
                        t.SetBorder(TableBorderType.Left, b);
                        t.SetBorder(TableBorderType.Right, b);

                        // Insert the Table into the document.
                        try
                        {
                            foreach (var paragraph in document.Paragraphs)
                            {
                                paragraph.FindAll("{UnitNumber}").ForEach(index => paragraph.InsertTableAfterSelf((t)));
                            }
                            document.ReplaceText("{UnitNumber}", "", false, RegexOptions.IgnoreCase);
                        }
                        catch (Exception)
                        {
                            document.InsertTable(t);
                        }

                        var groupPrice = lsPrice
                            .GroupBy(i => i) //Group the words
                            .Select(i => new { Amount = i.Key, Count = i.Count() });
                        String ReguiredInformation = "";
                        String ThridPartyRequired = "";
                        String ThirdPartyContact = "";
                        String ThirdPartyWarning = "";
                        String ThirdPartyNameContent = "";
                        String ThirdPartyMsg = "Kindly sign and return to us immediately so we may schedule your inspection accordingly";
                        if (ThirdParty == true)
                        {
                            ThridPartyRequired = "This inspection requires your hiring a Private Elevator Inspection Agency to witness this inspection.";
                            ThirdPartyNameContent = "Name of 3rd  Party Company:" + ThirdPartyName;
                            ThirdPartyContact = "Contact Person::_______________";
                            ThirdPartyWarning = "**WE CAN NOT ACCEPT PROPOSAL, IF THE THIRD PARTY INFORMATION IS NOT PROVIDED";
                            ThirdPartyMsg = "Kindly sign and provide your Third Party Witnessing Company information below and return to us immediately so we may schedule your inspection accordingly.";
                            ReguiredInformation = "** REQUIRED INFORMATION:";
                            document.ReplaceText("{ThirdpartyName}", ThirdPartyName, false, RegexOptions.IgnoreCase);
                        }
                        document.ReplaceText("{ThridPartyRequired}", ThridPartyRequired, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{ThirdPartyName}", ThirdPartyNameContent, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{ThirdPartyContact}", ThirdPartyContact, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{ThirdPartyWarning}", ThirdPartyWarning, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{ThirdPartyMsg}", ThirdPartyMsg, false, RegexOptions.IgnoreCase);
                        document.ReplaceText("{ReguiredInformation}", ReguiredInformation, false, RegexOptions.IgnoreCase);
                        if (isChargeable == 1)
                        {
                            //Our proposal is based on providing a team to perform the test for a three (3) hour period, PER ELEVATOR.
                            //Should this inspection exceed the three(3) hour period per elevator, our standard billing rates will apply.

                            String message = "Our proposal is based on providing a team to perform the test for " + testProposal.DefaultHour.ToString() + " hour period, PER " + ClassificationName + ". Should this inspection exceed the ";
                            message += testProposal.DefaultHour.ToString() + " hour inspection period, PER " + ClassificationName + " our standard billing rates will apply.";


                            String strPrice = "PRICE: ";
                            Double totalAmount = 0;
                            foreach (var item in groupPrice)
                            {
                                if (groupPrice.Count() == 1)
                                {
                                    strPrice = "PRICE: " + new BL_Utility().ConvertToWords(item.Amount.ToString()) + ", " + (item.Amount).ToString("C") + " PER " + ClassificationName + ", for a total of: " + (item.Amount * item.Count).ToString("C");
                                    totalAmount = item.Amount;
                                }
                                else
                                {
                                    strPrice += Environment.NewLine + "-" + new BL_Utility().ConvertToWords(item.Amount.ToString()) + ", " + (item.Amount).ToString("C") + " PER " + ClassificationName + ", for a total of: " + (item.Amount * item.Count).ToString("C");
                                    totalAmount += item.Amount;
                                }
                            }

                            document.ReplaceText("{TotalAmount}", totalAmount.ToString("C"), false, RegexOptions.IgnoreCase);

                            // strPrice += new BL_Utility().ConvertToWords(testProposal.DefaultAmount.ToString()) + ", " + (testProposal.DefaultAmount).ToString("C") + " PER " + ClassificationName + ", for a total of:" + (testProposal.DefaultAmount * lsEquipID.Count()).ToString("C");
                            document.ReplaceText("{chargeableMessage}", message, false, RegexOptions.IgnoreCase);
                            document.ReplaceText("{TestPricing}", strPrice.ToString(), false, RegexOptions.IgnoreCase);
                        }
                        else
                        {
                            //This inspection is covered under the terms of your maintenance service agreement with TEI Group. However,
                            //should this inspection exceed the proposed({ hour }) hour inspection period, PER { Classification}, our standard billing rates will apply.

                            String message = "This inspection is covered under the terms of your maintenance service agreement with TEI Group. However, should this inspection exceed the proposed ";
                            message += testProposal.DefaultHour.ToString() + " hour inspection period, PER " + ClassificationName + " our standard billing rates will apply.";
                            document.ReplaceText("{chargeableMessage}", message, false, RegexOptions.IgnoreCase);
                            document.ReplaceText("{TestPricing}", "0.00", false, RegexOptions.IgnoreCase);
                            document.ReplaceText("{TotalAmount}", "0.00", false, RegexOptions.IgnoreCase);
                        }

                        #region "Save Proposal to DB"
                        ProposalForm objForm = new ProposalForm();
                        objForm.FileName = proposalFileName;
                        objForm.FilePath = pathDoc;
                        objForm.PdfFilePath = pathPDF;
                        objForm.FromDate = Convert.ToDateTime(txtStartDate.Text + " 00:00:00");
                        objForm.ToDate = Convert.ToDateTime(txtEndDate.Text + " 23:59:59");
                        objForm.AddedBy = Session["username"].ToString();
                        objForm.LocID = Convert.ToInt32(dtLoc.Rows[0]["Loc"]);
                        objForm.Classification = ClassificationName;
                        objForm.Type = 1;
                        objForm.Status = "Pending";//Sold, Declined, Pending
                        objForm.AlertEmail = "";
                        objForm.ListEquipment = String.Join(",", lsEquipID);
                        objForm.ConnConfig = Session["config"].ToString();
                        objForm.YearProposal = testProposal.YearProposal;
                        objForm.Chargable = Convert.ToBoolean(isChargeable);
                        objForm.TestTypeID = testProposal.TestTypeID;

                        BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
                        int id = bl_SafetyTest.CreateProposalForm(objForm);
                        // int id = bl_SafetyTest.AddProposalForm(objForm);
                        //if (id != 0)
                        //{
                        //    foreach (ProposalEquipment itemEquip in testProposal.ProposalEquipment)
                        //    {
                        //        ProposalFormDetail detail = new ProposalFormDetail();
                        //        detail.ProposalID = id;
                        //        detail.EquipmentID = itemEquip.ID;
                        //        detail.TestID = itemEquip.TestID;
                        //        detail.Status = "Pending";
                        //        detail.ConnConfig = Session["config"].ToString();
                        //        detail.YearProposal = testProposal.YearProposal;
                        //        detail.Chargable = Convert.ToBoolean(isChargeable);
                        //        bl_SafetyTest = new BL_SafetyTest();
                        //        int detailId = bl_SafetyTest.AddProposalFormDetail(detail);

                        //    }
                        //}

                        //Generate Estimate and Opportunity 
                        #endregion

                        //Replace Estimate No with Proposal ID for now.
                        document.ReplaceText("{ProposalID}", id.ToString());

                        //Generate Estimate/Opportunity
                        document.Save();

                        #region Convert Docx file into PDF
                        //Free version of Spire.Doc has limitations of first three pages more details at https://www.e-iceblue.com/Introduce/free-doc-component.html
                        Spire.Doc.Document doc = new Spire.Doc.Document();
                        doc.LoadFromFile(pathDoc);

                        doc.SaveToFile(pathDoc, Spire.Doc.FileFormat.Docx);
                        doc.SaveToFile(pathPDF, Spire.Doc.FileFormat.PDF);

                        #endregion
                    }

                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddLoctype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }

    }

    private List<TestProposalDetail> getDataChargeableToGeneralProposal(DataTable dtResult, int chargeable)
    {

        int startYear = Convert.ToDateTime(txtStartDate.Text.ToString()).Year;
        int endYear = Convert.ToDateTime(txtEndDate.Text.ToString()).Year;
        List<int> lsYear = new List<int>();
        if (startYear <= endYear)
        {
            for (int i = startYear; i <= endYear; i++)
            {
                lsYear.Add(i);
            }
        }


        List<TestProposalDetail> lsTestProposal = new List<TestProposalDetail>();
        DataTable dtPricing = new DataTable();


        foreach (int yearNumber in lsYear)
        {
            DataTable dtGroup = new DataTable();
            dtGroup = GetDataGroup(dtResult, yearNumber, chargeable);
            if (dtGroup.Rows.Count > 0)
            {
                BL_User objBL_User = new BL_User();
                User objPropUser = new User();
                objPropUser.DBName = Session["dbname"].ToString();
                objPropUser.ConnConfig = Session["config"].ToString();


                foreach (DataRow row in dtGroup.Rows)
                {

                    TestProposalDetail objTestProposal = new TestProposalDetail();
                    List<ProposalEquipment> lsProposalEquipment = new List<ProposalEquipment>();

                    objTestProposal.YearProposal = yearNumber;
                    objTestProposal.Remark = "";
                    objTestProposal.DefaultHour = 0;
                    objTestProposal.DefaultAmount = 0;
                    objTestProposal.TestType = "";
                    objTestProposal.TestTypeCoverName = "";
                    objTestProposal.CoveredByTestTypeName = "";
                    objTestProposal.TestTypeID = Convert.ToInt32(row["LTID"]);

                    objTestProposal.Classification = row["Classification"].ToString();
                    String equipClassification = row["Classification"].ToString();


                    // Get Equipment infor
                    ProposalEquipment proEquimentClassification = new ProposalEquipment();
                    DataSet dsEquipment = objBL_User.GetAllTestInLocationByChargableAndClassification(Session["config"].ToString(), Convert.ToInt32(row["Loc"]), Convert.ToInt32(row["LTID"]), yearNumber, Convert.ToBoolean(chargeable), equipClassification);


                    DataTable dtEquipment = new DataTable();
                    if (dsEquipment.Tables.Count > 0)
                    {
                        dtEquipment = dsEquipment.Tables[0];
                        objPropUser.LocID = Convert.ToInt32(row["Loc"]);


                        objTestProposal.LocationInfo = dsEquipment.Tables[1];
                        objTestProposal.CustomerInfo = dsEquipment.Tables[3];

                        objTestProposal.Remark = dsEquipment.Tables[2].Rows[0]["Remarks"].ToString();
                        objTestProposal.DefaultHour = Double.Parse(dsEquipment.Tables[2].Rows[0]["DefaultHour"].ToString());
                        objTestProposal.DefaultAmount = Double.Parse(dsEquipment.Tables[2].Rows[0]["Amount"].ToString());
                        objTestProposal.TestType = dsEquipment.Tables[2].Rows[0]["TestTypeName"].ToString();
                        objTestProposal.TestTypeCoverName = dsEquipment.Tables[2].Rows[0]["TestTypeCoverName"].ToString();
                        objTestProposal.CoveredByTestTypeName = dsEquipment.Tables[2].Rows[0]["CoveredByTestTypeName"].ToString();


                        foreach (DataRow rowEqui in dtEquipment.Rows)
                        {
                            proEquimentClassification = new ProposalEquipment();
                            proEquimentClassification.TestID = Convert.ToInt32(rowEqui["LID"].ToString());
                            proEquimentClassification.ID = Convert.ToInt32(rowEqui["Elev"].ToString());
                            proEquimentClassification.unit = rowEqui["Unit"].ToString();
                            proEquimentClassification.Classification = rowEqui["Classification"].ToString();
                            proEquimentClassification.Amount = Convert.ToDouble(rowEqui["Amount"].ToString());
                            proEquimentClassification.OverrideAmount = Convert.ToDouble(rowEqui["OverrideAmount"].ToString());
                            if (Convert.ToDouble(rowEqui["OverrideAmount"]) != 0)
                            {
                                proEquimentClassification.Amount = Convert.ToDouble(rowEqui["OverrideAmount"]);
                            }
                            proEquimentClassification.Chargeable = Convert.ToBoolean(rowEqui["Chargeable"]);
                            proEquimentClassification.ThirdPartyName = rowEqui["ThirdPartyName"].ToString();
                            proEquimentClassification.ThirdPartyPhone = rowEqui["ThirdPartyPhone"].ToString();
                            proEquimentClassification.ThirdPartyRequired = Convert.ToBoolean(rowEqui["ThirdPartyRequired"]);
                            lsProposalEquipment.Add(proEquimentClassification);
                        }
                        objTestProposal.ProposalEquipment = lsProposalEquipment;


                        lsTestProposal.Add(objTestProposal);

                    }

                }
            }
        }


        return lsTestProposal;


    }

    private List<TestProposalDetail> getDataTestCoverToGeneralProposal(DataTable dtResult, int chargeable, int hasChild)
    {

        int startYear = Convert.ToDateTime(txtStartDate.Text.ToString()).Year;
        int endYear = Convert.ToDateTime(txtEndDate.Text.ToString()).Year;
        List<int> lsYear = new List<int>();
        if (startYear <= endYear)
        {
            for (int i = startYear; i <= endYear; i++)
            {
                lsYear.Add(i);
            }
        }


        List<TestProposalDetail> lsTestProposal = new List<TestProposalDetail>();
        DataTable dtPricing = new DataTable();


        foreach (int yearNumber in lsYear)
        {
            DataTable dtGroup = new DataTable();
            dtGroup = GetAllEquipmentHasTestCover(dtResult, yearNumber, chargeable, hasChild);
            if (dtGroup.Rows.Count > 0)
            {
                BL_User objBL_User = new BL_User();
                User objPropUser = new User();
                objPropUser.DBName = Session["dbname"].ToString();
                objPropUser.ConnConfig = Session["config"].ToString();


                foreach (DataRow row in dtGroup.Rows)
                {

                    String equipClassification = row["Classification"].ToString();

                    // Get Equipment infor
                    ProposalEquipment proEquimentClassification = new ProposalEquipment();
                    DataSet dsEquipment = objBL_User.GetAllTestCoverInLocationWithClassification(Session["config"].ToString(), Convert.ToInt32(row["Loc"]), Convert.ToInt32(row["LTID"]), yearNumber, Convert.ToBoolean(chargeable), equipClassification);


                    DataTable dtEquipment = new DataTable();
                    if (dsEquipment.Tables.Count > 0)
                    {
                        TestProposalDetail objTestProposal = new TestProposalDetail();
                        List<ProposalEquipment> lsProposalEquipment = new List<ProposalEquipment>();

                        dtEquipment = dsEquipment.Tables[0];

                        objTestProposal.YearProposal = yearNumber;
                        objTestProposal.Remark = "";
                        objTestProposal.DefaultHour = 0;
                        objTestProposal.DefaultAmount = 0;
                        objTestProposal.TestType = "";
                        objTestProposal.TestTypeCoverName = "";
                        objTestProposal.CoveredByTestTypeName = "";
                        objTestProposal.TestTypeID = Convert.ToInt32(row["LTID"]);

                        objTestProposal.Classification = row["Classification"].ToString();


                        objPropUser.LocID = Convert.ToInt32(row["Loc"]);

                        objTestProposal.LocationInfo = dsEquipment.Tables[1];

                        //Customer Info
                        objTestProposal.CustomerInfo = dsEquipment.Tables[3];

                        //Price

                        objTestProposal.Remark = dsEquipment.Tables[2].Rows[0]["Remarks"].ToString();
                        objTestProposal.DefaultHour = Double.Parse(dsEquipment.Tables[2].Rows[0]["DefaultHour"].ToString());
                        objTestProposal.DefaultAmount = Double.Parse(dsEquipment.Tables[2].Rows[0]["Amount"].ToString());
                        objTestProposal.TestType = dsEquipment.Tables[2].Rows[0]["TestTypeName"].ToString();
                        objTestProposal.TestTypeCoverName = dsEquipment.Tables[2].Rows[0]["TestTypeCoverName"].ToString();
                        objTestProposal.CoveredByTestTypeName = dsEquipment.Tables[2].Rows[0]["CoveredByTestTypeName"].ToString();

                        foreach (DataRow rowEqui in dtEquipment.Rows)
                        {
                            proEquimentClassification = new ProposalEquipment();
                            proEquimentClassification.TestID = Convert.ToInt32(rowEqui["LID"].ToString());
                            proEquimentClassification.ID = Convert.ToInt32(rowEqui["Elev"].ToString());
                            proEquimentClassification.unit = rowEqui["Unit"].ToString();
                            proEquimentClassification.Classification = rowEqui["Classification"].ToString();
                            proEquimentClassification.Amount = Convert.ToDouble(rowEqui["Amount"]);
                            if (Convert.ToDouble(rowEqui["OverrideAmount"]) != 0)
                            {
                                proEquimentClassification.Amount = Convert.ToDouble(rowEqui["OverrideAmount"]);
                            }
                            proEquimentClassification.OverrideAmount = Convert.ToDouble(rowEqui["OverrideAmount"]);
                            proEquimentClassification.Chargeable = Convert.ToBoolean(rowEqui["Chargeable"]);
                            proEquimentClassification.ThirdPartyName = rowEqui["ThirdPartyName"].ToString();
                            proEquimentClassification.ThirdPartyPhone = rowEqui["ThirdPartyPhone"].ToString();
                            proEquimentClassification.ThirdPartyRequired = Convert.ToBoolean(rowEqui["ThirdPartyRequired"]);
                            lsProposalEquipment.Add(proEquimentClassification);
                        }

                        objTestProposal.ProposalEquipment = lsProposalEquipment;


                        lsTestProposal.Add(objTestProposal);
                    }


                }
            }
        }
        return lsTestProposal;
    }
    private DataTable GetDataGroup(DataTable dt, int yearNumber, int ischargeable)
    {
        DataTable dtGroup = new DataTable();
        dtGroup.Columns.Add("Loc", typeof(int));
        dtGroup.Columns.Add("Address", typeof(String));
        dtGroup.Columns.Add("YearNumber", typeof(int));
        dtGroup.Columns.Add("LTID", typeof(int));
        dtGroup.Columns.Add("Classification", typeof(String));

        var query = (from order in dt.AsEnumerable()
                     where order.Field<int>("TestYear") == yearNumber
                     where order.Field<String>("ProposalStatus") == ""
                     where order.Field<int>("Chargeable") == ischargeable
                     where order.Field<String>("Classification") != ""
                     group order by new
                     {
                         Loc = order.Field<int>("Loc")
                     ,
                         Address = order.Field<String>("Address")
                     ,
                         YearNumber = order.Field<String>("Next").Substring(order.Field<String>("Next").LastIndexOf('/') + 1)
                     ,
                         LTID = order.Field<int>("LTID")
                      ,
                         Classification = order.Field<String>("Classification")
                     } into grp
                     orderby grp.Key.Loc
                     select new
                     {
                         Loc = grp.Key.Loc,
                         Address = grp.Key.Address,
                         YearNumber = grp.Key.YearNumber,
                         LTID = grp.Key.LTID,
                         Classification = grp.Key.Classification
                     }).ToList();

        foreach (var row in query)
        {
            dtGroup.Rows.Add(row.Loc, row.Address, yearNumber, row.LTID, row.Classification);
        }
        return dtGroup.DefaultView.ToTable(true, new string[] { "Loc", "Address", "YearNumber", "LTID", "Classification" });
    }
    private DataTable GetAllEquipmentHasTestCover(DataTable dt, int yearNumber, int ischargeable, int hasChild)
    {
        DataTable dtGroup = new DataTable();
        // dtGroup.Columns.Add("Elev", typeof(int));
        dtGroup.Columns.Add("Loc", typeof(int));
        //dtGroup.Columns.Add("Address", typeof(String));
        // dtGroup.Columns.Add("YearNumber", typeof(int));
        dtGroup.Columns.Add("LTID", typeof(int));
        dtGroup.Columns.Add("Classification", typeof(String));

        var query = (from order in dt.AsEnumerable()
                     where order.Field<int>("TestYear") == yearNumber
                     where order.Field<String>("ProposalStatus") == ""
                     where order.Field<int>("Chargeable") == ischargeable
                     where order.Field<String>("Classification") != ""
                     where order.Field<int>("IsCoverTestType") == 1
                     where order.Field<int>("HasChild") == hasChild
                     group order by new
                     {
                         // Elev = order.Field<int>("NID")
                         //,
                         Loc = order.Field<int>("Loc")

                         ,
                         LTID = order.Field<int>("LTID")
                          ,
                         Classification = order.Field<String>("Classification")
                     } into grp
                     orderby grp.Key.Loc
                     select new
                     {
                         //    Elev = grp.Key.Elev,
                         Loc = grp.Key.Loc,

                         LTID = grp.Key.LTID,
                         Classification = grp.Key.Classification
                     }).ToList();

        foreach (var row in query)
        {
            //dtGroup.Rows.Add(row.Elev,row.Loc,row.LTID,row.Classification);
            dtGroup.Rows.Add(row.Loc, row.LTID, row.Classification);
        }
        return dtGroup.DefaultView.ToTable(true, new string[] { "Loc", "LTID", "Classification" });
    }

    private DataTable GetAllProposal(DataTable dt)
    {
        DataTable dtGroup = new DataTable();
        dtGroup.Columns.Add("LocID", typeof(int));
        dtGroup.Columns.Add("ProposalID", typeof(int));
        dtGroup.Columns.Add("PDFFilePath", typeof(string));
        dtGroup.Columns.Add("Tag", typeof(string));

        var query = (from order in dt.AsEnumerable()
                     where order.Field<String>("ProposalStatus") != ""
                     group order by new
                     {

                         LocID = order.Field<int>("Loc")
                         ,
                         ProposalID = order.Field<int>("ProposalID")
                          ,
                         PDFFilePath = order.Field<string>("PDFFilePath")
                           ,
                         Tag = order.Field<string>("Tag"),
                     } into grp
                     orderby grp.Key.LocID
                     select new
                     {
                         LocID = grp.Key.LocID,
                         ProposalID = grp.Key.ProposalID,
                         PDFFilePath = grp.Key.PDFFilePath,

                         Tag = grp.Key.Tag
                     }).ToList();

        foreach (var row in query)
        {
            dtGroup.Rows.Add(row.LocID, row.ProposalID, row.PDFFilePath, row.Tag);
        }
        return dtGroup.DefaultView.ToTable(true, new string[] { "LocID", "ProposalID", "PDFFilePath", "Tag" });
    }
    #endregion


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
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileerrorWarning", "alert('" + str + "');", true);
        }
    }

    protected void btnProcessDownload_Click(object sender, EventArgs e)
    {
        if (hdnDownloadID.Value.Trim() != "")
        {
            BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
            DataSet ds = bl_SafetyTest.GetProposalFormByID(Session["config"].ToString(), Convert.ToInt32(hdnDownloadID.Value));
            String type = ".docx";
            String path = ds.Tables[0].Rows[0]["FilePath"].ToString();
            if (ds.Tables.Count > 0)
            {
                if (hdnDownloadType.Value == "1")
                {
                    type = ".pdf";
                    path = ds.Tables[0].Rows[0]["PdfFilePath"].ToString();
                }
                DownloadDocument(path, ds.Tables[0].Rows[0]["FileName"].ToString() + type);
            }
        }
    }


    #region "Send Proposal"
    private void CheckValidEmails(List<string> emails, StringBuilder sbdInValidEmails)
    {
        if (sbdInValidEmails == null) sbdInValidEmails = new StringBuilder();

        foreach (var item in emails.ToList())
        {
            if (!WebBaseUtility.IsValidEmailAddress1(item))
            {
                emails.Remove(item);
                sbdInValidEmails.AppendFormat("{0}</br>", item);
            }
        }
    }
    private void processSendProposal()
    {
        DataTable dtResult = processDataFilter((DataTable)Session["safetytest"]);

        if (Session["safetytest"] != null)
        {
            try
            {
                DataTable dt = processDataFilter((DataTable)Session["safetytest"]);
                // DataTable dt = (DataTable)ViewState["SafetyResultForEmail"];
                DataTable temp = dt.DefaultView.ToTable(true, "ProposalID");
                int countNoEmail = 0;
                int countEmail = 0;
                int countProposal = 0;
                foreach (DataRow row in temp.Rows)
                {
                    if (row["ProposalID"].ToString() != "")
                    {
                        countProposal += 1;
                        //get Proposal detail
                        BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
                        DataSet ds = bl_SafetyTest.GetProposalFormByID(Session["config"].ToString(), Convert.ToInt32(row["ProposalID"].ToString()));
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string _fromEmail = "";
                                _fromEmail = WebBaseUtility.GetFromEmailAddress();
                                BL_EmailLog bL_EmailLog = new BL_EmailLog();
                                EmailLog emailLog = new EmailLog();

                                int locId = Convert.ToInt32(ds.Tables[0].Rows[0]["LocID"].ToString());
                                //get Get all Contact
                                DataSet dsContact = getContactData(locId);
                                List<String> lsEmail = getListEmail(dsContact);


                                Mail mail = new Mail();
                                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                                mail.From = _fromEmail;
                                mail.To = lsEmail;
                                // mail.IsBodyHtml = true;


                                if (lsEmail.Count() == 0)
                                {
                                    emailLog.ConnConfig = Session["config"].ToString();
                                    emailLog.Function = "Email All";
                                    emailLog.Screen = "SafetyTest";
                                    emailLog.Username = Session["Username"].ToString();
                                    emailLog.SessionNo = Guid.NewGuid().ToString();
                                    emailLog.Ref = Convert.ToInt32(row["ProposalID"].ToString());
                                    emailLog.Status = 0;
                                    emailLog.From = _fromEmail;
                                    emailLog.To = String.Join(", ", lsEmail);
                                    emailLog.Sender = mail.Username;
                                    emailLog.UsrErrMessage = "Email address does not exist in location " + ds.Tables[0].Rows[0]["Tag"].ToString();
                                    bL_EmailLog.AddEmailLog(emailLog);

                                    countNoEmail += 1;

                                }
                                else
                                {
                                    StringBuilder sbdInValidEmails = new StringBuilder();
                                    CheckValidEmails(mail.To, sbdInValidEmails);
                                    if (mail.To.Count == 0)
                                    {
                                        //string error= "Email address does not exist in location " + ds.Tables[0].Rows[0]["Tag"].ToString();
                                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyNoEmail", "noty({text: '"+ error + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue:true});", true);
                                        if (sbdInValidEmails.Length > 0)
                                        {

                                            sbdInValidEmails.Insert(0, "Invalid emails address:</br>");
                                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnInValidEmails", "noty({text: '" + sbdInValidEmails.ToString() + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default', closable: false, dismissQueue: true});", true);

                                            emailLog.ConnConfig = Session["config"].ToString();
                                            emailLog.Function = "Email All";
                                            emailLog.Screen = "SafetyTest";
                                            emailLog.Username = Session["Username"].ToString();
                                            emailLog.SessionNo = Guid.NewGuid().ToString();
                                            emailLog.Ref = Convert.ToInt32(row["ProposalID"].ToString());
                                            emailLog.Status = 0;
                                            emailLog.From = _fromEmail;
                                            emailLog.To = String.Join(", ", lsEmail);
                                            emailLog.Sender = mail.Username;
                                            emailLog.UsrErrMessage = ds.Tables[0].Rows[0]["Tag"].ToString() + " has invalid email address";
                                            bL_EmailLog.AddEmailLog(emailLog);


                                        }

                                    }
                                    else
                                    {
                                        mail.Title = getLocationName(dsContact) + "- Test Proposal";
                                        //mail.Text = SetMailBody();
                                        var mailContent = "Please review the attached file." + Environment.NewLine + Environment.NewLine;
                                        var companySignature = WebBaseUtility.GetCompanySignature();
                                        mail.IsIncludeSignature = true;
                                        mail.Text = mailContent;
                                        mail.AttachmentFiles.Add(ds.Tables[0].Rows[0]["PDFFilePath"].ToString());


                                        mail.Send(companySignature);

                                        countEmail += 1;

                                        emailLog.ConnConfig = Session["config"].ToString();
                                        emailLog.Function = "Email All";
                                        emailLog.Screen = "SafetyTest";
                                        emailLog.Username = Session["Username"].ToString();
                                        emailLog.SessionNo = Guid.NewGuid().ToString();
                                        emailLog.Ref = Convert.ToInt32(row["ProposalID"].ToString());
                                        emailLog.Status = 1;
                                        emailLog.From = _fromEmail;
                                        emailLog.To = String.Join(", ", mail.To.ToArray());
                                        emailLog.Sender = mail.Username;

                                        bL_EmailLog.AddEmailLog(emailLog);

                                        //Update
                                        BL_SafetyTest proposalTbl = new BL_SafetyTest();
                                        proposalTbl.UpdateSenderInfoProposalForm(Session["config"].ToString(), Convert.ToInt32(row["ProposalID"].ToString()), String.Join(",", lsEmail.ToArray()), mail.From);
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySafetyTest", "noty({text: 'Email send successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                                    }

                                }

                            }
                        }
                    }

                }

                if (countProposal == 0)
                {
                    string error = "There were no emails sent out";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyNoEmail", "noty({text: '" + error + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue:true});", true);
                }
                else
                {
                    if (countNoEmail == countProposal)
                    {
                        string error = "There were no emails sent out";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyNoEmail", "noty({text: '" + error + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue:true});", true);
                    }
                    else
                    {
                        if (countEmail < countProposal)
                        {
                            string error = "Total " + countEmail + " failed of " + countProposal + " test proposal could not be sent";
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyNoEmail", "noty({text: '" + error + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue:true});", true);
                        }
                    }
                }


            }
            catch (Exception ex)
            {

                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "processSendProposalError", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }


        }

    }

    private void processSendProposalAll()
    {
        DataTable dtResult = processDataFilter((DataTable)Session["safetytest"]);

        if (Session["safetytest"] != null)
        {
            try
            {
                User objPropUser = new User();
                string _fromEmail = "";
                _fromEmail = WebBaseUtility.GetFromEmailAddress();
                if (string.IsNullOrEmpty(_fromEmail))
                {
                    _fromEmail = WebBaseUtility.GetFromEmailAddress();
                }

                DataTable dt = processDataFilter((DataTable)Session["safetytest"]);
                int totalTicket = 0;
                int totalSentEmails = 0;
                int totalSendErr = 0;
                DataTable temp = GetAllProposal(dt);
                totalTicket = temp.Rows.Count;
                if (totalTicket > 0)
                {

                    try
                    {
                        List<MimeKit.MimeMessage> mimeSentMessages = new List<MimeKit.MimeMessage>();
                        List<MimeKit.MimeMessage> mimeErrorMessages = new List<MimeKit.MimeMessage>();
                        Tuple<int, string, string> emailSendError = null;
                        Tuple<int, string, string> emailGetSentError = null;
                        StringBuilder sbdSentError = new StringBuilder();
                        StringBuilder sbdGetSentError = new StringBuilder();

                        EmailLog emailLog = new EmailLog();
                        emailLog.ConnConfig = Session["config"].ToString();
                        emailLog.Function = "Email All";
                        emailLog.Screen = "SafetyTest";
                        emailLog.Username = Session["Username"].ToString();
                        emailLog.SessionNo = Guid.NewGuid().ToString();

                        foreach (DataRow _dr in temp.Rows)
                        {
                            emailLog.Ref = Convert.ToInt32(_dr["ProposalID"].ToString());

                            int locId = Convert.ToInt32(_dr["LocID"].ToString());
                            //get Get all Contact
                            DataSet dsContact = getContactData(locId);
                            List<String> lsEmail = getListEmail(dsContact);

                            if (lsEmail.Count > 0)
                            {
                                Mail mail = new Mail();
                                mail.From = _fromEmail;
                                // Boolean IsMailSend = false;
                                mail.To = lsEmail;

                                mail.Title = getLocationName(dsContact) + "- Test Proposal";
                                //mail.Text = SetMailBody();
                                var mailContent = "Please review the attached file." + Environment.NewLine + Environment.NewLine;
                                var companySignature = WebBaseUtility.GetCompanySignature();
                                mail.IsIncludeSignature = true;
                                mail.Text = mailContent;

                                mail.AttachmentFiles.Add(_dr["PDFFilePath"].ToString());


                                //if (IsMailSend == true)
                                //{

                                MimeKit.MimeMessage mimeMessage = new MimeKit.MimeMessage();
                                emailSendError = mail.CompletingMessage(ref mimeMessage, true, emailLog, companySignature);
                                if (emailSendError != null && emailSendError.Item1 == 1)
                                {
                                    sbdSentError.Append(emailSendError.Item2);
                                    break;
                                }
                                else
                                {
                                    emailSendError = mail.Send(mimeMessage, true, emailLog);
                                    if (emailSendError != null)
                                    {
                                        if (emailSendError.Item1 == 1)
                                        {
                                            sbdSentError.Append(emailSendError.Item2);
                                            break;
                                        }
                                        else
                                        {
                                            sbdSentError.Append(emailSendError.Item2);
                                            mimeErrorMessages.Add(mimeMessage);
                                            totalSendErr++;
                                        }
                                    }
                                    else
                                    {
                                        mimeSentMessages.Add(mimeMessage);
                                        //Update
                                        BL_SafetyTest proposalTbl = new BL_SafetyTest();
                                        proposalTbl.UpdateSenderInfoProposalForm(Session["config"].ToString(), Convert.ToInt32(_dr["ProposalID"].ToString()), String.Join(",", lsEmail.ToArray()), mail.From);

                                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySafetyTest", "noty({text: 'Email send successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                                    }
                                }

                                // }
                            }
                            else
                            {
                                totalSendErr++;
                                emailLog.To = string.Empty;
                                emailLog.Status = 0;
                                emailLog.UsrErrMessage = "Email address does not exist in location " + _dr["Tag"].ToString();
                                BL_EmailLog bL_EmailLog = new BL_EmailLog();
                                bL_EmailLog.AddEmailLog(emailLog);
                            }


                        }
                        totalSentEmails = mimeSentMessages.Count;
                        if (totalSentEmails > 0)
                        {
                            Mail mail = new Mail();
                            WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                            if (mail.TakeASentEmailCopy)
                            {
                                // reset ref 
                                emailLog.Ref = 0;
                                var take = 100;
                                try
                                {
                                    var strTakeSent = ConfigurationManager.AppSettings["TakeSentItemspertime"].ToString();
                                    take = Convert.ToInt32(strTakeSent);
                                }
                                catch (Exception)
                                {
                                }

                                if (totalSentEmails >= take)
                                {
                                    List<List<MimeKit.MimeMessage>> lstTenMessages = new List<List<MimeKit.MimeMessage>>();
                                    while (mimeSentMessages.Any())
                                    {
                                        lstTenMessages.Add(mimeSentMessages.Take(take).ToList());
                                        mimeSentMessages = mimeSentMessages.Skip(take).ToList();
                                    }

                                    int imapLoginTimes = 0;
                                    foreach (var lst in lstTenMessages)
                                    {
                                        imapLoginTimes++;
                                        if (imapLoginTimes >= 15)
                                        {
                                            imapLoginTimes = 0;
                                            Thread.Sleep(10000);
                                            mail = new Mail();
                                            WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                                        }
                                        emailGetSentError = mail.GetSentItems(lst, true, emailLog);
                                    }
                                }
                                else
                                {
                                    emailGetSentError = mail.GetSentItems(mimeSentMessages, true, emailLog);
                                }
                            }
                        }

                        if (sbdSentError.Length > 0)
                        {
                            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(sbdSentError.ToString());
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                            if (emailGetSentError != null)
                            {
                                string str1 = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str1 + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                            }
                        }
                        else
                        {
                            if (totalSentEmails > 0)
                            {
                                var successfullMess = "There were " + totalSentEmails + " of "
                                    + totalTicket + " proposals sent out successfully.";

                                if (totalSendErr > 0)
                                {
                                    successfullMess += "<br>Total " + totalSendErr + " failed of "
                                        + totalTicket + " proposals could not be sent.";
                                }



                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: '" + successfullMess + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

                                if (emailGetSentError != null)
                                {
                                    string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                }
                            }
                            else
                            {
                                string str = "There were no emails sent out.";

                                if (totalSendErr > 0)
                                {
                                    str += "<br>Total " + totalSendErr + " failed of "
                                        + totalTicket + " proposals could not be sent.";
                                }

                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                else
                {
                    string error = "There were no emails sent out";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyNoEmail", "noty({text: '" + error + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue:true});", true);
                }





            }
            catch (Exception ex)
            {

                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "processSendProposalError", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }


        }

    }
    private DataSet getContactData(int id)
    {
        BL_User objBL_User = new BL_User();
        User objPropUser = new User();
        DataSet ds = new DataSet();

        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.LocID = id;

        ds = objBL_User.getLocationByID(objPropUser);



        return ds;
    }
    private string getLocationName(DataSet dsContact)
    {
        String locName = String.Empty;
        if (dsContact.Tables.Count > 0)
        {
            if (dsContact.Tables[0].Rows.Count > 0)
            {
                locName = dsContact.Tables[0].Rows[0]["Tag"].ToString();
            }


        }
        return locName;
    }
    private List<String> getListEmail(DataSet dsContact)
    {

        List<String> lsEmail = new List<string>();
        if (dsContact.Tables.Count > 0)
        {
            if (dsContact.Tables[1].Select("EmailRecTestProp =true").Count() > 0)
            {
                DataRow[] result = dsContact.Tables[1].Select("EmailRecTestProp =true");

                foreach (DataRow row in result)
                {

                    if (!lsEmail.Contains(row["Email"].ToString()))
                    {
                        lsEmail.Add(row["Email"].ToString());

                    }
                }
            }

        }

        return lsEmail;
    }
    #endregion

    protected void lnkEmail_Click(object sender, EventArgs e)
    {
        //processSendProposal();
        processSendProposalAll();
        RadGrid_SafetyTest.Rebind();
        RadGrid_gvLogs.Rebind();
    }

    private void CreateTestCustomTable()
    {

        DataSet dst = new DataSet();
        BL_SafetyTest _objbltesttypes = new BL_SafetyTest();
        dst = _objbltesttypes.GetAllTestCustom(Session["config"].ToString(), Session["dbname"].ToString());

        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("OrderNo", typeof(int));
        dt.Columns.Add("Label", typeof(string));
        dt.Columns.Add("IsAlert", typeof(Boolean));
        dt.Columns.Add("TeamMember", typeof(string));
        dt.Columns.Add("Format", typeof(int));
        dt.Columns.Add("TeamMemberDisplay", typeof(string));
        dt.Columns.Add("UserRoles", typeof(string));
        dt.Columns.Add("UserRolesDisplay", typeof(string));
        DataRow dr = dt.NewRow();

        dr["ID"] = 0;
        dr["Line"] = dt.Rows.Count + 1;
        dr["OrderNo"] = 0;
        dr["Label"] = "";
        dr["IsAlert"] = 0;
        dr["TeamMember"] = "";
        dr["Format"] = 0;
        dr["TeamMemberDisplay"] = "";
        dr["UserRoles"] = "";
        dr["UserRolesDisplay"] = "";
        dt.Rows.Add(dr);
        if (dst.Tables[0].Rows.Count == 0)
        {
            ViewState["TestCustomTable"] = dt;
            ViewState["TestCustomDeleteTable"] = dt;
        }
        else
        {
            ViewState["TestCustomTable"] = dst.Tables[0];
        }

        ViewState["TestCustomValues"] = dst.Tables[1];
    }

    protected void RadGrid_SafetyTest_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
    {
        Boolean flag = false;
        DataTable dt = new DataTable();
        dt.Columns.Add("TestID", typeof(int));
        dt.Columns.Add("EquipmentID", typeof(int));
        dt.Columns.Add("TestCustomFieldID", typeof(int));
        dt.Columns.Add("Value", typeof(String));
        dt.Columns.Add("OldValue", typeof(String));
        try
        {
            foreach (GridBatchEditingCommand command in e.Commands)
            {
                if ((command.Type == GridBatchEditingCommandType.Update))
                {
                    GridDataItem item = (GridDataItem)command.Item;
                    HiddenField hdnid = (HiddenField)item.FindControl("hdnid");
                    HiddenField hduid = (HiddenField)item.FindControl("hduid");

                    Hashtable newValues = command.NewValues;
                    Hashtable oldValues = command.OldValues;

                    if (!newValues.Equals(oldValues))
                    {
                        foreach (var key in newValues.Keys)
                        {
                            if (newValues[key] != null)
                            {
                                if (oldValues[key] != null)
                                {
                                    if (!newValues[key].ToString().Equals(oldValues[key].ToString()))
                                    {
                                        flag = true;
                                    }
                                }
                                else
                                {
                                    flag = true;
                                }

                            }

                            if (flag == true)
                            {
                                String oldData = "";
                                if (oldValues[key] != null)
                                {
                                    oldData = oldValues[key].ToString();
                                }
                                int fieldID = Convert.ToInt32(key.ToString().Substring(key.ToString().IndexOf("_") + 1));
                                dt.Rows.Add(Convert.ToInt32(hdnid.Value), Convert.ToInt32(hduid.Value), fieldID, newValues[key].ToString(), oldData);
                                flag = false;
                            }
                        }
                    }
                }
            }
            if (dt.Rows.Count > 0)
            {
                BL_SafetyTest objtestbl = new BL_SafetyTest();
                SafetyTest objproptest = new SafetyTest();
                objproptest.ConnConfig = Session["config"].ToString();
                objproptest.UserName = Convert.ToString(Session["Username"]);
                objproptest.Cus_TestItemValue = dt;
                objtestbl.UpdateTestCustomItemValue(objproptest);

                // get List Customer need alert
                DataSet lsAlert = new DataSet();
                objtestbl = new BL_SafetyTest();
                objproptest = new SafetyTest();
                objproptest.ConnConfig = Session["config"].ToString();
                objproptest.Cus_TestItemValue = dt;
                lsAlert = objtestbl.GetCustomFieldAlert(objproptest);
                if (lsAlert.Tables.Count > 0)
                {
                    GetTeamMember();
                    processSendMail(lsAlert.Tables[0]);
                    processCreateTask(lsAlert.Tables[0]);
                }

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Test updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }
    private void processSendMail(DataTable obj)
    {

        DataTable lstProjectTeamMember = (DataTable)ViewState["AllProjectTeamMemberList"];
        List<NotificationCustomChange> lsNotificationTask = new List<NotificationCustomChange>();
        List<String> lsEmail = new List<string>();
        DataSet ds = new DataSet();
        BusinessEntity.User objProp_User = new BusinessEntity.User();
        foreach (DataRow r in obj.Rows)
        {
            if (!String.IsNullOrEmpty(r["TeamMember"].ToString()) && r["IsAlert"].ToString() == "True")
            {
                NotificationCustomChange notification = new NotificationCustomChange();
                notification.SubjectEmail = r["Account"].ToString() + " - Equip ID " + r["EquipmentID"].ToString() + "Test Type " + r["TestType"].ToString() + " Alert";
                notification.UserName = Session["username"].ToString();
                notification.label = r["label"].ToString();
                notification.EquipmentName = r["EquipmentName"].ToString();
                notification.EquipmentDesc = r["EquipmentDesc"].ToString();

                List<String> ls = r["TeamMember"].ToString().Split(';').ToList();
                foreach (string iEmail in ls)
                {
                    string[] arr = iEmail.Split('_');
                    //check create task
                    if (arr.Length > 2)
                    {
                        if (arr[2] == "0")
                        {

                            if (arr[0] == "6")
                            {
                                var changedItem = lstProjectTeamMember.Select("memberkey='" + arr[0] + "_" + arr[1] + "'").FirstOrDefault();
                                var role = changedItem["RoleName"].ToString();
                                var projTeamUsers = lstProjectTeamMember.Select("RoleName='" + role.Trim() + "'").ToList();
                                foreach (var projUser in projTeamUsers)
                                {
                                    lsEmail.Add((string)projUser["email"]);
                                }
                            }
                            else
                            {
                                var changedItem = lstProjectTeamMember.Select("memberkey='" + arr[0] + "_" + arr[1] + "'").FirstOrDefault();
                                lsEmail.Add((string)changedItem["email"]);

                            }
                        }
                    }
                    else
                    {
                        if (arr[0] == "6")
                        {
                            var changedItem = lstProjectTeamMember.Select("memberkey='" + arr[0] + "_" + arr[1] + "'").FirstOrDefault();
                            var role = changedItem["RoleName"].ToString();
                            var projTeamUsers = lstProjectTeamMember.Select("RoleName='" + role.Trim() + "'").ToList();
                            foreach (var projUser in projTeamUsers)
                            {
                                lsEmail.Add((string)projUser["email"]);
                            }
                        }
                        else
                        {
                            var changedItem = lstProjectTeamMember.Select("memberkey='" + arr[0] + "_" + arr[1] + "'").FirstOrDefault();
                            lsEmail.Add((string)changedItem["email"]);

                        }
                    }

                }
                if (lsEmail.Count > 0)
                {
                    Mail mail = new Mail();
                    mail.From = WebBaseUtility.GetFromEmailAddress();
                    mail.To = lsEmail;
                    mail.Title = notification.SubjectEmail;
                    mail.Text = notification.EmailContent();

                    WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                    mail.Send();
                }

            }

        }




    }

    private void processCreateTask(DataTable obj)
    {

        List<String> ls = new List<string>();
        DataTable lstProjectTeamMember = (DataTable)ViewState["AllProjectTeamMemberList"];
        List<NotificationCustomChange> lsNotificationTask = new List<NotificationCustomChange>();

        foreach (DataRow r in obj.Rows)
        {

            if (!String.IsNullOrEmpty(r["TeamMember"].ToString()))
            {
                ls = new List<string>();
                ls = r["TeamMember"].ToString().Split(';').ToList();
                String subject = r["Account"].ToString() + " - Equip ID " + r["EquipmentID"].ToString() + "Test Type " + r["TestType"].ToString() + " Alert";
                String contend = Session["username"].ToString() + " has edited " + r["label"].ToString() + " for " + r["EquipmentName"].ToString() + " and " + r["EquipmentDesc"].ToString();

                foreach (string item in ls)
                {
                    string[] arr = item.Split('_');
                    if (arr.Length > 2)
                    {
                        if (arr[2] == "1")
                        {

                            if (arr[0] == "6")
                            {


                                var changedItem = lstProjectTeamMember.Select("memberkey='" + arr[0] + "_" + arr[1] + "'").FirstOrDefault();
                                if (changedItem != null)
                                {
                                    var role = changedItem["RoleName"].ToString();
                                    var projTeamUsers = lstProjectTeamMember.Select("RoleName='" + role.Trim() + "'").ToList();
                                    foreach (var projUser in projTeamUsers)
                                    {
                                        CreateTaskOnWorkflowChange(Convert.ToInt32(r["RolID"]), Convert.ToInt32(r["EquipmentID"]), r["LocName"].ToString(), subject, contend, (string)projUser["fUser"], (string)projUser["email"]);
                                    }
                                }


                            }
                            else
                            {

                                var changedItem = lstProjectTeamMember.Select("memberkey='" + arr[0] + "_" + arr[1] + "'").FirstOrDefault();
                                if (changedItem != null)
                                {
                                    CreateTaskOnWorkflowChange(Convert.ToInt32(r["RolID"]), Convert.ToInt32(r["EquipmentID"]), r["LocName"].ToString(), subject, contend, (string)changedItem["fUser"], (string)changedItem["email"]);
                                }


                            }

                        }
                    }

                }
            }

        }


    }

    #region show/hidden column
    protected void lnkSaveGridSettings_Click(object sender, EventArgs e)
    {
        BL_User objBL_User = new BL_User();
        User objProp_User = new User();
        #region Grid user settings
        var columnSettings = GetGridColumnSettings();
        objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objProp_User.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        objProp_User.PageName = "SafetyTest.aspx";
        objProp_User.GridId = "RadGrid_SafetyTest";

        objBL_User.UpdateUserGridCustomSettings(objProp_User, columnSettings);
        #endregion
    }
    protected void lnkRestoreGridSettings_Click(object sender, EventArgs e)
    {
        BL_User objBL_User = new BL_User();
        User objProp_User = new User();
        #region Grid user settings
        var columnSettings = string.Empty;
        objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objProp_User.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        objProp_User.PageName = "SafetyTest.aspx";
        objProp_User.GridId = "RadGrid_SafetyTest";

        var ds = objBL_User.DeleteUserGridCustomSettings(objProp_User);


        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            columnSettings = ds.Tables[0].Rows[0][0].ToString();
            var columnsArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ColumnSettings>>(columnSettings);

            var colIndex = 0;

            foreach (GridColumn column in RadGrid_SafetyTest.MasterTableView.OwnerGrid.Columns)
            {
                colIndex++;
                var clSetting = columnsArr.Where(t => t.Name.Equals(column.UniqueName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (clSetting != null)
                {
                    column.Display = clSetting.Display;
                    if (colIndex >= 3 && clSetting.Width != 0)
                        column.HeaderStyle.Width = clSetting.Width;

                    column.OrderIndex = clSetting.OrderIndex;
                }
            }
            RadGrid_SafetyTest.MasterTableView.Rebind();
        }
        else
        {
            //var arrColumnOrder = new string[3]{ "ReviewCheck", "Comp", "" };
            var colIndex = 0;
            foreach (GridColumn column in RadGrid_SafetyTest.MasterTableView.OwnerGrid.Columns)
            {
                colIndex++;
                column.Display = true;

            }
            RadGrid_SafetyTest.MasterTableView.SortExpressions.Clear();
            RadGrid_SafetyTest.MasterTableView.GroupByExpressions.Clear();
            RadGrid_SafetyTest.EditIndexes.Clear();
            RadGrid_SafetyTest.Rebind();
        }
        //CompanyPermission();
        // just for TEI
        if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("Transel"))
        {
            RadGrid_SafetyTest.MasterTableView.GetColumn("Capacity").Visible = true;
        }
        else
        {
            RadGrid_SafetyTest.MasterTableView.GetColumn("Capacity").Visible = false;
        }
        RadGrid_SafetyTest.MasterTableView.GetColumn("Company").Visible = Session["COPer"].ToString() == "1";
        #endregion
    }
    private string GetGridColumnSettings()
    {
        var columnSettings = string.Empty;

        List<ColumnSettings> lstColSetts = new List<ColumnSettings>();
        foreach (GridColumn column in RadGrid_SafetyTest.MasterTableView.OwnerGrid.Columns)
        {
            var colSett = new ColumnSettings();
            colSett.Name = column.UniqueName;
            colSett.Display = column.Display;
            colSett.Width = (int)column.HeaderStyle.Width.Value;
            colSett.OrderIndex = column.OrderIndex;
            lstColSetts.Add(colSett);
        }

        columnSettings = Newtonsoft.Json.JsonConvert.SerializeObject(lstColSetts);
        return columnSettings;
    }
    private string GetDefaultGridColumnSettingsFromDb()
    {
        BL_User objBL_User = new BL_User();
        User objProp_User = new User();
        var columnSettings = string.Empty;
        objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objProp_User.PageName = "SafetyTest.aspx";
        objProp_User.GridId = "RadGrid_SafetyTest";
        var ds = objBL_User.GetDefaultGridCustomSettings(objProp_User);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            columnSettings = ds.Tables[0].Rows[0][0].ToString();
        }

        return columnSettings;
    }

    private void DefineGridStructure()
    {
        BL_User objBL_User = new BL_User();
        User objProp_User = new User();

        DataSet ds = new DataSet();
        objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objProp_User.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        objProp_User.PageName = "SafetyTest.aspx";
        objProp_User.GridId = "RadGrid_SafetyTest";
        ds = objBL_User.GetGridUserSettings(objProp_User);

        if (ds.Tables[0].Rows.Count > 0)
        {
            //string columnSettings = "[{Name: \"BType\", Display: true, Width: 300},{Name: \"MatItem\", Display: false, Width: 300}]";
            var columnSettings = ds.Tables[0].Rows[0][0].ToString();
            var columnsArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ColumnSettings>>(columnSettings);

            var colIndex = 0;

            foreach (GridColumn column in RadGrid_SafetyTest.MasterTableView.OwnerGrid.Columns)
            {
                colIndex++;
                var clSetting = columnsArr.Where(t => t.Name.Equals(column.UniqueName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (clSetting != null)
                {
                    column.Display = clSetting.Display;
                    if (colIndex >= 3 && clSetting.Width != 0)
                        column.HeaderStyle.Width = clSetting.Width;

                    column.OrderIndex = clSetting.OrderIndex;
                }
            }

            // ScriptManager.RegisterStartupScript(this, Page.GetType(), "showhidebutton", "ShowRestoreGridSettingsButton();", true);
        }


    }
    public class ColumnSettings
    {
        public string Name { get; set; }
        public bool Display { get; set; }
        public int Width { get; set; }
        public int OrderIndex { get; set; }
    }

    #endregion

    #region Save filter
    private void SaveFilter()
    {
        Session["STfilterstate"] = ddlSearch.SelectedValue + ";"
            + ddlTestTypes.SelectedValue + ";"
            + ddlProposal.SelectedValue + ";"
            + txtSearch.Text + ";"
            + txtStartDate.Text + ";"
            + txtEndDate.Text + ";"
            + lnkChk.Checked + ";"
            + ddlBillingAmount.SelectedValue + ";"
            + lnkIncludeInActiveTest.Checked;
    }
    public void UpdateControl()
    {
        IsGridPageIndexChanged = true;
        if (Session["STfilterstate"] != null)
        {
            if (Session["STfilterstate"].ToString() != string.Empty)
            {
                string[] strFilter = Session["STfilterstate"].ToString().Split(';');
                ddlSearch.SelectedValue = strFilter[0];
                ddlTestTypes.SelectedValue = strFilter[1];
                ddlProposal.SelectedValue = strFilter[2];
                txtSearch.Text = strFilter[3];
                txtStartDate.Text = strFilter[4];
                txtEndDate.Text = strFilter[5];
                lnkChk.Checked = Convert.ToBoolean(strFilter[6]);
                ddlBillingAmount.SelectedValue = strFilter[7];
                lnkIncludeInActiveTest.Checked = Convert.ToBoolean(strFilter[8]);
                lblDay.Attributes.Remove("class");
                lblWeek.Attributes.Remove("class");
                lblMonth.Attributes.Remove("class");
                lblQuarter.Attributes.Remove("class");
                lblYear.Attributes.Remove("class");
                switch (hdnRcvPymtSelectDtRange.Value)
                {
                    case "Day":
                        lblDay.Attributes.Add("class", "labelactive");
                        break;
                    case "Week":
                        lblWeek.Attributes.Add("class", "labelactive");
                        break;
                    case "Month":
                        lblMonth.Attributes.Add("class", "labelactive");
                        break;
                    case "Quarter":
                        lblQuarter.Attributes.Add("class", "labelactive");
                        break;
                    case "Year":
                        lblYear.Attributes.Add("class", "labelactive");
                        break;
                }
            }
        }

        showFilterSearch();
    }
    public void showFilterSearch()
    {

        txtSearch.Style.Add("display", "none");
        ddlBillingAmount.Style.Add("display", "none");

        if (ddlSearch.SelectedValue == "IsDefaultAmount")
        {

            ddlBillingAmount.Style.Add("display", "block");


        }
        else
        {
            txtSearch.Style.Add("display", "block");
        }
    }
    private void resetClear()
    {
        IsGridPageIndexChanged = true;
        ddlSearch.SelectedValue = "";
        ddlTestTypes.SelectedIndex = 0;
        ddlProposal.SelectedIndex = 0;
        lnkChk.Checked = false;
        lnkIncludeInActiveTest.Checked = false;
        txtSearch.Text = "";
        lblDay.Attributes.Remove("class");
        lblWeek.Attributes.Remove("class");
        lblMonth.Attributes.Remove("class");
        lblQuarter.Attributes.Remove("class");
        lblYear.Attributes.Remove("class");
        switch (hdnRcvPymtSelectDtRange.Value)
        {
            case "Day":
                lblDay.Attributes.Add("class", "labelactive");
                break;
            case "Week":
                lblWeek.Attributes.Add("class", "labelactive");
                break;
            case "Month":
                lblMonth.Attributes.Add("class", "labelactive");
                break;
            case "Quarter":
                lblQuarter.Attributes.Add("class", "labelactive");
                break;
            case "Year":
                lblYear.Attributes.Add("class", "labelactive");
                break;

        }

    }

    private void resetClearForInactiveChecked()
    {
        IsGridPageIndexChanged = true;
        ddlSearch.SelectedValue = "";
        ddlTestTypes.SelectedIndex = 0;
        ddlProposal.SelectedIndex = 0;
        lnkChk.Checked = false;
        txtSearch.Text = "";
        lblDay.Attributes.Remove("class");
        lblWeek.Attributes.Remove("class");
        lblMonth.Attributes.Remove("class");
        lblQuarter.Attributes.Remove("class");
        lblYear.Attributes.Remove("class");
        switch (hdnRcvPymtSelectDtRange.Value)
        {
            case "Day":
                lblDay.Attributes.Add("class", "labelactive");
                break;
            case "Week":
                lblWeek.Attributes.Add("class", "labelactive");
                break;
            case "Month":
                lblMonth.Attributes.Add("class", "labelactive");
                break;
            case "Quarter":
                lblQuarter.Attributes.Add("class", "labelactive");
                break;
            case "Year":
                lblYear.Attributes.Add("class", "labelactive");
                break;

        }

    }

    private void resetShowAll()
    {
        IsGridPageIndexChanged = true;
        lblDay.Attributes.Remove("class");
        lblWeek.Attributes.Remove("class");
        lblMonth.Attributes.Remove("class");
        lblQuarter.Attributes.Remove("class");
        lblYear.Attributes.Remove("class");
        ddlSearch.SelectedValue = "";
        txtSearch.Text = "";
        lblWeek.Attributes.Add("class", "labelactive");
        lnkChk.Checked = false;
        lnkIncludeInActiveTest.Checked = false;
    }

    #endregion

    protected void lnkChk_CheckedChanged(object sender, EventArgs e)
    {
        SaveFilter();
        RadGrid_SafetyTest.Rebind();
    }

    protected void lnkIncludeInActiveTest_CheckedChanged(object sender, EventArgs e)
    {
        SaveFilter();
        RadGrid_SafetyTest.Rebind();
    }

    private void GetTeamMember()
    {
        User objPropUser = new User();
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.GetUsersAndRolesForTeamMemberList(objPropUser);
        var teamMembers = ds.Tables[0];

        DataTable contactList = WebBaseUtility.GetContactListOnExchangeServer();
        if (contactList != null && contactList.Rows.Count > 0)
        {
            // Merge this list to teamMembers
            foreach (DataRow item in contactList.Rows)
            {
                DataRow _dr = teamMembers.NewRow();
                if (string.IsNullOrEmpty(item["GroupName"].ToString()))
                {
                    _dr["memberkey"] = "3_" + item["Type"] + "|" + item["MemberEmail"] + "|" + item["MemberName"];
                    _dr["usertype"] = "Exchange " + item["Type"];
                }
                else
                {
                    _dr["memberkey"] = "4_" + item["GroupName"] + "|" + item["MemberEmail"] + "|" + item["MemberName"];
                    _dr["usertype"] = "Exchange Group: " + item["GroupName"];
                }
                _dr["fUser"] = item["MemberName"];
                _dr["email"] = item["MemberEmail"];
                _dr["roleName"] = "";
                teamMembers.Rows.Add(_dr);
            }
        }
        ViewState["AllProjectTeamMemberList"] = teamMembers;

    }
    private void CreateTaskOnWorkflowChange(int RolID, int equiID, String LocName, string strSubject, string strRemarks, string assignedTo, string strMailTo = "")
    {
        BL_Customer objBL_Customer = new BL_Customer();
        var objCustomer = new Customer();
        objCustomer.ConnConfig = Session["config"].ToString();
        objCustomer.ROL = RolID;
        objCustomer.DueDate = DateTime.Now;
        objCustomer.TimeDue = Convert.ToDateTime("01/01/1900 " + DateTime.Now.ToShortTimeString());
        objCustomer.Subject = strSubject;
        objCustomer.Remarks = strRemarks;
        objCustomer.AssignedTo = assignedTo;
        double dblDuration = 0.5;
        objCustomer.Duration = dblDuration;
        objCustomer.Name = Session["Username"].ToString();
        //objProp_Customer.Contact = txtContact.Text;
        objCustomer.Status = 0;//Open
        objCustomer.Resolution = "";
        objCustomer.LastUpdateUser = Session["username"].ToString();
        objCustomer.Category = "To Do";
        objCustomer.IsAlert = true;

        try
        {
            objCustomer.TaskID = 0;
            objCustomer.Mode = 0;
            objCustomer.Screen = "Equipment";

            objCustomer.Ref = Convert.ToInt32(equiID);
            objBL_Customer.AddTask(objCustomer);

            #region Thomas: Send email with a appointment to login user 
            if (objCustomer.IsAlert)
            {
                // Create
                BusinessEntity.User objPropUser = new BusinessEntity.User();
                BL_User objBL_User = new BL_User();
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.Username = assignedTo;

                var mailTo = string.Empty;
                if (!string.IsNullOrEmpty(strMailTo))
                {
                    mailTo = strMailTo;
                }
                else
                {
                    mailTo = objBL_User.getUserEmail(objPropUser);
                }

                if (!string.IsNullOrEmpty(mailTo))
                {
                    Mail mail = new Mail();
                    try
                    {
                        var uri = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, Request.ApplicationPath + "/addTask?uid=" + objCustomer.TaskID.ToString());
                        foreach (var toaddress in mailTo.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            mail.To.Add(toaddress.Trim());
                        }
                        //mail.To.Add(mailTo);
                        mail.From = WebBaseUtility.GetFromEmailAddress();
                        //mail.Title = "Task Appointment";
                        mail.Title = LocName + ": " + objCustomer.Subject;

                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.AppendFormat("Dear {0}<br><br>", objCustomer.AssignedTo);
                        stringBuilder.Append("You are receiving an appointment task from MOM-->Sales-->Tasks<br><br>");
                        stringBuilder.AppendFormat("Location Name: {0}<br>", LocName);

                        stringBuilder.AppendFormat("Subject: {0}<br>", objCustomer.Subject);
                        stringBuilder.AppendFormat("Description: {0}<br>", objCustomer.Remarks);
                        stringBuilder.AppendFormat("Due on: {0} {1}<br><br>", objCustomer.DueDate.ToString("MM/dd/yyyy"), DateTime.Now.ToShortTimeString());
                        stringBuilder.Append("Attached files is a task appointment assigned to you.<br>");
                        stringBuilder.Append("To add this appointment to your calendar, please open and save it<br><br>");
                        stringBuilder.AppendFormat("<a href={0}>{0}</a><br><br>", uri);
                        stringBuilder.Append("Thanks");

                        mail.Text = stringBuilder.ToString();

                        //todo
                        WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                        var apSubject = string.Format("Task name: {0}", LocName);

                        StringBuilder apBody = new StringBuilder();
                        var _strRemarks = objCustomer.Remarks.Replace("\r\n", "=0D=0A").Replace("\n", "=0D=0A");
                        apBody.AppendFormat("{0}.=0D=0A", _strRemarks);
                        apBody.AppendFormat("Due on: {0} {1}. =0D=0A ", objCustomer.DueDate.ToString("MM/dd/yyyy"), DateTime.Now.ToShortTimeString());
                        apBody.Append("Attached files is a task appointment assigned to you.  =0D=0A");
                        apBody.Append("To add this appointment to your calendar, please open and save it.=0D=0A");
                        apBody.Append("Thanks");


                        var strStartDate = string.Format("{0} {1}", objCustomer.DueDate.ToString("MM/dd/yyyy"), DateTime.Now.ToShortTimeString());
                        var apStart = Convert.ToDateTime(strStartDate);
                        var apEnd = apStart.AddHours(objCustomer.Duration);
                        //todo
                        var icsAttachmentContentsStr = WebBaseUtility.CreateICSAttachmentCalendarStr(apSubject
                            , apBody.ToString()
                            , LocName
                            , apStart
                            , apEnd
                            , 60
                            );
                        var myByteArray = System.Text.Encoding.UTF8.GetBytes(icsAttachmentContentsStr);
                        mail.attachmentBytes = myByteArray;
                        mail.FileName = "TaskAppointment.ics";
                        mail.Send();
                    }
                    catch (Exception ex)
                    {

                        string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
            }
            #endregion

        }
        catch (Exception ex)
        {

            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }


    }

    bool isGroupLog = false;
    protected void RadGrid_gvLogs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();
        DataSet dsLog = new DataSet();
        EmailLog emailLog = new EmailLog();
        emailLog.Screen = "SafetyTest";
        emailLog.ConnConfig = Session["config"].ToString();
        BL_EmailLog bL_EmailLog = new BL_EmailLog();
        dsLog = bL_EmailLog.GetEmailLogs(emailLog);
        if (dsLog.Tables[0].Rows.Count > 0)
        {
            RadGrid_gvLogs.VirtualItemCount = dsLog.Tables[0].Rows.Count;
            RadGrid_gvLogs.DataSource = dsLog.Tables[0];
        }
        else
        {
            RadGrid_gvLogs.DataSource = string.Empty;
        }
    }
    public bool ShouldApplySortFilterOrGroupLogs()
    {
        return RadGrid_gvLogs.MasterTableView.FilterExpression != "" ||
            (RadGrid_gvLogs.MasterTableView.GroupByExpressions.Count > 0 || isGroupLog) ||
            RadGrid_gvLogs.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_gvLogs_ItemCreated(object sender, GridItemEventArgs e)
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


    protected void ddlStatusDocument_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            String selectValue = ((DropDownList)sender).SelectedValue;
            int id = Convert.ToInt32(((DropDownList)sender).Attributes["ProposalID"]);
            BL_SafetyTest obj = new BL_SafetyTest();

            obj.UpdateStatusProposalForm(Session["config"].ToString(), id,
                      selectValue, Session["username"].ToString());


            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: 'Document updated status successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            RadGrid_SafetyTest.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrContct", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    protected void RadGrid_SafetyTest_ItemDataBound(object sender, GridItemEventArgs e)
    {
        e.Item.Edit = true;
        foreach (GridDataItem gvRow in RadGrid_SafetyTest.Items)
        {
            DropDownList ddlStatus = gvRow.FindControl("ddlStatusDocument") as DropDownList;
            HiddenField hdnID = gvRow.FindControl("hdnProposalId") as HiddenField;

            Label lblStatus = (Label)gvRow.FindControl("lblProposalStatus");
            if (ddlStatus != null)
            {
                ddlStatus.SelectedValue = lblStatus.Text;
                ddlStatus.Attributes.Add("ProposalID", hdnID.Value);

            }

            //Set default Schedule
            DropDownList ddlScheduledStatus = gvRow.FindControl("ddlScheduledStatus") as DropDownList;
            HiddenField hdnScheduledStatus = gvRow.FindControl("hdnScheduledStatus") as HiddenField;
            if (ddlScheduledStatus != null)
            {
                if (hdnScheduledStatus.Value == "")
                {
                    ddlScheduledStatus.SelectedValue = "0";
                }
                else
                {
                    ddlScheduledStatus.SelectedValue = hdnScheduledStatus.Value;
                }

            }

            //Set default service
            DropDownList ddlServiceStatus = gvRow.FindControl("ddlServiceStatus") as DropDownList;
            HiddenField hdnServiceStatus = gvRow.FindControl("hdnServiceStatus") as HiddenField;
            if (ddlServiceStatus != null)
            {
                if (hdnServiceStatus.Value == "")
                {
                    ddlServiceStatus.SelectedValue = "0";
                }
                else
                {
                    ddlServiceStatus.SelectedValue = hdnServiceStatus.Value;
                }

            }


        }

        //add event for Custom Control
        List<String> lsFormual = new List<string>();
        if (Session["FormulaControl"] != null)
        {
            List<CustomFieldID> ls = (List<CustomFieldID>)Session["FormulaControl"];
            if (ls.Count > 0)
            {

                JavaScriptSerializer sr = new JavaScriptSerializer();



                foreach (CustomFieldID str in ls)
                {

                    if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                    {
                        var txt = (e.Item as GridEditableItem)[str.controlID].Controls[0] as TextBox;
                        if (txt != null)
                        {
                            string param = sr.Serialize(ls);
                            txt.Attributes.Add("onchange", "UpdateFireDateMulti(this," + param + ")");
                            //txt.Attributes.Add("onchange", "UpdateFireDate(this,'TB_" + str.controlID + "','TB_" + str.ControlUpdate + "'," + str.xFireDay + "," + str.xScheduleDay + ")");
                            lsFormual.Add(str.controlID);
                        }
                    }
                }

                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    var txt = (e.Item as GridEditableItem)["EDate"].Controls[1] as TextBox;
                    if (txt != null)
                    {
                        txt.Attributes.Add("onchange", "UpdateScheduleDateMulti(this," + sr.Serialize(ls) + ")");
                        //txt.Attributes.Add("onchange", "UpdateScheduleDate(this,'TB_" + ls[0].controlID + "','TB_" + ls[0].ControlUpdate + "'," + ls[0].xFireDay + "," + ls[0].xScheduleDay + ")");
                    }

                }
            }


        }
        //add event for Custom Control
        if (Session["CustomControl"] != null)
        {
            List<String> ls = (List<String>)Session["CustomControl"];
            foreach (String str in ls)
            {
                if (lsFormual.Contains(str) != true)
                {
                    if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                    {
                        var txt = (e.Item as GridEditableItem)[str].Controls[0] as TextBox;
                        if (txt != null)
                        {
                            txt.Attributes.Add("onchange", "UpdateTextCustomField(this,'TB_" + str + "')");
                        }

                        var cbo = (e.Item as GridEditableItem)[str].Controls[0] as RadComboBox;
                        if (cbo != null)
                        {
                            cbo.Attributes.Add("onchange", "UpdateTextCustomField(this,'RCB_" + str + "')");
                        }

                    }
                }

            }

        }
    }

    public void CreateMergedPDF(string targetPDF)
    {
        try
        {

            using (FileStream stream = new FileStream(targetPDF, FileMode.Create))
            {
                List<string> ls = new List<string>();
                ls = GetListDocument();
                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4);
                iTextSharp.text.pdf.PdfCopy pdf = new iTextSharp.text.pdf.PdfCopy(pdfDoc, stream);
                pdfDoc.Open();
                var files = ls.ToArray();
                int i = 1;
                foreach (string file in files)
                {
                    pdf.AddDocument(new iTextSharp.text.pdf.PdfReader(file));
                    i++;
                }

                if (pdfDoc != null)
                    pdfDoc.Close();

            }
        }
        catch (Exception ex)
        {

            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "processDownloadPDFError", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

        }

    }

    public List<string> GetListDocument()
    {
        List<string> lsDocument = new List<string>();



        if (Session["safetytest"] != null)
        {
            DataTable dtResult = processDataFilter((DataTable)Session["safetytest"]);
            try
            {
                DataTable dt = processDataFilter((DataTable)Session["safetytest"]);
                DataView dv = dt.DefaultView;
                if (RadGrid_SafetyTest.MasterTableView.SortExpressions.Count > 0)
                {
                    dv.Sort = RadGrid_SafetyTest.MasterTableView.SortExpressions[0].ToString();
                }
                DataTable temp = dv.ToTable(true, "ProposalID");

                foreach (DataRow row in temp.Rows)
                {
                    if (row["ProposalID"].ToString() != "")
                    {
                        //get Proposal detail
                        BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
                        DataSet ds = bl_SafetyTest.GetProposalFormByID(Session["config"].ToString(), Convert.ToInt32(row["ProposalID"].ToString()));
                        if (ds.Tables.Count > 0)
                        {
                            lsDocument.Add(ds.Tables[0].Rows[0]["PdfFilePath"].ToString());
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "processSendProposalError", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }


        }
        return lsDocument;
    }

    protected void lnkPDF_Click(object sender, EventArgs e)
    {


        if (Session["safetytest"] != null)
        {
            DataTable dtResult = processDataFilter((DataTable)Session["safetytest"]);
            if (dtResult.Select("ProposalStatus <>''").Count() > 0)
            {
                string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                string savepath = savepathconfig + "\\" + Session["dbname"] + "\\SafetyTest\\";
                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }
                //CreateFile
                String proposalFileName = "Summary_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + "pdf";

                String pathPDF = savepath + proposalFileName;
                PdfDocument doc = new PdfDocument();
                doc.SaveToFile(pathPDF);

                doc.Close();


                CreateMergedPDF(pathPDF);
                DownloadDocument(pathPDF, proposalFileName);
            }
            else
            {
                string str = "There is no proposal file to download.";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "NoProposalError", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
        }
        else
        {
            string str = "There is no proposal file to download.";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "NoProposalError", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

        }


    }

    private String getRouteLabel()
    {
        BL_Customer objBL_Customer = new BL_Customer();
        Customer objCustomer = new Customer();

        objCustomer.ConnConfig = Session["config"].ToString();
        _GetDefaultWorkerHeader.ConnConfig = Session["config"].ToString();

        string getValue;

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "LocationsAPI/LocationsList_GetDefaultWorkerHeader";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetDefaultWorkerHeader);

            object JsonData = JsonConvert.DeserializeObject(_APIResponse.ResponseData);
            getValue = JsonData.ToString();
        }
        else
        {
            getValue = objBL_Customer.GetDefaultWorkerHeader(objCustomer);
        }

        if (!string.IsNullOrEmpty(getValue))
        {
            return getValue;
        }
        else
        {
            return "Default Worker";
        }
    }

    protected void RadGrid_SafetyTest_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;
            Session["RadGrid_SafetyTestCurrentPageIndex"] = e.NewPageIndex;
            ViewState["RadGrid_SafetyTestminimumRows"] = e.NewPageIndex * RadGrid_SafetyTest.PageSize;
            ViewState["RadGrid_SafetyTestmaximumRows"] = (e.NewPageIndex + 1) * RadGrid_SafetyTest.PageSize;
        }
        catch { }
    }

    protected void RadGrid_SafetyTest_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;
            ViewState["RadGrid_SafetyTestminimumRows"] = RadGrid_SafetyTest.CurrentPageIndex * e.NewPageSize;
            ViewState["RadGrid_SafetyTestmaximumRows"] = (RadGrid_SafetyTest.CurrentPageIndex + 1) * e.NewPageSize;
        }
        catch { }
    }


    protected void RadGrid_Emails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        User objPropUser = new User();
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Status = 0;
        objPropUser.Username = "";
        ds = objBL_User.getEMP(objPropUser);


        RadGrid_Emails.DataSource = ds.Tables[0];

    }

    protected void RadGrid_Emails_PreRender(object sender, EventArgs e)
    {
        String filterExpression = Convert.ToString(RadGrid_Emails.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Emails_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Emails.MasterTableView.OwnerGrid.Columns)
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

            Session["Emails_Filters"] = filters;
        }
        else
        {
            Session["Emails_FilterExpression"] = null;
            Session["Emails_Filters"] = null;
        }

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "bindingClickCheckbox", "BindClickEventForGridCheckBox();", true);
    }

    protected void lnkTESTReportLocation_Click(object sender, EventArgs e)
    {
        processRedirect("TestScheduledDetailsReport.aspx?page=SafetyTest");
    }

    private void processRedirect(string url)
    {
        Response.Redirect(url, true);
    }






    protected void lnkAddcalendar_Click(object sender, EventArgs e)
    {




        List<string> LocationList = new List<string>();

        foreach (GridDataItem gr in RadGrid_SafetyTest.Items)
        {
            Label lblAccount = (Label)gr.FindControl("lblAccount");

            Label lblcredithold = (Label)gr.FindControl("lblcredithold");

            TextBox txtScheduleDate = (TextBox)gr.FindControl("txtScheduleDate");

            TextBox txtMembers = (TextBox)gr.FindControl("txtMembers");

            if (txtScheduleDate.Text != "" && txtMembers.Text != "")
            {
                if (lblcredithold.Text == "1")
                {
                    string str = " ( " + lblAccount.Text + " ) Location on credit hold!";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "keyIsCreditHold" + lblAccount.Text, "noty(text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                }
                else
                {

                    if (!LocationList.Contains(lblAccount.Text))
                    {
                        LocationList.Add(lblAccount.Text);
                    }
                }
            }
        }


        if (LocationList.Count > 0)
        {

            Session["SafetyTestAppointLocationList"] = LocationList;

            Response.Redirect("SendAppointment.aspx");

        }
        else
        {
            string str = "Please select Schedule date and Worker";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrlopk", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }
    }


    [Serializable]
    public class CustomFieldID
    {
        public String controlName { get; set; }
        public String controlID { get; set; }
        public int xFireDay { get; set; }
        public int xScheduleDay { get; set; }
        public String ControlUpdate { get; set; }
        public CustomField ControlType { get; set; }
    }

    protected void Button1Schedule_Click(object sender, EventArgs e)
    {
        BL_SafetyTest objtestbl = new BL_SafetyTest();

        if (HiddenField1B.Value == "1")
        {


            foreach (GridDataItem gr in RadGrid_SafetyTest.Items)
            {
                HiddenField hdnTestID = (HiddenField)gr.FindControl("hdnTestID");

                Label lblTestYear = (Label)gr.FindControl("lblTestYear");

                objtestbl.UpdateTestScheduledStatus(Session["config"].ToString(), HiddenFieldStatus.Value, lblTestYear.Text, hdnTestID.Value);
            }
        }

        objtestbl.UpdateTestScheduledStatus(Session["config"].ToString(), HiddenFieldStatus.Value, HiddenFieldYear.Value, HiddenFieldLID.Value);

        string str = "Schedule Status Updated successfully.";

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "fbdgdrgdgdfg", "noty({text: '" + str + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

    }

    protected void ddlScheduledStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList dropdownlist1 = (DropDownList)sender;

        GridDataItem item = (GridDataItem)dropdownlist1.NamingContainer;

        HiddenField hdnTestID = (HiddenField)item.FindControl("hdnTestID");

        Label lblTestYear = (Label)item.FindControl("lblTestYear");

        TextBox txtScheduleDate = (TextBox)item.FindControl("txtScheduleDate");

        if (txtScheduleDate.Text == "")
        {

            string str = "Please select Schedule Date.";

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "fbdgdrgdgdfg", "noty({text: '" + str + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

        }
        else
        {

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrljjjfhjopk", "UpdateddlScheduledStatusMulti(" + hdnTestID.Value + ", " + lblTestYear.Text + ", " + dropdownlist1.SelectedValue + ");", true);
        }
    }
}