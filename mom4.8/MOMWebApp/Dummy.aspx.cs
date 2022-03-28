using System;
using System.Web.UI;

public partial class Dummy : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["id"] != null)
            {
                Session["NewJE"] = Request.QueryString["id"];
                Response.Redirect("AddJournalEntry.aspx?id=" + (Request.QueryString["id"]));
            }
        }
    }
}