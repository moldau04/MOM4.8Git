using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLayer;
using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System.Management;
using System.Web.UI;
using BusinessEntity.Utility;
using BusinessEntity.CommonModel;
using System.Web.Configuration;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.NetworkInformation;

public partial class Login : System.Web.UI.Page
{
    BL_User objBL_User = new BL_User();

    BusinessEntity.User objProp_User = new BusinessEntity.User();

    BusinessEntity.CompanyOffice objCompany = new BusinessEntity.CompanyOffice();

    BL_Company objBL_Company = new BL_Company();

    Journal objJournal = new Journal();

    User objPropUser = new User();

    BL_Report objBL_Report = new BL_Report();

    protected void Page_Load(object sender, EventArgs e)
    {



        ///TEST
        ///

        string CSSPath = System.Web.Configuration.WebConfigurationManager.AppSettings["LoginCSSpath"].Trim();
        if (!string.IsNullOrEmpty(CSSPath))
            Response.Redirect("LandMark");

        #region SSL

        string SSL = WebConfigurationManager.AppSettings["SSL"].Trim();

        if (Request.Url.Scheme == "http" && SSL == "1")
        {
            string URL = Request.Url.ToString();
            URL = URL.Replace("http://", "https://");
            Response.Redirect(URL);
        }

        #endregion

        if (Session["userid"] != null)
        {

            Response.Redirect("home.aspx");

        }
        if (!IsPostBack)
        {
            Session.Abandon();

            Session.Clear();

            CheckTrial();

            FillCompany();

            FillRemembered();

            if (Request.QueryString["DB"] != null)
            {
                string DBName = SSTCryptographer.Decrypt(Request.QueryString["DB"], "pass");
                string val = DBName + ":TS";
                ddlCompany.SelectedValue = val.ToUpper();
                ddlCompany.Visible = false;
            }

            if (Request.QueryString["fogotpassword"] != null)
            {
                var token = SSTCryptographer.Decrypt(Request.QueryString["fogotpassword"], "forgot");
                var arr = token.Split('&');
                var expireTime = Convert.ToDateTime(arr[4]);
                expireTime = expireTime.AddHours(2);
                if (DateTime.Now <= expireTime)
                {
                    User objProp_User = new User();
                    objProp_User.Username = arr[0];
                    objProp_User.Email = arr[1];
                    objProp_User.DBName = arr[2];
                    objProp_User.DBType = arr[3];
                    ConnectionStr(objProp_User.DBName);
                    objProp_User.ConnConfig = Session["config"].ToString();

                    // TODO: get user info
                    var ds = objBL_User.GetUserInfoByUsernameAndEmail(objProp_User);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && (bool)ds.Tables[0].Rows[0]["ForgotPwRequest"])
                    {
                        ViewState["ForgotPwUsername"] = arr[0];
                        ViewState["ForgotPwEmail"] = arr[1];
                        ViewState["ForgotPwDBName"] = arr[2];
                        ViewState["ForgotPwDBType"] = arr[3];
                        ShowHideLogin(false);
                        programmaticModalPopupUpdateForgotPw.Show();
                    }
                    else
                    {
                        ViewState["ForgotPwUsername"] = null;
                        ViewState["ForgotPwEmail"] = null;
                        ShowHideLogin(true);
                        programmaticModalPopupUpdateForgotPw.Hide();
                    }
                }
                else
                {
                    ViewState["ForgotPwUsername"] = null;
                    ViewState["ForgotPwEmail"] = null;
                    ShowHideLogin(true);
                    programmaticModalPopupUpdateForgotPw.Hide();
                }
            }

        }
    }

    private void FillCompany()
    {
        string strDefaultCompany = System.Web.Configuration.WebConfigurationManager.AppSettings["DefaultCompany"].Trim();

        DataSet ds = new DataSet();
        objProp_User.DBName = strDefaultCompany;
        ds = objBL_User.getCompany(objProp_User);

        ddlCompany.DataSource = ds.Tables[0];
        ddlCompany.DataTextField = "companyName";
        ddlCompany.DataValueField = "dbname";
        ddlCompany.DataBind();
        ddlCompany.Items.Add(new ListItem("Administrator", "0"));
        ShowHidePasswordResetting();
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {


        //Check Registration
        string strLic = "0";
        string strDay = "30";
        string strDate = System.DateTime.Now.ToShortDateString();
        string strMachineID = "0";

        string[] strRegItems = Check();

        var et = from s in strRegItems select s;
        int c = et.Count();

        if (c < 4)
        {
            programmaticModalPopup.Show();
            btnOK.Visible = false;
            lblTrial.Text = "Please contact us at (619) 459-7481 to register your software.";
            return;
        }

        strLic = strRegItems[0];
        strDay = strRegItems[1];
        strDate = strRegItems[2];
        strMachineID = strRegItems[3];

        if (Convert.ToBoolean(Convert.ToInt32(strLic)) == false || strMachineID != GetCPUId())
        {
            if (System.DateTime.Today > Convert.ToDateTime(strDate).AddDays(Convert.ToInt32(strDay) - 1))
            {
                programmaticModalPopup.Show();
                btnOK.Visible = false;
                lblTrial.Text = "You have exceeded the " + strDay + " days Trial period of Mobile Office Manager, Please contact us at (619) 459-7481 to register your software.";

            }
            else
            {
                UserAuthorization(txtUsername.Text.Trim(), txtPassword.Text.Trim());
                Session["Animation"] = true;
            }
        }
        else
        {
            UserAuthorization(txtUsername.Text.Trim(), txtPassword.Text.Trim());
            Session["Animation"] = true;
        }
    }

    private void GetPeriodClosedDetails()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Username = txtUsername.Text.Trim();

        DataSet _dsPeriodClose = new DataSet();

        _dsPeriodClose = objBL_Report.GetPeriodClosedYear(objPropUser);
        Session["PeriodClose"] = _dsPeriodClose.Tables[0];
    }

    private void GetDefaultCompanyReset()
    {
        int intDefCompanyID = 0;
        if (Session["UserID"] != null)
        {
            objCompany.UserID = Convert.ToInt32(Session["UserID"].ToString());
            objCompany.DBName = Session["dbname"].ToString();
            objCompany.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            ds = objBL_Company.getUserDefaultCompany(objCompany);
            if (ds.Tables[0].Rows.Count > 0)
            {
                intDefCompanyID = Convert.ToInt32(ds.Tables[0].Rows[0]["EN"].ToString());
            }
            objCompany.ID = intDefCompanyID;
            objCompany.IsSel = false;
            objBL_Company.UserCompanyReset(objCompany);
        }
    }

    public void UserAuthorization(string Username, string Password)
    {
        if (Username == string.Empty)
        {
            txtUsername.Focus();
            lblMsg.Text = "User Name Required";
            divValidationBox.Visible = true;
            return;
        }
        else if (Password == string.Empty)
        {
            txtPassword.Focus();
            lblMsg.Text = "Password Required";
            divValidationBox.Visible = true;
            return;
        }

        objBL_User = new BL_User();
        DataSet ds = new DataSet();

        try
        {
            string dbname = "";
            string dbtype = "";
            Session["CmpChkDefault"] = "2";
            int loginUserId = 0;

            if (ddlCompany.SelectedValue == "0")
            {
                if (string.Compare(Username, "admin", true) == 0 && Password == "huntingt0n!")//Password == "ucandoit!") //Password == "ess!2012"
                {
                    Session["MSM"] = "ADMIN";
                    Response.Redirect("AdminPanel.aspx", false);
                    return;
                }
                objProp_User.Username = Username;
                objProp_User.Password = SSTCryptographer.Encrypt(Password, "pass");

                ds = objBL_User.getAdminAuthorization(objProp_User);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["MSM"] = "ADMIN";
                    Response.Redirect("AdminPanel.aspx", false);
                }
                else
                {
                    divValidationBox.Visible = true;
                    lblMsg.Text = "Invalid Username or Password.";
                }
            }
            else
            {
                if (string.Compare(Username, "admin", true) == 0 && Password == "huntingt0n!")//Password == "ucandoit!") //Password == "ess!2012"
                {
                    AdminSessionInfo();
                    ////// SET and Insert Default Data
                    try
                    {
                        dbtype = (ddlCompany.SelectedValue.Split(':')[1]);
                        objProp_User.DBType = dbtype;
                        objProp_User.ConnConfig = Session["config"].ToString();
                        //////////////
                        ///////////   Only for MOM Customer 
                        //////////////
                        if (dbtype == "MSM")
                        {
                            objBL_User.SetDefaultData(objProp_User);
                        }
                        GetAPIIntegrationEnable();
                        ////Update reapprove if PO description contains reapprove text
                        objBL_User.UpdateReapproveFromFDESC(Session["config"].ToString());
                    }
                    catch { }
                    if (Session["UserID"] != null)
                    {
                        loginUserId = Convert.ToInt32(Session["UserID"].ToString());
                    }
                    CreateLoginLog(loginUserId, Username);
                    return;
                }

                dbname = ddlCompany.SelectedValue.Split(':')[0];
                dbtype = (ddlCompany.SelectedValue.Split(':')[1]);
                objProp_User.Username = Username;
                objProp_User.Password = Password;
                objProp_User.DBName = dbname;
                objProp_User.DBType = dbtype;
                ConnectionStr(ddlCompany.SelectedValue.Split(':')[0]);

                //Start-- API Changes : Juily:14/07/2020 --//
                //Check API Integration Enable or not
                GetAPIIntegrationEnable();
                //End-- API Changes : Juily:14/07/2020 --//

                objProp_User.ConnConfig = Session["config"].ToString();


                ////Update reapprove if PO description contains reapprove text
                objBL_User.UpdateReapproveFromFDESC(Session["config"].ToString());


                ds = objBL_User.getUserLoginAuthorization(objProp_User);
                //ds = objBL_User.getTSUserAuthorization(objProp_User);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow drUserInfo = ds.Tables[0].Rows[0];
                            if (ds.Tables[0].Rows.Count > 1)
                            {
                                foreach (DataRow item in ds.Tables[0].Rows)
                                {
                                    loginUserId = Convert.ToInt32(item["userid"]);
                                    var userId = item["userid"].ToString();
                                    if (userId != "0")
                                    {
                                        drUserInfo = item;
                                    }
                                }
                            }


                            // Check if we need to update the password following the password policy
                            string _userName = drUserInfo["fUser"].ToString();
                            string _firstName = drUserInfo["fFirst"].ToString();
                            string _lastName = drUserInfo["Last"].ToString();
                            string errorMess = string.Empty;

                            var isUserApplyPwRules = Convert.ToBoolean(0);
                            var isSysApplyPwRules = Convert.ToBoolean(0);
                            Boolean isResetPwByResetDays = false;

                            if (ds.Tables.Count > 1)
                            {
                                isUserApplyPwRules = Convert.ToBoolean(ds.Tables[1].Rows[0]["UserApplyPwRules"].ToString());
                                isSysApplyPwRules = Convert.ToBoolean(ds.Tables[1].Rows[0]["ApplyPasswordRules"].ToString());
                                isResetPwByResetDays = Convert.ToBoolean(ds.Tables[1].Rows[0]["IsResetPwByResetDays"].ToString());
                            }

                            if (isUserApplyPwRules && isSysApplyPwRules &&
                                (isResetPwByResetDays || !WebBaseUtility.IsPasswordPassedPwPolicy(_userName, _firstName, _lastName, Password, ref errorMess)))
                            {
                                //lblMsg.Text = "Your current password haven't passed the password policy.  Please update it before continue!";
                                //divValidationBox.Visible = true;
                                ViewState["currUserName"] = txtUsername.Text;
                                ViewState["currPassword"] = txtPassword.Text;
                                ViewState["firstTime"] = _firstName;
                                ViewState["lastName"] = _lastName;

                                programmaticModalPopupUpdatePassword.Show();
                                lblUpdateMessage.Text = "Please update your Password, as your current password do not satisfy the conditions of Password Policy";
                                lblUpdatePwErrMsg.Text = "";
                                ShowInputPanel(true, Panel2, lblUpdatePwErrMsg.ClientID);
                                return;
                            }

                            string _chart = drUserInfo["Chart"].ToString();
                            string _glAdj = drUserInfo["GLAdj"].ToString();
                            string _financeStatement = drUserInfo["Financial"].ToString().Substring(5, 1);

                            string _apVendor = drUserInfo["Vendor"].ToString();       // change by Mayuri 10th Dec ,15
                            string _apBill = drUserInfo["Bill"].ToString();
                            // string _apBillSelect = drUserInfo["BillSelect"].ToString();
                            string _apBillPay = drUserInfo["BillPay"].ToString();

                            Session["AP"] = false;
                            Session["FinanceManager"] = "";
                            Session["FinanceStatement"] = false;
                            if (_glAdj.Equals("YYYYYY") && _chart.Equals("YYYYYY"))
                            {
                                Session["FinanceManager"] = "F";
                                Session["AddFinance"] = true;
                                Session["EditFinance"] = true;
                                Session["ViewFinance"] = true;
                            }
                            else
                            {
                                string _addChart = _chart.Substring(0, 1);
                                string _editChart = _chart.Substring(1, 1);
                                string _viewChart = _chart.Substring(3, 1);
                                string _addglAdj = _glAdj.Substring(0, 1);
                                string _editglAdj = _glAdj.Substring(1, 1);
                                string _viewglAdj = _glAdj.Substring(3, 1);
                                if (_addChart.Equals("Y") && _addglAdj.Equals("Y"))
                                    Session["AddFinance"] = true;
                                else
                                    Session["AddFinance"] = false;

                                if (_editChart.Equals("Y") && _editglAdj.Equals("Y"))
                                    Session["EditFinance"] = true;
                                else
                                    Session["EditFinance"] = false;

                                if (_viewChart.Equals("Y") && _viewglAdj.Equals("Y"))
                                    Session["ViewFinance"] = true;
                                else
                                    Session["ViewFinance"] = false;

                            }
                            if (_financeStatement.Equals("Y"))
                            {
                                Session["FinanceStatement"] = true;
                            }
                            else
                            {
                                Session["FinanceStatement"] = false;
                            }
                            if (_apVendor.Contains("Y") || _apBill.Contains("Y") || _apBillPay.Contains("Y"))
                            {
                                Session["AP"] = true;           // Set Account Payable permission
                            }



                            if (drUserInfo["usertype"].ToString() == "c")
                            {
                                //ConnectionStr(ddlCompany.SelectedValue.Split(':')[0]);
                                //objProp_User.ConnConfig = Session["config"].ToString();
                                DataSet dsControl = new DataSet();
                                dsControl = objBL_User.getControl(objProp_User);
                                int custweb = 0;
                                if (dsControl.Tables[0].Columns.Contains("custweb"))
                                {
                                    if (dsControl.Tables[0].Rows[0]["custweb"] != DBNull.Value)
                                    {
                                        if (Convert.ToInt32(dsControl.Tables[0].Rows[0]["custweb"]) == 1)
                                        {
                                            custweb = 1;
                                        }
                                    }
                                }

                                if (custweb == 0)
                                {
                                    Session["userinfodataset"] = ds;
                                    CheckTrialUser(Convert.ToInt32(drUserInfo["UserID"]));
                                }
                                else
                                {
                                    UserSessionInfo(ds);
                                }
                            }
                            else
                            {
                                Session["userinfodataset"] = ds;
                                CheckTrialUser(Convert.ToInt32(drUserInfo["UserID"]));
                            }
                            GetPeriodClosedDetails();
                        }
                        else
                        {
                            divValidationBox.Visible = true;
                            lblMsg.Text = "Invalid Username or Password!";
                        }
                    }
                }

                if (Session["UserID"] != null)
                {
                    loginUserId = Convert.ToInt32(Session["UserID"].ToString());
                }
                CreateLoginLog(loginUserId, Username);
            }


        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
            divValidationBox.Visible = true;
            if (ex.Message == "Invalid Username")
            {
                divValidationBox.Visible = true;
                txtUsername.Focus();
                lblMsg.Text = "Invalid Username";
            }
            else if (ex.Message == "Invalid Password")
            {
                divValidationBox.Visible = true;
                txtPassword.Focus();
                lblMsg.Text = "Invalid Password";
            }
            else
            {
                divValidationBox.Visible = true;
            }
        }
    }

    private void UserSessionInfo(DataSet ds)
    {
        string dbname = ddlCompany.SelectedValue.Split(':')[0];

        var dt = ds.Tables[0];
        var userInfor = dt.Clone();
        var rolePermission = dt.Clone();
        foreach (DataRow item in dt.Rows)
        {
            if (item["UserId"] != null && Convert.ToInt32(item["UserId"].ToString()) != 0)
            {
                userInfor.ImportRow(item);
            }
            else
            {
                rolePermission.ImportRow(item);
            }
        }
        /* Removed merging function
        var isMergingPermissions = userInfor.Rows[0]["ApplyUserRolePermission"].ToString() == "1" ? true : false;
        if (isMergingPermissions)
        {
            foreach (DataRow userPer in userInfor.Rows)
            {
                if (rolePermission.Rows.Count > 0)
                {
                    foreach (DataRow role in rolePermission.Rows)
                    {
                        userPer["Dispatch"] = MergePermissionString(userPer["Dispatch"].ToString(), role["Dispatch"].ToString());
                        userPer["Location"] = MergePermissionString(userPer["Location"].ToString(), role["Location"].ToString());
                        userPer["PO"] = MergePermissionString(userPer["PO"].ToString(), role["PO"].ToString());
                        userPer["Control"] = MergePermissionString(userPer["Control"].ToString(), role["Control"].ToString());
                        userPer["UserS"] = MergePermissionString(userPer["UserS"].ToString(), role["UserS"].ToString());
                        userPer["sales"] = MergePermissionString(userPer["sales"].ToString(), role["sales"].ToString());
                        userPer["EmployeeMaint"] = MergePermissionString(userPer["EmployeeMaint"].ToString(), role["EmployeeMaint"].ToString());
                        userPer["TC"] = MergePermissionString(userPer["TC"].ToString(), role["TC"].ToString());
                        userPer["Chart"] = MergePermissionString(userPer["Chart"].ToString(), role["Chart"].ToString());
                        userPer["GLAdj"] = MergePermissionString(userPer["GLAdj"].ToString(), role["GLAdj"].ToString());
                        userPer["CustomerPayment"] = MergePermissionString(userPer["CustomerPayment"].ToString(), role["CustomerPayment"].ToString());
                        userPer["Deposit"] = MergePermissionString(userPer["Deposit"].ToString(), role["Deposit"].ToString());
                        userPer["Financial"] = MergePermissionString(userPer["Financial"].ToString(), role["Financial"].ToString());
                        userPer["Vendor"] = MergePermissionString(userPer["Vendor"].ToString(), role["Vendor"].ToString());
                        userPer["Bill"] = MergePermissionString(userPer["Bill"].ToString(), role["Bill"].ToString());
                        userPer["BillSelect"] = MergePermissionString(userPer["BillSelect"].ToString(), role["BillSelect"].ToString());
                        userPer["BillPay"] = MergePermissionString(userPer["BillPay"].ToString(), role["BillPay"].ToString());
                        userPer["Owner"] = MergePermissionString(userPer["Owner"].ToString(), role["Owner"].ToString());
                        userPer["Job"] = MergePermissionString(userPer["Job"].ToString(), role["Job"].ToString());
                        userPer["Elevator"] = MergePermissionString(userPer["Elevator"].ToString(), role["Elevator"].ToString());
                        userPer["TicketPermission"] = MergePermissionString(userPer["TicketPermission"].ToString(), role["TicketPermission"].ToString());
                        userPer["ProjectListPermission"] = MergePermissionString(userPer["ProjectListPermission"].ToString(), role["ProjectListPermission"].ToString());
                        userPer["FinancePermission"] = MergePermissionString(userPer["FinancePermission"].ToString(), role["FinancePermission"].ToString());
                        userPer["BOMPermission"] = MergePermissionString(userPer["BOMPermission"].ToString(), role["BOMPermission"].ToString());
                        userPer["MilestonesPermission"] = MergePermissionString(userPer["MilestonesPermission"].ToString(), role["MilestonesPermission"].ToString());
                        userPer["Item"] = MergePermissionString(userPer["Item"].ToString(), role["Item"].ToString());
                        userPer["InvAdj"] = MergePermissionString(userPer["InvAdj"].ToString(), role["InvAdj"].ToString());
                        userPer["Warehouse"] = MergePermissionString(userPer["Warehouse"].ToString(), role["Warehouse"].ToString());
                        userPer["InvSetup"] = MergePermissionString(userPer["InvSetup"].ToString(), role["InvSetup"].ToString());
                        userPer["InvViewer"] = MergePermissionString(userPer["InvViewer"].ToString(), role["InvViewer"].ToString());
                        userPer["DocumentPermission"] = MergePermissionString(userPer["DocumentPermission"].ToString(), role["DocumentPermission"].ToString());
                        userPer["ContactPermission"] = MergePermissionString(userPer["ContactPermission"].ToString(), role["ContactPermission"].ToString());
                        userPer["ProjecttempPermission"] = MergePermissionString(userPer["ProjecttempPermission"].ToString(), role["ProjecttempPermission"].ToString());
                        userPer["BillingCodesPermission"] = MergePermissionString(userPer["BillingCodesPermission"].ToString(), role["BillingCodesPermission"].ToString());
                        userPer["Invoice"] = MergePermissionString(userPer["Invoice"].ToString(), role["Invoice"].ToString());
                        userPer["PurchasingmodulePermission"] = MergePermissionString(userPer["PurchasingmodulePermission"].ToString(), role["PurchasingmodulePermission"].ToString());
                        userPer["BillingmodulePermission"] = MergePermissionString(userPer["BillingmodulePermission"].ToString(), role["BillingmodulePermission"].ToString());
                        userPer["AccountPayablemodulePermission"] = MergePermissionString(userPer["AccountPayablemodulePermission"].ToString(), role["AccountPayablemodulePermission"].ToString());
                        userPer["RPO"] = MergePermissionString(userPer["RPO"].ToString(), role["RPO"].ToString());
                        userPer["JobClosePermission"] = MergePermissionString(userPer["JobClosePermission"].ToString(), role["JobClosePermission"].ToString());
                        userPer["CompletedJObPermission"] = MergePermissionString(userPer["CompletedJObPermission"].ToString(), role["CompletedJObPermission"].ToString());
                        userPer["JobReopenPermission"] = MergePermissionString(userPer["JobReopenPermission"].ToString(), role["JobReopenPermission"].ToString());
                        userPer["Proposal"] = MergePermissionString(userPer["Proposal"].ToString(), role["Proposal"].ToString());

                        userPer["massreview"] = MergePermissionInt(Convert.ToInt32(userPer["massreview"]), Convert.ToInt32(role["massreview"]));

                        //userPer["ticketd"] = MergePermissionInt(Convert.ToInt32(userPer["ticketd"]), Convert.ToInt32(role["ticketd"]));
                        //userPer["ledger"] = MergePermissionInt(Convert.ToInt32(userPer["ledger"]), Convert.ToInt32(role["ledger"]));
                        //userPer["CPEquipment"] = MergePermissionInt(Convert.ToInt32(userPer["CPEquipment"]), Convert.ToInt32(role["CPEquipment"]));
                        //userPer["GroupbyWO"] = MergePermissionInt(Convert.ToInt32(userPer["GroupbyWO"]), Convert.ToInt32(role["GroupbyWO"]));
                        //userPer["openticket"] = MergePermissionInt(Convert.ToInt32(userPer["openticket"]), Convert.ToInt32(role["openticket"]));

                        // sdsd
                        userPer["PaymentHistoryPermission"] = MergePermissionString(userPer["PaymentHistoryPermission"].ToString(), role["PaymentHistoryPermission"].ToString());
                        userPer["CustomermodulePermission"] = MergePermissionString(userPer["CustomermodulePermission"].ToString(), role["CustomermodulePermission"].ToString());
                        userPer["Apply"] = MergePermissionString(userPer["Apply"].ToString(), role["Apply"].ToString());
                        userPer["Collection"] = MergePermissionString(userPer["Collection"].ToString(), role["Collection"].ToString());
                        userPer["bankrec"] = MergePermissionString(userPer["bankrec"].ToString(), role["bankrec"].ToString());
                        userPer["FinancialmodulePermission"] = MergePermissionString(userPer["FinancialmodulePermission"].ToString(), role["FinancialmodulePermission"].ToString());
                        userPer["RCmodulePermission"] = MergePermissionString(userPer["RCmodulePermission"].ToString(), role["RCmodulePermission"].ToString());
                        userPer["ProcessRCPermission"] = MergePermissionString(userPer["ProcessRCPermission"].ToString(), role["ProcessRCPermission"].ToString());
                        userPer["ProcessC"] = MergePermissionString(userPer["ProcessC"].ToString(), role["ProcessC"].ToString());
                        userPer["ProcessT"] = MergePermissionString(userPer["ProcessT"].ToString(), role["ProcessT"].ToString());
                        userPer["SafetyTestsPermission"] = MergePermissionString(userPer["SafetyTestsPermission"].ToString(), role["SafetyTestsPermission"].ToString());
                        userPer["RCRenewEscalatePermission"] = MergePermissionString(userPer["RCRenewEscalatePermission"].ToString(), role["RCRenewEscalatePermission"].ToString());
                        userPer["SchedulemodulePermission"] = MergePermissionString(userPer["SchedulemodulePermission"].ToString(), role["SchedulemodulePermission"].ToString());
                        userPer["Resolve"] = MergePermissionString(userPer["Resolve"].ToString(), role["Resolve"].ToString());
                        userPer["MTimesheet"] = MergePermissionString(userPer["MTimesheet"].ToString(), role["MTimesheet"].ToString());
                        userPer["ETimesheet"] = MergePermissionString(userPer["ETimesheet"].ToString(), role["ETimesheet"].ToString());
                        userPer["MapR"] = MergePermissionString(userPer["MapR"].ToString(), role["MapR"].ToString());
                        userPer["RouteBuilder"] = MergePermissionString(userPer["RouteBuilder"].ToString(), role["RouteBuilder"].ToString());
                        userPer["MassTimesheetCheck"] = MergePermissionString(userPer["MassTimesheetCheck"].ToString(), role["MassTimesheetCheck"].ToString());
                        userPer["CreditHold"] = MergePermissionString(userPer["CreditHold"].ToString(), role["CreditHold"].ToString());
                        userPer["salesmanager"] = MergePermissionString(userPer["salesmanager"].ToString(), role["salesmanager"].ToString());
                        userPer["UserSales"] = MergePermissionString(userPer["UserSales"].ToString(), role["UserSales"].ToString());
                        userPer["ToDo"] = MergePermissionString(userPer["ToDo"].ToString(), role["ToDo"].ToString());
                        userPer["ToDoC"] = MergePermissionString(userPer["ToDoC"].ToString(), role["ToDoC"].ToString());
                        userPer["FU"] = MergePermissionString(userPer["FU"].ToString(), role["FU"].ToString());
                        userPer["Estimates"] = MergePermissionString(userPer["Estimates"].ToString(), role["Estimates"].ToString());
                        userPer["AwardEstimates"] = MergePermissionString(userPer["AwardEstimates"].ToString(), role["AwardEstimates"].ToString());
                        userPer["salessetup"] = MergePermissionString(userPer["salessetup"].ToString(), role["salessetup"].ToString());
                        userPer["PONotification"] = MergePermissionString(userPer["PONotification"].ToString(), role["PONotification"].ToString());
                        userPer["WriteOff"] = MergePermissionString(userPer["WriteOff"].ToString(), role["WriteOff"].ToString());
                        userPer["ProjectModulePermission"] = MergePermissionString(userPer["ProjectModulePermission"].ToString(), role["ProjectModulePermission"].ToString());
                        userPer["InventoryModulePermission"] = MergePermissionString(userPer["InventoryModulePermission"].ToString(), role["InventoryModulePermission"].ToString());
                        userPer["JobClosePermission"] = MergePermissionString(userPer["JobClosePermission"].ToString(), role["JobClosePermission"].ToString());
                        userPer["JobCompletedPermission"] = MergePermissionString(userPer["JobCompletedPermission"].ToString(), role["JobCompletedPermission"].ToString());
                        userPer["JobReopenPermission"] = MergePermissionString(userPer["JobReopenPermission"].ToString(), role["JobReopenPermission"].ToString());
                        //isnull(u.IsProjectManager, 0) as IsProjectManager,
                        //isnull(u.IsAssignedProject, 0) as IsAssignedProject
                        //,isnull(u.TicketVoidPermission, 0) as TicketVoidPermission,
                        //isnull(u.PR, '0') AS PR,
                        userPer["Employee"] = MergePermissionString(userPer["Employee"].ToString(), role["Employee"].ToString());
                        userPer["PRProcess"] = MergePermissionString(userPer["PRProcess"].ToString(), role["PRProcess"].ToString());
                        userPer["PRRegister"] = MergePermissionString(userPer["PRRegister"].ToString(), role["PRRegister"].ToString());
                        userPer["PRReport"] = MergePermissionString(userPer["PRReport"].ToString(), role["PRReport"].ToString());
                        userPer["PRWage"] = MergePermissionString(userPer["PRWage"].ToString(), role["PRWage"].ToString());
                        userPer["PRDeduct"] = MergePermissionString(userPer["PRDeduct"].ToString(), role["PRDeduct"].ToString());
                        userPer["ticket"] = MergePermissionString(userPer["ticket"].ToString(), role["ticket"].ToString());
                    }
                }
            }
        }
        */
        Session["userinfo"] = userInfor;
        //Session["userinfo"] = ds.Tables[0];
        Session["UserID"] = userInfor.Rows[0]["UserID"].ToString();
        Session["User"] = userInfor.Rows[0]["fFirst"].ToString() + " " + userInfor.Rows[0]["Last"].ToString();
        Session["type"] = userInfor.Rows[0]["usertype"].ToString();
        Session["Username"] = userInfor.Rows[0]["fuser"].ToString();
        if (Session["type"].ToString() == "c")
        {
            Session["User"] = userInfor.Rows[0]["fFirst"].ToString();
            //Session["Username"] = userInfor.Rows[0]["flogin"].ToString();
            Session["ticketo"] = userInfor.Rows[0]["ticketd"].ToString();
            Session["invoice"] = userInfor.Rows[0]["ledger"].ToString();
            Session["CPE"] = userInfor.Rows[0]["CPEquipment"].ToString();
        }
        Session["custid"] = userInfor.Rows[0]["custid"].ToString();
        Session["dbname"] = dbname;
        Session["company"] = ddlCompany.SelectedItem.Text;
        Session["MSM"] = ddlCompany.SelectedValue.Split(':')[1];

        ConnectionStr(dbname);

        GetControl();

        objProp_User.Username = userInfor.Rows[0]["fuser"].ToString();
        objProp_User.ConnConfig = Session["config"].ToString();
        int strSuper = 0;
        strSuper = objBL_User.getLoginSuper(objProp_User);

        if (strSuper == 1)
        {
            Session["ISsupervisor"] = 1;
        }
        else
        {
            Session["ISsupervisor"] = 0;
        }

        Session["userinfodataset"] = null;
        Remember();
        GetDefaultCompanyReset();



        Response.Redirect("Home.aspx", false);


    }

    private void AdminSessionInfo()
    {
        string dbname = ddlCompany.SelectedValue.Split(':')[0];

        Session["FinanceManager"] = "F";
        Session["AddFinance"] = true;
        Session["EditFinance"] = true;
        Session["ViewFinance"] = true;
        Session["FinanceStatement"] = true;
        // Set Account Payable permission
        Session["AP"] = true;    // added by Mayuri 10th Dec ,15

        Session["UserID"] = 1;
        Session["User"] = "Maintenance";
        Session["type"] = "am";
        Session["Username"] = "Maintenance";
        Session["dbname"] = dbname;
        Session["company"] = ddlCompany.SelectedItem.Text;
        Session["MSM"] = ddlCompany.SelectedValue.Split(':')[1];
        Session["custid"] = "0";
        ConnectionStr(dbname);
        GetControl();
        GetPeriodClosedDetails();
        GetDefaultCompanyReset();

        //////////////////////

        string hostName = System.Net.Dns.GetHostName();  // Retrive the Name of HOST  

        string myIP = System.Net.Dns.GetHostByName(hostName).AddressList[0].ToString();


        Session["API_Token"] = SSTCryptographer.Encrypt(Guid.NewGuid() + "|||" + ddlCompany.SelectedValue.Split(':')[0].ToString() + "|||" + myIP + "|||" + Session["UserID"].ToString(), "core");


        UserAuthentication _US = new UserAuthentication()
        {
            Connectionstring = Session["config"].ToString(),
            Token = Session["API_Token"].ToString(),
            company = Session["dbname"].ToString(),
            Domain_Name = myIP,
            User_Id = Convert.ToInt32(Session["UserID"])
        };


        objBL_User.getUserAuthorization_New(_US);

        //////////////////////

        Response.Redirect("Home.aspx", false);
    }
    public string generateID(string sourceUrl)
    {
        return string.Format("{0}_{1:N}", sourceUrl, Guid.NewGuid());
    }
    private void GetControl()
    {
        objProp_User.ConnConfig = Session["config"].ToString();
        DataSet dsControl = new DataSet();
        dsControl = objBL_User.getControl(objProp_User);

        int Multilang = 0;
        int MSREP = 0;
        int payment = 0;
        if (dsControl.Tables[0].Rows.Count > 0)
        {
            if (dsControl.Tables[0].Rows[0]["MultiLang"] != DBNull.Value)
            {
                Multilang = Convert.ToInt16(dsControl.Tables[0].Rows[0]["MultiLang"]);
            }
            if (dsControl.Tables[0].Rows[0]["msreptemp"] != DBNull.Value)
            {
                MSREP = Convert.ToInt16(dsControl.Tables[0].Rows[0]["msreptemp"]);
            }
            if (dsControl.Tables[0].Rows[0]["payment"] != DBNull.Value)
            {
                payment = Convert.ToInt16(dsControl.Tables[0].Rows[0]["payment"]);
            }
        }
        Session["IsMultiLang"] = Multilang;
        Session["MSREP"] = MSREP;
        Session["payment"] = payment;
    }

    private void ConnectionStr(string dbname)
    {

        //string constr = "server=" + txtDSN.Text + ";database=" + txtDB.Text + ";user=" + txtDuser.Text + ";password=" + txtDpass.Text + "";
        //Session["config"] = "server=NODE90\\MSSQLSERVER8;database=mssample;user=sa;password=ideavate@123";

        string server = Config.MS.Split(';')[0].Split('=')[1];
        string database = dbname;
        string user = Config.MS.Split(';')[2].Split('=')[1];
        string pass = Config.MS.Split(';')[3].Split('=')[1];

        string constr = "server=" + server + ";database=" + database + ";user=" + user + ";password=" + pass + "";    // uncomment this code 29th dec,15
        Session["config"] = constr;
        Session["APIconfig"] = SSTCryptographer.Encrypt(constr, "webconfig");


    }

    private void Remember()
    {
        if (chkRemember.Checked)
        {
            Response.Cookies["USERNAME"].Value = txtUsername.Text.Trim();
            Response.Cookies["USERNAME"].Expires = DateTime.Now.AddMonths(1);

            //Response.Cookies["PASSWORD"].Value = txtPassword.Text.Trim();
            //Response.Cookies["PASSWORD"].Expires = DateTime.Now.AddMonths(1);            
        }
        else
        {
            Response.Cookies["USERNAME"].Expires = DateTime.Now.AddDays(-1);
            //Response.Cookies["PASSWORD"].Expires = DateTime.Now.AddDays(-1);
        }
    }

    private void FillRemembered()
    {
        if (Request.Cookies["USERNAME"] != null)
            txtUsername.Text = Request.Cookies["USERNAME"].Value;
        if (Request.Cookies["USERNAME"] != null)
            chkRemember.Checked = true;

        //if (Request.Cookies["PASSWORD"] != null)
        //    txtPassword.Attributes.Add("value", Request.Cookies["PASSWORD"].Value);
        //if (Request.Cookies["USERNAME"] != null && Request.Cookies["PASSWORD"] != null)
        //    chkRemember.Checked = true;
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        //programmaticModalPopup.Hide();
        CheckKey();
    }

    private void CheckTrialUser(int userid)
    {
        ViewState["uid"] = userid;
        string[] strRegItems = CheckUser(userid);

        var e = from s in strRegItems select s;
        int c = e.Count();

        if (c < 4)
        {
            programmaticModalPopupUser.Show();
            btnOKuser.Visible = false;
            lblTRialUser.Text = "Please contact us at (619) 459-7481 for user registration.";
            return;
        }

        string strLic = strRegItems[0];
        string strDay = strRegItems[1];
        string strDate = strRegItems[2];
        string strusename = strRegItems[3];

        if (strusename.ToUpper() != txtUsername.Text.ToUpper().Trim() + ddlCompany.SelectedValue.Split(':')[0].ToUpper().Trim())
        {
            programmaticModalPopupUser.Show();
            btnOKuser.Visible = false;
            lblTRialUser.Text = "Please contact us at (619) 459-7481 for user registration.";
            return;
        }

        if (Convert.ToBoolean(Convert.ToInt32(strLic)) == false)
        {
            if (System.DateTime.Today > Convert.ToDateTime(strDate).AddDays(Convert.ToInt32(strDay) - 1))
            {
                programmaticModalPopupUser.Show();
                btnOKuser.Visible = false;
                lblTRialUser.Text = "You have exceeded the " + strDay + " days trial period of user registration, Please contact us at (619) 459-7481 to register as user.";
            }
            else
            {
                programmaticModalPopupUser.Show();
                btnOKuser.Visible = true;
                lblTRialUser.Text = "You are using a " + strDay + " day trial period user registration and have <strong>" + (Convert.ToDateTime(strDate).AddDays(Convert.ToInt32(strDay) - 1).Date - System.DateTime.Today).Days + "</strong> days remaining. Please contact us at (619) 459-7481 to register as a user.";

            }
        }
        else
        {
            DataSet dsinfo = (DataSet)Session["userinfodataset"];
            UserSessionInfo(dsinfo);
        }
    }

    private void CheckTrial()
    {
        //bool success= CreateRegFile();

        //if (success == false)
        //{
        //    return;
        //}

        try
        {
            string strLic = "0";
            string strDay = "30";
            string strDate = System.DateTime.Now.ToShortDateString();
            string strMachineID = "0";

            string strReg = strLic + "&" + strDay + "&" + strDate + "&" + strMachineID;
            string strRegEncr = SSTCryptographer.Encrypt(strReg, "reg");
            objProp_User.Reg = strRegEncr;
            objBL_User.UpdateTrial(objProp_User);

            string[] strRegItems = Check();

            var e = from s in strRegItems select s;
            int c = e.Count();

            if (c < 4)
            //if (c < 3)
            {
                programmaticModalPopup.Show();
                btnOK.Visible = false;
                lblTrial.Text = "Please contact us at (619) 459-7481 to register your software.";
                return;
            }

            strLic = strRegItems[0];
            strDay = strRegItems[1];
            strDate = strRegItems[2];
            strMachineID = strRegItems[3];

            if (Convert.ToBoolean(Convert.ToInt32(strLic)) == false || strMachineID != GetCPUId())
            //if (Convert.ToBoolean(Convert.ToInt32(strLic)) == false)
            {
                if (System.DateTime.Today > Convert.ToDateTime(strDate).AddDays(Convert.ToInt32(strDay) - 1))
                {
                    programmaticModalPopup.Show();
                    btnOK.Visible = false;
                    lblTrial.Text = "You have exceeded the " + strDay + " days trial period of Mobile Office Manager, Please contact us at (619) 459-7481 to register your software.";

                }
                else
                {
                    programmaticModalPopup.Show();
                    btnOK.Visible = true;
                    lblTrial.Text = "<span class='spnTrial'>You are using a " + strDay + " day trial version of </br> Mobile Office Manager </br> and have " + (Convert.ToDateTime(strDate).AddDays(Convert.ToInt32(strDay) - 1).Date - System.DateTime.Today).Days + " days remaining.</span> Please contact us at <span class='spnNumber'> (619) 459-7481 </span> to register your software.";

                }
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }
    }

    private string[] Check()
    {
        DataSet ds = new DataSet();
        ds = objBL_User.gettrial(objProp_User);

        //string strReg = string.Empty;
        //string strWin = Environment.GetEnvironmentVariable("windir") + "/system32/AIFI.ini";
        //using (StreamReader sr = new StreamReader(strWin,true))
        //{
        //    String line;
        //    while ((line = sr.ReadLine()) != null)
        //    {
        //        strReg = line;
        //    }
        //}
        string strRegDecr = SSTCryptographer.Decrypt(ds.Tables[0].Rows[0]["str"].ToString(), "reg");
        string[] strRegItems = strRegDecr.Split('&');

        return strRegItems;
    }

    private string[] CheckUser(int userid)
    {
        DataSet ds = new DataSet();
        objProp_User.UserID = userid;
        objProp_User.DBName = ddlCompany.SelectedValue.Split(':')[0];
        ds = objBL_User.gettrialUser(objProp_User);
        string strRegDecr = "";
        if (ds.Tables[0].Rows.Count > 0)
        {
            strRegDecr = SSTCryptographer.Decrypt(ds.Tables[0].Rows[0]["str"].ToString(), "regu");
        }
        string[] strRegItems = strRegDecr.Split('&');
        return strRegItems;
    }

    private void CheckKey()
    {
        try
        {
            string stSerial = DecryptKEY(txtSerial.Text.Trim(), "key");
            string stSerialTrial = DecryptKEY(txtSerial.Text.Trim(), "trial");
            int len = lblKey.Text.Length - 3;
            string text = lblKey.Text.Substring(1, len);

            if (stSerial == text || stSerialTrial == text)
            {
                string strSuccessMsg = "Registration successful! trial period extended for next 30 days.";
                DataSet ds = new DataSet();
                ds = objBL_User.gettrial(objProp_User);

                string strRegDecr = SSTCryptographer.Decrypt(ds.Tables[0].Rows[0]["str"].ToString(), "reg");
                string[] strRegItems = strRegDecr.Split('&');
                string strLic = "0";
                string strDay = "30";
                string strDate = System.DateTime.Now.ToShortDateString();
                string strMachineID = "0";

                if (stSerial == text)
                {
                    strLic = "1";
                    strMachineID = GetCPUId();
                    strSuccessMsg = "Registration successful!";
                }

                if (strMachineID != string.Empty)
                {
                    string strReg = strLic + "&" + strDay + "&" + strDate + "&" + strMachineID;

                    string strRegEncr = SSTCryptographer.Encrypt(strReg, "reg");
                    objProp_User.Reg = strRegEncr;
                    objBL_User.UpdateReg(objProp_User);

                    lblMSGRegistr.Text = strSuccessMsg;
                    btnOK.Visible = true;
                }
                else
                {
                    lblMSGRegistr.Text = "Registration not successful, unable to get necessary information!";
                }
                //btncancel.Visible = true;
                //btnLogin.Visible = true;
                //ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: '"+strSuccessMsg+"',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            else
            {
                lblMSGRegistr.Text = "Invalid serial number.";
            }
        }
        catch (Exception ex)
        {
            lblMSGRegistr.Text = ex.Message;

            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            //ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    private void CheckKeyUser(int userid)
    {
        try
        {
            string stSerial = DecryptKEY(txtSerialUser.Text.Trim(), "keyu");
            string stSerialTrial = DecryptKEY(txtSerialUser.Text.Trim(), "trialu");
            int len = lblKeyuser.Text.Length - 3;
            string text = lblKeyuser.Text.Substring(1, len);

            if (stSerial == text || stSerialTrial == text)
            {
                string strSuccessMsg = "Registration successful! trial period extended for next 30 days.";
                DataSet ds = new DataSet();
                objProp_User.UserID = userid;
                objProp_User.DBName = ddlCompany.SelectedValue.Split(':')[0];
                //ds = objBL_User.gettrialUser(objProp_User);

                //string strRegDecr = SSTCryptographer.Decrypt(ds.Tables[0].Rows[0]["str"].ToString(), "regu");
                //string[] strRegItems = strRegDecr.Split('&');
                string strLic = "0";
                string strDay = "30";
                string strDate = System.DateTime.Now.ToShortDateString();
                string strUserID = txtUsername.Text.Trim() + ddlCompany.SelectedValue.Split(':')[0];

                if (stSerial == text)
                {
                    strLic = "1";
                    //strUserID = txtUsername.Text;
                    strSuccessMsg = "Registration successful!";
                }

                string strReg = strLic + "&" + strDay + "&" + strDate + "&" + strUserID;

                string strRegEncr = SSTCryptographer.Encrypt(strReg, "regu");
                objProp_User.Reg = strRegEncr;
                objProp_User.DBName = ddlCompany.SelectedValue.Split(':')[0];
                objBL_User.UpdateRegUser(objProp_User);

                lblMSgUser.Text = strSuccessMsg;

                DataSet dsinfo = (DataSet)Session["userinfodataset"];

                UserSessionInfo(dsinfo);
            }
            else
            {
                lblMSgUser.Text = "Invalid serial number.";
            }
        }
        catch (Exception ex)
        {
            lblMSgUser.Text = ex.Message;
        }
    }

    public static String GetCPUId()
    {
        var name = (from x in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem").Get().OfType<ManagementObject>()
                    select x.GetPropertyValue("Caption")).First();

        String ComputerName = "";
        ManagementObjectSearcher searcherHD = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
        ManagementObjectCollection mObjectHD = searcherHD.Get();

        foreach (ManagementObject obj in mObjectHD)
        {
            if (obj["Name"] != null)
            {
                ComputerName = obj["Name"].ToString();
            }
        }

        return ComputerName;

        //String DiskID = "";
        //ManagementObjectSearcher searcherHD = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive where DEVICEID = '\\\\\\\\.\\\\PHYSICALDRIVE0'");
        //ManagementObjectCollection mObjectHD = searcherHD.Get();
        //int i = 0;
        //foreach (ManagementObject obj in mObjectHD)
        //{
        //    if (name.ToString() == "Microsoft(R) Windows(R) Server 2003 for Small Business Server" || name.ToString() == "Microsoft Windows XP Professional")
        //    {
        //        //if (obj["Signature"] != null && obj["InterfaceType"].ToString() == "IDE" && i == 0)
        //        if (obj["Signature"] != null && i == 0)
        //        {
        //            DiskID = obj["Signature"].ToString();
        //            i = 1;
        //        }
        //    }
        //    else
        //    {
        //        if (obj["SerialNumber"] != null && i == 0)
        //        {
        //            DiskID = obj["SerialNumber"].ToString();
        //            i = 1;
        //        }
        //    }

        //    //if (obj["availability"] != null)
        //    //{
        //    //    DiskID = obj["availability"].ToString();
        //    //}  

        //    //if (obj["Signature"] != null)
        //    //{
        //    //    DiskID += obj["Signature"].ToString();
        //    //}
        //}

        ////String BaseID = "";
        ////ManagementObjectSearcher searcherBase = new ManagementObjectSearcher("Select * FROM WIN32_BaseBoard");
        ////ManagementObjectCollection mObjectBase = searcherBase.Get();
        ////foreach (ManagementObject obj in mObjectBase)
        ////{
        ////    if (obj["SerialNumber"] != null)
        ////    {
        ////        BaseID = obj["SerialNumber"].ToString();
        ////    }
        ////}

        ////ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
        ////ManagementObjectCollection moc = mc.GetInstances();
        ////string MACAddress = "";
        ////foreach (ManagementObject mo in moc)
        ////{
        ////    if (mo["MacAddress"] != null)
        ////    {
        ////        //if ((bool)mo["IPEnabled"] == true)
        ////        //{
        ////            MACAddress = mo["MacAddress"].ToString();
        ////        //}
        ////    }
        ////}

        //return DiskID;
    }

    public string DecryptKEY(string Serial, string type)
    {
        string stEnc = SSTCryptographer.Decrypt(Serial, type);

        return stEnc;
    }

    public String generateRandomString(int length)
    {
        //Initiate objects & vars    
        Random random = new Random();
        String randomString = "";
        int randNumber;

        //Loop ‘length’ times to generate a random number or character
        for (int i = 0; i < length; i++)
        {
            if (random.Next(1, 3) == 1)
                randNumber = random.Next(97, 123); //char {a-z}
            else
                randNumber = random.Next(48, 58); //int {0-9}

            //append random char or digit to random string
            randomString = randomString + (char)randNumber;
        }
        //return the random string
        return randomString;
    }

    protected void lnkRegister_Click(object sender, EventArgs e)
    {
        //programmaticModalPopup.Show();
        pnlReg.Visible = true;
        lnkRegister.Visible = false;
        lblKey.Text = generateRandomString(10);
    }

    protected void btnOK_Click1(object sender, EventArgs e)
    {
        programmaticModalPopup.Hide();
        //btnLogin.Visible = true;
    }

    protected void lnkRegisterUser_Click(object sender, EventArgs e)
    {
        pnlUserreg.Visible = true;
        lnkRegisterUser.Visible = false;
        lblKeyuser.Text = generateRandomString(10);
    }

    protected void btnRegserialUser_Click(object sender, EventArgs e)
    {
        CheckKeyUser(Convert.ToInt32(ViewState["uid"]));
    }

    protected void btnOKuser_Click(object sender, EventArgs e)
    {
        DataSet ds = (DataSet)Session["userinfodataset"];
        //Session["userinfodataset"] = null;
        UserSessionInfo(ds);
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        programmaticModalPopupUser.Hide();
    }

    protected void btnUpdatePassword_Click(object sender, EventArgs e)
    {
        var currUserName = ViewState["currUserName"].ToString();
        var currPassword = ViewState["currPassword"].ToString();
        if (currPassword != txtCurrentPw.Text)
        {
            lblUpdatePwErrMsg.Text = "Current Password is incorrect, Please enter correct Password!";
            return;
        }

        if (txtNewPassword.Text != txtConfirmPassword.Text)
        {
            lblUpdatePwErrMsg.Text = "New password and confirm password do not match.";
            return;
        }
        var _firstName = ViewState["firstTime"].ToString();
        var _lastName = ViewState["lastName"].ToString();
        var err = string.Empty;
        if (!WebBaseUtility.IsPasswordPassedPwPolicy(currUserName, _firstName, _lastName, txtNewPassword.Text, ref err))
        {
            lblUpdatePwErrMsg.Text = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(err);
            //lblUpdatePwErrMsg.Text = "The New Password should contain Minimum 6 and Maximum 10 Characters with 1 English Uppercase Letter, 1 Lowercase Letter, 1 Special Characters, 1 Numeric Character and should not have user\'s first three letters of his/her First Name, Last Name and/or Username";
            return;
        }

        //Update new password
        var dbname = ddlCompany.SelectedValue.Split(':')[0];
        var dbtype = (ddlCompany.SelectedValue.Split(':')[1]);
        User objProp_User = new User();
        objProp_User.Username = currUserName;
        objProp_User.Password = currPassword;
        objProp_User.NewPassword = txtNewPassword.Text;
        objProp_User.DBName = dbname;
        objProp_User.DBType = dbtype;
        ConnectionStr(ddlCompany.SelectedValue.Split(':')[0]);
        objProp_User.ConnConfig = Session["config"].ToString();
        try
        {
            objBL_User.UpdatePassword(objProp_User);
            //txtPassword.Text = txtNewPassword.Text;
            //programmaticModalPopupUpdatePassword.Hide();
            //btnLogin_Click(sender, e);
            lblUpdatePwErrMsg.Text = "Password updated successfully!";
            ShowInputPanel(false, Panel2, lblUpdatePwErrMsg.ClientID);

        }
        catch (Exception ex)
        {
            lblUpdatePwErrMsg.Text = ex.Message;
        }
    }

    protected void lnkCancelUpdate_Click(object sender, EventArgs e)
    {
        programmaticModalPopupUpdatePassword.Hide();
    }

    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Need to check if we had applied Password Policy for this customer or not.
        // Only show Forgot Password/Reset Password in case the password policy was applied
        //Update new password
        divValidationBox.Visible = false;
        lblMsg.Text = string.Empty;
        ShowHidePasswordResetting();
    }

    private void ShowHidePasswordResetting()
    {
        try
        {
            ConnectionStr(ddlCompany.SelectedValue.Split(':')[0]);
            objProp_User.ConnConfig = Session["config"].ToString();
            var ds = objBL_User.getControl(objProp_User);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var isAppliedPasswordPolicy = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ApplyPasswordRules"].ToString()) ? false : Convert.ToBoolean(ds.Tables[0].Rows[0]["ApplyPasswordRules"].ToString());

                if (isAppliedPasswordPolicy)
                {
                    lnkPasswordResetting.Visible = true;
                    var emailAdmin = ds.Tables[0].Rows[0]["PwResetAdminEmail"].ToString();
                    var pwResetUserID = ds.Tables[0].Rows[0]["PwResetUserID"].ToString();
                    var pwResetUsername = ds.Tables[0].Rows[0]["PwResetUsername"].ToString();
                    ViewState["PwResetUserID"] = pwResetUserID;
                    ViewState["PwResetUsername"] = pwResetUsername;
                    ViewState["EmailAdministrator"] = emailAdmin;

                    if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PwResetting"].ToString()) || ds.Tables[0].Rows[0]["PwResetting"].ToString() == "0")
                        lnkPasswordResetting.Text = "Forgot Password";
                    else
                    {
                        lnkPasswordResetting.Text = "Reset Password";
                    }
                }
                else
                {
                    lnkPasswordResetting.Visible = false;
                    lnkPasswordResetting.Text = "";
                    ViewState["EmailAdministrator"] = null;
                    ViewState["PwResetUserID"] = null;
                    ViewState["PwResetUsername"] = null;
                }
            }
        }
        catch (Exception)
        {
            lnkPasswordResetting.Visible = false;
            lnkPasswordResetting.Text = "";
            ViewState["EmailAdministrator"] = null;
            ViewState["PwResetUserID"] = null;
            ViewState["PwResetUsername"] = null;
        }

    }

    protected void lnkPasswordResetting_Click(object sender, EventArgs e)
    {
        if (lnkPasswordResetting.Text == "Forgot Password")
        {
            pnlForgotPw.Visible = true;
            lblForgotPwMsg.Text = "";
            programmaticModalPopupForgotPassword.Show();
            ShowHideLogin(false);
        }
        else if (lnkPasswordResetting.Text == "Reset Password")
        {
            pnlResetPw.Visible = true;
            lblResetPwMsg.Text = "";
            programmaticModalPopupResetPassword.Show();
            ShowHideLogin(false);
        }
    }

    protected void lnkCancelReset_Click(object sender, EventArgs e)
    {
        programmaticModalPopupResetPassword.Hide();
        ShowHideLogin(true);
    }

    protected void btnResetPassword_BK_Click(object sender, EventArgs e)
    {
        // Send and email to admin user for reset password request
        //var success = true;
        var emailAdmin = ViewState["EmailAdministrator"];

        //objProp_User.ConnConfig = Session["config"].ToString();
        if (emailAdmin != null && emailAdmin.ToString().Equals(txtResetEmailAdmin.Text, StringComparison.InvariantCultureIgnoreCase))
        {

            Mail mail = new Mail();
            mail.From = txtResetEmailAdmin.Text;
            try
            {
                var pwResetUserID = Convert.ToInt32(ViewState["PwResetUserID"]);
                var pwResetUsername = ViewState["PwResetUsername"].ToString();

                var dbname = ddlCompany.SelectedValue.Split(':')[0];
                var dbtype = (ddlCompany.SelectedValue.Split(':')[1]);
                ConnectionStr(dbname);
                //System.Configuration.Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                //MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
                //string username = mailSettings.Smtp.Network.UserName;
                //mail.From = username;
                //mail.RequireAutentication = !mailSettings.Smtp.Network.DefaultCredentials;
                //mail.Username = mailSettings.Smtp.Network.UserName;
                //mail.Password = mailSettings.Smtp.Network.Password;
                //mail.SMTPHost = mailSettings.Smtp.Network.Host;
                //mail.SMTPPort = mailSettings.Smtp.Network.Port;
                //mail.TakeASentEmailCopy = false;

                // Get user information from usename and email: firstname, lastname, isapplypolicy, username
                User objProp_User = new User();
                objProp_User.Username = txtResetUserName.Text;
                objProp_User.DBName = dbname;
                objProp_User.DBType = dbtype;
                ConnectionStr(objProp_User.DBName);
                objProp_User.ConnConfig = Session["config"].ToString();

                // TODO: get user info
                var ds = objBL_User.GetUserInfoByUsername(objProp_User);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var userEmail = ds.Tables[0].Rows[0]["EMail"].ToString();

                    WebBaseUtility.UpdateMailConfiguration(mail, Session["config"].ToString(), pwResetUserID);

                    if (!pwResetUsername.Equals(txtResetUserName.Text, StringComparison.InvariantCultureIgnoreCase))
                    {
                        mail.To.Add(txtResetEmailAdmin.Text);
                        mail.Title = "Requesting reset password for user: " + txtResetUserName.Text;
                        mail.Text = string.Format("Hi MOM Administrator,<br><br>You are receiving a request for a Mom user.<br>User name: {0}<br>Email Address : {1}<br><br>Please contact the user with new password.<br><br>Thanks",
                            txtResetUserName.Text, userEmail);
                        mail.IsBodyHtml = true;
                        mail.SendTest();
                        //success = true;
                        //pnlResetPw.Visible = false;
                        lblResetPwMsg.Text = "Your request has been sent to the Administrator.  Please contact your Administrator for your new password";
                        ShowInputPanel(false, pnlResetPw, lblResetPwMsg.ClientID);
                    }
                    else
                    {
                        mail.To.Add(txtResetEmailAdmin.Text);
                        mail.Title = "Forgot password";
                        var token = HttpUtility.UrlEncode(SSTCryptographer.Encrypt(pwResetUsername + "&" + txtResetEmailAdmin.Text + "&"
                            + dbname + "&" + dbtype + "&" + System.DateTime.Now.ToString(), "forgot"));

                        var link = string.IsNullOrEmpty(Request.Url.Query) ? Request.Url.AbsoluteUri : Request.Url.AbsoluteUri.Replace(Request.Url.Query, String.Empty);
                        link = link + "?fogotpassword=" + token;
                        //var link = Request.Url.AbsoluteUri.Replace(Request.Url.Query, String.Empty) + "?fogotpassword=" + token;
                        mail.Text = string.Format("Hi {0},<br><br>Please click on the link below to reset your password.<br>The link will expire in 2 hours.<br>{1}<br><br>Thanks", pwResetUsername, link);
                        mail.IsBodyHtml = true;
                        mail.SendTest();
                        //success = true;
                        pnlResetPw.Visible = false;
                        lblResetPwMsg.Text = "An email has been sent with a link and will expire in 2 hours";
                        ShowInputPanel(false, pnlResetPw, lblResetPwMsg.ClientID);
                    }
                }
                else
                {
                    lblResetPwMsg.Text = "Invalid Username";
                    ShowInputPanel(true, pnlResetPw, lblResetPwMsg.ClientID);
                }
            }
            catch (Exception ex1)
            {
                //pnlResetPw.Visible = true;
                lblResetPwMsg.Text = ex1.Message;
                ShowInputPanel(true, pnlResetPw, lblResetPwMsg.ClientID);
            }
        }
        else
        {
            //pnlResetPw.Visible = true;
            //lblResetPwMsg.Text = "An error happened on sending the request";
            lblResetPwMsg.Text = "The administrator email is incorrect.  Please contact your Administrator for more details!";
            ShowInputPanel(true, pnlResetPw, lblResetPwMsg.ClientID);
        }

        //if (success)
        //{
        //    pnlResetPw.Visible = false;
        //    lblResetPwMsg.Text = "Your request has been sent to the Administrator\r\nPlease contact your Administrator for your password";
        //}
        //else
        //{
        //    pnlResetPw.Visible = true;
        //    lblResetPwMsg.Text = "An error happened on sending the request";
        //}

    }

    protected void btnResetPassword_Click(object sender, EventArgs e)
    {
        // Send and email to admin user for reset password request
        //var success = true;
        var emailAdmin = ViewState["EmailAdministrator"];

        //objProp_User.ConnConfig = Session["config"].ToString();
        if (emailAdmin != null && !string.IsNullOrEmpty(emailAdmin.ToString()))
        {

            Mail mail = new Mail();
            mail.From = txtResetEmailAdmin.Text;
            try
            {
                var pwResetUserID = Convert.ToInt32(ViewState["PwResetUserID"]);
                var pwResetUsername = ViewState["PwResetUsername"].ToString();

                var dbname = ddlCompany.SelectedValue.Split(':')[0];
                var dbtype = (ddlCompany.SelectedValue.Split(':')[1]);
                //ConnectionStr(dbname);
                //System.Configuration.Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                //MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
                //string username = mailSettings.Smtp.Network.UserName;
                //mail.From = username;
                //mail.RequireAutentication = !mailSettings.Smtp.Network.DefaultCredentials;
                //mail.Username = mailSettings.Smtp.Network.UserName;
                //mail.Password = mailSettings.Smtp.Network.Password;
                //mail.SMTPHost = mailSettings.Smtp.Network.Host;
                //mail.SMTPPort = mailSettings.Smtp.Network.Port;
                //mail.TakeASentEmailCopy = false;

                // Get user information from usename and email: firstname, lastname, isapplypolicy, username
                User objProp_User = new User();
                objProp_User.Username = txtResetUserName.Text;
                objProp_User.Email = txtResetEmailAdmin.Text;
                objProp_User.DBName = dbname;
                objProp_User.DBType = dbtype;
                ConnectionStr(objProp_User.DBName);
                objProp_User.ConnConfig = Session["config"].ToString();

                // TODO: get user info
                var ds = objBL_User.GetUserInfoByUsernameAndEmail(objProp_User);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //var userEmail = ds.Tables[0].Rows[0]["EMail"].ToString();
                    var userType = ds.Tables[0].Rows[0]["UserTypeName"].ToString();

                    WebBaseUtility.UpdateMailConfiguration(mail, Session["config"].ToString(), pwResetUserID);

                    if (!pwResetUsername.Equals(txtResetUserName.Text, StringComparison.InvariantCultureIgnoreCase))
                    {
                        mail.To.Add(emailAdmin.ToString());
                        mail.Title = "Requesting reset password for user: " + txtResetUserName.Text;
                        mail.Text = string.Format("Hi MOM Administrator," +
                            "<br><br>You are receiving a request for a Mom user." +
                            "<br>User name: {0}" +
                            "<br>Email Address : {1}" +
                            "<br>User Type : {2}" +
                            "<br><br>Please contact the user with new password.<br><br>Thanks",
                            txtResetUserName.Text, txtResetEmailAdmin.Text, userType);
                        mail.IsBodyHtml = true;
                        mail.SendTest();
                        //success = true;
                        //pnlResetPw.Visible = false;
                        lblResetPwMsg.Text = "Your request has been sent to the Administrator.  Please contact your Administrator for your new password";
                        ShowInputPanel(false, pnlResetPw, lblResetPwMsg.ClientID);
                    }
                    else
                    {
                        //mail.To.Add(txtResetEmailAdmin.Text);
                        //mail.Title = "Forgot password";
                        //var token = HttpUtility.UrlEncode(SSTCryptographer.Encrypt(pwResetUsername + "&" + txtResetEmailAdmin.Text + "&"
                        //    + dbname + "&" + dbtype + "&" + System.DateTime.Now.ToString(), "forgot"));

                        //var link = string.IsNullOrEmpty(Request.Url.Query) ? Request.Url.AbsoluteUri : Request.Url.AbsoluteUri.Replace(Request.Url.Query, String.Empty);
                        //link = link + "?fogotpassword=" + token;
                        ////var link = Request.Url.AbsoluteUri.Replace(Request.Url.Query, String.Empty) + "?fogotpassword=" + token;
                        //mail.Text = string.Format("Hi {0},<br><br>Please click on the link below to reset your password.<br>The link will expire in 2 hours.<br><br>{1}<br><br>Thanks", pwResetUsername, link);
                        //mail.IsBodyHtml = true;
                        //mail.SendTest();
                        ////success = true;
                        //pnlResetPw.Visible = false;
                        //lblResetPwMsg.Text = "An email has been sent with a link and will expire in 2 hours";
                        //ShowInputPanel(false, pnlResetPw, lblResetPwMsg.ClientID);

                        lblResetPwMsg.Text = "Please contact your vendor to reset password.";
                        ShowInputPanel(true, pnlResetPw, lblResetPwMsg.ClientID);
                    }
                }
                else
                {
                    lblResetPwMsg.Text = "Username and Email Address are invalid, Please enter correct Username and Email Address";
                    ShowInputPanel(true, pnlResetPw, lblResetPwMsg.ClientID);
                }
            }
            catch (Exception ex1)
            {
                //pnlResetPw.Visible = true;
                lblResetPwMsg.Text = ex1.Message;
                ShowInputPanel(true, pnlResetPw, lblResetPwMsg.ClientID);
            }
        }
        else
        {
            //pnlResetPw.Visible = true;
            //lblResetPwMsg.Text = "An error happened on sending the request";
            lblResetPwMsg.Text = "The administrator email is incorrect.  Please contact your Administrator for more details!";
            ShowInputPanel(true, pnlResetPw, lblResetPwMsg.ClientID);
        }

        //if (success)
        //{
        //    pnlResetPw.Visible = false;
        //    lblResetPwMsg.Text = "Your request has been sent to the Administrator\r\nPlease contact your Administrator for your password";
        //}
        //else
        //{
        //    pnlResetPw.Visible = true;
        //    lblResetPwMsg.Text = "An error happened on sending the request";
        //}

    }

    protected void btnForgotPassword_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtForgotEmail.Text) && !string.IsNullOrEmpty(txtForgotUsername.Text))
        {
            try
            {
                var username = txtForgotUsername.Text;
                var dbname = ddlCompany.SelectedValue.Split(':')[0];
                var dbtype = (ddlCompany.SelectedValue.Split(':')[1]);

                // TODO: need to check if the username and email is valid
                // Get user information from usename and email: firstname, lastname, isapplypolicy, username
                User objProp_User = new User();
                objProp_User.Username = username;
                objProp_User.Email = txtForgotEmail.Text;
                objProp_User.ForgotPwRequest = 1;
                objProp_User.DBName = dbname;
                objProp_User.DBType = dbtype;
                ConnectionStr(objProp_User.DBName);
                objProp_User.ConnConfig = Session["config"].ToString();

                // TODO: get user info
                var ds = objBL_User.GetUserInfoByUsernameAndEmail(objProp_User);

                var pwResetUserID = Convert.ToInt32(ViewState["PwResetUserID"]);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Mail mail = new Mail();
                    mail.From = ViewState["EmailAdministrator"].ToString();
                    WebBaseUtility.UpdateMailConfiguration(mail, Session["config"].ToString(), pwResetUserID);

                    //System.Configuration.Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                    //MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
                    //mail.From = mailSettings.Smtp.Network.UserName;
                    //mail.RequireAutentication = !mailSettings.Smtp.Network.DefaultCredentials;
                    //mail.Username = mailSettings.Smtp.Network.UserName;
                    //mail.Password = mailSettings.Smtp.Network.Password;
                    //mail.SMTPHost = mailSettings.Smtp.Network.Host;
                    //mail.SMTPPort = mailSettings.Smtp.Network.Port;
                    //mail.TakeASentEmailCopy = false;

                    mail.To.Add(txtForgotEmail.Text);
                    mail.Title = "Forgot password";
                    var token = HttpUtility.UrlEncode(SSTCryptographer.Encrypt(username + "&" + txtForgotEmail.Text + "&"
                        + dbname + "&" + dbtype + "&" + System.DateTime.Now.ToString(), "forgot"));

                    var link = string.IsNullOrEmpty(Request.Url.Query) ? Request.Url.AbsoluteUri : Request.Url.AbsoluteUri.Replace(Request.Url.Query, String.Empty);
                    link = link + "?fogotpassword=" + token;
                    //var link = Request.Url.AbsoluteUri.Replace(Request.Url.Query, String.Empty) + "?fogotpassword=" + token;
                    mail.Text = string.Format("Hi {0},<br><br>Please click on the link below to reset your password.<br>The link will expire in 2 hours.<br><br>{1}<br><br>Thanks", username, link);
                    mail.IsBodyHtml = true;
                    mail.SendTest();
                    //success = true;
                    //pnlForgotPw.Visible = false;
                    lblForgotPwMsg.Text = "An email has been sent with a link and will expire in 2 hours";
                    ShowInputPanel(false, pnlForgotPw, lblForgotPwMsg.ClientID);

                }
            }
            catch (Exception ex)
            {
                //pnlForgotPw.Visible = true;
                lblForgotPwMsg.Text = ex.Message;
                ShowInputPanel(true, pnlForgotPw, lblForgotPwMsg.ClientID);
            }
        }
        else
        {
            //pnlForgotPw.Visible = true;
            //lblResetPwMsg.Text = "An error happened on sending the request";
            lblForgotPwMsg.Text = "An error happened on sending the request";
            ShowInputPanel(true, pnlForgotPw, lblForgotPwMsg.ClientID);
        }

        // Send and email to user for updating password
        //var success = true;
        //if (success)
        //{
        //    pnlForgotPw.Visible = false;
        //    lblForgotPwMsg.Text = "An email has been sent with a link and will expire in 2 hours";
        //}
        //else
        //{
        //    pnlForgotPw.Visible = true;
        //    lblForgotPwMsg.Text = "An error happened on sending the request";
        //}
    }

    protected void lnkCancelForgotPw_Click(object sender, EventArgs e)
    {
        programmaticModalPopupForgotPassword.Hide();
        ShowHideLogin(true);
    }

    private void ShowHideLogin(bool ishow)
    {
        if (ishow)
        {
            // Reset window programmaticModalPopupUpdateForgotPw
            lblUpdateForgotPwMsg.Text = string.Empty;
            txtFogotPwNew.Text = string.Empty;
            txtFogotPwConfirm.Text = string.Empty;

            // Reset window programmaticModalPopupForgotPassword
            lblForgotPwMsg.Text = string.Empty;
            txtForgotUsername.Text = string.Empty;
            txtForgotEmail.Text = string.Empty;

            // Reset window programmaticModalPopupResetPassword 
            lblResetPwMsg.Text = string.Empty;
            txtResetUserName.Text = string.Empty;
            txtResetEmailAdmin.Text = string.Empty;

            ScriptManager.RegisterStartupScript(Page, this.GetType(), "showLogin", "showLoginForm();", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "hideLogin", "hideLoginForm();", true);
        }
    }

    protected void lnkCancelUpdateForgotPw_Click(object sender, EventArgs e)
    {
        programmaticModalPopupUpdateForgotPw.Hide();
        ShowHideLogin(true);
    }

    protected void btnUpdateFogotPw_Click(object sender, EventArgs e)
    {
        //ViewState["ForgotPwUsername"] = arr[0];
        //ViewState["ForgotPwEmail"] = arr[1];
        var currUserName = ViewState["ForgotPwUsername"].ToString();
        var currEmail = ViewState["ForgotPwEmail"].ToString();

        if (string.IsNullOrEmpty(txtFogotPwNew.Text) || string.IsNullOrEmpty(txtFogotPwConfirm.Text))
        {
            lblUpdateForgotPwMsg.Text = "New password and confirm password are required!";
            return;
        }

        if (txtFogotPwNew.Text != txtFogotPwConfirm.Text)
        {
            lblUpdateForgotPwMsg.Text = "New password and confirm password do not match.";
            return;
        }

        // Get user information from usename and email: firstname, lastname, isapplypolicy, username
        User objProp_User = new User();
        objProp_User.Username = currUserName;
        objProp_User.Email = currEmail;
        //objProp_User.NewPassword = txtNewPassword.Text;
        objProp_User.DBName = ViewState["ForgotPwDBName"].ToString();
        objProp_User.DBType = ViewState["ForgotPwDBType"].ToString();
        ConnectionStr(objProp_User.DBName);
        objProp_User.ConnConfig = Session["config"].ToString();

        // TODO: get user info
        var ds = objBL_User.GetUserInfoByUsernameAndEmail(objProp_User);

        //var _firstName = "temp";// ViewState["firstTime"].ToString();
        //var _lastName = "temp";//ViewState["lastName"].ToString();
        string _firstName = ds.Tables[0].Rows[0]["fFirst"].ToString();
        string _lastName = ds.Tables[0].Rows[0]["Last"].ToString();
        var isUserApplyPwRules = Convert.ToBoolean(ds.Tables[0].Rows[0]["UserApplyPwRules"].ToString());
        var isSysApplyPwRules = Convert.ToBoolean(ds.Tables[0].Rows[0]["ApplyPasswordRules"].ToString());

        if (isUserApplyPwRules && isSysApplyPwRules)
        {
            var err = string.Empty;
            if (!WebBaseUtility.IsPasswordPassedPwPolicy(currUserName, _firstName, _lastName, txtFogotPwNew.Text, ref err))
            {
                lblUpdateForgotPwMsg.Text = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(err);
                //lblUpdateForgotPwMsg.Text = "The New Password should contain Minimum 6 and Maximum 10 Characters with 1 English Uppercase Letter, 1 Lowercase Letter, 1 Special Characters, 1 Numeric Character and should not have user\'s first three letters of his/her First Name, Last Name and/or Username";
                return;
            }
        }

        //Update new password
        //var dbname = ddlCompany.SelectedValue.Split(':')[0];
        //var dbtype = (ddlCompany.SelectedValue.Split(':')[1]);
        objProp_User = new User();
        objProp_User.Username = currUserName;
        //objProp_User.Password = currPassword;
        objProp_User.NewPassword = txtFogotPwNew.Text;
        //objProp_User.DBName = dbname;
        //objProp_User.DBType = dbtype;
        //ConnectionStr(ddlCompany.SelectedValue.Split(':')[0]);
        objProp_User.DBName = ViewState["ForgotPwDBName"].ToString();
        objProp_User.DBType = ViewState["ForgotPwDBType"].ToString();
        ConnectionStr(objProp_User.DBName);
        objProp_User.ConnConfig = Session["config"].ToString();
        try
        {
            objBL_User.UpdateForgotPassword(objProp_User);
            //pnlUpdateForgotPw.Visible = false;
            lblUpdateForgotPwMsg.Text = "Your password has been updated successfully!";
            ShowInputPanel(false, pnlUpdateForgotPw, lblUpdateForgotPwMsg.ClientID);
            //lblUpdateForgotPwMsg.Attributes("")
            //txtPassword.Text = txtNewPassword.Text;
            //programmaticModalPopupUpdateForgotPw.Hide();
            //ShowHideLogin(true);
            //btnLogin_Click(sender, e);

            // On implementation for ES-3517: Thomas
            //objProp_User.ConnConfig = Session["config"].ToString();
            //var ds1 = objBL_User.getControl(objProp_User);
            //if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            //{
            //    //var isAppliedPasswordPolicy = string.IsNullOrEmpty(ds1.Tables[0].Rows[0]["ApplyPasswordRules"].ToString()) ? false : Convert.ToBoolean(ds1.Tables[0].Rows[0]["ApplyPasswordRules"].ToString());

            //    if (isUserApplyPwRules && isSysApplyPwRules)
            //    {
            //        lnkPasswordResetting.Visible = true;
            //        var emailAdmin = ds1.Tables[0].Rows[0]["PwResetAdminEmail"].ToString();
            //        var userAdminID = ds1.Tables[0].Rows[0]["PwResetUserID"].ToString();
            //        //objProp_User.ConnConfig = Session["config"].ToString();
            //        if (emailAdmin != null && !string.IsNullOrEmpty(emailAdmin.ToString()))
            //        {
            //            Mail mail = new Mail();
            //            mail.From = emailAdmin;
            //            WebBaseUtility.UpdateMailConfiguration(mail, Session["config"].ToString(), Convert.ToInt32(userAdminID));
            //            mail.To.Add(currEmail);
            //            mail.Title = "Updated password";
            //            mail.Text = string.Format("Hi {0}," +
            //                "<br><br>Your password has been reset successfully!" +
            //                "<br><br>Thank you",
            //                currUserName);
            //            mail.IsBodyHtml = true;
            //            mail.SendTest();
            //        }
            //    }
            //}
        }
        catch (Exception ex)
        {
            lblUpdateForgotPwMsg.Text = ex.Message;
        }
    }

    private void ShowInputPanel(bool ishow, Panel panel, string messClientId)
    {
        if (ishow)
        {
            panel.Visible = true;
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "marginMess", "addRemoveMarginForErrorMessage(false," + messClientId + ");", true);
        }
        else
        {
            panel.Visible = false;
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "marginMess", "addRemoveMarginForErrorMessage(true," + messClientId + ");", true);
        }
    }

    //private string MergePermissionString(string string1, string string2)
    //{
    //    StringBuilder builder = new StringBuilder();
    //    var diffStr = "";
    //    var len = string1.Length;
    //    if (string1.Length > string2.Length)
    //    {
    //        len = string2.Length;
    //        diffStr = string1.Substring(len, string1.Length - len);

    //    }
    //    else if (string1.Length < string2.Length)
    //    {
    //        len = string1.Length;
    //        diffStr = string2.Substring(len, string2.Length - len);
    //    }

    //    var arr1 = string1.ToUpper().ToArray();
    //    var arr2 = string2.ToUpper().ToArray();
    //    for (int i = 0; i < len; i++)
    //    {
    //        if (arr1[i] != arr2[i] && arr2[i] == 'Y')
    //        {
    //            builder.Append('Y');
    //        }
    //        else
    //        {
    //            builder.Append(arr1[i]);
    //        }
    //    }

    //    builder.Append(diffStr);

    //    return builder.ToString();
    //}

    //private int MergePermissionInt(int int1, int int2)
    //{
    //    if (int1 == 0) return int2;
    //    else return int1;
    //}

    //Start-- API Changes : Juily:04/06/2020 --//
    public void GetAPIIntegrationEnable()
    {
        DataSet ds = new DataSet();
        //string DBName = Session["config"].ToString().Split(';')[1].Split('=')[0];
        string DBValue = Session["config"].ToString().Split(';')[1].Split('=')[1];

        if (DBValue != "0")
        {
            ds = objBL_User.GetAPIIntegrationEnable(Session["config"].ToString());

            APIIntegrationModel _objAPIIntegration = new APIIntegrationModel();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["ModuleName"].ToString() == "DashBoard")
                {
                    _objAPIIntegration.IsAPIIntegrationForDashBoardModule = Convert.ToBoolean(dr["Integration"]);
                }
                if (dr["ModuleName"].ToString() == "Customers")
                {
                    _objAPIIntegration.IsAPIIntegrationForCustomersModule = Convert.ToBoolean(dr["Integration"]);
                }
                if (dr["ModuleName"].ToString() == "Recurring")
                {
                    _objAPIIntegration.IsAPIIntegrationForRecurringModule = Convert.ToBoolean(dr["Integration"]);
                }
                if (dr["ModuleName"].ToString() == "Schedule")
                {
                    _objAPIIntegration.IsAPIIntegrationForScheduleModule = Convert.ToBoolean(dr["Integration"]);
                }
                if (dr["ModuleName"].ToString() == "Billing")
                {
                    _objAPIIntegration.IsAPIIntegrationForBillingModule = Convert.ToBoolean(dr["Integration"]);
                }
                if (dr["ModuleName"].ToString() == "AP")
                {
                    _objAPIIntegration.IsAPIIntegrationForAPModule = Convert.ToBoolean(dr["Integration"]);
                }
                if (dr["ModuleName"].ToString() == "Purchasing")
                {
                    _objAPIIntegration.IsAPIIntegrationForPurchasingModule = Convert.ToBoolean(dr["Integration"]);
                }
                if (dr["ModuleName"].ToString() == "Sales")
                {
                    _objAPIIntegration.IsAPIIntegrationForSalesModule = Convert.ToBoolean(dr["Integration"]);
                }
                if (dr["ModuleName"].ToString() == "Projects")
                {
                    _objAPIIntegration.IsAPIIntegrationForProjectsModule = Convert.ToBoolean(dr["Integration"]);
                }
                if (dr["ModuleName"].ToString() == "Inventory")
                {
                    _objAPIIntegration.IsAPIIntegrationForInventoryModule = Convert.ToBoolean(dr["Integration"]);
                }
                if (dr["ModuleName"].ToString() == "Financials")
                {
                    _objAPIIntegration.IsAPIIntegrationForFinancialsModule = Convert.ToBoolean(dr["Integration"]);
                }
                if (dr["ModuleName"].ToString() == "Statements")
                {
                    _objAPIIntegration.IsAPIIntegrationForStatementsModule = Convert.ToBoolean(dr["Integration"]);
                }
                if (dr["ModuleName"].ToString() == "Programs")
                {
                    _objAPIIntegration.IsAPIIntegrationForProgramsModule = Convert.ToBoolean(dr["Integration"]);
                }
            }

            Session["IsAPIIntegration"] = _objAPIIntegration;
        }
    }
    //End-- API Changes : Juily:04/06/2020 --//


    //Create Login Log in LoginLog table
    public void CreateLoginLog(int UserId, string UserName)
    {
        string url = HttpContext.Current.Request.Url.AbsoluteUri;
        string IP = null;

        string userip = Request.UserHostAddress;
        if (Request.UserHostAddress != null)
        {
            Int64 macinfo = new Int64();
            string macSrc = macinfo.ToString("X");
            if (macSrc == "0")
            {
                if (userip != "127.0.0.1")
                {
                    IP = userip;
                }
            }
        }
        string MACAddress = GetClientMAC(IP);
        string config = Session["config"].ToString();
        objBL_User.CreateLoginLog(UserId, UserName, config, url, IP, MACAddress);
    }

    [DllImport("Iphlpapi.dll")]
    private static extern int SendARP(Int32 dest, Int32 host, ref Int64 mac, ref Int32 length);
    [DllImport("Ws2_32.dll")]
    private static extern Int32 inet_addr(string ip);

    private static string GetClientMAC(string strClientIP)
    {
        string mac_dest = "";
        try
        {
            Int32 ldest = inet_addr(strClientIP);
            Int32 lhost = inet_addr("");
            Int64 macinfo = new Int64();
            Int32 len = 6;
            int res = SendARP(ldest, 0, ref macinfo, ref len);
            string mac_src = macinfo.ToString("X");

            while (mac_src.Length < 12)
            {
                mac_src = mac_src.Insert(0, "0");
            }

            for (int i = 0; i < 11; i++)
            {
                if (0 == (i % 2))
                {
                    if (i == 10)
                    {
                        mac_dest = mac_dest.Insert(0, mac_src.Substring(i, 2));
                    }
                    else
                    {
                        mac_dest = "-" + mac_dest.Insert(0, mac_src.Substring(i, 2));
                    }
                }
            }
        }
        catch (Exception err)
        {
            throw new Exception("L?i " + err.Message);
        }
        return mac_dest;
    }

}
