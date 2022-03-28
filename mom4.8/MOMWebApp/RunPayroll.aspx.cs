using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using Telerik.Web.UI;
using System.Linq.Dynamic;
using Telerik.Web.UI.GridExcelBuilder;
using BusinessEntity.APModels;
using Stimulsoft.Report;
using Microsoft.Reporting.WebForms;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ACHBAL;
using context = System.Web.HttpContext;
using System.Web.Configuration;
using Renci.SshNet;
using System.Configuration;

public partial class RunPayroll : System.Web.UI.Page
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
    General _objPropGeneral = new General();
    BL_General _objBLGeneral = new BL_General();

    PayrollRegisterModel _payrollRegister = new PayrollRegisterModel();
    public static string EmployeeIds = null;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!CheckAddEditPermission()) { Response.Redirect("Home.aspx?permission=no"); return; }

        if (this.IsPostBack)
            return;

        if (!IsPostBack)
        {
            string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();
            if (Request.Url.Scheme == "http" && SSL == "1")
            {
                string URL = Request.Url.ToString();
                URL = URL.Replace("http://", "https://");
                Response.Redirect(URL);
            }
            chkProcessOtherDeduction.Checked = true;
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
            FillPayFrequency();
            FillFilter();
            FillBank();
            FillDepartment();
            Permission();

            Session["PayrollRegisterId"] = null;
            if (Request.QueryString["id"] == null)
            {
                ViewState["EditPayroll"] = "0";
                Session["PayrollRegisterId"] = null;
                EmployeeIds = null;
            }
            if (Request.QueryString["id"] != null)  //Edit COA
            {
                ViewState["EditPayroll"] = "1";
                Session["PayrollRegisterId"] = Request.QueryString["id"];
                GetPayrollData(Request.QueryString["id"].ToString());
            }

            HighlightSideMenu("prID", "runpayrolllink", "payrollmenutab");
        }
        if (!IsPostBack)
        {
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
            ddlApTopCheckForLoad.DataBind();
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
            ddlApMiddleCheckForLoad.DataBind();
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
            ddlTopChecksForLoad.DataBind();
        }
        CompanyPermission();

    }

    private void GetPayrollData(string id)
    {
        try
        {
            DataSet ds = new DataSet();
            _payrollRegister.ConnConfig = Session["config"].ToString();
            _payrollRegister.PayrollRegisterId = Convert.ToInt32(id);
            ds = objBL_Wage.GetPayrollRegisterById(_payrollRegister);

            DataRow _dr = ds.Tables[0].Rows[0];

            SetRunPayroll(_dr);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "Materialize.updateTextFields();", true);
        }
        catch (Exception ex)
        {
            string type = "error";
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            if (str.Contains("Wage Deduction already exists"))
            {
                type = "warning";
            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddDepttype", "noty({text: '" + str + "', dismissQueue: true,  type : '" + type + "', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void SetRunPayroll(DataRow _dr)
    {
        txtstartDt.Text = _dr["StartDate"].ToString();
        txtendDt.Text = _dr["EndDate"].ToString();
        txtperioddesc.Text = _dr["Description"].ToString();

        ddlPayFrequency.SelectedValue = _dr["FrequencyId"].ToString();
        ddlGetTimeMethod.SelectedValue = _dr["GetTimeMethod"].ToString();
        ddlSuper.SelectedValue = _dr["SupervisorId"].ToString();
        ddlDepartment.SelectedValue = _dr["DepartmentId"].ToString();

        EmployeeIds = _dr["EmployeeIds"].ToString();
        //hdnPayrollRegisterId.Value = _dr["PayrollRegisterId"].ToString();

        RadGrid_RunPayroll.CurrentPageIndex = 0;
        RadGrid_RunPayroll.PageSize = 50;
        GetEmpList();
        RadGrid_RunPayroll.Rebind();
    }
    private void GetEmpList()
    {
        try
        {
            DataSet ds = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            //objPropUser.FStart = Convert.ToDateTime("05/28/2021");
            //objPropUser.Edate = Convert.ToDateTime("06/03/2021");
            objPropUser.FStart = Convert.ToDateTime(txtstartDt.Text);
            objPropUser.Edate = Convert.ToDateTime(txtendDt.Text);
            objPropUser.Supervisor = ddlSuper.SelectedValue.ToString();
            objPropUser.DepartmentID = Convert.ToInt32(ddlDepartment.SelectedValue.ToString());
            objPropUser.PayPeriod = Convert.ToInt32(ddlPayFrequency.SelectedValue.ToString());
            objPropUser.EN = 0;
            objPropUser.ID = 0;
            objPropUser.WorkId = 0;
            if (Session["PayrollRegisterId"] != null)
            {
                objPropUser.ID = Convert.ToInt32(Session["PayrollRegisterId"].ToString());
            }
            ds = new BL_Wage().GetRunPayroll(objPropUser);
            ds.Tables[0].Columns.Add("Reg_Holi", typeof(Double));
            ds.Tables[0].Columns.Add("OtherHr", typeof(Double));
            ds.Tables[0].Columns.Add("TotalHr", typeof(Double));
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr["Reg_Holi"] = Convert.ToDouble(dr["Reg"].ToString()) + Convert.ToDouble(dr["holiday"].ToString());
                    dr["OtherHr"] = Convert.ToDouble(dr["OT"].ToString()) + Convert.ToDouble(dr["DT"].ToString()) + Convert.ToDouble(dr["NT"].ToString()) + Convert.ToDouble(dr["TT"].ToString());
                    dr["TotalHr"] = Convert.ToDouble(dr["Reg_Holi"].ToString()) + Convert.ToDouble(dr["OtherHr"].ToString());
                }
            }
            RadGrid_RunPayroll.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGrid_RunPayroll.DataSource = ds.Tables[0];
            ViewState["VirtualItemCount"] = ds.Tables[0].Rows.Count;
            lblRecordCount.Text = ds.Tables[0].Rows.Count + " Record(s) found";
            Session["RunPayrollList"] = ds.Tables[0];
            //SaveFilter();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void HighlightSideMenu(string MenuParent, string PageLink, string SubMenuDiv)
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl(MenuParent);
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";
        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl(PageLink);
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
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
        }
        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx");
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }
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
            //VendorsPermission
            string VendorsPermission = dtUserPermission.Rows[0]["Vendor"] == DBNull.Value ? "YYYY" : dtUserPermission.Rows[0]["Vendor"].ToString();
            hdnAddDedcutions.Value = VendorsPermission.Length < 1 ? "Y" : VendorsPermission.Substring(0, 1);
            hdnEditDedcutions.Value = VendorsPermission.Length < 2 ? "Y" : VendorsPermission.Substring(1, 1);
            hdnDeleteDedcutions.Value = VendorsPermission.Length < 3 ? "Y" : VendorsPermission.Substring(2, 1);
            hdnViewDedcutions.Value = VendorsPermission.Length < 4 ? "Y" : VendorsPermission.Substring(3, 1);
            //if (hdnAddDedcutions.Value == "N")
            //{
            //    lnksubmit.Visible = false;
            //}
            if (hdnViewDedcutions.Value == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
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
    private void GetEmpListTicketTime()
    {
        try
        {
            DataSet ds = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.FStart = Convert.ToDateTime(txtstartDt.Text);
            objPropUser.Edate = Convert.ToDateTime(txtendDt.Text);
            //objPropUser.FStart = Convert.ToDateTime("05/28/2021");
            //objPropUser.Edate = Convert.ToDateTime("06/03/2021");
            objPropUser.Supervisor = ddlSuper.SelectedValue.ToString();
            objPropUser.DepartmentID = Convert.ToInt32(ddlDepartment.SelectedValue.ToString());
            objPropUser.EN = 0;
            objPropUser.ID = 0;
            objPropUser.WorkId = 0;
            ds = new BL_Wage().GetRunPayrollFromTicket(objPropUser);
            ds.Tables[0].Columns.Add("Reg_Holi", typeof(Double));
            ds.Tables[0].Columns.Add("OtherHr", typeof(Double));
            ds.Tables[0].Columns.Add("TotalHr", typeof(Double));
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dr["Reg_Holi"] = Convert.ToDouble(dr["Reg"].ToString()) + Convert.ToDouble(dr["holiday"].ToString());
                    dr["OtherHr"] = Convert.ToDouble(dr["OT"].ToString()) + Convert.ToDouble(dr["DT"].ToString()) + Convert.ToDouble(dr["NT"].ToString()) + Convert.ToDouble(dr["TT"].ToString());
                    dr["TotalHr"] = Convert.ToDouble(dr["Reg_Holi"].ToString()) + Convert.ToDouble(dr["OtherHr"].ToString());
                }
            }
            RadGrid_RunPayroll.VirtualItemCount = ds.Tables[0].Rows.Count;
            RadGrid_RunPayroll.DataSource = ds.Tables[0];
            ViewState["VirtualItemCount"] = ds.Tables[0].Rows.Count;
            lblRecordCount.Text = ds.Tables[0].Rows.Count + " Record(s) found";
            Session["RunPayrollList"] = ds.Tables[0];
            SaveFilter();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkGetTicketTime_Click(object sender, EventArgs e)
    {
        RadGrid_RunPayroll.CurrentPageIndex = 0;
        RadGrid_RunPayroll.PageSize = 50;
        GetEmpListTicketTime();
        RadGrid_RunPayroll.Rebind();
        //upPannelSearch.Update();
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
        //upPannelSearch.Update();
    }
    protected void lnkShowAll_Click(object sender, EventArgs e)
    {
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
        GetEmpList();
        RadGrid_RunPayroll.Rebind();
    }
    private double ConvertCurrentCurrencyFormatToDbl(string strCurrency)
    {
        if (!string.IsNullOrEmpty(strCurrency))
        {
            var dblReturn = double.Parse(strCurrency.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                                        NumberStyles.AllowThousands |
                                                                        NumberStyles.AllowDecimalPoint |
                                                                        NumberStyles.AllowTrailingSign |
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
        string username = ConfigurationManager.AppSettings["vertexApiUsername"].ToString(); // "dread@1000";
        string passWord = ConfigurationManager.AppSettings["vertexApiPassword"].ToString(); // "K3CHccxQ";
        // Setting up the URI for Payroll
        string uri = ConfigurationManager.AppSettings["vertexAddressURL"].ToString();
        //string uri = "https://payrollsandbox.ondemand.vertexinc.com:443/EiWebSvc/AddressWebService";

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
        // Below call to read the message back from server from stream to string
        async Task<string> getMessage(HttpResponseMessage messageFromServer)
        {
            code = await messageFromServer.Content.ReadAsStringAsync();
            return code;
        }
        //////////////////////////////////////////
        return workgeo;
    }
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
        if (Session["RunPayrollfromDate"] != null && Session["RunPayrollToDate"] != null)
        {
            txtstartDt.Text = Session["RunPayrollfromDate"].ToString();
            txtendDt.Text = Session["RunPayrollToDate"].ToString();
        }
        else
        {
            _objPropGeneral.ConnConfig = Session["config"].ToString();
            _objPropGeneral.CustomName = "PRLast";
            DataSet dsCustomPRLast = new DataSet();
            dsCustomPRLast = _objBLGeneral.getCustomFields(_objPropGeneral);
            if (dsCustomPRLast.Tables[0].Rows.Count > 0)
            {
                txtstartDt.Text = dsCustomPRLast.Tables[0].Rows[0]["Label"].ToString();
            }
            else
            {
                txtstartDt.Text = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).ToShortDateString();
            }
            _objPropGeneral.ConnConfig = Session["config"].ToString();
            _objPropGeneral.CustomName = "PRLast2";
            DataSet dsCustomPRLast2 = new DataSet();
            dsCustomPRLast2 = _objBLGeneral.getCustomFields(_objPropGeneral);
            if (dsCustomPRLast2.Tables[0].Rows.Count > 0)
            {
                txtendDt.Text = dsCustomPRLast2.Tables[0].Rows[0]["Label"].ToString();
            }
            else
            {
                txtendDt.Text = DateTime.Now.AddDays(DayOfWeek.Friday - (DateTime.Now.DayOfWeek)).Date.ToShortDateString();
            }
            Session["RunPayrollfromDate"] = txtstartDt.Text;
            Session["RunPayrollToDate"] = txtendDt.Text;
            //Session["RunPayrollfromDate"] = "05/28/2021";
            //Session["RunPayrollToDate"] = "06/03/2021";
        }
        txtcheckdate.Text = DateTime.Now.ToString();
        txtperioddesc.Text = "Period of " + txtstartDt.Text + " to " + txtendDt.Text;
        //txtweek.Text = GetIso8601WeekOfYear(Convert.ToDateTime("05/28/2021")).ToString();
    }
    public static int GetIso8601WeekOfYear(DateTime time)
    {
        // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
        // be the same week# as whatever Thursday, Friday or Saturday are,
        // and we always get those right
        DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
        if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
        {
            time = time.AddDays(3);
        }
        // Return the week of our adjusted day
        return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
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
        try
        {
            _objBLGeneral.UpdateCustomPRLast(Session["config"].ToString(), "05/28/2021", "06/03/2021");


            string str = "Saved successfully.";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  noty({text: '" + str + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelServyp", "noty({text: '" + str + "',  type : 'warning', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void btnGetDetail_Click(object sender, EventArgs e)
    {
        int shdnEmpIDint = 0;
        if (shdnEmpID.Value != "")
        {
            shdnEmpIDint = Convert.ToInt32(shdnEmpID.Value);
        }
        GetHourList(shdnEmpIDint);
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
        Response.Redirect("PayrollList.aspx");
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
        rowCount = Convert.ToInt32(ViewState["VirtualItemCount"]);


    }
    private void RowSelect()
    {
        foreach (GridDataItem gr in RadGrid_RunPayroll.Items)
        {
            HiddenField hdnid = (HiddenField)gr.FindControl("hdnid");
            HyperLink lblName = (HyperLink)gr.FindControl("lblName");
            HiddenField lblHoliday = (HiddenField)gr.FindControl("hdnlblHoliday");
            HiddenField lblVac = (HiddenField)gr.FindControl("hdnlblVac");
            HiddenField txtZone = (HiddenField)gr.FindControl("hdnZone");
            HiddenField txtReimb = (HiddenField)gr.FindControl("hdnReimb");
            HiddenField txtMilage = (HiddenField)gr.FindControl("hdnMileage");
            HiddenField txtBonus = (HiddenField)gr.FindControl("hdnBonus");
            HiddenField txtSick = (HiddenField)gr.FindControl("hdnsicktime");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

            int payrollRegisterId = 0;
            if (Session["PayrollRegisterId"] != null)
            {
                payrollRegisterId = Convert.ToInt32(Session["PayrollRegisterId"].ToString());
            }

            //chkSelect.ClientID.ToString();

            int ProcessDed = 0;
            if (chkProcessOtherDeduction.Checked == true)
            {
                ProcessDed = Convert.ToInt32("1");
            }
            else
            {
                ProcessDed = Convert.ToInt32("0");
            }
            lblName.Attributes["onclick"] = "OpenPayrollDetailModal('" + hdnid.Value + "','" + lblHoliday.Value + "','" + lblVac.Value + "','" + txtZone.Value + "','" + txtReimb.Value + "','" + txtMilage.Value + "','" + txtBonus.Value + "','" + ProcessDed + "','" + txtperioddesc.Text + "','21','" + txtSick.Value + "','" + chkSelect.ClientID.ToString() + "','" + payrollRegisterId + "'); ";
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
        }
        else
        {
            Session["Category_FilterExpression"] = null;
            Session["Category_Filters"] = null;
        }
        #endregion  
        GeneralFunctions obj = new GeneralFunctions();
        obj.CorrectTelerikPager(RadGrid_RunPayroll);
        RowSelect();
    }
    protected void lnkClear_Click(object sender, EventArgs e)
    {
        //ResetFormControlValues(this);
        check = false;
        lnkChk.Checked = false;
        foreach (GridColumn column in RadGrid_RunPayroll.MasterTableView.OwnerGrid.Columns)
        {
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
        }
        Session["Category_FilterExpression"] = null;
        Session["Category_Filters"] = null;
        RadGrid_RunPayroll.MasterTableView.FilterExpression = "";
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
            //objPropUser.FStart = Convert.ToDateTime("05/28/2021");
            //objPropUser.Edate = Convert.ToDateTime("06/03/2021");
            objPropUser.FStart = Convert.ToDateTime(txtstartDt.Text);
            objPropUser.Edate = Convert.ToDateTime(txtendDt.Text);
            objPropUser.Supervisor = ddlSuper.SelectedValue.ToString();
            objPropUser.DepartmentID = 0;
            objPropUser.EN = 0;
            objPropUser.ID = EmpID;
            objPropUser.WorkId = 0;
            ds = new BL_Wage().GetPayrollHour(objPropUser);
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
            shdnEmpIDint = Convert.ToInt32(shdnEmpID.Value);
        }
    }
    List<byte[]> lstbyte = new List<byte[]>();
    List<byte[]> lstbyteNew = new List<byte[]>();
    protected void imgPrintTemp1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            byte[] buffer1 = null;
            if (ddlApTopCheckTransType.SelectedValue == "0")
            {
                if (ddlApTopCheckForLoad.SelectedItem.Text.Trim() != null)
                {
                    string reportApTopCheckPathStimul = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + ddlApTopCheckForLoad.SelectedItem.Text.Trim() + ".mrt");
                    StiReport report = new StiReport();
                    FillReportApTopCheckDataSet(ddlApTopCheckForLoad.SelectedItem.Text.Trim());
                }
            }
            else if (ddlApTopCheckTransType.SelectedValue == "2")
            {
                string alert = string.Empty;
                bool Sent = false;
                string ACHfileResponseText = string.Empty;
                string ACHControleResponseText = string.Empty;
                string ACHfileName = string.Empty;
                string ACHControlefileName = string.Empty;
                string Time = DateTime.Now.ToString("yyyyMMdd.hhmmss");
                string FileCreationTime = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
                string FileCreationDate = DateTime.Now.ToString("yy/MM/dd");
                string TempACHfileName = string.Empty;
                try
                {
                    /*Create file from user's bank detail and save it locally to path given in web.config*/
                    ACHfileName = CreateAchFile(Time, FileCreationTime, FileCreationDate, out TempACHfileName);
                    //Create ACH controle File
                    //ACHControlefileName = CreateAchControleFile(Time, FileCreationTime, FileCreationDate);
                    /*Send file through SFTP protocol to PNC bank server*/
                    //ACHfileResponseText = SendFile(ACHfileName);
                    //ACHControleResponseText = SendFile(ACHControlefileName);
                    //if (ACHfileResponseText == "true" && ACHControleResponseText == "true")
                    //{
                    //    Sent = true;
                    //}
                    Sent = true;
                    /*Update transaction status in MOM*/
                    //MakePayment(TempACHfileName, Sent, ACHfileResponseText);
                    if (Sent)
                    {
                        alert = "ACH File has been sent successfully to PNC Server. <BR/>Your payment will be processed soon.";
                        Session["uidv"] = null;
                        string sGenName = "DDPAYROLLACH.txt";
                        System.IO.FileStream fs = null;
                        fs = System.IO.File.Open(Server.MapPath("~/ACHFile/" + TempACHfileName + ""), System.IO.FileMode.Open);
                        byte[] btFile = new byte[fs.Length];
                        fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
                        fs.Close();
                        Response.AddHeader("Content-disposition", "attachment; filename=" + sGenName);
                        Response.ContentType = "application/octet-stream";
                        Response.BinaryWrite(btFile);
                        RadAjaxManager_WageDeduction.ResponseScripts.Add("ClosetemplateModal();");
                        Response.End();
                    }
                    else
                    {
                        alert = "ACH Payment Failed. <BR/> ACH File Response :-" + ACHfileResponseText;
                        alert += "<BR/> ACH Controle Response  :-" + ACHControleResponseText;
                        Session["uidv"] = null;
                    }
                }
                catch (Exception ex)
                {
                    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    //lblErr.Text = ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.StackTrace;
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    /// <summary>
    /// Create Ach Controle File
    /// </summary>
    /// <param name="Time"></param>
    /// <param name="FileCreationTime"></param>
    /// <param name="FileCreationDate"></param>
    /// <returns></returns>
    private string CreateAchControleFile(string Time, string FileCreationTime, string FileCreationDate)
    {
        string FileName = string.Empty;
        //string path = String.Format(WebConfigurationManager.AppSettings["ACH_File_Path"].Trim());
        string path = context.Current.Server.MapPath("~/ACHFile/");
        if (!string.IsNullOrEmpty(path))
        {
            // specify your path
            //FileName=path+"aut.southern.cntl.in."+Time+".txt";
            FileName = path + "DDPAYROLLMIDWEST" + Time;

            string DecimalValue = string.Empty;
            string Amount = string.Empty;
            string ZeroValue = string.Empty;
            //string Amt = ViewState["amt"].ToString();
            string Amt = "0.00";
            Amount = Amt.ToString().Replace(".", "");
            //---------------
            string CustomerNumber = "0040000328";
            string NumberofCredits = "00000000";
            string CreditAmount = "000000000000";
            string NumberofDebits = "00000001";
            string DebitAmount = "";
            string ItemCount = "00000001";
            string SourceID = "AUTPCG14";
            for (int i = 1; i <= 12 - Amount.Length; i++)
            {
                DebitAmount += "0";
            }
            DebitAmount = DebitAmount + Amount;
            string Data = CustomerNumber + NumberofCredits + CreditAmount + NumberofDebits + DebitAmount + ItemCount + SourceID;
            using (StreamWriter file = new StreamWriter(FileName))
            {
                file.WriteLine(Data);
            }
        }
        return FileName;
    }
    /// <summary>
    /// Send File 
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    private string SendFile(string fileName)
    {
        string Response = string.Empty;
        try
        {
            //sFTP Folder name. Leave blank if you want to upload to root folder.
            string sFTP_Folder = WebConfigurationManager.AppSettings["sFTP_Folder"].Trim();
            //sFTP Server URL.
            var sFTP_Host = WebConfigurationManager.AppSettings["sFTP_Host"].Trim();
            var sFTP_Port = WebConfigurationManager.AppSettings["sFTP_Port"].Trim();
            var sFTP_UserName = WebConfigurationManager.AppSettings["sFTP_UserName"].Trim();
            var sFTP_Password = WebConfigurationManager.AppSettings["sFTP_Password"].Trim();
            // path for file you want to upload
            var uploadFile = fileName;
            using (var client = new SftpClient(sFTP_Host, Convert.ToInt16(sFTP_Port), sFTP_UserName, sFTP_Password))
            {
                client.Connect();
                client.ChangeDirectory(sFTP_Folder);
                if (client.IsConnected)
                {
                    using (var fileStream = new FileStream(uploadFile, FileMode.Open))
                    {
                        client.BufferSize = 4 * 1024; // bypass Payload error large files
                        client.UploadFile(fileStream, Path.GetFileName(uploadFile));
                    }
                }
            }
            return Response = "true";
        }
        catch (Exception ex)
        {
            return Response = ex.Message.ToString();
        }
    }
    /// <summary>
    /// Create Ach File
    /// </summary>
    /// <param name="Time"></param>
    /// <param name="FileCreationTime"></param>
    /// <param name="FileCreationDate"></param>
    /// <returns></returns>
    private string CreateAchFile(string Time, string FileCreationTime, string FileCreationDate, out string TempACHfileName)
    {
        string FileName = string.Empty;
        TempACHfileName = string.Empty;
        //string path = String.Format(WebConfigurationManager.AppSettings["ACH_File_Path"].Trim());
        string path = context.Current.Server.MapPath("~/ACHFile/");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        // specify your path
        if (!string.IsNullOrEmpty(path))
        {
            //FileName = path + "DDPAYROLLMIDWEST" + Time;
            //TempACHfileName = "DDPAYROLLMIDWEST" + Time;
            FileName = path + "DDPAYROLLACH" + Time;
            TempACHfileName = "DDPAYROLLACH" + Time;

            filecontrolvariables.ResetValues();
            Append.ResetValues();
            _objBank.ConnConfig = Session["config"].ToString();
            _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue.ToString());
            DataSet _dtBankDetail = new DataSet();
            _dtBankDetail = _objBL_Bank.spGetBankACH(_objBank);
            DataSet _checkDS = new DataSet();
            _checkDS = (DataSet)ViewState["ReturnACHDetail"];
            object sumObject;
            sumObject = _checkDS.Tables[0].Compute("Sum(Net)", string.Empty);
            int rownumber = 1;
            //string m_str = "07100028";
            string m_str = _dtBankDetail.Tables[0].Rows[0]["TraceNo1"].ToString();
            //---------------
            CreateFileHeader(FileName.Trim(), FileCreationTime, FileCreationDate, _dtBankDetail);
            //---------------
            foreach (DataRow _chRow in _checkDS.Tables[0].Rows)
            {
                AddEmpDetails(_chRow["ACHBank"].ToString(), _chRow["EmpName"].ToString(), _chRow["ACHBank"].ToString(), _chRow["Net"].ToString().ToString(), m_str + (rownumber.ToString().PadLeft(7, (char)48)), _dtBankDetail, _checkDS);
                rownumber++;
            }
            //---------------
            //AddEntryDetails(_dtBankDetail.Tables[0].Rows[0]["nAcct"].ToString(), "MIDWEST ELEVATOR", "", sumObject.ToString(), _dtBankDetail, _checkDS);
            AddEntryDetails(_dtBankDetail.Tables[0].Rows[0]["nAcct"].ToString(), _dtBankDetail.Tables[0].Rows[0]["CompanyName"].ToString(), "", sumObject.ToString(), _dtBankDetail, _checkDS);
            //---------------
            CreateBatchHeader(_dtBankDetail);
            //----------------
            CreateFileControle();

        }
        return FileName;
    }
    public void CreateFileHeader(string FileName, string FileCreationTime, string FileCreationDate, DataSet _dtBankDetail)
    {
        ACHFileheader objBL = new ACHFileheader();
        string ImmediateDestination = _dtBankDetail.Tables[0].Rows[0]["ACHFileHeaderStringB"].ToString();
        string ImmediateOrigin = "";
        string PriorityCode = "";
        string FormatCode = "";
        string FileIdModifier = _dtBankDetail.Tables[0].Rows[0]["ACHFileHeaderStringC"].ToString();
        string ReferenceTypeCode = "";
        string RecordSize = _dtBankDetail.Tables[0].Rows[0]["fDesc"].ToString();
        string BlockingFactor = "";
        //string FileCreationTime="2246";
        string ImmediateDestinationName = "";
        //string FileCreationDate="040520";
        //string ImmediateOriginName = "MIDWEST ELEVATOR CO INC";
        string ImmediateOriginName = _dtBankDetail.Tables[0].Rows[0]["APImmediateOrigin"].ToString().ToUpper();
        string ReferenceCode = "";
        Append.FileName = FileName;
        if (ImmediateDestination != string.Empty &&
        FileIdModifier != string.Empty && RecordSize != string.Empty)
        {
            objBL.ReferenceTypeCode = ReferenceTypeCode;
            objBL.PriorityCode = PriorityCode;
            objBL.ImmediateDestination = ImmediateDestination;
            objBL.ImmediateOrigin = ImmediateOrigin;
            objBL.FileCreationDate = Convert.ToString(FileCreationDate);//.ToString("yy/MM/dd"));
            objBL.FileCreationTime = FileCreationTime.Replace(":", "");
            objBL.FileIdModifier = FileIdModifier;
            objBL.RecordSize = RecordSize;
            objBL.BlockingFactor = BlockingFactor;
            objBL.FormatCode = FormatCode;
            objBL.ImmediateDestinationName = ImmediateDestinationName;
            objBL.ImmediateOriginName = ImmediateOriginName;
            objBL.ReferenceCode = ReferenceCode;
            objBL.SaveFileHeaderMidwest(Append.FileName);
        }

    }
    public void AddEmpDetails(string BankRouting_OR_DFIAccountNumber, string AccountHolderName_OR_RecievingCompanyName, string BankAccount_OR_IdentificationNumber, string Amount, string TraceNumber, DataSet _dtBankDetail, DataSet _checkDS)
    {
        //string RecordTypeCode = "6";
        //string TransactioCode = "22";
        string RecordTypeCode = _dtBankDetail.Tables[0].Rows[0]["RecordTypeCode1"].ToString();
        string TransactioCode = _dtBankDetail.Tables[0].Rows[0]["TransactionCode1"].ToString();
        string RecievingDFIIdentification = _checkDS.Tables[0].Rows[0]["ACHRoute"].ToString();
        string CheckDigit = "6";
        string DiscretionaryData = "";
        //string AddendaRecordIndicator = "0";
        string AddendaRecordIndicator = _dtBankDetail.Tables[0].Rows[0]["EndRecordIndicator1"].ToString();

        EntryDetail objEntry = new EntryDetail();
        bool m_flag = false;
        //if (objEntry.BankRoutingNumberValidation(RecievingDFIIdentification) && TransactioCode != "Select" && RecievingDFIIdentification != string.Empty &&
        //    BankRouting_OR_DFIAccountNumber != string.Empty && Amount != string.Empty && AccountHolderName_OR_RecievingCompanyName != string.Empty && AddendaRecordIndicator != string.Empty)
        if (TransactioCode != "Select" && RecievingDFIIdentification != string.Empty &&
           BankRouting_OR_DFIAccountNumber != string.Empty && Amount != string.Empty && AccountHolderName_OR_RecievingCompanyName != string.Empty && AddendaRecordIndicator != string.Empty)
        {

            objEntry.RecordTypeCode = RecordTypeCode;
            objEntry.TransactioCode = TransactioCode.ToString().Substring(0, 2);
            objEntry.RecievingDFIIdentification = RecievingDFIIdentification;
            objEntry.DFIAccountNumber = BankRouting_OR_DFIAccountNumber;
            objEntry.Amount = Amount.Replace("$", "").Replace(".", "");
            objEntry.IdentificationNumber = BankAccount_OR_IdentificationNumber;
            objEntry.RecievingCompanyName = AccountHolderName_OR_RecievingCompanyName;
            objEntry.DiscretionaryData = DiscretionaryData;
            objEntry.AddendaRecordIndicator = AddendaRecordIndicator;
            objEntry.TraceNumber = TraceNumber;
            objEntry.saveEntryMidwest(Append.FileName);
        }

    }
    public void AddEntryDetails(string BankRouting_OR_DFIAccountNumber, string AccountHolderName_OR_RecievingCompanyName, string BankAccount_OR_IdentificationNumber, string Amount, DataSet _dtBankDetail, DataSet _checkDS)
    {
        //string RecordTypeCode = "6";
        //string TransactioCode = "27";
        string RecordTypeCode = _dtBankDetail.Tables[0].Rows[0]["RecordTypeCode2"].ToString();
        string TransactioCode = _dtBankDetail.Tables[0].Rows[0]["TransactionCode2"].ToString();
        string RecievingDFIIdentification = _dtBankDetail.Tables[0].Rows[0]["NRoute"].ToString();
        string CheckDigit = "6";
        string DiscretionaryData = "";
        //string AddendaRecordIndicator = "0";
        string AddendaRecordIndicator = _dtBankDetail.Tables[0].Rows[0]["EndRecordIndicator2"].ToString();
        //string TraceNumber = "071000280000001";
        string TraceNumber = _dtBankDetail.Tables[0].Rows[0]["TraceNo2"].ToString();
        EntryDetail objEntry = new EntryDetail();
        bool m_flag = false;
        if (objEntry.BankRoutingNumberValidation(RecievingDFIIdentification) && TransactioCode != "Select" && RecievingDFIIdentification != string.Empty &&
            BankRouting_OR_DFIAccountNumber != string.Empty && Amount != string.Empty && AccountHolderName_OR_RecievingCompanyName != string.Empty && AddendaRecordIndicator != string.Empty)
        {

            objEntry.RecordTypeCode = RecordTypeCode;
            objEntry.TransactioCode = TransactioCode.ToString().Substring(0, 2);
            objEntry.RecievingDFIIdentification = RecievingDFIIdentification;
            objEntry.DFIAccountNumber = BankRouting_OR_DFIAccountNumber;
            objEntry.Amount = Amount.Replace("$", "").Replace(".", "");
            objEntry.IdentificationNumber = BankAccount_OR_IdentificationNumber;
            objEntry.RecievingCompanyName = AccountHolderName_OR_RecievingCompanyName;
            objEntry.DiscretionaryData = DiscretionaryData;
            objEntry.AddendaRecordIndicator = AddendaRecordIndicator;
            objEntry.TraceNumber = TraceNumber;
            objEntry.saveEntryMidwest(Append.FileName);
        }

    }
    public void CreateBatchHeader(DataSet _dtBankDetail)
    {
        bool m_flag = false;
        ACHBatchHeader objBatchHeader = new ACHBatchHeader();
        EntryDetail objEntry = new EntryDetail();
        int cmbServiceClassCode = 200;//ddl 
        //string CompanyName = "MIDWEST ELEVATOR";
        string CompanyName = _dtBankDetail.Tables[0].Rows[0]["CompanyName"].ToString();
        string CompanyIdentification = _dtBankDetail.Tables[0].Rows[0]["ACHCompanyHeaderString1"].ToString();
        int StandardEntryClassCode = 0;//ddl
        string CompanyEntryDescription = "";
        //string OriginatorStatusCode = "1";
        string OriginatorStatusCode = _dtBankDetail.Tables[0].Rows[0]["OriginatorStatusCode"].ToString();
        string OriginatingDFIIdentification = _dtBankDetail.Tables[0].Rows[0]["NRoute"].ToString();
        string CompanyDiscretionaryData = "";
        //string RecordTypeCode = "5";
        string RecordTypeCode = _dtBankDetail.Tables[0].Rows[0]["RecordTypeCode3"].ToString();
        //string BatchNumber = "1071000288000001";
        string BatchNumber = _dtBankDetail.Tables[0].Rows[0]["BatchNumber"].ToString();
        string CompanyDescriptiveDate = "";
        string EffectiveEntryDate = DateTime.Now.ToString("yyMMdd");   //"131011";
        //string JulianDate = "000";
        string JulianDate = _dtBankDetail.Tables[0].Rows[0]["JulianDate"].ToString();
        if (cmbServiceClassCode != -1 && CompanyName != string.Empty && CompanyIdentification != string.Empty &&
           StandardEntryClassCode == 0 && OriginatorStatusCode != string.Empty &&
               OriginatingDFIIdentification != string.Empty && objEntry.BankRoutingNumberValidation(OriginatingDFIIdentification))
        {

            objBatchHeader.RecordTypeCode = RecordTypeCode;
            objBatchHeader.ServiceClassCode = cmbServiceClassCode.ToString();
            objBatchHeader.CompanyName = CompanyName;
            objBatchHeader.CompanyDiscretionaryData = CompanyDiscretionaryData;
            objBatchHeader.CompanyIdentification = CompanyIdentification;
            //objBatchHeader.StandardEntryClassCode = StandardEntryClassCode.ToString() == "0" ? "PPD" : "CCD";
            objBatchHeader.StandardEntryClassCode = "";
            objBatchHeader.CompanyEntryDescription = CompanyEntryDescription;
            objBatchHeader.CompanyDescriptiveDate = Convert.ToString(CompanyDescriptiveDate);//ToString("yy/MM/dd"));
            objBatchHeader.EffectiveEntryDate = Convert.ToString(EffectiveEntryDate);//ToString("yy/MM/dd"));
            objBatchHeader.JulianDate = JulianDate.ToString();
            objBatchHeader.OriginatorStatusCode = OriginatorStatusCode;
            objBatchHeader.OriginatingDFIIdentification = OriginatingDFIIdentification.Substring(0, 8);
            objBatchHeader.BatchNumber = BatchNumber;
            //string FileName = frmmain._strPath;

            if (objBatchHeader.IsBatchValid())
            {
                string filedata = string.Empty;
                string strcontent = string.Empty;
                string srEnd = string.Empty;

                m_flag = true;
                if (m_flag)
                {
                    using (StreamReader sr = new StreamReader(Append.FileName))
                    {
                        while (sr.Peek() >= 0)
                        {
                            srEnd = sr.ReadLine();
                            //if (srEnd.StartsWith("9"))
                            if (srEnd.StartsWith("2"))
                            {
                                strcontent = srEnd;
                            }
                        }
                    }
                    //sr.Close();
                    using (StreamReader srNew = new StreamReader(Append.FileName))
                    {
                        while (srNew.Peek() >= 0)
                        {
                            filedata = srNew.ReadToEnd();
                            if (strcontent != string.Empty)
                                filedata = filedata.Replace(strcontent, "").TrimEnd(filecontrolvariables.charRemove);
                        }
                    }
                    //srNew.Close();
                    using (StreamWriter swwrite = new StreamWriter(Append.FileName))
                    {
                        swwrite.Write(filedata);
                    }
                    //swwrite.Close();
                    m_flag = false;
                    //sr.Close();
                }

                objBatchHeader.saveBatchHeaderMidwest(Append.FileName);
                //("Batch for the record saved successfully","Message"

            }


        }
        else if (cmbServiceClassCode != -1 && CompanyName != string.Empty && CompanyIdentification != string.Empty &&
           StandardEntryClassCode == 1 && OriginatorStatusCode != string.Empty &&
               OriginatingDFIIdentification != string.Empty && objEntry.BankRoutingNumberValidation(OriginatingDFIIdentification))
        {


            {
                objBatchHeader.RecordTypeCode = RecordTypeCode;
                objBatchHeader.ServiceClassCode = cmbServiceClassCode.ToString();
                objBatchHeader.CompanyName = CompanyName;
                objBatchHeader.CompanyDiscretionaryData = CompanyDiscretionaryData;
                objBatchHeader.CompanyIdentification = CompanyIdentification;
                objBatchHeader.StandardEntryClassCode = StandardEntryClassCode.ToString();
                objBatchHeader.CompanyEntryDescription = CompanyEntryDescription;
                objBatchHeader.CompanyDescriptiveDate = Convert.ToString(CompanyDescriptiveDate);//ToString("yy/MM/dd"));
                objBatchHeader.EffectiveEntryDate = Convert.ToString(EffectiveEntryDate);//ToString("yy/MM/dd"));
                objBatchHeader.JulianDate = "".PadRight(3, ' ').ToString();
                objBatchHeader.OriginatorStatusCode = OriginatorStatusCode;
                objBatchHeader.OriginatingDFIIdentification = OriginatingDFIIdentification.Substring(0, 8);
                objBatchHeader.BatchNumber = BatchNumber;
                if (objBatchHeader.IsBatchValid())
                {
                    string filedata = string.Empty;
                    string strcontent = string.Empty;
                    string srEnd = string.Empty;

                    m_flag = true;
                    if (m_flag)
                    {
                        using (StreamReader sr = new StreamReader(Append.FileName))
                        {
                            while (sr.Peek() >= 0)
                            {
                                srEnd = sr.ReadLine();
                                if (srEnd.StartsWith("9"))
                                {
                                    strcontent = srEnd;
                                }
                            }
                            //   sr.Close();
                        }
                        using (StreamReader srNew = new StreamReader(Append.FileName))
                        {
                            while (srNew.Peek() >= 0)
                            {
                                filedata = srNew.ReadToEnd();
                                if (strcontent != string.Empty)
                                    filedata = filedata.Replace(strcontent, "").TrimEnd(filecontrolvariables.charRemove);
                            }
                            //   srNew.Close();
                        }
                        using (StreamWriter swwrite = new StreamWriter(Append.FileName))
                        {
                            swwrite.Write(filedata);
                            //swwrite.Close();
                        }



                        m_flag = false;
                        // sr.Close();
                    }

                    objBatchHeader.saveBatchHeaderMidwest(Append.FileName);
                    //"Batch for the record saved successfully"

                }
                else
                {
                    //You must save atleast one entry" 
                }
            }
        }
    }
    public void CreateFileControle()
    {
        Fileentry objfileentry = new Fileentry();
        StringBuilder sb = new StringBuilder();
        objfileentry.createFileEntryMidwest(Append.FileName, out sb);
        //File.AppendAllText(Append.FileName, Environment.NewLine + sb.ToString());
        string lastlines = "9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999";
        sb.Append(Environment.NewLine + lastlines);
        sb.Append(Environment.NewLine + lastlines);
        sb.Append(Environment.NewLine + lastlines);
        sb.Append(Environment.NewLine + lastlines);
        File.AppendAllText(Append.FileName, Environment.NewLine + sb.ToString());

    }

    private void mail(string message)
    {
        User objPropUser = new User();
        BL_User objBL_User = new BL_User();

        Thread email = new Thread(delegate ()
        {
            string to = string.Empty;
            string from = string.Empty;
            DataSet dsC = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            dsC = objBL_User.getControl(objPropUser);
            if (dsC.Tables[0].Rows.Count > 0)
            {
                from = dsC.Tables[0].Rows[0]["Email"].ToString();
            }

            objPropUser.CustomerID = Convert.ToInt32(Session["custid"].ToString());
            to = objBL_User.getCustomerEmail(objPropUser);

            if (to.Trim() != string.Empty && from.Trim() != string.Empty)
            {
                try
                {
                    Mail mail = new Mail();
                    mail.From = from;
                    mail.To = to.Split(';', ',').OfType<string>().ToList();
                    mail.Bcc.Add(from);
                    mail.Title = "ACH File has been sent successfully to PNC Server for invoice# " + ViewState["uid"].ToString();
                    mail.Text = message;
                    mail.RequireAutentication = false;
                    mail.Send();
                }
                catch (Exception ex)
                {
                    log(ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.StackTrace);
                }
            }
        });
        email.IsBackground = true;
        email.Start();
    }

    private void log(String message)
    {
        DateTime datetime = DateTime.Now;
        string savepath = Server.MapPath(Request.ApplicationPath) + "/logs/";
        String oFileName = savepath + "MOM_" + datetime.ToString("dd_MM_yyyy") + ".log";
        if (!Directory.Exists(savepath))
        {
            Directory.CreateDirectory(savepath);
        }
        if (!File.Exists(oFileName))
        {
            System.IO.FileStream f = File.Create(oFileName);
            f.Close();
        }

        try
        {
            System.IO.StreamWriter writter = File.AppendText(oFileName);
            writter.WriteLine(datetime.ToString("MM-dd hh:mm") + " > " + message);
            writter.Flush();
            writter.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message.ToString());
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
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void ImageButton8_Click(object sender, ImageClickEventArgs e)
    {
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

    protected void imgPrintTemp6_Click(object sender, ImageClickEventArgs e)
    {                                                                           //              MADDEN – check top 
        try
        {

            //F:\ESS\ESSMOM\MOM\MOM - NewDesign\MSWeb\StimulsoftReports\APChecks\APTopCheck
            byte[] buffer1 = null;
            string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + ddlTopChecksForLoad.SelectedItem.Text.Trim() + ".mrt");
            StiReport report = new StiReport();
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
    protected void lnkprintchecktemp_Click(object sender, EventArgs e)
    {
        //try
        //{
        //DataTable dt = GetTransaction();
        ExceptionLogging.SendMsgToText("Payroll Submit Start: ");
        DataTable dt = GetTable();
        string code = "";
        string empgeocode = "000000000";
        ExceptionLogging.SendMsgToText("Get Work Geo Code Start: ");
        string workgeocode = GetWorkGeoCode();
        ExceptionLogging.SendMsgToText("Get Work Geo Code End: ");
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

            ExceptionLogging.SendMsgToText("Loop on all employee start : ");
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
                    ExceptionLogging.SendMsgToText("Data for Employee " + dict["hdnName"].ToString().Trim());
                    dr["ID"] = dict["hdnid"].ToString().Trim();
                    dr["Name"] = dict["hdnName"].ToString().Trim();
                    dr["Reg"] = double.Parse(dict["hdnlblReg"].ToString().Trim());
                    dr["OT"] = double.Parse(dict["hdnlblOT"].ToString().Trim());
                    dr["NT"] = double.Parse(dict["hdnlblNT"].ToString().Trim());
                    dr["DT"] = double.Parse(dict["hdnlblDT"].ToString().Trim());
                    dr["TT"] = double.Parse(dict["hdnlblTT"].ToString().Trim());

                    dr["Zone"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnZone"].ToString().Trim());
                    dr["Milage"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnMileage"].ToString());

                    //dr["Toll"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnToll"].ToString());
                    dr["Toll"] = 0.00;
                    dr["OtherE"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnOtherE"].ToString());
                    dr["pay"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnpay"].ToString());

                    dr["holiday"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnlblHoliday"].ToString().Trim());
                    dr["vacation"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnlblVac"].ToString().Trim());
                    dr["sicktime"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnsicktime"].ToString().Trim());
                    dr["reimb"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnReimb"].ToString().Trim());
                    dr["bonus"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnBonus"].ToString());

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
                    empgeocode = dict["hdnGeocode"].ToString().Trim();

                    string username = ConfigurationManager.AppSettings["vertexApiUsername"].ToString(); // "dread@1000";
                    string passWord = ConfigurationManager.AppSettings["vertexApiPassword"].ToString(); // "K3CHccxQ";

                    ExceptionLogging.SendMsgToText("Employee " + dict["hdnName"].ToString().Trim() + " Tax API Start : ");
                    //string payrolluri = "https://payrollsandbox.ondemand.vertexinc.com:443/EiWebSvc/PayTaxCalcWebService";
                    string payrolluri = ConfigurationManager.AppSettings["vertexPayrollURL"].ToString();
                    var chkdateString = Convert.ToDateTime(txtcheckdate.Text).ToString("yyyyMMdd");

                    double dedAmt401K = 0;
                    double dedAmt401KID = 0;
                    User objPropUser = new User();
                    DataSet ds = new DataSet();
                    objPropUser.ConnConfig = Session["config"].ToString();
                    objPropUser.FStart = Convert.ToDateTime(txtstartDt.Text);
                    objPropUser.Edate = Convert.ToDateTime(txtendDt.Text);
                    objPropUser.HolidayAm = ConvertCurrentCurrencyFormatToDbl(dict["hdnlblHoliday"].ToString().Trim());
                    objPropUser.VacAm = ConvertCurrentCurrencyFormatToDbl(dict["hdnlblVac"].ToString().Trim());
                    objPropUser.ZoneAm = ConvertCurrentCurrencyFormatToDbl(dict["hdnZone"].ToString().Trim());
                    objPropUser.ReimbAm = ConvertCurrentCurrencyFormatToDbl(dict["hdnReimb"].ToString().Trim());
                    objPropUser.MilageAm = ConvertCurrentCurrencyFormatToDbl(dict["hdnMileage"].ToString());
                    objPropUser.BonusAm = ConvertCurrentCurrencyFormatToDbl(dict["hdnBonus"].ToString());
                    int ProcessDed = 0;
                    if (chkProcessOtherDeduction.Checked == true)
                    {
                        ProcessDed = Convert.ToInt32("1");
                    }
                    else
                    {
                        ProcessDed = Convert.ToInt32("0");
                    }
                    objPropUser.ID = Convert.ToInt32(dict["hdnid"].ToString().Trim());

                    ds = new BL_Wage().GetPayrollDeductions(objPropUser, ProcessDed);
                    DataTable dtDedTablePre = ds.Tables[0];

                    StringBuilder sbDeduct = new StringBuilder();

                    DataRow drtemp = dtDedTablePre.Select("ID=1").FirstOrDefault();
                    if (drtemp != null)
                    {
                        dedAmt401K = Convert.ToDouble(drtemp["Amount"]);
                        dedAmt401KID = Convert.ToDouble(drtemp["ID"]);
                    }
                    if (dtDedTablePre.Rows.Count > 0)
                    {
                        dtDedTablePre.DefaultView.RowFilter = "PaidBy = 1";
                        dtDedTablePre = (dtDedTablePre.DefaultView).ToTable();

                        foreach (DataRow row in dtDedTablePre.Rows)
                        {
                            double dedAmt = 0;
                            double? dedID = null;
                            if (row["VertexDedutionId"] != DBNull.Value)
                            {
                                dedID = Convert.ToDouble(row["VertexDedutionId"]);
                            }
                            dedAmt = Convert.ToDouble(row["Amount"]);
                            if (dedID != null && dedAmt > 0)
                            {
                                sbDeduct.Append("<DEDUCT>" +
                                    " <ID>" + dedID + "</ID> " +
                                    "<Amt>" + dedAmt + "</Amt> " +
                                    "</DEDUCT>");
                            }
                            else if (dedID == null && dedAmt > 0)
                            {
                                sbDeduct.Append("<DEDUCT>" +
                                   "<ID>46</ID> " +
                                   "<Amt>" + dedAmt + "</Amt> " +
                                   "</DEDUCT>");
                            }
                        }
                    }
                    else
                    {
                        sbDeduct.Append("<DEDUCT>" +
                           " <ID>46</ID> " +
                           "<Amt>" + dedAmt401K + "</Amt> " +
                           "</DEDUCT>");
                    }

                    DataSet dsemp = new DataSet();
                    _objEmp.ConnConfig = Session["config"].ToString();
                    _objEmp.ID = Convert.ToInt32(dict["hdnid"].ToString().Trim());

                    dsemp = objBL_Wage.GetEmployeeListByID(_objEmp);
                    DataRow _drEmpRow = dsemp.Tables[0].Rows[0];

                    if (Convert.ToInt32(_drEmpRow["FAllow"].ToString()) == -1)
                    {
                        _drEmpRow["FAllow"] = 0;
                    }

                    objPropUser.ID = Convert.ToInt32(dict["hdnid"].ToString().Trim());

                    DataSet dsHourWithLocation = GetPayrollHourWithLocation(objPropUser);
                    //DataSet dsHourWithLocation = GetPayrollHourWithLocation(_objEmp.ID);
                    StringBuilder sbWork = new StringBuilder();
                    StringBuilder sbCityJUR = new StringBuilder();
                    int Filing_Stat = 1;
                    if (_drEmpRow["FStatus"].ToString() == "1") //Married
                    {
                        Filing_Stat = 2; //Married
                    }
                    else if (_drEmpRow["FStatus"].ToString() == "0") //Single
                    {
                        Filing_Stat = 1; //Single
                    }
                    List<string> lstStateGeoCode = new List<string>();
                    StringBuilder sbWorkInfo = new StringBuilder();
                    StringBuilder sbCMP = new StringBuilder();
                    StringBuilder sbAggCMP = new StringBuilder();
                    StringBuilder sbVprtCompensation = new StringBuilder();
                    List<string> lstGeo = new List<string>();
                    int cmpId = 0;

                    if (dsHourWithLocation.Tables[0].Rows.Count > 0)
                    {
                        lstStateGeoCode = (dsHourWithLocation.Tables[0].AsEnumerable().Select(x => x["sGeocode"].ToString()).Distinct()).ToList();
                        foreach (DataRow dRow in dsHourWithLocation.Tables[0].Rows)
                        {
                            double Amount = Convert.ToDouble(dRow["Amount"]);
                            if (Amount > 0)
                            {
                                if (dRow["lCity"].ToString() != "" && dRow["lState"].ToString() != "" && dRow["lZip"].ToString() != "")
                                {
                                    sbWorkInfo.Clear();
                                    sbCMP.Clear();
                                    //sbAggCMP.Clear();
                                    string geocode = GetWorkGeo(dRow);

                                    if (geocode == null)
                                    {
                                        geocode = _drEmpRow["Geocode"].ToString();
                                    }

                                    lstGeo.Add(geocode);
                                    sbWorkInfo.Append("<WRKINFO>" +
                                                           "<GEO>" + geocode + "</GEO>" +
                                                       "</WRKINFO>");
                                    if (Convert.ToBoolean(dRow["FIT"]) == false)
                                    {
                                        cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[0].Rows.IndexOf(dRow);
                                        sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                           "<TAXID>400</TAXID>" +
                                                           "<GEO>0</GEO>" +
                                                            "<TAX_TYPE>2</TAX_TYPE>" +
                                                           "<ID>" + cmpId + "</ID>" +
                                                           "<TYPE>r</TYPE>" +
                                                           "<OVTYPE>3</OVTYPE>" +
                                                       "</VPRT_COMPENSATION>");
                                    }
                                    if (Convert.ToBoolean(dRow["FICA"]) == false)
                                    {
                                        cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[0].Rows.IndexOf(dRow);
                                        sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                          "<TAXID>403</TAXID>" +
                                                          "<GEO>0</GEO>" +
                                                           "<TAX_TYPE>2</TAX_TYPE>" +
                                                          "<ID>" + cmpId + "</ID>" +
                                                          "<TYPE>r</TYPE>" +
                                                          "<OVTYPE>3</OVTYPE>" +
                                                      "</VPRT_COMPENSATION>");
                                    }
                                    if (Convert.ToBoolean(dRow["MEDI"]) == false)
                                    {
                                        cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[0].Rows.IndexOf(dRow);
                                        sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                         "<TAXID>406</TAXID>" +
                                                         "<GEO>0</GEO>" +
                                                          "<TAX_TYPE>2</TAX_TYPE>" +
                                                         "<ID>" + cmpId + "</ID>" +
                                                         "<TYPE>r</TYPE>" +
                                                         "<OVTYPE>3</OVTYPE>" +
                                                     "</VPRT_COMPENSATION>");
                                    }
                                    if (Convert.ToBoolean(dRow["FUTA"]) == false)
                                    {
                                        cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[0].Rows.IndexOf(dRow);
                                        sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                       "<TAXID>401</TAXID>" +
                                                       "<GEO>0</GEO>" +
                                                        "<TAX_TYPE>2</TAX_TYPE>" +
                                                       "<ID>" + cmpId + "</ID>" +
                                                       "<TYPE>r</TYPE>" +
                                                       "<OVTYPE>3</OVTYPE>" +
                                                   "</VPRT_COMPENSATION>");
                                    }
                                    if (Convert.ToBoolean(dRow["SIT"]) == false)
                                    {
                                        cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[0].Rows.IndexOf(dRow);
                                        sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                      "<TAXID>450</TAXID>" +
                                                      "<GEO>" + dRow["sGeocode"].ToString() + "</GEO>" +
                                                       "<TAX_TYPE>2</TAX_TYPE>" +
                                                      "<ID>" + cmpId + "</ID>" +
                                                      "<TYPE>r</TYPE>" +
                                                      "<OVTYPE>3</OVTYPE>" +
                                                  "</VPRT_COMPENSATION>");
                                    }
                                    sbCMP.Append("<CMP>" +
                                                       "<ID>" + cmpId + "</ID>" +
                                                       "<TYPE>r</TYPE>" +
                                                       "<Amt>" + Amount.ToString() + "</Amt>" +
                                                   //"<OVTYPE>1</OVTYPE>" +
                                                   "</CMP>");

                                    sbWork.Append("<WRK>" + sbWorkInfo + "<CMPARRAY>" + sbCMP + "</CMPARRAY>" + "</WRK>");
                                }
                                else
                                {
                                    sbWorkInfo.Clear();
                                    sbCMP.Clear();
                                    sbWorkInfo.Append("<WRKINFO>" +
                                                        "<GEO>" + _drEmpRow["Geocode"].ToString() + "</GEO>" +
                                                    "</WRKINFO>");

                                    sbCMP.Append("<CMP>" +
                                                     "<ID>0</ID>" +
                                                     "<TYPE>r</TYPE>" +
                                                     "<Amt>" + Amount.ToString() + "</Amt>" +
                                                 "</CMP>");

                                    sbWork.Append("<WRK>" + sbWorkInfo + "<CMPARRAY>" + sbCMP + "</CMPARRAY>" + "</WRK>");
                                }
                            }

                        }

                        if (dsHourWithLocation.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow dRow in dsHourWithLocation.Tables[1].Rows)
                            {
                                double Amount = Convert.ToDouble(dRow["Amount"]);

                                if (Amount > 0)
                                {
                                    sbWorkInfo.Clear();
                                    sbCMP.Clear();
                                    string geocode = _drEmpRow["Geocode"].ToString();
                                    sbWorkInfo.Append("<WRKINFO>" +
                                                           "<GEO>" + geocode + "</GEO>" +
                                                       "</WRKINFO>");
                                    if (Convert.ToBoolean(dRow["FIT"]) == false)
                                    {
                                        cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[1].Rows.IndexOf(dRow);
                                        sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                           "<TAXID>400</TAXID>" +
                                                           "<GEO>0</GEO>" +
                                                            "<TAX_TYPE>2</TAX_TYPE>" +
                                                           "<ID>" + cmpId + "</ID>" +
                                                           "<TYPE>r</TYPE>" +
                                                           "<OVTYPE>3</OVTYPE>" +
                                                       "</VPRT_COMPENSATION>");
                                    }
                                    if (Convert.ToBoolean(dRow["FICA"]) == false)
                                    {
                                        cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[1].Rows.IndexOf(dRow);
                                        sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                          "<TAXID>403</TAXID>" +
                                                          "<GEO>0</GEO>" +
                                                           "<TAX_TYPE>2</TAX_TYPE>" +
                                                          "<ID>" + cmpId + "</ID>" +
                                                          "<TYPE>r</TYPE>" +
                                                          "<OVTYPE>3</OVTYPE>" +
                                                      "</VPRT_COMPENSATION>");
                                    }
                                    if (Convert.ToBoolean(dRow["MEDI"]) == false)
                                    {
                                        cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[1].Rows.IndexOf(dRow);
                                        sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                         "<TAXID>406</TAXID>" +
                                                         "<GEO>0</GEO>" +
                                                          "<TAX_TYPE>2</TAX_TYPE>" +
                                                         "<ID>" + cmpId + "</ID>" +
                                                         "<TYPE>r</TYPE>" +
                                                         "<OVTYPE>3</OVTYPE>" +
                                                     "</VPRT_COMPENSATION>");
                                    }
                                    if (Convert.ToBoolean(dRow["FUTA"]) == false)
                                    {
                                        cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[1].Rows.IndexOf(dRow);
                                        sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                       "<TAXID>401</TAXID>" +
                                                       "<GEO>0</GEO>" +
                                                        "<TAX_TYPE>2</TAX_TYPE>" +
                                                       "<ID>" + cmpId + "</ID>" +
                                                       "<TYPE>r</TYPE>" +
                                                       "<OVTYPE>3</OVTYPE>" +
                                                   "</VPRT_COMPENSATION>");
                                    }
                                    if (Convert.ToBoolean(dRow["SIT"]) == false)
                                    {
                                        cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[1].Rows.IndexOf(dRow);
                                        sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
                                                      "<TAXID>450</TAXID>" +
                                                      "<GEO>" + dRow["sGeocode"].ToString() + "</GEO>" +
                                                       "<TAX_TYPE>2</TAX_TYPE>" +
                                                      "<ID>" + cmpId + "</ID>" +
                                                      "<TYPE>r</TYPE>" +
                                                      "<OVTYPE>3</OVTYPE>" +
                                                  "</VPRT_COMPENSATION>");
                                    }

                                    sbCMP.Append("<CMP>" +
                                                           "<ID>" + cmpId + "</ID>" +
                                                           "<TYPE>r</TYPE>" +
                                                           "<Amt>" + Amount.ToString() + "</Amt>" +
                                                       //"<OVTYPE>1</OVTYPE>" +
                                                       "</CMP>");

                                    sbWork.Append("<WRK>" + sbWorkInfo + "<CMPARRAY>" + sbCMP + "</CMPARRAY>" + "</WRK>");
                                }

                            }
                        }
                    }
                    else
                    {
                        sbWork.Append("<WRK><WRKINFO>" +
                            "<GEO>" + _drEmpRow["Geocode"].ToString() + "</GEO>" +
                            "</WRKINFO>" +
                            "<CMPARRAY>" +
                                "<CMP>" +
                                    "<ID>0</ID>" +
                                    "<TYPE>r</TYPE>" +
                                    "<Amt>" + dict["hdntotal"].ToString() + "</Amt>" +
                                "</CMP>" +
                            "</CMPARRAY>" +
                            "</WRK>");
                    }

                    StringBuilder stateJUR = new StringBuilder();
                    if (lstStateGeoCode != null)
                    {
                        foreach (string sGeo in lstStateGeoCode)
                        {
                            if (!String.IsNullOrEmpty(sGeo))
                            {
                                stateJUR.Append(
                                                  "<JUR>"
                                                      + "<TAXID>450</TAXID>"
                                                      + "<GEO>" + sGeo + "</GEO>"
                                                       + "<FILING_STAT>" + Filing_Stat + "</FILING_STAT>"
                                                           + "<PRI_EXEMPT>" + _drEmpRow["SAllow"].ToString() + "</PRI_EXEMPT>"
                                                      + "<CALCMETH>0</CALCMETH>"
                                                      + "<SUPL_METH>0</SUPL_METH>"
                                                  + "</JUR>");
                            }
                        }
                    }
                    else
                    {
                        stateJUR.Append(
                            "<JUR>"
                                + "<TAXID>450</TAXID>"
                                + "<GEO>" + _drEmpRow["VertexGeocode"].ToString() + "</GEO>"
                                 + "<FILING_STAT>" + Filing_Stat + "</FILING_STAT>"
                                     + "<PRI_EXEMPT>" + _drEmpRow["SAllow"].ToString() + "</PRI_EXEMPT>"
                                + "<CALCMETH>0</CALCMETH>"
                                + "<SUPL_METH>0</SUPL_METH>"
                            + "</JUR>");
                    }

                    if (lstGeo != null)
                    {
                        foreach (string cGeo in lstGeo.Distinct())
                        {
                            if (!String.IsNullOrEmpty(cGeo))
                            {
                                sbCityJUR.Append("<JUR>" +
                                                  "<TAXID>530</TAXID>" +
                                                  "<GEO>" + cGeo + "</GEO>" +
                                                  "<FILING_STAT>" + Filing_Stat + "</FILING_STAT>" +
                                                  "<PRI_EXEMPT>0</PRI_EXEMPT>" +
                                                  "<CALCMETH>0</CALCMETH>" +
                                                  "<SUPL_METH>0</SUPL_METH>" +
                                              "</JUR>");
                            }

                        }
                    }

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
               + "<PAYDATE>" + chkdateString + "</PAYDATE>";
                    if (dict["hdnPayPeriod"].ToString().Trim() == "0") //Weekly
                    {
                        payrollXML = payrollXML + "<PAYPERIODS>52</PAYPERIODS>";
                    }
                    else if (dict["hdnPayPeriod"].ToString().Trim() == "1") //Bi-Weekly
                    {
                        payrollXML = payrollXML + "<PAYPERIODS>26</PAYPERIODS>";
                    }
                    else
                    {
                        payrollXML = payrollXML + "<PAYPERIODS>1</PAYPERIODS>";
                    }
                    payrollXML = payrollXML + "<CURPERIOD>1</CURPERIOD>"
           + "<RES_GEO>" + empgeocode + "</RES_GEO>"
           + "<PRIMARY_WORK_GEO>" + empgeocode + "</PRIMARY_WORK_GEO>"
       + "</EMPSTRUCT>"

       + "<DEDUCTARRAY>"
            + sbDeduct
        + "</DEDUCTARRAY>"

   + "</EMPINFO>"
   + sbWork
   + "<JURARRAY>"
    + "<JUR>"
                + "<TAXID>400</TAXID>"
                + "<GEO>0</GEO>"
                + "<FILING_STAT>" + Filing_Stat + "</FILING_STAT>"
                 + "<PRI_EXEMPT>" + dict["hdnFAllow"].ToString().Trim() + "</PRI_EXEMPT>"
            + "<CALCMETH>0</CALCMETH>"
            + "<SUPL_METH>0</SUPL_METH>"
        + "</JUR>"

         //+ "<JUR>"
         //    + "<TAXID>400</TAXID>"
         //    + "<GEO>0</GEO>";
         //             if (dict["hdnFStatus"].ToString().Trim() == "1") //Married
         //             {
         //                 payrollXML = payrollXML + "<FILING_STAT>2</FILING_STAT>"; //Married
         //             }
         //             else if (dict["hdnFStatus"].ToString().Trim() == "1") //Single
         //             {
         //                 payrollXML = payrollXML + "<FILING_STAT>1</FILING_STAT>"; //Single
         //             }
         //             else
         //             {
         //                 payrollXML = payrollXML + "<FILING_STAT>1</FILING_STAT>";
         //             }
         //             payrollXML = payrollXML + "<PRI_EXEMPT>" + dict["hdnFAllow"].ToString().Trim() + "</PRI_EXEMPT>"
         //    + "<CALCMETH>0</CALCMETH>"
         //    + "<SUPL_METH>0</SUPL_METH>"
         //+ "</JUR>"
         + stateJUR
         + sbCityJUR
           + "</JURARRAY>"
                    //if (sbAggCMP != null)
                    //{
                    //    payrollXML = payrollXML + "<AGGCMPARRAY>" + sbAggCMP + "</AGGCMPARRAY>";
                    //}
                    //payrollXML = payrollXML
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
    + "<QUANTUM>"
    + "<VPRT_COMPENSATION_ARRAY>";
                    if (sbVprtCompensation != null)
                    {
                        payrollXML = payrollXML + sbVprtCompensation;
                    }
                    payrollXML = payrollXML
                    + "</VPRT_COMPENSATION_ARRAY>"
        + "</QUANTUM>"
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
                    ExceptionLogging.SendMsgToText("Employee " + dict["hdnName"].ToString().Trim() + " Tax API End : ");
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
                    ExceptionLogging.SendMsgToText("Data for Employee " + dict["hdnName"].ToString().Trim() + " End");
                }
            }
            ExceptionLogging.SendMsgToText("Loop on all employee End : ");
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
        ExceptionLogging.SendMsgToText("Payroll Submit End: ");

        ExceptionLogging.SendMsgToText("Payroll SP Start: ");
        dt.AcceptChanges();
        if (dt.Rows.Count > 0)
        {

            _objPRReg.Dt = dt;
            //ViewState["PaycheckDT"] = dt;
            _objPRReg.ConnConfig = Session["config"].ToString();
            //_objPRReg.StartDate = Convert.ToDateTime("05/28/2021");
            //_objPRReg.StartDate = Convert.ToDateTime("06/03/2021");
            _objPRReg.StartDate = Convert.ToDateTime(txtstartDt.Text);
            _objPRReg.EndDate = Convert.ToDateTime(txtendDt.Text);
            _objPRReg.CDate = Convert.ToDateTime(txtcheckdate.Text);
            _objPRReg.Bank = Convert.ToInt32(ddlBank.SelectedValue);
            _objPRReg.Memo = Convert.ToString(txtcheckmemo.Text);
            _objPRReg.WeekNo = 21;
            //_objPRReg.WeekNo = Convert.ToInt32(txtweek.Text);
            _objPRReg.Description = Convert.ToString(txtperioddesc.Text);
            _objPRReg.ProcessMethod = Convert.ToString(ddlGetTimeMethod.SelectedValue);
            _objPRReg.Supervisor = Convert.ToString(ddlSuper.SelectedValue);
            if (chkProcessOtherDeduction.Checked == true)
            {
                _objPRReg.PrcessDed = Convert.ToInt32("1");
            }
            else
            {
                _objPRReg.PrcessDed = Convert.ToInt32("0");
            }
            _objPRReg.Checkno = long.Parse(txtcheck.Text);
            _objPRReg.MOMUSer = Session["User"].ToString();
            DataSet _ReturnDS = new DataSet();
            _ReturnDS = objBL_Wage.ProcessPayroll(_objPRReg);
            ViewState["ReturnACHDetail"] = _ReturnDS;

            ExceptionLogging.SendMsgToText("Payroll SP End: ");
            string str = "Payroll process is done.";
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  noty({text: '" + str + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});setTimeout(function(){ location.reload(); }, 5000);", true);
            RadAjaxManager_WageDeduction.ResponseScripts.Add("OpentemplateModal();");

        }


    }
    private void AddBill()
    {
    }
    protected void ddlBank_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!ddlBank.SelectedValue.Equals("0"))
            {
                DataSet dsBank = new DataSet();
                _objBank.ConnConfig = Session["config"].ToString();
                _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);
                dsBank = _objBL_Bank.GetBankByID(_objBank);
                if (dsBank.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsBank.Tables[0].Rows[0];
                    long checkno = !string.IsNullOrEmpty(dr["NextC"].ToString()) ? long.Parse(dr["NextC"].ToString()) : 0;
                    var nextCheck = long.Parse(checkno.ToString());
                    ViewState["Checkno"] = nextCheck;
                    if ((!string.IsNullOrEmpty(checkno.ToString())))
                    {
                        txtcheck.Text = nextCheck.ToString();
                    }
                }
                else
                {
                    txtcheck.Text = "";
                }

            }
            else
            {
                txtcheck.Text = "";

            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "display write check", "AlertModal();", true);

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillDepartment()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getDepartment(objPropUser);

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlDepartment.Items.Clear();
            ddlDepartment.Items.Add(new System.Web.UI.WebControls.ListItem("--All--", "0"));
            ddlDepartment.AppendDataBoundItems = true;

            ddlDepartment.DataSource = ds.Tables[0];
            ddlDepartment.DataTextField = "type";
            ddlDepartment.DataValueField = "id";
            ddlDepartment.DataBind();
        }
        else
        {
            ddlDepartment.Items.Add(new System.Web.UI.WebControls.ListItem("No data found", "0"));
        }
    }

    private void FillPayFrequency()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getPayFrequencies(objPropUser);

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlPayFrequency.Items.Clear();
            ddlPayFrequency.Items.Add(new System.Web.UI.WebControls.ListItem("--Select Frequency--", "0"));
            ddlPayFrequency.AppendDataBoundItems = true;

            ddlPayFrequency.DataSource = ds.Tables[0];
            ddlPayFrequency.DataTextField = "ListText";
            ddlPayFrequency.DataValueField = "PickListValueId";
            ddlPayFrequency.DataBind();
        }
        else
        {
            ddlPayFrequency.Items.Add(new System.Web.UI.WebControls.ListItem("No data found", "0"));
        }
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

    #region Not_In_Use
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
        //upPannelSearch.Update();
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
    protected void ddlApTopCheckForLoad_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #endregion

    public DataSet GetPayrollHourWithLocation(User objPropUser)
    {
        //try
        //{
        //User objPropUser = new User();
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.FStart = Convert.ToDateTime(txtstartDt.Text);
        objPropUser.Edate = Convert.ToDateTime(txtendDt.Text);
        //objPropUser.FStart = Convert.ToDateTime("05/28/2021");
        //objPropUser.Edate = Convert.ToDateTime("06/03/2021");
        //objPropUser.ID = EmpID;
        //objPropUser.HolidayAm = holiday;
        //objPropUser.VacAm = vacation;
        //objPropUser.ZoneAm = zone;
        //objPropUser.ReimbAm = reimb;
        //objPropUser.MilageAm = mileage;
        //objPropUser.Bonus = bonus;
        //objPropUser.SickAm = sick;

        ds = new BL_Wage().GetPayrollHourWithLocation(objPropUser);
        return ds;
        //}
        //catch (Exception ex)
        //{
        //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        //    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        //}
    }

    private string GetWorkGeo(DataRow _dr)
    {
        string workgeo = "";
        string code = "";
        //////////////////////////////////////////
        //Credentials for Payroll OnDemand
        string username = ConfigurationManager.AppSettings["vertexApiUsername"].ToString(); // "dread@1000";
        string passWord = ConfigurationManager.AppSettings["vertexApiPassword"].ToString(); // "K3CHccxQ";
        // Setting up the URI for Payroll
        string uri = ConfigurationManager.AppSettings["vertexAddressURL"].ToString();
        //string uri = "https://payrollsandbox.ondemand.vertexinc.com:443/EiWebSvc/AddressWebService";
        string addrClnXML = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:eic = \"http://EiCalc/\">"
            + "<soapenv:Header/>"
            + "<soapenv:Body>"
            + "<eic:AddrCleanse>"
            + "<Request>"
            + "<![CDATA["
                + "<ADDRESS_CLEANSE_REQUEST>"
                    + "<StreetAddress1></StreetAddress1>"
                    + "<CityName>" + _dr["lCity"].ToString() + "</CityName>"
                    + "<StateName>" + _dr["lState"].ToString() + "</StateName>"
                    + "<ZipCode>" + _dr["lZip"].ToString() + "</ZipCode>"
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
                try
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
                        workgeo = nodedoc.ChildNodes.Item(0).InnerText;
                    }
                    else
                    {
                        workgeo = null;
                    }
                }
                catch (Exception ex)
                {
                    workgeo = null;
                }
                soapAddressEnvelope.Dispose();
                cl.Dispose();
            }
        }

        catch (AggregateException aggEx)
        {
            //If an error occurs outside of the service call capture and show below
            //Console.WriteLine("A Connection error occurred");
            //Console.WriteLine("----------------------------------------------------");
            //Console.WriteLine("Error Code: " + aggEx.Message.ToString() + Environment.NewLine + "Message: " + aggEx.InnerException.Message.ToString());

        }
        async Task<string> getMessage(HttpResponseMessage messageFromServer)
        {
            code = await messageFromServer.Content.ReadAsStringAsync();
            return code;
        }
        return workgeo;
    }

    protected void RadGrid_RunPayroll_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (EmployeeIds != null)
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    HiddenField hdnId = (HiddenField)dataItem.FindControl("hdnid");
                    int empId = Convert.ToInt32(hdnId.Value);
                    CheckBox chkSelect = (CheckBox)dataItem.FindControl("chkSelect");

                    List<int> empIds = EmployeeIds.Split(',').Select(int.Parse).ToList();

                    foreach (int id in empIds)
                    {
                        if (id == empId)
                        {
                            chkSelect.Checked = true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }


    //protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    //{
    //    updatePayrollRagister();
    //}

    //protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    //{
    //    updatePayrollRagister();
    //}

    //    [WebMethod]
    //    public string updatePayrollRagister()
    //    {
    //        ExceptionLogging.SendMsgToText("Payroll Submit Start: ");
    //        DataTable dt = GetTable();
    //        string code = "";
    //        string empgeocode = "000000000";
    //        ExceptionLogging.SendMsgToText("Get Work Geo Code Start: ");
    //        string workgeocode = GetWorkGeoCode();
    //        ExceptionLogging.SendMsgToText("Get Work Geo Code End: ");
    //        double FITdouble = 0;
    //        double SITdouble = 0;
    //        double Localdouble = 0;
    //        double Citydouble = 0;
    //        double Medidouble = 0;
    //        double Ficadouble = 0;
    //        string uname = "";
    //        string strerrorMessage = "";
    //        string strItems = hdnGLItem.Value.Trim();
    //        if (strItems != string.Empty)
    //        {
    //            JavaScriptSerializer sr = new JavaScriptSerializer();
    //            List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
    //            objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
    //            int i = 0;
    //            objEstimateItemData.RemoveAt(0);

    //            ExceptionLogging.SendMsgToText("Loop on all employee start : ");
    //            foreach (Dictionary<object, object> dict in objEstimateItemData)
    //            {
    //                if (dict.ContainsKey("chkSelect"))
    //                {
    //                    i++;
    //                    DataRow dr = dt.NewRow();
    //                    ExceptionLogging.SendMsgToText("Data for Employee " + dict["hdnName"].ToString().Trim());
    //                    dr["ID"] = dict["hdnid"].ToString().Trim();
    //                    dr["Name"] = dict["hdnName"].ToString().Trim();
    //                    dr["Reg"] = double.Parse(dict["hdnlblReg"].ToString().Trim());
    //                    dr["OT"] = double.Parse(dict["hdnlblOT"].ToString().Trim());
    //                    dr["NT"] = double.Parse(dict["hdnlblNT"].ToString().Trim());
    //                    dr["DT"] = double.Parse(dict["hdnlblDT"].ToString().Trim());
    //                    dr["TT"] = double.Parse(dict["hdnlblTT"].ToString().Trim());

    //                    dr["Zone"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnZone"].ToString().Trim());
    //                    dr["Milage"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnMileage"].ToString());

    //                    //dr["Toll"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnToll"].ToString());
    //                    dr["Toll"] = 0.00;
    //                    dr["OtherE"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnOtherE"].ToString());
    //                    dr["pay"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnpay"].ToString());

    //                    dr["holiday"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnlblHoliday"].ToString().Trim());
    //                    dr["vacation"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnlblVac"].ToString().Trim());
    //                    dr["sicktime"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnsicktime"].ToString().Trim());
    //                    dr["reimb"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnReimb"].ToString().Trim());
    //                    dr["bonus"] = ConvertCurrentCurrencyFormatToDbl(dict["hdnBonus"].ToString());

    //                    if (dict["hdnlblMethod"].ToString().Trim() != null)
    //                    {
    //                        dr["paymethod"] = dict["hdnlblMethod"].ToString();
    //                    }
    //                    else
    //                    {
    //                        dr["paymethod"] = "";
    //                    }
    //                    if (dict["hdnpmethod"].ToString().Trim() != null)
    //                    {
    //                        dr["pmethod"] = dict["hdnpmethod"].ToString();
    //                    }
    //                    else
    //                    {
    //                        dr["pmethod"] = "0";
    //                    }

    //                    dr["userid"] = dict["hdnuserid"].ToString();
    //                    dr["usertype"] = dict["hdnusertype"].ToString();
    //                    if (dict["hdntotal"].ToString().Trim() != null && dict["hdntotal"].ToString().Trim() != "")
    //                    {
    //                        dr["total"] = dict["hdntotal"].ToString();
    //                    }
    //                    else
    //                    {
    //                        dr["total"] = 0.00;
    //                    }
    //                    if (dict["hdnphour"].ToString().Trim() != null && dict["hdntotal"].ToString().Trim() != "")
    //                    {
    //                        dr["phour"] = dict["hdnphour"].ToString();
    //                    }
    //                    else
    //                    {
    //                        dr["phour"] = 0.00;
    //                    }
    //                    if (dict["hdnsalary"].ToString().Trim() != null && dict["hdntotal"].ToString().Trim() != "")
    //                    {
    //                        dr["salary"] = dict["hdnsalary"].ToString();
    //                    }
    //                    else
    //                    {
    //                        dr["salary"] = 0.00;
    //                    }
    //                    if (dict["hdnHourlyRate"].ToString().Trim() != null && dict["hdntotal"].ToString().Trim() != "")
    //                    {
    //                        dr["HourlyRate"] = dict["hdnHourlyRate"].ToString();
    //                    }
    //                    else
    //                    {
    //                        dr["HourlyRate"] = 0.00;
    //                    }
    //                    empgeocode = dict["hdnGeocode"].ToString().Trim();

    //                    string username = ConfigurationManager.AppSettings["vertexApiUsername"].ToString();
    //                    string passWord = ConfigurationManager.AppSettings["vertexApiPassword"].ToString();

    //                    ExceptionLogging.SendMsgToText("Employee " + dict["hdnName"].ToString().Trim() + " Tax API Start : ");
    //                    string payrolluri = ConfigurationManager.AppSettings["vertexPayrollURL"].ToString();
    //                    var chkdateString = Convert.ToDateTime(txtcheckdate.Text).ToString("yyyyMMdd");

    //                    double dedAmt401K = 0;
    //                    double dedAmt401KID = 0;
    //                    User objPropUser = new User();
    //                    DataSet ds = new DataSet();
    //                    objPropUser.ConnConfig = Session["config"].ToString();
    //                    objPropUser.FStart = Convert.ToDateTime(txtstartDt.Text);
    //                    objPropUser.Edate = Convert.ToDateTime(txtendDt.Text);
    //                    objPropUser.HolidayAm = ConvertCurrentCurrencyFormatToDbl(dict["hdnlblHoliday"].ToString().Trim());
    //                    objPropUser.VacAm = ConvertCurrentCurrencyFormatToDbl(dict["hdnlblVac"].ToString().Trim());
    //                    objPropUser.ZoneAm = ConvertCurrentCurrencyFormatToDbl(dict["hdnZone"].ToString().Trim());
    //                    objPropUser.ReimbAm = ConvertCurrentCurrencyFormatToDbl(dict["hdnReimb"].ToString().Trim());
    //                    objPropUser.MilageAm = ConvertCurrentCurrencyFormatToDbl(dict["hdnMileage"].ToString());
    //                    objPropUser.BonusAm = ConvertCurrentCurrencyFormatToDbl(dict["hdnBonus"].ToString());
    //                    int ProcessDed = 0;
    //                    if (chkProcessOtherDeduction.Checked == true)
    //                    {
    //                        ProcessDed = Convert.ToInt32("1");
    //                    }
    //                    else
    //                    {
    //                        ProcessDed = Convert.ToInt32("0");
    //                    }
    //                    objPropUser.ID = Convert.ToInt32(dict["hdnid"].ToString().Trim());

    //                    ds = new BL_Wage().GetPayrollDeductions(objPropUser, ProcessDed);
    //                    DataTable dtDedTablePre = ds.Tables[0];

    //                    StringBuilder sbDeduct = new StringBuilder();

    //                    DataRow drtemp = dtDedTablePre.Select("ID=1").FirstOrDefault();
    //                    if (drtemp != null)
    //                    {
    //                        dedAmt401K = Convert.ToDouble(drtemp["Amount"]);
    //                        dedAmt401KID = Convert.ToDouble(drtemp["ID"]);
    //                    }
    //                    if (dtDedTablePre.Rows.Count > 0)
    //                    {
    //                        dtDedTablePre.DefaultView.RowFilter = "PaidBy = 1";
    //                        dtDedTablePre = (dtDedTablePre.DefaultView).ToTable();

    //                        foreach (DataRow row in dtDedTablePre.Rows)
    //                        {
    //                            double dedAmt = 0;
    //                            double? dedID = null;
    //                            if (row["VertexDedutionId"] != DBNull.Value)
    //                            {
    //                                dedID = Convert.ToDouble(row["VertexDedutionId"]);
    //                            }
    //                            dedAmt = Convert.ToDouble(row["Amount"]);
    //                            if (dedID != null && dedAmt > 0)
    //                            {
    //                                sbDeduct.Append("<DEDUCT>" +
    //                                    " <ID>" + dedID + "</ID> " +
    //                                    "<Amt>" + dedAmt + "</Amt> " +
    //                                    "</DEDUCT>");
    //                            }
    //                            else if (dedID == null && dedAmt > 0)
    //                            {
    //                                sbDeduct.Append("<DEDUCT>" +
    //                                   "<ID>46</ID> " +
    //                                   "<Amt>" + dedAmt + "</Amt> " +
    //                                   "</DEDUCT>");
    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        sbDeduct.Append("<DEDUCT>" +
    //                           " <ID>46</ID> " +
    //                           "<Amt>" + dedAmt401K + "</Amt> " +
    //                           "</DEDUCT>");
    //                    }

    //                    DataSet dsemp = new DataSet();
    //                    _objEmp.ConnConfig = Session["config"].ToString();
    //                    _objEmp.ID = Convert.ToInt32(dict["hdnid"].ToString().Trim());

    //                    dsemp = objBL_Wage.GetEmployeeListByID(_objEmp);
    //                    DataRow _drEmpRow = dsemp.Tables[0].Rows[0];

    //                    if (Convert.ToInt32(_drEmpRow["FAllow"].ToString()) == -1)
    //                    {
    //                        _drEmpRow["FAllow"] = 0;
    //                    }

    //                    objPropUser.ID = Convert.ToInt32(dict["hdnid"].ToString().Trim());

    //                    DataSet dsHourWithLocation = GetPayrollHourWithLocation(objPropUser);
    //                    StringBuilder sbWork = new StringBuilder();
    //                    StringBuilder sbCityJUR = new StringBuilder();
    //                    int Filing_Stat = 1;
    //                    if (_drEmpRow["FStatus"].ToString() == "1") //Married
    //                    {
    //                        Filing_Stat = 2; //Married
    //                    }
    //                    else if (_drEmpRow["FStatus"].ToString() == "0") //Single
    //                    {
    //                        Filing_Stat = 1; //Single
    //                    }
    //                    List<string> lstStateGeoCode = new List<string>();
    //                    StringBuilder sbWorkInfo = new StringBuilder();
    //                    StringBuilder sbCMP = new StringBuilder();
    //                    StringBuilder sbAggCMP = new StringBuilder();
    //                    StringBuilder sbVprtCompensation = new StringBuilder();
    //                    List<string> lstGeo = new List<string>();
    //                    int cmpId = 0;

    //                    if (dsHourWithLocation.Tables[0].Rows.Count > 0)
    //                    {
    //                        lstStateGeoCode = (dsHourWithLocation.Tables[0].AsEnumerable().Select(x => x["sGeocode"].ToString()).Distinct()).ToList();
    //                        foreach (DataRow dRow in dsHourWithLocation.Tables[0].Rows)
    //                        {
    //                            double Amount = Convert.ToDouble(dRow["Amount"]);
    //                            if (Amount > 0)
    //                            {
    //                                if (dRow["lCity"].ToString() != "" && dRow["lState"].ToString() != "" && dRow["lZip"].ToString() != "")
    //                                {
    //                                    sbWorkInfo.Clear();
    //                                    sbCMP.Clear();
    //                                    //sbAggCMP.Clear();
    //                                    string geocode = GetWorkGeo(dRow);

    //                                    if (geocode == null)
    //                                    {
    //                                        geocode = _drEmpRow["Geocode"].ToString();
    //                                    }

    //                                    lstGeo.Add(geocode);
    //                                    sbWorkInfo.Append("<WRKINFO>" +
    //                                                           "<GEO>" + geocode + "</GEO>" +
    //                                                       "</WRKINFO>");
    //                                    if (Convert.ToBoolean(dRow["FIT"]) == false)
    //                                    {
    //                                        cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[0].Rows.IndexOf(dRow);
    //                                        sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
    //                                                           "<TAXID>400</TAXID>" +
    //                                                           "<GEO>0</GEO>" +
    //                                                            "<TAX_TYPE>2</TAX_TYPE>" +
    //                                                           "<ID>" + cmpId + "</ID>" +
    //                                                           "<TYPE>r</TYPE>" +
    //                                                           "<OVTYPE>3</OVTYPE>" +
    //                                                       "</VPRT_COMPENSATION>");
    //                                    }
    //                                    if (Convert.ToBoolean(dRow["FICA"]) == false)
    //                                    {
    //                                        cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[0].Rows.IndexOf(dRow);
    //                                        sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
    //                                                          "<TAXID>403</TAXID>" +
    //                                                          "<GEO>0</GEO>" +
    //                                                           "<TAX_TYPE>2</TAX_TYPE>" +
    //                                                          "<ID>" + cmpId + "</ID>" +
    //                                                          "<TYPE>r</TYPE>" +
    //                                                          "<OVTYPE>3</OVTYPE>" +
    //                                                      "</VPRT_COMPENSATION>");
    //                                    }
    //                                    if (Convert.ToBoolean(dRow["MEDI"]) == false)
    //                                    {
    //                                        cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[0].Rows.IndexOf(dRow);
    //                                        sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
    //                                                         "<TAXID>406</TAXID>" +
    //                                                         "<GEO>0</GEO>" +
    //                                                          "<TAX_TYPE>2</TAX_TYPE>" +
    //                                                         "<ID>" + cmpId + "</ID>" +
    //                                                         "<TYPE>r</TYPE>" +
    //                                                         "<OVTYPE>3</OVTYPE>" +
    //                                                     "</VPRT_COMPENSATION>");
    //                                    }
    //                                    if (Convert.ToBoolean(dRow["FUTA"]) == false)
    //                                    {
    //                                        cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[0].Rows.IndexOf(dRow);
    //                                        sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
    //                                                       "<TAXID>401</TAXID>" +
    //                                                       "<GEO>0</GEO>" +
    //                                                        "<TAX_TYPE>2</TAX_TYPE>" +
    //                                                       "<ID>" + cmpId + "</ID>" +
    //                                                       "<TYPE>r</TYPE>" +
    //                                                       "<OVTYPE>3</OVTYPE>" +
    //                                                   "</VPRT_COMPENSATION>");
    //                                    }
    //                                    if (Convert.ToBoolean(dRow["SIT"]) == false)
    //                                    {
    //                                        cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[0].Rows.IndexOf(dRow);
    //                                        sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
    //                                                      "<TAXID>450</TAXID>" +
    //                                                      "<GEO>" + dRow["sGeocode"].ToString() + "</GEO>" +
    //                                                       "<TAX_TYPE>2</TAX_TYPE>" +
    //                                                      "<ID>" + cmpId + "</ID>" +
    //                                                      "<TYPE>r</TYPE>" +
    //                                                      "<OVTYPE>3</OVTYPE>" +
    //                                                  "</VPRT_COMPENSATION>");
    //                                    }
    //                                    sbCMP.Append("<CMP>" +
    //                                                       "<ID>" + cmpId + "</ID>" +
    //                                                       "<TYPE>r</TYPE>" +
    //                                                       "<Amt>" + Amount.ToString() + "</Amt>" +
    //                                                   //"<OVTYPE>1</OVTYPE>" +
    //                                                   "</CMP>");

    //                                    sbWork.Append("<WRK>" + sbWorkInfo + "<CMPARRAY>" + sbCMP + "</CMPARRAY>" + "</WRK>");
    //                                }
    //                                else
    //                                {
    //                                    sbWorkInfo.Clear();
    //                                    sbCMP.Clear();
    //                                    sbWorkInfo.Append("<WRKINFO>" +
    //                                                        "<GEO>" + _drEmpRow["Geocode"].ToString() + "</GEO>" +
    //                                                    "</WRKINFO>");

    //                                    sbCMP.Append("<CMP>" +
    //                                                     "<ID>0</ID>" +
    //                                                     "<TYPE>r</TYPE>" +
    //                                                     "<Amt>" + Amount.ToString() + "</Amt>" +
    //                                                 "</CMP>");

    //                                    sbWork.Append("<WRK>" + sbWorkInfo + "<CMPARRAY>" + sbCMP + "</CMPARRAY>" + "</WRK>");
    //                                }
    //                            }

    //                        }

    //                        if (dsHourWithLocation.Tables[1].Rows.Count > 0)
    //                        {
    //                            foreach (DataRow dRow in dsHourWithLocation.Tables[1].Rows)
    //                            {
    //                                double Amount = Convert.ToDouble(dRow["Amount"]);

    //                                if (Amount > 0)
    //                                {
    //                                    sbWorkInfo.Clear();
    //                                    sbCMP.Clear();
    //                                    string geocode = _drEmpRow["Geocode"].ToString();
    //                                    sbWorkInfo.Append("<WRKINFO>" +
    //                                                           "<GEO>" + geocode + "</GEO>" +
    //                                                       "</WRKINFO>");
    //                                    if (Convert.ToBoolean(dRow["FIT"]) == false)
    //                                    {
    //                                        cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[1].Rows.IndexOf(dRow);
    //                                        sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
    //                                                           "<TAXID>400</TAXID>" +
    //                                                           "<GEO>0</GEO>" +
    //                                                            "<TAX_TYPE>2</TAX_TYPE>" +
    //                                                           "<ID>" + cmpId + "</ID>" +
    //                                                           "<TYPE>r</TYPE>" +
    //                                                           "<OVTYPE>3</OVTYPE>" +
    //                                                       "</VPRT_COMPENSATION>");
    //                                    }
    //                                    if (Convert.ToBoolean(dRow["FICA"]) == false)
    //                                    {
    //                                        cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[1].Rows.IndexOf(dRow);
    //                                        sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
    //                                                          "<TAXID>403</TAXID>" +
    //                                                          "<GEO>0</GEO>" +
    //                                                           "<TAX_TYPE>2</TAX_TYPE>" +
    //                                                          "<ID>" + cmpId + "</ID>" +
    //                                                          "<TYPE>r</TYPE>" +
    //                                                          "<OVTYPE>3</OVTYPE>" +
    //                                                      "</VPRT_COMPENSATION>");
    //                                    }
    //                                    if (Convert.ToBoolean(dRow["MEDI"]) == false)
    //                                    {
    //                                        cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[1].Rows.IndexOf(dRow);
    //                                        sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
    //                                                         "<TAXID>406</TAXID>" +
    //                                                         "<GEO>0</GEO>" +
    //                                                          "<TAX_TYPE>2</TAX_TYPE>" +
    //                                                         "<ID>" + cmpId + "</ID>" +
    //                                                         "<TYPE>r</TYPE>" +
    //                                                         "<OVTYPE>3</OVTYPE>" +
    //                                                     "</VPRT_COMPENSATION>");
    //                                    }
    //                                    if (Convert.ToBoolean(dRow["FUTA"]) == false)
    //                                    {
    //                                        cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[1].Rows.IndexOf(dRow);
    //                                        sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
    //                                                       "<TAXID>401</TAXID>" +
    //                                                       "<GEO>0</GEO>" +
    //                                                        "<TAX_TYPE>2</TAX_TYPE>" +
    //                                                       "<ID>" + cmpId + "</ID>" +
    //                                                       "<TYPE>r</TYPE>" +
    //                                                       "<OVTYPE>3</OVTYPE>" +
    //                                                   "</VPRT_COMPENSATION>");
    //                                    }
    //                                    if (Convert.ToBoolean(dRow["SIT"]) == false)
    //                                    {
    //                                        cmpId = 1100 + Convert.ToInt32(dRow["ID"]) + dsHourWithLocation.Tables[1].Rows.IndexOf(dRow);
    //                                        sbVprtCompensation.Append("<VPRT_COMPENSATION>" +
    //                                                      "<TAXID>450</TAXID>" +
    //                                                      "<GEO>" + dRow["sGeocode"].ToString() + "</GEO>" +
    //                                                       "<TAX_TYPE>2</TAX_TYPE>" +
    //                                                      "<ID>" + cmpId + "</ID>" +
    //                                                      "<TYPE>r</TYPE>" +
    //                                                      "<OVTYPE>3</OVTYPE>" +
    //                                                  "</VPRT_COMPENSATION>");
    //                                    }

    //                                    sbCMP.Append("<CMP>" +
    //                                                           "<ID>" + cmpId + "</ID>" +
    //                                                           "<TYPE>r</TYPE>" +
    //                                                           "<Amt>" + Amount.ToString() + "</Amt>" +
    //                                                       "</CMP>");

    //                                    sbWork.Append("<WRK>" + sbWorkInfo + "<CMPARRAY>" + sbCMP + "</CMPARRAY>" + "</WRK>");
    //                                }

    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        sbWork.Append("<WRK><WRKINFO>" +
    //                            "<GEO>" + _drEmpRow["Geocode"].ToString() + "</GEO>" +
    //                            "</WRKINFO>" +
    //                            "<CMPARRAY>" +
    //                                "<CMP>" +
    //                                    "<ID>0</ID>" +
    //                                    "<TYPE>r</TYPE>" +
    //                                    "<Amt>" + dict["hdntotal"].ToString() + "</Amt>" +
    //                                "</CMP>" +
    //                            "</CMPARRAY>" +
    //                            "</WRK>");
    //                    }

    //                    StringBuilder stateJUR = new StringBuilder();
    //                    if (lstStateGeoCode != null)
    //                    {
    //                        foreach (string sGeo in lstStateGeoCode)
    //                        {
    //                            if (!String.IsNullOrEmpty(sGeo))
    //                            {
    //                                stateJUR.Append(
    //                                                  "<JUR>"
    //                                                      + "<TAXID>450</TAXID>"
    //                                                      + "<GEO>" + sGeo + "</GEO>"
    //                                                       + "<FILING_STAT>" + Filing_Stat + "</FILING_STAT>"
    //                                                           + "<PRI_EXEMPT>" + _drEmpRow["SAllow"].ToString() + "</PRI_EXEMPT>"
    //                                                      + "<CALCMETH>0</CALCMETH>"
    //                                                      + "<SUPL_METH>0</SUPL_METH>"
    //                                                  + "</JUR>");
    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        stateJUR.Append(
    //                            "<JUR>"
    //                                + "<TAXID>450</TAXID>"
    //                                + "<GEO>" + _drEmpRow["VertexGeocode"].ToString() + "</GEO>"
    //                                 + "<FILING_STAT>" + Filing_Stat + "</FILING_STAT>"
    //                                     + "<PRI_EXEMPT>" + _drEmpRow["SAllow"].ToString() + "</PRI_EXEMPT>"
    //                                + "<CALCMETH>0</CALCMETH>"
    //                                + "<SUPL_METH>0</SUPL_METH>"
    //                            + "</JUR>");
    //                    }

    //                    if (lstGeo != null)
    //                    {
    //                        foreach (string cGeo in lstGeo.Distinct())
    //                        {
    //                            if (!String.IsNullOrEmpty(cGeo))
    //                            {
    //                                sbCityJUR.Append("<JUR>" +
    //                                                  "<TAXID>530</TAXID>" +
    //                                                  "<GEO>" + cGeo + "</GEO>" +
    //                                                  "<FILING_STAT>" + Filing_Stat + "</FILING_STAT>" +
    //                                                  "<PRI_EXEMPT>0</PRI_EXEMPT>" +
    //                                                  "<CALCMETH>0</CALCMETH>" +
    //                                                  "<SUPL_METH>0</SUPL_METH>" +
    //                                              "</JUR>");
    //                            }

    //                        }
    //                    }

    //                    string payrollXML = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:eic=\"http://EiCalc/\">"
    //                        + "<soapenv:Header/>"
    //                        + "<soapenv:Body>"
    //                        + "<eic:EiOperation>"
    //                          + "<Request>"
    //           + "<![CDATA["
    //    + "<EMP>"
    //       + "<EMPINFO>"
    //           + "<EMPSTRUCT>"
    //               + "<EMPID>ESS</EMPID>"
    //               + "<PAYDATE>" + chkdateString + "</PAYDATE>";
    //                    if (dict["hdnPayPeriod"].ToString().Trim() == "0") //Weekly
    //                    {
    //                        payrollXML = payrollXML + "<PAYPERIODS>52</PAYPERIODS>";
    //                    }
    //                    else if (dict["hdnPayPeriod"].ToString().Trim() == "1") //Bi-Weekly
    //                    {
    //                        payrollXML = payrollXML + "<PAYPERIODS>26</PAYPERIODS>";
    //                    }
    //                    else
    //                    {
    //                        payrollXML = payrollXML + "<PAYPERIODS>1</PAYPERIODS>";
    //                    }
    //                    payrollXML = payrollXML + "<CURPERIOD>1</CURPERIOD>"
    //           + "<RES_GEO>" + empgeocode + "</RES_GEO>"
    //           + "<PRIMARY_WORK_GEO>" + empgeocode + "</PRIMARY_WORK_GEO>"
    //       + "</EMPSTRUCT>"

    //       + "<DEDUCTARRAY>"
    //            + sbDeduct
    //        + "</DEDUCTARRAY>"

    //   + "</EMPINFO>"
    //   + sbWork
    //   + "<JURARRAY>"
    //    + "<JUR>"
    //                + "<TAXID>400</TAXID>"
    //                + "<GEO>0</GEO>"
    //                + "<FILING_STAT>" + Filing_Stat + "</FILING_STAT>"
    //                 + "<PRI_EXEMPT>" + dict["hdnFAllow"].ToString().Trim() + "</PRI_EXEMPT>"
    //            + "<CALCMETH>0</CALCMETH>"
    //            + "<SUPL_METH>0</SUPL_METH>"
    //        + "</JUR>"
    //         + stateJUR
    //         + sbCityJUR
    //           + "</JURARRAY>"

    //       + "<TAXAMTARRAY>"
    //       + "<AGGTAX>"
    //           + "<TAXID>400</TAXID>"
    //           + "<GEO>0</GEO>"
    //           + "<TAX_AMT>0</TAX_AMT>"
    //           + "<AGG_TYPE>Y</AGG_TYPE>"
    //           + "<TYPE>r</TYPE>"
    //           + "<AGG_ADJ_GROSS>0</AGG_ADJ_GROSS>"
    //       + "</AGGTAX>"
    //   + "</TAXAMTARRAY>"
    //    + "<QUANTUM>"
    //    + "<VPRT_COMPENSATION_ARRAY>";
    //                    if (sbVprtCompensation != null)
    //                    {
    //                        payrollXML = payrollXML + sbVprtCompensation;
    //                    }
    //                    payrollXML = payrollXML
    //                    + "</VPRT_COMPENSATION_ARRAY>"
    //        + "</QUANTUM>"
    //    + "</EMP>"
    //+ "]]>"
    //       + "</Request>"
    //      + "</eic:EiOperation>"
    //     + "</soapenv:Body>"
    //    + "</soapenv:Envelope> ";

    //                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
    //                    HttpClient clpay = new HttpClient();
    //                    clpay.BaseAddress = new Uri(payrolluri);
    //                    clpay.DefaultRequestHeaders.Clear();
    //                    clpay.DefaultRequestHeaders.Add("javax.xml.ws.security.auth.username", username);
    //                    clpay.DefaultRequestHeaders.Add("javax.xml.ws.security.auth.password", passWord);
    //                    HttpContent soapPayrollCalcTax = new StringContent(payrollXML);
    //                    soapPayrollCalcTax.Headers.Clear();
    //                    soapPayrollCalcTax.Headers.Add("Content-Type", "text/xml");
    //                    using (HttpResponseMessage response = clpay.PostAsync(payrolluri, soapPayrollCalcTax).Result)
    //                    {
    //                        string rawString = getMessage(response).Result;

    //                        XmlDocument responseXML = new XmlDocument();
    //                        responseXML.LoadXml(rawString);
    //                        XDocument responseXMLPretty = XDocument.Parse(responseXML.InnerText.ToString());

    //                        string responseXMLPrettystrr = responseXMLPretty.ToString();
    //                        responseXMLPrettystrr = responseXMLPrettystrr.Replace("\"", "'");
    //                        XmlDocument Docc = new XmlDocument();
    //                        Docc.LoadXml(responseXMLPrettystrr);
    //                        XmlNode Errornode = Docc.GetElementsByTagName("Error").Item(0);
    //                        if (Errornode == null)
    //                        {
    //                            XmlNodeList elemList = Docc.GetElementsByTagName("TaxOut");
    //                            for (int j = 0; j < elemList.Count; j++)
    //                            {
    //                                if (elemList[j]["TAXID"].InnerText == "400")
    //                                {
    //                                    FITdouble = Convert.ToDouble(elemList[j]["TAX_AMT"].InnerText);
    //                                }
    //                                if (elemList[j]["TAXID"].InnerText == "450")
    //                                {
    //                                    SITdouble = Convert.ToDouble(elemList[j]["TAX_AMT"].InnerText);
    //                                }
    //                                if (elemList[j]["TAXID"].InnerText == "530")
    //                                {
    //                                    Citydouble = Convert.ToDouble(elemList[j]["TAX_AMT"].InnerText);
    //                                }
    //                                if (elemList[j]["TAXID"].InnerText == "406")
    //                                {
    //                                    Medidouble = Convert.ToDouble(elemList[j]["TAX_AMT"].InnerText);
    //                                }
    //                                if (elemList[j]["TAXID"].InnerText == "403")
    //                                {
    //                                    Ficadouble = Convert.ToDouble(elemList[j]["TAX_AMT"].InnerText);
    //                                }

    //                            }
    //                        }
    //                        else
    //                        {
    //                            strerrorMessage = "Error in Address API for " + uname;
    //                            break;

    //                        }
    //                        soapPayrollCalcTax.Dispose();
    //                        clpay.Dispose();
    //                    }
    //                    ExceptionLogging.SendMsgToText("Employee " + dict["hdnName"].ToString().Trim() + " Tax API End : ");

    //                    async Task<string> getMessage(HttpResponseMessage messageFromServer)
    //                    {
    //                        code = await messageFromServer.Content.ReadAsStringAsync();
    //                        return code;
    //                    }

    //                    //////////////////////////////////////////
    //                    dr["FIT"] = FITdouble;
    //                    dr["SIT"] = SITdouble;
    //                    dr["LOCAL"] = Localdouble;
    //                    dr["MEDI"] = Medidouble;
    //                    dr["FICA"] = Ficadouble;
    //                    dt.Rows.Add(dr);
    //                    //}
    //                    ExceptionLogging.SendMsgToText("Data for Employee " + dict["hdnName"].ToString().Trim() + " End");
    //                }
    //            }
    //            ExceptionLogging.SendMsgToText("Loop on all employee End : ");
    //        }

    //        if (strerrorMessage != "")
    //        {
    //            dt.Clear();
    //            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  noty({text: '" + strerrorMessage + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
    //        }
    //        ExceptionLogging.SendMsgToText("Payroll Submit End: ");

    //        ExceptionLogging.SendMsgToText("Payroll SP Start: ");
    //        dt.AcceptChanges();
    //        if (dt.Rows.Count > 0)
    //        {

    //            _objPRReg.Dt = dt;
    //            _objPRReg.ConnConfig = Session["config"].ToString();
    //            _objPRReg.StartDate = Convert.ToDateTime(txtstartDt.Text);
    //            _objPRReg.EndDate = Convert.ToDateTime(txtendDt.Text);
    //            _objPRReg.CDate = Convert.ToDateTime(txtcheckdate.Text);
    //            _objPRReg.Bank = Convert.ToInt32(ddlBank.SelectedValue);
    //            _objPRReg.Memo = Convert.ToString(txtcheckmemo.Text);
    //            _objPRReg.WeekNo = 21;
    //            //_objPRReg.WeekNo = Convert.ToInt32(txtweek.Text);
    //            _objPRReg.Description = Convert.ToString(txtperioddesc.Text);
    //            _objPRReg.ProcessMethod = Convert.ToString(ddlGetTimeMethod.SelectedValue);
    //            _objPRReg.Supervisor = Convert.ToString(ddlSuper.SelectedValue);
    //            if (chkProcessOtherDeduction.Checked == true)
    //            {
    //                _objPRReg.PrcessDed = Convert.ToInt32("1");
    //            }
    //            else
    //            {
    //                _objPRReg.PrcessDed = Convert.ToInt32("0");
    //            }
    //            _objPRReg.SupervisorId = 0;
    //            _objPRReg.DepartmentId = Convert.ToInt32(ddlDepartment.SelectedValue);
    //            _objPRReg.FrequencyId = Convert.ToInt32(ddlPayFrequency.SelectedValue); ;
    //            _objPRReg.GrossPay = 0;
    //            _objPRReg.TotalDeduction = 0;
    //            _objPRReg.NetPay = 0;

    //            //_objPRReg.Checkno = long.Parse(txtcheck.Text);
    //            _objPRReg.MOMUSer = Session["User"].ToString();
    //            //DataSet _ReturnDS = new DataSet();
    //            objBL_Wage.AddPayrollRagister(_objPRReg);
    //            //ViewState["ReturnACHDetail"] = _ReturnDS;

    //            //ExceptionLogging.SendMsgToText("Payroll SP End: ");
    //            //string str = "Payroll process is done.";
    //            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  noty({text: '" + str + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});setTimeout(function(){ location.reload(); }, 5000);", true);
    //            //RadAjaxManager_WageDeduction.ResponseScripts.Add("OpentemplateModal();");

    //        }

    //        string msg = "";
    //        return JsonConvert.SerializeObject(msg);
    //    }
}

