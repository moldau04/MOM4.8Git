using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using BusinessEntity;
using BusinessLayer;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using BusinessEntity.InventoryModel;
using BusinessEntity.Utility;
using MOMWebApp;

public partial class AddInventoryAdjustment : System.Web.UI.Page
{
    BL_Inventory blinventory = new BL_Inventory();
    InventoryAdjustment invadj = new InventoryAdjustment();

    //API Variables
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    GetInventoryAdjustmentByIDParam _GetInventoryAdjustmentByID = new GetInventoryAdjustmentByIDParam();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            divMessage.Visible = false;
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }
            if (!IsPostBack)
            {

                SetInitialRow();
                if (Request.QueryString["id"] != null)
                {
                    var id = Convert.ToInt32(Request.QueryString["id"]);
                    invadj = new InventoryAdjustment();
                    blinventory = new BL_Inventory();
                    lblHeader.Text = "Edit Inventory Adjustment";
                    invadj.ConnConfig = Session["config"].ToString();
                    invadj.ID = id;


                    _GetInventoryAdjustmentByID = new GetInventoryAdjustmentByIDParam();
                    _GetInventoryAdjustmentByID.ConnConfig = Session["config"].ToString();
                    _GetInventoryAdjustmentByID.ID = id;

                    DataSet ds = new DataSet();
                    List <GetInventoryAdjustmentByIDViewModel> _lstGetInventoryAdjustmentByID = new List<GetInventoryAdjustmentByIDViewModel>();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "ItemAdjustmentAPI/AddInventoryAdjustments_GetInventoryAdjustmentByID";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInventoryAdjustmentByID);
                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        serializer.MaxJsonLength = Int32.MaxValue;

                        _lstGetInventoryAdjustmentByID = serializer.Deserialize<List<GetInventoryAdjustmentByIDViewModel>>(_APIResponse.ResponseData);

                        ds = CommonMethods.ToDataSet<GetInventoryAdjustmentByIDViewModel>(_lstGetInventoryAdjustmentByID);
                    }
                    else
                    {
                        ds = blinventory.GetInventoryAdjustmentByID(invadj);
                    }

                    btnSubmit.Visible = false;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gvGLItems.DataSource = ds.Tables[0];
                        gvGLItems.DataBind();
                    }

                    UpdatePlaybackButton(id);
                }
                else
                {
                    lnkFirst.Visible = false;
                    lnkNext.Visible = false;
                    lnkPrevious.Visible = false;
                    lnkLast.Visible = false;
                }
                GetInvDefaultAcct();

            }
            CompanyPermission();
           // GetPeriodDetails();
            HighlightSideMenu("InventoryMgr", "lnkAdjustment", "ulInventoryMgr");
                       

        }
        catch (Exception ex)
        {
            throw ex;
        }


    }
    private void GetInvDefaultAcct()
    {
        General _objPropGeneral = new General();
        BL_General _objBLGeneral = new BL_General();

        _objPropGeneral.ConnConfig = Session["config"].ToString();

        DataSet _dsDefaultAccount = new DataSet();

        _dsDefaultAccount = _objBLGeneral.getInvDefaultAcct(_objPropGeneral);

        if (_dsDefaultAccount.Tables[0].Rows.Count > 0)
        {
            hdnInvDefaultAcctID.Value = Convert.ToString(_dsDefaultAccount.Tables[0].Rows[0]["ID"]);
            hdnInvDefaultAcctName.Value = Convert.ToString(_dsDefaultAccount.Tables[0].Rows[0]["Acct"]);
        }

        DataSet _dsCustom = new DataSet();
        _objPropGeneral.ConnConfig = Session["config"].ToString();
        _dsCustom = _objBLGeneral.getCustomField(_objPropGeneral, "InvGL");
        
        if (_dsCustom.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
            {
                if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() != "0")
                {
                    hdnIsInvTrackingOn.Value = Convert.ToString(_dr["Label"]);
                }
            }
        }

    }
    private bool chkInvTracking()
    {
        General _objPropGeneral = new General();
        BL_General _objBLGeneral = new BL_General();
        DataSet _dsCustom = new DataSet();
        _objPropGeneral.ConnConfig = Session["config"].ToString();
        _dsCustom = _objBLGeneral.getCustomField(_objPropGeneral, "InvGL");
        Boolean TrackingInventory = false;
        if (_dsCustom.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
            {
                if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() != "0")
                {
                    TrackingInventory = Convert.ToBoolean(_dr["Label"]);
                }
            }
        }
        return TrackingInventory;
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

    private void UpdatePlaybackButton(int id)
    {
        DisableAllPlaybackButtons();

        var dt = Session["InventoryAdjustments"] as DataTable;

        if(dt == null || dt.Rows.Count < 2)
        {
            return;
        }

        var keyColumn = dt.Columns["ID"];
        
        dt.PrimaryKey = new DataColumn[] { keyColumn };
        DataRow d = dt.Rows.Find(id.ToString());
        if (d != null)
        {

            lblRefNumber.Text = string.Format("Ref Number: {0} | Item Name: {1}", id.ToString(), d["Name"].ToString());
            int index = dt.Rows.IndexOf(d);
            if (index > 0)
            {
                lnkFirst.Enabled = true;
                lnkPrevious.Enabled = true;
            }

            if (index < dt.Rows.Count - 1)
            {
                lnkLast.Enabled = true;
                lnkNext.Enabled = true;
            }
        }
    }

    private void DisableAllPlaybackButtons()
    {
        lnkFirst.Enabled = false;
        lnkNext.Enabled = false;
        lnkPrevious.Enabled = false;
        lnkLast.Enabled = false;
    }

    protected void Page_PreRender(Object o, EventArgs e)
    {
        foreach (GridDataItem gr in gvGLItems.Items)
        {
            TextBox txtGvItem = (TextBox)gr.FindControl("txtGvItem");
            TextBox txtGvAcccount = (TextBox)gr.FindControl("txtGvAcccount");

            gr.Attributes["onclick"] = "VisibleRow('" + gr.ClientID + "','" + txtGvItem.ClientID + "','" + gvGLItems.ClientID + "',event);";
            gr.Attributes["onclick"] = "VisibleRow('" + gr.ClientID + "','" + txtGvAcccount.ClientID + "','" + gvGLItems.ClientID + "',event);";
        }

        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "SelectedRowStyle('" + gvJournal.ClientID + "');", true);
    }

    private void CompanyPermission()
    {
        var companyColumn = gvGLItems.MasterTableView.GetColumn("Company");       
        if (Session["COPer"].ToString() == "1")
        {
            companyColumn.Visible = true;
          //  gvGLItems.HeaderRow.Cells[5].Visible = true;


        }
        else
        {
            companyColumn.Visible = false;
          //  gvGLItems.HeaderRow.Cells[5].Visible = false;
        }
    }


    #region ::Events::
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int successcount = 0;
        string err = ValidateRows();
        bool _chkInvTracking = chkInvTracking();
        if (validatePeriodClose() == true)
        {
            if (string.IsNullOrEmpty(err))
            {
                try
                {

                    foreach (GridDataItem gr in gvGLItems.Items)
                    {
                        BL_Inventory blinventory = new BL_Inventory();

                        HiddenField hdnInvID = (HiddenField)gr.FindControl("hdnInvID");
                        HiddenField hdnWarehouse = (HiddenField)gr.FindControl("hdnWarehouse");
                        HiddenField hdnWarehouseLocationID = (HiddenField)gr.FindControl("hdnWarehouseLocationID");
                        HiddenField hdnCompanyEN = (HiddenField)gr.FindControl("hdnCompanyEN");

                        HiddenField hdnAccount = (HiddenField)gr.FindControl("hdnAccount");
                        TextBox txtGvItem = (TextBox)gr.FindControl("txtGvItem");
                        TextBox txtNewOnHand = (TextBox)gr.FindControl("txtNewOnHand");
                        TextBox txtNewValue = (TextBox)gr.FindControl("txtNewValue");
                        TextBox txtAdj = (TextBox)gr.FindControl("txtAdj");
                        TextBox txtAdjAmount = (TextBox)gr.FindControl("txtAdjAmount");
                        TextBox txtAdjDate = (TextBox)gr.FindControl("txtAdjDate");


                        if (hdnInvID.Value != "0")
                        {
                            InventoryAdjustment invadj = new InventoryAdjustment();
                            invadj.ConnConfig = WebBaseUtility.ConnectionString;

                            #region Default Acct Get
                            if (Session["CmpChkDefault"].ToString() == "1")
                            {
                                invadj.EN = 1;
                                invadj.CompanyID = Convert.ToInt32(hdnCompanyEN.Value);
                            }
                            else
                            {
                                invadj.EN = 0;
                            }

                            #endregion

                            BusinessEntity.Inventory inv = new BusinessEntity.Inventory();
                            inv.ID = Convert.ToInt32(hdnInvID.Value);
                            inv.Hand = Convert.ToDecimal(txtNewOnHand.Text);
                            inv.Balance = Convert.ToDecimal(txtNewValue.Text);


                            IWarehouseLocAdj invWarehouseLoc = new IWarehouseLocAdj();
                            invWarehouseLoc.InvID = Convert.ToInt32(hdnInvID.Value);
                            invWarehouseLoc.WarehouseID = hdnWarehouse.Value;
                            invWarehouseLoc.locationID = Convert.ToInt32(hdnWarehouseLocationID.Value);
                            invWarehouseLoc.Hand = Convert.ToDecimal(txtNewOnHand.Text == "" ? "0" : txtNewOnHand.Text);
                            invWarehouseLoc.Balance = Convert.ToDecimal(txtNewValue.Text == "" ? "0" : txtNewValue.Text);


                            Chart account = new Chart();
                            account.ID = Convert.ToInt32(hdnAccount.Value);

                            Transaction trans = new Transaction();
                            trans.ID = 0;
                            trans.BatchID = 0;
                            trans.TimeStamp = System.Text.Encoding.UTF8.GetBytes(txtAdjDate.Text);

                            invadj.IWarehouseLocAdj = invWarehouseLoc;
                            invadj.Inv = inv;
                            invadj.Acct = account;
                            invadj.Trans = trans;
                            invadj.Quantity = Convert.ToDecimal(txtAdj.Text == "" ? "0" : txtAdj.Text);
                            invadj.Amount = Convert.ToDecimal(txtAdjAmount.Text == "" ? "0" : txtAdjAmount.Text);
                            invadj.fDate = Convert.ToDateTime(txtAdjDate.Text);
                            invadj.fDesc = "Inventory Adjustment";

                            //API
                            CreateInventoryAdjustmentsParam _createInventoryAdjustments = new CreateInventoryAdjustmentsParam();
                            _createInventoryAdjustments.ConnConfig = WebBaseUtility.ConnectionString;

                            #region Default Acct Get
                            if (Session["CmpChkDefault"].ToString() == "1")
                            {
                                _createInventoryAdjustments.EN = 1;
                                _createInventoryAdjustments.CompanyID = Convert.ToInt32(hdnCompanyEN.Value);
                            }
                            else
                            {
                                _createInventoryAdjustments.EN = 0;
                            }

                            #endregion

                            //BusinessEntity.Inventory inv = new BusinessEntity.Inventory();
                            _createInventoryAdjustments.Inv_ID = Convert.ToInt32(hdnInvID.Value);
                            _createInventoryAdjustments.Inv_Hand = Convert.ToDecimal(txtNewOnHand.Text);
                            _createInventoryAdjustments.Inv_Balance = Convert.ToDecimal(txtNewValue.Text);


                            //IWarehouseLocAdj invWarehouseLoc = new IWarehouseLocAdj();
                            _createInventoryAdjustments.IWarehouseLocAdj_InvID = Convert.ToInt32(hdnInvID.Value);
                            _createInventoryAdjustments.IWarehouseLocAdj_WarehouseID = hdnWarehouse.Value;
                            _createInventoryAdjustments.IWarehouseLocAdj_locationID = Convert.ToInt32(hdnWarehouseLocationID.Value);
                            _createInventoryAdjustments.IWarehouseLocAdj_Hand = Convert.ToDecimal(txtNewOnHand.Text == "" ? "0" : txtNewOnHand.Text);
                            _createInventoryAdjustments.IWarehouseLocAdj_Balance = Convert.ToDecimal(txtNewValue.Text == "" ? "0" : txtNewValue.Text);


                            //Chart account = new Chart();
                            _createInventoryAdjustments.Acct_ID = Convert.ToInt32(hdnAccount.Value);

                            //Transaction trans = new Transaction();
                            _createInventoryAdjustments.Trans_ID = 0;
                            _createInventoryAdjustments.Trans_BatchID = 0;
                            _createInventoryAdjustments.Trans_TimeStamp = System.Text.Encoding.UTF8.GetBytes(txtAdjDate.Text);

                            //_createInventoryAdjustments.IWarehouseLocAdj = _createInventoryAdjustments.IWarehouseLocAdj;
                            //_createInventoryAdjustments.Inv = _createInventoryAdjustments.Inv;
                            //_createInventoryAdjustments.Acct = _createInventoryAdjustments.Acct;
                            //_createInventoryAdjustments.Trans = _createInventoryAdjustments.Trans;
                            _createInventoryAdjustments.Quantity = Convert.ToDecimal(txtAdj.Text == "" ? "0" : txtAdj.Text);
                            _createInventoryAdjustments.Amount = Convert.ToDecimal(txtAdjAmount.Text == "" ? "0" : txtAdjAmount.Text);
                            _createInventoryAdjustments.fDate = Convert.ToDateTime(txtAdjDate.Text);
                            _createInventoryAdjustments.fDesc = "Inventory Adjustment";


                            if (IsAPIIntegrationEnable == "YES")
                            {
                                string APINAME = "ItemAdjustmentAPI/AddInventoryAdjustments_CreateInventoryAdjustments";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _createInventoryAdjustments);

                                int returnVal = Convert.ToInt32(_APIResponse.ResponseData);

                                if (returnVal > 0)
                                {
                                    successcount++;
                                }
                            }
                            else
                            {
                                if (blinventory.CreateInventoryAdjustments(invadj, _chkInvTracking) > 0)
                                    successcount++;
                            }

                        }

                    }
                }
                catch (Exception ex)
                {
                    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelProspect", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                    return;
                }

                if (successcount == 0)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Please select the default Inventory Acct.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
                else
                {
                    Session["InventoryAdj_SuccessCount"] = successcount;
                    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyDel", "noty({text: ' Adjustments Added Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    Response.Redirect("InventoryAdjustments.aspx", false);
                }
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelProspect", "noty({text: 'Errors in rows " + err + ".item,account,date is required.',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelProspect", "noty({text: 'Item, Warehouse, Account, Date is required.',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelProspect", "noty({text: 'These month/year period is closed out. You do not have permission to add/update this record.',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
       
    }    

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("InventoryAdjustments.aspx");
    }

    protected void gvGLItems_PreRender(object sender, EventArgs e)
    {
       foreach(GridDataItem row in gvGLItems.Items)
        {
            var txtAdj = (TextBox)row.FindControl("txtAdj");
            txtAdj.Attributes.Add("readonly", "readonly");

            var txtAdjAmount = (TextBox)row.FindControl("txtAdjAmount");
            txtAdjAmount.Attributes.Add("readonly", "readonly");
        }
    }

    protected void gvGLItems_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //if (gvGLItems.Rows.Count > 0)
            //{
            //    gvGLItems.HeaderRow.TableSection = TableRowSection.TableHeader;
            //}
            int rowIndex = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "DeleteTransaction")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridDataItem row = gvGLItems.Items[index];

                HiddenField hdnAcctID = (HiddenField)row.FindControl("hdnAcctID");
                TextBox txtGvAcctNo = (TextBox)row.FindControl("txtGvAcctNo");
                TextBox txtGvDesc = (TextBox)row.FindControl("txtGvDesc");
                TextBox txtGvAmount = (TextBox)row.FindControl("txtGvAmount");
                TextBox txtGvQuan = (TextBox)row.FindControl("txtGvQuan");
                TextBox txtGvPrice = (TextBox)row.FindControl("txtGvPrice");
                TextBox txtGvLoc = (TextBox)row.FindControl("txtGvLoc");
                TextBox txtGvJob = (TextBox)row.FindControl("txtGvJob");
                HiddenField hdnJobID = (HiddenField)row.FindControl("hdnJobID");
                TextBox txtGvPhase = (TextBox)row.FindControl("txtGvPhase");
                HiddenField hdnPID = (HiddenField)row.FindControl("hdnPID");
                TextBox txtGvDue = (TextBox)row.FindControl("txtGvDue");
                HiddenField hdnTypeId = (HiddenField)row.FindControl("hdnTypeId");
                TextBox txtGvItem = (TextBox)row.FindControl("txtGvItem");

                hdnAcctID.Value = "0";
                txtGvAcctNo.Text = "";
                txtGvDesc.Text = "";
                txtGvPrice.Text = "";
                txtGvQuan.Text = "";
                txtGvAmount.Text = "";
                txtGvLoc.Text = "";
                txtGvJob.Text = "";
                txtGvPhase.Text = "";
                hdnPID.Value = "0";
                hdnJobID.Value = "0";
                txtGvDue.Text = "";
                hdnTypeId.Value = "0";
                txtGvItem.Text = "";

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "CalculateTotalAmt", "CalculateTotalAmt();", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void btnAddNewLines_Click(object sender, EventArgs e)
    {
        try
        {
            int rowIndex = gvGLItems.Items.Count - 1;

            DataTable dt = GetPOGridItems();
            DataRow dr = null;
            for (int i = 0; i < 4; i++)
            {
                dr = dt.NewRow();

                dr["RowID"] = i + 1;
                dr["AdjID"] = 0;
                dr["InvId"] = 0;
                dr["TransID"] = 0;
                dr["Batch"] = string.Empty;
                dr["Acct"] = string.Empty;
                dr["Fdate"] = string.Empty;
                dr["Fdesc"] = string.Empty;
                dr["ItemName"] = string.Empty;
                dr["Quantity"] = string.Empty;
                dr["Amount"] = string.Empty;
                dr["ChartID"] = 0;
                dr["WarehouseID"] = "";
                dr["LocationID"] = 0;
                dr["Chart"] = string.Empty;
                dr["Hand"] = 0;
                dr["WarehouseName"] = string.Empty;
                dr["LocationName"] = string.Empty;
                dr["Company"] = string.Empty;
                dr["EN"] = 0;


                //dr["RowID"] = i + 1;
                //dr["Line"] = i + 1;
                //dr["ID"] = 0;
                //dr["AcctID"] = 0;
                //dr["AcctNo"] = string.Empty;
                //dr["fDesc"] = string.Empty;
                //dr["Loc"] = string.Empty;
                //dr["JobName"] = string.Empty;
                //dr["JobID"] = 0;
                //dr["Phase"] = string.Empty;
                //dr["PhaseID"] = 0;
                //dr["TypeID"] = DBNull.Value;
                //dr["Quan"] = string.Empty;
                //dr["Price"] = string.Empty;
                //dr["Amount"] = string.Empty;
                //dr["Freight"] = 0.00;
                //dr["Rquan"] = 0.00;
                //dr["Due"] = DBNull.Value;


                dt.Rows.Add(dr);
                rowIndex++;
            }

            gvGLItems.DataSource = dt;
            gvGLItems.DataBind();
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "addAutocomplete", 
                "$(function () { addAutocomplete(); });", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }    

    protected void gvGLItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].Visible = false; // Invisibiling Year Header Cell
            e.Row.Cells[1].Visible = false; // Invisibiling Year Header Cell
            e.Row.Cells[2].Visible = false; // Invisibiling Year Header Cell
            e.Row.Cells[3].Visible = false; // Invisibiling Year Header Cell
            e.Row.Cells[4].Visible = false; // Invisibiling Year Header Cell
            e.Row.Cells[5].Visible = false; // Invisibiling Year Header Cell
            e.Row.Cells[14].Visible = false; // Invisibiling Year Header Cell

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtAdj = (TextBox)e.Row.FindControl("txtAdj");
            txtAdj.Attributes.Add("readonly", "readonly");

            TextBox txtAdjAmount = (TextBox)e.Row.FindControl("txtAdjAmount");
            txtAdjAmount.Attributes.Add("readonly", "readonly");
            
        }

    }
    #endregion

    #region ::Methods::
    private DataTable GetPOGridItems()
    {
        DataTable dt = new DataTable();


        dt.Columns.Add(new DataColumn("RowID", typeof(string)));
        dt.Columns.Add(new DataColumn("AdjID", typeof(Int32)));        // PO
        dt.Columns.Add(new DataColumn("InvId", typeof(Int32)));
        dt.Columns.Add(new DataColumn("TransID", typeof(Int32)));    // GL
        dt.Columns.Add(new DataColumn("Batch", typeof(string)));
        dt.Columns.Add(new DataColumn("Acct", typeof(string)));  // fDesc
        dt.Columns.Add(new DataColumn("Fdate", typeof(string)));
        dt.Columns.Add(new DataColumn("Fdesc", typeof(string)));
        dt.Columns.Add(new DataColumn("ItemName", typeof(string)));
        dt.Columns.Add(new DataColumn("Quantity", typeof(string)));
        dt.Columns.Add(new DataColumn("Amount", typeof(string)));
        dt.Columns.Add(new DataColumn("ChartID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("WarehouseID", typeof(string)));
        dt.Columns.Add(new DataColumn("LocationID", typeof(int)));
        dt.Columns.Add(new DataColumn("Chart", typeof(string)));
        dt.Columns.Add(new DataColumn("Hand", typeof(int)));
        dt.Columns.Add(new DataColumn("WarehouseName", typeof(string)));
        dt.Columns.Add(new DataColumn("LocationName", typeof(string)));
        dt.Columns.Add(new DataColumn("Company", typeof(string)));
        dt.Columns.Add(new DataColumn("EN", typeof(Int32)));


        //dt.Columns.Add(new DataColumn("RowID", typeof(string)));
        //dt.Columns.Add(new DataColumn("ID", typeof(Int32)));        // PO
        //dt.Columns.Add(new DataColumn("Line", typeof(Int16)));
        //dt.Columns.Add(new DataColumn("AcctID", typeof(Int32)));    // GL
        //dt.Columns.Add(new DataColumn("AcctNo", typeof(string)));
        //dt.Columns.Add(new DataColumn("fDesc", typeof(string)));  // fDesc
        //dt.Columns.Add(new DataColumn("Quan", typeof(string)));
        //dt.Columns.Add(new DataColumn("Price", typeof(string)));
        //dt.Columns.Add(new DataColumn("Amount", typeof(string)));
        //dt.Columns.Add(new DataColumn("Loc", typeof(string)));
        //dt.Columns.Add(new DataColumn("JobName", typeof(string)));
        //dt.Columns.Add(new DataColumn("JobID", typeof(string)));    // Job
        //dt.Columns.Add(new DataColumn("Phase", typeof(string)));
        //dt.Columns.Add(new DataColumn("PhaseID", typeof(Int32)));   // Phase
        //dt.Columns.Add(new DataColumn("Inv", typeof(Int32)));       // Inv
        //dt.Columns.Add(new DataColumn("Freight", typeof(double)));  // Freight
        //dt.Columns.Add(new DataColumn("Rquan", typeof(double)));    // Rquan
        //dt.Columns.Add(new DataColumn("Billed", typeof(Int32)));    // Billed
        //dt.Columns.Add(new DataColumn("Ticket", typeof(Int32)));    // Ticket
        //dt.Columns.Add(new DataColumn("Due", typeof(DateTime)));      //due date
        //dt.Columns.Add(new DataColumn("TypeID", typeof(Int32)));
        //dt.Columns.Add(new DataColumn("ItemDesc", typeof(string)));

        try
        {
            string strItems = hdnItemJSON.Value.Trim();
            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    if (dict["hdnIndex"].ToString().Trim() == string.Empty)
                    {
                        return dt;
                    }
                    i++;
                    DataRow dr = dt.NewRow();
                    if (!(dict["hdnAdjID"].ToString().Trim() == string.Empty))
                    {
                        dr["AdjID"] = Convert.ToInt32(dict["hdnAdjID"].ToString());
                    }
                    if (!(dict["hdnInvID"].ToString().Trim() == string.Empty))
                    {
                        dr["InvId"] = Convert.ToInt32(dict["hdnInvID"].ToString());
                    }
                    if (!(dict["hdnTransID"].ToString().Trim() == string.Empty))
                    {
                        dr["TransID"] = Convert.ToInt32(dict["hdnTransID"].ToString());
                    }

                    dr["Batch"] = 0;

                    if (!(dict["hdnAccount"].ToString().Trim() == string.Empty))
                    {
                        dr["Acct"] = dict["hdnAccount"].ToString();
                    }
                    if (!(dict["txtAdjDate"].ToString().Trim() == string.Empty))
                    {
                        dr["Fdate"] = dict["txtAdjDate"].ToString();
                    }

                    dr["Fdesc"] = "Inventory Adjustment";


                    dr["ItemName"] = "";

                    if (!(dict["txtAdj"].ToString().Trim() == string.Empty))
                    {
                        dr["Quantity"] = Convert.ToInt32(dict["txtAdj"].ToString());
                    }
                    if (!(dict["txtAdjAmount"].ToString().Trim() == string.Empty))
                    {
                        dr["Amount"] = Convert.ToInt32(dict["txtAdjAmount"].ToString());
                    }

                    dr["ChartID"] = 0;

                    dr["WarehouseID"] = "";

                    dr["LocationID"] = 0;

                    dr["Chart"] = "";
                    dr["Hand"] = 0;

                    dr["WarehouseName"] = "";
                    dr["LocationName"] = "";
                    dr["Company"] = "";
                    dr["EN"] = 0;
                    //if (!(dict["hdnTID"].ToString().Trim() == string.Empty))
                    //{
                    //    dr["ID"] = Convert.ToInt32(dict["hdnTID"].ToString());
                    //}
                    //if (!(dict["hdnLine"].ToString().Trim() == string.Empty))
                    //{
                    //    dr["Line"] = Convert.ToInt16(dict["hdnLine"].ToString());
                    //}
                    //if (!(dict["hdnAcctID"].ToString().Trim() == string.Empty))
                    //{
                    //    dr["AcctID"] = Convert.ToInt32(dict["hdnAcctID"].ToString().Trim());
                    //}
                    //dr["AcctNo"] = dict["txtGvAcctNo"].ToString().Trim();
                    //dr["fDesc"] = dict["txtGvDesc"].ToString().Trim();
                    //dr["Quan"] = dict["txtGvQuan"].ToString();
                    //dr["Price"] = dict["txtGvPrice"].ToString().Trim();
                    //dr["Amount"] = dict["txtGvAmount"].ToString().Trim();
                    //if (!(dict["txtGvLoc"].ToString().Trim() == string.Empty))
                    //{
                    //    dr["Loc"] = dict["txtGvLoc"].ToString().Trim();
                    //}
                    //if (!(dict["txtGvJob"].ToString().Trim() == string.Empty))
                    //{
                    //    dr["JobName"] = dict["txtGvJob"].ToString().Trim();
                    //}
                    //if (!(dict["hdnJobID"].ToString().Trim() == string.Empty))
                    //{
                    //    dr["JobID"] = Convert.ToInt32(dict["hdnJobID"]);
                    //}
                    //if (!(dict["txtGvPhase"].ToString().Trim() == string.Empty))
                    //{
                    //    dr["Phase"] = dict["txtGvPhase"].ToString().Trim();
                    //}
                    //if (!(dict["hdnPID"].ToString().Trim() == string.Empty))
                    //{
                    //    dr["PhaseID"] = Convert.ToInt32(dict["hdnPID"].ToString());
                    //}
                    //if (!(dict["hdnTypeId"].ToString().Trim() == string.Empty))
                    //{
                    //    dr["TypeID"] = Convert.ToInt32(dict["hdnTypeId"].ToString().Trim());
                    //}
                    //dr["Freight"] = 0.00;
                    //if (!(dict["txtGvDue"].ToString().Trim() == string.Empty))
                    //{
                    //    dr["Due"] = Convert.ToDateTime(dict["txtGvDue"].ToString());
                    //}
                    //else
                    //{
                    //    dr["Due"] = DBNull.Value;
                    //}
                    //if (!(dict["hdnItemID"].ToString().Trim() == string.Empty))
                    //{
                    //    dr["Inv"] = Convert.ToInt32(dict["hdnItemID"]);
                    //}
                    //if (!(dict["txtGvItem"].ToString().Trim() == string.Empty))
                    //{
                    //    dr["ItemDesc"] = dict["txtGvItem"].ToString();
                    //}

                    dt.Rows.Add(dr);
                    i++;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }
    private DataTable GetPOItem()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add(new DataColumn("RowID", typeof(string)));
        dt.Columns.Add(new DataColumn("ID", typeof(Int32)));        // PO
        dt.Columns.Add(new DataColumn("Line", typeof(Int16)));
        dt.Columns.Add(new DataColumn("AcctID", typeof(Int32)));    // GL
        dt.Columns.Add(new DataColumn("AcctNo", typeof(string)));
        dt.Columns.Add(new DataColumn("fDesc", typeof(string)));  // fDesc
        dt.Columns.Add(new DataColumn("Quan", typeof(string)));
        dt.Columns.Add(new DataColumn("Price", typeof(string)));
        dt.Columns.Add(new DataColumn("Amount", typeof(string)));
        dt.Columns.Add(new DataColumn("Loc", typeof(string)));
        dt.Columns.Add(new DataColumn("JobName", typeof(string)));
        dt.Columns.Add(new DataColumn("JobID", typeof(string)));    // Job
        dt.Columns.Add(new DataColumn("Phase", typeof(string)));
        dt.Columns.Add(new DataColumn("PhaseID", typeof(Int32)));   // Phase
        dt.Columns.Add(new DataColumn("Inv", typeof(Int32)));       // Inv
        dt.Columns.Add(new DataColumn("Freight", typeof(double)));  // Freight
        dt.Columns.Add(new DataColumn("Rquan", typeof(double)));    // Rquan
        dt.Columns.Add(new DataColumn("Billed", typeof(Int32)));    // Billed
        dt.Columns.Add(new DataColumn("Ticket", typeof(Int32)));    // Ticket
        dt.Columns.Add(new DataColumn("Due", typeof(DateTime)));      //due date
        dt.Columns.Add(new DataColumn("TypeID", typeof(Int32)));
        dt.Columns.Add(new DataColumn("ItemDesc", typeof(string)));

        try
        {
            string strItems = hdnItemJSON.Value.Trim();
            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    if (dict["txtGvAcctNo"].ToString().Trim() == string.Empty)
                    {
                        //return dt;
                        continue;
                    }
                    i++;
                    DataRow dr = dt.NewRow();
                    if (!(dict["hdnTID"].ToString().Trim() == string.Empty))
                    {
                        dr["ID"] = Convert.ToInt32(dict["hdnTID"].ToString());
                    }
                    if (!(dict["hdnLine"].ToString().Trim() == string.Empty))
                    {
                        dr["Line"] = Convert.ToInt16(dict["hdnLine"].ToString());
                    }
                    dr["AcctID"] = Convert.ToInt32(dict["hdnAcctID"].ToString().Trim());
                    dr["AcctNo"] = dict["txtGvAcctNo"].ToString().Trim();
                    dr["fDesc"] = dict["txtGvDesc"].ToString().Trim();
                    dr["Quan"] = dict["txtGvQuan"].ToString();
                    dr["Price"] = dict["txtGvPrice"].ToString().Trim();
                    dr["Amount"] = dict["txtGvAmount"].ToString().Trim();
                    if (!(dict["txtGvLoc"].ToString().Trim() == string.Empty))
                    {
                        dr["Loc"] = dict["txtGvLoc"].ToString().Trim();
                    }
                    if (!(dict["txtGvJob"].ToString().Trim() == string.Empty))
                    {
                        dr["JobName"] = dict["txtGvJob"].ToString().Trim();
                    }
                    if (!(dict["hdnJobID"].ToString().Trim() == string.Empty))
                    {
                        dr["JobID"] = Convert.ToInt32(dict["hdnJobID"]);
                    }
                    if (!(dict["txtGvPhase"].ToString().Trim() == string.Empty))
                    {
                        dr["Phase"] = dict["txtGvPhase"].ToString().Trim();
                    }
                    if (!(dict["hdnPID"].ToString().Trim() == string.Empty))
                    {
                        dr["PhaseID"] = Convert.ToInt32(dict["hdnPID"].ToString());
                    }
                    if (!(dict["hdnTypeId"].ToString().Trim() == string.Empty))
                    {
                        dr["TypeID"] = Convert.ToInt32(dict["hdnTypeId"].ToString().Trim());
                    }
                    dr["Freight"] = 0.00;
                    if (!(dict["txtGvDue"].ToString().Trim() == string.Empty))
                    {
                        dr["Due"] = Convert.ToDateTime(dict["txtGvDue"].ToString());
                    }
                    else
                    {
                        dr["Due"] = DBNull.Value;
                    }
                    if (!(dict["hdnItemID"].ToString().Trim() == string.Empty))
                    {
                        dr["Inv"] = Convert.ToInt32(dict["hdnItemID"]);
                    }
                    if (!(dict["txtGvItem"].ToString().Trim() == string.Empty))
                    {
                        dr["ItemDesc"] = dict["txtGvItem"].ToString();
                    }

                    dt.Rows.Add(dr);
                    i++;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }

    private void SetInitialRow()    //Initialization of Datatable.
    {
        try
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("RowID", typeof(string)));
            dt.Columns.Add(new DataColumn("AdjID", typeof(Int32)));        // PO
            dt.Columns.Add(new DataColumn("InvId", typeof(Int32)));
            dt.Columns.Add(new DataColumn("TransID", typeof(Int32)));    // GL
            dt.Columns.Add(new DataColumn("Batch", typeof(string)));
            dt.Columns.Add(new DataColumn("Acct", typeof(string)));  // fDesc
            dt.Columns.Add(new DataColumn("Fdate", typeof(string)));
            dt.Columns.Add(new DataColumn("Fdesc", typeof(string)));
            dt.Columns.Add(new DataColumn("ItemName", typeof(string)));
            dt.Columns.Add(new DataColumn("Quantity", typeof(string)));
            dt.Columns.Add(new DataColumn("Amount", typeof(string)));
            dt.Columns.Add(new DataColumn("ChartID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("WarehouseID", typeof(String)));
            dt.Columns.Add(new DataColumn("LocationID", typeof(int)));
            dt.Columns.Add(new DataColumn("Chart", typeof(String)));
            dt.Columns.Add(new DataColumn("Hand", typeof(int)));
            dt.Columns.Add(new DataColumn("WarehouseName", typeof(String)));
            dt.Columns.Add(new DataColumn("LocationName", typeof(String)));
            dt.Columns.Add(new DataColumn("Company", typeof(String)));
            dt.Columns.Add(new DataColumn("EN", typeof(int)));

            int rowIndex = 0;
            for (int i = 0; i < 4; i++)
            {
                dr = dt.NewRow();
                dr["RowID"] = i + 1;
                dr["AdjID"] = 0;
                dr["InvId"] = 0;
                dr["TransID"] = 0;
                dr["Batch"] = string.Empty;
                dr["Acct"] = string.Empty;
                dr["Fdate"] = string.Empty;
                dr["Fdesc"] = string.Empty;
                dr["ItemName"] = string.Empty;
                dr["Quantity"] = string.Empty;
                dr["Amount"] = string.Empty;
                dr["ChartID"] = 0;
                dr["WarehouseID"] = string.Empty;
                dr["LocationID"] = 0;
                dr["Chart"] = string.Empty;
                dr["Hand"] = 0;
                dr["WarehouseName"] = string.Empty;
                dr["LocationName"] = string.Empty;
                dr["Company"] = string.Empty;
                dr["EN"] = 0;

                dt.Rows.Add(dr);
                rowIndex++;





            }

            ViewState["Transactions"] = dt;

            gvGLItems.DataSource = dt;
            gvGLItems.DataBind();

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private string ValidateRows()
    {
        bool isvalid = true;
        string ids = string.Empty;

        foreach (GridDataItem gr in gvGLItems.Items)
        {
            HiddenField hdnInvID = (HiddenField)gr.FindControl("hdnInvID");
            HiddenField hdnAccount = (HiddenField)gr.FindControl("hdnAccount");
            HiddenField hdnIndex = (HiddenField)gr.FindControl("hdnIndex");
            TextBox txtGvItem = (TextBox)gr.FindControl("txtGvItem");
            TextBox txtNewOnHand = (TextBox)gr.FindControl("txtNewOnHand");
            TextBox txtNewValue = (TextBox)gr.FindControl("txtNewValue");
            TextBox txtAdj = (TextBox)gr.FindControl("txtAdj");
            TextBox txtAdjAmount = (TextBox)gr.FindControl("txtAdjAmount");
            TextBox txtAdjDate = (TextBox)gr.FindControl("txtAdjDate");
            HiddenField hdnWarehouse = (HiddenField)gr.FindControl("hdnWarehouse");
            

            if (hdnInvID.Value != "0")
            {



                if (string.IsNullOrEmpty(hdnAccount.Value) || hdnAccount.Value == "0")
                {
                    isvalid = false;
                }
               
                if (string.IsNullOrEmpty(txtAdjDate.Text)  )
                {
                    isvalid = false;
                }
                else
                {
                    if (CommonHelper.GetPeriodDetails(Convert.ToDateTime( txtAdjDate.Text)) == false)
                    {
                        isvalid = false;
                    }

                }
                if (string.IsNullOrEmpty(hdnWarehouse.Value))
                {
                    isvalid = false;
                }


                if (!isvalid)
                {
                    ids += hdnIndex.Value + ",";

                }
            }


        }


        return ids;
    }
    private bool validatePeriodClose()
    {
        bool isvalid = true;
   
        foreach (GridDataItem gr in gvGLItems.Items)
        {
            HiddenField hdnInvID = (HiddenField)gr.FindControl("hdnInvID");
            TextBox txtAdjDate = (TextBox)gr.FindControl("txtAdjDate");
            if (hdnInvID.Value != "0")
            {
                if (string.IsNullOrEmpty(txtAdjDate.Text))
                {
                    isvalid = false;
                }
                else
                {
                    if (CommonHelper.GetPeriodDetails(Convert.ToDateTime(txtAdjDate.Text)) == false)
                    {
                        isvalid = false;
                    }

                }

            }
        }

        return isvalid;
    }
    #endregion

    #region ::WebMethods::
    
    #endregion


    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["InventoryAdjustments"];
            Response.Redirect("AddInventoryAdjustment.aspx?id=" + dt.Rows[0]["ID"]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["InventoryAdjustments"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);

            if (index > 0)
            {
                Response.Redirect("AddInventoryAdjustment.aspx?id=" + dt.Rows[index - 1]["ID"]);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["InventoryAdjustments"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;

            if (index < c)
            {
                Response.Redirect("AddInventoryAdjustment.aspx?id=" + dt.Rows[index + 1]["ID"]);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkLast_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["InventoryAdjustments"];
            Response.Redirect("AddInventoryAdjustment.aspx?id=" + dt.Rows[dt.Rows.Count - 1]["ID"]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void GetPeriodDetails()
    {
        bool Flag = CommonHelper.GetPeriodDetails(DateTime.Now);
        ViewState["FlagPeriodClose"] = Flag;
        if (!Flag)
        {
            divMessage.Visible = true;
            btnSubmit.Visible = false;
            gvGLItems.Enabled = false;
        }

    }
}