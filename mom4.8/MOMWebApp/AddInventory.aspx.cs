using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.InventoryModel;
using BusinessEntity.Utility;
using BusinessLayer;
using MOMWebApp;
using Newtonsoft.Json;
using Telerik.Web.UI;

public partial class AddInventory : System.Web.UI.Page
{


    #region ::Declarartion::
    BL_Inventory objBL_Inventory = new BL_Inventory();
    User objProp_User = new User();
    BusinessEntity.Inventory objProp_Inventory = new BusinessEntity.Inventory();
    Itype objProp_Intype = new BusinessEntity.Itype();
    Chart objProp_Chart = new BusinessEntity.Chart();


    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    //API Variables 
    UpdateInventoryParam _UpdateInventory = new UpdateInventoryParam();
    CreateInvMergeWarehouseParam _CreateInvMergeWarehouse = new CreateInvMergeWarehouseParam();
    CreateInventoryPartsParam _CreateInventoryParts = new CreateInventoryPartsParam();
    DeleteInventoryPartsParam _DeleteInventoryParts = new DeleteInventoryPartsParam();
    AddFileParam _AddFile = new AddFileParam();
    UpdateDocInfoParam _UpdateDocInfo = new UpdateDocInfoParam();
    GetDocumentsParam _GetDocuments = new GetDocumentsParam();
    DeleteFileParam _DeleteFile = new DeleteFileParam();
    GetChartByTypeParam _GetChartByType = new GetChartByTypeParam();
    GetInventoryActiveWarehouseParam _GetInventoryActiveWarehouse = new GetInventoryActiveWarehouseParam();
    CheckWarehouseIsActiveParam _CheckWarehouseIsActive = new CheckWarehouseIsActiveParam();
    GetItemPurchaseOrderByInvIDParam _GetItemPurchaseOrderByInvID = new GetItemPurchaseOrderByInvIDParam();
    GetAllItemQuantityByInvIDParam _GetAllItemQuantityByInvID = new GetAllItemQuantityByInvIDParam();
    CreateItemRevisionParam _CreateItemRevision = new CreateItemRevisionParam();
    DeleteInventoryWareHouseParam _DeleteInventoryWareHouse = new DeleteInventoryWareHouseParam();
    GetInventoryTransactionByInvIDParam _GetInventoryTransactionByInvID = new GetInventoryTransactionByInvIDParam();
    GetALLInventoryParam _GetALLInventory = new GetALLInventoryParam();
    ReadAllCommodityParam _ReadAllCommodity = new ReadAllCommodityParam();
    chkStatusOfChartParam _StatusOfChart = new chkStatusOfChartParam();
    
    private enum chartType
    {
        Assets = 0,
        Liabilities = 1,
        Equity = 2,
        Revenue = 3,
        CostofSales = 4,
        Expenses = 5,
        CashinBank = 6,
        PurchaseOrders = 7
    }

    public string invID = string.Empty;
    #endregion

    #region ::Page Events::
    protected void Page_Load(object sender, EventArgs e)
    {


        invID = !string.IsNullOrEmpty(Request.QueryString["uid"]) ? Request.QueryString["uid"] : string.Empty;

        if (!IsPostBack)
        {
            if (Request.QueryString["TabWarehouse"] != null)
            {
                //TabContainer1.ActiveTabIndex = 1;
                

            }

            if (Session["config"] != null)
            {
                BindControls();
                ViewState["mode"] = 0;

                tbTransactions.Visible = false;
                if (!string.IsNullOrEmpty(invID))
                {

                    EnablePnlDoc();
                    WarehouseHide.Visible = true;
                    //  pnlDoc.Visible = true;
                    DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    int DaysinMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - 1;
                    DateTime lastDay = firstDay.AddDays(DaysinMonth);

                    txtInvDtFrom.Text = firstDay.ToShortDateString();
                    txtInvDtTo.Text = lastDay.ToShortDateString();
                    tbTransactions.Visible = true;
                    FillData(invID);
                    GetInvTransactions();
                    // CompanyPermission();
                    if (Request.QueryString["t"] != null)
                    {
                        ViewState["mode"] = 0;
                         //txtDateCreated.Text = DateTime.Today.ToString("MM/dd/yyyy");
                        txtDateCreated.Text = DateTime.Now.ToString();
                        
                    }
                    else
                    {
                        ViewState["mode"] = 1;
                        lblHeader.Text = "Edit Item";
                        
                        //btnSageID.Visible = false;
                        //if (intintegration == 1)
                        //    txtAcctno.Enabled = false;
                    }
                }
                else
                {
                    disablePnlDoc();
                }
            }
            /// Successfully alert 
            if (Request.QueryString["Mode"] != null)
            {
                if (Request.QueryString["Mode"] == "1")
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccEdit", "noty({text: 'Item Updated Successfully! </br> <b> Inventory P/N# : " + txtItemHeaderName.Text + "</b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
                else if (Request.QueryString["Mode"] == "0")
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Item Created Successfully! </br> <b> Inventory P/N# : " + txtItemHeaderName.Text + "</b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
        }

        Permission();

        HighlightSideMenu("InventoryMgr", "lnkItemMaster", "ulInventoryMgr"); 
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

    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            RadGrid_Warehouse.Controls[7].Visible = true;

        }
        else
        {
            RadGrid_Warehouse.Controls[7].Visible = false;

        }
    }
    private void disablePnlDoc()
    {
        lnkDeleteDoc.Enabled = false;
        FileUpload1.Enabled = false;
        lnkUploadDoc.Enabled = false;
        RadGrid_Documents.Enabled = false;
        pnlDocumentButtons.Attributes.Add("style", "background: #787878");

    }

    private void EnablePnlDoc()
    {
        lnkDeleteDoc.Enabled = true;
        FileUpload1.Enabled = true;
        lnkUploadDoc.Enabled = true;
        RadGrid_Documents.Enabled = true;

    }


    protected void Page_PreRender(Object o, EventArgs e)
    {
        //foreach (GridDataItem gr in RadGridTran.Items)
        //{
        //    Label lblID = (Label)gr.FindControl("lblId");
            
        //    Label lblType = (Label)gr.FindControl("lblType");

        //    if (lblType.Text.Equals("AP Invoice Line Items"))
        //    {
        //        gr.Attributes["ondblclick"] = "location.href='AddBills.aspx?id=" + lblID.Text + "&page=AddInventory&lid=" + Request.QueryString["uid"].ToString() + "'";
        //    }
        //    else if (lblType.Text.Equals("Inv Adjustment - Asset side"))
        //    {
        //        gr.Attributes["ondblclick"] = "location.href='AddInventoryAdjustment.aspx?id=" + lblID.Text + "&page=AddInventory&lid=" + Request.QueryString["uid"].ToString() + "'";
        //    }
        //    else if (lblType.Text.Equals("Inventory Items on Tickets"))
        //    {
        //        gr.Attributes["ondblclick"] = "location.href='addticket.aspx?id=" + lblID.Text + "&page=AddInventory&lid=" + Request.QueryString["uid"].ToString() + "'";
        //    }

        //    else
        //    {
        //        gr.Attributes["ondblclick"] = "location.href='addreceivepo.aspx?id=" + lblID.Text + "&page=AddInventory&lid=" + Request.QueryString["uid"].ToString() + "'";
        //    }
        //    //gr.Attributes["ondblclick"] = "location.href='CustomerLedger.aspx?id=" + lblID.Text + "&page=addcustomer&lid=" + Request.QueryString["uid"].ToString() + "'";
        //    gr.Attributes["onclick"] = "SelectRowChk('" + gr.ClientID + "','" + RadGridTran.ClientID + "',event);";
        //}
    }






    #endregion

    #region ::Events::
    protected void btneditVendorInfo_Click(object sender, EventArgs e)
    {
        clear();
        string itemname = txtItemHeaderName.Text;
        string itemdecs = txtDes.Text;

        // foreach (DataListItem dtitemm in dtlVendors.Items)
        foreach (GridDataItem dtitemm in RadGrid_Vendors.SelectedItems)
        {
            InvParts item = new BusinessEntity.InvParts();


            //CheckBox chkinvi = dtitemm.FindControl("chkvenitem") as CheckBox;
            HiddenField id = dtitemm.FindControl("hdnid") as HiddenField;
            Label lblmpn = dtitemm.FindControl("lblmpn") as Label;
            Label lblVPN = dtitemm.FindControl("lblVPN") as Label;

            Label lblVendor = dtitemm.FindControl("lblVendor") as Label;
            Label lblVendorID = dtitemm.FindControl("lblVendorID") as Label;
            Label lblVendorPrice = dtitemm.FindControl("lblVendorPrice") as Label;
            Label lblManufacturerName = dtitemm.FindControl("lblManufacturerName") as Label;
            Label lblMfgPrice = dtitemm.FindControl("lblMfgPrice") as Label;
            //if (chkinvi.Checked)
            //{
            txtInventoryMPN.Text = Convert.ToString(lblmpn.Text);
            txtVendorPartNumber.Text = Convert.ToString(lblVPN.Text);
            txtVendorPrice.Text = Convert.ToString(lblVendorPrice.Text);
            ddlInventoryApprovedVendor.SelectedValue = Convert.ToString(lblVendorID.Text);
            txtInventoryApprovedManufacturer.Text = Convert.ToString(lblManufacturerName.Text);
            txtManufacturerPrice.Text = Convert.ToString(lblMfgPrice.Text);
            hdninvvendinfo.Value = id.Value;
            //}


        }
        string script = "function f(){$find(\"" + RadWindowVendor.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }
   

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {   
                bool chkinv = false;

                string strPartNo; 
                string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "InventoryAPI/AddInventory_GetPartNumber";

                    GetPartNumberParam _PartNumber = new GetPartNumberParam();
                    _PartNumber.strInventory = txtItemHeaderName.Text;

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _PartNumber);
                    strPartNo = _APIResponse.ResponseData;
                    object JsonData = JsonConvert.DeserializeObject(_APIResponse.ResponseData);
                    strPartNo = JsonData.ToString();
                }
                else
                {
                    strPartNo = objBL_Inventory.GetPartNumber(txtItemHeaderName.Text);
                }

                if (strPartNo == txtItemHeaderName.Text)
                    chkinv = true;
                else
                    chkinv = false;                
                string mode = string.Empty;
                if ((!string.IsNullOrEmpty(invID) && Request.QueryString["t"] == "c"))
                {

                    mode = "fromCopy";

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "InventoryAPI/AddInventory_GetPartNumber";

                        GetPartNumberParam _PartNumber = new GetPartNumberParam();
                        _PartNumber.strInventory = txtItemHeaderName.Text;

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _PartNumber);
                        strPartNo = _APIResponse.ResponseData;
                        object JsonData = JsonConvert.DeserializeObject(_APIResponse.ResponseData);
                        strPartNo = JsonData.ToString();
                    }
                    else
                    {
                        strPartNo = objBL_Inventory.GetPartNumber(txtItemHeaderName.Text);
                    }

                    chkinv = true;

                }
                if ((!string.IsNullOrEmpty(invID) && string.IsNullOrEmpty(Request.QueryString["t"])))
                    mode = "fromEdit";
                switch (mode)
                {
                    case "fromEdit":
                        chkinv = false;
                        break;
                    case "fromCopy":
                         if(strPartNo != txtItemHeaderName.Text)
                            chkinv = false;
                         else
                            chkinv = true;
                        break;
                }                
                if (!chkinv)
                {

                    //}
                    objProp_Inventory = new BusinessEntity.Inventory();
                    _UpdateInventory = new BusinessEntity.UpdateInventoryParam();
                    #region ItemHeader
                    objProp_Inventory.Measure = ddlUOM.SelectedValue;
                    objProp_Inventory.Status = Convert.ToInt32(ddlInvStatus.SelectedValue);

                    _UpdateInventory.Measure = ddlUOM.SelectedValue;
                    _UpdateInventory.Status = Convert.ToInt32(ddlInvStatus.SelectedValue);
                    //objProp_Inventory.Description2 = txtDes2.Text;
                    //objProp_Inventory.Description3 = txtDes3.Text;
                    //objProp_Inventory.Description4 = txtDes4.Text;
                    objProp_Inventory.DateCreated = Convert.ToDateTime(txtDateCreated.Text);
                    objProp_Inventory.Cat = ddlCategory.SelectedValue != "" ? Convert.ToInt32(ddlCategory.SelectedValue) : 0;
                    objProp_Inventory.Remarks = txtRemarks.Text;

                    _UpdateInventory.Cat = ddlCategory.SelectedValue != "" ? Convert.ToInt32(ddlCategory.SelectedValue) : 0;
                    _UpdateInventory.Remarks = txtRemarks.Text;
                    #endregion
                    #region Eng
                    objProp_Inventory.Specification = txtSpecification.Text;
                    _UpdateInventory.Specification = txtSpecification.Text;
                    //objProp_Inventory.Specification2 = txtSpecification2.Text;
                    //objProp_Inventory.Specification3 = txtSpecification3.Text;
                    //objProp_Inventory.Specification4 = txtSpecification4.Text;
                    //objProp_Inventory.Revision = txtRevision.Text;
                    //    objProp_Inventory.LastRevisionDate = string.IsNullOrEmpty(txtRevisionDate.Text) ? DateTime.MinValue : Convert.ToDateTime(txtRevisionDate.Text);
                    //   objProp_Inventory.Eco = txtECO.Text;
                    //  objProp_Inventory.Drawing = txtDrawing.Text;
                    //objProp_Inventory.Reference = txtReference.Text;
                    //objProp_Inventory.Length = txtLength.Text;
                    //objProp_Inventory.Width = txtWidth.Text;
                    //objProp_Inventory.Height = txtHeight.Text;
                    //objProp_Inventory.Weight = txtWeight.Text;
                    objProp_Inventory.ShelfLife = Convert.ToDecimal(txtshelflife.Text);
                    //objProp_Inventory.InspectionRequired = chkInspRequired.Checked;
                    //objProp_Inventory.CoCRequired = chkCoCpRequired.Checked;
                    //objProp_Inventory.SerializationRequired = chkSerializationRequired.Checked;
                    ProcessPostedDrawing(objProp_Inventory);

                    //ProcessPostedDrawing(_UpdateInventory);
                    #endregion
                    #region Finance
                    objProp_Inventory.UnitCost = 0;
                    _UpdateInventory.UnitCost = 0;
                    objProp_Inventory.LCost = 0;
                    _UpdateInventory.LCost = 0;
                    objProp_Inventory.SAcct = Convert.ToInt16(ddlglsales.SelectedValue);
                    _UpdateInventory.SAcct = Convert.ToInt16(ddlglsales.SelectedValue);
                    objProp_Inventory.GLSales = (string)ddlglsales.SelectedValue;
                    _UpdateInventory.GLSales = (string)ddlglsales.SelectedValue;
                    objProp_Inventory.GLcogs = (string)ddlglcogs.SelectedValue;
                    _UpdateInventory.GLcogs = (string)ddlglcogs.SelectedValue;
                    //  objProp_Inventory.LVendor = Convert.ToInt32(ddlLastPurchaseFromVendor.SelectedValue);
                    // objProp_Inventory.LVendor = Convert.ToInt32(0);

                    //Last Purchase date Once confirmed put here
                    objProp_Inventory.OHValue = 0;
                    _UpdateInventory.OHValue = 0;
                    objProp_Inventory.OOValue = 0;
                    _UpdateInventory.OOValue = 0;
                    objProp_Inventory.Committed = 0;
                    _UpdateInventory.Committed = 0;
                    objProp_Inventory.ABCClass = Convert.ToString(ddlABC.SelectedValue);
                    _UpdateInventory.ABCClass = Convert.ToString(ddlABC.SelectedValue);
                    objProp_Inventory.Tax = Convert.ToInt32(ckhTaxable.Checked);
                    _UpdateInventory.Tax = Convert.ToInt32(ckhTaxable.Checked);
                    objProp_Inventory.InventoryTurns = 0;
                    _UpdateInventory.InventoryTurns = 0;
                    #endregion
                    #region Purchasing
                    //objProp_Inventory.nextPOdate = Convert.ToDateTime(txtNextPoDate.Text);
                    //objProp_Inventory.lastpurchasedate = Convert.ToDateTime(txtLastPODate.Text);
                    objProp_Inventory.LCost = 0;
                    _UpdateInventory.LCost = 0;
                    //objProp_Inventory.LVendor = Convert.ToInt32(txtLastVendor.Text);
                    //objProp_Inventory.LastReceiptDate = string.IsNullOrEmpty(txtLastReceiptDate.Text) ? DateTime.MinValue : Convert.ToDateTime(txtLastReceiptDate.Text); //Convert.ToDateTime(txtLastReceiptDate.Text);
                    objProp_Inventory.Commodity = Convert.ToString(ddlCommodity.SelectedValue);
                    _UpdateInventory.Commodity = Convert.ToString(ddlCommodity.SelectedValue);
                    objProp_Inventory.EAU = 0; //Convert.ToDecimal(txtEAU.Text);
                    _UpdateInventory.EAU = 0; 
                    objProp_Inventory.EOLDate = string.IsNullOrEmpty(txtEOLDate.Text) ? DateTime.MinValue : Convert.ToDateTime(txtEOLDate.Text);//Convert.ToDateTime(txtEOLDate.Text);
                    _UpdateInventory.EOLDate = string.IsNullOrEmpty(txtEOLDate.Text) ? DateTime.MinValue : Convert.ToDateTime(txtEOLDate.Text);
                                                                                                                                                //objProp_Inventory.Weight = Convert.ToString(txtWeight.Text);
                                                                                                                                                //    objProp_Inventory.WarrantyPeriod = string.IsNullOrEmpty(txtWarrantyPeriod.Text) ? 0 : Convert.ToInt32(txtWarrantyPeriod.Text); //Convert.ToDateTime(txtWarrantyPeriod.Text);
                                                                                                                                                // objProp_Inventory.MPN = Convert.ToString(txtMPN.Text);
                                                                                                                                                // objProp_Inventory.ApprovedManufacturer = Convert.ToString(txtAppManuFacturer.Text);
                                                                                                                                                //objProp_Inventory.ApprovedVendor = Convert.ToString(ddlApprovedVendor.SelectedValue);
                    objProp_Inventory.LeadTime = (int)Convert.ToDouble(txtLeadTime.Text);
                    _UpdateInventory.LeadTime = (int)Convert.ToDouble(txtLeadTime.Text);
                    objProp_Inventory.MOQ = Convert.ToDecimal(txtMOQ.Text);
                    _UpdateInventory.MOQ = Convert.ToDecimal(txtMOQ.Text);
                    objProp_Inventory.EOQ = Convert.ToDecimal(txtEOQ.Text);
                    _UpdateInventory.EOQ = Convert.ToDecimal(txtEOQ.Text);
                    #endregion
                    #region Inventory
                    objProp_Inventory.Warehouse = Convert.ToString(0);
                    _UpdateInventory.Warehouse = Convert.ToString(0);
                    //objProp_Inventory.Warehouse = Convert.ToString("OFC");
                    objProp_Inventory.Aisle = Convert.ToString(txtAisle.Text);
                    _UpdateInventory.Aisle = Convert.ToString(txtAisle.Text);
                    objProp_Inventory.Shelf = Convert.ToString(txtShelf.Text);
                    _UpdateInventory.Shelf = Convert.ToString(txtShelf.Text);
                    objProp_Inventory.Bin = Convert.ToString(txtBin.Text);

                    _UpdateInventory.Bin = Convert.ToString(txtBin.Text);
                    //objProp_Inventory.DateLastUsed = Convert.ToString(txtDateLastUsed.Text);
                    //objProp_Inventory.ShelfLife = Convert.ToDecimal(txtshelflife.Text);
                    #endregion
                    #region Sales
                    objProp_Inventory.Price1 = Convert.ToDecimal(txtPrice1.Text);
                    objProp_Inventory.Price2 = Convert.ToDecimal(txtPrice2.Text);
                    objProp_Inventory.Price3 = Convert.ToDecimal(txtPrice3.Text);
                    objProp_Inventory.Price4 = Convert.ToDecimal(txtPrice4.Text);
                    objProp_Inventory.Price5 = Convert.ToDecimal(txtPrice5.Text);
                    objProp_Inventory.Price6 = Convert.ToDecimal(txtPrice6.Text);

                    _UpdateInventory.Price1 = Convert.ToDecimal(txtPrice1.Text);
                    _UpdateInventory.Price2 = Convert.ToDecimal(txtPrice2.Text);
                    _UpdateInventory.Price3 = Convert.ToDecimal(txtPrice3.Text);
                    _UpdateInventory.Price4 = Convert.ToDecimal(txtPrice4.Text);
                    _UpdateInventory.Price5 = Convert.ToDecimal(txtPrice5.Text);
                    _UpdateInventory.Price6 = Convert.ToDecimal(txtPrice6.Text);

                    //objProp_Inventory.AnnualSalesQty = Convert.ToDecimal(txtAnnualSalesQuantity.Text);
                    //objProp_Inventory.AnnualSalesAmt = Convert.ToDecimal(txtAnnualSales.Text);
                    //objProp_Inventory.MaxDiscountPercentage = Convert.ToDecimal(txtMaxDiscount.Text);


                    #endregion
                    #region Documents
                    objProp_Inventory.dtDocs = SaveDocInfo();
                    _UpdateInventory.dtDocs = SaveDocInfo();

                    if (_UpdateInventory.dtDocs.Rows.Count == 0)
                    {
                        DataTable returnVal = SaveEmptyDatatable();
                        _UpdateInventory.dtDocs = returnVal;

                    }
                    else
                    {
                        _UpdateInventory.dtDocs = SaveDocInfo();
                    }

                    #endregion
                    #region Inventory Vendor
                    DataTable dt = ViewState["POVendors"] as DataTable;
                    List<InvParts> _listInvParts = new List<InvParts>();
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                InvParts item = new BusinessEntity.InvParts();
                                item.ID = dt.Rows[i]["ID"] != DBNull.Value ? (int)dt.Rows[i]["ID"] : 0;
                                item.ItemID = dt.Rows[i]["ItemID"] != DBNull.Value ? (int)dt.Rows[i]["ItemID"] : 0;
                                item.MPN = (string)dt.Rows[i]["MPN"];
                                item.Part = (string)dt.Rows[i]["Part"];
                                item.Supplier = (string)dt.Rows[i]["Supplier"];
                                item.VendorID = (int)dt.Rows[i]["VendorID"];
                                item.Price = (float)dt.Rows[i]["Price"];
                                item.Mfg = (string)dt.Rows[i]["Mfg"];
                                item.MfgPrice = (float)dt.Rows[i]["MfgPrice"];



                                _listInvParts.Add(item);
                            }
                        }
                    }

                    #endregion

                    // objProp_Inventory.ApprovedVendors = Getvendors().ToArray();
                    objProp_Inventory.InvPartslist = Getvendors().ToArray();
                    objProp_Inventory.InvItemRevlist = GetRevisiondetail().ToArray();

                    _UpdateInventory.InvPartslist = Getvendors().ToArray();

                    if (Convert.ToInt32(ViewState["mode"]) == 1 && !string.IsNullOrEmpty(invID))
                    {
                        
                        
                        InvWarehouse objInvWarehouse = new InvWarehouse();                      
                        if (ddlInvStatus.SelectedValue == "1")
                        {
                            bool isInvOpen = false;

                            if (IsAPIIntegrationEnable == "YES")
                            {
                                string APINAME = "InventoryAPI/AddInventory_chkInvForOpen";

                                checkkInvForOpenParam _checkkInvForOpen = new checkkInvForOpenParam();
                                _checkkInvForOpen.invID = Convert.ToInt32(invID);

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _checkkInvForOpen);
                                isInvOpen = Convert.ToBoolean(_APIResponse.ResponseData);
                            }
                            else
                            {
                                isInvOpen = objBL_Inventory.chkInvForOpen(Convert.ToInt32(invID));
                            }

                            if (!isInvOpen)
                            {
                                objProp_Inventory.ID = Convert.ToInt32(invID);
                                objProp_Inventory.Name = txtItemHeaderName.Text;
                                objProp_Inventory.Part = txtItemHeaderName.Text;
                                objProp_Inventory.fDesc = txtDes.Text;

                                //API
                                _UpdateInventory.ID = Convert.ToInt32(invID);
                                _UpdateInventory.Name = txtItemHeaderName.Text;
                                _UpdateInventory.Part = txtItemHeaderName.Text;
                                _UpdateInventory.fDesc = txtDes.Text;

                                if (IsAPIIntegrationEnable == "YES")
                                {
                                    string APINAME = "InventoryAPI/AddInventory_UpdateInventory";

                                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateInventory);
                                }
                                else
                                {
                                    objBL_Inventory.UpdateInventory(objProp_Inventory);
                                }

                                clear();
                               
                                // code for save warehouse start
                                if (RadGrid_Warehouse.Items.Count > 0)
                                {
                                    SaveInventoryWarehouse(Convert.ToInt32(invID));
                                }

                                if (IsAPIIntegrationEnable == "YES")
                                {
                                    // code for save warehouse ent  
                                    Response.Redirect("AddInventory.aspx?uid=" + _UpdateInventory.ID + "&mode=1");
                                }
                                else
                                {
                                    // code for save warehouse ent  
                                    Response.Redirect("AddInventory.aspx?uid=" + objProp_Inventory.ID + "&mode=1");
                                }

                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningRef", "noty({text: 'There are open transactions for this item. The status cannot be set to inactive.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                            }
                        }
                        else
                        {
                            objProp_Inventory.ID = Convert.ToInt32(invID);
                            objProp_Inventory.Name = txtItemHeaderName.Text;
                            objProp_Inventory.Part = txtItemHeaderName.Text;
                            objProp_Inventory.fDesc = txtDes.Text;

                            _UpdateInventory.ID = Convert.ToInt32(invID);
                            _UpdateInventory.Name = txtItemHeaderName.Text;
                            _UpdateInventory.Part = txtItemHeaderName.Text;
                            _UpdateInventory.fDesc = txtDes.Text;

                            if (IsAPIIntegrationEnable == "YES")
                            {
                                string APINAME = "InventoryAPI/AddInventory_UpdateInventory";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateInventory);
                            }
                            else
                            {
                                objBL_Inventory.UpdateInventory(objProp_Inventory);
                            }

                            clear();
                            // code for save warehouse start
                            if (RadGrid_Warehouse.Items.Count > 0)                            
                                SaveInventoryWarehouse(Convert.ToInt32(invID));

                            if (IsAPIIntegrationEnable == "YES")
                            {
                                // code for save warehouse end
                                Response.Redirect("AddInventory.aspx?uid=" + _UpdateInventory.ID + "&mode=1");
                            }
                            else
                            {
                                // code for save warehouse end
                                Response.Redirect("AddInventory.aspx?uid=" + objProp_Inventory.ID + "&mode=1");
                            }
                        }
                    }
                    else
                    {
                        objProp_Inventory.Name = txtItemHeaderName.Text;
                        objProp_Inventory.fDesc = txtDes.Text;

                        BusinessEntity.Inventory createdinv = objBL_Inventory.CreateInventory(objProp_Inventory);
                        InvWarehouse objInvWarehouse = new InvWarehouse();                       
                        // code for save warehouse start
                        if (RadGrid_Warehouse.Items.Count > 0)                        
                            SaveInventoryWarehouse(Convert.ToInt32(createdinv.ID));                        
                        // code for save warehouse ent  
                        ViewState["mode"] = 0;
                        if (createdinv != null)
                        {
                            clear();
                            Response.Redirect("AddInventory.aspx?uid=" + createdinv.ID + "&mode=0");
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'Item could not be created! </br> <b> Inventory P/N# : " + txtItemHeaderName.Text + "</b>',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        }
                    }
                }
                else
                {                    
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningRef", "noty({text: 'Part Number already exists, Please use different Part Number!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }

            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }

  
    public void SaveInventoryWarehouse(int invID)
    {
        try
        {
            DataTable dt = new DataTable();
            List<string> objWareHouses = new List<string>();
            InvWarehouse objInvWarehouse = new InvWarehouse();
            foreach (GridDataItem dataItem in RadGrid_Warehouse.Items)
            {
                Label lblWarehouseID = (Label)dataItem.FindControl("lblWarehouseID");
                objInvWarehouse.InvID = Convert.ToInt32(invID);
                objInvWarehouse.WarehouseID = lblWarehouseID.Text;

                _CreateInvMergeWarehouse.InvID = Convert.ToInt32(invID);
                _CreateInvMergeWarehouse.WarehouseID = lblWarehouseID.Text;

                string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "InventoryAPI/AddInventory_CreateInvMergeWarehouse";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _CreateInvMergeWarehouse);
                }
                else
                {
                    objBL_Inventory.CreateInvMergeWarehouse(objInvWarehouse);
                }

            }

            objProp_Inventory = GetInventoryById(Convert.ToString(invID)).ReponseObject;
            dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("InvID", typeof(int));
            dt.Columns.Add("WarehouseID", typeof(string));
            dt.Columns.Add("WarehouseName", typeof(string));
            dt.Columns.Add("Hand", typeof(string));
            dt.Columns.Add("Balance", typeof(string));
            dt.Columns.Add("Committed", typeof(string));
            dt.Columns.Add("fOrder", typeof(string));
            dt.Columns.Add("Available", typeof(string));
            dt.Columns.Add("Company", typeof(string));
            dt.AcceptChanges();
            List<InvWarehouse> lstvendorinfo = new List<InvWarehouse>();
            if (objProp_Inventory.InvWarehouseMergelist != null)
            {
                foreach (InvWarehouse invman in objProp_Inventory.InvWarehouseMergelist)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = invman.ID;
                    dr["InvID"] = invman.InvID;
                    dr["WarehouseID"] = invman.WarehouseID;
                    dr["WarehouseName"] = invman.WarehouseName;
                    dr["Hand"] = invman.Hand;
                    dr["Balance"] = invman.Balance;
                    dr["Committed"] = invman.Committed;
                    dr["fOrder"] = invman.fOrder;
                    dr["Available"] = invman.Available;
                    dr["Company"] = invman.Company;
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }
            }
            RadGrid_Warehouse.VirtualItemCount = dt.Rows.Count;
            RadGrid_Warehouse.DataSource = dt;
            RadGrid_Warehouse.DataBind();
            ViewState["InvMergewarehouse"] = dt;
            objWareHouses = null;
        }
        catch (Exception ex)
        {            
        }
    }
    protected void btnaddVendorInfo_Click(object sender, EventArgs e)
    {
        clear();
        string itemname = txtItemHeaderName.Text;
        string itemdecs = txtDes.Text;


        string script = "function f(){$find(\"" + RadWindowVendor.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }

    protected void lnkSaveInventoryWarehouse_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["uid"] != null && Convert.ToString(Request.QueryString["uid"]) != "")
        {
            //Save Code

            // Grid Bind Code

            InvParts objinvparts = new InvParts();
            int partid = 0;
            if (hdninvvendinfo.Value.ToString() != "" && hdninvvendinfo.Value != null)
            {
                partid = Convert.ToInt32(hdninvvendinfo.Value);
            }
            objinvparts.ItemID = Convert.ToInt32(Request.QueryString["uid"]);
            objinvparts.MPN = txtInventoryMPN.Text;
            objinvparts.Part = txtVendorPartNumber.Text;
            objinvparts.Supplier = ddlInventoryApprovedVendor.SelectedItem.Text;
            objinvparts.VendorID = Convert.ToInt32(ddlInventoryApprovedVendor.SelectedItem.Value);
            objinvparts.Price = float.Parse(txtVendorPrice.Text);
            objinvparts.Mfg = txtInventoryApprovedManufacturer.Text;
            objinvparts.MfgPrice = float.Parse(txtManufacturerPrice.Text);
            objinvparts.ID = partid;

            //API
            _CreateInventoryParts.ItemID = Convert.ToInt32(Request.QueryString["uid"]);
            _CreateInventoryParts.MPN = txtInventoryMPN.Text;
            _CreateInventoryParts.Part = txtVendorPartNumber.Text;
            _CreateInventoryParts.Supplier = ddlInventoryApprovedVendor.SelectedItem.Text;
            _CreateInventoryParts.VendorID = Convert.ToInt32(ddlInventoryApprovedVendor.SelectedItem.Value);
            _CreateInventoryParts.Price = float.Parse(txtVendorPrice.Text);
            _CreateInventoryParts.Mfg = txtInventoryApprovedManufacturer.Text;
            _CreateInventoryParts.MfgPrice = float.Parse(txtManufacturerPrice.Text);
            _CreateInventoryParts.ID = partid;

            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "InventoryAPI/AddInventory_CreateInventoryParts";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _CreateInventoryParts);
            }
            else
            {
                objBL_Inventory.CreateInventoryParts(objinvparts);
            }

            objProp_Inventory = GetInventoryById(Request.QueryString["uid"]).ReponseObject;

            #region Inventory Vendor
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("ItemID", typeof(int));
            dt.Columns.Add("MPN", typeof(string));
            dt.Columns.Add("Part", typeof(string));
            dt.Columns.Add("Supplier", typeof(string));
            dt.Columns.Add("VendorID", typeof(int));
            dt.Columns.Add("Price", typeof(float));
            dt.Columns.Add("Mfg", typeof(string));
            dt.Columns.Add("MfgPrice", typeof(float));
            dt.AcceptChanges();



            List<InvParts> lstvendorinfo = new List<InvParts>();
            if (objProp_Inventory.InvPartslist != null)
            {
                foreach (InvParts invman in objProp_Inventory.InvPartslist)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = invman.ID;
                    dr["ItemID"] = invman.ItemID;
                    dr["Supplier"] = invman.Supplier;
                    dr["VendorID"] = invman.VendorID;
                    dr["Price"] = invman.Price;
                    dr["Mfg"] = invman.Mfg;
                    dr["MfgPrice"] = invman.MfgPrice;
                    dr["MPN"] = invman.MPN;
                    dr["Part"] = invman.Part;

                    dt.Rows.Add(dr);
                    dt.AcceptChanges();

                }
            }
            RadGrid_Vendors.VirtualItemCount = dt.Rows.Count;
            RadGrid_Vendors.DataSource = dt;
            RadGrid_Vendors.DataBind();

            ViewState["POVendors"] = dt;


            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "modalclose();", true);

            clear();
            #endregion

            if (partid == 0)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "addRef", "noty({text: 'Added successfully.',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "editRef", "noty({text: 'Updated successfully.',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }

        }
        else
        {
            DataTable dt = (DataTable)ViewState["POVendors"];

            // if (dtlVendors.Items.Count > 0)
            if (RadGrid_Vendors.Items.Count > 0)
            {
                if (hdninvvendinfo.Value != "")
                {
                    int index = 0;
                    //foreach (DataListItem dtitemm in dtlVendors.Items)
                    foreach (GridDataItem dtitemm in RadGrid_Vendors.Items)
                    {
                        InvParts item = new BusinessEntity.InvParts();
                        //CheckBox chkinvi = dtitemm.FindControl("chkvenitem") as CheckBox;
                        HiddenField id = dtitemm.FindControl("hdnid") as HiddenField;
                        Label lblmpn = dtitemm.FindControl("lblmpn") as Label;
                        Label lblVPN = dtitemm.FindControl("lblVPN") as Label;

                        Label lblVendor = dtitemm.FindControl("lblVendor") as Label;
                        Label lblVendorID = dtitemm.FindControl("lblVendorID") as Label;
                        Label lblVendorPrice = dtitemm.FindControl("lblVendorPrice") as Label;
                        Label lblManufacturerName = dtitemm.FindControl("lblManufacturerName") as Label;
                        Label lblMfgPrice = dtitemm.FindControl("lblMfgPrice") as Label;

                        if (dtitemm.Selected == true)
                        {
                            id.Value = hdninvvendinfo.Value;
                            lblmpn.Text = txtInventoryMPN.Text;
                            lblVPN.Text = txtVendorPartNumber.Text;
                            lblVendor.Text = ddlInventoryApprovedVendor.SelectedItem.Text;
                            lblVendorID.Text = ddlInventoryApprovedVendor.SelectedItem.Value;
                            lblVendorPrice.Text = txtVendorPrice.Text;
                            lblManufacturerName.Text = txtInventoryApprovedManufacturer.Text;
                            lblMfgPrice.Text = txtManufacturerPrice.Text;
                            if (dt != null)
                            {
                                if (dt.Rows.Count > 0)
                                {


                                    //for (int i = 0; i < dt.Rows.Count; i++)
                                    //{

                                    if (id.Value == Convert.ToString(dt.Rows[index]["ID"]))
                                    {
                                        // dt.Rows[i]["ID"] = hdninvvendinfo.Value != "" ? Convert.ToInt32(hdninvvendinfo.Value) : 0;
                                        dt.Rows[index]["ItemID"] = invID != "" ? Convert.ToInt32(invID) : 0;
                                        dt.Rows[index]["MPN"] = (txtInventoryMPN.Text);
                                        dt.Rows[index]["Part"] = (txtVendorPartNumber.Text);
                                        dt.Rows[index]["Supplier"] = (ddlInventoryApprovedVendor.SelectedItem.Text);
                                        dt.Rows[index]["VendorID"] = (ddlInventoryApprovedVendor.SelectedItem.Value);
                                        dt.Rows[index]["Price"] = (txtVendorPrice.Text);
                                        dt.Rows[index]["Mfg"] = (txtInventoryApprovedManufacturer.Text);
                                        dt.Rows[index]["MfgPrice"] = (txtManufacturerPrice.Text);
                                    }
                                    // }
                                }
                            }

                            ViewState["POVendors"] = dt;
                        }

                        index++;


                    }
                }
                else
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = hdninvvendinfo.Value != "" ? Convert.ToInt32(hdninvvendinfo.Value) : 0;
                    dr["ItemID"] = invID != "" ? Convert.ToInt32(invID) : 0;
                    dr["MPN"] = (txtInventoryMPN.Text);
                    dr["Part"] = (txtVendorPartNumber.Text);
                    dr["Supplier"] = (ddlInventoryApprovedVendor.SelectedItem.Text);
                    dr["VendorID"] = (ddlInventoryApprovedVendor.SelectedItem.Value);
                    dr["Price"] = (txtVendorPrice.Text);
                    dr["Mfg"] = (txtInventoryApprovedManufacturer.Text);
                    dr["MfgPrice"] = (txtManufacturerPrice.Text);
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                    RadGrid_Vendors.VirtualItemCount = dt.Rows.Count;
                    RadGrid_Vendors.DataSource = dt;
                    RadGrid_Vendors.DataBind();
                    ViewState["POVendors"] = dt;


                }
            }
            else
            {
                dt = new DataTable();
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("ItemID", typeof(int));
                dt.Columns.Add("MPN", typeof(string));
                dt.Columns.Add("Part", typeof(string));
                dt.Columns.Add("Supplier", typeof(string));
                dt.Columns.Add("VendorID", typeof(int));
                dt.Columns.Add("Price", typeof(float));
                dt.Columns.Add("Mfg", typeof(string));
                dt.Columns.Add("MfgPrice", typeof(float));
                dt.AcceptChanges();


                DataRow dr = dt.NewRow();
                dr["ID"] = hdninvvendinfo.Value != "" ? Convert.ToInt32(hdninvvendinfo.Value) : 0;
                dr["ItemID"] = invID != "" ? Convert.ToInt32(invID) : 0;
                dr["MPN"] = (txtInventoryMPN.Text);
                dr["Part"] = (txtVendorPartNumber.Text);
                dr["Supplier"] = (ddlInventoryApprovedVendor.SelectedItem.Text);
                dr["VendorID"] = (ddlInventoryApprovedVendor.SelectedItem.Value);
                dr["Price"] = (txtVendorPrice.Text);
                dr["Mfg"] = (txtInventoryApprovedManufacturer.Text);
                dr["MfgPrice"] = (txtManufacturerPrice.Text);
                dt.Rows.Add(dr);
                dt.AcceptChanges();
                RadGrid_Vendors.VirtualItemCount = dt.Rows.Count;
                RadGrid_Vendors.DataSource = dt;
                RadGrid_Vendors.DataBind();



                ViewState["POVendors"] = dt;
            }

            //this.programmaticModalPopup.Hide();
            //pnlInventoryWarehouse.Visible = false;


            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "modalclose();", true);

            clear();
        }
    }

    protected void lnkCloseInventoryWarehouse_Click(object sender, EventArgs e)
    {
        //this.programmaticModalPopup.Hide();
        //this.pnlInventoryWarehouse.Visible = false;
        clear();
    }
    //public void ddlHeaderNameName_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (ddlHeaderNameName.SelectedValue != "0")
    //    {
    //        FillData(ddlHeaderNameName.SelectedValue);
    //    }
    //}

    protected void btndekVendorInfo_Click(object sender, EventArgs e)
    {
        //DataTable dt = (DataTable)ViewState["POVendors"];

        foreach (GridDataItem item in RadGrid_Vendors.SelectedItems)
        {
            //Label lblindex = (Label)item.FindControl("lblindex");
            //dt.Rows.RemoveAt(Convert.ToInt32(lblindex.Text));
            //dt.AcceptChanges();

            HiddenField hdnid = (HiddenField)item.FindControl("hdnid");

            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "InventoryAPI/AddInventory_DeleteInventoryParts";

                _DeleteInventoryParts.invpartID = Convert.ToInt32(hdnid.Value);

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _DeleteInventoryParts);
            }
            else
            {
                objBL_Inventory.DeleteInventoryParts(Convert.ToInt32(hdnid.Value));
            }

            objProp_Inventory = GetInventoryById(Request.QueryString["uid"]).ReponseObject;

            #region Inventory Vendor
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("ItemID", typeof(int));
            dt.Columns.Add("MPN", typeof(string));
            dt.Columns.Add("Part", typeof(string));
            dt.Columns.Add("Supplier", typeof(string));
            dt.Columns.Add("VendorID", typeof(int));
            dt.Columns.Add("Price", typeof(float));
            dt.Columns.Add("Mfg", typeof(string));
            dt.Columns.Add("MfgPrice", typeof(float));
            dt.AcceptChanges();



            List<InvParts> lstvendorinfo = new List<InvParts>();
            if (objProp_Inventory.InvPartslist != null)
            {
                foreach (InvParts invman in objProp_Inventory.InvPartslist)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = invman.ID;
                    dr["ItemID"] = invman.ItemID;
                    dr["Supplier"] = invman.Supplier;
                    dr["VendorID"] = invman.VendorID;
                    dr["Price"] = invman.Price;
                    dr["Mfg"] = invman.Mfg;
                    dr["MfgPrice"] = invman.MfgPrice;
                    dr["MPN"] = invman.MPN;
                    dr["Part"] = invman.Part;

                    dt.Rows.Add(dr);
                    dt.AcceptChanges();

                }
            }
            RadGrid_Vendors.VirtualItemCount = dt.Rows.Count;
            RadGrid_Vendors.DataSource = dt;
            RadGrid_Vendors.DataBind();

            ViewState["POVendors"] = dt;
            #endregion

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "modalclose();", true);

            //clear();
        }
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "deleteRef", "noty({text: 'Deleted successfully.',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        //RadGrid_Vendors.VirtualItemCount = dt.Rows.Count;
        //RadGrid_Vendors.DataSource = dt;
        //RadGrid_Vendors.DataBind();

    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("Inventory.aspx?fil=1", false);
    }

    protected void lblName_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;

        string[] CommandArgument = btn.CommandArgument.Split(',');

        string FileName = CommandArgument[0];

        string FilePath = CommandArgument[1];

        DownloadDocument(FilePath, FileName);
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
                ////if blocks transfered not equals total number of blocks
                //if (i < maxCount)
                //    return false;
                //return true; 
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

    protected void lnkUploadDoc_Click(object sender, EventArgs e)
    {
        try
        {
            string filename = string.Empty;
            string fullpath = string.Empty;
            string MIME = string.Empty;
            if (FileUpload1.HasFile)
            {
                string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                string savepath = savepathconfig + @"\" + Session["dbname"] + @"\ld_" + Request.QueryString["uid"].ToString() + @"\";
                filename = FileUpload1.FileName;
                fullpath = savepath + filename;
                MIME = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).Substring(1);

                if (File.Exists(fullpath))
                {
                    GeneralFunctions objGeneralFunctions = new GeneralFunctions();
                    filename = objGeneralFunctions.generateRandomString(4) + "_" + filename;
                    fullpath = savepath + filename;
                }

                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }

                FileUpload1.SaveAs(fullpath);
            }

            objMapData.Screen = "Inventory";
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objMapData.TempId = "0";
            objMapData.FileName = filename;
            objMapData.DocTypeMIME = MIME;
            objMapData.FilePath = fullpath;
            //objMapData.Subject = txtNoteSub.Text.Trim();
            //objMapData.Body = txtNoteBody.Text.Trim();
            //objMapData.Mode = Convert.ToInt16(ViewState["notesmode"]);
            //if (ViewState["notesmode"].ToString() == "1")
            //    objMapData.DocID = Convert.ToInt32(hdnNoteID.Value);
            //else
            objMapData.DocID = 0;
            objMapData.Mode = 0;
            objMapData.ConnConfig = Session["config"].ToString();

            //API
            _AddFile.Screen = "Inventory";
            _AddFile.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            _AddFile.TempId = "0";
            _AddFile.FileName = filename;
            _AddFile.DocTypeMIME = MIME;
            _AddFile.FilePath = fullpath;
            _AddFile.DocID = 0;
            _AddFile.Mode = 0;
            _AddFile.ConnConfig = Session["config"].ToString();

            _GetDocuments.Mode = 0;

            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "InventoryAPI/AddInventory_AddFile";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddFile);
            }
            else
            {
                objBL_MapData.AddFile(objMapData);
            }

            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.dtDocs = SaveDocInfo();

            _UpdateDocInfo.ConnConfig = Session["config"].ToString();
            _UpdateDocInfo.dtDocs = SaveDocInfo();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "InventoryAPI/AddInventory_UpdateDocInfo";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateDocInfo);
            }
            else
            {
                objBL_User.UpdateDocInfo(objPropUser);
            }


            GetDocuments();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void GetDocuments()
    {
        bool IsProspect = false;
        if (Request.QueryString["cpw"] != null)
            IsProspect = true;

        if (IsProspect)
        {
            objMapData.Screen = "SalesLead";
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["prospectid"].ToString());

            _GetDocuments.Screen = "SalesLead";
            _GetDocuments.TicketID = Convert.ToInt32(Request.QueryString["prospectid"].ToString());
        }
        else
        {
            objMapData.Screen = "Inventory";
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());

            _GetDocuments.Screen = "Inventory";
            _GetDocuments.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        }

        objMapData.TempId = "0";
        _GetDocuments.TempId = "0";

        objMapData.ConnConfig = Session["config"].ToString();
        _GetDocuments.ConnConfig = Session["config"].ToString();

        DataSet ds = new DataSet();

        List<GetDocumentsViewModel> _lstGetDocuments = new List<GetDocumentsViewModel>();
        string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "InventoryAPI/AddInventory_GetDocuments";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetDocuments);
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetDocuments = serializer.Deserialize<List<GetDocumentsViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetDocumentsViewModel>(_lstGetDocuments);
        }
        else
        {
            ds = objBL_MapData.GetDocuments(objMapData);
        }

        RadGrid_Documents.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_Documents.DataSource = ds.Tables[0];
        RadGrid_Documents.DataBind();
    }

    private DataTable SaveDocInfo()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Portal", typeof(int));
        dt.Columns.Add("Remarks", typeof(string));
        dt.Columns.Add("MSVisible", typeof(byte));

        //foreach (GridViewRow gr in gvDocuments.Rows)
        foreach (GridDataItem gr in RadGrid_Documents.Items)
        {
            Label lblID = (Label)gr.FindControl("lblID");
            // CheckBox chkPortal = (CheckBox)gr.FindControl("chkPortal");
            TextBox txtRemarks = (TextBox)gr.FindControl("txtRemarrkss");
            CheckBox chkMSVisible = (CheckBox)gr.FindControl("chkMSVisible");

            DataRow dr = dt.NewRow();
            dr["ID"] = lblID.Text;
            dr["Portal"] = 1;
            dr["Remarks"] = txtRemarks.Text;
            dr["MSVisible"] = chkMSVisible.Checked;
            dt.Rows.Add(dr);
        }
        return dt;
    }

    //API
    public DataTable SaveEmptyDatatable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Portal", typeof(int));
        dt.Columns.Add("Remarks", typeof(string));
        dt.Columns.Add("MSVisible", typeof(byte));

        DataRow dr = dt.NewRow();
        dr["ID"] = "0";
        dr["Portal"] = "0";
        dr["Remarks"] = "";
        dr["MSVisible"] = "0";
        dt.Rows.Add(dr);

        return dt;
    }
    public void DeleteFileFromFolder(string StrFilename, int DocumentID)
    {
        try
        {
            //File.Delete(StrFilename);
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

    protected void lnkDeleteDoc_Click(object sender, EventArgs e)
    {
        //foreach (GridViewRow di in gvDocuments.Rows)
        foreach (GridDataItem di in RadGrid_Documents.SelectedItems)
        {

            Label lblID = (Label)di.FindControl("lblId");


            DeleteFileFromFolder(string.Empty, Convert.ToInt32(lblID.Text));

        }
    }

    private void DeleteFile(int DocumentID)
    {
        try
        {
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.DocumentID = DocumentID;

            _DeleteFile.ConnConfig = Session["config"].ToString();
            _DeleteFile.DocumentID = DocumentID;

            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "InventoryAPI/AddInventory_DeleteFile";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _DeleteFile);
            }
            else
            {
                objBL_MapData.DeleteFile(objMapData);
            }


            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.dtDocs = SaveDocInfo();

            _UpdateDocInfo.ConnConfig = Session["config"].ToString();
            _UpdateDocInfo.dtDocs = SaveDocInfo();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "InventoryAPI/AddInventory_UpdateDocInfo";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateDocInfo);
            }
            else
            {
                objBL_User.UpdateDocInfo(objPropUser);
            }

            GetDocuments();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrdelete", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

  

    #region ::Methods::
    private void BindControls()
    {
        #region Unit Of Measure
        objBL_Inventory = new BL_Inventory();
        List<UnitOfMeasure> dsuom = objBL_Inventory.GetALLUnitOfMeasure();

        if (dsuom != null)
        {
            if (dsuom.Count > 0)
            {

                //ddlUOM.DataSource = dsuom;
                //ddlUOM.DataValueField = "ID";
                //ddlUOM.DataTextField = "Description";
                //// ddlUOM.DataTextFormatString = "[(Code)]15[|(Description)]32";
                //ddlUOM.DataBind();

                ////ddlUOM.Items.Add(new ListItem("Select UOM", "0"));
                //ddlUOM.Items.Add(new ListItem("--Select--", "0"));
                //ddlUOM.SelectedValue = "0";


                ddlUOM.Items.Add(new ListItem("--Select--", "0"));
                foreach (UnitOfMeasure _dsuom in dsuom)
                {
                    ddlUOM.Items.Add(new ListItem(string.Format("{0}", _dsuom.Description), _dsuom.ID.ToString()));
                }
                ddlUOM.DataBind();
                ddlUOM.SelectedValue = "0";

            }
        }
        #endregion

        #region Status
        ddlInvStatus.Items.Add(new ListItem("Active", "0"));
        ddlInvStatus.Items.Add(new ListItem("Inactive", "1"));
        ddlInvStatus.Items.Add(new ListItem("On Hold", "2"));

        #endregion

        #region Category
        objBL_Inventory = new BL_Inventory();
        List<Itype> itypes = objBL_Inventory.GetALLItype();

        if (itypes != null)
        {
            if (itypes.Count > 0)
            {

                //ddlCategory.DataSource = itypes;
                //ddlCategory.DataValueField = "ID";
                //ddlCategory.DataTextField = "Type";
                //// ddlUOM.DataTextFormatString = "[(Code)]15[|(Description)]32";
                //ddlCategory.DataBind();

                ////ddlCategory.Items.Add(new ListItem("Select Category", "0"));
                //ddlCategory.Items.Add(new ListItem("--Select--", "0"));

                //ddlCategory.SelectedValue = "0";

                ddlCategory.Items.Add(new ListItem("--Select--", "0"));
                foreach (Itype _itypes in itypes)
                {
                    ddlCategory.Items.Add(new ListItem(string.Format("{0}", _itypes.Type), _itypes.ID.ToString()));
                }
                ddlCategory.DataBind();
                ddlCategory.SelectedValue = "0";


            }
        }
        #endregion

        #region Gl
        List<Chart> chart1 = new List<Chart>();

        string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "InventoryAPI/AddInventory_GetChartByType";

            _GetChartByType.type = (int)chartType.Revenue;

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetChartByType);
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            chart1 = serializer.Deserialize<List<Chart>>(_APIResponse.ResponseData);
        }
        else
        {
            chart1 = objBL_Inventory.GetChartByType((int)chartType.Revenue);
        }

        if (chart1 != null)
        {
            if (chart1.Count > 0)
            {
                //ddlglsales.DataSource = chart1;
                //ddlglsales.DataValueField = "ID";
                // ddlglsales.DataTextField = string.Format("{0}-{1}", "Acct", "fDesc");

                ddlglsales.Items.Add(new ListItem("Select GL Sales", "0"));

                foreach (Chart chart in chart1)
                {
                    ddlglsales.Items.Add(new ListItem(string.Format("{0}-{1}", chart.Acct, chart.fDesc), chart.ID.ToString()));

                }
                ddlglsales.DataBind();




                ddlglsales.SelectedValue = "0";
            }
        }

        List<Chart> chart2 = new List<Chart>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "InventoryAPI/AddInventory_GetChartByType";

            GetChartByTypeParam _getChartByType = new GetChartByTypeParam();
            _getChartByType.type = (int)chartType.CostofSales;

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getChartByType);
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            chart2 = serializer.Deserialize<List<Chart>>(_APIResponse.ResponseData);
        }
        else
        {
            chart2 = objBL_Inventory.GetChartByType((int)chartType.CostofSales);
        }

        if (chart2 != null)
        {


            if (chart2.Count > 0)
            {
                ddlglcogs.Items.Add(new ListItem("Select GL COGS", "0"));

                foreach (Chart chart in chart2)
                {
                    ddlglcogs.Items.Add(new ListItem(string.Format("{0}-{1}", chart.ID, chart.fDesc), chart.ID.ToString()));

                }
                ddlglcogs.DataBind();
                ddlglcogs.SelectedValue = "0";

                //ddlglcogs.DataSource = chart2;
                //ddlglcogs.DataValueField = "ID";
                //ddlglcogs.DataTextField = "fDesc";
                //ddlglcogs.DataBind();

                //ddlglcogs.Items.Add(new ListItem("Select GL COGS", "0"));

                //ddlglcogs.SelectedValue = "0";
            }
        }





        #endregion

        #region ABC Class
        ddlABC.Items.Add(new ListItem("None", "0"));
        ddlABC.Items.Add(new ListItem("Class A", "A"));
        ddlABC.Items.Add(new ListItem("Class B", "B"));
        ddlABC.Items.Add(new ListItem("Class C", "C"));
        #endregion

        //#region WareHouse
        //List<string> strwarehouse = new List<string>();
        //objProp_User.ConnConfig = Session["config"].ToString();
        //strwarehouse = objBL_Inventory.GetInventoryWarehouse(objProp_User);
        //ddlWareHouse.DataSource = strwarehouse;
        //ddlWareHouse.DataBind();
        //#endregion

        #region Warehouse Checkboxes


        DataSet dsstrwarehouse = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();

        _GetInventoryActiveWarehouse.ConnConfig = Session["config"].ToString();
        //dsstrwarehouse = objBL_Inventory.GetInventoryWarehouse(objProp_User);

        List<GetInventoryActiveWarehouseViewModel> _lstGetInventoryActiveWarehouse = new List<GetInventoryActiveWarehouseViewModel>();
      
        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "InventoryAPI/AddInventory_GetInventoryActiveWarehouse";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInventoryActiveWarehouse);
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetInventoryActiveWarehouse = serializer.Deserialize<List<GetInventoryActiveWarehouseViewModel>>(_APIResponse.ResponseData);
            dsstrwarehouse = CommonMethods.ToDataSet<GetInventoryActiveWarehouseViewModel>(_lstGetInventoryActiveWarehouse);
        }
        else
        {
            dsstrwarehouse = objBL_Inventory.GetInventoryActiveWarehouse(objProp_User);
        }

        

        //DataTable dt = new DataTable();
        //dt.Columns.Add("val", typeof(string));

        //for (int i = 0; i < 10; i++)
        //    dt.Rows.Add("testing" + i.ToString());
        DataTable DTWarehouse = dsstrwarehouse.Tables[0];
        foreach (DataRow drr in DTWarehouse.Rows)
        {
            if (Convert.ToString(drr["ID"]).ToLower() == "ofc".ToLower())
            {
                DTWarehouse.Rows.Remove(drr);
                break;
            }


        }

        rptWarehouse.DataSource = DTWarehouse;// dsstrwarehouse.Tables[0];
        rptWarehouse.DataBind();


        if (Request.QueryString["uid"] == null)
        {
            //DataSet dsDeaultWarehouse = new DataSet();
            //objProp_User.ConnConfig = Session["config"].ToString();
            //dsDeaultWarehouse = objBL_Inventory.GetDeaultWarehouse();
            //RadGrid_Warehouse.DataSource = dsDeaultWarehouse.Tables[0];
            //RadGrid_Warehouse.DataBind();

            DataTable dtMergeWarehouse = new DataTable();
            dtMergeWarehouse.Columns.Add("ID", typeof(int));
            dtMergeWarehouse.Columns.Add("InvID", typeof(int));
            dtMergeWarehouse.Columns.Add("WarehouseID", typeof(string));
            dtMergeWarehouse.Columns.Add("WarehouseName", typeof(string));
            dtMergeWarehouse.Columns.Add("Hand", typeof(decimal));
            dtMergeWarehouse.Columns.Add("Balance", typeof(decimal));
            dtMergeWarehouse.Columns.Add("Committed", typeof(decimal));
            dtMergeWarehouse.Columns.Add("fOrder", typeof(decimal));
            dtMergeWarehouse.Columns.Add("Available", typeof(decimal));
            dtMergeWarehouse.Columns.Add("Company", typeof(string));

            dtMergeWarehouse.AcceptChanges();



            List<InvWarehouse> lstInvWarehouseinfo = new List<InvWarehouse>();
            if (objProp_Inventory.InvWarehouseMergelist != null)
            {
                foreach (InvWarehouse InvWarehouse in objProp_Inventory.InvWarehouseMergelist)
                {
                    DataRow dr = dtMergeWarehouse.NewRow();
                    dr["ID"] = InvWarehouse.ID;
                    dr["InvID"] = InvWarehouse.InvID;
                    dr["WarehouseID"] = InvWarehouse.WarehouseID;
                    dr["WarehouseName"] = InvWarehouse.WarehouseName;
                    dr["Hand"] = InvWarehouse.Hand;
                    dr["Balance"] = InvWarehouse.Balance;
                    dr["Committed"] = InvWarehouse.Committed;
                    dr["fOrder"] = InvWarehouse.fOrder;
                    dr["Available"] = InvWarehouse.Available;
                    dr["Company"] = InvWarehouse.Company;
                    dtMergeWarehouse.Rows.Add(dr);
                    dtMergeWarehouse.AcceptChanges();

                }
            }
            DataSet dsDeaultWarehouse = new DataSet();
            //objProp_User.ConnConfig = Session["config"].ToString();
            dsDeaultWarehouse = objBL_Inventory.GetDeaultWarehouse();
            dtMergeWarehouse = dsDeaultWarehouse.Tables[0];
            RadGrid_Warehouse.VirtualItemCount = dtMergeWarehouse.Rows.Count;
            RadGrid_Warehouse.DataSource = dtMergeWarehouse;
            RadGrid_Warehouse.DataBind();

            ViewState["InvMergewarehouse"] = dtMergeWarehouse;
        }


        #endregion
        #region Vendor Information
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("InvID", typeof(int));
        dt.Columns.Add("MPN", typeof(string));
        dt.Columns.Add("ApprovedManufacturer", typeof(string));
        dt.Columns.Add("ApprovedVendor", typeof(string));
        dt.Columns.Add("ApprovedVendorId", typeof(string));
        dt.AcceptChanges();
        RadGrid_Vendors.DataSource = dt;
        RadGrid_Vendors.DataBind();

        //txtDateCreated.Text = DateTime.Now.ToString(); //DateTime.Today.ToString("MM/dd/yyyy");

        txtDateCreated.Text= DateTime.Today.ToString("MM/dd/yyyy");


        objBL_Inventory = new BL_Inventory();
        Dictionary<string, string> lstvendor = objBL_Inventory.GetAllVendor(Session["config"].ToString());
        ddlInventoryApprovedVendor.DataValueField = "Key";
        ddlInventoryApprovedVendor.DataTextField = "Value";
        ddlInventoryApprovedVendor.DataSource = lstvendor;
        ddlInventoryApprovedVendor.DataBind();

        //ddlLastPurchaseFromVendor.DataValueField = "Key";
        //ddlLastPurchaseFromVendor.DataTextField = "Value";
        //ddlLastPurchaseFromVendor.DataSource = lstvendor;
        //ddlLastPurchaseFromVendor.DataBind();

        ddlApprovedVendorrequestquote.DataValueField = "Key";
        ddlApprovedVendorrequestquote.DataTextField = "Value";
        ddlApprovedVendorrequestquote.DataSource = lstvendor;
        ddlApprovedVendorrequestquote.DataBind();
        linkquoterequestid.Visible = false;
        #endregion

        #region Name DDl Binding
        objBL_Inventory = new BL_Inventory();
        objProp_Inventory.ConnConfig = Session["config"].ToString();
        objProp_Inventory.UserID = Convert.ToInt32(Session["UserID"].ToString());

        _GetALLInventory.ConnConfig = Session["config"].ToString();
        _GetALLInventory.UserID = Convert.ToInt32(Session["UserID"].ToString());

        if (Session["CmpChkDefault"].ToString() == "1")
        {
            objProp_Inventory.EN = 1;
            _GetALLInventory.EN = 1;
        }
        else
        {
            objProp_Inventory.EN = 0;
            _GetALLInventory.EN = 0;
        }

        DataSet dsinv = new DataSet();
        ListGetALLInventory _lstInventory = new ListGetALLInventory();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "InventoryAPI/InventoryList_GetALLInventory";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetALLInventory);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstInventory = serializer.Deserialize<ListGetALLInventory>(_APIResponse.ResponseData);
            dsinv = _lstInventory.lstTable1.ToDataSet();
        }
        else
        {
            dsinv = objBL_Inventory.GetAllInventory(objProp_Inventory);
        }


        if (dsinv != null)
        {
            if (dsinv.Tables.Count > 0)
            {
                if (dsinv.Tables[0].Rows.Count > 0)
                {
                    var dtinv = from c in dsinv.Tables[0].Select() select new { ID = c[0], Name = c[1] };



                    //ddlHeaderNameName.DataSource = dtinv;
                    //ddlHeaderNameName.DataTextField = "Name";
                    //ddlHeaderNameName.DataValueField = "ID";
                    //ddlHeaderNameName.DataBind();
                    //ddlHeaderNameName.Items.Add(new ListItem("Select P/N", "0"));
                    //ddlHeaderNameName.SelectedValue = "0";

                    //ddlEngineeringName.DataSource = dtinv;
                    //ddlEngineeringName.DataTextField = "Name";
                    //ddlEngineeringName.DataValueField = "ID";
                    //ddlEngineeringName.DataBind();
                    //ddlEngineeringName.Items.Add(new ListItem("Select P/N", "0"));
                    //ddlEngineeringName.SelectedValue = "0";

                    //ddlFinanceName.DataSource = dtinv;
                    //ddlFinanceName.DataTextField = "Name";
                    //ddlFinanceName.DataValueField = "ID";
                    //ddlFinanceName.DataBind();
                    //ddlFinanceName.Items.Add(new ListItem("Select P/N", "0"));
                    //ddlFinanceName.SelectedValue = "0";

                    //ddlPurchasingName.DataSource = dtinv;
                    //ddlPurchasingName.DataTextField = "Name";
                    //ddlPurchasingName.DataValueField = "ID";
                    //ddlPurchasingName.DataBind();
                    //ddlPurchasingName.Items.Add(new ListItem("Select P/N", "0"));
                    //ddlPurchasingName.SelectedValue = "0";

                    //ddlSalesName.DataSource = dtinv;
                    //ddlSalesName.DataTextField = "Name";
                    //ddlSalesName.DataValueField = "ID";
                    //ddlSalesName.DataBind();
                    //ddlSalesName.Items.Add(new ListItem("Select P/N", "0"));
                    //ddlSalesName.SelectedValue = "0";

                    //ddlInventoryName.DataSource = dtinv;
                    //ddlInventoryName.DataTextField = "Name";
                    //ddlInventoryName.DataValueField = "ID";
                    //ddlInventoryName.DataBind();
                    //ddlInventoryName.Items.Add(new ListItem("Select P/N", "0"));
                    //ddlInventoryName.SelectedValue = "0";



                }
            }
        }
        #endregion

        #region commodity
        Commodity objBL_commodity = new Commodity();
        objBL_commodity.ConnConfig = Session["config"].ToString();

        _ReadAllCommodity.ConnConfig = Session["config"].ToString();

        DataSet dscommodity = new DataSet();
        List<CommodityViewModel> _lstCommodity = new List<CommodityViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "InventoryAPI/InventoryList_ReadAllCommodity";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _ReadAllCommodity);
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstCommodity = serializer.Deserialize<List<CommodityViewModel>>(_APIResponse.ResponseData);
            dscommodity = CommonMethods.ToDataSet<CommodityViewModel>(_lstCommodity);
        }
        else
        {
            dscommodity = objBL_Inventory.ReadAllCommodity(objBL_commodity);
        }

        if (dscommodity != null)
        {
            if (dscommodity.Tables.Count > 0)
            {
                if (dscommodity.Tables[0].Rows.Count > 0)
                {
                    var dtcommodity = from c in dscommodity.Tables[0].Select() select new { Id = c[0], DisplayVal = c[1] + " - " + c[2] };

                    ddlCommodity.DataSource = dtcommodity;
                    ddlCommodity.DataTextField = "DisplayVal";
                    ddlCommodity.DataValueField = "Id";
                    ddlCommodity.DataBind();
                    ddlCommodity.Items.Add(new ListItem("Select Commodity", "0"));
                    ddlCommodity.SelectedValue = "0";
                }
            }
        }
        #endregion




    }

    //protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
    //{
    //    if (e.Item.ItemType.Equals(ListItemType.AlternatingItem) || e.Item.ItemType.Equals(ListItemType.Item))
    //    {
    //        CheckBox cbox = e.Item.FindControl("chkWarehouseName") as CheckBox;
    //        if (cbox != null)
    //        {
    //            if (cbox.Text == "Maintenance Inventory")
    //            {
    //                //cbox.Checked = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "Name"));
    //                cbox.Checked = true;
    //            }
    //           // cbox.Checked = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "discontinued"));
    //        }
    //    }       
    //}

    private void clear()
    {
        txtInventoryMPN.Text = string.Empty;
        txtVendorPartNumber.Text = string.Empty;
        txtInventoryApprovedManufacturer.Text = string.Empty;
        txtManufacturerPrice.Text = string.Empty;
        txtVendorPrice.Text = string.Empty;
        ddlInventoryApprovedVendor.ClearSelection();
        hdninvvendinfo.Value = "";
    }

    private void FillData(string Id)
    {
        objProp_Inventory = new BusinessEntity.Inventory();
        objProp_Inventory = GetInventoryById(Id).ReponseObject;

        asp.Text = objProp_Inventory.Name +" | " + objProp_Inventory.fDesc;
        // ddlUOM.SelectedValue = objProp_Inv ;
        if (objProp_Inventory != null)
        {
            // DataSet dsprchaseinfo = objBL_Inventory.GetItemPurchaseOrder(objProp_Inventory.ID);
            string strWarehouse = string.Empty;
            string strGlsales = string.Empty;
            string strGlcogs = string.Empty;    
            #region ItemHeader
            txtItemHeaderName.Text = objProp_Inventory.Name;
            // ddlHeaderNameName.SelectedValue = objProp_Inventory.ID.ToString();
            txtDes.Text = objProp_Inventory.fDesc;
            ddlUOM.SelectedValue = Convert.ToString(objProp_Inventory.Measure);
            ddlInvStatus.SelectedValue = Convert.ToString(objProp_Inventory.Status);
            ddlCategory.SelectedValue = Convert.ToString(objProp_Inventory.Cat);
            txtRemarks.Text = objProp_Inventory.Remarks;
            DataTable dtMergeWarehouse = new DataTable();
            if (Request.QueryString["t"] == "c" && !string.IsNullOrEmpty(Request.QueryString["t"]))
            {
                int chkStatusChart = 1;

                if (!string.IsNullOrEmpty(objProp_Inventory.GLSales))
                {
                    DataSet ds = new DataSet();
                    List <CheckStatusOfChartViewModel> _lstStatusOfChart = new List<CheckStatusOfChartViewModel>();
                    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "InventoryAPI/AddInventory_chkStatusOfChart";

                        _StatusOfChart.ID = Convert.ToInt32(objProp_Inventory.GLSales);

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _StatusOfChart);
                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        serializer.MaxJsonLength = Int32.MaxValue;

                        _lstStatusOfChart = serializer.Deserialize<List<CheckStatusOfChartViewModel>>(_APIResponse.ResponseData);
                        ds = CommonMethods.ToDataSet<CheckStatusOfChartViewModel>(_lstStatusOfChart);
                    }
                    else
                    {
                        ds = objBL_Inventory.chkStatusOfChart(Convert.ToInt32(objProp_Inventory.GLSales));
                    }

                    
                    if (Convert.ToInt32(ds.Tables[0].Rows[0]["status"]) == 0)
                        ddlglsales.SelectedValue = !string.IsNullOrEmpty(objProp_Inventory.GLSales) ?objProp_Inventory.GLSales : "0";
                    else
                    {
                        strGlsales = ds.Tables[0].Rows[0]["fDesc"].ToString();
                        ddlglsales.SelectedValue = "0";
                    }
                 
                }
                if (!string.IsNullOrEmpty(objProp_Inventory.GLcogs))
                {
                    DataSet ds = new DataSet();
                    List<CheckStatusOfChartViewModel> _lstStatusOfChart = new List<CheckStatusOfChartViewModel>();
                    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "InventoryAPI/AddInventory_chkStatusOfChart";

                        _StatusOfChart.ID = Convert.ToInt32(objProp_Inventory.GLcogs);

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _StatusOfChart);
                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        serializer.MaxJsonLength = Int32.MaxValue;

                        _lstStatusOfChart = serializer.Deserialize<List<CheckStatusOfChartViewModel>>(_APIResponse.ResponseData);
                        ds = CommonMethods.ToDataSet<CheckStatusOfChartViewModel>(_lstStatusOfChart);
                    }
                    else
                    {
                        ds = objBL_Inventory.chkStatusOfChart(Convert.ToInt32(objProp_Inventory.GLcogs));
                    }

                    if (Convert.ToInt32(ds.Tables[0].Rows[0]["status"]) == 0)
                        ddlglcogs.SelectedValue = !string.IsNullOrEmpty(objProp_Inventory.GLcogs) ? objProp_Inventory.GLcogs : "0";
                    else
                    {
                        strGlcogs = ds.Tables[0].Rows[0]["fDesc"].ToString();
                        ddlglcogs.SelectedValue = "0";
                    }
                }
                #region Inventory Invwarehousemerge for Copy Case
                dtMergeWarehouse = new DataTable();
                dtMergeWarehouse.Columns.Add("ID", typeof(int));
            dtMergeWarehouse.Columns.Add("InvID", typeof(int));
            dtMergeWarehouse.Columns.Add("WarehouseID", typeof(string));
            dtMergeWarehouse.Columns.Add("WarehouseName", typeof(string));
            dtMergeWarehouse.Columns.Add("Hand", typeof(decimal));
            dtMergeWarehouse.Columns.Add("Balance", typeof(decimal));
            dtMergeWarehouse.Columns.Add("Committed", typeof(decimal));
            dtMergeWarehouse.Columns.Add("fOrder", typeof(decimal));
            dtMergeWarehouse.Columns.Add("Available", typeof(decimal));
            dtMergeWarehouse.Columns.Add("Company", typeof(string));
            dtMergeWarehouse.AcceptChanges();
                string strWarehouseID = string.Empty;
            if (objProp_Inventory.InvWarehouseMergelist != null)
            {
              
              foreach (InvWarehouse InvWarehouse in objProp_Inventory.InvWarehouseMergelist)
              {
                        int chkWarehouseIsActive = 0;
                        string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

                        if (IsAPIIntegrationEnable == "YES")
                        {
                            string APINAME = "InventoryAPI/AddInventory_CheckWarehouseIsActive";

                            _CheckWarehouseIsActive.WarehouseID = InvWarehouse.WarehouseID;

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _CheckWarehouseIsActive);
                            chkWarehouseIsActive = Convert.ToInt32(_APIResponse.ResponseData);
                        }
                        else
                        {
                            chkWarehouseIsActive = objBL_Inventory.CheckWarehouseIsActive(InvWarehouse.WarehouseID);
                        }


                        if (chkWarehouseIsActive == 0)
                        {
                            DataRow dr = dtMergeWarehouse.NewRow();
                            dr["ID"] = InvWarehouse.ID;
                            dr["InvID"] = InvWarehouse.InvID;

                            if (IsAPIIntegrationEnable == "YES")
                            {
                                dr["WarehouseID"] = _CheckWarehouseIsActive.WarehouseID;
                            }
                            else
                            {
                                dr["WarehouseID"] = InvWarehouse.WarehouseID;
                            }

                            dr["WarehouseName"] = InvWarehouse.WarehouseName;
                            dr["Hand"] = 0;
                            dr["Balance"] = 0;
                            dr["Committed"] = 0;
                            dr["fOrder"] = 0;
                            dr["Available"] = 0;
                            dr["Company"] = 0;
                            dtMergeWarehouse.Rows.Add(dr);
                            dtMergeWarehouse.AcceptChanges();
                        }
                        else
                        {
                            strWarehouse = InvWarehouse.WarehouseName;
                            if (IsAPIIntegrationEnable == "YES")
                            {
                                strWarehouseID = _CheckWarehouseIsActive.WarehouseID;
                            }
                            else
                            {
                                strWarehouseID = InvWarehouse.WarehouseID;
                            }
                        }
              }
            }
           RadGrid_Warehouse.VirtualItemCount = dtMergeWarehouse.Rows.Count;
           RadGrid_Warehouse.DataSource = dtMergeWarehouse;
           RadGrid_Warehouse.DataBind();
           ViewState["InvMergewarehouse"] = dtMergeWarehouse;

                string msg = string.Empty;
                string strWarehousemsg = string.Empty;

                if (strWarehouse != string.Empty && strGlcogs == string.Empty && strGlsales == string.Empty)
                {
                    msg = string.Empty;
                    if (strWarehouseID.ToLower() != "OFC".ToLower())
                    {
                        msg = strWarehouse + " inactive warehouse it can not be added.";
                        strWarehousemsg = msg;
                    }
                }

                if (strWarehouse != string.Empty && strGlcogs == string.Empty && strGlsales != string.Empty)
                {
                    msg = string.Empty;
                    msg = strWarehouse + " inactive warehouse it can not be added." + " And " + strGlcogs + " transaction cannot be saved because the Acct no. is inactive ";
                }

                if (strWarehouse != string.Empty && strGlcogs != string.Empty && strGlsales == string.Empty)
                {
                    msg = string.Empty;
                    msg = strWarehouse + " inactive warehouse it can not be added." + " And " + strGlsales + " transaction cannot be saved because the Acct no. is inactive ";
                }


                if (strWarehouse == string.Empty && strGlcogs != string.Empty && strGlsales == string.Empty)
                {
                    msg = string.Empty;
                    msg =  strGlcogs + " transaction cannot be saved because the Acct no. is inactive ";
                }

                if (strWarehouse == string.Empty && strGlcogs != string.Empty && strGlsales != string.Empty)
                {
                    msg = string.Empty;
                    msg = strGlcogs + " And " + strGlsales + " transaction cannot be saved because the Acct no. is inactive ";
                }

                if (strWarehouse != string.Empty && strGlcogs != string.Empty && strGlsales == string.Empty)
                {
                    msg = string.Empty;
                    msg = strWarehouse + " inactive warehouse it can not be added." + " And " + strGlcogs + " transaction cannot be saved because the Acct no. is inactive ";
                }


                if (strWarehouse == string.Empty && strGlcogs == string.Empty && strGlsales != string.Empty)
                {
                    msg = string.Empty;
                    msg = strGlsales + " transaction cannot be saved because the Acct no. is inactive ";
                }

                if (strWarehouse == string.Empty && strGlcogs != string.Empty && strGlsales != string.Empty)
                {
                    msg = string.Empty;
                    msg = strGlsales + " And " + strGlsales + " transaction cannot be saved because the Acct no. is inactive ";
                }

                if (strWarehouse != string.Empty && strGlcogs == string.Empty && strGlsales != string.Empty)
                {
                    msg = string.Empty;
                    msg = strWarehouse + " inactive warehouse it can not be added." + " And " + strGlsales + " transaction cannot be saved because the Acct no. is inactive ";
                }



                if (strWarehouse != string.Empty && strGlcogs != string.Empty && strGlsales != string.Empty)
                {
                    msg = string.Empty;
                    msg = strWarehouse + " inactive warehouse it can not be added." + " And " + strGlcogs + " And " + strGlsales + " transaction cannot be saved because the Acct no. is inactive ";
                }

                if (strWarehouse == string.Empty && strGlcogs == string.Empty && strGlsales == string.Empty)
                    msg = string.Empty;                    
                


                if (msg != string.Empty)
                   ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningRef", "noty({text: '" + msg + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

            }
            #endregion Inventory Invwarehousemerge for Copy Case

            if (Request.QueryString["t"] != "c")
            {
                // ddlUOM.SelectedValue = objProp_Inventory.Measure;

                //txtDes2.Text = objProp_Inventory.Description2;
                //txtDes3.Text = objProp_Inventory.Description3;
                //txtDes4.Text = objProp_Inventory.Description4;


                ddlglsales.SelectedValue = !string.IsNullOrEmpty(objProp_Inventory.GLSales) ? objProp_Inventory.GLSales : "0";
                ddlglcogs.SelectedValue = !string.IsNullOrEmpty(objProp_Inventory.GLcogs) ? objProp_Inventory.GLcogs : "0";
                txtDateCreated.Text = Convert.ToString(objProp_Inventory.DateCreated);


               
               
                #endregion
                #region Eng
                // ddlEngineeringName.SelectedValue = objProp_Inventory.ID.ToString();
                // txtEngineeringDescription.Text = objProp_Inventory.fDesc;
                txtSpecification.Text = objProp_Inventory.Specification;
                //txtSpecification2.Text = objProp_Inventory.Specification2;
                //txtSpecification3.Text = objProp_Inventory.Specification3;
                //txtSpecification4.Text = objProp_Inventory.Specification4;
                //txtRevision.Text = objProp_Inventory.Revision;
                //txtRevisionDate.Text = Convert.ToString(objProp_Inventory.LastRevisionDate);
                //txtECO.Text = objProp_Inventory.Eco;
                //txtDrawing.Text = objProp_Inventory.Drawing;
                //txtReference.Text = objProp_Inventory.Reference;
                //txtLength.Text = objProp_Inventory.Length;
                //txtWidth.Text = objProp_Inventory.Width;
                //txtHeight.Text = objProp_Inventory.Height;
                //txtWeight.Text = objProp_Inventory.Weight;
                txtshelflife.Text = Convert.ToString(objProp_Inventory.ShelfLife);
                //chkInspRequired.Checked = objProp_Inventory.InspectionRequired;
                //chkCoCpRequired.Checked = objProp_Inventory.CoCRequired;
                //chkSerializationRequired.Checked = objProp_Inventory.SerializationRequired;

                #endregion
                #region Finance
                // ddlFinanceName.SelectedValue = objProp_Inventory.ID.ToString();
                //txtFinanceDescription.Text = objProp_Inventory.fDesc;
                //    txtunitCost.Text = Convert.ToString(objProp_Inventory.UnitCost);

                // Commented Due TO ES-3889 As per Laxmi Mam Instruction //
                //txtLastPurchaseCost.Text = Convert.ToString(objProp_Inventory.LCost);
                // Replace to Last unit Price as Per ES-3889 Laxmi Mam Instruction // 
                txtLastUnitCost.Text = Convert.ToString(objProp_Inventory.LCost);
                //
               
                if (objProp_Inventory.LVendor != 0)
                    // ddlLastPurchaseFromVendor.SelectedValue = Convert.ToString(objProp_Inventory.LVendor);
                    //     txtLastPurchaseFrom.Text = "";
                    //Last Purchase date Once confirmed put here
                    txtOHVal.Text = Convert.ToString(objProp_Inventory.OHValue);
                txtOOVal.Text = Convert.ToString(objProp_Inventory.OOValue);
                // txtComittedValue.Text = Convert.ToString(objProp_Inventory.OOValue);
                objProp_Inventory.Committed = Convert.ToDecimal(objProp_Inventory.Committed);
                if (objProp_Inventory.ABCClass != "")
                    ddlABC.SelectedValue = objProp_Inventory.ABCClass;
                ckhTaxable.Checked = Convert.ToBoolean(objProp_Inventory.Tax);
                txtInventoryTurns.Text = Convert.ToString(objProp_Inventory.InventoryTurns);

                #endregion
                #region Purchasing
                //  ddlPurchasingName.SelectedValue = objProp_Inventory.ID.ToString();
                // txtPurchasingDescription.Text = objProp_Inventory.fDesc;
                //objProp_Inventory.nextPOdate = Convert.ToDateTime(txtNextPoDate.Text);
                txtLastPODate.Text = Convert.ToString(objProp_Inventory.DateLastPurchase);


                // Commented Due TO ES-3889 As per Laxmi Mam Instruction //
                //txtLastUnitCost.Text = Convert.ToString(objProp_Inventory.LCost);
                // Replace to Last unit Price as Per ES-3889 Laxmi Mam Instruction // 
                txtLastPurchaseCost.Text = Convert.ToString(objProp_Inventory.LCost);
                //


                //txtLastVendor.Text = ddlLastPurchaseFromVendor.SelectedItem.Text;
                txtLastVendor.Text = "";
                //objProp_Inventory.LVendor = Convert.ToInt32(txtLastVendor.Text);
                //   txtLastReceiptDate.Text = Convert.ToString(objProp_Inventory.LastReceiptDate);
                ddlCommodity.SelectedValue = objProp_Inventory.Commodity != "" ? objProp_Inventory.Commodity : "0";
                txtEAU.Text = Convert.ToString(objProp_Inventory.EAU);
                txtEOLDate.Text = Convert.ToString(objProp_Inventory.EOLDate);
                //txtWeight.Text = objProp_Inventory.Weight;
                //  txtWarrantyPeriod.Text = Convert.ToString(objProp_Inventory.WarrantyPeriod);
                txtLeadTime.Text = Convert.ToString(objProp_Inventory.LeadTime);
                txtMOQ.Text = Convert.ToString(objProp_Inventory.MOQ);
                txtEOQ.Text = Convert.ToString(objProp_Inventory.EOQ);
                #endregion
                #region Inventory
                // ddlInventoryName.SelectedValue = objProp_Inventory.ID.ToString();
                // txtInventoryDescription.Text = objProp_Inventory.fDesc;
                if (objProp_Inventory.Warehouse != "")
                    ddlWareHouse.SelectedValue = objProp_Inventory.Warehouse;
                txtAisle.Text = objProp_Inventory.Aisle;
                txtShelf.Text = objProp_Inventory.Shelf;
                txtBin.Text = objProp_Inventory.Bin;
                txtDateLastUsed.Text = Convert.ToString(objProp_Inventory.LastRevisionDate);
                //objProp_Inventory.DateLastUsed = Convert.ToString(txtDateLastUsed.Text);
                txtshelflife.Text = Convert.ToString(objProp_Inventory.ShelfLife);
                #endregion
                #region Sales
                //  ddlSalesName.SelectedValue = objProp_Inventory.ID.ToString();
                //  txtSalesDescription.Text = objProp_Inventory.fDesc;
                txtPrice1.Text = Convert.ToString(objProp_Inventory.Price1);
                txtPrice2.Text = Convert.ToString(objProp_Inventory.Price2);
                txtPrice3.Text = Convert.ToString(objProp_Inventory.Price3);
                txtPrice4.Text = Convert.ToString(objProp_Inventory.Price4);
                txtPrice5.Text = Convert.ToString(objProp_Inventory.Price5);
                txtPrice6.Text = Convert.ToString(objProp_Inventory.Price6);
                //txtAnnualSalesQuantity.Text = Convert.ToString(objProp_Inventory.AnnualSalesQty);
                //txtAnnualSales.Text = Convert.ToString(objProp_Inventory.AnnualSalesAmt);
                //txtMaxDiscount.Text = Convert.ToString(objProp_Inventory.MaxDiscountPercentage);
                #endregion

                #region Inventory Vendor
                DataTable dt = new DataTable();
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("ItemID", typeof(int));
                dt.Columns.Add("MPN", typeof(string));
                dt.Columns.Add("Part", typeof(string));
                dt.Columns.Add("Supplier", typeof(string));
                dt.Columns.Add("VendorID", typeof(int));
                dt.Columns.Add("Price", typeof(float));
                dt.Columns.Add("Mfg", typeof(string));
                dt.Columns.Add("MfgPrice", typeof(float));
                dt.AcceptChanges();



                List<InvParts> lstvendorinfo = new List<InvParts>();
                if (objProp_Inventory.InvPartslist != null)
                {
                    foreach (InvParts invman in objProp_Inventory.InvPartslist)
                    {
                        DataRow dr = dt.NewRow();
                        dr["ID"] = invman.ID;
                        dr["ItemID"] = invman.ItemID;
                        dr["Supplier"] = invman.Supplier;
                        dr["VendorID"] = invman.VendorID;
                        dr["Price"] = invman.Price;
                        dr["Mfg"] = invman.Mfg;
                        dr["MfgPrice"] = invman.MfgPrice;
                        dr["MPN"] = invman.MPN;
                        dr["Part"] = invman.Part;

                        dt.Rows.Add(dr);
                        dt.AcceptChanges();

                    }
                }
                RadGrid_Vendors.VirtualItemCount = dt.Rows.Count;
                RadGrid_Vendors.DataSource = dt;
                RadGrid_Vendors.DataBind();

                ViewState["POVendors"] = dt;

                #endregion

                #region Inventory ItemRev
                DataTable dt1 = new DataTable();
                dt1.Columns.Add("ID", typeof(int));
                dt1.Columns.Add("InvID", typeof(int));
                dt1.Columns.Add("Date", typeof(DateTime));
                dt1.Columns.Add("Comment", typeof(string));
                dt1.Columns.Add("Version", typeof(string));
                dt1.Columns.Add("Eco", typeof(string));
                dt1.Columns.Add("Drawing", typeof(string));

                dt1.AcceptChanges();



                List<InvItemRev> lstInvItemRevinfo = new List<InvItemRev>();
                if (objProp_Inventory.InvItemRevlist != null)
                {
                    foreach (InvItemRev invItemRev in objProp_Inventory.InvItemRevlist)
                    {
                        DataRow dr = dt1.NewRow();
                        dr["ID"] = invItemRev.ID;
                        dr["InvID"] = invItemRev.InvID;
                        dr["Date"] = invItemRev.Date;
                        dr["Version"] = invItemRev.Version;
                        dr["Comment"] = invItemRev.Comment;
                        dr["Eco"] = invItemRev.Eco;
                        dr["Drawing"] = invItemRev.Drawing;

                        dt1.Rows.Add(dr);
                        dt1.AcceptChanges();

                    }
                }
                RadGrid_Revision.DataSource = dt1;
                RadGrid_Revision.DataBind();

                ViewState["POItemRevision"] = dt1;

                #endregion

                #region Inventory Invwarehousemerge
                dtMergeWarehouse = new DataTable();
                dtMergeWarehouse.Columns.Add("ID", typeof(int));
                dtMergeWarehouse.Columns.Add("InvID", typeof(int));
                dtMergeWarehouse.Columns.Add("WarehouseID", typeof(string));
                dtMergeWarehouse.Columns.Add("WarehouseName", typeof(string));
                dtMergeWarehouse.Columns.Add("Hand", typeof(decimal));
                dtMergeWarehouse.Columns.Add("Balance", typeof(decimal));
                dtMergeWarehouse.Columns.Add("Committed", typeof(decimal));
                dtMergeWarehouse.Columns.Add("fOrder", typeof(decimal));
                dtMergeWarehouse.Columns.Add("Available", typeof(decimal));
                dtMergeWarehouse.Columns.Add("Company", typeof(string));

                dtMergeWarehouse.AcceptChanges();
                //bool isFromCopy = false;
                //if ((!string.IsNullOrEmpty(invID) && Request.QueryString["t"] == "c"))
                //{
                //    isFromCopy = true;
                //}
                //if (isFromCopy)
                //{
                //    if (objProp_Inventory.InvWarehouseMergelist != null)
                //    {
                //        foreach (InvWarehouse InvWarehouse in objProp_Inventory.InvWarehouseMergelist)
                //        {
                //            DataRow dr = dtMergeWarehouse.NewRow();
                //            dr["ID"] = InvWarehouse.ID;
                //            dr["InvID"] = InvWarehouse.InvID;
                //            dr["WarehouseID"] = InvWarehouse.WarehouseID;
                //            dr["WarehouseName"] = InvWarehouse.WarehouseName;
                //            dr["Hand"] = 0;
                //            dr["Balance"] = 0;
                //            dr["Committed"] = 0;
                //            dr["fOrder"] = 0;
                //            dr["Available"] = 0;
                //            dr["Company"] = 0;
                //            dtMergeWarehouse.Rows.Add(dr);
                //            dtMergeWarehouse.AcceptChanges();
                //        }
                //    }
                //    RadGrid_Warehouse.VirtualItemCount = dtMergeWarehouse.Rows.Count;
                //    RadGrid_Warehouse.DataSource = dtMergeWarehouse;
                //    RadGrid_Warehouse.DataBind();
                //    ViewState["InvMergewarehouse"] = dtMergeWarehouse;
                //}
                //else
                //{
                    List<InvWarehouse> lstInvWarehouseinfo = new List<InvWarehouse>();
                    if (objProp_Inventory.InvWarehouseMergelist != null)
                    {
                        foreach (InvWarehouse InvWarehouse in objProp_Inventory.InvWarehouseMergelist)
                        {
                            DataRow dr = dtMergeWarehouse.NewRow();
                            dr["ID"] = InvWarehouse.ID;
                            dr["InvID"] = InvWarehouse.InvID;
                            dr["WarehouseID"] = InvWarehouse.WarehouseID;
                            dr["WarehouseName"] = InvWarehouse.WarehouseName;
                            dr["Hand"] = InvWarehouse.Hand;
                            dr["Balance"] = InvWarehouse.Balance;
                            dr["Committed"] = InvWarehouse.Committed;
                            dr["fOrder"] = InvWarehouse.fOrder;
                            dr["Available"] = InvWarehouse.Available;
                            dr["Company"] = InvWarehouse.Company;
                            dtMergeWarehouse.Rows.Add(dr);
                            dtMergeWarehouse.AcceptChanges();
                        }
                    }
                    RadGrid_Warehouse.VirtualItemCount = dtMergeWarehouse.Rows.Count;
                    RadGrid_Warehouse.DataSource = dtMergeWarehouse;
                    RadGrid_Warehouse.DataBind();
                    ViewState["InvMergewarehouse"] = dtMergeWarehouse;


                
               // }


                #endregion

                GetDocuments();

                #region PO Info
                DataSet dsprchaseinfo = new DataSet();
                List<GetItemPurchaseOrderByInvIDViewModel> _lstItemPurchaseOrder = new List<GetItemPurchaseOrderByInvIDViewModel>();
                string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "InventoryAPI/AddInventory_GetItemPurchaseOrderByInvID";

                    _GetItemPurchaseOrderByInvID.ID = objProp_Inventory.ID;

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetItemPurchaseOrderByInvID);
                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstItemPurchaseOrder = serializer.Deserialize<List<GetItemPurchaseOrderByInvIDViewModel>>(_APIResponse.ResponseData);
                    dsprchaseinfo = CommonMethods.ToDataSet<GetItemPurchaseOrderByInvIDViewModel>(_lstItemPurchaseOrder);
                }
                else
                {
                    dsprchaseinfo = objBL_Inventory.GetItemPurchaseOrderByInvID(objProp_Inventory.ID);
                }


                if (dsprchaseinfo.Tables[0].Rows.Count > 0)
                {

                    txtNextPoDate.Text = dsprchaseinfo.Tables[0].Rows[0]["NextPODate"] != DBNull.Value ? (Convert.ToDateTime(dsprchaseinfo.Tables[0].Rows[0]["NextPODate"])).ToString("MM/dd/yyyy") : "";
                    txtLastVendor.Text = dsprchaseinfo.Tables[0].Rows[0]["VendorName"] != DBNull.Value ? Convert.ToString(dsprchaseinfo.Tables[0].Rows[0]["VendorName"]) : "";
                    txtLastPODate.Text = dsprchaseinfo.Tables[0].Rows[0]["LastPurchaseDate"] != DBNull.Value ? (Convert.ToDateTime(dsprchaseinfo.Tables[0].Rows[0]["LastPurchaseDate"])).ToString("MM/dd/yyyy") : "";


                    // Commented Due TO ES-3889 As per Laxmi Mam Instruction //
                    //txtLastPurchaseCost.Text = dsprchaseinfo.Tables[0].Rows[0]["LastPurchasePrice"] != DBNull.Value ? Convert.ToString(dsprchaseinfo.Tables[0].Rows[0]["LastPurchasePrice"]) : "";
                    // Replace to Last unit Price as Per ES-3889 Laxmi Mam Instruction // 
                    txtLastUnitCost.Text = dsprchaseinfo.Tables[0].Rows[0]["LastPurchasePrice"] != DBNull.Value ? Convert.ToString(dsprchaseinfo.Tables[0].Rows[0]["LastPurchasePrice"]) : "";
                    //

                    txtLastReceiptDate.Text = dsprchaseinfo.Tables[0].Rows[0]["LastReceiptDate"] != DBNull.Value ? (Convert.ToDateTime(dsprchaseinfo.Tables[0].Rows[0]["LastReceiptDate"])).ToString("MM/dd/yyyy") : "";

                    //    txtLastPurchaseFrom.Text = dsprchaseinfo.Tables[0].Rows[0]["VendorName"] != DBNull.Value ? Convert.ToString(dsprchaseinfo.Tables[0].Rows[0]["VendorName"]) : "";
                    //      txtLastPurchaseDate.Text = dsprchaseinfo.Tables[0].Rows[0]["LastPurchaseDate"] != DBNull.Value ? (Convert.ToDateTime (dsprchaseinfo.Tables[0].Rows[0]["LastPurchaseDate"])).ToString("MM/dd/yyyy") : "";
                }

                //if (dsprchaseinfo != null)
                //{
                //    if (dsprchaseinfo.Tables.Count > 0)
                //    {
                //        if (dsprchaseinfo.Tables[0].Rows.Count > 0)
                //        {
                //            if (dsprchaseinfo.Tables[1].Rows.Count > 0)
                //            {
                //                txtNextPoDate.Text = dsprchaseinfo.Tables[1].Rows[0]["Due"] != DBNull.Value ? Convert.ToString(dsprchaseinfo.Tables[1].Rows[0]["Due"]) : "";
                //            }
                //        }
                //    }
                //}
                #endregion


                #region Inventory Stats

                ddlInvStatus.SelectedValue = Convert.ToString(objProp_Inventory.Status);


                objProp_Inventory.UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"].ToString());
                if (System.Web.HttpContext.Current.Session["CmpChkDefault"].ToString() == "1")
                {
                    objProp_Inventory.EN = 1;
                }
                else
                {
                    objProp_Inventory.EN = 0;
                }
                DataSet dsAlliteminfo = new DataSet();
                List <GetAllItemQuantityByInvIDViewModel> _lstAllItemQuantity = new List<GetAllItemQuantityByInvIDViewModel>();
                
                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "InventoryAPI/AddInventory_GetAllItemQuantityByInvID";

                    _GetAllItemQuantityByInvID.ID = objProp_Inventory.ID;
                    _GetAllItemQuantityByInvID.UserID = objProp_Inventory.UserID;
                    _GetAllItemQuantityByInvID.EN = objProp_Inventory.EN;

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetAllItemQuantityByInvID);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;
                    _lstAllItemQuantity = serializer.Deserialize<List<GetAllItemQuantityByInvIDViewModel>>(_APIResponse.ResponseData);
                    dsAlliteminfo = CommonMethods.ToDataSet<GetAllItemQuantityByInvIDViewModel>(_lstAllItemQuantity);
                }
                else
                {
                    dsAlliteminfo = objBL_Inventory.GetAllItemQuantityByInvID(objProp_Inventory.ID, objProp_Inventory.UserID, objProp_Inventory.EN);
                }

                txtOnHnad.Text = dsAlliteminfo.Tables[0].Rows[0]["Hand"] != DBNull.Value ? Convert.ToString(dsAlliteminfo.Tables[0].Rows[0]["Hand"]) : "";
                txtOnOrder.Text = dsAlliteminfo.Tables[0].Rows[0]["forder"] != DBNull.Value ? Convert.ToString(dsAlliteminfo.Tables[0].Rows[0]["forder"]) : "";
                txtOnComitted.Text = dsAlliteminfo.Tables[0].Rows[0]["Committed"] != DBNull.Value ? Convert.ToString(dsAlliteminfo.Tables[0].Rows[0]["Committed"]) : "";
                txtOnAvaliable.Text = dsAlliteminfo.Tables[0].Rows[0]["Available"] != DBNull.Value ? Convert.ToString(dsAlliteminfo.Tables[0].Rows[0]["Available"]) : "";
                txtOOVal.Text = dsAlliteminfo.Tables[0].Rows[0]["OOValue"] != DBNull.Value ? Convert.ToString(dsAlliteminfo.Tables[0].Rows[0]["OOValue"]) : "";
                txtComittedValue.Text = dsAlliteminfo.Tables[0].Rows[0]["CommittedValue"] != DBNull.Value ? Convert.ToString(dsAlliteminfo.Tables[0].Rows[0]["CommittedValue"]) : "";
                txtOHVal.Text = dsAlliteminfo.Tables[0].Rows[0]["OHValue"] != DBNull.Value ? Convert.ToString(dsAlliteminfo.Tables[0].Rows[0]["OHValue"]) : "";
                //   txtIssuedtoOpenjobs.Text= dsAlliteminfo.Tables[0].Rows[0]["IssuesToOpenJobs"] != DBNull.Value ? Convert.ToString(dsAlliteminfo.Tables[0].Rows[0]["IssuesToOpenJobs"]) : "";
                // txtunitCost.Text = dsAlliteminfo.Tables[0].Rows[0]["UnitCost"] != DBNull.Value ? Convert.ToString(dsAlliteminfo.Tables[0].Rows[0]["UnitCost"]) : "";



                //objBL_Inventory = new BL_Inventory();
                //DataSet dsquantity = objBL_Inventory.GetItemQuantity();
                //if (dsquantity != null)
                //{
                //    if (dsquantity.Tables.Count > 0)
                //    {
                //        if (dsquantity.Tables[0].Rows.Count > 0)
                //        {
                //            txtOnHnad.Text = Convert.ToString(dsquantity.Tables[0].Rows[0]["OnHand"]);
                //            txtOnOrder.Text = Convert.ToString(dsquantity.Tables[0].Rows[0]["OnOrder"]);
                //            txtOnComitted.Text = Convert.ToString(dsquantity.Tables[0].Rows[0]["Comitted"]);
                //            txtOnAvaliable.Text = Convert.ToString(dsquantity.Tables[0].Rows[0]["Avaliable"]);
                //            txtIssuedtoOpenjobs.Text = Convert.ToString(dsquantity.Tables[0].Rows[0]["IssuesToOpenJobs"]);
                //        }
                //    }
                //}
                #endregion



                linkquoterequestid.Visible = true;
            }
            
        }
    }


    private void ProcessPostedDrawing(BusinessEntity.Inventory obj_inv)
    {
        //if (!string.IsNullOrEmpty(txtDrawing.Text))
        //{
        // Read the file and convert it to Byte Array
        if (flDrawing.PostedFile != null)
        {
                    
        string filePath = flDrawing.PostedFile.FileName;

        string filename = Path.GetFileName(filePath);

        string ext = Path.GetExtension(filename);

        string contenttype = String.Empty;

        #region Extension Check

        //Set the contenttype based on File Extension
        switch (ext)
        {

            case ".doc":

                contenttype = "application/vnd.ms-word";

                break;

            case ".docx":

                contenttype = "application/vnd.ms-word";

                break;

            case ".xls":

                contenttype = "application/vnd.ms-excel";

                break;

            case ".xlsx":

                contenttype = "application/vnd.ms-excel";

                break;

            case ".jpg":

                contenttype = "image/jpg";

                break;

            case ".png":

                contenttype = "image/png";

                break;

            case ".gif":

                contenttype = "image/gif";

                break;

            case ".pdf":

                contenttype = "application/pdf";

                break;

        }
        #endregion

        if (contenttype != String.Empty)
        {



            Stream fs = flDrawing.PostedFile.InputStream;

            BinaryReader br = new BinaryReader(fs);

            Byte[] bytes = br.ReadBytes((Int32)fs.Length);

            if (bytes != null)
            {

                string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                string savepath = savepathconfig + @"\" + Session["dbname"] + @"\Inventory\In_" + invID + @"\";
                string fullpath = savepath + filename;

                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }
                if (File.Exists(fullpath))
                {
                    GeneralFunctions objGeneralFunctions = new GeneralFunctions();
                    filename = objGeneralFunctions.generateRandomString(4) + "_" + filename;
                    fullpath = savepath + filename;
                }


                flDrawing.SaveAs(fullpath);

                obj_inv.Drawing = fullpath;

            }




        }
    }
    }

    //API
    //private void ProcessPostedDrawing(BusinessEntity.UpdateInventoryParam obj_inv)
    //{
    //    //if (!string.IsNullOrEmpty(txtDrawing.Text))
    //    //{
    //    // Read the file and convert it to Byte Array
    //    if (flDrawing.PostedFile != null)
    //    {

    //        string filePath = flDrawing.PostedFile.FileName;

    //        string filename = Path.GetFileName(filePath);

    //        string ext = Path.GetExtension(filename);

    //        string contenttype = String.Empty;

    //        #region Extension Check

    //        //Set the contenttype based on File Extension
    //        switch (ext)
    //        {

    //            case ".doc":

    //                contenttype = "application/vnd.ms-word";

    //                break;

    //            case ".docx":

    //                contenttype = "application/vnd.ms-word";

    //                break;

    //            case ".xls":

    //                contenttype = "application/vnd.ms-excel";

    //                break;

    //            case ".xlsx":

    //                contenttype = "application/vnd.ms-excel";

    //                break;

    //            case ".jpg":

    //                contenttype = "image/jpg";

    //                break;

    //            case ".png":

    //                contenttype = "image/png";

    //                break;

    //            case ".gif":

    //                contenttype = "image/gif";

    //                break;

    //            case ".pdf":

    //                contenttype = "application/pdf";

    //                break;

    //        }
    //        #endregion

    //        if (contenttype != String.Empty)
    //        {



    //            Stream fs = flDrawing.PostedFile.InputStream;

    //            BinaryReader br = new BinaryReader(fs);

    //            Byte[] bytes = br.ReadBytes((Int32)fs.Length);

    //            if (bytes != null)
    //            {

    //                string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
    //                string savepath = savepathconfig + @"\" + Session["dbname"] + @"\Inventory\In_" + invID + @"\";
    //                string fullpath = savepath + filename;

    //                if (!Directory.Exists(savepath))
    //                {
    //                    Directory.CreateDirectory(savepath);
    //                }
    //                if (File.Exists(fullpath))
    //                {
    //                    GeneralFunctions objGeneralFunctions = new GeneralFunctions();
    //                    filename = objGeneralFunctions.generateRandomString(4) + "_" + filename;
    //                    fullpath = savepath + filename;
    //                }


    //                flDrawing.SaveAs(fullpath);

    //                obj_inv.Drawing = fullpath;

    //            }




    //        }
    //    }
    //}

    private List<InvParts> Getvendors()
    {
        List<InvParts> items = new List<BusinessEntity.InvParts>();

        //if (dtlVendors != null)
        if (RadGrid_Vendors != null)
        {
            //foreach (DataListItem dt in dtlVendors.Items)
            foreach (GridDataItem dt in RadGrid_Vendors.Items)
            {
                InvParts item = new BusinessEntity.InvParts();
                HiddenField id = dt.FindControl("hdnid") as HiddenField;
                Label lblmpn = dt.FindControl("lblmpn") as Label;
                Label lblVPN = dt.FindControl("lblVPN") as Label;

                Label lblVendor = dt.FindControl("lblVendor") as Label;
                Label lblVendorID = dt.FindControl("lblVendorID") as Label;
                Label lblVendorPrice = dt.FindControl("lblVendorPrice") as Label;
                Label lblManufacturerName = dt.FindControl("lblManufacturerName") as Label;
                Label lblMfgPrice = dt.FindControl("lblMfgPrice") as Label;
                if (id.Value != "")
                {
                    item.ID = Convert.ToInt32(id.Value);
                    item.MPN = Convert.ToString(lblmpn.Text);
                    item.Part = Convert.ToString(lblVPN.Text);
                    item.Supplier = Convert.ToString(lblVendor.Text);
                    item.VendorID = Convert.ToInt32(lblVendorID.Text);
                    item.Price = float.Parse(lblVendorPrice.Text);
                    item.Mfg = Convert.ToString(lblManufacturerName.Text);
                    item.MfgPrice = float.Parse(lblMfgPrice.Text);





                    items.Add(item);
                }


            }
        }

        return items;
    }

    private List<InvItemRev> GetRevisiondetail()
    {
        List<InvItemRev> items = new List<BusinessEntity.InvItemRev>();

        if (RadGrid_Revision != null)
        {
            //foreach (GridViewRow di in grdRevisionDetail.Rows)
            foreach (GridDataItem di in RadGrid_Revision.Items)
            {


                InvItemRev item = new BusinessEntity.InvItemRev();

                Label lblId = (Label)di.FindControl("lblId");
                Label lblVersion = (Label)di.FindControl("lblVersion");

                Label lblDate = (Label)di.FindControl("lblDate");
                Label lblComment = (Label)di.FindControl("lblComment");
                Label lblEco = (Label)di.FindControl("lblEco");
                Label lblDrawing = (Label)di.FindControl("lblDrawing");


                if (lblId.Text != "")
                {
                    item.ID = Convert.ToInt32(lblId.Text);
                    item.Version = Convert.ToString(lblVersion.Text);
                    item.Date = Convert.ToDateTime(lblDate.Text);
                    item.Comment = Convert.ToString(lblComment.Text);
                    item.Eco = Convert.ToString(lblEco.Text);
                    item.Drawing = Convert.ToString(lblDrawing.Text);






                    items.Add(item);
                }


            }
        }

        return items;
    }
    #endregion

    #region ::WebMethods::
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static WebMethodResponse<BusinessEntity.Inventory> GetInventoryById(string EditID)
    {
        WebMethodResponse<BusinessEntity.Inventory> jsonInventoryInformation = new BusinessEntity.WebMethodResponse<BusinessEntity.Inventory>();
        jsonInventoryInformation.Header = new BusinessEntity.WebMethodHeader();


        BL_Inventory objBL_JsonInventory = new BL_Inventory();
        BusinessEntity.Inventory objProp_JsonInventory = new BusinessEntity.Inventory();
        objProp_JsonInventory.ID = Convert.ToInt32(EditID);
        objProp_JsonInventory.UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"].ToString());
        if (System.Web.HttpContext.Current.Session["CmpChkDefault"].ToString() == "1")
        {
            objProp_JsonInventory.EN = 1;
        }
        else
        {
            objProp_JsonInventory.EN = 0;
        }


        try
        {
            objProp_JsonInventory = objBL_JsonInventory.GetInventoryByID(objProp_JsonInventory);



            if (objProp_JsonInventory != null)
            {


                jsonInventoryInformation.Header.HasError = false;
                jsonInventoryInformation.ReponseObject = objProp_JsonInventory;
            }
            else
            {
                jsonInventoryInformation.Header.HasError = true;

            }
        }
        catch (Exception ex)
        {
            string errormsg = ex.Message;
            List<string> strmsg = new List<string>();
            strmsg.Add(errormsg);

            jsonInventoryInformation.Header.HasError = true;
            jsonInventoryInformation.Header.ErrorMessages = strmsg;
        }
        return jsonInventoryInformation;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static WebMethodResponse<RequestQuoteVendorJSON> GetApprovedVendorInfo(string strinvID, string Vendor)
    {
        WebMethodResponse<RequestQuoteVendorJSON> jsonInventoryInformation = new BusinessEntity.WebMethodResponse<RequestQuoteVendorJSON>();
        jsonInventoryInformation.Header = new BusinessEntity.WebMethodHeader();


        BL_Inventory objBL_JsonInventory = new BL_Inventory();

        int intinvID, vendorid = 0;
        Int32.TryParse(strinvID, out intinvID);
        Int32.TryParse(Vendor, out vendorid);



        RequestQuoteVendorJSON jsonitem = new RequestQuoteVendorJSON();

        try
        {
            List<InventoryViewModel> _lstInventory = new List<InventoryViewModel>();
            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "InventoryAPI/AddInventory_GetInvManufacturerInfoByInvAndVendorId";
                GetInvManufacturerInfoByInvAndVendorIdParam _GetInvManufacturerInfoByInvAndVendorId = new GetInvManufacturerInfoByInvAndVendorIdParam();

                _GetInvManufacturerInfoByInvAndVendorId.InventoryID = intinvID;
                _GetInvManufacturerInfoByInvAndVendorId._ApprovedVendorId = vendorid;

                 APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvManufacturerInfoByInvAndVendorId);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstInventory = serializer.Deserialize<List<InventoryViewModel>>(_APIResponse.ResponseData);
                DataSet ds = CommonMethods.ToDataSet<InventoryViewModel>(_lstInventory);
                jsonitem = JSONMappingUtility.RequestQuoteVendorMappingJSON(ds);
            }
            else
            {
                jsonitem = JSONMappingUtility.RequestQuoteVendorMappingJSON(objBL_JsonInventory.GetInvManufacturerInfoByInvAndVendorId(intinvID, vendorid));
            }

            jsonInventoryInformation.Header.HasError = false;
            jsonInventoryInformation.ReponseObject = jsonitem;

        }
        catch (Exception ex)
        {
            string errormsg = ex.Message;
            List<string> strmsg = new List<string>();
            strmsg.Add(errormsg);

            jsonInventoryInformation.Header.HasError = true;
            jsonInventoryInformation.Header.ErrorMessages = strmsg;
        }
        return jsonInventoryInformation;
    }




    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static WebMethodResponse<string> SendMail(string ToEmail, string Quantity, string body)
    {
        WebMethodResponse<string> jsonInventoryInformation = new BusinessEntity.WebMethodResponse<string>();
        jsonInventoryInformation.Header = new BusinessEntity.WebMethodHeader();
        string response = string.Empty;


        string senderID = "rasmi.das11@gmail.com";// use sender’s email id here..
        const string senderPassword = "asspd1600m"; // sender password here…

        try
        {
            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com", // smtp server address here…
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                Timeout = 30000,
                UseDefaultCredentials = false,


            };
            MailMessage message = new MailMessage(senderID, "rasmi.das11@gmail.com", "hiii", "opopoiopio");
            smtp.Send(message);

            response = "Quote sent successfully!";
            jsonInventoryInformation.Header.HasError = false;
            jsonInventoryInformation.ReponseObject = response;

        }
        catch (Exception ex)
        {



            response = "Quote could not be sent!";
            jsonInventoryInformation.Header.HasError = true;
            jsonInventoryInformation.ReponseObject = response;

        }
        return jsonInventoryInformation;
    }
    #endregion




    private void Permission()
    {
        try
        {
            if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
            {
                DataTable ds = new DataTable();
                ds = (DataTable)Session["userinfo"];


                #region Inventory edit button permission
                string InventoryItemPermission = ds.Rows[0]["Item"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Item"].ToString();
                String addInventory = InventoryItemPermission.Length < 1 ? "Y" : InventoryItemPermission.Substring(0, 1);
                String editInventory = InventoryItemPermission.Length < 2 ? "Y" : InventoryItemPermission.Substring(1, 1);
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

                //Document
                string DocumentPermission = ds.Rows[0]["DocumentPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["DocumentPermission"].ToString();
                hdnAddeDocument.Value = DocumentPermission.Length < 1 ? "Y" : DocumentPermission.Substring(0, 1);
                hdnEditeDocument.Value = DocumentPermission.Length < 2 ? "Y" : DocumentPermission.Substring(1, 1);
                hdnDeleteDocument.Value = DocumentPermission.Length < 3 ? "Y" : DocumentPermission.Substring(2, 1);
                hdnViewDocument.Value = DocumentPermission.Length < 4 ? "Y" : DocumentPermission.Substring(3, 1);

                pnlDocPermission.Visible = hdnViewDocument.Value == "N" ? false : true;

            }
        }
        catch (Exception)
        {
            btnSubmit.Visible = false;
        }
    }

    protected void lnkAddRevisionDetail_Click(object sender, EventArgs e)
    {
        string script = "function f(){$find(\"" + RadWindowRev.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }

    protected void lnkSaveRevisionDetail_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["uid"] != null && Convert.ToString(Request.QueryString["uid"]) != "")
        {
            //Save Code

            // Grid Bind Code

            InvItemRev objInvItemRev = new InvItemRev();
            objInvItemRev.InvID = Convert.ToInt32(Request.QueryString["uid"]);
            objInvItemRev.Date = Convert.ToDateTime(txtRevisionItemRevDate.Text);
            objInvItemRev.Version = txtVersion.Text;
            objInvItemRev.Comment = txtComment.Text;
            objInvItemRev.Eco = txtEco.Text;
            objInvItemRev.Drawing = txtDrawing.Text;

            _CreateItemRevision.InvID = Convert.ToInt32(Request.QueryString["uid"]);
            _CreateItemRevision.Date = Convert.ToDateTime(txtRevisionItemRevDate.Text);
            _CreateItemRevision.Version = txtVersion.Text;
            _CreateItemRevision.Comment = txtComment.Text;
            _CreateItemRevision.Eco = txtEco.Text;
            _CreateItemRevision.Drawing = txtDrawing.Text;


            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "InventoryAPI/AddInventory_CreateItemRevision";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _CreateItemRevision);
            }
            else
            {
                objBL_Inventory.CreateItemRevision(objInvItemRev);
            }

            objProp_Inventory = GetInventoryById(Request.QueryString["uid"]).ReponseObject;

            #region Inventory ItemRev
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("ID", typeof(int));
            dt1.Columns.Add("InvID", typeof(int));
            dt1.Columns.Add("Date", typeof(DateTime));
            dt1.Columns.Add("Comment", typeof(string));
            dt1.Columns.Add("Version", typeof(string));
            dt1.Columns.Add("Eco", typeof(string));
            dt1.Columns.Add("Drawing", typeof(string));

            dt1.AcceptChanges();



            List<InvItemRev> lstInvItemRevinfo = new List<InvItemRev>();
            if (objProp_Inventory.InvItemRevlist != null)
            {
                foreach (InvItemRev invItemRev in objProp_Inventory.InvItemRevlist)
                {
                    DataRow dr = dt1.NewRow();
                    dr["ID"] = invItemRev.ID;
                    dr["InvID"] = invItemRev.InvID;
                    dr["Date"] = invItemRev.Date;
                    dr["Version"] = invItemRev.Version;
                    dr["Comment"] = invItemRev.Comment;
                    dr["Eco"] = invItemRev.Eco;
                    dr["Drawing"] = invItemRev.Drawing;


                    dt1.Rows.Add(dr);
                    dt1.AcceptChanges();

                }
            }
            RadGrid_Revision.VirtualItemCount = dt1.Rows.Count;
      
            RadGrid_Revision.DataSource = dt1;
            RadGrid_Revision.DataBind();

            ViewState["POItemRevision"] = dt1;

            #endregion


        }
        else
        {
            DataTable dt = (DataTable)ViewState["POItemRevision"];

            if (RadGrid_Revision.Items.Count > 0)
            {
                if (hdninvvendinfo.Value != "")
                {
                    int index = 0;
                    foreach (GridDataItem di in RadGrid_Revision.Items)
                    {


                        InvItemRev item = new BusinessEntity.InvItemRev();
                        CheckBox chkSelect = (CheckBox)di.FindControl("chkSelect");
                        Label lblId = (Label)di.FindControl("lblId");
                        Label lblVersion = (Label)di.FindControl("lblVersion");

                        Label lblDate = (Label)di.FindControl("lblDate");
                        Label lblComment = (Label)di.FindControl("lblComment");
                        Label lblEco = (Label)di.FindControl("lblEco");
                        Label lblDrawing = (Label)di.FindControl("lblDrawing");



                        if (chkSelect.Checked)
                        {
                            lblId.Text = HiddenRevisionID.Value;
                            lblDate.Text = txtRevisionItemRevDate.Text;
                            lblVersion.Text = txtVersion.Text;
                            lblComment.Text = txtComment.Text;
                            lblEco.Text = txtEco.Text;
                            lblDrawing.Text = txtDrawing.Text;

                            if (dt != null)
                            {
                                if (dt.Rows.Count > 0)
                                {


                                    //for (int i = 0; i < dt.Rows.Count; i++)
                                    //{

                                    if (lblId.Text == Convert.ToString(dt.Rows[index]["ID"]))
                                    {
                                        // dt.Rows[i]["ID"] = hdninvvendinfo.Value != "" ? Convert.ToInt32(hdninvvendinfo.Value) : 0;
                                        dt.Rows[index]["InvID"] = invID != "" ? Convert.ToInt32(invID) : 0;
                                        dt.Rows[index]["Date"] = (txtRevisionItemRevDate.Text);
                                        dt.Rows[index]["Version"] = (txtVersion.Text);
                                        dt.Rows[index]["Comment"] = (txtComment.Text);
                                        dt.Rows[index]["Eco"] = (txtEco.Text);
                                        dt.Rows[index]["Drawing"] = (txtDrawing.Text);

                                    }
                                    // }
                                }
                            }

                            ViewState["POItemRevision"] = dt;
                        }

                        index++;


                    }
                }
                else
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = HiddenRevisionID.Value != "" ? Convert.ToInt32(HiddenRevisionID.Value) : 0;
                    dr["invID"] = invID != "" ? Convert.ToInt32(invID) : 0;
                    dr["Date"] = (txtRevisionItemRevDate.Text);
                    dr["Version"] = (txtVersion.Text);
                    dr["Comment"] = (txtComment.Text);
                    dr["Eco"] = (txtEco.Text);
                    dr["Drawing"] = (txtDrawing.Text);

                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                    RadGrid_Revision.VirtualItemCount = dt.Rows.Count;
                    RadGrid_Revision.DataSource = dt;
                    RadGrid_Revision.DataBind();
                    ViewState["POItemRevision"] = dt;


                }
            }
            else
            {
                dt = new DataTable();
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("InvID", typeof(int));
                dt.Columns.Add("Date", typeof(DateTime));
                dt.Columns.Add("Version", typeof(string));
                dt.Columns.Add("Comment", typeof(string));
                dt.Columns.Add("Eco", typeof(string));
                dt.Columns.Add("Drawing", typeof(string));

                dt.AcceptChanges();

                DataRow dr = dt.NewRow();
                dr["ID"] = HiddenRevisionID.Value != "" ? Convert.ToInt32(HiddenRevisionID.Value) : 0;
                dr["invID"] = invID != "" ? Convert.ToInt32(invID) : 0;
                dr["Date"] = (txtRevisionItemRevDate.Text);
                dr["Version"] = (txtVersion.Text);
                dr["Comment"] = (txtComment.Text);
                dr["Eco"] = (txtEco.Text);
                dr["Drawing"] = (txtDrawing.Text);

                dt.Rows.Add(dr);
                dt.AcceptChanges();
                RadGrid_Revision.VirtualItemCount = dt.Rows.Count;
                RadGrid_Revision.DataSource = dt;
                RadGrid_Revision.DataBind();



                ViewState["POItemRevision"] = dt;
            }

            //this.ModalpopupFormRevision.Hide();
            //   pnlInventoryWarehouse.Visible = false;


            //      ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "modalclose();", true);

            clear();
        }

    }

    protected void btnAddMergeWarehouse_Click(object sender, EventArgs e)
    {
        string script = "function f(){$find(\"" + RadWindowWarehouse.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }

    protected void btneditMergeWarehouse_Click(object sender, EventArgs e)
    {

    }

    protected void btndelMergewarehouse_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)ViewState["InvMergewarehouse"];
        if (Request.QueryString["uid"] != null)
        {            
            InvWarehouse objInvWarehouse = new InvWarehouse();
            foreach (GridDataItem dataItem in RadGrid_Warehouse.Items)
            {
                Label lblWarehouseID = (Label)dataItem.FindControl("lblWarehouseID");
                if ((dataItem["ClientSelectColumn"].Controls[0] as CheckBox).Checked)
                {
                    if (lblWarehouseID.Text.ToLower() == "OFC".ToLower())
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningRef", "noty({text: 'This is a default warehouse OFC and cannot be deleted.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        break;
                    }
                    else
                    {
                        //objInvWarehouse.InvID = Convert.ToInt32(invID);
                        //objInvWarehouse.WarehouseID = lblWarehouseID.Text;
                        //objBL_Inventory.DeleteInvMergeWarehouse(objInvWarehouse.InvID, objInvWarehouse.WarehouseID);
                        //foreach (DataRow row in dt.Rows)
                        //{
                        //    if (row["WarehouseID"].ToString().Trim().Contains(lblWarehouseID.Text))
                        //    {
                        //        dt.Rows.Remove(row);
                        //        dt.AcceptChanges();
                        //        break;
                        //    }
                        //}
                        objProp_User.AccountID = Convert.ToInt32(invID);
                        objProp_User.WarehouseID = lblWarehouseID.Text; ;
                        objProp_User.ConnConfig = Session["config"].ToString();

                        _DeleteInventoryWareHouse.AccountID = Convert.ToInt32(invID);
                        _DeleteInventoryWareHouse.WarehouseID = lblWarehouseID.Text; ;
                        _DeleteInventoryWareHouse.ConnConfig = Session["config"].ToString();
                        try
                        {
                            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

                            if (IsAPIIntegrationEnable == "YES")
                            {
                                string APINAME = "InventoryAPI/AddInventory_DeleteInventoryWareHouse";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _DeleteInventoryWareHouse);
                            }
                            else
                            {

                                objBL_User.DeleteInventoryWareHouse(objProp_User);
                            }

                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccWarehouse", "noty({text: 'Warehouse deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                            foreach (DataRow row in dt.Rows)
                            {
                                if (row["WarehouseID"].ToString().Trim().Contains(lblWarehouseID.Text))
                                {
                                    dt.Rows.Remove(row);
                                    dt.AcceptChanges();
                                    break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        }




                    }
                }         
            }
            RadGrid_Warehouse.DataSource = dt;
            RadGrid_Warehouse.DataBind();
            ViewState["InvMergewarehouse"] = dt;
        }
        else
        {
            foreach (GridDataItem dataItem in RadGrid_Warehouse.Items)
            {
                Label lblWarehouseID = (Label)dataItem.FindControl("lblWarehouseID");
                if ((dataItem["ClientSelectColumn"].Controls[0] as CheckBox).Checked)
                {
                    if (lblWarehouseID.Text.ToLower() == "OFC".ToLower())
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningRef", "noty({text: 'OFC is default warehoue.it can not be deleted.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        break;
                    }
                    else
                    {                      
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["WarehouseID"].ToString().Trim().Contains(lblWarehouseID.Text))
                            {
                                dt.Rows.Remove(row);
                                dt.AcceptChanges();
                                break;
                            }
                        }
                    }
                }
            }
            RadGrid_Warehouse.DataSource = dt;
            RadGrid_Warehouse.DataBind();
            ViewState["InvMergewarehouse"] = dt;            
        }       
    }

    protected void lnkCloseInvMergeWarehouse_Click(object sender, EventArgs e)
    {

    }

    protected void lnkSaveInvMergeWarehouse_Click(object sender, EventArgs e)
    {
      
            DataTable dt = (DataTable)ViewState["InvMergewarehouse"];
            InvWarehouse objInvWarehouse = new InvWarehouse();
            string strContainsWarehouse = string.Empty;
            foreach (RepeaterItem item in rptWarehouse.Items)
            {
                //to get the dropdown of each line
                CheckBox chkWarehouseName = (CheckBox)item.FindControl("chkWarehouseName");
                //  HiddenField hdnWarehouseID = (HiddenField)item.FindControl("hdnWarehouseID");
                Label lblWarehouseID = (Label)item.FindControl("lblWarehouseID");
                //to get the selected value of your dropdownlist
                Boolean value = chkWarehouseName.Checked;
                if (value == true)
                {
                    bool contains = dt.AsEnumerable().Any(row => chkWarehouseName.Text == row.Field<String>("WarehouseName"));
                    if (!contains)
                    {
                        //try
                        //{
                        objInvWarehouse.WarehouseID = lblWarehouseID.Text;
                        DataRow dr = dt.NewRow();
                        dr["ID"] = 0;
                        dr["InvID"] = 0;
                        dr["WarehouseID"] = objInvWarehouse.WarehouseID;
                        dr["WarehouseName"] = chkWarehouseName.Text;
                        dr["Hand"] = "0";
                        dr["Balance"] = "0";
                        dr["fOrder"] = "0";
                        dr["Available"] = "0";
                        dt.Rows.Add(dr);
                        dt.AcceptChanges();                       
                    }
                    else
                    {
                        strContainsWarehouse += "," + lblWarehouseID.Text;                       
                    }
                }
               
            }
            
                    RadGrid_Warehouse.VirtualItemCount = dt.Rows.Count;
                    RadGrid_Warehouse.DataSource = dt;
                    RadGrid_Warehouse.DataBind();
                    ViewState["InvMergewarehouse"] = dt;
            if (strContainsWarehouse != string.Empty)
            {

                if (strContainsWarehouse.StartsWith(","))
                    strContainsWarehouse = strContainsWarehouse.Substring(1);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningRef", "noty({text: '"+ strContainsWarehouse + "is already exist!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }                        
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "modalclose();", true);
            clear();        
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
       if(!string.IsNullOrEmpty(Request.QueryString["uid"].ToString()))
          GetInvTransactions();
    }

    private void GetInvTransactions(bool flag =false)
    {
        #region Inventory Transactions

        DataSet dSInvTrasaction = new DataSet();
        if(!string.IsNullOrEmpty(Request.QueryString["uid"]))
        {
            objProp_Inventory = new BusinessEntity.Inventory();
            objProp_Inventory.ID = Convert.ToInt32(Request.QueryString["uid"].ToString());

            _GetInventoryTransactionByInvID = new BusinessEntity.GetInventoryTransactionByInvIDParam();
            _GetInventoryTransactionByInvID.ID = Convert.ToInt32(Request.QueryString["uid"].ToString());

            if (txtInvDtFrom.Text != string.Empty)
            {
                objProp_Inventory.FromDate = Convert.ToDateTime(txtInvDtFrom.Text);
                _GetInventoryTransactionByInvID.FromDate = Convert.ToDateTime(txtInvDtFrom.Text);
            }
            else
            {
                objProp_Inventory.FromDate = System.DateTime.MinValue;
                _GetInventoryTransactionByInvID.FromDate = System.DateTime.MinValue;
            }

            if (txtInvDtTo.Text != string.Empty)
            {
                objProp_Inventory.EndDate = Convert.ToDateTime(txtInvDtTo.Text);
                _GetInventoryTransactionByInvID.EndDate = Convert.ToDateTime(txtInvDtTo.Text);
            }
            else
            {
                objProp_Inventory.EndDate = System.DateTime.MinValue;
                _GetInventoryTransactionByInvID.EndDate = System.DateTime.MinValue;
            }
            objProp_Inventory.UserID = Convert.ToInt32(Session["UserID"].ToString());
            _GetInventoryTransactionByInvID.UserID = Convert.ToInt32(Session["UserID"].ToString());
            if (System.Web.HttpContext.Current.Session["CmpChkDefault"].ToString() == "1")
            {
                objProp_Inventory.EN = 1;
                _GetInventoryTransactionByInvID.EN = 1;
            }
            else
            {
                objProp_Inventory.EN = 0;
                _GetInventoryTransactionByInvID.EN = 0;
            }


            List<InventoryTransactionByInvIDViewModel> _lstInventoryTransaction = new List<InventoryTransactionByInvIDViewModel>();
            string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "InventoryAPI/AddInventory_GetInventoryTransactionByInvID";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInventoryTransactionByInvID);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstInventoryTransaction = serializer.Deserialize<List<InventoryTransactionByInvIDViewModel>>(_APIResponse.ResponseData);
                dSInvTrasaction = CommonMethods.ToDataSet<InventoryTransactionByInvIDViewModel>(_lstInventoryTransaction);
                dSInvTrasaction.Tables[0].Columns["Ref"].ColumnName = "ref";
            }
            else
            {
                dSInvTrasaction = objBL_Inventory.GetInventoryTransactionByInvID(objProp_Inventory);
            }


            #endregion
            RadGridTran.VirtualItemCount = dSInvTrasaction.Tables[0].Rows.Count;
            RadGridTran.DataSource = dSInvTrasaction.Tables[0];
            lblRecordCount.Text = dSInvTrasaction.Tables[0].Rows.Count.ToString() + " Record(s) Found.";
            if (!flag)  RadGridTran.Rebind();

            //if (isShowAllInvoices)
            //{
            //    gvInvoice.PageSize = ds.Tables[0].Rows.Count;
            //}
            //if (dSInvTrasaction.Tables[0].Rows.Count > 0)
            //{
            //BindInvTrasactionGridDatatable(dSInvTrasaction.Tables[0]);
            //}
        }

    }

    private void BindInvTrasactionGridDatatable(DataTable dt)
    {

        List<DataRow> rowsWantToDelete = new List<DataRow>();

        foreach (DataRow dr in dt.Rows)
        {
            String ReceivePO_2 = Convert.ToString(dr["ReceivePO_2"] == DBNull.Value ? "0" : dr["ReceivePO_2"]);
            foreach (DataRow drr in dt.Rows)
            {
                String ReceivePO_1 = Convert.ToString(drr["ReceivePO_1"] == DBNull.Value ? "0" : drr["ReceivePO_1"]);
                String Type = Convert.ToString(drr["Type"]);
                if (ReceivePO_2 == ReceivePO_1 && Type == "81")
                {
                    rowsWantToDelete.Add(drr);
                }
            }
        }

        foreach (DataRow dr in rowsWantToDelete)
        {
            try
            {
                dt.Rows.Remove(dr);
            }
            catch { }
        }
        dt.AcceptChanges();


        Session["InvoiceSrchCust"] = dt;




        if (dt.Rows.Count > 0)

        {
            if (!dt.Columns.Contains("Balance"))
            {
                dt.Columns.Add("Balance", typeof(Double));
            }
            Double Runtotal = 0.00;
            //to convert deposit, recievepayment  to -ve values 
            foreach (DataRow row in dt.Rows)
            {
                 if (((row["Type"].ToString() == "60") || (row["Type"].ToString() == "41") || (row["Type"].ToString() == "81") || (row["Type"].ToString() == "70")) && (Convert.ToDouble(row["Credits"].ToString()) > 0))
                {
                    if (row["IssuedType"].ToString() != "1")
                    {
                        //row["Charges"] = Convert.ToDouble(row["Charges"].ToString()) * (-1);
                        row["Balance"] = Convert.ToDouble(row["Charges"].ToString()) + Runtotal;
                    }
                    else
                    {
                        row["Balance"] = 0 + Runtotal;
                    }
                }
                else if (((row["Type"].ToString() == "60") || (row["Type"].ToString() == "41") || (row["Type"].ToString() == "81") || (row["Type"].ToString() == "70")) && (Convert.ToDouble(row["Credits"].ToString()) > 0))
                {
                    if (row["IssuedType"].ToString() != "1")
                    {
                        row["Credits"] = Convert.ToDouble(row["Credits"].ToString()) * (-1);
                        row["Balance"] = Convert.ToDouble(row["Credits"].ToString()) + Runtotal;
                    }
                    else
                    {
                        row["Balance"] = 0 + Runtotal;
                    }
                }
                else
                {
                    row["Balance"] = 0.00;
                }



                Runtotal = Convert.ToDouble(row["Balance"].ToString());


            }
        }
        RadGridTran.VirtualItemCount = dt.Rows.Count;
        RadGridTran.DataSource = dt;
        RadGridTran.DataBind();

        lblRecordCount.Text = dt.Rows.Count.ToString() + " Record(s) Found.";



        //if (dt.Rows.Count > 0)
        //{
        //    Label lblTotalAmount = (Label)gvInvoice.FooterRow.FindControl("lblTotalAmount");
        //    lblTotalAmount.Text = string.Format("{0:c}", dt.Compute("sum(amount)", string.Empty));
        //}
    }
    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGridTran.MasterTableView.FilterExpression != "" ||
            (RadGridTran.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGridTran.MasterTableView.SortExpressions.Count > 0;
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        int DaysinMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - 1;
        DateTime lastDay = firstDay.AddDays(DaysinMonth);
        txtInvDtFrom.Text = firstDay.ToShortDateString();
        txtInvDtTo.Text = lastDay.ToShortDateString();
        
        if (!string.IsNullOrEmpty(Request.QueryString["uid"].ToString()))
            GetInvTransactions();
    }


    protected void RadGrid_RadGridTran_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGridTran.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        GetInvTransactions(true);


    }
    protected void RadGrid_RadGridTran_ItemCreated(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridPagerItem)
        {
            var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
            var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;

            GeneralFunctions obj = new GeneralFunctions();
            var sizes = obj.TelerikPageSize(totalCount);

            dropDown.Items.Clear();
            foreach (var size in sizes)
            {
                var cboItem = new RadComboBoxItem() { Text = size.Key, Value = size.Value };
                cboItem.Attributes.Add("ownerTableViewId", e.Item.OwnerTableView.ClientID);
                dropDown.Items.Add(cboItem);
            }
            dropDown.FindItemByValue(e.Item.OwnerTableView.PageSize.ToString()).Selected = true;

        }
    }
    protected void RadGrid_RadGridTran_PreRender(object sender, EventArgs e)
    {
        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGridTran);
        
    }
    protected void lnkShowAll_Click(object sender, EventArgs e)
    {

        
        txtInvDtTo.Text = string.Empty;
        txtInvDtFrom.Text = string.Empty;
        // isShowAllInvoices = true;
        GetInvTransactions();
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
       
            try
        {
            DataTable dt = (DataTable)Session["InventoryList"];
            Response.Redirect("AddInventory.aspx?uid=" + dt.Rows[0]["ID"]);
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
            DataTable dt = (DataTable)Session["InventoryList"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
            int index = dt.Rows.IndexOf(d);

            if (index > 0)
            {
                Response.Redirect("AddInventory.aspx?uid=" + dt.Rows[index - 1]["ID"]);
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
            DataTable dt = (DataTable)Session["InventoryList"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;

            if (index < c)
            {
                Response.Redirect("AddInventory.aspx?uid=" + dt.Rows[index + 1]["ID"]);
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
            DataTable dt = (DataTable)Session["InventoryList"];
            Response.Redirect("AddInventory.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["ID"]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}



