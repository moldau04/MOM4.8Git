using BusinessEntity;
using BusinessLayer;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Stimulsoft.Report;
using Telerik.Web.UI;
using System.Collections;
using System.Web.Script.Serialization;
using BusinessEntity.Utility;
using MOMWebApp;
using BusinessEntity.APModels;
using BusinessEntity.Payroll;
using BusinessEntity.payroll;
using Newtonsoft.Json;
using BusinessEntity.CommonModel;

public partial class WriteChecks : System.Web.UI.Page
{
    #region Variables
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    //Chart _objChart = new Chart();
    //BL_Chart _objBLChart = new BL_Chart();
    General _objPropGeneral = new General();
    BL_General _objBLGeneral = new BL_General();
    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();
    Journal _objJournal = new Journal();
    Transaction _objTrans = new Transaction();
    BL_JournalEntry _objBLJournal = new BL_JournalEntry();

    Bank _objBank = new Bank();
    BL_BankAccount _objBL_Bank = new BL_BankAccount();

    CD _objCD = new CD();
    User _objUser = new User();

    Vendor _objVendor = new Vendor();
    BL_Vendor _objBLVendor = new BL_Vendor();

    OpenAP _objOpenAP = new OpenAP();
    BL_Bills _objBLBill = new BL_Bills();
    BusinessEntity.CompanyOffice objCompany = new BusinessEntity.CompanyOffice();
    BL_Company objBL_Company = new BL_Company();

    Paid _objPaid = new Paid();

    GenerateCheck _objCheck = new GenerateCheck();
    PJ _objPJ = new PJ();
    long checkno = 0;
    protected DataTable dti = new DataTable();
    protected DataTable dtpay = new DataTable();
    protected DataTable dtBank = new DataTable();
    byte[] array = null;
    byte[] arrayNew = null;
    BL_Bills _objBLBills = new BL_Bills();
    private bool IsGridPageIndexChanged = false;

    //API Variables
    //string IsAPIIntegrationEnable = System.Web.Configuration.WebConfigurationManager.AppSettings["IsAPIIntegrationEnable"].Trim();
    APIIntegrationModel _objAPIIntegration = new APIIntegrationModel();
    GetVendorByNameParam _getVendorByName = new GetVendorByNameParam();
    getUserDefaultCompanyParam _getUserDefaultCompany = new getUserDefaultCompanyParam();
    GetOpenBillVendorByCompanyParam _getOpenBillVendorByCompany = new GetOpenBillVendorByCompanyParam();
    GetAllBankNamesByCompanyParam _getAllBankNamesByCompany= new GetAllBankNamesByCompanyParam();
    GetOpenBillVendorParam _getOpenBillVendor = new GetOpenBillVendorParam();
    GetRunningBalanceCountsParam _getRunningBalanceCounts = new GetRunningBalanceCountsParam();
    GetCheckTemplateParam _getCheckTemplate = new GetCheckTemplateParam();
    GetAutoSelectPaymentParam _getAutoSelectPayment = new GetAutoSelectPaymentParam();
    GetVendorParam _getVendor = new GetVendorParam();
    GetBillsByVendorParam _getBillsByVendor = new GetBillsByVendorParam();
    AddCheckParam _addCheck = new AddCheckParam();
    GetBankByIDParam _getBankByID = new GetBankByIDParam();
    ApplyCreditParam _applyCredit = new ApplyCreditParam();
    GetCreditBillVendorParam _getCreditBillVendor = new GetCreditBillVendorParam();
    GetBankCDParam _getBankCD = new GetBankCDParam();
    GetVendorAcctParam _getVendorAcct = new GetVendorAcctParam();
    updateCheckTemplateParam _updateCheckTemplate = new updateCheckTemplateParam();
    AutoSelectPaymentParam _autoSelectPayment = new AutoSelectPaymentParam();
    AddBillsParam _addBills = new AddBillsParam();
    AddCheckRecurrParam _addCheckRecurr = new AddCheckRecurrParam();
    GetSelectedOpenAPPJIDParam _getSelectedOpenAPPJID = new GetSelectedOpenAPPJIDParam();
    UpdateWriteCheckOpenAPpaymentParam _updateWriteCheckOpenAPpayment = new UpdateWriteCheckOpenAPpaymentParam();
    getCustomFieldsParam _getCustomFields = new getCustomFieldsParam();
    getCustomFieldsControlParam _getCustomFieldsControl = new getCustomFieldsControlParam();
    GetChartParam _getChart = new GetChartParam();
    GetInvDefaultAcctParam _getInvDefaultAcct= new GetInvDefaultAcctParam();
    GetCompanyByCustomerParam _getCompanyByCustomer = new GetCompanyByCustomerParam();
    IsExistCheckNumParam _isExistCheckNum = new IsExistCheckNumParam();
    getConnectionConfigParam _getConnectionConfig = new getConnectionConfigParam();
    GetControlBranchParam _getControlBranch = new GetControlBranchParam();
    getCustomFieldParam _getCustomField = new getCustomFieldParam();
    GetUserByIdParam _getUserById = new GetUserByIdParam();
    GetCheckDetailsByBankAndRefParam _getCheckDetailsByBankAndRef = new GetCheckDetailsByBankAndRefParam();
    GetVendorRolDetailsParam _getVendorRolDetails = new GetVendorRolDetailsParam();
    GetAllBankNamesParam _getAllBankNames = new GetAllBankNamesParam();
    #endregion

    #region Events

    #region PAGELOAD
    protected void RadGrid_gvJobCostItems_PreRender(object sender, EventArgs e)
    {
        foreach (GridDataItem gr in RadGrid_gvJobCostItems.Items)
        {
            TextBox txtGvAcctNo = (TextBox)gr.FindControl("txtGvAcctNo");
            CheckBox chkTaxable = (CheckBox)gr.FindControl("chkTaxable");
            CheckBox chkGTaxable = (CheckBox)gr.FindControl("chkGTaxable");
            gr.Attributes["onclick"] = "VisibleRows('" + gr.ClientID + "','" + txtGvAcctNo.ClientID + "','" + RadGrid_gvJobCostItems.ClientID + "',event);";
            chkTaxable.Attributes["onclick"] = "CalTotalValStax(this);";
            chkGTaxable.Attributes["onclick"] = "CalTotalValGtax(this);";
        }
    }
    protected void RadGrid_gvJobCostItems_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            DataTable dt = GetCurrentTransaction();

            if (e.CommandName == "DeleteTransaction")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                dt.Rows.RemoveAt(index);
                dt.AcceptChanges();

                BINDGRID(dt);

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "CallCalAmountTax", "CallCalAmountTax();", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private DataTable GetTransaction()
    {
        DataTable dt = GetTable();

        try
        {
            string strItems = hdnGLItem.Value.Trim();

            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;

                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    if (dict["hdnAcctID"].ToString().Trim() == string.Empty || dict["hdnAcctID"].ToString() == "0")
                    {
                        continue;
                    }
                    i++;
                    DataRow dr = dt.NewRow();
                    //dr["Line"] = i;
                    dr["AcctID"] = dict["hdnAcctID"].ToString().Trim();
                    //if (dict["hdnQuantity"].ToString().Trim() != string.Empty)
                    //{
                    //    dr["Quan"] = dict["hdnQuantity"].ToString();
                    //}

                    if (dict["txtGvQuan"].ToString().Trim() != string.Empty)
                    {
                        dr["Quan"] = dict["txtGvQuan"].ToString();
                    }

                    dr["Ticket"] = !string.IsNullOrEmpty(dict["txtGvTicket"].ToString()) ? dict["txtGvTicket"].ToString() : "0";
                    if (dict["hdnTID"].ToString().Trim() != string.Empty)
                    {
                        dr["ID"] = dict["hdnTID"].ToString();
                    }
                    if (dict["txtGvDesc"].ToString().Trim() != string.Empty)
                    {
                        dr["fDesc"] = dict["txtGvDesc"].ToString().Trim();
                    }
                    if (dict["txtGvAmount"].ToString().Trim() != string.Empty)
                    {
                        dr["Amount"] = dict["txtGvAmount"].ToString();
                    }
                    //if (dict["txtGvUseTax"].ToString().Trim() != string.Empty)
                    //{
                    //    dr["Usetax"] = dict["txtGvUseTax"].ToString();
                    //}
                    //if (dict["hdnUtax"].ToString().Trim() != string.Empty)
                    //{
                    //    dr["UtaxName"] = dict["hdnUtax"].ToString();
                    //}
                    //dr["UName"] = dict["hdnUtax"].ToString();
                    //if (dict["hdnUtaxGL"].ToString().Trim() != string.Empty)
                    //{
                    //    dr["UtaxGL"] = dict["hdnUtaxGL"].ToString();
                    //}
                    //else
                    //{
                    //    dr["UtaxGL"] = DBNull.Value;
                    //}

                    if (dict.ContainsKey("txtGvUseTax"))
                    {
                        if (dict["txtGvUseTax"].ToString().Trim() != string.Empty)
                        {
                            dr["Usetax"] = dict["txtGvUseTax"].ToString();
                        }
                        else
                        {
                            dr["Usetax"] = 0;
                        }
                    }
                    else
                    {
                        dr["Usetax"] = 0;
                    }

                    if (dict.ContainsKey("hdnUtax"))
                    {
                        if (dict["hdnUtax"].ToString().Trim() != string.Empty)
                        {
                            dr["UtaxName"] = dict["hdnUtax"].ToString();
                            dr["UName"] = dict["hdnUtax"].ToString();
                        }
                        else
                        {
                            dr["UtaxName"] = DBNull.Value;
                            dr["UName"] = DBNull.Value;
                        }
                    }
                    else
                    {
                        dr["UtaxName"] = DBNull.Value;
                        dr["UName"] = DBNull.Value;
                    }
                    if (dict.ContainsKey("hdnUtaxGL"))
                    {
                        if (dict["hdnUtaxGL"].ToString().Trim() != string.Empty)
                        {
                            dr["UtaxGL"] = dict["hdnUtaxGL"].ToString();
                        }
                        else
                        {
                            dr["UtaxGL"] = 0;
                        }
                    }
                    else
                    {
                        dr["UtaxGL"] = 0;
                    }


                    if (dict["hdnJobID"].ToString().Trim() != string.Empty)
                    {
                        dr["JobID"] = dict["hdnJobID"].ToString();
                    }
                    dr["JobName"] = dict["txtGvJob"].ToString();
                    if (dict["hdnPID"].ToString().Trim() != string.Empty)
                    {
                        double temp = Convert.ToDouble(dict["hdnPID"]);
                        dr["PhaseID"] = Convert.ToDouble(dict["hdnPID"]);
                    }
                    else
                    {
                        dr["PhaseID"] = 0;
                    }
                    if (dict["hdnItemID"].ToString().Trim() != string.Empty)
                    {
                        dr["ItemID"] = dict["hdnItemID"].ToString();
                    }
                    dr["AcctNo"] = dict["txtGvAcctNo"].ToString();

                    dr["Loc"] = dict["txtGvLoc"].ToString();
                    dr["Phase"] = dict["txtGvPhase"].ToString();
                    if (dict["hdnTypeId"].ToString().Trim() != string.Empty)
                    {
                        dr["TypeID"] = dict["hdnTypeId"].ToString();
                    }
                    dr["ItemDesc"] = dict["txtGvItem"].ToString();
                    if (!(dict["hdOpSq"].ToString().Trim() == string.Empty))
                    {
                        dr["OpSq"] = dict["hdOpSq"].ToString();
                    }
                    else
                    {
                        dr["OpSq"] = "100";
                    }
                    if (dict["hdnLine"].ToString().Trim() != string.Empty)
                        dr["Line"] = dict["hdnLine"].ToString();
                    if (dict["hdnPrvInQuan"].ToString().Trim() != string.Empty)
                        dr["PrvInQuan"] = Convert.ToDouble(dict["hdnPrvInQuan"]);
                    if (dict["hdnPrvIn"].ToString().Trim() != string.Empty)
                        dr["PrvIn"] = Convert.ToDouble(dict["hdnPrvIn"]);
                    if (dict["hdnOutstandQuan"].ToString().Trim() != string.Empty)
                        dr["OutstandQuan"] = Convert.ToDouble(dict["hdnOutstandQuan"]);
                    if (dict["hdnOutstandBalance"].ToString().Trim() != string.Empty)
                        dr["OutstandBalance"] = Convert.ToDouble(dict["hdnOutstandBalance"]);

                    BL_Inventory DL_INV = new BL_Inventory();

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/AddCheck_ISINVENTORYTRACKINGISON";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, Session["config"].ToString());
                        bool returnVal =Convert.ToBoolean(_APIResponse.ResponseData);
                        if (returnVal)
                        {
                            dr["Warehouse"] = "22";
                        }
                        //Temporally for A4 Access We set default warehouse 22 , Then later we will need to allow user to select warehouse and location
                        else
                        {
                            dr["Warehouse"] = "";
                        }
                    }
                    else
                    {
                        if (DL_INV.ISINVENTORYTRACKINGISON(Session["config"].ToString())) { dr["Warehouse"] = "22"; }  //Temporally for A4 Access We set default warehouse 22 , Then later we will need to allow user to select warehouse and location
                        else
                        {
                            dr["Warehouse"] = "";
                        }
                    }

                    dr["WHLocID"] = "0";

                    if (dict.ContainsKey("hdnchkTaxable"))
                    {
                        if (dict["hdnchkTaxable"].ToString().Trim() != string.Empty)
                        {

                            dr["stax"] = Convert.ToInt32(dict["hdnchkTaxable"]);
                        }

                    }
                    else
                    {
                        dr["stax"] = "0";
                    }
                    if (dict.ContainsKey("hdnSTaxAm"))
                    {
                        dr["STaxName"] = hdnSTaxName.Value.ToString();
                        dr["STaxRate"] = Convert.ToDecimal(hdnQST.Value);
                        //dr["STaxAmt"] = dict["hdnSTaxAm"].ToString();
                        if (dict["hdnSTaxAm"].ToString().Trim() != string.Empty)
                        {
                            dr["STaxAmt"] = dict["hdnSTaxAm"].ToString();
                        }
                        else
                        {
                            dr["STaxAmt"] = 0;
                        }
                    }
                    else
                    {
                        dr["STaxName"] = "";
                        dr["STaxRate"] = 0;
                        dr["STaxAmt"] = 0;
                    }
                    if (dict.ContainsKey("hdnSTaxGL"))
                    {
                        //dr["STaxGL"] = dict["hdnSTaxGL"].ToString();
                        if (dict["hdnSTaxGL"].ToString().Trim() != string.Empty)
                        {
                            dr["STaxGL"] = dict["hdnSTaxGL"].ToString();
                        }
                        else
                        {
                            dr["STaxGL"] = 0;
                        }
                    }
                    else
                    {
                        dr["STaxGL"] = 0;
                    }

                    if (dict.ContainsKey("hdnGSTTaxAm"))
                    {
                        dr["GSTRate"] = Convert.ToDecimal(hdnGST.Value);
                        //dr["GTaxAmt"] = dict["hdnGSTTaxAm"].ToString();
                        if (dict["hdnGSTTaxAm"].ToString().Trim() != string.Empty)
                        {
                            dr["GTaxAmt"] = dict["hdnGSTTaxAm"].ToString();
                        }
                        else
                        {
                            dr["GTaxAmt"] = 0;
                        }
                    }
                    else
                    {
                        dr["GSTRate"] = 0;
                        dr["GTaxAmt"] = 0;
                    }
                    if (dict.ContainsKey("hdnGSTTaxGL"))
                    {
                        //dr["GSTTaxGL"] = dict["hdnGSTTaxGL"].ToString();
                        if (dict["hdnGSTTaxGL"].ToString().Trim() != string.Empty && dict["hdnGSTTaxGL"].ToString().Trim() != "NaN")
                        {
                            dr["GSTTaxGL"] = dict["hdnGSTTaxGL"].ToString();
                        }
                        else
                        {
                            dr["GSTTaxGL"] = 0;
                        }
                    }
                    else
                    {
                        dr["GSTTaxGL"] = 0;
                    }
                    //return test[myKey];
                    //if (dict["hdnSTaxAm"].ToString().Trim() != string.Empty)
                    //{
                    //    dr["STaxAmt"] = dict["hdnSTaxAm"].ToString();
                    //}
                    //if (dict["hdnSTaxGL"].ToString().Trim() != string.Empty)
                    //{
                    //    dr["STaxGL"] = dict["hdnSTaxGL"].ToString();
                    //}
                    //if (dict["hdnGSTTaxAm"].ToString().Trim() != string.Empty)
                    //{
                    //    dr["GTaxAmt"] = dict["hdnGSTTaxAm"].ToString();
                    //}
                    //if (dict["hdnGSTTaxGL"].ToString().Trim() != string.Empty)
                    //{
                    //    dr["GSTTaxGL"] = dict["hdnGSTTaxGL"].ToString();
                    //}
                    dr["IsPO"] = 1;
                    if (dict.ContainsKey("hdnchkGTaxable"))
                    {
                        if (dict["hdnchkGTaxable"].ToString().Trim() != string.Empty)
                        {

                            dr["GTax"] = Convert.ToInt32(dict["hdnchkGTaxable"]);
                        }

                    }
                    else
                    {
                        dr["GTax"] = "0";
                    }
                    if (dict["txtGvPrice"].ToString().Trim() != string.Empty)
                    {
                        dr["Price"] = dict["txtGvPrice"].ToString();
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

    //private DataTable GetTransaction()
    //{
    //    DataTable dt = GetTable();

    //    try
    //    {
    //        string strItems = hdnGLItem.Value.Trim();

    //        if (strItems != string.Empty)
    //        {
    //            JavaScriptSerializer sr = new JavaScriptSerializer();
    //            List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
    //            objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
    //            int i = 0;

    //            foreach (Dictionary<object, object> dict in objEstimateItemData)
    //            {
    //                if (dict["hdnAcctID"].ToString().Trim() == string.Empty || dict["hdnAcctID"].ToString() == "0")
    //                {
    //                    continue;
    //                }
    //                i++;
    //                DataRow dr = dt.NewRow();
    //                //dr["Line"] = i;
    //                dr["AcctID"] = dict["hdnAcctID"].ToString().Trim();
    //                //if (dict["hdnQuantity"].ToString().Trim() != string.Empty)
    //                //{
    //                //    dr["Quan"] = dict["hdnQuantity"].ToString();
    //                //}

    //                if (dict["txtGvQuan"].ToString().Trim() != string.Empty)
    //                {
    //                    dr["Quan"] = dict["txtGvQuan"].ToString();
    //                }

    //                dr["Ticket"] = !string.IsNullOrEmpty(dict["txtGvTicket"].ToString()) ? dict["txtGvTicket"].ToString() : "0";
    //                if (dict["hdnTID"].ToString().Trim() != string.Empty)
    //                {
    //                    dr["ID"] = dict["hdnTID"].ToString();
    //                }
    //                if (dict["txtGvDesc"].ToString().Trim() != string.Empty)
    //                {
    //                    dr["fDesc"] = dict["txtGvDesc"].ToString().Trim();
    //                }
    //                if (dict["txtGvAmount"].ToString().Trim() != string.Empty)
    //                {
    //                    dr["Amount"] = dict["txtGvAmount"].ToString();
    //                }
    //                if (dict["txtGvUseTax"].ToString().Trim() != string.Empty)
    //                {
    //                    dr["Usetax"] = dict["txtGvUseTax"].ToString();
    //                }
    //                if (dict["hdnUtax"].ToString().Trim() != string.Empty)
    //                {
    //                    dr["UtaxName"] = dict["hdnUtax"].ToString();
    //                }
    //                if (dict["hdnJobID"].ToString().Trim() != string.Empty)
    //                {
    //                    dr["JobID"] = dict["hdnJobID"].ToString();
    //                }
    //                dr["JobName"] = dict["txtGvJob"].ToString();
    //                if (dict["hdnPID"].ToString().Trim() != string.Empty)
    //                {
    //                    double temp = Convert.ToDouble(dict["hdnPID"]);
    //                    dr["PhaseID"] = Convert.ToDouble(dict["hdnPID"]);
    //                }
    //                else
    //                {
    //                    dr["PhaseID"] = 0;
    //                }
    //                if (dict["hdnItemID"].ToString().Trim() != string.Empty)
    //                {
    //                    dr["ItemID"] = dict["hdnItemID"].ToString();
    //                }
    //                dr["AcctNo"] = dict["txtGvAcctNo"].ToString();
    //                dr["UName"] = dict["hdnUtax"].ToString();
    //                if (dict["hdnUtaxGL"].ToString().Trim() != string.Empty)
    //                {
    //                    dr["UtaxGL"] = dict["hdnUtaxGL"].ToString();
    //                }
    //                else
    //                {
    //                    dr["UtaxGL"] = DBNull.Value;
    //                }
    //                dr["Loc"] = dict["txtGvLoc"].ToString();
    //                dr["Phase"] = dict["txtGvPhase"].ToString();
    //                if (dict["hdnTypeId"].ToString().Trim() != string.Empty)
    //                {
    //                    dr["TypeID"] = dict["hdnTypeId"].ToString();
    //                }
    //                dr["ItemDesc"] = dict["txtGvItem"].ToString();
    //                if (!(dict["hdOpSq"].ToString().Trim() == string.Empty))
    //                {
    //                    dr["OpSq"] = dict["hdOpSq"].ToString();
    //                }
    //                else
    //                {
    //                    dr["OpSq"] = "100";
    //                }
    //                if (dict["hdnLine"].ToString().Trim() != string.Empty)
    //                    dr["Line"] = dict["hdnLine"].ToString();
    //                if (dict["hdnPrvInQuan"].ToString().Trim() != string.Empty)
    //                    dr["PrvInQuan"] = Convert.ToDouble(dict["hdnPrvInQuan"]);
    //                if (dict["hdnPrvIn"].ToString().Trim() != string.Empty)
    //                    dr["PrvIn"] = Convert.ToDouble(dict["hdnPrvIn"]);
    //                if (dict["hdnOutstandQuan"].ToString().Trim() != string.Empty)
    //                    dr["OutstandQuan"] = Convert.ToDouble(dict["hdnOutstandQuan"]);
    //                if (dict["hdnOutstandBalance"].ToString().Trim() != string.Empty)
    //                    dr["OutstandBalance"] = Convert.ToDouble(dict["hdnOutstandBalance"]);

    //                BL_Inventory DL_INV = new BL_Inventory();

    //                if (DL_INV.ISINVENTORYTRACKINGISON(Session["config"].ToString())) { dr["Warehouse"] = "22"; }  //Temporally for A4 Access We set default warehouse 22 , Then later we will need to allow user to select warehouse and location
    //                else
    //                {
    //                    dr["Warehouse"] = "";
    //                }

    //                dr["WHLocID"] = "0";
    //                if (dict["hdnchkTaxable"].ToString().Trim() != string.Empty)
    //                {

    //                    dr["stax"] = Convert.ToInt32(dict["hdnchkTaxable"]);
    //                }

    //                if (dict.ContainsKey("hdnSTaxAm"))
    //                {
    //                    dr["STaxName"] = hdnSTaxName.Value.ToString();
    //                    dr["STaxRate"] = Convert.ToDecimal(hdnQST.Value);
    //                    //dr["STaxAmt"] = dict["hdnSTaxAm"].ToString();
    //                    if (dict["hdnSTaxAm"].ToString().Trim() != string.Empty)
    //                    {
    //                        dr["STaxAmt"] = dict["hdnSTaxAm"].ToString();
    //                    }
    //                    else
    //                    {
    //                        dr["STaxAmt"] = 0;
    //                    }
    //                }
    //                else
    //                {
    //                    dr["STaxName"] = "";
    //                    dr["STaxRate"] = 0;
    //                    dr["STaxAmt"] = 0;
    //                }
    //                if (dict.ContainsKey("hdnSTaxGL"))
    //                {
    //                    //dr["STaxGL"] = dict["hdnSTaxGL"].ToString();
    //                    if (dict["hdnSTaxGL"].ToString().Trim() != string.Empty)
    //                    {
    //                        dr["STaxGL"] = dict["hdnSTaxGL"].ToString();
    //                    }
    //                    else
    //                    {
    //                        dr["STaxGL"] = 0;
    //                    }
    //                }
    //                else
    //                {
    //                    dr["STaxGL"] = 0;
    //                }

    //                if (dict.ContainsKey("hdnGSTTaxAm"))
    //                {
    //                    dr["GSTRate"] = Convert.ToDecimal(hdnGST.Value);
    //                    //dr["GTaxAmt"] = dict["hdnGSTTaxAm"].ToString();
    //                    if (dict["hdnGSTTaxAm"].ToString().Trim() != string.Empty)
    //                    {
    //                        dr["GTaxAmt"] = dict["hdnGSTTaxAm"].ToString();
    //                    }
    //                    else
    //                    {
    //                        dr["GTaxAmt"] = 0;
    //                    }
    //                }
    //                else
    //                {
    //                    dr["GSTRate"] = 0;
    //                    dr["GTaxAmt"] = 0;
    //                }
    //                if (dict.ContainsKey("hdnGSTTaxGL"))
    //                {
    //                    //dr["GSTTaxGL"] = dict["hdnGSTTaxGL"].ToString();
    //                    if (dict["hdnGSTTaxGL"].ToString().Trim() != string.Empty)
    //                    {
    //                        dr["GSTTaxGL"] = dict["hdnGSTTaxGL"].ToString();
    //                    }
    //                    else
    //                    {
    //                        dr["GSTTaxGL"] = 0;
    //                    }
    //                }
    //                else
    //                {
    //                    dr["GSTTaxGL"] = 0;
    //                }

    //                dt.Rows.Add(dr);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    return dt;
    //}
    private DataTable GetCurrentTransaction()
    {
        DataTable dt = GetTable();

        try
        {
            BL_Inventory DL_INV = new BL_Inventory();

            bool inv = false;

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/AddCheck_ISINVENTORYTRACKINGISON";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, Session["config"].ToString());
                inv = Convert.ToBoolean(_APIResponse.ResponseData);
            }
            else
            {
                inv = DL_INV.ISINVENTORYTRACKINGISON(Session["config"].ToString());
            }

            foreach (GridDataItem row in RadGrid_gvJobCostItems.Items)
            {
                HiddenField hdnTID = (HiddenField)row.FindControl("hdnTID");
                HiddenField hdnAcctID = (HiddenField)row.FindControl("hdnAcctID");
                TextBox txtGvAcctNo = (TextBox)row.FindControl("txtGvAcctNo");
                TextBox txtGvDesc = (TextBox)row.FindControl("txtGvDesc");
                TextBox txtGvAmount = (TextBox)row.FindControl("txtGvAmount");
                TextBox txtGvUseTax = (TextBox)row.FindControl("txtGvUseTax");
                HiddenField hdnUtax = (HiddenField)row.FindControl("hdnUtax");
                Label lblTID = (Label)row.FindControl("lblTID");

                TextBox txtGvLoc = (TextBox)row.FindControl("txtGvLoc");
                TextBox txtGvJob = (TextBox)row.FindControl("txtGvJob");
                TextBox txtGvTicket = (TextBox)row.FindControl("txtGvTicket");
                HiddenField hdnJobID = (HiddenField)row.FindControl("hdnJobID");
                TextBox txtGvPhase = (TextBox)row.FindControl("txtGvPhase");
                HiddenField hdnPID = (HiddenField)row.FindControl("hdnPID");
                TextBox txtGvItem = (TextBox)row.FindControl("txtGvItem");
                HiddenField hdnItemID = (HiddenField)row.FindControl("hdnItemID");
                HiddenField hdnQuantity = (HiddenField)row.FindControl("hdnQuantity");
                TextBox txtGvQuan = (TextBox)row.FindControl("txtGvQuan");
                HiddenField hdnLine = (HiddenField)row.FindControl("hdnLine");
                HiddenField hdnPrvInQuan = (HiddenField)row.FindControl("hdnPrvInQuan");
                HiddenField hdnPrvIn = (HiddenField)row.FindControl("hdnPrvIn");
                HiddenField hdnOutstandQuan = (HiddenField)row.FindControl("hdnOutstandQuan");
                HiddenField hdnOutstandBalance = (HiddenField)row.FindControl("hdnOutstandBalance");
                HiddenField hdnUtaxGL = (HiddenField)row.FindControl("hdnUtaxGL");
                HiddenField hdnTypeId = (HiddenField)row.FindControl("hdnTypeId");
                HiddenField hdOpSq = (HiddenField)row.FindControl("hdOpSq");

                HiddenField hdnchkTaxable = (HiddenField)row.FindControl("hdnchkTaxable");
                HiddenField hdnSTaxAm = (HiddenField)row.FindControl("hdnSTaxAm");
                HiddenField hdnSTaxGL = (HiddenField)row.FindControl("hdnSTaxGL");
                HiddenField hdnGSTTaxAm = (HiddenField)row.FindControl("hdnGSTTaxAm");
                HiddenField hdnGSTTaxGL = (HiddenField)row.FindControl("hdnGSTTaxGL");
                Label lblAmountWithTax = (Label)row.FindControl("lblAmountWithTax");
                TextBox lblSalesTax = (TextBox)row.FindControl("lblSalesTax");
                TextBox lblGstTax = (TextBox)row.FindControl("lblGstTax");
                CheckBox chkTaxable = (CheckBox)row.FindControl("chkTaxable");
                HiddenField hdnAmountWithTax = (HiddenField)row.FindControl("hdnAmountWithTax");
                HiddenField hdnchkGTaxable = (HiddenField)row.FindControl("hdnchkGTaxable");
                CheckBox chkGTaxable = (CheckBox)row.FindControl("chkGTaxable");
                TextBox txtGvPrice = (TextBox)row.FindControl("txtGvPrice");
                var dr = dt.NewRow();
                if (!string.IsNullOrEmpty(hdnTID.Value))
                {
                    dr["ID"] = hdnTID.Value;
                }
                if (!string.IsNullOrEmpty(hdnAcctID.Value))
                {
                    dr["AcctID"] = hdnAcctID.Value;
                }
                if (!string.IsNullOrEmpty(txtGvAmount.Text))
                {
                    dr["Amount"] = txtGvAmount.Text;
                }
                if (!string.IsNullOrEmpty(txtGvUseTax.Text))
                {
                    dr["Usetax"] = txtGvUseTax.Text;
                }
                if (!string.IsNullOrEmpty(hdnJobID.Value))
                {
                    dr["JobID"] = hdnJobID.Value;
                }
                if (!string.IsNullOrEmpty(hdnPID.Value))
                {
                    dr["PhaseID"] = hdnPID.Value;
                }
                if (!string.IsNullOrEmpty(hdnItemID.Value))
                {
                    dr["ItemID"] = hdnItemID.Value;
                }
                if (!string.IsNullOrEmpty(hdnUtaxGL.Value))
                {
                    dr["UtaxGL"] = hdnUtaxGL.Value;
                }
                if (!string.IsNullOrEmpty(hdnTypeId.Value))
                {
                    dr["TypeID"] = hdnTypeId.Value;
                }
                if (!string.IsNullOrEmpty(txtGvQuan.Text))
                {
                    dr["Quan"] = txtGvQuan.Text;
                }
                if (!string.IsNullOrEmpty(txtGvTicket.Text))
                {
                    dr["Ticket"] = txtGvTicket.Text;
                }

                dr["fDesc"] = txtGvDesc.Text;
                dr["UtaxName"] = hdnUtax.Value;
                dr["AcctNo"] = txtGvAcctNo.Text;
                dr["JobName"] = txtGvJob.Text;
                dr["Phase"] = txtGvPhase.Text;
                dr["UName"] = hdnUtax.Value;
                dr["ItemDesc"] = txtGvItem.Text;
                dr["Loc"] = txtGvLoc.Text;
                dr["TypeDesc"] = "";
                dr["OpSq"] = hdOpSq.Value;

                if (!string.IsNullOrEmpty(hdnLine.Value))
                {
                    dr["Line"] = hdnLine.Value;
                }
                if (!string.IsNullOrEmpty(hdnPrvInQuan.Value))
                {
                    dr["PrvInQuan"] = hdnPrvInQuan.Value;
                }
                if (!string.IsNullOrEmpty(hdnPrvIn.Value))
                {
                    dr["PrvIn"] = hdnPrvIn.Value;
                }
                if (!string.IsNullOrEmpty(hdnOutstandQuan.Value))
                {
                    dr["OutstandQuan"] = hdnOutstandQuan.Value;
                }
                if (!string.IsNullOrEmpty(hdnOutstandBalance.Value))
                {
                    dr["OutstandBalance"] = hdnOutstandBalance.Value;
                }

                if (inv)
                {
                    dr["Warehouse"] = "22";
                }
                else
                {
                    dr["Warehouse"] = "";
                }

                dr["WHLocID"] = "0";
                if (chkTaxable.Checked == true)
                {
                    dr["stax"] = "1";
                }
                else
                {
                    dr["stax"] = "0";
                }
                //dr["stax"] = Convert.ToInt32(hdnchkTaxable.Value);


                if (!string.IsNullOrEmpty(hdnSTaxAm.Value))
                {
                    dr["STaxName"] = hdnSTaxName.Value.ToString();
                    dr["STaxRate"] = Convert.ToDecimal(hdnQST.Value);
                    dr["STaxAmt"] = hdnSTaxAm.Value.ToString();
                }
                else
                {
                    dr["STaxName"] = "";
                    dr["STaxRate"] = DBNull.Value;
                    dr["STaxAmt"] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(hdnSTaxGL.Value))
                {
                    dr["STaxGL"] = hdnSTaxGL.Value;
                }
                else
                {
                    dr["STaxGL"] = DBNull.Value;
                }

                if (!string.IsNullOrEmpty(hdnGSTTaxAm.Value))
                {
                    dr["GSTRate"] = Convert.ToDecimal(hdnGST.Value);
                    dr["GTaxAmt"] = hdnGSTTaxAm.Value;
                }
                else
                {
                    dr["GSTRate"] = DBNull.Value;
                    dr["GTaxAmt"] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(hdnGSTTaxGL.Value))
                {
                    dr["GSTTaxGL"] = hdnGSTTaxGL.Value;
                }
                else
                {
                    dr["GSTTaxGL"] = DBNull.Value;
                }
                if (!string.IsNullOrEmpty(hdnAmountWithTax.Value))
                {
                    dr["AmountTot"] = hdnAmountWithTax.Value;
                }
                else
                {
                    dr["AmountTot"] = "0.00";
                }
                if (chkGTaxable.Checked == true)
                {
                    dr["GTax"] = "1";
                }
                else
                {
                    dr["GTax"] = "0";
                }
                if (!string.IsNullOrEmpty(txtGvPrice.Text))
                {
                    dr["Price"] = txtGvPrice.Text;
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private DataTable GetBillItems_New(DataTable dtt)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("fDate", typeof(DateTime));
        dt.Columns.Add("PJID", typeof(int));
        dt.Columns.Add("Ref", typeof(string));
        dt.Columns.Add("TRID", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Spec", typeof(int));
        dt.Columns.Add("Original", typeof(double));
        dt.Columns.Add("Balance", typeof(double));
        dt.Columns.Add("Disc", typeof(double));
        dt.Columns.Add("Paid", typeof(double));

        try
        {
            //foreach (GridViewRow gr in gvBills.Rows)
            foreach (DataRow gr in dtt.Rows)
            {
                    DataRow dr = dt.NewRow();
                    dr["fDate"] = Convert.ToDateTime(gr["fDate"]);
                    dr["PJID"] = Convert.ToInt32(gr["PJID"]);
                    dr["Ref"] = gr["Ref"];
                    dr["TRID"] = Convert.ToInt32(gr["TRID"]);
                    dr["fDesc"] = gr["fDesc"];
                    dr["Spec"] = gr["Spec"];
                    dr["Original"] = gr["Original"];
                dr["Balance"] = gr["Balance"];
                dr["Disc"] = gr["Disc"];
                dr["Paid"] = gr["Balance"];
                dt.Rows.Add(dr);
                
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return dt;
    }
    public DataTable GetTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("AcctID", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Amount", typeof(double));
        dt.Columns.Add("Usetax", typeof(string));
        dt.Columns.Add("UtaxName", typeof(string));
        dt.Columns.Add("JobID", typeof(int));
        dt.Columns.Add("PhaseID", typeof(int));
        dt.Columns.Add("ItemID", typeof(int));

        dt.Columns.Add("AcctNo", typeof(string));
        dt.Columns.Add("JobName", typeof(string));
        dt.Columns.Add("Phase", typeof(string));
        dt.Columns.Add("UName", typeof(string));
        dt.Columns.Add("UtaxGL", typeof(Int32));
        dt.Columns.Add("ItemDesc", typeof(string));
        dt.Columns.Add("TypeID", typeof(Int32));
        dt.Columns.Add("Loc", typeof(string));
        dt.Columns.Add("TypeDesc", typeof(string));
        dt.Columns.Add("Quan", typeof(double));
        dt.Columns.Add("Ticket", typeof(Int32));
        dt.Columns.Add("OpSq", typeof(string));

        dt.Columns.Add("Warehouse", typeof(string));
        dt.Columns.Add("WHLocID", typeof(int));

        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("PrvInQuan", typeof(double));
        dt.Columns.Add("PrvIn", typeof(double));
        dt.Columns.Add("OutstandQuan", typeof(double));
        dt.Columns.Add("OutstandBalance", typeof(double));

        dt.Columns.Add("STax", typeof(int));
        dt.Columns.Add("STaxName", typeof(string));
        dt.Columns.Add("STaxRate", typeof(double));
        dt.Columns.Add("StaxAmt", typeof(double));
        dt.Columns.Add("STaxGL", typeof(Int32));
        dt.Columns.Add("GSTRate", typeof(double));
        dt.Columns.Add("GTaxAmt", typeof(double));
        dt.Columns.Add("GSTTaxGL", typeof(Int32));
        dt.Columns.Add("AmountTot", typeof(double));
        dt.Columns.Add("IsPO", typeof(int));
        dt.Columns.Add("GTax", typeof(int));
        dt.Columns.Add("Price", typeof(double));

        return dt;

        //return dt;
    }
    private void SetInitialRow()            //Initialization of Datatable.
    {
        try
        {
            int rowIndex = 0;
            DataTable dtJob = new DataTable();

            dtJob.Columns.Add(new DataColumn("RowID", typeof(string)));
            dtJob.Columns.Add(new DataColumn("ID", typeof(Int32)));
            dtJob.Columns.Add(new DataColumn("AcctID", typeof(Int32)));
            dtJob.Columns.Add(new DataColumn("AcctNo", typeof(string)));
            //dtJob.Columns.Add(new DataColumn("Account", typeof(string)));
            dtJob.Columns.Add(new DataColumn("fDesc", typeof(string)));
            dtJob.Columns.Add(new DataColumn("Amount", typeof(string)));
            dtJob.Columns.Add(new DataColumn("UseTax", typeof(string)));
            dtJob.Columns.Add(new DataColumn("Loc", typeof(string)));
            dtJob.Columns.Add(new DataColumn("JobName", typeof(string)));
            dtJob.Columns.Add(new DataColumn("JobID", typeof(string)));
            dtJob.Columns.Add(new DataColumn("Phase", typeof(string)));
            dtJob.Columns.Add(new DataColumn("PhaseID", typeof(Int32)));
            dtJob.Columns.Add(new DataColumn("UName", typeof(string)));
            dtJob.Columns.Add(new DataColumn("UtaxGL", typeof(Int32)));
            dtJob.Columns.Add(new DataColumn("ItemDesc", typeof(string)));
            dtJob.Columns.Add(new DataColumn("ItemID", typeof(Int32)));
            dtJob.Columns.Add(new DataColumn("TypeID", typeof(Int32)));
            dtJob.Columns.Add(new DataColumn("Quan", typeof(String)));
            dtJob.Columns.Add(new DataColumn("Ticket", typeof(Int32)));
            dtJob.Columns.Add(new DataColumn("OpSq", typeof(string)));// Ticket 

            dtJob.Columns.Add(new DataColumn("Warehouse", typeof(string)));
            dtJob.Columns.Add(new DataColumn("WHLocID", typeof(int)));
            dtJob.Columns.Add(new DataColumn("Line", typeof(int)));
            dtJob.Columns.Add(new DataColumn("PrvInQuan", typeof(int)));
            dtJob.Columns.Add(new DataColumn("PrvIn", typeof(int)));
            dtJob.Columns.Add(new DataColumn("OutstandQuan", typeof(int)));
            dtJob.Columns.Add(new DataColumn("OutstandBalance", typeof(int)));

            dtJob.Columns.Add(new DataColumn("STax", typeof(int)));
            dtJob.Columns.Add(new DataColumn("STaxName", typeof(string)));
            dtJob.Columns.Add(new DataColumn("STaxRate", typeof(double)));
            dtJob.Columns.Add(new DataColumn("StaxAmt", typeof(double)));
            dtJob.Columns.Add(new DataColumn("STaxGL", typeof(Int32)));
            dtJob.Columns.Add(new DataColumn("GSTRate", typeof(double)));
            dtJob.Columns.Add(new DataColumn("GTaxAmt", typeof(double)));
            dtJob.Columns.Add(new DataColumn("GSTTaxGL", typeof(Int32)));
            dtJob.Columns.Add(new DataColumn("AmountTot", typeof(string)));

            dtJob.Columns.Add(new DataColumn("GTax", typeof(int)));
            dtJob.Columns.Add(new DataColumn("Price", typeof(double)));
            rowIndex = 0;

            DataRow drJob = dtJob.NewRow();
            drJob["STax"] = 0;
            drJob["GTax"] = 0;
            dtJob.Rows.Add(drJob);

            ViewState["Transactions_JobCost"] = dtJob;

            BINDGRID(dtJob);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private bool ValidateGrid(DataTable dt)
    {
        bool Flag = false;
        bool IsExist;
        bool IsValid = true;
        if (string.IsNullOrEmpty(hdnVendorID.Value) && !string.IsNullOrEmpty(txtVendor.Text))
        {
            Vendor _objVendor = new Vendor();
            BL_Vendor _objBLVendor = new BL_Vendor();
            DataSet ds = new DataSet();
            _objVendor.SearchValue = txtVendor.Text;
            _objVendor.ConnConfig = Session["config"].ToString();

            _getVendorByName.SearchValue = txtVendor.Text;
            _getVendorByName.ConnConfig = Session["config"].ToString();

            if (Session["CmpChkDefault"].ToString() == "1")
            {
                _objVendor.EN = 1;
                _getVendorByName.EN = 1;
            }
            else
            {
                _objVendor.EN = 0;
                _getVendorByName.EN = 0;
            }

            List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/AddCheck_GetVendorByName";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorByName);

                _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
                ds = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
            }
            else
            {
                ds = _objBLVendor.GetVendorByName(_objVendor);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                hdnVendorID.Value = ds.Tables[0].Rows[0]["ID"].ToString();
            }
        }

        try
        {
            _objPJ.Vendor = Convert.ToInt32(hdnVendorID.Value);
        }
        catch (Exception)
        {
            //ClientScript.RegisterStartupScript(Page.GetType(), "vendorWarning", "noty({text: 'Can't find vendor information! Please help to re-fill the vendor again.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: 'Cannot find vendor information! Please help to re-fill the vendor again.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
            return false;
        }
        _objPJ.ConnConfig = Session["config"].ToString();
        _objPJ.Ref = "";
        //_objPJ.Vendor = Convert.ToInt32(hdnVendorID.Value);

        
        
            // IsExist = _objBLBills.IsBillExistForInsert(_objPJ);
            IsExist = false;
            GetPeriodDetails(Convert.ToDateTime(txtDate.Text));

            Flag = (bool)ViewState["FlagPeriodClose"];
        

        if (IsExist.Equals(false))
        {
            if (Flag)
            {
                string strMessage = "";
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["JobName"].ToString() != "")
                    {
                        if (dr["Phase"].ToString() == "")
                        {

                            IsValid = false;
                            strMessage = "Please enter a type for the Project." + dr["JobName"].ToString();
                        }

                    }
                    if ((dr["Amount"].ToString() == "") || (dr["fDesc"].ToString() == "") || (dr["AcctID"].ToString() == ""))
                    {
                        IsValid = false;
                        strMessage = "Item description, acct no. and amount are required.";
                    }
                }
                if (dt.Rows.Count.Equals(0))
                {
                    IsValid = false;
                    strMessage = "You must have at least one item on the purchase order.";
                }

                if (IsValid.Equals(false))
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + strMessage + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
            else
            {
                IsValid = false;
                divSuccess.Visible = true;
            }
        }
        else
        {
            IsValid = false;
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningRef", "noty({text: 'Ref number with this vendor already exists, Please use different Ref number!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);
        }
        return IsValid;
    }
    private void BINDGRID(DataTable dt)
    {
        DataColumnCollection columns = dt.Columns;
        if (!columns.Contains("AmountTot"))
        {
            //dt.Columns.Add("AmountTot", typeof(double), "STaxAmt + GTaxAmt + Amount");
            //dt.AcceptChanges();

            dt.Columns.Add(new DataColumn("AmountTot"));
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                double _sTaxAmt = 0;
                double _gTaxAmt = 0;
                double _sAmt = 0;
                if (!string.IsNullOrEmpty(dt.Rows[i]["STaxAmt"].ToString()))
                {
                    _sTaxAmt = Convert.ToDouble(dt.Rows[i]["STaxAmt"].ToString());
                }
                if (!string.IsNullOrEmpty(dt.Rows[i]["GTaxAmt"].ToString()))
                {
                    _gTaxAmt = Convert.ToDouble(dt.Rows[i]["GTaxAmt"].ToString());
                }
                if (!string.IsNullOrEmpty(dt.Rows[i]["Amount"].ToString()))
                {
                    _sAmt = Convert.ToDouble(dt.Rows[i]["Amount"].ToString());
                }
                dt.Rows[i]["AmountTot"] = _sAmt + _sTaxAmt + _gTaxAmt;
            }
            dt.AcceptChanges();



        }

        RadGrid_gvJobCostItems.DataSource = dt;


        _objPropGeneral.ConnConfig = Session["config"].ToString();
        _objPropGeneral.CustomName = "Country";

        _getCustomFields.ConnConfig = Session["config"].ToString();
        _getCustomFields.CustomName = "Country";

        DataSet dsCustom = new DataSet();
        List<CustomViewModel> _lstCustomViewModel = new List<CustomViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/AddCheck_GetCustomFields";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomFields);

            _lstCustomViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomViewModel>>(_APIResponse.ResponseData);
            dsCustom = CommonMethods.ToDataSet<CustomViewModel>(_lstCustomViewModel);
        }
        else
        {
            dsCustom = _objBLGeneral.getCustomFields(_objPropGeneral);
        }

        if (dsCustom.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(dsCustom.Tables[0].Rows[0]["Label"].ToString()) && dsCustom.Tables[0].Rows[0]["Label"].ToString().Equals("1"))
            {
                //RadGrid_gvJobCostItems.Columns[12].HeaderText = "Provincial Tax";
                //RadGrid_gvJobCostItems.Columns[13].Visible = true;
                RadGrid_gvJobCostItems.Columns[14].HeaderText = "PST Tax";
                RadGrid_gvJobCostItems.Columns[12].Visible = true;
                RadGrid_gvJobCostItems.Columns[11].Visible = true;
                txtgstgv.Visible = true;
                spansalestax.InnerText = "PST Tax";
                //hdnGstTax.Value = dsCustom.Tables[0].Rows[0]["GstRate"].ToString();
                ////////////////////If GST Set 0 Then Again Show Sales Tax intead of Provicinal Tax ES-3180///////////////////////////////////////
                string gst_gstgl = "";
                string gst_gstrate = "";
                _objPropGeneral.ConnConfig = Session["config"].ToString();
                _getCustomFieldsControl.ConnConfig = Session["config"].ToString();

                DataSet _dsCustom = new DataSet();
                List<CustomViewModel> _lstCustomFieldsControl = new List<CustomViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/AddCheck_GetCustomFieldsControl";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomFieldsControl);

                    _lstCustomFieldsControl = (new JavaScriptSerializer()).Deserialize<List<CustomViewModel>>(_APIResponse.ResponseData);
                    _dsCustom = CommonMethods.ToDataSet<CustomViewModel>(_lstCustomFieldsControl);
                }
                else
                {
                    _dsCustom = _objBLGeneral.getCustomFieldsControl(_objPropGeneral);
                }

                if (_dsCustom.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
                    {


                        if (_dr["Name"].ToString().Equals("GSTGL"))
                        {
                            if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
                            {
                                _objChart.ConnConfig = Session["config"].ToString();
                                _objChart.ID = Convert.ToInt32(_dr["Label"].ToString());

                                _getChart.ConnConfig = Session["config"].ToString();
                                _getChart.ID = Convert.ToInt32(_dr["Label"].ToString());

                                DataSet _dsChart = new DataSet();
                                List<ChartViewModel> _lstChartViewModel = new List<ChartViewModel>();

                                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                                //if (IsAPIIntegrationEnable == "YES")
                                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                                {
                                    string APINAME = "ManageChecksAPI/AddCheck_GetChart";

                                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getChart);

                                    _lstChartViewModel = (new JavaScriptSerializer()).Deserialize<List<ChartViewModel>>(_APIResponse.ResponseData);
                                    _dsChart = CommonMethods.ToDataSet<ChartViewModel>(_lstChartViewModel);
                                }
                                else
                                {
                                    _dsChart = _objBLChart.GetChart(_objChart);
                                }

                                if (_dsChart.Tables[0].Rows.Count > 0)
                                {
                                    //txtGSTGL.Text = _dsChart.Tables[0].Rows[0]["fDesc"].ToString();
                                    gst_gstgl = _dr["Label"].ToString();
                                }

                            }
                            else
                            {
                                gst_gstgl = "0";
                            }
                        }
                        else if (_dr["Name"].ToString().Equals("GSTRate"))
                        {
                            if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
                            {
                                gst_gstrate = _dr["Label"].ToString();

                            }
                            else
                            {
                                gst_gstrate = "0";
                            }
                        }

                    }

                }

                if (gst_gstrate == "")
                {
                    gst_gstrate = "0";
                }
                if (gst_gstrate == "0" || gst_gstrate == "0.0000")
                {
                    spansalestax.InnerText = "Sales Tax";
                    RadGrid_gvJobCostItems.Columns[14].HeaderText = "Sales Tax Amount";
                }
                ////////////////////////////////////////////////////////
            }
            else
            {
                RadGrid_gvJobCostItems.Columns[12].Visible = false;
                RadGrid_gvJobCostItems.Columns[11].Visible = false;
                RadGrid_gvJobCostItems.Columns[14].HeaderText = "Sales Tax Amount";
                txtgstgv.Visible = false;
                spansalestax.InnerText = "Sales Tax";
            }
        }
        else
        {
            RadGrid_gvJobCostItems.Columns[12].Visible = false;
            RadGrid_gvJobCostItems.Columns[11].Visible = false;
            RadGrid_gvJobCostItems.Columns[14].HeaderText = "Sales Tax Amount";
            txtgstgv.Visible = false;
            spansalestax.InnerText = "Sales Tax";
        }

        BusinessEntity.User objProp_User = new BusinessEntity.User();
        DataSet ds = new DataSet();
        objProp_User.ConnConfig = Session["config"].ToString();
        _getConnectionConfig.ConnConfig = Session["config"].ToString();


        List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetControl";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

            _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
        }
        else
        {
            ds = objBL_User.getControl(objProp_User);
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (Convert.ToBoolean(ds.Tables[0].Rows[0]["IsUseTaxAPBills"].ToString()) == false)
            {
                RadGrid_gvJobCostItems.Columns[10].Visible = false;
            }
            else
            {
                RadGrid_gvJobCostItems.Columns[10].Visible = true;
            }

            if (Convert.ToBoolean(ds.Tables[0].Rows[0]["IsSalesTaxAPBills"].ToString()) == false)
            {
                RadGrid_gvJobCostItems.Columns[14].Visible = false;
                RadGrid_gvJobCostItems.Columns[13].Visible = false;
                RadGrid_gvJobCostItems.Columns[11].Visible = false;
                txtgstgv.Visible = false;
                RadGrid_gvJobCostItems.Columns[12].Visible = false;
                hdnQST.Value = "0";
                hdnQSTGL.Value = "0";
                hdnSTaxType.Value = "";
                hdnSTaxName.Value = "";
            }
            else
            {
                RadGrid_gvJobCostItems.Columns[14].Visible = true;
                RadGrid_gvJobCostItems.Columns[13].Visible = true;
                
                if (txtgstgv.Visible == true)
                {
                    txtgstgv.Visible = true;
                    RadGrid_gvJobCostItems.Columns[11].Visible = true;
                    RadGrid_gvJobCostItems.Columns[12].Visible = true;
                }


                if (txtgstgv.Visible == true)
                {
                    if (hdnGST.Value.Trim() == "0" || hdnGST.Value.Trim() == "0.0000")
                    {
                        txtgstgv.Visible = false;
                        RadGrid_gvJobCostItems.Columns[11].Visible = false;
                        RadGrid_gvJobCostItems.Columns[12].Visible = false;
                    }
                }
            }
        }


        RadGrid_gvJobCostItems.Rebind();
        foreach (GridDataItem gr in RadGrid_gvJobCostItems.Items)//Project Expense grid 
        {

            TextBox txtGvQuan = (TextBox)gr.FindControl("txtGvQuan");
            TextBox txtGvAmount = (TextBox)gr.FindControl("txtGvAmount");
            TextBox txtGvPrice = (TextBox)gr.FindControl("txtGvPrice");
            if (txtGvQuan.Text != "" && txtGvAmount.Text != "")
            {
                double Qty = 0; double Amount = 0; double Price = 0;

                double.TryParse(txtGvQuan.Text, out Qty);
                double.TryParse(txtGvAmount.Text, out Amount);
                double.TryParse(txtGvPrice.Text, out Price);

                if (Qty == 0)
                {
                    txtGvPrice.Text = string.Empty;
                }
                else
                {
                    if (Price == 0)
                    {
                        Price = (Amount) / (Qty);
                        txtGvPrice.Text = Price.ToString();
                    }
                }
            }
        }
    }
    protected void lbtnAddNewLines_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    int rowIndex;
        //    //Job cost grid view
        //    rowIndex = RadGrid_gvJobCostItems.Items.Count - 1;
        //    GridDataItem row = RadGrid_gvJobCostItems.Items[rowIndex];
        //    Label lblJId = row.FindControl("lblId") as Label;

        //    DataTable dt = GetCurrentTransaction();
        //    if (dt.Rows.Count > 0)
        //    {
        //        DataRow dr = dt.NewRow();
        //        //dr["UseTax"] = txtTotalUseTax.Text;
        //        dr["STax"] = 0;
        //        dt.Rows.Add(dr);
        //    }
        //    else
        //    {
        //        dt = (DataTable)ViewState["Transactions_JobCost"];
        //    }

        //    ViewState["Transactions_JobCost"] = dt;
        //    BINDGRID(dt);

        //    //Focus last row
        //    GridDataItem lastRow = RadGrid_gvJobCostItems.Items[RadGrid_gvJobCostItems.Items.Count - 1];
        //    TextBox txtGvJob = (TextBox)lastRow.FindControl("txtGvJob");
        //    if (txtGvJob != null)
        //    {
        //        txtGvJob.Focus();
        //    }
        //}
        //catch (Exception ex)
        //{
        //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        //    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        //}

        try
        {
            if (hdnVendorID.Value != "")
            {
                //int rowIndex;
                ////Job cost grid view
                //rowIndex = RadGrid_gvJobCostItems.Items.Count - 1;
                //GridDataItem row = RadGrid_gvJobCostItems.Items[rowIndex];
                //Label lblJId = row.FindControl("lblId") as Label;

                //DataTable dt = GetCurrentTransaction();



                int rowIndex;
                //Job cost grid view
                rowIndex = RadGrid_gvJobCostItems.Items.Count - 1;
                if (RadGrid_gvJobCostItems.Items.Count > 0)
                {
                    GridDataItem row = RadGrid_gvJobCostItems.Items[rowIndex];
                    Label lblJId = row.FindControl("lblId") as Label;
                }
                DataTable dt = GetCurrentTransaction();
                //if (dt.Rows.Count > 0)
                //{
                DataRow dr = dt.NewRow();
                dr["STax"] = 0;
                dr["GTax"] = 0;
                dt.Rows.Add(dr);
                dt.AcceptChanges();

                ViewState["Transactions_JobCost"] = dt;
                BINDGRID(dt);

                //if (dt.Rows.Count > 0)
                //{
                //    DataRow dr = dt.NewRow();
                //    //dr["UseTax"] = txtTotalUseTax.Text;
                //    dr["STax"] = 0;
                //    dt.Rows.Add(dr);
                //}
                //else
                //{
                //    dt = (DataTable)ViewState["Transactions_JobCost"];
                //}

                //ViewState["Transactions_JobCost"] = dt;
                //BINDGRID(dt);

                //Focus last row
                GridDataItem lastRow = RadGrid_gvJobCostItems.Items[RadGrid_gvJobCostItems.Items.Count - 1];
                TextBox txtGvJob = (TextBox)lastRow.FindControl("txtGvJob");
                if (txtGvJob != null)
                {
                    txtGvJob.Focus();
                }
            }
            else
            {
                string str = "Please select vendor first";
                //ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: 'Please select vendor first.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true});", true);

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void btnCopyPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            var selectIndex = 0;

            if (!string.IsNullOrEmpty(hdnSelectPOIndex.Value))
            {
                selectIndex = Convert.ToInt32(hdnSelectPOIndex.Value);
            }
            else
            {
                var selectItem = RadGrid_gvJobCostItems.MasterTableView.GetSelectedItems();
                if (selectItem.Count() > 0)
                {
                    selectIndex = selectItem[0].ClientRowIndex;
                }
            }

            var dt = GetCurrentTransaction();
            if (dt.Rows.Count > 0 && selectIndex > 0)
            {
                var copyRow = dt.Rows[selectIndex - 1];
                var dr = dt.Rows[selectIndex];

                dr["ID"] = copyRow["ID"];
                dr["AcctID"] = copyRow["AcctID"];
                dr["fDesc"] = copyRow["fDesc"];
                dr["Amount"] = copyRow["Amount"];
                dr["Usetax"] = copyRow["Usetax"];
                dr["UtaxName"] = copyRow["UtaxName"];
                dr["JobID"] = copyRow["JobID"];
                dr["PhaseID"] = copyRow["PhaseID"];
                dr["ItemID"] = copyRow["ItemID"];

                dr["AcctNo"] = copyRow["AcctNo"];
                dr["JobName"] = copyRow["JobName"];
                dr["Phase"] = copyRow["Phase"];
                dr["UName"] = copyRow["UName"];
                dr["UtaxGL"] = copyRow["UtaxGL"];
                dr["ItemDesc"] = copyRow["ItemDesc"];
                dr["TypeID"] = copyRow["TypeID"];
                dr["Loc"] = copyRow["Loc"];
                dr["TypeDesc"] = copyRow["TypeDesc"];
                dr["Quan"] = copyRow["Quan"];
                dr["Ticket"] = copyRow["Ticket"];
                dr["OpSq"] = copyRow["OpSq"];

                dr["Warehouse"] = copyRow["Warehouse"];
                dr["WHLocID"] = copyRow["WHLocID"];

                dr["Line"] = copyRow["Line"];
                dr["PrvInQuan"] = copyRow["PrvInQuan"];
                dr["PrvIn"] = copyRow["PrvIn"];
                dr["OutstandQuan"] = copyRow["OutstandQuan"];
                dr["OutstandBalance"] = copyRow["OutstandBalance"];

                dr["STax"] = copyRow["STax"];
                dr["STaxName"] = copyRow["STaxName"];
                dr["STaxName"] = copyRow["STaxName"];
                dr["STaxRate"] = copyRow["STaxRate"];
                dr["StaxAmt"] = copyRow["StaxAmt"];
                dr["STaxGL"] = copyRow["STaxGL"];
                dr["GSTRate"] = copyRow["GSTRate"];
                dr["GTaxAmt"] = copyRow["GTaxAmt"];
                dr["GSTTaxGL"] = copyRow["GSTTaxGL"];
                dr["AmountTot"] = copyRow["AmountTot"];
                dr["GTax"] = copyRow["GTax"];
                dr["Price"] = copyRow["Price"];
                dt.AcceptChanges();

                ViewState["Transactions_JobCost"] = dt;
                BINDGRID(dt);

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "CalculateTotalUseTaxExpense", "CalculateTotalUseTaxExpense();", true);

                //Focus row
                GridDataItem focusRow = RadGrid_gvJobCostItems.Items[selectIndex];
                TextBox txtGvJob = (TextBox)focusRow.FindControl("txtGvJob");
                if (txtGvJob != null)
                {
                    txtGvJob.Focus();
                }
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }
            if (Request.QueryString["bill"] == null)
            {
                
                srchpane.Style["display"] = "block";
                dpane.Style["display"] = "none";
                dvbilldetailtop.Style["display"] = "block";
                accrdGlAccount.Style["display"] = "none";


                if (!IsPostBack)
                {
                    
                    CompanyPermission();
                    FillBank();
                    FillVendor();
                    FillPayment();
                    //SetBankDetails();
                    FillFrequency();
                    SetTax();
                    ResetForm();
                    StiWebDesigner1.ShowReportTree = StiWebDesigner1.ShowPropertiesGrid = StiWebDesigner1.ShowDictionary = false;
                    StiWebDesigner2.ShowReportTree = StiWebDesigner2.ShowPropertiesGrid = StiWebDesigner2.ShowDictionary = false;
                    StiWebDesigner3.ShowReportTree = StiWebDesigner3.ShowPropertiesGrid = StiWebDesigner3.ShowDictionary = false;
                }
                Permission();
                UserPermission();
                HighlightSideMenu("acctPayable", "lnkWriteCheck2", "acctPayableSub");
                if (!IsPostBack)
                {
                    string path = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/");
                    DirectoryInfo d = new DirectoryInfo(path);
                    FileInfo[] Files = d.GetFiles("*.mrt");
                    foreach (FileInfo file in Files)
                    {

                        string FileName = string.Empty;
                        if (file.Name.Contains(".mrt"))
                            FileName = file.Name.Replace(".mrt", " ");
                        ddlApTopCheckForLoad.Items.Add((FileName));
                    }

                    ddlApTopCheckForLoad.DataBind();


                    string MidCheckpath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/");
                    DirectoryInfo dirMidPath = new DirectoryInfo(MidCheckpath);
                    FileInfo[] FilesMid = dirMidPath.GetFiles("*.mrt");
                    foreach (FileInfo fileMid in FilesMid)
                    {
                        string FileName = string.Empty;
                        if (fileMid.Name.Contains(".mrt"))
                            FileName = fileMid.Name.Replace(".mrt", " ");
                        ddlApMiddleCheckForLoad.Items.Add((FileName));
                    }

                    ddlApMiddleCheckForLoad.DataBind();


                    string TopCheckpath = Server.MapPath("StimulsoftReports/APChecks/TopChecks/");
                    DirectoryInfo dirTopcheckPath = new DirectoryInfo(TopCheckpath);
                    FileInfo[] FilesTop = dirTopcheckPath.GetFiles("*.mrt");
                    foreach (FileInfo fileTop in FilesTop)
                    {
                        string FileName = string.Empty;
                        if (fileTop.Name.Contains(".mrt"))
                            FileName = fileTop.Name.Replace(".mrt", " ");
                        ddlTopChecksForLoad.Items.Add((FileName));
                    }
                    ddlTopChecksForLoad.DataBind();

                    _objUser.ConnConfig = Session["config"].ToString();
                    _objUser.MOMUSer = Session["Username"].ToString();

                    _getCheckTemplate.ConnConfig = Session["config"].ToString();
                    _getCheckTemplate.MOMUSer = Session["Username"].ToString();

                    DataSet dsusercdtemp = new DataSet();

                    List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/AddCheck_GetCheckTemplate";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCheckTemplate);

                        _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                        dsusercdtemp = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
                    }
                    else
                    {
                        dsusercdtemp = _objBLBill.GetCheckTemplate(_objUser);
                    }

                    if (dsusercdtemp.Tables[0].Rows.Count > 0)
                    {
                        if (dsusercdtemp.Tables[0].Rows[0]["CD_Template"].ToString() != "")
                        {
                            if (ddlApTopCheckForLoad.Items.FindByText(dsusercdtemp.Tables[0].Rows[0]["CD_Template"].ToString()) != null)
                            {
                                ddlApTopCheckForLoad.Items.FindByText(dsusercdtemp.Tables[0].Rows[0]["CD_Template"].ToString()).Selected = true;
                            }
                            else if (ddlApMiddleCheckForLoad.Items.FindByText(dsusercdtemp.Tables[0].Rows[0]["CD_Template"].ToString()) != null)
                            {
                                ddlApMiddleCheckForLoad.Items.FindByText(dsusercdtemp.Tables[0].Rows[0]["CD_Template"].ToString()).Selected = true;
                            }
                            else if (ddlTopChecksForLoad.Items.FindByText(dsusercdtemp.Tables[0].Rows[0]["CD_Template"].ToString()) != null)
                            {
                                ddlTopChecksForLoad.Items.FindByText(dsusercdtemp.Tables[0].Rows[0]["CD_Template"].ToString()).Selected = true;
                            }
                        }
                    }

                    //objProp_User.ConnConfig = Session["config"].ToString();
                    //DataSet dsControl = new DataSet();
                    //dsControl = objBL_User.getControl(objProp_User);


                    //if (dsControl.Tables[0].Rows.Count > 0)
                    //{
                    //    lbltopcom.Text = dsControl.Tables[0].Rows[0]["Name"].ToString();
                    //    lblmidcom.Text = dsControl.Tables[0].Rows[0]["Name"].ToString();
                    //    lbldetailcom.Text = dsControl.Tables[0].Rows[0]["Name"].ToString();
                    //    lbltopdd.Text = dsControl.Tables[0].Rows[0]["Address"].ToString() + " " + dsControl.Tables[0].Rows[0]["City"].ToString() + " " + dsControl.Tables[0].Rows[0]["State"].ToString() + ", " + dsControl.Tables[0].Rows[0]["Zip"].ToString();
                    //    lblmidadd.Text = dsControl.Tables[0].Rows[0]["Address"].ToString() + " " + dsControl.Tables[0].Rows[0]["City"].ToString() + " " + dsControl.Tables[0].Rows[0]["State"].ToString() + ", " + dsControl.Tables[0].Rows[0]["Zip"].ToString();
                    //    lbldetailadd.Text = dsControl.Tables[0].Rows[0]["Address"].ToString() + " " + dsControl.Tables[0].Rows[0]["City"].ToString() + " " + dsControl.Tables[0].Rows[0]["State"].ToString() + ", " + dsControl.Tables[0].Rows[0]["Zip"].ToString();
                    //    lbltopemail.Text = dsControl.Tables[0].Rows[0]["Email"].ToString();
                    //    lblmidemail.Text = dsControl.Tables[0].Rows[0]["Email"].ToString();
                    //    lbldetailemail.Text = dsControl.Tables[0].Rows[0]["Email"].ToString();
                    //}

                }

            }
            else if (Request.QueryString["bill"] == "c")
            {
                 


                if (!IsPostBack)
                {
                     CompanyPermission();
                    FillBank();
                    FillVendor();
                    //FillVendorbill();
                    FillPayment();
                    //SetBankDetails();
                    FillFrequency();
                    SetTax();
                    ResetForm();
                    StiWebDesigner1.ShowReportTree = StiWebDesigner1.ShowPropertiesGrid = StiWebDesigner1.ShowDictionary = false;
                    StiWebDesigner2.ShowReportTree = StiWebDesigner2.ShowPropertiesGrid = StiWebDesigner2.ShowDictionary = false;
                    StiWebDesigner3.ShowReportTree = StiWebDesigner3.ShowPropertiesGrid = StiWebDesigner3.ShowDictionary = false;
                    SetInitialRow();
                    if (Request.QueryString["vid"] != null)
                    {
                        ddlVendor.SelectedValue = Convert.ToString(Request.QueryString["vid"]);
                        ddlVendor_SelectedIndexChanged(sender, e);
                        hdnVendorID.Value = Convert.ToString(Request.QueryString["vid"]);
                        txtVendor.Text = ddlVendor.SelectedItem.Text;
                        if (Request.QueryString["ref"] != null)
                        {
                            chkIsRecurr.Visible = false;
                            
                        }
                            //DataTable dtbills = (DataTable)Session["dsBills"];
                            //foreach (GridDataItem gr in gvBills.Items)
                            //{
                            //    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                            //    TextBox txtGvDisc = (TextBox)gr.FindControl("txtGvDisc");
                            //    HiddenField hdnSelected = (HiddenField)gr.FindControl("hdnSelected");
                            //    HiddenField hdnOriginal = (HiddenField)gr.FindControl("hdnOriginal");
                            //    Label lblBalance = (Label)gr.FindControl("lblBalance");
                            //    lblBalance.Text = string.Format("{0:c}", (Convert.ToDouble(hdnOriginal.Value) - Convert.ToDouble(hdnSelected.Value) - Convert.ToDouble(txtGvDisc.Text)));
                            //    _totalOrginal = _totalOrginal + Convert.ToDouble(hdnOriginal.Value);
                            //    if (chkSelect.Checked == true)
                            //    {
                            //        //TextBox txtGvDisc = (TextBox)gr.FindControl("txtGvDisc");
                            //        TextBox txtGvPay = (TextBox)gr.FindControl("txtGvPay");

                            //        _totalPay = _totalPay + Convert.ToDouble(txtGvPay.Text);
                            //        _totalDisc = _totalDisc + Convert.ToDouble(txtGvDisc.Text);
                            //        lblBalance.Text = string.Format("{0:c}", (Convert.ToDouble(hdnOriginal.Value) - Convert.ToDouble(hdnSelected.Value) - Convert.ToDouble(txtGvPay.Text) - Convert.ToDouble(txtGvDisc.Text)));

                            //    }
                            //    string balns = lblBalance.Text.Replace("$", "");
                            //    balns = balns.Replace("(", "-");
                            //    balns = balns.Replace(")", "");
                            //    _totalBalance = _totalBalance + Convert.ToDouble(balns);
                            //}

                        }

                }
                Permission();
                UserPermission();
                HighlightSideMenu("acctPayable", "lnkWriteCheck2", "acctPayableSub");
                if (!IsPostBack)
                {
                    string path = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/");
                    DirectoryInfo d = new DirectoryInfo(path);
                    FileInfo[] Files = d.GetFiles("*.mrt");
                    foreach (FileInfo file in Files)
                    {

                        string FileName = string.Empty;
                        if (file.Name.Contains(".mrt"))
                            FileName = file.Name.Replace(".mrt", " ");
                        ddlApTopCheckForLoad.Items.Add((FileName));
                    }

                    ddlApTopCheckForLoad.DataBind();


                    string MidCheckpath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/");
                    DirectoryInfo dirMidPath = new DirectoryInfo(MidCheckpath);
                    FileInfo[] FilesMid = dirMidPath.GetFiles("*.mrt");
                    foreach (FileInfo fileMid in FilesMid)
                    {
                        string FileName = string.Empty;
                        if (fileMid.Name.Contains(".mrt"))
                            FileName = fileMid.Name.Replace(".mrt", " ");
                        ddlApMiddleCheckForLoad.Items.Add((FileName));
                    }

                    ddlApMiddleCheckForLoad.DataBind();


                    string TopCheckpath = Server.MapPath("StimulsoftReports/APChecks/TopChecks/");
                    DirectoryInfo dirTopcheckPath = new DirectoryInfo(TopCheckpath);
                    FileInfo[] FilesTop = dirTopcheckPath.GetFiles("*.mrt");
                    foreach (FileInfo fileTop in FilesTop)
                    {
                        string FileName = string.Empty;
                        if (fileTop.Name.Contains(".mrt"))
                            FileName = fileTop.Name.Replace(".mrt", " ");
                        ddlTopChecksForLoad.Items.Add((FileName));
                    }
                    ddlTopChecksForLoad.DataBind();

                    _objUser.ConnConfig = Session["config"].ToString();
                    _objUser.MOMUSer = Session["Username"].ToString();

                    _getCheckTemplate.ConnConfig = Session["config"].ToString();
                    _getCheckTemplate.MOMUSer = Session["Username"].ToString();
                    DataSet dsusercdtemp = new DataSet();

                    List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/AddCheck_GetCheckTemplate";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCheckTemplate);

                        _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                        dsusercdtemp = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
                    }
                    else
                    {
                        dsusercdtemp = _objBLBill.GetCheckTemplate(_objUser);
                    }

                    if (dsusercdtemp.Tables[0].Rows.Count > 0)
                    {
                        if (dsusercdtemp.Tables[0].Rows[0]["CD_Template"].ToString() != "")
                        {
                            if (ddlApTopCheckForLoad.Items.FindByText(dsusercdtemp.Tables[0].Rows[0]["CD_Template"].ToString()) != null)
                            {
                                ddlApTopCheckForLoad.Items.FindByText(dsusercdtemp.Tables[0].Rows[0]["CD_Template"].ToString()).Selected = true;
                            }
                            else if (ddlApMiddleCheckForLoad.Items.FindByText(dsusercdtemp.Tables[0].Rows[0]["CD_Template"].ToString()) != null)
                            {
                                ddlApMiddleCheckForLoad.Items.FindByText(dsusercdtemp.Tables[0].Rows[0]["CD_Template"].ToString()).Selected = true;
                            }
                            else if (ddlTopChecksForLoad.Items.FindByText(dsusercdtemp.Tables[0].Rows[0]["CD_Template"].ToString()) != null)
                            {
                                ddlTopChecksForLoad.Items.FindByText(dsusercdtemp.Tables[0].Rows[0]["CD_Template"].ToString()).Selected = true;
                            }
                        }
                    }
                }

                if (Request.QueryString["vid"] == null && Request.QueryString["ref"]== null)
                {
                    gvBills.Visible = true;
                    srchpane.Style["display"] = "none";
                    dpane.Style["display"] = "block";
                    dvbilldetailtop.Style["display"] = "none";
                    accrdGlAccount.Style["display"] = "block";
                    RadGrid_gvJobCostItems.Visible = true;
                    //ddlInvoice.Visible = false;
                }
                else
                {
                    gvBills.Visible = true;
                    srchpane.Style["display"] = "block";
                    dpane.Style["display"] = "none";
                    dvbilldetailtop.Style["display"] = "block";
                    accrdGlAccount.Style["display"] = "none";
                    RadGrid_gvJobCostItems.Visible = false;
                }
            }
            if (!IsPostBack)
            {
                GetInvDefaultAcct();
            }
            #region TrackingInventory

            ////TrackingInventory
            General _objPropGeneral = new General();
            BL_General _objBLGeneral = new BL_General();

            _objPropGeneral.ConnConfig = Session["config"].ToString();
            _getCustomField.ConnConfig = Session["config"].ToString();

            DataSet _dsCustom = new DataSet();
            List<CustomViewModel> _lstCustomViewModel = new List<CustomViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                _getCustomField.fieldName = "InvGL";

                string APINAME = "ManageChecksAPI/AddCheck_GetCustomField";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomField);

                _lstCustomViewModel = (new JavaScriptSerializer()).Deserialize<List<CustomViewModel>>(_APIResponse.ResponseData);
                _dsCustom = CommonMethods.ToDataSet<CustomViewModel>(_lstCustomViewModel);
            }
            else
            {
                _dsCustom = _objBLGeneral.getCustomField(_objPropGeneral, "InvGL");
            }

            Boolean TrackingInventory = false;
            if (_dsCustom.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
                {
                    if (!string.IsNullOrEmpty(_dr["Label"].ToString()) && _dr["Label"].ToString() != "0")
                    {
                        TrackingInventory = Convert.ToBoolean(_dr["Label"]);
                    }
                }
            }

            if (TrackingInventory == false)
            {


                //RadGrid_gvJobCostItems.Columns.FindByUniqueName("Warehouse").Visible = false;
                //RadGrid_gvJobCostItems.Columns.FindByUniqueName("WHLocID").Visible = false;

            }
            else
            {
                //RadGrid_gvJobCostItems.Columns.FindByUniqueName("Warehouse").Visible = true;
                //RadGrid_gvJobCostItems.Columns.FindByUniqueName("WHLocID").Visible = true;
                GetInvDefaultAcct();
            }
            #endregion


        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void GetInvDefaultAcct()
    {
        General _objPropGeneral = new General();
        BL_General _objBLGeneral = new BL_General();
        _objPropGeneral.ConnConfig = Session["config"].ToString();
        _getInvDefaultAcct.ConnConfig = Session["config"].ToString();

        DataSet _dsDefaultAccount = new DataSet();
        List<GeneralViewModel> _lstGeneralViewModel = new List<GeneralViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/AddCheck_GetInvDefaultAcct";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getInvDefaultAcct);

            _lstGeneralViewModel = (new JavaScriptSerializer()).Deserialize<List<GeneralViewModel>>(_APIResponse.ResponseData);
            _dsDefaultAccount = CommonMethods.ToDataSet<GeneralViewModel>(_lstGeneralViewModel);
        }
        else
        {
            _dsDefaultAccount = _objBLGeneral.getInvDefaultAcct(_objPropGeneral);
        }

        if (_dsDefaultAccount.Tables[0].Rows.Count > 0)
        {
            hdnInvDefaultAcctID.Value = Convert.ToString(_dsDefaultAccount.Tables[0].Rows[0]["ID"]);
            hdnInvDefaultAcctName.Value = Convert.ToString(_dsDefaultAccount.Tables[0].Rows[0]["Acct"]);
        }
    }
    #endregion
    private DataTable GetUserById()
    {
        User objPropUser = new User();
        objPropUser.TypeID = Convert.ToInt32(Session["usertypeid"]);
        objPropUser.UserID = Convert.ToInt32(Session["userid"]);
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.DBName = Session["dbname"].ToString();

        _getUserById.TypeID = Convert.ToInt32(Session["usertypeid"]);
        _getUserById.UserID = Convert.ToInt32(Session["userid"]);
        _getUserById.ConnConfig = Session["config"].ToString();
        _getUserById.DBName = Session["dbname"].ToString();

        DataSet ds = new DataSet();
        List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetUserById";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getUserById);

            _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
        }
        else
        {
            ds = objBL_User.GetUserPermissionByUserID(objPropUser);
        }

        return ds.Tables[0];
    }
    private void UserPermission()
    {
        // User Permission 
        if (Session["type"].ToString() != "am" && Session["type"].ToString() != "c")
        {
            DataTable ds = new DataTable();
            ds = GetUserById();

            /// AccountPayablemodulePermission ///////////////////------->

            string AccountPayablemodulePermission = ds.Rows[0]["AccountPayablemodulePermission"] == DBNull.Value ? "Y" : ds.Rows[0]["AccountPayablemodulePermission"].ToString();

            if (AccountPayablemodulePermission == "N")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }

            /// BillPay ///////////////////------->

            string BillPayPermission = ds.Rows[0]["BillPay"] == DBNull.Value ? "YYYYYY" : ds.Rows[0]["BillPay"].ToString();
            string ADD = BillPayPermission.Length < 1 ? "Y" : BillPayPermission.Substring(0, 1);
            if (ADD != "Y")
            {
                Response.Redirect("Home.aspx?permission=no"); return;
            }


        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("managechecks.aspx");
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidDate())
            {
                GetPeriodDetails(Convert.ToDateTime(txtDate.Text));     //Check period closed out permission
                bool Flag = (bool)ViewState["FlagPeriodClose"];

                if (Flag)
                {
                    if (ddlVendor.SelectedValue == "-1")
                    {
                        DataTable dt = (DataTable)Session["dsbills"];
                        DataTable dtNew = new DataTable();
                        dtNew.Columns.Add("Name");
                        dtNew.Columns.Add("Vendor");
                        foreach (DataRow drow in dt.Rows)
                        {
                            DataRow drNew = dtNew.NewRow();
                            drNew["Name"] = drow["Name"].ToString();
                            drNew["Vendor"] = drow["Vendor"].ToString();
                            dtNew.Rows.Add(drNew);
                        }
                        DataTable dtN = dtNew.DefaultView.ToTable(true);
                        int count = 0;
                        
                        foreach (DataRow dr in dtN.Rows)
                        {
                            _objCD.ConnConfig = Session["config"].ToString();
                            _objCD.Dt = GetVendorBillItems(dr["Name"].ToString());

                            _addCheck.ConnConfig = Session["config"].ToString();
                            _addCheck.Dt = GetVendorBillItems(dr["Name"].ToString());

                            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                if (_addCheck.Dt.Rows.Count > 0)
                                {
                                    _addCheck.fDate = DateTime.Now;
                                    //if (ddlPayment.SelectedValue == "0")
                                    //{
                                    //    _addCheck.NextC = Convert.ToInt32(txtNextCheck.Text) + count;
                                    //}
                                    //else
                                    //{
                                    //    _addCheck.NextC = Convert.ToInt32(txtNextCheck.Text);
                                    //}
                                    
                                        _addCheck.NextC = long.Parse(txtNextCheck.Text) + count;
                                    

                                    _addCheck.fDesc = dr["Name"].ToString();
                                    _addCheck.Bank = Convert.ToInt32(ddlBank.SelectedValue);
                                    _addCheck.Vendor = Convert.ToInt32(dr["Vendor"].ToString());
                                    _addCheck.Memo = txtMemo.Text;
                                    if (!string.IsNullOrEmpty(hdnDiscGL.Value))
                                    {
                                        _addCheck.DiscGL = Convert.ToInt32(hdnDiscGL.Value);
                                    }

                                    _addCheck.Type = Convert.ToInt16(ddlPayment.SelectedValue);
                                    _addCheck.MOMUSer = Session["Username"].ToString();
                                    
                                    string APINAME = "ManageChecksAPI/AddCheck";

                                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _addCheck);

                                    _objCD.ID = Convert.ToInt32(_APIResponse.ResponseData);
                                    
                                    count++;
                                }
                            }
                            else
                            {
                                if (_objCD.Dt.Rows.Count > 0)
                                {
                                    _objCD.fDate = DateTime.Now;
                                    //if (ddlPayment.SelectedValue == "0")
                                    //{
                                    //    _objCD.NextC = Convert.ToInt32(txtNextCheck.Text) + count;
                                    //}
                                    //else
                                    //{
                                    //    _objCD.NextC = Convert.ToInt32(txtNextCheck.Text);
                                    //}

                                    _objCD.NextC = long.Parse(txtNextCheck.Text) + count;
                                    

                                    _objCD.fDesc = dr["Name"].ToString();
                                    _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);
                                    _objCD.Vendor = Convert.ToInt32(dr["Vendor"].ToString());
                                    _objCD.Memo = txtMemo.Text;
                                    if (!string.IsNullOrEmpty(hdnDiscGL.Value))
                                    {
                                        _objCD.DiscGL = Convert.ToInt32(hdnDiscGL.Value);
                                    }

                                    _objCD.Type = Convert.ToInt16(ddlPayment.SelectedValue);
                                    _objCD.MOMUSer = Session["Username"].ToString();

                                    _objCD.ID = _objBLBill.AddCheck(_objCD);
                                   
                                    count++;
                                }
                            }

                        }
                        #region Reset Page
                        string check = txtNextCheck.Text;
                        ResetFormControlValues(this);
                        ResetForm();
                        btnSubmit.Visible = false;
                        btnCutCheck.Visible = true;
                        #endregion
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  noty({text: 'Checks saved successfully! </br> <b>', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                    }
                    else
                    {
                        //AddCheckDetails();
                        _objCD.ConnConfig = Session["config"].ToString();
                        _objCD.Dt = GetBillItems();
                        _objCD.fDate = DateTime.Now;
                        _objCD.NextC = long.Parse(txtNextCheck.Text);
                        _objCD.fDesc = ddlVendor.SelectedItem.Text;
                        _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);
                        _objCD.Vendor = Convert.ToInt32(ddlVendor.SelectedValue);
                        _objCD.Memo = txtMemo.Text;

                        _addCheck.ConnConfig = Session["config"].ToString();
                        _addCheck.Dt = GetBillItems();
                        _addCheck.fDate = DateTime.Now;
                        _addCheck.NextC = long.Parse(txtNextCheck.Text);
                        _addCheck.fDesc = ddlVendor.SelectedItem.Text;
                        _addCheck.Bank = Convert.ToInt32(ddlBank.SelectedValue);
                        _addCheck.Vendor = Convert.ToInt32(ddlVendor.SelectedValue);
                        _addCheck.Memo = txtMemo.Text;

                        if (!string.IsNullOrEmpty(hdnDiscGL.Value))
                        {
                            _objCD.DiscGL = Convert.ToInt32(hdnDiscGL.Value);
                            _addCheck.DiscGL = Convert.ToInt32(hdnDiscGL.Value);
                        }

                        _objCD.Type = Convert.ToInt16(ddlPayment.SelectedValue);
                        _objCD.MOMUSer = Session["Username"].ToString();

                        _addCheck.Type = Convert.ToInt16(ddlPayment.SelectedValue);
                        _addCheck.MOMUSer = Session["Username"].ToString();

                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "ManageChecksAPI/AddCheck";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _addCheck);

                            _objCD.ID = Convert.ToInt32(_APIResponse.ResponseData);
                        }
                        else
                        {
                            _objCD.ID = _objBLBill.AddCheck(_objCD);
                        }

                        #region Reset Page
                        string check = txtNextCheck.Text;
                        ResetFormControlValues(this);
                        ResetForm();
                        btnSubmit.Visible = false;
                        btnCutCheck.Visible = true;
                        #endregion
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  noty({text: 'Check saved successfully! </br> <b> Check# : " + check.ToString() + "</b>', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);

                    }


                    refreshVendorDDL();

                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void ddlBank_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!ddlBank.SelectedValue.Equals("0"))
        {
            SetBankDetails();
            btnSubmit.Visible = true;
            btnCutCheck.Visible = false;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "Materialize.updateTextFields();", true);
        }
        else
        {
            btnSubmit.Visible = false;
            btnCutCheck.Visible = true;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "Materialize.updateTextFields();", true);
        }

        ScriptManager.RegisterStartupScript(this, Page.GetType(), "display write check", "displayWriteCheck();", true);
        
    }
    protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlVendor.SelectedValue == "-1")
            {
                //vendorBalance.Visible = false;
                //lblVendorBal.Visible = false;
                vendorBalance.Visible = true;
                lblVendorBal.Visible = true;
                runningBalance.Visible = true;
                lblRunBalance.Visible = true;
                //SelectPay.Visible = false;
                //lblSelectedPayment.Visible = false;
                SelectPay.Visible = true;
                lblSelectedPayment.Visible = true;
                lblVendorCount.Visible = true;
                lblVCountValue.Visible = true;
                //DiscTaken.Visible = false;
                //lblTotalDiscount.Visible = false;
                DiscTaken.Visible = true;
                lblTotalDiscount.Visible = true;
                lblOI.Visible = true;
                lblOpenItems.Visible = true;
                lblBal.Visible = true;
                lblAutoSelectBalance.Visible = true;
                _objCD.ConnConfig = Session["config"].ToString();
                _getAutoSelectPayment.ConnConfig = Session["config"].ToString();

                DataSet _dsBills = new DataSet();
                DataSet _dsBills1 = new DataSet();
                DataSet _dsBills2 = new DataSet();
                ListGetAutoSelectPayment _lstAutoSelectPayment = new ListGetAutoSelectPayment();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/AddCheck_GetAutoSelectPayment";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getAutoSelectPayment);

                    _lstAutoSelectPayment = (new JavaScriptSerializer()).Deserialize<ListGetAutoSelectPayment>(_APIResponse.ResponseData);
                    _dsBills1 = _lstAutoSelectPayment.lstTable1.ToDataSet();
                    _dsBills2 = _lstAutoSelectPayment.lstTable2.ToDataSet();

                    DataTable dt1 = new DataTable();
                    DataTable dt2 = new DataTable(); 

                    dt1 = _dsBills1.Tables[0];
                    dt2 = _dsBills2.Tables[0];

                    dt1.TableName = "Table1";
                    dt2.TableName = "Table2";

                    _dsBills.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });
                }
                else
                {
                    _dsBills = _objBLBill.GetAutoSelectPayment(_objCD, Convert.ToString(Session["company"]));
                }

                //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                ////if (IsAPIIntegrationEnable == "YES")
                //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                //{
                //    gvBills.VirtualItemCount = _dsBills1.Tables[0].Rows.Count;
                //    gvBills.DataSource = _dsBills1.Tables[0];
                //    //gvBills.DataBind();
                //    gvBills.Rebind();
                //    Session["dsBills"] = _dsBills1.Tables[0];
                //    lblOpenItems.Text = _dsBills2.Tables[0].Rows[0]["NCount"].ToString();
                //    var sum = _dsBills1.Tables[0].AsEnumerable().Sum(x => x.Field<decimal>("Balance"));
                //    var count = _dsBills1.Tables[0].AsEnumerable().Select(r => r.Field<string>("Name")).Distinct().Count();
                //    lblAutoSelectBalance.Text = sum.ToString();
                //    lblVCountValue.Text = count.ToString();
                //}
                //else
                //{
                    gvBills.VirtualItemCount = _dsBills.Tables[0].Rows.Count;
                    gvBills.DataSource = _dsBills.Tables[0];
                    //gvBills.DataBind();
                    gvBills.Rebind();
                    Session["dsBills"] = _dsBills.Tables[0];
                    lblOpenItems.Text = _dsBills.Tables[1].Rows[0]["NCount"].ToString();
                    var sum = _dsBills.Tables[0].AsEnumerable().Sum(x => x.Field<decimal>("Balance"));
                    var count = _dsBills.Tables[0].AsEnumerable().Select(r => r.Field<string>("Name")).Distinct().Count();
                    lblAutoSelectBalance.Text = sum.ToString();
                    lblVCountValue.Text = count.ToString();
                //}
                
                CheckAllCheckbox();
                GetRunningBalance();

                double vanbalance = 0;
                lblVendorBal.Text = string.Format("{0:c}", vanbalance);
                lblSelectedPayment.Text = lblRunBalance.Text;

            }
            else
            {
                vendorBalance.Visible = true;
                lblVendorBal.Visible = true;
                runningBalance.Visible = true;
                lblRunBalance.Visible = true;
                SelectPay.Visible = true;
                lblSelectedPayment.Visible = true;
                lblVendorCount.Visible = false;
                lblVCountValue.Visible = false;
                DiscTaken.Visible = true;
                lblTotalDiscount.Visible = true;
                lblOI.Visible = false;
                lblOpenItems.Visible = false;
                lblBal.Visible = false;
                lblAutoSelectBalance.Visible = false;
                BindBills();
                CheckAllCheckbox();
                GetPaymentTotal();
                GetRunningBalance();
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void Page_PreRender(Object o, EventArgs e)
    {
        //foreach (GridViewRow gr in gvBills.Rows)
        foreach (GridDataItem gr in gvBills.Items)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            HiddenField hdnisSelected = (HiddenField)gr.FindControl("hdnisSelected");
            if ((hdnisSelected.Value) == "True")
            {
                chkSelect.Checked = true;
            }
            TextBox txtGvDisc = (TextBox)gr.FindControl("txtGvDisc");

            gr.Attributes["onclick"] = "VisibleRow('" + gr.ClientID + "','" + txtGvDisc.ClientID + "','" + gvBills.ClientID + "',event);";

        }
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "SelectedRowStyle('" + gvBills.ClientID + "');", true);
    }
    private string ChkBillStatus()
    {
        int chkbillstatus = 0;
        string chkbillstatusname = "";
        string chkbillref = "";
        string str = "";
        foreach (GridDataItem gr in gvBills.Items)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                Label lblRef = (Label)gr.FindControl("lblRef");
                HiddenField hdnbillspec = (HiddenField)gr.FindControl("hdnbillspec");
                HiddenField hdnbillspecstatus = (HiddenField)gr.FindControl("hdnbillspecstatus");
                if (Convert.ToInt16(hdnbillspec.Value) == 1 || Convert.ToInt16(hdnbillspec.Value) == 2 || Convert.ToInt16(hdnbillspec.Value) == 3)
                {
                    chkbillstatus = 1;
                    chkbillstatusname = hdnbillspecstatus.Value;
                    chkbillref = lblRef.Text;
                    break;
                }
            }
        }
        if (chkbillstatus == 1)
        {
            str = "This Bill#" + chkbillref + " is on status " + chkbillstatusname + " and cannot be paid., the check generation process cannot continue.";
            //criptManager.RegisterStartupScript(this, Page.GetType(), "keyProj1", "  noty({text: '" + str + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true , closeWith: ['click'], callback: { onShow: function() { }, afterShow: function() { }, onClose: function() { location.reload(); }, afterClose: function() { } }, buttons: false });", true);
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true}); $('#MOMloading').hide();", true);
            //return;
        }
        return str;               

    }
    protected void btnCutCheck_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["bill"] == null)
            {
                //if (gvBills.Rows.Count > 0)
                if (gvBills.Items.Count > 0)
                {
                    string strg = ChkBillStatus();
                    if (strg != "")
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: '" + strg + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true}); $('#MOMloading').hide();", true);
                        return;
                    }
                    else
                    {

                        SetBankDetails();
                        GetPaymentTotal();
                        bool chk = CheckNegativeBill();
                        if (chk == true)
                        {
                            ViewState["ProcessPaymentInitiated"] = true;
                            btnSubmit.Visible = true;
                            //btnPrintCheck.Visible = true;
                            //dvWriteCheck.Visible = true;
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "display write check", "displayWriteCheck();", true);
                        }
                    }
                }
            }
            else if (Request.QueryString["bill"] == "c")
            {
                string strg = ChkBillStatus();
                if (strg != "")
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: '" + strg + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true}); $('#MOMloading').hide();", true);
                    return;
                }
                else
                {
                    SetBankDetails();
                    GetPaymentTotal();
                    bool chk = CheckNegativeBill();
                    if (chk == true)
                    {
                        ViewState["ProcessPaymentInitiated"] = true;
                        btnSubmit.Visible = true;
                        //btnPrintCheck.Visible = true;
                        //dvWriteCheck.Visible = true;
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "display write check", "displayWriteCheck();", true);
                    }
                    else
                    {
                        //if (Request.QueryString["vid"] == null && Request.QueryString["ref"] == null)
                        //{
                        //    liaccrdGlAccount.Visible = true;
                        //    accrdGlAccount.Style["display"] = "block";
                        //    RadGrid_gvJobCostItems.Visible = true;
                        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "display write check158", "showbillgl();", true);
                            
                        //}
                    }
                    //if (RadGrid_gvJobCostItems.Items.Count > 0)
                    //{
                    //    SetBankDetails();
                    //    //GetPaymentTotal();
                    //    ViewState["ProcessPaymentInitiated"] = true;
                    //    //btnSubmit.Visible = true;
                    //    //btnPrintCheck.Visible = true;
                    //    //dvWriteCheck.Visible = true;
                    //    double _totalPay = 0.00;
                    //    double _totalPaysTax = 0.00;
                    //    double _totalPayGST = 0.00;
                    //    double _totalPayfull = 0.00;
                    //    string word = "";
                    //    foreach (GridDataItem gr in RadGrid_gvJobCostItems.Items)
                    //    {
                    //        TextBox txtGvAmount = (TextBox)gr.FindControl("txtGvAmount");
                    //        HiddenField hdnSTaxAm = (HiddenField)gr.FindControl("hdnSTaxAm");
                    //        HiddenField hdnGSTTaxAm = (HiddenField)gr.FindControl("hdnGSTTaxAm");
                    //        if (txtGvAmount.Text != null && txtGvAmount.Text != "")
                    //        {
                    //            _totalPay = _totalPay + Convert.ToDouble(txtGvAmount.Text);
                    //        }
                    //        if (hdnSTaxAm.Value != null && hdnSTaxAm.Value != "")
                    //        {
                    //            _totalPaysTax = _totalPaysTax + Convert.ToDouble(hdnSTaxAm.Value);
                    //        }
                    //        if (hdnGSTTaxAm.Value != null && hdnGSTTaxAm.Value != "")
                    //        {
                    //            _totalPayGST = _totalPayGST + Convert.ToDouble(hdnGSTTaxAm.Value);
                    //        }

                    //    }
                    //    lblTotalAmount.Text = string.Format("{0:c}", _totalPay+ _totalPaysTax+ _totalPayGST);
                    //    lblRequirement.Text = string.Format("{0:c}", _totalPay + _totalPaysTax + _totalPayGST);
                    //    if (!Convert.ToInt32(_totalPay + _totalPaysTax + _totalPayGST).Equals(0))
                    //        word = ConvertNumberToCurrency(Convert.ToDouble(_totalPay + _totalPaysTax + _totalPayGST));

                    //    lblDollar.Text = word;
                    //    ViewState["Dollar"] = word;

                    //    ViewState["Amount"] = (_totalPay + _totalPaysTax + _totalPayGST).ToString("0.00", CultureInfo.InvariantCulture);
                    //    _totalPayfull = _totalPay + _totalPaysTax + _totalPayGST;
                    //    if (gvBills.Items.Count > 0)
                    //    {
                    //        //SetBankDetails();
                    //        GetPaymentTotal();
                    //        ViewState["ProcessPaymentInitiated"] = true;
                    //        //btnSubmit.Visible = true;
                    //        //btnPrintCheck.Visible = true;
                    //        //dvWriteCheck.Visible = true;
                    //        double _billtotalPay = 0.00;
                    //        double _billtotalreq = 0.00;
                    //        if (lblTotalAmount.Text != null && lblTotalAmount.Text != "")
                    //        {
                    //            _billtotalPay = double.Parse(lblTotalAmount.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                    //                          NumberStyles.AllowThousands |
                    //                          NumberStyles.AllowDecimalPoint);
                    //        }
                    //        if (lblRequirement.Text != null && lblRequirement.Text != "")
                    //        {
                    //            _billtotalreq = double.Parse(lblRequirement.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                    //                          NumberStyles.AllowThousands |
                    //                          NumberStyles.AllowDecimalPoint);
                    //        }
                    //        lblTotalAmount.Text = string.Format("{0:c}", _totalPayfull + _billtotalPay);
                    //        lblRequirement.Text = string.Format("{0:c}", _totalPayfull + _billtotalreq);
                    //        if (!Convert.ToInt32(_totalPayfull + _billtotalPay).Equals(0))
                    //            word = ConvertNumberToCurrency(Convert.ToDouble(_totalPayfull) + Convert.ToDouble(_billtotalPay));

                    //        lblDollar.Text = word;
                    //        ViewState["Dollar"] = word;

                    //        ViewState["Amount"] = (_totalPayfull + _billtotalreq).ToString("0.00", CultureInfo.InvariantCulture);

                    //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "display write check", "displayWriteCheck();", true);
                    //    }
                    //    else
                    //    {
                    //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "display write check", "displayWriteCheck();", true);
                    //    }
                    //}

                }

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void btnApplyCredit_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidDate())
            {
                GetPeriodDetails(Convert.ToDateTime(txtDate.Text));     //Check period closed out permission
                bool Flag = (bool)ViewState["FlagPeriodClose"];
                Flag = true; //Credit Apply Should be Allow in Back Date.
                if (Flag)
                {
                    if (ddlVendor.SelectedValue == "-1")
                    {
                        DataTable dt = (DataTable)Session["dsbills"];
                        DataTable dtNew = new DataTable();
                        dtNew.Columns.Add("Name");
                        dtNew.Columns.Add("Vendor");
                        foreach (DataRow drow in dt.Rows)
                        {
                            DataRow drNew = dtNew.NewRow();
                            drNew["Name"] = drow["Name"].ToString();
                            drNew["Vendor"] = drow["Vendor"].ToString();
                            dtNew.Rows.Add(drNew);
                        }
                        DataTable dtN = dtNew.DefaultView.ToTable(true);
                        int count = 0;
                        foreach (DataRow dr in dtN.Rows)
                        {
                            _objCD.ConnConfig = Session["config"].ToString();
                            _objCD.Dt = GetVendorBillItems(dr["Name"].ToString());

                            _applyCredit.ConnConfig = Session["config"].ToString();
                            _applyCredit.Dt = GetVendorBillItems(dr["Name"].ToString());

                            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                if (_applyCredit.Dt.Rows.Count > 0)
                                {
                                    //_objCD.fDate = DateTime.Now;
                                    _applyCredit.fDate = Convert.ToDateTime(txtaplyDate.Text);
                                    //if (ddlPayment.SelectedValue == "0")
                                    //{
                                    //    _applyCredit.NextC = Convert.ToInt32(txtNextCheck.Text) + count;
                                    //}
                                    //else
                                    //{
                                    //    _applyCredit.NextC = Convert.ToInt32(txtNextCheck.Text);
                                    //}
                                    
                                        _applyCredit.NextC = long.Parse(txtNextCheck.Text) + count;
                                    

                                    _applyCredit.fDesc = dr["Name"].ToString();
                                    _applyCredit.Bank = Convert.ToInt32(ddlBank.SelectedValue);
                                    _applyCredit.Vendor = Convert.ToInt32(dr["Vendor"].ToString());
                                    _applyCredit.Memo = txtMemo.Text;
                                    if (!string.IsNullOrEmpty(hdnDiscGL.Value))
                                    {
                                        _applyCredit.DiscGL = Convert.ToInt32(hdnDiscGL.Value);
                                    }

                                    _applyCredit.Type = Convert.ToInt16(ddlPayment.SelectedValue);
                                    _applyCredit.MOMUSer = Session["Username"].ToString();

                                     string APINAME = "ManageChecksAPI/AddCheck_ApplyCredit";

                                     APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _applyCredit);

                                     _objCD.ID = Convert.ToInt32(_APIResponse.ResponseData);

                                    count++;
                                }
                            }
                            else
                            {
                                if (_objCD.Dt.Rows.Count > 0)
                                {
                                    //_objCD.fDate = DateTime.Now;
                                    _objCD.fDate = Convert.ToDateTime(txtaplyDate.Text);
                                    //if (ddlPayment.SelectedValue == "0")
                                    //{
                                    //    _objCD.NextC = Convert.ToInt32(txtNextCheck.Text) + count;
                                    //}
                                    //else
                                    //{
                                    //    _objCD.NextC = Convert.ToInt32(txtNextCheck.Text);
                                    //}
                                    
                                        _objCD.NextC = long.Parse(txtNextCheck.Text) + count;
                                    

                                    _objCD.fDesc = dr["Name"].ToString();
                                    _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);
                                    _objCD.Vendor = Convert.ToInt32(dr["Vendor"].ToString());
                                    _objCD.Memo = txtMemo.Text;
                                    if (!string.IsNullOrEmpty(hdnDiscGL.Value))
                                    {
                                        _objCD.DiscGL = Convert.ToInt32(hdnDiscGL.Value);
                                    }

                                    _objCD.Type = Convert.ToInt16(ddlPayment.SelectedValue);
                                    _objCD.MOMUSer = Session["Username"].ToString();
                                    
                                    _objCD.ID = _objBLBill.ApplyCredit(_objCD);
                                    
                                    count++;
                                }
                            }

                        }
                        #region Reset Page
                        string check = txtNextCheck.Text;
                        ResetFormControlValues(this);
                        ResetForm();
                        btnSubmit.Visible = false;
                        btnCutCheck.Visible = true;
                        #endregion
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  noty({text: 'Apply credit saved successfully! </br> <b>', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  alert('Checks Saved Successfully! </br> <b>');", true);
                        //Response.Write("<script>alert('Checks Saved Successfully!');</script>");
                        refreshVendorDDL();
                    }
                    else
                    {
                        
                        decimal _totalApplyCredit = 0;
                        foreach (DataRow row in GetBillItems().Rows)
                        {
                            _totalApplyCredit += Decimal.Parse(row["Paid"].ToString());
                        }

                        //if (Convert.ToDouble(GetBillItems().Compute("sum(Paid)", "").ToString()) == 0)
                        if (_totalApplyCredit == 0)
                        {
                            //AddCheckDetails();
                            _objCD.ConnConfig = Session["config"].ToString();
                            _objCD.Dt = GetBillItems();
                            // _objCD.fDate = DateTime.Now;
                            //_objCD.fDate = Convert.ToDateTime(txtDate.Text);
                            _objCD.fDate = Convert.ToDateTime(txtaplyDate.Text);
                            _objCD.NextC = Convert.ToInt32("0");
                            _objCD.fDesc = ddlVendor.SelectedItem.Text;
                            _objCD.Bank = Convert.ToInt32("0");
                            _objCD.Vendor = Convert.ToInt32(ddlVendor.SelectedValue);
                            _objCD.Memo = "Apply Credit";

                            _applyCredit.ConnConfig = Session["config"].ToString();
                            _applyCredit.Dt = GetBillItems();
                            _applyCredit.fDate = Convert.ToDateTime(txtaplyDate.Text);
                            _applyCredit.NextC = long.Parse("0");
                            _applyCredit.fDesc = ddlVendor.SelectedItem.Text;
                            _applyCredit.Bank = Convert.ToInt32("0");
                            _applyCredit.Vendor = Convert.ToInt32(ddlVendor.SelectedValue);
                            _applyCredit.Memo = "Apply Credit";

                            if (!string.IsNullOrEmpty(hdnDiscGL.Value))
                            {
                                _objCD.DiscGL = Convert.ToInt32(hdnDiscGL.Value);
                                _applyCredit.DiscGL = Convert.ToInt32(hdnDiscGL.Value);
                            }

                            _objCD.Type = Convert.ToInt16("0");
                            _objCD.MOMUSer = Session["Username"].ToString();

                            _applyCredit.Type = Convert.ToInt16("0");
                            _applyCredit.MOMUSer = Session["Username"].ToString();

                            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                string APINAME = "ManageChecksAPI/AddCheck_ApplyCredit";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _applyCredit);

                                _objCD.ID = Convert.ToInt32(_APIResponse.ResponseData);
                            }
                            else
                            {
                                _objCD.ID = _objBLBill.ApplyCredit(_objCD);
                            }

                            #region Reset Page
                            string check = txtNextCheck.Text;
                            ResetFormControlValues(this);
                            ResetForm();
                            btnSubmit.Visible = false;
                            btnCutCheck.Visible = true;
                            #endregion
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  noty({text: 'Apply credit saved successfully! </br> <b> </b>', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", " alert('Check Saved Successfully! </br> <b> Check# : " + check.ToString() + "</b>');", true);
                            //Response.Write("<script>alert('Checks Saved Successfully!');</script>");
                            refreshVendorDDL();
                            if (ddlVendor.Items.FindByValue(_objCD.Vendor.ToString()) != null)
                            {
                                ddlVendor.SelectedValue = _objCD.Vendor.ToString();
                                ddlVendor_SelectedIndexChanged(sender, e);
                            }
                            else
                            {
                                ddlVendor.SelectedValue = "0";
                                ddlVendor_SelectedIndexChanged(sender, e);
                            }
                            
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  noty({text: 'Apply credit amount mismatched! ',  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false}); displayapplycredit('1');", true);
                        }
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
    protected void ddlInvoice_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlInvoice.SelectedValue.Equals("2"))
        {
            txtSearchDate.Visible = true;
            FillVendor();
            ddlVendor.SelectedValue = "0";
            gvBills.DataSource = "";
            gvBills.Rebind();
        }
        else if (ddlInvoice.SelectedValue.Equals("3"))
        {
            txtSearchDate.Visible = false;
            FillCreditVendor();
            ddlVendor.SelectedValue = "0";
            gvBills.DataSource = "";
            gvBills.Rebind();
        }
        else
        {
            FillVendor();
            txtSearchDate.Visible = false;
            txtSearchDate.Text = "";
            BindBills();
        }
        //BindBills();
    }


    protected void imgPrintTemp1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            byte[] buffer1 = null;
            if (ddlApTopCheckForLoad.SelectedItem.Text.Trim() != null)
            {
                string reportApTopCheckPathStimul = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + ddlApTopCheckForLoad.SelectedItem.Text.Trim() + ".mrt");
                //  string reportPathStimul = Server.MapPath("StimulsoftReports/APTopCheck.mrt");

                StiReport report = new StiReport();
                if (Request.QueryString["bill"] == null)
                {
                    FillReportApTopCheckDataSet(ddlApTopCheckForLoad.SelectedItem.Text.Trim());
                    _objUser.ConnConfig = Session["config"].ToString();
                    _objUser.MOMUSer = Session["Username"].ToString();
                    _objUser.UserLic = ddlApTopCheckForLoad.SelectedItem.Text;

                    _updateCheckTemplate.ConnConfig = Session["config"].ToString();
                    _updateCheckTemplate.MOMUSer = Session["Username"].ToString();
                    _updateCheckTemplate.UserLic = ddlApTopCheckForLoad.SelectedItem.Text;


                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/AddCheck_updateCheckTemplate";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateCheckTemplate);
                    }
                    else
                    {
                        _objBLBill.updateCheckTemplate(_objUser);
                    }
                }
                else if (Request.QueryString["bill"] == "c")
                {
                    if (Request.QueryString["vid"] != null)
                    {
                        FillReportApTopCheckDataSet(ddlApTopCheckForLoad.SelectedItem.Text.Trim());
                    }
                    else
                    {
                        FillReportApTopCheckDataSet_New(ddlApTopCheckForLoad.SelectedItem.Text.Trim());
                    
                    }
                    _objUser.ConnConfig = Session["config"].ToString();
                    _objUser.MOMUSer = Session["Username"].ToString();
                    _objUser.UserLic = ddlApTopCheckForLoad.SelectedItem.Text;

                    _updateCheckTemplate.ConnConfig = Session["config"].ToString();
                    _updateCheckTemplate.MOMUSer = Session["Username"].ToString();
                    _updateCheckTemplate.UserLic = ddlApTopCheckForLoad.SelectedItem.Text;

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/AddCheck_updateCheckTemplate";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateCheckTemplate);
                    }
                    else
                    {
                        _objBLBill.updateCheckTemplate(_objUser);
                    }
                }

                    //report = FillDataSetToReport(ddlApTopCheckForLoad.SelectedItem.Text.Trim());
                    //var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                    //var service = new Stimulsoft.Report.Export.StiPdfExportService();
                    //System.IO.MemoryStream stream = new System.IO.MemoryStream();
                    //service.ExportTo(report, stream, settings);
                    //buffer1 = stream.ToArray();

                    //string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF", "APCheckTop.pdf");

                    //if (buffer1 != null)
                    //{
                    //    if (File.Exists(filename))
                    //        File.Delete(filename);
                    //    using (var fs = new FileStream(filename, FileMode.Create))
                    //    {
                    //        fs.Write(buffer1, 0, buffer1.Length);
                    //        fs.Close();
                    //    }
                    //}

                    ////END


                    ////rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsInvoices", _dti));
                    ////rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsCheck", _dtCheck));

                    ////rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsTicket", dsC.Tables[0]));
                    ////string reportPath = "Reports/ReportCheck.rdlc";

                    ////rvChecks.LocalReport.ReportPath = reportPath;

                    ////rvChecks.LocalReport.EnableExternalImages = true;
                    ////List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
                    ////string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
                    ////param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));

                    ////rvChecks.LocalReport.SetParameters(param1);

                    ////rvChecks.LocalReport.Refresh();

                    ////byte[] buffer = null;
                    ////buffer = ExportReportToPDF("", rvChecks);
                    //Response.ClearContent();
                    //Response.ClearHeaders();
                    //Response.AddHeader("Content-Disposition", "attachment;filename=PrintCheck.pdf");
                    //Response.ContentType = "application/pdf";
                    //Response.AddHeader("Content-Length", (buffer1.Length).ToString());
                    //Response.BinaryWrite(buffer1);
                    //Response.Flush();
                    //Response.Close();
                }
        }



        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private StiReport FillMaddenDataSetForReport_New(string reportName)
    {

        double SumAmountpay = 0.00;
        DataTable _dti = new DataTable();
        DataRow _dri = null;
        _dti.Columns.Add(new DataColumn("Ref", typeof(string)));
        _dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("Reference", typeof(string)));
        _dti.Columns.Add(new DataColumn("Total", typeof(double)));
        _dti.Columns.Add(new DataColumn("Disc", typeof(double)));
        _dti.Columns.Add(new DataColumn("AmountPay", typeof(double)));
        _dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("VendorName", typeof(string)));

        //RAHIL
        _dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("Description", typeof(string)));

        //New column
        _dti.Columns.Add(new DataColumn("State", typeof(string)));
        _dti.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dti.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dti.Columns.Add(new DataColumn("TotalAmount", typeof(string)));

        DataTable _dtCheck = new DataTable();
        DataRow _drC = null;
        _dtCheck.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrder", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Date", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("CheckAmount", typeof(double)));
        _dtCheck.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("State", typeof(string)));

        //NEW COLUMN
        _dtCheck.Columns.Add(new DataColumn("VendorAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("RemitAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Zip", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("TotalAmountPay", typeof(double)));
        _dtCheck.Columns.Add(new DataColumn("Memo", typeof(string)));

        _objCD.ConnConfig = Session["config"].ToString();
        _objCD.Ref = long.Parse(txtNextCheck.Text);
        _objCD.NextC = long.Parse(txtNextCheck.Text);
        _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);

        _getCheckDetailsByBankAndRef.ConnConfig = Session["config"].ToString();
        _getCheckDetailsByBankAndRef.Ref = long.Parse(txtNextCheck.Text);
        _getCheckDetailsByBankAndRef.NextC = long.Parse(txtNextCheck.Text);
        _getCheckDetailsByBankAndRef.Bank = Convert.ToInt32(ddlBank.SelectedValue);

        DataSet _dsCheck = new DataSet();
        DataSet _dsCheck1 = new DataSet();
        DataSet _dsCheck2 = new DataSet();
        ListCheckDetailsByBankAndRef _ListCheckDetailsByBankAndRef = new ListCheckDetailsByBankAndRef();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetCheckDetailsByBankAndRef";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCheckDetailsByBankAndRef);

            _ListCheckDetailsByBankAndRef = (new JavaScriptSerializer()).Deserialize<ListCheckDetailsByBankAndRef>(_APIResponse.ResponseData);

            _dsCheck1 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable1.ToDataSet();
            _dsCheck2 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable2.ToDataSet();

            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            dt1 = _dsCheck1.Tables[0];
            dt2 = _dsCheck2.Tables[0];

            dt1.TableName = "Table1";
            dt2.TableName = "Table2";

            _dsCheck.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });
        }
        else
        {
            _dsCheck = _objBLBill.GetCheckDetailsByBankAndRef(_objCD);
        }

         //int vid = Convert.ToInt32(_dsCheck.Tables[0].Rows[0]["Vendor"].ToString());
        DataTable dtNew = new DataTable();
        dtNew.Columns.Add("Name");
        dtNew.Columns.Add("Vendor");

        //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        ////if (IsAPIIntegrationEnable == "YES")
        //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        //{
        //    foreach (DataRow drow in _dsCheck1.Tables[0].Rows)
        //    {
        //        DataRow drNew = dtNew.NewRow();
        //        drNew["Name"] = drow["VendorName"].ToString();
        //        drNew["Vendor"] = drow["Vendor"].ToString();
        //        dtNew.Rows.Add(drNew);
        //    }
        //}
        //else
        //{
            foreach (DataRow drow in _dsCheck.Tables[0].Rows)
            {
                DataRow drNew = dtNew.NewRow();
                drNew["Name"] = drow["VendorName"].ToString();
                drNew["Vendor"] = drow["Vendor"].ToString();
                dtNew.Rows.Add(drNew);
            }
        //}

        DataTable dtN = dtNew.DefaultView.ToTable(true);

        //foreach (DataRow dr in dtN.Rows)
        //{
        int vid = Convert.ToInt32(dtN.Rows[0]["Vendor"].ToString());
        double AmountPay = 0.00;
        SumAmountpay = 0.00;



        //DataTable dtInvoice = _dsCheck.Tables[0].DefaultView.ToTable(true);
        DataView dtInv = new DataView();
        //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        ////if (IsAPIIntegrationEnable == "YES")
        //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        //{
        //    dtInv = _dsCheck1.Tables[0].DefaultView;
        //}
        //else
        //{
            dtInv = _dsCheck.Tables[0].DefaultView;
        //}

        dtInv.RowFilter = "Vendor = '" + vid + "'";
        foreach (DataRow drow in dtInv.ToTable(true).Rows)
        {
            _dri = _dti.NewRow();
            _dri["Ref"] = drow["Ref"].ToString();
            _dri["Description"] = drow["Description"].ToString();
            _dri["InvoiceDate"] = drow["InvoiceDate"].ToString();
            _dri["Reference"] = drow["Refrerence"].ToString();
            _dri["Total"] = double.Parse(drow["Total"].ToString().Replace('$', '0'), NumberStyles.AllowParentheses |
                          NumberStyles.AllowThousands |
                          NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
            _dri["Disc"] = Convert.ToDouble(drow["Disc"].ToString()).ToString();
            _dri["AmountPay"] = Convert.ToDouble(drow["AmountPay"].ToString()).ToString();
            SumAmountpay = SumAmountpay + Convert.ToDouble(drow["AmountPay"].ToString());
            _dri["PayDate"] = drow["PayDate"].ToString();
            _dri["CheckNo"] = drow["CheckNo"].ToString();


            //_dri["VendorID"] = Convert.ToInt32(ddlVendor.SelectedValue);
            _dri["VendorID"] = drow["Vendor"].ToString();
            //ac _dri["VendorName"] = ddlVendor.SelectedItem.Text;
            _dri["VendorName"] = drow["VendorName"].ToString();
            _dti.Rows.Add(_dri);

            _dti.AcceptChanges();

        }

        _objVendor.ConnConfig = Session["config"].ToString();
        _objVendor.ID = vid;

        _getVendorRolDetails.ConnConfig = Session["config"].ToString();
        _getVendorRolDetails.ID = vid;

        DataSet _dsV = new DataSet();
        List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetVendorRolDetails";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorRolDetails);

            _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
            _dsV = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
            _dsV.Tables[0].Columns.Remove("Type");
            _dsV.Tables[0].Columns["VType"].ColumnName = "Type";
            _dsV.Tables[0].Columns["Vendor1099"].ColumnName = "1099";
            _dsV.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
            //_dsV.Tables[0].Columns["Remit"].ColumnName = "RemitAddress";
        }
        else
        {
            _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
        }

        string vendAddress = "";
        string vendAddress2 = "";
        if (_dsV.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
            {
                vendAddress = _dsV.Tables[0].Rows[0]["Address"].ToString() + ", ";
            }

            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
            {
                vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
            }
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
            {
                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                {
                    vendAddress2 += ", ";
                }
                vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
            }
        }


        string chknos = null;
        long checkno = 0;
        DataView dtcheck = new DataView();
        //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        ////if (IsAPIIntegrationEnable == "YES")
        //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        //{
        //    dtcheck = _dsCheck2.Tables[0].DefaultView;
        //}
        //else
        //{
            dtcheck = _dsCheck.Tables[1].DefaultView;
        //}

        dtcheck.RowFilter = "Vendor = '" + vid + "'";
        foreach (DataRow drow in dtcheck.ToTable(true).Rows)
        //foreach (DataRow drow in _dsCheck.Tables[1].Rows)
        {
            _drC = _dtCheck.NewRow();
            if (Convert.ToDouble(drow["Pay"]) > 1000)
            {
                _drC["Pay"] = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                //_drC["Pay"] = ViewState["Dollar"].ToString();
            }
            else
            {
                string dollar = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                _drC["Pay"] = dollar + " Dollars";
            }
            _drC["ToOrder"] = drow["ToOrder"].ToString();
            //_drC["ToOrder"] = ViewState["Vendor"].ToString();
            _drC["Date"] = drow["Date"].ToString();
            _drC["CheckAmount"] = Convert.ToDouble(drow["Pay"]);
            _drC["ToOrderAddress"] = vendAddress;
            _drC["State"] = vendAddress2;

            _drC["TotalAmountpay"] = SumAmountpay;
            _drC["State"] = drow["State"].ToString();
            chknos = drow["CheckNo"].ToString();
            _dtCheck.Rows.Add(_drC);
        }

        DataSet dsCC = new DataSet();
        User objPropUser = new User();
        //ViewState["Checkno"] = lblNextCheck.Text;
        objPropUser.ConnConfig = Session["config"].ToString();
        _getConnectionConfig.ConnConfig = Session["config"].ToString();
        _getControlBranch.ConnConfig = Session["config"].ToString();

        if (Session["MSM"].ToString() != "TS")
        {
            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetControl";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                dsCC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
            }
            else
            {
                dsCC = objBL_User.getControl(objPropUser);
            }
        }
        else
        {
            objPropUser.LocID = Convert.ToInt32(0);
            _getControlBranch.LocID = Convert.ToInt32(0);

            List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetControlBranch";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getControlBranch);

                _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                dsCC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
            }
            else
            {
                dsCC = objBL_User.getControlBranch(objPropUser);
            }
        }
        //dsBank

        CreateTableBank();

        DataRow _drB = null;
        DataRow _drA = null;
        _objBank.ConnConfig = Session["config"].ToString();
        _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);

        _getBankCD.ConnConfig = Session["config"].ToString();
        _getBankCD.ID = Convert.ToInt32(ddlBank.SelectedValue);

        DataSet _dsB = new DataSet();

        List<BankViewModel> _lstBankViewModel = new List<BankViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/AddCheck_GetBankCD";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getBankCD);

            _lstBankViewModel = (new JavaScriptSerializer()).Deserialize<List<BankViewModel>>(_APIResponse.ResponseData);
            _dsB = CommonMethods.ToDataSet<BankViewModel>(_lstBankViewModel);
            _dsB.Tables[0].Columns["RolName"].ColumnName = "Name";
            _dsB.Tables[0].Columns["RolAddress"].ColumnName = "Address";
            _dsB.Tables[0].Columns["RolState"].ColumnName = "State";
            _dsB.Tables[0].Columns["RolCity"].ColumnName = "City";
            _dsB.Tables[0].Columns["RolZip"].ColumnName = "Zip";
        }
        else
        {
            _dsB = _objBLBill.GetBankCD(_objBank);
        }

        _drB = dtBank.NewRow();
        if (_dsB.Tables[0].Rows.Count > 0)
        {
            _drB["Name"] = _dsB.Tables[0].Rows[0]["Name"].ToString();
            _drB["Address"] = _dsB.Tables[0].Rows[0]["Address"].ToString();
            _drB["State"] = _dsB.Tables[0].Rows[0]["State"].ToString();
            _drB["City"] = _dsB.Tables[0].Rows[0]["City"].ToString();
            _drB["Zip"] = _dsB.Tables[0].Rows[0]["Zip"].ToString();
            _drB["NBranch"] = _dsB.Tables[0].Rows[0]["NBranch"].ToString();
            _drB["NAcct"] = _dsB.Tables[0].Rows[0]["NAcct"].ToString();
            _drB["NRoute"] = _dsB.Tables[0].Rows[0]["NRoute"].ToString();
            //_drB["Ref"] = _dsB.Tables[0].Rows[0]["Ref"].ToString();
            //_dtBank.Rows.Add(_drB);
        }

        string checkNumber = string.Empty;
        if (!string.IsNullOrEmpty(chknos))
        {
            checkNumber = chknos;
        }
        else
        {
            checkNumber = chknos.ToString();
        }

        if (checkNumber.Length == 1)
        {
            _drB["Ref"] = "00000000" + checkNumber;
        }
        else if (checkNumber.Length == 2)
        {
            _drB["Ref"] = "0000000" + checkNumber;
        }
        else if (checkNumber.Length == 3)
        {
            _drB["Ref"] = "000000" + checkNumber;
        }
        else if (checkNumber.Length == 4)
        {
            _drB["Ref"] = "00000" + checkNumber;
        }
        else if (checkNumber.Length == 5)
        {
            _drB["Ref"] = "0000" + checkNumber;
        }
        else if (checkNumber.Length == 6)
        {
            _drB["Ref"] = "000" + checkNumber;
        }
        else if (checkNumber.Length == 7)
        {
            _drB["Ref"] = "00" + checkNumber;
        }
        else if (checkNumber.Length == 8)
        {
            _drB["Ref"] = "0" + checkNumber;
        }
        else
        {
            _drB["Ref"] = "000000000";
        }

        dtBank.Rows.Add(_drB);

        _objVendor.ConnConfig = Session["config"].ToString();
        _objVendor.ID = vid;

        _getVendorAcct.ConnConfig = Session["config"].ToString();
        _getVendorAcct.ID = vid;

        DataSet _dsA = new DataSet();
        List<GetVendorAcctList> _lstGetVendorAcctList = new List<GetVendorAcctList>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/AddCheck_GetVendorAcct";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorAcct);

            _lstGetVendorAcctList = (new JavaScriptSerializer()).Deserialize<List<GetVendorAcctList>>(_APIResponse.ResponseData);
            _dsA = CommonMethods.ToDataSet<GetVendorAcctList>(_lstGetVendorAcctList);
            _dsA.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
        }
        else
        {
            _dsA = _objBLVendor.GetVendorAcct(_objVendor);
        }

        DataTable _dtAcct = new DataTable();
        _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));
        if (_dsA.Tables[0].Rows.Count > 0)
        {
            _drA = _dtAcct.NewRow();
            _drA["VendorID"] = _dsA.Tables[0].Rows[0]["ID"].ToString();
            _drA["VendorAcct"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
            _dtAcct.Rows.Add(_drA);
        }


        //dsBank end


        ReportViewer rvChecks = new ReportViewer();
        rvChecks.LocalReport.DataSources.Clear();
        //STIMULSOFT 
        byte[] buffer1 = null;
        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + reportName.Trim() + ".mrt");
        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        report.Compile();
        report["TotalAmountPay"] = SumAmountpay;

        //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        ////if (IsAPIIntegrationEnable == "YES")
        //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        //{
        //    report["Memo"] = Convert.ToString(_dsCheck2.Tables[0].Rows[0]["Memo"].ToString());
        //}
        //else
        //{
            report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
        //}

        report["InvoiceCount"] = dti.Rows.Count;
        DataSet Invoice = new DataSet();
        DataTable dtInvoice = _dti;
        _dti.TableName = "Invoice";
        Invoice.Tables.Add(_dti);
        Invoice.DataSetName = "Invoice";

        DataSet Check = new DataSet();
        DataTable dtCheck = _dtCheck;
        _dtCheck.TableName = "Check";
        Check.Tables.Add(_dtCheck);
        Check.DataSetName = "Check";

        DataSet ControlBranch = new DataSet();
        DataTable dtControlBranch = new DataTable();
        dtControlBranch = dsCC.Tables[0].Copy();
        ControlBranch.Tables.Add(dtControlBranch);
        dtControlBranch.TableName = "ControlBranch";
        ControlBranch.DataSetName = "ControlBranch";


        DataSet Bank = new DataSet();
        DataTable _dtBank = dtBank;
        dtBank.TableName = "Bank";
        Bank.Tables.Add(dtBank);
        Bank.DataSetName = "Bank";

        DataSet Account = new DataSet();
        DataTable dtAccount = _dtAcct;
        _dtAcct.TableName = "Account";
        Account.Tables.Add(_dtAcct);
        Account.DataSetName = "Account";


        report.RegData("dsInvoices", Invoice);
        report.RegData("dsCheck", Check);
        report.RegData("dsTicket", ControlBranch);
        report.RegData("dsBank", Bank);
        report.RegData("dsAccount", Account);
        report.Render();

        //StiWebDesigner1.Visible = true;
        //StiWebDesigner1.Report = report;

        //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "opencCeateForm();", true);
        return report;
    }
    private StiReport FillMaddenDataSetForReport(string reportName)
    {

        double SumAmountpay = 0.00;
        DataTable _dti = new DataTable();
        DataRow _dri = null;
        _dti.Columns.Add(new DataColumn("Ref", typeof(string)));
        _dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("Reference", typeof(string)));
        _dti.Columns.Add(new DataColumn("Total", typeof(double)));
        _dti.Columns.Add(new DataColumn("Disc", typeof(double)));
        _dti.Columns.Add(new DataColumn("AmountPay", typeof(double)));
        _dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("VendorName", typeof(string)));

        //RAHIL
        _dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("Description", typeof(string)));

        //New column
        _dti.Columns.Add(new DataColumn("State", typeof(string)));
        _dti.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dti.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dti.Columns.Add(new DataColumn("TotalAmount", typeof(string)));

        DataTable _dtCheck = new DataTable();
        DataRow _drC = null;
        _dtCheck.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrder", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Date", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("CheckAmount", typeof(double)));
        _dtCheck.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("State", typeof(string)));

        //NEW COLUMN
        _dtCheck.Columns.Add(new DataColumn("VendorAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("RemitAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Zip", typeof(string)));
        int vid = Convert.ToInt32(ddlVendor.SelectedValue);
        //foreach (GridViewRow gr in gvBills.Rows)
        foreach (GridDataItem gr in gvBills.Items)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                TextBox txtGvPay = (TextBox)gr.FindControl("txtGvPay");
                Label lblBalance = (Label)gr.FindControl("lblBalance");
                TextBox txtGvDisc = (TextBox)gr.FindControl("txtGvDisc");
                Label lblOrig = (Label)gr.FindControl("lblOrig");
                HiddenField hdnRef = (HiddenField)gr.FindControl("hdnRef");

                Label lblBillfdesc = (Label)gr.FindControl("lblBillfdesc");
                TextBox txtGvDesc = (TextBox)gr.FindControl("txtGvDesc");
                Label lblfDate = (Label)gr.FindControl("lblfDate");
                Label lblRef = (Label)gr.FindControl("lblRef");

                _dri = _dti.NewRow();
                _dri["Ref"] = hdnRef.Value;

                //RAHIL
                //_objOpenAP.Ref = hdnRef.Value;
                //DataSet _dsCheck = _objBLBill.GetCheckDetails(_objOpenAP);

                //if (_dsCheck.Tables[0].Rows.Count > 0)
                //{
                //    _dri["Description"] = _dsCheck.Tables[0].Rows[0]["fDesc"].ToString();
                //}

                //DataRow[] dr = gds.Tables[0].Select("Ref='" + hdnRef.Value + "'");                   

                _dri["InvoiceDate"] = lblfDate.Text;
                _dri["Reference"] = lblRef.Text;
                //_dri["Description"] = lblBillfdesc.Text;
                _dri["Description"] = txtGvDesc.Text;
                _dri["Total"] = double.Parse(lblOrig.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                              NumberStyles.AllowThousands |
                              NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                _dri["Disc"] = Convert.ToDouble(txtGvDisc.Text);
                _dri["AmountPay"] = Convert.ToDouble(txtGvPay.Text);
                SumAmountpay = SumAmountpay + Convert.ToDouble(txtGvPay.Text);
                _dri["PayDate"] = txtDate.Text;
                //_dri["CheckNo"] = ViewState["Checkno"].ToString();
                if (!string.IsNullOrEmpty(txtNextCheck.Text))
                {
                    _dri["CheckNo"] = txtNextCheck.Text;
                }
                else
                {
                    _dri["CheckNo"] = ViewState["Checkno"].ToString();
                }
                //_dri["VendorID"] = gr["Vendor"];
                _dri["VendorName"] = lblVendor.Text;
                _dti.Rows.Add(_dri);
            }
        }
        _objVendor.ConnConfig = Session["config"].ToString();
        _objVendor.ID = vid;

        _getVendorRolDetails.ConnConfig = Session["config"].ToString();
        _getVendorRolDetails.ID = vid;

        DataSet _dsV = new DataSet();
        List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetVendorRolDetails";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorRolDetails);

            _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
            _dsV = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
            _dsV.Tables[0].Columns.Remove("Type");
            _dsV.Tables[0].Columns["VType"].ColumnName = "Type";
            _dsV.Tables[0].Columns["Vendor1099"].ColumnName = "1099";
            _dsV.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
            //_dsV.Tables[0].Columns["Remit"].ColumnName = "RemitAddress";
        }
        else
        {
            _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
        }

        string vendAddress = "";
        string vendAddress2 = "";
        if (_dsV.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
            {
                vendAddress = _dsV.Tables[0].Rows[0]["Address"].ToString() + ", ";
            }

            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
            {
                vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
            }
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
            {
                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                {
                    vendAddress2 += ", ";
                }
                vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
            }
        }
        _drC = _dtCheck.NewRow();
        //_drC["Pay"] = ViewState["Dollar"].ToString();
        if (Convert.ToDouble(SumAmountpay) > 1000)
        {
            //_drC["Pay"] = ViewState["Dollar"].ToString();
            _drC["Pay"] = ConvertNumberToCurrency(SumAmountpay);
        }
        else
        {
            string dollar = ConvertNumberToCurrency(SumAmountpay);
            _drC["Pay"] = dollar + " Dollars";
        }
        _drC["ToOrder"] = ViewState["Vendor"].ToString();
        _drC["Date"] = txtDate.Text;
        //_drC["CheckAmount"] = Convert.ToDouble(ViewState["Amount"]).ToString("0.00", CultureInfo.InvariantCulture);
        _drC["CheckAmount"] = Convert.ToDouble(SumAmountpay).ToString("0.00", CultureInfo.InvariantCulture);
        _drC["ToOrderAddress"] = vendAddress;
        if (string.IsNullOrEmpty(_drC["State"].ToString()))
            _drC["State"] = vendAddress2.Replace(",,", ",");
        _dtCheck.Rows.Add(_drC);

        DataSet dsC = new DataSet();

        //ViewState["Checkno"] = lblNextCheck.Text;
        objPropUser.ConnConfig = Session["config"].ToString();
        _getConnectionConfig.ConnConfig = Session["config"].ToString();
        _getControlBranch.ConnConfig = Session["config"].ToString();

        if (Session["MSM"].ToString() != "TS")
        {
            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetControl";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                dsC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
            }
            else
            {
                dsC = objBL_User.getControl(objPropUser);
            }
        }
        else
        {
            objPropUser.LocID = Convert.ToInt32(0);
            _getControlBranch.LocID = Convert.ToInt32(0);

            List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetControlBranch";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getControlBranch);

                _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                dsC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
            }
            else
            {
                dsC = objBL_User.getControlBranch(objPropUser);
            }
        }
        //dsBank

        CreateTableBank();

        DataRow _drB = null;
        DataRow _drA = null;
        _objBank.ConnConfig = Session["config"].ToString();
        _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);

        _getBankCD.ConnConfig = Session["config"].ToString();
        _getBankCD.ID = Convert.ToInt32(ddlBank.SelectedValue);
        DataSet _dsB = new DataSet();

        List<BankViewModel> _lstBankViewModel = new List<BankViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/AddCheck_GetBankCD";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getBankCD);

            _lstBankViewModel = (new JavaScriptSerializer()).Deserialize<List<BankViewModel>>(_APIResponse.ResponseData);
            _dsB = CommonMethods.ToDataSet<BankViewModel>(_lstBankViewModel);
            _dsB.Tables[0].Columns["RolName"].ColumnName = "Name";
            _dsB.Tables[0].Columns["RolAddress"].ColumnName = "Address";
            _dsB.Tables[0].Columns["RolState"].ColumnName = "State";
            _dsB.Tables[0].Columns["RolCity"].ColumnName = "City";
            _dsB.Tables[0].Columns["RolZip"].ColumnName = "Zip";
        }
        else
        {
            _dsB = _objBLBill.GetBankCD(_objBank);
        }

        _drB = dtBank.NewRow();
        if (_dsB.Tables[0].Rows.Count > 0)
        {
            _drB["Name"] = _dsB.Tables[0].Rows[0]["Name"].ToString();
            _drB["Address"] = _dsB.Tables[0].Rows[0]["Address"].ToString();
            _drB["State"] = _dsB.Tables[0].Rows[0]["State"].ToString();
            _drB["City"] = _dsB.Tables[0].Rows[0]["City"].ToString();
            _drB["Zip"] = _dsB.Tables[0].Rows[0]["Zip"].ToString();
            _drB["NBranch"] = _dsB.Tables[0].Rows[0]["NBranch"].ToString();
            _drB["NAcct"] = _dsB.Tables[0].Rows[0]["NAcct"].ToString();
            _drB["NRoute"] = _dsB.Tables[0].Rows[0]["NRoute"].ToString();
            //_drB["Ref"] = _dsB.Tables[0].Rows[0]["Ref"].ToString();
            //_dtBank.Rows.Add(_drB);
        }

        string checkNumber = string.Empty;
        if (!string.IsNullOrEmpty(txtNextCheck.Text))
        {
            checkNumber = txtNextCheck.Text;
        }
        else
        {
            checkNumber = ViewState["Checkno"].ToString();
        }

        if (checkNumber.Length == 1)
        {
            _drB["Ref"] = "00000000" + checkNumber;
        }
        else if (checkNumber.Length == 2)
        {
            _drB["Ref"] = "0000000" + checkNumber;
        }
        else if (checkNumber.Length == 3)
        {
            _drB["Ref"] = "000000" + checkNumber;
        }
        else if (checkNumber.Length == 4)
        {
            _drB["Ref"] = "00000" + checkNumber;
        }
        else if (checkNumber.Length == 5)
        {
            _drB["Ref"] = "0000" + checkNumber;
        }
        else if (checkNumber.Length == 6)
        {
            _drB["Ref"] = "000" + checkNumber;
        }
        else if (checkNumber.Length == 7)
        {
            _drB["Ref"] = "00" + checkNumber;
        }
        else if (checkNumber.Length == 8)
        {
            _drB["Ref"] = "0" + checkNumber;
        }
        else
        {
            _drB["Ref"] = "000000000";
        }

        dtBank.Rows.Add(_drB);

        _objVendor.ConnConfig = Session["config"].ToString();
        _objVendor.ID = vid;

        _getVendorAcct.ConnConfig = Session["config"].ToString();
        _getVendorAcct.ID = vid;

        DataSet _dsA = new DataSet();
        List<GetVendorAcctList> _lstGetVendorAcct = new List<GetVendorAcctList>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/AddCheck_GetVendorAcct";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorAcct);

            _lstGetVendorAcct = (new JavaScriptSerializer()).Deserialize<List<GetVendorAcctList>>(_APIResponse.ResponseData);
            _dsA = CommonMethods.ToDataSet<GetVendorAcctList>(_lstGetVendorAcct);
            _dsA.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
        }
        else
        {
            _dsA = _objBLVendor.GetVendorAcct(_objVendor);
        }

        DataTable _dtAcct = new DataTable();
        _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));
        if (_dsA.Tables[0].Rows.Count > 0)
        {
            _drA = _dtAcct.NewRow();
            _drA["VendorID"] = _dsA.Tables[0].Rows[0]["ID"].ToString();
            _drA["VendorAcct"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
            _dtAcct.Rows.Add(_drA);
        }


        //dsBank end


        ReportViewer rvChecks = new ReportViewer();
        rvChecks.LocalReport.DataSources.Clear();
        //STIMULSOFT 
        byte[] buffer1 = null;
        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + reportName.Trim() + ".mrt");
        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        report.Compile();
        report["TotalAmountPay"] = SumAmountpay;
        report["Memo"] = txtMemo.Text;

        report["InvoiceCount"] = dti.Rows.Count;
        DataSet Invoice = new DataSet();
        DataTable dtInvoice = _dti;
        _dti.TableName = "Invoice";
        Invoice.Tables.Add(_dti);
        Invoice.DataSetName = "Invoice";

        DataSet Check = new DataSet();
        DataTable dtCheck = _dtCheck;
        _dtCheck.TableName = "Check";
        Check.Tables.Add(_dtCheck);
        Check.DataSetName = "Check";

        DataSet ControlBranch = new DataSet();
        DataTable dtControlBranch = new DataTable();
        dtControlBranch = dsC.Tables[0].Copy();
        ControlBranch.Tables.Add(dtControlBranch);
        dtControlBranch.TableName = "ControlBranch";
        ControlBranch.DataSetName = "ControlBranch";


        DataSet Bank = new DataSet();
        DataTable _dtBank = dtBank;
        dtBank.TableName = "Bank";
        Bank.Tables.Add(dtBank);
        Bank.DataSetName = "Bank";

        DataSet Account = new DataSet();
        DataTable dtAccount = _dtAcct;
        _dtAcct.TableName = "Account";
        Account.Tables.Add(_dtAcct);
        Account.DataSetName = "Account";


        report.RegData("dsInvoices", Invoice);
        report.RegData("dsCheck", Check);
        report.RegData("dsTicket", ControlBranch);
        report.RegData("dsBank", Bank);
        report.RegData("dsAccount", Account);
        report.Render();

        //StiWebDesigner1.Visible = true;
        //StiWebDesigner1.Report = report;

        //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "opencCeateForm();", true);
        return report;
    }
    private StiReport FillMiddleDataSetReport_New(string reportName)
    {

        double SumAmountpay = 0.00;
        DataTable _dti = new DataTable();
        DataRow _dri = null;
        _dti.Columns.Add(new DataColumn("Ref", typeof(string)));
        _dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("Reference", typeof(string)));
        _dti.Columns.Add(new DataColumn("Total", typeof(double)));
        _dti.Columns.Add(new DataColumn("Disc", typeof(double)));
        _dti.Columns.Add(new DataColumn("AmountPay", typeof(double)));
        _dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("VendorName", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

        //RAHIL
        _dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("Description", typeof(string)));

        //New column
        _dti.Columns.Add(new DataColumn("State", typeof(string)));
        _dti.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dti.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dti.Columns.Add(new DataColumn("TotalAmount", typeof(string)));

        DataTable _dtCheck = new DataTable();
        DataRow _drC = null;
        _dtCheck.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrder", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Date", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("State", typeof(string)));

        //NEW COLUMN
        _dtCheck.Columns.Add(new DataColumn("VendorAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("RemitAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Zip", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("TotalAmountPay", typeof(double)));
        _dtCheck.Columns.Add(new DataColumn("Memo", typeof(string)));


        _objCD.ConnConfig = Session["config"].ToString();
        _objCD.Ref = long.Parse(txtNextCheck.Text);
        _objCD.NextC = long.Parse(txtNextCheck.Text);
        _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);

        _getCheckDetailsByBankAndRef.ConnConfig = Session["config"].ToString();
        _getCheckDetailsByBankAndRef.Ref = long.Parse(txtNextCheck.Text);
        _getCheckDetailsByBankAndRef.NextC = long.Parse(txtNextCheck.Text);
        _getCheckDetailsByBankAndRef.Bank = Convert.ToInt32(ddlBank.SelectedValue);

        DataSet _dsCheck = new DataSet();
        DataSet _dsCheck1 = new DataSet();
        DataSet _dsCheck2 = new DataSet();
        ListCheckDetailsByBankAndRef _ListCheckDetailsByBankAndRef = new ListCheckDetailsByBankAndRef();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetCheckDetailsByBankAndRef";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCheckDetailsByBankAndRef);

            _ListCheckDetailsByBankAndRef = (new JavaScriptSerializer()).Deserialize<ListCheckDetailsByBankAndRef>(_APIResponse.ResponseData);

            _dsCheck1 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable1.ToDataSet();
            _dsCheck2 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable2.ToDataSet();

            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            dt1 = _dsCheck1.Tables[0];
            dt2 = _dsCheck2.Tables[0];

            dt1.TableName = "Table1";
            dt2.TableName = "Table2";

            _dsCheck.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });

        }
        else
        {
            _dsCheck = _objBLBill.GetCheckDetailsByBankAndRef(_objCD);
        }

        //int vid = Convert.ToInt32(_dsCheck.Tables[0].Rows[0]["Vendor"].ToString());
        DataTable dtNew = new DataTable();
        dtNew.Columns.Add("Name");
        dtNew.Columns.Add("Vendor");

        //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        ////if (IsAPIIntegrationEnable == "YES")
        //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        //{
        //    foreach (DataRow drow in _dsCheck1.Tables[0].Rows)
        //    {
        //        DataRow drNew = dtNew.NewRow();
        //        drNew["Name"] = drow["VendorName"].ToString();
        //        drNew["Vendor"] = drow["Vendor"].ToString();
        //        dtNew.Rows.Add(drNew);
        //    }
        //}
        //else
        //{
            foreach (DataRow drow in _dsCheck.Tables[0].Rows)
            {
                DataRow drNew = dtNew.NewRow();
                drNew["Name"] = drow["VendorName"].ToString();
                drNew["Vendor"] = drow["Vendor"].ToString();
                dtNew.Rows.Add(drNew);
            }
        //}
        DataTable dtN = dtNew.DefaultView.ToTable(true);
        DataTable _dtAcct = new DataTable();
        //foreach (DataRow dr in dtN.Rows)
        //{
        int vid = Convert.ToInt32(dtN.Rows[0]["Vendor"].ToString());
        double AmountPay = 0.00;
        SumAmountpay = 0.00;



        //DataTable dtInvoice = _dsCheck.Tables[0].DefaultView.ToTable(true);
        DataView dtInv = new DataView();
        ////_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //////if (IsAPIIntegrationEnable == "YES")
        ////if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        ////{
        ////    dtInv = _dsCheck1.Tables[0].DefaultView;
        ////}
        ////else
        ////{
            dtInv = _dsCheck.Tables[0].DefaultView;
        //}

        dtInv.RowFilter = "Vendor = '" + vid + "'";
        foreach (DataRow drow in dtInv.ToTable(true).Rows)
        {
            _dri = _dti.NewRow();
            _dri["Ref"] = drow["Ref"].ToString();
            _dri["Description"] = drow["Description"].ToString();
            _dri["InvoiceDate"] = drow["InvoiceDate"].ToString();
            _dri["Reference"] = drow["Refrerence"].ToString();
            _dri["Total"] = double.Parse(drow["Total"].ToString().Replace('$', '0'), NumberStyles.AllowParentheses |
                          NumberStyles.AllowThousands |
                          NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
            _dri["Disc"] = Convert.ToDouble(drow["Disc"].ToString()).ToString();
            _dri["AmountPay"] = Convert.ToDouble(drow["AmountPay"].ToString()).ToString();
            SumAmountpay = SumAmountpay + Convert.ToDouble(drow["AmountPay"].ToString());
            _dri["PayDate"] = drow["PayDate"].ToString();
            _dri["CheckNo"] = drow["CheckNo"].ToString();


            //_dri["VendorID"] = Convert.ToInt32(ddlVendor.SelectedValue);
            _dri["VendorID"] = drow["Vendor"].ToString();
            //ac _dri["VendorName"] = ddlVendor.SelectedItem.Text;
            _dri["VendorName"] = drow["VendorName"].ToString();
            _dti.Rows.Add(_dri);

            _dti.AcceptChanges();

        }










        _objVendor.ConnConfig = Session["config"].ToString();
        _objVendor.ID = vid;

        _getVendorRolDetails.ConnConfig = Session["config"].ToString();
        _getVendorRolDetails.ID = vid;

        DataSet _dsV = new DataSet();
        List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetVendorRolDetails";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorRolDetails);

            _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
            _dsV = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
            _dsV.Tables[0].Columns.Remove("Type");
            _dsV.Tables[0].Columns["VType"].ColumnName = "Type";
            _dsV.Tables[0].Columns["Vendor1099"].ColumnName = "1099";
            _dsV.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
            //_dsV.Tables[0].Columns["Remit"].ColumnName = "RemitAddress";
        }
        else
        {
            _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
        }

        string vendAddress = "";
        string vendAddress2 = "";
        if (_dsV.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
            {
                vendAddress = _dsV.Tables[0].Rows[0]["Address"].ToString() + ", ";
            }

            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
            {
                vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
            }
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
            {
                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                {
                    vendAddress2 += ", ";
                }
                vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
            }
        }


        DataView dtcheck = new DataView();
        //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        ////if (IsAPIIntegrationEnable == "YES")
        //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        //{
        //    dtcheck = _dsCheck2.Tables[0].DefaultView;
        //}
        //else
        //{
            dtcheck = _dsCheck.Tables[1].DefaultView;
        //}

        dtcheck.RowFilter = "Vendor = '" + vid + "'";
        foreach (DataRow drow in dtcheck.ToTable(true).Rows)
        //foreach (DataRow drow in _dsCheck.Tables[1].Rows)
        {
            _drC = _dtCheck.NewRow();
            if (Convert.ToDouble(drow["Pay"]) > 1000)
            {
                _drC["Pay"] = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                //_drC["Pay"] = ViewState["Dollar"].ToString();
            }
            else
            {
                string dollar = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                _drC["Pay"] = dollar + " Dollars";
            }
            _drC["ToOrder"] = drow["ToOrder"].ToString();
            //_drC["ToOrder"] = ViewState["Vendor"].ToString();
            _drC["Date"] = drow["Date"].ToString();
            _drC["CheckAmount"] = Convert.ToDouble(drow["Pay"]);
            _drC["ToOrderAddress"] = vendAddress;
            _drC["State"] = vendAddress2;

            _drC["TotalAmountpay"] = SumAmountpay;
            _drC["State"] = drow["State"].ToString();
            _dtCheck.Rows.Add(_drC);
        }






        DataSet dsCC = new DataSet();
        User objPropUser = new User();
        //ViewState["Checkno"] = lblNextCheck.Text;
        objPropUser.ConnConfig = Session["config"].ToString();
        _getConnectionConfig.ConnConfig = Session["config"].ToString();
        _getControlBranch.ConnConfig = Session["config"].ToString();
        if (Session["MSM"].ToString() != "TS")
        {
            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetControl";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                dsCC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
            }
            else
            {
                dsCC = objBL_User.getControl(objPropUser);
            }
        }
        else
        {
            objPropUser.LocID = Convert.ToInt32(0);
            _getControlBranch.LocID = Convert.ToInt32(0);
            List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetControlBranch";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getControlBranch);

                _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                dsCC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
            }
            else
            {
                dsCC = objBL_User.getControlBranch(objPropUser);
            }
        }

        ReportViewer rvChecks = new ReportViewer();
        rvChecks.LocalReport.DataSources.Clear();
        //STIMULSOFT 
        byte[] buffer1 = null;
        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + reportName.Trim() + ".mrt");

        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        report.Compile();
        report["TotalAmountPay"] = SumAmountpay;

        //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        ////if (IsAPIIntegrationEnable == "YES")
        //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        //{
        //    report["Memo"] = Convert.ToString(_dsCheck2.Tables[0].Rows[0]["Memo"].ToString());
        //}
        //else
        //{
            report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
        //}

        report["InvoiceCount"] = dti.Rows.Count;
        DataSet Invoice = new DataSet();
        DataTable dtInvoice = _dti;
        _dti.TableName = "Invoice";
        Invoice.Tables.Add(_dti);
        Invoice.DataSetName = "Invoice";

        DataSet Check = new DataSet();
        DataTable dtCheck = _dtCheck;
        _dtCheck.TableName = "Check";
        Check.Tables.Add(_dtCheck);
        Check.DataSetName = "Check";

        DataSet ControlBranch = new DataSet();
        DataTable dtControlBranch = new DataTable();
        dtControlBranch = dsCC.Tables[0].Copy();
        ControlBranch.Tables.Add(dtControlBranch);
        dtControlBranch.TableName = "ControlBranch";
        ControlBranch.DataSetName = "ControlBranch";

        report.RegData("dsInvoices", Invoice);
        report.RegData("dsCheck", Check);
        report.RegData("dsTicket", ControlBranch);
        report.Render();

        //StiWebDesigner1.Visible = true;
        //StiWebDesigner1.Report = report;

        //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "opencCeateForm();", true);
        return report;





    }
    private StiReport FillMiddleDataSetReport(string reportName)
    {

        double SumAmountpay = 0.00;
        DataTable _dti = new DataTable();
        DataRow _dri = null;
        _dti.Columns.Add(new DataColumn("Ref", typeof(string)));
        _dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("Reference", typeof(string)));
        _dti.Columns.Add(new DataColumn("Total", typeof(double)));
        _dti.Columns.Add(new DataColumn("Disc", typeof(double)));
        _dti.Columns.Add(new DataColumn("AmountPay", typeof(double)));
        _dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("VendorName", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

        //RAHIL
        _dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("Description", typeof(string)));

        //New column
        _dti.Columns.Add(new DataColumn("State", typeof(string)));
        _dti.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dti.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dti.Columns.Add(new DataColumn("TotalAmount", typeof(string)));

        DataTable _dtCheck = new DataTable();
        DataRow _drC = null;
        _dtCheck.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrder", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Date", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("State", typeof(string)));

        //NEW COLUMN
        _dtCheck.Columns.Add(new DataColumn("VendorAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("RemitAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Zip", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("TotalAmountPay", typeof(double)));
        _dtCheck.Columns.Add(new DataColumn("Memo", typeof(string)));

        
        int vid = Convert.ToInt32(ddlVendor.SelectedValue);
        
        //foreach (GridViewRow gr in gvBills.Rows)
        foreach (GridDataItem gr in gvBills.Items)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                TextBox txtGvPay = (TextBox)gr.FindControl("txtGvPay");
                Label lblBalance = (Label)gr.FindControl("lblBalance");
                TextBox txtGvDisc = (TextBox)gr.FindControl("txtGvDisc");
                Label lblOrig = (Label)gr.FindControl("lblOrig");
                HiddenField hdnRef = (HiddenField)gr.FindControl("hdnRef");

                Label lblBillfdesc = (Label)gr.FindControl("lblBillfdesc");
                TextBox txtGvDesc = (TextBox)gr.FindControl("txtGvDesc");
                Label lblfDate = (Label)gr.FindControl("lblfDate");
                Label lblRef = (Label)gr.FindControl("lblRef");

                _dri = _dti.NewRow();
                _dri["Ref"] = hdnRef.Value;
                //_dri["Description"] = lblBillfdesc.Text;
                _dri["Description"] = txtGvDesc.Text;
                //RAHIL
                //_objOpenAP.Ref = hdnRef.Value;
                //DataSet _dsCheck = _objBLBill.GetCheckDetails(_objOpenAP);

                //if (_dsCheck.Tables[0].Rows.Count > 0)
                //{
                //    _dri["Description"] = _dsCheck.Tables[0].Rows[0]["fDesc"].ToString();
                //}

                //DataRow[] dr = gds.Tables[0].Select("Ref='" + hdnRef.Value + "'");                   

                _dri["InvoiceDate"] = lblfDate.Text;
                _dri["Reference"] = lblRef.Text;
                _dri["Total"] = double.Parse(lblOrig.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                              NumberStyles.AllowThousands |
                              NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                _dri["Disc"] = Convert.ToDouble(txtGvDisc.Text);
                _dri["AmountPay"] = Convert.ToDouble(txtGvPay.Text);
                SumAmountpay = SumAmountpay + Convert.ToDouble(txtGvPay.Text);
                _dri["PayDate"] = txtDate.Text;
                //_dri["CheckNo"] = ViewState["Checkno"].ToString();
                if (!string.IsNullOrEmpty(txtNextCheck.Text))
                {
                    _dri["CheckNo"] = txtNextCheck.Text;
                }
                else
                {
                    _dri["CheckNo"] = ViewState["Checkno"].ToString();
                }
                _dri["VendorID"] = Convert.ToInt32(ddlVendor.SelectedValue);
                _dri["VendorName"] = ddlVendor.SelectedItem.Text;
                _objVendor.ID = Convert.ToInt32(ddlVendor.SelectedValue);
                _objVendor.ConnConfig = Session["config"].ToString();

                _getVendorAcct.ConnConfig = Session["config"].ToString();
                _getVendorAcct.ID = Convert.ToInt32(ddlVendor.SelectedValue);

                List<GetVendorAcctList> _lstGetVendorAcct = new List<GetVendorAcctList>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/AddCheck_GetVendorAcct";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorAcct);

                    _lstGetVendorAcct = (new JavaScriptSerializer()).Deserialize<List<GetVendorAcctList>>(_APIResponse.ResponseData);
                    DataSet ds = CommonMethods.ToDataSet<GetVendorAcctList>(_lstGetVendorAcct);
                    ds.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                    _dri["VendorAcct"] = ds;
                    //_dsA.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                }
                else
                {
                    _dri["VendorAcct"] = _objBLVendor.GetVendorAcct(_objVendor);
                }

                _dti.Rows.Add(_dri);
            }
        }
        _objVendor.ConnConfig = Session["config"].ToString();
        _objVendor.ID = vid;

        _getVendorRolDetails.ConnConfig = Session["config"].ToString();
        _getVendorRolDetails.ID = vid;

        DataSet _dsV = new DataSet();
        List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetVendorRolDetails";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorRolDetails);

            _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
            _dsV = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
            _dsV.Tables[0].Columns.Remove("Type");
            _dsV.Tables[0].Columns["VType"].ColumnName = "Type";
            _dsV.Tables[0].Columns["Vendor1099"].ColumnName = "1099";
            _dsV.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
           // _dsV.Tables[0].Columns["Remit"].ColumnName = "RemitAddress";
        }
        else
        {
            _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
        }

        string vendAddress = "";
        string vendAddress2 = "";
        if (_dsV.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
            {
                vendAddress = _dsV.Tables[0].Rows[0]["Address"].ToString() + ", ";
            }

            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
            {
                vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
            }
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
            {
                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                {
                    vendAddress2 += ", ";
                }
                vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
            }
        }
        _drC = _dtCheck.NewRow();
        if (Convert.ToDouble(SumAmountpay) > 1000)
        {
            _drC["Pay"] = ConvertNumberToCurrency(Convert.ToDouble(SumAmountpay));
            //_drC["Pay"] = ViewState["Dollar"].ToString();
        }
        else
        {
            string dollar = ConvertNumberToCurrency(Convert.ToDouble(SumAmountpay));
            _drC["Pay"] = dollar + " Dollars";
        }
        //_drC["Pay"] = ViewState["Dollar"].ToString();
        _drC["ToOrder"] = ViewState["Vendor"].ToString();
        _drC["Date"] = txtDate.Text;
        _drC["CheckAmount"] = Convert.ToDouble(SumAmountpay).ToString("0.00", CultureInfo.InvariantCulture);
        //_drC["CheckAmount"] = Convert.ToDouble(ViewState["Amount"]).ToString("0.00", CultureInfo.InvariantCulture);
        _drC["ToOrderAddress"] = vendAddress;
        if (string.IsNullOrEmpty(_drC["State"].ToString()))
            _drC["State"] = vendAddress2.Replace(",,", ",");
        _drC["TotalAmountPay"] = SumAmountpay;
        _drC["Memo"] = txtMemo.Text;
        _dtCheck.Rows.Add(_drC);

        DataSet dsC = new DataSet();

        //ViewState["Checkno"] = lblNextCheck.Text;
        objPropUser.ConnConfig = Session["config"].ToString();
        _getConnectionConfig.ConnConfig = Session["config"].ToString();
        _getControlBranch.ConnConfig = Session["config"].ToString();

        if (Session["MSM"].ToString() != "TS")
        {
            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetControl";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                dsC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
            }
            else
            {
                dsC = objBL_User.getControl(objPropUser);
            }
        }
        else
        {
            objPropUser.LocID = Convert.ToInt32(0);
            _getControlBranch.LocID = Convert.ToInt32(0);
            List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetControlBranch";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getControlBranch);

                _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                dsC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
            }
            else
            {
                dsC = objBL_User.getControlBranch(objPropUser);
            }
        }
        ReportViewer rvChecks = new ReportViewer();
        rvChecks.LocalReport.DataSources.Clear();
        //STIMULSOFT 
        byte[] buffer1 = null;
        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + reportName.Trim() + ".mrt");

        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        report.Compile();
        report["TotalAmountPay"] = SumAmountpay;
        report["Memo"] = txtMemo.Text;

        report["InvoiceCount"] = dti.Rows.Count;
        DataSet Invoice = new DataSet();
        DataTable dtInvoice = _dti;
        _dti.TableName = "Invoice";
        Invoice.Tables.Add(_dti);
        Invoice.DataSetName = "Invoice";

        DataSet Check = new DataSet();
        DataTable dtCheck = _dtCheck;
        _dtCheck.TableName = "Check";
        Check.Tables.Add(_dtCheck);
        Check.DataSetName = "Check";

        DataSet ControlBranch = new DataSet();
        DataTable dtControlBranch = new DataTable();
        dtControlBranch = dsC.Tables[0].Copy();
        ControlBranch.Tables.Add(dtControlBranch);
        dtControlBranch.TableName = "ControlBranch";
        ControlBranch.DataSetName = "ControlBranch";

        report.RegData("dsInvoices", Invoice);
        report.RegData("dsCheck", Check);
        report.RegData("dsTicket", ControlBranch);
        report.Render();

        //StiWebDesigner1.Visible = true;
        //StiWebDesigner1.Report = report;

        //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "opencCeateForm();", true);
        return report;





    }
    private void FillReportMaddenDataSet_New(string reportName)
    {
        try
        {
            double SumAmountpay = 0.00;
            int count = 0;
            _objCD.ConnConfig = Session["config"].ToString();
            _objCD.Ref = long.Parse(txtNextCheck.Text);
            _objCD.NextC = long.Parse(txtNextCheck.Text);
            _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);

            _getCheckDetailsByBankAndRef.ConnConfig = Session["config"].ToString();
            _getCheckDetailsByBankAndRef.Ref = long.Parse(txtNextCheck.Text);
            _getCheckDetailsByBankAndRef.NextC = long.Parse(txtNextCheck.Text);
            _getCheckDetailsByBankAndRef.Bank = Convert.ToInt32(ddlBank.SelectedValue);

            DataSet _dsCheck = new DataSet();
            DataSet _dsCheck1 = new DataSet();
            DataSet _dsCheck2 = new DataSet();
            ListCheckDetailsByBankAndRef _ListCheckDetailsByBankAndRef = new ListCheckDetailsByBankAndRef();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetCheckDetailsByBankAndRef";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCheckDetailsByBankAndRef);

                _ListCheckDetailsByBankAndRef = (new JavaScriptSerializer()).Deserialize<ListCheckDetailsByBankAndRef>(_APIResponse.ResponseData);

                _dsCheck1 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable1.ToDataSet();
                _dsCheck2 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable2.ToDataSet();

                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                dt1 = _dsCheck1.Tables[0];
                dt2 = _dsCheck2.Tables[0];

                dt1.TableName = "Table1";
                dt2.TableName = "Table2";

                _dsCheck.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });
            }
            else
            {
                _dsCheck = _objBLBill.GetCheckDetailsByBankAndRef(_objCD);
            }
            DataSet ControlBranch = new DataSet();
            DataTable dtControlBranch = new DataTable();
            dtControlBranch = _dsCheck.Tables[2].Copy();
            ControlBranch.Tables.Add(dtControlBranch);
            dtControlBranch.TableName = "ControlBranch";
            ControlBranch.DataSetName = "ControlBranch";

            DataSet Account = new DataSet();
            DataTable dtAccount = new DataTable();
            dtAccount = _dsCheck.Tables[3].Copy();
            Account.Tables.Add(dtAccount);
            dtAccount.TableName = "Account";
            Account.DataSetName = "Account";

            DataSet Bank = new DataSet();
            DataTable dtBank = new DataTable();
            dtBank = _dsCheck.Tables[4].Copy();
            Bank.Tables.Add(dtBank);
            dtBank.TableName = "Bank";
            Bank.DataSetName = "Bank";

            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("Name");
            dtNew.Columns.Add("Vendor");
            dtNew.Columns.Add("CheckNo");

            //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            ////if (IsAPIIntegrationEnable == "YES")
            //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            //{
            //    foreach (DataRow drow in _dsCheck1.Tables[0].Rows)
            //    {
            //        DataRow drNew = dtNew.NewRow();
            //        drNew["Name"] = drow["VendorName"].ToString();
            //        drNew["Vendor"] = drow["Vendor"].ToString();
            //        drNew["CheckNo"] = drow["CheckNo"].ToString();
            //        dtNew.Rows.Add(drNew);
            //    }
            //}
            //else
            //{
                foreach (DataRow drow in _dsCheck.Tables[0].Rows)
                {
                    DataRow drNew = dtNew.NewRow();
                    drNew["Name"] = drow["VendorName"].ToString();
                    drNew["Vendor"] = drow["Vendor"].ToString();
                    drNew["CheckNo"] = drow["CheckNo"].ToString();
                    dtNew.Rows.Add(drNew);
                }
            //}


            DataTable dtN = dtNew.DefaultView.ToTable(true);
            DataTable _dtAcct = new DataTable();
            foreach (DataRow dr in dtN.Rows)
            {
                bool isChecked = true;
                _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
                _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

                CreateTableInvoice();
                CreateTablePayee();
                CreateTableBank();


                DataRow _dri = null;
                DataRow _drC = null;

                int vid = Convert.ToInt32(dr["Vendor"].ToString());
                string checkNo = Convert.ToString(dr["CheckNo"].ToString());
                //ac     int vid = Convert.ToInt32(ddlVendor.SelectedValue);
                //RAHIL'S IMPLEMENTATION
                DataRow _drB = null;
                DataRow _drA = null;
                double AmountPay = 0.00;
                SumAmountpay = 0.00;



                //DataTable dtInvoice = _dsCheck.Tables[0].DefaultView.ToTable(true);
                DataView dtInv = new DataView();
                //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                ////if (IsAPIIntegrationEnable == "YES")
                //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                //{
                //    dtInv = _dsCheck1.Tables[0].DefaultView;
                //}
                //else
                //{
                    dtInv = _dsCheck.Tables[0].DefaultView;
                //}

                dtInv.RowFilter = "Vendor = '" + vid + "' and checkNo = '" + checkNo + "'";
                foreach (DataRow drow in dtInv.ToTable(true).Rows)
                {
                    _dri = dti.NewRow();
                    _dri["Ref"] = drow["Ref"].ToString();
                    _dri["Description"] = drow["Description"].ToString();
                    _dri["InvoiceDate"] = drow["InvoiceDate"].ToString();
                    _dri["Reference"] = drow["Refrerence"].ToString();
                    _dri["Total"] = double.Parse(drow["Total"].ToString().Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                    _dri["Disc"] = Convert.ToDouble(drow["Disc"].ToString()).ToString();
                    _dri["AmountPay"] = Convert.ToDouble(drow["AmountPay"].ToString()).ToString();
                    AmountPay = AmountPay + Convert.ToDouble(drow["AmountPay"].ToString());
                    _dri["PayDate"] = drow["PayDate"].ToString();
                    _dri["CheckNo"] = drow["CheckNo"].ToString();


                    //_dri["VendorID"] = Convert.ToInt32(ddlVendor.SelectedValue);
                    _dri["VendorID"] = drow["Vendor"].ToString();
                    //ac _dri["VendorName"] = ddlVendor.SelectedItem.Text;
                    _dri["VendorName"] = drow["VendorName"].ToString();
                    dti.Rows.Add(_dri);

                    dti.AcceptChanges();

                }
                if (isChecked)
                {
                    if (dti.Rows.Count > 0)
                    {
                        string chknos = null;

                        DataView dtcheck = new DataView();

                        //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                        ////if (IsAPIIntegrationEnable == "YES")
                        //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        //{
                        //    dtcheck = _dsCheck2.Tables[0].DefaultView;
                        //}
                        //else
                        //{
                            dtcheck = _dsCheck.Tables[1].DefaultView;
                        //}

                        dtcheck.RowFilter = "Vendor = '" + vid + "' and checkNo = '" + checkNo + "'";
                        ViewState["CheckStatus"] = "0";
                        foreach (DataRow drow in dtcheck.ToTable(true).Rows)
                        //foreach (DataRow drow in _dsCheck.Tables[1].Rows)
                        {
                            _drC = dtpay.NewRow();
                            if (Convert.ToDouble(drow["Pay"]) > 1000)
                            {
                                _drC["Pay"] = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                                //_drC["Pay"] = ViewState["Dollar"].ToString();
                            }
                            else
                            {
                                string dollar = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                                _drC["Pay"] = dollar + " Dollars";
                            }
                            _drC["ToOrder"] = drow["ToOrder"].ToString();
                            //_drC["ToOrder"] = ViewState["Vendor"].ToString();
                            _drC["Date"] = drow["Date"].ToString();
                            _drC["CheckAmount"] = Convert.ToDouble(drow["Pay"]);
                            _drC["VendorAddress"] = drow["VendorAddress"].ToString();                 // change by Mayuri on 8th nov,16
                            _drC["RemitAddress"] = drow["RemitAddress"].ToString();                  // change by Mayuri on 8th nov,16
                            _drC["State"] = drow["State"].ToString();
                            ViewState["CheckStatus"] = drow["Status"].ToString();
                            chknos = drow["CheckNo"].ToString();
                            dtpay.Rows.Add(_drC);
                        }

                        _objBank.ConnConfig = Session["config"].ToString();
                        _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);

                        _getBankCD.ConnConfig = Session["config"].ToString();
                        _getBankCD.ID = Convert.ToInt32(ddlBank.SelectedValue);
                        DataSet _dsB = new DataSet();

                        List<BankViewModel> _lstBankViewModel = new List<BankViewModel>();

                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "ManageChecksAPI/AddCheck_GetBankCD";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getBankCD);

                            _lstBankViewModel = (new JavaScriptSerializer()).Deserialize<List<BankViewModel>>(_APIResponse.ResponseData);
                            _dsB = CommonMethods.ToDataSet<BankViewModel>(_lstBankViewModel);
                            _dsB.Tables[0].Columns["RolName"].ColumnName = "Name";
                            _dsB.Tables[0].Columns["RolAddress"].ColumnName = "Address";
                            _dsB.Tables[0].Columns["RolState"].ColumnName = "State";
                            _dsB.Tables[0].Columns["RolCity"].ColumnName = "City";
                            _dsB.Tables[0].Columns["RolZip"].ColumnName = "Zip";
                        }
                        else
                        {
                            _dsB = _objBLBill.GetBankCD(_objBank);
                        }

                        _drB = dtBank.NewRow();
                        if (_dsB.Tables[0].Rows.Count > 0)
                        {
                            _drB["Name"] = _dsB.Tables[0].Rows[0]["Name"].ToString();
                            _drB["Address"] = _dsB.Tables[0].Rows[0]["Address"].ToString();
                            _drB["State"] = _dsB.Tables[0].Rows[0]["State"].ToString();
                            _drB["City"] = _dsB.Tables[0].Rows[0]["City"].ToString();
                            _drB["Zip"] = _dsB.Tables[0].Rows[0]["Zip"].ToString();
                            _drB["NBranch"] = _dsB.Tables[0].Rows[0]["NBranch"].ToString();
                            _drB["NAcct"] = _dsB.Tables[0].Rows[0]["NAcct"].ToString();
                            _drB["NRoute"] = _dsB.Tables[0].Rows[0]["NRoute"].ToString();
                            //_drB["Ref"] = _dsB.Tables[0].Rows[0]["Ref"].ToString();
                            //_dtBank.Rows.Add(_drB);
                        }
                        string checkNumber = string.Empty;
                        if (!string.IsNullOrEmpty(chknos))
                        {
                            checkNumber = chknos.ToString();
                        }
                        else
                        {
                            checkNumber = chknos.ToString();
                        }

                        if (checkNumber.Length == 1)
                        {
                            _drB["Ref"] = "00000000" + checkNumber;
                        }
                        else if (checkNumber.Length == 2)
                        {
                            _drB["Ref"] = "0000000" + checkNumber;
                        }
                        else if (checkNumber.Length == 3)
                        {
                            _drB["Ref"] = "000000" + checkNumber;
                        }
                        else if (checkNumber.Length == 4)
                        {
                            _drB["Ref"] = "00000" + checkNumber;
                        }
                        else if (checkNumber.Length == 5)
                        {
                            _drB["Ref"] = "0000" + checkNumber;
                        }
                        else if (checkNumber.Length == 6)
                        {
                            _drB["Ref"] = "000" + checkNumber;
                        }
                        else if (checkNumber.Length == 7)
                        {
                            _drB["Ref"] = "00" + checkNumber;
                        }
                        else if (checkNumber.Length == 8)
                        {
                            _drB["Ref"] = "0" + checkNumber;
                        }
                        else
                        {
                            _drB["Ref"] = "000000000";
                        }

                        dtBank.Rows.Add(_drB);

                        _objVendor.ConnConfig = Session["config"].ToString();
                        _objVendor.ID = vid;

                        _getVendorAcct.ConnConfig = Session["config"].ToString();
                        _getVendorAcct.ID = vid;

                        DataSet _dsA = new DataSet();
                        List<GetVendorAcctList> _lstGetVendorAcctList = new List<GetVendorAcctList>();

                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "ManageChecksAPI/AddCheck_GetVendorAcct";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorAcct);

                            _lstGetVendorAcctList = (new JavaScriptSerializer()).Deserialize<List<GetVendorAcctList>>(_APIResponse.ResponseData);
                            _dsA = CommonMethods.ToDataSet<GetVendorAcctList>(_lstGetVendorAcctList);
                            _dsA.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                        }
                        else
                        {
                            _dsA = _objBLVendor.GetVendorAcct(_objVendor);
                        }

                        _drA = _dtAcct.NewRow();
                        _drA["VendorID"] = _dsA.Tables[0].Rows[0]["ID"].ToString();
                        _drA["VendorAcct"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
                        _dtAcct.Rows.Add(_drA);
                        //-----------------------------------------------


                        var rowCount = 0;
                        var totalRows = dti.Rows.Count;
                        if (reportName.Contains("-"))
                        {
                            try
                            {
                                string[] reportNameArr = reportName.Split('-');
                                rowCount = Convert.ToInt32(reportNameArr[1].ToString().Trim().TrimStart());
                                if (totalRows < rowCount)
                                    rowCount = totalRows;
                            }
                            catch (Exception ex) { rowCount = totalRows; }
                        }
                        else
                            rowCount = 6;
                        var dtiCopy = dti.Copy();
                        DataView dv = dtiCopy.DefaultView;
                        dv.Sort = "Ref asc";
                        DataTable sortedDT = dv.ToTable();
                        var dtCopy = sortedDT.Copy();
                        var firstHalf = dtCopy;
                        var secondHalf = dtCopy;
                        if (dtCopy.Rows.Count > rowCount)
                        {
                            firstHalf = dtCopy.AsEnumerable().Take(rowCount).CopyToDataTable();
                            secondHalf = dtCopy.Clone();
                            if (totalRows > rowCount)
                            {
                                secondHalf = dtCopy.AsEnumerable().Skip(rowCount).Take(totalRows - rowCount).CopyToDataTable();
                            }
                        }
                        else
                        {
                            firstHalf = dtCopy;
                        }

                        DataSet dsCC = new DataSet();
                        User objPropUser = new User();
                        objPropUser.ConnConfig = Session["config"].ToString();
                        _getConnectionConfig.ConnConfig = Session["config"].ToString();
                        _getControlBranch.ConnConfig = Session["config"].ToString();

                        if (Session["MSM"].ToString() != "TS")
                        {
                            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

                            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                string APINAME = "ManageChecksAPI/CheckList_GetControl";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                                _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                                dsCC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
                            }
                            else
                            {
                                dsCC = objBL_User.getControl(objPropUser);
                            }
                        }
                        else
                        {
                            objPropUser.LocID = Convert.ToInt32(0);
                            _getControlBranch.LocID = Convert.ToInt32(0);
                            List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

                            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                string APINAME = "ManageChecksAPI/CheckList_GetControlBranch";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getControlBranch);

                                _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                                dsCC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
                            }
                            else
                            {
                                dsCC = objBL_User.getControlBranch(objPropUser);
                            }
                        }

                        //STIMULSOFT 
                        byte[] buffer1 = null;
                        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + reportName.Trim() + ".mrt");
                        StiReport report = new StiReport();
                        report.Load(reportPathStimul);
                        if (Convert.ToInt16(ViewState["CheckStatus"]).Equals(2))
                        {
                            report.Pages[0].Watermark.Enabled = true;
                            //report.Pages[0].Watermark.Angle = 0;
                            //report.Pages[0].Watermark.Text = "Void";
                            string imagepath = Server.MapPath("images/icons/voidcheck.png");
                            report.Pages[0].Watermark.Image = System.Drawing.Image.FromFile(imagepath);
                            report.Pages[0].Watermark.ImageAlignment = System.Drawing.ContentAlignment.TopCenter;
                            report.Pages[0].Watermark.ShowImageBehind = true;
                        }
                        report.Compile();
                        report["TotalAmountPay"] = AmountPay;
                        //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                        ////if (IsAPIIntegrationEnable == "YES")
                        //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        //{
                        //    report["Memo"] = Convert.ToString(_dsCheck2.Tables[0].Rows[0]["Memo"].ToString());
                        //}
                        //else
                        //{
                            report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
                        //}

                        report["InvoiceCount"] = totalRows;
                        if (dsCC.Tables[0].Rows.Count > 0)
                        {
                            report["CompanyName"] = dsCC.Tables[0].Rows[0]["Name"].ToString();
                            report["CompanyAddress"] = dsCC.Tables[0].Rows[0]["Address"].ToString();
                            report["CompanyCity"] = dsCC.Tables[0].Rows[0]["City"].ToString() + "' " + dsCC.Tables[0].Rows[0]["State"].ToString() + " - " + dsCC.Tables[0].Rows[0]["Zip"].ToString();
                        }
                        DataSet Invoice = new DataSet();
                        DataTable dtInvoice = firstHalf.Copy();
                        dtInvoice.TableName = "Invoice";
                        Invoice.Tables.Add(dtInvoice);
                        Invoice.DataSetName = "Invoice";

                        DataSet Check = new DataSet();
                        DataTable dtCheck = dtpay.Copy();
                        dtCheck.TableName = "Check";
                        Check.Tables.Add(dtCheck);
                        Check.DataSetName = "Check";

                        //DataSet ControlBranch = new DataSet();
                        //DataTable dtControlBranch = new DataTable();
                        //dtControlBranch = dsC.Tables[0].Copy();
                        //ControlBranch.Tables.Add(dtControlBranch);
                        //dtControlBranch.TableName = "ControlBranch";
                        //ControlBranch.DataSetName = "ControlBranch";

                        //DataSet ControlBranch = new DataSet();
                        //DataTable dtControlBranch = new DataTable();
                        //dtControlBranch = dsCC.Tables[0].Copy();
                        //ControlBranch.Tables.Add(dtControlBranch);
                        //dtControlBranch.TableName = "ControlBranch";
                        //ControlBranch.DataSetName = "ControlBranch";


                        //DataSet Bank = new DataSet();
                        //DataTable _dtBank = dtBank.Copy();
                        //_dtBank.TableName = "Bank";
                        //Bank.Tables.Add(_dtBank);
                        //Bank.DataSetName = "Bank";

                        //DataSet Account = new DataSet();
                        //DataTable dtAccount = _dtAcct.Copy();
                        //dtAccount.TableName = "Account";
                        //Account.Tables.Add(dtAccount);
                        //Account.DataSetName = "Account";


                        report.RegData("Invoice", Invoice);
                        report.RegData("Check", Check);
                        report.RegData("ControlBranch", ControlBranch);
                        report.RegData("dsBank", Bank);
                        report.RegData("dsAccount", Account);
                        report.Render();





                        var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                        var service = new Stimulsoft.Report.Export.StiPdfExportService();
                        System.IO.MemoryStream stream = new System.IO.MemoryStream();
                        service.ExportTo(report, stream, settings);
                        buffer1 = stream.ToArray();
                        lstbyte.Add(buffer1);




                        if (totalRows > rowCount)
                        {
                            byte[] bufferNew = null;
                            reportPathStimul = Server.MapPath("StimulsoftReports/TopCheckSubReport.mrt");
                            report = new StiReport();
                            report.Load(reportPathStimul);
                            report.Compile();

                            //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                            ////if (IsAPIIntegrationEnable == "YES")
                            //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            //{
                            //    report["TotalAmountPay"] = Convert.ToDouble(_dsCheck1.Tables[0].Rows[0]["AmountPay"].ToString());
                            //    report["AccountNo"] = "";
                            //    report["Memo"] = Convert.ToString(_dsCheck2.Tables[0].Rows[0]["Memo"].ToString());
                            //}
                            //else
                            //{
                                report["TotalAmountPay"] = Convert.ToDouble(_dsCheck.Tables[0].Rows[0]["AmountPay"].ToString());
                                report["AccountNo"] = "";
                                report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
                            //}

                            report["InvoiceCount"] = totalRows;
                            Invoice = new DataSet();
                            dtInvoice = secondHalf.Copy();
                            dtInvoice.TableName = "Invoice";
                            Invoice.Tables.Add(dtInvoice);
                            Invoice.DataSetName = "Invoice";

                            Check = new DataSet();
                            dtCheck = dtpay.Copy();
                            dtCheck.TableName = "Check";
                            Check.Tables.Add(dtCheck);
                            Check.DataSetName = "Check";

                            //DataSet ControlBranch = new DataSet();
                            //DataTable dtControlBranch = new DataTable();
                            //dtControlBranch = dsC.Tables[0].Copy();
                            //ControlBranch.Tables.Add(dtControlBranch);
                            //dtControlBranch.TableName = "ControlBranch";
                            //ControlBranch.DataSetName = "ControlBranch";


                            


                            report.RegData("Invoice", Invoice);
                            report.RegData("Check", Check);
                            report.RegData("ControlBranch", ControlBranch);
                            report.RegData("Bank", Bank);
                            report.RegData("Account", Account);
                            report.Render();


                            settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                            service = new Stimulsoft.Report.Export.StiPdfExportService();
                            stream = new System.IO.MemoryStream();
                            service.ExportTo(report, stream, settings);
                            bufferNew = stream.ToArray();

                            lstbyteNew.Add(bufferNew);
                        }
                    }
                    count++;
                }

                _dtAcct.Reset();
                dti.Reset();
                dtpay.Reset();
                dtBank.Reset();
            }
            byte[] finalbyte = null;

            if (lstbyteNew.Count != 0)
            {
                finalbyte = WriteChecks.concatAndAddContentFinal(lstbyte, lstbyteNew);
            }
            else
            {
                finalbyte = WriteChecks.concatAndAddContent(lstbyte);
            }
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment;filename=TopDetailCheckCub.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (finalbyte.Length).ToString());
            Response.BinaryWrite(finalbyte);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillReportMaddenDataSet(string reportName)
    {
        try
        {
            double SumAmountpay = 0.00;
            GetPaymentTotal();

            if (int.Parse(ddlVendor.SelectedValue) <= 0)
            {
                DataTable dt = (DataTable)Session["dsbills"];
                DataTable dtNew = new DataTable();
                dtNew.Columns.Add("Name");
                dtNew.Columns.Add("Vendor");
                foreach (DataRow drow in dt.Rows)
                {
                    DataRow drNew = dtNew.NewRow();
                    drNew["Name"] = drow["Name"].ToString();
                    drNew["Vendor"] = drow["Vendor"].ToString();
                    dtNew.Rows.Add(drNew);
                }

                DataTable dtN = dtNew.DefaultView.ToTable(true);
                DataTable _dtAcct = new DataTable();
                int count = 0;
                foreach (DataRow dr in dtN.Rows)
                {
                    bool isChecked = false;
                    CreateTableInvoice();
                    CreateTablePayee();
                    CreateTableBank();
                    //DataTable _dtAcct = new DataTable();
                    _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
                    _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

                    DataRow _dri = null;
                    DataRow _drC = null;

                    int vid = Convert.ToInt32(dr["Vendor"].ToString());
                    //ac     int vid = Convert.ToInt32(ddlVendor.SelectedValue);
                    //RAHIL'S IMPLEMENTATION
                    DataRow _drB = null;
                    DataRow _drA = null;
                    double AmountPay = 0.00;
                    SumAmountpay = 0.00;
                    long checkNo = long.Parse(ViewState["Checkno"].ToString());

                    //foreach (GridViewRow gr in gvBills.Rows)
                    foreach (GridDataItem gr in gvBills.Items)
                    {
                        Label lblVendor = (Label)gr.FindControl("lblVendor");
                        if (lblVendor.Text == dr["Name"].ToString())
                        {
                            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

                            if (chkSelect.Checked == true)
                            {
                                isChecked = true;
                                TextBox txtGvPay = (TextBox)gr.FindControl("txtGvPay");

                                Label lblBalance = (Label)gr.FindControl("lblBalance");
                                TextBox txtGvDisc = (TextBox)gr.FindControl("txtGvDisc");
                                Label lblOrig = (Label)gr.FindControl("lblOrig");
                                HiddenField hdnRef = (HiddenField)gr.FindControl("hdnRef");

                                Label lblBillfdesc = (Label)gr.FindControl("lblBillfdesc");
                                TextBox txtGvDesc = (TextBox)gr.FindControl("txtGvDesc");
                                Label lblfDate = (Label)gr.FindControl("lblfDate");
                                Label lblRef = (Label)gr.FindControl("lblRef");

                                _dri = dti.NewRow();
                                _dri["Ref"] = hdnRef.Value;
                                _dri["InvoiceDate"] = lblfDate.Text;
                                _dri["Reference"] = lblRef.Text;
                                //_dri["Description"] = lblBillfdesc.Text;
                                _dri["Description"] = txtGvDesc.Text;
                                _dri["Total"] = double.Parse(lblOrig.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                              NumberStyles.AllowThousands |
                                              NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                                _dri["Disc"] = Convert.ToDouble(txtGvDisc.Text).ToString();
                                _dri["AmountPay"] = Convert.ToDouble(txtGvPay.Text).ToString();
                                AmountPay = AmountPay + Convert.ToDouble(txtGvPay.Text);
                                _dri["PayDate"] = txtDate.Text;
                                //_dri["CheckNo"] = ViewState["Checkno"].ToString();
                                if (!string.IsNullOrEmpty(txtNextCheck.Text))
                                {
                                    checkNo = long.Parse(txtNextCheck.Text);
                                }
                                else
                                {
                                    checkNo = long.Parse(ViewState["Checkno"].ToString());
                                }
                                //if (ddlPayment.SelectedValue == "0")
                                //{
                                //    _dri["CheckNo"] = checkNo + count;
                                //}
                                //else
                                //{
                                //    _dri["CheckNo"] = checkNo;
                                //}
                                
                                _dri["CheckNo"] = checkNo + count;
                                

                                //if (!string.IsNullOrEmpty(txtNextCheck.Text))
                                //{
                                //    _dri["CheckNo"] = txtNextCheck.Text;
                                //}
                                //else
                                //{
                                //    _dri["CheckNo"] = ViewState["Checkno"].ToString();
                                //}
                                // ac_dri["VendorID"] = Convert.ToInt32(ddlVendor.SelectedValue);
                                _dri["VendorID"] = dr["Vendor"];
                                //ac _dri["VendorName"] = ddlVendor.SelectedItem.Text;
                                _dri["VendorName"] = lblVendor.Text;
                                dti.Rows.Add(_dri);


                            }
                        }
                    }
                    while (AmountPay < 0)
                    {
                        List<DataRow> rowsWantToDelete = new List<DataRow>();
                        foreach (DataRow drow in dti.Rows)
                        {
                            if (Convert.ToDouble(drow["AmountPay"].ToString()) < 0)
                            {
                                rowsWantToDelete.Add(drow);
                                foreach (DataRow rows in rowsWantToDelete)
                                {
                                    dti.Rows.Remove(rows);
                                }

                                AmountPay = dti.AsEnumerable().Sum(x => Convert.ToDouble(x["AmountPay"]));
                                break;
                            }
                        }


                    }
                    if (isChecked)
                    {
                        _objVendor.ConnConfig = Session["config"].ToString();
                        _objVendor.ID = vid;

                        _getVendorRolDetails.ConnConfig = Session["config"].ToString();
                        _getVendorRolDetails.ID = vid;

                        DataSet _dsV = new DataSet();
                        List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "ManageChecksAPI/CheckList_GetVendorRolDetails";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorRolDetails);

                            _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
                            _dsV = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
                            _dsV.Tables[0].Columns.Remove("Type");
                            _dsV.Tables[0].Columns["VType"].ColumnName = "Type";
                            _dsV.Tables[0].Columns["Vendor1099"].ColumnName = "1099";
                            _dsV.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                            //_dsV.Tables[0].Columns["Remit"].ColumnName = "RemitAddress";
                        }
                        else
                        {
                            _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
                        }

                        string vendAddress = "";
                        string vendAddress2 = "";
                        if (_dsV.Tables[0].Rows.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
                            {
                                string add1 = Regex.Replace(_dsV.Tables[0].Rows[0]["Address"].ToString(), @"( |\r?\n)\1+", "$1");
                                vendAddress = add1 + ", ";
                            }

                            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                            {
                                vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
                            }
                            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
                            {
                                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                                {
                                    vendAddress2 += ", ";
                                }
                                vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
                            }
                        }
                        if (dti.Rows.Count > 0)
                        {
                            string _amount = String.Format("{0:c}", Convert.ToDouble(AmountPay));
                            _amount = _amount.Replace("$", string.Empty);
                            _drC = dtpay.NewRow();

                            //if (Convert.ToDouble(ViewState["Amount"]) > 1000)
                            if (AmountPay > 1000)
                            {
                                _drC["Pay"] = ConvertNumberToCurrency(AmountPay);
                                //_drC["Pay"] = ViewState["Dollar"].ToString();
                            }
                            else
                            {
                                string dollar = ConvertNumberToCurrency(AmountPay);
                                _drC["Pay"] = dollar + " Dollars";
                            }
                            _drC["ToOrder"] = dr["Name"].ToString();
                            //_drC["ToOrder"] = ViewState["Vendor"].ToString();
                            _drC["Date"] = txtDate.Text;
                            //_drC["CheckAmount"] = String.Format("{0:c}", Convert.ToDouble(AmountPay));
                            _drC["CheckAmount"] = Convert.ToDouble(AmountPay);
                            _drC["ToOrderAddress"] = vendAddress;
                            _drC["State"] = vendAddress2;
                            _drC["VendorAddress"] = _dsV.Tables[0].Rows[0]["VendorAddress"];                // change by Mayuri on 8th nov,16
                            _drC["RemitAddress"] = _dsV.Tables[0].Rows[0]["RemitAddress"];                  // change by Mayuri on 8th nov,16
                            dtpay.Rows.Add(_drC);
                            long checkno = 0;
                            //RAHIL'S IMPLEMENTATION
                            //_objCD.ConnConfig = Session["config"].ToString();
                            //if (ddlPayment.SelectedValue == "0")
                            //{
                            //    checkno = Convert.ToInt32(ViewState["Checkno"]) + count;
                            //}
                            //else
                            //{
                            //    checkno = Convert.ToInt32(ViewState["Checkno"]);
                            //}

                            DataSet dsC = new DataSet();

                            objPropUser.ConnConfig = Session["config"].ToString();

                            //API
                            _getConnectionConfig.ConnConfig = Session["config"].ToString();

                            if (Session["MSM"].ToString() != "TS")
                            {
                                List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

                                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                                //if (IsAPIIntegrationEnable == "YES")
                                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                                {
                                    string APINAME = "ManageChecksAPI/CheckList_GetControl";

                                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                                    _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                                    dsC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
                                }
                                else
                                {
                                    dsC = objBL_User.getControl(objPropUser);
                                }
                            }
                            else
                            {
                                objPropUser.LocID = Convert.ToInt32(0);

                                //API
                                _getControlBranch.LocID = Convert.ToInt32(0);

                                List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();
                                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                                //if (IsAPIIntegrationEnable == "YES")
                                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                                {
                                    string APINAME = "ManageChecksAPI/CheckList_GetControlBranch";

                                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getControlBranch);

                                    _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                                    dsC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
                                }
                                else
                                {
                                    dsC = objBL_User.getControlBranch(objPropUser);
                                }
                                // dsC = objBL_User.getControlBranch(objPropUser);
                            }

                            checkno = long.Parse (ViewState["Checkno"].ToString()) + count;
                            

                            _objCD.Ref = checkno;

                            _objBank.ConnConfig = Session["config"].ToString();
                            _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);

                            _getBankCD.ConnConfig = Session["config"].ToString();
                            _getBankCD.ID = Convert.ToInt32(ddlBank.SelectedValue);

                            DataSet _dsB = new DataSet();

                            List<BankViewModel> _lstBankViewModel = new List<BankViewModel>();

                            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                string APINAME = "ManageChecksAPI/AddCheck_GetBankCD";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getBankCD);

                                _lstBankViewModel = (new JavaScriptSerializer()).Deserialize<List<BankViewModel>>(_APIResponse.ResponseData);
                                _dsB = CommonMethods.ToDataSet<BankViewModel>(_lstBankViewModel);
                                _dsB.Tables[0].Columns["RolName"].ColumnName = "Name";
                                _dsB.Tables[0].Columns["RolAddress"].ColumnName = "Address";
                                _dsB.Tables[0].Columns["RolState"].ColumnName = "State";
                                _dsB.Tables[0].Columns["RolCity"].ColumnName = "City";
                                _dsB.Tables[0].Columns["RolZip"].ColumnName = "Zip";
                            }
                            else
                            {
                                _dsB = _objBLBill.GetBankCD(_objBank);
                            }

                            _drB = dtBank.NewRow();
                            if (_dsB.Tables[0].Rows.Count > 0)
                            {
                                _drB["Name"] = _dsB.Tables[0].Rows[0]["Name"].ToString();
                                _drB["Address"] = _dsB.Tables[0].Rows[0]["Address"].ToString();
                                _drB["State"] = _dsB.Tables[0].Rows[0]["State"].ToString();
                                _drB["City"] = _dsB.Tables[0].Rows[0]["City"].ToString();
                                _drB["Zip"] = _dsB.Tables[0].Rows[0]["Zip"].ToString();
                                _drB["NBranch"] = _dsB.Tables[0].Rows[0]["NBranch"].ToString();
                                _drB["NAcct"] = _dsB.Tables[0].Rows[0]["NAcct"].ToString();
                                _drB["NRoute"] = _dsB.Tables[0].Rows[0]["NRoute"].ToString();
                                //_drB["Ref"] = _dsB.Tables[0].Rows[0]["Ref"].ToString();
                                //_dtBank.Rows.Add(_drB);
                            }
                            string checkNumber = string.Empty;
                            if (!string.IsNullOrEmpty(txtNextCheck.Text))
                            {
                                checkNumber = checkno.ToString();
                            }
                            else
                            {
                                checkNumber = checkno.ToString();
                            }

                            if (checkNumber.Length == 1)
                            {
                                _drB["Ref"] = "00000000" + checkNumber;
                            }
                            else if (checkNumber.Length == 2)
                            {
                                _drB["Ref"] = "0000000" + checkNumber;
                            }
                            else if (checkNumber.Length == 3)
                            {
                                _drB["Ref"] = "000000" + checkNumber;
                            }
                            else if (checkNumber.Length == 4)
                            {
                                _drB["Ref"] = "00000" + checkNumber;
                            }
                            else if (checkNumber.Length == 5)
                            {
                                _drB["Ref"] = "0000" + checkNumber;
                            }
                            else if (checkNumber.Length == 6)
                            {
                                _drB["Ref"] = "000" + checkNumber;
                            }
                            else if (checkNumber.Length == 7)
                            {
                                _drB["Ref"] = "00" + checkNumber;
                            }
                            else if (checkNumber.Length == 8)
                            {
                                _drB["Ref"] = "0" + checkNumber;
                            }
                            else
                            {
                                _drB["Ref"] = "000000000";
                            }

                            dtBank.Rows.Add(_drB);

                            _objVendor.ConnConfig = Session["config"].ToString();
                            _objVendor.ID = vid;

                            _getVendorAcct.ConnConfig = Session["config"].ToString();
                            _getVendorAcct.ID = vid;

                            DataSet _dsA = new DataSet();
                            List<GetVendorAcctList> _lstGetVendorAcct = new List<GetVendorAcctList>();

                            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                string APINAME = "ManageChecksAPI/AddCheck_GetVendorAcct";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorAcct);

                                _lstGetVendorAcct = (new JavaScriptSerializer()).Deserialize<List<GetVendorAcctList>>(_APIResponse.ResponseData);
                                _dsA = CommonMethods.ToDataSet<GetVendorAcctList>(_lstGetVendorAcct);
                                _dsA.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                            }
                            else
                            {
                                _dsA = _objBLVendor.GetVendorAcct(_objVendor);
                            }

                            _drA = _dtAcct.NewRow();
                            _drA["VendorID"] = _dsA.Tables[0].Rows[0]["ID"].ToString();
                            _drA["VendorAcct"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
                            _dtAcct.Rows.Add(_drA);
                            //-----------------------------------------------



                            var rowCount = 0;
                            var totalRows = dti.Rows.Count;
                            if (reportName.Contains("-"))
                            {
                                try
                                {
                                    string[] reportNameArr = reportName.Split('-');
                                    rowCount = Convert.ToInt32(reportNameArr[1].ToString().Trim().TrimStart());
                                    if (totalRows < rowCount)
                                        rowCount = totalRows;
                                }
                                catch (Exception ex) { rowCount = totalRows; }
                            }
                            else
                                rowCount = 6;
                            var dtiCopy = dti.Copy();
                            DataView dv = dtiCopy.DefaultView;
                            dv.Sort = "Ref asc";
                            DataTable sortedDT = dv.ToTable();
                            var dtCopy = sortedDT.Copy();
                            var firstHalf = dtCopy;
                            var secondHalf = dtCopy;
                            if (dtCopy.Rows.Count > rowCount)
                            {
                                firstHalf = dtCopy.AsEnumerable().Take(rowCount).CopyToDataTable();
                                secondHalf = dtCopy.Clone();
                                if (totalRows > rowCount)
                                {
                                    secondHalf = dtCopy.AsEnumerable().Skip(rowCount).Take(totalRows - rowCount).CopyToDataTable();
                                }
                            }
                            else
                            {
                                firstHalf = dtCopy;
                            }


                            DataTable _dti = new DataTable();

                            
                            //STIMULSOFT 
                            byte[] buffer1 = null;
                            string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + reportName.Trim() + ".mrt");
                            StiReport report = new StiReport();
                            report.Load(reportPathStimul);
                            report.Compile();
                            report["TotalAmountPay"] = AmountPay;
                            report["Memo"] = txtMemo.Text;

                            report["InvoiceCount"] = totalRows;
                            if (dsC.Tables[0].Rows.Count > 0)
                            {
                                report["CompanyName"] = dsC.Tables[0].Rows[0]["Name"].ToString();
                                report["CompanyAddress"] = dsC.Tables[0].Rows[0]["Address"].ToString();
                                report["CompanyCity"] = dsC.Tables[0].Rows[0]["City"].ToString() + "' " + dsC.Tables[0].Rows[0]["State"].ToString() + " - " + dsC.Tables[0].Rows[0]["Zip"].ToString();
                            }
                            DataSet Invoice = new DataSet();
                            DataTable dtInvoice = firstHalf.Copy();
                            dtInvoice.TableName = "Invoice";
                            Invoice.Tables.Add(dtInvoice);
                            Invoice.DataSetName = "Invoice";

                            DataSet Check = new DataSet();
                            DataTable dtCheck = dtpay.Copy();
                            dtCheck.TableName = "Check";
                            Check.Tables.Add(dtCheck);
                            Check.DataSetName = "Check";

                            DataSet ControlBranch = new DataSet();
                            DataTable dtControlBranch = new DataTable();
                            dtControlBranch = dsC.Tables[0].Copy();
                            ControlBranch.Tables.Add(dtControlBranch);
                            dtControlBranch.TableName = "ControlBranch";
                            ControlBranch.DataSetName = "ControlBranch";
                                                       
                            //DataSet Bank = new DataSet();
                            //DataTable _dtBank = dtBank.Copy();
                            //_dtBank.TableName = "Bank";
                            //Bank.Tables.Add(_dtBank);
                            //Bank.DataSetName = "Bank";

                            //DataSet Account = new DataSet();
                            //DataTable dtAccount = _dtAcct.Copy();
                            //dtAccount.TableName = "Account";
                            //Account.Tables.Add(dtAccount);
                            //Account.DataSetName = "Account";

                            DataSet Bank = new DataSet();
                            DataTable _dtBank = new DataTable();
                            _dtBank = dtBank.Copy();
                            dtBank.TableName = "Bank";
                            Bank.Tables.Add(_dtBank);
                            Bank.DataSetName = "Bank";

                            DataSet Account = new DataSet();
                            DataTable dtAccount = new DataTable();
                            dtAccount = _dtAcct.Copy();
                            _dtAcct.TableName = "Account";
                            Account.Tables.Add(dtAccount);
                            Account.DataSetName = "Account";


                            report.RegData("Invoice", Invoice);
                            report.RegData("Check", Check);
                            report.RegData("ControlBranch", ControlBranch);
                            report.RegData("dsBank", Bank);
                            report.RegData("dsAccount", Account);
                            report.Render();
                            var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                            var service = new Stimulsoft.Report.Export.StiPdfExportService();
                            System.IO.MemoryStream stream = new System.IO.MemoryStream();
                            service.ExportTo(report, stream, settings);
                            buffer1 = stream.ToArray();
                            lstbyte.Add(buffer1);




                            if (totalRows > rowCount)
                            {
                                byte[] bufferNew = null;
                                reportPathStimul = Server.MapPath("StimulsoftReports/TopCheckSubReport.mrt");
                                report = new StiReport();
                                report.Load(reportPathStimul);
                                report.Compile();
                                report["TotalAmountPay"] = AmountPay;
                                report["AccountNo"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
                                report["Memo"] = txtMemo.Text;

                                report["InvoiceCount"] = totalRows;
                                Invoice = new DataSet();
                                dtInvoice = secondHalf.Copy();
                                dtInvoice.TableName = "Invoice";
                                Invoice.Tables.Add(dtInvoice);
                                Invoice.DataSetName = "Invoice";

                                Check = new DataSet();
                                dtCheck = dtpay.Copy();
                                dtCheck.TableName = "Check";
                                Check.Tables.Add(dtCheck);
                                Check.DataSetName = "Check";

                                ControlBranch = new DataSet();
                                dtControlBranch = new DataTable();
                                dtControlBranch = dsC.Tables[0].Copy();
                                ControlBranch.Tables.Add(dtControlBranch);
                                dtControlBranch.TableName = "ControlBranch";
                                ControlBranch.DataSetName = "ControlBranch";


                                Bank = new DataSet();
                                _dtBank = dtBank.Copy();
                                _dtBank.TableName = "Bank";
                                Bank.Tables.Add(_dtBank);
                                Bank.DataSetName = "Bank";

                                Account = new DataSet();
                                dtAccount = _dtAcct.Copy();
                                dtAccount.TableName = "Account";
                                Account.Tables.Add(dtAccount);
                                Account.DataSetName = "Account";


                                report.RegData("Invoice", Invoice);
                                report.RegData("Check", Check);
                                report.RegData("Bank", Bank);
                                report.RegData("Account", Account);
                                report.Render();
                                settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                                service = new Stimulsoft.Report.Export.StiPdfExportService();
                                stream = new System.IO.MemoryStream();
                                service.ExportTo(report, stream, settings);
                                bufferNew = stream.ToArray();

                                lstbyteNew.Add(bufferNew);
                            }

                        }
                        count++;
                    }

                    _dtAcct.Reset();
                    dti.Reset();
                    dtpay.Reset();
                    dtBank.Reset();

                }
                byte[] finalbyte = null;

                if (lstbyteNew.Count != 0)
                {
                    finalbyte = WriteChecks.concatAndAddContentFinal(lstbyte, lstbyteNew);
                }
                else
                {
                    finalbyte = WriteChecks.concatAndAddContent(lstbyte);
                }
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Buffer = true;
                Response.AddHeader("Content-Disposition", "attachment;filename=TopDetailCheckCub.pdf");
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Length", (finalbyte.Length).ToString());
                Response.BinaryWrite(finalbyte);
            }




            else
            {

                CreateTableInvoice();
                CreateTablePayee();
                CreateTableBank();
                DataTable _dtAcct = new DataTable();
                _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
                _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

                DataRow _dri = null;
                DataRow _drC = null;
                int vid = Convert.ToInt32(ddlVendor.SelectedValue);
                //RAHIL'S IMPLEMENTATION
                DataRow _drB = null;
                DataRow _drA = null;
                SumAmountpay = 0.00;

                //foreach (GridViewRow gr in gvBills.Rows)
                foreach (GridDataItem gr in gvBills.Items)
                {
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

                    if (chkSelect.Checked == true)
                    {
                        TextBox txtGvPay = (TextBox)gr.FindControl("txtGvPay");
                        Label lblBalance = (Label)gr.FindControl("lblBalance");
                        TextBox txtGvDisc = (TextBox)gr.FindControl("txtGvDisc");
                        Label lblOrig = (Label)gr.FindControl("lblOrig");
                        HiddenField hdnRef = (HiddenField)gr.FindControl("hdnRef");
                        Label lblBillfdesc = (Label)gr.FindControl("lblBillfdesc");
                        TextBox txtGvDesc = (TextBox)gr.FindControl("txtGvDesc");
                        Label lblfDate = (Label)gr.FindControl("lblfDate");
                        Label lblRef = (Label)gr.FindControl("lblRef");

                        _dri = dti.NewRow();
                        _dri["Ref"] = hdnRef.Value;
                        _dri["InvoiceDate"] = lblfDate.Text;
                        _dri["Reference"] = lblRef.Text;
                        //_dri["Description"] = lblBillfdesc.Text;
                        _dri["Description"] = txtGvDesc.Text;
                        _dri["Total"] = double.Parse(lblOrig.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                      NumberStyles.AllowThousands |
                                      NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                        _dri["Disc"] = Convert.ToDouble(txtGvDisc.Text).ToString();
                        _dri["AmountPay"] = Convert.ToDouble(txtGvPay.Text).ToString();
                        SumAmountpay = SumAmountpay + Convert.ToDouble(txtGvPay.Text);
                        _dri["PayDate"] = txtDate.Text;
                        //_dri["CheckNo"] = ViewState["Checkno"].ToString();
                        if (!string.IsNullOrEmpty(txtNextCheck.Text))
                        {
                            _dri["CheckNo"] = txtNextCheck.Text;
                        }
                        else
                        {
                            _dri["CheckNo"] = ViewState["Checkno"].ToString();
                        }
                        _dri["VendorID"] = Convert.ToInt32(ddlVendor.SelectedValue);
                        _dri["VendorName"] = ddlVendor.SelectedItem.Text;
                        dti.Rows.Add(_dri);
                    }
                }

                _objVendor.ConnConfig = Session["config"].ToString();
                _objVendor.ID = vid;

                _getVendorRolDetails.ConnConfig = Session["config"].ToString();
                _getVendorRolDetails.ID = vid;

                DataSet _dsV = new DataSet();
                List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/CheckList_GetVendorRolDetails";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorRolDetails);

                    _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
                    _dsV = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
                    _dsV.Tables[0].Columns.Remove("Type");
                    //_dsV.Tables[0].Columns.Remove("Remit");
                    _dsV.Tables[0].Columns["VType"].ColumnName = "Type";
                    _dsV.Tables[0].Columns["Vendor1099"].ColumnName = "1099";
                    _dsV.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                   // _dsV.Tables[0].Columns["Remit"].ColumnName = "RemitAddress";
                }
                else
                {
                    _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
                }

                //string _amount = String.Format("{0:c}", Convert.ToDouble(ViewState["Amount"]));
                string _amount = String.Format("{0:c}", Convert.ToDouble(SumAmountpay));
                _amount = _amount.Replace("$", string.Empty);
                _drC = dtpay.NewRow();

                //if (Convert.ToDouble(ViewState["Amount"]) > 1000)
                if (Convert.ToDouble(SumAmountpay) > 1000)
                {
                    //_drC["Pay"] = ViewState["Dollar"].ToString();
                    _drC["Pay"] = ConvertNumberToCurrency(SumAmountpay);
                }
                else
                {
                    //string dollar = ViewState["Dollar"].ToString();
                    string dollar = ConvertNumberToCurrency(SumAmountpay);
                    _drC["Pay"] = dollar + " Dollars";
                }
               
                string vendAddress2 = string.Empty;
                string vendAddress = string.Empty;
                if (_dsV.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
                    {
                        string add1 = Regex.Replace(_dsV.Tables[0].Rows[0]["Address"].ToString(), @"( |\r?\n)\1+", "$1");
                        vendAddress = add1 + ", ";
                    }

                    if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                    {
                        vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
                    }
                    if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
                    {
                        if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                        {
                            vendAddress2 += ", ";
                        }
                        vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
                    }
                }

                _drC["ToOrder"] = ViewState["Vendor"].ToString();
                _drC["Date"] = txtDate.Text;
                //_drC["CheckAmount"] =  String.Format("{0:c}", Convert.ToDouble(SumAmountpay));
                _drC["CheckAmount"] = Convert.ToDouble(SumAmountpay);
                _drC["VendorAddress"] = _dsV.Tables[0].Rows[0]["VendorAddress"];                // change by Mayuri on 8th nov,16
                _drC["RemitAddress"] = _dsV.Tables[0].Rows[0]["RemitAddress"];                  // change by Mayuri on 8th nov,16
                if (string.IsNullOrEmpty(_drC["State"].ToString()))
                    _drC["State"] = vendAddress2;
                _drC["ToOrderAddress"] = vendAddress;
                dtpay.Rows.Add(_drC);

                //RAHIL'S IMPLEMENTATION
                //_objCD.ConnConfig = Session["config"].ToString();
                long checkno = long.Parse (ViewState["Checkno"].ToString());
                _objCD.Ref = checkno;

                _objBank.ConnConfig = Session["config"].ToString();
                _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);

                _getBankCD.ConnConfig = Session["config"].ToString();
                _getBankCD.ID = Convert.ToInt32(ddlBank.SelectedValue);
                DataSet _dsB = new DataSet();

                List<BankViewModel> _lstBankViewModel = new List<BankViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/AddCheck_GetBankCD";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getBankCD);

                    _lstBankViewModel = (new JavaScriptSerializer()).Deserialize<List<BankViewModel>>(_APIResponse.ResponseData);
                    _dsB = CommonMethods.ToDataSet<BankViewModel>(_lstBankViewModel);
                    _dsB.Tables[0].Columns["RolName"].ColumnName = "Name";
                    _dsB.Tables[0].Columns["RolAddress"].ColumnName = "Address";
                    _dsB.Tables[0].Columns["RolState"].ColumnName = "State";
                    _dsB.Tables[0].Columns["RolCity"].ColumnName = "City";
                    _dsB.Tables[0].Columns["RolZip"].ColumnName = "Zip";

                }
                else
                {
                    _dsB = _objBLBill.GetBankCD(_objBank);
                }

                _drB = dtBank.NewRow();
                if (_dsB.Tables[0].Rows.Count > 0)
                {
                    _drB["Name"] = _dsB.Tables[0].Rows[0]["Name"].ToString();
                    _drB["Address"] = _dsB.Tables[0].Rows[0]["Address"].ToString();
                    _drB["State"] = _dsB.Tables[0].Rows[0]["State"].ToString();
                    _drB["City"] = _dsB.Tables[0].Rows[0]["City"].ToString();
                    _drB["Zip"] = _dsB.Tables[0].Rows[0]["Zip"].ToString();
                    _drB["NBranch"] = _dsB.Tables[0].Rows[0]["NBranch"].ToString();
                    _drB["NAcct"] = _dsB.Tables[0].Rows[0]["NAcct"].ToString();
                    _drB["NRoute"] = _dsB.Tables[0].Rows[0]["NRoute"].ToString();
                    //_drB["Ref"] = _dsB.Tables[0].Rows[0]["Ref"].ToString();
                    //_dtBank.Rows.Add(_drB);
                }
                string checkNumber = string.Empty;
                if (!string.IsNullOrEmpty(txtNextCheck.Text))
                {
                    checkNumber = txtNextCheck.Text;
                }
                else
                {
                    checkNumber = ViewState["Checkno"].ToString();
                }

                if (checkNumber.Length == 1)
                {
                    _drB["Ref"] = "00000000" + checkNumber;
                }
                else if (checkNumber.Length == 2)
                {
                    _drB["Ref"] = "0000000" + checkNumber;
                }
                else if (checkNumber.Length == 3)
                {
                    _drB["Ref"] = "000000" + checkNumber;
                }
                else if (checkNumber.Length == 4)
                {
                    _drB["Ref"] = "00000" + checkNumber;
                }
                else if (checkNumber.Length == 5)
                {
                    _drB["Ref"] = "0000" + checkNumber;
                }
                else if (checkNumber.Length == 6)
                {
                    _drB["Ref"] = "000" + checkNumber;
                }
                else if (checkNumber.Length == 7)
                {
                    _drB["Ref"] = "00" + checkNumber;
                }
                else if (checkNumber.Length == 8)
                {
                    _drB["Ref"] = "0" + checkNumber;
                }
                else
                {
                    _drB["Ref"] = "000000000";
                }

                dtBank.Rows.Add(_drB);

                _objVendor.ConnConfig = Session["config"].ToString();
                _objVendor.ID = vid;

                _getVendorAcct.ConnConfig = Session["config"].ToString();
                _getVendorAcct.ID = vid;

                DataSet _dsA = new DataSet();
                List<GetVendorAcctList> _lstGetVendorAcct = new List<GetVendorAcctList>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/AddCheck_GetVendorAcct";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorAcct);

                    _lstGetVendorAcct = (new JavaScriptSerializer()).Deserialize<List<GetVendorAcctList>>(_APIResponse.ResponseData);
                    _dsA = CommonMethods.ToDataSet<GetVendorAcctList>(_lstGetVendorAcct);
                    _dsA.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                }
                else
                {
                    _dsA = _objBLVendor.GetVendorAcct(_objVendor);
                }

                _drA = _dtAcct.NewRow();
                _drA["VendorID"] = _dsA.Tables[0].Rows[0]["ID"].ToString();
                _drA["VendorAcct"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
                _dtAcct.Rows.Add(_drA);
                //-----------------------------------------------

                var rowCount = 0;
                var totalRows = dti.Rows.Count;
                if (reportName.Contains("-"))
                {
                    try
                    {
                        string[] reportNameArr = reportName.Split('-');
                        rowCount = Convert.ToInt32(reportNameArr[1].ToString().Trim().TrimStart());
                        if (totalRows < rowCount)
                            rowCount = totalRows;
                    }
                    catch (Exception ex) { rowCount = totalRows; }
                }
                else
                    rowCount = 9;
                var dtiCopy = dti.Copy();
                DataView dv = dtiCopy.DefaultView;
                dv.Sort = "Ref asc";
                DataTable sortedDT = dv.ToTable();
                var dtCopy = sortedDT.Copy();
                var firstHalf = dtCopy;
                var secondHalf = dtCopy;
                if (dtCopy.Rows.Count > rowCount)
                {
                    firstHalf = dtCopy.AsEnumerable().Take(rowCount).CopyToDataTable();
                    secondHalf = dtCopy.Clone();
                    if (totalRows > rowCount)
                    {
                        secondHalf = dtCopy.AsEnumerable().Skip(rowCount).Take(totalRows - rowCount).CopyToDataTable();
                    }
                }
                else
                {
                    firstHalf = dtCopy;
                }

                DataTable _dti = new DataTable();

                DataSet dsC = new DataSet();
                objPropUser.ConnConfig = Session["config"].ToString();
                _getConnectionConfig.ConnConfig = Session["config"].ToString();
                _getControlBranch.ConnConfig = Session["config"].ToString();
                if (Session["MSM"].ToString() != "TS")
                {
                    List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_GetControl";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                        _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                        dsC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
                    }
                    else
                    {
                        dsC = objBL_User.getControl(objPropUser);
                    }
                }
                else
                {
                    objPropUser.LocID = Convert.ToInt32(0);
                    _getControlBranch.LocID = Convert.ToInt32(0);
                    List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_GetControlBranch";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getControlBranch);

                        _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                        dsC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
                    }
                    else
                    {
                        dsC = objBL_User.getControlBranch(objPropUser);
                    }
                }
                //STIMULSOFT 
                byte[] buffer1 = null;
                string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + reportName.Trim() + ".mrt");
                StiReport report = new StiReport();
                report.Load(reportPathStimul);
                report.Compile();
                report["TotalAmountPay"] = SumAmountpay;
                report["Memo"] = txtMemo.Text;

                report["InvoiceCount"] = totalRows;
                if (dsC.Tables[0].Rows.Count > 0)
                {
                    report["CompanyName"] = dsC.Tables[0].Rows[0]["Name"].ToString();
                    report["CompanyAddress"] = dsC.Tables[0].Rows[0]["Address"].ToString();
                    report["CompanyCity"] = dsC.Tables[0].Rows[0]["City"].ToString() + "' " + dsC.Tables[0].Rows[0]["State"].ToString() + " - " + dsC.Tables[0].Rows[0]["Zip"].ToString();
                }
                DataSet Invoice = new DataSet();
                DataTable dtInvoice = firstHalf.Copy();
                dtInvoice.TableName = "Invoice";
                Invoice.Tables.Add(dtInvoice);
                Invoice.DataSetName = "Invoice";

                DataSet Check = new DataSet();
                DataTable dtCheck = dtpay.Copy();
                dtCheck.TableName = "Check";
                Check.Tables.Add(dtCheck);
                Check.DataSetName = "Check";

                DataSet ControlBranch = new DataSet();
                DataTable dtControlBranch = new DataTable();
                dtControlBranch = dsC.Tables[0].Copy();
                ControlBranch.Tables.Add(dtControlBranch);
                dtControlBranch.TableName = "ControlBranch";
                ControlBranch.DataSetName = "ControlBranch";


                DataSet Bank = new DataSet();
                DataTable _dtBank = dtBank.Copy();
                _dtBank.TableName = "Bank";
                Bank.Tables.Add(_dtBank);
                Bank.DataSetName = "Bank";

                DataSet Account = new DataSet();
                DataTable dtAccount = _dtAcct.Copy();
                dtAccount.TableName = "Account";
                Account.Tables.Add(dtAccount);
                Account.DataSetName = "Account";


                report.RegData("Invoice", Invoice);
                report.RegData("Check", Check);
                report.RegData("dsBank", Bank);
                report.RegData("dsTicket", ControlBranch);
                report.RegData("ControlBranch", ControlBranch);
                report.RegData("dsAccount", Account);
                report.Render();
                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(report, stream, settings);
                buffer1 = stream.ToArray();
                lstbyte.Add(buffer1);




                if (totalRows > 9)
                {
                    byte[] bufferNew = null;
                    reportPathStimul = Server.MapPath("StimulsoftReports/TopCheckSubReport.mrt");
                    report = new StiReport();
                    report.Load(reportPathStimul);
                    report.Compile();
                    report["TotalAmountPay"] = SumAmountpay;
                    report["AccountNo"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
                    report["Memo"] = txtMemo.Text;

                    report["InvoiceCount"] = totalRows;
                    Invoice = new DataSet();
                    dtInvoice = secondHalf.Copy();
                    dtInvoice.TableName = "Invoice";
                    Invoice.Tables.Add(dtInvoice);
                    Invoice.DataSetName = "Invoice";

                    Check = new DataSet();
                    dtCheck = dtpay.Copy();
                    dtCheck.TableName = "Check";
                    Check.Tables.Add(dtCheck);
                    Check.DataSetName = "Check";

                    ControlBranch = new DataSet();
                    dtControlBranch = new DataTable();
                    dtControlBranch = dsC.Tables[0].Copy();
                    ControlBranch.Tables.Add(dtControlBranch);
                    dtControlBranch.TableName = "ControlBranch";
                    ControlBranch.DataSetName = "ControlBranch";


                    Bank = new DataSet();
                    _dtBank = dtBank.Copy();
                    _dtBank.TableName = "Bank";
                    Bank.Tables.Add(_dtBank);
                    Bank.DataSetName = "Bank";

                    Account = new DataSet();
                    dtAccount = _dtAcct.Copy();
                    dtAccount.TableName = "Account";
                    Account.Tables.Add(dtAccount);
                    Account.DataSetName = "Account";


                    report.RegData("Invoice", Invoice);
                    report.RegData("Check", Check);
                    report.RegData("Bank", Bank);
                    report.RegData("dsTicket", ControlBranch);
                    report.RegData("ControlBranch", ControlBranch);
                    report.RegData("Account", Account);
                    report.Render();
                    settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                    service = new Stimulsoft.Report.Export.StiPdfExportService();
                    stream = new System.IO.MemoryStream();
                    service.ExportTo(report, stream, settings);
                    bufferNew = stream.ToArray();

                    lstbyteNew.Add(bufferNew);
                }
                byte[] finalbyte = null;

                if (lstbyteNew.Count != 0)
                {
                    finalbyte = WriteChecks.concatAndAddContentFinal(lstbyte, lstbyteNew);
                }
                else
                {
                    finalbyte = WriteChecks.concatAndAddContent(lstbyte);
                }
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Buffer = true;
                Response.AddHeader("Content-Disposition", "attachment;filename=TopDetailCheckCub.pdf");
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Length", (finalbyte.Length).ToString());
                Response.BinaryWrite(finalbyte);
            }



        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillReportApTopCheckDataSet_New(string reportName)
    {
        try
        {
            double SumAmountpay = 0.00;
            int count = 0;
            _objCD.ConnConfig = Session["config"].ToString();
            _objCD.Ref = long.Parse(txtNextCheck.Text);
            _objCD.NextC = long.Parse(txtNextCheck.Text);
            _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);

            _getCheckDetailsByBankAndRef.ConnConfig = Session["config"].ToString();
            _getCheckDetailsByBankAndRef.Ref = long.Parse(txtNextCheck.Text);
            _getCheckDetailsByBankAndRef.NextC = long.Parse(txtNextCheck.Text);
            _getCheckDetailsByBankAndRef.Bank = Convert.ToInt32(ddlBank.SelectedValue);

            DataSet _dsCheck = new DataSet();
            DataSet _dsCheck1 = new DataSet();
            DataSet _dsCheck2 = new DataSet();
            ListCheckDetailsByBankAndRef _ListCheckDetailsByBankAndRef = new ListCheckDetailsByBankAndRef();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetCheckDetailsByBankAndRef";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCheckDetailsByBankAndRef);

                _ListCheckDetailsByBankAndRef = (new JavaScriptSerializer()).Deserialize<ListCheckDetailsByBankAndRef>(_APIResponse.ResponseData);

                _dsCheck1 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable1.ToDataSet();
                _dsCheck2 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable2.ToDataSet();

                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                dt1 = _dsCheck1.Tables[0];
                dt2 = _dsCheck2.Tables[0];

                dt1.TableName = "Table1";
                dt2.TableName = "Table2";

                _dsCheck.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });
            }
            else
            {
                _dsCheck = _objBLBill.GetCheckDetailsByBankAndRef(_objCD);
            }

            DataSet ControlBranch = new DataSet();
            DataTable dtControlBranch = new DataTable();
            dtControlBranch = _dsCheck.Tables[2].Copy();
            ControlBranch.Tables.Add(dtControlBranch);
            dtControlBranch.TableName = "ControlBranch";
            ControlBranch.DataSetName = "ControlBranch";

            DataSet Account = new DataSet();
            DataTable dtAccount = new DataTable();
            dtAccount = _dsCheck.Tables[3].Copy();
            Account.Tables.Add(dtAccount);
            dtAccount.TableName = "Account";
            Account.DataSetName = "Account";

            DataSet Bank = new DataSet();
            DataTable dtBank = new DataTable();
            dtBank = _dsCheck.Tables[4].Copy();
            Bank.Tables.Add(dtBank);
            dtBank.TableName = "Bank";
            Bank.DataSetName = "Bank";


            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("Name");
            dtNew.Columns.Add("Vendor");
            dtNew.Columns.Add("CheckNo");

            //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            ////if (IsAPIIntegrationEnable == "YES")
            //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            //{
            //    foreach (DataRow drow in _dsCheck1.Tables[0].Rows)
            //    {
            //        DataRow drNew = dtNew.NewRow();
            //        drNew["Name"] = drow["VendorName"].ToString();
            //        drNew["Vendor"] = drow["Vendor"].ToString();
            //        drNew["CheckNo"] = drow["CheckNo"].ToString();
            //        dtNew.Rows.Add(drNew);
            //    }
            //}
            //else
            //{
                foreach (DataRow drow in _dsCheck.Tables[0].Rows)
                {
                    DataRow drNew = dtNew.NewRow();
                    drNew["Name"] = drow["VendorName"].ToString();
                    drNew["Vendor"] = drow["Vendor"].ToString();
                    drNew["CheckNo"] = drow["CheckNo"].ToString();
                    dtNew.Rows.Add(drNew);
                }
            //}

            DataTable dtN = dtNew.DefaultView.ToTable(true);
            DataTable _dtAcct = new DataTable();
            foreach (DataRow dr in dtN.Rows)
            {
                bool isChecked = true;
                _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
                _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

                CreateTableInvoice();
                CreateTablePayee();
                CreateTableBank();


                DataRow _dri = null;
                DataRow _drC = null;

                int vid = Convert.ToInt32(dr["Vendor"].ToString());
                string checkNo = Convert.ToString(dr["CheckNo"].ToString());
                //ac     int vid = Convert.ToInt32(ddlVendor.SelectedValue);
                //RAHIL'S IMPLEMENTATION
                DataRow _drB = null;
                DataRow _drA = null;
                double AmountPay = 0.00;
                SumAmountpay = 0.00;



                //DataTable dtInvoice = _dsCheck.Tables[0].DefaultView.ToTable(true);
                DataView dtInv = new DataView();
                //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                ////if (IsAPIIntegrationEnable == "YES")
                //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                //{
                //    dtInv = _dsCheck1.Tables[0].DefaultView;
                //}
                //else
                //{
                    dtInv = _dsCheck.Tables[0].DefaultView;
                //}

                dtInv.RowFilter = "Vendor = '" + vid + "' and CheckNo = '" + checkNo + "'";
                foreach (DataRow drow in dtInv.ToTable(true).Rows)
                {
                    _dri = dti.NewRow();
                    _dri["Ref"] = drow["Ref"].ToString();
                    _dri["Description"] = drow["Description"].ToString();
                    _dri["InvoiceDate"] = drow["InvoiceDate"].ToString();
                    _dri["Reference"] = drow["Refrerence"].ToString();
                    _dri["Total"] = double.Parse(drow["Total"].ToString().Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                    _dri["Disc"] = Convert.ToDouble(drow["Disc"].ToString()).ToString();
                    _dri["AmountPay"] = Convert.ToDouble(drow["AmountPay"].ToString()).ToString();
                    AmountPay = AmountPay + Convert.ToDouble(drow["AmountPay"].ToString());
                    _dri["PayDate"] = drow["PayDate"].ToString();
                    _dri["CheckNo"] = drow["CheckNo"].ToString();


                    //_dri["VendorID"] = Convert.ToInt32(ddlVendor.SelectedValue);
                    _dri["VendorID"] = drow["Vendor"].ToString();
                    //ac _dri["VendorName"] = ddlVendor.SelectedItem.Text;
                    _dri["VendorName"] = drow["VendorName"].ToString();
                    dti.Rows.Add(_dri);

                    dti.AcceptChanges();

                }
                if (isChecked)
                {
                    if (dti.Rows.Count > 0)
                    {
                        DataView dtcheck = new DataView();
                        //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                        ////if (IsAPIIntegrationEnable == "YES")
                        //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        //{
                        //    dtcheck = _dsCheck2.Tables[0].DefaultView;
                        //}
                        //else
                        //{
                            dtcheck = _dsCheck.Tables[1].DefaultView;
                        //}

                        dtcheck.RowFilter = "Vendor = '" + vid + "' and CheckNo = '" + checkNo + "'";
                        ViewState["CheckStatus"] = "0";
                        foreach (DataRow drow in dtcheck.ToTable(true).Rows)
                        //foreach (DataRow drow in _dsCheck.Tables[1].Rows)
                        {
                            _drC = dtpay.NewRow();
                            if (Convert.ToDouble(drow["Pay"]) > 1000)
                            {
                                _drC["Pay"] = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                                //_drC["Pay"] = ViewState["Dollar"].ToString();
                            }
                            else
                            {
                                string dollar = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                                _drC["Pay"] = dollar + " Dollars";
                            }
                            _drC["ToOrder"] = drow["ToOrder"].ToString();
                            //_drC["ToOrder"] = ViewState["Vendor"].ToString();
                            _drC["Date"] = drow["Date"].ToString();
                            _drC["CheckAmount"] = Convert.ToDouble(drow["Pay"]);
                            _drC["VendorAddress"] = drow["VendorAddress"].ToString();                 // change by Mayuri on 8th nov,16
                            _drC["RemitAddress"] = drow["RemitAddress"].ToString();                  // change by Mayuri on 8th nov,16
                            _drC["State"] = drow["State"].ToString();
                            ViewState["CheckStatus"] = drow["Status"].ToString();
                            dtpay.Rows.Add(_drC);
                        }

                        var rowCount = 0;
                        var totalRows = dti.Rows.Count;
                        if (reportName.Contains("-"))
                        {
                            try
                            {
                                string[] reportNameArr = reportName.Split('-');
                                rowCount = Convert.ToInt32(reportNameArr[1].ToString().Trim().TrimStart());
                                if (totalRows < rowCount)
                                    rowCount = totalRows;
                            }
                            catch (Exception ex) { rowCount = totalRows; }
                        }
                        else
                            rowCount = 6;
                        var dtiCopy = dti.Copy();
                        DataView dv = dtiCopy.DefaultView;
                        dv.Sort = "Ref asc";
                        DataTable sortedDT = dv.ToTable();
                        var dtCopy = sortedDT.Copy();
                        var firstHalf = dtCopy;
                        var secondHalf = dtCopy;
                        if (dtCopy.Rows.Count > rowCount)
                        {
                            firstHalf = dtCopy.AsEnumerable().Take(rowCount).CopyToDataTable();
                            secondHalf = dtCopy.Clone();
                            if (totalRows > rowCount)
                            {
                                secondHalf = dtCopy.AsEnumerable().Skip(rowCount).Take(totalRows - rowCount).CopyToDataTable();
                            }
                        }
                        else
                        {
                            firstHalf = dtCopy;
                        }

                        //STIMULSOFT 
                        byte[] buffer1 = null;
                        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + reportName.Trim() + ".mrt");
                        StiReport report = new StiReport();
                        report.Load(reportPathStimul);
                        if (Convert.ToInt16(ViewState["CheckStatus"]).Equals(2))
                        {
                            report.Pages[0].Watermark.Enabled = true;
                            //report.Pages[0].Watermark.Angle = 0;
                            //report.Pages[0].Watermark.Text = "Void";
                            string imagepath = Server.MapPath("images/icons/voidcheck.png");
                            report.Pages[0].Watermark.Image = System.Drawing.Image.FromFile(imagepath);
                            report.Pages[0].Watermark.ImageAlignment = System.Drawing.ContentAlignment.TopCenter;
                            report.Pages[0].Watermark.ShowImageBehind = true;
                        }
                        report.Compile();
                        report["TotalAmountPay"] = AmountPay;
                        //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                        ////if (IsAPIIntegrationEnable == "YES")
                        //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        //{
                        //    report["Memo"] = Convert.ToString(_dsCheck2.Tables[0].Rows[0]["Memo"].ToString());
                        //}
                        //else
                        //{
                            report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
                        //}
                        report["InvoiceCount"] = totalRows;
                        DataSet Invoice = new DataSet();
                        DataTable dtInvoice = firstHalf.Copy();
                        dtInvoice.TableName = "Invoice";
                        Invoice.Tables.Add(dtInvoice);
                        Invoice.DataSetName = "Invoice";

                        DataSet Check = new DataSet();
                        DataTable dtCheck = dtpay.Copy();
                        dtCheck.TableName = "Check";
                        Check.Tables.Add(dtCheck);
                        Check.DataSetName = "Check";

                        //DataSet ControlBranch = new DataSet();
                        //DataTable dtControlBranch = new DataTable();
                        //dtControlBranch = dsC.Tables[0].Copy();
                        //ControlBranch.Tables.Add(dtControlBranch);
                        //dtControlBranch.TableName = "ControlBranch";
                        //ControlBranch.DataSetName = "ControlBranch";





                        report.RegData("Invoice", Invoice);
                        report.RegData("Check", Check);
                        report.RegData("ControlBranch", ControlBranch);
                        report.RegData("dsBank", Bank);
                        report.RegData("dsAccount", Account);

                        report.Render();
                        var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                        var service = new Stimulsoft.Report.Export.StiPdfExportService();
                        System.IO.MemoryStream stream = new System.IO.MemoryStream();
                        service.ExportTo(report, stream, settings);
                        buffer1 = stream.ToArray();
                        lstbyte.Add(buffer1);




                        if (totalRows > rowCount)
                        {
                            byte[] bufferNew = null;
                            reportPathStimul = Server.MapPath("StimulsoftReports/TopCheckSubReport.mrt");
                            report = new StiReport();
                            report.Load(reportPathStimul);
                            report.Compile();

                            //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                            ////if (IsAPIIntegrationEnable == "YES")
                            //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            //{
                            //    report["TotalAmountPay"] = Convert.ToDouble(_dsCheck1.Tables[0].Rows[0]["AmountPay"].ToString());
                            //    report["AccountNo"] = "";
                            //    report["Memo"] = Convert.ToString(_dsCheck2.Tables[0].Rows[0]["Memo"].ToString());
                            //}
                            //else
                            //{
                                report["TotalAmountPay"] = Convert.ToDouble(_dsCheck.Tables[0].Rows[0]["AmountPay"].ToString());
                                report["AccountNo"] = "";
                                report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
                            //}

                            report["InvoiceCount"] = totalRows;
                            Invoice = new DataSet();
                            dtInvoice = secondHalf.Copy();
                            dtInvoice.TableName = "Invoice";
                            Invoice.Tables.Add(dtInvoice);
                            Invoice.DataSetName = "Invoice";

                            Check = new DataSet();
                            dtCheck = dtpay.Copy();
                            dtCheck.TableName = "Check";
                            Check.Tables.Add(dtCheck);
                            Check.DataSetName = "Check";

                            //DataSet ControlBranch = new DataSet();
                            //DataTable dtControlBranch = new DataTable();
                            //dtControlBranch = dsC.Tables[0].Copy();
                            //ControlBranch.Tables.Add(dtControlBranch);
                            //dtControlBranch.TableName = "ControlBranch";
                            //ControlBranch.DataSetName = "ControlBranch";




                            report.RegData("Invoice", Invoice);
                            report.RegData("Check", Check);
                            report.Render();
                            settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                            service = new Stimulsoft.Report.Export.StiPdfExportService();
                            stream = new System.IO.MemoryStream();
                            service.ExportTo(report, stream, settings);
                            bufferNew = stream.ToArray();

                            lstbyteNew.Add(bufferNew);
                        }
                    }
                    count++;
                }

                _dtAcct.Reset();
                dti.Reset();
                dtpay.Reset();
                dtBank.Reset();
            }
            byte[] finalbyte = null;

            if (lstbyteNew.Count != 0)
            {
                finalbyte = WriteChecks.concatAndAddContentFinal(lstbyte, lstbyteNew);
            }
            else
            {
                finalbyte = WriteChecks.concatAndAddContent(lstbyte);
            }
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment;filename=ApTopCheckCub.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (finalbyte.Length).ToString());
            Response.BinaryWrite(finalbyte);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillReportApTopCheckDataSet(string reportName)
    {
        try
        {
            int chkbillstatus = 0;
            string chkbillstatusname = "";
            double SumAmountpay = 0.00;
            GetPaymentTotal();
            if (int.Parse(ddlVendor.SelectedValue) <= 0)
            {
                DataTable dt = (DataTable)Session["dsbills"];
                DataTable dtNew = new DataTable();
                dtNew.Columns.Add("Name");
                dtNew.Columns.Add("Vendor");
                foreach (DataRow drow in dt.Rows)
                {
                    DataRow drNew = dtNew.NewRow();
                    drNew["Name"] = drow["Name"].ToString();
                    drNew["Vendor"] = drow["Vendor"].ToString();
                    dtNew.Rows.Add(drNew);
                }

                DataTable dtN = dtNew.DefaultView.ToTable(true);
                DataTable _dtAcct = new DataTable();
                int count = 0;
                foreach (DataRow dr in dtN.Rows)
                {
                    bool isChecked = false;
                    CreateTableInvoice();
                    CreateTablePayee();
                    CreateTableBank();
                    //DataTable _dtAcct = new DataTable();
                    _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
                    _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

                    DataRow _dri = null;
                    DataRow _drC = null;

                    int vid = Convert.ToInt32(dr["Vendor"].ToString());
                    //ac     int vid = Convert.ToInt32(ddlVendor.SelectedValue);
                    //RAHIL'S IMPLEMENTATION
                    DataRow _drB = null;
                    DataRow _drA = null;
                    double AmountPay = 0.00;
                    SumAmountpay = 0.00;
                    long checkNo = long.Parse(ViewState["Checkno"].ToString());

                    //foreach (GridViewRow gr in gvBills.Rows)
                    foreach (GridDataItem gr in gvBills.Items)
                    {
                        Label lblVendor = (Label)gr.FindControl("lblVendor");
                        if (lblVendor.Text == dr["Name"].ToString())
                        {
                            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

                            if (chkSelect.Checked == true)
                            {
                                isChecked = true;
                                TextBox txtGvPay = (TextBox)gr.FindControl("txtGvPay");

                                Label lblBalance = (Label)gr.FindControl("lblBalance");
                                TextBox txtGvDisc = (TextBox)gr.FindControl("txtGvDisc");
                                Label lblOrig = (Label)gr.FindControl("lblOrig");
                                HiddenField hdnRef = (HiddenField)gr.FindControl("hdnRef");

                                Label lblBillfdesc = (Label)gr.FindControl("lblBillfdesc");
                                TextBox txtGvDesc = (TextBox)gr.FindControl("txtGvDesc");
                                
                                Label lblfDate = (Label)gr.FindControl("lblfDate");
                                Label lblRef = (Label)gr.FindControl("lblRef");
                                //HiddenField hdnbillspec = (HiddenField)gr.FindControl("hdnbillspec");
                                //HiddenField hdnbillspecstatus = (HiddenField)gr.FindControl("hdnbillspec");
                                
                                    _dri = dti.NewRow();
                                    _dri["Ref"] = hdnRef.Value;
                                //_dri["Description"] = lblBillfdesc.Text;
                                _dri["Description"] = txtGvDesc.Text;
                                _dri["InvoiceDate"] = lblfDate.Text;
                                    _dri["Reference"] = lblRef.Text;
                                    _dri["Total"] = double.Parse(lblOrig.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                  NumberStyles.AllowThousands |
                                                  NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                                    _dri["Disc"] = Convert.ToDouble(txtGvDisc.Text).ToString();
                                    _dri["AmountPay"] = Convert.ToDouble(txtGvPay.Text).ToString();
                                    AmountPay = AmountPay + Convert.ToDouble(txtGvPay.Text);
                                    _dri["PayDate"] = txtDate.Text;
                                    _dri["CheckNo"] = ViewState["Checkno"].ToString();
                                    if (!string.IsNullOrEmpty(txtNextCheck.Text))
                                    {
                                        checkNo = long.Parse(txtNextCheck.Text);
                                    }
                                    else
                                    {
                                        checkNo = long.Parse(ViewState["Checkno"].ToString());
                                    }
                                //if (ddlPayment.SelectedValue == "0")
                                //{
                                //    _dri["CheckNo"] = checkNo + count;
                                //}
                                //else
                                //{
                                //    _dri["CheckNo"] = checkNo;
                                //}

                                
                                _dri["CheckNo"] = checkNo + count;
                                

                                //_dri["VendorID"] = Convert.ToInt32(ddlVendor.SelectedValue);
                                _dri["VendorID"] = dr["Vendor"];
                                    //ac _dri["VendorName"] = ddlVendor.SelectedItem.Text;
                                    _dri["VendorName"] = lblVendor.Text;
                                    dti.Rows.Add(_dri);

                                    dti.AcceptChanges();
                                
                            }
                        }
                    }

                    


                    while (AmountPay < 0)
                    {
                        List<DataRow> rowsWantToDelete = new List<DataRow>();
                        foreach (DataRow drow in dti.Rows)
                        {
                            if (Convert.ToDouble(drow["AmountPay"].ToString()) < 0)
                            {
                                rowsWantToDelete.Add(drow);
                                foreach (DataRow rows in rowsWantToDelete)
                                {
                                    dti.Rows.Remove(rows);
                                }

                                AmountPay = dti.AsEnumerable().Sum(x => Convert.ToDouble(x["AmountPay"]));
                                break;
                            }
                        }


                    }
                    if (isChecked)
                    {
                        _objVendor.ConnConfig = Session["config"].ToString();
                        _objVendor.ID = vid;

                        _getVendorRolDetails.ConnConfig = Session["config"].ToString();
                        _getVendorRolDetails.ID = vid;

                        DataSet _dsV = new DataSet();
                        List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "ManageChecksAPI/CheckList_GetVendorRolDetails";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorRolDetails);

                            _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
                            _dsV = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
                            _dsV.Tables[0].Columns.Remove("Type");
                            _dsV.Tables[0].Columns["VType"].ColumnName = "Type";
                            _dsV.Tables[0].Columns["Vendor1099"].ColumnName = "1099";
                            _dsV.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                           //_dsV.Tables[0].Columns["Remit"].ColumnName = "RemitAddress";
                        }
                        else
                        {
                            _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
                        }

                        string vendAddress = "";
                        string vendAddress2 = "";
                        if (_dsV.Tables[0].Rows.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
                            {
                                vendAddress = _dsV.Tables[0].Rows[0]["Address"].ToString() + ", ";
                            }

                            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                            {
                                vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
                            }
                            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
                            {
                                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                                {
                                    vendAddress2 += ", ";
                                }
                                vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
                            }
                        }

                        DataSet dsC = new DataSet();

                        objPropUser.ConnConfig = Session["config"].ToString();

                        //API
                        _getConnectionConfig.ConnConfig = Session["config"].ToString();

                        if (Session["MSM"].ToString() != "TS")
                        {
                            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

                            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                string APINAME = "ManageChecksAPI/CheckList_GetControl";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                                _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                                dsC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
                            }
                            else
                            {
                                dsC = objBL_User.getControl(objPropUser);
                            }
                        }
                        else
                        {
                            objPropUser.LocID = Convert.ToInt32(0);

                            //API
                            _getControlBranch.LocID = Convert.ToInt32(0);

                            List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();
                            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                string APINAME = "ManageChecksAPI/CheckList_GetControlBranch";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getControlBranch);

                                _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                                dsC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
                            }
                            else
                            {
                                dsC = objBL_User.getControlBranch(objPropUser);
                            }
                            // dsC = objBL_User.getControlBranch(objPropUser);
                        }

                        if (dti.Rows.Count > 0)
                        {
                            string _amount = String.Format("{0:c}", Convert.ToDouble(AmountPay));
                            _amount = _amount.Replace("$", string.Empty);
                            _drC = dtpay.NewRow();

                            //if (Convert.ToDouble(ViewState["Amount"]) > 1000)
                            if (AmountPay > 1000)
                            {
                                _drC["Pay"] = ConvertNumberToCurrency(AmountPay);
                                //_drC["Pay"] = ViewState["Dollar"].ToString();
                            }
                            else
                            {
                                string dollar = ConvertNumberToCurrency(AmountPay);
                                _drC["Pay"] = dollar + " Dollars";
                            }
                            _drC["ToOrder"] = dr["Name"].ToString();
                            //_drC["ToOrder"] = ViewState["Vendor"].ToString();
                            _drC["Date"] = txtDate.Text;
                            _drC["CheckAmount"] = AmountPay;
                            _drC["VendorAddress"] = vendAddress;                // change by Mayuri on 8th nov,16
                            _drC["RemitAddress"] = _dsV.Tables[0].Rows[0]["RemitAddress"];                  // change by Mayuri on 8th nov,16
                            _drC["State"] = vendAddress2;
                            dtpay.Rows.Add(_drC);
                            long checkno = 0;
                            //RAHIL'S IMPLEMENTATION
                            //_objCD.ConnConfig = Session["config"].ToString();
                            //if (ddlPayment.SelectedValue == "0")
                            //{
                            //    checkno = Convert.ToInt32(ViewState["Checkno"]) + count;
                            //}
                            //else
                            //{
                            //    checkno = Convert.ToInt32(ViewState["Checkno"]);
                            //}
                            
                                checkno = long.Parse (ViewState["Checkno"].ToString()) + count;
                            

                            _objCD.Ref = checkno;

                            _objBank.ConnConfig = Session["config"].ToString();
                            _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);

                            _getBankCD.ConnConfig = Session["config"].ToString();
                            _getBankCD.ID = Convert.ToInt32(ddlBank.SelectedValue);
                            DataSet _dsB = new DataSet();

                            List<BankViewModel> _lstBankViewModel = new List<BankViewModel>();

                            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                string APINAME = "ManageChecksAPI/AddCheck_GetBankCD";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getBankCD);

                                _lstBankViewModel = (new JavaScriptSerializer()).Deserialize<List<BankViewModel>>(_APIResponse.ResponseData);
                                _dsB = CommonMethods.ToDataSet<BankViewModel>(_lstBankViewModel);
                                _dsB.Tables[0].Columns["RolName"].ColumnName = "Name";
                                _dsB.Tables[0].Columns["RolAddress"].ColumnName = "Address";
                                _dsB.Tables[0].Columns["RolState"].ColumnName = "State";
                                _dsB.Tables[0].Columns["RolCity"].ColumnName = "City";
                                _dsB.Tables[0].Columns["RolZip"].ColumnName = "Zip";
                            }
                            else
                            {
                                _dsB = _objBLBill.GetBankCD(_objBank);
                            }

                            _drB = dtBank.NewRow();
                            if (_dsB.Tables[0].Rows.Count > 0)
                            {
                                _drB["Name"] = _dsB.Tables[0].Rows[0]["Name"].ToString();
                                _drB["Address"] = _dsB.Tables[0].Rows[0]["Address"].ToString();
                                _drB["State"] = _dsB.Tables[0].Rows[0]["State"].ToString();
                                _drB["City"] = _dsB.Tables[0].Rows[0]["City"].ToString();
                                _drB["Zip"] = _dsB.Tables[0].Rows[0]["Zip"].ToString();
                                _drB["NBranch"] = _dsB.Tables[0].Rows[0]["NBranch"].ToString();
                                _drB["NAcct"] = _dsB.Tables[0].Rows[0]["NAcct"].ToString();
                                _drB["NRoute"] = _dsB.Tables[0].Rows[0]["NRoute"].ToString();
                                //_drB["Ref"] = _dsB.Tables[0].Rows[0]["Ref"].ToString();
                                //_dtBank.Rows.Add(_drB);
                            }
                            string checkNumber = string.Empty;
                            if (!string.IsNullOrEmpty(txtNextCheck.Text))
                            {
                                checkNumber = checkno.ToString();
                            }
                            else
                            {
                                checkNumber = checkno.ToString();
                            }

                            if (checkNumber.Length == 1)
                            {
                                _drB["Ref"] = "00000000" + checkNumber;
                            }
                            else if (checkNumber.Length == 2)
                            {
                                _drB["Ref"] = "0000000" + checkNumber;
                            }
                            else if (checkNumber.Length == 3)
                            {
                                _drB["Ref"] = "000000" + checkNumber;
                            }
                            else if (checkNumber.Length == 4)
                            {
                                _drB["Ref"] = "00000" + checkNumber;
                            }
                            else if (checkNumber.Length == 5)
                            {
                                _drB["Ref"] = "0000" + checkNumber;
                            }
                            else if (checkNumber.Length == 6)
                            {
                                _drB["Ref"] = "000" + checkNumber;
                            }
                            else if (checkNumber.Length == 7)
                            {
                                _drB["Ref"] = "00" + checkNumber;
                            }
                            else if (checkNumber.Length == 8)
                            {
                                _drB["Ref"] = "0" + checkNumber;
                            }
                            else
                            {
                                _drB["Ref"] = "000000000";
                            }

                            dtBank.Rows.Add(_drB);

                            _objVendor.ConnConfig = Session["config"].ToString();
                            _objVendor.ID = vid;

                            _getVendorAcct.ConnConfig = Session["config"].ToString();
                            _getVendorAcct.ID = vid;

                            DataSet _dsA = new DataSet();
                            List <GetVendorAcctList> _lstGetVendorAcct = new List<GetVendorAcctList>();

                            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                string APINAME = "ManageChecksAPI/AddCheck_GetVendorAcct";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorAcct);

                                _lstGetVendorAcct = (new JavaScriptSerializer()).Deserialize<List<GetVendorAcctList>>(_APIResponse.ResponseData);
                                _dsA = CommonMethods.ToDataSet<GetVendorAcctList>(_lstGetVendorAcct);
                                _dsA.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                            }
                            else
                            {
                                _dsA = _objBLVendor.GetVendorAcct(_objVendor);
                            }

                            _drA = _dtAcct.NewRow();
                            _drA["VendorID"] = _dsA.Tables[0].Rows[0]["ID"].ToString();
                            _drA["VendorAcct"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
                            _dtAcct.Rows.Add(_drA);
                            //-----------------------------------------------

                            var rowCount = 0;
                            var totalRows = dti.Rows.Count;
                            if (reportName.Contains("-"))
                            {
                                try
                                {
                                    string[] reportNameArr = reportName.Split('-');
                                    rowCount = Convert.ToInt32(reportNameArr[1].ToString().Trim().TrimStart());
                                    if (totalRows < rowCount)
                                        rowCount = totalRows;
                                }
                                catch (Exception ex) { rowCount = totalRows; }
                            }
                            else
                                rowCount = 6;
                            var dtiCopy = dti.Copy();
                            DataView dv = dtiCopy.DefaultView;
                            dv.Sort = "Ref asc";
                            DataTable sortedDT = dv.ToTable();
                            var dtCopy = sortedDT.Copy();
                            var firstHalf = dtCopy;
                            var secondHalf = dtCopy;
                            if (dtCopy.Rows.Count > rowCount)
                            {
                                firstHalf = dtCopy.AsEnumerable().Take(rowCount).CopyToDataTable();
                                secondHalf = dtCopy.Clone();
                                if (totalRows > rowCount)
                                {
                                    secondHalf = dtCopy.AsEnumerable().Skip(rowCount).Take(totalRows - rowCount).CopyToDataTable();
                                }
                            }
                            else
                            {
                                firstHalf = dtCopy;
                            }

                            DataTable _dti = new DataTable();

                            

                            //STIMULSOFT 
                            byte[] buffer1 = null;
                            string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + reportName.Trim() + ".mrt");
                            StiReport report = new StiReport();
                            report.Load(reportPathStimul);
                            report.Compile();
                            report["TotalAmountPay"] = AmountPay;
                            report["Memo"] = txtMemo.Text;

                            report["InvoiceCount"] = totalRows;
                            DataSet Invoice = new DataSet();
                            DataTable dtInvoice = firstHalf.Copy();
                            dtInvoice.TableName = "Invoice";
                            Invoice.Tables.Add(dtInvoice);
                            Invoice.DataSetName = "Invoice";

                            DataSet Check = new DataSet();
                            DataTable dtCheck = dtpay.Copy();
                            dtCheck.TableName = "Check";
                            Check.Tables.Add(dtCheck);
                            Check.DataSetName = "Check";

                            DataSet ControlBranch = new DataSet();
                            DataTable dtControlBranch = new DataTable();
                            dtControlBranch = dsC.Tables[0].Copy();
                            ControlBranch.Tables.Add(dtControlBranch);
                            dtControlBranch.TableName = "ControlBranch";
                            ControlBranch.DataSetName = "ControlBranch";

                            DataSet Bank = new DataSet();
                            DataTable _dtBank = new DataTable();
                            _dtBank = dtBank.Copy();
                            dtBank.TableName = "Bank";
                            Bank.Tables.Add(_dtBank);
                            Bank.DataSetName = "Bank";

                            DataSet Account = new DataSet();
                            DataTable dtAccount = new DataTable();
                            dtAccount =_dtAcct.Copy();
                            _dtAcct.TableName = "Account";
                            Account.Tables.Add(dtAccount);
                            Account.DataSetName = "Account";


                            report.RegData("Invoice", Invoice);
                            report.RegData("Check", Check);
                            report.RegData("ControlBranch", ControlBranch);
                            report.RegData("dsBank", Bank);
                            report.RegData("dsAccount", Account);

                            report.Render();
                            var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                            var service = new Stimulsoft.Report.Export.StiPdfExportService();
                            System.IO.MemoryStream stream = new System.IO.MemoryStream();
                            service.ExportTo(report, stream, settings);
                            buffer1 = stream.ToArray();
                            lstbyte.Add(buffer1);




                            if (totalRows > rowCount)
                            {
                                byte[] bufferNew = null;
                                reportPathStimul = Server.MapPath("StimulsoftReports/TopCheckSubReport.mrt");
                                report = new StiReport();
                                report.Load(reportPathStimul);
                                report.Compile();
                                report["TotalAmountPay"] = AmountPay;
                                report["AccountNo"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
                                report["Memo"] = txtMemo.Text;

                                report["InvoiceCount"] = totalRows;
                                Invoice = new DataSet();
                                dtInvoice = secondHalf.Copy();
                                dtInvoice.TableName = "Invoice";
                                Invoice.Tables.Add(dtInvoice);
                                Invoice.DataSetName = "Invoice";

                                Check = new DataSet();
                                dtCheck = dtpay.Copy();
                                dtCheck.TableName = "Check";
                                Check.Tables.Add(dtCheck);
                                Check.DataSetName = "Check";

                                //DataSet ControlBranch = new DataSet();
                                //DataTable dtControlBranch = new DataTable();
                                //dtControlBranch = dsC.Tables[0].Copy();
                                //ControlBranch.Tables.Add(dtControlBranch);
                                //dtControlBranch.TableName = "ControlBranch";
                                //ControlBranch.DataSetName = "ControlBranch";




                                report.RegData("Invoice", Invoice);
                                report.RegData("Check", Check);
                                report.Render();
                                settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                                service = new Stimulsoft.Report.Export.StiPdfExportService();
                                stream = new System.IO.MemoryStream();
                                service.ExportTo(report, stream, settings);
                                bufferNew = stream.ToArray();

                                lstbyteNew.Add(bufferNew);
                            }

                        }
                        count++;
                    }

                    _dtAcct.Reset();
                    dti.Reset();
                    dtpay.Reset();
                    dtBank.Reset();

                }
                byte[] finalbyte = null;

                if (lstbyteNew.Count != 0)
                {
                    finalbyte = WriteChecks.concatAndAddContentFinal(lstbyte, lstbyteNew);
                }
                else
                {
                    finalbyte = WriteChecks.concatAndAddContent(lstbyte);
                }
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Buffer = true;
                Response.AddHeader("Content-Disposition", "attachment;filename=ApTopCheckCub.pdf");
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Length", (finalbyte.Length).ToString());
                Response.BinaryWrite(finalbyte);
            }




            else
            {

                CreateTableInvoice();
                CreateTablePayee();
                CreateTableBank();
                DataTable _dtAcct = new DataTable();
                _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
                _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

                DataRow _dri = null;
                DataRow _drC = null;
                int vid = Convert.ToInt32(ddlVendor.SelectedValue);
                //RAHIL'S IMPLEMENTATION
                DataRow _drB = null;
                DataRow _drA = null;

                //foreach (GridViewRow gr in gvBills.Rows)
                foreach (GridDataItem gr in gvBills.Items)
                {
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

                    if (chkSelect.Checked == true)
                    {
                        //TextBox test = (TextBox)gr.FindControl("VendorAcct");
                        TextBox txtGvPay = (TextBox)gr.FindControl("txtGvPay");
                        Label lblBalance = (Label)gr.FindControl("lblBalance");
                        TextBox txtGvDisc = (TextBox)gr.FindControl("txtGvDisc");
                        Label lblOrig = (Label)gr.FindControl("lblOrig");
                        HiddenField hdnRef = (HiddenField)gr.FindControl("hdnRef");

                        Label lblBillfdesc = (Label)gr.FindControl("lblBillfdesc");
                        TextBox txtGvDesc = (TextBox)gr.FindControl("txtGvDesc");
                        Label lblfDate = (Label)gr.FindControl("lblfDate");
                        Label lblRef = (Label)gr.FindControl("lblRef");
                            _dri = dti.NewRow();
                            _dri["Ref"] = hdnRef.Value;
                        //_dri["Description"] = lblBillfdesc.Text;
                        _dri["Description"] = txtGvDesc.Text;
                        _dri["InvoiceDate"] = lblfDate.Text;
                            _dri["Reference"] = lblRef.Text;
                            _dri["Total"] = double.Parse(lblOrig.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                          NumberStyles.AllowThousands |
                                          NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                            _dri["Disc"] = Convert.ToDouble(txtGvDisc.Text).ToString();
                            _dri["AmountPay"] = Convert.ToDouble(txtGvPay.Text).ToString();
                            SumAmountpay = SumAmountpay + Convert.ToDouble(txtGvPay.Text);
                            _dri["PayDate"] = txtDate.Text;
                        //_dri["CheckNo"] = ViewState["Checkno"].ToString();
                        if (!string.IsNullOrEmpty(txtNextCheck.Text))
                        {
                            _dri["CheckNo"] = txtNextCheck.Text;

                        }
                        else
                        {
                            _dri["CheckNo"] = ViewState["Checkno"].ToString();

                        }
                        
                        _dri["VendorID"] = Convert.ToInt32(ddlVendor.SelectedValue);
                            _dri["VendorName"] = ddlVendor.SelectedItem.Text;
                            dti.Rows.Add(_dri);
                        
                    }
                }
                

                _objVendor.ConnConfig = Session["config"].ToString();
                _objVendor.ID = vid;

                _getVendorRolDetails.ConnConfig = Session["config"].ToString();
                _getVendorRolDetails.ID = vid;

                DataSet _dsV = new DataSet();
                List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/CheckList_GetVendorRolDetails";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorRolDetails);

                    _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
                    _dsV = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
                    _dsV.Tables[0].Columns.Remove("Type");
                    _dsV.Tables[0].Columns["VType"].ColumnName = "Type";
                    _dsV.Tables[0].Columns["Vendor1099"].ColumnName = "1099";
                    _dsV.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                    //_dsV.Tables[0].Columns["Remit"].ColumnName = "RemitAddress";
                }
                else
                {
                    _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
                }

                //string _amount = String.Format("{0:c}", Convert.ToDouble(ViewState["Amount"]));
                string _amount = String.Format("{0:c}", Convert.ToDouble(SumAmountpay));
                _amount = _amount.Replace("$", string.Empty);
                _drC = dtpay.NewRow();

                //if (Convert.ToDouble(ViewState["Amount"]) > 1000)
                if (Convert.ToDouble(SumAmountpay) > 1000)
                {
                    //_drC["Pay"] = ViewState["Dollar"].ToString();
                    _drC["Pay"] = ConvertNumberToCurrency(SumAmountpay);
                }
                else
                {
                    //string dollar = ViewState["Dollar"].ToString();
                    string dollar = ConvertNumberToCurrency(SumAmountpay);
                    _drC["Pay"] = dollar + " Dollars";
                }

                _drC["ToOrder"] = ViewState["Vendor"].ToString();
                _drC["Date"] = txtDate.Text;
                _drC["CheckAmount"] = SumAmountpay;
                _drC["VendorAddress"] = _dsV.Tables[0].Rows[0]["VendorAddress"];                // change by Mayuri on 8th nov,16
                _drC["RemitAddress"] = _dsV.Tables[0].Rows[0]["RemitAddress"];
                _drC["VendorAcct"] = _dsV.Tables[0].Rows[0]["Acct#"].ToString();// change by Mayuri on 8th nov,16
                dtpay.Rows.Add(_drC);

                //RAHIL'S IMPLEMENTATION
                //_objCD.ConnConfig = Session["config"].ToString();
                long checkno = long.Parse (ViewState["Checkno"].ToString());
                _objCD.Ref = checkno;

                _objBank.ConnConfig = Session["config"].ToString();
                _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);

                _getBankCD.ConnConfig = Session["config"].ToString();
                _getBankCD.ID = Convert.ToInt32(ddlBank.SelectedValue);
                DataSet _dsB = new DataSet();

                List<BankViewModel> _lstBankViewModel = new List<BankViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/AddCheck_GetBankCD";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getBankCD);

                    _lstBankViewModel = (new JavaScriptSerializer()).Deserialize<List<BankViewModel>>(_APIResponse.ResponseData);
                    _dsB = CommonMethods.ToDataSet<BankViewModel>(_lstBankViewModel);
                    _dsB.Tables[0].Columns["RolName"].ColumnName = "Name";
                    _dsB.Tables[0].Columns["RolAddress"].ColumnName = "Address";
                    _dsB.Tables[0].Columns["RolState"].ColumnName = "State";
                    _dsB.Tables[0].Columns["RolCity"].ColumnName = "City";
                    _dsB.Tables[0].Columns["RolZip"].ColumnName = "Zip";
                }
                else
                {
                    _dsB = _objBLBill.GetBankCD(_objBank);
                }

                _drB = dtBank.NewRow();
                if (_dsB.Tables[0].Rows.Count > 0)
                {
                    _drB["Name"] = _dsB.Tables[0].Rows[0]["Name"].ToString();
                    _drB["Address"] = _dsB.Tables[0].Rows[0]["Address"].ToString();
                    _drB["State"] = _dsB.Tables[0].Rows[0]["State"].ToString();
                    _drB["City"] = _dsB.Tables[0].Rows[0]["City"].ToString();
                    _drB["Zip"] = _dsB.Tables[0].Rows[0]["Zip"].ToString();
                    _drB["NBranch"] = _dsB.Tables[0].Rows[0]["NBranch"].ToString();
                    _drB["NAcct"] = _dsB.Tables[0].Rows[0]["NAcct"].ToString();
                    _drB["NRoute"] = _dsB.Tables[0].Rows[0]["NRoute"].ToString();
                    //_drB["Ref"] = _dsB.Tables[0].Rows[0]["Ref"].ToString();
                    //_dtBank.Rows.Add(_drB);
                }
                string checkNumber = string.Empty;
                if (!string.IsNullOrEmpty(txtNextCheck.Text))
                {
                    checkNumber = txtNextCheck.Text;
                }
                else
                {
                    checkNumber = ViewState["Checkno"].ToString();
                }

                if (checkNumber.Length == 1)
                {
                    _drB["Ref"] = "00000000" + checkNumber;
                }
                else if (checkNumber.Length == 2)
                {
                    _drB["Ref"] = "0000000" + checkNumber;
                }
                else if (checkNumber.Length == 3)
                {
                    _drB["Ref"] = "000000" + checkNumber;
                }
                else if (checkNumber.Length == 4)
                {
                    _drB["Ref"] = "00000" + checkNumber;
                }
                else if (checkNumber.Length == 5)
                {
                    _drB["Ref"] = "0000" + checkNumber;
                }
                else if (checkNumber.Length == 6)
                {
                    _drB["Ref"] = "000" + checkNumber;
                }
                else if (checkNumber.Length == 7)
                {
                    _drB["Ref"] = "00" + checkNumber;
                }
                else if (checkNumber.Length == 8)
                {
                    _drB["Ref"] = "0" + checkNumber;
                }
                else
                {
                    _drB["Ref"] = "000000000";
                }

                dtBank.Rows.Add(_drB);

                _objVendor.ConnConfig = Session["config"].ToString();
                _objVendor.ID = vid;
                _getVendorAcct.ConnConfig = Session["config"].ToString();
                _getVendorAcct.ID = vid;


                DataSet _dsA = new DataSet();
                List<GetVendorAcctList> _lstGetVendorAcct = new List<GetVendorAcctList>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/AddCheck_GetVendorAcct";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorAcct);

                    _lstGetVendorAcct = (new JavaScriptSerializer()).Deserialize<List<GetVendorAcctList>>(_APIResponse.ResponseData);
                    _dsA = CommonMethods.ToDataSet<GetVendorAcctList>(_lstGetVendorAcct);
                    _dsA.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                }
                else
                {
                    _dsA = _objBLVendor.GetVendorAcct(_objVendor);
                }

                _drA = _dtAcct.NewRow();
                _drA["VendorID"] = _dsA.Tables[0].Rows[0]["ID"].ToString();
                _drA["VendorAcct"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
                _dtAcct.Rows.Add(_drA);
                //-----------------------------------------------
                var rowCount = 0;
                var totalRows = dti.Rows.Count;
                if (reportName.Contains("-"))
                {
                    try
                    {
                        string[] reportNameArr = reportName.Split('-');
                        rowCount = Convert.ToInt32(reportNameArr[1].ToString().Trim().TrimStart());
                        if (totalRows < rowCount)
                            rowCount = totalRows;
                    }
                    catch (Exception ex) { rowCount = totalRows; }
                }
                else
                    rowCount = 6;
                var dtiCopy = dti.Copy();
                DataView dv = dtiCopy.DefaultView;
                dv.Sort = "Ref asc";
                DataTable sortedDT = dv.ToTable();
                var dtCopy = sortedDT.Copy();
                var firstHalf = dtCopy;
                var secondHalf = dtCopy;
                if (dtCopy.Rows.Count > rowCount)
                {
                    firstHalf = dtCopy.AsEnumerable().Take(rowCount).CopyToDataTable();
                    secondHalf = dtCopy.Clone();
                    if (totalRows > rowCount)
                    {
                        secondHalf = dtCopy.AsEnumerable().Skip(rowCount).Take(totalRows - rowCount).CopyToDataTable();
                    }
                }
                else
                {
                    firstHalf = dtCopy;
                }

                DataTable _dti = new DataTable();

                DataSet dsC = new DataSet();

                objPropUser.ConnConfig = Session["config"].ToString();

                //API
                _getConnectionConfig.ConnConfig = Session["config"].ToString();

                if (Session["MSM"].ToString() != "TS")
                {
                    List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_GetControl";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                        _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                        dsC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
                    }
                    else
                    {
                        dsC = objBL_User.getControl(objPropUser);
                    }
                }
                else
                {
                    objPropUser.LocID = Convert.ToInt32(0);

                    //API
                    _getControlBranch.LocID = Convert.ToInt32(0);

                    List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();
                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_GetControlBranch";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getControlBranch);

                        _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                        dsC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
                    }
                    else
                    {
                        dsC = objBL_User.getControlBranch(objPropUser);
                    }
                    // dsC = objBL_User.getControlBranch(objPropUser);
                }

                //STIMULSOFT 
                byte[] buffer1 = null;
                string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + reportName.Trim() + ".mrt");
                StiReport report = new StiReport();
                report.Load(reportPathStimul);
                report.Compile();
                report["TotalAmountPay"] = SumAmountpay;
                report["Memo"] = txtMemo.Text;

                report["InvoiceCount"] = totalRows;
                DataSet Invoice = new DataSet();
                DataTable dtInvoice = firstHalf.Copy();
                dtInvoice.TableName = "Invoice";
                Invoice.Tables.Add(dtInvoice);
                Invoice.DataSetName = "Invoice";

                DataSet Check = new DataSet();
                DataTable dtCheck = dtpay.Copy();
                dtCheck.TableName = "Check";
                Check.Tables.Add(dtCheck);
                Check.DataSetName = "Check";

                DataSet ControlBranch = new DataSet();
                DataTable dtControlBranch = new DataTable();
                dtControlBranch = dsC.Tables[0].Copy();
                ControlBranch.Tables.Add(dtControlBranch);
                dtControlBranch.TableName = "ControlBranch";
                ControlBranch.DataSetName = "ControlBranch";

                DataSet Bank = new DataSet();
                DataTable _dtBank = dtBank;
                dtBank.TableName = "Bank";
                Bank.Tables.Add(dtBank);
                Bank.DataSetName = "Bank";

                DataSet Account = new DataSet();
                DataTable dtAccount = _dtAcct;
                _dtAcct.TableName = "Account";
                Account.Tables.Add(_dtAcct);
                Account.DataSetName = "Account";
                                              

                report.RegData("Invoice", Invoice);
                report.RegData("Check", Check);
                report.RegData("ControlBranch", ControlBranch);
                report.RegData("dsBank", Bank);
                report.RegData("dsAccount", Account);

                report.Render();
                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(report, stream, settings);
                buffer1 = stream.ToArray();
                lstbyte.Add(buffer1);




                if (totalRows > rowCount)
                {
                    byte[] bufferNew = null;
                    reportPathStimul = Server.MapPath("StimulsoftReports/TopCheckSubReport.mrt");
                    report = new StiReport();
                    report.Load(reportPathStimul);
                    report.Compile();
                    report["TotalAmountPay"] = SumAmountpay;
                    report["AccountNo"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
                    report["Memo"] = txtMemo.Text;
                    report["InvoiceCount"] = totalRows;
                    Invoice = new DataSet();
                    dtInvoice = secondHalf.Copy();
                    dtInvoice.TableName = "Invoice";
                    Invoice.Tables.Add(dtInvoice);
                    Invoice.DataSetName = "Invoice";

                    Check = new DataSet();
                    dtCheck = dtpay.Copy();
                    dtCheck.TableName = "Check";
                    Check.Tables.Add(dtCheck);
                    Check.DataSetName = "Check";

                    //DataSet ControlBranch = new DataSet();
                    //DataTable dtControlBranch = new DataTable();
                    //dtControlBranch = dsC.Tables[0].Copy();
                    //ControlBranch.Tables.Add(dtControlBranch);
                    //dtControlBranch.TableName = "ControlBranch";
                    //ControlBranch.DataSetName = "ControlBranch";




                    report.RegData("Invoice", Invoice);
                    report.RegData("Check", Check);
                    report.Render();
                    settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                    service = new Stimulsoft.Report.Export.StiPdfExportService();
                    stream = new System.IO.MemoryStream();
                    service.ExportTo(report, stream, settings);
                    bufferNew = stream.ToArray();

                    lstbyteNew.Add(bufferNew);
                }
                byte[] finalbyte = null;

                if (lstbyteNew.Count != 0)
                {
                    finalbyte = WriteChecks.concatAndAddContentFinal(lstbyte, lstbyteNew);
                }
                else
                {
                    finalbyte = WriteChecks.concatAndAddContent(lstbyte);
                }
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Buffer = true;
                Response.AddHeader("Content-Disposition", "attachment;filename=ApTopCheckCub.pdf");
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Length", (finalbyte.Length).ToString());
                Response.BinaryWrite(finalbyte);
            }



        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillReportMiddleDataSet_New(string reportName)
    {
        try
        {
            double SumAmountpay = 0.00;
            int count = 0;
            _objCD.ConnConfig = Session["config"].ToString();
            _objCD.Ref = long.Parse(txtNextCheck.Text);
            _objCD.NextC = long.Parse(txtNextCheck.Text);
            _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);

            _getCheckDetailsByBankAndRef.ConnConfig = Session["config"].ToString();
            _getCheckDetailsByBankAndRef.Ref = long.Parse(txtNextCheck.Text);
            _getCheckDetailsByBankAndRef.NextC = long.Parse(txtNextCheck.Text);
            _getCheckDetailsByBankAndRef.Bank = Convert.ToInt32(ddlBank.SelectedValue);

            DataSet _dsCheck = new DataSet();
            DataSet _dsCheck1 = new DataSet();
            DataSet _dsCheck2 = new DataSet();
            ListCheckDetailsByBankAndRef _ListCheckDetailsByBankAndRef = new ListCheckDetailsByBankAndRef();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetCheckDetailsByBankAndRef";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCheckDetailsByBankAndRef);

                _ListCheckDetailsByBankAndRef = (new JavaScriptSerializer()).Deserialize<ListCheckDetailsByBankAndRef>(_APIResponse.ResponseData);

                _dsCheck1 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable1.ToDataSet();
                _dsCheck2 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable2.ToDataSet();

                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                dt1 = _dsCheck1.Tables[0];
                dt2 = _dsCheck2.Tables[0];

                dt1.TableName = "Table1";
                dt2.TableName = "Table2";

                _dsCheck.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });
            }
            else
            {
                _dsCheck = _objBLBill.GetCheckDetailsByBankAndRef(_objCD);
            }
            DataSet ControlBranch = new DataSet();
            DataTable dtControlBranch = new DataTable();
            dtControlBranch = _dsCheck.Tables[2].Copy();
            ControlBranch.Tables.Add(dtControlBranch);
            dtControlBranch.TableName = "ControlBranch";
            ControlBranch.DataSetName = "ControlBranch";

            DataSet Account = new DataSet();
            DataTable dtAccount = new DataTable();
            dtAccount = _dsCheck.Tables[3].Copy();
            Account.Tables.Add(dtAccount);
            dtAccount.TableName = "Account";
            Account.DataSetName = "Account";

            DataSet Bank = new DataSet();
            DataTable dtBank = new DataTable();
            dtBank = _dsCheck.Tables[4].Copy();
            Bank.Tables.Add(dtBank);
            dtBank.TableName = "Bank";
            Bank.DataSetName = "Bank";


            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("Name");
            dtNew.Columns.Add("Vendor");
            dtNew.Columns.Add("CheckNo");

            //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            ////if (IsAPIIntegrationEnable == "YES")
            //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            //{
            //    foreach (DataRow drow in _dsCheck1.Tables[0].Rows)
            //    {
            //        DataRow drNew = dtNew.NewRow();
            //        drNew["Name"] = drow["VendorName"].ToString();
            //        drNew["Vendor"] = drow["Vendor"].ToString();
            //        drNew["CheckNo"] = drow["CheckNo"].ToString();
            //        dtNew.Rows.Add(drNew);
            //    }
            //}
            //else
            //{
                foreach (DataRow drow in _dsCheck.Tables[0].Rows)
                {
                    DataRow drNew = dtNew.NewRow();
                    drNew["Name"] = drow["VendorName"].ToString();
                    drNew["Vendor"] = drow["Vendor"].ToString();
                    drNew["CheckNo"] = drow["CheckNo"].ToString();
                    dtNew.Rows.Add(drNew);
                }
            //}

            DataTable dtN = dtNew.DefaultView.ToTable(true);
            DataTable _dtAcct = new DataTable();
            foreach (DataRow dr in dtN.Rows)
            {
                bool isChecked = true;
                _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
                _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

                CreateTableInvoice();
                CreateTablePayee();
                CreateTableBank();


                DataRow _dri = null;
                DataRow _drC = null;

                int vid = Convert.ToInt32(dr["Vendor"].ToString());
                string checkNo = Convert.ToString(dr["CheckNo"].ToString());
                //ac     int vid = Convert.ToInt32(ddlVendor.SelectedValue);
                //RAHIL'S IMPLEMENTATION
                DataRow _drB = null;
                DataRow _drA = null;
                double AmountPay = 0.00;
                SumAmountpay = 0.00;



                //DataTable dtInvoice = _dsCheck.Tables[0].DefaultView.ToTable(true);
                DataView dtInv = new DataView();
                //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                ////if (IsAPIIntegrationEnable == "YES")
                //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                //{
                //    dtInv = _dsCheck1.Tables[0].DefaultView;
                //}
                //else
                //{
                    dtInv = _dsCheck.Tables[0].DefaultView;
                //}

                dtInv.RowFilter = "Vendor = '" + vid + "' and CheckNo = '" + checkNo + "'";
                foreach (DataRow drow in dtInv.ToTable(true).Rows)
                {
                    _dri = dti.NewRow();
                    _dri["Ref"] = drow["Ref"].ToString();
                    _dri["Description"] = drow["Description"].ToString();
                    _dri["InvoiceDate"] = drow["InvoiceDate"].ToString();
                    _dri["Reference"] = drow["Refrerence"].ToString();
                    _dri["Total"] = double.Parse(drow["Total"].ToString().Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                    _dri["Disc"] = Convert.ToDouble(drow["Disc"].ToString()).ToString();
                    _dri["AmountPay"] = Convert.ToDouble(drow["AmountPay"].ToString()).ToString();
                    AmountPay = AmountPay + Convert.ToDouble(drow["AmountPay"].ToString());
                    _dri["PayDate"] = drow["PayDate"].ToString();
                    _dri["CheckNo"] = drow["CheckNo"].ToString();


                    //_dri["VendorID"] = Convert.ToInt32(ddlVendor.SelectedValue);
                    _dri["VendorID"] = drow["Vendor"].ToString();
                    //ac _dri["VendorName"] = ddlVendor.SelectedItem.Text;
                    _dri["VendorName"] = drow["VendorName"].ToString();
                    dti.Rows.Add(_dri);

                    dti.AcceptChanges();

                }
                if (isChecked)
                {
                    if (dti.Rows.Count > 0)
                    {
                        DataView dtcheck = new DataView();
                        //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                        ////if (IsAPIIntegrationEnable == "YES")
                        //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        //{
                        //    dtcheck = _dsCheck2.Tables[0].DefaultView;
                        //}
                        //else
                        //{
                            dtcheck = _dsCheck.Tables[1].DefaultView;
                        //}

                        dtcheck.RowFilter = "Vendor = '" + vid + "' and CheckNo = '" + checkNo + "'";
                        ViewState["CheckStatus"] = "0";
                        foreach (DataRow drow in dtcheck.ToTable(true).Rows)
                        //foreach (DataRow drow in _dsCheck.Tables[1].Rows)
                        {
                            _drC = dtpay.NewRow();
                            if (Convert.ToDouble(drow["Pay"]) > 1000)
                            {
                                _drC["Pay"] = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                                //_drC["Pay"] = ViewState["Dollar"].ToString();
                            }
                            else
                            {
                                string dollar = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                                _drC["Pay"] = dollar + " Dollars";
                            }
                            _drC["ToOrder"] = drow["ToOrder"].ToString();
                            //_drC["ToOrder"] = ViewState["Vendor"].ToString();
                            _drC["Date"] = drow["Date"].ToString();
                            _drC["CheckAmount"] = Convert.ToDouble(drow["Pay"]);
                            _drC["VendorAddress"] = drow["VendorAddress"].ToString();                 // change by Mayuri on 8th nov,16
                            _drC["RemitAddress"] = drow["RemitAddress"].ToString();                  // change by Mayuri on 8th nov,16
                            _drC["State"] = drow["State"].ToString();
                            ViewState["CheckStatus"] = drow["Status"].ToString();
                            dtpay.Rows.Add(_drC);
                        }

                        var rowCount = 0;
                        var totalRows = dti.Rows.Count;
                        if (reportName.Contains("-"))
                        {
                            try
                            {
                                string[] reportNameArr = reportName.Split('-');
                                rowCount = Convert.ToInt32(reportNameArr[1].ToString().Trim().TrimStart());
                                if (totalRows < rowCount)
                                    rowCount = totalRows;
                            }
                            catch (Exception ex) { rowCount = totalRows; }
                        }
                        else
                            rowCount = 6;
                        var dtiCopy = dti.Copy();
                        DataView dv = dtiCopy.DefaultView;
                        dv.Sort = "Ref asc";
                        DataTable sortedDT = dv.ToTable();
                        var dtCopy = sortedDT.Copy();
                        var firstHalf = dtCopy;
                        var secondHalf = dtCopy;
                        if (dtCopy.Rows.Count > rowCount)
                        {
                            firstHalf = dtCopy.AsEnumerable().Take(rowCount).CopyToDataTable();
                            secondHalf = dtCopy.Clone();
                            if (totalRows > rowCount)
                            {
                                secondHalf = dtCopy.AsEnumerable().Skip(rowCount).Take(totalRows - rowCount).CopyToDataTable();
                            }
                        }
                        else
                        {
                            firstHalf = dtCopy;
                        }

                        //STIMULSOFT 
                        byte[] buffer1 = null;
                        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + reportName.Trim() + ".mrt");
                        StiReport report = new StiReport();
                        report.Load(reportPathStimul);
                        if (Convert.ToInt16(ViewState["CheckStatus"]).Equals(2))
                        {
                            report.Pages[0].Watermark.Enabled = true;
                            //report.Pages[0].Watermark.Angle = 0;
                            //report.Pages[0].Watermark.Text = "Void";
                            string imagepath = Server.MapPath("images/icons/voidcheck.png");
                            report.Pages[0].Watermark.Image = System.Drawing.Image.FromFile(imagepath);
                            report.Pages[0].Watermark.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                            report.Pages[0].Watermark.ShowImageBehind = true;
                        }
                        report.Compile();
                        report["TotalAmountPay"] = AmountPay;

                        //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                        ////if (IsAPIIntegrationEnable == "YES")
                        //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        //{
                        //    report["Memo"] = Convert.ToString(_dsCheck2.Tables[0].Rows[0]["Memo"].ToString());
                        //}
                        //else
                        //{
                            report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
                        //}

                        report["InvoiceCount"] = totalRows;
                        DataSet Invoice = new DataSet();
                        DataTable dtInvoice = firstHalf.Copy();
                        dtInvoice.TableName = "Invoice";
                        Invoice.Tables.Add(dtInvoice);
                        Invoice.DataSetName = "Invoice";

                        DataSet Check = new DataSet();
                        DataTable dtCheck = dtpay.Copy();
                        dtCheck.TableName = "Check";
                        Check.Tables.Add(dtCheck);
                        Check.DataSetName = "Check";
                                                
                        report.RegData("Invoice", Invoice);
                        report.RegData("Check", Check);
                        report.RegData("ControlBranch", ControlBranch);
                        report.RegData("dsBank", Bank);
                        report.RegData("dsAccount", Account);
                        report.Render();
                        var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                        var service = new Stimulsoft.Report.Export.StiPdfExportService();
                        System.IO.MemoryStream stream = new System.IO.MemoryStream();
                        service.ExportTo(report, stream, settings);
                        buffer1 = stream.ToArray();
                        lstbyte.Add(buffer1);




                        if (totalRows > rowCount)
                        {
                            byte[] bufferNew = null;
                            reportPathStimul = Server.MapPath("StimulsoftReports/TopCheckSubReport.mrt");                            
                            report = new StiReport();
                            report.Load(reportPathStimul);
                            report.Compile();

                            //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                            ////if (IsAPIIntegrationEnable == "YES")
                            //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            //{
                            //    report["TotalAmountPay"] = Convert.ToDouble(_dsCheck1.Tables[0].Rows[0]["AmountPay"].ToString());
                            //    report["AccountNo"] = "";
                            //    report["Memo"] = Convert.ToString(_dsCheck2.Tables[0].Rows[0]["Memo"].ToString());
                            //}
                            //else
                            //{
                                report["TotalAmountPay"] = Convert.ToDouble(_dsCheck.Tables[0].Rows[0]["AmountPay"].ToString());
                                report["AccountNo"] = "";
                                report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
                            //}

                            report["InvoiceCount"] = totalRows;
                            Invoice = new DataSet();
                            dtInvoice = secondHalf.Copy();
                            dtInvoice.TableName = "Invoice";
                            Invoice.Tables.Add(dtInvoice);
                            Invoice.DataSetName = "Invoice";

                            Check = new DataSet();
                            dtCheck = dtpay.Copy();
                            dtCheck.TableName = "Check";
                            Check.Tables.Add(dtCheck);
                            Check.DataSetName = "Check";

                            //DataSet ControlBranch = new DataSet();
                            //DataTable dtControlBranch = new DataTable();
                            //dtControlBranch = dsC.Tables[0].Copy();
                            //ControlBranch.Tables.Add(dtControlBranch);
                            //dtControlBranch.TableName = "ControlBranch";
                            //ControlBranch.DataSetName = "ControlBranch";




                            report.RegData("Invoice", Invoice);
                            report.RegData("Check", Check);
                            report.Render();
                            settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                            service = new Stimulsoft.Report.Export.StiPdfExportService();
                            stream = new System.IO.MemoryStream();
                            service.ExportTo(report, stream, settings);
                            bufferNew = stream.ToArray();

                            lstbyteNew.Add(bufferNew);
                        }
                    }
                    count++;
                }

                _dtAcct.Reset();
                dti.Reset();
                dtpay.Reset();
                dtBank.Reset();
            }
            byte[] finalbyte = null;

            if (lstbyteNew.Count != 0)
            {
                finalbyte = WriteChecks.concatAndAddContentFinal(lstbyte, lstbyteNew);
            }
            else
            {
                finalbyte = WriteChecks.concatAndAddContent(lstbyte);
            }
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment;filename=MidCheckCub.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (finalbyte.Length).ToString());
            Response.BinaryWrite(finalbyte);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillReportMiddleDataSet(string reportName)
    {
        try
        {
            
            double SumAmountpay = 0.00;
            GetPaymentTotal();

            if (int.Parse(ddlVendor.SelectedValue) <= 0)
            {
                DataTable dt = (DataTable)Session["dsbills"];
                DataTable dtNew = new DataTable();
                dtNew.Columns.Add("Name");
                dtNew.Columns.Add("Vendor");
                foreach (DataRow drow in dt.Rows)
                {
                    DataRow drNew = dtNew.NewRow();
                    drNew["Name"] = drow["Name"].ToString();
                    drNew["Vendor"] = drow["Vendor"].ToString();
                    dtNew.Rows.Add(drNew);
                }

                DataTable dtN = dtNew.DefaultView.ToTable(true);
                DataTable _dtAcct = new DataTable();
                int count = 0;
                //if (!dti.Columns.Contains("VendorAcct"))
                //    dti.Columns.Add("VendorAcct");                
                foreach (DataRow dr in dtN.Rows)
                {
                    bool isChecked = false;
                    CreateTableInvoice();
                    CreateTablePayee();
                    CreateTableBank();
                    //DataTable _dtAcct = new DataTable();
                    _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
                    _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

                    DataRow _dri = null;
                    DataRow _drC = null;

                    int vid = Convert.ToInt32(dr["Vendor"].ToString());
                    //ac     int vid = Convert.ToInt32(ddlVendor.SelectedValue);
                    //RAHIL'S IMPLEMENTATION
                    DataRow _drB = null;
                    DataRow _drA = null;
                    double AmountPay = 0.00;
                    SumAmountpay = 0.00;
                    long checkNo = long.Parse(ViewState["Checkno"].ToString());

                    // foreach (GridViewRow gr in gvBills.Rows)

                    foreach (GridDataItem gr in gvBills.Items)
                    {
                        Label lblVendor = (Label)gr.FindControl("lblVendor");
                        if (lblVendor.Text == dr["Name"].ToString())
                        {
                            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

                            if (chkSelect.Checked == true)
                            {
                                isChecked = true;
                                TextBox txtGvPay = (TextBox)gr.FindControl("txtGvPay");

                                Label lblBalance = (Label)gr.FindControl("lblBalance");
                                TextBox txtGvDisc = (TextBox)gr.FindControl("txtGvDisc");
                                Label lblOrig = (Label)gr.FindControl("lblOrig");
                                HiddenField hdnRef = (HiddenField)gr.FindControl("hdnRef");

                                Label lblBillfdesc = (Label)gr.FindControl("lblBillfdesc");
                                TextBox txtGvDesc = (TextBox)gr.FindControl("txtGvDesc");
                                Label lblfDate = (Label)gr.FindControl("lblfDate");
                                Label lblRef = (Label)gr.FindControl("lblRef");
                                if (!dti.Columns.Contains("VendorAcct"))
                                    dti.Columns.Add("VendorAcct");
                                _dri = dti.NewRow();
                                _dri["Ref"] = hdnRef.Value;
                                //_dri["Description"] = lblBillfdesc.Text;
                                _dri["Description"] = txtGvDesc.Text;
                                _dri["InvoiceDate"] = lblfDate.Text;
                                _dri["Reference"] = lblRef.Text;
                                _dri["Total"] = double.Parse(lblOrig.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                              NumberStyles.AllowThousands |
                                              NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                                _dri["Disc"] = Convert.ToDouble(txtGvDisc.Text).ToString();
                                _dri["AmountPay"] = Convert.ToDouble(txtGvPay.Text).ToString();
                                AmountPay = AmountPay + Convert.ToDouble(txtGvPay.Text);
                                _dri["PayDate"] = txtDate.Text;
                                _dri["CheckNo"] = ViewState["Checkno"].ToString();
                                if (!string.IsNullOrEmpty(txtNextCheck.Text))
                                {
                                    checkNo = long.Parse(txtNextCheck.Text);
                                }
                                else
                                {
                                    checkNo = long.Parse(ViewState["Checkno"].ToString());
                                }
                                //if (ddlPayment.SelectedValue == "0")
                                //{
                                //    _dri["CheckNo"] = checkNo + count;
                                //}
                                //else
                                //{
                                //    _dri["CheckNo"] = checkNo;
                                //}
                                
                                _dri["CheckNo"] = checkNo + count;
                                

                                //_dri["VendorID"] = Convert.ToInt32(ddlVendor.SelectedValue);
                                _dri["VendorID"] = dr["Vendor"];
                                //ac _dri["VendorName"] = ddlVendor.SelectedItem.Text;
                                _dri["VendorName"] = lblVendor.Text;
                                _objVendor.ID = Convert.ToInt32(dr["Vendor"].ToString());
                                _objVendor.ConnConfig = Session["config"].ToString();

                                _getVendorAcct.ConnConfig = Session["config"].ToString();
                                _getVendorAcct.ID = Convert.ToInt32(dr["Vendor"].ToString());

                                DataSet da = new DataSet();
                                List<GetVendorAcctList> _lstGetVendorAcctList = new List<GetVendorAcctList>();

                                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                                //if (IsAPIIntegrationEnable == "YES")
                                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                                {
                                    string APINAME = "ManageChecksAPI/AddCheck_GetVendorAcct";

                                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorAcct);

                                    _lstGetVendorAcctList = (new JavaScriptSerializer()).Deserialize<List<GetVendorAcctList>>(_APIResponse.ResponseData);
                                    da = CommonMethods.ToDataSet<GetVendorAcctList>(_lstGetVendorAcctList);
                                    da.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                                }
                                else
                                {
                                    da = _objBLVendor.GetVendorAcct(_objVendor);
                                }

                                if (da.Tables[0].Rows.Count > 0)
                                {
                                    var acct = da.Tables[0].Rows[0]["Acct#"].ToString();
                                    _dri["VendorAcct"] = acct;
                                }
                                dti.Rows.Add(_dri);


                            }
                        }
                    }
                    while (AmountPay < 0)
                    {
                        List<DataRow> rowsWantToDelete = new List<DataRow>();
                        foreach (DataRow drow in dti.Rows)
                        {
                            if (Convert.ToDouble(drow["AmountPay"].ToString()) < 0)
                            {
                                rowsWantToDelete.Add(drow);
                                foreach (DataRow rows in rowsWantToDelete)
                                {
                                    dti.Rows.Remove(rows);
                                }

                                AmountPay = dti.AsEnumerable().Sum(x => Convert.ToDouble(x["AmountPay"]));
                                break;
                            }
                        }


                    }
                    if (isChecked)
                    {
                        _objVendor.ConnConfig = Session["config"].ToString();
                        _objVendor.ID = vid;

                        _getVendorRolDetails.ConnConfig = Session["config"].ToString();
                        _getVendorRolDetails.ID = vid;

                        DataSet _dsV = new DataSet();
                        List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "ManageChecksAPI/CheckList_GetVendorRolDetails";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorRolDetails);

                            _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
                            _dsV = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
                            _dsV.Tables[0].Columns.Remove("Type");
                            _dsV.Tables[0].Columns["VType"].ColumnName = "Type";
                            _dsV.Tables[0].Columns["Vendor1099"].ColumnName = "1099";
                            _dsV.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                            //_dsV.Tables[0].Columns["Remit"].ColumnName = "RemitAddress";
                        }
                        else
                        {
                            _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
                        }

                        string vendAddress = "";
                        string vendAddress2 = "";
                        if (_dsV.Tables[0].Rows.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
                            {
                                vendAddress = _dsV.Tables[0].Rows[0]["Address"].ToString() + ", ";
                            }

                            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                            {
                                vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
                            }
                            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
                            {
                                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                                {
                                    vendAddress2 += ", ";
                                }
                                vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
                            }
                        }
                        if (dti.Rows.Count > 0)
                        {
                            string _amount = String.Format("{0:c}", Convert.ToDouble(AmountPay));
                            _amount = _amount.Replace("$", string.Empty);
                            _drC = dtpay.NewRow();

                            //if (Convert.ToDouble(ViewState["Amount"]) > 1000)
                            if (AmountPay > 1000)
                            {
                                _drC["Pay"] = ConvertNumberToCurrency(AmountPay);
                                //_drC["Pay"] = ViewState["Dollar"].ToString();
                            }
                            else
                            {
                                string dollar = ConvertNumberToCurrency(AmountPay);
                                _drC["Pay"] = dollar + " Dollars";
                            }
                            _drC["ToOrder"] = dr["Name"].ToString();
                            //_drC["ToOrder"] = ViewState["Vendor"].ToString();
                            _drC["Date"] = txtDate.Text;
                            _drC["CheckAmount"] = AmountPay;
                            _drC["VendorAddress"] = vendAddress;                // change by Mayuri on 8th nov,16
                            _drC["RemitAddress"] = _dsV.Tables[0].Rows[0]["RemitAddress"];                  // change by Mayuri on 8th nov,16
                            _drC["State"] = vendAddress2;
                            dtpay.Rows.Add(_drC);
                            long checkno = 0;
                            //RAHIL'S IMPLEMENTATION
                            //_objCD.ConnConfig = Session["config"].ToString();
                            //if (ddlPayment.SelectedValue == "0")
                            //{
                            //    checkno = Convert.ToInt32(ViewState["Checkno"]) + count;
                            //}
                            //else
                            //{
                            //    checkno = Convert.ToInt32(ViewState["Checkno"]);
                            //}


                            DataSet dsC = new DataSet();

                            objPropUser.ConnConfig = Session["config"].ToString();

                            //API
                            _getConnectionConfig.ConnConfig = Session["config"].ToString();

                            if (Session["MSM"].ToString() != "TS")
                            {
                                List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

                                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                                //if (IsAPIIntegrationEnable == "YES")
                                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                                {
                                    string APINAME = "ManageChecksAPI/CheckList_GetControl";

                                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                                    _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                                    dsC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
                                }
                                else
                                {
                                    dsC = objBL_User.getControl(objPropUser);
                                }
                            }
                            else
                            {
                                objPropUser.LocID = Convert.ToInt32(0);

                                //API
                                _getControlBranch.LocID = Convert.ToInt32(0);

                                List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();
                                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                                //if (IsAPIIntegrationEnable == "YES")
                                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                                {
                                    string APINAME = "ManageChecksAPI/CheckList_GetControlBranch";

                                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getControlBranch);

                                    _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                                    dsC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
                                }
                                else
                                {
                                    dsC = objBL_User.getControlBranch(objPropUser);
                                }
                                // dsC = objBL_User.getControlBranch(objPropUser);
                            }
                            //dsBank



                            checkno = long.Parse (ViewState["Checkno"].ToString()) + count;
                            

                            _objCD.Ref = checkno;

                            _objBank.ConnConfig = Session["config"].ToString();
                            _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);

                            _getBankCD.ConnConfig = Session["config"].ToString();
                            _getBankCD.ID = Convert.ToInt32(ddlBank.SelectedValue);

                            DataSet _dsB = new DataSet();

                            List<BankViewModel> _lstBankViewModel = new List<BankViewModel>();

                            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                string APINAME = "ManageChecksAPI/AddCheck_GetBankCD";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getBankCD);

                                _lstBankViewModel = (new JavaScriptSerializer()).Deserialize<List<BankViewModel>>(_APIResponse.ResponseData);
                                _dsB = CommonMethods.ToDataSet<BankViewModel>(_lstBankViewModel);
                                _dsB.Tables[0].Columns["RolName"].ColumnName = "Name";
                                _dsB.Tables[0].Columns["RolAddress"].ColumnName = "Address";
                                _dsB.Tables[0].Columns["RolState"].ColumnName = "State";
                                _dsB.Tables[0].Columns["RolCity"].ColumnName = "City";
                                _dsB.Tables[0].Columns["RolZip"].ColumnName = "Zip";
                            }
                            else
                            {
                                _dsB = _objBLBill.GetBankCD(_objBank);
                            }

                            _drB = dtBank.NewRow();
                            if (_dsB.Tables[0].Rows.Count > 0)
                            {
                                _drB["Name"] = _dsB.Tables[0].Rows[0]["Name"].ToString();
                                _drB["Address"] = _dsB.Tables[0].Rows[0]["Address"].ToString();
                                _drB["State"] = _dsB.Tables[0].Rows[0]["State"].ToString();
                                _drB["City"] = _dsB.Tables[0].Rows[0]["City"].ToString();
                                _drB["Zip"] = _dsB.Tables[0].Rows[0]["Zip"].ToString();
                                _drB["NBranch"] = _dsB.Tables[0].Rows[0]["NBranch"].ToString();
                                _drB["NAcct"] = _dsB.Tables[0].Rows[0]["NAcct"].ToString();
                                _drB["NRoute"] = _dsB.Tables[0].Rows[0]["NRoute"].ToString();
                                //_drB["Ref"] = _dsB.Tables[0].Rows[0]["Ref"].ToString();
                                //_dtBank.Rows.Add(_drB);
                            }
                            string checkNumber = string.Empty;
                            if (!string.IsNullOrEmpty(txtNextCheck.Text))
                            {
                                checkNumber = checkno.ToString();
                            }
                            else
                            {
                                checkNumber = checkno.ToString();
                            }

                            if (checkNumber.Length == 1)
                            {
                                _drB["Ref"] = "00000000" + checkNumber;
                            }
                            else if (checkNumber.Length == 2)
                            {
                                _drB["Ref"] = "0000000" + checkNumber;
                            }
                            else if (checkNumber.Length == 3)
                            {
                                _drB["Ref"] = "000000" + checkNumber;
                            }
                            else if (checkNumber.Length == 4)
                            {
                                _drB["Ref"] = "00000" + checkNumber;
                            }
                            else if (checkNumber.Length == 5)
                            {
                                _drB["Ref"] = "0000" + checkNumber;
                            }
                            else if (checkNumber.Length == 6)
                            {
                                _drB["Ref"] = "000" + checkNumber;
                            }
                            else if (checkNumber.Length == 7)
                            {
                                _drB["Ref"] = "00" + checkNumber;
                            }
                            else if (checkNumber.Length == 8)
                            {
                                _drB["Ref"] = "0" + checkNumber;
                            }
                            else
                            {
                                _drB["Ref"] = "000000000";
                            }

                            dtBank.Rows.Add(_drB);

                            _objVendor.ConnConfig = Session["config"].ToString();
                            _objVendor.ID = vid;

                            _getVendorAcct.ConnConfig = Session["config"].ToString();
                            _getVendorAcct.ID = vid;

                            DataSet _dsA = new DataSet();
                            List<GetVendorAcctList> _lstGetVendorAcct = new List<GetVendorAcctList>();

                            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                string APINAME = "ManageChecksAPI/AddCheck_GetVendorAcct";

                                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorAcct);

                                _lstGetVendorAcct = (new JavaScriptSerializer()).Deserialize<List<GetVendorAcctList>>(_APIResponse.ResponseData);
                                _dsA = CommonMethods.ToDataSet<GetVendorAcctList>(_lstGetVendorAcct);
                                _dsA.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                            }
                            else
                            {
                                _dsA = _objBLVendor.GetVendorAcct(_objVendor);
                            }

                            _drA = _dtAcct.NewRow();
                            _drA["VendorID"] = _dsA.Tables[0].Rows[0]["ID"].ToString();
                            _drA["VendorAcct"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
                            _dtAcct.Rows.Add(_drA);
                            //-----------------------------------------------

                            var rowCount = 0;
                            var totalRows = dti.Rows.Count;
                            if (reportName.Contains("-"))
                            {
                                try
                                {
                                    string[] reportNameArr = reportName.Split('-');
                                    rowCount = Convert.ToInt32(reportNameArr[1].ToString().Trim().TrimStart());
                                    if (totalRows < rowCount)
                                        rowCount = totalRows;
                                }
                                catch (Exception ex) { rowCount = totalRows; }
                            }
                            else
                                rowCount = 6;
                            var dtiCopy = dti.Copy();
                            DataView dv = dtiCopy.DefaultView;
                            dv.Sort = "Ref asc";
                            DataTable sortedDT = dv.ToTable();
                            var dtCopy = sortedDT.Copy();
                            var firstHalf = dtCopy;
                            var secondHalf = dtCopy;
                            if (dtCopy.Rows.Count > rowCount)
                            {
                                firstHalf = dtCopy.AsEnumerable().Take(rowCount).CopyToDataTable();
                                secondHalf = dtCopy.Clone();
                                if (totalRows > rowCount)
                                {
                                    secondHalf = dtCopy.AsEnumerable().Skip(rowCount).Take(totalRows - rowCount).CopyToDataTable();
                                }
                            }
                            else
                            {
                                firstHalf = dtCopy;
                            }

                            DataTable _dti = new DataTable();

                            

                            //STIMULSOFT 
                            byte[] buffer1 = null;
                            string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + reportName.Trim() + ".mrt");
                            StiReport report = new StiReport();
                            report.Load(reportPathStimul);
                            report.Compile();
                            report["TotalAmountPay"] = AmountPay;
                            report["Memo"] = txtMemo.Text;
                            report["InvoiceCount"] = totalRows;
                            DataSet Invoice = new DataSet();
                            DataTable dtInvoice = firstHalf.Copy();
                            dtInvoice.TableName = "Invoice";
                            Invoice.Tables.Add(dtInvoice);
                            Invoice.DataSetName = "Invoice";

                            DataSet Check = new DataSet();
                            DataTable dtCheck = dtpay.Copy();
                            dtCheck.TableName = "Check";
                            Check.Tables.Add(dtCheck);
                            Check.DataSetName = "Check";

                            DataSet ControlBranch = new DataSet();
                            DataTable dtControlBranch = new DataTable();
                            dtControlBranch = dsC.Tables[0].Copy();
                            ControlBranch.Tables.Add(dtControlBranch);
                            dtControlBranch.TableName = "ControlBranch";
                            ControlBranch.DataSetName = "ControlBranch";


                            //DataSet Bank = new DataSet();
                            //DataTable _dtBank = dtBank;
                            //dtBank.TableName = "Bank";
                            //Bank.Tables.Add(dtBank);
                            //Bank.DataSetName = "Bank";

                            //DataSet Account = new DataSet();
                            //DataTable dtAccount = _dtAcct;
                            //_dtAcct.TableName = "Account";
                            //Account.Tables.Add(_dtAcct);
                            //Account.DataSetName = "Account";

                            DataSet Bank = new DataSet();
                            DataTable _dtBank = new DataTable();
                            _dtBank = dtBank.Copy();
                            dtBank.TableName = "Bank";
                            Bank.Tables.Add(_dtBank);
                            Bank.DataSetName = "Bank";

                            DataSet Account = new DataSet();
                            DataTable dtAccount = new DataTable();
                            dtAccount = _dtAcct.Copy();
                            _dtAcct.TableName = "Account";
                            Account.Tables.Add(dtAccount);
                            Account.DataSetName = "Account";


                            report.RegData("Invoice", Invoice);
                            report.RegData("Check", Check);
                            report.RegData("ControlBranch", ControlBranch);
                            report.RegData("dsBank", Bank);
                            report.RegData("dsAccount", Account);
                            report.Render();
                            var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                            var service = new Stimulsoft.Report.Export.StiPdfExportService();
                            System.IO.MemoryStream stream = new System.IO.MemoryStream();
                            service.ExportTo(report, stream, settings);
                            buffer1 = stream.ToArray();
                            lstbyte.Add(buffer1);




                            if (totalRows > rowCount)
                            {
                                byte[] bufferNew = null;
                                reportPathStimul = Server.MapPath("StimulsoftReports/TopCheckSubReport.mrt");
                                report = new StiReport();
                                report.Load(reportPathStimul);
                                report.Compile();
                                report["TotalAmountPay"] = AmountPay;
                                report["AccountNo"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
                                report["Memo"] = txtMemo.Text;
                                report["InvoiceCount"] = totalRows;
                                Invoice = new DataSet();
                                dtInvoice = secondHalf.Copy();
                                dtInvoice.TableName = "Invoice";
                                Invoice.Tables.Add(dtInvoice);
                                Invoice.DataSetName = "Invoice";

                                Check = new DataSet();
                                dtCheck = dtpay.Copy();
                                dtCheck.TableName = "Check";
                                Check.Tables.Add(dtCheck);
                                Check.DataSetName = "Check";

                                //DataSet ControlBranch = new DataSet();
                                //DataTable dtControlBranch = new DataTable();
                                //dtControlBranch = dsC.Tables[0].Copy();
                                //ControlBranch.Tables.Add(dtControlBranch);
                                //dtControlBranch.TableName = "ControlBranch";
                                //ControlBranch.DataSetName = "ControlBranch";




                                report.RegData("Invoice", Invoice);
                                report.RegData("Check", Check);
                                report.Render();
                                settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                                service = new Stimulsoft.Report.Export.StiPdfExportService();
                                stream = new System.IO.MemoryStream();
                                service.ExportTo(report, stream, settings);
                                bufferNew = stream.ToArray();

                                lstbyteNew.Add(bufferNew);
                            }

                        }
                        count++;
                    }

                    _dtAcct.Reset();
                    dti.Reset();
                    dtpay.Reset();
                    dtBank.Reset();

                }
                byte[] finalbyte = null;

                if (lstbyteNew.Count != 0)
                {
                    finalbyte = WriteChecks.concatAndAddContentFinal(lstbyte, lstbyteNew);
                }
                else
                {
                    finalbyte = WriteChecks.concatAndAddContent(lstbyte);
                }
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Buffer = true;
                Response.AddHeader("Content-Disposition", "attachment;filename=MidCheckCub.pdf");
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Length", (finalbyte.Length).ToString());
                Response.BinaryWrite(finalbyte);
            }




            else
            {

                CreateTableInvoice();
                CreateTablePayee();
                CreateTableBank();
                DataTable _dtAcct = new DataTable();
                _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
                _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

                DataRow _dri = null;
                DataRow _drC = null;
                int vid = Convert.ToInt32(ddlVendor.SelectedValue);
                //RAHIL'S IMPLEMENTATION
                DataRow _drB = null;
                DataRow _drA = null;
                //dti.Columns.Add("VendorAcct");
                //foreach (GridViewRow gr in gvBills.Rows)
                foreach (GridDataItem gr in gvBills.Items)
                {
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

                    if (chkSelect.Checked == true)
                    {
                        TextBox txtGvPay = (TextBox)gr.FindControl("txtGvPay");
                        Label lblBalance = (Label)gr.FindControl("lblBalance");
                        TextBox txtGvDisc = (TextBox)gr.FindControl("txtGvDisc");
                        Label lblOrig = (Label)gr.FindControl("lblOrig");
                        HiddenField hdnRef = (HiddenField)gr.FindControl("hdnRef");
                        Label lblBillfdesc = (Label)gr.FindControl("lblBillfdesc");
                        TextBox txtGvDesc = (TextBox)gr.FindControl("txtGvDesc");
                        Label lblfDate = (Label)gr.FindControl("lblfDate");
                        Label lblRef = (Label)gr.FindControl("lblRef");

                        _dri = dti.NewRow();
                        _dri["Ref"] = hdnRef.Value;
                        //_dri["Description"] = lblBillfdesc.Text;
                        _dri["Description"] = txtGvDesc.Text;
                        _dri["InvoiceDate"] = lblfDate.Text;
                        _dri["Reference"] = lblRef.Text;
                        _dri["Total"] = double.Parse(lblOrig.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                      NumberStyles.AllowThousands |
                                      NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                        _dri["Disc"] = Convert.ToDouble(txtGvDisc.Text).ToString();
                        _dri["AmountPay"] = Convert.ToDouble(txtGvPay.Text).ToString();
                        SumAmountpay = SumAmountpay + Convert.ToDouble(txtGvPay.Text);
                        _dri["PayDate"] = txtDate.Text;
                        //_dri["CheckNo"] = ViewState["Checkno"].ToString();
                        if (!string.IsNullOrEmpty(txtNextCheck.Text))
                        {
                            _dri["CheckNo"] = txtNextCheck.Text;
                        }
                        else
                        {
                            _dri["CheckNo"] = ViewState["Checkno"].ToString();
                        }
                        _dri["VendorID"] = Convert.ToInt32(ddlVendor.SelectedValue);
                        _dri["VendorName"] = ddlVendor.SelectedItem.Text;
                        _objVendor.ID = Convert.ToInt32(ddlVendor.SelectedValue);
                        _objVendor.ConnConfig = Session["config"].ToString();

                        _getVendorAcct.ConnConfig = Session["config"].ToString();
                        _getVendorAcct.ID = Convert.ToInt32(ddlVendor.SelectedValue);

                        DataSet da = new DataSet();
                        List<GetVendorAcctList> _lstGetVendorAcctList = new List<GetVendorAcctList>();

                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "ManageChecksAPI/AddCheck_GetVendorAcct";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorAcct);

                            _lstGetVendorAcctList = (new JavaScriptSerializer()).Deserialize<List<GetVendorAcctList>>(_APIResponse.ResponseData);
                            da = CommonMethods.ToDataSet<GetVendorAcctList>(_lstGetVendorAcctList);
                            da.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                        }
                        else
                        {
                            da = _objBLVendor.GetVendorAcct(_objVendor);
                        }

                        if (da.Tables[0].Rows.Count > 0)
                        {
                            var acct = da.Tables[0].Rows[0]["Acct#"].ToString();
                            _dri["VendorAcct"] = acct;
                        }
                        dti.Rows.Add(_dri);
                    }
                }

                _objVendor.ConnConfig = Session["config"].ToString();
                _objVendor.ID = vid;

                _getVendorRolDetails.ConnConfig = Session["config"].ToString();
                _getVendorRolDetails.ID = vid;

                DataSet _dsV = new DataSet();
                List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/CheckList_GetVendorRolDetails";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorRolDetails);

                    _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
                    _dsV = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
                    _dsV.Tables[0].Columns.Remove("Type");
                    _dsV.Tables[0].Columns["VType"].ColumnName = "Type";
                    _dsV.Tables[0].Columns["Vendor1099"].ColumnName = "1099";
                    _dsV.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                    //_dsV.Tables[0].Columns["Remit"].ColumnName = "RemitAddress";
                }
                else
                {
                    _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
                }


                //string _amount = String.Format("{0:c}", Convert.ToDouble(ViewState["Amount"]));
                string _amount = String.Format("{0:c}", Convert.ToDouble(SumAmountpay));
                _amount = _amount.Replace("$", string.Empty);
                _drC = dtpay.NewRow();
                

                if (Convert.ToDouble(SumAmountpay) > 1000)
                {
                    _drC["Pay"] = ConvertNumberToCurrency(SumAmountpay);
                    //_drC["Pay"] = ViewState["Dollar"].ToString();
                }
                else
                {
                    string dollar = ConvertNumberToCurrency(SumAmountpay);
                    _drC["Pay"] = dollar + " Dollars";
                }

                _drC["ToOrder"] = ViewState["Vendor"].ToString();
                _drC["Date"] = txtDate.Text;
                _drC["CheckAmount"] = SumAmountpay;
                _drC["VendorAddress"] = _dsV.Tables[0].Rows[0]["VendorAddress"];                // change by Mayuri on 8th nov,16
                _drC["RemitAddress"] = _dsV.Tables[0].Rows[0]["RemitAddress"];                  // change by Mayuri on 8th nov,16
                _drC["VendorAcct"] = _dsV.Tables[0].Rows[0]["Acct#"];                  // change by Mayuri on 8th nov,16
                dtpay.Rows.Add(_drC);

                //RAHIL'S IMPLEMENTATION
                //_objCD.ConnConfig = Session["config"].ToString();
                long checkno = long.Parse (ViewState["Checkno"].ToString());
                _objCD.Ref = checkno;

                _objBank.ConnConfig = Session["config"].ToString();
                _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);

                _getBankCD.ConnConfig = Session["config"].ToString();
                _getBankCD.ID = Convert.ToInt32(ddlBank.SelectedValue);
                DataSet _dsB = new DataSet();

                List<BankViewModel> _lstBankViewModel = new List<BankViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/AddCheck_GetBankCD";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getBankCD);

                    _lstBankViewModel = (new JavaScriptSerializer()).Deserialize<List<BankViewModel>>(_APIResponse.ResponseData);
                    _dsB = CommonMethods.ToDataSet<BankViewModel>(_lstBankViewModel);
                    _dsB.Tables[0].Columns["RolName"].ColumnName = "Name";
                    _dsB.Tables[0].Columns["RolAddress"].ColumnName = "Address";
                    _dsB.Tables[0].Columns["RolState"].ColumnName = "State";
                    _dsB.Tables[0].Columns["RolCity"].ColumnName = "City";
                    _dsB.Tables[0].Columns["RolZip"].ColumnName = "Zip";
                }
                else
                {
                    _dsB = _objBLBill.GetBankCD(_objBank);
                }

                _drB = dtBank.NewRow();
                if (_dsB.Tables[0].Rows.Count > 0)
                {
                    _drB["Name"] = _dsB.Tables[0].Rows[0]["Name"].ToString();
                    _drB["Address"] = _dsB.Tables[0].Rows[0]["Address"].ToString();
                    _drB["State"] = _dsB.Tables[0].Rows[0]["State"].ToString();
                    _drB["City"] = _dsB.Tables[0].Rows[0]["City"].ToString();
                    _drB["Zip"] = _dsB.Tables[0].Rows[0]["Zip"].ToString();
                    _drB["NBranch"] = _dsB.Tables[0].Rows[0]["NBranch"].ToString();
                    _drB["NAcct"] = _dsB.Tables[0].Rows[0]["NAcct"].ToString();
                    _drB["NRoute"] = _dsB.Tables[0].Rows[0]["NRoute"].ToString();
                    //_drB["Ref"] = _dsB.Tables[0].Rows[0]["Ref"].ToString();
                    //_dtBank.Rows.Add(_drB);
                }
                string checkNumber = string.Empty;
                if (!string.IsNullOrEmpty(txtNextCheck.Text))
                {
                    checkNumber = txtNextCheck.Text;
                }
                else
                {
                    checkNumber = ViewState["Checkno"].ToString();
                }

                if (checkNumber.Length == 1)
                {
                    _drB["Ref"] = "00000000" + checkNumber;
                }
                else if (checkNumber.Length == 2)
                {
                    _drB["Ref"] = "0000000" + checkNumber;
                }
                else if (checkNumber.Length == 3)
                {
                    _drB["Ref"] = "000000" + checkNumber;
                }
                else if (checkNumber.Length == 4)
                {
                    _drB["Ref"] = "00000" + checkNumber;
                }
                else if (checkNumber.Length == 5)
                {
                    _drB["Ref"] = "0000" + checkNumber;
                }
                else if (checkNumber.Length == 6)
                {
                    _drB["Ref"] = "000" + checkNumber;
                }
                else if (checkNumber.Length == 7)
                {
                    _drB["Ref"] = "00" + checkNumber;
                }
                else if (checkNumber.Length == 8)
                {
                    _drB["Ref"] = "0" + checkNumber;
                }
                else
                {
                    _drB["Ref"] = "000000000";
                }

                dtBank.Rows.Add(_drB);

                _objVendor.ConnConfig = Session["config"].ToString();
                _objVendor.ID = vid;

                _getVendorAcct.ConnConfig = Session["config"].ToString();
                _getVendorAcct.ID = vid;

                DataSet _dsA = new DataSet();
                List<GetVendorAcctList> _lstGetVendorAcct = new List<GetVendorAcctList>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/AddCheck_GetVendorAcct";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorAcct);

                    _lstGetVendorAcct = (new JavaScriptSerializer()).Deserialize<List<GetVendorAcctList>>(_APIResponse.ResponseData);
                    _dsA = CommonMethods.ToDataSet<GetVendorAcctList>(_lstGetVendorAcct);
                    _dsA.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                }
                else
                {
                    _dsA = _objBLVendor.GetVendorAcct(_objVendor);
                }

                _drA = _dtAcct.NewRow();
                _drA["VendorID"] = _dsA.Tables[0].Rows[0]["ID"].ToString();
                _drA["VendorAcct"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
                _dtAcct.Rows.Add(_drA);
                //-----------------------------------------------

                var rowCount = 0;
                var totalRows = dti.Rows.Count;
                if (reportName.Contains("-"))
                {
                    try
                    {
                        string[] reportNameArr = reportName.Split('-');
                        rowCount = Convert.ToInt32(reportNameArr[1].ToString().Trim().TrimStart());
                        if (totalRows < rowCount)
                            rowCount = totalRows;
                    }
                    catch (Exception ex) { rowCount = totalRows; }
                }
                else
                    rowCount = 6;
                var dtiCopy = dti.Copy();
                DataView dv = dtiCopy.DefaultView;
                dv.Sort = "Ref asc";
                DataTable sortedDT = dv.ToTable();
                var dtCopy = sortedDT.Copy();
                var firstHalf = dtCopy;
                var secondHalf = dtCopy;
                if (dtCopy.Rows.Count > rowCount)
                {
                    firstHalf = dtCopy.AsEnumerable().Take(rowCount).CopyToDataTable();
                    secondHalf = dtCopy.Clone();
                    if (totalRows > rowCount)
                    {
                        secondHalf = dtCopy.AsEnumerable().Skip(rowCount).Take(totalRows - rowCount).CopyToDataTable();
                    }
                }
                else
                {
                    firstHalf = dtCopy;
                }

                DataTable _dti = new DataTable();

                DataSet dsC = new DataSet();

                objPropUser.ConnConfig = Session["config"].ToString();

                //API
                _getConnectionConfig.ConnConfig = Session["config"].ToString();

                if (Session["MSM"].ToString() != "TS")
                {
                    List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_GetControl";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                        _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                        dsC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
                    }
                    else
                    {
                        dsC = objBL_User.getControl(objPropUser);
                    }
                }
                else
                {
                    objPropUser.LocID = Convert.ToInt32(0);

                    //API
                    _getControlBranch.LocID = Convert.ToInt32(0);

                    List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();
                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];

                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_GetControlBranch";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getControlBranch);

                        _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                        dsC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
                    }
                    else
                    {
                        dsC = objBL_User.getControlBranch(objPropUser);
                    }
                    // dsC = objBL_User.getControlBranch(objPropUser);
                }

                //STIMULSOFT 
                byte[] buffer1 = null;
                string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + reportName.Trim() + ".mrt");
                StiReport report = new StiReport();
                report.Load(reportPathStimul);
                report.Compile();
                report["TotalAmountPay"] = SumAmountpay;
                report["Memo"] = txtMemo.Text;

                report["InvoiceCount"] = totalRows;
                DataSet Invoice = new DataSet();
                DataTable dtInvoice = firstHalf.Copy();
                dtInvoice.TableName = "Invoice";
                Invoice.Tables.Add(dtInvoice);
                Invoice.DataSetName = "Invoice";

                DataSet Check = new DataSet();
                DataTable dtCheck = dtpay.Copy();
                dtCheck.TableName = "Check";
                Check.Tables.Add(dtCheck);
                Check.DataSetName = "Check";

                DataSet ControlBranch = new DataSet();
                DataTable dtControlBranch = new DataTable();
                dtControlBranch = dsC.Tables[0].Copy();
                ControlBranch.Tables.Add(dtControlBranch);
                dtControlBranch.TableName = "ControlBranch";
                ControlBranch.DataSetName = "ControlBranch";


                DataSet Bank = new DataSet();
                DataTable _dtBank = dtBank;
                dtBank.TableName = "Bank";
                Bank.Tables.Add(dtBank);
                Bank.DataSetName = "Bank";

                DataSet Account = new DataSet();
                DataTable dtAccount = _dtAcct;
                _dtAcct.TableName = "Account";
                Account.Tables.Add(_dtAcct);
                Account.DataSetName = "Account";





                report.RegData("Invoice", Invoice);
                report.RegData("Check", Check);
                report.RegData("ControlBranch", ControlBranch);
                report.RegData("dsBank", Bank);
                report.RegData("dsAccount", Account);
                report.Render();
                var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                var service = new Stimulsoft.Report.Export.StiPdfExportService();
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                service.ExportTo(report, stream, settings);
                buffer1 = stream.ToArray();
                lstbyte.Add(buffer1);




                if (totalRows > 6)
                {
                    byte[] bufferNew = null;
                    reportPathStimul = Server.MapPath("StimulsoftReports/TopCheckSubReport.mrt");
                    report = new StiReport();
                    report.Load(reportPathStimul);
                    report.Compile();
                    report["TotalAmountPay"] = SumAmountpay;
                    report["AccountNo"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
                    report["Memo"] = txtMemo.Text;

                    report["InvoiceCount"] = totalRows;
                    Invoice = new DataSet();
                    dtInvoice = secondHalf.Copy();
                    dtInvoice.TableName = "Invoice";
                    Invoice.Tables.Add(dtInvoice);
                    Invoice.DataSetName = "Invoice";

                    Check = new DataSet();
                    dtCheck = dtpay.Copy();
                    dtCheck.TableName = "Check";
                    Check.Tables.Add(dtCheck);
                    Check.DataSetName = "Check";

                    //DataSet ControlBranch = new DataSet();
                    //DataTable dtControlBranch = new DataTable();
                    //dtControlBranch = dsC.Tables[0].Copy();
                    //ControlBranch.Tables.Add(dtControlBranch);
                    //dtControlBranch.TableName = "ControlBranch";
                    //ControlBranch.DataSetName = "ControlBranch";




                    report.RegData("Invoice", Invoice);
                    report.RegData("Check", Check);
                    report.Render();
                    settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                    service = new Stimulsoft.Report.Export.StiPdfExportService();
                    stream = new System.IO.MemoryStream();
                    service.ExportTo(report, stream, settings);
                    bufferNew = stream.ToArray();

                    lstbyteNew.Add(bufferNew);
                }
                byte[] finalbyte = null;
                if (lstbyteNew.Count != 0)
                {
                    finalbyte = WriteChecks.concatAndAddContentFinal(lstbyte, lstbyteNew);
                }
                else
                {
                    finalbyte = WriteChecks.concatAndAddContent(lstbyte);
                }
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Buffer = true;
                Response.AddHeader("Content-Disposition", "attachment;filename=MidCheckCub.pdf");
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Length", (finalbyte.Length).ToString());
                Response.BinaryWrite(finalbyte);
            }

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }


    protected DataTable BuildCompanyDetailsTable()
    {
        DataTable companyDetailsTable = new DataTable();
        companyDetailsTable.Columns.Add("CompanyAddress");
        companyDetailsTable.Columns.Add("CompanyName");
        companyDetailsTable.Columns.Add("ContactNo");
        companyDetailsTable.Columns.Add("Email");
        companyDetailsTable.Columns.Add("LogoURL");

        return companyDetailsTable;
    }
    private StiReport FillDataSetToReport_New(string reportName)
    {

        double SumAmountpay = 0.00;
        DataTable _dti = new DataTable();
        DataRow _dri = null;
        _dti.Columns.Add(new DataColumn("Ref", typeof(string)));
        _dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("Reference", typeof(string)));
        _dti.Columns.Add(new DataColumn("Total", typeof(double)));
        _dti.Columns.Add(new DataColumn("Disc", typeof(double)));
        _dti.Columns.Add(new DataColumn("AmountPay", typeof(double)));
        _dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("VendorName", typeof(string)));

        //RAHIL
        _dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("Description", typeof(string)));

        //New column
        _dti.Columns.Add(new DataColumn("State", typeof(string)));
        _dti.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dti.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dti.Columns.Add(new DataColumn("TotalAmount", typeof(string)));

        DataTable _dtCheck = new DataTable();
        DataRow _drC = null;
        _dtCheck.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrder", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Date", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("State", typeof(string)));

        //NEW COLUMN
        _dtCheck.Columns.Add(new DataColumn("VendorAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("RemitAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Zip", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("TotalAmountPay", typeof(double)));
        _dtCheck.Columns.Add(new DataColumn("Memo", typeof(string)));

        _objCD.ConnConfig = Session["config"].ToString();
        _objCD.Ref = long.Parse(txtNextCheck.Text);
        _objCD.NextC = long.Parse(txtNextCheck.Text);
        _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);

        _getCheckDetailsByBankAndRef.ConnConfig = Session["config"].ToString();
        _getCheckDetailsByBankAndRef.Ref = long.Parse(txtNextCheck.Text);
        _getCheckDetailsByBankAndRef.NextC = long.Parse(txtNextCheck.Text);
        _getCheckDetailsByBankAndRef.Bank = Convert.ToInt32(ddlBank.SelectedValue);

        DataSet _dsCheck = new DataSet();
        DataSet _dsCheck1 = new DataSet();
        DataSet _dsCheck2 = new DataSet();
        ListCheckDetailsByBankAndRef _ListCheckDetailsByBankAndRef = new ListCheckDetailsByBankAndRef();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetCheckDetailsByBankAndRef";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCheckDetailsByBankAndRef);

            _ListCheckDetailsByBankAndRef = (new JavaScriptSerializer()).Deserialize<ListCheckDetailsByBankAndRef>(_APIResponse.ResponseData);

            _dsCheck1 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable1.ToDataSet();
            _dsCheck2 = _ListCheckDetailsByBankAndRef.lstCheckDetailsByBankAndRefTable2.ToDataSet();

            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            dt1 = _dsCheck1.Tables[0];
            dt2 = _dsCheck2.Tables[0];

            dt1.TableName = "Table1";
            dt2.TableName = "Table2";

            _dsCheck.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });
        }
        else
        {
            _dsCheck = _objBLBill.GetCheckDetailsByBankAndRef(_objCD);
        }

        //int vid = Convert.ToInt32(_dsCheck.Tables[0].Rows[0]["Vendor"].ToString());
        DataTable dtNew = new DataTable();
        dtNew.Columns.Add("Name");
        dtNew.Columns.Add("Vendor");

        //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        ////if (IsAPIIntegrationEnable == "YES")
        //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        //{
        //    foreach (DataRow drow in _dsCheck1.Tables[0].Rows)
        //    {
        //        DataRow drNew = dtNew.NewRow();
        //        drNew["Name"] = drow["VendorName"].ToString();
        //        drNew["Vendor"] = drow["Vendor"].ToString();
        //        dtNew.Rows.Add(drNew);
        //    }
        //}
        //else
        //{
            foreach (DataRow drow in _dsCheck.Tables[0].Rows)
            {
                DataRow drNew = dtNew.NewRow();
                drNew["Name"] = drow["VendorName"].ToString();
                drNew["Vendor"] = drow["Vendor"].ToString();
                dtNew.Rows.Add(drNew);
            }
        //}
        DataTable dtN = dtNew.DefaultView.ToTable(true);
        DataTable _dtAcct = new DataTable();
        //foreach (DataRow dr in dtN.Rows)
        //{
        int vid = Convert.ToInt32(dtN.Rows[0]["Vendor"].ToString());
        double AmountPay = 0.00;
        SumAmountpay = 0.00;



        //DataTable dtInvoice = _dsCheck.Tables[0].DefaultView.ToTable(true);
        DataView dtInv = new DataView();
        //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        ////if (IsAPIIntegrationEnable == "YES")
        //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        //{
        //    dtInv = _dsCheck1.Tables[0].DefaultView;
        //}
        //else
        //{
            dtInv = _dsCheck.Tables[0].DefaultView;
        //}

        dtInv.RowFilter = "Vendor = '" + vid + "'";
        foreach (DataRow drow in dtInv.ToTable(true).Rows)
        {
            _dri = _dti.NewRow();
            _dri["Ref"] = drow["Ref"].ToString();
            _dri["Description"] = drow["Description"].ToString();
            _dri["InvoiceDate"] = drow["InvoiceDate"].ToString();
            _dri["Reference"] = drow["Refrerence"].ToString();
            _dri["Total"] = double.Parse(drow["Total"].ToString().Replace('$', '0'), NumberStyles.AllowParentheses |
                          NumberStyles.AllowThousands |
                          NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
            _dri["Disc"] = Convert.ToDouble(drow["Disc"].ToString()).ToString();
            _dri["AmountPay"] = Convert.ToDouble(drow["AmountPay"].ToString()).ToString();
            SumAmountpay = SumAmountpay + Convert.ToDouble(drow["AmountPay"].ToString());
            _dri["PayDate"] = drow["PayDate"].ToString();
            _dri["CheckNo"] = drow["CheckNo"].ToString();


            //_dri["VendorID"] = Convert.ToInt32(ddlVendor.SelectedValue);
            _dri["VendorID"] = drow["Vendor"].ToString();
            //ac _dri["VendorName"] = ddlVendor.SelectedItem.Text;
            _dri["VendorName"] = drow["VendorName"].ToString();
            _dti.Rows.Add(_dri);

            _dti.AcceptChanges();

        }










        _objVendor.ConnConfig = Session["config"].ToString();
        _objVendor.ID = vid;

        _getVendorRolDetails.ConnConfig = Session["config"].ToString();
        _getVendorRolDetails.ID = vid;

        DataSet _dsV = new DataSet();
        List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetVendorRolDetails";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorRolDetails);

            _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
            _dsV = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
            _dsV.Tables[0].Columns.Remove("Type");
            _dsV.Tables[0].Columns["VType"].ColumnName = "Type";
            _dsV.Tables[0].Columns["Vendor1099"].ColumnName = "1099";
            _dsV.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
           // _dsV.Tables[0].Columns["Remit"].ColumnName = "RemitAddress";
        }
        else
        {
            _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
        }

        string vendAddress = "";
        string vendAddress2 = "";
        if (_dsV.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
            {
                vendAddress = _dsV.Tables[0].Rows[0]["Address"].ToString() + ", ";
            }

            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
            {
                vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
            }
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
            {
                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                {
                    vendAddress2 += ", ";
                }
                vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
            }
        }


        DataView dtcheck = new DataView();
        //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        ////if (IsAPIIntegrationEnable == "YES")
        //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        //{
        //    dtcheck = _dsCheck2.Tables[0].DefaultView;
        //}
        //else
        //{
            dtcheck = _dsCheck.Tables[1].DefaultView;
        //}

        dtcheck.RowFilter = "Vendor = '" + vid + "'";
        foreach (DataRow drow in dtcheck.ToTable(true).Rows)
        //foreach (DataRow drow in _dsCheck.Tables[1].Rows)
        {
            _drC = _dtCheck.NewRow();
            if (Convert.ToDouble(drow["Pay"]) > 1000)
            {
                _drC["Pay"] = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                //_drC["Pay"] = ViewState["Dollar"].ToString();
            }
            else
            {
                string dollar = ConvertNumberToCurrency(Convert.ToDouble(drow["Pay"]));
                _drC["Pay"] = dollar + " Dollars";
            }
            _drC["ToOrder"] = drow["ToOrder"].ToString();
            //_drC["ToOrder"] = ViewState["Vendor"].ToString();
            _drC["Date"] = drow["Date"].ToString();
            _drC["CheckAmount"] = Convert.ToDouble(drow["Pay"]);
            _drC["ToOrderAddress"] = vendAddress;
            _drC["State"] = vendAddress2;

            _drC["TotalAmountpay"] = SumAmountpay;
            _drC["State"] = drow["State"].ToString();
            _dtCheck.Rows.Add(_drC);
        }






        DataSet dsCC = new DataSet();
        User objPropUser = new User();
        //ViewState["Checkno"] = lblNextCheck.Text;
        objPropUser.ConnConfig = Session["config"].ToString();
        _getConnectionConfig.ConnConfig = Session["config"].ToString();
        _getControlBranch.ConnConfig = Session["config"].ToString();

        if (Session["MSM"].ToString() != "TS")
        {
            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetControl";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                dsCC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
            }
            else
            {
                dsCC = objBL_User.getControl(objPropUser);
            }
        }
        else
        {
            objPropUser.LocID = Convert.ToInt32(0);
            _getControlBranch.LocID = Convert.ToInt32(0);
            List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetControlBranch";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getControlBranch);

                _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                dsCC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
            }
            else
            {
                dsCC = objBL_User.getControlBranch(objPropUser);
            }
        }
        ReportViewer rvChecks = new ReportViewer();
        rvChecks.LocalReport.DataSources.Clear();
        //STIMULSOFT 
        byte[] buffer1 = null;
        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + reportName.Trim() + ".mrt");
        //  string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/APTopCheckDefault.mrt");
        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        report.Compile();
        report["TotalAmountPay"] = SumAmountpay;

        //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        ////if (IsAPIIntegrationEnable == "YES")
        //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        //{
        //    report["Memo"] = Convert.ToString(_dsCheck2.Tables[0].Rows[0]["Memo"].ToString());
        //}
        //else
        //{
            report["Memo"] = Convert.ToString(_dsCheck.Tables[1].Rows[0]["Memo"].ToString());
        //}

        report["InvoiceCount"] = dti.Rows.Count;
        DataSet Invoice = new DataSet();
        DataTable dtInvoice = _dti;
        _dti.TableName = "Invoice";
        Invoice.Tables.Add(_dti);
        Invoice.DataSetName = "Invoice";

        DataSet Check = new DataSet();
        DataTable dtCheck = _dtCheck;
        _dtCheck.TableName = "Check";
        Check.Tables.Add(_dtCheck);
        Check.DataSetName = "Check";

        DataSet ControlBranch = new DataSet();
        DataTable dtControlBranch = new DataTable();
        dtControlBranch = dsCC.Tables[0].Copy();
        ControlBranch.Tables.Add(dtControlBranch);
        dtControlBranch.TableName = "ControlBranch";
        ControlBranch.DataSetName = "ControlBranch";

        report.RegData("dsInvoices", Invoice);
        report.RegData("dsCheck", Check);
        report.RegData("dsTicket", ControlBranch);
        report.Render();

        //StiWebDesigner1.Visible = true;
        //StiWebDesigner1.Report = report;

        //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "opencCeateForm();", true);
        return report;


    }
    private StiReport FillDataSetToReport(string reportName)
    {

        double SumAmountpay = 0.00;
        DataTable _dti = new DataTable();
        DataRow _dri = null;
        _dti.Columns.Add(new DataColumn("Ref", typeof(string)));
        _dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("Reference", typeof(string)));
        _dti.Columns.Add(new DataColumn("Total", typeof(double)));
        _dti.Columns.Add(new DataColumn("Disc", typeof(double)));
        _dti.Columns.Add(new DataColumn("AmountPay", typeof(double)));
        _dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("VendorName", typeof(string)));

        //RAHIL
        _dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("Description", typeof(string)));

        //New column
        _dti.Columns.Add(new DataColumn("State", typeof(string)));
        _dti.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dti.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dti.Columns.Add(new DataColumn("TotalAmount", typeof(string)));

        DataTable _dtCheck = new DataTable();
        DataRow _drC = null;
        _dtCheck.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrder", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Date", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("State", typeof(string)));

        //NEW COLUMN
        _dtCheck.Columns.Add(new DataColumn("VendorAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("RemitAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Zip", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("TotalAmountPay", typeof(double)));
        _dtCheck.Columns.Add(new DataColumn("Memo", typeof(string)));

        int vid = Convert.ToInt32(ddlVendor.SelectedValue);
        //foreach (GridViewRow gr in gvBills.Rows)
        foreach (GridDataItem gr in gvBills.Items)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            if (chkSelect.Checked == true)
            {
                TextBox txtGvPay = (TextBox)gr.FindControl("txtGvPay");
                Label lblBalance = (Label)gr.FindControl("lblBalance");
                TextBox txtGvDisc = (TextBox)gr.FindControl("txtGvDisc");
                Label lblOrig = (Label)gr.FindControl("lblOrig");
                HiddenField hdnRef = (HiddenField)gr.FindControl("hdnRef");

                Label lblBillfdesc = (Label)gr.FindControl("lblBillfdesc");
                TextBox txtGvDesc = (TextBox)gr.FindControl("txtGvDesc");
                Label lblfDate = (Label)gr.FindControl("lblfDate");
                Label lblRef = (Label)gr.FindControl("lblRef");

                _dri = _dti.NewRow();
                _dri["Ref"] = hdnRef.Value;
                //_dri["Description"] = lblBillfdesc.Text;
                _dri["Description"] = txtGvDesc.Text;
                //RAHIL
                //_objOpenAP.Ref = hdnRef.Value;
                //DataSet _dsCheck = _objBLBill.GetCheckDetails(_objOpenAP);

                //if (_dsCheck.Tables[0].Rows.Count > 0)
                //{
                //    _dri["Description"] = _dsCheck.Tables[0].Rows[0]["fDesc"].ToString();
                //}

                //DataRow[] dr = gds.Tables[0].Select("Ref='" + hdnRef.Value + "'");                   

                _dri["InvoiceDate"] = lblfDate.Text;
                _dri["Reference"] = lblRef.Text;
                _dri["Total"] = double.Parse(lblOrig.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                              NumberStyles.AllowThousands |
                              NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                _dri["Disc"] = Convert.ToDouble(txtGvDisc.Text);
                _dri["AmountPay"] = Convert.ToDouble(txtGvPay.Text);
                SumAmountpay = SumAmountpay + Convert.ToDouble(txtGvPay.Text);
                _dri["PayDate"] = txtDate.Text;
                //_dri["CheckNo"] = ViewState["Checkno"].ToString();
                if (!string.IsNullOrEmpty(txtNextCheck.Text))
                {
                    _dri["CheckNo"] = txtNextCheck.Text;
                }
                else
                {
                    _dri["CheckNo"] = ViewState["Checkno"].ToString();
                }
                _dri["VendorID"] = Convert.ToInt32(ddlVendor.SelectedValue);
                _dri["VendorName"] = ddlVendor.SelectedItem.Text;
                _dti.Rows.Add(_dri);
            }
        }
        _objVendor.ConnConfig = Session["config"].ToString();
        _objVendor.ID = vid;

        _getVendorRolDetails.ConnConfig = Session["config"].ToString();
        _getVendorRolDetails.ID = vid;

        DataSet _dsV = new DataSet();
        List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetVendorRolDetails";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorRolDetails);

            _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
            _dsV = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
            _dsV.Tables[0].Columns.Remove("Type");
            _dsV.Tables[0].Columns["VType"].ColumnName = "Type";
            _dsV.Tables[0].Columns["Vendor1099"].ColumnName = "1099";
            _dsV.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
            //_dsV.Tables[0].Columns["Remit"].ColumnName = "RemitAddress";
        }
        else
        {
            _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
        }

        string vendAddress = "";
        string vendAddress2 = "";
        if (_dsV.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
            {
                vendAddress = _dsV.Tables[0].Rows[0]["Address"].ToString() + ", ";
            }

            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
            {
                vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
            }
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
            {
                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                {
                    vendAddress2 += ", ";
                }
                vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
            }
        }
        _drC = _dtCheck.NewRow();
        //_drC["Pay"] = ViewState["Dollar"].ToString();
        if (Convert.ToDouble(SumAmountpay) > 1000)
        {
            _drC["Pay"] = ConvertNumberToCurrency(SumAmountpay);
            //_drC["Pay"] = ViewState["Dollar"].ToString();
        }
        else
        {
            string dollar = ConvertNumberToCurrency(SumAmountpay);
            _drC["Pay"] = dollar + " Dollars";
        }
        _drC["ToOrder"] = ViewState["Vendor"].ToString();
        _drC["Date"] = txtDate.Text;
        //_drC["CheckAmount"] = Convert.ToDouble(ViewState["Amount"]).ToString("0.00", CultureInfo.InvariantCulture);
        _drC["CheckAmount"] = Convert.ToDouble(SumAmountpay).ToString("0.00", CultureInfo.InvariantCulture);
        _drC["ToOrderAddress"] = vendAddress;
        _drC["State"] = vendAddress2;

        _drC["TotalAmountpay"] = SumAmountpay;
        _drC["State"] = txtMemo.Text;
        _dtCheck.Rows.Add(_drC);

        DataSet dsC = new DataSet();

        //ViewState["Checkno"] = lblNextCheck.Text;
        objPropUser.ConnConfig = Session["config"].ToString();
        _getConnectionConfig.ConnConfig = Session["config"].ToString();
        _getControlBranch.ConnConfig = Session["config"].ToString();

        if (Session["MSM"].ToString() != "TS")
        {
            List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetControl";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                dsC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
            }
            else
            {
                dsC = objBL_User.getControl(objPropUser);
            }
        }
        else
        {
            objPropUser.LocID = Convert.ToInt32(0);
            _getControlBranch.LocID = Convert.ToInt32(0);
            List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetControlBranch";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getControlBranch);

                _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                dsC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
            }
            else
            {
                dsC = objBL_User.getControlBranch(objPropUser);
            }
        }
        ReportViewer rvChecks = new ReportViewer();
        rvChecks.LocalReport.DataSources.Clear();
        //STIMULSOFT 
        byte[] buffer1 = null;
        string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + reportName.Trim() + ".mrt");
        //  string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/APTopCheckDefault.mrt");
        StiReport report = new StiReport();
        report.Load(reportPathStimul);
        report.Compile();
        report["TotalAmountPay"] = SumAmountpay;
        report["Memo"] = txtMemo.Text;

        report["InvoiceCount"] = dti.Rows.Count;
        DataSet Invoice = new DataSet();
        DataTable dtInvoice = _dti;
        _dti.TableName = "Invoice";
        Invoice.Tables.Add(_dti);
        Invoice.DataSetName = "Invoice";

        DataSet Check = new DataSet();
        DataTable dtCheck = _dtCheck;
        _dtCheck.TableName = "Check";
        Check.Tables.Add(_dtCheck);
        Check.DataSetName = "Check";

        DataSet ControlBranch = new DataSet();
        DataTable dtControlBranch = new DataTable();
        dtControlBranch = dsC.Tables[0].Copy();
        ControlBranch.Tables.Add(dtControlBranch);
        dtControlBranch.TableName = "ControlBranch";
        ControlBranch.DataSetName = "ControlBranch";

        report.RegData("dsInvoices", Invoice);
        report.RegData("dsCheck", Check);
        report.RegData("dsTicket", ControlBranch);
        report.Render();

        //StiWebDesigner1.Visible = true;
        //StiWebDesigner1.Report = report;

        //ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), "opencCeateForm();", true);
        return report;


    }

    public void FillDataSetToReport1()
    {
        double Amountpay = 0.00;
        DataTable _dti = new DataTable();
        DataRow _dri = null;
        _dti.Columns.Add(new DataColumn("Ref", typeof(string)));
        _dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("Reference", typeof(string)));
        _dti.Columns.Add(new DataColumn("Total", typeof(string)));
        _dti.Columns.Add(new DataColumn("Disc", typeof(string)));
        _dti.Columns.Add(new DataColumn("AmountPay", typeof(string)));
        _dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
        _dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
        _dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dti.Columns.Add(new DataColumn("VendorName", typeof(string)));

        DataTable _dtCheck = new DataTable();
        DataRow _drC = null;
        _dtCheck.Columns.Add(new DataColumn("Pay", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrder", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("Date", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        _dtCheck.Columns.Add(new DataColumn("State", typeof(string)));
        int vid = Convert.ToInt32(ddlVendor.SelectedValue);

        //foreach (GridViewRow gr in gvBills.Rows)
        foreach (GridDataItem gr in gvBills.Items)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

            if (chkSelect.Checked == true)
            {
                TextBox txtGvPay = (TextBox)gr.FindControl("txtGvPay");
                Label lblBalance = (Label)gr.FindControl("lblBalance");
                TextBox txtGvDisc = (TextBox)gr.FindControl("txtGvDisc");
                Label lblOrig = (Label)gr.FindControl("lblOrig");
                HiddenField hdnRef = (HiddenField)gr.FindControl("hdnRef");
                Label lblBillfdesc = (Label)gr.FindControl("lblBillfdesc");
                TextBox txtGvDesc = (TextBox)gr.FindControl("txtGvDesc");
                Label lblfDate = (Label)gr.FindControl("lblfDate");
                Label lblRef = (Label)gr.FindControl("lblRef");

                _dri = _dti.NewRow();
                _dri["Ref"] = hdnRef.Value;
                //_dri["Description"] = lblBillfdesc.Text;
                _dri["Description"] = txtGvDesc.Text;
                _dri["InvoiceDate"] = lblfDate.Text;
                _dri["Reference"] = lblRef.Text;
                _dri["Total"] = double.Parse(lblOrig.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                              NumberStyles.AllowThousands |
                              NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                _dri["Disc"] = Convert.ToDouble(txtGvDisc.Text).ToString();
                _dri["AmountPay"] = Convert.ToDouble(txtGvPay.Text).ToString();
                Amountpay = Amountpay + Convert.ToDouble(txtGvPay.Text);
                _dri["PayDate"] = txtDate.Text;
                //_dri["CheckNo"] = ViewState["Checkno"].ToString();
                if (!string.IsNullOrEmpty(txtNextCheck.Text))
                {
                    _dri["CheckNo"] = txtNextCheck.Text;
                }
                else
                {
                    _dri["CheckNo"] = ViewState["Checkno"].ToString();
                }
                _dri["VendorID"] = Convert.ToInt32(ddlVendor.SelectedValue);
                _dri["VendorName"] = ddlVendor.SelectedItem.Text;
                _dti.Rows.Add(_dri);
            }
        }

        _objVendor.ConnConfig = Session["config"].ToString();
        _objVendor.ID = vid;

        _getVendorRolDetails.ConnConfig = Session["config"].ToString();
        _getVendorRolDetails.ID = vid;

        DataSet _dsV = new DataSet();
        List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/CheckList_GetVendorRolDetails";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorRolDetails);

            _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
            _dsV = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
            _dsV.Tables[0].Columns.Remove("Type");
            _dsV.Tables[0].Columns["VType"].ColumnName = "Type";
            _dsV.Tables[0].Columns["Vendor1099"].ColumnName = "1099";
            _dsV.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
           // _dsV.Tables[0].Columns["Remit"].ColumnName = "RemitAddress";
        }
        else
        {
            _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
        }

        string vendAddress = "";
        string vendAddress2 = "";
        if (_dsV.Tables[0].Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
            {
                vendAddress = _dsV.Tables[0].Rows[0]["Address"].ToString() + ", ";
            }

            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
            {
                vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
            }
            if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
            {
                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                {
                    vendAddress2 += ", ";
                }
                vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
            }
        }
        //string _amount = String.Format("{0:c}", Convert.ToDouble(ViewState["Amount"]));
        string _amount = String.Format("{0:c}", Convert.ToDouble(Amountpay));
        
        _amount = _amount.Replace("$", string.Empty);
        _drC = _dtCheck.NewRow();
        //_drC["Pay"] = ViewState["Dollar"].ToString();
        if (Convert.ToDouble(Amountpay) > 1000)
        {
            _drC["Pay"] = ConvertNumberToCurrency(Amountpay);
            //_drC["Pay"] = ViewState["Dollar"].ToString();
        }
        else
        {
            string dollar = ConvertNumberToCurrency(Amountpay);
            _drC["Pay"] = dollar + " Dollars";
        }

        _drC["ToOrder"] = ViewState["Vendor"].ToString();
        _drC["Date"] = txtDate.Text;
        //_drC["CheckAmount"] = _amount;
        _drC["CheckAmount"] = Convert.ToDouble(Amountpay).ToString("0.00", CultureInfo.InvariantCulture);
        _drC["ToOrderAddress"] = vendAddress;
        _drC["State"] = vendAddress2;
        _dtCheck.Rows.Add(_drC);

        DataSet dsC = new DataSet();
        ReportViewer rvChecks = new ReportViewer();
        rvChecks.LocalReport.DataSources.Clear();
        rvChecks.ProcessingMode = ProcessingMode.Local;
        rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsInvoices", _dti));
        rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsCheck", _dtCheck));
        string reportPath = "Reports/CheckTemplate2.rdlc";


        rvChecks.LocalReport.ReportPath = reportPath;
        rvChecks.LocalReport.Refresh();

        byte[] buffer = null;
        buffer = ExportReportToPDF("", rvChecks);
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Content-Disposition", "attachment;filename=PrintCheck.pdf");
        Response.ContentType = "application/pdf";
        Response.AddHeader("Content-Length", (buffer.Length).ToString());
        Response.BinaryWrite(buffer);
        Response.Flush();
        Response.Close();

    }
    protected void imgPrintTemp2_Click(object sender, ImageClickEventArgs e)
    {                                                                        //                AP – check middle 
        try
        {

            byte[] buffer1 = null;
            //  string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/APMidCheckDefault.mrt");
            string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + ddlApMiddleCheckForLoad.SelectedItem.Text.Trim() + ".mrt");

            StiReport report = new StiReport();
            //  report = FillMiddleDataSetReport("APMidCheckDefault");
            if (Request.QueryString["bill"] == null)
            {
                FillReportMiddleDataSet(ddlApMiddleCheckForLoad.SelectedItem.Text.Trim());
                _objUser.ConnConfig = Session["config"].ToString();
                _objUser.MOMUSer = Session["Username"].ToString();
                _objUser.UserLic = ddlApMiddleCheckForLoad.SelectedItem.Text;

                _updateCheckTemplate.ConnConfig = Session["config"].ToString();
                _updateCheckTemplate.MOMUSer = Session["Username"].ToString();
                _updateCheckTemplate.UserLic = ddlApMiddleCheckForLoad.SelectedItem.Text;

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/AddCheck_updateCheckTemplate";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateCheckTemplate);
                }
                else
                {
                    _objBLBill.updateCheckTemplate(_objUser);
                }
            }
            else if (Request.QueryString["bill"] == "c")
            {
                if (Request.QueryString["vid"] != null)
                {
                    FillReportMiddleDataSet(ddlApMiddleCheckForLoad.SelectedItem.Text.Trim());
                }
                else
                {
                    FillReportMiddleDataSet_New(ddlApMiddleCheckForLoad.SelectedItem.Text.Trim());
                }
                _objUser.ConnConfig = Session["config"].ToString();
                _objUser.MOMUSer = Session["Username"].ToString();
                _objUser.UserLic = ddlApMiddleCheckForLoad.SelectedItem.Text;

                _updateCheckTemplate.ConnConfig = Session["config"].ToString();
                _updateCheckTemplate.MOMUSer = Session["Username"].ToString();
                _updateCheckTemplate.UserLic = ddlApMiddleCheckForLoad.SelectedItem.Text;

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/AddCheck_updateCheckTemplate";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateCheckTemplate);
                }
                else
                {
                    _objBLBill.updateCheckTemplate(_objUser);
                }
                
            }
                //AddCheck();
                //var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
                //var service = new Stimulsoft.Report.Export.StiPdfExportService();
                //System.IO.MemoryStream stream = new System.IO.MemoryStream();
                //service.ExportTo(report, stream, settings);
                //buffer1 = stream.ToArray();

                //string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "/TempPDF", "APCheckMiddle.pdf");

                //if (buffer1 != null)
                //{
                //    if (File.Exists(filename))
                //        File.Delete(filename);
                //    using (var fs = new FileStream(filename, FileMode.Create))
                //    {
                //        fs.Write(buffer1, 0, buffer1.Length);
                //        fs.Close();
                //    }
                //}

                //Response.ClearContent();
                //Response.ClearHeaders();
                //Response.AddHeader("Content-Disposition", "attachment;filename=PrintCheck.pdf");
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("Content-Length", (buffer1.Length).ToString());
                //Response.BinaryWrite(buffer1);
                //Response.Flush();
                //Response.Close();
            }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void imgPrintTemp5_Click(object sender, ImageClickEventArgs e)
    {                                                                       //                  BNN – check top
        try
        {
            double AmountPay = 0;
            DataTable _dti = new DataTable();
            DataRow _dri = null;
            _dti.Columns.Add(new DataColumn("Ref", typeof(string)));
            _dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
            _dti.Columns.Add(new DataColumn("Reference", typeof(string)));
            _dti.Columns.Add(new DataColumn("Total", typeof(string)));
            _dti.Columns.Add(new DataColumn("Disc", typeof(string)));
            _dti.Columns.Add(new DataColumn("AmountPay", typeof(string)));
            _dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
            _dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
            _dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
            _dti.Columns.Add(new DataColumn("VendorName", typeof(string)));

            //RAHIL
            _dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
            _dti.Columns.Add(new DataColumn("Description", typeof(string)));


            DataTable _dtCheck = new DataTable();
            DataRow _drC = null;
            _dtCheck.Columns.Add(new DataColumn("Pay", typeof(string)));
            _dtCheck.Columns.Add(new DataColumn("ToOrder", typeof(string)));
            _dtCheck.Columns.Add(new DataColumn("Date", typeof(string)));
            _dtCheck.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
            _dtCheck.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
            _dtCheck.Columns.Add(new DataColumn("State", typeof(string)));
            int vid = Convert.ToInt32(ddlVendor.SelectedValue);
            //foreach (GridViewRow gr in gvBills.Rows)
            foreach (GridDataItem gr in gvBills.Items)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    TextBox txtGvPay = (TextBox)gr.FindControl("txtGvPay");
                    Label lblBalance = (Label)gr.FindControl("lblBalance");
                    TextBox txtGvDisc = (TextBox)gr.FindControl("txtGvDisc");
                    Label lblOrig = (Label)gr.FindControl("lblOrig");
                    HiddenField hdnRef = (HiddenField)gr.FindControl("hdnRef");
                    Label lblBillfdesc = (Label)gr.FindControl("lblBillfdesc");
                    TextBox txtGvDesc = (TextBox)gr.FindControl("txtGvDesc");
                    Label lblfDate = (Label)gr.FindControl("lblfDate");
                    Label lblRef = (Label)gr.FindControl("lblRef");

                    _dri = _dti.NewRow();
                    _dri["Ref"] = hdnRef.Value;
                    //_dri["Description"] = lblBillfdesc.Text;
                    _dri["Description"] = txtGvDesc.Text;
                    //RAHIL
                    //_objOpenAP.Ref = hdnRef.Value;
                    //DataSet _dsCheck = _objBLBill.GetCheckDetails(_objOpenAP);

                    //if (_dsCheck.Tables[0].Rows.Count > 0)
                    //{
                    //    _dri["Description"] = _dsCheck.Tables[0].Rows[0]["fDesc"].ToString();
                    //}

                    //DataRow[] dr = gds.Tables[0].Select("Ref='" + hdnRef.Value + "'");                   

                    _dri["InvoiceDate"] = lblfDate.Text;
                    _dri["Reference"] = lblRef.Text;
                    _dri["Total"] = double.Parse(lblOrig.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                    _dri["Disc"] = Convert.ToDouble(txtGvDisc.Text).ToString();
                    _dri["AmountPay"] = Convert.ToDouble(txtGvPay.Text).ToString();
                    
                    AmountPay = AmountPay + Convert.ToDouble(txtGvPay.Text);
                    _dri["PayDate"] = txtDate.Text;
                    if (!string.IsNullOrEmpty(txtNextCheck.Text))
                    {
                        _dri["CheckNo"] = txtNextCheck.Text;
                    }
                    else
                    {
                        _dri["CheckNo"] = ViewState["Checkno"].ToString();
                    }
                    _dri["VendorID"] = Convert.ToInt32(ddlVendor.SelectedValue);
                    _dri["VendorName"] = ddlVendor.SelectedItem.Text;
                    _dti.Rows.Add(_dri);
                }
            }
            _objVendor.ConnConfig = Session["config"].ToString();
            _objVendor.ID = vid;

            _getVendorRolDetails.ConnConfig = Session["config"].ToString();
            _getVendorRolDetails.ID = vid;

            DataSet _dsV = new DataSet();
            List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetVendorRolDetails";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorRolDetails);

                _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
                _dsV = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
                _dsV.Tables[0].Columns.Remove("Type");
                _dsV.Tables[0].Columns["VType"].ColumnName = "Type";
                _dsV.Tables[0].Columns["Vendor1099"].ColumnName = "1099";
                _dsV.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                //_dsV.Tables[0].Columns["Remit"].ColumnName = "RemitAddress";
            }
            else
            {
                _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
            }

            string vendAddress = "";
            string vendAddress2 = "";
            if (_dsV.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Address"].ToString()))
                {
                    vendAddress = _dsV.Tables[0].Rows[0]["Address"].ToString() + ", ";
                }

                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                {
                    vendAddress2 += _dsV.Tables[0].Rows[0]["City"].ToString();
                }
                if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["State"].ToString()) || !string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["Zip"].ToString()))
                {
                    if (!string.IsNullOrEmpty(_dsV.Tables[0].Rows[0]["City"].ToString()))
                    {
                        vendAddress2 += ", ";
                    }
                    vendAddress2 += _dsV.Tables[0].Rows[0]["State"].ToString() + " " + _dsV.Tables[0].Rows[0]["Zip"].ToString();
                }
            }
            _drC = _dtCheck.NewRow();
            //_drC["Pay"] = ViewState["Dollar"].ToString();
            if (Convert.ToDouble(AmountPay) > 1000)
            {
                _drC["Pay"] = ConvertNumberToCurrency(AmountPay);
                //_drC["Pay"] = ViewState["Dollar"].ToString();
            }
            else
            {
                string dollar = ConvertNumberToCurrency(AmountPay);
                _drC["Pay"] = dollar + " Dollars";
            }
            _drC["ToOrder"] = ViewState["Vendor"].ToString();
            _drC["Date"] = txtDate.Text;
            //_drC["CheckAmount"] = Convert.ToDouble(ViewState["Amount"]).ToString("0.00", CultureInfo.InvariantCulture);
            _drC["CheckAmount"] = Convert.ToDouble(AmountPay).ToString("0.00", CultureInfo.InvariantCulture);
            _drC["ToOrderAddress"] = vendAddress;
            _drC["State"] = vendAddress2;
            _dtCheck.Rows.Add(_drC);

            DataSet dsC = new DataSet();

            //ViewState["Checkno"] = lblNextCheck.Text;
            objPropUser.ConnConfig = Session["config"].ToString();
            _getConnectionConfig.ConnConfig = Session["config"].ToString();
            _getControlBranch.ConnConfig = Session["config"].ToString();

            if (Session["MSM"].ToString() != "TS")
            {
                List<GetControlViewModel> _GetControlViewModel = new List<GetControlViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/CheckList_GetControl";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getConnectionConfig);

                    _GetControlViewModel = (new JavaScriptSerializer()).Deserialize<List<GetControlViewModel>>(_APIResponse.ResponseData);
                    dsC = CommonMethods.ToDataSet<GetControlViewModel>(_GetControlViewModel);
                }
                else
                {
                    dsC = objBL_User.getControl(objPropUser);
                }
            }
            else
            {
                objPropUser.LocID = Convert.ToInt32(0);
                _getControlBranch.LocID = Convert.ToInt32(0);
                List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/CheckList_GetControlBranch";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getControlBranch);

                    _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
                    dsC = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
                }
                else
                {
                    dsC = objBL_User.getControlBranch(objPropUser);
                }
            }
            ReportViewer rvChecks = new ReportViewer();
            rvChecks.LocalReport.DataSources.Clear();

            rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsInvoices", _dti));
            rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsCheck", _dtCheck));

            rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsTicket", dsC.Tables[0]));

            string reportPath = "Reports/MaddReportCheck.rdlc";

            rvChecks.LocalReport.ReportPath = reportPath;

            rvChecks.LocalReport.EnableExternalImages = true;
            List<Microsoft.Reporting.WebForms.ReportParameter> param1 = new List<Microsoft.Reporting.WebForms.ReportParameter>();
            string strPath = "file:///" + Server.MapPath(Request.ApplicationPath).Replace("\\", "/");
            param1.Add(new Microsoft.Reporting.WebForms.ReportParameter("Path", strPath + "/images/Company_logo.jpg"));

            rvChecks.LocalReport.SetParameters(param1);

            rvChecks.LocalReport.Refresh();

            byte[] buffer = null;
            buffer = ExportReportToPDF("", rvChecks);
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=MaddCheckBBN.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (buffer.Length).ToString());
            Response.BinaryWrite(buffer);
            Response.Flush();
            Response.Close();

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }
    private void FillDataSetReport2()
    {


        if (ddlVendor.SelectedValue == "-1")
        {
            DataTable dt = (DataTable)Session["dsbills"];
            DataTable dtNew = new DataTable();
            dtNew.Columns.Add("Name");
            dtNew.Columns.Add("Vendor");
            foreach (DataRow drow in dt.Rows)
            {
                DataRow drNew = dtNew.NewRow();
                drNew["Name"] = drow["Name"].ToString();
                drNew["Vendor"] = drow["Vendor"].ToString();
                dtNew.Rows.Add(drNew);
            }

            DataTable dtN = dtNew.DefaultView.ToTable(true);
            DataTable _dtAcct = new DataTable();
            int count = 0;
            foreach (DataRow dr in dtN.Rows)
            {
                bool isChecked = false;
                CreateTableInvoice();
                CreateTablePayee();
                CreateTableBank();
                //DataTable _dtAcct = new DataTable();
                _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
                _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

                DataRow _dri = null;
                DataRow _drC = null;

                int vid = Convert.ToInt32(dr["Vendor"].ToString());
                //ac     int vid = Convert.ToInt32(ddlVendor.SelectedValue);
                //RAHIL'S IMPLEMENTATION
                DataRow _drB = null;
                DataRow _drA = null;
                double AmountPay = 0.00;
                long checkNo = long.Parse(ViewState["Checkno"].ToString());

                // foreach (GridViewRow gr in gvBills.Rows)
                foreach (GridDataItem gr in gvBills.Items)
                {
                    Label lblVendor = (Label)gr.FindControl("lblVendor");
                    if (lblVendor.Text == dr["Name"].ToString())
                    {
                        CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

                        if (chkSelect.Checked == true)
                        {
                            isChecked = true;
                            TextBox txtGvPay = (TextBox)gr.FindControl("txtGvPay");

                            Label lblBalance = (Label)gr.FindControl("lblBalance");
                            TextBox txtGvDisc = (TextBox)gr.FindControl("txtGvDisc");
                            Label lblOrig = (Label)gr.FindControl("lblOrig");
                            HiddenField hdnRef = (HiddenField)gr.FindControl("hdnRef");
                            Label lblBillfdesc = (Label)gr.FindControl("lblBillfdesc");
                            TextBox txtGvDesc = (TextBox)gr.FindControl("txtGvDesc");
                            Label lblfDate = (Label)gr.FindControl("lblfDate");
                            Label lblRef = (Label)gr.FindControl("lblRef");

                            _dri = dti.NewRow();
                            _dri["Ref"] = hdnRef.Value;
                            //_dri["Description"] = lblBillfdesc.Text;
                            _dri["Description"] = txtGvDesc.Text;
                            _dri["InvoiceDate"] = lblfDate.Text;
                            _dri["Reference"] = lblRef.Text;
                            _dri["Total"] = double.Parse(lblOrig.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                          NumberStyles.AllowThousands |
                                          NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                            _dri["Disc"] = Convert.ToDouble(txtGvDisc.Text).ToString();
                            _dri["AmountPay"] = Convert.ToDouble(txtGvPay.Text).ToString();
                            AmountPay = AmountPay + Convert.ToDouble(txtGvPay.Text);
                            _dri["PayDate"] = txtDate.Text;
                            //_dri["CheckNo"] = ViewState["Checkno"].ToString();
                            if (!string.IsNullOrEmpty(txtNextCheck.Text))
                            {
                                checkNo = long.Parse(txtNextCheck.Text);
                            }
                            else
                            {
                                checkNo = long.Parse(ViewState["Checkno"].ToString());
                            }
                            //if (ddlPayment.SelectedValue == "0")
                            //{
                            //    _dri["CheckNo"] = checkNo + count;
                            //}
                            //else
                            //{
                            //    _dri["CheckNo"] = checkNo;
                            //}
                            
                                _dri["CheckNo"] = checkNo + count;
                            
                            //if (!string.IsNullOrEmpty(txtNextCheck.Text))
                            //{
                            //    _dri["CheckNo"] = txtNextCheck.Text;
                            //}
                            //else
                            //{
                            //    _dri["CheckNo"] = ViewState["Checkno"].ToString();
                            //}
                            // ac_dri["VendorID"] = Convert.ToInt32(ddlVendor.SelectedValue);
                            _dri["VendorID"] = dr["Vendor"];
                            //ac _dri["VendorName"] = ddlVendor.SelectedItem.Text;
                            _dri["VendorName"] = lblVendor.Text;
                            dti.Rows.Add(_dri);


                        }
                    }
                }
                while (AmountPay < 0)
                {
                    List<DataRow> rowsWantToDelete = new List<DataRow>();
                    foreach (DataRow drow in dti.Rows)
                    {
                        if (Convert.ToDouble(drow["AmountPay"].ToString()) < 0)
                        {
                            rowsWantToDelete.Add(drow);
                            foreach (DataRow rows in rowsWantToDelete)
                            {
                                dti.Rows.Remove(rows);
                            }
                            AmountPay = dti.AsEnumerable().Sum(x => Convert.ToDouble(x["AmountPay"]));
                            break;
                        }
                    }
                }
                if (isChecked)
                {
                    _objVendor.ConnConfig = Session["config"].ToString();
                    _objVendor.ID = vid;

                    _getVendorRolDetails.ConnConfig = Session["config"].ToString();
                    _getVendorRolDetails.ID = vid;

                    DataSet _dsV = new DataSet();
                    List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                    //if (IsAPIIntegrationEnable == "YES")
                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                    {
                        string APINAME = "ManageChecksAPI/CheckList_GetVendorRolDetails";

                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorRolDetails);

                        _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
                        _dsV = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
                        _dsV.Tables[0].Columns.Remove("Type");
                        _dsV.Tables[0].Columns["VType"].ColumnName = "Type";
                        _dsV.Tables[0].Columns["Vendor1099"].ColumnName = "1099";
                        _dsV.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                        //_dsV.Tables[0].Columns["Remit"].ColumnName = "RemitAddress";
                    }
                    else
                    {
                        _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
                    }

                    if (dti.Rows.Count > 0)
                    {
                        string _amount = String.Format("{0:c}", Convert.ToDouble(AmountPay));
                        _amount = _amount.Replace("$", string.Empty);
                        _drC = dtpay.NewRow();

                        //if (Convert.ToDouble(ViewState["Amount"]) > 1000)
                        if (Convert.ToDouble(AmountPay) > 1000)
                        {
                            _drC["Pay"] = ConvertNumberToCurrency(AmountPay);
                            //_drC["Pay"] = ViewState["Dollar"].ToString();
                        }
                        else
                        {
                            string dollar = ConvertNumberToCurrency(AmountPay);
                            _drC["Pay"] = dollar + " Dollars";
                        }
                        _drC["ToOrder"] = dr["Name"].ToString();
                        //_drC["ToOrder"] = ViewState["Vendor"].ToString();
                        _drC["Date"] = txtDate.Text;
                        //_drC["CheckAmount"] = _amount;
                        _drC["CheckAmount"] = Convert.ToDouble(AmountPay).ToString("0.00", CultureInfo.InvariantCulture);
                        _drC["VendorAddress"] = _dsV.Tables[0].Rows[0]["VendorAddress"];                // change by Mayuri on 8th nov,16
                        _drC["RemitAddress"] = _dsV.Tables[0].Rows[0]["RemitAddress"];                  // change by Mayuri on 8th nov,16
                        dtpay.Rows.Add(_drC);
                        long checkno = 0;
                        //RAHIL'S IMPLEMENTATION
                        //_objCD.ConnConfig = Session["config"].ToString();
                        //if (ddlPayment.SelectedValue == "0")
                        //{
                        //    checkno = Convert.ToInt32(ViewState["Checkno"]) + count;
                        //}
                        //else
                        //{
                        //    checkno = Convert.ToInt32(ViewState["Checkno"]);
                        //}
                        
                            checkno = long.Parse (ViewState["Checkno"].ToString()) + count;
                        

                        _objCD.Ref = checkno;

                        _objBank.ConnConfig = Session["config"].ToString();
                        _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);

                        _getBankCD.ConnConfig = Session["config"].ToString();
                        _getBankCD.ID = Convert.ToInt32(ddlBank.SelectedValue);
                        DataSet _dsB = new DataSet();

                        List<BankViewModel> _lstBankViewModel = new List<BankViewModel>();

                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "ManageChecksAPI/AddCheck_GetBankCD";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getBankCD);

                            _lstBankViewModel = (new JavaScriptSerializer()).Deserialize<List<BankViewModel>>(_APIResponse.ResponseData);
                            _dsB = CommonMethods.ToDataSet<BankViewModel>(_lstBankViewModel);
                            _dsB.Tables[0].Columns["RolName"].ColumnName = "Name";
                            _dsB.Tables[0].Columns["RolAddress"].ColumnName = "Address";
                            _dsB.Tables[0].Columns["RolState"].ColumnName = "State";
                            _dsB.Tables[0].Columns["RolCity"].ColumnName = "City";
                            _dsB.Tables[0].Columns["RolZip"].ColumnName = "Zip";
                        }
                        else
                        {
                            _dsB = _objBLBill.GetBankCD(_objBank);
                        }

                        _drB = dtBank.NewRow();
                        if (_dsB.Tables[0].Rows.Count > 0)
                        {
                            _drB["Name"] = _dsB.Tables[0].Rows[0]["Name"].ToString();
                            _drB["Address"] = _dsB.Tables[0].Rows[0]["Address"].ToString();
                            _drB["State"] = _dsB.Tables[0].Rows[0]["State"].ToString();
                            _drB["City"] = _dsB.Tables[0].Rows[0]["City"].ToString();
                            _drB["Zip"] = _dsB.Tables[0].Rows[0]["Zip"].ToString();
                            _drB["NBranch"] = _dsB.Tables[0].Rows[0]["NBranch"].ToString();
                            _drB["NAcct"] = _dsB.Tables[0].Rows[0]["NAcct"].ToString();
                            _drB["NRoute"] = _dsB.Tables[0].Rows[0]["NRoute"].ToString();
                            //_drB["Ref"] = _dsB.Tables[0].Rows[0]["Ref"].ToString();
                            //_dtBank.Rows.Add(_drB);
                        }
                        string checkNumber = string.Empty;
                        if (!string.IsNullOrEmpty(txtNextCheck.Text))
                        {
                            checkNumber = checkno.ToString();
                        }
                        else
                        {
                            checkNumber = checkno.ToString();
                        }

                        if (checkNumber.Length == 1)
                        {
                            _drB["Ref"] = "00000000" + checkNumber;
                        }
                        else if (checkNumber.Length == 2)
                        {
                            _drB["Ref"] = "0000000" + checkNumber;
                        }
                        else if (checkNumber.Length == 3)
                        {
                            _drB["Ref"] = "000000" + checkNumber;
                        }
                        else if (checkNumber.Length == 4)
                        {
                            _drB["Ref"] = "00000" + checkNumber;
                        }
                        else if (checkNumber.Length == 5)
                        {
                            _drB["Ref"] = "0000" + checkNumber;
                        }
                        else if (checkNumber.Length == 6)
                        {
                            _drB["Ref"] = "000" + checkNumber;
                        }
                        else if (checkNumber.Length == 7)
                        {
                            _drB["Ref"] = "00" + checkNumber;
                        }
                        else if (checkNumber.Length == 8)
                        {
                            _drB["Ref"] = "0" + checkNumber;
                        }
                        else
                        {
                            _drB["Ref"] = "000000000";
                        }

                        dtBank.Rows.Add(_drB);

                        _objVendor.ConnConfig = Session["config"].ToString();
                        _objVendor.ID = vid;
                        _getVendorAcct.ConnConfig = Session["config"].ToString();
                        _getVendorAcct.ID = vid;

                        DataSet _dsA = new DataSet();
                        List<GetVendorAcctList> _lstGetVendorAcct = new List<GetVendorAcctList>();

                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "ManageChecksAPI/AddCheck_GetVendorAcct";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorAcct);

                            _lstGetVendorAcct = (new JavaScriptSerializer()).Deserialize<List<GetVendorAcctList>>(_APIResponse.ResponseData);
                            _dsA = CommonMethods.ToDataSet<GetVendorAcctList>(_lstGetVendorAcct);
                            _dsA.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                        }
                        else
                        {
                            _dsA = _objBLVendor.GetVendorAcct(_objVendor);
                        }

                        _drA = _dtAcct.NewRow();
                        _drA["VendorID"] = _dsA.Tables[0].Rows[0]["ID"].ToString();
                        _drA["VendorAcct"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
                        _dtAcct.Rows.Add(_drA);
                        //-----------------------------------------------



                        var totalRows = dti.Rows.Count;
                        var firstHalf = dti.AsEnumerable().Take(9).CopyToDataTable();
                        var secondHalf = dti.Clone();
                        if (totalRows > 9)
                        {
                            secondHalf = dti.AsEnumerable().Skip(9).Take(totalRows - 9).CopyToDataTable();
                        }

                        DataTable _dti = new DataTable();

                        DataSet dsC = new DataSet();

                        ReportViewer rvChecks = new ReportViewer();
                        rvChecks.LocalReport.DataSources.Clear();
                        rvChecks.ProcessingMode = ProcessingMode.Local;
                        rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsInvoices", firstHalf));
                        rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsCheck", dtpay));
                        //RAHIL'S
                        rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsBank", dtBank));
                        rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsAcct", _dtAcct));
                        //--------------------------------
                        string reportPath = "Reports/MaddCheckTemplate2.rdlc";
                        rvChecks.LocalReport.ReportPath = reportPath;
                        rvChecks.LocalReport.Refresh();
                        array = ExportReportToPDF("", rvChecks);
                        lstbyte.Add(array);

                        if (totalRows > 9)
                        {
                            ReportViewer rvChecksNew = new ReportViewer();
                            rvChecksNew.LocalReport.DataSources.Clear();
                            rvChecksNew.ProcessingMode = ProcessingMode.Local;
                            rvChecksNew.LocalReport.DataSources.Add(new ReportDataSource("dsInvoices", secondHalf));
                            rvChecksNew.LocalReport.DataSources.Add(new ReportDataSource("dsCheck", dtpay));
                            //RAHIL'S
                            rvChecksNew.LocalReport.DataSources.Add(new ReportDataSource("dsBank", dtBank));
                            rvChecksNew.LocalReport.DataSources.Add(new ReportDataSource("dsAcct", _dtAcct));
                            //--------------------------------
                            string reportPathNew = "Reports/MaddCheckTemplate2New.rdlc";
                            rvChecksNew.LocalReport.ReportPath = reportPathNew;
                            rvChecksNew.LocalReport.Refresh();
                            arrayNew = ExportReportToPDF("", rvChecksNew);
                            lstbyteNew.Add(arrayNew);
                        }

                    }
                    count++;
                }

                _dtAcct.Reset();
                dti.Reset();
                dtpay.Reset();
                dtBank.Reset();

            }
            byte[] finalbyte = null;

            if (lstbyteNew.Count != 0)
            {
                finalbyte = WriteChecks.concatAndAddContentFinal(lstbyte, lstbyteNew);
            }
            else
            {
                finalbyte = WriteChecks.concatAndAddContent(lstbyte);
            }
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment;filename=TopDetailCheckCub.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (finalbyte.Length).ToString());
            Response.BinaryWrite(finalbyte);
        }




        else
        {

            CreateTableInvoice();
            CreateTablePayee();
            CreateTableBank();
            DataTable _dtAcct = new DataTable();
            _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
            _dtAcct.Columns.Add(new DataColumn("VendorAcct", typeof(string)));

            DataRow _dri = null;
            DataRow _drC = null;
            int vid = Convert.ToInt32(ddlVendor.SelectedValue);
            //RAHIL'S IMPLEMENTATION
            DataRow _drB = null;
            DataRow _drA = null;
            double AmountPay = 0.00;
            //foreach (GridViewRow gr in gvBills.Rows)
            foreach (GridDataItem gr in gvBills.Items)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

                if (chkSelect.Checked == true)
                {
                    TextBox txtGvPay = (TextBox)gr.FindControl("txtGvPay");
                    Label lblBalance = (Label)gr.FindControl("lblBalance");
                    TextBox txtGvDisc = (TextBox)gr.FindControl("txtGvDisc");
                    Label lblOrig = (Label)gr.FindControl("lblOrig");
                    HiddenField hdnRef = (HiddenField)gr.FindControl("hdnRef");

                    Label lblBillfdesc = (Label)gr.FindControl("lblBillfdesc");
                    TextBox txtGvDesc = (TextBox)gr.FindControl("txtGvDesc");
                    Label lblfDate = (Label)gr.FindControl("lblfDate");
                    Label lblRef = (Label)gr.FindControl("lblRef");

                    _dri = dti.NewRow();
                    _dri["Ref"] = hdnRef.Value;
                    //_dri["Description"] = lblBillfdesc.Text;
                    _dri["Description"] = txtGvDesc.Text;
                    _dri["InvoiceDate"] = lblfDate.Text;
                    _dri["Reference"] = lblRef.Text;
                    _dri["Total"] = double.Parse(lblOrig.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign);
                    _dri["Disc"] = Convert.ToDouble(txtGvDisc.Text).ToString();
                    _dri["AmountPay"] = Convert.ToDouble(txtGvPay.Text).ToString();
                    AmountPay = AmountPay + Convert.ToDouble(txtGvPay.Text);
                    _dri["PayDate"] = txtDate.Text;
                    //_dri["CheckNo"] = ViewState["Checkno"].ToString();
                    if (!string.IsNullOrEmpty(txtNextCheck.Text))
                    {
                        _dri["CheckNo"] = txtNextCheck.Text;
                    }
                    else
                    {
                        _dri["CheckNo"] = ViewState["Checkno"].ToString();
                    }
                    _dri["VendorID"] = Convert.ToInt32(ddlVendor.SelectedValue);
                    _dri["VendorName"] = ddlVendor.SelectedItem.Text;
                    dti.Rows.Add(_dri);
                }
            }

            _objVendor.ConnConfig = Session["config"].ToString();
            _objVendor.ID = vid;

            _getVendorRolDetails.ConnConfig = Session["config"].ToString();
            _getVendorRolDetails.ID = vid;

            DataSet _dsV = new DataSet();
            List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetVendorRolDetails";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorRolDetails);

                _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
                _dsV = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
                _dsV.Tables[0].Columns.Remove("Type");
                _dsV.Tables[0].Columns["VType"].ColumnName = "Type";
                _dsV.Tables[0].Columns["Vendor1099"].ColumnName = "1099";
                _dsV.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                //_dsV.Tables[0].Columns["Remit"].ColumnName = "RemitAddress";
            }
            else
            {
                _dsV = _objBLVendor.GetVendorRolDetails(_objVendor);
            }


            //string _amount = String.Format("{0:c}", Convert.ToDouble(ViewState["Amount"]));
            string _amount = String.Format("{0:c}", Convert.ToDouble(AmountPay));
            _amount = _amount.Replace("$", string.Empty);
            _drC = dtpay.NewRow();

            //if (Convert.ToDouble(ViewState["Amount"]) > 1000)
            if (Convert.ToDouble(AmountPay) > 1000)
            {
                //_drC["Pay"] = ViewState["Dollar"].ToString();
                _drC["Pay"] = ConvertNumberToCurrency(AmountPay);
            }
            else
            {
                string dollar = ConvertNumberToCurrency(AmountPay);
                _drC["Pay"] = dollar + " Dollars";
            }
            
            _drC["ToOrder"] = ViewState["Vendor"].ToString();
            _drC["Date"] = txtDate.Text;
            //_drC["CheckAmount"] = _amount;            
            _drC["CheckAmount"] = Convert.ToDouble(AmountPay).ToString("0.00", CultureInfo.InvariantCulture);
            _drC["VendorAddress"] = _dsV.Tables[0].Rows[0]["VendorAddress"];                // change by Mayuri on 8th nov,16
            _drC["RemitAddress"] = _dsV.Tables[0].Rows[0]["RemitAddress"];                  // change by Mayuri on 8th nov,16
            dtpay.Rows.Add(_drC);

            //RAHIL'S IMPLEMENTATION
            //_objCD.ConnConfig = Session["config"].ToString();
            long checkno = long.Parse (ViewState["Checkno"].ToString());
            _objCD.Ref = checkno;

            _objBank.ConnConfig = Session["config"].ToString();
            _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);

            _getBankCD.ConnConfig = Session["config"].ToString();
            _getBankCD.ID = Convert.ToInt32(ddlBank.SelectedValue);
            DataSet _dsB = new DataSet();

            List<BankViewModel> _lstBankViewModel = new List<BankViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/AddCheck_GetBankCD";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getBankCD);

                _lstBankViewModel = (new JavaScriptSerializer()).Deserialize<List<BankViewModel>>(_APIResponse.ResponseData);
                _dsB = CommonMethods.ToDataSet<BankViewModel>(_lstBankViewModel);
                _dsB.Tables[0].Columns["RolName"].ColumnName = "Name";
                _dsB.Tables[0].Columns["RolAddress"].ColumnName = "Address";
                _dsB.Tables[0].Columns["RolState"].ColumnName = "State";
                _dsB.Tables[0].Columns["RolCity"].ColumnName = "City";
                _dsB.Tables[0].Columns["RolZip"].ColumnName = "Zip";
            }
            else
            {
                _dsB = _objBLBill.GetBankCD(_objBank);
            }

            _drB = dtBank.NewRow();
            if (_dsB.Tables[0].Rows.Count > 0)
            {
                _drB["Name"] = _dsB.Tables[0].Rows[0]["Name"].ToString();
                _drB["Address"] = _dsB.Tables[0].Rows[0]["Address"].ToString();
                _drB["State"] = _dsB.Tables[0].Rows[0]["State"].ToString();
                _drB["City"] = _dsB.Tables[0].Rows[0]["City"].ToString();
                _drB["Zip"] = _dsB.Tables[0].Rows[0]["Zip"].ToString();
                _drB["NBranch"] = _dsB.Tables[0].Rows[0]["NBranch"].ToString();
                _drB["NAcct"] = _dsB.Tables[0].Rows[0]["NAcct"].ToString();
                _drB["NRoute"] = _dsB.Tables[0].Rows[0]["NRoute"].ToString();
                //_drB["Ref"] = _dsB.Tables[0].Rows[0]["Ref"].ToString();
                //_dtBank.Rows.Add(_drB);
            }
            string checkNumber = string.Empty;
            if (!string.IsNullOrEmpty(txtNextCheck.Text))
            {
                checkNumber = txtNextCheck.Text;
            }
            else
            {
                checkNumber = ViewState["Checkno"].ToString();
            }

            if (checkNumber.Length == 1)
            {
                _drB["Ref"] = "00000000" + checkNumber;
            }
            else if (checkNumber.Length == 2)
            {
                _drB["Ref"] = "0000000" + checkNumber;
            }
            else if (checkNumber.Length == 3)
            {
                _drB["Ref"] = "000000" + checkNumber;
            }
            else if (checkNumber.Length == 4)
            {
                _drB["Ref"] = "00000" + checkNumber;
            }
            else if (checkNumber.Length == 5)
            {
                _drB["Ref"] = "0000" + checkNumber;
            }
            else if (checkNumber.Length == 6)
            {
                _drB["Ref"] = "000" + checkNumber;
            }
            else if (checkNumber.Length == 7)
            {
                _drB["Ref"] = "00" + checkNumber;
            }
            else if (checkNumber.Length == 8)
            {
                _drB["Ref"] = "0" + checkNumber;
            }
            else
            {
                _drB["Ref"] = "000000000";
            }

            dtBank.Rows.Add(_drB);

            _objVendor.ConnConfig = Session["config"].ToString();
            _objVendor.ID = vid;

            _getVendorAcct.ConnConfig = Session["config"].ToString();
            _getVendorAcct.ID = vid;

            DataSet _dsA = new DataSet();
            List<GetVendorAcctList> _lstGetVendorAcct = new List<GetVendorAcctList>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/AddCheck_GetVendorAcct";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendorAcct);

                _lstGetVendorAcct = (new JavaScriptSerializer()).Deserialize<List<GetVendorAcctList>>(_APIResponse.ResponseData);
                _dsA = CommonMethods.ToDataSet<GetVendorAcctList>(_lstGetVendorAcct);
                _dsA.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
            }
            else
            {
                _dsA = _objBLVendor.GetVendorAcct(_objVendor);
            }

            _drA = _dtAcct.NewRow();
            _drA["VendorID"] = _dsA.Tables[0].Rows[0]["ID"].ToString();
            _drA["VendorAcct"] = _dsA.Tables[0].Rows[0]["Acct#"].ToString();
            _dtAcct.Rows.Add(_drA);
            //-----------------------------------------------

            DataSet dsC = new DataSet();
            ReportViewer rvChecks = new ReportViewer();
            rvChecks.LocalReport.DataSources.Clear();
            rvChecks.ProcessingMode = ProcessingMode.Local;
            rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsInvoices", dti));
            rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsCheck", dtpay));

            //RAHIL'S
            rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsBank", dtBank));
            rvChecks.LocalReport.DataSources.Add(new ReportDataSource("dsAcct", _dtAcct));
            //--------------------------------

            string reportPath = "Reports/MaddCheckTemplate2.rdlc";


            rvChecks.LocalReport.ReportPath = reportPath;
            rvChecks.LocalReport.Refresh();

            byte[] buffer = null;
            buffer = ExportReportToPDF("", rvChecks);
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=TopDetailCheckCub.pdf");
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Length", (buffer.Length).ToString());
            Response.BinaryWrite(buffer);
            Response.Flush();
            Response.Close();
            //---------------------------------------------------
        }

    }
    List<byte[]> lstbyte = new List<byte[]>();
    List<byte[]> lstbyteNew = new List<byte[]>();
    protected void imgPrintTemp6_Click(object sender, ImageClickEventArgs e)
    {                                                                           //              MADDEN – check top 
        try
        {
            //F:\ESS\ESSMOM\MOM\MOM - NewDesign\MSWeb\StimulsoftReports\APChecks\APTopCheck
            byte[] buffer1 = null;
            string reportPathStimul = Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + ddlTopChecksForLoad.SelectedItem.Text.Trim() + ".mrt");
            StiReport report = new StiReport();
            //  FillDataSetReport2();
            // report = FillMaddenDataSetForReport("TopCheckReportDefault");
            //report = FillMaddenDataSetForReport(ddlTopChecksForLoad.SelectedItem.Text.Trim());
            //var settings = new Stimulsoft.Report.Export.StiPdfExportSettings();
            //var service = new Stimulsoft.Report.Export.StiPdfExportService();
            //System.IO.MemoryStream stream = new System.IO.MemoryStream();
            //service.ExportTo(report, stream, settings);
            //buffer1 = stream.ToArray();

            //string filename = Path.Combine(Server.MapPath(Request.ApplicationPath) + "\\TempPDF", "TopCheckReport.pdf");

            //if (buffer1 != null)
            //{
            //    if (File.Exists(filename))
            //        File.Delete(filename);
            //    using (var fs = new FileStream(filename, FileMode.Create))
            //    {
            //        fs.Write(buffer1, 0, buffer1.Length);
            //        fs.Close();
            //    }
            //}

            //Response.ClearContent();
            //Response.ClearHeaders();
            //Response.AddHeader("Content-Disposition", "attachment;filename=PrintCheck.pdf");
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("Content-Length", (buffer1.Length).ToString());
            //Response.BinaryWrite(buffer1);
            //Response.Flush();
            //Response.Close();
            if (Request.QueryString["bill"] == null)
            {
                FillReportMaddenDataSet(ddlTopChecksForLoad.SelectedItem.Text.Trim());
                _objUser.ConnConfig = Session["config"].ToString();
                _objUser.MOMUSer = Session["Username"].ToString();
                _objUser.UserLic = ddlTopChecksForLoad.SelectedItem.Text;

                _updateCheckTemplate.ConnConfig = Session["config"].ToString();
                _updateCheckTemplate.MOMUSer = Session["Username"].ToString();
                _updateCheckTemplate.UserLic = ddlTopChecksForLoad.SelectedItem.Text;

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/AddCheck_updateCheckTemplate";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateCheckTemplate);
                }
                else
                {
                    _objBLBill.updateCheckTemplate(_objUser);
                }
            }
            else if (Request.QueryString["bill"] == "c")
            {
                if (Request.QueryString["vid"] != null)
                {
                    FillReportMaddenDataSet(ddlTopChecksForLoad.SelectedItem.Text.Trim());
                }
                else
                {
                    FillReportMaddenDataSet_New(ddlTopChecksForLoad.SelectedItem.Text.Trim());
                }
                _objUser.ConnConfig = Session["config"].ToString();
                _objUser.MOMUSer = Session["Username"].ToString();
                _objUser.UserLic = ddlTopChecksForLoad.SelectedItem.Text;

                _updateCheckTemplate.ConnConfig = Session["config"].ToString();
                _updateCheckTemplate.MOMUSer = Session["Username"].ToString();
                _updateCheckTemplate.UserLic = ddlTopChecksForLoad.SelectedItem.Text;

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/AddCheck_updateCheckTemplate";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateCheckTemplate);
                }
                else
                {
                    _objBLBill.updateCheckTemplate(_objUser);
                }
                
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }


    #endregion
    private byte[] Combine(byte[] a, byte[] b)
    {
        byte[] c = new byte[a.Length + b.Length];
        System.Buffer.BlockCopy(a, 0, c, 0, a.Length);
        System.Buffer.BlockCopy(b, 0, c, a.Length, b.Length);
        return c;
    }
    #region Custom Functions
    private void ResetForm()
    {
        try
        {
            divSuccess.Visible = false;
            lblVendorBal.Text = string.Format("{0:c}", 0);
            lblSelectedPayment.Text = string.Format("{0:c}", 0);
            lblTotalDiscount.Text = string.Format("{0:c}", 0);
            lblRequirement.Text = string.Format("{0:c}", 0);
            lblCurrentBal.Text = string.Format("{0:c}", 0);
            lblCheckEndingBalance.Text = string.Format("{0:c}", 0);
            lblRunBalance.Text = string.Format("{0:c}", 0);
            
            //txtAmount.Text = "0.00";
            txtDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            ViewState["Checkno"] = 0;
            txtSearchDate.Visible = false;
            gvBills.DataSource = "";
            gvBills.DataBind();
            Session.Remove("selectedItems");
            GetRunningBalance();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
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

        DataTable dt = new DataTable();
        dt = GetUserById();

        string BillPayPermission = dt.Rows[0]["BillPay"] == DBNull.Value ? "NNNNNN" : dt.Rows[0]["BillPay"].ToString();

        string ShowBankBalancesPermission = BillPayPermission.Length < 5 ? "Y" : BillPayPermission.Substring(4, 1);
        if (ShowBankBalancesPermission.ToUpper() == "Y")
        {
            divCurrentBalance.Visible = true;
            divEndingBalance.Visible = true;
            //divCurrentBalance.Style["display"] = "inline-block";
            //divEndingBalance.Style["display"] = "inline-block";
        }
        else
        {
            divCurrentBalance.Visible = false;
            divEndingBalance.Visible = false;
            //divCurrentBalance.Style["display"] = "none";
            //divEndingBalance.Style["display"] = "none";
        }


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
            ViewState["CompPermission"] = 1;
            dvCompanyPermission.Visible = true;
            FillCompany();
        }
        else
        {
            ViewState["CompPermission"] = 0;
            dvCompanyPermission.Visible = false;
        }
    }

    private void FillCompany()
    {
        objCompany.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();

        _getCompanyByCustomer.UserID = Convert.ToInt32(Session["UserID"].ToString());
        _getCompanyByCustomer.DBName = Session["dbname"].ToString();
        _getCompanyByCustomer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();

        List<CompanyOfficeViewModel> _CompanyOfficeViewModel = new List<CompanyOfficeViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/AddCheck_GetCompanyByCustomer";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCompanyByCustomer);

            _CompanyOfficeViewModel = (new JavaScriptSerializer()).Deserialize<List<CompanyOfficeViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<CompanyOfficeViewModel>(_CompanyOfficeViewModel);
        }
        else
        {
            ds = objBL_Company.getCompanyByCustomer(objCompany);
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlCompany.DataSource = ds.Tables[0];
            ddlCompany.DataTextField = "Name";
            ddlCompany.DataValueField = "CompanyID";
            ddlCompany.DataBind();
            ddlCompany.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));

            //ddlCompanyEdit.DataSource = ds.Tables[0];
            //ddlCompanyEdit.DataTextField = "Name";
            //ddlCompanyEdit.DataValueField = "CompanyID";
            //ddlCompanyEdit.DataBind();
            //ddlCompanyEdit.Items.Insert(0, new ListItem("Select", "0"));

        }
        if (!string.IsNullOrEmpty(GetDefaultCompany()))
            ddlCompany.Items.FindByText(GetDefaultCompany()).Selected = true;
        else
        {
            if (ddlCompany.Items.Count > 1)
                ddlCompany.SelectedIndex = 1;
        }
        LoadDataBasedOnCompany();
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
    private byte[] ExportReportToPDF(string reportName, ReportViewer ReportViewer1)
    {
        Warning[] warnings;
        string[] streamids;
        string mimeType;
        string encoding;
        string filenameExtension;

        byte[] bytes = ReportViewer1.LocalReport.Render(
            "PDF", null, out mimeType, out encoding, out filenameExtension,
             out streamids, out warnings);
        return bytes;
    }
    private void FillBank()
    {
        try
        {
            if (Session["COPer"].ToString() == "1")
            {
                //do nothing
            }
            else
            {
                _objBank.ConnConfig = Session["config"].ToString();
                _getAllBankNames.ConnConfig = Session["config"].ToString();
                DataSet _dsBank = new DataSet();

                List<GetAllBankNamesViewModel> _lstGetAllBankNamesViewModel = new List<GetAllBankNamesViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/CheckList_GetAllBankNames";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getAllBankNames);

                    _lstGetAllBankNamesViewModel = (new JavaScriptSerializer()).Deserialize<List<GetAllBankNamesViewModel>>(_APIResponse.ResponseData);
                    _dsBank = CommonMethods.ToDataSet<GetAllBankNamesViewModel>(_lstGetAllBankNamesViewModel);
                }
                else
                {
                    _dsBank = _objBL_Bank.GetAllBankNames(_objBank);
                }

                if (_dsBank.Tables[0].Rows.Count > 0)
                {
                    ddlBank.Items.Clear();
                    ddlBank.Items.Add(new System.Web.UI.WebControls.ListItem(":: Select ::", "0"));
                    ddlBank.AppendDataBoundItems = true;

                    ddlBank.DataSource = _dsBank;
                    ddlBank.DataValueField = "ID";
                    ddlBank.DataTextField = "fDesc";
                    ddlBank.DataBind();
                }
                else
                {
                    ddlBank.Items.Add(new System.Web.UI.WebControls.ListItem("No data found", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void FillVendor()
    {
        try
        {
            if (Session["COPer"].ToString() == "1")
            {
                //Do nothing
            }
            else
            {
                _objVendor.ConnConfig = Session["config"].ToString();
                _getOpenBillVendor.ConnConfig = Session["config"].ToString();
                DataSet _dsVendor = new DataSet();

                List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/AddCheck_GetOpenBillVendor";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getOpenBillVendor);

                    _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
                    _dsVendor = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
                }
                else
                {
                    _dsVendor = _objBLVendor.GetOpenBillVendor(_objVendor);
                }

                if (ddlVendor.Items.Count > 0)
                {
                    ddlVendor.Items.Clear();
                }
                //ddlVendor.Items.Add(new ListItem(" "));
                if (_dsVendor.Tables[0].Rows.Count > 0)
                {
                    ddlVendor.Items.Add(new System.Web.UI.WebControls.ListItem(":: Select ::", "0"));
                    ddlVendor.Items.Add(new System.Web.UI.WebControls.ListItem(":: Selected Payment ::", "-1"));
                    ddlVendor.AppendDataBoundItems = true;

                    ddlVendor.DataSource = _dsVendor;
                    ddlVendor.DataValueField = "ID";
                    ddlVendor.DataTextField = "Name";
                    ddlVendor.DataBind();
                }
                else
                {
                    ddlVendor.Items.Add(new System.Web.UI.WebControls.ListItem("No data found", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillCreditVendor()
    {
        try
        {
            if (Session["COPer"].ToString() == "1")
            {
                //Do nothing
            }
            else
            {
                _objVendor.ConnConfig = Session["config"].ToString();
                _getCreditBillVendor.ConnConfig = Session["config"].ToString();
                DataSet _dsVendor = new DataSet();

                List<OpenAPViewModel> _lstOpenAPViewModel = new List<OpenAPViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/AddCheck_GetCreditBillVendor";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCreditBillVendor);

                    _lstOpenAPViewModel = (new JavaScriptSerializer()).Deserialize<List<OpenAPViewModel>>(_APIResponse.ResponseData);
                    _dsVendor = CommonMethods.ToDataSet<OpenAPViewModel>(_lstOpenAPViewModel);
                    _dsVendor.Tables[0].Columns["RolName"].ColumnName = "Name";

                }
                else
                {
                    _dsVendor = _objBLBill.GetCreditBillVendor(_objVendor);
                }

                if (ddlVendor.Items.Count > 0)
                {
                    ddlVendor.Items.Clear();
                }
                //ddlVendor.Items.Add(new ListItem(" "));
                if (_dsVendor.Tables[0].Rows.Count > 0)
                {
                    ddlVendor.Items.Add(new System.Web.UI.WebControls.ListItem(":: Select ::", "0"));
                    ddlVendor.Items.Add(new System.Web.UI.WebControls.ListItem(":: Selected Payment ::", "-1"));
                    ddlVendor.AppendDataBoundItems = true;

                    ddlVendor.DataSource = _dsVendor;
                    ddlVendor.DataValueField = "ID";
                    ddlVendor.DataTextField = "Name";
                    ddlVendor.DataBind();
                }
                else
                {
                    ddlVendor.Items.Add(new System.Web.UI.WebControls.ListItem("No data found", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    public static string ConvertNumbertoWords(int number)
    {
        if (number == 0)
            return "Zero";
        if (number < 0)
            return "minus " + ConvertNumbertoWords(Math.Abs(number));
        string words = "";
        if ((number / 1000000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000000) + " Million ";
            number %= 1000000;
        }
        if ((number / 1000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000) + " Thousand ";
            number %= 1000;
        }
        if ((number / 100) > 0)
        {
            words += ConvertNumbertoWords(number / 100) + " Hundred ";
            number %= 100;
        }
        if (number > 0)
        {
            if (words != "")
                words += "And ";
            //var unitsMap = new[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };
            //var tensMap = new[] { "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };
            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };


            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }
        return words;
    }
    public void SetBankDetails()
    {
        try
        {
            double endBal = 0;
            double pay = 0;
            _objBank.ConnConfig = Session["config"].ToString();
            _getBankByID.ConnConfig = Session["config"].ToString();

            if (ddlBank.Items.Count > 0)
            {
                _objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);
                _getBankByID.ID = Convert.ToInt32(ddlBank.SelectedValue);

                DataSet dsBank = new DataSet();
                List <GetBankByIDViewModel> _lstGetBankByID = new List<GetBankByIDViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/AddCheck_GetBankByID";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getBankByID);

                    _lstGetBankByID = (new JavaScriptSerializer()).Deserialize<List<GetBankByIDViewModel>>(_APIResponse.ResponseData);
                    dsBank = CommonMethods.ToDataSet<GetBankByIDViewModel>(_lstGetBankByID);
                }
                else
                {
                    dsBank = _objBL_Bank.GetBankByID(_objBank);
                }

                if (dsBank.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsBank.Tables[0].Rows[0];
                    if (ddlPayment.SelectedValue != "-1")
                    {
                        if (ddlPayment.SelectedValue == "0")
                        {
                            lblCheck.Text = "Next Check#";
                            checkno = !string.IsNullOrEmpty(dr["NextC"].ToString()) ? long.Parse(dr["NextC"].ToString()) : 0;
                            var nextCheck = long.Parse(checkno.ToString());
                            //AccountAutoFill accountAutoFill = new AccountAutoFill();
                            //while (!accountAutoFill.CheckNumValid(nextCheck.ToString(), ddlBank.SelectedValue))
                            //{
                            //    nextCheck++;
                            //}
                            ViewState["Checkno"] = nextCheck;
                            if ((!string.IsNullOrEmpty(checkno.ToString())))
                            {
                                txtNextCheck.Text = nextCheck.ToString();
                            }
                            ViewState["Checkno"] = txtNextCheck.Text;
                        }
                        else if (ddlPayment.SelectedValue == "1")
                        {
                            //txtNextCheck.Text = string.Empty;
                            lblCheck.Text = "Next Ref#";
                            checkno = !string.IsNullOrEmpty(dr["NextCash"].ToString()) ? long.Parse(dr["NextCash"].ToString()) : 0;
                            var nextCheck = long.Parse(checkno.ToString());
                            
                            ViewState["Checkno"] = nextCheck;
                            if ((!string.IsNullOrEmpty(checkno.ToString())))
                            {
                                txtNextCheck.Text = nextCheck.ToString();
                            }
                            ViewState["Checkno"] = txtNextCheck.Text;
                        }
                        else if (ddlPayment.SelectedValue == "2")
                        {
                            //txtNextCheck.Text = string.Empty;
                            lblCheck.Text = "Next Ref#";
                            checkno = !string.IsNullOrEmpty(dr["NextWire"].ToString()) ? long.Parse(dr["NextWire"].ToString()) : 0;
                            var nextCheck = long.Parse(checkno.ToString());

                            ViewState["Checkno"] = nextCheck;
                            if ((!string.IsNullOrEmpty(checkno.ToString())))
                            {
                                txtNextCheck.Text = nextCheck.ToString();
                            }
                            ViewState["Checkno"] = txtNextCheck.Text;
                        }
                        else if (ddlPayment.SelectedValue == "3")
                        {
                            //txtNextCheck.Text = string.Empty;
                            lblCheck.Text = "Next Ref#";
                            checkno = !string.IsNullOrEmpty(dr["NextACH"].ToString()) ? long.Parse(dr["NextACH"].ToString()) : 0;
                            var nextCheck = long.Parse(checkno.ToString());

                            ViewState["Checkno"] = nextCheck;
                            if ((!string.IsNullOrEmpty(checkno.ToString())))
                            {
                                txtNextCheck.Text = nextCheck.ToString();
                            }
                            ViewState["Checkno"] = txtNextCheck.Text;
                        }
                        else if (ddlPayment.SelectedValue == "4")
                        {
                            //txtNextCheck.Text = string.Empty;
                            lblCheck.Text = "Next Ref#";
                            checkno = !string.IsNullOrEmpty(dr["NextCC"].ToString()) ? long.Parse(dr["NextCC"].ToString()) : 0;
                            var nextCheck = long.Parse(checkno.ToString());

                            ViewState["Checkno"] = nextCheck;
                            if ((!string.IsNullOrEmpty(checkno.ToString())))
                            {
                                txtNextCheck.Text = nextCheck.ToString();
                            }
                            ViewState["Checkno"] = txtNextCheck.Text;
                        }
                        else
                        {
                            txtNextCheck.Text = string.Empty;
                            lblCheck.Text = "Next Ref#";
                        }

                    }

                    if (!string.IsNullOrEmpty(dr["BankBalance"].ToString()))
                    {
                        lblCurrentBal.Text = string.Format("{0:c}", Convert.ToDouble(dr["BankBalance"].ToString()));
                    }
                    if (!string.IsNullOrEmpty(dr["BankBalance"].ToString()))
                    {
                        endBal = Convert.ToDouble(dr["BankBalance"].ToString());
                    }

                    pay = double.Parse(lblSelectedPayment.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                  NumberStyles.AllowThousands |
                                  NumberStyles.AllowDecimalPoint);

                    //Convert.ToDouble(lblSelectedPayment.Text);
                    lblCheckEndingBalance.Text = string.Format("{0:c}", (endBal - pay));
                    if (!ddlBank.SelectedValue.Equals("0"))
                    {
                        if ((endBal - pay) < 0 && Convert.ToInt32(dr["Warn"].ToString()) == 1)
                        {
                            //ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'Please note your selected will be overdrawn.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                            ViewState["bankwarn"] = "1";
                            
                        }
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
    public double GetTotalDiscAmount()
    {
        double total = 0.00;
        try
        {
            foreach (GridDataItem gr in gvBills.Items)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    Label lblSpec = (Label)gr.FindControl("lblSpec");
                    TextBox txtGvDisc = (TextBox)gr.FindControl("txtGvDisc");
                    Int16 spec = Convert.ToInt16(lblSpec.Text);
                    if (!spec.Equals(1) && !spec.Equals(2) && !spec.Equals(3))
                    {
                        total = total + Convert.ToDouble(txtGvDisc.Text);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return total;
    }
    public bool CheckNegativeBill()
    {
        bool chk = true;
        DataTable _dtAcct = new DataTable();
        _dtAcct.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        _dtAcct.Columns.Add(new DataColumn("Name", typeof(string)));
        _dtAcct.Columns.Add(new DataColumn("PayAmount", typeof(double)));
        DataRow _dri = null;
        foreach (GridDataItem gr in gvBills.Items)
        {
            _dri = _dtAcct.NewRow();
            HiddenField hdnbillvendorid = (HiddenField)gr.FindControl("hdnbillvendorid");
            Label lblVendor = (Label)gr.FindControl("lblVendor");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            TextBox txtGvPay = (TextBox)gr.FindControl("txtGvPay");
            if (chkSelect.Checked == true)
            {
                _dri["VendorID"] = Convert.ToInt32(hdnbillvendorid.Value);
                _dri["Name"] = Convert.ToString(lblVendor.Text);
                _dri["PayAmount"] = Convert.ToDouble(txtGvPay.Text);
                _dtAcct.Rows.Add(_dri);
                _dtAcct.AcceptChanges();

            }
        }

        if (RadGrid_gvJobCostItems.Items.Count > 0)
        {

            double _totalPaybill = 0.00;
            double _totalPaysTaxbill = 0.00;
            double _totalPayGSTbill = 0.00;
            double _newbilltotalPay = 0.00;

            foreach (GridDataItem gr in RadGrid_gvJobCostItems.Items)
            {
                
                TextBox txtGvAmount = (TextBox)gr.FindControl("txtGvAmount");
                HiddenField hdnSTaxAm = (HiddenField)gr.FindControl("hdnSTaxAm");
                HiddenField hdnGSTTaxAm = (HiddenField)gr.FindControl("hdnGSTTaxAm");
                if (txtGvAmount.Text != null && txtGvAmount.Text != "")
                {
                    _totalPaybill = _totalPaybill + Convert.ToDouble(txtGvAmount.Text);
                }
                if (hdnSTaxAm.Value != null && hdnSTaxAm.Value != "")
                {
                    _totalPaysTaxbill = _totalPaysTaxbill + Convert.ToDouble(hdnSTaxAm.Value);
                }
                if (hdnGSTTaxAm.Value != null && hdnGSTTaxAm.Value != "")
                {
                    _totalPayGSTbill = _totalPayGSTbill + Convert.ToDouble(hdnGSTTaxAm.Value);
                }

            }

            _newbilltotalPay = _totalPaybill + _totalPaysTaxbill + _totalPayGSTbill;
            _dri = _dtAcct.NewRow();
            _dri["VendorID"] = Convert.ToInt32(hdnVendorID.Value);
            _dri["Name"] = Convert.ToString(txtVendor.Text);
            _dri["PayAmount"] = Convert.ToDouble(_newbilltotalPay);
            _dtAcct.Rows.Add(_dri);
            _dtAcct.AcceptChanges();
        }


        var query = from row in _dtAcct.AsEnumerable()
                    group row by row.Field<string>("Name") into grp
                    select new
                    {
                        Id = grp.Key,
                        sum = grp.Sum(r => r.Field<double>("PayAmount"))
                    };
        foreach (var grp in query)
        {
            //Response.Write(String.Format("The Sum of '{0}' is {1}", grp.Id, grp.sum));
            if (Convert.ToDouble(grp.sum) < 0)
            {
                chk = false;
                string strScript = string.Empty;
                if (Request.QueryString["bill"] == "c" && Request.QueryString["vid"] == null && Request.QueryString["ref"] == null)
                {
                    strScript += "noty({text: 'Check amount not acceptable for vendor " + grp.Id + " .', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});  $('#MOMloading').hide(); showbillgl();";
                }
                else
                {
                    strScript += "noty({text: 'Check amount not acceptable for vendor " + grp.Id + " .', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});  $('#MOMloading').hide();";
                }
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", strScript, true);
                break;
            }
            
        }
        return chk;
    }
    public void GetPaymentTotal()
    {
        try
        {
            double _totalPay = 0.00;
            double _totalDisc = 0.00;
            double _totalOrginal = 0.00;
            double _totalBalance = 0.00;

            double _newbilltotalPay = 0.00;
            
            
            foreach (GridDataItem gr in gvBills.Items)
            {
                

                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                TextBox txtGvDisc = (TextBox)gr.FindControl("txtGvDisc");
                HiddenField hdnSelected = (HiddenField)gr.FindControl("hdnSelected");
                HiddenField hdnOriginal = (HiddenField)gr.FindControl("hdnOriginal");
               
                Label lblBalance = (Label)gr.FindControl("lblBalance");
                lblBalance.Text = string.Format("{0:c}", (Convert.ToDouble(hdnOriginal.Value) - Convert.ToDouble(hdnSelected.Value) - Convert.ToDouble(txtGvDisc.Text)));
                _totalOrginal = _totalOrginal + Convert.ToDouble(hdnOriginal.Value);
                if (chkSelect.Checked == true)
                {
                    //TextBox txtGvDisc = (TextBox)gr.FindControl("txtGvDisc");
                    TextBox txtGvPay = (TextBox)gr.FindControl("txtGvPay");

                    _totalPay = _totalPay + Convert.ToDouble(txtGvPay.Text);
                    _totalDisc = _totalDisc + Convert.ToDouble(txtGvDisc.Text);
                    lblBalance.Text = string.Format("{0:c}", (Convert.ToDouble(hdnOriginal.Value) - Convert.ToDouble(hdnSelected.Value) - Convert.ToDouble(txtGvPay.Text) - Convert.ToDouble(txtGvDisc.Text)));

                    

                }
                string balns = lblBalance.Text.Replace("$", "");
                balns = balns.Replace("(", "-");
                balns = balns.Replace(")", "");
                _totalBalance = _totalBalance + Convert.ToDouble(balns);
            }
            string word = "";

            lblTotalDiscount.Text = string.Format("{0:c}", _totalDisc);
            lblSelectedPayment.Text = string.Format("{0:c}", _totalPay);

            //lblTotalAmount11.Text = string.Format("{0:c}", _totalPay);
            if (RadGrid_gvJobCostItems.Items.Count > 0)
            {

                double _totalPaybill = 0.00;
                double _totalPaysTaxbill = 0.00;
                double _totalPayGSTbill = 0.00;


                foreach (GridDataItem gr in RadGrid_gvJobCostItems.Items)
                {
                    TextBox txtGvAmount = (TextBox)gr.FindControl("txtGvAmount");
                    HiddenField hdnSTaxAm = (HiddenField)gr.FindControl("hdnSTaxAm");
                    HiddenField hdnGSTTaxAm = (HiddenField)gr.FindControl("hdnGSTTaxAm");
                    if (txtGvAmount.Text != null && txtGvAmount.Text != "")
                    {
                        _totalPaybill = _totalPaybill + Convert.ToDouble(txtGvAmount.Text);
                    }
                    if (hdnSTaxAm.Value != null && hdnSTaxAm.Value != "")
                    {
                        _totalPaysTaxbill = _totalPaysTaxbill + Convert.ToDouble(hdnSTaxAm.Value);
                    }
                    if (hdnGSTTaxAm.Value != null && hdnGSTTaxAm.Value != "")
                    {
                        _totalPayGSTbill = _totalPayGSTbill + Convert.ToDouble(hdnGSTTaxAm.Value);
                    }

                }

                _newbilltotalPay = _totalPaybill + _totalPaysTaxbill + _totalPayGSTbill;

            }
            lblTotalAmount.Text = string.Format("{0:c}", _totalPay + _newbilltotalPay);
            lblRequirement.Text = string.Format("{0:c}", _totalPay + _newbilltotalPay);
            //lblAutoSelectBalance.Text = string.Format("{0:c}", _totalPay);

            GridFooterItem footerItem = (GridFooterItem)gvBills.MasterTableView.GetItems(GridItemType.Footer)[0];
            Label lblTotalPay = (Label)footerItem.FindControl("lblTotalPay");
            Label lblTotalOrig = (Label)footerItem.FindControl("lblTotalOrig");
            Label lblTotalDisc = (Label)footerItem.FindControl("lblTotalDisc");
            Label lblTotalBalance = (Label)footerItem.FindControl("lblTotalBalance");


            lblTotalPay.Text = string.Format("{0:c}", _totalPay);
            lblTotalOrig.Text = string.Format("{0:c}", _totalOrginal);
            lblTotalDisc.Text = string.Format("{0:c}", _totalDisc);
            lblTotalBalance.Text = string.Format("{0:c}", _totalBalance);

            if (!Convert.ToInt32(_totalPay + _newbilltotalPay).Equals(0))
                word = ConvertNumberToCurrency(Convert.ToDouble(_totalPay + _newbilltotalPay));

            lblDollar.Text = word;
            ViewState["Dollar"] = word;

            ViewState["Amount"] = _totalPay + _newbilltotalPay.ToString("0.00", CultureInfo.InvariantCulture);
            if (ddlVendor.SelectedValue == "-1")
            {
                ViewState["Vendor"] = "Batch Check";
            }
            else
            {
                ViewState["Vendor"] = ddlVendor.SelectedItem.Text;
            }


            




            //foreach (GridDataItem gr in RadGrid_gvJobCostItems.Items)
            //{
            //    TextBox txtGvAmount = (TextBox)gr.FindControl("txtGvAmount");
            //    if (txtGvAmount.Text != null && txtGvAmount.Text != "")
            //    {
            //        _totalPay = _totalPay + Convert.ToDouble(txtGvAmount.Text);
            //    }
            //}
            //lblSelectedPayment.Text = string.Format("{0:c}", _totalPay);
            //lblTotalAmount.Text = string.Format("{0:c}", _totalPay);
            //lblTotalAmount11.Text = string.Format("{0:c}", _totalPay);
            //lblRequirement.Text = string.Format("{0:c}", _totalPay);

            //lblTotalPay.Text = string.Format("{0:c}", _totalPay);
            //lblTotalDisc.Text = string.Format("{0:c}", _totalDisc);

            //if (!Convert.ToInt32(_totalPay).Equals(0))
            //    word = ConvertNumberToCurrency(Convert.ToDouble(_totalPay));

            //lblDollar.Text = word;
            //ViewState["Dollar"] = word;

            //ViewState["Amount"] = _totalPay.ToString("0.00", CultureInfo.InvariantCulture);


        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    public string ConvertNumberToCurrency(double _amount)
    {
        string _currencyInWord = ConvertNumbertoWords(Convert.ToInt32(Math.Truncate(_amount)));
        double d = _amount - Math.Truncate(_amount);
        if (d > 0)
        {
            d = Math.Round(d * 100);
            _currencyInWord = _currencyInWord + " And " + d.ToString() + " / 100";
        }
        _currencyInWord = "*** " + _currencyInWord + "****************";
        return _currencyInWord;
    }
    //private void UpdateChartBalance()
    //{
    //    try
    //    {
    //        _objChart.ConnConfig = Session["config"].ToString();
    //        _objChart.ID = _objTrans.Acct;
    //        _objChart.Amount = _objTrans.Amount;
    //        _objBLChart.UpdateChartBalance(_objChart);
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
    //    }
    //}
    private void GetPeriodDetails(DateTime _transDate)
    {
        bool _flag = CommonHelper.GetPeriodDetails(_transDate);
        ViewState["FlagPeriodClose"] = _flag;
        if (!_flag)
        {
            divSuccess.Visible = true;
        }
    }
    private void BindBills()
    {
        try
        {


            if (!ddlVendor.SelectedValue.Equals("0"))
            {
                #region Set Vendor details

                lblVendor.Text = ddlVendor.SelectedItem.Text;
                _objVendor.ConnConfig = Session["config"].ToString();
                _objVendor.ID = Convert.ToInt32(ddlVendor.SelectedValue);

                _getVendor.ConnConfig = Session["config"].ToString();
                _getVendor.ID = Convert.ToInt32(ddlVendor.SelectedValue);

                DataSet _dsVendor = new DataSet();
                List <VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/AddCheck_GetVendor";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getVendor);

                    _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
                    _dsVendor = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
                    _dsVendor.Tables[0].Columns.Remove("Type");
                    _dsVendor.Tables[0].Columns["VType"].ColumnName = "Type";
                    _dsVendor.Tables[0].Columns["Vendor1099"].ColumnName = "1099";
                    _dsVendor.Tables[0].Columns["AcctNumber"].ColumnName = "Acct#";
                }
                else
                {
                    _dsVendor = _objBLVendor.GetVendor(_objVendor);
                }

                double _balance = Convert.ToDouble(_dsVendor.Tables[0].Rows[0]["Balance"].ToString());
                if (_balance < 0)
                    _balance = _balance * -1;
                lblVendorBal.Text = string.Format("{0:c}", _balance);
                _objOpenAP.ConnConfig = Session["config"].ToString();
                _objOpenAP.Vendor = Convert.ToInt32(ddlVendor.SelectedValue);
                _objOpenAP.SearchValue = Convert.ToInt16(ddlInvoice.SelectedValue);
                _objOpenAP.Company = Session["company"].ToString();                

                _getBillsByVendor.ConnConfig = Session["config"].ToString();
                _getBillsByVendor.Vendor = Convert.ToInt32(ddlVendor.SelectedValue);
                _getBillsByVendor.SearchValue = Convert.ToInt16(ddlInvoice.SelectedValue);
                _getBillsByVendor.Company = Session["company"].ToString();

                if (_objOpenAP.SearchValue.Equals(2))
                {
                    if (string.IsNullOrEmpty(txtSearchDate.Text))
                    {
                        _objOpenAP.SearchDate = DateTime.Now;
                        txtSearchDate.Text = DateTime.Now.ToShortDateString();
                    }
                    else
                    {
                        _objOpenAP.SearchDate = Convert.ToDateTime(txtSearchDate.Text);
                    }

                }

                if (_getBillsByVendor.SearchValue.Equals(2))
                {
                    if (string.IsNullOrEmpty(txtSearchDate.Text))
                    {
                        _getBillsByVendor.SearchDate = DateTime.Now;
                        txtSearchDate.Text = DateTime.Now.ToShortDateString();
                    }
                    else
                    {
                        _getBillsByVendor.SearchDate = Convert.ToDateTime(txtSearchDate.Text);
                    }

                }

                DataSet _dsBills = new DataSet();
                List<OpenAPViewModel> _lstOpenAPViewModel = new List<OpenAPViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/AddCheck_GetBillsByVendor";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getBillsByVendor);

                    _lstOpenAPViewModel = (new JavaScriptSerializer()).Deserialize<List<OpenAPViewModel>>(_APIResponse.ResponseData);
                    _dsBills = CommonMethods.ToDataSet<OpenAPViewModel>(_lstOpenAPViewModel);
                    _dsBills.Tables[0].Columns["RolName"].ColumnName = "Name";
                }
                else
                {
                    _dsBills = _objBLBill.GetBillsByVendor(_objOpenAP);
                }

                gvBills.VirtualItemCount = _dsBills.Tables[0].Rows.Count;
                gvBills.DataSource = _dsBills;
                //gvBills.DataBind();
                gvBills.Rebind();
                Session["dsBills"] = _dsBills.Tables[0];
                if (_dsBills.Tables[0].Rows.Count > 0)
                {
                    //Label lblTotalOrig = (Label)gvBills.FooterRow.FindControl("lblTotalOrig");
                    //Label lblTotalBalance = (Label)gvBills.FooterRow.FindControl("lblTotalBalance");
                    //Label lblTotalPay = (Label)gvBills.FooterRow.FindControl("lblTotalPay");
                    //Label lblTotalDisc = (Label)gvBills.FooterRow.FindControl("lblTotalDisc");

                    GridFooterItem footerItem = (GridFooterItem)gvBills.MasterTableView.GetItems(GridItemType.Footer)[0];
                    Label lblTotalOrig = (Label)footerItem.FindControl("lblTotalOrig");
                    Label lblTotalBalance = (Label)footerItem.FindControl("lblTotalBalance");
                    Label lblTotalPay = (Label)footerItem.FindControl("lblTotalPay");
                    Label lblTotalDisc = (Label)footerItem.FindControl("lblTotalDisc");


                    lblTotalOrig.Text = string.Format("{0:c}", Convert.ToDouble(_dsBills.Tables[0].Compute("sum(Original)", "").ToString()));
                    //lblTotalBalance.Text = "$0.00"; //string.Format("{0:c}", Convert.ToDouble(_dsBills.Tables[0].Compute("sum(Balance)", "").ToString()));
                    lblTotalBalance.Text = string.Format("{0:c}", Convert.ToDouble(_dsBills.Tables[0].Compute("sum(Balance)", "").ToString()));
                    lblTotalPay.Text = "$0.00";
                    lblTotalDisc.Text = "$0.00";

                    DataRow[] Rowschkbalance = _dsBills.Tables[0].Select("Balance < 0");
                    if (Rowschkbalance.Length > 0)
                    {
                        //btnApplyCredit.Visible = true;
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "display apply credit", "displayapplycredit('1');", true);

                    }
                    else
                    {
                        //btnApplyCredit.Visible = false;
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "display apply credit", "displayapplycredit('0');", true);

                    }

                }

                #endregion

                GetPaymentTotal();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private bool IsValidDate()
    {
        DateTime dateValue;
        string[] formats = {"M/d/yyyy", "M/d/yyyy",
                "MM/dd/yyyy", "M/d/yyyy",
                "M/d/yyyy", "M/d/yyyy",
                "M/d/yyyy", "M/d/yyyy",
                "MM/dd/yyyy", "M/dd/yyyy"};
        var dt = DateTime.TryParseExact(txtDate.Text.ToString(), formats,
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
    private void CreateTableBill()
    {

        dti.Columns.Add(new DataColumn("Ref", typeof(string)));
        dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        dti.Columns.Add(new DataColumn("Reference", typeof(string)));
        dti.Columns.Add(new DataColumn("Total", typeof(string)));
        dti.Columns.Add(new DataColumn("Disc", typeof(string)));
        dti.Columns.Add(new DataColumn("AmountPay", typeof(string)));
        dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
        dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
        dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        dti.Columns.Add(new DataColumn("VendorName", typeof(string)));
        dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
        dti.Columns.Add(new DataColumn("Description", typeof(string)));
    }
    private void CreateTableInvoice()
    {

        dti.Columns.Add(new DataColumn("Ref", typeof(string)));
        dti.Columns.Add(new DataColumn("InvoiceDate", typeof(string)));
        dti.Columns.Add(new DataColumn("Reference", typeof(string)));
        dti.Columns.Add(new DataColumn("Total", typeof(string)));
        dti.Columns.Add(new DataColumn("Disc", typeof(string)));
        dti.Columns.Add(new DataColumn("AmountPay", typeof(string)));
        dti.Columns.Add(new DataColumn("PayDate", typeof(string)));
        dti.Columns.Add(new DataColumn("CheckNo", typeof(string)));
        dti.Columns.Add(new DataColumn("VendorID", typeof(Int32)));
        dti.Columns.Add(new DataColumn("VendorName", typeof(string)));
        dti.Columns.Add(new DataColumn("Type", typeof(Int32)));
        dti.Columns.Add(new DataColumn("Description", typeof(string)));
        dti.Columns.Add(new DataColumn("VendorAcct", typeof(string)));
    }
    private void CreateTablePayee()
    {
        dtpay.Columns.Add(new DataColumn("Pay", typeof(string)));
        dtpay.Columns.Add(new DataColumn("ToOrder", typeof(string)));
        dtpay.Columns.Add(new DataColumn("Date", typeof(string)));
        dtpay.Columns.Add(new DataColumn("CheckAmount", typeof(string)));
        dtpay.Columns.Add(new DataColumn("ToOrderAddress", typeof(string)));
        dtpay.Columns.Add(new DataColumn("State", typeof(string)));
        dtpay.Columns.Add(new DataColumn("VendorAddress", typeof(string)));
        dtpay.Columns.Add(new DataColumn("RemitAddress", typeof(string)));
        dtpay.Columns.Add(new DataColumn("VendorAcct", typeof(string)));
    }
    private void CreateTableBank()
    {
        dtBank.Columns.Add(new DataColumn("Name", typeof(string)));
        dtBank.Columns.Add(new DataColumn("Address", typeof(string)));
        dtBank.Columns.Add(new DataColumn("City", typeof(string)));
        dtBank.Columns.Add(new DataColumn("State", typeof(string)));
        dtBank.Columns.Add(new DataColumn("Zip", typeof(string)));
        dtBank.Columns.Add(new DataColumn("NBranch", typeof(string)));
        dtBank.Columns.Add(new DataColumn("NAcct", typeof(string)));
        dtBank.Columns.Add(new DataColumn("NRoute", typeof(string)));
        dtBank.Columns.Add(new DataColumn("Ref", typeof(string)));
    }
    private DataTable GetBillItems()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("fDate", typeof(DateTime));
        dt.Columns.Add("PJID", typeof(int));
        dt.Columns.Add("Ref", typeof(string));
        dt.Columns.Add("TRID", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Spec", typeof(int));
        dt.Columns.Add("Original", typeof(double));
        dt.Columns.Add("Balance", typeof(double));
        dt.Columns.Add("Disc", typeof(double));
        dt.Columns.Add("Paid", typeof(double));

        try
        {
            //foreach (GridViewRow gr in gvBills.Rows)
            foreach (GridDataItem gr in gvBills.Items)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {

                    HiddenField hdnPJID = (HiddenField)gr.FindControl("hdnPJID");
                    HiddenField hdnTRID = (HiddenField)gr.FindControl("hdnTRID");
                    Label lblOrig = (Label)gr.FindControl("lblOrig");
                    HiddenField hdnPrevDue = (HiddenField)gr.FindControl("hdnPrevDue");
                    TextBox txtGvDisc = (TextBox)gr.FindControl("txtGvDisc");
                    TextBox txtGvPay = (TextBox)gr.FindControl("txtGvPay");
                    HiddenField hdnRef = (HiddenField)gr.FindControl("hdnRef");
                    Label lblSpec = (Label)gr.FindControl("lblSpec");
                    Label lblBillfdesc = (Label)gr.FindControl("lblBillfdesc");
                    TextBox txtGvDesc = (TextBox)gr.FindControl("txtGvDesc");
                    Label lblfDate = (Label)gr.FindControl("lblfDate");

                    DataRow dr = dt.NewRow();
                    dr["fDate"] = Convert.ToDateTime(lblfDate.Text);
                    dr["PJID"] = Convert.ToInt32(hdnPJID.Value);
                    dr["Ref"] = hdnRef.Value;
                    dr["TRID"] = Convert.ToInt32(hdnTRID.Value);
                    //dr["fDesc"] = lblBillfdesc.Text;
                    dr["fDesc"] = txtGvDesc.Text;
                    dr["Spec"] = !string.IsNullOrEmpty(lblSpec.Text) ? Convert.ToInt32(lblSpec.Text) : 0;
                    dr["Original"] = Math.Round(double.Parse(lblOrig.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                          NumberStyles.AllowThousands |
                                          NumberStyles.AllowDecimalPoint) * 100) / 100;
                    dr["Balance"] = Convert.ToDouble(hdnPrevDue.Value);
                    dr["Disc"] = Convert.ToDouble(txtGvDisc.Text);
                    dr["Paid"] = Convert.ToDouble(txtGvPay.Text);
                    dt.Rows.Add(dr);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return dt;
    }
    #endregion

    #region Comments
    //private void AddCheckDetails()
    //{
    //    try
    //    {
    //        //double _totalAmount = GetTotalAmount();
    //        //int transId = 0;
    //        #region Add transaction

    //        //_objChart.ConnConfig = Session["config"].ToString();
    //        //_objJournal.ConnConfig = Session["config"].ToString();

    //        //DataSet _dsActPayable = _objBLChart.GetAcctPayable(_objChart);
    //        //_objChart.Bank = Convert.ToInt32(ddlBank.SelectedValue);
    //        //int BankGL = _objBLChart.GetBankAcctID(_objChart);  // Change done by Mayuri 2nd Feb, 2017

    //        //int _batch = _objBLJournal.GetMaxTransBatch(_objJournal);
    //        ////Cash in bank          Credited
    //        //_objTrans = new Transaction();
    //        //_objTrans.ConnConfig = Session["config"].ToString();
    //        //_objTrans.BatchID = _batch;
    //        //_objTrans.Ref = Convert.ToInt32(txtNextCheck.Text);
    //        //_objTrans.TransDate = Convert.ToDateTime(txtDate.Text);
    //        //_objTrans.Line = 0;
    //        //_objTrans.TransDescription = ddlVendor.SelectedItem.Text;
    //        //_objTrans.Acct = BankGL;
    //        ////_objTrans.Amount = (Convert.ToDouble(lblTotalAmount.Text) * -1);
    //        //_objTrans.Amount = (_totalAmount * -1);
    //        //_objTrans.AcctSub = Convert.ToInt32(ddlBank.SelectedValue); 
    //        //_objTrans.Status = "";
    //        //_objTrans.Type = 20;
    //        //_objTrans.Sel = 0;
    //        //_objBLJournal.AddJournalTrans(_objTrans);
    //        //UpdateChartBalance();

    //        //double _discAmount = GetTotalDiscAmount();
    //        //if (_discAmount > 0)
    //        //{
    //        //    //DataSet _dsBankCharg = (_objChart);        //Bank charges         Credited
    //        //    //int discAcct = _objBLChart.GetDiscount(_objChart);
    //        //    //if(_dsBankCharg.Tables[0].Rows.Count > 0)
    //        //    //{
    //        //    int discAcct = 0;                            //Discount GL          Credited
    //        //    if(!string.IsNullOrEmpty(hdnDiscGL.Value))
    //        //    {
    //        //        discAcct = Convert.ToInt32(hdnDiscGL.Value);
    //        //    }
    //        //    _objTrans = new Transaction();
    //        //    _objTrans.ConnConfig = Session["config"].ToString();
    //        //    _objTrans.BatchID = _batch;
    //        //    _objTrans.Ref = Convert.ToInt32(txtNextCheck.Text);
    //        //    _objTrans.TransDate = Convert.ToDateTime(txtDate.Text);
    //        //    _objTrans.Line = 0;
    //        //    _objTrans.TransDescription = "Discount Taken";
    //        //    _objTrans.Acct = discAcct;
    //        //    _objTrans.Amount = (_discAmount * -1);
    //        //    _objTrans.Status = "";
    //        //    _objTrans.Type = 20;
    //        //    _objTrans.Sel = 0;
    //        //    _objBLJournal.AddJournalTrans(_objTrans);
    //        //    UpdateChartBalance();
    //        //    //}
    //        //}

    //        ////Accounts Payable      Debited
    //        //_objTrans = new Transaction();
    //        //_objTrans.ConnConfig = Session["config"].ToString();
    //        //_objTrans.BatchID = _batch;
    //        //_objTrans.Ref = Convert.ToInt32(txtNextCheck.Text);
    //        //_objTrans.TransDate = Convert.ToDateTime(txtDate.Text);
    //        //_objTrans.Line = 1;    
    //        //_objTrans.TransDescription = "Payment";
    //        //_objTrans.Acct = Convert.ToInt32(_dsActPayable.Tables[0].Rows[0]["ID"]);
    //        //_objTrans.Amount = _totalAmount + _discAmount;
    //        //_objTrans.AcctSub = Convert.ToInt32(ddlVendor.SelectedValue);
    //        //_objTrans.Status = "";
    //        //_objTrans.Type = 21;
    //        //_objTrans.Sel = 1;
    //        //transId = _objBLJournal.AddJournalTrans(_objTrans);
    //        //UpdateChartBalance();

    //        //#endregion

    //        //#region Add CD

    //        //_objCD.ConnConfig = Session["config"].ToString();
    //        //_objCD.fDate = Convert.ToDateTime(txtDate.Text);
    //        //_objCD.Ref = Convert.ToInt32(txtNextCheck.Text);
    //        //_objCD.fDesc = ddlVendor.SelectedItem.Text; //Payee's name
    //        //_objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);
    //        //_objCD.Type = 0;
    //        //_objCD.Status = 0; //0 = Paid; 2 = Voided;
    //        //_objCD.TransID = transId;
    //        //_objCD.Vendor = Convert.ToInt32(ddlVendor.SelectedValue);
    //        //_objCD.Memo = txtMemo.Text;
    //        ////_objCD.Amount = Convert.ToDouble(lblTotalAmount.Text);
    //        //_objCD.Amount = _totalAmount;
    //        //_objCD.IsRecon = false;
    //        //int _pitr = _objBLBill.AddCD(_objCD);
    //        #endregion

    //        #region Add paid
    //        short i = 1;
    //        foreach (GridViewRow gr in gvBills.Rows)
    //        {
    //            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

    //            if (chkSelect.Checked == true)
    //            {
    //                HiddenField hdnPJID = (HiddenField)gr.FindControl("hdnPJID");
    //                HiddenField hdnTRID = (HiddenField)gr.FindControl("hdnTRID");
    //                Label lblOrig = (Label)gr.FindControl("lblOrig");
    //                Label lblBalance = (Label)gr.FindControl("lblBalance");
    //                TextBox txtGvDisc = (TextBox)gr.FindControl("txtGvDisc");
    //                TextBox txtGvPay = (TextBox)gr.FindControl("txtGvPay");
    //                HiddenField hdnRef = (HiddenField)gr.FindControl("hdnRef");
    //                Label lblSpec = (Label)gr.FindControl("lblSpec");
    //                Label lblBillfdesc = (Label)gr.FindControl("lblBillfdesc");
    //                Label lblfDate = (Label)gr.FindControl("lblfDate");

    //                Int16 _spec = Convert.ToInt16(lblSpec.Text);
    //                if (!_spec.Equals(1) && !_spec.Equals(2) && !_spec.Equals(3))
    //                {

    //                    //_objPaid.ConnConfig = Session["config"].ToString();
    //                    //_objPaid.PITR = _pitr;
    //                    //_objPaid.fDate = Convert.ToDateTime(lblfDate.Text);
    //                    //_objPaid.Type = 0;
    //                    //_objPaid.Line = i;
    //                    //_objPaid.TRID = Convert.ToInt32(hdnTRID.Value);
    //                    //_objPaid.fDesc = lblBillfdesc.Text;
    //                    //_objPaid.Original = double.Parse(lblOrig.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
    //                    //          NumberStyles.AllowThousands |
    //                    //          NumberStyles.AllowDecimalPoint);
    //                    //_objPaid.Balance = double.Parse(lblBalance.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
    //                    //          NumberStyles.AllowThousands |
    //                    //          NumberStyles.AllowDecimalPoint);
    //                    //_objPaid.Disc = Convert.ToDouble(txtGvDisc.Text);
    //                    //_objPaid.Paid1 = Convert.ToDouble(txtGvPay.Text);
    //                    //_objPaid.Ref = hdnRef.Value;
    //                    //_objBLBill.AddPaid(_objPaid);
    //                    //i++;

    //                    //#region Update OpenAP

    //                    //_objOpenAP.ConnConfig = Session["config"].ToString();
    //                    //_objOpenAP.PJID = Convert.ToInt32(hdnPJID.Value);
    //                    //_objOpenAP.Selected = Convert.ToDouble(txtGvPay.Text);
    //                    //_objOpenAP.Balance = double.Parse(lblBalance.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
    //                    //          NumberStyles.AllowThousands |
    //                    //          NumberStyles.AllowDecimalPoint) - Convert.ToDouble(txtGvPay.Text);
    //                    //_objOpenAP.Disc = Convert.ToDouble(txtGvDisc.Text);
    //                    //_objBLBill.UpdateOpenAPPayment(_objOpenAP);

    //                    //#endregion

    //                    //#region Clear AP Bill

    //                    //_objTrans.ConnConfig = Session["config"].ToString();
    //                    //_objTrans.ID = Convert.ToInt32(hdnTRID.Value);
    //                    //_objTrans.Sel = 1;
    //                    //_objBLJournal.UpdateTransSel(_objTrans);

    //                    //#endregion

    //                    //#region PJ Status

    //                    //_objPJ.ConnConfig = Session["config"].ToString();
    //                    //_objPJ.TRID = Convert.ToInt32(hdnTRID.Value);
    //                    //DataSet _dsPJ = _objBLBill.GetPJByTransID(_objPJ);
    //                    //if (_dsPJ.Tables[0].Rows.Count > 0)
    //                    //{
    //                    //    _objPJ.ConnConfig = Session["config"].ToString();
    //                    //    _objPJ.ID = Convert.ToInt32(_dsPJ.Tables[0].Rows[0]["ID"]);
    //                    //    _objPJ.Status = 1; //   Open bill status : Paid
    //                    //    _objBLBill.UpdatePJClear(_objPJ);
    //                    //}

    //                    //#endregion
    //                }

    //            }
    //        }

    //        #endregion

    //        #region Update Bank Balance
    //        //_objBank.ConnConfig = Session["config"].ToString();
    //        //_objBank.ID = Convert.ToInt32(ddlBank.SelectedValue);
    //        //DataSet _dsbank = _objBL_Bank.GetBankByID(_objBank);
    //        //double _currrentBankBalance = 0;
    //        //if (_dsbank.Tables[0].Rows.Count > 0)
    //        //{
    //        //    _currrentBankBalance = Convert.ToDouble(_dsbank.Tables[0].Rows[0]["BankBalance"]);
    //        //    //_objBank.Balance = _currrentBankBalance + Convert.ToDouble(lblTotalAmount.Text);
    //        //    _objBank.Balance = _currrentBankBalance + _totalAmount;
    //        //    _objBank.NextC = Convert.ToInt32(txtNextCheck.Text) + 1;
    //        //    _objBL_Bank.UpdateBankBalanceNcheck(_objBank);
    //        //}

    //        #endregion

    //        #region Update Vendor balance

    //        //_objVendor.ConnConfig = Session["config"].ToString();
    //        //_objVendor.ID = Convert.ToInt32(ddlVendor.SelectedValue);
    //        //_objBLVendor.UpdateVendorBalance(_objVendor);

    //        #endregion
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
    //    }
    //}
    //public double GetTotalAmount()
    //{
    //    double total = 0.00;
    //    try
    //    {

    //        foreach (GridViewRow gr in gvBills.Rows)
    //        {
    //            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
    //            if (chkSelect.Checked == true)
    //            {
    //                Label lblSpec = (Label)gr.FindControl("lblSpec");
    //                TextBox txtGvPay = (TextBox)gr.FindControl("txtGvPay");
    //                Int16 spec = Convert.ToInt16(lblSpec.Text);
    //                if (!spec.Equals(1) && !spec.Equals(2) && !spec.Equals(3))
    //                {
    //                    total = total + Math.Round(double.Parse(txtGvPay.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
    //                                            NumberStyles.AllowThousands |
    //                                            NumberStyles.AllowDecimalPoint),2);
    //                }
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
    //    }
    //    return total;
    //}
    //protected void txtGvDisc_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        TextBox txtGvDisc = (TextBox)sender;
    //        if (string.IsNullOrEmpty(txtGvDisc.Text.ToString()))
    //            txtGvDisc.Text = "0.00";
    //        GridViewRow gridrow = (GridViewRow)txtGvDisc.NamingContainer;
    //        int rowIndex = gridrow.RowIndex;

    //        #region set invoice pay
    //        CheckBox chkSelect = (CheckBox)gvBills.Rows[rowIndex].FindControl("chkSelect");
    //        TextBox txtGvPay = (TextBox)gvBills.Rows[rowIndex].FindControl("txtGvPay");
    //        Label lblBalance = (Label)gvBills.Rows[rowIndex].FindControl("lblBalance");

    //        if (string.IsNullOrEmpty(txtGvPay.Text))
    //            txtGvPay.Text = "0.00";
    //        chkSelect.Checked = true;
    //        double balance = double.Parse(lblBalance.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
    //                              NumberStyles.AllowThousands |
    //                              NumberStyles.AllowDecimalPoint);

    //        if (string.IsNullOrEmpty(txtGvPay.Text))
    //            txtGvPay.Text = "0";

    //        if (string.IsNullOrEmpty(txtGvDisc.Text))
    //            txtGvDisc.Text = "0";

    //        double total = Convert.ToDouble(txtGvPay.Text) + Convert.ToDouble(txtGvDisc.Text);
    //        if (balance > total)
    //        {
    //            total = balance - total;
    //            total = total + Convert.ToDouble(txtGvPay.Text);
    //            txtGvPay.Text = total.ToString("0.00", CultureInfo.InvariantCulture);
    //            txtGvDisc.Text = Convert.ToDouble(txtGvDisc.Text).ToString("0.00", CultureInfo.InvariantCulture);
    //        }
    //        else if (balance < total)
    //        {
    //            txtGvDisc.Text = Convert.ToDouble(txtGvDisc.Text).ToString("0.00", CultureInfo.InvariantCulture); ;
    //            double payAmount = balance - Convert.ToDouble(txtGvDisc.Text);
    //            txtGvPay.Text = payAmount.ToString("0.00", CultureInfo.InvariantCulture);
    //        }

    //        #endregion

    //        GetPaymentTotal();
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
    //    }
    //}
    //protected void txtGvPay_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        TextBox txtGvPay = (TextBox)sender;
    //        GridViewRow gridrow = (GridViewRow)txtGvPay.NamingContainer;
    //        if (string.IsNullOrEmpty(txtGvPay.Text.ToString()))
    //            txtGvPay.Text = "0.00";
    //        double _paidAmount = Convert.ToDouble(txtGvPay.Text);
    //        int rowIndex = gridrow.RowIndex;

    //        #region set invoice pay
    //        CheckBox chkSelect = (CheckBox)gvBills.Rows[rowIndex].FindControl("chkSelect");
    //        TextBox txtGvDisc = (TextBox)gvBills.Rows[rowIndex].FindControl("txtGvDisc");
    //        Label lblBalance = (Label)gvBills.Rows[rowIndex].FindControl("lblBalance");

    //        if (string.IsNullOrEmpty(txtGvDisc.Text))
    //            txtGvDisc.Text = "0.00";
    //        chkSelect.Checked = true;
    //        double _dueAmount = double.Parse(lblBalance.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
    //                              NumberStyles.AllowThousands |
    //                              NumberStyles.AllowDecimalPoint);
    //        double total = Convert.ToDouble(txtGvPay.Text) + Convert.ToDouble(txtGvDisc.Text);
    //        if (_dueAmount < total)
    //        {
    //            txtGvPay.Text = _dueAmount.ToString("0.00", CultureInfo.InvariantCulture);
    //            txtGvDisc.Text = "0.00";
    //        }
    //        else
    //        {
    //            if (!Convert.ToDouble(txtGvPay.Text).Equals(0))
    //            {
    //                txtGvPay.Text = Convert.ToDouble(txtGvPay.Text).ToString("0.00", CultureInfo.InvariantCulture);
    //            }
    //            else
    //            {
    //                total = _dueAmount - total;
    //                total = total + Convert.ToDouble(txtGvPay.Text);
    //                txtGvPay.Text = total.ToString("0.00", CultureInfo.InvariantCulture);
    //                txtGvDisc.Text = Convert.ToDouble(txtGvDisc.Text).ToString("0.00", CultureInfo.InvariantCulture);
    //            }
    //        }

    //        #endregion

    //        GetPaymentTotal();
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
    //    }
    //}
    //protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CheckBox chkSelect = (CheckBox)sender;
    //        GridViewRow row = (GridViewRow)chkSelect.NamingContainer;
    //        TextBox txtGvPay = (TextBox)row.FindControl("txtGvPay");
    //        TextBox txtGvDisc = (TextBox)row.FindControl("txtGvDisc");

    //        if(chkSelect.Checked.Equals(true))
    //        {
    //            Label lblBalance = (Label)row.FindControl("lblBalance");

    //            double _pay = Convert.ToDouble(txtGvPay.Text);

    //            double _dueBalance = double.Parse(lblBalance.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
    //                              NumberStyles.AllowThousands |
    //                              NumberStyles.AllowDecimalPoint);

    //            if (_pay.Equals(0))
    //            {
    //                txtGvPay.Text = _dueBalance.ToString("0.00", CultureInfo.InvariantCulture);
    //            }
    //        }
    //        else
    //        {
    //            txtGvPay.Text = "0.00";
    //            txtGvDisc.Text = "0.00";
    //        }

    //        GetPaymentTotal();
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true})", true);
    //    }
    //}
    #endregion



    protected void ddlPayment_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!ddlBank.SelectedValue.Equals("0"))
        {
            SetBankDetails();
            btnSubmit.Visible = true;
            btnCutCheck.Visible = false;
            if (Convert.ToString(ViewState["bankwarn"]) == "1")
            {
                string strScript = string.Empty;
                strScript += "noty({text: 'Please note your selected will be overdrawn.', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", strScript, true);
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "Materialize.updateTextFields();", true);
        }
        else
        {
            btnSubmit.Visible = false;
            btnCutCheck.Visible = true;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "closeScript", "Materialize.updateTextFields();", true);
        }
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "display write check", "displayWriteCheck();", true);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "is exist", "IsExistCheckNo();", true);
    }
    private void FillPayment()
    {
        ddlPayment.Items.Clear();
        ddlPayment.Items.Add(new System.Web.UI.WebControls.ListItem("Select", "-1"));
        ddlPayment.Items.Add(new System.Web.UI.WebControls.ListItem("Check", "0"));
        ddlPayment.Items.Add(new System.Web.UI.WebControls.ListItem("Cash", "1"));
        ddlPayment.Items.Add(new System.Web.UI.WebControls.ListItem("Wire Transfer", "2"));
        ddlPayment.Items.Add(new System.Web.UI.WebControls.ListItem("ACH", "3"));
        ddlPayment.Items.Add(new System.Web.UI.WebControls.ListItem("Credit Card", "4"));
    }

    protected void chkBatch_CheckedChanged(object sender, EventArgs e)
    {
        string script = "function f(){$find(\"" + RadWindowAutomaticSelectionForPayment.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

    }
    protected void lnkSaveAutopayment_Click(object sender, EventArgs e)
    {
        try
        {
            int intChkHeader = 1;
            string selected = Request.Form["radio-group"].ToString();
            CompanyPermission();
            FillBank();
            FillVendor();
            FillPayment();
            if (selected == "rdDue")

            {
                _objCD.ConnConfig = Session["config"].ToString();
                _objCD.updateBy = "Due Date";
                _objCD.updateByValue = DateTime.Now;

                _autoSelectPayment.ConnConfig = Session["config"].ToString();
                _autoSelectPayment.updateBy = "Due Date";
                _autoSelectPayment.updateByValue = DateTime.Now;

            }
            else if (selected == "rdDated")
            {
                _objCD.updateBy = "Dated";
                _objCD.updateByValue = Convert.ToDateTime(txtdated.Text);

                _autoSelectPayment.updateBy = "Dated";
                _autoSelectPayment.updateByValue = Convert.ToDateTime(txtdated.Text);
            }
            else if (selected == "rdDateBefore")
            {
                _objCD.updateBy = "Due Before";
                _objCD.updateByValue = Convert.ToDateTime(txtDateBefore.Text);

                _autoSelectPayment.updateBy = "Due Before";
                _autoSelectPayment.updateByValue = Convert.ToDateTime(txtDateBefore.Text);
            }
            else if (selected == "rdRegard")
            {
                _objCD.updateBy = "No date";
                _autoSelectPayment.updateBy = "No date";
            }
            else if (selected == "rdClear")
            {
                _objCD.updateBy = "Clear";
                _autoSelectPayment.updateBy = "Clear";
                intChkHeader = 0;
            }

            if (Request.Form["checkbox-group"] != null)
            {
                if (Request.Form["checkbox-group"].ToString() == "chkVH")
                {
                    _objCD.isVH = true;
                    _autoSelectPayment.isVH = true;
                }
                else
                {
                    _objCD.isVH = false;
                    _autoSelectPayment.isVH = false;
                }
                if (Request.Form["checkbox-group"].ToString() == "chkDisc")
                {
                    _objCD.isDisc = true;
                    _autoSelectPayment.isDisc = true;

                }
                else
                {
                    _objCD.isDisc = false;
                    _autoSelectPayment.isDisc = false;
                }
                if (Request.Form["checkbox-group"].ToString() == "chkVH,chkDisc")
                {
                    _objCD.isVH = true;
                    _objCD.isDisc = true;

                    _autoSelectPayment.isVH = true;
                    _autoSelectPayment.isDisc = true;

                }
                //else
                //{
                //    _objCD.isVH = false;
                //    _objCD.isDisc = false;

                //    _autoSelectPayment.isVH = false;
                //    _autoSelectPayment.isDisc = false;
                //}
            }
            else
            {
                _objCD.isVH = false;
                _objCD.isDisc = false;

                _autoSelectPayment.isVH = false;
                _autoSelectPayment.isDisc = false;

            }
            _objCD.ConnConfig = Session["config"].ToString();
            _autoSelectPayment.ConnConfig = Session["config"].ToString();

            DataSet _dsBills = new DataSet();
            DataSet _dsBills1 = new DataSet();
            DataSet _dsBills2 = new DataSet();

            ListAutoSelectPayment _lstAutoSelectPayment = new ListAutoSelectPayment();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/AddCheck_AutoSelectPayment";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _autoSelectPayment);

                _lstAutoSelectPayment = (new JavaScriptSerializer()).Deserialize<ListAutoSelectPayment>(_APIResponse.ResponseData);

                _dsBills1 = _lstAutoSelectPayment.lstTable1.ToDataSet();
                _dsBills2 = _lstAutoSelectPayment.lstTable2.ToDataSet();

                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                dt1 = _dsBills1.Tables[0];
                dt2 = _dsBills2.Tables[0];

                dt1.TableName = "Table1";
                dt2.TableName = "Table2";

                _dsBills.Tables.AddRange(new DataTable[] { dt1.Copy(), dt2.Copy() });

            }
            else
            {
                _dsBills = _objBLBill.AutoSelectPayment(_objCD,Convert.ToString(Session["company"]));
            }


            //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            ////if (IsAPIIntegrationEnable == "YES")
            //if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            //{
            //    gvBills.VirtualItemCount = _dsBills1.Tables[0].Rows.Count;
            //    gvBills.DataSource = _dsBills1.Tables[0];
            //    Session["dsBills"] = _dsBills1.Tables[0];
            //    lblOI.Visible = true;
            //    lblOpenItems.Visible = true;
            //    lblBal.Visible = true;
            //    lblAutoSelectBalance.Visible = true;
            //    lblOpenItems.Text = _dsBills2.Tables[0].Rows[0]["NCount"].ToString();
            //    lblAutoSelectBalance.Text = _dsBills2.Tables[0].Rows[0]["NAmt"].ToString();
            //}
            //else
            //{
                gvBills.VirtualItemCount = _dsBills.Tables[0].Rows.Count;
                gvBills.DataSource = _dsBills.Tables[0];
                Session["dsBills"] = _dsBills.Tables[0];
                lblOI.Visible = true;
                lblOpenItems.Visible = true;
                lblBal.Visible = true;
                lblAutoSelectBalance.Visible = true;
                lblOpenItems.Text = _dsBills.Tables[1].Rows[0]["NCount"].ToString();
                lblAutoSelectBalance.Text = _dsBills.Tables[1].Rows[0]["NAmt"].ToString();
            //}
            //gvBills.DataBind();
            gvBills.Rebind();
            CheckAllCheckbox();
            refreshVendorDDL();
            if (ddlVendor.Items.FindByValue("-1") != null)
                ddlVendor.SelectedValue = "-1";
            if (Session["SelectedCompany"] != null)
                ddlCompany.SelectedValue = Session["SelectedCompany"].ToString();

            //SetBankDetails();
            //ResetForm();
            GetPaymentTotal();
            GetRunningBalance();

            //_objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            ////if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            ////{
            ////    if (_dsBills1.Tables[0].Rows.Count == 0)
            ////    {
            ////        string str = "No open bills found to process the check.";
            ////        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  noty({text: '" + str + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            ////    }

            ////    if (_dsBills1.Tables[0].Rows.Count > 0)
            ////    {
            ////        //string script = "function lblselectp(){$find(\"" + RadWindowAutomaticSelectionForPayment.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
            ////        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            ////        ScriptManager.RegisterStartupScript(this, Page.GetType(), "CallMyFunction", "lblselectp();", true);

            ////    }
            ////}
            ////else
            ////{
                if (_dsBills.Tables[0].Rows.Count == 0)
                {
                    string str = "No open bills found to process the check.";
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  noty({text: '" + str + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                }

                if (_dsBills.Tables[0].Rows.Count > 0)
                {
                    //string script = "function lblselectp(){$find(\"" + RadWindowAutomaticSelectionForPayment.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "CallMyFunction", "lblselectp();", true);

                }
            //}

            lblVendorBal.Text = "$0.00";
            
            lblSelectedPayment.Text = lblRunBalance.Text;
            
            CheckHeaderCheckbox(intChkHeader);
            ViewState["AutoCheckRun"] = "True";
        }
        catch(Exception ex)
        {}
    }
    private void CheckHeaderCheckbox(int intChkID)
    {
        GridHeaderItem headerItem = gvBills.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
        if (intChkID == 1)
            (headerItem.FindControl("chkSelectAll") as CheckBox).Checked = true;
        else
            (headerItem.FindControl("chkSelectAll") as CheckBox).Checked = false;
    }
    public void CheckAllCheckbox()
    {
        string Duepayment = "0";
        //foreach (GridViewRow grow in gvBills.Rows)
        foreach (GridDataItem grow in gvBills.Items)
        {
            CheckBox chkSelect = (CheckBox)grow.FindControl("chkSelect");
         
            HiddenField hdnisSelected = (HiddenField)grow.FindControl("hdnisSelected");
            if ((hdnisSelected.Value) == "True")
            {
                
                    TextBox txtGvDisc = (TextBox)grow.FindControl("txtGvDisc");
                TextBox txtGvPay = (TextBox)grow.FindControl("txtGvPay");
                Label lblBalance = (Label)grow.FindControl("lblBalance");
                HiddenField hdnPrevDue = (HiddenField)grow.FindControl("hdnPrevDue");
                HiddenField hdnSelected = (HiddenField)grow.FindControl("hdnSelected");
                HiddenField hdnOriginal = (HiddenField)grow.FindControl("hdnOriginal");
                Duepayment = Convert.ToString(Convert.ToDouble(hdnOriginal.Value) - Convert.ToDouble(hdnSelected.Value) - Convert.ToDouble(txtGvPay.Text)- Convert.ToDouble(txtGvDisc.Text));
                Duepayment = Convert.ToDouble(Duepayment).ToString("0.00", CultureInfo.InvariantCulture);
                txtGvPay.Text = Duepayment.Replace("$", "");
                txtGvPay.Text = txtGvPay.Text.Replace("(", "-");
                txtGvPay.Text = txtGvPay.Text.Replace(")", "");
                // lblBalance.Text = "$0.00";
                chkSelect.Checked = true;

            }
        }
    }
    public static byte[] concatAndAddContent(List<byte[]> pdfByteContent)
    {
        MemoryStream ms = new MemoryStream();
        Document doc = new Document();
        PdfSmartCopy copy = new PdfSmartCopy(doc, ms);

        doc.Open();

        //Loop through each byte array
        foreach (var p in pdfByteContent)
        {
            PdfReader reader = new PdfReader(p);
            int n = reader.NumberOfPages;

            for (int i = 1; i <= n; i++)
            {
                byte[] red = reader.GetPageContent(i);
                if (red.Length < 1000)
                {
                    //n = n - 1;
                }
            }
            for (int page = 0; page < n;)
            {
                copy.AddPage(copy.GetImportedPage(reader, ++page));
            }
        }
        doc.Close();
        //Return just before disposing
        return ms.ToArray();
    }

    public static byte[] concatAndAddContentFinal(List<byte[]> pdfByteContentA, List<byte[]> pdfByteContentB)
    {
        MemoryStream ms = new MemoryStream();
        Document doc = new Document();
        PdfSmartCopy copy = new PdfSmartCopy(doc, ms);

        doc.Open();

        //Loop through each byte array
        foreach (var p in pdfByteContentA)
        {
            PdfReader reader = new PdfReader(p);
            int n = reader.NumberOfPages;

            for (int i = 1; i <= n; i++)
            {
                byte[] red = reader.GetPageContent(i);
                if (red.Length < 1000)
                {
                    n = n - 1;
                }
                for (int page = 0; page < n;)
                {
                    copy.AddPage(copy.GetImportedPage(reader, ++page));
                }
            }
        }
        foreach (var p1 in pdfByteContentB)
        {
            PdfReader reader1 = new PdfReader(p1);
            int n = reader1.NumberOfPages;

            for (int i = 1; i <= n; i++)
            {
                byte[] red = reader1.GetPageContent(i);
                if (red.Length < 1000)
                {
                    n = n - 1;
                }
            }
            for (int page = 0; page < n;)
            {
                copy.AddPage(copy.GetImportedPage(reader1, ++page));
            }


        }
        doc.Close();
        //Return just before disposing
        return ms.ToArray();

    }


    private DataTable GetVendorBillItems(string Vendor)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("fDate", typeof(DateTime));
        dt.Columns.Add("PJID", typeof(int));
        dt.Columns.Add("Ref", typeof(string));
        dt.Columns.Add("TRID", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Spec", typeof(int));
        dt.Columns.Add("Original", typeof(double));
        dt.Columns.Add("Balance", typeof(double));
        dt.Columns.Add("Disc", typeof(double));
        dt.Columns.Add("Paid", typeof(double));
        double AmountPay = 0.00;
        try
        {
            //foreach (GridViewRow gr in gvBills.Rows)
            foreach (GridDataItem gr in gvBills.Items)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                if (chkSelect.Checked == true)
                {
                    Label lblVendor = (Label)gr.FindControl("lblVendor");
                    if (lblVendor.Text == Vendor)
                    {
                        TextBox txtGvPay = (TextBox)gr.FindControl("txtGvPay");

                        HiddenField hdnPJID = (HiddenField)gr.FindControl("hdnPJID");
                        HiddenField hdnTRID = (HiddenField)gr.FindControl("hdnTRID");
                        Label lblOrig = (Label)gr.FindControl("lblOrig");
                        HiddenField hdnPrevDue = (HiddenField)gr.FindControl("hdnPrevDue");
                        TextBox txtGvDisc = (TextBox)gr.FindControl("txtGvDisc");

                        HiddenField hdnRef = (HiddenField)gr.FindControl("hdnRef");
                        Label lblSpec = (Label)gr.FindControl("lblSpec");
                        Label lblBillfdesc = (Label)gr.FindControl("lblBillfdesc");
                        
                        Label lblfDate = (Label)gr.FindControl("lblfDate");

                        TextBox txtGvDesc = (TextBox)gr.FindControl("txtGvDesc");

                        DataRow dr = dt.NewRow();
                        dr["fDate"] = Convert.ToDateTime(lblfDate.Text);
                        dr["PJID"] = Convert.ToInt32(hdnPJID.Value);
                        dr["Ref"] = hdnRef.Value;
                        dr["TRID"] = Convert.ToInt32(hdnTRID.Value);
                        //dr["fDesc"] = lblBillfdesc.Text;
                        dr["fDesc"] = txtGvDesc.Text;
                        dr["Spec"] = !string.IsNullOrEmpty(lblSpec.Text) ? Convert.ToInt32(lblSpec.Text) : 0;
                        dr["Original"] = Math.Round(double.Parse(lblOrig.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                              NumberStyles.AllowThousands |
                                              NumberStyles.AllowDecimalPoint) * 100) / 100;
                        dr["Balance"] = Convert.ToDouble(hdnPrevDue.Value);
                        dr["Disc"] = Convert.ToDouble(txtGvDisc.Text);
                        dr["Paid"] = Convert.ToDouble(txtGvPay.Text);
                        AmountPay = AmountPay + Convert.ToDouble(txtGvPay.Text);
                        dt.Rows.Add(dr);

                    }
                }
            }

            while (AmountPay < 0)
            {
                List<DataRow> rowsWantToDelete = new List<DataRow>();
                foreach (DataRow drow in dt.Rows)
                {
                    if (Convert.ToDouble(drow["Paid"].ToString()) < 0)
                    {
                        rowsWantToDelete.Add(drow);
                        foreach (DataRow rows in rowsWantToDelete)
                        {
                            dt.Rows.Remove(rows);
                        }

                        AmountPay = dt.AsEnumerable().Sum(x => Convert.ToDouble(x["Paid"]));
                        break;
                    }
                }


            }



        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        return dt;
    }
    private DataTable ValidateAmount(DataTable dt)
    {
        DataRow[] result = dt.Select("AmountPay < 0");
        foreach (DataRow row in result)
        {
            dt.Rows.Remove(row);
            break;
        }
        dt.Reset();
        dt.Rows.Add(result);
        return dt;
    }


    #region Print check


    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDataBasedOnCompany();
    }
    private void LoadDataBasedOnCompany()
    {
        ddlVendor.Items.Clear();
        ddlBank.Items.Clear();
        _objVendor.ConnConfig = Session["config"].ToString();
        _objBank.ConnConfig = Session["config"].ToString();

        _getOpenBillVendorByCompany.ConnConfig = Session["config"].ToString();
        _getAllBankNamesByCompany.ConnConfig = Session["config"].ToString();
        _getOpenBillVendor.ConnConfig = Session["config"].ToString();
        _getAllBankNames.ConnConfig = Session["config"].ToString();

        DataSet _dsVendor = new DataSet();
        DataSet _dsBank = new DataSet();

        if (Session["COPer"].ToString() == "1")
        {
            if (ddlCompany.SelectedValue != "0")
            {
                _objVendor.EN = Convert.ToInt32(ddlCompany.SelectedValue);
                _objBank.EN = Convert.ToInt32(ddlCompany.SelectedValue);

                _getOpenBillVendorByCompany.EN = Convert.ToInt32(ddlCompany.SelectedValue);
                _getAllBankNamesByCompany.EN = Convert.ToInt32(ddlCompany.SelectedValue);
                _getOpenBillVendor.EN = Convert.ToInt32(ddlCompany.SelectedValue);

                List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/AddCheck_GetOpenBillVendorByCompany";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getOpenBillVendorByCompany);

                    _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
                    _dsVendor = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
                }
                else
                {
                    _dsVendor = _objBLVendor.GetOpenBillVendorByCompany(_objVendor);
                }

                List<BankViewModel> _lstBankViewModel = new List<BankViewModel>();
                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/AddCheck_GetAllBankNamesByCompany";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getAllBankNamesByCompany);

                    _lstBankViewModel = (new JavaScriptSerializer()).Deserialize<List<BankViewModel>>(_APIResponse.ResponseData);
                    _dsBank = CommonMethods.ToDataSet<BankViewModel>(_lstBankViewModel);
                }
                else
                {
                    _dsBank = _objBL_Bank.GetAllBankNamesByCompany(_objBank);
                }

            }
            else
            {
                List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/AddCheck_GetOpenBillVendor";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getOpenBillVendor);

                    _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
                    _dsBank = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
                }
                else
                {
                    _dsVendor = _objBLVendor.GetOpenBillVendor(_objVendor);
                }

                List<GetAllBankNamesViewModel> _lstGetAllBankNamesViewModel = new List<GetAllBankNamesViewModel>();
                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/CheckList_GetAllBankNames";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getAllBankNames);

                    _lstGetAllBankNamesViewModel = (new JavaScriptSerializer()).Deserialize<List<GetAllBankNamesViewModel>>(_APIResponse.ResponseData);
                    _dsBank = CommonMethods.ToDataSet<GetAllBankNamesViewModel>(_lstGetAllBankNamesViewModel);
                }
                else
                {
                    _dsBank = _objBL_Bank.GetAllBankNames(_objBank);
                }
            }

        }
        else
        {
            List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/AddCheck_GetOpenBillVendor";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getOpenBillVendor);

                _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
                _dsBank = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
            }
            else
            {
                _dsVendor = _objBLVendor.GetOpenBillVendor(_objVendor);
            }

            List<GetAllBankNamesViewModel> _lstGetAllBankNamesViewModel = new List<GetAllBankNamesViewModel>();
            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/CheckList_GetAllBankNames";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getAllBankNames);

                _lstGetAllBankNamesViewModel = (new JavaScriptSerializer()).Deserialize<List<GetAllBankNamesViewModel>>(_APIResponse.ResponseData);
                _dsBank = CommonMethods.ToDataSet<GetAllBankNamesViewModel>(_lstGetAllBankNamesViewModel);
            }
            else
            {
                _dsBank = _objBL_Bank.GetAllBankNames(_objBank);
            }
        }

        if (_dsVendor.Tables[0].Rows.Count > 0)
        {
            ddlVendor.Items.Add(new System.Web.UI.WebControls.ListItem(":: Select ::", "0"));
            ddlVendor.Items.Add(new System.Web.UI.WebControls.ListItem(":: Selected Payment ::", "-1"));
            ddlVendor.AppendDataBoundItems = true;

            ddlVendor.DataSource = _dsVendor;
            ddlVendor.DataValueField = "ID";
            ddlVendor.DataTextField = "Name";
            ddlVendor.DataBind();
        }
        else
        {
            ddlVendor.Items.Add(new System.Web.UI.WebControls.ListItem("No data found", "0"));
        }
        if (_dsBank.Tables[0].Rows.Count > 0)
        {
            ddlBank.Items.Clear();
            ddlBank.Items.Add(new System.Web.UI.WebControls.ListItem(":: Select ::", "0"));
            ddlBank.AppendDataBoundItems = true;

            ddlBank.DataSource = _dsBank;
            ddlBank.DataValueField = "ID";
            ddlBank.DataTextField = "fDesc";
            ddlBank.DataBind();
        }
        else
        {
            ddlBank.Items.Add(new System.Web.UI.WebControls.ListItem("No data found", "0"));
        }
    }
    private string GetDefaultCompany()
    {
        objCompany.UserID = Convert.ToInt32(Session["UserID"].ToString());
        objCompany.DBName = Session["dbname"].ToString();
        objCompany.ConnConfig = Session["config"].ToString();

        _getUserDefaultCompany.UserID = Convert.ToInt32(Session["UserID"].ToString());
        _getUserDefaultCompany.DBName = Session["dbname"].ToString();
        _getUserDefaultCompany.ConnConfig = Session["config"].ToString();

        DataSet ds = new DataSet();

        List<UserViewModel> _lstUserViewModel = new List<UserViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/AddCheck_getUserDefaultCompany";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getUserDefaultCompany);

            _lstUserViewModel = (new JavaScriptSerializer()).Deserialize<List<UserViewModel>>(_APIResponse.ResponseData);
            ds = CommonMethods.ToDataSet<UserViewModel>(_lstUserViewModel);
        }
        else
        {
            ds = objBL_Company.getUserDefaultCompany(objCompany);
        }

        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["DefaultCompID"] = ds.Tables[0].Rows[0]["EN"].ToString();
            string companyname = ds.Tables[0].Rows[0]["Name"].ToString();
            //if (companyname.Length > 16)
            //    return companyname.Substring(0, 16);
            //else
            return companyname;
        }
        return "";
    }
    private void refreshVendorDDL()
    {
        ddlVendor.Items.Clear();

        _objVendor.ConnConfig = Session["config"].ToString();
        _getOpenBillVendorByCompany.ConnConfig = Session["config"].ToString();
        _getOpenBillVendor.ConnConfig = Session["config"].ToString();

        DataSet _dsVendor = new DataSet();


        if (Session["COPer"].ToString() == "1")
        {
            if (ddlCompany.SelectedValue != "0")
            {
                _objVendor.EN = Convert.ToInt32(ddlCompany.SelectedValue);
                _getOpenBillVendorByCompany.EN = Convert.ToInt32(ddlCompany.SelectedValue);

                List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/AddCheck_GetOpenBillVendorByCompany";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getOpenBillVendorByCompany);

                    _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
                    _dsVendor = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
                }
                else
                {
                    _dsVendor = _objBLVendor.GetOpenBillVendorByCompany(_objVendor);
                }

            }
            else
            {
                List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                //if (IsAPIIntegrationEnable == "YES")
                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                {
                    string APINAME = "ManageChecksAPI/AddCheck_GetOpenBillVendor";

                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getOpenBillVendor);

                    _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
                    _dsVendor = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
                }
                else
                {
                    _dsVendor = _objBLVendor.GetOpenBillVendor(_objVendor);
                }
            }

        }
        else
        {
            List<VendorViewModel> _lstVendorViewModel = new List<VendorViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/AddCheck_GetOpenBillVendor";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getOpenBillVendor);

                _lstVendorViewModel = (new JavaScriptSerializer()).Deserialize<List<VendorViewModel>>(_APIResponse.ResponseData);
                _dsVendor = CommonMethods.ToDataSet<VendorViewModel>(_lstVendorViewModel);
            }
            else
            {
                _dsVendor = _objBLVendor.GetOpenBillVendor(_objVendor);
            }
        }

        if (_dsVendor.Tables[0].Rows.Count > 0)
        {
            ddlVendor.Items.Add(new System.Web.UI.WebControls.ListItem(":: Select ::", "0"));
            ddlVendor.Items.Add(new System.Web.UI.WebControls.ListItem(":: Selected Payment ::", "-1"));
            ddlVendor.AppendDataBoundItems = true;

            ddlVendor.DataSource = _dsVendor;
            ddlVendor.DataValueField = "ID";
            ddlVendor.DataTextField = "Name";
            ddlVendor.DataBind();
        }
        else
        {
            ddlVendor.Items.Add(new System.Web.UI.WebControls.ListItem("No data found", "0"));
        }
    }

    protected void StiWebDesigner1_SaveReport(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {

        //ReportModalPopupExtender.Hide();
        Session["wc_first"] = null;
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + e.FileName));

    }


    protected void StiWebDesigner2_SaveReport(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {

        // ReportModalPopupExtender1.Hide();
        Session["wc_second"] = null;
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + e.FileName));

    }

    protected void StiWebDesigner3_SaveReport(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {

        // ReportModalPopupExtender2.Hide();
        Session["wc_third"] = null;
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + e.FileName));

    }


    protected void ImageButton7_Click(object sender, ImageClickEventArgs e)
    {
        if (Request.QueryString["bill"] == null)
        {
            //mpeTemplate.Hide();
            string reportName = ddlApTopCheckForLoad.SelectedItem.Text.Trim();
            StiReport report = FillDataSetToReport(reportName);
            StiWebDesigner1.Report = report;
            //ReportModalPopupExtender.Show();
            StiWebDesigner1.Visible = true;

            Session["wc_first"] = "true";
        }
        else if (Request.QueryString["bill"] == "c")
        {
            if (Request.QueryString["vid"] != null)
            {
                string reportName = ddlApTopCheckForLoad.SelectedItem.Text.Trim();
                StiReport report = FillDataSetToReport(reportName);
                StiWebDesigner1.Report = report;
                //ReportModalPopupExtender.Show();
                
            }
            else
            {
                //mpeTemplate.Hide();
                string reportName = ddlApTopCheckForLoad.SelectedItem.Text.Trim();
                StiReport report = FillDataSetToReport_New(reportName);
                StiWebDesigner1.Report = report;
                //ReportModalPopupExtender.Show();
                
            }
            StiWebDesigner1.Visible = true;

            Session["wc_first"] = "true";
        }

    }

    protected void ImageButton8_Click(object sender, ImageClickEventArgs e)
    {

        //mpeTemplate.Hide();
        if (Request.QueryString["bill"] == null)
        {
            string reportName = ddlApMiddleCheckForLoad.SelectedItem.Text.Trim();
            StiReport report = FillMiddleDataSetReport(reportName);
            StiWebDesigner2.Report = report;
            //ReportModalPopupExtender1.Show();
            Session["wc_second"] = "true";
            StiWebDesigner2.Visible = true;
        }
        else if (Request.QueryString["bill"] == "c")
        {
            if (Request.QueryString["vid"] != null)
            {
                string reportName = ddlApMiddleCheckForLoad.SelectedItem.Text.Trim();
                StiReport report = FillMiddleDataSetReport(reportName);
                StiWebDesigner2.Report = report;
                //    StiWebDesigner2.Report = report;
                //    //ReportModalPopupExtender1.Show();
                //    Session["wc_second"] = "true";
                //    StiWebDesigner2.Visible = true;
            }
            else
            {
                string reportName = ddlApMiddleCheckForLoad.SelectedItem.Text.Trim();
                StiReport report = FillMiddleDataSetReport_New(reportName);
                StiWebDesigner2.Report = report;
            }
            
            //ReportModalPopupExtender1.Show();
            Session["wc_second"] = "true";
            StiWebDesigner2.Visible = true;
        }

    }

    protected void ImageButton9_Click(object sender, ImageClickEventArgs e)
    {
        if (Request.QueryString["bill"] == null)
        {
            string reportName = ddlTopChecksForLoad.SelectedItem.Text.Trim();
            // mpeTemplate.Hide();
            StiReport report = FillMaddenDataSetForReport(reportName);
            StiWebDesigner3.Report = report;
            //ReportModalPopupExtender2.Show();
            Session["wc_third"] = "true";
            StiWebDesigner3.Visible = true;
        }
        else if (Request.QueryString["bill"] == "c")
        {
            if (Request.QueryString["vid"] != null)
            {
                string reportName = ddlTopChecksForLoad.SelectedItem.Text.Trim();
                // mpeTemplate.Hide();
                StiReport report = FillMaddenDataSetForReport(reportName);
                StiWebDesigner3.Report = report;
                //ReportModalPopupExtender2.Show();
                
            }
            else
            {
                string reportName = ddlTopChecksForLoad.SelectedItem.Text.Trim();
                // mpeTemplate.Hide();
                StiReport report = FillMaddenDataSetForReport_New(reportName);
                StiWebDesigner3.Report = report;
                //ReportModalPopupExtender2.Show();
                
            }
            Session["wc_third"] = "true";
            StiWebDesigner3.Visible = true;
        }
    }



    protected void ddlApTopCheck_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void lnkSaveDefault_Click(object sender, EventArgs e)
    {
        try
        {
            string defaultpath = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/ApTopCheckDefault.mrt");
            string filePath = Server.MapPath("StimulsoftReports/APChecks/APTopCheck");
            string tempPath = Server.MapPath("StimulsoftReports/APChecks/APTopCheck");
            string selValue = ddlApTopCheckForLoad.Text.TrimEnd();
            if (selValue != null)
            {
                filePath = filePath + "\\" + selValue + ".mrt";
                tempPath = tempPath + "\\" + selValue + "temp.mrt";
                if (File.Exists(defaultpath))
                {
                    string[] lines = System.IO.File.ReadAllLines(defaultpath);
                    var myfile = File.Create(tempPath);
                    myfile.Close();
                    using (TextWriter tw = new StreamWriter(tempPath))
                        foreach (string line in lines)
                        {
                            tw.WriteLine(line);
                        }
                    File.Delete(defaultpath);
                    if (File.Exists(filePath))
                    {
                        string[] lines1 = System.IO.File.ReadAllLines(filePath);
                        var myfile1 = File.Create(defaultpath);
                        myfile1.Close();
                        using (TextWriter tw1 = new StreamWriter(defaultpath))
                            foreach (string line1 in lines1)
                            {
                                tw1.WriteLine(line1);
                            }
                        File.Delete(filePath);
                    }
                    if (File.Exists(tempPath))
                    {
                        string[] lines2 = System.IO.File.ReadAllLines(tempPath);
                        var myfile2 = File.Create(filePath);
                        myfile2.Close();
                        using (TextWriter tw2 = new StreamWriter(filePath))
                            foreach (string line2 in lines2)
                            {
                                tw2.WriteLine(line2);
                            }
                        File.Delete(tempPath);
                    }
                    Response.Redirect("WriteChecks.aspx");

                }
                else
                    throw new Exception("ApTopCheckDefault.mrt is not available");

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }


    }

    protected void StiWebDesigner1_SaveReportAs(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/APTopCheck/" + e.FileName));
    }
    protected void StiWebDesigner1_Exit(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        Response.Redirect("WriteChecks.aspx");
    }


    protected void StiWebDesigner2_SaveReportAs(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/APMidCheck/" + e.FileName));
    }

    protected void StiWebDesigner3_SaveReportAs(object sender, Stimulsoft.Report.Web.StiSaveReportEventArgs e)
    {
        StiReport oRep = e.Report;
        e.Report.Save(Server.MapPath("StimulsoftReports/APChecks/TopChecks/" + e.FileName));
    }




    protected void StiWebDesigner2_Exit(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        Response.Redirect("WriteChecks.aspx");

    }

    protected void StiWebDesigner3_Exit(object sender, Stimulsoft.Report.Web.StiReportDataEventArgs e)
    {
        Response.Redirect("WriteChecks.aspx");
    }

    protected void lnkSaveApMiddleCheck_Click(object sender, EventArgs e)
    {

        try
        {
            string defaultpath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/APMidCheckDefault.mrt");
            string filePath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck");
            string tempPath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck");

            string selValue = ddlApMiddleCheckForLoad.Text.TrimEnd();
            if (selValue != null)
            {
                filePath = filePath + "\\" + selValue + ".mrt";
                tempPath = tempPath + "\\" + selValue + "temp.mrt";
                if (File.Exists(defaultpath))
                {
                    string[] lines = System.IO.File.ReadAllLines(defaultpath);
                    var myfile = File.Create(tempPath);
                    myfile.Close();
                    using (TextWriter tw = new StreamWriter(tempPath))
                        foreach (string line in lines)
                        {
                            tw.WriteLine(line);
                        }
                    File.Delete(defaultpath);
                    if (File.Exists(filePath))
                    {
                        string[] lines1 = System.IO.File.ReadAllLines(filePath);
                        var myfile1 = File.Create(defaultpath);
                        myfile1.Close();
                        using (TextWriter tw1 = new StreamWriter(defaultpath))
                            foreach (string line1 in lines1)
                            {
                                tw1.WriteLine(line1);
                            }
                        File.Delete(filePath);
                    }
                    if (File.Exists(tempPath))
                    {
                        string[] lines2 = System.IO.File.ReadAllLines(tempPath);
                        var myfile2 = File.Create(filePath);
                        myfile2.Close();
                        using (TextWriter tw2 = new StreamWriter(filePath))
                            foreach (string line2 in lines2)
                            {
                                tw2.WriteLine(line2);
                            }
                        File.Delete(tempPath);
                    }
                    Response.Redirect("WriteChecks.aspx");

                }
                else
                    throw new Exception("ApMiddleCheckDefault.mrt is not available");

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }


    }

    protected void lnkTopChecks_Click(object sender, EventArgs e)
    {

        try
        {
            string defaultpath = Server.MapPath("StimulsoftReports/APChecks/TopChecks/TopCheckReportDefault.mrt");
            string filePath = Server.MapPath("StimulsoftReports/APChecks/TopChecks");
            string tempPath = Server.MapPath("StimulsoftReports/APChecks/TopChecks");
            string selValue = ddlTopChecksForLoad.Text.TrimEnd();
            if (selValue != null)
            {
                filePath = filePath + "\\" + selValue + ".mrt";
                tempPath = tempPath + "\\" + selValue + "temp.mrt";
                if (File.Exists(defaultpath))
                {
                    string[] lines = System.IO.File.ReadAllLines(defaultpath);
                    var myfile = File.Create(tempPath);
                    myfile.Close();
                    using (TextWriter tw = new StreamWriter(tempPath))
                        foreach (string line in lines)
                        {
                            tw.WriteLine(line);
                        }
                    File.Delete(defaultpath);
                    if (File.Exists(filePath))
                    {
                        string[] lines1 = System.IO.File.ReadAllLines(filePath);
                        var myfile1 = File.Create(defaultpath);
                        myfile1.Close();
                        using (TextWriter tw1 = new StreamWriter(defaultpath))
                            foreach (string line1 in lines1)
                            {
                                tw1.WriteLine(line1);
                            }
                        File.Delete(filePath);
                    }
                    if (File.Exists(tempPath))
                    {
                        string[] lines2 = System.IO.File.ReadAllLines(tempPath);
                        var myfile2 = File.Create(filePath);
                        myfile2.Close();
                        using (TextWriter tw2 = new StreamWriter(filePath))
                            foreach (string line2 in lines2)
                            {
                                tw2.WriteLine(line2);
                            }
                        File.Delete(tempPath);
                    }
                    Response.Redirect("WriteChecks.aspx");

                }
                else
                    throw new Exception("TopCheckReportDefault.mrt is not available");

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }

    }

    protected void ddlTopChecksForLoad_SelectedIndexChanged(object sender, EventArgs e)
    {

    }



    protected void lnkCancelTopCheck_Click(object sender, EventArgs e)
    {
        Response.Redirect("WriteChecks.aspx");
    }

    protected void ddlApMiddleCheck_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void lnkDeleteApMiddleCheck_Click(object sender, EventArgs e)
    {

    }

    protected void lnkEditApMiddleCheck_Click(object sender, EventArgs e)
    {

    }

    protected void lnkCancelApMiddleCheck_Click(object sender, EventArgs e)
    {
        Response.Redirect("WriteChecks.aspx");
    }

    protected void ddlApTopCheckForLoad_SelectedIndexChanged(object sender, EventArgs e)
    {


    }

    protected void lnkDeleteApTopCheck_Click(object sender, EventArgs e)
    {

    }

    protected void lnkEditApTopCheck_Click(object sender, EventArgs e)
    {
        //mpeTemplate.Hide();
        StiReport report = FillDataSetToReport("APTopCheckDefault");
        StiWebDesigner1.Report = report;
        //ReportModalPopupExtender.Show();
        Session["wc_first"] = "true";
        StiWebDesigner1.Visible = true;

    }

    protected void lnkCancelApTopCheck_Click(object sender, EventArgs e)
    {
        Response.Redirect("WriteChecks.aspx");
    }

    protected void ddlApMiddleCheckForLoad_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
    {

        string filePath = Server.MapPath("StimulsoftReports/APChecks/APTopCheck");

        string selValue = ddlApTopCheckForLoad.Text.Trim();
        if (selValue != null)
        {
            filePath = filePath + "\\" + selValue + ".mrt";
            if (!selValue.Equals("ApTopCheckDefault"))
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                ddlApTopCheckForLoad.Items.Clear();

                string path = Server.MapPath("StimulsoftReports/APChecks/APTopCheck/");
                DirectoryInfo d = new DirectoryInfo(path);
                FileInfo[] Files = d.GetFiles("*.mrt");
                foreach (FileInfo file in Files)
                {
                    string FileName = string.Empty;
                    if (file.Name.Contains(".mrt"))
                        FileName = file.Name.Replace(".mrt", " ");
                    ddlApTopCheckForLoad.Items.Add((FileName));
                }
                ddlApTopCheckForLoad.Items.Remove(selValue);

                ddlApTopCheckForLoad.DataBind();
                string str = "Template " + selValue + " Deleted!--";
                if (Request.QueryString["bill"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", " window.parent.document.getElementById('btnCancel').click(); noty({text: '" + str + " </br> <b>', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                }
                
            }
            btnCutCheck.Visible = true;
        }
    }

    protected void ImageButton6_Click(object sender, ImageClickEventArgs e)
    {

        string filePath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck");

        string selValue = ddlApMiddleCheckForLoad.Text.Trim();
        if (selValue != null)
        {
            filePath = filePath + "\\" + selValue + ".mrt";
            if (!selValue.Equals("APMidCheckDefault"))
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                ddlApMiddleCheckForLoad.Items.Clear();
                string MidCheckpath = Server.MapPath("StimulsoftReports/APChecks/APMidCheck/");
                DirectoryInfo dirMidPath = new DirectoryInfo(MidCheckpath);
                FileInfo[] FilesMid = dirMidPath.GetFiles("*.mrt");
                foreach (FileInfo fileMid in FilesMid)
                {
                    string FileName = string.Empty;
                    if (fileMid.Name.Contains(".mrt"))
                        FileName = fileMid.Name.Replace(".mrt", " ");
                    ddlApMiddleCheckForLoad.Items.Add((FileName));
                }
                ddlApMiddleCheckForLoad.Items.Remove(selValue);
                ddlApMiddleCheckForLoad.DataBind();
                string str = "Template " + selValue + " Deleted!--";
                if (Request.QueryString["bill"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", " window.parent.document.getElementById('btnCancel').click(); noty({text: '" + str + " </br> <b>', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            btnCutCheck.Visible = true;
        }
    }

    protected void ImageButton14_Click(object sender, ImageClickEventArgs e)
    {
        string filePath = Server.MapPath("StimulsoftReports/APChecks/TopChecks");

        string selValue = ddlTopChecksForLoad.Text.Trim();
        if (selValue != null)
        {
            filePath = filePath + "\\" + selValue + ".mrt";
            if (!selValue.Equals("TopCheckReportDefault"))
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                ddlTopChecksForLoad.Items.Clear();

                string TopCheckpath = Server.MapPath("StimulsoftReports/APChecks/TopChecks/");
                DirectoryInfo dirTopcheckPath = new DirectoryInfo(TopCheckpath);
                FileInfo[] FilesTop = dirTopcheckPath.GetFiles("*.mrt");
                foreach (FileInfo fileTop in FilesTop)
                {
                    string FileName = string.Empty;
                    if (fileTop.Name.Contains(".mrt"))
                        FileName = fileTop.Name.Replace(".mrt", " ");
                    ddlTopChecksForLoad.Items.Add((FileName));
                }
                ddlTopChecksForLoad.Items.Remove((selValue));
                ddlTopChecksForLoad.DataBind();
                string str = "Template " + selValue + " Deleted!--";
                if (Request.QueryString["bill"] == null)
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", " window.parent.document.getElementById('btnCancel').click(); noty({text: '" + str + " </br> <b>', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
        }
        btnCutCheck.Visible = true;

    }

    #endregion

    public bool CheckNumValid(string checkno, string bank,string Paymenttype)
    {
        CD _objCD = new CD();
        BL_Bills _objBLBills = new BL_Bills();
        _objCD.IsExistCheckNo = false;
        _isExistCheckNum.IsExistCheckNo = false;

        if (!string.IsNullOrEmpty(checkno) && !string.IsNullOrEmpty(bank))
        {
            _objCD.ConnConfig = Session["config"].ToString();
            _objCD.Ref = long.Parse(checkno);
            _objCD.Bank = Convert.ToInt32(bank);
            _objCD.Vendor = Convert.ToInt32(bank);
            _isExistCheckNum.ConnConfig = Session["config"].ToString();
            _isExistCheckNum.Ref = long.Parse(checkno);
            _isExistCheckNum.Bank = Convert.ToInt32(bank);
            //_isExistCheckNum.Vendor = Convert.ToInt32(bank);

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/AddCheck_IsExistCheckNum";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _isExistCheckNum);
                _objCD.IsExistCheckNo = Convert.ToBoolean(_APIResponse.ResponseData);
            }
            else
            {
                _objCD.IsExistCheckNo = _objBLBills.IsExistCheckNum(_objCD);
            }
        }

        return _objCD.IsExistCheckNo;
    }
    protected void btnSubmit_Click1(object sender, EventArgs e)
    {
        if (txtNextCheck.Text .Trim() == "")
        {
            string str = "Check #" + txtNextCheck.Text + " already exists. Since duplicate check numbers are not supported, the check generation process cannot continue.";
            //criptManager.RegisterStartupScript(this, Page.GetType(), "keyProj1", "  noty({text: '" + str + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true , closeWith: ['click'], callback: { onShow: function() { }, afterShow: function() { }, onClose: function() { location.reload(); }, afterClose: function() { } }, buttons: false });", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true}); $('#MOMloading').hide();", true);
            return;
        }
        if (Request.QueryString["bill"] == null)
        {
            GetPeriodDetails(Convert.ToDateTime(txtDate.Text));     //Check period closed out permission
            bool Flag = (bool)ViewState["FlagPeriodClose"];
            bool IsCheckNoExists = CheckNumValid(txtNextCheck.Text,ddlBank.SelectedValue.ToString(),ddlPayment.SelectedValue.ToString());   
            try
            {
                if (Flag)
                {
                    if (!IsCheckNoExists)
                    {
                        bool chk = CheckNegativeBill();
                        if (chk == true)
                        {
                            string script = "function f(){$find(\"" + RadWindowTemplates.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                        }
                    }
                    else
                    {
                        string str = "Check #" + txtNextCheck.Text + " already exists. Since duplicate check numbers are not supported, the check generation process cannot continue.";
                        //criptManager.RegisterStartupScript(this, Page.GetType(), "keyProj1", "  noty({text: '" + str + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true , closeWith: ['click'], callback: { onShow: function() { }, afterShow: function() { }, onClose: function() { location.reload(); }, afterClose: function() { } }, buttons: false });", true);
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true}); $('#MOMloading').hide();", true);
                        return;
                    }
                }
                else
                {
                    Exception ex = new Exception();
                    throw ex;

                }
            }
            catch (Exception ex)
            {
                string str = "These month/year period is closed out. You do not have permission to process the check.";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  noty({text: '" + str + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true , closeWith: ['click'], callback: { onShow: function() { }, afterShow: function() { }, onClose: function() { location.reload(); }, afterClose: function() { } }, buttons: false }); $('#MOMloading').hide();", true);
            }
        }
        if (Request.QueryString["bill"] == "c" && Request.QueryString["vid"] != null)
        {
            GetPeriodDetails(Convert.ToDateTime(txtDate.Text));     //Check period closed out permission
            bool Flag = (bool)ViewState["FlagPeriodClose"];
            bool IsCheckNoExists = CheckNumValid(txtNextCheck.Text, ddlBank.SelectedValue.ToString(),ddlPayment.SelectedValue.ToString());
            try
            {
                if (Flag)
                {
                    if (!IsCheckNoExists)
                    {
                        bool chk = CheckNegativeBill();
                        if (chk == true)
                        {
                            string script = "function f(){$find(\"" + RadWindowTemplates.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                        }
                    }
                    else
                    {
                        string str = "Check #" + txtNextCheck.Text + " already exists. Since duplicate check numbers are not supported, the check generation process cannot continue.";
                        //criptManager.RegisterStartupScript(this, Page.GetType(), "keyProj1", "  noty({text: '" + str + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true , closeWith: ['click'], callback: { onShow: function() { }, afterShow: function() { }, onClose: function() { location.reload(); }, afterClose: function() { } }, buttons: false });", true);
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true}); $('#MOMloading').hide();", true);
                        return;
                    }
                }
                else
                {
                    Exception ex = new Exception();
                    throw ex;

                }
            }
            catch (Exception ex)
            {
                string str = "These month/year period is closed out. You do not have permission to process the check.";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  noty({text: '" + str + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true , closeWith: ['click'], callback: { onShow: function() { }, afterShow: function() { }, onClose: function() { location.reload(); }, afterClose: function() { } }, buttons: false }); $('#MOMloading').hide();", true);
            }
        }
        else if (Request.QueryString["bill"] == "c")
        {


            GetPeriodDetails(Convert.ToDateTime(txtDate.Text));     //Check period closed out permission
            bool Flagt = (bool)ViewState["FlagPeriodClose"];
            bool IsCheckNoExists = CheckNumValid(txtNextCheck.Text, ddlBank.SelectedValue.ToString(),ddlPayment.SelectedValue.ToString());
            try
            {
                if (Flagt)
                {
                    if (chkIsRecurr.Checked == false)
                    {
                        if (!IsCheckNoExists)
                        {
                            bool chks = CheckNegativeBill();
                            if (chks == true)
                            {
                                string script = "function f(){$find(\"" + RadWindowTemplates.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                            }
                        }
                        else
                        {
                            string str = "Check #" + txtNextCheck.Text + " already exists. Since duplicate check numbers are not supported, the check generation process cannot continue.";
                            //criptManager.RegisterStartupScript(this, Page.GetType(), "keyProj1", "  noty({text: '" + str + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true , closeWith: ['click'], callback: { onShow: function() { }, afterShow: function() { }, onClose: function() { location.reload(); }, afterClose: function() { } }, buttons: false });", true);
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true}); $('#MOMloading').hide();", true);
                            return;
                        }
                    }
                }
                else
                {
                    Exception ex = new Exception();
                    throw ex;

                }

                bool chk1 = CheckNegativeBill();
                if (chk1 == true)
                {
                    if (RadGrid_gvJobCostItems.Items.Count > 0)
                    {



                        DataTable dt = GetTransaction();
                        if (dt.Rows.Count > 0)
                        {
                            if (ValidateGrid(dt))
                            {
                                if (chkIsRecurr.Checked)
                                {
                                    dt.Columns.Remove("AcctNo");
                                    dt.Columns.Remove("JobName");
                                    dt.Columns.Remove("UName");
                                    dt.Columns.Remove("Loc");
                                    dt.Columns.Remove("Line");
                                    dt.Columns.Remove("PrvInQuan");
                                    dt.Columns.Remove("PrvIn");
                                    dt.Columns.Remove("OutstandQuan");
                                    dt.Columns.Remove("OutstandBalance");
                                    dt.Columns.Remove("AmountTot");
                                    dt.Select("JobID = 0")
                                          .AsEnumerable().ToList()
                                          .ForEach(t => t["JobID"] = DBNull.Value);

                                    dt.Select("ItemID = 0")
                                        .AsEnumerable().ToList()
                                        .ForEach(t => t["ItemID"] = DBNull.Value);

                                    dt.AcceptChanges();

                                    _objPJ.ConnConfig = Session["config"].ToString();
                                    _addBills.ConnConfig = Session["config"].ToString();
                                    try
                                    {
                                        _objPJ.Vendor = Convert.ToInt32(hdnVendorID.Value);
                                        _addBills.Vendor = Convert.ToInt32(hdnVendorID.Value);
                                    }
                                    catch (Exception)
                                    {
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: 'Cannot find vendor information! Please help to re-fill the vendor again.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true}); $('#MOMloading').hide();", true);
                                        return;
                                    }
                                    _objPJ.fDate = Convert.ToDateTime(txtDate.Text);
                                    _objPJ.PostDate = Convert.ToDateTime(txtDate.Text);
                                    _objPJ.Due = Convert.ToDateTime(txtDate.Text);
                                    //_objPJ.Ref = "INV-CHK-" + txtNextCheck.Text;
                                    _objPJ.Ref = txtbillref.Text;
                                    _objPJ.fDesc = txtMemo1.Text;
                                    _objPJ.Terms = Convert.ToInt16("0");
                                    _objPJ.Spec = Convert.ToInt16("0");
                                    _objPJ.MOMUSer = Session["User"].ToString();
                                    _objPJ.Dt = dt;
                                    _objPJ.Status = 0;
                                    _objPJ.IsRecurring = chkIsRecurr.Checked;

                                    _addBills.fDate = Convert.ToDateTime(txtDate.Text);
                                    _addBills.PostDate = Convert.ToDateTime(txtDate.Text);
                                    _addBills.Due = Convert.ToDateTime(txtDate.Text);
                                    _addBills.Ref = txtbillref.Text;
                                    _addBills.fDesc = txtMemo1.Text;
                                    _addBills.Terms = Convert.ToInt16("0");
                                    _addBills.Spec = Convert.ToInt16("0");
                                    _addBills.MOMUSer = Session["User"].ToString();
                                    _addBills.Dt = dt;
                                    _addBills.Status = 0;
                                    _addBills.IsRecurring = chkIsRecurr.Checked;

                                    if (chkIsRecurr.Checked)
                                    {
                                        _objPJ.Frequency = Convert.ToInt16(ddlFrequency.SelectedValue);
                                        _addBills.Frequency = Convert.ToInt16(ddlFrequency.SelectedValue);
                                    }

                                    ///////////// Start - ES-3274 Data need to save at bill level only ////////

                                    _objPJ.STax = Convert.ToDouble(dt.Compute("SUM(StaxAmt)", string.Empty));
                                    _addBills.STax = Convert.ToDouble(dt.Compute("SUM(StaxAmt)", string.Empty));
                                    if (!string.IsNullOrEmpty(hdnQST.Value))
                                    {
                                        _objPJ.STaxRate = Convert.ToDouble(hdnQST.Value);
                                        _addBills.STaxRate = Convert.ToDouble(hdnQST.Value);
                                    }
                                    else
                                    {
                                        _objPJ.STaxRate = 0;
                                        _addBills.STaxRate = 0;
                                    }
                                    if (!string.IsNullOrEmpty(hdnQSTGL.Value))
                                    {
                                        _objPJ.STaxGL = Convert.ToInt32(hdnQSTGL.Value);
                                        _addBills.STaxGL = Convert.ToInt32(hdnQSTGL.Value);
                                    }
                                    else
                                    {
                                        _objPJ.STaxGL = 0;
                                        _addBills.STaxGL = 0;
                                    }

                                    _objPJ.STaxName = hdnSTaxName.Value.ToString();
                                    //_objPJ.STaxGL = Convert.ToInt32(hdnQSTGL.Value);
                                    _objPJ.UTax = Convert.ToDouble(dt.Compute("SUM(GTaxAmt)", string.Empty));
                                    //_objPJ.UTaxRate = Convert.ToDouble(husetaxRate.Value);
                                    _objPJ.UTaxName = hdnUTaxName.Value.ToString();
                                    //_objPJ.UTaxGL = Convert.ToInt32(husetaxGL.Value);

                                    _addBills.STaxName = hdnSTaxName.Value.ToString();
                                    _addBills.UTax = Convert.ToDouble(dt.Compute("SUM(GTaxAmt)", string.Empty));
                                    _addBills.UTaxName = hdnUTaxName.Value.ToString();

                                    if (!string.IsNullOrEmpty(hdnusetaxc.Value))
                                    {
                                        _objPJ.UTaxRate = Convert.ToDouble(hdnusetaxc.Value);
                                        _addBills.UTaxRate = Convert.ToDouble(hdnusetaxc.Value);
                                    }
                                    else
                                    {
                                        _objPJ.UTaxRate = 0;
                                        _addBills.UTaxRate = 0;
                                    }
                                    if (!string.IsNullOrEmpty(hdnusetaxcGL.Value))
                                    {
                                        _objPJ.UTaxGL = Convert.ToInt32(hdnusetaxcGL.Value);
                                        _addBills.UTaxGL = Convert.ToInt32(hdnusetaxcGL.Value);
                                    }
                                    else
                                    {
                                        _objPJ.UTaxGL = 0;
                                        _addBills.UTaxGL = 0;
                                    }

                                    if (!string.IsNullOrEmpty(hdnGSTGL.Value))
                                    {
                                        _objPJ.GSTGL = Convert.ToInt32(hdnGSTGL.Value);
                                        _addBills.GSTGL = Convert.ToInt32(hdnGSTGL.Value);
                                    }
                                    else
                                    {
                                        _objPJ.GSTGL = 0;
                                        _addBills.GSTGL = 0;
                                    }
                                    if (!string.IsNullOrEmpty(hdnGST.Value))
                                    {
                                        _objPJ.GSTRate = Convert.ToDouble(hdnGST.Value);
                                        _addBills.GSTRate = Convert.ToDouble(hdnGST.Value);
                                    }
                                    else
                                    {
                                        _objPJ.GSTRate = 0;
                                        _addBills.GSTRate = 0;
                                    }
                                    _objPJ.GST = Convert.ToDouble(dt.Compute("SUM(GTaxAmt)", string.Empty));
                                    _addBills.GST = Convert.ToDouble(dt.Compute("SUM(GTaxAmt)", string.Empty));
                                    _objPJ.IsPOClose = false;
                                    _addBills.IsPOClose = false;

                                    //////////////// End - ES-3274 Data need to save at bill level only ////////



                                    string strpjid;

                                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                                //if (IsAPIIntegrationEnable == "YES")
                                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                                {
                                    string APINAME = "ManageChecksAPI/AddCheck_AddBills";

                                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _addBills);

                                        strpjid = _APIResponse.ResponseData;
                                        object JsonData = JsonConvert.DeserializeObject(_APIResponse.ResponseData);
                                        strpjid = JsonData.ToString();
                                        //strpjid = strpjid.Split('/')[0]; "/""2" "2" {}  )}
                                    }
                                    else
                                    {
                                        strpjid = _objBLBills.AddBills(_objPJ);
                                    }

                                    //AddCheckDetails();
                                    _objCD.ConnConfig = Session["config"].ToString();
                                    double sumAmount = Convert.ToDouble(dt.Compute("SUM(Amount)", string.Empty));
                                    _objCD.Amount = sumAmount;
                                    _objCD.fDateYear = Convert.ToInt32(ddlFrequency.SelectedValue);
                                    _objCD.TransID = Convert.ToInt32(strpjid);
                                    // _objCD.fDate = DateTime.Now;
                                    _objCD.fDate = Convert.ToDateTime(txtDate.Text);
                                    _objCD.NextC = long.Parse(txtNextCheck.Text);
                                    _objCD.fDesc = txtVendor.Text;
                                    _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);
                                    _objCD.Vendor = Convert.ToInt32(hdnVendorID.Value);
                                    _objCD.Memo = txtMemo.Text;

                                    _addCheckRecurr.ConnConfig = Session["config"].ToString();
                                    _addCheckRecurr.Amount = sumAmount;
                                    _addCheckRecurr.fDateYear = Convert.ToInt32(ddlFrequency.SelectedValue);
                                    _addCheckRecurr.TransID = Convert.ToInt32(strpjid);
                                    _addCheckRecurr.fDate = Convert.ToDateTime(txtDate.Text);
                                    _addCheckRecurr.NextC = long.Parse(txtNextCheck.Text);
                                    _addCheckRecurr.fDesc = txtVendor.Text;
                                    _addCheckRecurr.Bank = Convert.ToInt32(ddlBank.SelectedValue);
                                    _addCheckRecurr.Vendor = Convert.ToInt32(hdnVendorID.Value);
                                    _addCheckRecurr.Memo = txtMemo.Text;

                                    if (!string.IsNullOrEmpty(hdnDiscGL.Value))
                                    {
                                        _objCD.DiscGL = Convert.ToInt32(hdnDiscGL.Value);
                                        _addCheckRecurr.DiscGL = Convert.ToInt32(hdnDiscGL.Value);
                                    }

                                    _objCD.Type = Convert.ToInt16(ddlPayment.SelectedValue);
                                    _objCD.MOMUSer = Session["Username"].ToString();

                                    _addCheckRecurr.Type = Convert.ToInt16(ddlPayment.SelectedValue);
                                    _addCheckRecurr.MOMUSer = Session["Username"].ToString();

                                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                                    //if (IsAPIIntegrationEnable == "YES")
                                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                                    {
                                        string APINAME = "ManageChecksAPI/AddCheck_AddCheckRecurr";

                                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _addCheckRecurr);
                                        _objCD.ID = Convert.ToInt32(_APIResponse.ResponseData);
                                    }
                                    else
                                    {
                                        _objCD.ID = _objBLBill.AddCheckRecurr(_objCD);
                                    }

                                    ResetFormControlValues(this);
                                    ResetForm();
                                    btnSubmit.Visible = false;
                                    btnCutCheck.Visible = true;
                                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  noty({text: 'Recurring checks saved successfully! </br> <b>', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});  window.setTimeout(function () { window.location.href = 'WriteChecks.aspx'; }, 500); ", true);

                                }
                                else
                                {
                                    dt.Columns.Remove("AcctNo");
                                    dt.Columns.Remove("JobName");
                                    dt.Columns.Remove("UName");
                                    dt.Columns.Remove("Loc");
                                    dt.Columns.Remove("Line");
                                    dt.Columns.Remove("PrvInQuan");
                                    dt.Columns.Remove("PrvIn");
                                    dt.Columns.Remove("OutstandQuan");
                                    dt.Columns.Remove("OutstandBalance");
                                    dt.Columns.Remove("AmountTot");
                                    dt.Select("JobID = 0")
                                          .AsEnumerable().ToList()
                                          .ForEach(t => t["JobID"] = DBNull.Value);

                                    dt.Select("ItemID = 0")
                                        .AsEnumerable().ToList()
                                        .ForEach(t => t["ItemID"] = DBNull.Value);

                                    dt.AcceptChanges();

                                    _objPJ.ConnConfig = Session["config"].ToString();
                                    _addBills.ConnConfig = Session["config"].ToString();
                                    try
                                    {
                                        _objPJ.Vendor = Convert.ToInt32(hdnVendorID.Value);
                                        _addBills.Vendor = Convert.ToInt32(hdnVendorID.Value);
                                    }
                                    catch (Exception)
                                    {
                                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "noty({text: 'Cannot find vendor information! Please help to re-fill the vendor again.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : 5000,theme : 'noty_theme_default',  closable : true}); $('#MOMloading').hide();", true);
                                        return;
                                    }
                                    _objPJ.fDate = Convert.ToDateTime(txtDate.Text);
                                    _objPJ.PostDate = Convert.ToDateTime(txtDate.Text);
                                    _objPJ.Due = Convert.ToDateTime(txtDate.Text);
                                    //_objPJ.Ref = "INV-CHK-" + txtNextCheck.Text;
                                    _objPJ.Ref = txtbillref.Text;
                                    _objPJ.fDesc = txtMemo1.Text;
                                    _objPJ.Terms = Convert.ToInt16("0");
                                    _objPJ.Spec = Convert.ToInt16("0");
                                    _objPJ.MOMUSer = Session["User"].ToString();

                                    _objPJ.Dt = dt;
                                    _objPJ.Status = 0;
                                    _objPJ.IsRecurring = false;



                                    _addBills.fDate = Convert.ToDateTime(txtDate.Text);
                                    _addBills.PostDate = Convert.ToDateTime(txtDate.Text);
                                    _addBills.Due = Convert.ToDateTime(txtDate.Text);
                                    _addBills.Ref = txtbillref.Text;
                                    _addBills.fDesc = txtMemo1.Text;
                                    _addBills.Terms = Convert.ToInt16("0");
                                    _addBills.Spec = Convert.ToInt16("0");
                                    _addBills.MOMUSer = Session["User"].ToString();
                                    _addBills.Dt = dt;
                                    _addBills.Status = 0;
                                    _addBills.IsRecurring = false;

                                    ///////////// Start - ES-3274 Data need to save at bill level only ////////

                                    _objPJ.STax = Convert.ToDouble(dt.Compute("SUM(StaxAmt)", string.Empty));
                                    _addBills.STax = Convert.ToDouble(dt.Compute("SUM(StaxAmt)", string.Empty));
                                    if (!string.IsNullOrEmpty(hdnQST.Value))
                                    {
                                        _objPJ.STaxRate = Convert.ToDouble(hdnQST.Value);
                                        _addBills.STaxRate = Convert.ToDouble(hdnQST.Value);
                                    }
                                    else
                                    {
                                        _objPJ.STaxRate = 0;
                                        _addBills.STaxRate = 0;
                                    }
                                    if (!string.IsNullOrEmpty(hdnQSTGL.Value))
                                    {
                                        _objPJ.STaxGL = Convert.ToInt32(hdnQSTGL.Value);
                                        _addBills.STaxGL = Convert.ToInt32(hdnQSTGL.Value);
                                    }
                                    else
                                    {
                                        _objPJ.STaxGL = 0;
                                        _addBills.STaxGL = 0;
                                    }
                                    _objPJ.STaxName = hdnSTaxName.Value.ToString();
                                    //_objPJ.STaxGL = Convert.ToInt32(hdnQSTGL.Value);
                                    _objPJ.UTax = Convert.ToDouble(dt.Compute("SUM(GTaxAmt)", string.Empty));
                                    //_objPJ.UTaxRate = Convert.ToDouble(husetaxRate.Value);
                                    _objPJ.UTaxName = hdnUTaxName.Value.ToString();
                                    //_objPJ.UTaxGL = Convert.ToInt32(husetaxGL.Value);

                                    _addBills.STaxName = hdnSTaxName.Value.ToString();
                                    _addBills.UTax = Convert.ToDouble(dt.Compute("SUM(GTaxAmt)", string.Empty));
                                    _addBills.UTaxName = hdnUTaxName.Value.ToString();

                                    if (!string.IsNullOrEmpty(hdnusetaxc.Value))
                                    {
                                        _objPJ.UTaxRate = Convert.ToDouble(hdnusetaxc.Value);
                                        _addBills.UTaxRate = Convert.ToDouble(hdnusetaxc.Value);
                                    }
                                    else
                                    {
                                        _objPJ.UTaxRate = 0;
                                        _addBills.UTaxRate = 0;
                                    }
                                    if (!string.IsNullOrEmpty(hdnusetaxcGL.Value))
                                    {
                                        _objPJ.UTaxGL = Convert.ToInt32(hdnusetaxcGL.Value);
                                        _addBills.UTaxGL = Convert.ToInt32(hdnusetaxcGL.Value);
                                    }
                                    else
                                    {
                                        _objPJ.UTaxGL = 0;
                                        _addBills.UTaxGL = 0;
                                    }

                                    if (!string.IsNullOrEmpty(hdnGSTGL.Value))
                                    {
                                        _objPJ.GSTGL = Convert.ToInt32(hdnGSTGL.Value);
                                        _addBills.GSTGL = Convert.ToInt32(hdnGSTGL.Value);
                                    }
                                    else
                                    {
                                        _objPJ.GSTGL = 0;
                                        _addBills.GSTGL = 0;
                                    }
                                    if (!string.IsNullOrEmpty(hdnGST.Value))
                                    {
                                        _objPJ.GSTRate = Convert.ToDouble(hdnGST.Value);
                                        _addBills.GSTRate = Convert.ToDouble(hdnGST.Value);
                                    }
                                    else
                                    {
                                        _objPJ.GSTRate = 0;
                                        _addBills.GSTRate = 0;
                                    }
                                    _objPJ.GST = Convert.ToDouble(dt.Compute("SUM(GTaxAmt)", string.Empty));
                                    _addBills.GST = Convert.ToDouble(dt.Compute("SUM(GTaxAmt)", string.Empty));
                                    _objPJ.IsPOClose = false;
                                    _addBills.IsPOClose = false;

                                    string returnPJID = "";
                                    //////////////// End - ES-3274 Data need to save at bill level only ////////

                                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                                    //if (IsAPIIntegrationEnable == "YES")
                                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                                    {
                                    string APINAME = "ManageChecksAPI/AddCheck_AddBills";

                                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _addBills);
                                    }
                                    else
                                    {
                                        returnPJID = _objBLBills.AddBills(_objPJ);
                                    }

                                    //////////////////////////////
                                    _objOpenAP.ConnConfig = Session["config"].ToString();
                                    _objOpenAP.Vendor = Convert.ToInt32(hdnVendorID.Value);
                                    _objOpenAP.SearchValue = Convert.ToInt16(ddlInvoice.SelectedValue);
                                    _objOpenAP.Company = Convert.ToString(Session["company"].ToString());
                                    _getBillsByVendor.ConnConfig = Session["config"].ToString();
                                    _getBillsByVendor.Vendor = Convert.ToInt32(hdnVendorID.Value);
                                    _getBillsByVendor.SearchValue = Convert.ToInt16(ddlInvoice.SelectedValue);
                                    _getBillsByVendor.Company = Convert.ToString(Session["company"].ToString());
                                    if (_objOpenAP.SearchValue.Equals(2))
                                    {
                                        if (string.IsNullOrEmpty(txtSearchDate.Text))
                                        {
                                            _objOpenAP.SearchDate = DateTime.Now;
                                            txtSearchDate.Text = DateTime.Now.ToShortDateString();
                                        }
                                        else
                                        {
                                            _objOpenAP.SearchDate = Convert.ToDateTime(txtSearchDate.Text);
                                        }

                                    }

                                    if (_getBillsByVendor.SearchValue.Equals(2))
                                    {
                                        if (string.IsNullOrEmpty(txtSearchDate.Text))
                                        {
                                            _getBillsByVendor.SearchDate = DateTime.Now;
                                            txtSearchDate.Text = DateTime.Now.ToShortDateString();
                                        }
                                        else
                                        {
                                            _getBillsByVendor.SearchDate = Convert.ToDateTime(txtSearchDate.Text);
                                        }

                                    }

                                    DataSet _dsBills = new DataSet();
                                    List<OpenAPViewModel> _lstOpenAPViewModel = new List<OpenAPViewModel>();

                                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                                    //if (IsAPIIntegrationEnable == "YES")
                                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                                    {
                                        string APINAME = "ManageChecksAPI/AddCheck_GetBillsByVendor";

                                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getBillsByVendor);

                                        _lstOpenAPViewModel = (new JavaScriptSerializer()).Deserialize<List<OpenAPViewModel>>(_APIResponse.ResponseData);
                                        _dsBills = CommonMethods.ToDataSet<OpenAPViewModel>(_lstOpenAPViewModel);
                                    }
                                    else
                                    {
                                        _dsBills = _objBLBill.GetBillsByVendor(_objOpenAP);
                                    }

                                    //gvBills.VirtualItemCount = _dsBills.Tables[0].Rows.Count;
                                    //gvBills.DataSource = _dsBills;
                                    //gvBills.DataBind();
                                    //gvBills.Rebind();
                                    //Session["dsBills"] = _dsBills.Tables[0];
                                    //string sbill_no = "INV-CHK-" + txtNextCheck.Text;
                                    string sbill_no = txtbillref.Text;
                                    DataView dtview = _dsBills.Tables[0].DefaultView;
                                    dtview.RowFilter = "PJID = '" + returnPJID + "'";
                                    DataTable _final_table = dtview.ToTable(true);




                                    //AddCheckDetails();
                                    _objCD.ConnConfig = Session["config"].ToString();
                                    //_objCD.Dt = GetBillItems_New(_final_table);
                                    DataTable dtgl = GetBillItems_New(_final_table);
                                    dtgl.Merge(GetBillItems());
                                    _objCD.Dt = dtgl;
                                    // _objCD.fDate = DateTime.Now;
                                    _objCD.fDate = Convert.ToDateTime(txtDate.Text);
                                    _objCD.NextC = long.Parse(txtNextCheck.Text);



                                    _objCD.fDesc = txtVendor.Text;
                                    _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);
                                    _objCD.Vendor = Convert.ToInt32(hdnVendorID.Value);
                                    _objCD.Memo = txtMemo.Text;



                                    //API
                                    _addCheck.ConnConfig = Session["config"].ToString();
                                    _addCheck.Dt = dtgl;
                                    _addCheck.fDate = Convert.ToDateTime(txtDate.Text);
                                    _addCheck.NextC = long.Parse(txtNextCheck.Text);
                                    _addCheck.fDesc = txtVendor.Text;
                                    _addCheck.Bank = Convert.ToInt32(ddlBank.SelectedValue);
                                    _addCheck.Vendor = Convert.ToInt32(hdnVendorID.Value);
                                    _addCheck.Memo = txtMemo.Text;

                                    if (!string.IsNullOrEmpty(hdnDiscGL.Value))
                                    {
                                        _objCD.DiscGL = Convert.ToInt32(hdnDiscGL.Value);
                                        _addCheck.DiscGL = Convert.ToInt32(hdnDiscGL.Value);
                                    }

                                    _objCD.Type = Convert.ToInt16(ddlPayment.SelectedValue);
                                    _objCD.MOMUSer = Session["Username"].ToString();

                                    _addCheck.Type = Convert.ToInt16(ddlPayment.SelectedValue);
                                    _addCheck.MOMUSer = Session["Username"].ToString();

                                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                                    //if (IsAPIIntegrationEnable == "YES")
                                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                                    {
                                        string APINAME = "ManageChecksAPI/AddCheck";

                                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _addCheck);

                                        _objCD.ID = Convert.ToInt32(_APIResponse.ResponseData);
                                    }
                                    else
                                    {
                                        _objCD.ID = _objBLBill.AddCheck(_objCD);
                                    }


                                    string script = "function f(){$find(\"" + RadWindowTemplates.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                                }


                            }
                        }
                        else if (gvBills.Items.Count > 0)
                        {

                            if (IsValidDate())
                            {
                                GetPeriodDetails(Convert.ToDateTime(txtDate.Text));     //Check period closed out permission
                                bool Flags = (bool)ViewState["FlagPeriodClose"];

                                if (Flags)
                                {

                                    //AddCheckDetails();
                                    _objCD.ConnConfig = Session["config"].ToString();
                                    _objCD.Dt = GetBillItems();
                                    // _objCD.fDate = DateTime.Now;
                                    _objCD.fDate = Convert.ToDateTime(txtDate.Text);
                                    _objCD.NextC = long.Parse(txtNextCheck.Text);



                                    _objCD.fDesc = ddlVendor.SelectedItem.Text;
                                    _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);
                                    _objCD.Vendor = Convert.ToInt32(ddlVendor.SelectedValue);
                                    _objCD.Memo = txtMemo.Text;


                                    //API
                                    _addCheck.ConnConfig = Session["config"].ToString();
                                    _addCheck.Dt = GetBillItems();
                                    _addCheck.fDate = Convert.ToDateTime(txtDate.Text);
                                    _addCheck.NextC = long.Parse(txtNextCheck.Text);
                                    _addCheck.fDesc = ddlVendor.SelectedItem.Text;
                                    _addCheck.Bank = Convert.ToInt32(ddlBank.SelectedValue);
                                    _addCheck.Vendor = Convert.ToInt32(ddlVendor.SelectedValue);
                                    _addCheck.Memo = txtMemo.Text;

                                    if (!string.IsNullOrEmpty(hdnDiscGL.Value))
                                    {
                                        _objCD.DiscGL = Convert.ToInt32(hdnDiscGL.Value);
                                        _addCheck.DiscGL = Convert.ToInt32(hdnDiscGL.Value);
                                    }

                                    _objCD.Type = Convert.ToInt16(ddlPayment.SelectedValue);
                                    _objCD.MOMUSer = Session["Username"].ToString();

                                    _addCheck.Type = Convert.ToInt16(ddlPayment.SelectedValue);
                                    _addCheck.MOMUSer = Session["Username"].ToString();

                                
                                    _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                                    //if (IsAPIIntegrationEnable == "YES")
                                    if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                                    {
                                        string APINAME = "ManageChecksAPI/AddCheck";

                                        APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _addCheck);

                                        _objCD.ID = Convert.ToInt32(_APIResponse.ResponseData);
                                    }
                                    else
                                    {
                                        _objCD.ID = _objBLBill.AddCheck(_objCD);
                                    }

                                    #region Reset Page
                                    string check = txtNextCheck.Text;
                                    //ResetFormControlValues(this);
                                    //ResetForm();
                                    //btnSubmit.Visible = false;
                                    //btnCutCheck.Visible = true;

                                    string script = "function f(){$find(\"" + RadWindowTemplates.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                                    #endregion

                                    //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", " alert('Check Saved Successfully! </br> <b> Check# : " + check.ToString() + "</b>');", true);
                                    //Response.Write("<script>alert('Checks Saved Successfully!');</script>");



                                    //refreshVendorDDL();

                                }
                            }







                            GetPeriodDetails(Convert.ToDateTime(txtDate.Text));     //Check period closed out permission
                            bool Flag = (bool)ViewState["FlagPeriodClose"];
                            try
                            {
                                if (Flag)
                                {
                                    string script = "function f(){$find(\"" + RadWindowTemplates.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                                }
                                else
                                {
                                    Exception ex = new Exception();
                                    throw ex;

                                }
                            }
                            catch (Exception ex)
                            {
                                string str = "These month/year period is closed out. You do not have permission to process the check.";
                                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  noty({text: '" + str + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true , closeWith: ['click'], callback: { onShow: function() { }, afterShow: function() { }, onClose: function() { location.reload(); }, afterClose: function() { } }, buttons: false }); $('#MOMloading').hide();", true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string str = "These month/year period is closed out. You do not have permission to process the check.";
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  noty({text: '" + str + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true , closeWith: ['click'], callback: { onShow: function() { }, afterShow: function() { }, onClose: function() { location.reload(); }, afterClose: function() { } }, buttons: false }); $('#MOMloading').hide();", true);
            }

        }

    }

    protected void gvBills_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        gvBills.AllowCustomPaging = !ShouldApplySortFilterOrGroup();
        if (Session["dsBills"] != null)
        {
            DataTable dt = (DataTable)Session["dsBills"];
            gvBills.VirtualItemCount = dt.Rows.Count;
            gvBills.DataSource = dt;

            //if (ViewState["AutoCheckRun"] != null && Convert.ToString(ViewState["AutoCheckRun"]) == "True")
            //{
            //    //DataTable _dTable = new DataTable();
            //    //_dTable = dt.Copy();
            //    DataTable _dTable = dt.Clone();
            //    _dTable.Columns["payment"].DataType = typeof(Double);
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        _dTable.ImportRow(row);
            //    }
            //    for (int rowIndex = 0; rowIndex < _dTable.Rows.Count; rowIndex++)
            //    {
            //        _dTable.Rows[rowIndex]["payment"] = Convert.ToDouble(Convert.ToDouble(_dTable.Rows[rowIndex]["Original"]) - Convert.ToDouble(_dTable.Rows[rowIndex]["Selected"]) - Convert.ToDouble(_dTable.Rows[rowIndex]["Discount"]));
            //    }
            //    _dTable.AcceptChanges();
            //    gvBills.DataSource = _dTable;

            //    GridFooterItem footerItem = (GridFooterItem)gvBills.MasterTableView.GetItems(GridItemType.Footer)[0];
            //    Label lblTotalPay = (Label)footerItem.FindControl("lblTotalPay");
            //    Label lblTotalOrig = (Label)footerItem.FindControl("lblTotalOrig");
            //    Label lblTotalDisc = (Label)footerItem.FindControl("lblTotalDisc");
            //    Label lblTotalBalance = (Label)footerItem.FindControl("lblTotalBalance");


            //    lblTotalPay.Text = string.Format("{0:c}", Convert.ToDouble(_dTable.Compute("SUM(payment)", string.Empty)));
            //    lblTotalOrig.Text = string.Format("{0:c}", Convert.ToDouble(_dTable.Compute("SUM(Original)", string.Empty)));
            //    lblTotalDisc.Text = string.Format("{0:c}", Convert.ToDouble(_dTable.Compute("SUM(Discount)", string.Empty)));
            //    lblTotalBalance.Text = string.Format("{0:c}", Convert.ToDouble(_dTable.Compute("SUM(Balance)", string.Empty)));

            //}




        }
        //if (!IsGridPageIndexChanged)
        //{
        //    gvBills.CurrentPageIndex = 0;
        //    Session["gvBills_ProjectCurrentPageIndex"] = 0;
        //    ViewState["gvBills_ProjectminimumRows"] = 0;
        //    ViewState["gvBills_ProjectmaximumRows"] = gvBills.PageSize;
        //}
        //else
        //{
        //    if (Session["gvBills_ProjectCurrentPageIndex"] != null && Convert.ToInt32(Session["gvBills_ProjectCurrentPageIndex"].ToString()) != 0)
        //    {
        //        gvBills.CurrentPageIndex = Convert.ToInt32(Session["gvBills_ProjectCurrentPageIndex"].ToString());
        //        ViewState["RadGvTicketListminimumRows"] = gvBills.CurrentPageIndex * gvBills.PageSize;
        //        ViewState["RadGvTicketListmaximumRows"] = (gvBills.CurrentPageIndex + 1) * gvBills.PageSize;
                
        //    }
        //}
        

    }
    protected void gvBills_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
    {
        try
        {
            IsGridPageIndexChanged = true;
            ViewState["gvBills_ProjectminimumRows"] = gvBills.CurrentPageIndex * e.NewPageSize;
            ViewState["gvBills_ProjectmaximumRows"] = (gvBills.CurrentPageIndex + 1) * e.NewPageSize;
        }
        catch { }
    }
    protected void gvBills_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        //_take = radGridCompanies.PageSize; 
        //Label1.Text = "ny:" + radGridCompanies.PageSize.ToString(); 
        //radGridCompanies.PageSize = e.NewPageSize; 
        //_skip = e.NewPageIndex * radGridCompanies.PageSize; 
        //radGridCompanies.PageSize = radGridCompanies.PageSize; 

        //_skip = 0; 

        //radGridCompanies.CurrentPageIndex = 0;
        //radGridCompanies.DataSource = null; 
        IsGridPageIndexChanged = true;
        Session["gvBills_ProjectCurrentPageIndex"] = e.NewPageIndex;
        ViewState["gvBills_ProjectminimumRows"] = e.NewPageIndex * gvBills.PageSize;
        ViewState["gvBills_ProjectmaximumRows"] = (e.NewPageIndex + 1) * gvBills.PageSize;
        //gvBills.CurrentPageIndex = e.NewPageIndex;
        //DataTable dt = (DataTable)Session["dsBills"];
        //gvBills.DataSource = dt;
        //gvBills.Rebind();

        //gvBills.Rebind();
        //int ij = gvBills.Items.Count;
        //CheckAllCheckbox();


    }

    bool isGrouping = false;
    public bool ShouldApplySortFilterOrGroup()
    {
        return gvBills.MasterTableView.FilterExpression != "" ||
            (gvBills.MasterTableView.GroupByExpressions.Count > 0 || isGrouping) ||
            gvBills.MasterTableView.SortExpressions.Count > 0;
    }

    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            Session.Remove("selectedItems");
            ArrayList selectedItems = new ArrayList();
            if (Session["selectedItems"] == null)
            {
                selectedItems = new ArrayList();
            }
            else
            {
                selectedItems = (ArrayList)Session["selectedItems"];
            }
            foreach (GridDataItem row in gvBills.Items)
            {
                //try like this and see all update panels that need change der????? mathu???
                //CheckBox chkSelect = (CheckBox)sender;
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                //GridViewRow row = (GridViewRow)chkSelect.NamingContainer;
                TextBox txtGvPay = (TextBox)row.FindControl("txtGvPay");
                TextBox txtGvDisc = (TextBox)row.FindControl("txtGvDisc");

                if (chkSelect.Checked.Equals(true))
                {
                    Label lblBalance = (Label)row.FindControl("lblBalance");

                    double _pay = Convert.ToDouble(txtGvPay.Text);

                    double _dueBalance = double.Parse(lblBalance.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                      NumberStyles.AllowThousands |
                                      NumberStyles.AllowDecimalPoint);

                    if (_pay.Equals(0))
                    {
                        txtGvPay.Text = _dueBalance.ToString("0.00", CultureInfo.InvariantCulture);
                    }
                    HiddenField hdnPJID = (HiddenField)row.FindControl("hdnPJID");
                    selectedItems.Add(hdnPJID.Value);
                    Session["selectedItems"] = selectedItems;
                }
                else
                {
                    txtGvPay.Text = "0.00";
                    txtGvDisc.Text = "0.00";
                }
            }
            //   GetPaymentTotal();
            if (ViewState["ProcessPaymentInitiated"] != null && (Boolean.Parse(ViewState["ProcessPaymentInitiated"].ToString())))
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "display write check", "displayCheck();", true);
                btnCutCheck.Visible = false;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void RadAjaxManager_WC_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        var str = "";
    }

    protected void RadAjaxManager_WC_AjaxRequest1(object sender, AjaxRequestEventArgs e)
    {
        try
        {
            if (IsValidDate())
            {
                GetPeriodDetails(Convert.ToDateTime(txtDate.Text));     //Check period closed out permission
                bool Flag = (bool)ViewState["FlagPeriodClose"];

                if (Flag)
                {
                    if (ddlVendor.SelectedValue == "-1")
                    {
                        DataTable dt = (DataTable)Session["dsbills"];
                        DataTable dtNew = new DataTable();
                        dtNew.Columns.Add("Name");
                        dtNew.Columns.Add("Vendor");
                        foreach (DataRow drow in dt.Rows)
                        {
                            DataRow drNew = dtNew.NewRow();
                            drNew["Name"] = drow["Name"].ToString();
                            drNew["Vendor"] = drow["Vendor"].ToString();
                            dtNew.Rows.Add(drNew);
                        }
                        DataTable dtN = dtNew.DefaultView.ToTable(true);
                        int count = 0;
                        foreach (DataRow dr in dtN.Rows)
                        {
                            _objCD.ConnConfig = Session["config"].ToString();
                            _objCD.Dt = GetVendorBillItems(dr["Name"].ToString());

                            _addCheck.ConnConfig = Session["config"].ToString();
                            _addCheck.Dt = GetVendorBillItems(dr["Name"].ToString());


                            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                            //if (IsAPIIntegrationEnable == "YES")
                            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                            {
                                if (_addCheck.Dt.Rows.Count > 0)
                                {
                                    //_addCheck.fDate = DateTime.Now;
                                    _addCheck.fDate = Convert.ToDateTime(txtDate.Text);
                                    //if (ddlPayment.SelectedValue == "0")
                                    //{
                                    //    _addCheck.NextC = Convert.ToInt32(txtNextCheck.Text) + count;
                                    //}
                                    //else
                                    //{
                                    //    _addCheck.NextC = Convert.ToInt32(txtNextCheck.Text);
                                    //}

                                    _addCheck.NextC = long.Parse(txtNextCheck.Text) + count;
                                    
                                    _addCheck.fDesc = dr["Name"].ToString();
                                    _addCheck.Bank = Convert.ToInt32(ddlBank.SelectedValue);
                                    _addCheck.Vendor = Convert.ToInt32(dr["Vendor"].ToString());
                                    _addCheck.Memo = txtMemo.Text;
                                    if (!string.IsNullOrEmpty(hdnDiscGL.Value))
                                    {
                                        _addCheck.DiscGL = Convert.ToInt32(hdnDiscGL.Value);
                                    }

                                    _addCheck.Type = Convert.ToInt16(ddlPayment.SelectedValue);
                                    _addCheck.MOMUSer = Session["Username"].ToString();

                                   
                                     string APINAME = "ManageChecksAPI/AddCheck";

                                     APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _addCheck);

                                     _objCD.ID = Convert.ToInt32(_APIResponse.ResponseData);
                                   
                                    count++;
                                }
                            }
                            else
                            {
                                if (_objCD.Dt.Rows.Count > 0)
                                {
                                    //_objCD.fDate = DateTime.Now;
                                    _objCD.fDate = Convert.ToDateTime(txtDate.Text);
                                    //if (ddlPayment.SelectedValue == "0")
                                    //{
                                    //    _objCD.NextC = Convert.ToInt32(txtNextCheck.Text) + count;
                                    //}
                                    //else
                                    //{
                                    //    _objCD.NextC = Convert.ToInt32(txtNextCheck.Text);
                                    //}

                                    _objCD.NextC = long.Parse(txtNextCheck.Text) + count;
                                    

                                    _objCD.fDesc = dr["Name"].ToString();
                                    _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);
                                    _objCD.Vendor = Convert.ToInt32(dr["Vendor"].ToString());
                                    _objCD.Memo = txtMemo.Text;
                                    if (!string.IsNullOrEmpty(hdnDiscGL.Value))
                                    {
                                        _objCD.DiscGL = Convert.ToInt32(hdnDiscGL.Value);
                                    }

                                    _objCD.Type = Convert.ToInt16(ddlPayment.SelectedValue);
                                    _objCD.MOMUSer = Session["Username"].ToString();

                                    _objCD.ID = _objBLBill.AddCheck(_objCD);
                                   
                                    count++;
                                }
                            }

                        }
                        #region Reset Page
                        string check = txtNextCheck.Text;
                        ResetFormControlValues(this);
                        ResetForm();
                        btnSubmit.Visible = false;
                        btnCutCheck.Visible = true;
                        #endregion
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  noty({text: 'Checks saved successfully! </br> <b>', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});  window.setTimeout(function () { window.location.href = 'WriteChecks.aspx'; }, 500); ", true);
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  alert('Checks Saved Successfully! </br> <b>');", true);
                        //Response.Write("<script>alert('Checks Saved Successfully!');</script>");
                    }
                    else
                    {
                        //AddCheckDetails();
                        _objCD.ConnConfig = Session["config"].ToString();
                        _objCD.Dt = GetBillItems();
                        // _objCD.fDate = DateTime.Now;
                        _objCD.fDate = Convert.ToDateTime(txtDate.Text);
                        _objCD.NextC = long.Parse(txtNextCheck.Text);



                        _objCD.fDesc = ddlVendor.SelectedItem.Text;
                        _objCD.Bank = Convert.ToInt32(ddlBank.SelectedValue);
                        _objCD.Vendor = Convert.ToInt32(ddlVendor.SelectedValue);
                        _objCD.Memo = txtMemo.Text;

                        //API
                        _addCheck.ConnConfig = Session["config"].ToString();
                        _addCheck.Dt = GetBillItems();
                        _addCheck.fDate = Convert.ToDateTime(txtDate.Text);
                        _addCheck.NextC = long.Parse(txtNextCheck.Text);
                        _addCheck.fDesc = ddlVendor.SelectedItem.Text;
                        _addCheck.Bank = Convert.ToInt32(ddlBank.SelectedValue);
                        _addCheck.Vendor = Convert.ToInt32(ddlVendor.SelectedValue);
                        _addCheck.Memo = txtMemo.Text;

                        if (!string.IsNullOrEmpty(hdnDiscGL.Value))
                        {
                            _objCD.DiscGL = Convert.ToInt32(hdnDiscGL.Value);
                            _addCheck.DiscGL = Convert.ToInt32(hdnDiscGL.Value);
                        }

                        _objCD.Type = Convert.ToInt16(ddlPayment.SelectedValue);
                        _objCD.MOMUSer = Session["Username"].ToString();

                        _addCheck.Type = Convert.ToInt16(ddlPayment.SelectedValue);
                        _addCheck.MOMUSer = Session["Username"].ToString();


                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "ManageChecksAPI/AddCheck";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _addCheck);

                            _objCD.ID = Convert.ToInt32(_APIResponse.ResponseData);
                        }
                        else
                        {
                            _objCD.ID = _objBLBill.AddCheck(_objCD);
                        }

                        #region Reset Page
                        string check = txtNextCheck.Text;
                        ResetFormControlValues(this);
                        ResetForm();
                        btnSubmit.Visible = false;
                        btnCutCheck.Visible = true;

                        #endregion
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "  noty({text: 'Check saved successfully! </br> <b> Check# : " + check.ToString() + "</b>', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false}); window.setTimeout(function () { window.location.href = 'WriteChecks.aspx'; }, 500); ", true);
                        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", " alert('Check Saved Successfully! </br> <b> Check# : " + check.ToString() + "</b>');", true);
                        //Response.Write("<script>alert('Checks Saved Successfully!');</script>");
                    }


                    refreshVendorDDL();

                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            //ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningVendor", "  noty({text: '" + str + "', dismissQueue: true,  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false}); $('#MOMloading').hide();", true);
            
        }
    }

    protected void btnSelectChkBox_Click(object sender, EventArgs e)
    {
        GetPaymentRetainCheckbox();
        GetPaymentTotal();
        GetRunningBalance();
        if (ddlVendor.SelectedValue == "-1")
        {
            //string script = "function lblselectp(){$find(\"" + RadWindowAutomaticSelectionForPayment.ClientID + "\").show(); Sys.Application.remove_load(f);}Sys.Application.add_load(f); Materialize.updateTextFields();";
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "CallMyFunction", "lblselectp();", true);

        }
    }
    public void GetRunningBalance()
    {
        _objCD.ConnConfig = Session["config"].ToString();
        _getRunningBalanceCounts.ConnConfig = Session["config"].ToString();

        DataSet _dsRunBal = new DataSet();
        List <OpenAPViewModel> _lstOpenAPViewModel = new List<OpenAPViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/AddCheck_GetRunningBalanceCounts";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getRunningBalanceCounts);

            _lstOpenAPViewModel = (new JavaScriptSerializer()).Deserialize<List<OpenAPViewModel>>(_APIResponse.ResponseData);
            _dsRunBal = CommonMethods.ToDataSet<OpenAPViewModel>(_lstOpenAPViewModel);
        }
        else
        {
            _dsRunBal = _objBLBill.GetRunningBalanceCounts(_objCD);
        }

        lblRunBalance.Text = string.Format("{0:c}", Convert.ToDouble(_dsRunBal.Tables[0].Rows[0]["RunningBalance"]));
        //lblTotalVendorCount.Text = _dsRunBal.Tables[0].Rows[0]["Counts"].ToString();
        lblTotalVendorCount.Text = _dsRunBal.Tables[0].Rows[0]["TotVendor"].ToString();
        lblruntotbillcount.Text = _dsRunBal.Tables[0].Rows[0]["Counts"].ToString();
        lblSelectedVendorCount.Text = _dsRunBal.Tables[0].Rows[0]["TotVendor"].ToString();
        if (lblAutoSelectBalance.Visible == true)
        {
            lblAutoSelectBalance.Text = string.Format("{0:c}", Convert.ToDouble(_dsRunBal.Tables[0].Rows[0]["RunningBalance"]));
        }
        if (lblOpenItems.Visible == true)
        {
            lblOpenItems.Text = _dsRunBal.Tables[0].Rows[0]["Counts"].ToString();
        }
        if (lblVCountValue.Visible == true)
        {
            lblVCountValue.Text = _dsRunBal.Tables[0].Rows[0]["TotVendor"].ToString();
        }
        if (ddlVendor.SelectedValue == "-1")
        {
            
            lblSelectedPayment.Text = string.Format("{0:c}", Convert.ToDouble(_dsRunBal.Tables[0].Rows[0]["RunningBalance"]));
            
        }
        if (ddlVendor.SelectedValue == "0")
        {
            lblSelectedVendorCount.Text = "0";
            
        }
        if (ddlVendor.SelectedValue != "0" && ddlVendor.SelectedValue != "-1")
        {
            lblSelectedVendorCount.Text = "1";
        }

        


    }
    public void GetPaymentRetainCheckbox()
    {
        try
        {
            double DueBalance = 0;
            ArrayList selectedItems = new ArrayList();
            DataSet dsPJID = new DataSet();
            _objCD.ConnConfig = Session["config"].ToString();
            _getSelectedOpenAPPJID.ConnConfig = Session["config"].ToString();

            List<OpenAPViewModel> _lstOpenAPViewModel = new List<OpenAPViewModel>();

            _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
            //if (IsAPIIntegrationEnable == "YES")
            if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
            {
                string APINAME = "ManageChecksAPI/AddCheck_GetSelectedOpenAPPJID";

                APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getSelectedOpenAPPJID);

                _lstOpenAPViewModel = (new JavaScriptSerializer()).Deserialize<List<OpenAPViewModel>>(_APIResponse.ResponseData);
                dsPJID = CommonMethods.ToDataSet<OpenAPViewModel>(_lstOpenAPViewModel);
            }
            else
            {
                dsPJID = _objBLBill.GetSelectedOpenAPPJID(_objCD);
            }

            if (dsPJID.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsPJID.Tables[0].Rows.Count; i++)
                {
                    string strPJID = Convert.ToString(dsPJID.Tables[0].Rows[i]["PJID"]);
                    selectedItems.Add(strPJID);
                    Session["selectedItems"] = selectedItems;
                }
            }
            foreach (GridDataItem gr in gvBills.Items)
            {
                _objOpenAP.ConnConfig = Session["config"].ToString();
                _updateWriteCheckOpenAPpayment.ConnConfig = Session["config"].ToString();
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                HiddenField hdnPJID = (HiddenField)gr.FindControl("hdnPJID");
                HiddenField hdnRef = (HiddenField)gr.FindControl("hdnRef");
                TextBox txtGvDisc = (TextBox)gr.FindControl("txtGvDisc");
                Label lblBalance = (Label)gr.FindControl("lblBalance");
                HiddenField hdnPrevDue = (HiddenField)gr.FindControl("hdnPrevDue");
                HiddenField hdnSelected = (HiddenField)gr.FindControl("hdnSelected");
                HiddenField hdnOriginal = (HiddenField)gr.FindControl("hdnOriginal");
                if (chkSelect.Checked == true)
                {
                    selectedItems.Remove(hdnPJID.Value);
                    TextBox txtGvPay = (TextBox)gr.FindControl("txtGvPay");
                    if (!Convert.ToDouble(txtGvPay.Text).Equals(0))
                    {
                        //DueBalance = (Convert.ToDouble(hdnOriginal.Value) - Convert.ToDouble(hdnSelected.Value) - Convert.ToDouble(txtGvPay.Text));
                        DueBalance = (Convert.ToDouble(hdnOriginal.Value) - Convert.ToDouble(hdnSelected.Value) - Convert.ToDouble(txtGvPay.Text)-Convert.ToDouble(txtGvDisc.Text));
                        _objOpenAP.PJID = Convert.ToInt32(hdnPJID.Value);
                        _objOpenAP.Ref = Convert.ToString(hdnRef.Value);
                        _objOpenAP.Balance = DueBalance;//ConvertCurrentCurrencyFormatToDbl(hdnPrevDue.Value);
                        _objOpenAP.Disc = ConvertCurrentCurrencyFormatToDbl(txtGvDisc.Text);
                        _objOpenAP.IsSelected = 1;

                        _updateWriteCheckOpenAPpayment.PJID = Convert.ToInt32(hdnPJID.Value);
                        _updateWriteCheckOpenAPpayment.Ref = Convert.ToString(hdnRef.Value);
                        _updateWriteCheckOpenAPpayment.Balance = DueBalance;//ConvertCurrentCurrencyFormatToDbl(hdnPrevDue.Value);
                        _updateWriteCheckOpenAPpayment.Disc = ConvertCurrentCurrencyFormatToDbl(txtGvDisc.Text);
                        _updateWriteCheckOpenAPpayment.IsSelected = 1;

                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "ManageChecksAPI/AddCheck_UpdateWriteCheckOpenAPpayment";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateWriteCheckOpenAPpayment);
                        }
                        else
                        {
                            _objBLBill.UpdateWriteCheckOpenAPpayment(_objOpenAP);
                        }
                    }
                    DataTable dt = (DataTable)Session["dsBills"];
                    DataRow dr = dt.Select("PJID=" + hdnPJID.Value + "").FirstOrDefault();
                    if (dr != null)
                    {
                        dr["IsSelected"] = true;
                    }
                    dt.AcceptChanges();
                    Session["dsBills"] = dt;
                }
                else
                {
                    if (Session["selectedItems"] != null)
                    {
                        Int16 stackIndex;
                        for (stackIndex = 0; stackIndex <= selectedItems.Count - 1; stackIndex++)
                        {
                            string curItem = selectedItems[stackIndex].ToString();
                            if (curItem.Equals(hdnPJID.Value) & chkSelect.Checked == false)
                            {
                                selectedItems.Remove(hdnPJID.Value);

                                //DueBalance = (Convert.ToDouble(hdnOriginal.Value) - Convert.ToDouble(hdnSelected.Value));
                                DueBalance = (Convert.ToDouble(hdnOriginal.Value) - Convert.ToDouble(hdnSelected.Value)- Convert.ToDouble(txtGvDisc.Text));
                                _objOpenAP.PJID = Convert.ToInt32(hdnPJID.Value);
                                _objOpenAP.Ref = Convert.ToString(hdnRef.Value);
                                _objOpenAP.Balance = DueBalance;//ConvertCurrentCurrencyFormatToDbl(lblBalance.Text);
                                _objOpenAP.Disc = ConvertCurrentCurrencyFormatToDbl(txtGvDisc.Text);
                                _objOpenAP.IsSelected = 0;

                                _updateWriteCheckOpenAPpayment.PJID = Convert.ToInt32(hdnPJID.Value);
                                _updateWriteCheckOpenAPpayment.Ref = Convert.ToString(hdnRef.Value);
                                _updateWriteCheckOpenAPpayment.Balance = DueBalance;//ConvertCurrentCurrencyFormatToDbl(lblBalance.Text);
                                _updateWriteCheckOpenAPpayment.Disc = ConvertCurrentCurrencyFormatToDbl(txtGvDisc.Text);
                                _updateWriteCheckOpenAPpayment.IsSelected = 0;

                                _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                                //if (IsAPIIntegrationEnable == "YES")
                                if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                                {
                                    string APINAME = "ManageChecksAPI/AddCheck_UpdateWriteCheckOpenAPpayment";

                                    APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _updateWriteCheckOpenAPpayment);
                                }
                                else
                                {
                                    _objBLBill.UpdateWriteCheckOpenAPpayment(_objOpenAP);
                                }

                                DataTable dt = (DataTable)Session["dsBills"];
                                DataRow dr = dt.Select("PJID="+ hdnPJID.Value + "").FirstOrDefault(); 
                                if (dr != null)
                                {
                                    dr["IsSelected"] = false; 
                                }
                                dt.AcceptChanges();
                                Session["dsBills"] = dt;

                            }
                        }
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
    private double ConvertCurrentCurrencyFormatToDbl(string strCurrency)
    {
        if (!string.IsNullOrEmpty(strCurrency))
        {
            var dblReturn = double.Parse(strCurrency.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                        NumberStyles.AllowThousands |
                                                        NumberStyles.AllowDecimalPoint | NumberStyles.AllowTrailingSign |
                                                        NumberStyles.Float);
            return dblReturn;
        }
        else
        {
            return 0;
        }
    }
    protected void gvBills_ItemCreated(object sender, GridItemEventArgs e)
    {
        try
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
        catch { }
    }
    private void FillFrequency()
    {
        try
        {
            List<Frequency> _lstFrequency = new List<Frequency>();
            _lstFrequency = FrequencyHelper.GetAll();
            ddlFrequency.DataSource = _lstFrequency;
            ddlFrequency.DataValueField = "ID";
            ddlFrequency.DataTextField = "Name";
            ddlFrequency.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void SetTax()
    {

        _objPropGeneral.ConnConfig = Session["config"].ToString();
        _getCustomFieldsControl.ConnConfig = Session["config"].ToString();

        DataSet _dsCustom = new DataSet();
        List<CustomViewModel> _lstCustomFieldsControl = new List<CustomViewModel>();

        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
        //if (IsAPIIntegrationEnable == "YES")
        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
        {
            string APINAME = "ManageChecksAPI/AddCheck_GetCustomFieldsControl";

            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getCustomFieldsControl);

            _lstCustomFieldsControl = (new JavaScriptSerializer()).Deserialize<List<CustomViewModel>>(_APIResponse.ResponseData);
            _dsCustom = CommonMethods.ToDataSet<CustomViewModel>(_lstCustomFieldsControl);
        }
        else
        {
            _dsCustom = _objBLGeneral.getCustomFieldsControl(_objPropGeneral);
        }

        if (_dsCustom.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow _dr in _dsCustom.Tables[0].Rows)
            {


                if (_dr["Name"].ToString().Equals("GSTGL"))
                {
                    if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
                    {
                        _objChart.ConnConfig = Session["config"].ToString();
                        _objChart.ID = Convert.ToInt32(_dr["Label"].ToString());

                        _getChart.ConnConfig = Session["config"].ToString();
                        _getChart.ID = Convert.ToInt32(_dr["Label"].ToString());

                        DataSet _dsChart = new DataSet();
                        List<ChartViewModel> _lstChartViewModel = new List<ChartViewModel>();

                        _objAPIIntegration = (APIIntegrationModel)Session["IsAPIIntegration"];
                        //if (IsAPIIntegrationEnable == "YES")
                        if (_objAPIIntegration.IsAPIIntegrationForAPModule == true)
                        {
                            string APINAME = "ManageChecksAPI/AddCheck_GetChart";

                            APIResponse _APIResponse = new MOMWebUtility().CallMOMWebAPI(APINAME, _getChart);

                            _lstChartViewModel = (new JavaScriptSerializer()).Deserialize<List<ChartViewModel>>(_APIResponse.ResponseData);
                            _dsChart = CommonMethods.ToDataSet<ChartViewModel>(_lstChartViewModel);
                        }
                        else
                        {
                            _dsChart = _objBLChart.GetChart(_objChart);
                        }


                        if (_dsChart.Tables[0].Rows.Count > 0)
                        {
                            //txtGSTGL.Text = _dsChart.Tables[0].Rows[0]["fDesc"].ToString();
                            hdnGSTGL.Value = _dr["Label"].ToString();
                        }

                    }
                    else
                    {
                        hdnGSTGL.Value = "0";
                    }
                }
                else if (_dr["Name"].ToString().Equals("GSTRate"))
                {
                    if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
                    {
                        txtgst.Text = _dr["Label"].ToString();
                        hdnGST.Value = _dr["Label"].ToString();
                    }
                    else
                    {
                        txtgst.Text = "0";
                        hdnGST.Value = "0";
                    }
                }

            }
        }
        if (txtgst.Text.Trim() == "")
        {
            txtgst.Text = "0";
        }
        if (hdnGST.Value.Trim() == "")
        {
            hdnGST.Value = "0";
        }

        if (txtgstgv.Visible == true)
        {
            if (hdnGST.Value.Trim() == "0" || hdnGST.Value.Trim() == "0.0000")
            {
                txtgstgv.Visible = false;
                RadGrid_gvJobCostItems.Columns[12].Visible = false;
                RadGrid_gvJobCostItems.Columns[11].Visible = false;
            }
        }
        //_objPropGeneral.ConnConfig = Session["config"].ToString();
        //_objPropGeneral.CustomName = "Country";
        //DataSet dsCustom = _objBLGeneral.getCustomFields(_objPropGeneral);

        //if (dsCustom.Tables[0].Rows.Count > 0)
        //{
        //    if (!string.IsNullOrEmpty(dsCustom.Tables[0].Rows[0]["Label"].ToString()) && dsCustom.Tables[0].Rows[0]["Label"].ToString().Equals("1"))
        //    {
        //        txtgstgv.Visible = true;
        //        //hdnGstTax.Value = dsCustom.Tables[0].Rows[0]["GstRate"].ToString();
        //    }
        //    else
        //    {
        //        txtgstgv.Visible = false;
        //    }
        //}
        //else
        //{
        //    txtgstgv.Visible = false;
        //}


    }
}
