using BusinessEntity;
using BusinessEntity.InventoryModel;
using BusinessEntity.Utility;
using BusinessLayer;
using MOMWebApp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PostToProject : System.Web.UI.Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    BL_Bills _objBLBills = new BL_Bills();
    BL_Vendor _objBLVendor = new BL_Vendor();
    Vendor _objVendor = new Vendor();
    PO _objPO = new PO();
    protected DataTable dtWorker = new DataTable();
    PostInventoryItemsToProjectParam _PostInventoryItemsToProject = new PostInventoryItemsToProjectParam();

    //API Variables

    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    GetEMPParam _GetEMP = new GetEMPParam();
    protected void Page_Load(object sender, EventArgs e)
	{
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        FillWorker("");
        if (!IsPostBack)
        {
            string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

            if (Request.Url.Scheme == "http" && SSL == "1")
            {
                string URL = Request.Url.ToString();
                URL = URL.Replace("http://", "https://");
                Response.Redirect(URL);
            }

            _objPO.ConnConfig = Session["config"].ToString();
            _objPO.POID = Convert.ToInt32(Request.QueryString["id"]);
            #region Company Check
            _objPO.UserID = Convert.ToInt32(Session["UserID"].ToString());
            if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            {
                _objPO.EN = 1;
            }
            else
            {
                _objPO.EN = 0;
            }
            #endregion
            var dt = BuildPtProjTable();
            DataRow dr = dt.NewRow();
            dr["Line"] = 1;
            dr["FDate"] = DateTime.Now.ToString("MM/dd/yyyy");
            dr["FTime"] = DateTime.Now.ToString("hh:mm tt");
            dt.Rows.Add(dr);
            
            RadGrid_VendorItems.DataSource = dt;//ds.Tables[1];
            RadGrid_VendorItems.DataBind();
        }

        Permission();
    }

    protected void RadGrid_VendorItems_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeleteTransaction")
            {
                int index = 1;
                int indexRow = Convert.ToInt32(e.CommandArgument);
                var dt = BuildPtProjTable();

                for (var i = 0; i < RadGrid_VendorItems.Items.Count; i++)
                {
                    if (i != indexRow)
                    {
                        GridDataItem row = RadGrid_VendorItems.Items[i];

                        HiddenField hdnLine = (HiddenField)row.FindControl("hdnLine");
                        HiddenField hdnAcctID = (HiddenField)row.FindControl("hdnAcctID");
                        TextBox txtGvAcctNo = (TextBox)row.FindControl("txtGvAcctNo");
                        TextBox txtGvDesc = (TextBox)row.FindControl("txtGvDesc");
                        TextBox txtGvQuan = (TextBox)row.FindControl("txtGvQuan");
                        TextBox txtGvLoc = (TextBox)row.FindControl("txtGvLoc");
                        TextBox txtGvJob = (TextBox)row.FindControl("txtGvJob");
                        HiddenField hdnJobID = (HiddenField)row.FindControl("hdnJobID");
                        TextBox txtGvPhase = (TextBox)row.FindControl("txtGvPhase");
                        HiddenField hdnPID = (HiddenField)row.FindControl("hdnPID");
                        HiddenField hdnTypeId = (HiddenField)row.FindControl("hdnTypeId");
                        TextBox txtGvItem = (TextBox)row.FindControl("txtGvItem");
                        HiddenField hdnItemID = (HiddenField)row.FindControl("hdnItemID");
                        TextBox txtGvWarehouse = (TextBox)row.FindControl("txtGvWarehouse");
                        HiddenField hdnWarehouse = (HiddenField)row.FindControl("hdnWarehouse");
                        TextBox txtGvWarehouseLocation = (TextBox)row.FindControl("txtGvWarehouseLocation");
                        HiddenField hdnWarehouseLocationID = (HiddenField)row.FindControl("hdnWarehouseLocationID");
                        TextBox txtDate = (TextBox)row.FindControl("txtDate");
                        TextBox txtTime = (TextBox)row.FindControl("txtTime");
                        DropDownList ddlWorker = (DropDownList)row.FindControl("ddlWorker");
                        CheckBox chkBill = (CheckBox)row.FindControl("chkBill");

                        var dr = dt.NewRow();
                        dr["Line"] = index;
                        dr["AcctID"] = hdnAcctID.Value;
                        dr["AcctNo"] = txtGvAcctNo.Text;
                        dr["fDesc"] = txtGvDesc.Text;
                        dr["Loc"] = txtGvLoc.Text;
                        dr["JobName"] = txtGvJob.Text;
                        dr["JobID"] = hdnJobID.Value;
                        dr["Phase"] = txtGvPhase.Text;
                        dr["PhaseID"] = hdnPID.Value;
                        dr["TypeID"] = hdnTypeId.Value;
                        dr["Quan"] = txtGvQuan.Text;
                        dr["ItemDesc"] = txtGvItem.Text;
                        dr["Inv"] = hdnItemID.Value;
                        dr["Warehousefdesc"] = txtGvWarehouse.Text;
                        dr["WarehouseID"] = hdnWarehouse.Value;
                        dr["Locationfdesc"] = txtGvWarehouseLocation.Text;
                        dr["WHLocID"] = hdnWarehouseLocationID.Value;
                        dr["Fdate"] = txtDate.Text;
                        dr["FTime"] = txtTime.Text;
                        dr["Worker"] = ddlWorker.SelectedValue;
                        dr["Billed"] = chkBill.Checked ? 1 : 0;

                        dt.Rows.Add(dr);

                        index++;
                    }
                }

                RadGrid_VendorItems.DataSource = dt;
                RadGrid_VendorItems.DataBind();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void btnAddNewLines_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = GetPtProjGridItems();
            Int32 Line = 0;//Default if dt is null
            if (dt != null && dt.Rows.Count > 0)
            {
                Line = Convert.ToInt32(Convert.ToString(dt.Rows[dt.Rows.Count - 1]["Line"]) == "" ? "0" : Convert.ToString(dt.Rows[dt.Rows.Count - 1]["Line"]));
            }

            DataRow dr = dt.NewRow();
            dr["Line"] = Line + 1;
            dr["FDate"] = DateTime.Now.ToString("MM/dd/yyyy");
            dr["FTime"] = DateTime.Now.ToString("hh:mm tt");

            dt.Rows.Add(dr);
            
            RadGrid_VendorItems.DataSource = dt;
            RadGrid_VendorItems.DataBind();

            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "CalculateTotalAmt", "CalculateTotalAmt();", true);

            //Focus last row
            GridDataItem lastRow = RadGrid_VendorItems.Items[RadGrid_VendorItems.Items.Count - 1];
            TextBox txtGvJob = (TextBox)lastRow.FindControl("txtGvJob");
            if (txtGvJob != null)
            {
                txtGvJob.Focus();
            }
            hdnSelectedInvIndex.Value = (RadGrid_VendorItems.Items.Count - 1).ToString();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this,Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void btnCopyPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            var selectIndex = 0;

            if (!string.IsNullOrEmpty(hdnSelectedInvIndex.Value))
            {
                selectIndex = Convert.ToInt32(hdnSelectedInvIndex.Value);
            }
            else
            {
                var selectItem = RadGrid_VendorItems.MasterTableView.GetSelectedItems();
                if (selectItem.Count() > 0)
                {
                    selectIndex = selectItem[0].ClientRowIndex;
                }
            }

            DataTable dt = GetPtProjGridItems();
            if (dt.Rows.Count > 0 && selectIndex > 0)
            {
                var copyRow = dt.Rows[selectIndex - 1];
                var dr = dt.Rows[selectIndex];

                dr["AcctID"] = copyRow["AcctID"];
                dr["AcctNo"] = copyRow["AcctNo"];
                dr["fDesc"] = copyRow["fDesc"];
                dr["Loc"] = copyRow["Loc"];
                dr["JobName"] = copyRow["JobName"];
                dr["JobID"] = copyRow["JobID"];
                dr["Phase"] = copyRow["Phase"];
                dr["PhaseID"] = copyRow["PhaseID"];
                dr["TypeID"] = copyRow["TypeID"];
                dr["Quan"] = copyRow["Quan"];
                dr["ItemDesc"] = copyRow["ItemDesc"];
                dr["Inv"] = copyRow["Inv"];
                dr["Warehousefdesc"] = copyRow["Warehousefdesc"];
                dr["WarehouseID"] = copyRow["WarehouseID"];
                dr["Locationfdesc"] = copyRow["Locationfdesc"];
                dr["WHLocID"] = copyRow["WHLocID"];
                dr["Fdate"] = copyRow["Fdate"];
                dr["FTime"] = copyRow["FTime"];
                dr["Worker"] = copyRow["Worker"];
                dr["Billed"] = copyRow["Billed"];
                dt.AcceptChanges();

                RadGrid_VendorItems.DataSource = dt;
                RadGrid_VendorItems.VirtualItemCount = dt.Rows.Count;
                RadGrid_VendorItems.DataBind();

                //ScriptManager.RegisterStartupScript(this, Page.GetType(), "CalculateTotalAmt", "CalculateTotalAmt();", true);

                //Focus row
                GridDataItem focusRow = RadGrid_VendorItems.Items[selectIndex];
                TextBox txtGvJob = (TextBox)focusRow.FindControl("txtGvJob");
                if (txtGvJob != null)
                {
                    txtGvJob.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private DataTable GetPtProjGridItems()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add(new DataColumn("PhaseID", typeof(string)));
        dt.Columns.Add(new DataColumn("Phase", typeof(string)));
        dt.Columns.Add(new DataColumn("Inv", typeof(string)));
        dt.Columns.Add(new DataColumn("ItemDesc", typeof(string)));
        dt.Columns.Add(new DataColumn("fDesc", typeof(string)));
        dt.Columns.Add(new DataColumn("Quan", typeof(string)));
        dt.Columns.Add(new DataColumn("WHLocID", typeof(string)));
        dt.Columns.Add(new DataColumn("WarehouseID", typeof(string)));
        dt.Columns.Add(new DataColumn("TypeID", typeof(string)));
        dt.Columns.Add(new DataColumn("JobID", typeof(string)));
        dt.Columns.Add(new DataColumn("AcctID", typeof(string)));
        dt.Columns.Add(new DataColumn("AcctNo", typeof(string)));
        dt.Columns.Add(new DataColumn("SchDate", typeof(string)));
        dt.Columns.Add(new DataColumn("Fdate", typeof(string)));
        dt.Columns.Add(new DataColumn("FTime", typeof(string)));
        dt.Columns.Add(new DataColumn("worker", typeof(string)));
        dt.Columns.Add(new DataColumn("Billed", typeof(Int32)));

        dt.Columns.Add(new DataColumn("JobName", typeof(string)));
        dt.Columns.Add(new DataColumn("Locationfdesc", typeof(string)));
        dt.Columns.Add(new DataColumn("Warehousefdesc", typeof(string)));
        dt.Columns.Add(new DataColumn("Loc", typeof(string)));
        dt.Columns.Add(new DataColumn("Line", typeof(Int16)));

        try
        {
            string strItems = hdnItemJSON.Value.Trim();
            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);

                int index = 1;
                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    if (dict["hdnIndex"].ToString().Trim() == string.Empty)
                    {
                        return dt;
                    }
                    DataRow dr = dt.NewRow();
                    dr["AcctNo"] = dict["txtGvAcctNo"].ToString().Trim();
                    dr["fDesc"] = dict["txtGvDesc"].ToString().Trim();
                    dr["Quan"] = dict["txtGvQuan"].ToString();
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
                    if (dict.ContainsKey("hdnTypeId"))
                    {
                        if (!(dict["hdnTypeId"].ToString().Trim() == string.Empty))
                        {
                            dr["TypeID"] = Convert.ToInt32(dict["hdnTypeId"].ToString().Trim());
                        }
                    }
                    if (!(dict["hdnItemID"].ToString().Trim() == string.Empty))
                    {
                        dr["Inv"] = Convert.ToInt32(dict["hdnItemID"]);
                    }
                    if (!(dict["txtGvItem"].ToString().Trim() == string.Empty))
                    {
                        dr["ItemDesc"] = dict["txtGvItem"].ToString();
                    }
                    if (dict.ContainsKey("hdnPID"))
                    {
                        if (!(dict["hdnPID"].ToString().Trim() == string.Empty))
                        {
                            dr["PhaseID"] = Convert.ToInt32(dict["hdnPID"].ToString());
                        }
                    }
                    if (dict.ContainsKey("txtGvPhase"))
                    {
                        if (!(dict["txtGvPhase"].ToString().Trim() == string.Empty))
                        {
                            dr["Phase"] = dict["txtGvPhase"].ToString().Trim();
                        }
                    }
                    dr["WarehouseID"] = DBNull.Value;
                    if (dict.ContainsKey("hdnWarehouse"))
                    {
                        if (!(dict["hdnWarehouse"].ToString().Trim() == string.Empty))
                        {
                            dr["WarehouseID"] = dict["hdnWarehouse"].ToString();
                        }
                    }
                    if (dict.ContainsKey("txtGvWarehouse"))
                    {
                        if (!(dict["txtGvWarehouse"].ToString().Trim() == string.Empty))
                        {
                            dr["Warehousefdesc"] = dict["txtGvWarehouse"].ToString();
                        }
                    }
                    dr["WHLocID"] = DBNull.Value;
                    if (dict.ContainsKey("hdnWarehouseLocationID"))
                    {
                        if (!(dict["hdnWarehouseLocationID"].ToString().Trim() == string.Empty))
                        {
                            dr["WHLocID"] = dict["hdnWarehouseLocationID"].ToString();
                        }
                    }
                    if (dict.ContainsKey("txtGvWarehouseLocation"))
                    {
                        if (!(dict["txtGvWarehouseLocation"].ToString().Trim() == string.Empty))
                        {
                            dr["Locationfdesc"] = dict["txtGvWarehouseLocation"].ToString();
                        }
                    }
                    
                    var schDate = dict["txtDate"].ToString() + " " + dict["txtTime"].ToString();
                    dr["SchDate"] = schDate;
                    //if (!(dict["txtDate"].ToString().Trim() == string.Empty))
                    //    dr["Fdate"] = Convert.ToDateTime(dict["txtDate"].ToString());
                    //if (!(dict["txtTime"].ToString().Trim() == string.Empty))
                    //    dr["Ftime"] = Convert.ToDateTime(dict["txtTime"].ToString());
                    if (!(dict["txtDate"].ToString().Trim() == string.Empty))
                        dr["Fdate"] = dict["txtDate"].ToString();
                    if (!(dict["txtTime"].ToString().Trim() == string.Empty))
                        dr["Ftime"] = dict["txtTime"].ToString();
                    dr["worker"] = dict["ddlWorker"].ToString();
                    if (!(dict["hdnAcctID"].ToString().Trim() == string.Empty))
                    {
                        dr["AcctID"] = Convert.ToInt32(dict["hdnAcctID"].ToString().Trim());
                    }
                    if (!(dict["hdnLine"].ToString().Trim() == string.Empty))
                    {
                        dr["Line"] = Convert.ToInt16(dict["hdnLine"].ToString());
                    }
                    if (dict.ContainsKey("chkBill"))
                    {
                        dr["Billed"] = 1;
                    }
                    else
                    {
                        dr["Billed"] = 0;
                    }

                    dt.Rows.Add(dr);
                    index++;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }

    protected DataTable BuildPtProjTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("PhaseID", typeof(string)));
        dt.Columns.Add(new DataColumn("Phase", typeof(string)));
        dt.Columns.Add(new DataColumn("Inv", typeof(string)));
        dt.Columns.Add(new DataColumn("ItemDesc", typeof(string)));
        dt.Columns.Add(new DataColumn("fDesc", typeof(string)));
        dt.Columns.Add(new DataColumn("Quan", typeof(string)));
        dt.Columns.Add(new DataColumn("WHLocID", typeof(string)));
        dt.Columns.Add(new DataColumn("WarehouseID", typeof(string)));
        dt.Columns.Add(new DataColumn("TypeID", typeof(string)));
        dt.Columns.Add(new DataColumn("JobID", typeof(string)));
        dt.Columns.Add(new DataColumn("AcctID", typeof(string)));
        dt.Columns.Add(new DataColumn("AcctNo", typeof(string)));
        dt.Columns.Add(new DataColumn("Fdate", typeof(string)));
        dt.Columns.Add(new DataColumn("FTime", typeof(string)));
        dt.Columns.Add(new DataColumn("worker", typeof(string)));
        dt.Columns.Add(new DataColumn("Billed", typeof(Int32)));

        dt.Columns.Add(new DataColumn("JobName", typeof(string)));
        dt.Columns.Add(new DataColumn("Locationfdesc", typeof(string)));
        dt.Columns.Add(new DataColumn("Loc", typeof(string)));
        dt.Columns.Add(new DataColumn("Warehousefdesc", typeof(string)));
        dt.Columns.Add(new DataColumn("Line", typeof(string)));

        //dt.Columns.Add(new DataColumn("RowID", typeof(string)));
        //dt.Columns.Add(new DataColumn("ID", typeof(string)));
        //dt.Columns.Add(new DataColumn("Price", typeof(string)));
        //dt.Columns.Add(new DataColumn("Amount", typeof(string)));
        //dt.Columns.Add(new DataColumn("Ticket", typeof(string)));
        //dt.Columns.Add(new DataColumn("Due", typeof(string)));
        //dt.Columns.Add(new DataColumn("OpSq", typeof(string)));
        return dt;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = GetPtProjGridItems();
            if (ValidateINV_GRID(dt))
            {
                dt.Columns.Remove("Loc");
                dt.Columns.Remove("JobName");
                dt.Columns.Remove("Locationfdesc");
                dt.Columns.Remove("Warehousefdesc");
                dt.Columns.Remove("Line");
                dt.Columns.Remove("FDate");
                dt.Columns.Remove("FTime");
                dt.AcceptChanges();


                BL_MapData bL_MapData = new BL_MapData();
                MapData mapData = new MapData();

                mapData.ConnConfig = Session["config"].ToString();
                mapData.CallDate = DateTime.Now;
                mapData.Who = Session["username"].ToString();
                mapData.Category = "";
                mapData.Reason = "Post To Project";
                mapData.dtTicketINV = dt;

                //API
                _PostInventoryItemsToProject.ConnConfig = Session["config"].ToString();
                _PostInventoryItemsToProject.CallDate = DateTime.Now;
                _PostInventoryItemsToProject.Who = Session["username"].ToString();
                _PostInventoryItemsToProject.Category = "";
                _PostInventoryItemsToProject.Reason = "Post To Project";
                _PostInventoryItemsToProject.dtTicketINV = dt;

                var tickectIds = string.Empty;
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet ds3 = new DataSet();

                ListPostInventoryItemsToProject _listPostInventoryItemsToProject = new ListPostInventoryItemsToProject();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "PostToProjectAPI/PostToProject_PostInventoryItemsToProject";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _PostInventoryItemsToProject, true);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _listPostInventoryItemsToProject = serializer.Deserialize<ListPostInventoryItemsToProject>(_APIResponse.ResponseData);

                    ds1 = _listPostInventoryItemsToProject.lstTable.ToDataSet();
                    ds2 = _listPostInventoryItemsToProject.lstTable1.ToDataSet();
                    ds3 = _listPostInventoryItemsToProject.lstTable2.ToDataSet();

                    DataTable dt1 = new DataTable();
                    DataTable dt2 = new DataTable();
                    DataTable dt3 = new DataTable();

                    dt1 = ds1.Tables[0];
                    dt2 = ds2.Tables[0];
                    dt3 = ds3.Tables[0];

                    dt1.TableName = "Table1";
                    dt2.TableName = "Table2";
                    dt3.TableName = "Table3";
                    ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(),dt3.Copy() });
                }
                else
                {
                    ds = bL_MapData.PostInventoryItemsToProject(mapData);
                }

                var count = ds.Tables.Count;
                if (count > 0)
                {
                    tickectIds = ds.Tables[count-1].Rows[0][0].ToString();
                }
                if (!string.IsNullOrEmpty(tickectIds))
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccess", "noty({text: 'Tickets created successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    var dt1 = BuildPtProjTable();
                    dt1.Rows.Add(dt1.NewRow());
                    RadGrid_VendorItems.DataSource = dt1;
                    RadGrid_VendorItems.DataBind();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: 'There was no ticket created',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private DataTable GetElevData()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ticket_id", typeof(int));
        dt.Columns.Add("elev_id", typeof(int));
        dt.Columns.Add("labor_percentage", typeof(double));

        return dt;
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    private void FillWorker(string Worker)
    {
        if (ViewState["dtWorker"] == null)
        {
            DataSet ds = new DataSet();
            var objPropUser = new User();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.Status = 0;
            objPropUser.Username = Worker;

            _GetEMP.ConnConfig = Session["config"].ToString();
            _GetEMP.Status = 0;
            _GetEMP.Username = Worker;

            List<GetEMPViewModel> _lstGetEMP = new List<GetEMPViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "PostToProjectAPI/PostToProject_GetEMP";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetEMP, true);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetEMP = serializer.Deserialize<List<GetEMPViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetEMPViewModel>(_lstGetEMP);
            }
            else
            {
                ds = objBL_User.getEMP(objPropUser);
            }

            ViewState["dtWorker"] = ds.Tables[0];
            dtWorker = ds.Tables[0];
        }
        else
        {
            dtWorker = (DataTable)ViewState["dtWorker"];
        }
    }

    private bool ValidateINV_GRID(DataTable dt)
    {
        //dr["PhaseID"]
        //dr["Phase"]
        //dr["Inv"]
        //dr["ItemDesc"]
        //dr["fDesc"]
        //dr["Quan"]
        //dr["WHLocID"]
        //dr["WarehouseID"]
        //dr["TypeID"]
        //dr["JobID"]
        //dr["AcctID"]
        //dr["AcctNo"]
        //dr["SchDate"]
        //dr["Fdate"]
        //dr["FTime"]
        //dr["worker"]
        foreach (DataRow dr in dt.Rows)
        {
            if(string.IsNullOrEmpty(dr["Phase"].ToString()) 
                || string.IsNullOrEmpty(dr["Inv"].ToString())
                || string.IsNullOrEmpty(dr["Quan"].ToString())
                || string.IsNullOrEmpty(dr["WarehouseID"].ToString())
                || string.IsNullOrEmpty(dr["JobID"].ToString())
                || string.IsNullOrEmpty(dr["worker"].ToString())
                || string.IsNullOrEmpty(dr["Fdate"].ToString())
                || string.IsNullOrEmpty(dr["FTime"].ToString())
                )
            {
                //ShowMessage("Please fill all required fields!", 0);
                StringBuilder errMess = new StringBuilder();
                if (string.IsNullOrEmpty(dr["JobID"].ToString())) errMess.Append("Project,");
                if (string.IsNullOrEmpty(dr["Phase"].ToString())) errMess.Append("Code,");
                if (string.IsNullOrEmpty(dr["Inv"].ToString())) errMess.Append("Item,");
                if (string.IsNullOrEmpty(dr["Quan"].ToString())) errMess.Append("Quan,");
                if (string.IsNullOrEmpty(dr["WarehouseID"].ToString())) errMess.Append("Warehouse,");
                if (string.IsNullOrEmpty(dr["worker"].ToString())) errMess.Append("Worker,");
                if (string.IsNullOrEmpty(dr["Fdate"].ToString())) errMess.Append("Date,");
                if (string.IsNullOrEmpty(dr["FTime"].ToString())) errMess.Append("Time,");
                if(errMess.Length > 0)
                {
                    errMess.Insert(0, "Please enter data for fields: ");
                    errMess.Remove(errMess.Length - 1, 1);
                }
                ShowMessage(errMess.ToString(), 0);
                return false;
            }

            if (!string.IsNullOrEmpty(dr["Inv"].ToString().Trim()))
            {
                if (!string.IsNullOrEmpty(dr["Phase"].ToString().Trim()))
                {
                    double QunaINV = 0.00;
                    double.TryParse(dr["Quan"].ToString().Trim(), out QunaINV);

                    if (QunaINV == 0)
                    {
                        ShowMessage("Please enter a quantity for the inventory items." + dr["fDesc"].ToString(), 0);
                        return false;
                    }

                    if (QunaINV > 0)
                    {
                        if (string.IsNullOrEmpty(dr["WarehouseID"].ToString().Trim()))
                        {
                            ShowMessage("Please enter a warehouse for the inventory items." + dr["fDesc"].ToString(), 0);
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Show Message
    /// </summary>
    /// <param name="mesg"></param>
    /// <param name="type"> if (type == 0)   /// Warning message  if (type == 1)  /// Success message</param>
    /// 

    private void ShowMessage(string mesg, Int16 type)
    {
        if (type == 0)            /// Warning message
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keywarning1", "noty({text: '" + mesg + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
        else if (type == 1)       /// Success message
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysucc1", "noty({text: '" + mesg + "',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
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

                string InventoryItemPermission = ds.Rows[0]["Item"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Item"].ToString();
                String addInventory = InventoryItemPermission.Length < 1 ? "Y" : InventoryItemPermission.Substring(0, 1);

                if (addInventory == "N")
                {
                    btnSubmit.Visible = false;
                }
                else
                {
                    btnSubmit.Visible = true;
                }
            }
        }
        catch (Exception)
        {
            btnSubmit.Visible = false;
        }
    }
}
