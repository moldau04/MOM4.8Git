using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using BusinessEntity;
using System.Data;
using Telerik.Web.UI;
using System.Web.Services;
using System.Collections.Generic;
using Microsoft.ApplicationBlocks.Data;
using System.Web;
using BusinessLayer.Programs;
using BusinessEntity.Programs;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;

public partial class AddEmployee : System.Web.UI.Page    
    {

        BL_User objBL_User = new BL_User();
        BL_Wage objBL_Wage = new BL_Wage();
        User objProp_User = new User();
    Emp _objEmp = new Emp();
    BL_Company objBL_Company = new BL_Company();
    BL_BankAccount _objBLBank = new BL_BankAccount();
    
    
    BusinessEntity.CompanyOffice objCompany = new BusinessEntity.CompanyOffice();
    //consult
    tblConsult objProp_Consult = new tblConsult();
        BL_General objBL_General = new BL_General();
        General objGeneral = new General();
        BL_Customer objBL_Customer = new BL_Customer();
        BL_Vendor objBL_Vendor = new BL_Vendor();
        Customer objCustomer = new Customer();
        BL_ReportsData objBL_ReportData = new BL_ReportsData();
        GeneralFunctions objGeneralFunctions = new GeneralFunctions();
        public static bool IsAddEdit = false;
        public static bool IsDelete = false;
        Wage _objWage = new Wage();
    PRDed _objPRDed = new PRDed();
        bool api = false;
    protected DataTable dtWage = new DataTable();
    protected DataTable dtWageDeduction = new DataTable();
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!CheckAddEditPermission()) { Response.Redirect("Home.aspx?permission=no"); return; }

        if (!IsPostBack)
        {

            HighlightSideMenu("prID", "Employeelink", "payrollmenutab");
        }
        FillWageCategorys();
        FillWageDeductionCategorys();
        try
        {
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }
            string _connectionString = Session["config"].ToString();

            if (!IsPostBack)
            {
                GetControlData();
                userpermissions();
                CompanyPermission();
                ViewState["editcon"] = 0;
                ViewState["mode"] = 0;
                FillState();
                FillDefaultWage();
                FillOtherIncomeWage();
                
                //FillCountry();
                if (Request.QueryString["uid"] == null)
                {
                    Page.Title = "Add Employee || MOM";
                    ViewState["edit"] = "0";
                    ClearControl();

                }
                if (Request.QueryString["id"] != null)  //Edit COA
                {
                    
                    
                    //_objvendor.ConnConfig = _connectionString;
                    //SetDataForEdit();

                    if (Request.QueryString["t"] != null)
                    {
                        Page.Title = "Add Employee || MOM";
                        ViewState["mode"] = 0;
                        lblHeader.Text = "Add Employee";
                        ViewState["edit"] = "0";
                        GetEditData(Request.QueryString["id"].ToString());
                        hdnEmpID.Value = "";
                    }
                    else
                    {
                        Page.Title = "Edit Employee || MOM";
                        ViewState["mode"] = 1;
                        lblHeader.Text = "Edit Employee";
                        ViewState["edit"] = "1";
                        GetEditData(Request.QueryString["id"].ToString());
                    }
                    //lblAcctType.Text = ddlType.SelectedItem.Text;
                    //lblAcctNum.Text = txtAcctNum.Text;
                    //lblAcctName.Text = txtAcName.Text;




                    //foreach(DataListItem item in gvTrans.Items)
                    //{
                    //    if(item.ItemType == ListItemType.Footer)
                    //    {
                    //        Label lblFooterAmount = (Label)item.FindControl("lblFooterAmount");
                    //        if(ds.Tables[0].Rows.Count > 0)
                    //        {
                    //            lblFooterAmount.Text = string.Format("{0:c}", ds.Tables[0].Compute("sum(Amount)", string.Empty));
                    //        }
                    //    }
                    //}

                }
                Permission();

                
            }



           

            pnlNext.Visible = false;
            if (Request.QueryString["id"] != null)
            {
                pnlNext.Visible = true;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
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
                objProp_User.PageName = "AddEmployee.aspx";
                //DataSet dspage = _objBLUser.getScreensByUser(_objPropUser);
                //if (dspage.Tables[0].Rows.Count > 0)
                //{
                //    if (Convert.ToBoolean(dspage.Tables[0].Rows[0]["access"].ToString()) == false)
                //    {
                //        Response.Redirect("home.aspx");
                //    }
                //}
                //else
                //{
                //    Response.Redirect("home.aspx");
                //}
                if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
                {
                    DataTable dtUserPermission = new DataTable();
                    dtUserPermission = GetUserById();
                    /// AccountPayablemodulePermission ///////////////////------->

                    string AccountPayablemodulePermission = dtUserPermission.Rows[0]["AccountPayablemodulePermission"] == DBNull.Value ? "Y" : dtUserPermission.Rows[0]["AccountPayablemodulePermission"].ToString();

                    if (AccountPayablemodulePermission == "N")
                    {
                        Response.Redirect("Home.aspx?permission=no"); return;
                    }

                    /// Vendor  ///////////////////------->

                    string VendorPermission = dtUserPermission.Rows[0]["Vendor"] == DBNull.Value ? "YYYYYY" : dtUserPermission.Rows[0]["Vendor"].ToString();
                    string ADD = VendorPermission.Length < 1 ? "Y" : VendorPermission.Substring(0, 1);
                    string Edit = VendorPermission.Length < 2 ? "Y" : VendorPermission.Substring(1, 1);
                    string Delete = VendorPermission.Length < 3 ? "Y" : VendorPermission.Substring(2, 1);
                    string View = VendorPermission.Length < 4 ? "Y" : VendorPermission.Substring(3, 1);

                    if (Request.QueryString["id"] != null)
                    {
                        //aImport.Visible = false;
                    }
                    if (View == "N")
                    {
                        Response.Redirect("Home.aspx?permission=no"); return;
                    }
                    else if (Request.QueryString["id"] == null)
                    {
                        if (ADD == "N")
                        {
                            Response.Redirect("Home.aspx?permission=no"); return;
                        }
                    }
                    else if (Edit == "N")
                    {
                        if (View == "Y")
                        {
                            btnSubmit.Visible = false;
                            //btnSubmitJob.Visible = false;
                        }
                        else
                        {
                            Response.Redirect("Home.aspx?permission=no"); return;
                        }
                    }
                }
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
    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["EmployeeList"];
            string url = "AddEmployee.aspx?id=" + dt.Rows[0]["ID"];
            Response.Redirect(url);
        }
        catch (Exception ex)
        {

        }
    }
    private void GetEditData (string id)
    {
        try
        {

            DataSet ds = new DataSet();
            _objEmp.ConnConfig = Session["config"].ToString();
            _objEmp.ID = Convert.ToInt32(id);
            ds = objBL_Wage.GetEmployeeListByID(_objEmp);
            DataRow _dr = ds.Tables[0].Rows[0];
            SetEmpData(_dr);

            //gvWagePayRate.DataSource = ds.Tables[1];
            //gvWagePayRate.DataBind();

            //RadGridWageDeduction.DataSource = ds.Tables[2];
            //RadGridWageDeduction.DataBind();

            RadGrid_WageCategoryOtherIncome.DataSource = ds.Tables[3];
            RadGrid_WageCategoryOtherIncome.DataBind();

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "Materialize.updateTextFields();", true);

        }
        catch (Exception ex)
        {
            string type = "error";
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddDepttype", "noty({text: '" + str + "', dismissQueue: true,  type : '" + type + "', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    private void FillState()
    {
        //try
        //{
        //    if (ddlState.SelectedIndex != 0)
        //    {
        //        DataSet _dsState = new DataSet();
        //        State _objState = new State();

        //        _objState.ConnConfig = Session["config"].ToString();

        //        //List<StateViewModel> _StateViewModel = new List<StateViewModel>();

        //        //string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();

        //        //if (IsAPIIntegrationEnable == "YES")
        //        //{
        //        //    string APINAME = "VendorAPI/AddVendor_GetStates";

        //        //    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _objState);

        //        //    _StateViewModel = (new JavaScriptSerializer()).Deserialize<List<StateViewModel>>(_APIResponse.ResponseData);
        //        //    _dsState = CommonMethods.ToDataSet<StateViewModel>(_StateViewModel);
        //        //}
        //        //else
        //        //{
        //        //    _dsState = _objBLBank.GetStates(_objState); 
        //        //}
        //        _dsState = _objBLBank.GetStates(_objState);
        //        ddlState.Items.Add(new ListItem("Select State", ""));
        //        ddlState.AppendDataBoundItems = true;
        //        ddlState.DataSource = _dsState;
        //        ddlState.DataValueField = "Name";
        //        ddlState.DataTextField = "fDesc";
        //        ddlState.DataBind();


               
        //        ddlFilingState.Items.Add(new RadComboBoxItem("Select State", ""));
        //        ddlFilingState.AppendDataBoundItems = true;
        //        ddlFilingState.DataSource = _dsState;
        //        ddlFilingState.DataValueField = "Name";
        //        ddlFilingState.DataTextField = "fDesc";
        //        ddlFilingState.DataBind();
        //    }
        //    else
        //    {

        //    }
        //}
        //catch (Exception ex)
        //{
        //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        //    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        //}
    }
    private void FillDefaultWage()
    {
        try
        {
            DataSet ds = new DataSet();
            _objWage.ConnConfig = Session["config"].ToString();

            ds = new BL_Wage().getWage(_objWage);

            ddlDefaultWage.DataSource = ds.Tables[0];
            ddlDefaultWage.DataTextField = "fdesc";
            ddlDefaultWage.DataValueField = "id";
            ddlDefaultWage.DataBind();

            //ddlDefaultWage.Items.Insert(0, new ListItem(":: Select ::", ""));
        }
        catch (Exception ex)
        {
            throw ex;            
        }
    }
    public DataTable GetOtherIncomeWageTable()
    {
        string PayrollTaxAcctName = "";
        int PayrollTaxAcct = 0;
        DataSet ds = new DataSet();
        _objWage.ConnConfig = Session["config"].ToString();

        ds = new BL_Wage().getPayrollTaxAccount(_objWage);
        if (ds.Tables[0].Rows.Count > 0)
        {
            PayrollTaxAcctName = ds.Tables[0].Rows[0]["fDesc"].ToString();
            PayrollTaxAcct = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"].ToString());
        }

        DataTable dt = new DataTable();
        dt.Columns.Add("Cat", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Rate", typeof(double));
        dt.Columns.Add("ExpAcctName", typeof(string));
        dt.Columns.Add("GL", typeof(int));
        dt.Columns.Add("FIT", typeof(int));
        dt.Columns.Add("FICA", typeof(int));
        dt.Columns.Add("MEDI", typeof(int));
        dt.Columns.Add("FUTA", typeof(int));
        dt.Columns.Add("SIT", typeof(int));
        dt.Columns.Add("Vac", typeof(int));
        dt.Columns.Add("Wc", typeof(int));
        dt.Columns.Add("Uni", typeof(int));
        dt.Columns.Add("Sick", typeof(int));

        for (int i = 0; i < 7; i++)
        {
            DataRow dr = dt.NewRow();
            dr["Cat"] = i;
            if (i == 0)
            {
                dr["fDesc"] = "Bonus";
                dr["Rate"] = 0.00;
                dr["ExpAcctName"] = PayrollTaxAcctName;
                dr["GL"] = PayrollTaxAcct;
                dr["FIT"] = 1;
                dr["FICA"] = 1;
                dr["MEDI"] = 1;
                dr["FUTA"] = 1;
                dr["SIT"] = 1;
                dr["Vac"] = 0;
                dr["Wc"] = 1;
                dr["Uni"] = 1;
                dr["Sick"] = 1;
            }
            else if (i == 1)
            {
                dr["fDesc"] = "Holiday";
                dr["Rate"] = 0.00;
                dr["ExpAcctName"] = PayrollTaxAcctName;
                dr["GL"] = PayrollTaxAcct;
                dr["FIT"] = 1;
                dr["FICA"] = 1;
                dr["MEDI"] = 1;
                dr["FUTA"] = 1;
                dr["SIT"] = 1;
                dr["Vac"] = 0;
                dr["Wc"] = 1;
                dr["Uni"] = 1;
                dr["Sick"] = 1;
            }
            else if (i == 2)
            {
                dr["fDesc"] = "Vacation";
                dr["Rate"] = 0.00;
                dr["ExpAcctName"] = PayrollTaxAcctName;
                dr["GL"] = PayrollTaxAcct;
                dr["FIT"] = 1;
                dr["FICA"] = 1;
                dr["MEDI"] = 1;
                dr["FUTA"] = 1;
                dr["SIT"] = 1;
                dr["Vac"] = 0;
                dr["Wc"] = 1;
                dr["Uni"] = 0;
                dr["Sick"] = 1;
            }
            else if (i == 3)
            {
                dr["fDesc"] = "Zone";
                dr["Rate"] = 0.00;
                dr["ExpAcctName"] = PayrollTaxAcctName;
                dr["GL"] = PayrollTaxAcct;
                dr["FIT"] = 1;
                dr["FICA"] = 1;
                dr["MEDI"] = 1;
                dr["FUTA"] = 1;
                dr["SIT"] = 1;
                dr["Vac"] = 0;
                dr["Wc"] = 1;
                dr["Uni"] = 1;
                dr["Sick"] = 1;
            }
            else if (i == 4)
            {
                dr["fDesc"] = "Reimbursement";
                dr["Rate"] = 0.00;
                dr["ExpAcctName"] = PayrollTaxAcctName;
                dr["GL"] = PayrollTaxAcct;
                dr["FIT"] = 0;
                dr["FICA"] = 0;
                dr["MEDI"] = 0;
                dr["FUTA"] = 0;
                dr["SIT"] = 0;
                dr["Vac"] = 0;
                dr["Wc"] = 0;
                dr["Uni"] = 0;
                dr["Sick"] = 0;
            }
            else if (i == 5)
            {
                dr["fDesc"] = "Mileage";
                dr["Rate"] = 0.00;
                dr["ExpAcctName"] = PayrollTaxAcctName;
                dr["GL"] = PayrollTaxAcct;
                dr["FIT"] = 0;
                dr["FICA"] = 0;
                dr["MEDI"] = 0;
                dr["FUTA"] = 0;
                dr["SIT"] = 0;
                dr["Vac"] = 0;
                dr["Wc"] = 0;
                dr["Uni"] = 0;
                dr["Sick"] = 0;
            }
            else if (i == 6)
            {
                dr["fDesc"] = "Sick Leave";
                dr["Rate"] = 0.00;
                dr["ExpAcctName"] = PayrollTaxAcctName;
                dr["GL"] = PayrollTaxAcct;
                dr["FIT"] = 0;
                dr["FICA"] = 0;
                dr["MEDI"] = 0;
                dr["FUTA"] = 0;
                dr["SIT"] = 0;
                dr["Vac"] = 0;
                dr["Wc"] = 0;
                dr["Uni"] = 0;
                dr["Sick"] = 0;
            }

           

            dt.Rows.Add(dr);
            dt.AcceptChanges  ();
        }

        return dt;
    }
    public DataTable GetWageTable()
    {
        //DataSet ds = new DataSet();
        //_objWage.ConnConfig = Session["config"].ToString();
        //_objWage.ID = Convert.ToInt32(0);
        //ds = objBL_Wage.GetWageByID(_objWage);
       
        //return ds.Tables[0];


        DataTable dt = new DataTable();
        dt.Columns.Add("id", typeof(int));
        dt.Columns.Add("GL", typeof(int));
        dt.Columns.Add("GLName", typeof(string));
        dt.Columns.Add("Reg", typeof(double));
        dt.Columns.Add("OT", typeof(double));
        dt.Columns.Add("DT", typeof(double));
        dt.Columns.Add("TT", typeof(double));
        dt.Columns.Add("FIT", typeof(int));
        dt.Columns.Add("FICA", typeof(int));
        dt.Columns.Add("MEDI", typeof(int));
        dt.Columns.Add("FUTA", typeof(int));
        dt.Columns.Add("SIT", typeof(int));
        dt.Columns.Add("Vac", typeof(int));
        dt.Columns.Add("Wc", typeof(int));
        dt.Columns.Add("Uni", typeof(int));
        dt.Columns.Add("InUse", typeof(int));
        dt.Columns.Add("Sick", typeof(int));
        dt.Columns.Add("Status", typeof(int));
                
        dt.Columns.Add("YTD", typeof(double));
        dt.Columns.Add("YTDH", typeof(double));
        dt.Columns.Add("OYTD", typeof(double));
        dt.Columns.Add("OYTDH", typeof(double));
        dt.Columns.Add("DYTD", typeof(double));
        dt.Columns.Add("DYTDH", typeof(double));
        dt.Columns.Add("TYTD", typeof(double));
        dt.Columns.Add("TYTDH", typeof(double));
        dt.Columns.Add("NT", typeof(double));
        dt.Columns.Add("NYTD", typeof(double));
        dt.Columns.Add("NYTDH", typeof(double));
        dt.Columns.Add("VacR", typeof(string));
        dt.Columns.Add("CReg", typeof(double));
        dt.Columns.Add("COT", typeof(double));
        dt.Columns.Add("CDT", typeof(double));
        dt.Columns.Add("CNT", typeof(double));
        dt.Columns.Add("CTT", typeof(double));
        dt.Columns.Add("fdesc", typeof(string));

        return dt;
    }
    //public DataTable GetWageTableItem()
    //{
    //    //DataSet ds = new DataSet();
    //    //_objWage.ConnConfig = Session["config"].ToString();
    //    //_objWage.ID = Convert.ToInt32(0);
    //    //ds = objBL_Wage.GetWageByID(_objWage);

    //    //return ds.Tables[0];


    //    DataTable dt = new DataTable();
    //    dt.Columns.Add("Wage", typeof(int));
    //    dt.Columns.Add("GL", typeof(int));
    //    dt.Columns.Add("Reg", typeof(double));
    //    dt.Columns.Add("OT", typeof(double));
    //    dt.Columns.Add("DT", typeof(double));
    //    dt.Columns.Add("TT", typeof(double));
    //    dt.Columns.Add("FIT", typeof(int));
    //    dt.Columns.Add("FICA", typeof(int));
    //    dt.Columns.Add("MEDI", typeof(int));
    //    dt.Columns.Add("FUTA", typeof(int));
    //    dt.Columns.Add("SIT", typeof(int));
    //    dt.Columns.Add("Vac", typeof(int));
    //    dt.Columns.Add("Wc", typeof(int));
    //    dt.Columns.Add("Uni", typeof(int));
    //    dt.Columns.Add("InUse", typeof(int));
       

    //    dt.Columns.Add("YTD", typeof(double));
    //    dt.Columns.Add("YTDH", typeof(double));
    //    dt.Columns.Add("OYTD", typeof(double));
    //    dt.Columns.Add("OYTDH", typeof(double));
    //    dt.Columns.Add("DYTD", typeof(double));
    //    dt.Columns.Add("DYTDH", typeof(double));
    //    dt.Columns.Add("TYTD", typeof(double));
    //    dt.Columns.Add("TYTDH", typeof(double));
    //    dt.Columns.Add("NT", typeof(double));
    //    dt.Columns.Add("NYTD", typeof(double));
    //    dt.Columns.Add("NYTDH", typeof(double));
    //    dt.Columns.Add("VacR", typeof(string));
    //    dt.Columns.Add("CReg", typeof(double));
    //    dt.Columns.Add("COT", typeof(double));
    //    dt.Columns.Add("CDT", typeof(double));
    //    dt.Columns.Add("CNT", typeof(double));
    //    dt.Columns.Add("CTT", typeof(double));
    //    dt.Columns.Add("Status", typeof(int));
    //    dt.Columns.Add("Sick", typeof(int));

    //    foreach (GridDataItem gr in RadGrid_WageCategory.Items)
    //    {
    //        DataRow dr = dt.NewRow();

    //        Label lblID = (Label)gr.FindControl("lblId");
    //        HiddenField hdnWageGL = (HiddenField)gr.FindControl("hdnWageGL");
    //        HiddenField hdnWageReg = (HiddenField)gr.FindControl("hdnWageReg");
    //        HiddenField hdnWageOT = (HiddenField)gr.FindControl("hdnWageOT");
    //        HiddenField hdnWageDT = (HiddenField)gr.FindControl("hdnWageDT");
    //        HiddenField hdnWageTT = (HiddenField)gr.FindControl("hdnWageTT");
    //        HiddenField hdnWageFIT = (HiddenField)gr.FindControl("hdnWageFIT");
    //        HiddenField hdnWageFICA = (HiddenField)gr.FindControl("hdnWageFICA");
    //        HiddenField hdnWageMEDI = (HiddenField)gr.FindControl("hdnWageMEDI");
    //        HiddenField hdnWageFUTA = (HiddenField)gr.FindControl("hdnWageFUTA");
    //        HiddenField hdnWageSIT = (HiddenField)gr.FindControl("hdnWageSIT");
    //        HiddenField hdnWageVac = (HiddenField)gr.FindControl("hdnWageVac");
    //        HiddenField hdnWageWC = (HiddenField)gr.FindControl("hdnWageWC");
    //        HiddenField hdnWageUni = (HiddenField)gr.FindControl("hdnWageUni");
    //        HiddenField hdnWageInUse = (HiddenField)gr.FindControl("hdnWageInUse");
    //        HiddenField hdnWageYTD = (HiddenField)gr.FindControl("hdnWageYTD");
    //        HiddenField hdnWageYTDH = (HiddenField)gr.FindControl("hdnWageYTDH");
    //        HiddenField hdnWageOYTD = (HiddenField)gr.FindControl("hdnWageOYTD");
    //        HiddenField hdnWageOYTDH = (HiddenField)gr.FindControl("hdnWageOYTDH");
    //        HiddenField hdnWageDYTD = (HiddenField)gr.FindControl("hdnWageDYTD");
    //        HiddenField hdnWageDYTDH = (HiddenField)gr.FindControl("hdnWageDYTDH");
    //        HiddenField hdnWageTYTD = (HiddenField)gr.FindControl("hdnWageTYTD");
    //        HiddenField hdnWageTYTDH = (HiddenField)gr.FindControl("hdnWageTYTDH");
    //        HiddenField hdnWageNT = (HiddenField)gr.FindControl("hdnWageNT");
    //        HiddenField hdnWageNYTD = (HiddenField)gr.FindControl("hdnWageNYTD");
    //        HiddenField hdnWageNYTDH = (HiddenField)gr.FindControl("hdnWageNYTDH");
    //        HiddenField hdnWageVacR = (HiddenField)gr.FindControl("hdnWageVacR");
    //        HiddenField hdnWageCReg = (HiddenField)gr.FindControl("hdnWageCReg");
    //        HiddenField hdnWageCOT = (HiddenField)gr.FindControl("hdnWageCOT");
    //        HiddenField hdnWageCDT = (HiddenField)gr.FindControl("hdnWageCDT");
    //        HiddenField hdnWageCNT = (HiddenField)gr.FindControl("hdnWageCNT");
    //        HiddenField hdnWageCTT = (HiddenField)gr.FindControl("hdnWageCTT");
    //        HiddenField hdnWageStatus = (HiddenField)gr.FindControl("hdnWageStatus");
    //        HiddenField hdnWageSick = (HiddenField)gr.FindControl("hdnWageSick");

    //        dr["Wage"] = Convert.ToInt32(lblID.Text);
    //        dr["GL"] = Convert.ToInt32(hdnWageGL.Value);
    //        dr["Reg"] = Convert.ToDouble(hdnWageReg.Value);
    //        dr["OT"] = Convert.ToString(hdnWageOT.Value);
    //        dr["DT"] = Convert.ToInt32(hdnWageDT.Value);
    //        dr["TT"] = Convert.ToInt32(hdnWageTT.Value);
    //        dr["FIT"] = Convert.ToInt32(hdnWageFIT.Value);
    //        dr["FICA"] = Convert.ToInt32(hdnWageFICA.Value);
    //        dr["MEDI"] = Convert.ToInt32(hdnWageMEDI.Value);
    //        dr["FUTA"] = Convert.ToInt32(hdnWageFUTA.Value);
    //        dr["SIT"] = Convert.ToInt32(hdnWageSIT.Value);
    //        dr["Vac"] = Convert.ToInt32(hdnWageVac.Value);
    //        dr["Wc"] = Convert.ToInt32(hdnWageWC.Value);
    //        dr["Uni"] = Convert.ToInt32(hdnWageUni.Value);
    //        dr["InUse"] = Convert.ToInt32(hdnWageInUse.Value);
    //        dr["Sick"] = Convert.ToInt32(hdnWageSick.Value);
    //        dr["Status"] = Convert.ToInt32(hdnWageStatus.Value);

    //        dr["YTD"] = 0.00;
    //        dr["YTDH"] = 0.00;
    //        dr["OYTD"] = 0.00;
    //        dr["OYTDH"] = 0.00;
    //        dr["DYTD"] = 0.00;
    //        dr["DYTDH"] = 0.00;
    //        dr["TYTD"] = 0.00;
    //        dr["TYTDH"] = 0.00;
    //        dr["NT"] = Convert.ToDouble(hdnWageNT.Value);
    //        dr["NYTD"] = 0.00;
    //        dr["NYTDH"] = 0.00;
    //        dr["VacR"] = "";
    //        dr["CReg"] = Convert.ToDouble(hdnWageCReg.Value);
    //        dr["COT"] = Convert.ToDouble(hdnWageCOT.Value);
    //        dr["CDT"] = Convert.ToDouble(hdnWageCDT.Value);
    //        dr["CNT"] = Convert.ToDouble(hdnWageCNT.Value);
    //        dr["CTT"] = Convert.ToDouble(hdnWageCTT.Value);
            

    //        dt.Rows.Add(dr);
    //        dt.AcceptChanges();
    //    }

    //    return dt;
    //}
    public DataTable GetOtherIncomeWageTableItem()
    {
        
        DataTable dt = new DataTable();
        dt.Columns.Add("Cat", typeof(int));
        dt.Columns.Add("Rate", typeof(double));
        dt.Columns.Add("GL", typeof(int));
        dt.Columns.Add("FIT", typeof(int));
        dt.Columns.Add("FICA", typeof(int));
        dt.Columns.Add("MEDI", typeof(int));
        dt.Columns.Add("FUTA", typeof(int));
        dt.Columns.Add("SIT", typeof(int));
        dt.Columns.Add("Vac", typeof(int));
        dt.Columns.Add("Wc", typeof(int));
        dt.Columns.Add("Uni", typeof(int));
        dt.Columns.Add("Sick", typeof(int));

        
        foreach (GridDataItem gr in RadGrid_WageCategoryOtherIncome.Items)
        {
            DataRow dr = dt.NewRow();

            Label lblID1 = (Label)gr.FindControl("lblID1");
            HiddenField hdnotherGL = (HiddenField)gr.FindControl("hdnotherGL");
            HiddenField hdnotherRate = (HiddenField)gr.FindControl("hdnotherRate");
            HiddenField hdnotherFIT = (HiddenField)gr.FindControl("hdnotherFIT");
            HiddenField hdnotherFICA = (HiddenField)gr.FindControl("hdnotherFICA");
            HiddenField hdnotherMEDI = (HiddenField)gr.FindControl("hdnotherMEDI");
            HiddenField hdnotherFUTA = (HiddenField)gr.FindControl("hdnotherFUTA");
            HiddenField hdnotherSIT = (HiddenField)gr.FindControl("hdnotherSIT");
            HiddenField hdnotherVac = (HiddenField)gr.FindControl("hdnotherVac");
            HiddenField hdnotherWc = (HiddenField)gr.FindControl("hdnotherWc");
            HiddenField hdnotherUni = (HiddenField)gr.FindControl("hdnotherUni");
            HiddenField hdnotherSick = (HiddenField)gr.FindControl("hdnotherSick");


            dr["Cat"] = Convert.ToInt32(lblID1.Text);
            dr["Rate"] = Convert.ToDouble(hdnotherRate.Value);
            dr["GL"] = Convert.ToInt32(hdnotherGL.Value);
            dr["FIT"] = Convert.ToInt32(hdnotherFIT.Value);
            dr["FICA"] = Convert.ToInt32(hdnotherFICA.Value);
            dr["MEDI"] = Convert.ToInt32(hdnotherMEDI.Value);
            dr["FUTA"] = Convert.ToInt32(hdnotherFUTA.Value);
            dr["SIT"] = Convert.ToInt32(hdnotherSIT.Value);
            dr["Vac"] = Convert.ToInt32(hdnotherVac.Value);
            dr["Wc"] = Convert.ToInt32(hdnotherWc.Value);
            dr["Uni"] = Convert.ToInt32(hdnotherUni.Value);
            dr["Sick"] = Convert.ToInt32(hdnotherSick.Value);


            dt.Rows.Add(dr);
            dt.AcceptChanges();
        }

        return dt;
    }
    //public DataTable GetWageDeductionTableItem()
    //{

    //    DataTable dt = new DataTable();
    //    dt.Columns.Add("Ded", typeof(int));
    //    dt.Columns.Add("BasedOn", typeof(int));
    //    dt.Columns.Add("AccruedOn", typeof(int));
    //    dt.Columns.Add("ByW", typeof(int));
    //    dt.Columns.Add("EmpRate", typeof(double));
    //    dt.Columns.Add("EmpTop", typeof(double));
    //    dt.Columns.Add("EmpGL", typeof(double));
    //    dt.Columns.Add("CompRate", typeof(double));
    //    dt.Columns.Add("CompTop", typeof(double));
    //    dt.Columns.Add("CompGL", typeof(int));
    //    dt.Columns.Add("CompGLE", typeof(int));
    //    dt.Columns.Add("InUse", typeof(int));
    //    dt.Columns.Add("YTD", typeof(double));
    //    dt.Columns.Add("YTDC", typeof(double));
        

    //    foreach (GridDataItem gr in RadGrid_WageDeduction.Items)
    //    {
    //        DataRow dr = dt.NewRow();

    //        Label lblIDdedu1 = (Label)gr.FindControl("lblIDdedu1");
    //        HiddenField hdndeduBasedOn = (HiddenField)gr.FindControl("hdndeduBasedOn");
    //        HiddenField hdndeduAccruedOn = (HiddenField)gr.FindControl("hdndeduAccruedOn");
    //        HiddenField hdndeduByW = (HiddenField)gr.FindControl("hdndeduByW");
    //        HiddenField hdndeduEmpRate = (HiddenField)gr.FindControl("hdndeduEmpRate");
    //        HiddenField hdndeduEmpTop = (HiddenField)gr.FindControl("hdndeduEmpTop");
    //        HiddenField hdndeduEmpGL = (HiddenField)gr.FindControl("hdndeduEmpGL");
    //        HiddenField hdndeduCompRate = (HiddenField)gr.FindControl("hdndeduCompRate");
    //        HiddenField hdndeduCompTop = (HiddenField)gr.FindControl("hdndeduCompTop");
    //        HiddenField hdndeduCompGL = (HiddenField)gr.FindControl("hdndeduCompGL");
    //        HiddenField hdndeduCompGLE = (HiddenField)gr.FindControl("hdndeduCompGLE");
    //        HiddenField hdndeduInUse = (HiddenField)gr.FindControl("hdndeduInUse");
    //        HiddenField hdndeduYTD = (HiddenField)gr.FindControl("hdndeduYTD");
    //        HiddenField hdndeduYTDC = (HiddenField)gr.FindControl("hdndeduYTDC");

    //        dr["Ded"] = Convert.ToInt32(lblIDdedu1.Text);
    //        dr["BasedOn"] = Convert.ToInt32(hdndeduBasedOn.Value);
    //        dr["AccruedOn"] = Convert.ToInt32(hdndeduAccruedOn.Value);
    //        dr["ByW"] = Convert.ToInt32(hdndeduByW.Value);
    //        dr["EmpRate"] = Convert.ToDouble(hdndeduEmpRate.Value);
    //        dr["EmpTop"] = Convert.ToDouble(hdndeduEmpTop.Value);
    //        dr["EmpGL"] = Convert.ToDouble(hdndeduEmpGL.Value);
    //        dr["CompRate"] = Convert.ToDouble(hdndeduCompRate.Value);
    //        dr["CompTop"] = Convert.ToDouble(hdndeduCompTop.Value);
    //        dr["CompGL"] = Convert.ToInt32(hdndeduCompGL.Value);
    //        dr["CompGLE"] = Convert.ToInt32(hdndeduCompGLE.Value);
    //        dr["InUse"] = 0;
    //        dr["YTD"] = 0.00;
    //        dr["YTDC"] = 0.00;


    //        dt.Rows.Add(dr);
    //        dt.AcceptChanges();
    //    }

    //    return dt;



    //}
    public DataTable GetWageDeductionTable()
    {
        //DataSet ds = new DataSet();
        //_objPRDed.ConnConfig = Session["config"].ToString();
        //_objPRDed.ID = Convert.ToInt32(0);
        //ds = new BL_Wage().GetWageDeductionByID(_objPRDed);
        //return ds.Tables[0];


        DataTable dt = new DataTable();
        dt.Columns.Add("id", typeof(int));
        dt.Columns.Add("BasedOn", typeof(int));
        dt.Columns.Add("AccruedOn", typeof(int));
        dt.Columns.Add("ByW", typeof(int));
        dt.Columns.Add("EmpRate", typeof(double));
        dt.Columns.Add("EmpTop", typeof(double));
        dt.Columns.Add("EmpGL", typeof(int));
        dt.Columns.Add("EmpGLName", typeof(string));
        dt.Columns.Add("CompRate", typeof(double));
        dt.Columns.Add("CompTop", typeof(double));
        dt.Columns.Add("CompGL", typeof(int));
        dt.Columns.Add("CompGLName", typeof(string));
        dt.Columns.Add("CompGLE", typeof(int));
        dt.Columns.Add("CompGLEName", typeof(string));
        dt.Columns.Add("InUse", typeof(int));
        dt.Columns.Add("YTD", typeof(double));
        dt.Columns.Add("YTDC", typeof(double));
        dt.Columns.Add("fdesc", typeof(string));

        return dt;



    }
    
    private void FillOtherIncomeWage()
    {
        try
        {

            DataTable dt = GetOtherIncomeWageTable();

            RadGrid_WageCategoryOtherIncome.DataSource = dt;
            RadGrid_WageCategoryOtherIncome.DataBind();

            //ddlDefaultWage.Items.Insert(0, new ListItem(":: Select ::", ""));
        }
        catch (Exception ex)
        {
            throw ex;
            //FillOtherIncomeWage
        }
    }
    private void FillWageCategory()
    {
        try
        {

           // DataTable dt = GetWageTable();

           // RadGrid_WageCategory.DataSource = dt;


            //ddlDefaultWage.Items.Insert(0, new ListItem(":: Select ::", ""));
        }
        catch (Exception ex)
        {
            throw ex;
            //FillOtherIncomeWage
        }
    }
    //private void FillWageDeduction()
    //{
    //    try
    //    {

    //        DataTable dt = GetWageDeductionTable();

    //        RadGrid_WageDeduction.DataSource = dt;


    //        //ddlDefaultWage.Items.Insert(0, new ListItem(":: Select ::", ""));
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //        //FillOtherIncomeWage
    //    }
    //}
    protected void RadGrid_WageCategoryOtherIncome_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        FillOtherIncomeWage();
    }
    protected void RadGrid_WageCategoryOtherIncome_PreRender(object sender, EventArgs e)
    {
        RowSelectWageCategoryOtherIncome();
    }
    protected void RadGrid_WageCategoryOtherIncome_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = e.Item as GridDataItem;
            CheckBox checkColumn = item.FindControl("checkColumnWageCategoryOtherIncome") as CheckBox;
            checkColumn.Attributes.Add("onclick", "uncheckWageCategoryOtherIncome(this);");
        }
    }
    protected void RadGrid_WageCategory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        FillWageCategory();
    }
    protected void RadGrid_WageCategory_PreRender(object sender, EventArgs e)
    {
        RowSelectWageCategory();
    }
    protected void RadGrid_WageCategory_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = e.Item as GridDataItem;
            CheckBox checkColumn = item.FindControl("checkColumnWageCategory") as CheckBox;
            checkColumn.Attributes.Add("onclick", "uncheckWageCategory(this);");
        }
    }
    protected void RadGrid_WageDeduction_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        //FillWageDeduction();
    }
    protected void RadGrid_WageDeduction_PreRender(object sender, EventArgs e)
    {
        RowSelectWageDeduction();
    }
    protected void RadGrid_WageDeduction_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = e.Item as GridDataItem;
            CheckBox checkColumn = item.FindControl("checkColumnWageDeduction") as CheckBox;
            checkColumn.Attributes.Add("onclick", "uncheckWageDeduction(this);");
        }
    }
    private void RowSelectWageCategoryOtherIncome()
    {
        
    }
    private void RowSelectWageCategory()
    {
       
    }
    private void RowSelectWageDeduction()
    {
        
    }

    private void SetEmpData(DataRow _dr)
    {
        //if (Convert.ToInt16(_dr["Sales"]).Equals(1))
        //{
        //    chkSales.Checked = true;
        //}
        //else
        //    chkSales.Checked = false;

        //txtFirst.Text = _dr["fFirst"].ToString();
        //txtLast.Text = _dr["Last"].ToString();
        //txtMiddle.Text = _dr["Middle"].ToString();
        //txtSSN.Text = _dr["SSN"].ToString();
        //txtTitle.Text = _dr["Title"].ToString();

        //ddlField.SelectedValue = _dr["Field"].ToString();
        //ddlStatus.SelectedValue = _dr["Status"].ToString();
        //ddlaalownancesFederal.SelectedValue = _dr["FStatus"].ToString();
        //ddlaalownancesState.SelectedValue = _dr["SStatus"].ToString();
        //ddlPayPeriod.SelectedValue = _dr["PayPeriod"].ToString();
        ////ddlBasedOn.SelectedValue = _dr["VBase"].ToString();
        //ddlPayMethod.SelectedValue = _dr["PMethod"].ToString();
        //ddlPayrollHours.SelectedValue = _dr["PFixed"].ToString();
        //ddlAllowancesAdditonalLocal.SelectedValue = _dr["LName"].ToString();
        //ddlaalownancesLocal.SelectedValue = _dr["LStatus"].ToString();
        //ddlState.SelectedValue = _dr["State"].ToString();

        //txtTextMsgAddress.Text = _dr["Pager"].ToString();
        //txtHired.Text = _dr["DHired"].ToString();
        //txtTerminated.Text = _dr["DFired"].ToString();
        //txtBirthday.Text = _dr["DBirth"].ToString();
        //txtReview.Text = _dr["DReview"].ToString();
        //txtTerminated.Text = _dr["DLast"].ToString();

        //txtAllowncesFedral.Text = _dr["FAllow"].ToString();
        //txtAllowancesAdditonalFedral.Text = _dr["FAdd"].ToString();
        //txtAllowncesState.Text = _dr["SAllow"].ToString();
        //txtAllowancesAdditonalState.Text = _dr["SAdd"].ToString();
        //txtCallSign.Text = _dr["CallSign"].ToString();
        //txtVacationRate.Text = _dr["VRate"].ToString();
        //txtAvailableVacation.Text = _dr["VLast"].ToString();
        //txtSickUnits.Text = _dr["Sick"].ToString();
        //txtAllownceslocal.Text = _dr["LAllow"].ToString();
        //hdntxtPRTaxGL.Value = _dr["PRTaxE"].ToString();
        //txtPaidMisc.Text = _dr["NPaid"].ToString();
        //txtBankRoute1.Text = _dr["ACHRoute"].ToString();
        //txtBankAcct1.Text = _dr["ACHBank"].ToString();
        //txtRehire.Text = _dr["Anniversary"].ToString();
        //txtPDASerial.Text = _dr["PDASerialNumber"].ToString();
        //txtBankRoute2.Text = _dr["ACHRoute2"].ToString();
        //txtBankAcct2.Text = _dr["ACHBank2"].ToString();

        //ddlNationalOrigin.SelectedValue = _dr["Race"].ToString();
        //ddlGender.SelectedValue = _dr["Sex"].ToString();
        //ddlDirectDeposit.SelectedValue = _dr["ACH"].ToString();
        //ddlAccountType1.SelectedValue = _dr["ACHType"].ToString();
        //ddlDefaultWage.SelectedValue = _dr["WageCat"].ToString();
        //ddlSplitType.SelectedValue = _dr["DDType"].ToString();
        //ddlAccountType2.SelectedValue = _dr["ACHType2"].ToString();

        //txtBillRate.Text = _dr["BillRate"].ToString();
        //txtSales.Text = _dr["BMSales"].ToString();
        //txtInvoiceAverage.Text = _dr["BMInvAve"].ToString();
        //txtClosingPercentage.Text = _dr["BMClosing"].ToString();
        //txtBillableEfficiency.Text = _dr["BMBillEff"].ToString();
        //txtProdcutionEfficiency.Text = _dr["BMProdEff"].ToString();
        //txtAverageTasks.Text = _dr["BMAveTask"].ToString();
        //txtCustom6.Text = _dr["BMCustom1"].ToString();
        //txtCustom7.Text = _dr["BMCustom2"].ToString();
        //txtCustom8.Text = _dr["BMCustom3"].ToString();
        //txtCustom9.Text = _dr["BMCustom4"].ToString();
        //txtCustom10.Text = _dr["BMCustom5"].ToString();

        //txtSickRate.Text = _dr["SickRate"].ToString();
        //txtAccuredVacation.Text = _dr["VacAccrued"].ToString();
        //txtPDASerial.Text = _dr["PDASerialNumber"].ToString();

        // txtCity.Text = _dr["City"].ToString();
        // txtZip.Text = _dr["Zip"].ToString();
        // txtPhone.Text = _dr["Phone"].ToString();
        // txtAddress.Text = _dr["Address"].ToString();
        // txtEmail.Text = _dr["Email"].ToString();
        // txtCellular.Text = _dr["Cellular"].ToString();
        // txtRemark.Text = _dr["Remarks"].ToString();

        //txtCountry.Text = _dr["Country"].ToString();
        //txtFax.Text = _dr["Fax"].ToString();
        
        //hdnEmpID.Value = _dr["ID"].ToString();

        
    }
    private void ClearControl()
    {
        //txtHired.Text = DateTime.Today.ToString();
        //ViewState["edit"] = "0";
        ////ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "Materialize.updateTextFields();", true);
    }
    protected void btnAddWages_Click(object sender, EventArgs e)
    {
        //FillWageCategory();
        //RadGrid_WageCategory.DataBind();

    }
    protected void btndeductionAdd_Click(object sender, EventArgs e)
    {
        //FillWageDeduction();
        //RadGrid_WageDeduction.DataBind();

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //try
        //{

        //    if (IsValidWageRateGrid() && Page.IsValid)
        //    {
        //        if (IsValidWageDedcutionRateGrid())
        //        {
        //            _objEmp.ConnConfig = Session["config"].ToString();
        //            _objEmp.ID = 0;
        //            _objEmp.fFirst = txtFirst.Text;
        //            _objEmp.Last = txtLast.Text;
        //            _objEmp.Middle = txtMiddle.Text;
        //            _objEmp.Name = txtFirst.Text + " " + txtLast.Text;
        //            _objEmp.Rol = 0;
        //            _objEmp.SSN = txtSSN.Text;
        //            _objEmp.Title = txtTitle.Text;
        //            if (chkSales.Checked == true)
        //            {
        //                _objEmp.Sales = 1;
        //            }
        //            else
        //            {
        //                _objEmp.Sales = 0;
        //            }
        //            _objEmp.Field = Convert.ToInt32(ddlField.SelectedValue.ToString());
        //            _objEmp.Status = Convert.ToInt32(ddlStatus.SelectedValue.ToString());
        //            _objEmp.Pager = txtTextMsgAddress.Text;
        //            _objEmp.InUse = 0;
        //            _objEmp.PayPeriod = Convert.ToInt32(ddlPayPeriod.SelectedValue.ToString());
        //            _objEmp.DHired = Convert.ToDateTime(txtHired.Text);
        //            //if (txtTerminated.Text != "")
        //            //{
        //            //    _objEmp.DFired = Convert.ToDateTime(txtTerminated.Text);
        //            //}
        //            DateTime MinDate = System.DateTime.MinValue;
        //            if (DateTime.TryParse(txtTerminated.Text.Trim(), out MinDate))
        //                _objEmp.DFired = Convert.ToDateTime(txtTerminated.Text.Trim());

        //            _objEmp.DBirth = Convert.ToDateTime(txtBirthday.Text);

        //            //if (txtReview.Text != "")
        //            //{
        //            //    _objEmp.DReview = Convert.ToDateTime(txtReview.Text);
        //            //}

        //            if (DateTime.TryParse(txtReview.Text.Trim(), out MinDate))
        //                _objEmp.DReview = Convert.ToDateTime(txtReview.Text.Trim());

        //            //if (txtTerminated.Text != "")
        //            //{
        //            //    _objEmp.DLast = Convert.ToDateTime(txtTerminated.Text);
        //            //}
        //            if (DateTime.TryParse(txtTerminated.Text.Trim(), out MinDate))
        //                _objEmp.DLast = Convert.ToDateTime(txtTerminated.Text.Trim());

        //            _objEmp.FStatus = Convert.ToInt32(ddlaalownancesFederal.SelectedValue.ToString());
        //            if (txtAllowncesFedral.Text.Trim() != "")
        //            {
        //                _objEmp.FAllow = Convert.ToInt32(txtAllowncesFedral.Text);
        //            }
        //            else
        //            {
        //                _objEmp.FAllow = 0;
        //            }
        //            if (txtAllowancesAdditonalFedral.Text.Trim() != "")
        //            {
        //                _objEmp.FAdd = Convert.ToDouble(txtAllowancesAdditonalFedral.Text);
        //            }
        //            else
        //            {
        //                _objEmp.FAdd = 0;
        //            }
        //            _objEmp.SStatus = Convert.ToInt32(ddlaalownancesState.SelectedValue.ToString());
        //            if (txtAllowncesState.Text.Trim() != "")
        //            {
        //                _objEmp.SAllow = Convert.ToInt32(txtAllowncesState.Text);
        //            }
        //            else
        //            {
        //                _objEmp.SAllow = 0;
        //            }
        //            if (txtAllowancesAdditonalState.Text.Trim() != "")
        //            {
        //                _objEmp.SAdd = Convert.ToDouble(txtAllowancesAdditonalState.Text);
        //            }
        //            else
        //            {
        //                _objEmp.SAdd = 0;
        //            }
        //            _objEmp.CallSign = txtCallSign.Text;
        //            if (txtVacationRate.Text.Trim() != "")
        //            {
        //                _objEmp.VRate = Convert.ToDouble(txtVacationRate.Text);
        //            }
        //            else
        //            {
        //                _objEmp.VRate = 0.00;
        //            }



        //            //_objEmp.VBase = Convert.ToInt32(ddlBasedOn.SelectedValue.ToString());
        //            if (txtAvailableVacation.Text.Trim() != "")
        //            {
        //                _objEmp.VLast = Convert.ToDouble(txtAvailableVacation.Text);
        //            }
        //            else
        //            {
        //                _objEmp.VLast = 0.00;
        //            }

        //            _objEmp.VThis = 0;

        //            if (txtSickUnits.Text.Trim() != "")
        //            {
        //                _objEmp.Sick = Convert.ToDouble(txtSickUnits.Text);
        //            }
        //            else
        //            {
        //                _objEmp.Sick = 0.00;
        //            }

        //            _objEmp.PMethod = Convert.ToInt32(ddlPayMethod.SelectedValue.ToString());
        //            _objEmp.PFixed = Convert.ToInt32(ddlPayrollHours.SelectedValue.ToString());
        //            _objEmp.PHour = 0;
        //            _objEmp.LName = Convert.ToInt32(ddlAllowancesAdditonalLocal.SelectedValue.ToString());
        //            _objEmp.LStatus = Convert.ToInt32(ddlaalownancesLocal.SelectedValue.ToString());

        //            if (txtAllownceslocal.Text.Trim() != "")
        //            {
        //                _objEmp.LAllow = Convert.ToInt32(txtAllownceslocal.Text);
        //            }
        //            else
        //            {
        //                _objEmp.LAllow = 0;
        //            }
        //            if (hdntxtPRTaxGL.Value.Trim() != "")
        //            {
        //                _objEmp.PRTaxE = Convert.ToInt32(hdntxtPRTaxGL.Value);
        //            }
        //            else
        //            {
        //                _objEmp.PRTaxE = 0;
        //            }


        //            _objEmp.State = ddlState.SelectedValue.ToString();
        //            _objEmp.Salary = 0;
        //            _objEmp.SalaryF = 0;
        //            _objEmp.SalaryGL = 7;
        //            _objEmp.fWork = 15;

        //            if (txtPaidMisc.Text.Trim() != "")
        //            {
        //                _objEmp.NPaid = Convert.ToInt32(txtPaidMisc.Text);
        //            }
        //            else
        //            {
        //                _objEmp.NPaid = 0;
        //            }

        //            _objEmp.Balance = 0;
        //            _objEmp.PBRate = 0;
        //            _objEmp.FITYTD = 0;
        //            _objEmp.FICAYTD = 0;
        //            _objEmp.MEDIYTD = 0;
        //            _objEmp.FUTAYTD = 0;
        //            _objEmp.SITYTD = 0;
        //            _objEmp.LocalYTD = 0;
        //            _objEmp.BonusYTD = 0;
        //            _objEmp.HolH = 0;
        //            _objEmp.HolYTD = 0;
        //            _objEmp.VacH = 0;
        //            _objEmp.VacYTD = 0;
        //            _objEmp.ZoneH = 0;
        //            _objEmp.ZoneYTD = 0;
        //            _objEmp.ReimbYTD = 0;
        //            _objEmp.MileH = 0;
        //            _objEmp.MileYTD = 0;
        //            _objEmp.Race = ddlNationalOrigin.SelectedValue.ToString();
        //            _objEmp.Sex = ddlGender.SelectedValue.ToString();
        //            _objEmp.Ref = "";
        //            _objEmp.ACH = Convert.ToInt32(ddlDirectDeposit.SelectedValue.ToString());
        //            _objEmp.ACHType = Convert.ToInt32(ddlAccountType1.SelectedValue.ToString());
        //            _objEmp.ACHRoute = txtBankRoute1.Text;
        //            _objEmp.ACHBank = txtBankAcct1.Text;
        //            //if (txtRehire.Text != "")
        //            //{
        //            //    _objEmp.Anniversary = Convert.ToDateTime(txtRehire.Text);
        //            //}
        //            if (DateTime.TryParse(txtRehire.Text.Trim(), out MinDate))
        //                _objEmp.Anniversary = Convert.ToDateTime(txtRehire.Text.Trim());

        //            _objEmp.Level = 0;
        //            _objEmp.WageCat = Convert.ToInt32(ddlDefaultWage.SelectedValue.ToString());
        //            _objEmp.DSenior = DateTime.Now;
        //            _objEmp.PRWBR = 0;
        //            _objEmp.PDASerialNumber_1 = txtPDASerial.Text;
        //            _objEmp.StatusChange = 0;
        //            _objEmp.SCDate = DateTime.Now;
        //            _objEmp.SCReason = "";
        //            _objEmp.DemoChange = 0;
        //            _objEmp.Language = "";
        //            _objEmp.TicketD = 0;
        //            _objEmp.Custom1 = "";
        //            _objEmp.Custom2 = "";
        //            _objEmp.Custom3 = "";
        //            _objEmp.Custom4 = "";
        //            _objEmp.Custom5 = "";
        //            _objEmp.DDType = Convert.ToInt32(ddlSplitType.SelectedValue.ToString());
        //            _objEmp.DDRate = 0;
        //            _objEmp.ACHType2 = Convert.ToInt32(ddlAccountType2.SelectedValue.ToString());
        //            _objEmp.ACHRoute2 = txtBankRoute2.Text;
        //            _objEmp.ACHBank2 = txtBankAcct2.Text;

        //            if (txtBillRate.Text.Trim() != "")
        //            {
        //                _objEmp.BillRate = Convert.ToDouble(txtBillRate.Text);
        //            }
        //            else
        //            {
        //                _objEmp.BillRate = 0.00;
        //            }
        //            if (txtSales.Text.Trim() != "")
        //            {
        //                _objEmp.BMSales = Convert.ToDouble(txtSales.Text);
        //            }
        //            else
        //            {
        //                _objEmp.BMSales = 0.00;
        //            }
        //            if (txtInvoiceAverage.Text.Trim() != "")
        //            {
        //                _objEmp.BMInvAve = Convert.ToDouble(txtInvoiceAverage.Text);
        //            }
        //            else
        //            {
        //                _objEmp.BMInvAve = 0.00;
        //            }
        //            if (txtClosingPercentage.Text.Trim() != "")
        //            {
        //                _objEmp.BMClosing = Convert.ToDouble(txtClosingPercentage.Text);
        //            }
        //            else
        //            {
        //                _objEmp.BMClosing = 0.00;
        //            }
        //            if (txtBillableEfficiency.Text.Trim() != "")
        //            {
        //                _objEmp.BMBillEff = Convert.ToDouble(txtBillableEfficiency.Text);
        //            }
        //            else
        //            {
        //                _objEmp.BMBillEff = 0.00;
        //            }
        //            if (txtProdcutionEfficiency.Text.Trim() != "")
        //            {
        //                _objEmp.BMProdEff = Convert.ToDouble(txtProdcutionEfficiency.Text);
        //            }
        //            else
        //            {
        //                _objEmp.BMProdEff = 0.00;
        //            }
        //            if (txtAverageTasks.Text.Trim() != "")
        //            {
        //                _objEmp.BMAveTask = Convert.ToInt32(txtAverageTasks.Text);
        //            }
        //            else
        //            {
        //                _objEmp.BMAveTask = 0;
        //            }
        //            if (txtCustom6.Text.Trim() != "")
        //            {
        //                _objEmp.BMCustom1 = Convert.ToInt32(txtCustom6.Text);
        //            }
        //            else
        //            {
        //                _objEmp.BMCustom1 = 0;
        //            }
        //            if (txtCustom7.Text.Trim() != "")
        //            {
        //                _objEmp.BMCustom2 = Convert.ToInt32(txtCustom7.Text);
        //            }
        //            else
        //            {
        //                _objEmp.BMCustom2 = 0;
        //            }
        //            if (txtCustom8.Text.Trim() != "")
        //            {
        //                _objEmp.BMCustom3 = Convert.ToInt32(txtCustom8.Text);
        //            }
        //            else
        //            {
        //                _objEmp.BMCustom3 = 0;
        //            }
        //            if (txtCustom9.Text.Trim() != "")
        //            {
        //                _objEmp.BMCustom4 = Convert.ToInt32(txtCustom9.Text);
        //            }
        //            else
        //            {
        //                _objEmp.BMCustom4 = 0;
        //            }
        //            if (txtCustom10.Text.Trim() != "")
        //            {
        //                _objEmp.BMCustom5 = Convert.ToInt32(txtCustom10.Text);
        //            }
        //            else
        //            {
        //                _objEmp.BMCustom5 = 0;
        //            }


        //            _objEmp.TaxCodeNR = "";
        //            _objEmp.TaxCodeR = "";
        //            _objEmp.DeviceID = "";
        //            _objEmp.MileageRate = 0;
        //            _objEmp.Import1 = "0";
        //            _objEmp.MSDeviceId = "";
        //            _objEmp.TechnicianBio = "";
        //            _objEmp.PayPortalPassword = "";
        //            if (txtSickRate.Text.Trim() != "")
        //            {
        //                _objEmp.SickRate = Convert.ToDouble(txtSickRate.Text);
        //            }
        //            else
        //            {
        //                _objEmp.SickRate = 0.00;
        //            }
        //            if (txtAccuredVacation.Text.Trim() != "")
        //            {
        //                _objEmp.VacAccrued = Convert.ToDouble(txtAccuredVacation.Text);
        //            }
        //            else
        //            {
        //                _objEmp.VacAccrued = 0.00;
        //            }
        //            _objEmp.SickAccrued = 0;
        //            _objEmp.SickUsed = 0;
        //            _objEmp.SickYTD = 0;

        //            _objEmp.SCounty = 0;
        //            _objEmp.PDASerialNumber = txtPDASerial.Text;

        //            _objEmp.City = txtCity.Text;
        //            _objEmp.Zip = txtZip.Text;
        //            _objEmp.Tel = txtPhone.Text;
        //            _objEmp.Address = txtAddress.Text;
        //            _objEmp.Email = txtEmail.Text;
        //            _objEmp.Cell = txtCellular.Text;
        //            _objEmp.Remarks = txtRemark.Text;
        //            _objEmp.Type = 5;
        //            _objEmp.Contact = txtContact.Text;
        //            _objEmp.Website = "";

        //            _objEmp.Country = txtCountry.Text;
        //            _objEmp.Fax = txtFax.Text;

        //            //_objEmp.dtWageCategory = GetWageTableItem();
        //            //_objEmp.dtWageDeduction = GetWageDeductionTableItem();
        //            if (ViewState["WageItems"] != null)
        //            {
        //                DataTable dataTable = (DataTable)ViewState["WageItems"];
        //                dataTable.Columns.Remove("GLName");
        //                dataTable.Columns.Remove("fDesc");
        //                dataTable.Columns.Remove("Checked");
        //                dataTable.AcceptChanges();
        //                _objEmp.dtWageCategory = dataTable;
        //            }
        //            if (ViewState["WageDeductionItems"] != null)
        //            {
        //                DataTable dataTable = (DataTable)ViewState["WageDeductionItems"];
        //                dataTable.Columns.Remove("Checked");
        //                dataTable.Columns.Remove("EmpGLName");
        //                dataTable.Columns.Remove("fDesc");
        //                dataTable.Columns.Remove("CompGLName");
        //                dataTable.Columns.Remove("CompGLEName");
        //                dataTable.AcceptChanges();
        //                _objEmp.dtWageDeduction = dataTable;
        //            }
        //            _objEmp.dtOtherIncome = GetOtherIncomeWageTableItem();

        //            string msg = "Added";
        //            if (ViewState["edit"].ToString() == "0")
        //            {
        //                objBL_Wage.AddEmp(_objEmp);
        //            }
        //            else if (ViewState["edit"].ToString() == "1")
        //            {
        //                msg = "Updated";
        //                _objEmp.ID = Convert.ToInt32(hdnEmpID.Value);
        //                objBL_Wage.UpdateEmp(_objEmp);
        //            }

        //            ////this.programmaticModalPopup.Hide();
        //            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddDeparmenttype", "noty({text: 'Employee " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    string type = "error";
        //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        //    if (str.Contains("Employee already exists, please use different name"))
        //    {
        //        type = "warning";
        //    }
        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddDepttype", "noty({text: '" + str + "', dismissQueue: true,  type : '" + type + "', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        //}


    }
    
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("EmployeeList.aspx");
    }
    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["EmployeeList"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;
            if (index < c)
            {
                string url = "AddEmployee.aspx?id=" + dt.Rows[index - 1]["ID"];
                Response.Redirect(url);
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["EmployeeList"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;
            if (index < c)
            {
                string url = "AddEmployee.aspx?id=" + dt.Rows[index + 1]["ID"];
                Response.Redirect(url);
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void lnkLast_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["EmployeeList"];
            string url = "AddEmployee.aspx?id=" + dt.Rows[dt.Rows.Count - 1]["ID"];
            Response.Redirect(url);
        }
        catch (Exception ex)
        {

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

        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderCstm");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("cstmMgrSub");
        //ul.Style.Add("display", "block");
        //ul.Style.Add("visibility", "visible");

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
    }

    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            ViewState["CompPermission"] = 1;
            //dvCompanyPermission.Visible = true;
            
        }
        else
        {
            ViewState["CompPermission"] = 0;
            //dvCompanyPermission.Visible = false;
        }
    }
    protected void lnkWageCategorySave_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    DataTable dt = GetWageTable();
            

        //    foreach (GridDataItem gr in RadGrid_WageCategoryList.Items)
        //    {
        //        Label lblID = (Label)gr.FindControl("lblId");
        //        CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
        //        if (chkSelect.Checked == true)
        //        {
        //            DataRow dr = dt.NewRow();

        //            DataSet ds = new DataSet();
        //           _objWage.ConnConfig = Session["config"].ToString();
        //            _objWage.ID = Convert.ToInt32(lblID.Text);
        //            ds = objBL_Wage.GetWageByID(_objWage);
        //            //dr = ds.Tables[0].Rows[0];
        //            //dt.Rows.Add(dr.ItemArray);


        //            dr["id"] = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]);
        //            dr["GL"] = Convert.ToInt32(ds.Tables[0].Rows[0]["GL"]);
        //            dr["GLName"] = Convert.ToString(ds.Tables[0].Rows[0]["GLName"]);
        //            dr["Reg"]= Convert.ToDouble(ds.Tables[0].Rows[0]["Reg"]);
        //            dr["OT"]=Convert.ToString(ds.Tables[0].Rows[0]["OT1"]); 
        //            dr["DT"]=Convert.ToInt32(ds.Tables[0].Rows[0]["OT2"]); 
        //            dr["TT"]=Convert.ToInt32(ds.Tables[0].Rows[0]["TT"]); 
        //            dr["FIT"]= Convert.ToInt32(ds.Tables[0].Rows[0]["FIT"]); 
        //            dr["FICA"]= Convert.ToInt32(ds.Tables[0].Rows[0]["FICA"]); 
        //            dr["MEDI"]= Convert.ToInt32(ds.Tables[0].Rows[0]["MEDI"]); 
        //            dr["FUTA"]= Convert.ToInt32(ds.Tables[0].Rows[0]["FUTA"]); 
        //            dr["SIT"]= Convert.ToInt32(ds.Tables[0].Rows[0]["SIT"]); 
        //            dr["Vac"]= Convert.ToInt32(ds.Tables[0].Rows[0]["Vac"]); 
        //            dr["Wc"]= Convert.ToInt32(ds.Tables[0].Rows[0]["Wc"]); 
        //            dr["Uni"]= Convert.ToInt32(ds.Tables[0].Rows[0]["Uni"]); 
        //            dr["InUse"]= Convert.ToInt32(0); 
        //            dr["Sick"]= Convert.ToInt32(ds.Tables[0].Rows[0]["Sick"]); 
        //            dr["Status"]= Convert.ToInt32(ds.Tables[0].Rows[0]["Status"]);

        //            dr["YTD"] = 0.00;
        //            dr["YTDH"]= 0.00;
        //            dr["OYTD"]= 0.00;
        //            dr["OYTDH"]= 0.00;
        //            dr["DYTD"]= 0.00;
        //            dr["DYTDH"]= 0.00;
        //            dr["TYTD"]= 0.00;
        //            dr["TYTDH"]= 0.00;
        //            dr["NT"]= Convert.ToDouble(ds.Tables[0].Rows[0]["NT"]); 
        //            dr["NYTD"]= 0.00;
        //            dr["NYTDH"]= 0.00;
        //            dr["VacR"] = "";
        //            dr["CReg"]= Convert.ToDouble(ds.Tables[0].Rows[0]["CReg"]); 
        //            dr["COT"]= Convert.ToDouble(ds.Tables[0].Rows[0]["COT"]); 
        //            dr["CDT"]= Convert.ToDouble(ds.Tables[0].Rows[0]["CDT"]); 
        //            dr["CNT"]= Convert.ToDouble(ds.Tables[0].Rows[0]["CNT"]); 
        //            dr["CTT"]= Convert.ToDouble(ds.Tables[0].Rows[0]["CTT"]);
        //            dr["fdesc"] = Convert.ToString(ds.Tables[0].Rows[0]["fDesc"]);

        //            dt.Rows.Add(dr);
        //            dt.AcceptChanges();
        //        }
        //        //HyperLink lnkName = (HyperLink)gr.FindControl("lblWageFdesc");
        //        //lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='WageCategory.aspx?id=" + lblID.Text + "'";
        //    }
        //    RadGrid_WageCategory.DataSource = dt;
        //    RadGrid_WageCategory.DataBind();
        //}
        //catch (Exception ex)
        //{
        //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        //    string strerror = "warning";

        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddServicetype", "noty({text: '" + str + "',dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        //}
    }
    protected void lnkWageDeductionListSave_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    DataTable dt = GetWageDeductionTable();


        //    foreach (GridDataItem gr in RadGrid_WageDeductionList.Items)
        //    {
        //        Label lblID = (Label)gr.FindControl("lblId");
        //        CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelectD");
        //        if (chkSelect.Checked == true)
        //        {
        //            DataRow dr = dt.NewRow();

        //            DataSet ds = new DataSet();
        //            _objPRDed.ConnConfig = Session["config"].ToString();
        //            _objPRDed.ID = Convert.ToInt32(lblID.Text);
        //            ds = new BL_Wage().GetWageDeductionByID(_objPRDed);
        //            //dr = ds.Tables[0].Rows[0];

        //            //dt.Rows.Add(dr.ItemArray);


        //            dr["id"]= Convert.ToInt32(ds.Tables[0].Rows[0]["id"]);
        //            dr["BasedOn"]= Convert.ToInt32(ds.Tables[0].Rows[0]["BasedOn"]);
        //            dr["AccruedOn"]= Convert.ToInt32(ds.Tables[0].Rows[0]["AccruedOn"]);
        //            dr["ByW"]= Convert.ToInt32(ds.Tables[0].Rows[0]["ByW"]);
        //            dr["EmpRate"]= Convert.ToDouble(ds.Tables[0].Rows[0]["EmpRate"]);
        //            dr["EmpTop"]= Convert.ToDouble(ds.Tables[0].Rows[0]["EmpTop"]);
        //            dr["EmpGL"]= Convert.ToInt32(ds.Tables[0].Rows[0]["EmpGL"]);
        //            dr["EmpGLName"] = Convert.ToString(ds.Tables[0].Rows[0]["EmpGLAcct"]);
        //            dr["CompRate"]= Convert.ToDouble(ds.Tables[0].Rows[0]["CompRate"]);
        //            dr["CompTop"]= Convert.ToDouble(ds.Tables[0].Rows[0]["CompTop"]);
        //            dr["CompGL"]= Convert.ToInt32(ds.Tables[0].Rows[0]["CompGL"]);
        //            dr["CompGLName"] = Convert.ToString(ds.Tables[0].Rows[0]["CompGLAcct"]);
        //            dr["CompGLE"]= Convert.ToInt32(ds.Tables[0].Rows[0]["CompGLE"]);
        //            dr["CompGLEName"] = Convert.ToString(ds.Tables[0].Rows[0]["CompGLEAcct"]);
        //            dr["InUse"]= 0;
        //            dr["YTD"]= 0.00;
        //            dr["YTDC"]= 0.00;
        //            dr["fDesc"] = Convert.ToString(ds.Tables[0].Rows[0]["fDesc"]);
        //            dt.Rows.Add(dr);
        //            dt.AcceptChanges();
        //        }
        //        //HyperLink lnkName = (HyperLink)gr.FindControl("lblWageFdesc");
        //        //lnkName.Attributes["onclick"] = gr.Attributes["ondblclick"] = "location.href='WageCategory.aspx?id=" + lblID.Text + "'";
        //    }
        //    RadGrid_WageDeduction.DataSource = dt;
        //    RadGrid_WageDeduction.DataBind();
        //}
        //catch (Exception ex)
        //{
        //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        //    string strerror = "warning";

        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddServicetype", "noty({text: '" + str + "',dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        //}
    }


    protected void lnkWageDeductionSave_Click(object sender, EventArgs e)
    {
        //IsAddEdit = false;
        try
        {
            //if (!Page.IsValid) { return; }

            ////int RT = Convert.ToInt32(ddlRT.SelectedValue);
            ////int OT = Convert.ToInt32(DDlot.SelectedValue);
            ////int NT = Convert.ToInt32(ddl1Point7.SelectedValue);
            ////int DT = Convert.ToInt32(ddlDT.SelectedValue);

            //int RT = 0;
            //int OT = 0;
            //int NT = 0;
            //int DT = 0;
            //int STATUS = 0;
            //int ExpenseGL = 0;
            //int InterestGL = 0;
            //int LaborWageC = 0;
            //int InvID = 0;
            //string strddldepartment = "";
            //if (ddlRT1.SelectedValue != "" && ddlRT1.SelectedValue != null)
            //{
            //    RT = Convert.ToInt32(ddlRT1.SelectedValue);
            //}
            //if (DDlot1.SelectedValue != "" && DDlot1.SelectedValue != null)
            //{
            //     OT = Convert.ToInt32(DDlot1.SelectedValue);
            //}
            //if (ddl1Point71.SelectedValue != "" && ddl1Point71.SelectedValue != null)
            //{
            //     NT = Convert.ToInt32(ddl1Point71.SelectedValue);
            //}
            //if (ddlDT1.SelectedValue != "" && ddlDT1.SelectedValue != null)
            //{
            //     DT = Convert.ToInt32(ddlDT1.SelectedValue);
            //}
            //string ConnConfig = Session["config"].ToString();
            //string TYPE = txtServiceType.Text;
            //string FDESC = txtServiceTypeDesc.Text;
            //string REMARKS = txtServRemarks.Text;
            //if (ddlServiceTypeStatus.SelectedValue != "" && ddlServiceTypeStatus.SelectedValue != null)
            //{
            //     STATUS = Convert.ToInt32(ddlServiceTypeStatus.SelectedValue);
            //}

            //string LocType = ddlLocationtype1.SelectedValue;
            //if (DDLEGL1.SelectedValue != "" && DDLEGL1.SelectedValue != null)
            //{
            //     ExpenseGL = Convert.ToInt32(DDLEGL1.SelectedValue);
            //}
            //if (DDLIGL1.SelectedValue != "" && DDLIGL1.SelectedValue != null)
            //{
            //     InterestGL = Convert.ToInt32(DDLIGL1.SelectedValue);
            //}
            //if (ddlWC1.SelectedValue != "" && ddlWC1.SelectedValue != null)
            //{
            //     LaborWageC = Convert.ToInt32(ddlWC1.SelectedValue);
            //}
            //if (ddlBillingCode1.SelectedValue != "" && ddlBillingCode1.SelectedValue != null)
            //{
            //     InvID = Convert.ToInt32(ddlBillingCode1.SelectedValue);
            //}
            //string route = GetSelectedCategory();
            //string msg = string.Empty;
            //strddldepartment = GetSelectedDepartment();
            //STATUS = ddlServiceTypeStatus.SelectedIndex;
            //if (hdnAddEdit.Value == "0")
            //{

            //    msg = "Added";

            //    objBL_User.AddServiceType(ConnConfig, TYPE, FDESC, REMARKS, RT, OT, NT, DT, STATUS, LocType, ExpenseGL, InterestGL, LaborWageC, InvID, route, strddldepartment);

            //}
            //else
            //{
            //    int Flage = Convert.ToInt32(hdnFlage.Value);  //If Yes for Update =1 else 0
            //    var userName = Session["username"].ToString();
            //    objBL_User.UpdateServiceType(ConnConfig, TYPE, FDESC, REMARKS, RT, OT, NT, DT, STATUS, LocType, ExpenseGL, InterestGL, LaborWageC, InvID, route,Flage, strddldepartment, userName);
            //    msg = "Updated";
            //}

            ////ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "", true);

            //if (!string.IsNullOrEmpty(msg))
            //{
            //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddServicetype", "noty({text: 'Service Type " + msg + " successfully.', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});  window.setTimeout(function () { window.location.href = 'ServiceType.aspx'; }, 500); ", true);
            //}
            //FillWageDeduction();
            //RadGrid_WageCategoryList.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string strerror = "warning";

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddServicetype", "noty({text: '" + str + "',dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
        protected void lnkDeleteWageDeduction_Click(object sender, EventArgs e)
        {
            
            
        }

        
    
        protected void lnkEditWageDeduction_Click(object sender, EventArgs e)
        {

            //if (hdnAddEdit.Value != "")
            //{
            //    string route = "";
            //    string Department = "";

            //    DataSet ds = objBL_User.GetServiceType(Session["config"].ToString(), hdnAddEdit.Value);

            //    foreach (RadComboBoxItem itm in ddlRoute.Items)
            //    {
            //        itm.Checked = false;
            //    }

            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {

            //        ddlRT1.Text = dr["RTNAME"].ToString();
            //        DDlot1.Text = dr["OTNAME"].ToString();
            //        ddl1Point71.Text = dr["NTNAME"].ToString();
            //        ddlDT1.Text = dr["DTNAME"].ToString();
            //        DDLEGL1.Text = dr["ExpenseGLNAME"].ToString();
            //        DDLIGL1.Text = dr["InterestGLNAME"].ToString();
            //        ddlBillingCode1.Text = dr["InvIDNAME"].ToString();
            //        ddlWC1.Text = dr["LaborWageCName"].ToString();

            //        ddlServiceTypeStatus.SelectedValue = dr["status"].ToString();
            //        ddlRT1.SelectedValue = dr["RT"].ToString();
            //        DDlot1.SelectedValue = dr["OT"].ToString();
            //        ddl1Point71.SelectedValue = dr["NT"].ToString();
            //        ddlDT1.SelectedValue = dr["DT"].ToString();
            //        DDLEGL1.SelectedValue = dr["ExpenseGL"].ToString();
            //        DDLIGL1.SelectedValue = dr["InterestGL"].ToString();
            //        ddlBillingCode1.SelectedValue = dr["InvID"].ToString();
            //        ddlWC1.SelectedValue = dr["LaborWageC"].ToString();



            //        txtServiceType.Text = dr["TYPE"].ToString();
            //        txtServiceType.ReadOnly = true;
            //        txtServiceTypeDesc.Text = dr["FDESC"].ToString();
            //        txtServRemarks.Text = dr["REMARKS"].ToString();

            //        string Ltype = dr["LocTypeNAME"].ToString();

            //        if (!string.IsNullOrEmpty(Ltype))
            //        {
            //            ddlLocationtype1.Text = dr["LocTypeNAME"].ToString();
            //            ddlLocationtype1.SelectedValue = dr["LocType"].ToString();
            //        }

            //        route = dr["route"].ToString();
            //        Department = dr["Department"].ToString();
            //        string routelabel = dr["routelabel"].ToString();
            //        lblroutelabel.InnerText = routelabel == string.Empty ? "Route" : routelabel;
            //        break;
            //    }

            //    try
            //    {

            //        if (!string.IsNullOrEmpty(route))
            //        {
            //            List<string> result = route.ToString().Split(',').ToList();

            //            foreach (RadComboBoxItem itm in ddlRoute.Items)
            //            {
            //                string s1 = itm.Value;

            //                foreach (var item in result)
            //                {
            //                    string s2 = item.ToString().Replace("'", "");

            //                    if (s1 == s2)
            //                    {
            //                        itm.Checked = true;
            //                    }
            //                }
            //            }
            //        }
            //        if (!string.IsNullOrEmpty(Department))
            //        {
            //            List<string> result = Department.ToString().Split(',').ToList();

            //            foreach (RadComboBoxItem itm in ddldepartment1.Items)
            //            {
            //                string s1 = itm.Value;

            //                foreach (var item in result)
            //                {
            //                    string s2 = item.ToString().Replace("'", "");

            //                    if (s1 == s2)
            //                    {
            //                        itm.Checked = true;
            //                    }
            //                }
            //            }
            //        }


            //    }
            //    catch
            //    {

            //    }
            //    hdnFlage.Value = "0";
            //    WageDeductionWindow.Title = "Edit Service Type";

            //    string script = "function f(){$find(\"" + WageDeductionWindow.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
            //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "keyedit", script, true);
            //}

        }

        protected void lnkAddWageDeduction_Click(object sender, EventArgs e)
        {
            try
            {

              
                //DDlot1.SelectedIndex = 0;
                //ddl1Point71.SelectedIndex = 0;
                //ddlDT1.SelectedIndex = 0;
                //txtServiceType.Text = "";
                //txtServiceType.ReadOnly = false;
                //txtServiceTypeDesc.Text = "";
                //txtServRemarks.Text = "";
              
                //DDLEGL1.SelectedIndex = 0;
                //DDLIGL1.SelectedIndex = 0;
                //ddlWC1.SelectedIndex = 0;
                //ddlBillingCode1.SelectedIndex = 0;
                //ddlLocationtype1.SelectedIndex = 0;
                
                //foreach (RadComboBoxItem itm in ddlRoute.Items)
                //{
                //    itm.Checked = false;
                //}
                //foreach (RadComboBoxItem itm in ddldepartment1.Items)
                //{
                //    itm.Checked = false;
                //}

                //ddlRT1.Items.Clear();
                //DDlot1.Items.Clear();
                //ddl1Point71.Items.Clear();
                //ddlDT1.Items.Clear();
                //DDLEGL1.Items.Clear();
                //DDLIGL1.Items.Clear();
                //ddlWC1.Items.Clear();
                //ddlBillingCode1.Items.Clear();
                //ddlLocationtype1.Items.Clear();
                
                //ddlRT1.Text = "";
                //DDlot1.Text = "";
                //ddl1Point71.Text = "";
                //ddlDT1.Text = "";
                //DDLEGL1.Text = "";
                //DDLIGL1.Text = "";
                //ddlWC1.Text = "";
                //ddlBillingCode1.Text = "";
                //ddlLocationtype1.Text = "";
                
                //DataSet ds = objBL_User.GetServiceType(Session["config"].ToString(), "0");

                //foreach (DataRow dr in ds.Tables[0].Rows)
                //{
                //    string routelabel = dr["routelabel"].ToString();
                //    lblroutelabel.InnerText = routelabel == string.Empty ? "Route" : routelabel;
                //}

                ////WageDeductionWindow.Title = "Add Wage Deduction";

                ////hdnAddEdit.Value = "0";
                ////hdnFlage.Value = "0";
                ////string script = "function f(){$find(\"" + WageDeductionWindow.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
                ////ScriptManager.RegisterStartupScript(Page, Page.GetType(), "keyaddqq", script, true);


            }
            catch { }
        }

    private void CreateWageTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Wage", typeof(int));
        dt.Columns.Add("Reg", typeof(double));
        dt.Columns.Add("OT", typeof(double));
        dt.Columns.Add("DT", typeof(double));
        dt.Columns.Add("TT", typeof(double));
        dt.Columns.Add("NT", typeof(double));
        dt.Columns.Add("CReg", typeof(double));
        dt.Columns.Add("COT", typeof(double));
        dt.Columns.Add("CDT", typeof(double));
        dt.Columns.Add("CTT", typeof(double));
        dt.Columns.Add("CNT", typeof(double));

        dt.Columns.Add("GL", typeof(int));
        dt.Columns.Add("FIT", typeof(int));
        dt.Columns.Add("FICA", typeof(int));
        dt.Columns.Add("MEDI", typeof(int));
        dt.Columns.Add("FUTA", typeof(int));
        dt.Columns.Add("SIT", typeof(int));
        dt.Columns.Add("Vac", typeof(int));
        dt.Columns.Add("Wc", typeof(int));
        dt.Columns.Add("Uni", typeof(int));
        dt.Columns.Add("InUse", typeof(int));
        dt.Columns.Add("Sick", typeof(int));
        dt.Columns.Add("Status", typeof(int));
        dt.Columns.Add("YTD", typeof(double));
        dt.Columns.Add("YTDH", typeof(double));
        dt.Columns.Add("OYTD", typeof(double));
        dt.Columns.Add("OYTDH", typeof(double));
        dt.Columns.Add("DYTD", typeof(double));
        dt.Columns.Add("DYTDH", typeof(double));
        dt.Columns.Add("TYTD", typeof(double));
        dt.Columns.Add("TYTDH", typeof(double));
        dt.Columns.Add("NYTD", typeof(double));
        dt.Columns.Add("NYTDH", typeof(double));
        dt.Columns.Add("VacR", typeof(string));
        dt.Columns.Add("fdesc", typeof(string));
        dt.Columns.Add("GLName", typeof(string));

        DataRow dr = dt.NewRow();
        dr["Wage"] = 0;
        dr["Reg"] = 0;
        dr["OT"] = 0;
        dr["DT"] = 0;
        dr["TT"] = 0;
        dr["NT"] = 0;
        dr["CReg"] = 0;
        dr["COT"] = 0;
        dr["CDT"] = 0;
        dr["CTT"] = 0;
        dr["CNT"] = 0;

        dr["GL"] = 0;
        dr["FIT"] = 0;
        dr["FICA"] = 0;
        dr["MEDI"] = 0;
        dr["FUTA"] = 0;
        dr["SIT"] = 0;
        dr["Vac"] = 0;
        dr["Wc"] = 0;
        dr["Uni"] = 0;
        dr["InUse"] = 0;
        dr["Sick"] = 0;
        dr["Status"] = 0;
        dr["YTD"] = 0;
        dr["YTDH"] = 0;
        dr["OYTD"] = 0;
        dr["OYTDH"] = 0;
        dr["DYTD"] = 0;
        dr["DYTDH"] = 0;
        dr["TYTD"] = 0;
        dr["TYTDH"] = 0;
        dr["NYTD"] = 0;
        dr["NYTDH"] = 0;
        dr["VacR"] = "";
        dr["fdesc"] = "";
        dr["GLName"] = "";

        dt.Rows.Add(dr);
        ViewState["WageItems"] = dt;
        gvWagePayRate.DataSource = dt;

        //gvWagePayRate.DataBind();
    }
    private void FillWageCategorys()
    {
        try
        {
            DataSet ds = new DataSet();
            _objWage.ConnConfig = HttpContext.Current.Session["config"].ToString();
            ds = objBL_Wage.GetAllWage(_objWage);
            if (ds.Tables.Count > 0)
            {
                DataRow dr = ds.Tables[0].NewRow();
                dr["Wage"] = 0;
                dr["fDesc"] = "Select Wage";
                ds.Tables[0].Rows.InsertAt(dr, 0);
                dtWage = ds.Tables[0];
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvWagePayRate_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        CreateWageTable();
        
        if (Request.QueryString["id"] != null)
        {
            DataSet dsWage = new DataSet();
            _objEmp.ConnConfig = Session["config"].ToString();
            _objEmp.ID = Convert.ToInt32(Request.QueryString["id"].ToString());
            dsWage = objBL_Wage.GetEmployeeListByID(_objEmp);

            //ViewState["dsWage"] = dsWage;
            
            if (dsWage.Tables[1].Rows.Count > 0)
            {
                gvWagePayRate.DataSource = dsWage.Tables[1];
                //gvWagePayRate.DataBind();
                ViewState["WageItems"] = dsWage.Tables[1];
            }
            else
            {
                CreateWageTable();
            }

        }
    }
    protected void gvWagePayRate_ItemDataBound(object sender, GridItemEventArgs e)
    {
        string TicketID = string.Empty;

        if (e.Item is GridDataItem)
        {
            DropDownList ddlWage = (DropDownList)e.Item.FindControl("ddlWage");
            Int32 UserID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"]);

            if (IsWageRateIsUsed(UserID, Convert.ToInt32(ddlWage.SelectedItem.Value), out TicketID))
            {
                string str = "Wage category is used in Ticket #" + TicketID + "!";

                ddlWage.Attributes["onclick"] = "   noty({ text: '" + str + "', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";

                foreach (ListItem item in ddlWage.Items)
                {
                    if (item.Value != ddlWage.SelectedItem.Value) { item.Enabled = false; }
                }
            }


        }
    }
    protected void gvWagePayRate_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = gvWagePayRate.Items.Count - 1;
        GridDataItem row = gvWagePayRate.Items[rowIndex];
        //HiddenField hdnIndex = row.FindControl("hdnIndex") as HiddenField;

        DataTable dt = new DataTable();

        dt = GetWageGridItems();
        DataRow dr = dt.NewRow();
        dr["Wage"] = 0;
        dr["Reg"] = 0;
        dr["OT"] = 0;
        dr["DT"] = 0;
        dr["TT"] = 0;
        dr["NT"] = 0;
        dr["CReg"] = 0;
        dr["COT"] = 0;
        dr["CDT"] = 0;
        dr["CTT"] = 0;
        dr["CNT"] = 0;
        dr["Check"] = false;

        dr["GL"] = 0;
        dr["FIT"] = 0;
        dr["FICA"] = 0;
        dr["MEDI"] = 0;
        dr["FUTA"] = 0;
        dr["SIT"] = 0;
        dr["Vac"] = 0;
        dr["Wc"] = 0;
        dr["Uni"] = 0;
        dr["InUse"] = 0;
        dr["Sick"] = 0;
        dr["Status"] = 0;
        dr["YTD"] = 0;
        dr["YTDH"] = 0;
        dr["OYTD"] = 0;
        dr["OYTDH"] = 0;
        dr["DYTD"] = 0;
        dr["DYTDH"] = 0;
        dr["TYTD"] = 0;
        dr["TYTDH"] = 0;
        dr["NYTD"] = 0;
        dr["NYTDH"] = 0;
        dr["VacR"] = "";
        dr["fdesc"] = "";
        dr["GLName"] = "";

        dt.Rows.Add(dr);

        gvWagePayRate.DataSource = dt;
        //gvWagePayRate.DataBind();

        ViewState["WageItems"] = dt;
    }
    private DataTable GetWageGridItems()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Wage", typeof(int));
        dt.Columns.Add("Reg", typeof(double));
        dt.Columns.Add("OT", typeof(double));
        dt.Columns.Add("DT", typeof(double));
        dt.Columns.Add("TT", typeof(double));
        dt.Columns.Add("NT", typeof(double));
        dt.Columns.Add("CReg", typeof(double));
        dt.Columns.Add("COT", typeof(double));
        dt.Columns.Add("CDT", typeof(double));
        dt.Columns.Add("CTT", typeof(double));
        dt.Columns.Add("CNT", typeof(double));
        dt.Columns.Add("Checked", typeof(bool));

        dt.Columns.Add("GL", typeof(int));
        dt.Columns.Add("FIT", typeof(int));
        dt.Columns.Add("FICA", typeof(int));
        dt.Columns.Add("MEDI", typeof(int));
        dt.Columns.Add("FUTA", typeof(int));
        dt.Columns.Add("SIT", typeof(int));
        dt.Columns.Add("Vac", typeof(int));
        dt.Columns.Add("Wc", typeof(int));
        dt.Columns.Add("Uni", typeof(int));
        dt.Columns.Add("InUse", typeof(int));
        dt.Columns.Add("Sick", typeof(int));
        dt.Columns.Add("Status", typeof(int));
        dt.Columns.Add("YTD", typeof(double));
        dt.Columns.Add("YTDH", typeof(double));
        dt.Columns.Add("OYTD", typeof(double));
        dt.Columns.Add("OYTDH", typeof(double));
        dt.Columns.Add("DYTD", typeof(double));
        dt.Columns.Add("DYTDH", typeof(double));
        dt.Columns.Add("TYTD", typeof(double));
        dt.Columns.Add("TYTDH", typeof(double));
        dt.Columns.Add("NYTD", typeof(double));
        dt.Columns.Add("NYTDH", typeof(double));
        dt.Columns.Add("VacR", typeof(string));
        dt.Columns.Add("fdesc", typeof(string));
        dt.Columns.Add("GLName", typeof(string));

        try
        {
            string strItems = hdnWageRate.Value.Trim();

            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objWageItemData = new List<Dictionary<object, object>>();
                objWageItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                objWageItemData.RemoveAt(0);
                objWageItemData.RemoveAt(0);
                foreach (Dictionary<object, object> dict in objWageItemData)
                {
                    i++;
                    DataRow dr = dt.NewRow();
                    if (dict["ddlWage"].ToString() != "0")
                    {
                        dr["Wage"] = Convert.ToInt32(dict["ddlWage"]);
                    }
                    else
                    {
                        dr["Wage"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["txtReg"].ToString()))
                    {
                        dr["Reg"] = Convert.ToDouble(dict["txtReg"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtOt"].ToString()))
                    {
                        dr["OT"] = Convert.ToDouble(dict["txtOt"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtNt"].ToString()))
                    {
                        dr["NT"] = Convert.ToDouble(dict["txtNt"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtDt"].ToString()))
                    {
                        dr["DT"] = Convert.ToDouble(dict["txtDt"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtTt"].ToString()))
                    {
                        dr["TT"] = Convert.ToDouble(dict["txtTt"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtCReg"].ToString()))
                    {
                        dr["CReg"] = Convert.ToDouble(dict["txtCReg"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtCOt"].ToString()))
                    {
                        dr["COT"] = Convert.ToDouble(dict["txtCOt"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtCNt"].ToString()))
                    {
                        dr["CNT"] = Convert.ToDouble(dict["txtCNt"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtCDt"].ToString()))
                    {
                        dr["CDT"] = Convert.ToDouble(dict["txtCDt"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtCTt"].ToString()))
                    {
                        dr["CTT"] = Convert.ToDouble(dict["txtCTt"]);
                    }

                    if (!string.IsNullOrEmpty(dict["hdnGL"].ToString()))
                    {
                        dr["GL"] = Convert.ToInt32(dict["hdnGL"]);
                    }
                    else
                    {
                        dr["GL"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnFIT"].ToString()))
                    {
                        dr["FIT"] = Convert.ToInt32(dict["hdnFIT"]);
                    }
                    else
                    {
                        dr["FIT"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnFICA"].ToString()))
                    {
                        dr["FICA"] = Convert.ToInt32(dict["hdnFICA"]);
                    }
                    else
                    {
                        dr["FICA"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnMEDI"].ToString()))
                    {
                        dr["MEDI"] = Convert.ToInt32(dict["hdnMEDI"]);
                    }
                    else
                    {
                        dr["MEDI"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnFUTA"].ToString()))
                    {
                        dr["FUTA"] = Convert.ToInt32(dict["hdnFUTA"]);
                    }
                    else
                    {
                        dr["FUTA"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnSIT"].ToString()))
                    {
                        dr["SIT"] = Convert.ToInt32(dict["hdnSIT"]);
                    }
                    else
                    {
                        dr["SIT"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnVac"].ToString()))
                    {
                        dr["Vac"] = Convert.ToInt32(dict["hdnVac"]);
                    }
                    else
                    {
                        dr["Vac"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnWc"].ToString()))
                    {
                        dr["Wc"] = Convert.ToInt32(dict["hdnWc"]);
                    }
                    else
                    {
                        dr["Wc"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnUni"].ToString()))
                    {
                        dr["Uni"] = Convert.ToInt32(dict["hdnUni"]);
                    }
                    else
                    {
                        dr["Uni"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnInUse"].ToString()))
                    {
                        dr["InUse"] = Convert.ToInt32(dict["hdnInUse"]);
                    }
                    else
                    {
                        dr["InUse"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnSick"].ToString()))
                    {
                        dr["Sick"] = Convert.ToInt32(dict["hdnSick"]);
                    }
                    else
                    {
                        dr["Sick"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnStatus"].ToString()))
                    {
                        dr["Status"] = Convert.ToInt32(dict["hdnStatus"]);
                    }
                    else
                    {
                        dr["Status"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnYTD"].ToString()))
                    {
                        dr["YTD"] = Convert.ToDouble(dict["hdnYTD"]);
                    }
                    else
                    {
                        dr["YTD"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnYTDH"].ToString()))
                    {
                        dr["YTDH"] = Convert.ToDouble(dict["hdnYTDH"]);
                    }
                    else
                    {
                        dr["YTDH"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnOYTD"].ToString()))
                    {
                        dr["OYTD"] = Convert.ToDouble(dict["hdnOYTD"]);
                    }
                    else
                    {
                        dr["OYTD"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnOYTDH"].ToString()))
                    {
                        dr["OYTDH"] = Convert.ToDouble(dict["hdnOYTDH"]);
                    }
                    else
                    {
                        dr["OYTDH"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnDYTD"].ToString()))
                    {
                        dr["DYTD"] = Convert.ToDouble(dict["hdnDYTD"]);
                    }
                    else
                    {
                        dr["DYTD"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnDYTDH"].ToString()))
                    {
                        dr["DYTDH"] = Convert.ToDouble(dict["hdnDYTDH"]);
                    }
                    else
                    {
                        dr["DYTDH"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnTYTD"].ToString()))
                    {
                        dr["TYTD"] = Convert.ToDouble(dict["hdnTYTD"]);
                    }
                    else
                    {
                        dr["TYTD"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnTYTDH"].ToString()))
                    {
                        dr["TYTDH"] = Convert.ToDouble(dict["hdnTYTDH"]);
                    }
                    else
                    {
                        dr["TYTDH"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnNYTD"].ToString()))
                    {
                        dr["NYTD"] = Convert.ToDouble(dict["hdnNYTD"]);
                    }
                    else
                    {
                        dr["NYTD"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnNYTDH"].ToString()))
                    {
                        dr["NYTDH"] = Convert.ToDouble(dict["hdnNYTDH"]);
                    }
                    else
                    {
                        dr["NYTDH"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnVacR"].ToString()))
                    {
                        dr["VacR"] = Convert.ToString(dict["hdnVacR"]);
                    }
                    else
                    {
                        dr["VacR"] = "";
                    }
                    if (!string.IsNullOrEmpty(dict["hdnfdesc"].ToString()))
                    {
                        dr["fdesc"] = Convert.ToString(dict["hdnfdesc"]);
                    }
                    else
                    {
                        dr["fdesc"] = "";
                    }
                    if (!string.IsNullOrEmpty(dict["hdnGLName"].ToString()))
                    {
                        dr["GLName"] = Convert.ToString(dict["hdnGLName"]);
                    }
                    else
                    {
                        dr["GLName"] = "";
                    }






                    try
                    {
                        if (!string.IsNullOrEmpty(dict["chkSelect"].ToString()) && dict["chkSelect"].ToString() == "on")
                        {
                            dr["Checked"] = true;
                        }
                        else
                        {
                            dr["Checked"] = false;
                        }
                    }
                    catch (Exception)
                    {
                        dr["Checked"] = false;
                    }

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
    protected void imgBtnWageDelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string TicketID = string.Empty;
            DataTable dt = GetWageGridItems();
            if (dt.Rows.Count > 1)
            {
                foreach (GridDataItem gr in gvWagePayRate.Items)
                {
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                    if (chkSelect.Checked == true)
                    {
                        DropDownList ddlWage = (DropDownList)gr.FindControl("ddlWage");
                        Int32 UserID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"]);
                        if (!IsWageRateIsUsed(UserID, Convert.ToInt32(ddlWage.SelectedItem.Value), out TicketID))
                        {
                            DataRow dr = dt.Select("Wage = " + Convert.ToInt32(ddlWage.SelectedValue)).FirstOrDefault();
                            if (dr != null)
                                dt.Rows.Remove(dr);
                        }
                        else
                        {
                            string str = "Wage category is used in Ticket #" + TicketID + "!";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "key112", "noty({text: '" + str + "',dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                            return;
                        }
                    }
                }
            }
            else if (dt.Rows.Count > 0)
            {
                foreach (GridDataItem gr in gvWagePayRate.Items)
                {
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                    if (chkSelect.Checked == true)
                    {
                        DropDownList ddlWage = (DropDownList)gr.FindControl("ddlWage");
                        Int32 UserID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"]);
                        if (!IsWageRateIsUsed(UserID, Convert.ToInt32(ddlWage.SelectedItem.Value), out TicketID))
                        {
                            if (ddlWage.SelectedValue != "0")
                            {
                                DataRow dr = dt.Select("Wage = " + Convert.ToInt32(ddlWage.SelectedValue)).FirstOrDefault();
                                if (dr != null)
                                    dt.Rows.Remove(dr);

                                dr = dt.NewRow();
                                dr["Wage"] = 0;
                                dr["Reg"] = 0;
                                dr["OT"] = 0;
                                dr["DT"] = 0;
                                dr["TT"] = 0;
                                dr["NT"] = 0;
                                dr["CReg"] = 0;
                                dr["COT"] = 0;
                                dr["CDT"] = 0;
                                dr["CTT"] = 0;
                                dr["CNT"] = 0;

                                dr["GL"] = 0;
                                dr["FIT"] = 0;
                                dr["FICA"] = 0;
                                dr["MEDI"] = 0;
                                dr["FUTA"] = 0;
                                dr["SIT"] = 0;
                                dr["Vac"] = 0;
                                dr["Wc"] = 0;
                                dr["Uni"] = 0;
                                dr["InUse"] = 0;
                                dr["Sick"] = 0;
                                dr["Status"] = 0;
                                dr["YTD"] = 0;
                                dr["YTDH"] = 0;
                                dr["OYTD"] = 0;
                                dr["OYTDH"] = 0;
                                dr["DYTD"] = 0;
                                dr["DYTDH"] = 0;
                                dr["TYTD"] = 0;
                                dr["TYTDH"] = 0;
                                dr["NYTD"] = 0;
                                dr["NYTDH"] = 0;
                                dr["VacR"] = "";
                                dr["fdesc"] = "";
                                dr["GLName"] = "";
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            string str = "Wage category is used in Ticket #" + TicketID + "!";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "key1512", "noty({text: '" + str + "',dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                            return;
                        }

                    }
                }
            }
            ViewState["WageItems"] = dt;
            gvWagePayRate.DataSource = dt;
            //gvWagePayRate.DataBind();
            gvWagePayRate.Rebind();

            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "RemoveWageGridRow", "RemoveWageGridRow('" + gvWagePayRate.ClientID + "')", true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlWage_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlWage = (DropDownList)sender;
            GridDataItem row = (GridDataItem)ddlWage.NamingContainer;
            TextBox txtReg = (TextBox)row.FindControl("txtReg");
            TextBox txtOt = (TextBox)row.FindControl("txtOt");
            TextBox txtNt = (TextBox)row.FindControl("txtNt");
            TextBox txtDt = (TextBox)row.FindControl("txtDt");
            TextBox txtTt = (TextBox)row.FindControl("txtTt");
            TextBox txtCReg = (TextBox)row.FindControl("txtCReg");
            TextBox txtCOt = (TextBox)row.FindControl("txtCOt");
            TextBox txtCNt = (TextBox)row.FindControl("txtCNt");
            TextBox txtCDt = (TextBox)row.FindControl("txtCDt");
            TextBox txtCTt = (TextBox)row.FindControl("txtCTt");

            HiddenField hdnGL = (HiddenField)row.FindControl("hdnGL");
            HiddenField hdnFIT = (HiddenField)row.FindControl("hdnFIT");
            HiddenField hdnFICA = (HiddenField)row.FindControl("hdnFICA");
            HiddenField hdnMEDI = (HiddenField)row.FindControl("hdnMEDI");
            HiddenField hdnFUTA = (HiddenField)row.FindControl("hdnFUTA");
            HiddenField hdnSIT = (HiddenField)row.FindControl("hdnSIT");
            HiddenField hdnVac = (HiddenField)row.FindControl("hdnVac");
            HiddenField hdnWc = (HiddenField)row.FindControl("hdnWc");
            HiddenField hdnUni = (HiddenField)row.FindControl("hdnUni");
            HiddenField hdnInUse = (HiddenField)row.FindControl("hdnInUse");
            HiddenField hdnSick = (HiddenField)row.FindControl("hdnSick");
            HiddenField hdnStatus = (HiddenField)row.FindControl("hdnStatus");
            HiddenField hdnYTD = (HiddenField)row.FindControl("hdnYTD");
            HiddenField hdnYTDH = (HiddenField)row.FindControl("hdnYTDH");
            HiddenField hdnOYTD = (HiddenField)row.FindControl("hdnOYTD");
            HiddenField hdnOYTDH = (HiddenField)row.FindControl("hdnOYTDH");
            HiddenField hdnDYTD = (HiddenField)row.FindControl("hdnDYTD");
            HiddenField hdnDYTDH = (HiddenField)row.FindControl("hdnDYTDH");
            HiddenField hdnTYTD = (HiddenField)row.FindControl("hdnTYTD");
            HiddenField hdnTYTDH = (HiddenField)row.FindControl("hdnTYTDH");
            HiddenField hdnNYTD = (HiddenField)row.FindControl("hdnNYTD");
            HiddenField hdnNYTDH = (HiddenField)row.FindControl("hdnNYTDH");
            HiddenField hdnVacR = (HiddenField)row.FindControl("hdnVacR");
            HiddenField hdnfdesc = (HiddenField)row.FindControl("hdnfdesc");
            HiddenField hdnGLName = (HiddenField)row.FindControl("hdnGLName");

            _objWage.ConnConfig = Session["config"].ToString();
            _objWage.ID = Convert.ToInt32(ddlWage.SelectedValue);
            DataSet ds = objBL_User.GetWageByID(_objWage);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtReg.Text = string.Format("{0:n}", Convert.ToDouble(dr["Reg"] == DBNull.Value ? 0 : dr["Reg"]));
                txtOt.Text = string.Format("{0:n}", Convert.ToDouble(dr["OT1"] == DBNull.Value ? 0 : dr["OT1"]));
                txtNt.Text = string.Format("{0:n}", Convert.ToDouble(dr["NT"] == DBNull.Value ? 0 : dr["NT"]));
                txtDt.Text = string.Format("{0:n}", Convert.ToDouble(dr["OT2"] == DBNull.Value ? 0 : dr["OT2"]));
                txtTt.Text = string.Format("{0:n}", Convert.ToDouble(dr["TT"] == DBNull.Value ? 0 : dr["TT"]));
                txtCReg.Text = string.Format("{0:n}", Convert.ToDouble(dr["CReg"] == DBNull.Value ? 0 : dr["CReg"]));
                txtCOt.Text = string.Format("{0:n}", Convert.ToDouble(dr["COT"] == DBNull.Value ? 0 : dr["COT"]));
                txtCNt.Text = string.Format("{0:n}", Convert.ToDouble(dr["CNT"] == DBNull.Value ? 0 : dr["CNT"]));
                txtCDt.Text = string.Format("{0:n}", Convert.ToDouble(dr["CDT"] == DBNull.Value ? 0 : dr["CDT"]));
                txtCTt.Text = string.Format("{0:n}", Convert.ToDouble(dr["CTT"] == DBNull.Value ? 0 : dr["CTT"]));

                hdnGL.Value = Convert.ToString((Convert.ToInt32(dr["GL"] == DBNull.Value ? 0 : dr["GL"])));
                hdnFIT.Value = Convert.ToString(Convert.ToInt32(dr["FIT"] == DBNull.Value ? 0 : dr["FIT"]));
                hdnFICA.Value = Convert.ToString(Convert.ToInt32(dr["FICA"] == DBNull.Value ? 0 : dr["FICA"]));
                hdnMEDI.Value = Convert.ToString(Convert.ToInt32(dr["MEDI"] == DBNull.Value ? 0 : dr["MEDI"]));
                hdnFUTA.Value = Convert.ToString(Convert.ToInt32(dr["FUTA"] == DBNull.Value ? 0 : dr["FUTA"]));
                hdnSIT.Value = Convert.ToString(Convert.ToInt32(dr["SIT"] == DBNull.Value ? 0 : dr["SIT"]));
                hdnVac.Value = Convert.ToString(Convert.ToInt32(dr["Vac"] == DBNull.Value ? 0 : dr["Vac"]));
                hdnWc.Value = Convert.ToString(Convert.ToInt32(dr["Wc"] == DBNull.Value ? 0 : dr["Wc"]));
                hdnUni.Value = Convert.ToString(Convert.ToInt32(dr["Uni"] == DBNull.Value ? 0 : dr["Uni"]));
                hdnInUse.Value = "0";
                hdnSick.Value = Convert.ToString(Convert.ToInt32(dr["Sick"] == DBNull.Value ? 0 : dr["Sick"]));
                hdnStatus.Value = Convert.ToString(Convert.ToInt32(dr["Status"] == DBNull.Value ? 0 : dr["Status"]));
                hdnYTD.Value = "0.00";
                hdnYTDH.Value = "0.00";
                hdnOYTD.Value = "0.00";
                hdnOYTDH.Value = "0.00";
                hdnDYTD.Value = "0.00";
                hdnDYTDH.Value = "0.00";
                hdnTYTD.Value = "0.00";
                hdnTYTDH.Value = "0.00";
                hdnNYTD.Value = "0.00";
                hdnNYTDH.Value = "0.00";
                hdnVacR.Value = "";
                hdnfdesc.Value = Convert.ToString(dr["fdesc"] == DBNull.Value ? 0 : dr["fdesc"]);
                hdnGLName.Value = Convert.ToString(dr["GLName"] == DBNull.Value ? 0 : dr["GLName"]);
            }
            else
            {
                txtReg.Text = string.Format("{0:n}", 0);
                txtOt.Text = string.Format("{0:n}", 0);
                txtNt.Text = string.Format("{0:n}", 0);
                txtDt.Text = string.Format("{0:n}", 0);
                txtTt.Text = string.Format("{0:n}", 0);
                txtCReg.Text = string.Format("{0:n}", 0);
                txtCOt.Text = string.Format("{0:n}", 0);
                txtCNt.Text = string.Format("{0:n}", 0);
                txtCDt.Text = string.Format("{0:n}", 0);
                txtCTt.Text = string.Format("{0:n}", 0);

                hdnGL.Value = "0";
                hdnFIT.Value = "0";
                hdnFICA.Value = "0";
                hdnMEDI.Value = "0";
                hdnFUTA.Value = "0";
                hdnSIT.Value = "0";
                hdnVac.Value = "0";
                hdnWc.Value = "0";
                hdnUni.Value = "0";
                hdnInUse.Value = "0";
                hdnSick.Value = "0";
                hdnStatus.Value = "0";
                hdnYTD.Value = string.Format("{0:n}", 0);
                hdnYTDH.Value = string.Format("{0:n}", 0);
                hdnOYTD.Value = string.Format("{0:n}", 0);
                hdnOYTDH.Value = string.Format("{0:n}", 0);
                hdnDYTD.Value = string.Format("{0:n}", 0);
                hdnDYTDH.Value = string.Format("{0:n}", 0);
                hdnTYTD.Value = string.Format("{0:n}", 0);
                hdnTYTDH.Value = string.Format("{0:n}", 0);
                hdnNYTD.Value = string.Format("{0:n}", 0);
                hdnNYTDH.Value = string.Format("{0:n}", 0);
                hdnVacR.Value = "";
                hdnfdesc.Value = "";
                hdnGLName.Value = "";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private bool IsValidWageRateGrid()
    {
        try
        {
            DataTable dtWage = GetWageGridItems();
            var lst = dtWage.Rows.Cast<DataRow>().Where(r => (int)r.ItemArray[0] == 0).ToList();
            dtWage.Rows.Cast<DataRow>().Where(r => (int)r["Wage"] == 0).ToList().ForEach(r => r.Delete());
            dtWage.AcceptChanges();
            ViewState["WageItems"] = dtWage;
            DataView dv = new DataView(dtWage);
            DataTable distDt = dv.ToTable(true, "Wage");
            if (distDt.Rows.Count != dtWage.Rows.Count)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "keySuperAlert", "noty({text: 'Selected wage categories must be unique!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                return false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return true;
    }

    private bool IsWageRateIsUsed(Int32 userID, Int32 WageID, out string TicketID)
    {
        // used Wage category can't delete if the Job Cost Labor = Burden Rate

        string wagesRage = ViewState["JobCostLabor"].ToString();

        TicketID = "";
        if (userID == 0 || WageID == 0 || wagesRage != "1") return false;
        DataSet IsWageRateIsUsed = new DataSet();
        _objWage.ConnConfig = HttpContext.Current.Session["config"].ToString();
        _objWage.ID = WageID;
        IsWageRateIsUsed = objBL_User.IsWageRateIsUsed(_objWage, userID);

        if (IsWageRateIsUsed.Tables[0].Rows.Count > 0)
        {
            TicketID = IsWageRateIsUsed.Tables[0].Rows[0]["ID"].ToString();
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void imgBtnWageAdd_Click(object sender, EventArgs e)
    {
        AddNewRow();
    }
    private void AddNewRow()
    {
        //DataTable dt = new DataTable();
        //dt = (DataTable)ViewState["WageItems"];
        //DataTable dt1 = new DataTable();
        //dt.Columns.Add("Wage", typeof(int));
        //dt.Columns.Add("Reg", typeof(double));
        //dt.Columns.Add("OT", typeof(double));
        //dt.Columns.Add("DT", typeof(double));
        //dt.Columns.Add("TT", typeof(double));
        //dt.Columns.Add("NT", typeof(double));
        //dt.Columns.Add("CReg", typeof(double));
        //dt.Columns.Add("COT", typeof(double));
        //dt.Columns.Add("CDT", typeof(double));
        //dt.Columns.Add("CTT", typeof(double));
        //dt.Columns.Add("CNT", typeof(double));
        DataTable dt = GetWageGridItems();
        DataRow dr = dt.NewRow();
        dr["Wage"] = 0;
        dr["Reg"] = 0;
        dr["OT"] = 0;
        dr["DT"] = 0;
        dr["TT"] = 0;
        dr["NT"] = 0;
        dr["CReg"] = 0;
        dr["COT"] = 0;
        dr["CDT"] = 0;
        dr["CTT"] = 0;
        dr["CNT"] = 0;

        dr["GL"] = 0;
        dr["FIT"] = 0;
        dr["FICA"] = 0;
        dr["MEDI"] = 0;
        dr["FUTA"] = 0;
        dr["SIT"] = 0;
        dr["Vac"] = 0;
        dr["Wc"] = 0;
        dr["Uni"] = 0;
        dr["InUse"] = 0;
        dr["Sick"] = 0;
        dr["Status"] = 0;
        dr["YTD"] = 0;
        dr["YTDH"] = 0;
        dr["OYTD"] = 0;
        dr["OYTDH"] = 0;
        dr["DYTD"] = 0;
        dr["DYTDH"] = 0;
        dr["TYTD"] = 0;
        dr["TYTDH"] = 0;
        dr["NYTD"] = 0;
        dr["NYTDH"] = 0;
        dr["VacR"] = "";
        dr["fdesc"] = "";
        dr["GLName"] = "";
        dt.Rows.Add(dr);
        //dt1 = (DataTable)ViewState["WageItems"];
        gvWagePayRate.DataSource = dt;
        //gvWagePayRate.DataBind();
        gvWagePayRate.Rebind();
    }
    private void GetControlData()
    {
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getControl(objProp_User);

        // used Wages category can't delete if the Job Cost Labor = Burden Rate

        string wagesRage = ds.Tables[0].Rows[0]["JobCostLabor"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["JobCostLabor"].ToString();
        ViewState["JobCostLabor"] = wagesRage;
    }











    private void CreateWageDedcutionTable()
    {
        
        DataTable dt = new DataTable();
        dt.Columns.Add("Ded", typeof(int));
        dt.Columns.Add("BasedOn", typeof(int));
        dt.Columns.Add("AccruedOn", typeof(int));
        dt.Columns.Add("ByW", typeof(int));
        dt.Columns.Add("EmpRate", typeof(double));
        dt.Columns.Add("EmpTop", typeof(double));
        dt.Columns.Add("EmpGL", typeof(int));
        dt.Columns.Add("EmpGLName", typeof(string));
        dt.Columns.Add("CompRate", typeof(double));
        dt.Columns.Add("CompTop", typeof(double));
        dt.Columns.Add("CompGL", typeof(int));
        dt.Columns.Add("CompGLName", typeof(string));
        dt.Columns.Add("CompGLE", typeof(int));
        dt.Columns.Add("CompGLEName", typeof(string));
        dt.Columns.Add("InUse", typeof(int));
        dt.Columns.Add("YTD", typeof(double));
        dt.Columns.Add("YTDC", typeof(double));
        dt.Columns.Add("fdesc", typeof(string));
        dt.Columns.Add("Checked", typeof(bool));

        DataRow dr = dt.NewRow();
        dr["Ded"] = 0;
        dr["BasedOn"] = 0;
        dr["AccruedOn"] = 0;
        dr["ByW"] = 0;
        dr["EmpRate"] = 0.00;
        dr["EmpTop"] = 0.00;
        dr["EmpGL"] = 0;
        dr["EmpGLName"] = "";
        dr["CompRate"] = 0.00;
        dr["CompTop"] = 0.00;
        dr["CompGL"] = 0;
        dr["CompGLE"] = 0;
        dr["CompGLName"] = "";
        dr["CompGLEName"] = "";
        dr["InUse"] = 0;
        dr["YTD"] = 0.00;
        dr["YTDC"] = 0.00;
        dr["fdesc"] = "";
        dr["Checked"] = false;
        dt.Rows.Add(dr);
        ViewState["WageDeductionItems"] = dt;
        RadGridWageDeduction.DataSource = dt;


    }
    private void FillWageDeductionCategorys()
    {
        try
        {
            DataSet ds = new DataSet();
            _objPRDed.ConnConfig = HttpContext.Current.Session["config"].ToString();
            ds = objBL_Wage.GetWageDeduction(_objPRDed);
            if (ds.Tables.Count > 0)
            {
                DataRow dr = ds.Tables[0].NewRow();
                dr["ID"] = 0;
                dr["Ded"] = 0;
                dr["fDesc"] = "Select Deduction";
                ds.Tables[0].Rows.InsertAt(dr, 0);
                dtWageDeduction = ds.Tables[0];
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void RadGridWageDeduction_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        
        CreateWageDedcutionTable();

        if (Request.QueryString["id"] != null)
        {
            DataSet dsWage = new DataSet();
            _objEmp.ConnConfig = Session["config"].ToString();
            _objEmp.ID = Convert.ToInt32(Request.QueryString["id"].ToString());
            dsWage = objBL_Wage.GetEmployeeListByID(_objEmp);

            //ViewState["WageDeductionItems"] = dsWage;

            if (dsWage.Tables[2].Rows.Count > 0)
            {
                RadGridWageDeduction.DataSource = dsWage.Tables[2];
                //gvWagePayRate.DataBind();
                ViewState["WageDeductionItems"] = dsWage.Tables[2];
            }
            else
            {
                CreateWageDedcutionTable();
            }

        }
    }
    protected void RadGridWageDeduction_ItemDataBound(object sender, GridItemEventArgs e)
    {
        string TicketID = string.Empty;

        if (e.Item is GridDataItem)
        {
            DropDownList ddlWageD = (DropDownList)e.Item.FindControl("ddlWageD");
            Int32 UserID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"]);

            if (IsWageRateIsUsed(UserID, Convert.ToInt32(ddlWageD.SelectedItem.Value), out TicketID))
            {
                string str = "Wage deduction is used in Ticket #" + TicketID + "!";

                ddlWageD.Attributes["onclick"] = "   noty({ text: '" + str + "', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";

                foreach (ListItem item in ddlWageD.Items)
                {
                    if (item.Value != ddlWageD.SelectedItem.Value) { item.Enabled = false; }
                }
            }


        }
    }
    protected void RadGridWageDeduction_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = RadGridWageDeduction.Items.Count - 1;
        GridDataItem row = RadGridWageDeduction.Items[rowIndex];
        //HiddenField hdnIndex = row.FindControl("hdnIndex") as HiddenField;

        DataTable dt = new DataTable();

        dt = GetWageDeductionGridItems();
        
        DataRow dr = dt.NewRow();
        dr["Ded"] = 0;
        dr["BasedOn"] = 0;
        dr["AccruedOn"] = 0;
        dr["ByW"] = 0;
        dr["EmpRate"] = 0.00;
        dr["EmpTop"] = 0.00;
        dr["EmpGL"] = 0;
        dr["EmpGLName"] = "";
        dr["CompRate"] = 0.00;
        dr["CompTop"] = 0.00;
        dr["CompGL"] = 0;
        dr["CompGLE"] = 0;
        dr["CompGLName"] = "";
        dr["CompGLEName"] = "";
        dr["InUse"] = 0;
        dr["YTD"] = 0.00;
        dr["YTDC"] = 0.00;
        dr["fdesc"] = "";
        dr["Checked"] = false;
        dt.Rows.Add(dr);

        RadGridWageDeduction.DataSource = dt;
        //gvWagePayRate.DataBind();

        ViewState["WageDeductionItems"] = dt;
    }
    private DataTable GetWageDeductionGridItems()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Ded", typeof(int));
        dt.Columns.Add("BasedOn", typeof(int));
        dt.Columns.Add("AccruedOn", typeof(int));
        dt.Columns.Add("ByW", typeof(int));
        dt.Columns.Add("EmpRate", typeof(double));
        dt.Columns.Add("EmpTop", typeof(double));
        dt.Columns.Add("EmpGL", typeof(int));
        dt.Columns.Add("EmpGLName", typeof(string));
        dt.Columns.Add("CompRate", typeof(double));
        dt.Columns.Add("CompTop", typeof(double));
        dt.Columns.Add("CompGL", typeof(int));
        dt.Columns.Add("CompGLName", typeof(string));
        dt.Columns.Add("CompGLE", typeof(int));
        dt.Columns.Add("CompGLEName", typeof(string));
        dt.Columns.Add("InUse", typeof(int));
        dt.Columns.Add("YTD", typeof(double));
        dt.Columns.Add("YTDC", typeof(double));
        dt.Columns.Add("fdesc", typeof(string));
        dt.Columns.Add("Checked", typeof(bool));

        try
        {
            string strItems = hdnWageDRate.Value.Trim();

            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objWageDedcutionItemData = new List<Dictionary<object, object>>();
                objWageDedcutionItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                objWageDedcutionItemData.RemoveAt(0);
                //objWageDedcutionItemData.RemoveAt(0);
                foreach (Dictionary<object, object> dict in objWageDedcutionItemData)
                {
                    i++;
                    DataRow dr = dt.NewRow();
                    if (dict["ddlWageD"].ToString() != "0")
                    {
                        dr["Ded"] = Convert.ToInt32(dict["ddlWageD"]);
                    }
                    else
                    {
                        dr["Ded"] = 0;
                    }
                    if (dict["ddlBasedOnD"].ToString() != "0")
                    {
                        dr["BasedOn"] = Convert.ToInt32(dict["ddlBasedOnD"]);
                    }
                    else
                    {
                        dr["BasedOn"] = 0;
                    }
                    if (dict["ddlAccuredOnD"].ToString() != "0")
                    {
                        dr["AccruedOn"] = Convert.ToInt32(dict["ddlAccuredOnD"]);
                    }
                    else
                    {
                        dr["AccruedOn"] = 0;
                    }


                    if (!string.IsNullOrEmpty(dict["txtEmpRateD"].ToString()))
                    {
                        dr["EmpRate"] = Convert.ToDouble(dict["txtEmpRateD"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtEmpMaxRateD"].ToString()))
                    {
                        dr["EmpTop"] = Convert.ToDouble(dict["txtEmpMaxRateD"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtCompanyRateD"].ToString()))
                    {
                        dr["CompRate"] = Convert.ToDouble(dict["txtCompanyRateD"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtCompanyMaxRateD"].ToString()))
                    {
                        dr["CompTop"] = Convert.ToDouble(dict["txtCompanyMaxRateD"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdndByW"].ToString()))
                    {
                        dr["ByW"] = Convert.ToInt32(dict["hdndByW"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdndEmpGL"].ToString()))
                    {
                        dr["EmpGL"] = Convert.ToInt32(dict["hdndEmpGL"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdndEmpGLName"].ToString()))
                    {
                        dr["EmpGLName"] = Convert.ToString(dict["hdndEmpGLName"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdndCompGL"].ToString()))
                    {
                        dr["CompGL"] = Convert.ToInt32(dict["hdndCompGL"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdndCompGLName"].ToString()))
                    {
                        dr["CompGLName"] = Convert.ToString(dict["hdndCompGLName"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdndCompGLE"].ToString()))
                    {
                        dr["CompGLE"] = Convert.ToInt32(dict["hdndCompGLE"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdndCompGLEName"].ToString()))
                    {
                        dr["CompGLEName"] = Convert.ToString(dict["hdndCompGLEName"]);
                    }
                    
                    if (!string.IsNullOrEmpty(dict["hdndInUse"].ToString()))
                    {
                        dr["InUse"] = Convert.ToInt32(dict["hdndInUse"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdndYTD"].ToString()))
                    {
                        dr["YTD"] = Convert.ToDouble(dict["hdndYTD"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdndYTDC"].ToString()))
                    {
                        dr["YTDC"] = Convert.ToDouble(dict["hdndYTDC"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdndfdesc"].ToString()))
                    {
                        dr["fdesc"] = Convert.ToString(dict["hdndfdesc"]);
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(dict["chkSelect"].ToString()) && dict["chkSelect"].ToString() == "on")
                        {
                            dr["Checked"] = true;
                        }
                        else
                        {
                            dr["Checked"] = false;
                        }
                    }
                    catch (Exception)
                    {
                        dr["Checked"] = false;
                    }

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
    protected void imgBtnWageDDelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string TicketID = string.Empty;
            DataTable dt = GetWageDeductionGridItems();
            if (dt.Rows.Count > 1)
            {
                foreach (GridDataItem gr in RadGridWageDeduction.Items)
                {
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                    if (chkSelect.Checked == true)
                    {
                        DropDownList ddlWageD = (DropDownList)gr.FindControl("ddlWageD");
                        Int32 UserID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"]);
                        if (!IsWageRateIsUsed(UserID, Convert.ToInt32(ddlWageD.SelectedItem.Value), out TicketID))
                        {
                            DataRow dr = dt.Select("Wage = " + Convert.ToInt32(ddlWageD.SelectedValue)).FirstOrDefault();
                            if (dr != null)
                                dt.Rows.Remove(dr);
                        }
                        else
                        {
                            string str = "Wage deduction is used in Ticket #" + TicketID + "!";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "key112", "noty({text: '" + str + "',dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                            return;
                        }
                    }
                }
            }
            else if (dt.Rows.Count > 0)
            {
                foreach (GridDataItem gr in RadGridWageDeduction.Items)
                {
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                    if (chkSelect.Checked == true)
                    {
                        DropDownList ddlWageD = (DropDownList)gr.FindControl("ddlWageD");
                        Int32 UserID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"]);
                        if (!IsWageRateIsUsed(UserID, Convert.ToInt32(ddlWageD.SelectedItem.Value), out TicketID))
                        {
                            if (ddlWageD.SelectedValue != "0")
                            {
                                DataRow dr = dt.Select("Wage = " + Convert.ToInt32(ddlWageD.SelectedValue)).FirstOrDefault();
                                if (dr != null)
                                    dt.Rows.Remove(dr);

                                dr = dt.NewRow();
                                dr["Ded"] = 0;
                                dr["BasedOn"] = 0;
                                dr["AccruedOn"] = 0;
                                dr["ByW"] = 0;
                                dr["EmpRate"] = 0.00;
                                dr["EmpTop"] = 0.00;
                                dr["EmpGL"] = 0;
                                dr["EmpGLName"] = "";
                                dr["CompRate"] = 0.00;
                                dr["CompTop"] = 0.00;
                                dr["CompGL"] = 0;
                                dr["CompGLE"] = 0;
                                dr["CompGLName"] = "";
                                dr["CompGLEName"] = "";
                                dr["InUse"] = 0;
                                dr["YTD"] = 0;
                                dr["YTDC"] = 0;
                                dr["fdesc"] = "";
                                dt.Rows.Add(dr);
                                
                            }
                        }
                        else
                        {
                            string str = "Wage deduction is used in Ticket #" + TicketID + "!";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "key1512", "noty({text: '" + str + "',dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                            return;
                        }

                    }
                }
            }
            ViewState["WageDeductionItems"] = dt;
            RadGridWageDeduction.DataSource = dt;
            //gvWagePayRate.DataBind();
            RadGridWageDeduction.Rebind();

            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "RemoveWageGridRow", "RemoveWageGridRow('" + gvWagePayRate.ClientID + "')", true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlWageD_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlWageD = (DropDownList)sender;
            GridDataItem row = (GridDataItem)ddlWageD.NamingContainer;
            TextBox txtEmpRateD = (TextBox)row.FindControl("txtEmpRateD");
            TextBox txtEmpMaxRateD = (TextBox)row.FindControl("txtEmpMaxRateD");
            TextBox txtCompanyRateD = (TextBox)row.FindControl("txtCompanyRateD");
            TextBox txtCompanyMaxRateD = (TextBox)row.FindControl("txtCompanyMaxRateD");
            DropDownList ddlBasedOnD = (DropDownList)row.FindControl("ddlBasedOnD");
            DropDownList ddlAccuredOnD = (DropDownList)row.FindControl("ddlAccuredOnD");

            HiddenField hdndCompGLE = (HiddenField)row.FindControl("hdndCompGLE");
            HiddenField hdndByW = (HiddenField)row.FindControl("hdndByW");
            HiddenField hdndEmpGL = (HiddenField)row.FindControl("hdndEmpGL");
            HiddenField hdndEmpGLName = (HiddenField)row.FindControl("hdndEmpGLName");
            HiddenField hdndCompGL = (HiddenField)row.FindControl("hdndCompGL");
            HiddenField hdndCompGLName = (HiddenField)row.FindControl("hdndCompGLName");

            HiddenField hdndCompGLEName = (HiddenField)row.FindControl("hdndCompGLEName");
            HiddenField hdndInUse = (HiddenField)row.FindControl("hdndInUse");
            HiddenField hdndYTD = (HiddenField)row.FindControl("hdndYTD");
            HiddenField hdndYTDC = (HiddenField)row.FindControl("hdndYTDC");
            HiddenField hdndfdesc = (HiddenField)row.FindControl("hdndfdesc");

            _objPRDed.ConnConfig = Session["config"].ToString();
            _objPRDed.ID = Convert.ToInt32(ddlWageD.SelectedValue);
            DataSet ds = objBL_Wage.GetWageDeductionByID(_objPRDed);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtEmpRateD.Text = string.Format("{0:n}", Convert.ToDouble(dr["EmpRate"] == DBNull.Value ? 0 : dr["EmpRate"]));
                txtEmpMaxRateD.Text = string.Format("{0:n}", Convert.ToDouble(dr["EmpTop"] == DBNull.Value ? 0 : dr["EmpTop"]));
                txtCompanyRateD.Text = string.Format("{0:n}", Convert.ToDouble(dr["CompRate"] == DBNull.Value ? 0 : dr["CompRate"]));
                txtCompanyMaxRateD.Text = string.Format("{0:n}", Convert.ToDouble(dr["CompTop"] == DBNull.Value ? 0 : dr["CompTop"]));
                hdndCompGLE.Value = Convert.ToString(Convert.ToInt32(dr["CompGLE"] == DBNull.Value ? 0 : dr["CompGLE"]));
                hdndByW.Value = Convert.ToString(Convert.ToInt32(dr["ByW"] == DBNull.Value ? 0 : dr["ByW"]));
                hdndEmpGL.Value = Convert.ToString(Convert.ToInt32(dr["EmpGL"] == DBNull.Value ? 0 : dr["EmpGL"]));
                hdndEmpGLName.Value = Convert.ToString(dr["EmpGLAcct"].ToString());
                hdndCompGL.Value = Convert.ToString(Convert.ToInt32(dr["CompGL"] == DBNull.Value ? 0 : dr["CompGL"]));
                hdndCompGLName.Value = Convert.ToString(dr["CompGLAcct"].ToString());
                hdndCompGLEName.Value = Convert.ToString(dr["CompGLEAcct"].ToString());
                hdndInUse.Value = Convert.ToString(Convert.ToInt32(dr["InUse"] == DBNull.Value ? 0 : dr["InUse"]));
                hdndYTD.Value = "0";
                hdndYTDC.Value = "0";
                hdndfdesc.Value = Convert.ToString(dr["fdesc"].ToString());
                ddlBasedOnD.SelectedValue = Convert.ToString(Convert.ToInt32(dr["BasedOn"] == DBNull.Value ? 0 : dr["BasedOn"]));
                ddlAccuredOnD.SelectedValue = Convert.ToString(Convert.ToInt32(dr["AccruedOn"] == DBNull.Value ? 0 : dr["AccruedOn"]));
            }
            else
            {
                txtEmpRateD.Text = string.Format("{0:n}", 0);
                txtEmpMaxRateD.Text = string.Format("{0:n}", 0);
                txtCompanyRateD.Text = string.Format("{0:n}", 0);
                txtCompanyMaxRateD.Text = string.Format("{0:n}", 0);
                hdndCompGLE.Value = "0";
                hdndByW.Value = "0";
                hdndEmpGL.Value = "0";
                hdndEmpGLName.Value = "";
                hdndCompGL.Value = "0";
                hdndCompGLName.Value = "";

                hdndCompGLEName.Value = "";
                hdndInUse.Value = "0";
                hdndYTD.Value = "0";
                hdndYTDC.Value = "0";
                hdndfdesc.Value = "";
                ddlBasedOnD.SelectedValue = "0";
                ddlAccuredOnD.SelectedValue = "0";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private bool IsValidWageDedcutionRateGrid()
    {
        try
        {
            DataTable dtWage = GetWageDeductionGridItems();
            var lst = dtWage.Rows.Cast<DataRow>().Where(r => (int)r.ItemArray[0] == 0).ToList();
            dtWage.Rows.Cast<DataRow>().Where(r => (int)r["Ded"] == 0).ToList().ForEach(r => r.Delete());
            dtWage.AcceptChanges();
            ViewState["WageDeductionItems"] = dtWage;
            DataView dv = new DataView(dtWage);
            DataTable distDt = dv.ToTable(true, "Ded");
            if (distDt.Rows.Count != dtWage.Rows.Count)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "keySuperAlert", "noty({text: 'Selected wage categories must be unique!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                return false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return true;
    }

    private bool IsWageDeductionRateIsUsed(Int32 userID, Int32 WageID, out string TicketID)
    {
        // used Wage category can't delete if the Job Cost Labor = Burden Rate

        string wagesRage = ViewState["JobCostLabor"].ToString();

        TicketID = "";
        if (userID == 0 || WageID == 0 || wagesRage != "1") return false;
        DataSet IsWageRateIsUsed = new DataSet();
        _objWage.ConnConfig = HttpContext.Current.Session["config"].ToString();
        _objWage.ID = WageID;
        IsWageRateIsUsed = objBL_User.IsWageRateIsUsed(_objWage, userID);

        if (IsWageRateIsUsed.Tables[0].Rows.Count > 0)
        {
            TicketID = IsWageRateIsUsed.Tables[0].Rows[0]["ID"].ToString();
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void imgBtnWageDAdd_Click(object sender, EventArgs e)
    {
        AddNewRowWageDeduction();
    }
    private void AddNewRowWageDeduction()
    {
        
        DataTable dt = GetWageDeductionGridItems();
        DataRow dr = dt.NewRow();
        dr["Ded"] = 0;
        dr["BasedOn"] = 0;
        dr["AccruedOn"] = 0;
        dr["ByW"] = 0;
        dr["EmpRate"] = 0.00;
        dr["EmpTop"] = 0.00;
        dr["EmpGL"] = 0;
        dr["EmpGLName"] = "";
        dr["CompRate"] = 0.00;
        dr["CompTop"] = 0.00;
        dr["CompGL"] = 0;
        dr["CompGLE"] = 0;
        dr["CompGLName"] = "";
        dr["CompGLEName"] = "";
        dr["InUse"] = 0;
        dr["YTD"] = 0.00;
        dr["YTDC"] = 0.00;
        dr["fdesc"] = "";
        dt.Rows.Add(dr);
        //dt1 = (DataTable)ViewState["WageItems"];
        RadGridWageDeduction.DataSource = dt;
        //gvWagePayRate.DataBind();
        RadGridWageDeduction.Rebind();
    }
    protected void btnCopyRateDed_Click(object sender, EventArgs e)
    {
        try
        {
            string TicketID = string.Empty;
            DataTable dt = GetWageDeductionGridItems();


            if (dt.Rows.Count > 1)
            {
                DataRow dr = dt.Select("Checked = true").FirstOrDefault();
                foreach (DataRow row in dt.Rows)
                {
                    row["Ded"] = dr["Ded"] ;
                    row["BasedOn"] = dr["BasedOn"] ;
                    row["AccruedOn"] =dr["AccruedOn"];
                    row["ByW"] =dr["ByW"] ;
                    row["EmpRate"] =dr["EmpRate"];
                    row["EmpTop"] =dr["EmpTop"] ;
                    row["EmpGL"] =dr["EmpGL"] ;
                    row["EmpGLName"] =dr["EmpGLName"] ;
                    row["CompRate"] =dr["CompRate"] ;
                    row["CompTop"] =dr["CompTop"] ;
                    row["CompGL"] =dr["CompGL"] ;
                    row["CompGLE"] =dr["CompGLE"] ;
                    row["CompGLName"] =dr["CompGLName"] ;
                    row["CompGLEName"] =dr["CompGLEName"] ;
                    row["InUse"] =dr["InUse"] ;
                    row["YTD"] =dr["YTD"] ;
                    row["YTDC"] =dr["YTDC"] ;
                    row["fdesc"] =dr["fdesc"] ;

                }
            }

            ViewState["WageDeductionItems"] = dt;
            RadGridWageDeduction.DataSource = dt;
            ////gvWagePayRate.DataBind();
            RadGridWageDeduction.Rebind();

            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "RemoveWageGridRow", "RemoveWageGridRow('" + gvWagePayRate.ClientID + "')", true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnCopyRate_Click(object sender, EventArgs e)
    {
        try
        {
            string TicketID = string.Empty;
            DataTable dt = GetWageGridItems();

            if (dt.Rows.Count > 1)
            {
                DataRow dr = dt.Select("Checked = true").FirstOrDefault();
                foreach (DataRow row in dt.Rows)
                {
                    row["Wage"]=dr["Wage"];
                    row["Reg"] = dr["Reg"];
                    row["OT"]=dr["OT"];
                    row["DT"] = dr["DT"];
                    row["TT"] = dr["TT"];
                    row["NT"] = dr["NT"];
                    row["CReg"] = dr["CReg"];
                    row["COT"] = dr["COT"];
                    row["CDT"] = dr["CDT"];
                    row["CTT"] = dr["CTT"];
                    row["CNT"] = dr["CNT"];
                    row["GL"] = dr["GL"];
                    row["FIT"] = dr["FIT"];
                    row["FICA"] = dr["FICA"];
                    row["MEDI"] = dr["MEDI"];
                    row["FUTA"] = dr["FUTA"];
                    row["SIT"] = dr["SIT"];
                    row["Vac"] = dr["Vac"];
                    row["Wc"] = dr["Wc"];
                    row["Uni"] = dr["Uni"];
                    row["InUse"] = dr["InUse"];
                    row["Sick"] = dr["Sick"];
                    row["Status"] = dr["Status"];
                    row["YTD"] = dr["YTD"];
                    row["YTDH"] = dr["YTDH"];
                    row["OYTD"] = dr["OYTD"];
                    row["OYTDH"] = dr["OYTDH"];
                    row["DYTD"] = dr["DYTD"];
                    row["DYTDH"] = dr["DYTDH"];
                    row["TYTD"] = dr["TYTD"];
                    row["TYTDH"] = dr["TYTDH"];
                    row["NYTD"] = dr["NYTD"];
                    row["NYTDH"] = dr["NYTDH"];
                    row["VacR"] = dr["VacR"];
                    row["fdesc"] = dr["fdesc"];
                    row["GLName"] = dr["GLName"];

                }
            }

            ViewState["WageItems"] = dt;
            gvWagePayRate.DataSource = dt;
            ////gvWagePayRate.DataBind();
            gvWagePayRate.Rebind();
            
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "RemoveWageGridRow", "RemoveWageGridRow('" + gvWagePayRate.ClientID + "')", true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}

 