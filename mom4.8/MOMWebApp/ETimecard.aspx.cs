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
using Stimulsoft.Report;
using Microsoft.Reporting.WebForms;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

public partial class ETimecard : System.Web.UI.Page
{
    #region "Variables"
    CD _objCD = new CD();
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    BL_Wage objBL_Wage = new BL_Wage();
    Wage objProp_Wage = new Wage();
    PRReg _objPRReg = new PRReg();
    PRDed objPRDed = new PRDed();
    public static bool check = false;
    public static bool IsAddEdit = false;
    public static bool IsDelete = false;
    BL_BankAccount _objBLBank = new BL_BankAccount();
    
    User objPropUser = new User();
    Emp _objEmp = new Emp();
    Vendor _objVendor = new Vendor();
    BL_Vendor _objBLVendor = new BL_Vendor();
    Bank _objBank = new Bank();
    BL_BankAccount _objBL_Bank = new BL_BankAccount();
    protected DataTable dti = new DataTable();
    protected DataTable dtpay = new DataTable();
    protected DataTable dtBank = new DataTable();
    BL_Bills _objBLBill = new BL_Bills();
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
            BindSearchFilters();
            FillSupervisor();
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
            FillBank();
            Permission();
            HighlightSideMenu("prID", "ETimecardlink", "payrollmenutab");
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
            //RadGrid_RunPayroll.Columns[9].Visible = true;
        }
        else
        {
            //RadGrid_RunPayroll.Columns[9].Visible = false;
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

        RadGrid_RunPayroll.CurrentPageIndex = 0;
        RadGrid_RunPayroll.PageSize = 50;
        GetEmpList();
        RadGrid_RunPayroll.Rebind();
    }
    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
        //txtSearch.Text = string.Empty;
        //ddlSearch.SelectedIndex = 0;
        //ddlType.SelectedIndex = 0;
        //ddlStatus.SelectedIndex = 0;
        //SelectSearch();
        foreach (GridColumn column in RadGrid_RunPayroll.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
            RadGrid_RunPayroll.MasterTableView.SortExpressions.Clear();
        Session["Category_FilterExpression"] = null;
        Session["Category_Filters"] = null;
        RadGrid_RunPayroll.MasterTableView.FilterExpression = "";
        RadGrid_RunPayroll.CurrentPageIndex = 1;        
        upPannelSearch.Update();
        GetEmpList();
        RadGrid_RunPayroll.Rebind();
    }
    private double ConvertCurrentCurrencyFormatToDbl(string strCurrency)
    {
        if (!string.IsNullOrEmpty(strCurrency))
        {
            var dblReturn = double.Parse(strCurrency.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                        NumberStyles.AllowThousands |
                                                        NumberStyles.AllowDecimalPoint | NumberStyles.AllowTrailingSign |
                                                        NumberStyles.Float);
            return dblReturn;
        }
        else
        {
            return 0;
        }
    }
    private string GetWorkGeoCode()
    {
        string workgeo = "";
        string code = "";
        //////////////////////////////////////////
        //Credentials for Payroll OnDemand
        string username = "dread@1000";
        string passWord = "K3CHccxQ";
        // Setting up the URI for Payroll
        string uri = "https://payrollsandbox.ondemand.vertexinc.com:443/EiWebSvc/AddressWebService";

        DataSet dswork = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        dswork = objBL_User.getControl(objPropUser);
        DataRow _dr = dswork.Tables[0].Rows[0];

        string addrClnXML = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:eic = \"http://EiCalc/\">"
            + "<soapenv:Header/>"
            + "<soapenv:Body>"
            + "<eic:AddrCleanse>"
            + "<Request>"
            + "<![CDATA["
                + "<ADDRESS_CLEANSE_REQUEST>"
                    + "<StreetAddress1>" + _dr["Address"].ToString() + "</StreetAddress1>"
                    + "<CityName>" + _dr["City"].ToString() + "</CityName>"
                    + "<StateName>" + _dr["state"].ToString() + "</StateName>"
                    + "<ZipCode>" + _dr["zip"].ToString() + "</ZipCode>"
                + "</ADDRESS_CLEANSE_REQUEST>]]>"
            + "</Request>"
            + "</eic:AddrCleanse>"
            + "</soapenv:Body>"
            + "</soapenv:Envelope>";

        try
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            HttpClient cl = new HttpClient();
            cl.BaseAddress = new Uri(uri);
            cl.DefaultRequestHeaders.Clear();
            cl.DefaultRequestHeaders.Add("javax.xml.ws.security.auth.username", username);
            cl.DefaultRequestHeaders.Add("javax.xml.ws.security.auth.password", passWord);
            HttpContent soapAddressEnvelope = new StringContent(addrClnXML);
            soapAddressEnvelope.Headers.Clear();
            soapAddressEnvelope.Headers.Add("Content-Type", "text/xml");
            using (HttpResponseMessage response = cl.PostAsync(uri, soapAddressEnvelope).Result)
            {
                string rawString = getMessage(response).Result;
                
                XmlDocument responseXML = new XmlDocument();
                responseXML.LoadXml(rawString);
                XDocument responseXMLPretty = XDocument.Parse(responseXML.InnerText.ToString());

                string responseXMLPrettystr = responseXMLPretty.ToString();
                responseXMLPrettystr = responseXMLPrettystr.Replace("\"", "'");
                XmlDocument Doc = new XmlDocument();
                Doc.LoadXml(responseXMLPrettystr);
                XmlNode nodedoc = Doc.GetElementsByTagName("GeoCode").Item(0);
                workgeo = nodedoc.ChildNodes.Item(0).InnerText;

                ////Console.WriteLine(responseXMLPretty.ToString());
                //XmlNode node = responseXML.GetElementsByTagName("GeoCode").Item(0);
                //workgeo = node.ChildNodes.Item(0).InnerText;

                soapAddressEnvelope.Dispose();
                cl.Dispose();
            }
        }

        catch (AggregateException aggEx)
        {
            //If an error occurs outside of the service call capture and show below
            Console.WriteLine("A Connection error occurred");
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("Error Code: " + aggEx.Message.ToString() + Environment.NewLine + "Message: " + aggEx.InnerException.Message.ToString());

        }


        //Console.ReadLine();



        // Below call to read the message back from server from stream to string
        async Task<string> getMessage(HttpResponseMessage messageFromServer)
        {
            code = await messageFromServer.Content.ReadAsStringAsync();
            return code;
        }

        //////////////////////////////////////////
        return workgeo;
    }
    //private DataTable GetTransaction()
    //{
        
    //}
    public DataTable GetTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Name", typeof(string));
        dt.Columns.Add("Reg", typeof(double));
        dt.Columns.Add("OT", typeof(double));
        dt.Columns.Add("DT", typeof(double));
        dt.Columns.Add("TT", typeof(double));
        dt.Columns.Add("NT", typeof(double));
        dt.Columns.Add("Zone", typeof(double));
        dt.Columns.Add("Milage", typeof(double));
        dt.Columns.Add("Toll", typeof(double));
        dt.Columns.Add("OtherE", typeof(double));
        dt.Columns.Add("pay", typeof(double));
        dt.Columns.Add("holiday", typeof(double));
        dt.Columns.Add("vacation", typeof(double));
        dt.Columns.Add("sicktime", typeof(double));
        dt.Columns.Add("reimb", typeof(double));
        dt.Columns.Add("bonus", typeof(double));
        dt.Columns.Add("paymethod", typeof(string));
        dt.Columns.Add("pmethod", typeof(int));
        dt.Columns.Add("userid", typeof(string));
        dt.Columns.Add("usertype", typeof(string));
        dt.Columns.Add("total", typeof(double));
        dt.Columns.Add("phour", typeof(double));
        dt.Columns.Add("salary", typeof(double));
        dt.Columns.Add("HourlyRate", typeof(double));
        dt.Columns.Add("FIT", typeof(double));
        dt.Columns.Add("SIT", typeof(double));
        dt.Columns.Add("LOCAL", typeof(double));
        dt.Columns.Add("MEDI", typeof(double));
        dt.Columns.Add("FICA", typeof(double));

        return dt;
    }
    private void SetBillForm()
    {        
        txtcheckdate.Text = DateTime.Now.ToString();
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
        //AddBill();
        //ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Bill created successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        //ResetFormControlValues(this);
        //SetBillForm();
        //RadGrid_RunPayroll.Rebind();
    }
    protected void btnQuickCheck_Click(object sender, EventArgs e)
    {
        //AddBill();
        //ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Bill created successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true}); window.setTimeout(function() { window.location.href = 'WriteChecks.aspx?bill=c&vid=" + _objPJ.Vendor + "&ref=" + _objPJ.Ref + "'; }, 500); ", true);
       
        
    }
    protected void lnkProcessPayroll_Click(object sender, EventArgs e)
    {
        //AddBill();
        //ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Bill created successfully! <BR/> <b> Bill ref# : " + _objPJ.Ref + " </b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true}); window.setTimeout(function() { window.location.href = 'WriteChecks.aspx?bill=c&vid=" + _objPJ.Vendor + "&ref=" + _objPJ.Ref + "'; }, 500); ", true);


    }
    protected void btnGetDetail_Click(object sender, EventArgs e)
    {
        int shdnEmpIDint = 0;
        if (shdnEmpID.Value != "")
        {
            shdnEmpIDint = Convert.ToInt32(shdnEmpID.Value);
        }
        GetHourList(shdnEmpIDint);
        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "CallMyFunction", "OpPayrollDetailModal()", true);
        //string script = "function f(){$find(\"" + PayrollDetail.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "keyedit", script, true);

    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        

        IsDelete = false;
        try
        {
            foreach (GridDataItem di in RadGrid_RunPayroll.SelectedItems)
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
                    GetEmpList();
                    RadGrid_RunPayroll.Rebind();
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
            GetEmpList();
            RadGrid_RunPayroll.Rebind();
        }
        else
        {
            check = false;
            GetEmpList();
            RadGrid_RunPayroll.Rebind();
        }
    }
    private void GetEmpList()
    {
        try
        {

            DataSet ds = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            //objPropUser.FStart = Convert.ToDateTime(txtstartDt.Text);
            //objPropUser.Edate = Convert.ToDateTime(txtendDt.Text);
            objPropUser.Supervisor = ddlSuper.SelectedValue.ToString();
            objPropUser.DepartmentID = 0;
            objPropUser.EN = 0;            
            objPropUser.ID = 0;
            objPropUser.WorkId = 0;
            

            ds = new BL_Wage().GetTimeCardInput(objPropUser);

            //DataTable filterdt = new DataTable();
            //DataSet FilteredDs = new DataSet();
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    DataRow[] dr = ds.Tables[0].Select("Paid=1");
            //    if (dr.Length > 0)
            //    {
            //        filterdt = dr.CopyToDataTable();
            //        FilteredDs.Tables.Add(filterdt);
            //    }
            //    else
            //    {
            //        FilteredDs = ds.Clone();
            //    }
            //}

            RadGrid_RunPayroll.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGrid_RunPayroll.DataSource = ds.Tables[0];
            ViewState["VirtualItemCount"] = ds.Tables[0].Rows.Count;
            //lblRecordCount.Text = ds.Tables[0].Rows.Count + " Record(s) found";
            Session["RunPayrollList"] = ds.Tables[0];
            SaveFilter();

           
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
        filterexpression = RadGrid_RunPayroll.MasterTableView.FilterExpression;
        if (filterexpression != "")
        {
            DT = (DataTable)RadGrid_RunPayroll.DataSource;
            FilteredDT = DT.AsEnumerable()
            .AsQueryable()
            .Where(filterexpression)
            .CopyToDataTable();
            return FilteredDT;
        }
        else
        {
            return (DataTable)RadGrid_RunPayroll.DataSource;
        }

    }
    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_RunPayroll.MasterTableView.FilterExpression != "" ||
            (RadGrid_RunPayroll.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_RunPayroll.MasterTableView.SortExpressions.Count > 0;
    }
    protected void RadGrid_RunPayroll_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_RunPayroll.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        #region Set the Grid Filters
        if (!IsPostBack)
        {
            if (Convert.ToString(Request.QueryString["f"]) != "c")
            {
                if (Session["Category_FilterExpression"] != null && Convert.ToString(Session["Category_FilterExpression"]) != "" && Session["Category_Filters"] != null)
                {
                    RadGrid_RunPayroll.MasterTableView.FilterExpression = Convert.ToString(Session["Category_FilterExpression"]);
                    var filtersGet = Session["Category_Filters"] as List<RetainFilter>;
                    if (filtersGet != null)
                    {
                        foreach (var _filter in filtersGet)
                        {
                            GridColumn column = RadGrid_RunPayroll.MasterTableView.GetColumnSafe(_filter.FilterColumn);
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
        GetEmpList();
    }
    protected void RadGrid_RunPayroll_ItemEvent(object sender, GridItemEventArgs e)
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
        foreach (GridDataItem gr in RadGrid_RunPayroll.Items)
        {
            HiddenField hdnid = (HiddenField)gr.FindControl("hdnid");
            HyperLink lblName = (HyperLink)gr.FindControl("lblName");
            lblName.Attributes["onclick"] = "OpenPayrollDetailModal('"+ hdnid .Value+ "');";
            //lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='WageCategory.aspx?id=" + lblID.Text + "'";
        }
    }
    protected void RadGrid_RunPayroll_PreRender(object sender, EventArgs e)
    {
        #region Save the Grid Filter
        String filterExpression = Convert.ToString(RadGrid_RunPayroll.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Category_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_RunPayroll.MasterTableView.OwnerGrid.Columns)
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
            //Session["Vendor_VirtulItemCount"] = RadGrid_RunPayroll.VirtualItemCount;
        }
        else
        {
            Session["Category_FilterExpression"] = null;
            Session["Category_Filters"] = null;
            //Session["Vendor_VirtulItemCount"] = null;
        }
        #endregion  
        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_RunPayroll);
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
        
        foreach (GridColumn column in RadGrid_RunPayroll.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        Session["Category_FilterExpression"] = null;
        Session["Category_Filters"] = null;
        //Session["Vendor_VirtulItemCount"] = null;
        RadGrid_RunPayroll.MasterTableView.FilterExpression = "";
        //RadGrid_RunPayroll.MasterTableView.FilterExpression = string.Empty;
        //RadGrid_RunPayroll.PageSize = 50;
        ////lnkSearch_Click(sender, e);
        //Page_Load(sender, e);
        ////GetEmpList();
        ////RadGrid_RunPayroll.Rebind();
        //Response.Redirect("vendors.aspx?f=c");
        
    }

   

    protected void RadGrid_RunPayroll_ItemCreated(object sender, GridItemEventArgs e)
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
    
    protected void RadGrid_RunPayroll_ExcelMLExportRowCreated(object source, GridExportExcelMLRowCreatedArgs e)
    {
        int currentItem = 0;
        if (Convert.ToString(Session["CmpChkDefault"]) == "1")
            currentItem = 4;
        else
            currentItem = 5;
        if (e.Worksheet.Table.Rows.Count == RadGrid_RunPayroll.Items.Count + 1)
        {
            GridFooterItem footerItem = RadGrid_RunPayroll.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;
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
   

    private void FillSupervisor()
    {
        try
        {
            DataSet ds = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            ds = objBL_User.getSupervisor(objPropUser);

            if (ddlSuper.Items.Count > 0)
            {
                ddlSuper.Items.Clear();
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlSuper.AppendDataBoundItems = true;
                DataView dataviw = ds.Tables[0].DefaultView;
                dataviw.Sort = "fuser";
                DataTable _dsSupervisorDt = dataviw.ToTable();

                ddlSuper.DataSource = _dsSupervisorDt;
                ddlSuper.DataTextField = "fuser";
                ddlSuper.DataValueField = "fuser";
                ddlSuper.DataBind();

                ddlSuper.Items.Insert(0, new ListItem("-- All --", ""));
            }
            else
            {
                ddlSuper.Items.Add(new System.Web.UI.WebControls.ListItem("No data found", "0"));
            }
            
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void GetHourList(int EmpID)
    {
        try
        {

            DataSet ds = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            //objPropUser.FStart = Convert.ToDateTime(txtstartDt.Text);
            //objPropUser.Edate = Convert.ToDateTime(txtendDt.Text);
            //objPropUser.Supervisor = ddlSuper.SelectedValue.ToString();
            objPropUser.DepartmentID = 0;
            objPropUser.EN = 0;
            objPropUser.ID = EmpID;
            objPropUser.WorkId = 0;


            ds = new BL_Wage().GetPayrollHour(objPropUser);

            //DataTable filterdt = new DataTable();
            //DataSet FilteredDs = new DataSet();
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    DataRow[] dr = ds.Tables[0].Select("Paid=1");
            //    if (dr.Length > 0)
            //    {
            //        filterdt = dr.CopyToDataTable();
            //        FilteredDs.Tables.Add(filterdt);
            //    }
            //    else
            //    {
            //        FilteredDs = ds.Clone();
            //    }
            //}

            //RadGridPayrollHours.VirtualItemCount = ds.Tables[0].Rows.Count;
            //RadGridPayrollHours.DataSource = ds.Tables[0];
            //RadGridPayrollHours.DataBind();
            ViewState["VirtualItemCountHour"] = ds.Tables[0].Rows.Count;
            
            Session["PayrollHourList"] = ds.Tables[0];
            SaveFilter();


        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void RadGridPayrollHours_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int shdnEmpIDint = 0;
        if (shdnEmpID.Value != "")
        {
            shdnEmpIDint = Convert.ToInt32( shdnEmpID.Value);
        }
        //GetHourList(shdnEmpIDint);
        
    }
    protected void ddlApTopCheckForLoad_SelectedIndexChanged(object sender, EventArgs e)
    {


    }
    List<byte[]> lstbyte = new List<byte[]>();
    List<byte[]> lstbyteNew = new List<byte[]>();
    protected void imgPrintTemp1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            byte[] buffer1 = null;
            if (ddlApTopCheckForLoad.SelectedItem.Text.Trim() != null)
            {

                string reportApTopCheckPathStimul = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + ddlApTopCheckForLoad.SelectedItem.Text.Trim() + ".mrt");
                //  string reportPathStimul = Server.MapPath("StimulsoftReports/APTopCheck.mrt");

                StiReport report = new StiReport();

                FillReportApTopCheckDataSet(ddlApTopCheckForLoad.SelectedItem.Text.Trim());
                //ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "document.getElementById('loaders').style.display = 'block';", true);
                //report = FillDataSetToReport(ddlApTopCheckForLoad.SelectedItem.Text.Trim());
                //var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                //var service = new Stimulsoft.Report.Export.StiPdfExportService();
                //System.IO.MemoryStream stream = new System.IO.MemoryStream();
                //service.ExportTo(report, stream, settings);
                //buffer1 = stream.ToArray();

                //string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF", "APCheckTop.pdf");

                //if (buffer1 != null)
                //{
                //    if (File.Exists(filename))
                //        File.Delete(filename);
                //    using (var fs = new FileStream(filename, FileMode.Create))
                //    {
                //        fs.Write(buffer1, 0, buffer1.Length);
                //        fs.Close();
                //    }
                //}

                ////END


                ////rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsInvoices", _dti));
                ////rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsCheck", _dtCheck));

                ////rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsTicket", dsC.Tables[0]));
                ////string reportPath = "Reports/ReportCheck.rdlc";

                ////rvChecks.LocalReport.ReportPath = reportPath;

                ////rvChecks.LocalReport.EnableExternalImages = true;
                ////List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
                ////string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
                ////param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));

                ////rvChecks.LocalReport.SetParameters(param1);

                ////rvChecks.LocalReport.Refresh();

                ////byte[] buffer = null;
                ////buffer = ExportReportToPDF("", rvChecks);
                //Response.ClearContent();
                //Response.ClearHeaders();
                //Response.AddHeader("Content-Disposition", "attachment;filename=PrintCheck.pdf");
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("Content-Length", (buffer1.Length).ToString());
                //Response.BinaryWrite(buffer1);
                //Response.Flush();
                //Response.Close();
            }
        }



        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void ImageButton7_Click(object sender, ImageClickEventArgs e)
    {
        //mpeTemplate.Hide();
        string reportName = ddlApTopCheckForLoad.SelectedItem.Text.Trim();
        StiReport report = FillDataSetToReport(reportName);
        StiWebDesigner1.Report = report;
        //ReportModalPopupExtender.Show();
        StiWebDesigner1.Visible = true;

        Session["wc_first"] = "true";

        string script = "function f(){$find(\"" + RadWindowFirstReport.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

    }
    protected void StiWebDesigner1_SaveReport(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {

        //ReportModalPopupExtender.Hide();
        Session["wc_first"] = null;
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + e.FileName));

    }

    protected void StiWebDesigner2_SaveReport(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {

        // ReportModalPopupExtender1.Hide();
        Session["wc_second"] = null;
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + e.FileName));

    }

    protected void StiWebDesigner3_SaveReport(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {

        // ReportModalPopupExtender2.Hide();
        Session["wc_third"] = null;
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + e.FileName));

    }
    protected void StiWebDesigner1_SaveReportAs(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + e.FileName));
    }
    protected void StiWebDesigner1_Exit(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        Response.Redirect("ManageChecks.aspx");
    }

    protected void StiWebDesigner2_SaveReportAs(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + e.FileName));
    }

    protected void StiWebDesigner3_SaveReportAs(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + e.FileName));
    }

    protected void StiWebDesigner2_Exit(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        Response.Redirect("ManageChecks.aspx");

    }

    protected void StiWebDesigner3_Exit(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        Response.Redirect("ManageChecks.aspx");
    }

    private StiReport FillDataSetToReport(string reportName)
    {

        double SumAmountpay = 0.00;
        DataTable _dti = new DataTable();
        DataRow _dri = null;
        _dti.Columns.Add(new DataColumn("Ref", typeof(string)));
        _dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("Reference", typeof(string)));
        _dti.Columns.Add(new DataColumn("Total", typeof(double)));
        _dti.Columns.Add(new DataColumn("Disc", typeof(double)));
        _dti.Columns.Add(new DataColumn("AmountPay", typeof(double)));
        _dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("VendorName", typeof(string)));

        //RAHIL
        _dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("Description", typeof(string)));

        //New column
        _dti.Columns.Add(new DataColumn("State", typeof(string)));
        _dti.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dti.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dti.Columns.Add(new DataColumn("TotalAmount", typeof(string)));

        DataTable _dtCheck = new DataTable();
        DataRow _drC = null;
        _dtCheck.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrder", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Date", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("State", typeof(string)));

        //NEW COLUMN
        _dtCheck.Columns.Add(new DataColumn("VendorAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("RemitAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Zip", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("TotalAmountPay", typeof(double)));
        _dtCheck.Columns.Add(new DataColumn("Memo", typeof(string)));

        _objCD.ConnConfig = Session["config"].ToString();
        _objCD.Ref = long.Parse(txtcheckfrom.Text);
        _objCD.NextC = long.Parse(txtcheckto.Text);
        _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);

        

        DataSet _dsCheck = new DataSet();
        DataSet _dsCheck1 = new DataSet();
        DataSet _dsCheck2 = new DataSet();
        ListCheckDetailsByBankAndRef _ListCheckDetailsByBankAndRef = new ListCheckDetailsByBankAndRef();

        

        
            _dsCheck = _objBLBill.GetCheckDetailsByBankAndRef(_objCD);
        


        //int vid = Convert.ToInt32(_dsCheck.Tables[0].Rows[0]["Vendor"].ToString());
        DataTable dtNew = new DataTable();
        dtNew.Columns.Add("Name");
        dtNew.Columns.Add("Vendor");

        
            foreach (DataRow drow in _dsCheck.Tables[0].Rows)
            {
                DataRow drNew = dtNew.NewRow();
                drNew["Name"] = drow["VendorName"].ToString();
                drNew["Vendor"] = drow["Vendor"].ToString();
                dtNew.Rows.Add(drNew);
            }
        

        DataTable dtN = dtNew.DefaultView.ToTable(true);
        DataTable _dtAcct = new DataTable();
        //foreach (DataRow dr in dtN.Rows)
        //{
        int vid = Convert.ToInt32(dtN.Rows[0]["Vendor"].ToString());
        double AmountPay = 0.00;
        SumAmountpay = 0.00;



        //DataTable dtInvoice = _dsCheck.Tables[0].DefaultView.ToTable(true);
        DataView dtInv = new DataView();

        
        
            dtInv = _dsCheck.Tables[0].DefaultView;
        

        dtInv.RowFilter = "Vendor = '" + vid + "'";
        foreach (DataRow drow in dtInv.ToTable(true).Rows)
        {
            _dri = _dti.NewRow();
            _dri["Ref"] = drow["Ref"].ToString();
            _dri["Description"] = drow["Description"].ToString();
            _dri["InvoiceDate"] = drow["InvoiceDate"].ToString();
            _dri["Reference"] = drow["Refrerence"].ToString();
            _dri["Total"] = double.Parse(drow["Total"].ToString().Replace('$', '0'), NumberStyles.AllowParentheses |
                          NumberStyles.AllowThousands |
                          NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
            _dri["Disc"] = Convert.ToDouble(drow["Disc"].ToString()).ToString();
            _dri["AmountPay"] = Convert.ToDouble(drow["AmountPay"].ToString()).ToString();
            SumAmountpay = SumAmountpay + Convert.ToDouble(drow["AmountPay"].ToString());
            _dri["PayDate"] = drow["PayDate"].ToString();
            _dri["CheckNo"] = drow["CheckNo"].ToString();


            //_dri["VendorID"] = Convert.ToInt32(ddlVendor.SelectedValue);
            _dri["VendorID"] = drow["Vendor"].ToString();
            //ac _dri["VendorName"] = ddlVendor.SelectedItem.Text;
            _dri["VendorName"] = drow["VendorName"].ToString();
            _dti.Rows.Add(_dri);

            _dti.AcceptChanges();

        }










        _objVendor.ConnConfig = Session["config"].ToString();
        _objVendor.ID = vid;

        

        DataSet _dsV = new DataSet();
        List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

        

        
            _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
        

        string vendAddress = "";
        string vendAddress2 = "";
        if (_dsV.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
            {
                vendAddress = _dsV.Tables[0].Rows[0]["Address"].ToString() + ", ";
            }

            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
            {
                vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
            }
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
            {
                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                {
                    vendAddress2 += ", ";
                }
                vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
            }
        }


        DataView dtcheck = new DataView();
        
            dtcheck = _dsCheck.Tables[1].DefaultView;
        

        dtcheck.RowFilter = "Vendor = '" + vid + "'";
        foreach (DataRow drow in dtcheck.ToTable(true).Rows)
        //foreach (DataRow drow in _dsCheck.Tables[1].Rows)
        {
            _drC = _dtCheck.NewRow();
            if (Convert.ToDouble(drow["Pay"]) > 1000)
            {
                _drC["Pay"] = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                //_drC["Pay"] = ViewState["Dollar"].ToString();
            }
            else
            {
                string dollar = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                _drC["Pay"] = dollar + " Dollars";
            }
            _drC["ToOrder"] = drow["ToOrder"].ToString();
            //_drC["ToOrder"] = ViewState["Vendor"].ToString();
            _drC["Date"] = drow["Date"].ToString();
            _drC["CheckAmount"] = Convert.ToDouble(drow["Pay"]);
            _drC["ToOrderAddress"] = vendAddress;
            _drC["State"] = vendAddress2;

            _drC["TotalAmountpay"] = SumAmountpay;
            _drC["State"] = drow["State"].ToString();
            _dtCheck.Rows.Add(_drC);
        }






        DataSet dsCC = new DataSet();
        User objPropUser = new User();
        //ViewState["Checkno"] = lblNextCheck.Text;
        objPropUser.ConnConfig = Session["config"].ToString();
        

        if (Session["MSM"].ToString() != "TS")
        {
            

            
                dsCC = objBL_User.getControl(objPropUser);
            
        }
        else
        {
            objPropUser.LocID = Convert.ToInt32(0);
            

            

            

            
                dsCC = objBL_User.getControlBranch(objPropUser);
            
        }
        ReportViewer rvChecks = new ReportViewer();
        rvChecks.LocalReport.DataSources.Clear();
        //STIMULSOFT 
        byte[] buffer1 = null;
        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + reportName.Trim() + ".mrt");
        //  string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/APTopCheckDefault.mrt");
        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        report.Compile();
        report["TotalAmountPay"] = SumAmountpay;

        
            report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
        

        report["InvoiceCount"] = dti.Rows.Count;
        DataSet Invoice = new DataSet();
        DataTable dtInvoice = _dti;
        _dti.TableName = "Invoice";
        Invoice.Tables.Add(_dti);
        Invoice.DataSetName = "Invoice";

        DataSet Check = new DataSet();
        DataTable dtCheck = _dtCheck;
        _dtCheck.TableName = "Check";
        Check.Tables.Add(_dtCheck);
        Check.DataSetName = "Check";

        DataSet ControlBranch = new DataSet();
        DataTable dtControlBranch = new DataTable();
        dtControlBranch = dsCC.Tables[0].Copy();
        ControlBranch.Tables.Add(dtControlBranch);
        dtControlBranch.TableName = "ControlBranch";
        ControlBranch.DataSetName = "ControlBranch";

        report.RegData("dsInvoices", Invoice);
        report.RegData("dsCheck", Check);
        report.RegData("dsTicket", ControlBranch);
        report.Render();

        //StiWebDesigner1.Visible = true;
        //StiWebDesigner1.Report = report;

        //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "opencCeateForm();", true);
        return report;


    }
    protected void lnkSaveDefault_Click(object sender, EventArgs e)
    {
        try
        {
            string defaultpath = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/ApTopCheckDefault.mrt");
            string filePath = Server.MapPath("StimulsoftReports/APChecks/APTopCheck");
            string tempPath = Server.MapPath("StimulsoftReports/APChecks/APTopCheck");
            string selValue = ddlApTopCheckForLoad.Text.TrimEnd();
            if (selValue != null)
            {
                filePath = filePath + "\\" + selValue + ".mrt";
                tempPath = tempPath + "\\" + selValue + "temp.mrt";
                if (File.Exists(defaultpath))
                {
                    string[] lines = System.IO.File.ReadAllLines(defaultpath);
                    var myfile = File.Create(tempPath);
                    myfile.Close();
                    using (TextWriter tw = new StreamWriter(tempPath))
                        foreach (string line in lines)
                        {
                            tw.WriteLine(line);
                        }
                    File.Delete(defaultpath);
                    if (File.Exists(filePath))
                    {
                        string[] lines1 = System.IO.File.ReadAllLines(filePath);
                        var myfile1 = File.Create(defaultpath);
                        myfile1.Close();
                        using (TextWriter tw1 = new StreamWriter(defaultpath))
                            foreach (string line1 in lines1)
                            {
                                tw1.WriteLine(line1);
                            }
                        File.Delete(filePath);
                    }
                    if (File.Exists(tempPath))
                    {
                        string[] lines2 = System.IO.File.ReadAllLines(tempPath);
                        var myfile2 = File.Create(filePath);
                        myfile2.Close();
                        using (TextWriter tw2 = new StreamWriter(filePath))
                            foreach (string line2 in lines2)
                            {
                                tw2.WriteLine(line2);
                            }
                        File.Delete(tempPath);
                    }
                    Response.Redirect("ManageChecks.aspx");

                }
                else
                    throw new Exception("ApTopCheckDefault.mrt is not available");

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }


    }
    protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
    {

        string filePath = Server.MapPath("StimulsoftReports/APChecks/APTopCheck");

        string selValue = ddlApTopCheckForLoad.Text.Trim();
        if (selValue != null)
        {
            filePath = filePath + "\\" + selValue + ".mrt";
            if (!selValue.Equals("ApTopCheckDefault"))
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                ddlApTopCheckForLoad.Items.Clear();

                string path = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/");
                DirectoryInfo d = new DirectoryInfo(path);
                FileInfo[] Files = d.GetFiles("*.mrt");
                foreach (FileInfo file in Files)
                {
                    string FileName = string.Empty;
                    if (file.Name.Contains(".mrt"))
                        FileName = file.Name.Replace(".mrt", " ");
                    ddlApTopCheckForLoad.Items.Add((FileName));
                }
                ddlApTopCheckForLoad.Items.Remove(selValue);

                ddlApTopCheckForLoad.DataBind();
                string str = "Template " + selValue + " Deleted!--";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", " noty({text: '" + str + " </br> <b>', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
            // btnCutCheck.Visible = true;
        }
    }
    protected void ddlApMiddleCheckForLoad_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void imgPrintTemp2_Click(object sender, ImageClickEventArgs e)
    {                                                                        //                AP – check middle 
        try
        {


            byte[] buffer1 = null;
            //  string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/APMidCheckDefault.mrt");
            string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + ddlApMiddleCheckForLoad.SelectedItem.Text.Trim() + ".mrt");

            StiReport report = new StiReport();
            //  report = FillMiddleDataSetReport("APMidCheckDefault");
            FillReportMiddleDataSet(ddlApMiddleCheckForLoad.SelectedItem.Text.Trim());
            //AddCheck();
            //var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            //var service = new Stimulsoft.Report.Export.StiPdfExportService();
            //System.IO.MemoryStream stream = new System.IO.MemoryStream();
            //service.ExportTo(report, stream, settings);
            //buffer1 = stream.ToArray();

            //string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF", "APCheckMiddle.pdf");

            //if (buffer1 != null)
            //{
            //    if (File.Exists(filename))
            //        File.Delete(filename);
            //    using (var fs = new FileStream(filename, FileMode.Create))
            //    {
            //        fs.Write(buffer1, 0, buffer1.Length);
            //        fs.Close();
            //    }
            //}

            //Response.ClearContent();
            //Response.ClearHeaders();
            //Response.AddHeader("Content-Disposition", "attachment;filename=PrintCheck.pdf");
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("Content-Length", (buffer1.Length).ToString());
            //Response.BinaryWrite(buffer1);
            //Response.Flush();
            //Response.Close();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void ImageButton8_Click(object sender, ImageClickEventArgs e)
    {

        //mpeTemplate.Hide();

        string reportName = ddlApMiddleCheckForLoad.SelectedItem.Text.Trim();
        StiReport report = FillMiddleDataSetReport(reportName);
        StiWebDesigner2.Report = report;
        //ReportModalPopupExtender1.Show();
        Session["wc_second"] = "true";
        StiWebDesigner2.Visible = true;

        string script = "function f(){$find(\"" + RadWindowSecondReport.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

    }
    private StiReport FillMiddleDataSetReport(string reportName)
    {

        double SumAmountpay = 0.00;
        DataTable _dti = new DataTable();
        DataRow _dri = null;
        _dti.Columns.Add(new DataColumn("Ref", typeof(string)));
        _dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("Reference", typeof(string)));
        _dti.Columns.Add(new DataColumn("Total", typeof(double)));
        _dti.Columns.Add(new DataColumn("Disc", typeof(double)));
        _dti.Columns.Add(new DataColumn("AmountPay", typeof(double)));
        _dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("VendorName", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

        //RAHIL
        _dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("Description", typeof(string)));

        //New column
        _dti.Columns.Add(new DataColumn("State", typeof(string)));
        _dti.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dti.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dti.Columns.Add(new DataColumn("TotalAmount", typeof(string)));

        DataTable _dtCheck = new DataTable();
        DataRow _drC = null;
        _dtCheck.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrder", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Date", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("State", typeof(string)));

        //NEW COLUMN
        _dtCheck.Columns.Add(new DataColumn("VendorAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("RemitAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Zip", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("TotalAmountPay", typeof(double)));
        _dtCheck.Columns.Add(new DataColumn("Memo", typeof(string)));


        _objCD.ConnConfig = Session["config"].ToString();
        _objCD.Ref = long.Parse(txtcheckfrom.Text);
        _objCD.NextC = long.Parse(txtcheckto.Text);
        _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);

        

        DataSet _dsCheck = new DataSet();
        DataSet _dsCheck1 = new DataSet();
        DataSet _dsCheck2 = new DataSet();
        
            _dsCheck = _objBLBill.GetCheckDetailsByBankAndRef(_objCD);
        


        //int vid = Convert.ToInt32(_dsCheck.Tables[0].Rows[0]["Vendor"].ToString());
        DataTable dtNew = new DataTable();
        dtNew.Columns.Add("Name");
        dtNew.Columns.Add("Vendor");

        //if (IsAPIIntegrationEnable == "YES")
        
            foreach (DataRow drow in _dsCheck.Tables[0].Rows)
            {
                DataRow drNew = dtNew.NewRow();
                drNew["Name"] = drow["VendorName"].ToString();
                drNew["Vendor"] = drow["Vendor"].ToString();
                dtNew.Rows.Add(drNew);
            }
        

        DataTable dtN = dtNew.DefaultView.ToTable(true);
        DataTable _dtAcct = new DataTable();
        //foreach (DataRow dr in dtN.Rows)
        //{
        int vid = Convert.ToInt32(dtN.Rows[0]["Vendor"].ToString());
        double AmountPay = 0.00;
        SumAmountpay = 0.00;



        //DataTable dtInvoice = _dsCheck.Tables[0].DefaultView.ToTable(true);
        DataView dtInv = new DataView();
        
            dtInv = _dsCheck.Tables[0].DefaultView;
        

        dtInv.RowFilter = "Vendor = '" + vid + "'";
        foreach (DataRow drow in dtInv.ToTable(true).Rows)
        {
            _dri = _dti.NewRow();
            _dri["Ref"] = drow["Ref"].ToString();
            _dri["Description"] = drow["Description"].ToString();
            _dri["InvoiceDate"] = drow["InvoiceDate"].ToString();
            _dri["Reference"] = drow["Refrerence"].ToString();
            _dri["Total"] = double.Parse(drow["Total"].ToString().Replace('$', '0'), NumberStyles.AllowParentheses |
                          NumberStyles.AllowThousands |
                          NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
            _dri["Disc"] = Convert.ToDouble(drow["Disc"].ToString()).ToString();
            _dri["AmountPay"] = Convert.ToDouble(drow["AmountPay"].ToString()).ToString();
            SumAmountpay = SumAmountpay + Convert.ToDouble(drow["AmountPay"].ToString());
            _dri["PayDate"] = drow["PayDate"].ToString();
            _dri["CheckNo"] = drow["CheckNo"].ToString();


            //_dri["VendorID"] = Convert.ToInt32(ddlVendor.SelectedValue);
            _dri["VendorID"] = drow["Vendor"].ToString();
            //ac _dri["VendorName"] = ddlVendor.SelectedItem.Text;
            _dri["VendorName"] = drow["VendorName"].ToString();
            _dti.Rows.Add(_dri);

            _dti.AcceptChanges();

        }










        _objVendor.ConnConfig = Session["config"].ToString();
        _objVendor.ID = vid;

        

        DataSet _dsV = new DataSet();
        
            _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
        
        string vendAddress = "";
        string vendAddress2 = "";
        if (_dsV.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
            {
                vendAddress = _dsV.Tables[0].Rows[0]["Address"].ToString() + ", ";
            }

            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
            {
                vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
            }
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
            {
                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                {
                    vendAddress2 += ", ";
                }
                vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
            }
        }


        DataView dtcheck = new DataView();
        
            dtcheck = _dsCheck.Tables[1].DefaultView;
        

        dtcheck.RowFilter = "Vendor = '" + vid + "'";
        foreach (DataRow drow in dtcheck.ToTable(true).Rows)
        //foreach (DataRow drow in _dsCheck.Tables[1].Rows)
        {
            _drC = _dtCheck.NewRow();
            if (Convert.ToDouble(drow["Pay"]) > 1000)
            {
                _drC["Pay"] = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                //_drC["Pay"] = ViewState["Dollar"].ToString();
            }
            else
            {
                string dollar = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                _drC["Pay"] = dollar + " Dollars";
            }
            _drC["ToOrder"] = drow["ToOrder"].ToString();
            //_drC["ToOrder"] = ViewState["Vendor"].ToString();
            _drC["Date"] = drow["Date"].ToString();
            _drC["CheckAmount"] = Convert.ToDouble(drow["Pay"]);
            _drC["ToOrderAddress"] = vendAddress;
            _drC["State"] = vendAddress2;

            _drC["TotalAmountpay"] = SumAmountpay;
            _drC["State"] = drow["State"].ToString();
            _dtCheck.Rows.Add(_drC);
        }






        DataSet dsCC = new DataSet();
        User objPropUser = new User();
        //ViewState["Checkno"] = lblNextCheck.Text;
        objPropUser.ConnConfig = Session["config"].ToString();

        

        if (Session["MSM"].ToString() != "TS")
        {
            
                dsCC = objBL_User.getControl(objPropUser);
            
        }
        else
        {
            objPropUser.LocID = Convert.ToInt32(0);
           
                dsCC = objBL_User.getControlBranch(objPropUser);
           
        }

        ReportViewer rvChecks = new ReportViewer();
        rvChecks.LocalReport.DataSources.Clear();
        //STIMULSOFT 
        byte[] buffer1 = null;
        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + reportName.Trim() + ".mrt");

        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        report.Compile();
        report["TotalAmountPay"] = SumAmountpay;

        
            report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
        

        report["InvoiceCount"] = dti.Rows.Count;
        DataSet Invoice = new DataSet();
        DataTable dtInvoice = _dti;
        _dti.TableName = "Invoice";
        Invoice.Tables.Add(_dti);
        Invoice.DataSetName = "Invoice";

        DataSet Check = new DataSet();
        DataTable dtCheck = _dtCheck;
        _dtCheck.TableName = "Check";
        Check.Tables.Add(_dtCheck);
        Check.DataSetName = "Check";

        DataSet ControlBranch = new DataSet();
        DataTable dtControlBranch = new DataTable();
        dtControlBranch = dsCC.Tables[0].Copy();
        ControlBranch.Tables.Add(dtControlBranch);
        dtControlBranch.TableName = "ControlBranch";
        ControlBranch.DataSetName = "ControlBranch";

        report.RegData("dsInvoices", Invoice);
        report.RegData("dsCheck", Check);
        report.RegData("dsTicket", ControlBranch);
        report.Render();

        //StiWebDesigner1.Visible = true;
        //StiWebDesigner1.Report = report;

        //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "opencCeateForm();", true);
        return report;





    }
    protected void lnkSaveApMiddleCheck_Click(object sender, EventArgs e)
    {

        try
        {
            string defaultpath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/APMidCheckDefault.mrt");
            string filePath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck");
            string tempPath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck");

            string selValue = ddlApMiddleCheckForLoad.Text.TrimEnd();
            if (selValue != null)
            {
                filePath = filePath + "\\" + selValue + ".mrt";
                tempPath = tempPath + "\\" + selValue + "temp.mrt";
                if (File.Exists(defaultpath))
                {
                    string[] lines = System.IO.File.ReadAllLines(defaultpath);
                    var myfile = File.Create(tempPath);
                    myfile.Close();
                    using (TextWriter tw = new StreamWriter(tempPath))
                        foreach (string line in lines)
                        {
                            tw.WriteLine(line);
                        }
                    File.Delete(defaultpath);
                    if (File.Exists(filePath))
                    {
                        string[] lines1 = System.IO.File.ReadAllLines(filePath);
                        var myfile1 = File.Create(defaultpath);
                        myfile1.Close();
                        using (TextWriter tw1 = new StreamWriter(defaultpath))
                            foreach (string line1 in lines1)
                            {
                                tw1.WriteLine(line1);
                            }
                        File.Delete(filePath);
                    }
                    if (File.Exists(tempPath))
                    {
                        string[] lines2 = System.IO.File.ReadAllLines(tempPath);
                        var myfile2 = File.Create(filePath);
                        myfile2.Close();
                        using (TextWriter tw2 = new StreamWriter(filePath))
                            foreach (string line2 in lines2)
                            {
                                tw2.WriteLine(line2);
                            }
                        File.Delete(tempPath);
                    }
                    Response.Redirect("ManageChecks.aspx");

                }
                else
                    throw new Exception("ApMiddleCheckDefault.mrt is not available");

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }


    }
    protected void ImageButton6_Click(object sender, ImageClickEventArgs e)
    {

        string filePath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck");

        string selValue = ddlApMiddleCheckForLoad.Text.Trim();
        if (selValue != null)
        {
            filePath = filePath + "\\" + selValue + ".mrt";
            if (!selValue.Equals("APMidCheckDefault"))
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                ddlApMiddleCheckForLoad.Items.Clear();
                string MidCheckpath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/");
                DirectoryInfo dirMidPath = new DirectoryInfo(MidCheckpath);
                FileInfo[] FilesMid = dirMidPath.GetFiles("*.mrt");
                foreach (FileInfo fileMid in FilesMid)
                {
                    string FileName = string.Empty;
                    if (fileMid.Name.Contains(".mrt"))
                        FileName = fileMid.Name.Replace(".mrt", " ");
                    ddlApMiddleCheckForLoad.Items.Add((FileName));
                }
                ddlApMiddleCheckForLoad.Items.Remove(selValue);
                ddlApMiddleCheckForLoad.DataBind();
                string str = "Template " + selValue + " Deleted!--";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", " noty({text: '" + str + " </br> <b>', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
            //btnCutCheck.Visible = true;
        }
    }
    protected void ddlTopChecksForLoad_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void imgPrintTemp6_Click(object sender, ImageClickEventArgs e)
    {                                                                           //              MADDEN – check top 
        try
        {

            //F:\ESS\ESSMOM\MOM\MOM - NewDesign\MSWeb\StimulsoftReports\APChecks\APTopCheck
            byte[] buffer1 = null;
            string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + ddlTopChecksForLoad.SelectedItem.Text.Trim() + ".mrt");
            StiReport report = new StiReport();
            //  FillDataSetReport2();
            // report = FillMaddenDataSetForReport("TopCheckReportDefault");
            //report = FillMaddenDataSetForReport(ddlTopChecksForLoad.SelectedItem.Text.Trim());
            //var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            //var service = new Stimulsoft.Report.Export.StiPdfExportService();
            //System.IO.MemoryStream stream = new System.IO.MemoryStream();
            //service.ExportTo(report, stream, settings);
            //buffer1 = stream.ToArray();

            //string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "\\TempPDF", "TopCheckReport.pdf");

            //if (buffer1 != null)
            //{
            //    if (File.Exists(filename))
            //        File.Delete(filename);
            //    using (var fs = new FileStream(filename, FileMode.Create))
            //    {
            //        fs.Write(buffer1, 0, buffer1.Length);
            //        fs.Close();
            //    }
            //}

            //Response.ClearContent();
            //Response.ClearHeaders();
            //Response.AddHeader("Content-Disposition", "attachment;filename=PrintCheck.pdf");
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("Content-Length", (buffer1.Length).ToString());
            //Response.BinaryWrite(buffer1);
            //Response.Flush();
            //Response.Close();
            FillReportMaddenDataSet(ddlTopChecksForLoad.SelectedItem.Text.Trim());

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void ImageButton9_Click(object sender, ImageClickEventArgs e)
    {

        string reportName = ddlTopChecksForLoad.SelectedItem.Text.Trim();
        // mpeTemplate.Hide();
        StiReport report = FillMaddenDataSetForReport(reportName);
        StiWebDesigner3.Report = report;
        //ReportModalPopupExtender2.Show();
        Session["wc_third"] = "true";
        StiWebDesigner3.Visible = true;

        string script = "function f(){$find(\"" + RadWindowThirdReport.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);


    }
    private StiReport FillMaddenDataSetForReport(string reportName)
    {

        double SumAmountpay = 0.00;
        DataTable _dti = new DataTable();
        DataRow _dri = null;
        _dti.Columns.Add(new DataColumn("Ref", typeof(string)));
        _dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("Reference", typeof(string)));
        _dti.Columns.Add(new DataColumn("Total", typeof(double)));
        _dti.Columns.Add(new DataColumn("Disc", typeof(double)));
        _dti.Columns.Add(new DataColumn("AmountPay", typeof(double)));
        _dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("VendorName", typeof(string)));

        //RAHIL
        _dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("Description", typeof(string)));

        //New column
        _dti.Columns.Add(new DataColumn("State", typeof(string)));
        _dti.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dti.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dti.Columns.Add(new DataColumn("TotalAmount", typeof(string)));

        DataTable _dtCheck = new DataTable();
        DataRow _drC = null;
        _dtCheck.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrder", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Date", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("CheckAmount", typeof(double)));
        _dtCheck.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("State", typeof(string)));

        //NEW COLUMN
        _dtCheck.Columns.Add(new DataColumn("VendorAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("RemitAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Zip", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("TotalAmountPay", typeof(double)));
        _dtCheck.Columns.Add(new DataColumn("Memo", typeof(string)));

        _objCD.ConnConfig = Session["config"].ToString();
        _objCD.Ref = long.Parse(txtcheckfrom.Text);
        _objCD.NextC = long.Parse(txtcheckto.Text);
        _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);

        

        DataSet _dsCheck = new DataSet();
        DataSet _dsCheck1 = new DataSet();
        DataSet _dsCheck2 = new DataSet();

       
            _dsCheck = _objBLBill.GetCheckDetailsByBankAndRef(_objCD);
       


        //int vid = Convert.ToInt32(_dsCheck.Tables[0].Rows[0]["Vendor"].ToString());
        DataTable dtNew = new DataTable();
        dtNew.Columns.Add("Name");
        dtNew.Columns.Add("Vendor");

        
            foreach (DataRow drow in _dsCheck.Tables[0].Rows)
            {
                DataRow drNew = dtNew.NewRow();
                drNew["Name"] = drow["VendorName"].ToString();
                drNew["Vendor"] = drow["Vendor"].ToString();
                dtNew.Rows.Add(drNew);
            }
        

        DataTable dtN = dtNew.DefaultView.ToTable(true);

        //foreach (DataRow dr in dtN.Rows)
        //{
        int vid = Convert.ToInt32(dtN.Rows[0]["Vendor"].ToString());
        double AmountPay = 0.00;
        SumAmountpay = 0.00;



        //DataTable dtInvoice = _dsCheck.Tables[0].DefaultView.ToTable(true);
        DataView dtInv = new DataView();
        
            dtInv = _dsCheck.Tables[0].DefaultView;
        

        dtInv.RowFilter = "Vendor = '" + vid + "'";
        foreach (DataRow drow in dtInv.ToTable(true).Rows)
        {
            _dri = _dti.NewRow();
            _dri["Ref"] = drow["Ref"].ToString();
            _dri["Description"] = drow["Description"].ToString();
            _dri["InvoiceDate"] = drow["InvoiceDate"].ToString();
            _dri["Reference"] = drow["Refrerence"].ToString();
            _dri["Total"] = double.Parse(drow["Total"].ToString().Replace('$', '0'), NumberStyles.AllowParentheses |
                          NumberStyles.AllowThousands |
                          NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
            _dri["Disc"] = Convert.ToDouble(drow["Disc"].ToString()).ToString();
            _dri["AmountPay"] = Convert.ToDouble(drow["AmountPay"].ToString()).ToString();
            SumAmountpay = SumAmountpay + Convert.ToDouble(drow["AmountPay"].ToString());
            _dri["PayDate"] = drow["PayDate"].ToString();
            _dri["CheckNo"] = drow["CheckNo"].ToString();


            //_dri["VendorID"] = Convert.ToInt32(ddlVendor.SelectedValue);
            _dri["VendorID"] = drow["Vendor"].ToString();
            //ac _dri["VendorName"] = ddlVendor.SelectedItem.Text;
            _dri["VendorName"] = drow["VendorName"].ToString();
            _dti.Rows.Add(_dri);

            _dti.AcceptChanges();

        }

        _objVendor.ConnConfig = Session["config"].ToString();
        _objVendor.ID = vid;

        

        DataSet _dsV = new DataSet();
        
            _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
        

        string vendAddress = "";
        string vendAddress2 = "";
        if (_dsV.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
            {
                vendAddress = _dsV.Tables[0].Rows[0]["Address"].ToString() + ", ";
            }

            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
            {
                vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
            }
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
            {
                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                {
                    vendAddress2 += ", ";
                }
                vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
            }
        }


        string chknos = null;
        int checkno = 0;

        DataView dtcheck = new DataView();
        
            dtcheck = _dsCheck.Tables[1].DefaultView;
        

        dtcheck.RowFilter = "Vendor = '" + vid + "'";
        foreach (DataRow drow in dtcheck.ToTable(true).Rows)
        //foreach (DataRow drow in _dsCheck.Tables[1].Rows)
        {
            _drC = _dtCheck.NewRow();
            if (Convert.ToDouble(drow["Pay"]) > 1000)
            {
                _drC["Pay"] = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                //_drC["Pay"] = ViewState["Dollar"].ToString();
            }
            else
            {
                string dollar = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                _drC["Pay"] = dollar + " Dollars";
            }
            _drC["ToOrder"] = drow["ToOrder"].ToString();
            //_drC["ToOrder"] = ViewState["Vendor"].ToString();
            _drC["Date"] = drow["Date"].ToString();
            _drC["CheckAmount"] = Convert.ToDouble(drow["Pay"]);
            _drC["ToOrderAddress"] = vendAddress;
            _drC["State"] = vendAddress2;

            _drC["TotalAmountpay"] = SumAmountpay;
            _drC["State"] = drow["State"].ToString();
            chknos = drow["CheckNo"].ToString();
            _dtCheck.Rows.Add(_drC);
        }

        DataSet dsCC = new DataSet();
        User objPropUser = new User();
        //ViewState["Checkno"] = lblNextCheck.Text;
        objPropUser.ConnConfig = Session["config"].ToString();
       

        if (Session["MSM"].ToString() != "TS")
        {
            
                dsCC = objBL_User.getControl(objPropUser);
            
        }
        else
        {
            objPropUser.LocID = Convert.ToInt32(0);

            

            
                dsCC = objBL_User.getControlBranch(objPropUser);
            
        }
        //dsBank

        CreateTableBank();

        DataRow _drB = null;
        DataRow _drA = null;
        _objBank.ConnConfig = Session["config"].ToString();
        _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);

        

        DataSet _dsB = new DataSet();
        
            _dsB = _objBLBill.GetBankCD(_objBank);
        

        _drB = dtBank.NewRow();
        if (_dsB.Tables[0].Rows.Count > 0)
        {
            _drB["Name"] = _dsB.Tables[0].Rows[0]["Name"].ToString();
            _drB["Address"] = _dsB.Tables[0].Rows[0]["Address"].ToString();
            _drB["State"] = _dsB.Tables[0].Rows[0]["State"].ToString();
            _drB["City"] = _dsB.Tables[0].Rows[0]["City"].ToString();
            _drB["Zip"] = _dsB.Tables[0].Rows[0]["Zip"].ToString();
            _drB["NBranch"] = _dsB.Tables[0].Rows[0]["NBranch"].ToString();
            _drB["NAcct"] = _dsB.Tables[0].Rows[0]["NAcct"].ToString();
            _drB["NRoute"] = _dsB.Tables[0].Rows[0]["NRoute"].ToString();
            //_drB["Ref"] = _dsB.Tables[0].Rows[0]["Ref"].ToString();
            //_dtBank.Rows.Add(_drB);
        }

        string checkNumber = string.Empty;
        if (!string.IsNullOrEmpty(chknos))
        {
            checkNumber = chknos;
        }
        else
        {
            checkNumber = chknos.ToString();
        }

        if (checkNumber.Length == 1)
        {
            _drB["Ref"] = "00000000" + checkNumber;
        }
        else if (checkNumber.Length == 2)
        {
            _drB["Ref"] = "0000000" + checkNumber;
        }
        else if (checkNumber.Length == 3)
        {
            _drB["Ref"] = "000000" + checkNumber;
        }
        else if (checkNumber.Length == 4)
        {
            _drB["Ref"] = "00000" + checkNumber;
        }
        else if (checkNumber.Length == 5)
        {
            _drB["Ref"] = "0000" + checkNumber;
        }
        else if (checkNumber.Length == 6)
        {
            _drB["Ref"] = "000" + checkNumber;
        }
        else if (checkNumber.Length == 7)
        {
            _drB["Ref"] = "00" + checkNumber;
        }
        else if (checkNumber.Length == 8)
        {
            _drB["Ref"] = "0" + checkNumber;
        }
        else
        {
            _drB["Ref"] = "000000000";
        }

        dtBank.Rows.Add(_drB);

        _objVendor.ConnConfig = Session["config"].ToString();
        _objVendor.ID = vid;

       

        DataSet _dsA = new DataSet();
       
            _dsA = _objBLVendor.GetVendorAcct(_objVendor);
       

        DataTable _dtAcct = new DataTable();
        _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));
        if (_dsA.Tables[0].Rows.Count > 0)
        {
            _drA = _dtAcct.NewRow();
            _drA["VendorID"] = _dsA.Tables[0].Rows[0]["ID"].ToString();
            _drA["VendorAcct"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
            _dtAcct.Rows.Add(_drA);
        }


        //dsBank end


        ReportViewer rvChecks = new ReportViewer();
        rvChecks.LocalReport.DataSources.Clear();
        //STIMULSOFT 
        byte[] buffer1 = null;
        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + reportName.Trim() + ".mrt");
        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        report.Compile();
        report["TotalAmountPay"] = SumAmountpay;


        
            report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
        

        report["InvoiceCount"] = dti.Rows.Count;
        DataSet Invoice = new DataSet();
        DataTable dtInvoice = _dti;
        _dti.TableName = "Invoice";
        Invoice.Tables.Add(_dti);
        Invoice.DataSetName = "Invoice";

        DataSet Check = new DataSet();
        DataTable dtCheck = _dtCheck;
        _dtCheck.TableName = "Check";
        Check.Tables.Add(_dtCheck);
        Check.DataSetName = "Check";

        DataSet ControlBranch = new DataSet();
        DataTable dtControlBranch = new DataTable();
        dtControlBranch = dsCC.Tables[0].Copy();
        ControlBranch.Tables.Add(dtControlBranch);
        dtControlBranch.TableName = "ControlBranch";
        ControlBranch.DataSetName = "ControlBranch";


        DataSet Bank = new DataSet();
        DataTable _dtBank = dtBank;
        dtBank.TableName = "Bank";
        Bank.Tables.Add(dtBank);
        Bank.DataSetName = "Bank";

        DataSet Account = new DataSet();
        DataTable dtAccount = _dtAcct;
        _dtAcct.TableName = "Account";
        Account.Tables.Add(_dtAcct);
        Account.DataSetName = "Account";


        report.RegData("dsInvoices", Invoice);
        report.RegData("dsCheck", Check);
        report.RegData("dsTicket", ControlBranch);
        report.RegData("dsBank", Bank);
        report.RegData("dsAccount", Account);
        report.Render();

        //StiWebDesigner1.Visible = true;
        //StiWebDesigner1.Report = report;

        //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "opencCeateForm();", true);
        return report;
    }
    protected void lnkTopChecks_Click(object sender, EventArgs e)
    {

        try
        {
            string defaultpath = Server.MapPath("StimulsoftReports/APChecks/TopChecks/TopCheckReportDefault.mrt");
            string filePath = Server.MapPath("StimulsoftReports/APChecks/TopChecks");
            string tempPath = Server.MapPath("StimulsoftReports/APChecks/TopChecks");
            string selValue = ddlTopChecksForLoad.Text.TrimEnd();
            if (selValue != null)
            {
                filePath = filePath + "\\" + selValue + ".mrt";
                tempPath = tempPath + "\\" + selValue + "temp.mrt";
                if (File.Exists(defaultpath))
                {
                    string[] lines = System.IO.File.ReadAllLines(defaultpath);
                    var myfile = File.Create(tempPath);
                    myfile.Close();
                    using (TextWriter tw = new StreamWriter(tempPath))
                        foreach (string line in lines)
                        {
                            tw.WriteLine(line);
                        }
                    File.Delete(defaultpath);
                    if (File.Exists(filePath))
                    {
                        string[] lines1 = System.IO.File.ReadAllLines(filePath);
                        var myfile1 = File.Create(defaultpath);
                        myfile1.Close();
                        using (TextWriter tw1 = new StreamWriter(defaultpath))
                            foreach (string line1 in lines1)
                            {
                                tw1.WriteLine(line1);
                            }
                        File.Delete(filePath);
                    }
                    if (File.Exists(tempPath))
                    {
                        string[] lines2 = System.IO.File.ReadAllLines(tempPath);
                        var myfile2 = File.Create(filePath);
                        myfile2.Close();
                        using (TextWriter tw2 = new StreamWriter(filePath))
                            foreach (string line2 in lines2)
                            {
                                tw2.WriteLine(line2);
                            }
                        File.Delete(tempPath);
                    }
                    Response.Redirect("ManageChecks.aspx");

                }
                else
                    throw new Exception("TopCheckReportDefault.mrt is not available");

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }
    protected void ImageButton14_Click(object sender, ImageClickEventArgs e)
    {
        string filePath = Server.MapPath("StimulsoftReports/APChecks/TopChecks");

        string selValue = ddlTopChecksForLoad.Text.Trim();
        if (selValue != null)
        {
            filePath = filePath + "\\" + selValue + ".mrt";
            if (!selValue.Equals("TopCheckReportDefault"))
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                ddlTopChecksForLoad.Items.Clear();

                string TopCheckpath = Server.MapPath("StimulsoftReports/APChecks/TopChecks/");
                DirectoryInfo dirTopcheckPath = new DirectoryInfo(TopCheckpath);
                FileInfo[] FilesTop = dirTopcheckPath.GetFiles("*.mrt");
                foreach (FileInfo fileTop in FilesTop)
                {
                    string FileName = string.Empty;
                    if (fileTop.Name.Contains(".mrt"))
                        FileName = fileTop.Name.Replace(".mrt", " ");
                    ddlTopChecksForLoad.Items.Add((FileName));
                }
                ddlTopChecksForLoad.Items.Remove((selValue));
                ddlTopChecksForLoad.DataBind();
                string str = "Template " + selValue + " Deleted!--";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", " noty({text: '" + str + " </br> <b>', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        //btnCutCheck.Visible = true;

    }
    private void FillReportApTopCheckDataSet(string reportName)
    {
        try
        {
            int count = 0;
            _objCD.ConnConfig = Session["config"].ToString();
            _objCD.Ref = long.Parse(txtcheckfrom.Text);
            _objCD.NextC = long.Parse(txtcheckto.Text);
            _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);

            


            DataSet _dsCheck = new DataSet();
            DataSet _dsCheck1 = new DataSet();
            DataSet _dsCheck2 = new DataSet();

            
                _dsCheck = _objBLBill.GetCheckDetailsByBankAndRef(_objCD);
            


            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("Name");
            dtNew.Columns.Add("Vendor");
            dtNew.Columns.Add("CheckNo");
            //if (IsAPIIntegrationEnable == "YES")
            
                foreach (DataRow drow in _dsCheck.Tables[0].Rows)
                {
                    DataRow drNew = dtNew.NewRow();
                    drNew["Name"] = drow["VendorName"].ToString();
                    drNew["Vendor"] = drow["Vendor"].ToString();
                    drNew["CheckNo"] = drow["CheckNo"].ToString();
                    dtNew.Rows.Add(drNew);
                }
            

            DataTable dtN = dtNew.DefaultView.ToTable(true);
            DataTable _dtAcct = new DataTable();
            foreach (DataRow dr in dtN.Rows)
            {
                bool isChecked = true;
                _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
                _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

                CreateTableInvoice();
                CreateTablePayee();
                CreateTableBank();

                DataRow _dri = null;
                DataRow _drC = null;

                int vid = Convert.ToInt32(dr["Vendor"].ToString());
                string checkNo = Convert.ToString(dr["CheckNo"].ToString());
                double AmountPay = 0.00;

                DataView dtInv = new DataView();
                
                    dtInv = _dsCheck.Tables[0].DefaultView;
                
                dtInv.RowFilter = "Vendor = '" + vid + "' and CheckNo = '" + checkNo + "'";
                foreach (DataRow drow in dtInv.ToTable(true).Rows)
                {
                    _dri = dti.NewRow();
                    _dri["Ref"] = drow["Ref"].ToString();
                    _dri["Description"] = drow["Description"].ToString();
                    _dri["InvoiceDate"] = drow["InvoiceDate"].ToString();
                    _dri["Reference"] = drow["Refrerence"].ToString();
                    _dri["Total"] = double.Parse(drow["Total"].ToString().Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                    _dri["Disc"] = Convert.ToDouble(drow["Disc"].ToString()).ToString();
                    _dri["AmountPay"] = Convert.ToDouble(drow["AmountPay"].ToString()).ToString();
                    AmountPay = AmountPay + Convert.ToDouble(drow["AmountPay"].ToString());
                    _dri["PayDate"] = drow["PayDate"].ToString();
                    _dri["CheckNo"] = drow["CheckNo"].ToString();

                    _dri["VendorID"] = drow["Vendor"].ToString();
                    _dri["VendorName"] = drow["VendorName"].ToString();
                    dti.Rows.Add(_dri);

                    dti.AcceptChanges();
                }

                if (isChecked)
                {
                    if (dti.Rows.Count > 0)
                    {
                        DataView dtcheck = new DataView();
                        
                            dtcheck = _dsCheck.Tables[1].DefaultView;
                        

                        dtcheck.RowFilter = "Vendor = '" + vid + "' and CheckNo = '" + checkNo + "'";
                        ViewState["CheckStatus"] = "0";
                        foreach (DataRow drow in dtcheck.ToTable(true).Rows)
                        {
                            _drC = dtpay.NewRow();
                            if (Convert.ToDouble(drow["Pay"]) > 1000)
                            {
                                _drC["Pay"] = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                            }
                            else
                            {
                                string dollar = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                                _drC["Pay"] = dollar + " Dollars";
                            }
                            _drC["ToOrder"] = drow["ToOrder"].ToString();
                            _drC["Date"] = drow["Date"].ToString();
                            _drC["CheckAmount"] = Convert.ToDouble(drow["Pay"]);
                            _drC["VendorAddress"] = drow["VendorAddress"].ToString();
                            _drC["RemitAddress"] = drow["RemitAddress"].ToString();
                            _drC["State"] = drow["State"].ToString();
                            _drC["Zip"] = drow["Zip"].ToString();
                            ViewState["CheckStatus"] = drow["Status"].ToString();
                            dtpay.Rows.Add(_drC);
                        }

                        var rowCount = 0;
                        var totalRows = dti.Rows.Count;
                        if (reportName.Contains("-"))
                        {
                            try
                            {
                                string[] reportNameArr = reportName.Split('-');
                                rowCount = Convert.ToInt32(reportNameArr[1].ToString().Trim().TrimStart());
                                if (totalRows < rowCount)
                                    rowCount = totalRows;
                            }
                            catch (Exception ex) { rowCount = totalRows; }
                        }
                        else
                            rowCount = 6;
                        var dtiCopy = dti.Copy();
                        DataView dv = dtiCopy.DefaultView;
                        dv.Sort = "Ref asc";
                        DataTable sortedDT = dv.ToTable();
                        var dtCopy = sortedDT.Copy();
                        var firstHalf = dtCopy;
                        var secondHalf = dtCopy;
                        if (dtCopy.Rows.Count > rowCount)
                        {
                            firstHalf = dtCopy.AsEnumerable().Take(rowCount).CopyToDataTable();
                            secondHalf = dtCopy.Clone();
                            if (totalRows > rowCount)
                            {
                                secondHalf = dtCopy.AsEnumerable().Skip(rowCount).Take(totalRows - rowCount).CopyToDataTable();
                            }
                        }
                        else
                        {
                            firstHalf = dtCopy;
                        }

                        //STIMULSOFT 
                        byte[] buffer1 = null;
                        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + reportName.Trim() + ".mrt");
                        StiReport report = new StiReport();
                        report.Load(reportPathStimul);
                        if (Convert.ToInt16(ViewState["CheckStatus"]).Equals(2))
                        {
                            report.Pages[0].Watermark.Enabled = true;
                            string imagepath = Server.MapPath("images/icons/voidcheck.png");
                            report.Pages[0].Watermark.Image = System.Drawing.Image.FromFile(imagepath);
                            report.Pages[0].Watermark.ImageAlignment = System.Drawing.ContentAlignment.TopCenter;
                            report.Pages[0].Watermark.ShowImageBehind = true;
                        }
                        report.Compile();
                        report["TotalAmountPay"] = AmountPay;
                        
                            report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
                        


                        report["InvoiceCount"] = totalRows;
                        DataSet Invoice = new DataSet();
                        DataTable dtInvoice = firstHalf.Copy();
                        dtInvoice.TableName = "Invoice";
                        Invoice.Tables.Add(dtInvoice);
                        Invoice.DataSetName = "Invoice";

                        DataSet Check = new DataSet();
                        DataTable dtCheck = dtpay.Copy();
                        dtCheck.TableName = "Check";
                        Check.Tables.Add(dtCheck);
                        Check.DataSetName = "Check";

                        report.RegData("Invoice", Invoice);
                        report.RegData("Check", Check);

                        report.Render();
                        var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                        var service = new Stimulsoft.Report.Export.StiPdfExportService();
                        System.IO.MemoryStream stream = new System.IO.MemoryStream();
                        service.ExportTo(report, stream, settings);
                        buffer1 = stream.ToArray();
                        lstbyte.Add(buffer1);

                        if (totalRows > rowCount)
                        {
                            byte[] bufferNew = null;
                            reportPathStimul = Server.MapPath("StimulsoftReports/TopCheckSubReport.mrt");
                            report = new StiReport();
                            report.Load(reportPathStimul);
                            report.Compile();

                            
                                report["TotalAmountPay"] = Convert.ToDouble(_dsCheck.Tables[0].Rows[0]["AmountPay"].ToString());
                                report["AccountNo"] = "";
                                report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
                            

                            report["InvoiceCount"] = totalRows;
                            Invoice = new DataSet();
                            dtInvoice = secondHalf.Copy();
                            dtInvoice.TableName = "Invoice";
                            Invoice.Tables.Add(dtInvoice);
                            Invoice.DataSetName = "Invoice";

                            Check = new DataSet();
                            dtCheck = dtpay.Copy();
                            dtCheck.TableName = "Check";
                            Check.Tables.Add(dtCheck);
                            Check.DataSetName = "Check";

                            report.RegData("Invoice", Invoice);
                            report.RegData("Check", Check);
                            report.Render();
                            settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                            service = new Stimulsoft.Report.Export.StiPdfExportService();
                            stream = new System.IO.MemoryStream();
                            service.ExportTo(report, stream, settings);
                            bufferNew = stream.ToArray();

                            lstbyteNew.Add(bufferNew);
                        }
                    }
                    count++;
                }

                _dtAcct.Reset();
                dti.Reset();
                dtpay.Reset();
                dtBank.Reset();
            }

            byte[] finalbyte = null;

            if (lstbyteNew.Count != 0)
            {
                finalbyte = WriteChecks.concatAndAddContentFinal(lstbyte, lstbyteNew);
            }
            else
            {
                finalbyte = WriteChecks.concatAndAddContent(lstbyte);
            }

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment;filename=ApTopCheckCub.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (finalbyte.Length).ToString());
            Response.BinaryWrite(finalbyte);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillReportMaddenDataSet(string reportName)
    {
        try
        {
            int count = 0;
            _objCD.ConnConfig = Session["config"].ToString();
            _objCD.Ref = long.Parse(txtcheckfrom.Text);
            _objCD.NextC = long.Parse(txtcheckto.Text);
            _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);

            

            DataSet _dsCheck = new DataSet();
            DataSet _dsCheck1 = new DataSet();
            DataSet _dsCheck2 = new DataSet();

            
                _dsCheck = _objBLBill.GetCheckDetailsByBankAndRef(_objCD);
            



            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("Name");
            dtNew.Columns.Add("Vendor");
            dtNew.Columns.Add("CheckNo");

            
                foreach (DataRow drow in _dsCheck.Tables[0].Rows)
                {
                    DataRow drNew = dtNew.NewRow();
                    drNew["Name"] = drow["VendorName"].ToString();
                    drNew["Vendor"] = drow["Vendor"].ToString();
                    drNew["CheckNo"] = drow["CheckNo"].ToString();
                    dtNew.Rows.Add(drNew);
                }
            

            DataTable dtN = dtNew.DefaultView.ToTable(true);
            DataTable _dtAcct = new DataTable();
            foreach (DataRow dr in dtN.Rows)
            {
                bool isChecked = true;
                _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
                _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

                CreateTableInvoice();
                CreateTablePayee();
                CreateTableBank();


                DataRow _dri = null;
                DataRow _drC = null;

                int vid = Convert.ToInt32(dr["Vendor"].ToString());
                string checkNo = Convert.ToString(dr["CheckNo"].ToString());
                DataRow _drB = null;
                DataRow _drA = null;
                double AmountPay = 0.00;

                DataView dtInv = new DataView();
                
                    dtInv = _dsCheck.Tables[0].DefaultView;
                

                dtInv.RowFilter = "Vendor = '" + vid + "' and CheckNo = '" + checkNo + "'";
                foreach (DataRow drow in dtInv.ToTable(true).Rows)
                {
                    _dri = dti.NewRow();
                    _dri["Ref"] = drow["Ref"].ToString();
                    _dri["Description"] = drow["Description"].ToString();
                    _dri["InvoiceDate"] = drow["InvoiceDate"].ToString();
                    _dri["Reference"] = drow["Refrerence"].ToString();
                    _dri["Total"] = double.Parse(drow["Total"].ToString().Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                    _dri["Disc"] = Convert.ToDouble(drow["Disc"].ToString()).ToString();
                    _dri["AmountPay"] = Convert.ToDouble(drow["AmountPay"].ToString()).ToString();
                    AmountPay = AmountPay + Convert.ToDouble(drow["AmountPay"].ToString());
                    _dri["PayDate"] = drow["PayDate"].ToString();
                    _dri["CheckNo"] = drow["CheckNo"].ToString();
                    _dri["VendorID"] = drow["Vendor"].ToString();
                    _dri["VendorName"] = drow["VendorName"].ToString();
                    dti.Rows.Add(_dri);

                    dti.AcceptChanges();
                }
                if (isChecked)
                {
                    if (dti.Rows.Count > 0)
                    {
                        string chknos = null;
                        DataView dtcheck = new DataView();
                        
                            dtcheck = _dsCheck.Tables[1].DefaultView;
                        

                        dtcheck.RowFilter = "Vendor = '" + vid + "' and CheckNo = '" + checkNo + "'";
                        ViewState["CheckStatus"] = "0";
                        foreach (DataRow drow in dtcheck.ToTable(true).Rows)
                        {
                            _drC = dtpay.NewRow();
                            if (Convert.ToDouble(drow["Pay"]) > 1000)
                            {
                                _drC["Pay"] = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                            }
                            else
                            {
                                string dollar = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                                _drC["Pay"] = dollar + " Dollars";
                            }
                            _drC["ToOrder"] = drow["ToOrder"].ToString();
                            _drC["Date"] = drow["Date"].ToString();
                            _drC["CheckAmount"] = Convert.ToDouble(drow["Pay"]);
                            _drC["VendorAddress"] = drow["VendorAddress"].ToString();
                            _drC["RemitAddress"] = drow["RemitAddress"].ToString();
                            _drC["State"] = drow["State"].ToString();
                            _drC["Zip"] = drow["Zip"].ToString();
                            ViewState["CheckStatus"] = drow["Status"].ToString();
                            chknos = drow["CheckNo"].ToString();
                            dtpay.Rows.Add(_drC);
                        }

                        _objBank.ConnConfig = Session["config"].ToString();
                        _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);

                        

                        DataSet _dsB = new DataSet();
                        
                            _dsB = _objBLBill.GetBankCD(_objBank);
                        

                        _drB = dtBank.NewRow();
                        if (_dsB.Tables[0].Rows.Count > 0)
                        {
                            _drB["Name"] = _dsB.Tables[0].Rows[0]["Name"].ToString();
                            _drB["Address"] = _dsB.Tables[0].Rows[0]["Address"].ToString();
                            _drB["State"] = _dsB.Tables[0].Rows[0]["State"].ToString();
                            _drB["City"] = _dsB.Tables[0].Rows[0]["City"].ToString();
                            _drB["Zip"] = _dsB.Tables[0].Rows[0]["Zip"].ToString();
                            _drB["NBranch"] = _dsB.Tables[0].Rows[0]["NBranch"].ToString();
                            _drB["NAcct"] = _dsB.Tables[0].Rows[0]["NAcct"].ToString();
                            _drB["NRoute"] = _dsB.Tables[0].Rows[0]["NRoute"].ToString();
                        }
                        string checkNumber = string.Empty;
                        if (!string.IsNullOrEmpty(chknos))
                        {
                            checkNumber = chknos.ToString();
                        }
                        else
                        {
                            checkNumber = chknos.ToString();
                        }

                        if (checkNumber.Length == 1)
                        {
                            _drB["Ref"] = "00000000" + checkNumber;
                        }
                        else if (checkNumber.Length == 2)
                        {
                            _drB["Ref"] = "0000000" + checkNumber;
                        }
                        else if (checkNumber.Length == 3)
                        {
                            _drB["Ref"] = "000000" + checkNumber;
                        }
                        else if (checkNumber.Length == 4)
                        {
                            _drB["Ref"] = "00000" + checkNumber;
                        }
                        else if (checkNumber.Length == 5)
                        {
                            _drB["Ref"] = "0000" + checkNumber;
                        }
                        else if (checkNumber.Length == 6)
                        {
                            _drB["Ref"] = "000" + checkNumber;
                        }
                        else if (checkNumber.Length == 7)
                        {
                            _drB["Ref"] = "00" + checkNumber;
                        }
                        else if (checkNumber.Length == 8)
                        {
                            _drB["Ref"] = "0" + checkNumber;
                        }
                        else
                        {
                            _drB["Ref"] = "000000000";
                        }

                        dtBank.Rows.Add(_drB);

                        _objVendor.ConnConfig = Session["config"].ToString();
                        _objVendor.ID = vid;

                       

                        DataSet _dsA = new DataSet();
                        
                            _dsA = _objBLVendor.GetVendorAcct(_objVendor);
                        

                        _drA = _dtAcct.NewRow();
                        _drA["VendorID"] = _dsA.Tables[0].Rows[0]["ID"].ToString();
                        _drA["VendorAcct"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
                        _dtAcct.Rows.Add(_drA);

                        var rowCount = 0;
                        var totalRows = dti.Rows.Count;
                        if (reportName.Contains("-"))
                        {
                            try
                            {
                                string[] reportNameArr = reportName.Split('-');
                                rowCount = Convert.ToInt32(reportNameArr[1].ToString().Trim().TrimStart());
                                if (totalRows < rowCount)
                                    rowCount = totalRows;
                            }
                            catch (Exception ex) { rowCount = totalRows; }
                        }
                        else
                            rowCount = 6;
                        var dtiCopy = dti.Copy();
                        DataView dv = dtiCopy.DefaultView;
                        dv.Sort = "Ref asc";
                        DataTable sortedDT = dv.ToTable();
                        var dtCopy = sortedDT.Copy();
                        var firstHalf = dtCopy;
                        var secondHalf = dtCopy;
                        if (dtCopy.Rows.Count > rowCount)
                        {
                            firstHalf = dtCopy.AsEnumerable().Take(rowCount).CopyToDataTable();
                            secondHalf = dtCopy.Clone();
                            if (totalRows > rowCount)
                            {
                                secondHalf = dtCopy.AsEnumerable().Skip(rowCount).Take(totalRows - rowCount).CopyToDataTable();
                            }
                        }
                        else
                        {
                            firstHalf = dtCopy;
                        }

                        DataSet dsCC = new DataSet();
                        User objPropUser = new User();
                        objPropUser.ConnConfig = Session["config"].ToString();
                        

                        if (Session["MSM"].ToString() != "TS")
                        {
                            
                                dsCC = objBL_User.getControl(objPropUser);
                            
                        }
                        else
                        {
                            objPropUser.LocID = Convert.ToInt32(0);
                            
                                dsCC = objBL_User.getControlBranch(objPropUser);
                            

                        }

                        //STIMULSOFT 
                        byte[] buffer1 = null;
                        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + reportName.Trim() + ".mrt");
                        StiReport report = new StiReport();
                        report.Load(reportPathStimul);
                        if (Convert.ToInt16(ViewState["CheckStatus"]).Equals(2))
                        {
                            report.Pages[0].Watermark.Enabled = true;
                            string imagepath = Server.MapPath("images/icons/voidcheck.png");
                            report.Pages[0].Watermark.Image = System.Drawing.Image.FromFile(imagepath);
                            report.Pages[0].Watermark.ImageAlignment = System.Drawing.ContentAlignment.TopCenter;
                            report.Pages[0].Watermark.ShowImageBehind = true;
                        }
                        report.Compile();
                        report["TotalAmountPay"] = AmountPay;

                        
                            report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
                        

                        report["InvoiceCount"] = totalRows;
                        if (dsCC.Tables[0].Rows.Count > 0)
                        {
                            report["CompanyName"] = dsCC.Tables[0].Rows[0]["Name"].ToString();
                            report["CompanyAddress"] = dsCC.Tables[0].Rows[0]["Address"].ToString();
                            report["CompanyCity"] = dsCC.Tables[0].Rows[0]["City"].ToString() + "' " + dsCC.Tables[0].Rows[0]["State"].ToString() + " - " + dsCC.Tables[0].Rows[0]["Zip"].ToString();
                        }
                        DataSet Invoice = new DataSet();
                        DataTable dtInvoice = firstHalf.Copy();
                        dtInvoice.TableName = "Invoice";
                        Invoice.Tables.Add(dtInvoice);
                        Invoice.DataSetName = "Invoice";

                        DataSet Check = new DataSet();
                        DataTable dtCheck = dtpay.Copy();
                        dtCheck.TableName = "Check";
                        Check.Tables.Add(dtCheck);
                        Check.DataSetName = "Check";

                        DataSet ControlBranch = new DataSet();
                        DataTable dtControlBranch = new DataTable();
                        dtControlBranch = dsCC.Tables[0].Copy();
                        ControlBranch.Tables.Add(dtControlBranch);
                        dtControlBranch.TableName = "ControlBranch";
                        ControlBranch.DataSetName = "ControlBranch";

                        DataSet Bank = new DataSet();
                        DataTable _dtBank = dtBank.Copy();
                        _dtBank.TableName = "Bank";
                        Bank.Tables.Add(_dtBank);
                        Bank.DataSetName = "Bank";

                        DataSet Account = new DataSet();
                        DataTable dtAccount = _dtAcct.Copy();
                        dtAccount.TableName = "Account";
                        Account.Tables.Add(dtAccount);
                        Account.DataSetName = "Account";

                        report.RegData("Invoice", Invoice);
                        report.RegData("Check", Check);
                        report.RegData("Bank", Bank);
                        report.RegData("Account", Account);
                        report.Render();

                        var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                        var service = new Stimulsoft.Report.Export.StiPdfExportService();
                        System.IO.MemoryStream stream = new System.IO.MemoryStream();
                        service.ExportTo(report, stream, settings);
                        buffer1 = stream.ToArray();
                        lstbyte.Add(buffer1);

                        if (totalRows > rowCount)
                        {
                            byte[] bufferNew = null;
                            reportPathStimul = Server.MapPath("StimulsoftReports/TopCheckSubReport.mrt");
                            report = new StiReport();
                            report.Load(reportPathStimul);
                            report.Compile();

                            
                                report["TotalAmountPay"] = Convert.ToDouble(_dsCheck.Tables[0].Rows[0]["AmountPay"].ToString());
                                report["AccountNo"] = "";
                                report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
                            

                            report["InvoiceCount"] = totalRows;
                            Invoice = new DataSet();
                            dtInvoice = secondHalf.Copy();
                            dtInvoice.TableName = "Invoice";
                            Invoice.Tables.Add(dtInvoice);
                            Invoice.DataSetName = "Invoice";

                            Check = new DataSet();
                            dtCheck = dtpay.Copy();
                            dtCheck.TableName = "Check";
                            Check.Tables.Add(dtCheck);
                            Check.DataSetName = "Check";

                            ControlBranch = new DataSet();
                            dtControlBranch = new DataTable();
                            dtControlBranch = dsCC.Tables[0].Copy();
                            ControlBranch.Tables.Add(dtControlBranch);
                            dtControlBranch.TableName = "ControlBranch";
                            ControlBranch.DataSetName = "ControlBranch";

                            Bank = new DataSet();
                            _dtBank = dtBank.Copy();
                            _dtBank.TableName = "Bank";
                            Bank.Tables.Add(_dtBank);
                            Bank.DataSetName = "Bank";

                            Account = new DataSet();
                            dtAccount = _dtAcct.Copy();
                            dtAccount.TableName = "Account";
                            Account.Tables.Add(dtAccount);
                            Account.DataSetName = "Account";

                            report.RegData("Invoice", Invoice);
                            report.RegData("Check", Check);
                            report.RegData("Bank", Bank);
                            report.RegData("Account", Account);
                            report.Render();

                            settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                            service = new Stimulsoft.Report.Export.StiPdfExportService();
                            stream = new System.IO.MemoryStream();
                            service.ExportTo(report, stream, settings);
                            bufferNew = stream.ToArray();

                            lstbyteNew.Add(bufferNew);
                        }
                    }
                    count++;
                }

                _dtAcct.Reset();
                dti.Reset();
                dtpay.Reset();
                dtBank.Reset();
            }

            byte[] finalbyte = null;

            if (lstbyteNew.Count != 0)
            {
                finalbyte = WriteChecks.concatAndAddContentFinal(lstbyte, lstbyteNew);
            }
            else
            {
                finalbyte = WriteChecks.concatAndAddContent(lstbyte);
            }

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment;filename=ApTopCheckCub.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (finalbyte.Length).ToString());
            Response.BinaryWrite(finalbyte);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillReportMiddleDataSet(string reportName)
    {
        try
        {
            int count = 0;
            _objCD.ConnConfig = Session["config"].ToString();
            _objCD.Ref = long.Parse(txtcheckfrom.Text);
            _objCD.NextC = long.Parse(txtcheckto.Text);
            _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);

            

            DataSet _dsCheck = new DataSet();
            DataSet _dsCheck1 = new DataSet();
            DataSet _dsCheck2 = new DataSet();

            
                _dsCheck = _objBLBill.GetCheckDetailsByBankAndRef(_objCD);
            



            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("Name");
            dtNew.Columns.Add("Vendor");
            dtNew.Columns.Add("CheckNo");

            
                foreach (DataRow drow in _dsCheck.Tables[0].Rows)
                {
                    DataRow drNew = dtNew.NewRow();
                    drNew["Name"] = drow["VendorName"].ToString();
                    drNew["Vendor"] = drow["Vendor"].ToString();
                    drNew["CheckNo"] = drow["CheckNo"].ToString();
                    dtNew.Rows.Add(drNew);
                }
            

            DataTable dtN = dtNew.DefaultView.ToTable(true);
            DataTable _dtAcct = new DataTable();
            foreach (DataRow dr in dtN.Rows)
            {
                bool isChecked = true;
                _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
                _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

                CreateTableInvoice();
                CreateTablePayee();
                CreateTableBank();

                DataRow _dri = null;
                DataRow _drC = null;

                int vid = Convert.ToInt32(dr["Vendor"].ToString());
                string checkNo = Convert.ToString(dr["CheckNo"].ToString());

                //RAHIL'S IMPLEMENTATION
                double AmountPay = 0.00;

                DataView dtInv = new DataView();
                
                    dtInv = _dsCheck.Tables[0].DefaultView;
                

                dtInv.RowFilter = "Vendor = '" + vid + "' and CheckNo = '" + checkNo + "'";
                foreach (DataRow drow in dtInv.ToTable(true).Rows)
                {
                    _dri = dti.NewRow();
                    _dri["Ref"] = drow["Ref"].ToString();
                    _dri["Description"] = drow["Description"].ToString();
                    _dri["InvoiceDate"] = drow["InvoiceDate"].ToString();
                    _dri["Reference"] = drow["Refrerence"].ToString();
                    _dri["Total"] = double.Parse(drow["Total"].ToString().Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                    _dri["Disc"] = Convert.ToDouble(drow["Disc"].ToString()).ToString();
                    _dri["AmountPay"] = Convert.ToDouble(drow["AmountPay"].ToString()).ToString();
                    AmountPay = AmountPay + Convert.ToDouble(drow["AmountPay"].ToString());
                    _dri["PayDate"] = drow["PayDate"].ToString();
                    _dri["CheckNo"] = drow["CheckNo"].ToString();
                    _dri["VendorID"] = drow["Vendor"].ToString();
                    _dri["VendorName"] = drow["VendorName"].ToString();
                    dti.Rows.Add(_dri);

                    dti.AcceptChanges();
                }
                if (isChecked)
                {
                    if (dti.Rows.Count > 0)
                    {
                        DataView dtcheck = new DataView();
                        
                            dtcheck = _dsCheck.Tables[1].DefaultView;
                        

                        dtcheck.RowFilter = "Vendor = '" + vid + "' and CheckNo = '" + checkNo + "'";
                        ViewState["CheckStatus"] = "0";
                        foreach (DataRow drow in dtcheck.ToTable(true).Rows)
                        {
                            _drC = dtpay.NewRow();
                            if (Convert.ToDouble(drow["Pay"]) > 1000)
                            {
                                _drC["Pay"] = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                            }
                            else
                            {
                                string dollar = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                                _drC["Pay"] = dollar + " Dollars";
                            }
                            _drC["ToOrder"] = drow["ToOrder"].ToString();
                            _drC["Date"] = drow["Date"].ToString();
                            _drC["CheckAmount"] = Convert.ToDouble(drow["Pay"]);
                            _drC["VendorAddress"] = drow["VendorAddress"].ToString();
                            _drC["RemitAddress"] = drow["RemitAddress"].ToString();
                            _drC["State"] = drow["State"].ToString();
                            _drC["Zip"] = drow["Zip"].ToString();
                            ViewState["CheckStatus"] = drow["Status"].ToString();
                            dtpay.Rows.Add(_drC);
                        }

                        var rowCount = 0;
                        var totalRows = dti.Rows.Count;
                        if (reportName.Contains("-"))
                        {
                            try
                            {
                                string[] reportNameArr = reportName.Split('-');
                                rowCount = Convert.ToInt32(reportNameArr[1].ToString().Trim().TrimStart());
                                if (totalRows < rowCount)
                                    rowCount = totalRows;
                            }
                            catch (Exception ex) { rowCount = totalRows; }
                        }
                        else
                            rowCount = 6;

                        var dtiCopy = dti.Copy();
                        DataView dv = dtiCopy.DefaultView;
                        dv.Sort = "Ref asc";
                        DataTable sortedDT = dv.ToTable();
                        var dtCopy = sortedDT.Copy();
                        var firstHalf = dtCopy;
                        var secondHalf = dtCopy;
                        if (dtCopy.Rows.Count > rowCount)
                        {
                            firstHalf = dtCopy.AsEnumerable().Take(rowCount).CopyToDataTable();
                            secondHalf = dtCopy.Clone();
                            if (totalRows > rowCount)
                            {
                                secondHalf = dtCopy.AsEnumerable().Skip(rowCount).Take(totalRows - rowCount).CopyToDataTable();
                            }
                        }
                        else
                        {
                            firstHalf = dtCopy;
                        }

                        //STIMULSOFT 
                        byte[] buffer1 = null;
                        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + reportName.Trim() + ".mrt");
                        StiReport report = new StiReport();
                        report.Load(reportPathStimul);
                        if (Convert.ToInt16(ViewState["CheckStatus"]).Equals(2))
                        {
                            report.Pages[0].Watermark.Enabled = true;
                            string imagepath = Server.MapPath("images/icons/voidcheck.png");
                            report.Pages[0].Watermark.Image = System.Drawing.Image.FromFile(imagepath);
                            report.Pages[0].Watermark.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                            report.Pages[0].Watermark.ShowImageBehind = true;
                        }
                        report.Compile();
                        report["TotalAmountPay"] = AmountPay;

                        
                            report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
                        

                        report["InvoiceCount"] = totalRows;
                        DataSet Invoice = new DataSet();
                        DataTable dtInvoice = firstHalf.Copy();
                        dtInvoice.TableName = "Invoice";
                        Invoice.Tables.Add(dtInvoice);
                        Invoice.DataSetName = "Invoice";

                        DataSet Check = new DataSet();
                        DataTable dtCheck = dtpay.Copy();
                        dtCheck.TableName = "Check";
                        Check.Tables.Add(dtCheck);
                        Check.DataSetName = "Check";

                        report.RegData("Invoice", Invoice);
                        report.RegData("Check", Check);

                        report.Render();
                        var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                        var service = new Stimulsoft.Report.Export.StiPdfExportService();
                        System.IO.MemoryStream stream = new System.IO.MemoryStream();
                        service.ExportTo(report, stream, settings);
                        buffer1 = stream.ToArray();
                        lstbyte.Add(buffer1);

                        if (totalRows > rowCount)
                        {
                            byte[] bufferNew = null;
                            reportPathStimul = Server.MapPath("StimulsoftReports/TopCheckSubReport.mrt");
                            report = new StiReport();
                            report.Load(reportPathStimul);
                            report.Compile();

                            
                                report["TotalAmountPay"] = Convert.ToDouble(_dsCheck.Tables[0].Rows[0]["AmountPay"].ToString());
                                report["AccountNo"] = "";
                                report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
                            

                            report["InvoiceCount"] = totalRows;
                            Invoice = new DataSet();
                            dtInvoice = secondHalf.Copy();
                            dtInvoice.TableName = "Invoice";
                            Invoice.Tables.Add(dtInvoice);
                            Invoice.DataSetName = "Invoice";

                            Check = new DataSet();
                            dtCheck = dtpay.Copy();
                            dtCheck.TableName = "Check";
                            Check.Tables.Add(dtCheck);
                            Check.DataSetName = "Check";

                            report.RegData("Invoice", Invoice);
                            report.RegData("Check", Check);
                            report.Render();
                            settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                            service = new Stimulsoft.Report.Export.StiPdfExportService();
                            stream = new System.IO.MemoryStream();
                            service.ExportTo(report, stream, settings);
                            bufferNew = stream.ToArray();

                            lstbyteNew.Add(bufferNew);
                        }
                    }

                    count++;
                }

                _dtAcct.Reset();
                dti.Reset();
                dtpay.Reset();
                dtBank.Reset();
            }

            byte[] finalbyte = null;

            if (lstbyteNew.Count != 0)
            {
                finalbyte = WriteChecks.concatAndAddContentFinal(lstbyte, lstbyteNew);
            }
            else
            {
                finalbyte = WriteChecks.concatAndAddContent(lstbyte);
            }

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment;filename=ApTopCheckCub.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (finalbyte.Length).ToString());
            Response.BinaryWrite(finalbyte);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    public string ConvertNumberToCurrency(double _amount)
    {
        string _currencyInWord = ConvertNumbertoWords(Convert.ToInt32(Math.Truncate(_amount)));
        double d = _amount - Math.Truncate(_amount);
        if (d > 0)
        {
            d = Math.Round(d * 100);
            _currencyInWord = _currencyInWord + " And " + d.ToString() + " / 100";
        }
        _currencyInWord = "*** " + _currencyInWord + "****************";
        return _currencyInWord;
    }
    public static string ConvertNumbertoWords(int number)
    {
        if (number == 0)
            return "Zero";
        if (number < 0)
            return "minus " + ConvertNumbertoWords(Math.Abs(number));
        string words = "";
        if ((number / 1000000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000000) + " Million ";
            number %= 1000000;
        }
        if ((number / 1000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000) + " Thousand ";
            number %= 1000;
        }
        if ((number / 100) > 0)
        {
            words += ConvertNumbertoWords(number / 100) + " Hundred ";
            number %= 100;
        }
        if (number > 0)
        {
            if (words != "")
                words += "And ";
            //var unitsMap = new[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };
            //var tensMap = new[] { "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };
            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };


            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }
        return words;
    }
    private void CreateTableInvoice()
    {

        dti.Columns.Add(new DataColumn("Ref", typeof(string)));
        dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        dti.Columns.Add(new DataColumn("Reference", typeof(string)));
        dti.Columns.Add(new DataColumn("Total", typeof(string)));
        dti.Columns.Add(new DataColumn("Disc", typeof(string)));
        dti.Columns.Add(new DataColumn("AmountPay", typeof(string)));
        dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
        dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
        dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        dti.Columns.Add(new DataColumn("VendorName", typeof(string)));
        dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
        dti.Columns.Add(new DataColumn("Description", typeof(string)));
    }
    private void CreateTablePayee()
    {
        dtpay.Columns.Add(new DataColumn("Pay", typeof(string)));
        dtpay.Columns.Add(new DataColumn("ToOrder", typeof(string)));
        dtpay.Columns.Add(new DataColumn("Date", typeof(string)));
        dtpay.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        dtpay.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        dtpay.Columns.Add(new DataColumn("State", typeof(string)));
        dtpay.Columns.Add(new DataColumn("Zip", typeof(string)));
        dtpay.Columns.Add(new DataColumn("VendorAddress", typeof(string)));
        dtpay.Columns.Add(new DataColumn("RemitAddress", typeof(string)));
    }
    private void CreateTableBank()
    {
        dtBank.Columns.Add(new DataColumn("Name", typeof(string)));
        dtBank.Columns.Add(new DataColumn("Address", typeof(string)));
        dtBank.Columns.Add(new DataColumn("City", typeof(string)));
        dtBank.Columns.Add(new DataColumn("State", typeof(string)));
        dtBank.Columns.Add(new DataColumn("Zip", typeof(string)));
        dtBank.Columns.Add(new DataColumn("NBranch", typeof(string)));
        dtBank.Columns.Add(new DataColumn("NAcct", typeof(string)));
        dtBank.Columns.Add(new DataColumn("NRoute", typeof(string)));
        dtBank.Columns.Add(new DataColumn("Ref", typeof(string)));
    }
    protected void btnSubmit_Click1(object sender, EventArgs e)
    {


        try
        {

            string script = "function f(){$find(\"" + RadWindowTemplates.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

        }
        catch (Exception ex)
        {
            string str = "These month/year period is closed out. You do not have permission to process the check.";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  noty({text: '" + str + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});setTimeout(function(){ location.reload(); }, 5000);", true);
        }

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        

    }
    protected void lnkprintchecktemp_Click(object sender, EventArgs e)
    {
        //try
        //{
        //DataTable dt = GetTransaction();

        DataTable dt = GetTable();
        string code = "";
        string empgeocode = "000000000";
        string workgeocode = GetWorkGeoCode();
        double FITdouble = 0;
        double SITdouble = 0;
        double Localdouble = 0;
        double Citydouble = 0;
        double Medidouble = 0;
        double Ficadouble = 0;
        string uname = "";
        string strerrorMessage = "";
        //try
        //{


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
                //if (!dict.ContainsKey("chkSelect"))
                //
                //    if (dict["chkSelect"].ToString().Trim() == string.Empty || dict["chkSelect"].ToString() == "on")
                //{ 
                //    continue;
                //}
                if (dict.ContainsKey("chkSelect"))
                {
                    i++;
                    DataRow dr = dt.NewRow();
                    uname = dict["hdnName"].ToString().Trim();
                    if (uname == "Muehling , Christopher")
                    {
                        uname = dict["hdnName"].ToString().Trim();
                    }

                    //if (uname == "Anderson-Tyner , Robert")
                    //{



                    dr["ID"] = dict["hdnid"].ToString().Trim();
                    dr["Name"] = dict["hdnName"].ToString().Trim();
                    dr["Reg"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnlblReg"].ToString().Trim());
                    dr["OT"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnlblOT"].ToString().Trim());
                    dr["NT"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnlblNT"].ToString().Trim());
                    dr["DT"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnlblDT"].ToString().Trim());
                    dr["TT"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnlblTT"].ToString().Trim());
                    dr["Zone"] = ConvertCurrentCurrencyFormatToDbl(dict["txtZone"].ToString().Trim());
                    dr["Milage"] = ConvertCurrentCurrencyFormatToDbl(dict["txtMilage"].ToString());

                    //dr["Toll"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnToll"].ToString());
                    dr["Toll"] = 0.00;
                    dr["OtherE"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnOtherE"].ToString());
                    dr["pay"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnpay"].ToString());

                    dr["holiday"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnlblHoliday"].ToString().Trim());
                    dr["vacation"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnlblVac"].ToString().Trim());
                    dr["sicktime"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnsicktime"].ToString().Trim());
                    dr["reimb"] = ConvertCurrentCurrencyFormatToDbl(dict["txtReimb"].ToString().Trim());
                    dr["bonus"] = ConvertCurrentCurrencyFormatToDbl(dict["txtBonus"].ToString());
                    if (dict["hdnlblMethod"].ToString().Trim() != null)
                    {
                        dr["paymethod"] = dict["hdnlblMethod"].ToString();
                    }
                    else
                    {
                        dr["paymethod"] = "";
                    }
                    if (dict["hdnpmethod"].ToString().Trim() != null)
                    {
                        dr["pmethod"] = dict["hdnpmethod"].ToString();
                    }
                    else
                    {
                        dr["pmethod"] = "0";
                    }



                    dr["userid"] = dict["hdnuserid"].ToString();
                    dr["usertype"] = dict["hdnusertype"].ToString();
                    if (dict["hdntotal"].ToString().Trim() != null && dict["hdntotal"].ToString().Trim() != "")
                    {
                        dr["total"] = dict["hdntotal"].ToString();
                    }
                    else
                    {
                        dr["total"] = 0.00;
                    }
                    if (dict["hdnphour"].ToString().Trim() != null && dict["hdntotal"].ToString().Trim() != "")
                    {
                        dr["phour"] = dict["hdnphour"].ToString();
                    }
                    else
                    {
                        dr["phour"] = 0.00;
                    }
                    if (dict["hdnsalary"].ToString().Trim() != null && dict["hdntotal"].ToString().Trim() != "")
                    {
                        dr["salary"] = dict["hdnsalary"].ToString();
                    }
                    else
                    {
                        dr["salary"] = 0.00;
                    }
                    if (dict["hdnHourlyRate"].ToString().Trim() != null && dict["hdntotal"].ToString().Trim() != "")
                    {
                        dr["HourlyRate"] = dict["hdnHourlyRate"].ToString();
                    }
                    else
                    {
                        dr["HourlyRate"] = 0.00;
                    }


                    ///////////////// HERE CALL API FOR FIT/SIT ////////////////////////
                    //dr["FIT"] = "0";
                    //dr["SIT"] = "0";
                    //dr["LOCAL"] = "0";
                    //////////////////////////////////////////
                    //Credentials for Payroll OnDemand
                    string username = "dread@1000";
                    string passWord = "K3CHccxQ";
                    // Setting up the URI for Payroll
                    string uri = "https://payrollsandbox.ondemand.vertexinc.com:443/EiWebSvc/AddressWebService";

                    DataSet dsemp = new DataSet();
                    _objEmp.ConnConfig = Session["config"].ToString();
                    _objEmp.ID = Convert.ToInt32(dict["hdnid"].ToString().Trim());
                    dsemp = objBL_Wage.GetEmployeeListByID(_objEmp);
                    DataRow _dr = dsemp.Tables[0].Rows[0];

                    string addrClnXML = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:eic = \"http://EiCalc/\">"
                        + "<soapenv:Header/>"
                        + "<soapenv:Body>"
                        + "<eic:AddrCleanse>"
                        + "<Request>"
                        + "<![CDATA["
                            + "<ADDRESS_CLEANSE_REQUEST>"
                                + "<StreetAddress1>" + _dr["Address"].ToString() + "</StreetAddress1>"
                                + "<CityName>" + _dr["City"].ToString() + "</CityName>"
                                + "<StateName>" + _dr["State"].ToString() + "</StateName>"
                                + "<ZipCode>" + _dr["Zip"].ToString() + "</ZipCode>"
                            + "</ADDRESS_CLEANSE_REQUEST>]]>"
                        + "</Request>"
                        + "</eic:AddrCleanse>"
                        + "</soapenv:Body>"
                        + "</soapenv:Envelope>";

                    //try
                    //{
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    HttpClient cl = new HttpClient();
                    cl.BaseAddress = new Uri(uri);
                    cl.DefaultRequestHeaders.Clear();
                    cl.DefaultRequestHeaders.Add("javax.xml.ws.security.auth.username", username);
                    cl.DefaultRequestHeaders.Add("javax.xml.ws.security.auth.password", passWord);
                    HttpContent soapAddressEnvelope = new StringContent(addrClnXML);
                    soapAddressEnvelope.Headers.Clear();
                    soapAddressEnvelope.Headers.Add("Content-Type", "text/xml");
                    using (HttpResponseMessage response = cl.PostAsync(uri, soapAddressEnvelope).Result)
                    {
                        string rawString = getMessage(response).Result;
                        
                        XmlDocument responseXML = new XmlDocument();
                        responseXML.LoadXml(rawString);
                        XDocument responseXMLPretty = XDocument.Parse(responseXML.InnerText.ToString());

                        string responseXMLPrettystr = responseXMLPretty.ToString();
                        responseXMLPrettystr = responseXMLPrettystr.Replace("\"", "'");
                        XmlDocument Doc = new XmlDocument();
                        Doc.LoadXml(responseXMLPrettystr);

                        XmlNode Errornode = Doc.GetElementsByTagName("Error").Item(0);
                        if (Errornode == null)
                        {
                            XmlNode nodedoc = Doc.GetElementsByTagName("GeoCode").Item(0);
                            if (nodedoc != null)
                            {
                                empgeocode = nodedoc.ChildNodes.Item(0).InnerText;
                            }
                            else
                            {
                                strerrorMessage = "Null Exception for " + uname;
                                break;
                            }

                        }
                        else
                        {
                            strerrorMessage = "Error in Address Geocode for " + uname;
                            break;

                        }

                        ////Console.WriteLine(responseXMLPretty.ToString());
                        //XmlNode node = responseXML.GetElementsByTagName("GeoCode").Item(0);
                        //empgeocode = node.ChildNodes.Item(0).InnerText;

                        soapAddressEnvelope.Dispose();
                        cl.Dispose();
                    }
                    ///////////////////////////////////////////////////////////////////
                    string payrolluri = "https://payrollsandbox.ondemand.vertexinc.com:443/EiWebSvc/PayTaxCalcWebService";

                    //string payrollXML = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:eic = \"http://EiCalc/\">"
                    //    + "<soapenv:Header/>"
                    //    + "<soapenv:Body>"
                    //    + "<eic:EiOperation>"
                    //    + "<Request>"
                    //    + "<![CDATA["
                    //    + "<EMP><EMPINFO><EMPSTRUCT><EMPID>DAVE</EMPID>"
                    //    + "<PAYDATE>20200101</PAYDATE><PAYPERIODS>" + 1 + "</PAYPERIODS><CURPERIOD>1</CURPERIOD><RES_GEO>" + empgeocode + "</RES_GEO><PRIMARY_WORK_GEO>" + workgeocode + "</PRIMARY_WORK_GEO>"
                    //    + "</EMPSTRUCT></EMPINFO><WRK><WRKINFO><GEO>100991210</GEO></WRKINFO><CMPARRAY><CMP><ID>0</ID><TYPE>r</TYPE><Amt>" + dict["hdntotal"].ToString() + "</Amt></CMP></CMPARRAY></WRK></EMP>]]>"
                    //    + "</Request>"
                    //    + "</eic:EiOperation>"
                    //    + "</soapenv:Body>"
                    //    + "</soapenv:Envelope>";
                    string payrollXML = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:eic=\"http://EiCalc/\">"
                        + "<soapenv:Header/>"
                        + "<soapenv:Body>"
                        + "<eic:EiOperation>"
                          + "<Request>"
           + "<![CDATA["
    + "<EMP>"
       + "<EMPINFO>"
           + "<EMPSTRUCT>"
               //+ "<EMPID>" + _dr["fFirst"].ToString() + "</EMPID>"
               + "<EMPID>ESS</EMPID>"
               + "<PAYDATE>" + Convert.ToDateTime(txtcheckdate.Text).Year.ToString() + Convert.ToDateTime(txtcheckdate.Text).Month.ToString() + Convert.ToDateTime(txtcheckdate.Text).Day.ToString() + "</PAYDATE>";
                    if (_dr["PayPeriod"].ToString() == "0") //Weekly
                    {
                        payrollXML = payrollXML + "<PAYPERIODS>52</PAYPERIODS>";
                    }
                    else
                    {
                        payrollXML = payrollXML + "<PAYPERIODS>1</PAYPERIODS>";
                    }
                    payrollXML = payrollXML + "<CURPERIOD>1</CURPERIOD>"
           + "<RES_GEO>" + empgeocode + "</RES_GEO>"
           + "<PRIMARY_WORK_GEO>" + workgeocode + "</PRIMARY_WORK_GEO>"
       + "</EMPSTRUCT>"
   + "</EMPINFO>"
   + "<WRK>"
       + "<WRKINFO>"
        + "<GEO>" + workgeocode + "</GEO>"
       + "</WRKINFO>"
       + "<CMPARRAY>"
           + "<CMP>"
               + "<ID>0</ID>"
               + "<TYPE>r</TYPE>"
               + "<Amt>" + dict["hdntotal"].ToString() + "</Amt>"
           //+ "<Amt>1678.00</Amt>"
           + "</CMP>"
       + "</CMPARRAY>"
   + "</WRK>"
   + "<JURARRAY>"
       + "<JUR>"
           + "<TAXID>400</TAXID>"
           + "<GEO>0</GEO>";
                    if (_dr["FStatus"].ToString() == "1") //Married
                    {
                        payrollXML = payrollXML + "<FILING_STAT>2</FILING_STAT>"; //Married
                    }
                    else
                    {
                        payrollXML = payrollXML + "<FILING_STAT>1</FILING_STAT>";
                    }

                    payrollXML = payrollXML + "<PRI_EXEMPT>" + _dr["FAllow"].ToString() + "</PRI_EXEMPT>"
           + "<CALCMETH>0</CALCMETH>"
           + "<SUPL_METH>0</SUPL_METH>"
       + "</JUR>"
   + "</JURARRAY>"
   + "<TAXAMTARRAY>"
       + "<AGGTAX>"
           + "<TAXID>400</TAXID>"
           + "<GEO>0</GEO>"
           + "<TAX_AMT>0</TAX_AMT>"
           + "<AGG_TYPE>Y</AGG_TYPE>"
           + "<TYPE>r</TYPE>"
           + "<AGG_ADJ_GROSS>0</AGG_ADJ_GROSS>"
       + "</AGGTAX>"
   + "</TAXAMTARRAY>"
+ "</EMP>"
+ "]]>"
       + "</Request>"
      + "</eic:EiOperation>"
     + "</soapenv:Body>"
    + "</soapenv:Envelope> ";
                    //try
                    //{
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    HttpClient clpay = new HttpClient();
                    clpay.BaseAddress = new Uri(payrolluri);
                    clpay.DefaultRequestHeaders.Clear();
                    clpay.DefaultRequestHeaders.Add("javax.xml.ws.security.auth.username", username);
                    clpay.DefaultRequestHeaders.Add("javax.xml.ws.security.auth.password", passWord);
                    HttpContent soapPayrollCalcTax = new StringContent(payrollXML);
                    soapPayrollCalcTax.Headers.Clear();
                    soapPayrollCalcTax.Headers.Add("Content-Type", "text/xml");
                    using (HttpResponseMessage response = clpay.PostAsync(payrolluri, soapPayrollCalcTax).Result)
                    {
                        string rawString = getMessage(response).Result;
                        
                        XmlDocument responseXML = new XmlDocument();
                        responseXML.LoadXml(rawString);
                        XDocument responseXMLPretty = XDocument.Parse(responseXML.InnerText.ToString());

                        string responseXMLPrettystrr = responseXMLPretty.ToString();
                        responseXMLPrettystrr = responseXMLPrettystrr.Replace("\"", "'");
                        XmlDocument Docc = new XmlDocument();
                        Docc.LoadXml(responseXMLPrettystrr);
                        //XmlNode nodedocc = Docc.GetElementsByTagName("TaxOut").Item(0);

                        XmlNode Errornode = Docc.GetElementsByTagName("Error").Item(0);
                        if (Errornode == null)
                        {
                            XmlNodeList elemList = Docc.GetElementsByTagName("TaxOut");
                            for (int j = 0; j < elemList.Count; j++)
                            {
                                //Console.WriteLine(elemList[j]["TAXID"].InnerText);
                                //Console.WriteLine(elemList[i].InnerText);
                                if (elemList[j]["TAXID"].InnerText == "400")
                                {
                                    //Console.WriteLine(elemList[j]["TAX_AMT"].InnerText);
                                    FITdouble = Convert.ToDouble(elemList[j]["TAX_AMT"].InnerText);
                                }
                                if (elemList[j]["TAXID"].InnerText == "450")
                                {
                                    SITdouble = Convert.ToDouble(elemList[j]["TAX_AMT"].InnerText);
                                }
                                if (elemList[j]["TAXID"].InnerText == "530")
                                {
                                    Citydouble = Convert.ToDouble(elemList[j]["TAX_AMT"].InnerText);
                                }
                                if (elemList[j]["TAXID"].InnerText == "406")
                                {
                                    Medidouble = Convert.ToDouble(elemList[j]["TAX_AMT"].InnerText);
                                }
                                if (elemList[j]["TAXID"].InnerText == "403")
                                {
                                    Ficadouble = Convert.ToDouble(elemList[j]["TAX_AMT"].InnerText);
                                }
                                
                            }
                        }
                        else
                        {
                            strerrorMessage = "Error in Address API for " + uname;
                            break;

                        }



                        //Console.WriteLine(responseXMLPretty.ToString());
                        //XmlNode node = responseXML.GetElementsByTagName("GeoCode").Item(0);
                        //empgeocode = node.ChildNodes.Item(0).InnerText;
                        soapPayrollCalcTax.Dispose();
                        clpay.Dispose();
                    }
                    //}

                    //catch (AggregateException aggEx)
                    //{
                    //    Console.WriteLine("A Connection error occurred");
                    //    Console.WriteLine("----------------------------------------------------");
                    //    Console.WriteLine("Error Code: " + aggEx.Message.ToString() + Environment.NewLine + "Message: " + aggEx.InnerException.Message.ToString());

                    //}
                    ///////////////////////////////////////////////////////////////////

                    //}

                    //catch (AggregateException aggEx)
                    //{
                    //    //If an error occurs outside of the service call capture and show below
                    //    Console.WriteLine("A Connection error occurred");
                    //    Console.WriteLine("----------------------------------------------------");
                    //    Console.WriteLine("Error Code: " + aggEx.Message.ToString() + Environment.NewLine + "Message: " + aggEx.InnerException.Message.ToString());

                    //}


                    //Console.ReadLine();



                    // Below call to read the message back from server from stream to string
                    async Task<string> getMessage(HttpResponseMessage messageFromServer)
                    {
                        code = await messageFromServer.Content.ReadAsStringAsync();
                        return code;
                    }

                    //////////////////////////////////////////
                    dr["FIT"] = FITdouble;
                    dr["SIT"] = SITdouble;
                    dr["LOCAL"] = Localdouble;
                    dr["MEDI"] = Medidouble;
                    dr["FICA"] = Ficadouble;
                    dt.Rows.Add(dr);
                    //}
                }
            }
        }
        //}
        //catch (Exception ex)
        //{
        //    string opo = uname;
        //    //throw ex;

        //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelServyp", "noty({text: '" + str + "',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        //}
        if (strerrorMessage != "")
        {
            dt.Clear();
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  noty({text: '" + strerrorMessage + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);

        }
        


        //dt.AcceptChanges();
        if (dt.Rows.Count > 0)
        {
            _objPRReg.Dt = dt;
            _objPRReg.ConnConfig = Session["config"].ToString();
            //_objPRReg.StartDate = Convert.ToDateTime(txtstartDt.Text);
            //_objPRReg.EndDate = Convert.ToDateTime(txtendDt.Text);
            _objPRReg.CDate = Convert.ToDateTime(txtcheckdate.Text);
            _objPRReg.Bank = Convert.ToInt32(ddlBank.SelectedValue);
            _objPRReg.Memo = Convert.ToString(txtcheckmemo.Text);
            //_objPRReg.WeekNo = Convert.ToInt32(txtweek.Text);
            //_objPRReg.Description = Convert.ToString(txtperioddesc.Text);
            //_objPRReg.ProcessMethod = Convert.ToString(ddlGetTimeMethod.SelectedValue);
            //_objPRReg.Supervisor = Convert.ToString(ddlSuper.SelectedValue);
            _objPRReg.PrcessDed = Convert.ToInt32("0");
            _objPRReg.Checkno = long.Parse(txtcheck.Text);
            _objPRReg.MOMUSer = Session["User"].ToString();


            objBL_Wage.ProcessPayroll(_objPRReg);
            string str = "Payroll process is done.";
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  noty({text: '" + str + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});setTimeout(function(){ location.reload(); }, 5000);", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  noty({text: '" + str + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false}); OpentemplateModal();", true);
            //}
            //catch (Exception ex)
            //{
            //    string str = ex.Message.ToString();
            //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  noty({text: '" + str + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            //}
        }
    }
    private void AddBill()
    {
       
            
        
    }

    private void FillBank()
    {
        try
        {
                _objBank.ConnConfig = Session["config"].ToString();
            
                DataSet _dsBank = new DataSet();
                _dsBank = _objBL_Bank.GetAllBankNames(_objBank);
                
                if (_dsBank.Tables[0].Rows.Count > 0)
                {
                    ddlBank.Items.Clear();
                    ddlBank.Items.Add(new System.Web.UI.WebControls.ListItem(":: Select ::", "0"));
                    ddlBank.AppendDataBoundItems = true;

                    ddlBank.DataSource = _dsBank;
                    ddlBank.DataValueField = "ID";
                    ddlBank.DataTextField = "fDesc";
                    ddlBank.DataBind();
                }
                else
                {
                    ddlBank.Items.Add(new System.Web.UI.WebControls.ListItem("No data found", "0"));
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