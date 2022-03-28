using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace MOMWebApp
{
    public partial class PayrollList : System.Web.UI.Page
    {
        #region "Variables"
        CD _objCD = new CD();
        BL_User objBL_User = new BL_User();
        User objProp_User = new User();
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
        General _objPropGeneral = new General();
        BL_General _objBLGeneral = new BL_General();
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
               
                #region Show Selected Filter
                //if (Convert.ToString(Request.QueryString["f"]) != "c")
                //{
                   
                //}
                //else
                //{
                //    Session["ddlSearch_Vendor"] = null;
                //    Session["ddlSearch_Value_Vendor"] = null;
                //}
                #endregion

                Permission();
                HighlightSideMenu("prID", "runpayrolllink", "payrollmenutab");
            }
            if (!IsPostBack)
            {
               
            }
            CompanyPermission();

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
                hdnAddPayroll.Value = VendorsPermission.Length < 1 ? "Y" : VendorsPermission.Substring(0, 1);
                hdnEditPayroll.Value = VendorsPermission.Length < 2 ? "Y" : VendorsPermission.Substring(1, 1);
                hdnDeletePayroll.Value = VendorsPermission.Length < 3 ? "Y" : VendorsPermission.Substring(2, 1);
                hdnViewPayroll.Value = VendorsPermission.Length < 4 ? "Y" : VendorsPermission.Substring(3, 1);
                if (hdnAddPayroll.Value == "N")
                {
                    //lnksubmit.Visible = false;
                }
                if (hdnViewPayroll.Value == "N")
                {
                    Response.Redirect("Home.aspx?permission=no"); return;
                }
            }
            else
            {
                hdnAddPayroll.Value = "Y";
                hdnEditPayroll.Value = "Y";
                hdnDeletePayroll.Value = "Y";
                hdnViewPayroll.Value = "Y";
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

    }
}