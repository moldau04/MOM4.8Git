using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BusinessLayer;
using BusinessEntity;

public partial class testquery : System.Web.UI.Page
{
    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (Session["type"].ToString() != "am")
        {
            Response.Redirect("login.aspx");
        }
    }
    protected void btnExecute_Click(object sender, EventArgs e)
    {
        if (txtUser.Text.Trim().Equals("admin", StringComparison.CurrentCultureIgnoreCase) && txtPass.Text.Trim().Equals("ess@2012!quer", StringComparison.CurrentCultureIgnoreCase))
        {
            objGeneral.ConnConfig = Session["config"].ToString();
            objGeneral.TextQuery = txtQuery.Text;

            DataSet ds = new DataSet();
            ds = objBL_General.ExecQuery(objGeneral);

            gvResults.DataSource = ds.Tables[0];
            gvResults.DataBind();
        }        
    }
}
