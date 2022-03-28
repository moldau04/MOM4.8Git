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

public partial class ProjectDep : System.Web.UI.Page
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
    protected void Page_PreRender(Object o, EventArgs e)
    {
        //foreach (GridViewRow gr in gvProject.Rows)
        //{
        //    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
        //    Label lblID = (Label)gr.FindControl("lblId");

        //    gr.Attributes["onclick"] = "SelectRowChk('" + gr.ClientID + "','" + chkSelect.ClientID + "','" + gvProject.ClientID + "',event);";
        //    //gr.Attributes["ondblclick"] = "location.href='addproject.aspx?uid=" + lblID.Text + "'";
        //    gr.Attributes["ondblclick"] = "window.open('addproject.aspx?uid=" + lblID.Text + "','_blank');";
        //}
        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "key8", "SelectedRowStyle('" + gvProject.ClientID + "');", true);
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


    }

    private void BindSearchFilters()
    {
        Dictionary<string, string> listsearchitems = new Dictionary<string, string>();
        listsearchitems.Add("0", "select");
        listsearchitems.Add("Id", "Project#");
        //listsearchitems.Add("fDate", "Date");
        listsearchitems.Add("locname", "Location");
        //listsearchitems.Add("VendorName", "Vendor Name");
        //listsearchitems.Add("Due", "Due Date");
        listsearchitems.Add("Status", "Status");
        listsearchitems.Add("fdesc", "Description");


        ddlSearch.DataSource = listsearchitems;
        ddlSearch.DataTextField = "Value";
        ddlSearch.DataValueField = "Key";
        ddlSearch.DataBind();

    }



    #region ::WebMethods::
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static WebMethodResponse<Dictionary<string, object>> GetProject(string searchTerm, string column, string page, string stdate, string enddate, int Department)
    {
        WebMethodResponse<Dictionary<string, object>> jsonPOInformation = new BusinessEntity.WebMethodResponse<Dictionary<string, object>>();
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        jsonPOInformation.Header = new BusinessEntity.WebMethodHeader();

        BL_Job _objBLJobs = new BL_Job();
        JobT _objJob = new JobT();
        DataSet dt = null;


        int noofrecordsperpage = string.IsNullOrEmpty(System.Web.Configuration.WebConfigurationManager.AppSettings["RecordPerPage"].Trim()) ? 10 : Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["RecordPerPage"].Trim());
        page = string.IsNullOrEmpty(page) ? "1" : page;
        int MaxRecord = (noofrecordsperpage * Convert.ToInt32(page)) - 1;

        int MinRecord = (MaxRecord - noofrecordsperpage) < 0 ? 0 : (MaxRecord - noofrecordsperpage) + 1;

        int pagecount = 1;


        try
        {
            dt = _objBLJobs.GetAllJobTypeForAjaxSearch(Department);



            if (dt != null)
            {


                if (dt.Tables.Count > 0)
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {



                        #region Valid SearchTerms
                        if (!string.IsNullOrEmpty(searchTerm) && dt.Tables[0].Columns[column] != null)
                        {

                            int searchindex = dt.Tables[0].Columns[column].Ordinal;

                            string filterexp = !string.IsNullOrEmpty(stdate) & !string.IsNullOrEmpty(enddate) ?
                             "fDate>='" + Convert.ToDateTime(stdate).ToString("MM/dd/yyyy") + "' and fDate<='" + Convert.ToDateTime(enddate).ToString("MM/dd/yyyy") + "'" : (!string.IsNullOrEmpty(stdate) & string.IsNullOrEmpty(enddate) ?

                             "fDate>='" + Convert.ToDateTime(stdate).ToString("MM/dd/yyyy") + "'" : string.Empty);

                            var dr = string.IsNullOrEmpty(filterexp) ? dt.Tables[0].Rows.Cast<DataRow>().ToList().Select(x => x.ItemArray) :
                                dt.Tables[0].Select(filterexp).Cast<DataRow>().ToList().Select(x => x.ItemArray);

                            var matchedData = (from projectitem in dr select projectitem).ToList();
                            if (matchedData != null)
                            {

                                var itemsfound = matchedData.Where(x => x[1] == "").ToList();
                                if (column == "Id")
                                {

                                    List<object> projectitemid = new List<object>();
                                    itemsfound = (from projectitem in matchedData
                                                  where Convert.ToString(projectitem[1]) == searchTerm
                                                  select projectitem).ToList();


                                }
                                else
                                    itemsfound = matchedData.Where(x => Convert.ToString(x[searchindex]).ToUpper().Contains(searchTerm.ToUpper())).ToList();
                                // matchedData.Where(c => c.SearchCol.ToUpper().Contains(searchTerm.ToUpper())).ToList();

                                if (itemsfound != null)
                                {
                                    if (itemsfound.Count > 0)
                                    {
                                        pagecount = Convert.ToInt32(Math.Round((itemsfound.Count / Convert.ToDecimal(noofrecordsperpage))));
                                        List<object> pageditems = new List<object>();

                                        for (int i = 0; i < itemsfound.Count; i++)
                                        {
                                            //Check If the records found are less than the no of reords that needs to be displayed in the gird
                                            if (pagecount > 0)
                                            {
                                                if (i >= MinRecord && i <= MaxRecord)
                                                    pageditems.Add(itemsfound[i]);
                                            }
                                            else
                                                pageditems.Add(itemsfound[i]);

                                        }

                                        dictionary.Add("Items", pageditems.ToArray());
                                        dictionary.Add("PageCount", pagecount > 0 ? pagecount : 1);

                                    }
                                }
                            }

                        }

                        #endregion

                        #region Typo
                        else if (!string.IsNullOrEmpty(searchTerm))
                        {

                            string filterexp = !string.IsNullOrEmpty(stdate) & !string.IsNullOrEmpty(enddate) ?
                             "fDate>='" + Convert.ToDateTime(stdate).ToString("MM/dd/yyyy") + "' and fDate<='" + Convert.ToDateTime(enddate).ToString("MM/dd/yyyy") + "'" : (!string.IsNullOrEmpty(stdate) & string.IsNullOrEmpty(enddate) ?

                             "fDate>='" + Convert.ToDateTime(stdate).ToString("MM/dd/yyyy") + "'" : string.Empty);

                            var dr = string.IsNullOrEmpty(filterexp) ? dt.Tables[0].Rows.Cast<DataRow>().ToList().Select(x => x.ItemArray) :
                                dt.Tables[0].Select(filterexp).Cast<DataRow>().ToList().Select(x => x.ItemArray);

                            var matchedData = (from items in dr select new { ID = Convert.ToString(items[1]), FDate = Convert.ToString(items[5]), Description = Convert.ToString(items[2]), location = Convert.ToString(items[4]), status = Convert.ToString(items[3]) }).ToList();
                            if (matchedData != null)
                            {
                                var searchcolfound = matchedData.Where(c => c.ID.ToUpper().Contains(searchTerm.ToUpper())).ToList();


                                if (searchcolfound == null || searchcolfound.Count == 0)
                                {
                                    searchcolfound = matchedData.Where(c => c.FDate.ToUpper().Contains(searchTerm.ToUpper())).ToList();
                                }
                                if (searchcolfound == null || searchcolfound.Count == 0)
                                {
                                    searchcolfound = matchedData.Where(c => c.location.ToUpper().Contains(searchTerm.ToUpper())).ToList();
                                }
                                if (searchcolfound == null || searchcolfound.Count == 0)
                                {
                                    searchcolfound = matchedData.Where(c => c.status.ToUpper().Contains(searchTerm.ToUpper())).ToList();
                                }
                                if (searchcolfound == null || searchcolfound.Count == 0)
                                {
                                    searchcolfound = matchedData.Where(c => c.Description.ToUpper().Contains(searchTerm.ToUpper())).ToList();
                                }
                                var itemsfound = (from x in dr join o in searchcolfound on Convert.ToString(x[1]) equals o.ID select x).ToList();
                                if (itemsfound != null)
                                {
                                    if (itemsfound.Count > 0)
                                    {
                                        pagecount = Convert.ToInt32(Math.Round((itemsfound.Count / Convert.ToDecimal(noofrecordsperpage))));
                                        List<object> pageditems = new List<object>();

                                        for (int i = 0; i < itemsfound.Count; i++)
                                        {
                                            //Check If the records found are less than the no of reords that needs to be displayed in the gird
                                            if (pagecount > 0)
                                            {
                                                if (i >= MinRecord && i <= MaxRecord)
                                                    pageditems.Add(itemsfound[i]);
                                            }
                                            else
                                                pageditems.Add(itemsfound[i]);

                                        }

                                        dictionary.Add("Items", pageditems.ToArray());
                                        dictionary.Add("PageCount", pagecount > 0 ? pagecount : 1);

                                    }
                                }
                            }
                        }
                        #endregion

                        #region Empty Search
                        else
                        {

                            string filterexp = !string.IsNullOrEmpty(stdate) & !string.IsNullOrEmpty(enddate) ?
                              "fDate>='" + Convert.ToDateTime(stdate).ToString("MM/dd/yyyy") + "' and fDate<='" + Convert.ToDateTime(enddate).ToString("MM/dd/yyyy") + "'" : (!string.IsNullOrEmpty(stdate) & string.IsNullOrEmpty(enddate) ?

                              "fDate>='" + Convert.ToDateTime(stdate).ToString("MM/dd/yyyy") + "'" : string.Empty);

                            var dr = string.IsNullOrEmpty(filterexp) ? dt.Tables[0].Rows.Cast<DataRow>().ToList().Select(x => x.ItemArray) :
                                dt.Tables[0].Select(filterexp).Cast<DataRow>().ToList().Select(x => x.ItemArray);

                            var matchedData = (from items in dr select items).ToList();
                            if (matchedData != null)
                            {
                                if (matchedData.Count > 0)
                                {
                                    pagecount = (matchedData.Count + noofrecordsperpage - 1) / noofrecordsperpage; //Convert.ToInt32(Math.Round((matchedData.Count / Convert.ToDecimal(noofrecordsperpage))));
                                    List<object> pageditems = new List<object>();

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
                                    dictionary.Add("Items", pageditems.ToArray());
                                    dictionary.Add("PageCount", pagecount > 0 ? pagecount : 1);
                                }

                            }


                        }
                        #endregion

                    }
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
