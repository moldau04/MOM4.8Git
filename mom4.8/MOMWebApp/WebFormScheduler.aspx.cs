using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

 
    public partial class WebFormScheduler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        if (!Page.IsPostBack)
        {
            string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();

            if (Request.Url.Scheme == "http" && SSL == "1")
            {
                string URL = Request.Url.ToString();
                URL = URL.Replace("http://", "https://");
                Response.Redirect(URL);
            }

            HighlightSideMenu("schMgr", "lnkScheduleMenu", "schdMgrSub");
        }
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
}
 