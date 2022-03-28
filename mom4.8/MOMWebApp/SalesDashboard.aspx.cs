using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class SalesDashboard : System.Web.UI.Page
{
    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            FillDashboard();
            
        }
        Permission();
    }
    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("SalesMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("SalesLink");
       // a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkCRM");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("SalesMgrSub");
        //ul.Attributes.Remove("class");
        ul.Style.Add("display", "block");

        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
        }
       
        //if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        //{
        //    Response.Redirect("home.aspx");
        //}

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
    private void FillDashboard()
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Customer.getSalesDashboard(objProp_Customer);
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[1].Rows.Count > 0)
            {
                string gross = ds.Tables[0].Rows[0]["GrossInc"].ToString();
                string CurrentValue = ds.Tables[1].Rows[0]["Revenue"].ToString();
                string strScript = "createGauge(" + gross + "," + CurrentValue + ");";                
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keygauge", strScript, true);
            }
        }
        //if (ds.Tables[2].Rows.Count > 0)
        //{
            gvOpenOpportunity.DataSource = ds.Tables[2];
            gvOpenOpportunity.DataBind();
        //}
        //if (ds.Tables[3].Rows.Count > 0)
        //{
            gvSalesLead.DataSource = ds.Tables[3];
            gvSalesLead.DataBind();
        //}
        //if (ds.Tables[4].Rows.Count > 0)
        //{
            gvAccounts.DataSource = ds.Tables[4];
            gvAccounts.DataBind();
        //}

        lblLast.Text = "As of Today at   <B>" + System.DateTime.Now.ToShortTimeString()+"</B>";
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        FillDashboard();        
    }
    protected void ibtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        FillDashboard();
    }
}
