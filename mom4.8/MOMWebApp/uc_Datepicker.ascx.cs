using AjaxControlToolkit;
using System;
using System.Web.UI.WebControls;

public partial class uc_Datepicker : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public TextBox _txtDate
    {
        get { return txtDate; }
    }
    public CalendarExtender _ceDate
    {
        get { return txtDate_CalendarExtender; }
    }
    protected override void OnInit(EventArgs e)
    {
        //base.OnInit(e);
        base.OnPreRender(e);
        ToolkitScriptManager ts = new ToolkitScriptManager();
        ts.RegisterExtenderControl(txtDate_CalendarExtender, txtDate);
    }
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }
}