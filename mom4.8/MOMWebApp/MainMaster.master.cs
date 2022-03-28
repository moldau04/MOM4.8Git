using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessEntity;
using BusinessLayer;

public partial class MainMaster : System.Web.UI.MasterPage
{
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();

    General objGeneral = new General();
    BL_General objBL_General = new BL_General();
    Journal _objJe = new Journal();
    BL_GLARecur _objBLRecurr = new BL_GLARecur();
    protected void Page_Load(object sender, EventArgs e)
    {
        //salesMgr.Visible = false;
        //financeMgr.Visible = false;
        if (Session["MSM"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (Session["MSM"].ToString() == "TS")
        {
            //salesMgr.Visible = false;
            lnkEstimate.Visible = false;
            lnkEstimateTempl.Visible = false;
            ProjectMgr.Visible = false;
        }

        if (!IsPostBack)
        {
            #region Process recurring notifications
            DataSet _dsRecurrCount = new DataSet();
            _objJe.ConnConfig = Session["config"].ToString();
            _dsRecurrCount = _objBLRecurr.GetProcessRecurrCount(_objJe);
            if (_dsRecurrCount != null)
            {
                int _recurCount = Convert.ToInt32(_dsRecurrCount.Tables[0].Rows[0]["CountRecur"]);
                btnNotifyRecur.Text = _recurCount.ToString();
            }
            #endregion

            if (Session["MSM"].ToString() == "ADMIN")
            {
                Menu.Visible = false;
                lblUser.Text = "Welcome: Administrator";
                lblCompany.Text = "You are logged in to : Administrator Database";
            }
            else
            {
                userpermissions();
            }

            if (Session["user"] != null)
            {
                Qblastsync();

                lblUser.Text = "Welcome: " + Session["username"].ToString();
                lblCompany.Text = "Company : " + Session["company"].ToString();

                if (Session["type"].ToString() != "am")
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["userinfo"];
                    string role = dt.Rows[0]["role"].ToString();
                    string ProgFunc = dt.Rows[0]["Control"].ToString().Substring(0, 1);
                    string usertype = dt.Rows[0]["usertype"].ToString();
                    string AccessUser = dt.Rows[0]["users"].ToString().Substring(0, 1);
                    string Sales = dt.Rows[0]["sales"].ToString().Substring(0, 1);

                    if (role != string.Empty)
                        lblUser.Text += "/" + role;

                    if (Sales == "Y")
                    {
                        lnkProspect.Visible = true;
                    }
                    else
                    {
                        lnkProspect.Visible = false;
                    }

                    if (usertype == "c")
                    {
                        schMgr.Visible = true;
                        //cstmMgr.Visible = false;
                        cntractsMgr.Visible = false;
                        acctMgr.Visible = true;
                        salesMgr.Visible = false;
                        progMgr.Visible = false;

                        lnkBillcodeSMenu.Visible = false;
                        lnkScheduleMenu.Visible = false;
                        lnkMapMenu.Visible = false;
                        //lnkPaymentHistory.Visible = true;
                        lnkRouteBuilder.Visible = false;
                        lnkProspect.Visible = false;
                        divQBContents.Visible = false;
                        lnkTimesheet.Visible = false;

                        if (Session["ticketo"].ToString() == "1")
                        {
                            schMgr.Visible = true;
                        }
                        else
                        {
                            schMgr.Visible = false;
                        }

                        if (Session["invoice"].ToString() == "1")
                        {
                            acctMgr.Visible = true;
                        }
                        else
                        {
                            acctMgr.Visible = false;
                        }

                        if (Session["CPE"].ToString() == "1")
                        {
                            cstmMgr.Visible = true;
                            lnkCustomersSmenu.Visible = false;
                            lnkLocationsSMenu.Visible = false;
                        }
                    }
                    if (ProgFunc == "Y")
                    {
                        progMgr.Visible = true;
                        if (AccessUser != "Y")
                        {
                            lnkUsersSMenu.NavigateUrl = "";
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster", " $(" + lnkUsersSMenu.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access users!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true});  });", true);
                        }
                    }
                    else
                    {
                        progMgr.Visible = false;
                    }



                    if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
                    {
                        //cstmMgr.Visible = false;
                        cntractsMgr.Visible = false;
                        acctMgr.Visible = false;
                        lblSuper.Visible = true;
                        //lnkSetup.Visible = false;
                        lnkCntrlPnl.Visible = false;
                        lnkCustomFields.Visible = false;
                        lnkCustomersSmenu.Visible = false;
                        lnkLocationsSMenu.Visible = false;
                    }
                }
                if (Session["MSM"].ToString() == "TS")
                {
                    lnkCntrlPnl.Visible = false;
                    cntractsMgr.Visible = false;
                    if (Session["type"].ToString() != "c")
                    {
                        ////acctMgr.Visible = false;
                        //lnkInvoicesSMenu.Visible = false;
                    }
                    //cstmMgr.Visible = false;
                    //lnkSetup.Visible = false;
                    lnkCustomFields.Visible = false;
                    lnkCustomersSmenu.Visible = false;
                    lnkLocationsSMenu.Visible = false;
                    lnkBillcodeSMenu.Visible = false;
                    lnkTimesheet.Visible = false;
                    if (Convert.ToInt16(Session["MSREP"]) != 1)
                    {
                        cstmMgr.Visible = false;
                    }
                    if (Convert.ToInt16(Session["payment"]) != 1)
                    {
                        if (Session["type"].ToString() != "c")
                        {
                            //acctMgr.Visible = false;
                        }
                        else
                        {
                            lnkPaymentHistory.Visible = false;
                        }
                    }
                   
                }

                //switch (Session["FinanceManager"].ToString())
                //{
                //    case "F": // Finance Manager case
                //        lnkFinanceMgr.Visible = true;
                //        lnkJournalEntry.Visible = true;
                //        lnkCOA.Visible = true;
                //        lnkRecurringAdjust.Visible = true;
                //        break;
                //    //GLAdj case
                //    default:
                //        lnkFinanceMgr.Visible = false;
                //        lnkJournalEntry.Visible = false;
                //        lnkCOA.Visible = false;
                //        lnkRecurringAdjust.Visible = false;
                //        break;
                //}
                bool _addFinance = (bool)Session["AddFinance"];
                bool _editFinance = (bool)Session["EditFinance"];
                bool _viewFinance = (bool)Session["ViewFinance"];
                if(Session["FinanceManager"].ToString() == "F")
                {
                    lnkFinanceMgr.Visible = true;
                    lnkJournalEntry.Visible = true;
                    lnkCOA.Visible = true;
                    //lnkRecurringAdjust.Visible = true;
                }
                else if (_addFinance.Equals(true) || _editFinance.Equals(true) || _viewFinance.Equals(true))
                {
                    lnkFinanceMgr.Visible = true;
                    lnkJournalEntry.Visible = true;
                    lnkCOA.Visible = true;
                    //lnkRecurringAdjust.Visible = true;   
                }
                else
                {
                    lnkFinanceMgr.Visible = false;
                    lnkJournalEntry.Visible = false;
                    lnkCOA.Visible = false;
                    //lnkRecurringAdjust.Visible = false;
                }
            }

        }
    }

    private void Qblastsync()
    {
        int visible = 0;
        if (Session["MSM"].ToString() != "TS")
        {
            if (Convert.ToInt32(Session["ISsupervisor"]) != 1)
            {
                objGeneral.ConnConfig = Session["config"].ToString();
                DataSet dsLastSync = objBL_General.getQBlatsync(objGeneral);
                string strLastSync = dsLastSync.Tables[0].Rows[0]["qblastsync"].ToString();
                int intintegration = Convert.ToInt32(dsLastSync.Tables[0].Rows[0]["qbintegration"]);
                if (intintegration == 1)
                {
                    if (!string.IsNullOrEmpty(strLastSync))
                    {
                        lblQblastSync.Text = strLastSync;
                        lnkLastsync.Text = "Quickbooks Last Sync : ";
                        visible = 1;
                    }
                }
                dsLastSync = objBL_General.getSagelatsync(objGeneral);
                strLastSync = dsLastSync.Tables[0].Rows[0]["Sagelastsync"].ToString();
                intintegration = Convert.ToInt32(dsLastSync.Tables[0].Rows[0]["sageintegration"]);
                if (intintegration == 1)
                {
                    if (!string.IsNullOrEmpty(strLastSync))
                    {
                        lblQblastSync.Text = strLastSync;
                        lnkLastsync.Text = "Sage Last Sync : ";
                        visible = 1;
                    }
                }
            }
        }
        if (visible == 0)
        {
            divQBContents.Visible = false;
        }
    }

    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Response.Redirect("Login.aspx");
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("Adduser.aspx");
    }
    protected void lnkHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }
    protected void LinkButton3_Click(object sender, EventArgs e)
    {
        Response.Redirect("users.aspx");
    }
    protected void lnkQBLastSync_Click(object sender, EventArgs e)
    {
        Qblastsync();
    }

    private void userpermissions()
    {
        if (Session["type"].ToString() != "c")
        {
            if (Session["type"].ToString() != "am")
            {
                objProp_User.ConnConfig = Session["config"].ToString();
                objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
                DataSet dspage = objBL_User.getScreens(objProp_User);
                foreach (DataRow dr in dspage.Tables[0].Rows)
                {
                    if (Convert.ToBoolean(dr["access"]) == false)
                        DisableLinks(this.Page, "~/" + dr["url"].ToString());
                }
            }
        }
    }

    public void DisableLinks(Control parent, string URL)
    {
        foreach (Control c in parent.Controls)
        {
            if (c.Controls.Count > 0)
            {
                DisableLinks(c, URL);
            }
            else
            {
                switch (c.GetType().ToString())
                {
                    case "System.Web.UI.WebControls.HyperLink":
                        HyperLink h = (HyperLink)c;
                        if (URL.Equals(h.NavigateUrl, StringComparison.InvariantCultureIgnoreCase))
                        {
                            h.Enabled = false;
                            h.CssClass = "grayscales";
                        }
                        break;
                }
            }
        }
    }
    protected void btnNotifyRecur_Click(object sender, EventArgs e)
    {
        Response.Redirect("JournalEntry.aspx?r=1");
    }
}
