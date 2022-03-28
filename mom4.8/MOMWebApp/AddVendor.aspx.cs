using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CommonModel;
using BusinessEntity.Utility;
using BusinessLayer;
using System.Linq;
using MOMWebApp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using System.Linq.Dynamic;
public partial class AddVendor : System.Web.UI.Page
{
    #region Variables
    BL_Vendor objBL_Vendor = new BL_Vendor();
    Vendor _objvendor = new Vendor();
    private double PrevRuntotal = 0.00;
    BL_BankAccount _objBLBank = new BL_BankAccount();
    Rol _objRol = new Rol();
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    User _objPropUser = new User();
    BL_User _objBLUser = new BL_User();
    BL_Bills objBLBill = new BL_Bills();
    BusinessEntity.CompanyOffice objCompany = new BusinessEntity.CompanyOffice();
    BL_Company objBL_Company = new BL_Company();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();
    General _objPropGeneral = new General();
    BL_General _objBLGeneral = new BL_General();
    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();

    //API Variables

    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    APIIntegrationModel _objAPIIntegration = new APIIntegrationModel();
    getSTaxParam _getSTax = new getSTaxParam();
    getCustomFieldsParam _getCustomFields = new getCustomFieldsParam();
    getCustomFieldsControlParam _getCustomFieldsControl = new getCustomFieldsControlParam();
    GetChartParam _getChart = new GetChartParam();
    getUseTaxParam _getUseTax = new getUseTaxParam();
    GetCompanyByCustomerParam _getCompanyByCustomer = new GetCompanyByCustomerParam();
    GetVendorEditParam _getVendorEdit = new GetVendorEditParam();
    getVendorContactByRolIDParam _getVendorContactByRolID = new getVendorContactByRolIDParam();
    UpdateRolParam _updateRole = new UpdateRolParam();
    UpdateRolesParam _updateRoles = new UpdateRolesParam();
    IsExistForUpdateVendorParam _IsExistForUpdateVendor = new IsExistForUpdateVendorParam();
    UpdateVendorParam _updateVendor = new UpdateVendorParam();
    UpdateVendorTaxParam _updateVendorTax = new UpdateVendorTaxParam();
    AddRolParam _addRol = new AddRolParam();
    IsExistsForInsertVendorParam _IsExistsForInsertVendor = new IsExistsForInsertVendorParam();
    AddVendorParam _addVendor = new AddVendorParam();
    GetTermsParam _getTerms = new GetTermsParam();
    UpdateVendorContactParam _updateVendorContact = new UpdateVendorContactParam();
    GetAPExpensesParam _getAPExpenses = new GetAPExpensesParam();
    getVendorTypeParam _getVendorType = new getVendorTypeParam();
    GetVendorLogsParam _getVendorLogs = new GetVendorLogsParam();

    private int uniqueRowId = 1;
    private int rolID = 0;
    #endregion

    #region Events

    #region PAGELOAD
    protected void Page_PreRender(Object o, EventArgs e)
    {
        //foreach (GridViewRow gr in gvContacts.Rows)
        //{

        //    HiddenField hdnSelected = (HiddenField)gr.FindControl("hdnSelected");
        //    Label lblMail = (Label)gr.FindControl("lblEmail");
        //    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
        //    // CheckBox chkShutdown = (CheckBox)gr.FindControl("chkShutdown");
        //    if (hdnEditeContact.Value == "Y")
        //    {
        //        gr.Attributes["ondblclick"] = "clickEdit('" + hdnSelected.ClientID + "','" + chkSelect.ClientID + "','" + btnEdit.ClientID + "');";
        //        gr.Attributes["onclick"] = "SelectRowmail('" + hdnSelected.ClientID + "','" + gr.ClientID + "','" + lblMail.ClientID + "','" + chkSelect.ClientID + "','" + gvContacts.ClientID + "','" + lnkMail.ClientID + "');";
        //    }
        //    else
        //    {
        //        // chkSelect.Enabled = chkShutdown.Enabled = false;
        //        gr.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";
        //    }
        //}

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }
            string _connectionString = Session["config"].ToString();

            if (!IsPostBack)
            {
                if (Session["VendorTransfromDate"] == null && Session["VendorTransToDate"] == null)
                {
                    txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                    txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                   Session["VendorTransfromDate"] = txtFromDate.Text;
                    Session["VendorTransToDate"] = txtToDate.Text;
                }
                else
                {
                    txtFromDate.Text =Session["VendorTransfromDate"].ToString();
                    txtToDate.Text = Session["VendorTransToDate"].ToString();
                }

                if (Session["VendorTransfromDate"] == null)
                {
                    txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                   Session["VendorTransfromDate"] = txtFromDate.Text;
                }
                else
                {
                    txtFromDate.Text =Session["VendorTransfromDate"].ToString();
                }



                 userpermissions();
                CompanyPermission();
                FillState();
                FillTerms();
                //FillSalesTax();
                FillUseTax();
                FillVendorType();
                ViewState["editcon"] = 0;
                ViewState["mode"] = 0;
                Session["contacttable"] = null;
                CreateTable();

                liTransactions.Style["display"] = "none";
                adTransactions.Style["display"] = "none";
                //FillCountry();
                if (Request.QueryString["uid"] == null)
                {
                    Page.Title = "Add Vendor || MOM";
                    ddlCompany.Visible = true;
                    lblddlCompany.Visible = true;
                    txtCompany.Visible = false;
                    lbltxtCompany.Visible = false;
                    btnCompanyPopUp.Visible = false;
                    FillSalesTax();
                }
                if (Request.QueryString["id"] != null)  //Edit COA
                {
                    Page.Title = "Edit Vendor || MOM";
                    ddlCompany.Visible = false;
                    lblddlCompany.Visible = false;
                    lbltxtCompany.Visible = true;
                    txtCompany.Visible = true;
                    btnCompanyPopUp.Visible = true;
                    liTransactions.Style["display"] = "";
                    adTransactions.Style["display"] = "";
                    liLogs.Style["display"] = "inline-block";
                    tbLogs.Style["display"] = "block";
                    _objvendor.ConnConfig = _connectionString;
                    _getVendorEdit.ConnConfig = _connectionString;
                    SetDataForEdit();
                    
                    if (Request.QueryString["t"] != null)
                    {
                        ViewState["mode"] = 0;
                        lblHeader.Text = "Add New Vendor";
                        liLogs.Style["display"] = "none";
                        tbLogs.Style["display"] = "none";
                        txtBalance.Text = "0.00";
                        lblVendorName.Text = "";
                    }
                    else
                    {
                        ViewState["mode"] = 1;
                        lblHeader.Text = "Edit Vendor";


                    }
                    //lblAcctType.Text = ddlType.SelectedItem.Text;
                    //lblAcctNum.Text = txtAcctNum.Text;
                    //lblAcctName.Text = txtAcName.Text;




                    //foreach(DataListItem item in gvTrans.Items)
                    //{
                    //    if(item.ItemType == ListItemType.Footer)
                    //    {
                    //        Label lblFooterAmount = (Label)item.FindControl("lblFooterAmount");
                    //        if(ds.Tables[0].Rows.Count > 0)
                    //        {
                    //            lblFooterAmount.Text = string.Format("{0:c}", ds.Tables[0].Compute("sum(Amount)", string.Empty));
                    //        }
                    //    }
                    //}

                }
                Permission();

                HighlightSideMenu("acctPayable", "lnkVendors", "acctPayableSub");
            }



            txtGeolock.Visible = false;
            txtSince.Visible = false;
            txtLast.Visible = false;
            txtInuse.Visible = false;

            divGeolock.Visible = false;
            divSince.Visible = false;
            divLast.Visible = false;
            divInuse.Visible = false;

            pnlNext.Visible = false;
            if (Request.QueryString["id"] != null)
            {
                pnlNext.Visible = true;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void ddlState_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillSalesTax();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void btnSelectAdd_Click(object sender, EventArgs e)
    {
        try
        {
            FillSalesTax();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillSalesTax()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        _getSTax.ConnConfig = Session["config"].ToString();

        List<STaxViewModel> _STaxViewModel = new List<STaxViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "VendorAPI/AddVendor_GetSTax";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getSTax);

            _STaxViewModel = (new JavaScriptSerializer()).Deserialize<List<STaxViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<STaxViewModel>(_STaxViewModel);
        }
        else
        {
            ds = objBL_User.getSTax(objPropUser);
        }
        /////////// ES-3242 Vendor Add/Edit Vendor Filter out the sales taxes as per the state/province (AZHAR) DT31-12-2019///////

        DataTable dtaxdt = new DataTable();
        dtaxdt = ds.Tables[0];
        DataView dv = new DataView(dtaxdt);
        //dv.RowFilter = "state='' OR state='" + ddlState.SelectedValue.ToString() + "'"; // query example = "id = 10"
        dv.RowFilter = "state='' OR state='" + txtState.Text.ToString() + "'"; // query example = "id = 10"
        ddlSTax.DataSource = dv.ToTable();

        /////////// ES-3242 Vendor Add/Edit Vendor Filter out the sales taxes as per the state/province (AZHAR) DT31-12-2019///////
        //objBL_User.getSalesTax(objPropUser);
        ddlSTax.Items.Clear();
        //ddlSTax.DataSource = ds.Tables[0];
        ddlSTax.DataSource = dv.ToTable();
        ddlSTax.DataTextField = "Name";
        ddlSTax.DataValueField = "Name";
        ddlSTax.DataBind();
        ddlSTax.Items.Insert(0, new ListItem(":: Select ::", ""));

        ///////////// SALE Tax Label Text Changed///////////////////
        _objPropGeneral.ConnConfig = Session["config"].ToString();
        _objPropGeneral.CustomName = "Country";

        _getCustomFields.ConnConfig = Session["config"].ToString();
        _getCustomFields.CustomName = "Country";

        DataSet dsCustom = new DataSet();
        List<CustomViewModel> _CustomViewModel = new List<CustomViewModel>();
        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "VendorAPI/AddVendor_GetCustomFields";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomFields);

            _CustomViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomViewModel>>(_APIResponse.ResponseData);
            dsCustom = CommonMethods.ToDataSet<CustomViewModel>(_CustomViewModel);
        }
        else
        {
             dsCustom = _objBLGeneral.getCustomFields(_objPropGeneral);
        }
        if (dsCustom.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(dsCustom.Tables[0].Rows[0]["Label"].ToString()) && dsCustom.Tables[0].Rows[0]["Label"].ToString().Equals("1"))
            {
                spansalestax.InnerText = "Provincial Tax";
                //hdnGstTax.Value = dsCustom.Tables[0].Rows[0]["GstRate"].ToString();
                ////////////////////If GST Set 0 Then Again Show Sales Tax intead of Provicinal Tax ES-3180///////////////////////////////////////
                string gst_gstgl = "";
                string gst_gstrate = "";
                _objPropGeneral.ConnConfig = Session["config"].ToString();
                _getCustomFieldsControl.ConnConfig = Session["config"].ToString();

                DataSet _dsCustom = new DataSet();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "VendorAPI/AddVendor_GetCustomFieldsControl";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomFieldsControl);

                    _CustomViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomViewModel>>(_APIResponse.ResponseData);

                    _dsCustom = CommonMethods.ToDataSet<CustomViewModel>(_CustomViewModel);
                }
                else
                {
                    _dsCustom = _objBLGeneral.getCustomFieldsControl(_objPropGeneral);
                }
                if (_dsCustom.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
                    {


                        if (_dr["Name"].ToString().Equals("GSTGL"))
                        {
                            if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
                            {
                                _objChart.ConnConfig = Session["config"].ToString();
                                _objChart.ID = Convert.ToInt32(_dr["Label"].ToString());

                                _getChart.ConnConfig = Session["config"].ToString();
                                _getChart.ID = Convert.ToInt32(_dr["Label"].ToString());

                                DataSet _dsChart = new DataSet();

                                List<ChartViewModel> _ChartViewModel = new List<ChartViewModel>();

                                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                                //if (IsAPIIntegrationEnable == "YES")
                                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                                {
                                    string APINAME = "VendorAPI/AddVendor_GetChart";

                                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getChart);

                                    _ChartViewModel = (new JavaScriptSerializer()).Deserialize<List<ChartViewModel>>(_APIResponse.ResponseData);
                                    _dsChart = CommonMethods.ToDataSet<ChartViewModel>(_ChartViewModel);
                                }
                                else
                                {
                                    _dsChart = _objBLChart.GetChart(_objChart);
                                }
                                if (_dsChart.Tables[0].Rows.Count > 0)
                                {
                                    //txtGSTGL.Text = _dsChart.Tables[0].Rows[0]["fDesc"].ToString();
                                    gst_gstgl = _dr["Label"].ToString();
                                }

                            }
                            else
                            {
                                gst_gstgl = "0";
                            }
                        }
                        else if (_dr["Name"].ToString().Equals("GSTRate"))
                        {
                            if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
                            {
                                gst_gstrate = _dr["Label"].ToString();

                            }
                            else
                            {
                                gst_gstrate = "0";
                            }
                        }

                    }

                }

                if (gst_gstrate == "")
                {
                    gst_gstrate = "0";
                }
                if (gst_gstrate == "0" || gst_gstrate == "0.0000")
                {
                    spansalestax.InnerText = "Sales Tax";
                }
                ////////////////////////////////////////////////////////
            }
            else
            {
                spansalestax.InnerText = "Sales Tax";
            }
        }
        else
        {
            spansalestax.InnerText = "Sales Tax";
        }

        

        ///////////// SALE Tax Label Text Changed///////////////////


    }
    private void FillUseTax()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        _getUseTax.ConnConfig = Session["config"].ToString();

        List<STaxViewModel> _STaxViewModel = new List<STaxViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "VendorAPI/AddVendor_GetUseTax";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getUseTax);

            _STaxViewModel = (new JavaScriptSerializer()).Deserialize<List<STaxViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<STaxViewModel>(_STaxViewModel);
        }
        else
        {
            ds = objBL_User.getUseTax(objPropUser);
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlUseTax.DataSource = ds.Tables[0];
            ddlUseTax.DataTextField = "Name";
            ddlUseTax.DataValueField = "Name";
            ddlUseTax.DataBind();
            ddlUseTax.Items.Insert(0, new ListItem(":: Select ::", ""));
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
    private DataTable GetUserById()
    {
        User objPropUser = new User();
        objPropUser.TypeID = Convert.ToInt32(Session["usertypeid"]);
        objPropUser.UserID = Convert.ToInt32(Session["userid"]);
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.DBName = Session["dbname"].ToString();

        GetUserByIdParam _GetUserById = new GetUserByIdParam();
        _GetUserById.TypeID = Convert.ToInt32(Session["usertypeid"]);
        _GetUserById.UserID = Convert.ToInt32(Session["userid"]);
        _GetUserById.ConnConfig = Session["config"].ToString();
        _GetUserById.DBName = Session["dbname"].ToString();

        DataSet ds = new DataSet();
        List<UserViewModel> _UserViewModel = new List<UserViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {

            string APINAME = "VendorAPI/VendorList_GetUserById";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetUserById);

            _UserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<UserViewModel>(_UserViewModel);
        }
        else
        {
            ds = _objBLUser.GetUserPermissionByUserID(objPropUser);
        }
        return ds.Tables[0];
    }
    private void userpermissions()
    {
        if (Session["type"].ToString() != "c")
        {
            if (Session["type"].ToString() != "am")
            {
                _objPropUser.ConnConfig = Session["config"].ToString();
                _objPropUser.Username = Session["username"].ToString();
                _objPropUser.PageName = "addvendor.aspx";
                //DataSet dspage = _objBLUser.getScreensByUser(_objPropUser);
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
                if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
                {
                    DataTable dtUserPermission = new DataTable();
                    dtUserPermission = GetUserById();
                    /// AccountPayablemodulePermission ///////////////////------->

                    string AccountPayablemodulePermission = dtUserPermission.Rows[0]["AccountPayablemodulePermission"] == DBNull.Value ? "Y" : dtUserPermission.Rows[0]["AccountPayablemodulePermission"].ToString();

                    if (AccountPayablemodulePermission == "N")
                    {
                        Response.Redirect("Home.aspx?permission=no"); return;
                    }

                    /// Vendor  ///////////////////------->

                    string VendorPermission = dtUserPermission.Rows[0]["Vendor"] == DBNull.Value ? "YYYYYY" : dtUserPermission.Rows[0]["Vendor"].ToString();
                    string ADD = VendorPermission.Length < 1 ? "Y" : VendorPermission.Substring(0, 1);
                    string Edit = VendorPermission.Length < 2 ? "Y" : VendorPermission.Substring(1, 1);
                    string Delete = VendorPermission.Length < 3 ? "Y" : VendorPermission.Substring(2, 1);
                    string View = VendorPermission.Length < 4 ? "Y" : VendorPermission.Substring(3, 1);

                    if (Request.QueryString["id"] != null)
                    {
                        //aImport.Visible = false;
                    }
                    if (View == "N")
                    {
                        Response.Redirect("Home.aspx?permission=no"); return;
                    }
                    else if (Request.QueryString["id"] == null)
                    {
                        if (ADD == "N")
                        {
                            Response.Redirect("Home.aspx?permission=no"); return;
                        }
                    }
                    else if (Edit == "N")
                    {
                        if (View == "Y")
                        {
                            btnSubmit.Visible = false;
                            //btnSubmitJob.Visible = false;
                        }
                        else
                        {
                            Response.Redirect("Home.aspx?permission=no"); return;
                        }
                   }
                }
            }
        }
    }
    private void Permission()
    {

        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderCstm");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("cstmMgrSub");
        //ul.Style.Add("display", "block");
        //ul.Style.Add("visibility", "visible");

        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
            //Response.Redirect("addcustomer.aspx?uid=" + Session["userid"].ToString());
        }

        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx");
            //pnlGridButtons.Visible = false;
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }

        //DataTable dt = new DataTable();
        //dt = (DataTable)Session["userinfo"];

        //string ProgFunc = dt.Rows[0]["Control"].ToString().Substring(0, 1);
        //if (ProgFunc == "N")
        //{
        //    Response.Redirect("home.aspx");
        //}
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

        _getCompanyByCustomer.UserID = Convert.ToInt32(Session["UserID"].ToString());
        _getCompanyByCustomer.DBName = Session["dbname"].ToString();
        _getCompanyByCustomer.ConnConfig = Session["config"].ToString();

        DataSet ds = new DataSet();

        List<CompanyOfficeViewModel> _CompanyOfficeViewModel = new List<CompanyOfficeViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "VendorAPI/AddVendor_GetCompanyByCustomer";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCompanyByCustomer);

            _CompanyOfficeViewModel = (new JavaScriptSerializer()).Deserialize<List<CompanyOfficeViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<CompanyOfficeViewModel>(_CompanyOfficeViewModel);
        }
        else
        {
            ds = objBL_Company.getCompanyByCustomer(objCompany);
        }
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

    #endregion
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Submit();

        if (Convert.ToString(ViewState["ReturnTrue"]) == "false")
        {
           // ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'Vendor ID already exists.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
        else
        {

            #region Update Contact Grid 
            DataSet ds = new DataSet();
            _objvendor.ConnConfig = Session["config"].ToString();
            //_objvendor.ID = Convert.ToInt32(Request.QueryString["id"]);
            _objvendor.ID = Convert.ToInt32(ViewState["ReturnTrue"]);

            _getVendorEdit.ConnConfig = Session["config"].ToString();
            _getVendorEdit.ID = Convert.ToInt32(ViewState["ReturnTrue"]);

            List<VendorViewModel> _VendorViewModel = new List<VendorViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "VendorAPI/AddVendor_GetVendorEdit";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorEdit);

                _VendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<VendorViewModel>(_VendorViewModel);
                ds.Tables[0].Columns["ContactName"].ColumnName = "Contact";
                ds.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                ds.Tables[0].Columns["Vendor1099"].ColumnName = "1099";
            }
            else
            {
                ds = objBL_Vendor.GetVendorEdit(_objvendor);
            }
            DataRow dr = ds.Tables[0].Rows[0];

            _objvendor.ConnConfig = Session["config"].ToString();
            _objvendor.Rol = Convert.ToInt32(dr["Rol"].ToString());

            _getVendorContactByRolID.ConnConfig = Session["config"].ToString();
            _getVendorContactByRolID.Rol = Convert.ToInt32(dr["Rol"].ToString());
            
            DataSet dsVendorContact = new DataSet();

            List<GetVendorContactByRolIDViewModel> _GetVendorContactByRolIDViewModel = new List<GetVendorContactByRolIDViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "VendorAPI/AddVendor_GetVendorContactByRolID";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorContactByRolID);

                _GetVendorContactByRolIDViewModel = (new JavaScriptSerializer()).Deserialize<List<GetVendorContactByRolIDViewModel>>(_APIResponse.ResponseData);
                dsVendorContact = CommonMethods.ToDataSet<GetVendorContactByRolIDViewModel>(_GetVendorContactByRolIDViewModel);
            }
            else
            {
                dsVendorContact = objBL_Vendor.getVendorContactByRolID(_objvendor);
            }
            //gvContacts.DataSource = dsVendorContact.Tables[0];
            //gvContacts.DataBind();
            RadGrid_VendorContact.DataSource = dsVendorContact.Tables[0];
            RadGrid_VendorContact.Rebind();

            Session["contacttable"] = dsVendorContact.Tables[0];
            #endregion
        }

    }

    private void Submit()
    {
        try
        {
            if (Request.QueryString["id"] != null && Convert.ToInt32(ViewState["mode"]) == 1)
            {
                #region "EDIT Vendor"

                _objRol.ConnConfig = Session["config"].ToString();
                _objRol.ID = Convert.ToInt32(hdnRolID.Value);         //Update Vandor
                _objRol.Name = txtName.Text;
                _objRol.Address = txtAddress.Text;
                _objRol.City = txtCity.Text;
                _objRol.State = txtState.Text;
                _objRol.Country = txtCountry.Text;
                _objRol.EMail = txtEmailid.Text;
                _objRol.Website = txtWebsite.Text;
                _objRol.Zip = txtZip.Text;
                _objRol.Phone = txtPhone.Text;
                _objRol.Fax = txtFax.Text;
                _objRol.Contact = txtContact.Text;
                _objRol.Cellular = txtCellular.Text;
                //_objRol.Remarks = txtvenremark.Text;
                _objRol.GeoLock = Convert.ToInt32(txtGeolock.Text);
                _objRol.Since = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                _objRol.Last = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                _objRol.Type = 1;

                _updateRole.ConnConfig = Session["config"].ToString();
                _updateRole.ID = Convert.ToInt32(hdnRolID.Value);         //Update Vandor
                _updateRole.Name = txtName.Text;
                _updateRole.Address = txtAddress.Text;
                _updateRole.City = txtCity.Text;
                _updateRole.State = txtState.Text;
                _updateRole.Country = txtCountry.Text;
                _updateRole.EMail = txtEmailid.Text;
                _updateRole.Website = txtWebsite.Text;
                _updateRole.Zip = txtZip.Text;
                _updateRole.Phone = txtPhone.Text;
                _updateRole.Fax = txtFax.Text;
                _updateRole.Contact = txtContact.Text;
                _updateRole.Cellular = txtCellular.Text;
                //_objRol.Remarks = txtvenremark.Text;
                //_updateRole.GeoLock = Convert.ToInt32(txtGeolock.Text);
                _updateRole.Since = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                _updateRole.Last = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                _updateRole.Type = 1;

                if (Convert.ToInt32(ViewState["CompPermission"]) == 1)
                {
                    _objRol.EN = Convert.ToInt32(ddlCompanyEdit.SelectedValue);
                    _updateRole.EN = Convert.ToInt32(ddlCompanyEdit.SelectedValue); 
                }
                else 
                {
                    _objRol.EN = 0;
                    _updateRole.EN = 0; 
                }

                _objRol.MOMUSer = Session["User"].ToString();
                _updateRole.MOMUSer = Session["User"].ToString();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "VendorAPI/AddVendor_UpdateRole";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateRole);
                }
                else
                {
                    _objBLBank.UpdateRol(_objRol);
                }

                _objRol.ConnConfig = Session["config"].ToString();
                _objRol.Remarks = txtvenremark.Text;
                _objRol.ID = Convert.ToInt32(hdnRolID.Value);

                _updateRoles.ConnConfig = Session["config"].ToString();
                _updateRoles.Remarks = txtvenremark.Text;
                _updateRoles.ID = Convert.ToInt32(hdnRolID.Value);

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "VendorAPI/AddVendor_UpdateRoles";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateRoles);
                }
                else
                {
                    _objBLBank.UpdateRoles(_objRol);
                }

                _objvendor.Rol = Convert.ToInt32(hdnRolID.Value);
                _objvendor.ConnConfig = Session["config"].ToString();
                _objvendor.ID = Convert.ToInt32(Request.QueryString["id"]);

                _IsExistForUpdateVendor.Rol = Convert.ToInt32(hdnRolID.Value);
                _IsExistForUpdateVendor.ConnConfig = Session["config"].ToString();
                _IsExistForUpdateVendor.ID = Convert.ToInt32(Request.QueryString["id"]);

                _updateVendor.Rol = Convert.ToInt32(hdnRolID.Value);
                _updateVendor.ID = Convert.ToInt32(Request.QueryString["id"]);

                DataSet _dsIsAcctExitForEdit = new DataSet();
                List<VendorViewModel> _VendorViewModel = new List<VendorViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "VendorAPI/AddVendor_isExistForUpdateVendor";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _IsExistForUpdateVendor);
                    _VendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
                    _dsIsAcctExitForEdit = CommonMethods.ToDataSet<VendorViewModel>(_VendorViewModel);
                    _dsIsAcctExitForEdit.Tables[0].Columns["Count"].ColumnName = "CountVendor";
                }
                else
                {
                    _dsIsAcctExitForEdit = objBL_Vendor.IsExistForUpdateVendor(_objvendor);
                }
                int _count = Convert.ToInt32(_dsIsAcctExitForEdit.Tables[0].Rows[0]["CountVendor"]);
                if (_count.Equals(0))
                {
                    _objvendor.ConnConfig = Session["config"].ToString();
                    _objvendor.Acct = txtAccountid.Text.Replace("'", "''");

                    _IsExistForUpdateVendor.ConnConfig = Session["config"].ToString();
                    _IsExistForUpdateVendor.Acct = txtAccountid.Text.Replace("'", "''");

                    _updateVendor.Acct = txtAccountid.Text.Replace("'", "''");

                    DataSet _dsIsAcctExit = new DataSet();

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "VendorAPI/AddVendor_isExistForUpdateVendor";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _IsExistForUpdateVendor);
                        _VendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);

                        _dsIsAcctExit = CommonMethods.ToDataSet<VendorViewModel>(_VendorViewModel);
                        _dsIsAcctExit.Tables[0].Columns["Count"].ColumnName = "CountVendor";
                    }
                    else
                    {
                        _dsIsAcctExit = objBL_Vendor.IsExistForUpdateVendor(_objvendor);
                    }
                    int _count1 = Convert.ToInt32(_dsIsAcctExit.Tables[0].Rows[0]["CountVendor"]);
                    if (_count1.Equals(0))
                    {
                        GetVenderData();
                        if (ddlStatus.SelectedIndex != 0)
                        {
                            _objvendor.Status = Convert.ToInt16(ddlStatus.SelectedValue);
                            _updateVendor.Status = Convert.ToInt16(ddlStatus.SelectedValue);
                        }
                        else
                        {
                            _objvendor.Status = 0;
                            _updateVendor.Status = 0;
                        }
                        _objvendor.ShipVia = txtShipvia.Text;
                        _objvendor.Balance = Convert.ToDouble(txtBalance.Text);

                        _updateVendor.ShipVia = txtShipvia.Text;
                        _updateVendor.Balance = Convert.ToDouble(txtBalance.Text);

                        if (ddlTerms.SelectedIndex != 0)
                        {
                            _objvendor.Terms = Convert.ToInt16(ddlTerms.SelectedValue);
                            _updateVendor.Terms = Convert.ToInt16(ddlTerms.SelectedValue);
                        }
                        else
                        {
                            _objvendor.Terms = 1;
                            _updateVendor.Terms = 1;
                        }
                        if (!string.IsNullOrEmpty(txtDays.Text))
                        {
                            _objvendor.Days = Convert.ToInt16(txtDays.Text);
                            _updateVendor.Days = Convert.ToInt16(txtDays.Text);
                        }
                        if (!string.IsNullOrEmpty(txt1099Box.Text))
                        {
                            _objvendor.Vendor1099Box = Convert.ToInt16(txt1099Box.Text);
                            _updateVendor.Vendor1099Box = Convert.ToInt16(txt1099Box.Text);
                        }
                        else
                        {
                            _objvendor.Vendor1099Box = 0;
                            _updateVendor.Vendor1099Box = 0;
                        }

                        if (chk1099.Checked == true)
                        {
                            _objvendor.Vendor1099 = 1;
                            _updateVendor.Vendor1099 = 1;
                        }
                        else
                        {
                            _objvendor.Vendor1099 = 0;
                            _updateVendor.Vendor1099 = 0;
                        }

                        _objvendor.InUse = Convert.ToInt16(txtInuse.Text);
                        _updateVendor.InUse = Convert.ToInt16(txtInuse.Text);

                        if (!string.IsNullOrEmpty(txtCreditlimit.Text))
                        {
                            _objvendor.CLimit = Convert.ToDouble(txtCreditlimit.Text);
                            _updateVendor.CLimit = Convert.ToDouble(txtCreditlimit.Text);
                        }
                        if (!string.IsNullOrEmpty(hdnAcctID.Value))
                        {
                            _objvendor.DA = Convert.ToInt32(hdnAcctID.Value);
                            _updateVendor.DA = Convert.ToInt32(hdnAcctID.Value);
                        }
                        _objvendor.Type = ddlType.SelectedItem.Text;  // change by Mayuri 9th May, 16
                        _objvendor.Remit = txtRemitTo.Text;
                        _objvendor.FID = txtFedID.Text;
                        _objvendor.AcctNumber = txtAcct.Text;
                        _objvendor.VendorData = (DataTable)Session["contacttable"];

                        _objvendor.Email = txtEmailid.Text;
                        _objvendor.Cell = txtCellular.Text;
                        _objvendor.Phone = txtPhone.Text;
                        _objvendor.Fax = txtFax.Text;
                        _objvendor.ContactName = txtContact.Text;
                        _objvendor.EmailRecPO = chkmainEmailPO.Checked;
                        _objvendor.MOMUSer = Session["User"].ToString();
                        _objvendor.CourierAccount = txtCourieracct.Text.ToString().Trim();

                        _updateVendor.Type = ddlType.SelectedItem.Text;  // change by Mayuri 9th May, 16
                        _updateVendor.Remit = txtRemitTo.Text;
                        _updateVendor.FID = txtFedID.Text;
                        _updateVendor.AcctNumber = txtAcct.Text;

                        DataTable sessionData = (DataTable)Session["contacttable"];

                        if (sessionData.Rows.Count == 0)
                        {
                            DataTable returnVal = SaveEmptyDatatable();
                            _updateVendor.VendorData = returnVal;

                        }
                        else
                        {
                            _updateVendor.VendorData = (DataTable)Session["contacttable"];
                        }
                        
                        _updateVendor.Email = txtEmailid.Text;
                        _updateVendor.Cell = txtCellular.Text;
                        _updateVendor.Phone = txtPhone.Text;
                        _updateVendor.Fax = txtFax.Text;
                        _updateVendor.ContactName = txtContact.Text;
                        _updateVendor.EmailRecPO = chkmainEmailPO.Checked;
                        _updateVendor.MOMUSer = Session["User"].ToString();
                        

                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "VendorAPI/AddVendor_UpdateVendor";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateVendor);

                        }
                        else
                        {
                            objBL_Vendor.UpdateVendor(_objvendor);
                        }
                        ViewState["ReturnTrue"] = _objvendor.ID.ToString();

                        // Update Sale Tax And Use Tax //
                        _objvendor.ConnConfig = Session["config"].ToString();
                        _objvendor.ID = Convert.ToInt32(Request.QueryString["id"]);

                        _updateVendorTax.ConnConfig = Session["config"].ToString();
                        _updateVendorTax.ID = Convert.ToInt32(Request.QueryString["id"]);

                        if (ddlSTax.SelectedIndex != 0)
                        {
                            _objvendor.Custom1 = ddlSTax.SelectedValue.ToString();
                            _updateVendorTax.Custom1 = ddlSTax.SelectedValue.ToString();
                        }
                        else
                        {
                            _objvendor.Custom1 = "";
                            _updateVendorTax.Custom1 = "";
                        }
                        if (ddlUseTax.SelectedIndex != 0)
                        {
                            _objvendor.Custom2 = ddlUseTax.SelectedValue.ToString();
                            _updateVendorTax.Custom2 = ddlUseTax.SelectedValue.ToString();
                        }
                        else
                        {
                            _objvendor.Custom2 = "";
                            _updateVendorTax.Custom2 = "";
                        }
                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "VendorAPI/AddVendor_UpdateVendorTax";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateVendorTax);
                        }
                        else
                        {
                            objBL_Vendor.UpdateVendorTax(_objvendor);
                        }
                        string strScript = string.Empty;
                        strScript += "noty({text: 'Vendor updated successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", strScript, true);
                        RadGrid_gvLogs.Rebind();
                        // Response.Redirect("~/Vendors.aspx");
                    }
                    else
                    {
                        if (ddlType.SelectedItem.Text.Equals("Bank"))
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "resize controls", "ResizeControls('true');", true);
                        }
                        ViewState["ReturnTrue"] = "false";
                        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'This acct# number already exist.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
                #endregion

            }
            else
            {
                _objRol.ConnConfig = Session["config"].ToString();
                _objRol.Name = txtName.Text;
                _objRol.Address = txtAddress.Text;
                _objRol.City = txtCity.Text;
                //_objRol.State =ddlState.SelectedValue;
                _objRol.State = txtState.Text;
                _objRol.Country = txtCountry.Text;
                _objRol.EMail = txtEmailid.Text;
                _objRol.Website = txtWebsite.Text;
                _objRol.Zip = txtZip.Text;
                _objRol.Phone = txtPhone.Text;
                _objRol.Fax = txtFax.Text;
                _objRol.Contact = txtContact.Text;
                _objRol.Cellular = txtCellular.Text;
                _objRol.GeoLock = Convert.ToInt32(txtGeolock.Text);
                _objRol.Since = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                _objRol.Last = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                _objRol.Type = 1;                                           // change by Mayuri 9th May, 16

                _addRol.ConnConfig = Session["config"].ToString();
                _addRol.Name = txtName.Text;
                _addRol.Address = txtAddress.Text;
                _addRol.City = txtCity.Text;
                //_objRol.State =ddlState.SelectedValue;
                _addRol.State = txtState.Text;
                _addRol.Country = txtCountry.Text;
                _addRol.EMail = txtEmailid.Text;
                _addRol.Website = txtWebsite.Text;
                _addRol.Zip = txtZip.Text;
                _addRol.Phone = txtPhone.Text;
                _addRol.Fax = txtFax.Text;
                _addRol.Contact = txtContact.Text;
                _addRol.Cellular = txtCellular.Text;
                _addRol.GeoLock = Convert.ToInt32(txtGeolock.Text);
                _addRol.Since = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                _addRol.Last = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                _addRol.Type = 1;                                           // change by Mayuri 9th May, 16

                if (Convert.ToInt32(ViewState["CompPermission"]) == 1)
                {
                    _objRol.EN = Convert.ToInt32(ddlCompany.SelectedValue);
                    _addRol.EN = Convert.ToInt32(ddlCompany.SelectedValue);
                }
                else
                {
                    _objRol.EN = 0;
                    _addRol.EN = 0;
                }

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "VendorAPI/AddVendor_AddRol";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _addRol);
                    _objvendor.Rol = Convert.ToInt32(_APIResponse.ResponseData);
                    _addVendor.Rol = Convert.ToInt32(_APIResponse.ResponseData);
                }
                else
                {
                    _objvendor.Rol = _objBLBank.AddRol(_objRol);
                }

                ViewState["rolid"] = _objvendor.Rol;
                //_objvendor.Rol = Convert.ToInt32(_dsRol.Tables[0].Rows[0]["RolID"]);

                _objRol.ConnConfig = Session["config"].ToString();
                _objRol.Remarks = txtvenremark.Text;
                _objRol.ID = Convert.ToInt32(ViewState["rolid"]);

                _updateRoles.ConnConfig = Session["config"].ToString();
                _updateRoles.Remarks = txtvenremark.Text;
                _updateRoles.ID = Convert.ToInt32(ViewState["rolid"]);

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "VendorAPI/AddVendor_UpdateRoles";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateRoles);
                }
                else
                {
                    _objBLBank.UpdateRoles(_objRol);
                }

                _objvendor.ConnConfig = Session["config"].ToString();
                _objvendor.Acct = txtAccountid.Text.Replace("'", "''");

                _IsExistsForInsertVendor.ConnConfig = Session["config"].ToString();
                _IsExistsForInsertVendor.Acct = txtAccountid.Text.Replace("'", "''");

                //_addVendor.Acct = txtAccountid.Text.Replace("'", "''");

                DataSet _dsIsAcctExit = new DataSet();
                List<VendorViewModel> _VendorViewModel = new List<VendorViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "VendorAPI/AddVendor_isExistsForInsertVendor";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _IsExistsForInsertVendor);
                    _VendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
                    _dsIsAcctExit = CommonMethods.ToDataSet<VendorViewModel>(_VendorViewModel);
                    _dsIsAcctExit.Tables[0].Columns["Count"].ColumnName = "CountVendor";
                }
                else
                {
                    _dsIsAcctExit = objBL_Vendor.IsExistsForInsertVendor(_objvendor);
                }
                int _count1 = Convert.ToInt32(_dsIsAcctExit.Tables[0].Rows[0]["CountVendor"]);
                if (_count1.Equals(0))
                {
                    GetVenderData();
                    if (ddlStatus.SelectedIndex != 0)
                    {
                        _objvendor.Status = Convert.ToInt16(ddlStatus.SelectedValue);
                        _addVendor.Status = Convert.ToInt16(ddlStatus.SelectedValue);
                    }
                    else
                    {
                        _objvendor.Status = 0;
                        _addVendor.Status = 0;
                    }
                    _objvendor.ShipVia = txtShipvia.Text;
                    _addVendor.ShipVia = txtShipvia.Text;

                    if (string.IsNullOrEmpty(txtBalance.Text))
                        txtBalance.Text = "0.00";

                    _objvendor.Balance = Convert.ToDouble(txtBalance.Text);
                    _addVendor.Balance = Convert.ToDouble(txtBalance.Text);

                    if (!string.IsNullOrEmpty(txtCreditlimit.Text))
                    {
                        _objvendor.CLimit = Convert.ToDouble(txtCreditlimit.Text);
                        _addVendor.CLimit = Convert.ToDouble(txtCreditlimit.Text);
                    }

                    if (ddlTerms.SelectedIndex != 0)
                    {
                        _objvendor.Terms = Convert.ToInt16(ddlTerms.SelectedValue);
                        _addVendor.Terms = Convert.ToInt16(ddlTerms.SelectedValue);
                    }
                    else
                    {
                        _objvendor.Terms = 1;
                        _addVendor.Terms = 1;
                    }
                    if (!string.IsNullOrEmpty(txtDays.Text))
                    {
                        _objvendor.Days = Convert.ToInt16(txtDays.Text);
                        _addVendor.Days = Convert.ToInt16(txtDays.Text);
                    }
                    if (!string.IsNullOrEmpty(txt1099Box.Text))
                    {
                        _objvendor.Vendor1099Box = Convert.ToInt16(txt1099Box.Text);
                        _addVendor.Vendor1099Box = Convert.ToInt16(txt1099Box.Text);
                    }
                    else
                    {
                        _objvendor.Vendor1099Box = 0;
                        _addVendor.Vendor1099Box = 0;
                    }

                    if (chk1099.Checked == true)
                    {
                        _objvendor.Vendor1099 = 1;
                        _addVendor.Vendor1099 = 1;
                    }
                    else
                    {
                        _objvendor.Vendor1099 = 0;
                        _addVendor.Vendor1099 = 0;
                    }

                    _objvendor.InUse = Convert.ToInt16(txtInuse.Text);
                    _addVendor.InUse = Convert.ToInt16(txtInuse.Text);

                    if (!string.IsNullOrEmpty(hdnAcctID.Value))
                    {
                        _objvendor.DA = Convert.ToInt32(hdnAcctID.Value);
                        _addVendor.DA = Convert.ToInt32(hdnAcctID.Value);
                    }

                    _objvendor.Type = ddlType.SelectedItem.Text;     // change by Mayuri 9th May, 16
                    _objvendor.FID = txtFedID.Text;
                    _objvendor.Remit = txtRemitTo.Text;
                    _objvendor.AcctNumber = txtAcct.Text;
                    _objvendor.VendorData = (DataTable)Session["contacttable"];
                    _objvendor.Email = txtEmailid.Text;
                    _objvendor.Cell = txtCellular.Text;
                    _objvendor.Phone = txtPhone.Text;
                    _objvendor.Fax = txtFax.Text;
                    _objvendor.ContactName = txtContact.Text;
                    _objvendor.EmailRecPO = chkmainEmailPO.Checked;
                    _objvendor.MOMUSer = Session["User"].ToString();
                    _objvendor.CourierAccount = txtCourieracct.Text.ToString().Trim();

                    _addVendor.Type = ddlType.SelectedItem.Text;     // change by Mayuri 9th May, 16
                    _addVendor.FID = txtFedID.Text;
                    _addVendor.Remit = txtRemitTo.Text;
                    _addVendor.AcctNumber = txtAcct.Text;

                    DataTable sessionData = (DataTable)Session["contacttable"];

                    if (sessionData.Rows.Count == 0)
                    {
                        DataTable returnVal = SaveEmptyDatatable();
                        _addVendor.VendorData = returnVal;
                      
                    }
                    else
                    {
                        _addVendor.VendorData = (DataTable)Session["contacttable"];
                    }

                    _addVendor.Email = txtEmailid.Text;
                    _addVendor.Cell = txtCellular.Text;
                    _addVendor.Phone = txtPhone.Text;
                    _addVendor.Fax = txtFax.Text;
                    _addVendor.ContactName = txtContact.Text;
                    _addVendor.EmailRecPO = chkmainEmailPO.Checked;
                    _addVendor.MOMUSer = Session["User"].ToString();

                    int VendorID = 0;

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "VendorAPI/AddVendor_AddVendor";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _addVendor);
                        VendorID = Convert.ToInt32(_APIResponse.ResponseData);
                    }
                    else
                    {
                        VendorID = objBL_Vendor.AddVendor(_objvendor);
                    }

                    // Update Sale Tax And Use Tax //
                    _objvendor.ConnConfig = Session["config"].ToString();
                    _objvendor.ID = VendorID;

                    _updateVendorTax.ConnConfig = Session["config"].ToString();
                    _updateVendorTax.ID = VendorID;
                    
                    if (ddlSTax.SelectedIndex != 0)
                    {
                        _objvendor.Custom1 = ddlSTax.SelectedValue.ToString();
                        _updateVendorTax.Custom1 = ddlSTax.SelectedValue.ToString();
                    }
                    else
                    {
                        _objvendor.Custom1 = "";
                        _updateVendorTax.Custom1 = "";
                    }
                    if (ddlUseTax.SelectedIndex != 0)
                    {
                        _objvendor.Custom2 = ddlUseTax.SelectedValue.ToString();
                        _updateVendorTax.Custom2 = ddlUseTax.SelectedValue.ToString();
                    }
                    else
                    {
                        _objvendor.Custom2 = "";
                        _updateVendorTax.Custom2 = "";
                    }

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "VendorAPI/AddVendor_UpdateVendorTax";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateVendorTax);
                    }
                    else
                    {
                        objBL_Vendor.UpdateVendorTax(_objvendor);
                    }
                    ViewState["ReturnTrue"] = VendorID.ToString();
                    //ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Vendor added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false}); window.location.href='AddVendor.aspx?id="+ VendorID.ToString() + "';", true);
                    //Response.Redirect("AddVendor.aspx?id=" + VendorID);
                    //Response.Write("Insert Data....");
                    if (Request.QueryString["t"] != null)
                    {
                        //Response.Redirect("AddVendor.aspx?id=" + VendorID.ToString());
                        ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Vendor added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 1000,theme : 'noty_theme_default',  closable : false}); setTimeout(function(){ window.location.href='AddVendor.aspx?id=" + VendorID.ToString() + "'; }, 1000); ", true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Vendor added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 1000,theme : 'noty_theme_default',  closable : false}); setTimeout(function(){ window.location.href='AddVendor.aspx?id=" + VendorID.ToString() + "'; }, 1000); ", true);
                    }
                }
                else
                {
                    ViewState["ReturnTrue"] = "false";
                    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'Vendor ID already exists.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("Vendors.aspx?AddVendor=Y");
    }
    #endregion

    #region Custom Functions
    public void SetDataForEdit()  //EDIT Vendor
    {
        try
        {
            ViewState["mode"] = 1;
            lblHeader.Text = "Edit Vendor";

            DataSet ds = new DataSet();

            _objvendor.ID = Convert.ToInt32(Request.QueryString["id"]);
            _getVendorEdit.ID = Convert.ToInt32(Request.QueryString["id"]);

            List<VendorViewModel> _VendorViewModel = new List<VendorViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "VendorAPI/AddVendor_GetVendorEdit";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorEdit);

                _VendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<VendorViewModel>(_VendorViewModel);
                ds.Tables[0].Columns["ContactName"].ColumnName = "Contact";
                ds.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                ds.Tables[0].Columns["Vendor1099"].ColumnName = "1099";
            }
            else
            {
                ds = objBL_Vendor.GetVendorEdit(_objvendor);
            }
            DataRow dr = ds.Tables[0].Rows[0];
            txtAccountid.Text = dr["Acct"].ToString();
            txtName.Text = dr["Name"].ToString();
            lblVendorName.Text = dr["Name"].ToString();
            txtAddress.Text = dr["Address"].ToString();
            txtCity.Text = dr["City"].ToString();
            txtState.Text = dr["State"].ToString();
            txtCountry.Text = dr["Country"].ToString();
            txtEmailid.Text = dr["EMail"].ToString();
            txtWebsite.Text = dr["Website"].ToString();
            txtZip.Text = dr["Zip"].ToString();
            txtPhone.Text = dr["Phone"].ToString();
            txtFax.Text = dr["Fax"].ToString();
            txtContact.Text = dr["Contact"].ToString();
            txtCellular.Text = dr["Cellular"].ToString();
            txtGeolock.Text = dr["GeoLock"].ToString();
            txtSince.Text = dr["Since"].ToString();
            txtLast.Text = dr["Last"].ToString();
            txtvenremark.Text = dr["Remarks"].ToString();
            txtCourieracct.Text = dr["CourierAccount"].ToString();
            if (!string.IsNullOrEmpty(dr["VType"].ToString()))
            {
                ddlType.SelectedValue = dr["VType"].ToString();
            }
            FillSalesTax();
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["STax"].ToString()))
                ddlSTax.SelectedValue = ds.Tables[0].Rows[0]["STax"].ToString();
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["UTax"].ToString()))
                ddlUseTax.SelectedValue = ds.Tables[0].Rows[0]["UTax"].ToString();

            ddlStatus.SelectedValue = dr["Status"].ToString();
            txtShipvia.Text = dr["ShipVia"].ToString();
            txtBalance.Text = dr["Balance"].ToString();
            lblVendorBalance.Text = "$" + dr["Balance"].ToString();
            txtCreditlimit.Text = dr["CLimit"].ToString();
            ddlTerms.SelectedValue = dr["Terms"].ToString();
            txtDays.Text = dr["Days"].ToString();
            //txt1099.Text = _dsVender.Tables[0].Rows[0]["1099"].ToString();
            if (!string.IsNullOrEmpty(dr["1099"].ToString()))
            {
                if (Convert.ToInt16(dr["1099"].ToString()).Equals(1))
                {
                    chk1099.Checked = true;
                }
                else
                {
                    chk1099.Checked = false;
                }
            }
            txt1099Box.Text = dr["intBox"].ToString();
            txtInuse.Text = dr["InUse"].ToString();
            txtDefaultAcct.Text = dr["DefaultAcct"].ToString();
            hdnAcctID.Value = dr["DA"].ToString();
            hdnRolID.Value = dr["Rol"].ToString();
            txtRemitTo.Text = dr["Remit"].ToString();
            txtFedID.Text = dr["FID"].ToString();
            txtAcct.Text = dr["Acct#"].ToString();
            ddlCompany.SelectedValue = dr["EN"].ToString();
            txtCompany.Text = dr["Company"].ToString();
            ddlCompanyEdit.SelectedValue = dr["EN"].ToString();
            chkmainEmailPO.Checked = Convert.ToBoolean(dr["EmailREcPO"]);
            ViewState["rolid"] = dr["Rol"].ToString();


            _objvendor.ConnConfig = Session["config"].ToString();
            _objvendor.Rol = Convert.ToInt32(dr["Rol"].ToString());
            rolID= Convert.ToInt32(dr["Rol"].ToString());
            _getVendorContactByRolID.ConnConfig = Session["config"].ToString();
            _getVendorContactByRolID.Rol = Convert.ToInt32(dr["Rol"].ToString());

            if (Request.QueryString["t"] == null)
            {
                DataSet dsVendorContact = new DataSet();

                List<GetVendorContactByRolIDViewModel> _GetVendorContactByRolIDViewModel = new List<GetVendorContactByRolIDViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "VendorAPI/AddVendor_GetVendorContactByRolID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorContactByRolID);

                    _GetVendorContactByRolIDViewModel = (new JavaScriptSerializer()).Deserialize<List<GetVendorContactByRolIDViewModel>>(_APIResponse.ResponseData);
                    dsVendorContact = CommonMethods.ToDataSet<GetVendorContactByRolIDViewModel>(_GetVendorContactByRolIDViewModel);
                }
                else
                {
                    dsVendorContact = objBL_Vendor.getVendorContactByRolID(_objvendor);
                }

                RadGrid_VendorContact.VirtualItemCount = dsVendorContact.Tables[0].Rows.Count;
                RadGrid_VendorContact.DataSource = dsVendorContact.Tables[0];
                RadGrid_VendorContact.DataBind();
                Session["contacttable"] = dsVendorContact.Tables[0];
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void GetVenderData()
    {
        try
        {
            _objvendor.Acct = txtAccountid.Text;
            _addVendor.Acct = txtAccountid.Text;
            _updateVendor.Acct = txtAccountid.Text;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillState()
    {
        //try
        //{
        //    if (ddlState.SelectedIndex != 0)
        //    {
        //        DataSet _dsState = new DataSet();
        //        State _objState = new State();

        //        _objState.ConnConfig = Session["config"].ToString();

        //        List<StateViewModel> _StateViewModel = new List<StateViewModel>();

        //        string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

        //        if (IsAPIIntegrationEnable == "YES")
        //        {
        //            string APINAME = "VendorAPI/AddVendor_GetStates";

        //            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _objState);

        //            _StateViewModel = (new JavaScriptSerializer()).Deserialize<List<StateViewModel>>(_APIResponse.ResponseData);
        //            _dsState = CommonMethods.ToDataSet<StateViewModel>(_StateViewModel);
        //        }
        //        else
        //        {
        //            _dsState = _objBLBank.GetStates(_objState);
        //        }

        //        ddlState.Items.Add(new ListItem("Select State", ""));
        //        ddlState.AppendDataBoundItems = true;
        //        ddlState.DataSource = _dsState;
        //        ddlState.DataValueField = "Name";
        //        ddlState.DataTextField = "fDesc";
        //        ddlState.DataBind();
        //    }
        //    else
        //    {

        //    }
        //}
        //catch (Exception ex)
        //{
        //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        //    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        //}
    }
    private void FillTerms()
    {
        try
        {
            DataSet ds = new DataSet();
            _objPropUser.ConnConfig = Session["config"].ToString();
            _getTerms.ConnConfig = Session["config"].ToString();

            List<TermsViewModel> _TermsViewModel = new List<TermsViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "VendorAPI/AddVendor_GetTerms";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getTerms);

                _TermsViewModel = (new JavaScriptSerializer()).Deserialize<List<TermsViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<TermsViewModel>(_TermsViewModel);
            }
            else
            {
                ds = _objBLUser.getTerms(_objPropUser);
            }
            ddlTerms.DataSource = ds.Tables[0];
            ddlTerms.DataTextField = "name";
            ddlTerms.DataValueField = "id";
            ddlTerms.DataBind();

            ddlTerms.Items.Insert(0, new ListItem(":: Select ::", ""));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
       Session["VendorTransfromDate"] = txtFromDate.Text;
        Session["VendorTransToDate"] = txtToDate.Text;
         
        if (hdnCssActive.Value == "CssActive")
        {
            Session["lblVendorTranActive"] = "1";
        }
        else
        {
            Session["lblVendorTranActive"] = "2";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "CssClearLabel()", true);
        }

        getTransList();
        RadGrid_VendorTran.Rebind();
    }
    protected void gvTrans_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Label lblId = (Label)e.Item.FindControl("lblId");
            Label lblType = (Label)e.Item.FindControl("lblType");
            LinkButton lnkId = (LinkButton)e.Item.FindControl("lnkId");
            if (lblType.Text.Equals("Bill"))
            {
                //lnkId.Click = Response.Redirect();
                lnkId.OnClientClick = "window.open('addbills.aspx?id=" + lblId.Text + "');";
            }
            else
            {
                lnkId.OnClientClick = "window.open('editcheck.aspx?id=" + lblId.Text + "');";
            }
        }
    }

    protected void btnCompanyEdit_Click(object sender, EventArgs e)
    {
        Submit();
        if (Convert.ToInt32(ViewState["CompPermission"]) == 1)
        {
            Response.Redirect(Request.RawUrl);
        }
    }

    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        RadWindowContact.Title = "Add Contact";
        ClearContact();
        string script = "function f(){$find(\"" + RadWindowContact.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        RadWindowContact.Title = "Edit Contact";
        foreach (GridDataItem di in RadGrid_VendorContact.SelectedItems)
        {
            DataTable dt = (DataTable)Session["contacttable"];
            Label lblindex = (Label)di.Cells[1].FindControl("lblindex");

            DataRow dr = dt.Rows[Convert.ToInt32(lblindex.Text)];
            txtContcName.Text = dr["Name"].ToString();
            txtContPhone.Text = dr["Phone"].ToString();
            txtContFax.Text = dr["Fax"].ToString();
            txtContCell.Text = dr["Cell"].ToString();
            txtContEmail.Text = dr["Email"].ToString();
            txtTitle.Text = dr["Title"].ToString();
            chkEmailPo.Checked = Convert.ToBoolean(dr["EmailRecPO"]);
            ViewState["editcon"] = 1;
            ViewState["index"] = lblindex.Text;

        }

        string script = "function f(){$find(\"" + RadWindowContact.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["contacttable"];

        foreach (GridDataItem di in RadGrid_VendorContact.SelectedItems)
        {
            Label lblindex = (Label)di.Cells[1].FindControl("lblindex");

            dt.Rows.RemoveAt(Convert.ToInt32(lblindex.Text));

        }
        dt.AcceptChanges();
        Session["contacttable"] = dt;
        //gvContacts.DataSource = dt;
        //gvContacts.DataBind();

        RadGrid_VendorContact.DataSource = dt;
        RadGrid_VendorContact.Rebind();
        //RowSelectContact();

        if (ViewState["mode"].ToString() == "1")
        {
            SubmitContact();
        }
    }

    private void CreateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ContactID", typeof(int));
        dt.Columns.Add("Name", typeof(string));
        dt.Columns.Add("Phone", typeof(string));
        dt.Columns.Add("Fax", typeof(string));
        dt.Columns.Add("Cell", typeof(string));
        dt.Columns.Add("Email", typeof(string));
        dt.Columns.Add("Title", typeof(string));
        dt.Columns.Add("EmailRecPO", typeof(bool));
        Session["contacttable"] = dt;
    }

    public DataTable SaveEmptyDatatable() 
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ContactID", typeof(int));
        dt.Columns.Add("Name", typeof(string));
        dt.Columns.Add("Phone", typeof(string));
        dt.Columns.Add("Fax", typeof(string));
        dt.Columns.Add("Cell", typeof(string));
        dt.Columns.Add("Email", typeof(string));
        dt.Columns.Add("Title", typeof(string));
        dt.Columns.Add("EmailRecPO", typeof(bool));

        DataRow dr = dt.NewRow();
        dr["ContactID"] = "0";
        dr["Name"] = "";
        dr["Phone"] = "";
        dr["Fax"] = "";
        dr["Cell"] = "";
        dr["Email"] = "";
        dr["Title"] = "";
        dr["EmailRecPO"] = false;
        dt.Rows.Add(dr);

        return dt;
    }

    private void ClearContact()
    {
        txtContcName.Text = string.Empty;
        txtContPhone.Text = string.Empty;
        txtContFax.Text = string.Empty;
        txtContCell.Text = string.Empty;
        txtContEmail.Text = string.Empty;
        txtTitle.Text = string.Empty;
        chkEmailPo.Checked = false;
    }

    public string Truncate(string Value, int length)
    {
        if (Value.Length > length)
        {
            Value = Value.Substring(0, length);
        }
        return Value;
    }

    protected void lnkContactSave_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["contacttable"];

        DataRow dr = dt.NewRow();

        dr["Name"] = Truncate(txtContcName.Text, 50);
        dr["Phone"] = Truncate(txtContPhone.Text, 22);
        dr["Fax"] = Truncate(txtContFax.Text, 22);
        dr["Cell"] = Truncate(txtContCell.Text, 22);
        dr["Email"] = Truncate(txtContEmail.Text, 50);
        dr["Title"] = Truncate(txtTitle.Text, 50);
        dr["EmailRecPO"] = chkEmailPo.Checked;

        if (ViewState["editcon"].ToString() == "1")
        {
            dt.Rows.RemoveAt(Convert.ToInt32(ViewState["index"]));
            dt.Rows.InsertAt(dr, Convert.ToInt32(ViewState["index"]));
            ViewState["editcon"] = 0;
        }
        else
        {
            dt.Rows.Add(dr);
        }

        dt.AcceptChanges();

        Session["contacttable"] = dt;

        //gvContacts.DataSource = dt;
        //gvContacts.DataBind();
        //RadGrid_VendorContact.DataSource = dt;
        //RadGrid_VendorContact.Rebind();

        //RowSelectContact();

        ClearContact();

        if (ViewState["mode"].ToString() == "1")
        {
            SubmitContact();
            fillContract();
        }
    }

    private void SubmitContact()
    {
        try
        {
            if (Session["contacttable"] != null)
            {
                _objvendor.VendorData = (DataTable)Session["contacttable"];
                _updateVendorContact.VendorData = (DataTable)Session["contacttable"];

            }

            if (Convert.ToInt32(ViewState["mode"]) == 1)
            {
                _objvendor.RolId = Convert.ToInt32(ViewState["rolid"].ToString());
                _objvendor.ConnConfig = Session["config"].ToString();

                _updateVendorContact.RolId = Convert.ToInt32(ViewState["rolid"].ToString());
                _updateVendorContact.ConnConfig = Session["config"].ToString();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "VendorAPI/AddVendor_UpdateVendorContact";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateVendorContact);
                }
                else
                {
                    objBL_Vendor.UpdateVendorContact(_objvendor);
                }
                //lblMsg.Text = "Customer updated successfully.";                             
            }
        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message;
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrContct", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    protected void btnCompanyPopUp_Click(object sender, EventArgs e)
    {
        string script = "function f(){$find(\"" + RadWindowCompany.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }


    protected void RadGrid_VendorContact_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_VendorContact.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
    }

    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_VendorContact.MasterTableView.FilterExpression != "" ||
            (RadGrid_VendorContact.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_VendorContact.MasterTableView.SortExpressions.Count > 0;
    }
    
    protected void RadGrid_VendorTran_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_VendorTran.AllowCustomPaging = !ShouldApplySortFilterOrGroup();

        #region Set the Grid Filters
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["VendorTransaction_FilterExpression"] != null && Convert.ToString(Session["VendorTransaction_FilterExpression"]) != "" && Session["VendorTransaction_Filters"] != null)
                {
                    RadGrid_VendorTran.MasterTableView.FilterExpression = Convert.ToString(Session["VendorTransaction_FilterExpression"]);
                    var filtersGet = Session["VendorTransaction_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_VendorTran.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
            }
            else
            {
                Session["VendorTransaction_FilterExpression"] = null;
                Session["VendorTransaction_Filters"] = null;
                //Session["Vendor_VirtulItemCount"] = null;
            }
            
        }

        #endregion

        getTransList();
    }
    bool isGroupingTran = false;
    public bool ShouldApplySortFilterOrGroupTran()
    {
        return RadGrid_VendorTran.MasterTableView.FilterExpression != "" ||
            (RadGrid_VendorTran.MasterTableView.GroupByExpressions.Count > 0 || isGroupingTran) ||
            RadGrid_VendorTran.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_VendorTran_PreRender(object sender, EventArgs e)
    {

        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_VendorTran.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["VendorTransaction_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_VendorTran.MasterTableView.OwnerGrid.Columns)
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

            Session["VendorTransaction_Filters"] = filters;
            //Session["Vendor_VirtulItemCount"] = RadGrid_Vendor.VirtualItemCount;
        }
        else
        {
            Session["VendorTransaction_FilterExpression"] = null;
            Session["VendorTransaction_Filters"] = null;
            //Session["Vendor_VirtulItemCount"] = null;
        }
        #endregion  
        


        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_VendorTran);
        foreach (GridDataItem gr in RadGrid_VendorTran.Items)
        {

            Label lblType = (Label)gr.FindControl("lblType");
            LinkButton lnkId = (LinkButton)gr.FindControl("lblId");
            HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");
            if (lblType.Text.Equals("Bill"))
            {
                //lnkId.OnClientClick = "window.open('addbills.aspx?id=" + lnkId.Text + "');";
                lnkId.OnClientClick = "window.open('addbills.aspx?id=" + hdnID.Value + "');";
            }
            else
            {
                //lnkId.OnClientClick = "window.open('editcheck.aspx?id=" + lnkId.Text + "');";
                lnkId.OnClientClick = "window.open('editcheck.aspx?id=" + hdnID.Value + "');";
            }
        }
    }

    private void getTransList()
    {
        #region Condition SearchValue & SearchBy
        String SearchValue = "";
        if (rdAll.Checked == true)
        {
            SearchValue = "All";
        }
        else if (rdOpen.Checked == true)
        {
            SearchValue = "Open";
        }
        else if (rdClosed.Checked == true)
        {
            SearchValue = "Closed";
        }
        else
        {
            SearchValue = "All";
        }

        String SearchBy = "";
        if (rdAll2.Checked == true)
        {
            SearchBy = "All";
        }
        else if (rdCharges.Checked == true)
        {
            SearchBy = "Charges";
        }
        else if (rdCredit.Checked == true)
        {
            SearchBy = "Credits";
        }
        else
        {
            SearchBy = "All";
        }
        #endregion
        
        string stdate = txtFromDate.Text + " 00:00:00";
        string enddate = txtToDate.Text + " 23:59:59";
        _objvendor.ConnConfig = Session["config"].ToString();
        _objvendor.ID = Convert.ToInt32(Request.QueryString["id"]);
        _objvendor.SearchBy = SearchBy;
        _objvendor.SearchValue = SearchValue;

        if (txtFromDate.Text == "" && txtToDate.Text == "")
        {
            _objvendor.StartDate = Convert.ToDateTime("01/01/1900 00:00:00");
            _objvendor.EndDate = Convert.ToDateTime("01/01/3000 00:00:00");
        }
        else
        {
            _objvendor.StartDate = Convert.ToDateTime(stdate);
            _objvendor.EndDate = Convert.ToDateTime(enddate);
        }

        _getAPExpenses.ConnConfig = Session["config"].ToString();
        _getAPExpenses.ID = Convert.ToInt32(Request.QueryString["id"]);
        _getAPExpenses.SearchBy = SearchBy;
        _getAPExpenses.SearchValue = SearchValue;
        
        if (txtFromDate.Text == "" && txtToDate.Text == "")
        {
            _getAPExpenses.StartDate = Convert.ToDateTime("01/01/1900 00:00:00");
            _getAPExpenses.EndDate = Convert.ToDateTime("01/01/3000 00:00:00");
        }
        else
        {
            _getAPExpenses.StartDate = Convert.ToDateTime(stdate);
            _getAPExpenses.EndDate = Convert.ToDateTime(enddate);
        }

        string filterexpression = string.Empty;
        filterexpression = RadGrid_VendorTran.MasterTableView.FilterExpression;
        

        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();

        ListGetAPExpenses _lstGetAPExpenses = new ListGetAPExpenses();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "VendorAPI/AddVendor_GetAPExpenses";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getAPExpenses);

            _lstGetAPExpenses = (new JavaScriptSerializer()).Deserialize<ListGetAPExpenses>(_APIResponse.ResponseData);

            ds1 = _lstGetAPExpenses.lstTable1.ToDataSet();
            ds2 = _lstGetAPExpenses.lstTable2.ToDataSet();

            var prevRuntotal = Convert.ToDouble(ds2.Tables[0].Rows[0][0].ToString());

            RadGrid_VendorTran.VirtualItemCount = ds1.Tables[0].Rows.Count;
            RadGrid_VendorTran.DataSource = BindTransGridDatatable(ds1.Tables[0], prevRuntotal);
            lblRecordCount.Text = ds1.Tables[0].Rows.Count.ToString() + " Record(s) found";
        }
        else
        {
            ds = objBLBill.GetAPExpenses(_objvendor);
            var prevRuntotal = Convert.ToDouble(ds.Tables[1].Rows[0][0].ToString());
            RadGrid_VendorTran.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGrid_VendorTran.DataSource = BindTransGridDatatable(ds.Tables[0], prevRuntotal);
            lblRecordCount.Text = ds.Tables[0].Rows.Count.ToString() + " Record(s) found";


            DataTable dtID;
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataView view = new DataView(GetFilteredDataSource());                
                dtID = view.ToTable("vendorsTransaction", true, "ID");

                if (filterexpression != "")
                {
                    RadGrid_VendorTran.AllowCustomPaging = true;
                    RadGrid_VendorTran.VirtualItemCount = dtID.Rows.Count;
                    ViewState["VirtualItemCount"] = dtID.Rows.Count;
                    lblRecordCount.Text = dtID.Rows.Count + " Record(s) found";
                    //updpnl.Update();
                }


            }
            else
            {
                dtID = ds.Tables[0];
            }
            

            

        }

        //ds = objBLBill.GetAPExpenses(_objvendor);
        //var prevRuntotal = Convert.ToDouble(ds.Tables[1].Rows[0][0].ToString());
        //RadGrid_VendorTran.VirtualItemCount = ds.Tables[0].Rows.Count;
        //RadGrid_VendorTran.DataSource = BindTransGridDatatable(ds.Tables[0], prevRuntotal);
        //lblRecordCount.Text = ds.Tables[0].Rows.Count.ToString() + " Record(s) found";
    }
    private DataTable GetFilteredDataSource()
    {
        DataTable DT = new DataTable();
        DataTable FilteredDT = new DataTable();
        string filterexpression = string.Empty;
        filterexpression = RadGrid_VendorTran.MasterTableView.FilterExpression;
        if (filterexpression != "")
        {
            DT = (DataTable)RadGrid_VendorTran.DataSource;
            if (DT.Rows.Count > 0)
            {
                FilteredDT = DT.AsEnumerable().AsQueryable()
                .Where(filterexpression)
                .CopyToDataTable();
                return FilteredDT;
            }
            else
            {
                return (DataTable)RadGrid_VendorTran.DataSource;
            }
        }
        else
        {
            return (DataTable)RadGrid_VendorTran.DataSource;
        }

    }
    private DataTable BindTransGridDatatable(DataTable dt, double prevRuntotal)
    {
        foreach (DataRow row in dt.Rows)
        {
            row["RunTotal"] = Convert.ToDouble(row["RunTotal"].ToString()) + prevRuntotal;
        }

        return dt;
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["vendors"];
            string url = "addvendor.aspx?id=" + dt.Rows[0]["ID"];
            Response.Redirect(url);
        }
        catch (Exception ex)
        {

        }
    }

    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["vendors"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;
            if (index < c)
            {
                string url = "addvendor.aspx?id=" + dt.Rows[index - 1]["ID"];
                Response.Redirect(url);
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["vendors"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;
            if (index < c)
            {
                string url = "addvendor.aspx?id=" + dt.Rows[index + 1]["ID"];
                Response.Redirect(url);
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void lnkLast_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["vendors"];
            string url = "addvendor.aspx?id=" + dt.Rows[dt.Rows.Count - 1]["ID"];
            Response.Redirect(url);
        }
        catch (Exception ex)
        {

        }
    }
    public void cleanFilter()
    {
        foreach (GridColumn column in RadGrid_VendorTran.MasterTableView.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        Session["VendorTransaction_FilterExpression"] = null;
        Session["VendorTransaction_Filters"] = null;
        RadGrid_VendorTran.MasterTableView.FilterExpression = string.Empty;
        RadGrid_VendorTran.MasterTableView.Rebind();
        RadGrid_VendorTran.Rebind();
        
    }
    protected void lnkShowAllOpen_Click(object sender, EventArgs e)
    {
        ViewState["ShowAll"] = "1";
        txtFromDate.Text = string.Empty;
        //txtFromDate.Style.Add("display", "none");
        txtToDate.Text = string.Empty;
        rdAll.Checked = false;
        rdOpen.Checked = true;
        rdClosed.Checked = false;
        rdAll2.Checked = false;
        rdCharges.Checked = false;
        rdCredit.Checked = false;
        hdnSearchValue.Value = "rdOpen";
        
        updtall.Update();
        cleanFilter();
        getTransList();
        RadGrid_VendorTran.Rebind();
       
    }
    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        ViewState["ShowAll"] = "1";
        
        //txtFromDate.Text = string.Empty;
        //txtFromDate.Style.Add("display", "none");
        //txtToDate.Text = string.Empty;
        rdAll.Checked = true;
        rdOpen.Checked = false;
        rdClosed.Checked = false;
        rdAll2.Checked = false;
        rdCharges.Checked = false;
        rdCredit.Checked = false;

        //DateTime sdtDate = new DateTime(1900, 1, 1);
        //DateTime edtDate = new DateTime(3000, 1, 1);
        //txtFromDate.Text = sdtDate.ToShortDateString();
        //txtToDate.Text = edtDate.ToShortDateString();

        txtFromDate.Text = string.Empty;        
        txtToDate.Text = string.Empty;

        updtall.Update();

        cleanFilter();
        getTransList();
        RadGrid_VendorTran.Rebind();
    }
    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
       Session["VendorTransfromDate"] = txtFromDate.Text;
       
    }

    protected void txtToDate_TextChanged(object sender, EventArgs e)
    {
        Session["VendorTransToDate"] = txtToDate.Text;
        
    }
    protected void lnkClear_Click(object sender, EventArgs e)
    {
        rdAll.Checked = false;
        rdOpen.Checked = false;
        rdClosed.Checked = false;
        rdAll2.Checked = false;
        rdCharges.Checked = false;
        rdCredit.Checked = false;
        if (Convert.ToString(ViewState["ShowAll"]) == "1")
        {
            if (Session["VendorTransToDate"] == null || Convert.ToString(Session["VendorTransToDate"]).Trim() == "")
            {
                //txtToDate.Text = DateTime.Now.AddMonths(1).Date.ToShortDateString();
                txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
            }
            else
            {
                txtToDate.Text = Session["VendorTransToDate"].ToString();
            }
            if (Session["VendorTransfromDate"] == null || Convert.ToString(Session["VendorTransfromDate"]).Trim() == "")
            {
                //txtInvDtFrom.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                //txtFromDate.Text = DateTime.Now.AddMonths(-1).Date.ToShortDateString();
                txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                
            }
            else
            {
                txtFromDate.Text =Session["VendorTransfromDate"].ToString();
            }            
            ViewState["ShowAll"] = "0";
        }
        else
        {
            if (txtToDate.Text == "" && txtFromDate.Text == "")
            {
                if (Session["VendorTransToDate"] == null  || Convert.ToString(Session["VendorTransToDate"]).Trim() == "")
                {
                    //txtToDate.Text = DateTime.Now.AddMonths(1).Date.ToShortDateString();                    
                    txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                }
                else
                {
                    txtToDate.Text = Session["VendorTransToDate"].ToString();
                }
                if (Session["VendorTransfromDate"] == null || Convert.ToString(Session["VendorTransfromDate"]).Trim() == "")
                {
                    //txtInvDtFrom.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                    //txtFromDate.Text = DateTime.Now.AddMonths(-1).Date.ToShortDateString();
                    txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                    
                }
                else
                {
                    txtFromDate.Text =Session["VendorTransfromDate"].ToString();
                }
            }
        }
        updtall.Update();



        getTransList();
        cleanFilter();
        
    }
    protected void RadGrid_VendorTran_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            foreach (GridColumn col in RadGrid_VendorTran.MasterTableView.RenderColumns)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                if (col.UniqueName == "Balance")
                {
                    HiddenField hdnID = (HiddenField)dataItem.FindControl("hdnID");
                    Label lblType = (Label)dataItem.FindControl("lblType");
                    Label lblStatusName = (Label)dataItem.FindControl("lblStatusName");
                    HiddenField hdnTRID = (HiddenField)dataItem.FindControl("hdnTRID");
                    //HiddenField hdnLinkTo = (HiddenField)dataItem.FindControl("hdnLinkTo");
                    //HiddenField hdnTransID = (HiddenField)dataItem.FindControl("hdnTransID");
                    //dataItem[col.UniqueName].Attributes.Add("onclick", "ShowHistoryTransactionPopup(" + hdnID.Value + "," + Convert.ToInt32(hdnLinkTo.Value) + "," + Convert.ToInt32(Request.QueryString["id"].ToString()) + ",0,'" + lblStatusName.Text + "'," + hdnTransID.Value + ")");
                    dataItem[col.UniqueName].Attributes.Add("onclick", "ShowHistoryTransactionPopup(" + hdnID.Value + ",'" + lblType.Text + "'," + Convert.ToInt32(Request.QueryString["id"].ToString()) + ",0,'" + lblStatusName.Text + "'," + hdnTRID.Value + ")");
                }

            }
        }
    }
    protected void RadGrid_VendorTran_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 2;

        if (e.Worksheet.Table.Rows.Count == RadGrid_VendorTran.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_VendorTran.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;

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
    protected void RadGrid_VendorTran_ItemCreated(object sender, GridItemEventArgs e)
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
    protected void lnkTransactionExcel_Click(object sender, EventArgs e)
    {
        //  RadGrid_VendorTran.MasterTableView.GetColumn("CustomerName").Visible = true;
        RadGrid_VendorTran.ExportSettings.FileName = "Transactions";
        RadGrid_VendorTran.ExportSettings.IgnorePaging = true;
        RadGrid_VendorTran.ExportSettings.ExportOnlyData = true;
        RadGrid_VendorTran.ExportSettings.OpenInNewWindow = true;
        RadGrid_VendorTran.ExportSettings.HideStructureColumns = true;
        RadGrid_VendorTran.MasterTableView.UseAllDataFields = true;
        RadGrid_VendorTran.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_VendorTran.MasterTableView.ExportToExcel();

    }
    private void FillVendorType()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        _getVendorType.ConnConfig = Session["config"].ToString();

        List<GetVendorTypeViewModel> _GetVendorTypeViewModel = new List<GetVendorTypeViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {

            string APINAME = "VendorAPI/VendorList_getVendorType";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorType);

            _GetVendorTypeViewModel = (new JavaScriptSerializer()).Deserialize<List<GetVendorTypeViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetVendorTypeViewModel>(_GetVendorTypeViewModel);
        }
        else
        {
            ds = objBL_User.getVendorType(objPropUser);
        }
        ddlType.DataSource = ds.Tables[0];
        ddlType.DataTextField = "Type";
        ddlType.DataValueField = "Type";
        ddlType.DataBind();
        ddlType.Items.Insert(0, new ListItem("Select", "0"));


    }
    #region logs
    protected void RadGrid_gvLogs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();
        if (Request.QueryString["id"] != null)
        {
            DataSet dsLog = new DataSet();
            _objvendor.ConnConfig = Session["config"].ToString();
            _objvendor.ID = Convert.ToInt32(Request.QueryString["id"]);

            _getVendorLogs.ConnConfig = Session["config"].ToString();
            _getVendorLogs.ID = Convert.ToInt32(Request.QueryString["id"]);

            List<LogViewModel> _LogViewModel = new List<LogViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "VendorAPI/AddVendor_GetVendorLogs";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorLogs);

                _LogViewModel = (new JavaScriptSerializer()).Deserialize<List<LogViewModel>>(_APIResponse.ResponseData);
                dsLog = CommonMethods.ToDataSet<LogViewModel>(_LogViewModel);
            }
            else
            {
                dsLog = objBL_Vendor.GetVendorLogs(_objvendor);
            }
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
    }
    bool isGroupLog = false;
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

    public void fillContract()
    {
        _objvendor.Rol = Convert.ToInt32(ViewState["rolid"].ToString());
        _objvendor.ConnConfig = Session["config"].ToString();

        _getVendorContactByRolID.Rol = Convert.ToInt32(ViewState["rolid"].ToString());
        _getVendorContactByRolID.ConnConfig = Session["config"].ToString();

        DataSet dsVendorContact = new DataSet();

        List<GetVendorContactByRolIDViewModel> _GetVendorContactByRolIDViewModel = new List<GetVendorContactByRolIDViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "VendorAPI/AddVendor_GetVendorContactByRolID";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorContactByRolID);

            _GetVendorContactByRolIDViewModel = (new JavaScriptSerializer()).Deserialize<List<GetVendorContactByRolIDViewModel>>(_APIResponse.ResponseData);
            dsVendorContact = CommonMethods.ToDataSet<GetVendorContactByRolIDViewModel>(_GetVendorContactByRolIDViewModel);
        }
        else
        {
            dsVendorContact = objBL_Vendor.getVendorContactByRolID(_objvendor);
        }    
       
        RadGrid_VendorContact.DataSource = dsVendorContact.Tables[0];
        RadGrid_VendorContact.Rebind();
        Session["contacttable"] = dsVendorContact.Tables[0];
    }
    #endregion
}