using BusinessEntity;
using BusinessLayer;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class ManualTimeCard : System.Web.UI.Page
{
    BL_TimeCard objTimeCard = new BL_TimeCard();
    BL_User objBL_User = new BL_User();
    protected void Page_Load(object sender, EventArgs e)
    {
        string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

        if (Request.Url.Scheme == "http" && SSL == "1")
        {
            string URL = Request.Url.ToString();
            URL = URL.Replace("http://", "https://");
            Response.Redirect(URL);
        }
        HighlightSideMenu("schMgr", "HyperLink1", "schdMgrSub");

        Permission();
    }


    private void HighlightSideMenu(string MenuParent, string PageLink, string SubMenuDiv)
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl(MenuParent);    
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl"; 
        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl(PageLink);         
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");        
        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl(SubMenuDiv);
        div.Style.Add("display", "block");
    }
    private void Permission()
    {

        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = GetUserById();

            /// Ticket ///////////////////------->
            string MTimesheetPermission = ds.Rows[0]["MTimesheet"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["MTimesheet"].ToString();
            string view = MTimesheetPermission.Length < 4 ? "Y" : MTimesheetPermission.Substring(3, 1);
            if (view == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }
        }
    }
    private DataTable GetUserById()
    {
        User objPropUser = new User();
        objPropUser.TypeID = Convert.ToInt32(Session["usertypeid"]);
        objPropUser.UserID = Convert.ToInt32(Session["userid"]);
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.DBName = Session["dbname"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_User.GetUserPermissionByUserID(objPropUser);
        return ds.Tables[0];
    }
    [WebMethodAttribute()]
    public static int SaveCardInput(TimeInput obj)
    {
        return 0;
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    public DataSet GetInputCard(TimeCard timeCard)
    {
        DataSet ds = new DataSet();
        try
        {
            ds = SqlHelper.ExecuteDataset(timeCard.ConnConfig, CommandType.StoredProcedure, "spGetTimeCardInput");
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return ds;
    }
}

public class TimeInput
{
    public int date { get; set; }
    public int start { get; set; }
    public int reg { get; set; }
    public int ot { get; set; }
    public int nt { get; set; }
    public int dt { get; set; }
    public int travel { get; set; }
    public int miles { get; set; }
    public int project { get; set; }
    public int phase { get; set; }
    public int zone { get; set; }
    public int reimb { get; set; }
    public int wage { get; set; }
    public int unit { get; set; }
    public int group { get; set; }
    public int wo { get; set; }

}