using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using BusinessEntity.Utility;
using BusinessLayer;
using MOMWebApp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AddDeposit : System.Web.UI.Page
{
    #region Variables
    ReceivedPayment _objReceivePay = new ReceivedPayment();
    BL_Deposit _objBL_Deposit = new BL_Deposit();

    Chart _objChart = new Chart();
    BL_Chart _objBL_Chart = new BL_Chart();
    Bank _objBank = new Bank();
    BL_BankAccount _objBL_Bank = new BL_BankAccount();

    Contracts _objPropContracts = new Contracts();
    Dep _objDep = new Dep();
    PaymentDetails _objPayment = new PaymentDetails();

    Journal _objJournal = new Journal();
    Transaction _objTrans = new Transaction();
    BL_JournalEntry _objBLJournal = new BL_JournalEntry();

    DepositDetails _objDepDetails = new DepositDetails();
    OpenAR _objOpenAR = new OpenAR();

    User _objPropUser = new User();
    BL_User _objBLUser = new BL_User();
    public int depIdVal = 0;
    public int IsRecond = 0;

    //API Variables 
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim(); 
    GetDepByIDParam _GetDepByID = new GetDepByIDParam();
    GetDepHeadByIDParam _GetDepHeadByID = new GetDepHeadByIDParam();
    UpdateDepositParam _UpdateDeposit = new UpdateDepositParam();
    GetAllInvoiceByDepParam _GetAllInvoiceByDep = new GetAllInvoiceByDepParam();
    GetReceivedPaymentByDepParam _GetReceivedPaymentByDep = new GetReceivedPaymentByDepParam();
    GetAllReceivePaymentForDepParam _GetAllReceivePaymentForDep = new GetAllReceivePaymentForDepParam();
    GetAllBankNamesParam _GetAllBankNames = new GetAllBankNamesParam();
    DepositInfor_UpdateDepositParam _DepositInfor_UpdateDeposit = new DepositInfor_UpdateDepositParam();
    UpdateReceivedPayStatusParam _UpdateReceivedPayStatus = new UpdateReceivedPayStatusParam();
    GetBankAcctIDParam _GetBankAcctID = new GetBankAcctIDParam();
    UpdateDepositTransBankParam _UpdateDepositTransBank = new UpdateDepositTransBankParam();
    AddDepositWithGLParam _AddDepositWithGL = new AddDepositWithGLParam();
    UpdateChartBalanceParam _UpdateChartBalance = new UpdateChartBalanceParam();
    GetScreensByUserParam _GetScreensByUser = new GetScreensByUserParam();

    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }
            if (!IsPostBack)
            {
                imgCleared.Visible = false;
                // userpermissions();
                if (!CheckAddEditPermission()) { Response.Redirect("Home.aspx?permission=no"); return; }
                divSuccess.Visible = false;
                _objDep.ConnConfig = Session["config"].ToString();

                _GetDepByID.ConnConfig = Session["config"].ToString();
                _GetDepHeadByID.ConnConfig = Session["config"].ToString();

                lblDepositTotal.Text = string.Format("{0:c}", 0);
                FillBank();

                if (Request.QueryString["id"] != null)   // EDIT
                {
                    Page.Title = "Edit Deposit || MOM";
                    pnlNext.Visible = true;                   

                    liReceiptPay.Style["display"] = "none";
                    liInvoice.Style["display"] = "";
                    adEditPayment.Visible = false;
                    adEditInvoice.Visible = true;
                   
                    RadGrid_gvInvoiceDeposit.Visible = true;
                    RadGrid_gvDepositGL.Visible = true;
                    RadGrid_gvReceivePayment.Visible = false;
                    lblRef.Visible = true;
                    lblRefId.Visible = true;
                    lblRefId.Text = Request.QueryString["id"];
                    lblHeader.Text = "Edit Deposit";
                    _objDep.Ref = Convert.ToInt32(Request.QueryString["id"]);

                    _GetDepByID.Ref = Convert.ToInt32(Request.QueryString["id"]);
                    _GetDepHeadByID.Ref = Convert.ToInt32(Request.QueryString["id"]);

                    DataSet _dsDep = new DataSet();
                    List<GetDepByIDViewModel> _lstGetDepByID = new List<GetDepByIDViewModel>();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "MakeDepositAPI/AddDeposit_GetDepByID";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetDepByID);

                        _lstGetDepByID = (new JavaScriptSerializer()).Deserialize<List<GetDepByIDViewModel>>(_APIResponse.ResponseData);
                        _dsDep = CommonMethods.ToDataSet<GetDepByIDViewModel>(_lstGetDepByID);
                    }
                    else
                    {
                        _dsDep = _objBL_Deposit.GetDepByID(_objDep);
                    }

                    DataSet _sessionds = new DataSet();
                    List<GetDepHeadByIDViewModel> _lstGetDepHeadByID = new List<GetDepHeadByIDViewModel>();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "MakeDepositAPI/AddDeposit_GetDepHeadByID";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetDepHeadByID);

                        _lstGetDepHeadByID = (new JavaScriptSerializer()).Deserialize<List<GetDepHeadByIDViewModel>>(_APIResponse.ResponseData);
                        _sessionds = CommonMethods.ToDataSet<GetDepHeadByIDViewModel>(_lstGetDepHeadByID);
                    }
                    else
                    {
                        _sessionds = _objBL_Deposit.GetDepHeadByID(_objDep);
                    }

                    Session["DepositHead"] = _sessionds;
                    Session["RefNo"] = lblRefId.Text;
                    if (_dsDep.Tables[0].Columns.Contains("Ref"))
                    {
                        SetDeposit(_dsDep.Tables[0]);                     
                    }
                    hdnIsRecon.Value = "0";
                    if (_dsDep.Tables[0].Rows[0]["IsRecon"].ToString()=="1")
                    {
                        btnSubmit.Visible = false;
                        RadGrid_gvInvoiceDeposit.Enabled = false;
                        RadGrid_gvDepositGL.Enabled = false;
                        RadGrid_gvReceivePayment.Enabled = false;
                        hdnIsRecon.Value = "1";
                        imgCleared.Visible = true;
                    }
                }
                else
                {
                    Page.Title = "Add Deposit || MOM";
                    pnlNext.Visible = false;
                    txtDateDeposite.Text = DateTime.Now.ToShortDateString();
                    txtMemo.Text = "Deposit";                  
                    RadGrid_gvReceivePayment.Visible = true;
                    RadGrid_gvDepositGL.Visible = true;
                    RadGrid_gvInvoiceDeposit.Visible = false;
                    BindReceivedPaymentGrid();               
                   
                
                    adEditPayment.Visible = true;
                    adEditInvoice.Visible = false;
                    liReceiptPay.Style["display"] = "";
                    liInvoice.Style["display"] = "none";

                }

               // GetPeriodDetails(Convert.ToDateTime(txtDateDeposite.Text));

              
            }
            CompanyPermission();
            Permission();
            HighlightSideMenu("cstmMgr", "lnkDeposit", "cstmMgrSub");
            GetPeriodDetails(Convert.ToDateTime(txtDateDeposite.Text));
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
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
    protected void Page_PreRender(Object o, EventArgs e)
    {
        try
        {
            foreach (GridDataItem gr in RadGrid_gvReceivePayment.Items)

            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");

                AjaxControlToolkit.HoverMenuExtender hmeRes = (AjaxControlToolkit.HoverMenuExtender)gr.FindControl("hmeRes");
                //gr.Attributes["onclick"] = "SelectRowChk('" + gr.ClientID + "','" + chkSelect.ClientID + "','" + gvReceivePayment.ClientID + "',event);";

                //gr.Attributes["ondblclick"] = "";
                //"window.open('addreceivepayment.aspx?id=" + lblId.Text + "');";

            }
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "SelectedRowStyle('" + gvReceivePayment.ClientID + "');", true);


            //foreach (GridDataItem gr in RadGrid_gvDeposit.Items)
            //{
            //    TextBox txtDescription = (TextBox)gr.FindControl("txtDescription");
            //    gr.Attributes["onclick"] = "VisibleRow('" + gr.ClientID + "','" + txtDescription.ClientID + "','" + RadGrid_gvDeposit.ClientID + "',event);";
            //}
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    //protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int count = 0;

    //        DataSet _dsDep = new DataSet();
    //        foreach (GridDataItem gr in RadGrid_gvReceivePayment.Items)
    //        {
    //            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
    //            HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");

    //            if (chkSelect.Checked.Equals(true))
    //            {
    //                _objReceivePay.ConnConfig = Session["config"].ToString();
    //                _objReceivePay.ID = Convert.ToInt32(hdnID.Value);
    //                DataSet _dsRecev = new DataSet();
    //                _dsRecev = _objBL_Deposit.GetReceivePaymentDetailsByID(_objReceivePay);
    //                if (count > 0)
    //                {
    //                    DataTable _dt = new DataTable();
    //                    _dt = _dsRecev.Tables[0];
    //                    _dsDep.Merge(_dsRecev, true);
    //                }
    //                else
    //                {
    //                    _dsDep = _dsRecev;
    //                }
    //                count++;
    //            }
    //        }

    //        GridFooterItem footerItem = (GridFooterItem)RadGrid_gvReceivePayment.MasterTableView.GetItems(GridItemType.Footer)[0];
    //        Label lblSelectedItems = footerItem.FindControl("lblSelectedItems") as Label;
    //        lblSelectedItems.Text = "Selected Item(s) " + Convert.ToString(count);

    //        if (count > 0)
    //        {
    //            if (_dsDep.Tables[0].Columns.Contains("Loc"))
    //            {

    //                RadGrid_gvDeposit.DataSource = _dsDep.Tables[0];
    //                RadGrid_gvDeposit.Rebind();
    //                lblRecordCount.Text = Convert.ToString(_dsDep.Tables[0].Rows.Count) + " Record(s) found";
    //                double totalAmount = Convert.ToDouble(_dsDep.Tables[0].Compute("SUM(Amount)", string.Empty));
    //                lblDepositTotal.Text = string.Format("{0:c}", totalAmount);
    //                UpdDeposit.Update();
    //            }
    //        }
    //        else
    //        {
    //            DataTable dt = new DataTable();
    //            RadGrid_gvDeposit.DataSource = dt;
    //            RadGrid_gvDeposit.Rebind();
    //            lblRecordCount.Text = Convert.ToString(dt.Rows.Count) + " Record(s) found";
    //            lblDepositTotal.Text = string.Format("{0:c}", 0);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
    //    }
    //}

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {           
            if(Request.QueryString["id"] != null)
            {
                UpdateDepositInfor();
                doEditDeposit();
            }
            else
            {
                if (Confirm_Value.Value == "Yes")
                {
                   
                        submit();

                  


                    _objDep.Ref = depIdVal;
                    _GetDepByID.Ref = depIdVal;
                    _GetDepHeadByID.Ref = depIdVal;

                    DataSet _dsDep = new DataSet();
                    List<GetDepByIDViewModel> _lstGetDepByID = new List<GetDepByIDViewModel>();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "MakeDepositAPI/AddDeposit_GetDepByID";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetDepByID);

                        _lstGetDepByID = (new JavaScriptSerializer()).Deserialize<List<GetDepByIDViewModel>>(_APIResponse.ResponseData);
                        _dsDep = CommonMethods.ToDataSet<GetDepByIDViewModel>(_lstGetDepByID);
                    }
                    else
                    {
                        _dsDep = _objBL_Deposit.GetDepByID(_objDep);
                    }

                    DataSet _sessionds = new DataSet();
                    List<GetDepHeadByIDViewModel> _lstGetDepHeadByID = new List<GetDepHeadByIDViewModel>();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "MakeDepositAPI/AddDeposit_GetDepHeadByID";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetDepHeadByID);

                        _lstGetDepHeadByID = (new JavaScriptSerializer()).Deserialize<List<GetDepHeadByIDViewModel>>(_APIResponse.ResponseData);
                        _sessionds = CommonMethods.ToDataSet<GetDepHeadByIDViewModel>(_lstGetDepHeadByID);
                    }
                    else
                    {
                         _sessionds = _objBL_Deposit.GetDepHeadByID(_objDep);
                    }

                    Session["DepositHead"] = _sessionds;
                    Session["RefNo"] = depIdVal.ToString();
                    if (_dsDep.Tables[0].Columns.Contains("Ref"))
                    {
                        SetDeposit(_dsDep.Tables[0]);
                        //gvDeposit.Columns[5].Visible = false;
                        //Response.Redirect("DepositReport.aspx");
                        Response.Redirect("DepositSlipReport.aspx?uid=" + depIdVal);
                    }
                }

                else
                {

                    submit();
                }
            }
            
        }
        catch (Exception)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'There is an error. Please try again later.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void doEditDeposit()
    {
        DataTable dtDelete = CreateInvoiceTable();
        DataTable dtNew = CreateInvoiceTable();

        if (ViewState["InvoiceTablesDelete"] != null)
        {
            dtDelete = (DataTable)ViewState["InvoiceTablesDelete"];
        }
        DataTable dtDeleteGL = new DataTable();
        if (ViewState["GLTablesDelete"] != null)
        {
            dtDeleteGL = (DataTable)ViewState["GLTablesDelete"];
        }
        else
        {
            dtDeleteGL.Columns.Add("ID", typeof(int));
            dtDeleteGL.Columns.Add("Amount", typeof(double));
            dtDeleteGL.Columns.Add("Description", typeof(String));

            //_UpdateDeposit.dtDeleteGL.Columns.Add("ID", typeof(int));
            //_UpdateDeposit.dtDeleteGL.Columns.Add("Amount", typeof(double));
            //_UpdateDeposit.dtDeleteGL.Columns.Add("Description", typeof(String));
        }

        //DataTable dtDeleteGL = new DataTable();


        DataTable dtNewGL = new DataTable();
        dtNewGL.Columns.Add("ID", typeof(int));
        dtNewGL.Columns.Add("Amount", typeof(double));
        dtNewGL.Columns.Add("Description", typeof(String));
        try
        {
            //Invoice
            foreach (GridDataItem gr in RadGrid_gvInvoiceDeposit.Items)
            {

                HiddenField hdnOwner = (HiddenField)gr.FindControl("hdnOwner");
                HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");
                TextBox txtInvoiceID = (TextBox)gr.FindControl("txtInvoiceID");
                HiddenField hdnRol = (HiddenField)gr.FindControl("hdnRol");
                TextBox txtCustomerName = (TextBox)gr.FindControl("txtCustomerName");
                HiddenField hdnLoc = (HiddenField)gr.FindControl("hdnLoc");
                TextBox txtTag = (TextBox)gr.FindControl("txtLocationName");
                HiddenField hdnEn = (HiddenField)gr.FindControl("hdnEn");
                HiddenField hdnCompany = (HiddenField)gr.FindControl("hdnCompany");
                TextBox txtAmount = (TextBox)gr.FindControl("txtAmount");
                TextBox txtPaymentReceivedDate = (TextBox)gr.FindControl("txtPaymentReceivedDate");
                TextBox txtDesc = (TextBox)gr.FindControl("txtDesc");
                DropDownList ddlPaymentMethod = (DropDownList)gr.FindControl("ddlPaymentMethod");
                TextBox txtCheckNumber = (TextBox)gr.FindControl("txtCheckNumber");
                HiddenField hdnAmountDue = (HiddenField)gr.FindControl("hdnAmountDue");
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkInvSelect");
                HiddenField hdnOrderNo = (HiddenField)gr.FindControl("hdnOrderNo");
                HiddenField hdnRefTranID = (HiddenField)gr.FindControl("hdnRefTranID");

                if (Convert.ToInt32(hdnID.Value) == 0  )
                {
                    if (hdnOwner.Value!="0" )
                    {
                        DataRow dr = dtNew.NewRow();
                        dr["Owner"] = hdnOwner.Value;
                        dr["ID"] = hdnID.Value;
                        dr["InvoiceID"] = txtInvoiceID.Text == "" ? 0 : Convert.ToInt32(txtInvoiceID.Text);
                        dr["Rol"] = hdnRol.Value;
                        dr["customerName"] = txtCustomerName.Text;
                        dr["loc"] = hdnLoc.Value;
                        dr["Tag"] = txtTag.Text;
                        dr["En"] = hdnEn.Value;
                        dr["Company"] = hdnCompany.Value;
                        dr["Amount"] = double.Parse(txtAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                     NumberStyles.AllowThousands |
                                     NumberStyles.AllowDecimalPoint |NumberStyles.AllowLeadingSign);
                        dr["PaymentReceivedDate"] = txtPaymentReceivedDate.Text;
                        dr["fDesc"] = txtDesc.Text;
                        dr["PaymentMethod"] = ddlPaymentMethod.SelectedValue;
                        dr["CheckNumber"] = txtCheckNumber.Text;
                        dr["AmountDue"] = double.Parse(hdnAmountDue.Value.Replace('$', '0'), NumberStyles.AllowParentheses |
                                     NumberStyles.AllowThousands |
                                     NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                        dr["RefTranID"] = hdnRefTranID.Value == "" ? 0 : Convert.ToInt32(hdnRefTranID.Value);
                        dtNew.Rows.Add(dr);
                    }                    

                }
            }
            
            //GL
            foreach (GridDataItem gr in RadGrid_gvDepositGL.Items)
            {

                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");
                HiddenField hdnTransID = (HiddenField)gr.FindControl("hdnTransID");
                TextBox txtGLAmount = (TextBox)gr.FindControl("txtGLAmount");
                TextBox txtDesc = (TextBox)gr.FindControl("txtDesc");
                TextBox txtRef = (TextBox)gr.FindControl("txtRef");
                if (Convert.ToInt32(hdnTransID.Value) == 0)
                {
                    if (hdnID.Value != "")
                    {
                        DataRow dr = dtNewGL.NewRow();
                        dr["ID"] = hdnID.Value;
                        dr["Amount"] = (-1) * double.Parse(txtGLAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                    NumberStyles.AllowThousands |
                                    NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                        dr["Description"] = txtDesc.Text;

                        dtNewGL.Rows.Add(dr);
                    }
                    
                }
            }

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "MakeDepositAPI/AddDeposit_UpdateDeposit";

                _UpdateDeposit.ConnConfig = Session["config"].ToString();
                _UpdateDeposit.depId = Convert.ToInt32(Request.QueryString["id"]);

                if (dtNew.Rows.Count == 0)
                {
                    DataTable returnVal = InvoiceEmptyTablesDelete();
                    _UpdateDeposit.dtNew = returnVal;
                }
                else
                {
                    _UpdateDeposit.dtNew = dtNew;
                }

                if (dtNewGL.Rows.Count == 0)
                {
                    DataTable returnVal = GLEmptyTablesDelete();
                    _UpdateDeposit.dtNewGL = returnVal;
                }
                else
                {
                    _UpdateDeposit.dtNewGL = dtNewGL;
                }

                if (dtDelete.Rows.Count == 0)
                {
                    DataTable returnVal = InvoiceEmptyTablesDelete();
                    _UpdateDeposit.dtDelete = returnVal;
                }
                else
                {
                    _UpdateDeposit.dtDelete = dtDelete;
                }

                if (dtDeleteGL.Rows.Count == 0)
                {
                    DataTable returnVal = GLEmptyTablesDelete();
                    _UpdateDeposit.dtDeleteGL = returnVal;
                }
                else
                {
                    _UpdateDeposit.dtDeleteGL = dtDeleteGL;
                }

                _UpdateDeposit.UpdatedBy = Session["username"].ToString();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateDeposit);
            }
            else
            {
                _objBL_Deposit.UpdateDeposit(Session["config"].ToString(), Convert.ToInt32(Request.QueryString["id"]), dtDelete, dtNew, dtDeleteGL, dtNewGL, Session["username"].ToString());
            }

            _objDep.Ref = Convert.ToInt32(Request.QueryString["id"]);
            _objDep.ConnConfig = Session["config"].ToString();

            _GetDepByID.Ref = Convert.ToInt32(Request.QueryString["id"]);
            _GetDepByID.ConnConfig = Session["config"].ToString();

            _GetDepHeadByID.Ref = Convert.ToInt32(Request.QueryString["id"]);
            _GetDepHeadByID.ConnConfig = Session["config"].ToString();

            DataSet _dsDep = new DataSet();
            List<GetDepByIDViewModel> _lstGetDepByID = new List<GetDepByIDViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "MakeDepositAPI/AddDeposit_GetDepByID";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetDepByID);

                _lstGetDepByID = (new JavaScriptSerializer()).Deserialize<List<GetDepByIDViewModel>>(_APIResponse.ResponseData);
                _dsDep = CommonMethods.ToDataSet<GetDepByIDViewModel>(_lstGetDepByID);
            }
            else
            {
                _dsDep = _objBL_Deposit.GetDepByID(_objDep);
            }

            DataSet _sessionds = new DataSet();
            List<GetDepHeadByIDViewModel> _lstGetDepHeadByID = new List<GetDepHeadByIDViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "MakeDepositAPI/AddDeposit_GetDepHeadByID";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetDepHeadByID);

                _lstGetDepHeadByID = (new JavaScriptSerializer()).Deserialize<List<GetDepHeadByIDViewModel>>(_APIResponse.ResponseData);
                _sessionds = CommonMethods.ToDataSet<GetDepHeadByIDViewModel>(_lstGetDepHeadByID);
            }
            else
            {
                _sessionds = _objBL_Deposit.GetDepHeadByID(_objDep);
            }
            Session["DepositHead"] = _sessionds;
            Session["RefNo"] = lblRefId.Text;
            depIdVal =Convert.ToInt32(lblRefId.Text);
            if (_dsDep.Tables[0].Columns.Contains("Ref"))
            {
                SetDeposit(_dsDep.Tables[0]);                
            }

                
            if (Confirm_Value.Value == "Yes")
            {
                // Response.Redirect("DepositReport.aspx");
                Response.Redirect("DepositSlipReport.aspx?uid=" + depIdVal);

            }

            DataSet _dsDepNew = new DataSet();
            DataSet _dsDepNew1 = new DataSet();
            DataSet _dsDepNew2 = new DataSet();
            ListGetAllInvoiceByDep _lstGetAllInvoiceByDep = new ListGetAllInvoiceByDep();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "MakeDepositAPI/AddDeposit_GetAllInvoiceByDep";

                _GetAllInvoiceByDep.ConnConfig = Session["config"].ToString();
                _GetAllInvoiceByDep.depId = Convert.ToInt32(Request.QueryString["id"]);

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetAllInvoiceByDep);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetAllInvoiceByDep = serializer.Deserialize<ListGetAllInvoiceByDep>(_APIResponse.ResponseData);

                _dsDepNew1 = _lstGetAllInvoiceByDep.lstTable1.ToDataSet();
                _dsDepNew2 = _lstGetAllInvoiceByDep.lstTable2.ToDataSet();

                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                dt1 = _dsDepNew1.Tables[0];
                dt2 = _dsDepNew2.Tables[0];

                dt1.TableName = "Table1";
                dt2.TableName = "Table2";

                _dsDepNew.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });
            }
            else
            {
                _dsDepNew = _objBL_Deposit.GetAllInvoiceByDep(Session["config"].ToString(), Convert.ToInt32(Request.QueryString["id"]));
            }

            Session["DepositInvoicedetails"] = _dsDepNew;
            ViewState["Invoices"] = _dsDepNew.Tables[0];
            RadGrid_gvInvoiceDeposit.DataSource = _dsDepNew.Tables[0];// _dsDep;
            RadGrid_gvInvoiceDeposit.Rebind();

            RadGrid_gvDepositGL.DataSource = _dsDepNew.Tables[1];// _dsDep;
            RadGrid_gvDepositGL.Rebind();
            ViewState["GLTables"] = _dsDepNew.Tables[1];
            ViewState["GLTablesDelete"] = null;
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "showMessage", "noty({text: 'Deposit Updated Successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }

    }

    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["Deposit"];
            if (dt != null)
            {
                DataColumn[] keyColumns = new DataColumn[1];
                keyColumns[0] = dt.Columns["Ref"];
                dt.PrimaryKey = keyColumns;

                if (Request.QueryString["id"] != null)
                {
                    DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
                    int index = dt.Rows.IndexOf(d);

                    if (index > 0)
                    {
                        Response.Redirect("adddeposit.aspx?id=" + dt.Rows[index - 1]["Ref"]);
                    }
                    if (index == 0)
                    {
                        Response.Redirect("adddeposit.aspx?id=" + dt.Rows[dt.Rows.Count - 1]["Ref"]);
                    }
                }
                else
                {
                    DataRow d = dt.Rows.Find(dt.Rows.Count);
                    int index = dt.Rows.IndexOf(d);

                    Response.Redirect("adddeposit.aspx?id=" + dt.Rows[index]["Ref"]);
                }
            }
            
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["Deposit"];
            if (dt != null)
            {
                DataColumn[] keyColumns = new DataColumn[1];
                keyColumns[0] = dt.Columns["Ref"];
                dt.PrimaryKey = keyColumns;

                if (Request.QueryString["id"] != null)
                {
                    DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
                    int index = dt.Rows.IndexOf(d);
                    int c = dt.Rows.Count - 1;
                    if (index < c)
                    {
                        Response.Redirect("adddeposit.aspx?id=" + dt.Rows[index + 1]["Ref"]);
                    }
                    if (index == c)
                    {
                        Response.Redirect("adddeposit.aspx?id=" + dt.Rows[0]["Ref"]);
                    }
                }
                else
                {
                    Response.Redirect("adddeposit.aspx");
                }
            }
            
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    protected void lnkLast_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["Deposit"];
            if (dt != null)
            {
                Response.Redirect("adddeposit.aspx?id=" + dt.Rows[dt.Rows.Count - 1]["Ref"]);
            }
           
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["Deposit"];
            if (dt != null)
            {
                Response.Redirect("adddeposit.aspx?id=" + dt.Rows[0]["Ref"]);
            }
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["page"] != null)
        {
            if (Request.QueryString["page"].ToString() == "Collection")
            {
                Response.Redirect("iCollections.aspx");
            }
            else
            {
                if (Request.QueryString["page"].ToString() == "AccountLedger")
                {
                    Response.Redirect("AccountLedger?id=" + Session["alId"]);
                }
                else
                {
                    if (Request.QueryString["page"].ToString() == "addinvoice")
                    {
                        Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + Request.QueryString["invoiceId"].ToString());
                    }
                    else
                    {
                        if (Request.QueryString["page"].ToString() == "bankrecon")
                        {
                            Response.Redirect(Request.QueryString["page"].ToString() + ".aspx");
                        }
                        else
                        {
                            Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + Request.QueryString["lid"].ToString() + "&tab=inv");
                        }
                       
                    }
                }
                
            }   
        }       
        else
        {
            Response.Redirect("managedeposit.aspx");
        }

    }
    #endregion

    #region Custom Functions
    private void Permission()
    {

        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderCstm");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("cstmMgrSub");
        //ul.Style.Add("display", "block");
        //ul.Style.Add("visibility", "visible");

        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
            //Response.Redirect("addcustomer.aspx?uid=" + Session["userid"].ToString());
        }

        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx");
            //pnlGridButtons.Visible = false;
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }

        //DataTable dt = new DataTable();
        //dt = (DataTable)Session["userinfo"];

        //string ProgFunc = dt.Rows[0]["Control"].ToString().Substring(0, 1);
        //if (ProgFunc == "N")
        //{
        //    Response.Redirect("home.aspx");
        //}
    }
    private void CompanyPermission()
    {
        if (Session["COPer"].ToString() == "1")
        {
           
            //RadGrid_gvReceivePayment.Columns[4].Visible = true;
            RadGrid_gvReceivePayment.MasterTableView.GetColumn("Company").Display = true;
        }
        else
        {
          
            //RadGrid_gvReceivePayment.Columns[4].Visible = false;
            RadGrid_gvReceivePayment.MasterTableView.GetColumn("Company").Display = false;
            Session["CmpChkDefault"] = "2";
        }
    }
    private void SetDeposit(DataTable dt)
    {
        try
        {
            if (dt.Rows.Count > 0)
            {
                txtDateDeposite.Text = Convert.ToDateTime(dt.Rows[0]["fDate"]).ToString("MM/dd/yyyy");
                txtMemo.Text = dt.Rows[0]["fDesc"].ToString();
                lblDepositTotal.Text = string.Format("{0:c}", Convert.ToDouble(dt.Rows[0]["Amount"].ToString()));
                ddlBank.SelectedValue = Convert.ToInt32(dt.Rows[0]["Bank"]).ToString();
                hdnTransId.Value = dt.Rows[0]["TransID"].ToString();
            }

            _objReceivePay.ConnConfig = Session["config"].ToString();
            _GetReceivedPaymentByDep.ConnConfig = Session["config"].ToString();
            if (depIdVal == 0)
            {
                _objReceivePay.DepID = Convert.ToInt32(Request.QueryString["id"]);
                _GetReceivedPaymentByDep.DepID = Convert.ToInt32(Request.QueryString["id"]);
            }
            else
            {
                _objReceivePay.DepID = depIdVal;
                _GetReceivedPaymentByDep.DepID = depIdVal;
            }

            DataSet _dsDep = new DataSet();
            DataSet _dsDep1 = new DataSet();
            DataSet _dsDep2 = new DataSet();
            ListGetReceivedPaymentByDep _lstGetReceivedPaymentByDep = new ListGetReceivedPaymentByDep();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "MakeDepositAPI/AddDeposit_GetReceivedPaymentByDep";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetReceivedPaymentByDep);

                _lstGetReceivedPaymentByDep = (new JavaScriptSerializer()).Deserialize<ListGetReceivedPaymentByDep>(_APIResponse.ResponseData);

                _dsDep1 = _lstGetReceivedPaymentByDep.lstTable1.ToDataSet();
                _dsDep2 = _lstGetReceivedPaymentByDep.lstTable2.ToDataSet();

                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                dt1 = _dsDep1.Tables[0];
                dt2 = _dsDep2.Tables[0];

                dt1.TableName = "Table1";
                dt2.TableName = "Table2";

                _dsDep.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });
            }
            else
            {
                _dsDep = _objBL_Deposit.GetReceivedPaymentByDep(_objReceivePay);
            }

            Session["Depositdetails"] = _dsDep;
            
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    private void BindReceivedPaymentGrid()
    {
        try
        {
            DataSet _ds = new DataSet();
            _objReceivePay.ConnConfig = Session["config"].ToString();
            _objReceivePay.PaymentReceivedDate = Convert.ToDateTime(txtDateDeposite.Text);

            _GetAllReceivePaymentForDep.ConnConfig = Session["config"].ToString();
            _GetAllReceivePaymentForDep.PaymentReceivedDate = Convert.ToDateTime(txtDateDeposite.Text);

            List<GetAllReceivePaymentForDepViewModel> _lstGetAllReceivePaymentForDep = new List<GetAllReceivePaymentForDepViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "MakeDepositAPI/AddDeposit_GetAllReceivePaymentForDep";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetAllReceivePaymentForDep, true);

                _lstGetAllReceivePaymentForDep = (new JavaScriptSerializer()).Deserialize<List<GetAllReceivePaymentForDepViewModel>>(_APIResponse.ResponseData);
                _ds = CommonMethods.ToDataSet<GetAllReceivePaymentForDepViewModel>(_lstGetAllReceivePaymentForDep);
            }
            else
            {
                _ds = _objBL_Deposit.GetAllReceivePaymentForDep(_objReceivePay);
            }

            RadGrid_gvReceivePayment.VirtualItemCount = _ds.Tables[0].Rows.Count;
            RadGrid_gvReceivePayment.DataSource = _ds;
            RadGrid_gvReceivePayment.DataBind();
            lblRecordCount.Text = Convert.ToString(_ds.Tables[0].Rows.Count) + " Record(s) found";
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    private void FillPaymentMethod(DropDownList ddlPaymentMethod)
    {
        ddlPaymentMethod.Items.Add(new ListItem("Check", "0"));
        ddlPaymentMethod.Items.Add(new ListItem("Cash", "1"));
        ddlPaymentMethod.Items.Add(new ListItem("Wire Transfer", "2"));
        ddlPaymentMethod.Items.Add(new ListItem("ACH", "3"));
        ddlPaymentMethod.Items.Add(new ListItem("Credit Card", "4"));
        ddlPaymentMethod.Items.Add(new ListItem("e-Transfer", "5"));
    }
    private void FillBank()
    {
        try
        {
            _objBank.ConnConfig = Session["config"].ToString();

            _GetAllBankNames.ConnConfig = Session["config"].ToString();
            DataSet _dsBank = new DataSet();

            List<GetAllBankNamesViewModel> _lstGetAllBankNamesViewModel = new List<GetAllBankNamesViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "MakeDepositAPI/AddDeposit_GetAllBankNames";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetAllBankNames);

                _lstGetAllBankNamesViewModel = (new JavaScriptSerializer()).Deserialize<List<GetAllBankNamesViewModel>>(_APIResponse.ResponseData);
                _dsBank = CommonMethods.ToDataSet<GetAllBankNamesViewModel>(_lstGetAllBankNamesViewModel);
            }
            else
            {
                _dsBank = _objBL_Bank.GetAllBankNames(_objBank);
            }

            if (_dsBank.Tables[0].Rows.Count > 0)
            {
                ddlBank.Items.Add(new ListItem("Select", "0"));
                ddlBank.AppendDataBoundItems = true;

                ddlBank.DataSource = _dsBank;
                ddlBank.DataValueField = "ID";
                ddlBank.DataTextField = "fDesc";
                ddlBank.DataBind();
            }
            else
            {
                ddlBank.Items.Add(new ListItem("No data found", "0"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    private void ResetForm()
    {
        try
        {
            ResetFormControlValues(this);

            txtDateDeposite.Text = DateTime.Now.ToShortDateString();
            txtMemo.Text = "Deposit";
            lblDepositTotal.Text = string.Format("{0:c}", 0);
            BindReceivedPaymentGrid();
            RadGrid_gvReceivePayment.Rebind();
            RadGrid_gvDepositGL.DataSource = null;
            RadGrid_gvDepositGL.Rebind();
            //RadGrid_gvDeposit.DataSource = null;
            //RadGrid_gvDeposit.Rebind();

            //RadGrid_gvInvoiceDeposit.DataSource = null;
            //RadGrid_gvInvoiceDeposit.Rebind();
            divSuccess.Visible = false;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
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
                    case "System.Web.UI.WebControls.HiddenField":
                        ((HiddenField)c).Value = "";
                        break;
                }
            }
        }
    }
    //private bool ValidateGrid()
    //{
    //    bool _isValid = true;

    //    try
    //    {
    //        foreach (GridDataItem grDep in RadGrid_gvDeposit.Items)

    //        {
    //            TextBox txtDescription = (TextBox)grDep.FindControl("txtDescription");
    //            if (string.IsNullOrEmpty(txtDescription.Text))
    //            {
    //                _isValid = false;
    //            }
    //        }
          
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
    //    }
    //    return _isValid;
    //}
    private void EditDepositData()
    {
        try
        {
            _objDep.ConnConfig = Session["config"].ToString();
            _objDep.Ref = Convert.ToInt32(Request.QueryString["id"]);
            _objDep.fDate = Convert.ToDateTime(txtDateDeposite.Text);
            _objDep.Bank = Convert.ToInt32(ddlBank.SelectedValue);
            _objDep.fDesc = txtMemo.Text;
            depIdVal = Convert.ToInt32(Request.QueryString["id"]);
            _objDep.Amount = double.Parse(lblDepositTotal.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);

            //API
            _DepositInfor_UpdateDeposit.ConnConfig = Session["config"].ToString();
            _DepositInfor_UpdateDeposit.Ref = Convert.ToInt32(Request.QueryString["id"]);
            _DepositInfor_UpdateDeposit.fDate = Convert.ToDateTime(txtDateDeposite.Text);
            _DepositInfor_UpdateDeposit.Bank = Convert.ToInt32(ddlBank.SelectedValue);
            _DepositInfor_UpdateDeposit.fDesc = txtMemo.Text;
            _DepositInfor_UpdateDeposit.Amount = double.Parse(lblDepositTotal.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "MakeDepositAPI/AddDeposit_DepositInfor_UpdateDeposit";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _DepositInfor_UpdateDeposit);
            }
            else
            {
                _objBL_Deposit.UpdateDeposit(_objDep);
            }

            foreach (GridDataItem di in RadGrid_gvInvoiceDeposit.Items)
            {
                HiddenField hdnID = (HiddenField)di.FindControl("hdnID");
                _objReceivePay.ConnConfig = Session["config"].ToString();
                _objReceivePay.ID = Convert.ToInt32(hdnID.Value);
                _objReceivePay.Status = 1;

                _UpdateReceivedPayStatus.ConnConfig = Session["config"].ToString();
                _UpdateReceivedPayStatus.ID = Convert.ToInt32(hdnID.Value);
                _UpdateReceivedPayStatus.Status = 1;

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "MakeDepositAPI/AddDeposit_UpdateReceivedPayStatus";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateReceivedPayStatus);
                }
                else
                {
                    _objBL_Deposit.UpdateReceivedPayStatus(_objReceivePay);
                }
            }
            if (!string.IsNullOrEmpty(hdnTransId.Value.ToString()))
            {
                _objChart.ConnConfig = Session["config"].ToString();
                _objChart.Bank = Convert.ToInt32(ddlBank.SelectedValue);

                _GetBankAcctID.ConnConfig = Session["config"].ToString();
                _GetBankAcctID.Bank = Convert.ToInt32(ddlBank.SelectedValue);

                _objTrans.ConnConfig = Session["config"].ToString();
                _objTrans.ID = Convert.ToInt32(hdnTransId.Value);

                _UpdateDepositTransBank.ConnConfig = Session["config"].ToString();
                _UpdateDepositTransBank.ID = Convert.ToInt32(hdnTransId.Value);

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "MakeDepositAPI/AddDeposit_GetBankAcctID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetBankAcctID);
                    _UpdateDepositTransBank.Acct = Convert.ToInt32(_APIResponse.ResponseData);
                }
                else
                {
                    _objTrans.Acct = _objBL_Chart.GetBankAcctID(_objChart);
                }

                _objTrans.AcctSub = Convert.ToInt32(ddlBank.SelectedValue);
                _objTrans.TransDate = Convert.ToDateTime(txtDateDeposite.Text);

                _UpdateDepositTransBank.AcctSub = Convert.ToInt32(ddlBank.SelectedValue);
                _UpdateDepositTransBank.TransDate = Convert.ToDateTime(txtDateDeposite.Text);

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "MakeDepositAPI/AddDeposit_UpdateDepositTransBank";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateDepositTransBank);
                }
                else
                {
                    _objBL_Deposit.UpdateDepositTransBank(_objTrans);
                }
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }

    }
    
    private int AddDepositData()
    {
        int dep = 0;
        try
        {
            #region add deposit           
            _objDep.ConnConfig = Session["config"].ToString();
            _objDep.fDate = Convert.ToDateTime(txtDateDeposite.Text);
            _objDep.Bank = Convert.ToInt32(ddlBank.SelectedValue);
            _objDep.fDesc = txtMemo.Text; 
            _objDep.Amount = double.Parse(hdDepositTotal.Value.Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);

            //API
            _AddDepositWithGL.ConnConfig = Session["config"].ToString();
            _AddDepositWithGL.fDate = Convert.ToDateTime(txtDateDeposite.Text);
            _AddDepositWithGL.Bank = Convert.ToInt32(ddlBank.SelectedValue);
            _AddDepositWithGL.fDesc = txtMemo.Text;
            _AddDepositWithGL.Amount = double.Parse(hdDepositTotal.Value.Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);

            DataTable dtReceipt = new DataTable();
            dtReceipt.Columns.Add("ID", typeof(int));
            dtReceipt.Columns.Add("Amount", typeof(double));
            dtReceipt.Columns.Add("Description", typeof(String));

            foreach (GridDataItem gr in RadGrid_gvReceivePayment.Items)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                if (chkSelect.Checked.Equals(true))
                {
                    HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");
                    Label lblAmount = (Label)gr.FindControl("lblAmount");
                    DataRow dr = dtReceipt.NewRow();                   
                    dr["ID"] = hdnID.Value;                  
                    dr["Amount"] = double.Parse(lblAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                NumberStyles.AllowThousands |
                                NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                    dr["Description"] = "";
                    dtReceipt.Rows.Add(dr);                    
                }
            }
            _objDep._dtReceipt = dtReceipt;

            _AddDepositWithGL._dtReceipt = dtReceipt;

            DataTable dtGLAccount = new DataTable();
            dtGLAccount.Columns.Add("ID", typeof(int));
            dtGLAccount.Columns.Add("Amount", typeof(double));
            dtGLAccount.Columns.Add("Description", typeof(String));
            foreach (GridDataItem gr in RadGrid_gvDepositGL.Items)
            {                
                    HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");
                    TextBox txtGLAmount = (TextBox)gr.FindControl("txtGLAmount");
                    TextBox txtDesc = (TextBox)gr.FindControl("txtDesc");
                    TextBox txtRef = (TextBox)gr.FindControl("txtRef");
                    DataRow dr = dtGLAccount.NewRow();
                    if (hdnID.Value != "")
                    {
                        dr["ID"] = hdnID.Value;
                        dr["Amount"] = (-1) * double.Parse(txtGLAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                    NumberStyles.AllowThousands |
                                    NumberStyles.AllowDecimalPoint|NumberStyles.AllowLeadingSign);
                        dr["Description"] = txtDesc.Text + " " + txtRef.Text;
                        dtGLAccount.Rows.Add(dr);
                    }
                    
               
            }
            _objDep._dtGlAccount = dtGLAccount;

            _AddDepositWithGL._dtGlAccount = dtGLAccount;

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "MakeDepositAPI/AddDeposit_AddDepositWithGL";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddDepositWithGL);

                object JsonData = JsonConvert.DeserializeObject(_APIResponse.ResponseData);
                dep = Convert.ToInt32(JsonData.ToString());
            }
            else
            {
                dep = _objBL_Deposit.AddDepositWithGL(_objDep);
            }

            #endregion

         
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
        return dep;
    }

    protected void gvDeposit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                GridViewRow row = e.Row;

                //DropDownList ddlPaymentMethod = (e.Row.FindControl("ddlPaymentMethod") as DropDownList);
                //FillPaymentMethod(ddlPaymentMethod);

                //GridViewRow row = gvDeposit.Rows[index];
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                AsyncPostBackTrigger trigger = new AsyncPostBackTrigger();
                trigger.ControlID = chkSelect.ID;
                trigger.EventName = "CheckedChanged";
                //uPnlDeposit.Triggers.Add(trigger);  Anmol
                break;

        }
    }
    private void UpdateChartBalance()
    {
        try
        {
            _objChart.ConnConfig = Session["config"].ToString();
            _objChart.ID = _objTrans.Acct;
            _objChart.Amount = _objTrans.Amount;

            _UpdateChartBalance.ConnConfig = Session["config"].ToString();
            _UpdateChartBalance.ID = _objTrans.Acct;
            _UpdateChartBalance.Amount = _objTrans.Amount;

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "MakeDepositAPI/AddDeposit_UpdateChartBalance";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateChartBalance);
            }
            else
            {
                _objBL_Chart.UpdateChartBalance(_objChart);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    private void GetPeriodDetails(DateTime _transDate)
    {
        bool _flag = CommonHelper.GetPeriodDetails(_transDate);
        ViewState["FlagPeriodClose"] = _flag;
        if (!_flag)
        {
            divSuccess.Visible = true;
            btnSubmit.Visible = false;
            hdnIsRecon.Value = "1";
            RadGrid_gvInvoiceDeposit.Enabled = false;
            RadGrid_gvDepositGL.Enabled = false;
            RadGrid_gvReceivePayment.Enabled = false;
            if (Request.QueryString["id"] == null)
            {
                lnkPrint.Visible = false;
            }
        }
        else
        {
            divSuccess.Visible = false;
            //btnSubmit.Visible = true;
            //hdnIsRecon.Value = "0";
            //RadGrid_gvInvoiceDeposit.Enabled = true;
            //RadGrid_gvDepositGL.Enabled = true;
            //RadGrid_gvReceivePayment.Enabled = true;
            //if(Request.QueryString["id"] == null){
            //    lnkPrint.Visible = true;
            //}
            if (hdnIsRecon.Value == "1")
            {
                btnSubmit.Visible = false;
                RadGrid_gvInvoiceDeposit.Enabled = false;
                RadGrid_gvDepositGL.Enabled = false;
                RadGrid_gvReceivePayment.Enabled = false;
                hdnIsRecon.Value = "1";
                imgCleared.Visible = true;
            }
        }
    }
    private void userpermissions()
    {
        if (Session["type"].ToString() != "c")
        {
            if (Session["type"].ToString() != "am")
            {
                _objPropUser.ConnConfig = Session["config"].ToString();
                _objPropUser.Username = Session["username"].ToString();
                _objPropUser.PageName = "adddeposit.aspx";

                _GetScreensByUser.ConnConfig = Session["config"].ToString();
                _GetScreensByUser.Username = Session["username"].ToString();
                _GetScreensByUser.PageName = "adddeposit.aspx";

                DataSet dspage = new DataSet();
                List<GetScreensByUserViewModel> _lstGetScreensByUser = new List<GetScreensByUserViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "MakeDepositAPI/AddDeposit_GetScreensByUser";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetScreensByUser);

                    _lstGetScreensByUser = (new JavaScriptSerializer()).Deserialize<List<GetScreensByUserViewModel>>(_APIResponse.ResponseData);
                    dspage = CommonMethods.ToDataSet<GetScreensByUserViewModel>(_lstGetScreensByUser);
                }
                else
                {
                    dspage = _objBLUser.getScreensByUser(_objPropUser);
                }

                if (dspage.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dspage.Tables[0].Rows[0]["access"].ToString()) == false)
                    {
                        Response.Redirect("home.aspx");
                    }
                }
                else
                {
                    Response.Redirect("home.aspx");
                }
            }
        }
    }
    public bool CheckAddEditPermission()
    {
        bool result = true;
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = GetUserById();
            /// Location ///////////////////------->

            //Location

            string DepositPermission = ds.Rows[0]["Deposit"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Deposit"].ToString();
            string stAddeDeposit = DepositPermission.Length < 1 ? "Y" : DepositPermission.Substring(0, 1);
            string stEditDeposit = DepositPermission.Length < 2 ? "Y" : DepositPermission.Substring(1, 1);
            string stDeleteDeposit = DepositPermission.Length < 3 ? "Y" : DepositPermission.Substring(2, 1);
            string stViewDeposit = DepositPermission.Length < 4 ? "Y" : DepositPermission.Substring(3, 1);


            if (stViewDeposit == "N")
            {
                result = false;
            }
            else if (Request.QueryString["id"] == null)
            {
                if (stAddeDeposit == "N")
                {
                    result = false;
                }
            }
            else if (stEditDeposit == "N")
            {
                if (stViewDeposit == "Y")
                {
                    btnSubmit.Visible = false;
                    hdnIsRecon.Value = "1";
                    RadGrid_gvInvoiceDeposit.Enabled = false;
                    RadGrid_gvDepositGL.Enabled = false;
                    RadGrid_gvReceivePayment.Enabled = false;
                }
                else
                {
                    result = false;
                }
            }
        }

        return result;
    }
    private DataTable GetUserById()
    {
        User objPropUser = new User();
        objPropUser.TypeID = Convert.ToInt32(Session["usertypeid"]);
        objPropUser.UserID = Convert.ToInt32(Session["userid"]);
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.DBName = Session["dbname"].ToString();
        DataSet ds = new DataSet();
        ds = _objBLUser.GetUserPermissionByUserID(objPropUser);
        return ds.Tables[0];
    }
    private bool IsValidDate()
    {
        DateTime dateValue;
        string[] formats = {"M/d/yyyy", "M/d/yyyy",
                "MM/dd/yyyy", "M/d/yyyy",
                "M/d/yyyy", "M/d/yyyy",
                "M/d/yyyy", "M/d/yyyy",
                "MM/dd/yyyy", "M/dd/yyyy"};
        var dt = DateTime.TryParseExact(txtDateDeposite.Text.ToString(), formats,
                            new CultureInfo("en-US"),
                            DateTimeStyles.None,
                            out dateValue);

        if (dt)
        {
            return true;
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "noty({text: 'Please enter valid date.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return false;
        }
    }

    //protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    //{
    //    CheckBox ChkAll = (CheckBox)sender;
    //    //CheckBox ChkAll = (CheckBox)RadGrid_gvReceivePayment.MasterTableView.Items["ChkAll"].FindControl("ChkAll");
    //    foreach (GridDataItem row in RadGrid_gvReceivePayment.Items)
    //    {

    //        CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
    //        if (ChkAll.Checked.Equals(true))
    //        {
    //            chkSelect.Checked = true;
    //            chkSelect_CheckedChanged(sender, e);
    //        }
    //        else
    //        {
    //            chkSelect.Checked = false;
    //            chkSelect_CheckedChanged(sender, e);

    //        }
    //    }
    //}

    #endregion
    protected void lnkPrint_Click(object sender, EventArgs e)
    {
        Boolean chkCount = false;

        #region Validation Check
        if (Request.QueryString["id"] != null)
        {
            chkCount = true;
            depIdVal = Convert.ToInt32(Request.QueryString["id"]);
        }
        else
        {
            foreach (GridDataItem gr in RadGrid_gvReceivePayment.Items)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                if (chkSelect.Checked.Equals(true))
                {
                    chkCount = true;
                }
            }
        }
           
        #endregion

        //if (RadGrid_gvDeposit.Items.Count > 0)
        if (chkCount == true)
        {
            if (ddlBank.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Please select a bank ', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
            else
            {
                submit();
                _objDep.Ref = depIdVal;
                _objDep.ConnConfig= Session["config"].ToString();

                _GetDepHeadByID.Ref = depIdVal;
                _GetDepHeadByID.ConnConfig = Session["config"].ToString();

                _GetDepByID.Ref = depIdVal;
                _GetDepByID.ConnConfig = Session["config"].ToString();
                DataSet _dsDep = new DataSet();
                List<GetDepByIDViewModel> _lstGetDepByID = new List<GetDepByIDViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "MakeDepositAPI/AddDeposit_GetDepByID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetDepByID);

                    _lstGetDepByID = (new JavaScriptSerializer()).Deserialize<List<GetDepByIDViewModel>>(_APIResponse.ResponseData);
                    _dsDep = CommonMethods.ToDataSet<GetDepByIDViewModel>(_lstGetDepByID);
                }
                else
                {
                    _dsDep = _objBL_Deposit.GetDepByID(_objDep);
                }

                DataSet _sessionds = new DataSet();
                List<GetDepHeadByIDViewModel> _lstGetDepHeadByID = new List<GetDepHeadByIDViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "MakeDepositAPI/AddDeposit_GetDepHeadByID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetDepHeadByID);

                    _lstGetDepHeadByID = (new JavaScriptSerializer()).Deserialize<List<GetDepHeadByIDViewModel>>(_APIResponse.ResponseData);
                    _sessionds = CommonMethods.ToDataSet<GetDepHeadByIDViewModel>(_lstGetDepHeadByID);
                }
                else
                {
                    _sessionds = _objBL_Deposit.GetDepHeadByID(_objDep);
                }

                Session["DepositHead"] = _sessionds;
                Session["RefNo"] = depIdVal.ToString();
                if (_dsDep.Tables[0].Columns.Contains("Ref"))
                {
                    SetDeposit(_dsDep.Tables[0]);
                    //gvDeposit.Columns[5].Visible = false;
                    // Response.Redirect("DepositReport.aspx");
                    Response.Redirect("DepositSlipReport.aspx?uid=" + depIdVal);
                }

            }

        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Please select an item ', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    protected void submit()
    {
        try
        {
            if (hdnIsRecon.Value != "1")
            {
                if (IsValidDate())
                {
                    bool Flag = false;

                    if (Request.QueryString["id"] != null)   // EDIT
                    {
                        Flag = (bool)ViewState["FlagPeriodClose"];
                        if (Flag)
                        {
                            #region EDIT

                            //EditDepositData();
                            UpdateDepositInfor();
                            doEditDeposit();
                            //Response.Redirect("managedeposit.aspx", false);
                            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Deposit Updated Successfully! <BR/>Ref# " + Request.QueryString["id"] + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                            #endregion
                        }
                        if (!Flag)
                        {
                            divSuccess.Visible = true;
                        }
                        else
                        {
                            divSuccess.Visible = false;
                        }
                    }
                    else //ADD
                    {
                        GetPeriodDetails(Convert.ToDateTime(txtDateDeposite.Text));     //Check period closed out permission
                        Flag = (bool)ViewState["FlagPeriodClose"];

                        if (Flag)
                        {
                            #region ADD

                            int depId = AddDepositData();
                            depIdVal = depId;
                            ResetForm();
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "showMessage", "noty({text: 'Deposit Added Successfully! <BR/>Ref# " + depId + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                            #endregion
                        }

                    }
                }
            }
           
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    protected void txtDate_TextChanged(object sender, EventArgs e)
    {
        BindReceivedPaymentGrid();
        RadGrid_gvReceivePayment.Rebind();
    }

    protected void btnDateDeposite_Click(object sender, EventArgs e)
    {
        BindReceivedPaymentGrid();
        RadGrid_gvReceivePayment.Rebind();
    }

    protected void RadGrid_gvReceivePayment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_gvReceivePayment.AllowCustomPaging = !ShouldApplySortFilterOrGroup();

        DataSet _ds = new DataSet();
        _objReceivePay.ConnConfig = Session["config"].ToString();
        _objReceivePay.PaymentReceivedDate = Convert.ToDateTime(txtDateDeposite.Text);

        _GetAllReceivePaymentForDep.ConnConfig = Session["config"].ToString();
        _GetAllReceivePaymentForDep.PaymentReceivedDate = Convert.ToDateTime(txtDateDeposite.Text);

        List<GetAllReceivePaymentForDepViewModel> _lstGetAllReceivePaymentForDep = new List<GetAllReceivePaymentForDepViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "MakeDepositAPI/AddDeposit_GetAllReceivePaymentForDep";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetAllReceivePaymentForDep, true);

            _lstGetAllReceivePaymentForDep = (new JavaScriptSerializer()).Deserialize<List<GetAllReceivePaymentForDepViewModel>>(_APIResponse.ResponseData);
            _ds = CommonMethods.ToDataSet<GetAllReceivePaymentForDepViewModel>(_lstGetAllReceivePaymentForDep);
        }
        else
        {
            _ds = _objBL_Deposit.GetAllReceivePaymentForDep(_objReceivePay);
        }

        RadGrid_gvReceivePayment.VirtualItemCount = _ds.Tables[0].Rows.Count;
        RadGrid_gvReceivePayment.DataSource = _ds;
        lblRecordCount.Text = Convert.ToString(_ds.Tables[0].Rows.Count) + " Record(s) found";
    }

    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_gvReceivePayment.MasterTableView.FilterExpression != "" ||
            (RadGrid_gvReceivePayment.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_gvReceivePayment.MasterTableView.SortExpressions.Count > 0;
    }

    #region 'Add New row'
    private void getDataOnInvoiceDepositGrid()
    {      
        try
        {
            if (ViewState["Invoices"] != null)
            {
                DataTable dt = (DataTable)ViewState["Invoices"];
                DataTable dtInvoice = dt.Clone();
                foreach (GridDataItem gr in RadGrid_gvInvoiceDeposit.Items)                {                

                    HiddenField hdnOwner = (HiddenField)gr.FindControl("hdnOwner");
                    HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");
                    TextBox txtInvoiceID = (TextBox)gr.FindControl("txtInvoiceID");
                    HiddenField hdnRol = (HiddenField)gr.FindControl("hdnRol");
                    TextBox txtCustomerName = (TextBox)gr.FindControl("txtCustomerName");
                    HiddenField hdnLoc = (HiddenField)gr.FindControl("hdnLoc");
                    TextBox txtTag = (TextBox)gr.FindControl("txtLocationName");
                    HiddenField hdnEn = (HiddenField)gr.FindControl("hdnEn");
                    HiddenField hdnCompany = (HiddenField)gr.FindControl("hdnCompany");
                    TextBox txtAmount = (TextBox)gr.FindControl("txtAmount");
                    TextBox txtPaymentReceivedDate = (TextBox)gr.FindControl("txtPaymentReceivedDate");
                    TextBox txtDesc = (TextBox)gr.FindControl("txtDesc");
                    DropDownList ddlPaymentMethod = (DropDownList)gr.FindControl("ddlPaymentMethod");
                    TextBox txtCheckNumber = (TextBox)gr.FindControl("txtCheckNumber");
                    HiddenField hdnAmountDue = (HiddenField)gr.FindControl("hdnAmountDue");
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkInvSelect");
                    HiddenField hdnOrderNo = (HiddenField)gr.FindControl("hdnOrderNo");
                    HiddenField hdnRefTranID = (HiddenField)gr.FindControl("hdnRefTranID");

                    DataRow dr = dtInvoice.NewRow();
                    dr["Owner"] = hdnOwner.Value;
                    dr["ID"] = hdnID.Value;
                    dr["InvoiceID"] = txtInvoiceID.Text==""?0:Convert.ToInt32(txtInvoiceID.Text);
                    dr["Rol"] = hdnRol.Value;

                    dr["customerName"] = txtCustomerName.Text;
                    dr["loc"] = hdnLoc.Value;
                    dr["Tag"] = txtTag.Text;
                    dr["En"] = hdnEn.Value;
                    dr["Company"] = hdnCompany.Value;

                    dr["Amount"] = double.Parse(txtAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                 NumberStyles.AllowThousands |
                                 NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign); 
                    dr["PaymentReceivedDate"] = txtPaymentReceivedDate.Text;
                    dr["fDesc"] = txtDesc.Text;
                    dr["PaymentMethod"] = ddlPaymentMethod.SelectedItem.Text;
                    dr["PaymentMethodID"] = ddlPaymentMethod.SelectedValue;
                    dr["CheckNumber"] = txtCheckNumber.Text;
                    
                    dr["AmountDue"] = double.Parse(hdnAmountDue.Value.Replace('$', '0'), NumberStyles.AllowParentheses |
                                 NumberStyles.AllowThousands |
                                 NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                    if (dr["AmountDue"].ToString()=="0")
                    {
                        dr["AmountDue"] = dr["Amount"];
                    }
                        dr["OrderNo"] = hdnOrderNo.Value;                         
                    dr["isChecked"] = chkSelect.Checked;
                    if (Convert.ToInt32(hdnOwner.Value) != 0)
                    {
                        dtInvoice.Rows.Add(dr);
                    }
                    dr["RefTranID"] = hdnRefTranID.Value == "" ? 0 : Convert.ToInt32(hdnRefTranID.Value);
                }
                ViewState["Invoices"] = dtInvoice;
            }

        }
        catch (Exception ex)
        {
            string str =ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);         
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    private void BindInvoiceDepositGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["Invoices"];
            RadGrid_gvInvoiceDeposit.DataSource = dt;
            RadGrid_gvInvoiceDeposit.VirtualItemCount = dt.Rows.Count;
            RadGrid_gvInvoiceDeposit.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkAddnewRow_Click(object sender, EventArgs e)
    {
        getDataOnInvoiceDepositGrid();
        DataTable dt = new DataTable();
        if (ViewState["Invoices"] != null)
        {
            dt = (DataTable)ViewState["Invoices"];
        }
        else
        {
            dt.Columns.Add("Owner", typeof(int));
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("InvoiceID", typeof(int));
            dt.Columns.Add("Rol", typeof(int));
            dt.Columns.Add("customerName", typeof(String));
            dt.Columns.Add("loc", typeof(int));
            dt.Columns.Add("Tag", typeof(String));
            dt.Columns.Add("En", typeof(int));
            dt.Columns.Add("Company", typeof(String));
            dt.Columns.Add("Amount", typeof(double));
            dt.Columns.Add("PaymentReceivedDate", typeof(DateTime));
            dt.Columns.Add("fDesc", typeof(String));
            dt.Columns.Add("PaymentMethod", typeof(String));
            dt.Columns.Add("PaymentMethodID", typeof(String));
            dt.Columns.Add("CheckNumber", typeof(String));
            dt.Columns.Add("AmountDue", typeof(double));         
          
            dt.Columns.Add("OrderNo", typeof(int));   
            dt.Columns.Add("isChecked", typeof(int));
            dt.Columns.Add("RefTranID", typeof(int));

        }
        DataRow dr = dt.NewRow();      
        dr["Owner"] = 0;
        dr["ID"] = 0;
        dr["InvoiceID"] = 0;
        dr["Rol"] = 0;
        dr["customerName"] = DBNull.Value;
        dr["loc"] = 0;
        dr["Tag"] = DBNull.Value;
        dr["En"] = 0;
        dr["Company"] = DBNull.Value;
        dr["Amount"] = 0;
        dr["PaymentReceivedDate"] = DateTime.Now;
        dr["fDesc"] = DBNull.Value;
        dr["PaymentMethod"] = "Check";
        dr["PaymentMethodID"] = "0";
        dr["CheckNumber"] = DBNull.Value;
        dr["AmountDue"] = 0;
        dr["OrderNo"] = 0;
        dr["isChecked"] = 1;
        dr["RefTranID"] = 0;
        dt.Rows.Add(dr);
        ViewState["Invoices"] = dt;
        BindInvoiceDepositGrid();
    }
   

    #endregion

    protected void RadGrid_gvInvoiceDeposit_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;           
            DropDownList ddlPaymentMethod = (DropDownList)item.FindControl("ddlPaymentMethod");
           if (ddlPaymentMethod != null)
            {
                ddlPaymentMethod.Items.Add(new ListItem("Check", "0"));
                ddlPaymentMethod.Items.Add(new ListItem("Cash", "1"));
                ddlPaymentMethod.Items.Add(new ListItem("Wire Transfer", "2"));
                ddlPaymentMethod.Items.Add(new ListItem("ACH", "3"));
                ddlPaymentMethod.Items.Add(new ListItem("Credit Card", "4"));
                ddlPaymentMethod.Items.Add(new ListItem("e-Transfer", "5"));
                ddlPaymentMethod.Items.Add(new ListItem("Lockbox", "6"));
                ddlPaymentMethod.SelectedValue = DataBinder.Eval(item.DataItem, "PaymentMethodID").ToString();
                ddlPaymentMethod.Items.FindByValue(DataBinder.Eval(item.DataItem, "PaymentMethodID").ToString()).Selected = true;
            }           

        }        
    }

    private DataTable CreateInvoiceTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Owner", typeof(int));
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("InvoiceID", typeof(int));
        dt.Columns.Add("Rol", typeof(int));
        dt.Columns.Add("customerName", typeof(String));
        dt.Columns.Add("loc", typeof(int));
        dt.Columns.Add("Tag", typeof(String));
        dt.Columns.Add("En", typeof(int));
        dt.Columns.Add("Company", typeof(String));
        dt.Columns.Add("Amount", typeof(double));
        dt.Columns.Add("PaymentReceivedDate", typeof(DateTime));
        dt.Columns.Add("fDesc", typeof(String));
        dt.Columns.Add("PaymentMethod", typeof(String));
        dt.Columns.Add("CheckNumber", typeof(String));
        dt.Columns.Add("AmountDue", typeof(double));
        dt.Columns.Add("RefTranID", typeof(int));
        return dt;
    }

    //API
    private DataTable InvoiceEmptyTablesDelete()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Owner", typeof(int));
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("InvoiceID", typeof(int));
        dt.Columns.Add("Rol", typeof(int));
        dt.Columns.Add("customerName", typeof(String));
        dt.Columns.Add("loc", typeof(int));
        dt.Columns.Add("Tag", typeof(String));
        dt.Columns.Add("En", typeof(int));
        dt.Columns.Add("Company", typeof(String));
        dt.Columns.Add("Amount", typeof(double));
        dt.Columns.Add("PaymentReceivedDate", typeof(DateTime));
        dt.Columns.Add("fDesc", typeof(String));
        dt.Columns.Add("PaymentMethod", typeof(String));
        dt.Columns.Add("CheckNumber", typeof(String));
        dt.Columns.Add("AmountDue", typeof(double));
        dt.Columns.Add("RefTranID", typeof(int));
        DataRow dr = dt.NewRow();
        dr["Owner"] = "0";
        dr["ID"] = "0";
        dr["InvoiceID"] = "0";
        dr["Rol"] = "0";
        dr["customerName"] = "";
        dr["loc"] = "0";
        dr["Tag"] = "";
        dr["En"] = "0";
        dr["Company"] = "";
        dr["Amount"] = "0.00";
        dr["PaymentReceivedDate"] = DBNull.Value;
        dr["fDesc"] = "";
        dr["PaymentMethod"] = "";
        dr["CheckNumber"] = "";
        dr["AmountDue"] = "0.00";
        dr["RefTranID"] = "0";
        dt.Rows.Add(dr);

        return dt;
    }

    //API
    private DataTable GLEmptyTablesDelete()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("Amount", typeof(double));
        dt.Columns.Add("Description", typeof(String));

        DataRow dr = dt.NewRow();
        dr["ID"] = "0";
        dr["Amount"] = "0.00";
        dr["Description"] = "";

        dt.Rows.Add(dr);

        return dt;
    }


    protected void RadGrid_gvInvoiceDeposit_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (Request.QueryString["id"] != null)
        {
            DataSet _dsDep = new DataSet();
            DataSet _dsDep1 = new DataSet();
            DataSet _dsDep2 = new DataSet();
            ListGetAllInvoiceByDep _lstGetAllInvoiceByDep = new ListGetAllInvoiceByDep();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "MakeDepositAPI/AddDeposit_GetAllInvoiceByDep";

                _GetAllInvoiceByDep.ConnConfig = Session["config"].ToString();
                _GetAllInvoiceByDep.depId = Convert.ToInt32(Request.QueryString["id"]);

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetAllInvoiceByDep);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetAllInvoiceByDep = serializer.Deserialize<ListGetAllInvoiceByDep>(_APIResponse.ResponseData);

                _dsDep1 = _lstGetAllInvoiceByDep.lstTable1.ToDataSet();
                _dsDep2 = _lstGetAllInvoiceByDep.lstTable2.ToDataSet();

                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                dt1 = _dsDep1.Tables[0];
                dt2 = _dsDep2.Tables[0];

                dt1.TableName = "Table1";
                dt2.TableName = "Table2";

                _dsDep.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });
            }
            else
            {
                _dsDep = _objBL_Deposit.GetAllInvoiceByDep(Session["config"].ToString(), Convert.ToInt32(Request.QueryString["id"]));
            }

            Session["DepositInvoicedetails"] = _dsDep;
            ViewState["Invoices"] = _dsDep.Tables[0];
            RadGrid_gvInvoiceDeposit.DataSource = _dsDep.Tables[0];// _dsDep;
        }
        else
        {
            RadGrid_gvInvoiceDeposit.DataSource = null;
        }
    }

    #region 'GL Account'
    private DataTable CreateGLTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Acct", typeof(String));
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("TransID", typeof(int));
        dt.Columns.Add("fTitle", typeof(String));
        dt.Columns.Add("fDesc", typeof(String));
        dt.Columns.Add("Ref", typeof(String));
        dt.Columns.Add("Amount", typeof(double));
        dt.Columns.Add("OrderNo", typeof(int));
        dt.Columns.Add("isChecked", typeof(int));

        return dt;
    }
    private void getDataOnGLGrid()
    {
        try
        {
            if (ViewState["GLTables"] != null)
            {
                DataTable dt = (DataTable)ViewState["GLTables"];
                DataTable dtGLAccount = dt.Clone();
                foreach (GridDataItem gr in RadGrid_gvDepositGL.Items)
                {
                    TextBox txtAcct = (TextBox)gr.FindControl("txtAcct");
                    HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");
                    HiddenField hdnTransID = (HiddenField)gr.FindControl("hdnTransID");
                    HiddenField hdnTitle = (HiddenField)gr.FindControl("hdnTitle");
                    TextBox txtDesc = (TextBox)gr.FindControl("txtDesc");
                    TextBox txtRef = (TextBox)gr.FindControl("txtRef");                   
                    TextBox txtGLAmount = (TextBox)gr.FindControl("txtGLAmount");              
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                    HiddenField hdnOrderNo = (HiddenField)gr.FindControl("hdnOrderNo");
                    DataRow dr = dtGLAccount.NewRow();
                    dr["Acct"] = txtAcct.Text;
                    dr["ID"] = hdnID.Value;
                    dr["TransID"] = hdnTransID.Value;
                    dr["fTitle"] = hdnTitle.Value;
                    dr["fDesc"] = txtDesc.Text;
                    dr["Ref"] = txtRef.Text;
                    dr["Amount"] = double.Parse(txtGLAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
              NumberStyles.AllowThousands |
              NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);

                    
                    dr["OrderNo"] = hdnOrderNo.Value;
                    dr["isChecked"] = chkSelect.Checked;

                    dtGLAccount.Rows.Add(dr);
                }
                ViewState["GLTables"] = dtGLAccount;
            }


        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void BindGLGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["GLTables"];
            RadGrid_gvDepositGL.DataSource = dt;
            RadGrid_gvDepositGL.VirtualItemCount = dt.Rows.Count;
            RadGrid_gvDepositGL.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void RadGrid_gvDepositGL_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (Request.QueryString["id"] != null)
        {

            DataSet _dsDep = new DataSet();
            DataSet _dsDep1 = new DataSet();
            DataSet _dsDep2 = new DataSet();
            ListGetAllInvoiceByDep _lstGetAllInvoiceByDep = new ListGetAllInvoiceByDep();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "MakeDepositAPI/AddDeposit_GetAllInvoiceByDep";

                _GetAllInvoiceByDep.ConnConfig = Session["config"].ToString();
                _GetAllInvoiceByDep.depId = Convert.ToInt32(Request.QueryString["id"]);

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetAllInvoiceByDep);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetAllInvoiceByDep = serializer.Deserialize<ListGetAllInvoiceByDep>(_APIResponse.ResponseData);

                _dsDep1 = _lstGetAllInvoiceByDep.lstTable1.ToDataSet();
                _dsDep2 = _lstGetAllInvoiceByDep.lstTable2.ToDataSet();

                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                dt1 = _dsDep1.Tables[0];
                dt2 = _dsDep2.Tables[0];

                dt1.TableName = "Table1";
                dt2.TableName = "Table2";

                _dsDep.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });
            }
            else
            {
                _dsDep = _objBL_Deposit.GetAllInvoiceByDep(Session["config"].ToString(), Convert.ToInt32(Request.QueryString["id"]));
            }


            if (_dsDep.Tables[1].Rows.Count > 0)
            {
                RadGrid_gvDepositGL.DataSource = _dsDep.Tables[1];// _dsDep;
                ViewState["GLTables"] = _dsDep.Tables[1];
                hdnAreaActive.Value = "0";
            }
            else
            {
                DataTable dt = CreateGLTable();
                DataRow dr = dt.NewRow();
                dr["Acct"] = DBNull.Value;
                dr["ID"] = DBNull.Value;
                dr["TransID"] = 0;
                dr["fTitle"] = "";
                dr["fDesc"] = "";
                dr["Ref"] = "";
                dr["Amount"] = 0;
                dr["OrderNo"] = 0;
                dr["isChecked"] = 1;

                dt.Rows.Add(dr);
                ViewState["GLTables"] = dt;
                RadGrid_gvDepositGL.DataSource = dt;
              
            }

        }
        else
        {
            DataTable dt = CreateGLTable();
            DataRow dr = dt.NewRow();
            dr["Acct"] = DBNull.Value;
            dr["ID"] = DBNull.Value;
            dr["TransID"] = 0;
            dr["fTitle"] = "";
            dr["fDesc"] = "";
            dr["Ref"] = "";
            dr["Amount"] = 0;
            dr["OrderNo"] = 0;
            dr["isChecked"] = 1;

            dt.Rows.Add(dr);
            ViewState["GLTables"] = dt;
            RadGrid_gvDepositGL.DataSource = dt;
          
        }


    }

    protected void lnkGLAddnewRow_Click(object sender, EventArgs e)
    {
        getDataOnGLGrid();
        DataTable dt = new DataTable();
        if (ViewState["GLTables"] != null)
        {
            dt = (DataTable)ViewState["GLTables"];
        }
        else
        {
            dt.Columns.Add("Acct", typeof(String));
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("TransID", typeof(int));
            dt.Columns.Add("fTitle", typeof(String));
            dt.Columns.Add("fDesc", typeof(String));
            dt.Columns.Add("Ref", typeof(String));
            dt.Columns.Add("Amount", typeof(double));

            dt.Columns.Add("OrderNo", typeof(int));
            dt.Columns.Add("isChecked", typeof(int));

        }
        DataRow dr = dt.NewRow();
        dr["Acct"] = DBNull.Value;
        dr["ID"] = DBNull.Value;
        dr["TransID"] = 0;
        dr["fTitle"] = "";
        dr["fDesc"] = "";
        dr["Ref"] = "";
        dr["Amount"] = 0;
        dr["OrderNo"] = 0;
        dr["isChecked"] = 1;

        dt.Rows.Add(dr);
        ViewState["GLTables"] = dt;
        BindGLGrid();
    }


    protected void ibtnDeleteGLRow_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable tblDeleteGL = new DataTable();
            if (ViewState["GLTablesDelete"] != null)
            {
                tblDeleteGL = (DataTable)ViewState["GLTablesDelete"];
            }
            else
            {

                tblDeleteGL.Columns.Add("ID", typeof(int));
                tblDeleteGL.Columns.Add("Amount", typeof(double));
                tblDeleteGL.Columns.Add("Description", typeof(String));

            }
            if (ViewState["GLTables"] != null)
            {
                getDataOnGLGrid();

                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["GLTables"];
                int count = 0;
                foreach (GridDataItem gr in RadGrid_gvDepositGL.Items)
                {

                    HiddenField hdnTransID = (HiddenField)gr.FindControl("hdnTransID");
                    TextBox txtDesc = (TextBox)gr.FindControl("txtDesc");
                    TextBox txtGLAmount = (TextBox)gr.FindControl("txtGLAmount");
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");


                    if (chkSelect.Checked)
                    {
                        if (hdnTransID.Value != "0")
                        {
                            DataRow dr = tblDeleteGL.NewRow();

                            dr["ID"] = Convert.ToInt32(hdnTransID.Value);
                            dr["Description"] = txtDesc.Text;
                            dr["Amount"] = double.Parse(txtGLAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                      NumberStyles.AllowThousands |
                      NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                            tblDeleteGL.Rows.Add(dr);
                        }

                        //Remove row
                        Label lblIndex = (Label)gr.FindControl("lblIndex");
                        int index = Convert.ToInt32(lblIndex.Text) - 1;
                        dt.Rows.RemoveAt(index - count);
                        count++;
                        dt.AcceptChanges();
                    }

                }
                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["Acct"] = DBNull.Value;
                    dr["ID"] = DBNull.Value;
                    dr["TransID"] = 0;
                    dr["fTitle"] = "";
                    dr["fDesc"] = "";
                    dr["Ref"] = "";
                    dr["Amount"] = 0;
                    dr["OrderNo"] = 0;
                    dr["isChecked"] = 1;
                    dt.Rows.Add(dr);
                }

                ViewState["GLTables"] = dt;
                RadGrid_gvDepositGL.DataSource = dt;
                RadGrid_gvDepositGL.VirtualItemCount = dt.Rows.Count;
                RadGrid_gvDepositGL.DataBind();
                RadGrid_gvDepositGL.Rebind();
            }
            ViewState["GLTablesDelete"] = tblDeleteGL;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "calCount", "CalCountAmount();", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion


    protected void ibtnDeleteInvoiceRow_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtDelete = CreateInvoiceTable();
            if (ViewState["InvoiceTablesDelete"] != null)
            {
                dtDelete = (DataTable)ViewState["InvoiceTablesDelete"];
            }
            
            if (ViewState["Invoices"] != null)
            {
                getDataOnInvoiceDepositGrid();

                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["Invoices"];
                int count = 0;
                foreach (GridDataItem gr in RadGrid_gvInvoiceDeposit.Items)
                {

                    HiddenField hdnOwner = (HiddenField)gr.FindControl("hdnOwner");
                    HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");
                    TextBox txtInvoiceID = (TextBox)gr.FindControl("txtInvoiceID");
                    HiddenField hdnRol = (HiddenField)gr.FindControl("hdnRol");
                    TextBox txtCustomerName = (TextBox)gr.FindControl("txtCustomerName");
                    HiddenField hdnLoc = (HiddenField)gr.FindControl("hdnLoc");
                    TextBox txtTag = (TextBox)gr.FindControl("txtLocationName");
                    HiddenField hdnEn = (HiddenField)gr.FindControl("hdnEn");
                    HiddenField hdnCompany = (HiddenField)gr.FindControl("hdnCompany");
                    TextBox txtAmount = (TextBox)gr.FindControl("txtAmount");
                    TextBox txtPaymentReceivedDate = (TextBox)gr.FindControl("txtPaymentReceivedDate");
                    TextBox txtDesc = (TextBox)gr.FindControl("txtDesc");
                    DropDownList ddlPaymentMethod = (DropDownList)gr.FindControl("ddlPaymentMethod");
                    TextBox txtCheckNumber = (TextBox)gr.FindControl("txtCheckNumber");
                    HiddenField hdnAmountDue = (HiddenField)gr.FindControl("hdnAmountDue");
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkInvSelect");
                    HiddenField hdnOrderNo = (HiddenField)gr.FindControl("hdnOrderNo");
                    HiddenField hdnRefTranID = (HiddenField)gr.FindControl("hdnRefTranID");

                    if (chkSelect.Checked)
                    {
                        if (Convert.ToInt32(hdnID.Value) != 0)
                        {
                            DataRow dr = dtDelete.NewRow();
                            dr["Owner"] = hdnOwner.Value;
                            dr["ID"] = hdnID.Value;
                            dr["InvoiceID"] = txtInvoiceID.Text==""?0:Convert.ToInt32( txtInvoiceID.Text);
                            dr["Rol"] = hdnRol.Value;

                            dr["customerName"] = txtCustomerName.Text;
                            dr["loc"] = hdnLoc.Value;
                            dr["Tag"] = txtTag.Text;
                            dr["En"] = hdnEn.Value;
                            dr["Company"] = hdnCompany.Value;

                            dr["Amount"] = double.Parse(txtAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                         NumberStyles.AllowThousands |
                                         NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                            dr["PaymentReceivedDate"] = txtPaymentReceivedDate.Text;
                            dr["fDesc"] = txtDesc.Text;
                            dr["PaymentMethod"] = ddlPaymentMethod.SelectedItem.Text;
                            //dr["PaymentMethodID"] = ddlPaymentMethod.SelectedValue;
                            dr["CheckNumber"] = txtCheckNumber.Text;
                            dr["AmountDue"] = double.Parse(hdnAmountDue.Value.Replace('$', '0'), NumberStyles.AllowParentheses |
                                         NumberStyles.AllowThousands |
                                         NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                            dr["RefTranID"] = hdnRefTranID.Value == "" ? 0 : Convert.ToInt32(hdnRefTranID.Value);
                            dtDelete.Rows.Add(dr);
                        }

                        //Remove row
                        Label lblIndex = (Label)gr.FindControl("lblIndex");
                        int index = Convert.ToInt32(lblIndex.Text) - 1;
                        dt.Rows.RemoveAt(index - count);
                        count++;
                        dt.AcceptChanges();
                    }

                }
                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["Owner"] = 0;
                    dr["ID"] = 0;
                    dr["InvoiceID"] = 0;
                    dr["Rol"] = 0;
                    dr["customerName"] = DBNull.Value;
                    dr["loc"] = 0;
                    dr["Tag"] = DBNull.Value;
                    dr["En"] = 0;
                    dr["Company"] = DBNull.Value;
                    dr["Amount"] = 0;
                    dr["PaymentReceivedDate"] = DateTime.Now;
                    dr["fDesc"] = DBNull.Value;
                    dr["PaymentMethod"] = "Check";
                    dr["PaymentMethodID"] = "0";
                    dr["CheckNumber"] = DBNull.Value;
                    dr["AmountDue"] = 0;
                    dr["OrderNo"] = 0;
                    dr["isChecked"] = 1;
                    dr["RefTranID"] = 0;
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }

                ViewState["Invoice"] = dt;
                RadGrid_gvInvoiceDeposit.DataSource = dt;
                RadGrid_gvInvoiceDeposit.VirtualItemCount = dt.Rows.Count;
                RadGrid_gvInvoiceDeposit.DataBind();
                RadGrid_gvInvoiceDeposit.Rebind();
              
            }
            ViewState["InvoiceTablesDelete"] = dtDelete;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "calCount", "CalCountAmount();", true);

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void RadGrid_gvInvoiceDeposit_PreRender(object sender, EventArgs e)
    {
        try
        {

            if (ViewState["Invoices"] != null)
            {
                DataTable dt = (DataTable)ViewState["Invoices"];
                if (dt.Columns.Contains("AmountDue"))
                {
                    GridFooterItem footerItem = (GridFooterItem)RadGrid_gvInvoiceDeposit.MasterTableView.GetItems(GridItemType.Footer)[0];

                    Label lblTotalInvPayAmount = footerItem.FindControl("lblTotalInvPayAmount") as Label;
                    lblTotalInvPayAmount.Text = string.Format("{0:c}", dt.Compute("sum(AmountDue)", string.Empty));

                   

                }
            }
           
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }

    private void UpdateDepositInfor()
    {
        try
        {
            _objDep.ConnConfig = Session["config"].ToString();
            _objDep.Ref = Convert.ToInt32(Request.QueryString["id"]);
            _objDep.fDate = Convert.ToDateTime(txtDateDeposite.Text);
            _objDep.Bank = Convert.ToInt32(ddlBank.SelectedValue);
            _objDep.fDesc = txtMemo.Text;
            depIdVal = Convert.ToInt32(Request.QueryString["id"]);
            _objDep.Amount = double.Parse(hdDepositTotal.Value.Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);

            //API
            _DepositInfor_UpdateDeposit.ConnConfig = Session["config"].ToString();
            _DepositInfor_UpdateDeposit.Ref = Convert.ToInt32(Request.QueryString["id"]);
            _DepositInfor_UpdateDeposit.fDate = Convert.ToDateTime(txtDateDeposite.Text);
            _DepositInfor_UpdateDeposit.Bank = Convert.ToInt32(ddlBank.SelectedValue);
            _DepositInfor_UpdateDeposit.fDesc = txtMemo.Text;
            _DepositInfor_UpdateDeposit.Amount = double.Parse(hdDepositTotal.Value.Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "MakeDepositAPI/AddDeposit_DepositInfor_UpdateDeposit";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _DepositInfor_UpdateDeposit);
            }
            else
            {
                _objBL_Deposit.UpdateDeposit(_objDep);
            }
           
            if (!string.IsNullOrEmpty(hdnTransId.Value.ToString()))
            {
                _objChart.ConnConfig = Session["config"].ToString();
                _objChart.Bank = Convert.ToInt32(ddlBank.SelectedValue);

                _GetBankAcctID.ConnConfig = Session["config"].ToString();
                _GetBankAcctID.Bank = Convert.ToInt32(ddlBank.SelectedValue);

                _objTrans.ConnConfig = Session["config"].ToString();
                _objTrans.ID = Convert.ToInt32(hdnTransId.Value);

                _UpdateDepositTransBank.ConnConfig = Session["config"].ToString();
                _UpdateDepositTransBank.ID = Convert.ToInt32(hdnTransId.Value);

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "MakeDepositAPI/AddDeposit_GetBankAcctID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetBankAcctID);
                    _UpdateDepositTransBank.Acct = Convert.ToInt32(_APIResponse.ResponseData);
                }
                else
                {
                    _objTrans.Acct = _objBL_Chart.GetBankAcctID(_objChart);
                }

                _objTrans.AcctSub = Convert.ToInt32(ddlBank.SelectedValue);
                _objTrans.TransDate = Convert.ToDateTime(txtDateDeposite.Text);

                _UpdateDepositTransBank.AcctSub = Convert.ToInt32(ddlBank.SelectedValue);
                _UpdateDepositTransBank.TransDate = Convert.ToDateTime(txtDateDeposite.Text);

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "MakeDepositAPI/AddDeposit_UpdateDepositTransBank";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateDepositTransBank);
                }
                else
                {
                    _objBL_Deposit.UpdateDepositTransBank(_objTrans);
                }
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }

    }

    protected void RadGrid_gvDepositGL_PreRender(object sender, EventArgs e)
    {

        try
        {

            if (ViewState["GLTables"] != null)
            {
                DataTable dt = (DataTable)ViewState["GLTables"];
                if (dt.Columns.Contains("Amount"))
                {
                    GridFooterItem footerItem = (GridFooterItem)RadGrid_gvDepositGL.MasterTableView.GetItems(GridItemType.Footer)[0];

                    Label lblTotalGLAmount = footerItem.FindControl("lblTotalGLAmount") as Label;
                    lblTotalGLAmount.Text = string.Format("{0:c}", dt.Compute("sum(Amount)", string.Empty));



                }
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }

    protected void lnkDepositSlip_Click(object sender, EventArgs e)
    {
         Boolean chkCount = false;

        #region Validation Check
        if (Request.QueryString["id"] != null)
        {
            chkCount = true;
            depIdVal = Convert.ToInt32(Request.QueryString["id"]);
        }
        else
        {
            foreach (GridDataItem gr in RadGrid_gvReceivePayment.Items)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                if (chkSelect.Checked.Equals(true))
                {
                    chkCount = true;
                }
            }
        }
           
        #endregion

       
        if (chkCount == true)
        {
            if (ddlBank.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Please select a bank ', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
            else
            {
                submit();
                _objDep.Ref = depIdVal;
                _objDep.ConnConfig= Session["config"].ToString();

                _GetDepByID.Ref = depIdVal;
                _GetDepByID.ConnConfig = Session["config"].ToString();

                _GetDepHeadByID.Ref = depIdVal;
                _GetDepHeadByID.ConnConfig = Session["config"].ToString();

                DataSet _dsDep = new DataSet();
                List<GetDepByIDViewModel> _lstGetDepByID = new List<GetDepByIDViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "MakeDepositAPI/AddDeposit_GetDepByID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetDepByID);

                    _lstGetDepByID = (new JavaScriptSerializer()).Deserialize<List<GetDepByIDViewModel>>(_APIResponse.ResponseData);
                    _dsDep = CommonMethods.ToDataSet<GetDepByIDViewModel>(_lstGetDepByID);
                }
                else
                {
                    _dsDep = _objBL_Deposit.GetDepByID(_objDep);
                }

                DataSet _sessionds = new DataSet();
                List<GetDepHeadByIDViewModel> _lstGetDepHeadByID = new List<GetDepHeadByIDViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "MakeDepositAPI/AddDeposit_GetDepHeadByID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetDepHeadByID);

                    _lstGetDepHeadByID = (new JavaScriptSerializer()).Deserialize<List<GetDepHeadByIDViewModel>>(_APIResponse.ResponseData);
                    _sessionds = CommonMethods.ToDataSet<GetDepHeadByIDViewModel>(_lstGetDepHeadByID);
                }
                else
                {
                    _sessionds = _objBL_Deposit.GetDepHeadByID(_objDep);
                }

                Session["DepositHead"] = _sessionds;
                Session["RefNo"] = depIdVal.ToString();
                if (_dsDep.Tables[0].Columns.Contains("Ref"))
                {                
                   
                    Response.Redirect("DepositSlipReport.aspx?uid="+ depIdVal);

                }

            }

        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Please select an item ', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
        }
    }  
}