using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Web.Script.Serialization;
namespace MOMWebApp.Controllers
{
    public class MasterController : Controller
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
        // GET: Master

        public void Page_Load()
        {
            if (Session["MSM"] == null)
            {
                Response.Redirect("~/login.aspx");
            }
            else
            {
                DrawControls();
            }
            // Hide Estimate, Estimate Template and Project manager in the menu
            //TempData["lblQblastSync"] = "";
            if (Session["MSM"].ToString() == "TS")
            {
                //lnkEstimate.Visible = false;
                //lnkEstimateTempl.Visible = false;
                //ProjectMgr.Visible = false;
                TempData["lnkEstimate"] = "";
                TempData["lnkEstimateTempl"] = "";
                TempData["ProjectMgr"] = "";
            }

            if (Session["MSM"].ToString() == "ADMIN")
            {
                
                AdministratorPermissions();
                //lnkEstimate.Visible = false;
                //lnkEstimateTempl.Visible = false;
                //ProjectMgr.Visible = false;
            }
            else
            {
                //Page Permission 
                //customerpermissions();
                
                //liChangePassword.Visible = false;
                //liGPSSetting.Visible = false;
            }
            if (Session["user"] != null)
            {
                FinancialPermissions();
                Qblastsync();
                TempData["lblUser"] = Session["username"].ToString();
                TempData["lblCompany"] = Session["company"].ToString();
                NonAdminUSerPermissions();

                TSDBPermissions();
                ProgramManagerPermission();
                SalesPersonPermission();
                GetDefaultCompany();

                //TempData["cstmMgr"] = "block";
                //string str = "test&nbsp123";

                //TempData["abcd"] = "<a  ID='cstmMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl' onclick=tt1('You&nbspdo&nbspnot&nbsphave&nbsppermissions&nbspto&nbspaccess&nbspusers!','warning');><i class='mdi-social-people' ></i>Customers</a>";



            }
            if (Session["userProfileImage"] != null)
            {
                //imgProfileImage.Src = Session["userProfileImage"].ToString();
                //imgProfileImageLg.Src = Session["userProfileImage"].ToString();
                TempData["imgProfileImage"] = Session["userProfileImage"].ToString();
                TempData["imgProfileImageLg"] = Session["userProfileImage"].ToString();
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
                    ds = objBL_User.GetUserInfoByID(objProp_User);
                    var profileImage = ds.Tables[0].Rows[0]["ProfileImage"].ToString();
                    //imgProfileImage.Src = profileImage;
                    //imgProfileImageLg.Src = profileImage;
                    TempData["imgProfileImage"] = profileImage;
                    TempData["imgProfileImageLg"] = profileImage;
                    Session["userProfileImage"] = profileImage;
                }
                catch (Exception)
                {
                    // Do nothing
                }

            }

            if (string.IsNullOrWhiteSpace(Convert.ToString (TempData["imgProfileImage"])))
            {
                //imgProfileImage.Src = "Design/images/avatar.jpg";
                //imgProfileImageLg.Src = "Design/images/avatar.jpg";
                TempData["imgProfileImage"] = "../Design/images/avatar.jpg"; ;
                TempData["imgProfileImageLg"] = "../Design/images/avatar.jpg"; ;
            }
            if (Session["user"] != null)
            {
                LoadDashboardMenu();
                // Updating todo tasks for the day
                //hdnItemJSON.Value = GetUserTodoTasksForToday();
                TempData["hdnItemJSON"] = GetUserTodoTasksForToday();
            }

            if (TodoTasks == 0)
            {
                //todoNotify.Visible = false;
                TempData["todoNotify"] = "False";
            }
            else
            {
                //todoNotify.Visible = true;
                //lblTodoNumber.Text = TodoTasks.ToString();
                TempData["todoNotify"] = "True";
                TempData["lblTodoNumber"] = TodoTasks.ToString();
            }
            //return RedirectToAction("_Layout");
        }
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult QBLastSync()
        {
            Qblastsync();
            return RedirectToAction("_Layout");

        }
        public ActionResult Logout()
        {
            GetDefaultCompanyReset();
            Session.Abandon();
            Session.Clear();
            Response.Redirect("~/login.aspx");
            return RedirectToAction("_Layout");
            
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
                //linkDashboarDefault.Visible = false;
                TempData["linkDashboarDefault"] = "";
                //linkDashboardMenu.Visible = true;
                //listDashboardMenu.Visible = true;
                //listDashboardMenu.DataSource = ds.Tables[0];
                //listDashboardMenu.DataBind();

            }
            else
            {
                //linkDashboarDefault.Visible = true;
                TempData["linkDashboarDefault"] = "<a ID='linkDashboarDefault' waves-effect waves-cyan collapsible-height-nl' href ='~/Home.aspx' ><i class='mdi-action-dashboard no-collapse'></i>Dashboard</a>";
                //linkDashboardMenu.Visible = false;
                ///listDashboardMenu.Visible = false;
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
                    //cntractsMgr.Visible = false;
                    //acctMgr.Visible = false;
                    //financeMgr.Visible = false;
                    //purchaseMgr.Visible = false;
                    //InventoryMgr.Visible = false;
                    //ReportMgr.Visible = false;
                    //financialStatement.Visible = false;
                    //acctPayable.Visible = false;
                    //progMgr.Visible = false;

                    TempData["cntractsMgr"] = "";
                    TempData["acctMgr"] = "";
                    TempData["financeMgr"] = "";
                    TempData["purchaseMgr"] = "";
                    TempData["InventoryMgr"] = "";
                    TempData["ReportMgr"] = "";
                    TempData["financialStatement"] = "";
                    TempData["acctPayable"] = "";
                    TempData["progMgr"] = "";

                    //customer manager sub Menu   

                    //lnkReceivePayment.Visible = false;
                    //lnkDeposit.Visible = false;
                    //lnkCollections.Visible = false;

                    TempData["lnkReceivePayment"] = "";
                    TempData["lnkDeposit"] = "";
                    TempData["lnkCollections"] = "";

                    // Scheduler sub Menu  

                    //lnkScheduleMenu.Visible = false;
                    //lnkTimesheet.Visible = false;
                    //lnkMapMenu.Visible = false;
                    //lnkRouteBuilder.Visible = false;
                    TempData["lnkScheduleMenu"] = "";
                    TempData["lnkTimesheet"] = "";
                    TempData["lnkMapMenu"] = "";
                    TempData["lnkRouteBuilder"] = "";
                    

                    //lnkRoutes.Visible = false;

                    //Contract manager sub Menu  
                    //lnkDepartments.Visible = false;
                    TempData["lnkDepartments"] = "";
                    //lnkDepartments.Visible = false;
                    //lnkDepartments.Visible = false;

                    // Project manager sub Menu 
                    //lnkDepartments.Visible = false;
                }
            }
        }

        private void GetDefaultCompany()
        {
           // lblSelectCompanyCount.Text = "";
            TempData["lblSelectCompanyCount"] = "";
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
                    //lblSelectCompany.Text = companyname.Substring(0, 16);
                TempData["lblSelectCompany"] = companyname.Substring(0, 16);
                else
                    //lblSelectCompany.Text = companyname;
                TempData["lblSelectCompany"] = companyname;
                //lblSelectCompany.ToolTip = ds.Tables[0].Rows[0]["Name"].ToString();
                if (Session["chkCompanyName"] != null)
                {
                    string StrDefComName = Convert.ToString(Session["chkCompanyName"]);
                    if (StrDefComName.Length > 16)
                        //lblSelectCompany.Text = StrDefComName.Substring(0, 16);
                    TempData["lblSelectCompany"] = StrDefComName.Substring(0, 16); 
                    else
                        //lblSelectCompany.Text = StrDefComName;
                    TempData["lblSelectCompany"] = StrDefComName;
                    //lblSelectCompany.ToolTip = StrDefComName;
                }
                if (Convert.ToInt32(ds.Tables[0].Rows[0]["NoOfCompany"].ToString()) > 1)
                {
                    //lblSelectCompany.Text = "Multi Company";
                    TempData["lblSelectCompany"] = "Multi Company";
                    //lblSelectCompanyCount.Text = "(" + ds.Tables[0].Rows[0]["NoOfCompany"].ToString() + ")";
                    TempData["lblSelectCompanyCount"] = "(" + ds.Tables[0].Rows[0]["NoOfCompany"].ToString() + ")";
                    //lblSelectCompany.ToolTip = Convert.ToString(Session["CompList"]);
                }
            }
            else
            {
                //lblSelectCompany.Text = "Select Company";
                //lblSelectCompanyCount.Text = "";
                TempData["lblSelectCompany"] = "Select Company";
                TempData["lblSelectCompanyCount"] = "";
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
                //liSelectCompany.Visible = false;
                //lnkManageCompanies.Visible = false;
                TempData["liSelectCompany"] = "";
                TempData["lnkManageCompanies"] = "";
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
                                //liSelectCompany.Visible = true;
                                //lnkManageCompanies.Visible = true;
                                TempData["liSelectCompany"] = "True";
                                TempData["lnkManageCompanies"] = "<a  ID='lnkManageCompanies' href='../ManageCompanies.aspx'><i class='mdi-image-navigate-next' ></i>Manage Companies</a>";
                            }
                            else
                            {
                                Session["COPer"] = "2";
                                Session["CmpChkDefault"] = "2";
                                //liSelectCompany.Visible = false;
                                //lnkManageCompanies.Visible = false;
                                TempData["liSelectCompany"] = "";
                                TempData["lnkManageCompanies"] = "";
                            }
                        }
                        if (_dr["Name"].ToString().Equals("MultiOffice"))
                        {
                            //if (_dr["Label"].ToString() == "1")
                                //liSelectOffice.Visible = false;
                            //else
                                //liSelectOffice.Visible = false;
                        }
                    }
                }
            }


        }
        private void TSDBPermissions()
        {
            /**IF database logged in is TS**/
            if (Session["MSM"].ToString() == "TS")
            {
                //lnkCollections.Visible = false;
                //financeMgr.Visible = false;
                //acctPayable.Visible = false;
                //financialStatement.Visible = false;
                //ReportMgr.Visible = false;
                //lnkPeriodCloseOut.Visible = false;
                //lnkReceivePayment.Visible = false;
                //lnkDeposit.Visible = false;
                //lnkCntrlPnl.Visible = false;
                //cntractsMgr.Visible = false;
                //InventoryMgr.Visible = false;
                //HyperLink1.Visible = false;
                ////purchaseMgr.Visible = false;
                //liReceivePO.Visible = false;
                //lnkCustomFields.Visible = false;
                //lnkCustomersSmenu.Visible = false;
                //lnkLocationsSMenu.Visible = false;
                //lnkBillcodeSMenu.Visible = false;
                //lnkTimesheet.Visible = false;

                TempData["lnkCollections"] = "";
                TempData["financeMgr"] = "";
                TempData["acctPayable"] = "";
                TempData["financialStatement"] = "";
                TempData["ReportMgr"] = "";
                TempData["lnkPeriodCloseOut"] = "";
                TempData["lnkReceivePayment"] = "";
                TempData["lnkDeposit"] = "";
                TempData["lnkCntrlPnl"] = "";
                TempData["cntractsMgr"] = "";
                TempData["InventoryMgr"] = "";
                TempData["HyperLink1"] = "";
                TempData["liReceivePO"] = "";
                TempData["lnkCustomFields"] = "";
                TempData["lnkCustomersSmenu"] = "";
                TempData["lnkLocationsSMenu"] = "";
                TempData["lnkBillcodeSMenu"] = "";
                TempData["lnkTimesheet"] = "";

                if (Convert.ToInt16(Session["MSREP"]) != 1)
                {
                    //cstmMgr.Visible = false;
                    TempData["cstmMgr"] = "";
                }
                if (Convert.ToInt16(Session["payment"]) != 1)
                {
                    if (Session["type"].ToString() == "c")
                    {
                        //lnkPaymentHistory.Visible = false;
                        TempData["lnkPaymentHistory"] = "";
                    }

                }
            }
        }
        public ActionResult _Layout()
        {
            return View();
        }
        public ActionResult Master()
        {
            return View();
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
        private void AdministratorPermissions()
        {
            //hide menu
            //lblUser.Text = "Administrator";
            //lblCompany.Text = "Administrator Database";
            TempData["lblUser"] = "Administrator";
            TempData["lblCompany"] = "Administrator Database";
            //cntractsMgr.Visible = false;
            TempData["cntractsMgr"] = "";
            //acctMgr.Visible = false;
            TempData["acctMgr"] = "";
            //SalesMgr.Visible = false;
            TempData["SalesMgr"] = "";
            //progMgr.Visible = false;
            TempData["progMgr"] = "";
            //lnkBillcodeSMenu.Visible = false;
            TempData["lnkBillcodeSMenu"] = "";
            //lnkScheduleMenu.Visible = false;
            TempData["lnkScheduleMenu"] = "";
            //lnkMapMenu.Visible = false;
            TempData["lnkMapMenu"] = "";
            //lnkRouteBuilder.Visible = false;
            TempData["lnkRouteBuilder"] = "";
            //lnkProspect.Visible = false;
            TempData["lnkProspect"] = "";
            //lnkTimesheet.Visible = false;
            TempData["lnkTimesheet"] = "";
            //cstmMgr.Visible = false;
            TempData["cstmMgr"] = "";
            //schMgr.Visible = false;
            TempData["schMgr"] = "";
            //ProjectMgr.Visible = false;
            TempData["ProjectMgr"] = "";
            //financeMgr.Visible = false;
            TempData["financeMgr"] = "";
            //acctPayable.Visible = false;
            TempData["acctPayable"] = "";
            ////financialStatement.Visible = false;
            TempData["financialStatement"] = "";
            ////ReportMgr.Visible = false;
            //InventoryMgr.Visible = false;
            TempData["InventoryMgr"] = "";
            //liChangePassword.Visible = true;
            //liGPSSetting.Visible = true;
        }
        private void NonAdminUSerPermissions()
        {

            if (Convert.ToInt16(Session["payment"]) != 1)
            {
                //lnkPaymentHistory.NavigateUrl = "";
                //lnkPaymentHistory.Attributes.Add("style", "display:none");
                TempData["lnkPaymentHistory"] = "<a  ID='lnkPaymentHistory' href='../paymenthistory.aspx'><i class='mdi-image-navigate-next' ></i>Online Payment</a>";
            }


            /*****************IF logged in user is not Admin**************/
            if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
            {
                DataTable dt = new DataTable();
                DataTable dtUserPermission = new DataTable();
                dt = (DataTable)Session["userinfo"];
                dtUserPermission = GetUserById();
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
                        //cstmMgr.NavigateUrl = "";
                        //cstmMgr.Attributes.Add("style", "display:none");                        
                        TempData["cstmMgr"] = "<a  ID='cstmMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-social-people' ></i>Customers</a>";
                    }

                    string ProjectModulePermission = dtUserPermission.Rows[0]["ProjectModulePermission"] == DBNull.Value ? "N" : dtUserPermission.Rows[0]["ProjectModulePermission"].ToString();

                    if (ProjectModulePermission == "Y")
                    {
                        //ProjectMgr.NavigateUrl = "";
                        //ProjectMgr.Attributes.Add("style", "display:none");

                        TempData["ProjectMgr"] = "<a  ID='ProjectMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-action-assignment' ></i>Projects</a>";
                    }


                    string InventoryModulePermission = dtUserPermission.Rows[0]["InventoryModulePermission"] == DBNull.Value ? "N" : dtUserPermission.Rows[0]["InventoryModulePermission"].ToString();

                    if (InventoryModulePermission != "Y")
                    {
                        //InventoryMgr.NavigateUrl = "";
                        //InventoryMgr.Attributes.Add("style", "display:none");

                        TempData["InventoryMgr"] = "<a  ID='InventoryMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-editor-format-align-justify' ></i>Inventory</a>";
                    }



                    if (ViewEquipment != "Y")
                    {
                        //lnkEquipmentsSMenu.NavigateUrl = "";
                        //lnkEquipmentsSMenu.Attributes.Add("style", "display:none");
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster1", "if( $(" + lnkEquipmentsSMenu.ClientID + ").length ){ $(" + lnkEquipmentsSMenu.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access Equipment!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true});  }); }", true);

                        TempData["lnkEquipmentsSMenu"] = "<a  ID='lnkEquipmentsSMenu' href='../Equipments.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Equipment</a>";
                    }
                    if (ViewProject != "Y")
                    {
                        //lnkProject.NavigateUrl = "";
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster2", "if($(" + lnkProject.ClientID + ").length){ $(" + lnkProject.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access Projects!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true });  });}", true);
                        TempData["lnkProject"] = "<a  ID='lnkProject' href='../project.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Projects</a>";
                    }
                    if (ViewLocation != "Y")
                    {
                        //lnkLocationsSMenu.NavigateUrl = "";
                        //lnkLocationsSMenu.Attributes.Add("style", "display:none");
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster3", "if($(" + lnkLocationsSMenu.ClientID + ").length){ $(" + lnkLocationsSMenu.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access Locations!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true });  });}", true);                        
                        TempData["lnkLocationsSMenu"] = "<a  ID='lnkLocationsSMenu' href='../locations.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Locations</a>";
                    }
                    if (ViewOwner != "Y")
                    {
                        //lnkCustomersSmenu.NavigateUrl = "";
                        //lnkCustomersSmenu.Attributes.Add("style", "display:none");
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster4", "if($(" + lnkCustomersSmenu.ClientID + ").length){ $(" + lnkCustomersSmenu.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access Customers!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true });  });}", true);
                        TempData["lnkCustomersSmenu"] = "<a  ID='lnkCustomersSmenu' href='../Customers.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Customers</a>";
                    }



                    string ViewApplyPermission = dtUserPermission.Rows[0]["Apply"].ToString();

                    ViewApplyPermission = ViewApplyPermission.Length < 4 ? "N" : ViewApplyPermission.Substring(3, 1);

                    if (ViewApplyPermission != "Y")
                    {
                        //lnkReceivePayment.NavigateUrl = "";
                        //lnkReceivePayment.Attributes.Add("style", "display:none");
                        TempData["lnkReceivePayment"] = "<a  ID='lnkReceivePayment' href='../receivepayment.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Receive Payment</a>";
                    }
                    string ViewDepositPermission = dtUserPermission.Rows[0]["Deposit"].ToString();

                    ViewDepositPermission = ViewDepositPermission.Length < 4 ? "N" : ViewDepositPermission.Substring(3, 1);

                    if (ViewDepositPermission != "Y")
                    {
                        //lnkDeposit.NavigateUrl = "";
                        //lnkDeposit.Attributes.Add("style", "display:none");
                        TempData["lnkDeposit"] = "<a  ID='lnkDeposit' href='../managedeposit.aspx'><i class='mdi-image-navigate-next' ></i>Make Deposit</a>";
                    }

                    string ViewCollectionPermission = dtUserPermission.Rows[0]["Collection"].ToString();

                    ViewCollectionPermission = ViewCollectionPermission.Length < 4 ? "N" : ViewCollectionPermission.Substring(3, 1);

                    if (ViewCollectionPermission != "Y")
                    {
                        //lnkCollections.NavigateUrl = "";
                        //lnkCollections.Attributes.Add("style", "display:none");
                        TempData["lnkCollections"] = "<a  ID='lnkCollections' href='../iCollections.aspx'><i class='mdi-image-navigate-next' ></i>Collections</a>";
                    }

                    if (ViewTicket != "Y")
                    {
                        //lnkListView.NavigateUrl = "";
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster5", "if( $(" + lnkListView.ClientID + ").length ){ $(" + lnkListView.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access Ticket!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true});  }); }", true);
                        TempData["lnkListView"] = "<a  ID='lnkListView' href='../TicketListView.aspx'><i class='mdi-image-navigate-next' ></i>Ticket List</a>";

                    }
                    if (new GeneralFunctions().GetSalesAsigned() == 0)
                    {
                        if (ViewInventoryItem != "Y")
                        {
                            //lnkItemMaster.NavigateUrl = "";
                            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster6", "if($(" + lnkItemMaster.ClientID + ").length){ $(" + lnkItemMaster.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access Inventory Item!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true });  });}", true);
                            TempData["lnkItemMaster"] = "<a  ID='lnkItemMaster' href='../Inventory.aspx'><i class='mdi-image-navigate-next' ></i>Item Master</a>";
                        }
                        if (ViewInventoryAdjustment != "Y")
                        {
                            //lnkAdjustment.NavigateUrl = "";
                            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster7", "if($(" + lnkAdjustment.ClientID + ").length){ $(" + lnkAdjustment.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access Inventory Adjustment!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true });  });}", true);
                            TempData["lnkAdjustment"] = "<a  ID='lnkAdjustment' href='../InventoryAdjustments.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Item Adjustment</a>";
                        }
                        if (ViewProjecttempPermission != "Y")
                        {
                            //lnkProjectTempl.NavigateUrl = "";
                            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster8", "if($(" + lnkProjectTempl.ClientID + ").length){ $(" + lnkProjectTempl.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access Templates!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true });  });}", true);

                            //lnkEstimateTempl.NavigateUrl = "";
                            TempData["lnkProjectTempl"] = "<a  ID='lnkProjectTempl' href='../projecttemplate.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Project Templates</a>";
                            TempData["lnkEstimateTempl"] = "<a  ID='lnkEstimateTempl' href='../estimatetemplate.aspx'><i class='mdi-image-navigate-next' ></i>Templates</a>";

                        }

                    }



                    string ViewInvoicesPermission = dtUserPermission.Rows[0]["Invoice"].ToString();

                    ViewInvoicesPermission = ViewInvoicesPermission.Length < 4 ? "N" : ViewInvoicesPermission.Substring(3, 1);

                    if (ViewInvoicesPermission != "Y")
                    {
                        //lnkInvoicesSMenu.NavigateUrl = "";
                        //lnkInvoicesSMenu.Attributes.Add("style", "display:none");

                        TempData["lnkInvoicesSMenu"] = "<a  ID='lnkInvoicesSMenu' href='../invoices.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Invoices</a>";
                    }


                    string ViewPOPermission = dtUserPermission.Rows[0]["PO"].ToString();

                    ViewPOPermission = ViewPOPermission.Length < 4 ? "N" : ViewPOPermission.Substring(3, 1);
                    if (ViewPOPermission != "Y")
                    {
                        //lnkPO.NavigateUrl = "";
                        //lnkPO.Attributes.Add("style", "display:none");
                        TempData["lnkPO"] = "<a  ID='lnkPO' href='../managepo.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Purchase Orders</a>";



                    }

                    string ViewReceivePOPermission = dtUserPermission.Rows[0]["RPO"].ToString();

                    ViewReceivePOPermission = ViewReceivePOPermission.Length < 4 ? "N" : ViewReceivePOPermission.Substring(3, 1);
                    if (ViewReceivePOPermission != "Y")
                    {



                        //lnkReceivePO.NavigateUrl = "";
                        //lnkReceivePO.Attributes.Add("style", "display:none");
                        TempData["lnkReceivePO"] = "<a  ID='lnkReceivePO' href='../managereceivepo.aspx'><i class='mdi-image-navigate-next' ></i>Receive PO</a>";

                    }
                    string ViewBillingCodesPermissionn = dtUserPermission.Rows[0]["BillingCodesPermission"].ToString();

                    ViewBillingCodesPermissionn = ViewBillingCodesPermissionn.Length < 4 ? "N" : ViewBillingCodesPermissionn.Substring(3, 1);

                    if (ViewBillingCodesPermissionn != "Y")
                    {
                        //lnkBillcodeSMenu.NavigateUrl = "";
                        //lnkBillcodeSMenu.Attributes.Add("style", "display:none");
                        TempData["lnkBillcodeSMenu"] = "<a  ID='lnkBillcodeSMenu' href='../billingcodes.aspx'><i class='mdi-image-navigate-next' ></i>Billing Codes</a>";
                    }

                    string ViewPaymentHistoryPermissionn = dtUserPermission.Rows[0]["PaymentHistoryPermission"].ToString();

                    ViewPaymentHistoryPermissionn = ViewPaymentHistoryPermissionn.Length < 4 ? "N" : ViewPaymentHistoryPermissionn.Substring(3, 1);

                    if (ViewPaymentHistoryPermissionn != "Y")
                    {
                        //lnkPaymentHistory.NavigateUrl = "";
                        //lnkPaymentHistory.Attributes.Add("style", "display:none");
                        TempData["lnkPaymentHistory"] = "<a  ID='lnkPaymentHistory' href='../paymenthistory.aspx'><i class='mdi-image-navigate-next' ></i>Online Payment</a>";
                    }
                    string ViewManageChecksPermission = dtUserPermission.Rows[0]["BillPay"].ToString();

                    ViewManageChecksPermission = ViewManageChecksPermission.Length < 4 ? "N" : ViewManageChecksPermission.Substring(3, 1);

                    if (ViewManageChecksPermission != "Y")
                    {
                        //lnkWriteCheck2.NavigateUrl = "";
                        //lnkWriteCheck2.Attributes.Add("style", "display:none");
                        //lnkWriteCheck.NavigateUrl = "";
                        //lnkWriteCheck.Attributes.Add("style", "display:none");
                        TempData["lnkWriteCheck2"] = "<a  ID='lnkWriteCheck2' href='../managechecks.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Manage Checks</a>";
                        TempData["lnkWriteCheck"] = "<a  ID='lnkWriteCheck' href='../managechecks.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Manage Checks</a>";
                    }

                    string ViewBillPermission = dtUserPermission.Rows[0]["Bill"].ToString();

                    ViewBillPermission = ViewBillPermission.Length < 4 ? "N" : ViewBillPermission.Substring(3, 1);

                    if (ViewBillPermission != "Y")
                    {
                        //lnkAddBill.NavigateUrl = "";
                        //lnkAddBill.Attributes.Add("style", "display:none");
                        TempData["lnkAddBill"] = "<a  ID='lnkAddBill' href='../managebills.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Bills</a>";
                    }

                    string ViewVendorPermission = dtUserPermission.Rows[0]["Vendor"].ToString();

                    ViewVendorPermission = ViewVendorPermission.Length < 4 ? "N" : ViewVendorPermission.Substring(3, 1);

                    if (ViewVendorPermission != "Y")
                    {
                        //lnkVendors.NavigateUrl = "";
                        //lnkVendors.Attributes.Add("style", "display:none");
                        TempData["lnkVendors"] = "<a  ID='lnkVendors' href='../vendors.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Vendors</a>";
                    }


                    string FinancialmodulePermission = dtUserPermission.Rows[0]["FinancialmodulePermission"] == DBNull.Value ? "N" : dtUserPermission.Rows[0]["FinancialmodulePermission"].ToString();

                    if (FinancialmodulePermission != "Y")
                    {
                        //financeMgr.NavigateUrl = "";
                        //financeMgr.Attributes.Add("style", "display:none");
                        
                                 TempData["financeMgr"] = "<a  ID='financeMgr' class = 'collapsible-header waves-effect waves-cyan collapsible-height-nl' ><i class='mdi-editor-attach-money'></i>Financials</a>";
                    }

                    string chartofAccountPermission = dtUserPermission.Rows[0]["chart"].ToString();

                    chartofAccountPermission = chartofAccountPermission.Length < 4 ? "N" : chartofAccountPermission.Substring(3, 1);

                    if (chartofAccountPermission != "Y")
                    {
                        //lnkCOA.NavigateUrl = "";
                        //lnkCOA.Attributes.Add("style", "display:none");
                        TempData["lnkCOA"] = "<a  ID='lnkCOA' href='../chartofaccount.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Chart of Accounts</a>";
                    }
                    string bankrecPermission = dtUserPermission.Rows[0]["bankrec"].ToString();

                    bankrecPermission = bankrecPermission.Length < 4 ? "N" : bankrecPermission.Substring(3, 1);

                    if (bankrecPermission != "Y")
                    {
                        //lnkBankRecon.NavigateUrl = "";
                        //lnkBankRecon.Attributes.Add("style", "display:none");
                        TempData["lnkBankRecon"] = "<a  ID='lnkBankRecon' href='../bankrecon.aspx'><i class='mdi-image-navigate-next' ></i>Bank Reconciliation</a>";
                    }
                    string JournalEntryPermission = dtUserPermission.Rows[0]["GLAdj"].ToString();

                    JournalEntryPermission = JournalEntryPermission.Length < 4 ? "N" : JournalEntryPermission.Substring(3, 1);

                    if (JournalEntryPermission != "Y")
                    {
                        //lnkJournalEntry.NavigateUrl = "";
                        //lnkJournalEntry.Attributes.Add("style", "display:none");
                        TempData["lnkJournalEntry"] = "<a  ID='lnkJournalEntry' href='../journalentry.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Journal Entry</a>";
                    }

                    string RCmodulePermission = dtUserPermission.Rows[0]["RCmodulePermission"] == DBNull.Value ? "N" : dtUserPermission.Rows[0]["RCmodulePermission"].ToString();

                    if (RCmodulePermission != "Y")
                    {
                        //cntractsMgr.NavigateUrl = "";
                        //cntractsMgr.Attributes.Add("style", "display:none");
                        TempData["cntractsMgr"] = "<a  ID='cntractsMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-notification-sync'></i>Recurring </a>";
                    }

                    string processRCPermission = dtUserPermission.Rows[0]["ProcessRCPermission"].ToString();

                    processRCPermission = processRCPermission.Length < 4 ? "N" : processRCPermission.Substring(3, 1);
                    if (processRCPermission != "Y")
                    {
                        //lnkContractsMenu.NavigateUrl = "";
                        //lnkContractsMenu.Attributes.Add("style", "display:none");
                        TempData["lnkContractsMenu"] = "<a  ID='lnkContractsMenu' href='../RecContracts.aspx'><i class='mdi-image-navigate-next' ></i>Recurring Contracts</a>";
                    }

                    string ProcessC = dtUserPermission.Rows[0]["ProcessC"].ToString();

                    ProcessC = ProcessC.Length < 4 ? "N" : ProcessC.Substring(3, 1);
                    if (ProcessC != "Y")
                    {
                        //lnkInvoicesMenu.NavigateUrl = "";
                        //lnkInvoicesMenu.Attributes.Add("style", "display:none");
                        TempData["lnkInvoicesMenu"] = "<a  ID='lnkInvoicesMenu' href='../recurringinvoices.aspx'><i class='mdi-image-navigate-next' ></i>Recurring Invoices</a>";
                    }

                    string ProcessT = dtUserPermission.Rows[0]["ProcessT"].ToString();

                    ProcessT = ProcessT.Length < 4 ? "N" : ProcessT.Substring(3, 1);
                    if (ProcessT != "Y")
                    {
                        //lnkTicketsMenu.NavigateUrl = "";
                        //lnkTicketsMenu.Attributes.Add("style", "display:none");
                        TempData["lnkTicketsMenu"] = "<a  ID='lnkTicketsMenu' href='../RecurringTickets.aspx'><i class='mdi-image-navigate-next' ></i>Recurring Tickets</a>";
                    }

                    string SafteyTest = dtUserPermission.Rows[0]["SafetyTestsPermission"].ToString();

                    SafteyTest = SafteyTest.Length < 4 ? "N" : SafteyTest.Substring(3, 1);
                    if (SafteyTest != "Y")
                    {
                        //lnk.NavigateUrl = "";
                        //lnk.Attributes.Add("style", "display:none");
                        TempData["lnk"] = "<a  ID='lnk' href='../SafetyTest.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Safety Tests</a>";
                    }

                    string SchedulemodulePermission = dtUserPermission.Rows[0]["SchedulemodulePermission"] == DBNull.Value ? "N" : dtUserPermission.Rows[0]["SchedulemodulePermission"].ToString();

                    if (SchedulemodulePermission != "Y")
                    {
                        //schMgr.NavigateUrl = "";
                        //schMgr.Attributes.Add("style", "display:none");
                        TempData["schMgr"] = "<a  ID='schMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl' ><i class='mdi-editor-insert-invitation'></i>Schedule</a>";
                    }
                    string ScheduleboardPermission = dtUserPermission.Rows[0]["Ticket"].ToString();

                    ScheduleboardPermission = ScheduleboardPermission.Length < 4 ? "N" : ScheduleboardPermission.Substring(3, 1);
                    if (ScheduleboardPermission != "Y")
                    {
                        //lnkScheduleMenu.NavigateUrl = "";
                        //lnkScheduleMenu.Attributes.Add("style", "display:none");
                        TempData["lnkScheduleMenu"] = "<a  ID='lnkScheduleMenu' href='../Scheduler.aspx'><i class='mdi-image-navigate-next' ></i>Schedule</a>";
                    }
                    string TicketListPermission = dtUserPermission.Rows[0]["Dispatch"].ToString();

                    TicketListPermission = TicketListPermission.Length < 4 ? "N" : TicketListPermission.Substring(3, 1);
                    if (TicketListPermission != "Y")
                    {
                        //lnkListView.NavigateUrl = "";
                        //lnkListView.Attributes.Add("style", "display:none");
                        TempData["lnkListView"] = "<a  ID='lnkListView' href='../TicketListView.aspx'><i class='mdi-image-navigate-next' ></i>Ticket List</a>";
                    }
                    string MTimesheetPermission = dtUserPermission.Rows[0]["MTimesheet"].ToString();

                    MTimesheetPermission = MTimesheetPermission.Length < 4 ? "N" : MTimesheetPermission.Substring(3, 1);
                    if (MTimesheetPermission != "Y")
                    {
                        //HyperLink1.NavigateUrl = "";
                        //HyperLink1.Attributes.Add("style", "display:none");
                        TempData["HyperLink1"] = "<a  ID='HyperLink1' href='../ManualTimeCard.aspx'><i class='mdi-image-navigate-next' ></i>Timesheet Entry</a>";
                    }

                    string eTimesheetPermission = dtUserPermission.Rows[0]["ETimesheet"].ToString();

                    eTimesheetPermission = eTimesheetPermission.Length < 4 ? "N" : eTimesheetPermission.Substring(3, 1);
                    if (eTimesheetPermission != "Y")
                    {
                        //lnkTimesheet.NavigateUrl = "";
                        //lnkTimesheet.Attributes.Add("style", "display:none");
                        TempData["lnkTimesheet"] = "<a  ID='lnkTimesheet' href='../etimesheet.aspx'><i class='mdi-image-navigate-next' ></i>e-Timesheet</a>";
                    }

                    string MapPermission = dtUserPermission.Rows[0]["MapR"].ToString();

                    MapPermission = MapPermission.Length < 4 ? "N" : MapPermission.Substring(3, 1);
                    if (MapPermission != "Y")
                    {
                        //lnkMapMenu.NavigateUrl = "";
                        //lnkMapMenu.Attributes.Add("style", "display:none");
                        TempData["lnkMapMenu"] = "<a  ID='lnkMapMenu' href='../Map.aspx'><i class='mdi-image-navigate-next' ></i>Map</a>";
                    }

                    string RouteBuilderPermission = dtUserPermission.Rows[0]["RouteBuilder"].ToString();
                    RouteBuilderPermission = RouteBuilderPermission.Length < 4 ? "N" : RouteBuilderPermission.Substring(3, 1);
                    if (RouteBuilderPermission != "Y")
                    {
                        //lnkRouteBuilder.NavigateUrl = "";
                        //lnkRouteBuilder.Attributes.Add("style", "display:none");
                        TempData["lnkRouteBuilder"] = "<a  ID='lnkRouteBuilder' href='../RouteBuilder.aspx'><i class='mdi-image-navigate-next' ></i>Route Builder</a>";
                    }

                    string SalesManagerModulePermission = dtUserPermission.Rows[0]["SalesManager"] == DBNull.Value ? "N" : dtUserPermission.Rows[0]["SalesManager"].ToString();

                    if (SalesManagerModulePermission != "Y")
                    {
                        //SalesMgr.NavigateUrl = "";
                        //SalesMgr.Attributes.Add("style", "display:none");
                        TempData["SalesMgr"] = "<a  ID='SalesMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-action-wallet-travel'></i>Sales </a>";
                    }

                    string UserSalesPermission = dtUserPermission.Rows[0]["UserSales"].ToString();
                    UserSalesPermission = UserSalesPermission.Length < 4 ? "N" : UserSalesPermission.Substring(3, 1);
                    if (UserSalesPermission != "Y")
                    {
                        //lnkProspect.NavigateUrl = "";
                        //lnkProspect.Attributes.Add("style", "display:none");
                        TempData["lnkProspect"] = "<a  ID='lnkProspect' href='../prospects.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Leads</a>";
                    }

                    int taskPermission = dtUserPermission.Rows[0]["ToDo"] == DBNull.Value ? 0 : Convert.ToInt32(dtUserPermission.Rows[0]["ToDo"]);

                    if (taskPermission == 0)
                    {
                        //lnkTasks.NavigateUrl = "";
                        //lnkTasks.Attributes.Add("style", "display:none");
                        TempData["lnkTasks"] = "<a  ID='lnkTasks' href='../tasks.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Tasks</a>";
                    }

                    string ProposalPermission = dtUserPermission.Rows[0]["Proposal"].ToString();
                    ProposalPermission = ProposalPermission.Length < 4 ? "N" : ProposalPermission.Substring(3, 1);
                    if (ProposalPermission != "Y")
                    {
                        //lnkOpportunities.NavigateUrl = "";
                        //lnkOpportunities.Attributes.Add("style", "display:none");
                        TempData["lnkOpportunities"] = "<a  ID='lnkOpportunities' href='../opportunity.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Opportunities</a>";
                    }

                    string EstimatesPermission = dtUserPermission.Rows[0]["Estimates"].ToString();
                    EstimatesPermission = EstimatesPermission.Length < 4 ? "N" : EstimatesPermission.Substring(3, 1);
                    if (EstimatesPermission != "Y")
                    {
                        //lnkEstimate.NavigateUrl = "";
                        //lnkEstimate.Attributes.Add("style", "display:none");
                        TempData["lnkEstimate"] = "<a  ID='lnkEstimate' href='../estimate.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Estimate</a>";
                    }

                    string TemplatesPermission = dtUserPermission.Rows[0]["ProjecttempPermission"].ToString();
                    TemplatesPermission = TemplatesPermission.Length < 4 ? "N" : TemplatesPermission.Substring(3, 1);
                    if (TemplatesPermission != "Y")
                    {
                        //lnkEstimateTempl.NavigateUrl = "";
                        //lnkEstimateTempl.Attributes.Add("style", "display:none");
                        TempData["lnkEstimateTempl"] = "<a  ID='lnkEstimateTempl' href='../estimatetemplate.aspx'><i class='mdi-image-navigate-next' ></i>Templates</a>";
                    }
                    string salessetupPermission = dtUserPermission.Rows[0]["salessetup"].ToString();
                    salessetupPermission = salessetupPermission.Length < 4 ? "N" : salessetupPermission.Substring(3, 1);
                    if (salessetupPermission != "Y")
                    {
                        //lnkSalesSetup.NavigateUrl = "";
                        //lnkSalesSetup.Attributes.Add("style", "display:none");
                        TempData["lnkSalesSetup"] = "<a  ID='lnkSalesSetup' href='../salessetup.aspx'><i class='mdi-image-navigate-next' ></i>Sales Setup</a>";
                    }




                    string PurchasingmodulePermission = dtUserPermission.Rows[0]["PurchasingmodulePermission"] == DBNull.Value ? "N" : dtUserPermission.Rows[0]["PurchasingmodulePermission"].ToString();

                    if (PurchasingmodulePermission != "Y")
                    {
                        //purchaseMgr.Visible = false;
                        TempData["purchaseMgr"] = "<a  ID='purchaseMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-action-work'></i>Purchasing </a>";
                    }
                    string BillingmodulePermission = dtUserPermission.Rows[0]["BillingmodulePermission"] == DBNull.Value ? "N" : dtUserPermission.Rows[0]["BillingmodulePermission"].ToString();

                    if (BillingmodulePermission != "Y")
                    {
                        //acctMgr.NavigateUrl = "";
                        //acctMgr.Visible = false;
                        TempData["acctMgr"] = "<a  ID='acctMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-action-payment'></i>Billing </a>";
                    }

                    string AccountPayablemodulePermission = dtUserPermission.Rows[0]["AccountPayablemodulePermission"] == DBNull.Value ? "N" : dtUserPermission.Rows[0]["AccountPayablemodulePermission"].ToString();

                    if (AccountPayablemodulePermission != "Y")
                    {
                        //acctPayable.NavigateUrl = "";
                        //acctPayable.Visible = false;
                        TempData["acctPayable"] = "<a  ID='acctPayable' class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-action-account-balance'></i>AP</a>";
                    }
                }

                if (role != string.Empty)
                    //lblUser.Text += "/" + role;

                    if (Sales == "Y")
                    {
                        //lnkProspect.Visible = true;
                        TempData["lnkProspect"] = "<a  ID='lnkProspect' href='../prospects.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Leads</a>";
                    }
                    else
                    {
                        //lnkProspect.Visible = false;
                        TempData["lnkProspect"] = "<a  ID='lnkProspect' href='../prospects.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Leads</a>";
                    }

                CustomerPortalPermissions(usertype);

                if (ProgFunc == "Y")
                {
                    //progMgr.Visible = true;
                    TempData["progMgr"] = "<a  ID='progMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-action-extension'></i>Programs </a>";
                    if (AccessUser != "Y")
                    {
                        if (new GeneralFunctions().GetSalesAsigned() == 0)
                        {
                            //lnkUsersSMenu.NavigateUrl = "";
                            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyusermaster5", " $(" + lnkUsersSMenu.ClientID + ").click(function(event) { noty({text: 'You do not have permissions to access users!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 1000,theme : 'noty_theme_default',  closable : true , dismissQueue: true});  });", true);
                            TempData["lnkUsersSMenu"] = "<a  ID='lnkUsersSMenu' href='../Users.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Users</a>";
                        }
                    }
                }
                else
                {
                    //progMgr.Visible = false;
                    TempData["progMgr"] = "<a  ID='progMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-action-extension'></i>Programs </a>";
                }

                if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
                {

                    //cntractsMgr.Visible = false;
                    //acctMgr.Visible = false;
                    //lblSuper.Visible = true;
                    //lnkCntrlPnl.Visible = false;
                    //lnkCustomFields.Visible = false;
                    //lnkCustomersSmenu.Visible = false;
                    //lnkLocationsSMenu.Visible = false;
                    TempData["cntractsMgr"] = "";
                    TempData["acctMgr"] = "";
                    TempData["lblSuper"] = "";
                    TempData["lnkCntrlPnl"] = "";
                    TempData["lnkCustomFields"] = "";
                    TempData["lnkCustomersSmenu"] = "";
                    TempData["lnkLocationsSMenu"] = "";


                }



            }
        }
        private void DrawControls()
        {
            TempData["Qbvisible"] = "0";
            TempData["lblQblastSync"] = "";

            TempData["liSelectCompany"] = "";
            TempData["lnkManageCompanies"] = "";

            TempData["lnkPaymentHistory"] = "<a  ID='lnkPaymentHistory' href='../paymenthistory.aspx'><i class='mdi-image-navigate-next' ></i>Online Payment</a>";
            /*****************IF logged in user is not Admin**************/
            TempData["cstmMgr"] = "<a  ID='cstmMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-social-people' ></i>Customers</a>";
            TempData["ProjectMgr"] = "<a  ID='ProjectMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-action-assignment' ></i>Projects</a>";
            TempData["InventoryMgr"] = "<a  ID='InventoryMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-editor-format-align-justify' ></i>Inventory</a>";
            TempData["lnkEquipmentsSMenu"] = "<a  ID='lnkEquipmentsSMenu' href='../Equipments.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Equipment</a>";
            TempData["lnkProject"] = "<a  ID='lnkProject' href='../project.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Projects</a>";

            TempData["lnkDepartments"] = "<a  ID='lnkDepartments' href='../setup.aspx?tab=1'><i class='mdi-image-navigate-next' ></i>Departments</a>";

            TempData["lnkLocationsSMenu"] = "<a  ID='lnkLocationsSMenu' href='../locations.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Locations</a>";
            TempData["lnkCustomersSmenu"] = "<a  ID='lnkCustomersSmenu' href='../Customers.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Customers</a>";
            TempData["lnkReceivePayment"] = "<a  ID='lnkReceivePayment' href='../receivepayment.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Receive Payment</a>";
            TempData["lnkDeposit"] = "<a  ID='lnkDeposit' href='../managedeposit.aspx'><i class='mdi-image-navigate-next' ></i>Make Deposit</a>";
            TempData["lnkCollections"] = "<a  ID='lnkCollections' href='../iCollections.aspx'><i class='mdi-image-navigate-next' ></i>Collections</a>";
            TempData["lnkListView"] = "<a  ID='lnkListView' href='../TicketListView.aspx'><i class='mdi-image-navigate-next' ></i>Ticket List</a>";
            TempData["lnkItemMaster"] = "<a  ID='lnkItemMaster' href='../Inventory.aspx'><i class='mdi-image-navigate-next' ></i>Item Master</a>";
            TempData["lnkAdjustment"] = "<a  ID='lnkAdjustment' href='../InventoryAdjustments.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Item Adjustment</a>";
            TempData["lnkProjectTempl"] = "<a  ID='lnkProjectTempl' href='../projecttemplate.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Project Templates</a>";
            TempData["lnkEstimateTempl"] = "<a  ID='lnkEstimateTempl' href='../estimatetemplate.aspx'><i class='mdi-image-navigate-next' ></i>Templates</a>";
            TempData["lnkInvoicesSMenu"] = "<a  ID='lnkInvoicesSMenu' href='../invoices.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Invoices</a>";
            TempData["lnkPO"] = "<a  ID='lnkPO' href='../managepo.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Purchase Orders</a>";
            TempData["lnkReceivePO"] = "<a  ID='lnkReceivePO' href='../managereceivepo.aspx'><i class='mdi-image-navigate-next' ></i>Receive PO</a>";
            TempData["lnkBillcodeSMenu"] = "<a  ID='lnkBillcodeSMenu' href='../billingcodes.aspx'><i class='mdi-image-navigate-next' ></i>Billing Codes</a>";
            TempData["lnkPaymentHistory"] = "<a  ID='lnkPaymentHistory' href='../paymenthistory.aspx'><i class='mdi-image-navigate-next' ></i>Online Payment</a>";

            //TempData["lnkPartsSMenu"] = "<a  ID='lnkPartsSMenu' href='../parts.aspx'><i class='mdi-image-navigate-next' ></i>Parts</a>";

            TempData["lnkWriteCheck2"] = "<a  ID='lnkWriteCheck2' href='../managechecks.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Manage Checks</a>";
            TempData["lnkWriteCheck"] = "<a  ID='lnkWriteCheck' href='../managechecks.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Manage Checks</a>";
            TempData["lnkAddBill"] = "<a  ID='lnkAddBill' href='../managebills.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Bills</a>";
            TempData["lnkVendors"] = "<a  ID='lnkVendors' href='../vendors.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Vendors</a>";
            TempData["financeMgr"] = "<a  ID='financeMgr' class = 'collapsible-header waves-effect waves-cyan collapsible-height-nl' ><i class='mdi-editor-attach-money'></i>Financials</a>";
            TempData["lnkCOA"] = "<a  ID='lnkCOA' href='../chartofaccount.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Chart of Accounts</a>";
            TempData["lnkBankRecon"] = "<a  ID='lnkBankRecon' href='../bankrecon.aspx'><i class='mdi-image-navigate-next' ></i>Bank Reconciliation</a>";
            TempData["lnkJournalEntry"] = "<a  ID='lnkJournalEntry' href='../journalentry.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Journal Entry</a>";
            TempData["cntractsMgr"] = "<a  ID='cntractsMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-notification-sync'></i>Recurring </a>";
            TempData["lnkContractsMenu"] = "<a  ID='lnkContractsMenu' href='../RecContracts.aspx'><i class='mdi-image-navigate-next' ></i>Recurring Contracts</a>";
            TempData["lnkInvoicesMenu"] = "<a  ID='lnkInvoicesMenu' href='../recurringinvoices.aspx'><i class='mdi-image-navigate-next' ></i>Recurring Invoices</a>";
            TempData["lnkTicketsMenu"] = "<a  ID='lnkTicketsMenu' href='../RecurringTickets.aspx'><i class='mdi-image-navigate-next' ></i>Recurring Tickets</a>";
            TempData["lnk"] = "<a  ID='lnk' href='../SafetyTest.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Safety Tests</a>";
            TempData["schMgr"] = "<a  ID='schMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl' ><i class='mdi-editor-insert-invitation'></i>Schedule</a>";
            TempData["lnkScheduleMenu"] = "<a  ID='lnkScheduleMenu' href='../Scheduler.aspx'><i class='mdi-image-navigate-next' ></i>Schedule</a>";
            TempData["lnkListView"] = "<a  ID='lnkListView' href='../TicketListView.aspx'><i class='mdi-image-navigate-next' ></i>Ticket List</a>";
            TempData["HyperLink1"] = "<a  ID='HyperLink1' href='../ManualTimeCard.aspx'><i class='mdi-image-navigate-next' ></i>Timesheet Entry</a>";
            TempData["lnkTimesheet"] = "<a  ID='lnkTimesheet' href='../etimesheet.aspx'><i class='mdi-image-navigate-next' ></i>e-Timesheet</a>";
            TempData["lnkMapMenu"] = "<a  ID='lnkMapMenu' href='../Map.aspx'><i class='mdi-image-navigate-next' ></i>Map</a>";
            TempData["lnkRouteBuilder"] = "<a  ID='lnkRouteBuilder' href='../RouteBuilder.aspx'><i class='mdi-image-navigate-next' ></i>Route Builder</a>";
            //TempData["lnkRoutes"] = "<a  ID='lnkRoutes' href='../Routes.aspx'><i class='mdi-image-navigate-next' ></i>Routes</a>";
            TempData["lnkRoutes"] = "";
            TempData["SalesMgr"] = "<a  ID='SalesMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-action-wallet-travel'></i>Sales </a>";
            TempData["lnkProspect"] = "<a  ID='lnkProspect' href='../prospects.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Leads</a>";
            TempData["lnkTasks"] = "<a  ID='lnkTasks' href='../tasks.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Tasks</a>";
            TempData["lnkOpportunities"] = "<a  ID='lnkOpportunities' href='../opportunity.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Opportunities</a>";
            TempData["lnkEstimate"] = "<a  ID='lnkEstimate' href='../estimate.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Estimate</a>";
            TempData["lnkEstimateTempl"] = "<a  ID='lnkEstimateTempl' href='../estimatetemplate.aspx'><i class='mdi-image-navigate-next' ></i>Templates</a>";
            TempData["lnkSalesSetup"] = "<a  ID='lnkSalesSetup' href='../salessetup.aspx'><i class='mdi-image-navigate-next' ></i>Sales Setup</a>";
            TempData["purchaseMgr"] = "<a  ID='purchaseMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-action-work'></i>Purchasing </a>";
            TempData["acctMgr"] = "<a  ID='acctMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-action-payment'></i>Billing </a>";
            TempData["acctPayable"] = "<a  ID='acctPayable' class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-action-account-balance'></i>AP</a>";
            TempData["lnkProspect"] = "<a  ID='lnkProspect' href='../prospects.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Leads</a>";
            TempData["progMgr"] = "<a  ID='progMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-action-extension'></i>Programs </a>";
            TempData["lnkUsersSMenu"] = "<a  ID='lnkUsersSMenu' href='../Users.aspx?f=c'><i class='mdi-image-navigate-next' ></i>Users</a>";

            TempData["financialStatement"] = "<a  ID='financialStatement' class='collapsible-header waves-effect waves-cyan collapsible-height-nl' ><i class='mdi-editor-attach-money'></i>Statements</a>";
            TempData["lnkTrialBalance"] = "<a  ID='lnkTrialBalance' href='../trialbalance.aspx'><i class='mdi-image-navigate-next' ></i>Trial Balance</a>";
            TempData["lnkIncomeStatement"] = "<a  ID='lnkIncomeStatement' href='../incomestatement.aspx'><i class='mdi-image-navigate-next' ></i>Profit and Loss</a>";
            TempData["lnkComparativeStatement"] = "<a  ID='lnkComparativeStatement' href='../ComparativeStatement.aspx'><i class='mdi-image-navigate-next' ></i>Comparative Report</a>";
            TempData["lnkBalanceSheet"] = "<a  ID='lnkBalanceSheet' href='../balancesheet.aspx'><i class='mdi-image-navigate-next' ></i>Balance Sheet</a>";
            TempData["lnkBudgets"] = "<a  ID='lnkBudgets' href='../Budgets.aspx'><i class='mdi-image-navigate-next' ></i>Budgets</a>";

            TempData["ReportMgr"] = "<a class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-editor-insert-chart'></i>Reports </a>";

            TempData["lnkSetup"] = "<a  ID='lnkSetup' href='../setup.aspx'><i class='mdi-image-navigate-next' ></i>Setup</a>";
            TempData["lnkCntrlPnl"] = "<a  ID='lnkCntrlPnl' href='../controlpanel.aspx'><i class='mdi-image-navigate-next' ></i>Control Panel</a>";
            TempData["lnkCustomFields"] = "<a  ID='lnkCustomFields' href='../CustomFields.aspx'><i class='mdi-image-navigate-next' ></i>Custom Labels</a>";
            TempData["lnkPeriodCloseOut"] = "<a  ID='lnkPeriodCloseOut' href='../periodcloseout.aspx'><i class='mdi-image-navigate-next' ></i>Period Close Out</a>";
            TempData["lnkManageCompanies"] = "<a  ID='lnkManageCompanies' href='../ManageCompanies.aspx'><i class='mdi-image-navigate-next' ></i>Manage Companies</a>";

            TempData["linkDashboarDefault"] = "<a ID='linkDashboarDefault' waves-effect waves-cyan collapsible-height-nl' href ='~/Home.aspx' ><i class='mdi-action-dashboard no-collapse'></i>Dashboard</a>"; 
        }
        private void FinancialPermissions()
        {
            bool _addFinance = (bool)Session["AddFinance"];     //Check FM permission
            bool _editFinance = (bool)Session["EditFinance"];
            bool _viewFinance = (bool)Session["ViewFinance"];
            if (Session["FinanceManager"].ToString() == "F")
            {

            }
            else if (_addFinance.Equals(true) || _editFinance.Equals(true) || _viewFinance.Equals(true))
            {

            }
            else
            {

            }


            if ((bool)Session["FinanceStatement"].Equals(true))     // Check FS permission
            {
                //financialStatement.Visible = true;
                //lnkTrialBalance.Visible = true;
                //lnkIncomeStatement.Visible = true;
                //lnkBalanceSheet.Visible = true;
                //lnkComparativeStatement.Visible = true;

                TempData["financialStatement"] = "<a  ID='financialStatement' class='collapsible-header waves-effect waves-cyan collapsible-height-nl' ><i class='mdi-editor-attach-money'></i>Statements</a>";
                TempData["lnkTrialBalance"] = "<a  ID='lnkTrialBalance' href='../trialbalance.aspx'><i class='mdi-image-navigate-next' ></i>Trial Balance</a>";
                TempData["lnkIncomeStatement"] = "<a  ID='lnkIncomeStatement' href='../incomestatement.aspx'><i class='mdi-image-navigate-next' ></i>Profit and Loss</a>";
                TempData["lnkComparativeStatement"] = "<a  ID='lnkComparativeStatement' href='../ComparativeStatement.aspx'><i class='mdi-image-navigate-next' ></i>Comparative Report</a>";
                TempData["lnkBalanceSheet"] = "<a  ID='lnkBalanceSheet' href='../balancesheet.aspx'><i class='mdi-image-navigate-next' ></i>Balance Sheet</a>";
                
            }
            else
            {
                //financialStatement.Visible = false;
                //lnkTrialBalance.Visible = false;
                //lnkIncomeStatement.Visible = false;
                //lnkBalanceSheet.Visible = false;
                //lnkComparativeStatement.Visible = false;
                TempData["financialStatement"] = "";
                TempData["lnkTrialBalance"] = "";
                TempData["lnkIncomeStatement"] = "";
                TempData["lnkComparativeStatement"] = "";
                TempData["lnkBalanceSheet"] = "";
            }
        }
        private void CustomerPortalPermissions(string usertype)
        {
            /**Exclude the menu items for customer portal**/
            if (usertype == "c")
            {

                TempData["schMgr"] = "<a  ID='schMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-editor-insert-invitation'></i>Schedule </a>";
                //cntractsMgr.Visible = false;
                TempData["cntractsMgr"] = "";
                //acctMgr.Visible = true;
                TempData["acctMgr"] = "<a  ID='acctMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-action-payment'></i>Billing </a>";
                //SalesMgr.Visible = false;
                TempData["SalesMgr"] = "";
                //progMgr.Visible = false;
                TempData["progMgr"] = "";
                //ProjectMgr.Visible = false;
                TempData["ProjectMgr"] = "";
                ////notifications.Visible = false;
                //lnkBillcodeSMenu.Visible = false;
                TempData["lnkBillcodeSMenu"] = "";
                //lnkScheduleMenu.Visible = false;
                TempData["lnkScheduleMenu"] = "";
                //lnkMapMenu.Visible = false;
                TempData["lnkMapMenu"] = "";
                //lnkRouteBuilder.Visible = false;
                TempData["lnkRouteBuilder"] = "";
                ////lnkRoutes.Visible = false;
                //lnkProspect.Visible = false;
                TempData["lnkProspect"] = "";
                //divBreadCrumbWithQB.Visible = false;
                //divQBContents.Visible = false;
                TempData["Qbvisible"] = "0";
                //lnkTimesheet.Visible = false;
                TempData["lnkTimesheet"] = "";
                //ReportMgr.Visible = false;
                TempData["ReportMgr"] = "";                
                //purchaseMgr.Visible = false;
                TempData["purchaseMgr"] = "";
                //InventoryMgr.Visible = false;
                TempData["InventoryMgr"] = "";
                if (Session["ticketo"].ToString() == "1")
                {
                    TempData["schMgr"] = "<a  ID='schMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-editor-insert-invitation'></i>Schedule </a>";
                }
                else
                {
                    TempData["schMgr"] = "";
                }

                if (Session["invoice"].ToString() == "1")
                {
                    TempData["acctMgr"] = "<a  ID='acctMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-action-payment'></i>Billing </a>";
                }
                else
                {
                    TempData["acctMgr"] = "";
                }

                if (Session["CPE"].ToString() == "1")
                {
                    TempData["cstmMgr"] = "<a  ID='cstmMgr' class='collapsible-header waves-effect waves-cyan collapsible-height-nl'><i class='mdi-social-people' ></i>Customers</a>";
                    TempData["lnkCustomersSmenu"] = "";
                    TempData["lnkLocationsSMenu"] = "";
                }
                else
                    TempData["cstmMgr"] = "";
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
                            //lblQblastSync.Text = strLastSync;
                            //lnkLastsync.Text = "Quickbooks Last Sync : ";
                            TempData["lblQblastSync"] = strLastSync;
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
                            //lblQblastSync.Text = strLastSync;
                            //lnkLastsync.Text = "Sage Last Sync : ";
                            TempData["lblQblastSync"] = strLastSync;
                            visible = 1;
                        }
                    }
                }
            }
            TempData["Qbvisible"] = visible.ToString();
            if (visible == 0)
            {
                //divBreadCrumbWithQB.Visible = false;
                //divQBContents.Visible = false;
            }
        }

    }
}