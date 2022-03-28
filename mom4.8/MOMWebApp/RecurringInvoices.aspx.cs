using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Globalization;
using Telerik.Web.UI;
using System.Linq.Dynamic;
using System.Configuration;
using Telerik.Web.UI.GridExcelBuilder;

public partial class RecurringInvoices : System.Web.UI.Page
{
    #region Variable

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    Inv _objInv = new Inv();
    Transaction _objTrans = new Transaction();

    Journal _objJournal = new Journal();
    BL_JournalEntry _objBL_Journal = new BL_JournalEntry();

    Invoices _objInvoices = new Invoices();
    BL_Invoice objBL_Invoice = new BL_Invoice();

    Chart _objChart = new Chart();
    BL_Chart _objBL_Chart = new BL_Chart();

    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
   
    private int totalItemCount;
    public class GetTotalItemModel
    {
        public string TotalPretaxAmt { get; set; }
        public string TotalInvoice { get; set; }
    }

    GetTotalItemModel getTotalItem;

    bool success;
    #endregion

    #region "events"

    #region PAGELOAD
    protected void Page_Load(object sender, EventArgs e)
    {        
        try
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

                ViewState["RecInvoices"] = null;
                //divSuccess.Visible = false;
                DateTime baseDate = DateTime.Today;

                var today = baseDate;
                var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
                var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
               
                //txtStartDt.Text = thisMonthStart.ToShortDateString();
                //txtEndDate.Text = thisMonthEnd.ToShortDateString();
                FillMonth();
                FillYear();
                ddlYears.SelectedValue = baseDate.Year.ToString();
                ddlMonths.SelectedValue = (baseDate.Month - 1).ToString();

                txtInvoiceDt.Text = baseDate.ToShortDateString();
                txtPostDt.Text = baseDate.ToShortDateString();
                txtRemark.Text = "@s - @f billing @d for the period of @p";
                getUIHistory();
                FillTerms();
                //GetRecurringInvoices();
                //BindRecurringInvoice();
                rgRecurringInvoice.Rebind();
                GetPeriodDetails(Convert.ToDateTime(txtInvoiceDt.Text));
            }
            Permission();
            HighlightSideMenu("cntractsMgr", "lnkInvoicesMenu", "recurMgrSub");
            CompanyPermission();
            string Report1 = string.Empty;
            string Report2 = string.Empty;
            string Report3 = string.Empty;
            Report1 = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesMaintReport"].Trim();
            Report2 = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoicesExceptionReport"].Trim();
            Report3 = System.Web.Configuration.WebConfigurationManager.AppSettings["InvoiceLNY"].Trim();
            if (Report1 == string.Empty || Report2 == string.Empty)
            {
                lnk_InvoiceMaint.Visible = false;
                lnk_InvoiceException.Visible = false;
            }
            if (Report3 == string.Empty)
            {
                lnk_InvoiceLNY.Visible = false;
            }
            //todo
            SetupCanadaCompanyUI();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
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

    protected void Page_PreRender(Object o, EventArgs e)
    {
        //foreach (DataGridItem gr in rgRecurringInvoice.Items)
        //{
        //    Label lblID = (Label)gr.FindControl("lblId");
        //    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

        //    //gr.Attributes["ondblclick"] = "showModalPopupViaClientCust(" + lblID.Text + "," + lblComp.Text + ");";
        //    gr.Attributes["onclick"] = "SelectRowChk('" + gr.ClientID + "','" + chkSelect.ClientID + "','" + rgRecurringInvoice.ClientID + "',event);";
        //}
        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key1", "SelectedRowStyle('" + gvOpenCalls.ClientID + "');", true);
    }

    #endregion

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    private void ProcessRecurringInvoice()
    {
        try
        {
            List<string> GridContractlist = new List<string>();

            foreach (GridDataItem gr in rgRecurringInvoice.MasterTableView.Items)
            {
                string lblJobID = ((Label)gr.FindControl("lblJobID")).Text.Trim();

                GridContractlist.Add(lblJobID);
            }

            Session["InvoiceName"] = "Invoice";

            if (Convert.ToInt32(ViewState["mode"]) != 1)
            {
                GetPeriodDetails(Convert.ToDateTime(txtInvoiceDt.Text));
            }
            if ((bool)ViewState["FlagPeriodClose"])
            {
                #region Process Invoice

                objProp_Contracts.ConnConfig = Session["config"].ToString();

                if (txtInvoiceDt.Text != string.Empty)
                {
                    objProp_Contracts.Date = Convert.ToDateTime(txtInvoiceDt.Text);
                }

                else
                {
                    objProp_Contracts.Date = System.DateTime.Now;
                }

                objProp_Contracts.PostDate = Convert.ToDateTime(txtPostDt.Text);

                objProp_Contracts.DueDate = DueByTerms();

                objProp_Contracts.Remarks = txtRemark.Text;

                if (!string.IsNullOrEmpty(ddlTerms.SelectedValue))
                {
                    objProp_Contracts.PaymentTerms = Convert.ToInt32(ddlTerms.SelectedValue);
                }

                //todo
                if (Convert.ToBoolean(ViewState["isCanada"]) == true)
                {
                    objProp_Contracts.TaxApply = ddlTaxType.SelectedValue;
                }
                else
                {
                    objProp_Contracts.Taxable = Convert.ToInt32(chkTaxable.Checked);
                }
                //objProp_Contracts.ProcessPeriod = txtStartDt.Text + " - " + txtEndDate.Text;
                objProp_Contracts.ProcessPeriod = ddlMonths.SelectedItem.Text + " " + ddlYears.SelectedValue;



                DataTable dtr = (DataTable)ViewState["RecInvoiceSrch"];

                foreach (DataRow drow in dtr.Rows)
                {
                    string JobId = drow["job"].ToString();

                    if (!GridContractlist.Contains(JobId))
                    {
                        drow.Delete();
                    }

                }

                dtr.AcceptChanges();

                dtr.AsEnumerable().ToList()
                    .ForEach(r => r["fDate"] = DateTime.Parse(txtPostDt.Text));


                //detailLevel=2 detailed with price code
                DataTable dtNew = new DataTable();

                dtNew = dtr.Clone();

                DataColumn dtcol = dtNew.Columns.Add("detailItem", typeof(string));

                foreach (DataRow drow in dtr.Rows)
                {
                    DataSet dsTemp = new DataSet();

                    DataTable dtTemp = new DataTable();

                    objProp_Contracts.JobId = Convert.ToInt32(drow["OrgLoc"].ToString());

                    dsTemp = objBL_Contracts.GetEquipmentByInvoice(objProp_Contracts);

                    if (drow["detailLevel"].ToString() == "2")
                    {

                        foreach (DataRow dr in dsTemp.Tables[0].Rows)
                        {

                            if (drow["job"].ToString() == dr["id"].ToString())
                            {
                                Decimal Srate = Convert.ToDecimal(drow["taxrate"]);
                                Decimal price = Convert.ToDecimal(dr["Price"]);
                                Decimal gstrate = Convert.ToDecimal(drow["GSTRate"]);
                                var newDataRow = dtNew.NewRow();

                                newDataRow.ItemArray = drow.ItemArray;

                                newDataRow["detailItem"] = dr["Unit"];

                                newDataRow["Price"] = dr["Price"];

                                newDataRow["Amount"] = dr["Price"];

                                newDataRow["taxable"] = dr["Price"];
                                newDataRow["stax"] = ((price * Srate) / 100);
                                newDataRow["GST"] = ((price * gstrate) / 100);
                                newDataRow["total"] = price+((price * Srate) / 100)+ ((price * gstrate) / 100);
                                //newDataRow["Amount"] = price + ((price * Srate) / 100) + ((price * gstrate) / 100);
                                //todo
                                if (!Convert.ToBoolean(ViewState["isCanada"]))
                                {
                                    if (chkTaxable.Checked)
                                    {
                                        newDataRow["taxable"] = dr["Price"];
                                        newDataRow["stax"] = ((price * Srate) / 100);
                                    }
                                }
                                dtNew.Rows.Add(newDataRow);
                            }
                        }

                    }
                    else
                    {
                        var newdr = dtNew.NewRow();

                        newdr.ItemArray = drow.ItemArray;

                        dtNew.Rows.Add(newdr);
                    }
                }


                objProp_Contracts.Fuser = Session["User"].ToString();


                dtNew.AcceptChanges();


                DataTable dtrinv = CreatetblTypeRecurringInvoice(dtNew);


                objProp_Contracts.DtRecContr = dtrinv;

                DataSet _ds = new DataSet();


                if (dtrinv.Rows.Count > 0)
                {

                    //todo
                    if (Convert.ToBoolean(ViewState["isCanada"]) == true)
                    {

                        int IncludeContractRemarks = 0;

                        if (chkIncludeContractRemarks.Checked) IncludeContractRemarks = 1;
                        _ds = objBL_Contracts.AddRecurringInvoices(objProp_Contracts, IncludeContractRemarks);
                        //Save UI history
                        objProp_Contracts.TaxApply = ddlTaxType.SelectedValue;
                        objProp_Contracts.isCanadaCompany = Convert.ToBoolean(ViewState["isCanada"]);
                        objProp_Contracts.lastUpdatedby = Session["Username"].ToString();
                        objBL_Contracts.AddRecurringInvoicesUIHistory(objProp_Contracts);

                    }
                    else
                    {

                        int IncludeContractRemarks = 0;

                        if (chkIncludeContractRemarks.Checked) IncludeContractRemarks = 1;
                        _ds = objBL_Contracts.CreateRecurringInvoices(objProp_Contracts , IncludeContractRemarks);
                        //Save UI history
                        objProp_Contracts.Taxable = Convert.ToInt32(chkTaxable.Checked);
                        objProp_Contracts.isCanadaCompany = false;
                        objProp_Contracts.lastUpdatedby = Session["Username"].ToString();
                        objBL_Contracts.AddRecurringInvoicesUIHistory(objProp_Contracts);
                    }


                    BindRecurringInvoice(0, true,true);

                    //rgRecurringInvoice.Rebind();

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "keySuccUp", "noty({text: 'Invoices processed successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});setTimeout(AvoidResubmit, 2000);", true);
                    #endregion

                    if (_ds != null)
                    {

                        if (_ds.Tables[0].Rows.Count > 0)
                        {
                            txtStartInv.Text = _ds.Tables[0].Rows[0]["Ref"].ToString();

                            int _index = _ds.Tables[_ds.Tables.Count - 1].Rows.Count - 1;

                            txtEndInv.Text = _ds.Tables[_ds.Tables.Count - 1].Rows[0]["Ref"].ToString();
                        }
                    }

                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }

    }


    private DataTable CreatetblTypeRecurringInvoice(DataTable dt)
    {
        int InvoiceID = 0;

        foreach (DataRow dr in dt.Rows)
        {
            InvoiceID = 0;

            int.TryParse(dr["InvoiceID"].ToString(), out InvoiceID);

            if (dr["credit"].ToString() == "1" && hdnCreditHold.Value == "1")
            {
                dr.Delete();
            }
            else if (InvoiceID > 0)
            {
                dr.Delete();
            }
        }
         

        if (dt.Columns.Contains("EN"))
        {
            dt.Columns.Remove("EN");
        }

        if (dt.Columns.Contains("Company"))
        {
            dt.Columns.Remove("Company");
        }

        if (dt.Columns.Contains("ref"))
        {
            dt.Columns.Remove("ref");
        }

        if (dt.Columns.Contains("credit"))
        {
            dt.Columns.Remove("credit");
        }

        if (dt.Columns.Contains("InvoiceID"))
        {
            dt.Columns.Remove("InvoiceID");
        }

     


        if (dt.Columns.Contains("SRemarks"))
        {
            dt.Columns.Remove("SRemarks");
        }

        if (dt.Columns.Contains("ExpirationDate"))
        {
            dt.Columns.Remove("ExpirationDate");
        }

        if (dt.Columns.Contains("EscType"))
        {
            dt.Columns.Remove("EscType");
        }


        dt.AcceptChanges();



        return dt;

    }
    private DataTable GetFilteredDataSource()
    {
        DataTable DT = new DataTable();
        DataTable FilteredDT = new DataTable();
        string filterexpression = string.Empty;
        filterexpression = rgRecurringInvoice.MasterTableView.FilterExpression;

        DT = (DataTable)ViewState["RecInvoiceSrch"];

        if (!string.IsNullOrEmpty(filterexpression))
        {
            FilteredDT = DT.AsEnumerable()
                    .AsQueryable()
                    .Where(filterexpression)
                    .CopyToDataTable();
        }
        else
        {
            FilteredDT = DT;
        }

        return FilteredDT;
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        try
        {
            List<DataRow> rowsToDelete = new List<DataRow>();
            //DataTable dt = (DataTable)ViewState["RecInvoiceSrch"];
            DataTable dt = GetFilteredDataSource();

            foreach (GridDataItem item in rgRecurringInvoice.Items)
            {
                Label lblCustID = (Label)item.FindControl("lblId");
                CheckBox chkbox = (CheckBox)item.FindControl("chkSelect");

                if (chkbox.Checked)
                {
                    DeleteInvoice(Convert.ToInt32(lblCustID.Text), dt, rowsToDelete);
                }
            }
            foreach (DataRow row in rowsToDelete)
            {
                dt.Rows.Remove(row);
            }

            ViewState["RecInvoiceSrch"] = dt;

            //BindGridDatatable((DataTable)ViewState["RecInvoiceSrch"]);
            BindGridDatatable(dt);
            rgRecurringInvoice.DataBind();

            //hdnCreditHold.Value = "0";

            //foreach (DataRow dr in dt.Rows)
            //{
            //    if (dr["credit"].ToString() == "1")
            //    {
            //        hdnCreditHold.Value = "1";
            //    }

            //}
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void DeleteInvoice(int InvoiceID, DataTable dt, List<DataRow> rowsToDelete)
    {
    
        rowsToDelete.Add(dt.Rows[InvoiceID]);
        
    }

    protected void gvOpenCalls_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    #region Sorting

    protected void gvOpenCalls_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING);
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING);
        }
    }
    #endregion

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        rgRecurringInvoice.Rebind();
        if (!string.IsNullOrEmpty(txtInvoiceDt.Text))
        {
            GetPeriodDetails(Convert.ToDateTime(txtInvoiceDt.Text));
        }

        //By Yashasvi Jadav.
        rgRecurringInvoice.Visible = true;
        txtPostDt.Text = txtInvoiceDt.Text;
    }


    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        objProp_Contracts.InvoiceID = Convert.ToInt32(txtStartInv.Text);
        objProp_Contracts.InvoiceEndID = Convert.ToInt32(txtEndInv.Text);
        string monthName = new DateTime(Convert.ToInt32(ddlYears.SelectedItem.Text), (Convert.ToInt32(ddlMonths.SelectedValue) + 1), 1).ToString("MMM", CultureInfo.InvariantCulture); ;
        string year = ddlYears.SelectedItem.Text;
        var reportFormat = ConfigurationManager.AppSettings["InvoiceReportFormat"].ToString();
        if(reportFormat.Equals("RDLC"))
            Response.Redirect("PrintInvoices.aspx?uid=" + objProp_Contracts.InvoiceID + "&eid=" + objProp_Contracts.InvoiceEndID + "&m=" + monthName + "&y=" + year + "&check=1&page=recurringinvoices", true);
        else
            Response.Redirect("PrintInvoicesMrt.aspx?uid=" + objProp_Contracts.InvoiceID + "&eid=" + objProp_Contracts.InvoiceEndID + "&m=" + monthName + "&y=" + year + "&check=1&page=recurringinvoices", true);
    }
    #endregion

    #region Custom functions
    private void FillMonth()
    {
        try
        {
            var _varMonth = Enum.GetValues(typeof(CommonHelper.Months));
            var values = Enum.GetValues(typeof(CommonHelper.Months)).Cast<CommonHelper.Months>();

            //ddlMonths.Items.Add(new ListItem(":: Select ::"));
            int i = 0;
            foreach (var v in values)
            {
                ddlMonths.Items.Add(new ListItem(v.Description(), i.ToString()));
                i++;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void FillYear()
    {
        try
        {
            //ddlYears.Items.Add(new ListItem(":: Select ::"));
            DateTime baseDate = DateTime.Today;
            int _currentYear = baseDate.Year;

            ddlYears.Items.Add(new ListItem((_currentYear - 1).ToString(), (_currentYear - 1).ToString()));
            ddlYears.Items.Add(new ListItem(_currentYear.ToString(), _currentYear.ToString()));
            ddlYears.Items.Add(new ListItem((_currentYear + 1).ToString(), (_currentYear + 1).ToString()));

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void Permission()
    {
      
        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
        }
        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx");
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }
        // User Permission 
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = GetUserById();

            /// RCmodulePermission ///////////////////------->

            string RCmodulePermission = ds.Rows[0]["RCmodulePermission"] == DBNull.Value ? "Y" : ds.Rows[0]["RCmodulePermission"].ToString();

            if (RCmodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }

            /// RecurringInvoices ///////////////////------->

            string ProcessC = ds.Rows[0]["ProcessC"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["ProcessC"].ToString();
            string ADD = ProcessC.Length < 1 ? "Y" : ProcessC.Substring(0, 1);
            string Edit = ProcessC.Length < 2 ? "Y" : ProcessC.Substring(1, 1);
            string Delete = ProcessC.Length < 2 ? "Y" : ProcessC.Substring(2, 1);
            string View = ProcessC.Length < 4 ? "Y" : ProcessC.Substring(3, 1);
            if (ADD == "N")
            {
                LinkButton2.Visible = false;
                //lnkAddnew.Visible = false;
            }
            if (Edit == "N")
            {
               
            }
            if (Delete == "N")
            {
                lnk_InvoiceDelete.Visible = false;

            }
            if (View == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
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
    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            rgRecurringInvoice.Columns[7].Visible = true;
        }
        else
        {
            rgRecurringInvoice.Columns[7].Visible = false;
            Session["CmpChkDefault"] = "2";
        }
    }

    private void BindRecurringInvoice(int? taxable = 0 , bool isreset=false , bool GetDataFromServer=true)
    {
        try
        {
            //todo
            if (!Convert.ToBoolean(ViewState["isCanada"]))
            {
                if (chkTaxable.Checked == true)
                {
                    taxable = 1;
                }
            }
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (hdnLocId.Value != string.Empty)
            {
                objProp_Contracts.Loc = Convert.ToInt32(hdnLocId.Value);
            }
            else
            {
                objProp_Contracts.Loc = 0;
            }

            if (hdnPatientId.Value != string.Empty)
            {
                objProp_Contracts.Owner = Convert.ToInt32(hdnPatientId.Value);
            }
            else
            {
                objProp_Contracts.Owner = 0;
            }
            objProp_Contracts.ConnConfig = Session["config"].ToString(); 
            objProp_Contracts.Month = Convert.ToInt32(ddlMonths.SelectedValue) + 1;
            objProp_Contracts.Year = Convert.ToInt32(ddlYears.SelectedValue);
            objProp_Contracts.Handel = Convert.ToInt16(ddlSpecialNotes.SelectedItem.Value);
            objProp_Contracts.UserID = Session["UserID"].ToString();
            if (Session["CmpChkDefault"].ToString() == "1")
            {
                objProp_Contracts.FlagEN = 1;
            }
            else
            {
                objProp_Contracts.FlagEN = 0;
            }
            string GriDCust = ""; string GriDLoc = ""; string GriDLocAcc = "";

            string _filterExpression = rgRecurringInvoice.MasterTableView.FilterExpression;
             
            if (_filterExpression != "")
            { 

                foreach (GridColumn column in rgRecurringInvoice.MasterTableView.OwnerGrid.Columns)
                {
                    String filterValues = column.CurrentFilterValue;

                    if (filterValues != "")
                    {
                        String columnName = column.UniqueName;

                        if (columnName == "customername") { GriDCust = filterValues; }
                        else if (columnName == "ItemDesc") { GriDLocAcc = filterValues; }
                        else if(columnName == "locname") { GriDLoc = filterValues; }
                    }
                }
 
            }
           

            //GetEmptyGriD
            if (!Page.IsPostBack)
            {
                objProp_Contracts.Owner = -1;
                objProp_Contracts.Loc = -1; 
            }

            else if (isreset)
            {
                objProp_Contracts.Owner = -1;
                objProp_Contracts.Loc = -1;
            }
            //todo

            if (GetDataFromServer)
            {
               
                ds = objBL_Contracts.GetListRecurringInvoices(objProp_Contracts ,  GriDCust ,  GriDLoc  , GriDLocAcc ); 

                ViewState["RecInvoices"] = ds;
            }
            else {

                ds = (DataSet) ViewState["RecInvoices"];
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0].Select("[Frequency] <> 'Never'").CopyToDataTable();
            }
            else
            {
                //dt = null;
            }

            //todo
            Double gstAmount = 0;
            if (Convert.ToBoolean(ViewState["isCanada"]) == true)
            {
                if (ddlTaxType.SelectedValue == "All")
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["TaxType"].ToString() == "3")
                        {
                            dr["GST"] = 0;
                            dr["stax"] = 0;
                            dr["total"] = Convert.ToDouble(dr["amount"]);
                        }
                        else
                        {
                            if (dr["TaxType"].ToString() == "2")
                            {
                                dr["stax"] = (((Convert.ToDouble(dr["Quan"]) * Convert.ToDouble(dr["Price"])) * Convert.ToDouble(dr["taxrate"])) / 100);
                                dr["total"] = Convert.ToDouble(dr["amount"]) + Convert.ToDouble(dr["stax"]);
                                dr["GST"] = 0;
                            }
                            else
                            {
                                gstAmount = 0;
                                if (dr["TaxType"].ToString() == "1")
                                {
                                    gstAmount = (((Convert.ToDouble(dr["Quan"]) * Convert.ToDouble(dr["Price"])) * Convert.ToDouble(dr["GSTRate"])) / 100);
                                    dr["GST"] = gstAmount;
                                    dr["stax"] = (((Convert.ToDouble(dr["Quan"]) * Convert.ToDouble(dr["Price"]) + gstAmount) * Convert.ToDouble(dr["taxrate"])) / 100);

                                    dr["total"] = Convert.ToDouble(dr["amount"]) + Convert.ToDouble(dr["stax"]) + Convert.ToDouble(dr["GST"]);
                                }
                                else
                                {
                                    dr["stax"] = (((Convert.ToDouble(dr["Quan"]) * Convert.ToDouble(dr["Price"])) * Convert.ToDouble(dr["taxrate"])) / 100);
                                    dr["GST"] = (((Convert.ToDouble(dr["Quan"]) * Convert.ToDouble(dr["Price"])) * Convert.ToDouble(dr["GSTRate"])) / 100);
                                    dr["total"] = Convert.ToDouble(dr["amount"]) + Convert.ToDouble(dr["stax"]) + Convert.ToDouble(dr["GST"]);
                                }

                            }
                        }

                    }
                    dt.AsEnumerable().ToList().ForEach(r => r["taxable"] = r["amount"]);
                }
                if (ddlTaxType.SelectedValue == "GST")
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["GST"] = 0;

                        if (dr["TaxType"].ToString() == "2" || dr["TaxType"].ToString() == "3")
                        {
                            
                            dr["stax"] = 0;
                            dr["total"] = Convert.ToDouble(dr["amount"]) + Convert.ToDouble(dr["stax"]);
                            dr["GST"] = 0;
                        }
                        else
                        {     
                            dr["stax"] = 0;
                            dr["GST"] = (((Convert.ToDouble(dr["Quan"]) * Convert.ToDouble(dr["Price"])) * Convert.ToDouble(dr["GSTRate"])) / 100);
                            dr["total"] = Convert.ToDouble(dr["amount"]) + Convert.ToDouble(dr["stax"]) + Convert.ToDouble(dr["GST"]);
                        }


                    }
                    dt.AsEnumerable().ToList().ForEach(r => r["taxable"] = r["amount"]);
                }

                if (ddlTaxType.SelectedValue == "PST")
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["GST"] = 0;
                        if (dr["TaxType"].ToString() == "3")
                        {
                            dr["GST"] = 0;
                            dr["stax"] = 0;
                            dr["total"] = Convert.ToDouble(dr["amount"]);
                        }
                        else
                        {
                            if (dr["TaxType"].ToString() == "2")
                            {
                                dr["stax"] = (((Convert.ToDouble(dr["Quan"]) * Convert.ToDouble(dr["Price"])) * Convert.ToDouble(dr["taxrate"])) / 100);
                                dr["total"] = Convert.ToDouble(dr["amount"]) + Convert.ToDouble(dr["stax"]);
                                dr["GST"] = 0;
                            }
                            else
                            {
                                dr["stax"] = (((Convert.ToDouble(dr["Quan"]) * Convert.ToDouble(dr["Price"])) * Convert.ToDouble(dr["taxrate"])) / 100);
                                dr["total"] = Convert.ToDouble(dr["amount"]) + Convert.ToDouble(dr["stax"]);
                                dr["GST"] = 0;

                            }
                        }



                    }
                    dt.AsEnumerable().ToList().ForEach(r => r["taxable"] = r["amount"]);
                }

                if (ddlTaxType.SelectedValue == "None")
                {
                    dt.AsEnumerable().ToList().ForEach(r => r.SetField("stax", 0.00));
                    dt.AsEnumerable().ToList().ForEach(r => r["total"] = r["amount"]);
                    dt.AsEnumerable().ToList().ForEach(r => r.SetField("taxable", 0.00));
                }
            }
            else
            {
                if (chkTaxable.Checked.Equals(true))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["stax"] = (((Convert.ToDouble(dr["Quan"]) * Convert.ToDouble(dr["Price"])) * Convert.ToDouble(dr["taxrate"])) / 100);
                        dr["total"] = Convert.ToDouble(dr["amount"]) + Convert.ToDouble(dr["stax"]);

                    }
                    dt.AsEnumerable().ToList().ForEach(r => r["taxable"] = r["amount"]);
                }
                else if (chkTaxable.Checked.Equals(false))
                {
                    dt.AsEnumerable().ToList().ForEach(r => r.SetField("stax", 0.00));
                    dt.AsEnumerable().ToList().ForEach(r => r["total"] = r["amount"]);
                    dt.AsEnumerable().ToList().ForEach(r => r.SetField("taxable", 0.00));
                }
            }

            lblRecordCount.Text = dt.Rows.Count.ToString() + " Record(s) found";

            rgRecurringInvoice.DataSource = dt;

            upSearch.Update();

            DataTable dtr = dt;
           

            BindGridDatatable(dtr);

            ViewState["RecInvoiceSrch"] = dtr;

            DataSet dsProcess = new DataSet();
            dsProcess = objBL_Contracts.GetLastProcessDateOfInvoice(objProp_Contracts);
            if (dsProcess.Tables[0].Rows.Count > 0)
            {
                string strLastProcess = dsProcess.Tables[0].Rows[0]["custom17"].ToString();
                string strLastProcessPeriod = dsProcess.Tables[0].Rows[0]["custom15"].ToString();
                if (strLastProcess != string.Empty)
                {
                    DateTime lastprocessdate = Convert.ToDateTime(strLastProcess);
                    lblLastProcessDate.Text = "Last: " + lastprocessdate.ToString("MM/dd/yyyy (hh:mm tt)");
                    lblProcessPeriod.Text = "Last Period: " + strLastProcessPeriod.ToString();
                }
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void BindGridDatatable(DataTable dt)
    {
        try
        {
            ViewState["RecInvoiceSrch"] = dt;
            rgRecurringInvoice.DataSource = dt;
            //rgRecurringInvoice.DataBind();

            if (dt.Rows.Count > 0)
            {
                //GridFooterItem fitem = (GridFooterItem)rgRecurringInvoice.MasterTableView.GetItems(GridItemType.Footer)[0]; 
                //Label lblTotalPretaxAmt = (Label)rgRecurringInvoice.FindControl("lblTotalPretaxAmt");
                //Label lblTotalSalesTax = (Label)rgRecurringInvoice.FindControl("lblTotalSalesTax");
                //Label lblTotalInvoice = (Label)rgRecurringInvoice.FindControl("InvTotalInvoice");
                double TotalPretaxAmt = 0;
                double TotalSalesTax = 0;
                double TotalInvoice = 0;

                double PretaxAmt = 0;
                double SalesTax = 0;
                double Invoice = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["amount"] != DBNull.Value && dr["amount"] != "")
                    {
                        PretaxAmt = Convert.ToDouble(dr["amount"]);
                    }

                    //if (dr["taxrate"] != DBNull.Value && dr["taxrate"] != "")
                    //{
                    //    SalesTax = Convert.ToDouble(dr["taxrate"]);
                    //}

                    if (dr["total"] != DBNull.Value && dr["total"] != "")
                    {
                        Invoice = Convert.ToDouble(dr["total"]);
                    }

                    TotalPretaxAmt += PretaxAmt;
                    TotalSalesTax += SalesTax;
                    TotalInvoice += Invoice;
                }

                //getTotalItem = new GetTotalItemModel();
                //getTotalItem.TotalPretaxAmt = string.Format("{0:c}", TotalPretaxAmt).ToString();
                //getTotalItem.TotalInvoice = string.Format("{0:c}", TotalInvoice).ToString();
                //lblTotalPretaxAmt.Text = string.Format("{0:c}", TotalPretaxAmt).ToString();
                ////lblTotalSalesTax.Text = string.Format("{0:c}", TotalSalesTax);
                //lblTotalInvoice.Text = string.Format("{0:c}", TotalInvoice).ToString();

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void SortGridView(string sortExpression, string direction)
    {
        DataTable dt = PageSortData();

        DataView dv = new DataView(dt);
        dv.Sort = sortExpression + direction;

        BindGridDatatable(dv.ToTable());
        rgRecurringInvoice.DataBind();
    }
    private DataTable PageSortData()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["RecInvoiceSrch"];
        return dt;
    }

    private void GetPeriodDetails(DateTime _invDate)
    {
        bool _flag = CommonHelper.GetPeriodDetails(_invDate);
        ViewState["FlagPeriodClose"] = _flag;
        if (!_flag)
        {
            divSuccess.Visible = true;
        }
        else
        {
            divSuccess.Visible = false;
        }
    }
    #endregion

    protected void chkTaxable_CheckedChanged(object sender, EventArgs e)
    {
        if (chkTaxable.Checked == true)
        {
            BindRecurringInvoice(1,false,false);
        }
        else
        {
            BindRecurringInvoice(0, false, false);
        }
    }

    private DateTime DueByTerms()
    {
        if (ddlTerms.SelectedValue == "0")
        {
            return Convert.ToDateTime(txtInvoiceDt.Text);
        }
        else if (ddlTerms.SelectedValue == "1")
        {
            return Convert.ToDateTime(txtInvoiceDt.Text).AddDays(10);
        }
        else if (ddlTerms.SelectedValue == "2")
        {
            return Convert.ToDateTime(txtInvoiceDt.Text).AddDays(15);
        }
        else if (ddlTerms.SelectedValue == "3")
        {
            return Convert.ToDateTime(txtInvoiceDt.Text).AddDays(30);
        }
        else if (ddlTerms.SelectedValue == "4")
        {
            return Convert.ToDateTime(txtInvoiceDt.Text).AddDays(45);
        }
        else if (ddlTerms.SelectedValue == "5")
        {
            return Convert.ToDateTime(txtInvoiceDt.Text).AddDays(60);
        }
        else if (ddlTerms.SelectedValue == "6")
        {
            return Convert.ToDateTime(txtInvoiceDt.Text).AddDays(30);
        }
        else if (ddlTerms.SelectedValue == "7")
        {
            return Convert.ToDateTime(txtInvoiceDt.Text).AddDays(90);
        }
        else if (ddlTerms.SelectedValue == "8")
        {
            return Convert.ToDateTime(txtInvoiceDt.Text).AddDays(180);
        }
        else if (ddlTerms.SelectedValue == "9")
        {
            return Convert.ToDateTime(txtInvoiceDt.Text);
        }
        else if (ddlTerms.SelectedValue == "10") //120 days
        {
            return Convert.ToDateTime(txtInvoiceDt.Text).AddDays(120);
        }
        else if (ddlTerms.SelectedValue == "11") //150 days
        {
            return Convert.ToDateTime(txtInvoiceDt.Text).AddDays(150);
        }
        else if (ddlTerms.SelectedValue == "12") //210 days
        {
            return Convert.ToDateTime(txtInvoiceDt.Text).AddDays(210);
        }
        else if (ddlTerms.SelectedValue == "13") //240 days
        {
            return Convert.ToDateTime(txtInvoiceDt.Text).AddDays(240);
        }
        else if (ddlTerms.SelectedValue == "14") //270 days
        {
            return Convert.ToDateTime(txtInvoiceDt.Text).AddDays(270);
        }
        else if (ddlTerms.SelectedValue == "15") //300 days
        {
            return Convert.ToDateTime(txtInvoiceDt.Text).AddDays(300);
        }
        else if (ddlTerms.SelectedValue == "16") //net due on 10th
        {
            return DateTime.Now;
        }
        else if (ddlTerms.SelectedValue == "17") //net due
        {
            return DateTime.Now;
        }
        else if (ddlTerms.SelectedValue == "18") //Credit card
        {
            return DateTime.Now;
        }
        return DateTime.Now;
    }
   
    protected void lnk_InvoiceMaint_Click(object sender, EventArgs e)
    {
        try
        {
            Session["InvoiceName"] = "InvoiceMaint";
            if (Convert.ToInt32(ViewState["mode"]) != 1)
            {
                GetPeriodDetails(Convert.ToDateTime(txtInvoiceDt.Text));
            }
            if ((bool)ViewState["FlagPeriodClose"])
            {
                #region Process Invoice
                objProp_Contracts.ConnConfig = Session["config"].ToString();
                if (txtInvoiceDt.Text != string.Empty)
                {
                    objProp_Contracts.Date = Convert.ToDateTime(txtInvoiceDt.Text);
                }
                else
                {
                    objProp_Contracts.Date = System.DateTime.Now;
                }
                objProp_Contracts.PostDate = Convert.ToDateTime(txtPostDt.Text);
                objProp_Contracts.DueDate = DueByTerms();
                objProp_Contracts.Remarks = txtRemark.Text;
                objProp_Contracts.PaymentTerms = Convert.ToInt32(ddlTerms.SelectedValue);
                //todo
                if (Convert.ToBoolean(ViewState["isCanada"]) == true)
                {
                    objProp_Contracts.TaxApply = ddlTaxType.SelectedValue;
                }
                else
                {
                    objProp_Contracts.Taxable = Convert.ToInt32(chkTaxable.Checked);
                }
                //objProp_Contracts.ProcessPeriod = txtStartDt.Text + " - " + txtEndDate.Text;
                objProp_Contracts.ProcessPeriod = ddlMonths.SelectedItem.Text + " " + ddlYears.SelectedValue;

                DataTable dtr = (DataTable)ViewState["RecInvoiceSrch"];
                dtr.AsEnumerable().ToList()
                    .ForEach(r => r["fDate"] = DateTime.Parse(txtPostDt.Text));

                //detailLevel=2 detailed with price code
                DataTable dtNew = new DataTable();
                dtNew = dtr.Clone();
                DataColumn dtcol = dtNew.Columns.Add("detailItem", typeof(string));
                foreach (DataRow drow in dtr.Rows)
                {
                    DataSet dsTemp = new DataSet();
                    DataTable dtTemp = new DataTable();
                    objProp_Contracts.JobId = Convert.ToInt32(drow["OrgLoc"].ToString());
                    dsTemp = objBL_Contracts.GetEquipmentByInvoice(objProp_Contracts);
                    if (drow["detailLevel"].ToString() == "2")
                    {
                        foreach (DataRow dr in dsTemp.Tables[0].Rows)
                        {
                            if (drow["job"].ToString() == dr["id"].ToString())
                            {
                                var newDataRow = dtNew.NewRow();
                                newDataRow.ItemArray = drow.ItemArray;
                                newDataRow["detailItem"] = dr["Unit"];
                                newDataRow["Price"] = dr["Price"];
                                newDataRow["Amount"] = dr["Price"];
                                dtNew.Rows.Add(newDataRow);
                            }
                        }
                    }
                    else
                    {
                        var newdr = dtNew.NewRow();
                        newdr.ItemArray = drow.ItemArray;
                        dtNew.Rows.Add(newdr);
                    }
                }

                objProp_Contracts.Fuser = Session["User"].ToString();
                if (dtNew.Columns.Contains("EN"))
                {
                    dtNew.Columns.Remove("EN");
                }

                if (dtNew.Columns.Contains("Company"))
                {
                    dtNew.Columns.Remove("Company");
                }
                dtNew.AcceptChanges();
                objProp_Contracts.DtRecContr = dtNew;

                //todo
                DataSet _ds = new DataSet();
                if (Convert.ToBoolean(ViewState["isCanada"]) == true)
                {                   
                    _ds = objBL_Contracts.AddRecurringInvoices(objProp_Contracts);
                }
                else
                {
                    _ds = objBL_Contracts.CreateRecurringInvoices(objProp_Contracts);
                }

                //BindRecurringInvoice();
                rgRecurringInvoice.Rebind();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "keySuccUp", "noty({text: 'Invoices processed successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                #endregion
                if (_ds != null && _ds.Tables.Count > 0)
                {
                    mpeRecurInv.Show();
                    if (_ds.Tables[0].Rows.Count > 0)
                    {
                        txtStartInv.Text = _ds.Tables[0].Rows[0]["Ref"].ToString();
                        int _index = _ds.Tables[_ds.Tables.Count - 1].Rows.Count - 1;
                        txtEndInv.Text = _ds.Tables[_ds.Tables.Count - 1].Rows[0]["Ref"].ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    protected void lnk_InvoiceException_Click(object sender, EventArgs e)
    {
        try
        {
            Session["InvoiceName"] = "InvoiceException";
            if (Convert.ToInt32(ViewState["mode"]) != 1)
            {
                GetPeriodDetails(Convert.ToDateTime(txtInvoiceDt.Text));
            }
            if ((bool)ViewState["FlagPeriodClose"])
            {
                #region Process Invoice
                objProp_Contracts.ConnConfig = Session["config"].ToString();
                if (txtInvoiceDt.Text != string.Empty)
                {
                    objProp_Contracts.Date = Convert.ToDateTime(txtInvoiceDt.Text);
                }
                else
                {
                    objProp_Contracts.Date = System.DateTime.Now;
                }
                objProp_Contracts.PostDate = Convert.ToDateTime(txtPostDt.Text);
                objProp_Contracts.DueDate = DueByTerms();
                objProp_Contracts.Remarks = txtRemark.Text;
                objProp_Contracts.PaymentTerms = Convert.ToInt32(ddlTerms.SelectedValue);
                //todo
                if (Convert.ToBoolean(ViewState["isCanada"]) == true)
                {
                    objProp_Contracts.TaxApply = ddlTaxType.SelectedValue;
                }
                else
                {
                    objProp_Contracts.Taxable = Convert.ToInt32(chkTaxable.Checked);
                }
                
                objProp_Contracts.ProcessPeriod = ddlMonths.SelectedItem.Text + " " + ddlYears.SelectedValue;

                DataTable dtr = (DataTable)ViewState["RecInvoiceSrch"];
                dtr.AsEnumerable().ToList()
                    .ForEach(r => r["fDate"] = DateTime.Parse(txtPostDt.Text));
                //detailLevel=2 detailed with price code
                DataTable dtNew = new DataTable();
                dtNew = dtr.Clone();
                DataColumn dtcol = dtNew.Columns.Add("detailItem", typeof(string));
                foreach (DataRow drow in dtr.Rows)
                {
                    DataSet dsTemp = new DataSet();
                    DataTable dtTemp = new DataTable();
                    objProp_Contracts.JobId = Convert.ToInt32(drow["OrgLoc"].ToString());
                    dsTemp = objBL_Contracts.GetEquipmentByInvoice(objProp_Contracts);
                    if (drow["detailLevel"].ToString() == "2")
                    {
                        foreach (DataRow dr in dsTemp.Tables[0].Rows)
                        {
                            if (drow["job"].ToString() == dr["id"].ToString())
                            {
                                var newDataRow = dtNew.NewRow();
                                newDataRow.ItemArray = drow.ItemArray;
                                newDataRow["detailItem"] = dr["Unit"];
                                newDataRow["Price"] = dr["Price"];
                                newDataRow["Amount"] = dr["Price"];
                                dtNew.Rows.Add(newDataRow);
                            }
                        }
                    }
                    else
                    {
                        var newdr = dtNew.NewRow();
                        newdr.ItemArray = drow.ItemArray;
                        dtNew.Rows.Add(newdr);
                    }
                }

                objProp_Contracts.Fuser = Session["User"].ToString();
                if (dtNew.Columns.Contains("EN"))
                {
                    dtNew.Columns.Remove("EN");
                }

                if (dtNew.Columns.Contains("Company"))
                {
                    dtNew.Columns.Remove("Company");
                }
                dtNew.AcceptChanges();
                objProp_Contracts.DtRecContr = dtNew;
                //todo
                DataSet _ds = new DataSet();
                if (Convert.ToBoolean(ViewState["isCanada"]) == true)
                {
                    _ds = objBL_Contracts.AddRecurringInvoices(objProp_Contracts);
                }
                else
                {
                    _ds = objBL_Contracts.CreateRecurringInvoices(objProp_Contracts);
                }

                //BindRecurringInvoice();
                rgRecurringInvoice.Rebind();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "keySuccUp", "noty({text: 'Invoices processed successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                #endregion
                if (_ds != null && _ds.Tables.Count > 0)
                {
                    mpeRecurInv.Show();

                    if (_ds.Tables[0].Rows.Count > 0)
                    {
                        txtStartInv.Text = _ds.Tables[0].Rows[0]["Ref"].ToString();
                        int _index = _ds.Tables[_ds.Tables.Count - 1].Rows.Count - 1;
                        txtEndInv.Text = _ds.Tables[_ds.Tables.Count - 1].Rows[0]["Ref"].ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }
    
    protected void lnk_InvoiceLNY_Click(object sender, EventArgs e)
    {
        try
        {
            Session["InvoiceName"] = "Invoice-LNY";
            if (Convert.ToInt32(ViewState["mode"]) != 1)
            {
                GetPeriodDetails(Convert.ToDateTime(txtInvoiceDt.Text));
            }
            if ((bool)ViewState["FlagPeriodClose"])
            {
                #region Process Invoice
                objProp_Contracts.ConnConfig = Session["config"].ToString();
                //todo
                if (Convert.ToBoolean(ViewState["isCanada"]) == true)
                {
                    objProp_Contracts.TaxApply = ddlTaxType.SelectedValue;
                }
                else
                {
                    objProp_Contracts.Taxable = Convert.ToInt32(chkTaxable.Checked);
                }
                objProp_Contracts.PostDate = Convert.ToDateTime(txtPostDt.Text);
                objProp_Contracts.DueDate = DueByTerms();
                objProp_Contracts.Remarks = txtRemark.Text;
                objProp_Contracts.PaymentTerms = Convert.ToInt32(ddlTerms.SelectedValue);
                objProp_Contracts.Taxable = Convert.ToInt32(chkTaxable.Checked);
                //objProp_Contracts.ProcessPeriod = txtStartDt.Text + " - " + txtEndDate.Text;
                objProp_Contracts.ProcessPeriod = ddlMonths.SelectedItem.Text + " " + ddlYears.SelectedValue;

                DataTable dtr = (DataTable)ViewState["RecInvoiceSrch"];
                dtr.AsEnumerable().ToList()
                    .ForEach(r => r["fDate"] = DateTime.Parse(txtPostDt.Text));

                objProp_Contracts.Fuser = Session["User"].ToString();

                if (dtr.Columns.Contains("EN"))
                {
                    dtr.Columns.Remove("EN");
                }

                if (dtr.Columns.Contains("Company"))
                {
                    dtr.Columns.Remove("Company");
                }
                dtr.AcceptChanges();

                objProp_Contracts.DtRecContr = dtr;
                //todo
                DataSet _ds = new DataSet();
                if (Convert.ToBoolean(ViewState["isCanada"]) == true)
                {
                    _ds = objBL_Contracts.AddRecurringInvoices(objProp_Contracts);
                }
                else
                {
                    _ds = objBL_Contracts.CreateRecurringInvoices(objProp_Contracts);
                }

                //BindRecurringInvoice();
                rgRecurringInvoice.Rebind();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "keySuccUp", "noty({text: 'Invoices processed successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                #endregion
                if (_ds != null && _ds.Tables.Count > 0)
                {
                    mpeRecurInv.Show();

                    if (_ds.Tables[0].Rows.Count > 0)
                    {
                        txtStartInv.Text = _ds.Tables[0].Rows[0]["Ref"].ToString();
                        int _index = _ds.Tables[_ds.Tables.Count - 1].Rows.Count - 1;
                        txtEndInv.Text = _ds.Tables[_ds.Tables.Count - 1].Rows[0]["Ref"].ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }
  
    protected void ddlMonths_SelectedIndexChanged(object sender, EventArgs e)
    {

        //rgRecurringInvoice.DataSource = null;
        //rgRecurringInvoice.DataBind();
        //lblRecordCount.Text = " 0 Record(s) found";
    }
   
    protected void ddlYears_SelectedIndexChanged(object sender, EventArgs e)
    {
        //rgRecurringInvoice.DataSource = null;
        //rgRecurringInvoice.DataBind();
        //lblRecordCount.Text = " 0 Record(s) found";
    }
   
    protected void ddlSpecialNotes_SelectedIndexChanged(object sender, EventArgs e)
    {
        //rgRecurringInvoice.DataSource = null;
        //rgRecurringInvoice.DataBind();
        //lblRecordCount.Text = " 0 Record(s) found";
    }
   
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {

        //rgRecurringInvoice.DataSource = null;
        //rgRecurringInvoice.DataBind();
        //lblRecordCount.Text = " 0 Record(s) found";
    }
    
    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        //rgRecurringInvoice.DataSource = null;
        //rgRecurringInvoice.DataBind();
        //lblRecordCount.Text = " 0 Record(s) found";
    }

    protected void rgRecurringInvoice_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {        
        BindRecurringInvoice();
    }

    protected void rgRecurringInvoice_ItemDataBound(object sender, GridItemEventArgs e)
    {
        //if (e.Item is GridFooterItem)
        //{
        //    if (getTotalItem != null)
        //    {
        //        var pretaxAmtVal = getTotalItem.TotalPretaxAmt;
        //        var invoiceVal = getTotalItem.TotalInvoice;
        //        GridFooterItem footerItem = (GridFooterItem)e.Item;
        //        Label lblTotalPretaxAmt = (Label)footerItem.FindControl("lblTotalPretaxAmt");
        //        lblTotalPretaxAmt.Text = pretaxAmtVal;
        //        Label lblInvTotalInvoicet = (Label)footerItem.FindControl("InvTotalInvoice");
        //        lblInvTotalInvoicet.Text = invoiceVal;
        //    }
        //}

    }

    protected void rgRecurringInvoice_ItemEvent(object sender, GridItemEventArgs e)
    {
        if (e.EventInfo is GridInitializePagerItem)
        {
            totalItemCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
            if (totalItemCount < 2)
            {
                lblRecordCount.Text = totalItemCount.ToString() + " Record found";
            }
            else if (totalItemCount > 1)
            {
                lblRecordCount.Text = totalItemCount.ToString() + " Record(s) found";
            }
            if (totalItemCount == 0)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "hideSelectAllChkb", "hideSelectAllChkb();", true);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "showSelectAllChkb", "showSelectAllChkb();", true);

            }
        }
    }

    protected void rgRecurringInvoice_PreRender(object sender, EventArgs e)
    {
        GeneralFunctions obj = new GeneralFunctions();

        obj.CorrectTelerikPager(rgRecurringInvoice);

    }

    protected void rgRecurringInvoice_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridPagerItem)
        {
            var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");

            var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
            
            if (totalCount == 0) { totalCount = 1000; }

            GeneralFunctions obj = new GeneralFunctions();

            var sizes = obj.TelerikPageSize(totalCount);

            dropDown.Items.Clear();

            foreach (var size in sizes)
            {
                var cboItem = new RadComboBoxItem() { Text = size.Key, Value = size.Value };
                cboItem.Attributes.Add("ownerTableViewId", e.Item.OwnerTableView.ClientID);
                if (e.Item.OwnerTableView.PageSize.ToString() == size.Value) { cboItem.Selected = true; }
                dropDown.Items.Add(cboItem);
            }
            //dropDown.FindItemByValue(e.Item.OwnerTableView.PageSize.ToString()).Selected = true;

        }

        if(e.Item is GridFilteringItem)
        {
            if (Convert.ToString(rgRecurringInvoice.MasterTableView.FilterExpression) != "")
            {
                lblRecordCount.Text = rgRecurringInvoice.MasterTableView.DataSourceCount + " Record(s) found";
                upSearch.Update();
            }
        }
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
       // rgRecurringInvoice.MasterTableView.GetColumn("ClientSelectColumn").Visible = false;
        rgRecurringInvoice.ExportSettings.FileName = "RecurringInvoice";
        rgRecurringInvoice.ExportSettings.IgnorePaging = true;
        rgRecurringInvoice.ExportSettings.ExportOnlyData = true;
        rgRecurringInvoice.ExportSettings.OpenInNewWindow = true;
        rgRecurringInvoice.ExportSettings.HideStructureColumns = true;
        rgRecurringInvoice.MasterTableView.UseAllDataFields = true;
        rgRecurringInvoice.ExportSettings.Excel.Format = GridExcelExportFormat.ExcelML;
        rgRecurringInvoice.MasterTableView.ExportToExcel();
    }

    protected void rgRecurringInvoice_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 6;
        else
            currentItem = 7;
        if (e.Worksheet.Table.Rows.Count == rgRecurringInvoice.Items.Count + 1)
        {
            GridFooterItem footerItem = rgRecurringInvoice.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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

                if (!CheckControlExist(c.UniqueID))
                {
                    switch (c.GetType().ToString())
                    {
                        case "System.Web.UI.WebControls.DropDownList":
                                ((DropDownList)c).SelectedIndex = -1;
                            break;
                        case "System.Web.UI.WebControls.TextBox":
                            ((TextBox)c).Text = string.Empty;
                            break;
                        case "System.Web.UI.WebControls.CheckBox":
                            ((CheckBox)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.RadioButton":
                            ((RadioButton)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.HtmlTextArea":
                            ((HtmlTextArea)c).Value = string.Empty;
                            break;
                        case "System.Web.UI.WebControls.HiddenField":
                            ((HiddenField)c).Value = string.Empty;
                            break;
                    }
                }
            }
        }
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        ResetFormControlValues(this);

        foreach (GridColumn column in rgRecurringInvoice.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        rgRecurringInvoice.MasterTableView.FilterExpression = string.Empty;
        rgRecurringInvoice.Rebind();
    }

    private List<string> ControlNotReset()
    {
        List<string> control = new List<string>();
        control.Add("ddlMonths");
        control.Add("ddlYears");
        control.Add("txtInvoiceDt");
        control.Add("txtPostDt");
        return control;
    }

    private bool CheckControlExist(string controlId)
    {
        bool check = false;
        foreach (var item in ControlNotReset())
        {
            if (controlId.Contains(item))
            {
                check = true;
            }
        }
        return check;
    }

    protected void AvoidResubmit_Click(object sender, EventArgs e)
    {
        mpeRecurInv.Show();
    }

    protected void lbtnClose_Click(object sender, EventArgs e)
    {

    }
     
    protected void lnkProcess_Click(object sender, EventArgs e)
    {
        hdnCreditHold.Value = "0";

        foreach (GridDataItem gr in rgRecurringInvoice.MasterTableView.Items)
        {
            string  lblcredithold = ((Label)gr.FindControl("Recurringlblcredithold")).Text.Trim();

            if (lblcredithold == "1")
            {

                hdnCreditHold.Value = "1";
            }
        }


        if (hdnCreditHold.Value == "0")
        {
            ProcessRecurringInvoice();
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "keyhdnCreditHold", "CreditHoldnotyConfirm();", true);
        }
    }
     
    protected void lnkExclude_Click(object sender, EventArgs e)
    {
        hdnCreditHold.Value = "1";
        ProcessRecurringInvoice();
    }
     
    protected void lnkInclude_Click(object sender, EventArgs e)
    {
        hdnCreditHold.Value = "0";
        ProcessRecurringInvoice();
    }
    
    public Boolean isCanadaCompany()
    {
        General _objPropGeneral = new General();
        BL_General _objBLGeneral = new BL_General();
        Boolean flag = false;
        _objPropGeneral.ConnConfig = Session["config"].ToString();
        DataSet _dsCustom = _objBLGeneral.getCompanyCountry(_objPropGeneral);
        try
        {
            if (_dsCustom.Tables[0].Rows[0]["Country"].ToString() == "Canada")
            {
                flag = true;
            }
        }
        catch (Exception ex)
        {
            flag = false;
        }
        return flag;
    }
    
    private void SetupCanadaCompanyUI()
    {
        ViewState["isCanada"] = isCanadaCompany();
        if (Convert.ToBoolean(ViewState["isCanada"])==true)
        {
            divDropDownlist.Style.Add("display", "block");
            divCheckBox.Style.Add("display", "none");
            //for grid
            //rgRecurringInvoice.Columns.FindByUniqueName("InvoiceTotal").Visible = true;
            rgRecurringInvoice.Columns.FindByUniqueName("GSTTax").Visible = true;
            rgRecurringInvoice.Columns.FindByUniqueName("stax").HeaderText = "HST/PST Tax";
        }
        else
        {
            divDropDownlist.Style.Add("display", "none");
            divCheckBox.Style.Add("display", "block");

            //for grid
            // rgRecurringInvoice.Columns.FindByUniqueName("InvoiceTotal").Visible = false;
            rgRecurringInvoice.Columns.FindByUniqueName("GSTTax").Visible = false;
            rgRecurringInvoice.Columns.FindByUniqueName("stax").HeaderText = "Sales Tax";

        }


    }
    
    private void getUIHistory()
    {
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        DataSet _dsCustom = objBL_Contracts.GetRecurringInvoicesUIHistory(objProp_Contracts);
        try
        {
            if (Convert.ToBoolean(_dsCustom.Tables[0].Rows[0]["IsCanadaCompany"]) == true)
            {
                ddlTaxType.SelectedValue = _dsCustom.Tables[0].Rows[0]["TaxType"].ToString();
            }
            else
            {
                chkTaxable.Checked = Convert.ToBoolean(_dsCustom.Tables[0].Rows[0]["Taxable"]);
            }
            ddlTerms.SelectedValue = Convert.ToString(_dsCustom.Tables[0].Rows[0]["PaymentTerms"]);
            txtRemark.Text = Convert.ToString(_dsCustom.Tables[0].Rows[0]["Remarks"]);
        }
        catch (Exception ex)
        {

        }

    }
   
    protected void ddlTaxType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindRecurringInvoice(0,false,false);
    }
    private void FillTerms()
    {
        try
        {
            DataSet ds = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();

            ds = objBL_User.getTerms(objPropUser);

            ddlTerms.DataSource = ds.Tables[0];
            ddlTerms.DataTextField = "name";
            ddlTerms.DataValueField = "id";
            ddlTerms.DataBind();

            ddlTerms.Items.Insert(0, new ListItem(":: Select ::", ""));
           
            if (ConfigurationManager.AppSettings["CustomerName"].ToString().Equals("PCE"))
            {
                if (ds.Tables[0].Select("name ='Due on receipt'").Count() > 0)
                {
                    DataRow[] rows = ds.Tables[0].Select("name ='Due on receipt'");
                    ddlTerms.SelectedValue = rows[0]["id"].ToString();
                }

            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr754", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #region logs

    protected void RadGrid_gvLogs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        loadLog();
    }
    private void loadLog()
    {
        try
        {
            RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();

            DataSet dsLog = new DataSet();
            dsLog = objBL_Contracts.GetRecurringInvoicesLogs(Session["config"].ToString());
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
        catch { }
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
}