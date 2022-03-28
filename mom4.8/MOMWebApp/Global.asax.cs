using Stimulsoft.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace MOMWebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Code that runs on application startup       
            Stimulsoft.Base.StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHlgHN5p4wFaNqaDgehHwPI3o2yZv8P5AgvHzgWQKWodhZsOug" +
            "YiZkrdLmU/XqA+/UPfNtByhqBpYDCZm2EbsThXjQo40gEU0m917ZQzsJtK7AmfdxbOBZekTJy+tmOpqEwJo0oU7KhG" +
            "5nbAJRF3Bs7EUXHmXUHVgBQrxck4F8gW5WgnAKszvQsAvqterBx6rX50nUB+f/CVvWAXC8TUNpxHYa5yEofvIjwcfi" +
            "07Kb6e8NWpWmy+9CvgF0q5C1ywoq0GEgewrIHnRrI/ri/LRJCEgfLh+xPNynFpV3wL6EAikEN5pOqO3lgBtO8lZ+F3" +
            "ipjXUzD83APniT86vodkEq5I6sEl77wT5mrfQa8Xe2UHsMs2GONyid+QkaMmCAgEet+Fpy2Q/hYwsAeHx2LwLM8xP9" +
            "vZiPovsQql55C6wHgXBLWVn7tdrVyBnxz9uNu84HSH6ipr+IFHn35qjtgpya/7Jak3uOVMtA/zm9uFxQEja9zlHbaq" +
            "mfuhd/8RFlmMHv61brrXkwl51xVJ6jTBDheJQGhlIyvKEbb+WZ11xzWRVw==";

            StiOptions.Export.Html.UseImageResolution = true;
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started       
        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.        
        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
            //// Bug fix for MS SSRS Blank.gif 500 server error missing parameter IterationId
            //// https://connect.microsoft.com/VisualStudio/feedback/details/556989/
            //if (HttpContext.Current.Request.Url.PathAndQuery.StartsWith("/Reserved.ReportViewerWebControl.axd") &&
            //!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["ResourceStreamID"]) &&
            //HttpContext.Current.Request.QueryString["ResourceStreamID"].ToLower().Equals("blank.gif"))
            //{
            //    Context.RewritePath(String.Concat(HttpContext.Current.Request.Url.PathAndQuery, "&IterationId=0"));
            //}        
        }
        private const string _WebApiPrefix = "api";
        private static string _WebApiExecutionPath = String.Format("~/{0}", _WebApiPrefix);
        private static bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(_WebApiExecutionPath);
        }
        protected void Application_PostAuthorizeRequest()
        {

            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }

        }
    }
}