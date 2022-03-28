using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLayer;
using BusinessEntity;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class PaymentHistory:System.Web.UI.Page
{
    #region Variables
    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();

    GeneralFunctions objGeneralFunctions = new GeneralFunctions();
    private static readonly string CookieName = "Rad_CreditCard";
    protected void Page_Init(object sender, EventArgs e)
    {
        RadPersistenceCreditCard.StorageProviderKey = CookieName;
        RadPersistenceCreditCard.StorageProvider = new CookieStorageProvider(CookieName);
    }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();
        if (SSL == "1")
        {
            bool isLocal = HttpContext.Current.Request.IsLocal;
            if (!isLocal)
            {
                bool isSecure = HttpContext.Current.Request.IsSecureConnection;
                string webPath = System.Web.Configuration.WebConfigurationManager.AppSettings["webPath"].Trim();
                if (!isSecure)
                {
                    if (Session["type"].ToString() == "c")
                    {
                        bool port = HttpContext.Current.Request.Url.IsDefaultPort;
                        string Auth = HttpContext.Current.Request.Url.Authority;
                        if (!port)
                        {
                            Auth = HttpContext.Current.Request.Url.DnsSafeHost;
                        }
                        string URL = Auth + webPath;
                        string redirect = "HTTPS://" + URL + "/PaymentHistory.aspx";
                        int ii = 0;
                        foreach (String key in Request.QueryString.AllKeys)
                        {
                            if (ii == 0)
                                redirect += "?" + key + "=" + Request.QueryString[key];
                            else
                                redirect += "&" + key + "=" + Request.QueryString[key];
                            ii++;
                        }
                        Response.Redirect(redirect);
                    }
                }
            }
        }

        if (!IsPostBack)
        { 
            userpermissions();
            GetCustomerAll();
            if (Session["PaymtfromDate"] == null && Session["PaymtToDate"] == null)
            {
                DateTime firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                int DaysinMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - 1;
                DateTime lastDay = firstDay.AddDays(DaysinMonth);
                txtFromDate.Text = firstDay.ToShortDateString();
                txtToDate.Text = lastDay.ToShortDateString();
            }
            else
            {
                txtFromDate.Text = Session["PaymtfromDate"].ToString();
                txtToDate.Text = Session["PaymtToDate"].ToString();
            }          
            if (Request.Cookies[CookieName] != null)
            {
                RadPersistenceCreditCard.LoadState();
                RadGrid_CreditCard.Rebind();
                updpnl.Update();
            }
        }
        Permission();
        CompanyPermission();
        HighlightSideMenu("acctMgr", "lnkPaymentHistory", "billMgrSub");
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

    private void Permission()
    {
        
        if (Session["type"].ToString() == "c")
        {
            //Response.Redirect("home.aspx");
            ddlCustomer.Visible = false;
            lblCustomer.Visible = false;
           // RadGrid_ACH.Columns[0].Visible = false;
        }

        if (Session["MSM"].ToString() == "TS")
        {
            //Response.Redirect("home.aspx");
            //pnlGridButtons.Visible = false;
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }

       
    }

    private void userpermissions()
    {
        if (Session["type"].ToString() != "c")
        {
            if (Session["type"].ToString() != "am")
            {
                objProp_User.ConnConfig = Session["config"].ToString();
                objProp_User.Username = Session["username"].ToString();
                objProp_User.PageName = "paymenthistory.aspx";
                DataSet dspage = objBL_User.getScreensByUser(objProp_User);
                if (dspage.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dspage.Tables[0].Rows[0]["access"].ToString()) == false)
                    {
                     //   Response.Redirect("home.aspx");
                    }
                }
                else
                {
                   // Response.Redirect("home.aspx");
                }
            }
        }


        // User Permission 
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = GetUserById();

            /// BillingmodulePermission ///////////////////------->

            string BillingmodulePermission = ds.Rows[0]["BillingmodulePermission"] == DBNull.Value ? "Y" : ds.Rows[0]["BillingmodulePermission"].ToString();
            
            if (BillingmodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }

            string POPermission = ds.Rows[0]["PaymentHistoryPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["PaymentHistoryPermission"].ToString();
            string ADD = POPermission.Length < 1 ? "Y" : POPermission.Substring(0, 1);
            string Edit = POPermission.Length < 2 ? "Y" : POPermission.Substring(1, 1);
            string Delete = POPermission.Length < 2 ? "Y" : POPermission.Substring(2, 1);
            string View = POPermission.Length < 4 ? "Y" : POPermission.Substring(3, 1);


            if (View == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
            //else if (Request.QueryString["id"] == null)
            //{
            //    if (ADD == "N")
            //    {
            //        Response.Redirect("Home.aspx?permission=no"); return;
            //    }
            //}
            //else if (Edit == "N")
            //{
            //    if (View == "Y")
            //    {
            //        btnSubmit.Visible = false;
            //    }
            //    else
            //    {
            //        Response.Redirect("Home.aspx?permission=no"); return;
            //    }
            //}
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
            RadGrid_CreditCard.Columns[5].Visible = true;
        }
        else
        {
            RadGrid_CreditCard.Columns[5].Visible = false;
        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }
    protected void lnkClear_Click(object sender, EventArgs e)
    {
        ClearControls();
    }
    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        ClearControls();
        FillPaymentHistory();
        RadGrid_CreditCard.Rebind();
        RadGrid_ACH.Rebind();
    }
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        if (hdnCssActive.Value == "CssActive")
        {
            Session["lblPaymtActive"] = "1";
        }
        else
        {
            Session["lblPaymtActive"] = "2";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "CssClearLabel()", true);
        }
        Session["PaymtfromDate"] = txtFromDate.Text;
        Session["PaymtToDate"] = txtToDate.Text;
        FillPaymentHistory();
        RadGrid_CreditCard.Rebind();
        RadGrid_ACH.Rebind();
    }
    private void FillPaymentHistory()
    {
        DataSet ds = new DataSet();
        DataSet ACHds = new DataSet();
        objProp_Contracts.ConnConfig = Session["config"].ToString();

        if (txtFromDate.Text != string.Empty)
        {
            objProp_Contracts.StartDate = Convert.ToDateTime(txtFromDate.Text);
        }
        else
        {
            objProp_Contracts.StartDate = System.DateTime.MinValue;
        }

        if (txtToDate.Text != string.Empty)
        {
            objProp_Contracts.EndDate = Convert.ToDateTime(txtToDate.Text);
        }
        else
        {
            objProp_Contracts.EndDate = System.DateTime.MinValue;
        }
        if (!String.IsNullOrEmpty(txtInvoiceNo.Text.Trim()))
        {
            objProp_Contracts.InvoiceID = Convert.ToInt32(txtInvoiceNo.Text.Trim());
        }
        else { objProp_Contracts.InvoiceID = 0; }

        objProp_Contracts.Medium = ddlMedium.SelectedValue;
        #region Company Check
        objProp_Contracts.UserID = Session["UserID"].ToString();
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
        {
            objProp_Contracts.EN = 1;
        }
        else
        {
            objProp_Contracts.EN = 0;
        }
        #endregion
        if (Session["type"].ToString() == "c")
        {
            objProp_Contracts.CustID = Convert.ToInt32(Session["custid"].ToString());

            DataTable dtcust = new DataTable();
            dtcust = (DataTable)Session["userinfo"];
            int RoleID = 0;
            if (dtcust.Rows.Count > 0)
            {
                RoleID = Convert.ToInt32(dtcust.Rows[0]["roleid"]);
                objProp_Contracts.RoleId = RoleID;
            }
        }
        else
        {
            objProp_Contracts.CustID = Convert.ToInt32(ddlCustomer.SelectedValue);
        }

        objProp_Contracts.IsSuccess = Convert.ToInt32(ddlApproved.SelectedValue);

        ds = objBL_Contracts.GetPaymentHistory(objProp_Contracts);
        ACHds = objBL_Contracts.GetACHPaymentHistory(objProp_Contracts, ddlApproved.SelectedItem.Text);

        BindGridDatatable(ds.Tables[0]);
        BindACHGridDatatable(ACHds.Tables[0]);
    }
    private void ClearControls()
    {
        objGeneralFunctions.ResetFormControlValues(this);
    }
    private void FillWorker()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.IsTS = Session["MSM"].ToString();
        ds = objBL_User.getEMPScheduler(objProp_User);
        ddlUser.DataSource = ds.Tables[0];
        ddlUser.DataTextField = "fDesc";
        ddlUser.DataValueField = "fDesc";
        ddlUser.DataBind();
        ddlUser.Items.Insert(0, new ListItem("All", ""));
    }

    private void BindGridDatatable(DataTable dt)
    {
        Session["PayHistorySrch"] = dt;
        RadGrid_CreditCard.VirtualItemCount = dt.Rows.Count;
        RadGrid_CreditCard.DataSource = dt;
        RadPersistenceCreditCard.SaveState();
    }
    private void BindACHGridDatatable(DataTable dt)
    {
        Session["ACHPayHistorySrch"] = dt;
        RadGrid_ACH.VirtualItemCount = dt.Rows.Count;
        RadGrid_ACH.DataSource = dt;

    }
    private void GetCustomerAll()
    {
        DataSet ds = new DataSet();
        objProp_User.DBName = Session["dbname"].ToString();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getCustomers(objProp_User);

        ddlCustomer.DataSource = ds.Tables[0];
        ddlCustomer.DataTextField = "Name";
        ddlCustomer.DataValueField = "ID";
        ddlCustomer.DataBind();
        ddlCustomer.Items.Insert(0, new ListItem(":: All ::", "0"));
    }

    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_CreditCard.MasterTableView.FilterExpression != "" ||
            (RadGrid_CreditCard.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_CreditCard.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_CreditCard_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_CreditCard.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        FillPaymentHistory();
    }
    protected void RadGrid_CreditCard_ItemEvent(object sender, GridItemEventArgs e)
    {
        int rowCount = 0;
        if (e.EventInfo is GridInitializePagerItem)
        {
            rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
        }
        lblRecordCount.Text = rowCount + " Record(s) found";
        updpnl.Update();
    }

    public bool ShouldApplySortFilterOrGroupACH()
    {
        return RadGrid_ACH.MasterTableView.FilterExpression != "" ||
            (RadGrid_ACH.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_ACH.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_CreditCard_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
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
        catch { }
    }
    protected void RadGrid_ACH_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
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
        catch { }
    }
    protected void RadGrid_ACH_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_ACH.AllowCustomPaging = !ShouldApplySortFilterOrGroupACH();
        FillPaymentHistory();
    }
    protected void RadGrid_ACH_ItemEvent(object sender, GridItemEventArgs e)
    {
        int rowCount = 0;
        if (e.EventInfo is GridInitializePagerItem)
        {
            rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
        }
        lblRecordCountAch.Text = rowCount + " Record(s) found";
        updpnl.Update();
    }
    protected void RadGrid_ACH_ItemCommand(object sender, GridCommandEventArgs e)
    {
        int Status = 1;
        if (Session["MSM"].ToString() == "TS")
            Status = 5;
        objProp_Contracts.Status = Status;
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        if (e.CommandName == "Approved")
        {
            string CommandArgument = e.CommandArgument.ToString();
            string[] slipstr = CommandArgument.Split(':');
            string PaymentUID = (string)(slipstr[0]);
            string Invoicestr = (string)slipstr[1];
            string[] InvoicestrArry = Invoicestr.Split(',');
            foreach (var item in InvoicestrArry)
            {
                objProp_Contracts.InvoiceID = Convert.ToInt32(item);
                objBL_Contracts.UpdateACHpaymentHistry(objProp_Contracts, "Approved", PaymentUID);
            }
            FillPaymentHistory();
            RadGrid_ACH.Rebind();
        }
        if (e.CommandName == "Declined")
        {
            string CommandArgument = e.CommandArgument.ToString();
            string[] slipstr = CommandArgument.Split(':');
            string PaymentUID = (string)(slipstr[0]);
            string Invoicestr = (string)slipstr[1];
            string[] InvoicestrArry = Invoicestr.Split(',');
            foreach (var item in InvoicestrArry)
            {
                objProp_Contracts.InvoiceID = Convert.ToInt32(item);
                objBL_Contracts.UpdateACHpaymentHistry(objProp_Contracts, "Declined", PaymentUID);
            }
            FillPaymentHistory();
            RadGrid_ACH.Rebind();
        }
       // FillPaymentHistory();
    }
   
}