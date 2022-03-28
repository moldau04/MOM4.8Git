using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using Telerik.Web.UI;
using Telerik.Web.UI.PersistenceFramework;
using System.Linq.Dynamic;
using Telerik.Web.UI.GridExcelBuilder;
using BusinessEntity.APModels;
using BusinessEntity.Utility;
using MOMWebApp;

public partial class RemitTax : System.Web.UI.Page
{
    #region "Variables"
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    BL_Wage objBL_Wage = new BL_Wage();
    Wage objProp_Wage = new Wage();
    PRDed objPRDed = new PRDed();
    public static bool check = false;
    public static bool IsAddEdit = false;
    public static bool IsDelete = false;
    BL_BankAccount _objBLBank = new BL_BankAccount();
    Vendor _objVendor = new Vendor();
    BL_Vendor _objBLVendor = new BL_Vendor();
    PJ _objPJ = new PJ();
    BL_Bills _objBLBills = new BL_Bills();
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!CheckAddEditPermission()) { Response.Redirect("Home.aspx?permission=no"); return; }
        
        if (!IsPostBack)
        {
            string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

            if (Request.Url.Scheme == "http" && SSL == "1")
            {
                string URL = Request.Url.ToString();
                URL = URL.Replace("http://", "https://");
                Response.Redirect(URL);
            }

            BindSearchFilters();
            FillVendor();
            SetBillForm();
            #region Show Selected Filter
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                //if (Session["ddlSearch_Vendor"] != null)
                //{
                //    String selectedValue = Convert.ToString(Session["ddlSearch_Vendor"]);
                //    ddlSearch.SelectedValue = selectedValue;

                //    String searchValue = Convert.ToString(Session["ddlSearch_Value_Vendor"]);
                //    if (selectedValue == "Rol.Type")
                //    {
                //        ddlType.SelectedValue = searchValue;
                //    }
                //    else if (selectedValue == "Vendor.Status")
                //    {
                //        ddlStatus.SelectedValue = searchValue;
                //    }
                //    {
                //        txtSearch.Text = searchValue;
                //    }

                //    SelectSearch();
                //}
            }
            else
            {
                Session["ddlSearch_Vendor"] = null;
                Session["ddlSearch_Value_Vendor"] = null;
            }
            #endregion
            FillFilter();

            Permission();
            HighlightSideMenu("prID", "remittax", "payrollmenutab");
        }
        CompanyPermission();
        //ConvertToJSON();
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

            /// Vendor ///////////////////------->

            string VendorPermission = ds.Rows[0]["Vendor"] == DBNull.Value ? "YYYY" : ds.Rows[0]["Vendor"].ToString();
            string ViewVendor = VendorPermission.Length < 4 ? "Y" : VendorPermission.Substring(3, 1);
            if (ViewVendor == "N")
            {
                result = false;
            }
        }

        return result;
    }

    

    private void Permission()
    {
       
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

        if (Session["type"].ToString() != "am")
        {
            DataTable dtUserPermission = new DataTable();
            dtUserPermission = GetUserById();
            /// AccountPayablemodulePermission ///////////////////------->

            string AccountPayablemodulePermission = dtUserPermission.Rows[0]["AccountPayablemodulePermission"] == DBNull.Value ? "Y" : dtUserPermission.Rows[0]["AccountPayablemodulePermission"].ToString();

            if (AccountPayablemodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
            DataTable ds = new DataTable();

           // ds = (DataTable)Session["userinfo"];

            //VendorsPermission
            string VendorsPermission = dtUserPermission.Rows[0]["Vendor"] == DBNull.Value ? "YYYY" : dtUserPermission.Rows[0]["Vendor"].ToString();

            hdnAddDedcutions.Value = VendorsPermission.Length < 1 ? "Y" : VendorsPermission.Substring(0, 1);
            hdnEditDedcutions.Value = VendorsPermission.Length < 2 ? "Y" : VendorsPermission.Substring(1, 1);
            hdnDeleteDedcutions.Value = VendorsPermission.Length < 3 ? "Y" : VendorsPermission.Substring(2, 1);
            hdnViewDedcutions.Value = VendorsPermission.Length < 4 ? "Y" : VendorsPermission.Substring(3, 1);
             if (hdnAddDedcutions.Value == "N")
            {

                lnksubmit.Visible = false;
                lnkQuickCheck.Visible = false;
            }
            //if (hdnEditDedcutions.Value == "N")
            //{
            //    btnEdit.Visible = false;               
            //}
            //if (hdnDeleteDedcutions.Value == "N")
            //{
            //    lnkDelete.Visible = false;

            //}
            if (hdnViewDedcutions.Value == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
            //if (hdnAddDedcutions.Value == "N")
            //{
            //    lnkAddnew.Enabled = false;
            //}
            //if (hdnEditDedcutions.Value == "N")
            //{
            //    btnEdit.Enabled = false;
            //}
            //if (hdnDeleteDedcutions.Value == "N")
            //{
            //    lnkDelete.Enabled = false;
            //}
        }
        else
        {
            hdnAddDedcutions.Value = "Y";
            hdnEditDedcutions.Value = "Y";
            hdnDeleteDedcutions.Value = "Y";
            hdnViewDedcutions.Value = "Y";
        }
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

        
            ds = objBL_User.GetUserPermissionByUserID(objPropUser);
        return ds.Tables[0];
    }
    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            //RadGrid_RemitTax.Columns[9].Visible = true;
        }
        else
        {
            //RadGrid_RemitTax.Columns[9].Visible = false;
            Session["CmpChkDefault"] = "2";
        }
    }

    private void BindSearchFilters()
    {
        //Dictionary<string, string> listsearchitems = new Dictionary<string, string>();
        //listsearchitems.Add("0", "Select");
        //listsearchitems.Add("Vendor.Acct", "Vendor ID");
        //listsearchitems.Add("Rol.Name", "Vendor name");
        //listsearchitems.Add("Rol.Address", "Address");
        //listsearchitems.Add("Rol.Contact", "Contact");
        //listsearchitems.Add("Rol.Phone", "Phone");
        //listsearchitems.Add("Rol.EMail", "Email");
        //listsearchitems.Add("Rol.Type", "Type");
        //listsearchitems.Add("Vendor.Status", "Status");


        //ddlSearch.DataSource = listsearchitems;
        //ddlSearch.DataTextField = "Value";
        //ddlSearch.DataValueField = "Key";
        //ddlSearch.DataBind();

    }

    // Start : Fill Type DropDownList : Juily 27-12-2019 

    

    

    
    protected void ddlSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectSearch();
    }
    private void SelectSearch()
    {
        //if (ddlSearch.SelectedValue == "Rol.Type")
        //{
        //    ddlType.Visible = true;
        //    ddlStatus.Visible = false;
        //    txtSearch.Visible = false;
        //    txtSearch.Text = "";            
        //}
        //else if (ddlSearch.SelectedValue == "Vendor.Status")
        //{
        //    ddlType.Visible = false;
        //    ddlStatus.Visible = true;
        //    txtSearch.Visible = false;
        //    txtSearch.Text = "";
        //    ddlType.SelectedIndex = 0;
        //}
        //else
        //{
        //    ddlType.Visible = false;
        //    ddlStatus.Visible = false;
        //    txtSearch.Visible = true;
        //    ddlType.SelectedIndex = 0;
            
        //}
        upPannelSearch.Update();
    }
    private void SaveFilter()
    {
        //Dictionary<string, string> dictFilter = new Dictionary<string, string>();
        //dictFilter["Search"] = ddlSearch.SelectedValue;
        //dictFilter["status"] = ddlStatus.SelectedValue;
        //dictFilter["type"] = ddlType.SelectedValue;
        //dictFilter["searchtxt"] = txtSearch.Text.Trim();
        //Session["FilterVendor"] = dictFilter;
    }
    private void FillFilter()
    {
        //if (Session["FilterVendor"] != null)
        //{
        //    if (Convert.ToString(Request.QueryString["f"]) != "c")
        //    {
        //        Dictionary<string, string> dictFilter = new Dictionary<string, string>();
        //        dictFilter = (Dictionary<string, string>)Session["FilterVendor"];
        //        ddlSearch.SelectedValue = dictFilter["Search"];
        //        SelectSearch();
        //        ddlStatus.SelectedValue = dictFilter["status"];
        //        ddlType.SelectedValue = dictFilter["type"];
        //        txtSearch.Text = dictFilter["searchtxt"];
        //    }
        //    else
        //    {
        //        Session["FilterVendor"] = null;
        //    }
        //}
    }
    
    
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        #region Search Filter
        //SelectSearch();
        //upPannelSearch.Update();
        //String selectedValue = ddlSearch.SelectedValue;
        //Session["ddlSearch_Vendor"] = selectedValue;

        //if (selectedValue == "Rol.Type")
        //{
        //    Session["ddlSearch_Value_Vendor"] = ddlType.SelectedValue;
        //}
        //else if (selectedValue == "Vendor.Status")
        //{
        //    Session["ddlSearch_Value_Vendor"] = ddlStatus.SelectedValue;
        //}
        //else
        //{
        //    Session["ddlSearch_Value_Vendor"] = txtSearch.Text;
        //}
        #endregion

        RadGrid_RemitTax.CurrentPageIndex = 0;
        RadGrid_RemitTax.PageSize = 50;
        GetWageDeductionList();
        RadGrid_RemitTax.Rebind();
    }
    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        //txtSearch.Text = string.Empty;
        //ddlSearch.SelectedIndex = 0;
        //ddlType.SelectedIndex = 0;
        //ddlStatus.SelectedIndex = 0;
        //SelectSearch();
        foreach (GridColumn column in RadGrid_RemitTax.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
            RadGrid_RemitTax.MasterTableView.SortExpressions.Clear();
        Session["Category_FilterExpression"] = null;
        Session["Category_Filters"] = null;
        RadGrid_RemitTax.MasterTableView.FilterExpression = "";
        RadGrid_RemitTax.CurrentPageIndex = 1;        
        upPannelSearch.Update();
        GetWageDeductionList();
        RadGrid_RemitTax.Rebind();
    }
    private void AddBill()
    {
        DataTable dt = GetTransaction();
        dt.Columns.Remove("AcctNo");
        dt.Columns.Remove("JobName");
        dt.Columns.Remove("UName");
        dt.Columns.Remove("Loc");
        dt.Columns.Remove("Line");
        dt.Columns.Remove("PrvInQuan");
        dt.Columns.Remove("PrvIn");
        dt.Columns.Remove("OutstandQuan");
        dt.Columns.Remove("OutstandBalance");
        dt.Columns.Remove("AmountTot");
        dt.Columns.Remove("Warehousefdesc");
        dt.Columns.Remove("Locationfdesc");
        dt.Select("JobID = 0")
              .AsEnumerable().ToList()
              .ForEach(t => t["JobID"] = DBNull.Value);
        dt.Select("ItemID = 0")
            .AsEnumerable().ToList()
            .ForEach(t => t["ItemID"] = DBNull.Value);

        dt.AcceptChanges();


        _objPJ.ConnConfig = Session["config"].ToString();
        _objPJ.Vendor = Convert.ToInt32(ddlVendor.SelectedValue.ToString());
        _objPJ.fDate = Convert.ToDateTime(txtHireDt.Text);
        _objPJ.PostDate = Convert.ToDateTime(txtHireDt.Text);
        //_objPJ.IDate = Convert.ToDateTime(txtDueDate.Text);
        _objPJ.Due = Convert.ToDateTime(txtHireDt.Text);
        _objPJ.Ref = txtRef.Text;
        _objPJ.fDesc = txtdesc.Text;
        _objPJ.Terms = 0;
        _objPJ.Spec = 5;
        _objPJ.IfPaid = 0;
        _objPJ.PO = 0;
        _objPJ.ReceivePo = 0;
        _objPJ.Disc = 0;
        _objPJ.MOMUSer = Session["User"].ToString();

        _objPJ.Dt = dt;

        _objPJ.STax = 0;
        _objPJ.STaxRate = 0;
        _objPJ.STaxGL = 0;
        _objPJ.STaxName = "";
        _objPJ.UTax = 0;
        _objPJ.UTaxName = "";
        _objPJ.UTaxRate = 0;
        _objPJ.UTaxGL = 0;
        _objPJ.GSTGL = 0;
        _objPJ.GSTRate = 0;
        _objPJ.GST = 0;
        _objPJ.IsPOClose = false;
        _objPJ.Status = 5;

        _objBLBills.AddRemitTaxBills(_objPJ);
    }
    private DataTable GetTransaction()
    {
        DataTable dt = GetTable();

        try
        {
            string strItems = hdnGLItem.Value.Trim();

            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                objEstimateItemData.RemoveAt(0);
                

                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    if (!dict.ContainsKey("chkSelect"))
                    {
                    //if (dict["chkSelect"].ToString().Trim() == string.Empty || dict["chkSelect"].ToString() == "0")
                    
                        continue;
                    }
                    
                    i++;
                    DataRow dr = dt.NewRow();
                    if (dict["hdnByW"].ToString().Trim() == "0")
                    {
                        dr["AcctID"] = dict["hdnCompGL"].ToString().Trim();
                    }
                    else if (dict["hdnByW"].ToString().Trim() == "1")
                    {
                        dr["AcctID"] = dict["hdnEmpGL"].ToString().Trim();
                    }
                    else 
                    {
                        dr["AcctID"] = dict["hdnCompGLE"].ToString().Trim();
                    }
                    dr["Quan"] = DBNull.Value;
                    dr["Ticket"] = "0";
                    dr["ID"] = DBNull.Value;
                    dr["fDesc"] = dict["hdnfdesc"].ToString().Trim();
                    dr["Amount"] = dict["txtPay"].ToString();
                    dr["Usetax"] = 0;
                    dr["UtaxName"] = DBNull.Value;
                    dr["UName"] = DBNull.Value;
                    dr["UtaxGL"] = 0;
                    dr["JobID"] = 0;
                    dr["JobName"] = dict["hdnfdesc"].ToString();
                    dr["PhaseID"] = dict["hdnDedID"].ToString();
                    dr["ItemID"] = DBNull.Value;

                    if (dict["hdnByW"].ToString().Trim() == "0")
                    {
                        dr["AcctNo"] = dict["hdnCompGLAcct"].ToString().Trim();
                    }
                    else if (dict["hdnByW"].ToString().Trim() == "1")
                    {
                        dr["AcctNo"] = dict["hdnEmpGLAcct"].ToString().Trim();
                    }
                    else
                    {
                        dr["AcctNo"] = dict["hdnCompGLEAcct"].ToString().Trim();
                    }
                    dr["Loc"] = DBNull.Value;
                    dr["Phase"] = DBNull.Value;
                    dr["TypeID"] = DBNull.Value;
                    dr["ItemDesc"] = dict["hdnfdesc"].ToString().Trim();
                    dr["OpSq"] = "100";
                    dr["Line"] = DBNull.Value;
                    dr["PrvInQuan"] = DBNull.Value;
                    dr["PrvIn"] = DBNull.Value;
                    dr["OutstandQuan"] = DBNull.Value;
                    dr["OutstandBalance"] = DBNull.Value;
                    dr["Warehouse"] = "";
                    dr["WHLocID"] = "0";
                    dr["stax"] = "0";
                    dr["STaxName"] = "";
                    dr["STaxRate"] = 0;
                    dr["STaxAmt"] = 0;
                    dr["STaxGL"] = 0;
                    dr["GSTRate"] = 0;
                    dr["GTaxAmt"] = 0;
                    dr["GSTTaxGL"] = 0;
                    dr["IsPO"] = 1;
                    dr["GTax"] = "0";
                    dr["Price"] = DBNull.Value;
                    //if (dict["hdnAcctID"].ToString().Trim() == string.Empty || dict["hdnAcctID"].ToString() == "0")
                    //{
                    //    continue;
                    //}
                    //i++;
                    //DataRow dr = dt.NewRow();

                    dt.Rows.Add(dr);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }
    public DataTable GetTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("AcctID", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Amount", typeof(double));
        dt.Columns.Add("Usetax", typeof(string));
        dt.Columns.Add("UtaxName", typeof(string));
        dt.Columns.Add("JobID", typeof(int));
        dt.Columns.Add("PhaseID", typeof(int));
        dt.Columns.Add("ItemID", typeof(int));

        dt.Columns.Add("AcctNo", typeof(string));
        dt.Columns.Add("JobName", typeof(string));
        dt.Columns.Add("Phase", typeof(string));
        dt.Columns.Add("UName", typeof(string));
        dt.Columns.Add("UtaxGL", typeof(Int32));
        dt.Columns.Add("ItemDesc", typeof(string));
        dt.Columns.Add("TypeID", typeof(Int32));
        dt.Columns.Add("Loc", typeof(string));
        dt.Columns.Add("TypeDesc", typeof(string));
        dt.Columns.Add("Quan", typeof(double));
        dt.Columns.Add("Ticket", typeof(Int32));
        dt.Columns.Add("OpSq", typeof(string));

        dt.Columns.Add("Warehouse", typeof(string));
        dt.Columns.Add("WHLocID", typeof(Int32));

        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("PrvInQuan", typeof(double));
        dt.Columns.Add("PrvIn", typeof(double));
        dt.Columns.Add("OutstandQuan", typeof(double));
        dt.Columns.Add("OutstandBalance", typeof(double));

        dt.Columns.Add("STax", typeof(int));
        dt.Columns.Add("STaxName", typeof(string));
        dt.Columns.Add("STaxRate", typeof(double));
        dt.Columns.Add("StaxAmt", typeof(double));
        dt.Columns.Add("STaxGL", typeof(Int32));
        dt.Columns.Add("GSTRate", typeof(double));
        dt.Columns.Add("GTaxAmt", typeof(double));
        dt.Columns.Add("GSTTaxGL", typeof(Int32));
        dt.Columns.Add("AmountTot", typeof(double));


        dt.Columns.Add("Warehousefdesc", typeof(string));
        dt.Columns.Add("Locationfdesc", typeof(string));
        dt.Columns.Add("IsPO", typeof(int));
        dt.Columns.Add("GTax", typeof(int));
        dt.Columns.Add("Price", typeof(double));

        return dt;
    }
    private void SetBillForm()
    {
        //FillVendor();
        txtHireDt.Text = DateTime.Now.ToString("MM/dd/yyyy");
        ddlVendor.SelectedIndex = 0;
        txtRef.Text = "0";
        txtyear.Text = DateTime.Now.ToString("yyyy");
        txtdesc.Text = "Payroll Withholding & Contribution Remittance";
        if (DateTime.Now.Month == 1 || DateTime.Now.Month == 2 || DateTime.Now.Month == 3)
        {
            txtquarter.Text = "4";
        }
        else if (DateTime.Now.Month == 4 || DateTime.Now.Month == 5 || DateTime.Now.Month == 6)
        {
            txtquarter.Text = "1";
        }
        else if (DateTime.Now.Month == 7 || DateTime.Now.Month == 8 || DateTime.Now.Month == 9)
        {
            txtquarter.Text = "2";
        }
        else
        {
            txtquarter.Text = "3";
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
    protected void lnksubmit_Click(object sender, EventArgs e)
    {
        AddBill();
        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Bill created successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        ResetFormControlValues(this);
        SetBillForm();
        RadGrid_RemitTax.Rebind();
    }
    protected void btnQuickCheck_Click(object sender, EventArgs e)
    {
        AddBill();
        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Bill created successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true}); window.setTimeout(function() { window.location.href = 'WriteChecks.aspx?bill=c&vid=" + _objPJ.Vendor + "&ref=" + _objPJ.Ref + "'; }, 500); ", true);
        //ResetFormControlValues(this);
        //SetBillForm();
        //RadGrid_RemitTax.Rebind();
        
    }
    
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        

        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_RemitTax.SelectedItems)
            {
                IsDelete = true;
                TableCell cell = di["chkSelect"];
                CheckBox chkSelect = (CheckBox)cell.Controls[0];
                Label lblId = (Label)di.FindControl("lblId");
                HyperLink lblWageFdesc = (HyperLink)di.FindControl("lblWageFdesc");
                if (chkSelect.Checked == true)
                {
                    objProp_Wage.ConnConfig = Session["config"].ToString();
                    objProp_Wage.ID = Convert.ToInt32(lblId.Text);
                    objBL_Wage.DeleteWageByID(objProp_Wage);
                    GetWageDeductionList();
                    RadGrid_RemitTax.Rebind();
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddCusttype", "noty({text: 'Wage Category " + lblWageFdesc.Text + " Deleted Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            if (!IsDelete)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarningServyp", "noty({text: 'Please select Wage Category to Delete!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelServyp", "noty({text: '" + str + "',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);

        }

    }
    
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }
    protected void lnkchk_Click(object sender, EventArgs e)
    {
        if (lnkChk.Checked)
        {
            check = true;
            GetWageDeductionList();
            RadGrid_RemitTax.Rebind();
        }
        else
        {
            check = false;
            GetWageDeductionList();
            RadGrid_RemitTax.Rebind();
        }
    }
    private void GetWageDeductionList()
    {
        try
        {

            DataSet ds = new DataSet();
            objPRDed.ConnConfig = Session["config"].ToString();

            ds = new BL_Wage().GetWageDeduction(objPRDed);

            DataTable filterdt = new DataTable();
            DataSet FilteredDs = new DataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow[] dr = ds.Tables[0].Select("Paid=1");
                if (dr.Length > 0)
                {
                    filterdt = dr.CopyToDataTable();
                    FilteredDs.Tables.Add(filterdt);
                }
                else
                {
                    FilteredDs = ds.Clone();
                }
            }
            if (FilteredDs.Tables.Count > 0)
            {
                RadGrid_RemitTax.VirtualItemCount = FilteredDs.Tables[0].Rows.Count;
                RadGrid_RemitTax.DataSource = FilteredDs.Tables[0];
                ViewState["VirtualItemCount"] = FilteredDs.Tables[0].Rows.Count;
                //lblRecordCount.Text = ds.Tables[0].Rows.Count + " Record(s) found";
                Session["RemitTaxList"] = FilteredDs.Tables[0];
                SaveFilter();
            }


           
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private DataTable GetFilteredDataSource()
    {
        DataTable DT = new DataTable();
        DataTable FilteredDT = new DataTable();
        string filterexpression = string.Empty;
        filterexpression = RadGrid_RemitTax.MasterTableView.FilterExpression;
        if (filterexpression != "")
        {
            DT = (DataTable)RadGrid_RemitTax.DataSource;
            FilteredDT = DT.AsEnumerable()
            .AsQueryable()
            .Where(filterexpression)
            .CopyToDataTable();
            return FilteredDT;
        }
        else
        {
            return (DataTable)RadGrid_RemitTax.DataSource;
        }

    }
    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_RemitTax.MasterTableView.FilterExpression != "" ||
            (RadGrid_RemitTax.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_RemitTax.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_RemitTax_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_RemitTax.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        #region Set the Grid Filters
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["Category_FilterExpression"] != null && Convert.ToString(Session["Category_FilterExpression"]) != "" && Session["Category_Filters"] != null)
                {
                    RadGrid_RemitTax.MasterTableView.FilterExpression = Convert.ToString(Session["Category_FilterExpression"]);
                    var filtersGet = Session["Category_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_RemitTax.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                            column.CurrentFilterValue = _filter.FilterValue;
                        }
                    }
                }
            }
            else
            {
                Session["Category_FilterExpression"] = null;
                Session["Category_Filters"] = null;
                //Session["Vendor_VirtulItemCount"] = null;
            }
            //if (Request.QueryString["AddVendor"] != null)
            //{
            //    if (Convert.ToString(Request.QueryString["AddVendor"]) == "Y")
            //    {
            //        if (check == true)
            //        {
            //            lnkChk.Checked = true;
            //        }
            //        else
            //        {
            //            lnkChk.Checked = false;
            //        }
            //    }
            //}
        }
        
        #endregion
        GetWageDeductionList();
    }
    protected void RadGrid_RemitTax_ItemEvent(object sender, GridItemEventArgs e)
    {
        int rowCount = 0;
        if (e.EventInfo is GridInitializePagerItem)
        {
            rowCount = (e.EventInfo as GridInitializePagerItem).PagingManager.DataSourceCount;
        }
        //if (Session["vendors"] != null)
        //{
        //    DataTable dtID = (DataTable)Session["vendors"];
        //    lblRecordCount.Text = dtID.Rows.Count + " Record(s) found";
        //    updpnl.Update();
        //}
        rowCount = Convert.ToInt32(ViewState["VirtualItemCount"]);
        //lblRecordCount.Text = rowCount + " Record(s) found";
        //updpnl.Update();
    }
    private void RowSelect()
    {
        foreach (GridDataItem gr in RadGrid_RemitTax.Items)
        {
            Label lblID = (Label)gr.FindControl("lblId");
            HyperLink lnkName = (HyperLink)gr.FindControl("lblWageFdesc");
            //lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='WageCategory.aspx?id=" + lblID.Text + "'";
        }
    }
    protected void RadGrid_RemitTax_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_RemitTax.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Category_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_RemitTax.MasterTableView.OwnerGrid.Columns)
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

            Session["Category_Filters"] = filters;
            //Session["Vendor_VirtulItemCount"] = RadGrid_RemitTax.VirtualItemCount;
        }
        else
        {
            Session["Category_FilterExpression"] = null;
            Session["Category_Filters"] = null;
            //Session["Vendor_VirtulItemCount"] = null;
        }
        #endregion  
        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_RemitTax);
        RowSelect();

        
    }

    protected void lnkClear_Click(object sender, EventArgs e)
    {
        

        //ddlType.Visible = false;
        //ddlStatus.Visible = false;
        //txtSearch.Visible = true;
        ResetFormControlValues(this);
        check = false;
        lnkChk.Checked = false;        
        
        foreach (GridColumn column in RadGrid_RemitTax.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        Session["Category_FilterExpression"] = null;
        Session["Category_Filters"] = null;
        //Session["Vendor_VirtulItemCount"] = null;
        RadGrid_RemitTax.MasterTableView.FilterExpression = "";
        //RadGrid_RemitTax.MasterTableView.FilterExpression = string.Empty;
        //RadGrid_RemitTax.PageSize = 50;
        ////lnkSearch_Click(sender, e);
        //Page_Load(sender, e);
        ////GetWageDeductionList();
        ////RadGrid_RemitTax.Rebind();
        //Response.Redirect("vendors.aspx?f=c");
        
    }

   

    protected void RadGrid_RemitTax_ItemCreated(object sender, GridItemEventArgs e)
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
    
    protected void RadGrid_RemitTax_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 4;
        else
            currentItem = 5;
        if (e.Worksheet.Table.Rows.Count == RadGrid_RemitTax.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_RemitTax.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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
    private void FillVendor()
    {
        try
        {
            if (Session["COPer"].ToString() == "1")
            {
                //Do nothing
            }
            else
            {
                _objVendor.ConnConfig = Session["config"].ToString();
                
                DataSet _dsVendor = new DataSet();

                
                    _dsVendor = _objBLVendor.GetAllVendors(_objVendor);
                

                if (ddlVendor.Items.Count > 0)
                {
                    ddlVendor.Items.Clear();
                }
                //ddlVendor.Items.Add(new ListItem(" "));
                if (_dsVendor.Tables[0].Rows.Count > 0)
                {
                    
                    ddlVendor.AppendDataBoundItems = true;

                    DataView dataviw = _dsVendor.Tables[0].DefaultView;
                    dataviw.Sort = "Name";
                    DataTable _dsVendordt = dataviw.ToTable();

                    ddlVendor.DataSource = _dsVendordt;
                    ddlVendor.DataValueField = "ID";
                    ddlVendor.DataTextField = "Name";
                    ddlVendor.DataBind();
                }
                else
                {
                    ddlVendor.Items.Add(new System.Web.UI.WebControls.ListItem("No data found", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

}

//public static class Test
//{
//    public static DataSet ToDataSet<T>(this IList<T> list)
//    {
//        Type elementType = typeof(T);
//        DataSet ds = new DataSet();
//        DataTable t = new DataTable();
//        ds.Tables.Add(t);

//        //add a column to table for each public property on T
//        foreach (var propInfo in elementType.GetProperties())
//        {
//            Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

//            t.Columns.Add(propInfo.Name, ColType);
//        }

//        //go through each property on T and add each value to the table
//        foreach (T item in list)
//        {
//            DataRow row = t.NewRow();

//            foreach (var propInfo in elementType.GetProperties())
//            {
//                row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;
//            }

//            t.Rows.Add(row);
//        }

//        return ds;
//    }
//}