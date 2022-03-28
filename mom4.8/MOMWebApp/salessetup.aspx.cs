using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using BusinessEntity;
using BusinessLayer;

public partial class salessetup : System.Web.UI.Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

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

            DataSet ds = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            ds = objBL_User.getControl(objPropUser);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtAmountSalesp.Text = ds.Tables[0].Rows[0]["SalesAnnual"].ToString();
                ddlMonth.SelectedValue = ds.Tables[0].Rows[0]["Month"].ToString();
                txtGrossAmount.Text = ds.Tables[0].Rows[0]["GrossInc"].ToString();
            }           
        }
       // Permission();
        UserPermission();
    }
    private void Permission()
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl("SalesMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        HyperLink a = (HyperLink)Page.Master.FindControl("SalesLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkSalesSetup");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;

        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl("SalesMgrSub");
        //ul.Attributes.Remove("class");
        div.Style.Add("display", "block");

        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
        }
        
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
    private void UserPermission()
    {
        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
        }
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

            string salessetupPermission = ds.Rows[0]["salessetup"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["salessetup"].ToString();

            string View = salessetupPermission.Length < 4 ? "Y" : salessetupPermission.Substring(3, 1);
            // string Report = salessetupPermission.Length < 6 ? "Y" : salessetupPermission.Substring(5, 1);

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
    protected void lnkCancelContact_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }
    protected void lnkContactSave_Click(object sender, EventArgs e)
    {
        if (Convert.ToDouble(txtGrossAmount.Text.Trim()) == 0)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'Value can not be zero.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return;
        }
        try
        {
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.AnnualAmount = Convert.ToDouble(txtGrossAmount.Text.Trim());
            objPropUser.Month = Convert.ToInt16(ddlMonth.SelectedValue);
            objPropUser.SalesAmount = Convert.ToDouble(txtAmountSalesp.Text.Trim());

            objBL_User.UpdateAnnualAmount(objPropUser);
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Sales setup updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
}
