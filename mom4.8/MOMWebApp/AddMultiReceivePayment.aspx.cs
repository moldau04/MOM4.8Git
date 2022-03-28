using AjaxControlToolkit;
using BusinessEntity;
using BusinessEntity.APModels;
using BusinessEntity.CustomersModel;
using BusinessEntity.Utility;
using BusinessLayer;
using MOMWebApp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class AddMultiReceivePayment : System.Web.UI.Page
{
    #region Variables   
    Bank _objBank = new Bank();
    BL_BankAccount _objBL_Bank = new BL_BankAccount();

    //API Variables 
    string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    GetAllBankNamesParam _GetAllBankNames = new GetAllBankNamesParam();
    AddBatchReceivePaymentParam _AddBatchReceivePayment = new AddBatchReceivePaymentParam();
    GetDepByIDParam _GetDepByID = new GetDepByIDParam();
    GetDepHeadByIDParam _GetDepHeadByID = new GetDepHeadByIDParam();
    GetReceivedPaymentByDepParam _GetReceivedPaymentByDep = new GetReceivedPaymentByDepParam();
    GetInvoiceByListParam _GetInvoiceByList = new GetInvoiceByListParam();
    GetInvoicesByReceivedPayMultiParam _GetInvoicesByReceivedPayMulti = new GetInvoicesByReceivedPayMultiParam();
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
            if (!IsPostBack)
            {
                FillBank();
                FillPayment();
                txtDate.Text = DateTime.Today.ToShortDateString();
            }
            Permission();


            if (Session["AddBatchSuccess"] != null)
            {
                Session["AddBatchSuccess"] = null;
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Received payment Added Successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                if (Session["AddBatchDep"] != null)
                {
                    if (Convert.ToInt32(Session["AddBatchDep"]) != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Deposit Added Successfully! <BR/>Ref# " + Session["AddBatchDep"].ToString() + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                    }
                    Session["AddBatchDep"] = null;
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    private void Permission()
    {

        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
        }

        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx");

        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
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
        ddlPayment.Items.Add(new ListItem("Lockbox", "6"));

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
                string APINAME = "ReceivePaymentAPI/AddMultiReceivePayment_GetAllBankNames";

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
    #endregion

    #endregion
    #region control
    public int submit()
    {
        int depId = 0;
        try
        {
            CreateDatatable();
            DataTable dt = (DataTable)ViewState["ReceivPay"];
            DataTable dtReceive = dt.Clone();
            foreach (GridDataItem gr in RadGrid_gvInvoice.Items)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

                if (chkSelect.Checked.Equals(true))
                {
                    HiddenField hdnOrderNo = (HiddenField)gr.FindControl("hdnOrderNo");
                    HiddenField hdnOwner = (HiddenField)gr.FindControl("hdnOwner");
                    TextBox txtOwnerName = (TextBox)gr.FindControl("txtOwnerName");
                    HiddenField hdnLocID = (HiddenField)gr.FindControl("hdnLocID");
                    TextBox txtLocationName = (TextBox)gr.FindControl("txtLocationName");
                    HiddenField hdnSTax = (HiddenField)gr.FindControl("hdnSTax");
                    HiddenField hdnTotal = (HiddenField)gr.FindControl("hdnTotal");
                    HiddenField hdnAmount = (HiddenField)gr.FindControl("hdnAmount");
                    HiddenField hdnPrevDue = (HiddenField)gr.FindControl("hdnPrevDue");
                    TextBox txtPAmount = (TextBox)gr.FindControl("txtPAmount");
                    TextBox txtInvoice = (TextBox)gr.FindControl("txtInvoice");
                    TextBox txt_gCheck = (TextBox)gr.FindControl("txt_gCheck");
                    HiddenField hdnBatchReceive = (HiddenField)gr.FindControl("hdnBatchReceive");
                    HiddenField hdnRefTranID = (HiddenField)gr.FindControl("hdnRefTranID");
                    if (txtPAmount.Text.ToString().Trim() != "$0.00")
                    {
                        DataRow dr = dtReceive.NewRow();

                        dr["Owner"] = (hdnOwner.Value == "") ? 0 : Convert.ToInt32(hdnOwner.Value);
                        dr["OwnerName"] = txtOwnerName.Text;
                        dr["LocID"] = hdnLocID.Value;
                        dr["LocationName"] = txtLocationName.Text;

                        dr["STax"] = double.Parse(hdnSTax.Value.Replace('$', '0'), NumberStyles.AllowParentheses |
                                   NumberStyles.AllowThousands |
                                   NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                        dr["Total"] = double.Parse(hdnTotal.Value.Replace('$', '0'), NumberStyles.AllowParentheses |
                                   NumberStyles.AllowThousands |
                                   NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                        dr["Amount"] = double.Parse(hdnAmount.Value.Replace('$', '0'), NumberStyles.AllowParentheses |
                                     NumberStyles.AllowThousands |
                                     NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                        dr["AmountDue"] = double.Parse(hdnPrevDue.Value.Replace('$', '0'), NumberStyles.AllowParentheses |
                                     NumberStyles.AllowThousands |
                                     NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                        dr["paymentAmt"] = double.Parse(txtPAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                     NumberStyles.AllowThousands |
                                     NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);

                        dr["Invoice"] = txtInvoice.Text;
                        dr["CheckNumber"] = txt_gCheck.Text;
                        dr["BatchReceive"] = hdnBatchReceive.Value;
                        dr["isChecked"] = 1;
                        dr["RefTranID"] = hdnRefTranID.Value;
                        dtReceive.Rows.Add(dr);
                    }


                }
            }

            if (dtReceive.Rows.Count > 0)
            {
                BL_Deposit objBL_Deposit = new BL_Deposit();
                ReceivedPayment objReceivePay = new ReceivedPayment();
                objReceivePay.DtPay = dtReceive;
                objReceivePay.MOMUSer = Session["User"].ToString();
                objReceivePay.PaymentReceivedDate = Convert.ToDateTime(txtDate.Text);
                objReceivePay.PaymentMethod = Convert.ToInt16(ddlPayment.SelectedValue);
                objReceivePay.ConnConfig = Session["config"].ToString();

                DataSet dsResult = new DataSet();
                dsResult = objBL_Deposit.AddBatchReceivePayment(objReceivePay, Convert.ToInt32(ddlBank.SelectedValue), Convert.ToBoolean(chkCreateDeposit.Checked));
                depId = Convert.ToInt32(dsResult.Tables[0].Rows[0]["DepId"]);

                //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Received payment Added Successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                //ResetForm();
                //txtDate.Text = DateTime.Today.ToShortDateString();

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return -1;
        }
        return depId;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int depId;
        try
        {
            if (Confirm_Value.Value == "Yes")
            {
                depId = submit();
                if (chkCreateDeposit.Checked == true && depId > 0)
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Deposit Added Successfully! <BR/>Ref# " + depId + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                    BL_Deposit _objBL_Deposit = new BL_Deposit();
                    Dep _objDep = new Dep();
                    _objDep.Ref = depId;
                    _objDep.ConnConfig = Session["config"].ToString();

                    _GetDepByID.Ref = depId;
                    _GetDepByID.ConnConfig = Session["config"].ToString();

                    _GetDepHeadByID.Ref = depId;
                    _GetDepHeadByID.ConnConfig = Session["config"].ToString();

                    DataSet _dsDep = new DataSet();
                    List<GetDepByIDViewModel> _lstGetDepByID = new List<GetDepByIDViewModel>();

                    if (IsAPIIntegrationEnable == "YES")
                    {
                        string APINAME = "ReceivePaymentAPI/AddMultiReceivePayment_GetDepByID";

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
                        string APINAME = "ReceivePaymentAPI/AddMultiReceivePayment_GetDepHeadByID";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetDepHeadByID);

                        _lstGetDepHeadByID = (new JavaScriptSerializer()).Deserialize<List<GetDepHeadByIDViewModel>>(_APIResponse.ResponseData);
                        _sessionds = CommonMethods.ToDataSet<GetDepHeadByIDViewModel>(_lstGetDepHeadByID);
                    }
                    else
                    {
                        _sessionds = _objBL_Deposit.GetDepHeadByID(_objDep);
                    }

                    Session["DepositHead"] = _sessionds;
                    Session["RefNo"] = depId.ToString();
                    if (_dsDep.Tables[0].Columns.Contains("Ref"))
                    {
                        SetDeposit(_dsDep.Tables[0], depId);
                        //Response.Redirect("DepositReport.aspx");
                        Response.Redirect("DepositSlipReport.aspx?uid=" + depId);

                    }
                }

            }
            else
            {
                depId = submit();
                //if (chkCreateDeposit.Checked == true)
                //{
                Session["AddBatchSuccess"] = true;
                Session["AddBatchDep"] = depId;
                Response.Redirect("addMultiReceivepayment.aspx");
                //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Deposit Added Successfully! <BR/>Ref# " + depId + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                // }
            }
        }
        catch (Exception)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'There is an error. Please try again later.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }




    }
    private void SetDeposit(DataTable dt, int depid)
    {
        try
        {
            BL_Deposit _objBL_Deposit = new BL_Deposit();
            ReceivedPayment _objReceivePay = new ReceivedPayment();
            _objReceivePay.ConnConfig = Session["config"].ToString();
            _GetReceivedPaymentByDep.ConnConfig = Session["config"].ToString();

            if (depid == 0)
            {
                _objReceivePay.DepID = Convert.ToInt32(Request.QueryString["id"]);
                _GetReceivedPaymentByDep.DepID = Convert.ToInt32(Request.QueryString["id"]);
            }
            else
            {
                _objReceivePay.DepID = depid;
                _GetReceivedPaymentByDep.DepID = depid;
            }

            DataSet _dsDep = new DataSet();
            DataSet _dsDep1 = new DataSet();
            DataSet _dsDep2 = new DataSet();
            ListGetReceivedPaymentByDep _lstGetReceivedPaymentByDep = new ListGetReceivedPaymentByDep();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "ReceivePaymentAPI/AddMultiReceivePayment_GetReceivedPaymentByDep";

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
    private void ResetForm()
    {
        ViewState["Invoices"] = null;
        ViewState["ReceivPay"] = null;
        RadGrid_gvInvoice.DataSource = string.Empty;
        RadGrid_gvInvoice.Rebind();
        txtlsInvoice.Text = "";
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("receivepayment.aspx");
    }

    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        getDataOnTestCustomGrid();
        String strInvoice = txtlsInvoice.Text;
        String checkNumber = txtCheck.Text;
        BL_Deposit objBL_Deposit = new BL_Deposit();

        DataTable dt = new DataTable();
        DataSet ds = new DataSet();


        List<String> lsInvoice = new List<string>();
        List<String> lsSearch = new List<string>();
        if (ViewState["Invoices"] != null)
        {
            DataTable root = (DataTable)ViewState["Invoices"];
            foreach (DataRow row in root.Rows)
            {
                lsInvoice.AddRange(row["Invoice"].ToString().Split(',').ToList());
            }
            List<String> temp = strInvoice.Split(',').ToList();

            lsSearch = temp.Where(x => !lsInvoice.Contains(x)).ToList();

            List<GetInvoiceByListViewModel> _lstGetInvoiceByList = new List<GetInvoiceByListViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "ReceivePaymentAPI/AddMultiReceivePayment_GetInvoiceByList";

                _GetInvoiceByList.ConnConfig = HttpContext.Current.Session["config"].ToString();
                _GetInvoiceByList.invoiceId = String.Join(",", lsSearch.ToArray());
                _GetInvoiceByList.checkNumber = checkNumber;
                _GetInvoiceByList.isSeparate = chkSeparateInvoice.Checked;

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoiceByList);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetInvoiceByList = serializer.Deserialize<List<GetInvoiceByListViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetInvoiceByListViewModel>(_lstGetInvoiceByList);
            }
            else
            {
                ds = objBL_Deposit.GetInvoiceByList(HttpContext.Current.Session["config"].ToString(), String.Join(",", lsSearch.ToArray()), checkNumber, chkSeparateInvoice.Checked);
            }

        }
        else
        {
            List<GetInvoiceByListViewModel> _lstGetInvoiceByList = new List<GetInvoiceByListViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "ReceivePaymentAPI/AddMultiReceivePayment_GetInvoiceByList";

                _GetInvoiceByList.ConnConfig = HttpContext.Current.Session["config"].ToString();
                _GetInvoiceByList.invoiceId = strInvoice;
                _GetInvoiceByList.checkNumber = checkNumber;
                _GetInvoiceByList.isSeparate = chkSeparateInvoice.Checked;

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoiceByList);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetInvoiceByList = serializer.Deserialize<List<GetInvoiceByListViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetInvoiceByListViewModel>(_lstGetInvoiceByList);
            }
            else
            {
                ds = objBL_Deposit.GetInvoiceByList(HttpContext.Current.Session["config"].ToString(), strInvoice, checkNumber, chkSeparateInvoice.Checked);
            }
        }




        dt = ds.Tables[0];
        SetGridView(dt);
        txtlsInvoice.Text = "";
    }

    protected void ibtnDeleteRow_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["Invoices"] != null)
            {
                getDataOnTestCustomGrid();

                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["Invoices"];
                int count = 0;
                foreach (GridDataItem gr in RadGrid_gvInvoice.Items)
                {
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                    if (chkSelect.Checked == false)
                    {
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
                    dr["Owner"] = DBNull.Value;
                    dr["OwnerName"] = DBNull.Value;
                    dr["LocID"] = "";
                    dr["LocationName"] = DBNull.Value;
                    dr["STax"] = 0;
                    dr["Total"] = 0;
                    dr["Amount"] = 0;
                    dr["AmountDue"] = 0;
                    dr["paymentAmt"] = 0;
                    dr["Invoice"] = DBNull.Value;
                    dr["OrderNo"] = 0;
                    dr["CheckNumber"] = DBNull.Value;
                    dr["BatchReceive"] = DBNull.Value;
                    dr["isChecked"] = 1;
                    dr["RefTranID"] = DBNull.Value;
                    dt.Rows.Add(dr);
                }

                ViewState["Invoices"] = dt;
                BindTestCustomGrid();
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkAddnewRow_Click(object sender, EventArgs e)
    {
        getDataOnTestCustomGrid();
        DataTable dt = new DataTable();
        if (ViewState["Invoices"] != null)
        {
            dt = (DataTable)ViewState["Invoices"];
        }
        else
        {
            dt.Columns.Add("Owner", typeof(int));
            dt.Columns.Add("OwnerName", typeof(String));
            dt.Columns.Add("LocID", typeof(String));
            dt.Columns.Add("LocationName", typeof(String));
            dt.Columns.Add("STax", typeof(double));
            dt.Columns.Add("Total", typeof(double));
            dt.Columns.Add("Amount", typeof(double));
            dt.Columns.Add("AmountDue", typeof(double));
            dt.Columns.Add("paymentAmt", typeof(double));
            dt.Columns.Add("Invoice", typeof(String));
            dt.Columns.Add("OrderNo", typeof(int));
            dt.Columns.Add("CheckNumber", typeof(String));
            dt.Columns.Add("BatchReceive", typeof(String));
            dt.Columns.Add("isChecked", typeof(int));
            dt.Columns.Add("RefTranID", typeof(String));

        }
        DataRow dr = dt.NewRow();
        dr["Owner"] = 0;
        dr["OwnerName"] = DBNull.Value;
        dr["LocID"] = "";
        dr["LocationName"] = DBNull.Value;
        dr["STax"] = 0;
        dr["Total"] = 0;
        dr["Amount"] = 0;
        dr["AmountDue"] = 0;
        dr["paymentAmt"] = 0;
        dr["Invoice"] = DBNull.Value;
        dr["OrderNo"] = 0;
        dr["CheckNumber"] = "";
        dr["BatchReceive"] = "";
        dr["isChecked"] = 1;
        dr["RefTranID"] = DBNull.Value;
        dt.Rows.Add(dr);
        ViewState["Invoices"] = dt;
        BindTestCustomGrid();
    }
    #endregion

    #region Invoice
    private void CreateDatatable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Owner", typeof(int));
        dt.Columns.Add("OwnerName", typeof(String));
        dt.Columns.Add("LocID", typeof(String));
        dt.Columns.Add("LocationName", typeof(String));
        dt.Columns.Add("STax", typeof(double));
        dt.Columns.Add("Total", typeof(double));
        dt.Columns.Add("Amount", typeof(double));
        dt.Columns.Add("AmountDue", typeof(double));
        dt.Columns.Add("paymentAmt", typeof(double));
        dt.Columns.Add("Invoice", typeof(String));
        dt.Columns.Add("CheckNumber", typeof(String));
        dt.Columns.Add("BatchReceive", typeof(String));
        dt.Columns.Add("isChecked", typeof(int));
        dt.Columns.Add("RefTranID", typeof(String));

        DataRow dr = dt.NewRow();
        dr["Owner"] = DBNull.Value;
        dr["OwnerName"] = DBNull.Value;
        dr["LocID"] = "";
        dr["LocationName"] = DBNull.Value;
        dr["STax"] = 0;
        dr["Total"] = 0;
        dr["Amount"] = 0;
        dr["AmountDue"] = 0;
        dr["paymentAmt"] = 0;
        dr["Invoice"] = DBNull.Value;
        dr["CheckNumber"] = DBNull.Value;
        dr["BatchReceive"] = DBNull.Value;
        dr["isChecked"] = 1;
        dr["RefTranID"] = DBNull.Value;

        dt.Rows.Add(dr);
        ViewState["ReceivPay"] = dt;
    }
    private void SetGridView(DataTable dt)
    {

        DataTable currentData = new DataTable();

        try
        {
            if (ViewState["Invoices"] == null)
            {
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["Invoices"] = dt;
                        RadGrid_gvInvoice.DataSource = dt;
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
            else
            {
                currentData = (DataTable)ViewState["Invoices"];
                if (dt != null)
                {

                    if (dt.Rows.Count > 0)
                    {
                        mergerData(currentData, dt);
                        currentData = (DataTable)ViewState["Invoices"];
                        /// currentData.Merge(dt);
                        RadGrid_gvInvoice.DataSource = currentData;
                        RadGrid_gvInvoice.Rebind();
                    }
                    else
                    {
                        RadGrid_gvInvoice.DataSource = currentData;
                        RadGrid_gvInvoice.Rebind();
                    }
                }
                else
                {
                    RadGrid_gvInvoice.DataSource = currentData;
                    RadGrid_gvInvoice.Rebind();
                }
            }


        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
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
            if (dt.Columns.Contains("Amount"))
            {
                GridFooterItem footerItem = (GridFooterItem)RadGrid_gvInvoice.MasterTableView.GetItems(GridItemType.Footer)[0];

                Label lblTotalOrigAmount = footerItem.FindControl("lblTotalOrigAmount") as Label;
                lblTotalOrigAmount.Text = string.Format("{0:c}", dt.Compute("sum(Total)", string.Empty));

                Label lblTotalDueAmount = footerItem.FindControl("lblTotalDueAmount") as Label;
                lblTotalDueAmount.Text = string.Format("{0:c}", dt.Compute("sum(AmountDue)-sum(paymentAmt)", string.Empty));


                Label lblTotalPayAmount = footerItem.FindControl("lblTotalPayAmount") as Label;
                lblTotalPayAmount.Text = string.Format("{0:c}", dt.Compute("sum(paymentAmt)", string.Empty));
            }
            try
            {
                if (RadGrid_gvInvoice.Items.Count > 1)
                {
                    int lastRow = RadGrid_gvInvoice.Items.Count;

                    GridDataItem gr = (GridDataItem)RadGrid_gvInvoice.Items[lastRow - 1];
                    TextBox txtOwnerName = (TextBox)gr.FindControl("txtOwnerName");
                    txtOwnerName.Focus();
                }

            }
            catch (Exception ex)
            {

            }

        }
    }


    private void BindTestCustomGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["Invoices"];
            RadGrid_gvInvoice.DataSource = dt;
            RadGrid_gvInvoice.VirtualItemCount = dt.Rows.Count;
            RadGrid_gvInvoice.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void getDataOnTestCustomGrid()
    {
        try
        {
            if (ViewState["Invoices"] != null)
            {
                DataTable dt = (DataTable)ViewState["Invoices"];
                DataTable dtInvoice = dt.Clone();
                foreach (GridDataItem gr in RadGrid_gvInvoice.Items)
                {
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                    HiddenField hdnOrderNo = (HiddenField)gr.FindControl("hdnOrderNo");
                    HiddenField hdnOwner = (HiddenField)gr.FindControl("hdnOwner");
                    TextBox txtOwnerName = (TextBox)gr.FindControl("txtOwnerName");
                    HiddenField hdnLocID = (HiddenField)gr.FindControl("hdnLocID");
                    TextBox txtLocationName = (TextBox)gr.FindControl("txtLocationName");
                    HiddenField hdnSTax = (HiddenField)gr.FindControl("hdnSTax");
                    HiddenField hdnTotal = (HiddenField)gr.FindControl("hdnTotal");
                    HiddenField hdnAmount = (HiddenField)gr.FindControl("hdnAmount");
                    HiddenField hdnPrevDue = (HiddenField)gr.FindControl("hdnPrevDue");
                    TextBox txtPAmount = (TextBox)gr.FindControl("txtPAmount");
                    TextBox txtInvoice = (TextBox)gr.FindControl("txtInvoice");
                    TextBox txt_gCheck = (TextBox)gr.FindControl("txt_gCheck");
                    HiddenField hdnBatchReceive = (HiddenField)gr.FindControl("hdnBatchReceive");
                    HiddenField hdnRefTranID = (HiddenField)gr.FindControl("hdnRefTranID");
                    DataRow dr = dtInvoice.NewRow();
                    dr["Owner"] = (hdnOwner.Value == "") ? 0 : Convert.ToInt32(hdnOwner.Value);
                    dr["OwnerName"] = txtOwnerName.Text;
                    dr["LocID"] = hdnLocID.Value;
                    dr["LocationName"] = txtLocationName.Text;
                    dr["STax"] = double.Parse(hdnSTax.Value.Replace('$', '0'), NumberStyles.AllowParentheses |
                           NumberStyles.AllowThousands |
                           NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                    dr["Total"] = double.Parse(hdnTotal.Value.Replace('$', '0'), NumberStyles.AllowParentheses |
                               NumberStyles.AllowThousands |
                               NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                    dr["Amount"] = double.Parse(hdnAmount.Value.Replace('$', '0'), NumberStyles.AllowParentheses |
                                 NumberStyles.AllowThousands |
                                 NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                    dr["AmountDue"] = double.Parse(hdnPrevDue.Value.Replace('$', '0'), NumberStyles.AllowParentheses |
                                 NumberStyles.AllowThousands |
                                 NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                    dr["paymentAmt"] = double.Parse(txtPAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                 NumberStyles.AllowThousands |
                                 NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);

                    dr["Invoice"] = txtInvoice.Text;
                    dr["OrderNo"] = hdnOrderNo.Value;
                    dr["CheckNumber"] = txt_gCheck.Text;
                    dr["BatchReceive"] = hdnBatchReceive.Value;
                    dr["isChecked"] = chkSelect.Checked;
                    dr["RefTranID"] = hdnRefTranID.Value;
                    dtInvoice.Rows.Add(dr);

                }
                ViewState["Invoices"] = dtInvoice;
            }


        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    public DataTable FillInvoices(string prefixText, int cus, string loc)
    {
        BL_Deposit objBL_Deposit = new BL_Deposit();

        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        try
        {

            List<GetInvoicesByReceivedPayMultiViewModel> _lstGetInvoicesByReceivedPayMulti = new List<GetInvoicesByReceivedPayMultiViewModel>();

            if (IsAPIIntegrationEnable == "YES")
            {
                string APINAME = "ReceivePaymentAPI/AddMultiReceivePayment_GetInvoicesByReceivedPayMulti";

                _GetInvoicesByReceivedPayMulti.ConnConfig = HttpContext.Current.Session["config"].ToString();
                _GetInvoicesByReceivedPayMulti.owner = cus;
                _GetInvoicesByReceivedPayMulti.loc = loc;
                _GetInvoicesByReceivedPayMulti.invoice = prefixText;

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoicesByReceivedPayMulti);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                serializer.MaxJsonLength = Int32.MaxValue;

                _lstGetInvoicesByReceivedPayMulti = serializer.Deserialize<List<GetInvoicesByReceivedPayMultiViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<GetInvoicesByReceivedPayMultiViewModel>(_lstGetInvoicesByReceivedPayMulti);
            }
            else
            {
                ds = objBL_Deposit.GetInvoicesByReceivedPayMulti(HttpContext.Current.Session["config"].ToString(), cus, loc, prefixText);
            }

            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return dt;
    }

    #endregion



    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string GetAmountInvoice(string prefixText, string con)
    {
        string str;
        JavaScriptSerializer sr = new JavaScriptSerializer();
        List<Dictionary<object, object>> dictListEval = new List<Dictionary<object, object>>();
        BL_User objBL_User = new BL_User();
        BusinessEntity.User objPropUser = new BusinessEntity.User();
        GeneralFunctions objGeneral = new GeneralFunctions();

        BL_Deposit objBL_Deposit = new BL_Deposit();
        PaymentDetails objPayment = new PaymentDetails();

        DataSet ds = new DataSet();

        string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
        GetInvoiceByListParam _GetInvoiceByList = new GetInvoiceByListParam();
        List<GetInvoiceByListViewModel> _lstGetInvoiceByList = new List<GetInvoiceByListViewModel>();

        if (IsAPIIntegrationEnable == "YES")
        {
            string APINAME = "ReceivePaymentAPI/AddMultiReceivePayment_GetInvoiceByList";

            _GetInvoiceByList.ConnConfig = HttpContext.Current.Session["config"].ToString();
            _GetInvoiceByList.invoiceId = prefixText;
            _GetInvoiceByList.checkNumber = "";
            _GetInvoiceByList.isSeparate = false;

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _GetInvoiceByList);

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            serializer.MaxJsonLength = Int32.MaxValue;

            _lstGetInvoiceByList = serializer.Deserialize<List<GetInvoiceByListViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetInvoiceByListViewModel>(_lstGetInvoiceByList);
        }
        else
        {
            ds = objBL_Deposit.GetInvoiceByList(HttpContext.Current.Session["config"].ToString(), prefixText, "", false);
        }

        dictListEval = objGeneral.RowsToDictionary(ds.Tables[0]);
        str = sr.Serialize(dictListEval);

        return str;


    }





    protected void btnAddnewRow_Click(object sender, EventArgs e)
    {
        getDataOnTestCustomGrid();
        DataTable dt = new DataTable();
        if (ViewState["Invoices"] != null)
        {
            dt = (DataTable)ViewState["Invoices"];
        }
        else
        {
            dt.Columns.Add("Owner", typeof(int));
            dt.Columns.Add("OwnerName", typeof(String));
            dt.Columns.Add("LocID", typeof(String));
            dt.Columns.Add("LocationName", typeof(String));
            dt.Columns.Add("STax", typeof(double));
            dt.Columns.Add("Total", typeof(double));
            dt.Columns.Add("Amount", typeof(double));
            dt.Columns.Add("AmountDue", typeof(double));
            dt.Columns.Add("paymentAmt", typeof(double));
            dt.Columns.Add("Invoice", typeof(String));
            dt.Columns.Add("OrderNo", typeof(int));
            dt.Columns.Add("CheckNumber", typeof(String));
            dt.Columns.Add("BatchReceive", typeof(String));
            dt.Columns.Add("isChecked", typeof(int));
            dt.Columns.Add("RefTranID", typeof(String));
        }
        DataRow dr = dt.NewRow();
        dr["Owner"] = 0;
        dr["OwnerName"] = DBNull.Value;
        dr["LocID"] = "";
        dr["LocationName"] = DBNull.Value;
        dr["STax"] = 0;
        dr["Total"] = 0;
        dr["Amount"] = 0;
        dr["AmountDue"] = 0;
        dr["paymentAmt"] = 0;
        dr["Invoice"] = DBNull.Value;
        dr["OrderNo"] = 0;
        dr["CheckNumber"] = DBNull.Value;
        dr["BatchReceive"] = DBNull.Value;
        dr["isChecked"] = 1;
        dr["RefTranID"] = DBNull.Value;
        dt.Rows.Add(dr);
        ViewState["Invoices"] = dt;
        BindTestCustomGrid();

    }

    private void mergerData(DataTable root, DataTable dt)
    {
        foreach (DataRow row in dt.Rows)
        {
            DataRow dr = root.NewRow();
            dr["Owner"] = row["Owner"];
            dr["OwnerName"] = row["OwnerName"];
            dr["LocID"] = row["LocID"];
            dr["LocationName"] = row["LocationName"];
            dr["STax"] = row["STax"];
            dr["Total"] = row["Total"];
            dr["Amount"] = row["Amount"];
            dr["AmountDue"] = row["AmountDue"];
            dr["paymentAmt"] = row["paymentAmt"];
            dr["Invoice"] = row["Invoice"];
            dr["OrderNo"] = row["OrderNo"];
            dr["CheckNumber"] = row["CheckNumber"];
            dr["BatchReceive"] = row["BatchReceive"];
            dr["isChecked"] = row["isChecked"];
            dr["RefTranID"] = row["RefTranID"];
            root.Rows.Add(dr);

        }
        ViewState["Invoices"] = root;
    }
}
