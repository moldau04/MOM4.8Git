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
using System.Net.Http;
using System.Xml;
using System.Xml.Linq;
using System.Threading.Tasks;

public partial class AddEmp : System.Web.UI.Page
{
    #region Properties
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    BL_ReportsData objBL_Report = new BL_ReportsData();

    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    BusinessEntity.CompanyOffice objCompany = new BusinessEntity.CompanyOffice();
    BL_Company objBL_Company = new BL_Company();

    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    
    protected DataTable dtWage = new DataTable();
    protected DataTable dtWageDeduction = new DataTable();
    BL_Wage objBL_Wage = new BL_Wage();
    Wage objWage = new Wage();
    Wage _objWage = new Wage();
    PRDed _objPRDed = new PRDed();
    Emp _objEmp = new Emp();
    public bool check = false;
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
        if (Request.QueryString["sup"] != null)
        {
            Page.MasterPageFile = "popup.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {


        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        UserPermission();

        FillWageCategory();
        GetControlForPayroll();
        FillWageDeductionCategorys();
        if (!IsPostBack)
        {
            GetControlData();
            ClearControls();
            ViewState["mode"] = 0;
            ViewState["super"] = 0;
            
            //FillDefaultWage();
            //FillOtherIncomeWage();

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

            if (Session["InInActiveWageEmp"] != null)
            {
                if (Session["InInActiveWageEmp"].ToString() == "True")
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
                    lblHeader.Text = "Copy Employee";
                    Page.Title = "Copy User || MOM";
                    pnlSave.Visible = false;
                }
                else
                {
                    lblHeader.Text = "Edit Employee";
                    Page.Title = "Edit Employee || MOM";
                    ViewState["mode"] = 1;
                    pnlSave.Visible = true;
                    txtUserName.Enabled = false;
                    liLogs.Style["display"] = "inline-block";
                    tbLogs.Style["display"] = "block";
                }
                pnlNext.Visible = true;
                //objPropUser.UserID = Convert.ToInt32(Request.QueryString["uid"]);
                //objPropUser.TypeID = Convert.ToInt32(Request.QueryString["type"]);
                //objPropUser.DBName = Session["dbname"].ToString();
                //DataSet ds = new DataSet();
                //ds = objBL_User.GetUserInfoByID(objPropUser);


                DataSet ds = new DataSet();
                _objEmp.ConnConfig = Session["config"].ToString();
                _objEmp.ID = Convert.ToInt32(Request.QueryString["uid"]);
                _objEmp.Type = Convert.ToInt32(Request.QueryString["type"]);
                _objEmp.DBName = Session["dbname"].ToString();
                ds = objBL_Wage.GetEmpInfoByID(_objEmp);
                


                ViewState["getUserById"] = ds;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (Request.QueryString["type"].ToString() == "2" && Request.QueryString["t"] == null)
                    {
                        ddlUserType.Items.Insert(2, new ListItem("Customer", "2"));
                        tblPermission.Visible = false;
                        txtHireDt.Visible = false;
                        txtTerminationDt.Visible = false;
                        //hire.Visible = false;
                        //fire.Visible = false;
                        ddlUserType.SelectedIndex = 2;
                        ddlUserType.Enabled = false;
                        chkMap.AutoPostBack = false;
                        btnSubmit.Visible = false;
                        chkSalesperson.Visible = false;
                        //lblSales.Visible = false;
                        //lblSalesAssigned.Visible = false;
                        chkSalesAssigned.Visible = false;
                        //lblNotification.Visible = false;
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
                        //if (sch == "Y")
                        //{
                        //    chkScheduleBrd.Checked = true;
                        //}
                        //if (Session["MSM"].ToString() == "TS")
                        //{
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
                        //}
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
                    ddlSalaryPeriod.SelectedValue = ds.Tables[0].Rows[0]["salaryF"].ToString();
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
                    //ddlFillingState.SelectedValue = ds.Tables[0].Rows[0]["FillingState"].ToString();
                    txtSSNSIN.Text = ds.Tables[0].Rows[0]["SSN"].ToString();
                    ddlPayPeriod.SelectedValue = ds.Tables[0].Rows[0]["payperiod"].ToString();
                    txtLatitude.Text = ds.Tables[0].Rows[0]["Lat"].ToString();
                    txtSSNSIN.Attributes["value"] = txtSSNSIN.Text;

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
                                chkSuper.Enabled = false;
                            else
                                chkSuper.Enabled = true;
                        }
                        else
                        {
                            ViewState["super"] = 0;
                            lblSuper.Enabled = true;
                            ddlSuper.Enabled = true;
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
                    if(JobClosePermission.Contains("Y")) chkJobClosePermission.Checked = true;                    else chkJobClosePermission.Checked = false;


                    //JobCompletedPermission
                    string JobCompletedPermission = ds.Tables[0].Rows[0]["JobCompletedPermission"] == DBNull.Value ? "Y" : ds.Tables[0].Rows[0]["JobCompletedPermission"].ToString();
                    if (JobCompletedPermission.Contains("Y")) chkJobCompletedPermission.Checked = true;        else chkJobCompletedPermission.Checked = false;

                    //JobReopenPermission
                    string JobReopenPermission = ds.Tables[0].Rows[0]["JobReopenPermission"] == DBNull.Value ? "Y" : ds.Tables[0].Rows[0]["JobReopenPermission"].ToString();
                    if (JobReopenPermission.Contains("Y")) chkJobReopenPermission.Checked = true;               else chkJobReopenPermission.Checked = false;


                    //SchedulePermission
                    string SchedulePermission = ds.Tables[0].Rows[0]["Ticket"] == DBNull.Value ? "NNNNNN" : ds.Tables[0].Rows[0]["Ticket"].ToString();

                    
                    string ViewSchedulePermission = SchedulePermission.Length < 4 ? "Y" : SchedulePermission.Substring(3, 1);
                    string TicketReportPer = SchedulePermission.Length < 6 ? "Y" : SchedulePermission.Substring(5, 1);

                    chkScheduleBoard.Checked= (ViewSchedulePermission == "N") ? false : true;

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
                    chkTicketListView.Checked =   (ViewTicketePermission == "N") ? false : true;
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
                    bool PRPermission =Convert.ToBoolean(ds.Tables[0].Rows[0]["PR"] == DBNull.Value ? false : ds.Tables[0].Rows[0]["PR"]);
                    payrollModulchck.Checked= (PRPermission == false) ? false : true;


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
                    string CollectionsPermission = ds.Tables[0].Rows[0]["Collection"] == DBNull.Value ? "NNNN" : ds.Tables[0].Rows[0]["Collection"].ToString();

                    string AddCollectionsPermission = CollectionsPermission.Length < 1 ? "Y" : CollectionsPermission.Substring(0, 1);
                    string EditCollectionsPermission = CollectionsPermission.Length < 2 ? "Y" : CollectionsPermission.Substring(1, 1);
                    string DeleteCollectionsPermission = CollectionsPermission.Length < 3 ? "Y" : CollectionsPermission.Substring(2, 1);
                    string ViewCollectionsPermission = CollectionsPermission.Length < 4 ? "Y" : CollectionsPermission.Substring(3, 1);

                    chkCollectionsAdd.Checked = (AddCollectionsPermission == "N") ? false : true;
                    chkCollectionsEdit.Checked = (EditCollectionsPermission == "N") ? false : true;
                    chkCollectionsDelete.Checked = (DeleteDepositPermission == "N") ? false : true;
                    chkCollectionsView.Checked = (ViewDepositPermission == "N") ? false : true;

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



                    if (Sales == "Y")
                    {
                        chkSalesMgr.Checked = true;
                    }
                    else
                    {
                        chkSalesMgr.Checked = false;
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
                /////////////////////////////////
                DataSet dsemp = new DataSet();
                _objEmp.ConnConfig = Session["config"].ToString();
                _objEmp.ID = Convert.ToInt32(Request.QueryString["uid"]);
                dsemp = objBL_Wage.GetEmployeeListByID(_objEmp);
                DataRow _dr = dsemp.Tables[0].Rows[0];
                SetEmpData(_dr);
                /////////////////////////////////

            }
        }
        CompanyPermission();

        HighlightSideMenu("prID", "Employeelink", "payrollmenutab");
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

    private void SetEmpData(DataRow _dr)
    {
        

        txtFName.Text = _dr["fFirst"].ToString();
        txtLName.Text = _dr["Last"].ToString();
        txtMName.Text = _dr["Middle"].ToString();
        txtSSNSIN.Text = _dr["SSN"].ToString();


        ddlUserType.SelectedValue = _dr["Field"].ToString();
        rbStatus.SelectedValue = _dr["Status"].ToString();
        ddlaalownancesFederal.SelectedValue = _dr["FStatus"].ToString();
        //ddlaalownancesState.SelectedValue = _dr["SStatus"].ToString();
        ddlPayPeriod.SelectedValue = _dr["PayPeriod"].ToString();
        ddlBasedOnmisc.SelectedValue = _dr["VBase"].ToString();
        ddlPayMethod.SelectedValue = _dr["PMethod"].ToString();
        ddlSalaryPeriod.SelectedValue = _dr["SalaryF"].ToString();
        //ddlPayrollHours.SelectedValue = _dr["PFixed"].ToString();
        //ddlAllowancesAdditonalLocal.SelectedValue = _dr["LName"].ToString();
        //ddlaalownancesLocal.SelectedValue = _dr["LStatus"].ToString();
        ddlState.Text = _dr["State"].ToString();

        txtMsg.Text = _dr["Pager"].ToString();
        txtHireDt.Text = _dr["DHired"].ToString();
        txtTerminationDt.Text = _dr["DFired"].ToString();
        txtDateOfBirth.Text = _dr["DBirth"].ToString();
        //txtReview.Text = _dr["DReview"].ToString();
        txtTerminationDt.Text = _dr["DLast"].ToString();

        txtAllowncesFedral.Text = _dr["FAllow"].ToString();
        txtAllowancesAdditonalFedral.Text = _dr["FAdd"].ToString();
        //txtAllowncesState.Text = _dr["SAllow"].ToString();
        //txtAllowancesAdditonalState.Text = _dr["SAdd"].ToString();
        txtUserName.Text = _dr["CallSign"].ToString();
        txtVacationRate.Text = _dr["VRate"].ToString();
        txtAvailableVacation.Text = _dr["VLast"].ToString();
        txtSickUnits.Text = _dr["Sick"].ToString();
        //txtAllownceslocal.Text = _dr["LAllow"].ToString();
        hdntxtPRTaxGL.Value = _dr["PRTaxE"].ToString();
        txtPaidMisc.Text = _dr["NPaid"].ToString();
        txtBankRoute1.Text = _dr["ACHRoute"].ToString();
        txtBankAcct1.Text = _dr["ACHBank"].ToString();
        //txtRehire.Text = _dr["Anniversary"].ToString();
        txtPDASerial.Text = _dr["PDASerialNumber"].ToString();
        txtBankRoute2.Text = _dr["ACHRoute2"].ToString();
        txtBankAcct2.Text = _dr["ACHBank2"].ToString();

        ddlEthnicity.SelectedValue = _dr["Race"].ToString();
        ddlGender.SelectedValue = _dr["Sex"].ToString();
        ddlDirectDeposit.SelectedValue = _dr["ACH"].ToString();
        ddlAccountType1.SelectedValue = _dr["ACHType"].ToString();
        //ddlDefaultWage.SelectedValue = _dr["WageCat"].ToString();
        ddlSplitType.SelectedValue = _dr["DDType"].ToString();
        ddlAccountType2.SelectedValue = _dr["ACHType2"].ToString();

        //txtBillRate.Text = _dr["BillRate"].ToString();
        //txtSales.Text = _dr["BMSales"].ToString();
        //txtInvoiceAverage.Text = _dr["BMInvAve"].ToString();
        //txtClosingPercentage.Text = _dr["BMClosing"].ToString();
        //txtBillableEfficiency.Text = _dr["BMBillEff"].ToString();
        //txtProdcutionEfficiency.Text = _dr["BMProdEff"].ToString();
        //txtAverageTasks.Text = _dr["BMAveTask"].ToString();
        txtCustom6.Text = _dr["BMCustom1"].ToString();
        txtCustom7.Text = _dr["BMCustom2"].ToString();
        txtCustom8.Text = _dr["BMCustom3"].ToString();
        txtCustom9.Text = _dr["BMCustom4"].ToString();
        txtCustom10.Text = _dr["BMCustom5"].ToString();

        txtSickRate.Text = _dr["SickRate"].ToString();
        txtAccuredVacation.Text = _dr["VacAccrued"].ToString();
        txtPDASerial.Text = _dr["PDASerialNumber"].ToString();

        txtCity.Text = _dr["City"].ToString();
        txtZip.Text = _dr["Zip"].ToString();
        txtTelephone.Text = _dr["Phone"].ToString();
        txtAddress.Value = _dr["Address"].ToString();
        txtEmail.Text = _dr["Email"].ToString();
        txtCell.Text = _dr["Cellular"].ToString();
        txtRemark.Text = _dr["Remarks"].ToString();
        ddlCountry.SelectedValue = _dr["Country"].ToString();
        txtEmpID.Text = _dr["Ref"].ToString();
        txtPRTaxGL.Text = _dr["PRTaxEName"].ToString();
        //txtFax.Text = _dr["Fax"].ToString();

        //hdnEmpID.Value = _dr["ID"].ToString();


    }
    private string GetGeoCode(string _sAddres,string _sCity,string _sState,string _sZip)
    {
        string geo = "";
        string code = "";
        string username = "dread@1000";
        string passWord = "K3CHccxQ";
        string uri = "https://payrollsandbox.ondemand.vertexinc.com:443/EiWebSvc/AddressWebService";
        string addrClnXML = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:eic = \"http://EiCalc/\">"
            + "<soapenv:Header/>"
            + "<soapenv:Body>"
            + "<eic:AddrCleanse>"
            + "<Request>"
            + "<![CDATA["
                + "<ADDRESS_CLEANSE_REQUEST>"
                    + "<StreetAddress1>" + _sAddres + "</StreetAddress1>"
                    + "<CityName>" + _sCity + "</CityName>"
                    + "<StateName>" + _sState + "</StateName>"
                    + "<ZipCode>" + _sZip + "</ZipCode>"
                + "</ADDRESS_CLEANSE_REQUEST>]]>"
            + "</Request>"
            + "</eic:AddrCleanse>"
            + "</soapenv:Body>"
            + "</soapenv:Envelope>";

        try
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            HttpClient cl = new HttpClient();
            cl.BaseAddress = new Uri(uri);
            cl.DefaultRequestHeaders.Clear();
            cl.DefaultRequestHeaders.Add("javax.xml.ws.security.auth.username", username);
            cl.DefaultRequestHeaders.Add("javax.xml.ws.security.auth.password", passWord);
            HttpContent soapAddressEnvelope = new StringContent(addrClnXML);
            soapAddressEnvelope.Headers.Clear();
            soapAddressEnvelope.Headers.Add("Content-Type", "text/xml");
            using (HttpResponseMessage response = cl.PostAsync(uri, soapAddressEnvelope).Result)
            {
                string rawString = getMessage(response).Result;
                XmlDocument responseXML = new XmlDocument();
                responseXML.LoadXml(rawString);
                XDocument responseXMLPretty = XDocument.Parse(responseXML.InnerText.ToString());
                string responseXMLPrettystr = responseXMLPretty.ToString();
                responseXMLPrettystr = responseXMLPrettystr.Replace("\"", "'");
                XmlDocument Doc = new XmlDocument();
                Doc.LoadXml(responseXMLPrettystr);
                XmlNode nodedoc = Doc.GetElementsByTagName("GeoCode").Item(0);
                geo = nodedoc.ChildNodes.Item(0).InnerText;
                soapAddressEnvelope.Dispose();
                cl.Dispose();
            }
        }

        catch (AggregateException aggEx)
        {
            Console.WriteLine("A Connection error occurred");
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("Error Code: " + aggEx.Message.ToString() + Environment.NewLine + "Message: " + aggEx.InnerException.Message.ToString());
        }
        async Task<string> getMessage(HttpResponseMessage messageFromServer)
        {
            code = await messageFromServer.Content.ReadAsStringAsync();
            return code;
        }
        return geo;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidWageRateGrid() && IsValidOtherWageRateGrid() && IsValidWageDedcutionRateGrid() && Page.IsValid)
            {
                var pwError = string.Empty;
                if (!CheckPasswordPolicy(ddlUserType.SelectedValue, txtPassword.Text, txtUserName.Text, txtFName.Text, txtLName.Text, ref pwError))
                {
                    pwError = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(pwError);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "keySuperAlert", "noty({text: '"+ pwError + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    return;
                }

                _objEmp.ConnConfig = Session["config"].ToString();
                _objEmp.ID = 0;
                _objEmp.fFirst = txtFName.Text;
                _objEmp.Last = txtLName.Text;
                _objEmp.Middle = txtMName.Text;
                _objEmp.Name = txtFName.Text + " " + txtLName.Text;
                _objEmp.Rol = 0;
                _objEmp.SSN = txtSSNSIN.Text;
                _objEmp.Title = "";
                //if (chkSales.Checked == true)
                //{
                //    _objEmp.Sales = 1;
                //}
                //else
                //{
                    _objEmp.Sales = 0;
                //}
                _objEmp.Field = Convert.ToInt32(ddlUserType.SelectedValue.ToString());
                _objEmp.Status = Convert.ToInt32(rbStatus.SelectedValue.ToString());
                _objEmp.Pager = txtMsg.Text;
                _objEmp.InUse = 0;
                _objEmp.PayPeriod = Convert.ToInt32(ddlPayPeriod.SelectedValue.ToString());
                _objEmp.DHired = Convert.ToDateTime(txtHireDt.Text);
                DateTime MinDate = System.DateTime.MinValue;
                if (DateTime.TryParse(txtTerminationDt.Text.Trim(), out MinDate))
                    _objEmp.DFired = Convert.ToDateTime(txtTerminationDt.Text.Trim());

                _objEmp.DBirth = Convert.ToDateTime(txtDateOfBirth.Text);
                _objEmp.DReview = Convert.ToDateTime(MinDate);

                if (DateTime.TryParse(txtTerminationDt.Text.Trim(), out MinDate))
                    _objEmp.DLast = Convert.ToDateTime(txtTerminationDt.Text.Trim());

                _objEmp.FStatus = Convert.ToInt32(ddlaalownancesFederal.SelectedValue.ToString());
                if (txtAllowncesFedral.Text.Trim() != "")
                {
                    _objEmp.FAllow = Convert.ToInt32(txtAllowncesFedral.Text);
                }
                else
                {
                    _objEmp.FAllow = 0;
                }
                if (txtAllowancesAdditonalFedral.Text.Trim() != "")
                {
                    _objEmp.FAdd = Convert.ToDouble(txtAllowancesAdditonalFedral.Text);
                }
                else
                {
                    _objEmp.FAdd = 0;
                }
                //_objEmp.SStatus = Convert.ToInt32(ddlaalownancesState.SelectedValue.ToString());
                //if (txtAllowncesState.Text.Trim() != "")
                //{
                //    _objEmp.SAllow = Convert.ToInt32(txtAllowncesState.Text);
                //}
                //else
                //{
                //    _objEmp.SAllow = 0;
                //}
                //if (txtAllowancesAdditonalState.Text.Trim() != "")
                //{
                //    _objEmp.SAdd = Convert.ToDouble(txtAllowancesAdditonalState.Text);
                //}
                //else
                //{
                //    _objEmp.SAdd = 0;
                //}
                _objEmp.CallSign = txtUserName.Text.Trim();
                if (txtVacationRate.Text.Trim() != "")
                {
                    _objEmp.VRate = Convert.ToDouble(txtVacationRate.Text);
                }
                else
                {
                    _objEmp.VRate = 0.00;
                }
                _objEmp.VBase = Convert.ToInt32(ddlBasedOnmisc.SelectedValue);
                if (txtAvailableVacation.Text.Trim() != "")
                {
                    _objEmp.VLast = Convert.ToDouble(txtAvailableVacation.Text);
                }
                else
                {
                    _objEmp.VLast = 0.00;
                }

                _objEmp.VThis = 0;

                if (txtSickUnits.Text.Trim() != "")
                {
                    _objEmp.Sick = Convert.ToDouble(txtSickUnits.Text);
                }
                else
                {
                    _objEmp.Sick = 0.00;
                }

                _objEmp.PMethod = Convert.ToInt32(ddlPayMethod.SelectedValue.ToString());
                // _objEmp.PFixed = Convert.ToInt32(ddlPayrollHours.SelectedValue.ToString());
                if (Convert.ToInt32(ddlPayMethod.SelectedValue.ToString()) == 2)
                {
                    _objEmp.PFixed = 0;
                    if (txtHours.Text.Trim() != "")
                    {
                        _objEmp.PHour = Convert.ToInt32(txtHours.Text.ToString());
                    }
                    else
                    {
                        _objEmp.PHour = 0;
                    }
                }
                else
                {                    
                    _objEmp.PFixed = 99;
                    _objEmp.PHour = 0;
                }
                
                //_objEmp.LName = Convert.ToInt32(ddlAllowancesAdditonalLocal.SelectedValue.ToString());
                //_objEmp.LStatus = Convert.ToInt32(ddlaalownancesLocal.SelectedValue.ToString());

                //if (txtAllownceslocal.Text.Trim() != "")
                //{
                //    _objEmp.LAllow = Convert.ToInt32(txtAllownceslocal.Text);
                //}
                //else
                //{
                //    _objEmp.LAllow = 0;
                //}
                if (hdntxtPRTaxGL.Value.Trim() != "")
                {
                    _objEmp.PRTaxE = Convert.ToInt32(hdntxtPRTaxGL.Value);
                }
                else
                {
                    _objEmp.PRTaxE = 0;
                }



                _objEmp.State = ddlState.Text;
                //_objEmp.FillingState = ddlFillingState.SelectedValue.ToString();
                // _objEmp.Salary= 0;
                _objEmp.Salary = txtAmount.Text.Replace("$", string.Empty) != string.Empty ? Convert.ToDouble(txtAmount.Text.Replace("$", string.Empty)) : 0;
                _objEmp.SalaryF = Convert.ToInt32(ddlSalaryPeriod.SelectedValue.ToString());
                _objEmp.SalaryGL = 0;
                _objEmp.fWork = 0;

                if (txtPaidMisc.Text.Trim() != "")
                {
                    _objEmp.NPaid = Convert.ToInt32(txtPaidMisc.Text);
                }
                else
                {
                    _objEmp.NPaid = 0;
                }

                _objEmp.Balance = 0;
                _objEmp.PBRate = 0;
                _objEmp.FITYTD = 0;
                _objEmp.FICAYTD = 0;
                _objEmp.MEDIYTD = 0;
                _objEmp.FUTAYTD = 0;
                _objEmp.SITYTD = 0;
                _objEmp.LocalYTD = 0;
                _objEmp.BonusYTD = 0;
                _objEmp.HolH = 0;
                _objEmp.HolYTD = 0;
                _objEmp.VacH = 0;
                _objEmp.VacYTD = 0;
                _objEmp.ZoneH = 0;
                _objEmp.ZoneYTD = 0;
                _objEmp.ReimbYTD = 0;
                _objEmp.MileH = 0;
                _objEmp.MileYTD = 0;
                _objEmp.Race = ddlEthnicity.SelectedValue.ToString();
                _objEmp.Sex = ddlGender.SelectedValue.ToString();
                _objEmp.Ref = txtEmpID.Text;
                _objEmp.ACH = Convert.ToInt32(ddlDirectDeposit.SelectedValue.ToString());
                _objEmp.ACHType = Convert.ToInt32(ddlAccountType1.SelectedValue.ToString());
                _objEmp.ACHRoute = txtBankRoute1.Text;
                _objEmp.ACHBank = txtBankAcct1.Text;
                //if (txtRehire.Text != "")
                //{
                //    _objEmp.Anniversary = Convert.ToDateTime(txtRehire.Text);
                //}
                //if (DateTime.TryParse(txtRehire.Text.Trim(), out MinDate))
                    _objEmp.Anniversary = Convert.ToDateTime(MinDate);

                _objEmp.Level = 0;
                //_objEmp.WageCat = Convert.ToInt32(ddlDefaultWage.SelectedValue.ToString());
                _objEmp.DSenior = DateTime.Now;
                _objEmp.PRWBR = 0;
                _objEmp.PDASerialNumber_1 = txtPDASerial.Text;
                _objEmp.StatusChange = 0;
                _objEmp.SCDate = DateTime.Now;
                _objEmp.SCReason = "";
                _objEmp.DemoChange = 0;
                _objEmp.Language = "";
                _objEmp.TicketD = 0;
                _objEmp.Custom1 = "";
                _objEmp.Custom2 = "";
                _objEmp.Custom3 = "";
                _objEmp.Custom4 = "";
                _objEmp.Custom5 = "";
                _objEmp.DDType = Convert.ToInt32(ddlSplitType.SelectedValue.ToString());
                _objEmp.DDRate = 0;
                _objEmp.ACHType2 = Convert.ToInt32(ddlAccountType2.SelectedValue.ToString());
                _objEmp.ACHRoute2 = txtBankRoute2.Text;
                _objEmp.ACHBank2 = txtBankAcct2.Text;

                //if (txtBillRate.Text.Trim() != "")
                //{
                //    _objEmp.BillRate = Convert.ToDouble(txtBillRate.Text);
                //}
                //else
                //{
                //    _objEmp.BillRate = 0.00;
                //}
                //if (txtSales.Text.Trim() != "")
                //{
                //    _objEmp.BMSales = Convert.ToDouble(txtSales.Text);
                //}
                //else
                //{
                //    _objEmp.BMSales = 0.00;
                //}
                //if (txtInvoiceAverage.Text.Trim() != "")
                //{
                //    _objEmp.BMInvAve = Convert.ToDouble(txtInvoiceAverage.Text);
                //}
                //else
                //{
                //    _objEmp.BMInvAve = 0.00;
                //}
                //if (txtClosingPercentage.Text.Trim() != "")
                //{
                //    _objEmp.BMClosing = Convert.ToDouble(txtClosingPercentage.Text);
                //}
                //else
                //{
                //    _objEmp.BMClosing = 0.00;
                //}
                //if (txtBillableEfficiency.Text.Trim() != "")
                //{
                //    _objEmp.BMBillEff = Convert.ToDouble(txtBillableEfficiency.Text);
                //}
                //else
                //{
                //    _objEmp.BMBillEff = 0.00;
                //}
                //if (txtProdcutionEfficiency.Text.Trim() != "")
                //{
                //    _objEmp.BMProdEff = Convert.ToDouble(txtProdcutionEfficiency.Text);
                //}
                //else
                //{
                //    _objEmp.BMProdEff = 0.00;
                //}
                //if (txtAverageTasks.Text.Trim() != "")
                //{
                //    _objEmp.BMAveTask = Convert.ToInt32(txtAverageTasks.Text);
                //}
                //else
                //{
                //    _objEmp.BMAveTask = 0;
                //}
                if (txtCustom6.Text.Trim() != "")
                {
                    _objEmp.BMCustom1 = Convert.ToInt32(txtCustom6.Text);
                }
                else
                {
                    _objEmp.BMCustom1 = 0;
                }
                if (txtCustom7.Text.Trim() != "")
                {
                    _objEmp.BMCustom2 = Convert.ToInt32(txtCustom7.Text);
                }
                else
                {
                    _objEmp.BMCustom2 = 0;
                }
                if (txtCustom8.Text.Trim() != "")
                {
                    _objEmp.BMCustom3 = Convert.ToInt32(txtCustom8.Text);
                }
                else
                {
                    _objEmp.BMCustom3 = 0;
                }
                if (txtCustom9.Text.Trim() != "")
                {
                    _objEmp.BMCustom4 = Convert.ToInt32(txtCustom9.Text);
                }
                else
                {
                    _objEmp.BMCustom4 = 0;
                }
                if (txtCustom10.Text.Trim() != "")
                {
                    _objEmp.BMCustom5 = Convert.ToInt32(txtCustom10.Text);
                }
                else
                {
                    _objEmp.BMCustom5 = 0;
                }


                _objEmp.TaxCodeNR = "";
                _objEmp.TaxCodeR = "";
                _objEmp.DeviceID = "";
                _objEmp.MileageRate = 0;
                _objEmp.Import1 = "0";
                _objEmp.MSDeviceId = "";
                _objEmp.TechnicianBio = "";
                _objEmp.PayPortalPassword = "";
                if (txtSickRate.Text.Trim() != "")
                {
                    _objEmp.SickRate = Convert.ToDouble(txtSickRate.Text);
                }
                else
                {
                    _objEmp.SickRate = 0.00;
                }
                if (txtAccuredVacation.Text.Trim() != "")
                {
                    _objEmp.VacAccrued = Convert.ToDouble(txtAccuredVacation.Text);
                }
                else
                {
                    _objEmp.VacAccrued = 0.00;
                }
                _objEmp.SickAccrued = 0;
                _objEmp.SickUsed = 0;
                _objEmp.SickYTD = 0;

                _objEmp.SCounty = 0;
                _objEmp.PDASerialNumber = txtPDASerial.Text;

                _objEmp.City = txtCity.Text;
                _objEmp.Zip = txtZip.Text;
                _objEmp.Tel = txtTelephone.Text;
                _objEmp.Address = txtAddress.Value;
                _objEmp.Email = txtEmail.Text;
                _objEmp.Cell = txtCell.Text;
                _objEmp.Remarks = txtRemark.Text;
                _objEmp.Type = 5;
                _objEmp.Contact = txtUserName.Text;
                _objEmp.Website = "";

                _objEmp.Country = ddlCountry.SelectedValue.ToString();
                _objEmp.Fax = "";
                _objEmp.Geocode = GetGeoCode(txtAddress.Value.ToString(), txtCity.Text.ToString(), ddlState.Text.ToString(), txtZip.Text.ToString());

                if (ViewState["WageItems"] != null)
                {
                    DataTable dataTable = (DataTable)ViewState["WageItems"];
                    dataTable.Columns.Remove("GLName");
                    dataTable.Columns.Remove("fDesc");
                    dataTable.Columns.Remove("Checked");
                    dataTable.Columns.Remove("StatusName");
                    dataTable.AcceptChanges();
                    _objEmp.dtWageCategory = dataTable;
                }
                if (ViewState["WageDeductionItems"] != null)
                {
                    DataTable dataTable = (DataTable)ViewState["WageDeductionItems"];
                    dataTable.Columns.Remove("Checked");
                    dataTable.Columns.Remove("EmpGLName");
                    dataTable.Columns.Remove("fDesc");
                    dataTable.Columns.Remove("CompGLName");
                    dataTable.Columns.Remove("CompGLEName");
                    dataTable.AcceptChanges();
                    _objEmp.dtWageDeduction = dataTable;
                }
                if (ViewState["OtherWageItems"] != null)
                {
                    DataTable dataTable = (DataTable)ViewState["OtherWageItems"];
                    
                    dataTable.Columns.Remove("ExpAcctName");
                    dataTable.Columns.Remove("fDesc");
                   
                    dataTable.AcceptChanges();
                    _objEmp.dtOtherIncome = dataTable;
                }

                //******************************************************
                //string msg = "Added";
                //if (ViewState["mode"].ToString() == "0")
                //{
                //    objBL_Wage.AddEmp(_objEmp);
                //}
                //else if(Convert.ToInt32(ViewState["mode"]) == 1)
                //{
                //    msg = "Updated";
                //    _objEmp.ID = Convert.ToInt32(ViewState["userid"]);
                //    objBL_Wage.UpdateEmp(_objEmp);
                //}

                //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: '" + msg + " added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                //ClearControls();
                //ResetFormControlValues(this);
                //*********************************************************

                if (ViewState["WageItems"] != null)
                {
                    DataTable dataTable = (DataTable)ViewState["WageItems"];

                    DataTable dt_copy = new DataTable();                    
                    dt_copy = dataTable.Copy();

                    //dataTable.Columns.Remove("Checked");
                    dt_copy.Columns.Remove("GL");
                    dt_copy.Columns.Remove("FIT");
                    dt_copy.Columns.Remove("FICA");
                    dt_copy.Columns.Remove("MEDI");
                    dt_copy.Columns.Remove("FUTA");
                    dt_copy.Columns.Remove("SIT");
                    dt_copy.Columns.Remove("Vac");
                    dt_copy.Columns.Remove("Wc");
                    dt_copy.Columns.Remove("Uni");
                    dt_copy.Columns.Remove("InUse");
                    dt_copy.Columns.Remove("Sick");
                    //dt_copy.Columns.Remove("Status");
                    dt_copy.Columns.Remove("YTD");
                    dt_copy.Columns.Remove("YTDH");
                    dt_copy.Columns.Remove("OYTD");
                    dt_copy.Columns.Remove("OYTDH");
                    dt_copy.Columns.Remove("DYTD");
                    dt_copy.Columns.Remove("DYTDH");
                    dt_copy.Columns.Remove("TYTD");
                    dt_copy.Columns.Remove("TYTDH");
                    dt_copy.Columns.Remove("NYTD");
                    dt_copy.Columns.Remove("NYTDH");
                    dt_copy.Columns.Remove("VacR");

                    dt_copy.AcceptChanges();
                    objPropUser.DtWage = dt_copy;
                }
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
                objPropUser.NotificationOnAddOpportunity = chkNotification.Checked ? true : false;
                objPropUser.EstApproveProposal = chkEstApprovalStatus.Checked ? true : false;
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
                            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'Max Amount must be greater than Min amount',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
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

                //objPropUser.POApproveAmt = Convert.ToInt16(ddlPOApproveAmt.SelectedValue);
                //if (!String.IsNullOrEmpty(txtMinAmount.Text))
                //    objPropUser.MinAmount = Convert.ToDecimal(txtMinAmount.Text);
                //if (!String.IsNullOrEmpty(txtMaxAmount.Text))
                //    objPropUser.MaxAmount = Convert.ToDecimal(txtMaxAmount.Text);
                //if (objPropUser.MaxAmount > 0)
                //{
                //    if (objPropUser.MaxAmount <= objPropUser.MinAmount)
                //    {
                //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'Max Amount must be greater than Min amount',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                //        return;
                //    }
                //}

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
                objPropUser.Title = string.Empty;
                objPropUser.IsProjectManager = chkProjectManager.Checked;
                objPropUser.IsAssignedProject = chkAssignedProject.Checked;
                if (ddlMerchantID.SelectedValue != string.Empty)
                {
                    objPropUser.MerchantInfoId = Convert.ToInt32(ddlMerchantID.SelectedValue);
                }

                objPropUser.RoleID = Convert.ToInt32(ddlUserRole.SelectedValue);
                objPropUser.ApplyUserRolePermission = Convert.ToInt32(ddlApplyUserRolePermission.SelectedValue);
                // objPropUser.dtPageData = PagePermissionData();

                if (Convert.ToInt32(ViewState["mode"]) == 1)
                {
                    objPropUser.UserID = Convert.ToInt32(ViewState["userid"]);
                    if (!string.IsNullOrEmpty(Request.QueryString["uid"].ToString()))
                    {
                        objPropUser.EmpId = Convert.ToInt32(Request.QueryString["uid"]);
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
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnUp", "noty({text: 'This employee has assigned locations and cannot be set inactive.',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }
                    else if (ddlUserType.SelectedValue == "0" && hdnLocCount.Value != "0")
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWarnUp", "noty({text: 'This employee has assigned locations and cannot be removed from field.',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }
                    else
                    {
                        var updatedBy = Session["Username"].ToString();
                        objBL_User.UpdateUser(objPropUser, updatedBy);

                        /////////////////////////////////////////////////////////////////

                        int Empid = Convert.ToInt32(Request.QueryString["uid"]);
                        _objEmp.ID = Empid;
                        _objEmp.MOMUSer = Session["Username"].ToString();
                        objBL_Wage.UpdateEmp(_objEmp);

                        /////////////////////////////////////////////////////////////////

                        hdnApplyUserRolePermissionOrg.Value = ddlApplyUserRolePermission.SelectedValue;
                        //objBL_User.UpdateUserPermission(objPropUser);
                        if (Session["COPer"].ToString() == "1")
                        {
                            SubmitCompany();
                            FillCompanySelected();
                        }
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "noty({text: 'Employee updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    }

                    RadGrid_gvLogs.Rebind();

                }
                else
                {
                    var createdBy = Session["Username"].ToString();
                    objPropUser.UserID = objBL_User.AddUser(objPropUser, createdBy);

                    /////////////////////////////////////////////////////////////////
                    
                    objPropUser.TypeID = Convert.ToInt32(ddlUserType.SelectedValue);
                    objPropUser.DBName = Session["dbname"].ToString();
                    DataSet ds = new DataSet();
                    ds = objBL_User.GetUserInfoByID(objPropUser);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int Empid = Convert.ToInt32(ds.Tables[0].Rows[0]["Empid"].ToString());
                        _objEmp.ID = Empid;
                        _objEmp.MOMUSer = Session["Username"].ToString();
                        objBL_Wage.UpdateEmp(_objEmp);
                    }
                    
                    /////////////////////////////////////////////////////////////////


                    hdnApplyUserRolePermissionOrg.Value = ddlApplyUserRolePermission.SelectedValue;
                    //objPropUser.UserID = objBL_User.AddUser(objPropUser);
                    //objBL_User.UpdateUserPermission(objPropUser);
                    if (Session["COPer"].ToString() == "1")
                    {
                        ViewState["AddUserID"] = objPropUser.UserID;
                        SubmitCompany();
                    }
                    ViewState["mode"] = 0;
                    //lblMsg.Text = "User added successfully.";

                    string strsuper = "Employee";
                    if (Request.QueryString["sup"] != null)
                    {
                        strsuper = "Supervisor";
                    }

                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: '" + strsuper + " added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    ClearControls();
                    ResetFormControlValues(this);
                }
            }
        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message;    
            //string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string str = WebBaseUtility.RemoveSpecCharsForJsScriptNotification(ex.Message);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

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

        chkTicketVoidPermission.Checked = false;
        chkMassPayrollTicket1.Checked= chkMassPayrollTicket.Checked = false;
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("EmployeeList.aspx");
        
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
        DataTable dt = (DataTable)Session["EmployeeList"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["ID"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find( Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);
        int c = dt.Rows.Count - 1;

        if (index < c)
        {
            //if (Convert.ToInt16(dt.Rows[index + 1]["usertypeid"].ToString()) == 2)
            //    Response.Redirect("customeruser.aspx?uid=" + dt.Rows[index + 1]["userid"] + "&type=" + dt.Rows[index + 1]["usertypeid"]);
            //else
            //    Response.Redirect("adduser.aspx?uid=" + dt.Rows[index + 1]["userid"] + "&type=" + dt.Rows[index + 1]["usertypeid"]);
            
            string url = "AddEmp.aspx?uid=" + dt.Rows[index + 1]["ID"] + "&type=" + dt.Rows[index + 1]["usertypeid"];
            Response.Redirect(url);
        }
    }

    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["EmployeeList"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["ID"];
        dt.PrimaryKey = keyColumns;

        //DataRow d = dt.Rows.Find(Request.QueryString["type"].ToString() + "_" + Request.QueryString["uid"].ToString());
        DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
        int index = dt.Rows.IndexOf(d);
        int c = dt.Rows.Count - 1;
        if (index < c)
        {
            
            string url = "AddEmp.aspx?uid=" + dt.Rows[index - 1]["ID"] + "&type=" + dt.Rows[index - 1]["usertypeid"];
            Response.Redirect(url);
        }
        //if (index > 0)
        //{
        //    if (Convert.ToInt16(dt.Rows[index - 1]["usertypeid"].ToString()) == 2)
        //        Response.Redirect("customeruser.aspx?uid=" + dt.Rows[index - 1]["userid"] + "&type=" + dt.Rows[index - 1]["usertypeid"]);
        //    else
        //        Response.Redirect("adduser.aspx?uid=" + dt.Rows[index - 1]["userid"] + "&type=" + dt.Rows[index - 1]["usertypeid"]);
        //}
    }

    protected void lnkLast_Click(object sender, EventArgs e)
    {
        //DataTable dt = (DataTable)Session["EmployeeList"];
        //if (Convert.ToInt16(dt.Rows[dt.Rows.Count - 1]["usertypeid"].ToString()) == 2)
        //    Response.Redirect("customeruser.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["userid"] + "&type=" + dt.Rows[dt.Rows.Count - 1]["usertypeid"]);
        //else
        //    Response.Redirect("adduser.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["userid"] + "&type=" + dt.Rows[dt.Rows.Count - 1]["usertypeid"]);
        DataTable dt = new DataTable();
        dt = (DataTable)Session["EmployeeList"];
        string url = "AddEmp.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["ID"] + "&type=" + dt.Rows[dt.Rows.Count - 1]["usertypeid"];        
        Response.Redirect(url);
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        //DataTable dt = (DataTable)Session["EmployeeList"];
        //if (Convert.ToInt16(dt.Rows[0]["usertypeid"].ToString()) == 2)
        //    Response.Redirect("customeruser.aspx?uid=" + dt.Rows[0]["userid"] + "&type=" + dt.Rows[0]["usertypeid"]);
        //else
        //    Response.Redirect("adduser.aspx?uid=" + dt.Rows[0]["userid"] + "&type=" + dt.Rows[0]["usertypeid"]);
        DataTable dt = new DataTable();
        dt = (DataTable)Session["EmployeeList"];
        string url = "AddEmp.aspx?uid=" + dt.Rows[0]["ID"] + "&type=" + dt.Rows[0]["usertypeid"];        
        Response.Redirect(url);
    }

    private void DisableButton()
    {
        DataTable dt = new DataTable();
        if (Session["EmployeeList"] != null)
        {
            dt = (DataTable)Session["EmployeeList"];
        }
        else
        {
            GetUesrdata();
            dt = (DataTable)Session["EmployeeList"];
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
        Session["EmployeeList"] = ds.Tables[0];
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

            this.programmaticModalPopup.Show();
            iframeTicket.Attributes["src"] = "adduser.aspx?sup=1";
        }
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
    }

    protected void hideModalPopupViaServer_Click(object sender, EventArgs e)
    {
        this.programmaticModalPopup.Hide();
        iframeTicket.Attributes["src"] = "";
        FillSupervisor();
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
            pnlGrid.Visible = true;
        }
        else
        {
            ddlSuper.Enabled = true;
            ddlSuper.SelectedIndex = 0;
            pnlGrid.Visible = false;
        }
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyPress", "Materialize.updateTextFields();", true);
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
            mail.SendTest();

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
            ddlSalaryPeriod.Enabled = false;
        }
        else if (ddlPayMethod.SelectedValue == "0")
        {
            txtHours.Enabled = false;
            txtAmount.Enabled = true;
            ddlSalaryPeriod.Enabled = true;
        }
        else
        {
            txtAmount.Enabled = false;
            txtHours.Enabled = false;
            ddlSalaryPeriod.Enabled = false;
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
        
        dt.Columns.Add("GL", typeof(int));
        dt.Columns.Add("FIT", typeof(int));
        dt.Columns.Add("FICA", typeof(int));
        dt.Columns.Add("MEDI", typeof(int));
        dt.Columns.Add("FUTA", typeof(int));
        dt.Columns.Add("SIT", typeof(int));
        dt.Columns.Add("Vac", typeof(int));
        dt.Columns.Add("Wc", typeof(int));
        dt.Columns.Add("Uni", typeof(int));
        dt.Columns.Add("InUse", typeof(int));
        dt.Columns.Add("Sick", typeof(int));
        dt.Columns.Add("Status", typeof(int));
        dt.Columns.Add("YTD", typeof(double));
        dt.Columns.Add("YTDH", typeof(double));
        dt.Columns.Add("OYTD", typeof(double));
        dt.Columns.Add("OYTDH", typeof(double));
        dt.Columns.Add("DYTD", typeof(double));
        dt.Columns.Add("DYTDH", typeof(double));
        dt.Columns.Add("TYTD", typeof(double));
        dt.Columns.Add("TYTDH", typeof(double));
        dt.Columns.Add("NYTD", typeof(double));
        dt.Columns.Add("NYTDH", typeof(double));
        dt.Columns.Add("VacR", typeof(string));
        dt.Columns.Add("fdesc", typeof(string));
        dt.Columns.Add("GLName", typeof(string));
        dt.Columns.Add("StatusName", typeof(string));

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

        dr["GL"] = 0;
        dr["FIT"] = 0;
        dr["FICA"] = 0;
        dr["MEDI"] = 0;
        dr["FUTA"] = 0;
        dr["SIT"] = 0;
        dr["Vac"] = 0;
        dr["Wc"] = 0;
        dr["Uni"] = 0;
        dr["InUse"] = 0;
        dr["Sick"] = 0;
        dr["Status"] = 0;
        dr["YTD"] = 0;
        dr["YTDH"] = 0;
        dr["OYTD"] = 0;
        dr["OYTDH"] = 0;
        dr["DYTD"] = 0;
        dr["DYTDH"] = 0;
        dr["TYTD"] = 0;
        dr["TYTDH"] = 0;
        dr["NYTD"] = 0;
        dr["NYTDH"] = 0;
        dr["VacR"] = "";
        dr["fdesc"] = "";
        dr["GLName"] = "";
        dr["StatusName"] = "";
        dt.Rows.Add(dr);
        ViewState["WageItems"] = dt;
        gvWagePayRate.DataSource = dt;
        //gvWagePayRate.DataBind();
    }
    private void FillWageCategory()
    {
        try
        {
            DataSet ds = new DataSet();
            objWage.ConnConfig = HttpContext.Current.Session["config"].ToString();
            ds = objBL_User.GetAllWage(objWage);
            if (ds.Tables.Count > 0)
            {
                DataTable filterdt = new DataTable();
                if (ds.Tables[0].Rows.Count > 0)
                {                    
                    DataRow[] drf = ds.Tables[0].Select("Status=0");
                    if (drf.Length > 0)
                    {
                        filterdt = drf.CopyToDataTable();
                    }
                    else
                    {
                        filterdt = ds.Tables[0].Clone();
                    }
                }
                else
                {
                    filterdt = ds.Tables[0];

                }

                DataView view = filterdt.DefaultView;
                view.Sort = "fDesc ASC";
                filterdt = view.ToTable();

                DataRow dr = filterdt.NewRow();
                dr["Wage"] = 0;
                dr["fDesc"] = "Select Wage";
                filterdt.Rows.InsertAt(dr, 0);


                dtWage = filterdt;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvWagePayRate_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = gvWagePayRate.Items.Count - 1;
        GridDataItem row = gvWagePayRate.Items[rowIndex];
        //HiddenField hdnIndex = row.FindControl("hdnIndex") as HiddenField;

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

        dr["GL"] = 0;
        dr["FIT"] = 0;
        dr["FICA"] = 0;
        dr["MEDI"] = 0;
        dr["FUTA"] = 0;
        dr["SIT"] = 0;
        dr["Vac"] = 0;
        dr["Wc"] = 0;
        dr["Uni"] = 0;
        dr["InUse"] = 0;
        dr["Sick"] = 0;
        dr["Status"] = 0;
        dr["YTD"] = 0;
        dr["YTDH"] = 0;
        dr["OYTD"] = 0;
        dr["OYTDH"] = 0;
        dr["DYTD"] = 0;
        dr["DYTDH"] = 0;
        dr["TYTD"] = 0;
        dr["TYTDH"] = 0;
        dr["NYTD"] = 0;
        dr["NYTDH"] = 0;
        dr["VacR"] = "";
        dr["fdesc"] = "";
        dr["GLName"] = "";
        dt.Rows.Add(dr);

        gvWagePayRate.DataSource = dt;
        //gvWagePayRate.DataBind();

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
        dt.Columns.Add("Checked", typeof(bool));

        dt.Columns.Add("GL", typeof(int));
        dt.Columns.Add("FIT", typeof(int));
        dt.Columns.Add("FICA", typeof(int));
        dt.Columns.Add("MEDI", typeof(int));
        dt.Columns.Add("FUTA", typeof(int));
        dt.Columns.Add("SIT", typeof(int));
        dt.Columns.Add("Vac", typeof(int));
        dt.Columns.Add("Wc", typeof(int));
        dt.Columns.Add("Uni", typeof(int));
        dt.Columns.Add("InUse", typeof(int));
        dt.Columns.Add("Sick", typeof(int));
        dt.Columns.Add("Status", typeof(int));
        dt.Columns.Add("YTD", typeof(double));
        dt.Columns.Add("YTDH", typeof(double));
        dt.Columns.Add("OYTD", typeof(double));
        dt.Columns.Add("OYTDH", typeof(double));
        dt.Columns.Add("DYTD", typeof(double));
        dt.Columns.Add("DYTDH", typeof(double));
        dt.Columns.Add("TYTD", typeof(double));
        dt.Columns.Add("TYTDH", typeof(double));
        dt.Columns.Add("NYTD", typeof(double));
        dt.Columns.Add("NYTDH", typeof(double));
        dt.Columns.Add("VacR", typeof(string));
        dt.Columns.Add("fdesc", typeof(string));
        dt.Columns.Add("GLName", typeof(string));
        dt.Columns.Add("StatusName", typeof(string));
        try
        {
            string strItems = hdnWageRate.Value.Trim();

            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objWageItemData = new List<Dictionary<object, object>>();
                objWageItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                objWageItemData.RemoveAt(0);
                objWageItemData.RemoveAt(0);
                foreach (Dictionary<object, object> dict in objWageItemData)
                {
                    i++;
                    DataRow dr = dt.NewRow();
                    if (dict["ddlWage"].ToString() != "0")
                    {
                        dr["Wage"] = Convert.ToInt32(dict["ddlWage"]);
                    }
                    else
                    {
                        dr["Wage"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["txtReg"].ToString()))
                    {
                        dr["Reg"] = Convert.ToDouble(dict["txtReg"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtOt"].ToString()))
                    {
                        dr["OT"] = Convert.ToDouble(dict["txtOt"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtNt"].ToString()))
                    {
                        dr["NT"] = Convert.ToDouble(dict["txtNt"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtDt"].ToString()))
                    {
                        dr["DT"] = Convert.ToDouble(dict["txtDt"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtTt"].ToString()))
                    {
                        dr["TT"] = Convert.ToDouble(dict["txtTt"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtCReg"].ToString()))
                    {
                        dr["CReg"] = Convert.ToDouble(dict["txtCReg"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtCOt"].ToString()))
                    {
                        dr["COT"] = Convert.ToDouble(dict["txtCOt"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtCNt"].ToString()))
                    {
                        dr["CNT"] = Convert.ToDouble(dict["txtCNt"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtCDt"].ToString()))
                    {
                        dr["CDT"] = Convert.ToDouble(dict["txtCDt"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtCTt"].ToString()))
                    {
                        dr["CTT"] = Convert.ToDouble(dict["txtCTt"]);
                    }

                    if (!string.IsNullOrEmpty(dict["hdnGL"].ToString()))
                    {
                        dr["GL"] = Convert.ToInt32(dict["hdnGL"]);
                    }
                    else
                    {
                        dr["GL"] = 0;
                    }
                    if (dict.ContainsKey("cbFIT"))
                    {
                        if (!string.IsNullOrEmpty(dict["cbFIT"].ToString()))
                        {
                            //dr["FIT"] = Convert.ToInt32(dict["cbFIT"]);
                            if (Convert.ToString(dict["cbFIT"]) == "on")
                            {
                                dr["FIT"] = 1;
                            }
                            else
                            {
                                dr["FIT"] = 0;
                            }
                        }
                        else
                        {
                            dr["FIT"] = 0;
                        }
                    }
                    else
                    {
                        dr["FIT"] = 0;
                    }
                    if (dict.ContainsKey("cbFICA"))
                    {
                        if (!string.IsNullOrEmpty(dict["cbFICA"].ToString()))
                        {
                            //dr["FICA"] = Convert.ToInt32(dict["cbFICA"]);
                            if (Convert.ToString(dict["cbFICA"]) == "on")
                            {
                                dr["FICA"] = 1;
                            }
                            else
                            {
                                dr["FICA"] = 0;
                            }
                        }
                        else
                        {
                            dr["FICA"] = 0;
                        }
                    }
                    else
                    {
                        dr["FICA"] = 0;
                    }
                    if (dict.ContainsKey("cbMEDI"))
                    {
                        if (!string.IsNullOrEmpty(dict["cbMEDI"].ToString()))
                        {
                            //dr["MEDI"] = Convert.ToInt32(dict["cbMEDI"]);
                            if (Convert.ToString(dict["cbMEDI"]) == "on")
                            {
                                dr["MEDI"] = 1;
                            }
                            else
                            {
                                dr["MEDI"] = 0;
                            }
                        }
                        else
                        {
                            dr["MEDI"] = 0;
                        }
                    }
                    else
                    {
                        dr["MEDI"] = 0;
                    }
                    if (dict.ContainsKey("cbFUTA"))
                    {
                        if (!string.IsNullOrEmpty(dict["cbFUTA"].ToString()))
                        {
                            //dr["FUTA"] = Convert.ToInt32(dict["cbFUTA"]);
                            if (Convert.ToString(dict["cbFUTA"]) == "on")
                            {
                                dr["FUTA"] = 1;
                            }
                            else
                            {
                                dr["FUTA"] = 0;
                            }
                        }
                        else
                        {
                            dr["FUTA"] = 0;
                        }
                    }
                    else
                    {
                        dr["FUTA"] = 0;
                    }
                    if (dict.ContainsKey("cbSIT"))
                    {
                        if (!string.IsNullOrEmpty(dict["cbSIT"].ToString()))
                        {
                            //dr["SIT"] = Convert.ToInt32(dict["cbSIT"]);
                            if (Convert.ToString(dict["cbSIT"]) == "on")
                            {
                                dr["SIT"] = 1;
                            }
                            else
                            {
                                dr["SIT"] = 0;
                            }
                        }
                        else
                        {
                            dr["SIT"] = 0;
                        }
                    }
                    else
                    {
                        dr["SIT"] = 0;
                    }
                    if (dict.ContainsKey("cbVac"))
                    {
                        if (!string.IsNullOrEmpty(dict["cbVac"].ToString()))
                        {
                            //dr["Vac"] = Convert.ToInt32(dict["cbVac"]);
                            if (Convert.ToString(dict["cbVac"]) == "on")
                            {
                                dr["Vac"] = 1;
                            }
                            else
                            {
                                dr["Vac"] = 0;
                            }
                        }
                        else
                        {
                            dr["Vac"] = 0;
                        }
                    }
                    else
                    {
                        dr["Vac"] = 0;
                    }
                    if (dict.ContainsKey("cbWc"))
                    {
                        if (!string.IsNullOrEmpty(dict["cbWc"].ToString()))
                        {
                            //dr["Wc"] = Convert.ToInt32(dict["cbWc"]);
                            if (Convert.ToString(dict["cbWc"]) == "on")
                            {
                                dr["Wc"] = 1;
                            }
                            else
                            {
                                dr["Wc"] = 0;
                            }
                        }
                        else
                        {
                            dr["Wc"] = 0;
                        }
                    }
                    else
                    {
                        dr["Wc"] = 0;
                    }
                    if (dict.ContainsKey("cbUni"))
                    {
                        if (!string.IsNullOrEmpty(dict["cbUni"].ToString()))
                        {
                            //dr["Uni"] = Convert.ToInt32(dict["cbUni"]);
                            if (Convert.ToString(dict["cbUni"]) == "on")
                            {
                                dr["Uni"] = 1;
                            }
                            else
                            {
                                dr["Uni"] = 0;
                            }
                        }
                        else
                        {
                            dr["Uni"] = 0;
                        }
                    }
                    else
                    {
                        dr["Uni"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnInUse"].ToString()))
                    {
                        dr["InUse"] = Convert.ToInt32(dict["hdnInUse"]);
                        
                    }
                    else
                    {
                        dr["InUse"] = 0;
                    }
                    if (dict.ContainsKey("cbSick"))
                    {
                        if (!string.IsNullOrEmpty(dict["cbSick"].ToString()))
                        {
                            //dr["Sick"] = Convert.ToInt32(dict["cbSick"]);
                            if (Convert.ToString(dict["cbSick"]) == "on")
                            {
                                dr["Sick"] = 1;
                            }
                            else
                            {
                                dr["Sick"] = 0;
                            }
                        }
                        else
                        {
                            dr["Sick"] = 0;
                        }
                    }
                    else
                    {
                        dr["Sick"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnStatus"].ToString()))
                    {
                        dr["Status"] = Convert.ToInt32(dict["hdnStatus"]);
                    }
                    else
                    {
                        dr["Status"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnYTD"].ToString()))
                    {
                        dr["YTD"] = Convert.ToDouble(dict["hdnYTD"]);
                    }
                    else
                    {
                        dr["YTD"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnYTDH"].ToString()))
                    {
                        dr["YTDH"] = Convert.ToDouble(dict["hdnYTDH"]);
                    }
                    else
                    {
                        dr["YTDH"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnOYTD"].ToString()))
                    {
                        dr["OYTD"] = Convert.ToDouble(dict["hdnOYTD"]);
                    }
                    else
                    {
                        dr["OYTD"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnOYTDH"].ToString()))
                    {
                        dr["OYTDH"] = Convert.ToDouble(dict["hdnOYTDH"]);
                    }
                    else
                    {
                        dr["OYTDH"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnDYTD"].ToString()))
                    {
                        dr["DYTD"] = Convert.ToDouble(dict["hdnDYTD"]);
                    }
                    else
                    {
                        dr["DYTD"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnDYTDH"].ToString()))
                    {
                        dr["DYTDH"] = Convert.ToDouble(dict["hdnDYTDH"]);
                    }
                    else
                    {
                        dr["DYTDH"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnTYTD"].ToString()))
                    {
                        dr["TYTD"] = Convert.ToDouble(dict["hdnTYTD"]);
                    }
                    else
                    {
                        dr["TYTD"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnTYTDH"].ToString()))
                    {
                        dr["TYTDH"] = Convert.ToDouble(dict["hdnTYTDH"]);
                    }
                    else
                    {
                        dr["TYTDH"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnNYTD"].ToString()))
                    {
                        dr["NYTD"] = Convert.ToDouble(dict["hdnNYTD"]);
                    }
                    else
                    {
                        dr["NYTD"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnNYTDH"].ToString()))
                    {
                        dr["NYTDH"] = Convert.ToDouble(dict["hdnNYTDH"]);
                    }
                    else
                    {
                        dr["NYTDH"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["hdnVacR"].ToString()))
                    {
                        dr["VacR"] = Convert.ToString(dict["hdnVacR"]);
                    }
                    else
                    {
                        dr["VacR"] = "";
                    }
                    if (!string.IsNullOrEmpty(dict["hdnfdesc"].ToString()))
                    {
                        dr["fdesc"] = Convert.ToString(dict["hdnfdesc"]);
                    }
                    else
                    {
                        dr["fdesc"] = "";
                    }
                    if (!string.IsNullOrEmpty(dict["hdnGLName"].ToString()))
                    {
                        dr["GLName"] = Convert.ToString(dict["hdnGLName"]);
                    }
                    else
                    {
                        dr["GLName"] = "";
                    }
                    if (!string.IsNullOrEmpty(dict["hdnStatusName"].ToString()))
                    {
                        dr["StatusName"] = Convert.ToString(dict["hdnStatusName"]);
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(dict["chkSelect"].ToString()) && dict["chkSelect"].ToString() == "on")
                        {
                            dr["Checked"] = true;
                        }
                        else
                        {
                            dr["Checked"] = false;
                        }
                    }
                    catch (Exception)
                    {
                        dr["Checked"] = false;
                    }
                    
                    dt.Rows.Add(dr);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }
    private DataTable GetOtherWageGridItems()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Cat", typeof(int));
        dt.Columns.Add("GL", typeof(int));
        dt.Columns.Add("Rate", typeof(double));
        dt.Columns.Add("FIT", typeof(int));
        dt.Columns.Add("FICA", typeof(int));
        dt.Columns.Add("MEDI", typeof(int));
        dt.Columns.Add("FUTA", typeof(int));
        dt.Columns.Add("SIT", typeof(int));
        dt.Columns.Add("Vac", typeof(int));
        dt.Columns.Add("Wc", typeof(int));
        dt.Columns.Add("Uni", typeof(int));
        dt.Columns.Add("Sick", typeof(int));
        dt.Columns.Add("ExpAcctName", typeof(string));
        dt.Columns.Add("fDesc", typeof(string));


        try
        {
            string strItems = hdnWageOtherRate.Value.Trim();

            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objWageItemData = new List<Dictionary<object, object>>();
                objWageItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                //objWageItemData.RemoveAt(0);
                //objWageItemData.RemoveAt(0);
                foreach (Dictionary<object, object> dict in objWageItemData)
                {
                    i++;
                    DataRow dr = dt.NewRow();
                    
                    if (!string.IsNullOrEmpty(dict["hdnotherid"].ToString()))
                    {
                        dr["Cat"] = Convert.ToInt32(dict["hdnotherid"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdnotherfdesc"].ToString()))
                    {
                        dr["fDesc"] = Convert.ToString(dict["hdnotherfdesc"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtOtherRate"].ToString()))
                    {
                        dr["Rate"] = Convert.ToDouble(dict["txtOtherRate"]);
                    }
                    else
                    {
                        dr["Rate"] = 0;
                    }
                    if (!string.IsNullOrEmpty(dict["txtotherGL"].ToString()))
                    {
                        dr["ExpAcctName"] = Convert.ToString(dict["txtotherGL"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdnotherGL"].ToString()))
                    {
                        dr["GL"] = Convert.ToInt32(dict["hdnotherGL"]);
                    }
                    else
                    {
                        dr["GL"] = 0;
                    }
                    
                    if (dict.ContainsKey("cboFIT"))
                    {
                        if (!string.IsNullOrEmpty(dict["cboFIT"].ToString()))
                        {
                            //dr["FIT"] = Convert.ToInt32(dict["cbFIT"]);
                            if (Convert.ToString(dict["cboFIT"]) == "on")
                            {
                                dr["FIT"] = 1;
                            }
                            else
                            {
                                dr["FIT"] = 0;
                            }
                        }
                        else
                        {
                            dr["FIT"] = 0;
                        }
                    }
                    else
                    {
                        dr["FIT"] = 0;
                    }

                    if (dict.ContainsKey("cboFICA"))
                    {
                        if (!string.IsNullOrEmpty(dict["cboFICA"].ToString()))
                        {
                            //dr["FIT"] = Convert.ToInt32(dict["cbFIT"]);
                            if (Convert.ToString(dict["cboFICA"]) == "on")
                            {
                                dr["FICA"] = 1;
                            }
                            else
                            {
                                dr["FICA"] = 0;
                            }
                        }
                        else
                        {
                            dr["FICA"] = 0;
                        }
                    }
                    else
                    {
                        dr["FICA"] = 0;
                    }

                    if (dict.ContainsKey("cboMEDI"))
                    {
                        if (!string.IsNullOrEmpty(dict["cboMEDI"].ToString()))
                        {
                            //dr["FIT"] = Convert.ToInt32(dict["cbFIT"]);
                            if (Convert.ToString(dict["cboMEDI"]) == "on")
                            {
                                dr["MEDI"] = 1;
                            }
                            else
                            {
                                dr["MEDI"] = 0;
                            }
                        }
                        else
                        {
                            dr["MEDI"] = 0;
                        }
                    }
                    else
                    {
                        dr["MEDI"] = 0;
                    }

                    if (dict.ContainsKey("cboFUTA"))
                    {
                        if (!string.IsNullOrEmpty(dict["cboFUTA"].ToString()))
                        {
                            //dr["FIT"] = Convert.ToInt32(dict["cbFIT"]);
                            if (Convert.ToString(dict["cboFUTA"]) == "on")
                            {
                                dr["FUTA"] = 1;
                            }
                            else
                            {
                                dr["FUTA"] = 0;
                            }
                        }
                        else
                        {
                            dr["FUTA"] = 0;
                        }
                    }
                    else
                    {
                        dr["FUTA"] = 0;
                    }

                    if (dict.ContainsKey("cboSIT"))
                    {
                        if (!string.IsNullOrEmpty(dict["cboSIT"].ToString()))
                        {
                            //dr["FIT"] = Convert.ToInt32(dict["cbFIT"]);
                            if (Convert.ToString(dict["cboSIT"]) == "on")
                            {
                                dr["SIT"] = 1;
                            }
                            else
                            {
                                dr["SIT"] = 0;
                            }
                        }
                        else
                        {
                            dr["SIT"] = 0;
                        }
                    }
                    else
                    {
                        dr["SIT"] = 0;
                    }

                    if (dict.ContainsKey("cboVac"))
                    {
                        if (!string.IsNullOrEmpty(dict["cboVac"].ToString()))
                        {
                            //dr["FIT"] = Convert.ToInt32(dict["cbFIT"]);
                            if (Convert.ToString(dict["cboVac"]) == "on")
                            {
                                dr["Vac"] = 1;
                            }
                            else
                            {
                                dr["Vac"] = 0;
                            }
                        }
                        else
                        {
                            dr["Vac"] = 0;
                        }
                    }
                    else
                    {
                        dr["Vac"] = 0;
                    }

                    if (dict.ContainsKey("cboWc"))
                    {
                        if (!string.IsNullOrEmpty(dict["cboWc"].ToString()))
                        {
                            //dr["FIT"] = Convert.ToInt32(dict["cbFIT"]);
                            if (Convert.ToString(dict["cboWc"]) == "on")
                            {
                                dr["Wc"] = 1;
                            }
                            else
                            {
                                dr["Wc"] = 0;
                            }
                        }
                        else
                        {
                            dr["Wc"] = 0;
                        }
                    }
                    else
                    {
                        dr["Wc"] = 0;
                    }

                    if (dict.ContainsKey("cboUni"))
                    {
                        if (!string.IsNullOrEmpty(dict["cboUni"].ToString()))
                        {
                            //dr["FIT"] = Convert.ToInt32(dict["cbFIT"]);
                            if (Convert.ToString(dict["cboUni"]) == "on")
                            {
                                dr["Uni"] = 1;
                            }
                            else
                            {
                                dr["Uni"] = 0;
                            }
                        }
                        else
                        {
                            dr["Uni"] = 0;
                        }
                    }
                    else
                    {
                        dr["Uni"] = 0;
                    }

                    if (dict.ContainsKey("cboSick"))
                    {
                        if (!string.IsNullOrEmpty(dict["cboSick"].ToString()))
                        {
                            //dr["FIT"] = Convert.ToInt32(dict["cbFIT"]);
                            if (Convert.ToString(dict["cboSick"]) == "on")
                            {
                                dr["Sick"] = 1;
                            }
                            else
                            {
                                dr["Sick"] = 0;
                            }
                        }
                        else
                        {
                            dr["Sick"] = 0;
                        }
                    }
                    else
                    {
                        dr["Sick"] = 0;
                    }


                    
                    

                    dt.Rows.Add(dr);
                }
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
                        DropDownList ddlWage = (DropDownList)gr.FindControl("ddlWage");
                        Int32 UserID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"]);
                        if (!IsWageRateIsUsed(UserID, Convert.ToInt32(ddlWage.SelectedItem.Value), out TicketID))
                        {
                            DataRow dr = dt.Select("Wage = " + Convert.ToInt32(ddlWage.SelectedValue)).FirstOrDefault();
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
                        DropDownList ddlWage = (DropDownList)gr.FindControl("ddlWage");
                        Int32 UserID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"]);
                        if (!IsWageRateIsUsed(UserID, Convert.ToInt32(ddlWage.SelectedItem.Value), out TicketID))
                        {
                            if (ddlWage.SelectedValue != "0")
                            {
                                DataRow dr = dt.Select("Wage = " + Convert.ToInt32(ddlWage.SelectedValue)).FirstOrDefault();
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

                                dr["GL"] = 0;
                                dr["FIT"] = 0;
                                dr["FICA"] = 0;
                                dr["MEDI"] = 0;
                                dr["FUTA"] = 0;
                                dr["SIT"] = 0;
                                dr["Vac"] = 0;
                                dr["Wc"] = 0;
                                dr["Uni"] = 0;
                                dr["InUse"] = 0;
                                dr["Sick"] = 0;
                                dr["Status"] = 0;
                                dr["YTD"] = 0;
                                dr["YTDH"] = 0;
                                dr["OYTD"] = 0;
                                dr["OYTDH"] = 0;
                                dr["DYTD"] = 0;
                                dr["DYTDH"] = 0;
                                dr["TYTD"] = 0;
                                dr["TYTDH"] = 0;
                                dr["NYTD"] = 0;
                                dr["NYTDH"] = 0;
                                dr["VacR"] = "";
                                dr["fdesc"] = "";
                                dr["GLName"] = "";

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
            //gvWagePayRate.DataBind();
            gvWagePayRate.Rebind();

            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "RemoveWageGridRow", "RemoveWageGridRow('" + gvWagePayRate.ClientID + "')", true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlWage_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlWage = (DropDownList)sender;
            GridDataItem row = (GridDataItem)ddlWage.NamingContainer;
            TextBox txtReg = (TextBox)row.FindControl("txtReg");
            TextBox txtOt = (TextBox)row.FindControl("txtOt");
            TextBox txtNt = (TextBox)row.FindControl("txtNt");
            TextBox txtDt = (TextBox)row.FindControl("txtDt");
            TextBox txtTt = (TextBox)row.FindControl("txtTt");
            TextBox txtCReg = (TextBox)row.FindControl("txtCReg");
            TextBox txtCOt = (TextBox)row.FindControl("txtCOt");
            TextBox txtCNt = (TextBox)row.FindControl("txtCNt");
            TextBox txtCDt = (TextBox)row.FindControl("txtCDt");
            TextBox txtCTt = (TextBox)row.FindControl("txtCTt");

            HiddenField hdnGL = (HiddenField)row.FindControl("hdnGL");
            HiddenField hdnFIT = (HiddenField)row.FindControl("hdnFIT");
            HiddenField hdnFICA = (HiddenField)row.FindControl("hdnFICA");
            HiddenField hdnMEDI = (HiddenField)row.FindControl("hdnMEDI");
            HiddenField hdnFUTA = (HiddenField)row.FindControl("hdnFUTA");
            HiddenField hdnSIT = (HiddenField)row.FindControl("hdnSIT");
            HiddenField hdnVac = (HiddenField)row.FindControl("hdnVac");
            HiddenField hdnWc = (HiddenField)row.FindControl("hdnWc");
            HiddenField hdnUni = (HiddenField)row.FindControl("hdnUni");
            HiddenField hdnInUse = (HiddenField)row.FindControl("hdnInUse");
            HiddenField hdnSick = (HiddenField)row.FindControl("hdnSick");
            HiddenField hdnStatus = (HiddenField)row.FindControl("hdnStatus");
            HiddenField hdnYTD = (HiddenField)row.FindControl("hdnYTD");
            HiddenField hdnYTDH = (HiddenField)row.FindControl("hdnYTDH");
            HiddenField hdnOYTD = (HiddenField)row.FindControl("hdnOYTD");
            HiddenField hdnOYTDH = (HiddenField)row.FindControl("hdnOYTDH");
            HiddenField hdnDYTD = (HiddenField)row.FindControl("hdnDYTD");
            HiddenField hdnDYTDH = (HiddenField)row.FindControl("hdnDYTDH");
            HiddenField hdnTYTD = (HiddenField)row.FindControl("hdnTYTD");
            HiddenField hdnTYTDH = (HiddenField)row.FindControl("hdnTYTDH");
            HiddenField hdnNYTD = (HiddenField)row.FindControl("hdnNYTD");
            HiddenField hdnNYTDH = (HiddenField)row.FindControl("hdnNYTDH");
            HiddenField hdnVacR = (HiddenField)row.FindControl("hdnVacR");
            HiddenField hdnfdesc = (HiddenField)row.FindControl("hdnfdesc");
            HiddenField hdnGLName = (HiddenField)row.FindControl("hdnGLName");

            CheckBox cbFIT = (CheckBox)row.FindControl("cbFIT");            
            CheckBox cbFICA = (CheckBox)row.FindControl("cbFICA");
            CheckBox cbMEDI = (CheckBox)row.FindControl("cbMEDI");
            CheckBox cbFUTA = (CheckBox)row.FindControl("cbFUTA");
            CheckBox cbSIT = (CheckBox)row.FindControl("cbSIT");
            CheckBox cbVac = (CheckBox)row.FindControl("cbVac");
            CheckBox cbWc = (CheckBox)row.FindControl("cbWc");
            CheckBox cbUni = (CheckBox)row.FindControl("cbUni");
            CheckBox cbSick = (CheckBox)row.FindControl("cbSick");

            Label lblStatus = (Label)row.FindControl("lblStatus");
            HiddenField hdnStatusName = (HiddenField)row.FindControl("hdnStatusName");

            objWage.ConnConfig = Session["config"].ToString();
            objWage.ID = Convert.ToInt32(ddlWage.SelectedValue);
            DataSet ds = objBL_User.GetWageByID(objWage);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtReg.Text = string.Format("{0:n}", Convert.ToDouble(dr["Reg"] == DBNull.Value ? 0 : dr["Reg"]));
                txtOt.Text = string.Format("{0:n}", Convert.ToDouble(dr["OT1"] == DBNull.Value ? 0 : dr["OT1"]));
                txtNt.Text = string.Format("{0:n}", Convert.ToDouble(dr["NT"] == DBNull.Value ? 0 : dr["NT"]));
                txtDt.Text = string.Format("{0:n}", Convert.ToDouble(dr["OT2"] == DBNull.Value ? 0 : dr["OT2"]));
                txtTt.Text = string.Format("{0:n}", Convert.ToDouble(dr["TT"] == DBNull.Value ? 0 : dr["TT"]));
                txtCReg.Text = string.Format("{0:n}", Convert.ToDouble(dr["CReg"] == DBNull.Value ? 0 : dr["CReg"]));
                txtCOt.Text = string.Format("{0:n}", Convert.ToDouble(dr["COT"] == DBNull.Value ? 0 : dr["COT"]));
                txtCNt.Text = string.Format("{0:n}", Convert.ToDouble(dr["CNT"] == DBNull.Value ? 0 : dr["CNT"]));
                txtCDt.Text = string.Format("{0:n}", Convert.ToDouble(dr["CDT"] == DBNull.Value ? 0 : dr["CDT"]));
                txtCTt.Text = string.Format("{0:n}", Convert.ToDouble(dr["CTT"] == DBNull.Value ? 0 : dr["CTT"]));

                hdnGL.Value = Convert.ToString((Convert.ToInt32(dr["GL"] == DBNull.Value ? 0 : dr["GL"])));
                hdnFIT.Value = Convert.ToString(Convert.ToInt32(dr["FIT"] == DBNull.Value ? 0 : dr["FIT"]));
                hdnFICA.Value = Convert.ToString(Convert.ToInt32(dr["FICA"] == DBNull.Value ? 0 : dr["FICA"]));
                hdnMEDI.Value = Convert.ToString(Convert.ToInt32(dr["MEDI"] == DBNull.Value ? 0 : dr["MEDI"]));
                hdnFUTA.Value = Convert.ToString(Convert.ToInt32(dr["FUTA"] == DBNull.Value ? 0 : dr["FUTA"]));
                hdnSIT.Value = Convert.ToString(Convert.ToInt32(dr["SIT"] == DBNull.Value ? 0 : dr["SIT"]));
                hdnVac.Value = Convert.ToString(Convert.ToInt32(dr["Vac"] == DBNull.Value ? 0 : dr["Vac"]));
                hdnWc.Value = Convert.ToString(Convert.ToInt32(dr["Wc"] == DBNull.Value ? 0 : dr["Wc"]));
                hdnUni.Value = Convert.ToString(Convert.ToInt32(dr["Uni"] == DBNull.Value ? 0 : dr["Uni"]));
                hdnInUse.Value = "0";
                hdnSick.Value = Convert.ToString(Convert.ToInt32(dr["Sick"] == DBNull.Value ? 0 : dr["Sick"]));
                hdnStatus.Value = Convert.ToString(Convert.ToInt32(dr["Status"] == DBNull.Value ? 0 : dr["Status"]));
                hdnYTD.Value = "0.00";
                hdnYTDH.Value = "0.00";
                hdnOYTD.Value = "0.00";
                hdnOYTDH.Value = "0.00";
                hdnDYTD.Value = "0.00";
                hdnDYTDH.Value = "0.00";
                hdnTYTD.Value = "0.00";
                hdnTYTDH.Value = "0.00";
                hdnNYTD.Value = "0.00";
                hdnNYTDH.Value = "0.00";
                hdnVacR.Value = "";
                hdnfdesc.Value = Convert.ToString(dr["fdesc"] == DBNull.Value ? 0 : dr["fdesc"]);
                hdnGLName.Value = Convert.ToString(dr["GLName"] == DBNull.Value ? 0 : dr["GLName"]);

                cbFIT.Checked = Convert.ToBoolean(Convert.ToInt32(hdnFIT.Value));
                cbFICA.Checked = Convert.ToBoolean(Convert.ToInt32(hdnFICA.Value));
                cbMEDI.Checked = Convert.ToBoolean(Convert.ToInt32(hdnMEDI.Value));
                cbFUTA.Checked = Convert.ToBoolean(Convert.ToInt32(hdnFUTA.Value));
                cbSIT.Checked = Convert.ToBoolean(Convert.ToInt32(hdnSIT.Value));
                cbVac.Checked = Convert.ToBoolean(Convert.ToInt32(hdnVac.Value));
                cbWc.Checked = Convert.ToBoolean(Convert.ToInt32(hdnWc.Value));
                cbUni.Checked = Convert.ToBoolean(Convert.ToInt32(hdnUni.Value));
                cbSick.Checked = Convert.ToBoolean(Convert.ToInt32(hdnSick.Value));
                if (Convert.ToString(dr["Status"]).Trim() == "0")
                {
                    lblStatus.Text = "Active";
                    hdnStatusName.Value = "Active";
                }
                else
                {
                    lblStatus.Text = "Inactive";
                    hdnStatusName.Value = "Inactive";
                }
            }
            else
            {
                txtReg.Text = string.Format("{0:n}", 0);
                txtOt.Text = string.Format("{0:n}", 0);
                txtNt.Text = string.Format("{0:n}", 0);
                txtDt.Text = string.Format("{0:n}", 0);
                txtTt.Text = string.Format("{0:n}", 0);
                txtCReg.Text = string.Format("{0:n}", 0);
                txtCOt.Text = string.Format("{0:n}", 0);
                txtCNt.Text = string.Format("{0:n}", 0);
                txtCDt.Text = string.Format("{0:n}", 0);
                txtCTt.Text = string.Format("{0:n}", 0);

                hdnGL.Value = "0";
                hdnFIT.Value = "0";
                hdnFICA.Value = "0";
                hdnMEDI.Value = "0";
                hdnFUTA.Value = "0";
                hdnSIT.Value = "0";
                hdnVac.Value = "0";
                hdnWc.Value = "0";
                hdnUni.Value = "0";
                hdnInUse.Value = "0";
                hdnSick.Value = "0";
                hdnStatus.Value = "0";
                hdnYTD.Value = string.Format("{0:n}", 0);
                hdnYTDH.Value = string.Format("{0:n}", 0);
                hdnOYTD.Value = string.Format("{0:n}", 0);
                hdnOYTDH.Value = string.Format("{0:n}", 0);
                hdnDYTD.Value = string.Format("{0:n}", 0);
                hdnDYTDH.Value = string.Format("{0:n}", 0);
                hdnTYTD.Value = string.Format("{0:n}", 0);
                hdnTYTDH.Value = string.Format("{0:n}", 0);
                hdnNYTD.Value = string.Format("{0:n}", 0);
                hdnNYTDH.Value = string.Format("{0:n}", 0);
                hdnVacR.Value = "";
                hdnfdesc.Value = "";
                hdnGLName.Value = "";

                cbFIT.Checked = Convert.ToBoolean(Convert.ToInt32(hdnFIT.Value));
                cbFICA.Checked = Convert.ToBoolean(Convert.ToInt32(hdnFICA.Value));
                cbMEDI.Checked = Convert.ToBoolean(Convert.ToInt32(hdnMEDI.Value));
                cbFUTA.Checked = Convert.ToBoolean(Convert.ToInt32(hdnFUTA.Value));
                cbSIT.Checked = Convert.ToBoolean(Convert.ToInt32(hdnSIT.Value));
                cbVac.Checked = Convert.ToBoolean(Convert.ToInt32(hdnVac.Value));
                cbWc.Checked = Convert.ToBoolean(Convert.ToInt32(hdnWc.Value));
                cbUni.Checked = Convert.ToBoolean(Convert.ToInt32(hdnUni.Value));
                cbSick.Checked = Convert.ToBoolean(Convert.ToInt32(hdnSick.Value));
                lblStatus.Text = "";
                hdnStatusName.Value = "";
            }
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
            DropDownList ddlWage = (DropDownList)e.Item.FindControl("ddlWage");
            Int32 UserID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"]);

            if (IsWageRateIsUsed(UserID, Convert.ToInt32(ddlWage.SelectedItem.Value), out TicketID))
            {
                string str = "Wage category is used in Ticket #" + TicketID + "!";

                ddlWage.Attributes["onclick"] = "   noty({ text: '" + str + "', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";

                foreach (ListItem item in ddlWage.Items)
                {
                    if (item.Value != ddlWage.SelectedItem.Value) { item.Enabled = false; }
                }
            }


        }
    }


    protected void gvWagePayRate_ItemCreated(object sender, GridItemEventArgs e)
    {
        //DataTable dt = new DataTable();
        //dt.Columns.Add("chkSelect", typeof(bool));
        //dt.Columns.Add("No.", typeof(int));
        //dt.Columns.Add("Wage", typeof(string));
        //dt.Columns.Add("Reg", typeof(float));
        //dt.Columns.Add("OT", typeof(float));
        //dt.Columns.Add("NT", typeof(float));
        //dt.Columns.Add("DT", typeof(float));
        //dt.Columns.Add("TT", typeof(float));
        //dt.Columns.Add("CReg", typeof(float));
        //dt.Columns.Add("COT", typeof(float));
        //dt.Columns.Add("CNT", typeof(float));
        //dt.Columns.Add("CDT", typeof(float));
        //dt.Columns.Add("CTT", typeof(float));



        if (e.Item is GridDataItem)
        {
            //CreateWageTable();
            //    RadGrid HeaderGrid = (RadGrid)sender;
            //    HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            //    HeaderGridRow.Font.Bold = true;
            //    HeaderGridRow.HorizontalAlign = HorizontalAlign.Center;

            //    TableCell HeaderCell = new TableCell();
            //    HeaderCell.Text = "";
            //    HeaderGridRow.Cells.Add(HeaderCell);

            //    HeaderCell = new TableCell();
            //    HeaderCell.Text = "";
            //    HeaderGridRow.Cells.Add(HeaderCell);

            //    HeaderCell = new TableCell();
            //    HeaderCell.Text = "";
            //    HeaderGridRow.Cells.Add(HeaderCell);

            //    HeaderCell = new TableCell();
            //    HeaderCell.Text = "Pay Rate";
            //    HeaderCell.ColumnSpan = 5;
            //    HeaderGridRow.Cells.Add(HeaderCell);

            //    HeaderCell = new TableCell();
            //    HeaderCell.Text = "Burden Rate";
            //    HeaderCell.ColumnSpan = 5;
            //    HeaderGridRow.Cells.Add(HeaderCell);

            //    gvWagePayRate.Controls[0].Controls.AddAt(0, HeaderGridRow);

        }
    }

    protected void btnTestIncoming_Click(object sender, EventArgs e)
    {
        try
        {
            //Pop3Client pop3Client = new Pop3Client();
            //if (pop3Client.Connected)
            //    pop3Client.Disconnect();

            //pop3Client.Connect(txtInServer.Text.Trim(), int.Parse(txtinPort.Text.Trim()), true);
            //pop3Client.Authenticate(txtInUSername.Text.Trim(), txtInPassword.Text.Trim());

            //int count = pop3Client.GetMessageCount();

            //using (ImapClient client = new ImapClient())
            //{
            //    if (client.Connect())
            //        client.Disconnect();

            //    try
            //    {
            //        //if (client.Connect(txtInServer.Text.Trim(), Convert.ToInt32(txtinPort.Text.Trim()), chkSSL.Checked, false))
            //        if (client.Connect(txtInServer.Text.Trim(), Convert.ToInt32(txtinPort.Text.Trim()), SecureSocketOptions.Auto))
            //        {
            //            if (client.Login(txtInUSername.Text.Trim(), txtInPassword.Text.Trim()))
            //            {
            //                //int count = client.Folders.Inbox.Messages.Count();
            //                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Connection Successful');", true);
            //                //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('" + count.ToString() + " emails found.');", true);
            //                client.Disconnect();
            //            }
            //            else
            //            {
            //                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Invalid Credentials');", true);
            //            }
            //        }
            //        else
            //        {
            //            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Connection Failed');", true);
            //        }
            //    }
            //    catch (ImapX.Exceptions.ServerAlertException ex)
            //    {
            //        throw ex;
            //    }
            //    catch (ImapX.Exceptions.OperationFailedException ex)
            //    {
            //        throw ex;
            //    }
            //    catch (ImapX.Exceptions.InvalidStateException ex)
            //    {
            //        throw ex;
            //    }
            //}

            using (var client = new MailKit.Net.Imap.ImapClient())
            {
                if (client.IsConnected)
                    client.Disconnect(true);

                try
                {
                    if (chkSSL.Checked) {
                        try
                        {
                            client.Connect(txtInServer.Text.Trim(), Convert.ToInt32(txtinPort.Text.Trim()), SecureSocketOptions.SslOnConnect);
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
                            client.Authenticate(txtInUSername.Text.Trim(), txtInPassword.Text.Trim());
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Connection Successful');", true);
                            client.Disconnect(true);
                        }
                        catch (Exception)
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

        //catch (InvalidLoginException)
        //{
        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('The server did not accept the user credentials!');", true);
        //}
        //catch (PopServerNotFoundException)
        //{
        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('The server could not be found');", true);
        //}
        //catch (PopServerLockedException)
        //{
        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('The mailbox is locked. It might be in use or under maintenance. Are you connected elsewhere?');", true);
        //}
        //catch (LoginDelayException)
        //{
        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Login not allowed. Server enforces delay between logins. Have you connected recently?');", true);
        //}
        //catch (Exception ex)
        //{
        //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        //}
    }

    private void AddNewRow()
    {
        //DataTable dt = new DataTable();
        //dt = (DataTable)ViewState["WageItems"];
        //DataTable dt1 = new DataTable();
        //dt.Columns.Add("Wage", typeof(int));
        //dt.Columns.Add("Reg", typeof(double));
        //dt.Columns.Add("OT", typeof(double));
        //dt.Columns.Add("DT", typeof(double));
        //dt.Columns.Add("TT", typeof(double));
        //dt.Columns.Add("NT", typeof(double));
        //dt.Columns.Add("CReg", typeof(double));
        //dt.Columns.Add("COT", typeof(double));
        //dt.Columns.Add("CDT", typeof(double));
        //dt.Columns.Add("CTT", typeof(double));
        //dt.Columns.Add("CNT", typeof(double));
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

        dr["GL"] = 0;
        dr["FIT"] = 0;
        dr["FICA"] = 0;
        dr["MEDI"] = 0;
        dr["FUTA"] = 0;
        dr["SIT"] = 0;
        dr["Vac"] = 0;
        dr["Wc"] = 0;
        dr["Uni"] = 0;
        dr["InUse"] = 0;
        dr["Sick"] = 0;
        dr["Status"] = 0;
        dr["YTD"] = 0;
        dr["YTDH"] = 0;
        dr["OYTD"] = 0;
        dr["OYTDH"] = 0;
        dr["DYTD"] = 0;
        dr["DYTDH"] = 0;
        dr["TYTD"] = 0;
        dr["TYTDH"] = 0;
        dr["NYTD"] = 0;
        dr["NYTDH"] = 0;
        dr["VacR"] = "";
        dr["fdesc"] = "";
        dr["GLName"] = "";
        dr["StatusName"] = "";

        dt.Rows.Add(dr);
        //dt1 = (DataTable)ViewState["WageItems"];
        gvWagePayRate.DataSource = dt;
        //gvWagePayRate.DataBind();
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
                if (!string.IsNullOrEmpty(Request.QueryString["uid"].ToString()))
                {
                    //objPropUser.ConnConfig = Session["config"].ToString();
                    //objPropUser.EmpId = Convert.ToInt32(ViewState["empid"].ToString());
                    //DataSet dsWage = objBL_User.GetEmpWageItems(objPropUser);
                    //ViewState["dsWage"] = dsWage;

                    //if (dsWage.Tables[0].Rows.Count > 0)
                    //{
                    //    gvWagePayRate.DataSource = dsWage.Tables[0];
                    //    //gvWagePayRate.DataBind();
                    //    ViewState["WageItems"] = dsWage.Tables[0];
                    //}
                    //else
                    //{
                    //    CreateWageTable();
                    //}

                    DataSet dsWage = new DataSet();
                    _objEmp.ConnConfig = Session["config"].ToString();
                    _objEmp.ID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                    dsWage = objBL_Wage.GetEmployeeListByID(_objEmp);
                    ViewState["dsWage"] = dsWage;

                    if (dsWage.Tables[1].Rows.Count > 0)
                    {

                        DataTable filterdt = new DataTable();
                        check = Convert.ToBoolean(Session["InInActiveWageEmp"]);
                        if (check)
                        {
                            lnkChk.Checked = true;
                            filterdt = dsWage.Tables[1];
                        }
                        else
                        {
                            if (dsWage.Tables[1].Rows.Count > 0)
                            {
                                DataRow[] dr = dsWage.Tables[1].Select("StatusName='Active'");
                                if (dr.Length > 0)
                                {
                                    filterdt = dr.CopyToDataTable();
                                }
                                else
                                {
                                    filterdt = dsWage.Tables[1].Clone();
                                }
                            }
                            else
                            {
                                filterdt = dsWage.Tables[1];

                            }
                        }
                        gvWagePayRate.VirtualItemCount = filterdt.Rows.Count;
                        gvWagePayRate.DataSource = filterdt;
                        ViewState["WageItems"] = filterdt;



                        //gvWagePayRate.DataSource = dsWage.Tables[1];
                        //ViewState["WageItems"] = dsWage.Tables[1];
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

            foreach (DataRow dr in dtSupersUsers.Rows)
            {
                if (lblUserID.Text == dr["userid"].ToString())
                {
                    chkSelected.Checked = true;
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
    }
    protected void chkChartView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkChartView.Checked == false)
            chkChartAdd.Checked = chkChartEdit.Checked = chkChartDelete.Checked = chkChartView.Checked;
        if (chkFinancialmodule.Checked == false && chkChartView.Checked == true)
            chkFinancialmodule.Checked = true;
    }
    protected void chkJournalEntryView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkJournalEntryView.Checked == false)
            chkJournalEntryAdd.Checked = chkJournalEntryEdit.Checked = chkJournalEntryDelete.Checked = chkJournalEntryView.Checked;
        if (chkFinancialmodule.Checked == false && chkJournalEntryView.Checked == true)
            chkFinancialmodule.Checked = true;
    }
    protected void chkBankView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkBankView.Checked == false)
            chkBankAdd.Checked = chkBankEdit.Checked = chkBankDelete.Checked = chkBankView.Checked;
        if (chkFinancialmodule.Checked == false && chkBankView.Checked == true)
            chkFinancialmodule.Checked = true;
    }
    protected void chkAccountPayable_CheckedChanged(object sender, EventArgs e)
    {
        chkVendorsAdd.Checked = chkVendorsEdit.Checked = chkVendorsDelete.Checked = chkVendorsView.Checked =
        chkAddBills.Checked = chkEditBills.Checked = chkDeleteBills.Checked = chkViewBills.Checked =
        chkAddManageChecks.Checked = chkEditManageChecks.Checked = chkDeleteManageChecks.Checked = 
        chkShowBankBalances.Checked = chkViewManageChecks.Checked = chkAccountPayable.Checked;
    }
    protected void chkPurchasingmodule_CheckedChanged(object sender, EventArgs e)
    {
        chkPOAdd.Checked = chkPOEdit.Checked = chkPODelete.Checked = chkPOView.Checked =
        chkRPOAdd.Checked = chkRPOEdit.Checked = chkRPODelete.Checked = chkRPOView.Checked = chkPONotification.Checked =
        chkPurchasingmodule.Checked;
    }
    protected void chkPOView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkPOView.Checked == false)
            chkPOAdd.Checked = chkPOEdit.Checked = chkPODelete.Checked = chkPOView.Checked;
        if (chkPurchasingmodule.Checked == false && chkPOView.Checked == true)
            chkPurchasingmodule.Checked = true;
    }
    protected void chkRPOView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRPOView.Checked == false)
            chkRPOAdd.Checked = chkRPOEdit.Checked = chkRPODelete.Checked = chkRPOView.Checked;
        if (chkPurchasingmodule.Checked == false && chkRPOView.Checked == true)
            chkPurchasingmodule.Checked = true;
    }
    protected void chkCustomerModule_CheckedChanged(object sender, EventArgs e)
    {
        chkCustomeradd.Checked = chkCustomeredit.Checked = chkCustomerdelete.Checked = chkCustomerview.Checked =
        chkLocationadd.Checked = chkLocationedit.Checked = chkLocationdelete.Checked = chkLocationview.Checked =
        chkReceivePaymentAdd.Checked = chkReceivePaymentEdit.Checked = chkReceivePaymentDelete.Checked = chkReceivePaymentView.Checked =
         chkMakeDepositAdd.Checked = chkMakeDepositEdit.Checked = chkMakeDepositDelete.Checked = chkMakeDepositView.Checked =
         chkCollectionsAdd.Checked = chkCollectionsEdit.Checked = chkCollectionsDelete.Checked = chkCollectionsView.Checked =
        chkEquipmentsadd.Checked = chkEquipmentsedit.Checked = chkEquipmentsdelete.Checked = chkEquipmentsview.Checked =
        //chkCreditHold.Checked = chkWriteOff.Checked = chkCustomerModule.Checked;
        chkCreditHold.Checked = chkCreditFlag.Checked = chkWriteOff.Checked = chkCustomerModule.Checked;
    }
    protected void chkCustomerview_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCustomerview.Checked == false)
            chkCustomeradd.Checked = chkCustomeredit.Checked = chkCustomerdelete.Checked = chkCustomerview.Checked;
        if (chkCustomerModule.Checked == false && chkCustomerview.Checked == true)
            chkCustomerModule.Checked = true;
    }
    protected void chkLocationview_CheckedChanged(object sender, EventArgs e)
    {
        if (chkLocationview.Checked == false)
            chkLocationadd.Checked = chkLocationedit.Checked = chkLocationdelete.Checked = chkLocationview.Checked;
        if (chkCustomerModule.Checked == false && chkLocationview.Checked == true)
            chkCustomerModule.Checked = true;
    }
    protected void chkEquipmentsview_CheckedChanged(object sender, EventArgs e)
    {
        if (chkEquipmentsview.Checked == false)
            chkEquipmentsadd.Checked = chkEquipmentsedit.Checked = chkEquipmentsdelete.Checked = chkEquipmentsview.Checked;
        if (chkCustomerModule.Checked == false && chkEquipmentsview.Checked == true)
            chkCustomerModule.Checked = true;
    }
    protected void chkCreditHold_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCustomerModule.Checked == false && chkCreditHold.Checked == true)
            chkCustomerModule.Checked = true;
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
    }
    protected void chkMakeDepositView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkMakeDepositView.Checked == false)
            chkMakeDepositAdd.Checked = chkMakeDepositEdit.Checked = chkMakeDepositDelete.Checked = chkMakeDepositView.Checked;
        if (chkCustomerModule.Checked == false && chkMakeDepositView.Checked == true)
            chkCustomerModule.Checked = true;
    }

    protected void chkCollectionsView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCollectionsView.Checked == false)
            chkCollectionsAdd.Checked = chkCollectionsEdit.Checked = chkCollectionsDelete.Checked = chkCollectionsView.Checked;
        if (chkCustomerModule.Checked == false && chkCollectionsView.Checked == true)
            chkCustomerModule.Checked = true;
    }
    protected void chkVendorsView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkVendorsView.Checked == false)
            chkVendorsAdd.Checked = chkVendorsEdit.Checked = chkVendorsDelete.Checked = chkVendorsView.Checked;
        if (chkAccountPayable.Checked == false && chkVendorsView.Checked == true)
            chkAccountPayable.Checked = true;

    }
    protected void chkViewBills_CheckedChanged(object sender, EventArgs e)
    {
        if (chkViewBills.Checked == false)
            chkAddBills.Checked = chkEditBills.Checked = chkDeleteBills.Checked = chkViewBills.Checked = chkViewBills.Checked;
        if (chkAccountPayable.Checked == false && chkViewBills.Checked == true)
            chkAccountPayable.Checked = true;
    }
    protected void chkViewManageChecks_CheckedChanged(object sender, EventArgs e)
    {
        if (chkViewManageChecks.Checked == false)
            chkAddManageChecks.Checked = chkEditManageChecks.Checked = chkDeleteManageChecks.Checked = 
                chkShowBankBalances.Checked = chkViewManageChecks.Checked;
        if (chkAccountPayable.Checked == false && chkViewManageChecks.Checked == true)
            chkAccountPayable.Checked = true;
    }
    protected void chkBillingmodule_CheckedChanged(object sender, EventArgs e)
    {
        chkInvoicesAdd.Checked = chkInvoicesEdit.Checked = chkInvoicesDelete.Checked = chkInvoicesView.Checked =
        chkPaymentHistoryView.Checked =
         chkBillingcodesAdd.Checked = chkBillingcodesEdit.Checked = chkBillingcodesDelete.Checked = chkBillingcodesView.Checked = chkBillingmodule.Checked;
    }
    protected void chkInvoicesView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkInvoicesView.Checked == false)
            chkInvoicesAdd.Checked = chkInvoicesEdit.Checked = chkInvoicesDelete.Checked = chkInvoicesView.Checked = chkInvoicesView.Checked;
        if (chkBillingmodule.Checked == false && chkInvoicesView.Checked == true)
            chkBillingmodule.Checked = true;
    }
    protected void chkPaymentHistoryView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkPaymentHistoryView.Checked == false)
            chkPaymentHistoryAdd.Checked = chkPaymentHistoryEdit.Checked = chkPaymentHistoryDelete.Checked = chkPaymentHistoryView.Checked = chkPaymentHistoryView.Checked;
        if (chkBillingmodule.Checked == false && chkPaymentHistoryView.Checked == true)
            chkBillingmodule.Checked = true;
    }
    protected void chkBillingcodesView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkBillingcodesView.Checked == false)
            chkBillingcodesAdd.Checked = chkBillingcodesEdit.Checked = chkBillingcodesDelete.Checked = chkBillingcodesView.Checked = chkBillingcodesView.Checked;
        if (chkBillingmodule.Checked == false && chkBillingcodesView.Checked == true)
            chkBillingmodule.Checked = true;
    }

    //payroll check/uncheck
    protected void chkempView_CheckedChanged(object sender, EventArgs e)
    {
        if (empView.Checked == false)
            empAdd.Checked = empEdit.Checked = empDelete.Checked = empView.Checked = empView.Checked;
        if (payrollModulchck.Checked == false && empView.Checked == true)
            payrollModulchck.Checked = true;
    }
    protected void chkrunpayrollView_CheckedChanged(object sender, EventArgs e)
    {
        if (runView.Checked == false)
            runAdd.Checked = runEdit.Checked = runDelete.Checked = runView.Checked = runView.Checked;
        if (payrollModulchck.Checked == false && runView.Checked == true)
            payrollModulchck.Checked = true;
    }
    protected void chkpayrollcheckView_CheckedChanged(object sender, EventArgs e)
    {
        if (payrollchckView.Checked == false)
            payrollchckAdd.Checked = payrollchckEdit.Checked = payrollchckDelete.Checked = payrollchckView.Checked = payrollchckView.Checked;
        if (payrollModulchck.Checked == false && payrollchckView.Checked == true)
            payrollModulchck.Checked = true;
    }
    protected void chkpayrollformView_CheckedChanged(object sender, EventArgs e)
    {
        if (payrollformView.Checked == false)
            payrollformAdd.Checked = payrollformEdit.Checked = payrollformDelete.Checked = payrollformView.Checked = payrollformView.Checked;
        if (payrollModulchck.Checked == false && payrollformView.Checked == true)
            payrollModulchck.Checked = true;
    }
    protected void chkwagesView_CheckedChanged(object sender, EventArgs e)
    {
        if (wagesView.Checked == false)
            wagesadd.Checked = wagesEdit.Checked = wagesDelete.Checked = wagesView.Checked = wagesView.Checked;
        if (payrollModulchck.Checked == false && wagesView.Checked == true)
            payrollModulchck.Checked = true;
    }
    protected void chkdeductionView_CheckedChanged(object sender, EventArgs e)
    {
        if (deductionsView.Checked == false)
            deductionsAdd.Checked = deductionsEdit.Checked = deductionsDelete.Checked = deductionsView.Checked = deductionsView.Checked;
        if (payrollModulchck.Checked == false && deductionsView.Checked == true)
            payrollModulchck.Checked = true;
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
    }
    protected void chkRecContractsView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRecContractsView.Checked == false)
            chkRecContractsAdd.Checked = chkRecContractsEdit.Checked = chkRecContractsDelete.Checked = chkRecContractsView.Checked = chkRecContractsView.Checked;
        if (chkRecurring.Checked == false && chkRecContractsView.Checked == true)
            chkRecurring.Checked = true;
    }
    protected void chkRecInvoicesView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRecInvoicesView.Checked == false)
            chkRecInvoicesAdd.Checked = chkRecInvoicesDelete.Checked = chkRecInvoicesView.Checked = chkRecInvoicesView.Checked;
        if (chkRecurring.Checked == false && chkRecInvoicesView.Checked == true)
            chkRecurring.Checked = true;
    }
    protected void chkRecTicketsView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRecTicketsView.Checked == false)
            chkRecTicketsAdd.Checked = chkRecTicketsDelete.Checked = chkRecTicketsView.Checked = chkRecTicketsView.Checked;
        if (chkRecurring.Checked == false && chkRecTicketsView.Checked == true)
            chkRecurring.Checked = true;
    }
    protected void chkSafetyTestsView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSafetyTestsView.Checked == false)
            chkSafetyTestsAdd.Checked = chkSafetyTestsEdit.Checked = chkSafetyTestsDelete.Checked = chkSafetyTestsView.Checked = chkSafetyTestsView.Checked;
        if (chkRecurring.Checked == false && chkSafetyTestsView.Checked == true)
            chkRecurring.Checked = true;
    }
    protected void chkRenewEscalateView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRenewEscalateView.Checked == false)
            chkRenewEscalateAdd.Checked = chkRenewEscalateView.Checked = chkRenewEscalateView.Checked;
        if (chkRecurring.Checked == false && chkRenewEscalateView.Checked == true)
            chkRecurring.Checked = true;
    }

    protected void chkSchedule_CheckedChanged(object sender, EventArgs e)
    {
      chkScheduleBoard.Checked =
        chkTimesheetadd.Checked = chkTimesheetedit.Checked = chkTimesheetdelete.Checked = chkTimesheetview.Checked = chkTimesheetview.Checked = chkETimesheetreport.Checked =
        chkMapAdd.Checked = chkMapEdit.Checked = chkMapDelete.Checked = chkMapView.Checked = chkMapView.Checked = chkMapReport.Checked =
           chkRouteBuilderAdd.Checked = chkRouteBuilderEdit.Checked = chkRouteBuilderDelete.Checked = chkRouteBuilderView.Checked = chkRouteBuilderView.Checked = chkRouteBuilderReport.Checked =
           chkMassReview.Checked = chkMassTimesheetCheck.Checked = chkETimesheetview.Checked = chkTimestampFix.Checked =   chkResolveTicketView.Checked =
           chkResolveTicketAdd.Checked = chkResolveTicketEdit.Checked = chkResolveTicketDelete.Checked = chkResolveTicketReport.Checked = chkResolveTicketView.Checked =
          chkTicketListAdd.Checked = chkTicketListEdit.Checked = chkTicketListDelete.Checked = chkTicketListView.Checked = chkTicketListReport.Checked = chkTicketListView.Checked = chkSchedule.Checked;
    }
    
    protected void chkTimesheetview_CheckedChanged(object sender, EventArgs e)
    {
        if (chkTimesheetview.Checked == false)
            chkTimesheetadd.Checked = chkTimesheetedit.Checked = chkTimesheetdelete.Checked = chkTimesheetreport.Checked = chkTimesheetview.Checked = chkTimesheetview.Checked;
        if (chkSchedule.Checked == false && chkTimesheetview.Checked == true)
            chkSchedule.Checked = true;
    }
    protected void chkETimesheetview_CheckedChanged(object sender, EventArgs e)
    {
        if (chkETimesheetview.Checked == false)
            chkETimesheetadd.Checked = chkETimesheetedit.Checked = chkETimesheetdelete.Checked = chkETimesheetview.Checked = chkETimesheetreport.Checked = chkETimesheetview.Checked;
        if (chkSchedule.Checked == false && chkETimesheetview.Checked == true)
            chkSchedule.Checked = true;
    }

    protected void chkMapView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkMapView.Checked == false)
            chkMapAdd.Checked = chkMapEdit.Checked = chkMapDelete.Checked = chkMapView.Checked = chkMapReport.Checked = chkMapView.Checked;
        if (chkSchedule.Checked == false && chkMapView.Checked == true)
            chkSchedule.Checked = true;
    }
    protected void chkRouteBuilderView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRouteBuilderView.Checked == false)
            chkRouteBuilderAdd.Checked = chkRouteBuilderEdit.Checked = chkRouteBuilderDelete.Checked = chkRouteBuilderView.Checked = chkRouteBuilderView.Checked;
        if (chkSchedule.Checked == false && chkRouteBuilderView.Checked == true)
            chkSchedule.Checked = true;
    }
    protected void chkMassReview_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSchedule.Checked == false && chkMassReview.Checked == true)
            chkSchedule.Checked = true;
    }
    protected void chkMassTimesheetCheck_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSchedule.Checked == false && chkMassTimesheetCheck.Checked == true)
            chkSchedule.Checked = true;
    }
    protected void chkTimestampFix_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSchedule.Checked == false && chkTimestampFix.Checked == true)
            chkSchedule.Checked = true;
    }
    protected void chkTicketListView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkTicketListView.Checked == false)
            chkTicketListAdd.Checked = chkTicketListEdit.Checked = chkTicketListDelete.Checked = chkTicketListReport.Checked = chkTicketListView.Checked = chkResolveTicketReport.Checked = chkTicketListView.Checked;
        if (chkSchedule.Checked == false && chkTicketListView.Checked == true)
            chkSchedule.Checked = true;
          chkScheduleBrd.Checked = chkTicketListView.Checked;
    }
   
    protected void chkResolveTicketView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkResolveTicketView.Checked == false)
            chkResolveTicketAdd.Checked = chkResolveTicketEdit.Checked = chkResolveTicketDelete.Checked = chkResolveTicketView.Checked = chkResolveTicketView.Checked;
        if (chkSchedule.Checked == false && chkResolveTicketView.Checked == true)
            chkSchedule.Checked = true;
    }
    protected void chkSalesMgr_CheckedChanged(object sender, EventArgs e)
    {
        chkLeadAdd.Checked = chkLeadEdit.Checked = chkLeadDelete.Checked = chkLeadReport.Checked = chkLeadView.Checked =
        chkTasks.Checked = chkCompleteTask.Checked = chkFollowUp.Checked =
        chkOppAdd.Checked = chkOppEdit.Checked = chkOppDelete.Checked = chkOppView.Checked = chkOppReport.Checked =
           chkEstimateAdd.Checked = chkEstimateEdit.Checked = chkEstimateDelete.Checked = chkEstimateView.Checked = chkEstimateReport.Checked =
           chkConvertEstimate.Checked = chkSalesSetup.Checked = chkSalesMgr.Checked;
    }

    protected void chkPayrollMgr_CheckedChanged(object sender, EventArgs e)
    {
        empAdd.Checked = empEdit.Checked = empDelete.Checked = empView.Checked = 
            runAdd.Checked = runEdit.Checked = runDelete.Checked = runView.Checked = 
        payrollchckAdd.Checked = payrollchckEdit.Checked = payrollchckDelete.Checked = payrollchckView.Checked = 
        payrollformAdd.Checked = payrollformEdit.Checked = payrollformDelete.Checked = payrollformView.Checked = 
        wagesadd.Checked = wagesEdit.Checked = wagesDelete.Checked = wagesView.Checked =  deductionsAdd.Checked = deductionsEdit.Checked
        = deductionsDelete.Checked = deductionsView.Checked ;

    }

    protected void chkLeadView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSalesMgr.Checked == false && chkLeadView.Checked == true)
            chkSalesMgr.Checked = true;
    }
    protected void chkTasks_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSalesMgr.Checked == false && chkTasks.Checked == true)
            chkSalesMgr.Checked = true;
    }
    protected void chkCompleteTask_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSalesMgr.Checked == false && chkCompleteTask.Checked == true)
            chkSalesMgr.Checked = true;
    }
    protected void chkOppView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSalesMgr.Checked == false && chkOppView.Checked == true)
            chkSalesMgr.Checked = true;
    }
    protected void chkEstimateView_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSalesMgr.Checked == false && chkEstimateView.Checked == true)
            chkSalesMgr.Checked = true;
    }
    protected void chkSalesSetup_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSalesMgr.Checked == false && chkSalesSetup.Checked == true)
            chkSalesMgr.Checked = true;
    }
    protected void chkFollowUp_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSalesMgr.Checked == false && chkFollowUp.Checked == true)
            chkSalesMgr.Checked = true;
    }
    protected void chkConvertEstimate_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSalesMgr.Checked == false && chkConvertEstimate.Checked == true)
            chkSalesMgr.Checked = true;
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
            ////gvWagePayRate.DataBind();
            gvWagePayRate.Rebind();

            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "RemoveWageGridRow", "RemoveWageGridRow('" + gvWagePayRate.ClientID + "')", true);
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
            objProp_Customer.LogScreen = "Emp";
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
    }

    protected void chkProjectmodule_CheckedChanged(object sender, EventArgs e)
    {
        chkProjectadd.Checked = chkProjectDelete.Checked = chkProjectEdit.Checked = chkProjectView.Checked =
            chkProjectTempAdd.Checked = chkProjectTempDelete.Checked = chkProjectTempEdit.Checked = chkProjectTempView.Checked =
            chkAddBOM.Checked = chkDeleteBOM.Checked = chkEditBOM.Checked = chkViewBOM.Checked =
            chkAddMilesStones.Checked = chkDeleteMilesStones.Checked = chkEditMilesStones.Checked = chkViewMilesStones.Checked =
            chkJobClosePermission.Checked = chkJobCompletedPermission.Checked = chkJobReopenPermission.Checked =
            chkViewProjectList.Checked = chkViewFinance.Checked = chkProjectmodule.Checked;
    }

    protected void chkProgram_CheckedChanged(object sender, EventArgs e)
    {
        chkEmpMainten.Checked = chkExpenses.Checked = chkAccessUser.Checked = chkDispatch.Checked = chkProgram.Checked;
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
                    }else if(ddlApplyUserRolePermission.SelectedValue == "1")
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

            //if (roleInfo.Rows[0]["dboard"] != DBNull.Value)
            //{
            //    if (roleInfo.Rows[0]["dboard"].ToString().Trim() != string.Empty)
            //    {
            //        int schTS = Convert.ToInt32(roleInfo.Rows[0]["dboard"]);
            //        if (schTS == 1)
            //        {
            //            chkScheduleBrd.Checked = true;
            //        }
            //        else
            //        {
            //            chkScheduleBrd.Checked = false;
            //        }
            //    }
            //}
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
        //if (ddlPOApprove.SelectedValue == "0")
        //{
        //    //divMinAmount.Attributes.Remove("visibility");
        //    //divMaxAmount.Attributes.Remove("visibility");
        //    //divApprovePo.Attributes.Remove("visibility");
        //    //divApprovePo.Visible = false;
        //    divApprovePo.Style["display"] = "none";
        //    divMinAmount.Style["display"] = "none";
        //    divMaxAmount.Style["display"] = "none";

        //}
        //else
        //{
        //    divApprovePo.Style["display"] = "block";

        //    //divMinAmount.Style.Add("visibility", "visible");
        //    //divMaxAmount.Style.Add("visibility", "visible");
        //    //divApprovePo.Style.Add("visibility", "visible");
        //    //divApprovePo.Visible = true;

        //    if (ddlPOApproveAmt.SelectedValue == "0")
        //    {
        //        divMinAmount.Style["display"] = "block";
        //        divMaxAmount.Style["display"] = "block";
        //        //divMinAmount.Style.Add("visibility", "visible");
        //        //divMaxAmount.Style.Add("visibility", "visible");
        //        //divMinAmount.Visible = true;
        //        //divMaxAmount.Visible = true;
        //    }
        //    else if (ddlPOApproveAmt.SelectedValue == "1")
        //    {
        //        divMinAmount.Style["display"] = "block";
        //        divMaxAmount.Style["display"] = "none";
        //        //divMaxAmount.Attributes.Remove("visibility");
        //        //divMaxAmount.Visible = false;
        //    }
        //    else
        //    {
        //        divMinAmount.Style["display"] = "none";
        //        divMaxAmount.Style["display"] = "none";
        //        //divMinAmount.Attributes.Remove("visibility");
        //        //divMaxAmount.Attributes.Remove("visibility");
        //        //divMinAmount.Visible = false;
        //        //divMaxAmount.Visible = false;
        //    }
        //}


        chkMassReview.Checked = Convert.ToBoolean(roleInfo.Rows[0]["massreview"]);
        //chkProjectManager.Checked = Convert.ToBoolean(roleInfo.Rows[0]["IsProjectManager"]);
        //chkAssignedProject.Checked = Convert.ToBoolean(roleInfo.Rows[0]["IsAssignedProject"]);
        // TODO: Thomas need to check for ES-33

        //if (ds.Tables[2] != null)
        //{

        //    foreach (DataRow dr in ds.Tables[2].Rows)
        //    {
        //        foreach (RadComboBoxItem li in ddlDepartment.Items)
        //        {
        //            if (dr["department"].ToString().Equals(li.Value))
        //                li.Checked = true;
        //        }
        //    }
        //}

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

        string _apVendor = roleInfo.Rows[0]["Vendor"].ToString(); //check Account payable permission
        string _apBill = roleInfo.Rows[0]["Bill"].ToString();
        string _apBillPay = roleInfo.Rows[0]["BillPay"].ToString();



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
        string BillPayPermission = roleInfo.Rows[0]["BillPay"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["BillPay"].ToString();

        string AddBillPayPermission = BillPayPermission.Length < 1 ? "Y" : BillPayPermission.Substring(0, 1);
        string EditBillPayPermission = BillPayPermission.Length < 2 ? "Y" : BillPayPermission.Substring(1, 1);
        string DeleteBillPayPermission = BillPayPermission.Length < 3 ? "Y" : BillPayPermission.Substring(2, 1);
        string ViewBillPayPermission = BillPayPermission.Length < 4 ? "Y" : BillPayPermission.Substring(3, 1);

        chkAddManageChecks.Checked = (AddBillPayPermission == "N") ? false : true;
        chkEditManageChecks.Checked = (EditBillPayPermission == "N") ? false : true;
        chkDeleteManageChecks.Checked = (DeleteBillPayPermission == "N") ? false : true;
        chkViewManageChecks.Checked = (ViewBillPayPermission == "N") ? false : true;

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
        string CollectionsPermission = roleInfo.Rows[0]["Collection"] == DBNull.Value ? "NNNN" : roleInfo.Rows[0]["Collection"].ToString();

        string AddCollectionsPermission = CollectionsPermission.Length < 1 ? "Y" : CollectionsPermission.Substring(0, 1);
        string EditCollectionsPermission = CollectionsPermission.Length < 2 ? "Y" : CollectionsPermission.Substring(1, 1);
        string DeleteCollectionsPermission = CollectionsPermission.Length < 3 ? "Y" : CollectionsPermission.Substring(2, 1);
        string ViewCollectionsPermission = CollectionsPermission.Length < 4 ? "Y" : CollectionsPermission.Substring(3, 1);

        chkCollectionsAdd.Checked = (AddCollectionsPermission == "N") ? false : true;
        chkCollectionsEdit.Checked = (EditCollectionsPermission == "N") ? false : true;
        chkCollectionsDelete.Checked = (DeleteDepositPermission == "N") ? false : true;
        chkCollectionsView.Checked = (ViewDepositPermission == "N") ? false : true;

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
                else if(ddlApplyUserRolePermission.SelectedValue == "1")
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
                    if (dsUser!=null && dsUser.Tables.Count > 0 && ds.Tables.Count > 0)
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
                    if(uPOLimit == 0)
                    {
                        //var urPOLimit = string.IsNullOrEmpty(role["POLimit"].ToString()) ? 0 : Convert.ToDecimal(role["POLimit"].ToString());
                        userPer["POLimit"] = role["POLimit"];
                    }
                    var uPOApprove = Convert.ToBoolean(userPer["POApprove"]);
                    var urPOApprove = Convert.ToBoolean(role["POApprove"]);
                    if(uPOApprove == false && urPOApprove)
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

        //if (string1.Length == string2.Length)
        //{
        //    var arr1 = string1.ToUpper().ToArray();
        //    var arr2 = string2.ToUpper().ToArray();
        //    var len = arr1.Length;
        //    for (int i = 0; i < len; i++)
        //    {
        //        if(arr1[i] != arr2[i] && arr2[i] == 'Y')
        //        {
        //            builder.Append('Y');
        //        }
        //        else
        //        {
        //            builder.Append(arr1[i]);
        //        }
        //    }

        //    return builder.ToString();
        //}
        //else
        //{
        //    throw new Exception("Permission merging error!");
        //}
    }

    private int MergePermissionInt(int int1, int int2)
    {
        if (int1 == 0) return int2;
        else return int1;
    }

    protected void chkMassPayrollTicket1_CheckedChanged(object sender, EventArgs e)
    {
        chkMassPayrollTicket.Checked = chkMassPayrollTicket1.Checked;
    }

    protected void chkMassPayrollTicket_CheckedChanged(object sender, EventArgs e)
    {
        chkMassPayrollTicket1.Checked = chkMassPayrollTicket.Checked;
    }
    //*********************************************************************************************************
    public DataTable GetOtherIncomeWageTable()
    {
        string PayrollTaxAcctName = "";
        int PayrollTaxAcct = 0;
        DataSet ds = new DataSet();
        _objWage.ConnConfig = Session["config"].ToString();

        ds = new BL_Wage().getPayrollTaxAccount(_objWage);
        if (ds.Tables[0].Rows.Count > 0)
        {
            PayrollTaxAcctName = ds.Tables[0].Rows[0]["fDesc"].ToString();
            PayrollTaxAcct = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"].ToString());
        }

        DataTable dt = new DataTable();
        dt.Columns.Add("Cat", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Rate", typeof(double));
        dt.Columns.Add("ExpAcctName", typeof(string));
        dt.Columns.Add("GL", typeof(int));
        dt.Columns.Add("FIT", typeof(int));
        dt.Columns.Add("FICA", typeof(int));
        dt.Columns.Add("MEDI", typeof(int));
        dt.Columns.Add("FUTA", typeof(int));
        dt.Columns.Add("SIT", typeof(int));
        dt.Columns.Add("Vac", typeof(int));
        dt.Columns.Add("Wc", typeof(int));
        dt.Columns.Add("Uni", typeof(int));
        dt.Columns.Add("Sick", typeof(int));

        for (int i = 0; i < 7; i++)
        {
            DataRow dr = dt.NewRow();
            dr["Cat"] = i;
            if (i == 0)
            {
                dr["fDesc"] = "Bonus";
                dr["Rate"] = 0.00;
                //dr["ExpAcctName"] = PayrollTaxAcctName;
                //dr["GL"] = PayrollTaxAcct;
                dr["ExpAcctName"] = "";
                dr["GL"] = 0;
                dr["FIT"] = 1;
                dr["FICA"] = 1;
                dr["MEDI"] = 1;
                dr["FUTA"] = 1;
                dr["SIT"] = 1;
                dr["Vac"] = 0;
                dr["Wc"] = 1;
                dr["Uni"] = 1;
                dr["Sick"] = 1;
            }
            else if (i == 1)
            {
                dr["fDesc"] = "Holiday";
                dr["Rate"] = 0.00;
                //dr["ExpAcctName"] = PayrollTaxAcctName;
                //dr["GL"] = PayrollTaxAcct;
                dr["ExpAcctName"] = "";
                dr["GL"] = 0;
                dr["FIT"] = 1;
                dr["FICA"] = 1;
                dr["MEDI"] = 1;
                dr["FUTA"] = 1;
                dr["SIT"] = 1;
                dr["Vac"] = 0;
                dr["Wc"] = 1;
                dr["Uni"] = 1;
                dr["Sick"] = 1;
            }
            else if (i == 2)
            {
                dr["fDesc"] = "Vacation";
                dr["Rate"] = 0.00;
                //dr["ExpAcctName"] = PayrollTaxAcctName;
                //dr["GL"] = PayrollTaxAcct;
                dr["ExpAcctName"] = "";
                dr["GL"] = 0;
                dr["FIT"] = 1;
                dr["FICA"] = 1;
                dr["MEDI"] = 1;
                dr["FUTA"] = 1;
                dr["SIT"] = 1;
                dr["Vac"] = 0;
                dr["Wc"] = 1;
                dr["Uni"] = 0;
                dr["Sick"] = 1;
            }
            else if (i == 3)
            {
                dr["fDesc"] = "Zone";
                dr["Rate"] = 0.00;
                //dr["ExpAcctName"] = PayrollTaxAcctName;
                //dr["GL"] = PayrollTaxAcct;
                dr["ExpAcctName"] = "";
                dr["GL"] = 0;
                dr["FIT"] = 1;
                dr["FICA"] = 1;
                dr["MEDI"] = 1;
                dr["FUTA"] = 1;
                dr["SIT"] = 1;
                dr["Vac"] = 0;
                dr["Wc"] = 1;
                dr["Uni"] = 1;
                dr["Sick"] = 1;
            }
            else if (i == 4)
            {
                dr["fDesc"] = "Reimbursement";
                dr["Rate"] = 0.00;
                //dr["ExpAcctName"] = PayrollTaxAcctName;
                //dr["GL"] = PayrollTaxAcct;
                dr["ExpAcctName"] = "";
                dr["GL"] = 0;
                dr["FIT"] = 0;
                dr["FICA"] = 0;
                dr["MEDI"] = 0;
                dr["FUTA"] = 0;
                dr["SIT"] = 0;
                dr["Vac"] = 0;
                dr["Wc"] = 0;
                dr["Uni"] = 0;
                dr["Sick"] = 0;
            }
            else if (i == 5)
            {
                dr["fDesc"] = "Mileage";
                dr["Rate"] = 0.00;
                //dr["ExpAcctName"] = PayrollTaxAcctName;
                //dr["GL"] = PayrollTaxAcct;
                dr["ExpAcctName"] = "";
                dr["GL"] = 0;
                dr["FIT"] = 0;
                dr["FICA"] = 0;
                dr["MEDI"] = 0;
                dr["FUTA"] = 0;
                dr["SIT"] = 0;
                dr["Vac"] = 0;
                dr["Wc"] = 0;
                dr["Uni"] = 0;
                dr["Sick"] = 0;
            }
            else if (i == 6)
            {
                dr["fDesc"] = "Sick Leave";
                dr["Rate"] = 0.00;
                //dr["ExpAcctName"] = PayrollTaxAcctName;
                //dr["GL"] = PayrollTaxAcct;
                dr["ExpAcctName"] = "";
                dr["GL"] = 0;
                dr["FIT"] = 0;
                dr["FICA"] = 0;
                dr["MEDI"] = 0;
                dr["FUTA"] = 0;
                dr["SIT"] = 0;
                dr["Vac"] = 0;
                dr["Wc"] = 0;
                dr["Uni"] = 0;
                dr["Sick"] = 0;
            }

            dt.Rows.Add(dr);
            dt.AcceptChanges();
        }

        return dt;
    }
    private void FillOtherIncomeWage()
    {
        try
        {

            DataTable dt = GetOtherIncomeWageTable();
            RadGrid_WageCategoryOtherIncome.DataSource = dt;
            //RadGrid_WageCategoryOtherIncome.DataBind();
            //ddlDefaultWage.Items.Insert(0, new ListItem(":: Select ::", ""));
        }
        catch (Exception ex)
        {
            throw ex;
            //FillOtherIncomeWage
        }
    }
    private void RowSelectWageCategoryOtherIncome()
    {

    }
    protected void RadGrid_WageCategoryOtherIncome_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        //FillOtherIncomeWage();

        

        if (Request.QueryString["uid"] != null)
        {
            DataSet ds = new DataSet();
            ds = (DataSet)ViewState["getUserById"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["uid"].ToString()))
                {
                    
                    DataSet dsWage = new DataSet();
                    _objEmp.ConnConfig = Session["config"].ToString();
                    _objEmp.ID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                    dsWage = objBL_Wage.GetEmployeeListByID(_objEmp);
                    //ViewState["dsWage"] = dsWage;

                    if (dsWage.Tables[3].Rows.Count > 0)
                    {
                        RadGrid_WageCategoryOtherIncome.DataSource = dsWage.Tables[3];
                        //gvWagePayRate.DataBind();
                        ViewState["OtherWageItems"] = dsWage.Tables[3];
                    }
                    else
                    {
                        FillOtherIncomeWage();
                    }
                }
            }
        }
        else
        {
            FillOtherIncomeWage();
        }
    }
    protected void RadGrid_WageCategoryOtherIncome_PreRender(object sender, EventArgs e)
    {
        RowSelectWageCategoryOtherIncome();
    }
    protected void RadGrid_WageCategoryOtherIncome_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        //if (e.Item is GridDataItem)
        //{
        //    GridDataItem item = e.Item as GridDataItem;
        //    CheckBox checkColumn = item.FindControl("checkColumnWageCategoryOtherIncome") as CheckBox;
        //    checkColumn.Attributes.Add("onclick", "uncheckWageCategoryOtherIncome(this);");
        //}
    }
    protected void RadGridWageDeduction_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex = RadGridWageDeduction.Items.Count - 1;
        GridDataItem row = RadGridWageDeduction.Items[rowIndex];
        //HiddenField hdnIndex = row.FindControl("hdnIndex") as HiddenField;

        DataTable dt = new DataTable();

        dt = GetWageDeductionGridItems();

        DataRow dr = dt.NewRow();
        dr["Ded"] = 0;
        dr["BasedOn"] = 0;
        dr["AccruedOn"] = 0;
        dr["ByW"] = 0;
        dr["EmpRate"] = 0.00;
        dr["EmpTop"] = 0.00;
        dr["EmpGL"] = 0;
        dr["EmpGLName"] = "";
        dr["CompRate"] = 0.00;
        dr["CompTop"] = 0.00;
        dr["CompGL"] = 0;
        dr["CompGLE"] = 0;
        dr["CompGLName"] = "";
        dr["CompGLEName"] = "";
        dr["InUse"] = 0;
        dr["YTD"] = 0.00;
        dr["YTDC"] = 0.00;
        dr["fdesc"] = "";
        dr["Checked"] = false;
        dt.Rows.Add(dr);

        RadGridWageDeduction.DataSource = dt;
        //gvWagePayRate.DataBind();

        ViewState["WageDeductionItems"] = dt;
    }
    private DataTable GetWageDeductionGridItems()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Ded", typeof(int));
        dt.Columns.Add("BasedOn", typeof(int));
        dt.Columns.Add("AccruedOn", typeof(int));
        dt.Columns.Add("ByW", typeof(int));
        dt.Columns.Add("EmpRate", typeof(double));
        dt.Columns.Add("EmpTop", typeof(double));
        dt.Columns.Add("EmpGL", typeof(int));
        dt.Columns.Add("EmpGLName", typeof(string));
        dt.Columns.Add("CompRate", typeof(double));
        dt.Columns.Add("CompTop", typeof(double));
        dt.Columns.Add("CompGL", typeof(int));
        dt.Columns.Add("CompGLName", typeof(string));
        dt.Columns.Add("CompGLE", typeof(int));
        dt.Columns.Add("CompGLEName", typeof(string));
        dt.Columns.Add("InUse", typeof(int));
        dt.Columns.Add("YTD", typeof(double));
        dt.Columns.Add("YTDC", typeof(double));
        dt.Columns.Add("fdesc", typeof(string));
        dt.Columns.Add("Checked", typeof(bool));

        try
        {
            string strItems = hdnWageDRate.Value.Trim();

            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objWageDedcutionItemData = new List<Dictionary<object, object>>();
                objWageDedcutionItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                objWageDedcutionItemData.RemoveAt(0);
                //objWageDedcutionItemData.RemoveAt(0);
                foreach (Dictionary<object, object> dict in objWageDedcutionItemData)
                {
                    i++;
                    DataRow dr = dt.NewRow();
                    if (dict["ddlWageD"].ToString() != "0")
                    {
                        dr["Ded"] = Convert.ToInt32(dict["ddlWageD"]);
                    }
                    else
                    {
                        dr["Ded"] = 0;
                    }
                    if (dict["ddlBasedOnD"].ToString() != "0")
                    {
                        dr["BasedOn"] = Convert.ToInt32(dict["ddlBasedOnD"]);
                    }
                    else
                    {
                        dr["BasedOn"] = 0;
                    }
                    if (dict["ddlAccuredOnD"].ToString() != "0")
                    {
                        dr["AccruedOn"] = Convert.ToInt32(dict["ddlAccuredOnD"]);
                    }
                    else
                    {
                        dr["AccruedOn"] = 0;
                    }


                    if (!string.IsNullOrEmpty(dict["txtEmpRateD"].ToString()))
                    {
                        dr["EmpRate"] = Convert.ToDouble(dict["txtEmpRateD"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtEmpMaxRateD"].ToString()))
                    {
                        dr["EmpTop"] = Convert.ToDouble(dict["txtEmpMaxRateD"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtCompanyRateD"].ToString()))
                    {
                        dr["CompRate"] = Convert.ToDouble(dict["txtCompanyRateD"]);
                    }
                    if (!string.IsNullOrEmpty(dict["txtCompanyMaxRateD"].ToString()))
                    {
                        dr["CompTop"] = Convert.ToDouble(dict["txtCompanyMaxRateD"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdndByW"].ToString()))
                    {
                        dr["ByW"] = Convert.ToInt32(dict["hdndByW"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdndEmpGL"].ToString()))
                    {
                        dr["EmpGL"] = Convert.ToInt32(dict["hdndEmpGL"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdndEmpGLName"].ToString()))
                    {
                        dr["EmpGLName"] = Convert.ToString(dict["hdndEmpGLName"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdndCompGL"].ToString()))
                    {
                        dr["CompGL"] = Convert.ToInt32(dict["hdndCompGL"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdndCompGLName"].ToString()))
                    {
                        dr["CompGLName"] = Convert.ToString(dict["hdndCompGLName"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdndCompGLE"].ToString()))
                    {
                        dr["CompGLE"] = Convert.ToInt32(dict["hdndCompGLE"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdndCompGLEName"].ToString()))
                    {
                        dr["CompGLEName"] = Convert.ToString(dict["hdndCompGLEName"]);
                    }

                    if (!string.IsNullOrEmpty(dict["hdndInUse"].ToString()))
                    {
                        dr["InUse"] = Convert.ToInt32(dict["hdndInUse"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdndYTD"].ToString()))
                    {
                        dr["YTD"] = Convert.ToDouble(dict["hdndYTD"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdndYTDC"].ToString()))
                    {
                        dr["YTDC"] = Convert.ToDouble(dict["hdndYTDC"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdndfdesc"].ToString()))
                    {
                        dr["fdesc"] = Convert.ToString(dict["hdndfdesc"]);
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(dict["chkSelect"].ToString()) && dict["chkSelect"].ToString() == "on")
                        {
                            dr["Checked"] = true;
                        }
                        else
                        {
                            dr["Checked"] = false;
                        }
                    }
                    catch (Exception)
                    {
                        dr["Checked"] = false;
                    }

                    dt.Rows.Add(dr);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }
    protected void imgBtnWageDDelete_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string TicketID = string.Empty;
            DataTable dt = GetWageDeductionGridItems();
            if (dt.Rows.Count > 1)
            {
                foreach (GridDataItem gr in RadGridWageDeduction.Items)
                {
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect1");
                    if (chkSelect.Checked == true)
                    {
                        DropDownList ddlWageD = (DropDownList)gr.FindControl("ddlWageD");
                        Int32 UserID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"]);
                        if (!IsWageRateIsUsed(UserID, Convert.ToInt32(ddlWageD.SelectedItem.Value), out TicketID))
                        {
                            DataRow dr = dt.Select("Ded = " + Convert.ToInt32(ddlWageD.SelectedValue)).FirstOrDefault();
                            if (dr != null)
                                dt.Rows.Remove(dr);
                        }
                        else
                        {
                            string str = "Wage deduction is used in Ticket #" + TicketID + "!";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "key112", "noty({text: '" + str + "',dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                            return;
                        }
                    }
                }
            }
            else if (dt.Rows.Count > 0)
            {
                foreach (GridDataItem gr in RadGridWageDeduction.Items)
                {
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect1");
                    if (chkSelect.Checked == true)
                    {
                        DropDownList ddlWageD = (DropDownList)gr.FindControl("ddlWageD");
                        Int32 UserID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"]);
                        if (!IsWageRateIsUsed(UserID, Convert.ToInt32(ddlWageD.SelectedItem.Value), out TicketID))
                        {
                            if (ddlWageD.SelectedValue != "0")
                            {
                                DataRow dr = dt.Select("Ded = " + Convert.ToInt32(ddlWageD.SelectedValue)).FirstOrDefault();
                                if (dr != null)
                                    dt.Rows.Remove(dr);

                                dr = dt.NewRow();
                                dr["Ded"] = 0;
                                dr["BasedOn"] = 0;
                                dr["AccruedOn"] = 0;
                                dr["ByW"] = 0;
                                dr["EmpRate"] = 0.00;
                                dr["EmpTop"] = 0.00;
                                dr["EmpGL"] = 0;
                                dr["EmpGLName"] = "";
                                dr["CompRate"] = 0.00;
                                dr["CompTop"] = 0.00;
                                dr["CompGL"] = 0;
                                dr["CompGLE"] = 0;
                                dr["CompGLName"] = "";
                                dr["CompGLEName"] = "";
                                dr["InUse"] = 0;
                                dr["YTD"] = 0;
                                dr["YTDC"] = 0;
                                dr["fdesc"] = "";
                                dt.Rows.Add(dr);

                            }
                        }
                        else
                        {
                            string str = "Wage deduction is used in Ticket #" + TicketID + "!";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "key1512", "noty({text: '" + str + "',dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                            return;
                        }

                    }
                }
            }
            ViewState["WageDeductionItems"] = dt;
            RadGridWageDeduction.DataSource = dt;
            //gvWagePayRate.DataBind();
            RadGridWageDeduction.Rebind();

            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "RemoveWageGridRow", "RemoveWageGridRow('" + gvWagePayRate.ClientID + "')", true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlWageD_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddlWageD = (DropDownList)sender;
            GridDataItem row = (GridDataItem)ddlWageD.NamingContainer;
            TextBox txtEmpRateD = (TextBox)row.FindControl("txtEmpRateD");
            TextBox txtEmpMaxRateD = (TextBox)row.FindControl("txtEmpMaxRateD");
            TextBox txtCompanyRateD = (TextBox)row.FindControl("txtCompanyRateD");
            TextBox txtCompanyMaxRateD = (TextBox)row.FindControl("txtCompanyMaxRateD");
            DropDownList ddlBasedOnD = (DropDownList)row.FindControl("ddlBasedOnD");
            DropDownList ddlAccuredOnD = (DropDownList)row.FindControl("ddlAccuredOnD");
            DropDownList ddlPaidbyD = (DropDownList)row.FindControl("ddlPaidbyD");
            

            HiddenField hdndCompGLE = (HiddenField)row.FindControl("hdndCompGLE");
            HiddenField hdndByW = (HiddenField)row.FindControl("hdndByW");
            HiddenField hdndEmpGL = (HiddenField)row.FindControl("hdndEmpGL");
            HiddenField hdndEmpGLName = (HiddenField)row.FindControl("hdndEmpGLName");
            HiddenField hdndCompGL = (HiddenField)row.FindControl("hdndCompGL");
            HiddenField hdndCompGLName = (HiddenField)row.FindControl("hdndCompGLName");

            HiddenField hdndCompGLEName = (HiddenField)row.FindControl("hdndCompGLEName");
            HiddenField hdndInUse = (HiddenField)row.FindControl("hdndInUse");
            HiddenField hdndYTD = (HiddenField)row.FindControl("hdndYTD");
            HiddenField hdndYTDC = (HiddenField)row.FindControl("hdndYTDC");
            HiddenField hdndfdesc = (HiddenField)row.FindControl("hdndfdesc");

            TextBox txtEmpGL = (TextBox)row.FindControl("txtEmpGL");
            TextBox txtCompGL = (TextBox)row.FindControl("txtCompGL");
            TextBox txtCompGLE = (TextBox)row.FindControl("txtCompGLE");

            _objPRDed.ConnConfig = Session["config"].ToString();
            _objPRDed.ID = Convert.ToInt32(ddlWageD.SelectedValue);
            DataSet ds = objBL_Wage.GetWageDeductionByID(_objPRDed);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtEmpRateD.Text = string.Format("{0:n}", Convert.ToDouble(dr["EmpRate"] == DBNull.Value ? 0 : dr["EmpRate"]));
                txtEmpMaxRateD.Text = string.Format("{0:n}", Convert.ToDouble(dr["EmpTop"] == DBNull.Value ? 0 : dr["EmpTop"]));
                txtCompanyRateD.Text = string.Format("{0:n}", Convert.ToDouble(dr["CompRate"] == DBNull.Value ? 0 : dr["CompRate"]));
                txtCompanyMaxRateD.Text = string.Format("{0:n}", Convert.ToDouble(dr["CompTop"] == DBNull.Value ? 0 : dr["CompTop"]));
                hdndCompGLE.Value = Convert.ToString(Convert.ToInt32(dr["CompGLE"] == DBNull.Value ? 0 : dr["CompGLE"]));
                hdndByW.Value = Convert.ToString(Convert.ToInt32(dr["ByW"] == DBNull.Value ? 0 : dr["ByW"]));
                hdndEmpGL.Value = Convert.ToString(Convert.ToInt32(dr["EmpGL"] == DBNull.Value ? 0 : dr["EmpGL"]));
                hdndEmpGLName.Value = Convert.ToString(dr["EmpGLAcct"].ToString());
                hdndCompGL.Value = Convert.ToString(Convert.ToInt32(dr["CompGL"] == DBNull.Value ? 0 : dr["CompGL"]));
                hdndCompGLName.Value = Convert.ToString(dr["CompGLAcct"].ToString());
                hdndCompGLEName.Value = Convert.ToString(dr["CompGLEAcct"].ToString());
                hdndInUse.Value = Convert.ToString(Convert.ToInt32(dr["InUse"] == DBNull.Value ? 0 : dr["InUse"]));
                hdndYTD.Value = "0";
                hdndYTDC.Value = "0";
                hdndfdesc.Value = Convert.ToString(dr["fdesc"].ToString());
                ddlBasedOnD.SelectedValue = Convert.ToString(Convert.ToInt32(dr["BasedOn"] == DBNull.Value ? 0 : dr["BasedOn"]));
                ddlAccuredOnD.SelectedValue = Convert.ToString(Convert.ToInt32(dr["AccruedOn"] == DBNull.Value ? 0 : dr["AccruedOn"]));
                ddlPaidbyD.SelectedValue = Convert.ToString(Convert.ToInt32(dr["ByW"] == DBNull.Value ? 0 : dr["ByW"]));
                
                txtEmpGL.Text = Convert.ToString(dr["EmpGLAcct"].ToString());
                txtCompGL.Text = Convert.ToString(dr["CompGLAcct"].ToString());
                txtCompGLE.Text = Convert.ToString(dr["CompGLEAcct"].ToString());
            }
            else
            {
                txtEmpRateD.Text = string.Format("{0:n}", 0);
                txtEmpMaxRateD.Text = string.Format("{0:n}", 0);
                txtCompanyRateD.Text = string.Format("{0:n}", 0);
                txtCompanyMaxRateD.Text = string.Format("{0:n}", 0);
                hdndCompGLE.Value = "0";
                hdndByW.Value = "0";
                hdndEmpGL.Value = "0";
                hdndEmpGLName.Value = "";
                hdndCompGL.Value = "0";
                hdndCompGLName.Value = "";

                hdndCompGLEName.Value = "";
                hdndInUse.Value = "0";
                hdndYTD.Value = "0";
                hdndYTDC.Value = "0";
                hdndfdesc.Value = "";
                ddlBasedOnD.SelectedValue = "0";
                ddlAccuredOnD.SelectedValue = "0";
                txtEmpGL.Text = "";
                txtCompGL.Text = "";
                txtCompGLE.Text = "";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void imgBtnWageDAdd_Click(object sender, EventArgs e)
    {
        AddNewRowWageDeduction();
    }
    private void AddNewRowWageDeduction()
    {

        DataTable dt = GetWageDeductionGridItems();
        DataRow dr = dt.NewRow();
        dr["Ded"] = 0;
        dr["BasedOn"] = 0;
        dr["AccruedOn"] = 0;
        dr["ByW"] = 0;
        dr["EmpRate"] = 0.00;
        dr["EmpTop"] = 0.00;
        dr["EmpGL"] = 0;
        dr["EmpGLName"] = "";
        dr["CompRate"] = 0.00;
        dr["CompTop"] = 0.00;
        dr["CompGL"] = 0;
        dr["CompGLE"] = 0;
        dr["CompGLName"] = "";
        dr["CompGLEName"] = "";
        dr["InUse"] = 0;
        dr["YTD"] = 0.00;
        dr["YTDC"] = 0.00;
        dr["fdesc"] = "";
        dt.Rows.Add(dr);
        //dt1 = (DataTable)ViewState["WageItems"];
        RadGridWageDeduction.DataSource = dt;
        //gvWagePayRate.DataBind();
        RadGridWageDeduction.Rebind();
    }
    protected void btnCopyRateDed_Click(object sender, EventArgs e)
    {
        try
        {
            string TicketID = string.Empty;
            DataTable dt = GetWageDeductionGridItems();


            if (dt.Rows.Count > 1)
            {
                DataRow dr = dt.Select("Checked = true").FirstOrDefault();
                foreach (DataRow row in dt.Rows)
                {
                    row["Ded"] = dr["Ded"];
                    row["BasedOn"] = dr["BasedOn"];
                    row["AccruedOn"] = dr["AccruedOn"];
                    row["ByW"] = dr["ByW"];
                    row["EmpRate"] = dr["EmpRate"];
                    row["EmpTop"] = dr["EmpTop"];
                    row["EmpGL"] = dr["EmpGL"];
                    row["EmpGLName"] = dr["EmpGLName"];
                    row["CompRate"] = dr["CompRate"];
                    row["CompTop"] = dr["CompTop"];
                    row["CompGL"] = dr["CompGL"];
                    row["CompGLE"] = dr["CompGLE"];
                    row["CompGLName"] = dr["CompGLName"];
                    row["CompGLEName"] = dr["CompGLEName"];
                    row["InUse"] = dr["InUse"];
                    row["YTD"] = dr["YTD"];
                    row["YTDC"] = dr["YTDC"];
                    row["fdesc"] = dr["fdesc"];

                }
            }

            ViewState["WageDeductionItems"] = dt;
            RadGridWageDeduction.DataSource = dt;
            ////gvWagePayRate.DataBind();
            RadGridWageDeduction.Rebind();

            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "RemoveWageGridRow", "RemoveWageGridRow('" + gvWagePayRate.ClientID + "')", true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void RadGridWageDeduction_ItemDataBound(object sender, GridItemEventArgs e)
    {
        string TicketID = string.Empty;

        if (e.Item is GridDataItem)
        {
            DropDownList ddlWageD = (DropDownList)e.Item.FindControl("ddlWageD");
            Int32 UserID = Request.QueryString["uid"] == null ? 0 : Convert.ToInt32(Request.QueryString["uid"]);

            if (IsWageRateIsUsed(UserID, Convert.ToInt32(ddlWageD.SelectedItem.Value), out TicketID))
            {
                string str = "Wage deduction is used in Ticket #" + TicketID + "!";

                ddlWageD.Attributes["onclick"] = "   noty({ text: '" + str + "', type: 'warning', layout: 'topCenter', closeOnSelfClick: true, timeout: 5000, theme: 'noty_theme_default', closable: false,dismissQueue:true });";

                foreach (ListItem item in ddlWageD.Items)
                {
                    if (item.Value != ddlWageD.SelectedItem.Value) { item.Enabled = false; }
                }
            }


        }
    }
    protected void RadGridWageDeduction_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        
        CreateWageDedcutionTable();

        if (Request.QueryString["uid"] != null)
        {
            DataSet ds = new DataSet();
            ds = (DataSet)ViewState["getUserById"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["uid"].ToString()))
                {
                    //objPropUser.ConnConfig = Session["config"].ToString();
                    //objPropUser.EmpId = Convert.ToInt32(ViewState["empid"].ToString());
                    //DataSet dsWage = objBL_User.GetEmpWageItems(objPropUser);
                    //ViewState["dsWage"] = dsWage;

                    //if (dsWage.Tables[0].Rows.Count > 0)
                    //{
                    //    gvWagePayRate.DataSource = dsWage.Tables[0];
                    //    //gvWagePayRate.DataBind();
                    //    ViewState["WageItems"] = dsWage.Tables[0];
                    //}
                    //else
                    //{
                    //    CreateWageTable();
                    //}

                    DataSet dsWage = new DataSet();
                    _objEmp.ConnConfig = Session["config"].ToString();
                    _objEmp.ID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                    dsWage = objBL_Wage.GetEmployeeListByID(_objEmp);
                    //ViewState["dsWage"] = dsWage;

                    if (dsWage.Tables[1].Rows.Count > 0)
                    {
                        RadGridWageDeduction.DataSource = dsWage.Tables[2];
                        //gvWagePayRate.DataBind();
                        ViewState["WageDeductionItems"] = dsWage.Tables[2];
                    }
                    else
                    {
                        CreateWageDedcutionTable();
                    }
                }
            }
        }


    }
    private void CreateWageDedcutionTable()
    {

        DataTable dt = new DataTable();
        dt.Columns.Add("Ded", typeof(int));
        dt.Columns.Add("BasedOn", typeof(int));
        dt.Columns.Add("AccruedOn", typeof(int));
        dt.Columns.Add("ByW", typeof(int));
        dt.Columns.Add("EmpRate", typeof(double));
        dt.Columns.Add("EmpTop", typeof(double));
        dt.Columns.Add("EmpGL", typeof(int));
        
        dt.Columns.Add("CompRate", typeof(double));
        dt.Columns.Add("CompTop", typeof(double));
        dt.Columns.Add("CompGL", typeof(int));
        
        dt.Columns.Add("CompGLE", typeof(int));
        
        dt.Columns.Add("InUse", typeof(int));
        dt.Columns.Add("YTD", typeof(double));
        dt.Columns.Add("YTDC", typeof(double));
        dt.Columns.Add("fdesc", typeof(string));
        dt.Columns.Add("EmpGLName", typeof(string));
        dt.Columns.Add("CompGLName", typeof(string));
        dt.Columns.Add("CompGLEName", typeof(string));
        dt.Columns.Add("Checked", typeof(bool));

        DataRow dr = dt.NewRow();
        dr["Ded"] = 0;
        dr["BasedOn"] = 0;
        dr["AccruedOn"] = 0;
        dr["ByW"] = 0;
        dr["EmpRate"] = 0.00;
        dr["EmpTop"] = 0.00;
        dr["EmpGL"] = 0;
        dr["EmpGLName"] = "";
        dr["CompRate"] = 0.00;
        dr["CompTop"] = 0.00;
        dr["CompGL"] = 0;
        dr["CompGLE"] = 0;
        dr["CompGLName"] = "";
        dr["CompGLEName"] = "";
        dr["InUse"] = 0;
        dr["YTD"] = 0.00;
        dr["YTDC"] = 0.00;
        dr["fdesc"] = "";
        dr["Checked"] = false;
        dt.Rows.Add(dr);
        ViewState["WageDeductionItems"] = dt;
        RadGridWageDeduction.DataSource = dt;


    }
    private void FillDefaultWage()
    {
        try
        {
            //DataSet ds = new DataSet();
            //_objWage.ConnConfig = Session["config"].ToString();

            //ds = new BL_Wage().getWage(_objWage);

            //ddlDefaultWage.DataSource = ds.Tables[0];
            //ddlDefaultWage.DataTextField = "fdesc";
            //ddlDefaultWage.DataValueField = "id";
            //ddlDefaultWage.DataBind();

            //ddlDefaultWage.Items.Insert(0, new ListItem(":: Select ::", ""));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void FillWageDeductionCategorys()
    {
        try
        {
            DataSet ds = new DataSet();
            _objPRDed.ConnConfig = HttpContext.Current.Session["config"].ToString();
            ds = objBL_Wage.GetWageDeduction(_objPRDed);
            if (ds.Tables.Count > 0)
            {
                DataRow dr = ds.Tables[0].NewRow();
                dr["ID"] = 0;
                dr["Ded"] = 0;
                dr["fDesc"] = "Select Deduction";
                ds.Tables[0].Rows.InsertAt(dr, 0);
                dtWageDeduction = ds.Tables[0];
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void RadGridWageDeduction_PreRender(object sender, EventArgs e)
    {
        foreach (GridDataItem gr in RadGridWageDeduction.Items)
        {
            TextBox txtEmpGL = (TextBox)gr.FindControl("txtEmpGL");
            gr.Attributes["onclick"] = "VisibleRow('" + gr.ClientID + "','" + txtEmpGL.ClientID + "','" + RadGridWageDeduction.ClientID + "',event);";
            
            
        }
    }
    private bool IsValidOtherWageRateGrid()
    {
        try
        {
            DataTable dtOtherWage = GetOtherWageGridItems();
            var lst = dtOtherWage.Rows.Cast<DataRow>().Where(r => (int)r.ItemArray[0] == 0).ToList();
            //dtOtherWage.Rows.Cast<DataRow>().Where(r => (int)r["Cat"] == 0).ToList().ForEach(r => r.Delete());
            //dtOtherWage.AcceptChanges();
            ViewState["OtherWageItems"] = dtOtherWage;
            DataView dv = new DataView(dtOtherWage);
            DataTable distDt = dv.ToTable(true, "Cat");
            if (distDt.Rows.Count != dtOtherWage.Rows.Count)
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
    private bool IsValidWageDedcutionRateGrid()
    {
        try
        {
            DataTable dtWage = GetWageDeductionGridItems();
            var lst = dtWage.Rows.Cast<DataRow>().Where(r => (int)r.ItemArray[0] == 0).ToList();
            dtWage.Rows.Cast<DataRow>().Where(r => (int)r["Ded"] == 0).ToList().ForEach(r => r.Delete());
            dtWage.AcceptChanges();
            ViewState["WageDeductionItems"] = dtWage;
            DataView dv = new DataView(dtWage);
            DataTable distDt = dv.ToTable(true, "Ded");
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
    protected void lnkChk_CheckedChanged(object sender, EventArgs e)
    {
        if (lnkChk.Checked)
        {
            Session["InInActiveWageEmp"] = "True";
            check = true;
            if (ViewState["WageItems"] != null)
            {
                DataTable _dataTableWageItems = (DataTable)ViewState["WageItems"];

                DataTable filterdt = new DataTable();
                check = Convert.ToBoolean(Session["InInActiveWageEmp"]);
                if (check)
                {
                    lnkChk.Checked = true;
                    filterdt = _dataTableWageItems;
                }
                else
                {
                    if (_dataTableWageItems.Rows.Count > 0)
                    {
                        DataRow[] dr = _dataTableWageItems.Select("StatusName='Active'");
                        if (dr.Length > 0)
                        {
                            filterdt = dr.CopyToDataTable();
                        }
                        else
                        {
                            filterdt = _dataTableWageItems.Clone();
                        }
                    }
                    else
                    {
                        filterdt = _dataTableWageItems;

                    }
                }
                gvWagePayRate.VirtualItemCount = filterdt.Rows.Count;
                gvWagePayRate.DataSource = filterdt;

                gvWagePayRate.Rebind();
            }
        }
        else
        {
            Session["InInActiveWageEmp"] = "False";
            check = false;
            if (ViewState["WageItems"] != null)
            {
                DataTable _dataTableWageItems = (DataTable)ViewState["WageItems"];
                DataTable filterdt = new DataTable();
                check = Convert.ToBoolean(Session["InInActiveWageEmp"]);
                if (check)
                {
                    lnkChk.Checked = true;
                    filterdt = _dataTableWageItems;
                }
                else
                {
                    if (_dataTableWageItems.Rows.Count > 0)
                    {
                        DataRow[] dr = _dataTableWageItems.Select("StatusName='Active'");
                        if (dr.Length > 0)
                        {
                            filterdt = dr.CopyToDataTable();
                        }
                        else
                        {
                            filterdt = _dataTableWageItems.Clone();
                        }
                    }
                    else
                    {
                        filterdt = _dataTableWageItems;

                    }
                }
                gvWagePayRate.VirtualItemCount = filterdt.Rows.Count;
                gvWagePayRate.DataSource = filterdt;
                gvWagePayRate.Rebind();
            }

        }
    }
    private void FillFillingState()
    {
        try
        {
            DataSet _dsState = new DataSet();
            State _objState = new State();
            BL_BankAccount _objBLBank = new BL_BankAccount();
            _objState.ConnConfig = Session["config"].ToString();
            _dsState = _objBLBank.GetStates(_objState);
            //ddlFillingState.Items.Add(new ListItem("Select State", ""));
            //ddlFillingState.AppendDataBoundItems = true;
            //ddlFillingState.DataSource = _dsState;
            //ddlFillingState.DataValueField = "Name";
            //ddlFillingState.DataTextField = "fDesc";
            //ddlFillingState.DataBind();
           
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void imgBtnAddFillingState_Click(object sender, EventArgs e)
    {

    }

    
    protected void ddlFillingStatus_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlFillingState_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void imgBtnFillingStateDelete_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void gvFillingStates_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        CreateFillingStateTable();
    }

    private void CreateFillingStateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("StateId", typeof(int));
        dt.Columns.Add("State", typeof(string));
        dt.Columns.Add("FillingStatusId", typeof(int));
        dt.Columns.Add("FillingStatus", typeof(string));
        dt.Columns.Add("Allowances", typeof(int)); 
        dt.Columns.Add("AdditionalAmount", typeof(double));
        dt.Columns.Add("Default", typeof(bool));

        DataRow dr = dt.NewRow();
        dr["StateId"] = 0;
        dr["State"] = "";
        dr["FillingStatusId"] = 0;
        dr["FillingStatus"] = "";
        dr["Allowances"] = 0;
        dr["AdditionalAmount"] = 0;
        dr["Default"] = false;

        dt.Rows.Add(dr);
        ViewState["FillingStates"] = dt;
        gvFillingStates.DataSource = dt;
    }
}
