using System;
using System.Web.UI.WebControls;

public partial class uc_LocationSearch : System.Web.UI.UserControl
{
    public TextBox _txtLocation
    {
        get { return txtLocation; }
    }

    public HiddenField _hdnLocId
    {
        get { return hdnLocId; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
