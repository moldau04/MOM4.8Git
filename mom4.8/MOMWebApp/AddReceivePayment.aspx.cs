using BusinessEntity;
using BusinessEntity.CustomersModel;
using BusinessEntity.Utility;
using BusinessLayer;
using BusinessLayer.Billing;
using MOMWebApp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AddReceivePayment : System.Web.UI.Page
{
    #region Variables
    private const string asc = " ASC";
    private const string desc = " DESC";

    Customer objCustomer = new Customer();

    BL_Customer objBL_Customer = new BL_Customer();
    Contracts objPropContracts = new Contracts();

    Chart objChart = new Chart();
    BL_Chart objBL_Chart = new BL_Chart();

    BusinessEntity.Invoices objInvoice = new BusinessEntity.Invoices();

    Transaction objTrans = new Transaction();
    ReceivedPayment objReceivePay = new ReceivedPayment();
    PaymentDetails objPayment = new PaymentDetails();

    BL_Deposit objBL_Deposit = new BL_Deposit();

    Journal objJournal = new Journal();
    BL_JournalEntry objBL_Journal = new BL_JournalEntry();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    //API Variables 
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    GetInvoiceByCustomerIDParam _GetInvoiceByCustomerID = new GetInvoiceByCustomerIDParam();
    GetInvoiceNosChangeParam _GetInvoiceNosChange = new GetInvoiceNosChangeParam();
    UpdateReceivePaymentParam _UpdateReceivePayment = new UpdateReceivePaymentParam();
    AddReceivePaymentParam _AddReceivePayment = new AddReceivePaymentParam();
    GetInvoicesByReceivedPayParam _GetInvoicesByReceivedPay = new GetInvoicesByReceivedPayParam();
    GetUndepositeAcctParam _GetUndepositeAcct = new GetUndepositeAcctParam();
    GetReceivePaymentByIDParam _GetReceivePaymentByID = new GetReceivePaymentByIDParam();
    GetScreensByUserParam _GetScreensByUser = new GetScreensByUserParam();
    GetReceivePaymentLogsParam _GetReceivePaymentLogs = new GetReceivePaymentLogsParam();
    GetCustomerUnAppliedCreditParam _GetCustomerUnAppliedCredit = new GetCustomerUnAppliedCreditParam();
    GetInvoicesByIDParam _GetInvoicesByID = new GetInvoicesByIDParam();
    GetActiveBillingCodeParam _GetActiveBillingCode = new GetActiveBillingCodeParam();
    writeOffInvoiceMultiParam _writeOffInvoiceMulti = new writeOffInvoiceMultiParam();
    writeOffInvoiceParam _writeOffInvoice = new writeOffInvoiceParam();
    GetLocationByIDParam _GetLocationByID = new GetLocationByIDParam();
    TransferPaymentParam _TransferPayment = new TransferPaymentParam();
    UnapplyPaymentParam _UnapplyPayment = new UnapplyPaymentParam();
    #endregion

    #region Events

    #region PAGELOAD
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }
            ViewState["isCanada"] = isCanadaCompany();
            if (!IsPostBack)
            {
                txtStatus.Enabled = false;
                txtStatus.Text = "Open";
                //userpermissions();
                if (!CheckAddEditPermission()) { Response.Redirect("Home.aspx?permission=no"); return; }
                divSuccess.Visible = false;
                //FillCustomer();
                FillPayment();
                FillFilterOption();
                FillCustomerUnAppliedCredit(Convert.ToInt32(ddlFilter.SelectedValue));
                SetUndepositedFund();
                if (Request.QueryString["id"] != null) // edit received payment
                {  //Display Report
                    Page.Title = "Edit Receive Payment || MOM";
                    //lnkPrint.Style["visibility"] = "visible";
                    lnkPrint.Visible = true;
                    lnkWriteOff.Visible = false;
                    lnkTranfer.Visible = false;

                    lblHeader.Text = "Edit Receive Payment";
                    liLogs.Style["display"] = "inline-block";
                    tbLogs.Style["display"] = "block";
                    SetDataForEdit();
                    GetPeriodDetails(Convert.ToDateTime(txtDate.Text));
                    txtCustomer.ReadOnly = true;
                    //txtLocation.ReadOnly = true;
                    rfvCustomer.Enabled = false;
                    dvCompanyPermission.Visible = true;
                    ddlCustomer.Visible = false;

                    if (Session["receMsg"] != null && Session["receMsg"].ToString() == "UpdateSuccess")
                    {
                        Session["receMsg"] = null;
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Received payment Updated Successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                    }
                }
                else //  add received payment
                {
                    lnkUnApply.Visible = false;
                    if (Session["receMsg"] != null && Session["receMsg"].ToString() == "success")
                    {
                            Session["receMsg"] = null;
                            if (Request.QueryString["page"] != null && Request.QueryString["uid"] != null)
                            {
                                String url = "addinvoice?uid=" + Request.QueryString["uid"];
                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "showMessage", "showMessageAndRedirect('Received payment Added Successfully!','success','" + url + "');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Received payment Added Successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                            }                       

                    }
                    // ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Received payment Added Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                    //

                    lnkPrint.Visible = false;
                    dvCompanyPermission.Visible = false;
                    Page.Title = "Add Receive Payment || MOM";
                    txtMemo.Text = "Received payment";
                    lblCustomerBalance.Text = "$0.00";
                    txtAmount.Text = "$0.00";
                    txtStatus.Text = "Open";

                    DataSet ds = new DataSet();
                    DataSet ds1 = new DataSet();
                    DataSet ds2 = new DataSet();

                    objProp_Contracts.ConnConfig = Session["config"].ToString();
                    objProp_Contracts.Rol = 0;

                    _GetInvoiceByCustomerID.ConnConfig = Session["config"].ToString();
                    _GetInvoiceByCustomerID.Rol = 0;

                    ListGetInvoiceByCustomerID _lstGetInvoiceByCustomerID = new ListGetInvoiceByCustomerID();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "ReceivePaymentAPI/AddReceivePayment_GetInvoiceByCustomerID";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoiceByCustomerID);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        serializer.MaxJsonLength = Int32.MaxValue;

                        _lstGetInvoiceByCustomerID = serializer.Deserialize<ListGetInvoiceByCustomerID>(_APIResponse.ResponseData);

                        ds1 = _lstGetInvoiceByCustomerID.lstTable1.ToDataSet();
                        ds2 = _lstGetInvoiceByCustomerID.lstTable2.ToDataSet();

                        DataTable dt1 = new DataTable();
                        DataTable dt2 = new DataTable();

                        dt1 = ds1.Tables[0];
                        dt2 = ds2.Tables[0];

                        dt1.TableName = "Table1";
                        dt2.TableName = "Table2";

                        ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });
                    }
                    else
                    {
                        ds = objBL_Deposit.GetInvoiceByCustomerID(objProp_Contracts);
                    }

                    SetGridView(ds.Tables[0]);
                    txtDate.Text = DateTime.Today.ToShortDateString();
                    if (hdnDateValidate.Value == "")
                    {
                        hdnDateValidate.Value = DateTime.Today.ToShortDateString();
                    }
                    if (Convert.ToInt32(ddlFilter.SelectedValue) == 1)
                    {
                        divCustomer.Attributes.Add("style", "display:block");
                        ddlCustomer.Visible = false;
                        //ScriptManager.RegisterStartupScript(this, GetType(), "hiddenDiv", " $('#" + divCustomer.ClientID + "').css('display', 'block');", true);
                    }
                    else
                    {
                        divCustomer.Attributes.Add("style", "display:none");
                        ddlCustomer.Visible = true;
                    }
                    if (Request.QueryString["uid"] != null)
                    {
                        txtInvoice.Text = Request.QueryString["uid"].ToString();
                        DataSet dsInv = new DataSet();
                        PaymentDetails pd = new PaymentDetails();
                        pd.ConnConfig = Session["config"].ToString();
                        pd.strInvoiceId = Request.QueryString["uid"].ToString();

                        _GetInvoiceNosChange.ConnConfig = Session["config"].ToString();
                        _GetInvoiceNosChange.strInvoiceId = Request.QueryString["uid"].ToString();

                        List<GetInvoiceNosChangeViewModel> _lstGetInvoiceNosChange = new List<GetInvoiceNosChangeViewModel>();

                        if (IsAPIIntegrationEnable == "YES")
                        {
                            string APINAME = "ReceivePaymentAPI/AddReceivePayment_GetInvoiceNosChange";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoiceNosChange);

                            JavaScriptSerializer serializer = new JavaScriptSerializer();

                            serializer.MaxJsonLength = Int32.MaxValue;

                            _lstGetInvoiceNosChange = serializer.Deserialize<List<GetInvoiceNosChangeViewModel>>(_APIResponse.ResponseData);
                            dsInv = CommonMethods.ToDataSet<GetInvoiceNosChangeViewModel>(_lstGetInvoiceNosChange);
                        }
                        else
                        {
                            dsInv = objBL_Deposit.GetInvoiceNosChange(pd);
                        }

                        if (dsInv != null)
                        {
                            txtCustomer.Text = dsInv.Tables[0].Rows[0]["OwnerName"].ToString();
                            hdnCustID.Value = dsInv.Tables[0].Rows[0]["Owner"].ToString();
                            txtLocation.Text = dsInv.Tables[0].Rows[0]["Tag"].ToString();
                            hdnLocID.Value = dsInv.Tables[0].Rows[0]["Loc"].ToString();

                        }
                    }
                }
                Permission();
                CompanyPermission();
                HighlightSideMenu("cstmMgr", "lnkReceivePayment", "cstmMgrSub");

            }

            if (Request.QueryString["id"] != null)
            {
                pnlNext.Visible = true;
            }
            else
            {
                pnlNext.Visible = false;
            }
            Session["receMsg"] = null;


        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

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
            dvCompanyPermission.Visible = true;
        }
        else
        {
            dvCompanyPermission.Visible = false;
        }
    }
    protected void Page_PreRender(Object o, EventArgs e)
    {
        try
        {
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "SelectedRowStyle('" + gvInvoice.ClientID + "');", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
        }
    }
    #endregion
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnIsWriteOff.Value == "1")
            {
                openWriteOffMulti();
            }
            else
            {
                if (Page.IsValid)
                { //variable added to check if any line item checked

                    bool IsValid = true;
                    //if (IsValid)
                    //{
                    DateTime receivedt;
                    //if (Request.QueryString["id"] != null)
                    //{
                    //    receivedt = Convert.ToDateTime(hdnDate.Value);
                    //}
                    //else
                    receivedt = Convert.ToDateTime(txtDate.Text);

                    GetPeriodDetails(receivedt);     //Check period closed out permission
                    bool Flag = (bool)ViewState["FlagPeriodClose"];
                    if (!Flag)
                    {
                        IsValid = false;
                    }

                    if (IsValid)
                    {
                        divSuccess.Visible = false;
                        double totalPay = 0.00;
                        double totalDue = 0.00;

                        CreateDatatable();
                        DataTable dt = (DataTable)ViewState["ReceivPay"];
                        DataTable dtReceive = dt.Clone();
                        objReceivePay.ConnConfig = Session["config"].ToString();
                        objReceivePay.ID = Convert.ToInt32(Request.QueryString["id"]);
                        objReceivePay.Rol = Convert.ToInt32(hdnCustID.Value);

                        _UpdateReceivePayment.ConnConfig = Session["config"].ToString();
                        _UpdateReceivePayment.ID = Convert.ToInt32(Request.QueryString["id"]);

                        _AddReceivePayment.ConnConfig = Session["config"].ToString();
                        _AddReceivePayment.Rol = Convert.ToInt32(hdnCustID.Value);

                        if (!string.IsNullOrEmpty(hdnLocID.Value))
                        {
                            objReceivePay.Loc = Convert.ToInt32(hdnLocID.Value);
                            _UpdateReceivePayment.Loc = Convert.ToInt32(hdnLocID.Value);
                            _AddReceivePayment.Loc = Convert.ToInt32(hdnLocID.Value);
                        }
                        else
                        {
                            objReceivePay.Loc = 0;
                            _UpdateReceivePayment.Loc = 0;
                            _AddReceivePayment.Loc = 0;
                        }

                        objReceivePay.PaymentReceivedDate = Convert.ToDateTime(txtDate.Text);
                        objReceivePay.PaymentMethod = Convert.ToInt16(ddlPayment.SelectedValue);
                        objReceivePay.CheckNumber = txtCheck.Text.Trim();
                        objReceivePay.fDesc = txtMemo.Text;
                        objReceivePay.MOMUSer = Session["User"].ToString();

                        _UpdateReceivePayment.PaymentReceivedDate = Convert.ToDateTime(txtDate.Text);
                        _UpdateReceivePayment.PaymentMethod = Convert.ToInt16(ddlPayment.SelectedValue);
                        _UpdateReceivePayment.CheckNumber = txtCheck.Text.Trim();
                        _UpdateReceivePayment.fDesc = txtMemo.Text;
                        _UpdateReceivePayment.MOMUSer = Session["User"].ToString();

                        _AddReceivePayment.PaymentReceivedDate = Convert.ToDateTime(txtDate.Text);
                        _AddReceivePayment.PaymentMethod = Convert.ToInt16(ddlPayment.SelectedValue);
                        _AddReceivePayment.CheckNumber = txtCheck.Text.Trim();
                        _AddReceivePayment.fDesc = txtMemo.Text;
                        _AddReceivePayment.MOMUSer = Session["User"].ToString();

                        foreach (GridDataItem gr in RadGrid_gvInvoice.Items)
                        {
                            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

                            if (chkSelect.Checked.Equals(true) || chkSelect.Enabled == false)
                            {
                                HiddenField hdnType = (HiddenField)gr.FindControl("hdnType");
                                HiddenField hdnPaymentID = (HiddenField)gr.FindControl("hdnPaymentID");
                                HiddenField hdnTransID = (HiddenField)gr.FindControl("hdnTransID");
                                HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");
                                Label lblLoc = (Label)gr.FindControl("lblLoc");
                                HiddenField lblIsCredit = (HiddenField)gr.FindControl("lblIsCredit");
                                Label lblOrigAmount = (Label)gr.FindControl("lblOrigAmount");
                                Label lblDueAmount = (Label)gr.FindControl("lblDueAmount");
                                TextBox txtPAmount = (TextBox)gr.FindControl("txtPAmount");
                                HiddenField hdPAmount = (HiddenField)gr.FindControl("hdPAmount");
                                HiddenField hdnPrevDue = (HiddenField)gr.FindControl("hdnPrevDue");
                                HiddenField hdnRefTranID = (HiddenField)gr.FindControl("hdnRefTranID");
                                Label lblType = (Label)gr.FindControl("lblType");
                                double pay = 0;
                                if (Request.QueryString["id"] != null && hdnPaymentID.Value != "0")
                                {
                                    pay = double.Parse(hdPAmount.Value.Replace('$', '0'), NumberStyles.AllowParentheses |
                                            NumberStyles.AllowThousands |
                                            NumberStyles.AllowDecimalPoint);
                                }
                                else
                                {
                                    pay = double.Parse(txtPAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                             NumberStyles.AllowThousands |
                                             NumberStyles.AllowDecimalPoint);
                                }
                                //pay = double.Parse(txtPAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                //            NumberStyles.AllowThousands |
                                //            NumberStyles.AllowDecimalPoint);

                                double orig = double.Parse(lblOrigAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                NumberStyles.AllowThousands |
                                                NumberStyles.AllowDecimalPoint);
                                double due = double.Parse(lblDueAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                NumberStyles.AllowThousands |
                                                NumberStyles.AllowDecimalPoint);
                                double prevDue = double.Parse(hdnPrevDue.Value);    // actual due amount = Previous due amount


                                due = double.Parse(lblDueAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                NumberStyles.AllowThousands |
                                                NumberStyles.AllowDecimalPoint);      // calculated due amount

                                totalPay = +(totalPay + pay);
                                totalDue = totalDue + prevDue;

                                #region invoice status

                                DataRow dr = dtReceive.NewRow();
                                dr["InvoiceID"] = Convert.ToInt32(hdnID.Value);
                                bool IsNeg = false;
                                if (pay < 0)
                                {
                                    IsNeg = true;
                                    pay = pay * -1;
                                    due = due * -1;
                                    prevDue = prevDue * -1;
                                }
                                if (pay < due)
                                {
                                    dr["Status"] = 3;           // partial payment : invoice
                                }
                                else if (prevDue.Equals(pay))
                                {
                                    dr["Status"] = 1;           // paid : invoice
                                }
                                else
                                {
                                    if (pay < prevDue)
                                    {
                                        dr["Status"] = 3;
                                    }
                                    //dr["Status"] = 0;           // open : invoice
                                }
                                if (IsNeg)
                                {
                                    pay = pay * -1;
                                    due = due * -1;
                                    prevDue = prevDue * -1;
                                }

                                dr["PayAmount"] = pay;
                                if (lblIsCredit.Value.Equals("1"))
                                {
                                    dr["IsCredit"] = 1;
                                }
                                else if (lblIsCredit.Value.Equals("3"))
                                {
                                    dr["IsCredit"] = 3;
                                }
                                else if (lblIsCredit.Value.Equals("2") && hdnType.Value.Equals("3"))
                                {
                                    dr["IsCredit"] = 3;
                                }
                                else
                                {
                                    dr["IsCredit"] = 0;
                                }

                                #endregion
                                dr["Loc"] = Convert.ToInt32(lblLoc.Text);
                                dr["Type"] = 0;
                                dr["RefTranID"] = hdnRefTranID.Value;
                                if (hdnType.Value != "")
                                {
                                    dr["Type"] = Convert.ToInt16(hdnType.Value);
                                }

                                dtReceive.Rows.Add(dr);
                            }                            
                        }
                       

                        objReceivePay.Amount = double.Parse(txtAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                                            NumberStyles.AllowThousands |
                                                                            NumberStyles.AllowDecimalPoint);
                        objReceivePay.AmountDue = totalDue - totalPay;

                        objReceivePay.DtPay = dtReceive;


                        _UpdateReceivePayment.Amount = double.Parse(txtAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                                            NumberStyles.AllowThousands |
                                                                            NumberStyles.AllowDecimalPoint);
                        _UpdateReceivePayment.AmountDue = totalDue - totalPay;

                        _AddReceivePayment.Amount = double.Parse(txtAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                                            NumberStyles.AllowThousands |
                                                                            NumberStyles.AllowDecimalPoint);
                        _AddReceivePayment.AmountDue = totalDue - totalPay;

                        if (dtReceive.Rows.Count == 0)
                        {
                            DataTable returnVal = EmptydtReceive();
                            _UpdateReceivePayment.DtPay = returnVal;
                            _AddReceivePayment.DtPay = returnVal;
                        }
                        else
                        {
                            _UpdateReceivePayment.DtPay = dtReceive;
                            _AddReceivePayment.DtPay = dtReceive;
                        }

                        if (Request.QueryString["id"] != null)
                        {
                            if (IsAPIIntegrationEnable == "YES")
                            {
                                string APINAME = "ReceivePaymentAPI/AddReceivePayment_UpdateReceivePayment";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateReceivePayment);
                            }
                            else
                            {
                                objBL_Deposit.UpdateReceivePayment(objReceivePay);
                            }

                           // SetDataForEdit();
                           // RadGrid_gvLogs.Rebind();                           
                           // ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Received payment Updated Successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                          
                            Session["receMsg"] = "UpdateSuccess";
                            Response.Redirect("addreceivepayment.aspx?id=" + Request.QueryString["id"]);
                        }
                        else
                        {
                            if (IsAPIIntegrationEnable == "YES")
                            {
                                string APINAME = "ReceivePaymentAPI/AddReceivePayment_AddReceivePayment";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddReceivePayment);
                            }
                            else
                            {
                                objBL_Deposit.AddReceivePayment(objReceivePay);
                            }

                            RadGrid_gvInvoice.DataSource = string.Empty;
                            RadGrid_gvInvoice.Rebind();

                            RadGrid_gvLogs.Rebind();
                            ViewState["Invoices"] = null;
                            Session["receMsg"] = "success";
                            if (Request.QueryString["page"] != null && Request.QueryString["uid"] != null)
                            {
                                Response.Redirect("addreceivepayment?page=" + Request.QueryString["page"] + "&uid=" + Request.QueryString["uid"]);
                            }
                            else
                            {
                                Response.Redirect("addreceivepayment.aspx");
                            }

                        }

                        //}
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'These month/year period is closed out. You do not have permission to add/update this record.',  type : 'Warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        // SetDataForEdit();
                        //BindGridInvoice();
                        //RadGrid_gvLogs.Rebind();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["page"] != null)
        {

            if (Request.QueryString["page"].ToString() == "addinvoice")
            {
                if (Request.QueryString["uid"] != null)
                {
                    Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + Request.QueryString["uid"].ToString());
                }
                else
                {
                    Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + Request.QueryString["invoiceId"].ToString());
                }

            }
            else
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
                        Response.Redirect(Request.QueryString["page"].ToString() + ".aspx?uid=" + Request.QueryString["lid"].ToString() + "&tab=inv");
                    }
                }

            }
        }
        else
        {
            Response.Redirect("receivepayment.aspx");
        }
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["ReceivedPayment"];
            Response.Redirect("addreceivepayment.aspx?id=" + dt.Rows[0]["ID"]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["ReceivedPayment"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);

            if (index > 0)
            {
                Response.Redirect("addreceivepayment.aspx?id=" + dt.Rows[index - 1]["ID"]);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["ReceivedPayment"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;

            if (index < c)
            {
                Response.Redirect("addreceivepayment.aspx?id=" + dt.Rows[index + 1]["ID"]);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkLast_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["ReceivedPayment"];
            Response.Redirect("addreceivepayment.aspx?id=" + dt.Rows[dt.Rows.Count - 1]["ID"]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
   
    protected void btnSelectDate_Click(object sender, EventArgs e)
    {
        if (txtLocation.Text != "")
        {
             processSelectLoc();
            hdnDateValidate.Value = txtDate.Text;
        }
        else
        {
            if(txtCustomer.Text != "")
            {
                btnSelectCustomer_Click(sender, e);
                hdnDateValidate.Value = txtDate.Text;
            }
            else
            {
                hdnDateValidate.Value = txtDate.Text;
            }
        }


    }
    protected void btnSelectLoc_Click(object sender, EventArgs e)
    {
        processSelectLoc();
    }
    protected void btnSelectCustomer_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["Invoices"] = null;
            txtAmount.Text = string.Format("{0:c}", 0);
            txtInvoice.Text = "";
            lblCustomerBalance.Text = string.Format("{0:c}", 0);
            if (!string.IsNullOrEmpty(hdnCustID.Value))
            {
                if (Convert.ToInt32(hdnCustID.Value) > 0)
                {
                    DataSet ds = new DataSet();
                    DataSet ds1 = new DataSet();
                    DataSet ds2 = new DataSet();

                    objPayment.ConnConfig = Session["config"].ToString();
                    objPayment.Rol = Convert.ToInt32(hdnCustID.Value);

                    _GetInvoicesByReceivedPay.ConnConfig = Session["config"].ToString();
                    _GetInvoicesByReceivedPay.Rol = Convert.ToInt32(hdnCustID.Value);

                    ListGetInvoicesByReceivedPay _lstGetInvoicesByReceivedPay = new ListGetInvoicesByReceivedPay();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "ReceivePaymentAPI/AddReceivePayment_GetInvoicesByReceivedPay";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoicesByReceivedPay);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        serializer.MaxJsonLength = Int32.MaxValue;

                        _lstGetInvoicesByReceivedPay = serializer.Deserialize<ListGetInvoicesByReceivedPay>(_APIResponse.ResponseData);

                        ds1 = _lstGetInvoicesByReceivedPay.lstTable1.ToDataSet();
                        DataTable dt1 = new DataTable();
                        dt1 = ds1.Tables[0];
                        dt1.TableName = "Table1";
                        ds.Tables.AddRange(new DataTable[] { dt1.Copy() });


                        if (_lstGetInvoicesByReceivedPay.lstTable2 != null)
                        {
                            ds2 = _lstGetInvoicesByReceivedPay.lstTable2.ToDataSet();
                            DataTable dt2 = new DataTable();
                            dt2 = ds2.Tables[0];
                            dt2.TableName = "Table2";
                            ds.Tables.AddRange(new DataTable[] { dt2.Copy() });
                        }
                    }
                    else
                    {
                        ds = objBL_Deposit.GetInvoicesByReceivedPay(objPayment);
                    }
                    DataTable filterdt = new DataTable();
                    DataRow[] dr = ds.Tables[0].Select("fDate<='" + txtDate.Text + "'");
                    if (dr.Length > 0)
                    {
                        filterdt = dr.CopyToDataTable();                        
                    }
                    else
                    {
                        filterdt = ds.Tables[0].Clone();
                    }                    
                    SetGridView(filterdt);
                    //SetGridView(ds.Tables[0]);
                    DataTable dt = ds.Tables[1];
                    lblCustomerBalance.Text = string.Format("{0:c}", dt.Rows[0]["Balance"]);

                    RadGrid_Location.Rebind();

                }
                
                txtLocation.Focus();
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Custom Functions

    private void CreateDatatable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("InvoiceID", typeof(int));
        dt.Columns.Add("Status", typeof(Int16));
        dt.Columns.Add("PayAmount", typeof(double));
        dt.Columns.Add("IsCredit", typeof(Int16));
        dt.Columns.Add("Type", typeof(Int16));
        dt.Columns.Add("Loc", typeof(Int32));
        dt.Columns.Add("RefTranID", typeof(Int32));


        DataRow dr = dt.NewRow();
        dr["InvoiceID"] = DBNull.Value;
        dr["Status"] = DBNull.Value;
        dr["PayAmount"] = 0;
        dr["IsCredit"] = DBNull.Value;
        dr["Loc"] = DBNull.Value;
        dr["Type"] = DBNull.Value;
        dr["RefTranID"] = DBNull.Value;
        dt.Rows.Add(dr);
        ViewState["ReceivPay"] = dt;
    }

    //API
    private DataTable EmptydtReceive()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("InvoiceID", typeof(int));
        dt.Columns.Add("Status", typeof(Int16));
        dt.Columns.Add("PayAmount", typeof(double));
        dt.Columns.Add("IsCredit", typeof(Int16));
        dt.Columns.Add("Type", typeof(Int16));
        dt.Columns.Add("Loc", typeof(Int32));
        dt.Columns.Add("RefTranID", typeof(Int32));
        DataRow dr = dt.NewRow();
        dr["InvoiceID"] = "0";
        dr["Status"] = "0";
        dr["PayAmount"] = "0";
        dr["IsCredit"] = "0";
        dr["Loc"] = "0";
        dr["Type"] = "0";
        dr["RefTranID"] = DBNull.Value;
        dt.Rows.Add(dr);
        return dt;
    }

    //private bool ValidateGrid()
    //{
    //    bool IsValid = true;
    //    try
    //    {
    //int count = 0;
    //foreach (GridViewRow gr in gvInvoice.Rows)
    //{
    //    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
    //    TextBox txtPAmount = (TextBox)gr.FindControl("txtPAmount");
    //    if (chkSelect.Checked.Equals(true))
    //    {
    //        if (!string.IsNullOrEmpty(txtPAmount.Text))
    //        {
    //            double pay = double.Parse(txtPAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
    //                      NumberStyles.AllowThousands |
    //                      NumberStyles.AllowDecimalPoint);
    //            if (!Convert.ToDouble(pay).Equals(0))
    //            {
    //                count++;
    //            }
    //        }
    //    }
    //}
    //if (count.Equals(0))
    //{
    //    IsValid = false;
    //}
    //if(!IsValid)
    //{
    //    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'You cannot record a blank transaction. Fill in the appropriate fields and try again.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //}
    //}
    //catch (Exception ex)
    //{
    //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //}
    //return IsValid;
    //}
    private void ResetForm()
    {
        ResetFormControlValues(this);

        //objPropContracts.ConnConfig = Session["config"].ToString();
        //objPropContracts.Rol = 0;

        objPayment.ConnConfig = Session["config"].ToString();
        objPayment.Rol = 0;
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();
        //ds = objBL_Deposit.GetInvoicesByReceivedPay(objPayment);

        objProp_Contracts.ConnConfig = Session["config"].ToString();
        objProp_Contracts.Rol = 0;

        _GetInvoiceByCustomerID.ConnConfig = Session["config"].ToString();
        _GetInvoiceByCustomerID.Rol = 0;

        ListGetInvoiceByCustomerID _lstGetInvoiceByCustomerID = new ListGetInvoiceByCustomerID();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "ReceivePaymentAPI/AddReceivePayment_GetInvoiceByCustomerID";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoiceByCustomerID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetInvoiceByCustomerID = serializer.Deserialize<ListGetInvoiceByCustomerID>(_APIResponse.ResponseData);

            ds1 = _lstGetInvoiceByCustomerID.lstTable1.ToDataSet();
            ds2 = _lstGetInvoiceByCustomerID.lstTable2.ToDataSet();

            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            dt1 = ds1.Tables[0];
            dt2 = ds2.Tables[0];

            dt1.TableName = "Table1";
            dt2.TableName = "Table2";

            ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });
        }
        else
        {
            ds = objBL_Deposit.GetInvoiceByCustomerID(objProp_Contracts);
        }

        SetGridView(ds.Tables[0]);
        txtStatus.Enabled = false;
        txtStatus.Text = "Open";

        //lblCheck.Text = "Check";
        lblCustomerBalance.Text = "$0.00";
        txtAmount.Text = "$0.00";
        txtMemo.Text = "Received payment";

    }
    private void SetUndepositedFund()
    {
        try
        {
            DataSet _dsAcct = new DataSet();
            objChart.ConnConfig = Session["config"].ToString();
            _GetUndepositeAcct.ConnConfig = Session["config"].ToString();

            List<GetUndepositeAcctViewModel> _lstGetUndepositeAcct = new List<GetUndepositeAcctViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "ReceivePaymentAPI/AddReceivePayment_GetUndepositeAcct";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetUndepositeAcct);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetUndepositeAcct = serializer.Deserialize<List<GetUndepositeAcctViewModel>>(_APIResponse.ResponseData);
                _dsAcct = CommonMethods.ToDataSet<GetUndepositeAcctViewModel>(_lstGetUndepositeAcct);
            }
            else
            {
                _dsAcct = objBL_Chart.GetUndepositeAcct(objChart);
            }

            if (_dsAcct.Tables[0].Columns.Contains("fDesc"))
            {
                lblDepositTo.Text = _dsAcct.Tables[0].Rows[0]["Acct"].ToString() + " - " + _dsAcct.Tables[0].Rows[0]["fDesc"].ToString();
                Session["DepositTo"] = lblDepositTo.Text;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
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
    private void FillPayment()
    {
        ddlPayment.Items.Add(new ListItem("Check", "0"));
        ddlPayment.Items.Add(new ListItem("Cash", "1"));
        ddlPayment.Items.Add(new ListItem("Wire Transfer", "2"));
        ddlPayment.Items.Add(new ListItem("ACH", "3"));
        ddlPayment.Items.Add(new ListItem("Credit Card", "4"));
        ddlPayment.Items.Add(new ListItem("e-Transfer", "5"));
        ddlPayment.Items.Add(new ListItem("Lockbox ", "6"));
    }
    private void SetGridView(DataTable dt)
    {
        try
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    ViewState["Invoices"] = dt;
                    RadGrid_gvInvoice.Rebind();
                }
                else
                {
                    ViewState["Invoices"] = null;
                    RadGrid_gvInvoice.DataSource = string.Empty;
                    RadGrid_gvInvoice.Rebind();
                }
            }
            else
            {
                ViewState["Invoices"] = null;
                RadGrid_gvInvoice.DataSource = string.Empty;
                RadGrid_gvInvoice.Rebind();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void SetDataForEdit()
    {
        try
        {
            divlblRef.Style["display"] = "block";

            // lblHeader.Text = "Edit Receive Payment";
            DataSet dsReceive = new DataSet();
            objReceivePay.ConnConfig = Session["config"].ToString();
            objReceivePay.ID = Convert.ToInt32(Request.QueryString["id"]);

            _GetReceivePaymentByID.ConnConfig = Session["config"].ToString();
            _GetReceivePaymentByID.ID = Convert.ToInt32(Request.QueryString["id"]);
            if (Convert.ToString(Request.QueryString["lid"]) != "")
            {
                if (Convert.ToString(Request.QueryString["page"]) == "addlocation")
                {
                    objReceivePay.page = "addlocation";
                }
                else
                {
                    objReceivePay.page = "addcustomer";
                }
                objReceivePay.Loc = Convert.ToInt32(Request.QueryString["lid"]);
            }
            else
            {
                objReceivePay.Loc = 0;
            }

            List<GetReceivePaymentByIDViewModel> _lstGetReceivePaymentByID = new List<GetReceivePaymentByIDViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "ReceivePaymentAPI/AddReceivePayment_GetReceivePaymentByID";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetReceivePaymentByID);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetReceivePaymentByID = serializer.Deserialize<List<GetReceivePaymentByIDViewModel>>(_APIResponse.ResponseData);
                dsReceive = CommonMethods.ToDataSet<GetReceivePaymentByIDViewModel>(_lstGetReceivePaymentByID);
            }
            else
            {
                dsReceive = objBL_Deposit.GetReceivePaymentByID(objReceivePay);
            }

            if (dsReceive != null && dsReceive.Tables.Count > 0 && dsReceive.Tables[0].Rows.Count > 0)
            {
                if (dsReceive.Tables[0].Columns.Contains("ID"))
                {
                    lblReceiveID.Text = dsReceive.Tables[0].Rows[0]["ID"].ToString();
                    Session["RefNo"] = lblReceiveID.Text;

                    if (Convert.ToInt16(dsReceive.Tables[0].Rows[0]["Status"]).Equals(1))
                    {
                        btnSubmit.Visible = false;
                        txtStatus.Text = "Deposited";
                    }
                    else if (Convert.ToInt16(dsReceive.Tables[0].Rows[0]["Status"]).Equals(2))
                    {
                        btnSubmit.Visible = false;
                        //lnkUnApply.Visible = false;
                        txtStatus.Text = "Applied";
                    }
                    else
                    {
                        txtStatus.Text = "Open";
                    }

                    if (Convert.ToInt16(dsReceive.Tables[0].Rows[0]["DepID"]).Equals(0))
                    {
                        lnkDepID.Visible = false;
                        trDeposit.Visible = false;
                    }
                    else
                    {
                        trDeposit.Visible = true;
                        lnkDepID.Visible = true;
                        lnkDepID.Text = Convert.ToString(dsReceive.Tables[0].Rows[0]["DepID"]);
                        lnkDepID.NavigateUrl = "adddeposit.aspx?id=" + Convert.ToString(dsReceive.Tables[0].Rows[0]["DepID"]);
                    }

                    Session["Status"] = txtStatus.Text;

                    txtCustomer.Text = dsReceive.Tables[0].Rows[0]["RolName"].ToString();
                    txtLocation.Text = dsReceive.Tables[0].Rows[0]["Tag"].ToString();
                    hdnCustID.Value = dsReceive.Tables[0].Rows[0]["Owner"].ToString();
                    hdnLocID.Value = dsReceive.Tables[0].Rows[0]["Loc"].ToString();
                    txtAmount.Text = string.Format("{0:c}", Convert.ToDouble(dsReceive.Tables[0].Rows[0]["Amount"]));

                    txtDate.Text = Convert.ToDateTime(dsReceive.Tables[0].Rows[0]["PaymentReceivedDate"]).ToString("MM/dd/yyyy");
                    hdnDateValidate.Value = Convert.ToDateTime(dsReceive.Tables[0].Rows[0]["PaymentReceivedDate"]).ToString("MM/dd/yyyy");
                    hdnDate.Value = Convert.ToDateTime(dsReceive.Tables[0].Rows[0]["PaymentReceivedDate"]).ToString("MM/dd/yyyy");
                    txtCheck.Text = dsReceive.Tables[0].Rows[0]["CheckNumber"].ToString();
                    if (txtCheck.Text == "") txtCheck.Text = " ";
                    ddlPayment.SelectedValue = dsReceive.Tables[0].Rows[0]["PaymentMethod"].ToString();
                    txtMemo.Text = dsReceive.Tables[0].Rows[0]["fDesc"].ToString();
                    txtCompany.Text = dsReceive.Tables[0].Rows[0]["Company"].ToString();
                    DataSet dsPayment = new DataSet();
                    objPayment.ConnConfig = Session["config"].ToString();
                    objPayment.ReceivedPaymentID = Convert.ToInt32(dsReceive.Tables[0].Rows[0]["ID"]);
                    objPayment.Rol = Convert.ToInt32(hdnCustID.Value);
                    objPayment.Loc = Convert.ToInt32(hdnLocID.Value);

                    _GetInvoicesByReceivedPay.ConnConfig = Session["config"].ToString();
                    _GetInvoicesByReceivedPay.ReceivedPaymentID = Convert.ToInt32(dsReceive.Tables[0].Rows[0]["ID"]);
                    _GetInvoicesByReceivedPay.Rol = Convert.ToInt32(hdnCustID.Value);
                    _GetInvoicesByReceivedPay.Loc = Convert.ToInt32(hdnLocID.Value);

                    DataSet ds = new DataSet();
                    DataSet ds1 = new DataSet();
                    DataSet ds2 = new DataSet();

                    ListGetInvoicesByReceivedPay _lstGetInvoicesByReceivedPay = new ListGetInvoicesByReceivedPay();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "ReceivePaymentAPI/AddReceivePayment_GetInvoicesByReceivedPay";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoicesByReceivedPay);

                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        serializer.MaxJsonLength = Int32.MaxValue;

                        _lstGetInvoicesByReceivedPay = serializer.Deserialize<ListGetInvoicesByReceivedPay>(_APIResponse.ResponseData);

                        ds1 = _lstGetInvoicesByReceivedPay.lstTable1.ToDataSet();
                        DataTable dt1 = new DataTable();
                        dt1 = ds1.Tables[0];
                        dt1.TableName = "Table1";
                        ds.Tables.AddRange(new DataTable[] { dt1.Copy() });


                        if (_lstGetInvoicesByReceivedPay.lstTable2 != null)
                        {
                            ds2 = _lstGetInvoicesByReceivedPay.lstTable2.ToDataSet();
                            DataTable dt2 = new DataTable();
                            dt2 = ds2.Tables[0];
                            dt2.TableName = "Table2";
                            ds.Tables.AddRange(new DataTable[] { dt2.Copy() });
                        }
                    }
                    else
                    {
                        ds = objBL_Deposit.GetInvoicesByReceivedPay(objPayment);
                    }


                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        lblCustomerBalance.Text = string.Format("{0:c}", Convert.ToDouble(ds.Tables[1].Rows[0]["Balance"]));
                    }
                    DataTable filterdt = new DataTable();
                    DataRow[] dr = ds.Tables[0].Select("fDate<='" + txtDate.Text + "'");
                    if (dr.Length > 0)
                    {
                        filterdt = dr.CopyToDataTable();
                    }
                    else
                    {
                        filterdt = ds.Tables[0].Clone();
                    }
                    SetGridView(filterdt);
                    //SetGridView(ds.Tables[0]);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'No Record Found.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void GetPeriodDetails(DateTime transDate)
    {
        bool Flag = CommonHelper.GetPeriodDetails(transDate);
        ViewState["FlagPeriodClose"] = Flag;
        if (!Flag)
        {
            divSuccess.Visible = true;
        }
    }
    private void userpermissions()
    {
        if (Session["type"].ToString() != "c")
        {
            if (Session["type"].ToString() != "am")
            {
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.Username = Session["username"].ToString();
                objPropUser.PageName = "addreceivepayment.aspx";

                _GetScreensByUser.ConnConfig = Session["config"].ToString();
                _GetScreensByUser.Username = Session["username"].ToString();
                _GetScreensByUser.PageName = "addreceivepayment.aspx";

                DataSet dspage = new DataSet();
                List<GetScreensByUserViewModel> _lstGetScreensByUser = new List<GetScreensByUserViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "ReceivePaymentAPI/AddReceivePayment_GetScreensByUser";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetScreensByUser);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetScreensByUser = serializer.Deserialize<List<GetScreensByUserViewModel>>(_APIResponse.ResponseData);
                    dspage = CommonMethods.ToDataSet<GetScreensByUserViewModel>(_lstGetScreensByUser);
                }
                else
                {
                    dspage = objBL_User.getScreensByUser(objPropUser);
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

            string ApplyPermission = ds.Rows[0]["Apply"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["Apply"].ToString();
            string stAddeApply = ApplyPermission.Length < 1 ? "Y" : ApplyPermission.Substring(0, 1);
            string stEditApply = ApplyPermission.Length < 2 ? "Y" : ApplyPermission.Substring(1, 1);
            string stDeleteApply = ApplyPermission.Length < 3 ? "Y" : ApplyPermission.Substring(2, 1);
            string stViewApply = ApplyPermission.Length < 4 ? "Y" : ApplyPermission.Substring(3, 1);

            //Write off
            string WriteOffPermissions = ds.Rows[0]["WriteOff"] == DBNull.Value ? "N" : ds.Rows[0]["WriteOff"].ToString().Substring(0, 1);
            lnkWriteOff.Visible = false;
            if (WriteOffPermissions == "Y")
            {
                lnkWriteOff.Visible = true;
            }


            if (stViewApply == "N")
            {
                result = false;
            }
            else if (Request.QueryString["id"] == null)
            {
                if (stAddeApply == "N")
                {
                    result = false;
                }
            }
            else if (stEditApply == "N")
            {
                if (stViewApply == "Y")
                {
                    btnSubmit.Visible = false;
                    lnkUnApply.Visible = false;
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
        ds = objBL_User.GetUserPermissionByUserID(objPropUser);
        return ds.Tables[0];
    }




    private void GetSelectedInvoice()
    {
        DataTable dtInvoice = new DataTable();
        dtInvoice = (DataTable)ViewState["Invoices"];

        foreach (GridDataItem row in RadGrid_gvInvoice.Items)
        {
            CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
            TextBox txtPAmount = (TextBox)row.FindControl("txtPAmount");
            HiddenField hdnPrevDue = (HiddenField)row.FindControl("hdnPrevDue");
            HiddenField hdnID = (HiddenField)row.FindControl("hdnID");
            double pay = double.Parse(txtPAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint);
            double due = double.Parse(hdnPrevDue.Value.Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint);
            int tref = Convert.ToInt32(hdnID.Value);

            if (!pay.Equals(0))
            {
                chkSelect.Checked = true;
                DataRow[] drInv = dtInvoice.Select("Ref = " + tref);

                foreach (var dr in drInv)
                {
                    dr["paymentAmt"] = Convert.ToDouble(pay);
                    dr["DueAmount"] = due - pay;
                }
            }
        }
        ViewState["Invoices"] = dtInvoice;
    }
    #endregion

    //protected void cvAmount_ServerValidate(object source, ServerValidateEventArgs args)
    //{
    //    try
    //    {
    //        double total = 0;
    //        foreach(GridViewRow gr in gvInvoice.Rows)
    //        {
    //            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
    //            if(chkSelect.Checked.Equals(true))
    //            {
    //                TextBox txtPayAmt = (TextBox)gr.FindControl("txtPAmount");
    //                if(!string.IsNullOrEmpty(txtPayAmt.Text))
    //                {
    //                    total += double.Parse(txtPayAmt.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
    //                              NumberStyles.AllowThousands |
    //                              NumberStyles.AllowDecimalPoint);
    //                }
    //            }
    //        }

    //        if(total.Equals(double.Parse(txtAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
    //                              NumberStyles.AllowThousands |
    //                              NumberStyles.AllowDecimalPoint)))
    //        {
    //            args.IsValid = true;
    //        }
    //        else
    //        {
    //            args.IsValid = false;
    //        }
    //    }
    //    catch(Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    protected void cvLocation_ServerValidate(object source, ServerValidateEventArgs args)
    {
        try
        {
            double total = 0;
            foreach (GridDataItem gr in RadGrid_gvInvoice.Items)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                if (chkSelect.Checked.Equals(true))
                {
                    TextBox txtPayAmt = (TextBox)gr.FindControl("txtPAmount");
                    if (!string.IsNullOrEmpty(txtPayAmt.Text))
                    {
                        total = total + double.Parse(txtPayAmt.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint);
                    }
                }
            }
            total = Math.Round(total * 100) / 100;
            if (!(double.Parse(txtAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses | NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint).Equals(total)))
            {
                if (string.IsNullOrEmpty(hdnLocID.Value) || hdnLocID.Value == "0")
                {
                    args.IsValid = false;
                }
                else
                {
                    args.IsValid = true;
                }
            }
            else
            {
                args.IsValid = true;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkPrint_Click(object sender, EventArgs e)
    {
        Response.Redirect("ReceivePaymentReport.aspx?id=" + Request.QueryString["id"]);
    }

    //protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    //{
    //    ((sender as CheckBox).NamingContainer as GridItem).Selected = (sender as CheckBox).Checked;
    //    bool checkHeader = true;
    //    foreach (GridDataItem dataItem in RadGrid_gvInvoice.MasterTableView.Items)
    //    {
    //        if (!(dataItem.FindControl("chkSelect") as CheckBox).Checked)
    //        {
    //            checkHeader = false;
    //            break;
    //        }
    //    }
    //    GridHeaderItem headerItem = RadGrid_gvInvoice.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
    //    (headerItem.FindControl("chkSelectAll") as CheckBox).Checked = checkHeader;
    //}
    protected void RadGrid_gvInvoice_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem gd = (GridDataItem)e.Item;
            CheckBox chk = (CheckBox)gd.FindControl("chkSelect");
            int index = gd.ItemIndex;
            e.Item.Attributes["onclick"] = "check('" + index + "','" + chk.ClientID + "')";
            //chk.Attributes.Add("onclick", "javascript:check();");
            //chk.Attributes.Add("onclick", "javascript:check('" + index + "','" + chk.ClientID + "')");
        }
    }


    protected void RadGrid_gvInvoice_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid_gvInvoice.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        if (ViewState["Invoices"] != null)
        {
            DataTable dt = (DataTable)ViewState["Invoices"];
            RadGrid_gvInvoice.VirtualItemCount = dt.Rows.Count;
            RadGrid_gvInvoice.DataSource = dt;
        }
        else
        {
            RadGrid_gvInvoice.DataSource = string.Empty;
        }
    }

    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return RadGrid_gvInvoice.MasterTableView.FilterExpression != "" ||
            (RadGrid_gvInvoice.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            RadGrid_gvInvoice.MasterTableView.SortExpressions.Count > 0;
    }

    protected void RadGrid_gvInvoice_PreRender(object sender, EventArgs e)
    {
        if (ViewState["Invoices"] != null)
        {
            DataTable dt = (DataTable)ViewState["Invoices"];
            if (dt.Columns.Contains("OrigAmount"))
            {
                GridFooterItem footerItem = (GridFooterItem)RadGrid_gvInvoice.MasterTableView.GetItems(GridItemType.Footer)[0];

                Label lblTotalOrigAmount = footerItem.FindControl("lblTotalOrigAmount") as Label;
                lblTotalOrigAmount.Text = string.Format("{0:c}", dt.Compute("sum(OrigAmount)", string.Empty));

                Label lblTotalSalesTax = footerItem.FindControl("lblTotalSalesTax") as Label;
                lblTotalSalesTax.Text = string.Format("{0:c}", dt.Compute("sum(STax)", string.Empty));

                Label lblTotalPretaxAmount = footerItem.FindControl("lblTotalPretaxAmount") as Label;
                lblTotalPretaxAmount.Text = string.Format("{0:c}", dt.Compute("sum(Amount)", string.Empty));

                Label lblTotalDueAmount = footerItem.FindControl("lblTotalDueAmount") as Label;
                lblTotalDueAmount.Text = string.Format("{0:c}", dt.Compute("sum(DueAmount)", string.Empty));

                Label lblTotalPayAmount = footerItem.FindControl("lblTotalPayAmount") as Label;
                lblTotalPayAmount.Text = string.Format("{0:c}", dt.Compute("sum(paymentAmt)", string.Empty));

            }
        }
        else
        {
            GridFooterItem footerItem = (GridFooterItem)RadGrid_gvInvoice.MasterTableView.GetItems(GridItemType.Footer)[0];

            Label lblTotalOrigAmount = footerItem.FindControl("lblTotalOrigAmount") as Label;
            lblTotalOrigAmount.Text = string.Format("{0:c}", 0);

            Label lblTotalSalesTax = footerItem.FindControl("lblTotalSalesTax") as Label;
            lblTotalSalesTax.Text = string.Format("{0:c}", 0);

            Label lblTotalPretaxAmount = footerItem.FindControl("lblTotalPretaxAmount") as Label;
            lblTotalPretaxAmount.Text = string.Format("{0:c}", 0);

            Label lblTotalDueAmount = footerItem.FindControl("lblTotalDueAmount") as Label;
            lblTotalDueAmount.Text = string.Format("{0:c}", 0);

            Label lblTotalPayAmount = footerItem.FindControl("lblTotalPayAmount") as Label;
            lblTotalPayAmount.Text = string.Format("{0:c}", 0);
        }
    }
    #region logs
    protected void RadGrid_gvLogs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        RadGrid_gvLogs.AllowCustomPaging = !ShouldApplySortFilterOrGroupLogs();
        if (Request.QueryString["id"] != null)
        {
            DataSet dsLog = new DataSet();
            objReceivePay.ConnConfig = Session["config"].ToString();
            objReceivePay.ID = Convert.ToInt32(Request.QueryString["id"]);

            _GetReceivePaymentLogs.ConnConfig = Session["config"].ToString();
            _GetReceivePaymentLogs.ID = Convert.ToInt32(Request.QueryString["id"]);

            List<GetLocationLogViewModel> _lstGetLocationLog = new List<GetLocationLogViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "ReceivePaymentAPI/AddReceivePayment_GetReceivePaymentLogs";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetReceivePaymentLogs);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetLocationLog = serializer.Deserialize<List<GetLocationLogViewModel>>(_APIResponse.ResponseData);
                dsLog = CommonMethods.ToDataSet<GetLocationLogViewModel>(_lstGetLocationLog);
            }
            else
            {
                dsLog = objBL_Deposit.GetReceivePaymentLogs(objReceivePay);
            }

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
    }
    bool isGroupLog = false;
    public bool ShouldApplySortFilterOrGroupLogs()
    {
        return RadGrid_gvLogs.MasterTableView.FilterExpression != "" ||
            (RadGrid_gvLogs.MasterTableView.GroupByExpressions.Count > 0 || isGroupLog) ||
            RadGrid_gvLogs.MasterTableView.SortExpressions.Count > 0;
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


    #endregion
    private void FillCustomerUnAppliedCredit(int filter)
    {
        DataSet ds = new DataSet();
        BL_Deposit obj = new BL_Deposit();

        List<GetCustomerUnAppliedCreditViewModel> _lstGetCustomerUnAppliedCredit = new List<GetCustomerUnAppliedCreditViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "ReceivePaymentAPI/AddReceivePayment_GetCustomerUnAppliedCredit";

            _GetCustomerUnAppliedCredit.ConnConfig = Session["config"].ToString();
            _GetCustomerUnAppliedCredit.userId = Convert.ToInt32(Session["userid"]);
            _GetCustomerUnAppliedCredit.filter = filter;

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetCustomerUnAppliedCredit);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetCustomerUnAppliedCredit = serializer.Deserialize<List<GetCustomerUnAppliedCreditViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetCustomerUnAppliedCreditViewModel>(_lstGetCustomerUnAppliedCredit);
        }
        else
        {
            ds = obj.GetCustomerUnAppliedCredit(Session["config"].ToString(), Convert.ToInt32(Session["userid"]), filter);
        }

        ddlCustomer.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string id = ds.Tables[0].Rows[i]["ID"].ToString();
                string desc = ds.Tables[0].Rows[i]["Name"].ToString();
                ddlCustomer.Items.Add(new ListItem(desc, id));
            }

        }

    }

    protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Invoices"] = null;
        txtAmount.Text = string.Format("{0:c}", 0);
        txtInvoice.Text = "";
        lblCustomerBalance.Text = string.Format("{0:c}", 0);
        if (Convert.ToInt32(ddlFilter.SelectedValue) == 1)
        {
            ddlCustomer.Visible = false;
            RadGrid_gvInvoice.DataSource = string.Empty;
            RadGrid_gvInvoice.Rebind();

            divCustomer.Attributes.Add("style", "display:block");
        }
        else
        {
            ddlCustomer.Visible = true;
            FillCustomerUnAppliedCredit(Convert.ToInt32(ddlFilter.SelectedValue));
            //ScriptManager.RegisterStartupScript(this, GetType(), "hiddenDiv", " $('#"+ divCustomer.ClientID + "').css('display', 'none');", true);
            divCustomer.Attributes.Add("style", "display:none");
            if (ddlCustomer.Items.Count > 0)
            {
                ddlCustomer.SelectedIndex = 0;
            }
            else
            {
                RadGrid_gvInvoice.DataSource = string.Empty;
                RadGrid_gvInvoice.Rebind();
            }

        }
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "ddl_changedCustomer();", true);
    }
    private void FillFilterOption()
    {
        ddlFilter.Items.Add(new ListItem("All", "1"));
        ddlFilter.Items.Add(new ListItem("Credit", "2"));
        ddlFilter.Items.Add(new ListItem("Credit & Invoice", "3"));
    }

    protected void lnkWriteOff_Click(object sender, EventArgs e)
    {

        // ScriptManager.RegisterStartupScript(this, Page.GetType(), "closeScript", "CloseWriteOffWindow();", true);
        try
        {







            BL_Contracts objBL_Contracts = new BL_Contracts();
            Contracts objProp_Contracts = new Contracts();
            User objPropUser = new User();
            int txtRef = Convert.ToInt32(txtInvoiceWriteOff.Text);
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.InvoiceID = txtRef;

            _GetInvoicesByID.ConnConfig = Session["config"].ToString();
            _GetInvoicesByID.InvoiceID = txtRef;

            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();

            ListGetInvoicesByID _lstGetInvoicesByID = new ListGetInvoicesByID();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "ReceivePaymentAPI/AddReceivePayment_GetInvoicesByID";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoicesByID);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetInvoicesByID = serializer.Deserialize<ListGetInvoicesByID>(_APIResponse.ResponseData);

                ds1 = _lstGetInvoicesByID.lstTable1.ToDataSet();
                ds2 = _lstGetInvoicesByID.lstTable2.ToDataSet();

                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                dt1 = ds1.Tables[0];
                dt2 = ds2.Tables[0];

                dt1.TableName = "Table1";
                dt2.TableName = "Table2";

                ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });
            }
            else
            {
                if (hdnIsCreditWriteOff.Value == "0")
                {
                    ds = objBL_Contracts.GetInvoicesByID(objProp_Contracts);
                    // ddlCode.Enabled = true;
                }
                else if (hdnIsCreditWriteOff.Value == "2")
                {
                    ds = objBL_Contracts.GetCreditByID(objProp_Contracts);
                    // ddlCode.Enabled = false;
                }
                else
                {
                    ds = objBL_Contracts.GetDepositByID(objProp_Contracts);
                }

            }

            txtWriteOffCust.Text = ds.Tables[0].Rows[0]["customername"].ToString();
            txtWriteOffLoc.Text = ds.Tables[0].Rows[0]["locname"].ToString();
            txtDescription.Text = "Write off from account '" + txtWriteOffCustID.Text + "' on " + DateTime.Today.ToString("MM/dd/yyyy") + " by " + Session["username"].ToString();


            if (ds.Tables[0].Columns.Contains("job"))
            {
                if (Convert.ToString(ds.Tables[0].Rows[0]["job"]) != "0")
                {
                    txtWriteOffProject.Text = ds.Tables[0].Rows[0]["job"].ToString() + "-" + ds.Tables[0].Rows[0]["JobDecs"].ToString();
                }
            }

            Customer objProp_Customer = new Customer();

            BL_Customer objBL_Customer = new BL_Customer();

            objProp_Customer.ConnConfig = Session["config"].ToString();

            if (ds.Tables[0].Columns.Contains("job") && !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["job"].ToString()))
            {
                objProp_Customer.ProjectJobID = Convert.ToInt32(ds.Tables[0].Rows[0]["job"]);
            }


            objProp_Customer.Type = "0";



            DataSet dsCode = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.Billingmodule = "";

            List<GetActiveBillingCodeViewModel> _lstGetActiveBillingCode = new List<GetActiveBillingCodeViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "ReceivePaymentAPI/AddReceivePayment_GetActiveBillingCode";

                _GetActiveBillingCode.ConnConfig = Session["config"].ToString();

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetActiveBillingCode);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetActiveBillingCode = serializer.Deserialize<List<GetActiveBillingCodeViewModel>>(_APIResponse.ResponseData);
                dsCode = CommonMethods.ToDataSet<GetActiveBillingCodeViewModel>(_lstGetActiveBillingCode);
            }
            else
            {
                dsCode = new BL_BillCodes().GetActiveBillingCode(Session["config"].ToString());
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("BillCode");

            DataRow dr = dt.NewRow();
            dr["ID"] = 0;
            dr["BillCode"] = "-Select-";
            dt.Rows.Add(dr);

            for (int i = 0; i < dsCode.Tables[0].Rows.Count; i++)
            {
                dr = dt.NewRow();
                dr["ID"] = dsCode.Tables[0].Rows[i]["id"];
                dr["BillCode"] = dsCode.Tables[0].Rows[i]["fDesc"];
                dt.Rows.Add(dr);
            }

            ddlCode.DataSource = dt;
            ddlCode.DataBind();
            BusinessEntity.User objProp_User = new BusinessEntity.User();
            DataSet dscontrol = new DataSet();
            objProp_User.ConnConfig = Session["config"].ToString();

            dscontrol = objBL_User.getControl(objProp_User);


            if (dscontrol.Tables[0].Rows.Count > 0)
            {
                if (dscontrol.Tables[0].Rows[0]["DefaultBillingCode"].ToString() != "")
                {
                    if (Convert.ToInt32(dscontrol.Tables[0].Rows[0]["DefaultBillingCode"].ToString()) == 0)
                    {
                        ddlCode.SelectedValue = "0";
                    }
                    else
                    {
                        ddlCode.SelectedValue = dscontrol.Tables[0].Rows[0]["DefaultBillingCode"].ToString();
                    }
                }
                else
                {
                    ddlCode.SelectedValue = "0";
                }
                //if (Convert.ToString(dscontrol.Tables[0].Rows[0]["DefaultBillingCodeDesc"].ToString()) != "")
                //{
                //    ddlCode.SelectedItem.Text = Convert.ToString(dscontrol.Tables[0].Rows[0]["DefaultBillingCodeDesc"].ToString());
                //}

            }

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "closeScript", "CloseWriteOffWindow();", true);
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);

        }


    }
    protected void lnkCredit_Click(object sender, EventArgs e)
    {

        // ScriptManager.RegisterStartupScript(this, Page.GetType(), "closeScript", "CloseWriteOffWindow();", true);
        try
        {

            BL_Contracts objBL_Contracts = new BL_Contracts();
            Contracts objProp_Contracts = new Contracts();
            User objPropUser = new User();
            int txtRef = Convert.ToInt32(hdnInvoiceCredit.Value);
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.InvoiceID = txtRef;

            _GetInvoicesByID.ConnConfig = Session["config"].ToString();
            _GetInvoicesByID.InvoiceID = txtRef;

            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();

            ListGetInvoicesByID _lstGetInvoicesByID = new ListGetInvoicesByID();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "ReceivePaymentAPI/AddReceivePayment_GetInvoicesByID";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoicesByID);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetInvoicesByID = serializer.Deserialize<ListGetInvoicesByID>(_APIResponse.ResponseData);

                ds1 = _lstGetInvoicesByID.lstTable1.ToDataSet();
                ds2 = _lstGetInvoicesByID.lstTable2.ToDataSet();

                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                dt1 = ds1.Tables[0];
                dt2 = ds2.Tables[0];

                dt1.TableName = "Table1";
                dt2.TableName = "Table2";

                ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });
            }
            else
            {
                if (hdnIsCreditWriteOff1.Value == "0")
                {
                    ds = objBL_Contracts.GetInvoicesByID(objProp_Contracts);
                    // ddlCode.Enabled = true;
                }
                else if (hdnIsCreditWriteOff1.Value == "2")
                {
                    ds = objBL_Contracts.GetCreditByID(objProp_Contracts);
                    // ddlCode.Enabled = false;
                }
                else
                {
                    ds = objBL_Contracts.GetDepositByID(objProp_Contracts);
                }

            }

            
            txtDescriptionCredit.Text = "Credit from Invoice# " + hdnInvoiceCredit.Value + " by " + Session["username"].ToString();


            

            Customer objProp_Customer = new Customer();

            BL_Customer objBL_Customer = new BL_Customer();

            objProp_Customer.ConnConfig = Session["config"].ToString();

            if (ds.Tables[0].Columns.Contains("job") && !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["job"].ToString()))
            {
                objProp_Customer.ProjectJobID = Convert.ToInt32(ds.Tables[0].Rows[0]["job"]);
                hdnJobIDCredit.Value = Convert.ToString(ds.Tables[0].Rows[0]["job"].ToString());
            }


            objProp_Customer.Type = "0";



        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "closeScript", "CloseCreditWindow();", true);
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);

        }


    }
    private DataTable CreateCreditInvTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Ref", typeof(int));
        dt.Columns.Add("line", typeof(int));
        dt.Columns.Add("Acct", typeof(int));
        dt.Columns.Add("Quan", typeof(double));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Price", typeof(double));
        dt.Columns.Add("Amount", typeof(double));
        dt.Columns.Add("STax", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("JobItem", typeof(int));
        dt.Columns.Add("TransID", typeof(int));
        dt.Columns.Add("Measure", typeof(string));
        dt.Columns.Add("Disc", typeof(double));
        dt.Columns.Add("StaxAmt", typeof(double));
        dt.Columns.Add("GSTAmt", typeof(double));
        dt.Columns.Add("Code", typeof(int));
        dt.Columns.Add("JobOrg", typeof(int));
        dt.Columns.Add("INVType", typeof(int));
        dt.Columns.Add("Warehouse", typeof(string));
        dt.Columns.Add("WHLocID", typeof(int));                
        if (Convert.ToBoolean(ViewState["isCanada"]))
        {
            dt.Columns.Add("EnableGSTTax", typeof(Boolean));
        }
        return dt;
    }
    protected void lnkSaveCredit_Click(object sender, EventArgs e)
    {
        try
        {

            Boolean isProjectClose = false;
            bool IsValid = true;
            DateTime receivedt;
            receivedt = Convert.ToDateTime(txtCreditDate.Text);
            GetPeriodDetails(receivedt);     //Check period closed out permission
            bool Flag = (bool)ViewState["FlagPeriodClose"];
            if (!Flag)
            {
                IsValid = false;
            }
            if (!string.IsNullOrEmpty(hdnJobIDCredit.Value.ToString()))
            {
                JobT objJob = new JobT();
                BL_Job objBL_Job = new BL_Job();
                objJob.ConnConfig = Session["config"].ToString();
                objJob.ID = Convert.ToInt32(hdnJobIDCredit.Value);

                DataSet dsJ = objBL_Job.GetJobById(objJob);
                if (dsJ != null && dsJ.Tables.Count > 0)
                {
                    if (dsJ.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dsJ.Tables[0].Rows[0]["Status"]) == 1)
                        {
                            isProjectClose = true;
                        }
                    }

                }
            }

            if (IsValid)
            {
                if (isProjectClose)
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "closeScript", "CloseCreditWindow();", true);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Please note this project is closed. You will need to change it before saving.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);                    
                }
                else
                {

                    //-------------------------------------------*******************************************
                    WriteOff obj = new WriteOff();
                    obj.ConnConfig = Session["config"].ToString();

                    //obj.Acct = Convert.ToInt32(ddlCode.SelectedItem.Value);
                    obj.Acct = Convert.ToInt32(0);
                    obj.Desc = txtDescriptionCredit.Text;
                    obj.fDate = txtCreditDate.Text == "" ? DateTime.Now : Convert.ToDateTime(txtCreditDate.Text);
                    obj.CreateBy = Session["username"].ToString();

                    obj.TransID = Convert.ToInt32(hdnTransIDCredit.Value);
                    obj.CheckNo = txtCheck.Text;
                    obj.WriteoffDesc = txtMemo.Text;
                    obj.ID = Convert.ToInt32(hdnInvoiceCredit.Value);
                    objBL_Deposit.writeOffInvoice(obj);
                    //--------------------------------------------------------------------------------------

                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "closeScript", "CloseCreditWindow();", true);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: 'Credit successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    BindGridInvoice();
                    RadGrid_gvInvoice.Rebind();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "closeScript", "CloseCreditWindow();", true);
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'These month/year period is closed out. You do not have permission to add/update this record.',  type : 'Warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);

        }

    }
    protected void lnkSaveWriteOff_Click(object sender, EventArgs e)
    {
        try
        {


        bool IsValid = true;
        //if (IsValid)
        //{
        DateTime receivedt;
        //if (Request.QueryString["id"] != null)
        //{
        //    receivedt = Convert.ToDateTime(hdnDate.Value);
        //}
        //else
        receivedt = Convert.ToDateTime(txtDate.Text);

        GetPeriodDetails(receivedt);     //Check period closed out permission
        bool Flag = (bool)ViewState["FlagPeriodClose"];
        if (!Flag)
        {
            IsValid = false;
        }

            if (IsValid)
            {



                WriteOff obj = new WriteOff();
                obj.ConnConfig = Session["config"].ToString();

                obj.Acct = Convert.ToInt32(ddlCode.SelectedItem.Value);
                obj.Desc = txtDescription.Text;
                obj.fDate = txtWriteOffDate.Text == "" ? DateTime.Now : Convert.ToDateTime(txtWriteOffDate.Text);
                obj.CreateBy = Session["username"].ToString();

                obj.TransID = Convert.ToInt32(hdnTransID.Value);
                obj.CheckNo = txtCheck.Text;
                obj.WriteoffDesc = txtMemo.Text;

                _writeOffInvoiceMulti.ConnConfig = Session["config"].ToString();
                _writeOffInvoiceMulti.Acct = Convert.ToInt32(ddlCode.SelectedItem.Value);
                _writeOffInvoiceMulti.Desc = txtDescription.Text;
                _writeOffInvoiceMulti.fDate = txtWriteOffDate.Text == "" ? DateTime.Now : Convert.ToDateTime(txtWriteOffDate.Text);
                _writeOffInvoiceMulti.CreateBy = Session["username"].ToString();
                _writeOffInvoiceMulti.CheckNo = txtCheck.Text;
                _writeOffInvoiceMulti.WriteoffDesc = txtMemo.Text;

                _writeOffInvoice.ConnConfig = Session["config"].ToString();
                _writeOffInvoice.Acct = Convert.ToInt32(ddlCode.SelectedItem.Value);
                _writeOffInvoice.Desc = txtDescription.Text;
                _writeOffInvoice.fDate = txtWriteOffDate.Text == "" ? DateTime.Now : Convert.ToDateTime(txtWriteOffDate.Text);
                _writeOffInvoice.CreateBy = Session["username"].ToString();
                _writeOffInvoice.CheckNo = txtCheck.Text;
                _writeOffInvoice.WriteoffDesc = txtMemo.Text;


                if (hdnIsWriteOff.Value == "1")
                {
                    obj.ListInvoice = txtInvoiceWriteOff.Text;
                    obj.WriteOffAmount = double.Parse(txtWriteOffAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                NumberStyles.AllowThousands |
                                                NumberStyles.AllowDecimalPoint); ;

                    _writeOffInvoiceMulti.ListInvoice = txtInvoiceWriteOff.Text;
                    _writeOffInvoiceMulti.WriteOffAmount = double.Parse(txtWriteOffAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                NumberStyles.AllowThousands |
                                                NumberStyles.AllowDecimalPoint);

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "ReceivePaymentAPI/AddReceivePayment_writeOffInvoiceMulti";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _writeOffInvoiceMulti);
                    }
                    else
                    {
                        objBL_Deposit.writeOffInvoiceMulti(obj);
                    }
                }
                else
                {
                    obj.ID = Convert.ToInt32(txtInvoiceWriteOff.Text);
                    _writeOffInvoice.ID = Convert.ToInt32(txtInvoiceWriteOff.Text);

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "ReceivePaymentAPI/AddReceivePayment_writeOffInvoice";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _writeOffInvoice);
                    }
                    else
                    {
                        if (hdnIsCreditWriteOff.Value == "0")
                        {
                            objBL_Deposit.writeOffInvoice(obj);
                        }
                        else if (hdnIsCreditWriteOff.Value == "1")
                        {
                            objBL_Deposit.writeOffDeposit(obj);
                        }
                        else
                        {
                            objBL_Deposit.writeOffCredit(obj);
                        }

                    }
                }

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "closeScript", "CloseWriteOffWindow();", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: 'Write off successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                BindGridInvoice();
                RadGrid_gvInvoice.Rebind();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "closeScript", "CloseWriteOffWindow();", true);
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'These month/year period is closed out. You do not have permission to add/update this record.',  type : 'Warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);

        }

    }
    public void BindGridInvoice()
    {
        try
        {
            objPayment.ConnConfig = Session["config"].ToString();
            _GetInvoicesByReceivedPay.ConnConfig = Session["config"].ToString();

            if (!string.IsNullOrEmpty(hdnLocID.Value))
            {
                objPayment.Loc = Convert.ToInt32(hdnLocID.Value);
                _GetInvoicesByReceivedPay.Loc = Convert.ToInt32(hdnLocID.Value);
            }
            else if (!string.IsNullOrEmpty(hdnCustID.Value))
            {
                objPayment.Rol = Convert.ToInt32(hdnCustID.Value);
                _GetInvoicesByReceivedPay.Rol = Convert.ToInt32(hdnCustID.Value);
            }
            objPropUser.DBName = Session["dbname"].ToString();
            _GetLocationByID.DBName = Session["dbname"].ToString();

            if (!string.IsNullOrEmpty(hdnLocID.Value))
            {
                objPropUser.LocID = Convert.ToInt32(hdnLocID.Value);
                _GetLocationByID.LocID = Convert.ToInt32(hdnLocID.Value);
            }

            objPropUser.ConnConfig = Session["config"].ToString();
            _GetLocationByID.ConnConfig = Session["config"].ToString();

            DataSet dsloc = new DataSet();
            DataSet dsloc1 = new DataSet();
            DataSet dsloc2 = new DataSet();
            DataSet dsloc3 = new DataSet();
            DataSet dsloc4 = new DataSet();
            DataSet dsloc5 = new DataSet();

            ListGetLocationByID _lstGetLocationByID = new ListGetLocationByID();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "ReceivePaymentAPI/AddReceivePayment_GetLocationByID";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetLocationByID);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetLocationByID = serializer.Deserialize<ListGetLocationByID>(_APIResponse.ResponseData);

                dsloc1 = _lstGetLocationByID.lstTable1.ToDataSet();
                dsloc2 = _lstGetLocationByID.lstTable2.ToDataSet();
                dsloc3 = _lstGetLocationByID.lstTable3.ToDataSet();
                dsloc4 = _lstGetLocationByID.lstTable4.ToDataSet();
                dsloc5 = _lstGetLocationByID.lstTable5.ToDataSet();

                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();
                DataTable dt4 = new DataTable();
                DataTable dt5 = new DataTable();

                dt1 = dsloc1.Tables[0];
                dt2 = dsloc2.Tables[0];
                dt3 = dsloc3.Tables[0];
                dt4 = dsloc4.Tables[0];
                dt5 = dsloc5.Tables[0];

                dt1.TableName = "Table1";
                dt2.TableName = "Table2";
                dt3.TableName = "Table3";
                dt4.TableName = "Table4";
                dt5.TableName = "Table5";

                dsloc.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy(), dt4.Copy(), dt5.Copy() });
            }
            else
            {
                dsloc = objBL_User.getLocationByID(objPropUser);
            }

            if (dsloc.Tables[0].Rows.Count > 0)
            {
                txtLocation.Text = dsloc.Tables[0].Rows[0]["tag"].ToString();
                txtCustomer.Text = dsloc.Tables[0].Rows[0]["custname"].ToString();
                hdnCustID.Value = dsloc.Tables[0].Rows[0]["owner"].ToString();
                txtCompany.Text = dsloc.Tables[0].Rows[0]["Company"].ToString();
            }

            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();

            ListGetInvoicesByReceivedPay _lstGetInvoicesByReceivedPay = new ListGetInvoicesByReceivedPay();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "ReceivePaymentAPI/AddReceivePayment_GetInvoicesByReceivedPay";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoicesByReceivedPay);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetInvoicesByReceivedPay = serializer.Deserialize<ListGetInvoicesByReceivedPay>(_APIResponse.ResponseData);

                ds1 = _lstGetInvoicesByReceivedPay.lstTable1.ToDataSet();
                DataTable dt1 = new DataTable();
                dt1 = ds1.Tables[0];
                dt1.TableName = "Table1";
                ds.Tables.AddRange(new DataTable[] { dt1.Copy() });


                if (_lstGetInvoicesByReceivedPay.lstTable2 != null)
                {
                    ds2 = _lstGetInvoicesByReceivedPay.lstTable2.ToDataSet();
                    DataTable dt2 = new DataTable();
                    dt2 = ds2.Tables[0];
                    dt2.TableName = "Table2";
                    ds.Tables.AddRange(new DataTable[] { dt2.Copy() });
                }
            }
            else
            {
                ds = objBL_Deposit.GetInvoicesByReceivedPay(objPayment);
            }
            DataTable filterdt = new DataTable();
            DataRow[] dr = ds.Tables[0].Select("fDate<='" + txtDate.Text + "'");
            if (dr.Length > 0)
            {
                filterdt = dr.CopyToDataTable();
            }
            else
            {
                filterdt = ds.Tables[0].Clone();
            }
            SetGridView(filterdt);
            //SetGridView(ds.Tables[0]);
            txtAmount.Text = string.Format("{0:c}", 0);
            lblCustomerBalance.Text = string.Format("{0:c}", ds.Tables[1].Rows[0]["Balance"]);
            if (hdTabIndex.Value == "1")
            {
                txtInvoice.Focus();
                txtInvoice.Text = "";
            }
            else
            {

                txtDate.Focus();
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void openWriteOffMulti()
    {
        double checkAmount = 0;
        double totalAmountSelected = 0;
        int invoiceID = 0;
        String lsInvoice = "";
        Boolean isFirst = true;
        int countInvoice = 0;
        checkAmount = double.Parse(txtAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                                       NumberStyles.AllowThousands |
                                                                       NumberStyles.AllowDecimalPoint);
        totalAmountSelected = double.Parse(hdnAmountSelected.Value.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                                       NumberStyles.AllowThousands |
                                                                       NumberStyles.AllowDecimalPoint);
        foreach (GridDataItem gr in RadGrid_gvInvoice.Items)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            if (chkSelect.Checked.Equals(true))
            {
                HiddenField hdnPrevDue = (HiddenField)gr.FindControl("hdnPrevDue");
                HiddenField lblIsCredit = (HiddenField)gr.FindControl("lblIsCredit");
                HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");
                if (lblIsCredit.Value != "1")
                {
                    countInvoice = countInvoice + 1;


                    if (isFirst)
                    {
                        invoiceID = Convert.ToInt32(hdnID.Value);

                        isFirst = false;
                    }


                }
                lsInvoice = lsInvoice + hdnID.Value + ",";
            }

        }
        if (countInvoice >= 1)
        {
            try
            {

                objProp_Contracts.ConnConfig = Session["config"].ToString();
                objProp_Contracts.InvoiceID = invoiceID;

                _GetInvoicesByID.ConnConfig = Session["config"].ToString();
                _GetInvoicesByID.InvoiceID = invoiceID;

                DataSet ds = new DataSet();
                DataSet ds1 = new DataSet();
                DataSet ds2 = new DataSet();

                ListGetInvoicesByID _lstGetInvoicesByID = new ListGetInvoicesByID();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "ReceivePaymentAPI/AddReceivePayment_GetInvoicesByID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoicesByID);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetInvoicesByID = serializer.Deserialize<ListGetInvoicesByID>(_APIResponse.ResponseData);

                    ds1 = _lstGetInvoicesByID.lstTable1.ToDataSet();
                    ds2 = _lstGetInvoicesByID.lstTable2.ToDataSet();

                    DataTable dt1 = new DataTable();
                    DataTable dt2 = new DataTable();

                    dt1 = ds1.Tables[0];
                    dt2 = ds2.Tables[0];

                    dt1.TableName = "Table1";
                    dt2.TableName = "Table2";

                    ds.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });
                }
                else
                {
                    ds = objBL_Contracts.GetInvoicesByID(objProp_Contracts);
                }

                txtInvoiceWriteOff.Text = lsInvoice.ToString();
                txtWriteOffAmount.Text = string.Format("{0:c}", totalAmountSelected - checkAmount);
                txtWriteOffCust.Text = ds.Tables[0].Rows[0]["customername"].ToString();
                txtWriteOffLoc.Text = ds.Tables[0].Rows[0]["locname"].ToString();
                txtDescription.Text = "Write off from account '" + txtWriteOffCustID.Text + "' on " + DateTime.Today.ToString("MM/dd/yyyy") + " by " + Session["username"].ToString();

                if (Convert.ToString(ds.Tables[0].Rows[0]["job"]) != "0" && countInvoice == 1)
                {
                    txtWriteOffProject.Text = ds.Tables[0].Rows[0]["job"].ToString() + "-" + ds.Tables[0].Rows[0]["JobDecs"].ToString();

                }
                else
                {
                    txtWriteOffProject.Text = "";
                }
                DataSet dsCode = new DataSet();
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.Billingmodule = "";

                //dsCode = new BL_BillCodes().GetAutoCompleteBillCodes(objPropUser, "");

                List<GetActiveBillingCodeViewModel> _lstGetActiveBillingCode = new List<GetActiveBillingCodeViewModel>();

                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "ReceivePaymentAPI/AddReceivePayment_GetActiveBillingCode";

                    _GetActiveBillingCode.ConnConfig = Session["config"].ToString();

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetActiveBillingCode);

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    serializer.MaxJsonLength = Int32.MaxValue;

                    _lstGetActiveBillingCode = serializer.Deserialize<List<GetActiveBillingCodeViewModel>>(_APIResponse.ResponseData);
                    dsCode = CommonMethods.ToDataSet<GetActiveBillingCodeViewModel>(_lstGetActiveBillingCode);
                }
                else
                {
                    dsCode = new BL_BillCodes().GetActiveBillingCode(Session["config"].ToString());
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("BillCode");

                DataRow dr = dt.NewRow();
                dr["ID"] = 0;
                dr["BillCode"] = "-Select-";
                dt.Rows.Add(dr);

                for (int i = 0; i < dsCode.Tables[0].Rows.Count; i++)
                {
                    dr = dt.NewRow();
                    dr["ID"] = dsCode.Tables[0].Rows[i]["id"];
                    dr["BillCode"] = dsCode.Tables[0].Rows[i]["fDesc"];
                    dt.Rows.Add(dr);
                }

                ddlCode.DataSource = dt;
                ddlCode.DataBind();

                

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "closeScript", "CloseWriteOffWindow();", true);
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);

            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "closeScript", "CloseWriteOffWindow();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: 'Please select a invoice', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkTranfer_Click(object sender, EventArgs e)
    {
        //GetLocationList();
    }

    protected void lnkSaveTransfer_Click(object sender, EventArgs e)
    {
        String lsInvoice = "";
        int locID = (hdnNewLocID.Value == "") ? 0 : Convert.ToInt32(hdnNewLocID.Value);
        String conn = Session["config"].ToString();
        Boolean isFirst = true;
        int invoiceID = 0;
        try
        {
            foreach (GridDataItem gr in RadGrid_gvInvoice.Items)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                if (chkSelect.Checked.Equals(true))
                {

                    HiddenField lblIsCredit = (HiddenField)gr.FindControl("lblIsCredit");
                    HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");
                    if (lblIsCredit.Value == "1")
                    {
                        if (isFirst)
                        {
                            invoiceID = Convert.ToInt32(hdnID.Value);
                            isFirst = false;
                        }
                    }
                    lsInvoice = lsInvoice + hdnID.Value + ";";
                }

            }

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "ReceivePaymentAPI/AddReceivePayment_TransferPayment";

                _TransferPayment.ConnConfig = conn;
                _TransferPayment.strRef = lsInvoice;
                _TransferPayment.newLoc = locID;

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _TransferPayment);
            }
            else
            {
                objBL_Deposit.TransferPayment(conn, lsInvoice, locID);
            }

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "closeScript", "CloseTransferPaymentWindow();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: 'Transfer payment successfully', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            BindGridInvoice();
            RadGrid_gvInvoice.Rebind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccAddProstype1", "noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);

        }

    }

    protected void lnkUnApply_Click(object sender, EventArgs e)
    {
        String conn = Session["config"].ToString();
        try
        {

            if (Request.QueryString["id"] != null)
            {
                if (IsAPIIntegrationEnable == "YES")
                {
                    string APINAME = "ReceivePaymentAPI/AddReceivePayment_UnapplyPayment";

                    _UnapplyPayment.ConnConfig = conn;
                    _UnapplyPayment.Ref = Convert.ToInt32(Request.QueryString["id"]);

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UnapplyPayment);
                }
                else
                {
                    objBL_Deposit.UnapplyPayment(conn, Convert.ToInt32(Request.QueryString["id"]), Session["User"].ToString());
                }
            }
            SetDataForEdit();
            RadGrid_gvLogs.Rebind();
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Unapply payment successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            string errortype = "error";

            if (str.Contains("This receive payment has applied for multi locations and can therefore not be unapplied.") || str.Contains("This receive payment has applied and can therefore not be unapplied."))
            {
                errortype = "warning";
            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "LocInactive", "noty({text: '" + str + "',  type : '" + errortype + "', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);


        }


    }

    private void processSelectLoc()
    {
        try
        {
            objPayment.ConnConfig = Session["config"].ToString();
            _GetInvoicesByReceivedPay.ConnConfig = Session["config"].ToString();
            if (!string.IsNullOrEmpty(hdnLocID.Value))
            {
                objPayment.Loc = Convert.ToInt32(hdnLocID.Value);
                _GetInvoicesByReceivedPay.Loc = Convert.ToInt32(hdnLocID.Value);
            }
            else if (!string.IsNullOrEmpty(hdnCustID.Value))
            {
                objPayment.Rol = Convert.ToInt32(hdnCustID.Value);
                _GetInvoicesByReceivedPay.Rol = Convert.ToInt32(hdnCustID.Value);
            }
            objPropUser.DBName = Session["dbname"].ToString();
            _GetLocationByID.DBName = Session["dbname"].ToString();

            if (!string.IsNullOrEmpty(hdnLocID.Value))
            {
                objPropUser.LocID = Convert.ToInt32(hdnLocID.Value);
                _GetLocationByID.LocID = Convert.ToInt32(hdnLocID.Value);
            }

            objPropUser.ConnConfig = Session["config"].ToString();
            _GetLocationByID.ConnConfig = Session["config"].ToString();

            DataSet dsloc = new DataSet();
            DataSet dsloc1 = new DataSet();
            DataSet dsloc2 = new DataSet();
            DataSet dsloc3 = new DataSet();
            DataSet dsloc4 = new DataSet();
            DataSet dsloc5 = new DataSet();

            ListGetLocationByID _lstGetLocationByID = new ListGetLocationByID();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "ReceivePaymentAPI/AddReceivePayment_GetLocationByID";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetLocationByID);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetLocationByID = serializer.Deserialize<ListGetLocationByID>(_APIResponse.ResponseData);

                dsloc1 = _lstGetLocationByID.lstTable1.ToDataSet();
                dsloc2 = _lstGetLocationByID.lstTable2.ToDataSet();
                dsloc3 = _lstGetLocationByID.lstTable3.ToDataSet();
                dsloc4 = _lstGetLocationByID.lstTable4.ToDataSet();
                dsloc5 = _lstGetLocationByID.lstTable5.ToDataSet();

                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();
                DataTable dt4 = new DataTable();
                DataTable dt5 = new DataTable();

                dt1 = dsloc1.Tables[0];
                dt2 = dsloc2.Tables[0];
                dt3 = dsloc3.Tables[0];
                dt4 = dsloc4.Tables[0];
                dt5 = dsloc5.Tables[0];

                dt1.TableName = "Table1";
                dt2.TableName = "Table2";
                dt3.TableName = "Table3";
                dt4.TableName = "Table4";
                dt5.TableName = "Table5";

                dsloc.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy(), dt3.Copy(), dt4.Copy(), dt5.Copy() });
            }
            else
            {
                dsloc = objBL_User.getLocationByID(objPropUser);
            }

            if (dsloc.Tables[0].Rows.Count > 0)
            {
                txtLocation.Text = dsloc.Tables[0].Rows[0]["tag"].ToString();
                txtCustomer.Text = dsloc.Tables[0].Rows[0]["custname"].ToString();
                hdnCustID.Value = dsloc.Tables[0].Rows[0]["owner"].ToString();
                txtCompany.Text = dsloc.Tables[0].Rows[0]["Company"].ToString();
            }

            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();

            ListGetInvoicesByReceivedPay _lstGetInvoicesByReceivedPay = new ListGetInvoicesByReceivedPay();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "ReceivePaymentAPI/AddReceivePayment_GetInvoicesByReceivedPay";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoicesByReceivedPay);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetInvoicesByReceivedPay = serializer.Deserialize<ListGetInvoicesByReceivedPay>(_APIResponse.ResponseData);

                ds1 = _lstGetInvoicesByReceivedPay.lstTable1.ToDataSet();
                DataTable dt1 = new DataTable();
                dt1 = ds1.Tables[0];
                dt1.TableName = "Table1";
                ds.Tables.AddRange(new DataTable[] { dt1.Copy() });


                if (_lstGetInvoicesByReceivedPay.lstTable2 != null)
                {
                    ds2 = _lstGetInvoicesByReceivedPay.lstTable2.ToDataSet();
                    DataTable dt2 = new DataTable();
                    dt2 = ds2.Tables[0];
                    dt2.TableName = "Table2";
                    ds.Tables.AddRange(new DataTable[] { dt2.Copy() });
                }
            }
            else
            {
                ds = objBL_Deposit.GetInvoicesByReceivedPay(objPayment);
            }
            DataTable filterdt = new DataTable();
            DataRow[] dr = ds.Tables[0].Select("fDate<='" + txtDate.Text + "'");
            if (dr.Length > 0)
            {
                filterdt = dr.CopyToDataTable();
            }
            else
            {
                filterdt = ds.Tables[0].Clone();
            }
            SetGridView(filterdt);
            //SetGridView(ds.Tables[0]);
            txtAmount.Text = string.Format("{0:c}", 0);
            lblCustomerBalance.Text = string.Format("{0:c}", ds.Tables[1].Rows[0]["Balance"]);
            if (hdTabIndex.Value == "1")
            {
                txtInvoice.Focus();
                txtInvoice.Text = "";
            }
            else
            {
                txtDate.Focus();
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void RadGrid_Location_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (hdnCustID.Value != "" && hdnCustID.Value != "0")
        {
            objPropUser.CustomerID = Convert.ToInt32(hdnCustID.Value);          
            objPropUser.ConnConfig = Session["config"].ToString();
            DataSet ds = objBL_User.getLocationActiveByCustomerID(objPropUser, new GeneralFunctions().GetSalesAsigned());
            RadGrid_Location.DataSource = ds.Tables[0];
        }

    }

    protected void lnkPopupOK_Click(object sender, EventArgs e)
    {
        int locCredit = 0;
        foreach (GridDataItem gr in RadGrid_Location.Items)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkLocationSelectSelectCheckBox");
            HiddenField hdnLocSelected = (HiddenField)gr.FindControl("hdnLocSelected");
            if (chkSelect.Checked == true)
            {
                locCredit = Convert.ToInt32( hdnLocSelected.Value);
            }
        }


        if (Page.IsValid)
        { //variable added to check if any line item checked

            bool IsValid = true;          
            DateTime receivedt;           
            receivedt = Convert.ToDateTime(txtDate.Text);

            GetPeriodDetails(receivedt);     //Check period closed out permission
            bool Flag = (bool)ViewState["FlagPeriodClose"];
            if (!Flag)
            {
                IsValid = false;
            }

            if (IsValid)
            {
                divSuccess.Visible = false;
                double totalPay = 0.00;
                double totalDue = 0.00;

                CreateDatatable();
                DataTable dt = (DataTable)ViewState["ReceivPay"];
                DataTable dtReceive = dt.Clone();
                objReceivePay.ConnConfig = Session["config"].ToString();
                objReceivePay.ID = Convert.ToInt32(Request.QueryString["id"]);
                objReceivePay.Rol = Convert.ToInt32(hdnCustID.Value);

                _UpdateReceivePayment.ConnConfig = Session["config"].ToString();
                _UpdateReceivePayment.ID = Convert.ToInt32(Request.QueryString["id"]);

                _AddReceivePayment.ConnConfig = Session["config"].ToString();
                _AddReceivePayment.Rol = Convert.ToInt32(hdnCustID.Value);

                if (!string.IsNullOrEmpty(hdnLocID.Value))
                {
                    objReceivePay.Loc = Convert.ToInt32(hdnLocID.Value);
                    _UpdateReceivePayment.Loc = Convert.ToInt32(hdnLocID.Value);
                    _AddReceivePayment.Loc = Convert.ToInt32(hdnLocID.Value);
                }
                else
                {
                    objReceivePay.Loc = 0;
                    _UpdateReceivePayment.Loc = 0;
                    _AddReceivePayment.Loc = 0;
                }

                objReceivePay.PaymentReceivedDate = Convert.ToDateTime(txtDate.Text);
                objReceivePay.PaymentMethod = Convert.ToInt16(ddlPayment.SelectedValue);
                objReceivePay.CheckNumber = txtCheck.Text.Trim();
                objReceivePay.fDesc = txtMemo.Text;
                objReceivePay.MOMUSer = Session["User"].ToString();

                _UpdateReceivePayment.PaymentReceivedDate = Convert.ToDateTime(txtDate.Text);
                _UpdateReceivePayment.PaymentMethod = Convert.ToInt16(ddlPayment.SelectedValue);
                _UpdateReceivePayment.CheckNumber = txtCheck.Text.Trim();
                _UpdateReceivePayment.fDesc = txtMemo.Text;
                _UpdateReceivePayment.MOMUSer = Session["User"].ToString();

                _AddReceivePayment.PaymentReceivedDate = Convert.ToDateTime(txtDate.Text);
                _AddReceivePayment.PaymentMethod = Convert.ToInt16(ddlPayment.SelectedValue);
                _AddReceivePayment.CheckNumber = txtCheck.Text.Trim();
                _AddReceivePayment.fDesc = txtMemo.Text;
                _AddReceivePayment.MOMUSer = Session["User"].ToString();

                foreach (GridDataItem gr in RadGrid_gvInvoice.Items)
                {
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

                    if (chkSelect.Checked.Equals(true) || chkSelect.Enabled == false)
                    {
                        HiddenField hdnType = (HiddenField)gr.FindControl("hdnType");
                        HiddenField hdnPaymentID = (HiddenField)gr.FindControl("hdnPaymentID");
                        HiddenField hdnTransID = (HiddenField)gr.FindControl("hdnTransID");
                        HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");
                        Label lblLoc = (Label)gr.FindControl("lblLoc");
                        HiddenField lblIsCredit = (HiddenField)gr.FindControl("lblIsCredit");
                        Label lblOrigAmount = (Label)gr.FindControl("lblOrigAmount");
                        Label lblDueAmount = (Label)gr.FindControl("lblDueAmount");
                        TextBox txtPAmount = (TextBox)gr.FindControl("txtPAmount");
                        HiddenField hdPAmount = (HiddenField)gr.FindControl("hdPAmount");
                        HiddenField hdnPrevDue = (HiddenField)gr.FindControl("hdnPrevDue");
                        HiddenField hdnRefTranID = (HiddenField)gr.FindControl("hdnRefTranID");
                        Label lblType = (Label)gr.FindControl("lblType");
                        double pay = 0;
                        if (Request.QueryString["id"] != null && hdnPaymentID.Value != "0")
                        {
                            pay = double.Parse(hdPAmount.Value.Replace('$', '0'), NumberStyles.AllowParentheses |
                                    NumberStyles.AllowThousands |
                                    NumberStyles.AllowDecimalPoint);
                        }
                        else
                        {
                            pay = double.Parse(txtPAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                     NumberStyles.AllowThousands |
                                     NumberStyles.AllowDecimalPoint);
                        }
                       

                        double orig = double.Parse(lblOrigAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                        NumberStyles.AllowThousands |
                                        NumberStyles.AllowDecimalPoint);
                        double due = double.Parse(lblDueAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                        NumberStyles.AllowThousands |
                                        NumberStyles.AllowDecimalPoint);
                        double prevDue = double.Parse(hdnPrevDue.Value);    // actual due amount = Previous due amount


                        due = double.Parse(lblDueAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                        NumberStyles.AllowThousands |
                                        NumberStyles.AllowDecimalPoint);      // calculated due amount

                        totalPay = +(totalPay + pay);
                        totalDue = totalDue + prevDue;

                        #region invoice status

                        DataRow dr = dtReceive.NewRow();
                        dr["InvoiceID"] = Convert.ToInt32(hdnID.Value);
                        bool IsNeg = false;
                        if (pay < 0)
                        {
                            IsNeg = true;
                            pay = pay * -1;
                            due = due * -1;
                            prevDue = prevDue * -1;
                        }
                        if (pay < due)
                        {
                            dr["Status"] = 3;           // partial payment : invoice
                        }
                        else if (prevDue.Equals(pay))
                        {
                            dr["Status"] = 1;           // paid : invoice
                        }
                        else
                        {
                            if (pay < prevDue)
                            {
                                dr["Status"] = 3;
                            }
                         
                        }
                        if (IsNeg)
                        {
                            pay = pay * -1;
                            due = due * -1;
                            prevDue = prevDue * -1;
                        }

                        dr["PayAmount"] = pay;
                        if (lblIsCredit.Value.Equals("1"))
                        {
                            dr["IsCredit"] = 1;
                        }
                        else if (lblIsCredit.Value.Equals("3"))
                        {
                            dr["IsCredit"] = 3;
                        }
                        else if (lblIsCredit.Value.Equals("2") && hdnType.Value.Equals("3"))
                        {
                            dr["IsCredit"] = 3;
                        }
                        else
                        {
                            dr["IsCredit"] = 0;
                        }

                        #endregion
                        dr["Loc"] = Convert.ToInt32(lblLoc.Text);
                        dr["Type"] = 0;
                        dr["RefTranID"] = hdnRefTranID.Value;
                        if (hdnType.Value != "")
                        {
                            dr["Type"] = Convert.ToInt16(hdnType.Value);
                        }

                        dtReceive.Rows.Add(dr);
                    }
                 
                }
               
                objReceivePay.Amount = double.Parse(txtAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                                    NumberStyles.AllowThousands |
                                                                    NumberStyles.AllowDecimalPoint);
                objReceivePay.AmountDue = totalDue - totalPay;

                objReceivePay.DtPay = dtReceive;


                _UpdateReceivePayment.Amount = double.Parse(txtAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                                    NumberStyles.AllowThousands |
                                                                    NumberStyles.AllowDecimalPoint);
                _UpdateReceivePayment.AmountDue = totalDue - totalPay;

                _AddReceivePayment.Amount = double.Parse(txtAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                                    NumberStyles.AllowThousands |
                                                                    NumberStyles.AllowDecimalPoint);
                _AddReceivePayment.AmountDue = totalDue - totalPay;

                if (dtReceive.Rows.Count == 0)
                {
                    DataTable returnVal = EmptydtReceive();
                    _UpdateReceivePayment.DtPay = returnVal;
                    _AddReceivePayment.DtPay = returnVal;
                }
                else
                {
                    _UpdateReceivePayment.DtPay = dtReceive;
                    _AddReceivePayment.DtPay = dtReceive;
                }

                if (Request.QueryString["id"] != null)
                {
                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "ReceivePaymentAPI/AddReceivePayment_UpdateReceivePayment";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _UpdateReceivePayment);
                    }
                    else
                    {
                        objBL_Deposit.UpdateReceivePayment(objReceivePay, locCredit);
                    }

                    //SetDataForEdit();
                    //RadGrid_gvLogs.Rebind();
                    Session["receMsg"] = "UpdateSuccess";
                    Response.Redirect("addreceivepayment.aspx?id=" + Request.QueryString["id"]);

                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Received payment Updated Successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
                else
                {
                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "ReceivePaymentAPI/AddReceivePayment_AddReceivePayment";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _AddReceivePayment);
                    }
                    else
                    {
                        objBL_Deposit.AddReceivePayment(objReceivePay, locCredit);
                    }

                    RadGrid_gvInvoice.DataSource = string.Empty;
                    RadGrid_gvInvoice.Rebind();

                    RadGrid_gvLogs.Rebind();
                    ViewState["Invoices"] = null;
                    Session["receMsg"] = "success";
                    if (Request.QueryString["page"] != null && Request.QueryString["uid"] != null)
                    {
                        Response.Redirect("addreceivepayment?page=" + Request.QueryString["page"] + "&uid=" + Request.QueryString["uid"]);
                    }
                    else
                    {
                        Response.Redirect("addreceivepayment.aspx");
                    }
                }
              
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'These month/year period is closed out. You do not have permission to add/update this record.',  type : 'Warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
               
            }
        }
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "CloseLocationsWindow", "CloseLocationsWindow();", true);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Received payment added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
    }
    public Boolean isCanadaCompany()
    {
        General _objPropGeneral = new General();
        BL_General _objBLGeneral = new BL_General();
        Boolean flag = false;
        _objPropGeneral.ConnConfig = Session["config"].ToString();
        DataSet _dsCustom = _objBLGeneral.getCompanyCountry(_objPropGeneral);
        try
        {
            if (_dsCustom.Tables[0].Rows[0]["Country"].ToString() == "Canada")
            {
                flag = true;
            }
        }
        catch (Exception ex)
        {
            flag = false;
        }
        return flag;
    }
}