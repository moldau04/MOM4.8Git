using AjaxControlToolkit;
using BusinessEntity;
using BusinessLayer;
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


    public partial class EditMultiReceivePayment : System.Web.UI.Page
{
    #region Variables   
    Bank _objBank = new Bank();
    BL_BankAccount _objBL_Bank = new BL_BankAccount();
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
                loadEditData();
            }
            Permission();
            if (Session["editBatchReceiptMessage"] != null) {
                Session["editBatchReceiptMessage"] = null;
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Batch receipt updated Successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                if (Session["editBatchReceiptDepID"] != null)
                {
                    if (Convert.ToInt32(Session["editBatchReceiptDepID"]) != 0)
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Deposit Added Successfully! <BR/>Ref# " + Session["editBatchReceiptDepID"].ToString() + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                    }                   
                    Session["editBatchReceiptDepID"] = null;
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
            DataSet _dsBank = new DataSet();
            _dsBank = _objBL_Bank.GetAllBankNames(_objBank);

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
    public void submit()
    {
        int depId=0;
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
                    HiddenField hdnReceiptID = (HiddenField)gr.FindControl("hdnReceiptID");
                    HiddenField hdnDepID = (HiddenField)gr.FindControl("hdnDepID");
                    HiddenField hdnDepStatus = (HiddenField)gr.FindControl("hdnDepStatus");
                    HiddenField hdnenableEdit = (HiddenField)gr.FindControl("hdnenableEdit");
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
                        dr["ReceiptID"] = (hdnReceiptID.Value == "") ? 0 : Convert.ToInt32(hdnReceiptID.Value);  
                        dr["DepID"] = (hdnDepID.Value == "") ? 0 : Convert.ToInt32(hdnDepID.Value); 
                        dr["DepStatus"] = (hdnDepStatus.Value == "") ? false : Convert.ToBoolean(hdnDepStatus.Value);
                        dr["enableEdit"] = (hdnenableEdit.Value == "") ? true : Convert.ToBoolean(hdnenableEdit.Value);
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
                dsResult= objBL_Deposit.UpdateBatchReceivePayment(objReceivePay, Convert.ToInt32(ddlBank.SelectedValue), Convert.ToBoolean(chkCreateDeposit.Checked), Convert.ToInt32(Request.QueryString["uid"]));
                depId =Convert.ToInt32( dsResult.Tables[0].Rows[0]["DepId"]);

                Session["editBatchReceiptMessage"] = "Batch receipt updated Successfully!";
                Session["editBatchReceiptDepID"] = depId;
                Response.Redirect("EditMultiReceivepayment?uid=" + Request.QueryString["uid"]);


                //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Batch receipt updated Successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                //ResetForm();
                //loadEditData();
     

            }
        }
        catch (Exception ex)
        {
            String type = "error";
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            if (str.Contains("Please select a bank") || str.Contains("The Batch Receipt date cannot be dated after the Deposit date!"))
            {
                type = "warning";
            }
            
            ScriptManager.RegisterStartupScript(this,Page.GetType(), "keyErr", "noty({text: '" + str + "',  type :'"+ type+"', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
           // return 0;
        }
       // return depId;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        submit();
        //int depId;
        //try {
        //    depId = submit();
        //    if (chkCreateDeposit.Checked == true && depId!=0)
        //    {
        //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Deposit Added Successfully! <BR/>Ref# " + depId + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
        //    }
            
        //}
        //catch (Exception)
        //{
        //    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'There is an error. Please try again later.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        //}




    }
    private void SetDeposit(DataTable dt,int depid)
    {
        try
        {           
            BL_Deposit _objBL_Deposit = new BL_Deposit();
            ReceivedPayment _objReceivePay = new ReceivedPayment();
            _objReceivePay.ConnConfig = Session["config"].ToString();

            if (depid == 0)
            {
                _objReceivePay.DepID = Convert.ToInt32(Request.QueryString["id"]);
            }
            else
            {
                _objReceivePay.DepID = depid;
            }
            DataSet _dsDep = _objBL_Deposit.GetReceivedPaymentByDep(_objReceivePay);
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
            ds = objBL_Deposit.GetInvoiceByList(HttpContext.Current.Session["config"].ToString(), String.Join(",", lsSearch.ToArray()), checkNumber,chkSeparateInvoice.Checked);
        }
        else
        {
            ds = objBL_Deposit.GetInvoiceByList(HttpContext.Current.Session["config"].ToString(), strInvoice, checkNumber, chkSeparateInvoice.Checked);
        }


       
        dt = ds.Tables[0];

        dt.Columns.Add("ReceiptID", typeof(int));
        dt.Columns.Add("DepID", typeof(int));
        dt.Columns.Add("DepStatus", typeof(Boolean));
        dt.Columns.Add("enableEdit", typeof(Boolean));
        foreach (DataRow row in dt.Rows)
        {           
            row["ReceiptID"] = 0;
            row["DepID"] = 0;
            row["DepStatus"] = false;
            row["enableEdit"] = true;     
        }

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
                    if (chkSelect.Checked==false)
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
                    dr["ReceiptID"] = DBNull.Value;
                    dr["DepID"] = DBNull.Value;
                    dr["DepStatus"] = false;
                    dr["enableEdit"] = true;
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
        if (ViewState["Invoices"] != null) { 
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
            dt.Columns.Add("ReceiptID", typeof(int));
            dt.Columns.Add("DepID", typeof(int));
            dt.Columns.Add("DepStatus", typeof(Boolean));
            dt.Columns.Add("enableEdit", typeof(Boolean));
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
        dr["BatchReceive"] = 0;
        dr["isChecked"] = 1;
        dr["ReceiptID"] = DBNull.Value;
        dr["DepID"] = DBNull.Value;
        dr["DepStatus"] = false;
        dr["enableEdit"] = true;
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
        dt.Columns.Add("ReceiptID", typeof(int));
        dt.Columns.Add("DepID", typeof(int));
        dt.Columns.Add("DepStatus", typeof(Boolean));
        dt.Columns.Add("enableEdit", typeof(Boolean));
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
        dr["ReceiptID"] = DBNull.Value;
        dr["DepID"] = DBNull.Value;
        dr["DepStatus"] = false;
        dr["DepStatus"] = true;
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

                    GridDataItem gr = (GridDataItem)RadGrid_gvInvoice.Items[lastRow-1];
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
                    HiddenField hdnReceiptID = (HiddenField)gr.FindControl("hdnReceiptID");
                    HiddenField hdnDepID = (HiddenField)gr.FindControl("hdnDepID");
                    HiddenField hdnDepStatus = (HiddenField)gr.FindControl("hdnDepStatus");
                    HiddenField hdnEnableEdit = (HiddenField)gr.FindControl("hdnEnableEdit");
                    HiddenField hdnRefTranID = (HiddenField)gr.FindControl("hdnRefTranID");
                    DataRow dr = dtInvoice.NewRow();
                    dr["Owner"] = (hdnOwner.Value=="")?0:Convert.ToInt32( hdnOwner.Value);
                    dr["OwnerName"] = txtOwnerName.Text;
                    dr["LocID"] = hdnLocID.Value;
                    dr["LocationName"] = txtLocationName.Text;
                    dr["STax"] = double.Parse(hdnSTax.Value.Replace('$', '0'), NumberStyles.AllowParentheses |
                           NumberStyles.AllowThousands |
                           NumberStyles.AllowDecimalPoint|NumberStyles.AllowLeadingSign);
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

                    dr["Invoice"] = (txtInvoice.Text == "") ? 0 : Convert.ToInt32(txtInvoice.Text); 
                    dr["OrderNo"] = hdnOrderNo.Value;
                    dr["CheckNumber"] = txt_gCheck.Text;
                    dr["BatchReceive"] = (hdnBatchReceive.Value == "") ? 0 : Convert.ToInt32(hdnBatchReceive.Value); 
                    dr["isChecked"] = chkSelect.Checked;
           

                    dr["ReceiptID"] = (hdnReceiptID.Value == "") ? 0 : Convert.ToInt32(hdnReceiptID.Value);
                    dr["DepID"] = (hdnDepID.Value == "") ? 0 : Convert.ToInt32(hdnDepID.Value);
                    dr["DepStatus"] = (hdnDepStatus.Value == "") ? false : Convert.ToBoolean(hdnDepStatus.Value);
                    dr["enableEdit"] = (hdnEnableEdit.Value == "") ? true : Convert.ToBoolean(hdnEnableEdit.Value);
                    dr["RefTranID"] = (hdnRefTranID.Value == "") ? 0 : Convert.ToInt32(hdnRefTranID.Value); 
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
            ds = objBL_Deposit.GetInvoicesByReceivedPayMulti(HttpContext.Current.Session["config"].ToString(), cus, loc, prefixText);

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
            dt.Columns.Add("ReceiptID", typeof(int));
            dt.Columns.Add("DepID", typeof(int));
            dt.Columns.Add("DepStatus", typeof(Boolean));
            dt.Columns.Add("enableEdit", typeof(Boolean));
            dt.Columns.Add("RefTranID", typeof(String));
        }
        DataRow dr = dt.NewRow();
        dr["Owner"] = 0;
        dr["OwnerName"] = DBNull.Value;
        dr["LocID"] = "";
        dr["LocationName"] = DBNull.Value;
        dr["STax"] = 0;
        dr["Total"] =0;      
        dr["Amount"] = 0;
        dr["AmountDue"] = 0;
        dr["paymentAmt"] = 0;
        dr["Invoice"] = DBNull.Value;
        dr["OrderNo"] = 0;
        dr["CheckNumber"] = DBNull.Value;
        dr["BatchReceive"] = DBNull.Value;
        dr["isChecked"] = 1;
        dr["ReceiptID"] = DBNull.Value;
        dr["DepID"] = DBNull.Value;
        dr["DepStatus"] =false;
        dr["enableEdit"] =true;
        dr["RefTranID"] = DBNull.Value;
        dt.Rows.Add(dr);
        ViewState["Invoices"] = dt;
        BindTestCustomGrid();
        
    }

    private void mergerData(DataTable root,DataTable dt)
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
                dr["ReceiptID"] = row["ReceiptID"];
                dr["DepID"] = row["DepID"];
                dr["DepStatus"] = row["DepStatus"]; 
                dr["enableEdit"] = row["enableEdit"];
                dr["RefTranID"] = row["RefTranID"];
            root.Rows.Add(dr);       
           
         }
        ViewState["Invoices"] = root;
    }

    private void loadEditData()
    {
        if (Request.QueryString["uid"] != null)
        {
            DataTable dt = new DataTable();
            BL_Deposit objBL_Deposit = new BL_Deposit();
            DataSet ds = new DataSet();
            ds = objBL_Deposit.GetBatchReceivePayment(HttpContext.Current.Session["config"].ToString(), Convert.ToInt32(Request.QueryString["uid"]));
            dt = ds.Tables[0];
            int countDep = 0;
            if (ds.Tables[2]!=null)
            {
                countDep = Convert.ToInt32(ds.Tables[2].Rows[0]["CountDep"]);
            }
            hdnMultiDep.Value = countDep.ToString();
            if (countDep > 0)
            {
                chkCreateDeposit.Enabled = false;
            }
          

            SetGridView(dt);
            txtlsInvoice.Text = "";
            txtDate.Text = ds.Tables[1].Rows[0]["PaymentReceivedDate"].ToString();
            ddlPayment.SelectedValue = ds.Tables[1].Rows[0]["PaymentMethod"].ToString();
        }

       
    }
}
