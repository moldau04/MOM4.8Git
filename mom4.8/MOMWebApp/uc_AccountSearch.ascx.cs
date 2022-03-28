using System;
using System.Web.UI.WebControls;

public partial class uc_AccountSearch : System.Web.UI.UserControl
{
    public TextBox _txtGLAcct
    {
        get { return txtGLAcct; }
    }
    public HiddenField _hdnAcctID
    {
        get { return hdnAcctID; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}