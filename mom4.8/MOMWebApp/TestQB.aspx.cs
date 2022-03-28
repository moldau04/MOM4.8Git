using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QBFC12Lib;
//using QBXMLRP2ELib;
using System.Data;
using BusinessLayer;
using BusinessEntity;

public partial class TestQB : System.Web.UI.Page
{
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
       // //User[] objInvoice = null;
       ////getCustomerDetails();
       // //QBFC_AddInvoice();
       // addCustomer();
    }

    public void getCustomerDetails()
    {
        //array of user class objects
        //User[] invInvoice = null;


        IMsgSetRequest requestMsgSet;
        IMsgSetResponse responseMsgSet;

        // Create the session manager object using QBFC
        QBSessionManager sessionManager = null;

        bool booSessionBegun = false;

        //try
        //{
        sessionManager = new QBSessionManager();

        sessionManager.CommunicateOutOfProcess(true);
        sessionManager.QBAuthPreferences.PutIsReadOnly(false);
        sessionManager.QBAuthPreferences.PutUnattendedModePref(ENUnattendedModePrefType.umptRequired);
        sessionManager.QBAuthPreferences.PutPersonalDataPref(ENPersonalDataPrefType.pdptRequired);

        sessionManager.OpenConnection2("", "Invoice", ENConnectionType.ctLocalQBDLaunchUI);
        //sessionManager.OpenConnection("", "Invoice");

        //sessionManager.BeginSession("C:\\Users\\Public\\Documents\\Intuit\\QuickBooks\\Company Files\\Ideavate.QBW", ENOpenMode.omDontCare);
        //sessionManager.BeginSession("C:\\QuickBooksDataBase\\QB Company.QBW", ENOpenMode.omDontCare);
        //sessionManager.BeginSession("C:\\Users\\Public\\Documents\\Intuit\\QuickBooks\\Sample Company Files\\QuickBooks 2010\\sample_company_file.qbw", ENOpenMode.omDontCare);
        sessionManager.BeginSession("C:\\Users\\Public\\Documents\\Intuit\\QuickBooks\\Sample Company Files\\QuickBooks 2007\\sample_product-based business.qbw", ENOpenMode.omDontCare);


        if (sessionManager.QBAuthPreferences.WasAuthPreferencesObeyed() != true)
        {
            throw new Exception("Auth Not Obeyed!!");
        }
        booSessionBegun = true;

        // Get the RequestMsgSet based on the correct QB Version
        requestMsgSet = getLatestMsgSetRequest(sessionManager);
        // requestMsgSet = sessionManager.CreateMsgSetRequest("US", 4, 0);

        // Initialize the message set request object
        requestMsgSet.Attributes.OnError = ENRqOnError.roeStop;


        // Add the request to the message set request object
        IInvoiceQuery invoiceAdd = requestMsgSet.AppendInvoiceQueryRq();

        //IInvoiceFilter invFilter = invoiceAdd.ORCustomerListQuery.CustomerListFilter;

        //if (strFacilityName != "" && strFacilityName != null)
        //{
        //    invFilter.EntityFilter.OREntityFilter.FullNameWithChildren.SetValue(strFacilityName);
        //}

        //if (strInvoiceNo != "" && strInvoiceNo != null)
        //{
        //    invFilter.ORRefNumberFilter.RefNumberFilter.RefNumber.SetValue(strInvoiceNo);
        //    invFilter.ORRefNumberFilter.RefNumberFilter.MatchCriterion.SetValue(ENMatchCriterion.mcContains);
        //}


        //if (strStatus != "" && strStatus != null)
        //{
        //    if (strStatus == "Paid")
        //    {
        //        invFilter.PaidStatus.SetValue(ENPaidStatus.psPaidOnly);
        //    }
        //    if (strStatus == "Open")
        //    {
        //        invFilter.PaidStatus.SetValue(ENPaidStatus.psAll);
        //    }
        //    if (strStatus == "UnPaid")
        //    {
        //        invFilter.PaidStatus.SetValue(ENPaidStatus.psNotPaidOnly);
        //    }
        //}
        //if (dtStartDate.ToString() != "" && dtStartDate.ToString() != "1/1/0001 12:00:00 AM")
        //{
        //    invFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.TxnDateFilter.FromTxnDate.SetValue(dtStartDate);
        //}
        //if (dtEndDate.ToString() != "" && dtEndDate.ToString() != "1/1/0001 12:00:00 AM")
        //{
        //    invFilter.ORDateRangeFilter.TxnDateRangeFilter.ORTxnDateRangeFilter.TxnDateFilter.ToTxnDate.SetValue(dtEndDate);
        //}

        //invoiceAdd.IncludeLineItems.SetValue(true);

        responseMsgSet = sessionManager.DoRequests(requestMsgSet);
        //IResponse responceInvoice = responseMsgSet.ResponseList.GetAt(0);

        IResponse response = responseMsgSet.ResponseList.GetAt(0);
        // int statusCode = response.StatusCode;

        ICustomerRetList invoiceRet = response.Detail as ICustomerRetList;
        if (invoiceRet != null)
        {
            if (!(invoiceRet.Count == 0))
            {
                int rowcount = invoiceRet.Count;
                //invInvoice = new User[rowcount];
                int fCount = 0;
                for (int ndx = 0; ndx < rowcount; ndx++)
                {
                    //User objtemInvoice = new User();
                    ICustomerRet invoiceRet1 = invoiceRet.GetAt(ndx);

                    //if (invoiceRet1.CompanyName != null && invoiceRet1.CompanyName.ToString() != "")
                    //    objtemInvoice.Locationname = invoiceRet1.CompanyName.GetValue();

                    //if (invoiceRet1.BillAddress != null)
                    //{
                    //    if (invoiceRet1.BillAddress.Addr1 != null)
                    //        objtemInvoice.Address = invoiceRet1.BillAddress.Addr1.GetValue();                           
                    //    if (invoiceRet1.BillAddress.City != null)
                    //        objtemInvoice.City = invoiceRet1.BillAddress.City.GetValue();
                    //    if (invoiceRet1.BillAddress.State != null)
                    //        objtemInvoice.State = invoiceRet1.BillAddress.State.GetValue();
                    //    if (invoiceRet1.BillAddress.Country != null)
                    //        objtemInvoice.Country = invoiceRet1.BillAddress.Country.GetValue();
                    //    if (invoiceRet1.BillAddress.PostalCode != null)
                    //        objtemInvoice.Zip = invoiceRet1.BillAddress.PostalCode.GetValue();
                    //}

                    //invInvoice[fCount] = objtemInvoice;

                    string stgu = "";

                    if (invoiceRet1.ListID != null && invoiceRet1.ListID.ToString() != "")
                        stgu = invoiceRet1.ListID.GetValue();

                    if (invoiceRet1.FullName != null && invoiceRet1.FullName.ToString() != "")
                    {
                        objProp_User.FirstName = invoiceRet1.FullName.GetValue();
                    }
                    else
                    {
                        objProp_User.FirstName = string.Empty;
                    }

                    if (invoiceRet1.Notes != null && invoiceRet1.Notes.ToString() != "")
                        objProp_User.Remarks = invoiceRet1.Notes.GetValue();
                    else
                        objProp_User.Remarks = string.Empty;

                    if (invoiceRet1.Contact != null && invoiceRet1.Contact.ToString() != "")
                        objProp_User.MainContact = invoiceRet1.Contact.GetValue();

                    if (invoiceRet1.Phone != null && invoiceRet1.Phone.ToString() != "")
                        objProp_User.Phone = invoiceRet1.Phone.GetValue();
                    else
                        objProp_User.Phone = string .Empty;

                    if (invoiceRet1.Email != null && invoiceRet1.Email.ToString() != "")
                        objProp_User.Email = invoiceRet1.Email.GetValue();
                    else
                        objProp_User.Email = string .Empty;

                    if (invoiceRet1.Mobile != null && invoiceRet1.Mobile.ToString() != "")
                        objProp_User.Cell = invoiceRet1.Mobile.GetValue();
                    else
                        objProp_User.Cell = string.Empty;

                    if (invoiceRet1.ListID != null && invoiceRet1.ListID.ToString() != "")
                        objProp_User.QBCustomerID = invoiceRet1.ListID.GetValue();
                    else
                        objProp_User.QBCustomerID = string.Empty;

                    if (invoiceRet1.BillAddress != null)
                    {
                        if (invoiceRet1.BillAddress.Addr1 != null && invoiceRet1.BillAddress.Addr1.ToString() != "")
                            objProp_User.Address = invoiceRet1.BillAddress.Addr1.GetValue();
                            if (invoiceRet1.BillAddress.Addr2 != null && invoiceRet1.BillAddress.Addr2.ToString() != "")
                                objProp_User.Address = invoiceRet1.BillAddress.Addr1.GetValue() +" "+ invoiceRet1.BillAddress.Addr2.GetValue();
                        else
                            objProp_User.Address = string.Empty;

                        if (invoiceRet1.BillAddress.City != null && invoiceRet1.BillAddress.City.ToString() != "")
                            objProp_User.City = invoiceRet1.BillAddress.City.GetValue();
                        else
                            objProp_User.City = string.Empty;

                        if (invoiceRet1.BillAddress.State != null && invoiceRet1.BillAddress.State.ToString() != "")
                            objProp_User.State = invoiceRet1.BillAddress.State.GetValue();
                        else
                            objProp_User.State = string.Empty;

                        if (invoiceRet1.BillAddress.PostalCode != null && invoiceRet1.BillAddress.PostalCode.ToString() != "")
                            objProp_User.Zip = invoiceRet1.BillAddress.PostalCode.GetValue();
                        else
                            objProp_User.Zip = string.Empty;
                    }

                    objProp_User.Username = "";
                    objProp_User.Password = "";
                    objProp_User.Website = "";
                    objProp_User.Type = "General";
                    objProp_User.Schedule = 0;
                    objProp_User.Mapping = 0;
                    objProp_User.Internet = 0;
                    //objProp_User.ContactData = null;
                    objProp_User.ConnConfig = Session["config"].ToString();
                    
                    objBL_User.AddCustomerQB(objProp_User);

                    fCount++;
                }
            }
        }
        //else
        //{
        //    invInvoice = null;
        //}
        sessionManager.EndSession();
        booSessionBegun = false;
        sessionManager.CloseConnection();
        //}
        //catch (Exception ex)
        //{
        //    if (booSessionBegun)
        //    {
        //        sessionManager.EndSession();
        //        sessionManager.CloseConnection();
        //    }
        //    throw new Exception(ex.Message.ToString() + "\nStack Trace: \n" + ex.StackTrace + "\nExiting the application");
        //}
        //return object of Invoice class
        //return invInvoice;
    }

    public IMsgSetRequest getLatestMsgSetRequest(QBSessionManager sessionManager)
    {
        // Find and adapt to supported version of QuickBooks
        double supportedVersion = QBFCLatestVersion(sessionManager);

        short qbXMLMajorVer = 0;
        short qbXMLMinorVer = 0;

        if (supportedVersion >= 6.0)
        {
            qbXMLMajorVer = 6;
            qbXMLMinorVer = 0;
        }
        else if (supportedVersion >= 5.0)
        {
            qbXMLMajorVer = 5;
            qbXMLMinorVer = 0;
        }
        else if (supportedVersion >= 4.0)
        {
            qbXMLMajorVer = 4;
            qbXMLMinorVer = 0;
        }
        else if (supportedVersion >= 3.0)
        {
            qbXMLMajorVer = 3;
            qbXMLMinorVer = 0;
        }
        else if (supportedVersion >= 2.0)
        {
            qbXMLMajorVer = 2;
            qbXMLMinorVer = 0;
        }
        else if (supportedVersion >= 1.1)
        {
            qbXMLMajorVer = 1;
            qbXMLMinorVer = 1;
        }
        else
        {
            qbXMLMajorVer = 1;
            qbXMLMinorVer = 0;
            //Response.Write("It seems that you are running QuickBooks 2002 Release 1. We strongly recommend that you use QuickBooks' online update feature to obtain the latest fixes and enhancements");
        }

        // Create the message set request object
        IMsgSetRequest requestMsgSet = sessionManager.CreateMsgSetRequest("US", qbXMLMajorVer, qbXMLMinorVer);
        return requestMsgSet;
    }

    private double QBFCLatestVersion(QBSessionManager SessionManager)
    {
        // Use oldest version to ensure that this application work with any QuickBooks (US)
        IMsgSetRequest msgset = SessionManager.CreateMsgSetRequest("US", 1, 0);
        msgset.AppendHostQueryRq();
        IMsgSetResponse QueryResponse = SessionManager.DoRequests(msgset);


        IResponse response = QueryResponse.ResponseList.GetAt(0);

        // Please refer to QBFC Developers Guide for details on why 
        // "as" clause was used to link this derrived class to its base class
        IHostRet HostResponse = response.Detail as IHostRet;
        IBSTRList supportedVersions = HostResponse.SupportedQBXMLVersionList as IBSTRList;

        int i;
        double vers;
        double LastVers = 0;
        string svers = null;

        for (i = 0; i <= supportedVersions.Count - 1; i++)
        {
            svers = supportedVersions.GetAt(i);
            vers = Convert.ToDouble(svers);
            if (vers > LastVers)
            {
                LastVers = vers;
            }
        }
        return LastVers;
    }
    
    //public User[] getJobDetails()
    //{
    //    User[] invInvoice = null;

    //    IMsgSetRequest requestMsgSet;
    //    IMsgSetResponse responseMsgSet;

    //    QBSessionManager sessionManager = null;

    //    bool booSessionBegun = false;

    //    sessionManager = new QBSessionManager();

    //    sessionManager.CommunicateOutOfProcess(true);
    //    sessionManager.QBAuthPreferences.PutIsReadOnly(false);
    //    sessionManager.QBAuthPreferences.PutUnattendedModePref(ENUnattendedModePrefType.umptRequired);
    //    sessionManager.QBAuthPreferences.PutPersonalDataPref(ENPersonalDataPrefType.pdptRequired);

    //    sessionManager.OpenConnection2("", "Invoice", ENConnectionType.ctLocalQBDLaunchUI);
    //    sessionManager.BeginSession("C:\\Users\\Public\\Documents\\Intuit\\QuickBooks\\Sample Company Files\\QuickBooks 2007\\sample_product-based business.qbw", ENOpenMode.omDontCare);

    //    if (sessionManager.QBAuthPreferences.WasAuthPreferencesObeyed() != true)
    //    {
    //        throw new Exception("Auth Not Obeyed!!");
    //    }
    //    booSessionBegun = true;

    //    requestMsgSet = getLatestMsgSetRequest(sessionManager);

    //    requestMsgSet.Attributes.OnError = ENRqOnError.roeStop;

    //    IJobTypeQuery invoiceAdd = requestMsgSet.AppendJobTypeQueryRq() ;

    //    IJobTypeRetList invFilter = invoiceAdd.ORListQuery..CustomerListFilter;

    //    responseMsgSet = sessionManager.DoRequests(requestMsgSet);

    //    IResponse response = responseMsgSet.ResponseList.GetAt(0);

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
       
    //    return invInvoice;
    //}

    public void QBFC_AddInvoice()
    {
      IMsgSetRequest	requestMsgSet;
			IMsgSetResponse responseMsgSet;
			// Create the session manager object using QBFC
			QBSessionManager sessionManager = new QBSessionManager();

			// We want to know if we begun a session so we can end it if an
			// error happens
			bool booSessionBegun=false;

            sessionManager.CommunicateOutOfProcess(true);
            sessionManager.QBAuthPreferences.PutIsReadOnly(false);
            sessionManager.QBAuthPreferences.PutUnattendedModePref(ENUnattendedModePrefType.umptRequired);
            sessionManager.QBAuthPreferences.PutPersonalDataPref(ENPersonalDataPrefType.pdptRequired);
                // Use SessionManager object to open a connection and begin a session 
                // with QuickBooks. At this time, you should add interop.QBFCxLib into 
                // your Project References
            sessionManager.OpenConnection2("", "Invoice", ENConnectionType.ctLocalQBD);

                //sessionManager.OpenConnection("", "IDN InvoiceAdd C# sample");
                //sessionManager.BeginSession("", ENOpenMode.omDontCare);
                sessionManager.BeginSession("C:\\Users\\Public\\Documents\\Intuit\\QuickBooks\\Sample Company Files\\QuickBooks 2007\\sample_product-based business.qbw", ENOpenMode.omDontCare);

                booSessionBegun = true;

                // Get the RequestMsgSet based on the correct QB Version
                requestMsgSet = getLatestMsgSetRequest(sessionManager);
                // requestMsgSet = sessionManager.CreateMsgSetRequest("US", 4, 0);

                // Initialize the message set request object
                requestMsgSet.Attributes.OnError = ENRqOnError.roeContinue;

                // ERROR RECOVERY: 
                // All steps are described in QBFC Developers Guide, on pg 41
                // under section titled "Automated Error Recovery"

                // (1) Set the error recovery ID using ErrorRecoveryID function
                //		Value must be in GUID format
                //	You could use c:\Program Files\Microsoft Visual Studio\Common\Tools\GuidGen.exe 
                //	to create a GUID for your unique ID
                string errecid = "{E74068B5-0D6D-454d-B0FD-BDDF93CE67C3}";
                sessionManager.ErrorRecoveryID.SetValue(errecid);

                // (2) Set EnableErrorRecovery to true to enable error recovery
                sessionManager.EnableErrorRecovery = true;

                // (3) Set SaveAllMsgSetRequestInfo to true so the entire contents of the MsgSetRequest
                //		will be saved to disk. If SaveAllMsgSetRequestInfo is false (default), only the 
                //		newMessageSetID will be saved. 
                sessionManager.SaveAllMsgSetRequestInfo = true;

                // (4) Use IsErrorRecoveryInfo to check whether an unprocessed response exists. 
                //		If IsErrorRecoveryInfo is true:
                if (sessionManager.IsErrorRecoveryInfo())
                {
                    //string reqXML;
                    //string resXML;
                    IMsgSetRequest reqMsgSet = null;
                    IMsgSetResponse resMsgSet = null;

                    // a. Get the response status, using GetErrorRecoveryStatus
                    resMsgSet = sessionManager.GetErrorRecoveryStatus();
                    // resXML = resMsgSet.ToXMLString();
                    // MessageBox.Show(resXML);

                    if (resMsgSet.Attributes.MessageSetStatusCode.Equals("600"))
                    {
                        // This case may occur when a transaction has failed after QB processed 
                        // the request but client app didn't get the response and started with 
                        // another company file.
                        //MessageBox.Show("The oldMessageSetID does not match any stored IDs, and no newMessageSetID is provided.");
                    }
                    else if (resMsgSet.Attributes.MessageSetStatusCode.Equals("9001"))
                    {
                        Response.Write("Invalid checksum. The newMessageSetID specified, matches the currently stored ID, but checksum fails.");
                    }
                    else if (resMsgSet.Attributes.MessageSetStatusCode.Equals("9002"))
                    {
                        // Response was not successfully stored or stored properly
                        Response.Write("No stored response was found.");
                    }
                    // 9003 = Not used
                    else if (resMsgSet.Attributes.MessageSetStatusCode.Equals("9004"))
                    {
                        // MessageSetID is set with a string of size > 24 char
                        Response.Write("Invalid MessageSetID, greater than 24 character was given.");
                    }
                    else if (resMsgSet.Attributes.MessageSetStatusCode.Equals("9005"))
                    {
                        Response.Write("Unable to store response.");
                    }
                    else
                    {
                        IResponse res = resMsgSet.ResponseList.GetAt(0);
                        int sCode = res.StatusCode;
                        string sMessage = res.StatusMessage;
                        string sSeverity = res.StatusSeverity;
                        Response.Write("StatusCode = " + sCode + "\n" + "StatusMessage = " + sMessage + "\n" + "StatusSeverity = " + sSeverity);

                        if (sCode == 0)
                        {
                            Response.Write("Last request was processed and Invoice was added successfully!");
                        }
                        else if (sCode > 0)
                        {
                            Response.Write("There was a warning but last request was processed successfully!");
                        }
                        else
                        {
                            Response.Write("It seems that there was an error in processing last request"); 
                            // b. Get the saved request, using GetSavedMsgSetRequest
                            reqMsgSet = sessionManager.GetSavedMsgSetRequest();
                            //reqXML = reqMsgSet.ToXMLString();
                            //MessageBox.Show(reqXML);

                            // c. Process the response, possibly using the saved request
                            resMsgSet = sessionManager.DoRequests(reqMsgSet);
                            IResponse resp = resMsgSet.ResponseList.GetAt(0);
                            int statCode = resp.StatusCode;
                            if (statCode == 0)
                            {
                                string resStr = null;
                                ICustomerRet invRet = resp.Detail as ICustomerRet;
                                resStr = resStr + "Following invoice has been successfully submitted to QuickBooks:\n\n\n";
                                //if (invRet.TxnNumber != null)
                                //    resStr = resStr + "Txn Number = " + Convert.ToString(invRet.TxnNumber.GetValue()) + "\n";
                            } // if (statusCode == 0)
                        } // else (sCode)
                    } // else (MessageSetStatusCode)

                    // d. Clear the response status, using ClearErrorRecovery
                    sessionManager.ClearErrorRecovery();
                    //MessageBox.Show("Proceeding with current transaction.");
                }


                ICustomerAdd invoiceAdd = requestMsgSet.AppendCustomerAddRq();

                string customer = "12342353";
                if (!customer.Equals(""))
                {
                    invoiceAdd.Name.SetValue(customer);
                }
                // Invoice Date
                string invoiceDate = "134541";
                if (!invoiceDate.Equals(""))
                {
                    invoiceAdd.Notes.SetValue(invoiceDate);
                }
                // Invoice Number
                string invoiceNumber = "345134";
                if (!invoiceNumber.Equals(""))
                {
                    invoiceAdd.Contact.SetValue(invoiceNumber);
                }
                // Bill Address
                string bAddr1 = "345";
                string bAddr2 = "345";
                string bAddr3 = "3145";
                string bAddr4 = "345";
                string bCity = "345";
                string bState = "315";
                string bPostal = "345";
                string bCountry = "345";
                invoiceAdd.BillAddress.Addr1.SetValue(bAddr1);
                invoiceAdd.BillAddress.Addr2.SetValue(bAddr2);
                invoiceAdd.BillAddress.Addr3.SetValue(bAddr3);
                invoiceAdd.BillAddress.Addr4.SetValue(bAddr4);
                invoiceAdd.BillAddress.City.SetValue(bCity);
                invoiceAdd.BillAddress.State.SetValue(bState);
                invoiceAdd.BillAddress.PostalCode.SetValue(bPostal);
                invoiceAdd.BillAddress.Country.SetValue(bCountry);

                // Close the session and connection with QuickBooks
                sessionManager.ClearErrorRecovery();
                sessionManager.EndSession();
                booSessionBegun = false;
                sessionManager.CloseConnection();
            }
         


    //public void addCustomer()
    //{      
    //    QBSessionManager aSession=null;
    //    IMsgSetRequest requests ;
    //    IMsgSetResponse responses;
    //    bool connected = false;
                      
    //    aSession = new QBSessionManager();       
                                              
    //    aSession.OpenConnection2("", "ADDCUST", ENConnectionType.ctLocalQBD);
    //    aSession.BeginSession("C:\\Users\\Public\\Documents\\Intuit\\QuickBooks\\Sample Company Files\\QuickBooks 2007\\sample_product-based business.qbw", ENOpenMode.omDontCare);
    //    connected = true;
    //    requests = getLatestMsgSetRequest(aSession);
    //    requests.Attributes.OnError = ENRqOnError.roeStop;

    //    objProp_User.ConnConfig = Session["config"].ToString();
    //    DataSet ds = new DataSet();
    //    ds = objBL_User.getMSMCustomers(objProp_User);

    //    foreach (DataRow dr in ds.Tables[0].Rows)
    //    {         
    //        requests.ClearRequests();
    //        IInvoiceAdd customerReq = requests.AppendInvoiceAddRq();
    //        //customerReq.CompanyName.SetValue(dr["name"].ToString());
    //        //customerReq.Name.SetValue(dr["name"].ToString());
    //        //customerReq.Notes.SetValue(dr["remarks"].ToString());
    //        //customerReq.Contact.SetValue(dr["contact"].ToString());
    //        //customerReq.Phone.SetValue(dr["phone"].ToString());
    //        //customerReq.Email.SetValue(dr["email"].ToString());
    //        //customerReq.Fax.SetValue(dr["fax"].ToString());
    //        customerReq.BillAddress.Addr2.SetValue(dr["address"].ToString());
    //        customerReq.BillAddress.City.SetValue(dr["city"].ToString());
    //        customerReq.BillAddress.State.SetValue(dr["state"].ToString());
    //        customerReq.BillAddress.PostalCode.SetValue(dr["zip"].ToString());

    //        responses = aSession.DoRequests(requests);
    //        IResponse thisResponse = responses.ResponseList.GetAt(0);
    //        if (thisResponse.StatusCode == 0)
    //        {
    //            IInvoiceRet customer = (IInvoiceRet)thisResponse.Detail;

    //            objProp_User.ConnConfig = Session["config"].ToString();
    //            objProp_User.QBCustomerID = customer.ListID.GetValue();
    //            objProp_User.CustomerID = Convert.ToInt32(dr["id"]);
    //            //objBL_User.UpdateQBCustomerID(objProp_User);
    //        }
    //    }

    //    DataSet dsQB = new DataSet();
    //    dsQB = objBL_User.getQBCustomers(objProp_User);
    //    foreach (DataRow dr in dsQB.Tables[0].Rows)
    //    {
    //        requests.ClearRequests();
    //        IInvoiceQuery CustQ = requests.AppendInvoiceQueryRq();
    //        CustQ.ORInvoiceQuery.ListIDList.Add(dr["QBCustomerID"].ToString());

    //        responses = aSession.DoRequests(requests);

    //        IResponse thisResponse = responses.ResponseList.GetAt(0);
    //        ICustomerRetList customerRetList = thisResponse.Detail as ICustomerRetList;
    //        ICustomerRet customerRet = customerRetList.GetAt(0);

    //        string editSequence = customerRet.EditSequence.GetValue();

    //        requests.ClearRequests();
    //        ICustomerMod CustMod = requests.AppendCustomerModRq();
    //        CustMod.ListID.SetValue(dr["QBCustomerID"].ToString());
    //        CustMod.EditSequence.SetValue(editSequence);
    //        CustMod.CompanyName.SetValue(dr["name"].ToString());
    //        CustMod.Name.SetValue(dr["name"].ToString());
    //        CustMod.Notes.SetValue(dr["remarks"].ToString());
    //        CustMod.Contact.SetValue(dr["contact"].ToString());
    //        CustMod.Phone.SetValue(dr["phone"].ToString());
    //        CustMod.Email.SetValue(dr["email"].ToString());
    //        CustMod.Fax.SetValue(dr["fax"].ToString());
    //        CustMod.BillAddress.Addr2.SetValue(dr["address"].ToString());
    //        CustMod.BillAddress.City.SetValue(dr["city"].ToString());
    //        CustMod.BillAddress.State.SetValue(dr["state"].ToString());
    //        CustMod.BillAddress.PostalCode.SetValue(dr["zip"].ToString());

    //        responses = aSession.DoRequests(requests);
    //        IResponse thisResponse1 = responses.ResponseList.GetAt(0);
    //    }                   

    //    aSession.EndSession();
    //    connected = false;
    //    aSession.CloseConnection();              
    //}			
}
