using BusinessEntity;
using BusinessLayer;
using MSWeb;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

public partial class CustomerDashboard : System.Web.UI.Page
{
    #region Variables

    static Contracts objContract = new Contracts();
    static BL_Contracts objBLContracts = new BL_Contracts();
    public SQLNotifier Notifier { get; set; }
    static BL_User objBL_User = new BL_User();
    static BusinessEntity.User objPropUser = new BusinessEntity.User();

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {
            objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
            Notifier = new SQLNotifier(objPropUser.ConnConfig);
            Notifier.NewMessage += new EventHandler<SqlNotificationEventArgs>(notifier_NewMessage);
            DataTable dt = Notifier.RegisterDependency("Select [Type],[Price] From dbo.Elev");
        }
    }

    void notifier_NewMessage(object sender, SqlNotificationEventArgs e)
    {
        DBUpdationNotificationHub.Send("");
        Notifier = new SQLNotifier(objPropUser.ConnConfig);
        Notifier.NewMessage += new EventHandler<SqlNotificationEventArgs>(notifier_NewMessage);
        DataTable dt = Notifier.RegisterDependency("Select [Type],[Price] From dbo.Elev");
    }

    static DateTime GetFullDate(object dt)
    {
        var dtString = dt.ToString();
        var year = int.Parse(dtString.Substring(0, 4));
        var month = int.Parse(dtString.Substring(4, 2));
        return new DateTime(year, month, 1);
    }

    #region Web methods
   
    private void GetARAgingReport()
    {
        try
        {
           
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
   
     
    #endregion
}


