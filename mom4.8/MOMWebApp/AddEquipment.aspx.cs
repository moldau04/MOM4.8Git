using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using System.Globalization;
using Telerik.Web.UI;
using Telerik.Web.UI.PersistenceFramework;
using AjaxControlToolkit;
using ZXing;
using System.Text;
using System.IO;
using System.Data.Odbc;
using BusinessEntity.Recurring;
using System.Threading;
using BusinessEntity.CustomersModel;
using BusinessEntity.Utility;
using MOMWebApp;
using Newtonsoft.Json;
using BusinessEntity.InventoryModel;

public partial class AddEquipment : System.Web.UI.Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    BL_Customer objBL_Customer = new BL_Customer();
    Customer objPropCustomer = new Customer();
    MapData objMapData = new MapData();
    BL_MapData objBL_MapData = new BL_MapData();
    private static readonly string CookieName = "CkEditEquip";
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    public int templateId = 0;
    private int pickerCount = 4;
    StringBuilder scriptText = new StringBuilder();

    //API Variables 
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    GetLeadEquipByIDParam _GetLeadEquipByID = new GetLeadEquipByIDParam();
    GetequipByIDParam _GetequipByID = new GetequipByIDParam();
    GetBuildingLeadEquipParam _GetBuildingLeadEquip = new GetBuildingLeadEquipParam();
    GetBuildingElevParam _GetBuildingElev = new GetBuildingElevParam();
    GetequipREPDetailsParam _GetequipREPDetails = new GetequipREPDetailsParam();
    GetCustomTemplateParam _GetCustomTemplate = new GetCustomTemplateParam();
    GetEquipmentCategoryParam _GetEquipmentCategory = new GetEquipmentCategoryParam();
    GetLeadEquipmentCategoryParam _GetLeadEquipmentCategory = new GetLeadEquipmentCategoryParam();
    GetEquiptypeParam _GetEquiptype = new GetEquiptypeParam();
    GetLeadEquiptypeParam _GetLeadEquiptype = new GetLeadEquiptypeParam();
    GetActiveServiceTypeParam _GetActiveServiceType = new GetActiveServiceTypeParam();
    GetRepTemplateNameParam _GetRepTemplateName = new GetRepTemplateNameParam();
    UpdateDocInfoParam _UpdateDocInfo = new UpdateDocInfoParam();
    UpdateLeadEquipmentParam _UpdateLeadEquipment = new UpdateLeadEquipmentParam();
    UpdateEquipmentParam _UpdateEquipment = new UpdateEquipmentParam();
    AddEquipmentForLeadParam _AddEquipmentForLead = new AddEquipmentForLeadParam();
    AddEquipmentParam _AddEquipment = new AddEquipmentParam();
    GetTemplateItemByIDParam _GetTemplateItemByID = new GetTemplateItemByIDParam();
    GetCustTemplateItemByIDParam _GetCustTemplateItemByID = new GetCustTemplateItemByIDParam();
    GetContractTypeParam _GetContractType = new GetContractTypeParam();
    GetEquipmentTestsParam _GetEquipmentTests = new GetEquipmentTestsParam();
    AddFileParam _AddFile = new AddFileParam();
    DeleteFileParam _DeleteFile = new DeleteFileParam();
    GetDocumentsParam _GetDocuments = new GetDocumentsParam();
    GetLeadEquipClassificationParam _GetLeadEquipClassification = new GetLeadEquipClassificationParam();
    GetEquipClassificationParam _GetEquipClassification = new GetEquipClassificationParam();
    GetShutdownReasonsParam _GetShutdownReasons = new GetShutdownReasonsParam();
    GetShutdownReasonByIDParam _GetShutdownReasonByID = new GetShutdownReasonByIDParam();
    AddShutdownReasonParam _AddShutdownReason = new AddShutdownReasonParam();
    EditShutdownReasonParam _EditShutdownReason = new EditShutdownReasonParam();
    GetEquipShutdownLogsParam _GetEquipShutdownLogs = new GetEquipShutdownLogsParam();
    GetLocationByCustomerIDParam _GetLocationByCustomerID = new GetLocationByCustomerIDParam();

    private void Get_CustomData()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.EquipID = Convert.ToInt32(Request.QueryString["uid"]);

        _GetLeadEquipByID.ConnConfig = Session["config"].ToString();
        _GetLeadEquipByID.EquipID = Convert.ToInt32(Request.QueryString["uid"]);

        _GetequipByID.ConnConfig = Session["config"].ToString();
        _GetequipByID.EquipID = Convert.ToInt32(Request.QueryString["uid"]);

        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();
        DataSet ds3 = new DataSet();
        DataSet ds4 = new DataSet();
        DataSet ds5 = new DataSet();
        ListGetequipByID _lstGetequipByID = new ListGetequipByID();
        //ds = objBL_User.getequipByID(objPropUser);        

        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString() == "addprospect")
            {
                ListGetLeadEquipByID _lstGetLeadEquipByID = new ListGetLeadEquipByID();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "EquipmentAPI/AddEquipment_GetLeadEquipByID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetLeadEquipByID);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetLeadEquipByID = serializer.Deserialize<ListGetLeadEquipByID>(_APIResponse.ResponseData);

                    ds1 = _lstGetLeadEquipByID.lstTable.ToDataSet();
                    ds2 = _lstGetLeadEquipByID.lstTable1.ToDataSet();
                    ds3 = _lstGetLeadEquipByID.lstTable2.ToDataSet();
                    ds4 = _lstGetLeadEquipByID.lstTable3.ToDataSet();
                    ds5 = _lstGetLeadEquipByID.lstTable4.ToDataSet();

                    DataTable dt1 = new DataTable();
                    DataTable dt2 = new DataTable();
                    DataTable dt3 = new DataTable();
                    DataTable dt4 = new DataTable();
                    DataTable dt5 = new DataTable();

                    dt1 = ds1.Tables[0];
                    dt2 = ds2.Tables[0];
                    dt3 = ds3.Tables[0];
                    dt4 = ds4.Tables[0];
                    dt5 = ds5.Tables[0];

                    dt1.TableName = "Table1";
                    dt2.TableName = "Table2";
                    dt3.TableName = "Table3";
                    dt4.TableName = "Table4";
                    dt5.TableName = "Table5";

                    ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy(), dt4.Copy(), dt5.Copy() });
                }
                else
                {
                    ds = objBL_User.getLeadEquipByID(objPropUser);
                }
            }

            else
            {
                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "EquipmentAPI/AddEquipment_GetequipByID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetequipByID);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetequipByID = serializer.Deserialize<ListGetequipByID>(_APIResponse.ResponseData);

                    ds1 = _lstGetequipByID.lstTable1.ToDataSet();
                    ds2 = _lstGetequipByID.lstTable2.ToDataSet();
                    ds3 = _lstGetequipByID.lstTable3.ToDataSet();
                    ds4 = _lstGetequipByID.lstTable4.ToDataSet();
                    ds5 = _lstGetequipByID.lstTable5.ToDataSet();

                    ds3.Tables[0].Columns["OrderNo1"].ColumnName = "OrderNo";

                    DataTable dt1 = new DataTable();
                    DataTable dt2 = new DataTable();
                    DataTable dt3 = new DataTable();
                    DataTable dt4 = new DataTable();
                    DataTable dt5 = new DataTable();

                    dt1 = ds1.Tables[0];
                    dt2 = ds2.Tables[0];
                    dt3 = ds3.Tables[0];
                    dt4 = ds4.Tables[0];
                    dt5 = ds5.Tables[0];

                    dt1.TableName = "Table1";
                    dt2.TableName = "Table2";
                    dt3.TableName = "Table3";
                    dt4.TableName = "Table4";
                    dt5.TableName = "Table5";

                    ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy(), dt4.Copy(), dt5.Copy() });
                }
                else
                {
                    ds = objBL_User.getequipByID(objPropUser);
                }
            }
        }
        else
        {
            
            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/AddEquipment_GetequipByID";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetequipByID);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetequipByID = serializer.Deserialize<ListGetequipByID>(_APIResponse.ResponseData);

                ds1 = _lstGetequipByID.lstTable1.ToDataSet();
                ds2 = _lstGetequipByID.lstTable2.ToDataSet();
                ds3 = _lstGetequipByID.lstTable3.ToDataSet();
                ds4 = _lstGetequipByID.lstTable4.ToDataSet();
                ds5 = _lstGetequipByID.lstTable5.ToDataSet();

                ds3.Tables[0].Columns["OrderNo1"].ColumnName = "OrderNo";

                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();
                DataTable dt4 = new DataTable();
                DataTable dt5 = new DataTable();

                dt1 = ds1.Tables[0];
                dt2 = ds2.Tables[0];
                dt3 = ds3.Tables[0];
                dt4 = ds4.Tables[0];
                dt5 = ds5.Tables[0];

                dt1.TableName = "Table1";
                dt2.TableName = "Table2";
                dt3.TableName = "Table3";
                dt4.TableName = "Table4";
                dt5.TableName = "Table5";

                ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy(), dt4.Copy(), dt5.Copy() });
            }
            else
            {
                ds = objBL_User.getequipByID(objPropUser);
            }
        }
        if (ddlCustTemplate.SelectedIndex != 0)
        {
            RadGrid_gvCtemplItems.VirtualItemCount = ds.Tables[2].Rows.Count;
            RadGrid_gvCtemplItems.DataSource = ds.Tables[2];
            RadGrid_gvCtemplItems.Rebind();

        }
        else
        {
            DataTable dt = new DataTable();
            RadGrid_gvCtemplItems.VirtualItemCount = dt.Rows.Count;
            RadGrid_gvCtemplItems.DataSource = dt;
            //RadGrid_gvCtemplItems.Rebind();
        } 

        if (ds.Tables[3].Rows.Count > 0)
        {
            binditemgrid(ds.Tables[3]);
        }
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        RadPersistenceManager1.StorageProviderKey = CookieName;
        RadPersistenceManager1.StorageProvider = new CookieStorageProvider(CookieName);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (Request.QueryString["c"] != null)
        {
            if (Request.QueryString["c"] == "1" && ViewState["mode"]==null)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Equipment added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        PagePermission();
        if (!CheckAddEditPermission()) { Response.Redirect("Home.aspx?permission=no"); return; }
        //hdnCon.Value = Session["config"].ToString();

        pnlNext.Style["display"] = "none";
        if (!IsPostBack)
        {
            // AppendTemplateItemstoGrid(0, "", true);
            FillEquipClassification();
            FillEquipCategory();
            FillEquiptype();
            FillServiceType();
            FillRepTemplate();
            FillBuilding();
            FillShutdownReasons();
            GetCustomTemplate();
            ViewState["mode"] = 0;
            ViewState["editcon"] = 0;
            divShutdownReasonDesc.Visible = false;
            if (Request.QueryString["lid"] != null && Request.QueryString["locname"] != null)
            {
                hdnLocId.Value = Request.QueryString["lid"].ToString();
                txtLocation.Text = Request.QueryString["locname"].ToString();
                ShowMCPTabs(false);
            }

            if (Request.QueryString["cuid"] != null)
            {
                hdnPatientId.Value = Request.QueryString["cuid"].ToString();
                GetFillLocation();
            }
            if (Request.QueryString["uid"] == null)
            {
                lnkReport.Visible = false;
                lnkPrintMCP.Visible = false;
                dvCompanyPermission.Visible = false;
                Page.Title = "Add Equipment || MOM";
                if (chkShutdown.Checked)
                {
                    divShutdownReason.Visible = true;
                    //rfvShutdownReason.Enabled = true;
                }
                else
                {
                    divShutdownReason.Visible = false;
                    //rfvShutdownReason.Enabled = false;
                }

                ViewState["shut_down"] = false;
                hdnEqShutdownStatus.Value = "0";
            }
            if (Request.QueryString["uid"] != null)
            {
                Page.Title = "Edit Equipment || MOM";
                pnlNext.Style["display"] = "block";
                dvCompanyPermission.Visible = true;
                if (Request.QueryString["t"] != null)
                {
                    ViewState["mode"] = 0;
                    lblHeader.Text = "Copy Equipment";
                }
                else
                {
                    ViewState["mode"] = 1;
                    lnkReport.Visible = true;
                    lblHeader.Text = "Edit Equipment";
                }
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.EquipID = Convert.ToInt32(Request.QueryString["uid"]);

                _GetLeadEquipByID.ConnConfig = Session["config"].ToString();
                _GetLeadEquipByID.EquipID = Convert.ToInt32(Request.QueryString["uid"]);

                _GetequipByID.ConnConfig = Session["config"].ToString();
                _GetequipByID.EquipID = Convert.ToInt32(Request.QueryString["uid"]);
                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet ds3 = new DataSet();
                DataSet ds4 = new DataSet();
                DataSet ds5 = new DataSet();
                if (Request.QueryString["page"] != null)
                {
                    if (Request.QueryString["page"].ToString() == "addprospect")
                    {
                        location_container.Visible = false;

                        ListGetLeadEquipByID _lstGetLeadEquipByID = new ListGetLeadEquipByID();

                        if (IsAPIIntegrationEnable == "YES")
                        {
                            string APINAME = "EquipmentAPI/AddEquipment_GetLeadEquipByID";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetLeadEquipByID);

                            JavaScriptSerializer serializer = new JavaScriptSerializer();

                            serializer.MaxJsonLength = Int32.MaxValue;

                            _lstGetLeadEquipByID = serializer.Deserialize<ListGetLeadEquipByID>(_APIResponse.ResponseData);

                            ds1 = _lstGetLeadEquipByID.lstTable.ToDataSet();
                            ds2 = _lstGetLeadEquipByID.lstTable1.ToDataSet();
                            ds3 = _lstGetLeadEquipByID.lstTable2.ToDataSet();
                            ds4 = _lstGetLeadEquipByID.lstTable3.ToDataSet();
                            ds5 = _lstGetLeadEquipByID.lstTable4.ToDataSet();

                            DataTable dt1 = new DataTable();
                            DataTable dt2 = new DataTable();
                            DataTable dt3 = new DataTable();
                            DataTable dt4 = new DataTable();
                            DataTable dt5 = new DataTable();

                            dt1 = ds1.Tables[0];
                            dt2 = ds2.Tables[0];
                            dt3 = ds3.Tables[0];
                            dt4 = ds4.Tables[0];
                            dt5 = ds5.Tables[0];

                            dt1.TableName = "Table1";
                            dt2.TableName = "Table2";
                            dt3.TableName = "Table3";
                            dt4.TableName = "Table4";
                            dt5.TableName = "Table5";

                            ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy(), dt4.Copy(), dt5.Copy() });
                        }
                        else
                        {
                            ds = objBL_User.getLeadEquipByID(objPropUser);
                        }

                        ShowMCPTabs(false);
                    }
                    else
                    {
                        ListGetequipByID _listGetequipByID = new ListGetequipByID();

                        if (IsAPIIntegrationEnable == "YES")
                        {
                            string APINAME = "EquipmentAPI/AddEquipment_GetequipByID";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetequipByID);

                            JavaScriptSerializer serializer = new JavaScriptSerializer();

                            serializer.MaxJsonLength = Int32.MaxValue;

                            _listGetequipByID = serializer.Deserialize<ListGetequipByID>(_APIResponse.ResponseData);

                            ds1 = _listGetequipByID.lstTable1.ToDataSet();
                            ds2 = _listGetequipByID.lstTable2.ToDataSet();
                            ds3 = _listGetequipByID.lstTable3.ToDataSet();
                            ds4 = _listGetequipByID.lstTable4.ToDataSet();
                            ds5 = _listGetequipByID.lstTable5.ToDataSet();

                            ds3.Tables[0].Columns["OrderNo1"].ColumnName = "OrderNo";

                            DataTable dt1 = new DataTable();
                            DataTable dt2 = new DataTable();
                            DataTable dt3 = new DataTable();
                            DataTable dt4 = new DataTable();
                            DataTable dt5 = new DataTable();

                            dt1 = ds1.Tables[0];
                            dt2 = ds2.Tables[0];
                            dt3 = ds3.Tables[0];
                            dt4 = ds4.Tables[0];
                            dt5 = ds5.Tables[0];

                            dt1.TableName = "Table1";
                            dt2.TableName = "Table2";
                            dt3.TableName = "Table3";
                            dt4.TableName = "Table4";
                            dt5.TableName = "Table5";

                            ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy(), dt4.Copy(), dt5.Copy() });
                        }
                        else
                        {
                            ds = objBL_User.getequipByID(objPropUser);
                        }
                        ShowMCPTabs(true);
                    }
                }
                else
                {
                    ListGetequipByID _lstGetequipByID = new ListGetequipByID();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "EquipmentAPI/AddEquipment_GetequipByID";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetequipByID);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        serializer.MaxJsonLength = Int32.MaxValue;

                        _lstGetequipByID = serializer.Deserialize<ListGetequipByID>(_APIResponse.ResponseData);

                        ds1 = _lstGetequipByID.lstTable1.ToDataSet();
                        ds2 = _lstGetequipByID.lstTable2.ToDataSet();
                        ds3 = _lstGetequipByID.lstTable3.ToDataSet();
                        ds4 = _lstGetequipByID.lstTable4.ToDataSet();
                        ds5 = _lstGetequipByID.lstTable5.ToDataSet();

                        ds3.Tables[0].Columns["OrderNo1"].ColumnName = "OrderNo";
                        DataTable dt1 = new DataTable();
                        DataTable dt2 = new DataTable();
                        DataTable dt3 = new DataTable();
                        DataTable dt4 = new DataTable();
                        DataTable dt5 = new DataTable();

                        dt1 = ds1.Tables[0];
                        dt2 = ds2.Tables[0];
                        dt3 = ds3.Tables[0];
                        dt4 = ds4.Tables[0];
                        dt5 = ds5.Tables[0];

                        dt1.TableName = "Table1";
                        dt2.TableName = "Table2";
                        dt3.TableName = "Table3";
                        dt4.TableName = "Table4";
                        dt5.TableName = "Table5";

                        ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy(), dt4.Copy(), dt5.Copy() });
                    }
                    else
                    {
                        ds = objBL_User.getequipByID(objPropUser);
                    }
                    ShowMCPTabs(true);
                }


                Session["PageType"] = "Equip";
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        int DaysinMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - 1;
                        DateTime lastDay = firstDay.AddDays(DaysinMonth);
                        txtfromDate.Text = firstDay.ToShortDateString();
                        txtToDate.Text = lastDay.ToShortDateString();

                        
                        liTest.Style["display"] = "inline-block";
                        liLogs.Style["display"] = "inline-block";
                        liShutdownLogs.Style["display"] = "inline-block";
                        lidocuments.Style["display"] = "inline-block";
                        tbTests.Style["display"] = "block";
                        tbLogs.Style["display"] = "block";
                        tbShutdownLogs.Style["display"] = "block";
                        dvDocuments.Style["display"] = "block";

                        lblEquipName.Text = ds.Tables[0].Rows[0]["Unit"].ToString();
                        if (Request.QueryString["page"] != null)
                        {
                            if (Request.QueryString["page"].ToString() == "addprospect")
                            {
                                hdnLocId.Value = ds.Tables[0].Rows[0]["lead"].ToString();
                                hdnLocPrevious.Value = ds.Tables[0].Rows[0]["lead"].ToString();
                            }

                            else
                            {
                                txtLocation.Text = ds.Tables[0].Rows[0]["location"].ToString();
                                hdnLocId.Value = ds.Tables[0].Rows[0]["loc"].ToString();
                                hdnLocPrevious.Value = ds.Tables[0].Rows[0]["loc"].ToString();
                                txtCompany.Text = ds.Tables[0].Rows[0]["Company"].ToString();
                            }
                        }
                        else
                        {
                            txtLocation.Text = ds.Tables[0].Rows[0]["location"].ToString();
                            hdnLocId.Value = ds.Tables[0].Rows[0]["loc"].ToString();
                            hdnLocPrevious.Value = ds.Tables[0].Rows[0]["loc"].ToString();
                            txtCompany.Text = ds.Tables[0].Rows[0]["Company"].ToString();
                        }


                        txtEquipID.Text = ds.Tables[0].Rows[0]["unit"].ToString();
                        txtDesc.Text = ds.Tables[0].Rows[0]["fdesc"].ToString();
                        ddlType.SelectedValue = ds.Tables[0].Rows[0]["type"].ToString();
                        ddlServiceType.SelectedValue = ds.Tables[0].Rows[0]["cat"].ToString();
                        txtManuf.Text = ds.Tables[0].Rows[0]["manuf"].ToString();
                        txtSerial.Text = ds.Tables[0].Rows[0]["serial"].ToString();
                        txtUnique.Text = ds.Tables[0].Rows[0]["state"].ToString();
                        txtPrice.Text = ds.Tables[0].Rows[0]["price"].ToString();
                        ddlCategory.SelectedValue = ds.Tables[0].Rows[0]["category"].ToString();
                        ddlBuilding.SelectedValue = ds.Tables[0].Rows[0]["Building"].ToString();
                        ddlClassification.SelectedValue = ds.Tables[0].Rows[0]["Classification"].ToString();
                        ddlCustTemplate.SelectedValue = ds.Tables[0].Rows[0]["template"].ToString();
                        hdnSelectedVal.Value = ds.Tables[0].Rows[0]["template"].ToString();
                        if (ds.Tables[0].Rows[0]["install"] != DBNull.Value)
                        {
                            txtInstalled.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["install"]).ToShortDateString();
                        }

                        chkShutdown.Checked = ds.Tables[0].Rows[0]["shut_down"] == DBNull.Value ? false : Convert.ToBoolean(ds.Tables[0].Rows[0]["shut_down"]);
                        // Keep current shut_down status of equipment in a viewstate
                        ViewState["shut_down"] = chkShutdown.Checked;
                        hdnEqShutdownStatus.Value = chkShutdown.Checked ? "1" : "0";
                        txtShutdownReason.Text = ds.Tables[0].Rows[0]["ShutdownReason"].ToString();

                        if(chkShutdown.Checked) {
                            divShutdownReason.Visible = false;
                            divShutdownReasonDesc.Visible = true;
                            txtShutdownReason.Enabled = false;
                        }
                        else
                        {
                            divShutdownReason.Visible = false;
                            divShutdownReasonDesc.Visible = false;
                        }

                        if (ds.Tables[0].Rows[0]["last"] != DBNull.Value)
                        {
                            txtLast.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["last"]).ToShortDateString();
                        }
                        if (ds.Tables[0].Rows[0]["since"] != DBNull.Value)
                        {
                            txtSince.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["since"]).ToShortDateString();
                        }
                        rbStatus.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
                        txtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();

                        pnlQR.Visible = true;
                      
                        if (Request.QueryString["page"] != null)
                        {
                            if (Request.QueryString["page"].ToString() != "addprospect")
                            {
                                string strQRString = ds.Tables[0].Rows[0]["locationID"].ToString() + ":" + ds.Tables[0].Rows[0]["unit"].ToString() + ":" + ds.Tables[0].Rows[0]["loc"].ToString() + ":" + ds.Tables[0].Rows[0]["unitid"].ToString();
                                imgQR.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(GenerateQR(strQRString));
                            }
                        }
                        else
                        {
                            string strQRString = ds.Tables[0].Rows[0]["locationID"].ToString() + ":" + ds.Tables[0].Rows[0]["unit"].ToString() + ":" + ds.Tables[0].Rows[0]["loc"].ToString() + ":" + ds.Tables[0].Rows[0]["unitid"].ToString();
                            imgQR.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(GenerateQR(strQRString));
                        }

                        GetContractType();

                        RadGrid_gvTemplateItems.VirtualItemCount = ds.Tables[1].Rows.Count;
                        RadGrid_gvTemplateItems.DataSource = ds.Tables[1];


                        RadGrid_gvCtemplItems.VirtualItemCount = ds.Tables[2].Rows.Count;
                        RadGrid_gvCtemplItems.DataSource = ds.Tables[2];
                        RadGrid_gvCtemplItems.Rebind();

                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            Session["dsCust"] = ds.Tables[3];
                            binditemgrid(ds.Tables[3]);
                        }


                        if (Request.QueryString["t"] != null)
                        {
                            txtEquipID.Text = string.Empty;
                            lblEquipName.Text = "Add Equipment";
                            dvCompanyPermission.Visible = false;
                        }
                        else
                        {
                            fillREPHistory();
                        }

                        if (Request.QueryString["page"] != null)
                        {
                            if (Request.QueryString["page"].ToString() == "addlocation")
                            {
                                //txtLocation.Text = ds.Tables[0].Rows[0]["location"].ToString();
                                //hdnLocId.Value = ds.Tables[0].Rows[0]["location"].ToString();
                                //hdnLocPrevious.Value = ds.Tables[0].Rows[0]["location"].ToString();
                            }

                            else
                            {
                                lnkAddTicket.NavigateUrl = "AddTicket.aspx?locid=" + hdnLocId.Value + "&unitid=" + Request.QueryString["uid"].ToString() + "&unit=" + lblEquipName.Text;
                            }
                        }
                        else
                        {
                            lnkAddTicket.NavigateUrl = "AddTicket.aspx?locid=" + hdnLocId.Value + "&unitid=" + Request.QueryString["uid"].ToString() + "&unit=" + lblEquipName.Text;
                        }


                    }
                }
                else
                {
                    ViewState["shut_down"] = false;
                    hdnEqShutdownStatus.Value = "0";
                }

                 
                hdnShutdownReasonPlanned.Value = "0";

               

                GetTests();
                if (Request.Cookies[CookieName] != null)
                {
                    RadPersistenceManager1.LoadState();
                    RadGrid_gvtests.Rebind();
                }
            }

            Locstatus();
        }
        Permission();
        CompanyPermission();
        HighlightSideMenu("cstmMgr", "lnkEquipmentsSMenu", "cstmMgrSub");
        if (Request.QueryString["uid"] != null && Session["addequipmentStatus"] !=null && Session["addequipmentStatus"].ToString()== "success")
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Equipment added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            Session["addequipmentStatus"] = null;
        }
     }


    public void Locstatus()
    {

        if (hdnLocId.Value != "" && Request.QueryString["page"] != "addprospect")
        { 

            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.LocID = Convert.ToInt32(hdnLocId.Value);
            objPropUser.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            ds = objBL_User.getLocationByID(objPropUser);


            string LoCStatus = ds.Tables[0].Rows[0]["status"].ToString();
            if (LoCStatus == "1")
            {
                btnSubmit.Visible = false;
                ClientScript.RegisterStartupScript(Page.GetType(), "keyerrrinactive", "noty({text: 'Location is inactive!.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }

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
    public bool CheckAddEditPermission()
    {
        bool result = true;
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];
            ds = GetUserById();
            /// Elevator Permission ///////////////////------->

            string ElevatorPermission = ds.Rows[0]["Elevator"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Elevator"].ToString();
            string stAdde = ElevatorPermission.Length < 1 ? "Y" : ElevatorPermission.Substring(0, 1);
            string stEdite = ElevatorPermission.Length < 2 ? "Y" : ElevatorPermission.Substring(1, 1);
            string DeleteElev = ElevatorPermission.Length < 3 ? "Y" : ElevatorPermission.Substring(2, 1);
            string ViewElev = ElevatorPermission.Length < 4 ? "Y" : ElevatorPermission.Substring(3, 1);


            if (ViewElev == "N")
            {
                result = false;
            }
            else if (Request.QueryString["uid"] == null)
            {
                if (stAdde == "N")
                {
                    result = false;
                }
            }
            else if (stEdite == "N")
            {
                if (ViewElev == "Y")
                {
                    btnSubmit.Visible = false;
                }
                else
                {
                    result = false;
                }
            }
        }

        return result;
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
    private byte[] GenerateQR(string QR)
    {
        byte[] QRbytes = null;
        var qrValue = QR;
        if (qrValue.ToString().Trim() != string.Empty)
        {
            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 200,
                    Width = 200,
                    Margin = 1
                }
            };

            using (var bitmap = barcodeWriter.Write(qrValue))
            using (var stream = new System.IO.MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                QRbytes = stream.GetBuffer();
            }
        }
        return QRbytes;
    }

    private void Permission()
    {
        HyperLink li = (HyperLink)Page.Master.FindControl("cstmMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("cstmlink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkEquipmentsSmenu");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderCstm");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("cstmMgrSub");
        //ul.Style.Add("display", "block");
        //ul.Style.Add("visibility", "visible");

        if (Session["type"].ToString() == "c")
        {
            //Response.Redirect("home.aspx");
            //pnlSave.Visible = false;
            lblHeader.Text = "Equipment";
            btnSubmit.Visible = false;
          //ac  pnlEquipments.Enabled = false;
            tpCustom.Style["display"] = "none";
            liCustom.Style["display"] = "none";
            tpREP.Style["display"] = "none";
            liMCPTemp.Style["display"] = "none";
            lnkAddTicket.Visible = false;
            lnkDelTest.Visible = false;
            lnkAddTest.Visible = false;
            lnkDeleteDoc.Visible = false;

        }

        if (Session["MSM"].ToString() == "TS")
        {
            //Response.Redirect("home.aspx");
            //btnSubmit.Visible = false;
          //ac  pnlEquipments.Enabled = false;
        }

        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            //Response.Redirect("home.aspx");
        }
        if (Session["type"].ToString() != "am")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];

            /// Ticket ///////////////////------->

            string ticketPermission = ds.Rows[0]["TicketPermission"] == DBNull.Value ? "YYNYYY" : ds.Rows[0]["TicketPermission"].ToString();
            hdnAddeTicket.Value = ticketPermission.Length < 1 ? "Y" : ticketPermission.Substring(0, 1);
        }
    }

    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            dvCompanyPermission.Visible = true;  
        }
        else
        {
            dvCompanyPermission.Visible = false;
        }
    }
    private void FillBuilding()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        _GetBuildingLeadEquip.ConnConfig = Session["config"].ToString();
        _GetBuildingElev.ConnConfig = Session["config"].ToString();

        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString() == "addprospect")
            {
                List<GetBuildingElevViewModel> _lstGetBuildingLeadEquip = new List<GetBuildingElevViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "EquipmentAPI/AddEquipment_GetBuildingLeadEquip";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetBuildingLeadEquip);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetBuildingLeadEquip = serializer.Deserialize<List<GetBuildingElevViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetBuildingElevViewModel>(_lstGetBuildingLeadEquip);
                }
                else
                {
                    ds = objBL_User.getBuildingLeadEquip(objPropUser);
                }
            }

            else
            {
                List<GetBuildingElevViewModel> _lstGetBuildingElev = new List<GetBuildingElevViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "EquipmentAPI/EquipmentsList_GetBuildingElev";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetBuildingElev);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetBuildingElev = serializer.Deserialize<List<GetBuildingElevViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetBuildingElevViewModel>(_lstGetBuildingElev);
                }
                else
                {
                    ds = objBL_User.getBuildingElev(objPropUser);
                }
            }
        }
        else
        {
            List<GetBuildingElevViewModel> _lstGetBuildingElev = new List<GetBuildingElevViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentsList_GetBuildingElev";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetBuildingElev);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetBuildingElev = serializer.Deserialize<List<GetBuildingElevViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetBuildingElevViewModel>(_lstGetBuildingElev);
            }
            else
            {
                ds = objBL_User.getBuildingElev(objPropUser);
            }

        }
        
        
        ddlBuilding.DataSource = ds.Tables[0];
        ddlBuilding.DataTextField = "EDesc";
        ddlBuilding.DataValueField = "EDesc";
        ddlBuilding.DataBind();
        ddlBuilding.Items.Insert(0, new ListItem("None", ""));
        lblBuilding.Text = "Building";
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            lblBuilding.Text = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }
    private void fillREPHistory()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.EquipID = Convert.ToInt32(Request.QueryString["uid"]);
        objPropUser.SearchBy = ddlSearch.SelectedValue;

        _GetequipREPDetails.ConnConfig = Session["config"].ToString();
        _GetequipREPDetails.EquipID = Convert.ToInt32(Request.QueryString["uid"]);
        _GetequipREPDetails.SearchBy = ddlSearch.SelectedValue;
        if (ddlSearch.SelectedValue == "template")
        {
            objPropUser.SearchValue = ddlTemplate.SelectedValue;
            _GetequipREPDetails.SearchValue = ddlTemplate.SelectedValue;
        }
        else if (ddlSearch.SelectedValue == "rd.Code")
        {
            objPropUser.SearchValue = txtCodeSearch.Text.Trim();
            _GetequipREPDetails.SearchValue = txtCodeSearch.Text.Trim();
        }
        else if (ddlSearch.SelectedValue == "eti.frequency")
        {
            objPropUser.SearchValue = ddlFreq.SelectedValue;
            _GetequipREPDetails.SearchValue = ddlFreq.SelectedValue;
        }
        else
        {
            objPropUser.SearchValue = txtSearch.Text.Trim().Replace("'", "''");
            _GetequipREPDetails.SearchValue = txtSearch.Text.Trim().Replace("'", "''");
        }
        objPropUser.Status = Convert.ToInt16(ddlDates.SelectedValue);
        objPropUser.StartDate = txtfromDate.Text.Trim();
        objPropUser.EndDate = txtToDate.Text.Trim();

        _GetequipREPDetails.Status = Convert.ToInt16(ddlDates.SelectedValue);
        _GetequipREPDetails.StartDate = txtfromDate.Text.Trim();
        _GetequipREPDetails.EndDate = txtToDate.Text.Trim();
        if (Session["type"].ToString() == "c")
        {
            objPropUser.Cust = 1;
            _GetequipREPDetails.Cust = 1;
        }
        else
        {
            objPropUser.Cust = 0;
            _GetequipREPDetails.Cust = 0;
        }
        DataSet ds = new DataSet();

        List<GetequipREPDetailsViewModel> _lstGetequipREPDetails = new List<GetequipREPDetailsViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/AddEquipment_GetequipREPDetails";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetequipREPDetails);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetequipREPDetails = serializer.Deserialize<List<GetequipREPDetailsViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetequipREPDetailsViewModel>(_lstGetequipREPDetails);
        }
        else
        {
            ds = objBL_User.getequipREPDetails(objPropUser);
        }

        FillREPDetails(ds.Tables[0]);
    }

    protected void GetCustomTemplate()
    {
        objPropCustomer.ConnConfig = Session["config"].ToString();
        _GetCustomTemplate.ConnConfig = Session["config"].ToString();

        DataSet ds = new DataSet();
        List<GetCustomTemplateViewModel> _lstGetCustomTemplate = new List<GetCustomTemplateViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/AddEquipment_GetCustomTemplate";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomTemplate);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetCustomTemplate = serializer.Deserialize<List<GetCustomTemplateViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetCustomTemplateViewModel>(_lstGetCustomTemplate);
        }
        else
        {
            ds = objBL_Customer.getCustomTemplate(objPropCustomer);
        }

        ddlCustTemplate.DataSource = ds.Tables[0];
        ddlCustTemplate.DataTextField = "Fdesc";
        ddlCustTemplate.DataValueField = "ID";
        ddlCustTemplate.DataBind();
        ddlCustTemplate.Items.Insert(0, new ListItem("--Select--", "0"));
        ddlCustTemplate.SelectedIndex = 0;
    }
    private void FillEquipCategory()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        _GetEquipmentCategory.ConnConfig = Session["config"].ToString();
        _GetLeadEquipmentCategory.ConnConfig = Session["config"].ToString();

        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString() == "addprospect")
            {
                List<GetEquiptypeViewModel> _lstGetEquiptype = new List<GetEquiptypeViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "EquipmentAPI/AddEquipment_GetLeadEquipmentCategory";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetLeadEquipmentCategory);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetEquiptype = serializer.Deserialize<List<GetEquiptypeViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetEquiptypeViewModel>(_lstGetEquiptype);
                }
                else
                {
                    ds = objBL_User.getLeadEquipmentCategory(objPropUser);
                }
            }

            else
            {
                List<GetEquiptypeViewModel> _lstGetEquiptype = new List<GetEquiptypeViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "EquipmentAPI/EquipmentsList_GetEquipmentCategory";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetEquipmentCategory);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetEquiptype = serializer.Deserialize<List<GetEquiptypeViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetEquiptypeViewModel>(_lstGetEquiptype);
                }
                else
                {
                    ds = objBL_User.getEquipmentCategory(objPropUser);
                }
            }
        }
        else
        {
            List<GetEquiptypeViewModel> _lstGetEquiptype = new List<GetEquiptypeViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentsList_GetEquipmentCategory";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetEquipmentCategory);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetEquiptype = serializer.Deserialize<List<GetEquiptypeViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetEquiptypeViewModel>(_lstGetEquiptype);
            }
            else
            {
                ds = objBL_User.getEquipmentCategory(objPropUser);
            }
        }
        
        
        ddlCategory.DataSource = ds.Tables[0];
        ddlCategory.DataTextField = "edesc";
        ddlCategory.DataValueField = "edesc";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, new ListItem("None", "None"));
        //ddlCategory.Items.Add(new ListItem("New", "New"));
        //ddlCategory.Items.Add(new ListItem("Refurbished", "Refurbished"));
        lblCategory.Text = "Category";
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            lblCategory.Text = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }
    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSearch.SelectedValue == "template")
        {
            txtSearch.Visible = false;
            ddlTemplate.Visible = true;
            txtCodeSearch.Visible = false;
            ddlFreq.Visible = false;
        }
        else if (ddlSearch.SelectedValue == "rd.Code")
        {
            txtSearch.Visible = false;
            ddlTemplate.Visible = false;
            txtCodeSearch.Visible = true;
            ddlFreq.Visible = false;
        }
        else if (ddlSearch.SelectedValue == "eti.frequency")
        {
            txtSearch.Visible = false;
            ddlTemplate.Visible = false;
            txtCodeSearch.Visible = false;
            ddlFreq.Visible = true;
        }
        else
        {
            txtSearch.Visible = true;
            ddlTemplate.Visible = false;
            txtCodeSearch.Visible = false;
            ddlFreq.Visible = false;
        }
    }

    private void FillREPDetails(DataTable dt)
    {
        Session["dtREPDetail"] = dt;
        if (dt != null)
        {
            RadGrid_gvRepDetails.VirtualItemCount = dt.Rows.Count;
            RadGrid_gvRepDetails.DataSource = dt;
           

            lblRecordCountHist.Text = dt.Rows.Count + " record(s) found.";
        }
    }

    private DataTable GetREPDatafromSession()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtREPDetail"];
        return dt;
    }

    private void FillGridfromSession()
    {
        DataTable dt = new DataTable();
        dt = GetREPDatafromSession();
        FillREPDetails(dt);
    }

  


    private void FillEquiptype()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        _GetEquiptype.ConnConfig = Session["config"].ToString();
        _GetLeadEquiptype.ConnConfig = Session["config"].ToString();
        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString() == "addprospect")
            {
                List<GetEquiptypeViewModel> _lstGetEquiptype = new List<GetEquiptypeViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "EquipmentAPI/AddEquipment_GetLeadEquiptype";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetLeadEquiptype);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetEquiptype = serializer.Deserialize<List<GetEquiptypeViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetEquiptypeViewModel>(_lstGetEquiptype);
                }
                else
                {
                    ds = objBL_User.getLeadEquiptype(objPropUser);
                }
            }

            else
            {
                List<GetEquiptypeViewModel> _lstGetEquiptype = new List<GetEquiptypeViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "EquipmentAPI/EquipmentsList_GetEquiptype";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetEquiptype);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetEquiptype = serializer.Deserialize<List<GetEquiptypeViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetEquiptypeViewModel>(_lstGetEquiptype);
                }
                else
                {
                    ds = objBL_User.getEquiptype(objPropUser);
                }
            }
        }
        else
        {
            List<GetEquiptypeViewModel> _lstGetEquiptype = new List<GetEquiptypeViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentsList_GetEquiptype";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetEquiptype);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetEquiptype = serializer.Deserialize<List<GetEquiptypeViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetEquiptypeViewModel>(_lstGetEquiptype);
            }
            else
            {
                ds = objBL_User.getEquiptype(objPropUser);
            }
        }
       
        
        ddlType.DataSource = ds.Tables[0];
        ddlType.DataTextField = "edesc";
        ddlType.DataValueField = "edesc";
        ddlType.DataBind();
        ddlType.Items.Insert(0, new ListItem("None", "None"));
        lblType.Text = "Type";
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            lblType.Text = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }

    private void FillServiceType()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        _GetActiveServiceType.ConnConfig = Session["config"].ToString();

        //if (Request.QueryString["page"] != null)
        //{
        //    if (Request.QueryString["page"].ToString() == "addprospect")
        //    {
        //        ds = objBL_User.GetLeadServiceType(objPropUser);
        //    }
        //    else
        //    {
        //        ds = objBL_User.GetServiceType(objPropUser);
        //    }
        //}
        //else
        //{
        //    ds = objBL_User.GetServiceType(objPropUser);
        //}

        List<GetActiveServiceTypeViewModel> _lstGetActiveServiceType = new List<GetActiveServiceTypeViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/EquipmentsList_GetActiveServiceType";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetActiveServiceType);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetActiveServiceType = serializer.Deserialize<List<GetActiveServiceTypeViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetActiveServiceTypeViewModel>(_lstGetActiveServiceType);
        }
        else
        {
            ds = new BusinessLayer.Programs.BL_ServiceType().GetActiveServiceType(objPropUser.ConnConfig);
        }

        ddlServiceType.DataSource = ds.Tables[0];
        ddlServiceType.DataTextField = "type";
        ddlServiceType.DataValueField = "type";
        ddlServiceType.DataBind();
        ddlServiceType.Items.Insert(0, new ListItem("None", "None"));
    }

    private void FillRepTemplate()
    {
        DataSet ds = new DataSet();
        objPropCustomer.ConnConfig = Session["config"].ToString();
        _GetRepTemplateName.ConnConfig = Session["config"].ToString();

        List<GetRepTemplateNameViewModel> _lstGetRepTemplateName = new List<GetRepTemplateNameViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/MassMCP_GetRepTemplateName";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetRepTemplateName);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetRepTemplateName = serializer.Deserialize<List<GetRepTemplateNameViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetRepTemplateNameViewModel>(_lstGetRepTemplateName);
        }
        else
        {
            ds = objBL_Customer.getRepTemplateName(objPropCustomer);
        }

        RadGrid_gvSelectTemplate.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_gvSelectTemplate.DataSource = ds.Tables[0];
        //gvSelectTemplate.DataSource = ds;
        //gvSelectTemplate.DataBind();

        ddlRepTemp.DataSource = ds;
        ddlRepTemp.DataTextField = "fdesc";
        ddlRepTemp.DataValueField = "id";
        ddlRepTemp.DataBind();

        ddlRepTemp.Items.Insert(0, new ListItem("--Select--", ""));

        ddlTemplate.DataSource = ds;
        ddlTemplate.DataTextField = "fdesc";
        ddlTemplate.DataValueField = "id";
        ddlTemplate.DataBind();

        ddlTemplate.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void lnkLast_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();

        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString() == "addcustomer")
            {
                dt = (DataTable)Session["ElevSrchCust"];
            }
            else if (Request.QueryString["page"].ToString() == "addlocation")
            {
                dt = (DataTable)Session["ElevSrchLoc"];
            }
            else if (Request.QueryString["page"].ToString() == "addprospect")
            {
                dt = (DataTable)Session["ElevSrchLead"];
            }
            else
            {
                dt = (DataTable)Session["ElevSrch"];
            }
        }
        else
        {
            dt = (DataTable)Session["ElevSrch"];
        }

        string url = "addequipment.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["id"];
        string screenid = string.Empty;
        if (Request.QueryString["cuid"] != null)
        {
            screenid = "&cuid=" + Request.QueryString["cuid"].ToString();
        }
        else if (Request.QueryString["lid"] != null)
        {
            screenid = "&lid=" + Request.QueryString["lid"].ToString();
        }
        if (Request.QueryString["page"] != null)
        {
            url += "&page=" + Request.QueryString["page"].ToString() + screenid;
        }
        Response.Redirect(url);
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();

        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString() == "addcustomer")
            {
                dt = (DataTable)Session["ElevSrchCust"];
            }
            else if (Request.QueryString["page"].ToString() == "addlocation")
            {
                dt = (DataTable)Session["ElevSrchLoc"];
            }
            else if (Request.QueryString["page"].ToString() == "addprospect")
            {
                dt = (DataTable)Session["ElevSrchLead"];
            }
            else
            {
                dt = (DataTable)Session["ElevSrch"];
            }
        }
        else
        {
            dt = (DataTable)Session["ElevSrch"];
        }

        string url = "addequipment.aspx?uid=" + dt.Rows[0]["id"];
        string screenid = string.Empty;
        if (Request.QueryString["cuid"] != null)
        {
            screenid = "&cuid=" + Request.QueryString["cuid"].ToString();
        }
        else if (Request.QueryString["lid"] != null)
        {
            screenid = "&lid=" + Request.QueryString["lid"].ToString();
        }
        if (Request.QueryString["page"] != null)
        {
            url += "&page=" + Request.QueryString["page"].ToString() + screenid;
        }
        Response.Redirect(url);

    }

    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();

        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString() == "addcustomer")
            {
                dt = (DataTable)Session["ElevSrchCust"];
            }
            else if (Request.QueryString["page"].ToString() == "addlocation")
            {
                dt = (DataTable)Session["ElevSrchLoc"];
            }
            else if (Request.QueryString["page"].ToString() == "addprospect")
            {
                dt = (DataTable)Session["ElevSrchLead"];
            }
            else
            {
                dt = (DataTable)Session["ElevSrch"];
            }
        }
        else
        {
            dt = (DataTable)Session["ElevSrch"];
        }

        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["ID"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);
        int item = 0;
        if (index >0 )
        {
            item = index - 1;
        }
       

       
            string url = "addequipment.aspx?uid=" + dt.Rows[item]["id"];
            string screenid = string.Empty;
            if (Request.QueryString["cuid"] != null)
            {
                screenid = "&cuid=" + Request.QueryString["cuid"].ToString();
            }
            else if (Request.QueryString["lid"] != null)
            {
                screenid = "&lid=" + Request.QueryString["lid"].ToString();
            }
            if (Request.QueryString["page"] != null)
            {
                url += "&page=" + Request.QueryString["page"].ToString() + screenid;
            }
            Response.Redirect(url);
       
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();

        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString() == "addcustomer")
            {
                dt = (DataTable)Session["ElevSrchCust"];
            }
            else if (Request.QueryString["page"].ToString() == "addlocation")
            {
                dt = (DataTable)Session["ElevSrchLoc"];
            }
            else if (Request.QueryString["page"].ToString() == "addprospect")
            {
                dt = (DataTable)Session["ElevSrchLead"];
            }
            else
            {
                dt = (DataTable)Session["ElevSrch"];
            }
        }
        else
        {
            dt = (DataTable)Session["ElevSrch"];
        }

        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["ID"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);
        int c = dt.Rows.Count - 1;
        int item = 0;
        if (index < c)
        {
            item = index + 1;
        }
        else
        {
            item = index;
        }

       
            string url = "addequipment.aspx?uid=" + dt.Rows[item]["id"];
            string screenid = string.Empty;
            if (Request.QueryString["cuid"] != null)
            {
                screenid = "&cuid=" + Request.QueryString["cuid"].ToString();
            }
            else if (Request.QueryString["lid"] != null)
            {
                screenid = "&lid=" + Request.QueryString["lid"].ToString();
            }
            if (Request.QueryString["page"] != null)
            {
                url += "&page=" + Request.QueryString["page"].ToString() + screenid;
            }
            Response.Redirect(url);
        
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
      
        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString().ToLower() == "addprospect")
            {
                Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + Request.QueryString["lid"] + "&tab=equip");
            }
            if (Request.QueryString["page"].ToString().ToLower() == "addcustomer")
            {
                Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + Request.QueryString["cuid"] + "&tab=equip");
            }
            if (Request.QueryString["page"].ToString().ToLower() == "addlocation")
            {
                Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + Request.QueryString["lid"] + "&tab=equip");
            }

            if (Request.QueryString["page"].ToString().ToLower() == "AddTests.aspx".ToLower())
            {
                String url = Request.QueryString["page"];
                String para = String.Empty;
                string elv = string.Empty;
                string lid = string.Empty;
                if (Request.QueryString["elv"] != null)
                {
                    para = "?elv=" + Request.QueryString["elv"].ToString();

                    if (Request.QueryString["lid"] != null)
                    {
                        para = para + "&lid=" + Request.QueryString["lid"].ToString();
                    }
                }
                else
                {
                    if (Request.QueryString["lid"] != null)
                    {
                        para = "?lid=" + Request.QueryString["lid"].ToString();
                    }
                }
                Response.Redirect(Request.QueryString["page"].ToString() + para);
            }

            
        }
        else
        {
            Response.Redirect("equipments.aspx");
        }
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if(IsAPIIntegrationEnable == "YES")
            {
                //API
                _UpdateLeadEquipment.LocID = Convert.ToInt32(hdnLocId.Value);
                _UpdateLeadEquipment.Unit = txtEquipID.Text;
                _UpdateLeadEquipment.Remarks = txtRemarks.Text;
                _UpdateLeadEquipment.Type = ddlType.SelectedValue;
                _UpdateLeadEquipment.Cat = ddlServiceType.SelectedValue;
                _UpdateLeadEquipment.Manufacturer = txtManuf.Text;
                _UpdateLeadEquipment.Serial = txtSerial.Text;
                _UpdateLeadEquipment.UniqueID = txtUnique.Text;
                _UpdateLeadEquipment.Category = ddlCategory.SelectedValue;
                _UpdateLeadEquipment.building = ddlBuilding.SelectedValue;
                _UpdateLeadEquipment.Description = txtDesc.Text;
                _UpdateLeadEquipment.MOMUSer = Session["User"].ToString();
                _UpdateLeadEquipment.UserID = Convert.ToInt32(Session["userid"].ToString());
                _UpdateLeadEquipment.Classification = ddlClassification.SelectedValue;

                _UpdateEquipment.LocID = Convert.ToInt32(hdnLocId.Value);
                _UpdateEquipment.Unit = txtEquipID.Text;
                _UpdateEquipment.Remarks = txtRemarks.Text;
                _UpdateEquipment.Type = ddlType.SelectedValue;
                _UpdateEquipment.Cat = ddlServiceType.SelectedValue;
                _UpdateEquipment.Manufacturer = txtManuf.Text;
                _UpdateEquipment.Serial = txtSerial.Text;
                _UpdateEquipment.UniqueID = txtUnique.Text;
                _UpdateEquipment.Category = ddlCategory.SelectedValue;
                _UpdateEquipment.building = ddlBuilding.SelectedValue;
                _UpdateEquipment.Description = txtDesc.Text;
                _UpdateEquipment.MOMUSer = Session["User"].ToString();
                _UpdateEquipment.UserID = Convert.ToInt32(Session["userid"].ToString());
                _UpdateEquipment.Classification = ddlClassification.SelectedValue;

                _AddEquipmentForLead.LocID = Convert.ToInt32(hdnLocId.Value);
                _AddEquipmentForLead.Unit = txtEquipID.Text;
                _AddEquipmentForLead.Remarks = txtRemarks.Text;
                _AddEquipmentForLead.Type = ddlType.SelectedValue;
                _AddEquipmentForLead.Cat = ddlServiceType.SelectedValue;
                _AddEquipmentForLead.Manufacturer = txtManuf.Text;
                _AddEquipmentForLead.Serial = txtSerial.Text;
                _AddEquipmentForLead.UniqueID = txtUnique.Text;
                _AddEquipmentForLead.Category = ddlCategory.SelectedValue;
                _AddEquipmentForLead.building = ddlBuilding.SelectedValue;
                _AddEquipmentForLead.Description = txtDesc.Text;
                _AddEquipmentForLead.MOMUSer = Session["User"].ToString();
                _AddEquipmentForLead.UserID = Convert.ToInt32(Session["userid"].ToString());
                _AddEquipmentForLead.Classification = ddlClassification.SelectedValue;

                _AddEquipment.LocID = Convert.ToInt32(hdnLocId.Value);
                _AddEquipment.Unit = txtEquipID.Text;
                _AddEquipment.Remarks = txtRemarks.Text;
                _AddEquipment.Type = ddlType.SelectedValue;
                _AddEquipment.Cat = ddlServiceType.SelectedValue;
                _AddEquipment.Manufacturer = txtManuf.Text;
                _AddEquipment.Serial = txtSerial.Text;
                _AddEquipment.UniqueID = txtUnique.Text;
                _AddEquipment.Category = ddlCategory.SelectedValue;
                _AddEquipment.building = ddlBuilding.SelectedValue;
                _AddEquipment.Description = txtDesc.Text;
                _AddEquipment.MOMUSer = Session["User"].ToString();
                _AddEquipment.UserID = Convert.ToInt32(Session["userid"].ToString());
                _AddEquipment.Classification = ddlClassification.SelectedValue;

                //if (Request.QueryString["page"].ToString() == "addprospect")
                //{
                //    objPropUser.IsLeadEquip = true;
                //}
               
                _UpdateLeadEquipment.Shutdown = Convert.ToBoolean(chkShutdown.Checked);
                _UpdateLeadEquipment.ShutdownReason = txtShutdownReason.Text;

                _UpdateEquipment.Shutdown = Convert.ToBoolean(chkShutdown.Checked);
                _UpdateEquipment.ShutdownReason = txtShutdownReason.Text;

                _AddEquipmentForLead.Shutdown = Convert.ToBoolean(chkShutdown.Checked);
                _AddEquipmentForLead.ShutdownReason = txtShutdownReason.Text;

                _AddEquipment.Shutdown = Convert.ToBoolean(chkShutdown.Checked);
                _AddEquipment.ShutdownReason = txtShutdownReason.Text;

                //if (hdnShutdownReasonPlanned.Value == "1")
                //{
                //    objPropUser.ShutdownLongDesc = string.Empty;
                //}
                //else
                //{
                //    objPropUser.ShutdownLongDesc = hdnShutdownLongDesc.Value;
                //}
               
                _UpdateLeadEquipment.ShutdownLongDesc = hdnShutdownLongDesc.Value;
                _UpdateEquipment.ShutdownLongDesc = hdnShutdownLongDesc.Value;
                _AddEquipmentForLead.ShutdownLongDesc = hdnShutdownLongDesc.Value;
                _AddEquipment.ShutdownLongDesc = hdnShutdownLongDesc.Value;

                if (hdnShutdownReasonPlanned.Value == "1")
                {
                    
                    _UpdateEquipment.PlannedShutdown = true;
                    _AddEquipmentForLead.PlannedShutdown = true;
                }
                else
                {
                 
                    _UpdateEquipment.PlannedShutdown = false;
                    _AddEquipmentForLead.PlannedShutdown = false;
                }

                if (txtSince.Text.Trim() == string.Empty)
                {
                  
                    _UpdateLeadEquipment.InstallDateTime = System.DateTime.MinValue;
                    _UpdateEquipment.InstallDateTime = System.DateTime.MinValue;
                    _AddEquipmentForLead.InstallDateTime = System.DateTime.MinValue;
                    _AddEquipment.InstallDateTime = System.DateTime.MinValue;
                }
                else
                {
                  
                    _UpdateLeadEquipment.InstallDateTime = Convert.ToDateTime(txtSince.Text);
                    _UpdateEquipment.InstallDateTime = Convert.ToDateTime(txtSince.Text);
                    _AddEquipmentForLead.InstallDateTime = Convert.ToDateTime(txtSince.Text);
                    _AddEquipment.InstallDateTime = Convert.ToDateTime(txtSince.Text);
                }

                if (txtLast.Text.Trim() == string.Empty)
                {
                   
                    _UpdateLeadEquipment.LastServiceDate = System.DateTime.MinValue;
                    _UpdateEquipment.LastServiceDate = System.DateTime.MinValue;
                    _AddEquipmentForLead.LastServiceDate = System.DateTime.MinValue;
                    _AddEquipment.LastServiceDate = System.DateTime.MinValue;
                }
                else
                {
                
                    _UpdateLeadEquipment.LastServiceDate = Convert.ToDateTime(txtLast.Text);
                    _UpdateEquipment.LastServiceDate = Convert.ToDateTime(txtLast.Text);
                    _AddEquipmentForLead.LastServiceDate = Convert.ToDateTime(txtLast.Text);
                    _AddEquipment.LastServiceDate = Convert.ToDateTime(txtLast.Text);
                }

                if (txtInstalled.Text.Trim() == string.Empty)
                {
                   
                    _UpdateLeadEquipment.InstallDateimport = System.DateTime.MinValue;
                    _UpdateEquipment.InstallDateimport = System.DateTime.MinValue;
                    _AddEquipmentForLead.InstallDateimport = System.DateTime.MinValue;
                    _AddEquipment.InstallDateimport = System.DateTime.MinValue;
                }
                else
                {
                   
                    _UpdateLeadEquipment.InstallDateimport = Convert.ToDateTime(txtInstalled.Text);
                    _UpdateEquipment.InstallDateimport = Convert.ToDateTime(txtInstalled.Text);
                    _AddEquipmentForLead.InstallDateimport = Convert.ToDateTime(txtInstalled.Text);
                    _AddEquipment.InstallDateimport = Convert.ToDateTime(txtInstalled.Text);
                }

                if (txtPrice.Text.Trim() == string.Empty)
                {
                    
                    _UpdateLeadEquipment.EquipPrice = Convert.ToDouble("0.00");
                    _UpdateEquipment.EquipPrice = Convert.ToDouble("0.00");
                    _AddEquipmentForLead.EquipPrice = Convert.ToDouble("0.00");
                    _AddEquipment.EquipPrice = Convert.ToDouble("0.00");
                }
                else
                {
                  
                    _UpdateLeadEquipment.EquipPrice = Convert.ToDouble(txtPrice.Text);
                    _UpdateEquipment.EquipPrice = Convert.ToDouble(txtPrice.Text);
                    _AddEquipmentForLead.EquipPrice = Convert.ToDouble(txtPrice.Text);
                    _AddEquipment.EquipPrice = Convert.ToDouble(txtPrice.Text);
                }
               
                _UpdateLeadEquipment.Status = Convert.ToInt32(rbStatus.SelectedValue);
                _UpdateLeadEquipment.Remarks = txtRemarks.Text;

                _UpdateEquipment.Status = Convert.ToInt32(rbStatus.SelectedValue);
                _UpdateEquipment.Remarks = txtRemarks.Text;

                _AddEquipmentForLead.Status = Convert.ToInt32(rbStatus.SelectedValue);
                _AddEquipmentForLead.Remarks = txtRemarks.Text;

                _AddEquipment.Status = Convert.ToInt32(rbStatus.SelectedValue);
                _AddEquipment.Remarks = txtRemarks.Text;
                //if (Request.QueryString["page"] != null)
                //{
                //    if (Request.QueryString["page"].ToString() == "addprospect")
                //    {
                //        DataTable dt = CreateTableFromGridForLead();
                //        dt.Columns.Remove("Name");
                //        objPropUser.DtItems = dt;
                //        DataTable dtCustom = CreateCustomTemplateForLead();
                //        objPropUser.dtcustom = dtCustom;
                //    }

                //    else
                //    {
                //        DataTable dt = CreateTableFromGrid();
                //        dt.Columns.Remove("Name");
                //        objPropUser.DtItems = dt;
                //        DataTable dtCustom = CreateCustomTemplate();
                //        objPropUser.dtcustom = dtCustom;
                //    }
                //}
                //else
                //{
                DataTable dt = CreateTableFromGrid();
                dt.Columns.Remove("Name");
                
                if (dt.Rows.Count == 0)
                {
                    DataTable returnVal = TableFromGridEmptyDatatable();
                    _UpdateLeadEquipment.DtItems = returnVal;
                    _UpdateEquipment.DtItems = returnVal;
                    _AddEquipmentForLead.DtItems = returnVal;
                    _AddEquipment.DtItems = returnVal;
                }
                else
                {
                    _UpdateLeadEquipment.DtItems = dt;
                    _UpdateEquipment.DtItems = dt;
                    _AddEquipmentForLead.DtItems = dt;
                    _AddEquipment.DtItems = dt;
                }

                DataTable dtCustom = CreateCustomTemplate();
               
                if (dtCustom.Rows.Count == 0)
                {
                    DataTable returnVal = CustomTemplateEmptyDatatable();
                    _UpdateLeadEquipment.dtcustom = returnVal;
                    _UpdateEquipment.dtcustom = returnVal;
                    _AddEquipmentForLead.dtcustom = returnVal;
                    _AddEquipment.dtcustom = returnVal;
                }
                else
                {
                    _UpdateLeadEquipment.dtcustom = dtCustom;
                    _UpdateEquipment.dtcustom = dtCustom;
                    _AddEquipmentForLead.dtcustom = dtCustom;
                    _AddEquipment.dtcustom = dtCustom;
                }

                //}


               
                //objPropUser.UpdateTicket = (hdnUpdateTicket.Value != string.Empty) ? Convert.ToInt16(hdnUpdateTicket.Value) : 0;
                

                _UpdateLeadEquipment.CustomTemplateID = Convert.ToInt32(ddlCustTemplate.SelectedValue);
                _UpdateEquipment.CustomTemplateID = Convert.ToInt32(ddlCustTemplate.SelectedValue);
                _AddEquipmentForLead.CustomTemplateID = Convert.ToInt32(ddlCustTemplate.SelectedValue);
                _AddEquipment.CustomTemplateID = Convert.ToInt32(ddlCustTemplate.SelectedValue);

                _UpdateLeadEquipment.ConnConfig = Session["config"].ToString();
                _UpdateEquipment.ConnConfig = Session["config"].ToString();
                _AddEquipmentForLead.ConnConfig = Session["config"].ToString();
                _UpdateDocInfo.ConnConfig = Session["config"].ToString();
                _AddEquipment.ConnConfig = Session["config"].ToString();

                if (Convert.ToInt32(ViewState["mode"]) == 1)
                {
                    if (Session["MSM"].ToString() == "TS")
                    {
                        
                        _UpdateLeadEquipment.ItemsOnly = 1;
                        _UpdateEquipment.ItemsOnly = 1;
                        _AddEquipmentForLead.ItemsOnly = 1;
                        //_AddEquipment.ItemsOnly = 1;
                    }
                    
                    //document                
                    
                    _UpdateLeadEquipment.EquipID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                    _UpdateEquipment.EquipID = Convert.ToInt32(Request.QueryString["uid"].ToString());

                    //API

                    DataTable viewstatedata = SaveDocInfo();

                    if (viewstatedata.Rows.Count == 0)
                    {
                        DataTable returnVal = SaveDocInfoEmptyDatatable();
                        _UpdateDocInfo.dtDocs = returnVal;
                    }
                    else
                    {
                        _UpdateDocInfo.dtDocs = SaveDocInfo();
                    }

                    
                    string APINAME = "EquipmentAPI/AddEquipment_UpdateDocInfo";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateDocInfo);
                   

                    if (Request.QueryString["page"] != null)
                    {
                        if (Request.QueryString["page"].ToString() == "addprospect")
                        {
                            string APINAME1 = "EquipmentAPI/AddEquipment_UpdateLeadEquipment";

                            APIResponse _APIResponse1 = new MOMWebUtility().CallMOMWebAPI(APINAME1, _UpdateLeadEquipment);

                        }

                        else
                        {
                            string APINAME2 = "EquipmentAPI/AddEquipment_UpdateEquipment";

                            APIResponse _APIResponse2 = new MOMWebUtility().CallMOMWebAPI(APINAME2, _UpdateEquipment);
                        }
                    }
                    else
                    {
                       
                      string APINAME3 = "EquipmentAPI/AddEquipment_UpdateEquipment";

                      APIResponse _APIResponse3 = new MOMWebUtility().CallMOMWebAPI(APINAME3, _UpdateEquipment);
                        
                    }

                    hdnLocPrevious.Value = hdnLocId.Value;
                    Get_CustomData();
                    RadGrid_gvLogs.Rebind();

                    // Update shutdown reason and shut down view state after saving 
                    ViewState["shut_down"] = chkShutdown.Checked;
                    hdnEqShutdownStatus.Value = chkShutdown.Checked ? "1" : "0";
                    divShutdownReason.Visible = false;
                    if (chkShutdown.Checked)
                    {
                        divShutdownReasonDesc.Visible = true;
                    }
                    else
                    {
                        divShutdownReasonDesc.Visible = false;
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "keySuccUp", "noty({text: 'Equipment updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);


                    _GetequipByID.ConnConfig = Session["config"].ToString();
                    _GetequipByID.EquipID = Convert.ToInt32(Request.QueryString["uid"]);
                    DataSet ds = new DataSet();
                    DataSet ds1 = new DataSet();
                    DataSet ds2 = new DataSet();
                    DataSet ds3 = new DataSet();
                    DataSet ds4 = new DataSet();
                    DataSet ds5 = new DataSet();

                    ListGetequipByID _lstGetequipByID = new ListGetequipByID();

                   
                    string APINAME4 = "EquipmentAPI/AddEquipment_GetequipByID";

                    APIResponse _APIResponse4 = new MOMWebUtility().CallMOMWebAPI(APINAME4, _GetequipByID);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetequipByID = serializer.Deserialize<ListGetequipByID>(_APIResponse4.ResponseData);

                    ds1 = _lstGetequipByID.lstTable1.ToDataSet();
                    ds2 = _lstGetequipByID.lstTable2.ToDataSet();
                    ds3 = _lstGetequipByID.lstTable3.ToDataSet();
                    ds4 = _lstGetequipByID.lstTable4.ToDataSet();
                    ds5 = _lstGetequipByID.lstTable5.ToDataSet();

                    DataTable dt1 = new DataTable();
                    DataTable dt2 = new DataTable();
                    DataTable dt3 = new DataTable();
                    DataTable dt4 = new DataTable();
                    DataTable dt5 = new DataTable();

                    dt1 = ds1.Tables[0];
                    dt2 = ds2.Tables[0];
                    dt3 = ds3.Tables[0];
                    dt4 = ds4.Tables[0];
                    dt5 = ds5.Tables[0];

                    dt1.TableName = "Table1";
                    dt2.TableName = "Table2";
                    dt3.TableName = "Table3";
                    dt4.TableName = "Table4";
                    dt5.TableName = "Table5";

                    ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy(), dt4.Copy(), dt5.Copy() });

                    RadGrid_gvTemplateItems.VirtualItemCount = ds.Tables[1].Rows.Count;
                    RadGrid_gvTemplateItems.DataSource = ds.Tables[1];
                    RadGrid_gvTemplateItems.Rebind();

                }
                else
                {
                    Int32 EquipID = 0;
                    String url = "addequipment.aspx?uid=";//&page=addcustomer&cuid=1"

                    if (Request.QueryString["page"] != null)
                    {
                        if (Request.QueryString["page"].ToString() == "addprospect")
                        {

                            string APINAME = "EquipmentAPI/AddEquipment_AddEquipmentForLead";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddEquipmentForLead);
                            object JsonData = JsonConvert.DeserializeObject(_APIResponse.ResponseData);
                            EquipID = Convert.ToInt32(JsonData.ToString());


                            url = url + EquipID.ToString() + "&page=addprospect&lid=" + Request.QueryString["lid"].ToString() + "&locname=" + Server.UrlEncode(url);
                        }

                        else
                        {
                            string APINAME = "EquipmentAPI/AddEquipment_AddEquipment";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddEquipment);
                            object JsonData = JsonConvert.DeserializeObject(_APIResponse.ResponseData);
                            EquipID = Convert.ToInt32(JsonData.ToString());


                            if (Request.QueryString["page"].ToString() == "addcustomer")
                            {
                                url = url + EquipID.ToString() + "&page=addcustomer&cuid=" + Request.QueryString["cuid"].ToString();
                            }
                            else
                            {
                                url = url + EquipID.ToString() + "&page=addlocation&lid=" + Request.QueryString["lid"].ToString() + "&locname=" + Server.UrlEncode(url);
                            }

                        }

                        Session["addequipmentStatus"] = "success";
                        Response.Redirect(url);

                    }
                    else
                    {
                        string APINAME = "EquipmentAPI/AddEquipment_AddEquipment";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddEquipment);
                        object JsonData = JsonConvert.DeserializeObject(_APIResponse.ResponseData);
                        EquipID = Convert.ToInt32(JsonData.ToString());


                        url = url + EquipID.ToString();
                        Session["addequipmentStatus"] = "success";
                        Response.Redirect(url);

                    }



                    if (Request.QueryString["addFrom"] != null)
                    {
                        Session["RefreshAddticketScreen"] = EquipID;
                        ///////////// Refresh Ticket Screen \\\\\\\\\\
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "RefreshAddTicket", "RefreshAddTicket();", true);
                    }
                    if (Request.QueryString["page"] != null)
                    {
                        string uid = string.Empty;
                        if (Request.QueryString["cuid"] != null)
                        {
                            uid = Request.QueryString["cuid"].ToString();
                        }
                        else
                        {
                            uid = Request.QueryString["lid"].ToString();
                        }
                        Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + uid + "&tab=equip");
                    }

                    ResetFormControlValues(this);

                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Equipment added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);


                    if (Request.QueryString["t"] != null)
                    {
                        Response.Redirect("addequipment.aspx?uid=" + EquipID + "&c=1");
                    }


                }
                hdnSaved.Value = "1";
                //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
                RadGrid_gvShutdownLogs.Rebind();
                RadGrid_gvLogs.Rebind();

            }
            else
            {
                objPropUser.LocID = Convert.ToInt32(hdnLocId.Value);
                objPropUser.Unit = txtEquipID.Text;
                objPropUser.Remarks = txtRemarks.Text;
                objPropUser.Type = ddlType.SelectedValue;
                objPropUser.Cat = ddlServiceType.SelectedValue;
                objPropUser.Manufacturer = txtManuf.Text;
                objPropUser.Serial = txtSerial.Text;
                objPropUser.UniqueID = txtUnique.Text;
                objPropUser.Category = ddlCategory.SelectedValue;
                objPropUser.building = ddlBuilding.SelectedValue;
                objPropUser.Description = txtDesc.Text;
                objPropUser.MOMUSer = Session["User"].ToString();
                objPropUser.UserID = Convert.ToInt32(Session["userid"].ToString());
                objPropUser.Classification = ddlClassification.SelectedValue;

                //if (Request.QueryString["page"].ToString() == "addprospect")
                //{
                //    objPropUser.IsLeadEquip = true;
                //}
                objPropUser.Shutdown = Convert.ToBoolean(chkShutdown.Checked);
                objPropUser.ShutdownReason = txtShutdownReason.Text;

                
                //if (hdnShutdownReasonPlanned.Value == "1")
                //{
                //    objPropUser.ShutdownLongDesc = string.Empty;
                //}
                //else
                //{
                //    objPropUser.ShutdownLongDesc = hdnShutdownLongDesc.Value;
                //}
                objPropUser.ShutdownLongDesc = hdnShutdownLongDesc.Value;

                if (hdnShutdownReasonPlanned.Value == "1")
                {
                    objPropUser.PlannedShutdown = true;
                }
                else
                {
                    objPropUser.PlannedShutdown = false;
                }

                if (txtSince.Text.Trim() == string.Empty)
                {
                    objPropUser.InstallDateTime = System.DateTime.MinValue;
                }
                else
                {
                    objPropUser.InstallDateTime = Convert.ToDateTime(txtSince.Text);
                }

                if (txtLast.Text.Trim() == string.Empty)
                {
                    objPropUser.LastServiceDate = System.DateTime.MinValue;
                }
                else
                {
                    objPropUser.LastServiceDate = Convert.ToDateTime(txtLast.Text);
                }

                if (txtInstalled.Text.Trim() == string.Empty)
                {
                    objPropUser.InstallDateimport = System.DateTime.MinValue;
                }
                else
                {
                    objPropUser.InstallDateimport = Convert.ToDateTime(txtInstalled.Text);
                }

                if (txtPrice.Text.Trim() == string.Empty)
                {
                    objPropUser.EquipPrice = Convert.ToDouble("0.00");
                }
                else
                {
                    objPropUser.EquipPrice = Convert.ToDouble(txtPrice.Text);
                }
                objPropUser.Status = Convert.ToInt32(rbStatus.SelectedValue);
                objPropUser.Remarks = txtRemarks.Text;

                //if (Request.QueryString["page"] != null)
                //{
                //    if (Request.QueryString["page"].ToString() == "addprospect")
                //    {
                //        DataTable dt = CreateTableFromGridForLead();
                //        dt.Columns.Remove("Name");
                //        objPropUser.DtItems = dt;
                //        DataTable dtCustom = CreateCustomTemplateForLead();
                //        objPropUser.dtcustom = dtCustom;
                //    }

                //    else
                //    {
                //        DataTable dt = CreateTableFromGrid();
                //        dt.Columns.Remove("Name");
                //        objPropUser.DtItems = dt;
                //        DataTable dtCustom = CreateCustomTemplate();
                //        objPropUser.dtcustom = dtCustom;
                //    }
                //}
                //else
                //{
                DataTable dt = CreateTableFromGrid();
                dt.Columns.Remove("Name");
                objPropUser.DtItems = dt;

                DataTable dtCustom = CreateCustomTemplate();
                objPropUser.dtcustom = dtCustom;
                
                //}


                objPropUser.CustomTemplateID = Convert.ToInt32(ddlCustTemplate.SelectedValue);
                //objPropUser.UpdateTicket = (hdnUpdateTicket.Value != string.Empty) ? Convert.ToInt16(hdnUpdateTicket.Value) : 0;
                objPropUser.ConnConfig = Session["config"].ToString();

                
                if (Convert.ToInt32(ViewState["mode"]) == 1)
                {
                    if (Session["MSM"].ToString() == "TS")
                    {
                        objPropUser.ItemsOnly = 1;
                        
                    }
                    objPropUser.EquipID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                    //document                
                    objPropUser.dtDocs = SaveDocInfo();

                    objBL_User.UpdateDocInfo(objPropUser);

                    if (Request.QueryString["page"] != null)
                    {
                        if (Request.QueryString["page"].ToString() == "addprospect")
                        {
                            objBL_User.UpdateLeadEquipment(objPropUser);
                        }

                        else
                        {
                            objBL_User.UpdateEquipment(objPropUser);
                        }
                    }
                    else
                    {
                        objBL_User.UpdateEquipment(objPropUser);
                    }

                    hdnLocPrevious.Value = hdnLocId.Value;
                    Get_CustomData();
                    RadGrid_gvLogs.Rebind();

                    // Update shutdown reason and shut down view state after saving 
                    ViewState["shut_down"] = chkShutdown.Checked;
                    hdnEqShutdownStatus.Value = chkShutdown.Checked ? "1" : "0";
                    divShutdownReason.Visible = false;
                    if (chkShutdown.Checked)
                    {
                        divShutdownReasonDesc.Visible = true;
                    }
                    else
                    {
                        divShutdownReasonDesc.Visible = false;
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "keySuccUp", "noty({text: 'Equipment updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);


                    objPropUser.ConnConfig = Session["config"].ToString();
                    objPropUser.EquipID = Convert.ToInt32(Request.QueryString["uid"]);

                    DataSet ds = new DataSet();
                    ds = objBL_User.getequipByID(objPropUser);
                    
                    RadGrid_gvTemplateItems.VirtualItemCount = ds.Tables[1].Rows.Count;
                    RadGrid_gvTemplateItems.DataSource = ds.Tables[1];
                    RadGrid_gvTemplateItems.Rebind();

                }
                else
                {
                    Int32 EquipID = 0;
                    String url = "addequipment.aspx?uid=";//&page=addcustomer&cuid=1"

                    if (Request.QueryString["page"] != null)
                    {
                        if (Request.QueryString["page"].ToString() == "addprospect")
                        {
                            EquipID = objBL_User.AddEquipmentForLead(objPropUser);
                            
                            url = url + EquipID.ToString() + "&page=addprospect&lid=" + Request.QueryString["lid"].ToString() + "&locname=" + Server.UrlEncode(url);
                        }

                        else
                        {
                            EquipID = objBL_User.AddEquipment(objPropUser);
                            
                            if (Request.QueryString["page"].ToString() == "addcustomer")
                            {
                                url = url + EquipID.ToString() + "&page=addcustomer&cuid=" + Request.QueryString["cuid"].ToString();
                            }
                            else
                            {
                                url = url + EquipID.ToString() + "&page=addlocation&lid=" + Request.QueryString["lid"].ToString() + "&locname=" + Server.UrlEncode(url);
                            }

                        }

                        Session["addequipmentStatus"] = "success";
                        Response.Redirect(url);

                    }
                    else
                    {
                        EquipID = objBL_User.AddEquipment(objPropUser);
                        
                        url = url + EquipID.ToString();
                        Session["addequipmentStatus"] = "success";
                        Response.Redirect(url);

                    }



                    if (Request.QueryString["addFrom"] != null)
                    {
                        Session["RefreshAddticketScreen"] = EquipID;
                        ///////////// Refresh Ticket Screen \\\\\\\\\\
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "RefreshAddTicket", "RefreshAddTicket();", true);
                    }
                    if (Request.QueryString["page"] != null)
                    {
                        string uid = string.Empty;
                        if (Request.QueryString["cuid"] != null)
                        {
                            uid = Request.QueryString["cuid"].ToString();
                        }
                        else
                        {
                            uid = Request.QueryString["lid"].ToString();
                        }
                        Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + uid + "&tab=equip");
                    }

                    ResetFormControlValues(this);

                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Equipment added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);


                    if (Request.QueryString["t"] != null)
                    {
                        Response.Redirect("addequipment.aspx?uid=" + EquipID + "&c=1");
                    }


                }
                hdnSaved.Value = "1";
                //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
                RadGrid_gvShutdownLogs.Rebind();
                RadGrid_gvLogs.Rebind();

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            if (str == "Equipment ID already exist.")
            {                
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            }
            else
            {
              
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
          

        }
    }

    protected void ddlFreq_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlFrequency = (DropDownList)sender;
        GridDataItem gvRow = (GridDataItem)ddlFrequency.Parent.Parent;



        RadDatePicker txtLastDate = (RadDatePicker)gvRow.FindControl("RadDatePickerLastDate");
        TextBox txtDueDate = (TextBox)gvRow.FindControl("txtDuedate");


        if (Convert.ToInt32(ddlFrequency.SelectedValue) > -1 && txtLastDate.SelectedDate.ToString() != "")
        {
            //string[] arr = txtLastDate.Text.Split('/');
            //DateTime dt = new DateTime(Convert.ToInt32(arr[2]), Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]));

            DateTime dt = (DateTime)txtLastDate.SelectedDate;
            txtDueDate.Text = calculateNextDueDate(dt, Convert.ToInt32(ddlFrequency.SelectedItem.Value)).ToShortDateString();
        }
        else
        {
            txtDueDate.Text = "";
        }

    }

   // protected void txtLdate_TextChanged(object sender, EventArgs e)
       protected void txtLdate_TextChanged(object sender, EventArgs e)
    {

        RadDatePicker radDatePicker = (RadDatePicker)sender;
        string txtLastDate = radDatePicker.SelectedDate.ToString();
        GridDataItem gvRow = (GridDataItem)radDatePicker.Parent.Parent;
        DropDownList ddlFrequency = (DropDownList)gvRow.FindControl("ddlFreq");
        TextBox txtDueDate = (TextBox)gvRow.FindControl("txtDuedate");
        if (Convert.ToInt32(ddlFrequency.SelectedValue) > -1 && txtLastDate != "")
        {
            string[] arr = txtLastDate.Split('/');
            DateTime dt = (DateTime)radDatePicker.SelectedDate;
             //new DateTime(Convert.ToInt32(arr[2]), Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]));

            txtDueDate.Text = calculateNextDueDate(dt, Convert.ToInt32(ddlFrequency.SelectedItem.Value)).ToShortDateString();
        }
        else
        {
            txtDueDate.Text = "";
        }

        //**************************************************************************************//
        // TextBox txtLastDate = (TextBox)sender;
        //GridDataItem gvRow = (GridDataItem)txtLastDate.Parent.Parent;
        //DropDownList ddlFrequency = (DropDownList)gvRow.FindControl("ddlFreq");
        //TextBox txtDueDate = (TextBox)gvRow.FindControl("txtDuedate");

        //if (Convert.ToInt32(ddlFrequency.SelectedValue) > -1 && txtLastDate.Text != "")
        //{
        //    string[] arr = txtLastDate.Text.Split('/');
        //    DateTime dt = new DateTime(Convert.ToInt32(arr[2]), Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]));

        //    txtDueDate.Text = calculateNextDueDate(dt, Convert.ToInt32(ddlFrequency.SelectedItem.Value)).ToShortDateString();
        //}
        //else
        //{
        //    txtDueDate.Text = "";
        //}
    }

    protected DateTime calculateNextDueDate(DateTime dt, int frequencyIndex)
    {
        switch (frequencyIndex)
        {
            case 0: dt = dt.AddDays(1); break;
            case 1: dt = dt.AddDays(7); break;
            case 2: dt = dt.AddDays(14); break;
            case 3: dt = dt.AddMonths(1); break;
            case 4: dt = dt.AddMonths(2); break;
            case 5: dt = dt.AddMonths(3); break;
            case 6: dt = dt.AddMonths(6); break;
            case 7: dt = dt.AddYears(1); break;
            case 8: dt = dt; break;
            case 9: dt = dt.AddMonths(4); break;
            case 10: dt = dt.AddYears(2); break;
            case 11: dt = dt.AddYears(3); break;
            case 12: dt = dt.AddYears(5); break;
            case 13: dt = dt.AddYears(7); break;
            default:
                //default stuff
                break;
        }
        return dt;
    }

    protected void btnAddNewItem_Click(object sender, EventArgs e)
    {
        this.programmaticModalPopup.Show();
        pnlREPT.Visible = true;
    }

    protected void lnkCloseTemplate_Click(object sender, EventArgs e)
    {
        this.programmaticModalPopup.Hide();
    }

    private DataTable CreateTableFromGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Code", typeof(string));
        dt.Columns.Add("Name", typeof(string));
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
        foreach (GridDataItem gr in RadGrid_gvTemplateItems.Items)
        {
            //if (((TextBox)gr.FindControl("lblDesc")).Text.Trim() != string.Empty)
            //{
            DataRow dr = dt.NewRow();
            dr["Code"] = ((Label)gr.FindControl("lblCode")).Text.Trim();
            dr["ID"] = ((Label)gr.FindControl("lblID")).Text.Trim();            
            dr["Name"] = ((Label)gr.FindControl("lblName")).Text.Trim();
            dr["EquipT"] = ((Label)gr.FindControl("lblEquipT")).Text.Trim();
            dr["Elev"] = 0;
            dr["fDesc"] = ((TextBox)gr.FindControl("lblDesc")).Text.Trim();
            dr["Line"] = dt.Rows.Count;
            if(gr.FindControl("RadDatePickerLastDate") != null)
            {
                //RadDatePicker radDatePicker = (RadDatePicker)sender;
                //string txtLastDate = radDatePicker.SelectedDate.ToString();

                if (((RadDatePicker)gr.FindControl("RadDatePickerLastDate")).SelectedDate.ToString() == string.Empty)
                {
                    dr["Lastdate"] = DBNull.Value;
                }
                else
                {
                    dr["Lastdate"] = Convert.ToDateTime(((RadDatePicker)gr.FindControl("RadDatePickerLastDate")).SelectedDate.ToString()).ToShortDateString();
                }
            }else
            {
                dr["Lastdate"] = DBNull.Value;
            }

            if (gr.FindControl("txtDuedate") != null)
            {

                if (((TextBox)gr.FindControl("txtDuedate")).Text.Trim() == string.Empty)
                {
                    dr["NextDateDue"] = DBNull.Value;
                }
                else
                {
                    dr["NextDateDue"] = Convert.ToDateTime(((TextBox)gr.FindControl("txtDuedate")).Text.Trim()).ToShortDateString();
                }
            }
            else
            {
                dr["NextDateDue"] = DBNull.Value;
            }


            dr["Frequency"] = ((DropDownList)gr.FindControl("ddlFreq")).SelectedItem.Value;
            dr["Section"] = ((TextBox)gr.FindControl("txtSection")).Text.Trim();
            dr["Notes"] = ((TextBox)gr.FindControl("txtNotes")).Text.Trim();
            dr["LeadEquip"] = 0;
            dt.Rows.Add(dr);
            //}
        }

        return dt;
    }

    //API
    public DataTable TableFromGridEmptyDatatable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Code", typeof(string));
        //dt.Columns.Add("Name", typeof(string));
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
        dr["ID"] = "0";
        dr["Code"] = "";
        //dr["Name"] = "";
        dr["EquipT"] = "0";
        dr["Elev"] = "0";
        dr["fDesc"] = "";
        dr["Line"] = "0";
        dr["Lastdate"] = DBNull.Value;
        dr["NextDateDue"] = DBNull.Value;
        dr["Frequency"] = "0";
        dr["Section"] = "";
        dr["Notes"] = "";
        dr["LeadEquip"] = "0";
        dt.Rows.Add(dr);
        return dt;
    }

    //API
    public DataTable CustomTemplateEmptyDatatable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("ElevT", typeof(int));
        dt.Columns.Add("Elev", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("value", typeof(string));
        dt.Columns.Add("Format", typeof(string));
        dt.Columns.Add("LastUpdated", typeof(string));
        dt.Columns.Add("LastUpdateUser", typeof(string));
        dt.Columns.Add("OrderNo", typeof(int));
        //dt.Columns.Add("LeadEquipT", typeof(int));
        dt.Columns.Add("LeadEquip", typeof(int));

        DataRow dr = dt.NewRow();
        dr["ID"] = "0";
        dr["ElevT"] = "0";
        dr["Elev"] = "0";
        dr["fDesc"] = "";
        dr["Line"] = "0";
        dr["value"] = "";
        dr["Format"] = "";
        dr["LastUpdated"] = "";
        dr["LastUpdateUser"] = "";
        dr["OrderNo"] = "0";
        dr["LeadEquip"] = "0";

        dt.Rows.Add(dr);
        return dt;
    }


    private DataTable CreateCustomTemplate()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("ElevT", typeof(int));
        dt.Columns.Add("Elev", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("value", typeof(string));
        dt.Columns.Add("Format", typeof(string));
        dt.Columns.Add("LastUpdated", typeof(string));
        dt.Columns.Add("LastUpdateUser", typeof(string));
        dt.Columns.Add("OrderNo", typeof(int));
        //dt.Columns.Add("LeadEquipT", typeof(int));
        dt.Columns.Add("LeadEquip", typeof(int));
        foreach (GridDataItem gr in RadGrid_gvCtemplItems.Items)
        {
            DataRow dr = dt.NewRow();
            dr["ID"] = Convert.ToInt32(((Label)gr.FindControl("lblID")).Text);
            dr["ElevT"] = 0;
            dr["Elev"] = 0;
            dr["fDesc"] = ((Label)gr.FindControl("lblDesc")).Text;
            dr["Line"] = ((Label)gr.FindControl("lblIndex")).Text.Trim(); //dt.Rows.Count + 1;
            if (((Label)gr.FindControl("lblFormat")).Text == "Dropdown")
                dr["value"] = ((DropDownList)gr.FindControl("ddlFormat")).Text.Trim();
            else
                dr["value"] = ((TextBox)gr.FindControl("lblValue")).Text.Trim();
            dr["Format"] = ((Label)gr.FindControl("lblFormat")).Text;
            HiddenField hdnValue = ((HiddenField)gr.FindControl("hdnValue"));
            if (!string.Equals(dr["value"].ToString(), hdnValue.Value, StringComparison.CurrentCultureIgnoreCase))
            {
                dr["LastUpdated"] = System.DateTime.Now.ToString();
                dr["LastUpdateUser"] = Session["username"].ToString();
            }
            else
            {
                dr["LastUpdated"] = ((Label)gr.FindControl("lblUpdateDate")).Text;
                dr["LastUpdateUser"] = ((Label)gr.FindControl("lblUpdateUser")).Text;
            }
            HiddenField txtRowLine = ((HiddenField)gr.FindControl("txtRowLine"));
            dr["OrderNo"] = txtRowLine.Value;
            //dr["LeadEquipT"] = 0;
            dr["LeadEquip"] = 0;
            dt.Rows.Add(dr);
        }
        return dt;
    }

    private DataTable CreateCustomTemplateForLead()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("ElevT", typeof(int));
        //dt.Columns.Add("Elev", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("value", typeof(string));
        dt.Columns.Add("Format", typeof(string));
        dt.Columns.Add("LastUpdated", typeof(string));
        dt.Columns.Add("LastUpdateUser", typeof(string));
        dt.Columns.Add("OrderNo", typeof(int));
        //dt.Columns.Add("LeadEquipT", typeof(int));
        dt.Columns.Add("LeadEquip", typeof(int));
        foreach (GridDataItem gr in RadGrid_gvCtemplItems.Items)
        {
            DataRow dr = dt.NewRow();
            dr["ID"] = Convert.ToInt32(((Label)gr.FindControl("lblID")).Text);
            dr["ElevT"] = 0;
            //dr["Elev"] = 0;
            dr["fDesc"] = ((Label)gr.FindControl("lblDesc")).Text;
            dr["Line"] = ((Label)gr.FindControl("lblIndex")).Text.Trim(); //dt.Rows.Count + 1;
            if (((Label)gr.FindControl("lblFormat")).Text == "Dropdown")
                dr["value"] = ((DropDownList)gr.FindControl("ddlFormat")).Text.Trim();
            else
                dr["value"] = ((TextBox)gr.FindControl("lblValue")).Text.Trim();
            dr["Format"] = ((Label)gr.FindControl("lblFormat")).Text;
            HiddenField hdnValue = ((HiddenField)gr.FindControl("hdnValue"));
            if (!string.Equals(dr["value"].ToString(), hdnValue.Value, StringComparison.CurrentCultureIgnoreCase))
            {
                dr["LastUpdated"] = System.DateTime.Now.ToString();
                dr["LastUpdateUser"] = Session["username"].ToString();
            }
            else
            {
                dr["LastUpdated"] = ((Label)gr.FindControl("lblUpdateDate")).Text;
                dr["LastUpdateUser"] = ((Label)gr.FindControl("lblUpdateUser")).Text;
            }
            HiddenField txtRowLine = ((HiddenField)gr.FindControl("txtRowLine"));
            dr["OrderNo"] = txtRowLine.Value;
            //dr["LeadEquipT"] = 0;
            dr["LeadEquip"] = 0;
            dt.Rows.Add(dr);
        }
        return dt;
    }

    private void AppendTemplateItemstoGrid(int TemplateID, string Startdate, bool Unique)
    {
        DataTable dtItems = CreateTableFromGrid(); 
        DataSet dsNewItems = new DataSet();
        objPropCustomer.ConnConfig = Session["config"].ToString();
        int Elev = 0;
        int LeadEquip = 0;
        objPropCustomer.TemplateID = TemplateID;
        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString() == "addprospect")
            {
                objPropCustomer.IsLeadEquip = true;
            }

            else
            {
                objPropCustomer.IsLeadEquip = false;
            }
        }
        else
        {
            objPropCustomer.IsLeadEquip = false;
        }

        List<GetTemplateItemByIDViewModel> _lstGetTemplateItemByID = new List<GetTemplateItemByIDViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/MassMCP_GetTemplateItemByID";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetTemplateItemByID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetTemplateItemByID = serializer.Deserialize<List<GetTemplateItemByIDViewModel>>(_APIResponse.ResponseData);
            dsNewItems = CommonMethods.ToDataSet<GetTemplateItemByIDViewModel>(_lstGetTemplateItemByID);
        }
        else
        {
            dsNewItems = objBL_Customer.getTemplateItemByID(objPropCustomer);
        }

        foreach (DataRow dr in dsNewItems.Tables[0].Rows)
        {
            //DataRow[] drSelect = dtItems.Select("equipt=" + Convert.ToInt32(dr["EquipT"]) + " and fdesc='" + dr["fDesc"].ToString() + "'");
            int count = 0;

            //if (Unique)
            //{
            //    DataRow[] drSelect = dtItems.Select("Code='" + dr["Code"].ToString() + "'");
            //    count = drSelect.Count();
            //}

            if (count == 0)
            {
                DataRow drNew = dtItems.NewRow();
                drNew["Code"] = dr["Code"].ToString();
                drNew["ID"] =  "0";
                drNew["Name"] = dr["Name"].ToString();
                drNew["EquipT"] = dr["EquipT"].ToString();
                drNew["Elev"] = Elev;
                drNew["fDesc"] = dr["fDesc"].ToString();
                drNew["Line"] = dtItems.Rows.Count;

                if (Startdate != string.Empty)
                {
                    DateTime dtst = new DateTime();
                    if (DateTime.TryParse(Startdate, out dtst))
                    {
                        drNew["Lastdate"] = dtst;
                        if (Convert.ToInt32(dr["Frequency"].ToString()) > -1)
                            drNew["NextDateDue"] = calculateNextDueDate(dtst, Convert.ToInt32(dr["Frequency"].ToString()));
                    }
                    else
                    {
                        drNew["Lastdate"] = DBNull.Value;
                        drNew["NextDateDue"] = DBNull.Value;
                    }
                }
                else
                {
                    drNew["Lastdate"] = DBNull.Value;
                    drNew["NextDateDue"] = DBNull.Value;
                }

                drNew["Frequency"] = dr["Frequency"].ToString();
                drNew["Section"] = dr["Section"].ToString();
                drNew["Notes"] = dr["Notes"].ToString();
                drNew["LeadEquip"] = LeadEquip;
                dtItems.Rows.InsertAt(drNew, 0);
            }
        }


        //RadGrid_gvTemplateItems.Visible = true;
        //gvtempgrid.Visible = true; 
        RadGrid_gvTemplateItems.VirtualItemCount = dtItems.Rows.Count;
        RadGrid_gvTemplateItems.DataSource = dtItems; 
        RadGrid_gvTemplateItems.Rebind(); 
        //Session["templtableEquipment"] = dtItems;
    }

    protected void cbRepTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        //GridViewRow gvRow = (GridViewRow)lnk.Parent.Parent;
        GridDataItem dataItem = (GridDataItem)lnk.NamingContainer;
        Label lblRepTempID = (Label)dataItem.FindControl("lblRepTempId");
        TextBox txtStartDate = (TextBox)dataItem.FindControl("txtStartDate");
        templateId = Convert.ToInt32(lblRepTempID.Text);
        AppendTemplateItemstoGrid(Convert.ToInt32(lblRepTempID.Text), txtStartDate.Text.Trim(), true);
        hdnSaved.Value = "0";
    }

    protected void lnkSaveTemplate_Click(object sender, EventArgs e)
    {
         

        AppendTemplateItemstoGrid(Convert.ToInt32(ddlRepTemp.SelectedValue), txtLastDate.Text.Trim(), false);
        this.programmaticModalPopup.Hide();
    }

    protected void btnDeleteItem_Click(object sender, EventArgs e)
    {
        DataTable dt = CreateTableFromGrid();
        int index = 0;
        foreach (GridDataItem gr in RadGrid_gvTemplateItems.MasterTableView.Items)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                dt.Rows.RemoveAt(index);
            }
            else
            {
                index++;
            }
        }
        RadGrid_gvTemplateItems.DataSource = dt;
        RadGrid_gvTemplateItems.VirtualItemCount = dt.Rows.Count;
        RadGrid_gvTemplateItems.Rebind();
        hdnSaved.Value = "0";
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        fillREPHistory();
        RadGrid_gvRepDetails.Rebind();
    }

    protected void lnkclear_Click(object sender, EventArgs e)
    {
        ddlSearch.SelectedIndex = -1;
        txtSearch.Text = string.Empty;
        txtCodeSearch.Text = string.Empty;
        ddlTemplate.SelectedIndex = -1;
        ddlFreq.SelectedIndex = -1;
        ddlDates.SelectedIndex = -1;
        txtfromDate.Text = string.Empty;
        txtToDate.Text = string.Empty;
        txtSearch.Visible = true;
        txtCodeSearch.Visible = false;
        ddlFreq.Visible = false;
        ddlTemplate.Visible = false;
    }

    protected void hideModalPopupViaServer_Click(object sender, EventArgs e)
    {
        fillREPHistory();
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
                switch (c.GetType().ToString())
                {
                    case "System.Web.UI.WebControls.DropDownList":
                        ((DropDownList)c).SelectedIndex = -1;
                        break;
                    case "System.Web.UI.WebControls.TextBox":
                        ((TextBox)c).Text = "";
                        break;
                    case "System.Web.UI.WebControls.CheckBox":
                        ((CheckBox)c).Checked = false;
                        break;
                    case "System.Web.UI.WebControls.RadioButton":
                        ((RadioButton)c).Checked = false;
                        break;
                }
            }
        }
    }

   
    protected void SetSortDirection(string sortDirection)
    {
        if (sortDirection == "ASC")
        {
            _sortDirection = "DESC";
        }
        else
        {
            _sortDirection = "ASC";
        }
    }

    public string SortDireaction
    {
        get
        {
            if (ViewState["SortDireaction"] == null)
                return string.Empty;
            else
                return ViewState["SortDireaction"].ToString();
        }
        set
        {
            ViewState["SortDireaction"] = value;
        }
    }
    private string _sortDirection;

    
    protected void ddlCustTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();
        //objPropCustomer.TemplateID = Convert.ToInt32(ddlCustTemplate.Text);
        

        _GetCustTemplateItemByID.TemplateID = Convert.ToInt32(ddlCustTemplate.Text);
        _GetCustTemplateItemByID.ConnConfig = Session["config"].ToString();

        ListGetCustTemplateItemByID _lstGetCustTemplateItemByID = new ListGetCustTemplateItemByID();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/AddEquipment_GetCustTemplateItemByID";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustTemplateItemByID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetCustTemplateItemByID = serializer.Deserialize<ListGetCustTemplateItemByID>(_APIResponse.ResponseData);

            ds1 = _lstGetCustTemplateItemByID.lstTable1.ToDataSet();
            ds2 = _lstGetCustTemplateItemByID.lstTable2.ToDataSet();

            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            dt1 = ds1.Tables[0];
            dt2 = ds2.Tables[0];

            dt1.TableName = "Table1";
            dt2.TableName = "Table2";

            ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });
        }
        else
        {
            var templateId = Convert.ToInt32(ddlCustTemplate.Text);
            int leadEquipId = 0;
            int equipIdtemp = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["uid"]))
            {
                if (Request.QueryString["page"] != null && Request.QueryString["page"].ToString() == "addprospect")
                {
                    leadEquipId = Convert.ToInt32(Request.QueryString["uid"]);
                }
                else
                {
                    equipIdtemp = Convert.ToInt32(Request.QueryString["uid"]);
                }
            }

            ds = objBL_Customer.GetEquipmentCustTemplateItem(Session["config"].ToString(), templateId, equipIdtemp, leadEquipId);
        }

        RadGrid_gvCtemplItems.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_gvCtemplItems.DataSource = ds.Tables[0];
        RadGrid_gvCtemplItems.Rebind();
        //gvCtemplItems.DataSource = ds;
        //gvCtemplItems.DataBind();
        hdnSelectedVal.Value = ddlCustTemplate.SelectedValue;
        //ac
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                //gvCtemplItems.DataSource = ds;
                //gvCtemplItems.DataBind();
                GridFooterItem footeritem = (GridFooterItem)RadGrid_gvCtemplItems.MasterTableView.GetItems(GridItemType.Footer)[0];
                Label lblRowCount = (Label)footeritem.FindControl("lblRowCount");
                lblRowCount.Text = "Total Line Items: " + Convert.ToString(ds.Tables[0].Rows.Count - 0);
                // ac ((Label)RadGrid_gvCtemplItems.FooterRow.FindControl("lblRowCount")).Text = "Total Line Items: " + Convert.ToString(ds.Tables[0].Rows.Count - 0);
                if (ds.Tables[1].Rows.Count > 0)
                {
                    //ViewState["customvalues"] = ds.Tables[1];
                    binditemgrid(ds.Tables[1]);
                }
            }
        }
    }

    private void binditemgrid(DataTable dtValues)
    {
        foreach (GridDataItem gr in RadGrid_gvCtemplItems.MasterTableView.Items)
        {
            Label lblFormat = (Label)gr.FindControl("lblFormat");
            if (lblFormat.Text == "Dropdown")
            {

                DropDownList ddlFormat = (DropDownList)gr.FindControl("ddlFormat");
                ddlFormat.Visible = true;
                Label lblID = (Label)gr.FindControl("lblID");
                Label lblIndex = (Label)gr.FindControl("lblIndex");
                Label lblValueh = (Label)gr.FindControl("lblValueh");

                DataTable dt = dtValues.Clone();
                DataRow[] result = dtValues.Select("ItemID = " + Convert.ToInt32(lblID.Text));// + " AND Line = " + Convert.ToInt32(lblIndex.Text) + ""
                foreach (DataRow row in result)
                {
                    dt.ImportRow(row);
                }

                if (dt.Rows.Count > 0)
                {
                    //dt.DefaultView.Sort = "Value  ASC";
                    dt.DefaultView.Sort = "LINE ASC";
                    dt = dt.DefaultView.ToTable();
                }
                ddlFormat.Style["class"] = "default-browser";
                ddlFormat.DataSource = dt;
                ddlFormat.DataTextField = "Value";
                ddlFormat.DataValueField = "Value";
                ddlFormat.DataBind();
                ddlFormat.Items.Insert(0, (new ListItem("", "")));
                //gr["Value"].Controls.Add(ddlFormat);

                if (ddlFormat.Items.Contains(new ListItem(lblValueh.Text, lblValueh.Text)))
                {
                    ddlFormat.SelectedValue = lblValueh.Text;
                }
                else
                {
                    ddlFormat.Items.Add(new ListItem(lblValueh.Text, lblValueh.Text));
                    ddlFormat.SelectedValue = lblValueh.Text;
                }
            }
        }

        
    }

    public string setLinkStyle(string internet)
    {
        string str = "cursor:pointer";
        if (Session["type"].ToString() == "c")
        {
            if (internet == "0")
                str = "color:black";
        }
        return str;
    }

    private void GetContractType()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.EquipID = Convert.ToInt32(Request.QueryString["uid"]);

        _GetContractType.ConnConfig = Session["config"].ToString();
        _GetContractType.EquipID = Convert.ToInt32(Request.QueryString["uid"]);

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/AddEquipment_GetContractType";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetContractType);
            object JsonData = JsonConvert.DeserializeObject(_APIResponse.ResponseData);
            lblContractType.Text = Convert.ToString(JsonData.ToString());
        }
        else
        {
            lblContractType.Text = objBL_User.getContractType(objPropUser);
        }

        divContractType.Visible = true;
    }

    #region ::Tests::
    private void GetTests()
    {
        tbTests.Style["display"] = "block";
        objPropUser.ConnConfig = WebBaseUtility.ConnectionString;
        objPropUser.EquipID = Convert.ToInt32(Request.QueryString["uid"]);

        _GetEquipmentTests.ConnConfig = WebBaseUtility.ConnectionString;
        _GetEquipmentTests.EquipID = Convert.ToInt32(Request.QueryString["uid"]);

        DataSet ds = new DataSet();

        List<GetEquipmentTestsViewModel> _lstGetEquipmentTests = new List<GetEquipmentTestsViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/AddEquipment_GetEquipmentTests";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetEquipmentTests);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetEquipmentTests = serializer.Deserialize<List<GetEquipmentTestsViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetEquipmentTestsViewModel>(_lstGetEquipmentTests);
        }
        else
        {
            ds = objBL_User.GetAllTestByEquipmentID(objPropUser);
        }

        RadGrid_gvtests.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_gvtests.DataSource = ds.Tables[0];
        RadPersistenceManager1.SaveState();
        //gvtests.DataSource = ds.Tables[0];
        //gvtests.DataBind();
    }
    #endregion
    protected void lnkAddTest_Click(object sender, EventArgs e)
    {

        Response.Redirect("AddTests.aspx?elv=" + Request.QueryString["uid"].ToString());
    }
    protected void lnkDelTest_Click(object sender, EventArgs e)
    {
        SafetyTest objproptest = new SafetyTest();
        BL_SafetyTest objtestbl = new BL_SafetyTest();
        objproptest.ConnConfig = WebBaseUtility.ConnectionString;
        int index = 0;
        int success = 0;
        string msg = string.Empty;
        try
        {
            foreach (GridDataItem gr in RadGrid_gvtests.Items)
            {
               // CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

                CheckBox chkSelect = (CheckBox)gr["chkSelect"].Controls[0];

                HiddenField hdnidTestItem = (HiddenField)gr.FindControl("hdnidTestItem");

                if (chkSelect.Checked == true)
                {

                    objproptest.LID = Convert.ToInt32(hdnidTestItem.Value);

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
                                success = objtestbl.DeleteTest(objproptest);
                            }
                        }
                    }

                }
                else
                {
                    index++;
                }


                switch (success)
                {
                    case 0:

                        msg = ("Test " + objproptest.LID + " could not be deleted.");

                        break;
                    case 1:

                        msg = ("Test " + objproptest.LID + " deleted successfully.");
                        break;
                    case -1:


                        msg = ("Test " + objproptest.LID + " has already been scheduled. You must cancel the ticket for this test prior to being able to delete the test itself.");
                        break;


                    default:
                        // jsonPOInformation.ReponseObject = string.Empty;
                        break;


                }
            }

            if (success == 1)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "noty({text: '" + msg + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                RadGrid_gvtests.Rebind();
            }
            else
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "noty({text: '" + msg + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "noty({text: '" + ex.Message + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }

        GetTests();
    }
    protected void gvtests_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
     [System.Web.Services.WebMethod(EnableSession = true)]
   
    public static string FillLocInfo(string LocID)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        BL_Customer objBL_Customer = new BL_Customer();
        Customer objPropCustomer = new Customer();
        string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
        GetLocationByIDParam _GetLocationByID = new GetLocationByIDParam();

        string comapny = string.Empty;
        objPropUser.DBName = HttpContext.Current.Session["dbname"].ToString();
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        //objPropUser.LocID = Convert.ToInt32(hdnLocId.Value);
        objPropUser.LocID = Convert.ToInt32(LocID);
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();

        //API
        _GetLocationByID.DBName = HttpContext.Current.Session["dbname"].ToString();
        _GetLocationByID.ConnConfig = HttpContext.Current.Session["config"].ToString();
        _GetLocationByID.LocID = Convert.ToInt32(LocID);
        _GetLocationByID.ConnConfig = HttpContext.Current.Session["config"].ToString();

        DataSet ds = new DataSet();

        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();
        DataSet ds3 = new DataSet();
        DataSet ds4 = new DataSet();
        DataSet ds5 = new DataSet();

        ListGetLocationByID _lstGetLocationByID = new ListGetLocationByID();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/AddEquipment_GetLocationByID";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetLocationByID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetLocationByID = serializer.Deserialize<ListGetLocationByID>(_APIResponse.ResponseData);

            ds1 = _lstGetLocationByID.lstTable1.ToDataSet();
            ds2 = _lstGetLocationByID.lstTable2.ToDataSet();
            ds3 = _lstGetLocationByID.lstTable3.ToDataSet();
            ds4 = _lstGetLocationByID.lstTable4.ToDataSet();
            ds5 = _lstGetLocationByID.lstTable5.ToDataSet();

            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            DataTable dt5 = new DataTable();

            dt1 = ds1.Tables[0];
            dt2 = ds2.Tables[0];
            dt3 = ds3.Tables[0];
            dt4 = ds4.Tables[0];
            dt5 = ds5.Tables[0];

            dt1.TableName = "Table1";
            dt2.TableName = "Table2";
            dt3.TableName = "Table3";
            dt4.TableName = "Table4";
            dt5.TableName = "Table5";

            ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy(), dt4.Copy(), dt5.Copy() });
        }
        else
        {
            ds = objBL_User.getLocationByID(objPropUser);
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            comapny = ds.Tables[0].Rows[0]["Company"].ToString();
        }
        return comapny;

    }
    //fixed.
    protected void lnkUploadDoc_Click(object sender, EventArgs e)
    {
        try
        {
            string filename = string.Empty;
            string fullpath = string.Empty;
            string MIME = string.Empty;
            // HttpPostedFile filePosted = Request.Files["ctl00$ContentPlaceHolder1$FileUpload1"];

            //if (filePosted != null && filePosted.ContentLength > 0)
            if (Request.QueryString["uid"] != null)
            {
                foreach (HttpPostedFile postedFile in FileUpload1.PostedFiles)
                {

                    string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();

                    string savepath = savepathconfig + @"\" + Session["dbname"] + @"\ld_" + Request.QueryString["uid"].ToString() + @"\";

                    string fileExtensionApplication = System.IO.Path.GetExtension(savepathconfig);
                    //filename = System.IO.Path.GetFileName(FileUpload1.Value);
                    filename = postedFile.FileName;
                  //  filename = filename.Replace(",", "");
                    fullpath = savepath + filename;
                    //MIME = Path.GetExtension(FileUpload1.PostedFile.FileName).Substring(1);
                    MIME = Path.GetExtension(postedFile.FileName).Substring(1);
                    if (File.Exists(fullpath))
                    {
                        GeneralFunctions objGeneralFunctions = new GeneralFunctions();
                        filename = objGeneralFunctions.generateRandomString(4) + "_" + filename;
                        fullpath = savepath + filename;

                    }
                    using (new NetworkConnection())
                    {
                        if (!Directory.Exists(savepath))
                        {
                            Directory.CreateDirectory(savepath);
                        }

                        postedFile.SaveAs(fullpath);
                    }

                    objMapData.Screen = "Equipment";
                    objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                    objMapData.TempId = "0";
                    objMapData.FileName = filename;
                    objMapData.DocTypeMIME = MIME;
                    objMapData.FilePath = fullpath;
                    objMapData.DocID = 0;
                    objMapData.Mode = 0;
                    objMapData.ConnConfig = Session["config"].ToString();

                    objMapData.Worker = Session["User"].ToString();
                    objBL_MapData.AddFile(objMapData);

                    ////API
                    //_AddFile.Screen = "Equipment";
                    //_AddFile.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                    //_AddFile.TempId = "0";
                    //_AddFile.FileName = filename;
                    //_AddFile.DocTypeMIME = MIME;
                    //_AddFile.FilePath = fullpath;
                    //_AddFile.DocID = 0;
                    //_AddFile.Mode = 0;
                    //_AddFile.ConnConfig = Session["config"].ToString();

                    //if (IsAPIIntegrationEnable == "YES")
                    //{
                    //    string APINAME = "EquipmentAPI/AddEquipment_AddFile";

                    //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddFile);
                    //}
                    //else
                    //{
                    //    objBL_MapData.AddFile(objMapData);
                    //}


                    //objPropUser.ConnConfig = Session["config"].ToString();
                    //objPropUser.dtDocs = SaveDocInfo();

                    ////API
                    //_UpdateDocInfo.ConnConfig = Session["config"].ToString();
                    //DataTable viewstatedata = SaveDocInfo();

                    //if (viewstatedata.Rows.Count == 0)
                    //{
                    //    DataTable returnVal = SaveDocInfoEmptyDatatable();
                    //    _UpdateDocInfo.dtDocs = returnVal;
                    //}
                    //else
                    //{
                    //    _UpdateDocInfo.dtDocs = SaveDocInfo();
                    //}

                    //if (IsAPIIntegrationEnable == "YES")
                    //{
                    //    string APINAME = "EquipmentAPI/AddEquipment_UpdateDocInfo";

                    //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateDocInfo);
                    //}
                    //else
                    //{
                    //    objBL_User.UpdateDocInfo(objPropUser);
                    //}

                }
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.dtDocs = SaveDocInfo();
                objBL_User.UpdateDocInfo(objPropUser);

                GetDocuments();
                RadGrid_Documents.Rebind();
                ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadSuccess", "noty({text: 'File uploaded successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string extension = Path.GetExtension(FileUpload1.FileName);
            if (extension == "")
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadextension", "noty({text: 'Invalid File!',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }
    protected void lnkDeleteDoc_Click(object sender, EventArgs e)
    {


        foreach (GridDataItem item in RadGrid_Documents.SelectedItems)
        {

            CheckBox chkSelected = (CheckBox)item["chkSelect"].Controls[0];            
            Label lblID = (Label)item.FindControl("lblId");
            if (chkSelected.Checked == true)
            {
                DeleteFileFromFolder(string.Empty, Convert.ToInt32(lblID.Text));
                RadGrid_Documents.Rebind();
            }

        }

    }
    protected void lblName_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;

        string[] CommandArgument = btn.CommandArgument.Replace(btn.Text, " ").Split(',');

        string FileName = btn.Text;
        string FilePath = CommandArgument[1].Trim() + btn.Text.Trim();

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
    protected void RadGrid_Documents_PreRender(object sender, EventArgs e)
    {

        RowSelectDocuments();
    }
    private void RowSelectDocuments()
    {
        if (hdnEditeDocument.Value == "N")
        {
            foreach (GridDataItem item in RadGrid_Documents.Items)
            {

                TableCell cell = item["chkSelect"];
                CheckBox chkSelected = (CheckBox)cell.Controls[0];
                CheckBox chkPortal = (CheckBox)item.FindControl("chkPortal");
                TextBox txtremarks = (TextBox)item.FindControl("txtremarks");
                chkSelected.Enabled = chkPortal.Enabled = txtremarks.Enabled = false;
                item.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";
            }
        }
    }
    private void PagePermission()
    {
        if (Convert.ToString(Session["type"]) != "am" && Convert.ToString(Session["type"]) != "c")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];
            string DocumentPermission = ds.Rows[0]["DocumentPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["DocumentPermission"].ToString();
            hdnAddeDocument.Value = DocumentPermission.Length < 1 ? "Y" : DocumentPermission.Substring(0, 1);
            hdnEditeDocument.Value = DocumentPermission.Length < 2 ? "Y" : DocumentPermission.Substring(1, 1);
            hdnDeleteDocument.Value = DocumentPermission.Length < 3 ? "Y" : DocumentPermission.Substring(2, 1);
            hdnViewDocument.Value = DocumentPermission.Length < 4 ? "Y" : DocumentPermission.Substring(3, 1);
            if (hdnDeleteDocument.Value == "N")
            {
                lnkDeleteDoc.Enabled = false;
            }

            if (hdnAddeDocument.Value == "N")
            {
                lnkUploadDoc.Enabled = false;
            }

            string SafetyTestsPermission = ds.Rows[0]["SafetyTestsPermission"] == DBNull.Value ? "YYYYNN" : ds.Rows[0]["SafetyTestsPermission"].ToString();
            hdnAddSafetyTest.Value = SafetyTestsPermission.Length < 1 ? "Y" : SafetyTestsPermission.Substring(0, 1);
            hdnDeleteSafetyTest.Value = SafetyTestsPermission.Length < 3 ? "Y" : SafetyTestsPermission.Substring(2, 1);
           
            if (hdnAddSafetyTest.Value == "N")
            {
                lnkAddTest.Visible = false;
            }

            if (hdnDeleteSafetyTest.Value == "N")
            {
                lnkDelTest.Visible = false;
            }
            if(hdnAddSafetyTest.Value == "N" && hdnDeleteSafetyTest.Value == "N")
            {
                divTestButton.Visible = false;
            }

            string ticketPermission = ds.Rows[0]["TicketPermission"] == DBNull.Value ? "YYNYYY" : ds.Rows[0]["TicketPermission"].ToString();
            hdnAddeTicket.Value = ticketPermission.Length < 1 ? "Y" : ticketPermission.Substring(0, 1);

            if (hdnAddeTicket.Value == "N")
            {
                lnkAddTicket.Visible = false;
            }

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

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/AddEquipment_DeleteFile";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _DeleteFile);
            }
            else
            {
                objBL_MapData.DeleteFile(objMapData);
            }

            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.dtDocs = SaveDocInfo();

            //API
            _UpdateDocInfo.ConnConfig = Session["config"].ToString();
            DataTable viewstatedata = SaveDocInfo();

            if (viewstatedata.Rows.Count == 0)
            {
                DataTable returnVal = SaveDocInfoEmptyDatatable();
                _UpdateDocInfo.dtDocs = returnVal;
            }
            else
            {
                _UpdateDocInfo.dtDocs = SaveDocInfo();
            }

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/AddEquipment_UpdateDocInfo";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateDocInfo);
            }
            else
            {
                objBL_User.UpdateDocInfo(objPropUser);
            }

            //GetDocuments();
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
    bool isGroupingDocuments = false;
    public bool ShouldApplySortFilterOrGroupDocuments()
    {
        return RadGrid_Documents.MasterTableView.FilterExpression != "" ||
            (RadGrid_Documents.MasterTableView.GroupByExpressions.Count > 0 || isGroupingDocuments) ||
            RadGrid_Documents.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_Documents_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_Documents.AllowCustomPaging = !ShouldApplySortFilterOrGroupDocuments();
        if (!string.IsNullOrEmpty(Request.QueryString["uid"]))
        {
            GetDocuments();
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
            objMapData.Screen = "Equipment";
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());

            _GetDocuments.Screen = "Equipment";
            _GetDocuments.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        }

        objMapData.TempId = "0";
        _GetDocuments.TempId = "0";


        objMapData.ConnConfig = Session["config"].ToString();
        _GetDocuments.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();

        List<GetDocumentsViewModel> _lstGetDocuments = new List<GetDocumentsViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/AddEquipment_GetDocuments";

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

        //gvDocuments.DataSource = ds.Tables[0];
        //gvDocuments.DataBind();

    }
    private DataTable SaveDocInfo()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Portal", typeof(int));
        dt.Columns.Add("Remarks", typeof(string));
        dt.Columns.Add("MSVisible", typeof(byte));

        foreach (GridDataItem gr in RadGrid_Documents.Items)
        {
            Label lblID = (Label)gr.FindControl("lblID");
            CheckBox chkPortal = (CheckBox)gr.FindControl("chkPortal");
            TextBox txtRemarks = (TextBox)gr.FindControl("txtRemarks");
            CheckBox chkMSVisible = (CheckBox)gr.FindControl("chkMSVisible");

            DataRow dr = dt.NewRow();
            dr["ID"] = lblID.Text;
            dr["Portal"] = chkPortal.Checked;
            dr["Remarks"] = txtRemarks.Text;
            dr["MSVisible"] = chkMSVisible.Checked;
            dt.Rows.Add(dr);
        }
        return dt;
    }

    //API
    public DataTable SaveDocInfoEmptyDatatable()
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

    #region CustomTemplates 
    protected void RadGrid_gvCtemplItems_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_gvCtemplItems.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
         Get_CustomData(); 
    


    }
    protected void RadGrid_gvCtemplItems_PreRender(object sender, EventArgs e)
    {
       
    }

    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
        
    {
        return RadGrid_gvCtemplItems.MasterTableView.FilterExpression != "" ||
            (RadGrid_gvCtemplItems.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_gvCtemplItems.MasterTableView.SortExpressions.Count > 0;



    }

    #endregion

    #region SelectTemplate
    protected void RadGrid_gvSelectTemplate_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {

        FillRepTemplate();

    }
    #endregion
    #region TemplateItems

     


    //}
    protected void RadGrid_gvTemplateItems_PreRender(object sender, EventArgs e)
    {

    }
    #endregion

    #region REPHistory
    protected void RadGrid_gvRepDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_gvRepDetails.AllowCustomPaging = !ShouldApplySortFilterOrGroupREP();
        FillGridfromSession();
     

    }
    bool isGroup = false;
    public bool ShouldApplySortFilterOrGroupREP()
    {
        return RadGrid_gvRepDetails.MasterTableView.FilterExpression != "" ||
            (RadGrid_gvRepDetails.MasterTableView.GroupByExpressions.Count > 0 || isGroup) ||
            RadGrid_gvRepDetails.MasterTableView.SortExpressions.Count > 0;



    }
    protected void RadGrid_gvRepDetails_PreRender(object sender, EventArgs e)
    {

    }
    #endregion
    #region Tests
    protected void RadGrid_gvtests_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_gvtests.AllowCustomPaging = !ShouldApplySortFilterOrGroupTest();
        if (Request.QueryString["uid"] != null)
        {
            GetTests();
        }
      
    }



    protected void RadGrid_gvtests_PreRender(object sender, EventArgs e)
    {
        RowSelect();
    }

    private void RowSelect() 
    {
       foreach (GridDataItem item in RadGrid_gvtests.MasterTableView.Items)
        {
            HiddenField hdnEquipmentid = (HiddenField)item.FindControl("hdnidUnit");
            HiddenField hdnidTestItem = (HiddenField)item.FindControl("hdnidTestItem");
            HiddenField hdnTestYear = (HiddenField)item.FindControl("hdnTestYear");

            CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");

            item.Attributes["ondblclick"] = "location.href='AddTests.aspx?elv=" + hdnEquipmentid.Value + "&lid=" + hdnidTestItem.Value + "&tyear="+ hdnTestYear.Value+ "'" ;
            if (chkSelect != null)
            {
                item.Attributes["onclick"] = "SelectRowChk('" + item.ClientID + "','" + chkSelect.ClientID + "','" + RadGrid_gvtests.ClientID + "',event);";
            }

        }
        }

       
    

    bool isGroupTest = false;
    public bool ShouldApplySortFilterOrGroupTest()
    {
        return RadGrid_gvtests.MasterTableView.FilterExpression != "" ||
            (RadGrid_gvtests.MasterTableView.GroupByExpressions.Count > 0 || isGroupTest) ||
            RadGrid_gvtests.MasterTableView.SortExpressions.Count > 0;



    }
    #endregion
    #region logs
    protected void RadGrid_gvLogs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();

        if (Request.QueryString["uid"] != null)
        {
            objPropUser.EquipID = Convert.ToInt32(Request.QueryString["uid"]);
            objPropUser.ConnConfig = Session["config"].ToString();

            _GetequipByID.EquipID = Convert.ToInt32(Request.QueryString["uid"]);
            _GetequipByID.ConnConfig = Session["config"].ToString();

            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            DataSet ds3 = new DataSet();
            DataSet ds4 = new DataSet();
            DataSet ds5 = new DataSet();
            ListGetequipByID _lstGetequipByID = new ListGetequipByID();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/AddEquipment_GetequipByID";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetequipByID);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetequipByID = serializer.Deserialize<ListGetequipByID>(_APIResponse.ResponseData);

                ds1 = _lstGetequipByID.lstTable1.ToDataSet();
                ds2 = _lstGetequipByID.lstTable2.ToDataSet();
                ds3 = _lstGetequipByID.lstTable3.ToDataSet();
                ds4 = _lstGetequipByID.lstTable4.ToDataSet();
                ds5 = _lstGetequipByID.lstTable5.ToDataSet();

                ds3.Tables[0].Columns["OrderNo1"].ColumnName = "OrderNo";
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();
                DataTable dt4 = new DataTable();
                DataTable dt5 = new DataTable();

                dt1 = ds1.Tables[0];
                dt2 = ds2.Tables[0];
                dt3 = ds3.Tables[0];
                dt4 = ds4.Tables[0];
                dt5 = ds5.Tables[0];

                dt1.TableName = "Table1";
                dt2.TableName = "Table2";
                dt3.TableName = "Table3";
                dt4.TableName = "Table4";
                dt5.TableName = "Table5";

                ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy(), dt4.Copy(), dt5.Copy() });
            }
            else
            {
                ds = objBL_User.getequipByID(objPropUser);
            }

            if (ds.Tables[4].Rows.Count > 0)
            {
                RadGrid_gvLogs.VirtualItemCount = ds.Tables[4].Rows.Count;
                RadGrid_gvLogs.DataSource = ds.Tables[4];            
            }
            else
            {
                RadGrid_gvLogs.DataSource = string.Empty;
            }
        }
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

    bool isGroupLog = false;
    public bool ShouldApplySortFilterOrGroupLogs()
    {
        return RadGrid_gvLogs.MasterTableView.FilterExpression != "" ||
            (RadGrid_gvLogs.MasterTableView.GroupByExpressions.Count > 0 || isGroupLog) ||
            RadGrid_gvLogs.MasterTableView.SortExpressions.Count > 0;



    }
    #endregion

    #region "Classification"
    private void FillEquipClassification()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();

        _GetLeadEquipClassification.ConnConfig = Session["config"].ToString();
        _GetEquipClassification.ConnConfig = Session["config"].ToString();
        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString() == "addprospect")
            {
                List<GetEquiptypeViewModel> _lstGetEquiptype = new List<GetEquiptypeViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "EquipmentAPI/AddEquipment_GetLeadEquipClassification";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetLeadEquipClassification);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetEquiptype = serializer.Deserialize<List<GetEquiptypeViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetEquiptypeViewModel>(_lstGetEquiptype);
                }
                else
                {
                    ds = objBL_User.getLeadEquipClassification(objPropUser);
                }
            }

            else
            {
                List<GetEquiptypeViewModel> _lstGetEquiptype = new List<GetEquiptypeViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "EquipmentAPI/EquipmentsList_GetEquipClassification";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetEquipClassification);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetEquiptype = serializer.Deserialize<List<GetEquiptypeViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetEquiptypeViewModel>(_lstGetEquiptype);
                }
                else
                {
                    ds = objBL_User.getEquipClassificationActive(objPropUser);
                }
            }
        }
        else
        {
            List<GetEquiptypeViewModel> _lstGetEquiptype= new List<GetEquiptypeViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/EquipmentsList_GetEquipClassification";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetEquipClassification);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetEquiptype = serializer.Deserialize<List<GetEquiptypeViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetEquiptypeViewModel>(_lstGetEquiptype);
            }
            else
            {
                ds = objBL_User.getEquipClassificationActive(objPropUser);
            }
        }
        ddlClassification.DataSource = ds.Tables[0];
        ddlClassification.DataTextField = "edesc";
        ddlClassification.DataValueField = "edesc";
        ddlClassification.DataBind();
        ddlClassification.Items.Insert(0, new ListItem("None", "None"));       
        lblClassification.Text = "Classification";
        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Label"].ToString() != "")
        {
            lblClassification.Text = ds.Tables[0].Rows[0]["Label"].ToString();
        }
    }
    #endregion

    protected void chkShutdown_Changed(object sender, EventArgs e)
    {
        //if(ViewState["shut_down"].ToString().ToLower() == "true")
        if(hdnEqShutdownStatus.Value == "1")
        {
            divShutdownReason.Visible = false;
            if (chkShutdown.Checked)
            {
                //rfvShutdownReason.Enabled = true;
                divShutdownReasonDesc.Visible = true;
                
            }
            else
            {
                //rfvShutdownReason.Enabled = false;
                //txtShutdownReason.Enabled = true;
                //txtShutdownReason.Text = string.Empty;
                //ddlShutdownReason.SelectedValue = "0";
                divShutdownReasonDesc.Visible = false;
            }
        }
        else
        {
            divShutdownReasonDesc.Visible = false;
            if (chkShutdown.Checked)
            {
                //rfvShutdownReason.Enabled = true;
                divShutdownReason.Visible = true;
            }
            else
            {
                //rfvShutdownReason.Enabled = false;
                //txtShutdownReason.Enabled = true;
                //txtShutdownReason.Text = string.Empty;
                //ddlShutdownReason.SelectedValue = "0";
                divShutdownReason.Visible = false;
            }
        }
        
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    private void FillShutdownReasons()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        _GetShutdownReasons.ConnConfig = Session["config"].ToString();

        List<GetShutdownReasonsViewModel> _lstGetShutdownReasons = new List<GetShutdownReasonsViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "EquipmentAPI/AddEquipment_GetShutdownReasons";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetShutdownReasons);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetShutdownReasons = serializer.Deserialize<List<GetShutdownReasonsViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetShutdownReasonsViewModel>(_lstGetShutdownReasons);
        }
        else
        {
            ds = objBL_User.GetShutdownReasons(objPropUser);
        }

        ddlShutdownReason.DataSource = ds.Tables[0];
        ddlShutdownReason.DataTextField = "Reason";
        ddlShutdownReason.DataValueField = "ID";
        ddlShutdownReason.DataBind();
        ddlShutdownReason.Items.Insert(0, new ListItem("Select", "0"));
    }

    protected void ddlShutdownReason_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlShutdownReason.SelectedValue == "0")
        {
            //lnkEditShutdownReason.Enabled = false;
            //lnkEditShutdownReason.Visible = false;

            //txtShutdownReason.Enabled = true;
            //txtShutdownReason.Text = string.Empty;

            hdnShutdownReasonPlanned.Value = "0";
            txtShutdownReason.Text = string.Empty;
        }
        else {
            //lnkEditShutdownReason.Enabled = true;
            //lnkEditShutdownReason.Visible = true;

            //hdnShutdownReasonPlanned.Value = "1";
            objPropUser.ConnConfig = Session["config"].ToString();
            _GetShutdownReasonByID.ConnConfig = Session["config"].ToString();

            var inteqsdReasonID = string.IsNullOrEmpty(ddlShutdownReason.SelectedValue) ? 0 : Convert.ToInt32(ddlShutdownReason.SelectedValue);
            DataSet ds = new DataSet();

            List<GetShutdownReasonsViewModel> _lstGetShutdownReasons = new List<GetShutdownReasonsViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/AddEquipment_GetShutdownReasonByID";

                _GetShutdownReasonByID.eqsdReasonID = inteqsdReasonID;

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetShutdownReasonByID);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetShutdownReasons = serializer.Deserialize<List<GetShutdownReasonsViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetShutdownReasonsViewModel>(_lstGetShutdownReasons);
            }
            else
            {
                ds = objBL_User.GetShutdownReasonByID(objPropUser, inteqsdReasonID);
            }

            if(ds.Tables[0].Rows.Count > 0)
            {
                hdnShutdownReasonPlanned.Value = ds.Tables[0].Rows[0]["Planned"].ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase) ? "1" : "0";

                //if (hdnShutdownReasonPlanned.Value == "1")
                //{
                //    txtShutdownReason.Text = ddlShutdownReason.SelectedItem.Text;
                //    //txtShutdownReason.Enabled = false;
                //}
                //else
                //{
                //    //txtShutdownReason.Text = ds.Tables[0].Rows[0]["Description"].ToString();
                //    //txtShutdownReason.Enabled = true;
                //    txtShutdownReason.Text = string.Empty;
                //}
            }
            //else
            //{
            //    hdnShutdownReasonPlanned.Value = "0";
            //    txtShutdownReason.Text = string.Empty;
            //    //txtShutdownReason.Enabled = true;
            //}

            txtShutdownReason.Text = ddlShutdownReason.SelectedItem.Text;
        }

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void lnkPopupSave_Click(object sender, EventArgs e)
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.MOMUSer = Session["User"].ToString();

        _AddShutdownReason.ConnConfig = Session["config"].ToString();
        _AddShutdownReason.MOMUSer = Session["User"].ToString();

        _EditShutdownReason.ConnConfig = Session["config"].ToString();
        _EditShutdownReason.MOMUSer = Session["User"].ToString();

        _GetShutdownReasons.ConnConfig = Session["config"].ToString();

        if (hdnShutdownReasonMode.Value == "0") // Add Shut Down Reason
        {
            try
            {
                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "EquipmentAPI/AddEquipment_AddShutdownReason";

                    _AddShutdownReason.eqsdReason = txtPopupShutdownReason.Text;
                    _AddShutdownReason.eqsdPlanned = chkPopupPlanned.Checked;

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddShutdownReason);
                }
                else
                {
                    objBL_User.AddShutdownReason(objPropUser, txtPopupShutdownReason.Text, chkPopupPlanned.Checked);
                }

                DataSet ds = new DataSet();

                List<GetShutdownReasonsViewModel> _lstGetShutdownReasons = new List<GetShutdownReasonsViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "EquipmentAPI/AddEquipment_GetShutdownReasons";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetShutdownReasons);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetShutdownReasons = serializer.Deserialize<List<GetShutdownReasonsViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetShutdownReasonsViewModel>(_lstGetShutdownReasons);
                }
                else
                {
                    ds = objBL_User.GetShutdownReasons(objPropUser);
                }

                ddlShutdownReason.DataSource = ds.Tables[0];
                ddlShutdownReason.DataTextField = "Reason";
                ddlShutdownReason.DataValueField = "ID";
                ddlShutdownReason.DataBind();
                ddlShutdownReason.Items.Insert(0, new ListItem("Select", "0"));

                // Updated Plannned hidden value
                hdnShutdownReasonPlanned.Value = "0";
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseShutdownReasonWindow();", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        else // mode = 1: Edit Shut Down Reason
        {
            //txtShutdownReason.Text = "Edit mode";
            try
            {
                var sdID = string.IsNullOrEmpty(ddlShutdownReason.SelectedValue) ? 0 : Convert.ToInt32(ddlShutdownReason.SelectedValue);

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "EquipmentAPI/AddEquipment_EditShutdownReason";

                    _EditShutdownReason.eqsdID = sdID;
                    _EditShutdownReason.eqsdReason = txtPopupShutdownReason.Text;
                    _EditShutdownReason.eqsdPlanned = chkPopupPlanned.Checked;

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _EditShutdownReason);
                }
                else
                {
                    objBL_User.EditShutdownReason(objPropUser, sdID, txtPopupShutdownReason.Text, chkPopupPlanned.Checked);
                }

                DataSet ds = new DataSet();

                List<GetShutdownReasonsViewModel> _lstGetShutdownReasons = new List<GetShutdownReasonsViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "EquipmentAPI/AddEquipment_GetShutdownReasons";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetShutdownReasons);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetShutdownReasons = serializer.Deserialize<List<GetShutdownReasonsViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetShutdownReasonsViewModel>(_lstGetShutdownReasons);
                }
                else
                {
                    ds = objBL_User.GetShutdownReasons(objPropUser);
                }

                ddlShutdownReason.DataSource = ds.Tables[0];
                ddlShutdownReason.DataTextField = "Reason";
                ddlShutdownReason.DataValueField = "ID";
                ddlShutdownReason.DataBind();
                ddlShutdownReason.Items.Insert(0, new ListItem("Select", "0"));
                // set selected value for drop down list
                ddlShutdownReason.SelectedValue = sdID.ToString();
                // Updated Plannned hidden value
                hdnShutdownReasonPlanned.Value = chkPopupPlanned.Checked ? "1" : "0";
                txtShutdownReason.Text = ddlShutdownReason.SelectedItem.Text;
                //if (hdnShutdownReasonPlanned.Value == "1")
                //{
                //    txtShutdownReason.Text = ddlShutdownReason.SelectedItem.Text;
                //    //txtShutdownReason.Enabled = false;
                //}
                //else
                //{
                //    txtShutdownReason.Text = string.Empty;
                //    //txtShutdownReason.Enabled = true;
                //}
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "CloseShutdownReasonWindow();", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }

    protected void lnkPopupOK_Click(object sender, EventArgs e)
    {
        //txtShutdownReason.Text = txtPopupShutdownReasonDesc.Text;
        hdnShutdownLongDesc.Value = txtPopupShutdownReasonDesc.Text;
        this.btnSubmit_Click(sender, e);

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void RadGrid_gvShutdownLogs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_gvShutdownLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupShutdownLogs();

        if (Request.QueryString["uid"] != null)
        {
            objPropUser.EquipID = Convert.ToInt32(Request.QueryString["uid"]);
            objPropUser.ConnConfig = Session["config"].ToString();

            _GetEquipShutdownLogs.EquipID = Convert.ToInt32(Request.QueryString["uid"]);
            _GetEquipShutdownLogs.ConnConfig = Session["config"].ToString();

            DataSet ds = new DataSet();

            List<GetEquipShutdownLogsViewModel> _lstGetEquipShutdownLogs = new List<GetEquipShutdownLogsViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/AddEquipment_GetEquipShutdownLogs";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetEquipShutdownLogs);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetEquipShutdownLogs = serializer.Deserialize<List<GetEquipShutdownLogsViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetEquipShutdownLogsViewModel>(_lstGetEquipShutdownLogs);
            }
            else
            {
                ds = objBL_User.GetEquipShutdownLogs(objPropUser);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                RadGrid_gvShutdownLogs.VirtualItemCount = ds.Tables[0].Rows.Count;
                RadGrid_gvShutdownLogs.DataSource = ds.Tables[0];
            }
            else
            {
                RadGrid_gvShutdownLogs.DataSource = string.Empty;
            }
        }
    }

    protected void RadGrid_gvShutdownLogs_ItemCreated(object sender, GridItemEventArgs e)
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

    bool isGroupShutdownLog = false;
    public bool ShouldApplySortFilterOrGroupShutdownLogs()
    {
        return RadGrid_gvShutdownLogs.MasterTableView.FilterExpression != "" ||
            (RadGrid_gvShutdownLogs.MasterTableView.GroupByExpressions.Count > 0 || isGroupShutdownLog) ||
            RadGrid_gvShutdownLogs.MasterTableView.SortExpressions.Count > 0;
    }

    protected void lnkReport_Click(object sender, EventArgs e)
    {
        var eqId = Request.QueryString["uid"];
        if(!string.IsNullOrEmpty(eqId))
            Response.Redirect("EquipmentShutdownReport.aspx?type=1&eqId=" + eqId);
    }

    private void ShowMCPTabs(bool isShow)
    {
        if (isShow)
        {
            tpREP.Style["display"] = "block";
            tpnlREPH.Style["display"] = "block";
            liMCPHistory.Style["display"] = "inline-block";
            liMCPTemp.Style["display"] = "inline-block";
        }
        else
        {
            tpREP.Style["display"] = "none";
            tpnlREPH.Style["display"] = "none";
            liMCPHistory.Style["display"] = "none";
            liMCPTemp.Style["display"] = "none";
        }
    }

    protected void lnkPrintMCP_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["uid"] != null)
        {
            // Redirect when close the report
            var redirect = HttpUtility.UrlEncode(Request.RawUrl);

            Response.Redirect("MaintenanceControlPlan.aspx?eid=" + Convert.ToInt32(Request.QueryString["uid"]) + "&redirect=" + redirect);
        }
    }
    private void GetFillLocation()
    {
        BL_User objBL_User = new BL_User();
        User obj = new User();
        DataSet ds = new DataSet();

        if (Request.QueryString["cuid"] != null)
        {
            obj.CustomerID =Convert.ToInt32( Request.QueryString["cuid"].ToString());
            obj.ConnConfig = HttpContext.Current.Session["config"].ToString();
            obj.DBName = Session["dbname"].ToString();

            //API
            _GetLocationByCustomerID.CustomerID = Convert.ToInt32(Request.QueryString["cuid"].ToString());
            _GetLocationByCustomerID.ConnConfig = HttpContext.Current.Session["config"].ToString();
            _GetLocationByCustomerID.DBName = Session["dbname"].ToString();

            List<GetLocationByCustomerIDViewModel> _lstGetLocationByCustomerID = new List<GetLocationByCustomerIDViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "EquipmentAPI/AddEquipment_GetLocationByCustomerID";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetLocationByCustomerID);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetLocationByCustomerID = serializer.Deserialize<List<GetLocationByCustomerIDViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetLocationByCustomerIDViewModel>(_lstGetLocationByCustomerID);
            }
            else
            {
                ds = objBL_User.getLocationByCustomerID(obj);
            }

        }

        if (ds!=null && ds.Tables.Count>0 && ds.Tables[0].Rows.Count == 1)
        {
            hdnLocId.Value = ds.Tables[0].Rows[0]["loc"].ToString(); 
            txtLocation.Text = ds.Tables[0].Rows[0]["Tag"].ToString();
        }
    }

    protected void lnkEquipmentCustomDetailReport_Click(object sender, EventArgs e)
    {
        var eqId = Request.QueryString["uid"];
        if (!string.IsNullOrEmpty(eqId))
            Response.Redirect("EquipmentCustomDetailReport.aspx?type=1&eqId=" + eqId);
    }
}
