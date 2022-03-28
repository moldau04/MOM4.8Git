using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using BusinessEntity;
using System.Web.UI.HtmlControls;
using System.Data;
using AjaxControlToolkit;
using Telerik.Web.UI;
using BusinessLayer.Billing;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Drawing;

public partial class Setup : System.Web.UI.Page
{
    #region "Variables"
    BL_User objBL_User = new BL_User();
    BL_Wage objBL_Wage = new BL_Wage();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    //consult
    BusinessEntity.tblConsult objProp_Consult = new BusinessEntity.tblConsult();
    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    BL_Customer objBL_Customer = new BL_Customer();
    BL_Vendor objBL_Vendor = new BL_Vendor();
    Customer objCustomer = new Customer();
    BL_ReportsData objBL_ReportData = new BL_ReportsData();
    BL_Inventory objBL_Inventory = new BL_Inventory();
    GeneralFunctions objGeneralFunctions = new GeneralFunctions();
    public static bool IsAddEdit = false;
    public static bool IsDelete = false;
    Wage _objWage = new Wage();
    bool api = false;
    //TestCustom   
    protected DataTable dtTestCstFormat = new DataTable();
    protected DataTable dtWorkflowFormat = new DataTable();
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        objProp_User.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_ReportData.GetControlForPayroll(objProp_User);
        bool PR = Convert.ToBoolean(DBNull.Value.Equals(ds.Tables[0].Rows[0]["PR"]) ? 0 : ds.Tables[0].Rows[0]["PR"]);

        if (PR == true)
        {
            wagetab.Visible = false;
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

            if (!string.IsNullOrEmpty(Request.QueryString["rw"]))
            {
                hdnQueryString.Value = Request.QueryString["rw"];
            }
            if (Request.QueryString["tab"] != null)
            {
                //TabContainer2.ActiveTabIndex = 7;

            }
            if (Request.QueryString["tabWarehouse"] != null)
            {
                //TabContainerInventory.Style["display"] = "block";
                //liContainerInventory.Style["display"] = "inline-block";
            }
            userpermissions();
            Permission();
            ZonePermission();
            HighlightSideMenu("progMgr", "lnkSetup", "progMgrSub");
            CompanyPermission();
            ViewState["edit"] = 0;
            if (Session["MSM"].ToString() != "TS")
            {

            }

            getDiagnosticCategory();

            FillInventoryCostTypes();

            GetControl();
            GetBillingCode();

            lnkBtnAddParameter.Visible = false;



            FillTestCustomFormat();
            GetTestCustomTable();
            BindTestCustomGrid();
            FillTestSetupForms();
            SetDefaultWorker();


            FillworkflowFormat();
            CreateworkflowTable();
            BindworkflowGrid();
            FillViolationSection();
            FillViolationCategory();

            BindStageItemGrid(true, false);

            FillSalesApproveEstimate();
        }
    }
    private void FillVendors()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getVendorType(objProp_User);
        RadGrid_vendorType.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_vendorType.DataSource = ds.Tables[0];

    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    #region Custom Function
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
            Response.Redirect("home.aspx");
        }
        if (Session["MSM"].ToString() == "TS")
        {
            //Comment by Rustam need to be work for permissioon
            //tbpnlCust.Visible = false;
            //tbpnlBill.Visible = false;
            //tbEquipType.Visible = false;
            //tbpnlSales.Visible = false;

            //lnkAddDepart.Visible = false;
            //lnkDelDepart.Visible = false;
            //lnkAddCat.Visible = false;
            //lnkDelCcat.Visible = false;
            //lnkAddCallCode.Visible = false;
            //lnkDelCallCodes.Visible = false;
            //lnkAddQuickCode.Visible = false;
            //lnkDelQuickCodes.Visible = false;
            //lnkAddPrType.Visible = false;
            //lnkDelPrType.Visible = false;
        }

        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            //Comment by Rustam need to be work for permissioon
            //tbpnlCust.Visible = false;
            //tbpnlBill.Visible = false;

        }

        if (Session["type"].ToString() != "am")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];

            //Inventory item  //Comment by Rustam need to be work for permissioon
            string InventoryWarehousePermission = ds.Rows[0]["Warehouse"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Warehouse"].ToString();
            hdnAddInventoryWarehouse.Value = InventoryWarehousePermission.Length < 1 ? "Y" : InventoryWarehousePermission.Substring(0, 1);
            hdnEditInventoryWarehouse.Value = InventoryWarehousePermission.Length < 2 ? "Y" : InventoryWarehousePermission.Substring(1, 1);
            hdnDeleteInventoryWarehouse.Value = InventoryWarehousePermission.Length < 3 ? "Y" : InventoryWarehousePermission.Substring(2, 1);
            hdnViewInventoryWarehouse.Value = InventoryWarehousePermission.Length < 4 ? "Y" : InventoryWarehousePermission.Substring(3, 1);

            string stViewInventoryWarehouse = InventoryWarehousePermission.Length < 4 ? "Y" : InventoryWarehousePermission.Substring(3, 1);

            Panel20.Visible = stViewInventoryWarehouse == "N" ? false : true;
            RadGrid_Warehouse.Visible = stViewInventoryWarehouse == "N" ? false : true;

            //Inventory setup
            string InventorysetupPermission = ds.Rows[0]["Invsetup"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Invsetup"].ToString();

            hdnViewInventorysetup.Value = InventorysetupPermission.Length < 4 ? "Y" : InventorysetupPermission.Substring(3, 1);
            string stViewInventorysetup = InventorysetupPermission.Length < 4 ? "Y" : InventorysetupPermission.Substring(3, 1);
            TabContainerInventory.Visible = stViewInventorysetup == "N" ? false : true;
        }
    }
    private void userpermissions()
    {
        if (Session["type"].ToString() != "c")
        {
            if (Session["type"].ToString() != "am")
            {
                objProp_User.ConnConfig = Session["config"].ToString();
                objProp_User.Username = Session["username"].ToString();
                objProp_User.PageName = "setup.aspx";
                DataTable dt = new DataTable(); dt = (DataTable)Session["userinfo"];

                string ProgFunc = dt.Rows[0]["Control"].ToString().Substring(0, 1);

                if (ProgFunc == "N")
                {
                    Response.Redirect("home.aspx?permission=no");
                }
                //DataSet dspage = objBL_User.getScreensByUser(objProp_User);
                //if (dspage.Tables[0].Rows.Count > 0)
                //{
                //    if (Convert.ToBoolean(dspage.Tables[0].Rows[0]["access"].ToString()) == false)
                //    {
                //        Response.Redirect("home.aspx");
                //    }
                //}
                //else
                //{
                //    Response.Redirect("home.aspx");
                //}
            }
        }
    }
    private void CompanyPermission()
    {

        if (Session["COPer"].ToString() == "1")
        {
            RadGrid_Warehouse.Columns[4].Visible = true;
        }
        else
        {
            RadGrid_Warehouse.Columns[4].Visible = false;
        }
    }
    private void GetControl()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getControl(objProp_User);
        if (ds.Tables[0].Rows.Count > 0)
        {
            chkInternet.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["tinternett"]);
            chkMCP.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["msreptemp"]);
            if (ds.Tables[0].Rows[0]["businessstart"] != DBNull.Value)
                txtBstart.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["businessstart"]).ToShortTimeString();
            if (ds.Tables[0].Rows[0]["businessend"] != DBNull.Value)
                txtBend.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["businessend"]).ToShortTimeString();
            ddlJobCostLabor.SelectedValue = ds.Tables[0].Rows[0]["JobCostLabor1"].ToString();
            chkTask.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["TaskCode"]);
            //chkTaskProject.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["TaskCode"]);
            ddlTasks.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["codes"].ToString());
            //Default Setting in customer setup
            chkHomeowner.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["ISshowHomeowner"] == DBNull.Value ? 0 : ds.Tables[0].Rows[0]["ISshowHomeowner"]);
            chkLocAddBlank.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsLocAddressBlank"] == DBNull.Value ? 0 : ds.Tables[0].Rows[0]["IsLocAddressBlank"]);
            //Default Setting in AP setup
            chkVendorSalesTax.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsSalesTaxAPBill"] == DBNull.Value ? 0 : ds.Tables[0].Rows[0]["IsSalesTaxAPBill"]);
            chkVendorUseTax.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsUseTaxAPBill"] == DBNull.Value ? 0 : ds.Tables[0].Rows[0]["IsUseTaxAPBill"]);
            //Contact Type
            ddlShowContactTypeinProjectScren.SelectedValue = Convert.ToString(ds.Tables[0].Rows[0]["ContactType"].ToString() == null ? "0" : ds.Tables[0].Rows[0]["ContactType"].ToString());

            if (ds.Tables[0].Rows[0]["TargetHPermission"].ToString() == "1")
                chkTargetHours.Checked = true;
            else
                chkTargetHours.Checked = false;
        }
    }
    private void GetBillingCode()
    {
        DataSet dsCode = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.Billingmodule = "";

        dsCode = new BL_BillCodes().GetActiveBillingCode(Session["config"].ToString());
        
        DataTable dt = new DataTable();
        dt.Columns.Add("ID");
        dt.Columns.Add("BillCode");

        DataRow dr = dt.NewRow();
        dr["ID"] = 0;
        dr["BillCode"] = "-Select a Billing Code-";
        dt.Rows.Add(dr);

        for (int i = 0; i < dsCode.Tables[0].Rows.Count; i++)
        {
            dr = dt.NewRow();
            dr["ID"] = dsCode.Tables[0].Rows[i]["id"];
            dr["BillCode"] = dsCode.Tables[0].Rows[i]["fDesc"];
            dt.Rows.Add(dr);
        }

        ddlDefaultBillingCode.DataSource = dt;
        ddlDefaultBillingCode.DataBind();

        BusinessEntity.User objProp_Users = new BusinessEntity.User();
        DataSet dscontrol = new DataSet();
        objProp_Users.ConnConfig = Session["config"].ToString();

        dscontrol = objBL_User.getControl(objProp_User);


        if (dscontrol.Tables[0].Rows.Count > 0)
        {
            if (Convert.ToString(dscontrol.Tables[0].Rows[0]["DefaultBillingCode"]) == "")
            {
                ddlDefaultBillingCode.SelectedValue = "0";
            }
            else
            {
                if (Convert.ToInt32(dscontrol.Tables[0].Rows[0]["DefaultBillingCode"].ToString()) == 0)
                {
                    ddlDefaultBillingCode.SelectedValue = "0";
                }
                else
                {
                    ddlDefaultBillingCode.SelectedValue = dscontrol.Tables[0].Rows[0]["DefaultBillingCode"].ToString();
                }
            }
            //if (Convert.ToString(dscontrol.Tables[0].Rows[0]["DefaultBillingCodeDesc"].ToString()) != "")
            //{
            //    ddlCode.SelectedItem.Text = Convert.ToString(dscontrol.Tables[0].Rows[0]["DefaultBillingCodeDesc"].ToString());
            //}

        }

    }

    private void ZonePermission()
    {
        objGeneral.ConnConfig = Session["config"].ToString();
        DataSet _dsCustom = objBL_General.getCustomFieldsControl(objGeneral);
        if (_dsCustom.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
            {
                if (_dr["Name"].ToString().Equals("Zone"))
                {
                    if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() == "1")
                    {
                        liZone.Visible = true;
                    }
                    else
                        liZone.Visible = false;
                }

            }
        }
    }
    #endregion
    #region Customer Type
    private void FillCustomers()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getCustomerType(objProp_User);
        RadGrid_customerType.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_customerType.DataSource = ds.Tables[0];

    }

    bool isGroupingCustomerType = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_customerType.MasterTableView.FilterExpression != "" ||
            (RadGrid_customerType.MasterTableView.GroupByExpressions.Count > 0 || isGroupingCustomerType) ||
            RadGrid_customerType.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_customerType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_customerType.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        FillCustomers();
    }
    protected void RadGrid_vendorType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_vendorType.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        FillVendors();
    }
    protected void lnkCustSave_Click(object sender, EventArgs e)
    {
        IsAddEdit = false;
        try
        {
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.CustomerType = txtCustomerType.Text;
            objProp_User.Remarks = txtremarks.Text;

            string msg = string.Empty;
            foreach (GridDataItem di in RadGrid_customerType.SelectedItems)
            {
                IsAddEdit = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                if (chkSelect.Checked == true)
                {
                    objBL_User.UpdateCustomerType(objProp_User);
                    msg = "Updated";
                }
            }
            if (!IsAddEdit)
            {
                objBL_User.AddCustomerType(objProp_User);
                msg = "Added";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseCustomerTypeWindow();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddCusttype", "noty({text: 'Customer Type " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            FillCustomers();
            RadGrid_customerType.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddCusttype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }

    }
    protected void lnkVendSave_Click(object sender, EventArgs e)
    {
        IsAddEdit = false;
        try
        {
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.CustomerType = txtVendorType.Text;
            objProp_User.Remarks = txtremarksvendor.Text;

            string msg = string.Empty;
            foreach (GridDataItem di in RadGrid_vendorType.SelectedItems)
            {
                IsAddEdit = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                if (chkSelect.Checked == true)
                {
                    objBL_User.UpdateVendorType(objProp_User);
                    msg = "updated";
                }
            }
            if (!IsAddEdit)
            {
                objBL_User.AddVendorType(objProp_User);
                msg = "added";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseVendorTypeWindow();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddCusttype", "noty({text: 'Vendor Type " + msg + " successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            FillVendors();
            RadGrid_vendorType.Rebind();
        }
        catch (Exception ex)
        {
            if (ex.Message.ToString() == "Vendor Type already exists, please use different type !")
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                str = "This Vendor Type already exists. Please change the Vendor Type name.";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddCusttype", "noty({text: '" + str + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
            else
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddCusttype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }

    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_customerType.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                string lblId = di["lblType"].Text.Trim();
                if (chkSelect.Checked == true)
                {
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.CustomerType = lblId;

                    objBL_User.DeleteCustomerType(objProp_User);
                    FillCustomers();
                    RadGrid_customerType.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddCusttype", "noty({text: 'Customer Type " + lblId + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningDelCust", "noty({text: 'Please select Customer Type to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelCust", "noty({text: '" + str + "', dismissQueue: true, type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkDeleteVendor_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_vendorType.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                string lblId = di["lblType"].Text.Trim();
                if (chkSelect.Checked == true)
                {
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.CustomerType = lblId;

                    objBL_User.DeleteVendorType(objProp_User);
                    FillVendors();
                    RadGrid_vendorType.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddCusttype", "noty({text: 'Vendor Type " + lblId + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningDelCust", "noty({text: 'Please select Vendor Type to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelCust", "noty({text: '" + str + "', dismissQueue: true, type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion
    #region Location Type
    private void FillLocation()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getlocationType(objProp_User);

        RadGrid_locationType.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_locationType.DataSource = ds.Tables[0];
    }
    bool isGroupinglocationType = false;
    public bool ShouldApplySortlocationType()
    {
        return RadGrid_locationType.MasterTableView.FilterExpression != "" ||
            (RadGrid_locationType.MasterTableView.GroupByExpressions.Count > 0 || isGroupinglocationType) ||
            RadGrid_locationType.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_locationType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_locationType.AllowCustomPaging = !ShouldApplySortlocationType();
        FillLocation();
    }

    //consultant
    protected void RadGrid_consultant_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        FillConsult();
    }

    private void FillConsult()
    {
        DataSet ds = new DataSet();
        objProp_Consult.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getConsultant(objProp_Consult);

        RadGrid_consultant.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_consultant.DataSource = ds.Tables[0];

    }

    protected void lnkCopyRolConsultant_Click(object sender, EventArgs e)
    {
        try
        {

            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.FirstName = txtNameCopy.Text;
            objProp_User.Address = txtAddressCopy.Text;
            objProp_User.City = txtCityCopy.Text;
            objProp_User.State = ddlStateCopy.SelectedValue;
            objProp_User.Zip = txtZipCopy.Text;
            objProp_User.Country = ddlCountryCopy.SelectedValue;
            objProp_User.MainContact = txtContactCopy.Text;
            objProp_User.Phone = txtPhoneCopy.Text;
            objProp_User.Cell = txtCellCopy.Text;
            objProp_User.Fax = txtFaxCopy.Text;
            objProp_User.Website = txtWebsiteCopy.Text;
            objProp_User.Email = txtEmailCopy.Text;
            objProp_User.Type = Convert.ToString(8);
            DataSet ds = new DataSet();
            ds = objBL_User.IsConsultNameExist(objProp_User);
            int count = int.Parse(ds.Tables[0].Rows[0]["name"].ToString());
            string msg = "";
            if (count > 0)
            {
                msg = "Name is already exist, please type another Name.";
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseConsultantCopyWindow();", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddLoctype", "noty({text: 'Name is already exist, please type another Name. " + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                return;
            }

            int rolId = objBL_User.AddRol(objProp_User);
            objProp_Consult.ConnConfig = Session["config"].ToString();
            objProp_Consult.RolID = rolId;
            objProp_Consult.RolName = txtNameCopy.Text;
            objProp_Consult.Count = 0;
            objProp_Consult.API = 1;
            objProp_Consult.Username = txtUsernameCopy.Text;
            objProp_Consult.Password = txtPasswordCopy.Text;
            objProp_Consult.IP = txtIpaddressCopy.Text;
            objBL_User.AddConsult(objProp_Consult);
            msg = "Added";
            RadGrid_consultant.Rebind();
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseConsultantCopyWindow();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddLoctype", "noty({text: 'Consultant " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {

        }
    }
    protected void lnkAddRolConsultant_Click(object sender, EventArgs e)
    {
        IsAddEdit = false;
        try
        {
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.FirstName = txtName.Text;
            objProp_User.Address = txtAddress.Text;
            objProp_User.City = txtCity.Text;
            objProp_User.State = ddlState.SelectedValue;
            objProp_User.Zip = txtZip.Text;
            objProp_User.Country = ddlCountry.SelectedValue;
            objProp_User.MainContact = txtContact.Text;
            objProp_User.Phone = txtPhone.Text;
            objProp_User.Cell = txtCell.Text;
            objProp_User.Fax = txtFax.Text;
            objProp_User.Website = txtWebsite.Text;
            objProp_User.Email = txtEmail.Text;
            objProp_User.Type = Convert.ToString(8);
            if (Session["contacttable"] != null)
            {
                objProp_User.ContactData = (DataTable)Session["contacttable"];
            }
            DataSet ds = new DataSet();
            ds = objBL_User.IsConsultNameExist(objProp_User);
            int count = int.Parse(ds.Tables[0].Rows[0]["name"].ToString());
            string msg = "";


            foreach (GridDataItem di in RadGrid_consultant.SelectedItems)
            {
                IsAddEdit = true;
                TableCell cell = di["chkSelect"];
                Label lblRolID = (Label)di.FindControl("lblRol");
                Label lblID = (Label)di.FindControl("lblID");
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                if (chkSelect.Checked == true)
                {
                    objProp_User.ID = int.Parse(lblRolID.Text);
                    objProp_Consult.ID = int.Parse(lblID.Text);
                    objProp_Consult.Username = txtUsername.Text;
                    objProp_Consult.Password = txtPassword.Text;
                    objProp_Consult.IP = txtIpaddress.Text;
                    objBL_User.UpdateConsultant(objProp_User, objProp_Consult);
                    msg = "Updated";
                }
            }

            if (!IsAddEdit)
            {
                if (count > 0)
                {
                    msg = "Name is already exist, please type another Name.";
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseConsultantWindow();", true);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddLoctype", "noty({text: 'Name is already exist, please type another Name. " + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
                else
                {
                    int rolId = objBL_User.AddRol(objProp_User);
                    objProp_Consult.ConnConfig = Session["config"].ToString();
                    objProp_Consult.RolID = rolId;
                    objProp_Consult.RolName = txtName.Text;
                    objProp_Consult.Count = 0;
                    objProp_Consult.API = 1;
                    objProp_Consult.Username = txtUsername.Text;
                    objProp_Consult.Password = txtPassword.Text;
                    objProp_Consult.IP = txtIpaddress.Text;
                    objBL_User.AddConsult(objProp_Consult);
                    msg = "Added";
                }

            }

            RadGrid_consultant.Rebind();
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseConsultantWindow();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddLoctype", "noty({text: 'Consultant " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {

        }
    }
    protected void lnkSaveLoc_Click(object sender, EventArgs e)
    {
        IsAddEdit = false;
        try
        {
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.CustomerType = txtloc.Text;
            objProp_User.Remarks = txtlocrem.Text;
            string msg = string.Empty;
            foreach (GridDataItem di in RadGrid_locationType.SelectedItems)
            {
                IsAddEdit = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                if (chkSelect.Checked == true)
                {
                    objBL_User.UpdateLocationType(objProp_User);
                    msg = "Updated";
                }
            }
            if (!IsAddEdit)
            {
                objBL_User.AddLocationType(objProp_User);
                msg = "Added";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseLocTypeWindow();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddLoctype", "noty({text: 'Location Type " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            FillLocation();
            RadGrid_locationType.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddLoctype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

        }
    }

    protected void lnkDelConsultant_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_consultant.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                //string lblId = di["lblID"].Text.Trim();
                Label lblID = (Label)di.FindControl("lblID");
                if (chkSelect.Checked == true)
                {
                    objProp_Consult.ConnConfig = Session["config"].ToString();
                    objProp_Consult.ID = int.Parse(lblID.Text);
                    objBL_User.DeleteConsultant(objProp_Consult);
                    FillConsult();
                    RadGrid_consultant.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddLoctype", "noty({text: ' Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningDelLoc", "noty({text: 'Please select Consultant to delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelLoc", "noty({text: '" + str + "', dismissQueue: true, type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    protected void lnkDelLoc_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_locationType.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                string lblId = di["lblType"].Text.Trim();
                if (chkSelect.Checked == true)
                {
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.CustomerType = lblId;

                    objBL_User.DeleteLocType(objProp_User);
                    FillLocation();
                    RadGrid_locationType.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddLoctype", "noty({text: 'Location Type " + lblId + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningDelLoc", "noty({text: 'Please select Location Type to delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelLoc", "noty({text: '" + str + "', dismissQueue: true, type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }
    #endregion
    #region Default
    protected void chkHomeowner_CheckedChanged(object sender, EventArgs e)
    {
        int ISshowHomeowner = 0;
        int IsLocAddressBlank = 0;
        if (chkHomeowner.Checked) { ISshowHomeowner = 1; }
        if (chkLocAddBlank.Checked) { IsLocAddressBlank = 1; }

        objBL_Customer.ISshowHomeowner(Session["config"].ToString(), ISshowHomeowner, IsLocAddressBlank);
    }
    protected void chkVendorSalesTax_CheckedChanged(object sender, EventArgs e)
    {
        int IsSalesTaxAPBill = 0;
        int IsUseTaxAPBill = 0;
        if (chkVendorSalesTax.Checked) { IsSalesTaxAPBill = 1; }
        if (chkVendorUseTax.Checked) { IsUseTaxAPBill = 1; }

        objBL_Vendor.IsSalesTaxAPBill(Session["config"].ToString(), IsSalesTaxAPBill, IsUseTaxAPBill);
    }
    protected void lnksaveSchdefault_Click(object sender, EventArgs e)
    {
        try
        {
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.REPtemplateID = Convert.ToInt16(chkMCP.Checked);
            objProp_User.Internet = Convert.ToInt16(chkInternet.Checked);
            objProp_User.JobCostLabor = Convert.ToInt16(ddlJobCostLabor.SelectedValue);
            objProp_User.TaskCode = Convert.ToBoolean(chkTask.Checked);
            if (txtBstart.Text != string.Empty)
                objProp_User.bstart = Convert.ToDateTime(txtBstart.Text);
            if (txtBend.Text != string.Empty)
                objProp_User.bend = Convert.ToDateTime(txtBend.Text);



            objBL_User.UpdateControl(objProp_User);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddQCype", "noty({text: 'Default updated Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddQCtype", "noty({text: '" + str + "',dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    #endregion

    #region Schedule Category 
    private void Fillcategory()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getcategoryAll(objProp_User);
        RadGrid_CategorySchedule.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_CategorySchedule.DataSource = ds.Tables[0];
    }
    bool isGroupingSchCategory = false;
    public bool ShouldApplySortSchCategory()
    {
        return RadGrid_CategorySchedule.MasterTableView.FilterExpression != "" ||
            (RadGrid_CategorySchedule.MasterTableView.GroupByExpressions.Count > 0 || isGroupingSchCategory) ||
            RadGrid_CategorySchedule.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_CategorySchedule_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_CategorySchedule.AllowCustomPaging = !ShouldApplySortSchCategory();
        Fillcategory();
    }
    protected void lnkDelCcat_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_CategorySchedule.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                string lblId = di["lblCType"].Text.Trim();
                if (chkSelect.Checked == true)
                {
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.Category = lblId;

                    objBL_User.DeleteCategory(objProp_User);
                    Fillcategory();
                    RadGrid_CategorySchedule.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddLoctype", "noty({text: 'Category " + lblId + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningDelLoc", "noty({text: 'Please select category to delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelCat", "noty({text: '" + str + "', dismissQueue: true, type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);

        }
    }
    protected void lnkCatSave_Click(object sender, EventArgs e)
    {
        IsAddEdit = false;
        try
        {
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.CustomerType = txtCat.Text;//.Replace("&", "$");
            objProp_User.Remarks = txtCatRem.Text;
            objProp_User.Logo = (byte[])Session["Catlogo"];
            objProp_User.Default = Convert.ToInt16(chkDefaultCat.Checked);
            if (chkChargeable.Checked.Equals(true))
            {
                objProp_User.Chargeable = true;
            }
            else
                objProp_User.Chargeable = false;
            objProp_User.ScheduleCategoryStatus = false;
            if (ddlCategoryStatus.SelectedValue == "1")
            {
                objProp_User.ScheduleCategoryStatus = true;
            }


            string msg = string.Empty;
            //foreach (GridDataItem di in RadGrid_CategorySchedule.SelectedItems)
            //{
            //    IsAddEdit = true;
            //    TableCell cell = di["chkSelect"];
            //    CheckBox chkSelect = (CheckBox)cell.Controls[0];
            //    if (chkSelect.Checked == true)
            //    {
            //        objBL_User.UpdateCategory(objProp_User);
            //        msg = "Updated";

            //    }
            //}

            if (hddCategoryAction.Value == "0")
            {
                objBL_User.AddCategory(objProp_User);
                msg = "Added";
            }
            else
            {
                objBL_User.UpdateCategory(objProp_User);
                msg = "Updated";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseCategoryScheduleWindow('CatgImage');", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddCattype", "noty({text: 'Category " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            Fillcategory();
            RadGrid_CategorySchedule.Rebind();
            // imgCatIcon.ImageUrl = "";
        }
        catch (Exception ex)
        {

            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddCattype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    protected void btnUploadCatIcon_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            lblfileLabel.InnerText = FileUpload1.FileName;
            System.Drawing.Image imgfile = System.Drawing.Image.FromStream(FileUpload1.PostedFile.InputStream);
            Session["Catlogo"] = null;
            Session["Catlogo"] = objGeneralFunctions.ResizeImage(imgfile, 32, 32);
            string img = "data:image/png;base64," + Convert.ToBase64String(objGeneralFunctions.ResizeImage(imgfile, 32, 32));
            imgCatIcon.ImageUrl = img;
        }

        string script = "function f(){$find(\"" + CategoryScheduleWindow.ClientID + "\").show();$('#dvCustomerSetup').hide(); $('#dvCategorySchedule').show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

    }

    #endregion

    #region Schedule Call Codes 
    protected void btnUpdateCallCodeOrder_Click(object sender, EventArgs e)
    {
        objGeneral.ConnConfig = Session["config"].ToString();
        objGeneral.dtDiagnostic = UpdateDiagnosticOrder();
        objBL_General.UpdateDiagnosticOrder(objGeneral);
        FillDiagnostic();
        RadGrid_CallCodes.Rebind();
    }


    private DataTable UpdateDiagnosticOrder()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Category", typeof(string));
        dt.Columns.Add("Type", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("OrderNo", typeof(int));

        foreach (GridDataItem di in RadGrid_CallCodes.Items)
        {
            Label lblCat = (Label)di.FindControl("lblCat");
            Label lblName = (Label)di.FindControl("lblCDescription");
            Label lblType = (Label)di.FindControl("lblTypeid");
            HiddenField txtRowLine = (HiddenField)di.FindControl("txtRowLine");
            HiddenField hdnOld = (HiddenField)di.FindControl("hdnOldIndex");
            HiddenField isSelected = (HiddenField)di.FindControl("isSelected");
            if (isSelected.Value == "1")
            {
                DataRow dr = dt.NewRow();
                dr["Category"] = lblCat.Text;
                dr["Type"] = lblType.Text;
                dr["fDesc"] = lblName.Text;
                dr["OrderNo"] = Convert.ToInt32(hdnOld.Value) - Convert.ToInt32(txtRowLine.Value);

                dt.Rows.Add(dr);
            }

        }
        return dt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    private void getDiagnosticCategory()
    {
        DataSet ds = new DataSet();
        objGeneral.ConnConfig = Session["config"].ToString();
        ds = objBL_General.getDiagnosticCategory(objGeneral);
        ddlCodeCategory.DataSource = ds.Tables[0];
        ddlCodeCategory.DataTextField = "category";
        ddlCodeCategory.DataValueField = "category";
        ddlCodeCategory.DataBind();

        ddlCodeCategory.Items.Add(new ListItem("Other", "Other"));
    }
    protected void ddlCodeCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCodeCategory.SelectedItem.Text == "Other")
        {
            txtCallCateg.Text = "";
            txtCallCateg.Visible = true;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenScript", "OpenCalCatgtextBox('Other');return false", true);
        }
        else
        {
            txtCallCateg.Visible = false;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenScript", "OpenCalCatgtextBox('null');return false", true);
        }
    }
    private void FillDiagnostic()
    {
        objGeneral.ConnConfig = Session["config"].ToString();

        DataSet ds = new DataSet();
        ds = objBL_General.getDiagnosticAll(objGeneral);
        RadGrid_CallCodes.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_CallCodes.DataSource = ds.Tables[0];
    }
    bool isGroupingCallCodes = false;
    public bool ShouldApplySortFilterCallCodes()
    {
        return RadGrid_CallCodes.MasterTableView.FilterExpression != "" ||
            (RadGrid_CallCodes.MasterTableView.GroupByExpressions.Count > 0 || isGroupingCallCodes) ||
            RadGrid_CallCodes.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_CallCodes_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_CallCodes.AllowCustomPaging = !ShouldApplySortFilterCallCodes();
        FillDiagnostic();
    }
    protected void lnkSaveCode_Click(object sender, EventArgs e)
    {
        var callCodesMode = hdnCallCodesMode.Value == "1";
        try
        {
            string msg = string.Empty;
            objGeneral.ConnConfig = Session["config"].ToString();

            if (ddlCodeCategory.SelectedItem.Text == "Other")
            {
                objGeneral.Category = txtCallCateg.Text;
            }
            else
            {
                objGeneral.Category = ddlCodeCategory.SelectedValue;
            }
            objGeneral.DiagnosticType = Convert.ToInt32(ddlCodeType.SelectedValue);
            objGeneral.Remarks = txtCodeDesc.Text;
            if (callCodesMode)
            {
                foreach (GridDataItem di in RadGrid_CallCodes.SelectedItems)
                {
                    TableCell cell = di["chkSelect"];
                    CheckBox chkSelect = (CheckBox)cell.Controls[0];

                    if (chkSelect.Checked == true)
                    {
                        Label lblCat = (Label)di.FindControl("lblCat");
                        Label lblType = (Label)di.FindControl("lblTypeid");
                        objGeneral.DiagnosticCategoryOld = lblCat.Text;
                        objGeneral.DiagnosticTypeOld = Convert.ToInt32(lblType.Text);

                        objBL_General.UpdateDiagnostic(objGeneral);
                        msg = "Updated";
                    }
                }
            }
            else
            {
                objBL_General.InsertDiagnostic(objGeneral);
                msg = "Added";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseCallCodesWindow();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddDeparmenttype", "noty({text: 'Code " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            FillDiagnostic();
            RadGrid_CallCodes.Rebind();
            getDiagnosticCategory();
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddDepttype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    protected void lnkDelCallCodes_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_CallCodes.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                Label lblCat = (Label)di.FindControl("lblCat");
                Label lblName = (Label)di.FindControl("lblCDescription");
                Label lblType = (Label)di.FindControl("lblTypeid");

                if (chkSelect.Checked == true)
                {
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.Category = lblCat.Text;
                    objProp_User.DiagnosticType = Convert.ToInt32(lblType.Text);
                    objProp_User.Remarks = lblName.Text;

                    objBL_User.DeleteDiagnostic(objProp_User);
                    FillDiagnostic();
                    RadGrid_CallCodes.Rebind();
                    getDiagnosticCategory();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddCallCodes", "noty({text: 'Call Codes " + lblCat.Text + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningCallCodes", "noty({text: 'Please select Call Codes to delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelCallCode", "noty({text: '" + str + "', dismissQueue: true, type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);

        }
    }
    #endregion

    #region Schedule Quick Codes
    private void FillQuickCodes()
    {
        objGeneral.ConnConfig = Session["config"].ToString();

        DataSet ds = new DataSet();
        ds = objBL_General.getCodesAll(objGeneral);
        RadGrid_QuickCodes.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_QuickCodes.DataSource = ds.Tables[0];
    }
    bool isGroupingQuickCodes = false;
    public bool ShouldApplySortFilterQuickCodes()
    {
        return RadGrid_QuickCodes.MasterTableView.FilterExpression != "" ||
            (RadGrid_QuickCodes.MasterTableView.GroupByExpressions.Count > 0 || isGroupingQuickCodes) ||
            RadGrid_QuickCodes.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_QuickCodes_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_QuickCodes.AllowCustomPaging = !ShouldApplySortFilterQuickCodes();
        FillQuickCodes();
    }
    protected void lnkSaveQC_Click(object sender, EventArgs e)
    {
        //IsAddEdit = false;
        IsAddEdit = hdnQCodeMode.Value == "1";
        try
        {
            objGeneral.ConnConfig = Session["config"].ToString();
            objGeneral.Code = txtQuickCode.Text;
            objGeneral.CodeDesc = txtQuickCodeText.Text;
            string msg = string.Empty;
            if (IsAddEdit)
            {
                foreach (GridDataItem di in RadGrid_QuickCodes.SelectedItems)
                {
                    TableCell cell = di["chkSelect"];
                    CheckBox chkSelect = (CheckBox)cell.Controls[0];
                    if (chkSelect.Checked == true)
                    {
                        objBL_General.UpdateQuickCodes(objGeneral);
                        msg = "Updated";
                    }
                }
            }
            else
            {
                objBL_General.InsertQuickCodes(objGeneral);
                msg = "Added";
            }

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseQuickCodesWindow();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddQCype", "noty({text: 'Quick Code " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            FillQuickCodes();
            RadGrid_QuickCodes.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddQCtype", "noty({text: '" + str + "',dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }


    }
    protected void lnkDelQuickCodes_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_QuickCodes.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                string lblCode = di["lblCode"].Text.Trim();
                string lblText = di["lblText"].Text.Trim();
                if (chkSelect.Checked == true)
                {
                    objGeneral.ConnConfig = Session["config"].ToString();
                    objGeneral.Code = lblCode;
                    objGeneral.CodeDesc = lblText;
                    objBL_General.DeleteQuickCodes(objGeneral);
                    FillQuickCodes();
                    RadGrid_QuickCodes.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddQuickCode", "noty({text: 'Quick Code " + lblCode + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningQuickCode", "noty({text: 'Please select Quick Code to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelQuickCode", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }
    #endregion

    #region Schedule Zone
    private void FillZone()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getZone(objProp_User);
        RadGrid_Zone.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_Zone.DataSource = ds.Tables[0];
    }
    bool isGroupingZone = false;
    public bool ShouldApplySortFilterZone()
    {
        return RadGrid_Zone.MasterTableView.FilterExpression != "" ||
            (RadGrid_Zone.MasterTableView.GroupByExpressions.Count > 0 || isGroupingZone) ||
            RadGrid_Zone.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_Zone_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_Zone.AllowCustomPaging = !ShouldApplySortFilterZone();
        FillZone();
    }
    protected void lnkZoneSave_Click(object sender, EventArgs e)
    {
        IsAddEdit = false;
        try
        {
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.Name = txtZoneName.Text;
            objProp_User.fDesc = txtZoneDesc.Text;
            objProp_User.Bonus = string.IsNullOrEmpty(txtMechanicBonus.Text) ? 0 : Convert.ToDouble(txtMechanicBonus.Text);
            objProp_User.Price1 = string.IsNullOrEmpty(txtPrice.Text) ? 0 : Convert.ToDouble(txtPrice.Text);
            objProp_User.Count = string.IsNullOrEmpty(txtZoneCount.Text) ? 0 : Convert.ToInt32(txtZoneCount.Text);
            if (chkTaxable.Checked.Equals(true))
            {
                objProp_User.Tax = 1;
            }
            else
                objProp_User.Tax = 0;
            objProp_User.Remarks = txtZoneRemarks.Text;
            string msg = string.Empty;
            foreach (GridDataItem di in RadGrid_Zone.SelectedItems)
            {
                IsAddEdit = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                Label lblID = (Label)di.FindControl("lblId");
                if (chkSelect.Checked == true)
                {
                    objProp_User.ZoneID = Convert.ToInt32(lblID.Text);
                    objBL_User.UpdateZone(objProp_User);
                    msg = "Updated";

                }
            }
            if (!IsAddEdit)
            {
                objBL_User.AddZone(objProp_User);
                msg = "Added";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseZoneWindow();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddQCype", "noty({text: 'Zone " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            FillZone();
            RadGrid_Zone.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddQCtype", "noty({text: '" + str + "',dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    protected void lnkZoneDelete_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_Zone.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                Label lblID = (Label)di.FindControl("lblId");
                Label lblName = (Label)di.FindControl("lblName");
                if (chkSelect.Checked == true)
                {
                    objProp_User.ZoneID = Convert.ToInt32(lblID.Text);
                    objProp_User.Name = lblName.Text;
                    objBL_User.DeleteZone(objProp_User);
                    FillZone();
                    RadGrid_Zone.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddQuickCode", "noty({text: 'Zone " + lblName.Text + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningQuickCode", "noty({text: 'Please select Zone to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelQuickCode", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }
    #endregion

    #region Schedule Wage Schedule
    private void FillWage()
    {
        DataSet ds = new DataSet();
        _objWage.ConnConfig = Session["config"].ToString();
        ds = objBL_Wage.getWage(_objWage);
        RadGrid_WageSchedule.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_WageSchedule.DataSource = ds.Tables[0];
    }
    bool isGroupingWageSchedule = false;
    public bool ShouldApplySortFilterWageSchedule()
    {
        return RadGrid_WageSchedule.MasterTableView.FilterExpression != "" ||
            (RadGrid_WageSchedule.MasterTableView.GroupByExpressions.Count > 0 || isGroupingWageSchedule) ||
            RadGrid_WageSchedule.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_WageSchedule_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_WageSchedule.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Category_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_WageSchedule.MasterTableView.OwnerGrid.Columns)
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

            Session["Category_Filters"] = filters;
            //Session["Vendor_VirtulItemCount"] = RadGrid_WageDeduction.VirtualItemCount;
        }
        else
        {
            Session["Category_FilterExpression"] = null;
            Session["Category_Filters"] = null;
            //Session["Vendor_VirtulItemCount"] = null;
        }
        #endregion  
        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_WageSchedule);
        //RowSelect();


    }
    protected void RadGrid_WageSchedule_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_WageSchedule.AllowCustomPaging = !ShouldApplySortFilterWageSchedule();
        #region Set the Grid Filters
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["Category_FilterExpression"] != null && Convert.ToString(Session["Category_FilterExpression"]) != "" && Session["Category_Filters"] != null)
                {
                    RadGrid_WageSchedule.MasterTableView.FilterExpression = Convert.ToString(Session["Category_FilterExpression"]);
                    var filtersGet = Session["Category_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_WageSchedule.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
            }
            else
            {
                Session["Category_FilterExpression"] = null;
                Session["Category_Filters"] = null;
                //Session["Vendor_VirtulItemCount"] = null;
            }
            //if (Request.QueryString["AddVendor"] != null)
            //{
            //    if (Convert.ToString(Request.QueryString["AddVendor"]) == "Y")
            //    {
            //        if (check == true)
            //        {
            //            lnkChk.Checked = true;
            //        }
            //        else
            //        {
            //            lnkChk.Checked = false;
            //        }
            //    }
            //}
        }

        #endregion
        FillWage();
    }

    protected void lnkAddWage_Click(object sender, EventArgs e)
    {
        //ClearAll();
        ResetWageForm();
        pnlWageAddEdit.Visible = true;
        pnlWageGv.Visible = false;

        lnkWageSave.Visible = true;
        lnkWageClose.Visible = true;
        lnkAddWage.Visible = false;
        lnkEditWage.Visible = false;
        lnkDeleteWage.Visible = false;
        ViewState["edit"] = "0";
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "Materialize.updateTextFields();", true);
    }
    protected void lnkEditWage_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_WageSchedule.SelectedItems)
        {
            TableCell cell = di["chkSelect"];
            //CheckBox chkSelect = (CheckBox)di.FindControl("chkSelect");
            CheckBox chkSelect = (CheckBox)cell.Controls[0];
            Label lblWageid = (Label)di.FindControl("lblWageId");
            Label lblWageStatus = (Label)di.FindControl("lblWageStatus");
            string lblFdesc = di["lblWageFdesc"].Text.Trim();
            //string lblrem = di["lblrem"].Text.Trim();
            if (lblWageStatus.Text == "Active")
            {
                if (chkSelect.Checked == true)
                {
                    pnlWageGv.Visible = false;
                    ViewState["edit"] = 1;
                    pnlWageAddEdit.Visible = true;
                    lnkWageSave.Visible = true;
                    lnkWageClose.Visible = true;
                    lnkAddWage.Visible = false;
                    lnkEditWage.Visible = false;
                    lnkDeleteWage.Visible = false;
                    ResetWageForm();

                    _objWage.ConnConfig = Session["config"].ToString();
                    _objWage.ID = Convert.ToInt32(lblWageid.Text);
                    hdnWageID.Value = lblWageid.Text;
                    DataSet _dsWage = objBL_Wage.GetWageByID(_objWage);
                    DataRow _dr = _dsWage.Tables[0].Rows[0];
                    SetWage(_dr);
                    pnlWageAddEdit.Visible = true;
                    ViewState["edit"] = "1";
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "Materialize.updateTextFields();", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "error1212", "noty({text: 'Wage is InActive can not edit!', type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default', closable : true});", true);
            }

        }

    }
    protected void lnkDeleteWage_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_WageSchedule.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                Label lblWageId = (Label)di.FindControl("lblWageId");
                Label lblWageFdesc = (Label)di.FindControl("lblWageFdesc");
                if (chkSelect.Checked == true)
                {
                    _objWage.ConnConfig = Session["config"].ToString();
                    _objWage.ID = Convert.ToInt32(lblWageId.Text);
                    objBL_Wage.DeleteWageByID(_objWage);

                    FillWage();
                    RadGrid_WageSchedule.Rebind();

                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddWage", "noty({text: 'Wage " + lblWageFdesc.Text + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningWage", "noty({text: 'Please select Wage to delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDeldep", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkWageSave_Click(object sender, EventArgs e)
    {
        try
        {
            _objWage.ConnConfig = Session["config"].ToString();
            _objWage.Name = txtDesc.Text;
            _objWage.GL = Convert.ToInt32(hdnGLAcct.Value);
            _objWage.MileageGL = Convert.ToInt32(hdnMilegAcct.Value);
            _objWage.ReimGL = Convert.ToInt32(hdnReimbAcct.Value);
            _objWage.ZoneGL = Convert.ToInt32(hdnZoneAcct.Value);
            _objWage.Status = Convert.ToInt16(ddlWageStatus.SelectedValue);
            _objWage.Globe = Convert.ToInt16(ddlGlobal.SelectedValue);
            _objWage.Reg = Convert.ToDouble(txtRegularRate.Text);
            _objWage.OT1 = Convert.ToDouble(txtOvertimeRate.Text);
            _objWage.NT = Convert.ToDouble(txtTime.Text);
            _objWage.OT2 = Convert.ToDouble(txtDoubleTime.Text);
            _objWage.TT = Convert.ToDouble(txtTravelTime.Text);
            _objWage.CReg = Convert.ToDouble(txtCReg.Text);
            _objWage.COT = Convert.ToDouble(txtCOT.Text);
            _objWage.CNT = Convert.ToDouble(txtCNT.Text);
            _objWage.CDT = Convert.ToDouble(txtCDT.Text);
            _objWage.CTT = Convert.ToDouble(txtCTT.Text);

            _objWage.RegGL = Convert.ToInt32(hdnRegGL.Value);
            _objWage.OTGL = Convert.ToInt32(hdnOTGL.Value);
            _objWage.NTGL = Convert.ToInt32(hdnNTGL.Value);
            _objWage.DTGL = Convert.ToInt32(hdnDTGL.Value);
            _objWage.TTGL = Convert.ToInt32(hdnTTGL.Value);


            if (chkField.Checked.Equals(true))
            {
                _objWage.Field = 1;
            }
            else
            {
                _objWage.Field = 0;
            }
            if (chkFIT.Checked.Equals(true))
            {
                _objWage.FIT = 1;
            }
            else
            {
                _objWage.FIT = 0;
            }
            if (chkFICA.Checked.Equals(true))
            {
                _objWage.FICA = 1;
            }
            else
            {
                _objWage.FICA = 0;
            }
            if (chkMEDI.Checked.Equals(true))
            {
                _objWage.MEDI = 1;
            }
            else
            {
                _objWage.MEDI = 0;
            }
            if (chkFUTA.Checked.Equals(true))
            {
                _objWage.FUTA = 1;
            }
            else
            {
                _objWage.FUTA = 0;
            }
            if (chkSIT.Checked.Equals(true))
            {
                _objWage.SIT = 1;
            }
            else
            {
                _objWage.SIT = 0;
            }
            if (chkVacation.Checked.Equals(true))
            {
                _objWage.Vac = 1;
            }
            else
            {
                _objWage.Vac = 0;
            }
            if (chkWorkComp.Checked.Equals(true))
            {
                _objWage.WC = 1;
            }
            else
            {
                _objWage.WC = 0;
            }
            if (chkUnion.Checked.Equals(true))
            {
                _objWage.Uni = 1;
            }
            else
            {
                _objWage.Uni = 0;
            }
            if (chkSick.Checked.Equals(true))
            {
                _objWage.Sick = 1;
            }
            else
            {
                _objWage.Sick = 0;
            }
            _objWage.Remarks = txtRemark.Text;
            string msg = "Added";
            if (ViewState["edit"].ToString() == "0")
            {
                objBL_Wage.AddWage(_objWage);
                pnlWageAddEdit.Visible = false;
                pnlWageGv.Visible = true;

                lnkWageSave.Visible = false;
                lnkWageClose.Visible = false;
                lnkAddWage.Visible = true;
                lnkEditWage.Visible = true;
                lnkDeleteWage.Visible = true;

                //this.programmaticModalPopup.Hide();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddDeparmenttype", "noty({text: 'Wage " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                FillWage();
                RadGrid_WageSchedule.Rebind();
            }
            else if (ViewState["edit"].ToString() == "1")
            {
                if (hdnFlag.Value == "1" && ddlWageStatus.SelectedValue == "1")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "error1212", "noty({text: 'Wage is associated can not Inactive', type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default', closable : true});", true);
                }
                else
                {
                    msg = "Updated";
                    _objWage.ID = Convert.ToInt32(hdnWageID.Value);

                    objBL_Wage.UpdateWage(_objWage);
                    pnlWageAddEdit.Visible = false;
                    pnlWageGv.Visible = true;

                    lnkWageSave.Visible = false;
                    lnkWageClose.Visible = false;
                    lnkAddWage.Visible = true;
                    lnkEditWage.Visible = true;
                    lnkDeleteWage.Visible = true;

                    //this.programmaticModalPopup.Hide();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddDeparmenttype", "noty({text: 'Wage " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    FillWage();
                    RadGrid_WageSchedule.Rebind();
                }

            }
        }
        catch (Exception ex)
        {
            string type = "error";
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            if (str.Contains("Wage Category already exists, please use different name"))
            {
                type = "warning";
            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddDepttype", "noty({text: '" + str + "', dismissQueue: true,  type : '" + type + "', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void lnkWageClose_Click(object sender, EventArgs e)
    {
        pnlWageAddEdit.Visible = false;
        pnlWageGv.Visible = true;

        lnkAddWage.Visible = true;
        lnkEditWage.Visible = true;
        lnkDeleteWage.Visible = true;

        lnkWageClose.Visible = false;
        lnkWageSave.Visible = false;
        ResetWageForm();
        FillWage();
    }
    private void SetWage(DataRow _dr)
    {
        if (Convert.ToInt16(_dr["FIT"]).Equals(1))
        {
            chkFIT.Checked = true;
        }
        else
            chkFIT.Checked = false;

        if (Convert.ToInt16(_dr["FICA"]).Equals(1))
        {
            chkFICA.Checked = true;
        }
        else
            chkFICA.Checked = false;

        if (Convert.ToInt16(_dr["MEDI"]).Equals(1))
        {
            chkMEDI.Checked = true;
        }
        else
            chkMEDI.Checked = false;

        if (Convert.ToInt16(_dr["FUTA"]).Equals(1))
        {
            chkFUTA.Checked = true;
        }
        else
            chkFUTA.Checked = false;

        if (Convert.ToInt16(_dr["SIT"]).Equals(1))
        {
            chkSIT.Checked = true;
        }
        else
            chkSIT.Checked = false;

        if (Convert.ToInt16(_dr["Vac"]).Equals(1))
        {
            chkVacation.Checked = true;
        }
        else
            chkVacation.Checked = false;

        if (Convert.ToInt16(_dr["WC"]).Equals(1))
        {
            chkWorkComp.Checked = true;
        }
        else
            chkWorkComp.Checked = false;

        if (Convert.ToInt16(_dr["Uni"]).Equals(1))
        {
            chkUnion.Checked = true;
        }
        else
            chkUnion.Checked = false;
        if (Convert.ToInt16(_dr["SICK"]).Equals(1))
        {
            chkSick.Checked = true;
        }
        else
            chkSick.Checked = false;
        txtDesc.Text = _dr["fDesc"].ToString();
        txtRemark.Text = _dr["Remarks"].ToString();
        txtRegularRate.Text = String.Format("{0:0.00}", _dr["Reg"]);
        txtOvertimeRate.Text = String.Format("{0:0.00}", _dr["OT1"]);
        txtTime.Text = String.Format("{0:0.00}", _dr["NT"]);
        txtDoubleTime.Text = String.Format("{0:0.00}", _dr["OT2"]);
        txtTravelTime.Text = String.Format("{0:0.00}", _dr["TT"]);
        txtCReg.Text = String.Format("{0:0.00}", _dr["CReg"]);
        txtCOT.Text = String.Format("{0:0.00}", _dr["COT"]);
        txtCNT.Text = String.Format("{0:0.00}", _dr["CNT"]);
        txtCDT.Text = String.Format("{0:0.00}", _dr["CDT"]);
        txtCTT.Text = String.Format("{0:0.00}", _dr["CTT"]);
        ddlGlobal.SelectedValue = _dr["Globe"].ToString();
        ddlWageStatus.SelectedValue = _dr["Status"].ToString();

        hdnGLAcct.Value = _dr["GL"].ToString();
        hdnMilegAcct.Value = _dr["MileageGL"].ToString();
        hdnReimbAcct.Value = _dr["ReimburseGL"].ToString();
        hdnZoneAcct.Value = _dr["ZoneGL"].ToString();
        txtGLAcct.Text = _dr["GLName"].ToString();
        txtMileageAcct.Text = _dr["MileageGLName"].ToString();
        txtReimbAcct.Text = _dr["ReimGLName"].ToString();
        txtZoneAcct.Text = _dr["ZoneGLName"].ToString();

        hdnRegGL.Value = _dr["RegGL"].ToString();
        hdnOTGL.Value = _dr["OTGL"].ToString();
        hdnNTGL.Value = _dr["NTGL"].ToString();
        hdnDTGL.Value = _dr["DTGL"].ToString();
        hdnTTGL.Value = _dr["TTGL"].ToString();

        txtRegGL.Text = _dr["RegGLName"].ToString();
        txtOTGL.Text = _dr["OTGLName"].ToString();
        txtNTGL.Text = _dr["NTGLName"].ToString();
        txtDTGL.Text = _dr["DTGLName"].ToString();
        txtTTGL.Text = _dr["TTGLName"].ToString();
        hdnFlag.Value = _dr["Flag"].ToString();
    }
    private void ResetWageForm()
    {
        chkFIT.Checked = true;
        chkFICA.Checked = true;
        chkMEDI.Checked = true;
        chkFUTA.Checked = true;
        chkSIT.Checked = true;
        chkVacation.Checked = true;
        chkWorkComp.Checked = true;
        chkUnion.Checked = true;
        chkSick.Checked = true;
        txtRegularRate.Text = "0.00";
        txtOvertimeRate.Text = "0.00";
        txtTime.Text = "0.00";
        txtDoubleTime.Text = "0.00";
        txtTravelTime.Text = "0.00";
        txtCReg.Text = "0.00";
        txtCOT.Text = "0.00";
        txtCNT.Text = "0.00";
        txtCDT.Text = "0.00";
        txtCTT.Text = "0.00";
        ddlGlobal.SelectedValue = "1";
        chkField.Checked = true;

        txtDesc.Text = string.Empty;
        txtGLAcct.Text = string.Empty;
        txtMileageAcct.Text = string.Empty;
        txtReimbAcct.Text = string.Empty;
        txtZoneAcct.Text = string.Empty;
        hdnGLAcct.Value = "0";
        hdnMilegAcct.Value = "0";
        hdnReimbAcct.Value = "0";
        hdnZoneAcct.Value = "0";

        ddlWageStatus.SelectedValue = "0";

        txtRegGL.Text = string.Empty;
        txtOTGL.Text = string.Empty;
        txtNTGL.Text = string.Empty;
        txtDTGL.Text = string.Empty;
        txtTTGL.Text = string.Empty;
        txtRemark.Text = string.Empty;
        hdnRegGL.Value = "0";
        hdnOTGL.Value = "0";
        hdnNTGL.Value = "0";
        hdnDTGL.Value = "0";
        hdnTTGL.Value = "0";
    }

    #endregion

    #region Equipment Category
    private void FillEquipCategory()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getEquipmentCategory(objProp_User);
        RadGrid_EquipCategory.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_EquipCategory.DataSource = ds.Tables[0];
        lblEquipCategoryHeader.InnerText = "Category";
        lblEditCategory.InnerText = "Category";
        lnkEditCategory.Text = "Category";
        if (ds.Tables[0].Rows.Count > 0)
        {
            RadGrid_EquipCategory.Columns[1].HeaderText = "Category";
        }
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            RadGrid_EquipCategory.Columns[1].HeaderText = ds.Tables[0].Rows[0]["Label"].ToString();
            lblEquipCategoryHeader.InnerText = ds.Tables[0].Rows[0]["Label"].ToString();
            lblEditCategory.InnerText = ds.Tables[0].Rows[0]["Label"].ToString();
            lnkEditCategory.Text = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }
    bool isGroupingEquipCategory = false;
    public bool ShouldApplySortEquipCategory()
    {
        return RadGrid_EquipCategory.MasterTableView.FilterExpression != "" ||
            (RadGrid_EquipCategory.MasterTableView.GroupByExpressions.Count > 0 || isGroupingEquipCategory) ||
            RadGrid_EquipCategory.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_EquipCategory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_EquipCategory.AllowCustomPaging = !ShouldApplySortEquipCategory();
        FillEquipCategory();
    }
    protected void lnkEcatAddSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtEcat.Text.Trim() != string.Empty)
            {
                objProp_User.ConnConfig = Session["config"].ToString();
                objProp_User.EquipType = txtEcat.Text.Trim();
                objBL_User.AddEquipCateg(objProp_User);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseEquipCategoryAddWindow();", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccEqupCate", "noty({text: 'Equipment Category Added Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                FillEquipCategory();
                RadGrid_EquipCategory.Rebind();
                txtEcat.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            string type = "error";
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            if (str.Contains("Equipment category already exists, please use different name"))
            {
                type = "warning";
            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddEqupcate", "noty({text: '" + str + "', dismissQueue: true,  type : '" + type + "', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    protected void lnkUpdateCategoryHeader_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtEditCategory.Text != "")
            {
                objCustomer.ConnConfig = Session["config"].ToString();
                objCustomer.HeaderServices = txtEditCategory.Text;
                objBL_Customer.UpdateEquipmentCategoryHeader(objCustomer);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseEquipCategorylabelWindow();", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccEqupCate", "noty({text: 'Header Updated Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                FillEquipCategory();
                RadGrid_EquipCategory.Rebind();
                lblEquipCategoryHeader.InnerText = txtEditCategory.Text;
                lnkEditCategory.Text = txtEditCategory.Text;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddEqupcate", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    protected void lnkDeleteEquipCat_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_EquipCategory.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                Label lblId = (Label)di.FindControl("lblId");

                if (chkSelect.Checked == true)
                {
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.Category = lblId.Text;

                    objBL_User.DeleteEquipCateg(objProp_User);
                    FillEquipCategory();
                    RadGrid_EquipCategory.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccEquipCategory", "noty({text: 'Equipment Category " + lblId.Text + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningDelEquipCategory", "noty({text: 'Please select Equipment Category to delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelEqtyp", "noty({text: '" + str + "',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    #endregion

    #region Equipment Type
    private void FillEquipType()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getEquiptype(objProp_User);
        RadGrid_EquipType.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_EquipType.DataSource = ds.Tables[0];
        lblEquipTypeHeader.InnerText = "Type";
        lblEditType.InnerText = "Type";
        lnkEditType.Text = "Type";
        if (ds.Tables[0].Rows.Count > 0)
        {
            RadGrid_EquipType.Columns[1].HeaderText = "Type";
        }
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            lblEquipTypeHeader.InnerText = ds.Tables[0].Rows[0]["Label"].ToString();
            lblEditType.InnerText = ds.Tables[0].Rows[0]["Label"].ToString();
            lnkEditType.Text = ds.Tables[0].Rows[0]["Label"].ToString();
            RadGrid_EquipType.Columns[1].HeaderText = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }
    bool isGroupingEquipType = false;
    public bool ShouldApplySortEquipType()
    {
        return RadGrid_EquipType.MasterTableView.FilterExpression != "" ||
            (RadGrid_EquipType.MasterTableView.GroupByExpressions.Count > 0 || isGroupingEquipType) ||
            RadGrid_EquipType.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_EquipType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_EquipType.AllowCustomPaging = !ShouldApplySortEquipType();
        FillEquipType();
    }
    protected void lnkEquipTypeSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtEquip.Text.Trim() != string.Empty)
            {
                objProp_User.ConnConfig = Session["config"].ToString();
                objProp_User.EquipType = txtEquip.Text;
                objBL_User.AddEquipType(objProp_User);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseEquipTypeWindow('Add');", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccEqupCate", "noty({text: 'Equipment Added Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                FillEquipType();
                RadGrid_EquipType.Rebind();
                txtEquip.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            string type = "error";
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            if (str.Contains("Equipment Type already exists, please use different equipment"))
            {
                type = "warning";
            }

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddEquptype", "noty({text: '" + str + "', dismissQueue: true,  type : '" + type + "', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    protected void lnkUpdateTypeHeader_Click(object sender, EventArgs e)
    {
        if (txtEditType.Text != "")
        {
            objCustomer.ConnConfig = Session["config"].ToString();
            objCustomer.HeaderServices = txtEditType.Text;
            objBL_Customer.UpdateEquipmentTypeHeader(objCustomer);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseEquipTypeWindow('Header');", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccEquipType", "noty({text: 'Header Updated Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            FillEquipType();
            RadGrid_EquipType.Rebind();
            lblEquipTypeHeader.InnerText = txtEditType.Text;
            lnkEditType.Text = txtEditType.Text;

        }
    }
    protected void lnkDeleteEquip_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_EquipType.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                Label lblId = (Label)di.FindControl("lblId");
                if (chkSelect.Checked == true)
                {
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.EquipType = lblId.Text;
                    objBL_User.DeleteEquiptype(objProp_User);
                    FillEquipType();
                    RadGrid_EquipType.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccEquipType", "noty({text: 'Equipment " + lblId.Text + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningDelEquipType", "noty({text: 'Please select Equipment to delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelEqtyp", "noty({text: '" + str + "',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #endregion

    #region Equipment Building

    private void FillEquipBuilding()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getBuildingElev(objProp_User);
        RadGrid_EquipBuilding.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_EquipBuilding.DataSource = ds.Tables[0];
        lblEquipBuildingHeader.InnerText = "Building";
        lblEditBuilding.InnerText = "Building";
        lnkEditBuilding.Text = "Building";
        if (ds.Tables[0].Rows.Count > 0)
        {
            RadGrid_EquipBuilding.Columns[1].HeaderText = "Building";
        }
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            lblEquipBuildingHeader.InnerText = ds.Tables[0].Rows[0]["Label"].ToString();
            lblEditBuilding.InnerText = ds.Tables[0].Rows[0]["Label"].ToString();
            lnkEditBuilding.Text = ds.Tables[0].Rows[0]["Label"].ToString();
            RadGrid_EquipBuilding.Columns[1].HeaderText = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }
    bool isGroupingEquipBuilding = false;
    public bool ShouldApplySortEquipBuilding()
    {
        return RadGrid_EquipBuilding.MasterTableView.FilterExpression != "" ||
            (RadGrid_EquipBuilding.MasterTableView.GroupByExpressions.Count > 0 || isGroupingEquipBuilding) ||
            RadGrid_EquipBuilding.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_EquipBuilding_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_EquipBuilding.AllowCustomPaging = !ShouldApplySortEquipBuilding();
        FillEquipBuilding();
    }

    protected void lnkEquipBuildSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtBuilding.Text.Trim() != string.Empty)
            {
                objProp_User.ConnConfig = Session["config"].ToString();
                objProp_User.building = txtBuilding.Text;
                objBL_User.AddEquipBuilding(objProp_User);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseEquipBuildingWindow('Add');", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccEquptype", "noty({text: 'Equipment Added Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                FillEquipBuilding();
                RadGrid_EquipBuilding.Rebind();
                txtBuilding.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string type = "error";
            if (str.Contains("Equipment Building already exists, please use different equipment"))
            {
                type = "warning";

            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddEquptype", "noty({text: '" + str + "', dismissQueue: true,  type : '" + type + "', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    protected void lnkUpdateBuildingHeader_Click(object sender, EventArgs e)
    {
        if (txtEditBuilding.Text != "")
        {
            objCustomer.ConnConfig = Session["config"].ToString();
            objCustomer.HeaderServices = txtEditBuilding.Text;
            objBL_Customer.UpdateEquipmentBuildingHeader(objCustomer);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseEquipBuildingWindow('Header');", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccEquipBuilding", "noty({text: 'Header Updated Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            FillEquipBuilding();
            RadGrid_EquipBuilding.Rebind();
            lblEquipBuildingHeader.InnerText = txtEditBuilding.Text;
            lnkEditBuilding.Text = txtEditBuilding.Text;
        }
    }
    protected void lnkDeleteEquipBuild_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_EquipBuilding.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                Label lblId = (Label)di.FindControl("lblId");
                if (chkSelect.Checked == true)
                {
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.building = lblId.Text;

                    objBL_User.DeleteEquipBuilding(objProp_User);
                    FillEquipBuilding();
                    RadGrid_EquipBuilding.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccEquipBuilding", "noty({text: 'Equipment " + lblId.Text + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningDelEquipBuilding", "noty({text: 'Please select Equipment to delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelEquipBuilding", "noty({text: '" + str + "',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }
    #endregion

    #region MCP Template
    protected void FillMCPTemplate()
    {
        objCustomer.ConnConfig = Session["config"].ToString();

        DataSet ds = new DataSet();
        ds = objBL_Customer.getRepTemplate(objCustomer);
        RadGrid_MCPTemplate.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_MCPTemplate.DataSource = ds.Tables[0];
    }
    bool isGroupingMCPTemplate = false;
    public bool ShouldApplySortMCPTemplate()
    {
        return RadGrid_MCPTemplate.MasterTableView.FilterExpression != "" ||
            (RadGrid_MCPTemplate.MasterTableView.GroupByExpressions.Count > 0 || isGroupingMCPTemplate) ||
            RadGrid_MCPTemplate.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_MCPTemplate_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_MCPTemplate.AllowCustomPaging = !ShouldApplySortMCPTemplate();
        FillMCPTemplate();
    }
    protected void lnkDeleteREPT_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_MCPTemplate.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                Label lblId = (Label)di.FindControl("lblId");
                if (chkSelect.Checked == true)
                {
                    objProp_User.Lang = string.Empty;
                    objProp_User.Remarks = string.Empty;
                    objProp_User.DtItems = CreateTable();
                    objProp_User.Mode = 2;
                    objProp_User.REPtemplateID = Convert.ToInt32(lblId.Text);
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objBL_User.AddEquipmentTemplate(objProp_User);
                    FillMCPTemplate();
                    RadGrid_MCPTemplate.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyDelREP", "noty({text: 'Template deleted successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningDelMcptemplate", "noty({text: 'Please select Template to delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string[] str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty).Split('!');
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelREP", "noty({text: '" + str[0] + "', dismissQueue: true, type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkAddREPT_Click(object sender, EventArgs e)
    {
        txtREPdesc.Text = "";
        txtREPremarks.Text = "";
        ViewState["edit"] = "0";
        ViewState["McpTempID"] = string.Empty;
        HdnEquipTempID.Value = string.Empty;
        CreateTable();
        BindGrid();
        RadGrid_TemplateItems.Rebind();
        string script = "function f(){$find(\"" + McpTemplateWindow.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }

    protected void lnkEditRepTemplate_Click(object sender, EventArgs e)
    {
        ViewState["edit"] = 1;
        CreateTable();
        BindGrid();

        foreach (GridDataItem gv in RadGrid_MCPTemplate.SelectedItems)
        {
            TableCell cell = gv["chkSelect"];
            CheckBox chkSelect = (CheckBox)cell.Controls[0];
            Label lblDesc = (Label)gv.FindControl("lblFdesc");
            Label lblRem = (Label)gv.FindControl("lblRemark");
            Label ID = (Label)gv.FindControl("lblID");

            if (chkSelect.Checked == true)
            {
                txtREPdesc.Text = lblDesc.Text;
                txtREPremarks.Text = lblRem.Text;
                DataSet ds = new DataSet();
                ViewState["McpTempID"] = ID.Text;
                HdnEquipTempID.Value = ID.Text;
                objCustomer.TemplateID = Convert.ToInt32(ID.Text);
                objCustomer.ConnConfig = Session["config"].ToString();
                ds = objBL_Customer.getTemplateItemByID(objCustomer);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        RadGrid_TemplateItems.VirtualItemCount = ds.Tables[0].Rows.Count;
                        RadGrid_TemplateItems.DataSource = ds.Tables[0];
                        RadGrid_TemplateItems.Rebind();
                    }
                }
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "Materialize.updateTextFields();", true);
                string script = "function f(){$find(\"" + McpTemplateWindow.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
        }
    }
    protected void lnkCopyRepTemplate_Click(object sender, EventArgs e)
    {
        txtREPdesc.Text = "";
        txtREPremarks.Text = "";
        ViewState["edit"] = 0;
        CreateTable();
        BindGrid();
        foreach (GridDataItem gv in RadGrid_MCPTemplate.SelectedItems)
        {
            TableCell cell = gv["chkSelect"];
            CheckBox chkSelect = (CheckBox)cell.Controls[0];
            Label lblDesc = (Label)gv.FindControl("lblFdesc");
            Label lblRem = (Label)gv.FindControl("lblRemark");
            Label ID = (Label)gv.FindControl("lblID");

            if (chkSelect.Checked == true)
            {
                txtREPdesc.Text = lblDesc.Text;
                txtREPremarks.Text = lblRem.Text;

                DataSet ds = new DataSet();
                ViewState["McpTempID"] = string.Empty;
                HdnEquipTempID.Value = string.Empty;
                objCustomer.TemplateID = Convert.ToInt32(ID.Text);
                objCustomer.ConnConfig = Session["config"].ToString();
                ds = objBL_Customer.getTemplateItemByID(objCustomer);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        RadGrid_TemplateItems.VirtualItemCount = ds.Tables[0].Rows.Count;
                        RadGrid_TemplateItems.DataSource = ds.Tables[0];
                        RadGrid_TemplateItems.Rebind();

                    }
                }
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "Materialize.updateTextFields();", true);
                string script = "function f(){$find(\"" + McpTemplateWindow.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
        }
    }
    private void BindGrid()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["templtable"];
        if (dt.Rows.Count > 0)
        {
            RadGrid_TemplateItems.VirtualItemCount = dt.Rows.Count;
            RadGrid_TemplateItems.DataSource = dt;
        }
    }
    bool isGroupingTemplateItems = false;
    public bool ShouldApplySortTemplateItems()
    {
        return RadGrid_TemplateItems.MasterTableView.FilterExpression != "" ||
            (RadGrid_TemplateItems.MasterTableView.GroupByExpressions.Count > 0 || isGroupingTemplateItems) ||
            RadGrid_TemplateItems.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_TemplateItems_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_TemplateItems.AllowCustomPaging = !ShouldApplySortTemplateItems();
        CreateTable();
        BindGrid();
    }

    private DataTable CreateTable()
    {
        Session["templtable"] = null;

        DataTable dt = new DataTable();
        dt.Columns.Add("Code", typeof(string));
        dt.Columns.Add("EquipT", typeof(int));
        dt.Columns.Add("Elev", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("Lastdate", typeof(DateTime));
        dt.Columns.Add("NextDateDue", typeof(DateTime));
        dt.Columns.Add("Frequency", typeof(int));
        dt.Columns.Add("Section", typeof(string));
        dt.Columns.Add("Notes", typeof(string));
        dt.Columns.Add("LeadEquip", typeof(int));
        DataRow dr = dt.NewRow();
        dr["Code"] = DBNull.Value;
        dr["EquipT"] = DBNull.Value;
        dr["Elev"] = 0;
        dr["fDesc"] = DBNull.Value;
        dr["Line"] = dt.Rows.Count + 1;
        dr["Lastdate"] = DBNull.Value;
        dr["NextDateDue"] = DBNull.Value;
        dr["Frequency"] = -1;
        dr["Notes"] = DBNull.Value;
        dr["LeadEquip"] = 0;
        dt.Rows.Add(dr);

        Session["templtable"] = dt;
        return dt;
    }

    protected void ibtnDeleteItem_Click(object sender, EventArgs e)
    {
        DeleteREPItem();
        RadGrid_TemplateItems.Rebind();
    }
    private void DeleteREPItem()
    {
        GridData();

        DataTable dt = new DataTable();
        dt = (DataTable)Session["templtable"];

        int count = 0;
        foreach (GridDataItem gr in RadGrid_TemplateItems.Items)
        {
            // TableCell cell = gr["chkSelect"];
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            Label lblIndex = (Label)gr.FindControl("lblIndex");
            int index = Convert.ToInt32(lblIndex.Text) - 1;

            if (chkSelect.Checked == true)
            {
                dt.Rows.RemoveAt(index - count);
                count++;
            }
        }

        if (dt.Rows.Count == 0)
        {
            DataRow dr = dt.NewRow();
            dr["Code"] = DBNull.Value;
            dr["EquipT"] = DBNull.Value;
            dr["Elev"] = 0;
            dr["fDesc"] = DBNull.Value;
            dr["Line"] = dt.Rows.Count;
            dr["Lastdate"] = DBNull.Value;
            dr["NextDateDue"] = DBNull.Value;
            dr["Frequency"] = -1;
            dr["Notes"] = DBNull.Value;
            dr["LeadEquip"] = 0;
            dt.Rows.Add(dr);
        }

        Session["templtable"] = dt;
        BindGrid();
    }

    private void GridData()
    {
        DataTable dt = (DataTable)Session["templtable"];

        DataTable dtDetails = dt.Clone();

        foreach (GridDataItem gr in RadGrid_TemplateItems.Items)
        {
            Label lblIndex = (Label)gr.FindControl("lblIndex");
            TextBox lblDesc = (TextBox)gr.FindControl("lblDesc");
            DropDownList ddlFreq = (DropDownList)gr.FindControl("ddlFreq");
            TextBox lblLdate = (TextBox)gr.FindControl("lblLdate");
            TextBox lblDuedate = (TextBox)gr.FindControl("lblDuedate");
            TextBox txtCode = (TextBox)gr.FindControl("txtCode");
            TextBox txtSection = (TextBox)gr.FindControl("txtSection");
            HtmlTextArea txtNotes = (HtmlTextArea)gr.FindControl("txtNotes");
            //if (lblDesc.Text.Trim() != string.Empty && txtCode.Text.Trim()!=string.Empty)
            //{
            DataRow dr = dtDetails.NewRow();
            dr["Code"] = txtCode.Text.Trim();
            dr["EquipT"] = 0;
            dr["Elev"] = 0;
            dr["line"] = lblIndex.Text;
            dr["fDesc"] = lblDesc.Text.Trim();
            dr["Frequency"] = Convert.ToInt32(ddlFreq.SelectedValue);
            dr["Section"] = txtSection.Text.Trim();
            // dr["Notes"] = txtNotes.Text.Trim();
            dr["Notes"] = txtNotes.InnerText.Trim();
            dr["LeadEquip"] = 0;

            dtDetails.Rows.Add(dr);
            //}
        }

        Session["templtable"] = dtDetails;
    }
    protected void txtCode_TextChanged(object sender, EventArgs e)
    {
        int exists = 0;
        TextBox txtCode = (TextBox)sender;
        GridDataItem grSelected = (GridDataItem)txtCode.NamingContainer;
        TextBox lblDesc = (TextBox)grSelected.FindControl("lblDesc");

        if (txtCode.Text.Trim().Equals(string.Empty))
        {
            txtCode.BackColor = System.Drawing.Color.White;
            return;
        }

        foreach (GridDataItem gr in RadGrid_TemplateItems.Items)
        {
            TextBox txtCodeField = (TextBox)gr.FindControl("txtCode");
            if (txtCodeField != txtCode)
            {
                if (!txtCodeField.Text.Trim().Equals(string.Empty))
                {
                    if (txtCodeField.Text.Trim().Equals(txtCode.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        exists = 1;
                    }
                }
            }
        }
        if (exists == 1)
        {
            txtCode.Focus();
            txtCode.BackColor = System.Drawing.Color.LightPink;
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAlertCode", "alert('Code already exists, please enter different code.')", true);
        }
        else
        {
            lblDesc.Focus();
            txtCode.BackColor = System.Drawing.Color.White;
        }
    }
    protected void lnkAddnewRow_Click(object sender, EventArgs e)
    {
        AddNewRow();
        RadGrid_TemplateItems.Rebind();
    }
    private void AddNewRow()
    {
        GridData();

        DataTable dt = new DataTable();
        dt = (DataTable)Session["templtable"];

        DataRow dr = dt.NewRow();
        dr["Code"] = DBNull.Value;
        dr["EquipT"] = DBNull.Value;
        dr["Elev"] = 0;
        dr["fDesc"] = DBNull.Value;
        dr["Line"] = dt.Rows.Count + 1;
        dr["Lastdate"] = DBNull.Value;
        dr["NextDateDue"] = DBNull.Value;
        dr["Frequency"] = -1;
        dr["Notes"] = DBNull.Value;
        dr["LeadEquip"] = 0;
        dt.Rows.Add(dr);

        Session["templtable"] = dt;

        BindGrid();
    }
    protected void ddlFreq_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        GridDataItem row = (GridDataItem)ddl.NamingContainer;
        TextBox txtlastdate = (TextBox)row.FindControl("lblLdate");
        TextBox txtDuedate = (TextBox)row.FindControl("lblDuedate");
        if (row != null)
        {
            string selectedValue = ((DropDownList)(row.FindControl("ddlFreq"))).SelectedValue;
            //Response.Write(selectedValue + " friend..Happy Coding..you have selected \"" + selectedValue + "\" from  row " + row.RowIndex);

            DateTime nextdate = new DateTime();
            DateTime lastdate = new DateTime();

            if (txtlastdate.Text.Trim() != string.Empty)
            {
                if (DateTime.TryParse(txtlastdate.Text.Trim(), out lastdate))
                {
                    if (selectedValue == "0")
                    {
                        nextdate = lastdate.AddDays(1);
                    }
                    if (selectedValue == "1")
                    {
                        nextdate = lastdate.AddDays(7);
                    }
                    if (selectedValue == "2")
                    {
                        nextdate = lastdate.AddDays(14);
                    }
                    if (selectedValue == "3")
                    {
                        nextdate = lastdate.AddMonths(1);
                    }
                    if (selectedValue == "4")
                    {
                        nextdate = lastdate.AddMonths(2);
                    }
                    if (selectedValue == "5")
                    {
                        nextdate = lastdate.AddMonths(3);
                    }
                    if (selectedValue == "6")
                    {
                        nextdate = lastdate.AddMonths(6);
                    }
                    if (selectedValue == "7")
                    {
                        nextdate = lastdate.AddYears(1);
                    }
                    txtDuedate.Text = nextdate.ToShortDateString();
                }
            }
        }
    }
    private int CheckDuplicateCodes()
    {
        int exists = 0;
        foreach (GridDataItem gr in RadGrid_TemplateItems.Items)
        {
            TextBox txtCodeField = (TextBox)gr.FindControl("txtCode");

            foreach (GridDataItem gr1 in RadGrid_TemplateItems.Items)
            {
                TextBox txtCodeField1 = (TextBox)gr1.FindControl("txtCode");

                if (txtCodeField != txtCodeField1)
                {
                    if (txtCodeField.Text.Trim().Equals(txtCodeField1.Text.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        exists = 1;
                        txtCodeField.Focus();
                    }
                }
            }
        }

        return exists;
    }
    protected void lnkSaveTemplate_Click(object sender, EventArgs e)
    {
        try
        {

            if (CheckDuplicateCodes() == 1)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAlertCode", "alert('Code already exists, please enter different code.')", true);
                return;
            }

            GridData();
            DataTable dt = (DataTable)Session["templtable"];
            foreach (DataRow dr in dt.Rows)
            {
                dr["Lastdate"] = System.DateTime.Now;
                dr["NextDateDue"] = System.DateTime.Now;
            }

            string msg = "Added";
            int REPTID = 0;
            if (ViewState["edit"].ToString() == "1")
            {
                msg = "Updated";
                REPTID = Convert.ToInt32(ViewState["McpTempID"]);
            }
            objProp_User.Lang = txtREPdesc.Text.Trim();
            objProp_User.Remarks = txtREPremarks.Text.Trim();
            objProp_User.DtItems = dt;
            objProp_User.Mode = Convert.ToInt32(ViewState["edit"].ToString());
            objProp_User.REPtemplateID = REPTID;
            objProp_User.ConnConfig = Session["config"].ToString();
            objBL_User.AddEquipmentTemplate(objProp_User);

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddREP", "noty({text: 'Template " + msg + " Successfully.', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseMCPTemplateWindow();", true);
            FillMCPTemplate();
            RadGrid_MCPTemplate.Rebind();
        }
        catch (Exception ex)
        {
            string[] str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty).Split('!');
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddCattype", "noty({text: '" + str[0] + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

        }
    }
    #endregion

    #region MCP Status

    private void FillMCPS()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getMCPS(objProp_User);

        RadGrid_MCPStatus.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_MCPStatus.DataSource = ds.Tables[0];
    }
    bool isGroupingMCPStatus = false;
    public bool ShouldApplySortMCPStatus()
    {
        return RadGrid_MCPStatus.MasterTableView.FilterExpression != "" ||
            (RadGrid_MCPStatus.MasterTableView.GroupByExpressions.Count > 0 || isGroupingMCPStatus) ||
            RadGrid_MCPStatus.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_MCPStatus_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_MCPStatus.AllowCustomPaging = !ShouldApplySortMCPStatus();
        FillMCPS();
    }
    protected void lnkSaveMCPS_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtMCPS.Text.Trim() != string.Empty)
            {
                objProp_User.ConnConfig = Session["config"].ToString();
                objProp_User.EquipType = txtMCPS.Text.Trim();
                objBL_User.AddMCPS(objProp_User);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseMCPStatusWindow();", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccessMCPS", "noty({text: 'MCPS Status Added Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                FillMCPS();
                RadGrid_MCPStatus.Rebind();
                txtMCPS.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string type = "error";
            if (str.Contains("MCP Status already exists, please use different name"))
            {
                type = "warning";

            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddMCPS", "noty({text: '" + str + "', dismissQueue: true,  type : '" + type + "', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    protected void lnkDeleteMCPS_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_MCPStatus.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                Label lblId = (Label)di.FindControl("lblId");
                if (chkSelect.Checked == true)
                {
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.EquipType = lblId.Text;

                    objBL_User.DeleteMCPS(objProp_User);
                    FillMCPS();
                    RadGrid_MCPStatus.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccMCPS", "noty({text: 'MCP Status " + lblId.Text + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningDelMCPS", "noty({text: 'Please select Status to delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelMCPS", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }
    #endregion

    #region Custom Template
    protected void FillCustomTemplate()
    {
        objCustomer.ConnConfig = Session["config"].ToString();

        DataSet ds = new DataSet();
        ds = objBL_Customer.getCustomTemplate(objCustomer);
        RadGrid_CustomTemplate.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_CustomTemplate.DataSource = ds.Tables[0];
    }
    bool isGroupingCustomTemplate = false;
    public bool ShouldApplySortCustomTemplate()
    {
        return RadGrid_CustomTemplate.MasterTableView.FilterExpression != "" ||
            (RadGrid_CustomTemplate.MasterTableView.GroupByExpressions.Count > 0 || isGroupingCustomTemplate) ||
            RadGrid_CustomTemplate.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_CustomTemplate_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_CustomTemplate.AllowCustomPaging = !ShouldApplySortCustomTemplate();
        FillCustomTemplate();
    }
    protected void lnkDelCusttemp_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            DataTable dtCustomval = new DataTable();
            dtCustomval.Columns.Add("ElevT", typeof(int));
            dtCustomval.Columns.Add("ItemID", typeof(int));
            dtCustomval.Columns.Add("Line", typeof(int));
            dtCustomval.Columns.Add("Value", typeof(string));

            foreach (GridDataItem di in RadGrid_CustomTemplate.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                Label lblID = (Label)di.FindControl("lblId");

                if (chkSelect.Checked == true)
                {
                    objProp_User.Lang = string.Empty;
                    objProp_User.Remarks = string.Empty;
                    objProp_User.DtItems = CreateCustomTTable();
                    objProp_User.dtCustomValues = dtCustomval;
                    objProp_User.DtItemsDeleted = objProp_User.DtItems.Clone();
                    objProp_User.Mode = 2;
                    objProp_User.REPtemplateID = Convert.ToInt32(lblID.Text);
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objBL_User.AddCustomTemplate(objProp_User);

                    FillCustomTemplate();
                    RadGrid_CustomTemplate.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyDelCust", "noty({text: 'Template deleted successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningDelCusttemp", "noty({text: 'Please select Template to delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string[] str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty).Split('!');
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelREP", "noty({text: '" + str[0] + "', dismissQueue: true, type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private DataTable CreateCustomTTable()
    {
        Session["Custtempltable"] = null;

        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("ElevT", typeof(int));
        dt.Columns.Add("Elev", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("value", typeof(string));
        dt.Columns.Add("Format", typeof(string));
        dt.Columns.Add("OrderNo", typeof(int));
        dt.Columns.Add("LeadEquip", typeof(int));
        DataRow dr = dt.NewRow();
        dr["ID"] = 0;
        dr["ElevT"] = DBNull.Value;
        dr["Elev"] = 0;
        dr["fDesc"] = DBNull.Value;
        dr["Line"] = dt.Rows.Count + 1;
        dr["Format"] = DBNull.Value;
        dr["value"] = DBNull.Value;
        dr["OrderNo"] = 0;
        dr["LeadEquip"] = 0;
        dt.Rows.Add(dr);

        Session["Custtempltable"] = dt;
        return dt;
    }

    protected void lnkAddCusttemp_Click(object sender, EventArgs e)
    {
        // ClearAll();
        txtCustdesc.Text = string.Empty;
        txtCustRemarks.Text = string.Empty;
        hdnCusttempID.Value = string.Empty;
        ViewState["CustomTempID"] = string.Empty;
        ViewState["edit"] = "0";
        CreateCustomTTable();
        BindCustTGrid();
        RadGrid_CustomTempItems.Rebind();
        string script = "function f(){$find(\"" + CustomTemplateWindow.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }
    private void BindCustTGrid()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["Custtempltable"];

        if (dt.Rows.Count > 0)
        {
            RadGrid_CustomTempItems.VirtualItemCount = dt.Rows.Count;
            RadGrid_CustomTempItems.DataSource = dt;
        }
    }
    bool isGroupingCustomTempItems = false;
    public bool ShouldApplySortCustomTempItems()
    {
        return RadGrid_CustomTempItems.MasterTableView.FilterExpression != "" ||
            (RadGrid_CustomTempItems.MasterTableView.GroupByExpressions.Count > 0 || isGroupingCustomTempItems) ||
            RadGrid_CustomTempItems.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_CustomTempItems_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_CustomTempItems.AllowCustomPaging = !ShouldApplySortCustomTempItems();
        CreateCustomTTable();
        BindCustTGrid();
    }
    protected void lnkAddnewCTRow_Click(object sender, EventArgs e)
    {
        CustTempGridData();
        DataTable dt = new DataTable();
        dt = (DataTable)Session["Custtempltable"];
        DataRow dr = dt.NewRow();
        dr["ID"] = 0;
        dr["ElevT"] = DBNull.Value;
        dr["Elev"] = 0;
        dr["fDesc"] = DBNull.Value;
        dr["Line"] = dt.Rows.Count + 1;
        dr["Format"] = DBNull.Value;
        dr["value"] = DBNull.Value;
        dr["OrderNo"] = 0;
        dt.Rows.Add(dr);
        Session["Custtempltable"] = dt;
        BindCustTGrid();
        BindCustomDropDown();
        RadGrid_CustomTempItems.Rebind();
        ReorderGridRow();

    }
    private void CustTempGridData()
    {
        DataTable dt = (DataTable)Session["Custtempltable"];
        DataTable dtDetails = dt.Clone();
        DataTable dtCustomValues = new DataTable();

        dtCustomValues.Columns.Add("ElevT", typeof(int));
        dtCustomValues.Columns.Add("ItemID", typeof(int));
        dtCustomValues.Columns.Add("Line", typeof(int));
        dtCustomValues.Columns.Add("Value", typeof(string));

        //int i=1;
        foreach (GridDataItem gr in RadGrid_CustomTempItems.Items)
        {
            Label lblID = (Label)gr.FindControl("lblID");
            Label lblIndex = (Label)gr.FindControl("lblIndex");
            TextBox lblDesc = (TextBox)gr.FindControl("lblDesc");
            HiddenField txtRowLine = (HiddenField)gr.FindControl("txtRowLine");
            DropDownList ddlFormat = (DropDownList)gr.FindControl("ddlFormat");
            DropDownList ddlCustomValue = (DropDownList)gr.FindControl("ddlCustomValue");

            foreach (ListItem li in ddlCustomValue.Items)
            {
                if (li.Value != string.Empty)
                {
                    DataRow drCustomVal = dtCustomValues.NewRow();
                    drCustomVal["ElevT"] = 0;
                    drCustomVal["ItemID"] = Convert.ToInt32(lblID.Text);
                    drCustomVal["Line"] = lblIndex.Text;
                    drCustomVal["Value"] = li.Value;
                    dtCustomValues.Rows.Add(drCustomVal);
                    //i++;
                }

            }

            DataRow dr = dtDetails.NewRow();
            dr["ID"] = Convert.ToInt32(lblID.Text);
            dr["ElevT"] = 0;
            dr["Elev"] = 0;
            dr["line"] = lblIndex.Text;
            dr["fDesc"] = lblDesc.Text.Trim();
            dr["Format"] = ddlFormat.SelectedValue;
            dr["OrderNo"] = txtRowLine.Value;
            dr["LeadEquip"] = 0;
            dtDetails.Rows.Add(dr);

        }
        Session["Custtempltable"] = dtDetails;
        Session["customvalues"] = dtCustomValues;
    }
    private void BindCustomDropDown()
    {
        int rowIndex = 1;
        foreach (GridDataItem gr in RadGrid_CustomTempItems.Items)
        {

            DropDownList ddlFormat = (DropDownList)gr.FindControl("ddlFormat");
            Label lblLine = (Label)gr.FindControl("lblLine");
            Panel pnlCustomValue = (Panel)gr.FindControl("pnlCustomValue");
            if (ddlFormat.SelectedValue == "Dropdown")
            {
                pnlCustomValue.Visible = true;

            }
            else
                pnlCustomValue.Visible = false;

            DropDownList ddlCustomValue = (DropDownList)gr.FindControl("ddlCustomValue");
            Label lblID = (Label)gr.FindControl("lblID");

            if (Session["customvalues"] != null)
            {
                DataTable dtCustomval = (DataTable)Session["customvalues"];
                DataTable dt = dtCustomval.Clone();
                DataRow[] result = dtCustomval.Select("ItemID = " + Convert.ToInt32(lblID.Text) + "");
                // DataRow[] result = dtCustomval.Select("Line = " + Convert.ToInt32(lblLine.Text) + "");
                foreach (DataRow row in result)
                {
                    dt.ImportRow(row);
                }

                //objCustomer.ConnConfig = Session["config"].ToString();
                //objCustomer.ItemID = Convert.ToInt32(lblID.Text);
                //DataSet ds = objBL_Customer.getCustomValues(objCustomer);
                if (dt.Rows.Count > 0)
                {
                    //dt.DefaultView.Sort = "Value  ASC";
                    dt.DefaultView.Sort = "LINE  ASC";
                    dt = dt.DefaultView.ToTable();
                }

                ddlCustomValue.DataSource = dt;
                ddlCustomValue.DataTextField = "Value";
                ddlCustomValue.DataValueField = "Value";
                ddlCustomValue.DataBind();
                ddlCustomValue.Items.Insert(0, (new ListItem("--Add New--", "")));
                if (ddlFormat.SelectedValue == "Dropdown")
                {
                    rowIndex++;

                }
            }
        }
    }
    protected void RadGrid_CustomTempItems_ItemDataBound(object sender, GridItemEventArgs e)
    {
        int rowIndex = 1;
        foreach (GridDataItem gr in RadGrid_CustomTempItems.Items)
        {
            DropDownList ddlFormat = (DropDownList)gr.FindControl("ddlFormat");
            Label lblLine = (Label)gr.FindControl("lblLine");
            Panel pnlCustomValue = (Panel)gr.FindControl("pnlCustomValue");
            if (ddlFormat.SelectedValue == "Dropdown")
            {
                pnlCustomValue.Visible = true;
                DropDownList ddlCustomValue = (DropDownList)gr.FindControl("ddlCustomValue");
                Label lblID = (Label)gr.FindControl("lblID");

                if (Session["customvalues"] != null)
                {
                    DataTable dtCustomval = (DataTable)Session["customvalues"];
                    DataTable dt = dtCustomval.Clone();
                    //DataRow[] result = dtCustomval.Select("ItemID = " + Convert.ToInt32(lblID.Text) + "");
                    DataRow[] result = dtCustomval.Select("Line = " + Convert.ToInt32(lblLine.Text) + "");
                    foreach (DataRow row in result)
                    {
                        dt.ImportRow(row);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        dt.DefaultView.Sort = "LINE  ASC";
                        dt = dt.DefaultView.ToTable();
                    }

                    ddlCustomValue.DataSource = dt;
                    ddlCustomValue.DataTextField = "Value";
                    ddlCustomValue.DataValueField = "Value";
                    ddlCustomValue.DataBind();
                    ddlCustomValue.Items.Insert(0, (new ListItem("--Add New--", "")));
                    if (ddlFormat.SelectedValue == "Dropdown")
                    {
                        rowIndex++;
                    }
                }
            }
            else
                pnlCustomValue.Visible = false;
        }

        if (e.Item is GridFooterItem)
        {
            GridFooterItem footerItem = e.Item as GridFooterItem;
            Label lb = (Label)footerItem.FindControl("lblRowCount");
            lb.Text = "Total Line Items: " + Convert.ToString(RadGrid_CustomTempItems.MasterTableView.DataSourceCount);

        }
    }

    protected void RadGrid_WageSchedule_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
    protected void ReorderGridRow()
    {
        int count = 0;
        foreach (GridDataItem gr in RadGrid_CustomTempItems.Items)
        {
            HiddenField OrderNo = (HiddenField)gr.FindControl("txtRowLine");
            OrderNo.Value = (count = count + 1).ToString();
        }
    }
    protected void ibtnDeleteCItem_Click(object sender, EventArgs e)
    {
        DeleteCustItem();
        ReorderGridRow();
        RadGrid_CustomTempItems.Rebind();
    }
    private void DeleteCustItem()
    {
        CustTempGridData();

        DataTable dt = new DataTable();
        dt = (DataTable)Session["Custtempltable"];
        DataTable dtdeleted = dt.Clone();
        int count = 0;
        foreach (GridDataItem gr in RadGrid_CustomTempItems.Items)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            Label lblIndex = (Label)gr.FindControl("lblIndex");
            int index = Convert.ToInt32(lblIndex.Text) - 1;
            if (chkSelect.Checked == true)
            {
                dtdeleted.ImportRow(dt.Rows[index - count]);
                dt.Rows.RemoveAt(index - count);
                count++;
            }
        }

        Session["ctempdeletedrows"] = dtdeleted;

        if (dt.Rows.Count == 0)
        {
            DataRow dr = dt.NewRow();
            dr["ID"] = 0;
            dr["elevT"] = DBNull.Value;
            dr["Elev"] = 0;
            dr["fDesc"] = DBNull.Value;
            dr["Line"] = dt.Rows.Count;
            dr["Format"] = DBNull.Value;
            dr["OrderNo"] = 0;
            dt.Rows.Add(dr);
        }

        Session["Custtempltable"] = dt;
        BindCustTGrid();
        BindCustomDropDown();
    }
    protected void ddlFormat_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        GridDataItem row = (GridDataItem)ddl.NamingContainer;
        Panel pnlCustomValue = (Panel)row.FindControl("pnlCustomValue");
        if (row != null)
        {
            if (ddl.SelectedValue == "Dropdown")
                pnlCustomValue.Visible = true;
            else
                pnlCustomValue.Visible = false;
        }
    }
    protected void ddlCustomValue_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        GridDataItem row = (GridDataItem)ddl.NamingContainer;
        LinkButton lnkAddCustomValue = (LinkButton)row.FindControl("lnkAddCustomValue");
        LinkButton lnkUpdateCustomValue = (LinkButton)row.FindControl("lnkUpdateCustomValue");
        LinkButton lnkDelCustomValue = (LinkButton)row.FindControl("lnkDelCustomValue");
        TextBox txtCustomValue = (TextBox)row.FindControl("txtCustomValue");
        if (ddl.SelectedIndex == 0)
        {
            lnkAddCustomValue.Visible = true;
            lnkUpdateCustomValue.Visible = false;
            lnkDelCustomValue.Visible = false;
            txtCustomValue.Text = string.Empty;
        }
        else
        {
            lnkAddCustomValue.Visible = false;
            lnkUpdateCustomValue.Visible = true;
            lnkDelCustomValue.Visible = true;
            txtCustomValue.Text = ddl.SelectedValue;
        }

    }
    protected void lnkAddCustomValue_Click(object sender, EventArgs e)
    {
        LinkButton lnkAdd = (LinkButton)sender;
        GridDataItem row = (GridDataItem)lnkAdd.NamingContainer;
        TextBox txtCustomValue = (TextBox)row.FindControl("txtCustomValue");
        DropDownList ddlCustomValue = (DropDownList)row.FindControl("ddlCustomValue");
        LinkButton lnkUpdateCustomValue = (LinkButton)row.FindControl("lnkUpdateCustomValue");
        LinkButton lnkDelCustomValue = (LinkButton)row.FindControl("lnkDelCustomValue");
        if (txtCustomValue.Text.Trim() != string.Empty)
        {
            ddlCustomValue.Items.Add(new ListItem(txtCustomValue.Text.Trim(), txtCustomValue.Text.Trim()));
            txtCustomValue.Text = string.Empty;
            //ddlCustomValue.SelectedValue = txtCustomValue.Text.Trim();

            //lnkAdd.Visible = false;
            //lnkUpdateCustomValue.Visible = true;
            //lnkDelCustomValue.Visible = true;
        }
    }

    protected void lnkUpdateCustomValue_Click(object sender, EventArgs e)
    {
        LinkButton lnkUpdate = (LinkButton)sender;
        GridDataItem row = (GridDataItem)lnkUpdate.NamingContainer;
        TextBox txtCustomValue = (TextBox)row.FindControl("txtCustomValue");
        DropDownList ddlCustomValue = (DropDownList)row.FindControl("ddlCustomValue");
        if (txtCustomValue.Text.Trim() != string.Empty)
        {
            ddlCustomValue.Items.Remove(new ListItem(ddlCustomValue.SelectedValue, ddlCustomValue.SelectedValue));
            ddlCustomValue.Items.Add(new ListItem(txtCustomValue.Text.Trim(), txtCustomValue.Text.Trim()));
            ddlCustomValue.SelectedValue = txtCustomValue.Text.Trim();
        }
    }
    protected void lnkDelCustomValue_Click(object sender, EventArgs e)
    {
        LinkButton lnkDelete = (LinkButton)sender;
        GridDataItem row = (GridDataItem)lnkDelete.NamingContainer;
        TextBox txtCustomValue = (TextBox)row.FindControl("txtCustomValue");
        DropDownList ddlCustomValue = (DropDownList)row.FindControl("ddlCustomValue");
        LinkButton lnkAddCustomValue = (LinkButton)row.FindControl("lnkAddCustomValue");
        LinkButton lnkUpdateCustomValue = (LinkButton)row.FindControl("lnkUpdateCustomValue");

        ddlCustomValue.Items.Remove(new ListItem(ddlCustomValue.SelectedValue, ddlCustomValue.SelectedValue));
        ddlCustomValue.SelectedIndex = 0;
        lnkAddCustomValue.Visible = true;
        lnkUpdateCustomValue.Visible = false;
        lnkDelete.Visible = false;
        txtCustomValue.Text = string.Empty;
    }

    protected void lnkEditCusttemp_Click(object sender, EventArgs e)
    {
        txtCustdesc.Text = "";
        txtCustRemarks.Text = "";
        ViewState["edit"] = 1;
        txtloc.ReadOnly = true;
        CreateCustomTTable();
        BindCustTGrid();

        foreach (GridDataItem gv in RadGrid_CustomTemplate.SelectedItems)
        {
            TableCell cell = gv["chkSelect"];
            CheckBox chkSelect = (CheckBox)cell.Controls[0];
            Label lblDesc = (Label)gv.FindControl("lblFdesc");
            Label lblRem = (Label)gv.FindControl("lblRemark");
            Label ID = (Label)gv.FindControl("lblID");

            if (chkSelect.Checked == true)
            {
                txtCustdesc.Text = lblDesc.Text;
                txtCustRemarks.Text = lblRem.Text;
                DataSet ds = new DataSet();
                hdnCusttempID.Value = ID.Text;
                ViewState["CustomTempID"] = ID.Text;
                //objCustomer.TemplateID = Convert.ToInt32(ID.Text);
                //objCustomer.ConnConfig = Session["config"].ToString();
                //ds = objBL_Customer.getCustTemplateItemByID(objCustomer);

                ds = objBL_Customer.GetEquipmentCustTemplateItem(Session["config"].ToString(), Convert.ToInt32(ID.Text), 0, 0);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            Session["customvalues"] = ds.Tables[1];
                        }
                    }
                }
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        RadGrid_CustomTempItems.VirtualItemCount = ds.Tables[0].Rows.Count;
                        RadGrid_CustomTempItems.DataSource = ds.Tables[0];
                        RadGrid_CustomTempItems.Rebind();

                    }
                }
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "Materialize.updateTextFields();", true);
                string script = "function f(){$find(\"" + CustomTemplateWindow.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
        }

        BindCustomDropDown();
    }
    protected void lnkCopyCusttemp_Click(object sender, EventArgs e)
    {
        txtCustdesc.Text = "";
        txtCustRemarks.Text = "";
        ViewState["edit"] = 0;
        CreateCustomTTable();
        BindCustTGrid();

        foreach (GridDataItem gv in RadGrid_CustomTemplate.SelectedItems)
        {
            TableCell cell = gv["chkSelect"];
            CheckBox chkSelect = (CheckBox)cell.Controls[0];
            Label lblDesc = (Label)gv.FindControl("lblFdesc");
            Label lblRem = (Label)gv.FindControl("lblRemark");
            Label ID = (Label)gv.FindControl("lblID");
            if (chkSelect.Checked == true)
            {
                txtCustdesc.Text = lblDesc.Text;
                txtCustRemarks.Text = lblRem.Text;
                DataSet ds = new DataSet();
                ViewState["CustomTempID"] = string.Empty;
                hdnCusttempID.Value = ID.Text;
                //objCustomer.TemplateID = Convert.ToInt32(ID.Text);
                //objCustomer.ConnConfig = Session["config"].ToString();
                //ds = objBL_Customer.getCustTemplateItemByID(objCustomer);
                ds = objBL_Customer.GetEquipmentCustTemplateItem(Session["config"].ToString(), Convert.ToInt32(ID.Text), 0, 0);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            Session["customvalues"] = ds.Tables[1];
                        }
                    }
                }
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        RadGrid_CustomTempItems.VirtualItemCount = ds.Tables[0].Rows.Count;
                        RadGrid_CustomTempItems.DataSource = ds.Tables[0];
                        RadGrid_CustomTempItems.Rebind();
                    }
                }
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "Materialize.updateTextFields();", true);
                string script = "function f(){$find(\"" + CustomTemplateWindow.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
        }
    }
    protected void lnkSavecusttempl_Click(object sender, EventArgs e)
    {
        try
        {
            CustTempGridData();
            DataTable dt = (DataTable)Session["Custtempltable"];
            DataTable dtCustomval = (DataTable)Session["customvalues"];

            var duplicates = dt.AsEnumerable().GroupBy(r => r[3]).Where(gr => gr.Count() > 1).ToList();
            if (duplicates.Any())
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAlertDupl", "noty({text: 'Duplicate Items found', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                return;
            }

            DataTable dtDeleted = dt.Clone();
            if (Session["ctempdeletedrows"] != null)
                dtDeleted = (DataTable)Session["ctempdeletedrows"];

            string msg = "Added";
            int REPTID = 0;
            if (ViewState["edit"].ToString() == "1")
            {
                msg = "Updated";
                REPTID = Convert.ToInt32(ViewState["CustomTempID"]);
            }
            objProp_User.Lang = txtCustdesc.Text.Trim();
            objProp_User.Remarks = txtCustRemarks.Text.Trim();
            objProp_User.DtItems = dt;
            objProp_User.DtItemsDeleted = dtDeleted;
            objProp_User.dtCustomValues = dtCustomval;
            objProp_User.Mode = Convert.ToInt32(ViewState["edit"].ToString());
            objProp_User.REPtemplateID = REPTID;
            objProp_User.ConnConfig = Session["config"].ToString();
            objBL_User.AddCustomTemplate(objProp_User);

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddTemp", "noty({text: 'Template " + msg + " Successfully.', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseCustomTemplateWindow();", true);
            FillCustomTemplate();
            RadGrid_CustomTemplate.Rebind();
        }
        catch (Exception ex)
        {
            string[] str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty).Split('!');
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddCattype", "noty({text: '" + str[0] + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    #endregion

    #region Test Type
    private void BindTestTypes()
    {
        DataSet dst = new DataSet();
        TestTypes _objProptesttypes = new TestTypes();
        BL_SafetyTest _objbltesttypes = new BL_SafetyTest();

        _objProptesttypes.ConnConfig = WebBaseUtility.ConnectionString;
        txtTesttypeCount.ReadOnly = true;
        dst = _objbltesttypes.GetAllTestTypes(_objProptesttypes);

        RadGrid_TestType.VirtualItemCount = dst.Tables[0].Rows.Count;
        RadGrid_TestType.DataSource = dst.Tables[0];

        DataSet ds = _objbltesttypes.GetAllCategory(_objProptesttypes);
        ddlCategory.Items.Clear();
        ddlCategory.Items.Add(new ListItem("Select Category", "0"));
        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlCategory.Items.Add(new ListItem((string)ds.Tables[0].Rows[i]["Type"], (string)ds.Tables[0].Rows[i]["Type"]));
                }
            }
        }
        ddlTestTypeCover.Items.Clear();
        if (dst != null)
        {
            if (dst.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dst.Tables[0].Rows.Count; i++)
                {
                    var cboItem = new RadComboBoxItem() { Text = dst.Tables[0].Rows[i]["Name"].ToString(), Value = dst.Tables[0].Rows[i]["ID"].ToString() };
                    ddlTestTypeCover.Items.Add(cboItem);

                }
            }
        }

    }
    bool isGroupingTestType = false;
    public bool ShouldApplySortTestType()
    {
        return RadGrid_TestType.MasterTableView.FilterExpression != "" ||
            (RadGrid_TestType.MasterTableView.GroupByExpressions.Count > 0 || isGroupingTestType) ||
            RadGrid_TestType.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_TestType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_TestType.AllowCustomPaging = !ShouldApplySortTestType();
        BindTestTypes();
    }
    protected void lnksavetesttype_Click(object sender, EventArgs e)
    {
        try
        {
            bool isvalid = true;
            TestTypes _Objprptt = new TestTypes();
            BL_SafetyTest _bltest = new BL_SafetyTest();
            int count = 0;
            _Objprptt.ConnConfig = WebBaseUtility.ConnectionString;
            _Objprptt.Name = txttesttypename.Text;
            _Objprptt.Authority = txtAuthority.Text;
            _Objprptt.Cat = ddlCategory.SelectedValue;
            _Objprptt.fDesc = txtTicketDesc.Text;
            _Objprptt.Remarks = txtTestTypeRemarks.Text;
            _Objprptt.Frequency = string.IsNullOrEmpty(txtFrequency.Text) ? 0 : Convert.ToInt32(txtFrequency.Text);
            int.TryParse(hdnTestTypeCount.Value, out count);
            _Objprptt.Count = count;
            _Objprptt.ID = hdntesttypes.Value != string.Empty ? Convert.ToInt32(hdntesttypes.Value) : 0;
            _Objprptt.NextDateCalcMode = Convert.ToInt32(drpdwnTestDueBy.SelectedValue);
            _Objprptt.Charge = Convert.ToInt32(chkChargeableEdit.Checked);
            // _Objprptt.ThirdParty = Convert.ToInt32(chkThirdParty.Checked);
            _Objprptt.Status = Convert.ToInt32(ddlTestTypeStatus.SelectedValue);
            _Objprptt.TicketCovered = Convert.ToBoolean(chkTicketCovered.Checked);

            List<string> lsTestTypeCover = new List<string>();
            foreach (RadComboBoxItem item in ddlTestTypeCover.Items)
            {
                if (item.Checked)
                {
                    lsTestTypeCover.Add(item.Value);
                }
            }
            _Objprptt.TestTypeCover = "";
            if (lsTestTypeCover.Count() > 0)
            {
                _Objprptt.TestTypeCover = string.Join(",", lsTestTypeCover.ToArray());
            }



            DataSet dstesttype = _bltest.GetTestTypesByName(_Objprptt);

            if (dstesttype.Tables[0].Rows.Count > 0)
            {
                int dupNameId = dstesttype.Tables[0].Rows[0]["ID"] != DBNull.Value ? (int)dstesttype.Tables[0].Rows[0]["ID"] : 0;
                //During creation
                if (hdnAddEdittesttypes.Value == "0" & _Objprptt.ID != dupNameId)
                    isvalid = false;
                //During Update if the item being updated has an name already assigned to a test type
                if (hdnAddEdittesttypes.Value == "1" & _Objprptt.ID != dupNameId)
                    isvalid = false;
            }
            if (isvalid)
            {

                string msg = "Added";

                if (hdnAddEdittesttypes.Value == "0")
                {


                    _bltest.CreateTestType(_Objprptt);
                }
                else if (hdnAddEdittesttypes.Value == "1")
                {
                    msg = "Updated";

                    _bltest.UpdateTestType(_Objprptt);
                }
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseTestTypeWindow();", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: 'Test Type " + msg + " successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                BindTestTypes();
                RadGrid_TestType.Rebind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyerrorAddProstype1", "noty({text: 'A test type with the name " + _Objprptt.Name + " already exists.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddEqupClassification", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);


        }
    }
    protected void lnkdeletetesttypes_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            TestTypes _Objprptt = new TestTypes();
            BL_SafetyTest _bltest = new BL_SafetyTest();
            _Objprptt.ConnConfig = WebBaseUtility.ConnectionString;
            foreach (GridDataItem di in RadGrid_TestType.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                Label lblName = (Label)di.FindControl("lblName");
                Label lblCount = (Label)di.FindControl("lblCount");
                HiddenField hdnid = (HiddenField)di.FindControl("hdnid");
                int count = 0;
                int.TryParse(lblCount.Text, out count);

                if (chkSelect.Checked == true)
                {
                    if (count > 0)
                    {
                        string msg = "Test " + lblName.Text.TrimEnd() + " still has " + count + " units pointing to it.Correct this situation(by using the Mass Update method) prior to trying this operation again.";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: '" + msg + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }
                    else
                    {
                        _Objprptt.ID = Convert.ToInt32(hdnid.Value);
                        _bltest.DeleteTestType(_Objprptt);
                        BindTestTypes();
                        RadGrid_TestType.Rebind();
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: 'Test Type " + lblName.Text + " successfully deleted', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningDelMCPS", "noty({text: 'Please select Test Type to delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddWarehousetype1", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    #endregion

    #region Sales Tax
    private void FillSalesTax()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getSalesTax(objProp_User);

        RadGrid_SalesTax.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_SalesTax.DataSource = ds.Tables[0];
    }
    bool isGroupingSalesTax = false;
    public bool ShouldApplySortSalesTax()
    {
        return RadGrid_SalesTax.MasterTableView.FilterExpression != "" ||
            (RadGrid_SalesTax.MasterTableView.GroupByExpressions.Count > 0 || isGroupingSalesTax) ||
            RadGrid_SalesTax.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_SalesTax_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_SalesTax.AllowCustomPaging = !ShouldApplySortSalesTax();
        FillSalesTax();
        DisplayGSTFields();
    }
    protected void SearchTaxType_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.UType = Convert.ToInt16(SearchTaxType.SelectedValue);

        if (objProp_User.UType == 2)
        {
            ds = objBL_User.getSalesTax(objProp_User);
        }
        else
        {
            ds = objBL_User.getSalesTaxByTaxType(objProp_User);
        }

        RadGrid_SalesTax.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_SalesTax.DataSource = ds.Tables[0];
        RadGrid_SalesTax.Rebind();
    }
    private void DisplayGSTFields() // change by dev , For canandian companies display fields on 4th feb, 16
    {
        trpst.Visible = false;
        trpstType.Visible = false;
        objGeneral.ConnConfig = Session["config"].ToString();
        objGeneral.CustomName = "Country";
        DataSet _dsCustom = objBL_General.getCustomFields(objGeneral);
        if (_dsCustom.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(_dsCustom.Tables[0].Rows[0]["Label"].ToString()))
            {
                if (_dsCustom.Tables[0].Rows[0]["Label"].ToString().Equals("1"))
                {
                    trpst.Visible = true;
                    trpstType.Visible = true;
                }
            }
        }
    }
    protected void lnkSalesSave_Click(object sender, EventArgs e)
    {
        IsAddEdit = false;
        try
        {
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.SalesTax = txtsalesName.Text;
            objProp_User.SalesDescription = txtSalesDesc.Text;
            objProp_User.SalesRate = Convert.ToDouble(txtsalesRate.Text);
            objProp_User.State = ddlSalesState.SelectedValue;
            objProp_User.Remarks = txtSalesNotes.Text;
            if (hdnAcctID.Value != string.Empty)
                objProp_User.GLAccount = Convert.ToInt32(hdnAcctID.Value);          // change by dev on 2nd march, 16
            objProp_User.UType = Convert.ToInt16(rdpTaxType.SelectedValue);
            //objProp_User.sType = Convert.ToInt16(ddlType.SelectedValue);        // added by dev on 4th feb, 16
            objProp_User.sType = Convert.ToInt16(hdnStaxType.Value);        // added by dev on 4th feb, 16
            objProp_User.PSTReg = txtPSTReg.Text;
            if (hdnVendorID.Value != string.Empty)
                objProp_User.Vendor = Convert.ToInt32(hdnVendorID.Value);
            string msg = string.Empty;
            if (hdnStaxAction.Value == "0")
            {
                objBL_User.AddSalesTax(objProp_User);
                msg = "Added";
            }
            else
            {
                objBL_User.UpdateSalesTax(objProp_User);
                msg = "Updated";
            }


            //foreach (GridDataItem di in RadGrid_SalesTax.SelectedItems)
            //{
            //    IsAddEdit = true;
            //    TableCell cell = di["chkSelect"];
            //    CheckBox chkSelect = (CheckBox)cell.Controls[0];
            //    if (chkSelect.Checked == true)
            //    {
            //        objBL_User.UpdateSalesTax(objProp_User);
            //        msg = "Updated";
            //    }
            //}
            //if (!IsAddEdit)
            //{
            //    objBL_User.AddSalesTax(objProp_User);
            //    msg = "Added";
            //}
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseSalesTaxWindow();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddSalesTaxtype", "noty({text: 'Sales Tax " + msg + " successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            FillSalesTax();
            RadGrid_SalesTax.Rebind();
        }
        catch (Exception ex)
        {

            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddSaletype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    protected void lnkDeleteSales_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_SalesTax.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                Label lblId = (Label)di.FindControl("lblId");
                if (chkSelect.Checked == true)
                {
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.SalesTax = lblId.Text;

                    objBL_User.DeleteSalesTax(objProp_User);
                    FillSalesTax();
                    RadGrid_SalesTax.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddSalestax", "noty({text: 'Sales Tax " + lblId.Text + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningSalestax", "noty({text: 'Please select Sales Tax to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelSalestax", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkbtnSaveBillingCode_Click(object sender, EventArgs e)
    {        
        try
        {
            int DefaultBillingCode = 0;
            string DefaultBillingCodeDesc = "";
            if (ddlDefaultBillingCode.SelectedValue != "0") { DefaultBillingCode = Convert.ToInt32(ddlDefaultBillingCode.SelectedValue); }
            if (ddlDefaultBillingCode.SelectedValue != "0") { DefaultBillingCodeDesc = ddlDefaultBillingCode.SelectedItem.Text.ToString(); }

            new BL_BillCodes().UpdateDefaultBillingCode(Session["config"].ToString(), DefaultBillingCode, DefaultBillingCodeDesc);
            
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelSalestax", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Billing Code
    private void FillBillCodes()
    {
        DataSet ds = new DataSet();
        DataTable dtFinal = new DataTable();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.Type = "1";
        ds = new BL_BillCodes().getBillCodes(objProp_User);
        RadGrid_BillCodes.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_BillCodes.DataSource = ds.Tables[0];
        Session["BillCodeSrch"] = ds.Tables[0];
    }
    bool isGroupingBillCodes = false;
    public bool ShouldApplySortFilterBillCodes()
    {
        return RadGrid_BillCodes.MasterTableView.FilterExpression != "" ||
            (RadGrid_BillCodes.MasterTableView.GroupByExpressions.Count > 0 || isGroupingBillCodes) ||
            RadGrid_BillCodes.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_BillCodes_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_BillCodes.AllowCustomPaging = !ShouldApplySortFilterBillCodes();
        FillBillCodes();
    }
    protected void lnkDeleteBillingCodes_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_BillCodes.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                Label lblBillID = (Label)di.FindControl("lblIndexID");


                if (chkSelect.Checked == true)
                {
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.BillCode = Convert.ToInt32(lblBillID.Text);

                    new BL_BillCodes().DeleteBillingCode(objProp_User);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Billing Code deleted successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);

                    FillBillCodes();
                    RadGrid_BillCodes.Rebind();

                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningbillcodes", "noty({text: 'Please select Billing Code to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            }

        }

        catch (Exception ex)
        {
            if (ex.Message == "This is default billing code. You cannot delete!")
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningbillcodes", "noty({text: 'This is default billing code. You cannot delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            }
            else if (ex.Message == "This billing code is in use and cannot be deleted!")
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningbillcodes", "noty({text: 'This billing code is in use and cannot be deleted!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            }
            else
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelbillcodes", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            }

        }
    }
    protected void lnkBillingCodesSave_Click(object sender, EventArgs e)
    {
        Boolean changeDefault = false;
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.ContactName = txtBillCode.Text;
        //objProp_User.SalesDescription = txtBillDesc.Text.Replace("\n", "<br /> \r\n").Replace("'", "''");
        //objProp_User.SalesDescription = txtBillDesc.Text.Replace("\n", " \r\n").Replace("'", "''");
        objProp_User.SalesDescription = txtBillDesc.Text;
        objProp_User.CatStatus = Convert.ToInt32(ddlStatus.SelectedValue);
        objProp_User.Balance = Convert.ToDouble(txtBillBal.Text);
        objProp_User.Measure = txtBillMeasure.Text;
        // objProp_User.Remarks = txtBillRemarks.Text.Replace("\n", "<br /> \r\n").Replace("'", "''");
        //objProp_User.Remarks = txtBillRemarks.Text.Replace("\n", " \r\n").Replace("'", "''");
        objProp_User.Remarks = txtBillRemarks.Text;
        objProp_User.Type = "1";
        if (hdnPatientId.Value != string.Empty)
            objProp_User.sAcct = Convert.ToInt32(hdnPatientId.Value);
        else
            objProp_User.sAcct = 0;
        if (hdnAddEdit.Value == "0")
        {
            objBL_User.AddBillCode(objProp_User);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Billing Code added successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);

        }
        else if (hdnAddEdit.Value == "1")
        {


            objProp_User.BillCode = Convert.ToInt32(hdnBillID.Value);
            if (objProp_User.ContactName.ToLower() != hdnBillName.Value.ToLower())
            {
                if (hdnBillName.Value.ToLower() == "recurring" || hdnBillName.Value.ToLower() == "time spent" || hdnBillName.Value.ToLower() == "mileage" || hdnBillName.Value.ToLower() == "expenses")
                {
                    objProp_User.ContactName = hdnBillName.Value;
                    changeDefault = true;
                }
            }

            objBL_User.UpdateBillCode(objProp_User);
            if (changeDefault)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'The billing code name cannot be edited!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Billing Code updated successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            }


        }
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseBillingCodeWindow();", true);
        objProp_User.Type = null;
        FillBillCodes();
        RadGrid_BillCodes.Rebind();

    }

    #endregion


    #region Lead Type
    private void FillProspectType()
    {
        objCustomer.ConnConfig = Session["config"].ToString();

        DataSet ds = new DataSet();
        ds = objBL_Customer.getProspectType(objCustomer);
        RadGrid_LeadType.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_LeadType.DataSource = ds.Tables[0];
    }
    bool isGroupingLeadType = false;
    public bool ShouldApplySortFilterLeadType()
    {
        return RadGrid_LeadType.MasterTableView.FilterExpression != "" ||
            (RadGrid_LeadType.MasterTableView.GroupByExpressions.Count > 0 || isGroupingLeadType) ||
            RadGrid_LeadType.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_LeadType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_LeadType.AllowCustomPaging = !ShouldApplySortFilterLeadType();
        FillProspectType();
    }
    protected void lnkProsSave_Click(object sender, EventArgs e)
    {
        IsAddEdit = false;
        try
        {
            objCustomer.ConnConfig = Session["config"].ToString();
            objCustomer.Type = txtProsType.Text;
            objCustomer.Remarks = txtProsRemarks.Text;

            string msg = string.Empty;
            foreach (GridDataItem di in RadGrid_LeadType.SelectedItems)
            {
                IsAddEdit = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                if (chkSelect.Checked == true)
                {
                    objCustomer.Mode = 1;
                    objBL_Customer.AddProspectType(objCustomer);
                    msg = "Updated";
                }
            }
            if (!IsAddEdit)
            {
                objCustomer.Mode = 0;
                objBL_Customer.AddProspectType(objCustomer);
                msg = "Added";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseLeadTypeWindow();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddCusttype", "noty({text: 'Lead type " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            FillProspectType();
            RadGrid_LeadType.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrPrtype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkDelPrType_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_LeadType.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                string lblType = di["lblType"].Text.Trim();

                if (chkSelect.Checked == true)
                {
                    objCustomer.ConnConfig = Session["config"].ToString();
                    objCustomer.Type = lblType;
                    objCustomer.Mode = 2;
                    objBL_Customer.AddProspectType(objCustomer);
                    FillProspectType();
                    RadGrid_LeadType.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddPrtype", "noty({text: 'Lead Type " + lblType + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningPrtype", "noty({text: 'Please select Lead Type to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelPrtype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion
    #region Source Sale
    private void FillSource()
    {
        objCustomer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Customer.getSourceCount(objCustomer);
        RadGrid_SourceSale.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_SourceSale.DataSource = ds.Tables[0];
    }
    bool isGroupingSourceSale = false;
    public bool ShouldApplySortFilterSourceSale()
    {
        return RadGrid_SourceSale.MasterTableView.FilterExpression != "" ||
            (RadGrid_SourceSale.MasterTableView.GroupByExpressions.Count > 0 || isGroupingSourceSale) ||
            RadGrid_SourceSale.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_SourceSale_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_SourceSale.AllowCustomPaging = !ShouldApplySortFilterSourceSale();
        FillSource();
    }
    protected void lnkSourceSave_Click(object sender, EventArgs e)
    {
        IsAddEdit = false;
        try
        {
            objCustomer.ConnConfig = Session["config"].ToString();
            objCustomer.Type = txtSourceType.Text;
            objCustomer.SourceDescription = txtSourceDescription.Text;
            objCustomer.OldSourceDescription = Convert.ToString(hdnSourceSale.Value);
            string msg = string.Empty;
            foreach (GridDataItem di in RadGrid_SourceSale.SelectedItems)
            {
                IsAddEdit = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                if (chkSelect.Checked == true)
                {
                    objCustomer.Mode = 1;
                    objBL_Customer.AddSaleSource(objCustomer);
                    msg = "Updated";
                }
            }
            if (!IsAddEdit)
            {
                objCustomer.Mode = 0;
                objBL_Customer.AddSaleSource(objCustomer);
                msg = "Added";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseSourceSaleWindow();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddCusttype", "noty({text: 'Source " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            FillSource();
            RadGrid_SourceSale.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrPrtype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkDeleteSource_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_SourceSale.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                string lblfDesc = di["lblfDesc"].Text.Trim();
                if (chkSelect.Checked == true)
                {
                    objCustomer.ConnConfig = Session["config"].ToString();
                    objCustomer.SourceDescription = lblfDesc;
                    objCustomer.Mode = 2;
                    objBL_Customer.AddSaleSource(objCustomer);
                    FillSource();
                    RadGrid_SourceSale.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddSource", "noty({text: 'Source " + lblfDesc + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningSource", "noty({text: 'Please select Source to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelSource", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Stage Sale
    private void FillStages()
    {
        objCustomer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Customer.getStages(objCustomer);
        RadGrid_StageSale.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_StageSale.DataSource = ds.Tables[0];
        lblStageHeader.InnerText = "Stage";
        lblEditStage.InnerText = "Stage";
        lnkEditStageHeader.Text = "Stage";
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            lblStageHeader.InnerText = ds.Tables[0].Rows[0]["Label"].ToString();
            lblEditStage.InnerText = ds.Tables[0].Rows[0]["Label"].ToString();
            lnkEditStageHeader.Text = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }
    bool isGroupingStageSale = false;
    public bool ShouldApplySortStageSale()
    {
        return RadGrid_StageSale.MasterTableView.FilterExpression != "" ||
            (RadGrid_StageSale.MasterTableView.GroupByExpressions.Count > 0 || isGroupingStageSale) ||
            RadGrid_StageSale.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_StageSale_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_StageSale.AllowCustomPaging = !ShouldApplySortStageSale();
        FillStages();
    }

    protected void RadGrid_StageSale_PreRender(object sender, EventArgs e)
    {
        foreach (GridDataItem gr in RadGrid_StageSale.Items)
        {
            Panel txtChartColors = (Panel)gr.FindControl("txtChartColors");
            Label lblChartColor = (Label)gr.FindControl("lblChartColor");
            txtChartColors.Style.Add("background-color", "#" + lblChartColor.Text);
        }
    }
    protected void lnkUpdateStageHeader_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtEditStage.Text != "")
            {
                objCustomer.ConnConfig = Session["config"].ToString();
                objCustomer.HeaderStage = txtEditStage.Text;
                objBL_Customer.UpdateStageHeader(objCustomer);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseStageSaleWindow('Header');", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccStage", "noty({text: 'Header Updated Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                lblStageHeader.InnerText = txtEditStage.Text;
                lnkEditStageHeader.Text = txtEditStage.Text;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddStage", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void lnkSaveStage_Click(object sender, EventArgs e)
    {
        IsAddEdit = false;
        string StageId = "0";
        int Count = 0;
        try
        {
            #region code to push data to db
            objCustomer.ConnConfig = Session["config"].ToString();
            string msg = string.Empty;
            foreach (GridDataItem di in RadGrid_StageSale.SelectedItems)
            {
                IsAddEdit = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                StageId = di["lblStageId"].Text.Trim();
                Count = Convert.ToInt32(di["lblCount"].Text);
                if (chkSelect.Checked == true)
                {
                    objCustomer.Mode = 1;

                    msg = "Updated";
                }
            }
            if (!IsAddEdit)
            {
                objCustomer.Mode = 0;

                msg = "Added";
            }


            objCustomer.Stage = new Stage()
            {
                ID = StageId,
                Description = txtDescription.Text,
                Type = ddlTypeStage.SelectedValue,
                Probability = txtProbability.Text,
                ChartColor = ColorTranslator.ToHtml(txtChartColor.SelectedColor).Replace("#", ""),
                Count = Count,
                Label = lnkEditStageHeader.Text
            };

            objBL_Customer.AddStages(objCustomer);
            #endregion
            FillStages();
            RadGrid_StageSale.Rebind();

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseStageSaleWindow('Add');", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddCusttype", "noty({text: 'Stage " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrPrStageSale", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkDelStage_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_StageSale.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                string StageId = di["lblStageId"].Text.Trim();
                if (chkSelect.Checked == true)
                {
                    #region Add code to delete stage
                    objCustomer.ConnConfig = Session["config"].ToString();
                    objCustomer.Mode = 2;
                    objCustomer.Stage = new Stage()
                    {
                        ID = StageId
                    };
                    objBL_Customer.DeleteStages(objCustomer);
                    #endregion
                    FillStages();
                    RadGrid_StageSale.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddStageSale", "noty({text: 'Stage " + StageId + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningStageSale", "noty({text: 'Please select Stage to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelPrtype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }


    #endregion

    #region BusinessType
    private void FillBT()
    {
        objCustomer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Customer.getBT(objCustomer);
        RadGrid_BusinessType.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_BusinessType.DataSource = ds.Tables[0];
        lblBTHeader.InnerText = "Business Type";
        lnkEditBTHeader.Text = "Business Type";
        lblEditBTHeader.InnerText = "Business Type";
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            lblBTHeader.InnerText = ds.Tables[0].Rows[0]["Label"].ToString();
            lblEditBTHeader.InnerText = ds.Tables[0].Rows[0]["Label"].ToString();
            lnkEditBTHeader.Text = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }
    bool isGroupingBusinessType = false;
    public bool ShouldApplySortBusinessType()
    {
        return RadGrid_BusinessType.MasterTableView.FilterExpression != "" ||
            (RadGrid_BusinessType.MasterTableView.GroupByExpressions.Count > 0 || isGroupingBusinessType) ||
            RadGrid_BusinessType.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_BusinessType_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_BusinessType.AllowCustomPaging = !ShouldApplySortBusinessType();
        FillBT();
    }
    protected void lnkSaveHeaderBT_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtEditBTHeader.Text != "")
            {
                objCustomer.ConnConfig = Session["config"].ToString();
                objCustomer.HeaderBT = txtEditBTHeader.Text;
                objBL_Customer.UpdateBTHeader(objCustomer);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseBusinessTypeWindow('Header');", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccStage", "noty({text: 'Header Updated Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                lblBTHeader.InnerText = txtEditBTHeader.Text;
                lnkEditBTHeader.Text = txtEditBTHeader.Text;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddStage", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    protected void lnkSaveBT_Click(object sender, EventArgs e)
    {
        IsAddEdit = false;
        try
        {
            #region code to push data to db
            objCustomer.ConnConfig = Session["config"].ToString();
            int id = 0;
            int count = 0;
            string msg = string.Empty;
            String BusinessTypeID = "0";
            BusinessTypeID = hdnBusinessTypeID.Value;
            //foreach (GridDataItem di in RadGrid_BusinessType.SelectedItems)
            //{
            //    IsAddEdit = true;
            //    TableCell cell = di["chkSelect"];
            //    CheckBox chkSelect = (CheckBox)cell.Controls[0];
            //    id = Convert.ToInt32(di["lblBTId"].Text);
            //    count = Convert.ToInt32(di["lblCount"].Text);
            //    if (chkSelect.Checked == true)
            //    {
            //        objCustomer.Mode = 1;
            //        msg = "Updated";
            //    }
            //}
            //if (!IsAddEdit)
            //{
            //    objCustomer.Mode = 0;
            //    msg = "Added";
            //}
            objCustomer.Mode = 1;
            if (BusinessTypeID == "0")
            {
                objCustomer.Mode = 0;
                msg = "Added";
            }
            objCustomer.BT = new BT()
            {
                ID = Convert.ToInt32(BusinessTypeID),
                Description = txtBTDescription.Text,
                Count = count,
                Label = lnkEditBTHeader.Text
            };

            objBL_Customer.AddBT(objCustomer);
            #endregion

            FillBT();
            RadGrid_BusinessType.Rebind();

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseBusinessTypeWindow('Add');", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddBusiness", "noty({text: 'Business " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            String type = "error";
            if (str.Contains("This type is in use and cannot be edited.") || str.Contains("This type already exists"))
            {
                type = "warning";

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddCusttype", "noty({text: '" + str + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
            else
            {

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrPrBusiness", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }

        }
    }

    protected void lnkDelBT_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_BusinessType.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                string lblBTId = di["lblBTId"].Text.Trim();
                if (chkSelect.Checked == true)
                {
                    #region Add code to delete BT
                    objCustomer.ConnConfig = Session["config"].ToString();
                    objCustomer.Mode = 2;
                    objCustomer.BT = new BT()
                    {
                        ID = Convert.ToInt32(lblBTId)
                    };
                    objBL_Customer.DeleteBT(objCustomer);
                    #endregion
                    FillBT();
                    RadGrid_BusinessType.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddBusinessType", "noty({text: 'Business Type " + lblBTId + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningBusinessType", "noty({text: 'Please select Business Type to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelBusinessType", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Product Service
    private void FillProductService()
    {
        objCustomer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Customer.getService(objCustomer);
        RadGrid_ProductService.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_ProductService.DataSource = ds.Tables[0];
        lblPServiceHeader.InnerText = "Products";
        lblEditServicesHeader.InnerText = "Products";
        lnkEditServicesHeader.Text = "Products";
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            lblPServiceHeader.InnerText = ds.Tables[0].Rows[0]["Label"].ToString();
            lblEditServicesHeader.InnerText = ds.Tables[0].Rows[0]["Label"].ToString();
            lnkEditServicesHeader.Text = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }
    bool isGroupingProductService = false;
    public bool ShouldApplySortProductService()
    {
        return RadGrid_ProductService.MasterTableView.FilterExpression != "" ||
            (RadGrid_ProductService.MasterTableView.GroupByExpressions.Count > 0 || isGroupingProductService) ||
            RadGrid_ProductService.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_ProductService_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_ProductService.AllowCustomPaging = !ShouldApplySortProductService();
        FillProductService();
    }
    protected void lnkSaveHeaderServices_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtEditServicesHeader.Text != "")
            {
                objCustomer.ConnConfig = Session["config"].ToString();
                objCustomer.HeaderServices = txtEditServicesHeader.Text;
                objBL_Customer.UpdateServicesHeader(objCustomer);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseProductServiceWindow('Header');", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccService", "noty({text: 'Header Updated Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                lblPServiceHeader.InnerText = txtEditServicesHeader.Text;
                lnkEditServicesHeader.Text = txtEditServicesHeader.Text;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddService", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    protected void lnkSavePService_Click(object sender, EventArgs e)
    {
        IsAddEdit = false;
        string PServiceId = "0";
        int Count = 0;
        try
        {
            #region code to push data to db
            objCustomer.ConnConfig = Session["config"].ToString();
            string msg = string.Empty;
            foreach (GridDataItem di in RadGrid_ProductService.SelectedItems)
            {
                IsAddEdit = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                PServiceId = di["lblPServiceId"].Text.Trim();
                Count = Convert.ToInt32(di["lblCount"].Text);
                if (chkSelect.Checked == true)
                {
                    objCustomer.Mode = 1;

                    msg = "Updated";
                }
            }
            if (!IsAddEdit)
            {
                objCustomer.Mode = 0;

                msg = "Added";
            }
            objCustomer.Service = new Service()
            {
                ID = PServiceId,
                Description = txtPServiceDescription.Text,
                Count = Count,
                Label = lnkEditServicesHeader.Text
            };
            objBL_Customer.AddService(objCustomer);
            #endregion

            FillProductService();
            RadGrid_ProductService.Rebind();
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseProductServiceWindow('Add');", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddService", "noty({text: 'Product " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            String type = "Error";

            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            if (str.Contains("Already exists"))
            {
                type = "warning";
            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrService", "noty({text: '" + str + "', dismissQueue: true,  type : '" + type + "', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkDelPService_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_ProductService.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                string PServiceId = di["lblPServiceId"].Text.Trim();
                if (chkSelect.Checked == true)
                {
                    #region Add code to delete BT
                    objCustomer.ConnConfig = Session["config"].ToString();
                    objCustomer.Mode = 2;
                    objCustomer.Service = new Service()
                    {
                        ID = PServiceId
                    };
                    objBL_Customer.DeleteService(objCustomer);
                    #endregion
                    FillProductService();
                    RadGrid_ProductService.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProduct", "noty({text: 'Product " + PServiceId + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningProduct", "noty({text: 'Please select Product to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelProduct", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #endregion

    #region Terms And Condition
    private void FillTC()
    {
        try
        {
            objProp_User.ConnConfig = Session["config"].ToString();
            DataSet ds = objBL_User.GetAllTc(objProp_User);
            RadGrid_TandC.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGrid_TandC.DataSource = ds.Tables[0];
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    bool isGroupingTandC = false;
    public bool ShouldApplySortTandC()
    {
        return RadGrid_TandC.MasterTableView.FilterExpression != "" ||
            (RadGrid_TandC.MasterTableView.GroupByExpressions.Count > 0 || isGroupingTandC) ||
            RadGrid_TandC.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_TandC_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_TandC.AllowCustomPaging = !ShouldApplySortTandC();
        FillTC();
    }


    protected void lnkTcSave_Click(object sender, EventArgs e)
    {
        try
        {
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.PageID = Convert.ToInt32(hdnPageId.Value);
            objProp_User.TermsConditions = txtTc.Text;
            bool isExist;
            string msg = string.Empty;
            if (Convert.ToString(hdnAddEdit.Value) == "1")
            {
                objProp_User.TermsID = Convert.ToInt32(hdnTermID.Value);

                isExist = objBL_User.IsExistPageForUpdate(objProp_User);
                if (!isExist)
                {
                    objBL_User.UpdateTerms(objProp_User);
                    msg = "Updated";
                }
            }
            else
            {
                isExist = objBL_User.IsExistPage(objProp_User);
                if (!isExist)
                {
                    objBL_User.AddTerms(objProp_User);
                    msg = "Added";
                }
            }
            if (isExist)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddTandC", "noty({text: 'Terms and conditions of this page  already exists!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseTandCWindow();", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddTandC", "noty({text: 'Terms and conditions of this page " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                FillTC();
                RadGrid_TandC.Rebind();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelTandC", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkDeleteTc_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_TandC.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                Label lblTermID = (Label)di.FindControl("lblTermID");

                if (chkSelect.Checked == true)
                {
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.TermsID = Convert.ToInt32(lblTermID.Text);

                    objBL_User.DeleteTermsCondition(objProp_User);
                    FillTC();
                    RadGrid_TandC.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddTandC", "noty({text: 'Terms and conditions of this page Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningTandC", "noty({text: 'Please select Terms and conditions to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelTandC", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Warehouse Inventory
    private void FillInventoryWarehouse()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
        #region Company Check
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            objProp_User.EN = 1;
        }
        else
        {
            objProp_User.EN = 0;
        }
        #endregion
        ds = objBL_User.GetInventoryWarehouse(objProp_User);

        RadGrid_Warehouse.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_Warehouse.DataSource = ds.Tables[0];
    }
    bool isGroupingWarehouse = false;
    public bool ShouldApplySortWarehouse()
    {
        return RadGrid_Warehouse.MasterTableView.FilterExpression != "" ||
            (RadGrid_Warehouse.MasterTableView.GroupByExpressions.Count > 0 || isGroupingWarehouse) ||
            RadGrid_Warehouse.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_Warehouse_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_Warehouse.AllowCustomPaging = !ShouldApplySortWarehouse();
        FillInventoryWarehouse();
    }
    protected void lnkAddInventoryWarehouse_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddInventoryWarehouse.aspx");
    }
    protected void lnkEditInventoryWarehouse_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_Warehouse.SelectedItems)
        {
            TableCell cell = di["chkSelect"];
            CheckBox chkSelect = (CheckBox)cell.Controls[0];
            Label lblID = (Label)di.FindControl("lblinvId");
            if (chkSelect.Checked == true)
            {
                Response.Redirect("AddInventoryWarehouse.aspx?uid=" + lblID.Text);
            }
        }
    }
    protected void lnkDeleteInventoryWarehouse_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        foreach (GridDataItem di in RadGrid_Warehouse.SelectedItems)
        {
            IsDelete = true;
            TableCell cell = di["chkSelect"];
            CheckBox chkSelect = (CheckBox)cell.Controls[0];
            Label lblUserID = (Label)di.FindControl("lblinvId");

            if (chkSelect.Checked == true)
            {
                DeleteInventoryWarehouse(lblUserID.Text);
            }
        }
        if (!IsDelete)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningWarehouse", "noty({text: 'Please select WareHouse to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    private void DeleteInventoryWarehouse(string WarehouseID)
    {
        objProp_User.WarehouseID = WarehouseID;
        objProp_User.ConnConfig = Session["config"].ToString();
        try
        {
            objBL_User.DeleteInventoryWareHouse(objProp_User);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccWarehouse", "noty({text: 'Warehouse deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            FillInventoryWarehouse();
            RadGrid_Warehouse.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Commodity Inventory
    private void FillInventoryCommodity()
    {
        DataSet ds = new DataSet();
        Commodity objProp_Commodity = new Commodity();
        BL_Inventory objBL_Inventory = new BL_Inventory();
        objProp_Commodity.ConnConfig = Session["config"].ToString();

        ds = objBL_Inventory.ReadAllCommodity(objProp_Commodity);

        RadGrid_CommodityInventory.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_CommodityInventory.DataSource = ds.Tables[0];
    }
    bool isGroupingCommodityInventory = false;
    public bool ShouldApplySortFilterCommodityInventory()
    {
        return RadGrid_CommodityInventory.MasterTableView.FilterExpression != "" ||
            (RadGrid_CommodityInventory.MasterTableView.GroupByExpressions.Count > 0 || isGroupingCommodityInventory) ||
            RadGrid_CommodityInventory.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_CommodityInventory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_CommodityInventory.AllowCustomPaging = !ShouldApplySortFilterCommodityInventory();
        FillInventoryCommodity();
    }
    protected void lnksavecommodity_Click(object sender, EventArgs e)
    {
        IsAddEdit = false;
        try
        {
            Commodity objProp_Commodity = new Commodity();
            BL_Inventory objBL_Inventory = new BL_Inventory();
            objProp_Commodity.ConnConfig = Session["config"].ToString();
            objProp_Commodity.Code = txtCommodityCode.Text;
            objProp_Commodity.Desc = txtCommodityName.Text;
            string msg = string.Empty;
            foreach (GridDataItem di in RadGrid_CommodityInventory.SelectedItems)
            {
                IsAddEdit = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                if (chkSelect.Checked == true)
                {
                    objProp_Commodity.ID = Convert.ToInt32(hdncommodityId.Value.Trim());
                    objBL_Inventory.UpdateCommodity(objProp_Commodity);
                    msg = "Updated";
                }
            }
            if (!IsAddEdit)
            {
                objBL_Inventory.CreateCommodity(objProp_Commodity);
                msg = "Added";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseCommodityInventoryWindow();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: 'Commodity " + msg + " successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            FillInventoryCommodity();
            RadGrid_CommodityInventory.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddWarehousetype1", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void lnkDeleteInventoryCommodity_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        foreach (GridDataItem di in RadGrid_CommodityInventory.SelectedItems)
        {
            IsDelete = true;
            TableCell cell = di["chkSelect"];
            CheckBox chkSelect = (CheckBox)cell.Controls[0];
            Label lblcommodityId = (Label)di.FindControl("lblcommodityId");
            Label lblinvcommoditycount = (Label)di.FindControl("lblinvcommoditycount");
            if (chkSelect.Checked == true)
            {
                if (Convert.ToInt32(lblinvcommoditycount.Text) == 0)
                {
                    DeleteInventoryCommodity(lblcommodityId.Text);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningWarehouse", "noty({text: 'Commodity is in use, Can not delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
        }
        if (!IsDelete)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningWarehouse", "noty({text: 'Please select Commodity to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    private void DeleteInventoryCommodity(string CommodityID)
    {

        try
        {
            objBL_Inventory.DeleteInventoryCommodity(Session["config"].ToString(), CommodityID);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccWarehouse", "noty({text: 'Commodity deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            FillInventoryCommodity();
            RadGrid_CommodityInventory.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Costing Method
    private void FillInventoryCostTypes()
    {
        DataSet ds = new DataSet();
        BusinessEntity.Inventory objProp_inv = new BusinessEntity.Inventory();
        BL_Inventory objBL_Inventory = new BL_Inventory();
        objProp_inv.ConnConfig = Session["config"].ToString();


        ds = objBL_Inventory.ReadCostTypes(objProp_inv);

        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlcosting.Items.Add(new ListItem(Convert.ToString(ds.Tables[0].Rows[i]["CostTypes"]), Convert.ToString(ds.Tables[0].Rows[i]["ID"])));
                    if (Convert.ToBoolean(ds.Tables[0].Rows[i]["InUse"]))
                        ddlcosting.SelectedValue = Convert.ToString(ds.Tables[0].Rows[i]["ID"]);
                }
            }
        }
    }
    protected void lnksaveinvcosting_Click(object sender, EventArgs e)
    {
        try
        {
            BusinessEntity.Inventory objProp_inv = new BusinessEntity.Inventory();
            BL_Inventory objBL_Inventory = new BL_Inventory();
            objProp_inv.ConnConfig = Session["config"].ToString();
            objProp_inv.ID = Convert.ToInt32(ddlcosting.SelectedValue);
            objProp_inv.InUse = 1;

            objBL_Inventory.UseCostingType(objProp_inv);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: 'Costing updated successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddWarehousetype1", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    #endregion
    #region Category Inventory
    private void FillInventorycategoryType()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();

        ds = objBL_User.GetInventoryCategory(objProp_User);

        RadGrid_CategoryInventory.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_CategoryInventory.DataSource = ds.Tables[0];
    }
    bool isGroupingCategoryInventory = false;
    public bool ShouldApplySortFilterCategoryInventory()
    {
        return RadGrid_CategoryInventory.MasterTableView.FilterExpression != "" ||
            (RadGrid_CategoryInventory.MasterTableView.GroupByExpressions.Count > 0 || isGroupingCategoryInventory) ||
            RadGrid_CategoryInventory.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_CategoryInventory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_CategoryInventory.AllowCustomPaging = !ShouldApplySortFilterCategoryInventory();
        FillInventorycategoryType();
    }
    protected void lnkSaveInventoryCategory_Click(object sender, EventArgs e)
    {
        IsAddEdit = false;
        try
        {
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.CategoryName = txtCategoryName.Text;
            objProp_User.CategoryCount = 0;
            objProp_User.CategoryRemarks = txtCategoryRemarks.Text;
            objProp_User.Type = "1";//This is inventory type warehouse
            string msg = string.Empty;
            foreach (GridDataItem di in RadGrid_CategoryInventory.SelectedItems)
            {
                IsAddEdit = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                if (chkSelect.Checked == true)
                {
                    objProp_User.CategoryTypeID = Convert.ToInt32(hdnAddEdit.Value);
                    objBL_User.UpdateInventoryCategory(objProp_User);
                    msg = "Updated";
                }
            }
            if (!IsAddEdit)
            {
                objBL_User.CreateInventoryCategory(objProp_User);
                msg = "Added";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseCategoryInventoryWindow();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddCatgtype", "noty({text: 'Category " + msg + " successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            FillInventorycategoryType();
            RadGrid_CategoryInventory.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string type = "error";
            if (str.Contains("Category Type already exists, please use different type"))
            {
                type = "warning";

            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddEqupClassification", "noty({text: '" + str + "', dismissQueue: true,  type : '" + type + "', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);



        }
    }
    protected void lnkDeleteInventoryCategory_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_CategoryInventory.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                string lblinvCategoryId = di["lblinvCategoryId"].Text.Trim();
                Label lblinvCategoryCount = (Label)di.FindControl("lblinvCategoryCount");
                if (chkSelect.Checked == true)
                {
                    if (Convert.ToInt32(lblinvCategoryCount.Text) == 0)
                    {
                        objProp_User.ConnConfig = Session["config"].ToString();
                        objProp_User.CategoryTypeID = Convert.ToInt32(lblinvCategoryId);

                        objBL_User.DeleteInventoryCategory(objProp_User);
                        FillInventorycategoryType();
                        RadGrid_CategoryInventory.Rebind();
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddCatgtype", "noty({text: 'Category Type " + lblinvCategoryId + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningWarehouse", "noty({text: 'Category is in use, Can not delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }
                }

            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningCatgtype", "noty({text: 'Please select Category Type to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelCust", "noty({text: '" + str + "', dismissQueue: true, type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Department Project
    private void FillDepartment()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getDepartment(objProp_User);
        RadGrid_Department.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_Department.DataSource = ds.Tables[0];
    }

    bool isGroupingDepartment = false;
    public bool ShouldApplySortFilterDepartment()
    {
        return RadGrid_Department.MasterTableView.FilterExpression != "" ||
            (RadGrid_Department.MasterTableView.GroupByExpressions.Count > 0 || isGroupingDepartment) ||
            RadGrid_Department.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_Department_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_Department.AllowCustomPaging = !ShouldApplySortFilterDepartment();
        FillDepartment();
    }
    protected void lnkDepartSave_Click(object sender, EventArgs e)
    {
        IsAddEdit = false;
        try
        {
            objProp_User.ConnConfig = Session["config"].ToString();
            if (lblRecurring.Text != string.Empty)
            {
                objProp_User.Type = txtDepartment.Text + ":" + lblRecurring.Text;
            }
            else
            {
                objProp_User.Type = txtDepartment.Text;
            }
            objProp_User.Remarks = txtDepartRemarks.Text;
            objProp_User.Default = Convert.ToInt32(chkDefault.Checked);
            string msg = string.Empty;
            foreach (GridDataItem di in RadGrid_Department.SelectedItems)
            {
                IsAddEdit = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                if (chkSelect.Checked == true)
                {
                    objProp_User.JobtypeID = Convert.ToInt32(hdnDeptID.Value);
                    objBL_User.UpdateDepartment(objProp_User);
                    msg = "Updated";
                }
            }
            if (!IsAddEdit)
            {
                objBL_User.AddDepartment(objProp_User);
                msg = "Added";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseDepartmentWindow();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddDeparmenttype", "noty({text: 'Department " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            FillDepartment();
            RadGrid_Department.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddDepttype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    protected void lnkDeleteDepart_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_Department.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                Label lblDepId = (Label)di.FindControl("lblDepId");

                if (chkSelect.Checked == true)
                {
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.DepartmentID = Convert.ToInt32(lblDepId.Text);

                    objBL_User.DeleteDepartment(objProp_User);
                    FillDepartment();
                    RadGrid_Department.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddCatgtype", "noty({text: 'Department  " + lblDepId.Text + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningDepartment", "noty({text: 'Please select Department to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDeldep", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }


    #endregion

    #region Email Setting
    protected void ddlEmailSetup_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadGrid_TicketInvoice.Visible = true;
        lnkBtnAddParameter.Visible = true;
        ViewState["EmailID"] = "0";
        if (ddlEmailSetup.SelectedIndex == 1)
        {
            FillTicketParameter();
            lnkBtnAddParameter.Text = "Add Ticket";
            RadGrid_TicketInvoice.DataSource = FillDefaultTicketParameter();
            RadGrid_TicketInvoice.Rebind();
        }
        else if (ddlEmailSetup.SelectedIndex == 2)
        {

            FillInvoiceParameter();
            lnkBtnAddParameter.Text = "Add Invoice";
            RadGrid_TicketInvoice.DataSource = FillDefaultInvoiceParameter();
            RadGrid_TicketInvoice.Rebind();
        }
        else
        {
            txtBody.Content = string.Empty;
            txtSubject.Text = string.Empty;
            RadGrid_TicketInvoice.Visible = false;
            lnkBtnAddParameter.Visible = false;
        }
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "Materialize.updateTextFields();", true);
    }
    public DataTable FillDefaultTicketParameter()
    {

        DataTable dt = new DataTable();
        dt.Columns.AddRange(new DataColumn[2] { new DataColumn("ID", typeof(int)),
                    new DataColumn("Parameter",typeof(string)) });
        dt.Rows.Add(1, "@Worker");
        dt.Rows.Add(2, "@TicketID");
        dt.Rows.Add(3, "@Customer");
        dt.Rows.Add(4, "@Location");
        dt.Rows.Add(5, "@City");
        dt.Rows.Add(6, "@Address");
        dt.Rows.Add(7, "@Phone");
        dt.Rows.Add(8, "@ScheduledDate");
        dt.Rows.Add(9, "@Category");
        dt.Rows.Add(10, "@Equipment");
        dt.Rows.Add(11, "@Department");
        dt.Rows.Add(12, "@Caller");
        dt.Rows.Add(13, "@CallerPhone");
        dt.Rows.Add(14, "@Reason");
        return dt;
    }


    protected void RadGrid_TicketInvoice_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
    {
        LoadDataForRadGrid1();
    }

    protected void RadGrid_TicketInvoice_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        LoadDataForRadGrid1();
    }

    protected void RadGrid_TicketInvoice_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        LoadDataForRadGrid1();
    }

    private void LoadDataForRadGrid1()
    {
        RadGrid_TicketInvoice.Visible = true;
        if (ddlEmailSetup.SelectedIndex == 1)
        {
            RadGrid_TicketInvoice.DataSource = FillDefaultTicketParameter();
            RadGrid_TicketInvoice.Rebind();
        }
        else if (ddlEmailSetup.SelectedIndex == 2)
        {
            RadGrid_TicketInvoice.DataSource = FillDefaultInvoiceParameter();
            RadGrid_TicketInvoice.Rebind();
        }
        else
            RadGrid_TicketInvoice.Visible = false;
    }

    protected void RadGrid_TicketInvoice_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_TicketInvoice.Visible = true;
        if (ddlEmailSetup.SelectedIndex == 1)
            RadGrid_TicketInvoice.DataSource = FillDefaultTicketParameter();
        else if (ddlEmailSetup.SelectedIndex == 2)
            RadGrid_TicketInvoice.DataSource = FillDefaultInvoiceParameter();
        else
            RadGrid_TicketInvoice.Visible = false;

    }


    private void FillTicketParameter()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.Type = "Ticket";
        ds = objBL_User.getTicketInvoiceEmail(objProp_User);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["TicktInvoice"] = "1";
            ViewState["EmailID"] = ds.Tables[0].Rows[0]["ID"].ToString();
            txtSubject.Text = ds.Tables[0].Rows[0]["Subject"].ToString();
            txtBody.Content = ds.Tables[0].Rows[0]["Body"].ToString();
        }
        else
        {
            ViewState["TicktInvoice"] = "0";
            string Worker = "@Worker";//ds.Tables[0].Rows[0]["dwork"].ToString();
            string Ticket = "@TicketID";//ds.Tables[0].Rows[0]["ID"].ToString();
            string Customer = "@Customer";
            string LocationID = "@LocationID";
            string Location = "@Location"; //ds.Tables[0].Rows[0]["locname"].ToString();
            string Address = "@Address"; //ds.Tables[0].Rows[0]["address"].ToString();
            string Phone = "@Phone";// ds.Tables[0].Rows[0]["Phone"].ToString();
            string ScheduledDate = "@ScheduledDate";// string.Format("{0:MM/dd/yy hh:mm tt}", ds.Tables[0].Rows[0]["Edate"].ToString());
            string Category = "@Category";// ds.Tables[0].Rows[0]["Cat"].ToString();
            string Equipment = "@Equipment";// ds.Tables[0].Rows[0]["unitname"].ToString();
            string Department = "@Department";
            string Caller = "@Caller";// ds.Tables[0].Rows[0]["who"].ToString();
            string CallerPhone = "@CallerPhone";// ds.Tables[0].Rows[0]["cphone"].ToString();
            string Reason = "@Reason";
            txtSubject.Text = "Ticket # " + Ticket;

            string MailBody;

            MailBody = "Hi " + Worker + ",";
            MailBody += "</br></br>";
            MailBody += "   A ticket with <strong>ID " + Ticket + "</strong> has been assigned to you.";
            MailBody += "</br></br>";
            MailBody += "   Below are the details:";
            MailBody += "</br>";
            MailBody += "   Customer: <strong>" + Customer + "</strong>";
            MailBody += "</br>";
            MailBody += "   Location:<strong> " + Location + "</strong>";
            MailBody += "</br>";
            MailBody += "   Address: <strong>" + Address + "</strong>";
            MailBody += "</br>";
            MailBody += "   Phone: <strong>" + Phone + "</strong>";
            MailBody += "</br>";
            MailBody += "   Scheduled Date: <strong>" + ScheduledDate + "</strong>";
            MailBody += "</br>";
            MailBody += "   Category: <strong>" + Category + "</strong>";
            MailBody += "</br>";
            MailBody += "   Equipment: <strong>" + Equipment + "</strong>";
            MailBody += "</br>";
            MailBody += "   Department: <strong>" + Department + "</strong>";
            MailBody += "</br>";
            MailBody += "   LocationID: <strong>" + LocationID + "</strong>";
            MailBody += "</br>";
            MailBody += "   Caller: <strong>" + Caller + "</strong>";
            MailBody += "</br>";
            MailBody += "   Caller Phone: <strong>" + CallerPhone + "</strong>";
            MailBody += "</br>";
            MailBody += "   Reason for call: <strong>" + Reason + "</strong>";

            txtBody.Content = MailBody;
        }
    }
    public DataTable FillDefaultInvoiceParameter()
    {
        DataTable dt = new DataTable();
        dt.Columns.AddRange(new DataColumn[2] { new DataColumn("ID", typeof(int)),
                    new DataColumn("Parameter",typeof(string)) });
        dt.Rows.Add(1, "@Worker");
        dt.Rows.Add(2, "@InvoiceID");
        dt.Rows.Add(3, "@Customer");
        dt.Rows.Add(4, "@Location");
        dt.Rows.Add(5, "@City");
        dt.Rows.Add(6, "@Address");
        dt.Rows.Add(7, "@Project");
        dt.Rows.Add(8, "@InvoiceDate");
        dt.Rows.Add(9, "@PreTaxAmount");
        dt.Rows.Add(10, "@SalesTax");
        dt.Rows.Add(11, "@InvoiceTotal");
        dt.Rows.Add(12, "@Status");
        dt.Rows.Add(13, "@AmountDue");
        dt.Rows.Add(14, "@Remarks");
        dt.Rows.Add(15, "@Invoice");
        return dt;
    }
    private void FillInvoiceParameter()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.Type = "Invoice";
        ds = objBL_User.getTicketInvoiceEmail(objProp_User);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["TicktInvoice"] = "1";
            ViewState["EmailID"] = ds.Tables[0].Rows[0]["ID"].ToString();
            txtSubject.Text = ds.Tables[0].Rows[0]["Subject"].ToString();
            txtBody.Content = ds.Tables[0].Rows[0]["Body"].ToString();
        }
        else
        {
            ViewState["TicktInvoice"] = "0";
            string Worker = "@Worker";//ds.Tables[0].Rows[0]["dwork"].ToString();
            string Invoice = "@InvoiceID";//ds.Tables[0].Rows[0]["ID"].ToString();
            string Customer = "@Customer";
            string Location = "@Location"; //ds.Tables[0].Rows[0]["locname"].ToString();
            string Address = "@Address"; //ds.Tables[0].Rows[0]["address"].ToString();
            string Project = "@Project";// ds.Tables[0].Rows[0]["Project"].ToString();
            string InvoiceDate = "@InvoiceDate";// string.Format("{0:MM/dd/yy hh:mm tt}", ds.Tables[0].Rows[0]["Edate"].ToString());
            string PreTaxAmount = "@PreTaxAmount";// ds.Tables[0].Rows[0]["PreTaxAmount"].ToString();
            string SalesTax = "@SalesTax";// ds.Tables[0].Rows[0]["fdesc"].ToString();
            string InvoiceTotal = "@InvoiceTotal";// ds.Tables[0].Rows[0]["unitname"].ToString();
            string Status = "@Status";// ds.Tables[0].Rows[0]["who"].ToString();
            string AmountDue = "@AmountDue";// ds.Tables[0].Rows[0]["cphone"].ToString();
            string Remarks = "@Remarks";
            txtSubject.Text = "Invoice # " + Invoice;

            string MailBody;

            MailBody = "Hi " + Customer + ",";
            MailBody += "</br></br>";
            MailBody += "   A InvoiceID<strong> " + Invoice + "</strong>";
            MailBody += "</br></br>";
            MailBody += "   Below are the details:";
            MailBody += "</br>";
            MailBody += "   Customer: <strong>" + Customer + "</strong>";
            MailBody += "</br>";
            MailBody += "   Location:<strong> " + Location + "</strong>";
            MailBody += "</br>";
            MailBody += "   Address: <strong>" + Address + "</strong>";
            MailBody += "</br>";
            MailBody += "   Project: <strong>" + Project + "</strong>";
            MailBody += "</br>";
            MailBody += "   Invoice Date: <strong>" + InvoiceDate + "</strong>";
            MailBody += "</br>";
            MailBody += "   Pre Tax Amount: <strong>" + PreTaxAmount + "</strong>";
            MailBody += "</br>";
            MailBody += "   Sales Tax: <strong>" + SalesTax + "</strong>";
            MailBody += "</br>";
            MailBody += "   Invoice Total: <strong>" + InvoiceTotal + "</strong>";
            MailBody += "</br>";
            MailBody += "   Status: <strong>" + Status + "</strong>";
            MailBody += "</br>";
            MailBody += "   Amount Due: <strong>" + AmountDue + "</strong>";
            MailBody += "</br>";
            MailBody += "   Remarks: <strong>" + Remarks + "</strong>";

            txtBody.Content = MailBody;
        }
    }
    protected void lnkBtnAddParameter_Click(object sender, EventArgs e)
    {
        try
        {
            string MailBody;
            MailBody = "</br>";
            foreach (GridDataItem row in RadGrid_TicketInvoice.Items)
            {
                Label lblParmID = (Label)row.FindControl("lblId");
                TableCell cell = row["ClientSelectColumn"];
                CheckBox checkBox = (CheckBox)cell.Controls[0];
                if (checkBox.Checked)
                {
                    HyperLink lblParameterName = (HyperLink)row.FindControl("lnkName");
                    MailBody += (lblParameterName.Text).Substring(1) + ":<strong>" + lblParameterName.Text + "</strong>";
                }

            }
            txtBody.Content = txtBody.Content + MailBody;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddCusttype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    protected void lnkSaveTicketInvoice_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtBody.Content != string.Empty)
            {
                objProp_User.ConnConfig = Session["config"].ToString();
                objProp_User.Type = ddlEmailSetup.SelectedItem.Text;
                objProp_User.Subject = txtSubject.Text;
                objProp_User.Body = txtBody.Content;
                objProp_User.BitMap = true;
                objProp_User.BodyMulitple = txtBody.Content;
                objProp_User.Fields = txtBody.Content;
                if (ViewState["TicktInvoice"].ToString() == "0")
                {
                    objBL_User.AddTicketInvoiceEmail(objProp_User);
                    ViewState["TicktInvoice"] = "1";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccessMCPS", "noty({text: 'Email Added Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
                else if (ViewState["TicktInvoice"].ToString() == "1")
                {
                    objProp_User.EmailID = Convert.ToInt32(ViewState["EmailID"]);
                    objBL_User.UpdateTicketInvoiceEmail(objProp_User);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccessMCPS", "noty({text: 'Email Updated Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "Materialize.updateTextFields();", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddCusttype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    #endregion
    protected void RadGrid_consultant_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem data = (GridDataItem)e.Item;
            Label lbl = (Label)data.FindControl("lblPass");
            string value = lbl.Text;
            lbl.Text = "";
            foreach (char c in value)
                lbl.Text += "*";
        }
        //To change the password in * format in edit mode
        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        {
            GridEditableItem edit = (GridEditableItem)e.Item;
            RadTextBox txt = (RadTextBox)edit.FindControl("lblPass");
            string value = txt.Text;
            txt.Text = "";
            foreach (char c in value)
                txt.Text += "*";
        }
        //if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
        //{
        //    GridEditFormItem edititem = (GridEditFormItem)e.Item;
        //    TextBox txtpwd = (TextBox)edititem["lblPassword"].Controls[0];
        //    txtpwd.TextMode = TextBoxMode.Password;            
        //}
    }
    protected void RadGrid_consultant_PreRender(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        ds = objBL_User.getControl(objProp_User);
        if (ds.Tables[0].Rows[0]["consultAPI"].ToString() != "")
        {
            api = Convert.ToBoolean(ds.Tables[0].Rows[0]["consultAPI"]);
        }
        if (!api)
        {
            usernameDiv.Visible = false;
            passwordDiv.Visible = false;
            ipaddressDiv.Visible = false;
            usernameDivCopy.Visible = false;
            passwordDivCopy.Visible = false;
            ipaddressDivCopy.Visible = false;
            RadGrid_consultant.MasterTableView.GetColumn("lblUsername").Display = false;
            RadGrid_consultant.MasterTableView.GetColumn("lblPassword").Display = false;
            RadGrid_consultant.MasterTableView.GetColumn("lblIP").Display = false;
            RadGrid_consultant.MasterTableView.GetColumn("lblPassword1").Display = false;
        }
        RadGrid_consultant.MasterTableView.GetColumn("lblCity").Display = false;
        RadGrid_consultant.MasterTableView.GetColumn("lblAddress").Display = false;
        RadGrid_consultant.MasterTableView.GetColumn("lblState").Display = false;
        RadGrid_consultant.MasterTableView.GetColumn("lblZip").Display = false;
        RadGrid_consultant.MasterTableView.GetColumn("lblcountry").Display = false;
        RadGrid_consultant.MasterTableView.GetColumn("lblContact").Display = false;
        RadGrid_consultant.MasterTableView.GetColumn("lblPhone").Display = false;
        RadGrid_consultant.MasterTableView.GetColumn("lblCellular").Display = false;
        RadGrid_consultant.MasterTableView.GetColumn("lblFax").Display = false;
        RadGrid_consultant.MasterTableView.GetColumn("lblEmail").Display = false;
        RadGrid_consultant.MasterTableView.GetColumn("lblWebsite").Display = false;
        RadGrid_consultant.MasterTableView.GetColumn("lblPassword").Display = false;
    }

    #region Test Custom

    #region "Load data for Test Custom"

    private void FillTestCustomFormat()
    {
        try
        {
            dtTestCstFormat = new DataTable();
            dtTestCstFormat.Columns.Add("Value", typeof(string));
            dtTestCstFormat.Columns.Add("Format", typeof(string));

            DataRow drCustom = dtTestCstFormat.NewRow();
            //drCustom["Value"] = 0;
            //drCustom["Format"] = "";
            //dtTestCstFormat.Rows.Add(drCustom);

            List<string> lstCustom = System.Enum.GetNames(typeof(CommonHelper.CustomField)).ToList();

            int i = 0;
            foreach (var lst in lstCustom)
            {
                i = i + 1;
                drCustom = dtTestCstFormat.NewRow();
                drCustom["Value"] = i;
                drCustom["Format"] = lst;

                dtTestCstFormat.Rows.Add(drCustom);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private DataTable FillMembers(string userName)
    {
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        try
        {
            BusinessEntity.User objProp_User = new BusinessEntity.User();
            ds = objBL_User.getTeamByMonUser(Session["config"].ToString(), userName);
            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return dt;
    }

    //private void CreateTestCustomTable()
    //{

    //    DataSet dst = new DataSet();
    //    BL_SafetyTest _objbltesttypes = new BL_SafetyTest();
    //    dst = _objbltesttypes.GetAllTestCustom(Session["config"].ToString(), Session["dbname"].ToString());

    //    DataTable dt = new DataTable();
    //    dt.Columns.Add("ID", typeof(int));
    //    dt.Columns.Add("Line", typeof(int));
    //    dt.Columns.Add("OrderNo", typeof(int));
    //    dt.Columns.Add("Label", typeof(string));
    //    dt.Columns.Add("IsAlert", typeof(Boolean));
    //    //dt.Columns.Add("IsTask", typeof(Boolean));
    //    dt.Columns.Add("TeamMember", typeof(string));
    //    dt.Columns.Add("Format", typeof(int));
    //    dt.Columns.Add("TeamMemberDisplay", typeof(string));
    //    dt.Columns.Add("UserRoles", typeof(string));
    //    dt.Columns.Add("UserRolesDisplay", typeof(string));
    //    dt.Columns.Add("UseFormula", typeof(Boolean));
    //    dt.Columns.Add("Formula", typeof(string));
    //    dt.Columns.Add("MaxLineNo", typeof(Int16));
    //    DataRow dr = dt.NewRow();

    //    dr["ID"] = 0;
    //    dr["Line"] = dt.Rows.Count + 1;
    //    dr["OrderNo"] = 0;
    //    dr["Label"] = "";
    //    dr["IsAlert"] = 0;
    //    //dr["IsTask"] = 0;
    //    dr["TeamMember"] = "";
    //    dr["Format"] = 0;
    //    dr["TeamMemberDisplay"] = "";
    //    dr["UserRoles"] = "";
    //    dr["UserRolesDisplay"] = "";
    //    dr["UseFormula"] = 0;
    //    dr["Formula"] = "";
    //    dr["MaxLineNo"] = 0;
    //    dt.Rows.Add(dr);
    //    if (dst.Tables[0].Rows.Count == 0)
    //    {
    //        ViewState["TestCustomTable"] = dt;
    //        ViewState["TestCustomDeleteTable"] = dt;
    //    }
    //    else
    //    {
    //        ViewState["TestCustomTable"] = dst.Tables[0];
    //    }

    //    ViewState["TestCustomValues"] = dst.Tables[1];


    //}

    private void GetTestCustomTable()
    {

        DataSet dst = new DataSet();
        BL_SafetyTest _objbltesttypes = new BL_SafetyTest();
        dst = _objbltesttypes.GetAllTestCustom(Session["config"].ToString(), Session["dbname"].ToString());

        if (dst.Tables[0].Rows.Count == 0)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Line", typeof(int));
            dt.Columns.Add("OrderNo", typeof(int));
            dt.Columns.Add("Label", typeof(string));
            dt.Columns.Add("IsAlert", typeof(Boolean));
            //dt.Columns.Add("IsTask", typeof(Boolean));
            dt.Columns.Add("TeamMember", typeof(string));
            dt.Columns.Add("Format", typeof(int));
            dt.Columns.Add("TeamMemberDisplay", typeof(string));
            dt.Columns.Add("UserRoles", typeof(string));
            dt.Columns.Add("UserRolesDisplay", typeof(string));
            dt.Columns.Add("UseFormula", typeof(Boolean));
            dt.Columns.Add("Formula", typeof(string));
            dt.Columns.Add("MaxLineNo", typeof(Int16));
            DataRow dr = dt.NewRow();

            dr["ID"] = 0;
            dr["Line"] = dt.Rows.Count + 1;
            dr["OrderNo"] = 0;
            dr["Label"] = "";
            dr["IsAlert"] = 0;
            //dr["IsTask"] = 0;
            dr["TeamMember"] = "";
            dr["Format"] = 0;
            dr["TeamMemberDisplay"] = "";
            dr["UserRoles"] = "";
            dr["UserRolesDisplay"] = "";
            dr["UseFormula"] = 0;
            dr["Formula"] = "";
            dr["MaxLineNo"] = 0;
            dt.Rows.Add(dr);

            ViewState["TestCustomTable"] = dt;
            ViewState["TestCustomDeleteTable"] = dt;
        }
        else
        {
            ViewState["TestCustomTable"] = dst.Tables[0];
            ViewState["TestCustomValues"] = dst.Tables[1];
        }
    }
    #endregion


    private void BindTestCustomGrid()
    {
        try
        {

            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["TestCustomTable"];

            gvTestCustom.DataSource = dt;
            gvTestCustom.VirtualItemCount = dt.Rows.Count;
            gvTestCustom.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvTestCustom_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            //HiddenField hdSelectTeam = (HiddenField)item.FindControl("hdSelectTeam");
            DropDownList ddlFormat = (DropDownList)item.FindControl("ddlFormat");
            //RadComboBox drpMember = (RadComboBox)item.FindControl("ddlTeamMember");
            //DataTable dt = new DataTable();
            //DataTable dtFull = new DataTable();
            //dtFull = FillMembers("");
            //if (Convert.ToString(DataBinder.Eval(item.DataItem, "TeamMember")).Trim() != "")
            //{
            //    DataSet ds = new DataSet();
            //    BusinessEntity.User objProp_User = new BusinessEntity.User();
            //    ds = objBL_User.getTeamByListID(Session["config"].ToString(), Convert.ToString(DataBinder.Eval(item.DataItem, "TeamMember")));
            //    dt = ds.Tables[0];
            //}
            //else
            //{
            //    dt = dtFull;
            //}
            //dt.Merge(dtFull);
            //dt = RemoveDuplicateRows(dt, "ID");
            //hdSelectTeam.Value = Convert.ToString(DataBinder.Eval(item.DataItem, "TeamMember")).Trim();
            //drpMember.DataSource = dt;
            //drpMember.DataBind();
            //drpMember.Attributes.Add("hdSelectTeam", hdSelectTeam.ClientID);
            //((Literal)drpMember.Footer.FindControl("RadComboItemsCount")).Text = Convert.ToString(drpMember.Items.Count);

            //var lsMember = Convert.ToString(DataBinder.Eval(item.DataItem, "TeamMember")).Split(',').ToList();
            //String msg = "";
            //foreach (RadComboBoxItem x in drpMember.Items)
            //{
            //    if (lsMember.Count(i => i == x.Value && i != "") > 0)
            //    {
            //        msg += x.Text + ",";
            //        x.Checked = true;
            //    }
            //}

            var formatValue = Convert.ToInt32(DataBinder.Eval(item.DataItem, "Format"));
            ddlFormat.SelectedValue = Convert.ToString(formatValue);
            Panel pnlCustomValue = (Panel)item.FindControl("pnlTestCustomValue");

            if (ddlFormat.SelectedItem != null && ddlFormat.SelectedItem.Text == "Dropdown")
                pnlCustomValue.Visible = true;
            else
                pnlCustomValue.Visible = false;

            DropDownList ddlCustomValue = (DropDownList)item.FindControl("ddlTestCustomValue");
            Label lblID = (Label)item.FindControl("lblID");
            Label lblLine = (Label)item.FindControl("lblLine");

            if (ViewState["TestCustomValues"] != null)
            {
                DataTable dtCustomval = (DataTable)ViewState["TestCustomValues"];
                DataTable dataTemp = dtCustomval.Clone();
                DataRow[] result = dtCustomval.Select("Line = " + Convert.ToInt32(lblLine.Text) + "");
                foreach (DataRow row in result)
                {
                    dataTemp.ImportRow(row);
                }

                if (dataTemp.Rows.Count > 0)
                {
                    dataTemp.DefaultView.Sort = "Value  ASC";
                    dataTemp = dataTemp.DefaultView.ToTable();
                }

                ddlCustomValue.DataSource = dataTemp;
                ddlCustomValue.DataTextField = "Value";
                ddlCustomValue.DataValueField = "Value";
                ddlCustomValue.DataBind();
                ddlCustomValue.Items.Insert(0, (new ListItem("--Add New--", "")));
            }

        }

    }

    private void GetDataOnTestCustomGrid()
    {
        try
        {
            DataTable dt = (DataTable)ViewState["TestCustomTable"];
            DataTable dtDetails = dt.Clone();
            DataTable dtCustomValues = new DataTable();
            dtCustomValues.Columns.Add("ID", typeof(int));
            dtCustomValues.Columns.Add("tblTestCustomFieldsID", typeof(int));
            dtCustomValues.Columns.Add("Line", typeof(int));
            dtCustomValues.Columns.Add("Value", typeof(string));
            //dtCustomValues.Columns.Add("type", typeof(string));
            int line = 1;
            var tempLine = 0;

            foreach (GridDataItem gr in gvTestCustom.Items)
            {
                Label lblID = (Label)gr.FindControl("lblID");
                Label lblIndex = (Label)gr.FindControl("lblIndex");
                Label lblLine = (Label)gr.FindControl("lblLine");
                TextBox txtLabel = (TextBox)gr.FindControl("txtLabel");
                CheckBox chkAlert = (CheckBox)gr.FindControl("chkSelectAlert");
                HiddenField OrderNo = (HiddenField)gr.FindControl("txtOrderNo");
                RadComboBox drpMember = (RadComboBox)gr.FindControl("ddlTeamMember");
                DropDownList drpFormat = (DropDownList)gr.FindControl("ddlFormat");
                DropDownList ddlCustomValue = (DropDownList)gr.FindControl("ddlTestCustomValue");

                HiddenField hdSelectTeam = (HiddenField)gr.FindControl("hdnMembers");
                TextBox txtMembers = (TextBox)gr.FindControl("txtMembers");

                HiddenField hdSelectRoles = (HiddenField)gr.FindControl("hdnRoles");
                TextBox txtRoles = (TextBox)gr.FindControl("txtRoles");
                TextBox txtFormula = (TextBox)gr.FindControl("txtFormula");
                CheckBox chkFormual = (CheckBox)gr.FindControl("chkFormual");

                HiddenField hdnMaxLineNo = (HiddenField)gr.FindControl("hdnMaxLineNo");

                if (lblLine != null && lblLine.Text != "0")
                {
                    tempLine = Convert.ToInt16(lblLine.Text);
                }
                else
                {

                    if (Convert.ToInt16(hdnMaxLineNo.Value) < line)
                    {
                        line++;
                    }
                    else
                    {
                        line = Convert.ToInt16(hdnMaxLineNo.Value) + 1;
                    }
                    tempLine = line;
                }

                foreach (ListItem li in ddlCustomValue.Items)
                {
                    if (li.Value != string.Empty)
                    {

                        DataRow drCustomVal = dtCustomValues.NewRow();
                        drCustomVal["ID"] = 0;
                        drCustomVal["tblTestCustomFieldsID"] = Convert.ToInt32(lblID.Text);
                        //drCustomVal["Line"] = lblLine.Text;
                        drCustomVal["Line"] = tempLine;
                        drCustomVal["Value"] = li.Value;
                        dtCustomValues.Rows.Add(drCustomVal);
                    }
                }

                DataRow dr = dtDetails.NewRow();
                dr["ID"] = Convert.ToInt32(lblID.Text);
                //dr["Line"] = lblLine.Text;
                dr["Line"] = tempLine;
                dr["OrderNo"] = Convert.ToInt32(OrderNo.Value);
                dr["Label"] = txtLabel.Text.Trim();
                dr["IsAlert"] = Convert.ToBoolean(chkAlert.Checked);
                //dr["TeamMember"] = hdSelectTeam.Value.Replace("0_", "").Replace("1_", "").Replace("3_", "").Replace("4_", "");
                dr["TeamMember"] = hdSelectTeam.Value;
                dr["Format"] = Convert.ToInt32(drpFormat.SelectedValue);
                dr["TeamMemberDisplay"] = txtMembers.Text;
                dr["UseFormula"] = Convert.ToBoolean(chkFormual.Checked);
                dr["Formula"] = txtFormula.Text;
                dr["MaxLineNo"] = hdnMaxLineNo.Value;
                //dr["UserRoles"] = hdSelectRoles.Value;
                //dr["UserRolesDisplay"] = txtRoles.Text;
                dtDetails.Rows.Add(dr);
                //line++;
            }
            ViewState["TestCustomTable"] = dtDetails;
            ViewState["TestCustomValues"] = dtCustomValues;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #region "Add and Delete Row"

    protected void lnkAddnewRowTestCustom_Click(object sender, EventArgs e)
    {
        GetDataOnTestCustomGrid();
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["TestCustomTable"];
        var currMaxLineNo = 0;
        if(dt.Rows.Count > 0)
        {
            currMaxLineNo = Convert.ToInt16(dt.Rows[0]["MaxLineNo"]);
        }

        DataRow dr = dt.NewRow();

        dr["ID"] = 0;
        dr["Line"] = 0;
        dr["OrderNo"] = dt.Rows.Count + 1;
        dr["Label"] = "";
        dr["IsAlert"] = 0;
        dr["TeamMember"] = "";
        dr["Format"] = 0;
        dr["TeamMemberDisplay"] = "";
        dr["UserRoles"] = "";
        dr["UserRolesDisplay"] = "";
        dr["UseFormula"] = 0;
        dr["Formula"] = "";
        dr["MaxLineNo"] = currMaxLineNo;
        dt.Rows.Add(dr);

        ViewState["TestCustomTable"] = dt;

        FillTestCustomFormat();
        BindTestCustomGrid();
        ReorderTestCustomGridRow();
    }
    protected void ibtnDeleteTestCustomItem_Click(object sender, EventArgs e)
    {
        DeleteTestCustomItem();
        ReorderTestCustomGridRow();
    }
    private void DeleteTestCustomItem()
    {
        try
        {
            GetDataOnTestCustomGrid();

            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["TestCustomTable"];
            DataTable dtdeleted = dt.Clone();
            int count = 0;
            foreach (GridDataItem gr in gvTestCustom.Items)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                Label lblIndex = (Label)gr.FindControl("lblIndex");
                int index = Convert.ToInt32(lblIndex.Text) - 1;
                if (chkSelect.Checked == true)
                {
                    if (dt.Rows.Count > 0)
                    {
                        dtdeleted.ImportRow(dt.Rows[index - count]);
                        dt.Rows.RemoveAt(index - count);
                    }
                    count++;
                }
            }

            ViewState["TestCustomDeletedRows"] = dtdeleted;

            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();

                dr["ID"] = 0;
                dr["Line"] = 0;
                dr["OrderNo"] = dt.Rows.Count + 1;
                dr["Label"] = "";
                dr["IsAlert"] = 0;
                dr["TeamMember"] = "";
                dr["Format"] = 0;
                dr["TeamMemberDisplay"] = "";
                dr["UserRoles"] = "";
                dr["UserRolesDisplay"] = "";
                dr["UseFormula"] = 0;
                dr["Formula"] = "";
                dr["MaxLineNo"] = 0;
                dt.Rows.Add(dr);
            }

            ViewState["TestCustomTable"] = dt;

            FillTestCustomFormat();
            FillMembers("");
            BindTestCustomGrid();
            BindTestCustomDropDown();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void BindTestCustomDropDown()
    {
        int rowIndex = 1;
        try
        {
            foreach (GridDataItem gr in gvTestCustom.Items)
            {
                DropDownList ddlFormat = (DropDownList)gr.FindControl("ddlFormat");
                Label lblLine = (Label)gr.FindControl("lblLine");
                Panel pnlCustomValue = (Panel)gr.FindControl("pnlTestCustomValue");
                if (ddlFormat.SelectedItem.Text == "Dropdown")
                {
                    pnlCustomValue.Visible = true;
                }
                else
                    pnlCustomValue.Visible = false;

                DropDownList ddlCustomValue = (DropDownList)gr.FindControl("ddlTestCustomValue");
                Label lblID = (Label)gr.FindControl("lblID");

                if (ViewState["TestCustomValues"] != null)
                {
                    DataTable dtCustomval = (DataTable)ViewState["TestCustomValues"];
                    DataTable dt = dtCustomval.Clone();
                    // TODO: Thomas --> need to check
                    DataRow[] result = dtCustomval.Select("Line = " + Convert.ToInt32(lblLine.Text) + "");
                    foreach (DataRow row in result)
                    {
                        dt.ImportRow(row);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        dt.DefaultView.Sort = "Value  ASC";
                        dt = dt.DefaultView.ToTable();
                    }

                    ddlCustomValue.DataSource = dt;
                    ddlCustomValue.DataTextField = "Value";
                    ddlCustomValue.DataValueField = "Value";
                    ddlCustomValue.DataBind();
                    ddlCustomValue.Items.Insert(0, (new ListItem("--Add New--", "")));
                    if (ddlFormat.SelectedItem.Text == "Dropdown")
                    {
                        rowIndex++;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ReorderTestCustomGridRow()
    {
        int count = 0;
        foreach (GridDataItem gr in gvTestCustom.Items)
        {
            HiddenField OrderNo = (HiddenField)gr.FindControl("txtOrderNo");
            OrderNo.Value = (count = count + 1).ToString();
        }
    }
    #endregion

    #region "Test Custom Format"    
    protected void ddlTestCustomFormat_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddl = (DropDownList)sender;
            GridDataItem row = (GridDataItem)ddl.NamingContainer;
            Panel pnlCustomValue = (Panel)row.FindControl("pnlTestCustomValue");
            if (row != null)
            {
                if (ddl.SelectedItem.Text == "Dropdown")
                    pnlCustomValue.Visible = true;
                else
                    pnlCustomValue.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlTestCustomValue_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddl = (DropDownList)sender;
            GridDataItem row = (GridDataItem)ddl.NamingContainer;
            LinkButton lnkAddCustomValue = (LinkButton)row.FindControl("lnkAddTestCustomValue");
            LinkButton lnkUpdateCustomValue = (LinkButton)row.FindControl("lnkUpdateTestCustomValue");
            LinkButton lnkDelCustomValue = (LinkButton)row.FindControl("lnkDelTestCustomValue");
            TextBox txtCustomValue = (TextBox)row.FindControl("txtTestCustomValue");
            if (ddl.SelectedIndex == 0)
            {
                lnkAddCustomValue.Visible = true;
                lnkUpdateCustomValue.Visible = false;
                lnkDelCustomValue.Visible = false;
                txtCustomValue.Text = string.Empty;
            }
            else
            {
                lnkAddCustomValue.Visible = false;
                lnkUpdateCustomValue.Visible = true;
                lnkDelCustomValue.Visible = true;
                txtCustomValue.Text = ddl.SelectedValue;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion
    protected void RadComboBox1_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        e.Item.Text = ((DataRowView)e.Item.DataItem)["MomUserID"].ToString();
        e.Item.Value = ((DataRowView)e.Item.DataItem)["ID"].ToString();
    }

    protected void RadComboBox1_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {

        RadComboBox obj = sender as RadComboBox;
        obj.Items.Clear();

        obj.ClearCheckedItems();
        obj.ClearSelection();

        obj.DataSource = FillMembers(e.Text);
        obj.DataBind();
    }

    protected void LnkSaveTestCustom_Click(object sender, EventArgs e)
    {
        try
        {
            GetDataOnTestCustomGrid();
            DataTable dtTestCustom = (DataTable)ViewState["TestCustomTable"];
            DataTable dtCustomVal = (DataTable)ViewState["TestCustomValues"];
            DataTable dtDeleted = dtTestCustom.Clone();
            if (ViewState["TestCustomDeletedRows"] != null)
                dtDeleted = (DataTable)ViewState["TestCustomDeletedRows"];

            DataColumnCollection TestCustomCols = dtTestCustom.Columns;
            if (TestCustomCols.Contains("MaxLineNo"))
            {
                dtTestCustom.Columns.Remove("MaxLineNo");
                dtTestCustom.AcceptChanges();
            }

            DataColumnCollection dtDeletedCol = dtDeleted.Columns;
            if (dtDeletedCol.Contains("MaxLineNo"))
            {
                dtDeleted.Columns.Remove("MaxLineNo");
                dtDeleted.AcceptChanges();
            }

            //DataColumnCollection dtDeletedCol = dtDeleted.Columns;
            //if (dtDeletedCol.Contains("TeamMemberDisplay"))
            //{
            //    dtDeleted.Columns.Remove("TeamMemberDisplay");
            //}

            //DataColumnCollection TestCustomCols = dtTestCustom.Columns;
            //if (TestCustomCols.Contains("TeamMemberDisplay"))
            //{
            //    dtTestCustom.Columns.Remove("TeamMemberDisplay");
            //}


            TestCustom obj = new TestCustom();
            obj.ConnConfig = Session["config"].ToString();
            obj.TestCustomItem = dtTestCustom;
            obj.TestCustomItemDelete = dtDeleted;
            obj.TestCustomValue = dtCustomVal;
            BL_SafetyTest _bltest = new BL_SafetyTest();
            _bltest.CreateAndUpdateTestCustom(obj);

            GetTestCustomTable();
            FillTestCustomFormat();
            FillMembers("");
            BindTestCustomGrid();
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccessMCPS", "noty({text: 'Test custom updated successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddWarehousetype1", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    protected void gvCustom_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            GridDataItem item = (GridDataItem)e.Item;
            LinkButton lnkAddCustomValue = (LinkButton)item.FindControl("lnkAddTestCustomValue");
            LinkButton lnkDelCustomValue = (LinkButton)item.FindControl("lnkDelTestCustomValue");
            LinkButton lnkUpdateCustomValue = (LinkButton)item.FindControl("lnkUpdateTestCustomValue");


            if (e.CommandName.Equals("AddTestCustomValue"))
            {
                TextBox txtCustomValue = (TextBox)item.FindControl("txtTestCustomValue");
                DropDownList ddlCustomValue = (DropDownList)item.FindControl("ddlTestCustomValue");

                Boolean isExistItem = false;
                foreach (ListItem x in ddlCustomValue.Items)
                {
                    if (x.Text == txtCustomValue.Text.Trim())
                    {
                        isExistItem = true;
                        break;
                    }
                }

                if (txtCustomValue.Text.Trim() != string.Empty && !isExistItem)
                {
                    ddlCustomValue.Items.Add(new ListItem(txtCustomValue.Text.Trim(), txtCustomValue.Text.Trim()));
                    txtCustomValue.Text = string.Empty;
                    ddlCustomValue.SelectedValue = txtCustomValue.Text.Trim();
                }
            }
            else if (e.CommandName.Equals("UpdateTestCustomValue"))
            {

                TextBox txtCustomValue = (TextBox)item.FindControl("txtTestCustomValue");
                DropDownList ddlCustomValue = (DropDownList)item.FindControl("ddlTestCustomValue");
                if (txtCustomValue.Text.Trim() != string.Empty)
                {
                    ddlCustomValue.Items.Remove(new ListItem(ddlCustomValue.SelectedValue, ddlCustomValue.SelectedValue));
                    ddlCustomValue.Items.Add(new ListItem(txtCustomValue.Text.Trim(), txtCustomValue.Text.Trim()));
                    ddlCustomValue.SelectedValue = txtCustomValue.Text.Trim();

                }
                lnkAddCustomValue.Visible = true;
                lnkUpdateCustomValue.Visible = false;
                lnkDelCustomValue.Visible = false;
                txtCustomValue.Text = string.Empty;
            }
            else if (e.CommandName.Equals("DeleteTestCustomValue"))
            {

                TextBox txtCustomValue = (TextBox)item.FindControl("txtTestCustomValue");
                DropDownList ddlCustomValue = (DropDownList)item.FindControl("ddlTestCustomValue");

                ddlCustomValue.Items.Remove(new ListItem(ddlCustomValue.SelectedValue, ddlCustomValue.SelectedValue));
                ddlCustomValue.SelectedIndex = 0;
                lnkAddCustomValue.Visible = true;
                lnkUpdateCustomValue.Visible = false;
                lnkDelCustomValue.Visible = false;
                txtCustomValue.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable RemoveDuplicateRows(DataTable table, string DistinctColumn)
    {
        try
        {
            ArrayList UniqueRecords = new ArrayList();
            ArrayList DuplicateRecords = new ArrayList();
            foreach (DataRow dRow in table.Rows)
            {
                if (UniqueRecords.Contains(dRow[DistinctColumn]))
                    DuplicateRecords.Add(dRow);
                else
                    UniqueRecords.Add(dRow[DistinctColumn]);
            }

            foreach (DataRow dRow in DuplicateRecords)
            {
                table.Rows.Remove(dRow);
            }

            return table;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    protected void RadComboBox1_DataBound(object sender, EventArgs e)
    {
        RadComboBox obj = sender as RadComboBox;
        //set the initial footer label
        ((Literal)obj.Footer.FindControl("RadComboItemsCount")).Text = Convert.ToString(obj.Items.Count);
    }
    #endregion

    #region EquipTestPricing
    private void FillEquipTestTypePricing()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.GetAllEquipmentTestPricing(Session["config"].ToString());

        RadGrid_EquipTestPricing.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_EquipTestPricing.DataSource = ds.Tables[0];
    }
    protected void RadGrid_EquipTestPricing_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        FillEquipTestTypePricing();
    }

    protected void lnkSaveEquipTestPricing_Click(object sender, EventArgs e)
    {
        IsAddEdit = false;
        try
        {
            EquipTestPrice obj = new EquipTestPrice();
            obj.Id = hdnEquipTestPricingId.Value == "" ? 0 : Convert.ToInt32(hdnEquipTestPricingId.Value);
            obj.Classification = auto_hdnEquipClassification.Value;
            obj.TestTypeId = auto_hdnTestTypeId.Value == "" ? 0 : Convert.ToInt32(auto_hdnTestTypeId.Value);
            obj.Amount = txtEquipTestPrice.Text == "" ? 0 : double.Parse(txtEquipTestPrice.Text.Trim(), NumberStyles.Currency);
            obj.OverrideAmount = txtOverride.Text == "" ? 0 : double.Parse(txtOverride.Text.Trim(), NumberStyles.Currency);
            obj.Remarks = txtPriceRemarks.Text;
            obj.DefaultHour = txtDefaultHour.Text == "" ? 0 : double.Parse(txtDefaultHour.Text.Trim());
            obj.PriceYear = Convert.ToInt32(txtEquipTestPriceYear.Text);
            obj.IsThirdPartyRequired = chkPriceThirdParty.Checked;

            //obj.UpdateType = Convert.ToInt32(drpdwnTypeUpdate.SelectedValue);
            obj.UpdateType = Convert.ToInt32(chkUpdateAllTest.Checked);
            string msg = string.Empty;
            if (obj.Id == 0)
            {
                if (!DuplicateEquipTestPrice(obj))
                {
                    obj.CreatedBy = Session["username"].ToString();
                    objBL_User.AddEquipmentTestPricing(Session["config"].ToString(), obj);
                    msg = "Added";

                    chkUpdateAllTest.Checked = false;
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseEquipTestPricingWindow();", true);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddLoctype", "noty({text: 'Pricing " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    FillEquipTestTypePricing();
                    RadGrid_EquipTestPricing.Rebind();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "keyErrDuplicateEquipTest", "noty({text: 'This pricing already exists!',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                }
            }
            else
            {
                foreach (GridDataItem di in RadGrid_EquipTestPricing.SelectedItems)
                {
                    IsAddEdit = true;
                    TableCell cell = di["chkSelect"];
                    CheckBox chkSelect = (CheckBox)cell.Controls[0];
                    if (chkSelect.Checked == true)
                    {
                        obj.UpdatedBy = Session["username"].ToString();
                        objBL_User.UpdateEquipmentTestPricing(Session["config"].ToString(), obj);
                        msg = "Updated";
                    }
                }
                chkUpdateAllTest.Checked = false;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseEquipTestPricingWindow();", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddLoctype", "noty({text: 'Pricing " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                FillEquipTestTypePricing();
                RadGrid_EquipTestPricing.Rebind();
            }




            //if (!DuplicateEquipTestPrice(obj))
            //{
            //    foreach (GridDataItem di in RadGrid_EquipTestPricing.SelectedItems)
            //    {
            //        IsAddEdit = true;
            //        TableCell cell = di["chkSelect"];
            //        CheckBox chkSelect = (CheckBox)cell.Controls[0];
            //        if (chkSelect.Checked == true)
            //        {
            //            obj.UpdatedBy = Session["username"].ToString();
            //            objBL_User.UpdateEquipmentTestPricing(Session["config"].ToString(), obj);
            //            msg = "Updated";
            //        }
            //    }
            //    if (!IsAddEdit)
            //    {
            //        obj.CreatedBy = Session["username"].ToString();
            //        objBL_User.AddEquipmentTestPricing(Session["config"].ToString(), obj);
            //        msg = "Added";
            //    }
            //    chkUpdateAllTest.Checked = false;
            //    ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseEquipTestPricingWindow();", true);
            //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddLoctype", "noty({text: 'Pricing " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            //    FillEquipTestTypePricing();
            //    RadGrid_EquipTestPricing.Rebind();
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(Page, typeof(Page), "keyErrDuplicateEquipTest", "noty({text: 'This pricing already exists!',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            //}

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddPricing", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

        }
    }
    protected void lnkDeleteEquipTestPricing_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            objProp_User = new User();
            objProp_User.ConnConfig = Session["config"].ToString();
            EquipTestPrice obj = new EquipTestPrice();

            foreach (GridDataItem di in RadGrid_EquipTestPricing.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                HiddenField hdnid = (HiddenField)di.FindControl("hdnEquipTestPricingID");

                if (chkSelect.Checked == true)
                {
                    obj.Id = Convert.ToInt32(hdnid.Value);
                    objBL_User.DeleteEquipmentTestPricingById(Session["config"].ToString(), obj.Id);

                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: 'Test pricing successfully deleted', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningDelMCPS", "noty({text: 'Please select item to delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
            else
            {
                FillEquipTestTypePricing();
                RadGrid_EquipTestPricing.Rebind();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddWarehousetype1", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    #endregion


    private bool DuplicateEquipTestPrice(EquipTestPrice obj)
    {
        bool testexists = false;
        DataSet ds = objBL_User.DuplicateEquipTestPrice(Session["config"].ToString(), obj.Classification, obj.TestTypeId, obj.PriceYear);

        if (obj.Id == 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                testexists = true;
            }
        }
        else
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (obj.Id != Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]))
                {
                    testexists = true;
                }
            }
        }
        return testexists;
    }


    #region Equipment Classification
    private void FillEquipClassification()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getEquipClassification(objProp_User);
        RadGrid_EquipClassification.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_EquipClassification.DataSource = ds.Tables[0];
        lblEquipClassificationHeader.InnerText = "Classification";
        lblEditClassificationHeader.InnerText = "Classification";
        lnkEditClassification.Text = "Classification";
        if (ds.Tables[0].Rows.Count > 0)
        {
            RadGrid_EquipClassification.Columns[1].HeaderText = "Classification";
        }
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            lblEquipClassificationHeader.InnerText = ds.Tables[0].Rows[0]["Label"].ToString();
            lblEditClassificationHeader.InnerText = ds.Tables[0].Rows[0]["Label"].ToString();
            lnkEditClassification.Text = ds.Tables[0].Rows[0]["Label"].ToString();
            RadGrid_EquipClassification.Columns[1].HeaderText = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }
    bool isGroupingEquipClassification = false;
    public bool ShouldApplySortEquipClassification()
    {
        return RadGrid_EquipClassification.MasterTableView.FilterExpression != "" ||
            (RadGrid_EquipClassification.MasterTableView.GroupByExpressions.Count > 0 || isGroupingEquipClassification) ||
            RadGrid_EquipClassification.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_EquipClassification_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_EquipClassification.AllowCustomPaging = !ShouldApplySortEquipClassification();
        FillEquipClassification();
    }
    protected void lnkEquipClassificationSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnEquipClassification.Value == "")
            {
                objProp_User.ConnConfig = Session["config"].ToString();
                objProp_User.Classification = txtEquipClassification.Text;
                objProp_User.Status = 0;
                if (chkClassificationStatus.Checked)
                {
                    objProp_User.Status = 1;
                }

                objBL_User.AddEquipClassification(objProp_User);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseEquipClassificationWindow('Add');", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccEqupCate", "noty({text: 'Classification Added Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                FillEquipClassification();
                RadGrid_EquipClassification.Rebind();
                txtEquipClassification.Text = string.Empty;
            }
            else
            {
                objProp_User.ConnConfig = Session["config"].ToString();
                objProp_User.oldClassification = hdnEquipClassification.Value;
                objProp_User.Classification = txtEquipClassification.Text;
                objProp_User.Status = 0;
                if (chkClassificationStatus.Checked)
                {
                    objProp_User.Status = 1;
                }
                objBL_User.EditEquipClassification(objProp_User);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseEquipClassificationWindow('Add');", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccEqupCate", "noty({text: 'Classification Added Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                FillEquipClassification();
                RadGrid_EquipClassification.Rebind();
                txtEquipClassification.Text = string.Empty;
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string type = "error";
            if (str.Contains("Equipment Classification already exists, please use different equipment")
                || str.Contains("Equipment Classification is used in Elev, it cannot be updated!"))
            {
                type = "warning";
            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddEqupClassification", "noty({text: '" + str + "', dismissQueue: true,  type : '" + type + "', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    protected void lnkUpdateClassificationHeader_Click(object sender, EventArgs e)
    {
        if (txtEditClassificationHeader.Text != "")
        {
            objCustomer.ConnConfig = Session["config"].ToString();
            objCustomer.HeaderServices = txtEditClassificationHeader.Text;
            objBL_Customer.UpdateEquipmentClassificationHeader(objCustomer);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseEquipClassificationWindow('Header');", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccEquipClassification", "noty({text: 'Header Updated Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            FillEquipClassification();
            RadGrid_EquipClassification.Rebind();
            lblEquipClassificationHeader.InnerText = txtEditClassificationHeader.Text;
            lnkEditClassification.Text = txtEditClassificationHeader.Text;

        }
    }
    protected void lnkDeleteEquipClassification_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_EquipClassification.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                Label lblId = (Label)di.FindControl("lblId");
                if (chkSelect.Checked == true)
                {
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.Classification = lblId.Text;
                    objBL_User.DeleteEquipClassification(objProp_User);
                    FillEquipClassification();
                    RadGrid_EquipClassification.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccEquipClassification", "noty({text: 'Equipment " + lblId.Text + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningDelEquipType", "noty({text: 'Please select Equipment to delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelEqtyp", "noty({text: '" + str + "',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #endregion

    #region "Test Setup Form"
    private void FillTestSetupForms()
    {

        DataSet ds = new DataSet();
        BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
        ds = bl_SafetyTest.GetAllTestSetupForms(Session["config"].ToString());

        gvDocuments.VirtualItemCount = ds.Tables[0].Rows.Count;
        gvDocuments.DataSource = ds.Tables[0];
        gvDocuments.DataBind();

    }
    protected void gvDocuments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();
        BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
        ds = bl_SafetyTest.GetAllTestSetupForms(Session["config"].ToString());

        gvDocuments.VirtualItemCount = ds.Tables[0].Rows.Count;
        gvDocuments.DataSource = ds.Tables[0];
    }

    private TestSetupForm getTestSetupForm(int id)
    {
        TestSetupForm obj = new TestSetupForm();
        BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
        DataSet ds = bl_SafetyTest.GetTestSetupFormsById(Session["config"].ToString(), id);
        if (ds.Tables.Count > 0)
        {
            PopulateFields(obj, ds.Tables[0].Rows[0]);
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


    protected void lnkAddForm_Click(object sender, EventArgs e)
    {
        txtEstimateName.Text = "";
        hdnEstimateFormId.Value = "0";
        chkIsActive.Checked = false;
        string script = "function f(){$find(\"" + RadWindowForms.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

        ViewState["notesmode"] = 0;
    }

    protected void lnkEditForm_Click(object sender, EventArgs e)
    {
        Boolean flag = false;
        foreach (GridViewRow di in gvDocuments.Rows)
        {
            CheckBox chkSelect = (CheckBox)di.FindControl("chkSelect");
            Label lblID = (Label)di.FindControl("lblID");
            Label lblName = (Label)di.FindControl("lblName");
            CheckBox gchkActive = (CheckBox)di.FindControl("chkActive");


            if (chkSelect.Checked == true)
            {
                flag = true;
                hdnEstimateFormId.Value = lblID.Text;
                txtEstimateName.Text = lblName.Text;
                chkIsActive.Checked = gchkActive.Checked;
                break;
            }
        }
        if (flag)
        {
            string script = "function f(){$find(\"" + RadWindowForms.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
        }
        else
        {
            string str = "Please select item to edit";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddLoctype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

        }

    }


    protected void lnkUploadDoc_Click(object sender, EventArgs e)
    {

        try
        {
            string fullpath = string.Empty;
            string MIME = string.Empty;
            TestSetupForm obj = new TestSetupForm();
            obj.Id = hdnEstimateFormId.Value == "" ? 0 : Convert.ToInt32(hdnEstimateFormId.Value);
            obj.ConnConfig = Session["config"].ToString();
            if (obj.Id > 0)
            {
                BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
                DataSet ds = bl_SafetyTest.GetTestSetupFormsById(obj.ConnConfig, obj.Id);
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        obj.FileName = dt.Rows[0]["FileName"].ToString();
                        obj.FilePath = dt.Rows[0]["FilePath"].ToString();
                        obj.MIME = dt.Rows[0]["MIME"].ToString();
                    }
                }
            }
            if (FileUpload2.HasFile)
            {
                string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                string savepath = savepathconfig + @"\" + Session["dbname"] + @"\SetupForms" + @"\";
                obj.FileName = FileUpload2.FileName;
                MIME = System.IO.Path.GetExtension(FileUpload2.PostedFile.FileName).Substring(1);
                if (MIME.ToLower() != "docx")
                {
                    throw new Exception("MS Word 2007 or later .docx format is the only supported file type.");
                }

                if (obj.FilePath != "")
                {
                    if (System.IO.File.Exists(obj.FilePath))
                        System.IO.File.Delete(obj.FilePath);
                }

                fullpath = savepath + Guid.NewGuid() + "." + MIME;
                obj.FilePath = fullpath;

                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }
                obj.FilePath = fullpath;
                obj.MIME = MIME;
                FileUpload2.SaveAs(obj.FilePath);
            }
            obj.Name = txtEstimateName.Text;
            obj.Type = 1; //default template
            obj.IsActive = chkIsActive.Checked;
            try
            {
                String msg = "added";

                if (obj.Id > 0)
                {
                    BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
                    obj.UpdatedBy = Session["username"].ToString();
                    bl_SafetyTest.UpdateTestSetupForms(obj);
                    msg = "Updated";
                }
                else
                {
                    BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
                    obj.AddedBy = Session["username"].ToString();
                    bl_SafetyTest.AddTestSetupForms(obj);
                }

                FillTestSetupForms();

                string script = "function f(){$('#dvCustomerSetup').hide(); $('#dvEquipmentsetup').show(); $('#equpmentCategory').css('display','none'); $('#TestForms').css('display','block');$('#liEquipmentsetup').addClass( 'active');$('#accrdcustomersetup').removeClass('active');Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddLoctype", "noty({text: 'Document " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddLoctype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkFileName_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        TestSetupForm obj = getTestSetupForm(Convert.ToInt32(btn.CommandArgument));
        DownloadDocument(obj.FilePath, obj.FileName);
    }

    protected void lnkDeleteDoc_Click(object sender, EventArgs e)
    {
        Boolean flag = false;
        foreach (GridViewRow di in gvDocuments.Rows)
        {
            CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
            Label lblID = (Label)di.FindControl("lblId");

            if (chkSelected.Checked == true)
            {
                flag = true;
                DeleteFileFromFolder(string.Empty, Convert.ToInt32(lblID.Text));
            }
        }
        if (!flag)
        {
            string str = "Please select item to delete";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddLoctype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

        }
        else
        {
            FillTestSetupForms();
        }

    }
    private void DeleteFile(int ID)
    {
        try
        {
            TestSetupForm obj = getTestSetupForm(ID);

            BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
            bl_SafetyTest.DeleteTestSetupForms(Session["config"].ToString(), ID);

            if (obj.FilePath != "")
            {
                if (System.IO.File.Exists(obj.FilePath))
                    System.IO.File.Delete(obj.FilePath);
            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccTestForms", "noty({text: 'File  Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            FillTestSetupForms();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrdelete", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
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

    #endregion

    protected void lnkUpdateDefaultWorkerHeader_Click(object sender, EventArgs e)
    {
        if (txtDefaultWorker.Text != "")
        {
            objCustomer.ConnConfig = Session["config"].ToString();
            objCustomer.HeaderServices = txtDefaultWorker.Text;
            objBL_Customer.UpdateDefaultWorkerHeader(objCustomer);
            lnDefaultWorker.InnerText = txtDefaultWorker.Text;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseEquipClassificationWindow('Header');", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccEquipClassification", "noty({text: 'Header Updated Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "setvalue", "setDefaultWorkerValue('" + txtDefaultWorker.Text + "');", true);

        }
    }

    private void SetDefaultWorker()
    {
        objCustomer.ConnConfig = Session["config"].ToString();
        objCustomer.HeaderServices = txtDefaultWorker.Text;
        string getValue = objBL_Customer.GetDefaultWorkerHeader(objCustomer);
        if (!string.IsNullOrEmpty(getValue))
        {
            lnDefaultWorker.InnerText = getValue;
        }
        else
        {
            lnDefaultWorker.InnerText = "Default Worker";
        }

    }

    protected void lnkProjectDefault_Click(object sender, EventArgs e)
    {
        try
        {
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.codes = Convert.ToInt16(ddlTasks.SelectedValue);
            objProp_User.TargetHPermission = chkTargetHours.Checked == true ? 1 : 0;
            objBL_User.UpdateControlProjectDefaults(objProp_User, Convert.ToInt16(ddlShowContactTypeinProjectScren.SelectedValue));
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddDefault", "noty({text: 'Default updated Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddDefault", "noty({text: '" + str + "',dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void lnkEditCusttemp_Click1(object sender, EventArgs e)
    {

    }

    /* Removed Team Titles feature
    protected void lnkTeamTitleSave_Click(object sender, EventArgs e)
    {
        IsAddEdit = false;
        try
        {
            var objTeamTitle = new TeamMemberTitle();
            //objTeamTitle.IsDefault = chkTeamTitleDefault.Checked;
            objTeamTitle.Remarks = txtTeamTitleRemarks.Text;
            objTeamTitle.Title = txtTeamTitle.Text;
            objTeamTitle.ConnConfig = Session["config"].ToString();
            
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(hdnTeamTitleID.Value) && hdnTeamTitleID.Value != "0")
            {
                foreach (GridDataItem di in RadGrid_TeamTitle.SelectedItems)
                {
                    IsAddEdit = true;
                    TableCell cell = di["chkSelect"];
                    CheckBox chkSelect = (CheckBox)cell.Controls[0];
                    if (chkSelect.Checked == true)
                    {
                        objTeamTitle.Id = Convert.ToInt32(hdnTeamTitleID.Value);
                        objBL_User.UpdateProjectTeamMemberTitle(objTeamTitle);
                        msg = "Updated";
                    }
                }
            }
            if (!IsAddEdit)
            {
                objBL_User.AddProjectTeamMemberTitle(objTeamTitle);
                msg = "Added";
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseTeamTitleWindow();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Team Member " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            RadGrid_TeamTitle.Rebind();
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void RadGrid_TeamTitle_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_TeamTitle.AllowCustomPaging = !ShouldApplySortFilterTeamTitle();
        FillTeamTitles();
    }

    protected void lnkDeleteTeamTitle_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_TeamTitle.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                Label lblId = (Label)di.FindControl("lblId");
                var objTeamTitle = new TeamMemberTitle();
                if (chkSelect.Checked == true)
                {
                    objTeamTitle.ConnConfig = Session["config"].ToString();
                    objTeamTitle.Id = Convert.ToInt32(lblId.Text);

                    objBL_User.DeleteProjectTeamMemberTitleById(objTeamTitle);
                    RadGrid_TeamTitle.Rebind();
                    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Team Member " + lblId.Text + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Team Member Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarning", "noty({text: 'Please select Team Member to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    private void FillTeamTitles()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.GetTeamMemberTitle(objProp_User);
        RadGrid_TeamTitle.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_TeamTitle.DataSource = ds.Tables[0];
    }

    bool isGroupingTeamTitle = false;
    public bool ShouldApplySortFilterTeamTitle()
    {
        return RadGrid_TeamTitle.MasterTableView.FilterExpression != "" ||
            (RadGrid_TeamTitle.MasterTableView.GroupByExpressions.Count > 0 || isGroupingTeamTitle) ||
            RadGrid_TeamTitle.MasterTableView.SortExpressions.Count > 0;
    }
    */

    //protected void rdpTaxType_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //    ddlType.Items.Clear();
    //    ddlType.Items.Insert(0, new ListItem("Simple", "0"));
    //    ddlType.Items.Insert(1, new ListItem("Compound w/GST", "1"));
    //    if (rdpTaxType.SelectedValue == "0" && trpst.Visible == true){ 
    //    ddlType.Items.Insert(2, new ListItem("Harmonized", "2"));
    //    }


    //}

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

    protected void RadGrid_Emails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_Emails.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        if (!IsPostBack)
        {

            if (Session["Emails_FilterExpression"] != null && Convert.ToString(Session["Emails_FilterExpression"]) != "" && Session["Emails_Filters"] != null)
            {
                RadGrid_Emails.MasterTableView.FilterExpression = Convert.ToString(Session["Emails_FilterExpression"]);
                var filtersGet = Session["Emails_Filters"] as List<RetainFilter>;
                if (filtersGet != null)
                {
                    foreach (var _filter in filtersGet)
                    {
                        GridColumn column = RadGrid_Emails.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                        column.CurrentFilterValue = _filter.FilterValue;
                    }
                }
            }

        }

        InitTeamMemberGridView();
        //FillDistributionList("", "");

    }
    private void InitTeamMemberGridView()
    {
        User objPropUser = new User();
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        //objPropUser.Status = 0;
        ds = objBL_User.GetUsersAndRolesForTeamMemberList(objPropUser);
        var teamMembers = ds.Tables[0];

        // Get contacts list from exchange server
        //DataTable contactList = GetContactsForExchServer();
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
                teamMembers.Rows.Add(_dr);
            }
        }

        RadGrid_Emails.DataSource = teamMembers;
        if (teamMembers != null && teamMembers.Rows.Count > 0)
        {
            RadGrid_Emails.VirtualItemCount = teamMembers.Rows.Count;
        }
        else
        {
            RadGrid_Emails.VirtualItemCount = 0;
        }

    }

    //protected void ReloadGrid_Click(object sender, EventArgs e)
    //{
    //    InitTeamMemberGridView();
    //    RadGrid_Emails.Rebind();
    //    //InitUserRoleGridView();
    //    //RadGrid_UserRoles.Rebind();
    //}

    //private void InitUserRoleGridView()
    //{
    //    DataSet ds = new DataSet();
    //    UserRole userRole = new UserRole();
    //    userRole.ConnConfig = Session["config"].ToString();

    //    userRole.SearchBy = "";
    //    userRole.SearchValue = "";


    //    ds = objBL_User.GetRoleSearch(userRole, true);

    //    //DataTable result = ProcessDataFilterRole(ds.Tables[0]);
    //    DataTable result = ds.Tables[0];
    //    RadGrid_UserRoles.VirtualItemCount = result.Rows.Count;
    //    RadGrid_UserRoles.DataSource = result;

    //}

    //protected void RadGrid_UserRoles_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    //{
    //    RadGrid_UserRoles.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
    //    if (!IsPostBack)
    //    {

    //        if (Session["Roles_FilterExpression"] != null && Convert.ToString(Session["Roles_FilterExpression"]) != "" && Session["Roles_Filters"] != null)
    //        {
    //            RadGrid_UserRoles.MasterTableView.FilterExpression = Convert.ToString(Session["Roles_FilterExpression"]);
    //            var filtersGet = Session["Roles_Filters"] as List<RetainFilter>;
    //            if (filtersGet != null)
    //            {
    //                foreach (var _filter in filtersGet)
    //                {
    //                    GridColumn column = RadGrid_UserRoles.MasterTableView.GetColumnSafe(_filter.FilterColumn);
    //                    column.CurrentFilterValue = _filter.FilterValue;
    //                }
    //            }
    //        }

    //    }

    //    InitUserRoleGridView();
    //}

    //protected void RadGrid_UserRoles_PreRender(object sender, EventArgs e)
    //{
    //    String filterExpression = Convert.ToString(RadGrid_UserRoles.MasterTableView.FilterExpression);
    //    if (filterExpression != "")
    //    {
    //        Session["Roles_FilterExpression"] = filterExpression;
    //        List<RetainFilter> filters = new List<RetainFilter>();

    //        foreach (GridColumn column in RadGrid_UserRoles.MasterTableView.OwnerGrid.Columns)
    //        {
    //            String filterValues = column.CurrentFilterValue;
    //            if (filterValues != "")
    //            {
    //                String columnName = column.UniqueName;
    //                RetainFilter filter = new RetainFilter();
    //                filter.FilterColumn = columnName;
    //                filter.FilterValue = filterValues;
    //                filters.Add(filter);
    //            }
    //        }

    //        Session["Roles_Filters"] = filters;
    //    }
    //    else
    //    {
    //        Session["Roles_FilterExpression"] = null;
    //        Session["Roles_Filters"] = null;
    //    }

    //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "bindingClickCheckboxUserRoles", "BindClickEventForGridCheckBoxUserRoles();", true);
    //}

    bool isGroupingTaskCategory = false;
    public bool ShouldApplySortTaskCategory()
    {
        return RadGrid_TaskCategory.MasterTableView.FilterExpression != "" ||
            (RadGrid_TaskCategory.MasterTableView.GroupByExpressions.Count > 0 || isGroupingTaskCategory) ||
            RadGrid_TaskCategory.MasterTableView.SortExpressions.Count > 0;
    }

    private void FillTaskCategory()
    {
        objCustomer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Customer.GetTaskCategories(objCustomer);
        RadGrid_TaskCategory.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_TaskCategory.DataSource = ds.Tables[0];
    }

    protected void RadGrid_TaskCategory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_TaskCategory.AllowCustomPaging = !ShouldApplySortTaskCategory();
        FillTaskCategory();
    }

    protected void lnkDeleteTaskCategory_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_TaskCategory.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                string Id = di["lblTCId"].Text.Trim();
                if (chkSelect.Checked == true)
                {
                    #region Add code to delete BT
                    objCustomer.ConnConfig = Session["config"].ToString();
                    objCustomer.Mode = 2;
                    objCustomer.TaskCategory = new TaskCategory()
                    {
                        ID = Convert.ToInt32(Id),
                        CreatedDate = DateTime.Now
                    };
                    objBL_Customer.CRUDTaskCategory(objCustomer);
                    #endregion
                    FillTaskCategory();
                    RadGrid_TaskCategory.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProduct", "noty({text: 'Task Category " + Id + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningProduct", "noty({text: 'Please select Task Category to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelProduct", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkSaveTaskCategory_Click(object sender, EventArgs e)
    {
        IsAddEdit = hdnEditMode.Value == "1";
        string tcategoryName = "";
        string tcategoryID = "0";
        int count = 0;
        try
        {
            #region code to push data to db
            objCustomer.ConnConfig = Session["config"].ToString();
            string msg = string.Empty;
            if (IsAddEdit)
            {
                foreach (GridDataItem di in RadGrid_TaskCategory.SelectedItems)
                {
                    TableCell cell = di["chkSelect"];
                    CheckBox chkSelect = (CheckBox)cell.Controls[0];
                    tcategoryID = di["lblTCId"].Text.Trim();
                    tcategoryName = di["lblTCName"].Text.Trim();
                    count = Convert.ToInt32(di["lblTCCount"].Text);
                    if (chkSelect.Checked == true)
                    {
                        objCustomer.Mode = 1;

                        msg = "Updated";
                    }
                }
            }
            else
            {
                objCustomer.Mode = 0;

                msg = "Added";
            }
            objCustomer.TaskCategory = new TaskCategory()
            {
                ID = Convert.ToInt32(tcategoryID),
                Name = txtTCName.Text,
                Remarks = txtTCRemarks.Text,
                CreatedBy = Session["Username"].ToString(),
                CreatedDate = DateTime.Now
            };
            objBL_Customer.CRUDTaskCategory(objCustomer);
            #endregion

            FillTaskCategory();
            RadGrid_TaskCategory.Rebind();
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseTaskCategoryWindow('Add');", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddTaskCategory", "noty({text: 'Task Category " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            String type = "error";

            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            if (str.Contains("Already exists"))
            {
                type = "warning";
            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrService", "noty({text: '" + str + "', dismissQueue: true,  type : '" + type + "', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #region "workflow"
    protected void gvWorkflow_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DropDownList ddlWorkflowFormat = (DropDownList)item.FindControl("ddlWorkflowFormat");

            var formatValue = Convert.ToInt32(DataBinder.Eval(item.DataItem, "Format"));
            ddlWorkflowFormat.SelectedValue = Convert.ToString(formatValue);
            Panel pnlWorkflowValue = (Panel)item.FindControl("pnlWorkflowValue");
            if (ddlWorkflowFormat.SelectedItem != null && ddlWorkflowFormat.SelectedItem.Text == "Dropdown")
                pnlWorkflowValue.Visible = true;
            else
                pnlWorkflowValue.Visible = false;

            DropDownList ddlWorkflowValue = (DropDownList)item.FindControl("ddlWorkflowValue");
            Label lblID = (Label)item.FindControl("lblID");
            Label lblLine = (Label)item.FindControl("lblLine");

            if (ViewState["WorkflowValues"] != null)
            {
                DataTable dtWorkflowval = (DataTable)ViewState["WorkflowValues"];
                DataTable dataTemp = dtWorkflowval.Clone();

                DataRow[] result = dtWorkflowval.Select("Line = " + Convert.ToInt32(lblLine.Text) + "");
                foreach (DataRow row in result)
                {
                    dataTemp.ImportRow(row);
                }

                if (dataTemp.Rows.Count > 0)
                {
                    dataTemp.DefaultView.Sort = "Value  ASC";
                    dataTemp = dataTemp.DefaultView.ToTable();
                }

                ddlWorkflowValue.DataSource = dataTemp;
                ddlWorkflowValue.DataTextField = "Value";
                ddlWorkflowValue.DataValueField = "Value";
                ddlWorkflowValue.DataBind();
                ddlWorkflowValue.Items.Insert(0, (new ListItem("--Add New--", "")));
            }

        }
    }

    protected void gvWorkflow_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            GridDataItem item = (GridDataItem)e.Item;
            LinkButton lnkAddWorkflowValue = (LinkButton)item.FindControl("lnkAddWorkflowValue");
            LinkButton lnkDelWorkflowValue = (LinkButton)item.FindControl("lnkDelWorkflowValue");
            LinkButton lnkUpdateWorkflowValue = (LinkButton)item.FindControl("lnkUpdateWorkflowValue");


            if (e.CommandName.Equals("AddWorkflowValue"))
            {
                TextBox txtWorkflowValue = (TextBox)item.FindControl("txtWorkflowValue");
                DropDownList ddlWorkflowValue = (DropDownList)item.FindControl("ddlWorkflowValue");

                Boolean isExistItem = false;
                foreach (ListItem x in ddlWorkflowValue.Items)
                {
                    if (x.Text == txtWorkflowValue.Text.Trim())
                    {
                        isExistItem = true;
                        break;
                    }
                }

                if (txtWorkflowValue.Text.Trim() != string.Empty && !isExistItem)
                {
                    ddlWorkflowValue.Items.Add(new ListItem(txtWorkflowValue.Text.Trim(), txtWorkflowValue.Text.Trim()));
                    txtWorkflowValue.Text = string.Empty;
                    ddlWorkflowValue.SelectedValue = txtWorkflowValue.Text.Trim();
                }
            }
            else if (e.CommandName.Equals("UpdateWorkflowValue"))
            {

                TextBox txtWorkflowValue = (TextBox)item.FindControl("txtWorkflowValue");
                DropDownList ddlWorkflowValue = (DropDownList)item.FindControl("ddlWorkflowValue");
                if (txtWorkflowValue.Text.Trim() != string.Empty)
                {
                    ddlWorkflowValue.Items.Remove(new ListItem(ddlWorkflowValue.SelectedValue, ddlWorkflowValue.SelectedValue));
                    ddlWorkflowValue.Items.Add(new ListItem(txtWorkflowValue.Text.Trim(), txtWorkflowValue.Text.Trim()));
                    ddlWorkflowValue.SelectedValue = txtWorkflowValue.Text.Trim();

                }
                lnkAddWorkflowValue.Visible = true;
                lnkUpdateWorkflowValue.Visible = false;
                lnkDelWorkflowValue.Visible = false;
                txtWorkflowValue.Text = string.Empty;
            }
            else if (e.CommandName.Equals("DeleteWorkflowValue"))
            {

                TextBox txtWorkflowValue = (TextBox)item.FindControl("txtWorkflowValue");
                DropDownList ddlWorkflowValue = (DropDownList)item.FindControl("ddlWorkflowValue");

                ddlWorkflowValue.Items.Remove(new ListItem(ddlWorkflowValue.SelectedValue, ddlWorkflowValue.SelectedValue));
                ddlWorkflowValue.SelectedIndex = 0;
                lnkAddWorkflowValue.Visible = true;
                lnkUpdateWorkflowValue.Visible = false;
                lnkDelWorkflowValue.Visible = false;
                txtWorkflowValue.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlWorkflowFormat_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddl = (DropDownList)sender;
            GridDataItem row = (GridDataItem)ddl.NamingContainer;
            Panel pnlWorkflowValue = (Panel)row.FindControl("pnlWorkflowValue");
            if (row != null)
            {
                if (ddl.SelectedItem.Text == "Dropdown")
                    pnlWorkflowValue.Visible = true;
                else
                    pnlWorkflowValue.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlWorkflowValue_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddl = (DropDownList)sender;
            GridDataItem row = (GridDataItem)ddl.NamingContainer;
            LinkButton lnkAddWorkflowValue = (LinkButton)row.FindControl("lnkAddWorkflowValue");
            LinkButton lnkUpdateWorkflowValue = (LinkButton)row.FindControl("lnkUpdateWorkflowValue");
            LinkButton lnkDelWorkflowValue = (LinkButton)row.FindControl("lnkDelWorkflowValue");
            TextBox txtWorkflowValue = (TextBox)row.FindControl("txtWorkflowValue");
            if (ddl.SelectedIndex == 0)
            {
                lnkAddWorkflowValue.Visible = true;
                lnkUpdateWorkflowValue.Visible = false;
                lnkDelWorkflowValue.Visible = false;
                txtWorkflowValue.Text = string.Empty;
            }
            else
            {
                lnkAddWorkflowValue.Visible = false;
                lnkUpdateWorkflowValue.Visible = true;
                lnkDelWorkflowValue.Visible = true;
                txtWorkflowValue.Text = ddl.SelectedValue;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkAddNewRowWorkflow_Click(object sender, EventArgs e)
    {
        getDataOnWorkflowGrid();
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["WorkflowTable"];
        DataRow dr = dt.NewRow();

        dr["ID"] = 0;
        dr["Line"] = dt.Rows.Count + 1;
        dr["OrderNo"] = 0;
        dr["Label"] = "";
        dr["IsAlert"] = 0;
        dr["TeamMember"] = "";
        dr["Format"] = 0;
        dr["TeamMemberDisplay"] = "";
        dt.Rows.Add(dr);

        ViewState["WorkflowTable"] = dt;

        FillworkflowFormat();
        BindworkflowGrid();
        ReorderworkflowGridRow();
    }
    private void getDataOnWorkflowGrid()
    {
        try
        {
            DataTable dt = (DataTable)ViewState["WorkflowTable"];
            DataTable dtDetails = dt.Clone();
            DataTable dtWorkflowValues = new DataTable();
            dtWorkflowValues.Columns.Add("ID", typeof(int));
            dtWorkflowValues.Columns.Add("tblWorkfollowFieldsID", typeof(int));
            dtWorkflowValues.Columns.Add("Line", typeof(int));
            dtWorkflowValues.Columns.Add("Value", typeof(string));

            int line = 1;

            foreach (GridDataItem gr in gvWorkflow.Items)
            {
                Label lblID = (Label)gr.FindControl("lblID");
                Label lblIndex = (Label)gr.FindControl("lblIndex");
                Label lblLine = (Label)gr.FindControl("lblLine");
                TextBox txtLabel = (TextBox)gr.FindControl("txtLabel");
                CheckBox chkAlert = (CheckBox)gr.FindControl("chkSelectAlert");
                HiddenField OrderNo = (HiddenField)gr.FindControl("txtOrderNo");
                RadComboBox drpMember = (RadComboBox)gr.FindControl("ddlTeamMember");
                DropDownList ddlWorkflowFormat = (DropDownList)gr.FindControl("ddlWorkflowFormat");
                DropDownList ddlWorkflowValue = (DropDownList)gr.FindControl("ddlWorkflowValue");

                HiddenField hdSelectTeam = (HiddenField)gr.FindControl("hdnMembers");
                TextBox txtMembers = (TextBox)gr.FindControl("txtMembers");

                HiddenField hdSelectRoles = (HiddenField)gr.FindControl("hdnRoles");
                TextBox txtRoles = (TextBox)gr.FindControl("txtRoles");

                foreach (ListItem li in ddlWorkflowValue.Items)
                {
                    if (li.Value != string.Empty)
                    {

                        DataRow drCustomVal = dtWorkflowValues.NewRow();
                        drCustomVal["ID"] = 0;
                        drCustomVal["tblWorkflowFieldsID"] = Convert.ToInt32(lblID.Text);
                        drCustomVal["Line"] = line;
                        drCustomVal["Value"] = li.Value;
                        dtWorkflowValues.Rows.Add(drCustomVal);
                    }
                }

                DataRow dr = dtDetails.NewRow();
                dr["ID"] = Convert.ToInt32(lblID.Text);
                dr["Line"] = lblLine.Text;
                dr["OrderNo"] = Convert.ToInt32(OrderNo.Value);
                dr["Label"] = txtLabel.Text.Trim();
                dr["IsAlert"] = Convert.ToBoolean(chkAlert.Checked);
                dr["TeamMember"] = hdSelectTeam.Value;
                dr["Format"] = Convert.ToInt32(ddlWorkflowFormat.SelectedValue);
                dr["TeamMemberDisplay"] = txtMembers.Text;
                dtDetails.Rows.Add(dr);
                line++;
            }
            ViewState["WorkflowTable"] = dtDetails;
            ViewState["WorkflowValues"] = dtWorkflowValues;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void FillworkflowFormat()
    {
        try
        {
            dtWorkflowFormat = new DataTable();
            dtWorkflowFormat.Columns.Add("Value", typeof(string));
            dtWorkflowFormat.Columns.Add("Format", typeof(string));

            DataRow drWorkflow = dtWorkflowFormat.NewRow();


            List<string> lstCustom = System.Enum.GetNames(typeof(CommonHelper.CustomField)).ToList();

            int i = 0;
            foreach (var lst in lstCustom)
            {
                i = i + 1;
                drWorkflow = dtWorkflowFormat.NewRow();
                drWorkflow["Value"] = i;
                drWorkflow["Format"] = lst;

                dtWorkflowFormat.Rows.Add(drWorkflow);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void BindworkflowGrid()
    {
        try
        {

            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["WorkflowTable"];

            gvWorkflow.DataSource = dt;
            gvWorkflow.VirtualItemCount = dt.Rows.Count;
            gvWorkflow.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ReorderworkflowGridRow()
    {
        int count = 0;
        foreach (GridDataItem gr in gvWorkflow.Items)
        {
            HiddenField OrderNo = (HiddenField)gr.FindControl("txtOrderNo");
            OrderNo.Value = (count = count + 1).ToString();
        }
    }
    #endregion

    protected void ibtnDeleteWorkflowItem_Click(object sender, EventArgs e)
    {
        DeleteworkflowItem();
        ReorderworkflowGridRow();
    }
    private void DeleteworkflowItem()
    {
        try
        {
            getDataOnWorkflowGrid();

            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["WorkflowTable"];
            DataTable dtdeleted = dt.Clone();
            int count = 0;
            foreach (GridDataItem gr in gvWorkflow.Items)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                Label lblIndex = (Label)gr.FindControl("lblIndex");
                int index = Convert.ToInt32(lblIndex.Text) - 1;
                if (chkSelect.Checked == true)
                {
                    if (dt.Rows.Count > 0)
                    {
                        dtdeleted.ImportRow(dt.Rows[index - count]);
                        dt.Rows.RemoveAt(index - count);
                    }
                    count++;
                }
            }

            ViewState["WorkflowDeletedRows"] = dtdeleted;

            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();

                dr["ID"] = 0;
                dr["Line"] = dt.Rows.Count + 1;
                dr["OrderNo"] = 0;
                dr["Label"] = "";
                dr["IsAlert"] = 0;
                dr["TeamMember"] = "";
                dr["Format"] = 0;
                dr["TeamMemberDisplay"] = "";
                dt.Rows.Add(dr);
            }

            ViewState["WorkflowTable"] = dt;

            FillworkflowFormat();
            BindworkflowGrid();
            BindworkflowDropDown();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void BindworkflowDropDown()
    {
        int rowIndex = 1;
        try
        {
            foreach (GridDataItem gr in gvWorkflow.Items)
            {
                DropDownList ddlWorkflowFormat = (DropDownList)gr.FindControl("ddlWorkflowFormat");
                Label lblLine = (Label)gr.FindControl("lblLine");
                Panel pnlWorkflowValue = (Panel)gr.FindControl("pnlWorkflowValue");
                if (ddlWorkflowFormat.SelectedItem.Text == "Dropdown")
                {
                    pnlWorkflowValue.Visible = true;
                }
                else
                    pnlWorkflowValue.Visible = false;

                DropDownList ddlWorkflowValue = (DropDownList)gr.FindControl("ddlWorkflowValue");
                Label lblID = (Label)gr.FindControl("lblID");

                if (ViewState["WorkflowValues"] != null)
                {
                    DataTable dtWorkflowval = (DataTable)ViewState["WorkflowValues"];
                    DataTable dt = dtWorkflowval.Clone();
                    DataRow[] result = dtWorkflowval.Select("Line = " + Convert.ToInt32(lblLine.Text) + "");
                    foreach (DataRow row in result)
                    {
                        dt.ImportRow(row);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        dt.DefaultView.Sort = "Value  ASC";
                        dt = dt.DefaultView.ToTable();
                    }

                    ddlWorkflowValue.DataSource = dt;
                    ddlWorkflowValue.DataTextField = "Value";
                    ddlWorkflowValue.DataValueField = "Value";
                    ddlWorkflowValue.DataBind();
                    ddlWorkflowValue.Items.Insert(0, (new ListItem("--Add New--", "")));
                    if (ddlWorkflowFormat.SelectedItem.Text == "Dropdown")
                    {
                        rowIndex++;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void LnkSaveWorkflow_Click(object sender, EventArgs e)
    {
        try
        {
            getDataOnWorkflowGrid();

            DataTable dtworkflow = (DataTable)ViewState["WorkflowTable"];
            DataTable dtworkflowVal = (DataTable)ViewState["WorkflowValues"];
            DataTable dtDeleted = dtworkflow.Clone();

            if (ViewState["WorkflowDeletedRows"] != null)
                dtDeleted = (DataTable)ViewState["WorkflowDeletedRows"];


            Workflow obj = new Workflow();
            obj.ConnConfig = Session["config"].ToString();
            obj.workflowItem = dtworkflow;
            obj.workflowItemDelete = dtDeleted;
            obj.workflowValue = dtworkflowVal;
            BL_SafetyTest _bltest = new BL_SafetyTest();
            _bltest.CreateAndUpdateWorkflow(obj);


            CreateworkflowTable();
            FillworkflowFormat();
            BindworkflowGrid();
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccessMCPS", "noty({text: 'Test custom updated successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "errorMessage", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    private void CreateworkflowTable()
    {

        DataSet dst = new DataSet();
        BL_SafetyTest _objbltesttypes = new BL_SafetyTest();
        dst = _objbltesttypes.GetAllWorkflows(Session["config"].ToString());

        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("OrderNo", typeof(int));
        dt.Columns.Add("Label", typeof(string));
        dt.Columns.Add("IsAlert", typeof(Boolean));
        dt.Columns.Add("TeamMember", typeof(string));
        dt.Columns.Add("Format", typeof(int));
        dt.Columns.Add("TeamMemberDisplay", typeof(string));

        DataRow dr = dt.NewRow();

        dr["ID"] = 0;
        dr["Line"] = dt.Rows.Count + 1;
        dr["OrderNo"] = 0;
        dr["Label"] = "";
        dr["IsAlert"] = 0;
        dr["TeamMember"] = "";
        dr["Format"] = 0;
        dr["TeamMemberDisplay"] = "";

        dt.Rows.Add(dr);
        if (dst.Tables[0].Rows.Count == 0)
        {
            ViewState["WorkflowTable"] = dt;
            ViewState["WorkflowDeleteTable"] = dt;
        }
        else
        {
            ViewState["WorkflowTable"] = dst.Tables[0];
        }

        ViewState["WorkflowValues"] = dst.Tables[1];


    }

    protected void RadGrid_Emails_wf_PreRender(object sender, EventArgs e)
    {
        String filterExpression = Convert.ToString(RadGrid_Emails_wf.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Emails_wf_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Emails_wf.MasterTableView.OwnerGrid.Columns)
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

            Session["Emails_wf_Filters"] = filters;
        }
        else
        {
            Session["Emails_wf_FilterExpression"] = null;
            Session["Emails_wf_Filters"] = null;
        }

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "bindingClickCheckbox_wf", "BindClickEventForGridCheckBox_wf();", true);
    }

    protected void RadGrid_Emails_wf_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_Emails.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        if (!IsPostBack)
        {

            if (Session["Emails_wf_FilterExpression"] != null && Convert.ToString(Session["Emails_wf_FilterExpression"]) != "" && Session["Emails_wf_Filters"] != null)
            {
                RadGrid_Emails_wf.MasterTableView.FilterExpression = Convert.ToString(Session["Emails_wf_FilterExpression"]);
                var filtersGet = Session["Emails_Filters"] as List<RetainFilter>;
                if (filtersGet != null)
                {
                    foreach (var _filter in filtersGet)
                    {
                        GridColumn column = RadGrid_Emails_wf.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                        column.CurrentFilterValue = _filter.FilterValue;
                    }
                }
            }

        }

        InitTeamMemberGridView_wf();

    }

    private void InitTeamMemberGridView_wf()
    {
        User objPropUser = new User();
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        //objPropUser.Status = 0;
        ds = objBL_User.GetUsersAndRolesForTeamMemberList(objPropUser);
        var teamMembers = ds.Tables[0];

        // Get contacts list from exchange server       
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
                teamMembers.Rows.Add(_dr);
            }
        }

        RadGrid_Emails_wf.DataSource = teamMembers;
        if (teamMembers != null && teamMembers.Rows.Count > 0)
        {
            RadGrid_Emails_wf.VirtualItemCount = teamMembers.Rows.Count;
        }
        else
        {
            RadGrid_Emails_wf.VirtualItemCount = 0;
        }

    }

    protected void RadGrid_ViolationStatus_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();
        BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
        ds = bl_SafetyTest.GetAllViolationStatus(Session["config"].ToString());
        RadGrid_ViolationStatus.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_ViolationStatus.DataSource = ds.Tables[0];
    }

    protected void lnksaveViolationStatus_Click(object sender, EventArgs e)
    {
        try
        {
            BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
            ViolationStatus obj = new ViolationStatus();
            if (txtViolationStatusType.Text.Trim() != string.Empty)
            {
                obj.ConnConfig = Session["config"].ToString();
                obj.Type = txtViolationStatusType.Text;
                obj.Remark = txtViolationStatusRemark.Text;
                if (hdnViolationStatusID.Value == "0")
                {

                    bl_SafetyTest.AddViolationStatus(obj);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScriptViolationStatus", "CloseViolationStatusWindow();", true);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddViolationStatus", "noty({text: 'Violation Status added successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
                else
                {
                    obj.ID = Convert.ToInt32(hdnViolationStatusID.Value);
                    bl_SafetyTest.UpdateViolationStatus(obj);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScriptViolationStatus", "CloseViolationStatusWindow();", true);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUpdateViolationStatus", "noty({text: 'Violation Status updated successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }

                RadGrid_ViolationStatus.Rebind();

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string type = "error";
            if (str.Contains("This violation status already exists in the database. Please check and try again!"))
            {
                type = "warning";

            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyerrViolationStatus", "noty({text: '" + str + "', dismissQueue: true,  type : '" + type + "', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void lnkdeleteViolationStatus_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
            ViolationStatus obj = new ViolationStatus();

            foreach (GridDataItem gr in RadGrid_ViolationStatus.SelectedItems)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelectSelectCheckBox");
                HiddenField hdnid = (HiddenField)gr.FindControl("hdnid");

                if (chkSelect.Checked == true)
                {
                    IsDelete = true;
                    obj = new ViolationStatus();
                    obj.ConnConfig = Session["config"].ToString();
                    obj.ID = Convert.ToInt32(hdnid.Value);
                    bl_SafetyTest.DeleteViolationStatus(obj);
                    RadGrid_ViolationStatus.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccDeleteViolationStatus", "noty({text: 'Violation Status deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }


            }

            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningDelViolationStatus", "noty({text: 'Please select Violation Status  to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelViolationStatus", "noty({text: '" + str + "', dismissQueue: true, type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkdeleteViolationCode_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
            ViolationCode obj = new ViolationCode();

            foreach (GridDataItem gr in RadGrid_ViolationCode.SelectedItems)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelectSelectCheckBox");
                HiddenField hdnid = (HiddenField)gr.FindControl("hdnid");

                if (chkSelect.Checked == true)
                {
                    IsDelete = true;
                    obj = new ViolationCode();
                    obj.ConnConfig = Session["config"].ToString();
                    obj.ID = Convert.ToInt32(hdnid.Value);
                    bl_SafetyTest.DeleteViolationCode(obj);
                    RadGrid_ViolationCode.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccDeleteViolationCode", "noty({text: 'Violation Code deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }


            }

            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningDelViolationCode", "noty({text: 'Please select Violation Code to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelViolationCode", "noty({text: '" + str + "', dismissQueue: true, type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnksaveViolationCode_Click(object sender, EventArgs e)
    {
        try
        {
            BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
            ViolationCode obj = new ViolationCode();
            if (txtViolationCodeName.Text.Trim() != string.Empty)
            {
                obj.ConnConfig = Session["config"].ToString();
                obj.Code = txtViolationCodeName.Text;
                obj.Desc = txtViolationCodeDesc.Text;
                obj.SectionID = Convert.ToInt32(ddlViolationCodeSection.SelectedValue);
                obj.CategoryID = Convert.ToInt32(ddlViolationCodeCategory.SelectedValue);
                if (hdnViolationCodeID.Value == "0")
                {

                    bl_SafetyTest.AddViolationCode(obj);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScriptViolationCode", "CloseViolationCodeWindow();", true);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddViolationCode", "noty({text: 'Violation Code added successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
                else
                {
                    obj.ID = Convert.ToInt32(hdnViolationCodeID.Value);
                    bl_SafetyTest.UpdateViolationCode(obj);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScriptViolationCode", "CloseViolationCodeWindow();", true);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUpdateViolationCode", "noty({text: 'Violation Code updated successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }

                RadGrid_ViolationCode.Rebind();

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string type = "error";
            if (str.Contains("This violation status already exists in the database. Please check and try again!"))
            {
                type = "warning";

            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyerrViolationStatus", "noty({text: '" + str + "', dismissQueue: true,  type : '" + type + "', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void ddlViolationCodeCategory_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlViolationCodeSection_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void RadGrid_ViolationCode_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();
        BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
        ds = bl_SafetyTest.GetAllViolationCode(Session["config"].ToString());
        RadGrid_ViolationCode.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_ViolationCode.DataSource = ds.Tables[0];
    }

    private void FillViolationSection()
    {
        DataSet ds = new DataSet();
        BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
        ds = bl_SafetyTest.GetAllViolationSection(Session["config"].ToString());


        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {

                ddlViolationCodeSection.DataSource = ds.Tables[0];
                ddlViolationCodeSection.DataTextField = "Name";
                ddlViolationCodeSection.DataValueField = "ID";
                ddlViolationCodeSection.DataBind();
                ddlViolationCodeSection.Items.Insert(0, (new ListItem("--Add New--", "")));

            }
        }
    }

    private void FillViolationCategory()
    {
        DataSet ds = new DataSet();
        BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
        ds = bl_SafetyTest.GetAllViolationCategory(Session["config"].ToString());


        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {

                ddlViolationCodeCategory.DataSource = ds.Tables[0];
                ddlViolationCodeCategory.DataTextField = "Name";
                ddlViolationCodeCategory.DataValueField = "ID";
                ddlViolationCodeCategory.DataBind();
                ddlViolationCodeCategory.Items.Insert(0, (new ListItem("--Add New--", "")));

            }
        }
    }

    protected void lnksaveViolationSection_Click(object sender, EventArgs e)
    {
        try
        {
            BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
            ViolationCode obj = new ViolationCode();
            if (txtViolationSectionName.Text.Trim() != string.Empty)
            {
                obj.ConnConfig = Session["config"].ToString();
                obj.SectionName = txtViolationSectionName.Text;
                bl_SafetyTest.AddViolationSection(obj);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScriptViolationSection", "CloseViolationSectionWindow();", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddViolationSection", "noty({text: 'Violation Section added successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                FillViolationSection();

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string type = "error";
            if (str.Contains("This violation Section already exists in the database. Please check and try again!"))
            {
                type = "warning";

            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyerrViolationSection", "noty({text: '" + str + "', dismissQueue: true,  type : '" + type + "', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void lnksaveViolationCategory_Click(object sender, EventArgs e)
    {
        try
        {
            BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
            ViolationCode obj = new ViolationCode();
            if (txtViolationCategoryName.Text.Trim() != string.Empty)
            {
                obj.ConnConfig = Session["config"].ToString();
                obj.CategoryName = txtViolationCategoryName.Text;
                bl_SafetyTest.AddViolationCategory(obj);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScriptViolationCategory", "CloseViolationCategoryWindow();", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddViolationCategory", "noty({text: 'Violation Category added successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                FillViolationCategory();

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string type = "error";
            if (str.Contains("This violation category already exists in the database. Please check and try again!"))
            {
                type = "warning";

            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyerrViolationCategory", "noty({text: '" + str + "', dismissQueue: true,  type : '" + type + "', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void RadGrid_CallCodes_ItemCreated(object sender, GridItemEventArgs e)
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

    private void FillTestCover()
    {
        DataSet ds = new DataSet();
        TestTypes _objProptesttypes = new TestTypes();
        BL_SafetyTest _objbltesttypes = new BL_SafetyTest();

        _objProptesttypes.ConnConfig = WebBaseUtility.ConnectionString;
        txtTesttypeCount.ReadOnly = true;
        ds = _objbltesttypes.GetAllTestTypes(_objProptesttypes);
        ddlTestTypeCover.Items.Clear();
        ddlTestTypeCover.Items.Add(new RadComboBoxItem("Select Test Type", "0"));
        if (ds != null)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var cboItem = new RadComboBoxItem() { Text = ds.Tables[0].Rows[i]["Type"].ToString(), Value = ds.Tables[0].Rows[i]["ID"].ToString() };
                    ddlTestTypeCover.Items.Add(cboItem);

                }
            }
        }

    }
    private void FillTestSetupEmailForms()
    {

        DataSet ds = new DataSet();
        BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();
        ds = bl_SafetyTest.GetAllTestSetupEmailForms(Session["config"].ToString());

        gvEmailDocuments.VirtualItemCount = ds.Tables[0].Rows.Count;
        gvEmailDocuments.DataSource = ds.Tables[0];


    }

    protected void lnkDeleteEmailForm_Click(object sender, EventArgs e)
    {
        Boolean flag = false;
        foreach (GridDataItem di in gvEmailDocuments.SelectedItems)
        {
            //CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
            HiddenField lblID = (HiddenField)di.FindControl("hdnID");
            BL_SafetyTest bl_SafetyTest = new BL_SafetyTest();

            flag = true;

            bl_SafetyTest.DeleteTestSetupEmailForms(Session["config"].ToString(), Convert.ToInt32(lblID.Value));
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccDeleteEmailTemplate", "noty({text: 'Email template is deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);


        }
        if (!flag)
        {
            string str = "Please select item to delete";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddLoctype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

        }
        else
        {
            FillTestSetupEmailForms();
            gvEmailDocuments.Rebind();
        }
    }
    protected void btnRefressScreen_Click(object sender, EventArgs e)
    {
        FillTestSetupEmailForms();
        gvEmailDocuments.Rebind();

    }

    protected void gvEmailDocuments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        FillTestSetupEmailForms();

    }

    protected void RadGrid_StageProject_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_StageProject.AllowCustomPaging = !ShouldApplySortStageProject();
        FillStagesProject();
    }

    protected void RadGrid_StageProject_PreRender(object sender, EventArgs e)
    {
        foreach (GridDataItem gr in RadGrid_StageProject.Items)
        {
            Panel txtChartColors = (Panel)gr.FindControl("txtChartColors");
            Label lblChartColor = (Label)gr.FindControl("lblChartColor");
            txtChartColors.Style.Add("background-color", "#" + lblChartColor.Text);
        }
    }

    private void FillStagesProject()
    {
        DataSet ds = new DataSet();
        ds = objBL_Customer.GetAllProjectStages(Session["config"].ToString());
        RadGrid_StageProject.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_StageProject.DataSource = ds.Tables[0];
        lblStageProjectHeader.InnerText = "Stage";
        lblEditStageProject.InnerText = "Stage";
        lnkEditStageProjectHeader.Text = "Stage";
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            lblStageProjectHeader.InnerText = ds.Tables[0].Rows[0]["Label"].ToString();
            lblEditStageProject.InnerText = ds.Tables[0].Rows[0]["Label"].ToString();
            lnkEditStageProjectHeader.Text = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }
    bool isGroupingStageProject = false;
    public bool ShouldApplySortStageProject()
    {
        return RadGrid_StageProject.MasterTableView.FilterExpression != "" ||
            (RadGrid_StageProject.MasterTableView.GroupByExpressions.Count > 0 || isGroupingStageProject) ||
            RadGrid_StageProject.MasterTableView.SortExpressions.Count > 0;
    }

    protected void lnkUpdateStageProjectHeader_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtEditStageProject.Text != "")
            {
                objCustomer.ConnConfig = Session["config"].ToString();
                objCustomer.HeaderStage = txtEditStageProject.Text;
                objBL_Customer.UpdateStageProjectHeader(objCustomer);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseStageProjectWindow('Header');", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccStage", "noty({text: 'Header Updated Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                lblStageProjectHeader.InnerText = txtEditStageProject.Text;
                lnkEditStageProjectHeader.Text = txtEditStageProject.Text;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddStage", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    //protected void lnkSaveStageProject_Click(object sender, EventArgs e)
    //{
    //    IsAddEdit = false;
    //    string StageId = "0";
    //    int Count = 0;
    //    try
    //    {
    //        #region code to push data to db
    //        objCustomer.ConnConfig = Session["config"].ToString();
    //        string msg = string.Empty;
    //        foreach (GridDataItem di in RadGrid_StageProject.SelectedItems)
    //        {
    //            IsAddEdit = true;
    //            TableCell cell = di["chkSelect"];
    //            CheckBox chkSelect = (CheckBox)cell.Controls[0];
    //            StageId = di["lblStageId"].Text.Trim();
    //            Count = Convert.ToInt32(di["lblCount"].Text);
    //            if (chkSelect.Checked == true)
    //            {
    //                objCustomer.Mode = 1;

    //                msg = "Updated";
    //            }
    //        }
    //        if (!IsAddEdit)
    //        {
    //            objCustomer.Mode = 0;

    //            msg = "Added";
    //        }


    //        objCustomer.Stage = new Stage()
    //        {
    //            ID = StageId,
    //            Description = txtDescriptionProject.Text,
    //            ChartColor = ColorTranslator.ToHtml(txtChartColorProject.SelectedColor).Replace("#", ""),
    //            Count = Count,
    //            Label = lnkEditStageHeader.Text
    //        };

    //        objBL_Customer.AddStagesProject(objCustomer);
    //        #endregion
    //        FillStagesProject();
    //        RadGrid_StageProject.Rebind();

    //        ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseStageProjectWindow('Add');", true);
    //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddCusttype", "noty({text: 'Stage " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrPrStageSale", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}

    protected void lnkSaveStageProject_Click(object sender, EventArgs e)
    {
        IsAddEdit = false;
        string StageId = "0";
        int Count = 0;
        try
        {
            #region code to push data to db
            objCustomer.ConnConfig = Session["config"].ToString();
            string msg = string.Empty;
            foreach (GridDataItem di in RadGrid_StageProject.SelectedItems)
            {
                IsAddEdit = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                StageId = di["lblStageId"].Text.Trim();
                if (chkSelect.Checked == true)
                {
                    msg = "Updated";
                }
            }
            if (!IsAddEdit)
            {
                msg = "Added";
            }

            StageItemGridData();
            DataTable dtCustom = (DataTable)ViewState["StageItemTable"];

            DataTable dtCustomDelete = (DataTable)ViewState["StageItemDeletedRows"];
            if (dtCustomDelete == null)
            {
                dtCustomDelete = dtCustom.Clone();
            }

            objCustomer.Stage = new Stage()
            {
                ID = StageId,
                Description = txtDescriptionProject.Text,
                ChartColor = ColorTranslator.ToHtml(txtChartColorProject.SelectedColor).Replace("#", ""),
                Label = lnkEditStageHeader.Text,
                Items = dtCustom,
                DeleteItems = dtCustomDelete,
                // TODO: need to get list ID of Departments
                DepartmentIDs = txtUnit.Text
            };

            objBL_Customer.UpdateProjectStage(objCustomer);
            #endregion
            FillStagesProject();
            RadGrid_StageProject.Rebind();

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseStageProjectWindow('Add');", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddCusttype", "noty({text: 'Stage " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrPrStageSale", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkDelStageProject_Click(object sender, EventArgs e)
    {
        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_StageProject.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                string StageId = di["lblStageId"].Text.Trim();
                if (chkSelect.Checked == true)
                {
                    #region Add code to delete stage
                    objCustomer.ConnConfig = Session["config"].ToString();
                    objCustomer.Stage = new Stage()
                    {
                        ID = StageId
                    };
                    objBL_Customer.DeleteProjectStage(objCustomer);
                    #endregion
                    FillStagesProject();
                    RadGrid_StageProject.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddStageSale", "noty({text: 'Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningStageSale", "noty({text: 'Please select Stage to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelPrtype", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void RadGrid_StageDepartment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getDepartment(objProp_User);
        RadGrid_StageDepartment.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_StageDepartment.DataSource = ds.Tables[0];
    }

    protected void RadGrid_StageDepartment_PreRender(object sender, EventArgs e)
    {

    }

    protected void RadGrid_StageDepartment_DataBound(object sender, EventArgs e)
    {

    }

    protected void Page_PreRender(Object o, EventArgs e)
    {
        foreach (GridDataItem gr in RadGrid_StageDepartment.Items)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            chkSelect.Attributes["onclick"] = "SelectRowsUser();";
        }
    }

    protected void gvStageItem_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void gvStageItem_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    //DropDownList ddlFormat = (DropDownList)e.Row.FindControl("ddlFormat");
        //    //DropDownList ddlTab = (DropDownList)e.Row.FindControl("ddlTab");

        //    //Panel pnlCustomValue = (Panel)e.Row.FindControl("pnlCustomValue");
        //    //if (ddlFormat.SelectedItem.Text == "Dropdown")
        //    //    pnlCustomValue.Visible = true;
        //    //else
        //    //    pnlCustomValue.Visible = false;

        //    //DropDownList ddlCustomValue = (DropDownList)e.Row.FindControl("ddlCustomValue");
        //    //Label lblID = (Label)e.Row.FindControl("lblID");
        //    Label lblLine = (Label)e.Row.FindControl("lblLine");
        //    //TextBox txtMembers = (TextBox)e.Row.FindControl("txtMembers");
        //    //txtMembers.Attributes.Add("onclick", "ShowTeamMemberWindow("+ lblLine.Text + ", '"+ txtMembers.Text.Replace(" ","") + "')");

        //    if (ViewState["CustomValues"] != null)
        //    {
        //        DataTable dtCustomval = (DataTable)ViewState["CustomValues"];
        //        DataTable dt = dtCustomval.Clone();
        //        ////tblCustomTabID = " + Convert.ToInt32(lblID.Text) + " AND
        //        //DataRow[] result = dtCustomval.Select("Line = " + (e.Row.RowIndex + 1) + "");
        //        DataRow[] result = dtCustomval.Select("Line = " + Convert.ToInt32(lblLine.Text) + "");
        //        foreach (DataRow row in result)
        //        {
        //            dt.ImportRow(row);
        //        }

        //        if (dt.Rows.Count > 0)
        //        {
        //            dt.DefaultView.Sort = "Value  ASC";
        //            dt = dt.DefaultView.ToTable();
        //        }

        //        ddlCustomValue.DataSource = dt;
        //        ddlCustomValue.DataTextField = "Value";
        //        ddlCustomValue.DataValueField = "Value";
        //        ddlCustomValue.DataBind();
        //        ddlCustomValue.Items.Insert(0, (new ListItem("--Add New--", "")));
        //    }

        //    //if (ddlJobType.SelectedValue.Equals("0"))
        //    //{
        //    //    ddlTab.Enabled = false;
        //    //    ddlTab.SelectedValue = "0";
        //    //}
        //}
    }

    private void BindStageItemGrid(bool isRefresh = false, bool isUpdate = true)
    {
        General objPropGeneral = new General();
        BL_General objBL_General = new BL_General();
        try
        {
            DataTable dt = new DataTable();
            if (isRefresh)
            {

                var StageId = 0;
                if (isUpdate && hdnStageID.Value != "")
                {
                    StageId = Int32.Parse(hdnStageID.Value);
                }

                objPropGeneral.ScreenRefID = StageId;// In case of setup
                objPropGeneral.ConnConfig = Session["config"].ToString();
                var ds = objBL_General.GetStageItemsById(objPropGeneral);
                dt = ds.Tables[0];
                if (ds.Tables[1].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["MaxLine"].ToString()))
                    {
                        ViewState["maxLine"] = Convert.ToInt16(ds.Tables[1].Rows[0]["MaxLine"].ToString());
                    }
                    else
                    {
                        ViewState["maxLine"] = 0;
                    }
                }
                else
                {
                    ViewState["maxLine"] = 0;
                }

                UpdateSelectedDepartments(ds.Tables[2]);

                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = 0;
                    dr["Label"] = DBNull.Value;
                    dr["Line"] = 0;
                    dr["OrderNo"] = 0;
                    dr["IsAlert"] = 0;
                    dr["TeamMember"] = DBNull.Value;
                    dr["TeamMemberDisplay"] = DBNull.Value;

                    dt.Rows.Add(dr);
                }
                ViewState["StageItemTable"] = dt;
                //ViewState["CustomValues"] = ds.Tables[1];
            }
            else
            {
                if (ViewState["StageItemTable"] != null)
                    dt = (DataTable)ViewState["StageItemTable"];
                else
                {
                    objPropGeneral.ScreenRefID = 0;// In case of setup
                    objPropGeneral.ConnConfig = Session["config"].ToString();
                    var ds = objBL_General.GetStageItemsById(objPropGeneral);
                    dt = ds.Tables[0];
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(ds.Tables[1].Rows[0]["MaxLine"].ToString()))
                        {
                            ViewState["maxLine"] = Convert.ToInt16(ds.Tables[1].Rows[0]["MaxLine"].ToString());
                        }
                        else
                        {
                            ViewState["maxLine"] = 0;
                        }
                    }
                    else
                    {
                        ViewState["maxLine"] = 0;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        DataRow dr = dt.NewRow();
                        dr["ID"] = 0;
                        dr["Label"] = DBNull.Value;
                        dr["Line"] = 0;
                        dr["OrderNo"] = 0;
                        dr["IsAlert"] = 0;
                        dr["TeamMember"] = DBNull.Value;
                        dr["TeamMemberDisplay"] = DBNull.Value;

                        dt.Rows.Add(dr);
                    }
                    ViewState["StageItemTable"] = dt;
                }
            }

            gvStageItem.DataSource = dt;
            gvStageItem.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkAddStageItem_Click(object sender, EventArgs e)
    {
        try
        {
            StageItemGridData(false);
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["StageItemTable"];
            DataRow dr = dt.NewRow();

            dr["ID"] = 0;
            dr["Label"] = DBNull.Value;
            dr["Line"] = 0;
            dr["OrderNo"] = dt.Rows.Count + 1;
            dr["IsAlert"] = 0;
            dr["TeamMember"] = DBNull.Value;
            dr["TeamMemberDisplay"] = DBNull.Value;
            dt.Rows.Add(dr);

            ViewState["StageItemTable"] = dt;
            BindStageItemGrid();
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkDelStageItem_Click(object sender, EventArgs e)
    {
        try
        {
            DeleteStageItem();
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void StageItemGridData(bool isValidation = true)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["StageItemTable"];
            DataTable dtDetails = dt.Clone();
            int line = 0;
            var tempLine = 0;
            //var curLine = 0;
            foreach (GridViewRow gr in gvStageItem.Rows)
            {
                line++;
                Label lblID = (Label)gr.FindControl("lblID");
                Label lblIndex = (Label)gr.FindControl("lblIndex");
                Label lblLine = (Label)gr.FindControl("lblLine");
                TextBox lblDesc = (TextBox)gr.FindControl("lblDesc");
                HiddenField OrderNo = (HiddenField)gr.FindControl("txtRowLine");
                CheckBox chkSelectAlert = (CheckBox)gr.FindControl("chkSelectAlert");
                HiddenField hdnMembers = (HiddenField)gr.FindControl("hdnMembers");
                TextBox txtMembers = (TextBox)gr.FindControl("txtMembers");

                if (isValidation)
                {
                    if (lblLine != null && lblLine.Text != "0")
                    {
                        tempLine = Convert.ToInt16(lblLine.Text);
                        //curLine = Convert.ToInt16(lblLine.Text);
                    }
                    else
                    {

                        if (Convert.ToInt16(ViewState["maxLine"]) < line)
                        {
                            ViewState["maxLine"] = line;

                        }
                        else
                        {
                            line = Convert.ToInt16(ViewState["maxLine"]) + 1;
                            ViewState["maxLine"] = line;
                        }
                        tempLine = line;

                        //curLine = tempLine;
                    }

                    //custom items values of Grid
                    DataRow dr = dtDetails.NewRow();
                    dr["ID"] = Convert.ToInt32(lblID.Text);
                    dr["Label"] = lblDesc.Text.Trim();
                    dr["Line"] = tempLine;
                    dr["OrderNo"] = Convert.ToInt32(OrderNo.Value);
                    dr["IsAlert"] = chkSelectAlert.Checked;
                    dr["TeamMember"] = hdnMembers.Value;
                    dr["TeamMemberDisplay"] = txtMembers.Text;
                    dtDetails.Rows.Add(dr);
                    //line++;

                }
                else
                {
                    if (lblLine != null && lblLine.Text != "0")
                    {
                        tempLine = Convert.ToInt16(lblLine.Text);
                        //curLine = Convert.ToInt16(lblLine.Text);
                    }
                    else
                    {

                        if (Convert.ToInt16(ViewState["maxLine"]) < line)
                        {
                            ViewState["maxLine"] = line;

                        }
                        else
                        {
                            line = Convert.ToInt16(ViewState["maxLine"]) + 1;
                            ViewState["maxLine"] = line;
                        }
                        tempLine = line;

                        //curLine = tempLine;
                    }

                    //custom items values of Grid
                    DataRow dr = dtDetails.NewRow();
                    dr["ID"] = Convert.ToInt32(lblID.Text);
                    dr["Label"] = lblDesc.Text.Trim();
                    dr["Line"] = tempLine;
                    dr["OrderNo"] = Convert.ToInt32(OrderNo.Value);
                    dr["IsAlert"] = chkSelectAlert.Checked;
                    dr["TeamMember"] = hdnMembers.Value;
                    dr["TeamMemberDisplay"] = txtMembers.Text;
                    dtDetails.Rows.Add(dr);
                }
            }
            ViewState["StageItemTable"] = dtDetails;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Delete Custom field item from Custom gridview
    /// </summary>
    private void DeleteStageItem()
    {
        try
        {
            StageItemGridData(false);

            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["StageItemTable"];
            DataTable dtdeleted = dt.Clone();
            int count = 0;
            foreach (GridViewRow gr in gvStageItem.Rows)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                Label lblIndex = (Label)gr.FindControl("lblIndex");
                int index = Convert.ToInt32(lblIndex.Text) - 1;
                if (chkSelect.Checked == true)
                {
                    if (dt.Rows.Count > 0)
                    {
                        dtdeleted.ImportRow(dt.Rows[index - count]);
                        dt.Rows.RemoveAt(index - count);
                    }

                    count++;
                }
            }

            ViewState["StageItemDeletedRows"] = dtdeleted;

            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = 0;
                dr["Label"] = DBNull.Value;
                dr["Line"] = dt.Rows.Count + 1;
                dr["OrderNo"] = 0;
                dr["IsAlert"] = 0;
                dr["TeamMember"] = DBNull.Value;
                dr["TeamMemberDisplay"] = DBNull.Value;
                dt.Rows.Add(dr);
            }

            ViewState["StageItemTable"] = dt;
            BindStageItemGrid();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkEditStageProject_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_StageProject.SelectedItems)
        {
            hdnStageID.Value = di["lblStageId"].Text.Trim();
        }
        BindStageItemGrid(true, true);

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "open", "OpenStageProjectWindow('Edit Stage');", true);

    }

    protected void lnkAddStageProject_Click(object sender, EventArgs e)
    {
        hdnStageID.Value = "0";
        BindStageItemGrid(true, false);

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "open", "OpenStageProjectWindow('Add Stage');", true);
    }

    private void UpdateSelectedDepartments(DataTable users)
    {
        // Reset all checkbox of RadgvEquip
        foreach (GridDataItem gr in RadGrid_StageDepartment.Items)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            chkSelect.Checked = false;
        }

        foreach (DataRow dr in users.Rows)
        {
            foreach (GridDataItem gr in RadGrid_StageDepartment.Items)
            {
                Label lblID = (Label)gr.FindControl("lblID");
                Label lblname = (Label)gr.FindControl("lblunit");
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                if (dr["ID"].ToString() == lblID.Text)
                {
                    chkSelect.Checked = true;

                    if (txtUnit.Text != string.Empty)
                    {
                        txtUnit.Text = txtUnit.Text + ", " + lblID.Text;
                    }
                    else
                    {
                        txtUnit.Text = lblID.Text;
                    }
                }
            }
        }

    }

    protected void chkApproveEstimates_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            var isApprove = chkApproveEstimates.Checked;

            objBL_General.UpdateSalesApproveEstimate(Session["config"].ToString(), isApprove);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyUpdateSucc", "noty({text: 'Approve estimate option updated successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyUpdateErr", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }

    }

    private void FillSalesApproveEstimate()
    {
        var isApprove = objBL_General.GetSalesApproveEstimate(Session["config"].ToString());
        chkApproveEstimates.Checked = isApprove;
    }
    //private void UpdateStage()
    //{
    //    #region Custom

    //    StageItemGridData();
    //    DataTable dtCustom = (DataTable)ViewState["StageItemTable"];
    //    //if (!dtCustom.Columns.Contains("UpdatedDate"))
    //    //{
    //    //    dtCustom.Columns.Add("UpdatedDate", typeof(DateTime));
    //    //}
    //    //if (!dtCustom.Columns.Contains("Username"))
    //    //{
    //    //    dtCustom.Columns.Add("Username", typeof(string));
    //    //}

    //    DataTable dtCustomDelete = (DataTable)ViewState["StageItemDeletedRows"];
    //    if (dtCustomDelete == null)
    //    {
    //        dtCustomDelete = dtCustom.Clone();
    //    }
    //    //else
    //    //{
    //    //    if (!dtCustomDelete.Columns.Contains("UpdatedDate"))
    //    //    {
    //    //        dtCustomDelete.Columns.Add("UpdatedDate", typeof(DateTime));
    //    //    }
    //    //    if (!dtCustomDelete.Columns.Contains("Username"))
    //    //    {
    //    //        dtCustomDelete.Columns.Add("Username", typeof(string));
    //    //    }
    //    //}


    //    //dtCustom.AcceptChanges();
    //    objPropGeneral.CustomItems = dtCustom;
    //    objPropGeneral.CustomItemsValue = dtCustomVal;
    //    objPropGeneral.CustomItemsDelete = dtCustomDelete;
    //    objPropGeneral.Screen = "Estimate";
    //    objPropGeneral.ConnConfig = Session["config"].ToString();

    //    objBL_General.UpdateCustomFields(objPropGeneral);

    //    #endregion
    //}
}
