using System;
using System.Web.UI;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Web;

public partial class Mom : System.Web.UI.MasterPage
{

    BL_User objBL_User = new BL_User();

    User objProp_User = new User();

    General objGeneral = new General();

    BL_General objBL_General = new BL_General();

    Journal _objJe = new Journal();

    BL_GLARecur _objBLRecurr = new BL_GLARecur();

    CompanyOffice objCompany = new CompanyOffice();

    BL_Company objBL_Company = new BL_Company();

    private const string ASCENDING = " ASC";

    private const string DESCENDING = " DESC";

    public int TodoTasks { get; set; }

    Customer objProp_Customer = new Customer();

    BL_Customer objBL_Customer = new BL_Customer();

    BL_ReportsData objBL_Report = new BL_ReportsData();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //HttpContextBase currentContext = new HttpContextWrapper(HttpContext.Current);
            var request = HttpContext.Current.Request.Url;
            objProp_User.Username = request.ToString();
            objProp_User.UserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            objProp_User.Name = HttpContext.Current.Session["username"].ToString();
            objProp_User.LastNAme = HttpContext.Current.Session["company"].ToString();
            objProp_User.DBName = HttpContext.Current.Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();
            objBL_User.AddUserLog(objProp_User);
            objProp_User = new User();
        }
        catch (Exception ex)
        {

        }

        if (!IsPostBack)
        {
            if (Session["MSM"] == null)
            {
                Response.Redirect("login.aspx");
            }
            // Hide Estimate, Estimate Template and Project manager in the menu
            if (Session["MSM"].ToString() == "TS")
            {
                lnkEstimate.Visible = false;
                lnkEstimateTempl.Visible = false;
                ProjectMgr.Visible = false;
            }

            if (!IsPostBack)
            {
                /** Login to Administrator database **/
                if (Session["MSM"].ToString() == "ADMIN")
                {
                    AdministratorPermissions();
                    GetGPSInfo();
                }
                else
                {
                    //Page Permission 
                    customerpermissions();
                    // userpermissions();
                    liChangePassword.Visible = false;
                    liGPSSetting.Visible = false;
                }


                if (Session["user"] != null)
                {

                    FinancialPermissions();
                    Qblastsync();
                    lblUser.Text = Session["username"].ToString();
                    lblCompany.Text = Session["company"].ToString();
                    NonAdminUSerPermissions();
                    TSDBPermissions();
                    ProgramManagerPermission();
                    SalesPersonPermission();
                    GetDefaultCompany();
                    GetControlForPayroll();
                    //FillCompany();
                }

                if (Session["userProfileImage"] != null)
                {
                    imgProfileImage.Src = Session["userProfileImage"].ToString();
                    imgProfileImageLg.Src = Session["userProfileImage"].ToString();
                }
                else
                {
                    try
                    {
                        var userinfo = (DataTable)Session["userinfo"];
                        int usertypeid = 0;
                        if (userinfo != null)
                        {
                            usertypeid = Convert.ToInt32(userinfo.Rows[0]["usertypeid"]);
                        }
                        objProp_User.ConnConfig = Session["config"].ToString();
                        objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
                        objProp_User.TypeID = usertypeid;
                        objProp_User.ConnConfig = Session["config"].ToString();
                        objProp_User.DBName = Session["dbname"].ToString();
                        DataSet ds = new DataSet();
                        ds = objBL_User.GetUserPermissionByUserID(objProp_User);
                        var profileImage = ds.Tables[0].Rows[0]["ProfileImage"].ToString();
                        imgProfileImage.Src = profileImage;
                        imgProfileImageLg.Src = profileImage;
                        Session["userProfileImage"] = profileImage;
                    }
                    catch (Exception)
                    {
                        // Do nothing
                    }

                }

                if (string.IsNullOrWhiteSpace(imgProfileImage.Src))
                {
                    imgProfileImage.Src = "Design/images/avatar.jpg";
                    imgProfileImageLg.Src = "Design/images/avatar.jpg";
                }
            }

            if (Session["user"] != null)
            {
                LoadDashboardMenu();
                
                // Need to check if customer portal then won't show
                if (Session["type"].ToString() == "c")
                {
                    todoNotify.Visible = false;
                    pnlSearch.Visible = false;
                }
                else
                {
                    // Updating todo tasks for the day
                    hdnItemJSON.Value = GetUserTodoTasksForToday();

                    if (TodoTasks == 0)
                    {
                        todoNotify.Visible = false;
                    }
                    else
                    {
                        todoNotify.Visible = true;
                        lblTodoNumber.Text = TodoTasks.ToString();
                    }
                }
            }
            else
            {
                todoNotify.Visible = false;
                pnlSearch.Visible = false;
            }
        }
    }

    private void AdministratorPermissions()
    {
        //hide menu
        lblUser.Text = "Administrator";
        lblCompany.Text = "Administrator";
        cntractsMgr.Visible = false;
        acctMgr.Visible = false;
        SalesMgr.Visible = false;
        progMgr.Visible = false;
        lnkBillcodeSMenu.Visible = false;
        lnkScheduleMenu.Visible = false;
        lnkMapMenu.Visible = false;
        lnkRouteBuilder.Visible = false;
        lnkProspect.Visible = false;
        lnkTimesheet.Visible = false;
        cstmMgr.Visible = false;
        schMgr.Visible = false;
        ProjectMgr.Visible = false;
        financeMgr.Visible = false;
        acctPayable.Visible = false;
        financialStatement.Visible = false;
        ReportMgr.Visible = false;
        InventoryMgr.Visible = false;
        liChangePassword.Visible = true;
        liGPSSetting.Visible = true;
        purchaseMgr.Visible = false;
        liSelectCompany.Visible = false;
    }

    //private DataTable GetUserById()
    //{
    //    User objPropUser = new User();
    //    objPropUser.TypeID = Convert.ToInt32(Session["usertypeid"]);
    //    objPropUser.UserID = Convert.ToInt32(Session["userid"]);
    //    objPropUser.ConnConfig = Session["config"].ToString();
    //    objPropUser.DBName = Session["dbname"].ToString();
    //    DataSet ds = new DataSet();
    //    ds = objBL_User.getUserByID(objPropUser);
    //    return ds.Tables[0];
    //}

    private void customerpermissions()
    {
        string url = HttpContext.Current.Request.Url.AbsoluteUri;
        if (Session["type"].ToString() == "c")
        {
            menu.Attributes.Add("style", "display:none");
            //string path = HttpContext.Current.Request.Url.AbsolutePath;
            //if (path == "/invoices.aspx" || path == "/TicketListView.aspx" || path == "/Equipments.aspx" || path == "/Library.aspx" || path == "/portalhome.aspx" || path == "/paymenthistory.aspx")
            //{

            //}
            //else
            //{
            //    Response.Redirect("portalhome.aspx");
            //}
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
    private void GetControlForPayroll()
    {
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());

        DataSet ds = new DataSet();
        ds = objBL_Report.GetControlForPayroll(objProp_User);
        bool PR = Convert.ToBoolean(DBNull.Value.Equals(ds.Tables[0].Rows[0]["PR"]) ? 0 : ds.Tables[0].Rows[0]["PR"]);

        bool PRUser = objBL_User.getPRUserByID(objProp_User);

        if (PR == true && PRUser == true)
        {
            prID.Visible = true;
            lnkTimesheet.Visible = false;
        }
        else
        {
            if (PR == true && Session["User"].ToString() == "Maintenance")
            {
                prID.Visible = true;
                lnkTimesheet.Visible = false;
            }

        }
    }

    private void FinancialPermissions()
    {
        bool _addFinance = (bool)Session["AddFinance"];     //Check FM permission
        bool _editFinance = (bool)Session["EditFinance"];
        bool _viewFinance = (bool)Session["ViewFinance"];
        if (Session["FinanceManager"].ToString() == "F")
        {
            //financeMgr.Visible = true;
            //lnkJournalEntry.Visible = true;
            //lnkCOA.Visible = true;
            //lnkReceivePayment.Visible = true;
            //lnkDeposit.Visible = true;
            //lnkWriteCheck.Visible = true;
            // lnkCollections.Visible = true;
        }
        else if (_addFinance.Equals(true) || _editFinance.Equals(true) || _viewFinance.Equals(true))
        {
            //financeMgr.Visible = true;
            // lnkJournalEntry.Visible = true;
            //lnkCOA.Visible = true;
            //lnkReceivePayment.Visible = true;
            // lnkDeposit.Visible = true;
            //lnkWriteCheck.Visible = true;
            //lnkCollections.Visible = true;
        }
        else
        {
            //financeMgr.Attributes.Add("style", "display:none");
            //lnkJournalEntry.Attributes.Add("style", "display:none");
            //lnkCOA.Attributes.Add("style", "display:none");
            // lnkReceivePayment.Attributes.Add("style", "display:none");
            // lnkDeposit.Attributes.Add("style", "display:none");
            //lnkWriteCheck.Visible = false;
            //lnkCollections.Attributes.Add("style", "display:none");
        }
        //if ((bool)Session["AP"].Equals(true))
        //{                                           // Check AP permission
        //    acctPayable.Visible = true;
        //    lnkVendors.Visible = true;
        //    lnkAddBill.Visible = true;
        //    lnkPO.Visible = true;
        //    lnkWriteCheck2.Visible = true;
        //}
        //else
        //{
        //    acctPayable.Visible = false;
        //    lnkVendors.Visible = false;
        //    lnkAddBill.Visible = false;
        //    lnkPO.Visible = false;
        //    lnkWriteCheck2.Visible = false;
        //}


        if ((bool)Session["FinanceStatement"].Equals(true))     // Check FS permission
        {
            financialStatement.Visible = true;
            lnkTrialBalance.Visible = true;
            lnkIncomeStatement.Visible = true;
            lnkBalanceSheet.Visible = true;
            lnkComparativeStatement.Visible = true;
        }
        else
        {
            financialStatement.Visible = false;
            lnkTrialBalance.Visible = false;
            lnkIncomeStatement.Visible = false;
            lnkBalanceSheet.Visible = false;
            lnkComparativeStatement.Visible = false;
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
            divBreadCrumbWithQB.Visible = false;
            divQBContents.Visible = false;
        }
    }

    private void NonAdminUSerPermissions()
    {
        if (Convert.ToInt16(Session["payment"]) != 1)
        {
            lnkPaymentHistory.NavigateUrl = "";
            lnkPaymentHistory.Attributes.Add("style", "display:none");
        }

        /*****************IF logged in user is not Admin**************/
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable dt = new DataTable();
            DataTable dtUserPermission = new DataTable();
            dt = dtUserPermission = (DataTable)Session["userinfo"];
            //dtUserPermission = GetUserById();
            string role = dt.Rows[0]["role"].ToString();
            string ProgFunc = dt.Rows[0]["Control"].ToString().Substring(0, 1);
            string usertype = dt.Rows[0]["usertype"].ToString();
            string AccessUser = dt.Rows[0]["users"].ToString().Substring(0, 1);
            string Sales = dt.Rows[0]["sales"].ToString().Substring(0, 1);

            string ViewEquipment = dtUserPermission.Rows[0]["Elevator"].ToString();
            ViewEquipment = ViewEquipment.Length < 4 ? "N" : ViewEquipment.Substring(3, 1);

            string ViewProject = dt.Rows[0]["job"].ToString();
            ViewProject = ViewProject.Length < 4 ? "N" : ViewProject.Substring(3, 1);

            string ViewInventoryItem = dt.Rows[0]["Item"].ToString();
            ViewInventoryItem = ViewInventoryItem.Length < 4 ? "N" : ViewInventoryItem.Substring(3, 1);

            string ViewInventoryAdjustment = dt.Rows[0]["InvAdj"].ToString();
            ViewInventoryAdjustment = ViewInventoryAdjustment.Length < 4 ? "N" : ViewInventoryAdjustment.Substring(3, 1);

            string ViewLocation = dtUserPermission.Rows[0]["Location"].ToString();
            ViewLocation = ViewLocation.Length < 4 ? "N" : ViewLocation.Substring(3, 1);

            string ViewOwner = dtUserPermission.Rows[0]["Owner"].ToString();
            ViewOwner = ViewOwner.Length < 4 ? "N" : ViewOwner.Substring(3, 1);

            string ViewTicket = dt.Rows[0]["TicketPermission"].ToString();
            ViewTicket = ViewTicket.Length < 4 ? "N" : ViewTicket.Substring(3, 1);

            string ViewProjecttempPermission = dt.Rows[0]["ProjecttempPermission"].ToString();
            ViewProjecttempPermission = ViewProjecttempPermission.Length < 4 ? "N" : ViewProjecttempPermission.Substring(3, 1);


            {
                string CustomermodulePermission = dtUserPermission.Rows[0]["CustomermodulePermission"] == DBNull.Value ? "N" : dtUserPermission.Rows[0]["CustomermodulePermission"].ToString();

                if (CustomermodulePermission != "Y")
                {
                    cstmMgr.NavigateUrl = "";
                    cstmMgr.Attributes.Add("style", "display:none");
                }


                string ProjectModulePermission = dtUserPermission.Rows[0]["ProjectModulePermission"] == DBNull.Value ? "N" : dtUserPermission.Rows[0]["ProjectModulePermission"].ToString();

                if (ProjectModulePermission != "Y")
                {
                    ProjectMgr.NavigateUrl = "";
                    ProjectMgr.Attributes.Add("style", "display:none");
                }


                string InventoryModulePermission = dtUserPermission.Rows[0]["InventoryModulePermission"] == DBNull.Value ? "N" : dtUserPermission.Rows[0]["InventoryModulePermission"].ToString();

                if (InventoryModulePermission != "Y")
                {
                    InventoryMgr.NavigateUrl = "";
                    InventoryMgr.Attributes.Add("style", "display:none");
                }



                if (ViewEquipment != "Y")
                {
                    lnkEquipmentsSMenu.NavigateUrl = "";
                    lnkEquipmentsSMenu.Attributes.Add("style", "display:none");
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster1", "if( $('#" + lnkEquipmentsSMenu.ClientID + "').length ){ $('#" + lnkEquipmentsSMenu.ClientID + "').click(function(event) { noty({text: 'You do not have permissions to access Equipment!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true});  }); }", true);
                }
                if (ViewProject != "Y")
                {
                    lnkProject.NavigateUrl = "";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster2", "if($('#" + lnkProject.ClientID + "').length){ $('#" + lnkProject.ClientID + "').click(function(event) { noty({text: 'You do not have permissions to access Projects!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true });  });}", true);
                }
                if (ViewLocation != "Y")
                {
                    lnkLocationsSMenu.NavigateUrl = "";
                    lnkLocationsSMenu.Attributes.Add("style", "display:none");
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster3", "if($('#" + lnkLocationsSMenu.ClientID + "').length){$('#" + lnkLocationsSMenu.ClientID + "').click(function(event) { noty({text: 'You do not have permissions to access Locations!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true });  });}", true);
                }
                if (ViewOwner != "Y")
                {
                    lnkCustomersSmenu.NavigateUrl = "";
                    lnkCustomersSmenu.Attributes.Add("style", "display:none");
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster4", "if($('#" + lnkCustomersSmenu.ClientID + "').length){ $('#" + lnkCustomersSmenu.ClientID + "').click(function(event) { noty({text: 'You do not have permissions to access Customers!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true });  });}", true);
                }


                string ViewApplyPermission = dtUserPermission.Rows[0]["Apply"].ToString();

                ViewApplyPermission = ViewApplyPermission.Length < 4 ? "N" : ViewApplyPermission.Substring(3, 1);

                if (ViewApplyPermission != "Y")
                {
                    lnkReceivePayment.NavigateUrl = "";
                    lnkReceivePayment.Attributes.Add("style", "display:none");
                }


                //string ViewOnlinePaymentPermission = dtUserPermission.Rows[0]["OnlinePayment"].ToString();

                //ViewOnlinePaymentPermission = ViewOnlinePaymentPermission.Length < 4 ? "N" : ViewOnlinePaymentPermission.Substring(3, 1);

                //if (ViewOnlinePaymentPermission != "Y")
                //{
                //    lnkOnlinePayment.NavigateUrl = "";
                //    lnkOnlinePayment.Attributes.Add("style", "display:none");
                //}


                string ViewDepositPermission = dtUserPermission.Rows[0]["Deposit"].ToString();

                ViewDepositPermission = ViewDepositPermission.Length < 4 ? "N" : ViewDepositPermission.Substring(3, 1);

                if (ViewDepositPermission != "Y")
                {
                    lnkDeposit.NavigateUrl = "";
                    lnkDeposit.Attributes.Add("style", "display:none");
                }

                string ViewCollectionPermission = dtUserPermission.Rows[0]["Collection"].ToString();

                ViewCollectionPermission = ViewCollectionPermission.Length < 4 ? "N" : ViewCollectionPermission.Substring(3, 1);

                if (ViewCollectionPermission != "Y")
                {
                    lnkCollections.NavigateUrl = "";
                    lnkCollections.Attributes.Add("style", "display:none");
                }

                if (ViewTicket != "Y")
                {
                    lnkListView.NavigateUrl = "";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster5", "if($('#" + lnkListView.ClientID + "') != null && $('#" + lnkListView.ClientID + "').length ){ $('#" + lnkListView.ClientID + "').click(function(event) { noty({text: 'You do not have permissions to access Ticket!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true});  }); }", true);
                }
                if (new GeneralFunctions().GetSalesAsigned() == 0)
                {
                    if (ViewInventoryItem != "Y")
                    {
                        lnkItemMaster.NavigateUrl = "";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster6", "if($('#" + lnkItemMaster.ClientID + "') != null && $('#" + lnkItemMaster.ClientID + "').length){ $('#" + lnkItemMaster.ClientID + "').click(function(event) { noty({text: 'You do not have permissions to access Inventory Item!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true });  });}", true);
                    }
                    if (ViewInventoryAdjustment != "Y")
                    {
                        lnkAdjustment.NavigateUrl = "";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster7", "if($('#" + lnkAdjustment.ClientID + "') != null && $('#" + lnkAdjustment.ClientID + "').length){ $('#" + lnkAdjustment.ClientID + "').click(function(event) { noty({text: 'You do not have permissions to access Inventory Adjustment!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true });  });}", true);
                    }
                    if (ViewProjecttempPermission != "Y")
                    {
                        lnkProjectTempl.NavigateUrl = "";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster8", "if($('#" + lnkProjectTempl.ClientID + "') != null && $('" + lnkProjectTempl.ClientID + "').length){$('#" + lnkProjectTempl.ClientID + "').click(function(event) { noty({text: 'You do not have permissions to access Templates!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true });  });}", true);

                        lnkEstimateTempl.NavigateUrl = "";
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster9", "if($('#" + lnkEstimateTempl.ClientID + "').length){ $('#" + lnkEstimateTempl.ClientID + "').click(function(event) { noty({text: 'You do not have permissions to access Templates!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true });  });}", true);
                    }

                }



                string ViewInvoicesPermission = dtUserPermission.Rows[0]["Invoice"].ToString();

                ViewInvoicesPermission = ViewInvoicesPermission.Length < 4 ? "N" : ViewInvoicesPermission.Substring(3, 1);

                if (ViewInvoicesPermission != "Y")
                {
                    lnkInvoicesSMenu.NavigateUrl = "";
                    lnkInvoicesSMenu.Attributes.Add("style", "display:none");
                    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster5", "if( $('#" + lnkInvoicesSMenu.ClientID + "').length ){ $('#" + lnkInvoicesSMenu.ClientID + "').click(function(event) { noty({text: 'You do not have permissions to access Invoices!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true});  }); }", true);
                }


                string ViewPOPermission = dtUserPermission.Rows[0]["PO"].ToString();

                ViewPOPermission = ViewPOPermission.Length < 4 ? "N" : ViewPOPermission.Substring(3, 1);
                if (ViewPOPermission != "Y")
                {
                    lnkPO.NavigateUrl = "";
                    lnkPO.Attributes.Add("style", "display:none");

                    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyViewPOPermission", "if( $(" + lnkPO.ClientID + ").length ){ $(" + lnkPO.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access PO!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true});  }); }", true); 

                }

                string ViewReceivePOPermission = dtUserPermission.Rows[0]["RPO"].ToString();

                ViewReceivePOPermission = ViewReceivePOPermission.Length < 4 ? "N" : ViewReceivePOPermission.Substring(3, 1);
                if (ViewReceivePOPermission != "Y")
                {

                    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyViewPOPermission", "if( $(" + lnkPO.ClientID + ").length ){ $(" + lnkPO.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access PO!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true});  }); }", true); 

                    lnkReceivePO.NavigateUrl = "";
                    lnkReceivePO.Attributes.Add("style", "display:none");

                    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyViewPOPermission1", "if( $(" + lnkReceivePO.ClientID + ").length ){ $(" + lnkReceivePO.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access PO!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true});  }); }", true);
                }
                string ViewBillingCodesPermissionn = dtUserPermission.Rows[0]["BillingCodesPermission"].ToString();

                ViewBillingCodesPermissionn = ViewBillingCodesPermissionn.Length < 4 ? "N" : ViewBillingCodesPermissionn.Substring(3, 1);

                if (ViewBillingCodesPermissionn != "Y")
                {
                    lnkBillcodeSMenu.NavigateUrl = "";
                    lnkBillcodeSMenu.Attributes.Add("style", "display:none");
                    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyBillingCodes", "if( $(" + lnkBillcodeSMenu.ClientID + ").length ){ $(" + lnkBillcodeSMenu.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access Billing Codes!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true});  }); }", true);
                }

                string ViewPaymentHistoryPermissionn = dtUserPermission.Rows[0]["PaymentHistoryPermission"].ToString();

                ViewPaymentHistoryPermissionn = ViewPaymentHistoryPermissionn.Length < 4 ? "N" : ViewPaymentHistoryPermissionn.Substring(3, 1);

                if (ViewPaymentHistoryPermissionn != "Y")
                {
                    lnkPaymentHistory.NavigateUrl = "";
                    lnkPaymentHistory.Attributes.Add("style", "display:none");
                }
                string ViewManageChecksPermission = dtUserPermission.Rows[0]["BillPay"].ToString();

                ViewManageChecksPermission = ViewManageChecksPermission.Length < 4 ? "N" : ViewManageChecksPermission.Substring(3, 1);

                if (ViewManageChecksPermission != "Y")
                {
                    lnkWriteCheck2.NavigateUrl = "";
                    lnkWriteCheck2.Attributes.Add("style", "display:none");
                    lnkWriteCheck.NavigateUrl = "";
                    lnkWriteCheck.Attributes.Add("style", "display:none");
                }

                string ViewBillPermission = dtUserPermission.Rows[0]["Bill"].ToString();

                ViewBillPermission = ViewBillPermission.Length < 4 ? "N" : ViewBillPermission.Substring(3, 1);

                if (ViewBillPermission != "Y")
                {
                    lnkAddBill.NavigateUrl = "";
                    lnkAddBill.Attributes.Add("style", "display:none");
                }

                string ViewVendorPermission = dtUserPermission.Rows[0]["Vendor"].ToString();

                ViewVendorPermission = ViewVendorPermission.Length < 4 ? "N" : ViewVendorPermission.Substring(3, 1);

                if (ViewVendorPermission != "Y")
                {
                    lnkVendors.NavigateUrl = "";
                    lnkVendors.Attributes.Add("style", "display:none");
                }


                string FinancialmodulePermission = dtUserPermission.Rows[0]["FinancialmodulePermission"] == DBNull.Value ? "N" : dtUserPermission.Rows[0]["FinancialmodulePermission"].ToString();

                if (FinancialmodulePermission != "Y")
                {
                    financeMgr.NavigateUrl = "";
                    financeMgr.Attributes.Add("style", "display:none");
                }

                string chartofAccountPermission = dtUserPermission.Rows[0]["chart"].ToString();

                chartofAccountPermission = chartofAccountPermission.Length < 4 ? "N" : chartofAccountPermission.Substring(3, 1);

                if (chartofAccountPermission != "Y")
                {
                    lnkCOA.NavigateUrl = "";
                    lnkCOA.Attributes.Add("style", "display:none");
                }
                string bankrecPermission = dtUserPermission.Rows[0]["bankrec"].ToString();

                bankrecPermission = bankrecPermission.Length < 4 ? "N" : bankrecPermission.Substring(3, 1);

                if (bankrecPermission != "Y")
                {
                    lnkBankRecon.NavigateUrl = "";
                    lnkBankRecon.Attributes.Add("style", "display:none");
                }
                string JournalEntryPermission = dtUserPermission.Rows[0]["GLAdj"].ToString();

                JournalEntryPermission = JournalEntryPermission.Length < 4 ? "N" : JournalEntryPermission.Substring(3, 1);

                if (JournalEntryPermission != "Y")
                {
                    lnkJournalEntry.NavigateUrl = "";
                    lnkJournalEntry.Attributes.Add("style", "display:none");
                }

                string RCmodulePermission = dtUserPermission.Rows[0]["RCmodulePermission"] == DBNull.Value ? "N" : dtUserPermission.Rows[0]["RCmodulePermission"].ToString();

                if (RCmodulePermission != "Y")
                {
                    cntractsMgr.NavigateUrl = "";
                    cntractsMgr.Attributes.Add("style", "display:none");
                }

                string processRCPermission = dtUserPermission.Rows[0]["ProcessRCPermission"].ToString();

                processRCPermission = processRCPermission.Length < 4 ? "N" : processRCPermission.Substring(3, 1);
                if (processRCPermission != "Y")
                {
                    lnkContractsMenu.NavigateUrl = "";
                    lnkContractsMenu.Attributes.Add("style", "display:none");
                }

                string ProcessC = dtUserPermission.Rows[0]["ProcessC"].ToString();

                ProcessC = ProcessC.Length < 4 ? "N" : ProcessC.Substring(3, 1);
                if (ProcessC != "Y")
                {
                    lnkInvoicesMenu.NavigateUrl = "";
                    lnkInvoicesMenu.Attributes.Add("style", "display:none");
                }

                string ProcessT = dtUserPermission.Rows[0]["ProcessT"].ToString();

                ProcessT = ProcessT.Length < 4 ? "N" : ProcessT.Substring(3, 1);
                if (ProcessT != "Y")
                {
                    lnkTicketsMenu.NavigateUrl = "";
                    lnkTicketsMenu.Attributes.Add("style", "display:none");
                }

                string SafteyTest = dtUserPermission.Rows[0]["SafetyTestsPermission"].ToString();

                SafteyTest = SafteyTest.Length < 4 ? "N" : SafteyTest.Substring(3, 1);
                if (SafteyTest != "Y")
                {
                    lnk.NavigateUrl = "";
                    lnk.Attributes.Add("style", "display:none");
                }

                string SchedulemodulePermission = dtUserPermission.Rows[0]["SchedulemodulePermission"] == DBNull.Value ? "N" : dtUserPermission.Rows[0]["SchedulemodulePermission"].ToString();

                if (SchedulemodulePermission != "Y")
                {
                    schMgr.NavigateUrl = "";
                    schMgr.Attributes.Add("style", "display:none");
                }
                string ScheduleboardPermission = dtUserPermission.Rows[0]["Ticket"].ToString();

                ScheduleboardPermission = ScheduleboardPermission.Length < 4 ? "N" : ScheduleboardPermission.Substring(3, 1);
                if (ScheduleboardPermission != "Y")
                {
                    lnkScheduleMenu.NavigateUrl = "";
                    lnkScheduleMenu.Attributes.Add("style", "display:none");
                }
                string TicketListPermission = dtUserPermission.Rows[0]["Dispatch"].ToString();

                TicketListPermission = TicketListPermission.Length < 4 ? "N" : TicketListPermission.Substring(3, 1);
                if (TicketListPermission != "Y")
                {
                    lnkListView.NavigateUrl = "";
                    lnkListView.Attributes.Add("style", "display:none");
                }
                string MTimesheetPermission = dtUserPermission.Rows[0]["MTimesheet"].ToString();

                MTimesheetPermission = MTimesheetPermission.Length < 4 ? "N" : MTimesheetPermission.Substring(3, 1);
                if (MTimesheetPermission != "Y")
                {
                    HyperLink1.NavigateUrl = "";
                    HyperLink1.Attributes.Add("style", "display:none");
                }

                string eTimesheetPermission = dtUserPermission.Rows[0]["ETimesheet"].ToString();

                eTimesheetPermission = eTimesheetPermission.Length < 4 ? "N" : eTimesheetPermission.Substring(3, 1);
                if (eTimesheetPermission != "Y")
                {
                    lnkTimesheet.NavigateUrl = "";
                    lnkTimesheet.Attributes.Add("style", "display:none");
                }

                string MapPermission = dtUserPermission.Rows[0]["MapR"].ToString();

                MapPermission = MapPermission.Length < 4 ? "N" : MapPermission.Substring(3, 1);
                if (MapPermission != "Y")
                {
                    lnkMapMenu.NavigateUrl = "";
                    lnkMapMenu.Attributes.Add("style", "display:none");
                }

                string RouteBuilderPermission = dtUserPermission.Rows[0]["RouteBuilder"].ToString();
                RouteBuilderPermission = RouteBuilderPermission.Length < 4 ? "N" : RouteBuilderPermission.Substring(3, 1);
                if (RouteBuilderPermission != "Y")
                {
                    lnkRouteBuilder.NavigateUrl = "";
                    lnkRouteBuilder.Attributes.Add("style", "display:none");
                }

                string SalesManagerModulePermission = dtUserPermission.Rows[0]["SalesManager"] == DBNull.Value ? "N" : dtUserPermission.Rows[0]["SalesManager"].ToString();

                if (SalesManagerModulePermission != "Y")
                {
                    SalesMgr.NavigateUrl = "";
                    SalesMgr.Attributes.Add("style", "display:none");
                }

                string UserSalesPermission = dtUserPermission.Rows[0]["UserSales"].ToString();
                UserSalesPermission = UserSalesPermission.Length < 4 ? "N" : UserSalesPermission.Substring(3, 1);
                if (UserSalesPermission != "Y")
                {
                    lnkProspect.NavigateUrl = "";
                    lnkProspect.Attributes.Add("style", "display:none");
                }

                int taskPermission = dtUserPermission.Rows[0]["ToDo"] == DBNull.Value ? 0 : Convert.ToInt32(dtUserPermission.Rows[0]["ToDo"]);

                if (taskPermission == 0)
                {
                    lnkTasks.NavigateUrl = "";
                    lnkTasks.Attributes.Add("style", "display:none");
                }

                string ProposalPermission = dtUserPermission.Rows[0]["Proposal"].ToString();
                ProposalPermission = ProposalPermission.Length < 4 ? "N" : ProposalPermission.Substring(3, 1);
                if (ProposalPermission != "Y")
                {
                    lnkOpportunities.NavigateUrl = "";
                    lnkOpportunities.Attributes.Add("style", "display:none");
                }

                string EstimatesPermission = dtUserPermission.Rows[0]["Estimates"].ToString();
                EstimatesPermission = EstimatesPermission.Length < 4 ? "N" : EstimatesPermission.Substring(3, 1);
                if (EstimatesPermission != "Y")
                {
                    lnkEstimate.NavigateUrl = "";
                    lnkEstimate.Attributes.Add("style", "display:none");
                }

                string TemplatesPermission = dtUserPermission.Rows[0]["ProjecttempPermission"].ToString();
                TemplatesPermission = TemplatesPermission.Length < 4 ? "N" : TemplatesPermission.Substring(3, 1);
                if (TemplatesPermission != "Y")
                {
                    lnkEstimateTempl.NavigateUrl = "";
                    lnkEstimateTempl.Attributes.Add("style", "display:none");
                }
                string salessetupPermission = dtUserPermission.Rows[0]["salessetup"].ToString();
                salessetupPermission = salessetupPermission.Length < 4 ? "N" : salessetupPermission.Substring(3, 1);
                if (salessetupPermission != "Y")
                {
                    lnkSalesSetup.NavigateUrl = "";
                    lnkSalesSetup.Attributes.Add("style", "display:none");
                }

                #region $$$$ Module Permission $$$


                string PurchasingmodulePermission = dtUserPermission.Rows[0]["PurchasingmodulePermission"] == DBNull.Value ? "N" : dtUserPermission.Rows[0]["PurchasingmodulePermission"].ToString();

                if (PurchasingmodulePermission != "Y")
                {
                    purchaseMgr.Visible = false;
                }
                string BillingmodulePermission = dtUserPermission.Rows[0]["BillingmodulePermission"] == DBNull.Value ? "N" : dtUserPermission.Rows[0]["BillingmodulePermission"].ToString();

                if (BillingmodulePermission != "Y")
                {
                    //acctMgr.Visible = false;
                    acctMgr.NavigateUrl = "";
                    acctMgr.Visible = false;
                }

                string AccountPayablemodulePermission = dtUserPermission.Rows[0]["AccountPayablemodulePermission"] == DBNull.Value ? "N" : dtUserPermission.Rows[0]["AccountPayablemodulePermission"].ToString();

                if (AccountPayablemodulePermission != "Y")
                {
                    acctPayable.NavigateUrl = "";
                    acctPayable.Visible = false;
                }
                #endregion
            }

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

            //CustomerPortalPermissions(usertype);

            if (ProgFunc == "Y")
            {
                progMgr.Visible = true;
                if (AccessUser != "Y")
                {
                    if (new GeneralFunctions().GetSalesAsigned() == 0)
                    {
                        lnkUsersSMenu.NavigateUrl = "";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster5", " $('#" + lnkUsersSMenu.ClientID + "').click(function(event) { noty({text: 'You do not have permissions to access users!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true});  });", true);
                    }
                }
            }
            else
            {
                progMgr.Visible = false;
            }

            if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
            {

                cntractsMgr.Visible = false;
                acctMgr.Visible = false;
                lblSuper.Visible = true;
                lnkCntrlPnl.Visible = false;
                lnkCustomFields.Visible = false;
                lnkCustomersSmenu.Visible = false;
                lnkLocationsSMenu.Visible = false;
            }



        }
    }

    private void CustomerPortalPermissions(string usertype)
    {
        /**Exclude the menu items for customer portal**/
        if (usertype == "c")
        {
            
            schMgr.Visible = true;
            cntractsMgr.Visible = false;
            acctMgr.Visible = true;
            SalesMgr.Visible = false;
            progMgr.Visible = false;
            ProjectMgr.Visible = false;
            //notifications.Visible = false;
            lnkBillcodeSMenu.Visible = false;
            lnkScheduleMenu.Visible = false;
            lnkMapMenu.Visible = false;
            lnkRouteBuilder.Visible = false;
            //lnkRoutes.Visible = false;
            lnkProspect.Visible = false;
            divBreadCrumbWithQB.Visible = false;
            divQBContents.Visible = false;
            lnkTimesheet.Visible = false;
            ReportMgr.Visible = false;
            purchaseMgr.Visible = false;
            InventoryMgr.Visible = false;
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
            else
                cstmMgr.Visible = false;
        }
    }

    private void TSDBPermissions()
    {
        /**IF database logged in is TS**/
        if (Session["MSM"].ToString() == "TS")
        {
            lnkCollections.Visible = false;
            financeMgr.Visible = false;
            acctPayable.Visible = false;
            financialStatement.Visible = false;
            ReportMgr.Visible = false;
            lnkPeriodCloseOut.Visible = false;
            lnkReceivePayment.Visible = false;
            lnkDeposit.Visible = false;
            lnkCntrlPnl.Visible = false;
            cntractsMgr.Visible = false;
            InventoryMgr.Visible = false;
            HyperLink1.Visible = false;
            //purchaseMgr.Visible = false;
            liReceivePO.Visible = false;
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
                if (Session["type"].ToString() == "c")
                {
                    lnkPaymentHistory.Visible = false;
                }

            }
        }
    }

    private void ProgramManagerPermission()
    {
        objGeneral.ConnConfig = Session["config"].ToString();
        DataSet _dsCustom = objBL_General.getCustomFieldsControlBranch(objGeneral);

        ///////// IF Company Feature is OFF For the Customer //////////////////////

        if (Session["type"].ToString() == "c")
        {
            Session["COPer"] = "2";
            Session["CmpChkDefault"] = "2";
            liSelectCompany.Visible = false;
            lnkManageCompanies.Visible = false;
        }
        else
        {
            if (_dsCustom.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
                {
                    if (_dr["Name"].ToString().Equals("Branch"))
                    {
                        if (_dr["Label"].ToString() == "1")
                        {
                            Session["COPer"] = "1";
                            Session["CmpChkDefault"] = "1";
                            liSelectCompany.Visible = true;
                            lnkManageCompanies.Visible = true;
                        }
                        else
                        {
                            Session["COPer"] = "2";
                            Session["CmpChkDefault"] = "2";
                            liSelectCompany.Visible = false;
                            lnkManageCompanies.Visible = false;
                        }
                    }
                    if (_dr["Name"].ToString().Equals("MultiOffice"))
                    {
                        if (_dr["Label"].ToString() == "1")
                            liSelectOffice.Visible = false;
                        else
                            liSelectOffice.Visible = false;
                    }
                }
            }
        }
    }

    private void SalesPersonPermission()
    {
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c" && Session["MSM"].ToString() != "TS")
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["userinfo"];
            ///////////////// If the user is a salesperson only show assigned customer, locations, leads and projects.
            string SalesAssigned = dt.Rows[0]["SalesAssigned"] == DBNull.Value ? "0" : dt.Rows[0]["SalesAssigned"].ToString();
            if (SalesAssigned == "1")
            {
                //Main Menue   
                cntractsMgr.Visible = false;
                acctMgr.Visible = false;
                financeMgr.Visible = false;
                purchaseMgr.Visible = false;
                InventoryMgr.Visible = false;
                ReportMgr.Visible = false;
                financialStatement.Visible = false;
                acctPayable.Visible = false;
                progMgr.Visible = false;

                //customer manager sub Menu   

                lnkReceivePayment.Visible = false;
                lnkDeposit.Visible = false;
                lnkCollections.Visible = false;

                // Scheduler sub Menu  

                lnkScheduleMenu.Visible = false;
                lnkTimesheet.Visible = false;
                lnkMapMenu.Visible = false;
                lnkRouteBuilder.Visible = false;
                //lnkRoutes.Visible = false;

                //Contract manager sub Menu  
                lnkDepartments.Visible = false;
                lnkDepartments.Visible = false;
                lnkDepartments.Visible = false;

                // Project manager sub Menu 
                lnkDepartments.Visible = false;
            }
        }
    }

    private void GetDefaultCompany()
    {
        lblSelectCompanyCount.Text = "";
        objCompany.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Company.getUserDefaultCompany(objCompany);
        if (ds.Tables[0].Rows.Count > 0)
        {
            Session["DefaultCompID"] = ds.Tables[0].Rows[0]["EN"].ToString();
            string companyname = ds.Tables[0].Rows[0]["Name"].ToString();
            if (companyname.Length > 16)
                lblSelectCompany.Text = companyname.Substring(0, 16);
            else
                lblSelectCompany.Text = companyname;
            lblSelectCompany.ToolTip = ds.Tables[0].Rows[0]["Name"].ToString();
            if (Session["chkCompanyName"] != null)
            {
                string StrDefComName = Convert.ToString(Session["chkCompanyName"]);
                if (StrDefComName.Length > 16)
                    lblSelectCompany.Text = StrDefComName.Substring(0, 16);
                else
                    lblSelectCompany.Text = StrDefComName;
                lblSelectCompany.ToolTip = StrDefComName;
            }
            if (Convert.ToInt32(ds.Tables[0].Rows[0]["NoOfCompany"].ToString()) > 1)
            {
                lblSelectCompany.Text = "Multi Company";
                lblSelectCompanyCount.Text = "(" + ds.Tables[0].Rows[0]["NoOfCompany"].ToString() + ")";
                lblSelectCompany.ToolTip = Convert.ToString(Session["CompList"]);
            }
        }
        else
        {
            lblSelectCompany.Text = "Select Company";
            lblSelectCompanyCount.Text = "";
        }
    }

    protected void lnkQBLastSync_Click(object sender, EventArgs e)
    {
        Qblastsync();
    }

    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        GetDefaultCompanyReset();
        Session.Abandon();
        Session.Clear();
        Response.Redirect("Login.aspx");
    }

    private void GetDefaultCompanyReset()
    {
        if (Session["NewDefCompanyID"] != null)
        {
            int UserId = Convert.ToInt32(Session["UserID"].ToString());
            int intDefCompanyID = Convert.ToInt32(Session["NewDefCompanyID"]);
            objCompany.DBName = Session["dbname"].ToString();
            objCompany.ConnConfig = Session["config"].ToString();
            objCompany.ID = intDefCompanyID;
            objCompany.IsSel = false;
            objCompany.UserID = UserId;
            objBL_Company.UserCompanyReset(objCompany);
        }
    }

    private void LoadDashboardMenu()
    {
        User objPropUser = new User();

        var userId = Session["userid"];
        objPropUser.UserID = Convert.ToInt32(userId);
        objPropUser.ConnConfig = Session["config"].ToString();
        var ds = objBL_User.GetListDashboard(objPropUser);
        var data = ds.Tables[0];

        if (data.Rows.Count > 0)
        {
            linkDashboarDefault.Visible = false;
            linkDashboardMenu.Visible = true;
            listDashboardMenu.Visible = true;
            listDashboardMenu.DataSource = ds.Tables[0];
            listDashboardMenu.DataBind();

        }
        else
        {
            linkDashboarDefault.Visible = true;
            linkDashboardMenu.Visible = false;
            listDashboardMenu.Visible = false;
        }
    }

    private string GetUserTodoTasksForToday()
    {
        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();

        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        //objProp_Customer.Username = Session["username"].ToString();
        objProp_Customer.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objProp_Customer.DueDate = DateTime.Now;

        ds = objBL_Customer.GetTodoTasksOfUserForTheDate(objProp_Customer);

        GeneralFunctions objGeneral = new GeneralFunctions();
        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);

        // Update total tasks number of user
        TodoTasks = dictListEval.Count;
        return str;
    }


    private void GetGPSInfo()
    {
        General objgeneral = new General();
        string strGPSPing = objBL_General.GetGPSInterval(objgeneral);

        if (strGPSPing != string.Empty)
        {
            ddlGPSPing.SelectedValue = strGPSPing;
        }
        lblMSgGPS.Text = "";
    }

    protected void lnkGPS_Click(object sender, EventArgs e)
    {
        try
        {
            General objgeneral = new General();
            objgeneral.GPSInterval = Convert.ToInt32(ddlGPSPing.SelectedValue);
            objBL_General.InsertGPSInterval(objgeneral);
            lblMSgGPS.Text = "GPS settings updated successfully.";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'GPS settings updated successfully.',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            lblMSgGPS.Text = ex.Message;
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: '" + ex.Message + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
}
