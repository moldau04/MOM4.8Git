using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using Telerik.Web.UI;

public partial class AddInventoryWarehouse : System.Web.UI.Page
{

    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();

    BusinessEntity.CompanyOffice objCompany = new BusinessEntity.CompanyOffice();
    BL_Company objBL_Company = new BL_Company();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");

        }
        Permission();
        if (!IsPostBack)
        {
            Locations();
            TruckEmployee();
            CompanyPermission();
            if (Request.QueryString["uid"] == null)
            {
                txtWarehouseID.Enabled = true;
                ddlCompany.Visible = true;
                txtCompany.Visible = false;
                btnCompanyPopUp.Visible = false;
            }
            if (Request.QueryString["uid"] != null)
            {
                txtWarehouseID.Enabled = false;
                ViewState["mode"] = 1;
                ddlCompany.Visible = false;
                txtCompany.Visible = true;
                btnCompanyPopUp.Visible = true;
                lblHeader.Text = "Edit Warehouse";
                FillInventoryWarehouseByID();
                FillWarehouseLocation();
            }
        }
    }

    private void Permission()
    {
        try
        {
            if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
            {
                DataTable ds = new DataTable();
                ds = (DataTable)Session["userinfo"];
                #region Inventory edit button permission
                string InventoryWarehousePermission = ds.Rows[0]["Warehouse"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Warehouse"].ToString();
                String addInventory = InventoryWarehousePermission.Length < 1 ? "Y" : InventoryWarehousePermission.Substring(0, 1);
                String editInventory = InventoryWarehousePermission.Length < 2 ? "Y" : InventoryWarehousePermission.Substring(1, 1);
                if (Request.QueryString["uid"] != null)
                {
                    if (editInventory == "N")
                    {
                        btnSubmit.Visible = false;
                    }
                    else
                    {
                        btnSubmit.Visible = true;
                    }
                }
                else
                {
                    if (addInventory == "N")
                    {
                        btnSubmit.Visible = false;
                    }
                    else
                    {
                        btnSubmit.Visible = true;
                    }
                }
                #endregion
            }
        }
        catch (Exception)
        {
            btnSubmit.Visible = false;
        }
    }
    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            ViewState["CompPermission"] = 1;
            dvCompanyPermission.Visible = true;
            FillCompany();
        }
        else
        {
            ViewState["CompPermission"] = 0;
            dvCompanyPermission.Visible = false;
        }
    }
    private void FillCompany()
    {
        objCompany.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Company.getCompanyByCustomer(objCompany);
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlCompany.DataSource = ds.Tables[0];
            ddlCompany.DataTextField = "Name";
            ddlCompany.DataValueField = "CompanyID";
            ddlCompany.DataBind();
            ddlCompany.Items.Insert(0, new ListItem("Select", "0"));

            ddlCompanyEdit.DataSource = ds.Tables[0];
            ddlCompanyEdit.DataTextField = "Name";
            ddlCompanyEdit.DataValueField = "CompanyID";
            ddlCompanyEdit.DataBind();
            ddlCompanyEdit.Items.Insert(0, new ListItem("Select", "0"));
        }
    }
    private void FillInventoryWarehouseByID()
    {
        objProp_User.WarehouseID = Request.QueryString["uid"];
        objProp_User.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_User.GetInventoryWarehouseByID(objProp_User);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(objProp_User.WarehouseID))
                txtWarehouseID.Enabled = false;
            txtWarehouseID.Text = ds.Tables[0].Rows[0]["ID"].ToString();
            txtWarehouseName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
            txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
            // txtCount.Text = ds.Tables[0].Rows[0]["Count"].ToString();
            ddlType.SelectedValue = ds.Tables[0].Rows[0]["Type"].ToString();
            chkMultiSelect.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Multi"]);
            ddlCompany.SelectedValue = ds.Tables[0].Rows[0]["EN"].ToString();
            txtCompany.Text = ds.Tables[0].Rows[0]["Company"].ToString();
            ddlCompanyEdit.SelectedValue = ds.Tables[0].Rows[0]["EN"].ToString();
            ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
            if (chkMultiSelect.Checked == true)
            {
                WarehouselocationDiv.Style.Add("display", "inline");
            }
            else
            {
                WarehouselocationDiv.Style.Add("display", "none");
            }
            if (ddlType.SelectedValue == "0")
            {
                LocationDiv.Visible = false;

                TruckEmployeeDiv.Visible = true;
                ddlTruckEmployee.SelectedValue = ds.Tables[0].Rows[0]["Location"].ToString();
            }
            else if (ddlType.SelectedValue == "1")
            {
                TruckEmployeeDiv.Visible = false;

                LocationDiv.Visible = true;
                ddlLocation.SelectedValue = ds.Tables[0].Rows[0]["Location"].ToString();
            }
            else if (ddlType.SelectedValue == "2")
            {
                TruckEmployeeDiv.Visible = false;
                LocationDiv.Visible = false;

                ddlLocation.SelectedValue = ds.Tables[0].Rows[0]["Location"].ToString();
            }
            else
            {
                LocationDiv.Visible = false;
                TruckEmployeeDiv.Visible = false;
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Submit();
    }
    private void Submit()
    {
        string msg = "";
        String ReturnWarehouseID = "";
        try
        {
            //if (!string.IsNullOrEmpty(Request.QueryString["uid"]))
            //{
            //    if (Request.QueryString["uid"].Trim() != txtWarehouseID.Text.Trim())
            //    {
            //        string url = HttpContext.Current.Request.Url.AbsoluteUri;
            //        string[] separateURL = url.Split('?');
            //        NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(separateURL[1]);
            //        queryString["uid"] = txtWarehouseID.Text;
            //        url = separateURL[0] + "?" + queryString.ToString();
            //        //Request.QueryString["uid"] = txtWarehouseID.Text;
            //    }
            //}

            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.WarehouseID = txtWarehouseID.Text;

            
            objProp_User.WarehouseName = txtWarehouseName.Text;
            objProp_User.Remarks = txtRemarks.Text;
            //     objProp_User.Count = Convert.ToInt32(txtCount.Text ==string.Empty ? "0": txtCount.Text);
            objProp_User.Type = ddlType.SelectedValue;
            if (chkMultiSelect.Checked == true)
            {
                objProp_User.IsMultiValue = true;
            }
            else
            {
                objProp_User.IsMultiValue = false;
            }
            if (ddlType.SelectedValue == "0")
            {
                objProp_User.LocID = Convert.ToInt32(ddlTruckEmployee.SelectedValue);
            }
            else if (ddlType.SelectedValue == "1")
            {
                objProp_User.LocID = Convert.ToInt32(ddlLocation.SelectedValue);
            }
            else
            {
                objProp_User.LocID = 0;
            }
            if (ddlStatus.SelectedValue == "0")
            {
                objProp_User.Status = Convert.ToInt32(ddlStatus.SelectedValue);
            }
            if (ddlStatus.SelectedValue == "1")
            {
                objProp_User.Status = Convert.ToInt32(ddlStatus.SelectedValue);
            }
            // Add code for status end
            if (Convert.ToInt32(ViewState["mode"]) == 1)
            {
                if (Convert.ToInt32(ViewState["CompPermission"]) == 1)
                    objProp_User.EN = Convert.ToInt32(ddlCompanyEdit.SelectedValue);
                else
                    objProp_User.EN = 0;
                msg = "Updated";

                if (objProp_User.WarehouseID == "OFC")
                {
                    if (ddlStatus.SelectedValue == "1")
                    {
                        ReturnWarehouseID = objProp_User.WarehouseID;
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: 'This is a default warehouse OFC and cannot be made inactive.', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }
                    else
                    {
                        objBL_User.UpdateInventoryWarehouse(objProp_User);
                        ReturnWarehouseID = objProp_User.WarehouseID;
                    }
                }                
                else
                {
                    objBL_User.UpdateInventoryWarehouse(objProp_User);
                    ReturnWarehouseID = objProp_User.WarehouseID;
                }
            }
            else
            {
                if (Convert.ToInt32(ViewState["CompPermission"]) == 1)
                    objProp_User.EN = Convert.ToInt32(ddlCompany.SelectedValue);
                else
                    objProp_User.EN = 0;
                msg = "Added";
                ReturnWarehouseID = objBL_User.CreateWarehouse(objProp_User);
                if (ReturnWarehouseID == "Already Exist")
                {
                    msg = "Already Exist";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningWarehouse", "noty({text: 'Warehouse already Exist.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);                  
                }                
            }           
            if (msg == "Added")
                Response.Redirect("AddInventoryWarehouse.aspx?uid=" + ReturnWarehouseID + "&Fun=Added");

            if (msg == "Updated")
            {
                if (objProp_User.WarehouseID != "OFC")
                {
                    Response.Redirect("AddInventoryWarehouse.aspx?uid=" + ReturnWarehouseID + "&Fun=Updated");
                }
                else if (ddlStatus.SelectedValue == "0")
                    Response.Redirect("AddInventoryWarehouse.aspx?uid=" + ReturnWarehouseID + "&Fun=Updated");
                else
                    Response.Redirect("AddInventoryWarehouse.aspx?uid=" + objProp_User.WarehouseID + "&Fun=OFC");
            }
            if (msg == "Already Exist")
              ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningWarehouse", "noty({text: 'Already Exist', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);            
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddWarehousetype1", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }       
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("Setup.aspx?tabWarehouse=Warehouse");
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlType.SelectedValue == "0")
        {

            LocationDiv.Visible = false;
            TruckEmployeeDiv.Visible = true;
        }
        else if (ddlType.SelectedValue == "1")
        {

            TruckEmployeeDiv.Visible = false;
            LocationDiv.Visible = true;
        }
        else if (ddlType.SelectedValue == "2")
        {
            TruckEmployeeDiv.Visible = false;
            LocationDiv.Visible = false;
        }
        else
        {

            LocationDiv.Visible = false;
            TruckEmployeeDiv.Visible = false;
        }

    }

    #region BindControls

    private void Locations()
    {
        DataSet ds = new DataSet();

        objProp_User.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getAllLocation(objProp_User);

        ddlLocation.DataSource = ds.Tables[0];
        ddlLocation.DataTextField = "tag";
        ddlLocation.DataValueField = "loc";
        ddlLocation.DataBind();

        ddlLocation.Items.Insert(0, new ListItem("Select", "0"));
    }
    private void TruckEmployee()
    {
        DataSet ds = new DataSet();

        objProp_User.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getAlltblWork(objProp_User);

        ddlTruckEmployee.DataSource = ds.Tables[0];
        ddlTruckEmployee.DataTextField = "fdesc";
        ddlTruckEmployee.DataValueField = "ID";
        ddlTruckEmployee.DataBind();

        ddlTruckEmployee.Items.Insert(0, new ListItem("Select", "0"));
    }
    #endregion

    private void ClearAll()
    {
        txtWarehouseID.Text = string.Empty;
        txtWarehouseName.Text = string.Empty;
        txtRemarks.Text = string.Empty;
        //  txtCount.Text = string.Empty;
        ddlType.SelectedIndex = 0;
        ddlTruckEmployee.SelectedIndex = 0;
        ddlLocation.SelectedIndex = 0;
    }

    protected void lnkDeleteWarehouseLocation_Click(object sender, EventArgs e)
    {
        bool IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_WarehouseLocation.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                Label lblId = (Label)di.FindControl("lblId");

                if (chkSelect.Checked == true)
                {
                    objProp_User.ConnConfig = Session["config"].ToString();
                    objProp_User.WHLocID = Convert.ToInt32(lblId.Text);

                    objBL_User.DeleteWareHouseLocation(objProp_User);
                    FillWarehouseLocation();
                    RadGrid_WarehouseLocation.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddWarehouse", "noty({text: 'Warehouse Location Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningWarehouse", "noty({text: 'Please select Warehouse Location to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelWarehouse", "noty({text: '" + str + "', dismissQueue: true, type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkSaveWarehouselocation_Click(object sender, EventArgs e)
    {
        String retVal = "";
        try
        {
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.WarehouseID = txtWarehouseID.Text;
            objProp_User.WareHouseLocation = txtLocationName.Text;

            string msg = "Added";

            if (Convert.ToString(hdnAddEditWarehouse.Value) == "0")
            {
                if (Request.QueryString["uid"] != null && Convert.ToString(Request.QueryString["uid"]) != "")
                {
                    objProp_User.IsEdit = true;
                }
                else
                {
                    objProp_User.IsEdit = false;
                }
                retVal = objBL_User.CreateWareHouseLocation(objProp_User);
            }
            else if (Convert.ToString(hdnAddEditWarehouse.Value) == "1")
            {
                msg = "Updated";
                objProp_User.WHLocID = Convert.ToInt32(hdnWHLocId.Value);
                retVal = objBL_User.UpdateWareHouseLocation(objProp_User);
            }
            txtLocationName.Text = "";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseWarehouseLocationWindow();", true);

            if (retVal == "success")
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddWarehouse", "noty({text: 'Warehouse Location " + msg + " successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                FillWarehouseLocation();
                RadGrid_WarehouseLocation.Rebind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddWarehousetype1", "noty({text: '" + retVal + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddWarehousetype1", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    private void FillWarehouseLocation()
    {
        if (chkMultiSelect.Checked == true)
        {
            WarehouselocationDiv.Style.Add("display", "inline");
        }
        else
        {
            WarehouselocationDiv.Style.Add("display", "none");
        }
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.WarehouseID = txtWarehouseID.Text;
        ds = objBL_User.GetWareHouseLocation(objProp_User);

        RadGrid_WarehouseLocation.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_WarehouseLocation.DataSource = ds.Tables[0];

    }
    bool isGroupingWarehouseLocation = false;
    public bool ShouldApplySortWarehouseLocation()
    {
        return RadGrid_WarehouseLocation.MasterTableView.FilterExpression != "" ||
            (RadGrid_WarehouseLocation.MasterTableView.GroupByExpressions.Count > 0 || isGroupingWarehouseLocation) ||
            RadGrid_WarehouseLocation.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_WarehouseLocation_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_WarehouseLocation.AllowCustomPaging = !ShouldApplySortWarehouseLocation();
        FillWarehouseLocation();
    }
    protected void btnCompanyEdit_Click(object sender, EventArgs e)
    {
        Submit();
        if (Convert.ToInt32(ViewState["CompPermission"]) == 1)
        {
            Response.Redirect(Request.RawUrl);
        }
    }
}