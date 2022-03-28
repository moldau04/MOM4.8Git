using System;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        Session["UserName"] = ddlUsers.SelectedItem.Text;
        Session["UserId"] = ddlUsers.SelectedValue;
        Response.Redirect("~/StartChat.aspx");
    }
}