using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using BusinessEntity;
using BusinessLayer;

public partial class HomeMaster : System.Web.UI.MasterPage
{
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();

    General objGeneral = new General();
    BL_General objBL_General = new BL_General();
    Journal _objJe = new Journal();
    BL_GLARecur _objBLRecurr = new BL_GLARecur();
    BusinessEntity.CompanyOffice objCompany = new BusinessEntity.CompanyOffice();
    BL_Company objBL_Company = new BL_Company();

    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    private static int intCount = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["MSM"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (Session["MSM"].ToString() == "ADMIN")
        {
            purchaseMgr.Visible = false;
        }
        if (Session["MSM"].ToString() == "TS")
        {
            lnkEstimate.Visible = false;
            lnkEstimateTempl.Visible = false;
            ProjectMgr.Visible = false;
        }

        if (!IsPostBack)
        {
            #region Process recurring notifications
            ////DataSet _dsRecurrCount = new DataSet();
            ////_objJe.ConnConfig = Session["config"].ToString();
            ////_dsRecurrCount = _objBLRecurr.GetProcessRecurrCount(_objJe);
            ////if (_dsRecurrCount != null)
            ////{
            ////    int _recurCount = Convert.ToInt32(_dsRecurrCount.Tables[0].Rows[0]["CountRecur"]);
            ////    //btnNotifyRecur.Text = _recurCount.ToString();
            ////}
            //DataSet _dsRecurrCount = new DataSet();
            //_objJe.ConnConfig = Session["config"].ToString();
            //_dsRecurrCount = _objBLRecurr.GetProcessRecurrCount(_objJe);
            //if (_dsRecurrCount != null)
            //{
            //    int _recurCount = Convert.ToInt32(_dsRecurrCount.Tables[0].Rows[0]["CountRecur"]);
            //    //btnNotifyRecur.Text = _recurCount.ToString();
            //    spnProcessRecurring_Total.InnerText = _recurCount.ToString();
            //}
            #endregion


            /** Login to Administrator database **/
            if (Session["MSM"].ToString() == "ADMIN")
            {
                AdministratorPermissions();
            }
            else
            {
                userpermissions();//Page Permission 

                if (Session["userid"] != null)
                {
                    //#region Process recurring notifications

                    //DataSet _dsRecurrCount = new DataSet();
                    //_objJe.ConnConfig = Session["config"].ToString();
                    //_dsRecurrCount = _objBLRecurr.GetProcessRecurrCount(_objJe);
                    //if (_dsRecurrCount != null)
                    //{
                    //    int _recurCount = Convert.ToInt32(_dsRecurrCount.Tables[0].Rows[0]["CountRecur"]);
                    //    //btnNotifyRecur.Text = _recurCount.ToString();
                    //    //spnProcessRecurring_Total.InnerText = _recurCount.ToString();
                    //}
                    //#endregion
                }
                else
                {
                    Response.Redirect("login.aspx");
                }

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
                FillCompany();
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
            ViewState["DefaultCompID"] = ds.Tables[0].Rows[0]["EN"].ToString();
            string companyname = ds.Tables[0].Rows[0]["Name"].ToString();
            if (companyname.Length > 16)
                lblSelectCompany.Text = companyname.Substring(0, 16);
            else
                lblSelectCompany.Text = companyname;
            lblSelectCompany.ToolTip = ds.Tables[0].Rows[0]["Name"].ToString();
            if (intCount == 1)
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

    private void GetDefaultCompanyReset()
    {
        intCount = 0;
        int UserId = Convert.ToInt32(Session["UserID"].ToString());
        int intDefCompanyID = Convert.ToInt32(ViewState["SelectedContact"]);
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();
        foreach (GridViewRow row in gvCompany.Rows)
        {
            Label lblUCoID = (Label)row.FindControl("lblID");
            RadioButton rb = (RadioButton)row.FindControl("rbDefaultCompany");
            int UserCompanyId = Convert.ToInt32(lblUCoID.Text);
            objCompany.ID = UserCompanyId;
            objCompany.IsSel = false;
            if (rb.Checked == false)
            {
                objBL_Company.UserCompanyAccess(objCompany);
            }
            else
            {
                objCompany.IsSel = true;
                objBL_Company.UserCompanyAccess(objCompany);
            }
        }
    }
    private void FillCompany()
    {
        objCompany.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Company.getCompanyByUserID(objCompany);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCompany.DataSource = ds.Tables[0];
            gvCompany.DataBind();

            Session["searchdata"] = ds.Tables[0];
            GetDefaultCompanySelected();
        }
    }

    private void FinancialPermissions()
    {
        bool _addFinance = (bool)Session["AddFinance"];     //Check FM permission
        bool _editFinance = (bool)Session["EditFinance"];
        bool _viewFinance = (bool)Session["ViewFinance"];
        if (Session["FinanceManager"].ToString() == "F")
        {
            lnkFinanceMgr.Visible = true;
            lnkJournalEntry.Visible = true;
            lnkCOA.Visible = true;
            lnkReceivePayment.Visible = true;
            //header_notification_bar.Visible = true;
            lnkDeposit.Visible = true;
            lnkWriteCheck.Visible = true;
            lnkCollections.Visible = true;
        }
        else if (_addFinance.Equals(true) || _editFinance.Equals(true) || _viewFinance.Equals(true))
        {
            lnkFinanceMgr.Visible = true;
            lnkJournalEntry.Visible = true;
            lnkCOA.Visible = true;
            lnkReceivePayment.Visible = true;
            //header_notification_bar.Visible = true;
            lnkDeposit.Visible = true;
            lnkWriteCheck.Visible = true;
            lnkCollections.Visible = true;
            //lnkRecurringAdjust.Visible = true;
        }
        else
        {
            lnkFinanceMgr.Visible = false;
            lnkJournalEntry.Visible = false;
            lnkCOA.Visible = false;
            lnkReceivePayment.Visible = false;
            //header_notification_bar.Visible = false;
            lnkDeposit.Visible = false;
            lnkWriteCheck.Visible = false;
            lnkCollections.Visible = false;
            //lnkRecurringAdjust.Visible = false;
        }
        if ((bool)Session["AP"].Equals(true))       // Added by Mayuri 10th Dec,15
        {                                           // Check AP permission
            lnkAcctPayable.Visible = true;
            lnkVendors.Visible = true;
            lnkAddBill.Visible = true;
            lnkPO.Visible = true;
            lnkWriteCheck2.Visible = true;
        }
        else
        {
            lnkAcctPayable.Visible = false;
            lnkVendors.Visible = false;
            lnkAddBill.Visible = false;
            lnkPO.Visible = false;
            lnkWriteCheck2.Visible = false;
        }


        if ((bool)Session["FinanceStatement"].Equals(true))     // Check FS permission
        {
            lnkFinancialStatement.Visible = true;
            lnkTrialBalance.Visible = true;
            lnkIncomeStatement.Visible = true;
            lnkBalanceSheet.Visible = true;
        }
        else
        {
            lnkFinancialStatement.Visible = false;
            lnkTrialBalance.Visible = false;
            lnkIncomeStatement.Visible = false;
            lnkBalanceSheet.Visible = false;
        }
    }
    private void ProgramManagerPermission()
    {
        objGeneral.ConnConfig = Session["config"].ToString();
        DataSet _dsCustom = objBL_General.getCustomFieldsControlBranch(objGeneral);
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

    private void NonAdminUSerPermissions()
    {
        /**IF logged in user is not Admin**/
        if (Session["type"].ToString() != "am")
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["userinfo"];

            string role = dt.Rows[0]["role"].ToString();
            string ProgFunc = dt.Rows[0]["Control"].ToString().Substring(0, 1);
            string usertype = dt.Rows[0]["usertype"].ToString();
            string AccessUser = dt.Rows[0]["users"].ToString().Substring(0, 1);
            string Sales = dt.Rows[0]["sales"].ToString().Substring(0, 1);

            string ViewEquipment = dt.Rows[0]["Elevator"].ToString();
            ViewEquipment = ViewEquipment.Length < 4 ? "Y" : ViewEquipment.Substring(3, 1);

            string ViewProject = dt.Rows[0]["job"].ToString();
            ViewProject = ViewProject.Length < 4 ? "Y" : ViewProject.Substring(3, 1);

            string ViewInventoryItem = dt.Rows[0]["Item"].ToString();
            ViewInventoryItem = ViewInventoryItem.Length < 4 ? "Y" : ViewInventoryItem.Substring(3, 1);

            string ViewInventoryAdjustment = dt.Rows[0]["InvAdj"].ToString();
            ViewInventoryAdjustment = ViewInventoryAdjustment.Length < 4 ? "Y" : ViewInventoryAdjustment.Substring(3, 1);

            string ViewLocation = dt.Rows[0]["Location"].ToString();
            ViewLocation = ViewLocation.Length < 4 ? "Y" : ViewLocation.Substring(3, 1);

            string ViewOwner = dt.Rows[0]["Owner"].ToString();
            ViewOwner = ViewOwner.Length < 4 ? "Y" : ViewOwner.Substring(3, 1);

            string ViewTicket = dt.Rows[0]["TicketPermission"].ToString();
            ViewTicket = ViewTicket.Length < 4 ? "Y" : ViewTicket.Substring(3, 1);

            string ViewProjecttempPermission = dt.Rows[0]["ProjecttempPermission"].ToString();
            ViewProjecttempPermission = ViewProjecttempPermission.Length < 4 ? "Y" : ViewProjecttempPermission.Substring(3, 1);

            if (Session["type"].ToString() != "c" && Session["MSM"].ToString() != "TS")
            {

                if (ViewEquipment != "Y")
                {
                    lnkEquipmentsSMenu.NavigateUrl = "";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster1", "if( $(" + lnkEquipmentsSMenu.ClientID + ").length ){ $(" + lnkEquipmentsSMenu.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access Equipment!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true});  }); }", true);
                }
                if (ViewProject != "Y")
                {
                    lnkProject.NavigateUrl = "";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster2", "if($(" + lnkProject.ClientID + ").length){ $(" + lnkProject.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access Projects!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true });  });}", true);
                }
                if (ViewLocation != "Y")
                {
                    lnkLocationsSMenu.NavigateUrl = "";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster3", "if($(" + lnkLocationsSMenu.ClientID + ").length){ $(" + lnkLocationsSMenu.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access Locations!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true });  });}", true);
                }
                if (ViewOwner != "Y")
                {
                    lnkCustomersSmenu.NavigateUrl = "";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster4", "if($(" + lnkCustomersSmenu.ClientID + ").length){ $(" + lnkCustomersSmenu.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access Customers!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true });  });}", true);
                }

                if (ViewTicket != "Y")
                {
                    lnkListView.NavigateUrl = "";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster5", "if( $(" + lnkListView.ClientID + ").length ){ $(" + lnkListView.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access Ticket!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true});  }); }", true);
                }
                if (new GeneralFunctions().GetSalesAsigned() == 0)
                {
                    if (ViewInventoryItem != "Y")
                    {
                        lnkItemMaster.NavigateUrl = "";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster6", "if($(" + lnkItemMaster.ClientID + ").length){ $(" + lnkItemMaster.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access Inventory Item!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true });  });}", true);
                    }
                    if (ViewInventoryAdjustment != "Y")
                    {
                        lnkAdjustment.NavigateUrl = "";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster7", "if($(" + lnkAdjustment.ClientID + ").length){ $(" + lnkAdjustment.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access Inventory Adjustment!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true });  });}", true);
                    }
                    if (ViewProjecttempPermission != "Y")
                    {
                        lnkProjectTempl.NavigateUrl = "";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster8", "if($(" + lnkProjectTempl.ClientID + ").length){ $(" + lnkProjectTempl.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access Templates!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true });  });}", true);

                        lnkEstimateTempl.NavigateUrl = "";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster9", "if($(" + lnkEstimateTempl.ClientID + ").length){ $(" + lnkEstimateTempl.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access Templates!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true });  });}", true);
                    }

                }

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

            CustomerPortalPermissions(usertype);

            if (ProgFunc == "Y")
            {
                progMgr.Visible = true;
                if (AccessUser != "Y")
                {
                    if (new GeneralFunctions().GetSalesAsigned() == 0)
                    {
                        lnkUsersSMenu.NavigateUrl = "";
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster5", " $(" + lnkUsersSMenu.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access users!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true});  });", true);
                    }
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
    }
    private void AdministratorPermissions()
    {
        //Menu.Visible = false;
        lblUser.Text = "Administrator";
        lblCompany.Text = "Administrator Database";
        cntractsMgr.Visible = false;
        acctMgr.Visible = false;
        salesMgr.Visible = false;
        progMgr.Visible = false;

        lnkBillcodeSMenu.Visible = false;
        lnkScheduleMenu.Visible = false;
        lnkMapMenu.Visible = false;
        //salesMgr.Visible = false;
        //lnkPaymentHistory.Visible = true;
        lnkRouteBuilder.Visible = false;
        lnkProspect.Visible = false;
        //divQBContents.Visible = false;
        lnkTimesheet.Visible = false;
        cstmlink.Visible = false;
        lnkSchd.Visible = false;
        ProjectLink.Visible = false;
        lnkFinanceMgr.Visible = false;
        lnkAcctPayable.Visible = false;     // Change by Mayuri 9th Dec,15
        lnkFinancialStatement.Visible = false;
        //header_notification_bar.Visible = false;
        //header_inbox_bar.Visible = false;
        //header_image_bar.Visible = false;
        //lnkLogout1.Visible = true;
        lnkReportMgr.Visible = false;
        lnkInventoryMgr.Visible = false;
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
            lnkInventoryMgr.Visible = false;
            //lnkPurchaseMgr.Visible = false;
            liReceivePO.Visible = false;
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
    }
    private void CustomerPortalPermissions(string usertype)
    {
        /**Exclude the menu items for customer portal**/
        if (usertype == "c")
        {

            schMgr.Visible = true;
            //cstmMgr.Visible = false;
            cntractsMgr.Visible = false;
            acctMgr.Visible = true;
            salesMgr.Visible = false;
            progMgr.Visible = false;
            ProjectMgr.Visible = false;
            notifications.Visible = false;
            //date_sec.Visible = false;
            lnkBillcodeSMenu.Visible = false;
            lnkScheduleMenu.Visible = false;
            lnkMapMenu.Visible = false;
            //lnkPaymentHistory.Visible = true;
            lnkRouteBuilder.Visible = false;
            HyperLink2.Visible = false;
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
    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        GetDefaultCompanyReset();
        Session.Abandon();
        Session.Clear();
        Response.Redirect("Login.aspx");
    }

    private void GetSelectedRecord()
    {
        for (int i = 0; i < gvCompany.Rows.Count; i++)
        {
            RadioButton rb = (RadioButton)gvCompany.Rows[i]
                            .Cells[1].FindControl("rbDefaultCompany");
            if (rb != null)
            {
                if (rb.Checked)
                {
                    HiddenField hf = (HiddenField)gvCompany.Rows[i]
                                    .Cells[1].FindControl("hdnDefaultCompanyID");
                    if (hf != null)
                    {
                        ViewState["SelectedContact"] = hf.Value;
                    }

                    break;
                }
            }
        }
    }
    private void SetSelectedRecord()
    {
        for (int i = 0; i < gvCompany.Rows.Count; i++)
        {
            RadioButton rb = (RadioButton)gvCompany.Rows[i].Cells[1].FindControl("rbDefaultCompany");
            CheckBox chk = (CheckBox)gvCompany.Rows[i].Cells[0].FindControl("chkBranchSelect");
            if (rb != null)
            {
                HiddenField hf = (HiddenField)gvCompany.Rows[i]
                                    .Cells[1].FindControl("hdnDefaultCompanyID");
                if (hf != null && ViewState["SelectedContact"] != null)
                {
                    if (hf.Value.Equals(ViewState["SelectedContact"].ToString()))
                    {
                        rb.Checked = true;
                        chk.Checked = true;
                        break;
                    }
                }
            }
        }
    }
    private void GetDefaultCompanySelected()
    {
        for (int i = 0; i < gvCompany.Rows.Count; i++)
        {
            RadioButton rb = (RadioButton)gvCompany.Rows[i].Cells[1].FindControl("rbDefaultCompany");
            CheckBox chk = (CheckBox)gvCompany.Rows[i].Cells[0].FindControl("chkBranchSelect");
            if (rb != null)
            {
                HiddenField hf = (HiddenField)gvCompany.Rows[i]
                                    .Cells[1].FindControl("hdnDefaultCompanyID");
                if (hf != null && ViewState["DefaultCompID"] != null)
                {
                    if (hf.Value.Equals(ViewState["DefaultCompID"].ToString()))
                    {
                        rb.Checked = true;
                        //chk.Checked = true;
                        break;
                    }
                }
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //GetSelectedRecord();
        //SetSelectedRecord();
        Session["CmpChkDefault"] = "2";
        Submit();
        GetDefaultCompanySelected();
        Response.Redirect(Request.RawUrl);
    }
    private void Submit()
    {
        intCount = 0;
        int UserId = Convert.ToInt32(Session["UserID"].ToString());
        int intDefCompanyID = Convert.ToInt32(ViewState["SelectedContact"]);
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();
        StringBuilder _selectedCompany = new StringBuilder();
        try
        {
            foreach (GridViewRow row in gvCompany.Rows)
            {
                Label lblUCoID = (Label)row.FindControl("lblID");
                int UserCompanyId = Convert.ToInt32(lblUCoID.Text);
                objCompany.ID = UserCompanyId;
                if ((row.FindControl("chkBranchSelect") as CheckBox).Checked)
                {
                    intCount++;
                    Label lblCompName = (Label)row.FindControl("lblName");
                    objCompany.IsSel = true;
                    Session["CmpChkDefault"] = "1";
                    _selectedCompany.Append(lblCompName.Text + " , ");
                    if (intCount == 1)
                    {
                        Session["chkCompanyName"] = lblCompName.Text;
                    }
                }
                else
                {
                    objCompany.IsSel = false;
                }
                objBL_Company.UserCompanyAccess(objCompany);
            }
            for (int i = 0; i < gvCompany.Rows.Count; i++)
            {
                RadioButton rb = (RadioButton)gvCompany.Rows[i].Cells[1].FindControl("rbDefaultCompany");
                HiddenField hfCompanyID = (HiddenField)gvCompany.Rows[i].Cells[1].FindControl("hdnDefaultCompanyID");
                if (rb.Checked == true)
                {
                    objCompany.UserID = UserId;
                    objCompany.CompanyID = Convert.ToInt32(hfCompanyID.Value);
                    objBL_Company.UpdateUserCompany(objCompany);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            //ClientScript.RegisterStartupScript(Page.GetType(), "alert", "alert('" + str + "');", true);
            // ClientScript.RegisterStartupScript(Page.GetType(), "alert", "alert('Company Updated successfully!');", true);
        }
        finally
        {
            if (_selectedCompany.Length > 1)
            {
                _selectedCompany.Remove(_selectedCompany.Length - 2, 2);
                Session["CompList"] = Convert.ToString(_selectedCompany);
            }
            GetDefaultCompany();
            FillCompany();
        }
    }
    private void ItemAdjustmentPermissions()
    {
        if (Session["PeriodClose"] != null)
        {
            DataTable dtPeriodClose = (DataTable)Session["PeriodClose"];
            if (dtPeriodClose != null)
            {
                if (dtPeriodClose.Rows.Count > 0)
                {
                    // if(dtPeriodClose.Rows[0][""])
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

                //customer manager sub Menue    

                lnkReceivePayment.Visible = false;
                lnkDeposit.Visible = false;
                lnkCollections.Visible = false;

                // Scheduler sub Menue   

                lnkScheduleMenu.Visible = false;
                lnkTimesheet.Visible = false;
                lnkMapMenu.Visible = false;
                lnkRouteBuilder.Visible = false;
                HyperLink2.Visible = false;

                //Contract manager sub Menue  
                lnkDepartments.Visible = false;
                lnkDepartments.Visible = false;
                lnkDepartments.Visible = false;

                // Project manager sub Menue 
                lnkDepartments.Visible = false;
            }
        }
    }

    private DataTable PageSortData()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["searchdata"];
        return dt;
    }
    private void SortGridView(string sortExpression, string direction)
    {
        DataTable dt = PageSortData();

        DataView dv = new DataView(dt);
        dv.Sort = sortExpression + direction;

        BindGridDatatable(dv.ToTable());

    }
    private void BindGridDatatable(DataTable dt)
    {
        Session["searchdata"] = dt;
        if (dt.Rows.Count > 0)
        {
            gvCompany.DataSource = dt;
            gvCompany.DataBind();
        }
    }
    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }
    protected void gvCompany_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (this.gvCompany.Rows.Count > 0)
        {
            gvCompany.UseAccessibleHeader = true;
            gvCompany.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }

    protected void gvCompany_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING);
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING);
        }

    }
}
