using System;
using System.Web;

public partial class AddPlanner : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
            return;
        }
        else
        {
            WebBaseUtility.UpdatePageTitle(this, "Planner", Request.QueryString["plnid"], "");
            Session["GanttPlannerRefID"] = null;
            Session["GanttPlannerID"] = null;
            Session["GanttPlannerType"] = null;

            if (Request.QueryString["projid"] != null)
            {
                Session["GanttPlannerRefID"] = Request.QueryString["projid"].ToString();
                var url = "<span style='float :left'>Project #</span><a style='float :left' href='addproject?uid=" + Request.QueryString["projid"].ToString() + "'>" + Request.QueryString["projid"].ToString() + "</a>";
                trProj.InnerHtml = url.ToString();
            }
            if (Request.QueryString["plnid"] != null)
            {
                Session["GanttPlannerID"] = Request.QueryString["plnid"].ToString();
                Session["GanttPlannerType"] = "Project";

                lblPlannerNo.Text = "Planner #" + Request.QueryString["plnid"];
            }
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["projid"] != null)
        {
            Response.Redirect("addproject.aspx?uid=" + Request.QueryString["projid"]);
        }
    }
}

