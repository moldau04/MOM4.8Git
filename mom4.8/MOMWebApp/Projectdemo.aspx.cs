using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLayer;
using BusinessEntity;
using System.Web.Services;
using System.Web.Script.Services;

public partial class Projectdemo : System.Web.UI.Page
{
    Customer objProp_Customer = new Customer();
    JobT objProp_job = new JobT();
    BL_Customer objBL_Customer = new BL_Customer();
    BL_Job objBL_Project = new BL_Job();
    BusinessEntity.User objPropUser = new BusinessEntity.User();
    BL_User objBL_User = new BL_User();

    private const string _asc = " ASC";
    private const string _desc = " DESC";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            //DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //int DaysinMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - 1;
            //DateTime lastDay = firstDay.AddDays(DaysinMonth);
            //txtfromDate.Text = firstDay.ToShortDateString();
            //txtToDate.Text = lastDay.ToShortDateString();
            Locations();

            BindProjectDepartments();
            BindSearchFilters();
            Permission();
        }
    }
    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("ProjectMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("ProjectLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkProject");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("SalesMgrSub");
        ////ul.Attributes.Remove("class");
        //ul.Style.Add("display", "block");

        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
        }

    }



    private void BindProjectDepartments()
    {
        DataSet ds = new DataSet();
        objProp_job.ConnConfig = Session["config"].ToString();
        ds = objBL_Project.GetAllJobTypeForSearch(objProp_job);

        rptDepartment.DataSource = ds.Tables[0];
        rptDepartment.DataBind();

    }



    protected void lnkEdit_Click(object sender, EventArgs e)
    {

    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {

    }
    protected void lnkAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("addproject.aspx");
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
        Session.Remove("ProjectTemp");
    }


    private void Locations()
    {
        if (Session["type"].ToString() == "c")
        {
            DataTable dtcust = new DataTable();
            dtcust = (DataTable)Session["userinfo"];
            int RoleID = 0;
            if (dtcust.Rows.Count > 0)
            {
                RoleID = Convert.ToInt32(dtcust.Rows[0]["roleid"]);
                objPropUser.RoleID = RoleID;
            }
        }

        DataSet ds = new DataSet();
        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.CustomerID = Convert.ToInt32(Session["custid"].ToString());
        if (Convert.ToInt32(Session["custid"].ToString()) != 0)
        {
            ds = objBL_User.getLocationByCustomerID(objPropUser);
        }
        else
        {
            ds = objBL_User.getLocations(objPropUser);
        }
        ddllocation.DataSource = ds.Tables[0];
        ddllocation.DataTextField = "tag";
        ddllocation.DataValueField = "loc";
        ddllocation.DataBind();

    }

    private void BindSearchFilters()
    {
        //Dictionary<string, string> listsearchitems = new Dictionary<string, string>();
        //listsearchitems.Add("0", "select");
        //listsearchitems.Add("Id", "Project#");
        ////listsearchitems.Add("fDate", "Date");
        //listsearchitems.Add("locname", "Location");
        ////listsearchitems.Add("VendorName", "Vendor Name");
        ////listsearchitems.Add("Due", "Due Date");
        //listsearchitems.Add("Status", "Status");
        //listsearchitems.Add("fdesc", "Description");


        //ddlSearch.DataSource = listsearchitems;
        //ddlSearch.DataTextField = "Value";
        //ddlSearch.DataValueField = "Key";
        //ddlSearch.DataBind();

    }



    #region ::WebMethods::
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static WebMethodResponse<Dictionary<string, object>> GetProject(string searchBy, string SearchValue, string page, string stdate, string enddate, int Department, string Range, string SortDir, string SortCol)
    {
        WebMethodResponse<Dictionary<string, object>> jsonPOInformation = new BusinessEntity.WebMethodResponse<Dictionary<string, object>>();
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        jsonPOInformation.Header = new BusinessEntity.WebMethodHeader();

        BL_Customer objBL_Customer = new BL_Customer();

        Customer objProp_Customer = new Customer();

        int noofrecordsperpage = string.IsNullOrEmpty(System.Web.Configuration.WebConfigurationManager.AppSettings["RecordPerPage"].Trim()) ? 10 : Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["RecordPerPage"].Trim());
        page = string.IsNullOrEmpty(page) ? "1" : page;
        int MaxRecord = (noofrecordsperpage * Convert.ToInt32(page)) - 1;

        int MinRecord = (MaxRecord - noofrecordsperpage) < 0 ? 0 : (MaxRecord - noofrecordsperpage) + 1;

        int pagecount = 1;

        dictionary.Add("Items", (new List<ProjectItemsJSON>()).ToArray());
        dictionary.Add("PageCount", pagecount);
        dictionary.Add("TotalCount", "0 Record(s) Found");
        dictionary.Add("Totals", null);


        try
        {

            objProp_Customer.SearchBy = searchBy;
            objProp_Customer.SearchValue = SearchValue;
            objProp_Customer.JobType = Convert.ToInt16(Department);
            objProp_Customer.StartDate = stdate;


            objProp_Customer.EndDate = enddate;
            objProp_Customer.ConnConfig = WebBaseUtility.ConnectionString;
            objProp_Customer.Range = Convert.ToInt16(Range);


            if (objProp_Customer.Range > 1)
            {

                if (string.IsNullOrEmpty(objProp_Customer.StartDate) || string.IsNullOrEmpty(objProp_Customer.EndDate))
                {
                    jsonPOInformation.Header.HasError = true;
                    List<string> strmsg = new List<string>();
                    strmsg.Add("Please select a date range.");
                    jsonPOInformation.Header.ErrorMessages = strmsg;
                }
                else
                {
                    if (Convert.ToDateTime(objProp_Customer.StartDate) > Convert.ToDateTime(objProp_Customer.EndDate))
                    {
                        jsonPOInformation.Header.HasError = true;
                        List<string> strmsg = new List<string>();
                        strmsg.Add("Start date cannot be greater than end date.");
                        jsonPOInformation.Header.ErrorMessages = strmsg;

                    }
                }

            }

            if (!jsonPOInformation.Header.HasError)
            {
                DataTable dtFilters = CreateFiltersToDataTable();
                List<ProjectItemsJSON> items = JSONMappingUtility.ProjectItemsMappingJSON(objBL_Customer.getJobProject(objProp_Customer, dtFilters),SortDir, SortCol);



                if (items != null)
                {


                    if (items.Count > 0)
                    {
                        var matchedData = (from item in items where item.ID != "0" select item).ToList();
                        if (matchedData != null)
                        {
                            if (matchedData.Count > 0)
                            {
                                pagecount = (matchedData.Count + noofrecordsperpage - 1) / noofrecordsperpage; //Convert.ToInt32(Math.Round((matchedData.Count / Convert.ToDecimal(noofrecordsperpage))));
                                List<ProjectItemsJSON> pageditems = new List<ProjectItemsJSON>();

                                for (int i = 0; i < matchedData.Count; i++)
                                {
                                    //Check If the records found are less than the no of reords that needs to be displayed in the gird
                                    if (pagecount > 0)
                                    {
                                        if (i >= MinRecord && i <= MaxRecord)
                                            pageditems.Add(matchedData[i]);
                                    }
                                    else
                                        pageditems.Add(matchedData[i]);
                                }
                                dictionary["Items"] = pageditems.ToArray();
                                dictionary["PageCount"] = pagecount > 0 ? pagecount : 1;
                                dictionary["TotalCount"] = matchedData.Count + " Record(s) Found";
                            }
                        }

                        var Totals = (from item in items where item.ID == "0" select item).SingleOrDefault();

                        dictionary["Totals"] = Totals;

                    }



                    if (dictionary.Count > 0)
                        jsonPOInformation.Header.HasError = false;
                    else
                        jsonPOInformation.Header.HasError = true;


                    jsonPOInformation.ReponseObject = dictionary;
                }
                else
                {
                    jsonPOInformation.Header.HasError = true;

                }
            }
        }
        catch (Exception ex)
        {
            string errormsg = ex.Message;
            List<string> strmsg = new List<string>();
            strmsg.Add(errormsg);

            jsonPOInformation.Header.HasError = true;
            jsonPOInformation.Header.ErrorMessages = strmsg;
        }
        return jsonPOInformation;
    }

    private  static DataTable CreateFiltersToDataTable()
    {
        //create new table to add filter values.
        DataTable dtFilters = new DataTable();
        dtFilters.Clear();
        dtFilters.Columns.Add("Customer");
        dtFilters.Columns.Add("Tag");
        dtFilters.Columns.Add("ID");
        dtFilters.Columns.Add("fdesc");
        dtFilters.Columns.Add("Status");
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

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static WebMethodResponse<string> DeleteProject(object[] items)
    {
        WebMethodResponse<string> jsonPOInformation = new BusinessEntity.WebMethodResponse<string>();
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        jsonPOInformation.Header = new BusinessEntity.WebMethodHeader();
        BL_Customer objBL_Customer = new BL_Customer();

        Customer objProp_Customer = new Customer();


        try
        {

            objProp_Customer.ConnConfig = WebBaseUtility.ConnectionString;
            if (items != null)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    object id = 0;
                    Dictionary<string, object> x = ((Dictionary<string, object>)(items[i]));
                    if (x.TryGetValue("ID", out id))
                    {
                        objProp_Customer.ProjectJobID = Convert.ToInt32(id);
                        objBL_Customer.DeleteProject(objProp_Customer);
                        jsonPOInformation.Header.HasError = false;
                        jsonPOInformation.ReponseObject = "Project " + id + " deleted successfully";
                    }

                }
                // objProp_Customer.ProjectJobID = Convert.ToInt32(lblProspectID.Text);
                // objBL_Customer.DeleteProject(objProp_Customer);


            }






        }
        catch (Exception ex)
        {
            string errormsg = ex.Message;
            List<string> strmsg = new List<string>();
            strmsg.Add(errormsg);

            jsonPOInformation.Header.HasError = true;
            jsonPOInformation.Header.ErrorMessages = strmsg;
        }
        return jsonPOInformation;
    }
    #endregion
}
