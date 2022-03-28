using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Web.UI.HtmlControls;
using ImapX;
using System.Web.Script.Serialization;
using Telerik.Web.UI;
using System.Linq.Dynamic;
using MailKit.Security;
using System.Text;
using System.IO;

public partial class AddUser : System.Web.UI.Page
{
    #region Properties

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    BL_Customer objBL_Customer = new BL_Customer();
    BL_ReportsData objBL_Report = new BL_ReportsData();

    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    BusinessEntity.CompanyOffice objCompany = new BusinessEntity.CompanyOffice();
    BL_Company objBL_Company = new BL_Company();

    MapData objMapData = new MapData();
    BL_MapData objBL_MapData = new BL_MapData();

    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    protected DataTable dtWage = new DataTable();
    Wage objWage = new Wage();
    public bool check = false;
    bool isGrouping = false;

    #endregion

    #region Get/Set
    //private bool _CheckEdit;
    //public bool CheckEdit
    //{
    //    get { return _CheckEdit; }
    //    set { _CheckEdit = value; }
    //}
    #endregion

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        //if (Request.QueryString["sup"] != null)
        //{
        //    Page.MasterPageFile = "popup.master";
        //}
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        UserPermission();

        WebBaseUtility.UpdatePageTitle(this, "User", Request.QueryString["uid"], Request.QueryString["t"]);

        GetControlForPayroll();

        if (!IsPostBack)
        {
            GetControlData();
            ClearControls();
            ViewState["mode"] = 0;
            ViewState["super"] = 0;

            // ES-33
            ViewState["IsSetEmailAccount"] = false;
            if (Convert.ToInt16(Session["payment"]) != 1)
            {
                if (Session["type"].ToString() == "c")
                {
                    trOnlinePayment.Attributes.Add("style", "display:none");
                }
            }
            FillCompany();
            if (Request.QueryString["sup"] != null)
            {
                ViewState["super"] = 1;
                lblHeader.Text = "Add Supervisor";
                ddlSuper.Enabled = false;
                lnkClose.Visible = false;
                lnkCancelContact.Visible = true;
                btnSubmit.Visible = true;
                ddlUserType.SelectedValue = "1";
                ddlUserType_SelectedIndexChanged(sender, e);
                ddlUserType.Enabled = false;
                chkSuper.Checked = true;
                chkSuper.Enabled = false;
            }

            FillDepartment();
            FillSupervisor();
            GetControl();
            GetMerchantID();
            FillUserRole();

            if (ViewState["InInActiveWageUser"] != null)
            {
                if (ViewState["InInActiveWageUser"].ToString() == "True")
                {
                    check = true;
                    lnkChk.Checked = true;
                }
                else
                {
                    check = false;
                    lnkChk.Checked = false;
                }
            }
            else
            {
                check = false;
                lnkChk.Checked = false;
            }

            if (Request.QueryString["uid"] != null)
            {
                if (Request.QueryString["t"] != null)
                {
                    ViewState["mode"] = 0;
                    lblHeader.Text = "Copy User";
                    pnlSave.Visible = false;
                }
                else
                {
                    lblHeader.Text = "Edit User";
                    ViewState["mode"] = 1;
                    pnlSave.Visible = true;
                    txtUserName.Enabled = false;
                    liLogs.Style["display"] = "inline-block";
                    tbLogs.Style["display"] = "block";
                    pnlEmailSignature.Visible = true;
                }

                pnlNext.Visible = true;
                objPropUser.UserID = Convert.ToInt32(Request.QueryString["uid"]);
                objPropUser.TypeID = Convert.ToInt32(Request.QueryString["type"]);
                objPropUser.DBName = Session["dbname"].ToString();
                DataSet ds = new DataSet();
                ds = objBL_User.GetUserInfoByID(objPropUser);
                ViewState["getUserById"] = ds;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (Request.QueryString["type"].ToString() == "2" && Request.QueryString["t"] == null)
                    {
                        ddlUserType.Items.Insert(2, new ListItem("Customer", "2"));
                        tblPermission.Visible = false;
                        txtHireDt.Visible = false;
                        txtTerminationDt.Visible = false;

                        ddlUserType.SelectedIndex = 2;
                        ddlUserType.Enabled = false;
                        chkMap.AutoPostBack = false;
                        btnSubmit.Visible = false;
                        chkSalesperson.Visible = false;
                        chkSalesAssigned.Visible = false;
                        chkNotification.Visible = false;
                        if (ds.Tables[0].Rows[0]["ticketo"].ToString() == "1")
                        {
                            chkScheduleBrd.Checked = true;
                        }
                        if (ds.Tables[0].Rows[0]["ticketd"].ToString() == "1")
                        {
                            chkMap.Checked = true;
                        }
                        ddlLang.Visible = false;
                        lblMultiLang.Visible = false;
                    }
                    else
                    {
                        string map = ds.Tables[0].Rows[0]["ticket"].ToString().Substring(3, 1);
                        string sch = ds.Tables[0].Rows[0]["ticket"].ToString().Substring(0, 1);

                        if (map == "Y")
                        {
                            chkMap.Checked = true;
                        }

                        if (ds.Tables[0].Rows[0]["dboard"] != DBNull.Value)
                        {
                            if (ds.Tables[0].Rows[0]["dboard"].ToString().Trim() != string.Empty)
                            {
                                int schTS = Convert.ToInt32(ds.Tables[0].Rows[0]["dboard"]);
                                if (schTS == 1)
                                {
                                    chkScheduleBrd.Checked = true;
                                }
                                else
                                {
                                    chkScheduleBrd.Checked = false;
                                }
                            }
                        }

                        txtMsg.Text = ds.Tables[0].Rows[0]["pager"].ToString();
                        string lang = "english";
                        if (ds.Tables[0].Rows[0]["Lang"].ToString().ToLower() != "none")
                        {
                            lang = ds.Tables[0].Rows[0]["Lang"].ToString().ToLower();
                        }
                        ddlLang.SelectedValue = lang;
                        ddlMerchantID.SelectedValue = ds.Tables[0].Rows[0]["merchantinfoid"].ToString();
                        if (Session["MSM"].ToString() != "TS")
                        {
                            if (ds.Tables[0].Rows[0]["sales"].ToString() == "1")
                            {
                                chkSalesperson.Checked = true;
                                chkNotification.Enabled = chkSalesAssigned.Enabled = true;

                                chkSalesAssigned.Checked = ds.Tables[0].Rows[0]["SalesAssigned"] == null ? false : Convert.ToBoolean(ds.Tables[0].Rows[0]["SalesAssigned"]);
                                chkNotification.Checked = ds.Tables[0].Rows[0]["NotificationOnAddOpportunity"] == null ? false : Convert.ToBoolean(ds.Tables[0].Rows[0]["NotificationOnAddOpportunity"]);
                            }
                            else
                            {
                                chkNotification.Checked = chkSalesAssigned.Checked = false;
                            }

                            if (Convert.ToInt16(ds.Tables[0].Rows[0]["emailaccount"]) == 1)
                            {
                                chkEmailAcc.Checked = true;
                                pnlEmailAccount.Visible = true;
                                rfvEmail.Enabled = true;
                            }
                            else
                            {
                                chkEmailAcc.Checked = false;
                                pnlEmailAccount.Visible = false;
                                rfvEmail.Enabled = false;
                            }
                        }
                    }
                    chkEstApprovalStatus.Checked = ds.Tables[0].Rows[0]["EstApproveProposal"] == null ? false : Convert.ToBoolean(ds.Tables[0].Rows[0]["EstApproveProposal"]);

                    ViewState["userid"] = ds.Tables[0].Rows[0]["userid"].ToString();
                    ViewState["rolid"] = ds.Tables[0].Rows[0]["rolid"].ToString();
                    ViewState["empid"] = ds.Tables[0].Rows[0]["empid"].ToString();
                    ViewState["workid"] = ds.Tables[0].Rows[0]["workid"].ToString();

                    txtAddress.Value = ds.Tables[0].Rows[0]["Address"].ToString();
                    txtCell.Text = ds.Tables[0].Rows[0]["Cellular"].ToString();
                    txtCity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                    if (ds.Tables[0].Rows[0]["DFired"].ToString() != string.Empty)
                    {
                        txtTerminationDt.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["DFired"].ToString()).ToShortDateString();
                    }
                    if (ds.Tables[0].Rows[0]["DHired"].ToString() != string.Empty)
                    {
                        txtHireDt.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["DHired"].ToString()).ToShortDateString();
                    }
                    txtEmail.Text = ds.Tables[0].Rows[0]["EMail"].ToString();
                    txtFName.Text = ds.Tables[0].Rows[0]["fFirst"].ToString();
                    txtLName.Text = ds.Tables[0].Rows[0]["Last"].ToString();
                    if (ViewState["mode"].ToString() != "0")
                        lblUserName.Text = ds.Tables[0].Rows[0]["fUser"].ToString();
                    txtMName.Text = ds.Tables[0].Rows[0]["Middle"].ToString();
                    txtPassword.Text = ds.Tables[0].Rows[0]["Password"].ToString();
                    ddlState.Text = ds.Tables[0].Rows[0]["State"].ToString();
                    rbStatus.SelectedValue = ds.Tables[0].Rows[0]["Status"].ToString();
                    hdnLocCount.Value = ds.Tables[0].Rows[0]["LocCount"].ToString();
                    var imageUrl = ds.Tables[0].Rows[0].Field<string>("ProfileImage");
                    if (!string.IsNullOrWhiteSpace(imageUrl))
                    {
                        ProfileImage.ImageUrl = imageUrl;
                    }

                    if ((Request.QueryString["type"].ToString() == "0") && ds.Tables[0].Rows[0]["fUser"].ToString() == "ADMIN")
                    {
                        rbStatus.Enabled = false;
                    }
                    else
                    {
                        rbStatus.Enabled = true;
                    }
                    txtTelephone.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
                    txtUserName.Text = ds.Tables[0].Rows[0]["fUser"].ToString();
                    txtZip.Text = ds.Tables[0].Rows[0]["Zip"].ToString();
                    ddlUserType.SelectedValue = ds.Tables[0].Rows[0]["Field"].ToString();
                    txtPOLimit.Text = ds.Tables[0].Rows[0]["POLimit"].ToString();
                    ddlPOApprove.SelectedValue = ds.Tables[0].Rows[0]["POApprove"].ToString();
                    ddlPOApproveAmt.SelectedValue = ds.Tables[0].Rows[0]["POApproveAmt"].ToString();
                    txtMinAmount.Text = ds.Tables[0].Rows[0]["MinAmount"].ToString();
                    txtMaxAmount.Text = ds.Tables[0].Rows[0]["MaxAmount"].ToString();
                    if (ddlPOApprove.SelectedValue == "0")
                    {
                        //divMinAmount.Attributes.Remove("visibility");
                        //divMaxAmount.Attributes.Remove("visibility");
                        //divApprovePo.Attributes.Remove("visibility");
                        //divApprovePo.Visible = false;
                        divApprovePo.Style["display"] = "none";
                        divMinAmount.Style["display"] = "none";
                        divMaxAmount.Style["display"] = "none";
                    }
                    else
                    {
                        divApprovePo.Style["display"] = "block";

                        //divMinAmount.Style.Add("visibility", "visible");
                        //divMaxAmount.Style.Add("visibility", "visible");
                        //divApprovePo.Style.Add("visibility", "visible");
                        //divApprovePo.Visible = true;

                        if (ddlPOApproveAmt.SelectedValue == "0")
                        {
                            divMinAmount.Style["display"] = "block";
                            divMaxAmount.Style["display"] = "block";
                            //divMinAmount.Style.Add("visibility", "visible");
                            //divMaxAmount.Style.Add("visibility", "visible");
                            //divMinAmount.Visible = true;
                            //divMaxAmount.Visible = true;
                        }
                        else if (ddlPOApproveAmt.SelectedValue == "1")
                        {
                            divMinAmount.Style["display"] = "block";
                            divMaxAmount.Style["display"] = "none";
                            //divMaxAmount.Attributes.Remove("visibility");
                            //divMaxAmount.Visible = false;
                        }
                        else
                        {
                            divMinAmount.Style["display"] = "none";
                            divMaxAmount.Style["display"] = "none";
                            //divMinAmount.Attributes.Remove("visibility");
                            //divMaxAmount.Attributes.Remove("visibility");
                            //divMinAmount.Visible = false;
                            //divMaxAmount.Visible = false;
                        }
                    }

                    txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                    txtDeviceID.Text = ds.Tables[0].Rows[0]["PDASerialNumber"].ToString();
                    chkDefaultWorker.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["DefaultWorker"]);
                    chkMassReview.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["massreview"]);
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["hourlyrate"].ToString()))
                    {
                        txtHourlyRate.Text = String.Format("{0:C}", Convert.ToDouble(ds.Tables[0].Rows[0]["hourlyrate"]));
                    }
                    //txtMOMUserName.Text = ds.Tables[0].Rows[0]["msmuser"].ToString();
                    //txtMOMPassword.Text = ds.Tables[0].Rows[0]["msmpass"].ToString();
                    ddlPayMethod.SelectedValue = ds.Tables[0].Rows[0]["pmethod"].ToString();

                    ddlPayMethod_SelectedIndexChanged1(sender, e);
                    txtHours.Text = ds.Tables[0].Rows[0]["phour"].ToString();
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["salary"].ToString()))
                    {
                        txtAmount.Text = String.Format("{0:C}", Convert.ToDouble(ds.Tables[0].Rows[0]["salary"]));
                    }
                    lblHours.Attributes.Add("class", "lblHours active");
                    lblAmount.Attributes.Add("class", "lblAmount active");
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["mileagerate"].ToString()))
                    {
                        txtMileageRate.Text = String.Format("{0:C}", Convert.ToDouble(ds.Tables[0].Rows[0]["mileagerate"]));
                    }
                    txtEmpID.Text = ds.Tables[0].Rows[0]["ref"].ToString();
                    ddlPayPeriod.SelectedValue = ds.Tables[0].Rows[0]["payperiod"].ToString();
                    txtLatitude.Text = ds.Tables[0].Rows[0]["Lat"].ToString();
                    txtLongitude.Text = ds.Tables[0].Rows[0]["Lng"].ToString();
                    ddlCountry.SelectedValue = ds.Tables[0].Rows[0]["Country"].ToString();
                    txtEmName.Text = ds.Tables[0].Rows[0]["Contact"].ToString();
                    txtEmNum.Text = ds.Tables[0].Rows[0]["Website"].ToString();
                    txtAuthdevID.Text = ds.Tables[0].Rows[0]["MSDeviceId"].ToString();
                    txtUserTitle.Text = ds.Tables[0].Rows[0]["Title"].ToString();
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["fStart"].ToString()))
                    {
                        txtStartDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["fStart"]).ToString("MM/dd/yyyy");
                    }
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["fEnd"].ToString()))
                    {
                        txtEndDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["fEnd"]).ToString("MM/dd/yyyy");
                    }
                    txtSSNSIN.Text = ds.Tables[0].Rows[0]["SSN"].ToString();
                    ddlGender.SelectedValue = ds.Tables[0].Rows[0]["Sex"].ToString();
                    ddlEthnicity.SelectedValue = ds.Tables[0].Rows[0]["Race"].ToString();
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["DBirth"].ToString()))
                    {
                        txtDateOfBirth.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["DBirth"]).ToString("MM/dd/yyyy");
                    }
                    chkProjectManager.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsProjectManager"]);
                    chkAssignedProject.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsAssignedProject"]);
                    chkLaborExpense.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsReCalculateLaborExpense"]);
                    Session["IsReCalculateLaborExpense"] = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsReCalculateLaborExpense"]);

                    var userRole = ds.Tables[0].Rows[0]["RoleId"].ToString();
                    ddlUserRole.SelectedValue = userRole;
                    hdnUserRoleOrg.Value = userRole;
                    ddlApplyUserRolePermission.SelectedValue = ds.Tables[0].Rows[0]["ApplyUserRolePermission"].ToString();
                    hdnApplyUserRolePermissionOrg.Value = ds.Tables[0].Rows[0]["ApplyUserRolePermission"].ToString();
                    if (ddlUserRole.SelectedValue != "0")
                    {
                        //divApplyUserRolePermission.Style["display"] = "inline-block";
                        //divApplyUserRolePermission.Visible = true;
                    }
                    else
                    {
                        //divApplyUserRolePermission.Style["display"] = "none";
                        //divApplyUserRolePermission.Visible = false;
                    }

                    // TODO: Thomas need to check for ES-33
                    if (ds.Tables[1] != null)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            txtInServer.Text = ds.Tables[1].Rows[0]["InServer"].ToString();
                            txtInUSername.Text = ds.Tables[1].Rows[0]["InUsername"].ToString();
                            // just for mark password

                            txtInPassword.Text = ds.Tables[1].Rows[0]["InPassword"].ToString();
                            //txtInPassword.Attributes.Add("value", txtInPassword.Text);

                            txtinPort.Text = ds.Tables[1].Rows[0]["InPort"].ToString();
                            txtOutServer.Text = ds.Tables[1].Rows[0]["OutServer"].ToString();
                            txtOutUsername.Text = ds.Tables[1].Rows[0]["OutUsername"].ToString();
                            // just for mark password
                            txtOutPassword.Text = ds.Tables[1].Rows[0]["OutPassword"].ToString();
                            //txtOutPassword.Attributes.Add("value", txtOutPassword.Text);

                            txtOutPort.Text = ds.Tables[1].Rows[0]["OutPort"].ToString();
                            txtBccEmail.Text = ds.Tables[1].Rows[0]["BccEmail"].ToString();
                            chkSSL.Checked = Convert.ToBoolean(ds.Tables[1].Rows[0]["SSL"].ToString());
                            chkTakeASentEmailCopy.Checked = Convert.ToBoolean(ds.Tables[1].Rows[0]["TakeASentEmailCopy"].ToString());

                            // ES-33
                            // check and update status of InServer value from database
                            ViewState["IsSetEmailAccount"] = !string.IsNullOrWhiteSpace(ds.Tables[1].Rows[0]["InServer"].ToString());
                        }
                    }
                    if (ds.Tables[2] != null)
                    {
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            foreach (RadComboBoxItem li in ddlDepartment.Items)
                            {
                                if (dr["department"].ToString().Equals(li.Value))
                                    li.Checked = true;
                            }
                        }
                    }

                    if (ddlUserType.SelectedValue == "1")
                    {
                        pnlWorker.Enabled = true;
                        chkMap.Enabled = false;
                        chkMap.Checked = true;
                        chkMap_CheckedChanged(sender, e);

                        int sup = 0;
                        objPropUser.ConnConfig = Session["config"].ToString();
                        objPropUser.Username = ds.Tables[0].Rows[0]["fuser"].ToString();

                        sup = objBL_User.getLoginSuper(objPropUser);
                        if (sup == 1)
                        {
                            ViewState["super"] = 1;
                            lblSuper.Enabled = false;
                            ddlSuper.Enabled = false;
                            pnlGrid.Visible = true;
                            chkSuper.Checked = true;
                            objPropUser.Username = ds.Tables[0].Rows[0]["fuser"].ToString();

                            int superv = objBL_User.getISSuper(objPropUser);
                            if (superv != 0)
                            {
                                chkSuper.Enabled = false;
                            }

                            else
                            {
                                chkSuper.Enabled = true;
                            }
                            // test(true);
                        }
                        else
                        {
                            ViewState["super"] = 0;
                            lblSuper.Enabled = true;
                            ddlSuper.Enabled = true;
                            //test(false);
                        }

                        ddlSuper.SelectedValue = ds.Tables[0].Rows[0]["super"].ToString().ToUpper();
                    }
                    string LocnRemark = ds.Tables[0].Rows[0]["Location"].ToString().Substring(3, 1);

                    string PurcOrders = ds.Tables[0].Rows[0]["PO"].ToString().Substring(0, 1);
                    string Exp = ds.Tables[0].Rows[0]["PO"].ToString().Substring(1, 1);
                    string ProgFunc = string.Empty;
                    string AccessUser = string.Empty;

                    string Sales = ds.Tables[0].Rows[0]["UserSales"].ToString().Substring(0, 1);
                    string EmpMaintenace = ds.Tables[0].Rows[0]["employeeMaint"].ToString().Substring(3, 1);
                    string TCTimeFix = ds.Tables[0].Rows[0]["TC"].ToString().Substring(1, 1);

                    string _chart = ds.Tables[0].Rows[0]["Chart"].ToString();
                    string _glAdj = ds.Tables[0].Rows[0]["GLAdj"].ToString();
                    string _financeState = ds.Tables[0].Rows[0]["Financial"].ToString().Substring(5, 1);

                    string _apVendor = ds.Tables[0].Rows[0]["Vendor"].ToString(); //check Account payable permission
                    string _apBill = ds.Tables[0].Rows[0]["Bill"].ToString();
                    string _apBillPay = ds.Tables[0].Rows[0]["BillPay"].ToString();

                    if (_chart.Equals("YYYYYY") && _glAdj.Equals("YYYYYY"))
                    {
                        chkFinanceMgr.Checked = true;
                    }
                    if (_financeState.Equals("Y"))
                    {
                        chkFinanceStatement.Checked = true;
                    }
                    else
                    {
                        chkFinanceStatement.Checked = false;
                    }

                    string MSAuthorisedDeviceOnly = ds.Tables[0].Rows[0]["MSAuthorisedDeviceOnly"] == DBNull.Value ? "0" : ds.Tables[0].Rows[0]["MSAuthorisedDeviceOnly"].ToString();

                    chkMSAuthorisedDeviceOnly.Checked = MSAuthorisedDeviceOnly == "1" ? true : false;
                    if (chkMSAuthorisedDeviceOnly.Checked)
                    {
                        txtAuthdevID.Enabled = true;
                    }
                    else
                    {
                        txtAuthdevID.Enabled = false;
                    }

                    //Equipment 

                    string EquipmentPermission = ds.Tables[0].Rows[0]["elevator"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["elevator"].ToString();

                    string Addequip = EquipmentPermission.Length < 1 ? "Y" : EquipmentPermission.Substring(0, 1);
                    string Editequip = EquipmentPermission.Length < 2 ? "Y" : EquipmentPermission.Substring(1, 1);
                    string Deleteequip = EquipmentPermission.Length < 3 ? "Y" : EquipmentPermission.Substring(2, 1);
                    string Viewquip = EquipmentPermission.Length < 4 ? "Y" : EquipmentPermission.Substring(3, 1);

                    chkEquipmentsadd.Checked = (Addequip == "N") ? false : true;
                    chkEquipmentsedit.Checked = (Editequip == "N") ? false : true;
                    chkEquipmentsdelete.Checked = (Deleteequip == "N") ? false : true;
                    chkEquipmentsview.Checked = (Viewquip == "N") ? false : true;

                    //Project
                    string ProjectPermission = ds.Tables[0].Rows[0]["Job"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["Job"].ToString();

                    string Addejob = ProjectPermission.Length < 1 ? "Y" : ProjectPermission.Substring(0, 1);
                    string Editejob = ProjectPermission.Length < 2 ? "Y" : ProjectPermission.Substring(1, 1);
                    string Deleteejob = ProjectPermission.Length < 3 ? "Y" : ProjectPermission.Substring(2, 1);
                    string Viewjob = ProjectPermission.Length < 4 ? "Y" : ProjectPermission.Substring(3, 1);

                    chkProjectadd.Checked = (Addejob == "N") ? false : true;
                    chkProjectEdit.Checked = (Editejob == "N") ? false : true;
                    chkProjectDelete.Checked = (Deleteejob == "N") ? false : true;
                    chkProjectView.Checked = (Viewjob == "N") ? false : true;

                    //Location
                    string LocationPermission = ds.Tables[0].Rows[0]["Location"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["Location"].ToString();

                    string AddeLocation = LocationPermission.Length < 1 ? "Y" : LocationPermission.Substring(0, 1);
                    string EditeLocation = LocationPermission.Length < 2 ? "Y" : LocationPermission.Substring(1, 1);
                    string DeleteeLocation = LocationPermission.Length < 3 ? "Y" : LocationPermission.Substring(2, 1);
                    string ViewLocation = LocationPermission.Length < 4 ? "Y" : LocationPermission.Substring(3, 1);

                    chkLocationadd.Checked = (AddeLocation == "N") ? false : true;
                    chkLocationedit.Checked = (EditeLocation == "N") ? false : true;
                    chkLocationdelete.Checked = (DeleteeLocation == "N") ? false : true;
                    chkLocationview.Checked = (ViewLocation == "N") ? false : true;

                    //Owner

                    string OwnerPermission = ds.Tables[0].Rows[0]["Owner"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["Owner"].ToString();

                    string AddeOwner = OwnerPermission.Length < 1 ? "Y" : OwnerPermission.Substring(0, 1);
                    string EditeOwner = OwnerPermission.Length < 2 ? "Y" : OwnerPermission.Substring(1, 1);
                    string DeleteeOwner = OwnerPermission.Length < 3 ? "Y" : OwnerPermission.Substring(2, 1);
                    string ViewOwner = OwnerPermission.Length < 4 ? "Y" : OwnerPermission.Substring(3, 1);

                    chkCustomeradd.Checked = (AddeOwner == "N") ? false : true;
                    chkCustomeredit.Checked = (EditeOwner == "N") ? false : true;
                    chkCustomerdelete.Checked = (DeleteeOwner == "N") ? false : true;
                    chkCustomerview.Checked = (ViewOwner == "N") ? false : true;

                    //FinanceDataPermissions
                    string ViewFinance = ds.Tables[0].Rows[0]["FinancePermission"] == DBNull.Value ? "Y" : ds.Tables[0].Rows[0]["FinancePermission"].ToString();
                    chkViewFinance.Checked = (ViewFinance == "N") ? false : true;

                    // ProjectListPermission

                    string ViewProjectListPermission = ds.Tables[0].Rows[0]["ProjectListPermission"] == DBNull.Value ? "Y" : ds.Tables[0].Rows[0]["ProjectListPermission"].ToString();

                    chkViewProjectList.Checked = (ViewProjectListPermission == "N") ? false : true;

                    //BOMPermissions
                    string BOMPermission = ds.Tables[0].Rows[0]["BOMPermission"] == DBNull.Value ? "NNNN" : ds.Tables[0].Rows[0]["BOMPermission"].ToString();

                    string AddBOM = BOMPermission.Length < 1 ? "Y" : BOMPermission.Substring(0, 1);
                    string EditBOM = BOMPermission.Length < 2 ? "Y" : BOMPermission.Substring(1, 1);
                    string DeleteBOM = BOMPermission.Length < 3 ? "Y" : BOMPermission.Substring(2, 1);
                    string ViewBOM = BOMPermission.Length < 4 ? "Y" : BOMPermission.Substring(3, 1);

                    chkAddBOM.Checked = (AddBOM == "N") ? false : true;
                    chkEditBOM.Checked = (EditBOM == "N") ? false : true;
                    chkDeleteBOM.Checked = (DeleteBOM == "N") ? false : true;
                    chkViewBOM.Checked = (ViewBOM == "N") ? false : true;

                    //WIPPermissions
                    string WIPPermissions = ds.Tables[0].Rows[0]["WIPPermission"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["WIPPermission"].ToString();

                    string AddWIP = WIPPermissions.Length < 1 ? "Y" : WIPPermissions.Substring(0, 1);
                    string EditWIP = WIPPermissions.Length < 2 ? "Y" : WIPPermissions.Substring(1, 1);
                    string DeleteWIP = WIPPermissions.Length < 3 ? "Y" : WIPPermissions.Substring(2, 1);
                    string ViewWIP = WIPPermissions.Length < 4 ? "Y" : WIPPermissions.Substring(3, 1);
                    string ReportWIP = WIPPermissions.Length < 6 ? "Y" : WIPPermissions.Substring(5, 1);

                    chkAddWIP.Checked = (AddWIP == "N") ? false : true;
                    chkEditWIP.Checked = (EditWIP == "N") ? false : true;
                    chkDeleteWIP.Checked = (DeleteWIP == "N") ? false : true;
                    chkViewWIP.Checked = (ViewWIP == "N") ? false : true;
                    chkReportWIP.Checked = (ReportWIP == "N") ? false : true;

                    //MilestonesPermission
                    string MilesStonesPermission = ds.Tables[0].Rows[0]["MilestonesPermission"] == DBNull.Value ? "NNNN" : ds.Tables[0].Rows[0]["MilestonesPermission"].ToString();

                    string AddMilesStones = MilesStonesPermission.Length < 1 ? "Y" : MilesStonesPermission.Substring(0, 1);
                    string EditMilesStones = MilesStonesPermission.Length < 2 ? "Y" : MilesStonesPermission.Substring(1, 1);
                    string DeleteMilesStones = MilesStonesPermission.Length < 3 ? "Y" : MilesStonesPermission.Substring(2, 1);
                    string ViewMilesStones = MilesStonesPermission.Length < 4 ? "Y" : MilesStonesPermission.Substring(3, 1);

                    chkAddMilesStones.Checked = (AddMilesStones == "N") ? false : true;
                    chkEditMilesStones.Checked = (EditMilesStones == "N") ? false : true;
                    chkDeleteMilesStones.Checked = (DeleteMilesStones == "N") ? false : true;
                    chkViewMilesStones.Checked = (ViewMilesStones == "N") ? false : true;

                    //Inventory Item

                    string InventoryItemPermission = ds.Tables[0].Rows[0]["Item"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["Item"].ToString();

                    string AddInventoryItem = InventoryItemPermission.Length < 1 ? "Y" : InventoryItemPermission.Substring(0, 1);
                    string EditInventoryItem = InventoryItemPermission.Length < 2 ? "Y" : InventoryItemPermission.Substring(1, 1);
                    string DeleteInventoryItem = InventoryItemPermission.Length < 3 ? "Y" : InventoryItemPermission.Substring(2, 1);
                    string ViewInventoryItem = InventoryItemPermission.Length < 4 ? "Y" : InventoryItemPermission.Substring(3, 1);

                    chkInventoryItemadd.Checked = (AddInventoryItem == "N") ? false : true;
                    chkInventoryItemedit.Checked = (EditInventoryItem == "N") ? false : true;
                    chkInventoryItemdelete.Checked = (DeleteInventoryItem == "N") ? false : true;
                    chkInventoryItemview.Checked = (ViewInventoryItem == "N") ? false : true;

                    //Inventory AdjustMent

                    string InventoryAdjustmentPermission = ds.Tables[0].Rows[0]["InvAdj"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["InvAdj"].ToString();

                    string AddInventoryAdjustMent = InventoryAdjustmentPermission.Length < 1 ? "Y" : InventoryAdjustmentPermission.Substring(0, 1);
                    string EditInventoryAdjustMent = InventoryAdjustmentPermission.Length < 2 ? "Y" : InventoryAdjustmentPermission.Substring(1, 1);
                    string DeleteInventoryAdjustMent = InventoryAdjustmentPermission.Length < 3 ? "Y" : InventoryAdjustmentPermission.Substring(2, 1);
                    string ViewInventoryAdjustMent = InventoryAdjustmentPermission.Length < 4 ? "Y" : InventoryAdjustmentPermission.Substring(3, 1);

                    chkInventoryAdjustmentadd.Checked = (AddInventoryAdjustMent == "N") ? false : true;
                    chkInventoryAdjustmentedit.Checked = (EditInventoryAdjustMent == "N") ? false : true;
                    chkInventoryAdjustmentdelete.Checked = (DeleteInventoryAdjustMent == "N") ? false : true;
                    chkInventoryAdjustmentview.Checked = (ViewInventoryAdjustMent == "N") ? false : true;

                    //Inventory WareHouse

                    string InventoryWareHousePermission = ds.Tables[0].Rows[0]["Warehouse"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["Warehouse"].ToString();

                    string AddInventoryWareHouse = InventoryWareHousePermission.Length < 1 ? "Y" : InventoryWareHousePermission.Substring(0, 1);
                    string EditInventoryWareHouse = InventoryWareHousePermission.Length < 2 ? "Y" : InventoryWareHousePermission.Substring(1, 1);
                    string DeleteInventoryWareHouse = InventoryWareHousePermission.Length < 3 ? "Y" : InventoryWareHousePermission.Substring(2, 1);
                    string ViewInventoryWareHouse = InventoryWareHousePermission.Length < 4 ? "Y" : InventoryWareHousePermission.Substring(3, 1);

                    chkInventoryWareHouseadd.Checked = (AddInventoryWareHouse == "N") ? false : true;
                    chkInventoryWareHouseedit.Checked = (EditInventoryWareHouse == "N") ? false : true;
                    chkInventoryWareHousedelete.Checked = (DeleteInventoryWareHouse == "N") ? false : true;
                    chkInventoryWareHouseview.Checked = (ViewInventoryWareHouse == "N") ? false : true;

                    //Inventory Setup

                    string InventorySetupPermission = ds.Tables[0].Rows[0]["InvSetup"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["InvSetup"].ToString();

                    string AddInventorySetup = InventorySetupPermission.Length < 1 ? "Y" : InventorySetupPermission.Substring(0, 1);
                    string EditInventorySetup = InventorySetupPermission.Length < 2 ? "Y" : InventorySetupPermission.Substring(1, 1);
                    string DeleteInventorySetup = InventorySetupPermission.Length < 3 ? "Y" : InventorySetupPermission.Substring(2, 1);
                    string ViewInventorySetup = InventorySetupPermission.Length < 4 ? "Y" : InventorySetupPermission.Substring(3, 1);

                    chkInventorysetupadd.Checked = (AddInventorySetup == "N") ? false : true;
                    chkInventorysetupedit.Checked = (EditInventorySetup == "N") ? false : true;
                    chkInventorysetupdelete.Checked = (DeleteInventorySetup == "N") ? false : true;
                    chkInventorysetupview.Checked = (ViewInventorySetup == "N") ? false : true;

                    //DocumentPermission
                    string DocumentPermission = ds.Tables[0].Rows[0]["DocumentPermission"] == DBNull.Value ? "NNNN" : ds.Tables[0].Rows[0]["DocumentPermission"].ToString();

                    string AddDocumentPermission = DocumentPermission.Length < 1 ? "Y" : DocumentPermission.Substring(0, 1);
                    string EditDocumentPermission = DocumentPermission.Length < 2 ? "Y" : DocumentPermission.Substring(1, 1);
                    string DeleteDocumentPermission = DocumentPermission.Length < 3 ? "Y" : DocumentPermission.Substring(2, 1);
                    string ViewDocumentPermission = DocumentPermission.Length < 4 ? "Y" : DocumentPermission.Substring(3, 1);

                    chkDocumentAdd.Checked = (AddDocumentPermission == "N") ? false : true;
                    chkDocumentEdit.Checked = (EditDocumentPermission == "N") ? false : true;
                    chkDocumentDelete.Checked = (DeleteDocumentPermission == "N") ? false : true;
                    chkDocumentView.Checked = (ViewDocumentPermission == "N") ? false : true;

                    //ContactPermission
                    string ContactPermission = ds.Tables[0].Rows[0]["ContactPermission"] == DBNull.Value ? "NNNN" : ds.Tables[0].Rows[0]["ContactPermission"].ToString();

                    string AddContactPermission = ContactPermission.Length < 1 ? "Y" : ContactPermission.Substring(0, 1);
                    string EditContactPermission = ContactPermission.Length < 2 ? "Y" : ContactPermission.Substring(1, 1);
                    string DeleteContactPermission = ContactPermission.Length < 3 ? "Y" : ContactPermission.Substring(2, 1);
                    string ViewContactPermission = ContactPermission.Length < 4 ? "Y" : ContactPermission.Substring(3, 1);

                    chkContactAdd.Checked = (AddContactPermission == "N") ? false : true;
                    chkContactEdit.Checked = (EditContactPermission == "N") ? false : true;
                    chkContactDelete.Checked = (DeleteContactPermission == "N") ? false : true;
                    chkContactView.Checked = (ViewContactPermission == "N") ? false : true;

                    //VendorsPermission
                    string VendorsPermission = ds.Tables[0].Rows[0]["Vendor"] == DBNull.Value ? "NNNN" : ds.Tables[0].Rows[0]["Vendor"].ToString();

                    string AddVendorsPermission = VendorsPermission.Length < 1 ? "Y" : VendorsPermission.Substring(0, 1);
                    string EditVendorsPermission = VendorsPermission.Length < 2 ? "Y" : VendorsPermission.Substring(1, 1);
                    string DeleteVendorsPermission = VendorsPermission.Length < 3 ? "Y" : VendorsPermission.Substring(2, 1);
                    string ViewVendorsPermission = VendorsPermission.Length < 4 ? "Y" : VendorsPermission.Substring(3, 1);

                    chkVendorsAdd.Checked = (AddVendorsPermission == "N") ? false : true;
                    chkVendorsEdit.Checked = (EditVendorsPermission == "N") ? false : true;
                    chkVendorsDelete.Checked = (DeleteVendorsPermission == "N") ? false : true;
                    chkVendorsView.Checked = (ViewVendorsPermission == "N") ? false : true;

                    //Inventory Finance

                    string InventoryFinancePermission = ds.Tables[0].Rows[0]["InvViewer"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["InvViewer"].ToString();

                    string AddInventoryFinance = InventoryFinancePermission.Length < 1 ? "Y" : InventoryFinancePermission.Substring(0, 1);
                    string EditInventoryFinance = InventoryFinancePermission.Length < 2 ? "Y" : InventoryFinancePermission.Substring(1, 1);
                    string DeleteInventoryFinance = InventoryFinancePermission.Length < 3 ? "Y" : InventoryFinancePermission.Substring(2, 1);
                    string ViewInventoryFinance = InventoryFinancePermission.Length < 4 ? "Y" : InventoryFinancePermission.Substring(3, 1);

                    chkInventoryFinanceAdd.Checked = (AddInventoryFinance == "N") ? false : true;
                    chkInventoryFinanceedit.Checked = (EditInventoryFinance == "N") ? false : true;
                    chkInventoryFinancedelete.Checked = (DeleteInventoryFinance == "N") ? false : true;
                    chkInventoryFinanceview.Checked = (ViewInventoryFinance == "N") ? false : true;

                    // Project templates

                    string ProjecttempPermission = ds.Tables[0].Rows[0]["ProjecttempPermission"] == DBNull.Value ? "NNNN" : ds.Tables[0].Rows[0]["ProjecttempPermission"].ToString();

                    string AddProjecttempPermission = ProjecttempPermission.Length < 1 ? "Y" : ProjecttempPermission.Substring(0, 1);
                    string EditProjecttempPermission = ProjecttempPermission.Length < 2 ? "Y" : ProjecttempPermission.Substring(1, 1);
                    string DeleteProjecttempPermission = ProjecttempPermission.Length < 3 ? "Y" : ProjecttempPermission.Substring(2, 1);
                    string ViewProjecttempPermission = ProjecttempPermission.Length < 4 ? "Y" : ProjecttempPermission.Substring(3, 1);

                    chkProjectTempAdd.Checked = (AddProjecttempPermission == "N") ? false : true;
                    chkProjectTempEdit.Checked = (EditProjecttempPermission == "N") ? false : true;
                    chkProjectTempDelete.Checked = (DeleteProjecttempPermission == "N") ? false : true;
                    chkProjectTempView.Checked = (ViewProjecttempPermission == "N") ? false : true;

                    //BillingCodesPermission
                    string BillingCodesPermission = ds.Tables[0].Rows[0]["BillingCodesPermission"] == DBNull.Value ? "NNNN" : ds.Tables[0].Rows[0]["BillingCodesPermission"].ToString();

                    string AddBillingCodesPermission = BillingCodesPermission.Length < 1 ? "Y" : BillingCodesPermission.Substring(0, 1);
                    string EditBillingCodesPermission = BillingCodesPermission.Length < 2 ? "Y" : BillingCodesPermission.Substring(1, 1);
                    string DeleteBillingCodesPermission = BillingCodesPermission.Length < 3 ? "Y" : BillingCodesPermission.Substring(2, 1);
                    string ViewBillingCodesPermission = BillingCodesPermission.Length < 4 ? "Y" : BillingCodesPermission.Substring(3, 1);

                    chkBillingcodesAdd.Checked = (AddBillingCodesPermission == "N") ? false : true;
                    chkBillingcodesEdit.Checked = (EditBillingCodesPermission == "N") ? false : true;
                    chkBillingcodesDelete.Checked = (DeleteBillingCodesPermission == "N") ? false : true;
                    chkBillingcodesView.Checked = (ViewBillingCodesPermission == "N") ? false : true;

                    // Invoice Permission
                    string InvoicePermission = ds.Tables[0].Rows[0]["Invoice"] == DBNull.Value ? "NNNN" : ds.Tables[0].Rows[0]["Invoice"].ToString();

                    string AddInvoicePermission = InvoicePermission.Length < 1 ? "Y" : InvoicePermission.Substring(0, 1);
                    string EditInvoicePermission = InvoicePermission.Length < 2 ? "Y" : InvoicePermission.Substring(1, 1);
                    string DeleteInvoicePermission = InvoicePermission.Length < 3 ? "Y" : InvoicePermission.Substring(2, 1);
                    string ViewInvoicePermission = InvoicePermission.Length < 4 ? "Y" : InvoicePermission.Substring(3, 1);

                    chkInvoicesAdd.Checked = (AddInvoicePermission == "N") ? false : true;
                    chkInvoicesEdit.Checked = (EditInvoicePermission == "N") ? false : true;
                    chkInvoicesDelete.Checked = (DeleteInvoicePermission == "N") ? false : true;
                    chkInvoicesView.Checked = (ViewInvoicePermission == "N") ? false : true;

                    //POPermission
                    string POPermission = ds.Tables[0].Rows[0]["PO"] == DBNull.Value ? "NNNN" : ds.Tables[0].Rows[0]["PO"].ToString();

                    string AddPOPermission = POPermission.Length < 1 ? "Y" : POPermission.Substring(0, 1);
                    string EditPOPermission = POPermission.Length < 2 ? "Y" : POPermission.Substring(1, 1);
                    string DeletePOPermission = POPermission.Length < 3 ? "Y" : POPermission.Substring(2, 1);
                    string ViewPOPermission = POPermission.Length < 4 ? "Y" : POPermission.Substring(3, 1);

                    chkPOAdd.Checked = (AddPOPermission == "N") ? false : true;
                    chkPOEdit.Checked = (EditPOPermission == "N") ? false : true;
                    chkPODelete.Checked = (DeletePOPermission == "N") ? false : true;
                    chkPOView.Checked = (ViewPOPermission == "N") ? false : true;

                    //RPOPermission
                    string RPOPermission = ds.Tables[0].Rows[0]["RPO"] == DBNull.Value ? "NNNN" : ds.Tables[0].Rows[0]["RPO"].ToString();

                    string AddRPOPermission = RPOPermission.Length < 1 ? "Y" : RPOPermission.Substring(0, 1);
                    string EditRPOPermission = RPOPermission.Length < 2 ? "Y" : RPOPermission.Substring(1, 1);
                    string DeleteRPOPermission = RPOPermission.Length < 3 ? "Y" : RPOPermission.Substring(2, 1);
                    string ViewRPOPermission = RPOPermission.Length < 4 ? "Y" : RPOPermission.Substring(3, 1);

                    chkRPOAdd.Checked = (AddRPOPermission == "N") ? false : true;
                    chkRPOEdit.Checked = (EditRPOPermission == "N") ? false : true;
                    chkRPODelete.Checked = (DeleteRPOPermission == "N") ? false : true;
                    chkRPOView.Checked = (ViewRPOPermission == "N") ? false : true;

                    //PurchasingmodulePermission
                    string PurchasingmodulePermission = ds.Tables[0].Rows[0]["PurchasingmodulePermission"] == DBNull.Value ? "Y" : ds.Tables[0].Rows[0]["PurchasingmodulePermission"].ToString();
                    chkPurchasingmodule.Checked = (PurchasingmodulePermission == "N") ? false : true;

                    //PONotification
                    string PONotification = ds.Tables[0].Rows[0]["PONotification"] == DBNull.Value ? "N" : ds.Tables[0].Rows[0]["PONotification"].ToString();
                    chkPONotification.Checked = (PONotification == "N") ? false : true;

                    //BillingmodulePermission
                    string BillingmodulePermission = ds.Tables[0].Rows[0]["BillingmodulePermission"] == DBNull.Value ? "Y" : ds.Tables[0].Rows[0]["BillingmodulePermission"].ToString();

                    chkBillingmodule.Checked = (BillingmodulePermission == "N") ? false : true;

                    //BillPermission
                    string BillPermission = ds.Tables[0].Rows[0]["Bill"] == DBNull.Value ? "NNNN" : ds.Tables[0].Rows[0]["Bill"].ToString();

                    string AddBillPermission = BillPermission.Length < 1 ? "Y" : BillPermission.Substring(0, 1);
                    string EditBillPermission = BillPermission.Length < 2 ? "Y" : BillPermission.Substring(1, 1);
                    string DeleteBillPermission = BillPermission.Length < 3 ? "Y" : BillPermission.Substring(2, 1);
                    string ViewBillPermission = BillPermission.Length < 4 ? "Y" : BillPermission.Substring(3, 1);

                    chkAddBills.Checked = (AddBillPermission == "N") ? false : true;
                    chkEditBills.Checked = (EditBillPermission == "N") ? false : true;
                    chkDeleteBills.Checked = (DeleteBillPermission == "N") ? false : true;
                    chkViewBills.Checked = (ViewBillPermission == "N") ? false : true;

                    //BillPayPermission
                    string BillPayPermission = ds.Tables[0].Rows[0]["BillPay"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["BillPay"].ToString();

                    string AddBillPayPermission = BillPayPermission.Length < 1 ? "Y" : BillPayPermission.Substring(0, 1);
                    string EditBillPayPermission = BillPayPermission.Length < 2 ? "Y" : BillPayPermission.Substring(1, 1);
                    string DeleteBillPayPermission = BillPayPermission.Length < 3 ? "Y" : BillPayPermission.Substring(2, 1);
                    string ViewBillPayPermission = BillPayPermission.Length < 4 ? "Y" : BillPayPermission.Substring(3, 1);
                    string ShowBankBalancesPermission = BillPayPermission.Length < 5 ? "Y" : BillPayPermission.Substring(4, 1);

                    chkAddManageChecks.Checked = (AddBillPayPermission == "N") ? false : true;
                    chkEditManageChecks.Checked = (EditBillPayPermission == "N") ? false : true;
                    chkDeleteManageChecks.Checked = (DeleteBillPayPermission == "N") ? false : true;
                    chkViewManageChecks.Checked = (ViewBillPayPermission == "N") ? false : true;
                    chkShowBankBalances.Checked = (ShowBankBalancesPermission == "N") ? false : true;

                    //VendorPermission
                    string VendorPermission = ds.Tables[0].Rows[0]["Vendor"] == DBNull.Value ? "NNNN" : ds.Tables[0].Rows[0]["Vendor"].ToString();

                    string AddVendorPermission = VendorPermission.Length < 1 ? "Y" : VendorPermission.Substring(0, 1);
                    string EditVendorPermission = VendorPermission.Length < 2 ? "Y" : VendorPermission.Substring(1, 1);
                    string DeleteVendorPermission = VendorPermission.Length < 3 ? "Y" : VendorPermission.Substring(2, 1);
                    string ViewVendorPermission = VendorPermission.Length < 4 ? "Y" : VendorPermission.Substring(3, 1);

                    chkVendorsAdd.Checked = (AddVendorPermission == "N") ? false : true;
                    chkVendorsEdit.Checked = (EditVendorPermission == "N") ? false : true;
                    chkVendorsDelete.Checked = (DeleteVendorPermission == "N") ? false : true;
                    chkVendorsView.Checked = (ViewVendorPermission == "N") ? false : true;

                    //AccountPayablemodulePermission
                    string AccountPayablemodulePermission = ds.Tables[0].Rows[0]["AccountPayablemodulePermission"] == DBNull.Value ? "Y" : ds.Tables[0].Rows[0]["AccountPayablemodulePermission"].ToString();

                    chkAccountPayable.Checked = (AccountPayablemodulePermission == "N") ? false : true;

                    //PaymentHistoryPermission
                    string PaymentHistoryPermission = ds.Tables[0].Rows[0]["PaymentHistoryPermission"] == DBNull.Value ? "NNNN" : ds.Tables[0].Rows[0]["PaymentHistoryPermission"].ToString();

                    string ViewPaymentHistoryPermission = PaymentHistoryPermission.Length < 4 ? "Y" : PaymentHistoryPermission.Substring(3, 1);

                    chkPaymentHistoryView.Checked = (ViewPaymentHistoryPermission == "N") ? false : true;

                    //CustomermodulePermission
                    string CustomermodulePermission = ds.Tables[0].Rows[0]["AccountPayablemodulePermission"] == DBNull.Value ? "Y" : ds.Tables[0].Rows[0]["CustomermodulePermission"].ToString();

                    chkCustomerModule.Checked = (CustomermodulePermission == "N") ? false : true;

                    //CreditHoldPermissions
                    string CreditHoldPermissions = ds.Tables[0].Rows[0]["CreditHold"] == DBNull.Value ? "Y" : ds.Tables[0].Rows[0]["CreditHold"].ToString();
                    string ViewCreditHoldPermissions = CreditHoldPermissions.Length < 4 ? "Y" : CreditHoldPermissions.Substring(3, 1);
                    chkCreditHold.Checked = (ViewCreditHoldPermissions == "N") ? false : true;

                    //CreditFlagPermissions
                    string CreditFlagPermissions = ds.Tables[0].Rows[0]["CreditFlag"] == DBNull.Value ? "Y" : ds.Tables[0].Rows[0]["CreditFlag"].ToString();
                    string ViewCreditFlagPermissions = CreditFlagPermissions.Length < 4 ? "Y" : CreditFlagPermissions.Substring(3, 1);
                    chkCreditFlag.Checked = (ViewCreditFlagPermissions == "N") ? false : true;

                    //WriteOffPermissions
                    string WriteOffPermissions = ds.Tables[0].Rows[0]["WriteOff"] == DBNull.Value ? "N" : ds.Tables[0].Rows[0]["WriteOff"].ToString().Substring(0, 1);
                    chkWriteOff.Checked = (WriteOffPermissions == "N") ? false : true;

                    //FinancialmodulePermission
                    string FinancialmodulePermission = ds.Tables[0].Rows[0]["FinancialmodulePermission"] == DBNull.Value ? "Y" : ds.Tables[0].Rows[0]["FinancialmodulePermission"].ToString();

                    chkFinancialmodule.Checked = (FinancialmodulePermission == "N") ? false : true;

                    //ChartPermission
                    string ChartPermission = ds.Tables[0].Rows[0]["Chart"] == DBNull.Value ? "NNNN" : ds.Tables[0].Rows[0]["Chart"].ToString();

                    string AddChartPermission = ChartPermission.Length < 1 ? "Y" : ChartPermission.Substring(0, 1);
                    string EditChartPermission = ChartPermission.Length < 2 ? "Y" : ChartPermission.Substring(1, 1);
                    string DeleteChartPermission = ChartPermission.Length < 3 ? "Y" : ChartPermission.Substring(2, 1);
                    string ViewChartPermission = ChartPermission.Length < 4 ? "Y" : ChartPermission.Substring(3, 1);

                    chkChartAdd.Checked = (AddChartPermission == "N") ? false : true;
                    chkChartEdit.Checked = (EditChartPermission == "N") ? false : true;
                    chkChartDelete.Checked = (DeleteChartPermission == "N") ? false : true;
                    chkChartView.Checked = (ViewChartPermission == "N") ? false : true;

                    //JournalEntryPermission
                    string JournalEntryPermission = ds.Tables[0].Rows[0]["GLAdj"] == DBNull.Value ? "NNNN" : ds.Tables[0].Rows[0]["GLAdj"].ToString();

                    string AddJournalEntryPermission = JournalEntryPermission.Length < 1 ? "Y" : JournalEntryPermission.Substring(0, 1);
                    string EditJournalEntryPermission = JournalEntryPermission.Length < 2 ? "Y" : JournalEntryPermission.Substring(1, 1);
                    string DeleteJournalEntryPermission = JournalEntryPermission.Length < 3 ? "Y" : JournalEntryPermission.Substring(2, 1);
                    string ViewJournalEntryPermission = JournalEntryPermission.Length < 4 ? "Y" : JournalEntryPermission.Substring(3, 1);

                    chkJournalEntryAdd.Checked = (AddJournalEntryPermission == "N") ? false : true;
                    chkJournalEntryEdit.Checked = (EditJournalEntryPermission == "N") ? false : true;
                    chkJournalEntryDelete.Checked = (DeleteJournalEntryPermission == "N") ? false : true;
                    chkJournalEntryView.Checked = (ViewJournalEntryPermission == "N") ? false : true;

                    //BankrecPermission
                    string BankrecPermission = ds.Tables[0].Rows[0]["bankrec"] == DBNull.Value ? "NNNN" : ds.Tables[0].Rows[0]["bankrec"].ToString();

                    string AddBankrecPermission = BankrecPermission.Length < 1 ? "Y" : BankrecPermission.Substring(0, 1);
                    string EditBankrecPermission = BankrecPermission.Length < 2 ? "Y" : BankrecPermission.Substring(1, 1);
                    string DeleteBankrecPermission = BankrecPermission.Length < 3 ? "Y" : BankrecPermission.Substring(2, 1);
                    string ViewBankrecPermission = BankrecPermission.Length < 4 ? "Y" : BankrecPermission.Substring(3, 1);

                    chkBankAdd.Checked = (AddBankrecPermission == "N") ? false : true;
                    chkBankEdit.Checked = (EditBankrecPermission == "N") ? false : true;
                    chkBankDelete.Checked = (DeleteBankrecPermission == "N") ? false : true;
                    chkBankView.Checked = (ViewBankrecPermission == "N") ? false : true;

                    //RCmodulePermission
                    string RCmodulePermission = ds.Tables[0].Rows[0]["RCmodulePermission"] == DBNull.Value ? "Y" : ds.Tables[0].Rows[0]["RCmodulePermission"].ToString();

                    chkRecurring.Checked = (RCmodulePermission == "N") ? false : true;

                    //ProcessRCPermission
                    string ProcessRCPermission = ds.Tables[0].Rows[0]["ProcessRCPermission"] == DBNull.Value ? "NNNN" : ds.Tables[0].Rows[0]["ProcessRCPermission"].ToString();

                    string AddProcessRCPermissionn = ProcessRCPermission.Length < 1 ? "Y" : ProcessRCPermission.Substring(0, 1);
                    string EditProcessRCPermission = ProcessRCPermission.Length < 2 ? "Y" : ProcessRCPermission.Substring(1, 1);
                    string DeleteProcessRCPermission = ProcessRCPermission.Length < 3 ? "Y" : ProcessRCPermission.Substring(2, 1);
                    string ViewProcessRCPermission = ProcessRCPermission.Length < 4 ? "Y" : ProcessRCPermission.Substring(3, 1);

                    chkRecContractsAdd.Checked = (AddProcessRCPermissionn == "N") ? false : true;
                    chkRecContractsEdit.Checked = (EditProcessRCPermission == "N") ? false : true;
                    chkRecContractsDelete.Checked = (DeleteProcessRCPermission == "N") ? false : true;
                    chkRecContractsView.Checked = (ViewProcessRCPermission == "N") ? false : true;

                    //RCRenewEscalatePermission
                    string RCRenewEscalatePermission = ds.Tables[0].Rows[0]["RCRenewEscalatePermission"] == DBNull.Value ? "NNNN" : ds.Tables[0].Rows[0]["RCRenewEscalatePermission"].ToString();

                    string AddRCRenewEscalatePermission = RCRenewEscalatePermission.Length < 1 ? "Y" : RCRenewEscalatePermission.Substring(0, 1);
                    string ViewRCRenewEscalatePermission = RCRenewEscalatePermission.Length < 4 ? "Y" : RCRenewEscalatePermission.Substring(3, 1);

                    chkRenewEscalateAdd.Checked = (AddRCRenewEscalatePermission == "N") ? false : true;
                    chkRenewEscalateView.Checked = (ViewRCRenewEscalatePermission == "N") ? false : true;

                    //ProcessCPermission
                    string ProcessCPermission = ds.Tables[0].Rows[0]["ProcessC"] == DBNull.Value ? "NNNN" : ds.Tables[0].Rows[0]["ProcessC"].ToString();

                    string AddProcessCPermission = ProcessCPermission.Length < 1 ? "Y" : ProcessCPermission.Substring(0, 1);
                    string EditProcessCPermission = ProcessCPermission.Length < 2 ? "Y" : ProcessCPermission.Substring(1, 1);
                    string DeleteProcessCPermission = ProcessCPermission.Length < 3 ? "Y" : ProcessCPermission.Substring(2, 1);
                    string ViewProcessCPermission = ProcessCPermission.Length < 4 ? "Y" : ProcessCPermission.Substring(3, 1);

                    chkRecInvoicesAdd.Checked = (AddProcessCPermission == "N") ? false : true;
                    //chkRecInvoicesEdit.Checked = (EditProcessCPermission == "N") ? false : true;
                    chkRecInvoicesDelete.Checked = (DeleteProcessCPermission == "N") ? false : true;
                    chkRecInvoicesView.Checked = (ViewProcessCPermission == "N") ? false : true;

                    // Recurring TicketPermission
                    string ProcessTPermission = ds.Tables[0].Rows[0]["ProcessT"] == DBNull.Value ? "NNNN" : ds.Tables[0].Rows[0]["ProcessT"].ToString();

                    string AddProcessTPermission = ProcessTPermission.Length < 1 ? "Y" : ProcessTPermission.Substring(0, 1);
                    string EditProcessTPermission = ProcessTPermission.Length < 2 ? "Y" : ProcessTPermission.Substring(1, 1);
                    string DeleteProcessTPermission = ProcessTPermission.Length < 3 ? "Y" : ProcessTPermission.Substring(2, 1);
                    string ViewProcessTPermission = ProcessTPermission.Length < 4 ? "Y" : ProcessTPermission.Substring(3, 1);

                    chkRecTicketsAdd.Checked = (AddProcessTPermission == "N") ? false : true;
                    //chkRecTicketsEdit.Checked = (EditProcessTPermission == "N") ? false : true;
                    chkRecTicketsDelete.Checked = (DeleteProcessTPermission == "N") ? false : true;
                    chkRecTicketsView.Checked = (ViewProcessTPermission == "N") ? false : true;

                    //SafetyTestPermission
                    string SafetyTestPermission = ds.Tables[0].Rows[0]["SafetyTestsPermission"] == DBNull.Value ? "NNNN" : ds.Tables[0].Rows[0]["SafetyTestsPermission"].ToString();

                    string AddSafetyTestPermission = SafetyTestPermission.Length < 1 ? "Y" : SafetyTestPermission.Substring(0, 1);
                    string EditSafetyTestPermission = SafetyTestPermission.Length < 2 ? "Y" : SafetyTestPermission.Substring(1, 1);
                    string DeleteSafetyTestPermission = SafetyTestPermission.Length < 3 ? "Y" : SafetyTestPermission.Substring(2, 1);
                    string ViewSafetyTestPermission = SafetyTestPermission.Length < 4 ? "Y" : SafetyTestPermission.Substring(3, 1);

                    chkSafetyTestsAdd.Checked = (AddSafetyTestPermission == "N") ? false : true;
                    chkSafetyTestsEdit.Checked = (EditSafetyTestPermission == "N") ? false : true;
                    chkSafetyTestsDelete.Checked = (DeleteSafetyTestPermission == "N") ? false : true;
                    chkSafetyTestsView.Checked = (ViewSafetyTestPermission == "N") ? false : true;

                    //ViolationsPermission
                    string ViolationsPermission = ds.Tables[0].Rows[0]["ViolationPermission"] == DBNull.Value ? "NNNNN" : ds.Tables[0].Rows[0]["ViolationPermission"].ToString();

                    string AddViolationsPermission = ViolationsPermission.Length < 1 ? "Y" : ViolationsPermission.Substring(0, 1);
                    string DeleteViolationsPermission = ViolationsPermission.Length < 3 ? "Y" : ViolationsPermission.Substring(2, 1);

                    chkViolationsAdd.Checked = (AddViolationsPermission == "N") ? false : true;
                    chkViolationsDelete.Checked = (DeleteViolationsPermission == "N") ? false : true;

                    //ScheduleModulePermission
                    string SchedulemodulePermission = ds.Tables[0].Rows[0]["SchedulemodulePermission"] == DBNull.Value ? "Y" : ds.Tables[0].Rows[0]["SchedulemodulePermission"].ToString();

                    chkSchedule.Checked = (SchedulemodulePermission == "N") ? false : true;

                    //ProjectModulePermission
                    string ProjectModulePermission = ds.Tables[0].Rows[0]["ProjectModulePermission"] == DBNull.Value ? "Y" : ds.Tables[0].Rows[0]["ProjectModulePermission"].ToString();

                    chkProjectmodule.Checked = (ProjectModulePermission == "N") ? false : true;

                    //InventoryModulePermission
                    string InventoryModulePermission = ds.Tables[0].Rows[0]["InventoryModulePermission"] == DBNull.Value ? "Y" : ds.Tables[0].Rows[0]["InventoryModulePermission"].ToString();

                    chkInventorymodule.Checked = (InventoryModulePermission == "N") ? false : true;

                    //JobClosePermission
                    string JobClosePermission = ds.Tables[0].Rows[0]["JobClosePermission"] == DBNull.Value ? "Y" : ds.Tables[0].Rows[0]["JobClosePermission"].ToString();
                    if (JobClosePermission.Contains("Y")) chkJobClosePermission.Checked = true; else chkJobClosePermission.Checked = false;

                    //JobCompletedPermission
                    string JobCompletedPermission = ds.Tables[0].Rows[0]["JobCompletedPermission"] == DBNull.Value ? "Y" : ds.Tables[0].Rows[0]["JobCompletedPermission"].ToString();
                    if (JobCompletedPermission.Contains("Y")) chkJobCompletedPermission.Checked = true; else chkJobCompletedPermission.Checked = false;

                    //JobReopenPermission
                    string JobReopenPermission = ds.Tables[0].Rows[0]["JobReopenPermission"] == DBNull.Value ? "Y" : ds.Tables[0].Rows[0]["JobReopenPermission"].ToString();
                    if (JobReopenPermission.Contains("Y")) chkJobReopenPermission.Checked = true; else chkJobReopenPermission.Checked = false;

                    //SchedulePermission
                    string SchedulePermission = ds.Tables[0].Rows[0]["Ticket"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["Ticket"].ToString();

                    string ViewSchedulePermission = SchedulePermission.Length < 4 ? "Y" : SchedulePermission.Substring(3, 1);
                    string TicketReportPer = SchedulePermission.Length < 6 ? "Y" : SchedulePermission.Substring(5, 1);

                    chkScheduleBoard.Checked = (ViewSchedulePermission == "N") ? false : true;

                    //Dispatch
                    string TicketPermission = ds.Tables[0].Rows[0]["Dispatch"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["Dispatch"].ToString();

                    string AddTicketePermission = TicketPermission.Length < 1 ? "Y" : TicketPermission.Substring(0, 1);
                    string EditTicketePermission = TicketPermission.Length < 2 ? "Y" : TicketPermission.Substring(1, 1);
                    string DeleteTicketePermission = TicketPermission.Length < 3 ? "Y" : TicketPermission.Substring(2, 1);
                    string ViewTicketePermission = TicketPermission.Length < 4 ? "Y" : TicketPermission.Substring(3, 1);
                    //string ReportTicketePermission = TicketPermission.Length < 5 ? "Y" : TicketPermission.Substring(4, 1);
                    //string EmailDispatchPermission = TicketPermission.Length < 6 ? "Y" : TicketPermission.Substring(5, 1);

                    /** Thomas: I am changing this to make it the same other fields (Leads, Opportunities, Estimates)
                    1: Add
                    2: Edit
                    3: Delete
                    4: View
                    5: Dispatch (this case)
                    6: Report
                    */
                    string EmailDispatchPermission = TicketPermission.Length < 5 ? "Y" : TicketPermission.Substring(4, 1);
                    string ReportTicketePermission = TicketPermission.Length < 6 ? "Y" : TicketPermission.Substring(5, 1);

                    chkTicketListAdd.Checked = (AddTicketePermission == "N") ? false : true;
                    chkTicketListEdit.Checked = (EditTicketePermission == "N") ? false : true;
                    chkTicketListDelete.Checked = (DeleteTicketePermission == "N") ? false : true;
                    chkTicketListView.Checked = (ViewTicketePermission == "N") ? false : true;
                    chkTicketListReport.Checked = (ReportTicketePermission == "N") ? false : true;
                    chkDispatch.Checked = (EmailDispatchPermission == "N") ? false : true;

                    //Manual Timesheet Permission
                    string MTimesheetPermission = ds.Tables[0].Rows[0]["MTimesheet"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["MTimesheet"].ToString();

                    string Timesheetadd = MTimesheetPermission.Length < 1 ? "Y" : MTimesheetPermission.Substring(0, 1);
                    string Timesheetedit = MTimesheetPermission.Length < 2 ? "Y" : MTimesheetPermission.Substring(1, 1);
                    string Timesheetdelete = MTimesheetPermission.Length < 3 ? "Y" : MTimesheetPermission.Substring(2, 1);
                    string Timesheetview = MTimesheetPermission.Length < 4 ? "Y" : MTimesheetPermission.Substring(3, 1);
                    string Timesheetreport = MTimesheetPermission.Length < 6 ? "Y" : MTimesheetPermission.Substring(5, 1);

                    chkTimesheetadd.Checked = (Timesheetadd == "N") ? false : true;
                    chkTimesheetedit.Checked = (Timesheetedit == "N") ? false : true;
                    chkTimesheetdelete.Checked = (Timesheetdelete == "N") ? false : true;
                    chkTimesheetview.Checked = (Timesheetview == "N") ? false : true;
                    chkTimesheetreport.Checked = (Timesheetreport == "N") ? false : true;

                    //E Timesheet Permission
                    string ETimesheetPermission = ds.Tables[0].Rows[0]["ETimesheet"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["ETimesheet"].ToString();

                    string ETimesheetaddPermission = ETimesheetPermission.Length < 1 ? "Y" : ETimesheetPermission.Substring(0, 1);
                    string ETimesheeteditPermission = ETimesheetPermission.Length < 2 ? "Y" : ETimesheetPermission.Substring(1, 1);
                    string ETimesheetdeletePermission = ETimesheetPermission.Length < 3 ? "Y" : ETimesheetPermission.Substring(2, 1);
                    string ETimesheetviewPermission = ETimesheetPermission.Length < 4 ? "Y" : ETimesheetPermission.Substring(3, 1);
                    string ETimesheetreportPermission = ETimesheetPermission.Length < 6 ? "Y" : ETimesheetPermission.Substring(5, 1);

                    chkETimesheetadd.Checked = (ETimesheetaddPermission == "N") ? false : true;
                    chkETimesheetedit.Checked = (ETimesheeteditPermission == "N") ? false : true;
                    chkETimesheetdelete.Checked = (ETimesheetdeletePermission == "N") ? false : true;
                    chkETimesheetview.Checked = (ETimesheetviewPermission == "N") ? false : true;
                    chkETimesheetreport.Checked = (ETimesheetreportPermission == "N") ? false : true;

                    //MapR Permission
                    string MapRPermission = ds.Tables[0].Rows[0]["MapR"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["MapR"].ToString();

                    string MapaddPermission = MapRPermission.Length < 1 ? "Y" : MapRPermission.Substring(0, 1);
                    string MapeditPermission = MapRPermission.Length < 2 ? "Y" : MapRPermission.Substring(1, 1);
                    string MapdeletePermission = MapRPermission.Length < 3 ? "Y" : MapRPermission.Substring(2, 1);
                    string MapviewPermission = MapRPermission.Length < 4 ? "Y" : MapRPermission.Substring(3, 1);
                    string MapPermission = MapRPermission.Length < 6 ? "Y" : MapRPermission.Substring(5, 1);

                    chkMapAdd.Checked = (MapaddPermission == "N") ? false : true;
                    chkMapEdit.Checked = (MapeditPermission == "N") ? false : true;
                    chkMapDelete.Checked = (MapdeletePermission == "N") ? false : true;
                    chkMapView.Checked = (MapviewPermission == "N") ? false : true;
                    chkMapReport.Checked = (MapPermission == "N") ? false : true;

                    //RouteBuilder Permission
                    string RouteBuilderPermission = ds.Tables[0].Rows[0]["RouteBuilder"] == DBNull.Value ? "NNNN" : ds.Tables[0].Rows[0]["RouteBuilder"].ToString();

                    string RouteBuilderaddPermission = RouteBuilderPermission.Length < 1 ? "Y" : RouteBuilderPermission.Substring(0, 1);
                    string RouteBuildereditPermission = RouteBuilderPermission.Length < 2 ? "Y" : RouteBuilderPermission.Substring(1, 1);
                    string RouteBuilderdeletePermission = RouteBuilderPermission.Length < 3 ? "Y" : RouteBuilderPermission.Substring(2, 1);
                    string RouteBuilderviewPermission = RouteBuilderPermission.Length < 4 ? "Y" : RouteBuilderPermission.Substring(3, 1);
                    string RouteBuilderReportPermission = RouteBuilderPermission.Length < 6 ? "Y" : RouteBuilderPermission.Substring(5, 1);

                    chkRouteBuilderAdd.Checked = (RouteBuilderaddPermission == "N") ? false : true;
                    chkRouteBuilderEdit.Checked = (RouteBuildereditPermission == "N") ? false : true;
                    chkRouteBuilderDelete.Checked = (RouteBuilderdeletePermission == "N") ? false : true;
                    chkRouteBuilderView.Checked = (RouteBuilderviewPermission == "N") ? false : true;
                    chkRouteBuilderReport.Checked = (RouteBuilderReportPermission == "N") ? false : true;

                    //TicketSchedulePermission
                    string TicketSchedulePermission = ds.Tables[0].Rows[0]["Resolve"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["Resolve"].ToString();

                    string AddTicketeSchedulePermission = TicketSchedulePermission.Length < 1 ? "Y" : TicketSchedulePermission.Substring(0, 1);
                    string EditTicketeSchedulePermission = TicketSchedulePermission.Length < 2 ? "Y" : TicketSchedulePermission.Substring(1, 1);
                    string DeleteTicketeSchedulePermission = TicketSchedulePermission.Length < 3 ? "Y" : TicketSchedulePermission.Substring(2, 1);
                    string ViewTicketeSchedulePermission = TicketSchedulePermission.Length < 4 ? "Y" : TicketSchedulePermission.Substring(3, 1);
                    string ReportTicketeSchedulePermission = TicketSchedulePermission.Length < 6 ? "Y" : TicketSchedulePermission.Substring(5, 1);

                    chkResolveTicketAdd.Checked = (AddTicketeSchedulePermission == "N") ? false : true;
                    chkResolveTicketEdit.Checked = (EditTicketeSchedulePermission == "N") ? false : true;
                    chkResolveTicketDelete.Checked = (DeleteTicketeSchedulePermission == "N") ? false : true;
                    chkResolveTicketView.Checked = (ViewTicketeSchedulePermission == "N") ? false : true;
                    chkResolveTicketReport.Checked = (ReportTicketeSchedulePermission == "N") ? false : true;

                    // Sales Module Permission
                    string SalesModulePermission = ds.Tables[0].Rows[0]["SalesManager"] == DBNull.Value ? "Y" : ds.Tables[0].Rows[0]["SalesManager"].ToString();

                    chkSalesMgr.Checked = (SalesModulePermission == "N") ? false : true;

                    //Sales Permission
                    string SalesPermission = ds.Tables[0].Rows[0]["UserSales"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["UserSales"].ToString();

                    string SalesaddPermission = SalesPermission.Length < 1 ? "Y" : SalesPermission.Substring(0, 1);
                    string SaleseditPermission = SalesPermission.Length < 2 ? "Y" : SalesPermission.Substring(1, 1);
                    string SalesdeletePermission = SalesPermission.Length < 3 ? "Y" : SalesPermission.Substring(2, 1);
                    string SalesviewPermission = SalesPermission.Length < 4 ? "Y" : SalesPermission.Substring(3, 1);
                    string SalesReportPermission = SalesPermission.Length < 6 ? "Y" : SalesPermission.Substring(5, 1);

                    chkLeadAdd.Checked = (SalesaddPermission == "N") ? false : true;
                    chkLeadEdit.Checked = (SaleseditPermission == "N") ? false : true;
                    chkLeadDelete.Checked = (SalesdeletePermission == "N") ? false : true;
                    chkLeadView.Checked = (SalesviewPermission == "N") ? false : true;
                    chkLeadReport.Checked = (SalesReportPermission == "N") ? false : true;

                    //Payroll check
                    bool PRPermission = Convert.ToBoolean(ds.Tables[0].Rows[0]["PR"] == DBNull.Value ? false : ds.Tables[0].Rows[0]["PR"]);
                    payrollModulchck.Checked = (PRPermission == false) ? false : true;

                    //Payroll Permission Employee
                    string employeePermission = ds.Tables[0].Rows[0]["EmployeeMaint"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["EmployeeMaint"].ToString();

                    string AddemployeePermission = employeePermission.Length < 1 ? "Y" : employeePermission.Substring(0, 1);
                    string EditemployeePermission = employeePermission.Length < 2 ? "Y" : employeePermission.Substring(1, 1);
                    string DeleteemployeePermission = employeePermission.Length < 3 ? "Y" : employeePermission.Substring(2, 1);
                    string ViewemployeePermission = employeePermission.Length < 4 ? "Y" : employeePermission.Substring(3, 1);

                    empAdd.Checked = (AddTicketeSchedulePermission == "N") ? false : true;
                    empEdit.Checked = (EditTicketeSchedulePermission == "N") ? false : true;
                    empDelete.Checked = (DeleteTicketeSchedulePermission == "N") ? false : true;
                    empView.Checked = (ViewTicketeSchedulePermission == "N") ? false : true;

                    //Run Payroll Permission 

                    string RunPayrollPermission = ds.Tables[0].Rows[0]["PRProcess"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["PRProcess"].ToString();

                    string AddRunPayroll = RunPayrollPermission.Length < 1 ? "Y" : RunPayrollPermission.Substring(0, 1);
                    string EditRunPayrollPermission = RunPayrollPermission.Length < 2 ? "Y" : RunPayrollPermission.Substring(1, 1);
                    string DeleteRunPayrollPermission = RunPayrollPermission.Length < 3 ? "Y" : RunPayrollPermission.Substring(2, 1);
                    string ViewRunPayrollPermission = RunPayrollPermission.Length < 4 ? "Y" : RunPayrollPermission.Substring(3, 1);

                    runAdd.Checked = (AddRunPayroll == "N") ? false : true;
                    runEdit.Checked = (EditRunPayrollPermission == "N") ? false : true;
                    runDelete.Checked = (DeleteRunPayrollPermission == "N") ? false : true;
                    runView.Checked = (ViewRunPayrollPermission == "N") ? false : true;

                    //Payroll check Permission 
                    string payrollCheckPermission = ds.Tables[0].Rows[0]["PRRegister"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["PRRegister"].ToString();

                    string AddpayrollCheckPermission = payrollCheckPermission.Length < 1 ? "Y" : payrollCheckPermission.Substring(0, 1);
                    string EditpayrollCheckPermission = payrollCheckPermission.Length < 2 ? "Y" : payrollCheckPermission.Substring(1, 1);
                    string DeletepayrollCheckPermission = payrollCheckPermission.Length < 3 ? "Y" : payrollCheckPermission.Substring(2, 1);
                    string ViewpayrollCheckPermission = payrollCheckPermission.Length < 4 ? "Y" : payrollCheckPermission.Substring(3, 1);

                    payrollchckAdd.Checked = (AddpayrollCheckPermission == "N") ? false : true;
                    payrollchckEdit.Checked = (EditpayrollCheckPermission == "N") ? false : true;
                    payrollchckDelete.Checked = (DeletepayrollCheckPermission == "N") ? false : true;
                    payrollchckView.Checked = (ViewpayrollCheckPermission == "N") ? false : true;

                    //Payroll Form Permission 
                    string payrollFormPermission = ds.Tables[0].Rows[0]["PRReport"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["PRReport"].ToString();

                    string AddpayrollFormPermission = payrollFormPermission.Length < 1 ? "Y" : payrollFormPermission.Substring(0, 1);
                    string EdipayrollFormPermission = payrollFormPermission.Length < 2 ? "Y" : payrollFormPermission.Substring(1, 1);
                    string DeletepayrollFormPermission = payrollFormPermission.Length < 3 ? "Y" : payrollFormPermission.Substring(2, 1);
                    string ViewpayrollFormPermission = payrollFormPermission.Length < 4 ? "Y" : payrollFormPermission.Substring(3, 1);

                    payrollformAdd.Checked = (AddpayrollFormPermission == "N") ? false : true;
                    payrollformEdit.Checked = (EdipayrollFormPermission == "N") ? false : true;
                    payrollformDelete.Checked = (DeletepayrollFormPermission == "N") ? false : true;
                    payrollformView.Checked = (ViewpayrollFormPermission == "N") ? false : true;

                    //wages Permission 
                    string wagesPermission = ds.Tables[0].Rows[0]["PRWage"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["PRWage"].ToString();

                    string AddwagesPermission = wagesPermission.Length < 1 ? "Y" : wagesPermission.Substring(0, 1);
                    string EditwagesPermission = wagesPermission.Length < 2 ? "Y" : wagesPermission.Substring(1, 1);
                    string DeletewagesPermission = wagesPermission.Length < 3 ? "Y" : wagesPermission.Substring(2, 1);
                    string ViewwagesPermission = wagesPermission.Length < 4 ? "Y" : wagesPermission.Substring(3, 1);

                    wagesadd.Checked = (AddwagesPermission == "N") ? false : true;
                    wagesEdit.Checked = (EditwagesPermission == "N") ? false : true;
                    wagesDelete.Checked = (DeletewagesPermission == "N") ? false : true;
                    wagesView.Checked = (ViewwagesPermission == "N") ? false : true;

                    //Payroll Permission Employee
                    string deductionPermission = ds.Tables[0].Rows[0]["PRDeduct"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["PRDeduct"].ToString();

                    string AddedeductionPermission = deductionPermission.Length < 1 ? "Y" : deductionPermission.Substring(0, 1);
                    string EditdeductionPermission = deductionPermission.Length < 2 ? "Y" : deductionPermission.Substring(1, 1);
                    string DeletedeductionPermission = deductionPermission.Length < 3 ? "Y" : deductionPermission.Substring(2, 1);
                    string ViewdeductionPermission = deductionPermission.Length < 4 ? "Y" : deductionPermission.Substring(3, 1);

                    deductionsAdd.Checked = (AddedeductionPermission == "N") ? false : true;
                    deductionsEdit.Checked = (EditdeductionPermission == "N") ? false : true;
                    deductionsDelete.Checked = (DeletedeductionPermission == "N") ? false : true;
                    deductionsView.Checked = (ViewdeductionPermission == "N") ? false : true;

                    int taskPermission = ds.Tables[0].Rows[0]["ToDo"] == DBNull.Value ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["ToDo"]);

                    if (taskPermission == 1)
                    {
                        chkTasks.Checked = true;
                    }
                    else
                    {
                        chkTasks.Checked = false;
                    }
                    int completeTaskPermission = ds.Tables[0].Rows[0]["ToDoC"] == DBNull.Value ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["ToDoC"]);
                    if (completeTaskPermission == 1)
                    {
                        chkCompleteTask.Checked = true;
                    }
                    else
                    {
                        chkCompleteTask.Checked = false;
                    }
                    string FollowUpPermission = ds.Tables[0].Rows[0]["FU"] == DBNull.Value ? "YYYYYY" : ds.Tables[0].Rows[0]["FU"].ToString();

                    chkFollowUp.Checked = (FollowUpPermission == "NNNNNN") ? false : true;

                    string EstimatesPermission = ds.Tables[0].Rows[0]["Estimates"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["Estimates"].ToString();

                    string EstimatesaddPermission = EstimatesPermission.Length < 1 ? "Y" : EstimatesPermission.Substring(0, 1);
                    string EstimateseditPermission = EstimatesPermission.Length < 2 ? "Y" : EstimatesPermission.Substring(1, 1);
                    string EstimatesdeletePermission = EstimatesPermission.Length < 3 ? "Y" : EstimatesPermission.Substring(2, 1);
                    string EstimatesviewPermission = EstimatesPermission.Length < 4 ? "Y" : EstimatesPermission.Substring(3, 1);
                    string EstimatesReportPermission = EstimatesPermission.Length < 6 ? "Y" : EstimatesPermission.Substring(5, 1);

                    chkEstimateAdd.Checked = (EstimatesaddPermission == "N") ? false : true;
                    chkEstimateEdit.Checked = (EstimateseditPermission == "N") ? false : true;
                    chkEstimateDelete.Checked = (EstimatesdeletePermission == "N") ? false : true;
                    chkEstimateView.Checked = (EstimatesviewPermission == "N") ? false : true;
                    chkEstimateReport.Checked = (EstimatesReportPermission == "N") ? false : true;

                    string AwardEstimatesPermission = ds.Tables[0].Rows[0]["AwardEstimates"] == DBNull.Value ? "YYYYYY" : ds.Tables[0].Rows[0]["AwardEstimates"].ToString();

                    chkConvertEstimate.Checked = (AwardEstimatesPermission == "NNNNNN") ? false : true;

                    string salesSetupPermission = ds.Tables[0].Rows[0]["salessetup"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["salessetup"].ToString();

                    chkSalesSetup.Checked = (salesSetupPermission == "NNNNNN") ? false : true;

                    string proposalPermission = ds.Tables[0].Rows[0]["Proposal"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["Proposal"].ToString();

                    string oppaddPermission = proposalPermission.Length < 1 ? "Y" : proposalPermission.Substring(0, 1);
                    string oppeditPermission = proposalPermission.Length < 2 ? "Y" : proposalPermission.Substring(1, 1);
                    string oppdeletePermission = proposalPermission.Length < 3 ? "Y" : proposalPermission.Substring(2, 1);
                    string oppviewPermission = proposalPermission.Length < 4 ? "Y" : proposalPermission.Substring(3, 1);
                    string oppReportPermission = proposalPermission.Length < 6 ? "Y" : proposalPermission.Substring(5, 1);

                    chkOppAdd.Checked = (oppaddPermission == "N") ? false : true;
                    chkOppEdit.Checked = (oppeditPermission == "N") ? false : true;
                    chkOppDelete.Checked = (oppdeletePermission == "N") ? false : true;
                    chkOppView.Checked = (oppviewPermission == "N") ? false : true;
                    chkOppReport.Checked = (oppReportPermission == "N") ? false : true;

                    #region ticketPermission


                    /////// ticketPermission New
                    ///

                    // TicketVoidPermission

                    string strTicketVoidPermission = ds.Tables[0].Rows[0]["TicketVoidPermission"] == DBNull.Value ? "N" : ds.Tables[0].Rows[0]["TicketVoidPermission"].ToString();

                    chkTicketVoidPermission.Checked = (strTicketVoidPermission == "0") ? false : true;


                    // MassPayrollTicket
                    string strMassPayrollTicket = ds.Tables[0].Rows[0]["MassPayrollTicket"] == DBNull.Value ? "N" : ds.Tables[0].Rows[0]["MassPayrollTicket"].ToString();

                    chkMassPayrollTicket1.Checked = chkMassPayrollTicket.Checked = (strMassPayrollTicket == "Y") ? true : false;

                    // MassTimesheetCheck

                    string strMassTimesheetCheck = ds.Tables[0].Rows[0]["MassTimesheetCheck"] == DBNull.Value ? "N" : ds.Tables[0].Rows[0]["MassTimesheetCheck"].ToString();

                    chkMassTimesheetCheck.Checked = (strMassTimesheetCheck == "N") ? false : true;

                    string ticketPermission = ds.Tables[0].Rows[0]["Dispatch"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["Dispatch"].ToString();


                    string AddTicket = ticketPermission.Length < 1 ? "Y" : ticketPermission.Substring(0, 1);
                    string EditTicket = ticketPermission.Length < 2 ? "Y" : ticketPermission.Substring(1, 1);
                    string DeleteTicket = ticketPermission.Length < 3 ? "Y" : ticketPermission.Substring(2, 1);
                    string ViewTicket = ticketPermission.Length < 4 ? "Y" : ticketPermission.Substring(3, 1);


                    chkTicketListAdd.Checked = (AddTicket == "N") ? false : true;
                    chkTicketListEdit.Checked = (EditTicket == "N") ? false : true;
                    chkTicketListDelete.Checked = (DeleteTicket == "N") ? false : true;
                    chkTicketListView.Checked = (ViewTicket == "N") ? false : true;

                    /////// ApplyPermission New
                    string ApplyPermission = ds.Tables[0].Rows[0]["Apply"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["Apply"].ToString();

                    string AddReceivePayment = ApplyPermission.Length < 1 ? "Y" : ApplyPermission.Substring(0, 1);
                    string EditReceivePayment = ApplyPermission.Length < 2 ? "Y" : ApplyPermission.Substring(1, 1);
                    string DeleteReceivePayment = ApplyPermission.Length < 3 ? "Y" : ApplyPermission.Substring(2, 1);
                    string ViewReceivePayment = ApplyPermission.Length < 4 ? "Y" : ApplyPermission.Substring(3, 1);

                    chkReceivePaymentAdd.Checked = (AddReceivePayment == "N") ? false : true;
                    chkReceivePaymentEdit.Checked = (EditReceivePayment == "N") ? false : true;
                    chkReceivePaymentDelete.Checked = (DeleteReceivePayment == "N") ? false : true;
                    chkReceivePaymentView.Checked = (ViewReceivePayment == "N") ? false : true;

                    //// Online Payment Permission
                    //string OnlinePaymentPermission = ds.Tables[0].Rows[0]["OnlinePayment"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["OnlinePayment"].ToString();

                    ////string AddOnlinePayment = OnlinePaymentPermission.Length < 1 ? "Y" : OnlinePaymentPermission.Substring(0, 1);
                    ////string EditOnlinePayment = OnlinePaymentPermission.Length < 2 ? "Y" : OnlinePaymentPermission.Substring(1, 1);
                    ////string DeleteOnlinePayment = OnlinePaymentPermission.Length < 3 ? "Y" : OnlinePaymentPermission.Substring(2, 1);
                    //string ViewOnlinePayment = OnlinePaymentPermission.Length < 4 ? "Y" : OnlinePaymentPermission.Substring(3, 1);
                    ////string ReportOnlinePayment = OnlinePaymentPermission.Length < 5 ? "Y" : OnlinePaymentPermission.Substring(4, 1);
                    //string ApproveOnlinePayment = OnlinePaymentPermission.Length < 6 ? "Y" : OnlinePaymentPermission.Substring(5, 1);

                    ////chkOnlinePaymentAdd.Checked = (AddOnlinePayment == "N") ? false : true;
                    ////chkOnlinePaymentEdit.Checked = (EditOnlinePayment == "N") ? false : true;
                    ////chkOnlinePaymentDelete.Checked = (DeleteOnlinePayment == "N") ? false : true;
                    //chkOnlinePaymentView.Checked = (ViewOnlinePayment == "N") ? false : true;
                    ////chkOnlinePaymentReport.Checked = (ReportOnlinePayment == "N") ? false : true;
                    //chkOnlinePaymentApprove.Checked = (ApproveOnlinePayment == "N") ? false : true;

                    //DepositPermission
                    string DepositPermission = ds.Tables[0].Rows[0]["Deposit"] == DBNull.Value ? "NNNN" : ds.Tables[0].Rows[0]["Deposit"].ToString();

                    string AddDepositPermission = DepositPermission.Length < 1 ? "Y" : DepositPermission.Substring(0, 1);
                    string EditDepositPermission = DepositPermission.Length < 2 ? "Y" : DepositPermission.Substring(1, 1);
                    string DeleteDepositPermission = DepositPermission.Length < 3 ? "Y" : DepositPermission.Substring(2, 1);
                    string ViewDepositPermission = DepositPermission.Length < 4 ? "Y" : DepositPermission.Substring(3, 1);

                    chkMakeDepositAdd.Checked = (AddDepositPermission == "N") ? false : true;
                    chkMakeDepositEdit.Checked = (EditDepositPermission == "N") ? false : true;
                    chkMakeDepositDelete.Checked = (DeleteDepositPermission == "N") ? false : true;
                    chkMakeDepositView.Checked = (ViewDepositPermission == "N") ? false : true;


                    //CollectionsPermission
                    string CollectionsPermission = ds.Tables[0].Rows[0]["Collection"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["Collection"].ToString();

                    string AddCollectionsPermission = CollectionsPermission.Length < 1 ? "Y" : CollectionsPermission.Substring(0, 1);
                    string EditCollectionsPermission = CollectionsPermission.Length < 2 ? "Y" : CollectionsPermission.Substring(1, 1);
                    string DeleteCollectionsPermission = CollectionsPermission.Length < 3 ? "Y" : CollectionsPermission.Substring(2, 1);
                    string ViewCollectionsPermission = CollectionsPermission.Length < 4 ? "Y" : CollectionsPermission.Substring(3, 1);
                    string ReportCollectionsPermission = CollectionsPermission.Length < 6 ? "Y" : CollectionsPermission.Substring(5, 1);

                    chkCollectionsAdd.Checked = (AddCollectionsPermission == "N") ? false : true;
                    chkCollectionsEdit.Checked = (EditCollectionsPermission == "N") ? false : true;
                    chkCollectionsDelete.Checked = (DeleteCollectionsPermission == "N") ? false : true;
                    chkCollectionsView.Checked = (ViewCollectionsPermission == "N") ? false : true;
                    chkCollectionsReport.Checked = (ReportCollectionsPermission == "N") ? false : true;

                    #endregion

                    if (ds.Tables[0].Rows[0]["Control"].ToString() != string.Empty)
                    {
                        ProgFunc = ds.Tables[0].Rows[0]["Control"].ToString().Substring(0, 1);
                    }
                    if (ds.Tables[0].Rows[0]["users"].ToString() != string.Empty)
                    {
                        AccessUser = ds.Tables[0].Rows[0]["users"].ToString().Substring(0, 1);
                    }

                    if (EmpMaintenace == "Y")
                    {
                        chkEmpMainten.Checked = true;
                        //txtHourlyRate.Visible = true;
                        //lblHourlyR.Visible = true;
                    }
                    else
                    {
                        chkEmpMainten.Checked = false;
                        //txtHourlyRate.Visible = false;
                        //lblHourlyR.Visible = false;
                    }

                    if (TCTimeFix == "Y")
                        chkTimestampFix.Checked = true;
                    else
                        chkTimestampFix.Checked = false;

                    if (LocnRemark == "Y")
                    {
                        chkLocationview.Checked = true;
                    }
                    else
                    {
                        chkLocationview.Checked = false;
                    }

                    if (PurcOrders == "Y")
                    {
                        chkPOAdd.Checked = true;
                    }
                    else
                    {
                        chkPOAdd.Checked = false;
                    }

                    if (Exp == "Y")
                    {
                        chkExpenses.Checked = true;
                    }
                    else
                    {
                        chkExpenses.Checked = false;
                    }

                    if (ProgFunc == "Y")
                    {
                        chkProgram.Checked = true;
                    }
                    else
                    {
                        chkProgram.Checked = false;
                    }

                    if (AccessUser == "Y")
                    {
                        chkAccessUser.Checked = true;
                    }
                    else
                    {
                        chkAccessUser.Checked = false;
                    }

                    objPropUser.ConnConfig = Session["config"].ToString();

                    objPropUser.Supervisor = txtUserName.Text;

                    DataSet dsSupersUsers = new DataSet();

                    dsSupersUsers = objBL_User.getUserForSupervisor(objPropUser);

                    ViewState["superusers"] = dsSupersUsers.Tables[0];

                    ViewState["supersaved"] = dsSupersUsers.Tables[0];

                    //GetUserunderSuper();
                    gvUsers.Rebind();
                    if (Session["COPer"].ToString() == "1")
                    {
                        FillCompanySelected();
                    }
                    gvWagePayRate.Rebind();
                }
            }

            if (Session["adduser_message"] != null)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAddSuccess", "noty({text: '" + Session["adduser_message"].ToString() + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                Session["adduser_message"] = null;
            }

            ////Custom
            CreateUserCustomTable();
            BindUserCustomGrid();
        }

        CompanyPermission();

        DocumentPermission();

        HighlightSideMenu("progMgr", "lnkUsersSMenu", "progMgrSub");
        Page.LoadComplete += new EventHandler(Page_LoadComplete);
        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    private void HighlightSideMenu(string MenuParent, string PageLink, string SubMenuDiv)
    {
        HyperLink aNav = (HyperLink)Page.Master.FindControl(MenuParent);
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        aNav.CssClass = "active collapsible-header waves-effect waves-cyan collapsible-height-nl";

        //HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl(PageLink);
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        HtmlGenericControl div = (HtmlGenericControl)Page.Master.FindControl(SubMenuDiv);
        div.Style.Add("display", "block");
    }

    private void Page_LoadComplete(object sender, EventArgs e)
    {
        if (Request.QueryString["uid"] != null)
        {
            if (Request.QueryString["t"] == null)
            {
                DisableButton();
            }
        }

        if (ddlUserType.SelectedValue == "1" && chkMSAuthorisedDeviceOnly.Checked)
        {
            //RequiredFieldValidator12.Enabled = true;
        }

    }
    
    private void GetControl()
    {
        int Multilang = Convert.ToInt16(Session["IsMultiLang"]);
        if (Multilang == 0)
        {
            ddlLang.Visible = false;
            lblMultiLang.Visible = false;
        }
    }

    private void GetControlForPayroll()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Report.GetControlForPayroll(objPropUser);
        bool PR = Convert.ToBoolean(DBNull.Value.Equals(ds.Tables[0].Rows[0]["PR"]) ? 0 : ds.Tables[0].Rows[0]["PR"]);

        if (PR == true)
        {
            payrollsection.Visible = true;
            chkEmpMainten.Visible = false;
            chkMassPayrollTicket.Visible = false;
            chkMassPayrollTicket1.Visible = true;
        }
        else
        {
            chkMassPayrollTicket1.Visible = false;
            chkMassPayrollTicket.Visible = true;
        }
    }

    private void GetControlData()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getControl(objPropUser);

        // used Wages category can't delete if the Job Cost Labor = Burden Rate

        string wagesRage = ds.Tables[0].Rows[0]["JobCostLabor"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["JobCostLabor"].ToString();
        ViewState["JobCostLabor"] = wagesRage;
    }

    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
            dvCompanyPermission.Visible = true;
        }
        else
        {
            dvCompanyPermission.Visible = false;
        }
    }
    
    private void FillSupervisor()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getSupervisor(objPropUser);

        ddlSuper.DataSource = ds.Tables[0];
        ddlSuper.DataTextField = "fuser";
        ddlSuper.DataValueField = "fuser";
        ddlSuper.DataBind();

        ddlSuper.Items.Insert(0, new ListItem("-- Select --", ""));
        ddlSuper.Items.Insert(1, new ListItem("-- Add New --", "-1"));
    }

    private void FillCompany()
    {
        objCompany.ID = 0;
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();
        DataSet df = new DataSet();
        df = objBL_Company.getCompanyByID(objCompany);
        if (df.Tables[0].Rows.Count > 0)
        {
            lstSelectCompany.DataSource = df.Tables[0];
            lstSelectCompany.DataTextField = "Name";
            lstSelectCompany.DataValueField = "CompanyID";
            lstSelectCompany.DataBind();

            //ddlCompany.Items.Insert(0, new ListItem("Select", "0"));
        }
    }
    
    private void FillCompanySelected()
    {    //Selected Items which selected programmatically.  
        List<string> _selectedTechnology = new List<string>();
        if (Convert.ToInt32(ViewState["mode"]) == 1)
            objCompany.UserID = Convert.ToInt32(ViewState["userid"]);
        else
            objCompany.UserID = Convert.ToInt32(Session["UserID"].ToString());
        //objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();
        DataSet dc = new DataSet();
        dc = objBL_Company.getCompanyUserCoID(objCompany);
        if (dc.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dc.Tables[0].Rows.Count; i++)
            {
                if (Convert.ToBoolean(dc.Tables[0].Rows[i]["IsSel"].ToString()) == true)
                {
                    string strName = dc.Tables[0].Rows[i]["Name"].ToString();
                    _selectedTechnology.Add(strName);
                }
            }
        }
        if ((lstSelectCompany.Items.Count > 0) && (_selectedTechnology.Count > 0))
        {
            for (int i2 = 0; i2 < lstSelectCompany.Items.Count; i2++)
            {
                for (int i = 0; i < _selectedTechnology.Count; i++)
                {
                    if (lstSelectCompany.Items[i2].Text.ToString().Trim() == _selectedTechnology[i].ToString().Trim())
                    {
                        lstSelectCompany.Items[i2].Checked = true;
                    }
                }
            }
        }
        DisplaySelectedItems();
    }
    
    private void UserPermission()
    {
        if (Convert.ToString(Session["type"]) != "am")
        {
            DataTable dt = new DataTable(); dt = (DataTable)Session["userinfo"];

            string ProgFunc = dt.Rows[0]["Control"].ToString().Substring(0, 1);
            string AccessUser = dt.Rows[0]["users"].ToString().Substring(0, 1);
            string Maintenance = dt.Rows[0]["EmployeeMaint"].ToString().Substring(3, 1);

            if (ProgFunc == "N")
            {
                Response.Redirect("home.aspx?permission=no");
            }

            if (AccessUser == "N")
            {
                Response.Redirect("home.aspx?permission=no");
            }

            if (Maintenance == "N")
            {
                txtHourlyRate.Visible = false;
                lblHourlyR.Visible = false;
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidWageRateGrid() && Page.IsValid)
            {
                var pwError = string.Empty;
                if (!CheckPasswordPolicy(ddlUserType.SelectedValue, txtPassword.Text, txtUserName.Text, txtFName.Text, txtLName.Text, ref pwError))
                {
                    pwError = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(pwError);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "keySuperAlert", "noty({text: '" + pwError + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    return;
                }

                //if (ViewState["WageItems"] != null && Request.QueryString["uid"] != null)
                //{
                //    DataTable dataTable = (DataTable)ViewState["WageItems"];
                //    dataTable.Columns.Remove("Checked");
                //    dataTable.Columns.Remove("fDesc");
                //    dataTable.AcceptChanges();
                //    objPropUser.DtWage = dataTable;
                //}
                //else
                //{
                //    DataTable dataTable = (DataTable)ViewState["WageItems"];
                //    dataTable.Columns.Remove("Checked");
                //    dataTable.Columns.Remove("fDesc");
                //    dataTable.AcceptChanges();
                //    objPropUser.DtWage = dataTable;
                //}
                DataTable dataTable = (DataTable)ViewState["WageItems"];
                dataTable.Columns.Remove("Checked");
                dataTable.Columns.Remove("fDesc");
                dataTable.AcceptChanges();
                objPropUser.DtWage = dataTable;

                objPropUser.Address = txtAddress.Value;
                objPropUser.Cell = txtCell.Text;
                objPropUser.City = txtCity.Text;

                if (txtTerminationDt.Text.Trim() != string.Empty)
                {
                    objPropUser.DtFired = Convert.ToDateTime(txtTerminationDt.Text);
                }

                if (txtHireDt.Text.Trim() != string.Empty)
                {
                    objPropUser.DtHired = Convert.ToDateTime(txtHireDt.Text);
                }

                //objPropUser.DtHired =Convert.ToDateTime( txtHireDt.Text);
                objPropUser.Email = txtEmail.Text;
                objPropUser.FirstName = txtFName.Text;
                objPropUser.LastNAme = txtLName.Text;
                objPropUser.MiddleName = txtMName.Text.Trim();
                objPropUser.Password = txtPassword.Text.Trim();
                objPropUser.State = ddlState.Text;
                objPropUser.Status = Convert.ToInt32(rbStatus.SelectedValue);
                objPropUser.Tele = txtTelephone.Text;
                objPropUser.Username = txtUserName.Text.Trim();
                objPropUser.Zip = txtZip.Text;
                objPropUser.Remarks = txtRemarks.Text;
                objPropUser.DeviceID = txtDeviceID.Text.Trim();
                objPropUser.Pager = txtMsg.Text;
                GeneralFunctions objgn = new GeneralFunctions();
                objPropUser.InServer = txtInServer.Text.Trim();
                objPropUser.InUsername = txtInUSername.Text.Trim();
                objPropUser.InPassword = txtInPassword.Text.Trim();
                objPropUser.InPort = Convert.ToInt32(objgn.IsNull(txtinPort.Text.Trim(), "0"));
                objPropUser.OutServer = txtOutServer.Text.Trim();
                if (chkSame.Checked == true)
                {
                    objPropUser.OutUsername = txtInUSername.Text.Trim();
                    objPropUser.OutPassword = txtInPassword.Text.Trim();
                }
                else
                {
                    objPropUser.OutUsername = txtOutUsername.Text.Trim();
                    objPropUser.OutPassword = txtOutPassword.Text.Trim();
                }
                objPropUser.OutPort = Convert.ToInt32(objgn.IsNull(txtOutPort.Text.Trim(), "0"));
                objPropUser.SSL = chkSSL.Checked;
                objPropUser.TakeASentEmailCopy = chkTakeASentEmailCopy.Checked;
                objPropUser.BccEmail = txtBccEmail.Text.Trim();
                if (!string.IsNullOrEmpty(txtStartDate.Text))
                    objPropUser.FStart = Convert.ToDateTime(txtStartDate.Text);
                if (!string.IsNullOrEmpty(txtEndDate.Text))
                    objPropUser.FEnd = Convert.ToDateTime(txtEndDate.Text);
                if (chkSalesperson.Checked == true)
                {
                    objPropUser.Salesperson = 1;
                }
                else
                {
                    objPropUser.Salesperson = 0;
                }

                objPropUser.EmailAccount = Convert.ToInt16(chkEmailAcc.Checked);
                objPropUser.SalesAssigned = chkSalesAssigned.Checked ? true : false;
                objPropUser.EstApproveProposal = chkEstApprovalStatus.Checked ? true : false;
                objPropUser.NotificationOnAddOpportunity = chkNotification.Checked ? true : false;
                if (chkScheduleBrd.Checked == true)
                {
                    objPropUser.Schedule = 1;
                }
                else
                {
                    objPropUser.Schedule = 0;
                }

                if (chkMap.Checked == true)
                {
                    objPropUser.Mapping = 1;
                }
                else
                {
                    objPropUser.Mapping = 0;
                }

                objPropUser.Field = Convert.ToInt32(ddlUserType.SelectedValue);

                Decimal poLimit = 0;
                Decimal.TryParse(txtPOLimit.Text, out poLimit);
                objPropUser.POLimit = poLimit;
                objPropUser.POApprove = Convert.ToInt16(ddlPOApprove.SelectedValue);

                if (objPropUser.POApprove == 0)
                {
                    objPropUser.POApproveAmt = -1;
                    objPropUser.MinAmount = 0;
                    objPropUser.MaxAmount = 0;
                }
                else
                {
                    objPropUser.POApproveAmt = Convert.ToInt16(ddlPOApproveAmt.SelectedValue);
                    if (objPropUser.POApproveAmt == 0)
                    {
                        objPropUser.MinAmount = !String.IsNullOrEmpty(txtMinAmount.Text) ? Convert.ToDecimal(txtMinAmount.Text) : 0;
                        objPropUser.MaxAmount = !String.IsNullOrEmpty(txtMaxAmount.Text) ? Convert.ToDecimal(txtMaxAmount.Text) : 0;
                        if (objPropUser.MaxAmount <= objPropUser.MinAmount)
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWasdsdsdrnUp", "noty({text: 'Max Amount must be greater than Min amount.',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                            return;
                        }
                    }
                    else if (objPropUser.POApproveAmt == 1)
                    {
                        objPropUser.MinAmount = !String.IsNullOrEmpty(txtMinAmount.Text) ? Convert.ToDecimal(txtMinAmount.Text) : 0;
                        objPropUser.MaxAmount = 0;
                    }
                    else
                    {
                        objPropUser.MinAmount = 0;
                        objPropUser.MaxAmount = 0;
                    }
                }



                if (chkMSAuthorisedDeviceOnly.Checked)
                {
                    objPropUser.MSAuthorisedDeviceOnly = 1;
                }
                else
                {
                    objPropUser.MSAuthorisedDeviceOnly = 0;
                }

                if (chkAccessUser.Checked == true)
                {
                    objPropUser.AccessUser = "Y";
                }
                else
                {
                    objPropUser.AccessUser = "N";
                }


                if (chkExpenses.Checked == true)
                {
                    objPropUser.Expenses = "Y";
                }
                else
                {
                    objPropUser.Expenses = "N";
                }

                if (chkLocationview.Checked == true)
                {
                    objPropUser.LocationRemarks = "Y";
                }
                else
                {
                    objPropUser.LocationRemarks = "N";
                }

                if (chkProgram.Checked == true)
                {
                    objPropUser.ProgFunctions = "Y";
                }
                else
                {
                    objPropUser.ProgFunctions = "N";
                }

                if (chkPOAdd.Checked == true)
                {
                    objPropUser.PurchaseOrd = "Y";
                }
                else
                {
                    objPropUser.PurchaseOrd = "N";
                }

                #region TicketPermissions



                #endregion

                #region Permission

                ///      ProjectPermissions
                string ProjectPermissions = string.Empty;
                ProjectPermissions = chkProjectadd.Checked ? ProjectPermissions + "Y" : ProjectPermissions + "N";
                ProjectPermissions = chkProjectEdit.Checked ? ProjectPermissions + "Y" : ProjectPermissions + "N";
                ProjectPermissions = chkProjectDelete.Checked ? ProjectPermissions + "Y" : ProjectPermissions + "N";
                ProjectPermissions = chkProjectView.Checked ? ProjectPermissions + "Y" : ProjectPermissions + "N";
                objPropUser.ProjectPermissions = ProjectPermissions;

                // Customer Module Permission
                objPropUser.Customermodule = chkCustomerModule.Checked ? "Y" : "N";
                ///CustomerPermissions
                string CustomerPermissions = string.Empty;
                CustomerPermissions = chkCustomeradd.Checked ? CustomerPermissions + "Y" : CustomerPermissions + "N";
                CustomerPermissions = chkCustomeredit.Checked ? CustomerPermissions + "Y" : CustomerPermissions + "N";
                CustomerPermissions = chkCustomerdelete.Checked ? CustomerPermissions + "Y" : CustomerPermissions + "N";
                CustomerPermissions = chkCustomerview.Checked ? CustomerPermissions + "Y" : CustomerPermissions + "N";
                objPropUser.CustomerPermissions = CustomerPermissions;

                //LocationrPermissions
                string LocationrPermissions = string.Empty;
                LocationrPermissions = chkLocationadd.Checked ? LocationrPermissions + "Y" : LocationrPermissions + "N";
                LocationrPermissions = chkLocationedit.Checked ? LocationrPermissions + "Y" : LocationrPermissions + "N";
                LocationrPermissions = chkLocationdelete.Checked ? LocationrPermissions + "Y" : LocationrPermissions + "N";
                LocationrPermissions = chkLocationview.Checked ? LocationrPermissions + "Y" : LocationrPermissions + "N";
                objPropUser.LocationrPermissions = LocationrPermissions;

                //Receive Payment Permissions
                string ReceivePaymentPermissions = string.Empty;
                ReceivePaymentPermissions = chkReceivePaymentAdd.Checked ? ReceivePaymentPermissions + "Y" : ReceivePaymentPermissions + "N";
                ReceivePaymentPermissions = chkReceivePaymentEdit.Checked ? ReceivePaymentPermissions + "Y" : ReceivePaymentPermissions + "N";
                ReceivePaymentPermissions = chkReceivePaymentDelete.Checked ? ReceivePaymentPermissions + "Y" : ReceivePaymentPermissions + "N";
                ReceivePaymentPermissions = chkReceivePaymentView.Checked ? ReceivePaymentPermissions + "Y" : ReceivePaymentPermissions + "N";
                objPropUser.ApplyPermissions = ReceivePaymentPermissions;

                ////Online Payment Permissions
                //string OnlinePaymentPermissions = string.Empty;
                ////OnlinePaymentPermissions = chkOnlinePaymentAdd.Checked ? OnlinePaymentPermissions + "Y" : OnlinePaymentPermissions + "N";
                ////OnlinePaymentPermissions = chkOnlinePaymentEdit.Checked ? OnlinePaymentPermissions + "Y" : OnlinePaymentPermissions + "N";
                ////OnlinePaymentPermissions = chkOnlinePaymentDelete.Checked ? OnlinePaymentPermissions + "Y" : OnlinePaymentPermissions + "N";
                //OnlinePaymentPermissions = "NNN"; // Add, Edit, Delete 
                //OnlinePaymentPermissions = chkOnlinePaymentView.Checked ? OnlinePaymentPermissions + "Y" : OnlinePaymentPermissions + "N";
                //OnlinePaymentPermissions = OnlinePaymentPermissions + "N"; // Reports
                //OnlinePaymentPermissions = chkOnlinePaymentApprove.Checked ? OnlinePaymentPermissions + "Y" : OnlinePaymentPermissions + "N";
                //objPropUser.OnlinePaymentPermissions = OnlinePaymentPermissions;

                //DepositPermissions
                string DepositPermissions = string.Empty;
                DepositPermissions = chkMakeDepositAdd.Checked ? DepositPermissions + "Y" : DepositPermissions + "N";
                DepositPermissions = chkMakeDepositEdit.Checked ? DepositPermissions + "Y" : DepositPermissions + "N";
                DepositPermissions = chkMakeDepositDelete.Checked ? DepositPermissions + "Y" : DepositPermissions + "N";
                DepositPermissions = chkMakeDepositView.Checked ? DepositPermissions + "Y" : DepositPermissions + "N";
                objPropUser.DepositPermissions = DepositPermissions;


                //CollectionsPermissions
                string CollectionsPermissions = string.Empty;
                CollectionsPermissions = chkCollectionsAdd.Checked ? CollectionsPermissions + "Y" : CollectionsPermissions + "N";
                CollectionsPermissions = chkCollectionsEdit.Checked ? CollectionsPermissions + "Y" : CollectionsPermissions + "N";
                CollectionsPermissions = chkCollectionsDelete.Checked ? CollectionsPermissions + "Y" : CollectionsPermissions + "N";
                CollectionsPermissions = chkCollectionsView.Checked ? CollectionsPermissions + "Y" : CollectionsPermissions + "N";
                CollectionsPermissions += "N";
                CollectionsPermissions += chkCollectionsReport.Checked ? "Y" : "N";
                objPropUser.CollectionsPermissions = CollectionsPermissions;

                if (chkCreditHold.Checked == true)
                {
                    objPropUser.CreditHoldPermission = "YYYY";
                }
                else
                {
                    objPropUser.CreditHoldPermission = "NNNN";
                }

                if (chkCreditFlag.Checked == true)
                {
                    objPropUser.CreditFlagPermission = "YYYY";
                }
                else
                {
                    objPropUser.CreditFlagPermission = "NNNN";
                }

                //FinanceDataPermissions
                string FinancePermission = string.Empty;
                FinancePermission = chkViewFinance.Checked ? "Y" : "N";
                objPropUser.FinancePermission = FinancePermission;

                //ProjectListPermission
                string ProjectListPermission = string.Empty;
                ProjectListPermission = chkViewProjectList.Checked ? "Y" : "N";
                objPropUser.ProjectListPermission = ProjectListPermission;

                //BOMPermissions
                string BOMPermissions = string.Empty;
                BOMPermissions += chkAddBOM.Checked ? "Y" : "N";
                BOMPermissions += chkEditBOM.Checked ? "Y" : "N";
                BOMPermissions += chkDeleteBOM.Checked ? "Y" : "N";
                BOMPermissions += chkViewBOM.Checked ? "Y" : "N";
                objPropUser.BOMPermission = BOMPermissions;

                //WIPPermissions
                string WIPPermissions = string.Empty;
                WIPPermissions += chkAddWIP.Checked ? "Y" : "N";
                WIPPermissions += chkEditWIP.Checked ? "Y" : "N";
                WIPPermissions += chkDeleteWIP.Checked ? "Y" : "N";
                WIPPermissions += chkViewWIP.Checked ? "Y" : "N";
                WIPPermissions += "N";
                WIPPermissions += chkReportWIP.Checked ? "Y" : "N";
                objPropUser.WIPPermission = WIPPermissions;

                //MilestonesPermission
                string MilestonesPermission = string.Empty;
                MilestonesPermission += chkAddMilesStones.Checked ? "Y" : "N";
                MilestonesPermission += chkEditMilesStones.Checked ? "Y" : "N";
                MilestonesPermission += chkDeleteMilesStones.Checked ? "Y" : "N";
                MilestonesPermission += chkViewMilesStones.Checked ? "Y" : "N";
                objPropUser.MilestonesPermission = MilestonesPermission;


                ///InventoryItemPermissions
                string InventoryItemPermissions = string.Empty;
                InventoryItemPermissions = chkInventoryItemadd.Checked ? InventoryItemPermissions + "Y" : InventoryItemPermissions + "N";
                InventoryItemPermissions = chkInventoryItemedit.Checked ? InventoryItemPermissions + "Y" : InventoryItemPermissions + "N";
                InventoryItemPermissions = chkInventoryItemdelete.Checked ? InventoryItemPermissions + "Y" : InventoryItemPermissions + "N";
                InventoryItemPermissions = chkInventoryItemview.Checked ? InventoryItemPermissions + "Y" : InventoryItemPermissions + "N";
                objPropUser.InventoryItemPermissions = InventoryItemPermissions;

                ///InventoryAdjustmentPermissions
                string InventoryAdjustmentPermissions = string.Empty;
                InventoryAdjustmentPermissions = chkInventoryAdjustmentadd.Checked ? InventoryAdjustmentPermissions + "Y" : InventoryAdjustmentPermissions + "N";
                InventoryAdjustmentPermissions = chkInventoryAdjustmentedit.Checked ? InventoryAdjustmentPermissions + "Y" : InventoryAdjustmentPermissions + "N";
                InventoryAdjustmentPermissions = chkInventoryAdjustmentdelete.Checked ? InventoryAdjustmentPermissions + "Y" : InventoryAdjustmentPermissions + "N";
                InventoryAdjustmentPermissions = chkInventoryAdjustmentview.Checked ? InventoryAdjustmentPermissions + "Y" : InventoryAdjustmentPermissions + "N";
                objPropUser.InventoryAdjustmentPermissions = InventoryAdjustmentPermissions;

                ///InventoryWarehousePermissions
                string InventoryWarehousePermissions = string.Empty;
                InventoryWarehousePermissions = chkInventoryWareHouseadd.Checked ? InventoryWarehousePermissions + "Y" : InventoryWarehousePermissions + "N";
                InventoryWarehousePermissions = chkInventoryWareHouseedit.Checked ? InventoryWarehousePermissions + "Y" : InventoryWarehousePermissions + "N";
                InventoryWarehousePermissions = chkInventoryWareHousedelete.Checked ? InventoryWarehousePermissions + "Y" : InventoryWarehousePermissions + "N";
                InventoryWarehousePermissions = chkInventoryWareHouseview.Checked ? InventoryWarehousePermissions + "Y" : InventoryWarehousePermissions + "N";
                objPropUser.InventoryWarehousePermissions = InventoryWarehousePermissions;

                ///InventorysetupPermissions
                string InventorysetupPermissions = string.Empty;
                InventorysetupPermissions = chkInventorysetupadd.Checked ? InventorysetupPermissions + "Y" : InventorysetupPermissions + "N";
                InventorysetupPermissions = chkInventoryItemedit.Checked ? InventorysetupPermissions + "Y" : InventorysetupPermissions + "N";
                InventorysetupPermissions = chkInventorysetupdelete.Checked ? InventorysetupPermissions + "Y" : InventorysetupPermissions + "N";
                InventorysetupPermissions = chkInventorysetupview.Checked ? InventorysetupPermissions + "Y" : InventorysetupPermissions + "N";
                objPropUser.InventorysetupPermissions = InventorysetupPermissions;


                //DocumentPermissions
                string DocumentPermissions = string.Empty;
                DocumentPermissions += chkDocumentAdd.Checked ? "Y" : "N";
                DocumentPermissions += chkDocumentEdit.Checked ? "Y" : "N";
                DocumentPermissions += chkDocumentDelete.Checked ? "Y" : "N";
                DocumentPermissions += chkDocumentView.Checked ? "Y" : "N";
                objPropUser.DocumentPermissions = DocumentPermissions;

                //ContactPermissions
                string ContactPermissions = string.Empty;
                ContactPermissions += chkContactAdd.Checked ? "Y" : "N";
                ContactPermissions += chkContactEdit.Checked ? "Y" : "N";
                ContactPermissions += chkContactDelete.Checked ? "Y" : "N";
                ContactPermissions += chkContactView.Checked ? "Y" : "N";
                objPropUser.ContactPermission = ContactPermissions;

                //VendorsPermissions
                string VendorsPermissions = string.Empty;
                VendorsPermissions += chkVendorsAdd.Checked ? "Y" : "N";
                VendorsPermissions += chkVendorsEdit.Checked ? "Y" : "N";
                VendorsPermissions += chkVendorsDelete.Checked ? "Y" : "N";
                VendorsPermissions += chkVendorsView.Checked ? "Y" : "N";
                objPropUser.VendorsPermission = VendorsPermissions;

                ///InventoryFinancePermissions
                string InventoryFinancePermissions = string.Empty;
                InventoryFinancePermissions = chkInventoryFinanceAdd.Checked ? InventoryFinancePermissions + "Y" : InventoryFinancePermissions + "N";
                InventoryFinancePermissions = chkInventoryFinanceedit.Checked ? InventoryFinancePermissions + "Y" : InventoryFinancePermissions + "N";
                InventoryFinancePermissions = chkInventoryFinancedelete.Checked ? InventoryFinancePermissions + "Y" : InventoryFinancePermissions + "N";
                InventoryFinancePermissions = chkInventoryFinanceview.Checked ? InventoryFinancePermissions + "Y" : InventoryFinancePermissions + "N";
                objPropUser.InventoryFinancePermissions = InventoryFinancePermissions;

                // Project Templates
                string ProjectTempPermissions = string.Empty;
                ProjectTempPermissions += chkProjectTempAdd.Checked ? "Y" : "N";
                ProjectTempPermissions += chkProjectTempEdit.Checked ? "Y" : "N";
                ProjectTempPermissions += chkProjectTempDelete.Checked ? "Y" : "N";
                ProjectTempPermissions += chkProjectTempView.Checked ? "Y" : "N";
                objPropUser.ProjectTempPermissions = ProjectTempPermissions;

                // Financial Module Permission
                objPropUser.Financialmodule = chkFinancialmodule.Checked ? "Y" : "N";
                ///Chartof Account Permissions
                string ChartPermissions = string.Empty;
                ChartPermissions = chkChartAdd.Checked ? ChartPermissions + "Y" : ChartPermissions + "N";
                ChartPermissions = chkChartEdit.Checked ? ChartPermissions + "Y" : ChartPermissions + "N";
                ChartPermissions = chkChartDelete.Checked ? ChartPermissions + "Y" : ChartPermissions + "N";
                ChartPermissions = chkChartView.Checked ? ChartPermissions + "Y" : ChartPermissions + "N";
                objPropUser.ChartPermissions = ChartPermissions;

                //LocationrPermissions
                string JournalEntryPermissions = string.Empty;
                JournalEntryPermissions = chkJournalEntryAdd.Checked ? JournalEntryPermissions + "Y" : JournalEntryPermissions + "N";
                JournalEntryPermissions = chkJournalEntryEdit.Checked ? JournalEntryPermissions + "Y" : JournalEntryPermissions + "N";
                JournalEntryPermissions = chkJournalEntryDelete.Checked ? JournalEntryPermissions + "Y" : JournalEntryPermissions + "N";
                JournalEntryPermissions = chkJournalEntryView.Checked ? JournalEntryPermissions + "Y" : JournalEntryPermissions + "N";
                objPropUser.JournalEntryPermissions = JournalEntryPermissions;

                //Bank Reconciliation Permissions
                string BankReconciliationPermissions = string.Empty;
                BankReconciliationPermissions = chkBankAdd.Checked ? BankReconciliationPermissions + "Y" : BankReconciliationPermissions + "N";
                BankReconciliationPermissions = chkBankEdit.Checked ? BankReconciliationPermissions + "Y" : BankReconciliationPermissions + "N";
                BankReconciliationPermissions = chkBankDelete.Checked ? BankReconciliationPermissions + "Y" : BankReconciliationPermissions + "N";
                BankReconciliationPermissions = chkBankView.Checked ? BankReconciliationPermissions + "Y" : BankReconciliationPermissions + "N";
                objPropUser.BankReconciliationPermissions = BankReconciliationPermissions;

                //Write off Permission
                objPropUser.wirteOff = chkWriteOff.Checked ? "Y" : "N";
                #region Billing module

                objPropUser.Billingmodule = chkBillingmodule.Checked ? "Y" : "N";

                //InvoivePermissions
                string InvoivePermissions = string.Empty;
                InvoivePermissions += chkInvoicesAdd.Checked ? "Y" : "N";
                InvoivePermissions += chkInvoicesEdit.Checked ? "Y" : "N";
                InvoivePermissions += chkInvoicesDelete.Checked ? "Y" : "N";
                InvoivePermissions += chkInvoicesView.Checked ? "Y" : "N";
                objPropUser.InvoivePermissions = InvoivePermissions;



                //BillingCodesPermission
                string BillingCodesPermission = string.Empty;
                BillingCodesPermission += chkBillingcodesAdd.Checked ? "Y" : "N";
                BillingCodesPermission += chkBillingcodesEdit.Checked ? "Y" : "N";
                BillingCodesPermission += chkBillingcodesDelete.Checked ? "Y" : "N";
                BillingCodesPermission += chkBillingcodesView.Checked ? "Y" : "N";
                objPropUser.BillingCodesPermission = BillingCodesPermission;

                //Payment HistoryPermission
                string PaymentHistoryPermission = string.Empty;
                PaymentHistoryPermission += chkPaymentHistoryAdd.Checked ? "Y" : "N";
                PaymentHistoryPermission += chkPaymentHistoryEdit.Checked ? "Y" : "N";
                PaymentHistoryPermission += chkPaymentHistoryDelete.Checked ? "Y" : "N";
                PaymentHistoryPermission += chkPaymentHistoryView.Checked ? "Y" : "N";
                objPropUser.PaymentHistoryPermission = PaymentHistoryPermission;

                #endregion


                #region Purchasing module
                objPropUser.Purchasingmodule = chkPurchasingmodule.Checked ? "Y" : "N";

                //POPermission
                string POPermission = string.Empty;
                POPermission += chkPOAdd.Checked ? "Y" : "N";
                POPermission += chkPOEdit.Checked ? "Y" : "N";
                POPermission += chkPODelete.Checked ? "Y" : "N";
                POPermission += chkPOView.Checked ? "Y" : "N";
                objPropUser.POPermission = POPermission;

                //RPOPermission
                string RPOPermission = string.Empty;
                RPOPermission += chkRPOAdd.Checked ? "Y" : "N";
                RPOPermission += chkRPOEdit.Checked ? "Y" : "N";
                RPOPermission += chkRPODelete.Checked ? "Y" : "N";
                RPOPermission += chkRPOView.Checked ? "Y" : "N";
                objPropUser.RPOPermission = RPOPermission;

                objPropUser.PONotification = chkPONotification.Checked ? "Y" : "N";

                #endregion

                #region AccountPayable module
                objPropUser.AccountPayablemodule = chkAccountPayable.Checked ? "Y" : "N";

                //BillPermission 
                string BillPermission = string.Empty;
                BillPermission += chkAddBills.Checked ? "Y" : "N";
                BillPermission += chkEditBills.Checked ? "Y" : "N";
                BillPermission += chkDeleteBills.Checked ? "Y" : "N";
                BillPermission += chkViewBills.Checked ? "Y" : "N";
                objPropUser.APBill = BillPermission;

                //BillPayPermission
                string BillPayPermission = string.Empty;
                BillPayPermission += chkAddManageChecks.Checked ? "Y" : "N";
                BillPayPermission += chkEditManageChecks.Checked ? "Y" : "N";
                BillPayPermission += chkDeleteManageChecks.Checked ? "Y" : "N";
                BillPayPermission += chkViewManageChecks.Checked ? "Y" : "N";
                BillPayPermission += chkShowBankBalances.Checked ? "Y" : "N";
                objPropUser.APBillPay = BillPayPermission;

                //VendorPermission
                string VendorPermission = string.Empty;
                VendorPermission += chkVendorsAdd.Checked ? "Y" : "N";
                VendorPermission += chkVendorsEdit.Checked ? "Y" : "N";
                VendorPermission += chkVendorsDelete.Checked ? "Y" : "N";
                VendorPermission += chkVendorsView.Checked ? "Y" : "N";
                objPropUser.APVendor = VendorPermission;

                #endregion

                #region Recurring module
                objPropUser.Recurringmodule = chkRecurring.Checked ? "Y" : "N";

                //RecurringContractsPermission 
                string RecurringContractsPermission = string.Empty;
                RecurringContractsPermission += chkRecContractsAdd.Checked ? "Y" : "N";
                RecurringContractsPermission += chkRecContractsEdit.Checked ? "Y" : "N";
                RecurringContractsPermission += chkRecContractsDelete.Checked ? "Y" : "N";
                RecurringContractsPermission += chkRecContractsView.Checked ? "Y" : "N";
                objPropUser.RecurringContractsPermission = RecurringContractsPermission;

                //RecurringTicketsPermission
                string RecurringTicketsPermission = string.Empty;
                RecurringTicketsPermission += chkRecTicketsAdd.Checked ? "Y" : "N";
                RecurringTicketsPermission += chkRecTicketsEdit.Checked ? "Y" : "N";
                RecurringTicketsPermission += chkRecTicketsDelete.Checked ? "Y" : "N";
                RecurringTicketsPermission += chkRecTicketsView.Checked ? "Y" : "N";
                objPropUser.ProcessT = RecurringTicketsPermission;

                //RecurringInvoicesPermission
                string RecurringInvoicesPermission = string.Empty;
                RecurringInvoicesPermission += chkRecInvoicesAdd.Checked ? "Y" : "N";
                RecurringInvoicesPermission += chkRecInvoicesEdit.Checked ? "Y" : "N";
                RecurringInvoicesPermission += chkRecInvoicesDelete.Checked ? "Y" : "N";
                RecurringInvoicesPermission += chkRecInvoicesView.Checked ? "Y" : "N";
                objPropUser.ProcessC = RecurringInvoicesPermission;

                //SafetyTestsPermission
                string SafetyTestsPermission = string.Empty;
                SafetyTestsPermission += chkSafetyTestsAdd.Checked ? "Y" : "N";
                SafetyTestsPermission += chkSafetyTestsEdit.Checked ? "Y" : "N";
                SafetyTestsPermission += chkSafetyTestsDelete.Checked ? "Y" : "N";
                SafetyTestsPermission += chkSafetyTestsView.Checked ? "Y" : "N";
                objPropUser.SafetyTestsPermission = SafetyTestsPermission;

                //RenewEscalatePermission
                string RenewEscalatePermission = string.Empty;
                RenewEscalatePermission += chkRenewEscalateAdd.Checked ? "Y" : "N";
                RenewEscalatePermission += chkRenewEscalateEdit.Checked ? "Y" : "N";
                RenewEscalatePermission += chkRenewEscalateDelete.Checked ? "Y" : "N";
                RenewEscalatePermission += chkRenewEscalateView.Checked ? "Y" : "N";
                objPropUser.RenewEscalatePermission = RenewEscalatePermission;


                //ViolationsPermission
                string ViolationsPermission = string.Empty;
                ViolationsPermission += chkViolationsAdd.Checked ? "Y" : "N";
                ViolationsPermission += "N"; //Edit permission
                ViolationsPermission += chkViolationsDelete.Checked ? "Y" : "N";
                ViolationsPermission += "Y"; //View permission
                objPropUser.ViolationPermission = ViolationsPermission;

                #endregion

                #region Schedule module
                objPropUser.Schedulemodule = chkSchedule.Checked ? "Y" : "N";

                /////ScheduleBoardPermission
                ///
                string ScheduleBoardPermission = string.Empty;
                ScheduleBoardPermission += chkScheduleBoard.Checked ? "Y" : "N";
                ScheduleBoardPermission += chkScheduleBoard.Checked ? "Y" : "N";
                ScheduleBoardPermission += chkScheduleBoard.Checked ? "Y" : "N";
                ScheduleBoardPermission += chkScheduleBoard.Checked ? "Y" : "N";
                ScheduleBoardPermission += chkScheduleBoard.Checked ? "Y" : "N";
                ScheduleBoardPermission += chkScheduleBoard.Checked ? "Y" : "N";
                objPropUser.ScheduleBoardPermission = ScheduleBoardPermission;

                //TicketPermission 
                string TicketPermission = string.Empty;
                TicketPermission += chkTicketListAdd.Checked ? "Y" : "N";//1
                TicketPermission += chkTicketListEdit.Checked ? "Y" : "N";
                TicketPermission += chkTicketListDelete.Checked ? "Y" : "N";
                TicketPermission += chkTicketListView.Checked ? "Y" : "N";
                TicketPermission += chkDispatch.Checked ? "Y" : "N";
                TicketPermission += chkTicketListReport.Checked ? "Y" : "N";

                objPropUser.TicketPermission = TicketPermission;

                //TicketResolvedPermission 
                string TicketResolvedPermission = string.Empty;
                TicketResolvedPermission += chkResolveTicketAdd.Checked ? "Y" : "N";
                TicketResolvedPermission += chkResolveTicketEdit.Checked ? "Y" : "N";
                TicketResolvedPermission += chkResolveTicketDelete.Checked ? "Y" : "N";
                TicketResolvedPermission += chkResolveTicketView.Checked ? "Y" : "N";
                TicketResolvedPermission += "N";
                TicketResolvedPermission += chkResolveTicketReport.Checked ? "Y" : "N";
                objPropUser.TicketResolvedPermission = TicketResolvedPermission;


                //objPropUser.TimeStamsFixedPermission = TimeStamsFixedPermission;

                //Manual TimesheetPermission
                string MTimesheetPermission = string.Empty;
                MTimesheetPermission += chkTimesheetadd.Checked ? "Y" : "N";
                MTimesheetPermission += chkTimesheetedit.Checked ? "Y" : "N";
                MTimesheetPermission += chkTimesheetdelete.Checked ? "Y" : "N";
                MTimesheetPermission += chkTimesheetview.Checked ? "Y" : "N";
                MTimesheetPermission += "N";
                MTimesheetPermission += chkTimesheetreport.Checked ? "Y" : "N";
                objPropUser.MTimesheetPermission = MTimesheetPermission;

                //E-TimesheetPermission
                string ETimesheetPermission = string.Empty;
                ETimesheetPermission += chkETimesheetadd.Checked ? "Y" : "N";
                ETimesheetPermission += chkETimesheetedit.Checked ? "Y" : "N";
                ETimesheetPermission += chkETimesheetdelete.Checked ? "Y" : "N";
                ETimesheetPermission += chkETimesheetview.Checked ? "Y" : "N";
                ETimesheetPermission += "N";
                ETimesheetPermission += chkETimesheetreport.Checked ? "Y" : "N";
                objPropUser.ETimesheetPermission = ETimesheetPermission;

                //MapRPermission
                string MapRPermission = string.Empty;
                MapRPermission += chkMapAdd.Checked ? "Y" : "N";
                MapRPermission += chkMapEdit.Checked ? "Y" : "N";
                MapRPermission += chkMapDelete.Checked ? "Y" : "N";
                MapRPermission += chkMapView.Checked ? "Y" : "N";
                MapRPermission += "N";
                MapRPermission += chkMapReport.Checked ? "Y" : "N";
                objPropUser.MapRPermission = MapRPermission;

                //RouteBuildePermission
                string RouteBuildePermission = string.Empty;
                RouteBuildePermission += chkRouteBuilderAdd.Checked ? "Y" : "N";
                RouteBuildePermission += chkRouteBuilderEdit.Checked ? "Y" : "N";
                RouteBuildePermission += chkRouteBuilderDelete.Checked ? "Y" : "N";
                RouteBuildePermission += chkRouteBuilderView.Checked ? "Y" : "N";
                RouteBuildePermission += "N";
                RouteBuildePermission += chkRouteBuilderReport.Checked ? "Y" : "N";
                objPropUser.RouteBuilderPermission = RouteBuildePermission;

                //MassResolvePDATickets
                objPropUser.MassTimesheetCheck = chkMassTimesheetCheck.Checked ? "Y" : "N";

                //TicketVoidPermission

                objPropUser.TicketVoidPermission = chkTicketVoidPermission.Checked ? 1 : 0;

                // MassPayrollTicket

                objPropUser.MassPayrollTicket = chkMassPayrollTicket.Checked ? "Y" : "N";



                #endregion

                #region Sales module

                string LeadsPermission = string.Empty;
                LeadsPermission += chkLeadAdd.Checked ? "Y" : "N";
                LeadsPermission += chkLeadEdit.Checked ? "Y" : "N";
                LeadsPermission += chkLeadDelete.Checked ? "Y" : "N";
                LeadsPermission += chkLeadView.Checked ? "Y" : "N";
                LeadsPermission += "N";
                LeadsPermission += chkLeadReport.Checked ? "Y" : "N";
                objPropUser.SalesPermission = LeadsPermission;

                //MassResolvePDATickets
                objPropUser.TasksPermission = chkTasks.Checked ? 1 : 0;

                objPropUser.CompleteTasksPermission = chkCompleteTask.Checked ? 1 : 0;

                objPropUser.FollowUpPermission = chkFollowUp.Checked ? "YYYYYY" : "NNNNNN";
                //Opportunities  
                string ProposalPermission = string.Empty;
                ProposalPermission += chkOppAdd.Checked ? "Y" : "N";
                ProposalPermission += chkOppEdit.Checked ? "Y" : "N";
                ProposalPermission += chkOppDelete.Checked ? "Y" : "N";
                ProposalPermission += chkOppView.Checked ? "Y" : "N";
                ProposalPermission += "N";
                ProposalPermission += chkOppReport.Checked ? "Y" : "N";
                objPropUser.ProposalPermission = ProposalPermission;

                //Estimate   
                string EstimatePermission = string.Empty;
                EstimatePermission += chkEstimateAdd.Checked ? "Y" : "N";
                EstimatePermission += chkEstimateEdit.Checked ? "Y" : "N";
                EstimatePermission += chkEstimateDelete.Checked ? "Y" : "N";
                EstimatePermission += chkEstimateView.Checked ? "Y" : "N";
                EstimatePermission += "N";
                EstimatePermission += chkEstimateReport.Checked ? "Y" : "N";
                objPropUser.EstimatePermission = EstimatePermission;

                objPropUser.ConvertEstimatePermission = chkConvertEstimate.Checked ? "YYYYYY" : "NNNNNN";
                objPropUser.SalesSetupPermission = chkSalesSetup.Checked ? "YYYYYY" : "NNNNNN";
                #endregion

                #region payroll module

                //Employee
                string empPermission = string.Empty;
                empPermission += empAdd.Checked ? "Y" : "N";
                empPermission += empEdit.Checked ? "Y" : "N";
                empPermission += empDelete.Checked ? "Y" : "N";
                empPermission += empView.Checked ? "Y" : "N";
                empPermission += "NN";
                objPropUser.Employee = empPermission;


                //Run Payroll

                string RunPayrollPermission = string.Empty;
                RunPayrollPermission += runAdd.Checked ? "Y" : "N";
                RunPayrollPermission += runEdit.Checked ? "Y" : "N";
                RunPayrollPermission += runDelete.Checked ? "Y" : "N";
                RunPayrollPermission += runView.Checked ? "Y" : "N";
                RunPayrollPermission += "NN";
                objPropUser.PRProcess = RunPayrollPermission;

                //payroll Check   
                string PayrollCheckPermission = string.Empty;
                PayrollCheckPermission += payrollchckAdd.Checked ? "Y" : "N";
                PayrollCheckPermission += payrollchckEdit.Checked ? "Y" : "N";
                PayrollCheckPermission += payrollchckDelete.Checked ? "Y" : "N";
                PayrollCheckPermission += payrollchckView.Checked ? "Y" : "N";
                PayrollCheckPermission += "NN";
                objPropUser.PRRegister = PayrollCheckPermission;

                //payroll Form   
                string payrollformPermission = string.Empty;
                payrollformPermission += payrollformAdd.Checked ? "Y" : "N";
                payrollformPermission += payrollformEdit.Checked ? "Y" : "N";
                payrollformPermission += payrollformDelete.Checked ? "Y" : "N";
                payrollformPermission += payrollformView.Checked ? "Y" : "N";
                payrollformPermission += "NN";
                objPropUser.PRReport = payrollformPermission;

                //Wages  
                string wagesPermission = string.Empty;
                wagesPermission += wagesadd.Checked ? "Y" : "N";
                wagesPermission += wagesEdit.Checked ? "Y" : "N";
                wagesPermission += wagesDelete.Checked ? "Y" : "N";
                wagesPermission += wagesView.Checked ? "Y" : "N";
                wagesPermission += "NN";
                objPropUser.PRWage = wagesPermission;

                //deductions 
                string deductionsPermission = string.Empty;
                deductionsPermission += deductionsAdd.Checked ? "Y" : "N";
                deductionsPermission += deductionsEdit.Checked ? "Y" : "N";
                deductionsPermission += deductionsDelete.Checked ? "Y" : "N";
                deductionsPermission += deductionsView.Checked ? "Y" : "N";
                deductionsPermission += "NN";
                objPropUser.PRDeduct = deductionsPermission;

                #endregion

                objPropUser.PR = Convert.ToBoolean(payrollModulchck.Checked ? 1 : 0);

                objPropUser.Projectmodule = chkProjectmodule.Checked ? "Y" : "N";

                objPropUser.Inventorymodule = chkInventorymodule.Checked ? "Y" : "N";

                objPropUser.JobClosePermission = chkJobClosePermission.Checked ? "YYYYYY" : "NNNNNN";

                objPropUser.JobCompletedPermission = chkJobCompletedPermission.Checked ? "Y" : "N";

                objPropUser.JobReopenPermission = chkJobReopenPermission.Checked ? "Y" : "N";

                //Dispatch
                if (chkDispatch.Checked == true)
                {
                    objPropUser.Dispatch = "Y";
                }
                else
                {
                    objPropUser.Dispatch = "N";
                }

                #endregion Permission

                if (chkFinanceMgr.Checked == true)
                {
                    objPropUser.FChart = 1;
                    objPropUser.FGLAdj = 1;
                }
                else
                {
                    objPropUser.FChart = 0;
                    objPropUser.FGLAdj = 0;
                }
                if (chkFinanceStatement.Checked == true)
                {
                    objPropUser.FinanStatement = 1;
                }
                else
                {
                    objPropUser.FinanStatement = 0;
                }

                objPropUser.EmpMaintenance = Convert.ToInt16(chkEmpMainten.Checked);

                objPropUser.SalesMgr = Convert.ToInt16(chkSalesMgr.Checked);

                objPropUser.DefaultWorker = Convert.ToInt32(chkDefaultWorker.Checked);

                objPropUser.MassReview = Convert.ToInt32(chkMassReview.Checked);

                objPropUser.HourlyRate = Convert.ToDouble(objgn.IsNull(txtHourlyRate.Text.Replace("$", string.Empty), "0"));

                objPropUser.Timestampfix = Convert.ToInt32(chkTimestampFix.Checked);

                objPropUser.AddEquip = chkEquipmentsadd.Checked ? 1 : 0;

                objPropUser.EditEquip = chkEquipmentsedit.Checked ? 1 : 0;

                objPropUser.DeleteEquip = chkEquipmentsdelete.Checked ? 1 : 0;

                objPropUser.ViewEquip = chkEquipmentsview.Checked ? 1 : 0;

                objPropUser.PayMethod = Convert.ToInt16(ddlPayMethod.SelectedValue);
                objPropUser.PHours = txtHours.Text.Trim() != string.Empty ? Convert.ToDouble(txtHours.Text.Trim()) : 0;
                objPropUser.Salary = txtAmount.Text.Replace("$", string.Empty) != string.Empty ? Convert.ToDouble(txtAmount.Text.Replace("$", string.Empty)) : 0;
                objPropUser.EmpRefID = txtEmpID.Text.Trim();
                objPropUser.MileageRate = txtMileageRate.Text.Replace("$", string.Empty) != string.Empty ? Convert.ToDouble(txtMileageRate.Text.Replace("$", string.Empty)) : 0;
                objPropUser.PayPeriod = Convert.ToInt16(ddlPayPeriod.SelectedValue);
                objPropUser.SSN = txtSSNSIN.Text.Trim();
                objPropUser.Sex = ddlGender.SelectedValue;
                if (txtDateOfBirth.Text.Trim() != string.Empty)
                {
                    objPropUser.DBirth = Convert.ToDateTime(txtDateOfBirth.Text);
                }
                objPropUser.Race = ddlEthnicity.SelectedValue;

                List<string> Departmentids = new List<string>();
                foreach (RadComboBoxItem item in ddlDepartment.Items)
                {
                    if (item.Checked)
                    {
                        Departmentids.Add(item.Value);
                    }
                }
                string strDepartment = string.Join(",", Departmentids.ToArray());
                objPropUser.Department = strDepartment;
                //objPropUser.MOMUSer = txtMOMUserName.Text.Trim();
                //objPropUser.MOMPASS = txtMOMPassword.Text.Trim();

                objPropUser.ConnConfig = Session["config"].ToString();


                if (chkSuper.Checked == true)
                {
                    objPropUser.Supervisor = txtUserName.Text.Trim();

                    if (Convert.ToInt32(ViewState["mode"]) == 1)
                    {
                        UpdateUsers(1);
                    }
                }
                else
                {
                    objPropUser.Supervisor = ddlSuper.SelectedValue;

                    if (Convert.ToInt32(ViewState["mode"]) == 1)
                    {
                        UpdateUsers(0);
                    }

                }

                string strLic = "0";
                string strDay = "30";
                string strDate = System.DateTime.Now.ToShortDateString();
                string strUsername = txtUserName.Text;

                objPropUser.DBName = Session["dbname"].ToString();
                DataSet dsinfo = new DataSet();
                dsinfo = objBL_User.getLicenseInfoUser(objPropUser);

                if (dsinfo.Tables[0].Rows.Count > 0)
                {
                    string strRegDecr = SSTCryptographer.Decrypt(dsinfo.Tables[0].Rows[0]["str"].ToString(), "regu");
                    string[] strRegItems = strRegDecr.Split('&');
                    strLic = strRegItems[0];
                    strDay = strRegItems[1];
                    strDate = strRegItems[2];
                    objPropUser.UserLicID = Convert.ToInt32(dsinfo.Tables[0].Rows[0]["lid"]);
                }

                string strReg = strLic + "&" + strDay + "&" + strDate + "&" + strUsername;
                string strRegEncr = SSTCryptographer.Encrypt(strReg, "regu");
                objPropUser.UserLic = strRegEncr;

                objPropUser.Lang = ddlLang.SelectedValue;
                objPropUser.Lng = txtLongitude.Text;
                objPropUser.Lat = txtLatitude.Text;
                objPropUser.Country = ddlCountry.SelectedValue;
                objPropUser.EmName = txtEmName.Text;
                objPropUser.EmNum = txtEmNum.Text;
                objPropUser.authdevID = txtAuthdevID.Text;
                //objPropUser.Title = string.Empty;
                objPropUser.Title = txtUserTitle.Text;
                objPropUser.IsProjectManager = chkProjectManager.Checked;
                objPropUser.IsAssignedProject = chkAssignedProject.Checked;
                objPropUser.IsReCalculateLaborExpense = chkLaborExpense.Checked;
                Session["IsReCalculateLaborExpense"] = chkLaborExpense.Checked;

                if (ddlMerchantID.SelectedValue != string.Empty)
                {
                    objPropUser.MerchantInfoId = Convert.ToInt32(ddlMerchantID.SelectedValue);
                }

                objPropUser.RoleID = Convert.ToInt32(ddlUserRole.SelectedValue);
                objPropUser.ApplyUserRolePermission = Convert.ToInt32(ddlApplyUserRolePermission.SelectedValue);
                // objPropUser.dtPageData = PagePermissionData();

                //**
                //Get CustomValue
                processCustomTable(objPropUser);

                if (Convert.ToInt32(ViewState["mode"]) == 1)
                {
                    objPropUser.UserID = Convert.ToInt32(ViewState["userid"]);
                    if (!string.IsNullOrEmpty(ViewState["empid"].ToString()))
                    {
                        objPropUser.EmpId = Convert.ToInt32(ViewState["empid"]);
                    }
                    if (!string.IsNullOrEmpty(ViewState["rolid"].ToString()))
                    {
                        objPropUser.RolId = Convert.ToInt32(ViewState["rolid"]);
                    }
                    if (ViewState["workid"].ToString() != string.Empty)
                    {
                        objPropUser.WorkId = Convert.ToInt32(ViewState["workid"]);
                    }
                    else
                    {
                        objPropUser.WorkId = 0;
                    }

                    if (rbStatus.SelectedValue == "1" && hdnLocCount.Value != "0")
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnUp", "noty({text: 'This user has assigned locations and cannot be set inactive.',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }
                    else if (ddlUserType.SelectedValue == "0" && hdnLocCount.Value != "0")
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnUp", "noty({text: 'This user has assigned locations and cannot be removed from field.',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }
                    else
                    {
                        var updatedBy = Session["Username"].ToString();
                        objBL_User.UpdateUser(objPropUser, updatedBy);
                        hdnApplyUserRolePermissionOrg.Value = ddlApplyUserRolePermission.SelectedValue;
                        //objBL_User.UpdateUserPermission(objPropUser);
                        if (Session["COPer"].ToString() == "1")
                        {
                            SubmitCompany();
                            FillCompanySelected();
                        }
                        processSendMail(objPropUser);
                        processCreateTask(objPropUser);

                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "noty({text: 'User updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                        UpdateDocInfo();
                    }

                    // gvUserCustom Rebind
                    CreateUserCustomTable();
                    BindUserCustomGrid();

                    RadGrid_gvLogs.Rebind();
                }
                else
                {
                    var createdBy = Session["Username"].ToString();
                    objPropUser.UserID = objBL_User.AddUser(objPropUser, createdBy);
                    objBL_User.AddUserCustom(objPropUser);
                    hdnApplyUserRolePermissionOrg.Value = ddlApplyUserRolePermission.SelectedValue;
                    //objPropUser.UserID = objBL_User.AddUser(objPropUser);
                    //objBL_User.UpdateUserPermission(objPropUser);
                    if (Session["COPer"].ToString() == "1")
                    {
                        ViewState["AddUserID"] = objPropUser.UserID;
                        SubmitCompany();
                    }

                    processSendMail(objPropUser);
                    processCreateTask(objPropUser);

                    //Update Attachment Doc Info 
                    UpdateTempDateWhenCreatingNewUser(objPropUser.UserID.ToString());
                    UpdateDocInfo();

                    ViewState["mode"] = 0;
                    //lblMsg.Text = "User added successfully.";

                    //string strsuper = "User";
                    //if (Request.QueryString["sup"] != null)
                    //{
                    //    strsuper = "Supervisor";
                    //}

                    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: '" + strsuper + " added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    //ClearControls();
                    //ResetFormControlValues(this);

                    Session["adduser_message"] = "User added successfully!";
                    Response.Redirect("adduser.aspx?uid=" + objPropUser.UserID + "&type=" + objPropUser.Field);
                }

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
            }
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    
    private void SubmitCompany()
    {
        int UserId = 0;
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("UserID", typeof(int)),
                new DataColumn("CompanyID", typeof(int)),
                new DataColumn("OfficeID", typeof(int)),
                    new DataColumn("IsSel",typeof(bool)) });
            foreach (RadComboBoxItem li in lstSelectCompany.Items)
            {
                if (Convert.ToInt32(ViewState["mode"]) == 1)
                    UserId = Convert.ToInt32(ViewState["userid"]);
                else
                    UserId = Convert.ToInt32(ViewState["AddUserID"].ToString());
                int CompanyId = Convert.ToInt32(li.Value);
                objCompany.UserID = UserId;
                objCompany.CompanyID = CompanyId;
                if (li.Checked == true)
                {
                    int OfficeId = 0;
                    DataSet ds = new DataSet();
                    ds = objBL_Company.getCompanyByCompanyUserID(objCompany);
                    if (ds.Tables[0].Rows.Count <= 0)
                    {
                        dt.Rows.Add(UserId, CompanyId, OfficeId, false);
                    }
                }
                else
                {
                    objBL_Company.DeleteCompanyUserCo(objCompany);
                }
            }
            if (dt.Rows.Count > 0)
            {
                string consString = Session["config"].ToString();
                using (SqlConnection con = new SqlConnection(consString))
                {
                    using (SqlCommand cmd = new SqlCommand("spAddUserCompany"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@tblUserCompany", dt);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            DisplaySelectedItems();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    
    public void DisplaySelectedItems()
    {
        foreach (RadComboBoxItem item in lstSelectCompany.Items)
        {
            if (item.Checked)
            {
                lblSelectedTech.Text += "<li><b>" + item.Text + "<b></li>";
            }
        }
    }
    
    private void ClearControls()
    {
        txtAddress.Value = string.Empty;
        txtCell.Text = string.Empty;
        txtCity.Text = string.Empty;
        txtTerminationDt.Text = string.Empty;
        txtHireDt.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtFName.Text = string.Empty;
        txtLName.Text = string.Empty;
        //txtMName.Text=string.Empty;
        txtPassword.Text = string.Empty;
        ddlState.Text = string.Empty;
        rbStatus.SelectedIndex = -1;
        txtTelephone.Text = string.Empty;
        txtUserName.Text = string.Empty;
        txtZip.Text = string.Empty;
        //chkPDA.Checked = false;
        chkScheduleBrd.Checked = false;
        ddlUserType.SelectedIndex = -1;
        chkAccessUser.Checked = false;
        chkExpenses.Checked = false;
        chkProgram.Checked = false;
        chkPOAdd.Checked = false;
        chkScheduleBrd.Checked = false;
        chkMSAuthorisedDeviceOnly.Checked = false;

        //Finance

        chkViewFinance.Checked = false;

        //ProjectListPermission
        chkViewProjectList.Checked = false;

        //BOM
        chkAddBOM.Checked = false;
        chkEditBOM.Checked = false;
        chkDeleteBOM.Checked = false;
        chkViewBOM.Checked = false;

        //WIP
        chkAddWIP.Checked = false;
        chkEditWIP.Checked = false;
        chkDeleteWIP.Checked = false;
        chkViewWIP.Checked = false;
        chkReportWIP.Checked = false;

        //Milestones
        chkAddMilesStones.Checked = false;
        chkEditMilesStones.Checked = false;
        chkDeleteMilesStones.Checked = false;
        chkViewMilesStones.Checked = false;

        chkLocationadd.Checked = false;
        chkLocationedit.Checked = false;
        chkLocationdelete.Checked = false;
        chkLocationview.Checked = false;

        chkProjectadd.Checked = false;
        chkProjectEdit.Checked = false;
        chkProjectDelete.Checked = false;
        chkProjectView.Checked = false;


        chkScheduleBoard.Checked = false; //View

        chkEquipmentsadd.Checked = false;
        chkEquipmentsedit.Checked = false;
        chkEquipmentsdelete.Checked = false;
        chkEquipmentsview.Checked = false;


        chkContactAdd.Checked = false;
        chkContactEdit.Checked = false;
        chkContactDelete.Checked = false;
        chkContactView.Checked = false;

        chkVendorsAdd.Checked = false;
        chkVendorsEdit.Checked = false;
        chkVendorsDelete.Checked = false;
        chkVendorsView.Checked = false;

        chkBillingmodule.Checked = false;

        chkBillingcodesAdd.Checked = false;
        chkBillingcodesEdit.Checked = false;
        chkBillingcodesDelete.Checked = false;
        chkBillingcodesView.Checked = false;

        chkInvoicesAdd.Checked = false;
        chkInvoicesEdit.Checked = false;
        chkInvoicesDelete.Checked = false;
        chkInvoicesView.Checked = false;

        chkPurchasingmodule.Checked = false;

        chkPOAdd.Checked = false;
        chkPOEdit.Checked = false;
        chkPODelete.Checked = false;
        chkPOView.Checked = false;

        chkPONotification.Checked = false;

        chkProjectTempAdd.Checked = false;
        chkProjectTempEdit.Checked = false;
        chkProjectTempDelete.Checked = false;
        chkProjectTempView.Checked = false;

        chkDocumentAdd.Checked = false;
        chkDocumentEdit.Checked = false;
        chkDocumentDelete.Checked = false;
        chkDocumentView.Checked = false;

        chkCustomeradd.Checked = false;
        chkCustomeredit.Checked = false;
        chkCustomerdelete.Checked = false;
        chkCustomerview.Checked = false;

        chkInventoryItemadd.Checked = false;
        chkInventoryItemedit.Checked = false;
        chkInventoryItemdelete.Checked = false;
        chkInventoryItemview.Checked = false;

        chkInventoryAdjustmentadd.Checked = false;
        chkInventoryAdjustmentedit.Checked = false;
        chkInventoryAdjustmentdelete.Checked = false;
        chkInventoryAdjustmentview.Checked = false;

        chkInventoryWareHouseadd.Checked = false;
        chkInventoryWareHouseedit.Checked = false;
        chkInventoryWareHousedelete.Checked = false;
        chkInventoryWareHouseview.Checked = false;

        chkInventorysetupadd.Checked = false;
        chkInventorysetupedit.Checked = false;
        chkInventorysetupdelete.Checked = false;
        chkInventorysetupview.Checked = false;

        chkInventoryFinanceAdd.Checked = false;
        chkInventoryFinanceedit.Checked = false;
        chkInventoryFinancedelete.Checked = false;
        chkInventoryFinanceview.Checked = false;

        chkInventorymodule.Checked = false;
        chkProjectmodule.Checked = false;
        chkJobClosePermission.Checked = false;
        chkJobCompletedPermission.Checked = false;
        chkJobReopenPermission.Checked = false;

        chkProjectManager.Checked = false;

        chkAssignedProject.Checked = false;

        chkLaborExpense.Checked = false;

        chkTicketVoidPermission.Checked = false;
        chkMassPayrollTicket1.Checked = chkMassPayrollTicket.Checked = false;
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("users.aspx");
    }

    //private DataTable GetUser()
    //{
    //    objPropUser.UserID = Convert.ToInt32(Session["userid"]);
    //    DataSet ds = new DataSet();
    //    ds = objBL_User.getUserByID(objPropUser);
    //    return ds.Tables[0];
    //}

    private void UpdateUsers(int updatenew)
    {
        DataTable dtSaved = new DataTable();
        dtSaved = (DataTable)ViewState["supersaved"];

        foreach (DataRow dr in dtSaved.Rows)
        {
            UpdateSupervisorUser(Convert.ToInt32(dr["UserID"]), "");
        }

        if (updatenew == 1)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["superusers"];

            foreach (DataRow dr in dt.Rows)
            {
                UpdateSupervisorUser(Convert.ToInt32(dr["UserID"]), txtUserName.Text);
            }
        }
    }

    protected void ddlUserType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlUserType.SelectedValue == "1")
        {

            pnlWorker.Enabled = true;
            chkMap.Enabled = false;
            chkMap.Checked = true;
            chkMap_CheckedChanged(sender, e);
            txtAuthdevID.Enabled = false;
        }
        else
        {
            // RequiredFieldValidator12.Enabled = false;
            pnlWorker.Enabled = false;
            chkMap.Checked = false;
            chkMSAuthorisedDeviceOnly.Checked = false;
            chkScheduleBrd.Checked = false;
            ddlMerchantID.SelectedValue = "";
            txtDeviceID.Text = string.Empty;
            txtAuthdevID.Text = string.Empty;
            ddlSuper.SelectedValue = "";
            chkSuper.Checked = false;
            chkDefaultWorker.Checked = false;

        }
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["usersdata"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["userkey"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["type"].ToString() + "_" + Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);
        int c = dt.Rows.Count - 1;

        if (index < c)
        {
            if (Convert.ToInt16(dt.Rows[index + 1]["usertypeid"].ToString()) == 2)
                Response.Redirect("customeruser.aspx?uid=" + dt.Rows[index + 1]["userid"] + "&type=" + dt.Rows[index + 1]["usertypeid"]);
            else
                Response.Redirect("adduser.aspx?uid=" + dt.Rows[index + 1]["userid"] + "&type=" + dt.Rows[index + 1]["usertypeid"]);
        }
    }

    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["usersdata"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["userkey"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["type"].ToString() + "_" + Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);

        if (index > 0)
        {
            if (Convert.ToInt16(dt.Rows[index - 1]["usertypeid"].ToString()) == 2)
                Response.Redirect("customeruser.aspx?uid=" + dt.Rows[index - 1]["userid"] + "&type=" + dt.Rows[index - 1]["usertypeid"]);
            else
                Response.Redirect("adduser.aspx?uid=" + dt.Rows[index - 1]["userid"] + "&type=" + dt.Rows[index - 1]["usertypeid"]);
        }
    }

    protected void lnkLast_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["usersdata"];
        if (Convert.ToInt16(dt.Rows[dt.Rows.Count - 1]["usertypeid"].ToString()) == 2)
            Response.Redirect("customeruser.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["userid"] + "&type=" + dt.Rows[dt.Rows.Count - 1]["usertypeid"]);
        else
            Response.Redirect("adduser.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["userid"] + "&type=" + dt.Rows[dt.Rows.Count - 1]["usertypeid"]);
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["usersdata"];
        if (Convert.ToInt16(dt.Rows[0]["usertypeid"].ToString()) == 2)
            Response.Redirect("customeruser.aspx?uid=" + dt.Rows[0]["userid"] + "&type=" + dt.Rows[0]["usertypeid"]);
        else
            Response.Redirect("adduser.aspx?uid=" + dt.Rows[0]["userid"] + "&type=" + dt.Rows[0]["usertypeid"]);
    }

    private void DisableButton()
    {
        DataTable dt = new DataTable();
        if (Session["usersdata"] != null)
        {
            dt = (DataTable)Session["usersdata"];
        }
        else
        {
            GetUesrdata();
            dt = (DataTable)Session["usersdata"];
        }
        int uid = Int32.Parse(Request.QueryString["uid"].ToString());
        int lastItem = Int32.Parse(dt.Rows[dt.Rows.Count - 1]["userid"].ToString());
        int firstItem = Int32.Parse(dt.Rows[0]["userid"].ToString());
        if (uid == lastItem)
        {
            lnkNext.Enabled = false;
            lnkNext.CssClass = "disableControl";
            lnkLast.Enabled = false;
            lnkLast.CssClass = "disableControl";
        }
        else if (uid == firstItem)
        {
            lnkPrevious.Enabled = false;
            lnkPrevious.CssClass = "disableControl";
            lnkFirst.Enabled = false;
            lnkFirst.CssClass = "disableControl";
        }
    }

    private void GetUesrdata()
    {
        DataSet ds = new DataSet();
        objPropUser.DBName = Session["dbname"].ToString();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.IsSuper = 0;
        objPropUser.Supervisor = Session["username"].ToString();
        ds = objBL_User.getUser(objPropUser);
        Session["usersdata"] = ds.Tables[0];
    }

    protected void chkMap_CheckedChanged(object sender, EventArgs e)
    {
        if (chkMap.Checked == true)
        {
            if (ddlUserType.SelectedValue != "0")
            {
                lblDeviceID.Visible = true;
                txtDeviceID.Visible = true;
            }
        }
        else
        {
            lblDeviceID.Visible = false;
            txtDeviceID.Visible = false;
        }

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void ddlSuper_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSuper.SelectedIndex == 1)
        {
            if (Session["MSM"].ToString() == "TS")
            {
                ddlSuper.SelectedIndex = 0;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "keySuperAlert", "noty({text: 'Please add Supervisor and User from Total Service.',  type : 'information', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                return;
            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "dsds", "opensup1();", true);
        }
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void gvUsers_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;

        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;

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

    private void GetUsers()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Supervisor = txtUserName.Text;
        DataSet ds = new DataSet();
        ds = objBL_User.getUsersSuper(objPropUser);
        gvUsers.DataSource = ds.Tables[0];


    }

    private void GetUserunderSuper()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["superusers"];

        gvUsers.DataSource = dt;
        //gvUsers.DataBind();

    }

    private void UpdateSupervisorUser(int workid, string username)
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.WorkId = workid;
        objPropUser.Supervisor = username;
        objBL_User.UpdateSupervisorUser(objPropUser);
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        //GetUsers();
        hfCheckEdit.Value = "true";
        gvUsers.Rebind();
        lnkDone.Style.Add("display", "block");
        btnEdit.Style.Add("display", "none");
        gvUsers.Columns[0].Visible = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "updateField();", true);
    }

    protected void lnkDone_Click(object sender, EventArgs e)
    {
        hfCheckEdit.Value = "false";
        //GetUserunderSuper();
        int first = 0;
        string str = string.Empty;
        foreach (GridDataItem gr in gvUsers.Items)
        {
            CheckBox chkSelected = (CheckBox)gr.FindControl("chkSelect");
            Label lblUserID = (Label)gr.FindControl("lblId");

            if (chkSelected.Checked == true)
            {
                if (first == 0)
                {
                    str = lblUserID.Text;
                    first = 1;
                }
                else
                {
                    str = str + "," + lblUserID.Text;
                }
            }
        }


        if (str != string.Empty)
        {
            DataSet ds = new DataSet();

            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.Address = str;
            ds = objBL_User.getSelectedUser(objPropUser);

            ViewState["superusers"] = ds.Tables[0];
            //GetUserunderSuper();
            gvUsers.Rebind();
        }
        else
        {
            DataTable dt = (DataTable)ViewState["superusers"];
            DataTable dtSup = dt.Clone();
            ViewState["superusers"] = dtSup;
            //GetUserunderSuper();
            gvUsers.Rebind();
        }
        lnkDone.Style.Add("display", "none");
        btnEdit.Style.Add("display", "block");
        btnEdit.Attributes.Add("class", "btnEditAfter");
        gvUsers.Columns[0].Visible = false;

        DataTable dtsupr = (DataTable)ViewState["superusers"];
        if (dtsupr.Rows.Count == 0)
        {
            chkSuper.Enabled = true;
            ddlSuper.SelectedIndex = 0;
        }
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "updateField();", true);
    }

    private void ResetFormControlValues(Control parent)
    {
        foreach (Control c in parent.Controls)
        {
            if (c.Controls.Count > 0)
            {
                ResetFormControlValues(c);
            }
            else
            {
                switch (c.GetType().ToString())
                {
                    case "System.Web.UI.WebControls.DropDownList":
                        ((DropDownList)c).SelectedIndex = -1;
                        break;
                    case "System.Web.UI.WebControls.TextBox":
                        ((TextBox)c).Text = "";
                        break;
                    case "System.Web.UI.WebControls.CheckBox":
                        ((CheckBox)c).Checked = false;
                        break;
                    case "System.Web.UI.WebControls.RadioButton":
                        ((RadioButton)c).Checked = false;
                        break;
                }
            }
        }
    }

    private void GetMerchantID()
    {
        DataSet ds = new DataSet();
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        ds = objBL_Contracts.getPaymentGatewayInfo(objProp_Contracts);
        ddlMerchantID.DataSource = ds.Tables[0];
        ddlMerchantID.DataTextField = "merchantid";
        ddlMerchantID.DataValueField = "id";
        ddlMerchantID.DataBind();

        ddlMerchantID.Items.Insert(0, new ListItem("-- Select --", ""));
        ddlMerchantID.Items.Insert(1, new ListItem("-- Add New --", ""));
    }

    protected void lnkCancelMerchant_Click(object sender, EventArgs e)
    {
        ClearMerchant();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "focusElement", "focusElement();", true);
    }

    private void ClearMerchant()
    {
        txtMerchantID.Text = string.Empty;
        txtLoginID.Text = string.Empty;
        txtMerchantUsername.Text = string.Empty;
        txtMerchantPassword.Text = string.Empty;
        hdnMerchantInfoID.Value = "0";
        txtMerchantID.Enabled = true;
        //imgbtnDelete.Visible = false;

        GetMerchantID();
        //TogglePopUp();
        this.programmaticModalPopup.Hide();
    }

    private void TogglePopUp()
    {
        //string strScript = "TogglePopUp();";
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "keyDisplay", strScript, true);
        this.programmaticModalPopup.Show();
    }

    protected void lnkSaveMerchant_Click(object sender, EventArgs e)
    {
        try
        {
            string strMessage = string.Empty;

            if (Convert.ToInt32(hdnMerchantInfoID.Value) == 0)
            {
                strMessage = "Merchant added successfully!";
            }
            else
            {
                strMessage = "Merchant updated successfully!";
            }

            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.MerchantID = txtMerchantID.Text.Trim();
            objProp_Contracts.LoginID = txtLoginID.Text.Trim();
            objProp_Contracts.PaymentUser = txtMerchantUsername.Text.Trim();
            objProp_Contracts.MerchantInfoID = Convert.ToInt32(hdnMerchantInfoID.Value);
            objProp_Contracts.PaymentPass = AES_Algo.Encrypt(txtMerchantPassword.Text.Trim(), "MSMPAY", "4Bvq75DG", "SHA1", 1000, "pOWaTbO92LfXbh69JkYzfT7P465TNc0h", 256);

            objBL_Contracts.AddMerchant(objProp_Contracts);

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccMerchant", "noty({text: '" + strMessage + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false,dismissQueue: true});", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "hideModalPopup", "hideModalPopup();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "focusElement", "focusElement();", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrMerchant", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue: true});", true);
        }
    }

    protected void imgbtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.MerchantInfoID = Convert.ToInt32(hdnMerchantInfoID.Value);

            objBL_Contracts.DeleteMerchant(objProp_Contracts);

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccMerchantDel", "noty({text: 'Merchant deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false,dismissQueue: true});", true);

            ClearMerchant();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrMerchantDel", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue: true});", true);
        }
    }

    protected void chkSuper_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSuper.Checked)
        {
            ddlSuper.Enabled = false;
            ddlSuper.SelectedIndex = 0;
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "gvUsersShow(true);", true);
        }
        else
        {
            ddlSuper.Enabled = true;
            ddlSuper.SelectedIndex = 0;
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "gvUsersShow(false);", true);
        }
        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void chkSalesperson_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSalesperson.Checked == true)
        {
            if (Session["MSM"].ToString() != "TS")
            {
                chkNotification.Enabled = chkSalesAssigned.Enabled = true;
            }
        }
        else
        {
            chkNotification.Enabled = chkNotification.Checked = chkSalesAssigned.Enabled = chkSalesAssigned.Checked = false;
        }

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    // TODO: Thomas need to update for ES-33
    
    protected void chkSame_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSame.Checked == true)
        {
            txtOutUsername.Text = txtInUSername.Text.Trim();
            txtOutPassword.Text = txtInPassword.Text.Trim();
            //txtOutPassword.Attributes["value"] = txtInPassword.Text.Trim();
            //txtBccEmail.Text = txtInUSername.Text.Trim();
            txtOutPassword.Enabled = false;
            txtOutUsername.Enabled = false;
            //txtOutPassword.ReadOnly = true;
            //txtOutUsername.ReadOnly = true;
            RequiredFieldValidator26.Enabled = false;
        }
        else
        {
            txtOutPassword.Enabled = true;
            txtOutUsername.Enabled = true;
            //txtOutPassword.ReadOnly = false;
            //txtOutUsername.ReadOnly = false;
            RequiredFieldValidator26.Enabled = true;
        }
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);

    }

    // TODO: Thomas need to update for ES-33
    protected void chkEmailAcc_CheckedChanged(object sender, EventArgs e)
    {
        if (chkEmailAcc.Checked == true)
        {
            pnlEmailAccount.Visible = true;
            rfvEmail.Enabled = true;
            // ES-33
            // Update incoming email username in case txtEmail was set but incoming email username is empty
            //if(!string.IsNullOrWhiteSpace(txtEmail.Text) && string.IsNullOrWhiteSpace(txtInUSername.Text)) {
            if (!(bool)ViewState["IsSetEmailAccount"])
            {
                txtInUSername.Text = txtEmail.Text.Trim();
            }

            //if(ViewState["mode"].ToString() == "1")
            //{
            //    pnlEmailSignature.Visible = true;
            //    RadGrid_EmailSigns.Rebind();
            //}
            //else
            //{
            //    pnlEmailSignature.Visible = false;
            //}
        }
        else
        {
            pnlEmailAccount.Visible = false;
            rfvEmail.Enabled = false;
        }
        // ES-33

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void btnTestOut_Click(object sender, EventArgs e)
    {
        Mail mail = new Mail();
        try
        {
            mail.Username = txtOutUsername.Text.Trim();
            mail.Password = txtOutPassword.Text.Trim();
            mail.SMTPHost = txtOutServer.Text.Trim();
            mail.SMTPPort = Convert.ToInt32(txtOutPort.Text.Trim());

            mail.InUsername = txtInUSername.Text.Trim();
            mail.InPassword = txtInPassword.Text.Trim();
            mail.InHost = txtInServer.Text.Trim();
            mail.InPort = Convert.ToInt32(txtinPort.Text.Trim());

            var emailValidation = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
            if (emailValidation.IsValid(txtOutUsername.Text.Trim()))
            {
                mail.From = txtOutUsername.Text.Trim();
                mail.To = txtOutUsername.Text.Split(';', ',').OfType<string>().ToList();
            }
            else
            {
                mail.From = txtEmail.Text.Trim();
                mail.To = txtEmail.Text.Split(';', ',').OfType<string>().ToList();
            }

            mail.Bcc = txtBccEmail.Text.Split(';', ',').OfType<string>().ToList();
            mail.Title = "Test Email";
            mail.Text = "Test Email from Mobile Office Manager.";
            mail.RequireAutentication = true;
            mail.TakeASentEmailCopy = chkTakeASentEmailCopy.Checked;
            mail.SSL = chkSSL.Checked;
            mail.IsIncludeSignature = true;
            mail.IsBodyHtml = true;
            mail.SendTest(Request.QueryString["uid"]);

            try
            {
                // Emailing logs: testing only
                EmailLog emailLog = new EmailLog()
                {
                    ConnConfig = Session["config"].ToString(),
                    SessionNo = Guid.NewGuid().ToString(),
                    EmailDate = DateTime.Now,
                    From = mail.From,
                    Ref = Session["userid"] != null ? Convert.ToInt32(Session["userid"].ToString()) : 0,
                    Screen = "AddUser",
                    Sender = mail.From,
                    To = String.Join(", ", mail.To.ToArray()),
                    Status = 1,
                    SysErrMessage = string.Empty,
                    Username = Session["Username"] != null ? Session["Username"].ToString() : "",
                    UsrErrMessage = string.Empty,
                    Function = "TestOut"
                };
                BL_EmailLog bL_EmailLog = new BL_EmailLog();
                bL_EmailLog.AddEmailLog(emailLog);
            }
            catch (Exception)
            {
            }

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Email sent successfully.');", true);

        }
        catch (Exception ex)
        {
            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            //string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            // Emailing logs: testing only
            try
            {
                EmailLog emailLog = new EmailLog()
                {
                    ConnConfig = Session["config"].ToString(),
                    SessionNo = Guid.NewGuid().ToString(),
                    EmailDate = DateTime.Now,
                    From = mail.From,
                    Ref = Session["userid"] != null ? Convert.ToInt32(Session["userid"].ToString()) : 0,
                    Screen = "AddUser",
                    Sender = mail.From,
                    To = String.Join(", ", mail.To.ToArray()),
                    Status = 0,
                    SysErrMessage = ex.Message,
                    Username = Session["Username"] != null ? Session["Username"].ToString() : "",
                    UsrErrMessage = ex.Message,
                    Function = "TestOut"
                };
                BL_EmailLog bL_EmailLog = new BL_EmailLog();
                bL_EmailLog.AddEmailLog(emailLog);
            }
            catch (Exception)
            {
            }

            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
            WebBaseUtility.ShowEmailErrorMessageBox(this, Page.GetType(), ex);
        }
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    private void FillDepartment()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getDepartment(objPropUser);

        ddlDepartment.DataSource = ds.Tables[0];
        ddlDepartment.DataTextField = "type";
        ddlDepartment.DataValueField = "id";
        ddlDepartment.DataBind();
    }

    protected void ddlPayMethod_SelectedIndexChanged1(object sender, EventArgs e)
    {

        if (ddlPayMethod.SelectedValue == "2")
        {
            txtHours.Enabled = true;
            txtAmount.Enabled = false;
        }
        else if (ddlPayMethod.SelectedValue == "0")
        {
            txtHours.Enabled = false;
            txtAmount.Enabled = true;
        }
        else
        {
            txtAmount.Enabled = false;
            txtHours.Enabled = false;
        }

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "updateField();", true);
    }

    private void CreateWageTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Wage", typeof(int));
        dt.Columns.Add("Reg", typeof(double));
        dt.Columns.Add("OT", typeof(double));
        dt.Columns.Add("DT", typeof(double));
        dt.Columns.Add("TT", typeof(double));
        dt.Columns.Add("NT", typeof(double));
        dt.Columns.Add("CReg", typeof(double));
        dt.Columns.Add("COT", typeof(double));
        dt.Columns.Add("CDT", typeof(double));
        dt.Columns.Add("CTT", typeof(double));
        dt.Columns.Add("CNT", typeof(double));
        dt.Columns.Add("Status", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));

        DataRow dr = dt.NewRow();
        dr["Wage"] = 0;
        dr["Reg"] = 0;
        dr["OT"] = 0;
        dr["DT"] = 0;
        dr["TT"] = 0;
        dr["NT"] = 0;
        dr["CReg"] = 0;
        dr["COT"] = 0;
        dr["CDT"] = 0;
        dr["CTT"] = 0;
        dr["CNT"] = 0;
        dr["Status"] = 0;
        dr["fDesc"] = "";
        dt.Rows.Add(dr);
        ViewState["WageItems"] = dt;
        gvWagePayRate.DataSource = dt;

    }

    protected void gvWagePayRate_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = gvWagePayRate.Items.Count - 1;
        GridDataItem row = gvWagePayRate.Items[rowIndex];

        DataTable dt = new DataTable();

        dt = GetWageGridItems();
        DataRow dr = dt.NewRow();
        dr["Wage"] = 0;
        dr["Reg"] = 0;
        dr["OT"] = 0;
        dr["DT"] = 0;
        dr["TT"] = 0;
        dr["NT"] = 0;
        dr["CReg"] = 0;
        dr["COT"] = 0;
        dr["CDT"] = 0;
        dr["CTT"] = 0;
        dr["CNT"] = 0;
        dr["Check"] = false;
        dt.Rows.Add(dr);

        gvWagePayRate.DataSource = dt;
        ViewState["WageItems"] = dt;
    }

    private DataTable GetWageGridItems()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("Wage", typeof(int));
        dt.Columns.Add("Reg", typeof(double));
        dt.Columns.Add("OT", typeof(double));
        dt.Columns.Add("DT", typeof(double));
        dt.Columns.Add("TT", typeof(double));
        dt.Columns.Add("NT", typeof(double));
        dt.Columns.Add("CReg", typeof(double));
        dt.Columns.Add("COT", typeof(double));
        dt.Columns.Add("CDT", typeof(double));
        dt.Columns.Add("CTT", typeof(double));
        dt.Columns.Add("CNT", typeof(double));
        dt.Columns.Add("Status", typeof(int));
        dt.Columns.Add("Checked", typeof(bool));
        dt.Columns.Add("fDesc", typeof(string));


        try
        {

            foreach (GridDataItem gr in gvWagePayRate.Items)
            {
                HiddenField hdnWageID = (HiddenField)gr.FindControl("hdnWageID");
                DropDownList ddlWageStatus = (DropDownList)gr.FindControl("ddlWageStatus");

                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                TextBox txtWage = (TextBox)gr.FindControl("txtWage");

                TextBox txtReg = (TextBox)gr.FindControl("txtReg");
                TextBox txtOt = (TextBox)gr.FindControl("txtOt");
                TextBox txtNt = (TextBox)gr.FindControl("txtNt");
                TextBox txtDt = (TextBox)gr.FindControl("txtDt");
                TextBox txtTt = (TextBox)gr.FindControl("txtTt");


                TextBox txtCReg = (TextBox)gr.FindControl("txtCReg");
                TextBox txtCOt = (TextBox)gr.FindControl("txtCOt");
                TextBox txtCNt = (TextBox)gr.FindControl("txtCNt");
                TextBox txtCDt = (TextBox)gr.FindControl("txtCDt");
                TextBox txtCTt = (TextBox)gr.FindControl("txtCTt");


                DataRow dr = dt.NewRow();
                if (hdnWageID.Value != "0")
                {
                    dr["Wage"] = Convert.ToInt32(hdnWageID.Value);
                    dr["fDesc"] = txtWage.Text;
                }
                else
                {
                    dr["Wage"] = 0;
                    dr["fDesc"] = txtWage.Text;
                }
                if (!string.IsNullOrEmpty(txtReg.Text))
                {
                    dr["Reg"] = Convert.ToDouble(txtReg.Text);
                }
                if (!string.IsNullOrEmpty(txtOt.Text))
                {
                    dr["OT"] = Convert.ToDouble(txtOt.Text);
                }
                if (!string.IsNullOrEmpty(txtNt.Text))
                {
                    dr["NT"] = Convert.ToDouble(txtNt.Text);
                }
                if (!string.IsNullOrEmpty(txtDt.Text))
                {
                    dr["DT"] = Convert.ToDouble(txtDt.Text);
                }
                if (!string.IsNullOrEmpty(txtTt.Text))
                {
                    dr["TT"] = Convert.ToDouble(txtTt.Text);
                }
                if (!string.IsNullOrEmpty(txtCReg.Text))
                {
                    dr["CReg"] = Convert.ToDouble(txtCReg.Text);
                }
                if (!string.IsNullOrEmpty(txtCOt.Text))
                {
                    dr["COT"] = Convert.ToDouble(txtCOt.Text);
                }
                if (!string.IsNullOrEmpty(txtCNt.Text))
                {
                    dr["CNT"] = Convert.ToDouble(txtCNt.Text);
                }
                if (!string.IsNullOrEmpty(txtCDt.Text))
                {
                    dr["CDT"] = Convert.ToDouble(txtCDt.Text);
                }
                if (!string.IsNullOrEmpty(txtCTt.Text))
                {
                    dr["CTT"] = Convert.ToDouble(txtCTt.Text);
                }

                if (!string.IsNullOrEmpty(ddlWageStatus.SelectedValue))
                {
                    dr["Status"] = Convert.ToInt16(ddlWageStatus.SelectedValue);
                }
                try
                {

                    dr["Checked"] = chkSelect.Checked;
                }

                catch (Exception)
                {
                    dr["Checked"] = false;
                }

                dt.Rows.Add(dr);
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }

    protected void imgBtnWageDelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string TicketID = string.Empty;
            DataTable dt = GetWageGridItems();
            if (dt.Rows.Count > 1)
            {
                foreach (GridDataItem gr in gvWagePayRate.Items)
                {
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                    if (chkSelect.Checked == true)
                    {

                        HiddenField hdnIsUsed = (HiddenField)gr.FindControl("hdnIsUsed");
                        HiddenField hdnWageID = (HiddenField)gr.FindControl("hdnWageID");
                        TextBox TextBox = (TextBox)gr.FindControl("txtWage");

                        Int32 UserID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"]);
                        if (!IsWageRateIsUsed(UserID, Convert.ToInt32(hdnWageID.Value), out TicketID))
                        {
                            DataRow dr = dt.Select("Wage = " + Convert.ToInt32(hdnWageID.Value)).FirstOrDefault();
                            if (dr != null)
                                dt.Rows.Remove(dr);
                        }
                        else
                        {
                            string str = "Wage category is used in Ticket #" + TicketID + "!";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "key112", "noty({text: '" + str + "',dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                            return;
                        }
                    }
                }
            }
            else if (dt.Rows.Count > 0)
            {
                foreach (GridDataItem gr in gvWagePayRate.Items)
                {
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                    if (chkSelect.Checked == true)
                    {


                        HiddenField hdnIsUsed = (HiddenField)gr.FindControl("hdnIsUsed");
                        HiddenField hdnWageID = (HiddenField)gr.FindControl("hdnWageID");
                        TextBox TextBox = (TextBox)gr.FindControl("txtWage");

                        Int32 UserID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"]);
                        if (!IsWageRateIsUsed(UserID, Convert.ToInt32(hdnWageID.Value), out TicketID))
                        {
                            if (hdnWageID.Value != "0")
                            {
                                DataRow dr = dt.Select("Wage = " + Convert.ToInt32(hdnWageID.Value)).FirstOrDefault();
                                if (dr != null)
                                    dt.Rows.Remove(dr);

                                dr = dt.NewRow();
                                dr["Wage"] = 0;
                                dr["Reg"] = 0;
                                dr["OT"] = 0;
                                dr["DT"] = 0;
                                dr["TT"] = 0;
                                dr["NT"] = 0;
                                dr["CReg"] = 0;
                                dr["COT"] = 0;
                                dr["CDT"] = 0;
                                dr["CTT"] = 0;
                                dr["CNT"] = 0;
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            string str = "Wage category is used in Ticket #" + TicketID + "!";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "key1512", "noty({text: '" + str + "',dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                            return;
                        }

                    }
                }
            }
            ViewState["WageItems"] = dt;
            gvWagePayRate.DataSource = dt;

            gvWagePayRate.Rebind();


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private bool IsValidWageRateGrid()
    {
        try
        {
            DataTable dtWage = GetWageGridItems();
            var lst = dtWage.Rows.Cast<DataRow>().Where(r => (int)r.ItemArray[0] == 0).ToList();
            dtWage.Rows.Cast<DataRow>().Where(r => (int)r["Wage"] == 0).ToList().ForEach(r => r.Delete());
            dtWage.AcceptChanges();
            ViewState["WageItems"] = dtWage;
            DataView dv = new DataView(dtWage);
            DataTable distDt = dv.ToTable(true, "Wage");
            if (distDt.Rows.Count != dtWage.Rows.Count)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "keySuperAlert", "noty({text: 'Selected wage categories must be unique!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                return false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return true;
    }

    private bool IsWageRateIsUsed(Int32 userID, Int32 WageID, out string TicketID)
    {
        // used Wage category can't delete if the Job Cost Labor = Burden Rate

        string wagesRage = ViewState["JobCostLabor"].ToString();

        TicketID = "";
        if (userID == 0 || WageID == 0 || wagesRage != "1") return false;
        DataSet IsWageRateIsUsed = new DataSet();
        objWage.ConnConfig = HttpContext.Current.Session["config"].ToString();
        objWage.ID = WageID;
        IsWageRateIsUsed = objBL_User.IsWageRateIsUsed(objWage, userID);

        if (IsWageRateIsUsed.Tables[0].Rows.Count > 0)
        {
            TicketID = IsWageRateIsUsed.Tables[0].Rows[0]["ID"].ToString();
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void imgbtnMerchant_Click(object sender, EventArgs e)
    {
        if (ddlMerchantID.SelectedIndex > 1)
        {
            //TogglePopUp();
            this.programmaticModalPopup.Show();
            DataSet ds = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.MerchantID = ddlMerchantID.SelectedValue;
            ds = objBL_Contracts.getPaymentGatewayInfo(objProp_Contracts);
            if (ds.Tables[0].Rows.Count > 0)
            {
                imgbtnDelete.Visible = true;
                txtMerchantID.Enabled = false;
                txtMerchantID.Text = ds.Tables[0].Rows[0]["MerchantID"].ToString();
                txtLoginID.Text = ds.Tables[0].Rows[0]["LoginID"].ToString();
                txtMerchantUsername.Text = ds.Tables[0].Rows[0]["Username"].ToString();
                txtMerchantPassword.Text = AES_Algo.Decrypt(ds.Tables[0].Rows[0]["Password"].ToString(), "MSMPAY", "4Bvq75DG", "SHA1", 1000, "pOWaTbO92LfXbh69JkYzfT7P465TNc0h", 256).TrimEnd('\0');
                hdnMerchantInfoID.Value = ds.Tables[0].Rows[0]["id"].ToString();
            }
        }
    }

    protected void gvWagePayRate_ItemDataBound(object sender, GridItemEventArgs e)
    {
        string TicketID = string.Empty;


        if (e.Item is GridDataItem)
        {
            HiddenField hdnIsUsed = (HiddenField)e.Item.FindControl("hdnIsUsed");
            HiddenField hdnWageID = (HiddenField)e.Item.FindControl("hdnWageID");
            DropDownList ddlWageStatus = (DropDownList)e.Item.FindControl("ddlWageStatus");
            TextBox txtWage = (TextBox)e.Item.FindControl("txtWage");

            TextBox txtReg = (TextBox)e.Item.FindControl("txtReg");
            TextBox txtOt = (TextBox)e.Item.FindControl("txtOt");
            TextBox txtNt = (TextBox)e.Item.FindControl("txtNt");
            TextBox txtDt = (TextBox)e.Item.FindControl("txtDt");
            TextBox txtTt = (TextBox)e.Item.FindControl("txtTt");


            TextBox txtCReg = (TextBox)e.Item.FindControl("txtCReg");
            TextBox txtCOt = (TextBox)e.Item.FindControl("txtCOt");
            TextBox txtCNt = (TextBox)e.Item.FindControl("txtCNt");
            TextBox txtCDt = (TextBox)e.Item.FindControl("txtCDt");
            TextBox txtCTt = (TextBox)e.Item.FindControl("txtCTt");


            Int32 UserID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"]);

            if (IsWageRateIsUsed(UserID, Convert.ToInt32(hdnWageID.Value), out TicketID))
            {
                string str = "Wage category is used in Ticket #" + TicketID + "!";
                txtWage.Attributes["onclick"] = "   noty({ text: '" + str + "', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";
                hdnIsUsed.Value = "1";
            }
            else { hdnIsUsed.Value = "0"; }


            string STT = "True";

            if (!lnkChk.Checked)
            {
                STT = "False";
            }

            if (STT == "False" && ddlWageStatus.SelectedValue == "1")
            {
                e.Item.Display = false;
            }

            if (ddlWageStatus.SelectedValue == "1")
            {

                ddlWageStatus.Enabled = false;
                txtReg.ReadOnly =
                txtOt.ReadOnly =
                txtNt.ReadOnly =
                txtDt.ReadOnly =
                txtTt.ReadOnly =
                txtCReg.ReadOnly =
                txtCOt.ReadOnly =
                txtCNt.ReadOnly =
                txtCDt.ReadOnly =
                txtCTt.ReadOnly =
                txtWage.ReadOnly = true;
            }

        }
    }

    protected void gvWagePayRate_ItemCreated(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {


        }
    }

    protected void btnTestIncoming_Click(object sender, EventArgs e)
    {
        try
        {
            using (var client = new MailKit.Net.Imap.ImapClient())
            {
                if (client.IsConnected)
                    client.Disconnect(true);

                try
                {
                    if (chkSSL.Checked)
                    {
                        try
                        {
                            client.Connect(txtInServer.Text.Trim(), Convert.ToInt32(txtinPort.Text.Trim()), SecureSocketOptions.SslOnConnect);
                            //client.Connect(txtInServer.Text.Trim(), Convert.ToInt32(txtinPort.Text.Trim()), true);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (!client.IsConnected)
                    {
                        try
                        {
                            client.Connect(txtInServer.Text.Trim(), Convert.ToInt32(txtinPort.Text.Trim()), SecureSocketOptions.Auto);
                        }
                        catch
                        {
                            client.Connect(txtInServer.Text.Trim(), Convert.ToInt32(txtinPort.Text.Trim()), SecureSocketOptions.None);
                        }
                    }
                    //if (client.Connect(txtInServer.Text.Trim(), Convert.ToInt32(txtinPort.Text.Trim()), chkSSL.Checked, false))
                    if (client.IsConnected)
                    {
                        try
                        {
                            //client.AuthenticationMechanisms.Remove("NTLM");
                            //client.AuthenticationMechanisms.Remove("XOAUTH2");
                            client.Authenticate(txtInUSername.Text.Trim(), txtInPassword.Text.Trim());
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Connection Successful');", true);
                            client.Disconnect(true);
                        }
                        catch (Exception ex)
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Invalid Credentials');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Connection Failed');", true);
                    }
                }
                catch (Exception)
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Connection Failed');", true);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", "<br/>");
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    private void AddNewRow()
    {

        DataTable dt = GetWageGridItems();
        DataRow dr = dt.NewRow();
        dr["Wage"] = 0;
        dr["Reg"] = 0;
        dr["OT"] = 0;
        dr["DT"] = 0;
        dr["TT"] = 0;
        dr["NT"] = 0;
        dr["CReg"] = 0;
        dr["COT"] = 0;
        dr["CDT"] = 0;
        dr["CTT"] = 0;
        dr["CNT"] = 0;
        dr["Status"] = 0;
        dr["fDesc"] = "";
        dt.Rows.Add(dr);
        gvWagePayRate.DataSource = dt;
        gvWagePayRate.Rebind();
    }

    protected void gvWagePayRate_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        CreateWageTable();

        if (Request.QueryString["uid"] != null)
        {
            DataSet ds = new DataSet();
            ds = (DataSet)ViewState["getUserById"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(ViewState["empid"].ToString()))
                {
                    objPropUser.ConnConfig = Session["config"].ToString();
                    objPropUser.EmpId = Convert.ToInt32(ViewState["empid"].ToString());

                    DataSet dsWage = objBL_User.GetEmpWageItems(objPropUser);

                    ViewState["dsWage"] = dsWage;

                    if (dsWage.Tables[0].Rows.Count > 0)
                    {

                        DataTable filterdt = new DataTable();


                        filterdt = dsWage.Tables[0];

                        gvWagePayRate.VirtualItemCount = filterdt.Rows.Count;

                        gvWagePayRate.DataSource = filterdt;

                        ViewState["WageItems"] = filterdt;


                    }
                    else
                    {
                        CreateWageTable();
                    }
                }
            }
        }
    }

    protected void imgBtnWageAdd_Click(object sender, EventArgs e)
    {
        AddNewRow();
    }

    protected void gvUsers_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        if (hfCheckEdit.Value == "true")
        {
            GetUsers();

        }
        else
        {
            GetUserunderSuper();

        }
    }

    protected void gvUsers_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataTable dtSupersUsers = new DataTable();
        dtSupersUsers = (DataTable)ViewState["superusers"];
        //dsSupersUsers=objBL_User.getUserForSupervisor(objPropUser);

        foreach (GridDataItem gr in gvUsers.Items)
        {
            CheckBox chkSelected = (CheckBox)gr.FindControl("chkSelect");
            Label lblUserID = (Label)gr.FindControl("lblId");
            if (dtSupersUsers != null)
            {
                foreach (DataRow dr in dtSupersUsers.Rows)
                {
                    if (lblUserID.Text == dr["userid"].ToString())
                    {
                        chkSelected.Checked = true;
                    }
                }
            }
        }
    }

    protected void chkMSAuthorisedDeviceOnly_CheckedChanged(object sender, EventArgs e)
    {
        if (txtAuthdevID.Enabled)
        {
            txtAuthdevID.Enabled = false;
            //  RequiredFieldValidator12.Enabled = false;

        }
        else
        {
            txtAuthdevID.Enabled = true;
            // RequiredFieldValidator12.Enabled = true;
        }
        txtAuthdevID.Text = string.Empty;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkFinancialmodule_CheckedChanged(object sender, EventArgs e)
    {
        chkChartAdd.Checked = chkChartEdit.Checked = chkChartDelete.Checked = chkChartView.Checked =
        chkJournalEntryAdd.Checked = chkJournalEntryEdit.Checked = chkJournalEntryDelete.Checked = chkJournalEntryView.Checked =
        chkFinanceMgr.Checked = chkBankAdd.Checked = chkBankEdit.Checked = chkBankDelete.Checked = chkBankView.Checked = chkFinanceStatement.Checked =
        chkFinancialmodule.Checked;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkChartView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkChartView.Checked == false)
            chkChartAdd.Checked = chkChartEdit.Checked = chkChartDelete.Checked = chkChartView.Checked;
        if (chkFinancialmodule.Checked == false && chkChartView.Checked == true)
            chkFinancialmodule.Checked = true;

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkJournalEntryView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkJournalEntryView.Checked == false)
            chkJournalEntryAdd.Checked = chkJournalEntryEdit.Checked = chkJournalEntryDelete.Checked = chkJournalEntryView.Checked;
        if (chkFinancialmodule.Checked == false && chkJournalEntryView.Checked == true)
            chkFinancialmodule.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkBankView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkBankView.Checked == false)
            chkBankAdd.Checked = chkBankEdit.Checked = chkBankDelete.Checked = chkBankView.Checked;
        if (chkFinancialmodule.Checked == false && chkBankView.Checked == true)
            chkFinancialmodule.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkAccountPayable_CheckedChanged(object sender, EventArgs e)
    {
        chkVendorsAdd.Checked = chkVendorsEdit.Checked = chkVendorsDelete.Checked = chkVendorsView.Checked =
            chkAddBills.Checked = chkEditBills.Checked = chkDeleteBills.Checked = chkViewBills.Checked =
            chkAddManageChecks.Checked = chkEditManageChecks.Checked = chkDeleteManageChecks.Checked =
            chkViewManageChecks.Checked = chkShowBankBalances.Checked = chkAccountPayable.Checked;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkPurchasingmodule_CheckedChanged(object sender, EventArgs e)
    {
        chkPOAdd.Checked = chkPOEdit.Checked = chkPODelete.Checked = chkPOView.Checked =
        chkRPOAdd.Checked = chkRPOEdit.Checked = chkRPODelete.Checked = chkRPOView.Checked = chkPONotification.Checked =
        chkPurchasingmodule.Checked;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkPOView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkPOView.Checked == false)
            chkPOAdd.Checked = chkPOEdit.Checked = chkPODelete.Checked = chkPOView.Checked;
        if (chkPurchasingmodule.Checked == false && chkPOView.Checked == true)
            chkPurchasingmodule.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkRPOView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRPOView.Checked == false)
            chkRPOAdd.Checked = chkRPOEdit.Checked = chkRPODelete.Checked = chkRPOView.Checked;
        if (chkPurchasingmodule.Checked == false && chkRPOView.Checked == true)
            chkPurchasingmodule.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkCustomerModule_CheckedChanged(object sender, EventArgs e)
    {
        chkCustomeradd.Checked = chkCustomeredit.Checked = chkCustomerdelete.Checked = chkCustomerview.Checked =
        chkLocationadd.Checked = chkLocationedit.Checked = chkLocationdelete.Checked = chkLocationview.Checked =
        chkReceivePaymentAdd.Checked = chkReceivePaymentEdit.Checked = chkReceivePaymentDelete.Checked = chkReceivePaymentView.Checked =
        //chkOnlinePaymentView.Checked = chkOnlinePaymentApprove.Checked =
        chkMakeDepositAdd.Checked = chkMakeDepositEdit.Checked = chkMakeDepositDelete.Checked = chkMakeDepositView.Checked =
        chkCollectionsAdd.Checked = chkCollectionsEdit.Checked = chkCollectionsDelete.Checked = chkCollectionsReport.Checked = chkCollectionsView.Checked =
        chkEquipmentsadd.Checked = chkEquipmentsedit.Checked = chkEquipmentsdelete.Checked = chkEquipmentsview.Checked =
        chkCreditHold.Checked = chkCreditFlag.Checked = chkWriteOff.Checked = chkCustomerModule.Checked;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkCustomerview_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCustomerview.Checked == false)
            chkCustomeradd.Checked = chkCustomeredit.Checked = chkCustomerdelete.Checked = chkCustomerview.Checked;
        if (chkCustomerModule.Checked == false && chkCustomerview.Checked == true)
            chkCustomerModule.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkLocationview_CheckedChanged(object sender, EventArgs e)
    {
        if (chkLocationview.Checked == false)
            chkLocationadd.Checked = chkLocationedit.Checked = chkLocationdelete.Checked = chkLocationview.Checked;
        if (chkCustomerModule.Checked == false && chkLocationview.Checked == true)
            chkCustomerModule.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkEquipmentsview_CheckedChanged(object sender, EventArgs e)
    {
        if (chkEquipmentsview.Checked == false)
            chkEquipmentsadd.Checked = chkEquipmentsedit.Checked = chkEquipmentsdelete.Checked = chkEquipmentsview.Checked;
        if (chkCustomerModule.Checked == false && chkEquipmentsview.Checked == true)
            chkCustomerModule.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkCreditHold_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCustomerModule.Checked == false && chkCreditHold.Checked == true)
            chkCustomerModule.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void chkCreditFlag_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCustomerModule.Checked == false && chkCreditFlag.Checked == true)
            chkCustomerModule.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void chkReceivePaymentView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkReceivePaymentView.Checked == false)
            chkReceivePaymentAdd.Checked = chkReceivePaymentEdit.Checked = chkReceivePaymentDelete.Checked = chkReceivePaymentView.Checked;
        if (chkCustomerModule.Checked == false && chkReceivePaymentView.Checked == true)
            chkCustomerModule.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    //protected void chkOnlinePaymentView_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (chkOnlinePaymentView.Checked == false)
    //        chkOnlinePaymentApprove.Checked = chkOnlinePaymentView.Checked;
    //    if (chkCustomerModule.Checked == false && chkOnlinePaymentView.Checked == true)
    //        chkCustomerModule.Checked = true;
    //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    //}

    protected void chkMakeDepositView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkMakeDepositView.Checked == false)
            chkMakeDepositAdd.Checked = chkMakeDepositEdit.Checked = chkMakeDepositDelete.Checked = chkMakeDepositView.Checked;
        if (chkCustomerModule.Checked == false && chkMakeDepositView.Checked == true)
            chkCustomerModule.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void chkCollectionsView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCollectionsView.Checked == false)
            chkCollectionsAdd.Checked = chkCollectionsEdit.Checked = chkCollectionsDelete.Checked = chkCollectionsReport.Checked = chkCollectionsView.Checked;
        if (chkCustomerModule.Checked == false && chkCollectionsView.Checked == true)
            chkCustomerModule.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkVendorsView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkVendorsView.Checked == false)
            chkVendorsAdd.Checked = chkVendorsEdit.Checked = chkVendorsDelete.Checked = chkVendorsView.Checked;
        if (chkAccountPayable.Checked == false && chkVendorsView.Checked == true)
            chkAccountPayable.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkViewBills_CheckedChanged(object sender, EventArgs e)
    {
        if (chkViewBills.Checked == false)
            chkAddBills.Checked = chkEditBills.Checked = chkDeleteBills.Checked = chkViewBills.Checked = chkViewBills.Checked;
        if (chkAccountPayable.Checked == false && chkViewBills.Checked == true)
            chkAccountPayable.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkViewManageChecks_CheckedChanged(object sender, EventArgs e)
    {
        if (chkViewManageChecks.Checked == false)
            chkAddManageChecks.Checked = chkEditManageChecks.Checked = chkDeleteManageChecks.Checked = chkShowBankBalances.Checked = chkViewManageChecks.Checked;
        if (chkAccountPayable.Checked == false && chkViewManageChecks.Checked == true)
            chkAccountPayable.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkBillingmodule_CheckedChanged(object sender, EventArgs e)
    {
        chkInvoicesAdd.Checked = chkInvoicesEdit.Checked = chkInvoicesDelete.Checked = chkInvoicesView.Checked =
        chkPaymentHistoryView.Checked =
         chkBillingcodesAdd.Checked = chkBillingcodesEdit.Checked = chkBillingcodesDelete.Checked = chkBillingcodesView.Checked = chkBillingmodule.Checked;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkInvoicesView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkInvoicesView.Checked == false)
            chkInvoicesAdd.Checked = chkInvoicesEdit.Checked = chkInvoicesDelete.Checked = chkInvoicesView.Checked = chkInvoicesView.Checked;
        if (chkBillingmodule.Checked == false && chkInvoicesView.Checked == true)
            chkBillingmodule.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkPaymentHistoryView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkPaymentHistoryView.Checked == false)
            chkPaymentHistoryAdd.Checked = chkPaymentHistoryEdit.Checked = chkPaymentHistoryDelete.Checked = chkPaymentHistoryView.Checked = chkPaymentHistoryView.Checked;
        if (chkBillingmodule.Checked == false && chkPaymentHistoryView.Checked == true)
            chkBillingmodule.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkBillingcodesView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkBillingcodesView.Checked == false)
            chkBillingcodesAdd.Checked = chkBillingcodesEdit.Checked = chkBillingcodesDelete.Checked = chkBillingcodesView.Checked = chkBillingcodesView.Checked;
        if (chkBillingmodule.Checked == false && chkBillingcodesView.Checked == true)
            chkBillingmodule.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    //payroll check/uncheck
    protected void chkempView_CheckedChanged(object sender, EventArgs e)
    {
        if (empView.Checked == false)
            empAdd.Checked = empEdit.Checked = empDelete.Checked = empView.Checked = empView.Checked;
        if (payrollModulchck.Checked == false && empView.Checked == true)
            payrollModulchck.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkrunpayrollView_CheckedChanged(object sender, EventArgs e)
    {
        if (runView.Checked == false)
            runAdd.Checked = runEdit.Checked = runDelete.Checked = runView.Checked = runView.Checked;
        if (payrollModulchck.Checked == false && runView.Checked == true)
            payrollModulchck.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkpayrollcheckView_CheckedChanged(object sender, EventArgs e)
    {
        if (payrollchckView.Checked == false)
            payrollchckAdd.Checked = payrollchckEdit.Checked = payrollchckDelete.Checked = payrollchckView.Checked = payrollchckView.Checked;
        if (payrollModulchck.Checked == false && payrollchckView.Checked == true)
            payrollModulchck.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkpayrollformView_CheckedChanged(object sender, EventArgs e)
    {
        if (payrollformView.Checked == false)
            payrollformAdd.Checked = payrollformEdit.Checked = payrollformDelete.Checked = payrollformView.Checked = payrollformView.Checked;
        if (payrollModulchck.Checked == false && payrollformView.Checked == true)
            payrollModulchck.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkwagesView_CheckedChanged(object sender, EventArgs e)
    {
        if (wagesView.Checked == false)
            wagesadd.Checked = wagesEdit.Checked = wagesDelete.Checked = wagesView.Checked = wagesView.Checked;
        if (payrollModulchck.Checked == false && wagesView.Checked == true)
            payrollModulchck.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkdeductionView_CheckedChanged(object sender, EventArgs e)
    {
        if (deductionsView.Checked == false)
            deductionsAdd.Checked = deductionsEdit.Checked = deductionsDelete.Checked = deductionsView.Checked = deductionsView.Checked;
        if (payrollModulchck.Checked == false && deductionsView.Checked == true)
            payrollModulchck.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void gvUsers_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void gvUsers_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void txtEmail_TextChanged(object sender, EventArgs e)
    {
        // TODO: Thomas, need to check and update in case txtEmail changing
        if (!(bool)ViewState["IsSetEmailAccount"])
        {
            var strEmail = txtEmail.Text.Trim();
            txtInUSername.Text = strEmail;
            txtBccEmail.Text = strEmail;
            if (chkSame.Checked)
            {
                txtOutUsername.Text = strEmail;
            }
        }

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void txtInPassword_PreRender(object sender, EventArgs e)
    {
        txtInPassword.Attributes["value"] = txtInPassword.Text;
    }

    protected void txtOutPassword_PreRender(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtOutPassword.Text))
            txtOutPassword.Attributes["value"] = txtOutPassword.Text;
    }

    protected void chkRecurring_CheckedChanged(object sender, EventArgs e)
    {
        chkRecContractsAdd.Checked = chkRecContractsEdit.Checked = chkRecContractsDelete.Checked = chkRecContractsView.Checked =
        chkRecInvoicesAdd.Checked = chkRecInvoicesDelete.Checked = chkRecInvoicesView.Checked =
        chkRecTicketsAdd.Checked = chkRecTicketsDelete.Checked = chkRecTicketsView.Checked =
         chkSafetyTestsAdd.Checked = chkSafetyTestsEdit.Checked = chkSafetyTestsDelete.Checked = chkSafetyTestsView.Checked =
           chkRenewEscalateAdd.Checked = chkRenewEscalateView.Checked = chkRecurring.Checked;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void chkRecContractsView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRecContractsView.Checked == false)
            chkRecContractsAdd.Checked = chkRecContractsEdit.Checked = chkRecContractsDelete.Checked = chkRecContractsView.Checked = chkRecContractsView.Checked;
        if (chkRecurring.Checked == false && chkRecContractsView.Checked == true)
            chkRecurring.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void chkRecInvoicesView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRecInvoicesView.Checked == false)
            chkRecInvoicesAdd.Checked = chkRecInvoicesDelete.Checked = chkRecInvoicesView.Checked = chkRecInvoicesView.Checked;
        if (chkRecurring.Checked == false && chkRecInvoicesView.Checked == true)
            chkRecurring.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void chkRecTicketsView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRecTicketsView.Checked == false)
            chkRecTicketsAdd.Checked = chkRecTicketsDelete.Checked = chkRecTicketsView.Checked = chkRecTicketsView.Checked;
        if (chkRecurring.Checked == false && chkRecTicketsView.Checked == true)
            chkRecurring.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void chkSafetyTestsView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSafetyTestsView.Checked == false)
            chkSafetyTestsAdd.Checked = chkSafetyTestsEdit.Checked = chkSafetyTestsDelete.Checked = chkSafetyTestsView.Checked = chkSafetyTestsView.Checked;
        if (chkRecurring.Checked == false && chkSafetyTestsView.Checked == true)
            chkRecurring.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void chkRenewEscalateView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRenewEscalateView.Checked == false)
            chkRenewEscalateAdd.Checked = chkRenewEscalateView.Checked = chkRenewEscalateView.Checked;
        if (chkRecurring.Checked == false && chkRenewEscalateView.Checked == true)
            chkRecurring.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void chkSchedule_CheckedChanged(object sender, EventArgs e)
    {
        chkScheduleBoard.Checked =
          chkTimesheetadd.Checked = chkTimesheetedit.Checked = chkTimesheetdelete.Checked = chkTimesheetview.Checked = chkTimesheetview.Checked = chkETimesheetreport.Checked =
          chkMapAdd.Checked = chkMapEdit.Checked = chkMapDelete.Checked = chkMapView.Checked = chkMapView.Checked = chkMapReport.Checked =
             chkRouteBuilderAdd.Checked = chkRouteBuilderEdit.Checked = chkRouteBuilderDelete.Checked = chkRouteBuilderView.Checked = chkRouteBuilderView.Checked = chkRouteBuilderReport.Checked =
             chkMassReview.Checked = chkMassTimesheetCheck.Checked = chkETimesheetview.Checked = chkTimestampFix.Checked = chkResolveTicketView.Checked =
             chkResolveTicketAdd.Checked = chkResolveTicketEdit.Checked = chkResolveTicketDelete.Checked = chkResolveTicketReport.Checked = chkResolveTicketView.Checked =
            chkTicketListAdd.Checked = chkTicketListEdit.Checked = chkTicketListDelete.Checked = chkTicketListView.Checked = chkTicketListReport.Checked = chkTicketListView.Checked = chkSchedule.Checked;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void chkTimesheetview_CheckedChanged(object sender, EventArgs e)
    {
        if (chkTimesheetview.Checked == false)
            chkTimesheetadd.Checked = chkTimesheetedit.Checked = chkTimesheetdelete.Checked = chkTimesheetreport.Checked = chkTimesheetview.Checked = chkTimesheetview.Checked;
        if (chkSchedule.Checked == false && chkTimesheetview.Checked == true)
            chkSchedule.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkETimesheetview_CheckedChanged(object sender, EventArgs e)
    {
        if (chkETimesheetview.Checked == false)
            chkETimesheetadd.Checked = chkETimesheetedit.Checked = chkETimesheetdelete.Checked = chkETimesheetview.Checked = chkETimesheetreport.Checked = chkETimesheetview.Checked;
        if (chkSchedule.Checked == false && chkETimesheetview.Checked == true)
            chkSchedule.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void chkMapView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkMapView.Checked == false)
            chkMapAdd.Checked = chkMapEdit.Checked = chkMapDelete.Checked = chkMapView.Checked = chkMapReport.Checked = chkMapView.Checked;
        if (chkSchedule.Checked == false && chkMapView.Checked == true)
            chkSchedule.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkRouteBuilderView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRouteBuilderView.Checked == false)
            chkRouteBuilderAdd.Checked = chkRouteBuilderEdit.Checked = chkRouteBuilderDelete.Checked = chkRouteBuilderView.Checked = chkRouteBuilderView.Checked;
        if (chkSchedule.Checked == false && chkRouteBuilderView.Checked == true)
            chkSchedule.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkMassReview_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSchedule.Checked == false && chkMassReview.Checked == true)
            chkSchedule.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkMassTimesheetCheck_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSchedule.Checked == false && chkMassTimesheetCheck.Checked == true)
            chkSchedule.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkTimestampFix_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSchedule.Checked == false && chkTimestampFix.Checked == true)
            chkSchedule.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkTicketListView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkTicketListView.Checked == false)
            chkTicketListAdd.Checked = chkTicketListEdit.Checked = chkTicketListDelete.Checked = chkTicketListReport.Checked = chkTicketListView.Checked = chkResolveTicketReport.Checked = chkTicketListView.Checked;
        if (chkSchedule.Checked == false && chkTicketListView.Checked == true)
            chkSchedule.Checked = true;
        chkScheduleBrd.Checked = chkTicketListView.Checked;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void chkResolveTicketView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkResolveTicketView.Checked == false)
            chkResolveTicketAdd.Checked = chkResolveTicketEdit.Checked = chkResolveTicketDelete.Checked = chkResolveTicketView.Checked = chkResolveTicketView.Checked;
        if (chkSchedule.Checked == false && chkResolveTicketView.Checked == true)
            chkSchedule.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkSalesMgr_CheckedChanged(object sender, EventArgs e)
    {
        chkLeadAdd.Checked = chkLeadEdit.Checked = chkLeadDelete.Checked = chkLeadReport.Checked = chkLeadView.Checked =
        chkTasks.Checked = chkCompleteTask.Checked = chkFollowUp.Checked =
        chkOppAdd.Checked = chkOppEdit.Checked = chkOppDelete.Checked = chkOppView.Checked = chkOppReport.Checked =
           chkEstimateAdd.Checked = chkEstimateEdit.Checked = chkEstimateDelete.Checked = chkEstimateView.Checked = chkEstimateReport.Checked =
           chkConvertEstimate.Checked = chkSalesSetup.Checked = chkSalesMgr.Checked;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void chkPayrollMgr_CheckedChanged(object sender, EventArgs e)
    {
        empAdd.Checked = empEdit.Checked = empDelete.Checked = empView.Checked =
            runAdd.Checked = runEdit.Checked = runDelete.Checked = runView.Checked =
        payrollchckAdd.Checked = payrollchckEdit.Checked = payrollchckDelete.Checked = payrollchckView.Checked =
        payrollformAdd.Checked = payrollformEdit.Checked = payrollformDelete.Checked = payrollformView.Checked =
        wagesadd.Checked = wagesEdit.Checked = wagesDelete.Checked = wagesView.Checked = deductionsAdd.Checked = deductionsEdit.Checked
        = deductionsDelete.Checked = deductionsView.Checked = chkMassPayrollTicket1.Checked = payrollModulchck.Checked;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void chkLeadView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSalesMgr.Checked == false && chkLeadView.Checked == true)
            chkSalesMgr.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkTasks_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSalesMgr.Checked == false && chkTasks.Checked == true)
            chkSalesMgr.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkCompleteTask_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSalesMgr.Checked == false && chkCompleteTask.Checked == true)
            chkSalesMgr.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkOppView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSalesMgr.Checked == false && chkOppView.Checked == true)
            chkSalesMgr.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkEstimateView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSalesMgr.Checked == false && chkEstimateView.Checked == true)
            chkSalesMgr.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkSalesSetup_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSalesMgr.Checked == false && chkSalesSetup.Checked == true)
            chkSalesMgr.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void chkFollowUp_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSalesMgr.Checked == false && chkFollowUp.Checked == true)
            chkSalesMgr.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
 
    protected void chkConvertEstimate_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSalesMgr.Checked == false && chkConvertEstimate.Checked == true)
            chkSalesMgr.Checked = true;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void chkPONotification_CheckedChanged(object sender, EventArgs e)
    {

    }

    protected void chkWriteOff_CheckedChanged(object sender, EventArgs e)
    {

    }

    protected void btnCopyRate_Click(object sender, EventArgs e)
    {
        try
        {
            string TicketID = string.Empty;
            DataTable dt = GetWageGridItems();


            if (dt.Rows.Count > 1)
            {
                DataRow dr = dt.Select("Checked = true").FirstOrDefault();
                foreach (DataRow row in dt.Rows)
                {
                    row["Reg"] = dr["Reg"];
                    row["OT"] = dr["OT"];
                    row["DT"] = dr["DT"];
                    row["TT"] = dr["TT"];
                    row["NT"] = dr["NT"];
                    row["CReg"] = dr["CReg"];
                    row["COT"] = dr["COT"];
                    row["CDT"] = dr["CDT"];
                    row["CTT"] = dr["CTT"];
                    row["CNT"] = dr["CNT"];
                }
            }

            ViewState["WageItems"] = dt;
            gvWagePayRate.DataSource = dt;
            gvWagePayRate.Rebind();

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkLoadLogs_Click(object sender, EventArgs e)
    {
        hdnLoadLogs.Value = "1";
        RadGrid_gvLogs.Rebind();
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void RadGrid_gvLogs_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridPagerItem)
        {
            var dropDown = (RadComboBox)e.Item.FindControl("PageSizeComboBox");
            var totalCount = ((GridPagerItem)e.Item).Paging.DataSourceCount;
            if (totalCount == 0) totalCount = 1000;

            GeneralFunctions obj = new GeneralFunctions();

            var sizes = obj.TelerikPageSize(totalCount);

            dropDown.Items.Clear();

            foreach (var size in sizes)
            {
                var cboItem = new RadComboBoxItem() { Text = size.Key, Value = size.Value };
                cboItem.Attributes.Add("ownerTableViewId", e.Item.OwnerTableView.ClientID);
                if (e.Item.OwnerTableView.PageSize.ToString() == size.Value) cboItem.Selected = true;
                dropDown.Items.Add(cboItem);
            }
        }
    }

    protected void RadGrid_gvLogs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindLogs();
        }
        catch { }
    }

    private void BindLogs()
    {
        if (Request.QueryString["uid"] != null && hdnLoadLogs.Value == "1")
        {
            DataSet dsLog = new DataSet();
            Customer objProp_Customer = new Customer();
            BL_Customer objBL_Customer = new BL_Customer();
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.LogRefId = Convert.ToInt32(Request.QueryString["uid"]);
            objProp_Customer.LogScreen = "User";
            dsLog = objBL_Customer.GetLogs(objProp_Customer);
            if (dsLog.Tables[0].Rows.Count > 0)
            {
                RadGrid_gvLogs.VirtualItemCount = dsLog.Tables[0].Rows.Count;
                RadGrid_gvLogs.DataSource = dsLog.Tables[0];
            }
            else
            {
                RadGrid_gvLogs.DataSource = string.Empty;
            }
        }
        else
        {
            RadGrid_gvLogs.DataSource = string.Empty;
        }
    }

    protected void chkInventorymodule_CheckedChanged(object sender, EventArgs e)
    {
        chkInventoryItemadd.Checked = chkInventoryItemdelete.Checked = chkInventoryItemedit.Checked = chkInventoryItemview.Checked =
            chkInventoryAdjustmentadd.Checked = chkInventoryAdjustmentdelete.Checked = chkInventoryAdjustmentedit.Checked = chkInventoryAdjustmentview.Checked =
            chkInventoryWareHouseadd.Checked = chkInventoryWareHousedelete.Checked = chkInventoryWareHouseedit.Checked = chkInventoryWareHouseview.Checked =
            chkInventorysetupadd.Checked = chkInventorysetupdelete.Checked = chkInventorysetupedit.Checked = chkInventorysetupview.Checked =
            chkInventoryFinanceAdd.Checked = chkInventoryFinancedelete.Checked = chkInventoryFinanceedit.Checked = chkInventoryFinanceview.Checked =
            chkInventorymodule.Checked;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void chkProjectmodule_CheckedChanged(object sender, EventArgs e)
    {
        chkProjectadd.Checked = chkProjectDelete.Checked = chkProjectEdit.Checked = chkProjectView.Checked =
            chkProjectTempAdd.Checked = chkProjectTempDelete.Checked = chkProjectTempEdit.Checked = chkProjectTempView.Checked =
            chkAddBOM.Checked = chkDeleteBOM.Checked = chkEditBOM.Checked = chkViewBOM.Checked =
            chkAddWIP.Checked = chkDeleteWIP.Checked = chkEditWIP.Checked = chkViewWIP.Checked = chkReportWIP.Checked =
            chkAddMilesStones.Checked = chkDeleteMilesStones.Checked = chkEditMilesStones.Checked = chkViewMilesStones.Checked =
            chkJobClosePermission.Checked = chkJobCompletedPermission.Checked = chkJobReopenPermission.Checked =
            chkViewProjectList.Checked = chkViewFinance.Checked = chkProjectmodule.Checked;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void chkProgram_CheckedChanged(object sender, EventArgs e)
    {
        chkEmpMainten.Checked = chkExpenses.Checked = chkAccessUser.Checked = chkDispatch.Checked = chkProgram.Checked;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    private bool CheckPasswordPolicy(string userType, string password, string userAccount, string firstName, string lastName, ref string errorMessage)
    {
        DataSet ds = new DataSet();
        User objProp_User = new User();
        //String errorMessage = string.Empty;
        objProp_User.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getControl(objProp_User);
        //var isApplyPwPolicy = dsControl.Tables[0].Rows[0]["ApplyPasswordRules"].ToString();
        var isApplyPwPolicy = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ApplyPasswordRules"].ToString()) ? false : Convert.ToBoolean(ds.Tables[0].Rows[0]["ApplyPasswordRules"].ToString());
        if (!isApplyPwPolicy) return true;
        else
        {

            var applyFieldUser = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ApplyPwRulesToFieldUser"].ToString()) ? false : Convert.ToBoolean(ds.Tables[0].Rows[0]["ApplyPwRulesToFieldUser"].ToString());
            var applyOfficeUsers = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ApplyPwRulesToOfficeUser"].ToString()) ? false : Convert.ToBoolean(ds.Tables[0].Rows[0]["ApplyPwRulesToOfficeUser"].ToString());
            var applyCustomerUsers = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ApplyPwRulesToCustomerUser"].ToString()) ? false : Convert.ToBoolean(ds.Tables[0].Rows[0]["ApplyPwRulesToCustomerUser"].ToString());
            //var applyPwResetDays = string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ApplyPwReset"].ToString()) ? false : Convert.ToBoolean(ds.Tables[0].Rows[0]["ApplyPwReset"].ToString());
            //var pwResetDays = ds.Tables[0].Rows[0]["PwResetDays"].ToString();
            //if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PwResetting"].ToString())) ddlPasswordResetOption.SelectedIndex = 0;
            //else ddlPasswordResetOption.SelectedValue = ds.Tables[0].Rows[0]["PwResetting"].ToString();
            //txtAdminEmail.Text = ds.Tables[0].Rows[0]["EmailAdministrator"].ToString();
            switch (userType)
            {
                case "0": //office user
                    if (!applyOfficeUsers) return true;
                    break;
                case "1": // field user
                    if (!applyFieldUser) return true;
                    break;
                case "2": // customer type user
                    if (!applyCustomerUsers) return true;
                    break;
            }

            return WebBaseUtility.IsPasswordPassedPwPolicy(userAccount, firstName, lastName, password, ref errorMessage);
        }
    }

    protected void ddlUserRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ViewState["mode"] != null && ViewState["mode"].ToString() != "1")
        {
            if (ddlUserRole.SelectedValue != "0")
            {
                // Get role permission and fill for user permission
                UserRole userRole = new UserRole();
                userRole.ConnConfig = Session["config"].ToString();
                userRole.RoleID = Convert.ToInt32(ddlUserRole.SelectedValue);
                var ds = objBL_User.GetRoleByID(userRole);
                FillRolePermission(ds.Tables[0]);
                //divApplyUserRolePermission.Style["display"] = "inline-block";
                //divApplyUserRolePermission.Visible = true;
            }
            else
            {
                //divApplyUserRolePermission.Style["display"] = "none";
                //divApplyUserRolePermission.Visible = false;
            }
        }
        else
        {
            //if (ddlUserRole.SelectedValue != "0")
            //{
            //    divApplyUserRolePermission.Visible = true;
            //}
            //else
            //{
            //    divApplyUserRolePermission.Visible = false;
            //}
            if (ddlUserRole.SelectedValue != "0")
            {
                if (hdnApplyUserRolePermissionOrg.Value == ddlApplyUserRolePermission.SelectedValue)
                {
                    if (ddlApplyUserRolePermission.SelectedValue == "2")
                    {
                        // Get role permission and fill for user permission
                        UserRole userRole = new UserRole();
                        userRole.ConnConfig = Session["config"].ToString();
                        userRole.RoleID = Convert.ToInt32(ddlUserRole.SelectedValue);
                        var ds = objBL_User.GetRoleByID(userRole);
                        FillRolePermission(ds.Tables[0]);
                    }
                    else if (ddlApplyUserRolePermission.SelectedValue == "1")
                    {
                        UserRole userRole = new UserRole();
                        userRole.ConnConfig = Session["config"].ToString();
                        userRole.RoleID = Convert.ToInt32(ddlUserRole.SelectedValue);
                        var ds = objBL_User.GetRoleByID(userRole);
                        DataSet dsUser = new DataSet();
                        //dsUser = //(DataSet)ViewState["getUserById"];
                        User objPropUser = new User();
                        objPropUser.ConnConfig = Session["config"].ToString();
                        objPropUser.UserID = Convert.ToInt32(Request.QueryString["uid"]);
                        objPropUser.TypeID = Convert.ToInt32(Request.QueryString["type"]);
                        objPropUser.DBName = Session["dbname"].ToString();
                        dsUser = objBL_User.GetUserInfoByID(objPropUser);
                        if (dsUser != null && dsUser.Tables.Count > 0 && ds.Tables.Count > 0)
                        {
                            DataTable mergedDt = MergingPermissions(dsUser.Tables[0], ds.Tables[0]);
                            FillRolePermission(mergedDt);
                        }
                    }
                    else
                    {
                        DataSet dsUser = new DataSet();
                        //dsUser = //(DataSet)ViewState["getUserById"];
                        User objPropUser = new User();
                        objPropUser.ConnConfig = Session["config"].ToString();
                        objPropUser.UserID = Convert.ToInt32(Request.QueryString["uid"]);
                        objPropUser.TypeID = Convert.ToInt32(Request.QueryString["type"]);
                        objPropUser.DBName = Session["dbname"].ToString();
                        dsUser = objBL_User.GetUserInfoByID(objPropUser);
                        FillRolePermission(dsUser.Tables[0]);
                    }
                }
                //divApplyUserRolePermission.Visible = true;
                //divApplyUserRolePermission.Style["display"] = "inline-block";
            }
            else
            {
                //divApplyUserRolePermission.Style["display"] = "none";
                //divApplyUserRolePermission.Visible = false;
                DataSet dsUser = new DataSet();
                //dsUser = //(DataSet)ViewState["getUserById"];
                User objPropUser = new User();
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.UserID = Convert.ToInt32(Request.QueryString["uid"]);
                objPropUser.TypeID = Convert.ToInt32(Request.QueryString["type"]);
                objPropUser.DBName = Session["dbname"].ToString();
                dsUser = objBL_User.GetUserInfoByID(objPropUser);
                FillRolePermission(dsUser.Tables[0]);
            }
        }

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    private void FillUserRole()
    {
        DataSet ds = new DataSet();
        UserRole userRole = new UserRole();
        userRole.ConnConfig = Session["config"].ToString();
        userRole.SearchBy = "";
        userRole.SearchValue = "";

        ds = objBL_User.GetRoleSearch(userRole, false);

        ddlUserRole.DataSource = ds.Tables[0];
        ddlUserRole.DataTextField = "RoleName";
        ddlUserRole.DataValueField = "Id";
        ddlUserRole.DataBind();
        ddlUserRole.Items.Insert(0, new ListItem("Select", "0"));
    }

    private void FillRolePermission(DataTable roleInfo)
    {
        if (roleInfo.Rows.Count > 0)
        {
            string map = roleInfo.Rows[0]["ticket"].ToString().Substring(3, 1);
            string sch = roleInfo.Rows[0]["ticket"].ToString().Substring(0, 1);

            if (map == "Y")
            {
                chkMap.Checked = true;
            }

            if (roleInfo.Rows[0]["sales"].ToString() == "1")
            {
                chkSalesperson.Checked = true;
                chkNotification.Enabled = chkSalesAssigned.Enabled = true;

                chkSalesAssigned.Checked = roleInfo.Rows[0]["SalesAssigned"] == null ? false : Convert.ToBoolean(roleInfo.Rows[0]["SalesAssigned"]);
                chkNotification.Checked = roleInfo.Rows[0]["NotificationOnAddOpportunity"] == null ? false : Convert.ToBoolean(roleInfo.Rows[0]["NotificationOnAddOpportunity"]);
            }
            else
            {
                chkNotification.Checked = chkSalesAssigned.Checked = false;
            }
        }

        txtPOLimit.Text = roleInfo.Rows[0]["POLimit"].ToString();
        ddlPOApprove.SelectedValue = roleInfo.Rows[0]["POApprove"].ToString();
        ddlPOApproveAmt.SelectedValue = roleInfo.Rows[0]["POApproveAmt"].ToString();
        txtMinAmount.Text = roleInfo.Rows[0]["MinAmount"].ToString();
        txtMaxAmount.Text = roleInfo.Rows[0]["MaxAmount"].ToString();

        chkMassReview.Checked = Convert.ToBoolean(roleInfo.Rows[0]["massreview"]);
        string LocnRemark = roleInfo.Rows[0]["Location"].ToString().Substring(3, 1);

        string PurcOrders = roleInfo.Rows[0]["PO"].ToString().Substring(0, 1);
        string Exp = roleInfo.Rows[0]["PO"].ToString().Substring(1, 1);
        string ProgFunc = string.Empty;
        string AccessUser = string.Empty;

        string Sales = roleInfo.Rows[0]["UserSales"].ToString().Substring(0, 1);
        string EmpMaintenace = roleInfo.Rows[0]["employeeMaint"].ToString().Substring(3, 1);
        string TCTimeFix = roleInfo.Rows[0]["TC"].ToString().Substring(1, 1);

        string _chart = roleInfo.Rows[0]["Chart"].ToString();
        string _glAdj = roleInfo.Rows[0]["GLAdj"].ToString();
        string _financeState = roleInfo.Rows[0]["Financial"].ToString().Substring(5, 1);

        //string _apVendor = roleInfo.Rows[0]["Vendor"].ToString(); //check Account payable permission
        //string _apBill = roleInfo.Rows[0]["Bill"].ToString();
        //string _apBillPay = roleInfo.Rows[0]["BillPay"].ToString();

        if (_chart.Equals("YYYYYY") && _glAdj.Equals("YYYYYY"))
        {
            chkFinanceMgr.Checked = true;
        }
        if (_financeState.Equals("Y"))
        {
            chkFinanceStatement.Checked = true;
        }
        else
        {
            chkFinanceStatement.Checked = false;
        }

        //Equipment 

        string EquipmentPermission = roleInfo.Rows[0]["elevator"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["elevator"].ToString();

        string Addequip = EquipmentPermission.Length < 1 ? "Y" : EquipmentPermission.Substring(0, 1);
        string Editequip = EquipmentPermission.Length < 2 ? "Y" : EquipmentPermission.Substring(1, 1);
        string Deleteequip = EquipmentPermission.Length < 3 ? "Y" : EquipmentPermission.Substring(2, 1);
        string Viewquip = EquipmentPermission.Length < 4 ? "Y" : EquipmentPermission.Substring(3, 1);

        chkEquipmentsadd.Checked = (Addequip == "N") ? false : true;
        chkEquipmentsedit.Checked = (Editequip == "N") ? false : true;
        chkEquipmentsdelete.Checked = (Deleteequip == "N") ? false : true;
        chkEquipmentsview.Checked = (Viewquip == "N") ? false : true;

        //Project
        string ProjectPermission = roleInfo.Rows[0]["Job"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Job"].ToString();

        string Addejob = ProjectPermission.Length < 1 ? "Y" : ProjectPermission.Substring(0, 1);
        string Editejob = ProjectPermission.Length < 2 ? "Y" : ProjectPermission.Substring(1, 1);
        string Deleteejob = ProjectPermission.Length < 3 ? "Y" : ProjectPermission.Substring(2, 1);
        string Viewjob = ProjectPermission.Length < 4 ? "Y" : ProjectPermission.Substring(3, 1);

        chkProjectadd.Checked = (Addejob == "N") ? false : true;
        chkProjectEdit.Checked = (Editejob == "N") ? false : true;
        chkProjectDelete.Checked = (Deleteejob == "N") ? false : true;
        chkProjectView.Checked = (Viewjob == "N") ? false : true;


        //Location
        string LocationPermission = roleInfo.Rows[0]["Location"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Location"].ToString();

        string AddeLocation = LocationPermission.Length < 1 ? "Y" : LocationPermission.Substring(0, 1);
        string EditeLocation = LocationPermission.Length < 2 ? "Y" : LocationPermission.Substring(1, 1);
        string DeleteeLocation = LocationPermission.Length < 3 ? "Y" : LocationPermission.Substring(2, 1);
        string ViewLocation = LocationPermission.Length < 4 ? "Y" : LocationPermission.Substring(3, 1);

        chkLocationadd.Checked = (AddeLocation == "N") ? false : true;
        chkLocationedit.Checked = (EditeLocation == "N") ? false : true;
        chkLocationdelete.Checked = (DeleteeLocation == "N") ? false : true;
        chkLocationview.Checked = (ViewLocation == "N") ? false : true;

        //Owner

        string OwnerPermission = roleInfo.Rows[0]["Owner"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Owner"].ToString();

        string AddeOwner = OwnerPermission.Length < 1 ? "Y" : OwnerPermission.Substring(0, 1);
        string EditeOwner = OwnerPermission.Length < 2 ? "Y" : OwnerPermission.Substring(1, 1);
        string DeleteeOwner = OwnerPermission.Length < 3 ? "Y" : OwnerPermission.Substring(2, 1);
        string ViewOwner = OwnerPermission.Length < 4 ? "Y" : OwnerPermission.Substring(3, 1);

        chkCustomeradd.Checked = (AddeOwner == "N") ? false : true;
        chkCustomeredit.Checked = (EditeOwner == "N") ? false : true;
        chkCustomerdelete.Checked = (DeleteeOwner == "N") ? false : true;
        chkCustomerview.Checked = (ViewOwner == "N") ? false : true;


        //FinanceDataPermissions
        string ViewFinance = roleInfo.Rows[0]["FinancePermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["FinancePermission"].ToString();
        chkViewFinance.Checked = (ViewFinance == "N") ? false : true;

        // ProjectListPermission

        string ViewProjectListPermission = roleInfo.Rows[0]["ProjectListPermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["ProjectListPermission"].ToString();

        chkViewProjectList.Checked = (ViewProjectListPermission == "N") ? false : true;

        //BOMPermissions
        string BOMPermission = roleInfo.Rows[0]["BOMPermission"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["BOMPermission"].ToString();

        string AddBOM = BOMPermission.Length < 1 ? "Y" : BOMPermission.Substring(0, 1);
        string EditBOM = BOMPermission.Length < 2 ? "Y" : BOMPermission.Substring(1, 1);
        string DeleteBOM = BOMPermission.Length < 3 ? "Y" : BOMPermission.Substring(2, 1);
        string ViewBOM = BOMPermission.Length < 4 ? "Y" : BOMPermission.Substring(3, 1);

        chkAddBOM.Checked = (AddBOM == "N") ? false : true;
        chkEditBOM.Checked = (EditBOM == "N") ? false : true;
        chkDeleteBOM.Checked = (DeleteBOM == "N") ? false : true;
        chkViewBOM.Checked = (ViewBOM == "N") ? false : true;

        //WIPPermissions
        string WIPPermissions = roleInfo.Rows[0]["WIPPermission"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["WIPPermission"].ToString();

        string AddWIP = WIPPermissions.Length < 1 ? "Y" : WIPPermissions.Substring(0, 1);
        string EditWIP = WIPPermissions.Length < 2 ? "Y" : WIPPermissions.Substring(1, 1);
        string DeleteWIP = WIPPermissions.Length < 3 ? "Y" : WIPPermissions.Substring(2, 1);
        string ViewWIP = WIPPermissions.Length < 4 ? "Y" : WIPPermissions.Substring(3, 1);
        string ReportWIP = WIPPermissions.Length < 6 ? "Y" : WIPPermissions.Substring(5, 1);

        chkAddWIP.Checked = (AddWIP == "N") ? false : true;
        chkEditWIP.Checked = (EditWIP == "N") ? false : true;
        chkDeleteWIP.Checked = (DeleteWIP == "N") ? false : true;
        chkViewWIP.Checked = (ViewWIP == "N") ? false : true;
        chkReportWIP.Checked = (ReportWIP == "N") ? false : true;

        //MilestonesPermission
        string MilesStonesPermission = roleInfo.Rows[0]["MilestonesPermission"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["MilestonesPermission"].ToString();


        string AddMilesStones = MilesStonesPermission.Length < 1 ? "Y" : MilesStonesPermission.Substring(0, 1);
        string EditMilesStones = MilesStonesPermission.Length < 2 ? "Y" : MilesStonesPermission.Substring(1, 1);
        string DeleteMilesStones = MilesStonesPermission.Length < 3 ? "Y" : MilesStonesPermission.Substring(2, 1);
        string ViewMilesStones = MilesStonesPermission.Length < 4 ? "Y" : MilesStonesPermission.Substring(3, 1);

        chkAddMilesStones.Checked = (AddMilesStones == "N") ? false : true;
        chkEditMilesStones.Checked = (EditMilesStones == "N") ? false : true;
        chkDeleteMilesStones.Checked = (DeleteMilesStones == "N") ? false : true;
        chkViewMilesStones.Checked = (ViewMilesStones == "N") ? false : true;

        //Inventory Item

        string InventoryItemPermission = roleInfo.Rows[0]["Item"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Item"].ToString();

        string AddInventoryItem = InventoryItemPermission.Length < 1 ? "Y" : InventoryItemPermission.Substring(0, 1);
        string EditInventoryItem = InventoryItemPermission.Length < 2 ? "Y" : InventoryItemPermission.Substring(1, 1);
        string DeleteInventoryItem = InventoryItemPermission.Length < 3 ? "Y" : InventoryItemPermission.Substring(2, 1);
        string ViewInventoryItem = InventoryItemPermission.Length < 4 ? "Y" : InventoryItemPermission.Substring(3, 1);

        chkInventoryItemadd.Checked = (AddInventoryItem == "N") ? false : true;
        chkInventoryItemedit.Checked = (EditInventoryItem == "N") ? false : true;
        chkInventoryItemdelete.Checked = (DeleteInventoryItem == "N") ? false : true;
        chkInventoryItemview.Checked = (ViewInventoryItem == "N") ? false : true;

        //Inventory AdjustMent

        string InventoryAdjustmentPermission = roleInfo.Rows[0]["InvAdj"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["InvAdj"].ToString();

        string AddInventoryAdjustMent = InventoryAdjustmentPermission.Length < 1 ? "Y" : InventoryAdjustmentPermission.Substring(0, 1);
        string EditInventoryAdjustMent = InventoryAdjustmentPermission.Length < 2 ? "Y" : InventoryAdjustmentPermission.Substring(1, 1);
        string DeleteInventoryAdjustMent = InventoryAdjustmentPermission.Length < 3 ? "Y" : InventoryAdjustmentPermission.Substring(2, 1);
        string ViewInventoryAdjustMent = InventoryAdjustmentPermission.Length < 4 ? "Y" : InventoryAdjustmentPermission.Substring(3, 1);

        chkInventoryAdjustmentadd.Checked = (AddInventoryAdjustMent == "N") ? false : true;
        chkInventoryAdjustmentedit.Checked = (EditInventoryAdjustMent == "N") ? false : true;
        chkInventoryAdjustmentdelete.Checked = (DeleteInventoryAdjustMent == "N") ? false : true;
        chkInventoryAdjustmentview.Checked = (ViewInventoryAdjustMent == "N") ? false : true;

        //Inventory WareHouse

        string InventoryWareHousePermission = roleInfo.Rows[0]["Warehouse"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Warehouse"].ToString();

        string AddInventoryWareHouse = InventoryWareHousePermission.Length < 1 ? "Y" : InventoryWareHousePermission.Substring(0, 1);
        string EditInventoryWareHouse = InventoryWareHousePermission.Length < 2 ? "Y" : InventoryWareHousePermission.Substring(1, 1);
        string DeleteInventoryWareHouse = InventoryWareHousePermission.Length < 3 ? "Y" : InventoryWareHousePermission.Substring(2, 1);
        string ViewInventoryWareHouse = InventoryWareHousePermission.Length < 4 ? "Y" : InventoryWareHousePermission.Substring(3, 1);

        chkInventoryWareHouseadd.Checked = (AddInventoryWareHouse == "N") ? false : true;
        chkInventoryWareHouseedit.Checked = (EditInventoryWareHouse == "N") ? false : true;
        chkInventoryWareHousedelete.Checked = (DeleteInventoryWareHouse == "N") ? false : true;
        chkInventoryWareHouseview.Checked = (ViewInventoryWareHouse == "N") ? false : true;

        //Inventory Setup

        string InventorySetupPermission = roleInfo.Rows[0]["InvSetup"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["InvSetup"].ToString();

        string AddInventorySetup = InventorySetupPermission.Length < 1 ? "Y" : InventorySetupPermission.Substring(0, 1);
        string EditInventorySetup = InventorySetupPermission.Length < 2 ? "Y" : InventorySetupPermission.Substring(1, 1);
        string DeleteInventorySetup = InventorySetupPermission.Length < 3 ? "Y" : InventorySetupPermission.Substring(2, 1);
        string ViewInventorySetup = InventorySetupPermission.Length < 4 ? "Y" : InventorySetupPermission.Substring(3, 1);

        chkInventorysetupadd.Checked = (AddInventorySetup == "N") ? false : true;
        chkInventorysetupedit.Checked = (EditInventorySetup == "N") ? false : true;
        chkInventorysetupdelete.Checked = (DeleteInventorySetup == "N") ? false : true;
        chkInventorysetupview.Checked = (ViewInventorySetup == "N") ? false : true;

        //DocumentPermission
        string DocumentPermission = roleInfo.Rows[0]["DocumentPermission"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["DocumentPermission"].ToString();

        string AddDocumentPermission = DocumentPermission.Length < 1 ? "Y" : DocumentPermission.Substring(0, 1);
        string EditDocumentPermission = DocumentPermission.Length < 2 ? "Y" : DocumentPermission.Substring(1, 1);
        string DeleteDocumentPermission = DocumentPermission.Length < 3 ? "Y" : DocumentPermission.Substring(2, 1);
        string ViewDocumentPermission = DocumentPermission.Length < 4 ? "Y" : DocumentPermission.Substring(3, 1);

        chkDocumentAdd.Checked = (AddDocumentPermission == "N") ? false : true;
        chkDocumentEdit.Checked = (EditDocumentPermission == "N") ? false : true;
        chkDocumentDelete.Checked = (DeleteDocumentPermission == "N") ? false : true;
        chkDocumentView.Checked = (ViewDocumentPermission == "N") ? false : true;

        //ContactPermission
        string ContactPermission = roleInfo.Rows[0]["ContactPermission"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["ContactPermission"].ToString();

        string AddContactPermission = ContactPermission.Length < 1 ? "Y" : ContactPermission.Substring(0, 1);
        string EditContactPermission = ContactPermission.Length < 2 ? "Y" : ContactPermission.Substring(1, 1);
        string DeleteContactPermission = ContactPermission.Length < 3 ? "Y" : ContactPermission.Substring(2, 1);
        string ViewContactPermission = ContactPermission.Length < 4 ? "Y" : ContactPermission.Substring(3, 1);

        chkContactAdd.Checked = (AddContactPermission == "N") ? false : true;
        chkContactEdit.Checked = (EditContactPermission == "N") ? false : true;
        chkContactDelete.Checked = (DeleteContactPermission == "N") ? false : true;
        chkContactView.Checked = (ViewContactPermission == "N") ? false : true;


        //VendorsPermission
        string VendorsPermission = roleInfo.Rows[0]["Vendor"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["Vendor"].ToString();

        string AddVendorsPermission = VendorsPermission.Length < 1 ? "Y" : VendorsPermission.Substring(0, 1);
        string EditVendorsPermission = VendorsPermission.Length < 2 ? "Y" : VendorsPermission.Substring(1, 1);
        string DeleteVendorsPermission = VendorsPermission.Length < 3 ? "Y" : VendorsPermission.Substring(2, 1);
        string ViewVendorsPermission = VendorsPermission.Length < 4 ? "Y" : VendorsPermission.Substring(3, 1);

        chkVendorsAdd.Checked = (AddVendorsPermission == "N") ? false : true;
        chkVendorsEdit.Checked = (EditVendorsPermission == "N") ? false : true;
        chkVendorsDelete.Checked = (DeleteVendorsPermission == "N") ? false : true;
        chkVendorsView.Checked = (ViewVendorsPermission == "N") ? false : true;

        //Inventory Finance

        string InventoryFinancePermission = roleInfo.Rows[0]["InvViewer"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["InvViewer"].ToString();

        string AddInventoryFinance = InventoryFinancePermission.Length < 1 ? "Y" : InventoryFinancePermission.Substring(0, 1);
        string EditInventoryFinance = InventoryFinancePermission.Length < 2 ? "Y" : InventoryFinancePermission.Substring(1, 1);
        string DeleteInventoryFinance = InventoryFinancePermission.Length < 3 ? "Y" : InventoryFinancePermission.Substring(2, 1);
        string ViewInventoryFinance = InventoryFinancePermission.Length < 4 ? "Y" : InventoryFinancePermission.Substring(3, 1);

        chkInventoryFinanceAdd.Checked = (AddInventoryFinance == "N") ? false : true;
        chkInventoryFinanceedit.Checked = (EditInventoryFinance == "N") ? false : true;
        chkInventoryFinancedelete.Checked = (DeleteInventoryFinance == "N") ? false : true;
        chkInventoryFinanceview.Checked = (ViewInventoryFinance == "N") ? false : true;



        // Project templates

        string ProjecttempPermission = roleInfo.Rows[0]["ProjecttempPermission"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["ProjecttempPermission"].ToString();

        string AddProjecttempPermission = ProjecttempPermission.Length < 1 ? "Y" : ProjecttempPermission.Substring(0, 1);
        string EditProjecttempPermission = ProjecttempPermission.Length < 2 ? "Y" : ProjecttempPermission.Substring(1, 1);
        string DeleteProjecttempPermission = ProjecttempPermission.Length < 3 ? "Y" : ProjecttempPermission.Substring(2, 1);
        string ViewProjecttempPermission = ProjecttempPermission.Length < 4 ? "Y" : ProjecttempPermission.Substring(3, 1);

        chkProjectTempAdd.Checked = (AddProjecttempPermission == "N") ? false : true;
        chkProjectTempEdit.Checked = (EditProjecttempPermission == "N") ? false : true;
        chkProjectTempDelete.Checked = (DeleteProjecttempPermission == "N") ? false : true;
        chkProjectTempView.Checked = (ViewProjecttempPermission == "N") ? false : true;

        //BillingCodesPermission
        string BillingCodesPermission = roleInfo.Rows[0]["BillingCodesPermission"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["BillingCodesPermission"].ToString();

        string AddBillingCodesPermission = BillingCodesPermission.Length < 1 ? "Y" : BillingCodesPermission.Substring(0, 1);
        string EditBillingCodesPermission = BillingCodesPermission.Length < 2 ? "Y" : BillingCodesPermission.Substring(1, 1);
        string DeleteBillingCodesPermission = BillingCodesPermission.Length < 3 ? "Y" : BillingCodesPermission.Substring(2, 1);
        string ViewBillingCodesPermission = BillingCodesPermission.Length < 4 ? "Y" : BillingCodesPermission.Substring(3, 1);

        chkBillingcodesAdd.Checked = (AddBillingCodesPermission == "N") ? false : true;
        chkBillingcodesEdit.Checked = (EditBillingCodesPermission == "N") ? false : true;
        chkBillingcodesDelete.Checked = (DeleteBillingCodesPermission == "N") ? false : true;
        chkBillingcodesView.Checked = (ViewBillingCodesPermission == "N") ? false : true;

        // Invoice Permission
        string InvoicePermission = roleInfo.Rows[0]["Invoice"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["Invoice"].ToString();

        string AddInvoicePermission = InvoicePermission.Length < 1 ? "Y" : InvoicePermission.Substring(0, 1);
        string EditInvoicePermission = InvoicePermission.Length < 2 ? "Y" : InvoicePermission.Substring(1, 1);
        string DeleteInvoicePermission = InvoicePermission.Length < 3 ? "Y" : InvoicePermission.Substring(2, 1);
        string ViewInvoicePermission = InvoicePermission.Length < 4 ? "Y" : InvoicePermission.Substring(3, 1);

        chkInvoicesAdd.Checked = (AddInvoicePermission == "N") ? false : true;
        chkInvoicesEdit.Checked = (EditInvoicePermission == "N") ? false : true;
        chkInvoicesDelete.Checked = (DeleteInvoicePermission == "N") ? false : true;
        chkInvoicesView.Checked = (ViewInvoicePermission == "N") ? false : true;

        //POPermission
        string POPermission = roleInfo.Rows[0]["PO"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["PO"].ToString();

        string AddPOPermission = POPermission.Length < 1 ? "Y" : POPermission.Substring(0, 1);
        string EditPOPermission = POPermission.Length < 2 ? "Y" : POPermission.Substring(1, 1);
        string DeletePOPermission = POPermission.Length < 3 ? "Y" : POPermission.Substring(2, 1);
        string ViewPOPermission = POPermission.Length < 4 ? "Y" : POPermission.Substring(3, 1);

        chkPOAdd.Checked = (AddPOPermission == "N") ? false : true;
        chkPOEdit.Checked = (EditPOPermission == "N") ? false : true;
        chkPODelete.Checked = (DeletePOPermission == "N") ? false : true;
        chkPOView.Checked = (ViewPOPermission == "N") ? false : true;

        //RPOPermission
        string RPOPermission = roleInfo.Rows[0]["RPO"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["RPO"].ToString();

        string AddRPOPermission = RPOPermission.Length < 1 ? "Y" : RPOPermission.Substring(0, 1);
        string EditRPOPermission = RPOPermission.Length < 2 ? "Y" : RPOPermission.Substring(1, 1);
        string DeleteRPOPermission = RPOPermission.Length < 3 ? "Y" : RPOPermission.Substring(2, 1);
        string ViewRPOPermission = RPOPermission.Length < 4 ? "Y" : RPOPermission.Substring(3, 1);

        chkRPOAdd.Checked = (AddRPOPermission == "N") ? false : true;
        chkRPOEdit.Checked = (EditRPOPermission == "N") ? false : true;
        chkRPODelete.Checked = (DeleteRPOPermission == "N") ? false : true;
        chkRPOView.Checked = (ViewRPOPermission == "N") ? false : true;

        //PurchasingmodulePermission
        string PurchasingmodulePermission = roleInfo.Rows[0]["PurchasingmodulePermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["PurchasingmodulePermission"].ToString();
        chkPurchasingmodule.Checked = (PurchasingmodulePermission == "N") ? false : true;

        //PONotification
        string PONotification = roleInfo.Rows[0]["PONotification"] == DBNull.Value ? "N" : roleInfo.Rows[0]["PONotification"].ToString();
        chkPONotification.Checked = (PONotification == "N") ? false : true;


        //BillingmodulePermission
        string BillingmodulePermission = roleInfo.Rows[0]["BillingmodulePermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["BillingmodulePermission"].ToString();

        chkBillingmodule.Checked = (BillingmodulePermission == "N") ? false : true;

        //BillPermission
        string BillPermission = roleInfo.Rows[0]["Bill"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["Bill"].ToString();

        string AddBillPermission = BillPermission.Length < 1 ? "Y" : BillPermission.Substring(0, 1);
        string EditBillPermission = BillPermission.Length < 2 ? "Y" : BillPermission.Substring(1, 1);
        string DeleteBillPermission = BillPermission.Length < 3 ? "Y" : BillPermission.Substring(2, 1);
        string ViewBillPermission = BillPermission.Length < 4 ? "Y" : BillPermission.Substring(3, 1);

        chkAddBills.Checked = (AddBillPermission == "N") ? false : true;
        chkEditBills.Checked = (EditBillPermission == "N") ? false : true;
        chkDeleteBills.Checked = (DeleteBillPermission == "N") ? false : true;
        chkViewBills.Checked = (ViewBillPermission == "N") ? false : true;

        //BillPayPermission
        string BillPayPermission = roleInfo.Rows[0]["BillPay"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["BillPay"].ToString();

        string AddBillPayPermission = BillPayPermission.Length < 1 ? "Y" : BillPayPermission.Substring(0, 1);
        string EditBillPayPermission = BillPayPermission.Length < 2 ? "Y" : BillPayPermission.Substring(1, 1);
        string DeleteBillPayPermission = BillPayPermission.Length < 3 ? "Y" : BillPayPermission.Substring(2, 1);
        string ViewBillPayPermission = BillPayPermission.Length < 4 ? "Y" : BillPayPermission.Substring(3, 1);
        string ShowBankBalancesPermission = BillPayPermission.Length < 5 ? "Y" : BillPayPermission.Substring(4, 1);

        chkAddManageChecks.Checked = (AddBillPayPermission == "N") ? false : true;
        chkEditManageChecks.Checked = (EditBillPayPermission == "N") ? false : true;
        chkDeleteManageChecks.Checked = (DeleteBillPayPermission == "N") ? false : true;
        chkViewManageChecks.Checked = (ViewBillPayPermission == "N") ? false : true;
        chkShowBankBalances.Checked = (ShowBankBalancesPermission == "N") ? false : true;

        //VendorPermission
        string VendorPermission = roleInfo.Rows[0]["Vendor"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["Vendor"].ToString();

        string AddVendorPermission = VendorPermission.Length < 1 ? "Y" : VendorPermission.Substring(0, 1);
        string EditVendorPermission = VendorPermission.Length < 2 ? "Y" : VendorPermission.Substring(1, 1);
        string DeleteVendorPermission = VendorPermission.Length < 3 ? "Y" : VendorPermission.Substring(2, 1);
        string ViewVendorPermission = VendorPermission.Length < 4 ? "Y" : VendorPermission.Substring(3, 1);

        chkVendorsAdd.Checked = (AddVendorPermission == "N") ? false : true;
        chkVendorsEdit.Checked = (EditVendorPermission == "N") ? false : true;
        chkVendorsDelete.Checked = (DeleteVendorPermission == "N") ? false : true;
        chkVendorsView.Checked = (ViewVendorPermission == "N") ? false : true;

        //AccountPayablemodulePermission
        string AccountPayablemodulePermission = roleInfo.Rows[0]["AccountPayablemodulePermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["AccountPayablemodulePermission"].ToString();

        chkAccountPayable.Checked = (AccountPayablemodulePermission == "N") ? false : true;

        //PaymentHistoryPermission
        string PaymentHistoryPermission = roleInfo.Rows[0]["PaymentHistoryPermission"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["PaymentHistoryPermission"].ToString();


        string ViewPaymentHistoryPermission = PaymentHistoryPermission.Length < 4 ? "Y" : PaymentHistoryPermission.Substring(3, 1);

        chkPaymentHistoryView.Checked = (ViewPaymentHistoryPermission == "N") ? false : true;

        //CustomermodulePermission
        string CustomermodulePermission = roleInfo.Rows[0]["AccountPayablemodulePermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["CustomermodulePermission"].ToString();

        chkCustomerModule.Checked = (CustomermodulePermission == "N") ? false : true;

        //CreditHoldPermissions
        string CreditHoldPermissions = roleInfo.Rows[0]["CreditHold"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["CreditHold"].ToString();
        string ViewCreditHoldPermissions = CreditHoldPermissions.Length < 4 ? "Y" : CreditHoldPermissions.Substring(3, 1);
        chkCreditHold.Checked = (ViewCreditHoldPermissions == "N") ? false : true;

        //CreditFlagPermissions
        string CreditFlagPermissions = roleInfo.Rows[0]["CreditFlag"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["CreditFlag"].ToString();
        string ViewCreditFlagPermissions = CreditFlagPermissions.Length < 4 ? "Y" : CreditFlagPermissions.Substring(3, 1);
        chkCreditFlag.Checked = (ViewCreditFlagPermissions == "N") ? false : true;

        //WriteOffPermissions
        string WriteOffPermissions = roleInfo.Rows[0]["WriteOff"] == DBNull.Value ? "N" : roleInfo.Rows[0]["WriteOff"].ToString().Substring(0, 1);
        chkWriteOff.Checked = (WriteOffPermissions == "N") ? false : true;

        //FinancialmodulePermission
        string FinancialmodulePermission = roleInfo.Rows[0]["FinancialmodulePermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["FinancialmodulePermission"].ToString();

        chkFinancialmodule.Checked = (FinancialmodulePermission == "N") ? false : true;

        //ChartPermission
        string ChartPermission = roleInfo.Rows[0]["Chart"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["Chart"].ToString();

        string AddChartPermission = ChartPermission.Length < 1 ? "Y" : ChartPermission.Substring(0, 1);
        string EditChartPermission = ChartPermission.Length < 2 ? "Y" : ChartPermission.Substring(1, 1);
        string DeleteChartPermission = ChartPermission.Length < 3 ? "Y" : ChartPermission.Substring(2, 1);
        string ViewChartPermission = ChartPermission.Length < 4 ? "Y" : ChartPermission.Substring(3, 1);

        chkChartAdd.Checked = (AddChartPermission == "N") ? false : true;
        chkChartEdit.Checked = (EditChartPermission == "N") ? false : true;
        chkChartDelete.Checked = (DeleteChartPermission == "N") ? false : true;
        chkChartView.Checked = (ViewChartPermission == "N") ? false : true;

        //JournalEntryPermission
        string JournalEntryPermission = roleInfo.Rows[0]["GLAdj"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["GLAdj"].ToString();

        string AddJournalEntryPermission = JournalEntryPermission.Length < 1 ? "Y" : JournalEntryPermission.Substring(0, 1);
        string EditJournalEntryPermission = JournalEntryPermission.Length < 2 ? "Y" : JournalEntryPermission.Substring(1, 1);
        string DeleteJournalEntryPermission = JournalEntryPermission.Length < 3 ? "Y" : JournalEntryPermission.Substring(2, 1);
        string ViewJournalEntryPermission = JournalEntryPermission.Length < 4 ? "Y" : JournalEntryPermission.Substring(3, 1);

        chkJournalEntryAdd.Checked = (AddJournalEntryPermission == "N") ? false : true;
        chkJournalEntryEdit.Checked = (EditJournalEntryPermission == "N") ? false : true;
        chkJournalEntryDelete.Checked = (DeleteJournalEntryPermission == "N") ? false : true;
        chkJournalEntryView.Checked = (ViewJournalEntryPermission == "N") ? false : true;

        //BankrecPermission
        string BankrecPermission = roleInfo.Rows[0]["bankrec"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["bankrec"].ToString();

        string AddBankrecPermission = BankrecPermission.Length < 1 ? "Y" : BankrecPermission.Substring(0, 1);
        string EditBankrecPermission = BankrecPermission.Length < 2 ? "Y" : BankrecPermission.Substring(1, 1);
        string DeleteBankrecPermission = BankrecPermission.Length < 3 ? "Y" : BankrecPermission.Substring(2, 1);
        string ViewBankrecPermission = BankrecPermission.Length < 4 ? "Y" : BankrecPermission.Substring(3, 1);

        chkBankAdd.Checked = (AddBankrecPermission == "N") ? false : true;
        chkBankEdit.Checked = (EditBankrecPermission == "N") ? false : true;
        chkBankDelete.Checked = (DeleteBankrecPermission == "N") ? false : true;
        chkBankView.Checked = (ViewBankrecPermission == "N") ? false : true;


        //RCmodulePermission
        string RCmodulePermission = roleInfo.Rows[0]["RCmodulePermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["RCmodulePermission"].ToString();

        chkRecurring.Checked = (RCmodulePermission == "N") ? false : true;

        //ProcessRCPermission
        string ProcessRCPermission = roleInfo.Rows[0]["ProcessRCPermission"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["ProcessRCPermission"].ToString();

        string AddProcessRCPermissionn = ProcessRCPermission.Length < 1 ? "Y" : ProcessRCPermission.Substring(0, 1);
        string EditProcessRCPermission = ProcessRCPermission.Length < 2 ? "Y" : ProcessRCPermission.Substring(1, 1);
        string DeleteProcessRCPermission = ProcessRCPermission.Length < 3 ? "Y" : ProcessRCPermission.Substring(2, 1);
        string ViewProcessRCPermission = ProcessRCPermission.Length < 4 ? "Y" : ProcessRCPermission.Substring(3, 1);

        chkRecContractsAdd.Checked = (AddProcessRCPermissionn == "N") ? false : true;
        chkRecContractsEdit.Checked = (EditProcessRCPermission == "N") ? false : true;
        chkRecContractsDelete.Checked = (DeleteProcessRCPermission == "N") ? false : true;
        chkRecContractsView.Checked = (ViewProcessRCPermission == "N") ? false : true;

        //RCRenewEscalatePermission
        string RCRenewEscalatePermission = roleInfo.Rows[0]["RCRenewEscalatePermission"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["RCRenewEscalatePermission"].ToString();

        string AddRCRenewEscalatePermission = RCRenewEscalatePermission.Length < 1 ? "Y" : RCRenewEscalatePermission.Substring(0, 1);
        string ViewRCRenewEscalatePermission = RCRenewEscalatePermission.Length < 4 ? "Y" : RCRenewEscalatePermission.Substring(3, 1);

        chkRenewEscalateAdd.Checked = (AddRCRenewEscalatePermission == "N") ? false : true;
        chkRenewEscalateView.Checked = (ViewRCRenewEscalatePermission == "N") ? false : true;
        //ProcessCPermission
        string ProcessCPermission = roleInfo.Rows[0]["ProcessC"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["ProcessC"].ToString();

        string AddProcessCPermission = ProcessCPermission.Length < 1 ? "Y" : ProcessCPermission.Substring(0, 1);
        string EditProcessCPermission = ProcessCPermission.Length < 2 ? "Y" : ProcessCPermission.Substring(1, 1);
        string DeleteProcessCPermission = ProcessCPermission.Length < 3 ? "Y" : ProcessCPermission.Substring(2, 1);
        string ViewProcessCPermission = ProcessCPermission.Length < 4 ? "Y" : ProcessCPermission.Substring(3, 1);

        chkRecInvoicesAdd.Checked = (AddProcessCPermission == "N") ? false : true;
        //chkRecInvoicesEdit.Checked = (EditProcessCPermission == "N") ? false : true;
        chkRecInvoicesDelete.Checked = (DeleteProcessCPermission == "N") ? false : true;
        chkRecInvoicesView.Checked = (ViewProcessCPermission == "N") ? false : true;

        // Recurring TicketPermission
        string ProcessTPermission = roleInfo.Rows[0]["ProcessT"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["ProcessT"].ToString();

        string AddProcessTPermission = ProcessTPermission.Length < 1 ? "Y" : ProcessTPermission.Substring(0, 1);
        string EditProcessTPermission = ProcessTPermission.Length < 2 ? "Y" : ProcessTPermission.Substring(1, 1);
        string DeleteProcessTPermission = ProcessTPermission.Length < 3 ? "Y" : ProcessTPermission.Substring(2, 1);
        string ViewProcessTPermission = ProcessTPermission.Length < 4 ? "Y" : ProcessTPermission.Substring(3, 1);

        chkRecTicketsAdd.Checked = (AddProcessTPermission == "N") ? false : true;
        //chkRecTicketsEdit.Checked = (EditProcessTPermission == "N") ? false : true;
        chkRecTicketsDelete.Checked = (DeleteProcessTPermission == "N") ? false : true;
        chkRecTicketsView.Checked = (ViewProcessTPermission == "N") ? false : true;

        //SafetyTestPermission
        string SafetyTestPermission = roleInfo.Rows[0]["SafetyTestsPermission"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["SafetyTestsPermission"].ToString();

        string AddSafetyTestPermission = SafetyTestPermission.Length < 1 ? "Y" : SafetyTestPermission.Substring(0, 1);
        string EditSafetyTestPermission = SafetyTestPermission.Length < 2 ? "Y" : SafetyTestPermission.Substring(1, 1);
        string DeleteSafetyTestPermission = SafetyTestPermission.Length < 3 ? "Y" : SafetyTestPermission.Substring(2, 1);
        string ViewSafetyTestPermission = SafetyTestPermission.Length < 4 ? "Y" : SafetyTestPermission.Substring(3, 1);

        chkSafetyTestsAdd.Checked = (AddSafetyTestPermission == "N") ? false : true;
        chkSafetyTestsEdit.Checked = (EditSafetyTestPermission == "N") ? false : true;
        chkSafetyTestsDelete.Checked = (DeleteSafetyTestPermission == "N") ? false : true;
        chkSafetyTestsView.Checked = (ViewSafetyTestPermission == "N") ? false : true;

        //ViolationPermission
        string ViolationPermission = roleInfo.Rows[0]["ViolationPermission"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["ViolationPermission"].ToString();

        string AddViolationPermission = ViolationPermission.Length < 1 ? "Y" : ViolationPermission.Substring(0, 1);
        string EditViolationPermission = ViolationPermission.Length < 2 ? "Y" : ViolationPermission.Substring(1, 1);
        string DeleteViolationPermission = ViolationPermission.Length < 3 ? "Y" : ViolationPermission.Substring(2, 1);
        string ViewViolationPermission = ViolationPermission.Length < 4 ? "Y" : ViolationPermission.Substring(3, 1);

        chkViolationsAdd.Checked = (AddViolationPermission == "N") ? false : true;
        chkViolationsDelete.Checked = (DeleteSafetyTestPermission == "N") ? false : true;


        //ScheduleModulePermission
        string SchedulemodulePermission = roleInfo.Rows[0]["SchedulemodulePermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["SchedulemodulePermission"].ToString();

        chkSchedule.Checked = (SchedulemodulePermission == "N") ? false : true;




        //ProjectModulePermission
        string ProjectModulePermission = roleInfo.Rows[0]["ProjectModulePermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["ProjectModulePermission"].ToString();

        chkProjectmodule.Checked = (ProjectModulePermission == "N") ? false : true;

        //InventoryModulePermission
        string InventoryModulePermission = roleInfo.Rows[0]["InventoryModulePermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["InventoryModulePermission"].ToString();

        chkInventorymodule.Checked = (InventoryModulePermission == "N") ? false : true;

        //JobClosePermission
        string JobClosePermission = roleInfo.Rows[0]["JobClosePermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["JobClosePermission"].ToString();
        if (JobClosePermission.Contains("Y")) chkJobClosePermission.Checked = true; else chkJobClosePermission.Checked = false;


        //JobCompletedPermission
        string JobCompletedPermission = roleInfo.Rows[0]["JobCompletedPermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["JobCompletedPermission"].ToString();
        if (JobCompletedPermission.Contains("Y")) chkJobCompletedPermission.Checked = true; else chkJobCompletedPermission.Checked = false;

        //JobReopenPermission
        string JobReopenPermission = roleInfo.Rows[0]["JobReopenPermission"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["JobReopenPermission"].ToString();
        if (JobReopenPermission.Contains("Y")) chkJobReopenPermission.Checked = true; else chkJobReopenPermission.Checked = false;


        //SchedulePermission
        string SchedulePermission = roleInfo.Rows[0]["Ticket"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Ticket"].ToString();


        string ViewSchedulePermission = SchedulePermission.Length < 4 ? "Y" : SchedulePermission.Substring(3, 1);
        string TicketReportPer = SchedulePermission.Length < 6 ? "Y" : SchedulePermission.Substring(5, 1);

        chkScheduleBoard.Checked = (ViewSchedulePermission == "N") ? false : true;

        //Dispatch
        string TicketPermission = roleInfo.Rows[0]["Dispatch"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Dispatch"].ToString();

        string AddTicketePermission = TicketPermission.Length < 1 ? "Y" : TicketPermission.Substring(0, 1);
        string EditTicketePermission = TicketPermission.Length < 2 ? "Y" : TicketPermission.Substring(1, 1);
        string DeleteTicketePermission = TicketPermission.Length < 3 ? "Y" : TicketPermission.Substring(2, 1);
        string ViewTicketePermission = TicketPermission.Length < 4 ? "Y" : TicketPermission.Substring(3, 1);
        //string ReportTicketePermission = TicketPermission.Length < 5 ? "Y" : TicketPermission.Substring(4, 1);
        //string EmailDispatchPermission = TicketPermission.Length < 6 ? "Y" : TicketPermission.Substring(5, 1);

        /** Thomas: I am changing this to make it the same other fields (Leads, Opportunities, Estimates)
        1: Add
        2: Edit
        3: Delete
        4: View
        5: Dispatch (this case)
        6: Report
        */
        string EmailDispatchPermission = TicketPermission.Length < 5 ? "Y" : TicketPermission.Substring(4, 1);
        string ReportTicketePermission = TicketPermission.Length < 6 ? "Y" : TicketPermission.Substring(5, 1);

        chkTicketListAdd.Checked = (AddTicketePermission == "N") ? false : true;
        chkTicketListEdit.Checked = (EditTicketePermission == "N") ? false : true;
        chkTicketListDelete.Checked = (DeleteTicketePermission == "N") ? false : true;
        chkTicketListView.Checked = (ViewTicketePermission == "N") ? false : true;
        chkTicketListReport.Checked = (ReportTicketePermission == "N") ? false : true;
        chkDispatch.Checked = (EmailDispatchPermission == "N") ? false : true;


        //Manual Timesheet Permission
        string MTimesheetPermission = roleInfo.Rows[0]["MTimesheet"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["MTimesheet"].ToString();

        string Timesheetadd = MTimesheetPermission.Length < 1 ? "Y" : MTimesheetPermission.Substring(0, 1);
        string Timesheetedit = MTimesheetPermission.Length < 2 ? "Y" : MTimesheetPermission.Substring(1, 1);
        string Timesheetdelete = MTimesheetPermission.Length < 3 ? "Y" : MTimesheetPermission.Substring(2, 1);
        string Timesheetview = MTimesheetPermission.Length < 4 ? "Y" : MTimesheetPermission.Substring(3, 1);
        string Timesheetreport = MTimesheetPermission.Length < 6 ? "Y" : MTimesheetPermission.Substring(5, 1);

        chkTimesheetadd.Checked = (Timesheetadd == "N") ? false : true;
        chkTimesheetedit.Checked = (Timesheetedit == "N") ? false : true;
        chkTimesheetdelete.Checked = (Timesheetdelete == "N") ? false : true;
        chkTimesheetview.Checked = (Timesheetview == "N") ? false : true;
        chkTimesheetreport.Checked = (Timesheetreport == "N") ? false : true;

        //E Timesheet Permission
        string ETimesheetPermission = roleInfo.Rows[0]["ETimesheet"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["ETimesheet"].ToString();

        string ETimesheetaddPermission = ETimesheetPermission.Length < 1 ? "Y" : ETimesheetPermission.Substring(0, 1);
        string ETimesheeteditPermission = ETimesheetPermission.Length < 2 ? "Y" : ETimesheetPermission.Substring(1, 1);
        string ETimesheetdeletePermission = ETimesheetPermission.Length < 3 ? "Y" : ETimesheetPermission.Substring(2, 1);
        string ETimesheetviewPermission = ETimesheetPermission.Length < 4 ? "Y" : ETimesheetPermission.Substring(3, 1);
        string ETimesheetreportPermission = ETimesheetPermission.Length < 6 ? "Y" : ETimesheetPermission.Substring(5, 1);

        chkETimesheetadd.Checked = (ETimesheetaddPermission == "N") ? false : true;
        chkETimesheetedit.Checked = (ETimesheeteditPermission == "N") ? false : true;
        chkETimesheetdelete.Checked = (ETimesheetdeletePermission == "N") ? false : true;
        chkETimesheetview.Checked = (ETimesheetviewPermission == "N") ? false : true;
        chkETimesheetreport.Checked = (ETimesheetreportPermission == "N") ? false : true;

        //MapR Permission
        string MapRPermission = roleInfo.Rows[0]["MapR"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["MapR"].ToString();

        string MapaddPermission = MapRPermission.Length < 1 ? "Y" : MapRPermission.Substring(0, 1);
        string MapeditPermission = MapRPermission.Length < 2 ? "Y" : MapRPermission.Substring(1, 1);
        string MapdeletePermission = MapRPermission.Length < 3 ? "Y" : MapRPermission.Substring(2, 1);
        string MapviewPermission = MapRPermission.Length < 4 ? "Y" : MapRPermission.Substring(3, 1);
        string MapPermission = MapRPermission.Length < 6 ? "Y" : MapRPermission.Substring(5, 1);

        chkMapAdd.Checked = (MapaddPermission == "N") ? false : true;
        chkMapEdit.Checked = (MapeditPermission == "N") ? false : true;
        chkMapDelete.Checked = (MapdeletePermission == "N") ? false : true;
        chkMapView.Checked = (MapviewPermission == "N") ? false : true;
        chkMapReport.Checked = (MapPermission == "N") ? false : true;

        //RouteBuilder Permission
        string RouteBuilderPermission = roleInfo.Rows[0]["RouteBuilder"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["RouteBuilder"].ToString();

        string RouteBuilderaddPermission = RouteBuilderPermission.Length < 1 ? "Y" : RouteBuilderPermission.Substring(0, 1);
        string RouteBuildereditPermission = RouteBuilderPermission.Length < 2 ? "Y" : RouteBuilderPermission.Substring(1, 1);
        string RouteBuilderdeletePermission = RouteBuilderPermission.Length < 3 ? "Y" : RouteBuilderPermission.Substring(2, 1);
        string RouteBuilderviewPermission = RouteBuilderPermission.Length < 4 ? "Y" : RouteBuilderPermission.Substring(3, 1);
        string RouteBuilderReportPermission = RouteBuilderPermission.Length < 6 ? "Y" : RouteBuilderPermission.Substring(5, 1);

        chkRouteBuilderAdd.Checked = (RouteBuilderaddPermission == "N") ? false : true;
        chkRouteBuilderEdit.Checked = (RouteBuildereditPermission == "N") ? false : true;
        chkRouteBuilderDelete.Checked = (RouteBuilderdeletePermission == "N") ? false : true;
        chkRouteBuilderView.Checked = (RouteBuilderviewPermission == "N") ? false : true;
        chkRouteBuilderReport.Checked = (RouteBuilderReportPermission == "N") ? false : true;


        //TicketSchedulePermission
        string TicketSchedulePermission = roleInfo.Rows[0]["Resolve"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Resolve"].ToString();

        string AddTicketeSchedulePermission = TicketSchedulePermission.Length < 1 ? "Y" : TicketSchedulePermission.Substring(0, 1);
        string EditTicketeSchedulePermission = TicketSchedulePermission.Length < 2 ? "Y" : TicketSchedulePermission.Substring(1, 1);
        string DeleteTicketeSchedulePermission = TicketSchedulePermission.Length < 3 ? "Y" : TicketSchedulePermission.Substring(2, 1);
        string ViewTicketeSchedulePermission = TicketSchedulePermission.Length < 4 ? "Y" : TicketSchedulePermission.Substring(3, 1);
        string ReportTicketeSchedulePermission = TicketSchedulePermission.Length < 6 ? "Y" : TicketSchedulePermission.Substring(5, 1);

        chkResolveTicketAdd.Checked = (AddTicketeSchedulePermission == "N") ? false : true;
        chkResolveTicketEdit.Checked = (EditTicketeSchedulePermission == "N") ? false : true;
        chkResolveTicketDelete.Checked = (DeleteTicketeSchedulePermission == "N") ? false : true;
        chkResolveTicketView.Checked = (ViewTicketeSchedulePermission == "N") ? false : true;
        chkResolveTicketReport.Checked = (ReportTicketeSchedulePermission == "N") ? false : true;

        // Sales Module Permission
        string SalesModulePermission = roleInfo.Rows[0]["SalesManager"] == DBNull.Value ? "Y" : roleInfo.Rows[0]["SalesManager"].ToString();

        chkSalesMgr.Checked = (SalesModulePermission == "N") ? false : true;

        //Sales Permission
        string SalesPermission = roleInfo.Rows[0]["UserSales"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["UserSales"].ToString();

        string SalesaddPermission = SalesPermission.Length < 1 ? "Y" : SalesPermission.Substring(0, 1);
        string SaleseditPermission = SalesPermission.Length < 2 ? "Y" : SalesPermission.Substring(1, 1);
        string SalesdeletePermission = SalesPermission.Length < 3 ? "Y" : SalesPermission.Substring(2, 1);
        string SalesviewPermission = SalesPermission.Length < 4 ? "Y" : SalesPermission.Substring(3, 1);
        string SalesReportPermission = SalesPermission.Length < 6 ? "Y" : SalesPermission.Substring(5, 1);

        chkLeadAdd.Checked = (SalesaddPermission == "N") ? false : true;
        chkLeadEdit.Checked = (SaleseditPermission == "N") ? false : true;
        chkLeadDelete.Checked = (SalesdeletePermission == "N") ? false : true;
        chkLeadView.Checked = (SalesviewPermission == "N") ? false : true;
        chkLeadReport.Checked = (SalesReportPermission == "N") ? false : true;


        //Payroll check
        bool PRPermission = Convert.ToBoolean(roleInfo.Rows[0]["PR"] == DBNull.Value ? false : roleInfo.Rows[0]["PR"]);
        payrollModulchck.Checked = (PRPermission == false) ? false : true;


        //Payroll Permission Employee
        string employeePermission = roleInfo.Rows[0]["EmployeeMaint"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["EmployeeMaint"].ToString();

        string AddemployeePermission = employeePermission.Length < 1 ? "Y" : employeePermission.Substring(0, 1);
        string EditemployeePermission = employeePermission.Length < 2 ? "Y" : employeePermission.Substring(1, 1);
        string DeleteemployeePermission = employeePermission.Length < 3 ? "Y" : employeePermission.Substring(2, 1);
        string ViewemployeePermission = employeePermission.Length < 4 ? "Y" : employeePermission.Substring(3, 1);


        empAdd.Checked = (AddTicketeSchedulePermission == "N") ? false : true;
        empEdit.Checked = (EditTicketeSchedulePermission == "N") ? false : true;
        empDelete.Checked = (DeleteTicketeSchedulePermission == "N") ? false : true;
        empView.Checked = (ViewTicketeSchedulePermission == "N") ? false : true;


        //Run Payroll Permission 

        string RunPayrollPermission = roleInfo.Rows[0]["PRProcess"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["PRProcess"].ToString();

        string AddRunPayroll = RunPayrollPermission.Length < 1 ? "Y" : RunPayrollPermission.Substring(0, 1);
        string EditRunPayrollPermission = RunPayrollPermission.Length < 2 ? "Y" : RunPayrollPermission.Substring(1, 1);
        string DeleteRunPayrollPermission = RunPayrollPermission.Length < 3 ? "Y" : RunPayrollPermission.Substring(2, 1);
        string ViewRunPayrollPermission = RunPayrollPermission.Length < 4 ? "Y" : RunPayrollPermission.Substring(3, 1);


        runAdd.Checked = (AddRunPayroll == "N") ? false : true;
        runEdit.Checked = (EditRunPayrollPermission == "N") ? false : true;
        runDelete.Checked = (DeleteRunPayrollPermission == "N") ? false : true;
        runView.Checked = (ViewRunPayrollPermission == "N") ? false : true;


        //Payroll check Permission 
        string payrollCheckPermission = roleInfo.Rows[0]["PRRegister"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["PRRegister"].ToString();

        string AddpayrollCheckPermission = payrollCheckPermission.Length < 1 ? "Y" : payrollCheckPermission.Substring(0, 1);
        string EditpayrollCheckPermission = payrollCheckPermission.Length < 2 ? "Y" : payrollCheckPermission.Substring(1, 1);
        string DeletepayrollCheckPermission = payrollCheckPermission.Length < 3 ? "Y" : payrollCheckPermission.Substring(2, 1);
        string ViewpayrollCheckPermission = payrollCheckPermission.Length < 4 ? "Y" : payrollCheckPermission.Substring(3, 1);


        payrollchckAdd.Checked = (AddpayrollCheckPermission == "N") ? false : true;
        payrollchckEdit.Checked = (EditpayrollCheckPermission == "N") ? false : true;
        payrollchckDelete.Checked = (DeletepayrollCheckPermission == "N") ? false : true;
        payrollchckView.Checked = (ViewpayrollCheckPermission == "N") ? false : true;

        //Payroll Form Permission 
        string payrollFormPermission = roleInfo.Rows[0]["PRReport"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["PRReport"].ToString();

        string AddpayrollFormPermission = payrollFormPermission.Length < 1 ? "Y" : payrollFormPermission.Substring(0, 1);
        string EdipayrollFormPermission = payrollFormPermission.Length < 2 ? "Y" : payrollFormPermission.Substring(1, 1);
        string DeletepayrollFormPermission = payrollFormPermission.Length < 3 ? "Y" : payrollFormPermission.Substring(2, 1);
        string ViewpayrollFormPermission = payrollFormPermission.Length < 4 ? "Y" : payrollFormPermission.Substring(3, 1);


        payrollformAdd.Checked = (AddpayrollFormPermission == "N") ? false : true;
        payrollformEdit.Checked = (EdipayrollFormPermission == "N") ? false : true;
        payrollformDelete.Checked = (DeletepayrollFormPermission == "N") ? false : true;
        payrollformView.Checked = (ViewpayrollFormPermission == "N") ? false : true;

        //wages Permission 
        string wagesPermission = roleInfo.Rows[0]["PRWage"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["PRWage"].ToString();

        string AddwagesPermission = wagesPermission.Length < 1 ? "Y" : wagesPermission.Substring(0, 1);
        string EditwagesPermission = wagesPermission.Length < 2 ? "Y" : wagesPermission.Substring(1, 1);
        string DeletewagesPermission = wagesPermission.Length < 3 ? "Y" : wagesPermission.Substring(2, 1);
        string ViewwagesPermission = wagesPermission.Length < 4 ? "Y" : wagesPermission.Substring(3, 1);


        wagesadd.Checked = (AddwagesPermission == "N") ? false : true;
        wagesEdit.Checked = (EditwagesPermission == "N") ? false : true;
        wagesDelete.Checked = (DeletewagesPermission == "N") ? false : true;
        wagesView.Checked = (ViewwagesPermission == "N") ? false : true;

        //Payroll Permission Employee
        string deductionPermission = roleInfo.Rows[0]["PRDeduct"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["PRDeduct"].ToString();

        string AddedeductionPermission = deductionPermission.Length < 1 ? "Y" : deductionPermission.Substring(0, 1);
        string EditdeductionPermission = deductionPermission.Length < 2 ? "Y" : deductionPermission.Substring(1, 1);
        string DeletedeductionPermission = deductionPermission.Length < 3 ? "Y" : deductionPermission.Substring(2, 1);
        string ViewdeductionPermission = deductionPermission.Length < 4 ? "Y" : deductionPermission.Substring(3, 1);


        deductionsAdd.Checked = (AddedeductionPermission == "N") ? false : true;
        deductionsEdit.Checked = (EditdeductionPermission == "N") ? false : true;
        deductionsDelete.Checked = (DeletedeductionPermission == "N") ? false : true;
        deductionsView.Checked = (ViewdeductionPermission == "N") ? false : true;


        int taskPermission = roleInfo.Rows[0]["ToDo"] == DBNull.Value ? 0 : Convert.ToInt32(roleInfo.Rows[0]["ToDo"]);

        if (taskPermission == 1)
        {
            chkTasks.Checked = true;
        }
        else
        {
            chkTasks.Checked = false;
        }
        int completeTaskPermission = roleInfo.Rows[0]["ToDoC"] == DBNull.Value ? 0 : Convert.ToInt32(roleInfo.Rows[0]["ToDoC"]);
        if (completeTaskPermission == 1)
        {
            chkCompleteTask.Checked = true;
        }
        else
        {
            chkCompleteTask.Checked = false;
        }
        string FollowUpPermission = roleInfo.Rows[0]["FU"] == DBNull.Value ? "YYYYYY" : roleInfo.Rows[0]["FU"].ToString();

        chkFollowUp.Checked = (FollowUpPermission == "NNNNNN") ? false : true;

        string EstimatesPermission = roleInfo.Rows[0]["Estimates"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Estimates"].ToString();

        string EstimatesaddPermission = EstimatesPermission.Length < 1 ? "Y" : EstimatesPermission.Substring(0, 1);
        string EstimateseditPermission = EstimatesPermission.Length < 2 ? "Y" : EstimatesPermission.Substring(1, 1);
        string EstimatesdeletePermission = EstimatesPermission.Length < 3 ? "Y" : EstimatesPermission.Substring(2, 1);
        string EstimatesviewPermission = EstimatesPermission.Length < 4 ? "Y" : EstimatesPermission.Substring(3, 1);
        string EstimatesReportPermission = EstimatesPermission.Length < 6 ? "Y" : EstimatesPermission.Substring(5, 1);


        chkEstimateAdd.Checked = (EstimatesaddPermission == "N") ? false : true;
        chkEstimateEdit.Checked = (EstimateseditPermission == "N") ? false : true;
        chkEstimateDelete.Checked = (EstimatesdeletePermission == "N") ? false : true;
        chkEstimateView.Checked = (EstimatesviewPermission == "N") ? false : true;
        chkEstimateReport.Checked = (EstimatesReportPermission == "N") ? false : true;

        string AwardEstimatesPermission = roleInfo.Rows[0]["AwardEstimates"] == DBNull.Value ? "YYYYYY" : roleInfo.Rows[0]["AwardEstimates"].ToString();

        chkConvertEstimate.Checked = (AwardEstimatesPermission == "NNNNNN") ? false : true;

        string salesSetupPermission = roleInfo.Rows[0]["salessetup"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["salessetup"].ToString();

        chkSalesSetup.Checked = (salesSetupPermission == "NNNNNN") ? false : true;

        string proposalPermission = roleInfo.Rows[0]["Proposal"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Proposal"].ToString();

        string oppaddPermission = proposalPermission.Length < 1 ? "Y" : proposalPermission.Substring(0, 1);
        string oppeditPermission = proposalPermission.Length < 2 ? "Y" : proposalPermission.Substring(1, 1);
        string oppdeletePermission = proposalPermission.Length < 3 ? "Y" : proposalPermission.Substring(2, 1);
        string oppviewPermission = proposalPermission.Length < 4 ? "Y" : proposalPermission.Substring(3, 1);
        string oppReportPermission = proposalPermission.Length < 6 ? "Y" : proposalPermission.Substring(5, 1);


        chkOppAdd.Checked = (oppaddPermission == "N") ? false : true;
        chkOppEdit.Checked = (oppeditPermission == "N") ? false : true;
        chkOppDelete.Checked = (oppdeletePermission == "N") ? false : true;
        chkOppView.Checked = (oppviewPermission == "N") ? false : true;
        chkOppReport.Checked = (oppReportPermission == "N") ? false : true;
        #region ticketPermission


        /////// ticketPermission New

        // TicketVoidPermission

        string strTicketVoidPermission = roleInfo.Rows[0]["TicketVoidPermission"] == DBNull.Value ? "N" : roleInfo.Rows[0]["TicketVoidPermission"].ToString();

        chkTicketVoidPermission.Checked = (strTicketVoidPermission == "0") ? false : true;


        // MassTimesheetCheck

        string strMassTimesheetCheck = roleInfo.Rows[0]["MassTimesheetCheck"] == DBNull.Value ? "N" : roleInfo.Rows[0]["MassTimesheetCheck"].ToString();

        chkMassTimesheetCheck.Checked = (strMassTimesheetCheck == "N") ? false : true;

        string ticketPermission = roleInfo.Rows[0]["Dispatch"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Dispatch"].ToString();

        string AddTicket = ticketPermission.Length < 1 ? "Y" : ticketPermission.Substring(0, 1);
        string EditTicket = ticketPermission.Length < 2 ? "Y" : ticketPermission.Substring(1, 1);
        string DeleteTicket = ticketPermission.Length < 3 ? "Y" : ticketPermission.Substring(2, 1);
        string ViewTicket = ticketPermission.Length < 4 ? "Y" : ticketPermission.Substring(3, 1);

        chkTicketListAdd.Checked = (AddTicket == "N") ? false : true;
        chkTicketListEdit.Checked = (EditTicket == "N") ? false : true;
        chkTicketListDelete.Checked = (DeleteTicket == "N") ? false : true;
        chkTicketListView.Checked = (ViewTicket == "N") ? false : true;

        /////// ApplyPermission New
        string ApplyPermission = roleInfo.Rows[0]["Apply"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Apply"].ToString();

        string AddReceivePayment = ApplyPermission.Length < 1 ? "Y" : ApplyPermission.Substring(0, 1);
        string EditReceivePayment = ApplyPermission.Length < 2 ? "Y" : ApplyPermission.Substring(1, 1);
        string DeleteReceivePayment = ApplyPermission.Length < 3 ? "Y" : ApplyPermission.Substring(2, 1);
        string ViewReceivePayment = ApplyPermission.Length < 4 ? "Y" : ApplyPermission.Substring(3, 1);

        chkReceivePaymentAdd.Checked = (AddReceivePayment == "N") ? false : true;
        chkReceivePaymentEdit.Checked = (EditReceivePayment == "N") ? false : true;
        chkReceivePaymentDelete.Checked = (DeleteReceivePayment == "N") ? false : true;
        chkReceivePaymentView.Checked = (ViewReceivePayment == "N") ? false : true;


        ////OnlinePaymentPermission
        //string OnlinePaymentPermission = roleInfo.Rows[0]["OnlinePayment"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["OnlinePayment"].ToString();

        ////string AddOnlinePayment = OnlinePaymentPermission.Length < 1 ? "Y" : OnlinePaymentPermission.Substring(0, 1);
        ////string EditOnlinePayment = OnlinePaymentPermission.Length < 2 ? "Y" : OnlinePaymentPermission.Substring(1, 1);
        ////string DeleteOnlinePayment = OnlinePaymentPermission.Length < 3 ? "Y" : OnlinePaymentPermission.Substring(2, 1);
        //string ViewOnlinePayment = OnlinePaymentPermission.Length < 4 ? "Y" : OnlinePaymentPermission.Substring(3, 1);
        ////string ReportOnlinePayment = OnlinePaymentPermission.Length < 5 ? "Y" : OnlinePaymentPermission.Substring(4, 1);
        //string ApproveOnlinePayment = OnlinePaymentPermission.Length < 6 ? "Y" : OnlinePaymentPermission.Substring(5, 1);

        ////chkOnlinePaymentAdd.Checked = (AddOnlinePayment == "N") ? false : true;
        ////chkOnlinePaymentEdit.Checked = (EditOnlinePayment == "N") ? false : true;
        ////chkOnlinePaymentDelete.Checked = (DeleteOnlinePayment == "N") ? false : true;
        //chkOnlinePaymentView.Checked = (ViewOnlinePayment == "N") ? false : true;
        ////chkOnlinePaymentReport.Checked = (ReportOnlinePayment == "N") ? false : true;
        //chkOnlinePaymentApprove.Checked = (ApproveOnlinePayment == "N") ? false : true;


        //DepositPermission
        string DepositPermission = roleInfo.Rows[0]["Deposit"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["Deposit"].ToString();

        string AddDepositPermission = DepositPermission.Length < 1 ? "Y" : DepositPermission.Substring(0, 1);
        string EditDepositPermission = DepositPermission.Length < 2 ? "Y" : DepositPermission.Substring(1, 1);
        string DeleteDepositPermission = DepositPermission.Length < 3 ? "Y" : DepositPermission.Substring(2, 1);
        string ViewDepositPermission = DepositPermission.Length < 4 ? "Y" : DepositPermission.Substring(3, 1);

        chkMakeDepositAdd.Checked = (AddDepositPermission == "N") ? false : true;
        chkMakeDepositEdit.Checked = (EditDepositPermission == "N") ? false : true;
        chkMakeDepositDelete.Checked = (DeleteDepositPermission == "N") ? false : true;
        chkMakeDepositView.Checked = (ViewDepositPermission == "N") ? false : true;


        //CollectionsPermission
        string CollectionsPermission = roleInfo.Rows[0]["Collection"] == DBNull.Value ? "NNNNNN" : roleInfo.Rows[0]["Collection"].ToString();

        string AddCollectionsPermission = CollectionsPermission.Length < 1 ? "Y" : CollectionsPermission.Substring(0, 1);
        string EditCollectionsPermission = CollectionsPermission.Length < 2 ? "Y" : CollectionsPermission.Substring(1, 1);
        string DeleteCollectionsPermission = CollectionsPermission.Length < 3 ? "Y" : CollectionsPermission.Substring(2, 1);
        string ViewCollectionsPermission = CollectionsPermission.Length < 4 ? "Y" : CollectionsPermission.Substring(3, 1);
        string ReportCollectionsPermission = CollectionsPermission.Length < 6 ? "Y" : CollectionsPermission.Substring(5, 1);

        chkCollectionsAdd.Checked = (AddCollectionsPermission == "N") ? false : true;
        chkCollectionsEdit.Checked = (EditCollectionsPermission == "N") ? false : true;
        chkCollectionsDelete.Checked = (DeleteCollectionsPermission == "N") ? false : true;
        chkCollectionsView.Checked = (ViewCollectionsPermission == "N") ? false : true;
        chkCollectionsReport.Checked = (ReportCollectionsPermission == "N") ? false : true;

        #endregion

        if (roleInfo.Rows[0]["Control"].ToString() != string.Empty)
        {
            ProgFunc = roleInfo.Rows[0]["Control"].ToString().Substring(0, 1);
        }
        if (roleInfo.Rows[0]["users"].ToString() != string.Empty)
        {
            AccessUser = roleInfo.Rows[0]["users"].ToString().Substring(0, 1);
        }

        if (EmpMaintenace == "Y")
        {
            chkEmpMainten.Checked = true;
        }
        else
        {
            chkEmpMainten.Checked = false;
        }

        if (TCTimeFix == "Y")
            chkTimestampFix.Checked = true;
        else
            chkTimestampFix.Checked = false;

        if (LocnRemark == "Y")
        {
            chkLocationview.Checked = true;
        }
        else
        {
            chkLocationview.Checked = false;
        }

        if (PurcOrders == "Y")
        {
            chkPOAdd.Checked = true;
        }
        else
        {
            chkPOAdd.Checked = false;
        }

        if (Exp == "Y")
        {
            chkExpenses.Checked = true;
        }
        else
        {
            chkExpenses.Checked = false;
        }

        if (ProgFunc == "Y")
        {
            chkProgram.Checked = true;
        }
        else
        {
            chkProgram.Checked = false;
        }

        if (AccessUser == "Y")
        {
            chkAccessUser.Checked = true;
        }
        else
        {
            chkAccessUser.Checked = false;
        }

        if (Sales == "Y")
        {
            chkSalesMgr.Checked = true;
        }
        else
        {
            chkSalesMgr.Checked = false;
        }

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "showHipeApprovePO", "ShowHideApprovePOInfo();", true);
    }

    protected void ddlApplyUserRolePermission_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ViewState["mode"] != null && ViewState["mode"].ToString() == "1")
        {
            // ddlApplyUserRolePermission.SelectedValue == "2" : --> Override user permission by user role permission
            if (ddlUserRole.SelectedValue != "0")
            {
                hdnApplyUserRolePermissionOrg.Value = ddlApplyUserRolePermission.SelectedValue;
                if (ddlApplyUserRolePermission.SelectedValue == "2")
                {
                    // Get role permission and fill for user permission
                    UserRole userRole = new UserRole();
                    userRole.ConnConfig = Session["config"].ToString();
                    userRole.RoleID = Convert.ToInt32(ddlUserRole.SelectedValue);
                    var ds = objBL_User.GetRoleByID(userRole);
                    FillRolePermission(ds.Tables[0]);
                }
                else if (ddlApplyUserRolePermission.SelectedValue == "1")
                {
                    UserRole userRole = new UserRole();
                    userRole.ConnConfig = Session["config"].ToString();
                    userRole.RoleID = Convert.ToInt32(ddlUserRole.SelectedValue);
                    var ds = objBL_User.GetRoleByID(userRole);
                    DataSet dsUser = new DataSet();
                    //dsUser = //(DataSet)ViewState["getUserById"];
                    User objPropUser = new User();
                    objPropUser.ConnConfig = Session["config"].ToString();
                    objPropUser.UserID = Convert.ToInt32(Request.QueryString["uid"]);
                    objPropUser.TypeID = Convert.ToInt32(Request.QueryString["type"]);
                    objPropUser.DBName = Session["dbname"].ToString();
                    dsUser = objBL_User.GetUserInfoByID(objPropUser);
                    if (dsUser != null && dsUser.Tables.Count > 0 && ds.Tables.Count > 0)
                    {
                        DataTable mergedDt = MergingPermissions(dsUser.Tables[0], ds.Tables[0]);
                        FillRolePermission(mergedDt);
                    }
                }
                else
                {
                    DataSet dsUser = new DataSet();
                    //dsUser = //(DataSet)ViewState["getUserById"];
                    User objPropUser = new User();
                    objPropUser.ConnConfig = Session["config"].ToString();
                    objPropUser.UserID = Convert.ToInt32(Request.QueryString["uid"]);
                    objPropUser.TypeID = Convert.ToInt32(Request.QueryString["type"]);
                    objPropUser.DBName = Session["dbname"].ToString();
                    dsUser = objBL_User.GetUserInfoByID(objPropUser);
                    FillRolePermission(dsUser.Tables[0]);
                }
            }
        }
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    private DataTable MergingPermissions(DataTable userInfor, DataTable rolePermission)
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
                    userPer["UserSales"] = MergePermissionString(userPer["UserSales"].ToString(), role["sales"].ToString());
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
                    //userPer["CompletedJObPermission"] = MergePermissionString(userPer["CompletedJObPermission"].ToString(), role["CompletedJObPermission"].ToString());
                    userPer["JobReopenPermission"] = MergePermissionString(userPer["JobReopenPermission"].ToString(), role["JobReopenPermission"].ToString());
                    userPer["Proposal"] = MergePermissionString(userPer["Proposal"].ToString(), role["Proposal"].ToString());

                    userPer["massreview"] = MergePermissionInt(Convert.ToInt32(userPer["massreview"]), Convert.ToInt32(role["massreview"]));

                    //userPer["ticketd"] = MergePermissionInt(Convert.ToInt32(userPer["ticketd"]), Convert.ToInt32(role["ticketd"]));
                    //userPer["ledger"] = MergePermissionInt(Convert.ToInt32(userPer["ledger"]), Convert.ToInt32(role["ledger"]));
                    //userPer["CPEquipment"] = MergePermissionInt(Convert.ToInt32(userPer["CPEquipment"]), Convert.ToInt32(role["CPEquipment"]));
                    //userPer["GroupbyWO"] = MergePermissionInt(Convert.ToInt32(userPer["GroupbyWO"]), Convert.ToInt32(role["GroupbyWO"]));
                    //userPer["openticket"] = MergePermissionInt(Convert.ToInt32(userPer["openticket"]), Convert.ToInt32(role["openticket"]));
                    userPer["IsProjectManager"] = MergePermissionInt(Convert.ToInt32(userPer["IsProjectManager"]), Convert.ToInt32(role["IsProjectManager"]));
                    userPer["IsAssignedProject"] = MergePermissionInt(Convert.ToInt32(userPer["IsAssignedProject"]), Convert.ToInt32(role["IsAssignedProject"]));
                    userPer["TicketVoidPermission"] = MergePermissionInt(Convert.ToInt32(userPer["TicketVoidPermission"]), Convert.ToInt32(role["TicketVoidPermission"]));
                    userPer["PR"] = MergePermissionInt(Convert.ToInt32(userPer["PR"]), Convert.ToInt32(role["PR"]));

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
                    userPer["Employee"] = MergePermissionString(userPer["Employee"].ToString(), role["Employee"].ToString());
                    userPer["PRProcess"] = MergePermissionString(userPer["PRProcess"].ToString(), role["PRProcess"].ToString());
                    userPer["PRRegister"] = MergePermissionString(userPer["PRRegister"].ToString(), role["PRRegister"].ToString());
                    userPer["PRReport"] = MergePermissionString(userPer["PRReport"].ToString(), role["PRReport"].ToString());
                    userPer["PRWage"] = MergePermissionString(userPer["PRWage"].ToString(), role["PRWage"].ToString());
                    userPer["PRDeduct"] = MergePermissionString(userPer["PRDeduct"].ToString(), role["PRDeduct"].ToString());
                    userPer["ticket"] = MergePermissionString(userPer["ticket"].ToString(), role["ticket"].ToString());

                    var uPOLimit = string.IsNullOrEmpty(userPer["POLimit"].ToString()) ? 0 : Convert.ToDecimal(userPer["POLimit"].ToString());
                    if (uPOLimit == 0)
                    {
                        //var urPOLimit = string.IsNullOrEmpty(role["POLimit"].ToString()) ? 0 : Convert.ToDecimal(role["POLimit"].ToString());
                        userPer["POLimit"] = role["POLimit"];
                    }
                    var uPOApprove = Convert.ToBoolean(userPer["POApprove"]);
                    var urPOApprove = Convert.ToBoolean(role["POApprove"]);
                    if (uPOApprove == false && urPOApprove)
                    {
                        userPer["POApproveAmt"] = role["POApproveAmt"];
                        userPer["MinAmount"] = role["MinAmount"];
                        userPer["MaxAmount"] = role["MaxAmount"];
                    }
                    userPer["POApprove"] = MergePermissionInt(Convert.ToInt32(userPer["POApprove"]), Convert.ToInt32(role["POApprove"]));
                }
            }
        }
        return userInfor;
    }

    private string MergePermissionString(string string1, string string2)
    {
        StringBuilder builder = new StringBuilder();
        var diffStr = "";
        var len = string1.Length;
        if (string1.Length > string2.Length)
        {
            len = string2.Length;

            diffStr = string1.Substring(len, string1.Length - len);

        }
        else if (string1.Length < string2.Length)
        {
            len = string1.Length;
            diffStr = string2.Substring(len, string2.Length - len);
        }

        var arr1 = string1.ToUpper().ToArray();
        var arr2 = string2.ToUpper().ToArray();
        for (int i = 0; i < len; i++)
        {
            if (arr1[i] != arr2[i] && arr2[i] == 'Y')
            {
                builder.Append('Y');
            }
            else
            {
                builder.Append(arr1[i]);
            }
        }

        builder.Append(diffStr);

        return builder.ToString();
    }

    private int MergePermissionInt(int int1, int int2)
    {
        if (int1 == 0) return int2;
        else return int1;
    }

    protected void chkMassPayrollTicket1_CheckedChanged(object sender, EventArgs e)
    {
        chkMassPayrollTicket.Checked = chkMassPayrollTicket1.Checked;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void chkMassPayrollTicket_CheckedChanged(object sender, EventArgs e)
    {
        chkMassPayrollTicket1.Checked = chkMassPayrollTicket.Checked;
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }
    
    protected void lnkChk_CheckedChanged(object sender, EventArgs e)
    {
        if (lnkChk.Checked)
        {
            ViewState["InInActiveWageUser"] = "True";

        }
        else
        {
            ViewState["InInActiveWageUser"] = "False";
        }

        gvWagePayRate.Rebind();
    }

    protected void RadGrid_EmailSigns_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (Request.QueryString["uid"] != null)
        {
            objPropUser.ID = Convert.ToInt32(Request.QueryString["uid"]);
            objPropUser.ConnConfig = Session["config"].ToString();
            var ds = objBL_User.GetUserEmailSignature(objPropUser);
            if (ds.Tables.Count > 0)
            {
                RadGrid_EmailSigns.DataSource = ds.Tables[0];
            }
        }
    }

    protected void lnkRefreshScreen_Click(object sender, EventArgs e)
    {
        RadGrid_EmailSigns.Rebind();

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void btnDeleteEmailSign_Click(object sender, EventArgs e)
    {
        try
        {
            EmailSignature eSignature = new EmailSignature();
            eSignature.ConnConfig = Session["Config"].ToString();

            foreach (GridDataItem item in RadGrid_EmailSigns.SelectedItems)
            {
                HiddenField hdnSignId = (HiddenField)item.FindControl("hdnSignId");

                eSignature.Id = Convert.ToInt32(hdnSignId.Value);
            }

            BL_User objBL_User = new BL_User();
            objBL_User.DeleteEmailSignature(eSignature);
            RadGrid_EmailSigns.Rebind();

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyDelSucc", "noty({text: 'Deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false,dismissQueue: true});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue: true});", true);
        }
    }


    #region Custom

    private void BindUserCustomGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable) ViewState["UserCustomTable"];

            gvUserCustom.DataSource = dt;
            gvUserCustom.VirtualItemCount = dt.Rows.Count;
            gvUserCustom.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private DataTable FillMembers(string userName)
    {
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        try
        {
            BusinessEntity.User objProp_User = new BusinessEntity.User();
            ds = objBL_User.getTeamByMonUser(Session["config"].ToString(), userName);
            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return dt;
    }
    
    //public DataTable getUserCustomValue(int testId, int equiId)
    //{
    //    DataSet ds = new DataSet();
    //    BL_SafetyTest _objbltesttypes = new BL_SafetyTest();
    //    ds = _objbltesttypes.getUserCustomValueByEquipTest(Session["config"].ToString(), testId, equiId);
    //    return ds.Tables[0];
    //}

    protected void gvUserCustom_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem) e.Item;
            HiddenField hdnUserItemValueID = (HiddenField) item.FindControl("hdnUserItemValueID");
            HiddenField hdnMembers = (HiddenField) item.FindControl("hdnMembers");
            TextBox txtMembers = (TextBox) item.FindControl("txtMembers");
            DropDownList ddlFormat = (DropDownList) item.FindControl("ddlFormat");
            CheckBox chkSelectAlert = (CheckBox) item.FindControl("chkSelectAlert");
            Label lbUpdatedBy = (Label) item.FindControl("lbUpdatedBy");
            Label lbUpdatedDate = (Label) item.FindControl("lbUpdatedDate");

            int format = Convert.ToInt32(DataBinder.Eval(item.DataItem, "Format"));
            int id = Convert.ToInt32(DataBinder.Eval(item.DataItem, "ID"));

            //Process data get from UserCustomFieldsValue
            DataTable dtCustomValue = (DataTable) ViewState["UserCustomFieldsValue"]; 
            String customValue = "";
            String teamMember = "";
            String RolesMember = "";
            Boolean isAlert = Convert.ToBoolean(DataBinder.Eval(item.DataItem, "IsAlert"));

            if (dtCustomValue.Rows.Count > 0)
            {
                if (dtCustomValue.Select("tblUserCustomFieldsID = " + id + "").Count() > 0)
                {
                    DataRow resultItemValue = dtCustomValue.Select("tblUserCustomFieldsID = " + id + "").First();
                    lbUpdatedBy.Text = resultItemValue["UpdatedBy"].ToString();
                    lbUpdatedDate.Text = resultItemValue["UpdatedDate"].ToString() == "" ? "" : Convert.ToDateTime(resultItemValue["UpdatedDate"]).ToString("MM/dd/yyyy hh:mm tt");
                    hdnUserItemValueID.Value = resultItemValue["ID"].ToString();
                    customValue = resultItemValue["Value"].ToString();
                    teamMember = resultItemValue["TeamMember"].ToString();
                    hdnMembers.Value = resultItemValue["TeamMember"].ToString();
                    txtMembers.Text = resultItemValue["TeamMemberDisplay"].ToString();
                    RolesMember = resultItemValue["UserRoles"].ToString();
                    isAlert = Convert.ToBoolean(resultItemValue["IsAlert"]);
                }
            }
            else
            {
                lbUpdatedBy.Text = "";
                lbUpdatedDate.Text = "";
                hdnUserItemValueID.Value = "0";
                teamMember = "";
            }

            chkSelectAlert.Checked = isAlert;

            switch (format)
            {
                case 1:
                    {
                        Panel divFormatCurrent = (Panel)item.FindControl("divFormatCurrent");
                        divFormatCurrent.Visible = true;
                        TextBox txtFormatCurrent = (TextBox)item.FindControl("txtFormatCurrent");
                        txtFormatCurrent.Text = customValue;
                        break;
                    }
                case 2:
                    {
                        Panel divFormatDate = (Panel)item.FindControl("divFormatDate");
                        divFormatDate.Visible = true;
                        TextBox txtFormatDate = (TextBox)item.FindControl("txtFormatDate");
                        txtFormatDate.Text = customValue;
                        break;
                    }
                case 3:
                    {
                        Panel divFormatText = (Panel)item.FindControl("divFormatText");
                        divFormatText.Visible = true;
                        TextBox txtFormatText = (TextBox)item.FindControl("txtFormatText");
                        txtFormatText.Text = customValue;
                        break;
                    }
                case 4:
                    {
                        Panel divFormatDrop = (Panel)item.FindControl("divFormatDrop");
                        divFormatDrop.Visible = true;
                        DropDownList drpdwnCustom = (DropDownList)item.FindControl("drpdwnCustom");
                        if (ViewState["UserCustomValues"] != null)
                        {
                            DataTable dtCustomval = (DataTable)ViewState["UserCustomValues"];
                            DataTable dataTemp = dtCustomval.Clone();

                            DataRow[] result = dtCustomval.Select("tblUserCustomFieldsID = " + id + "");
                            foreach (DataRow row in result)
                            {
                                dataTemp.ImportRow(row);
                            }

                            if (dataTemp.Rows.Count > 0)
                            {
                                dataTemp.DefaultView.Sort = "Value  ASC";
                                dataTemp = dataTemp.DefaultView.ToTable();
                            }

                            drpdwnCustom.DataSource = dataTemp;
                            drpdwnCustom.DataTextField = "Value";
                            drpdwnCustom.DataValueField = "Value";
                            drpdwnCustom.DataBind();
                            drpdwnCustom.Items.Insert(0, (new ListItem("--Select item--", "")));
                            drpdwnCustom.SelectedValue = customValue;
                        }
                        break;
                    }
                case 5:
                    {
                        Panel divFormatCheckbox = (Panel)item.FindControl("divFormatCheckbox");
                        divFormatCheckbox.Visible = true;
                        CheckBox chkCustomFormat = (CheckBox)item.FindControl("chkCustomFormat");
                        chkCustomFormat.Checked = customValue == "" ? false : Convert.ToBoolean(customValue);
                        break;
                    }
            }
        }
    }

    //public DataTable RemoveDuplicateRows(DataTable table, string DistinctColumn)
    //{
    //    try
    //    {
    //        ArrayList UniqueRecords = new ArrayList();
    //        ArrayList DuplicateRecords = new ArrayList();
    //        foreach (DataRow dRow in table.Rows)
    //        {
    //            if (UniqueRecords.Contains(dRow[DistinctColumn]))
    //                DuplicateRecords.Add(dRow);
    //            else
    //                UniqueRecords.Add(dRow[DistinctColumn]);
    //        }

    //        foreach (DataRow dRow in DuplicateRecords)
    //        {
    //            table.Rows.Remove(dRow);
    //        }

    //        return table;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //}

    //protected void RadComboBox1_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    //{
    //    RadComboBox obj = sender as RadComboBox;
    //    obj.Items.Clear();

    //    obj.ClearCheckedItems();
    //    obj.ClearSelection();

    //    obj.DataSource = FillMembers(e.Text);
    //    obj.DataBind();
    //}

    //protected void RadComboBox1_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    //{
    //    e.Item.Text = ((DataRowView)e.Item.DataItem)["MomUserID"].ToString();
    //    e.Item.Value = ((DataRowView)e.Item.DataItem)["ID"].ToString();
    //}

    protected void gvCustom_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridDataItem item = (GridDataItem) e.Item;
            LinkButton lnkAddCustomValue = (LinkButton) item.FindControl("lnkAddUserCustomValue");
            LinkButton lnkDelCustomValue = (LinkButton) item.FindControl("lnkDelUserCustomValue");
            LinkButton lnkUpdateCustomValue = (LinkButton)item.FindControl("lnkUpdateUserCustomValue");

            if (e.CommandName.Equals("AddUserCustomValue"))
            {
                TextBox txtCustomValue = (TextBox)item.FindControl("txtUserCustomValue");
                DropDownList ddlCustomValue = (DropDownList)item.FindControl("ddlUserCustomValue");

                Boolean isExistItem = false;
                foreach (ListItem x in ddlCustomValue.Items)
                {
                    if (x.Text == txtCustomValue.Text.Trim())
                    {
                        isExistItem = true;
                        break;
                    }
                }

                if (txtCustomValue.Text.Trim() != string.Empty && !isExistItem)
                {
                    ddlCustomValue.Items.Add(new ListItem(txtCustomValue.Text.Trim(), txtCustomValue.Text.Trim()));
                    txtCustomValue.Text = string.Empty;
                    ddlCustomValue.SelectedValue = txtCustomValue.Text.Trim();
                }
            }
            else if (e.CommandName.Equals("UpdateUserCustomValue"))
            {
                TextBox txtCustomValue = (TextBox)item.FindControl("txtUserCustomValue");
                DropDownList ddlCustomValue = (DropDownList)item.FindControl("ddlUserCustomValue");
                if (txtCustomValue.Text.Trim() != string.Empty)
                {
                    ddlCustomValue.Items.Remove(new ListItem(ddlCustomValue.SelectedValue, ddlCustomValue.SelectedValue));
                    ddlCustomValue.Items.Add(new ListItem(txtCustomValue.Text.Trim(), txtCustomValue.Text.Trim()));
                    ddlCustomValue.SelectedValue = txtCustomValue.Text.Trim();
                }
                lnkAddCustomValue.Visible = true;
                lnkUpdateCustomValue.Visible = false;
                lnkDelCustomValue.Visible = false;
                txtCustomValue.Text = string.Empty;
            }
            else if (e.CommandName.Equals("DeleteUserCustomValue"))
            {
                TextBox txtCustomValue = (TextBox)item.FindControl("txtUserCustomValue");
                DropDownList ddlCustomValue = (DropDownList)item.FindControl("ddlUserCustomValue");
                ddlCustomValue.Items.Remove(new ListItem(ddlCustomValue.SelectedValue, ddlCustomValue.SelectedValue));
                ddlCustomValue.SelectedIndex = 0;
                lnkAddCustomValue.Visible = true;
                lnkUpdateCustomValue.Visible = false;
                lnkDelCustomValue.Visible = false;
                txtCustomValue.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void CreateUserCustomTable()
    {
        DataSet dst = new DataSet();
        BL_UserCustom _objblusercustom = new BL_UserCustom();
        dst = _objblusercustom.GetAllUserCustom(Session["config"].ToString(), Session["dbname"].ToString());

        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("OrderNo", typeof(int));
        dt.Columns.Add("Label", typeof(string));
        dt.Columns.Add("IsAlert", typeof(Boolean));
        dt.Columns.Add("TeamMember", typeof(string));
        dt.Columns.Add("Format", typeof(int));
        dt.Columns.Add("TeamMemberDisplay", typeof(string));
        dt.Columns.Add("UserRoles", typeof(string));
        dt.Columns.Add("UserRolesDisplay", typeof(string));
        DataRow dr = dt.NewRow();

        dr["ID"] = 0;
        dr["Line"] = dt.Rows.Count + 1;
        dr["OrderNo"] = 0;
        dr["Label"] = "";
        dr["IsAlert"] = 0;
        dr["TeamMember"] = "";
        dr["Format"] = 0;
        dr["TeamMemberDisplay"] = "";
        dr["UserRoles"] = "";
        dr["UserRolesDisplay"] = "";
        dt.Rows.Add(dr);

        if (dst.Tables[0].Rows.Count == 0)
        {
            ViewState["UserCustomTable"] = dt;
        }
        else
        {
            ViewState["UserCustomTable"] = dst.Tables[0];
        }

        ViewState["UserCustomValues"] = dst.Tables[1];

        //get User Custom Fields value
        int userID = 0;
        if (Request.QueryString["uid"] != null)
        {
            userID = Convert.ToInt32(Request.QueryString["uid"]);
        }
        DataSet dsUserCustomFieldsValue = new DataSet();
        dsUserCustomFieldsValue = _objblusercustom.GetUserCustomFieldValue(Session["config"].ToString(), userID);
        ViewState["UserCustomFieldsValue"] = dsUserCustomFieldsValue.Tables[0];
    }

    protected void RadGrid_Emails_PreRender(object sender, EventArgs e)
    {
        String filterExpression = Convert.ToString(RadGrid_Emails.MasterTableView.FilterExpression);
        if (filterExpression != "")
        {
            Session["Emails_FilterExpression"] = filterExpression;
            List<RetainFilter> filters = new List<RetainFilter>();

            foreach (GridColumn column in RadGrid_Emails.MasterTableView.OwnerGrid.Columns)
            {
                String filterValues = column.CurrentFilterValue;
                if (filterValues != "")
                {
                    String columnName = column.UniqueName;
                    RetainFilter filter = new RetainFilter();
                    filter.FilterColumn = columnName;
                    filter.FilterValue = filterValues;
                    filters.Add(filter);
                }
            }

            Session["Emails_Filters"] = filters;
        }
        else
        {
            Session["Emails_FilterExpression"] = null;
            Session["Emails_Filters"] = null;
        }

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "bindingClickCheckbox", "BindClickEventForGridCheckBox();", true);
    }

    protected void RadGrid_Emails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_Emails.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        if (!IsPostBack)
        {
            if (Session["Emails_FilterExpression"] != null && Convert.ToString(Session["Emails_FilterExpression"]) != "" && Session["Emails_Filters"] != null)
            {
                RadGrid_Emails.MasterTableView.FilterExpression = Convert.ToString(Session["Emails_FilterExpression"]);
                var filtersGet = Session["Emails_Filters"] as List<RetainFilter>;
                if (filtersGet != null)
                {
                    foreach (var _filter in filtersGet)
                    {
                        GridColumn column = RadGrid_Emails.MasterTableView.GetColumnSafe(_filter.FilterColumn);
                        column.CurrentFilterValue = _filter.FilterValue;
                    }
                }
            }
        }

        InitTeamMemberGridView();
        //FillDistributionList("", "");
    }

    private void InitTeamMemberGridView()
    {
        User objPropUser = new User();
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        //objPropUser.Status = 0;
        // ds = objBL_User.GetUsersForTeamMemberList(objPropUser);
        ds = objBL_User.GetUsersAndRolesForTeamMemberList(objPropUser);
        var teamMembers = ds.Tables[0];

        // Get contacts list from exchange server
        //DataTable contactList = GetContactsForExchServer();
        DataTable contactList = WebBaseUtility.GetContactListOnExchangeServer();
        if (contactList != null && contactList.Rows.Count > 0)
        {
            // Merge this list to teamMembers
            foreach (DataRow item in contactList.Rows)
            {
                DataRow _dr = teamMembers.NewRow();
                if (string.IsNullOrEmpty(item["GroupName"].ToString()))
                {
                    _dr["memberkey"] = "3_" + item["Type"] + "|" + item["MemberEmail"] + "|" + item["MemberName"];
                    _dr["usertype"] = "Exchange " + item["Type"];
                }
                else
                {
                    _dr["memberkey"] = "4_" + item["GroupName"] + "|" + item["MemberEmail"] + "|" + item["MemberName"];
                    _dr["usertype"] = "Exchange Group: " + item["GroupName"];
                }
                _dr["fUser"] = item["MemberName"];
                _dr["email"] = item["MemberEmail"];
                _dr["roleName"] = "";
                teamMembers.Rows.Add(_dr);
            }
        }
        ViewState["AllProjectTeamMemberList"] = teamMembers;
        RadGrid_Emails.DataSource = teamMembers;
        if (teamMembers != null && teamMembers.Rows.Count > 0)
        {
            RadGrid_Emails.VirtualItemCount = teamMembers.Rows.Count;
        }
        else
        {
            RadGrid_Emails.VirtualItemCount = 0;
        }
    }

    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_Emails.MasterTableView.FilterExpression != "" ||
            (RadGrid_Emails.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_Emails.MasterTableView.SortExpressions.Count > 0;
    }

    private void processCustomTable(User obj)
    {
        obj.Cus_CreateTask = new List<NotificationUserCustomChange>();
        obj.Cus_EmailToTeamMember = new List<NotificationUserCustomChange>();
        //int equiId = hdnEquipment.Value == "" ? 0 : Convert.ToInt32(hdnEquipment.Value);
        int userID = 0;

        if (Request.QueryString["uid"] != null)
        {
            userID = Convert.ToInt32(Request.QueryString["uid"]);
        }
        
        DataTable dtCustomValue = new DataTable();
        dtCustomValue.Columns.Add("ID", typeof(int));
        dtCustomValue.Columns.Add("tblUserCustomFieldsID", typeof(int));
        dtCustomValue.Columns.Add("Value", typeof(string));
        dtCustomValue.Columns.Add("UpdatedBy", typeof(string));
        dtCustomValue.Columns.Add("UserID", typeof(int));
        //dtCustomValue.Columns.Add("EquipmentID", typeof(int));
        dtCustomValue.Columns.Add("IsAlert", typeof(Boolean));
        dtCustomValue.Columns.Add("TeamMember", typeof(string));
        dtCustomValue.Columns.Add("TeamMemberDisplay", typeof(string));
        dtCustomValue.Columns.Add("UserRoles", typeof(string));
        dtCustomValue.Columns.Add("UserRolesDisplay", typeof(string));

        NotificationUserCustomChange notification = new NotificationUserCustomChange();
        NotificationUserCustomChange createTask = new NotificationUserCustomChange();

        foreach (GridDataItem gr in gvUserCustom.Items)
        {
            HiddenField hdSelectTeam = (HiddenField) gr.FindControl("hdnMembers");
            TextBox txtMembers = (TextBox) gr.FindControl("txtMembers");
            CheckBox chkSelectAlert = (CheckBox) gr.FindControl("chkSelectAlert");
            Label lblFormat = (Label) gr.FindControl("lblFormat");
            Label lblID = (Label) gr.FindControl("lblID");
            Label lblCustom = (Label) gr.FindControl("lblCustom");
            HiddenField hdnUserItemValueID = (HiddenField) gr.FindControl("hdnUserItemValueID");

            DataRow dr = dtCustomValue.NewRow();
            dr["ID"] = hdnUserItemValueID.Value == "" ? 0 : Convert.ToInt32(hdnUserItemValueID.Value);
            dr["tblUserCustomFieldsID"] = Convert.ToInt32(lblID.Text);
            int format = Convert.ToInt32(lblFormat.Text);

            switch (format)
            {
                case 1:
                    {
                        TextBox txtFormatCurrent = (TextBox)gr.FindControl("txtFormatCurrent");
                        dr["Value"] = txtFormatCurrent.Text;
                        break;
                    }
                case 2:
                    {
                        TextBox txtFormatDate = (TextBox)gr.FindControl("txtFormatDate");
                        dr["Value"] = txtFormatDate.Text;
                        break;
                    }
                case 3:
                    {
                        TextBox txtFormatText = (TextBox)gr.FindControl("txtFormatText");
                        dr["Value"] = txtFormatText.Text;
                        break;
                    }
                case 4:
                    {
                        DropDownList drpdwnCustom = (DropDownList)gr.FindControl("drpdwnCustom");
                        dr["Value"] = drpdwnCustom.SelectedValue;
                        break;
                    }
                case 5:
                    {
                        CheckBox chkCustomFormat = (CheckBox)gr.FindControl("chkCustomFormat");
                        dr["Value"] = chkCustomFormat.Checked;
                        break;
                    }
            }

            dr["UpdatedBy"] = Session["username"].ToString();
            dr["UserID"] = userID;
            //dr["EquipmentID"] = equiId;
            dr["IsAlert"] = Convert.ToBoolean(chkSelectAlert.Checked);
            dr["TeamMember"] = hdSelectTeam.Value;
            dr["TeamMemberDisplay"] = txtMembers.Text;

            dtCustomValue.Rows.Add(dr);

            //Email

            if (Request.QueryString["uid"] != null)
            {
                //check data change
                DataTable oldData = (DataTable) ViewState["UserCustomFieldsValue"];

                String sql = "UserID = " + userID + " and tblUserCustomFieldsID = " + lblID.Text;
                int lsChange = oldData.Select(sql).Count();
                if ((lsChange == 0) && ((dr["Value"].ToString() == "") || (dr["Value"].ToString() == "False")))
                {
                    lsChange = 1;
                }
                else
                { 
                    sql = "UserID = " + userID + " and tblUserCustomFieldsID = " + lblID.Text + " and Value ='" + dr["Value"] + "'";
                    lsChange = oldData.Select(sql).Count();
                }
                if (lsChange == 0)
                {
                    notification = new NotificationUserCustomChange();
                    //notification.SubjectEmail = txtAccount.Text + " - Equip ID " + hdnEquipment.Value + "Test Type " + ddlTestTypes.SelectedItem.Text + " Alert";
                    notification.SubjectEmail = txtUserName.Text + " Alert";
                    notification.UserName = Session["username"].ToString();
                    notification.label = lblCustom.Text + " with value = '" + dr["Value"] + "'";

                    createTask = new NotificationUserCustomChange();
                    //createTask.SubjectEmail = txtAccount.Text + " - Equip ID " + hdnEquipment.Value + "Test Type " + ddlTestTypes.SelectedItem.Text + " Alert";
                    createTask.SubjectEmail = txtUserName.Text + " Alert";
                    createTask.UserName = Session["username"].ToString();
                    createTask.label = lblCustom.Text + " with value = '" + dr["Value"] + "'";

                    List<String> ls = hdSelectTeam.Value.ToString().Split(';').ToList();

                    foreach (string item in ls)
                    {
                        string[] arr = item.Split('_');
                        if (arr.Length > 2)
                        {
                            if (arr[2] == "1")
                            {
                                if (arr[0] == "6")
                                {
                                    createTask.lsRole = createTask.lsRole + ";" + arr[0] + "_" + arr[1];
                                }
                                else
                                {
                                    createTask.lsTeamMember = createTask.lsTeamMember + ";" + arr[0] + "_" + arr[1];
                                }
                            }
                            else
                            {
                                if (chkSelectAlert.Checked)
                                {
                                    if (arr[0] == "6")
                                    {
                                        notification.lsRole = notification.lsRole + ";" + arr[0] + "_" + arr[1];
                                    }
                                    else
                                    {
                                        notification.lsTeamMember = notification.lsTeamMember + ";" + arr[0] + "_" + arr[1];
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (chkSelectAlert.Checked && arr.Length > 1)
                            {
                                if (arr[0] == "6")
                                {
                                    notification.lsRole = notification.lsRole + ";" + arr[0] + "_" + arr[1];
                                }
                                else
                                {
                                    notification.lsTeamMember = notification.lsTeamMember + ";" + arr[0] + "_" + arr[1];
                                }
                            }
                        }
                    }

                    obj.Cus_CreateTask.Add(createTask);
                    obj.Cus_EmailToTeamMember.Add(notification);
                }
            }
        }
        obj.Cus_UserCustomValue = dtCustomValue;
    }

    private void processCreateTask(User obj)
    {
        DataSet ds = new DataSet();
        BusinessEntity.User objProp_User = new BusinessEntity.User();
        DataTable lstProjectTeamMember = (DataTable) ViewState["AllProjectTeamMemberList"];

        foreach (NotificationUserCustomChange item in obj.Cus_CreateTask)
        {
            if (!String.IsNullOrEmpty(item.lsTeamMember))
            {
                List<String> ls = item.lsTeamMember.Substring(1).Split(';').ToList();
                foreach (string objUser in ls)
                {
                    var changedItem = lstProjectTeamMember.Select("memberkey='" + objUser + "'").FirstOrDefault();
                    if (changedItem != null)
                    {
                        CreateTaskOnWorkflowChange(item.SubjectEmail, item.EmailContent(), (string)changedItem["fUser"], (string)changedItem["email"]);
                    }
                }
            }

            if (!String.IsNullOrEmpty(item.lsRole))
            {
                List<String> ls = item.lsRole.Substring(1).Split(';').ToList();
                foreach (string objUser in ls)
                {
                    var changedItem = lstProjectTeamMember.Select("memberkey='" + objUser + "'").FirstOrDefault();
                    if (changedItem != null)
                    {
                        var role = changedItem["RoleName"].ToString();
                        var projTeamUsers = lstProjectTeamMember.Select("RoleName='" + role.Trim() + "'").ToList();
                        foreach (var projUser in projTeamUsers)
                        {
                            CreateTaskOnWorkflowChange(item.SubjectEmail, item.EmailContent(), (string)projUser["fUser"], (string)projUser["email"]);
                        }
                    }
                }
            }
        }
    }

    private void processSendMail(User obj)
    {
        List<String> lsEmail;
        List<String> ls;
        DataSet ds = new DataSet();
        BusinessEntity.User objProp_User = new BusinessEntity.User();
        DataTable lstProjectTeamMember = (DataTable)ViewState["AllProjectTeamMemberList"];

        foreach (NotificationUserCustomChange item in obj.Cus_EmailToTeamMember)
        {
            lsEmail = new List<string>();
            if (!String.IsNullOrEmpty(item.lsTeamMember))
            {
                ls = item.lsTeamMember.Substring(1).Split(';').ToList();
                foreach (string objUser in ls)
                {
                    var changedItem = lstProjectTeamMember.Select("memberkey='" + objUser + "'").FirstOrDefault();
                    lsEmail.Add((string)changedItem["email"]);
                }
            }

            if (!String.IsNullOrEmpty(item.lsRole))
            {
                ls = item.lsRole.Substring(1).Split(';').ToList();
                foreach (string objUser in ls)
                {
                    var changedItem = lstProjectTeamMember.Select("memberkey='" + objUser + "'").FirstOrDefault();
                    var role = changedItem["RoleName"].ToString();
                    var projTeamUsers = lstProjectTeamMember.Select("RoleName='" + role.Trim() + "'").ToList();
                    foreach (var projUser in projTeamUsers)
                    {
                        lsEmail.Add((string)projUser["email"]);
                    }
                }
            }

            if (lsEmail.Count > 0)
            {
                Mail mail = new Mail();
                mail.From = WebBaseUtility.GetFromEmailAddress();
                mail.To = lsEmail;
                mail.Title = item.SubjectEmail;
                mail.Text = item.EmailContent();
                try
                {
                    WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                    mail.Send();
                }
                catch (Exception ex)
                {
                    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
        }
    }

    private void CreateTaskOnWorkflowChange(string strSubject, string strRemarks, string assignedTo, string strMailTo = "")
    {
        var objCustomer = new Customer();
        objCustomer.ConnConfig = Session["config"].ToString();
        objCustomer.ROL = objPropUser.RolId; 
        objCustomer.DueDate = DateTime.Now;
        objCustomer.TimeDue = Convert.ToDateTime("01/01/1900 " + DateTime.Now.ToShortTimeString());
        objCustomer.Subject = strSubject;
        objCustomer.Remarks = strRemarks;
        objCustomer.AssignedTo = assignedTo;
        double dblDuration = 0.5;
        objCustomer.Duration = dblDuration;
        objCustomer.Name = Session["Username"].ToString();
        objCustomer.Status = 0;//Open
        objCustomer.Resolution = "";
        objCustomer.LastUpdateUser = Session["username"].ToString();
        objCustomer.Category = "To Do";
        objCustomer.IsAlert = true;

        try
        {
            objCustomer.TaskID = 0;
            objCustomer.Mode = 0;
            objCustomer.Screen = "User";
            objCustomer.Ref = objPropUser.EmpId;

            objBL_Customer.AddTask(objCustomer);

            #region Send email with a appointment to login user 
            if (objCustomer.IsAlert)
            {
                // Create
                BusinessEntity.User objPropUser = new BusinessEntity.User();
                BL_User objBL_User = new BL_User();
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.Username = assignedTo;

                var mailTo = string.Empty;
                if (!string.IsNullOrEmpty(strMailTo))
                {
                    mailTo = strMailTo;
                }
                else
                {
                    mailTo = objBL_User.getUserEmail(objPropUser);
                }

                if (!string.IsNullOrEmpty(mailTo))
                {
                    Mail mail = new Mail();
                    try
                    {
                        var uri = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, Request.ApplicationPath + "/addTask?uid=" + objCustomer.TaskID.ToString());
                        foreach (var toaddress in mailTo.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            mail.To.Add(toaddress.Trim());
                        }
                        mail.From = WebBaseUtility.GetFromEmailAddress();
                        mail.Title = objCustomer.Subject;

                        //*StringBuilder stringBuilder = new StringBuilder();
                        //stringBuilder.AppendFormat("Dear {0}<br><br>", objCustomer.AssignedTo);
                        //stringBuilder.Append("You are receiving an appointment task from MOM-->Sales-->Tasks<br><br>");
                        //stringBuilder.AppendFormat("Location Name: {0}<br>", txtAccount.Text);

                        //stringBuilder.AppendFormat("Subject: {0}<br>", objCustomer.Subject);
                        //stringBuilder.AppendFormat("Description: {0}<br>", objCustomer.Remarks);
                        //stringBuilder.AppendFormat("Due on: {0} {1}<br><br>", objCustomer.DueDate.ToString("MM/dd/yyyy"), DateTime.Now.ToShortTimeString());
                        //stringBuilder.Append("Attached files is a task appointment assigned to you.<br>");
                        //stringBuilder.Append("To add this appointment to your calendar, please open and save it<br><br>");
                        //stringBuilder.AppendFormat("<a href={0}>{0}</a><br><br>", uri);
                        //stringBuilder.Append("Thanks");

                        //*mail.Text = stringBuilder.ToString();

                        mail.Text = objCustomer.Remarks;

                        WebBaseUtility.UpdateMailConfigurationFromLoginUser(mail);
                        //*var apSubject = string.Format("Task name: {0}", txtAccount.Text);

                        //*StringBuilder apBody = new StringBuilder();
                        //var _strRemarks = objCustomer.Remarks.Replace("\r\n", "=0D=0A").Replace("\n", "=0D=0A");
                        //apBody.AppendFormat("{0}.=0D=0A", _strRemarks);
                        //apBody.AppendFormat("Due on: {0} {1}. =0D=0A ", objCustomer.DueDate.ToString("MM/dd/yyyy"), DateTime.Now.ToShortTimeString());
                        //apBody.Append("Attached files is a task appointment assigned to you.  =0D=0A");
                        //apBody.Append("To add this appointment to your calendar, please open and save it.=0D=0A");
                        //apBody.Append("Thanks");


                        //*var strStartDate = string.Format("{0} {1}", objCustomer.DueDate.ToString("MM/dd/yyyy"), DateTime.Now.ToShortTimeString());
                        //var apStart = Convert.ToDateTime(strStartDate);
                        //var apEnd = apStart.AddHours(objCustomer.Duration);

                        //*var icsAttachmentContentsStr = WebBaseUtility.CreateICSAttachmentCalendarStr(apSubject
                        //    , apBody.ToString()
                        //    , txtAccount.Text
                        //    , apStart
                        //    , apEnd
                        //    , 60
                        //    );
                        //var myByteArray = System.Text.Encoding.UTF8.GetBytes(icsAttachmentContentsStr);
                        //mail.attachmentBytes = myByteArray;
                        //mail.FileName = "TaskAppointment.ics";

                        mail.Send();
                    }
                    catch (Exception ex)
                    {
                        string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
            }
            #endregion
        }
        catch (Exception ex)
        {
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    #endregion


    #region Documents

    protected void lnkDeleteDoc_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid_Documents.SelectedItems)
        {
            Label lblID = (Label)item.FindControl("lblId");
            HiddenField hdnTempId = (HiddenField)item.FindControl("hdnTempId");
            if (lblID.Text == "0")
            {
                DeleteDocFromTempTable(hdnTempId.Value);
            }

            DeleteFileFromFolder(string.Empty, Convert.ToInt32(lblID.Text));
        }

        //ScriptManager.RegisterStartupScript(this, GetType(), "DeleteDoc", "$('.dropify').dropify();", true);
    }

    /// <summary>
    /// Add Attachment
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkUploadDoc_Click(object sender, EventArgs e)
    {
        try
        {
            string filename = string.Empty;
            string fullpath = string.Empty;
            string mime = string.Empty;
            var savepath = string.Empty;

            var mainDirectory = "UserDocs";
            
            if (!string.IsNullOrEmpty(Request.QueryString["uid"]))
            {
                mainDirectory += "\\User_" + Request.QueryString["uid"];
            }
            else
            {
                if (ViewState["TempUploadDirectory"] == null)
                {
                    ViewState["TempUploadDirectory"] = Guid.NewGuid().ToString("N");
                }

                mainDirectory += "\\" + ViewState["TempUploadDirectory"] as string;
            }

            savepath = GetUploadDirectory(mainDirectory);
            if (Request.QueryString["uid"] != null)
            {
                objMapData.Screen = "User";
                objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                objMapData.TempId = "0";

                foreach (HttpPostedFile postedFile in FileUpload1.PostedFiles)
                {
                    filename = postedFile.FileName;
                    fullpath = savepath + filename;
                    mime = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).Substring(1);

                    if (File.Exists(fullpath))
                    {
                        GeneralFunctions objGeneralFunctions = new GeneralFunctions();
                        filename = objGeneralFunctions.generateRandomString(4) + "_" + filename;
                        fullpath = savepath + filename;
                    }

                    using (new NetworkConnection())
                    {
                        if (!Directory.Exists(savepath))
                        {
                            Directory.CreateDirectory(savepath);
                        }

                        postedFile.SaveAs(fullpath);
                    }

                    objMapData.FileName = filename;
                    objMapData.DocTypeMIME = mime;
                    objMapData.FilePath = fullpath;

                    objMapData.DocID = 0;
                    objMapData.Mode = 0;
                    objMapData.ConnConfig = Session["config"].ToString();
                    objMapData.Worker = Session["User"].ToString();
                    objBL_MapData.AddFile(objMapData);
                }

                UpdateDocInfo();
                RadGrid_Documents.Rebind();
            }
            else
            {
                var tempTable = new DataTable();
                foreach (HttpPostedFile postedFile in FileUpload1.PostedFiles)
                {
                    filename = postedFile.FileName;
                    fullpath = savepath + filename;
                    mime = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).Substring(1);

                    if (File.Exists(fullpath))
                    {
                        GeneralFunctions objGeneralFunctions = new GeneralFunctions();
                        filename = objGeneralFunctions.generateRandomString(4) + "_" + filename;
                        fullpath = savepath + filename;
                    }

                    using (new NetworkConnection())
                    {
                        if (!Directory.Exists(savepath))
                        {
                            Directory.CreateDirectory(savepath);
                        }

                        postedFile.SaveAs(fullpath);
                    }

                    tempTable = SaveAttachedFilesWhenAddingUser(filename, fullpath, mime);
                }

                RadGrid_Documents.DataSource = tempTable;
                RadGrid_Documents.VirtualItemCount = tempTable.Rows.Count;
                RadGrid_Documents.DataBind();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private string GetUploadDirectory(string mainDirectory)
    {
        var savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
        return savepathconfig + @"\" + Session["dbname"] + @"\" + mainDirectory + @"\";
    }

    private void UpdateDocInfo()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.dtDocs = SaveDocInfo();
        objPropUser.Username = Session["User"].ToString();
        objBL_User.UpdateDocInfo(objPropUser);
    }

    private void GetDocuments()
    {
        if (Request.QueryString["uid"] != null)
        {
            objMapData.Screen = "User";
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objMapData.TempId = "0";
            objMapData.Mode = 1;
            objMapData.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            ds = objBL_MapData.GetDocuments(objMapData);
            //gvDocuments.DataSource = ds.Tables[0];
            //gvDocuments.DataBind();
            RadGrid_Documents.DataSource = ds.Tables[0];
            RadGrid_Documents.VirtualItemCount = ds.Tables[0].Rows.Count;
            //RadGrid_Documents.DataBind();
        }
        else
        {
            var source = ViewState["AttachedFiles"] as DataTable;
            pnlDocumentButtons.Visible = true;
            RadGrid_Documents.DataSource = source;
            RadGrid_Documents.VirtualItemCount = source != null ? source.Rows.Count : 0;
            //RadGrid_Documents.DataBind();
        }
    }

    private DataTable SaveDocInfo()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Portal", typeof(int));
        dt.Columns.Add("Remarks", typeof(string));
        dt.Columns.Add("MSVisible", typeof(byte));

        foreach (GridDataItem item in RadGrid_Documents.Items)
        {
            Label lblID = (Label)item.FindControl("lblID");
            TextBox txtRemarks = (TextBox)item.FindControl("txtRemarks");
            //CheckBox chkMSVisible = (CheckBox)item.FindControl("chkMSVisible");
            DataRow dr = dt.NewRow();
            dr["ID"] = lblID.Text;
            dr["Portal"] = false;//chkPortal.Checked;
            dr["Remarks"] = txtRemarks.Text;
            //dr["MSVisible"] = chkMSVisible.Checked;
            dr["MSVisible"] = false;
            dt.Rows.Add(dr);
        }

        return dt;
    }

    private void DeleteFile(int DocumentID)
    {
        try
        {
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.DocumentID = DocumentID;
            objMapData.Worker = Session["User"].ToString();
            objBL_MapData.DeleteFile(objMapData);
            UpdateDocInfo();
            //GetDocuments();
            RadGrid_Documents.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrdelete", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    public void DeleteFileFromFolder(string StrFilename, int DocumentID)
    {
        try
        {
            DeleteFile(DocumentID);
        }
        catch (FileNotFoundException ex)
        {
            DeleteFile(DocumentID);
        }
        catch (UnauthorizedAccessException ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(),
            "FileDeleteAccessWarning", "noty({text: 'Please provide delete permissions to the file path.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            ScriptManager.RegisterStartupScript(this, GetType(),
            "FileDeleteErrorWarning", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void DeleteDocFromTempTable(string tempId)
    {
        if (string.IsNullOrWhiteSpace(tempId))
        {
            return;
        }

        var tempAttachedFiles = ViewState["AttachedFiles"] as DataTable;

        if (tempAttachedFiles == null)
        {
            return;
        }

        var deleteFileRow = tempAttachedFiles.AsEnumerable().FirstOrDefault(t => t.Field<string>("TempId") == tempId);

        if (deleteFileRow != null)
        {
            tempAttachedFiles.Rows.Remove(deleteFileRow);
        }
    }

    private DataTable SaveAttachedFilesWhenAddingUser(string fileName, string fullPath, string doctype)
    {
        var tempAttachedFiles = ViewState["AttachedFiles"] as DataTable;

        if (tempAttachedFiles == null)
        {
            tempAttachedFiles = new DataTable();
            tempAttachedFiles.Columns.Add("id", typeof(int));
            tempAttachedFiles.Columns.Add("filename", typeof(string));
            tempAttachedFiles.Columns.Add("doctype", typeof(string));
            tempAttachedFiles.Columns.Add("Portal", typeof(bool));
            tempAttachedFiles.Columns.Add("Path", typeof(string));
            tempAttachedFiles.Columns.Add("remarks", typeof(string));
            tempAttachedFiles.Columns.Add("MSVisible", typeof(byte));
            tempAttachedFiles.Columns.Add("TempId", typeof(string));
            ViewState["AttachedFiles"] = tempAttachedFiles;
        }

        var row = tempAttachedFiles.NewRow();
        row["id"] = 0;
        row["filename"] = fileName;
        row["doctype"] = doctype;
        row["Portal"] = false;
        row["Path"] = fullPath;
        row["remarks"] = string.Empty;
        row["MSVisible"] = false;
        row["TempId"] = Guid.NewGuid().ToString("N");
        tempAttachedFiles.Rows.Add(row);
        return tempAttachedFiles;
    }

    protected void lblName_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;

        string[] CommandArgument = btn.CommandArgument.Split(',');

        string FileName = CommandArgument[0];

        string FilePath = CommandArgument[1];

        DownloadDocument(FilePath, FileName);
    }

    private void DownloadDocument(string filePath, string DownloadFileName)
    {
        try
        {
            using (new NetworkConnection())
            {
                System.IO.FileInfo FileName = new System.IO.FileInfo(filePath);
                FileStream myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader _BinaryReader = new BinaryReader(myFile);

                try
                {
                    long startBytes = 0;
                    string lastUpdateTiemStamp = File.GetLastWriteTimeUtc(filePath).ToString("r");
                    string _EncodedData = HttpUtility.UrlEncode(DownloadFileName, System.Text.Encoding.UTF8) + lastUpdateTiemStamp;

                    Response.Clear();
                    Response.Buffer = false;
                    Response.AddHeader("Accept-Ranges", "bytes");
                    Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                    Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);
                    Response.ContentType = "application/octet-stream";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFileName));
                    Response.AddHeader("Content-Length", (FileName.Length - startBytes).ToString());
                    Response.AddHeader("Connection", "Keep-Alive");
                    Response.ContentEncoding = System.Text.Encoding.UTF8;

                    //Send data
                    _BinaryReader.BaseStream.Seek(startBytes, SeekOrigin.Begin);

                    //Dividing the data in 1024 bytes package
                    int maxCount = (int)Math.Ceiling((FileName.Length - startBytes + 0.0) / 1024);

                    //Download in block of 1024 bytes
                    int i;
                    for (i = 0; i < maxCount && Response.IsClientConnected; i++)
                    {
                        Response.BinaryWrite(_BinaryReader.ReadBytes(1024));
                        Response.Flush();
                    }
                    ////if blocks transfered not equals total number of blocks
                    //if (i < maxCount)
                    //    return false;
                    //return true; 
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    Response.End();
                    _BinaryReader.Close();
                    myFile.Close();
                }
            }
        }
        catch (FileNotFoundException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('File not found.');", true);
        }
        catch (UnauthorizedAccessException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('Please provide access permissions to the file path.');", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileerrorWarning", "alert('" + str + "');", true);
        }
    }

    private void DocumentPermission()
    {
        if (Convert.ToString(Session["type"]) != "am" && Convert.ToString(Session["type"]) != "c")
        {
            DataTable ds = new DataTable();
            ds = (DataTable)Session["userinfo"];

            //Document--------------------->

            string DocumentPermission = ds.Rows[0]["DocumentPermission"] == DBNull.Value ? "YYYY" : ds.Rows[0]["DocumentPermission"].ToString();
            hdnAddeDocument.Value = DocumentPermission.Length < 1 ? "Y" : DocumentPermission.Substring(0, 1);
            hdnEditDocument.Value = DocumentPermission.Length < 2 ? "Y" : DocumentPermission.Substring(1, 1);
            hdnDeleteDocument.Value = DocumentPermission.Length < 3 ? "Y" : DocumentPermission.Substring(2, 1);
            hdnViewDocument.Value = DocumentPermission.Length < 4 ? "Y" : DocumentPermission.Substring(3, 1);

            if (hdnDeleteDocument.Value == "N")
            {
                lnkDeleteDoc.Enabled = false;
            }
            else
            {
                lnkDeleteDoc.Enabled = true;
            }

            if (hdnAddeDocument.Value == "N")
            {
                lnkUploadDoc.Enabled = false;
            }
            else
            {
                lnkUploadDoc.Enabled = true;
            }
            pnlDocPermission.Visible = hdnViewDocument.Value == "N" ? false : true;
        }
    }

    private void RowSelectDocuments()
    {
        if (hdnEditDocument.Value == "N")
        {
            foreach (GridDataItem item in RadGrid_Documents.Items)
            {
                //CheckBox chkSelected = (CheckBox)item.FindControl("chkSelect");
                //CheckBox chkPortal = (CheckBox)item.FindControl("chkPortal");
                TextBox txtremarks = (TextBox)item.FindControl("txtremarks");
                //chkSelected.Enabled = 
                //chkPortal.Enabled = false;
                txtremarks.Enabled = false;
                item.Attributes["ondblclick"] = "   noty({ text: 'You do not have permission!', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";
            }
        }
    }

    protected void RadGrid_Documents_PreRender(object sender, EventArgs e)
    {
        RowSelectDocuments();
    }

    protected void RadGrid_Documents_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        GetDocuments();
    }

    private void UpdateTempDateWhenCreatingNewUser(string strUserId)
    {
        var UserId = Convert.ToInt32(strUserId);
        if (ViewState["TempUploadDirectory"] == null)
        {
            return;
        }
        var tempAttachedFiles = ViewState["AttachedFiles"] as DataTable;
        var tempDirectory = "UserDocs\\" + ViewState["TempUploadDirectory"] as string;
        var newDirectory = "UserDocs\\";
        newDirectory += "User_" + strUserId;

        if (tempAttachedFiles == null)
        {
            return;
        }

        var sourceDirectory = GetUploadDirectory(tempDirectory);
        var destDirectory = GetUploadDirectory(newDirectory);
        Directory.Move(sourceDirectory, destDirectory);

        objMapData.Screen = "User";

        foreach (DataRow row in tempAttachedFiles.Rows)
        {
            //objMapData.Screen = "User";
            objMapData.TicketID = UserId;
            objMapData.TempId = "0";
            objMapData.FileName = row.Field<string>("filename");
            objMapData.DocTypeMIME = row.Field<string>("doctype");
            objMapData.FilePath = row.Field<string>("Path").Replace(sourceDirectory, destDirectory);
            //objMapData.FilePath = row.Field<string>("Path");
            objMapData.DocID = 0;
            objMapData.Mode = 0;
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.Worker = Session["User"].ToString();
            objBL_MapData.AddFile(objMapData);
        }

        ViewState["TempUploadDirectory"] = null;
        ViewState["AttachedFiles"] = null;


        //get document     
        objMapData.Screen = "User";
        objMapData.TicketID = UserId;
        objMapData.TempId = "0";
        objMapData.Mode = 1;
        objMapData.ConnConfig = Session["config"].ToString();
        var ds = objBL_MapData.GetDocuments(objMapData);
        var saveDocsRows = ds.Tables[0].AsEnumerable();

        foreach (GridDataItem item in RadGrid_Documents.Items)
        {
            Label lblID = (Label)item.FindControl("lblID");
            HiddenField hdnTempId = (HiddenField)item.FindControl("hdnTempId");
            LinkButton lblName = (LinkButton)item.FindControl("lblName");

            var docRow = saveDocsRows.FirstOrDefault(t => t.Field<string>("Filename") == lblName.Text);
            if (docRow != null)
            {
                lblID.Text = docRow.Field<int>("ID").ToString();
            }
        }
    }

    #endregion

}