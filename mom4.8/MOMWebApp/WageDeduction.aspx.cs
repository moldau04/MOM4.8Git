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

public partial class WageDeduction : System.Web.UI.Page
{

    BL_User objBL_User = new BL_User();
    BL_Wage objBL_Wage = new BL_Wage();
    User objProp_User = new User();
    PRDed objProp_PRDed = new PRDed();
    BL_Company objBL_Company = new BL_Company();
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
    bool api = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!CheckAddEditPermission()) { Response.Redirect("Home.aspx?permission=no"); return; }

        if (!IsPostBack)
        {

            HighlightSideMenu("prID", "wagedeductionslink", "payrollmenutab");
        }


        try
        {
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }
            string _connectionString = Session["config"].ToString();

            if (!IsPostBack)
            {
                userpermissions();
                CompanyPermission();
                FillVertexDeduction();
                ViewState["editcon"] = 0;
                ViewState["mode"] = 0;

                //FillCountry();
                if (Request.QueryString["uid"] == null)
                {
                    Page.Title = "Add Wage Deduction || MOM";
                    ViewState["edit"] = "0";


                }
                if (Request.QueryString["id"] != null)  //Edit COA
                {
                    Page.Title = "Edit Wage Deduction || MOM";

                    //_objvendor.ConnConfig = _connectionString;
                    //SetDataForEdit();

                    if (Request.QueryString["t"] != null)
                    {
                        ViewState["mode"] = 0;
                        lblHeader.Text = "Add Wage Deduction";
                        ViewState["edit"] = "0";
                        GetEditData(Request.QueryString["id"].ToString());
                        hdnWageDeductionID.Value = "";
                    }
                    else
                    {
                        ViewState["mode"] = 1;
                        lblHeader.Text = "Edit Wage Deduction";
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
                objProp_User.PageName = "WageDeduction.aspx";
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
            dt = (DataTable)Session["WageDeductionList"];
            string url = "WageDeduction.aspx?id=" + dt.Rows[0]["ID"];
            Response.Redirect(url);
        }
        catch (Exception ex)
        {

        }
    }
    private void GetEditData(string id)
    {
        try
        {

            DataSet ds = new DataSet();
            objProp_PRDed.ConnConfig = Session["config"].ToString();
            objProp_PRDed.ID = Convert.ToInt32(id);
            ds = new BL_Wage().GetWageDeductionByID(objProp_PRDed);

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtDeductionDesc.Text = ds.Tables[0].Rows[0]["fDesc"].ToString();
                ddlDeductionType.SelectedValue = ds.Tables[0].Rows[0]["Type"].ToString();
                ddlDeductionPaidBy.SelectedValue = ds.Tables[0].Rows[0]["ByW"].ToString();
                ddlDeductionBasedon.SelectedValue = ds.Tables[0].Rows[0]["BasedOn"].ToString();
                ddlDeductionAccuredon.SelectedValue = ds.Tables[0].Rows[0]["AccruedOn"].ToString();
                txtDeductionEmpRate.Text = ds.Tables[0].Rows[0]["EmpRate"].ToString();
                txtDeductionEmpCeiling.Text = ds.Tables[0].Rows[0]["EmpTop"].ToString();
                hdnEmpGLAcct.Value = ds.Tables[0].Rows[0]["EmpGL"].ToString();
                txtDeductionCompnayRate.Text = ds.Tables[0].Rows[0]["CompRate"].ToString();
                txtDeductionCompnayCeiling.Text = ds.Tables[0].Rows[0]["CompTop"].ToString();
                hdnComapnyGLAcct.Value = ds.Tables[0].Rows[0]["CompGL"].ToString();
                hdnCompanyExpGLAcct.Value = ds.Tables[0].Rows[0]["CompGLE"].ToString();

                ddlDeductionPaid.SelectedValue = ds.Tables[0].Rows[0]["Paid"].ToString();
                hdntxtVendor.Value = ds.Tables[0].Rows[0]["Vendor"].ToString();
                txtdeductionremark.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                if (ds.Tables[0].Rows[0]["VertexDeductionId"].ToString() != null && ds.Tables[0].Rows[0]["VertexDeductionId"].ToString() != "")
                {
                    ddlDeduction401.SelectedValue = ds.Tables[0].Rows[0]["VertexDeductionId"].ToString();
                }

                //ddlDeductionJobSp.SelectedValue = ds.Tables[0].Rows[0]["Job"].ToString();
                ddlDeductionW2Box.SelectedValue = ds.Tables[0].Rows[0]["Box"].ToString();
                //ddlDeductionFrequency.SelectedValue = ds.Tables[0].Rows[0]["Frequency"].ToString();

                if (ds.Tables[0].Rows[0]["Process"].ToString() == "True")
                {
                    chkProcessDeduction.Checked = true;
                }
                else
                {
                    chkProcessDeduction.Checked = false;
                }
                hdnWageDeductionID.Value = ds.Tables[0].Rows[0]["ID"].ToString();

                txtEmpGLAcct.Text = ds.Tables[0].Rows[0]["EmpGLAcct"].ToString();
                txtComapnyGLAcct.Text = ds.Tables[0].Rows[0]["CompGLAcct"].ToString();
                txtCompanyExpGLAcct.Text = ds.Tables[0].Rows[0]["CompGLEAcct"].ToString();
                txtVendor.Text = ds.Tables[0].Rows[0]["VendorName"].ToString();

            }




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
    private void ClearControl()
    {
        txtDeductionDesc.Text = "";
        txtDeductionEmpRate.Text = "0.00";
        txtDeductionEmpCeiling.Text = "0.00";
        hdnEmpGLAcct.Value = "";
        txtDeductionCompnayRate.Text = "0.00";
        txtDeductionCompnayCeiling.Text = "0.00";
        hdnComapnyGLAcct.Value = "";
        hdnCompanyExpGLAcct.Value = "";
        hdntxtVendor.Value = "";
        txtdeductionremark.Text = "";
        hdnWageDeductionID.Value = "";
        ViewState["edit"] = "0";
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //Submit();


        try
        {
            objProp_PRDed.ConnConfig = Session["config"].ToString();
            objProp_PRDed.fDesc = txtDeductionDesc.Text;
            objProp_PRDed.Type = Convert.ToInt32(ddlDeductionType.SelectedValue);
            objProp_PRDed.ByW = Convert.ToInt32(ddlDeductionPaidBy.SelectedValue);
            objProp_PRDed.BasedOn = Convert.ToInt32(ddlDeductionBasedon.SelectedValue);
            objProp_PRDed.AccruedOn = Convert.ToInt32(ddlDeductionAccuredon.SelectedValue);
            objProp_PRDed.Count = 0;
            if (txtDeductionEmpRate.Text != "")
            {
                objProp_PRDed.EmpRate = Convert.ToDouble(txtDeductionEmpRate.Text);
            }
            else
            {
                objProp_PRDed.EmpRate = 0;
            }
            if (txtDeductionEmpCeiling.Text != "")
            {
                objProp_PRDed.EmpTop = Convert.ToDouble(txtDeductionEmpCeiling.Text);
            }
            else
            {
                objProp_PRDed.EmpTop = 0;
            }
            if (hdnEmpGLAcct.Value != "")
            {
                objProp_PRDed.EmpGL = Convert.ToInt32(hdnEmpGLAcct.Value);
            }
            else
            {
                objProp_PRDed.EmpGL = 0;
            }
            if (txtDeductionCompnayRate.Text != "")
            {
                objProp_PRDed.CompRate = Convert.ToDouble(txtDeductionCompnayRate.Text);
            }
            else
            {
                objProp_PRDed.CompRate = 0;
            }
            if (txtDeductionCompnayCeiling.Text != "")
            {
                objProp_PRDed.CompTop = Convert.ToDouble(txtDeductionCompnayCeiling.Text);
            }
            else
            {
                objProp_PRDed.CompTop = 0;
            }
            if (hdnComapnyGLAcct.Value != "")
            {
                objProp_PRDed.CompGL = Convert.ToDouble(hdnComapnyGLAcct.Value);
            }
            else
            {
                objProp_PRDed.CompGL = 0;
            }
            if (hdnCompanyExpGLAcct.Value != "")
            {
                objProp_PRDed.CompGLE = Convert.ToInt32(hdnCompanyExpGLAcct.Value);
            }
            else
            {
                objProp_PRDed.CompGLE = 0;
            }

            objProp_PRDed.Paid = Convert.ToInt32(ddlDeductionPaid.SelectedValue);
            if (hdntxtVendor.Value != "")
            {
                objProp_PRDed.Vendor = Convert.ToInt32(hdntxtVendor.Value);
            }
            else
            {
                objProp_PRDed.Vendor = 0;
            }
            objProp_PRDed.Balance = 0;
            objProp_PRDed.InUse = 0;
            objProp_PRDed.Remarks = txtdeductionremark.Text;
            objProp_PRDed.DedType = 0;
            objProp_PRDed.Reimb = 0;
            //objProp_PRDed.Job = Convert.ToInt32(ddlDeductionJobSp.SelectedValue);
            objProp_PRDed.Box = Convert.ToInt32(ddlDeductionW2Box.SelectedValue);
            //objProp_PRDed.Frequency = Convert.ToInt32(ddlDeductionFrequency.SelectedValue);
            objProp_PRDed.VertexDeductionId = Convert.ToInt32(ddlDeduction401.SelectedValue);

            if (chkProcessDeduction.Checked.Equals(true))
            {
                objProp_PRDed.Process = 1;
            }
            else
            {
                objProp_PRDed.Process = 0;
            }
            string msg = "Added";
            if (ViewState["edit"].ToString() == "0")
            {
                objProp_PRDed.ID = 0;
                objBL_Wage.AddWageDeduction(objProp_PRDed);
            }
            else if (ViewState["edit"].ToString() == "1")
            {
                msg = "Updated";
                objProp_PRDed.ID = Convert.ToInt32(hdnWageDeductionID.Value);
                objBL_Wage.AddWageDeduction(objProp_PRDed);
            }

            //this.programmaticModalPopup.Hide();

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddDeparmenttype", "noty({text: 'Wage " + msg + " Successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            Response.Redirect("WageDeductList.aspx");


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
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("WageDeductList.aspx?WageDeduction=Y");
    }
    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["WageDeductionList"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;
            if (index < c)
            {
                string url = "WageDeduction.aspx?id=" + dt.Rows[index - 1]["ID"];
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
            dt = (DataTable)Session["WageDeductionList"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;
            if (index < c)
            {
                string url = "WageDeduction.aspx?id=" + dt.Rows[index + 1]["ID"];
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
            dt = (DataTable)Session["WageDeductionList"];
            string url = "WageDeduction.aspx?id=" + dt.Rows[dt.Rows.Count - 1]["ID"];
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
    private void FillVertexDeduction()
    {
        DataSet ds = new DataSet();
        objProp_PRDed.ConnConfig = Session["config"].ToString();
        ds = objBL_Wage.GetVertexDeductionList(objProp_PRDed);

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlDeduction401.DataSource = ds.Tables[0];
            ddlDeduction401.DataTextField = "DeductionName";
            ddlDeduction401.DataValueField = "VertexDeductionId";
            ddlDeduction401.DataBind();
        }
        else
        {
            ddlDeduction401.Items.Add(new System.Web.UI.WebControls.ListItem("No data found", "0"));
        }
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

}

