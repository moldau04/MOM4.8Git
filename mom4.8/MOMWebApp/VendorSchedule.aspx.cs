using System;

public partial class VendorSchedule : System.Web.UI.Page
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
            Session["GanttPlannerRefID"] = null;
            Session["GanttPlannerID"] = null;
            Session["GanttPlannerType"] = null;

            if (Request.QueryString["venid"] != null) Session["GanttPlannerRefID"] = Request.QueryString["venid"].ToString();
            if (Request.QueryString["plnid"] != null)
            {
                Session["GanttPlannerID"] = Request.QueryString["plnid"].ToString();
                Session["GanttPlannerType"] = "Vendor";
            }
        }
    }
}

