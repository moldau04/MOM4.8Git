using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLayer;
using BusinessEntity;
using System.Web.Script.Serialization;
using Telerik.Web.UI;
using Telerik.Web.UI.GridExcelBuilder;
using System.Configuration;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;

public partial class Estimate : System.Web.UI.Page
{
    #region Variables
    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();

    GeneralFunctions objGeneralFunctions = new GeneralFunctions();

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    BL_ReportsData objBL_ReportsData = new BL_ReportsData();
    Lead leadData = new Lead();
    BL_Lead objBL_Lead = new BL_Lead();
    #endregion
    #region PAGELOAD
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

            if (string.IsNullOrWhiteSpace(GetDefaultGridColumnSettingsFromDb()))
            {
                // Get initial grid settings
                var gridDefault = GetGridColumnSettings();
                // Save default settings to database
                objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
                objProp_User.UserID = 0;// UserId = 0 for default
                objProp_User.PageName = "Estimate.aspx";
                objProp_User.GridId = "RadGrid_Estimate";

                objBL_User.UpdateUserGridCustomSettings(objProp_User, gridDefault);
            }

            BindTemplate();
            BindDepartment();
            BindOpportunityStage();
            BindCustomFields();
            #region Show Selected Filter
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["from_estimate"] != null && Session["end_estimate"] != null)
                {
                    txtFromDate.Text = Convert.ToString(Session["from_estimate"]);
                    txtToDate.Text = Convert.ToString(Session["end_estimate"]);
                }
                else
                {
                    //txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                    //txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
                    txtFromDate.Text = string.Empty;
                    txtToDate.Text = string.Empty;
                }

                if (Session["ddlSearch_estimate"] != null)
                {
                    String selectedValue = Convert.ToString(Session["ddlSearch_estimate"]);
                    ddlSearch.SelectedValue = selectedValue;

                    String searchValue = Convert.ToString(Session["ddlSearch_Value_estimate"]);

                    if (selectedValue == "e.ID" || selectedValue == "e.EstimateAddress" || selectedValue == "e.CompanyName" || selectedValue == "e.Opportunity" || selectedValue == "e.job" || selectedValue == "e.Contact" || selectedValue == "e.Category")
                    {
                        txtSearch.Text = searchValue;
                    }
                    else if (selectedValue == "e.Status")
                    {
                        ddlSearch_SelectedIndexChanged(sender, e);
                        ddlStatus.SelectedValue = searchValue;
                    }
                    else if (selectedValue == "e.ffor")
                    {
                        ddlSearch_SelectedIndexChanged(sender, e);
                        ddlType.SelectedValue = searchValue;
                    }
                    else if (selectedValue == "e.Template")
                    {
                        ddlSearch_SelectedIndexChanged(sender, e);
                        ddlTemplate.SelectedValue = searchValue;
                    }
                    else if (selectedValue == "e.iscertifiedproject")
                    {
                        ddlSearch_SelectedIndexChanged(sender, e);
                        ddlCertified.SelectedValue = searchValue;
                    }
                    else if (selectedValue == "dep.ID")
                    {
                        ddlSearch_SelectedIndexChanged(sender, e);
                        ddlDepartment.SelectedValue = searchValue;
                    }
                    else if (selectedValue == "l.OpportunityStageID")
                    {
                        ddlSearch_SelectedIndexChanged(sender, e);
                        ddlOppStage.SelectedValue = searchValue;
                    }
                    //else if (selectedValue == "e.BDate")
                    //{
                    //    ddlSearch_SelectedIndexChanged(sender, e);
                    //    txtBidCloseDate.Text = Session["ddlSearch_Value_estimateFrDt"] != null ? Session["ddlSearch_Value_estimateFrDt"].ToString() : string.Empty;
                    //    txtBidCloseDateTo.Text = Session["ddlSearch_Value_estimateToDt"] != null ? Session["ddlSearch_Value_estimateToDt"].ToString() : string.Empty;
                    //}
                    else if (selectedValue.ToLower() == "customfield")
                    {
                        ddlSearch_SelectedIndexChanged(sender, e);
                        ddlCustomFields.SelectedValue = searchValue;
                        String searchValueExt = Convert.ToString(Session["ddlSearch_ValueExt_estimate"]);
                        txtCustomSearch.Text = searchValueExt;
                    }
                    else
                    {
                        txtSearch.Text = searchValue;
                    }
                }

                if (Session["ddlDateRange_estimate"] != null)
                {
                    String selectedValue = Convert.ToString(Session["ddlDateRange_estimate"]);
                    ddlDateRange.SelectedValue = selectedValue;
                }
            }
            else
            {
                // Refresh sessions
                Session["from_estimate"] = null;
                Session["end_estimate"] = null;
                Session["ddlSearch_estimate"] = null;
                Session["ddlSearch_Value_estimate"] = null;
                Session["ddlSearch_ValueExt_estimate"] = null;
                //Session["ddlSearch_Value_estimateFrDt"] = null;
                //Session["ddlSearch_Value_estimateToDt"] = null;
                Session["ddlDateRange_estimate"] = null;
                Session["Estimate_FilterExpression"] = null;
                Session["Estimate_Filters"] = null;
                txtFromDate.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
                txtToDate.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
            }
            #endregion

            string[] tokens = Session["config"].ToString().Split(';');
            if (tokens[1].ToString().ToLower().IndexOf("transel") != -1 || ConfigurationManager.AppSettings["CustomerName"].ToString().ToLower().Equals("transel"))
            {
                lnkExport.Visible = true;
                lnkExcel.Visible = false;
            }

            if (Session["ConvertToRecContractSucc"] != null && Session["ConvertToRecContractSucc"].ToString() == "1")
            {
                Session["ConvertToRecContractSucc"] = null;
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyConvertSuccess", "noty({text: 'Estimate Converted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        // Permission();
        UserPermission();
        HighlightSideMenu();
        CompanyPermission();
        ConvertToJSON();
    }

    private void HighlightSideMenu()
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl("SalesMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        //HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkEstimate");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl("SalesMgrSub");
        div.Style.Add("display", "block");
    }
    private void UserPermission()
    {

        // User Permission 
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = GetUserById();

            string SalesManagermodulePermission = ds.Rows[0]["SalesManager"] == DBNull.Value ? "Y" : ds.Rows[0]["SalesManager"].ToString();

            if (SalesManagermodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }

            string EstimatesPermission = ds.Rows[0]["Estimates"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Estimates"].ToString();
            string ADD = EstimatesPermission.Length < 1 ? "Y" : EstimatesPermission.Substring(0, 1);
            string Edit = EstimatesPermission.Length < 2 ? "Y" : EstimatesPermission.Substring(1, 1);
            string Delete = EstimatesPermission.Length < 2 ? "Y" : EstimatesPermission.Substring(2, 1);
            string View = EstimatesPermission.Length < 4 ? "Y" : EstimatesPermission.Substring(3, 1);
            string Report = EstimatesPermission.Length < 6 ? "Y" : EstimatesPermission.Substring(5, 1);

            string AwardEstimatesPermission = ds.Rows[0]["AwardEstimates"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["AwardEstimates"].ToString();

            string AwardEstimates = AwardEstimatesPermission.Length < 3 ? "Y" : AwardEstimatesPermission.Substring(2, 1);

            var isApproveProposal = ds.Rows[0]["EstApproveProposal"] == null ? false : Convert.ToBoolean(ds.Rows[0]["EstApproveProposal"]);

            if (isApproveProposal)
            {
                //RadGrid_Estimate.MasterTableView.GetColumn("lblApprStatus").Display = false;
                //RadGrid_Estimate.MasterTableView.GetColumn("ddlApprStatus").Display = true;
                ViewState["EstimateApproveProposalPermission"] = "Y";
            }
            else
            {
                //RadGrid_Estimate.MasterTableView.GetColumn("lblApprStatus").Display = true;
                //RadGrid_Estimate.MasterTableView.GetColumn("ddlApprStatus").Display = false;
                ViewState["EstimateApproveProposalPermission"] = "N";
            }


            if (AwardEstimates == "N")
            {
                lnkProject.Visible = false;
            }
            if (ADD == "N")
            {
                lnkAddnew.Visible = false;
            }
            if (Edit == "N")
            {
                lnkEdit.Visible = false;
                lnkCopy.Visible = false;
            }
            if (Delete == "N")
            {
                lnkDelete.Visible = false;

            }
            if (Report == "N")
            {
                lnkReport.Visible = false;
                lnkExcel.Visible = false;
            }
            if (View == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }

        }
        else if (Session["type"].ToString() == "am")
        {
            //RadGrid_Estimate.MasterTableView.GetColumn("lblApprStatus").Display = false;
            //RadGrid_Estimate.MasterTableView.GetColumn("ddlApprStatus").Display = true;
            ViewState["EstimateApproveProposalPermission"] = "Y";
        }
        else
        {
            //RadGrid_Estimate.MasterTableView.GetColumn("lblApprStatus").Display = true;
            //RadGrid_Estimate.MasterTableView.GetColumn("ddlApprStatus").Display = false;
            ViewState["EstimateApproveProposalPermission"] = "N";
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
    #endregion

    #region Custom Functions
    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            //RadGrid_Estimate.Columns[13].Visible = true;
            RadGrid_Estimate.MasterTableView.GetColumn("Company").Visible = true;
        }
        else
        {
            //RadGrid_Estimate.Columns[13].Visible = false;
            RadGrid_Estimate.MasterTableView.GetColumn("Company").Visible = false;
        }
    }
    private void BindTemplate()
    {
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        ds = objBL_Customer.getJobProjectTemplate(objProp_Customer);
        ddlTemplate.DataSource = ds.Tables[0];
        ddlTemplate.DataTextField = "fDesc";
        ddlTemplate.DataValueField = "id";
        ddlTemplate.DataBind();
    }
    private void BindDepartment()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getDepartment(objProp_User);
        ddlDepartment.DataSource = ds.Tables[0];
        ddlDepartment.DataTextField = "Type";
        ddlDepartment.DataValueField = "id";
        ddlDepartment.DataBind();
    }

    private void BindOpportunityStage()
    {
        //leadData.ConnConfig = Session["config"].ToString();
        //var ds = objBL_Lead.GetAllStage(leadData);
        objProp_Customer.ConnConfig = Session["config"].ToString();
        var ds = objBL_Customer.getStages(objProp_Customer);

        if (ds != null && ds.Tables[0] != null)
        {
            ddlOppStage.DataSource = ds.Tables[0];
            ddlOppStage.DataTextField = "DescWithProbability";
            ddlOppStage.DataValueField = "ID";
            ddlOppStage.DataBind();
        }
    }
    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("SalesMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("SalesLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.Master.FindControl("lnkEstimate");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("SalesMgrSub");
        //ul.Attributes.Remove("class");
        //ul.Style.Add("display", "block");

        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
        }

        if (Session["MSM"].ToString() == "TS")
        {
            lnkDelete.Visible = false;
        }
        //if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        //{
        //    Response.Redirect("home.aspx");
        //}

        if (Session["type"].ToString() != "am")
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["userinfo"];
            string Sales = dt.Rows[0]["sales"].ToString().Substring(0, 1);

            if (Sales == "N")
            {
                Response.Redirect("home.aspx");
            }
        }

    }
    private void FillEstimate()
    {
        try
        {
            //#region Save the Grid Filter
            //List<RetainFilter> filters = new List<RetainFilter>();
            //String filterExpression = Convert.ToString(RadGrid_Estimate.MasterTableView.FilterExpression);
            //if (string.IsNullOrEmpty(filterExpression) && Session["Estimate_FilterExpression"] != null)
            //{
            //    filterExpression = Session["Estimate_FilterExpression"].ToString();
            //}

            //if (filterExpression != "")
            //{
            //    foreach (GridColumn column in RadGrid_Estimate.MasterTableView.OwnerGrid.Columns)
            //    {
            //        String filterValues = column.CurrentFilterValue;
            //        if (filterValues != "")
            //        {
            //            String columnName = column.UniqueName;
            //            RetainFilter filter = new RetainFilter();
            //            filter.FilterColumn = columnName;
            //            filter.FilterValue = filterValues;
            //            if (columnName.ToUpper() == "ID")
            //            {
            //                txtFromDate.Text = "";
            //                txtToDate.Text = "";
            //            }

            //            filters.Add(filter);
            //        }
            //    }
            //    Session["Estimate_FilterExpression"] = filterExpression;
            //    Session["Estimate_Filters"] = filters;
            //}

            //#endregion


            //DataSet ds = new DataSet();
            //objProp_Customer.ConnConfig = Session["config"].ToString();
            //objProp_Customer.SearchBy = ddlSearch.SelectedValue;

            //switch (ddlSearch.SelectedValue.ToLower())
            //{
            //    case "e.status":
            //        objProp_Customer.SearchValue = ddlStatus.SelectedValue;
            //        break;
            //    case "e.ffor":
            //        objProp_Customer.SearchValue = ddlType.SelectedValue;
            //        break;
            //    case "e.template":
            //        objProp_Customer.SearchValue = ddlTemplate.SelectedValue;
            //        break;
            //    case "dep.id":
            //        objProp_Customer.SearchValue = ddlDepartment.SelectedValue;
            //        break;
            //    case "l.opportunitystageid":
            //        objProp_Customer.SearchValue = ddlOppStage.SelectedValue;
            //        break;
            //    //case "e.bdate":
            //    //    //objProp_Customer.SearchValue = txtBidCloseDate.Text;
            //    //    objProp_Customer.SearchValueFrDt = txtBidCloseDate.Text;
            //    //    objProp_Customer.SearchValueToDt = txtBidCloseDateTo.Text;
            //    //    break;
            //    case "customfield":
            //        objProp_Customer.SearchValue = ddlCustomFields.SelectedValue;
            //        objProp_Customer.SearchValueExt = txtCustomSearch.Text;
            //        break;
            //    default:
            //        objProp_Customer.SearchValue = txtSearch.Text;
            //        break;
            //}

            //objProp_Customer.StartDate = txtFromDate.Text;
            //objProp_Customer.EndDate = txtToDate.Text;
            //objProp_Customer.UserID = Convert.ToInt32(Session["UserID"].ToString());
            //#region Company Check
            //if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            //{
            //    objProp_Customer.EN = 1;
            //}
            //else
            //{
            //    objProp_Customer.EN = 0;
            //}
            //#endregion
            //objProp_Customer.Range = Convert.ToInt16(ddlDateRange.SelectedValue);

            //ds = objBL_Customer.GetEstimates(objProp_Customer, new GeneralFunctions().GetSalesAsigned(), filters);
            var dt = GetEstimates(false);

            RadGrid_Estimate.VirtualItemCount = dt.Rows.Count;
            RadGrid_Estimate.DataSource = dt;
            RadGrid_Estimate.MasterTableView.FilterExpression = string.Empty;
            UpdateSearchInfoSessions();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelProspect", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }


    private DataTable GetEstimates(bool isEmailProposalsFilter)
    {
        #region Save the Grid Filter
        List<RetainFilter> filters = new List<RetainFilter>();
        String filterExpression = Convert.ToString(RadGrid_Estimate.MasterTableView.FilterExpression);
        if (string.IsNullOrEmpty(filterExpression) && Session["Estimate_FilterExpression"] != null)
        {
            filterExpression = Session["Estimate_FilterExpression"].ToString();
        }

        if (filterExpression != "")
        {
            foreach (GridColumn column in RadGrid_Estimate.MasterTableView.OwnerGrid.Columns)
            {
                String filterValues = column.CurrentFilterValue;
                if (filterValues != "")
                {
                    String columnName = column.UniqueName;
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = columnName;
                    filter.FilterValue = filterValues;
                    if (columnName.ToUpper() == "ID")
                    {
                        txtFromDate.Text = "";
                        txtToDate.Text = "";
                    }

                    filters.Add(filter);
                }
            }
            Session["Estimate_FilterExpression"] = filterExpression;
            Session["Estimate_Filters"] = filters;
        }

        #endregion


        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.SearchBy = ddlSearch.SelectedValue;

        switch (ddlSearch.SelectedValue.ToLower())
        {
            case "e.status":
                objProp_Customer.SearchValue = ddlStatus.SelectedValue;
                break;
            case "e.ffor":
                objProp_Customer.SearchValue = ddlType.SelectedValue;
                break;
            case "e.template":
                objProp_Customer.SearchValue = ddlTemplate.SelectedValue;
                break;
            case "e.iscertifiedproject":
                objProp_Customer.SearchValue = ddlCertified.SelectedValue;
                break;
            case "dep.id":
                objProp_Customer.SearchValue = ddlDepartment.SelectedValue;
                break;
            case "l.opportunitystageid":
                objProp_Customer.SearchValue = ddlOppStage.SelectedValue;
                break;
            //case "e.bdate":
            //    //objProp_Customer.SearchValue = txtBidCloseDate.Text;
            //    objProp_Customer.SearchValueFrDt = txtBidCloseDate.Text;
            //    objProp_Customer.SearchValueToDt = txtBidCloseDateTo.Text;
            //    break;
            case "customfield":
                objProp_Customer.SearchValue = ddlCustomFields.SelectedValue;
                objProp_Customer.SearchValueExt = txtCustomSearch.Text;
                break;
            default:
                objProp_Customer.SearchValue = txtSearch.Text;
                break;
        }

        objProp_Customer.StartDate = txtFromDate.Text;
        objProp_Customer.EndDate = txtToDate.Text;
        objProp_Customer.UserID = Convert.ToInt32(Session["UserID"].ToString());
        #region Company Check
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            objProp_Customer.EN = 1;
        }
        else
        {
            objProp_Customer.EN = 0;
        }
        #endregion
        objProp_Customer.Range = Convert.ToInt16(ddlDateRange.SelectedValue);

        ds = objBL_Customer.GetEstimates(objProp_Customer, new GeneralFunctions().GetSalesAsigned(), filters, isEmailProposalsFilter);
        return ds.Tables[0];
    }

    public void ConvertToJSON()
    {
        JavaScriptSerializer jss1 = new JavaScriptSerializer();
        string _myJSONstring = jss1.Serialize(GetReportsName());
        string reports = "var reports=" + _myJSONstring + ";";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "reportsr123", reports, true);
    }
    #endregion
    #region Event
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    protected void lnkShowAll_Click(object sender, EventArgs e)
    {

        // Reset all search form
        ResetAllSearchForm();
        ddlSearch_SelectedIndexChanged(sender, e);
        // Clear grid filter
        ClearGridFilters();

        FillEstimate();
        RadGrid_Estimate.Rebind();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "CssClearLabel()", true);
    }
    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_Estimate.SelectedItems)
        {
            TableCell cell = di["chkSelect"];
            CheckBox chkSelect = (CheckBox)cell.Controls[0];
            //Label lblID = (Label)di.FindControl("lblId");
            HiddenField hdnID = di.FindControl("hdnId") as HiddenField;

            if (chkSelect.Checked == true)
            {
                Response.Redirect("addestimate.aspx?uid=" + hdnID.Value);
            }
           
        }
   
    }
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridDataItem di in RadGrid_Estimate.SelectedItems)
            {
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
               // Label lblID = (Label)di.FindControl("lblID");
                HiddenField hdnID = di.FindControl("hdnId") as HiddenField;

                string lblIDVal = hdnID.Value;
                if (chkSelect.Checked == true)
                {
                    objProp_Customer.ConnConfig = Session["config"].ToString();
                    objProp_Customer.dtLaborItems = null;
                    objProp_Customer.dtItems = null;
                    objProp_Customer.Mode = 2; 
                   // objProp_Customer.estimateno = Convert.ToInt32(lblID.Text);
                   objProp_Customer.estimateno = Convert.ToInt32(lblIDVal);
                    objProp_Customer.Username = Session["Username"].ToString();
                    objBL_Customer.DeleteEstimate(objProp_Customer);
                }
            }

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyDel", "noty({text: 'Estimate Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            FillEstimate();
            RadGrid_Estimate.Rebind();
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            if (str.IndexOf("Estimate#") == 0)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelProspect", "noty({text: '" + str + "',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelProspect", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
    }
    protected void lnkAddnew_Click(object sender, EventArgs e)
    {
        Response.Redirect("addestimate.aspx");
    }
    protected void lnkProject_Click(object sender, EventArgs e)
    {
        string prospectID = "";
        DataSet ds = new DataSet();
        foreach (GridDataItem di in RadGrid_Estimate.SelectedItems)
        {
            TableCell cell = di["chkSelect"];
            CheckBox chkSelect = (CheckBox)cell.Controls[0];
            //Label lblID = (Label)di.FindControl("lblId");
            HiddenField hdnID = di.FindControl("hdnId") as HiddenField;

            string lblIDVal = hdnID.Value;
            if (chkSelect.Checked == true)
            {
                objProp_Customer.ConnConfig = Session["config"].ToString();
                objProp_Customer.estimateno = Convert.ToInt32(lblIDVal);
                ds = objBL_Customer.GetEstimateByID(objProp_Customer);
                string ffor = ds.Tables[0].Rows[0]["ffor"].ToString();
                var templateId = ds.Tables[0].Rows[0]["Template"].ToString();

                if (!ffor.Equals("ACCOUNT"))
                {
                    objProp_Customer.ConnConfig = Session["config"].ToString();
                    objProp_Customer.estimateno = Convert.ToInt32(lblIDVal);
                    DataSet dsProspect = objBL_Customer.getProspectIDbyEstimateID(objProp_Customer);
                    if (dsProspect.Tables[0].Rows.Count > 0)
                    {
                        prospectID = dsProspect.Tables[0].Rows[0]["ID"].ToString();
                        Session["prosID"] = prospectID;
                    }
                    else
                    {
                        Session["prosID"] = null;
                    }
                }

                if (!string.IsNullOrEmpty(templateId) && templateId != "0")
                {
                    var oppID = ds.Tables[0].Rows[0]["Opportunity"].ToString();
                    if (!string.IsNullOrEmpty(oppID) && oppID != "0")
                    {
                        if (ffor.Equals("ACCOUNT"))
                        {
                            //ConvertToProject(lblIDVal);
                            string jobTypeID = ds.Tables[0].Rows[0]["JobTypeID"].ToString();
                            if (!string.IsNullOrEmpty(jobTypeID) && jobTypeID != "0")
                            {
                                ConvertToProject(lblIDVal);
                            }
                            else if (jobTypeID == "0")
                            {
                                //bool isExistProj = ds.Tables[0].Rows[0]["IsExistProj"].ToString() == "1";
                                //if (!isExistProj)
                                //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Convert Recurring Contract Wizard.'); window.location.href='addreccontract.aspx?eid=" + lblIDVal + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl) + "';", true);
                                //else
                                //    ConvertToProject(lblIDVal);
                                var projectId = ds.Tables[0].Rows[0]["job"].ToString();
                                if (string.IsNullOrEmpty(projectId) || projectId == "0")
                                {
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Convert Recurring Contract Wizard.'); window.location.href='addreccontract.aspx?eid=" + lblIDVal + "&redirect=" + HttpUtility.UrlEncode(Request.RawUrl) + "';", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyConvertValidation", "noty({text: 'Estimate already converted to project!',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                                }
                            }
                        }
                        else
                        {
                            if (Session["prosID"] != null)
                            {
                                prospectID = Session["prosID"].ToString();
                                Session["prosID"] = null;
                                string alertMess = "The lead needs to first be converted to a customer and/or location, continuing to lead conversion wizard.";
                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('" + alertMess + "'); window.location.href='addprospect.aspx?uid=" + prospectID + "&estimateid=" + lblIDVal + "';", true);
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyConvertValidation", "noty({text: 'Please update your estimate with a opportunity before converting',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyConvertValidation", "noty({text: 'Please update your estimate with a template before converting',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
        }

        FillEstimate();
        RadGrid_Estimate.Rebind();
    }

    private void ConvertToProject(string lblID)
    {
        try
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.TemplateID = Convert.ToInt32(lblID);
            objProp_Customer.estimateno = Convert.ToInt32(lblID);
            objProp_Customer.Username = Session["Username"].ToString();
            int jobid;
            String retVal = objBL_Customer.ConvertEstimateToProject(objProp_Customer);
            bool isNumeric = int.TryParse(retVal, out jobid);
            if (isNumeric == true)
            {
                #region Calculate Budget
                var estimateBOM = objBL_Customer.GetEstimateBOM(objProp_Customer);
                var estimateMile = objBL_Customer.GetEstimateMilestone(objProp_Customer);
                decimal MatPrice = 0, LabPrice = 0, OtherPrice = 0, Hours = 0, Rev = 0, Profit = 0, ProfitPer = 0, Cost = 0; ;

                foreach (DataRow dr in estimateBOM.Tables[0].Rows)
                {
                    //if (Convert.ToString(dr["BType"]) == "2")
                    //{
                    //    LabPrice = LabPrice + Convert.ToDecimal(dr["LabPrice"] == DBNull.Value ? "0" : dr["LabPrice"]);
                    //}
                    //else if (Convert.ToString(dr["BType"]) == "1")
                    //{
                    //    MatPrice = MatPrice + Convert.ToDecimal(dr["MatPrice"] == DBNull.Value ? "0" : dr["MatPrice"]);
                    //}
                    //else
                    //{
                    //    OtherPrice = OtherPrice + Convert.ToDecimal(dr["MatPrice"] == DBNull.Value ? "0" : dr["MatPrice"]);
                    //}

                    decimal LabPriceNK = 0; decimal LabHoursNK = 0;
                    if (Convert.ToString(dr["BType"]) == "1" || Convert.ToString(dr["BType"]) == "8")// Materials or Inventory
                    {
                        MatPrice = MatPrice + Convert.ToDecimal(dr["BudgetExt"] == DBNull.Value ? "0" : dr["BudgetExt"]);
                    }
                    else
                    {
                        OtherPrice = OtherPrice + Convert.ToDecimal(dr["BudgetExt"] == DBNull.Value ? "0" : dr["BudgetExt"]);
                    }

                    if (dr["LabExt"] != DBNull.Value && dr["LabExt"].ToString() != "")
                    {
                        decimal.TryParse(dr["LabExt"].ToString(), out LabPriceNK);
                        LabPrice = LabPrice + LabPriceNK;
                    }

                    //Hours = Hours + Convert.ToDecimal(dr["LabHours"] == DBNull.Value ? "0" : dr["LabHours"]);
                    if (dr["LabHours"] != DBNull.Value && dr["LabHours"].ToString() != "")
                    {
                        decimal.TryParse(dr["LabHours"].ToString(), out LabHoursNK);
                        Hours = Hours + LabHoursNK;
                    }
                }

                foreach (DataRow dr in estimateMile.Tables[0].Rows)
                {
                    Rev = Rev + Convert.ToDecimal(dr["Amount"] == DBNull.Value ? "0" : dr["Amount"]);
                }

                Cost = MatPrice + LabPrice + OtherPrice;

                Profit = Rev - Cost;

                if (Rev != 0)
                {
                    ProfitPer = (Profit / Rev) * 100;
                    ProfitPer = Convert.ToDecimal(String.Format("{0:0.00}", ProfitPer));
                }
                else
                {
                    ProfitPer = Convert.ToDecimal(String.Format("{0:0.00}", 0));
                }


                objBL_Customer.UpdateEstimateToProject(Session["config"].ToString(), Convert.ToInt32(retVal), Rev, LabPrice, MatPrice, OtherPrice, Cost, Profit, ProfitPer, Hours);
                #endregion

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Estimate Converted to Project Successfully! <BR/>Project# " + retVal + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + retVal + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }


        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    //protected void lnk_Estimate_Click(object sender, EventArgs e)
    //{
    //    foreach (GridDataItem di in RadGrid_Estimate.SelectedItems)
    //    {
    //        TableCell cell = di["chkSelect"];
    //        CheckBox chkSelect = (CheckBox)cell.Controls[0];
    //        Label lblID = (Label)di.FindControl("lblId");

    //        if (chkSelect.Checked == true)
    //        {
    //            Response.Redirect("MaddenEstimate.aspx?uid=" + lblID.Text);
    //        }
    //    }
    //}
    //protected void lnk_Service_Click(object sender, EventArgs e)
    //{
    //    foreach (GridDataItem di in RadGrid_Estimate.SelectedItems)
    //    {
    //        TableCell cell = di["chkSelect"];
    //        CheckBox chkSelect = (CheckBox)cell.Controls[0];
    //        Label lblID = (Label)di.FindControl("lblId");

    //        if (chkSelect.Checked == true)
    //        {
    //            Response.Redirect("MaddenServiceAgreement.aspx?uid=" + lblID.Text);
    //        }
    //    }
    //}
    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        String selectedValue = ddlSearch.SelectedValue;
        ShowHideFilterSearch(selectedValue);
    }
    private void ShowHideFilterSearch(String selectedValue)
    {
        txtSearch.Visible = false;
        ddlTemplate.Visible = false;
        ddlDepartment.Visible = false;
        ddlStatus.Visible = false;
        ddlType.Visible = false;
        ddlOppStage.Visible = false;
        ddlCustomFields.Visible = false;
        txtCustomSearch.Visible = false;
        ddlCertified.Visible = false;
        //txtBidCloseDate.Visible = false;
        //txtBidCloseDateTo.Visible = false;
        switch (selectedValue.ToLower())
        {
            case "e.iscertifiedproject":
                ddlCertified.Visible = true;
                break;
            case "e.status":
                ddlStatus.Visible = true;
                break;
            case "e.ffor":
                ddlType.Visible = true;
                break;
            case "e.template":
                ddlTemplate.Visible = true;
                break;
            case "dep.id":
                ddlDepartment.Visible = true;
                break;
            case "l.opportunitystageid":
                ddlOppStage.Visible = true;
                break;
            case "customfield":
                ddlCustomFields.Visible = true;
                txtCustomSearch.Visible = true;
                txtCustomSearch.Text = string.Empty;
                break;
            default:
                txtSearch.Visible = true;
                txtSearch.Text = string.Empty;
                break;
        }
    }
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        #region Search Filter
        String selectedValue = ddlSearch.SelectedValue;
        //Session["ddlSearch_estimate"] = selectedValue;

        //Session["from_estimate"] = txtFromDate.Text;
        //Session["end_estimate"] = txtToDate.Text;

        if (selectedValue == "e.ID")
        {
            txtFromDate.Text = "";
            txtToDate.Text = "";
        }


        //switch (selectedValue.ToLower())
        //{
        //    case "e.status":
        //        Session["ddlSearch_Value_estimate"] = ddlStatus.SelectedValue;
        //        break;
        //    case "e.ffor":
        //        Session["ddlSearch_Value_estimate"] = ddlType.SelectedValue;
        //        break;
        //    case "e.template":
        //        Session["ddlSearch_Value_estimate"] = ddlTemplate.SelectedValue;
        //        break;
        //    case "l.opportunitystageid":
        //        Session["ddlSearch_Value_estimate"] = ddlOppStage.SelectedValue;
        //        break;
        //    case "e.bdate":
        //        Session["ddlSearch_Value_estimate"] = txtBidCloseDate.Text;
        //        break;
        //    default:
        //        Session["ddlSearch_Value_estimate"] = txtSearch.Text;
        //        break;
        //}
        if (hdnCssActive.Value == "CssActive")
        {
            Session["lblEstimateActive"] = "1";
        }
        else
        {
            Session["lblEstimateActive"] = "2";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "CssClearLabel()", true);
        }

        #endregion

        FillEstimate();
        RadGrid_Estimate.Rebind();

    }
    private List<CustomerReport> GetReportsName()
    {
        List<CustomerReport> lstCustomerReport = new List<CustomerReport>();
        try
        {
            DataSet dsGetReports = new DataSet();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
            objProp_User.Type = "Estimate";
            dsGetReports = objBL_ReportsData.GetStockReports(objProp_User);
            //if (dsGetReports.Tables.Count > 0)
            for (int i = 0; i <= dsGetReports.Tables[0].Rows.Count - 1; i++)
            {
                CustomerReport objCustomerReport = new CustomerReport();

                objCustomerReport.ReportId = Convert.ToInt32(dsGetReports.Tables[0].Rows[i]["Id"]);
                objCustomerReport.ReportName = dsGetReports.Tables[0].Rows[i]["ReportName"].ToString();
                objCustomerReport.IsGlobal = Convert.ToBoolean(dsGetReports.Tables[0].Rows[i]["IsGlobal"]);

                lstCustomerReport.Add(objCustomerReport);
            }
        }
        catch (Exception ex)
        {
            //
        }
        return lstCustomerReport;
    }

    protected void lnkWeeklySalereport_Click(object sender, EventArgs e)
    {
        Response.Redirect("EstimateWeeklySaleReport.aspx?StartDate=" + txtFromDate.Text + "&EndDate=" + txtToDate.Text);
    }
    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Estimate.MasterTableView.FilterExpression != "" ||
            (RadGrid_Estimate.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_Estimate.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_Estimate_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["Estimate_FilterExpression"] != null && Convert.ToString(Session["Estimate_FilterExpression"]) != "" && Session["Estimate_Filters"] != null)
            {
                RadGrid_Estimate.MasterTableView.FilterExpression = Convert.ToString(Session["Estimate_FilterExpression"]);
                var filtersGet = Session["Estimate_Filters"] as List<RetainFilter>;
                if (filtersGet != null)
                {
                    foreach (var _filter in filtersGet)
                    {
                        GridColumn column = RadGrid_Estimate.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                        column.CurrentFilterValue = _filter.FilterValue;
                    }
                }
            }
        }

        FillEstimate();
    }
    //protected void RadGrid_Estimate_ItemEvent(object sender, GridItemEventArgs e)
    //{
    //    int rowCount = 0;
    //    if (e.EventInfo is GridInitializePagerItem)
    //    {
    //        rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
    //    }
    //    lblRecordCount.Text = rowCount + " Record(s) found";
    //    //updpnl.Update();
    //}
    private void RowSelect()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        foreach (GridDataItem gr in RadGrid_Estimate.Items)
        {
            //Label lblID = (Label)gr.FindControl("lblId");
            HiddenField hdnID = gr.FindControl("hdnId") as HiddenField;
            HyperLink lnkName = (HyperLink)gr.FindControl("lnkName");
            if (hdnID != null && lnkName != null)
            {
                lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='addestimate.aspx?uid=" + hdnID.Value + "'";
                DataRow dr = dt.NewRow();
                dr["ID"] = hdnID.Value;
                dt.Rows.Add(dr);
            }

            Label lblRes = (Label)gr.FindControl("lblRes");
            HyperLink lnkPdfFileName = (HyperLink)gr.FindControl("Image2");
            if (lblRes != null && lnkPdfFileName != null)
            {
                lnkPdfFileName.Attributes["onmouseover"] = "HoverMenutext('" + gr.ClientID + "','" + lblRes.ClientID + "',event);";
                lnkPdfFileName.Attributes["onmouseout"] = " $('#" + lblRes.ClientID + "').hide();";
            }

            Label lblEmailedInfo = (Label)gr.FindControl("lblEmailedInfo");
            Label lblEmailed = (Label)gr.FindControl("lblEmailed");
            if (lblEmailedInfo != null && lblEmailed != null)
            {
                lblEmailed.Attributes["onmouseover"] = "HoverMenutext('" + gr.ClientID + "','" + lblEmailedInfo.ClientID + "',event);";
                lblEmailed.Attributes["onmouseout"] = " $('#" + lblEmailedInfo.ClientID + "').hide();";
            }

        }
        Session["Estimate"] = dt;


    }

    public string ShowHoverText(object estimateId, object fileName, object addedOn, object addedBy, object manualUpload)
    {
        string result = string.Empty;
        string strFileName = fileName.ToString();
        if (!string.IsNullOrEmpty(strFileName))
        {
            string strFileEx = strFileName.Split('.').Length > 1 ? "." + strFileName.Split('.').LastOrDefault() : "";

            //string newFileName = strFileName.Replace(strFileEx, "") + "-" + estimateId.ToString() + "-" + String.Format("{0:yyyyMMddHHmmss}", Convert.ToDateTime(addedOn)) + ".pdf";
            string newFileName = "";
            if (Convert.ToString(manualUpload) != "0")
            {
                newFileName = strFileName.Replace(strFileEx, "") + "-" + estimateId.ToString() + "-" + String.Format("{0:yyyyMMddHHmmss}", Convert.ToDateTime(addedOn)) + ".pdf";
            }
            else
            {
                newFileName = strFileName;
            }
            result = "<i>File Name:</i> " + newFileName + "<br/>";

            if (!string.IsNullOrEmpty(addedOn.ToString()) && addedOn.ToString().Length > 80)
            {
                result += "<i>Created Date</i>: " + Convert.ToString(addedOn).Replace("\n", "<br/>").Substring(0, 80) + " ...";
            }
            else
            {
                result += "<i>Created Date</i>: " + Convert.ToString(addedOn).Replace("\n", "<br/>");
            }

            if (!string.IsNullOrEmpty(Convert.ToString(addedBy)) && addedBy.ToString().Length > 80)
            {
                result += "<br/><i>Created By</i>: " + Convert.ToString(addedBy).Replace("\n", "<br/>").Substring(0, 80) + " ...";
            }
            else
            {
                result += "<br/><i>Created By</i>: " + Convert.ToString(addedBy).Replace("\n", "<br/>");
            }
        }

        return result;
    }

    public string ShowHoverEmailedText(object emailedFrom, object emailedTo, object emailedOn, object emailedBy)
    {
        string result = string.Empty;
        if (!string.IsNullOrEmpty(emailedFrom.ToString()) && !string.IsNullOrEmpty(emailedTo.ToString()))
        {
            result = "<i>Emailed From:</i> " + emailedFrom.ToString().Replace("\n", "") + "<br/>";

            if (!string.IsNullOrEmpty(emailedTo.ToString()) && emailedTo.ToString().Length > 80)
            {
                result += "<i>Emailed To</i>: " + Convert.ToString(emailedTo).Replace("\n", "").Substring(0, 80) + " ... < br /> ";
            }
            else
            {
                result += "<i>Emailed To</i>: " + Convert.ToString(emailedTo).Replace("\n", "") + "<br/>";
            }

            if (!string.IsNullOrEmpty(emailedOn.ToString()) && emailedOn.ToString().Length > 80)
            {
                result += "<i>Emailed Date</i>: " + Convert.ToString(emailedOn).Replace("\n", "").Substring(0, 80) + " ...<br/>";
            }
            else
            {
                result += "<i>Emailed Date</i>: " + Convert.ToString(emailedOn).Replace("\n", "") + "<br/>";
            }

            if (!string.IsNullOrEmpty(Convert.ToString(emailedBy)) && emailedBy.ToString().Length > 80)
            {
                result += "<i>Emailed By</i>: " + Convert.ToString(emailedBy).Replace("\n", "").Substring(0, 80) + " ...";
            }
            else
            {
                result += "<i>Emailed By</i>: " + Convert.ToString(emailedBy).Replace("\n", "");
            }
        }
        return result;
    }

    protected void RadGrid_Estimate_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataSet ds = new DataSet();
            objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
            objProp_User.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
            objProp_User.PageName = "Estimate.aspx";
            objProp_User.GridId = "RadGrid_Estimate";
            ds = objBL_User.GetGridUserSettings(objProp_User);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //string columnSettings = "[{Name: \"BType\", Display: true, Width: 300},{Name: \"MatItem\", Display: false, Width: 300}]";
                var columnSettings = ds.Tables[0].Rows[0][0].ToString();
                var columnsArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ColumnSettings>>(columnSettings);

                var colIndex = 0;

                foreach (GridColumn column in RadGrid_Estimate.MasterTableView.OwnerGrid.Columns)
                {
                    colIndex++;
                    var clSetting = columnsArr.Where(t => t.Name.Equals(column.UniqueName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    if (clSetting != null)
                    {
                        column.Display = clSetting.Display;
                        if (colIndex >= 3 && clSetting.Width != 0)
                            column.HeaderStyle.Width = clSetting.Width;

                        column.OrderIndex = clSetting.OrderIndex;
                    }
                }
                RadGrid_Estimate.MasterTableView.Rebind();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "showhidebutton", "ShowRestoreGridSettingsButton();", true);
            }
        }

        RowSelect();
    }

    protected void RadGrid_Estimate_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
                var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
                if (Convert.ToString(RadGrid_Estimate.MasterTableView.FilterExpression) != "")
                    lblRecordCount.Text = totalCount + " Record(s) found";
                else
                    lblRecordCount.Text = RadGrid_Estimate.VirtualItemCount + " Record(s) found";
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
    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        RadGrid_Estimate.ExportSettings.FileName = "Estimate";
        RadGrid_Estimate.ExportSettings.IgnorePaging = true;
        RadGrid_Estimate.ExportSettings.ExportOnlyData = true;
        RadGrid_Estimate.ExportSettings.OpenInNewWindow = true;
        RadGrid_Estimate.ExportSettings.HideStructureColumns = true;
        RadGrid_Estimate.MasterTableView.UseAllDataFields = true;
        RadGrid_Estimate.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        RadGrid_Estimate.MasterTableView.ExportToExcel();
    }
    protected void RadGrid_Estimate_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 4;
        else
            currentItem = 5;
        if (e.Worksheet.Table.Rows.Count == RadGrid_Estimate.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_Estimate.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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
    #endregion

    protected void lnkCopy_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem di in RadGrid_Estimate.SelectedItems)
        {
            TableCell cell = di["chkSelect"];
            CheckBox chkSelect = (CheckBox)cell.Controls[0];
            //Label lblID = (Label)di.FindControl("lblId");
            HiddenField hdnID = di.FindControl("hdnId") as HiddenField;
            if (chkSelect.Checked == true)
            {
                Response.Redirect("addestimate.aspx?uid=" + hdnID.Value + "&t=c");
            }
        }
    }

    protected void lnkExportEstimateProfile_Click(object sender, EventArgs e)
    {
        bool IsChecked = false;
        foreach (GridDataItem di in RadGrid_Estimate.SelectedItems)
        {
            TableCell cell = di["chkSelect"];
            CheckBox chkSelect = (CheckBox)cell.Controls[0];
            // Label lblID = (Label)di.FindControl("lblId");
            HiddenField hdnID = di.FindControl("hdnId") as HiddenField;
            if (chkSelect.Checked == true)
            {
                Response.Redirect("EstimateProfile.aspx?eid=" + hdnID.Value);
                IsChecked = true;
            }
        }
        if (IsChecked == false)
        {
            Response.Redirect("EstimateProfile.aspx");
        }
    }

    protected void lnkEstimateBacklog_Click(object sender, EventArgs e)
    {
        List<RetainFilter> filters = new List<RetainFilter>();
        string filterExpression = Convert.ToString(RadGrid_Estimate.MasterTableView.FilterExpression);
        if (!string.IsNullOrEmpty(filterExpression))
        {
            foreach (GridColumn column in RadGrid_Estimate.MasterTableView.OwnerGrid.Columns)
            {
                String filterValues = column.CurrentFilterValue;
                if (filterValues != "")
                {
                    String columnName = column.UniqueName;
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = columnName;
                    filter.FilterValue = filterValues;
                    if (columnName.ToUpper() == "ID")
                    {
                        txtFromDate.Text = "";
                        txtToDate.Text = "";
                    }

                    filters.Add(filter);
                }
            }
            Session["Estimate_FilterExpression"] = filterExpression;
            Session["Estimate_Filters"] = filters;
        }

        var searchText = string.Empty;
        var searchTextExt = string.Empty;

        switch (ddlSearch.SelectedValue.ToLower())
        {
            case "e.status":
                searchText = ddlStatus.SelectedValue;
                break;
            case "e.ffor":
                searchText = ddlType.SelectedValue;
                break;
            case "e.template":
                searchText = ddlTemplate.SelectedValue;
                break;
            case "dep.id":
                searchText = ddlDepartment.SelectedValue;
                break;
            case "l.opportunitystageid":
                searchText = ddlOppStage.SelectedValue;
                break;
            case "e.iscertifiedproject":
                searchText = ddlCertified.SelectedValue;
                break;
            //case "e.bdate":
            //    searchValueFrDt = txtBidCloseDate.Text;
            //    searchValueToDt = txtBidCloseDateTo.Text;
            //    break;
            case "customfield":
                searchText = ddlCustomFields.SelectedValue;
                searchTextExt = txtCustomSearch.Text;
                break;
            default:
                searchText = txtSearch.Text;
                break;
        }

        string urlString = string.Empty;
        if (ddlDateRange.SelectedValue == "1")//
        {
            urlString = "EstimateBacklogReport.aspx?stype=" + ddlSearch.SelectedValue + "&stext=" + searchText + "&stextExt=" + searchTextExt + "&sd=" + txtFromDate.Text + "&ed=" + txtToDate.Text;
        }
        else
        {
            urlString = "EstimateBacklogReport.aspx?stype=" + ddlSearch.SelectedValue + "&stext=" + searchText + "&stextExt=" + searchTextExt + "&sfromdt=" + txtFromDate.Text + "&stodt=" + txtToDate.Text;
        }

        Response.Redirect(urlString, true);
    }

    protected void lnkEstimateRateTender_Click(object sender, EventArgs e)
    {
        List<RetainFilter> filters = new List<RetainFilter>();
        string filterExpression = Convert.ToString(RadGrid_Estimate.MasterTableView.FilterExpression);
        if (!string.IsNullOrEmpty(filterExpression))
        {
            foreach (GridColumn column in RadGrid_Estimate.MasterTableView.OwnerGrid.Columns)
            {
                String filterValues = column.CurrentFilterValue;
                if (filterValues != "")
                {
                    String columnName = column.UniqueName;
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = columnName;
                    filter.FilterValue = filterValues;
                    if (columnName.ToUpper() == "ID")
                    {
                        txtFromDate.Text = "";
                        txtToDate.Text = "";
                    }

                    filters.Add(filter);
                }
            }
            Session["Estimate_FilterExpression"] = filterExpression;
            Session["Estimate_Filters"] = filters;
        }

        var searchText = string.Empty;
        var searchTextExt = string.Empty;

        switch (ddlSearch.SelectedValue.ToLower())
        {
            case "e.status":
                searchText = ddlStatus.SelectedValue;
                break;
            case "e.ffor":
                searchText = ddlType.SelectedValue;
                break;
            case "e.template":
                searchText = ddlTemplate.SelectedValue;
                break;
            case "dep.id":
                searchText = ddlDepartment.SelectedValue;
                break;
            case "l.opportunitystageid":
                searchText = ddlOppStage.SelectedValue;
                break;
            case "e.iscertifiedproject":
                searchText = ddlCertified.SelectedValue;
                break;
            case "customfield":
                searchText = ddlCustomFields.SelectedValue;
                searchTextExt = txtCustomSearch.Text;
                break;
            default:
                searchText = txtSearch.Text;
                break;
        }

        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"].ToString());
        if (HttpContext.Current.Session["CmpChkDefault"].ToString() == "1")
        {
            objProp_Customer.EN = 1;
        }
        else
        {
            objProp_Customer.EN = 0;
        }

        objProp_Customer.SearchBy = ddlSearch.SelectedValue;
        objProp_Customer.SearchValue = searchText;
        objProp_Customer.SearchValueExt = searchTextExt;
        objProp_Customer.StartDate = txtFromDate.Text;
        objProp_Customer.EndDate = txtToDate.Text;
        objProp_Customer.Range = Convert.ToInt16(ddlDateRange.SelectedValue);
        DataSet ds = objBL_Customer.GetEstimateRate(objProp_Customer, filters, string.Empty, false);

        #region Export to Excel
        //using (ExcelPackage excelPackage = new ExcelPackage())
        //{
        //    // Set some properties of the Excel document
        //    excelPackage.Workbook.Properties.Author = "Mobile Office Manager";
        //    excelPackage.Workbook.Properties.Title = "Estimate Rate Report";
        //    excelPackage.Workbook.Properties.Subject = "TEI";
        //    excelPackage.Workbook.Properties.Created = DateTime.Now;

        //    DataTable table = ds.Tables[0];

        //    string[] selectedColumns = new[] { "Tender No.", "Tender Award Date", "Contract Sum" };
        //    var dView = table.DefaultView;
        //    dView.RowFilter = "[Tender Award Date] IS NOT NULL";
        //    DataTable table1 = dView.ToTable(false, selectedColumns);

        //    // Create the WorkSheet 1
        //    ExcelWorksheet worksheet1 = excelPackage.Workbook.Worksheets.Add("Tender");

        //    // Add column's header
        //    for (int i = 1; i <= table.Columns.Count; i++)
        //    {
        //        worksheet1.Cells[1, i].Value = table.Columns[i - 1].ColumnName;
        //        worksheet1.Cells[1, i].Style.Font.Bold = true;
        //        worksheet1.Cells[1, i].Style.Font.Color.SetColor(Color.White);
        //        worksheet1.Cells[1, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //        worksheet1.Cells[1, i].Style.Fill.BackgroundColor.SetColor(Color.SlateBlue);
        //    }

        //    // Add all the rows
        //    for (int j = 0; j < table.Rows.Count; j++)
        //    {
        //        for (int k = 0; k < table.Columns.Count; k++)
        //        {
        //            worksheet1.Cells[j + 2, k + 1].Value = table.Rows[j].ItemArray[k];

        //            if (table.Columns[k].ColumnName.Contains("Date"))
        //            {
        //                worksheet1.Cells[j + 2, k + 1].Style.Numberformat.Format = "yyyy-MM-dd";
        //            }
        //        }
        //    }

        //    // Create the WorkSheet 2
        //    // convert the excel package to a byte array
        //    byte[] bin = excelPackage.GetAsByteArray();

        //    Response.ClearHeaders();
        //    Response.Clear();
        //    Response.Buffer = true;
        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    Response.AddHeader("content-length", bin.Length.ToString());
        //    Response.AddHeader("content-disposition", "attachment; filename=\"EstimateRateReport.xlsx\"");
        //    Response.OutputStream.Write(bin, 0, bin.Length);

        //    // cleanup
        //    Response.Flush();
        //    HttpContext.Current.ApplicationInstance.CompleteRequest();
        //}
        #endregion

        DataTable table = ds.Tables[0];

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=Tender.csv");
        Response.Charset = "";
        Response.ContentType = "application/text";

        StringBuilder sb = new StringBuilder();

        // Add column's header
        for (int i = 1; i <= table.Columns.Count; i++)
        {
            sb.Append(table.Columns[i - 1].ColumnName + ',');
        }

        sb.Append("\r\n");

        // Add all the rows
        for (int j = 0; j < table.Rows.Count; j++)
        {
            for (int k = 0; k < table.Columns.Count; k++)
            {
                if (table.Columns[k].ColumnName.Contains("Date"))
                {
                    if (table.Rows[j].IsNull(k))
                    {
                        sb.Append(CsvEscape(table.Rows[j].ItemArray[k].ToString()) + ',');
                    }
                    else
                    {
                        var date = Convert.ToDateTime(table.Rows[j].ItemArray[k]);
                        sb.Append(date.ToString("yyyy-MM-dd") + ',');
                    }
                }
                else
                {
                    sb.Append(CsvEscape(table.Rows[j].ItemArray[k].ToString()) + ',');
                }
            }

            sb.Append("\r\n");
        }

        Response.Output.Write(sb.ToString());
        Response.Flush();
        Response.End();
    }

    protected void lnkEstimateRateTenderVO_Click(object sender, EventArgs e)
    {
        List<RetainFilter> filters = new List<RetainFilter>();
        string filterExpression = Convert.ToString(RadGrid_Estimate.MasterTableView.FilterExpression);
        if (!string.IsNullOrEmpty(filterExpression))
        {
            foreach (GridColumn column in RadGrid_Estimate.MasterTableView.OwnerGrid.Columns)
            {
                String filterValues = column.CurrentFilterValue;
                if (filterValues != "")
                {
                    String columnName = column.UniqueName;
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = columnName;
                    filter.FilterValue = filterValues;
                    if (columnName.ToUpper() == "ID")
                    {
                        txtFromDate.Text = "";
                        txtToDate.Text = "";
                    }

                    filters.Add(filter);
                }
            }
            Session["Estimate_FilterExpression"] = filterExpression;
            Session["Estimate_Filters"] = filters;
        }

        var searchText = string.Empty;
        var searchTextExt = string.Empty;

        switch (ddlSearch.SelectedValue.ToLower())
        {
            case "e.status":
                searchText = ddlStatus.SelectedValue;
                break;
            case "e.ffor":
                searchText = ddlType.SelectedValue;
                break;
            case "e.template":
                searchText = ddlTemplate.SelectedValue;
                break;
            case "dep.id":
                searchText = ddlDepartment.SelectedValue;
                break;
            case "l.opportunitystageid":
                searchText = ddlOppStage.SelectedValue;
                break;
            case "e.iscertifiedproject":
                searchText = ddlCertified.SelectedValue;
                break;
            case "customfield":
                searchText = ddlCustomFields.SelectedValue;
                searchTextExt = txtCustomSearch.Text;
                break;
            default:
                searchText = txtSearch.Text;
                break;
        }

        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.UserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"].ToString());
        if (HttpContext.Current.Session["CmpChkDefault"].ToString() == "1")
        {
            objProp_Customer.EN = 1;
        }
        else
        {
            objProp_Customer.EN = 0;
        }

        objProp_Customer.SearchBy = ddlSearch.SelectedValue;
        objProp_Customer.SearchValue = searchText;
        objProp_Customer.SearchValueExt = searchTextExt;
        objProp_Customer.StartDate = txtFromDate.Text;
        objProp_Customer.EndDate = txtToDate.Text;
        objProp_Customer.Range = Convert.ToInt16(ddlDateRange.SelectedValue);
        DataSet ds = objBL_Customer.GetEstimateRate(objProp_Customer, filters, string.Empty, false);
        DataTable table = ds.Tables[0];

        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=TenderVO.csv");
        Response.Charset = "";
        Response.ContentType = "application/text";

        StringBuilder sb = new StringBuilder();

        // Add column's header
        sb.Append("Tender" + ',');
        sb.Append("Sequence" + ',');
        sb.Append("AwardDate" + ',');
        sb.Append("ContractSum" + ',');
        sb.Append("\r\n");

        // Add all the rows
        for (int j = 0; j < table.Rows.Count; j++)
        {
            sb.Append(CsvEscape(table.Rows[j]["Tender"].ToString()) + ',');
            sb.Append("1" + ',');
            sb.Append(CsvEscape(table.Rows[j]["AwardDate"].ToString()) + ',');
            sb.Append(CsvEscape(table.Rows[j]["ContractSum"].ToString()) + ',');

            sb.Append("\r\n");
        }

        Response.Output.Write(sb.ToString());
        Response.Flush();
        Response.End();
    }

    public string CsvEscape(string value)
    {
        value = Regex.Replace(value, @"\r\n?|\n", " ");

        if (value.Contains(","))
        {
            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }

        return value;
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        // Clear grid filter
        ClearGridFilters();
        // reset search values
        ddlSearch.SelectedIndex = 0;
        txtSearch.Text = "";
        ddlSearch_SelectedIndexChanged(sender, e);

        //Session["ddlSearch_estimate"] = null;
        //Session["ddlSearch_Value_estimate"] = null;
        //Session["from_estimate"] = txtFromDate.Text;
        //Session["end_estimate"] = txtToDate.Text;
        // Refill estimate form
        FillEstimate();
        RadGrid_Estimate.Rebind();
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
                    case "System.Web.UI.WebControls.HiddenField":
                        ((HiddenField)c).Value = "";
                        break;
                }
            }
        }
    }

    private void ResetAllSearchForm()
    {
        ddlSearch.SelectedIndex = 0;
        txtSearch.Text = "";
        txtFromDate.Text = "";
        txtToDate.Text = "";
        ddlDateRange.SelectedIndex = 0;
        //txtBidCloseDate.Text = "";
        //txtBidCloseDateTo.Text = "";

    }

    // Clear grid filter
    private void ClearGridFilters()
    {
        if (Session["Estimate_FilterExpression"] != null && Convert.ToString(Session["Estimate_FilterExpression"]) != "" && Session["Estimate_Filters"] != null)
        {
            foreach (GridColumn column in RadGrid_Estimate.MasterTableView.OwnerGrid.Columns)
            {
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                column.CurrentFilterValue = string.Empty;
            }
            RadGrid_Estimate.MasterTableView.FilterExpression = string.Empty;
        }


        Session["Estimate_FilterExpression"] = null;
        Session["Estimate_Filters"] = null;
    }

    private void UpdateSearchInfoSessions()
    {
        String selectedValue = ddlSearch.SelectedValue;
        Session["ddlSearch_estimate"] = selectedValue;

        Session["from_estimate"] = txtFromDate.Text;
        Session["end_estimate"] = txtToDate.Text;
        Session["ddlDateRange_estimate"] = ddlDateRange.SelectedValue;
        switch (selectedValue.ToLower())
        {
            case "e.status":
                Session["ddlSearch_Value_estimate"] = ddlStatus.SelectedValue;
                break;
            case "e.ffor":
                Session["ddlSearch_Value_estimate"] = ddlType.SelectedValue;
                break;
            case "e.template":
                Session["ddlSearch_Value_estimate"] = ddlTemplate.SelectedValue;
                break;
            case "e.iscertifiedproject":
                Session["ddlSearch_Value_estimate"] = ddlCertified.SelectedValue;
                break;
            case "dep.id":
                Session["ddlSearch_Value_estimate"] = ddlDepartment.SelectedValue;
                break;
            case "l.opportunitystageid":
                Session["ddlSearch_Value_estimate"] = ddlOppStage.SelectedValue;
                break;
            //case "e.bdate":
            //    Session["ddlSearch_Value_estimate"] = null;
            //    Session["ddlSearch_Value_estimateFrDt"] = txtBidCloseDate.Text;
            //    Session["ddlSearch_Value_estimateToDt"] = txtBidCloseDateTo.Text;
            //    break;
            case "customfield":
                Session["ddlSearch_Value_estimate"] = ddlCustomFields.SelectedValue;
                Session["ddlSearch_ValueExt_estimate"] = txtCustomSearch.Text;
                break;
            default:
                Session["ddlSearch_Value_estimate"] = txtSearch.Text;
                break;
        }
    }

    private void BindCustomFields()
    {
        General objPropGeneral = new General();
        BL_General objBL_General = new BL_General();
        objPropGeneral.Screen = "Estimate";
        objPropGeneral.ConnConfig = Session["config"].ToString();
        objPropGeneral.ScreenRefID = 0;

        var ds = objBL_General.GetScreenCustomFields(objPropGeneral);

        if (ds != null && ds.Tables[0] != null)
        {
            ddlCustomFields.DataSource = ds.Tables[0];
            ddlCustomFields.DataTextField = "Label";
            ddlCustomFields.DataValueField = "ID";
            ddlCustomFields.DataBind();
        }
    }

    protected void lnkPdfFileName_Click(object sender, EventArgs e)
    {
        BusinessEntity.EstimateForm objEF = new BusinessEntity.EstimateForm();
        BL_EstimateForm objBL_EF = new BL_EstimateForm();
        objEF.ConnConfig = Session["config"].ToString();
        LinkButton btn = (LinkButton)sender;
        objEF.Estimate = Convert.ToInt32("0" + btn.CommandArgument);
        DataSet ds = objBL_EF.GetEstimateLastProposalByEstimateId(objEF);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            String fileName = ds.Tables[0].Rows[0]["FileName"].ToString();
            string strFileEx = fileName.Split('.').Length > 1 ? "." + fileName.Split('.').LastOrDefault() : "";
            String newFileName = fileName.Replace(strFileEx, "") + "-" + objEF.Estimate + "-" + String.Format("{0:yyyyMMddHHmmss}", (DateTime)ds.Tables[0].Rows[0]["AddedOn"]) + ".pdf";

            var filePath = ds.Tables[0].Rows[0]["PdfFilePath"].ToString();
            DownloadDocument(filePath, newFileName);
        }

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
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileerrorWarning", "alert('" + str + "');", true);
        }

    }

    protected void btnProcessDownload_Click(object sender, EventArgs e)
    {
        if (hdnDownloadID.Value.Trim() != "")
        {
            BusinessEntity.EstimateForm objEF = new BusinessEntity.EstimateForm();
            BL_EstimateForm objBL_EF = new BL_EstimateForm();
            objEF.ConnConfig = Session["config"].ToString();
            //LinkButton btn = (LinkButton)sender;
            objEF.Estimate = Convert.ToInt32(hdnDownloadID.Value);
            DataSet ds = objBL_EF.GetEstimateLastProposalByEstimateId(objEF);
            //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    String fileName = ds.Tables[0].Rows[0]["FileName"].ToString();
            //    string strFileEx = fileName.Split('.').Length > 1 ? "." + fileName.Split('.').LastOrDefault() : "";
            //    String newFileName = fileName.Replace(strFileEx, "") + "-" + objEF.Estimate + "-" + String.Format("{0:yyyyMMddHHmmss}", (DateTime)ds.Tables[0].Rows[0]["AddedOn"]) + ".pdf";

            //    var filePath = ds.Tables[0].Rows[0]["PdfFilePath"].ToString();
            //    DownloadDocument(filePath, newFileName);
            //}
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                String fileName = ds.Tables[0].Rows[0]["FileName"].ToString();
                string strFileEx = fileName.Split('.').Length > 1 ? "." + fileName.Split('.').LastOrDefault() : "";
                bool manualUpload = Convert.ToString(ds.Tables[0].Rows[0]["JobTID"]) == "0" ? true : false;
                if (manualUpload)
                {
                    DownloadDocument(ds.Tables[0].Rows[0]["FilePath"].ToString(), fileName);
                }
                else
                {
                    String newFileName = fileName.Replace(strFileEx, "") + "-" + objEF.Estimate + "-" + String.Format("{0:yyyyMMddHHmmss}", Convert.ToDateTime(ds.Tables[0].Rows[0]["AddedOn"].ToString())) + ".pdf";

                    DownloadDocument(ds.Tables[0].Rows[0]["PdfFilePath"].ToString(), newFileName);
                }
            }
        }
    }

    protected void RadGrid_Estimate_ItemDataBound(object sender, GridItemEventArgs e)
    {
        //e.Item.Edit = true;
        foreach (GridDataItem gvRow in RadGrid_Estimate.Items)
        {
            DropDownList ddlStatus = gvRow.FindControl("ddlApprStatus") as DropDownList;
            HiddenField hdnID = gvRow.FindControl("hdnId") as HiddenField;
            HiddenField hdnApprStatus = gvRow.FindControl("hdnApprStatus") as HiddenField;
            //Label lblStatus = (Label)gvRow.FindControl("lblApprStatus");
            if (ddlStatus != null)
            {
                ddlStatus.SelectedValue = hdnApprStatus.Value;
                ddlStatus.Attributes.Add("hdnEstId", hdnID.Value);
                ddlStatus.Attributes.Add("hdnCurrApproveStatus", hdnApprStatus.Value);

            }
        }
    }

    protected void ddlApprStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        String selectValue = ddl.SelectedValue;
        int id = Convert.ToInt32(ddl.Attributes["hdnEstId"]);

        bool allowChange = (string)ViewState["EstimateApproveProposalPermission"] == "Y";

        if (!allowChange)
        {
            // TODO: Get Approve Status Before change
            var currApproveStatus = (string)ddl.Attributes["hdnCurrApproveStatus"];
            allowChange = currApproveStatus == "2" && selectValue == "0";
        }

        if (allowChange)
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.estimateno = id;
            objProp_Customer.Status = Convert.ToInt16(selectValue);
            objProp_Customer.date = DateTime.Now;
            objProp_Customer.Username = Session["Username"].ToString();
            objProp_Customer.Notes = ddl.SelectedItem.Text + " by " + objProp_Customer.Username + " " + objProp_Customer.date.ToString("MM/dd/yyyy HH:mm tt");

            try
            {
                objBL_Customer.UpdateEstimateApprovalStatus(objProp_Customer);

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccApproveProposal", "noty({text: 'The approval status of estimate " + id.ToString() + " was updated successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                RadGrid_Estimate.Rebind();
            }
            catch (Exception ex)
            {
                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErrContct", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

            }
        }
    }

    protected void lnkClosePopup_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        dt = GetUserById();
        var isApproveProposal = dt.Rows[0]["EstApproveProposal"] == null ? false : Convert.ToBoolean(dt.Rows[0]["EstApproveProposal"]);

        int id = Convert.ToInt32(hdnEstId.Value);
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.estimateno = id;
        objProp_Customer.Status = Convert.ToInt16(hdnApprStatus.Value);
        objProp_Customer.date = DateTime.Now;
        objProp_Customer.Username = Session["Username"].ToString();

        if (!isApproveProposal)
        {
            var ds = objBL_Customer.GetEstimateApprovedStatusHistory(objProp_Customer);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string currApprStatus = ds.Tables[0].Rows[0]["NewStatus"].ToString();
                isApproveProposal = currApprStatus == "2" && hdnApprStatus.Value == "0";
            }
        }

        if (isApproveProposal || Session["type"].ToString() == "am")
        {
            var comment = new StringBuilder();
            if (hdnApprStatus.Value == "1")
            {
                comment.AppendFormat("Approved by {0} {1}", objProp_Customer.Username, objProp_Customer.date.ToString("MM/dd/yyyy HH:mm tt"));
            }
            else if (hdnApprStatus.Value == "2")
            {
                comment.AppendFormat("Required Changes by {0} {1}", objProp_Customer.Username, objProp_Customer.date.ToString("MM/dd/yyyy HH:mm tt"));
                //comment = "Required Changes by " + objProp_Customer.Username + " " + objProp_Customer.date.ToString("MM/dd/yyyy HH:mm tt");
            }
            else
            {
                comment.AppendFormat("Pending by {0} {1}", objProp_Customer.Username, objProp_Customer.date.ToString("MM/dd/yyyy HH:mm tt"));
                //comment = "Pending by " + objProp_Customer.Username + " " + objProp_Customer.date.ToString("MM/dd/yyyy HH:mm tt");
            }
            comment.AppendLine();
            comment.Append(txtApproveStatusComment.Text);

            objProp_Customer.Notes = comment.ToString();//ddl.SelectedItem.Text + " by " + objProp_Customer.Username + " " + objProp_Customer.date.ToString("MM/dd/yyyy HH:mm tt");

            try
            {
                objBL_Customer.UpdateEstimateApprovalStatus(objProp_Customer);

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccApproveProposal", "noty({text: 'The approval status of estimate " + id.ToString() + " was updated successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                RadGrid_Estimate.Rebind();
            }
            catch (Exception ex)
            {
                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrContct", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrContct", "noty({text: 'There is no permission to do this',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void RadGrid_gvLogs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();
        DataSet dsLog = new DataSet();
        EmailLog emailLog = new EmailLog();
        emailLog.Screen = "Estimate";
        emailLog.ConnConfig = Session["config"].ToString();
        BL_EmailLog bL_EmailLog = new BL_EmailLog();
        dsLog = bL_EmailLog.GetEmailLogs(emailLog);
        if (dsLog.Tables[0].Rows.Count > 0)
        {
            var userinfo = (DataTable)Session["userinfo"];
            int usertypeid = 0;
            if (userinfo != null)
            {
                usertypeid = Convert.ToInt32(userinfo.Rows[0]["usertypeid"]);
            }

            if (usertypeid == 2)
            {
                RadGrid_gvLogs.DataSource = string.Empty;
            }
            else
            {
                RadGrid_gvLogs.VirtualItemCount = dsLog.Tables[0].Rows.Count;
                RadGrid_gvLogs.DataSource = dsLog.Tables[0];
            }
        }
        else
        {
            RadGrid_gvLogs.DataSource = string.Empty;
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

    protected void lnkMailAll_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = GetEstimates(true);
            if (dt != null && dt.Rows.Count > 0)
            {
                int totalTicket = dt != null ? dt.Rows.Count : 0;

                int totalSentEmails = 0;
                int totalSendErr = 0;
                try
                {
                    Mail mail = new Mail();
                    mail.From = WebBaseUtility.GetFromEmailAddress();

                    //DataSet dsC = new DataSet();
                    User objPropUser = new User();
                    objPropUser.ConnConfig = Session["config"].ToString();
                    //if (Session["MSM"].ToString() != "TS")
                    //{
                    //    dsC = objBL_User.getControl(objPropUser);
                    //}
                    //else
                    //{
                    //    dsC = objBL_User.getControlBranch(objPropUser);
                    //}

                    //string address = dsC.Tables[0].Rows[0]["name"].ToString() + "<br/>";
                    //address += dsC.Tables[0].Rows[0]["Address"].ToString() + "<br/>";
                    //address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + "<br/>";
                    //address += "Tel: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + "<br/>";
                    //address += dsC.Tables[0].Rows[0]["email"].ToString() + "<br/>";

                    string address = WebBaseUtility.GetSignature();

                    List <MimeKit.MimeMessage> mimeSentMessages = new List<MimeKit.MimeMessage>();
                    List<MimeKit.MimeMessage> mimeErrorMessages = new List<MimeKit.MimeMessage>();
                    Tuple<int, string, string> emailSendError = null;
                    Tuple<int, string, string> emailGetSentError = null;
                    StringBuilder sbdSentError = new StringBuilder();
                    StringBuilder sbdGetSentError = new StringBuilder();

                    EmailLog emailLog = new EmailLog();
                    emailLog.ConnConfig = Session["config"].ToString();
                    emailLog.Function = "Email All Proposals";
                    emailLog.Screen = "Estimate";
                    emailLog.Username = Session["Username"].ToString();
                    emailLog.SessionNo = Guid.NewGuid().ToString();

                    BL_General bL_General = new BL_General();
                    EmailTemplate emailTemplate = new EmailTemplate();
                    emailTemplate.ConnConfig = Session["config"].ToString();
                    emailTemplate.Screen = "Estimate";
                    emailTemplate.FunctionName = "Email All Proposals";
                    string mailContent = bL_General.GetEmailTemplate(emailTemplate);

                    foreach (DataRow _dr in dt.Rows)
                    {
                        emailLog.Ref = (int)_dr["id"];

                        if (!string.IsNullOrEmpty(_dr["EstimateEmail"].ToString()))
                        {
                            mail.To = _dr["EstimateEmail"].ToString().Split(';', ',').OfType<string>().ToList();
                            // TODO: Thomas: need to update these info later
                            mail.Title = "Proposal of Estimate# " + _dr["id"];
                            mail.IsBodyHtml = true;
                            StringBuilder stringBuilder = new StringBuilder();

                            if (string.IsNullOrEmpty(mailContent))
                            {
                                stringBuilder.Append("Dear <br/><br/>");
                                stringBuilder.AppendFormat("We are sending you the proposal of estimate# {0}<br/>", _dr["id"]);
                                stringBuilder.Append("Please see attachment for more details <br/><br/>");
                                stringBuilder.Append("Thanks<br/><br/>");
                            }
                            else
                            {
                                stringBuilder.Append(mailContent.Replace("{EstimateNo}", _dr["id"].ToString())
                                    .Replace("{CompanyName}", _dr["CompanyName"].ToString()));
                            }

                            stringBuilder.Append(address);

                            mail.Text = stringBuilder.ToString();
                            mail.Attachments = new List<Attachment>();
                            System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(_dr["PdfFilePath"].ToString());

                            String fileName = Convert.ToString(_dr["FileName"]);
                            String addedOn = Convert.ToString(_dr["AddedOn"]);

                            string strFileEx = fileName.Split('.').Length > 1 ? "." + fileName.Split('.').LastOrDefault() : "";
                            string newFileName = fileName.Replace(strFileEx, "") + "-" + _dr["id"].ToString() + "-" + String.Format("{0:yyyyMMddHHmmss}", Convert.ToDateTime(_dr["AddedOn"])) + ".pdf";
                            attachment.Name = newFileName;
                            mail.Attachments.Add(attachment);
                            mail.RequireAutentication = true;

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
                                    // Need to update EstimateForm Proposal
                                    #region Update Values In EstimateForms Table
                                    BL_EstimateForm objBL_EF = new BL_EstimateForm();
                                    EstimateForm objEF = new EstimateForm();
                                    objEF.ConnConfig = Session["config"].ToString();
                                    objEF.Id = (int)_dr["ProposalId"];
                                    objBL_EF.UpdateEstimateForm(objEF, _dr["EstimateEmail"].ToString(), mail.From, Session["Username"].ToString());
                                    #endregion
                                }
                            }
                        }
                        else
                        {
                            totalSendErr++;
                            emailLog.To = string.Empty;
                            emailLog.Status = 0;
                            emailLog.UsrErrMessage = "Email address does not exist for this estimate";
                            BL_EmailLog bL_EmailLog = new BL_EmailLog();
                            bL_EmailLog.AddEmailLog(emailLog);
                        }
                    }

                    totalSentEmails = mimeSentMessages.Count;
                    if (totalSentEmails > 0)
                    {
                        //Mail mail = new Mail();
                        WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                        if (mail.TakeASentEmailCopy)
                        {
                            // reset ref 
                            emailLog.Ref = 0;
                            var take = 100;
                            try
                            {
                                var strTakeSent = ConfigurationManager.AppSettings["TakeSentItemspertime"].ToString();
                                take = Convert.ToInt32(strTakeSent);
                            }
                            catch (Exception)
                            {
                            }

                            if (totalSentEmails >= take)
                            {
                                List<List<MimeKit.MimeMessage>> lstTenMessages = new List<List<MimeKit.MimeMessage>>();
                                while (mimeSentMessages.Any())
                                {
                                    lstTenMessages.Add(mimeSentMessages.Take(take).ToList());
                                    mimeSentMessages = mimeSentMessages.Skip(take).ToList();
                                }

                                int imapLoginTimes = 0;
                                foreach (var lst in lstTenMessages)
                                {
                                    imapLoginTimes++;
                                    if (imapLoginTimes >= 15)
                                    {
                                        imapLoginTimes = 0;
                                        Thread.Sleep(10000);
                                        mail = new Mail();
                                        WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                                    }
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
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,dismissQueue: true,theme : 'noty_theme_default',  closable : true});", true);
                        if (emailGetSentError != null)
                        {
                            string str1 = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr1", "noty({text: '" + str1 + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,dismissQueue: true,theme : 'noty_theme_default',  closable : true});", true);
                        }
                    }
                    else
                    {
                        if (totalSentEmails > 0)
                        {
                            var successfullMess = "There were " + totalSentEmails + " of "
                                + totalTicket + " proposals sent out successfully.";

                            if (totalSendErr > 0)
                            {
                                successfullMess += "<br>Total " + totalSendErr + " failed of "
                                    + totalTicket + " proposals could not be sent.";
                            }

                            //if (totalNotSend > 0)
                            //{
                            //    successfullMess += "<br>Total " + totalNotSend + " of "
                            //        + totalTicket + " tickets have not been completed.";
                            //}

                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: '" + successfullMess + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,dismissQueue: true,theme : 'noty_theme_default',  closable : true});", true);

                            if (emailGetSentError != null)
                            {
                                string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(emailGetSentError.Item2);
                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,dismissQueue: true,theme : 'noty_theme_default',  closable : true});", true);
                            }
                        }
                        else
                        {
                            string str = "There were no emails sent out.";

                            if (totalSendErr > 0)
                            {
                                str += "<br>Total " + totalSendErr + " failed of "
                                    + totalTicket + " proposals could not be sent.";
                            }
                            //if (totalNotSend > 0)
                            //{
                            //    str += "<br>Total " + totalNotSend + " of "
                            //        + totalTicket + " tickets have not been completed.";
                            //}

                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,dismissQueue: true,theme : 'noty_theme_default',  closable : true});", true);
                        }
                    }

                    RadGrid_gvLogs.Rebind();
                    RadGrid_Estimate.Rebind();
                    //            //}
                }
                catch (Exception exp)
                {
                    string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(exp.Message);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
                //    }
                //    else
                //    {
                //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarning", "noty({text: 'There are no completed tickets on your list.',  type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });", true);
                //    }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarning", "noty({text: 'There was no proposal sent out',  type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });", true);
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkSaveGridSettings_Click(object sender, EventArgs e)
    {
        #region Grid user settings
        var columnSettings = GetGridColumnSettings();
        objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objProp_User.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        objProp_User.PageName = "Estimate.aspx";
        objProp_User.GridId = "RadGrid_Estimate";

        objBL_User.UpdateUserGridCustomSettings(objProp_User, columnSettings);
        #endregion
    }

    protected void lnkRestoreGridSettings_Click(object sender, EventArgs e)
    {
        #region Grid user settings
        var columnSettings = string.Empty;
        objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objProp_User.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        objProp_User.PageName = "Estimate.aspx";
        objProp_User.GridId = "RadGrid_Estimate";

        var ds = objBL_User.DeleteUserGridCustomSettings(objProp_User);


        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            columnSettings = ds.Tables[0].Rows[0][0].ToString();
            var columnsArr = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ColumnSettings>>(columnSettings);

            var colIndex = 0;

            foreach (GridColumn column in RadGrid_Estimate.MasterTableView.OwnerGrid.Columns)
            {
                colIndex++;
                var clSetting = columnsArr.Where(t => t.Name.Equals(column.UniqueName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (clSetting != null)
                {
                    column.Display = clSetting.Display;
                    if (colIndex >= 3 && clSetting.Width != 0)
                        column.HeaderStyle.Width = clSetting.Width;

                    column.OrderIndex = clSetting.OrderIndex;
                }
            }
            RadGrid_Estimate.MasterTableView.Rebind();
        }
        else
        {
            //var arrColumnOrder = new string[3]{ "ReviewCheck", "Comp", "" };
            var colIndex = 0;
            foreach (GridColumn column in RadGrid_Estimate.MasterTableView.OwnerGrid.Columns)
            {
                colIndex++;
                column.Display = true;
                //column.OrderIndex =
                //if(colIndex >= 3)
                //{
                //    column.HeaderStyle.Reset();
                //}
            }
            RadGrid_Estimate.MasterTableView.SortExpressions.Clear();
            RadGrid_Estimate.MasterTableView.GroupByExpressions.Clear();
            RadGrid_Estimate.EditIndexes.Clear();
            RadGrid_Estimate.Rebind();
        }
        #endregion
    }

    private string GetGridColumnSettings()
    {
        var columnSettings = string.Empty;

        List<ColumnSettings> lstColSetts = new List<ColumnSettings>();
        foreach (GridColumn column in RadGrid_Estimate.MasterTableView.OwnerGrid.Columns)
        {
            var colSett = new ColumnSettings();
            colSett.Name = column.UniqueName;
            colSett.Display = column.Display;
            colSett.Width = (int)column.HeaderStyle.Width.Value;
            colSett.OrderIndex = column.OrderIndex;
            lstColSetts.Add(colSett);
        }

        columnSettings = Newtonsoft.Json.JsonConvert.SerializeObject(lstColSetts);
        return columnSettings;
    }

    private string GetDefaultGridColumnSettingsFromDb()
    {
        var columnSettings = string.Empty;
        objProp_User.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objProp_User.PageName = "Estimate.aspx";
        objProp_User.GridId = "RadGrid_Estimate";

        var ds = objBL_User.GetDefaultGridCustomSettings(objProp_User);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            columnSettings = ds.Tables[0].Rows[0][0].ToString();
        }

        return columnSettings;
    }
}

