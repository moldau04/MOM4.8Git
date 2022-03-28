using System;
using System.Web.UI.WebControls;

public partial class uc_CustomerSearch : System.Web.UI.UserControl
{
    public TextBox _txtCustomer
    {
        get { return txtCustomer; }
    }
    public HiddenField _hdnCustID
    {
        get { return hdnCustID; }
    }
    public CustomValidator _CustomValidator2
    {
        get { return CustomValidator2; }
    }

    
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
}
