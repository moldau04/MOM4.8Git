using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Data.Odbc;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using BusinessLayer.Schedule;
using System.Collections.Generic;
using BusinessEntity.payroll;
using BusinessEntity.Utility;
using MOMWebApp;
using System.Web.Script.Serialization;
using BusinessEntity.CustomersModel;
using BusinessEntity.Payroll;
using BusinessEntity.APModels;
using BusinessEntity.InventoryModel;
using System.Linq;
using Newtonsoft.Json;

public partial class addcustomer : System.Web.UI.Page
{
    #region property

    Loc _objLoc = new Loc();

    Customer objProp_Customer = new Customer();

    BL_Customer objBL_Customer = new BL_Customer();

    User objPropUser = new User();

    BL_User objBL_User = new BL_User();

    BL_MapData objBL_MapData = new BL_MapData();

    MapData objMapData = new MapData();

    BL_Contracts objBL_Contracts = new BL_Contracts();

    Contracts objProp_Contracts = new Contracts();

    General objGeneral = new General();

    BL_General objBL_General = new BL_General();

    BL_Invoice objBL_Invoice = new BL_Invoice();

    BusinessEntity.CompanyOffice objCompany = new BusinessEntity.CompanyOffice();

    BL_Company objBL_Company = new BL_Company();

    private static readonly string CookieName = "CkEditCust";

    private double PrevRuntotal = 0.00;

    //API Variables
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    getConnectionConfigParam _getConnectionConfig = new getConnectionConfigParam();
    GetCustomerByIDParam _GetCustomerByID = new GetCustomerByIDParam();
    GetAllLocationOnCustomerParam _GetAllLocationOnCustomer = new GetAllLocationOnCustomerParam();
    getCustomFieldsParam _getCustomFields = new getCustomFieldsParam();
    GetCompanyByCustomerParam _GetCompanyByCustomer = new GetCompanyByCustomerParam();
    getCustomerTypeParam _getCustomerType = new getCustomerTypeParam();
    UpdateCustomerParam _UpdateCustomer = new UpdateCustomerParam();
    AddCustomerParam _AddCustomer = new AddCustomerParam();
    UpdateCustomerContactRecordLogParam _UpdateCustomerContactRecordLog = new UpdateCustomerContactRecordLogParam();
    DeleteLocationParam _DeleteLocation = new DeleteLocationParam();
    GetElevParam _GetElev = new GetElevParam();
    DeleteEquipmentParam _DeleteEquipment = new DeleteEquipmentParam();
    GetCategoryParam _GetCategory = new GetCategoryParam();
    GetProspectByIDParam _GetProspectByID = new GetProspectByIDParam();
    GetDepartmentParam _GetDepartment = new GetDepartmentParam();
    GetLocationByCustomerIDParam _GetLocationByCustomerID = new GetLocationByCustomerIDParam();
    AddFileParam _AddFile = new AddFileParam();
    UpdateDocInfoParam _UpdateDocInfo = new UpdateDocInfoParam();
    GetDocumentsParam _GetDocuments = new GetDocumentsParam();
    DeleteFileParam _DeleteFile = new DeleteFileParam();
    GetCustomersLogsParam _GetCustomersLogs = new GetCustomersLogsParam();
    GetContactLogByCustomerIDParam _GetContactLogByCustomerID = new GetContactLogByCustomerIDParam();
    GetContactByRolIDParam _GetContactByRolID = new GetContactByRolIDParam();
    DeleteOpportunityParam _DeleteOpportunity = new DeleteOpportunityParam();
    GetOpportunityOfCustomerParam _GetOpportunityOfCustomer = new GetOpportunityOfCustomerParam();
    GetJobProjectParam _GetJobProject = new GetJobProjectParam();
    GetARRevenueCustShowAllParam _GetARRevenueCustShowAll = new GetARRevenueCustShowAllParam();
    GetARRevenueCustParam _GetARRevenueCust = new GetARRevenueCustParam();
    GetCallHistoryParam _GetCallHistory = new GetCallHistoryParam();
    #endregion

    #region Page events

    protected void Page_PreInit(object sender, System.EventArgs e)

    {

        if (Request.QueryString["o"] != null)
        {
            Control header = Page.Master.FindControl("divHeader");
            header.Visible = false;
            Control menu = Page.Master.FindControl("menu");
            menu.Visible = false;
            this.Title = "Add Customer";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");

        }
        PagePermission();
        divNavigate.Style["display"] = "None";



        if (!CheckAddEditPermission()) { Response.Redirect("Home.aspx?permission=no"); return; }

        if (Request.QueryString["uid"] != null)
        {
            divNavigate.Style["display"] = "block";
            if (Request.QueryString["t"] != null && Request.QueryString["t"] == "c")
            {
                licontact.Style["display"] = "none";
                dvContact.Style["display"] = "none";
            }
        }
        else
        {
            lnkAddnew.Visible = false;
            licontact.Style["display"] = "none";
            dvContact.Style["display"] = "none";
        }

        if (!IsPostBack)
        {
            HighlightSideMenu("cstmMgr", "lnkCustomersSmenu", "cstmMgrSub");
            //assign postback to data range radio buttons
            //List<Tuple<RadioButton, HtmlGenericControl>> listRadio = new List<Tuple<RadioButton, HtmlGenericControl>>();
            //listRadio.Add(new Tuple<RadioButton, HtmlGenericControl>(rdDay, lblDay));
            //listRadio.Add(new Tuple<RadioButton, HtmlGenericControl>(rdWeek, lblWeek));
            //listRadio.Add(new Tuple<RadioButton, HtmlGenericControl>(rdMonth, lblMonth));
            //listRadio.Add(new Tuple<RadioButton, HtmlGenericControl>(rdQuarter, lblQuarter));
            //listRadio.Add(new Tuple<RadioButton, HtmlGenericControl>(rdYear, lblYear));

            //foreach (Tuple<RadioButton, HtmlGenericControl> objPair in listRadio)
            //{

            //    objPair.Item2.Attributes["Class"] = (objPair.Item1.Checked ? "labelactive" : "");
            //    objPair.Item2.Attributes["onclick"] = "javascript:setTimeout('__doPostBack(\\'" + objPair.Item1.ClientID + "\\',\\'\\')', 0); ";
            //}
            ////set default class
            //lblWeek.Attributes.Add("class", "labelactive");

            //set transaction tab default date range 

            if (Session["InvToDate"] == null)
            {
                txtInvDtTo.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
            }
            else
            {
                txtInvDtTo.Text = Session["InvToDate"].ToString();
                if (txtInvDtTo.Text.Trim() == "")
                {
                    txtInvDtTo.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                }
            }
            if (Session["InvFromDate"] == null)
            {
                txtInvDtFrom.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();

            }
            else
            {
                txtInvDtFrom.Text = Session["InvFromDate"].ToString();
                if (txtInvDtFrom.Text.Trim() == "")
                {
                    txtInvDtFrom.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                }
            }
            DataSet dscstm = new DataSet();

            dscstm = GetCustomFields("Owner1");
            if (dscstm.Tables[0].Rows.Count > 0)
            {
                lblCustom1.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
            }
            dscstm = GetCustomFields("Owner2");
            if (dscstm.Tables[0].Rows.Count > 0)
            {
                lblCustom2.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
            }
            //set service histort tab default date range 
            DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            int DaysinMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - 1;
            DateTime lastDay = firstDay.AddDays(DaysinMonth);

            txtfromDate.Text = firstDay.ToShortDateString();
            txtToDate.Text = lastDay.ToShortDateString();
            //Set Select as default Country
            ddlCountry.SelectedValue = "Select";

            GetQBInt();
            FillSpecifyLocation();
            CompanyPermission();
            //GetDocuments();
           // RadGrid_Documents.Rebind();

            DataSet dsLastSync = new DataSet();
            objGeneral.ConnConfig = Session["config"].ToString();
            _getConnectionConfig.ConnConfig = Session["config"].ToString();
            List<GeneralViewModel> _lstGeneral = new List<GeneralViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "CustomersAPI/CustomersList_GetSagelatsync";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGeneral = serializer.Deserialize<List<GeneralViewModel>>(_APIResponse.ResponseData);
                dsLastSync = CommonMethods.ToDataSet<GeneralViewModel>(_lstGeneral);
            }
            else
            {
                dsLastSync = objBL_General.getSagelatsync(objGeneral);
            }

            int intintegration = Convert.ToInt32(dsLastSync.Tables[0].Rows[0]["sageintegration"]);
            hdnSageIntegration.Value = intintegration.ToString();
            if (intintegration == 1)
            {
                txtCName.MaxLength = 50;
                btnSageID.Visible = true;
                txtAcctno.Visible = true;
                lblSageID.Visible = true;

            }
            else
            {
                btnSageID.Visible = false;
                txtAcctno.Visible = false;
                lblSageID.Visible = false;

            }



            FillCustomerType();
            ViewState["mode"] = 0;
            ViewState["editcon"] = 0;
            ViewState["contacttable"] = null;
            if (Request.QueryString["o"] == null)
            {
                Session["locationdataCust"] = null;
            }
            CreateTable();
            divEquipment.Style["display"] = "none";
            liequipments.Style["display"] = "none";
            divTrans.Style["display"] = "none";
            litrans.Style["display"] = "none";
            dvLocations.Style["display"] = "none";
            lilocations.Style["display"] = "none";
            dvTickets.Style["display"] = "none";
            litickets.Style["display"] = "none";
            dvDocuments.Style["display"] = "none";
            lidocuments.Style["display"] = "none";
            liOpportunities.Style["display"] = "none";
            adOpportunities.Style["display"] = "none";
            liProjects.Style["display"] = "none";
            adProjects.Style["display"] = "none";
            #region Getdata
            if (Request.QueryString["uid"] == null)
            {
                custReport.Visible = false;
                lnkReport.Style["display"] = "none";
                ddlCompany.Visible = true;
                txtCompany.Visible = false;
                btnCompanyPopUp.Visible = false;
                Page.Title = "Add Customer || MOM";
                FillProspect();
            }
            if (Request.QueryString["uid"] != null)
            {
                FillCategory();
                // GetOpenCalls();
                GetDataEquip();
                Page.Title = "Edit Customer || MOM";
                GetInvoices("All", "All");
                FillDepartment();
                if (Request.QueryString["t"] == null && Request.QueryString["t"] != "c")
                {
                    divEquipment.Style["display"] = "block";
                    liequipments.Style["display"] = "inline-block";
                    divTrans.Style["display"] = "block";
                    litrans.Style["display"] = "inline-block";
                    dvLocations.Style["display"] = "block";
                    lilocations.Style["display"] = "inline-block";
                    dvTickets.Style["display"] = "block";
                    litickets.Style["display"] = "inline-block";
                    dvDocuments.Style["display"] = "block";
                    lidocuments.Style["display"] = "inline-block";
                    liLogs.Style["display"] = "inline-block";
                    tbLogs.Style["display"] = "block";
                    liOpportunities.Style["display"] = "inline-block";
                    adOpportunities.Style["display"] = "block";
                    liProjects.Style["display"] = "inline-block";
                    adProjects.Style["display"] = "block";
                }

                ddlCompany.Visible = false;
                txtCompany.Visible = true;
                btnCompanyPopUp.Visible = true;
                if (Request.QueryString["t"] != null)
                {
                    ViewState["mode"] = 0;

                }
                else
                {
                    ViewState["mode"] = 1;
                    lblHeader.Text = "Edit Customer";

                }


                objPropUser.CustomerID = Convert.ToInt32(Request.QueryString["uid"]);
                objPropUser.DBName = Session["dbname"].ToString();
                objPropUser.ConnConfig = Session["config"].ToString();

                //API
                _GetCustomerByID.CustomerID = Convert.ToInt32(Request.QueryString["uid"]);
                _GetCustomerByID.DBName = Session["dbname"].ToString();
                _GetCustomerByID.ConnConfig = Session["config"].ToString();

                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet ds3 = new DataSet();

                ListGetCustomerByID _listGetCustomerByID = new ListGetCustomerByID();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "CustomersAPI/AddCustomer_GetCustomerByID";

                    _GetCustomerByID.IsSalesAsigned = new GeneralFunctions().GetSalesAsigned();

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomerByID);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _listGetCustomerByID = serializer.Deserialize<ListGetCustomerByID>(_APIResponse.ResponseData);
                    ds1 = _listGetCustomerByID.lstTable1.ToDataSet();
                    ds2 = _listGetCustomerByID.lstTable2.ToDataSet();
                    ds3 = _listGetCustomerByID.lstTable3.ToDataSet();

                    DataTable dt1 = new DataTable();
                    DataTable dt2 = new DataTable();
                    DataTable dt3 = new DataTable();

                    dt1 = ds1.Tables[0];
                    dt2 = ds2.Tables[0];
                    dt3 = ds3.Tables[0];

                    dt1.TableName = "Table1";
                    dt2.TableName = "Table2";
                    dt3.TableName = "Table3";
                    ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy() });
                }
                else
                {
                    ds = objBL_User.getCustomerByID(objPropUser, new GeneralFunctions().GetSalesAsigned());
                }

                if (ds.Tables[0].Rows.Count > 0)
                {


                    if (ViewState["qbint"].ToString() == "1")
                    {

                        Label5.Text = Request.QueryString["uid"];
                        Label5.Visible = true;
                        Label4.Visible = true;
                    }
                    else
                    {
                        Label5.Text = string.Empty;
                        Label4.Visible = false;
                        Label5.Visible = false;
                    }

                    txtBillRate.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["BillRate"].ToString()));
                    txtOt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateOT"].ToString()));
                    txtNt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateNT"].ToString()));
                    txtDt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateDT"].ToString()));
                    txtTravel.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateTravel"].ToString()));
                    txtMileage.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateMileage"].ToString()));

                    txtAcctno.Text = ds.Tables[0].Rows[0]["ownerid"].ToString();
                    hdnAcctID.Value = ds.Tables[0].Rows[0]["ownerid"].ToString();
                    txtGoogleAutoc.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                    txtCity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                    txtCName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    lblCustomerName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    txtPassword.Text = ds.Tables[0].Rows[0]["Password"].ToString();
                    // ddlState.SelectedValue = ds.Tables[0].Rows[0]["State"].ToString();
                    txtState.Text = ds.Tables[0].Rows[0]["State"].ToString();
                    ddlCountry.SelectedValue = ds.Tables[0].Rows[0]["Country"].ToString();
                    txtUserName.Text = ds.Tables[0].Rows[0]["flogin"].ToString();
                    txtZip.Text = ds.Tables[0].Rows[0]["Zip"].ToString();
                    txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                    txtMaincontact.Text = ds.Tables[0].Rows[0]["contact"].ToString();
                    txtPhoneCust.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                    txtWebsite.Text = ds.Tables[0].Rows[0]["website"].ToString();
                    txtEmail.Text = ds.Tables[0].Rows[0]["email"].ToString();
                    txtCell.Text = ds.Tables[0].Rows[0]["cellular"].ToString();
                    txtFax.Text = ds.Tables[0].Rows[0]["fax"].ToString();
                    ddlUserType.SelectedValue = ds.Tables[0].Rows[0]["type"].ToString();
                    ViewState["rolid"] = ds.Tables[0].Rows[0]["rol"].ToString();
                    ddlCustStatus.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
                    ddlCompany.SelectedValue = ds.Tables[0].Rows[0]["EN"].ToString();
                    txtCompany.Text = ds.Tables[0].Rows[0]["Company"].ToString();
                    ddlCompanyEdit.SelectedValue = ds.Tables[0].Rows[0]["EN"].ToString();
                    lat.Value = ds.Tables[0].Rows[0]["Lat"].ToString();
                    lng.Value = ds.Tables[0].Rows[0]["Lng"].ToString();
                    txtCst1.Text = ds.Tables[0].Rows[0]["Custom1"].ToString();
                    txtCst2.Text = ds.Tables[0].Rows[0]["Custom2"].ToString();
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Billing"].ToString()))
                    {
                        ddlBilling.SelectedValue = ds.Tables[0].Rows[0]["Billing"].ToString();
                    }
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Central"].ToString()))
                    {
                        ddlSpecifiedLocation.SelectedValue = ds.Tables[0].Rows[0]["Central"].ToString();
                    }

                    chkEquipments.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["CPEquipment"]);
                    chkGrpWO.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["groupbyWO"]);
                    chkOpenTicket.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["openticket"]);

                    if (ds.Tables[0].Rows[0]["internet"].ToString() == "1")
                    {
                        chkInternet.Checked = true;

                    }
                    if (ds.Tables[0].Rows[0]["ledger"].ToString() == "1")
                    {
                        chkScheduleBrd.Checked = true;
                    }
                    if (ds.Tables[0].Rows[0]["ticketd"].ToString() == "1")
                    {
                        chkMap.Checked = true;
                    }
                    chkShutdownAlert.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["ShutdownAlert"]);
                    txtAlert.Text = ds.Tables[0].Rows[0]["ShutdownMessage"].ToString();

                    lblCustomerBalance.Text = String.Format("{0:C}", Convert.ToDecimal(Convert.ToString(ds.Tables[0].Rows[0]["Balance"]) == "" ? "0" : Convert.ToString(ds.Tables[0].Rows[0]["Balance"]))); //"$" + Convert.ToString(ds.Tables[0].Rows[0]["Balance"]);
                    ViewState["CusBalance"] = Convert.ToString(ds.Tables[0].Rows[0]["Balance"]);
                    // lblTotalRunBalance.Text = String.Format("{0:C}", ds.Tables[0].Compute("Sum(Amount)", string.Empty));
                    lnkAddProject.NavigateUrl = "AddProject.aspx?cust=" + Request.QueryString["uid"].ToString()
                        + "&custName=" + txtCName.Text
                        + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl);

                    lnkAddopp.NavigateUrl = "AddOpprt.aspx?"
                        + "custId=" + Convert.ToString(Request.QueryString["uid"])
                        + "&custName=" + txtCName.Text
                        + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl);
                    GetDocuments();
                }

                try
                {
                    if (ds.Tables.Count > 1)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {

                            RadGrid_gvContacts.VirtualItemCount = ds.Tables[1].Rows.Count;
                            RadGrid_gvContacts.DataSource = ds.Tables[1];

                            ViewState["contacttable"] = ds.Tables[1];
                        }
                        else
                        {
                            DataTable dt = new DataTable();
                            RadGrid_gvContacts.DataSource = dt;

                        }
                    }

                    if (ds.Tables.Count > 2)
                    {
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            RadGrid_Location.VirtualItemCount = ds.Tables[2].Rows.Count;
                            RadGrid_Location.DataSource = ds.Tables[2];

                            Session["locationdataCust"] = ds.Tables[2];

                            ddllocation.DataSource = ds.Tables[2];
                            ddllocation.DataTextField = "tag";
                            ddllocation.DataValueField = "loc";
                            ddllocation.DataBind();
                            ddllocation.Items.Insert(0, new ListItem("Select", "0"));
                        }
                        else
                        {
                            DataTable dt = new DataTable();
                            RadGrid_Location.DataSource = dt;
                            ddllocation.Items.Insert(0, new ListItem("Select", "0"));
                        }
                    }
                }
                catch { }
                if (Request.QueryString["tab"] != null)
                {
                    if (Request.QueryString["tab"] == "loc")
                    {
                        //TabContainer1.ActiveTab = tpViewlocations; ac
                    }
                    else if (Request.QueryString["tab"] == "equip")
                    {
                        //TabContainer1.ActiveTab = tpViewEquipment; ac
                    }
                    else if (Request.QueryString["tab"] == "inv")
                    {
                        //TabContainer1.ActiveTab = tpViewInvoicelinks; ac
                    }
                }
                // Retain grid filter 


                //if (Request.Cookies[CookieName] != null)
                //{
                //    RadPersistenceManager1.LoadState();
                //    RadGrid_Location.Rebind();
                //    RadGrid_Equip.Rebind();
                //    RadGrid_Invoices.Rebind();
                //    RadGrid_OpenCalls.Rebind();

                //}
            }


        }
        // on postback call the selected date range method
        #endregion
        //string eventTarget = this.Request.Params.Get("__EVENTTARGET");
        //switch (eventTarget)
        //{
        //    case "ctl00_ContentPlaceHolder1_rdDay":
        //        AddClass();
        //        rdDay_CheckedChanged();
        //        GetData();
        //        break;
        //    case "ctl00_ContentPlaceHolder1_rdWeek":
        //        AddClass();
        //        rdWeek_CheckedChanged();
        //        GetData();
        //        break;
        //    case "ctl00_ContentPlaceHolder1_rdMonth":
        //        AddClass();
        //        rdMonth_CheckedChanged();
        //        GetData();
        //        break;
        //    case "ctl00_ContentPlaceHolder1_rdQuarter":
        //        AddClass();
        //        rdQuarter_CheckedChanged();
        //        GetData();
        //        break;
        //    case "ctl00_ContentPlaceHolder1_rdYear":
        //        AddClass();
        //        rdYear_CheckedChanged();
        //        GetData();
        //        break;


        //}


        /***********GetDataProspect****************/

        //FillProspect();

        /***************/



        if (Request.QueryString["o"] == null)
        {
            Permission();
        }
        else
        {
            lnkClose.Visible = false;
            divClose.Visible = false;
        }

        if (Request.QueryString["f"] != null && Request.QueryString["f"] == "r" && Session["filterstateAddCustomerHistory"] != null)
        {
            UpdateControl();
            Session.Remove("filterstateAddCustomerHistory");
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
            //ds = (DataTable)Session["userinfo"];
            ds = GetUserById();
            /// Owner ///////////////////------->

            string OwnerPermission = ds.Rows[0]["Owner"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Owner"].ToString();
            string stAddeOwner = OwnerPermission.Length < 1 ? "Y" : OwnerPermission.Substring(0, 1);
            string stEditeOwner = OwnerPermission.Length < 2 ? "Y" : OwnerPermission.Substring(1, 1);
            string DeleteOwner = OwnerPermission.Length < 3 ? "Y" : OwnerPermission.Substring(2, 1);
            string ViewOwner = OwnerPermission.Length < 4 ? "Y" : OwnerPermission.Substring(3, 1);

            if (ViewOwner == "N")
            {
                result = false;
            }
            else if (Request.QueryString["uid"] == null)
            {
                if (stAddeOwner == "N")
                {
                    result = false;
                }
            }
            else if (stEditeOwner == "N")
            {
                if (ViewOwner == "Y")
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
    private void GetQBInt()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        _getConnectionConfig.ConnConfig = Session["config"].ToString();

        List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "CustomersAPI/CustomersList_GetControl";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _GetControlViewModel = serializer.Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
        }
        else
        {
            ds = objBL_User.getControl(objPropUser);
        }


        ViewState["qbint"] = "0";

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["QBIntegration"].ToString() == "1")
                ViewState["qbint"] = "1";
        }
    }
    private void FillSpecifyLocation()
    {
        try
        {
            _objLoc.ConnConfig = Session["config"].ToString();
            _GetAllLocationOnCustomer.ConnConfig = Session["config"].ToString();

            DataSet _dsLocation = new DataSet();

            List<GetAllLocationOnCustomerViewModel> _lstGetAllLocationOnCustomer = new List<GetAllLocationOnCustomerViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "CustomersAPI/AddCustomer_GetAllLocationOnCustomer";

                _GetAllLocationOnCustomer.ownerId = Convert.ToInt32(Request.QueryString["uid"]);

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetAllLocationOnCustomer);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetAllLocationOnCustomer = serializer.Deserialize<List<GetAllLocationOnCustomerViewModel>>(_APIResponse.ResponseData);
                _dsLocation = CommonMethods.ToDataSet<GetAllLocationOnCustomerViewModel>(_lstGetAllLocationOnCustomer);
            }
            else
            {
                _dsLocation = objBL_Customer.getAllLocationOnCustomer(_objLoc, Convert.ToInt32(Request.QueryString["uid"]));
            }

            ddlSpecifiedLocation.Items.Clear();
            if (_dsLocation.Tables[0].Rows.Count > 0)
            {
                ddlSpecifiedLocation.Items.Add(new ListItem(":: Select ::", "0"));
                ddlSpecifiedLocation.AppendDataBoundItems = true;

                ddlSpecifiedLocation.DataSource = _dsLocation;
                ddlSpecifiedLocation.DataValueField = "Loc";
                ddlSpecifiedLocation.DataTextField = "Tag";
                ddlSpecifiedLocation.DataBind();
            }
            else
            {
                ddlSpecifiedLocation.Items.Add(new ListItem("No Locations Available", "0"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #endregion

    #region Functions
    private void Permission()
    {



        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");

            lnkAddnew.Visible = false;
            btnDelete.Visible = false;
            btnEdit.Visible = false;
            lblHeader.Text = "Customer";
        }

        if (Session["MSM"].ToString() == "TS")
        {

            Response.Redirect("home.aspx");

        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }
    }
    private DataSet GetCustomFields(string name)
    {
        DataSet ds = new DataSet();
        objGeneral.CustomName = name;
        objGeneral.ConnConfig = Session["config"].ToString();

        _getCustomFields.CustomName = name;
        _getCustomFields.ConnConfig = Session["config"].ToString();

        List<CustomViewModel> _lstCustomViewModel = new List<CustomViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "CustomersAPI/CustomersList_GetCustomFields";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomFields);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstCustomViewModel = serializer.Deserialize<List<CustomViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<CustomViewModel>(_lstCustomViewModel);
        }
        else
        {
            ds = objBL_General.getCustomFields(objGeneral);
        }

        return ds;
    }
    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            ViewState["CompPermission"] = 1;
            dvCompanyPermission.Style["display"] = "block";
            FillCompany();
            RadGrid_Opportunity.Columns.FindByDataField("Company").Visible = true;
        }
        else
        {
            ViewState["CompPermission"] = 0;
            dvCompanyPermission.Style["display"] = "none";
            RadGrid_Opportunity.Columns.FindByDataField("Company").Visible = false;
        }
    }
    private void ClearControls()
    {

        ResetFormControlValues(this);
        CreateTable();

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
    public string Truncate(string Value, int length)
    {
        if (Value.Length > length)
        {
            Value = Value.Substring(0, length);
        }
        return Value;
    }
    private void FillCompany()
    {
        objCompany.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();

        _GetCompanyByCustomer.UserID = Convert.ToInt32(Session["UserID"].ToString());
        _GetCompanyByCustomer.DBName = Session["dbname"].ToString();
        _GetCompanyByCustomer.ConnConfig = Session["config"].ToString();

        DataSet ds = new DataSet();

        List<CompanyOfficeViewModel> _lstCompanyOffice = new List<CompanyOfficeViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "CustomersAPI/CustomersList_GetCompanyByCustomer";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCompanyByCustomer);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstCompanyOffice = serializer.Deserialize<List<CompanyOfficeViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<CompanyOfficeViewModel>(_lstCompanyOffice);
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

    #region Customers
    private void FillCustomerType()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        _getCustomerType.ConnConfig = Session["config"].ToString();

        List<GetCustomerTypeViewModel> _lstGetCustomerType = new List<GetCustomerTypeViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "CustomersAPI/CustomersList_GetCustomerType";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomerType);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetCustomerType = serializer.Deserialize<List<GetCustomerTypeViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetCustomerTypeViewModel>(_lstGetCustomerType);
        }
        else
        {
            ds = objBL_User.getCustomerType(objPropUser);
        }

        ddlUserType.DataSource = ds.Tables[0];
        ddlUserType.DataTextField = "Type";
        ddlUserType.DataValueField = "Type";
        ddlUserType.DataBind();
        ddlUserType.Items.Insert(0, new ListItem("Select", "0"));


    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("customers.aspx");
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Submit();
        #region Update Contact Grid 
        objPropUser.CustomerID = Convert.ToInt32(Request.QueryString["uid"]);
        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.ConnConfig = Session["config"].ToString();

        _GetCustomerByID.CustomerID = Convert.ToInt32(Request.QueryString["uid"]);
        _GetCustomerByID.DBName = Session["dbname"].ToString();
        _GetCustomerByID.ConnConfig = Session["config"].ToString();

        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();
        DataSet ds3 = new DataSet();

        ListGetCustomerByID _listGetCustomerByID = new ListGetCustomerByID();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "CustomersAPI/AddCustomer_GetCustomerByID";

            _GetCustomerByID.IsSalesAsigned = new GeneralFunctions().GetSalesAsigned();

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomerByID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _listGetCustomerByID = serializer.Deserialize<ListGetCustomerByID>(_APIResponse.ResponseData);

            ds1 = _listGetCustomerByID.lstTable1.ToDataSet();
            ds2 = _listGetCustomerByID.lstTable2.ToDataSet();
            ds3 = _listGetCustomerByID.lstTable3.ToDataSet();

            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();

            dt1 = ds1.Tables[0];
            dt2 = ds2.Tables[0];
            dt3 = ds3.Tables[0];

            dt1.TableName = "Table1";
            dt2.TableName = "Table2";
            dt3.TableName = "Table3";
            ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy() });
        }
        else
        {
            ds = objBL_User.getCustomerByID(objPropUser, new GeneralFunctions().GetSalesAsigned());
        }

        RadGrid_gvContacts.DataSource = ds.Tables[1];
        RadGrid_gvContacts.DataBind();
        ViewState["contacttable"] = ds.Tables[1];
        RadGrid_gvLogs.Rebind();
        #endregion
        if (Request.QueryString["o"] != null)
        {
            if (ViewState["custid"] != null && !String.IsNullOrEmpty(ViewState["custid"].ToString()))
            {
                Response.Redirect("AddCustomer.aspx?uid=" + ViewState["custid"] + "&o=1");
            }

        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Submit();
        if (Convert.ToInt32(ViewState["CompPermission"]) == 1)
        {
            Response.Redirect(Request.RawUrl);
        }
    }
    private void Submit()
    {
        try
        {
            objPropUser.FirstName = txtCName.Text;
            objPropUser.Address = txtGoogleAutoc.Text;
            objPropUser.City = txtCity.Text;
            objPropUser.Password = txtPassword.Text.Trim();
            objPropUser.State = txtState.Text.Trim();
            objPropUser.Country = ddlCountry.SelectedValue;
            objPropUser.Username = txtUserName.Text.Trim();
            objPropUser.Zip = txtZip.Text;
            objPropUser.Remarks = txtRemarks.Text;
            objPropUser.MainContact = txtMaincontact.Text;
            objPropUser.Phone = txtPhoneCust.Text;
            objPropUser.Website = txtWebsite.Text;
            objPropUser.Email = txtEmail.Text;
            objPropUser.Cell = txtCell.Text;
            objPropUser.Fax = txtFax.Text;
            objPropUser.Type = ddlUserType.SelectedValue;
            objPropUser.Status = Convert.ToInt16(ddlCustStatus.SelectedValue);
            objPropUser.Lat = lat.Value.Trim();
            objPropUser.Lng = lng.Value.Trim();

            objPropUser.AccountNo = txtAcctno.Text.Trim();

            //API
            _UpdateCustomer.FirstName = txtCName.Text;
            _UpdateCustomer.Address = txtGoogleAutoc.Text;
            _UpdateCustomer.City = txtCity.Text;
            _UpdateCustomer.Password = txtPassword.Text.Trim();
            _UpdateCustomer.State = txtState.Text.Trim();
            _UpdateCustomer.Country = ddlCountry.SelectedValue;
            _UpdateCustomer.Username = txtUserName.Text.Trim();
            _UpdateCustomer.Zip = txtZip.Text;
            _UpdateCustomer.Remarks = txtRemarks.Text;
            _UpdateCustomer.MainContact = txtMaincontact.Text;
            _UpdateCustomer.Phone = txtPhoneCust.Text;
            _UpdateCustomer.Website = txtWebsite.Text;
            _UpdateCustomer.Email = txtEmail.Text;
            _UpdateCustomer.Cell = txtCell.Text;
            _UpdateCustomer.Fax = txtFax.Text;
            _UpdateCustomer.Type = ddlUserType.SelectedValue;
            _UpdateCustomer.Status = Convert.ToInt16(ddlCustStatus.SelectedValue);
            _UpdateCustomer.Lat = lat.Value.Trim();
            _UpdateCustomer.Lng = lng.Value.Trim();
            _UpdateCustomer.AccountNo = txtAcctno.Text.Trim();

            _AddCustomer.FirstName = txtCName.Text;
            _AddCustomer.Address = txtGoogleAutoc.Text;
            _AddCustomer.City = txtCity.Text;
            _AddCustomer.Password = txtPassword.Text.Trim();
            _AddCustomer.State = txtState.Text.Trim();
            _AddCustomer.Country = ddlCountry.SelectedValue;
            _AddCustomer.Username = txtUserName.Text.Trim();
            _AddCustomer.Zip = txtZip.Text;
            _AddCustomer.Remarks = txtRemarks.Text;
            _AddCustomer.MainContact = txtMaincontact.Text;
            _AddCustomer.Phone = txtPhoneCust.Text;
            _AddCustomer.Website = txtWebsite.Text;
            _AddCustomer.Email = txtEmail.Text;
            _AddCustomer.Cell = txtCell.Text;
            _AddCustomer.Fax = txtFax.Text;
            _AddCustomer.Type = ddlUserType.SelectedValue;
            _AddCustomer.Status = Convert.ToInt16(ddlCustStatus.SelectedValue);
            _AddCustomer.Lat = lat.Value.Trim();
            _AddCustomer.Lng = lng.Value.Trim();
            _AddCustomer.AccountNo = txtAcctno.Text.Trim();

            if ((ddlSpecifiedLocation.Items.Count - 2) > 0)
            {
                objPropUser.Billing = Convert.ToInt16(ddlBilling.SelectedValue);
                objPropUser.Central = Convert.ToInt32(ddlSpecifiedLocation.SelectedValue);

                _UpdateCustomer.Billing = Convert.ToInt16(ddlBilling.SelectedValue);
                _UpdateCustomer.Central = Convert.ToInt32(ddlSpecifiedLocation.SelectedValue);

                _AddCustomer.Billing = Convert.ToInt16(ddlBilling.SelectedValue);
            }
            else
            {
                objPropUser.Billing = 0;
                _UpdateCustomer.Billing = 0;
                _AddCustomer.Billing = 0;
            }

            if (chkScheduleBrd.Checked == true)
            {
                objPropUser.Schedule = 1;
                _UpdateCustomer.Schedule = 1;
                _AddCustomer.Schedule = 1;
            }
            else
            {
                objPropUser.Schedule = 0;
                _UpdateCustomer.Schedule = 0;
                _AddCustomer.Schedule = 0;
            }

            if (chkMap.Checked == true)
            {
                objPropUser.Mapping = 1;
                _UpdateCustomer.Mapping = 1;
                _AddCustomer.Mapping = 1;
            }
            else
            {
                objPropUser.Mapping = 0;
                _UpdateCustomer.Mapping = 0;
                _AddCustomer.Mapping = 0;
            }

            if (chkInternet.Checked == true)
            {
                objPropUser.Internet = 1;
                _UpdateCustomer.Internet = 1;
                _AddCustomer.Internet = 1;
            }
            else
            {
                objPropUser.Internet = 0;
                _UpdateCustomer.Internet = 0;
                _AddCustomer.Internet = 0;
            }

            objPropUser.EquipID = Convert.ToInt16(chkEquipments.Checked);
            objPropUser.grpbyWO = Convert.ToInt16(chkGrpWO.Checked);
            objPropUser.openticket = Convert.ToInt16(chkOpenTicket.Checked);

            _UpdateCustomer.EquipID = Convert.ToInt16(chkEquipments.Checked);
            _UpdateCustomer.grpbyWO = Convert.ToInt16(chkGrpWO.Checked);
            _UpdateCustomer.openticket = Convert.ToInt16(chkOpenTicket.Checked);

            _AddCustomer.EquipID = Convert.ToInt16(chkEquipments.Checked);
            _AddCustomer.grpbyWO = Convert.ToInt16(chkGrpWO.Checked);
            _AddCustomer.openticket = Convert.ToInt16(chkOpenTicket.Checked);

            if (ViewState["contacttable"] != null)
            {
                objPropUser.ContactData = (DataTable)ViewState["contacttable"];

                DataTable viewstateData = (DataTable)ViewState["contacttable"];

                if (viewstateData.Rows.Count == 0)
                {
                    DataTable returndt = SaveEmptyDatatable();
                    _UpdateCustomer.ContactData = returndt;
                }
                else
                {
                    _UpdateCustomer.ContactData = (DataTable)ViewState["contacttable"];
                }
            }

            if (Session["MSM"].ToString() == "TS")
            {
                objPropUser.IsTSDatabase = 1;
                _UpdateCustomer.IsTSDatabase = 1;
            }
            else
            {
                objPropUser.IsTSDatabase = 0;
                _UpdateCustomer.IsTSDatabase = 0;
            }

            if (ddlBilling.SelectedValue == "1")
            {
                var countSpecifiedLocationItems = ddlSpecifiedLocation.Items.Count - 1;
                if (countSpecifiedLocationItems == 0)
                {
                    // ClientScript.RegisterStartupScript(Page.GetType(),"ValidateSpecifyLocation","ValidateSpecifyLocation()", true);
                    divLabelMessage.Visible = true;
                    return;
                }
                else if (ddlSpecifiedLocation.SelectedValue == "0")
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySpecifyLocation", "noty({text: 'Please select specify location!',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    return;
                }

            }
            if (!string.IsNullOrEmpty(txtBillRate.Text))
            {
                objPropUser.BillRate = Convert.ToDouble(txtBillRate.Text);
                _UpdateCustomer.BillRate = Convert.ToDouble(txtBillRate.Text);
                _AddCustomer.BillRate = Convert.ToDouble(txtBillRate.Text);
            }
            if (!string.IsNullOrEmpty(txtOt.Text))
            {
                objPropUser.RateOT = Convert.ToDouble(txtOt.Text);
                _UpdateCustomer.RateOT = Convert.ToDouble(txtOt.Text);
                _AddCustomer.RateOT = Convert.ToDouble(txtOt.Text);
            }
            if (!string.IsNullOrEmpty(txtNt.Text))
            {
                objPropUser.RateNT = Convert.ToDouble(txtNt.Text);
                _UpdateCustomer.RateNT = Convert.ToDouble(txtNt.Text);
                _AddCustomer.RateNT = Convert.ToDouble(txtNt.Text);
            }
            if (!string.IsNullOrEmpty(txtDt.Text))
            {
                objPropUser.RateDT = Convert.ToDouble(txtDt.Text);
                _UpdateCustomer.RateDT = Convert.ToDouble(txtDt.Text);
                _AddCustomer.RateDT = Convert.ToDouble(txtDt.Text);
            }
            if (!string.IsNullOrEmpty(txtTravel.Text))
            {
                objPropUser.RateTravel = Convert.ToDouble(txtTravel.Text);
                _UpdateCustomer.RateTravel = Convert.ToDouble(txtTravel.Text);
                _AddCustomer.RateTravel = Convert.ToDouble(txtTravel.Text);
            }
            if (!string.IsNullOrEmpty(txtMileage.Text))
            {
                objPropUser.MileageRate = Convert.ToDouble(txtMileage.Text);
                _UpdateCustomer.MileageRate = Convert.ToDouble(txtMileage.Text);
                _AddCustomer.MileageRate = Convert.ToDouble(txtMileage.Text);
            }

            objPropUser.dtDocs = SaveDocInfo();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.MOMUSer = Session["User"].ToString();
            objPropUser.Custom1 = txtCst1.Text;
            objPropUser.Custom2 = txtCst2.Text;

            objPropUser.ShutdownAlert = Convert.ToInt32(chkShutdownAlert.Checked);
            objPropUser.ShutdownReason = txtAlert.Text;
            DataTable data = SaveDocInfo();
            if (data.Rows.Count == 0)
            {
                DataTable returndt = EmptyDatatable();
                _UpdateCustomer.dtDocs = returndt;
            }
            else
            {
                _UpdateCustomer.dtDocs = SaveDocInfo();
            }
            _UpdateCustomer.ConnConfig = Session["config"].ToString();
            _UpdateCustomer.MOMUSer = Session["User"].ToString();
            _UpdateCustomer.Custom1 = txtCst1.Text;
            _UpdateCustomer.Custom2 = txtCst2.Text;
            _UpdateCustomer.ShutdownAlert = Convert.ToInt32(chkShutdownAlert.Checked);
            _UpdateCustomer.ShutdownReason = txtAlert.Text;

            _AddCustomer.ConnConfig = Session["config"].ToString();
            _AddCustomer.MOMUSer = Session["User"].ToString();
            _AddCustomer.Custom1 = txtCst1.Text;
            _AddCustomer.Custom2 = txtCst2.Text;
            _AddCustomer.ShutdownAlert = Convert.ToInt32(chkShutdownAlert.Checked);
            _AddCustomer.ShutdownReason = txtAlert.Text;



            if (Convert.ToInt32(ViewState["mode"]) == 1)
            {
                if (Convert.ToInt32(ViewState["CompPermission"]) == 1)
                {
                    objPropUser.EN = Convert.ToInt32(ddlCompanyEdit.SelectedValue);
                    _UpdateCustomer.EN = Convert.ToInt32(ddlCompanyEdit.SelectedValue);
                    _AddCustomer.EN = Convert.ToInt32(ddlCompanyEdit.SelectedValue);
                }
                else
                {
                    objPropUser.EN = 0;
                    _UpdateCustomer.EN = 0;
                    _AddCustomer.EN = 0;
                }
                objPropUser.CustomerID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                _UpdateCustomer.CustomerID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                _AddCustomer.CustomerID = Convert.ToInt32(Request.QueryString["uid"].ToString());

                ViewState["custid"] = objPropUser.CustomerID;
                ViewState["custid"] = _UpdateCustomer.CustomerID;

                ViewState["custid"] = _AddCustomer.CustomerID;

                if (hdnAcctID.Value.Trim() != txtAcctno.Text.Trim())
                {
                    if (SageAlert() == 1)
                    {
                        return;
                    }
                }

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "CustomersAPI/AddCustomer_UpdateCustomer";

                    _UpdateCustomer.CopyToLocAndJob = CopyToLocAndJob.Checked;

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateCustomer);
                }
                else
                {
                    objBL_User.UpdateCustomer(objPropUser, CopyToLocAndJob.Checked);
                }

                hdnAcctID.Value = txtAcctno.Text;
                if (Request.QueryString["estimateid"] != null)
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Lead Converted Successfully.'); window.location.href='addestimate.aspx?uid=" + Request.QueryString["estimateid"].ToString() + "';", true);
                }
                else
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Customer updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            else
            {
                if (Convert.ToInt32(ViewState["CompPermission"]) == 1)
                {
                    objPropUser.EN = Convert.ToInt32(ddlCompany.SelectedValue);
                    _AddCustomer.EN = Convert.ToInt32(ddlCompany.SelectedValue);
                }
                else
                {
                    objPropUser.EN = 0;
                    _AddCustomer.EN = 0;
                }
                if (SageAlert() == 1)
                {
                    return;
                }


                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "CustomersAPI/AddCustomer_AddCustomer";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddCustomer);

                    object JsonData = JsonConvert.DeserializeObject(_APIResponse.ResponseData);
                    _AddCustomer.CustomerID = Convert.ToInt32(JsonData.ToString());

                    ConvertProspectWizard(_AddCustomer.CustomerID);

                    ViewState["mode"] = 0;
                    ViewState["custid"] = _AddCustomer.CustomerID;
                    Session["custidloc"] = _AddCustomer.CustomerID;
                }
                else
                {
                    objBL_User.AddCustomer(objPropUser);
                    ConvertProspectWizard(objPropUser.CustomerID);

                    ViewState["mode"] = 0;
                    ViewState["custid"] = objPropUser.CustomerID;
                    Session["custidloc"] = objPropUser.CustomerID;
                }

                ClearControls();

                if (Request.QueryString["cpw"] == null)
                {
                    if (Request.QueryString["o"] == null)
                    {
                        var str = "if (confirm('Customer added successfully. Do you want to add location for the saved customer?') == true) { window.location.href = 'addlocation.aspx?page=addcustomer&lid=" + ViewState["custid"] + "'}else{window.location.href='addcustomer?uid=" + ViewState["custid"] + "'}";
                        //string script = "function f(){$find(\"" + RadWindowCustomerSaved.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "key", script, true);
                        ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Customer added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyConfirmAddLocation", str, true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Customer added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }
                }
            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateparent", "if(window.opener && !window.opener.closed) { if(window.opener.document.getElementById('ctl00_ContentPlaceHolder1_lnkSearch')) window.opener.document.getElementById('ctl00_ContentPlaceHolder1_lnkSearch').click();}", true);

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
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
    private void ConvertProspectWizard(int custID)
    {
        if (Request.QueryString["cpw"] != null)
        {
            string ProspectID = Request.QueryString["prospectid"].ToString();
            if (Request.QueryString["opid"] != null)
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Customer Saved Successfully. Continue to Convert Lead Wizard.'); window.location.href='addlocation.aspx?cpw=1&prospectid=" + ProspectID + "&customerid=" + custID + "&opid=" + Request.QueryString["opid"].ToString() + "';", true);
            else if (Request.QueryString["ticketid"] != null)
            {
                string ticketid = Request.QueryString["ticketid"].ToString();
                string comp = Request.QueryString["comp"].ToString();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Customer Saved Successfully. Continue to Convert Lead Wizard.'); window.location.href='addlocation.aspx?cpw=1&prospectid=" + ProspectID + "&customerid=" + custID + "&Ticketid=" + ticketid + "&comp=" + comp + "';", true);
            }
            else if (Request.QueryString["estimateid"] != null)
            {
                string estimateid = Request.QueryString["estimateid"].ToString();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Customer Saved Successfully. Continue to Convert Lead Wizard.'); window.location.href='addlocation.aspx?cpw=1&prospectid=" + ProspectID + "&customerid=" + custID + "&estimatecid=" + estimateid + "';", true);
            }
            else
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Customer Saved Successfully. Continue to Convert Lead Wizard.'); window.location.href='addlocation.aspx?cpw=1&prospectid=" + ProspectID + "&customerid=" + custID + "';", true);
        }
    }

    protected void lnkCustomerStatement_Click(object sender, EventArgs e)
    {
        var url = "customerstatement.aspx?uid=" + Request.QueryString["uid"] + "&page=addcustomer&lid=" + Request.QueryString["uid"];
        string reportCustom = System.Web.Configuration.WebConfigurationManager.AppSettings["CustomerInvoieStatement"];
        if (!string.IsNullOrEmpty(reportCustom) && reportCustom.ToLower().Contains(".mrt"))
        {
            url = "customerstatementreport.aspx?uid=" + Request.QueryString["uid"] + "&page=addcustomer";
        }

        Response.Redirect(url);
    }

    protected void lnkCustomerTransLedger_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["uid"] != null)
        {
            Response.Redirect("CustomerTransactionLedger.aspx?uid=" + Request.QueryString["uid"]);
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        contactWindow.Title = "Edit Contact";
        foreach (GridDataItem di in RadGrid_gvContacts.SelectedItems)
        {
            DataTable dt = (DataTable)ViewState["contacttable"];
            HiddenField hdnSelected = (HiddenField)di.Cells[0].FindControl("hdnSelected");
            Label lblindex = (Label)di.Cells[0].FindControl("lblindex");

            DataRow dr = dt.Rows[Convert.ToInt32(lblindex.Text)];

            txtContcName.Text = dr["Name"].ToString();
            txtContPhone.Text = dr["Phone"].ToString();
            txtContFax.Text = dr["Fax"].ToString();
            txtContCell.Text = dr["Cell"].ToString();
            txtContEmail.Text = dr["Email"].ToString();
            txtTitle.Text = dr["Title"].ToString();
            chkShutdownA.Checked = Convert.ToBoolean(dr["ShutdownAlert"]);
            ViewState["editcon"] = 1;
            ViewState["index"] = lblindex.Text;

        }

        string script = "function f(){$find(\"" + contactWindow.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }
    //protected void LinkButton2_Click(object sender, EventArgs e)
    //{
    //    //TogglePopup();

    //    DataTable dt = (DataTable)ViewState["contacttable"];
    //    RadGrid_gvContacts.VirtualItemCount = dt.Rows.Count;
    //    RadGrid_gvContacts.DataSource = dt;

    //}
    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        if (hdnAddeContact.Value == "Y")
        {
            contactWindow.Title = "Add Contact";
            txtContcName.Text = "";
            txtTitle.Text = "";
            txtContPhone.Text = "";
            txtContFax.Text = "";
            txtContCell.Text = "";
            txtContEmail.Text = "";
            chkShutdownA.Checked = false;
            string script = "function f(){$find(\"" + contactWindow.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
        }
    }
    protected void lnkPrint_Click(object sender, EventArgs e)
    {
        SaveFilter();
        var searchText = string.Empty;
        if (ddlSearch.SelectedValue == "t.cat")
        {
            searchText = ddlCategory.SelectedValue;
        }
        else
        {
            searchText = txtSearch.Text;
        }

        // Redirect when close the report
        var redirect = HttpUtility.UrlEncode(Request.RawUrl);
        if (Request.RawUrl.IndexOf("f=r") == -1)
        {
            redirect = HttpUtility.UrlEncode(Request.RawUrl + "&f=r");

        }

        List<RetainFilter> filters = new List<RetainFilter>();
        var filterExpression = Convert.ToString(RadGrid_OpenCalls.MasterTableView.FilterExpression);
        if (!string.IsNullOrEmpty(filterExpression))
        {
            foreach (GridColumn column in RadGrid_OpenCalls.MasterTableView.OwnerGrid.Columns)
            {

                String filterValues = String.Empty;
                String columnName = column.UniqueName;

                if (column.UniqueName == "cat")
                {
                    if (column.ListOfFilterValues != null)
                    {
                        List<string> listFil = new List<string>(column.ListOfFilterValues);
                        filterValues = String.Join(",", listFil.Select(x => string.Format("{0}", x)));
                        columnName = "cat";
                    }
                    else
                    {
                        filterValues = column.CurrentFilterValue;
                    }
                }
                else
                {
                    filterValues = column.CurrentFilterValue;
                }

                if (filterValues != "")
                {
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = columnName;
                    filter.FilterValue = filterValues;
                    filters.Add(filter);
                }


            }
        }

        if (filters.Count > 0)
        {
            Session["TicketListRadGVFilters"] = filters;
        }

        Response.Redirect("CompletedTicketReport.aspx?cid=" + Convert.ToInt32(Request.QueryString["uid"]) + "&sd=" + txtfromDate.Text + "&ed=" + txtToDate.Text + "&stype=" + ddlSearch.SelectedItem.Value + "&stext=" + searchText + "&department=-1" + "&redirect=" + redirect);
    }

    protected void lnkContactSave_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)ViewState["contacttable"];

        if (ViewState["editcon"].ToString() == "1")
        {
            //dt.Rows.RemoveAt(Convert.ToInt32(ViewState["index"]));
            //dt.Rows.InsertAt(dr, Convert.ToInt32(ViewState["index"]));
            dt.Rows[Convert.ToInt32(ViewState["index"])]["Name"] = Truncate(txtContcName.Text, 50);
            dt.Rows[Convert.ToInt32(ViewState["index"])]["Phone"] = Truncate(txtContPhone.Text, 50);
            dt.Rows[Convert.ToInt32(ViewState["index"])]["Fax"] = Truncate(txtContFax.Text, 22);
            dt.Rows[Convert.ToInt32(ViewState["index"])]["Cell"] = Truncate(txtContCell.Text, 22);
            dt.Rows[Convert.ToInt32(ViewState["index"])]["Email"] = Truncate(txtContEmail.Text, 50);
            dt.Rows[Convert.ToInt32(ViewState["index"])]["Title"] = Truncate(txtTitle.Text, 50);
            dt.Rows[Convert.ToInt32(ViewState["index"])]["ShutdownAlert"] = chkShutdownA.Checked;
            ViewState["editcon"] = 0;
        }
        else
        {
            DataRow dr = dt.NewRow();

            dr["Name"] = Truncate(txtContcName.Text, 50);
            dr["Phone"] = Truncate(txtContPhone.Text, 50);
            dr["Fax"] = Truncate(txtContFax.Text, 22);
            dr["Cell"] = Truncate(txtContCell.Text, 22);
            dr["Email"] = Truncate(txtContEmail.Text, 50);
            dr["Title"] = Truncate(txtTitle.Text, 50);
            dr["ShutdownAlert"] = chkShutdownA.Checked;
            dt.Rows.Add(dr);
        }

        dt.AcceptChanges();

        ViewState["contacttable"] = dt;
        RadGrid_gvContacts.VirtualItemCount = dt.Rows.Count;
        RadGrid_gvContacts.DataSource = dt;
        RadGrid_gvContacts.Rebind();


        ClearContact();
        //TogglePopup();

        if (ViewState["mode"].ToString() == "1")
        {
            SubmitContact();
            getContact();
            RadGrid_gvContacts.Rebind();
        }
        getCustomerLog();
        RadGrid_gvLogs.Rebind();
        string script = "function f(){$find(\"" + contactWindow.ClientID + "\").close(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }
    private void SubmitContact()
    {
        try
        {
            if (ViewState["contacttable"] != null)
            {
                objPropUser.ContactData = (DataTable)ViewState["contacttable"];

                DataTable viewstateData = (DataTable)ViewState["contacttable"];

                if (viewstateData.Rows.Count == 0)
                {
                    DataTable returndt = SaveEmptyDatatable();
                    _UpdateCustomerContactRecordLog.ContactData = returndt;
                }
                else
                {
                    _UpdateCustomerContactRecordLog.ContactData = (DataTable)ViewState["contacttable"];
                }
            }

            if (Convert.ToInt32(ViewState["mode"]) == 1)
            {
                objPropUser.RolId = Convert.ToInt32(ViewState["rolid"].ToString());
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.MOMUSer = Session["User"].ToString();

                //API
                _UpdateCustomerContactRecordLog.RolId = Convert.ToInt32(ViewState["rolid"].ToString());
                _UpdateCustomerContactRecordLog.ConnConfig = Session["config"].ToString();
                _UpdateCustomerContactRecordLog.MOMUSer = Session["User"].ToString();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "CustomersAPI/AddCustomer_UpdateCustomerContactRecordLog";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateCustomerContactRecordLog);
                }
                else
                {
                    objBL_User.UpdateCustomerContactRecordLog(objPropUser);
                }

            }
        }
        catch (Exception ex)
        {

            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrContct", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }
    private void TogglePopup()
    {
        //if (pnlOverlay.Visible == false)
        //{
        //    pnlOverlay.Visible = true;
        //    modalContact.Visible = true;
        //}
        //else
        //{
        //    pnlOverlay.Visible = false;
        //    modalContact.Visible = false;
        //}
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
        dt.Columns.Add("EmailTicket", typeof(byte));
        dt.Columns.Add("EmailRecInvoice", typeof(byte));
        dt.Columns.Add("ShutdownAlert", typeof(byte));
        dt.Columns.Add("EmailRecTestProp", typeof(byte));

        ViewState["contacttable"] = dt;
    }

    //API
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
        dt.Columns.Add("EmailTicket", typeof(byte));
        dt.Columns.Add("EmailRecInvoice", typeof(byte));
        dt.Columns.Add("ShutdownAlert", typeof(byte));
        dt.Columns.Add("EmailRecTestProp", typeof(byte));

        DataRow dr = dt.NewRow();
        dr["ContactID"] = "0";
        dr["Name"] = "";
        dr["Phone"] = "";
        dr["Fax"] = "";
        dr["Cell"] = "";
        dr["Email"] = "";
        dr["Title"] = "";
        dr["EmailTicket"] = false;
        dr["EmailRecInvoice"] = false;
        dr["ShutdownAlert"] = false;
        dr["EmailRecTestProp"] = false;
        dt.Rows.Add(dr);

        return dt;
    }

    public DataTable EmptyDatatable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Portal", typeof(int));
        dt.Columns.Add("Remarks", typeof(string));

        DataRow dr = dt.NewRow();
        dr["ID"] = "0";
        dr["Portal"] = "0";
        dr["Remarks"] = "";

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
    }


    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["contacttable"];

            foreach (GridDataItem di in RadGrid_gvContacts.Items)
            {
                CheckBox chkSelected = (CheckBox)di["ClientSelectColumn"].Controls[0];
                Label lblindex = (Label)di.FindControl("lblIndex");

                if (chkSelected.Checked == true)
                {
                    dt.Rows.RemoveAt(Int32.Parse(lblindex.Text));
                }
            }

            dt.AcceptChanges();
            ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Contact deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            ViewState["contacttable"] = dt;
            RadGrid_gvContacts.VirtualItemCount = dt.Rows.Count;
            RadGrid_gvContacts.DataSource = dt;
            RadGrid_gvContacts.Rebind();



            if (ViewState["mode"].ToString() == "1")
            {
                SubmitContact();
            }
            getCustomerLog();
            RadGrid_gvLogs.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }
    //protected void gvContacts_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    e.Row.ID = uniqueRowId.ToString();
    //    ++uniqueRowId;
    //}

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["customers"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["ID"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);
        int c = dt.Rows.Count - 1;
        if (index < c)
        {
            Response.Redirect("addcustomer.aspx?uid=" + dt.Rows[index + 1]["id"]);
        }
        if (index == c)
        {
            Response.Redirect("addcustomer.aspx?uid=" + dt.Rows[0]["id"]);
        }
    }
    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["customers"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["ID"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);

        if (index > 0)
        {
            Response.Redirect("addcustomer.aspx?uid=" + dt.Rows[index - 1]["id"]);
        }
        if (index == 0)
        {
            Response.Redirect("addcustomer.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["id"]);
        }
    }
    protected void lnkLast_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["customers"];
        Response.Redirect("addcustomer.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["id"]);
    }
    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["customers"];
        Response.Redirect("addcustomer.aspx?uid=" + dt.Rows[0]["id"]);
    }

    #endregion

    #region Locations

    private void FillLoc()
    {
        objPropUser.CustomerID = Convert.ToInt32(Request.QueryString["uid"]);
        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.ConnConfig = Session["config"].ToString();

        _GetCustomerByID.CustomerID = Convert.ToInt32(Request.QueryString["uid"]);
        _GetCustomerByID.DBName = Session["dbname"].ToString();
        _GetCustomerByID.ConnConfig = Session["config"].ToString();

        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();
        DataSet ds3 = new DataSet();

        ListGetCustomerByID _listGetCustomerByID = new ListGetCustomerByID();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "CustomersAPI/AddCustomer_GetCustomerByID";

            _GetCustomerByID.IsSalesAsigned = new GeneralFunctions().GetSalesAsigned();

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomerByID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _listGetCustomerByID = serializer.Deserialize<ListGetCustomerByID>(_APIResponse.ResponseData);

            ds1 = _listGetCustomerByID.lstTable1.ToDataSet();
            ds2 = _listGetCustomerByID.lstTable2.ToDataSet();
            ds3 = _listGetCustomerByID.lstTable3.ToDataSet();

            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();

            dt1 = ds1.Tables[0];
            dt2 = ds2.Tables[0];
            dt3 = ds3.Tables[0];

            dt1.TableName = "Table1";
            dt2.TableName = "Table2";
            dt3.TableName = "Table3";
            ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy() });
        }
        else
        {
            ds = objBL_User.getCustomerByID(objPropUser, new GeneralFunctions().GetSalesAsigned());
        }


        if (ds.Tables.Count > 2)
        {
            if (ds.Tables[2].Rows.Count > 0)
            {
                RadGrid_Location.VirtualItemCount = ds.Tables[2].Rows.Count;
                RadGrid_Location.DataSource = ds.Tables[2];

                Session["locationdataCust"] = ds.Tables[2];

            }
        }
    }
    protected void lnkAddLoc_Click(object sender, EventArgs e)
    {
        Response.Redirect("addlocation.aspx?page=addcustomer&lid=" + Request.QueryString["uid"].ToString());
    }
    protected void lnkEditLoc_Click(object sender, EventArgs e)
    {

        foreach (GridDataItem di in RadGrid_Location.Items)
        {
            Label lblUserID = (Label)di.Cells[0].FindControl("lblloc");
            TableCell cell = di["chkSelect"];
            CheckBox chkSelected = (CheckBox)cell.Controls[0];
            if (chkSelected.Checked == true)
            {
                Response.Redirect("addlocation.aspx?uid=" + lblUserID.Text + "&page=addcustomer&lid=" + Request.QueryString["uid"].ToString());
            }
        }
    }
    protected void lnkCopyloc_Click(object sender, EventArgs e)
    {

        foreach (GridDataItem di in RadGrid_Location.Items)
        {
            TableCell cell = di["chkSelect"];
            CheckBox chkSelected = (CheckBox)cell.Controls[0];
            Label lblUserID = (Label)di.Cells[1].FindControl("lblloc");
            if (chkSelected.Checked == true)
            {
                Response.Redirect("addlocation.aspx?uid=" + lblUserID.Text + "&t=c&page=addcustomer&lid=" + Request.QueryString["uid"].ToString());
            }
        }
    }
    protected void lnkDeleteLoc_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_Location.Items)
        {
            TableCell cell = di["chkSelect"];
            CheckBox chkSelected = (CheckBox)cell.Controls[0];
            Label lblUserID = (Label)di.Cells[1].FindControl("lblloc");
            if (chkSelected.Checked == true)
            {
                DeleteLocation(Convert.ToInt32(lblUserID.Text));
            }
        }
        RadGrid_Location.Rebind();
    }
    private void DeleteLocation(int LocID)
    {
        objPropUser.LocID = LocID;
        objPropUser.ConnConfig = Session["config"].ToString();

        _DeleteLocation.LocID = LocID;
        _DeleteLocation.ConnConfig = Session["config"].ToString();

        try
        {
            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "CustomersAPI/AddCustomer_DeleteLocation";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _DeleteLocation);
            }
            else
            {
                objBL_User.DeleteLocation(objPropUser);
            }

            ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Location deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            FillLoc();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }



    protected void gvLoc_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string gvrow = ((GridView)sender).DataKeys[e.Row.RowIndex].Value.ToString();
            HiddenField hdnSelected = (HiddenField)e.Row.FindControl("hdnSelected");
            CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
            if (hdnEditeLoc.Value == "Y" || hdnViewLoc.Value == "Y")
            {
                e.Row.Attributes["ondblclick"] = "location.href='addlocation.aspx?uid=" + gvrow + "'";
            }
            else
            {
                e.Row.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";
            }



            //e.Row.Attributes["onclick"] = "SelectRow('" + hdnSelected.ClientID + "','" + e.Row.ClientID + "','" + chkSelect.ClientID + "','" + gvLoc.ClientID + "');";

        }
    }
    protected void gvLoc_DataBound(object sender, EventArgs e)
    {
        //GridViewRow gvrPager = gvLoc.BottomPagerRow;

        //if (gvrPager == null) return;

        //// get your controls from the gridview
        //DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        //Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        //if (ddlPages != null)
        //{
        //    // populate pager
        //    for (int i = 0; i < gvLoc.PageCount; i++)
        //    {

        //        int intPageNumber = i + 1;
        //        ListItem lstItem = new ListItem(intPageNumber.ToString());

        //        if (i == gvLoc.PageIndex)
        //            lstItem.Selected = true;

        //        ddlPages.Items.Add(lstItem);
        //    }
        //}

        //// populate page count
        //if (lblPageCount != null)
        //    lblPageCount.Text = gvLoc.PageCount.ToString();
    }

    #endregion

    #region Equipment

    private void GetDataEquip()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.SearchBy = string.Empty;
        objPropUser.CustomerID = 0;

        //API
        _GetElev.ConnConfig = Session["config"].ToString();
        _GetElev.SearchBy = string.Empty;
        _GetElev.CustomerID = 0;

        if (!String.IsNullOrEmpty(Request.QueryString["uid"].ToString()))
        {
            objPropUser.CustomerID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            _GetElev.CustomerID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        }

        //objPropUser.SearchBy = "e.owner";
        //objPropUser.SearchValue = Request.QueryString["uid"].ToString();
        objPropUser.InstallDate = string.Empty;
        objPropUser.ServiceDate = string.Empty;
        objPropUser.Price = string.Empty;
        objPropUser.Manufacturer = string.Empty;
        objPropUser.Status = -1;

        _GetElev.InstallDate = string.Empty;
        _GetElev.ServiceDate = string.Empty;
        _GetElev.Price = string.Empty;
        _GetElev.Manufacturer = string.Empty;
        _GetElev.Status = -1;

        try
        {
            List<GetElevViewModel> _lstGetElev = new List<GetElevViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "CustomersAPI/AddCustomer_GetElev";

                _GetElev.IsSalesAsigned = new GeneralFunctions().GetSalesAsigned();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetElev);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetElev = serializer.Deserialize<List<GetElevViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetElevViewModel>(_lstGetElev);
            }
            else
            {
                ds = objBL_User.getElev(objPropUser, new GeneralFunctions().GetSalesAsigned());
            }

            BindGridDatatable(ds.Tables[0]);
        }
        catch { }
    }

    protected void lnkAddEquip_Click(object sender, EventArgs e)
    {
        Response.Redirect("addequipment.aspx?page=addcustomer&cuid=" + Request.QueryString["uid"].ToString());
    }
    protected void lnkEditEquip_Click(object sender, EventArgs e)
    {

        foreach (GridDataItem di in RadGrid_Equip.MasterTableView.Items)
        {
            CheckBox chkSelected = (CheckBox)di["chkSelect"].Controls[0];
            Label lblUserID = (Label)di.FindControl("lblId");

            if (chkSelected.Checked == true)
            {
                Response.Redirect("addequipment.aspx?uid=" + lblUserID.Text + "&page=addcustomer&cuid=" + Request.QueryString["uid"].ToString());
            }
        }
    }
    protected void lnkcopyEquip_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_Equip.MasterTableView.Items)
        {
            CheckBox chkSelected = (CheckBox)di["chkSelect"].Controls[0];
            Label lblUserID = (Label)di.FindControl("lblId");

            if (chkSelected.Checked == true)
            {
                Response.Redirect("addequipment.aspx?uid=" + lblUserID.Text + "&t=c&page=addcustomer&cuid=" + Request.QueryString["uid"].ToString());
            }
        }

    }
    protected void lnkDeleteEquip_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Equip.SelectedItems)
        {
            Label lblUserID = (Label)item.FindControl("lblId");
            DeleteEquipment(Convert.ToInt32(lblUserID.Text));
        }

    }
    private void DeleteEquipment(int EquipID)
    {
        objPropUser.EquipID = EquipID;
        objPropUser.ConnConfig = Session["config"].ToString();

        _DeleteEquipment.EquipID = EquipID;
        _DeleteEquipment.ConnConfig = Session["config"].ToString();

        try
        {
            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "CustomersAPI/AddCustomer_DeleteEquipment";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _DeleteEquipment);
            }
            else
            {
                objBL_User.DeleteEquipment(objPropUser);
            }

            ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Equipment deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);


            RadGrid_Equip.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    private void BindGridDatatable(DataTable dt)
    {
        Session["ElevSrchCust"] = dt;

        RadGrid_Equip.VirtualItemCount = dt.Rows.Count;
        RadGrid_Equip.DataSource = dt;
        //RadPersistenceManager1.SaveState();

    }



    //protected void gvEquip_DataBound(object sender, EventArgs e)
    //{
    //    GridViewRow gvrPager = gvEquip.BottomPagerRow;

    //    if (gvrPager == null) return;

    //    // get your controls from the gridview
    //    DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
    //    Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

    //    if (ddlPages != null)
    //    {
    //        // populate pager
    //        for (int i = 0; i < gvEquip.PageCount; i++)
    //        {

    //            int intPageNumber = i + 1;
    //            ListItem lstItem = new ListItem(intPageNumber.ToString());

    //            if (i == gvEquip.PageIndex)
    //                lstItem.Selected = true;

    //            ddlPages.Items.Add(lstItem);
    //        }
    //    }

    //    // populate page count
    //    if (lblPageCount != null)
    //        lblPageCount.Text = gvEquip.PageCount.ToString();
    //}



    #endregion

    #region Call History

    private void FillCategory()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        _GetCategory.ConnConfig = Session["config"].ToString();

        List<GetCategoryViewModel> _lstGetCategory = new List<GetCategoryViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "CustomersAPI/AddCustomer_GetCategory";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCategory);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetCategory = serializer.Deserialize<List<GetCategoryViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetCategoryViewModel>(_lstGetCategory);
        }
        else
        {
            ds = objBL_User.getCategory(objPropUser);
        }

        ddlCategory.DataSource = ds.Tables[0];
        ddlCategory.DataTextField = "type";
        ddlCategory.DataValueField = "type";
        ddlCategory.DataBind();

        ddlCategory.Items.Insert(0, new ListItem("None", "None"));
    }
    private void GetOpenCalls()
    {
        DataSet ds = new DataSet();
        string stdate = txtfromDate.Text + " 00:00:00";
        string enddate = txtToDate.Text + " 23:59:59";
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.CustID = Convert.ToInt32(Request.QueryString["uid"]);
        objMapData.Assigned = Convert.ToInt32(ddlStatus.SelectedValue);

        _GetCallHistory.ConnConfig = Session["config"].ToString();
        _GetCallHistory.CustID = Convert.ToInt32(Request.QueryString["uid"]);
        _GetCallHistory.Assigned = Convert.ToInt32(ddlStatus.SelectedValue);

        if (txtfromDate.Text != string.Empty)
        {
            //objMapData.StartDate = Convert.ToDateTime(txtfromDate.Text);
            objMapData.StartDate = Convert.ToDateTime(stdate);
            _GetCallHistory.StartDate = Convert.ToDateTime(stdate);
        }
        else
        {
            objMapData.StartDate = System.DateTime.MinValue;
            _GetCallHistory.StartDate = System.DateTime.MinValue;
        }

        if (txtToDate.Text != string.Empty)
        {
            objMapData.EndDate = Convert.ToDateTime(enddate);
            _GetCallHistory.EndDate = Convert.ToDateTime(enddate);
        }
        else
        {
            objMapData.EndDate = System.DateTime.MinValue;
            _GetCallHistory.EndDate = System.DateTime.MinValue;
        }

        objMapData.SearchBy = ddlSearch.SelectedValue.Trim();
        _GetCallHistory.SearchBy = ddlSearch.SelectedValue.Trim();

        if (ddlSearch.SelectedValue == "t.cat")
        {
            objMapData.SearchValue = ddlCategory.SelectedValue;
            _GetCallHistory.SearchValue = ddlCategory.SelectedValue;
        }
        else
        {
            objMapData.SearchValue = txtSearch.Text.Replace("'", "''");
            _GetCallHistory.SearchValue = txtSearch.Text.Replace("'", "''");
        }
        objMapData.Department = -1;
        _GetCallHistory.Department = -1;

        try
        {
            List<GetCallHistoryViewModel> _lstGetCallHistory = new List<GetCallHistoryViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "CustomersAPI/AddCustomer_GetCallHistory";

                _GetCallHistory.IsSalesAsigned = new GeneralFunctions().GetSalesAsigned();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCallHistory);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetCallHistory = serializer.Deserialize<List<GetCallHistoryViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetCallHistoryViewModel>(_lstGetCallHistory);
            }
            else
            {
                ds = new BL_Tickets().getCallHistory(objMapData, new GeneralFunctions().GetSalesAsigned());
            }

            DataTable result = processDataFilter(ds.Tables[0]);
            RadGrid_OpenCalls.VirtualItemCount = result.Rows.Count;
            RadGrid_OpenCalls.DataSource = result;
            Session["dtTicketListCust"] = result;
        }
        catch (Exception ex)
        {
            Session["dtTicketListCust"] = null;
        }
    }


    //protected void showModalPopupServerOperatorButton_Click(object sender, EventArgs e)
    //{
    //    this.programmaticModalPopup.Show();
    //}


    //protected void hideModalPopupViaServer_Click(object sender, EventArgs e)
    //{
    //    this.ModalPopupExtender1.Hide();
    //    iframeCustomer.Attributes["src"] = "";
    //    GetOpenCalls();
    //    RadGrid_OpenCalls.Rebind();
    //}

    protected void hideModalPopupViaServerConfirm_Click(object sender, EventArgs e)
    {
        Response.Redirect("addlocation.aspx?page=addcustomer&lid=" + ViewState["custid"].ToString());
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GetOpenCalls();
        RadGrid_OpenCalls.Rebind();
    }

    protected void lnkEditTicket_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_OpenCalls.MasterTableView.Items)
        {
            CheckBox chkSelected = (CheckBox)di["chkSelect"].Controls[0];
            Label lblTicketId = (Label)di.FindControl("lblTicketId");
            Label lblComp = (Label)di.FindControl("lblComp");

            if (chkSelected.Checked == true)
            {
                //Panel2.Attributes.Add("style", "display:none");
                String src = "addticket.aspx?id=" + lblTicketId.Text + "&comp=" + lblComp.Text;
                iframeCustomer.Attributes["src"] = src;
                //this.ModalPopupExtender1.Show();

                //string script = "function f(){$find(\"" + RadWindowAddTickets.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
        }
    }



    #endregion

    #region Invoices

    private void GetInvoices(string SearchValue, string SearchBy)
    {
        if (ddlSearchInv.SelectedValue == "i.ref")
        {
            objProp_Contracts.filterBy = "Invoice";
            objProp_Contracts.filterValue = "'" + txtSearchInv.Text + "'";

            _GetARRevenueCustShowAll.filterBy = "Invoice";
            _GetARRevenueCustShowAll.filterValue = "'" + txtSearchInv.Text + "'";

            _GetARRevenueCust.filterBy = "Invoice";
            _GetARRevenueCust.filterValue = "'" + txtSearchInv.Text + "'";
        }
        else if (ddlSearchInv.SelectedValue == "i.fdate")
        {
            objProp_Contracts.filterBy = "InvoiceDate";
            _GetARRevenueCustShowAll.filterBy = "InvoiceDate";
            _GetARRevenueCust.filterBy = "InvoiceDate";

            DateTime temp;
            if (DateTime.TryParse(txtInvDt.Text, out temp))
            {
                objProp_Contracts.StartDate = Convert.ToDateTime(txtInvDt.Text);
                _GetARRevenueCustShowAll.StartDate = Convert.ToDateTime(txtInvDt.Text);
                _GetARRevenueCust.StartDate = Convert.ToDateTime(txtInvDt.Text);
            }
            else
            {
                objProp_Contracts.StartDate = Convert.ToDateTime("01/01/1900 00:00:00");
                _GetARRevenueCustShowAll.StartDate = Convert.ToDateTime("01/01/1900 00:00:00");
                _GetARRevenueCust.StartDate = Convert.ToDateTime("01/01/1900 00:00:00");
            }

        }
        else if (ddlSearchInv.SelectedValue == "i.ID")
        {
            objProp_Contracts.filterValue = "LocID";
            objProp_Contracts.filterValue = txtSearchInv.Text;

            _GetARRevenueCustShowAll.filterValue = "LocID";
            _GetARRevenueCustShowAll.filterValue = txtSearchInv.Text;

            _GetARRevenueCust.filterValue = "LocID";
            _GetARRevenueCust.filterValue = txtSearchInv.Text;
        }
        else if (ddlSearchInv.SelectedValue == "i.Type")
        {
            objProp_Contracts.filterBy = "Department";
            objProp_Contracts.filterValue = ddlDepartment.SelectedValue;

            _GetARRevenueCustShowAll.filterBy = "Department";
            _GetARRevenueCustShowAll.filterValue = ddlDepartment.SelectedValue;

            _GetARRevenueCust.filterBy = "Department";
            _GetARRevenueCust.filterValue = ddlDepartment.SelectedValue;
        }

        else if (ddlSearchInv.SelectedValue == "l.loc")
        {
            objProp_Contracts.filterBy = "Location";
            objProp_Contracts.filterValue = ddllocation.SelectedItem.Text;

            _GetARRevenueCustShowAll.filterBy = "Location";
            _GetARRevenueCustShowAll.filterValue = ddllocation.SelectedItem.Text;

            _GetARRevenueCust.filterBy = "Location";
            _GetARRevenueCust.filterValue = ddllocation.SelectedItem.Text;
        }
        else
        {
            objProp_Contracts.filterBy = "";
            objProp_Contracts.filterValue = txtSearchInv.Text;

            _GetARRevenueCustShowAll.filterBy = "";
            _GetARRevenueCustShowAll.filterValue = txtSearchInv.Text;

            _GetARRevenueCust.filterBy = "";
            _GetARRevenueCust.filterValue = txtSearchInv.Text;
        }

        //if (SearchValue=="Open")
        //{
        //    txtInvDtFrom.Text = "";
        //    txtInvDtTo.Text = "";
        //    objProp_Contracts.StartDate = Convert.ToDateTime("01/01/1900 00:00:00");
        //    objProp_Contracts.EndDate = Convert.ToDateTime("01/01/3000 00:00:00");
        //}

        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();
        string stdate = txtInvDtFrom.Text + " 00:00:00";
        string enddate = txtInvDtTo.Text + " 23:59:59";
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        objProp_Contracts.CustID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        objProp_Contracts.Loc = 0;

        _GetARRevenueCustShowAll.ConnConfig = Session["config"].ToString();
        _GetARRevenueCustShowAll.CustID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        _GetARRevenueCustShowAll.Loc = 0;

        _GetARRevenueCust.ConnConfig = Session["config"].ToString();
        _GetARRevenueCust.CustID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        _GetARRevenueCust.Loc = 0;

        if (Convert.ToString(ViewState["ShowAll"]) == "0" || Convert.ToString(ViewState["ShowAll"]) == "")
        {
            if (ddlSearchInv.SelectedValue != "i.fdate")
            {
                objProp_Contracts.StartDate = Convert.ToDateTime(stdate);
                _GetARRevenueCustShowAll.StartDate = Convert.ToDateTime(stdate);
                _GetARRevenueCust.StartDate = Convert.ToDateTime(stdate);
            }

            objProp_Contracts.EndDate = Convert.ToDateTime(enddate);
            _GetARRevenueCustShowAll.EndDate = Convert.ToDateTime(enddate);
            _GetARRevenueCust.EndDate = Convert.ToDateTime(enddate);
        }
        if (txtInvDtFrom.Text == "" && txtInvDtTo.Text == "")
        {
            if (ddlSearchInv.SelectedValue != "i.fdate")
            {
                objProp_Contracts.StartDate = Convert.ToDateTime("01/01/1900 00:00:00");
                objProp_Contracts.EndDate = Convert.ToDateTime("01/01/3000 00:00:00");

                _GetARRevenueCustShowAll.StartDate = Convert.ToDateTime("01/01/1900 00:00:00");
                _GetARRevenueCustShowAll.EndDate = Convert.ToDateTime("01/01/3000 00:00:00");

                _GetARRevenueCust.StartDate = Convert.ToDateTime("01/01/1900 00:00:00");
                _GetARRevenueCust.EndDate = Convert.ToDateTime("01/01/3000 00:00:00");
            }



        }
        objProp_Contracts.SearchBy = SearchBy;
        objProp_Contracts.SearchValue = SearchValue;

        _GetARRevenueCustShowAll.SearchBy = SearchBy;
        _GetARRevenueCustShowAll.SearchValue = SearchValue;

        _GetARRevenueCust.SearchBy = SearchBy;
        _GetARRevenueCust.SearchValue = SearchValue;
        try
        {
            if (Convert.ToString(ViewState["ShowAll"]) == "0" || Convert.ToString(ViewState["ShowAll"]) == "")
            {
                ListGetARRevenueCustShowAll _lstGetARRevenueCust = new ListGetARRevenueCustShowAll();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "CustomersAPI/AddCustomer_GetARRevenueCust";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetARRevenueCust);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetARRevenueCust = serializer.Deserialize<ListGetARRevenueCustShowAll>(_APIResponse.ResponseData);

                    ds1 = _lstGetARRevenueCust.lstTable1.ToDataSet();
                    ds2 = _lstGetARRevenueCust.lstTable2.ToDataSet();

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
                    ds = objBL_Invoice.GetARRevenueCust(objProp_Contracts);
                }
            }
            else
            {
                if (SearchValue == "Open")
                {
                    ListGetARRevenueCustShowAll _lstGetARRevenueCust = new ListGetARRevenueCustShowAll();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "CustomersAPI/AddCustomer_GetARRevenueCust";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetARRevenueCust);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        serializer.MaxJsonLength = Int32.MaxValue;

                        _lstGetARRevenueCust = serializer.Deserialize<ListGetARRevenueCustShowAll>(_APIResponse.ResponseData);

                        ds1 = _lstGetARRevenueCust.lstTable1.ToDataSet();
                        ds2 = _lstGetARRevenueCust.lstTable2.ToDataSet();

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
                        ds = objBL_Invoice.GetARRevenueCust(objProp_Contracts);
                    }
                }
                else
                {

                    ListGetARRevenueCustShowAll _lstGetARRevenueCustShowAll = new ListGetARRevenueCustShowAll();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "CustomersAPI/AddCustomer_GetARRevenueCustShowAll";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetARRevenueCustShowAll);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        serializer.MaxJsonLength = Int32.MaxValue;

                        _lstGetARRevenueCustShowAll = serializer.Deserialize<ListGetARRevenueCustShowAll>(_APIResponse.ResponseData);

                        ds1 = _lstGetARRevenueCustShowAll.lstTable1.ToDataSet();
                        ds2 = _lstGetARRevenueCustShowAll.lstTable2.ToDataSet();

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
                        ds = objBL_Invoice.GetARRevenueCustShowAll(objProp_Contracts);
                    }
                }

            }


            PrevRuntotal = Convert.ToDouble(ds.Tables[1].Rows[0][0].ToString());
            BindInvoiceGridDatatable(ds.Tables[0]);
            //lblCustomerBalance.Text = String.Format("{0:C}", ds.Tables[0].Compute("Sum(Amount)", string.Empty)); 
            // lblTotalRunBalance.Text = String.Format("{0:C}", ds.Tables[0].Compute("Sum(Amount)", string.Empty));
        }
        catch { }
    }
    protected void lnkEditInvoice_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_Invoices.Items)
        {
            CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
            Label lblID = (Label)di.FindControl("lblId");
            Label lblType = (Label)di.FindControl("lblType");
            HiddenField hdnLinkTo = (HiddenField)di.FindControl("hdnLinkTo");
            if (chkSelected.Checked == true)
            {
                //if (lblType.Text.Equals("AR Invoice"))
                //{
                //    Response.Redirect("addinvoice.aspx?uid=" + lblID.Text + "&page=addcustomer&lid=" + Request.QueryString["uid"].ToString());
                //}
                //else
                //{
                //    Response.Redirect("addreceivepayment.aspx?id=" + lblID.Text + "&page=addcustomer&lid=" + Request.QueryString["uid"].ToString());
                //}

                if (Convert.ToInt32(hdnLinkTo.Value) == 1)
                {
                    Response.Redirect("addinvoice.aspx?uid=" + lblID.Text + "&page=addcustomer&lid=" + Request.QueryString["uid"].ToString());
                }
                else if (Convert.ToInt32(hdnLinkTo.Value) == 3)
                {
                    Response.Redirect("adddeposit.aspx?id=" + lblID.Text + "&page=addcustomer&lid=" + Request.QueryString["uid"].ToString());

                }
                else
                {
                    Response.Redirect("addreceivepayment.aspx?id=" + lblID.Text + "&page=addcustomer&lid=" + Request.QueryString["uid"].ToString());
                }
            }
        }
    }
    protected void lnkCopyInvoice_Click(object sender, EventArgs e)
    {

    }
    //protected void lnkDeleteInvoice_Click(object sender, EventArgs e)
    //{

    //}
    protected void lnkAddInvoice_Click(object sender, EventArgs e)
    {
        Response.Redirect("addinvoice.aspx?page=addcustomer&lid=" + Request.QueryString["uid"].ToString());
    }

    protected void lnkShowAll_Click(object sender, EventArgs e)
    {

        ddlSearchInv.SelectedIndex = 0;
        txtSearchInv.Text = string.Empty;
        ddlStatusInv.SelectedIndex = 0;
        ddlDepartment.SelectedIndex = 0;
        txtInvDt.Text = string.Empty;
        txtInvDt.Style.Add("display", "none");
        txtSearchInv.Style.Add("display", "block");
        ddllocation.SelectedIndex = 0;
        ddllocation.Style.Add("display", "none");
        txtInvDtTo.Text = string.Empty;
        txtInvDtFrom.Text = string.Empty;
        ViewState["ShowAll"] = "1";
        ishowAllInvoice.Value = "1";
        //GetInvoices("All", "All");
        //RadGrid_Invoices.Rebind();
        cleanFilter();

    }
    protected void gvOpenCalls_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
            //Label lblComp = (Label)e.Row.FindControl("lblComp");
            //Label lblTicketId = (Label)e.Row.FindControl("lblTicketId");

            ////iframeCustomer.Attributes["src"] = "addticket.aspx?id=" + lblTicketId.Text + "&comp=" + lblComp.Text;            

            //e.Row.Attributes["onclick"] = "SelectRowChk('" + e.Row.ClientID + "','" + chkSelect.ClientID + "','" + gvOpenCalls.ClientID + "');";
            //e.Row.Attributes["ondblclick"] = "showModalPopupViaClientCust(" + lblTicketId.Text + "," + lblComp.Text + ");";
        }
    }
    //private void RowSelect()
    //{
    //    foreach (GridViewRow gr in gvOpenCalls.Rows)
    //    {
    //        CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
    //        Label lblComp = (Label)gr.FindControl("lblComp");
    //        Label lblTicketId = (Label)gr.FindControl("lblTicketId");

    //        gr.Attributes["onclick"] = "SelectRowChk('" + gr.ClientID + "','" + chkSelect.ClientID + "','" + gvOpenCalls.ClientID + "',event);";
    //        gr.Attributes["ondblclick"] = "showModalPopupViaClientCust(" + lblTicketId.Text + "," + lblComp.Text + ");";
    //    }

    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertScript", "SelectedRowStyle('" + gvOpenCalls.ClientID + "');", true);
    //} ac

    private void BindInvoiceGridDatatable(DataTable dt)
    {

        string selectedSearchValue = "";
        if (Request.Form["radio-group"] != null)
        {
            selectedSearchValue = Request.Form["radio-group"].ToString();
        }
        Session["InvoiceSrchCust"] = dt;
        if (!dt.Columns.Contains("RunTotal"))
        {
            dt.Columns.Add("RunTotal", typeof(Double));
        }

        dt.Columns.Add("IDD", typeof(string));
        dt.Columns.Add("CustomerName", typeof(string));
        Double Runtotal = 0.00;
        Runtotal = PrevRuntotal;
        if (selectedSearchValue == "rdOpen")
        {
            int i = 0;
            Runtotal = 0;
            //to convert deposit, recievepayment  to -ve values 
            foreach (DataRow row in dt.Rows)
            {
                if (i == 0)
                {
                    row["RunTotal"] = Convert.ToDouble(row["Balance"].ToString());

                }
                else
                {
                    //if (Convert.ToDouble(row["Credits"]) != 0 && Convert.ToDouble(row["Balance"]) != 0)
                    //{
                    //    row["RunTotal"] = Convert.ToDouble(row["amount"].ToString()) + Convert.ToDouble(row["Balance"].ToString()) + Runtotal;

                    //}
                    //else
                    //{
                    //    row["RunTotal"] = Convert.ToDouble(row["amount"].ToString()) - Convert.ToDouble(row["Credits"].ToString()) + Runtotal;


                    //}
                    row["RunTotal"] = Convert.ToDouble(row["Balance"].ToString()) + Runtotal;
                }
                Runtotal = Convert.ToDouble(row["RunTotal"].ToString());

                row["CustomerName"] = Convert.ToString(lblCustomerName.Text);
                i++;

            }
        }
        else
        {
            //if (txtInvDtFrom.Text == "")
            //{
            //to convert deposit, recievepayment  to -ve values 
            foreach (DataRow row in dt.Rows)
            {

                row["RunTotal"] = Convert.ToDouble(row["amount"].ToString()) - Convert.ToDouble(row["Credits"].ToString()) + Runtotal;
                Runtotal = Convert.ToDouble(row["RunTotal"].ToString());

                row["CustomerName"] = Convert.ToString(lblCustomerName.Text);
            }
            //}
            //    else
            //    {
            //        int i = 0;
            //        Runtotal = 0;
            //        //to convert deposit, recievepayment  to -ve values 
            //        foreach (DataRow row in dt.Rows)
            //        {
            //            if (i == 0)
            //            {
            //                row["RunTotal"] = Convert.ToDouble(row["Balance"].ToString());

            //            }
            //            else
            //            {
            //                if (Convert.ToDouble(row["Credits"]) != 0 && Convert.ToDouble(row["Balance"]) != 0)
            //                {
            //                    row["RunTotal"] = Convert.ToDouble(row["amount"].ToString()) + Convert.ToDouble(row["Balance"].ToString()) + Runtotal;

            //                }
            //                else
            //                {
            //                    row["RunTotal"] = Convert.ToDouble(row["amount"].ToString()) - Convert.ToDouble(row["Credits"].ToString()) + Runtotal;


            //                }
            //            }
            //            Runtotal = Convert.ToDouble(row["RunTotal"].ToString());

            //            row["CustomerName"] = Convert.ToString(lblCustomerName.Text);
            //            i++;

            //        }
            //    }

        }

        RadGrid_Invoices.VirtualItemCount = dt.Rows.Count;
        RadGrid_Invoices.DataSource = dt;

        lblRecordCount.Text = dt.Rows.Count.ToString() + " Record(s) Found.";

    }

    #endregion

    private void FillProspect()
    {
        if (Request.QueryString["cpw"] != null)
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.ProspectID = Convert.ToInt32(Request.QueryString["prospectid"].ToString());

            _GetProspectByID.ConnConfig = Session["config"].ToString();
            _GetProspectByID.ProspectID = Convert.ToInt32(Request.QueryString["prospectid"].ToString());

            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            DataSet ds3 = new DataSet();

            ListGetProspectByID _lstGetProspectByID = new ListGetProspectByID();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "CustomersAPI/AddCustomer_GetProspectByID";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetProspectByID);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetProspectByID = serializer.Deserialize<ListGetProspectByID>(_APIResponse.ResponseData);

                ds1 = _lstGetProspectByID.lstTable1.ToDataSet();
                ds2 = _lstGetProspectByID.lstTable2.ToDataSet();
                ds3 = _lstGetProspectByID.lstTable3.ToDataSet();

                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();

                dt1 = ds1.Tables[0];
                dt2 = ds2.Tables[0];
                dt3 = ds3.Tables[0];

                dt1.TableName = "Table1";
                dt2.TableName = "Table2";
                dt3.TableName = "Table3";
                ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy() });
            }
            else
            {
                ds = objBL_Customer.getProspectByID(objProp_Customer);
            }
           
            if (ds.Tables[0].Rows.Count > 0)
            {
                FileUpload1.Visible = false;
                lnkDeleteDoc.Visible = false;
                lnkAddnew.Visible = false;
                btnDelete.Visible = false;
                btnEdit.Visible = false;
                //pnlDoc.Visible = true;
                btnSubmit.Text = "Next";
                lnkClose.Visible = false;

                //txtCName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                //lblCustomerName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                txtCName.Text = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                lblCustomerName.Text = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                txtGoogleAutoc.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                txtCity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                // ddlState.SelectedValue = ds.Tables[0].Rows[0]["State"].ToString();
                txtState.Text = ds.Tables[0].Rows[0]["State"].ToString();
                //ddlCountry.SelectedValue = ds.Tables[0].Rows[0]["Country"].ToString();
                var item = ddlCountry.Items.FindByText(ds.Tables[0].Rows[0]["Country"].ToString());
                if (item != null)
                {
                    if (ddlCountry.SelectedItem != null) ddlCountry.SelectedItem.Selected = false;
                    item.Selected = true;
                }
                txtZip.Text = ds.Tables[0].Rows[0]["Zip"].ToString();
                txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                txtMaincontact.Text = ds.Tables[0].Rows[0]["contact"].ToString();
                txtPhoneCust.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                txtWebsite.Text = ds.Tables[0].Rows[0]["website"].ToString();
                txtEmail.Text = ds.Tables[0].Rows[0]["email"].ToString();
                txtCell.Text = ds.Tables[0].Rows[0]["cellular"].ToString();
                ddlCustStatus.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
                txtFax.Text = ds.Tables[0].Rows[0]["Fax"].ToString();
                ddlCompany.SelectedValue = ds.Tables[0].Rows[0]["EN"].ToString();
                txtCompany.Text = ds.Tables[0].Rows[0]["Company"].ToString();
                ddlCompanyEdit.SelectedValue = ds.Tables[0].Rows[0]["EN"].ToString();
                //ddlBilling.SelectedValue = ds.Tables[0].Rows[0]["Billing"].ToString();
                lat.Value = ds.Tables[0].Rows[0]["lat"].ToString();
                lng.Value = ds.Tables[0].Rows[0]["lng"].ToString();
                GetDocuments();
                //RadGrid_Documents.Rebind();

                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        //gvContacts.DataSource = ds.Tables[1]; ac
                        //gvContacts.DataBind(); ac

                        ViewState["contacttable"] = ds.Tables[1];
                    }
                }
            }
        }
    }

    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlSearch.SelectedValue == "t.cat")
        //{
        //    ddlCategory.Visible = true;
        //    txtSearch.Visible = false;
        //}
        //else
        //{
        //    ddlCategory.Visible = false;
        //    txtSearch.Visible = true;
        //}
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        ddlSearchInv.SelectedIndex = 0;
        txtSearchInv.Text = string.Empty;
        ddlStatusInv.SelectedIndex = 0;
        ddlDepartment.SelectedIndex = 0;
        txtInvDt.Text = string.Empty;
        txtInvDt.Style.Add("display", "none");
        txtSearchInv.Style.Add("display", "block");
        ddllocation.SelectedIndex = 0;
        ddllocation.Style.Add("display", "none");
        if (Convert.ToString(ViewState["ShowAll"]) == "1")
        {
            if (Session["InvToDate"] == null)
            {
                txtInvDtTo.Text = DateTime.Now.AddMonths(1).Date.ToShortDateString();
            }
            else
            {
                txtInvDtTo.Text = Session["InvToDate"].ToString();
            }
            if (Session["InvFromDate"] == null)
            {
                //txtInvDtFrom.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                txtInvDtFrom.Text = DateTime.Now.AddMonths(-1).Date.ToShortDateString();
            }
            else
            {
                txtInvDtFrom.Text = Session["InvFromDate"].ToString();
            }
            ishowAllInvoice.Value = "0";
            ViewState["ShowAll"] = "0";
        }
        else
        {
            if (txtInvDtTo.Text == "" && txtInvDtFrom.Text == "")
            {
                if (Session["InvToDate"] == null)
                {
                    txtInvDtTo.Text = DateTime.Now.AddMonths(1).Date.ToShortDateString();
                }
                else
                {
                    txtInvDtTo.Text = Session["InvToDate"].ToString();
                }
                if (Session["InvFromDate"] == null)
                {
                    //txtInvDtFrom.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                    txtInvDtFrom.Text = DateTime.Now.AddMonths(-1).Date.ToShortDateString();
                }
                else
                {
                    txtInvDtFrom.Text = Session["InvFromDate"].ToString();
                }
            }
        }
        GetData();
        cleanFilter();
    }
    public void cleanFilter()
    {
        foreach (GridColumn column in RadGrid_Invoices.MasterTableView.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        RadGrid_Invoices.MasterTableView.FilterExpression = string.Empty;
        RadGrid_Invoices.MasterTableView.Rebind();
        RadGrid_Invoices.Rebind();
    }
    protected void ddlSearchInv_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlSearchInv.SelectedValue == "i.Type")
        //{
        //    ddlStatusInv.Visible = false;
        //    txtSearchInv.Visible = false;
        //    ddlDepartment.Visible = true;
        //    txtInvDt.Visible = false;
        //    ddllocation.Visible = false;
        //}
        //else if (ddlSearchInv.SelectedValue == "i.Status")
        //{
        //    ddlStatusInv.Visible = true;
        //    txtSearchInv.Visible = false;
        //    ddlDepartment.Visible = false;
        //    txtInvDt.Visible = false;
        //    ddllocation.Visible = false;
        //}
        //else if (ddlSearchInv.SelectedValue == "i.fdate")
        //{
        //    ddlStatusInv.Visible = false;
        //    txtSearchInv.Visible = false;
        //    ddlDepartment.Visible = false;
        //    txtInvDt.Style["display"] = "block";
        //    ddllocation.Visible = false;
        //}
        //else if (ddlSearchInv.SelectedValue == "l.loc")
        //{
        //    ddlStatusInv.Visible = false;
        //    txtSearchInv.Visible = false;
        //    ddlDepartment.Visible = false;
        //    txtInvDt.Visible = false;
        //    ddllocation.Visible = true;
        //}
        //else
        //{
        //    ddlStatusInv.Visible = false;
        //    txtSearchInv.Visible = true;
        //    ddlDepartment.Visible = false;
        //    txtInvDt.Visible = false;
        //    ddllocation.Visible = false;
        //}
    }

    private void FillDepartment()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();

        _GetDepartment.ConnConfig = Session["config"].ToString();

        List<JobTypeViewModel> _lstJobType = new List<JobTypeViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "CustomersAPI/AddCustomer_GetDepartment";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetDepartment);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstJobType = serializer.Deserialize<List<JobTypeViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<JobTypeViewModel>(_lstJobType);
        }
        else
        {
            ds = objBL_User.getDepartment(objPropUser);
        }

        ddlDepartment.DataSource = ds.Tables[0];
        ddlDepartment.DataTextField = "type";
        ddlDepartment.DataValueField = "id";
        ddlDepartment.DataBind();

        ddlDepartment.Items.Insert(0, new ListItem("Select", ""));
    }

    private void Locations()
    {
        DataSet ds = new DataSet();
        //objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.CustomerID = Convert.ToInt32(Request.QueryString["uid"].ToString());

        _GetLocationByCustomerID.ConnConfig = Session["config"].ToString();
        _GetLocationByCustomerID.CustomerID = Convert.ToInt32(Request.QueryString["uid"].ToString());

        List<GetLocationByCustomerIDViewModel> _lstGetLocationByCustomerID = new List<GetLocationByCustomerIDViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "CustomersAPI/AddCustomer_GetLocationByCustomerID";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetLocationByCustomerID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetLocationByCustomerID = serializer.Deserialize<List<GetLocationByCustomerIDViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetLocationByCustomerIDViewModel>(_lstGetLocationByCustomerID);
        }
        else
        {
            ds = objBL_User.getLocationByCustomerID(objPropUser, new GeneralFunctions().GetSalesAsigned());
        }


        ddllocation.DataSource = ds.Tables[0];
        ddllocation.DataTextField = "tag";
        ddllocation.DataValueField = "loc";
        ddllocation.DataBind();

        //ddllocation.Items.Insert(0, new ListItem("All", "0"));
    }

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
                objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                objMapData.TempId = "0";
                foreach (HttpPostedFile postedFile in FileUpload1.PostedFiles)
                {

                    string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();

                    string savepath = savepathconfig + @"\" + Session["dbname"] + @"\CustDocs\Id_" + Request.QueryString["uid"].ToString() + @"\";
                   // string savepath = savepathconfig + @"\" + Session["dbname"] + @"\ld_" + Request.QueryString["uid"].ToString() + @"\";
                    // string fileExtensionApplication = System.IO.Path.GetExtension(savepathconfig);
                    //filename = System.IO.Path.GetFileName(FileUpload1.Value);
                    filename = postedFile.FileName;
                   // filename = filename.Replace(",", "");
                    fullpath = savepath + filename;
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

                    objMapData.Screen = "Customer";
                   
                    objMapData.FileName = filename;
                    objMapData.DocTypeMIME = MIME;
                    objMapData.FilePath = fullpath;
                    objMapData.DocID = 0;
                    objMapData.Mode = 0;
                    objMapData.ConnConfig = Session["config"].ToString();
                    
                    objMapData.Worker = Session["User"].ToString();
                    objBL_MapData.AddFile(objMapData);

                    //API
                    //_AddFile.Screen = "Customer";
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
                    //    string APINAME = "CustomersAPI/AddCustomer_AddFile";

                    //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddFile);
                    //}
                    //else
                    //{
                    //    objBL_MapData.AddFile(objMapData);
                    //}



                    //_UpdateDocInfo.ConnConfig = Session["config"].ToString();
                    //DataTable viewstatedata = SaveDocInfo();

                    //if (viewstatedata.Rows.Count == 0)
                    //{
                    //    DataTable returnVal = EmptyDatatable();
                    //    _UpdateDocInfo.dtDocs = returnVal;
                    //}
                    //else
                    //{
                    //    _UpdateDocInfo.dtDocs = SaveDocInfo();
                    //}

                    //if (IsAPIIntegrationEnable == "YES")
                    //{
                    //    string APINAME = "CustomersAPI/AddCustomer_UpdateDocInfo";

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
            //string extension = Path.GetExtension(FileUpload1.Name);
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
            objMapData.Screen = "Customer";
            _GetDocuments.Screen = "Customer";

            if (Request.QueryString["uid"] != null)
            {
                objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                _GetDocuments.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            }
        }

        objMapData.TempId = "0";
        _GetDocuments.TempId = "0";


        objMapData.ConnConfig = Session["config"].ToString();
        _GetDocuments.ConnConfig = Session["config"].ToString();

        DataSet ds = new DataSet();

        List<GetDocumentsViewModel> _lstGetDocuments = new List<GetDocumentsViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "CustomersAPI/AddCustomer_GetDocuments";

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

    protected void lblName_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;

        string[] CommandArgument = btn.CommandArgument.Replace(btn.Text, " ").Split(',');

        string FileName = btn.Text;
        string FilePath = CommandArgument[1].Trim() + btn.Text.Trim();

        DownloadDocument(FilePath, FileName);
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
        foreach (GridDataItem item in RadGrid_Documents.MasterTableView.Items)
        {
            CheckBox chkSelected = (CheckBox)item["chkSelect"].Controls[0];
            Label lblID = (Label)item.FindControl("lblId");
            if (chkSelected.Checked == true)
            {
                DeleteFileFromFolder(string.Empty, Convert.ToInt32(lblID.Text));
            }
        }

        RadGrid_Documents.Rebind();
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
                string APINAME = "CustomersAPI/AddCustomer_DeleteFile";

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
                string APINAME = "CustomersAPI/AddCustomer_UpdateDocInfo";

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

    protected void btnSageID_Click(object sender, EventArgs e)
    {
        int i = 1;
        string str = "Customer ID Already Exists in Sage";
        if (txtAcctno.Text.Trim() != string.Empty)
        {
            if (ViewState["mode"].ToString() == "1")
            {
                if (hdnAcctID.Value.Trim() == txtAcctno.Text.Trim())
                {
                    return;
                }
            }
            try
            {
                i = CheckSageID(txtAcctno.Text.Trim());
                if (i == 0)
                {
                    str = "Account # Available!";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysage", "noty({text: '" + str + "',  type : 'information', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default', dismissQueue: true,   closable : true});", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysage", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default', dismissQueue: true,   closable : true});", true);
                }
            }
            catch (Exception ex)
            {
                string strex = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrdelete", "noty({text: '" + strex + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',dismissQueue: true,    closable : true});", true);
            }
        }
    }

    private int CheckSageID(string job)
    {
        string DSN = System.Web.Configuration.WebConfigurationManager.AppSettings["SageDSN"].Trim();
        string query = "Select customer from master_arm_customer where customer = ?";
        OdbcConnection odbccon = new OdbcConnection(DSN);
        if (odbccon.State != ConnectionState.Open)
        {
            odbccon.Open();
        }
        System.Data.Odbc.OdbcDataAdapter da = new System.Data.Odbc.OdbcDataAdapter(query, odbccon);
        da.SelectCommand.Parameters.AddWithValue("@customer", job);
        DataTable dt = new DataTable();
        da.Fill(dt);
        odbccon.Close();
        int count = dt.Rows.Count;
        return count;
    }

    private int SageAlert()
    {
        objGeneral.ConnConfig = Session["config"].ToString();

        _getConnectionConfig.ConnConfig = Session["config"].ToString();

        DataSet dsLastSync = new DataSet();

        List<GeneralViewModel> _lstGeneral = new List<GeneralViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "CustomersAPI/CustomersList_GetSagelatsync";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGeneral = serializer.Deserialize<List<GeneralViewModel>>(_APIResponse.ResponseData);
            dsLastSync = CommonMethods.ToDataSet<GeneralViewModel>(_lstGeneral);
        }
        else
        {
            dsLastSync = objBL_General.getSagelatsync(objGeneral);
        }

        int intintegration = Convert.ToInt32(dsLastSync.Tables[0].Rows[0]["sageintegration"]);
        int count = 0;
        if (intintegration == 1)
        {
            int i = 1;
            string str = "Customer ID Already Exists in Sage..";
            try
            {
                if (txtAcctno.Text.Trim() != string.Empty)
                    i = CheckSageID(txtAcctno.Text.Trim());

                if (i != 0)
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysage", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',dismissQueue: true,  closable : true});", true);
                    count = 1;
                }
            }
            catch (Exception ex)
            {
                string strex = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrdelete", "noty({text: '" + strex + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default', dismissQueue: true, closable : true});", true);
                count = 1;
            }
        }
        return count;
    }

    public string ShowHoverText(object desc, object reason)
    {
        string result = string.Empty;
        result = "<B>Reason</B>: " + Convert.ToString(reason).Replace("\n", "<br/>");

        if (!string.IsNullOrEmpty(Convert.ToString(desc)))
            result += "<br/><br/><B>Resolution</B>: " + Convert.ToString(desc).Replace("\n", "<br/>");

        return result;
    }

    protected void gvOpenCalls_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            AjaxControlToolkit.HoverMenuExtender ajxHover = (AjaxControlToolkit.HoverMenuExtender)e.Row.FindControl("hmeRes");
            e.Row.ID = e.Row.RowIndex.ToString();
            ajxHover.TargetControlID = e.Row.ID;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("aragingreport.aspx?uid=" + Request.QueryString["uid"]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void txtInvDtFrom_TextChanged(object sender, EventArgs e)
    {
        Session["InvfromDate"] = txtInvDtFrom.Text;
        //if (Request.QueryString["uid"] != null)
        //{
        //    GetData();
        //}

    }

    protected void txtInvDtTo_TextChanged(object sender, EventArgs e)
    {
        Session["InvToDate"] = txtInvDtTo.Text;
        //if (Request.QueryString["uid"] != null)
        //{
        //    GetData();
        //}

    }

    private void GetData()
    {
        string selectedSearchValue = Request.Form["radio-group"].ToString();
        hdnSearchValue.Value = Request.Form["radio-group"].ToString();
        string selectedSearchBy = Request.Form["radio-group1"].ToString();
        hdnSearchBy.Value = Request.Form["radio-group1"].ToString();
        string SearchValue = string.Empty;
        string SearchBy = string.Empty;
        if (selectedSearchValue == "rdAll")
        {
            SearchValue = "All";
        }
        else if (selectedSearchValue == "rdOpen")
        {
            SearchValue = "Open";
        }
        else
        {
            SearchValue = "Closed";
        }



        if (selectedSearchBy == "rdAll2")
        {
            SearchBy = "All";
        }
        else if (selectedSearchBy == "rdCharges")
        {
            SearchBy = "Charges";
        }
        else
        {
            SearchBy = "Credits";
        }

        GetInvoices(SearchValue, SearchBy);

    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        ViewState["ShowAll"] = "0";
        GetData();
        RadGrid_Invoices.Rebind();
    }

    private void PagePermission()
    {

        if (Convert.ToString(Session["type"]) != "am" && Convert.ToString(Session["type"]) != "c")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];

            //Location------------------->
            string LocationPermission = ds.Rows[0]["Location"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Location"].ToString();
            hdnAddeLoc.Value = LocationPermission.Length < 1 ? "Y" : LocationPermission.Substring(0, 1);
            hdnEditeLoc.Value = LocationPermission.Length < 2 ? "Y" : LocationPermission.Substring(1, 1);
            hdnDeleteLoc.Value = LocationPermission.Length < 3 ? "Y" : LocationPermission.Substring(2, 1);
            hdnViewLoc.Value = LocationPermission.Length < 4 ? "Y" : LocationPermission.Substring(3, 1);

            //Equipment------------------->
            string EquipmentPermission = ds.Rows[0]["Elevator"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Elevator"].ToString();
            hdnAddeEquipment.Value = EquipmentPermission.Length < 1 ? "Y" : EquipmentPermission.Substring(0, 1);
            hdnEditeEquipment.Value = EquipmentPermission.Length < 2 ? "Y" : EquipmentPermission.Substring(1, 1);
            hdnDeleteEquipment.Value = EquipmentPermission.Length < 3 ? "Y" : EquipmentPermission.Substring(2, 1);
            hdnViewEquipment.Value = EquipmentPermission.Length < 4 ? "Y" : EquipmentPermission.Substring(3, 1);

            //Ticket---------------------->
            string ticketPermission = ds.Rows[0]["TicketPermission"] == DBNull.Value ? "YYNYYY" : ds.Rows[0]["TicketPermission"].ToString();
            hdnEditeTicket.Value = ticketPermission.Length < 2 ? "Y" : ticketPermission.Substring(1, 1);
            hdnViewTicket.Value = ticketPermission.Length < 4 ? "Y" : ticketPermission.Substring(3, 1);

            //Contact---------------------->
            string ContactPermission = ds.Rows[0]["ContactPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["ContactPermission"].ToString();
            hdnAddeContact.Value = ContactPermission.Length < 1 ? "Y" : ContactPermission.Substring(0, 1);
            hdnEditeContact.Value = ContactPermission.Length < 2 ? "Y" : ContactPermission.Substring(1, 1);
            hdnDeleteContact.Value = ContactPermission.Length < 3 ? "Y" : ContactPermission.Substring(2, 1);
            hdnViewContact.Value = ContactPermission.Length < 4 ? "Y" : ContactPermission.Substring(3, 1);

            if (hdnAddeContact.Value == "N")
            {
                lnkAddnew.Enabled = false;
            }
            if (hdnEditeContact.Value == "N")
            {
                btnEdit.Enabled = false;
            }
            if (hdnDeleteContact.Value == "N")
            {
                btnDelete.Enabled = false;
            }
            //accrdcontacts.Visible = hdnViewContact.Value == "N" ? false : true;
            //pnlgvConPermission.Visible = hdnViewContact.Value == "N" ? false : true;
            pnlgvConPermission.Visible = hdnViewContact.Value == "N" ? false : true;
            //Document--------------------->

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
            pnlDocPermission.Visible = hdnViewDocument.Value == "N" ? false : true;

            // Job
            string JobPermission = ds.Rows[0]["Job"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Job"].ToString();

            hdnAddeJob.Value = JobPermission.Length < 1 ? "Y" : JobPermission.Substring(0, 1);

            // Opportunity Permission
            string ProposalPermission = ds.Rows[0]["Proposal"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Proposal"].ToString();
            string ADDOpp = ProposalPermission.Length < 1 ? "Y" : ProposalPermission.Substring(0, 1);
            string EditOpp = ProposalPermission.Length < 2 ? "Y" : ProposalPermission.Substring(1, 1);
            string DeleteOpp = ProposalPermission.Length < 2 ? "Y" : ProposalPermission.Substring(2, 1);
            string ViewOpp = ProposalPermission.Length < 4 ? "Y" : ProposalPermission.Substring(3, 1);
            string ReportOpp = ProposalPermission.Length < 6 ? "Y" : ProposalPermission.Substring(5, 1);

            if (ADDOpp == "N")
            {
                lnkAddopp.Visible = false;
                lnkCopyOpp.Visible = false;
            }
            if (EditOpp == "N")
                lnkEditOpp.Visible = false;
            if (DeleteOpp == "N")
                lnkDeleteOpp.Visible = false;
            if (ReportOpp == "N")
                lnkExcelOpp.Visible = false;
            if (ViewOpp == "N")
                RadGrid_Opportunity.Visible = false;
        }

    }

    #region Contacts
    protected void RadGrid_gvContacts_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid_gvContacts.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
            if (Request.QueryString["uid"] != null)
            {
                objPropUser.CustomerID = Convert.ToInt32(Request.QueryString["uid"]);
                objPropUser.DBName = Session["dbname"].ToString();
                objPropUser.ConnConfig = Session["config"].ToString();

                _GetCustomerByID.CustomerID = Convert.ToInt32(Request.QueryString["uid"]);
                _GetCustomerByID.DBName = Session["dbname"].ToString();
                _GetCustomerByID.ConnConfig = Session["config"].ToString();

                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet ds3 = new DataSet();

                ListGetCustomerByID _listGetCustomerByID = new ListGetCustomerByID();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "CustomersAPI/AddCustomer_GetCustomerByID";

                    _GetCustomerByID.IsSalesAsigned = new GeneralFunctions().GetSalesAsigned();

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomerByID);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _listGetCustomerByID = serializer.Deserialize<ListGetCustomerByID>(_APIResponse.ResponseData);

                    ds1 = _listGetCustomerByID.lstTable1.ToDataSet();
                    ds2 = _listGetCustomerByID.lstTable2.ToDataSet();
                    ds3 = _listGetCustomerByID.lstTable3.ToDataSet();

                    DataTable dt1 = new DataTable();
                    DataTable dt2 = new DataTable();
                    DataTable dt3 = new DataTable();

                    dt1 = ds1.Tables[0];
                    dt2 = ds2.Tables[0];
                    dt3 = ds3.Tables[0];

                    dt1.TableName = "Table1";
                    dt2.TableName = "Table2";
                    dt3.TableName = "Table3";
                    ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy() });
                }
                else
                {
                    ds = objBL_User.getCustomerByID(objPropUser, new GeneralFunctions().GetSalesAsigned());
                }

                RadGrid_gvContacts.VirtualItemCount = ds.Tables[1].Rows.Count;
                RadGrid_gvContacts.DataSource = ds.Tables[1];
            }
            else
            {
                RadGrid_gvContacts.DataSource = string.Empty;
            }
        }
        catch { }
    }
    protected void RadGrid_gvContacts_PreRender(object sender, EventArgs e)
    {
        RowSelect();
    }
    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_gvContacts.MasterTableView.FilterExpression != "" ||
            (RadGrid_gvContacts.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_gvContacts.MasterTableView.SortExpressions.Count > 0;
    }
    private void RowSelect()
    {
        foreach (GridDataItem item in RadGrid_gvContacts.Items)
        {

            HiddenField hdnSelected = (HiddenField)item.FindControl("hdnSelected");
            Label lblMail = (Label)item.FindControl("lblEmail");
            CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
            CheckBox chkShutdown = (CheckBox)item.FindControl("chkShutdown");
            if (hdnEditeContact.Value == "Y")
            {
                item.Attributes["ondblclick"] = "clickEdit('" + hdnSelected.ClientID + "','" + chkSelect.ClientID + "','" + btnEdit.ClientID + "');";
                item.Attributes["onclick"] = "SelectRowmail('" + hdnSelected.ClientID + "','" + item.ClientID + "','" + lblMail.ClientID + "','" + chkSelect.ClientID + "','" + item.ClientID + "','" + lnkMail.ClientID + "');";
            }
            else
            {
                chkSelect.Enabled = chkShutdown.Enabled = false;
                item.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";
            }

        }
    }
    #endregion

    #region Location
    protected void RadGrid_Location_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid_Location.AllowCustomPaging = !ShouldApplySortFilterOrGroupLoc();
            if (Request.QueryString["uid"] != null)
            {
                objPropUser.CustomerID = Convert.ToInt32(Request.QueryString["uid"]);
                objPropUser.DBName = Session["dbname"].ToString();
                objPropUser.ConnConfig = Session["config"].ToString();

                _GetCustomerByID.CustomerID = Convert.ToInt32(Request.QueryString["uid"]);
                _GetCustomerByID.DBName = Session["dbname"].ToString();
                _GetCustomerByID.ConnConfig = Session["config"].ToString();

                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();
                DataSet ds3 = new DataSet();

                ListGetCustomerByID _listGetCustomerByID = new ListGetCustomerByID();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "CustomersAPI/AddCustomer_GetCustomerByID";

                    _GetCustomerByID.IsSalesAsigned = new GeneralFunctions().GetSalesAsigned();

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomerByID);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _listGetCustomerByID = serializer.Deserialize<ListGetCustomerByID>(_APIResponse.ResponseData);

                    ds1 = _listGetCustomerByID.lstTable1.ToDataSet();
                    ds2 = _listGetCustomerByID.lstTable2.ToDataSet();
                    ds3 = _listGetCustomerByID.lstTable3.ToDataSet();

                    DataTable dt1 = new DataTable();
                    DataTable dt2 = new DataTable();
                    DataTable dt3 = new DataTable();

                    dt1 = ds1.Tables[0];
                    dt2 = ds2.Tables[0];
                    dt3 = ds3.Tables[0];

                    dt1.TableName = "Table1";
                    dt2.TableName = "Table2";
                    dt3.TableName = "Table3";
                    ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy() });
                }
                else
                {
                    ds = objBL_User.getCustomerByID(objPropUser, new GeneralFunctions().GetSalesAsigned());
                }

                RadGrid_Location.VirtualItemCount = ds.Tables[2].Rows.Count;
                RadGrid_Location.DataSource = ds.Tables[2];
                //RadPersistenceManager1.SaveState();
            }

        }
        catch { }
    }
    protected void RadGrid_Location_PreRender(object sender, EventArgs e)
    {
        RowSelectLoc();
    }

    bool isGroupingLocation = false;
    public bool ShouldApplySortFilterOrGroupLoc()
    {
        return RadGrid_Location.MasterTableView.FilterExpression != "" ||
            (RadGrid_Location.MasterTableView.GroupByExpressions.Count > 0 || isGroupingLocation) ||
            RadGrid_Location.MasterTableView.SortExpressions.Count > 0;
    }
    private void RowSelectLoc()
    {
        foreach (GridDataItem item in RadGrid_Location.Items)
        {

            Label lblID = (Label)item.FindControl("lblloc");
            HiddenField hdnSelected = (HiddenField)item.FindControl("hdnSelected");
            CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");

            item.Attributes["ondblclick"] = "location.href='addlocation.aspx?uid=" + lblID.Text + "&page=addcustomer&lid=" + Request.QueryString["uid"].ToString() + "'";
            //gr.Attributes["ondblclick"] = "document.getElementById('"+ lnkEditLoc.ClientID +"').click()";
            // item.Attributes["onclick"] = "SelectRow('" + hdnSelected.ClientID + "','" + item.ClientID + "','" + chkSelect.ClientID + "','" + item.ClientID + "',event);";


        }
    }
    #endregion

    #region Equipment
    protected void RadGrid_Equip_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid_Equip.AllowCustomPaging = !ShouldApplySortFilterOrGroupEquip();

            if ((Request.QueryString["uid"]) != null)
            {
                GetDataEquip();
            }
        }
        catch { }
    }

    protected void RadGrid_Equip_ItemEvent(object sender, GridItemEventArgs e)
    {
        if (RadGrid_Equip.Items.Count > 0)
        {
            GridFooterItem footerItem = (GridFooterItem)RadGrid_Equip.MasterTableView.GetItems(GridItemType.Footer).FirstOrDefault();
            if (footerItem != null)
            {
                var dt = (DataTable)Session["ElevSrchCust"];
                var totalActive = dt.Select("Status = 0").Count();

                Label lblTotalActive = footerItem.FindControl("lblTotalActive") as Label;
                lblTotalActive.Text = string.Format("Active Count: {0:N0}", totalActive);
            }
        }
    }

    protected void RadGrid_Equip_PreRender(object sender, EventArgs e)
    {
        RowSelectEquip();
    }
    bool isGroupingEquip = false;
    public bool ShouldApplySortFilterOrGroupEquip()
    {
        return RadGrid_Equip.MasterTableView.FilterExpression != "" ||
            (RadGrid_Equip.MasterTableView.GroupByExpressions.Count > 0 || isGroupingEquip) ||
            RadGrid_Equip.MasterTableView.SortExpressions.Count > 0;
    }
    private void RowSelectEquip()
    {
        foreach (GridDataItem item in RadGrid_Equip.Items)
        {

            Label lblID = (Label)item.FindControl("lblId");
            CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");

            if (hdnEditeEquipment.Value == "Y" || hdnViewEquipment.Value == "Y")
            {
                item.Attributes["ondblclick"] = "location.href='addequipment.aspx?uid=" + lblID.Text + "&page=addcustomer&cuid=" + Request.QueryString["uid"].ToString() + "'";
            }
            else
            {
                item.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";
            }

            //item.Attributes["onclick"] = "SelectRowChk('" + item.ClientID + "','" + chkSelect.ClientID + "','" + item.ClientID + "',event);";


        }
    }
    #endregion

    #region OpenCalls
    protected void RadGrid_OpenCalls_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid_OpenCalls.AllowCustomPaging = !ShouldApplySortFilterOrGroupOpenCalls();

            if (!IsPostBack)
            {
                if (Request.QueryString["f"] != null && Request.QueryString["f"] == "r" && Session["TicketListRadGVFilters"] != null)
                {
                    List<RetainFilter> filters = new List<RetainFilter>();

                    RadGrid_OpenCalls.MasterTableView.FilterExpression = Convert.ToString(Session["AddCustomer_TicketListFilter"]);
                    if (Session["TicketListRadGVFilters"] != null)
                    {


                        var filtersGet = Session["TicketListRadGVFilters"] as List<RetainFilter>;
                        if (filtersGet != null)
                        {
                            foreach (var _filter in filtersGet)
                            {
                                GridColumn column = RadGrid_OpenCalls.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                                column.CurrentFilterValue = _filter.FilterValue;
                            }
                        }

                    }

                }

            }




            if ((Request.QueryString["uid"]) != null)
            {
                GetOpenCalls();
                showFilterSearch();
            }
        }
        catch { }

    }
    protected void RadGrid_OpenCalls_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["f"] != null && Request.QueryString["f"] == "r" && Session["TicketListRadGVFilters"] != null)
            {
                List<RetainFilter> filters = new List<RetainFilter>();

                RadGrid_OpenCalls.MasterTableView.FilterExpression = Convert.ToString(Session["AddCustomer_TicketListFilter"]);
                if (Session["TicketListRadGVFilters"] != null)
                {


                    var filtersGet = Session["TicketListRadGVFilters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_OpenCalls.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }

                }
                Session.Remove("TicketListRadGVFilters");
                Session.Remove("AddCustomer_TicketListFilter");
            }

        }
        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_OpenCalls);
        RowSelectOpenCalls();
    }

    bool isGroupingOpenCalls = false;
    public bool ShouldApplySortFilterOrGroupOpenCalls()
    {
        return RadGrid_OpenCalls.MasterTableView.FilterExpression != "" ||
            (RadGrid_OpenCalls.MasterTableView.GroupByExpressions.Count > 0 || isGroupingOpenCalls) ||
            RadGrid_OpenCalls.MasterTableView.SortExpressions.Count > 0;
    }
    private void RowSelectOpenCalls()
    {
        foreach (GridDataItem item in RadGrid_OpenCalls.Items)
        {

            CheckBox chkSelect = (CheckBox)item.FindControl("chkSelect");
            Label lblComp = (Label)item.FindControl("lblComp");
            Label lblTicketId = (Label)item.FindControl("lblTicketId");

            //item.Attributes["onclick"] = "SelectRowChk('" + item.ClientID + "','" + chkSelect.ClientID + "','" + item.ClientID + "',event);";
            //gr.Attributes["ondblclick"] = "showModalPopupViaClientCust(" + lblTicketId.Text + "," + lblComp.Text + ");";
            if (hdnEditeTicket.Value == "Y" || hdnViewTicket.Value == "Y")
            {
                item.Attributes["ondblclick"] = "window.open('addticket.aspx?id=" + lblTicketId.Text + "&comp=" + lblComp.Text + "&pop=1','_blank');";
            }
            else
            {
                item.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";
            }


        }
    }

    protected void RadGrid_OpenCalls_ItemCreated(object sender, GridItemEventArgs e)
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
    protected void lnkServiceExcel_Click(object sender, EventArgs e)
    {
        RadGrid_OpenCalls.MasterTableView.GetColumn("customername").Visible = true;
        RadGrid_OpenCalls.MasterTableView.GetColumn("City").Visible = true;
        RadGrid_OpenCalls.MasterTableView.GetColumn("Job").Visible = true;
        RadGrid_OpenCalls.MasterTableView.GetColumn("invoiceno").Visible = true;
        RadGrid_OpenCalls.MasterTableView.GetColumn("manualinvoice").Visible = true;
        RadGrid_OpenCalls.MasterTableView.GetColumn("defaultworker").Visible = true;
        RadGrid_OpenCalls.MasterTableView.GetColumn("ProjectDescription").Visible = true;
        RadGrid_OpenCalls.MasterTableView.GetColumn("timediff").Visible = true;
        RadGrid_OpenCalls.MasterTableView.GetColumn("department").Visible = true;
        RadGrid_OpenCalls.MasterTableView.GetColumn("fDesc").Visible = true;
        RadGrid_OpenCalls.MasterTableView.GetColumn("descres").Visible = true;

        RadGrid_OpenCalls.ExportSettings.FileName = "ServiceHistory";
        RadGrid_OpenCalls.ExportSettings.IgnorePaging = true;
        RadGrid_OpenCalls.ExportSettings.ExportOnlyData = true;
        RadGrid_OpenCalls.ExportSettings.OpenInNewWindow = true;
        RadGrid_OpenCalls.ExportSettings.HideStructureColumns = true;
        RadGrid_OpenCalls.MasterTableView.UseAllDataFields = true;
        RadGrid_OpenCalls.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_OpenCalls.MasterTableView.ExportToExcel();

    }
    #endregion

    #region Documents
    protected void RadGrid_Documents_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid_Documents.AllowCustomPaging = !ShouldApplySortFilterOrGroupDocuments();

            GetDocuments();
            //if (Session["mode"] != null)
            //{
            //    if (Session["mode"].ToString() == "0")
            //    {
            //        GetDocuments();
            //        // RadGrid_Documents.Rebind();

            //    }
            //}
            //GetDocuments(chkShowAllDocs.Checked);
        }
        catch { }
    }
    protected void RadGrid_Documents_PreRender(object sender, EventArgs e)
    {

        RowSelectDocuments();
    }

    bool isGroupingDocuments = false;
    public bool ShouldApplySortFilterOrGroupDocuments()
    {
        return RadGrid_Documents.MasterTableView.FilterExpression != "" ||
            (RadGrid_Documents.MasterTableView.GroupByExpressions.Count > 0 || isGroupingDocuments) ||
            RadGrid_Documents.MasterTableView.SortExpressions.Count > 0;
    }
    private void RowSelectDocuments()
    {
        if (hdnEditeDocument.Value == "N")
        {
            foreach (GridDataItem item in RadGrid_Documents.Items)
            {


                //CheckBox chkSelected = (CheckBox)item.FindControl("chkSelect");
                CheckBox chkPortal = (CheckBox)item.FindControl("chkPortal");
                TextBox txtremarks = (TextBox)item.FindControl("txtremarks");
                chkPortal.Enabled = txtremarks.Enabled = false;
                item.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";


            }
        }
    }
    #endregion

    #region Invoices
    protected void RadGrid_Invoices_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid_Invoices.AllowCustomPaging = !ShouldApplySortFilterOrGroupInvoices();
            if ((Request.QueryString["uid"]) != null)
            { GetData(); }
        }
        catch { }

    }
    protected void RadGrid_Invoices_PreRender(object sender, EventArgs e)
    {
        try
        {
            GeneralFunctions obj = new GeneralFunctions();
            obj.CorrectTelerikPager(RadGrid_Invoices);
            RowSelectInvoices();

            #region Grid Footer Balance 
            if (Request.QueryString["uid"] != null)
            {
                GridFooterItem footerItem = (GridFooterItem)RadGrid_Invoices.MasterTableView.GetItems(GridItemType.Footer)[0];

                //Label lblCusBal = footerItem.FindControl("lblCusBal") as Label;
                //// lblCusBal.Text = "Balance : $" + Convert.ToString(ViewState["CusBalance"]);
                //lblCusBal.Text = "Balance : " + String.Format("{0:C}", Convert.ToDecimal(ViewState["CusBalance"]));
            }
            #endregion
        }
        catch
        {
        }
    }
    bool isGroupingInvoices = false;
    public bool ShouldApplySortFilterOrGroupInvoices()
    {
        return RadGrid_Invoices.MasterTableView.FilterExpression != "" ||
            (RadGrid_Invoices.MasterTableView.GroupByExpressions.Count > 0 || isGroupingInvoices) ||
            RadGrid_Invoices.MasterTableView.SortExpressions.Count > 0;
    }
    private void RowSelectInvoices()
    {

        //foreach (GridDataItem gr in RadGrid_Invoices.Items)
        //{


        //    Label lblID = (Label)gr.FindControl("lblID");
        //    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
        //    Label lblType = (Label)gr.FindControl("lblType");
        //    HiddenField hdnLinkTo = (HiddenField)gr.FindControl("hdnLinkTo");

        //   gr.Attributes["ondblclick"] = "ShowHistoryTransactionPopup(" + lblID.Text + "," + Convert.ToInt32(hdnLinkTo.Value) + "," + Convert.ToInt32(Request.QueryString["uid"].ToString())+",0)";


        //}

    }
    protected void RadGrid_Invoices_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 5;

        if (e.Worksheet.Table.Rows.Count == RadGrid_Invoices.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_Invoices.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;

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
    protected void RadGrid_Invoices_ItemCreated(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridPagerItem)
        {
            var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
            var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
            if (Convert.ToString(RadGrid_Invoices.MasterTableView.FilterExpression) != "")
            {
                lblRecordCount.Text = totalCount + " Record(s) found";
            }
            else
            {
                lblRecordCount.Text = RadGrid_Invoices.VirtualItemCount + " Record(s) found";
            }
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
    protected void lnkInvoiceExcel_Click(object sender, EventArgs e)
    {
        RadGrid_Invoices.MasterTableView.GetColumn("CustomerName").Visible = true;
        RadGrid_Invoices.ExportSettings.FileName = "Transactions";
        RadGrid_Invoices.ExportSettings.IgnorePaging = true;
        RadGrid_Invoices.ExportSettings.ExportOnlyData = true;
        RadGrid_Invoices.ExportSettings.OpenInNewWindow = true;
        RadGrid_Invoices.ExportSettings.HideStructureColumns = true;
        RadGrid_Invoices.MasterTableView.UseAllDataFields = true;
        RadGrid_Invoices.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_Invoices.MasterTableView.ExportToExcel();

    }
    #endregion

    #region logs
    protected void RadGrid_gvLogs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        getCustomerLog();
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
    #endregion

    #region Eqiupment  Status Filter 

    #endregion
    protected void RadGrid_Equip_FilterCheckListItemsRequested(object sender, GridFilterCheckListItemsRequestedEventArgs e)
    {
        string DataField = (e.Column as IGridDataColumn).GetActiveDataField();
        DataTable dtEquip = CreateDataTableForEquipMultiFilter();
        if (dtEquip != null)
        {
            e.ListBox.DataSource = dtEquip;
            e.ListBox.DataKeyField = "Equip";
            e.ListBox.DataTextField = "Equip";
            e.ListBox.DataValueField = "Status";
            e.ListBox.DataBind();
        }
    }

    private DataTable CreateDataTableForEquipMultiFilter()
    {
        //create new table to add filter values.
        DataTable dtFilters = new DataTable();
        dtFilters.Clear();
        dtFilters.Columns.Add("Equip");
        dtFilters.Columns.Add("Status");
        DataRow dtFiltersRow = dtFilters.NewRow();
        dtFiltersRow["Equip"] = "Active";
        dtFiltersRow["Status"] = "0";
        dtFilters.Rows.Add(dtFiltersRow);
        DataRow dtFiltersRow2 = dtFilters.NewRow();
        dtFiltersRow2["Equip"] = "Inactive";
        dtFiltersRow2["Status"] = "1";
        dtFilters.Rows.Add(dtFiltersRow2);

        return dtFilters;

    }


    private void getCustomerLog()
    {
        try
        {
            RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();
            if (Request.QueryString["uid"] != null)
            {
                DataSet dsLog = new DataSet();
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.CustomerID = Convert.ToInt32(Request.QueryString["uid"].ToString());

                _GetCustomersLogs.ConnConfig = Session["config"].ToString();
                _GetCustomersLogs.CustomerID = Convert.ToInt32(Request.QueryString["uid"].ToString());

                List<GetCustomersLogsViewModel> _lstGetCustomersLogs = new List<GetCustomersLogsViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "CustomersAPI/AddCustomer_GetCustomersLogs";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomersLogs);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetCustomersLogs = serializer.Deserialize<List<GetCustomersLogsViewModel>>(_APIResponse.ResponseData);
                    dsLog = CommonMethods.ToDataSet<GetCustomersLogsViewModel>(_lstGetCustomersLogs);
                }
                else
                {
                    dsLog = objBL_User.GetCustomersLogs(objPropUser);
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
        catch { }
    }

    private void getContactLog()
    {
        try
        {
            RadGrid_gvContactLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();

            if (Request.QueryString["uid"] != null)
            {
                objPropUser.DBName = Session["dbname"].ToString();
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.LocID = Convert.ToInt32(Request.QueryString["uid"]);

                _GetContactLogByCustomerID.DBName = Session["dbname"].ToString();
                _GetContactLogByCustomerID.ConnConfig = Session["config"].ToString();
                _GetContactLogByCustomerID.LocID = Convert.ToInt32(Request.QueryString["uid"]);

                DataSet ds = new DataSet();

                List<GetContactLogByCustomerIDViewModel> _lstGetContactLogByCustomerID = new List<GetContactLogByCustomerIDViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "CustomersAPI/AddCustomer_GetContactLogByCustomerID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetContactLogByCustomerID);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetContactLogByCustomerID = serializer.Deserialize<List<GetContactLogByCustomerIDViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetContactLogByCustomerIDViewModel>(_lstGetContactLogByCustomerID);
                }
                else
                {
                    ds = objBL_User.getContactLogByCustomerID(objPropUser);
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    RadGrid_gvContactLogs.VirtualItemCount = ds.Tables[0].Rows.Count;
                    RadGrid_gvContactLogs.DataSource = ds.Tables[0];

                }
                else
                {
                    RadGrid_gvContactLogs.DataSource = string.Empty;
                }
            }

        }
        catch { }
    }

    private void getContact()
    {
        try
        {
            RadGrid_gvContacts.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();

            if (Request.QueryString["uid"] != null)
            {
                objPropUser.DBName = Session["dbname"].ToString();
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.RolId = Convert.ToInt32(ViewState["rolid"].ToString());

                _GetContactByRolID.DBName = Session["dbname"].ToString();
                _GetContactByRolID.ConnConfig = Session["config"].ToString();
                _GetContactByRolID.RolId = Convert.ToInt32(ViewState["rolid"].ToString());

                DataSet ds = new DataSet();

                List<GetContactByRolIDViewModel> _lstGetContactByRolID = new List<GetContactByRolIDViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "CustomersAPI/AddCustomer_GetContactByRolID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetContactByRolID);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetContactByRolID = serializer.Deserialize<List<GetContactByRolIDViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetContactByRolIDViewModel>(_lstGetContactByRolID);
                }
                else
                {
                    ds = objBL_User.getContactByRolID(objPropUser);
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    RadGrid_gvContacts.VirtualItemCount = ds.Tables[0].Rows.Count;
                    RadGrid_gvContacts.DataSource = ds.Tables[0];
                    ViewState["contacttable"] = ds.Tables[0];
                }
                else
                {
                    RadGrid_gvContacts.DataSource = string.Empty;
                    ViewState["contacttable"] = null;
                }
            }

        }
        catch { }

    }
    protected void lnkShowLog_Click(object sender, EventArgs e)
    {
        getContactLog();
        RadGrid_gvContactLogs.Rebind();
        string script = "function f(){$find(\"" + RadWindowContactLog.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }

    private void SaveFilter()
    {

        Session["filterstateAddCustomerHistory"] = ddlSearch.SelectedValue + ";"
            + txtSearch.Text + ";"
            + ddlCategory.SelectedValue + ";"
            + ddlStatus.SelectedValue + ";"
            + txtfromDate.Text + ";"
            + txtToDate.Text + ";"
            + hdnSelectedDtRangeHistory.Value;

    }

    public void UpdateControl()
    {

        if (Session["filterstateAddCustomerHistory"] != null)
        {
            if (Session["filterstateAddCustomerHistory"].ToString() != string.Empty)
            {
                string[] strFilter = Session["filterstateAddCustomerHistory"].ToString().Split(';');
                ddlSearch.SelectedValue = strFilter[0];
                txtSearch.Text = strFilter[1];
                ddlCategory.SelectedValue = strFilter[2];
                ddlStatus.SelectedValue = strFilter[3];
                txtfromDate.Text = strFilter[4];
                txtToDate.Text = strFilter[5];
                hdnSelectedDtRangeHistory.Value = strFilter[6];
                SHlblDay.Attributes.Remove("class");
                SHlblWeek.Attributes.Remove("class");
                SHlblMonth.Attributes.Remove("class");
                SHlblQuarter.Attributes.Remove("class");
                SHlblYear.Attributes.Remove("class");
                switch (hdnSelectedDtRangeHistory.Value)
                {
                    case "Day":
                        SHlblDay.Attributes.Add("class", "labelactive");
                        break;
                    case "Week":
                        SHlblWeek.Attributes.Add("class", "labelactive");
                        break;
                    case "Month":
                        SHlblMonth.Attributes.Add("class", "labelactive");
                        break;
                    case "Quarter":
                        SHlblQuarter.Attributes.Add("class", "labelactive");
                        break;
                    case "Year":
                        SHlblYear.Attributes.Add("class", "labelactive");
                        break;
                }
                showFilterSearch();
            }
        }
        Session.Remove("filterstateAddCustomerHistory");
    }


    public void showFilterSearch()
    {
        ddlCategory.Style.Add("display", "none");
        txtSearch.Style.Add("display", "none");

        if (ddlSearch.SelectedValue == "t.cat")
        {

            ddlCategory.Style.Add("display", "block");


        }
        else
        {
            txtSearch.Style.Add("display", "block");
        }
    }
    private DataTable processDataFilter(DataTable dt)
    {
        DataTable result = dt;
        try
        {
            String sql = "1=1";
            if (Request.QueryString["f"] != null && Request.QueryString["f"] == "r" && Session["TicketListRadGVFilters"] != null)
            {

                List<RetainFilter> filters = new List<RetainFilter>();

                String expression = Convert.ToString(Session["AddCustomer_TicketListFilter"]);
                if (Session["TicketListRadGVFilters"] != null)
                {


                    var filtersGet = Session["TicketListRadGVFilters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_OpenCalls.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            if (column.UniqueName == "ID")
                            {
                                sql = sql + " And " + column.UniqueName + " = " + _filter.FilterValue;

                            }
                            else
                            {
                                sql = sql + " And " + column.UniqueName + " like '%" + _filter.FilterValue + "%'";
                            }

                        }
                    }

                }
                return result.Select(sql).CopyToDataTable();

            }
            else
            {
                return result;
            }
        }
        catch (Exception ex)
        {
            return dt;
        }


    }

    protected void lnkEditOpp_Click(object sender, EventArgs e)
    {
        var redirect = HttpUtility.UrlEncode(Request.RawUrl);
        foreach (GridDataItem item in RadGrid_Opportunity.SelectedItems)
        {
            Label lblID = (Label)item.FindControl("lblId");
            Response.Redirect("addopprt.aspx?uid=" + lblID.Text + "&redirect=" + redirect);
        }
    }

    protected void lnkDeleteOpp_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridDataItem item in RadGrid_Opportunity.SelectedItems)
            {
                Label lblID = (Label)item.FindControl("lblId");
                objProp_Customer.ConnConfig = Session["config"].ToString();
                objProp_Customer.OpportunityID = Convert.ToInt32(lblID.Text);

                _DeleteOpportunity.ConnConfig = Session["config"].ToString();
                _DeleteOpportunity.OpportunityID = Convert.ToInt32(lblID.Text);

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "CustomersAPI/AddCustomer_DeleteOpportunity";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _DeleteOpportunity);
                }
                else
                {
                    objBL_Customer.DeleteOpportunity(objProp_Customer);
                }

                //OpportunityList();
                RadGrid_Opportunity.Rebind();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyS", "noty({text: 'Opportunity deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false,dismissQueue: true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue: true});", true);
        }
    }

    protected void lnkExcelOpp_Click(object sender, EventArgs e)
    {
        RadGrid_Opportunity.ExportSettings.FileName = "Opportunity";
        RadGrid_Opportunity.ExportSettings.IgnorePaging = true;
        RadGrid_Opportunity.ExportSettings.ExportOnlyData = true;
        RadGrid_Opportunity.ExportSettings.OpenInNewWindow = true;
        RadGrid_Opportunity.ExportSettings.HideStructureColumns = true;
        RadGrid_Opportunity.MasterTableView.UseAllDataFields = true;
        RadGrid_Opportunity.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_Opportunity.MasterTableView.ExportToExcel();
    }

    protected void RadGrid_Opportunity_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 3;
        else
            currentItem = 4;
        if (e.Worksheet.Table.Rows.Count == RadGrid_Opportunity.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_Opportunity.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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

    protected void RadGrid_Opportunity_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
                //if (Convert.ToString(RadGrid_Opportunity.MasterTableView.FilterExpression) != "")
                //    lblRecordCount.Text = totalCount + " Record(s) found";
                //else
                //    lblRecordCount.Text = RadGrid_Opportunity.VirtualItemCount + " Record(s) found";
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
        catch { }
    }

    protected void RadGrid_Opportunity_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_Opportunity.AllowCustomPaging = !ShouldApplySortFilterOrGroupOpportunity();

        if (!string.IsNullOrEmpty(Request.QueryString["uid"]))
        {
            FillOpportunity(Convert.ToInt32(Request.QueryString["uid"].ToString()));
        }
    }

    bool isGroupingOpportunity = false;
    public bool ShouldApplySortFilterOrGroupOpportunity()
    {
        return RadGrid_Opportunity.MasterTableView.FilterExpression != "" ||
            (RadGrid_Opportunity.MasterTableView.GroupByExpressions.Count > 0 || isGroupingOpportunity) ||
            RadGrid_Opportunity.MasterTableView.SortExpressions.Count > 0;
    }

    private void FillOpportunity(int customerId)
    {
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.CustomerID = customerId;

        _GetOpportunityOfCustomer.ConnConfig = Session["config"].ToString();
        _GetOpportunityOfCustomer.CustomerID = customerId;

        List<GetOpportunityOfCustomerViewModel> _lstGetOpportunityOfCustomer = new List<GetOpportunityOfCustomerViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "CustomersAPI/AddCustomer_GetOpportunityOfCustomer";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetOpportunityOfCustomer);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetOpportunityOfCustomer = serializer.Deserialize<List<GetOpportunityOfCustomerViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetOpportunityOfCustomerViewModel>(_lstGetOpportunityOfCustomer);
        }
        else
        {
            ds = objBL_Customer.getOpportunityOfCustomer(objProp_Customer);
        }

        RadGrid_Opportunity.VirtualItemCount = ds.Tables[0].Rows.Count;
        RadGrid_Opportunity.DataSource = ds.Tables[0];
    }

    protected void RadGrid_Project_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid_Project.AllowCustomPaging = !ShouldApplySortFilterOrGroupProject();
            if (Request.QueryString["uid"] != null)
            {
                // get all locations of this customer
                if (Session["locationdataCust"] != null)
                {
                    DataTable dataTableJobs = new DataTable();
                    DataTable locations = (DataTable)Session["locationdataCust"];
                    DataSet ds = new DataSet();
                    DataTable dtFilters = CreateFiltersToDataTable();
                    foreach (DataRow item in locations.Rows)
                    {
                        objProp_Customer.SearchBy = "j.loc";
                        objProp_Customer.SearchValue = item["loc"].ToString();
                        objProp_Customer.Range = 0;
                        objProp_Customer.JobType = -1;

                        objProp_Customer.ConnConfig = Session["config"].ToString();

                        //API
                        _GetJobProject.SearchBy = "j.loc";
                        _GetJobProject.SearchValue = item["loc"].ToString();
                        _GetJobProject.Range = 0;
                        _GetJobProject.JobType = -1;
                        _GetJobProject.ConnConfig = Session["config"].ToString();

                        List<GetJobProjectViewModel> _lstGetJobProject = new List<GetJobProjectViewModel>();

                        if (IsAPIIntegrationEnable == "YES")
                        {
                            string APINAME = "CustomersAPI/AddCustomer_GetJobProject";

                            _GetJobProject.filtersData = dtFilters;

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetJobProject);

                            JavaScriptSerializer serializer = new JavaScriptSerializer();

                            serializer.MaxJsonLength = Int32.MaxValue;

                            _lstGetJobProject = serializer.Deserialize<List<GetJobProjectViewModel>>(_APIResponse.ResponseData);
                            ds = CommonMethods.ToDataSet<GetJobProjectViewModel>(_lstGetJobProject);
                        }
                        else
                        {
                            ds = objBL_Customer.getJobProject(objProp_Customer, dtFilters);
                        }

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            dataTableJobs.Merge(ds.Tables[0]);
                        }
                    }

                    RadGrid_Project.VirtualItemCount = dataTableJobs.Rows.Count;
                    RadGrid_Project.DataSource = dataTableJobs;
                }

                //objProp_Customer.SearchBy = "j.loc";
                //objProp_Customer.SearchValue = Request.QueryString["uid"].ToString();
                //objProp_Customer.Range = 0;
                //objProp_Customer.JobType = -1;
                //DataSet ds = new DataSet();
                //objProp_Customer.ConnConfig = Session["config"].ToString();

                //DataTable dtFilters = CreateFiltersToDataTable();
                //ds = objBL_Customer.getJobProject(objProp_Customer, dtFilters);


            }
        }
        catch { }

    }

    bool isGroupingProject = false;
    public bool ShouldApplySortFilterOrGroupProject()
    {
        return RadGrid_Project.MasterTableView.FilterExpression != "" ||
            (RadGrid_Project.MasterTableView.GroupByExpressions.Count > 0 || isGroupingProject) ||
            RadGrid_Project.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_Project_PreRender(object sender, EventArgs e)
    {

    }

    private DataTable CreateFiltersToDataTable()
    {
        //create new table to add filter values.
        DataTable dtFilters = new DataTable();
        dtFilters.Clear();
        dtFilters.Columns.Add("Customer");
        dtFilters.Columns.Add("Tag");
        dtFilters.Columns.Add("ID");
        dtFilters.Columns.Add("fdesc");
        dtFilters.Columns.Add("Status");
        dtFilters.Columns.Add("Stage");
        dtFilters.Columns.Add("Company");
        dtFilters.Columns.Add("CType");
        dtFilters.Columns.Add("TemplateDesc");
        dtFilters.Columns.Add("Type");
        dtFilters.Columns.Add("SalesPerson");
        dtFilters.Columns.Add("Route");
        dtFilters.Columns.Add("NHour");
        dtFilters.Columns.Add("ContractPrice");
        dtFilters.Columns.Add("NotBilledYet");
        dtFilters.Columns.Add("NComm");
        dtFilters.Columns.Add("NRev");
        dtFilters.Columns.Add("NLabor");
        dtFilters.Columns.Add("NMat");
        dtFilters.Columns.Add("NOMat");
        dtFilters.Columns.Add("NCost");
        dtFilters.Columns.Add("NProfit");
        dtFilters.Columns.Add("NRatio");
        dtFilters.Columns.Add("RouteFilters");
        dtFilters.Columns.Add("StageFilters");
        dtFilters.Columns.Add("DepartmentFilters");
        dtFilters.Columns.Add("ProjectManagerUserName");
        dtFilters.Columns.Add("LocationType");
        dtFilters.Columns.Add("BuildingType");
        dtFilters.Columns.Add("TotalBudgetedExpense");
        dtFilters.Columns.Add("SupervisorUserName");
        dtFilters.Columns.Add("OpenARBalance");
        dtFilters.Columns.Add("OpenAPBalance");
        dtFilters.Columns.Add("ExpectedClosingDate");
        dtFilters.Columns.Add("Estimate");
        DataRow dtFiltersRow = dtFilters.NewRow();
        dtFilters.Rows.Add(dtFiltersRow);

        return dtFilters;
    }

    protected void RadGrid_Opportunity_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataTable dt = new DataTable();
        dt.Clear();
        dt.Columns.Add("EstimateID");
        dt.Columns.Add("Last");

        DataTable dtProject = new DataTable();
        dtProject.Clear();
        dtProject.Columns.Add("ProjectID");
        dtProject.Columns.Add("Last");

        if (e.Item is GridDataItem)
        {
            var sss = e.Item.DataItem;
            Repeater InterestsRepeater = e.Item.FindControl("rptEstimates") as Repeater;
            HiddenField hdnEstimate = e.Item.FindControl("hdnGridEstimate") as HiddenField;
            if (hdnEstimate != null && !string.IsNullOrEmpty(hdnEstimate.Value))
            {
                var estArr = hdnEstimate.Value.Trim().Split(',');
                for (int i = 0; i < estArr.Length; i++)
                {
                    DataRow _temp = dt.NewRow();
                    _temp["EstimateID"] = estArr[i].Trim();
                    if (i == estArr.Length - 1)
                    {
                        _temp["Last"] = "true";
                    }
                    else
                    {
                        _temp["Last"] = "false";
                    }

                    dt.Rows.Add(_temp);
                }
            }
            //Get the instance of the right type
            if (InterestsRepeater != null)
            {
                InterestsRepeater.DataSource = dt;
                InterestsRepeater.DataBind();
            }

            Repeater projectRepeater = e.Item.FindControl("rptProjects") as Repeater;
            HiddenField hdnGridProject = e.Item.FindControl("hdnGridProject") as HiddenField;
            if (hdnGridProject != null && !string.IsNullOrEmpty(hdnGridProject.Value))
            {
                var projArr = hdnGridProject.Value.Trim().Split(',');
                for (int i = 0; i < projArr.Length; i++)
                {
                    DataRow _temp = dtProject.NewRow();
                    _temp["ProjectID"] = projArr[i].Trim();
                    if (i == projArr.Length - 1)
                    {
                        _temp["Last"] = "true";
                    }
                    else
                    {
                        _temp["Last"] = "false";
                    }

                    dtProject.Rows.Add(_temp);
                }
            }
            //Get the instance of the right type
            if (projectRepeater != null)
            {
                projectRepeater.DataSource = dtProject;
                projectRepeater.DataBind();
            }
        }
    }

    protected void LinkButton_Click(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "Estimate #")
        {
            Response.Redirect("addestimate.aspx?uid=" + e.CommandArgument + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl));
        }
        else if (e.CommandName == "Project #")
        {
            Response.Redirect("addProject.aspx?uid=" + e.CommandArgument + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl));
        }
    }
    protected void lnkCopyOpp_Click(object sender, EventArgs e)
    {
        var redirect = HttpUtility.UrlEncode(Request.RawUrl);
        foreach (GridDataItem di in RadGrid_Opportunity.SelectedItems)
        {
            TableCell cell = di["ClientSelectColumn"];
            CheckBox chkSelect = (CheckBox)cell.Controls[0];
            Label lblID = (Label)di.FindControl("lblId");

            if (chkSelect.Checked == true)
            {
                Response.Redirect("addopprt.aspx?uid=" + lblID.Text + "&t=c" + "&redirect=" + redirect);
            }
        }
    }

    protected void RadGrid_Invoices_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            foreach (GridColumn col in RadGrid_Invoices.MasterTableView.RenderColumns)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                if (col.UniqueName == "Balance")
                {
                    Label lblID = (Label)dataItem.FindControl("lblID");
                    Label lblType = (Label)dataItem.FindControl("lblType");
                    Label lblStatus = (Label)dataItem.FindControl("lblStatus");
                    HiddenField hdnLinkTo = (HiddenField)dataItem.FindControl("hdnLinkTo");
                    HiddenField hdnTransID = (HiddenField)dataItem.FindControl("hdnTransID");
                    dataItem[col.UniqueName].Attributes.Add("onclick", "ShowHistoryTransactionPopup(" + lblID.Text + "," + Convert.ToInt32(hdnLinkTo.Value) + "," + Convert.ToInt32(Request.QueryString["uid"].ToString()) + ",0,'" + lblStatus.Text + "'," + hdnTransID.Value + ")");
                }

            }
        }
    }

    protected void lnkShowAllOpen_Click(object sender, EventArgs e)
    {
        ddlSearchInv.SelectedIndex = 0;
        txtSearchInv.Text = string.Empty;
        ddlStatusInv.SelectedIndex = 0;
        ddlDepartment.SelectedIndex = 0;
        txtInvDt.Text = string.Empty;
        txtInvDt.Style.Add("display", "none");
        txtSearchInv.Style.Add("display", "block");
        ddllocation.SelectedIndex = 0;
        ddllocation.Style.Add("display", "none");
        txtInvDtTo.Text = string.Empty;
        txtInvDtFrom.Text = string.Empty;
        ViewState["ShowAll"] = "1";
        ishowAllInvoice.Value = "1";
        cleanFilter();
    }

    protected void RadGrid_OpenCalls_FilterCheckListItemsRequested(object sender, GridFilterCheckListItemsRequestedEventArgs e)
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getFilterCategory(objPropUser);
        string DataField = (e.Column as IGridDataColumn).GetActiveDataField();
        if (ds.Tables[0] != null)
        {
            e.ListBox.DataSource = ds.Tables[0];
            e.ListBox.DataKeyField = DataField;
            e.ListBox.DataTextField = DataField;
            e.ListBox.DataValueField = DataField;
            e.ListBox.DataBind();
        }
    }
}
public class EditContactModel
{
    public String Name { get; set; }
    public String Phone { get; set; }
    public String Fax { get; set; }
    public String Cell { get; set; }
    public String Email { get; set; }
    public String Title { get; set; }
    public String lblIndex { get; set; }
    public Boolean ShutDownA { get; set; }

}