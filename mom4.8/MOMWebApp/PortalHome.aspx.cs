using System;
using System.Data;
using System.Web;
using System.Web.UI;

public partial class PortalHome : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //tblSales.Visible = false;

        //string url_path_current = HttpContext.Current.Request.Url.ToString();
        //if (url_path_current.StartsWith("https:") == true)
        //{
        //    HttpContext.Current.Response.Redirect("http" + url_path_current.Remove(0, 5), false);
        //} 

        if (Session["userid"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        if (Session["MSM"].ToString() == "TS")
        {
            //tblSales.Visible = false;
        }

        #region SSL Implementation
        string SSL = System.Web.Configuration.WebConfigurationManager.AppSettings["SSL"].Trim();
        if (SSL == "1")
        {
            bool isLocal = HttpContext.Current.Request.IsLocal;
            if (!isLocal)
            {
                bool isSecure = HttpContext.Current.Request.IsSecureConnection;
                string webPath = System.Web.Configuration.WebConfigurationManager.AppSettings["webPath"].Trim();
                string HTTP_Port = System.Web.Configuration.WebConfigurationManager.AppSettings["HTTP_Port"].Trim();
                if (!isSecure)
                {
                    if (Session["type"].ToString() == "c")
                    {

                        bool port = HttpContext.Current.Request.Url.IsDefaultPort;
                        string Auth = HttpContext.Current.Request.Url.Authority;
                        if (!port)
                        {
                            Auth = HttpContext.Current.Request.Url.DnsSafeHost;
                        }
                        string URL = Auth + webPath;

                        string redirect = "HTTPS://" + URL + "/home.aspx";
                        int ii = 0;
                        foreach (String key in Request.QueryString.AllKeys)
                        {
                            if (ii == 0)
                                redirect += "?" + key + "=" + Request.QueryString[key];
                            else
                                redirect += "&" + key + "=" + Request.QueryString[key];
                            ii++;
                        }
                        Response.Redirect(redirect);
                    }
                }
                else
                {
                    if (Session["type"].ToString() != "c")
                    {
                        string Auth = HttpContext.Current.Request.Url.DnsSafeHost;
                        if (HTTP_Port != string.Empty)
                            Auth += ":" + HTTP_Port;

                        string URL = Auth + webPath;
                        string redirect = "HTTP://" + URL + "/home.aspx";
                        int ii = 0;
                        foreach (String key in Request.QueryString.AllKeys)
                        {
                            if (ii == 0)
                                redirect += "?" + key + "=" + Request.QueryString[key];
                            else
                                redirect += "&" + key + "=" + Request.QueryString[key];
                            ii++;
                        }
                        Response.Redirect(redirect);
                    }
                }
            }
        }
        #endregion

        if (Session["type"].ToString() != "am")
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["userinfo"];

            string ProgFunc = dt.Rows[0]["Control"].ToString().Substring(0, 1);
            string usertype = dt.Rows[0]["usertype"].ToString();
            string AccessUser = dt.Rows[0]["users"].ToString().Substring(0, 1);
            string Sales = dt.Rows[0]["sales"].ToString().Substring(0, 1);

            if (Sales == "Y")
            {
                divProspects.Visible = true;
            }
            else
            {
                divProspects.Visible = false;
            }

            if (usertype == "c")
            {
                tblCustomer.Visible = false;
                tblRecurring.Visible = false;
                tblSchedule.Visible = true;
                tblBilling.Visible = true;
                tblProgram.Visible = false;
                tblSales.Visible = false;

                divBillCodes.Visible = false;
                //divPaymentH.Visible = true;
                divScheduler.Visible = false;
                divMapLink.Visible = false;
                divRouteBuilder.Visible = false;
                divTimesheet.Visible = false;
                tblProject.Visible = false;

                if (Session["ticketo"].ToString() == "1")
                {
                    tblSchedule.Visible = true;
                }
                else
                {
                    tblSchedule.Visible = false;
                }

                if (Session["invoice"].ToString() == "1")
                {
                    tblBilling.Visible = true;
                }
                else
                {
                    tblBilling.Visible = false;
                }

                if (Session["CPE"].ToString() == "1")
                {
                    tblCustomer.Visible = true;
                    divCustomers.Visible = false;
                    divLocations.Visible = false;
                }
            }
            if (ProgFunc == "Y")
            {
                tblProgram.Visible = true;
                if (AccessUser != "Y")
                {
                    lnkUsers.HRef = "";
                    ClientScript.RegisterStartupScript(Page.GetType(), "keyuser", " $(" + lnkUsers.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access users!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true});  });", true);

                }
            }
            else
            {
                tblProgram.Visible = false;
            }




            if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
            {
                tblBilling.Visible = false;
                //tblCustomer.Visible = false;
                tblRecurring.Visible = false;
                divControl.Visible = false;
                divCustom.Visible = false;
                divCustomers.Visible = false;
                divLocations.Visible = false;
            }
        }

        if (!IsPostBack)
        {
            if (Session["MSM"].ToString() == "TS")
            {
                ////////GetSP();
                if (Session["type"].ToString() != "c")
                {
                    ////tblBilling.Visible = false;
                    //divInvoices.Visible = false;
                }
                tblRecurring.Visible = false;
                //tblCustomer.Visible = false;                
                divControl.Visible = false;
                divCustom.Visible = false;
                divCustomers.Visible = false;
                divLocations.Visible = false;
                divBillCodes.Visible = false;
                divTimesheet.Visible = false;
                tblProject.Visible = false;

                if (Convert.ToInt16(Session["MSREP"]) != 1)
                {
                    tblCustomer.Visible = false;
                }

                if (Convert.ToInt16(Session["payment"]) != 1)
                {
                    if (Session["type"].ToString() != "c")
                    {
                        // tblBilling.Visible = false;
                    }
                    else
                    {
                        divPaymentH.Visible = false;
                    }
                }
            }
            if (Convert.ToInt16(Session["payment"]) != 1)
            {
                divPaymentH.Visible = false;
                //hlnkPayhist.NavigateUrl = "";
                //lnkPayhist.Attributes.Add("style", "display:none");
            }
            //UpdateLocationCoordinates();
        }
    }
}