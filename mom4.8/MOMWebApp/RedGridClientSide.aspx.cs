using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLayer;
using BusinessEntity;
using System.Collections;
using System.Text;
using Microsoft.ApplicationBlocks.Data;
using System.Globalization;
using MOMWebApp;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

namespace MOMWebApp
{
    public partial class RedGridClientSide : System.Web.UI.Page
    {
        #region "Variables"
        BL_User objBL_User = new BL_User();
        BusinessEntity.User objProp_User = new BusinessEntity.User();
        BL_Wage objBL_Wage = new BL_Wage();
        Wage objProp_Wage = new Wage();
        public static bool check = false;
        public static bool IsAddEdit = false;
        public static bool IsDelete = false;
        BL_BankAccount _objBLBank = new BL_BankAccount();
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
               
                Permission();
                HighlightSideMenu("prID", "wagecategorylink", "payrollmenutab");
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

                hdnAddDedcutions.Value = VendorsPermission.Length < 1 ? "Y" : VendorsPermission.Substring(0, 1);
                hdnEditDedcutions.Value = VendorsPermission.Length < 2 ? "Y" : VendorsPermission.Substring(1, 1);
                hdnDeleteDedcutions.Value = VendorsPermission.Length < 3 ? "Y" : VendorsPermission.Substring(2, 1);
                hdnViewDedcutions.Value = VendorsPermission.Length < 4 ? "Y" : VendorsPermission.Substring(3, 1);
                if (hdnAddDedcutions.Value == "N")
                {
                    lnkAddnew.Visible = false;
                }
                if (hdnEditDedcutions.Value == "N")
                {
                    btnEdit.Visible = false;
                }
                if (hdnDeleteDedcutions.Value == "N")
                {
                    lnkDelete.Visible = false;
                }
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
                //RadGrid_Wage.Columns[9].Visible = true;
            }
            else
            {
                //RadGrid_Wage.Columns[9].Visible = false;
                Session["CmpChkDefault"] = "2";
            }
        }
        protected void lnkAddnew_Click(object sender, EventArgs e)
        {
            Response.Redirect("WageCategory.aspx");
        }
        protected void btnCopy_Click(object sender, EventArgs e)
        {
            foreach (GridDataItem item in RadGrid_Wage.SelectedItems)
            {
                Label lblDeductID = (Label)item.FindControl("lblId");
                Response.Redirect("WageCategory.aspx?id=" + lblDeductID.Text + "&t=c");
            }

        }
        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            IsDelete = false;
            try
            {
                foreach (GridDataItem di in RadGrid_Wage.SelectedItems)
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
                        //GetWageDeductionList();
                        RadGrid_Wage.Rebind();
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
    }
}