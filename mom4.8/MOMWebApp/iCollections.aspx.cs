using BusinessEntity;
using BusinessLayer;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;
using Stimulsoft.Report;
using Stimulsoft.Report.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using System.Linq.Dynamic;
using System.Web;
using BusinessLayer.Billing;
using System.Text;
using BusinessEntity.APModels;
using BusinessEntity.Utility;
using MOMWebApp;
using System.Web.Script.Serialization;
using BusinessEntity.CustomersModel;
using BusinessEntity.Payroll;

public partial class iCollections : System.Web.UI.Page
{
    CollectionModel objCollectionModel = new CollectionModel();
    BL_Collection objBL_Collection = new BL_Collection();
    private static int intCustomerID = 0;
    private static int intExpExcelFlag = 0;
    BL_User objBL_User = new BL_User();
    int count = 0;
    int count_inv = 0;
    byte[] array = null;
    bool IsGst = false;
    private bool IsGridPageIndexChanged = false;

    //API Variables 
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    getCustomFieldsParam _getCustomFields = new getCustomFieldsParam();
    GetDepartmentParam _GetDepartment = new GetDepartmentParam();
    GetCompanyByCustomerParam _GetCompanyByCustomer = new GetCompanyByCustomerParam();
    getUserDefaultCompanyParam _getUserDefaultCompany = new getUserDefaultCompanyParam();
    GetCollectionsParam _GetCollections = new GetCollectionsParam();
    GetInvoicesByRefParam _GetInvoicesByRef = new GetInvoicesByRefParam();
    getConnectionConfigParam _getConnectionConfig = new getConnectionConfigParam();
    GetInvoiceItemByRefParam _GetInvoiceItemByRef = new GetInvoiceItemByRefParam();
    GetControlBranchParam _GetControlBranch = new GetControlBranchParam();
    GetTicketIDParam _GetTicketID = new GetTicketIDParam();
    GetTicketByIDParam _GetTicketByID = new GetTicketByIDParam();
    GetLocationByIDParam _GetLocationByID = new GetLocationByIDParam();
    GetElevByTicketParam _GetElevByTicket = new GetElevByTicketParam();
    GetequipREPDetailsParam _GetequipREPDetails = new GetequipREPDetailsParam();
    GetElevByTicketIDParam _GetElevByTicketID = new GetElevByTicketIDParam();
    GetCustomerStatementByLocParam _GetCustomerStatementByLoc = new GetCustomerStatementByLocParam();
    AddEmailLogParam _AddEmailLog = new AddEmailLogParam();
    GetCustomerStatementInvoicesSouthernParam _GetCustStatementInvSouthern = new GetCustomerStatementInvoicesSouthernParam();
    GetCustomerStatementInvoicesParam _GetCustomerStatementInvoices = new GetCustomerStatementInvoicesParam();
    GetEmailDetailByLocParam _GetEmailDetailByLoc = new GetEmailDetailByLocParam();
    GetCompanyDetailsParam _GetCompanyDetails = new GetCompanyDetailsParam();
    GetCustomerStatementByLocsParam _GetCustomerStatementByLocs = new GetCustomerStatementByLocsParam();
    GetCustStatementInvByLocationParam _GetCustStatementInvByLocation = new GetCustStatementInvByLocationParam();
    GetGLAccountParam _GetGLAccount = new GetGLAccountParam();
    writeOffInvoiceParam _writeOffInvoice = new writeOffInvoiceParam();
    GetInvoicesByIDParam _GetInvoicesByID = new GetInvoicesByIDParam();
    GetAutoCompleteBillCodesParam _GetAutoCompleteBillCodes = new GetAutoCompleteBillCodesParam();
    GetEmailLogsParam _GetEmailLogs = new GetEmailLogsParam();
    GetAllTicketIDParam _GetAllTicketID = new GetAllTicketIDParam();
    GetElevByTicketIDsParam _GetElevByTicketIDs = new GetElevByTicketIDsParam();
    GetActiveBillingCodeParam _GetActiveBillingCode = new GetActiveBillingCodeParam();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
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
            divSuccess.Visible = false;
            txtEndDate.Text = DateTime.Today.ToShortDateString();

            FillDepartment();
            //LoadRootNodes(RadTreeViewCustLocation, TreeNodeExpandMode.ServerSideCallBack);
            #region Check IsGstRate
            BL_General objBL_General = new BL_General();
            General objGeneral = new General();
            objGeneral.ConnConfig = Session["config"].ToString();
            objGeneral.CustomName = "Country";

            DataSet dsCustom = new DataSet();
            if (IsAPIIntegrationEnable == "YES")
            {
                List<CustomViewModel> _lstCustomViewModel = new List<CustomViewModel>();

                _getCustomFields.ConnConfig = Session["config"].ToString();
                _getCustomFields.CustomName = "Country";

                string APINAME = "iCollectionsAPI/iCollectionsList_GetCustomFields";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomFields);

                _lstCustomViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomViewModel>>(_APIResponse.ResponseData);
                dsCustom = CommonMethods.ToDataSet<CustomViewModel>(_lstCustomViewModel);
            }
            else
            {
                dsCustom = objBL_General.getCustomFields(objGeneral);
            }

            if (dsCustom.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dsCustom.Tables[0].Rows[0]["Label"].ToString()) && dsCustom.Tables[0].Rows[0]["Label"].ToString().Equals("1"))
                {
                    IsGst = true;
                }
            }
            #endregion
            ViewState["IsGst"] = IsGst;
            if (Request.QueryString["f"] == null)
            {
                UpdateControl();
            }

        }
        CompanyPermission();

        if (Convert.ToString(Session["MailSend"]) == "true")
        {
            Session["MailSend"] = null;
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Mail sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        isNoneTS.Visible = true;
        isTS.Visible = false;
        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["isDBTotalService"]))
        {
            if (ConfigurationManager.AppSettings["isDBTotalService"].ToString().ToLower().Equals("true".ToLower()))
            {
                isNoneTS.Visible = false;
                isTS.Visible = true;
            }
        }
        lnkARAgingReportByBusinessType.Text = string.Format("AR Aging by {0}", getBusinessTypeLabel());
    }

    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            RadGrid_Collections.MasterTableView.Columns.FindByUniqueName("Company").Visible = true;

        }
        else
        {
            //RadGrid_Collections.Columns[7].Visible = false;
            RadGrid_Collections.MasterTableView.Columns.FindByUniqueName("Company").Visible = false;
        }

        #region Hide/Dehide Invoices Reports 
        string Report1 = string.Empty;
        string Report2 = string.Empty;
        Report1 = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesMaintReport"].Trim();
        Report2 = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesExceptionReport"].Trim();
        if (Report1 == string.Empty || Report2 == string.Empty)
        {
            lnkMaintenance.Visible = false;
            lnkException.Visible = false;
        }
        if (Session["dbname"].ToString() == "adams")
        {
            lnkAdamMaintenance.Visible = true;

        }
        else
        {
            lnkAdamMaintenance.Visible = false;
        }
        #endregion

        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            //ds = (DataTable)Session["userinfo"];
            ds = GetUserById();
            //Equipment

            string CollectionPermission = ds.Rows[0]["Collection"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Collection"].ToString();

            string RCollectionView = CollectionPermission.Length < 4 ? "Y" : CollectionPermission.Substring(3, 1);
            string RCollectionEdit = CollectionPermission.Length < 2 ? "Y" : CollectionPermission.Substring(1, 1);
            string RCollectionReport = CollectionPermission.Length < 6 ? "Y" : CollectionPermission.Substring(5, 1);

            if (RCollectionView == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }

            if (RCollectionReport == "N")
            {
                lnkExcel.Visible = false;
                lnkEmailAll.Visible = false;
                lnkPDF.Visible = false;
                lnkInvoices.Visible = false;
                lnkReports.Visible = false;
            }

            //Write off
            string WriteOffPermissions = ds.Rows[0]["WriteOff"] == DBNull.Value ? "N" : ds.Rows[0]["WriteOff"].ToString().Substring(0, 1);
            lnkWriteOff.Visible = false;
            if (WriteOffPermissions == "Y")
            {
                lnkWriteOff.Visible = true;
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

    private void FillDepartment()
    {
        try
        {
            BusinessEntity.User objPropUser = new BusinessEntity.User();
            BL_User objBL_User = new BL_User();
            DataSet ds = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();

            List<JobTypeViewModel> _lstJobType = new List<JobTypeViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "iCollectionsAPI/iCollectionsList_GetDepartment";

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

            //ddlDepartment.DataSource = ds.Tables[0];
            //ddlDepartment.DataTextField = "type";
            //ddlDepartment.DataValueField = "id";
            //ddlDepartment.DataBind();

            //ddlDepartment.Items.Insert(0, new ListItem(":: Select ::", "0"));

            rcDepartment.DataSource = ds.Tables[0];
            rcDepartment.DataTextField = "type";
            rcDepartment.DataValueField = "id";
            rcDepartment.DataBind();

            //rcDepartment.Items.Insert(0, new ListItem(":: Select ::", "0"));

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void FillCompany()
    {
        BusinessEntity.CompanyOffice objCompany = new BusinessEntity.CompanyOffice();
        BL_Company objBL_Company = new BL_Company();
        objCompany.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();

        List<CompanyOfficeViewModel> _lstCompanyOffice = new List<CompanyOfficeViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "iCollectionsAPI/iCollectionsList_GetCompanyByCustomer";

            _GetCompanyByCustomer.UserID = Convert.ToInt32(Session["UserID"].ToString());
            _GetCompanyByCustomer.DBName = Session["dbname"].ToString();
            _GetCompanyByCustomer.ConnConfig = Session["config"].ToString();

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
            rcCompany.DataSource = ds.Tables[0];
            rcCompany.DataTextField = "Name";
            rcCompany.DataValueField = "CompanyID";
            rcCompany.DataBind();
            // ddlCompany.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
        }
        if (!string.IsNullOrEmpty(GetDefaultCompany()))
            rcCompany.Items.FindItemByText(GetDefaultCompany()).Selected = true;
        else
        {
            if (rcCompany.Items.Count > 1)
                rcCompany.SelectedIndex = 1;
        }
    }

    private string GetDefaultCompany()
    {
        BusinessEntity.CompanyOffice objCompany = new BusinessEntity.CompanyOffice();
        BL_Company objBL_Company = new BL_Company();

        objCompany.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();

        List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "iCollectionsAPI/iCollectionsList_getUserDefaultCompany";

            _getUserDefaultCompany.UserID = Convert.ToInt32(Session["UserID"].ToString());
            _getUserDefaultCompany.DBName = Session["dbname"].ToString();
            _getUserDefaultCompany.ConnConfig = Session["config"].ToString();

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getUserDefaultCompany);

            _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
        }
        else
        {
            ds = objBL_Company.getUserDefaultCompany(objCompany);
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["DefaultCompID"] = ds.Tables[0].Rows[0]["EN"].ToString();
            string companyname = ds.Tables[0].Rows[0]["Name"].ToString();
            return companyname;
        }
        return "";
    }

    protected void RadGrid_Collections_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            RadGrid_Collections.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
            BindCollectionGrid();
        }
        catch (Exception ex)
        {

        }
    }

    protected void RadGrid_Collections_PreRender(object sender, EventArgs e)
    {
        if (intExpExcelFlag == 0)
        {
            int intVal = 0;
            foreach (GridGroupFooterItem groupFooter in RadGrid_Collections.MasterTableView.GetItems(GridItemType.GroupFooter))
            {
                string valTotal = groupFooter["Total"].Text;
                string valThirtyDay = groupFooter["ThirtyDay"].Text;
                string valSixtyDay = groupFooter["SixtyDay"].Text;
                string valNintyDay = groupFooter["NintyDay"].Text;
                string valNintyOneDay = groupFooter["NintyOneDay"].Text;
                string valOneTwentyDay = groupFooter["OneTwentyDay"].Text;
                foreach (GridGroupHeaderItem groupHeader in RadGrid_Collections.MasterTableView.GetItems(GridItemType.GroupHeader))
                {
                    if (groupHeader.GroupIndex == groupFooter.GroupIndex)
                    {
                        if (groupHeader.GroupIndex == Convert.ToString(intVal))
                        {
                            if (Convert.ToString(Session["CmpChkDefault"]) == "1")
                                groupHeader.DataCell.ColumnSpan = 13;
                            else
                                groupHeader.DataCell.ColumnSpan = 12;
                            intVal = intVal + 2;
                        }
                        else
                             if (Convert.ToString(Session["CmpChkDefault"]) == "1")
                            groupHeader.DataCell.ColumnSpan = 12;
                        else
                            groupHeader.DataCell.ColumnSpan = 11;
                        groupHeader.DataCell.Text += "<td class=TotalColer>" + valTotal;
                        groupHeader.DataCell.Text += "</td><td class=TotalColer>" + valThirtyDay;
                        groupHeader.DataCell.Text += "</td><td class=TotalColer>" + valSixtyDay;
                        groupHeader.DataCell.Text += "</td><td class=TotalColer>" + valNintyDay;
                        groupHeader.DataCell.Text += "</td><td class=TotalColer>" + valNintyOneDay;
                        groupHeader.DataCell.Text += "</td><td class=TotalColer>" + valOneTwentyDay;

                    }
                }
            }
        }
        else
            intExpExcelFlag = 0;
    }

    private void BindCollectionGrid()
    {

        SaveFilter();

        List<RetainFilter> filters = new List<RetainFilter>();
        String filterExpression = Convert.ToString(RadGrid_Collections.MasterTableView.FilterExpression);
        if (!IsPostBack)
        {
            if (filterExpression == "")
            {
                if (Convert.ToString(Request.QueryString["f"]) != "c")
                {
                    if (Session["Collections_FilterExpression"] != null && Convert.ToString(Session["Collections_FilterExpression"]) != "" && Session["Collections_Filters"] != null)
                    {
                        filterExpression = Convert.ToString(Session["Collections_FilterExpression"]);
                        RadGrid_Collections.MasterTableView.FilterExpression = Convert.ToString(Session["Collections_FilterExpression"]);
                        var filtersGet = Session["Collections_Filters"] as List<RetainFilter>;
                        if (filtersGet != null)
                        {
                            foreach (var _filter in filtersGet)
                            {

                                var filterCol = _filter.FilterColumn;
                                if (filterCol == "Status")
                                {
                                    GridColumn column = RadGrid_Collections.MasterTableView.GetColumnSafe("Status");

                                    if (column != null)
                                    {
                                        column.ListOfFilterValues = _filter.FilterValue.Replace("'", "").Split(',');
                                    }
                                }
                                else
                                {
                                    GridColumn column = RadGrid_Collections.MasterTableView.GetColumnSafe(_filter.FilterColumn);

                                    if (column != null)
                                    {
                                        column.CurrentFilterValue = _filter.FilterValue;
                                    }
                                }
                            }
                        }

                    }
                }
                else
                {
                    Session["Collections_FilterExpression"] = null;
                    Session["Collections_Filters"] = null;
                }

            }
        }
        if (!IsGridPageIndexChanged)
        {
            RadGrid_Collections.CurrentPageIndex = 0;
            Session["RadGrid_CollectionsCurrentPageIndex"] = 0;
            ViewState["RadGrid_CollectionsminimumRows"] = 0;
            ViewState["RadGrid_CollectionsmaximumRows"] = RadGrid_Collections.PageSize;
        }
        else
        {
            if (Session["RadGrid_CollectionsCurrentPageIndex"] != null && Convert.ToInt32(Session["RadGrid_CollectionsCurrentPageIndex"].ToString()) != 0
                && Request.QueryString["fil"] != null && Request.QueryString["fil"].ToString() == "1")
            {
                RadGrid_Collections.CurrentPageIndex = Convert.ToInt32(Session["RadGrid_CollectionsCurrentPageIndex"].ToString());
                ViewState["RadGrid_CollectionsminimumRows"] = RadGrid_Collections.CurrentPageIndex * RadGrid_Collections.PageSize;
                ViewState["RadGrid_CollectionsmaximumRows"] = (RadGrid_Collections.CurrentPageIndex + 1) * RadGrid_Collections.PageSize;

            }
        }

        if (string.IsNullOrEmpty(filterExpression) && Session["Collections_FilterExpression"] != null)
        {
            filterExpression = Convert.ToString(Session["Collections_FilterExpression"]);
        }




        if (filterExpression != "")
        {
            Session["Collections_FilterExpression"] = filterExpression;
            foreach (GridColumn column in RadGrid_Collections.MasterTableView.OwnerGrid.Columns)
            {

                String filterValues = String.Empty;
                String columnName = column.UniqueName;

                if (column.UniqueName == "Status")
                {
                    if (column.ListOfFilterValues != null)
                    {
                        List<string> listFil = new List<string>(column.ListOfFilterValues);
                        filterValues = String.Join(",", listFil.Select(x => string.Format("{0}", x)));
                        columnName = "Status";
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

            Session["Collections_Filters"] = filters;
        }




        BL_Collection obj = new BL_Collection();

        CollectionModel data = new CollectionModel();
        data.ConnConfig = Convert.ToString(Session["config"]);
        data.Date = Convert.ToDateTime(txtEndDate.Text);

        _GetCollections.ConnConfig = Convert.ToString(Session["config"]);
        _GetCollections.Date = Convert.ToDateTime(txtEndDate.Text);

        #region Customer & Location
        String CustomerIDs = "";
        String LocationIDs = "";

        if (CustomerIDs.Length > 1)
        {
            CustomerIDs = CustomerIDs.Remove(CustomerIDs.Length - 1);
            Session["CollCustomers"] = CustomerIDs;
        }
        else if (intCustomerID > 0)
        {
            CustomerIDs = Convert.ToString(intCustomerID);
            Session["CollCustomers"] = intCustomerID;
        }
        if (LocationIDs.Length > 1)
        {
            LocationIDs = LocationIDs.Remove(LocationIDs.Length - 1);
            Session["CollLocations"] = LocationIDs;
        }

        #endregion

        #region Department
        String DepartmentIDs = "";
        foreach (RadComboBoxItem li in rcDepartment.Items)
        {
            if (li.Checked == true)
            {
                DepartmentIDs = DepartmentIDs + Convert.ToString(li.Value) + ",";
            }
        }
        if (DepartmentIDs.Length > 1)
        {
            DepartmentIDs = DepartmentIDs.Remove(DepartmentIDs.Length - 1);
        }

        #endregion

        #region Company Check
        data.UserID = Convert.ToInt32(Session["UserID"].ToString());
        _GetCollections.UserID = Convert.ToInt32(Session["UserID"].ToString());
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            data.EN = 1;
            _GetCollections.EN = 1;
        }
        else
        {
            data.EN = 0;
            _GetCollections.EN = 0;
        }
        #endregion

        data.CustomerIDs = CustomerIDs;
        data.LocationIDs = LocationIDs;
        data.DepartmentIDs = DepartmentIDs;
        data.PrintEmail = Convert.ToString(drpPrintEmail.SelectedValue);

        _GetCollections.CustomerIDs = CustomerIDs;
        _GetCollections.LocationIDs = LocationIDs;
        _GetCollections.DepartmentIDs = DepartmentIDs;
        _GetCollections.PrintEmail = Convert.ToString(drpPrintEmail.SelectedValue);

        if (txtCustomDay.Text == "")
        {
            data.CustomDay = 0;
            _GetCollections.CustomDay = 0;
        }
        else
        {
            int n;
            bool isNumeric = int.TryParse(txtCustomDay.Text, out n);
            if (isNumeric)
            {
                data.CustomDay = Convert.ToInt32(txtCustomDay.Text);
                _GetCollections.CustomDay = Convert.ToInt32(txtCustomDay.Text);
            }
            else
            {
                data.CustomDay = 0;
                _GetCollections.CustomDay = 0;
            }
        }

        data.HidePartial = chkHidePartial.Checked;
        data.isDBTotalService = true;

        _GetCollections.HidePartial = chkHidePartial.Checked;
        _GetCollections.isDBTotalService = true;

        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["isDBTotalService"]))
        {
            data.isDBTotalService = Convert.ToBoolean(ConfigurationManager.AppSettings["isDBTotalService"]);
            _GetCollections.isDBTotalService = Convert.ToBoolean(ConfigurationManager.AppSettings["isDBTotalService"]);
        }

        DataSet ds = new DataSet();
        if (IsAPIIntegrationEnable == "YES")
        {
            List<GetCollectionsViewModel> _lstGetCollections = new List<GetCollectionsViewModel>();

            string APINAME = "iCollectionsAPI/iCollectionsList_GetCollections";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCollections);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetCollections = serializer.Deserialize<List<GetCollectionsViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetCollectionsViewModel>(_lstGetCollections);
        }
        else
        {
            ds = obj.GetCollections(data);
        }

        if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
        {

            RadGrid_Collections.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGrid_Collections.DataSource = ds.Tables[0];

            #region Get Filter Data
            String filterexpression = Convert.ToString(RadGrid_Collections.MasterTableView.FilterExpression);
            if (filterexpression != "")
            {
                DataTable dtt = ds.Tables[0];
                DataTable dt = dtt.AsEnumerable().AsQueryable().Where(filterexpression).CopyToDataTable();
                Session["CollectionARData"] = dt;
            }
            else
            {
                Session["CollectionARData"] = ds.Tables[0];
            }
            #endregion

            getCollectionNotes();
        }
        else
        {
            RadGrid_Collections.DataSource = string.Empty;
        }
    }

    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Collections.MasterTableView.FilterExpression != "" ||
            (RadGrid_Collections.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_Collections.MasterTableView.SortExpressions.Count > 0;
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {

        BindCollectionGrid();
        RadGrid_Collections.Rebind();
    }

    protected void rcDepartment_ItemChecked(object sender, RadComboBoxItemEventArgs e)
    {
        BindCollectionGrid();
        RadGrid_Collections.Rebind();
    }

    protected void rcDepartment_CheckAllCheck(object sender, RadComboBoxCheckAllCheckEventArgs e)
    {
        BindCollectionGrid();
        RadGrid_Collections.Rebind();
    }

    protected void btnSelectCustomer_Click(object sender, EventArgs e)
    {
        intCustomerID = Convert.ToInt32(hdnOwnerID.Value);
        BindCollectionGrid();
        RadGrid_Collections.Rebind();
        intCustomerID = 0;
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        intExpExcelFlag = 1;
        RadGrid_Collections.MasterTableView.GetColumn("chkSelect").Visible = false;
        RadGrid_Collections.ExportSettings.FileName = "Collections";
        RadGrid_Collections.ExportSettings.IgnorePaging = true;
        RadGrid_Collections.ExportSettings.OpenInNewWindow = true;
        RadGrid_Collections.MasterTableView.UseAllDataFields = true;
        RadGrid_Collections.ExportSettings.ExportOnlyData = true;
        RadGrid_Collections.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_Collections.ExportSettings.HideStructureColumns = false;
        RadGrid_Collections.MasterTableView.HierarchyDefaultExpanded = true;
        RadGrid_Collections.ExportSettings.SuppressColumnDataFormatStrings = false;
        RadGrid_Collections.MasterTableView.GroupsDefaultExpanded = true;
        RadGrid_Collections.MasterTableView.ExportToExcel();
    }

    protected void RadGrid_Collections_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        //int currentItem = 3;

        //if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        //    currentItem =4;
        //else
        //    currentItem = 5;
        //if (e.Worksheet.Table.Rows.Count == RadGrid_Collections.Items.Count + 1)
        //{
        //    GridFooterItem footerItem = RadGrid_Collections.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
        //    RowElement row = new RowElement(); //create new row for the footer aggregates
        //    for (int i = currentItem; i < footerItem.Cells.Count; i++)
        //    {
        //        TableCell fcell = footerItem.Cells[i];
        //        CellElement cell = new CellElement();
        //        // cell.Data.DataItem =  fcell.Text == "&nbsp;" ? "" : fcell.Text;
        //        if (i == currentItem +2)
        //            cell.Data.DataItem = "Total:-";
        //        else
        //            cell.Data.DataItem = fcell.Text == "&nbsp;" ? "" : fcell.Text;
        //        row.Cells.Add(cell);
        //    }
        //    e.Worksheet.Table.Rows.Add(row);

        //}
        //var current = e.Worksheet.Table.Rows.Count;      

        //if (e.Worksheet.Table.Rows.Count > RadGrid_Collections.Items.Count)
        //{
        //   if (ViewState["LastItem"] != null)
        //    {
        //        if (e.Worksheet.Table.Rows[current - 1].Cells.Count > 8 && e.Worksheet.Table.Rows[current - 1].Cells[8].Data.DataItem.ToString() == ViewState["LastItem"].ToString())
        //        {
        //            GridFooterItem footerItem = RadGrid_Collections.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
        //            RowElement row = new RowElement(); //create new row for the footer aggregates
        //            for (int i = currentItem; i < footerItem.Cells.Count; i++)
        //            {
        //                TableCell fcell = footerItem.Cells[i];
        //                CellElement cell = new CellElement();
        //                // cell.Data.DataItem =  fcell.Text == "&nbsp;" ? "" : fcell.Text;
        //                if (i == currentItem)
        //                    cell.Data.DataItem = "Total:-";
        //                else
        //                {
        //                    if (fcell.Text == "&nbsp;")
        //                    {
        //                        cell.Data.DataItem = "";
        //                    }
        //                    else
        //                    {
        //                        cell.Data.DataItem = fcell.Text;
        //                    }

        //                    row.Cells.Add(cell);
        //                }

        //            }
        //            row.Cells[0].Data.DataItem = "Total:-";
        //            e.Worksheet.Table.Rows.Add(row);
        //        }
        //    }

        //}



    }

    protected void RadGrid_Collections_ItemCreated(object sender, GridItemEventArgs e)
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

    #region Show Hide Location Details
    protected void chkHideLocation_OnClick(Object sender, EventArgs args)
    {
        RadGrid_Collections.PageSize = 50;
        RadGrid_Collections.Rebind();
        if (chkHideLocation.Checked)
        {
            RadGrid_Collections.PageSize = 1000;
            RadGrid_Collections.Rebind();
            CollapseAll();
        }
        else if (chkHideDetails.Checked)
        {
            RadGrid_Collections.PageSize = 1000;
            RadGrid_Collections.Rebind();
            CollapseAllDetails();
        }
    }
    private void CollapseAll()
    {
        foreach (GridItem item in RadGrid_Collections.MasterTableView.Controls[0].Controls)
        {
            if (item is GridGroupHeaderItem)
            {
                item.Expanded = false;
            }
        }
    }
    protected void chkHideDetails_OnClick(Object sender, EventArgs args)
    {
        RadGrid_Collections.PageSize = 50;
        RadGrid_Collections.Rebind();
        if (chkHideDetails.Checked)
        {
            RadGrid_Collections.PageSize = 1000;
            RadGrid_Collections.Rebind();
            CollapseAllDetails();
        }
        else if (chkHideLocation.Checked)
        {
            RadGrid_Collections.PageSize = 1000;
            RadGrid_Collections.Rebind();
            CollapseAll();
        }
    }
    private void CollapseAllDetails()
    {
        foreach (GridItem gi in RadGrid_Collections.MasterTableView.GetItems(GridItemType.GroupHeader))
        {
            if (gi.GroupIndex.Contains("_"))
            {
                gi.Expanded = false;
            }
        }
    }
    #endregion

    protected void lnkPrint_Click(object sender, EventArgs e)
    {
        PrintPDF();
    }

    private void PrintPDF()
    {
        try
        {
            StiWebViewer rvInvoices = new StiWebViewer();

            List<byte[]> invoicesToPrint = PrintInvoices(rvInvoices);



            string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF", "Invoices.pdf");

            if (invoicesToPrint != null)
            {
                byte[] buffer1 = null;

                buffer1 = concatAndAddContent(invoicesToPrint);

                if (File.Exists(filename))
                    File.Delete(filename);
                using (var fs = new FileStream(filename, FileMode.Create))
                {
                    fs.Write(buffer1, 0, buffer1.Length);
                    fs.Close();
                }
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=Invoices.pdf");
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Length", (buffer1.Length).ToString());
                Response.BinaryWrite(buffer1);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkMaintenance_Click(object sender, EventArgs e)
    {
        try
        {
            Contracts objProp_Contracts = new Contracts();
            BL_Contracts objBL_Contracts = new BL_Contracts();

            objProp_Contracts.ConnConfig = Session["config"].ToString();
            _GetInvoicesByRef.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                {
                    objProp_Contracts.isTS = 1;
                }
            }
            DataTable _dtInvoice = new DataTable();
            int j = 0;

            foreach (GridDataItem gr in RadGrid_Collections.Items)
            {
                HiddenField hdType = (HiddenField)gr.FindControl("hdType");
                HiddenField hdRef = (HiddenField)gr.FindControl("hdRef");
                if (hdType.Value == "1")
                {
                    int _ref = Convert.ToInt32(hdRef.Value);
                    objProp_Contracts.InvoiceID = _ref;
                    _GetInvoicesByRef.InvoiceID = _ref;

                    DataSet ds = new DataSet();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        List<GetInvoicesByRefViewModel> _lstGetInvoicesByRef = new List<GetInvoicesByRefViewModel>();

                        string APINAME = "iCollectionsAPI/iCollectionsList_GetInvoicesByRef";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoicesByRef);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        serializer.MaxJsonLength = Int32.MaxValue;

                        _lstGetInvoicesByRef = serializer.Deserialize<List<GetInvoicesByRefViewModel>>(_APIResponse.ResponseData);
                        ds = CommonMethods.ToDataSet<GetInvoicesByRefViewModel>(_lstGetInvoicesByRef);
                    }
                    else
                    {
                        ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);
                    }

                    if (j > 0)
                    {
                        _dtInvoice.Merge(ds.Tables[0], true);
                    }
                    else
                    {
                        _dtInvoice = ds.Tables[0];
                    }
                    j++;
                }
            }

            ViewState["InvoicesSubReportResult"] = _dtInvoice;

            if (_dtInvoice.Rows.Count > 0)
            {
                ReportViewer rvInvoices = new ReportViewer();

                PrintMaintInvoices(rvInvoices, _dtInvoice);

                byte[] buffer = null;
                buffer = ExportReportToPDF1("", rvInvoices);
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=Invoices.pdf");
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Length", (buffer.Length).ToString());
                Response.BinaryWrite(buffer);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No Invoice(s) found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void PrintMaintInvoices(ReportViewer rv, DataTable dtInvoice)
    {
        DataTable dtCompany = new DataTable();
        BusinessEntity.User objPropUser = new BusinessEntity.User();
        BL_User objBL_User = new BL_User();
        if (ViewState["RecurCompany"] == null)
        {
            DataSet dsCompany = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();

            if (IsAPIIntegrationEnable == "YES")
            {
                List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

                string APINAME = "iCollectionsAPI/iCollectionsList_GetControl";

                _getConnectionConfig.ConnConfig = Session["config"].ToString();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _GetControlViewModel = serializer.Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                dsCompany = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
            }
            else
            {
                dsCompany = objBL_User.getControl(objPropUser);
            }

            ViewState["RecurCompany"] = dsCompany.Tables[0];
            dtCompany = dsCompany.Tables[0];
        }
        else
        {
            dtCompany = (DataTable)ViewState["RecurCompany"];
        }
        rv.LocalReport.DataSources.Clear();  //added by dev 15th march, 16

        rv.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemSubReportProcessing);


        string Report = string.Empty;

        Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesMaintReport"].Trim();

        if (Report == "PESMTC_InvoicesMaint.rdlc")
        {
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                rv.LocalReport.DataSources.Add(new ReportDataSource("Invoice_PESdtInvoice", dtInvoice));
            }
        }

        rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dtCompany));

        string reportPath = string.Empty;


        if (Report == "PESMTC_InvoicesMaint.rdlc")
        {
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                reportPath = "Reports/" + Report.Trim();
            }
        }


        rv.LocalReport.ReportPath = reportPath;

        rv.LocalReport.EnableExternalImages = true;
        List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
        string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
        if (Report == "")
        {
            param1.Add(new ReportParameter("IsGstTax", ViewState["IsGst"].ToString()));
        }
        rv.LocalReport.SetParameters(param1);

        rv.LocalReport.Refresh();
    }

    private void ItemSubReportProcessing(object sender, SubreportProcessingEventArgs e)
    {
        try
        {
            Contracts objProp_Contracts = new Contracts();
            BL_Contracts objBL_Contracts = new BL_Contracts();

            DataTable dt = (DataTable)ViewState["InvoicesSubReportResult"];
            DataTable dtItems = new DataTable();

            objProp_Contracts.InvoiceID = Convert.ToInt32(dt.Rows[count_inv]["Ref"]);
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            DataSet ds = new DataSet();

            if (IsAPIIntegrationEnable == "YES")
            {
                List<GetInvoiceItemByRefViewModel> _lstGetInvoiceItemByRef = new List<GetInvoiceItemByRefViewModel>();

                string APINAME = "iCollectionsAPI/iCollectionsList_GetInvoiceItemByRef";

                _GetInvoiceItemByRef.InvoiceID = Convert.ToInt32(dt.Rows[count_inv]["Ref"]);
                _GetInvoiceItemByRef.ConnConfig = Session["config"].ToString();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoiceItemByRef);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetInvoiceItemByRef = serializer.Deserialize<List<GetInvoiceItemByRefViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetInvoiceItemByRefViewModel>(_lstGetInvoiceItemByRef);
            }
            else
            {
                ds = objBL_Contracts.GetInvoiceItemByRef(objProp_Contracts);
            }

            if (ds.Tables[0].Rows.Count < 1)
            {
                dtItems = LoadInvoiceDetails(ds.Tables[0], objProp_Contracts.InvoiceID);    // if none line item exists of invoice
            }
            else
                dtItems = ds.Tables[0];

            ReportDataSource rdsItems = null;
            if (dtItems.Rows.Count > 0)
            {
                //string Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesReport"].Trim();

                string Report = string.Empty;

                Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesMaintReport"].Trim();

                if (Report == "PESMTC_InvoicesMaint.rdlc")
                {
                    if (!string.IsNullOrEmpty(Report.Trim()))
                    {
                        rdsItems = new ReportDataSource("dtPESInvoiceItems", dtItems);
                    }
                }

                e.DataSources.Add(rdsItems);
            }
            if (count_inv == dt.Rows.Count - 1)
            {
                ViewState["InvoicesSubReportResult"] = null;
            }
            count_inv++;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkException_Click(object sender, EventArgs e)
    {
        try
        {
            Contracts objProp_Contracts = new Contracts();
            BL_Contracts objBL_Contracts = new BL_Contracts();

            objProp_Contracts.ConnConfig = Session["config"].ToString();
            _GetInvoicesByRef.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }
            DataTable _dtInvoice = new DataTable();
            int j = 0;

            foreach (GridDataItem gr in RadGrid_Collections.Items)
            {
                HiddenField hdType = (HiddenField)gr.FindControl("hdType");
                HiddenField hdRef = (HiddenField)gr.FindControl("hdRef");
                if (hdType.Value == "1")
                {
                    int _ref = Convert.ToInt32(hdRef.Value);
                    objProp_Contracts.InvoiceID = _ref;
                    _GetInvoicesByRef.InvoiceID = _ref;

                    DataSet ds = new DataSet();
                    if (IsAPIIntegrationEnable == "YES")
                    {
                        List<GetInvoicesByRefViewModel> _lstGetInvoicesByRef = new List<GetInvoicesByRefViewModel>();

                        string APINAME = "iCollectionsAPI/iCollectionsList_GetInvoicesByRef";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoicesByRef);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        serializer.MaxJsonLength = Int32.MaxValue;

                        _lstGetInvoicesByRef = serializer.Deserialize<List<GetInvoicesByRefViewModel>>(_APIResponse.ResponseData);
                        ds = CommonMethods.ToDataSet<GetInvoicesByRefViewModel>(_lstGetInvoicesByRef);
                    }
                    else
                    {
                        ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);
                    }

                    if (j > 0)
                    {
                        _dtInvoice.Merge(ds.Tables[0], true);
                    }
                    else
                    {
                        _dtInvoice = ds.Tables[0];
                    }
                    j++;
                }
            }

            ViewState["InvoicesSubReportResult"] = _dtInvoice;

            if (_dtInvoice.Rows.Count > 0)
            {
                ReportViewer rvInvoices = new ReportViewer();

                PrintExceptionInvoices(rvInvoices, _dtInvoice);

                byte[] buffer = null;
                buffer = ExportReportToPDF1("", rvInvoices);
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=Invoices.pdf");
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Length", (buffer.Length).ToString());
                Response.BinaryWrite(buffer);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No Invoice(s) found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void PrintExceptionInvoices(ReportViewer rv, DataTable dtInvoice)
    {
        DataTable dtCompany = new DataTable();
        BusinessEntity.User objPropUser = new BusinessEntity.User();
        BL_User objBL_User = new BL_User();

        if (ViewState["RecurCompany"] == null)
        {
            DataSet dsCompany = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();

            if (IsAPIIntegrationEnable == "YES")
            {
                List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

                string APINAME = "iCollectionsAPI/iCollectionsList_GetControl";

                _getConnectionConfig.ConnConfig = Session["config"].ToString();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _GetControlViewModel = serializer.Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                dsCompany = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
            }
            else
            {
                dsCompany = objBL_User.getControl(objPropUser);
            }

            ViewState["RecurCompany"] = dsCompany.Tables[0];
            dtCompany = dsCompany.Tables[0];
        }
        else
        {
            dtCompany = (DataTable)ViewState["RecurCompany"];
        }

        rv.LocalReport.DataSources.Clear();  //added by dev 15th march, 16

        rv.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemSubExceptionReportProcessing);


        string Report = string.Empty;

        Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesExceptionReport"].Trim();

        if (Report == "PESMTC_InvoicesExceptions.rdlc")
        {
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                rv.LocalReport.DataSources.Add(new ReportDataSource("Invoice_PESdtInvoice", dtInvoice));
            }
        }

        rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dtCompany));

        string reportPath = string.Empty;


        if (Report == "PESMTC_InvoicesExceptions.rdlc")
        {
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                reportPath = "Reports/" + Report.Trim();
            }
        }


        rv.LocalReport.ReportPath = reportPath;

        rv.LocalReport.EnableExternalImages = true;
        List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
        string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
        if (Report == "")
        {
            param1.Add(new ReportParameter("IsGstTax", ViewState["IsGst"].ToString()));
        }
        rv.LocalReport.SetParameters(param1);

        rv.LocalReport.Refresh();
    }

    private void ItemSubExceptionReportProcessing(object sender, SubreportProcessingEventArgs e)
    {
        try
        {
            Contracts objProp_Contracts = new Contracts();
            BL_Contracts objBL_Contracts = new BL_Contracts();

            DataTable dt = (DataTable)ViewState["InvoicesSubReportResult"];
            DataTable dtItems = new DataTable();

            objProp_Contracts.InvoiceID = Convert.ToInt32(dt.Rows[count_inv]["Ref"]);
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            DataSet ds = new DataSet();
            if (IsAPIIntegrationEnable == "YES")
            {
                List<GetInvoiceItemByRefViewModel> _lstGetInvoiceItemByRef = new List<GetInvoiceItemByRefViewModel>();

                string APINAME = "iCollectionsAPI/iCollectionsList_GetInvoiceItemByRef";

                _GetInvoiceItemByRef.InvoiceID = Convert.ToInt32(dt.Rows[count_inv]["Ref"]);
                _GetInvoiceItemByRef.ConnConfig = Session["config"].ToString();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoiceItemByRef);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetInvoiceItemByRef = serializer.Deserialize<List<GetInvoiceItemByRefViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetInvoiceItemByRefViewModel>(_lstGetInvoiceItemByRef);
            }
            else
            {
                ds = objBL_Contracts.GetInvoiceItemByRef(objProp_Contracts);
            }

            if (ds.Tables[0].Rows.Count < 1)
            {
                dtItems = LoadInvoiceDetails(ds.Tables[0], objProp_Contracts.InvoiceID);    // if none line item exists of invoice
            }
            else
                dtItems = ds.Tables[0];

            ReportDataSource rdsItems = null;
            if (dtItems.Rows.Count > 0)
            {
                string Report = string.Empty;

                Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesExceptionReport"].Trim();

                if (Report == "PESMTC_InvoicesExceptions.rdlc")
                {
                    if (!string.IsNullOrEmpty(Report.Trim()))
                    {
                        rdsItems = new ReportDataSource("dtPESInvoiceItems", dtItems);
                    }
                }

                e.DataSources.Add(rdsItems);
            }
            if (count_inv == dt.Rows.Count - 1)
            {
                ViewState["InvoicesSubReportResult"] = null;
            }
            count_inv++;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    List<byte[]> lstbyte = new List<byte[]>();
    protected void lnkPDFTI_Click(object sender, EventArgs e)
    {
        string Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceLNYWithTicket"].Trim();
        try
        {
            Contracts objProp_Contracts = new Contracts();
            BL_Contracts objBL_Contracts = new BL_Contracts();


            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            _GetInvoicesByRef.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }


            DataTable _dtInvoice = new DataTable();

            int j = 0;

            foreach (GridDataItem gr in RadGrid_Collections.Items)
            {
                HiddenField hdType = (HiddenField)gr.FindControl("hdType");
                HiddenField hdRef = (HiddenField)gr.FindControl("hdRef");
                if (hdType.Value == "1")
                {
                    int _ref = Convert.ToInt32(hdRef.Value);

                    objProp_Contracts.InvoiceID = _ref;
                    _GetInvoicesByRef.InvoiceID = _ref;

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        List<GetInvoicesByRefViewModel> _lstGetInvoicesByRef = new List<GetInvoicesByRefViewModel>();

                        string APINAME = "iCollectionsAPI/iCollectionsList_GetInvoicesByRef";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoicesByRef);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        serializer.MaxJsonLength = Int32.MaxValue;

                        _lstGetInvoicesByRef = serializer.Deserialize<List<GetInvoicesByRefViewModel>>(_APIResponse.ResponseData);
                        ds = CommonMethods.ToDataSet<GetInvoicesByRefViewModel>(_lstGetInvoicesByRef);
                    }
                    else
                    {
                        ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);
                    }


                    if (j > 0)
                    {
                        _dtInvoice.Merge(ds.Tables[0], true);
                    }
                    else
                    {
                        _dtInvoice = ds.Tables[0];
                    }
                    j++;
                }
            }
            ViewState["InvoicesSubReportResult"] = _dtInvoice;
            Session["InvoiceReportDetails"] = _dtInvoice;

            if (_dtInvoice.Rows.Count > 0)
            {
                if (Report != null && Path.GetExtension(Report) == ".mrt")
                {
                    PrintInvoiceWithTicketMRT(_dtInvoice, Report);
                }
                else
                {
                    foreach (DataRow drow in _dtInvoice.Rows)
                    {
                        ReportViewer rvInvoices = new ReportViewer();
                        int invoiceNo = (int)drow[1];
                        ViewState["invoiceNo"] = invoiceNo;


                        if (Report == "InvoiceLNY-WithTicket-Adams.rdlc")
                        {
                            PrintInvoiceWithTicketForAdams(rvInvoices, invoiceNo);
                        }
                        else
                        {
                            PrintInvoicesTicket(rvInvoices, invoiceNo);
                        }
                        array = ExportReportToPDF1("", rvInvoices);

                        lstbyte.Add(array);

                    }
                    byte[] allbyte = iCollections.concatAndAddContent(lstbyte);
                    Response.Clear();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Buffer = true;
                    Response.AddHeader("Content-Disposition", "attachment;filename=InvoicesWithTicket.pdf");
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Length", (allbyte.Length).ToString());
                    Response.BinaryWrite(allbyte);
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "noty({text: 'No Invoice(s) found to print.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void PrintInvoiceWithTicketForAdams(ReportViewer rvInvoices, int invoiceNo)
    {

        try
        {
            BL_Contracts objBL_Contracts = new BL_Contracts();
            Contracts objProp_Contracts = new Contracts();
            BusinessEntity.User objPropUser = new BusinessEntity.User();
            BL_User objBL_User = new BL_User();
            BL_MapData objBL_MapData = new BL_MapData();
            MapData objMapData = new MapData();



            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            DataSet dsTicket = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            _GetInvoicesByRef.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }


            DataTable _dtInvoice = new DataTable();
            DataSet _dsInvoice = new DataSet();
            objProp_Contracts.InvoiceID = invoiceNo;
            _GetInvoicesByRef.InvoiceID = invoiceNo;

            if (IsAPIIntegrationEnable == "YES")
            {
                List<GetInvoicesByRefViewModel> _lstGetInvoicesByRef = new List<GetInvoicesByRefViewModel>();

                string APINAME = "iCollectionsAPI/iCollectionsList_GetInvoicesByRef";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoicesByRef);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetInvoicesByRef = serializer.Deserialize<List<GetInvoicesByRefViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetInvoicesByRefViewModel>(_lstGetInvoicesByRef);
            }
            else
            {
                ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);
            }

            _dtInvoice = ds.Tables[0];


            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();

            _GetControlBranch.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() != "TS")
            {
                if (IsAPIIntegrationEnable == "YES")
                {
                    List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

                    string APINAME = "iCollectionsAPI/iCollectionsList_GetControl";

                    _getConnectionConfig.ConnConfig = Session["config"].ToString();

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _GetControlViewModel = serializer.Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                    dsC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
                }
                else
                {
                    dsC = objBL_User.getControl(objPropUser);
                }
            }
            else
            {
                objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
                _GetControlBranch.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);

                if (IsAPIIntegrationEnable == "YES")
                {
                    List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

                    string APINAME = "iCollectionsAPI/iCollectionsList_GetControlBranch";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetControlBranch);

                    _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                    dsC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
                }
                else
                {
                    dsC = objBL_User.getControlBranch(objPropUser);
                }
            }

            int _days = 0;
            for (int i = 0; i < _dtInvoice.Rows.Count; i++)
            {

                #region Determine Pay Terms
                if (_dtInvoice.Rows[i]["payterms"].ToString() == "0")
                {
                    _days = 0;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "1")
                {
                    _days = 10;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "2")
                {
                    _days = 15;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "3")
                {
                    _days = 30;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "4")
                {
                    _days = 45;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "5")
                {
                    _days = 60;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "6")
                {
                    _days = 30;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "7")
                {
                    _days = 90;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "8")
                {
                    _days = 180;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "9")
                {
                    _days = 0;
                }
                #endregion
                if (!string.IsNullOrEmpty(_dtInvoice.Rows[i]["IDate"].ToString()))
                {
                    _dtInvoice.Rows[i]["DueDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString()).AddDays(_days);
                    _dtInvoice.Rows[i]["fDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString());
                }
            }

            rvInvoices.LocalReport.DataSources.Clear();

            DataTable _dtInvItems = GetInvoiceItems(invoiceNo);

            //////Get Ticket /////

            DataTable dtTicket = new DataTable();
            int ii = 0;

            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.InvoiceID = invoiceNo;

            _GetTicketID.ConnConfig = Session["config"].ToString();
            _GetTicketID.InvoiceID = invoiceNo;

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataSet TicketID = new DataSet();
            if (IsAPIIntegrationEnable == "YES")
            {
                List<GetTicketIDViewModel> _lstGetTicketID = new List<GetTicketIDViewModel>();

                string APINAME = "iCollectionsAPI/iCollectionsList_GetTicketID";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetTicketID);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetTicketID = serializer.Deserialize<List<GetTicketIDViewModel>>(_APIResponse.ResponseData);
                TicketID = CommonMethods.ToDataSet<GetTicketIDViewModel>(_lstGetTicketID);
            }
            else
            {
                TicketID = objBL_Contracts.GetTicketID(objProp_Contracts);
            }


            foreach (DataRow item in TicketID.Tables[0].Rows)
            {
                objMapData.ConnConfig = Session["config"].ToString();
                objMapData.TicketID = (int)item[0];
                dsTicket = objBL_MapData.GetTicketByID(objMapData);
                if (ii == 0)
                {
                    dtTicket = dsTicket.Tables[0];
                    ii++;
                }
                else
                {
                    dtTicket.Rows.Add(dsTicket.Tables[0].Rows[0].ItemArray);
                    ii++;
                }
            }

            //////////////// 
            List<ReportParameter> param1 = new List<ReportParameter>();
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("dtInvoiceItems", _dtInvItems));
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("Invoice_dtInvoice", _dtInvoice));
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dsC.Tables[0]));
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicket", dtTicket));
            if (dtTicket.Rows.Count > 0)
            {
                param1.Add(new ReportParameter("ISTicket", "1"));
            }
            else
            {
                param1.Add(new ReportParameter("ISTicket", "0"));
            }

            ///Email//
            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.LocID = Convert.ToInt32(_dtInvoice.Rows[0]["Loc"]);

            _GetLocationByID.DBName = Session["dbname"].ToString();
            _GetLocationByID.ConnConfig = Session["config"].ToString();
            _GetLocationByID.LocID = Convert.ToInt32(_dtInvoice.Rows[0]["Loc"]);

            DataSet dsloc = new DataSet();

            if (IsAPIIntegrationEnable == "YES")
            {
                DataSet dsloc1 = new DataSet();
                DataSet dsloc2 = new DataSet();
                DataSet dsloc3 = new DataSet();
                DataSet dsloc4 = new DataSet();
                DataSet dsloc5 = new DataSet();

                ListGetLocationByID _lstGetLocationByID = new ListGetLocationByID();

                string APINAME = "iCollectionsAPI/iCollectionsList_GetLocationByID";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetLocationByID);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetLocationByID = serializer.Deserialize<ListGetLocationByID>(_APIResponse.ResponseData);

                dsloc1 = _lstGetLocationByID.lstTable1.ToDataSet();
                dsloc2 = _lstGetLocationByID.lstTable2.ToDataSet();
                dsloc3 = _lstGetLocationByID.lstTable3.ToDataSet();
                dsloc4 = _lstGetLocationByID.lstTable4.ToDataSet();
                dsloc5 = _lstGetLocationByID.lstTable5.ToDataSet();

                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();
                DataTable dt4 = new DataTable();
                DataTable dt5 = new DataTable();

                dt1 = dsloc1.Tables[0];
                dt2 = dsloc2.Tables[0];
                dt3 = dsloc3.Tables[0];
                dt4 = dsloc4.Tables[0];
                dt5 = dsloc5.Tables[0];

                dt1.TableName = "Table1";
                dt2.TableName = "Table2";
                dt3.TableName = "Table3";
                dt4.TableName = "Table4";
                dt5.TableName = "Table5";

                dsloc.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy(), dt4.Copy(), dt5.Copy() });
            }
            else
            {
                dsloc = objBL_User.getLocationByID(objPropUser);
            }

            string EmailTo = "";
            string EmailCC = "";
            string LocID = "";
            if (dsloc.Tables[0].Rows.Count > 0)
            {
                LocID = dsloc.Tables[0].Rows[0]["ID"].ToString();
                EmailTo = dsloc.Tables[0].Rows[0]["custom12"].ToString();
                EmailCC = dsloc.Tables[0].Rows[0]["custom13"].ToString();
                ////
            }
            //param1.Add(new ReportParameter("EmailTo", EmailTo));
            //param1.Add(new ReportParameter("EmailCC", EmailCC));
            //param1.Add(new ReportParameter("LocID", LocID));
            string reportPath = "Reports/InvoiceLNY-WithTicket-Adams.rdlc";
            rvInvoices.LocalReport.ReportPath = reportPath;
            rvInvoices.LocalReport.EnableExternalImages = true;
            string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
            param1.Add(new ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
            param1.Add(new ReportParameter("IsGstTax", ViewState["IsGst"].ToString()));
            rvInvoices.LocalReport.SetParameters(param1);
            rvInvoices.LocalReport.Refresh();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void PrintInvoicesTicket(ReportViewer rv, int invoiceNo)
    {
        BL_Contracts objBL_Contracts = new BL_Contracts();
        Contracts objProp_Contracts = new Contracts();
        BusinessEntity.User objPropUser = new BusinessEntity.User();
        BL_User objBL_User = new BL_User();
        BL_MapData objBL_MapData = new BL_MapData();
        MapData objMapData = new MapData();


        DataTable dtCompany = new DataTable();
        if (ViewState["RecurCompany"] == null)
        {
            DataSet dsCompany = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();

            if (IsAPIIntegrationEnable == "YES")
            {
                List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

                string APINAME = "iCollectionsAPI/iCollectionsList_GetControl";

                _getConnectionConfig.ConnConfig = Session["config"].ToString();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _GetControlViewModel = serializer.Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                dsCompany = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
            }
            else
            {
                dsCompany = objBL_User.getControl(objPropUser);
            }

            ViewState["RecurCompany"] = dsCompany.Tables[0];
            dtCompany = dsCompany.Tables[0];
        }
        else
        {
            dtCompany = (DataTable)ViewState["RecurCompany"];
        }
        DataTable dtInvoice = (DataTable)Session["InvoiceReportDetails"];
        dtInvoice = dtInvoice.Select("Ref=" + invoiceNo).CopyToDataTable();
        string Report = string.Empty;

        Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceLNYWithTicket"].Trim();

        DataTable dtEquip = new DataTable();
        DataTable dtTicket = new DataTable();
        DataTable dtTicketPO = new DataTable();
        DataTable dtTicketI = new DataTable();
        DataTable dtDetails = new DataTable();

        if (Report == "Invoice_Ticket-LNY.rdlc")
        {
            int i = 0;

            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.InvoiceID = invoiceNo;

            _GetTicketID.ConnConfig = Session["config"].ToString();
            _GetTicketID.InvoiceID = invoiceNo;

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataSet TicketID = new DataSet();
            if (IsAPIIntegrationEnable == "YES")
            {
                List<GetTicketIDViewModel> _lstGetTicketID = new List<GetTicketIDViewModel>();

                string APINAME = "iCollectionsAPI/iCollectionsList_GetTicketID";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetTicketID);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetTicketID = serializer.Deserialize<List<GetTicketIDViewModel>>(_APIResponse.ResponseData);
                TicketID = CommonMethods.ToDataSet<GetTicketIDViewModel>(_lstGetTicketID);
            }
            else
            {
                TicketID = objBL_Contracts.GetTicketID(objProp_Contracts);
            }

            foreach (DataRow item in TicketID.Tables[0].Rows)
            {
                objMapData.ConnConfig = Session["config"].ToString();
                objMapData.TicketID = (int)item[0];

                _GetElevByTicket.ConnConfig = Session["config"].ToString();
                _GetElevByTicket.TicketID = (int)item[0];

                DataSet dsEquip = new DataSet();

                if (IsAPIIntegrationEnable == "YES")
                {
                    List<GetElevByTicketViewModel> _lstGetElevByTicket = new List<GetElevByTicketViewModel>();

                    string APINAME = "iCollectionsAPI/iCollectionsList_GetElevByTicket";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetElevByTicket);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetElevByTicket = serializer.Deserialize<List<GetElevByTicketViewModel>>(_APIResponse.ResponseData);
                    dsEquip = CommonMethods.ToDataSet<GetElevByTicketViewModel>(_lstGetElevByTicket);
                }
                else
                {
                    dsEquip = objBL_MapData.getElevByTicket(objMapData);
                }

                DataSet dsTicket = objBL_MapData.GetTicketByID(objMapData);
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.EquipID = 0;
                objPropUser.SearchBy = "rd.ticketID";
                objPropUser.SearchValue = item[0].ToString();

                _GetequipREPDetails.ConnConfig = Session["config"].ToString();
                _GetequipREPDetails.EquipID = 0;
                _GetequipREPDetails.SearchBy = "rd.ticketID";
                _GetequipREPDetails.SearchValue = item[0].ToString();

                DataSet dsDetails = new DataSet();

                if (IsAPIIntegrationEnable == "YES")
                {
                    List<GetequipREPDetailsViewModel> _lstGetequipREPDetails = new List<GetequipREPDetailsViewModel>();

                    string APINAME = "iCollectionsAPI/iCollectionsList_GetequipREPDetails";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetequipREPDetails);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetequipREPDetails = serializer.Deserialize<List<GetequipREPDetailsViewModel>>(_APIResponse.ResponseData);
                    dsDetails = CommonMethods.ToDataSet<GetequipREPDetailsViewModel>(_lstGetequipREPDetails);
                }
                else
                {
                    dsDetails = objBL_User.getequipREPDetails(objPropUser);
                }

                if (i == 0)
                {
                    dtEquip = dsEquip.Tables[0];
                    dtTicket = dsTicket.Tables[0];
                    dtTicketPO = dsTicket.Tables[1];
                    dtTicketI = dsTicket.Tables[2];
                    dtDetails = dsDetails.Tables[0];
                    i++;
                }
                else
                {
                    if (dtEquip.Rows.Count > 0)
                    {
                        dtEquip.Rows.Add(dsEquip.Tables[0].Rows[0].ItemArray);
                    }

                    dtTicket.Rows.Add(dsTicket.Tables[0].Rows[0].ItemArray);
                    i++;
                }
            }
        }


        rv.LocalReport.DataSources.Clear();  //added by dev 15th march, 16

        rv.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemSubInvoiceTicketReportProcessing);

        if (Report == "Invoice_Ticket-LNY.rdlc")
        {
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                rv.LocalReport.DataSources.Add(new ReportDataSource("Invoice_PESdtInvoice", dtInvoice));
                rv.LocalReport.DataSources.Add(new ReportDataSource("dtEquipDetails", dtEquip));
                rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicket", dtTicket));
                rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtMCP", dtDetails));
                rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtPOItem", dtTicketPO));
                rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicketI", dtTicketI));
            }
        }
        else
        {
            rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_dtTicket", dtTicket));
        }

        rv.LocalReport.DataSources.Add(new ReportDataSource("Ticket_Company", dtCompany));

        DataTable rowCount = (DataTable)Session["InvoiceReportDetails"];

        //if (count == rowCount.Rows.Count - 1)
        //{

        string reportPath = string.Empty;


        if (Report == "Invoice_Ticket-LNY.rdlc")
        {
            if (!string.IsNullOrEmpty(Report.Trim()))
            {
                reportPath = "Reports/" + Report.Trim();
            }
        }



        rv.LocalReport.ReportPath = reportPath;

        rv.LocalReport.EnableExternalImages = true;
        List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
        string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));
        if (Report == "")
        {
            param1.Add(new ReportParameter("IsGstTax", ViewState["IsGst"].ToString()));
        }
        rv.LocalReport.SetParameters(param1);

        rv.LocalReport.Refresh();
        //}
    }

    private void ItemSubInvoiceTicketReportProcessing(object sender, SubreportProcessingEventArgs e)
    {
        try
        {
            Contracts objProp_Contracts = new Contracts();
            BL_Contracts objBL_Contracts = new BL_Contracts();
            MapData objMapData = new MapData();
            BL_MapData objBL_MapData = new BL_MapData();

            DataTable dt = (DataTable)ViewState["InvoicesSubReportResult"];
            DataTable dtItems = new DataTable();
            objProp_Contracts.InvoiceID = (int)ViewState["invoiceNo"];
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            DataSet ds = new DataSet();
            if (IsAPIIntegrationEnable == "YES")
            {
                List<GetInvoiceItemByRefViewModel> _lstGetInvoiceItemByRef = new List<GetInvoiceItemByRefViewModel>();

                string APINAME = "iCollectionsAPI/iCollectionsList_GetInvoiceItemByRef";

                _GetInvoiceItemByRef.InvoiceID = (int)ViewState["invoiceNo"];
                _GetInvoiceItemByRef.ConnConfig = Session["config"].ToString();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoiceItemByRef);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetInvoiceItemByRef = serializer.Deserialize<List<GetInvoiceItemByRefViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetInvoiceItemByRefViewModel>(_lstGetInvoiceItemByRef);
            }
            else
            {
                ds = objBL_Contracts.GetInvoiceItemByRef(objProp_Contracts);
            }

            if (ds.Tables[0].Rows.Count < 1)
            {
                dtItems = LoadInvoiceDetails(ds.Tables[0], objProp_Contracts.InvoiceID);    // if none line item exists of invoice
            }
            else
            {
                dtItems = ds.Tables[0];
            }

            DataTable dtEquip = new DataTable();

            int i = 0;
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.InvoiceID = (int)ViewState["invoiceNo"];

            _GetTicketID.ConnConfig = Session["config"].ToString();
            _GetTicketID.InvoiceID = (int)ViewState["invoiceNo"];

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataSet TicketID = new DataSet();
            if (IsAPIIntegrationEnable == "YES")
            {
                List<GetTicketIDViewModel> _lstGetTicketID = new List<GetTicketIDViewModel>();

                string APINAME = "iCollectionsAPI/iCollectionsList_GetTicketID";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetTicketID);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetTicketID = serializer.Deserialize<List<GetTicketIDViewModel>>(_APIResponse.ResponseData);
                TicketID = CommonMethods.ToDataSet<GetTicketIDViewModel>(_lstGetTicketID);
            }
            else
            {
                TicketID = objBL_Contracts.GetTicketID(objProp_Contracts);
            }

            foreach (DataRow item in TicketID.Tables[0].Rows)
            {
                objMapData.ConnConfig = Session["config"].ToString();
                objMapData.TicketID = (int)item[0];

                _GetElevByTicketID.ConnConfig = Session["config"].ToString();
                _GetElevByTicketID.TicketID = (int)item[0];

                DataSet dsEquip = new DataSet();

                if (IsAPIIntegrationEnable == "YES")
                {
                    List<GetTicketIDViewModel> _lstGetTicketID = new List<GetTicketIDViewModel>();

                    string APINAME = "iCollectionsAPI/iCollectionsList_GetElevByTicketID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetElevByTicketID);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetTicketID = serializer.Deserialize<List<GetTicketIDViewModel>>(_APIResponse.ResponseData);
                    dsEquip = CommonMethods.ToDataSet<GetTicketIDViewModel>(_lstGetTicketID);
                }
                else
                {
                    dsEquip = objBL_MapData.getElevByTicketID(objMapData);
                }

                if (i == 0)
                {
                    dtEquip = dsEquip.Tables[0];
                    i++;
                }
                else
                {
                    if (dtEquip.Rows.Count > 0)
                    {
                        dtEquip.Rows.Add(dsEquip.Tables[0].Rows[0].ItemArray);
                    }
                    i++;
                }
            }
            ReportDataSource rdsItems = null;
            if (dtItems.Rows.Count > 0)
            {
                string Report = string.Empty;

                Report = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceLNYWithTicket"].Trim();

                if (Report == "Invoice_Ticket-LNY.rdlc")
                {
                    if (!string.IsNullOrEmpty(Report.Trim()))
                    {
                        e.DataSources.Add(rdsItems = new ReportDataSource("dtPESInvoiceItems", dtItems));
                        e.DataSources.Add(rdsItems = new ReportDataSource("dtEquipDetailsID", dtEquip));
                    }
                }
                else
                {
                    e.DataSources.Add(rdsItems = new ReportDataSource("dtInvoiceItems", dtItems));
                }
            }
            if (count_inv == dtItems.Rows.Count - 1)
            {
                ViewState["InvoicesSubReportResult"] = null;
            }
            count_inv++;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkAdamMaintenance_Click(object sender, EventArgs e)
    {
        PrintPDF();
    }

    protected void lnkAdamBilling_Click(object sender, EventArgs e)
    {
        PrintPDF();
    }

    protected void RadGrid_Collections_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridGroupHeaderItem)
        {
            GridGroupHeaderItem item = (GridGroupHeaderItem)e.Item;
            DataRowView groupDataRow = (DataRowView)e.Item.DataItem;
            string owner = Convert.ToString(groupDataRow["Customer"]);
            string loc = "";
            try
            {
                loc = Convert.ToString(groupDataRow["Location"]);
            }
            catch
            {

            }

            //item.Attributes["onclick"] = "OpenCollectionPopNew('" + owner + "','" + loc + "');";
            if (loc == "")
            {
                // item.Attributes["class"] = "css_owner";
                item.CssClass = "css_owner_tr";
                item.Cells[1].Attributes["onclick"] = "OpenCollectionPopNew('" + owner.Replace("'", "\\'") + "','" + loc.Replace("'", "\\'") + "');";
                item.Cells[1].CssClass = "css_owner";
            }
            else
            {
                // item.Attributes["class"] = "css_loc";
                item.Cells[2].Attributes["onclick"] = "OpenCollectionPopNew('" + owner.Replace("'", "\\'") + "','" + loc.Replace("'", "\\'") + "');";
                item.Cells[2].CssClass = "css_loc";
            }



        }
    }

    protected void lnkEmailCustomerStatements_Click(object sender, EventArgs e)
    {
        try
        {
            Contracts objProp_Contracts = new Contracts();
            BL_Contracts objBL_Contracts = new BL_Contracts();
            BusinessEntity.User objPropUser = new BusinessEntity.User();
            BL_User objBL_User = new BL_User();
            BL_Report bL_Report = new BL_Report();

            DataTable dtAR = (DataTable)Session["CollectionARData"];

            string _todayDate = DateTime.Now.Date.ToString("MM-dd-yyyy");
            string fromEmail;
            if (ViewState["EmailFrom"] == null)
            {
                //fromEmail = GetFromEmailAddress();
                fromEmail = WebBaseUtility.GetFromEmailAddress();
            }
            else
            {
                fromEmail = ViewState["EmailFrom"].ToString();
            }


            #region Collect All The Loc
            List<String> Locs = new List<string>();
            foreach (DataRow rowItem in dtAR.Rows)
            {
                if (!string.IsNullOrEmpty(rowItem["Loc"].ToString()))
                {
                    Locs.Add(rowItem["Loc"].ToString());
                }
            }
            //foreach (GridDataItem gr in RadGrid_Collections.Items)
            //{
            //    HiddenField hdType = (HiddenField)gr.FindControl("hdType");
            //    HiddenField hdRef = (HiddenField)gr.FindControl("hdRef");
            //    HiddenField hdLoc = (HiddenField)gr.FindControl("hdLoc");               
            //    if (!string.IsNullOrEmpty(hdLoc.Value))
            //    {
            //        Locs.Add(hdLoc.Value);
            //    }
            //}

            List<String> uniqueLocs = Locs.Distinct().ToList();
            #endregion

            int totalInvoicesInList = uniqueLocs.Count;//dt != null ? dt.Rows.Count : 0;
            //var temp = dt.AsEnumerable().Where(t => t.Field<Int16>("InvStatus") != 2);
            //if (temp.Count() > 0)
            //{
            //    dt = temp.CopyToDataTable();
            //}

            int totalInvoicesForEmail = totalInvoicesInList;// temp.Count();//dt != null ? dt.Rows.Count : 0;
            int totalSentEmails = 0;
            int totalSendErr = 0;
            //int totalNotSend = 0;// totalInvoicesInList - totalInvoicesForEmail;
            //int mailCount = 0;
            List<MimeKit.MimeMessage> mimeSentMessages = new List<MimeKit.MimeMessage>();
            List<MimeKit.MimeMessage> mimeErrorMessages = new List<MimeKit.MimeMessage>();
            List<string> invoiceIdsSentEmail = new List<string>();
            //List<string> invoiceIdsError = new List<string>();
            Tuple<int, string, string> emailSendError = null;
            Tuple<int, string, string> emailGetSentError = null;
            StringBuilder sbdSentError = new StringBuilder();
            StringBuilder sbdGetSentError = new StringBuilder();

            EmailLog emailLog = new EmailLog();
            emailLog.ConnConfig = Session["config"].ToString();
            emailLog.Function = "Customer Statement All";
            emailLog.Screen = "Collections";
            emailLog.Username = Session["Username"].ToString();
            emailLog.SessionNo = Guid.NewGuid().ToString();

            _AddEmailLog.ConnConfig = Session["config"].ToString();
            _AddEmailLog.Function = "Customer Statement All";
            _AddEmailLog.Screen = "Collections";
            _AddEmailLog.Username = Session["Username"].ToString();
            _AddEmailLog.SessionNo = Guid.NewGuid().ToString();

            StringBuilder sbdInValidEmails = new StringBuilder();
            foreach (var loc in uniqueLocs)
            {
                ViewState["CollectionLoc"] = loc;
                string toEmail = "";
                string ccEmail = "";

                DataSet dsC = new DataSet();
                objPropUser.ConnConfig = Session["config"].ToString();
                _GetControlBranch.ConnConfig = Session["config"].ToString();

                if (Session["MSM"].ToString() != "TS")
                {
                    if (IsAPIIntegrationEnable == "YES")
                    {
                        List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

                        string APINAME = "iCollectionsAPI/iCollectionsList_GetControl";

                        _getConnectionConfig.ConnConfig = Session["config"].ToString();

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        serializer.MaxJsonLength = Int32.MaxValue;

                        _GetControlViewModel = serializer.Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                        dsC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
                    }
                    else
                    {
                        dsC = objBL_User.getControl(objPropUser);
                    }
                }
                else
                {
                    objPropUser.LocID = Convert.ToInt32(loc);
                    _GetControlBranch.LocID = Convert.ToInt32(loc);

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

                        string APINAME = "iCollectionsAPI/iCollectionsList_GetControlBranch";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetControlBranch);

                        _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                        dsC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
                    }
                    else
                    {
                        dsC = objBL_User.getControlBranch(objPropUser);
                    }
                }


                objProp_Contracts.ConnConfig = Session["config"].ToString();
                objProp_Contracts.Loc = Convert.ToInt32(loc);
                emailLog.Ref = objProp_Contracts.Loc;

                DataSet ds = new DataSet();

                if (IsAPIIntegrationEnable == "YES")
                {
                    List<GetCustomerStatementByLocViewModel> _lstGetCustomerStatementByLoc = new List<GetCustomerStatementByLocViewModel>();

                    string APINAME = "iCollectionsAPI/iCollectionsList_GetCustomerStatementByLoc";

                    _GetCustomerStatementByLoc.ConnConfig = Session["config"].ToString();
                    _GetCustomerStatementByLoc.Loc = Convert.ToInt32(loc);
                    _AddEmailLog.Ref = _GetCustomerStatementByLoc.Loc;

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomerStatementByLoc);

                    _lstGetCustomerStatementByLoc = (new JavaScriptSerializer()).Deserialize<List<GetCustomerStatementByLocViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetCustomerStatementByLocViewModel>(_lstGetCustomerStatementByLoc);
                }
                else
                {
                    ds = objBL_Contracts.GetCustomerStatementByLoc(objProp_Contracts);
                }

                DataTable dtLoc = ds.Tables[0];
                ViewState["Invoices"] = dtLoc;
                toEmail = dtLoc.Rows[0]["custom12"].ToString();
                if (!string.IsNullOrEmpty(toEmail))
                {
                    #region Email
                    if (!string.IsNullOrEmpty(dtLoc.Rows[0]["custom13"].ToString()))
                    {
                        ccEmail = dtLoc.Rows[0]["custom13"].ToString();
                    }
                    Mail mail = new Mail();
                    mail.From = fromEmail;
                    //Boolean IsMailSend = false;
                    foreach (var toaddress in toEmail.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        //IsMailSend = true;
                        mail.To.Add(toaddress);
                    }
                    foreach (var ccaddress in ccEmail.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        mail.Cc.Add(ccaddress);
                    }
                    StringBuilder _sbdInValidEmails = new StringBuilder();
                    CheckValidEmails(mail.To, _sbdInValidEmails);
                    CheckValidEmails(mail.Cc, _sbdInValidEmails);
                    if (_sbdInValidEmails.Length > 0)
                    {
                        sbdInValidEmails.Append(_sbdInValidEmails);
                    }

                    if (mail.To.Count == 0)
                    {
                        totalSendErr++;
                        emailLog.To = _sbdInValidEmails.ToString();
                        emailLog.Status = 0;
                        emailLog.UsrErrMessage = "Invalid emails address";

                        _AddEmailLog.To = _sbdInValidEmails.ToString();
                        _AddEmailLog.Status = 0;
                        _AddEmailLog.UsrErrMessage = "Invalid emails address";

                        BL_EmailLog bL_EmailLog = new BL_EmailLog();

                        if (IsAPIIntegrationEnable == "YES")
                        {
                            string APINAME = "iCollectionsAPI/iCollectionsList_AddEmailLog";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddEmailLog);
                        }
                        else
                        {
                            bL_EmailLog.AddEmailLog(emailLog);
                        }

                        continue;
                    }

                    #region Generate Report
                    ReportViewer rvCs = new ReportViewer();

                    rvCs.LocalReport.DataSources.Clear();

                    rvCs.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemDetailsSubReportProcessing);
                    rvCs.LocalReport.DataSources.Add(new ReportDataSource("dsInvoice", dtLoc));
                    rvCs.LocalReport.DataSources.Add(new ReportDataSource("dsCompany", dsC.Tables[0]));

                    #region Mail Body
                    BL_General bL_General = new BL_General();
                    EmailTemplate emailTemplate = new EmailTemplate();
                    emailTemplate.ConnConfig = Session["config"].ToString();
                    emailTemplate.Screen = "Collections";
                    emailTemplate.FunctionName = "Customer Statement All";
                    string mailContent = bL_General.GetEmailTemplate(emailTemplate);
                    string address = "";
                    if (string.IsNullOrEmpty(mailContent))
                    {
                        address = "Please review the attached customer statement <br/><br/>" + WebBaseUtility.GetSignature();
                    }
                    else
                    {
                        address = mailContent + WebBaseUtility.GetSignature();
                    }

                    #endregion

                    string reportPath;
                    if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("SECO"))
                    {
                        reportPath = "Reports/CustomerStatementSouthern.rdlc";
                    }
                    else
                    {
                        reportPath = "Reports/CustomerStatement.rdlc";
                    }


                    string Report = string.Empty;
                    if (!string.IsNullOrEmpty(Report.Trim()))
                    {
                        reportPath = "Reports/" + Report.Trim();
                    }
                    rvCs.LocalReport.ReportPath = reportPath;
                    rvCs.LocalReport.EnableExternalImages = true;
                    List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
                    string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
                    param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("path", strPath + "/images/Company_logo.jpg"));

                    rvCs.LocalReport.SetParameters(param1);

                    rvCs.LocalReport.Refresh();


                    #endregion

                    mail.Title = "Customer Statement - " + dtLoc.Rows[0]["LocID"].ToString() + " " + dtLoc.Rows[0]["locname"].ToString();
                    mail.Text = address.Replace(Environment.NewLine, "<BR/>");

                    mail.attachmentBytes = ExportReportToPDF1("", rvCs);
                    mail.FileName = "CustomerStatement_" + _todayDate + ".pdf";

                    mail.DeleteFilesAfterSend = true;
                    mail.RequireAutentication = false;

                    WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                    MimeKit.MimeMessage mimeMessage = new MimeKit.MimeMessage();
                    emailSendError = mail.CompletingMessage(ref mimeMessage, true, emailLog);
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
                                //invoiceIdsError.Add("Invoice #" + _ref.ToString());
                                totalSendErr++;
                            }
                        }
                        else
                        {
                            mimeSentMessages.Add(mimeMessage);
                            invoiceIdsSentEmail.Add("Loc #" + loc);
                        }
                    }
                    #endregion

                }
                else
                {
                    totalSendErr++;
                    emailLog.To = string.Empty;
                    emailLog.Status = 0;
                    emailLog.UsrErrMessage = "Email address does not exist for this location";

                    _AddEmailLog.To = string.Empty;
                    _AddEmailLog.Status = 0;
                    _AddEmailLog.UsrErrMessage = "Email address does not exist for this location";

                    BL_EmailLog bL_EmailLog = new BL_EmailLog();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "iCollectionsAPI/iCollectionsList_AddEmailLog";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddEmailLog);
                    }
                    else
                    {
                        bL_EmailLog.AddEmailLog(emailLog);
                    }

                }
            }

            totalSentEmails = mimeSentMessages.Count;
            if (totalSentEmails > 0)
            {
                Mail mail = new Mail();
                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                if (mail.TakeASentEmailCopy)
                {
                    emailLog.Ref = 0;
                    if (totalSentEmails >= 10)
                    {
                        List<List<MimeKit.MimeMessage>> lstTenMessages = new List<List<MimeKit.MimeMessage>>();
                        while (mimeSentMessages.Any())
                        {
                            lstTenMessages.Add(mimeSentMessages.Take(11).ToList());
                            mimeSentMessages = mimeSentMessages.Skip(11).ToList();
                        }

                        foreach (var lst in lstTenMessages)
                        {
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
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnSentError", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                if (emailGetSentError != null)
                {
                    string str1 = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnGetSentError", "noty({text: '" + str1 + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                }
            }
            else
            {
                if (totalSentEmails > 0)
                {
                    var successfullMess = "There were " + totalSentEmails + " of "
                        + totalInvoicesInList.ToString() + " Customer Statement sent out successfully.";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: '" + successfullMess + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);

                    if (totalSendErr > 0)
                    {
                        var errMess = "Total " + totalSendErr + " failed of "
                            + totalInvoicesInList.ToString() + " Customer Statement could not be sent.";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnSendErr", "noty({text: '" + errMess + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                    }

                    if (emailGetSentError != null)
                    {
                        string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnGetSentError", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                    }
                }
                else
                {
                    string str = "There were no emails sent out.";

                    if (totalSendErr > 0)
                    {
                        str += "<br>Total " + totalSendErr + " failed of "
                            + totalInvoicesInList.ToString() + " Customer Statement could not be sent.";
                    }

                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnNoEmail", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                }
            }
            //}
            if (sbdInValidEmails.Length > 0)
            {
                sbdInValidEmails.Insert(0, "Invalid emails address:<br/>");
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnInValidEmails", "noty({text: '" + sbdInValidEmails.ToString() + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default', closable: false, dismissQueue: true});", true);
            }
            // Refresh Email History grid
            RadGrid_gvLogs.Rebind();
        }
        catch (Exception ex)
        {
            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private byte[] ExportReportToPDF1(string reportName, ReportViewer reportviewer1)
    {
        Warning[] warnings;
        string[] streamids;
        string mimeType;
        string encoding;
        string filenameExtension;
        reportviewer1.ProcessingMode = ProcessingMode.Local;
        byte[] bytes = reportviewer1.LocalReport.Render(
            "PDF", null, out mimeType, out encoding, out filenameExtension,
             out streamids, out warnings);

        return bytes;
    }

    private void ItemDetailsSubReportProcessing(object sender, SubreportProcessingEventArgs e)
    {
        //throw new NotImplementedException();
        try
        {
            Contracts objProp_Contracts = new Contracts();
            BL_Contracts objBL_Contracts = new BL_Contracts();
            BusinessEntity.User objPropUser = new BusinessEntity.User();
            BL_User objBL_User = new BL_User();
            BL_Report bL_Report = new BL_Report();

            DataTable dt = (DataTable)ViewState["Invoices"];
            int loc = Convert.ToInt32(ViewState["CollectionLoc"]);

            objProp_Contracts.Loc = loc;
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            DataSet ds = new DataSet();
            if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("SECO"))
            {
                if (IsAPIIntegrationEnable == "YES")
                {
                    List<GetCustStatementInvSouthernViewModel> _lstGetCustStatementInvSouthern = new List<GetCustStatementInvSouthernViewModel>();

                    string APINAME = "iCollectionsAPI/iCollectionsList_GetCustomerStatementInvoicesSouthern";

                    _GetCustStatementInvSouthern.Loc = loc;
                    _GetCustStatementInvSouthern.ConnConfig = Session["config"].ToString();
                    _GetCustStatementInvSouthern.includeCredit = true;

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustStatementInvSouthern);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetCustStatementInvSouthern = serializer.Deserialize<List<GetCustStatementInvSouthernViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetCustStatementInvSouthernViewModel>(_lstGetCustStatementInvSouthern);
                }
                else
                {
                    ds = objBL_Contracts.GetCustomerStatementInvoicesSouthern(objProp_Contracts, true);
                }

            }
            else
            {
                if (IsAPIIntegrationEnable == "YES")
                {
                    List<GetCustStatementInvSouthernViewModel> _lstGetCustomerStatementInvoices = new List<GetCustStatementInvSouthernViewModel>();

                    string APINAME = "iCollectionsAPI/iCollectionsList_GetCustomerStatementInvoices";

                    _GetCustomerStatementInvoices.Loc = loc;
                    _GetCustomerStatementInvoices.ConnConfig = Session["config"].ToString();
                    _GetCustomerStatementInvoices.includeCredit = true;

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomerStatementInvoices);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetCustomerStatementInvoices = serializer.Deserialize<List<GetCustStatementInvSouthernViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetCustStatementInvSouthernViewModel>(_lstGetCustomerStatementInvoices);
                }
                else
                {
                    ds = objBL_Contracts.GetCustomerStatementInvoices(objProp_Contracts, true);
                }

            }


            if (dt.Rows.Count > 0)
            {
                ReportDataSource rdsItems = new ReportDataSource("dsInvoiceItem", ds.Tables[0]);

                e.DataSources.Add(rdsItems);
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void ItemDetailsSubReportProcessingNew(object sender, SubreportProcessingEventArgs e)
    {
        //throw new NotImplementedException();
        try
        {
            Contracts objProp_Contracts = new Contracts();
            BL_Contracts objBL_Contracts = new BL_Contracts();
            BusinessEntity.User objPropUser = new BusinessEntity.User();
            BL_User objBL_User = new BL_User();
            BL_Report bL_Report = new BL_Report();

            DataTable dt = (DataTable)ViewState["Invoices"];
            int loc = Convert.ToInt32(dt.Rows[count]["Loc"]);

            objProp_Contracts.Loc = loc;
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("SECO"))
            {
                if (IsAPIIntegrationEnable == "YES")
                {
                    List<GetCustStatementInvSouthernViewModel> _lstGetCustStatementInvSouthern = new List<GetCustStatementInvSouthernViewModel>();

                    string APINAME = "iCollectionsAPI/iCollectionsList_GetCustomerStatementInvoicesSouthern";

                    _GetCustStatementInvSouthern.Loc = loc;
                    _GetCustStatementInvSouthern.ConnConfig = Session["config"].ToString();
                    _GetCustStatementInvSouthern.includeCredit = true;

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustStatementInvSouthern);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetCustStatementInvSouthern = serializer.Deserialize<List<GetCustStatementInvSouthernViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetCustStatementInvSouthernViewModel>(_lstGetCustStatementInvSouthern);
                }
                else
                {
                    ds = objBL_Contracts.GetCustomerStatementInvoicesSouthern(objProp_Contracts, true);
                }
            }
            else
            {
                if (IsAPIIntegrationEnable == "YES")
                {
                    List<GetCustStatementInvSouthernViewModel> _lstGetCustomerStatementInvoices = new List<GetCustStatementInvSouthernViewModel>();

                    string APINAME = "iCollectionsAPI/iCollectionsList_GetCustomerStatementInvoices";

                    _GetCustomerStatementInvoices.Loc = loc;
                    _GetCustomerStatementInvoices.ConnConfig = Session["config"].ToString();
                    _GetCustomerStatementInvoices.includeCredit = true;

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomerStatementInvoices);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetCustomerStatementInvoices = serializer.Deserialize<List<GetCustStatementInvSouthernViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetCustStatementInvSouthernViewModel>(_lstGetCustomerStatementInvoices);
                }
                else
                {
                    ds = objBL_Contracts.GetCustomerStatementInvoices(objProp_Contracts, true);
                }

            }


            if (dt.Rows.Count > 0)
            {
                ReportDataSource rdsItems = new ReportDataSource("dsInvoiceItem", ds.Tables[0]);

                e.DataSources.Add(rdsItems);
            }

            count++;

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkEmailInvoices_Click(object sender, EventArgs e)
    {
        try
        {
            Contracts objProp_Contracts = new Contracts();
            BL_Contracts objBL_Contracts = new BL_Contracts();
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            _GetEmailDetailByLoc.ConnConfig = Session["config"].ToString();

            DataTable dt = (DataTable)Session["CollectionARData"];
            int totalInvoicesInList = dt.Rows.Count;


            int totalInvoicesForEmail = 0;
            int totalSentEmails = 0;
            int totalSendErr = 0;
            int totalNotSend = 0;//totalInvoicesInList - totalInvoicesForEmail;
            //int mailCount = 0;
            List<MimeKit.MimeMessage> mimeSentMessages = new List<MimeKit.MimeMessage>();
            List<MimeKit.MimeMessage> mimeErrorMessages = new List<MimeKit.MimeMessage>();
            List<string> invoiceIdsSentEmail = new List<string>();
            //List<string> invoiceIdsError = new List<string>();
            Tuple<int, string, string> emailSendError = null;
            Tuple<int, string, string> emailGetSentError = null;
            StringBuilder sbdSentError = new StringBuilder();
            StringBuilder sbdGetSentError = new StringBuilder();

            EmailLog emailLog = new EmailLog();
            emailLog.ConnConfig = Session["config"].ToString();
            emailLog.Function = "Invoice All";
            emailLog.Screen = "Collections";
            emailLog.Username = Session["Username"].ToString();
            emailLog.SessionNo = Guid.NewGuid().ToString();

            _AddEmailLog.ConnConfig = Session["config"].ToString();
            _AddEmailLog.Function = "Invoice All";
            _AddEmailLog.Screen = "Collections";
            _AddEmailLog.Username = Session["Username"].ToString();
            _AddEmailLog.SessionNo = Guid.NewGuid().ToString();

            BL_General bL_General = new BL_General();
            EmailTemplate emailTemplate = new EmailTemplate();
            emailTemplate.ConnConfig = Session["config"].ToString();
            emailTemplate.Screen = "Collections";
            emailTemplate.FunctionName = "Invoice All";
            string mailContent = bL_General.GetEmailTemplate(emailTemplate);

            StringBuilder sbdInValidEmails = new StringBuilder();
            foreach (DataRow rowItem in dt.Rows)
            {
                if (rowItem["type"].ToString() == "1")
                {
                    totalInvoicesForEmail++;
                    String InvoiceID = rowItem["Ref"].ToString();
                    objProp_Contracts.Ref = Convert.ToInt32(InvoiceID);
                    _GetEmailDetailByLoc.Ref = Convert.ToInt32(InvoiceID);

                    emailLog.Ref = objProp_Contracts.Ref;
                    _AddEmailLog.Ref = _GetEmailDetailByLoc.Ref;

                    DataSet _dsCon = new DataSet();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        List<GetEmailDetailByLocViewModel> _lstGetEmailDetailByLoc = new List<GetEmailDetailByLocViewModel>();

                        string APINAME = "iCollectionsAPI/iCollectionsList_GetEmailDetailByLoc";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetEmailDetailByLoc);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        serializer.MaxJsonLength = Int32.MaxValue;

                        _lstGetEmailDetailByLoc = serializer.Deserialize<List<GetEmailDetailByLocViewModel>>(_APIResponse.ResponseData);
                        _dsCon = CommonMethods.ToDataSet<GetEmailDetailByLocViewModel>(_lstGetEmailDetailByLoc);
                    }
                    else
                    {
                        _dsCon = objBL_Contracts.GetEmailDetailByLoc(objProp_Contracts);
                    }

                    if (_dsCon.Tables[0].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(_dsCon.Tables[0].Rows[0]["custom12"].ToString()))
                        {
                            string _fromEmail = string.Empty;

                            _fromEmail = Convert.ToString(ViewState["EmailFrom"]);
                            if (string.IsNullOrEmpty(_fromEmail))
                            {
                                _fromEmail = WebBaseUtility.GetFromEmailAddress();
                            }

                            string _toEmail = "";
                            string _ccEmail = "";
                            if (!string.IsNullOrEmpty(_dsCon.Tables[0].Rows[0]["custom12"].ToString()))
                            {
                                _toEmail = _dsCon.Tables[0].Rows[0]["custom12"].ToString();

                                if (!string.IsNullOrEmpty(_dsCon.Tables[0].Rows[0]["custom13"].ToString()))
                                {
                                    _ccEmail = _dsCon.Tables[0].Rows[0]["custom13"].ToString();
                                }
                            }

                            Mail mail = new Mail();
                            mail.From = _fromEmail;
                            foreach (var toaddress in _toEmail.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                mail.To.Add(toaddress);
                            }
                            foreach (var ccaddress in _ccEmail.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                mail.Cc.Add(ccaddress);
                            }

                            StringBuilder _sbdInValidEmails = new StringBuilder();
                            CheckValidEmails(mail.To, _sbdInValidEmails);
                            CheckValidEmails(mail.Cc, _sbdInValidEmails);
                            if (_sbdInValidEmails.Length > 0)
                            {
                                sbdInValidEmails.Append(_sbdInValidEmails);
                            }

                            if (mail.To.Count == 0)
                            {
                                totalSendErr++;
                                emailLog.To = _sbdInValidEmails.ToString();
                                emailLog.Status = 0;
                                emailLog.UsrErrMessage = "Invalid emails address";

                                _AddEmailLog.To = _sbdInValidEmails.ToString();
                                _AddEmailLog.Status = 0;
                                _AddEmailLog.UsrErrMessage = "Invalid emails address";

                                BL_EmailLog bL_EmailLog = new BL_EmailLog();

                                if (IsAPIIntegrationEnable == "YES")
                                {
                                    string APINAME = "iCollectionsAPI/iCollectionsList_AddEmailLog";

                                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddEmailLog);
                                }
                                else
                                {
                                    bL_EmailLog.AddEmailLog(emailLog);
                                }

                                continue;
                            }

                            #region Generate report
                            StiWebViewer rvInvoices = new StiWebViewer();
                            byte[] buffer = null;

                            List<byte[]> invoicesToPrint = PrintInvoicesForIndivudial(rvInvoices, Convert.ToInt32(InvoiceID));

                            string strInvoiceFileName = "Invoice" + InvoiceID + ".pdf";
                            //string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF", "Invoices.pdf");
                            string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF", strInvoiceFileName);

                            if (invoicesToPrint != null)
                            {
                                buffer = concatAndAddContent(invoicesToPrint);
                                if (File.Exists(filename))
                                    File.Delete(filename);
                                using (var fs = new FileStream(filename, FileMode.Create))
                                {
                                    fs.Write(buffer, 0, buffer.Length);
                                    fs.Close();
                                }
                            }
                            #endregion

                            mail.Title = "Invoice " + InvoiceID + " - " + _dsCon.Tables[0].Rows[0]["Tag"].ToString();

                            //var mailContent = ViewState["MailContent"].ToString().Replace(Environment.NewLine, "<BR/>");
                            if (string.IsNullOrEmpty(mailContent))
                            {
                                mailContent = ViewState["MailContent"].ToString().Replace(Environment.NewLine, "<BR/>");
                            }
                            else
                            {
                                mailContent = mailContent.Replace("{Invoice_No}", InvoiceID);
                                //.Replace("{Invoice_CustomerName}", _dsCon.Tables[0].Rows[0]["customerName"].ToString())

                            }

                            var companySignature = ViewState["CompanyAddress"].ToString().Replace(Environment.NewLine, "<BR/>");

                            mail.Text = mailContent;

                            mail.attachmentBytes = buffer;
                            //mail.FileName = "Invoices.pdf";
                            mail.FileName = strInvoiceFileName;

                            mail.DeleteFilesAfterSend = true;
                            mail.RequireAutentication = false;
                            mail.IsIncludeSignature = true;
                            // ES-33:Task#2: Need to update email configuration before calling send function
                            WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
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
                                        //invoiceIdsError.Add("Invoice #" + _ref.ToString());
                                        totalSendErr++;
                                    }
                                }
                                else
                                {
                                    mimeSentMessages.Add(mimeMessage);
                                    invoiceIdsSentEmail.Add("Invoice #" + InvoiceID);
                                }
                            }
                        }
                        else
                        {
                            totalSendErr++;
                            emailLog.To = string.Empty;
                            emailLog.Status = 0;
                            emailLog.UsrErrMessage = "Email address does not exist for this location";

                            _AddEmailLog.To = string.Empty;
                            _AddEmailLog.Status = 0;
                            _AddEmailLog.UsrErrMessage = "Email address does not exist for this location";

                            BL_EmailLog bL_EmailLog = new BL_EmailLog();

                            if (IsAPIIntegrationEnable == "YES")
                            {
                                string APINAME = "iCollectionsAPI/iCollectionsList_AddEmailLog";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddEmailLog);
                            }
                            else
                            {
                                bL_EmailLog.AddEmailLog(emailLog);
                            }
                        }
                    }
                }

                totalNotSend = totalInvoicesInList - totalInvoicesForEmail;
            }

            totalSentEmails = mimeSentMessages.Count;
            if (totalSentEmails > 0)
            {
                Mail mail = new Mail();
                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                if (mail.TakeASentEmailCopy)
                {
                    emailLog.Ref = 0;
                    if (totalSentEmails >= 10)
                    {
                        List<List<MimeKit.MimeMessage>> lstTenMessages = new List<List<MimeKit.MimeMessage>>();
                        while (mimeSentMessages.Any())
                        {
                            lstTenMessages.Add(mimeSentMessages.Take(11).ToList());
                            mimeSentMessages = mimeSentMessages.Skip(11).ToList();
                        }

                        foreach (var lst in lstTenMessages)
                        {
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
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnSentError", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                if (emailGetSentError != null)
                {
                    string str1 = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnGetSentError", "noty({text: '" + str1 + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                }
            }
            else
            {
                if (totalSentEmails > 0)
                {
                    var successfullMess = "There were " + totalSentEmails + " of "
                        + totalInvoicesInList.ToString() + " items sent out successfully.";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: '" + successfullMess + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);

                    if (totalSendErr > 0)
                    {
                        var errMess = "Total " + totalSendErr + " failed of "
                            + totalInvoicesInList.ToString() + " items could not be sent.";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnSendErr", "noty({text: '" + errMess + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);

                    }
                    if (totalNotSend > 0)
                    {
                        var notSentMess = "Total " + totalNotSend + " of "
                            + totalInvoicesInList + " items are not invoices.";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnNotSend", "noty({text: '" + notSentMess + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                    }

                    if (emailGetSentError != null)
                    {
                        string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarn2", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                    }
                }
                else
                {
                    string str = "There were no emails sent out.";

                    if (totalSendErr > 0)
                    {
                        str += "<br>Total " + totalSendErr + " failed of "
                            + totalInvoicesInList.ToString() + " items could not be sent.";
                    }
                    if (totalNotSend > 0)
                    {

                        str += "<br>Total " + totalNotSend + " of "
                            + totalInvoicesInList + " items are not invoices";
                    }
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnNoEmail", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                }
            }
            if (sbdInValidEmails.Length > 0)
            {
                sbdInValidEmails.Insert(0, "Invalid emails address:<br/>");
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnInValidEmails", "noty({text: '" + sbdInValidEmails.ToString() + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default', closable: false, dismissQueue: true});", true);
            }

            // Refresh Email History grid
            RadGrid_gvLogs.Rebind();
        }

        catch (Exception exp)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(exp.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }

    public static byte[] concatAndAddContent(List<byte[]> pdfByteContent)
    {
        MemoryStream ms = new MemoryStream();
        Document doc = new Document();
        PdfSmartCopy copy = new PdfSmartCopy(doc, ms);

        doc.Open();

        //Loop through each byte array
        foreach (var p in pdfByteContent)
        {
            PdfReader reader = new PdfReader(p);
            int n = reader.NumberOfPages;

            for (int i = 1; i <= n; i++)
            {
                byte[] red = reader.GetPageContent(i);
                if (red.Length < 1000)
                {
                    n = n - 1;
                }
            }
            for (int page = 0; page < n;)
            {
                copy.AddPage(copy.GetImportedPage(reader, ++page));
            }
        }
        doc.Close();
        //Return just before disposing
        return ms.ToArray();
    }

    private List<byte[]> PrintInvoicesForIndivudial(StiWebViewer rvInvoices, Int32 Ref)
    {

        List<byte[]> invoicesAsBytes = new List<byte[]>();
        try
        {
            Contracts objProp_Contracts = new Contracts();
            BL_Contracts objBL_Contracts = new BL_Contracts();
            BusinessEntity.User objPropUser = new BusinessEntity.User();
            BL_User objBL_User = new BL_User();
            BL_Report bL_Report = new BL_Report();

            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            _GetInvoicesByRef.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataTable _dtInvoice = new DataTable();
            DataSet _dsInvoice = new DataSet();

            int _ref = Ref;

            objProp_Contracts.InvoiceID = _ref;
            _GetInvoicesByRef.InvoiceID = _ref;

            if (IsAPIIntegrationEnable == "YES")
            {
                List<GetInvoicesByRefViewModel> _lstGetInvoicesByRef = new List<GetInvoicesByRefViewModel>();

                string APINAME = "iCollectionsAPI/iCollectionsList_GetInvoicesByRef";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoicesByRef);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetInvoicesByRef = serializer.Deserialize<List<GetInvoicesByRefViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetInvoicesByRefViewModel>(_lstGetInvoicesByRef);
            }
            else
            {
                ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);
            }


            _dtInvoice = ds.Tables[0];
            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            _GetControlBranch.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() != "TS")
            {
                if (IsAPIIntegrationEnable == "YES")
                {
                    List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

                    string APINAME = "iCollectionsAPI/iCollectionsList_GetControl";

                    _getConnectionConfig.ConnConfig = Session["config"].ToString();

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _GetControlViewModel = serializer.Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                    dsC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
                }
                else
                {
                    dsC = objBL_User.getControl(objPropUser);
                }
            }
            else
            {
                objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
                _GetControlBranch.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
                if (IsAPIIntegrationEnable == "YES")
                {
                    List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

                    string APINAME = "iCollectionsAPI/iCollectionsList_GetControlBranch";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetControlBranch);

                    _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                    dsC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
                }
                else
                {
                    dsC = objBL_User.getControlBranch(objPropUser);
                }
            }

            int _days = 0;
            for (int i = 0; i < _dtInvoice.Rows.Count; i++)
            {

                #region Determine Pay Terms
                if (_dtInvoice.Rows[i]["payterms"].ToString() == "0")
                {
                    _days = 0;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "1")
                {
                    _days = 10;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "2")
                {
                    _days = 15;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "3")
                {
                    _days = 30;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "4")
                {
                    _days = 45;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "5")
                {
                    _days = 60;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "6")
                {
                    _days = 30;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "7")
                {
                    _days = 90;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "8")
                {
                    _days = 180;
                }
                else if (_dtInvoice.Rows[i]["payterms"].ToString() == "9")
                {
                    _days = 0;
                }
                #endregion
                if (!string.IsNullOrEmpty(_dtInvoice.Rows[i]["IDate"].ToString()))
                {
                    _dtInvoice.Rows[i]["DueDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString()).AddDays(_days);
                    _dtInvoice.Rows[i]["fDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString());
                }
            }
            #region Get Company Address

            string companySignature = GetCompanySignature(dsC);

            var mailContent = "";
            if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("Adams", StringComparison.InvariantCultureIgnoreCase))
            {
                mailContent = "Cher client : " + Environment.NewLine + "Veuillez consulter la facture ci-jointe pour paiement. " + Environment.NewLine +
                            "Veuillez noter qu’il peut y avoir plusieurs factures contenues " + Environment.NewLine +
                            "dans chaque pièce jointe. Si vous avez besoin de clarifications, " + Environment.NewLine +
                            "n’hésitez pas à nous contacter.  " + Environment.NewLine + Environment.NewLine +

                            "Nous vous remercions d'avoir fair affaire avec notre entreprise." + Environment.NewLine + Environment.NewLine +


                            "Dear Valued Customer: " + Environment.NewLine + Environment.NewLine +

                            "Please review the attached invoice(s) for processing." + Environment.NewLine +
                            "Please note there may be multiple invoices contained " + Environment.NewLine +
                            "in each attachment. Should you have any questions, " + Environment.NewLine +
                            "Please feel free to contact us." + Environment.NewLine + Environment.NewLine +
                            "We appreciate your business!" + Environment.NewLine + Environment.NewLine;

            }
            else if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("Brock", StringComparison.InvariantCultureIgnoreCase))
            {
                //
                StringBuilder addressBrock = new StringBuilder();

                addressBrock.AppendFormat("{0}", ds.Tables[0].Rows[0]["customerName"].ToString());
                addressBrock.AppendLine();
                addressBrock.AppendLine();
                //addressBrock.Append("<br/><br/>");
                //addressBrock.AppendLine("Thank you for giving us the opportunity to serve you. Attached to this email");
                addressBrock.AppendLine("Please review your past due invoices and reply stating payment status. Attached to this email");
                addressBrock.AppendFormat("is invoice {0}.", Ref.ToString());
                addressBrock.AppendLine();
                addressBrock.AppendLine();
                //addressBrock.Append("<br/><br/>");
                addressBrock.Append("Kind Regards,");
                addressBrock.AppendLine();
                addressBrock.AppendLine();
                //addressBrock.Append("<br/><br/>");


                mailContent = addressBrock.ToString();
            }
            else
            {
                mailContent = "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine;
            }

            ViewState["CompanyAddress"] = companySignature;
            ViewState["MailContent"] = mailContent;

            ViewState["EmailFrom"] = "";
            if (Session["MSM"].ToString() != "TS")
            {
                ViewState["EmailFrom"] = dsC.Tables[0].Rows[0]["Email"].ToString();
            }
            #endregion
            ViewState["InvoiceReport"] = _dtInvoice;
            ViewState["CompanyReport"] = dsC.Tables[0];
            Session["InvoiceReportDetails"] = _dtInvoice;

            //rvInvoices.LocalReport.DataSources.Clear();
            DataTable dt = (DataTable)ViewState["InvoiceReport"];
            DataTable dtCompany = (DataTable)ViewState["CompanyReport"];

            DataTable _dtInvItems1 = GetInvoiceItems(Ref);


            string reportPathStimul = string.Empty;

            reportPathStimul = Server.MapPath("StimulsoftReports/Invoices/" + ConfigurationManager.AppSettings["InvoiceReport"].ToString());

            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            report.Compile();

            DataSet companyLogo = new DataSet();

            //if (IsAPIIntegrationEnable == "YES")
            //{
            //    List<GetCompanyDetailsViewModel> _GetCompanyDetailsViewModel = new List<GetCompanyDetailsViewModel>();

            //    string APINAME = "iCollectionsAPI/iCollectionsList_GetCompanyDetails";

            //    _GetCompanyDetails.ConnConfig = Session["config"].ToString();

            //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCompanyDetails);
            //    JavaScriptSerializer serializer = new JavaScriptSerializer();

            //    serializer.MaxJsonLength = Int32.MaxValue;
            //    _GetCompanyDetailsViewModel = serializer.Deserialize<List<GetCompanyDetailsViewModel>>(_APIResponse.ResponseData);
            //    companyLogo = CommonMethods.ToDataSet<GetCompanyDetailsViewModel>(_GetCompanyDetailsViewModel);
            //}
            //else
            //{
            companyLogo = bL_Report.GetCompanyDetails(Session["config"].ToString());
            //}

            var imageString = companyLogo.Tables[0].Rows[0]["Logo"].ToString();
            byte[] barrImg = (byte[])(companyLogo.Tables[0].Rows[0]["Logo"]);
            string strfn = Convert.ToString(Server.MapPath(Request.ApplicationPath) + "/TempImages/" + DateTime.Now.ToFileTime().ToString());
            FileStream fs = new FileStream(strfn,
                              FileMode.CreateNew, FileAccess.Write);
            fs.Write(barrImg, 0, barrImg.Length);
            fs.Flush();
            fs.Close();


            System.Uri uri = new Uri(strfn);
            DataTable cTable = BuildCompanyDetailsTable();
            var cRow = cTable.NewRow();
            cRow["LogoURL"] = uri.AbsolutePath;
            cRow["CompanyName"] = companyLogo.Tables[0].Rows[0]["Name"].ToString();
            cRow["CompanyAddress"] = companyLogo.Tables[0].Rows[0]["Address"].ToString();
            cRow["ContactNo"] = companyLogo.Tables[0].Rows[0]["Contact"].ToString();
            cRow["Email"] = companyLogo.Tables[0].Rows[0]["Email"].ToString();

            cRow["City"] = companyLogo.Tables[0].Rows[0]["City"].ToString();
            cRow["State"] = companyLogo.Tables[0].Rows[0]["State"].ToString();
            cRow["Phone"] = companyLogo.Tables[0].Rows[0]["Phone"].ToString();
            cRow["Fax"] = companyLogo.Tables[0].Rows[0]["Fax"].ToString();
            cRow["Zip"] = companyLogo.Tables[0].Rows[0]["Zip"].ToString();


            cTable.Rows.Add(cRow);

            DataSet CompanyDetails = new DataSet();
            cTable.TableName = "CompanyDetails";
            CompanyDetails.Tables.Add(cTable);
            CompanyDetails.DataSetName = "CompanyDetails";


            DataSet Invoices = new DataSet();
            DataTable dtInvoice1 = _dtInvoice.Copy();
            dtInvoice1.TableName = "Invoices";
            Invoices.Tables.Add(dtInvoice1.Copy());
            Invoices.DataSetName = "Invoices";

            DataSet InvoiceItems = new DataSet();
            DataTable dtIInvItems = _dtInvItems1.Copy();
            dtIInvItems.TableName = "InvoiceItems";
            InvoiceItems.Tables.Add(dtIInvItems);
            InvoiceItems.DataSetName = "InvoiceItems";


            DataSet Ticket_Company = new DataSet();
            DataTable dtTicketCompany = new DataTable();
            dtTicketCompany = dsC.Tables[0].Copy();
            Ticket_Company.Tables.Add(dtTicketCompany);
            dtTicketCompany.TableName = "Ticket_Company";
            Ticket_Company.DataSetName = "Ticket_Company";


            DataSet Invoice_dtInvoice = new DataSet();
            DataTable dtInvoice = new DataTable();
            dtInvoice = ds.Tables[0].Copy();
            Invoice_dtInvoice.Tables.Add(dtInvoice);
            dtInvoice.TableName = "Invoice_dtInvoice";
            Invoice_dtInvoice.DataSetName = "Invoice_dtInvoice";

            report.RegData("Invoices", Invoices);
            report.RegData("CompanyDetails", CompanyDetails);

            report.RegData("Invoice_dtInvoice", Invoice_dtInvoice);

            report.RegData("Ticket_Company", Ticket_Company);
            report.RegData("InvoiceItems", InvoiceItems);
            report.Dictionary.Synchronize();
            report.Render();
            rvInvoices.Report = report;
            byte[] buffer1 = null;
            var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            var service = new Stimulsoft.Report.Export.StiPdfExportService();
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            service.ExportTo(rvInvoices.Report, stream, settings);
            buffer1 = stream.ToArray();
            invoicesAsBytes.Add(buffer1);

            return invoicesAsBytes;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return invoicesAsBytes;
        }
    }

    private DataTable GetInvoiceItems(int _refId)
    {
        DataTable _dtItem = new DataTable();
        try
        {
            BL_Contracts objBL_Contracts = new BL_Contracts();
            Contracts objProp_Contracts = new Contracts();


            objProp_Contracts.InvoiceID = _refId;
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            DataSet _dsItemDetails = new DataSet();
            if (IsAPIIntegrationEnable == "YES")
            {
                List<GetInvoiceItemByRefViewModel> _lstGetInvoiceItemByRef = new List<GetInvoiceItemByRefViewModel>();

                string APINAME = "iCollectionsAPI/iCollectionsList_GetInvoiceItemByRef";

                _GetInvoiceItemByRef.InvoiceID = _refId;
                _GetInvoiceItemByRef.ConnConfig = Session["config"].ToString();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoiceItemByRef);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetInvoiceItemByRef = serializer.Deserialize<List<GetInvoiceItemByRefViewModel>>(_APIResponse.ResponseData);
                _dsItemDetails = CommonMethods.ToDataSet<GetInvoiceItemByRefViewModel>(_lstGetInvoiceItemByRef);
            }
            else
            {
                _dsItemDetails = objBL_Contracts.GetInvoiceItemByRef(objProp_Contracts);
            }

            if (_dsItemDetails.Tables[0].Rows.Count < 1)
            {
                _dtItem = LoadInvoiceDetails(_dsItemDetails.Tables[0], _refId);
            }
            else
                _dtItem = _dsItemDetails.Tables[0];
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

        return _dtItem;
    }

    private DataTable LoadInvoiceDetails(DataTable _dt, int _idRef)
    {
        DataRow _dr = _dt.NewRow();
        _dr["Ref"] = _idRef;
        _dr["Acct"] = 0;
        _dr["Quan"] = 0;
        _dr["fDesc"] = string.Empty;
        _dr["Price"] = 0.00;
        _dr["Amount"] = 0.00;
        _dr["STax"] = 0.00;
        _dr["billcode"] = string.Empty;
        _dr["staxAmt"] = 0.00;
        _dr["balance"] = 0.00;
        _dr["amtpaid"] = 0.00;
        _dr["total"] = 0.00;
        _dt.Rows.Add(_dr);
        return _dt;
    }

    protected DataTable BuildCompanyDetailsTable()
    {
        DataTable companyDetailsTable = new DataTable();
        companyDetailsTable.Columns.Add("CompanyAddress");
        companyDetailsTable.Columns.Add("CompanyName");
        companyDetailsTable.Columns.Add("ContactNo");
        companyDetailsTable.Columns.Add("Email");
        companyDetailsTable.Columns.Add("LogoURL");
        companyDetailsTable.Columns.Add("City");
        companyDetailsTable.Columns.Add("State");
        companyDetailsTable.Columns.Add("Zip");
        companyDetailsTable.Columns.Add("Fax");
        companyDetailsTable.Columns.Add("Phone");
        return companyDetailsTable;
    }

    protected void lnkPDFCustomerStatements_Click(object sender, EventArgs e)
    {
        try
        {
            count = 0;
            Contracts objProp_Contracts = new Contracts();
            BL_Contracts objBL_Contracts = new BL_Contracts();
            BusinessEntity.User objPropUser = new BusinessEntity.User();
            BL_User objBL_User = new BL_User();
            BL_Report bL_Report = new BL_Report();

            #region Collect All The Loc

            List<string> locIDs = new List<string>();
            //foreach (GridDataItem gr in RadGrid_Collections.Items)
            //{
            //    HiddenField hdLoc = (HiddenField)gr.FindControl("hdLoc");
            //    if (!string.IsNullOrEmpty(hdLoc.Value))
            //    {
            //        locIDs.Add(hdLoc.Value);
            //    }
            //}
            DataTable dtAR = (DataTable)Session["CollectionARData"];
            foreach (DataRow rowItem in dtAR.Rows)
            {
                if (!string.IsNullOrEmpty(rowItem["Loc"].ToString()))
                {
                    locIDs.Add(rowItem["Loc"].ToString());
                }
            }

            var distinctIDs = locIDs.Distinct();

            #endregion

            #region Company Detail
            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();

            if (IsAPIIntegrationEnable == "YES")
            {
                List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

                string APINAME = "iCollectionsAPI/iCollectionsList_GetControl";

                _getConnectionConfig.ConnConfig = Session["config"].ToString();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _GetControlViewModel = serializer.Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                dsC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
            }
            else
            {
                dsC = objBL_User.getControl(objPropUser);
            }

            #endregion

            DataTable dt = new DataTable();
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.LocationIDs = string.Join(",", distinctIDs);
            objProp_Contracts.UserID = Session["userid"].ToString();

            DataSet ds = new DataSet();

            if (IsAPIIntegrationEnable == "YES")
            {
                List<GetCustomerStatementByLocViewModel> _lstGetCustomerStatementByLoc = new List<GetCustomerStatementByLocViewModel>();

                string APINAME = "iCollectionsAPI/iCollectionsList_GetCustomerStatementByLocs";

                _GetCustomerStatementByLocs.ConnConfig = Session["config"].ToString();
                _GetCustomerStatementByLocs.LocationIDs = string.Join(",", distinctIDs);
                _GetCustomerStatementByLocs.UserID = Session["userid"].ToString();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomerStatementByLocs);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetCustomerStatementByLoc = serializer.Deserialize<List<GetCustomerStatementByLocViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetCustomerStatementByLocViewModel>(_lstGetCustomerStatementByLoc);
            }
            else
            {
                ds = objBL_Contracts.GetCustomerStatementByLocs(objProp_Contracts);
            }

            dt = ds.Tables[0];
            ViewState["Invoices"] = dt;

            byte[] getPDF = null;
            string report = System.Web.Configuration.WebConfigurationManager.AppSettings["CustomerInvoieStatement"];

            if (!string.IsNullOrEmpty(report.Trim()) && report.Contains(".mrt"))
            {
                DataSet dsItem = new DataSet();

                if (IsAPIIntegrationEnable == "YES")
                {
                    List<GetCustStatementInvSouthernViewModel> _lstGetCustStatementInvSouthern = new List<GetCustStatementInvSouthernViewModel>();

                    string APINAME = "iCollectionsAPI/iCollectionsList_GetCustomerStatementInvoicesByLocation";

                    _GetCustStatementInvByLocation.ConnConfig = Session["config"].ToString();
                    _GetCustStatementInvByLocation.LocationIDs = string.Join(",", distinctIDs);
                    _GetCustStatementInvByLocation.includeCredit = true;

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustStatementInvByLocation);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetCustStatementInvSouthern = serializer.Deserialize<List<GetCustStatementInvSouthernViewModel>>(_APIResponse.ResponseData);
                    dsItem = CommonMethods.ToDataSet<GetCustStatementInvSouthernViewModel>(_lstGetCustStatementInvSouthern);
                }
                else
                {
                    dsItem = objBL_Contracts.GetCustomerStatementInvoicesByLocation(objProp_Contracts, true);
                }

                StiReport stiReport = PrintMRTReport(dsC.Tables[0], ds.Tables[0], dsItem.Tables[0]);

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(stiReport, stream, settings);
                getPDF = stream.ToArray();
            }
            else
            {
                ReportViewer rvCustomer = PrintRDLCReport(dsC.Tables[0], ds.Tables[0]);
                getPDF = ExportReportToPDF1("", rvCustomer);
            }

            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=CustomerStatement.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (getPDF.Length).ToString());
            Response.BinaryWrite(getPDF);
            Response.Flush();
            Response.Close();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkPDFInvoices_Click(object sender, EventArgs e)
    {
        try
        {
            StiWebViewer rvInvoices = new StiWebViewer();

            List<byte[]> invoicesToPrint = PrintInvoices(rvInvoices);

            string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF", "Invoices.pdf");

            if (invoicesToPrint != null)
            {
                byte[] buffer1 = null;

                buffer1 = concatAndAddContent(invoicesToPrint);

                if (File.Exists(filename))
                    File.Delete(filename);
                using (var fs = new FileStream(filename, FileMode.Create))
                {
                    fs.Write(buffer1, 0, buffer1.Length);
                    fs.Close();
                }
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=Invoices.pdf");
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Length", (buffer1.Length).ToString());
                Response.BinaryWrite(buffer1);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private List<byte[]> PrintInvoices(StiWebViewer rvInvoices)
    {

        List<byte[]> invoicesAsBytes = new List<byte[]>();
        try
        {
            Contracts objProp_Contracts = new Contracts();
            BL_Contracts objBL_Contracts = new BL_Contracts();
            BusinessEntity.User objPropUser = new BusinessEntity.User();
            BL_User objBL_User = new BL_User();
            BL_Report bL_Report = new BL_Report();


            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            _GetInvoicesByRef.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataTable dtNew = (DataTable)Session["InvoiceSrch"];
            DataTable _dtInvoice = new DataTable();
            DataSet _dsInvoice = new DataSet();
            int j = 0;

            DataTable dtAR = (DataTable)Session["CollectionARData"];


            foreach (DataRow rowItem in dtAR.Rows)
            {
                //HiddenField hdType = (HiddenField)gr.FindControl("hdType");
                //HiddenField hdRef = (HiddenField)gr.FindControl("hdRef");
                //if (hdType.Value == "1")
                if (rowItem["type"].ToString() == "1")
                {
                    int _ref = Convert.ToInt32(rowItem["Ref"]);

                    objProp_Contracts.InvoiceID = _ref;
                    _GetInvoicesByRef.InvoiceID = _ref;

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        List<GetInvoicesByRefViewModel> _lstGetInvoicesByRef = new List<GetInvoicesByRefViewModel>();

                        string APINAME = "iCollectionsAPI/iCollectionsList_GetInvoicesByRef";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoicesByRef);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        serializer.MaxJsonLength = Int32.MaxValue;

                        _lstGetInvoicesByRef = serializer.Deserialize<List<GetInvoicesByRefViewModel>>(_APIResponse.ResponseData);
                        ds = CommonMethods.ToDataSet<GetInvoicesByRefViewModel>(_lstGetInvoicesByRef);
                    }
                    else
                    {
                        ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);
                    }

                    _dtInvoice = ds.Tables[0];
                    DataSet dsC = new DataSet();
                    objPropUser.ConnConfig = Session["config"].ToString();

                    _GetControlBranch.ConnConfig = Session["config"].ToString();

                    if (Session["MSM"].ToString() != "TS")
                    {
                        if (IsAPIIntegrationEnable == "YES")
                        {
                            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

                            string APINAME = "iCollectionsAPI/iCollectionsList_GetControl";

                            _getConnectionConfig.ConnConfig = Session["config"].ToString();

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                            JavaScriptSerializer serializer = new JavaScriptSerializer();

                            serializer.MaxJsonLength = Int32.MaxValue;

                            _GetControlViewModel = serializer.Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                            dsC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
                        }
                        else
                        {
                            dsC = objBL_User.getControl(objPropUser);
                        }
                    }
                    else
                    {
                        objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
                        _GetControlBranch.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
                        if (IsAPIIntegrationEnable == "YES")
                        {
                            List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

                            string APINAME = "iCollectionsAPI/iCollectionsList_GetControlBranch";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetControlBranch);

                            _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                            dsC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
                        }
                        else
                        {
                            dsC = objBL_User.getControlBranch(objPropUser);
                        }
                    }

                    int _days = 0;
                    for (int i = 0; i < _dtInvoice.Rows.Count; i++)
                    {

                        #region Determine Pay Terms
                        if (_dtInvoice.Rows[i]["payterms"].ToString() == "0")
                        {
                            _days = 0;
                        }
                        else if (_dtInvoice.Rows[i]["payterms"].ToString() == "1")
                        {
                            _days = 10;
                        }
                        else if (_dtInvoice.Rows[i]["payterms"].ToString() == "2")
                        {
                            _days = 15;
                        }
                        else if (_dtInvoice.Rows[i]["payterms"].ToString() == "3")
                        {
                            _days = 30;
                        }
                        else if (_dtInvoice.Rows[i]["payterms"].ToString() == "4")
                        {
                            _days = 45;
                        }
                        else if (_dtInvoice.Rows[i]["payterms"].ToString() == "5")
                        {
                            _days = 60;
                        }
                        else if (_dtInvoice.Rows[i]["payterms"].ToString() == "6")
                        {
                            _days = 30;
                        }
                        else if (_dtInvoice.Rows[i]["payterms"].ToString() == "7")
                        {
                            _days = 90;
                        }
                        else if (_dtInvoice.Rows[i]["payterms"].ToString() == "8")
                        {
                            _days = 180;
                        }
                        else if (_dtInvoice.Rows[i]["payterms"].ToString() == "9")
                        {
                            _days = 0;
                        }
                        #endregion
                        if (!string.IsNullOrEmpty(_dtInvoice.Rows[i]["IDate"].ToString()))
                        {
                            _dtInvoice.Rows[i]["DueDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString()).AddDays(_days);
                            _dtInvoice.Rows[i]["fDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString());
                        }
                    }
                    #region Get Company Address
                    string companySignature = GetCompanySignature(dsC);

                    //string address = dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
                    //address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
                    //address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
                    //address += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
                    //address += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;
                    //address += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;

                    var mailContent = "";
                    if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("Adams", StringComparison.InvariantCultureIgnoreCase))
                    {
                        mailContent = "Cher client : " + Environment.NewLine + "Veuillez consulter la facture ci-jointe pour paiement. " + Environment.NewLine +
                            "Veuillez noter qu’il peut y avoir plusieurs factures contenues " + Environment.NewLine +
                            "dans chaque pièce jointe. Si vous avez besoin de clarifications, " + Environment.NewLine +
                            "n’hésitez pas à nous contacter.  " + Environment.NewLine + Environment.NewLine +

                            "Nous vous remercions d'avoir fair affaire avec notre entreprise." + Environment.NewLine + Environment.NewLine +


                            "Dear Valued Customer: " + Environment.NewLine + Environment.NewLine +

                            "Please review the attached invoice(s) for processing." + Environment.NewLine +
                            "Please note there may be multiple invoices contained " + Environment.NewLine +
                            "in each attachment. Should you have any questions, " + Environment.NewLine +
                            "Please feel free to contact us." + Environment.NewLine + Environment.NewLine +
                            "We appreciate your business!" + Environment.NewLine + Environment.NewLine;

                    }
                    else if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("Brock", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //
                        StringBuilder addressBrock = new StringBuilder();

                        addressBrock.AppendFormat("{0}", ds.Tables[0].Rows[0]["customerName"].ToString());
                        addressBrock.AppendLine();
                        addressBrock.AppendLine();
                        //addressBrock.Append("<br/><br/>");
                        //addressBrock.AppendLine("Thank you for giving us the opportunity to serve you. Please see the attached invoices.");
                        addressBrock.AppendLine("Please review your past due invoices and reply stating payment status. Please see the attached invoices.");
                        
                        addressBrock.AppendLine();
                        addressBrock.AppendLine();
                        //addressBrock.Append("<br/><br/>");
                        addressBrock.Append("Kind Regards,");
                        addressBrock.AppendLine();
                        addressBrock.AppendLine();
                        //addressBrock.Append("<br/><br/>");

                        mailContent = addressBrock.ToString();
                    }
                    else
                    {
                        mailContent = "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine;
                    }

                    ViewState["CompanyAddress"] = companySignature;
                    ViewState["MailContent"] = mailContent;

                    ViewState["EmailFrom"] = "";
                    if (Session["MSM"].ToString() != "TS")
                    {
                        ViewState["EmailFrom"] = dsC.Tables[0].Rows[0]["Email"].ToString();
                    }
                    #endregion
                    ViewState["InvoiceReport"] = _dtInvoice;
                    ViewState["CompanyReport"] = dsC.Tables[0];
                    Session["InvoiceReportDetails"] = _dtInvoice;


                    DataTable dt = (DataTable)ViewState["InvoiceReport"];
                    DataTable dtCompany = (DataTable)ViewState["CompanyReport"];
                    int refId = Convert.ToInt32(rowItem["Ref"]);
                    DataTable _dtInvItems1 = GetInvoiceItems(refId);


                    string reportPathStimul = string.Empty;

                    reportPathStimul = Server.MapPath("StimulsoftReports/Invoices/" + ConfigurationManager.AppSettings["InvoiceReport"].ToString());

                    StiReport report = new StiReport();
                    report.Load(reportPathStimul);
                    report.Compile();

                    DataSet companyLogo = new DataSet();

                    //if (IsAPIIntegrationEnable == "YES")
                    //{
                    //    List<GetCompanyDetailsViewModel> _GetCompanyDetailsViewModel = new List<GetCompanyDetailsViewModel>();

                    //    string APINAME = "iCollectionsAPI/iCollectionsList_GetCompanyDetails";

                    //    _GetCompanyDetails.ConnConfig = Session["config"].ToString();

                    //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCompanyDetails);
                    //    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    //    serializer.MaxJsonLength = Int32.MaxValue;

                    //    object jsondata = Newtonsoft.Json.JsonConvert.DeserializeObject(_APIResponse.ResponseData);
                    //    string jsondata1 = Newtonsoft.Json.JsonConvert.SerializeObject(jsondata);

                    //    _GetCompanyDetailsViewModel = serializer.Deserialize<List<GetCompanyDetailsViewModel>>(_APIResponse.ResponseData);
                    //    companyLogo = CommonMethods.ToDataSet<GetCompanyDetailsViewModel>(_GetCompanyDetailsViewModel);
                    //}
                    //else
                    //{
                    companyLogo = bL_Report.GetCompanyDetails(Session["config"].ToString());
                    //}

                    var imageString = companyLogo.Tables[0].Rows[0]["Logo"].ToString();
                    byte[] barrImg = (byte[])(companyLogo.Tables[0].Rows[0]["Logo"]);
                    string strfn = Convert.ToString(Server.MapPath(Request.ApplicationPath) + "/TempImages/" + DateTime.Now.ToFileTime().ToString());
                    FileStream fs = new FileStream(strfn,
                                      FileMode.CreateNew, FileAccess.Write);
                    fs.Write(barrImg, 0, barrImg.Length);
                    fs.Flush();
                    fs.Close();

                    System.Uri uri = new Uri(strfn);
                    DataTable cTable = BuildCompanyDetailsTable();
                    var cRow = cTable.NewRow();
                    cRow["LogoURL"] = uri.AbsolutePath;
                    cRow["CompanyName"] = companyLogo.Tables[0].Rows[0]["Name"].ToString();
                    cRow["CompanyAddress"] = companyLogo.Tables[0].Rows[0]["Address"].ToString();
                    cRow["ContactNo"] = companyLogo.Tables[0].Rows[0]["Contact"].ToString();
                    cRow["Email"] = companyLogo.Tables[0].Rows[0]["Email"].ToString();

                    cRow["City"] = companyLogo.Tables[0].Rows[0]["City"].ToString();
                    cRow["State"] = companyLogo.Tables[0].Rows[0]["State"].ToString();
                    cRow["Phone"] = companyLogo.Tables[0].Rows[0]["Phone"].ToString();
                    cRow["Fax"] = companyLogo.Tables[0].Rows[0]["Fax"].ToString();
                    cRow["Zip"] = companyLogo.Tables[0].Rows[0]["Zip"].ToString();


                    cTable.Rows.Add(cRow);

                    DataSet CompanyDetails = new DataSet();
                    cTable.TableName = "CompanyDetails";
                    CompanyDetails.Tables.Add(cTable);
                    CompanyDetails.DataSetName = "CompanyDetails";


                    DataSet Invoices = new DataSet();
                    DataTable dtInvoice1 = _dtInvoice.Copy();
                    dtInvoice1.TableName = "Invoices";
                    Invoices.Tables.Add(dtInvoice1.Copy());
                    Invoices.DataSetName = "Invoices";

                    DataSet InvoiceItems = new DataSet();
                    DataTable dtIInvItems = _dtInvItems1.Copy();
                    dtIInvItems.TableName = "InvoiceItems";
                    InvoiceItems.Tables.Add(dtIInvItems);
                    InvoiceItems.DataSetName = "InvoiceItems";


                    DataSet Ticket_Company = new DataSet();
                    DataTable dtTicketCompany = new DataTable();
                    dtTicketCompany = dsC.Tables[0].Copy();
                    Ticket_Company.Tables.Add(dtTicketCompany);
                    dtTicketCompany.TableName = "Ticket_Company";
                    Ticket_Company.DataSetName = "Ticket_Company";


                    DataSet Invoice_dtInvoice = new DataSet();
                    DataTable dtInvoice = new DataTable();
                    dtInvoice = ds.Tables[0].Copy();
                    Invoice_dtInvoice.Tables.Add(dtInvoice);
                    dtInvoice.TableName = "Invoice_dtInvoice";
                    Invoice_dtInvoice.DataSetName = "Invoice_dtInvoice";

                    report.RegData("Invoices", Invoices);
                    report.RegData("CompanyDetails", CompanyDetails);

                    report.RegData("Invoice_dtInvoice", Invoice_dtInvoice);

                    report.RegData("Ticket_Company", Ticket_Company);
                    report.RegData("InvoiceItems", InvoiceItems);
                    report.Dictionary.Synchronize();
                    report.Render();
                    rvInvoices.Report = report;
                    byte[] buffer1 = null;
                    var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                    var service = new Stimulsoft.Report.Export.StiPdfExportService();
                    System.IO.MemoryStream stream = new System.IO.MemoryStream();
                    service.ExportTo(rvInvoices.Report, stream, settings);
                    buffer1 = stream.ToArray();
                    invoicesAsBytes.Add(buffer1);

                    j++;
                }
            }
            return invoicesAsBytes;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return invoicesAsBytes;
        }
    }

    private List<byte[]> PrintInvoicesSelected(StiWebViewer rvInvoices)
    {

        List<byte[]> invoicesAsBytes = new List<byte[]>();
        try
        {
            Contracts objProp_Contracts = new Contracts();
            BL_Contracts objBL_Contracts = new BL_Contracts();
            BusinessEntity.User objPropUser = new BusinessEntity.User();
            BL_User objBL_User = new BL_User();
            BL_Report bL_Report = new BL_Report();


            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            _GetInvoicesByRef.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() == "TS")
            {
                if (Session["type"].ToString() != "c")
                    objProp_Contracts.isTS = 1;
            }

            DataTable dtNew = (DataTable)Session["InvoiceSrch"];
            DataTable _dtInvoice = new DataTable();
            DataSet _dsInvoice = new DataSet();
            int j = 0;

            foreach (GridDataItem gr in RadGrid_Collections.SelectedItems)
            {
                HiddenField hdType = (HiddenField)gr.FindControl("hdType");
                HiddenField hdRef = (HiddenField)gr.FindControl("hdRef");

                if (hdType.Value == "1")
                {
                    int _ref = Convert.ToInt32(hdRef.Value);

                    objProp_Contracts.InvoiceID = _ref;
                    _GetInvoicesByRef.InvoiceID = _ref;

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        List<GetInvoicesByRefViewModel> _lstGetInvoicesByRef = new List<GetInvoicesByRefViewModel>();

                        string APINAME = "iCollectionsAPI/iCollectionsList_GetInvoicesByRef";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoicesByRef);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        serializer.MaxJsonLength = Int32.MaxValue;

                        _lstGetInvoicesByRef = serializer.Deserialize<List<GetInvoicesByRefViewModel>>(_APIResponse.ResponseData);
                        ds = CommonMethods.ToDataSet<GetInvoicesByRefViewModel>(_lstGetInvoicesByRef);
                    }
                    else
                    {
                        ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);
                    }

                    _dtInvoice = ds.Tables[0];
                    DataSet dsC = new DataSet();
                    objPropUser.ConnConfig = Session["config"].ToString();

                    _GetControlBranch.ConnConfig = Session["config"].ToString();

                    if (Session["MSM"].ToString() != "TS")
                    {
                        if (IsAPIIntegrationEnable == "YES")
                        {
                            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

                            string APINAME = "iCollectionsAPI/iCollectionsList_GetControl";

                            _getConnectionConfig.ConnConfig = Session["config"].ToString();

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                            JavaScriptSerializer serializer = new JavaScriptSerializer();

                            serializer.MaxJsonLength = Int32.MaxValue;

                            _GetControlViewModel = serializer.Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                            dsC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
                        }
                        else
                        {
                            dsC = objBL_User.getControl(objPropUser);
                        }
                    }
                    else
                    {
                        objPropUser.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);
                        _GetControlBranch.LocID = Convert.ToInt32(ds.Tables[0].Rows[0]["loc"]);

                        if (IsAPIIntegrationEnable == "YES")
                        {
                            List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

                            string APINAME = "iCollectionsAPI/iCollectionsList_GetControlBranch";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetControlBranch);

                            _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                            dsC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
                        }
                        else
                        {
                            dsC = objBL_User.getControlBranch(objPropUser);
                        }
                    }

                    int _days = 0;
                    for (int i = 0; i < _dtInvoice.Rows.Count; i++)
                    {

                        #region Determine Pay Terms
                        if (_dtInvoice.Rows[i]["payterms"].ToString() == "0")
                        {
                            _days = 0;
                        }
                        else if (_dtInvoice.Rows[i]["payterms"].ToString() == "1")
                        {
                            _days = 10;
                        }
                        else if (_dtInvoice.Rows[i]["payterms"].ToString() == "2")
                        {
                            _days = 15;
                        }
                        else if (_dtInvoice.Rows[i]["payterms"].ToString() == "3")
                        {
                            _days = 30;
                        }
                        else if (_dtInvoice.Rows[i]["payterms"].ToString() == "4")
                        {
                            _days = 45;
                        }
                        else if (_dtInvoice.Rows[i]["payterms"].ToString() == "5")
                        {
                            _days = 60;
                        }
                        else if (_dtInvoice.Rows[i]["payterms"].ToString() == "6")
                        {
                            _days = 30;
                        }
                        else if (_dtInvoice.Rows[i]["payterms"].ToString() == "7")
                        {
                            _days = 90;
                        }
                        else if (_dtInvoice.Rows[i]["payterms"].ToString() == "8")
                        {
                            _days = 180;
                        }
                        else if (_dtInvoice.Rows[i]["payterms"].ToString() == "9")
                        {
                            _days = 0;
                        }
                        #endregion
                        if (!string.IsNullOrEmpty(_dtInvoice.Rows[i]["IDate"].ToString()))
                        {
                            _dtInvoice.Rows[i]["DueDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString()).AddDays(_days);
                            _dtInvoice.Rows[i]["fDate"] = DateTime.Parse(_dtInvoice.Rows[i]["IDate"].ToString());
                        }
                    }
                    #region Get Company Address
                    string companySignature = GetCompanySignature(dsC);

                    //string address = dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
                    //address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
                    //address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
                    //address += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
                    //address += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;
                    //address += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;

                    var mailContent = "";
                    if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("Adams", StringComparison.InvariantCultureIgnoreCase))
                    {
                        mailContent = "Cher client : " + Environment.NewLine + "Veuillez consulter la facture ci-jointe pour paiement. " + Environment.NewLine +
                            "Veuillez noter qu’il peut y avoir plusieurs factures contenues " + Environment.NewLine +
                            "dans chaque pièce jointe. Si vous avez besoin de clarifications, " + Environment.NewLine +
                            "n’hésitez pas à nous contacter.  " + Environment.NewLine + Environment.NewLine +

                            "Nous vous remercions d'avoir fair affaire avec notre entreprise." + Environment.NewLine + Environment.NewLine +


                            "Dear Valued Customer: " + Environment.NewLine + Environment.NewLine +

                            "Please review the attached invoice(s) for processing." + Environment.NewLine +
                            "Please note there may be multiple invoices contained " + Environment.NewLine +
                            "in each attachment. Should you have any questions, " + Environment.NewLine +
                            "Please feel free to contact us." + Environment.NewLine + Environment.NewLine +
                            "We appreciate your business!" + Environment.NewLine + Environment.NewLine;

                    }
                    else if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("Brock", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //
                        StringBuilder addressBrock = new StringBuilder();

                        addressBrock.AppendFormat("{0}", ds.Tables[0].Rows[0]["customerName"].ToString());
                        addressBrock.AppendLine();
                        addressBrock.AppendLine();
                        //addressBrock.Append("<br/><br/>");
                        //addressBrock.AppendLine("Thank you for giving us the opportunity to serve you. Please see the attached invoices.");
                        addressBrock.AppendLine("Please review your past due invoices and reply stating payment status. Please see the attached invoices.");

                        
                        //addressBrock.AppendFormat("is invoice {0}.", Request.QueryString["uid"].ToString());
                        addressBrock.AppendLine();
                        addressBrock.AppendLine();
                        //addressBrock.Append("<br/><br/>");
                        addressBrock.Append("Kind Regards,");
                        addressBrock.AppendLine();
                        addressBrock.AppendLine();
                        //addressBrock.Append("<br/><br/>");

                        
                        mailContent = addressBrock.ToString();
                    }
                    else
                    {
                        mailContent = "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine;
                    }

                    ViewState["CompanyAddress"] = companySignature;
                    ViewState["MailContent"] = mailContent;

                    ViewState["EmailFrom"] = "";
                    if (Session["MSM"].ToString() != "TS")
                    {
                        ViewState["EmailFrom"] = dsC.Tables[0].Rows[0]["Email"].ToString();
                    }
                    #endregion
                    ViewState["InvoiceReport"] = _dtInvoice;
                    ViewState["CompanyReport"] = dsC.Tables[0];
                    Session["InvoiceReportDetails"] = _dtInvoice;


                    DataTable dt = (DataTable)ViewState["InvoiceReport"];
                    DataTable dtCompany = (DataTable)ViewState["CompanyReport"];
                    int refId = Convert.ToInt32(hdRef.Value);
                    DataTable _dtInvItems1 = GetInvoiceItems(refId);


                    string reportPathStimul = string.Empty;

                    reportPathStimul = Server.MapPath("StimulsoftReports/Invoices/" + ConfigurationManager.AppSettings["InvoiceReport"].ToString());

                    StiReport report = new StiReport();
                    report.Load(reportPathStimul);
                    report.Compile();

                    DataSet companyLogo = new DataSet();
                    //if (IsAPIIntegrationEnable == "YES")
                    //{
                    //    List<GetCompanyDetailsViewModel> _GetCompanyDetailsViewModel = new List<GetCompanyDetailsViewModel>();

                    //    string APINAME = "iCollectionsAPI/iCollectionsList_GetCompanyDetails";

                    //    _GetCompanyDetails.ConnConfig = Session["config"].ToString();

                    //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCompanyDetails);
                    //    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    //    serializer.MaxJsonLength = Int32.MaxValue;
                    //    _GetCompanyDetailsViewModel = serializer.Deserialize<List<GetCompanyDetailsViewModel>>(_APIResponse.ResponseData);
                    //    companyLogo = CommonMethods.ToDataSet<GetCompanyDetailsViewModel>(_GetCompanyDetailsViewModel);
                    //}
                    //else
                    //{
                    companyLogo = bL_Report.GetCompanyDetails(Session["config"].ToString());
                    //}

                    var imageString = companyLogo.Tables[0].Rows[0]["Logo"].ToString();
                    byte[] barrImg = (byte[])(companyLogo.Tables[0].Rows[0]["Logo"]);
                    string strfn = Convert.ToString(Server.MapPath(Request.ApplicationPath) + "/TempImages/" + DateTime.Now.ToFileTime().ToString());
                    FileStream fs = new FileStream(strfn,
                                      FileMode.CreateNew, FileAccess.Write);
                    fs.Write(barrImg, 0, barrImg.Length);
                    fs.Flush();
                    fs.Close();

                    System.Uri uri = new Uri(strfn);
                    DataTable cTable = BuildCompanyDetailsTable();
                    var cRow = cTable.NewRow();
                    cRow["LogoURL"] = uri.AbsolutePath;
                    cRow["CompanyName"] = companyLogo.Tables[0].Rows[0]["Name"].ToString();
                    cRow["CompanyAddress"] = companyLogo.Tables[0].Rows[0]["Address"].ToString();
                    cRow["ContactNo"] = companyLogo.Tables[0].Rows[0]["Contact"].ToString();
                    cRow["Email"] = companyLogo.Tables[0].Rows[0]["Email"].ToString();

                    cRow["City"] = companyLogo.Tables[0].Rows[0]["City"].ToString();
                    cRow["State"] = companyLogo.Tables[0].Rows[0]["State"].ToString();
                    cRow["Phone"] = companyLogo.Tables[0].Rows[0]["Phone"].ToString();
                    cRow["Fax"] = companyLogo.Tables[0].Rows[0]["Fax"].ToString();
                    cRow["Zip"] = companyLogo.Tables[0].Rows[0]["Zip"].ToString();


                    cTable.Rows.Add(cRow);

                    DataSet CompanyDetails = new DataSet();
                    cTable.TableName = "CompanyDetails";
                    CompanyDetails.Tables.Add(cTable);
                    CompanyDetails.DataSetName = "CompanyDetails";


                    DataSet Invoices = new DataSet();
                    DataTable dtInvoice1 = _dtInvoice.Copy();
                    dtInvoice1.TableName = "Invoices";
                    Invoices.Tables.Add(dtInvoice1.Copy());
                    Invoices.DataSetName = "Invoices";

                    DataSet InvoiceItems = new DataSet();
                    DataTable dtIInvItems = _dtInvItems1.Copy();
                    dtIInvItems.TableName = "InvoiceItems";
                    InvoiceItems.Tables.Add(dtIInvItems);
                    InvoiceItems.DataSetName = "InvoiceItems";


                    DataSet Ticket_Company = new DataSet();
                    DataTable dtTicketCompany = new DataTable();
                    dtTicketCompany = dsC.Tables[0].Copy();
                    Ticket_Company.Tables.Add(dtTicketCompany);
                    dtTicketCompany.TableName = "Ticket_Company";
                    Ticket_Company.DataSetName = "Ticket_Company";


                    DataSet Invoice_dtInvoice = new DataSet();
                    DataTable dtInvoice = new DataTable();
                    dtInvoice = ds.Tables[0].Copy();
                    Invoice_dtInvoice.Tables.Add(dtInvoice);
                    dtInvoice.TableName = "Invoice_dtInvoice";
                    Invoice_dtInvoice.DataSetName = "Invoice_dtInvoice";

                    report.RegData("Invoices", Invoices);
                    report.RegData("CompanyDetails", CompanyDetails);

                    report.RegData("Invoice_dtInvoice", Invoice_dtInvoice);

                    report.RegData("Ticket_Company", Ticket_Company);
                    report.RegData("InvoiceItems", InvoiceItems);
                    report.Dictionary.Synchronize();
                    report.Render();
                    rvInvoices.Report = report;
                    byte[] buffer1 = null;
                    var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                    var service = new Stimulsoft.Report.Export.StiPdfExportService();
                    System.IO.MemoryStream stream = new System.IO.MemoryStream();
                    service.ExportTo(rvInvoices.Report, stream, settings);
                    buffer1 = stream.ToArray();
                    invoicesAsBytes.Add(buffer1);

                    j++;
                }
            }
            return invoicesAsBytes;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return invoicesAsBytes;
        }
    }

    private string GetCompanySignature(DataSet dsC)
    {
        string companySignature = "";
        if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("Brock", StringComparison.InvariantCultureIgnoreCase))
        {
            StringBuilder addressBrock = new StringBuilder();
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["name"])))
            {
                addressBrock.AppendFormat("{0}", dsC.Tables[0].Rows[0]["name"].ToString());
                addressBrock.AppendLine();
                //addressBrock.Append("<br/>");
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["Address"])))
            {
                addressBrock.AppendFormat("{0}", dsC.Tables[0].Rows[0]["Address"].ToString());
                addressBrock.AppendLine();
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["city"])))
            {
                addressBrock.AppendFormat("{0}, ", dsC.Tables[0].Rows[0]["city"].ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["state"])))
            {
                addressBrock.AppendFormat("{0}, ", dsC.Tables[0].Rows[0]["state"].ToString());
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["zip"])))
            {
                addressBrock.AppendFormat("{0}", dsC.Tables[0].Rows[0]["zip"].ToString());
            }
            addressBrock.AppendLine();
            //addressBrock.Append("<br/>");
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["Phone"])))
            {
                addressBrock.AppendFormat("Phone: {0}", dsC.Tables[0].Rows[0]["Phone"].ToString());
                addressBrock.AppendLine();
                //addressBrock.Append("<br/>");
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["fax"])))
            {
                addressBrock.AppendFormat("Fax: {0}", dsC.Tables[0].Rows[0]["Phone"].ToString());
                addressBrock.AppendLine();
                //addressBrock.Append("<br/>");
            }
            if (!string.IsNullOrEmpty(Convert.ToString(dsC.Tables[0].Rows[0]["email"])))
            {
                addressBrock.AppendFormat("Email: {0}", dsC.Tables[0].Rows[0]["email"].ToString());
                addressBrock.AppendLine();
                //addressBrock.Append("<br/>");
            }
            companySignature = addressBrock.ToString();
        }
        else
        {
            //companySignature = dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
            //companySignature += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
            //companySignature += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
            //companySignature += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
            //companySignature += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;
            //companySignature += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;
            companySignature = WebBaseUtility.GetSignature();
        }

        return companySignature;
    }

    protected void lnkEmailCustomerStatementSelected_Click(object sender, EventArgs e)
    {
        try
        {
            Contracts objProp_Contracts = new Contracts();
            BL_Contracts objBL_Contracts = new BL_Contracts();
            BusinessEntity.User objPropUser = new BusinessEntity.User();
            BL_User objBL_User = new BL_User();
            BL_Report bL_Report = new BL_Report();

            if (RadGrid_Collections.SelectedItems.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Please select at least one row from the grid.',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                return;
            }

            #region Collect All The Loc
            List<String> Locs = new List<string>();
            foreach (GridDataItem gr in RadGrid_Collections.SelectedItems)
            {
                HiddenField hdType = (HiddenField)gr.FindControl("hdType");
                HiddenField hdRef = (HiddenField)gr.FindControl("hdRef");
                HiddenField hdLoc = (HiddenField)gr.FindControl("hdLoc");
                if (!string.IsNullOrEmpty(hdLoc.Value))
                {
                    Locs.Add(hdLoc.Value);
                }
            }

            List<String> uniqueLocs = Locs.Distinct().ToList();
            if (uniqueLocs.Count <= 0)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'There is no Customer Statement in your selected items',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                return;
            }
            #endregion

            string _todayDate = DateTime.Now.Date.ToString("MM-dd-yyyy");
            string fromEmail;
            if (ViewState["EmailFrom"] == null)
            {
                fromEmail = WebBaseUtility.GetFromEmailAddress();
            }
            else
            {
                fromEmail = ViewState["EmailFrom"].ToString();
            }

            int totalInvoicesInList = uniqueLocs.Count;//dt != null ? dt.Rows.Count : 0;
            int totalInvoicesForEmail = totalInvoicesInList;// temp.Count();//dt != null ? dt.Rows.Count : 0;
            int totalSentEmails = 0;
            int totalSendErr = 0;
            List<MimeKit.MimeMessage> mimeSentMessages = new List<MimeKit.MimeMessage>();
            List<MimeKit.MimeMessage> mimeErrorMessages = new List<MimeKit.MimeMessage>();
            List<string> invoiceIdsSentEmail = new List<string>();
            Tuple<int, string, string> emailSendError = null;
            Tuple<int, string, string> emailGetSentError = null;
            StringBuilder sbdSentError = new StringBuilder();
            StringBuilder sbdGetSentError = new StringBuilder();

            EmailLog emailLog = new EmailLog();
            emailLog.ConnConfig = Session["config"].ToString();
            emailLog.Function = "Customer Statement Selected";
            emailLog.Screen = "Collections";
            emailLog.Username = Session["Username"].ToString();
            emailLog.SessionNo = Guid.NewGuid().ToString();

            _AddEmailLog.ConnConfig = Session["config"].ToString();
            _AddEmailLog.Function = "Customer Statement Selected";
            _AddEmailLog.Screen = "Collections";
            _AddEmailLog.Username = Session["Username"].ToString();
            _AddEmailLog.SessionNo = Guid.NewGuid().ToString();

            StringBuilder sbdInValidEmails = new StringBuilder();
            foreach (var loc in uniqueLocs)
            {
                ViewState["CollectionLoc"] = loc;
                string toEmail = "";
                string ccEmail = "";

                DataSet dsC = new DataSet();
                objPropUser.ConnConfig = Session["config"].ToString();
                _GetControlBranch.ConnConfig = Session["config"].ToString();
                if (Session["MSM"].ToString() != "TS")
                {
                    if (IsAPIIntegrationEnable == "YES")
                    {
                        List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

                        string APINAME = "iCollectionsAPI/iCollectionsList_GetControl";

                        _getConnectionConfig.ConnConfig = Session["config"].ToString();

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        serializer.MaxJsonLength = Int32.MaxValue;

                        _GetControlViewModel = serializer.Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                        dsC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
                    }
                    else
                    {
                        dsC = objBL_User.getControl(objPropUser);
                    }
                }
                else
                {
                    objPropUser.LocID = Convert.ToInt32(loc);
                    _GetControlBranch.LocID = Convert.ToInt32(loc);

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

                        string APINAME = "iCollectionsAPI/iCollectionsList_GetControlBranch";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetControlBranch);

                        _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                        dsC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
                    }
                    else
                    {
                        dsC = objBL_User.getControlBranch(objPropUser);
                    }
                }

                objProp_Contracts.ConnConfig = Session["config"].ToString();
                objProp_Contracts.Loc = Convert.ToInt32(loc);
                emailLog.Ref = objProp_Contracts.Loc;

                DataSet ds = new DataSet();

                if (IsAPIIntegrationEnable == "YES")
                {
                    List<GetCustomerStatementByLocViewModel> _lstGetCustomerStatementByLoc = new List<GetCustomerStatementByLocViewModel>();

                    string APINAME = "iCollectionsAPI/iCollectionsList_GetCustomerStatementByLoc";

                    _GetCustomerStatementByLoc.ConnConfig = Session["config"].ToString();
                    _GetCustomerStatementByLoc.Loc = Convert.ToInt32(loc);
                    _AddEmailLog.Ref = _GetCustomerStatementByLoc.Loc;

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomerStatementByLoc);

                    _lstGetCustomerStatementByLoc = (new JavaScriptSerializer()).Deserialize<List<GetCustomerStatementByLocViewModel>>(_APIResponse.ResponseData);
                    ds = CommonMethods.ToDataSet<GetCustomerStatementByLocViewModel>(_lstGetCustomerStatementByLoc);
                }
                else
                {
                    ds = objBL_Contracts.GetCustomerStatementByLoc(objProp_Contracts);
                }

                DataTable dtLoc = ds.Tables[0];
                ViewState["Invoices"] = dtLoc;

                toEmail = dtLoc.Rows[0]["custom12"].ToString();
                if (!string.IsNullOrEmpty(toEmail))
                {

                    #region Email

                    if (!string.IsNullOrEmpty(dtLoc.Rows[0]["custom13"].ToString()))
                    {
                        ccEmail = dtLoc.Rows[0]["custom13"].ToString();
                    }

                    Mail mail = new Mail();
                    mail.From = fromEmail;
                    foreach (var toaddress in toEmail.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        mail.To.Add(toaddress);
                    }
                    foreach (var ccaddress in ccEmail.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        mail.Cc.Add(ccaddress);
                    }
                    StringBuilder _sbdInValidEmails = new StringBuilder();
                    CheckValidEmails(mail.To, _sbdInValidEmails);
                    CheckValidEmails(mail.Cc, _sbdInValidEmails);
                    if (_sbdInValidEmails.Length > 0)
                    {
                        sbdInValidEmails.Append(_sbdInValidEmails);
                    }

                    if (mail.To.Count == 0)
                    {
                        totalSendErr++;
                        emailLog.To = _sbdInValidEmails.ToString();
                        emailLog.Status = 0;
                        emailLog.UsrErrMessage = "Invalid emails address";

                        _AddEmailLog.To = _sbdInValidEmails.ToString();
                        _AddEmailLog.Status = 0;
                        _AddEmailLog.UsrErrMessage = "Invalid emails address";

                        BL_EmailLog bL_EmailLog = new BL_EmailLog();

                        if (IsAPIIntegrationEnable == "YES")
                        {
                            string APINAME = "iCollectionsAPI/iCollectionsList_AddEmailLog";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddEmailLog);
                        }
                        else
                        {
                            bL_EmailLog.AddEmailLog(emailLog);
                        }

                        continue;
                    }
                    #region Generate Report
                    byte[] getPDF = null;
                    string report = System.Web.Configuration.WebConfigurationManager.AppSettings["CustomerInvoieStatement"];

                    ReportViewer rvCs = new ReportViewer();

                    #region Mail Body
                    BL_General bL_General = new BL_General();
                    EmailTemplate emailTemplate = new EmailTemplate();
                    emailTemplate.ConnConfig = Session["config"].ToString();
                    emailTemplate.Screen = "Collections";
                    emailTemplate.FunctionName = "Customer Statement All";
                    string mailContent = bL_General.GetEmailTemplate(emailTemplate);

                    string address = "";
                    if (string.IsNullOrEmpty(mailContent))
                    {
                        address = address = "Please review the attached customer statement <br/><br/>" + WebBaseUtility.GetSignature();
                    }
                    else
                    {
                        address = mailContent + WebBaseUtility.GetSignature();
                    }

                    #endregion
                    if (!string.IsNullOrEmpty(report.Trim()) && report.Contains(".mrt"))
                    {
                        DataSet dsItem = new DataSet();

                        if (IsAPIIntegrationEnable == "YES")
                        {
                            List<GetCustStatementInvSouthernViewModel> _lstGetCustStatementInvSouthern = new List<GetCustStatementInvSouthernViewModel>();

                            string APINAME = "iCollectionsAPI/iCollectionsList_GetCustomerStatementInvoicesByLocation";

                            // _GetCustStatementInvByLocation.ConnConfig = Session["config"].ToString();
                            //_GetCustStatementInvByLocation.LocationIDs = string.Join(",", distinctIDs);
                            //_GetCustStatementInvByLocation.includeCredit = true;

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustStatementInvByLocation);

                            JavaScriptSerializer serializer = new JavaScriptSerializer();

                            serializer.MaxJsonLength = Int32.MaxValue;

                            _lstGetCustStatementInvSouthern = serializer.Deserialize<List<GetCustStatementInvSouthernViewModel>>(_APIResponse.ResponseData);
                            dsItem = CommonMethods.ToDataSet<GetCustStatementInvSouthernViewModel>(_lstGetCustStatementInvSouthern);
                        }
                        else
                        {
                            dsItem = objBL_Contracts.GetCustomerStatementInvoicesByLocation(objProp_Contracts, true);
                        }

                        StiReport stiReport = PrintMRTReport(dsC.Tables[0], ds.Tables[0], dsItem.Tables[0]);

                        var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                        var service = new Stimulsoft.Report.Export.StiPdfExportService();
                        System.IO.MemoryStream stream = new System.IO.MemoryStream();
                        service.ExportTo(stiReport, stream, settings);
                        getPDF = stream.ToArray();
                        mail.attachmentBytes = getPDF;
                    }
                    else
                    {

                        rvCs.LocalReport.DataSources.Clear();

                        rvCs.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemDetailsSubReportProcessing);
                        rvCs.LocalReport.DataSources.Add(new ReportDataSource("dsInvoice", dtLoc));
                        rvCs.LocalReport.DataSources.Add(new ReportDataSource("dsCompany", dsC.Tables[0]));
                        string reportPath;
                        if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("SECO"))
                        {
                            reportPath = "Reports/CustomerStatementSouthern.rdlc";
                        }
                        else
                        {
                            reportPath = "Reports/CustomerStatement.rdlc";
                        }


                        string Report = string.Empty;
                        if (!string.IsNullOrEmpty(Report.Trim()))
                        {
                            reportPath = "Reports/" + Report.Trim();
                        }
                        rvCs.LocalReport.ReportPath = reportPath;
                        rvCs.LocalReport.EnableExternalImages = true;
                        List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
                        string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
                        param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("path", strPath + "/images/Company_logo.jpg"));

                        rvCs.LocalReport.SetParameters(param1);

                        rvCs.LocalReport.Refresh();
                        mail.attachmentBytes = ExportReportToPDF1("", rvCs);
                    }

                    #endregion

                    mail.Title = "Customer Statement - " + dtLoc.Rows[0]["LocID"].ToString() + " " + dtLoc.Rows[0]["locname"].ToString();
                    mail.Text = address.Replace(Environment.NewLine, "<BR/>");
                    mail.FileName = "CustomerStatement_" + _todayDate + ".pdf";

                    mail.DeleteFilesAfterSend = true;

                    WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                    MimeKit.MimeMessage mimeMessage = new MimeKit.MimeMessage();
                    emailSendError = mail.CompletingMessage(ref mimeMessage, true, emailLog);
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
                            invoiceIdsSentEmail.Add("Loc #" + loc);
                        }
                    }
                    #endregion
                }
                else
                {
                    totalSendErr++;
                    emailLog.To = string.Empty;
                    emailLog.Status = 0;
                    emailLog.UsrErrMessage = "Email address does not exist for this location";

                    _AddEmailLog.To = string.Empty;
                    _AddEmailLog.Status = 0;
                    _AddEmailLog.UsrErrMessage = "Email address does not exist for this location";

                    BL_EmailLog bL_EmailLog = new BL_EmailLog();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "iCollectionsAPI/iCollectionsList_AddEmailLog";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddEmailLog);
                    }
                    else
                    {
                        bL_EmailLog.AddEmailLog(emailLog);
                    }

                }
            }

            totalSentEmails = mimeSentMessages.Count;
            if (totalSentEmails > 0)
            {
                Mail mail = new Mail();
                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                if (mail.TakeASentEmailCopy)
                {
                    emailLog.Ref = 0;
                    if (totalSentEmails >= 10)
                    {
                        List<List<MimeKit.MimeMessage>> lstTenMessages = new List<List<MimeKit.MimeMessage>>();
                        while (mimeSentMessages.Any())
                        {
                            lstTenMessages.Add(mimeSentMessages.Take(11).ToList());
                            mimeSentMessages = mimeSentMessages.Skip(11).ToList();
                        }

                        foreach (var lst in lstTenMessages)
                        {
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
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnSentError", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);

                if (emailGetSentError != null)
                {
                    string str1 = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnGetSentError", "noty({text: '" + str1 + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                }
            }
            else
            {
                if (totalSentEmails > 0)
                {
                    var successfullMess = "There were " + totalSentEmails + " of "
                        + totalInvoicesInList.ToString() + " Customer Statement sent out successfully.";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: '" + successfullMess + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);

                    if (totalSendErr > 0)
                    {
                        var errMess = "Total " + totalSendErr + " failed of "
                            + totalInvoicesInList.ToString() + " Customer Statement could not be sent.";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnSendErr", "noty({text: '" + errMess + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);

                    }

                    if (emailGetSentError != null)
                    {
                        string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnGetSentError", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                    }
                }
                else
                {
                    string str = "There were no emails sent out.";

                    if (totalSendErr > 0)
                    {
                        str += "<br>Total " + totalSendErr + " failed of "
                            + totalInvoicesInList.ToString() + " Customer Statement could not be sent.";
                    }

                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnNoEmail", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                }
            }
            //}
            if (sbdInValidEmails.Length > 0)
            {
                sbdInValidEmails.Insert(0, "Invalid emails address:<br/>");
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnInValidEmails", "noty({text: '" + sbdInValidEmails.ToString() + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default', closable: false, dismissQueue: true});", true);
            }
            //Session["MailSend"] = "true";
            //Response.Redirect("iCollections.aspx");
            // Refresh Email History grid
            RadGrid_gvLogs.Rebind();
        }
        catch (Exception ex)
        {
            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }

    protected void lnkEmailInvoiceSelected_Click(object sender, EventArgs e)
    {
        try
        {
            #region Validate the selected rows
            Boolean isRowSelected = false;
            var selectedCount = RadGrid_Collections.SelectedItems.Count;
            if (selectedCount == 0)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Please select at least one row from the grid!',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                return;
            }

            foreach (GridDataItem gr in RadGrid_Collections.SelectedItems)
            {
                HiddenField hdType = (HiddenField)gr.FindControl("hdType");
                if (hdType.Value == "1")
                {
                    isRowSelected = true;
                    break;
                }
            }
            if (isRowSelected == false)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'There is no invoice in your selected items.',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                return;
            }

            #endregion
            Contracts objProp_Contracts = new Contracts();
            BL_Contracts objBL_Contracts = new BL_Contracts();
            objProp_Contracts.ConnConfig = Session["config"].ToString();

            _GetEmailDetailByLoc.ConnConfig = Session["config"].ToString();

            int totalInvoicesInList = RadGrid_Collections.SelectedItems.Count;
            int totalInvoicesForEmail = 0;
            int totalSentEmails = 0;
            int totalSendErr = 0;
            int totalNotSend = 0;//totalInvoicesInList - totalInvoicesForEmail;
            List<MimeKit.MimeMessage> mimeSentMessages = new List<MimeKit.MimeMessage>();
            List<MimeKit.MimeMessage> mimeErrorMessages = new List<MimeKit.MimeMessage>();
            List<string> invoiceIdsSentEmail = new List<string>();
            Tuple<int, string, string> emailSendError = null;
            Tuple<int, string, string> emailGetSentError = null;
            StringBuilder sbdSentError = new StringBuilder();
            StringBuilder sbdGetSentError = new StringBuilder();

            EmailLog emailLog = new EmailLog();
            emailLog.ConnConfig = Session["config"].ToString();
            emailLog.Function = "Invoice Selected";
            emailLog.Screen = "Collections";
            emailLog.Username = Session["Username"].ToString();
            emailLog.SessionNo = Guid.NewGuid().ToString();

            _AddEmailLog.ConnConfig = Session["config"].ToString();
            _AddEmailLog.Function = "Invoice Selected";
            _AddEmailLog.Screen = "Collections";
            _AddEmailLog.Username = Session["Username"].ToString();
            _AddEmailLog.SessionNo = Guid.NewGuid().ToString();

            BL_General bL_General = new BL_General();
            EmailTemplate emailTemplate = new EmailTemplate();
            emailTemplate.ConnConfig = Session["config"].ToString();
            emailTemplate.Screen = "Collections";
            emailTemplate.FunctionName = "Invoice Selected";
            string mailContent = bL_General.GetEmailTemplate(emailTemplate);

            StringBuilder sbdInValidEmails = new StringBuilder();
            foreach (GridDataItem gr in RadGrid_Collections.SelectedItems)
            {
                HiddenField hdType = (HiddenField)gr.FindControl("hdType");
                HiddenField hdRef = (HiddenField)gr.FindControl("hdRef");
                if (hdType.Value == "1")
                {
                    totalInvoicesForEmail++;
                    String InvoiceID = hdRef.Value;
                    objProp_Contracts.Ref = Convert.ToInt32(InvoiceID);
                    _GetEmailDetailByLoc.Ref = Convert.ToInt32(InvoiceID);

                    emailLog.Ref = objProp_Contracts.Ref;
                    _AddEmailLog.Ref = _GetEmailDetailByLoc.Ref;

                    DataSet _dsCon = new DataSet();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        List<GetEmailDetailByLocViewModel> _lstGetEmailDetailByLoc = new List<GetEmailDetailByLocViewModel>();

                        string APINAME = "iCollectionsAPI/iCollectionsList_GetEmailDetailByLoc";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetEmailDetailByLoc);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        serializer.MaxJsonLength = Int32.MaxValue;

                        _lstGetEmailDetailByLoc = serializer.Deserialize<List<GetEmailDetailByLocViewModel>>(_APIResponse.ResponseData);
                        _dsCon = CommonMethods.ToDataSet<GetEmailDetailByLocViewModel>(_lstGetEmailDetailByLoc);
                    }
                    else
                    {
                        _dsCon = objBL_Contracts.GetEmailDetailByLoc(objProp_Contracts);
                    }

                    if (_dsCon.Tables[0].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(_dsCon.Tables[0].Rows[0]["custom12"].ToString()))//if (!string.IsNullOrEmpty(_dsCon.Tables[0].Rows[0]["custom12"].ToString()))
                        {
                            string _fromEmail = string.Empty;

                            _fromEmail = Convert.ToString(ViewState["EmailFrom"]);
                            if (string.IsNullOrEmpty(_fromEmail))
                            {
                                //_fromEmail = GetFromEmailAddress();
                                _fromEmail = WebBaseUtility.GetFromEmailAddress();
                            }

                            string _toEmail = "";
                            string _ccEmail = "";
                            if (!string.IsNullOrEmpty(_dsCon.Tables[0].Rows[0]["custom12"].ToString()))
                            {
                                _toEmail = _dsCon.Tables[0].Rows[0]["custom12"].ToString();

                                if (!string.IsNullOrEmpty(_dsCon.Tables[0].Rows[0]["custom13"].ToString()))
                                {
                                    _ccEmail = _dsCon.Tables[0].Rows[0]["custom13"].ToString();
                                }
                            }

                            Mail mail = new Mail();
                            mail.From = _fromEmail;
                            foreach (var toaddress in _toEmail.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                mail.To.Add(toaddress);
                            }
                            foreach (var ccaddress in _ccEmail.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                mail.Cc.Add(ccaddress);
                            }
                            StringBuilder _sbdInValidEmails = new StringBuilder();
                            CheckValidEmails(mail.To, _sbdInValidEmails);
                            CheckValidEmails(mail.Cc, _sbdInValidEmails);
                            if (_sbdInValidEmails.Length > 0)
                            {
                                sbdInValidEmails.Append(_sbdInValidEmails);
                            }

                            if (mail.To.Count == 0)
                            {
                                totalSendErr++;
                                emailLog.To = _sbdInValidEmails.ToString();
                                emailLog.Status = 0;
                                emailLog.UsrErrMessage = "Invalid emails address";

                                _AddEmailLog.To = _sbdInValidEmails.ToString();
                                _AddEmailLog.Status = 0;
                                _AddEmailLog.UsrErrMessage = "Invalid emails address";

                                BL_EmailLog bL_EmailLog = new BL_EmailLog();

                                if (IsAPIIntegrationEnable == "YES")
                                {
                                    string APINAME = "iCollectionsAPI/iCollectionsList_AddEmailLog";

                                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddEmailLog);
                                }
                                else
                                {
                                    bL_EmailLog.AddEmailLog(emailLog);
                                }

                                continue;
                            }

                            #region Generate Report
                            StiWebViewer rvInvoices = new StiWebViewer();
                            byte[] buffer = null;

                            List<byte[]> invoicesToPrint = PrintInvoicesForIndivudial(rvInvoices, Convert.ToInt32(InvoiceID));

                            string strInvoiceFileName = "Invoice" + InvoiceID + ".pdf";
                            //string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF", "Invoices.pdf");
                            string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF", strInvoiceFileName);

                            if (invoicesToPrint != null)
                            {

                                buffer = concatAndAddContent(invoicesToPrint);
                                if (File.Exists(filename))
                                    File.Delete(filename);
                                using (var fs = new FileStream(filename, FileMode.Create))
                                {
                                    fs.Write(buffer, 0, buffer.Length);
                                    fs.Close();
                                }
                            }
                            #endregion

                            mail.Title = "Invoice " + InvoiceID + " - " + _dsCon.Tables[0].Rows[0]["Tag"].ToString();

                            if (string.IsNullOrEmpty(mailContent))
                            {
                                mailContent = ViewState["MailContent"].ToString().Replace(Environment.NewLine, "<BR/>");
                            }
                            else
                            {
                                mailContent = mailContent.Replace("{Invoice_No}", InvoiceID);
                                //.Replace("{Invoice_CustomerName}", _dsCon.Tables[0].Rows[0]["customerName"].ToString())
                            }
                            //var mailContent = ViewState["MailContent"].ToString().Replace(Environment.NewLine, "<BR/>");
                            var companySignature = ViewState["CompanyAddress"].ToString().Replace(Environment.NewLine, "<BR/>");
                            mail.Text = mailContent;
                            mail.IsIncludeSignature = true;

                            mail.attachmentBytes = buffer;
                            mail.FileName = strInvoiceFileName;
                            mail.DeleteFilesAfterSend = true;
                            mail.RequireAutentication = false;

                            // ES-33:Task#2: Need to update email configuration before calling send function
                            WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
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
                                        //invoiceIdsError.Add("Invoice #" + _ref.ToString());
                                        totalSendErr++;
                                    }
                                }
                                else
                                {
                                    mimeSentMessages.Add(mimeMessage);
                                    invoiceIdsSentEmail.Add("Invoice #" + InvoiceID);
                                }
                            }
                        }
                        else
                        {
                            //invoiceIdsError.Add("Invoice #" + _ref.ToString());
                            totalSendErr++;
                            emailLog.To = string.Empty;
                            emailLog.Status = 0;
                            emailLog.UsrErrMessage = "Email address does not exist for this location";

                            _AddEmailLog.To = string.Empty;
                            _AddEmailLog.Status = 0;
                            _AddEmailLog.UsrErrMessage = "Email address does not exist for this location";

                            BL_EmailLog bL_EmailLog = new BL_EmailLog();

                            if (IsAPIIntegrationEnable == "YES")
                            {
                                string APINAME = "iCollectionsAPI/iCollectionsList_AddEmailLog";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddEmailLog);
                            }
                            else
                            {
                                bL_EmailLog.AddEmailLog(emailLog);
                            }
                        }
                    }
                }
                totalNotSend = totalInvoicesInList - totalInvoicesForEmail;
            }

            totalSentEmails = mimeSentMessages.Count;
            if (totalSentEmails > 0)
            {
                Mail mail = new Mail();
                WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                if (mail.TakeASentEmailCopy)
                {
                    emailLog.Ref = 0;
                    if (totalSentEmails >= 10)
                    {
                        List<List<MimeKit.MimeMessage>> lstTenMessages = new List<List<MimeKit.MimeMessage>>();
                        while (mimeSentMessages.Any())
                        {
                            lstTenMessages.Add(mimeSentMessages.Take(11).ToList());
                            mimeSentMessages = mimeSentMessages.Skip(11).ToList();
                        }

                        foreach (var lst in lstTenMessages)
                        {
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
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnSentError", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                if (emailGetSentError != null)
                {
                    string str1 = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnGetSentError", "noty({text: '" + str1 + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                }
            }
            else
            {
                if (totalSentEmails > 0)
                {
                    var successfullMess = "There were " + totalSentEmails + " of "
                        + totalInvoicesInList.ToString() + " selected items sent out successfully.";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: '" + successfullMess + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);

                    if (totalSendErr > 0)
                    {
                        var errMess = "Total " + totalSendErr + " failed of "
                            + totalInvoicesInList.ToString() + " selected items could not be sent.";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnSendErr", "noty({text: '" + errMess + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                    }
                    if (totalNotSend > 0)
                    {
                        var notSentMess = "Total " + totalNotSend + " of "
                            + totalInvoicesInList + " selected items are not invoices.";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnNotSend", "noty({text: '" + notSentMess + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);

                    }

                    if (emailGetSentError != null)
                    {
                        string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnGetSentError", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                    }
                }
                else
                {
                    string str = "There were no emails sent out.";
                    if (totalSendErr > 0)
                    {
                        str += "<br>Total " + totalSendErr + " failed of "
                            + totalInvoicesInList.ToString() + " selected items could not be sent.";
                    }
                    if (totalNotSend > 0)
                    {
                        str += "<br>Total " + totalNotSend + " of "
                            + totalInvoicesInList + " selected items are not invoices.";
                    }
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnNoEmail", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true, dismissQueue:true});", true);
                }
            }

            if (sbdInValidEmails.Length > 0)
            {
                sbdInValidEmails.Insert(0, "Invalid emails address:<br/>");
                ClientScript.RegisterStartupScript(Page.GetType(), "keyWarn1", "noty({text: '" + sbdInValidEmails.ToString() + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default', closable: false, dismissQueue: true});", true);
            }
            // Refresh Email History grid
            RadGrid_gvLogs.Rebind();
        }
        catch (Exception exp)
        {
            //string str = exp.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(exp.Message);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkPDFCustomerStatementSelected_Click(object sender, EventArgs e)
    {
        try
        {
            #region Validate the selected rows           
            if (RadGrid_Collections.SelectedItems.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Please select at least one row from the grid.',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                return;
            }

            #endregion

            count = 0;
            Contracts objProp_Contracts = new Contracts();
            BL_Contracts objBL_Contracts = new BL_Contracts();
            BusinessEntity.User objPropUser = new BusinessEntity.User();
            BL_User objBL_User = new BL_User();
            BL_Report bL_Report = new BL_Report();

            #region Collect All The Loc

            List<string> locIDs = new List<string>();
            foreach (GridDataItem gr in RadGrid_Collections.SelectedItems)
            {
                HiddenField hdType = (HiddenField)gr.FindControl("hdType");
                HiddenField hdLoc = (HiddenField)gr.FindControl("hdLoc");
                if (!string.IsNullOrEmpty(hdLoc.Value))
                {
                    locIDs.Add(hdLoc.Value);
                }
            }

            var distinctIDs = locIDs.Distinct();

            #endregion

            #region Company Detail
            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();

            if (IsAPIIntegrationEnable == "YES")
            {
                List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

                string APINAME = "iCollectionsAPI/iCollectionsList_GetControl";

                _getConnectionConfig.ConnConfig = Session["config"].ToString();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _GetControlViewModel = serializer.Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                dsC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
            }
            else
            {
                dsC = objBL_User.getControl(objPropUser);
            }
            #endregion

            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.LocationIDs = string.Join(",", distinctIDs);
            objProp_Contracts.UserID = Session["userid"].ToString();

            DataSet ds = new DataSet();

            if (IsAPIIntegrationEnable == "YES")
            {
                List<GetCustomerStatementByLocViewModel> _lstGetCustomerStatementByLoc = new List<GetCustomerStatementByLocViewModel>();

                string APINAME = "iCollectionsAPI/iCollectionsList_GetCustomerStatementByLocs";

                _GetCustomerStatementByLocs.ConnConfig = Session["config"].ToString();
                _GetCustomerStatementByLocs.LocationIDs = string.Join(",", distinctIDs);
                _GetCustomerStatementByLocs.UserID = Session["userid"].ToString();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomerStatementByLocs);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetCustomerStatementByLoc = serializer.Deserialize<List<GetCustomerStatementByLocViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetCustomerStatementByLocViewModel>(_lstGetCustomerStatementByLoc);
            }
            else
            {
                ds = objBL_Contracts.GetCustomerStatementByLocs(objProp_Contracts);
            }

            ViewState["Invoices"] = ds.Tables[0];

            byte[] getPDF = null;
            string report = System.Web.Configuration.WebConfigurationManager.AppSettings["CustomerInvoieStatement"];

            if (!string.IsNullOrEmpty(report.Trim()) && report.Contains(".mrt"))
            {
                DataSet dsItem = new DataSet();

                if (IsAPIIntegrationEnable == "YES")
                {
                    List<GetCustStatementInvSouthernViewModel> _lstGetCustStatementInvSouthern = new List<GetCustStatementInvSouthernViewModel>();

                    string APINAME = "iCollectionsAPI/iCollectionsList_GetCustomerStatementInvoicesByLocation";

                    _GetCustStatementInvByLocation.ConnConfig = Session["config"].ToString();
                    _GetCustStatementInvByLocation.LocationIDs = string.Join(",", distinctIDs);
                    _GetCustStatementInvByLocation.includeCredit = true;

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustStatementInvByLocation);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetCustStatementInvSouthern = serializer.Deserialize<List<GetCustStatementInvSouthernViewModel>>(_APIResponse.ResponseData);
                    dsItem = CommonMethods.ToDataSet<GetCustStatementInvSouthernViewModel>(_lstGetCustStatementInvSouthern);
                }
                else
                {
                    dsItem = objBL_Contracts.GetCustomerStatementInvoicesByLocation(objProp_Contracts, true);
                }

                StiReport stiReport = PrintMRTReport(dsC.Tables[0], ds.Tables[0], dsItem.Tables[0]);

                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(stiReport, stream, settings);
                getPDF = stream.ToArray();
            }
            else
            {
                ReportViewer rvCustomer = PrintRDLCReport(dsC.Tables[0], ds.Tables[0]);
                getPDF = ExportReportToPDF1("", rvCustomer);
            }

            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=CustomerStatement.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (getPDF.Length).ToString());
            Response.BinaryWrite(getPDF);
            Response.Flush();
            Response.Close();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private ReportViewer PrintRDLCReport(DataTable companyInfo, DataTable invoices)
    {
        try
        {
            ReportViewer rvCustomer = new ReportViewer();

            rvCustomer.LocalReport.DataSources.Clear();

            rvCustomer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(ItemDetailsSubReportProcessingNew);
            rvCustomer.LocalReport.DataSources.Add(new ReportDataSource("dsInvoice", invoices));
            rvCustomer.LocalReport.DataSources.Add(new ReportDataSource("dsCompany", companyInfo));

            string reportPath = "Reports/CustomerStatement.rdlc";

            string Report = System.Web.Configuration.WebConfigurationManager.AppSettings["CustomerInvoieStatement"].Trim();
            if (!string.IsNullOrEmpty(Report.Trim()) && Report.Contains(".rdlc"))
            {
                reportPath = "Reports/" + Report.Trim();
            }

            rvCustomer.LocalReport.ReportPath = reportPath;
            rvCustomer.LocalReport.EnableExternalImages = true;
            List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
            string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
            param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("path", strPath + "/images/Company_logo.jpg"));

            rvCustomer.LocalReport.SetParameters(param1);
            rvCustomer.LocalReport.Refresh();

            return rvCustomer;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return null;
        }
    }

    private StiReport PrintMRTReport(DataTable companyInfo, DataTable invoices, DataTable invoiceItem)
    {
        try
        {
            string reportPathStimul = Server.MapPath("StimulsoftReports/CustomerStatement.mrt");

            string reportCustom = System.Web.Configuration.WebConfigurationManager.AppSettings["CustomerInvoieStatement"];
            if (!string.IsNullOrEmpty(reportCustom) && reportCustom.ToLower().Contains(".mrt"))
            {
                reportPathStimul = Server.MapPath($"StimulsoftReports/{reportCustom}");
            }

            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            report.RegData("CompanyDetails", companyInfo);
            report.RegData("Invoices", invoices);
            report.RegData("InvoiceItem", invoiceItem);

            report.CacheAllData = true;
            report.Render();

            return report;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return null;
        }
    }

    protected void lnkPDFInvoiceSelected_Click(object sender, EventArgs e)
    {
        try
        {
            #region Validate the selected rows
            Boolean isRowSelected = false;
            foreach (GridDataItem gr in RadGrid_Collections.SelectedItems)
            {
                HiddenField hdType = (HiddenField)gr.FindControl("hdType");
                if (hdType.Value == "1")
                {
                    isRowSelected = true;
                    break;
                }
            }
            if (isRowSelected == false)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Please select at least one row from the grid.',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                return;
            }

            #endregion

            StiWebViewer rvInvoices = new StiWebViewer();

            List<byte[]> invoicesToPrint = PrintInvoicesSelected(rvInvoices);



            string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF", "Invoices.pdf");

            if (invoicesToPrint != null)
            {
                byte[] buffer1 = null;

                buffer1 = concatAndAddContent(invoicesToPrint);

                if (File.Exists(filename))
                    File.Delete(filename);
                using (var fs = new FileStream(filename, FileMode.Create))
                {
                    fs.Write(buffer1, 0, buffer1.Length);
                    fs.Close();
                }
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=Invoices.pdf");
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Length", (buffer1.Length).ToString());
                Response.BinaryWrite(buffer1);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkColCustomerStatement_Click(object sender, EventArgs e)
    {
        #region Collect All The Loc

        List<string> locIDs = new List<string>();
        foreach (GridDataItem gr in RadGrid_Collections.Items)
        {
            HiddenField hdLoc = (HiddenField)gr.FindControl("hdLoc");
            if (!string.IsNullOrEmpty(hdLoc.Value))
            {
                locIDs.Add(hdLoc.Value);
            }
        }

        #endregion

        var distinctIDs = locIDs.Distinct();
        Session["ColLocationIds"] = string.Join(",", distinctIDs);

        string reportCustom = System.Web.Configuration.WebConfigurationManager.AppSettings["CustomerInvoieStatement"];
        if (!string.IsNullOrEmpty(reportCustom) && reportCustom.ToLower().Contains(".mrt"))
        {
            Response.Redirect("CustomerStatementCollectionReport.aspx");
        }
        else
        {
            Response.Redirect("CustomerStatementCollection.aspx");
        }
    }

    protected void chkHidePartial_CheckedChanged(object sender, EventArgs e)
    {
        BindCollectionGrid();
        RadGrid_Collections.Rebind();
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] ServiceGetAcct(string prefixText, int count, string contextKey)
    {
        DataSet ds = new DataSet();
        BL_Deposit objBL_Deposit = new BL_Deposit();

        string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
        GetGLAccountParam _GetGLAccount = new GetGLAccountParam();

        if (IsAPIIntegrationEnable == "YES")
        {
            List<GetGLAccountViewModel> _lstGetGLAccount = new List<GetGLAccountViewModel>();

            string APINAME = "iCollectionsAPI/iCollectionsList_GetGLAccount";

            _GetGLAccount.ConnConfig = HttpContext.Current.Session["config"].ToString();
            _GetGLAccount.acct = prefixText;

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetGLAccount);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetGLAccount = serializer.Deserialize<List<GetGLAccountViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetGLAccountViewModel>(_lstGetGLAccount);
        }
        else
        {
            ds = objBL_Deposit.GetGLAccount(HttpContext.Current.Session["config"].ToString(), prefixText);
        }

        DataTable dt = ds.Tables[0];

        List<string> txtItems = new List<string>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(row["Acct"].ToString() + "-" + row["fDesc"].ToString(), row["ID"].ToString());
            txtItems.Add(dbValues);
        }

        return txtItems.ToArray();


    }

    protected void lnkSaveWriteOff_Click(object sender, EventArgs e)
    {
        try
        {
            BL_Deposit objBL_Deposit = new BL_Deposit();
            WriteOff obj = new WriteOff();
            obj.ConnConfig = Session["config"].ToString();
            obj.ID = Convert.ToInt32(txtInvoiceWriteOff.Text);
            obj.Acct = Convert.ToInt32(ddlCode.SelectedItem.Value);
            obj.Desc = txtDescription.Text;
            obj.fDate = txtWriteOffDate.Text == "" ? DateTime.Now : Convert.ToDateTime(txtWriteOffDate.Text);
            obj.CreateBy = Session["username"].ToString();
            obj.CheckNo = "";
            obj.WriteoffDesc = "Received payment";
            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "iCollectionsAPI/iCollectionsList_writeOffInvoice";

                _writeOffInvoice.ConnConfig = Session["config"].ToString();
                _writeOffInvoice.ID = Convert.ToInt32(txtInvoiceWriteOff.Text);
                _writeOffInvoice.Acct = Convert.ToInt32(ddlCode.SelectedItem.Value);
                _writeOffInvoice.Desc = txtDescription.Text;
                _writeOffInvoice.fDate = txtWriteOffDate.Text == "" ? DateTime.Now : Convert.ToDateTime(txtWriteOffDate.Text);
                _writeOffInvoice.CreateBy = Session["username"].ToString();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _writeOffInvoice);
            }
            else
            {
                objBL_Deposit.writeOffInvoice(obj);
            }

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "closeScript", "CloseWriteOffWindow();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: 'Write off successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            BindCollectionGrid();
            RadGrid_Collections.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);

        }

    }

    protected void lnkWriteOff_Click(object sender, EventArgs e)
    {

        // ScriptManager.RegisterStartupScript(this, Page.GetType(), "closeScript", "CloseWriteOffWindow();", true);
        try
        {
            BL_Contracts objBL_Contracts = new BL_Contracts();
            Contracts objProp_Contracts = new Contracts();
            User objPropUser = new User();
            int txtRef = Convert.ToInt32(txtInvoiceWriteOff.Text);
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.InvoiceID = txtRef;
            DataSet ds = new DataSet();

            if (IsAPIIntegrationEnable == "YES")
            {
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();

                ListGetInvoicesByID _lstGetInvoicesByID = new ListGetInvoicesByID();

                string APINAME = "iCollectionsAPI/iCollectionsList_GetInvoicesByID";

                _GetInvoicesByID.ConnConfig = Session["config"].ToString();
                _GetInvoicesByID.InvoiceID = txtRef;

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoicesByID);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetInvoicesByID = serializer.Deserialize<ListGetInvoicesByID>(_APIResponse.ResponseData);

                ds1 = _lstGetInvoicesByID.lstTable1.ToDataSet();
                ds2 = _lstGetInvoicesByID.lstTable2.ToDataSet();

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
                ds = objBL_Contracts.GetInvoicesByID(objProp_Contracts);
            }


            txtWriteOffCust.Text = ds.Tables[0].Rows[0]["customername"].ToString();
            txtWriteOffLoc.Text = ds.Tables[0].Rows[0]["locname"].ToString();
            txtDescription.Text = "Write off from account '" + txtWriteOffCustID.Text + "' on " + DateTime.Today.ToString("MM/dd/yyyy") + " by " + Session["username"].ToString();

            if (Convert.ToString(ds.Tables[0].Rows[0]["job"]) != "0")
            {
                txtWriteOffProject.Text = ds.Tables[0].Rows[0]["job"].ToString() + "-" + ds.Tables[0].Rows[0]["JobDecs"].ToString();
            }
            Customer objProp_Customer = new Customer();

            BL_Customer objBL_Customer = new BL_Customer();

            objProp_Customer.ConnConfig = Session["config"].ToString();

            objProp_Customer.ProjectJobID = Convert.ToInt32(ds.Tables[0].Rows[0]["job"]);

            objProp_Customer.Type = "0";


            DataSet dsCode = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.Billingmodule = "";

            List<GetActiveBillingCodeViewModel> _lstGetActiveBillingCode = new List<GetActiveBillingCodeViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "iCollectionsAPI/iCollectionsList_GetActiveBillingCode";

                _GetActiveBillingCode.ConnConfig = Session["config"].ToString();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetActiveBillingCode);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetActiveBillingCode = serializer.Deserialize<List<GetActiveBillingCodeViewModel>>(_APIResponse.ResponseData);
                dsCode = CommonMethods.ToDataSet<GetActiveBillingCodeViewModel>(_lstGetActiveBillingCode);
            }
            else
            {
                dsCode = new BL_BillCodes().GetActiveBillingCode(Session["config"].ToString());
            }



            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("BillCode");

            DataRow dr = dt.NewRow();
            dr["ID"] = 0;
            dr["BillCode"] = "-Select-";
            dt.Rows.Add(dr);

            for (int i = 0; i < dsCode.Tables[0].Rows.Count; i++)
            {
                dr = dt.NewRow();
                dr["ID"] = dsCode.Tables[0].Rows[i]["id"];
                dr["BillCode"] = dsCode.Tables[0].Rows[i]["fDesc"];
                dt.Rows.Add(dr);
            }

            ddlCode.DataSource = dt;
            ddlCode.DataBind();

            BusinessEntity.User objProp_User = new BusinessEntity.User();
            DataSet dscontrol = new DataSet();
            objProp_User.ConnConfig = Session["config"].ToString();

            dscontrol = objBL_User.getControl(objProp_User);


            if (dscontrol.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(dscontrol.Tables[0].Rows[0]["DefaultBillingCode"].ToString()) == 0)
                {
                    ddlCode.SelectedValue = "0";
                }
                else
                {
                    ddlCode.SelectedValue = dscontrol.Tables[0].Rows[0]["DefaultBillingCode"].ToString();
                }
                //if (Convert.ToString(dscontrol.Tables[0].Rows[0]["DefaultBillingCodeDesc"].ToString()) != "")
                //{
                //    ddlCode.SelectedItem.Text = Convert.ToString(dscontrol.Tables[0].Rows[0]["DefaultBillingCodeDesc"].ToString());
                //}

            }

            if (ds.Tables[0].Rows[0]["Status"].ToString() == "1")
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: 'This invoice is paid.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "closeScript", "CloseWriteOffWindow();", true);
            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "closeScript", "CloseWriteOffWindow();", true);
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);

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

    protected void RadGrid_gvLogs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();
        DataSet dsLog = new DataSet();
        EmailLog emailLog = new EmailLog();
        emailLog.Screen = "Collections";
        emailLog.ConnConfig = Session["config"].ToString();
        BL_EmailLog bL_EmailLog = new BL_EmailLog();

        if (IsAPIIntegrationEnable == "YES")
        {
            List<GetEmailLogsViewModel> _lstGetEmailLogs = new List<GetEmailLogsViewModel>();

            string APINAME = "iCollectionsAPI/iCollectionsList_GetEmailLogs";

            _GetEmailLogs.Screen = "Collections";
            _GetEmailLogs.ConnConfig = Session["config"].ToString();

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetEmailLogs);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetEmailLogs = serializer.Deserialize<List<GetEmailLogsViewModel>>(_APIResponse.ResponseData);
            dsLog = CommonMethods.ToDataSet<GetEmailLogsViewModel>(_lstGetEmailLogs);
        }
        else
        {
            dsLog = bL_EmailLog.GetEmailLogs(emailLog);
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

    bool isGroupLog = false;

    public bool ShouldApplySortFilterOrGroupLogs()
    {
        return RadGrid_gvLogs.MasterTableView.FilterExpression != "" ||
            (RadGrid_gvLogs.MasterTableView.GroupByExpressions.Count > 0 || isGroupLog) ||
            RadGrid_gvLogs.MasterTableView.SortExpressions.Count > 0;
    }

    private void PrintInvoiceWithTicketMRT(DataTable _dtInvoice, string templateName)
    {
        // Export to PDF
        int invoiceNo = 0;
        List<byte[]> lstbyte = new List<byte[]>();
        try
        {
            foreach (DataRow drow in _dtInvoice.Rows)
            {

                StiWebViewer rvTemplate = new StiWebViewer();
                invoiceNo = (int)drow[1];
                ViewState["invoiceNo"] = invoiceNo;
                List<byte[]> invoicesToPrint = GetInvoiceWithTicketReport(rvTemplate, invoiceNo, templateName);

                if (invoicesToPrint != null)
                {
                    byte[] buffer1 = null;
                    buffer1 = concatAndAddContent(invoicesToPrint);
                    lstbyte.Add(buffer1);
                }

            }
            String fileName = "InvoiceWithTicket.pdf";
            if (_dtInvoice.Rows.Count == 1)
            {
                fileName = "InvoiceWithTicket_" + invoiceNo + ".pdf";
            }
            byte[] allbyte = Invoices.concatAndAddContent(lstbyte);
            Response.Clear();
            MemoryStream ms = new MemoryStream(allbyte);
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", string.Format("attachment;filename={0}", fileName));
            Response.AddHeader("Content-Length", (allbyte.Length).ToString());
            Response.Buffer = true;
            ms.WriteTo(Response.OutputStream);
            Response.End();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }
    private List<byte[]> GetInvoiceWithTicketReport(StiWebViewer rvTemplate, int invoiceNo, string templateName)
    {
        // Export to PDF
        List<byte[]> templateAsBytes = new List<byte[]>();
        try
        {
            DataSet ds = new DataSet();
            DataSet dsInv = new DataSet();
            DataSet dsTicket = new DataSet();
            Contracts objProp_Contracts = new Contracts();
            BL_Contracts objBL_Contracts = new BL_Contracts();
            BL_Report bL_Report = new BL_Report();
            BL_MapData objBL_MapData = new BL_MapData();
            MapData objMapData = new MapData();
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            _GetInvoicesByRef.ConnConfig = Session["config"].ToString();
            _GetAllTicketID.ConnConfig = Session["config"].ToString();

            string reportPathStimul = string.Empty;
            reportPathStimul = Server.MapPath(string.Format("StimulsoftReports/Invoices/{0}", templateName));
            StiReport report = new StiReport();
            report.Load(reportPathStimul);
            //report.Compile();

            DataSet companyInfo = new DataSet();

            //if (IsAPIIntegrationEnable == "YES")
            //{
            //    List<GetCompanyDetailsViewModel> _GetCompanyDetailsViewModel = new List<GetCompanyDetailsViewModel>();

            //    string APINAME = "iCollectionsAPI/iCollectionsList_GetCompanyDetails";

            //    _GetCompanyDetails.ConnConfig = Session["config"].ToString();

            //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCompanyDetails);
            //    JavaScriptSerializer serializer = new JavaScriptSerializer();

            //    serializer.MaxJsonLength = Int32.MaxValue;
            //    _GetCompanyDetailsViewModel = serializer.Deserialize<List<GetCompanyDetailsViewModel>>(_APIResponse.ResponseData);
            //    companyInfo = CommonMethods.ToDataSet<GetCompanyDetailsViewModel>(_GetCompanyDetailsViewModel);
            //}
            //else
            //{
            companyInfo = bL_Report.GetCompanyDetails(Session["config"].ToString());
            //}


            report.RegData("CompanyDetails", companyInfo.Tables[0]);

            DataTable dtInvoice = new DataTable();
            objProp_Contracts.InvoiceID = invoiceNo;
            _GetInvoicesByRef.InvoiceID = invoiceNo;

            if (IsAPIIntegrationEnable == "YES")
            {
                List<GetInvoicesByRefViewModel> _lstGetInvoicesByRef = new List<GetInvoicesByRefViewModel>();

                string APINAME = "iCollectionsAPI/iCollectionsList_GetInvoicesByRef";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoicesByRef);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetInvoicesByRef = serializer.Deserialize<List<GetInvoicesByRefViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetInvoicesByRefViewModel>(_lstGetInvoicesByRef);
            }
            else
            {
                ds = objBL_Contracts.GetInvoicesByRef(objProp_Contracts);
            }

            dtInvoice = ds.Tables[0];

            DataTable dtInvItems = GetInvoiceItems(invoiceNo);

            DataTable dtTicket = new DataTable();
            int ii = 0;

            objProp_Contracts.InvoiceID = invoiceNo;

            DataSet TicketID = new DataSet();

            if (IsAPIIntegrationEnable == "YES")
            {
                List<GetTicketIDViewModel> _lstGetTicketID = new List<GetTicketIDViewModel>();

                string APINAME = "iCollectionsAPI/iCollectionsList_GetAllTicketID";

                _GetAllTicketID.InvoiceID = invoiceNo;

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetAllTicketID);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetTicketID = serializer.Deserialize<List<GetTicketIDViewModel>>(_APIResponse.ResponseData);
                TicketID = CommonMethods.ToDataSet<GetTicketIDViewModel>(_lstGetTicketID);
            }
            else
            {
                TicketID = objBL_Contracts.GetAllTicketID(objProp_Contracts);
            }


            foreach (DataRow item in TicketID.Tables[0].Rows)
            {
                objMapData.ConnConfig = Session["config"].ToString();
                objMapData.TicketID = (int)item[0];
                dsTicket = objBL_MapData.GetTicketByID(objMapData);
                if (ii == 0)
                {
                    dtTicket = dsTicket.Tables[0];
                    ii++;
                }
                else
                {
                    dtTicket.Rows.Add(dsTicket.Tables[0].Rows[0].ItemArray);
                    ii++;
                }
            }

            report.RegData("InvoiceInfo", dtInvoice);
            report.RegData("InvoiceItems", dtInvItems);
            report.RegData("Tickets", dtTicket);

            var listTicketID = TicketID.Tables[0].Rows.OfType<DataRow>()
                  .Select(dr => dr.Field<int>("ID")).ToList();

            if (listTicketID.Count > 0)
            {
                objMapData.ConnConfig = Session["config"].ToString();
                DataSet dsEquips = new DataSet();

                if (IsAPIIntegrationEnable == "YES")
                {
                    List<GetElevByTicketIDsViewModel> _lstGetElevByTicketIDs = new List<GetElevByTicketIDsViewModel>();

                    string APINAME = "iCollectionsAPI/iCollectionsList_GetElevByTicketIDs";

                    _GetElevByTicketIDs.ConnConfig = Session["config"].ToString();
                    _GetElevByTicketIDs.ticketIDs = string.Join(",", listTicketID);

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetElevByTicketIDs);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetElevByTicketIDs = serializer.Deserialize<List<GetElevByTicketIDsViewModel>>(_APIResponse.ResponseData);
                    dsEquips = CommonMethods.ToDataSet<GetElevByTicketIDsViewModel>(_lstGetElevByTicketIDs);
                }
                else
                {
                    dsEquips = objBL_MapData.GetElevByTicketIDs(objMapData, string.Join(",", listTicketID));
                }


                if (dsEquips != null)
                {
                    report.RegData("dtEquipment", dsEquips.Tables[0]);
                }
            }

            report.CacheAllData = true;

            report.Dictionary.Synchronize();
            report.Render();
            rvTemplate.Report = report;
            byte[] buffer1 = null;
            var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            var service = new Stimulsoft.Report.Export.StiPdfExportService();
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            service.ExportTo(rvTemplate.Report, stream, settings);
            buffer1 = stream.ToArray();
            templateAsBytes.Add(buffer1);

            return templateAsBytes;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr753", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return templateAsBytes;
        }
    }

    protected void RadGrid_Collections_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;
            Session["RadGrid_CollectionsCurrentPageIndex"] = e.NewPageIndex;
            ViewState["RadGrid_CollectionsminimumRows"] = e.NewPageIndex * RadGrid_Collections.PageSize;
            ViewState["RadGrid_CollectionsmaximumRows"] = (e.NewPageIndex + 1) * RadGrid_Collections.PageSize;
        }
        catch { }
    }

    protected void RadGrid_Collections_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;
            ViewState["RadGrid_CollectionsminimumRows"] = RadGrid_Collections.CurrentPageIndex * e.NewPageSize;
            ViewState["RadGrid_CollectionsmaximumRows"] = (RadGrid_Collections.CurrentPageIndex + 1) * e.NewPageSize;
        }
        catch { }
    }

    private void SaveFilter()
    {

        var lsDep = string.Empty;
        if (rcDepartment.CheckedItems.Count > 0)
        {
            foreach (var item in rcDepartment.CheckedItems)
            {
                lsDep += item.Value + ",";
            }

            lsDep = lsDep.TrimEnd(',');
        }

        Session["filterstate"] = txtEndDate.Text + ";"
            + drpPrintEmail.SelectedValue + ";"
            + lsDep + ";";


    }

    public void UpdateControl()
    {
        IsGridPageIndexChanged = true;
        if (Session["filterstate"] != null)
        {
            if (Session["filterstate"].ToString() != string.Empty)
            {
                string[] strFilter = Session["filterstate"].ToString().Split(';');
                txtEndDate.Text = strFilter[0];

                drpPrintEmail.SelectedValue = strFilter[1];

                if (!string.IsNullOrEmpty(strFilter[2]))
                {
                    var selectedValArr = strFilter[2].Split(',');
                    foreach (RadComboBoxItem item in rcDepartment.Items)
                    {
                        if (Array.IndexOf(selectedValArr, item.Value) >= 0)
                        {
                            item.Checked = true;
                        }
                    }
                }

            }
        }
    }

    private void CheckValidEmails(List<string> emails, StringBuilder sbdInValidEmails)
    {
        if (sbdInValidEmails == null) sbdInValidEmails = new StringBuilder();

        foreach (var item in emails.ToList())
        {
            if (!WebBaseUtility.IsValidEmailAddress1(item))
            {
                emails.Remove(item);
                sbdInValidEmails.AppendFormat("{0}<br/>", item);
            }
        }
    }


    public string ShowLocNote(object loc)
    {
        string result = string.Empty;

        if (!string.IsNullOrEmpty(loc.ToString()))
        {
            if (ViewState["dsLocNote"] != null)
            {
                DataSet ds = new DataSet();
                ds = (DataSet)ViewState["dsLocNote"];

                if (ds.Tables[0].Select("LocId = " + Convert.ToInt32(loc)).Count() > 0)
                {
                    DataRow[] rows = ds.Tables[0].Select("LocId = " + Convert.ToInt32(loc), "CreatedDate DESC");
                    result = "<B>User</B>: " + Convert.ToString(rows[0]["CreatedBy"]);
                    result += "<br/><br/><B>Date</B>:" + Convert.ToString(rows[0]["CreatedDate"]);
                    result += "<br/><br/><B>Resolution</B>:" + Convert.ToString(rows[0]["Notes"]);
                }
            }
        }

        return result;
    }

    public string ShowCusNote(object cust)
    {
        string result = string.Empty;

        if (!string.IsNullOrEmpty(cust.ToString()))
        {
            if (ViewState["dsCusNote"] != null)
            {
                DataSet ds = new DataSet();
                ds = (DataSet)ViewState["dsCusNote"];

                if (ds.Tables[0].Select("OwnerID = " + Convert.ToInt32(cust)).Count() > 0)
                {
                    DataRow[] rows = ds.Tables[0].Select("OwnerID = " + Convert.ToInt32(cust), "CreatedDate DESC");
                    result = "<B>User</B>: " + Convert.ToString(rows[0]["CreatedBy"]);
                    result += "<br/><br/><B>Date</B>:" + Convert.ToString(rows[0]["CreatedDate"]);
                    result += "<br/><br/><B>Resolution</B>:" + Convert.ToString(rows[0]["Notes"]);
                }
            }
        }

        return result;
    }

    private void getCollectionNotes()
    {
        //Get Customer Note
        DataSet dsCusNote = new DataSet();
        BL_Collection objCusNote = new BL_Collection();
        dsCusNote = objCusNote.GetCollectionCustomerNote(Convert.ToString(Session["config"]), Convert.ToDateTime(txtEndDate.Text));
        ViewState["dsCusNote"] = dsCusNote;
        //Get Location Note
        DataSet dsLocNote = new DataSet();
        BL_Collection objLocNote = new BL_Collection();
        dsLocNote = objLocNote.GetCollectionLocationNote(Convert.ToString(Session["config"]), Convert.ToDateTime(txtEndDate.Text));
        ViewState["dsLocNote"] = dsLocNote;

    }





    protected void RadGrid_Collections_ExcelMLWorkBookCreated(object sender, GridExcelMLWorkBookCreatedEventArgs e)
    {
        var currentItem = 3;
        GridFooterItem footerItem = RadGrid_Collections.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
        RowElement row = new RowElement(); //create new row for the footer aggregates
        for (int i = currentItem; i < footerItem.Cells.Count; i++)
        {
            TableCell fcell = footerItem.Cells[i];
            CellElement cell = new CellElement();
            // cell.Data.DataItem =  fcell.Text == "&nbsp;" ? "" : fcell.Text;
            if (i == currentItem)
                cell.Data.DataItem = "Total:-";
            else
            {
                if (fcell.Text == "&nbsp;")
                {
                    cell.Data.DataItem = "";
                }
                else
                {
                    cell.Data.DataItem = fcell.Text;
                }

                row.Cells.Add(cell);
            }

        }
        row.Cells[0].Data.DataItem = "Total:-";
        e.WorkBook.Worksheets[0].Table.Rows.Add(row);

    }

    protected void btnRefressScreen_Click(object sender, EventArgs e)
    {
        getCollectionNotes();
        DataTable dt = (DataTable)Session["CollectionARData"];
        RadGrid_Collections.VirtualItemCount = dt.Rows.Count;
        RadGrid_Collections.DataSource = dt;
        RadGrid_Collections.Rebind();


    }

    protected void lnkARAgingReportByBusinessType_Click(object sender, EventArgs e)
    {
        Response.Redirect("ARAgingReportByBusinessType.aspx?page=iCollections", true);
    }
    private String getBusinessTypeLabel()
    {
        BL_Customer objBL_Customer = new BL_Customer();
        Customer objCustomer = new Customer();
        objCustomer.ConnConfig = Session["config"].ToString();

        DataSet ds = new DataSet();
        ds = objBL_Customer.getBT(objCustomer);
        try
        {
            return ds.Tables[0].Rows[0]["Label"].ToString();
        }
        catch
        {
            return "Business Type";
        }
    }
    protected void lnkCredit_Click(object sender, EventArgs e)
    {

        // ScriptManager.RegisterStartupScript(this, Page.GetType(), "closeScript", "CloseWriteOffWindow();", true);
        try
        {

            BL_Contracts objBL_Contracts = new BL_Contracts();
            Contracts objProp_Contracts = new Contracts();
            User objPropUser = new User();
            int txtRef = Convert.ToInt32(hdnInvoiceCredit.Value);
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.InvoiceID = txtRef;

            _GetInvoicesByID.ConnConfig = Session["config"].ToString();
            _GetInvoicesByID.InvoiceID = txtRef;

            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();

            ListGetInvoicesByID _lstGetInvoicesByID = new ListGetInvoicesByID();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "ReceivePaymentAPI/AddReceivePayment_GetInvoicesByID";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoicesByID);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetInvoicesByID = serializer.Deserialize<ListGetInvoicesByID>(_APIResponse.ResponseData);

                ds1 = _lstGetInvoicesByID.lstTable1.ToDataSet();
                ds2 = _lstGetInvoicesByID.lstTable2.ToDataSet();

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

                ds = objBL_Contracts.GetInvoicesByID(objProp_Contracts);


            }


            txtDescriptionCredit.Text = "Credit from Invoice# " + hdnInvoiceCredit.Value + " by " + Session["username"].ToString();




            Customer objProp_Customer = new Customer();

            BL_Customer objBL_Customer = new BL_Customer();

            objProp_Customer.ConnConfig = Session["config"].ToString();

            if (ds.Tables[0].Columns.Contains("job") && !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["job"].ToString()))
            {
                objProp_Customer.ProjectJobID = Convert.ToInt32(ds.Tables[0].Rows[0]["job"]);
                hdnJobIDCredit.Value = Convert.ToString(ds.Tables[0].Rows[0]["job"].ToString());
            }


            objProp_Customer.Type = "0";



        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "closeScript", "CloseCreditWindow();", true);
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);

        }


    }
    private void GetPeriodDetails(DateTime transDate)
    {
        bool Flag = CommonHelper.GetPeriodDetails(transDate);
        ViewState["FlagPeriodClose"] = Flag;
        if (!Flag)
        {
            divSuccess.Visible = true;
        }
    }
    protected void lnkSaveCredit_Click(object sender, EventArgs e)
    {
        try
        {

            Boolean isProjectClose = false;
            bool IsValid = true;
            DateTime receivedt;
            receivedt = Convert.ToDateTime(txtCreditDate.Text);
            GetPeriodDetails(receivedt);     //Check period closed out permission
            bool Flag = (bool)ViewState["FlagPeriodClose"];
            if (!Flag)
            {
                IsValid = false;
            }
            if (!string.IsNullOrEmpty(hdnJobIDCredit.Value.ToString()))
            {
                JobT objJob = new JobT();
                BL_Job objBL_Job = new BL_Job();
                objJob.ConnConfig = Session["config"].ToString();
                objJob.ID = Convert.ToInt32(hdnJobIDCredit.Value);

                DataSet dsJ = objBL_Job.GetJobById(objJob);
                if (dsJ != null && dsJ.Tables.Count > 0)
                {
                    if (dsJ.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dsJ.Tables[0].Rows[0]["Status"]) == 1)
                        {
                            isProjectClose = true;
                        }
                    }

                }
            }

            if (IsValid)
            {
                if (isProjectClose)
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "closeScript", "CloseCreditWindow();", true);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Please note this project is closed. You will need to change it before saving.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                }
                else
                {
                    //-------------------------------------------*******************************************
                    WriteOff obj = new WriteOff();
                    BL_Deposit objBL_Deposit = new BL_Deposit();
                    obj.ConnConfig = Session["config"].ToString();

                    //obj.Acct = Convert.ToInt32(ddlCode.SelectedItem.Value);
                    obj.Acct = Convert.ToInt32(0);
                    obj.Desc = txtDescriptionCredit.Text;
                    obj.fDate = txtCreditDate.Text == "" ? DateTime.Now : Convert.ToDateTime(txtCreditDate.Text);
                    obj.CreateBy = Session["username"].ToString();

                    obj.TransID = Convert.ToInt32(hdnTransIDCredit.Value);
                    obj.CheckNo = "";
                    obj.WriteoffDesc = "Credit from Invoice# " + hdnInvoiceCredit.Value + " by " + Session["username"].ToString();
                    obj.ID = Convert.ToInt32(hdnInvoiceCredit.Value);
                    objBL_Deposit.writeOffInvoice(obj);
                    //--------------------------------------------------------------------------------------

                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "closeScript", "CloseCreditWindow();", true);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: 'Credit successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    BindCollectionGrid();
                    RadGrid_Collections.Rebind();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "closeScript", "CloseCreditWindow();", true);
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'These month/year period is closed out. You do not have permission to add/update this record.',  type : 'Warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);

        }

    }
}

