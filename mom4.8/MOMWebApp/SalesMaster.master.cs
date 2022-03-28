using System;
using System.Web.UI.WebControls;
using System.Data;
using BusinessEntity;
using BusinessLayer;

public partial class SalesMaster : System.Web.UI.MasterPage
{
    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillRecentProspect();
        }
    }

    public void FillRecentProspect()
    {
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();

        ds = objBL_Customer.getRecentProspect(objProp_Customer);
        gvRecent.DataSource = ds.Tables[0];
        gvRecent.DataBind();
    }

    public void FillPendingRec(int rol)
    {
        pnlPendingrec.Visible = true;
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.SearchBy = "l.fuser";
        objProp_Customer.SearchValue = string.Empty;
        objProp_Customer.StartDate = string.Empty;
        objProp_Customer.EndDate = string.Empty;
        objProp_Customer.ROL = rol;
        ViewState["rol"] = rol;
        // TODO: need to replace this code but will do it later
        // ds = objBL_Customer.getOpportunity(objProp_Customer); ==> ds = objBL_Customer.getOpportunityNew(objProp_Customer);
        ds = objBL_Customer.getOpportunity(objProp_Customer);
        gvPendingRecommendations.DataSource = ds.Tables[0];
        gvPendingRecommendations.DataBind();
    }

    public void FillPendingLeads()
    {
        pnlPendingLeads.Visible = true;
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.SearchBy = "p.terr";
        objProp_Customer.SearchValue = "0";

        ds = objBL_Customer.getProspect(objProp_Customer);
        gvPendingLeads.DataSource = ds.Tables[0];
        gvPendingLeads.DataBind();
    }

    protected void lnkRecent_Click(object sender, EventArgs e)
    {
        LinkButton lnkRecent = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)lnkRecent.NamingContainer;
        Label lblListType = (Label)gvr.FindControl("lblListType");
        Label lblID = (Label)gvr.FindControl("lblID");

        if (lblListType.Text == "0")
        {
            Response.Redirect("addprospect.aspx?uid=" + lblID.Text);
        }
    }

    public string RecentListIcons(string listtype)
    {
        string imagePath = string.Empty;
        if (listtype == "0")
        {
            imagePath = "images/leader_s.png";
        }
        else if (listtype == "1")
        {
            imagePath = "images/tasks_s.png";
        }
        else if (listtype == "2")
        {
            imagePath = "images/Opportunities_s.png";
        }
        return imagePath;
    }

    public string RecentListURL(string listtype, string uid)
    {
        string URL = string.Empty;
        if (listtype == "0")
        {
            URL = "addprospect.aspx?uid=" + uid;
        }
        else if (listtype == "1")
        {
            URL = "addtask.aspx?uid=" + uid;
        }
        else if (listtype == "2")
        {
            URL = "addopprt.aspx?uid=" + uid;
        }
        return URL;
    }
    protected void gvPendingRecommendations_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPendingRecommendations.PageIndex = e.NewPageIndex;
        FillPendingRec(Convert.ToInt32(ViewState["rol"]));
    }
    protected void gvPendingLeads_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPendingLeads.PageIndex = e.NewPageIndex;
        FillPendingLeads();
    }
}
