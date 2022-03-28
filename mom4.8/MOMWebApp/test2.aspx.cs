using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLayer;
using BusinessEntity;
using System.Management;
using QBFC12Lib;
using System.Web.Services;
using System.Web.Script.Services;
using MobilePushNotification;
using System.Web.Configuration;
using MicrosoftTranslatorSdk.SoapSamples;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Net.Mail;
using System.Globalization;
using System.IO;
using System.Threading;
//using QBXMLRP2Lib;
//using FDTEST;
//using System.Security.Cryptography.X509Certificates;
//using System.Net;

public partial class test2 : System.Web.UI.Page
{
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //string username = "paya3928";
            //string password = "Test2013";

            ////PaymentGateway.Response resp = new PaymentGateway.Response();
            ////PaymentGateway.WS_GlobalPayments sdxx = new PaymentGateway.WS_GlobalPayments();
            ////resp= sdxx.GetInfo(username, password, "Initialize","");

            //PaymentGateway.WS_TrxDetail sdx = new PaymentGateway.WS_TrxDetail();
            //string str = sdx.GetCardTrx(username, password, "206318", "", "04/11/2013", "04/13/2013", "", "", "", "", "", "", "", "", "", "", "", "FALSE", "", "", "", "", "", "", "", "", "", "", "");
            //Response.Write(str);
            //Send();

            ////DateTime date = DateTime.ParseExact("yyyy-MM-dd", "04/17/2013 05:48 a.m.", CultureInfo.InvariantCulture); 
            //string dat = "04/17/2013 05:48 a.m.";
            ////Response.Write(dat.Replace(".", string.Empty));
            //DateTime date = Convert.ToDateTime(dat.Replace(".", string.Empty));
            ////DateTime date = Convert.ToDateTime("2013-01-03 15:48:31");
            //Response.Write(date.ToString());

        }
    }

    //private void CreateRecurring()
    //{
    //    string MerchantUsername = string.Empty;
    //    string MerchantPassword = string.Empty;
    //    string CertPath = string.Empty;
    //    string URL = string.Empty;

    //    MerchantUsername = System.Web.Configuration.WebConfigurationManager.AppSettings["PaymentUser"].Trim();
    //    MerchantPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["PaymentPassword"].Trim();
    //    CertPath = System.Web.Configuration.WebConfigurationManager.AppSettings["FDGGCertPath"].Trim();
    //    URL = System.Web.Configuration.WebConfigurationManager.AppSettings["FDGGURL"].Trim();

    //    System.Net.ServicePointManager.Expect100Continue = false;
    //    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;

    //    CreditCardTxType oCreditCardTxType = new CreditCardTxType();
    //    oCreditCardTxType.Type = CreditCardTxTypeType.sale;
    //    CreditCardData oCreditCardData = new CreditCardData();
    //    oCreditCardData.ItemsElementName = new ItemsChoiceType[] { ItemsChoiceType.CardNumber, ItemsChoiceType.ExpMonth, ItemsChoiceType.ExpYear, ItemsChoiceType.CardCodeValue };
    //    oCreditCardData.Items = new string[] { "4012000033330026", "12", "15", "123" };

    //    Payment oPayment = new Payment();
    //    decimal amtdc = Convert.ToDecimal(ViewState["amt"].ToString().Trim());
    //    oPayment.ChargeTotal = Math.Round(amtdc, 2);

    //    TransactionDetails oTransactionDetails = new TransactionDetails();
    //    oTransactionDetails.InvoiceNumber = ViewState["uid"].ToString().Replace(',', ':');
    //    oTransactionDetails.Recurring = Recurring.No;
    //    oTransactionDetails.TransactionOrigin = TransactionDetailsTransactionOrigin.ECI;

    //    RecurringPayment rp = new RecurringPayment();
    //    rp.RecurringPaymentInformation = new RecurringPaymentInformation() { RecurringStartDate = "20161231" };
    //    //oTransaction.TransactionDataType =

    //    Transaction oTransaction = new Transaction();
    //    oTransaction.Items = new object[] { oCreditCardTxType, oCreditCardData };
    //    oTransaction.Payment = oPayment;
    //    oTransaction.TransactionDetails = oTransactionDetails;
        
    //    FDGGWSApiOrderRequest oOrderRequest = new FDGGWSApiOrderRequest();
    //    oOrderRequest.Item = oTransaction;
    //    FDGGWSApiOrderResponse oResponse = null;
    //    FDGGWSApiOrderService oFDGGWSApiOrderService = new FDGGWSApiOrderService()
    //    {
    //        Url = URL,
    //        Credentials = new NetworkCredential(MerchantUsername, MerchantPassword)
    //    };
    //    oFDGGWSApiOrderService.ClientCertificates.Add(X509Certificate.CreateFromCertFile(CertPath));

    //    oResponse = oFDGGWSApiOrderService.FDGGWSApiOrder(oOrderRequest);
    //    string sApprovalCode = oResponse.TransactionResult;
    //}

    //public Invoice[] getInvoiceDetails(string strFacilityName, string strInvoiceNo, string strStatus, DateTime dtStartDate, DateTime dtEndDate)
    //{
    //    //array of user class objects
    //    Invoice[] invInvoice = null;


    //    IMsgSetRequest requestMsgSet;
    //    IMsgSetResponse responseMsgSet;

    //    // Create the session manager object using QBFC
    //    QBSessionManager sessionManager = null;

    //    bool booSessionBegun = false;

    //    try
    //    {
    //        sessionManager = new QBSessionManager();

    //        sessionManager.CommunicateOutOfProcess(true);
    //        sessionManager.QBAuthPreferences.PutIsReadOnly(false);
    //        sessionManager.QBAuthPreferences.PutUnattendedModePref(ENUnattendedModePrefType.umptRequired);
    //        sessionManager.QBAuthPreferences.PutPersonalDataPref(ENPersonalDataPrefType.pdptRequired);

    //        sessionManager.OpenConnection2("", "Invoice", ENConnectionType.ctLocalQBDLaunchUI);
    //        //sessionManager.OpenConnection("", "Invoice");

    //        //sessionManager.BeginSession("C:\\Users\\Public\\Documents\\Intuit\\QuickBooks\\Company Files\\Ideavate.QBW", ENOpenMode.omDontCare);
    //        //sessionManager.BeginSession("C:\\QuickBooksDataBase\\QB Company.QBW", ENOpenMode.omDontCare);
    //        //sessionManager.BeginSession("C:\\Users\\Public\\Documents\\Intuit\\QuickBooks\\Sample Company Files\\QuickBooks 2010\\sample_company_file.qbw", ENOpenMode.omDontCare);
    //        sessionManager.BeginSession("C:\\Users\\Public\\Documents\\Intuit\\QuickBooks\\Sample Company Files\\QuickBooks 2007\\sample_product-based business.qbw", ENOpenMode.omDontCare);


    //        if (sessionManager.QBAuthPreferences.WasAuthPreferencesObeyed() != true)
    //        {
    //            throw new Exception("Auth Not Obeyed!!");
    //        }
    //        booSessionBegun = true;

    //        // Get the RequestMsgSet based on the correct QB Version
    //        requestMsgSet = getLatestMsgSetRequest(sessionManager);
    //        // requestMsgSet = sessionManager.CreateMsgSetRequest("US", 4, 0);

    //        // Initialize the message set request object
    //        requestMsgSet.Attributes.OnError = ENRqOnError.roeStop;


    //        // Add the request to the message set request object
    //        IInvoiceQuery invoiceAdd = requestMsgSet.AppendInvoiceQueryRq();

    //        IInvoiceFilter invFilter = invoiceAdd.ORInvoiceQuery.InvoiceFilter;

    //        if (strFacilityName != "" && strFacilityName != null)
    //        {
    //            invFilter.EntityFilter.OREntityFilter.FullNameWithChildren.SetValue(strFacilityName);
    //        }

    //        if (strInvoiceNo != "" && strInvoiceNo != null)
    //        {
    //            invFilter.ORRefNumberFilter.RefNumberFilter.RefNumber.SetValue(strInvoiceNo);
    //            invFilter.ORRefNumberFilter.RefNumberFilter.MatchCriterion.SetValue(ENMatchCriterion.mcContains);
    //        }


    //        if (strStatus != "" && strStatus != null)
    //        {
    //            if (strStatus == "Paid")
    //            {
    //                invFilter.PaidStatus.SetValue(ENPaidStatus.psPaidOnly);
    //            }
    //            if (strStatus == "Open")
    //            {
    //                invFilter.PaidStatus.SetValue(ENPaidStatus.psAll);
    //            }
    //            if (strStatus == "UnPaid")
    //            {
    //                invFilter.PaidStatus.SetValue(ENPaidStatus.psNotPaidOnly);
    //            }
    //        }
    //        if (dtStartDate.ToString() != "" && dtStartDate.ToString() != "1/1/0001 12:00:00 AM")
    //        {
    //            invFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.TxnDateFilter.FromTxnDate.SetValue(dtStartDate);
    //        }
    //        if (dtEndDate.ToString() != "" && dtEndDate.ToString() != "1/1/0001 12:00:00 AM")
    //        {
    //            invFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.TxnDateFilter.ToTxnDate.SetValue(dtEndDate);
    //        }

    //        invoiceAdd.IncludeLineItems.SetValue(true);

    //        responseMsgSet = sessionManager.DoRequests(requestMsgSet);
    //        //IResponse responceInvoice = responseMsgSet.ResponseList.GetAt(0);

    //        IResponse response = responseMsgSet.ResponseList.GetAt(0);
    //        // int statusCode = response.StatusCode;

    //        IInvoiceRetList invoiceRet = response.Detail as IInvoiceRetList;
    //        if (invoiceRet != null)
    //        {
    //            if (!(invoiceRet.Count == 0))
    //            {
    //                int rowcount = invoiceRet.Count;
    //                invInvoice = new Invoice[rowcount];
    //                int fCount = 0;
    //                for (int ndx = 0; ndx < rowcount; ndx++)
    //                {
    //                    Invoice objtemInvoice = new Invoice();
    //                    IInvoiceRet invoiceRet1 = invoiceRet.GetAt(ndx);
    //                    if (invoiceRet1.RefNumber != null && invoiceRet1.RefNumber.ToString() != "")
    //                        objtemInvoice.InvoiceNumber = invoiceRet1.RefNumber.GetValue();

    //                    if (invoiceRet1.CustomerRef != null)
    //                    {
    //                        if (invoiceRet1.CustomerRef.FullName != null)
    //                            objtemInvoice.CustomerName = invoiceRet1.CustomerRef.FullName.GetValue();
    //                    }
    //                    if (invoiceRet1.TxnDate != null)
    //                        objtemInvoice.InvoiceDate = invoiceRet1.TxnDate.GetValue().ToString("MM/dd/yyyy");
    //                    if (invoiceRet1.BillAddress != null)
    //                    {
    //                        if (invoiceRet1.BillAddress.Addr1 != null)
    //                            objtemInvoice.Address1 = invoiceRet1.BillAddress.Addr1.GetValue();
    //                        if (invoiceRet1.BillAddress.Addr2 != null)
    //                            objtemInvoice.Address2 = invoiceRet1.BillAddress.Addr2.GetValue();
    //                        if (invoiceRet1.BillAddress.City != null)
    //                            objtemInvoice.City = invoiceRet1.BillAddress.City.GetValue();
    //                        if (invoiceRet1.BillAddress.State != null)
    //                            objtemInvoice.State = invoiceRet1.BillAddress.State.GetValue();
    //                        if (invoiceRet1.BillAddress.Country != null)
    //                            objtemInvoice.Country = invoiceRet1.BillAddress.Country.GetValue();
    //                        if (invoiceRet1.BillAddress.PostalCode != null)
    //                            objtemInvoice.PostalCode = invoiceRet1.BillAddress.PostalCode.GetValue();
    //                    }
    //                    if (invoiceRet1.TermsRef != null)
    //                    {
    //                        if (invoiceRet1.TermsRef.FullName != null)
    //                            objtemInvoice.Terms = invoiceRet1.TermsRef.FullName.GetValue();
    //                    }
    //                    if (invoiceRet1.DueDate != null)
    //                        objtemInvoice.DueDate = invoiceRet1.DueDate.GetValue().ToString("MM/dd/yyyy");


    //                    double _taxtotal = 0;
    //                    double _subtotal = 0;
    //                    if (invoiceRet1.Subtotal != null)
    //                    {
    //                        _subtotal = Convert.ToDouble(invoiceRet1.Subtotal.GetValue());
    //                    }
    //                    if (invoiceRet1.SalesTaxTotal != null)
    //                    {
    //                        _taxtotal = Convert.ToDouble(invoiceRet1.SalesTaxTotal.GetValue());
    //                    }

    //                    double _total = _taxtotal + _subtotal;
    //                    objtemInvoice.SubTotal = _total.ToString("0.00");


    //                    if (!(invoiceRet1.IsPaid.IsEmpty()))
    //                    {
    //                        if (invoiceRet1.IsPaid.GetValue())
    //                        {
    //                            objtemInvoice.PaidStatus = "Paid";
    //                        }
    //                        else
    //                        {
    //                            objtemInvoice.PaidStatus = "UnPaid";
    //                        }
    //                    }

    //                    invInvoice[fCount] = objtemInvoice;
    //                    fCount++;

    //                }

    //            }
    //        }
    //        else
    //        {
    //            invInvoice = null;
    //        }
    //        sessionManager.EndSession();
    //        booSessionBegun = false;
    //        sessionManager.CloseConnection();
    //    }
    //    catch (Exception ex)
    //    {
    //        if (booSessionBegun)
    //        {
    //            sessionManager.EndSession();
    //            sessionManager.CloseConnection();
    //        }
    //        throw new Exception(ex.Message.ToString() + "\nStack Trace: \n" + ex.StackTrace + "\nExiting the application");
    //    }
    //    //return object of Invoice class
    //    return invInvoice;
    //}

    //public IMsgSetRequest getLatestMsgSetRequest(QBSessionManager sessionManager)
    //{
    //    // Find and adapt to supported version of QuickBooks
    //    double supportedVersion = QBFCLatestVersion(sessionManager);

    //    short qbXMLMajorVer = 0;
    //    short qbXMLMinorVer = 0;

    //    if (supportedVersion >= 6.0)
    //    {
    //        qbXMLMajorVer = 6;
    //        qbXMLMinorVer = 0;
    //    }
    //    else if (supportedVersion >= 5.0)
    //    {
    //        qbXMLMajorVer = 5;
    //        qbXMLMinorVer = 0;
    //    }
    //    else if (supportedVersion >= 4.0)
    //    {
    //        qbXMLMajorVer = 4;
    //        qbXMLMinorVer = 0;
    //    }
    //    else if (supportedVersion >= 3.0)
    //    {
    //        qbXMLMajorVer = 3;
    //        qbXMLMinorVer = 0;
    //    }
    //    else if (supportedVersion >= 2.0)
    //    {
    //        qbXMLMajorVer = 2;
    //        qbXMLMinorVer = 0;
    //    }
    //    else if (supportedVersion >= 1.1)
    //    {
    //        qbXMLMajorVer = 1;
    //        qbXMLMinorVer = 1;
    //    }
    //    else
    //    {
    //        qbXMLMajorVer = 1;
    //        qbXMLMinorVer = 0;
    //        //Response.Write("It seems that you are running QuickBooks 2002 Release 1. We strongly recommend that you use QuickBooks' online update feature to obtain the latest fixes and enhancements");
    //    }

    //    // Create the message set request object
    //    IMsgSetRequest requestMsgSet = sessionManager.CreateMsgSetRequest("US", qbXMLMajorVer, qbXMLMinorVer);
    //    return requestMsgSet;
    //}

    //private double QBFCLatestVersion(QBSessionManager SessionManager)
    //{
    //    // Use oldest version to ensure that this application work with any QuickBooks (US)
    //    IMsgSetRequest msgset = SessionManager.CreateMsgSetRequest("US", 1, 0);
    //    msgset.AppendHostQueryRq();
    //    IMsgSetResponse QueryResponse = SessionManager.DoRequests(msgset);


    //    IResponse response = QueryResponse.ResponseList.GetAt(0);

    //    // Please refer to QBFC Developers Guide for details on why 
    //    // "as" clause was used to link this derrived class to its base class
    //    IHostRet HostResponse = response.Detail as IHostRet;
    //    IBSTRList supportedVersions = HostResponse.SupportedQBXMLVersionList as IBSTRList;

    //    int i;
    //    double vers;
    //    double LastVers = 0;
    //    string svers = null;

    //    for (i = 0; i <= supportedVersions.Count - 1; i++)
    //    {
    //        svers = supportedVersions.GetAt(i);
    //        vers = Convert.ToDouble(svers);
    //        if (vers > LastVers)
    //        {
    //            LastVers = vers;
    //        }
    //    }
    //    return LastVers;
    //}

    //public User[] getCustomerDetails()
    //{
    //    //array of user class objects
    //    User[] invInvoice = null;


    //    IMsgSetRequest requestMsgSet;
    //    IMsgSetResponse responseMsgSet;

    //    // Create the session manager object using QBFC
    //    QBSessionManager sessionManager = null;

    //    bool booSessionBegun = false;

    //    //try
    //    //{
    //    sessionManager = new QBSessionManager();

    //    sessionManager.CommunicateOutOfProcess(true);
    //    sessionManager.QBAuthPreferences.PutIsReadOnly(false);
    //    sessionManager.QBAuthPreferences.PutUnattendedModePref(ENUnattendedModePrefType.umptRequired);
    //    sessionManager.QBAuthPreferences.PutPersonalDataPref(ENPersonalDataPrefType.pdptRequired);

    //    sessionManager.OpenConnection2("", "Invoice", ENConnectionType.ctLocalQBDLaunchUI);
    //    //sessionManager.OpenConnection("", "Invoice");

    //    //sessionManager.BeginSession("C:\\Users\\Public\\Documents\\Intuit\\QuickBooks\\Company Files\\Ideavate.QBW", ENOpenMode.omDontCare);
    //    //sessionManager.BeginSession("C:\\QuickBooksDataBase\\QB Company.QBW", ENOpenMode.omDontCare);
    //    //sessionManager.BeginSession("C:\\Users\\Public\\Documents\\Intuit\\QuickBooks\\Sample Company Files\\QuickBooks 2010\\sample_company_file.qbw", ENOpenMode.omDontCare);
    //    sessionManager.BeginSession("C:\\Users\\Public\\Documents\\Intuit\\QuickBooks\\Sample Company Files\\QuickBooks 2007\\sample_product-based business.qbw", ENOpenMode.omDontCare);


    //    if (sessionManager.QBAuthPreferences.WasAuthPreferencesObeyed() != true)
    //    {
    //        throw new Exception("Auth Not Obeyed!!");
    //    }
    //    booSessionBegun = true;

    //    // Get the RequestMsgSet based on the correct QB Version
    //    requestMsgSet = getLatestMsgSetRequest(sessionManager);
    //    // requestMsgSet = sessionManager.CreateMsgSetRequest("US", 4, 0);

    //    // Initialize the message set request object
    //    requestMsgSet.Attributes.OnError = ENRqOnError.roeStop;


    //    // Add the request to the message set request object
    //    ICustomerQuery invoiceAdd = requestMsgSet.AppendCustomerQueryRq();

    //    ICustomerListFilter invFilter = invoiceAdd.ORCustomerListQuery.CustomerListFilter;

    //    //if (strFacilityName != "" && strFacilityName != null)
    //    //{
    //    //    invFilter.EntityFilter.OREntityFilter.FullNameWithChildren.SetValue(strFacilityName);
    //    //}

    //    //if (strInvoiceNo != "" && strInvoiceNo != null)
    //    //{
    //    //    invFilter.ORRefNumberFilter.RefNumberFilter.RefNumber.SetValue(strInvoiceNo);
    //    //    invFilter.ORRefNumberFilter.RefNumberFilter.MatchCriterion.SetValue(ENMatchCriterion.mcContains);
    //    //}


    //    //if (strStatus != "" && strStatus != null)
    //    //{
    //    //    if (strStatus == "Paid")
    //    //    {
    //    //        invFilter.PaidStatus.SetValue(ENPaidStatus.psPaidOnly);
    //    //    }
    //    //    if (strStatus == "Open")
    //    //    {
    //    //        invFilter.PaidStatus.SetValue(ENPaidStatus.psAll);
    //    //    }
    //    //    if (strStatus == "UnPaid")
    //    //    {
    //    //        invFilter.PaidStatus.SetValue(ENPaidStatus.psNotPaidOnly);
    //    //    }
    //    //}
    //    //if (dtStartDate.ToString() != "" && dtStartDate.ToString() != "1/1/0001 12:00:00 AM")
    //    //{
    //    //    invFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.TxnDateFilter.FromTxnDate.SetValue(dtStartDate);
    //    //}
    //    //if (dtEndDate.ToString() != "" && dtEndDate.ToString() != "1/1/0001 12:00:00 AM")
    //    //{
    //    //    invFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.TxnDateFilter.ToTxnDate.SetValue(dtEndDate);
    //    //}

    //    //invoiceAdd.IncludeLineItems.SetValue(true);

    //    responseMsgSet = sessionManager.DoRequests(requestMsgSet);
    //    //IResponse responceInvoice = responseMsgSet.ResponseList.GetAt(0);

    //    IResponse response = responseMsgSet.ResponseList.GetAt(0);
    //    // int statusCode = response.StatusCode;

    //    ICustomerRetList invoiceRet = response.Detail as ICustomerRetList;
    //    if (invoiceRet != null)
    //    {
    //        if (!(invoiceRet.Count == 0))
    //        {
    //            int rowcount = invoiceRet.Count;
    //            invInvoice = new User[rowcount];
    //            int fCount = 0;
    //            for (int ndx = 0; ndx < rowcount; ndx++)
    //            {
    //                User objtemInvoice = new User();
    //                ICustomerRet invoiceRet1 = invoiceRet.GetAt(ndx);

    //                //if (invoiceRet1.CompanyName != null && invoiceRet1.CompanyName.ToString() != "")
    //                //    objtemInvoice.Locationname = invoiceRet1.CompanyName.GetValue();

    //                //if (invoiceRet1.BillAddress != null)
    //                //{
    //                //    if (invoiceRet1.BillAddress.Addr1 != null)
    //                //        objtemInvoice.Address = invoiceRet1.BillAddress.Addr1.GetValue();                           
    //                //    if (invoiceRet1.BillAddress.City != null)
    //                //        objtemInvoice.City = invoiceRet1.BillAddress.City.GetValue();
    //                //    if (invoiceRet1.BillAddress.State != null)
    //                //        objtemInvoice.State = invoiceRet1.BillAddress.State.GetValue();
    //                //    if (invoiceRet1.BillAddress.Country != null)
    //                //        objtemInvoice.Country = invoiceRet1.BillAddress.Country.GetValue();
    //                //    if (invoiceRet1.BillAddress.PostalCode != null)
    //                //        objtemInvoice.Zip = invoiceRet1.BillAddress.PostalCode.GetValue();
    //                //}

    //                //invInvoice[fCount] = objtemInvoice;

    //                string stgu = "";

    //                if (invoiceRet1.ListID != null && invoiceRet1.ListID.ToString() != "")
    //                    stgu = invoiceRet1.ListID.GetValue();

    //                if (invoiceRet1.CompanyName != null && invoiceRet1.CompanyName.ToString() != "")
    //                    objProp_User.FirstName = invoiceRet1.CompanyName.GetValue();

    //                if (invoiceRet1.Notes != null && invoiceRet1.Notes.ToString() != "")
    //                    objProp_User.Remarks = invoiceRet1.Notes.GetValue();

    //                if (invoiceRet1.Contact != null && invoiceRet1.Contact.ToString() != "")
    //                    objProp_User.MainContact = invoiceRet1.Contact.GetValue();

    //                if (invoiceRet1.Phone != null && invoiceRet1.Phone.ToString() != "")
    //                    objProp_User.Phone = invoiceRet1.Phone.GetValue();

    //                if (invoiceRet1.Email != null && invoiceRet1.Email.ToString() != "")
    //                    objProp_User.Email = invoiceRet1.Email.GetValue();

    //                if (invoiceRet1.Mobile != null && invoiceRet1.Mobile.ToString() != "")
    //                    objProp_User.Cell = invoiceRet1.Mobile.GetValue();

    //                if (invoiceRet1.BillAddress != null)
    //                {
    //                    if (invoiceRet1.BillAddress.Addr1 != null && invoiceRet1.BillAddress.Addr1.ToString() != "")
    //                        objProp_User.Address = invoiceRet1.BillAddress.Addr1.GetValue();

    //                    if (invoiceRet1.BillAddress.City != null && invoiceRet1.BillAddress.City.ToString() != "")
    //                        objProp_User.City = invoiceRet1.BillAddress.City.GetValue();

    //                    if (invoiceRet1.BillAddress.State != null && invoiceRet1.BillAddress.State.ToString() != "")
    //                        objProp_User.State = invoiceRet1.BillAddress.State.GetValue();

    //                    if (invoiceRet1.BillAddress.PostalCode != null && invoiceRet1.BillAddress.PostalCode.ToString() != "")
    //                        objProp_User.Zip = invoiceRet1.BillAddress.PostalCode.GetValue();
    //                }

    //                objProp_User.Username = "";
    //                objProp_User.Password = "";
    //                objProp_User.Website = "";
    //                objProp_User.Type = "General";
    //                objProp_User.Schedule = 0;
    //                objProp_User.Mapping = 0;
    //                objProp_User.Internet = 0;
    //                objProp_User.ContactData = null;
    //                //objProp_User.ConnConfig = Session["config"].ToString();

    //                //objBL_User.AddCustomer(objProp_User);

    //                fCount++;

    //            }
    //        }
    //    }
    //    else
    //    {
    //        invInvoice = null;
    //    }
    //    sessionManager.EndSession();
    //    booSessionBegun = false;
    //    sessionManager.CloseConnection();
    //    //}
    //    //catch (Exception ex)
    //    //{
    //    //    if (booSessionBegun)
    //    //    {
    //    //        sessionManager.EndSession();
    //    //        sessionManager.CloseConnection();
    //    //    }
    //    //    throw new Exception(ex.Message.ToString() + "\nStack Trace: \n" + ex.StackTrace + "\nExiting the application");
    //    //}
    //    //return object of Invoice class
    //    return invInvoice;
    //}

    public void Send()
    {
        var client = new SmtpClient
        {
            ////Host="Localhost",
            //Host = "smtp.1and1.com",
            ////Host = "smtp.gmail.com",
            EnableSsl = true
            //Port = 25
        };

        string From = "kunal.panchal@ideavate.com";
        var message = new MailMessage
        {
            Sender = new MailAddress(From, From),
            From = new MailAddress(From, From)
        };
        message.To.Add(From);
        message.Subject = Title;
        message.Body = "";
        message.IsBodyHtml = true;
        message.Priority = MailPriority.High;

        client.Send(message);
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        //log("test");
        mail("oo", "anandm1012@gmail.com");

    }
    private void log(String message)
    {
        DateTime datetime = DateTime.Now;
        string savepath = "/logs/";
        String oFileName = savepath + "MOM_" + datetime.ToString("dd_MM_yyyy") + ".log";

        if (!Directory.Exists(savepath))
        {
            Directory.CreateDirectory(savepath);
        }
        if (!File.Exists(oFileName))
        {
            System.IO.FileStream f = File.Create(oFileName);
            f.Close();
        }

        try
        {
            System.IO.StreamWriter writter = File.AppendText(oFileName);
            writter.WriteLine(datetime.ToString("MM-dd hh:mm") + " > " + message);
            writter.Flush();
            writter.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message.ToString());
        }
    }
    private void mail(string message, string to)
    {
        Thread email = new Thread(delegate()
        {
            if (to.Trim() != string.Empty)
            {
                try
                {
                    Mail mail = new Mail();
                    mail.From = "mom";
                    mail.To = to.Split(';', ',').OfType<string>().ToList();
                    mail.Title = "Payment Received.";
                    mail.Text = "132";
                    mail.RequireAutentication = true;
                    mail.Send();
                }
                catch (Exception ex)
                {
                    log(ex.Message);
                }
            }
        });
        email.IsBackground = true;
        email.Start();
    }
}
